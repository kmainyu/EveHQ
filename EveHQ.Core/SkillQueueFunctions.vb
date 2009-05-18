' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
' 
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================
Imports System.Drawing
Imports System.Windows.Forms

Public Class SkillQueueFunctions

    Public Shared Event RefreshQueue()

    Shared Property StartQueueRefresh() As Boolean
        Get
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent RefreshQueue()
            End If
        End Set
    End Property

    Public Shared Function BuildQueue(ByVal qPilot As EveHQ.Core.Pilot, ByVal qQueue As EveHQ.Core.SkillQueue, Optional ByVal QuickBuild As Boolean = False) As ArrayList
        Dim trainingBonus As Double = 2
        Dim currentBonus As Double = 1
        Dim trainingBonusLimit As Double = 1600000

        Dim bQueue As EveHQ.Core.SkillQueue = CType(qQueue.Clone, SkillQueue)

        Dim arrQueue As ArrayList = New ArrayList
        Dim totalTime As Long = 0
        Dim totalSkills As Integer = 0
        Dim totalSP As Long = qPilot.SkillPoints

        ' Prep a new font ready for completed training queues
        Dim doneFont As Font = New Font("MS Sans Serif", 8, FontStyle.Strikeout)

        ' Try Queue Building
        Try
            ' Check for partially trained skills
            Call CheckAlreadyTrained(qPilot, bQueue)
            ' Check the current skill being trained
            Call CheckAlreadyTraining(qPilot, bQueue)
            ' Check the training queue for missing prereqs
            If QuickBuild = False Then Call CheckSkillFlow(qPilot, bQueue)
            ' Check if all the pre-reqs are present and add them if not
            Call CheckPreReqs(qPilot, bQueue)
            ' Check the order of the pre-requisites
            Call CheckReqOrder(qPilot, bQueue)
            ' Check the skill order of the existing skills
            If QuickBuild = False Then Call CheckSkillOrder(qPilot, bQueue)
            ' Check if we need to covertly delete skills!
            ' Deletes completed skills if appropriate
            If EveHQ.Core.HQ.EveHQSettings.DeleteSkills = True Then
                EveHQ.Core.SkillQueueFunctions.RemoveTrainedSkills(qPilot, bQueue)
            End If
        Catch ex As Exception
            MessageBox.Show("Error occurs in Queue Building", "BuildQueue Error")
            Return Nothing
            Exit Function
        End Try

        ' Declare variables for skill attribute modifications
        Dim attModifiers(5) As Integer      ' Order is as follows i.e. C,I,M,P,W,L

        ' Add in the currently training skill if applicable
        Dim endtime As Date = Now
        Try
            If qPilot.Training = True And bQueue.IncCurrentTraining = True Then
                'Dim mypos As Integer = 0
                Dim mySkill As EveHQ.Core.EveSkill = CType(EveHQ.Core.HQ.SkillListID(qPilot.TrainingSkillID), EveHQ.Core.EveSkill)
                Dim clevel As Integer = qPilot.TrainingSkillLevel
                Dim cTime As Double = qPilot.TrainingCurrentTime
                Dim strTime As String = EveHQ.Core.SkillFunctions.TimeToString(cTime)
                endtime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(qPilot.TrainingEndTime)

                Dim curLevel As Integer = 0
                Dim percent As Integer = 0
                Dim myCurSkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(mySkill.Name), EveHQ.Core.PilotSkill)
                curLevel = myCurSkill.Level
                percent = CInt((myCurSkill.SP + qPilot.TrainingCurrentSP - myCurSkill.LevelUp(clevel - 1)) / (myCurSkill.LevelUp(clevel) - myCurSkill.LevelUp(clevel - 1)) * 100)

                Dim qItem As EveHQ.Core.SortedQueue = New EveHQ.Core.SortedQueue
                qItem.IsTraining = True
                qItem.Key = mySkill.Name & curLevel & clevel
                qItem.ID = mySkill.ID
                qItem.Name = mySkill.Name
                qItem.CurLevel = CStr(curLevel)
                qItem.FromLevel = CStr(curLevel)
                qItem.ToLevel = CStr(clevel)
                qItem.Percent = CStr(percent)
                qItem.TrainTime = CStr(cTime)
                qItem.DateFinished = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(qPilot.TrainingEndTime)
                qItem.Rank = CStr(mySkill.Rank)
                qItem.PAtt = mySkill.PA
                qItem.SAtt = mySkill.SA
                qItem.SPRate = CStr(EveHQ.Core.SkillFunctions.CalculateSPRate(qPilot, mySkill))
                qItem.SPTrained = CStr(qPilot.TrainingEndSP - qPilot.TrainingStartSP)
                totalSP += CLng(qItem.SPTrained)
                arrQueue.Add(qItem)

                ' Before we end check if a learning skill has been trained which will affect our future training times!
                Dim modifierIncrease As Integer = 1
                Select Case mySkill.Name
                    Case "Analytical Mind"      ' I
                        attModifiers(1) += modifierIncrease
                    Case "Clarity"              ' P
                        attModifiers(3) += modifierIncrease
                    Case "Eidetic Memory"       ' M
                        attModifiers(2) += modifierIncrease
                    Case "Empathy"              ' C
                        attModifiers(0) += modifierIncrease
                    Case "Focus"                ' W
                        attModifiers(4) += modifierIncrease
                    Case "Instant Recall"       ' M
                        attModifiers(2) += modifierIncrease
                    Case "Iron Will"            ' W
                        attModifiers(4) += modifierIncrease
                    Case "Learning"             ' L
                        attModifiers(5) += modifierIncrease
                    Case "Logic"                ' I
                        attModifiers(1) += modifierIncrease
                    Case "Presence"             ' C
                        attModifiers(0) += modifierIncrease
                    Case "Spatial Awareness"    ' P
                        attModifiers(3) += modifierIncrease
                End Select
            End If
        Catch ex As Exception
            MessageBox.Show("Error occurred in adding the currently training skill", "BuildQueue Error")
            Return Nothing
            Exit Function
        End Try


        If bQueue.Queue.Count > 0 Then
            Dim myTSkill As EveHQ.Core.SkillQueueItem
            ' Get starting point of time
            Try
                If qPilot.Training = True And bQueue.IncCurrentTraining = True Then
                    If EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(qPilot.TrainingEndTime) < Now Then
                        endtime = Now
                    Else
                        endtime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(qPilot.TrainingEndTime)
                    End If
                Else
                    endtime = Now
                End If
            Catch ex As Exception
                MessageBox.Show("Error getting starting point of time", "BuildQueue Error")
                Return Nothing
                Exit Function
            End Try


            ' Iterate thru skill queue and display info
            ' Create array for sorting
            Dim skillArray(bQueue.Queue.Count - 1, 1) As Long
            Try
                Dim count As Integer = 0
                For Each myTSkill In bQueue.Queue
                    Dim specSkillID As String = EveHQ.Core.SkillFunctions.SkillNameToID(myTSkill.Name) & myTSkill.FromLevel & myTSkill.ToLevel
                    skillArray(count, 0) = CLng(specSkillID)
                    skillArray(count, 1) = myTSkill.Pos
                    count += 1
                Next
            Catch ex As Exception
                MessageBox.Show("Error creating sort array", "BuildQueue Error")
                Return Nothing
                Exit Function
            End Try


            ' Create a tag array ready to sort the skill times
            Dim tagArray(bQueue.Queue.Count - 1) As Integer
            For a As Integer = 0 To bQueue.Queue.Count - 1
                tagArray(a) = a
            Next
            ' Initialize the comparer and sort
            Dim myComparer As New EveHQ.Core.Reports.RectangularComparer(skillArray)
            Array.Sort(tagArray, myComparer)

            ' Build the queue
            Dim skillPOS As Integer = 0
            For i As Integer = 0 To tagArray.Length - 1
                Dim myskill As New EveHQ.Core.EveSkill
                Dim fromLevel As Integer = 0
                Dim toLevel As Integer = 0
                Dim frLvl As String = ""
                Dim toLvl As String = ""
                Dim specSkillName As String = ""
                Dim specSkillID As String = ""
                Try
                    specSkillName = CStr(skillArray(tagArray(i), 0))
                    frLvl = specSkillName.Substring(specSkillName.Length - 2, 1)
                    toLvl = specSkillName.Substring(specSkillName.Length - 1, 1)
                    specSkillID = specSkillName.Substring(0, specSkillName.Length - 2)
                    myTSkill = CType(bQueue.Queue(EveHQ.Core.SkillFunctions.SkillIDToName(specSkillID) & frLvl & toLvl), SkillQueueItem)
                    skillPOS = CInt(skillArray(tagArray(i), 1))
                    myskill = CType(EveHQ.Core.HQ.SkillListName(myTSkill.Name), EveHQ.Core.EveSkill)
                    fromLevel = myTSkill.FromLevel
                    toLevel = myTSkill.ToLevel
                    Dim myPos As Integer = myTSkill.Pos
                    If qPilot.Training = False Then     ' decrement if applicable
                        myPos -= 1
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error initialising the sort comparer", "BuildQueue Error")
                    Return Nothing
                    Exit Function
                End Try

                ' Check for any training bonus
                If totalSP < trainingBonusLimit Then
                    currentBonus = trainingBonus
                Else
                    currentBonus = 1
                End If

                ' Check if we already have the skill and therefore the percentage completed
                Dim partiallyTrained As Boolean = False
                Dim curLevel As Integer = 0
                Dim percent As Double = 0
                Try
                    If qPilot.PilotSkills.Contains(myskill.Name) = True Then
                        Dim myCurSkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(myskill.Name), EveHQ.Core.PilotSkill)
                        curLevel = myCurSkill.Level
                        If curLevel = fromLevel Then
                            partiallyTrained = True
                        End If
                        If fromLevel = toLevel Then
                            If curLevel >= fromLevel Then
                                percent = 100
                            Else
                                percent = 0
                            End If
                        Else
                            Select Case curLevel
                                Case 0
                                    percent = 0
                                Case 5
                                    percent = 100
                                Case Else
                                    ' Whole skill line percent
                                    percent = (Math.Min(Math.Max(CDbl((myCurSkill.SP - myCurSkill.LevelUp(fromLevel)) / (myCurSkill.LevelUp(toLevel) - myCurSkill.LevelUp(fromLevel)) * 100), 0), 100))
                            End Select
                        End If
                    Else
                        curLevel = 0
                        percent = 0
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error calculating percentages complete", "BuildQueue Error")
                    Return Nothing
                    Exit Function
                End Try

                ' Get the time taken to train to that level
                Dim cTime As Integer
                Dim qItem As EveHQ.Core.SortedQueue = New EveHQ.Core.SortedQueue
                Try
                    If partiallyTrained = False Then
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, fromLevel, attModifiers, currentBonus))
                    Else
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, -1, attModifiers, currentBonus))
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error calculating time taken for level", "BuildQueue Error")
                    Return Nothing
                    Exit Function
                End Try

                If QuickBuild = False Then
                    Try
                        ' Check if the skill has been trained and strike it out if it has!
                        If curLevel >= toLevel Then
                            qItem.Done = True
                            percent = 100
                            cTime = 0
                        End If
                        ' Check if the skill is currently being trained and strike it out if it is!
                        If qPilot.Training = True Then
                            If myskill.ID = qPilot.TrainingSkillID Then
                                ' Take account of whether the current training skill has been added to the queue
                                If bQueue.IncCurrentTraining = True Then
                                    If curLevel = fromLevel And qPilot.TrainingSkillLevel = toLevel Then
                                        qItem.Done = True
                                        percent = 100
                                        cTime = 0
                                    End If
                                End If
                            End If
                        End If
                        endtime = DateAdd(DateInterval.Second, cTime, endtime)
                    Catch ex As Exception
                        MessageBox.Show("Error calculating percentage for level", "BuildQueue Error")
                        Return Nothing
                        Exit Function
                    End Try

                    ' Check if the skill is a pre-requisite of another in the queue and highlight it if so
                    Try
                        Dim RequiredFor As String = IsPrerequisite(qPilot, bQueue, EveHQ.Core.SkillFunctions.SkillIDToName(specSkillID) & frLvl & toLvl)
                        If RequiredFor <> "" Then
                            qItem.IsPrereq = True
                            qItem.Prereq = "Required for: " & RequiredFor
                        End If
                        Dim Requires As String = HasPrerequisite(qPilot, bQueue, EveHQ.Core.SkillFunctions.SkillIDToName(specSkillID) & frLvl & toLvl)
                        If Requires <> "" Then
                            qItem.HasPrereq = True
                            qItem.Reqs = "Requires: " & Requires
                        End If
                        Dim strTime As String = EveHQ.Core.SkillFunctions.TimeToString(cTime)
                        qItem.Key = myskill.Name & fromLevel & toLevel
                        qItem.ID = myskill.ID
                        qItem.Name = myskill.Name
                        qItem.CurLevel = CStr(curLevel)
                        qItem.FromLevel = CStr(fromLevel)
                        qItem.ToLevel = CStr(toLevel)
                        qItem.Percent = CStr(Int(percent))
                        If percent > 0 And percent < 100 Then
                            qItem.PartTrained = True
                        Else
                            qItem.PartTrained = False
                        End If
                        qItem.TrainTime = CStr(cTime)
                        qItem.DateFinished = endtime
                        qItem.Rank = CStr(myskill.Rank)
                        qItem.PAtt = myskill.PA
                        qItem.SAtt = myskill.SA
                        qItem.SPRate = CStr(EveHQ.Core.SkillFunctions.CalculateSPRate(qPilot, myskill, attModifiers, currentBonus))
                        If qItem.Done = False Then
                            If curLevel < fromLevel Then
                                qItem.SPTrained = CStr(EveHQ.Core.SkillFunctions.CalculateSP(qPilot, myskill, toLevel, fromLevel))
                            Else
                                qItem.SPTrained = CStr(EveHQ.Core.SkillFunctions.CalculateSP(qPilot, myskill, toLevel, -1))
                            End If
                        Else
                            qItem.SPTrained = "0"
                        End If
                        arrQueue.Add(qItem)
                        totalSkills += 1
                        totalTime += cTime
                        totalSP += CLng(qItem.SPTrained)
                    Catch ex As Exception
                        MessageBox.Show("Error checking pre-requisite skills", "BuildQueue Error")
                        Return Nothing
                        Exit Function
                    End Try
                Else
                    totalSkills += 1
                    totalTime += cTime
                End If

                ' Before we end check if a learning skill has been trained which will affect our future training times!
                ' Need to check if the skill has been done before so that the attribute isn't modified
                If qItem.Done = False Then
                    Try
                        Dim modifierIncrease As Integer = toLevel - fromLevel
                        Select Case myskill.Name
                            Case "Analytical Mind"      ' I
                                attModifiers(1) += modifierIncrease
                            Case "Clarity"              ' P
                                attModifiers(3) += modifierIncrease
                            Case "Eidetic Memory"       ' M
                                attModifiers(2) += modifierIncrease
                            Case "Empathy"              ' C
                                attModifiers(0) += modifierIncrease
                            Case "Focus"                ' W
                                attModifiers(4) += modifierIncrease
                            Case "Instant Recall"       ' M
                                attModifiers(2) += modifierIncrease
                            Case "Iron Will"            ' W
                                attModifiers(4) += modifierIncrease
                            Case "Learning"             ' L
                                attModifiers(5) += modifierIncrease
                            Case "Logic"                ' I
                                attModifiers(1) += modifierIncrease
                            Case "Presence"             ' C
                                attModifiers(0) += modifierIncrease
                            Case "Spatial Awareness"    ' P
                                attModifiers(3) += modifierIncrease
                        End Select
                    Catch ex As Exception
                        MessageBox.Show("Error adjusting modifiers", "BuildQueue Error")
                        Return Nothing
                        Exit Function
                    End Try
                End If
            Next
            ' Add the totaltime and skills to the queue data
            qQueue.QueueTime = totalTime
            qQueue.QueueSkills = totalSkills
        Else
            qQueue.QueueTime = 0
            qQueue.QueueSkills = 0
        End If

        Return arrQueue
    End Function

    Private Shared Function IsPrerequisite(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue, ByVal skillQueueKey As String) As String
        Dim IsPreReq As String = ""
        Dim curToLevel As String = skillQueueKey.Substring(skillQueueKey.Length - 1, 1)
        Dim curFromLevel As String = skillQueueKey.Substring(skillQueueKey.Length - 2, 1)
        Dim curSkillName As String = skillQueueKey.Substring(0, skillQueueKey.Length - 2)

        Dim skill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each skill In bQueue.Queue

            Dim myPreReqs As String = GetSkillReqs(qPilot, EveHQ.Core.SkillFunctions.SkillNameToID(skill.Name))
            Dim preReqs() As String = myPreReqs.Split(ControlChars.Cr)
            For Each preReq As String In preReqs
                If preReq.Length <> 0 Then
                    Dim pilotLevel As String = preReq.Substring(preReq.Length - 1, 1)
                    Dim reqLevel As String = preReq.Substring(preReq.Length - 2, 1)
                    Dim reqSkill As String = preReq.Substring(0, preReq.Length - 2)
                    If reqSkill = curSkillName And curFromLevel < reqLevel Then
                        ' Check if the skill is already listed
                        If IsPreReq.Contains(skill.Name) = False Then
                            IsPreReq &= skill.Name & ", "
                        End If
                    End If
                End If
            Next
        Next
        If IsPreReq <> "" Then
            Return IsPreReq.Substring(0, IsPreReq.Length - 2)
        Else
            Return IsPreReq
        End If
    End Function

    Private Shared Function HasPrerequisite(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue, ByVal skillQueueKey As String) As String
        Dim HasPreReq As String = ""
        Dim curToLevel As String = skillQueueKey.Substring(skillQueueKey.Length - 1, 1)
        Dim curFromLevel As String = skillQueueKey.Substring(skillQueueKey.Length - 2, 1)
        Dim curSkillName As String = skillQueueKey.Substring(0, skillQueueKey.Length - 2)

        Dim myPreReqs As String = GetSkillReqs(qPilot, EveHQ.Core.SkillFunctions.SkillNameToID(curSkillName))
        Dim preReqs() As String = myPreReqs.Split(ControlChars.Cr)

        Dim skill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each skill In bQueue.Queue
            For Each preReq As String In preReqs
                If preReq.Length <> 0 Then
                    Dim pilotLevel As String = preReq.Substring(preReq.Length - 1, 1)
                    Dim reqLevel As String = preReq.Substring(preReq.Length - 2, 1)
                    Dim reqSkill As String = preReq.Substring(0, preReq.Length - 2)
                    If reqSkill = skill.Name And skill.ToLevel <= CDbl(reqLevel) Then
                        ' Check if the skill is already listed
                        If HasPreReq.Contains(skill.Name & " (Lvl " & reqLevel & ")") = False Then
                            HasPreReq &= skill.Name & " (Lvl " & reqLevel & "), "
                        End If
                    End If
                End If
            Next
        Next
        If HasPreReq <> "" Then
            Return HasPreReq.Substring(0, HasPreReq.Length - 2)
        Else
            Return HasPreReq
        End If
    End Function

    Private Shared Sub CheckAlreadyTrained(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue)
        Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each curSkill In bQueue.Queue
            If qPilot.PilotSkills.Contains(curSkill.Name) Then
                Dim fromLevel As Integer = curSkill.FromLevel
                Dim toLevel As Integer = curSkill.ToLevel
                Dim mySkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(curSkill.Name), EveHQ.Core.PilotSkill)
                Dim pilotLevel As Integer = mySkill.Level
                If pilotLevel < toLevel Then
                    If fromLevel < pilotLevel Then
                        Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                        curSkill.FromLevel = pilotLevel
                        Dim keyName As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                        bQueue.Queue.Remove(oldKey)
                        bQueue.Queue.Add(curSkill, keyName)
                    End If
                    'Else
                    '    ' Clear the trained skill
                    '    Dim keyName As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                    '    bQueue.Queue.Remove(keyName)
                End If
            End If
        Next
    End Sub

    Private Shared Sub CheckAlreadyTraining(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue)
        If qPilot.Training = True Then
            Dim trainSkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(qPilot.TrainingSkillName), EveHQ.Core.PilotSkill)
            Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            For Each curSkill In bQueue.Queue
                If curSkill.Name = trainSkill.Name Then
                    If qPilot.TrainingSkillLevel < curSkill.ToLevel And qPilot.TrainingSkillLevel - 1 = curSkill.FromLevel Then

                        ' Create a new training queue item that covers the current training
                        Dim newskill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                        newskill.Name = curSkill.Name
                        newskill.FromLevel = curSkill.FromLevel
                        newskill.ToLevel = newskill.FromLevel + 1
                        Dim newKey As String = newskill.Name & newskill.FromLevel & newskill.ToLevel
                        ' Increase the from level of the existing skill
                        Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                        curSkill.FromLevel += 1
                        Dim keyName As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                        bQueue.Queue.Remove(oldKey)
                        bQueue.Queue.Add(curSkill, keyName)
                        bQueue.Queue.Add(newskill, newKey)
                    End If
                End If
            Next
        End If
    End Sub

    Private Shared Sub CheckSkillFlow(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue)
        ' Check there aren't any discrepancies
        Dim skillsChecked As String = ""

        If bQueue.Queue.Count <> 0 Then
            For count As Integer = 1 To bQueue.Queue.Count
                Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                curSkill = CType(bQueue.Queue(count), SkillQueueItem)
                If skillsChecked.Contains(curSkill.Name) = False Then
                    Dim pilotLevel As Integer = 0
                    Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                    If qPilot.PilotSkills.Contains(curSkill.Name) Then
                        mySkill = CType(qPilot.PilotSkills(curSkill.Name), EveHQ.Core.PilotSkill)
                        ' Check if the skill is being trained, therefore the current level is actually
                        ' going to be the end level of the current skill
                        If qPilot.Training = True And mySkill.Name = qPilot.TrainingSkillName Then
                            pilotLevel = qPilot.TrainingSkillLevel
                        Else
                            pilotLevel = mySkill.Level
                        End If
                    End If
                    Dim fromLevel As Integer = curSkill.FromLevel
                    Dim tolevel As Integer = curSkill.ToLevel
                    Dim curKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel

                    Dim skillArray(bQueue.Queue.Count) As String
                    skillArray(0) = curKey

                    Dim counter As Integer = 0
                    For count2 As Integer = count + 1 To bQueue.Queue.Count
                        Dim checkSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                        checkSkill = CType(bQueue.Queue(count2), SkillQueueItem)
                        If curSkill.Name = checkSkill.Name Then
                            counter += 1
                            Dim checkKey As String = checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel
                            skillArray(counter) = checkKey
                        End If
                    Next
                    ReDim Preserve skillArray(counter)

                    ' We should have all the same skills in here at various levels, so sort the array
                    Array.Sort(skillArray)

                    ' Check the skill starts at the pilot's current level adjust lower level if not
                    Dim startToLevel As String = skillArray(0).Substring(skillArray(0).Length - 1, 1)
                    Dim startFromLevel As String = skillArray(0).Substring(skillArray(0).Length - 2, 1)
                    Dim startSkillName As String = skillArray(0).Substring(0, skillArray(0).Length - 2)
                    Dim startKeyName As String = startSkillName & startFromLevel & startToLevel
                    If CDbl(startFromLevel) > pilotLevel Then
                        Dim replaceSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                        replaceSkill = curSkill
                        replaceSkill.FromLevel = pilotLevel
                        bQueue.Queue.Remove(startKeyName)
                        startKeyName = replaceSkill.Name & replaceSkill.FromLevel & replaceSkill.ToLevel
                        bQueue.Queue.Add(replaceSkill, startKeyName)
                        skillArray(0) = startKeyName
                    End If

                    ' Now check all the other skills with the same name to ensure it flows
                    If counter <> 0 Then
                        For count2 As Integer = 0 To counter - 1
                            Dim curToLevel As String = skillArray(count2).Substring(skillArray(count2).Length - 1, 1)
                            Dim curFromLevel As String = skillArray(count2).Substring(skillArray(count2).Length - 2, 1)
                            Dim curSkillName As String = skillArray(count2).Substring(0, skillArray(count2).Length - 2)
                            Dim curKeyName As String = curSkillName & curFromLevel & curToLevel
                            Dim nextToLevel As String = skillArray(count2 + 1).Substring(skillArray(count2 + 1).Length - 1, 1)
                            Dim nextFromLevel As String = skillArray(count2 + 1).Substring(skillArray(count2 + 1).Length - 2, 1)
                            Dim nextSkillName As String = skillArray(count2 + 1).Substring(0, skillArray(count2 + 1).Length - 2)
                            Dim nextKeyName As String = nextSkillName & nextFromLevel & nextToLevel
                            If curToLevel <> nextFromLevel Then
                                If curToLevel > nextFromLevel Then
                                    ' We have increased the skill level? 
                                    ' Increase the current one to match
                                    Dim newKeyName As String = ""
                                    Dim replaceSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                                    replaceSkill = CType(bQueue.Queue(curKeyName), SkillQueueItem)
                                    replaceSkill.ToLevel = CInt(nextFromLevel)
                                    bQueue.Queue.Remove(curKeyName)
                                    newKeyName = replaceSkill.Name & replaceSkill.FromLevel & replaceSkill.ToLevel
                                    bQueue.Queue.Add(replaceSkill, newKeyName)
                                Else
                                    ' We have decreased the skill level? 
                                    ' Increase the current one to match
                                    Dim newKeyName As String = ""
                                    Dim replaceSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                                    replaceSkill = CType(bQueue.Queue(nextKeyName), SkillQueueItem)
                                    replaceSkill.FromLevel = CInt(curToLevel)
                                    bQueue.Queue.Remove(nextKeyName)
                                    newKeyName = replaceSkill.Name & replaceSkill.FromLevel & replaceSkill.ToLevel
                                    bQueue.Queue.Add(replaceSkill, newKeyName)
                                End If
                            End If
                        Next
                    End If
                    skillsChecked &= curSkill.Name & " "
                End If
            Next
        End If
    End Sub

    Private Shared Sub CheckPreReqs(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue)
        ' Sub to ensure we have all the prerequisite skills we require
        ' Skills are added if required

        Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each curSkill In bQueue.Queue
            ' Work out if Skill pre-requisites are needed and add them to the queue
            If EveHQ.Core.SkillFunctions.SkillNameToID(curSkill.Name) <> "" Then
                Dim myPreReqs As String = GetSkillReqs(qPilot, EveHQ.Core.SkillFunctions.SkillNameToID(curSkill.Name))
                Dim preReqs() As String = myPreReqs.Split(ControlChars.Cr)
                Dim preReq As String
                For Each preReq In preReqs
                    If preReq.Length <> 0 Then
                        Dim pilotLevel As String = preReq.Substring(preReq.Length - 1, 1)
                        Dim reqLevel As String = preReq.Substring(preReq.Length - 2, 1)
                        Dim reqSkill As String = preReq.Substring(0, preReq.Length - 2)
                        If pilotLevel <> "Y" Then
                            ' Skill is not trained, check training queue
                            bQueue = AddPreReqSkillToQueue(qPilot, bQueue, reqSkill, CInt(pilotLevel), CInt(reqLevel))
                        End If
                    End If
                Next
            End If
        Next
    End Sub

    Private Shared Sub CheckSkillOrder(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue)
        ' Sub designed to check the order of skills trained so that they are trained in a proper sequence
        ' This is for multiple instances of the same skill only

        Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each curSkill In bQueue.Queue
            Dim curPOS As Integer = curSkill.Pos

            Dim checkSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            Dim lowestKey As String = ""
            Dim lowestLevel As Integer = curSkill.FromLevel
            Dim moveNeeded As Boolean = False

            Do
                lowestLevel = curSkill.FromLevel
                moveNeeded = False
                For Each checkSkill In bQueue.Queue
                    If checkSkill.Name = curSkill.Name And checkSkill.Pos > curSkill.Pos And checkSkill.FromLevel < lowestLevel Then
                        ' We've found one but we need to check for others so we can move the lowest one
                        lowestKey = checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel
                        lowestLevel = checkSkill.FromLevel
                        moveNeeded = True
                    End If
                Next

                If moveNeeded = True Then
                    ' We should have the key of the lowest skill now
                    Dim moveSkill As EveHQ.Core.SkillQueueItem = CType(bQueue.Queue(lowestKey), SkillQueueItem)
                    Dim movePOS As Integer = moveSkill.Pos

                    Dim nudgeSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                    For Each nudgeSkill In bQueue.Queue
                        If nudgeSkill.Pos >= curPOS Then
                            If nudgeSkill.Pos = movePOS Then
                                nudgeSkill.Pos = curPOS
                            Else
                                nudgeSkill.Pos += 1
                            End If
                        End If
                    Next
                    curPOS += 1
                End If

                ' Repeat again until we have all the skills moved
            Loop Until moveNeeded = False
        Next

    End Sub

    Private Shared Sub CheckReqOrder(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue)
        ' Sub designed to check the order of skills trained so that they are trained in a proper sequence
        ' This is for correct pre-requisite order
        Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each curSkill In bQueue.Queue
            Dim curPOS As Integer = curSkill.Pos

            ' Get the list of pre-reqs for the current skill
            If EveHQ.Core.SkillFunctions.SkillNameToID(curSkill.Name) <> "" Then
                Dim myPreReqs As String = GetSkillReqs(qPilot, EveHQ.Core.SkillFunctions.SkillNameToID(curSkill.Name))
                Dim preReqs() As String = myPreReqs.Split(ControlChars.Cr)

                ' Iterate thru the pre-reqs starting at the lowest pre-req first
                For Each preReq As String In preReqs
                    If preReq.Length <> 0 Then
                        Dim pilotLevel As String = preReq.Substring(preReq.Length - 1, 1)
                        Dim reqLevel As String = preReq.Substring(preReq.Length - 2, 1)
                        Dim reqSkill As String = preReq.Substring(0, preReq.Length - 2)
                        Dim checkSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                        Dim lowestKey As String = ""
                        Dim lowestLevel As Integer = CInt(reqLevel)
                        Dim moveNeeded As Boolean = False

                        Do
                            moveNeeded = False
                            For Each checkSkill In bQueue.Queue
                                ' See if the checkSkill is one of our pre-reqs and is after our skill
                                If checkSkill.Name = reqSkill And checkSkill.Pos > curSkill.Pos And checkSkill.FromLevel < lowestLevel And checkSkill.ToLevel >= lowestLevel Then
                                    ' We've found one but we need to check for others so we can move the lowest one
                                    lowestKey = checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel
                                    lowestLevel = CInt(reqLevel)
                                    moveNeeded = True
                                End If
                            Next

                            If moveNeeded = True Then
                                ' We should have the key of the lowest skill now
                                Dim moveSkill As EveHQ.Core.SkillQueueItem = CType(bQueue.Queue(lowestKey), SkillQueueItem)
                                Dim movePOS As Integer = moveSkill.Pos

                                Dim nudgeSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                                For Each nudgeSkill In bQueue.Queue
                                    If nudgeSkill.Pos >= curPOS Then
                                        If nudgeSkill.Pos = movePOS Then
                                            nudgeSkill.Pos = curPOS
                                        Else
                                            nudgeSkill.Pos += 1
                                        End If
                                    End If
                                Next
                                curPOS += 1
                            End If

                            ' Repeat again until we have all the skills moved
                        Loop Until moveNeeded = False
                    End If
                Next
            End If
        Next

    End Sub

    Public Shared Function AddSkillToQueue(ByVal qPilot As EveHQ.Core.Pilot, ByVal addSkill As String, ByVal di As Integer, ByVal qQueue As EveHQ.Core.SkillQueue, Optional ByVal planLevel As Integer = 0, Optional ByVal silent As Boolean = False, Optional ByVal exitIfTrained As Boolean = False) As EveHQ.Core.SkillQueue
        ' Check if the skill is already in the list - key names are skill IDs!
        ' NB: include the current training skill!
        Dim newSkill As New ListViewItem
        newSkill.Text = addSkill

        If qPilot.Updated = False Then
            If silent = False Then
                Dim msg As String = ""
                msg &= "You need to update your pilot information before creating a skill queue!" & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Please either update the accounts or update your pilot manually."
                MessageBox.Show(msg, "Pilot Data Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return qQueue
            Exit Function
        End If

        Dim nQueue As EveHQ.Core.SkillQueue = qQueue

        ' See if the pilot already has that skill and optional exit if already trained
        If qPilot.PilotSkills.Contains(newSkill.Text) = True Then
            Dim mySkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(newSkill.Text), EveHQ.Core.PilotSkill)
            Dim myLevel As Integer = mySkill.Level
            If myLevel >= 5 Then
                If silent = False Then
                    MessageBox.Show("You already have " & newSkill.Text & " trained to Level 5", "Skill Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return qQueue
                Exit Function
            Else
                If myLevel >= planLevel And exitIfTrained = True Then
                    Return qQueue
                    Exit Function
                End If
            End If
        Else
            ' Work out if Skill pre-requisites are needed and add them to the queue
            If planLevel = 0 Then
                Dim myPreReqs As String = GetSkillReqs(qPilot, EveHQ.Core.SkillFunctions.SkillNameToID(newSkill.Text))
                Dim preReqs() As String = myPreReqs.Split(ControlChars.Cr)
                Dim preReq As String
                For Each preReq In preReqs
                    If preReq.Length <> 0 Then
                        Dim pilotLevel As String = preReq.Substring(preReq.Length - 1, 1)
                        Dim reqLevel As String = preReq.Substring(preReq.Length - 2, 1)
                        Dim reqSkill As String = preReq.Substring(0, preReq.Length - 2)
                        If pilotLevel <> "Y" Then
                            ' Skill is not trained, check training queue
                            nQueue = AddPreReqSkillToQueue(qPilot, nQueue, reqSkill, CInt(pilotLevel), CInt(reqLevel))
                        End If
                    End If
                Next
            End If
        End If

        ' Get initial from and to levels
        Dim myNewSkill As EveHQ.Core.EveSkill = CType(EveHQ.Core.HQ.SkillListName(newSkill.Text), EveHQ.Core.EveSkill)
        Dim fromLevel As Integer
        ' Get the next skill level to train - account for the skill level of the currently training skill!
        If qPilot.Training = True And qPilot.TrainingSkillName = newSkill.Text Then
            fromLevel = qPilot.TrainingSkillLevel
        Else
            fromLevel = EveHQ.Core.SkillFunctions.CalcCurrentLevel(qPilot, myNewSkill)
        End If
        Dim toLevel As Integer
        If planLevel = 0 Then
            toLevel = Math.Min(fromLevel + 1, 5)
        Else
            planLevel = Math.Min(Math.Max(fromLevel + 1, planLevel), 5)
            toLevel = Math.Min(planLevel, 5)
        End If
        Dim keyName As String = myNewSkill.Name & fromLevel & toLevel

        ' Check through all the items in the queue and see if we have any that exist
        Dim maxLevel As Integer = 0
        Dim checkSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        If newSkill.Text = qPilot.TrainingSkillName Then
            maxLevel = Math.Max(maxLevel, CInt(qPilot.TrainingSkillLevel))
        End If
        For Each checkSkill In nQueue.Queue
            If checkSkill.Name = newSkill.Text Then
                maxLevel = Math.Max(maxLevel, CInt(checkSkill.ToLevel))
            End If
        Next

        ' Exit if our planned limit has been exceeded
        If planLevel > 0 Then
            If planLevel <= maxLevel Then
                ' Exit if we have already planned to our limit
                Return qQueue
                Exit Function
            Else
                fromLevel = Math.Max(maxLevel, fromLevel)
                toLevel = planLevel
            End If
        Else
            If maxLevel >= 5 Then
                If silent = False Then
                    MessageBox.Show(newSkill.Text & " is already queued to Level 5", "Skill Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return qQueue
                Exit Function
            Else
                fromLevel = Math.Max(maxLevel, fromLevel)           ' Need to compare it to current skill also
                toLevel = fromLevel + 1
            End If
        End If

        ' Move all the items down the queue after the entry position
        Dim moveSkill As EveHQ.Core.SkillQueueItem
        For Each moveSkill In nQueue.Queue
            If moveSkill.Pos >= di Then
                moveSkill.Pos += 1
            End If
        Next

        ' Add skill to the pilot training queue
        Dim newTSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        newTSkill.Name = myNewSkill.Name
        newTSkill.FromLevel = fromLevel
        newTSkill.ToLevel = toLevel
        newTSkill.Pos = di
        keyName = newTSkill.Name & newTSkill.FromLevel & newTSkill.ToLevel
        newTSkill.Key = keyName
        nQueue.Queue.Add(newTSkill, newTSkill.Key)
        Return nQueue
    End Function

    Public Shared Function AddPreReqSkillToQueue(ByVal qPilot As EveHQ.Core.Pilot, ByVal qQueue As EveHQ.Core.SkillQueue, ByVal skillName As String, ByVal fromLevel As Integer, ByVal toLevel As Integer) As EveHQ.Core.SkillQueue
        Dim nQueue As EveHQ.Core.SkillQueue = qQueue
        Dim keyName As String = skillName & fromLevel & toLevel
        Dim myNewSkill As EveHQ.Core.EveSkill = CType(EveHQ.Core.HQ.SkillListName(skillName), EveHQ.Core.EveSkill)

        ' Check through all the items in the queue and see if we have any that exist
        Dim maxLevel As Integer = 0
        Dim checkSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        If skillName = qPilot.TrainingSkillName Then
            maxLevel = Math.Max(maxLevel, CInt(qPilot.TrainingSkillLevel))
        End If
        For Each checkSkill In nQueue.Queue
            If checkSkill.Name = skillName Then
                maxLevel = Math.Max(maxLevel, CInt(checkSkill.ToLevel))
            End If
        Next

        ' See if we have reached our maximum training capability (i.e. to lvl5)
        If maxLevel >= 5 Then
            Return qQueue
            Exit Function
        Else
            fromLevel = Math.Max(maxLevel, fromLevel)           ' Need to compare it to current skill also
            toLevel = Math.Max(fromLevel, toLevel)
        End If

        ' Check if the level we want has already been trained
        If fromLevel = toLevel Then
            Return qQueue
            Exit Function
        Else
            ' Add skill to the pilot training queue
            Dim newTSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            newTSkill.Name = myNewSkill.Name
            newTSkill.FromLevel = fromLevel
            newTSkill.ToLevel = toLevel
            newTSkill.Pos = qQueue.Queue.Count + 1
            keyName = newTSkill.Name & newTSkill.FromLevel & newTSkill.ToLevel
            newTSkill.Key = keyName
            nQueue.Queue.Add(newTSkill, newTSkill.Key)
        End If

        Return nQueue

    End Function

    Public Shared Function GetSkillReqs(ByVal qPilot As EveHQ.Core.Pilot, ByVal skillID As String) As String
        Dim strReqs As String = ""

        Dim level As Integer = 1
        Dim pointer(20) As Integer
        Dim parent(20) As Integer
        Dim skillName(20) As String
        Dim skillLevel(20) As String
        pointer(level) = 1
        parent(level) = CInt(skillID)

        Dim strTree As String = ""
        Dim cSkill As EveHQ.Core.EveSkill = CType(EveHQ.Core.HQ.SkillListID(skillID), EveHQ.Core.EveSkill)
        Dim curSkill As Integer = CInt(skillID)
        Dim curLevel As Integer = 0
        Dim counter As Integer = 0
        Dim curNode As TreeNode = New TreeNode

        ' Write the skill we are querying as the first (parent) node
        curNode.Text = cSkill.Name
        Dim skillTrained As Boolean = False

        If cSkill.PreReqSkills.Count > 0 Then
            Dim subSkill As EveHQ.Core.EveSkill
            For Each subSkillID As String In cSkill.PreReqSkills.Keys
                subSkill = CType(EveHQ.Core.HQ.SkillListID(subSkillID), EveHQ.Core.EveSkill)
                Call GetSkillPreReqs(qPilot, subSkill, cSkill.PreReqSkills(subSkillID), curNode, strReqs)
            Next
        End If
        Return strReqs

    End Function

    Private Shared Sub GetSkillPreReqs(ByVal qPilot As EveHQ.Core.Pilot, ByVal newskill As EveHQ.Core.EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode, ByRef strReqs As String)
        ' Check if the current pilot has the skill
        Dim newNode As TreeNode = New TreeNode
        newNode.Text = newskill.Name & curLevel
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        If qPilot.PilotSkills.Contains(newskill.Name) Then
            Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
            mySkill = CType(qPilot.PilotSkills(newskill.Name), EveHQ.Core.PilotSkill)
            myLevel = CInt(mySkill.Level)
            If myLevel >= curLevel Then skillTrained = True
        End If
        If skillTrained = True Then
            newNode.Text &= "Y"
            newNode.ForeColor = Color.LimeGreen
        Else
            newNode.Text &= myLevel
            newNode.ForeColor = Color.Red
        End If
        curNode.Nodes.Add(newNode)
        strReqs = newNode.Text & ControlChars.Cr & strReqs
        curNode = newNode

        If newskill.PreReqSkills.Count > 0 Then
            Dim subSkill As EveHQ.Core.EveSkill
            For Each subSkillID As String In newskill.PreReqSkills.Keys
                subSkill = CType(EveHQ.Core.HQ.SkillListID(subSkillID), EveHQ.Core.EveSkill)
                Call GetSkillPreReqs(qPilot, subSkill, newskill.PreReqSkills(subSkillID), curNode, strReqs)
            Next
        End If
    End Sub



    Public Shared Function IsPlanned(ByVal qPilot As EveHQ.Core.Pilot, ByVal skillName As String, ByVal level As Integer) As Integer

        ' Need to go through all the queues!
        Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        Dim planLevel As Integer = 0
        For Each qName As String In qPilot.TrainingQueues.Keys
            Dim nQ As EveHQ.Core.SkillQueue = CType(qPilot.TrainingQueues(qName), SkillQueue)
            For Each curSkill In nQ.Queue
                If curSkill.Name = skillName Then
                    planLevel = Math.Max(planLevel, curSkill.ToLevel)
                End If
            Next
        Next
        Return planLevel
    End Function

    Public Shared Sub RemoveTrainedSkills(ByVal qPilot As EveHQ.Core.Pilot, ByVal aQ As EveHQ.Core.SkillQueue)
        Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each curSkill In aQ.Queue
            If qPilot.PilotSkills.Contains(curSkill.Name) Then
                Dim fromLevel As Integer = curSkill.FromLevel
                Dim toLevel As Integer = curSkill.ToLevel
                Dim mySkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(curSkill.Name), Core.PilotSkill)
                Dim pilotLevel As Integer = mySkill.Level
                If pilotLevel >= toLevel Then
                    Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                    aQ.Queue.Remove(oldKey)
                End If
            End If
        Next
        aQ.QueueSkills = aQ.Queue.Count
    End Sub

    Public Shared Function SortQueueByPos(ByVal aQ As EveHQ.Core.SkillQueue) As EveHQ.Core.SkillQueue
        Dim sorter As New ArrayList
        For Each sqItem As EveHQ.Core.SkillQueueItem In aQ.Queue
            sqItem.Pos += 100
            sorter.Add(Format(sqItem.Pos, "00#") & sqItem.Key)
        Next
        sorter.Sort()
        Dim newQueue As EveHQ.Core.SkillQueue = CType(aQ.Clone, SkillQueue)
        newQueue.Queue.Clear()
        Dim sqKey As String
        Dim newPos As Integer = 0
        For Each sqItem As String In sorter
            newPos += 1
            sqKey = sqItem.Substring(3)
            Dim nQI As EveHQ.Core.SkillQueueItem = CType(aQ.Queue(sqKey), SkillQueueItem)
            nQI.Pos = newPos
            newQueue.Queue.Add(nQI, nQI.Key)
        Next
        Return newQueue
    End Function

    Public Shared Function TidyQueue(ByVal qPilot As EveHQ.Core.Pilot, ByVal aQueue As EveHQ.Core.SkillQueue, ByVal qList As ArrayList) As EveHQ.Core.SkillQueue
        ' Resets position numbers of the list
        Dim startPOS As Integer = 0
        If qPilot.Training = True Then
            startPOS = 1
        End If
        ' Rewrite the list!
        Dim nQueue As New Collection
        Dim count As Integer = 1
        If qList IsNot Nothing Then
            For Each qItem As EveHQ.Core.SortedQueue In qList
                If qItem.IsTraining = False Then
                    Dim newSkill As New EveHQ.Core.SkillQueueItem
                    newSkill.FromLevel = CInt(qItem.FromLevel)
                    newSkill.ToLevel = CInt(qItem.ToLevel)
                    newSkill.Name = qItem.Name
                    newSkill.Key = qItem.Key
                    newSkill.Pos = count
                    nQueue.Add(newSkill, newSkill.Key)
                    count += 1
                End If
            Next
            aQueue.Queue = nQueue
        End If
        Return aQueue
    End Function

    Private Shared Function LoadOptimalTraining() As ArrayList
        Dim optimalTraining As New ArrayList
        optimalTraining.Clear()
        optimalTraining.Add("Instant Recall1")
        optimalTraining.Add("Analytical Mind1")
        optimalTraining.Add("Learning1")
        optimalTraining.Add("Instant Recall2")
        optimalTraining.Add("Analytical Mind2")
        optimalTraining.Add("Learning2")
        optimalTraining.Add("Instant Recall3")
        optimalTraining.Add("Analytical Mind3")
        optimalTraining.Add("Learning3")
        optimalTraining.Add("Instant Recall4")
        optimalTraining.Add("Eidetic Memory1")
        optimalTraining.Add("Eidetic Memory2")
        optimalTraining.Add("Eidetic Memory3")
        optimalTraining.Add("Analytical Mind4")
        optimalTraining.Add("Logic1")
        optimalTraining.Add("Logic2")
        optimalTraining.Add("Logic3")
        optimalTraining.Add("Learning4")
        optimalTraining.Add("Instant Recall5")
        optimalTraining.Add("Eidetic Memory4")
        optimalTraining.Add("Analytical Mind5")
        optimalTraining.Add("Logic4")
        optimalTraining.Add("Eidetic Memory5")
        optimalTraining.Add("Learning5")
        optimalTraining.Add("Logic5")
        optimalTraining.Add("Empathy1")
        optimalTraining.Add("Iron Will1")
        optimalTraining.Add("Spatial Awareness1")
        optimalTraining.Add("Empathy2")
        optimalTraining.Add("Iron Will2")
        optimalTraining.Add("Spatial Awareness2")
        optimalTraining.Add("Empathy3")
        optimalTraining.Add("Iron Will3")
        optimalTraining.Add("Spatial Awareness3")
        optimalTraining.Add("Empathy4")
        optimalTraining.Add("Iron Will4")
        optimalTraining.Add("Spatial Awareness4")
        optimalTraining.Add("Empathy5")
        optimalTraining.Add("Iron Will5")
        optimalTraining.Add("Spatial Awareness5")
        ' Calculate the most efficient??
        optimalTraining.Add("Focus1")
        optimalTraining.Add("Clarity1")
        optimalTraining.Add("Presence1")
        optimalTraining.Add("Focus2")
        optimalTraining.Add("Clarity2")
        optimalTraining.Add("Presence2")
        optimalTraining.Add("Focus3")
        optimalTraining.Add("Clarity3")
        optimalTraining.Add("Presence3")
        optimalTraining.Add("Focus4")
        optimalTraining.Add("Clarity4")
        optimalTraining.Add("Presence4")
        optimalTraining.Add("Focus5")
        optimalTraining.Add("Clarity5")
        optimalTraining.Add("Presence5")
        Return optimalTraining
    End Function
    Public Shared Function FindSuggestions(ByVal qPilot As EveHQ.Core.Pilot, ByVal qQueue As EveHQ.Core.SkillQueue) As EveHQ.Core.SkillQueue

        Dim optimalTraining As ArrayList = LoadOptimalTraining()
        Dim oldQueueTime As Long = qQueue.QueueTime
        Dim originalQueueTime As Long = oldQueueTime
        Dim newQueueTime As Long = 0
        Dim skillSuggestions As New ArrayList

        Dim newQueue As EveHQ.Core.SkillQueue = CType(qQueue.Clone, Core.SkillQueue)

        Dim opSkill As String = ""
        Dim opLevel As Integer = 0
        Dim draftSkills As New ArrayList
        Dim suggestedSkills(54) As String
        skillSuggestions.Clear()
        Dim testQueue As EveHQ.Core.SkillQueue
        Dim OptimalSkill As String
        ' Do 3 passes
        For pass As Integer = 1 To 3
            For skillPos As Integer = 0 To optimalTraining.Count - 1
                OptimalSkill = CStr(optimalTraining.Item(skillPos))
                If draftSkills.Contains(OptimalSkill) = False Then
                    opLevel = CInt(OptimalSkill.Substring(OptimalSkill.Length - 1, 1))
                    opSkill = OptimalSkill.TrimEnd(CChar(CStr(opLevel)))
                    testQueue = CType(newQueue.Clone, Core.SkillQueue)
                    testQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(qPilot, opSkill, skillPos - 99, testQueue, opLevel, True)
                    Call EveHQ.Core.SkillQueueFunctions.BuildQueue(qPilot, testQueue, True)
                    newQueueTime = testQueue.QueueTime
                    If newQueueTime <= oldQueueTime Then
                        newQueue = CType(testQueue.Clone, Core.SkillQueue)
                        draftSkills.Add(OptimalSkill)
                        For lev As Integer = 1 To opLevel
                            draftSkills.Add(opSkill & lev)
                        Next
                        Dim basSkill As String = ""
                        Select Case opSkill
                            Case "Logic"
                                basSkill = "Analytical Mind"
                            Case "Presence"
                                basSkill = "Empathy"
                            Case "Eidetic Memory"
                                basSkill = "Instant Recall"
                            Case "Clarity"
                                basSkill = "Spatial Awareness"
                            Case "Focus"
                                basSkill = "Iron Will"
                        End Select
                        If basSkill <> "" Then
                            For lev As Integer = 1 To 4
                                draftSkills.Add(basSkill & lev)
                            Next
                        End If
                        oldQueueTime = newQueueTime
                    End If
                End If
            Next
            newQueueTime = oldQueueTime
        Next

        For Each optimalSkillItem As String In optimalTraining
            If draftSkills.Contains(optimalSkillItem) = True Then
                skillSuggestions.Add(optimalSkillItem)
            End If
        Next

        newQueue = CType(qQueue.Clone, Core.SkillQueue)
        For skillPos As Integer = 0 To skillSuggestions.Count - 1
            Dim finalSkill As String = CStr(skillSuggestions.Item(skillPos))
            opLevel = CInt(finalSkill.Substring(finalSkill.Length - 1, 1))
            opSkill = finalSkill.TrimEnd(CChar(CStr(opLevel)))
            newQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(qPilot, opSkill, skillPos - 99, newQueue, opLevel, True, True)
        Next
        newQueue = EveHQ.Core.SkillQueueFunctions.SortQueueByPos(newQueue)
        Dim newQueue2 As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(qPilot, newQueue)
        newQueueTime = newQueue.QueueTime

        Dim msg As String = ""
        If newQueueTime < originalQueueTime Then
            Return newQueue
        Else
            Return qQueue
        End If

    End Function

    Public Shared Function GetQueueTime(ByVal qPilot As EveHQ.Core.Pilot, ByVal qQueue As EveHQ.Core.SkillQueue) As Long

        If qQueue.Queue.Count > 0 Then
            Dim totalTime As Long = 0
            Dim curLevel As Integer = 0
            Dim cTime As Integer = 0
            Dim myskill As New EveHQ.Core.EveSkill
            Dim fromLevel As Integer = 0
            Dim toLevel As Integer = 0
            ' Declare variables for skill attribute modifications
            Dim attModifiers(5) As Integer      ' Order is as follows i.e. C,I,M,P,W,L
            Dim modifierIncrease As Integer = 0
            Dim myTSkill As EveHQ.Core.SkillQueueItem

            For i As Integer = 1 To qQueue.Queue.Count
                myTSkill = CType(qQueue.Queue(i), SkillQueueItem)
                myskill = CType(EveHQ.Core.HQ.SkillListName(myTSkill.Name), EveHQ.Core.EveSkill)
                fromLevel = myTSkill.FromLevel
                toLevel = myTSkill.ToLevel

                ' Check if we already have the skill and therefore the time taken
                If qPilot.PilotSkills.Contains(myskill.Name) = True Then
                    Dim myCurSkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(myskill.Name), EveHQ.Core.PilotSkill)
                    curLevel = myCurSkill.Level
                    If curLevel = fromLevel Then
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, -1, attModifiers))
                    Else
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, fromLevel, attModifiers))
                    End If
                Else
                    cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, fromLevel, attModifiers))
                End If

                totalTime += cTime

                ' Before we end check if a learning skill has been trained which will affect our future training times!
                ' Need to check if the skill has been done before so that the attribute isn't modified
                modifierIncrease = toLevel - fromLevel
                Select Case myskill.Name
                    Case "Analytical Mind"      ' I
                        attModifiers(1) += modifierIncrease
                    Case "Clarity"              ' P
                        attModifiers(3) += modifierIncrease
                    Case "Eidetic Memory"       ' M
                        attModifiers(2) += modifierIncrease
                    Case "Empathy"              ' C
                        attModifiers(0) += modifierIncrease
                    Case "Focus"                ' W
                        attModifiers(4) += modifierIncrease
                    Case "Instant Recall"       ' M
                        attModifiers(2) += modifierIncrease
                    Case "Iron Will"            ' W
                        attModifiers(4) += modifierIncrease
                    Case "Learning"             ' L
                        attModifiers(5) += modifierIncrease
                    Case "Logic"                ' I
                        attModifiers(1) += modifierIncrease
                    Case "Presence"             ' C
                        attModifiers(0) += modifierIncrease
                    Case "Spatial Awareness"    ' P
                        attModifiers(3) += modifierIncrease
                End Select
            Next
            ' Add the totaltime and skills to the queue data
            qQueue.QueueTime = totalTime
            Return totalTime
        End If

    End Function

