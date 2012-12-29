' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
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
Public Class frmNeuralRemap
    Dim iPilot As New EveHQ.Core.Pilot
    Dim nPilot As New EveHQ.Core.Pilot
    Dim Unused As Integer = 0
    Dim cPilotName As String = ""
    Dim cQueueName As String = ""
    Dim SkillQueue As New EveHQ.Core.SkillQueue
    Dim UpdateAllBases As Boolean = False
    Dim CMin, IMin, MMin, PMin, WMin As Integer

    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
            iPilot = CType(EveHQ.Core.HQ.EveHqSettings.Pilots(cPilotName), Core.Pilot)
            Call Me.InitialiseForm()
        End Set
    End Property
    Public Property QueueName() As String
        Get
            Return cQueueName
        End Get
        Set(ByVal value As String)
            cQueueName = value
            SkillQueue = CType(iPilot.TrainingQueues(cQueueName), Core.SkillQueue)
            nPilot.TrainingQueues.Clear()
            Call Me.DisplayQueueInfo()
        End Set
    End Property

    Private Sub frmNeuralRemap_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.InitialiseForm()

    End Sub

    Private Sub InitialiseForm()

        UpdateAllBases = True
        Dim MinAtt As Integer = 17
        Dim MaxAtt As Integer = 27

        Me.Text = "Neural Remapping - " & iPilot.Name
        ' Create a dummy pilot with which to check new attributes & skill queues

        ' Reset NUDs
        nudIBase.Minimum = MinAtt : nudIBase.Maximum = MaxAtt : nudIBase.Value = MinAtt
        nudPBase.Minimum = MinAtt : nudPBase.Maximum = MaxAtt : nudPBase.Value = MinAtt
        nudCBase.Minimum = MinAtt : nudCBase.Maximum = MaxAtt : nudCBase.Value = MinAtt
        nudWBase.Minimum = MinAtt : nudWBase.Maximum = MaxAtt : nudWBase.Value = MinAtt
        nudMBase.Minimum = MinAtt : nudMBase.Maximum = MaxAtt : nudMBase.Value = MinAtt

        nPilot.PilotSkills = iPilot.PilotSkills
        nPilot.SkillPoints = iPilot.SkillPoints
        EveHQ.Core.PilotParseFunctions.LoadKeySkillsForPilot(nPilot)
        nPilot.IAtt = iPilot.IAtt : nPilot.IImplant = iPilot.IImplant : nPilot.IAttT = iPilot.IAttT
        nPilot.PAtt = iPilot.PAtt : nPilot.PImplant = iPilot.PImplant : nPilot.PAttT = iPilot.PAttT
        nPilot.CAtt = iPilot.CAtt : nPilot.CImplant = iPilot.CImplant : nPilot.CAttT = iPilot.CAttT
        nPilot.WAtt = iPilot.WAtt : nPilot.WImplant = iPilot.WImplant : nPilot.WAttT = iPilot.WAttT
        nPilot.MAtt = iPilot.MAtt : nPilot.MImplant = iPilot.MImplant : nPilot.MAttT = iPilot.MAttT

        ' Check for the maximum allowable base units - API errors
        If nPilot.IAtt > MaxAtt Or nPilot.PAtt > MaxAtt Or nPilot.CAtt > MaxAtt Or nPilot.WAtt > MaxAtt Or nPilot.MAtt > MaxAtt Then
            MessageBox.Show("It would appear that your base attributes contain incorrect values. The Neural Remapper cannot continue until these have been resolved.", "Base Attributes Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Set the minimum allowable units
        IMin = MinAtt
        PMin = MinAtt
        CMin = MinAtt
        WMin = MinAtt
        MMin = MinAtt

        ' Set minimums on NUDs
        nudIBase.Minimum = IMin
        nudPBase.Minimum = PMin
        nudCBase.Minimum = CMin
        nudWBase.Minimum = WMin
        nudMBase.Minimum = MMin

        ' Check if any base attributes are less than 5 as this is not permitted under the remapping rules
        Unused = 0
        If nPilot.IAtt < MinAtt Then
            Unused += MinAtt - nPilot.IAtt
            nPilot.IAtt = MinAtt
        End If
        If nPilot.PAtt < MinAtt Then
            Unused += MinAtt - nPilot.PAtt
            nPilot.PAtt = MinAtt
        End If
        If nPilot.CAtt < MinAtt Then
            Unused += MinAtt - nPilot.CAtt
            nPilot.CAtt = MinAtt
        End If
        If nPilot.WAtt < MinAtt Then
            Unused += MinAtt - nPilot.WAtt
            nPilot.WAtt = MinAtt
        End If
        If nPilot.MAtt < MinAtt Then
            Unused += MinAtt - nPilot.MAtt
            nPilot.MAtt = MinAtt
        End If
        ' Now reallocate the unused against larger items
        Dim available As Integer = 0
        If Unused > 0 And nPilot.IAtt > MinAtt Then
            available = nPilot.IAtt - MinAtt
            If available >= Unused Then
                nPilot.IAtt = nPilot.IAtt - Unused
                Unused = 0
            Else
                nPilot.IAtt = MinAtt
                Unused = Unused - available
            End If
        End If
        If Unused > 0 And nPilot.PAtt > MinAtt Then
            available = nPilot.PAtt - MinAtt
            If available >= Unused Then
                nPilot.PAtt = nPilot.PAtt - Unused
                Unused = 0
            Else
                nPilot.PAtt = MinAtt
                Unused = Unused - available
            End If
        End If
        If Unused > 0 And nPilot.CAtt > MinAtt Then
            available = nPilot.CAtt - MinAtt
            If available >= Unused Then
                nPilot.CAtt = nPilot.CAtt - Unused
                Unused = 0
            Else
                nPilot.CAtt = MinAtt
                Unused = Unused - available
            End If
        End If
        If Unused > 0 And nPilot.WAtt > MinAtt Then
            available = nPilot.WAtt - MinAtt
            If available >= Unused Then
                nPilot.WAtt = nPilot.WAtt - Unused
                Unused = 0
            Else
                nPilot.WAtt = MinAtt
                Unused = Unused - available
            End If
        End If
        If Unused > 0 And nPilot.MAtt > MinAtt Then
            available = nPilot.MAtt - MinAtt
            If available >= Unused Then
                nPilot.MAtt = nPilot.MAtt - Unused
                Unused = 0
            Else
                nPilot.MAtt = MinAtt
                Unused = Unused - available
            End If
        End If

        Call RecalcAttributes()
        Call DisplayAtributes()

        UpdateAllBases = False

    End Sub
    Private Sub RecalcAttributes()
        nPilot.WAttT = nPilot.WAtt + nPilot.WImplant
        nPilot.CAttT = nPilot.CAtt + nPilot.CImplant
        nPilot.IAttT = nPilot.IAtt + nPilot.IImplant
        nPilot.MAttT = nPilot.MAtt + nPilot.MImplant
        nPilot.PAttT = nPilot.PAtt + nPilot.PImplant
        Unused = 99 - (nPilot.IAtt + nPilot.PAtt + nPilot.CAtt + nPilot.WAtt + nPilot.MAtt)
    End Sub
    Private Sub DisplayAtributes()
        ' Display Intelligence Info
        lblIntelligence.Text = "Intelligence (Default Base: " & iPilot.IAtt.ToString & ")"
        nudIBase.Value = nPilot.IAtt
        lblIImplant.Text = "Implant: " & nPilot.IImplant.ToString
        lblITotal.Text = "Total: " & nPilot.IAttT.ToString

        ' Display Perception Info
        lblPerception.Text = "Perception (Default Base: " & iPilot.PAtt.ToString & ")"
        nudPBase.Value = nPilot.PAtt
        lblPImplant.Text = "Implant: " & nPilot.PImplant.ToString
        lblPTotal.Text = "Total: " & nPilot.PAttT.ToString

        ' Display Charisma Info
        lblCharisma.Text = "Charisma (Default Base: " & iPilot.CAtt.ToString & ")"
        nudCBase.Value = nPilot.CAtt
        lblCImplant.Text = "Implant: " & nPilot.CImplant.ToString
        lblCTotal.Text = "Total: " & nPilot.CAttT.ToString

        ' Display Willpower Info
        lblWillpower.Text = "Willpower (Default Base: " & iPilot.WAtt.ToString & ")"
        nudWBase.Value = nPilot.WAtt
        lblWImplant.Text = "Implant: " & nPilot.WImplant.ToString
        lblWTotal.Text = "Total: " & nPilot.WAttT.ToString

        ' Display Memory Info
        lblMemory.Text = "Memory (Default Base: " & iPilot.MAtt.ToString & ")"
        nudMBase.Value = nPilot.MAtt
        lblMImplant.Text = "Implant: " & nPilot.MImplant.ToString
        lblMTotal.Text = "Total: " & nPilot.MAttT.ToString

        ' Display remaining attribute points
        lblUnusedPoints.Text = Unused.ToString

    End Sub

    Private Sub DisplayQueueInfo()
        If cQueueName <> "" Then
            ' Display basic queue name and time remaining
            lblActiveSkillQueue.Text = "No Queue Selected"
            lblSkillQueuePointsAnalysis.Text = "Skill Queue Points Analysis"
            lblActiveSkillQueue.Text = SkillQueue.Name
            Dim iQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(iPilot, CType(iPilot.TrainingQueues(cQueueName), Core.SkillQueue), False, True)
            Dim iTime As Long = CType(iPilot.TrainingQueues(cQueueName), Core.SkillQueue).QueueTime
            If CType(iPilot.TrainingQueues(cQueueName), Core.SkillQueue).IncCurrentTraining = True Then
                iTime += iPilot.TrainingCurrentTime
            End If
            lblActiveQueueTime.Text = "Time Remaining: " & EveHQ.Core.SkillFunctions.TimeToString(iTime)
            If nPilot.TrainingQueues.ContainsKey(cQueueName) = False Then
                nPilot.TrainingQueues.Add(cQueueName, SkillQueue.Clone)
            End If
            Call Me.SyncTraining()
            ' Add the pilot training info!
            Dim nQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(nPilot, CType(nPilot.TrainingQueues(cQueueName), Core.SkillQueue), False, True)
            Dim nTime As Long = CType(nPilot.TrainingQueues(cQueueName), Core.SkillQueue).QueueTime
            If CType(nPilot.TrainingQueues(cQueueName), Core.SkillQueue).IncCurrentTraining = True Then
                If iPilot.CAttT = nPilot.CAttT And iPilot.IAttT = nPilot.IAttT And iPilot.MAttT = nPilot.MAttT And iPilot.PAttT = nPilot.PAttT And iPilot.WAttT = nPilot.WAttT Then
                    nTime += nPilot.TrainingCurrentTime
                Else
                    If EveHQ.Core.HQ.SkillListID.ContainsKey(nPilot.TrainingSkillID) = True Then
                        Dim mySkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(nPilot.TrainingSkillID)
                        Dim ctime As Integer = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(nPilot, mySkill, nPilot.TrainingSkillLevel, -1))
                        If Math.Abs(ctime - nPilot.TrainingCurrentTime) > 5 Then
                            nTime += CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(nPilot, mySkill, nPilot.TrainingSkillLevel, -1))
                        Else
                            nTime += nPilot.TrainingCurrentTime
                        End If
                    End If
                End If
            End If
            lblRevisedQueueTime.Text = "Revised Time: " & EveHQ.Core.SkillFunctions.TimeToString(nTime)
            Dim pointScores(4, 1) As Long
            For a As Integer = 0 To 4
                pointScores(a, 0) = a
            Next
            For Each skill As EveHQ.Core.SortedQueueItem In nQueue
                Select Case skill.PAtt
                    Case "Charisma"
                        pointScores(0, 1) += CLng(skill.SPTrained) * 2
                    Case "Intelligence"
                        pointScores(1, 1) += CLng(skill.SPTrained) * 2
                    Case "Memory"
                        pointScores(2, 1) += CLng(skill.SPTrained) * 2
                    Case "Perception"
                        pointScores(3, 1) += CLng(skill.SPTrained) * 2
                    Case "Willpower"
                        pointScores(4, 1) += CLng(skill.SPTrained) * 2
                End Select
                Select Case skill.SAtt
                    Case "Charisma"
                        pointScores(0, 1) += CLng(skill.SPTrained)
                    Case "Intelligence"
                        pointScores(1, 1) += CLng(skill.SPTrained)
                    Case "Memory"
                        pointScores(2, 1) += CLng(skill.SPTrained)
                    Case "Perception"
                        pointScores(3, 1) += CLng(skill.SPTrained)
                    Case "Willpower"
                        pointScores(4, 1) += CLng(skill.SPTrained)
                End Select
            Next
            ' Sort the list
            ' Create a tag array ready to sort the skill times
            Dim tagArray(4) As Integer
            For a As Integer = 0 To 4
                tagArray(a) = a
            Next
            ' Initialize the comparer and sort
            Dim myComparer As New EveHQ.Core.Reports.RectangularComparer(pointScores)
            Array.Sort(tagArray, myComparer)
            Array.Reverse(tagArray)
            For att As Integer = 0 To 4
                Select Case tagArray(att)
                    Case 0
                        Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Charisma:"
                    Case 1
                        Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Intelligence:"
                    Case 2
                        Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Memory:"
                    Case 3
                        Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Perception:"
                    Case 4
                        Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Willpower:"
                End Select
                Me.gpSkillQueue.Controls("lblAttributePoints" & (att + 1).ToString).Text = pointScores(tagArray(att), 1).ToString("N2")
            Next
            If nTime <= iTime Then
                lblTimeSaving.Text = "Time Saving: " & EveHQ.Core.SkillFunctions.TimeToString(iTime - nTime, False)
            Else
                lblTimeSaving.Text = "Time Loss: " & EveHQ.Core.SkillFunctions.TimeToString(nTime - iTime, False)
            End If
            btnOptimise.Enabled = True
        Else
            If Me.IsHandleCreated = True Then
                lblActiveSkillQueue.Text = "No Queue Selected"
                lblSkillQueuePointsAnalysis.Text = ""
                For att As Integer = 0 To 4
                    Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = ""
                    Me.gpSkillQueue.Controls("lblAttributePoints" & (att + 1).ToString).Text = ""
                Next
                lblActiveQueueTime.Text = ""
                lblRevisedQueueTime.Text = ""
                lblTimeSaving.Text = ""
                btnOptimise.Enabled = False
            End If
        End If
    End Sub

    Private Sub SyncTraining()
        nPilot.Training = iPilot.Training
        nPilot.TrainingCurrentSP = iPilot.TrainingCurrentSP
        nPilot.TrainingCurrentTime = iPilot.TrainingCurrentTime
        nPilot.TrainingSkillID = iPilot.TrainingSkillID
        nPilot.TrainingSkillName = iPilot.TrainingSkillName
        nPilot.TrainingSkillLevel = iPilot.TrainingSkillLevel
        nPilot.TrainingStartSP = iPilot.TrainingStartSP
        nPilot.TrainingEndSP = iPilot.TrainingEndSP
        nPilot.TrainingStartTime = iPilot.TrainingStartTime
        nPilot.TrainingEndTime = iPilot.TrainingEndTime
    End Sub

    Private Sub nudIBase_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIBase.ValueChanged
        If UpdateAllBases = False Then
            If nPilot.IAtt - nudIBase.Value + Unused >= 0 Then
                nPilot.IAtt = CInt(nudIBase.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nudIBase.Value = nPilot.IAtt
            End If
        Else
            nPilot.IAtt = CInt(nudIBase.Value)
        End If
    End Sub

    Private Sub nudPBase_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPBase.ValueChanged
        If UpdateAllBases = False Then
            If nPilot.PAtt - nudPBase.Value + Unused >= 0 Then
                nPilot.PAtt = CInt(nudPBase.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nudPBase.Value = nPilot.PAtt
            End If
        Else
            nPilot.PAtt = CInt(nudPBase.Value)
        End If
    End Sub

    Private Sub nudCBase_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCBase.ValueChanged
        If UpdateAllBases = False Then
            If nPilot.CAtt - nudCBase.Value + Unused >= 0 Then
                nPilot.CAtt = CInt(nudCBase.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nudCBase.Value = nPilot.CAtt
            End If
        Else
            nPilot.CAtt = CInt(nudCBase.Value)
        End If
    End Sub

    Private Sub nudWBase_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudWBase.ValueChanged
        If UpdateAllBases = False Then
            If nPilot.WAtt - nudWBase.Value + Unused >= 0 Then
                nPilot.WAtt = CInt(nudWBase.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nudWBase.Value = nPilot.WAtt
            End If
        Else
            nPilot.WAtt = CInt(nudWBase.Value)
        End If
    End Sub

    Private Sub nudMBase_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMBase.ValueChanged
        If UpdateAllBases = False Then
            If nPilot.MAtt - nudMBase.Value + Unused >= 0 Then
                nPilot.MAtt = CInt(nudMBase.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nudMBase.Value = nPilot.MAtt
            End If
        Else
            nPilot.MAtt = CInt(nudMBase.Value)
        End If
    End Sub

    Private Sub btnOptimise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptimise.Click
        Me.Cursor = Cursors.WaitCursor
        Dim bestTime As Long = SkillQueue.QueueTime * 2
        Dim calcTime As Long = 0
        Dim count As Long = 0
        Dim bestI As Integer = nPilot.IAtt
        Dim bestP As Integer = nPilot.PAtt
        Dim bestC As Integer = nPilot.CAtt
        Dim bestW As Integer = nPilot.WAtt
        Dim bestM As Integer = nPilot.MAtt
        Dim MaxAvailable As Integer = 14 ' Maximum distributable points (39 - (5 x 5))
        Dim maxAtt As Integer = 27 ' Max = 27

        For IAtt As Integer = IMin To maxAtt
            nPilot.IAtt = IAtt
            For PAtt As Integer = PMin To maxAtt
                nPilot.PAtt = PAtt
                For CAtt As Integer = CMin To maxAtt
                    nPilot.CAtt = CAtt
                    For WAtt As Integer = WMin To maxAtt
                        nPilot.WAtt = WAtt
                        For MAtt As Integer = PMin To maxAtt
                            nPilot.MAtt = MAtt
                            If IAtt + PAtt + CAtt + WAtt + MAtt = 99 Then
                                Call RecalcAttributes()
                                calcTime = EveHQ.Core.SkillQueueFunctions.GetQueueTime(nPilot, CType(nPilot.TrainingQueues(cQueueName), Core.SkillQueue))
                                If calcTime <= bestTime Then
                                    bestTime = calcTime
                                    bestI = IAtt
                                    bestP = PAtt
                                    bestC = CAtt
                                    bestW = WAtt
                                    bestM = MAtt
                                End If
                            End If
                        Next
                    Next
                Next
            Next
        Next
        UpdateAllBases = True
        nPilot.IAtt = bestI
        nPilot.PAtt = bestP
        nPilot.CAtt = bestC
        nPilot.WAtt = bestW
        nPilot.MAtt = bestM
        Call RecalcAttributes()
        Call DisplayAtributes()
        Call DisplayQueueInfo()
        UpdateAllBases = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Call Me.InitialiseForm()
        Call Me.DisplayQueueInfo()
    End Sub
End Class