<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCSkillQueueInfoConfig
    Inherits System.Windows.Forms.Form

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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAccept = New System.Windows.Forms.Button
        Me.nudHeight = New System.Windows.Forms.NumericUpDown
        Me.nudWidth = New System.Windows.Forms.NumericUpDown
        Me.lblHeight = New System.Windows.Forms.Label
        Me.lblWidth = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lblDefaultPilot = New System.Windows.Forms.Label
        Me.lblDefaultQueueType = New System.Windows.Forms.Label
        Me.radEve = New System.Windows.Forms.RadioButton
        Me.radEveHQ = New System.Windows.Forms.RadioButton
        Me.cboSkillQueue = New System.Windows.Forms.ComboBox
        Me.lblDefaultQueue = New System.Windows.Forms.Label
        CType(Me.nudHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(187, 141)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(106, 141)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 14
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'nudHeight
        '
        Me.nudHeight.Location = New System.Drawing.Point(190, 12)
        Me.nudHeight.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudHeight.Minimum = New Decimal(New Integer() {220, 0, 0, 0})
        Me.nudHeight.Name = "nudHeight"
        Me.nudHeight.Size = New System.Drawing.Size(60, 21)
        Me.nudHeight.TabIndex = 13
        Me.nudHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudHeight.Value = New Decimal(New Integer() {220, 0, 0, 0})
        '
        'nudWidth
        '
        Me.nudWidth.Location = New System.Drawing.Point(58, 12)
        Me.nudWidth.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudWidth.Minimum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.nudWidth.Name = "nudWidth"
        Me.nudWidth.Size = New System.Drawing.Size(60, 21)
        Me.nudWidth.TabIndex = 12
        Me.nudWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudWidth.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'lblHeight
        '
        Me.lblHeight.AutoSize = True
        Me.lblHeight.Location = New System.Drawing.Point(142, 14)
        Me.lblHeight.Name = "lblHeight"
        Me.lblHeight.Size = New System.Drawing.Size(42, 13)
        Me.lblHeight.TabIndex = 11
        Me.lblHeight.Text = "Height:"
        '
        'lblWidth
        '
        Me.lblWidth.AutoSize = True
        Me.lblWidth.Location = New System.Drawing.Point(13, 14)
        Me.lblWidth.Name = "lblWidth"
        Me.lblWidth.Size = New System.Drawing.Size(39, 13)
        Me.lblWidth.TabIndex = 10
        Me.lblWidth.Text = "Width:"
        '
        'cboPilots
        '
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(87, 48)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(175, 21)
        Me.cboPilots.TabIndex = 9
        '
        'lblDefaultPilot
        '
        Me.lblDefaultPilot.AutoSize = True
        Me.lblDefaultPilot.Location = New System.Drawing.Point(12, 51)
        Me.lblDefaultPilot.Name = "lblDefaultPilot"
        Me.lblDefaultPilot.Size = New System.Drawing.Size(69, 13)
        Me.lblDefaultPilot.TabIndex = 8
        Me.lblDefaultPilot.Text = "Default Pilot:"
        '
        'lblDefaultQueueType
        '
        Me.lblDefaultQueueType.AutoSize = True
        Me.lblDefaultQueueType.Location = New System.Drawing.Point(9, 81)
        Me.lblDefaultQueueType.Name = "lblDefaultQueueType"
        Me.lblDefaultQueueType.Size = New System.Drawing.Size(108, 13)
        Me.lblDefaultQueueType.TabIndex = 16
        Me.lblDefaultQueueType.Text = "Default Queue Type:"
        '
        'radEve
        '
        Me.radEve.AutoSize = True
        Me.radEve.Checked = True
        Me.radEve.Location = New System.Drawing.Point(123, 79)
        Me.radEve.Name = "radEve"
        Me.radEve.Size = New System.Drawing.Size(43, 17)
        Me.radEve.TabIndex = 17
        Me.radEve.TabStop = True
        Me.radEve.Text = "Eve"
        Me.radEve.UseVisualStyleBackColor = True
        '
        'radEveHQ
        '
        Me.radEveHQ.AutoSize = True
        Me.radEveHQ.Location = New System.Drawing.Point(172, 79)
        Me.radEveHQ.Name = "radEveHQ"
        Me.radEveHQ.Size = New System.Drawing.Size(58, 17)
        Me.radEveHQ.TabIndex = 18
        Me.radEveHQ.Text = "EveHQ"
        Me.radEveHQ.UseVisualStyleBackColor = True
        '
        'cboSkillQueue
        '
        Me.cboSkillQueue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSkillQueue.FormattingEnabled = True
        Me.cboSkillQueue.Location = New System.Drawing.Point(100, 105)
        Me.cboSkillQueue.Name = "cboSkillQueue"
        Me.cboSkillQueue.Size = New System.Drawing.Size(162, 21)
        Me.cboSkillQueue.TabIndex = 20
        '
        'lblDefaultQueue
        '
        Me.lblDefaultQueue.AutoSize = True
        Me.lblDefaultQueue.Location = New System.Drawing.Point(13, 108)
        Me.lblDefaultQueue.Name = "lblDefaultQueue"
        Me.lblDefaultQueue.Size = New System.Drawing.Size(81, 13)
        Me.lblDefaultQueue.TabIndex = 19
        Me.lblDefaultQueue.Text = "Default Queue:"
        '
        'DBCSkillQueueInfoConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(274, 176)
        Me.Controls.Add(Me.cboSkillQueue)
        Me.Controls.Add(Me.lblDefaultQueue)
        Me.Controls.Add(Me.radEveHQ)
        Me.Controls.Add(Me.radEve)
        Me.Controls.Add(Me.lblDefaultQueueType)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.nudHeight)
        Me.Controls.Add(Me.nudWidth)
        Me.Controls.Add(Me.lblHeight)
        Me.Controls.Add(Me.lblWidth)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblDefaultPilot)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "DBCSkillQueueInfoConfig"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Skill Queue Info Configuration"
        CType(Me.nudHeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudWidth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents nudHeight As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblHeight As System.Windows.Forms.Label
    Friend WithEvents lblWidth As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblDefaultPilot As System.Windows.Forms.Label
    Friend WithEvents lblDefaultQueueType As System.Windows.Forms.Label
    Friend WithEvents radEve As System.Windows.Forms.RadioButton
    Friend WithEvents radEveHQ As System.Windows.Forms.RadioButton
    Friend WithEvents cboSkillQueue As System.Windows.Forms.ComboBox
    Friend WithEvents lblDefaultQueue As System.Windows.Forms.Label

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Add any initialization after the InitializeComponent() call.
        ' Load the combo box with the pilot info
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If pilot.Active = True Then
                cboPilots.Items.Add(pilot.Name)
            End If
        Next
        cboPilots.EndUpdate()
    End Sub
End Class