End Class

<Serializable()> Public Class SkillQueueItem
    Implements System.ICloneable
    Public Key As String
    Public Name As String
    Public FromLevel As Integer
    Public ToLevel As Integer
    Public Pos As Integer
    Public PreReq1 As String
    Public PreReq2 As String
    Public PreReq3 As String
    Public PreReq4 As String
    Public PreReq5 As String
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim R As SkillQueue = CType(Me.MemberwiseClone, SkillQueue)
        Return R
    End Function
End Class
Public Class SortedQueue
    Public Done As Boolean
    Public Key As String
    Public ID As String
    Public Name As String
    Public CurLevel As String
    Public FromLevel As String
    Public ToLevel As String
    Public PartTrained As Boolean
    Public Percent As String
    Public TrainTime As String
    Public DateFinished As Date
    Public Rank As String
    Public PAtt As String
    Public SAtt As String
    Public SPRate As String
    Public SPTrained As String
    Public IsTraining As Boolean
    Public IsPrereq As Boolean
    Public Prereq As String
    Public HasPrereq As Boolean
    Public Reqs As String
End Class
<Serializable()> Public Class SkillQueue
    Implements System.ICloneable
    Public Name As String
    Public IncCurrentTraining As Boolean = True
    Public Queue As New Collection
    Public Primary As Boolean
    Public QueueTime As Long
    Public QueueSkills As Integer

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim newQueue As SkillQueue = CType(Me.MemberwiseClone, SkillQueue)

        Dim newQ As New Collection
        For Each qItem As EveHQ.Core.SkillQueueItem In Me.Queue
            Dim nItem As New EveHQ.Core.SkillQueueItem
            nItem.ToLevel = qItem.ToLevel
            nItem.FromLevel = qItem.FromLevel
            nItem.Name = qItem.Name
            nItem.Key = nItem.Name & nItem.FromLevel & nItem.ToLevel
            nItem.Pos = qItem.Pos
            newQ.Add(nItem, nItem.Key)
        Next
        newQueue.Queue = newQ
        Return newQueue

    End Function
End Class
