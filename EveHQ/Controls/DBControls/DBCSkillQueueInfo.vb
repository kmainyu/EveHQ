Public Class DBCSkillQueueInfo

#Region "Control Variables"
    Dim cPilot As EveHQ.Core.Pilot
    Dim ReadConfig As Boolean = False
#End Region

#Region "Control Properties"

    Public ReadOnly Property ControlName() As String
        Get
            Return "Skill Queue Information"
        End Get
    End Property

    Dim cDefaultPilotName As String = ""
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
            If ReadConfig = False Then
                Me.SetConfig("DefaultPilotName", value)
            End If
        End Set
    End Property

    Dim cDefaultQueueName As String = ""
    Public Property DefaultQueueName() As String
        Get
            Return cDefaultQueueName
        End Get
        Set(ByVal value As String)
            cDefaultQueueName = value
            If cboSkillQueue.Items.Contains(DefaultQueueName) = True Then cboSkillQueue.SelectedItem = DefaultQueueName
            If ReadConfig = False Then
                Me.SetConfig("DefaultQueueName", value)
            End If
        End Set
    End Property

    Dim cEveQueue As Boolean = True
    Public Property EveQueue() As Boolean
        Get
            Return cEveQueue
        End Get
        Set(ByVal value As Boolean)
            cEveQueue = value
            If value = True Then
                radEveQueue.Checked = True
            Else
                radEveHQQueue.Checked = True
            End If
            If ReadConfig = False Then
                Me.SetConfig("EveQueue", value)
            End If
        End Set
    End Property

    Dim cControlWidth As Integer = 300
    Public Property ControlWidth() As Integer
        Get
            Return cControlWidth
        End Get
        Set(ByVal value As Integer)
            cControlWidth = value
            Me.Width = cControlWidth
            If ReadConfig = False Then
                Me.SetConfig("ControlWidth", value)
            End If
        End Set
    End Property

    Dim cControlHeight As Integer = 220
    Public Property ControlHeight() As Integer
        Get
            Return cControlHeight
        End Get
        Set(ByVal value As Integer)
            cControlHeight = value
            Me.Height = cControlHeight
            If ReadConfig = False Then
                Me.SetConfig("ControlHeight", value)
            End If
        End Set
    End Property

    Dim cControlPosition As Integer
    Public Property ControlPosition() As Integer
        Get
            Return cControlPosition
        End Get
        Set(ByVal value As Integer)
            cControlPosition = value
            If ReadConfig = False Then
                Me.SetConfig("ControlPosition", value)
            End If
        End Set
    End Property

    Dim cControlConfig As New SortedList(Of String, Object)
    Public Property ControlConfiguration() As SortedList(Of String, Object)
        Get
            ' Check for ControlName
            Call Me.SetConfig("ControlName", ControlName)
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
        ReadConfig = True
        For Each ConfigProperty As String In cControlConfig.Keys
            Dim pi As System.Reflection.PropertyInfo = Me.GetType().GetProperty(ConfigProperty)
            If ConfigProperty <> "ControlName" Then
                pi.SetValue(Me, Convert.ChangeType(cControlConfig(ConfigProperty), pi.PropertyType, Globalization.CultureInfo.InvariantCulture), Nothing)
            End If
        Next
        ReadConfig = False
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
        Call Me.UpdateColours()

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
        AGPSkillInfo.BorderColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCBorderColor))
        AGPSkillInfo.CornerRadius = 10
        AGPSkillInfo.Colors(0).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCMainColor1))
        AGPSkillInfo.Colors(1).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCMainColor2))
        AGPHeader.Colors(0).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCHeadColor1))
        AGPHeader.Colors(1).Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBCHeadColor2))
    End Sub

    Private Sub cboPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilot.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilot.SelectedItem.ToString) Then
            cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilot.SelectedItem.ToString), Core.Pilot)
            ' Update the list of EveHQ Skill Queues
            Call Me.UpdateQueueList()
            ' Update the details
            Call Me.UpdateQueueInfo()
        End If
    End Sub

    Private Sub UpdateQueueList()
        cboSkillQueue.BeginUpdate()
        cboSkillQueue.Items.Clear()
        For Each sq As EveHQ.Core.SkillQueue In cPilot.TrainingQueues.Values
            cboSkillQueue.Items.Add(sq.Name)
        Next
        cboSkillQueue.EndUpdate()
        If cboSkillQueue.Items.Count > 0 Then
            If cboSkillQueue.Items.Contains(cDefaultQueueName) = True Then
                cboSkillQueue.SelectedItem = cDefaultQueueName
            Else
                cboSkillQueue.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboSkillQueue_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSkillQueue.SelectedIndexChanged
        If radEveHQQueue.Checked = True Then
            Call Me.UpdateQueueInfo()
        End If
    End Sub

    Private Sub radEveQueue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEveQueue.CheckedChanged
        Call Me.UpdateQueueInfo()
    End Sub

    Private Sub radEveHQQueue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEveHQQueue.CheckedChanged
        Call Me.UpdateQueueInfo()
    End Sub

    Private Sub UpdateQueueInfo()
        If radEveQueue.Checked = True Then
            ' Draw Eve skill queue
            lvwSkills.BeginUpdate()
            lvwSkills.Items.Clear()
            If cPilot.QueuedSkills IsNot Nothing Then
                For Each QueuedSkill As EveHQ.Core.PilotQueuedSkill In cPilot.QueuedSkills.Values
                    Dim newitem As New ListViewItem
                    newitem.Text = EveHQ.Core.SkillFunctions.SkillIDToName(QueuedSkill.SkillID.ToString) & " (Lvl " & EveHQ.Core.SkillFunctions.Roman(QueuedSkill.Level) & ")"
                    Dim enddate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.EndTime)
                    newitem.ToolTipText = "Skill ends: " & Format(enddate, "ddd") & " " & enddate
                    newitem.Name = QueuedSkill.SkillID.ToString
                    lvwSkills.Items.Add(newitem)
                    'newitem.SubItems.Add(QueuedSkill.Level.ToString)
                Next
            End If
            lvwSkills.EndUpdate()
        Else
            ' Draw an EveHQ skill queue
            lvwSkills.BeginUpdate()
            lvwSkills.Items.Clear()
            If cboSkillQueue.SelectedItem IsNot Nothing Then
                If cPilot.TrainingQueues.ContainsKey(cboSkillQueue.SelectedItem.ToString) = True Then
                    Dim cQueue As EveHQ.Core.SkillQueue = CType(cPilot.TrainingQueues(cboSkillQueue.SelectedItem.ToString), Core.SkillQueue)
                    Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(cPilot, cQueue)
                    For skill As Integer = 0 To arrQueue.Count - 1
                        Dim qItem As EveHQ.Core.SortedQueue = CType(arrQueue(skill), EveHQ.Core.SortedQueue)
                        If qItem.Done = False Then
                            Dim newitem As New ListViewItem
                            newitem.Text = qItem.Name & " (" & EveHQ.Core.SkillFunctions.Roman(CInt(qItem.FromLevel)) & " -> " & EveHQ.Core.SkillFunctions.Roman(CInt(qItem.ToLevel)) & ")"
                            newitem.Name = qItem.ID
                            lvwSkills.Items.Add(newitem)
                            newitem.ToolTipText = "Skill ends: " & Format(qItem.DateFinished, "ddd") & " " & FormatDateTime(qItem.DateFinished, DateFormat.GeneralDate)
                        End If
                    Next
                End If
            End If
            lvwSkills.EndUpdate()
        End If
    End Sub

    Private Sub lblPilot_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblPilot.LinkClicked
        frmPilot.DisplayPilotName = cPilot.Name
        frmEveHQ.OpenPilotInfoForm()
    End Sub

    Private Sub pbConfig_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbConfig.MouseDoubleClick
        ' Initialise the configuration form
        Dim newConfigForm As New DBCSkillQueueInfoConfig
        newConfigForm.DBWidget = Me
        newConfigForm.ShowDialog()
    End Sub

    Private Sub lvwSkills_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSkills.Resize
        ' Alter the column size to reflect the new listview size
        lvwSkills.Columns(0).Width = lvwSkills.Width - 30
    End Sub
End Class
