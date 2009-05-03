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
            iPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cPilotName), Core.Pilot)
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

        Me.Text = "Neural Remapping - " & iPilot.Name
        ' Create a dummy pilot with which to check new attributes & skill queues

        ' Reset NUDs
        nudIBase.Value = 5
        nudPBase.Value = 5
        nudCBase.Value = 5
        nudWBase.Value = 5
        nudMBase.Value = 5

        nPilot.PilotSkills = iPilot.PilotSkills
        nPilot.SkillPoints = iPilot.SkillPoints
        EveHQ.Core.PilotParseFunctions.LoadKeySkillsForPilot(nPilot)
        nPilot.IAtt = iPilot.IAtt : nPilot.IImplant = iPilot.IImplant : nPilot.LIAtt = iPilot.LIAtt : nPilot.ALIAtt = iPilot.ALIAtt : nPilot.LSIAtt = iPilot.LSIAtt : nPilot.IAttT = iPilot.IAttT
        nPilot.PAtt = iPilot.PAtt : nPilot.PImplant = iPilot.PImplant : nPilot.LPAtt = iPilot.LPAtt : nPilot.ALPAtt = iPilot.ALPAtt : nPilot.LSPAtt = iPilot.LSPAtt : nPilot.PAttT = iPilot.PAttT
        nPilot.CAtt = iPilot.CAtt : nPilot.CImplant = iPilot.CImplant : nPilot.LCAtt = iPilot.LCAtt : nPilot.ALCAtt = iPilot.ALCAtt : nPilot.LSCAtt = iPilot.LSCAtt : nPilot.CAttT = iPilot.CAttT
        nPilot.WAtt = iPilot.WAtt : nPilot.WImplant = iPilot.WImplant : nPilot.LWAtt = iPilot.LWAtt : nPilot.ALWAtt = iPilot.ALWAtt : nPilot.LSWAtt = iPilot.LSWAtt : nPilot.WAttT = iPilot.WAttT
        nPilot.MAtt = iPilot.MAtt : nPilot.MImplant = iPilot.MImplant : nPilot.LMAtt = iPilot.LMAtt : nPilot.ALMAtt = iPilot.ALMAtt : nPilot.LSMAtt = iPilot.LSMAtt : nPilot.MAttT = iPilot.MAttT

        ' Check for the maximum allowable base units - API errors
        If nPilot.IAtt > 15 Or nPilot.PAtt > 15 Or nPilot.CAtt > 15 Or nPilot.WAtt > 15 Or nPilot.MAtt > 15 Then
            MessageBox.Show("It would appear that your base attributes contain incorrect values. The Neural Remapper cannot continue until these have been resolved.", "Base Attributes Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Set the minimum allowable units
        IMin = 5
        PMin = 5
        CMin = 5
        WMin = 5
        MMin = 5

        ' Set minimums on NUDs
        nudIBase.Minimum = IMin
        nudPBase.Minimum = PMin
        nudCBase.Minimum = CMin
        nudWBase.Minimum = WMin
        nudMBase.Minimum = MMin

        ' Check if any base attributes are less than 5 as this is not permitted under the remapping rules
        Unused = 0
        If nPilot.IAtt < 5 Then
            Unused += 5 - nPilot.IAtt
            nPilot.IAtt = 5
        End If
        If nPilot.PAtt < 5 Then
            Unused += 5 - nPilot.PAtt
            nPilot.PAtt = 5
        End If
        If nPilot.CAtt < 5 Then
            Unused += 5 - nPilot.CAtt
            nPilot.CAtt = 5
        End If
        If nPilot.WAtt < 5 Then
            Unused += 5 - nPilot.WAtt
            nPilot.WAtt = 5
        End If
        If nPilot.MAtt < 5 Then
            Unused += 5 - nPilot.MAtt
            nPilot.MAtt = 5
        End If
        ' Now reallocate the unused against larger items
        Dim available As Integer = 0
        If Unused > 0 And nPilot.IAtt > 5 Then
            available = nPilot.IAtt - 5
            If available >= Unused Then
                nPilot.IAtt = nPilot.IAtt - Unused
                Unused = 0
            Else
                nPilot.IAtt = 5
                Unused = Unused - available
            End If
        End If
        If Unused > 0 And nPilot.PAtt > 5 Then
            available = nPilot.PAtt - 5
            If available >= Unused Then
                nPilot.PAtt = nPilot.PAtt - Unused
                Unused = 0
            Else
                nPilot.PAtt = 5
                Unused = Unused - available
            End If
        End If
        If Unused > 0 And nPilot.CAtt > 5 Then
            available = nPilot.CAtt - 5
            If available >= Unused Then
                nPilot.CAtt = nPilot.CAtt - Unused
                Unused = 0
            Else
                nPilot.CAtt = 5
                Unused = Unused - available
            End If
        End If
        If Unused > 0 And nPilot.WAtt > 5 Then
            available = nPilot.WAtt - 5
            If available >= Unused Then
                nPilot.WAtt = nPilot.WAtt - Unused
                Unused = 0
            Else
                nPilot.WAtt = 5
                Unused = Unused - available
            End If
        End If
        If Unused > 0 And nPilot.MAtt > 5 Then
            available = nPilot.MAtt - 5
            If available >= Unused Then
                nPilot.MAtt = nPilot.MAtt - Unused
                Unused = 0
            Else
                nPilot.MAtt = 5
                Unused = Unused - available
            End If
        End If

        Call RecalcAttributes()
        Call DisplayAtributes()

        UpdateAllBases = False

    End Sub
    Private Sub RecalcAttributes()
        ' Get learning skill details
        Dim learningFactor As Integer = 0
        If nPilot.PilotSkills.Contains("Learning") = True Then
            learningFactor = CType(nPilot.PilotSkills("Learning"), EveHQ.Core.PilotSkill).Level
        End If

        ' Calculate learning skill increase
        Dim WT, CT, IT, MT, PT As Double
        WT = nPilot.WAtt + nPilot.LWAtt + nPilot.ALWAtt + nPilot.WImplant
        nPilot.LSWAtt = WT * (learningFactor / 50)
        nPilot.WAttT = WT + nPilot.LSWAtt

        CT = nPilot.CAtt + nPilot.LCAtt + nPilot.ALCAtt + nPilot.CImplant
        nPilot.LSCAtt = CT * (learningFactor / 50)
        nPilot.CAttT = CT + nPilot.LSCAtt

        IT = nPilot.IAtt + nPilot.LIAtt + nPilot.ALIAtt + nPilot.IImplant
        nPilot.LSIAtt = IT * (learningFactor / 50)
        nPilot.IAttT = IT + nPilot.LSIAtt

        MT = nPilot.MAtt + nPilot.LMAtt + nPilot.ALMAtt + nPilot.MImplant
        nPilot.LSMAtt = MT * (learningFactor / 50)
        nPilot.MAttT = MT + nPilot.LSMAtt

        PT = nPilot.PAtt + nPilot.LPAtt + nPilot.ALPAtt + nPilot.PImplant
        nPilot.LSPAtt = PT * (learningFactor / 50)
        nPilot.PAttT = PT + nPilot.LSPAtt

        Unused = 39 - (nPilot.IAtt + nPilot.PAtt + nPilot.CAtt + nPilot.WAtt + nPilot.MAtt)

    End Sub
    Private Sub DisplayAtributes()
        ' Display Intelligence Info
        lblIntelligence.Text = "Intelligence (Default Base: " & iPilot.IAtt.ToString & ")"
        nudIBase.Value = nPilot.IAtt
        lblIImplant.Text = "Implant: " & nPilot.IImplant.ToString
        lblISkills.Text = "Skills: " & (nPilot.LIAtt + nPilot.ALIAtt + nPilot.LSIAtt).ToString
        lblITotal.Text = "Total: " & nPilot.IAttT.ToString

        ' Display Perception Info
        lblPerception.Text = "Perception (Default Base: " & iPilot.PAtt.ToString & ")"
        nudPBase.Value = nPilot.PAtt
        lblPImplant.Text = "Implant: " & nPilot.PImplant.ToString
        lblPSkills.Text = "Skills: " & (nPilot.LPAtt + nPilot.ALPAtt + nPilot.LSPAtt).ToString
        lblPTotal.Text = "Total: " & nPilot.PAttT.ToString

        ' Display Charisma Info
        lblCharisma.Text = "Charisma (Default Base: " & iPilot.CAtt.ToString & ")"
        nudCBase.Value = nPilot.CAtt
        lblCImplant.Text = "Implant: " & nPilot.CImplant.ToString
        lblCSkills.Text = "Skills: " & (nPilot.LCAtt + nPilot.ALCAtt + nPilot.LSCAtt).ToString
        lblCTotal.Text = "Total: " & nPilot.CAttT.ToString

        ' Display Willpower Info
        lblWillpower.Text = "Willpower (Default Base: " & iPilot.WAtt.ToString & ")"
        nudWBase.Value = nPilot.WAtt
        lblWImplant.Text = "Implant: " & nPilot.WImplant.ToString
        lblWSkills.Text = "Skills: " & (nPilot.LWAtt + nPilot.ALWAtt + nPilot.LSWAtt).ToString
        lblWTotal.Text = "Total: " & nPilot.WAttT.ToString

        ' Display Memory Info
        lblMemory.Text = "Memory (Default Base: " & iPilot.MAtt.ToString & ")"
        nudMBase.Value = nPilot.MAtt
        lblMImplant.Text = "Implant: " & nPilot.MImplant.ToString
        lblMSkills.Text = "Skills: " & (nPilot.LMAtt + nPilot.ALMAtt + nPilot.LSMAtt).ToString
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
            Dim iQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(iPilot, CType(iPilot.TrainingQueues(cQueueName), Core.SkillQueue))
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
            Dim nQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(nPilot, CType(nPilot.TrainingQueues(cQueueName), Core.SkillQueue))
            Dim nTime As Long = CType(nPilot.TrainingQueues(cQueueName), Core.SkillQueue).QueueTime
            If CType(nPilot.TrainingQueues(cQueueName), Core.SkillQueue).IncCurrentTraining = True Then
                nTime += nPilot.TrainingCurrentTime
            End If
            lblRevisedQueueTime.Text = "Revised Time: " & EveHQ.Core.SkillFunctions.TimeToString(nTime)
            Dim pointScores(4, 1) As Long
            For a As Integer = 0 To 4
                pointScores(a, 0) = a
            Next
            For Each skill As EveHQ.Core.SortedQueue In nQueue
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
                        Me.gbSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Charisma:"
                    Case 1
                        Me.gbSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Intelligence:"
                    Case 2
                        Me.gbSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Memory:"
                    Case 3
                        Me.gbSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Perception:"
                    Case 4
                        Me.gbSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Willpower:"
                End Select
                Me.gbSkillQueue.Controls("lblAttributePoints" & (att + 1).ToString).Text = FormatNumber(pointScores(tagArray(att), 1), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
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
                    Me.gbSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = ""
                    Me.gbSkillQueue.Controls("lblAttributePoints" & (att + 1).ToString).Text = ""
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
        Dim bestI As Integer = 0
        Dim bestP As Integer = 0
        Dim bestC As Integer = 0
        Dim bestW As Integer = 0
        Dim bestM As Integer = 0
        Dim MaxAvailable As Integer = 14 ' Maximum distributable points (39 - (5 x 5))
        Dim maxAtt As Integer = 15 ' Max = 15

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
                            If IAtt + PAtt + CAtt + WAtt + MAtt = 39 Then
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