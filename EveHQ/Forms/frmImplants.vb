Public Class frmImplants
    Dim iPilot As EveHQ.Core.Pilot = EveHQ.Core.HQ.myPilot
    Dim nPilot As New EveHQ.Core.Pilot
    Dim Unused As Integer = 0
    Dim cPilotName As String = ""
    Dim cQueueName As String = ""
    Dim SkillQueue As New EveHQ.Core.SkillQueue
    Dim UpdateAllBases As Boolean = False

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
        Me.Text = "Implants - " & iPilot.Name
        ' Create a dummy pilot with which to check new attributes & skill queues

        nPilot.PilotSkills = iPilot.PilotSkills
        nPilot.SkillPoints = iPilot.SkillPoints
        EveHQ.Core.PilotParseFunctions.LoadKeySkillsForPilot(nPilot)
        nPilot.IAtt = iPilot.IAtt : nPilot.IImplant = iPilot.IImplant : nPilot.LIAtt = iPilot.LIAtt : nPilot.ALIAtt = iPilot.ALIAtt : nPilot.LSIAtt = iPilot.LSIAtt : nPilot.IAttT = iPilot.IAttT
        nPilot.PAtt = iPilot.PAtt : nPilot.PImplant = iPilot.PImplant : nPilot.LPAtt = iPilot.LPAtt : nPilot.ALPAtt = iPilot.ALPAtt : nPilot.LSPAtt = iPilot.LSPAtt : nPilot.PAttT = iPilot.PAttT
        nPilot.CAtt = iPilot.CAtt : nPilot.CImplant = iPilot.CImplant : nPilot.LCAtt = iPilot.LCAtt : nPilot.ALCAtt = iPilot.ALCAtt : nPilot.LSCAtt = iPilot.LSCAtt : nPilot.CAttT = iPilot.CAttT
        nPilot.WAtt = iPilot.WAtt : nPilot.WImplant = iPilot.WImplant : nPilot.LWAtt = iPilot.LWAtt : nPilot.ALWAtt = iPilot.ALWAtt : nPilot.LSWAtt = iPilot.LSWAtt : nPilot.WAttT = iPilot.WAttT
        nPilot.MAtt = iPilot.MAtt : nPilot.MImplant = iPilot.MImplant : nPilot.LMAtt = iPilot.LMAtt : nPilot.ALMAtt = iPilot.ALMAtt : nPilot.LSMAtt = iPilot.LSMAtt : nPilot.MAttT = iPilot.MAttT

        Call RecalcAttributes()
        Call DisplayAtributes()
    End Sub
    Private Sub RecalcAttributes()
        ' Get learning skill details
        Dim learningFactor As Integer = 0
        If nPilot.PilotSkills.Contains("Learning") = True Then
            learningFactor = CType(nPilot.PilotSkills("Learning"), EveHQ.Core.Skills).Level
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
        lblIBase.Text = nPilot.IAtt.ToString
        nudIImplant.Value = nPilot.IImplant
        lblIBase.Text = "Implant: " & nPilot.IImplant.ToString
        lblISkills.Text = "Skills: " & (nPilot.LIAtt + nPilot.ALIAtt + nPilot.LSIAtt).ToString
        lblITotal.Text = "Total: " & nPilot.IAttT.ToString

        ' Display Perception Info
        nudPImplant.Value = nPilot.PImplant
        lblPBase.Text = nPilot.PAtt.ToString
        lblPBase.Text = "Implant: " & nPilot.PImplant.ToString
        lblPSkills.Text = "Skills: " & (nPilot.LPAtt + nPilot.ALPAtt + nPilot.LSPAtt).ToString
        lblPTotal.Text = "Total: " & nPilot.PAttT.ToString

        ' Display Charisma Info
        nudCImplant.Value = nPilot.CImplant
        lblCBase.Text = nPilot.CAtt.ToString
        lblCBase.Text = "Implant: " & nPilot.CImplant.ToString
        lblCSkills.Text = "Skills: " & (nPilot.LCAtt + nPilot.ALCAtt + nPilot.LSCAtt).ToString
        lblCTotal.Text = "Total: " & nPilot.CAttT.ToString

        ' Display Willpower Info
        nudWImplant.Value = nPilot.WImplant
        lblWBase.Text = nPilot.WAtt.ToString
        lblWBase.Text = "Implant: " & nPilot.WImplant.ToString
        lblWSkills.Text = "Skills: " & (nPilot.LWAtt + nPilot.ALWAtt + nPilot.LSWAtt).ToString
        lblWTotal.Text = "Total: " & nPilot.WAttT.ToString

        ' Display Memory Info
        nudMImplant.Value = nPilot.MImplant
        lblMBase.Text = nPilot.MAtt.ToString
        lblMBase.Text = "Implant: " & nPilot.MImplant.ToString
        lblMSkills.Text = "Skills: " & (nPilot.LMAtt + nPilot.ALMAtt + nPilot.LSMAtt).ToString
        lblMTotal.Text = "Total: " & nPilot.MAttT.ToString

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

    Private Sub nudIImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIImplant.ValueChanged
        If UpdateAllBases = False Then
            nPilot.IImplant = CInt(nudIImplant.Value)
            Call RecalcAttributes()
            Call DisplayAtributes()
            Call DisplayQueueInfo()
        Else
            nPilot.IImplant = CInt(nudIImplant.Value)
        End If
    End Sub

    Private Sub nudPImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPImplant.ValueChanged
        If UpdateAllBases = False Then
            nPilot.PImplant = CInt(nudPImplant.Value)
            Call RecalcAttributes()
            Call DisplayAtributes()
            Call DisplayQueueInfo()
        Else
            nPilot.PImplant = CInt(nudPImplant.Value)
        End If
    End Sub

    Private Sub nudCImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCImplant.ValueChanged
        If UpdateAllBases = False Then
            nPilot.CImplant = CInt(nudCImplant.Value)
            Call RecalcAttributes()
            Call DisplayAtributes()
            Call DisplayQueueInfo()
        Else
            nPilot.CImplant = CInt(nudCImplant.Value)
        End If
    End Sub

    Private Sub nudWImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudWImplant.ValueChanged
        If UpdateAllBases = False Then
            nPilot.WImplant = CInt(nudWImplant.Value)
            Call RecalcAttributes()
            Call DisplayAtributes()
            Call DisplayQueueInfo()
        Else
            nPilot.WImplant = CInt(nudWImplant.Value)
        End If
    End Sub

    Private Sub nudMImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMImplant.ValueChanged
        If UpdateAllBases = False Then
            nPilot.MImplant = CInt(nudMImplant.Value)
            Call RecalcAttributes()
            Call DisplayAtributes()
            Call DisplayQueueInfo()
        Else
            nPilot.MImplant = CInt(nudMImplant.Value)
        End If
    End Sub

    Private Sub btnResetImplants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetImplants.Click
        UpdateAllBases = True
        nPilot.IImplant = iPilot.IImplant
        nPilot.PImplant = iPilot.PImplant
        nPilot.CImplant = iPilot.CImplant
        nPilot.WImplant = iPilot.WImplant
        nPilot.MImplant = iPilot.MImplant
        Call RecalcAttributes()
        Call DisplayAtributes()
        Call DisplayQueueInfo()
        UpdateAllBases = False
    End Sub
End Class