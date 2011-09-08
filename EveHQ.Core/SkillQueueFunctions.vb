' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports System.Text

Public Class SkillQueueFunctions

    Public Shared Event RefreshQueue()
    Public Shared SkillPrereqs As New SortedList(Of String, SortedList(Of String, Integer))
    Public Shared SkillDepends As New SortedList(Of String, SortedList(Of String, Integer))

    Shared Property StartQueueRefresh() As Boolean
        Get
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent RefreshQueue()
            End If
        End Set
    End Property

    Public Shared Function BuildQueue(ByVal qPilot As EveHQ.Core.Pilot, ByVal qQueue As EveHQ.Core.SkillQueue, ByVal QuickBuild As Boolean, ByVal UseAPIEndTime As Boolean) As ArrayList
        Dim bQueue As EveHQ.Core.SkillQueue = CType(qQueue.Clone, SkillQueue)

        Dim arrQueue As ArrayList = New ArrayList
        Dim totalTime As Long = 0
        Dim totalSkills As Integer = 0
        Dim totalSP As Long = qPilot.SkillPoints

        ' Prep a new font ready for completed training queues
        Dim doneFont As Font = New Font("Tahoma", 8, FontStyle.Strikeout)

        ' Try Queue Building
        Try
            ' Check for invalid skills
            Call CheckValidSkills(qPilot, bQueue)
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

        ' Add in the currently training skill if applicable
        Dim endtime As Date = Now
        Try
            If qPilot.Training = True And bQueue.IncCurrentTraining = True Then
                If EveHQ.Core.HQ.SkillListID.ContainsKey(qPilot.TrainingSkillID) = True Then
                    Dim mySkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(qPilot.TrainingSkillID)
                    Dim clevel As Integer = qPilot.TrainingSkillLevel
                    Dim cTime As Long = qPilot.TrainingCurrentTime
                    Dim strTime As String = EveHQ.Core.SkillFunctions.TimeToString(cTime)
                    If UseAPIEndTime = True Then
                        endtime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(qPilot.TrainingEndTime)
                    Else
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, mySkill, qPilot.TrainingSkillLevel, -1))
                        endtime = Now.AddSeconds(cTime)
                    End If

                    Dim curLevel As Integer = 0
                    Dim percent As Integer = 0
                    Dim myCurSkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(mySkill.Name), EveHQ.Core.PilotSkill)
                    curLevel = myCurSkill.Level
                    percent = CInt((myCurSkill.SP + qPilot.TrainingCurrentSP - myCurSkill.LevelUp(clevel - 1)) / (myCurSkill.LevelUp(clevel) - myCurSkill.LevelUp(clevel - 1)) * 100)
                    If (percent > 100) Then
                        percent = 100
                    End If

                    Dim qItem As New EveHQ.Core.SortedQueueItem
                    qItem.IsTraining = True
                    qItem.IsInjected = True
                    qItem.Key = mySkill.Name & curLevel & clevel
                    qItem.ID = mySkill.ID
                    qItem.Name = mySkill.Name
                    qItem.CurLevel = curLevel
                    qItem.FromLevel = curLevel
                    qItem.ToLevel = clevel
                    qItem.Percent = percent
                    qItem.TrainTime = cTime
                    qItem.TimeBeforeTrained = cTime
                    qItem.DateFinished = endtime
                    qItem.Rank = mySkill.Rank
                    qItem.PAtt = mySkill.PA
                    qItem.SAtt = mySkill.SA
                    qItem.SPTrained = qPilot.TrainingEndSP - qPilot.TrainingStartSP

                    qItem.SPRate = EveHQ.Core.SkillFunctions.CalculateSPRate(qPilot, mySkill)
                    totalSP += CLng(qItem.SPTrained)
                    arrQueue.Add(qItem)

                    If EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(qPilot.TrainingEndTime) < Now Then
                        endtime = Now
                    Else
                        If UseAPIEndTime = True Then
                            endtime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(qPilot.TrainingEndTime)
                        Else
                            endtime = Now.AddSeconds(CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, mySkill, qPilot.TrainingSkillLevel, -1)))
                        End If
                    End If

                Else
                    endtime = Now
                End If
            Else
                endtime = Now
            End If
        Catch ex As Exception
            MessageBox.Show("Error occurred in adding the currently training skill", "BuildQueue Error")
            Return Nothing
            Exit Function
        End Try

        If bQueue.Queue.Count > 0 Then
            Dim myTSkill As EveHQ.Core.SkillQueueItem

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

            ' Get a list of the skill names in the queue and their level
            Dim QueueReqs As New SortedList(Of String, Integer)
            For i As Integer = 0 To tagArray.Length - 1
                Dim frLvl As String = ""
                Dim toLvl As String = ""
                Dim specSkillName As String = ""
                Dim specSkillID As String = ""
                specSkillName = CStr(skillArray(tagArray(i), 0))
                frLvl = specSkillName.Substring(specSkillName.Length - 2, 1)
                toLvl = specSkillName.Substring(specSkillName.Length - 1, 1)
                specSkillName = EveHQ.Core.SkillFunctions.SkillIDToName(specSkillName.Substring(0, specSkillName.Length - 2))
                If QueueReqs.ContainsKey(specSkillName) = False Then
                    QueueReqs.Add(specSkillName, CInt(toLvl))
                End If
            Next

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
                Dim priority As Integer = 0
                Dim notes As String = ""
                Try
                    specSkillName = CStr(skillArray(tagArray(i), 0))
                    frLvl = specSkillName.Substring(specSkillName.Length - 2, 1)
                    toLvl = specSkillName.Substring(specSkillName.Length - 1, 1)
                    specSkillID = specSkillName.Substring(0, specSkillName.Length - 2)
                    myTSkill = CType(bQueue.Queue(EveHQ.Core.SkillFunctions.SkillIDToName(specSkillID) & frLvl & toLvl), SkillQueueItem)
                    skillPOS = CInt(skillArray(tagArray(i), 1))
                    myskill = EveHQ.Core.HQ.SkillListName(myTSkill.Name)
                    fromLevel = myTSkill.FromLevel
                    toLevel = myTSkill.ToLevel
                    If myTSkill.Notes <> "" Then
                        notes = myTSkill.Notes
                    End If
                    priority = myTSkill.Priority
                    Dim myPos As Integer = myTSkill.Pos
                    If qPilot.Training = False Then     ' decrement if applicable
                        myPos -= 1
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error initialising the sort comparer", "BuildQueue Error")
                    Return Nothing
                    Exit Function
                End Try

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
                Dim qItem As New EveHQ.Core.SortedQueueItem
                qItem.IsInjected = qPilot.PilotSkills.Contains(myskill.Name)

                Try
                    If partiallyTrained = False Then
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, fromLevel))
                    Else
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, -1))
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
                        If qPilot.Training = True And bQueue.IncCurrentTraining = True Then
                            If myskill.ID = qPilot.TrainingSkillID Then
                                ' Take account of whether the current training skill has been added to the queue
                                If curLevel = fromLevel And qPilot.TrainingSkillLevel = toLevel Then
                                    qItem.Done = True
                                    percent = 100
                                    cTime = 0
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
                    'Try
                    ' Check Depends
                    If EveHQ.Core.SkillQueueFunctions.SkillDepends.ContainsKey(myskill.Name) Then
                        For Each dependSkill As String In EveHQ.Core.SkillQueueFunctions.SkillDepends(myskill.Name).Keys
                            If QueueReqs.ContainsKey(dependSkill) Then
                                If EveHQ.Core.SkillFunctions.IsSkillTrained(qPilot, myskill.Name, EveHQ.Core.SkillQueueFunctions.SkillDepends(myskill.Name).Item(dependSkill)) = False Then
                                    qItem.Prereq &= dependSkill & ", "
                                End If
                            End If
                        Next
                        If qItem.Prereq <> "" Then
                            qItem.Prereq = qItem.Prereq.TrimEnd(", ".ToCharArray)
                            qItem.Prereq = "Required for: " & qItem.Prereq
                            qItem.IsPrereq = True
                        End If
                    End If
                    ' Check Prereqs
                    If EveHQ.Core.SkillQueueFunctions.SkillPrereqs.ContainsKey(myskill.Name) Then
                        For Each preReqSkill As String In EveHQ.Core.SkillQueueFunctions.SkillPrereqs(myskill.Name).Keys
                            If QueueReqs.ContainsKey(preReqSkill) Then
                                If Not EveHQ.Core.SkillFunctions.IsSkillTrained(qPilot, preReqSkill, EveHQ.Core.SkillQueueFunctions.SkillPrereqs(myskill.Name).Item(preReqSkill)) Then
                                    qItem.Reqs &= preReqSkill & " (Lvl " & EveHQ.Core.SkillQueueFunctions.SkillPrereqs(myskill.Name).Item(preReqSkill) & "), "
                                End If
                            End If
                        Next
                        If qItem.Reqs <> "" Then
                            qItem.Reqs = qItem.Reqs.TrimEnd(", ".ToCharArray)
                            qItem.Reqs = "Requires: " & qItem.Reqs
                            qItem.HasPrereq = True
                        End If
                    End If

                    qItem.Key = myskill.Name & fromLevel & toLevel
                    qItem.ID = myskill.ID
                    qItem.Name = myskill.Name
                    qItem.Notes = notes
                    qItem.Priority = priority
                    qItem.CurLevel = curLevel
                    qItem.FromLevel = fromLevel
                    qItem.ToLevel = toLevel
                    qItem.Percent = Int(percent)
                    If percent > 0 And percent < 100 Then
                        qItem.PartTrained = True
                    Else
                        qItem.PartTrained = False
                    End If
                    qItem.TrainTime = cTime
                    qItem.TimeBeforeTrained = CLng(EveHQ.Core.SkillFunctions.TimeBeforeCanTrain(qPilot, myskill.ID, toLevel))
                    qItem.DateFinished = endtime
                    qItem.Rank = myskill.Rank
                    qItem.PAtt = myskill.PA
                    qItem.SAtt = myskill.SA

                    If qItem.Done = False Then
                        If curLevel < fromLevel Then
                            qItem.SPTrained = EveHQ.Core.SkillFunctions.CalculateSP(qPilot, myskill, toLevel, fromLevel)
                        Else
                            qItem.SPTrained = EveHQ.Core.SkillFunctions.CalculateSP(qPilot, myskill, toLevel, -1)
                        End If
                    Else
                        qItem.SPTrained = 0
                    End If

                    If toLevel - fromLevel = 1 Then
                        ' If just a single level
                        qItem.SPRate = EveHQ.Core.SkillFunctions.CalculateSPRate(qPilot, myskill)
                    Else
                        ' If multiple levels, need to work out the correct bonus
                        If qItem.TrainTime > 0 Then
                            qItem.SPRate = CInt(Math.Round(qItem.SPTrained / qItem.TrainTime * 3600, 0))
                        Else
                            qItem.SPRate = EveHQ.Core.SkillFunctions.CalculateSPRate(qPilot, myskill)
                        End If
                    End If

                    arrQueue.Add(qItem)
                    totalSkills += 1
                    totalTime += cTime
                    totalSP += qItem.SPTrained
                    'Catch ex As Exception
                    'MessageBox.Show("Error checking pre-requisite skills", "BuildQueue Error")
                    'Return Nothing
                    'Exit Function
                    'End Try
                Else
                    totalSkills += 1
                    totalTime += cTime
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

    Private Shared Sub CheckValidSkills(ByVal qPilot As EveHQ.Core.Pilot, ByVal bQueue As EveHQ.Core.SkillQueue)
        For Each curSkill As EveHQ.Core.SkillQueueItem In bQueue.Queue
            If EveHQ.Core.HQ.SkillListName.ContainsKey(curSkill.Name) = False Then
                ' Remove the skill entry
                Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                bQueue.Queue.Remove(oldKey)
            End If
        Next
    End Sub

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
                        newskill.Notes = curSkill.Notes
                        newskill.Priority = curSkill.Priority
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
        Dim skillsToRemove As New ArrayList
        Dim mainCount As Integer = 0

        If bQueue.Queue.Count <> 0 Then
            Do
                mainCount += 1
                'For count As Integer = 1 To bQueue.Queue.Count
                Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                curSkill = CType(bQueue.Queue(mainCount), SkillQueueItem)
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
                    For count2 As Integer = mainCount + 1 To bQueue.Queue.Count
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
                        replaceSkill.Notes = curSkill.Notes
                        replaceSkill.Priority = curSkill.Priority
                        bQueue.Queue.Remove(startKeyName)
                        mainCount -= 1
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
                                    replaceSkill.Notes = curSkill.Notes
                                    replaceSkill.Priority = curSkill.Priority
                                    bQueue.Queue.Remove(curKeyName)
                                    mainCount -= 1
                                    newKeyName = replaceSkill.Name & replaceSkill.FromLevel & replaceSkill.ToLevel
                                    bQueue.Queue.Add(replaceSkill, newKeyName)
                                    If replaceSkill.ToLevel = replaceSkill.FromLevel Then
                                        skillsToRemove.Add(newKeyName)
                                    End If
                                Else
                                    ' We have decreased the skill level? 
                                    ' Check if the next skill starts at our current skill level
                                    If mySkill.Level = CInt(nextFromLevel) Then
                                        Dim newKeyName As String = ""
                                        Dim replaceSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                                        replaceSkill = CType(bQueue.Queue(curKeyName), SkillQueueItem)
                                        replaceSkill.ToLevel = mySkill.Level
                                        replaceSkill.Notes = curSkill.Notes
                                        replaceSkill.Priority = curSkill.Priority
                                        bQueue.Queue.Remove(curKeyName)
                                        mainCount -= 1
                                        newKeyName = replaceSkill.Name & replaceSkill.FromLevel & replaceSkill.ToLevel
                                        bQueue.Queue.Add(replaceSkill, newKeyName)
                                        If replaceSkill.FromLevel = replaceSkill.ToLevel Then
                                            skillsToRemove.Add(newKeyName)
                                        End If
                                    Else
                                        Dim newKeyName As String = ""
                                        Dim replaceSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                                        replaceSkill = CType(bQueue.Queue(nextKeyName), SkillQueueItem)
                                        replaceSkill.FromLevel = CInt(curToLevel)
                                        replaceSkill.Notes = curSkill.Notes
                                        replaceSkill.Priority = curSkill.Priority
                                        bQueue.Queue.Remove(nextKeyName)
                                        mainCount -= 1
                                        newKeyName = replaceSkill.Name & replaceSkill.FromLevel & replaceSkill.ToLevel
                                        bQueue.Queue.Add(replaceSkill, newKeyName)
                                        If replaceSkill.FromLevel = replaceSkill.ToLevel Then
                                            skillsToRemove.Add(newKeyName)
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                    skillsChecked &= curSkill.Name & " "
                End If
                If mainCount < 0 Then mainCount = 0
            Loop Until mainCount = bQueue.Queue.Count
            ' remove unwanted skills
            For Each unneededSkill As String In skillsToRemove
                bQueue.Queue.Remove(unneededSkill)
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
                If mySkill.Name = qPilot.TrainingSkillName Then
                    myLevel = qPilot.TrainingSkillLevel
                End If
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
        Dim myNewSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListName(newSkill.Text)
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
        Dim myNewSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListName(skillName)

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

    Public Shared Function GetImmediateSkillReqs(ByVal qPilot As EveHQ.Core.Pilot, ByVal skillID As String) As String
        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
        Dim strReqs As String = ""
        Dim subSkill As EveHQ.Core.EveSkill
        For Each subSkillID As String In cSkill.PreReqSkills.Keys
            subSkill = EveHQ.Core.HQ.SkillListID(subSkillID)
            strReqs &= subSkill.Name & cSkill.PreReqSkills(subSkillID)
            If qPilot.PilotSkills.Contains(subSkill.Name) Then
                Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                strReqs &= CType(qPilot.PilotSkills(subSkill.Name), EveHQ.Core.PilotSkill).Level.ToString.Trim
            Else
                strReqs &= "0"
            End If
            strReqs &= ControlChars.CrLf
        Next
        Return strReqs.Trim
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
        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
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
                subSkill = EveHQ.Core.HQ.SkillListID(subSkillID)
                If subSkill.ID <> cSkill.ID Then
                    Call GetSkillPreReqs(qPilot, subSkill, cSkill.PreReqSkills(subSkillID), curNode, strReqs)
                End If
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
                subSkill = EveHQ.Core.HQ.SkillListID(subSkillID)
                If subSkill.ID <> newskill.ID Then
                    Call GetSkillPreReqs(qPilot, subSkill, newskill.PreReqSkills(subSkillID), curNode, strReqs)
                End If
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
            sqItem.Pos += 10000
            sorter.Add(Format(sqItem.Pos, "0000#") & sqItem.Key)
        Next
        sorter.Sort()
        Dim newQueue As EveHQ.Core.SkillQueue = CType(aQ.Clone, SkillQueue)
        newQueue.Queue.Clear()
        Dim sqKey As String
        Dim newPos As Integer = 0
        For Each sqItem As String In sorter
            newPos += 1
            sqKey = sqItem.Substring(5)
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
            For Each qItem As EveHQ.Core.SortedQueueItem In qList
                If qItem.IsTraining = False Then
                    'If qItem.Done = False Then
                    Dim newSkill As New EveHQ.Core.SkillQueueItem
                    newSkill.FromLevel = CInt(qItem.FromLevel)
                    newSkill.ToLevel = CInt(qItem.ToLevel)
                    newSkill.Name = qItem.Name
                    newSkill.Key = qItem.Key
                    newSkill.Pos = count
                    newSkill.Priority = qItem.Priority
                    newSkill.Notes = qItem.Notes
                    nQueue.Add(newSkill, newSkill.Key)
                    count += 1
                    'End If
                End If
            Next
            aQueue.Queue = nQueue
        End If
        Return aQueue
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
            Dim myTSkill As EveHQ.Core.SkillQueueItem

            For i As Integer = 1 To qQueue.Queue.Count
                myTSkill = CType(qQueue.Queue(i), SkillQueueItem)
                myskill = EveHQ.Core.HQ.SkillListName(myTSkill.Name)
                fromLevel = myTSkill.FromLevel
                toLevel = myTSkill.ToLevel

                ' Check if we already have the skill and therefore the time taken
                If qPilot.PilotSkills.Contains(myskill.Name) = True Then
                    Dim myCurSkill As EveHQ.Core.PilotSkill = CType(qPilot.PilotSkills(myskill.Name), EveHQ.Core.PilotSkill)
                    curLevel = myCurSkill.Level
                    If curLevel = fromLevel Then
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, -1))
                    Else
                        cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, fromLevel))
                    End If
                Else
                    cTime = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(qPilot, myskill, toLevel, fromLevel))
                End If

                totalTime += cTime

            Next
            ' Add the totaltime and skills to the queue data
            qQueue.QueueTime = totalTime
            Return totalTime
        End If

    End Function

End Class

