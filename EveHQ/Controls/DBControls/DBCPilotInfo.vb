Public Class DBCPilotInfo

#Region "Control Variables"
    Dim cPilot As EveHQ.Core.Pilot
#End Region

#Region "Control Properties"

    Public ReadOnly Property ControlName() As String
        Get
            Return "PilotInfo"
        End Get
    End Property

    Dim cDefaultPilotName As String
    Public Property DefaultPilotName() As String
        Get
            Return cDefaultPilotName
        End Get
        Set(ByVal value As String)
            cDefaultPilotName = value
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(DefaultPilotName) Then
                cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(DefaultPilotName), Core.Pilot)
            End If
            If cboPilot.Items.Contains(DefaultPilotName) = True Then cboPilot.SelectedItem = DefaultPilotName
            Me.SetConfig("DefaultPilotName", value)
        End Set
    End Property

    Dim cControlWidth As Integer
    Public Property ControlWidth() As Integer
        Get
            Return cControlWidth
        End Get
        Set(ByVal value As Integer)
            cControlWidth = value
            Me.Width = cControlWidth
            Me.SetConfig("ControlWidth", value)
        End Set
    End Property

    Dim cControlHeight As Integer
    Public Property ControlHeight() As Integer
        Get
            Return cControlHeight
        End Get
        Set(ByVal value As Integer)
            cControlHeight = value
            Me.Height = cControlHeight
            Me.SetConfig("ControlHeight", value)
        End Set
    End Property

    Dim cControlPosition As Integer
    Public Property ControlPosition() As Integer
        Get
            Return cControlPosition
        End Get
        Set(ByVal value As Integer)
            cControlPosition = value
            Me.SetConfig("ControlPosition", value)
        End Set
    End Property

    Dim cControlConfig As New SortedList(Of String, Object)
    Public Property ControlConfiguration() As SortedList(Of String, Object)
        Get
            Return cControlConfig
        End Get
        Set(ByVal value As SortedList(Of String, Object))
            cControlConfig = value
            Call Me.ReadFromConfig()
        End Set
    End Property

#End Region

#Region "Control Configuration"
    Private Sub ReadFromConfig()
        For Each ConfigProperty As String In cControlConfig.Keys
            Dim pi As System.Reflection.PropertyInfo = Me.GetType().GetProperty(ConfigProperty)
            'pi.SetValue(Me, CType(cControlConfig(ConfigProperty), Object), Nothing)
            pi.SetValue(Me, Convert.ChangeType(cControlConfig(ConfigProperty), pi.PropertyType, Globalization.CultureInfo.InvariantCulture), Nothing)
            'Select Case ConfigProperty
            '    Case "DefaultPilotName"
            '        Me.DefaultPilotName = CStr(cControlConfig(ConfigProperty))
            '    Case "ControlHeight"
            '        Me.ControlHeight = CInt(cControlConfig(ConfigProperty))
            '    Case "ControlWidth"
            '        Me.ControlWidth = CInt(cControlConfig(ConfigProperty))
            '    Case "ControlPosition"
            '        Me.ControlPosition = CInt(cControlConfig(ConfigProperty))
            'End Select
        Next
    End Sub
    Private Sub SetConfig(ByVal ConfigProperty As String, ByVal ConfigData As Object)
        If cControlConfig.ContainsKey(ConfigProperty) = False Then
            cControlConfig.Add(ConfigProperty, ConfigData)
        Else
            cControlConfig(ConfigProperty) = ConfigData
        End If
    End Sub
#End Region

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Update the AGPs with the configured details
        AGPHeader.BorderColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCBorderColor))
        AGPHeader.CornerRadius = 10
        AGPPilotInfo.BorderColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCBorderColor))
        AGPPilotInfo.CornerRadius = 10
        AGPPilotInfo.Colors(0).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCMainColor1))
        AGPPilotInfo.Colors(1).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCMainColor2))
        AGPHeader.Colors(0).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCHeadColor1))
        AGPHeader.Colors(1).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCHeadColor2))

        ' Load the combo box with the pilot info
        cboPilot.BeginUpdate()
        cboPilot.Items.Clear()
        For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If pilot.Active = True Then
                cboPilot.Items.Add(pilot.Name)
            End If
        Next
        cboPilot.EndUpdate()

    End Sub

    Public Sub UpdateColours()
        AGPHeader.BorderColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCBorderColor))
        AGPHeader.CornerRadius = 10
        AGPPilotInfo.BorderColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCBorderColor))
        AGPPilotInfo.CornerRadius = 10
        AGPPilotInfo.Colors(0).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCMainColor1))
        AGPPilotInfo.Colors(1).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCMainColor2))
        AGPHeader.Colors(0).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCHeadColor1))
        AGPHeader.Colors(1).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCHeadColor2))
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

    Private Sub pbConfig_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbConfig.MouseDoubleClick
        ' Initialise the configuration form
        Dim newConfigForm As New DBCPilotInfoConfig
        newConfigForm.DBControl = Me
        newConfigForm.ShowDialog()
    End Sub

End Class
