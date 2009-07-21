Public Class DBCPilotInfo

#Region "Control Properties"

    Dim cPilot As EveHQ.Core.Pilot
    Dim cPilotName As String
    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cPilotName) Then
                cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cPilotName), Core.Pilot)
            End If
            If cboPilot.Items.Contains(cPilotName) = True Then cboPilot.SelectedItem = cPilotName
        End Set
    End Property
#End Region

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Update the AGP with the configured details
        AGPPilotInfo.Border = EveHQ.Core.HQ.EveHQSettings.DBCBorder
        AGPPilotInfo.BorderColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCBorderColor))
        AGPPilotInfo.CornerRadius = EveHQ.Core.HQ.EveHQSettings.DBCCornerRadius
        AGPPilotInfo.Colors(0).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCMainColor1))
        AGPPilotInfo.Colors(1).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCMainColor2))

        ' Load the combo box with the pilot info
        cboPilot.BeginUpdate()
        cboPilot.Items.Clear()
        For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            cboPilot.Items.Add(pilot.Name)
        Next
        cboPilot.EndUpdate()

    End Sub

    Private Sub cboPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilot.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilot.SelectedItem.ToString) Then
            cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilot.SelectedItem.ToString), Core.Pilot)
            Call Me.UpdatePilotInfo()
            ' Start the skill timer
            tmrSkill.Enabled = True
            tmrSkill.Start()
        Else
            tmrSkill.Stop()
            tmrSkill.Enabled = False
        End If
    End Sub

    Private Sub UpdatePilotInfo()
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilot.SelectedItem.ToString) Then
            ' Update the info
            Dim imgFilename As String = IO.Path.Combine(EveHQ.Core.HQ.imageCacheFolder, cPilot.ID & ".png")
            If My.Computer.FileSystem.FileExists(imgFilename) = True Then
                AGPPilotInfo.Image = Image.FromFile(imgFilename)
            Else
                AGPPilotInfo.Image = My.Resources.noitem
            End If
            lblCorp.Text = "Member of " & cPilot.Corp
            lblIsk.Text = "Balance: " & FormatNumber(cPilot.Isk, 2)

            Call Me.UpdateTrainingInfo()
        Else
            ' Clear the info
            lblCorp.Text = ""
            lblIsk.Text = ""
            lblSP.Text = ""
            lblTraining.Text = ""
            lblTrainingEnd.Text = ""
            lblTrainingTime.Text = ""
            lblSkillQueueEnd.Text = ""
            lblSkillQueueTime.Text = ""
            AGPPilotInfo.Image = My.Resources.noitem
        End If
    End Sub

    Private Sub UpdateTrainingInfo()
        If cPilot IsNot Nothing Then
            If cPilot.Training = True Then
                ' Training
                lblSP.Text = "Skillpoints: " & FormatNumber(cPilot.SkillPoints + cPilot.TrainingCurrentSP, 0)
                Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cPilot.TrainingEndTime)
                lblTraining.Text = "Training: " & cPilot.TrainingSkillName & " " & EveHQ.Core.SkillFunctions.Roman(cPilot.TrainingSkillLevel)
                lblTrainingEnd.Text = "Training Ends: " & Format(localdate, "ddd") & " " & localdate
                lblTrainingTime.Text = "Time Remaining: " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.TrainingCurrentTime)
            Else
                ' Not training
                lblSP.Text = "Skillpoints: " & FormatNumber(cPilot.SkillPoints, 0)
                lblTraining.Text = "Not currently training"
                lblTrainingEnd.Text = "Training Ends: n/a"
                lblTrainingTime.Text = "Time Remaining: n/a"
            End If
            If cPilot.QueuedSkills IsNot Nothing Then
                If cPilot.QueuedSkills.Count > 0 Then
                    Dim lastSkill As EveHQ.Core.PilotQueuedSkill = cPilot.QueuedSkills(cPilot.QueuedSkills.Keys(cPilot.QueuedSkills.Count - 1))
                    Dim skillDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(lastSkill.EndTime)
                    lblSkillQueueEnd.Text = "Skill Queue Ends: " & Format(skillDate, "ddd") & " " & skillDate
                    lblSkillQueueTime.Text = "Queue Time Remaining: " & EveHQ.Core.SkillFunctions.TimeToString(CType(skillDate - Now, TimeSpan).TotalSeconds)
                Else
                    lblSkillQueueEnd.Text = "Skill Queue Ends: n/a"
                    lblSkillQueueTime.Text = "Queue Time Remaining: n/a"
                End If
            Else
                lblSkillQueueEnd.Text = "Skill Queue Ends: n/a"
                lblSkillQueueTime.Text = "Queue Time Remaining: n/a"
            End If
        End If
    End Sub

    Private Sub tmrSkill_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSkill.Tick
        Call Me.UpdateTrainingInfo()
    End Sub

    Private Sub lblTraining_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblTraining.LinkClicked
        frmTraining.DisplayPilotName = cPilot.Name
        frmEveHQ.OpenSkillTrainingForm()
    End Sub

    Private Sub lblPilot_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblPilot.LinkClicked
        frmPilot.DisplayPilotName = cPilot.Name
        frmEveHQ.OpenPilotInfoForm()
    End Sub
End Class
