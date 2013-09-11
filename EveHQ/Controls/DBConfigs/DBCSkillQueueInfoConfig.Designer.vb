<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCSkillQueueInfoConfig
    Inherits DevComponents.DotNetBar.Office2007Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX
        Me.cboPilots = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.lblDefaultPilot = New DevComponents.DotNetBar.LabelX
        Me.lblDefaultQueueType = New DevComponents.DotNetBar.LabelX
        Me.lblDefaultQueue = New DevComponents.DotNetBar.LabelX
        Me.cboSkillQueue = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.radEve = New DevComponents.DotNetBar.Controls.CheckBoxX
        Me.radEveHQ = New DevComponents.DotNetBar.Controls.CheckBoxX
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.Location = New System.Drawing.Point(187, 110)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 21
        Me.btnCancel.Text = "Cancel"
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(106, 110)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAccept.TabIndex = 24
        Me.btnAccept.Text = "Accept"
        '
        'cboPilots
        '
        Me.cboPilots.DisplayMember = "Text"
        Me.cboPilots.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.ItemHeight = 15
        Me.cboPilots.Location = New System.Drawing.Point(94, 12)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(168, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboPilots.TabIndex = 23
        '
        'lblDefaultPilot
        '
        Me.lblDefaultPilot.AutoSize = True
        '
        '
        '
        Me.lblDefaultPilot.BackgroundStyle.Class = ""
        Me.lblDefaultPilot.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblDefaultPilot.Location = New System.Drawing.Point(12, 17)
        Me.lblDefaultPilot.Name = "lblDefaultPilot"
        Me.lblDefaultPilot.Size = New System.Drawing.Size(65, 16)
        Me.lblDefaultPilot.TabIndex = 22
        Me.lblDefaultPilot.Text = "Default Pilot:"
        '
        'lblDefaultQueueType
        '
        Me.lblDefaultQueueType.AutoSize = True
        '
        '
        '
        Me.lblDefaultQueueType.BackgroundStyle.Class = ""
        Me.lblDefaultQueueType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblDefaultQueueType.Location = New System.Drawing.Point(12, 43)
        Me.lblDefaultQueueType.Name = "lblDefaultQueueType"
        Me.lblDefaultQueueType.Size = New System.Drawing.Size(104, 16)
        Me.lblDefaultQueueType.TabIndex = 25
        Me.lblDefaultQueueType.Text = "Default Queue Type:"
        '
        'lblDefaultQueue
        '
        Me.lblDefaultQueue.AutoSize = True
        '
        '
        '
        Me.lblDefaultQueue.BackgroundStyle.Class = ""
        Me.lblDefaultQueue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblDefaultQueue.Location = New System.Drawing.Point(12, 75)
        Me.lblDefaultQueue.Name = "lblDefaultQueue"
        Me.lblDefaultQueue.Size = New System.Drawing.Size(76, 16)
        Me.lblDefaultQueue.TabIndex = 26
        Me.lblDefaultQueue.Text = "Default Queue:"
        '
        'cboSkillQueue
        '
        Me.cboSkillQueue.DisplayMember = "Text"
        Me.cboSkillQueue.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboSkillQueue.FormattingEnabled = True
        Me.cboSkillQueue.ItemHeight = 15
        Me.cboSkillQueue.Location = New System.Drawing.Point(100, 70)
        Me.cboSkillQueue.Name = "cboSkillQueue"
        Me.cboSkillQueue.Size = New System.Drawing.Size(162, 21)
        Me.cboSkillQueue.Sorted = True
        Me.cboSkillQueue.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboSkillQueue.TabIndex = 27
        '
        'radEve
        '
        Me.radEve.AutoSize = True
        '
        '
        '
        Me.radEve.BackgroundStyle.Class = ""
        Me.radEve.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.radEve.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.radEve.Checked = True
        Me.radEve.CheckState = System.Windows.Forms.CheckState.Checked
        Me.radEve.CheckValue = "Y"
        Me.radEve.Location = New System.Drawing.Point(122, 43)
        Me.radEve.Name = "radEve"
        Me.radEve.Size = New System.Drawing.Size(40, 16)
        Me.radEve.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.radEve.TabIndex = 28
        Me.radEve.Text = "Eve"
        '
        'radEveHQ
        '
        Me.radEveHQ.AutoSize = True
        '
        '
        '
        Me.radEveHQ.BackgroundStyle.Class = ""
        Me.radEveHQ.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.radEveHQ.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.radEveHQ.Location = New System.Drawing.Point(187, 43)
        Me.radEveHQ.Name = "radEveHQ"
        Me.radEveHQ.Size = New System.Drawing.Size(55, 16)
        Me.radEveHQ.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.radEveHQ.TabIndex = 29
        Me.radEveHQ.Text = "EveHQ"
        '
        'DBCSkillQueueInfoConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(274, 145)
        Me.Controls.Add(Me.radEveHQ)
        Me.Controls.Add(Me.radEve)
        Me.Controls.Add(Me.cboSkillQueue)
        Me.Controls.Add(Me.lblDefaultQueue)
        Me.Controls.Add(Me.lblDefaultQueueType)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblDefaultPilot)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DBCSkillQueueInfoConfig"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Skill Queue Info Configuration"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Add any initialization after the InitializeComponent() call.
        ' Load the combo box with the pilot info
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each pilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
            If pilot.Active = True Then
                cboPilots.Items.Add(pilot.Name)
            End If
        Next
        cboPilots.EndUpdate()
    End Sub
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboPilots As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblDefaultPilot As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblDefaultQueueType As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblDefaultQueue As DevComponents.DotNetBar.LabelX
    Friend WithEvents cboSkillQueue As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents radEve As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents radEveHQ As DevComponents.DotNetBar.Controls.CheckBoxX
End Class
