<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCPilotInfo
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container
        Me.tmrSkill = New System.Windows.Forms.Timer(Me.components)
        Me.AGPPilotInfo = New EveHQ.Core.AlphaGradientPanel
        Me.ColorWithAlpha1 = New EveHQ.Core.ColorWithAlpha
        Me.ColorWithAlpha2 = New EveHQ.Core.ColorWithAlpha
        Me.lblPilot = New System.Windows.Forms.LinkLabel
        Me.lblTraining = New System.Windows.Forms.LinkLabel
        Me.lblSkillQueueEnd = New System.Windows.Forms.Label
        Me.lblSkillQueueTime = New System.Windows.Forms.Label
        Me.lblTrainingTime = New System.Windows.Forms.Label
        Me.lblTrainingEnd = New System.Windows.Forms.Label
        Me.lblSP = New System.Windows.Forms.Label
        Me.lblIsk = New System.Windows.Forms.Label
        Me.lblCorp = New System.Windows.Forms.Label
        Me.cboPilot = New System.Windows.Forms.ComboBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pbConfig = New System.Windows.Forms.PictureBox
        Me.AGPHeader = New EveHQ.Core.AlphaGradientPanel
        Me.ColorWithAlpha3 = New EveHQ.Core.ColorWithAlpha
        Me.ColorWithAlpha4 = New EveHQ.Core.ColorWithAlpha
        Me.lblHeader = New System.Windows.Forms.Label
        Me.AGPPilotInfo.SuspendLayout()
        CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AGPHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'tmrSkill
        '
        Me.tmrSkill.Interval = 5000
        '
        'AGPPilotInfo
        '
        Me.AGPPilotInfo.BackColor = System.Drawing.Color.Transparent
        Me.AGPPilotInfo.Border = True
        Me.AGPPilotInfo.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AGPPilotInfo.Colors.Add(Me.ColorWithAlpha1)
        Me.AGPPilotInfo.Colors.Add(Me.ColorWithAlpha2)
        Me.AGPPilotInfo.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AGPPilotInfo.Controls.Add(Me.lblPilot)
        Me.AGPPilotInfo.Controls.Add(Me.lblTraining)
        Me.AGPPilotInfo.Controls.Add(Me.lblSkillQueueEnd)
        Me.AGPPilotInfo.Controls.Add(Me.lblSkillQueueTime)
        Me.AGPPilotInfo.Controls.Add(Me.lblTrainingTime)
        Me.AGPPilotInfo.Controls.Add(Me.lblTrainingEnd)
        Me.AGPPilotInfo.Controls.Add(Me.lblSP)
        Me.AGPPilotInfo.Controls.Add(Me.lblIsk)
        Me.AGPPilotInfo.Controls.Add(Me.lblCorp)
        Me.AGPPilotInfo.Controls.Add(Me.cboPilot)
        Me.AGPPilotInfo.CornerRadius = 10
        Me.AGPPilotInfo.Corners = CType((EveHQ.Core.Corner.BottomLeft Or EveHQ.Core.Corner.BottomRight), EveHQ.Core.Corner)
        Me.AGPPilotInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AGPPilotInfo.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AGPPilotInfo.Gradient = True
        Me.AGPPilotInfo.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.AGPPilotInfo.GradientOffset = 1.0!
        Me.AGPPilotInfo.GradientSize = New System.Drawing.Size(0, 0)
        Me.AGPPilotInfo.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AGPPilotInfo.Grayscale = False
        Me.AGPPilotInfo.Image = Global.EveHQ.My.Resources.Resources.noitem
        Me.AGPPilotInfo.ImageAlpha = 64
        Me.AGPPilotInfo.ImagePadding = New System.Windows.Forms.Padding(15)
        Me.AGPPilotInfo.ImagePosition = EveHQ.Core.ImagePosition.BottomRight
        Me.AGPPilotInfo.ImageSize = New System.Drawing.Size(128, 128)
        Me.AGPPilotInfo.Location = New System.Drawing.Point(0, 22)
        Me.AGPPilotInfo.Name = "AGPPilotInfo"
        Me.AGPPilotInfo.Rounded = True
        Me.AGPPilotInfo.Size = New System.Drawing.Size(300, 198)
        Me.AGPPilotInfo.TabIndex = 0
        '
        'ColorWithAlpha1
        '
        Me.ColorWithAlpha1.Alpha = 255
        Me.ColorWithAlpha1.Color = System.Drawing.Color.White
        Me.ColorWithAlpha1.Parent = Me.AGPPilotInfo
        '
        'ColorWithAlpha2
        '
        Me.ColorWithAlpha2.Alpha = 255
        Me.ColorWithAlpha2.Color = System.Drawing.Color.White
        Me.ColorWithAlpha2.Parent = Me.AGPPilotInfo
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblPilot.Location = New System.Drawing.Point(12, 11)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 12
        Me.lblPilot.TabStop = True
        Me.lblPilot.Text = "Pilot:"
        Me.ToolTip1.SetToolTip(Me.lblPilot, "Click to open pilot information")
        '
        'lblTraining
        '
        Me.lblTraining.AutoSize = True
        Me.lblTraining.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblTraining.Location = New System.Drawing.Point(12, 94)
        Me.lblTraining.Name = "lblTraining"
        Me.lblTraining.Size = New System.Drawing.Size(45, 13)
        Me.lblTraining.TabIndex = 11
        Me.lblTraining.TabStop = True
        Me.lblTraining.Text = "Training"
        Me.ToolTip1.SetToolTip(Me.lblTraining, "Click to open skill training")
        '
        'lblSkillQueueEnd
        '
        Me.lblSkillQueueEnd.AutoSize = True
        Me.lblSkillQueueEnd.Location = New System.Drawing.Point(12, 170)
        Me.lblSkillQueueEnd.Name = "lblSkillQueueEnd"
        Me.lblSkillQueueEnd.Size = New System.Drawing.Size(80, 13)
        Me.lblSkillQueueEnd.TabIndex = 10
        Me.lblSkillQueueEnd.Text = "Skill Queue End"
        '
        'lblSkillQueueTime
        '
        Me.lblSkillQueueTime.AutoSize = True
        Me.lblSkillQueueTime.Location = New System.Drawing.Point(12, 151)
        Me.lblSkillQueueTime.Name = "lblSkillQueueTime"
        Me.lblSkillQueueTime.Size = New System.Drawing.Size(84, 13)
        Me.lblSkillQueueTime.TabIndex = 9
        Me.lblSkillQueueTime.Text = "Skill Queue Time"
        '
        'lblTrainingTime
        '
        Me.lblTrainingTime.AutoSize = True
        Me.lblTrainingTime.Location = New System.Drawing.Point(12, 132)
        Me.lblTrainingTime.Name = "lblTrainingTime"
        Me.lblTrainingTime.Size = New System.Drawing.Size(70, 13)
        Me.lblTrainingTime.TabIndex = 8
        Me.lblTrainingTime.Text = "Training Time"
        '
        'lblTrainingEnd
        '
        Me.lblTrainingEnd.AutoSize = True
        Me.lblTrainingEnd.Location = New System.Drawing.Point(12, 113)
        Me.lblTrainingEnd.Name = "lblTrainingEnd"
        Me.lblTrainingEnd.Size = New System.Drawing.Size(66, 13)
        Me.lblTrainingEnd.TabIndex = 7
        Me.lblTrainingEnd.Text = "Training End"
        '
        'lblSP
        '
        Me.lblSP.AutoSize = True
        Me.lblSP.Location = New System.Drawing.Point(12, 75)
        Me.lblSP.Name = "lblSP"
        Me.lblSP.Size = New System.Drawing.Size(19, 13)
        Me.lblSP.TabIndex = 5
        Me.lblSP.Text = "SP"
        '
        'lblIsk
        '
        Me.lblIsk.AutoSize = True
        Me.lblIsk.Location = New System.Drawing.Point(12, 56)
        Me.lblIsk.Name = "lblIsk"
        Me.lblIsk.Size = New System.Drawing.Size(21, 13)
        Me.lblIsk.TabIndex = 4
        Me.lblIsk.Text = "Isk"
        '
        'lblCorp
        '
        Me.lblCorp.AutoSize = True
        Me.lblCorp.Location = New System.Drawing.Point(12, 37)
        Me.lblCorp.Name = "lblCorp"
        Me.lblCorp.Size = New System.Drawing.Size(30, 13)
        Me.lblCorp.TabIndex = 3
        Me.lblCorp.Text = "Corp"
        '
        'cboPilot
        '
        Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilot.FormattingEnabled = True
        Me.cboPilot.Location = New System.Drawing.Point(49, 8)
        Me.cboPilot.Name = "cboPilot"
        Me.cboPilot.Size = New System.Drawing.Size(234, 21)
        Me.cboPilot.Sorted = True
        Me.cboPilot.TabIndex = 0
        '
        'pbConfig
        '
        Me.pbConfig.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbConfig.Image = Global.EveHQ.My.Resources.Resources.info_icon
        Me.pbConfig.Location = New System.Drawing.Point(275, 1)
        Me.pbConfig.Name = "pbConfig"
        Me.pbConfig.Size = New System.Drawing.Size(20, 20)
        Me.pbConfig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbConfig.TabIndex = 13
        Me.pbConfig.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbConfig, "Double-click to configure the control")
        '
        'AGPHeader
        '
        Me.AGPHeader.BackColor = System.Drawing.Color.Transparent
        Me.AGPHeader.Border = True
        Me.AGPHeader.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AGPHeader.Colors.Add(Me.ColorWithAlpha3)
        Me.AGPHeader.Colors.Add(Me.ColorWithAlpha4)
        Me.AGPHeader.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AGPHeader.Controls.Add(Me.lblHeader)
        Me.AGPHeader.Controls.Add(Me.pbConfig)
        Me.AGPHeader.CornerRadius = 10
        Me.AGPHeader.Corners = CType((EveHQ.Core.Corner.TopLeft Or EveHQ.Core.Corner.TopRight), EveHQ.Core.Corner)
        Me.AGPHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.AGPHeader.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AGPHeader.ForeColor = System.Drawing.Color.White
        Me.AGPHeader.Gradient = True
        Me.AGPHeader.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.AGPHeader.GradientOffset = 1.0!
        Me.AGPHeader.GradientSize = New System.Drawing.Size(0, 0)
        Me.AGPHeader.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AGPHeader.Grayscale = False
        Me.AGPHeader.Image = Nothing
        Me.AGPHeader.ImageAlpha = 75
        Me.AGPHeader.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AGPHeader.ImagePosition = EveHQ.Core.ImagePosition.BottomRight
        Me.AGPHeader.ImageSize = New System.Drawing.Size(48, 48)
        Me.AGPHeader.Location = New System.Drawing.Point(0, 0)
        Me.AGPHeader.Name = "AGPHeader"
        Me.AGPHeader.Rounded = True
        Me.AGPHeader.Size = New System.Drawing.Size(300, 22)
        Me.AGPHeader.TabIndex = 1
        '
        'ColorWithAlpha3
        '
        Me.ColorWithAlpha3.Alpha = 255
        Me.ColorWithAlpha3.Color = System.Drawing.Color.DimGray
        Me.ColorWithAlpha3.Parent = Me.AGPHeader
        '
        'ColorWithAlpha4
        '
        Me.ColorWithAlpha4.Alpha = 255
        Me.ColorWithAlpha4.Color = System.Drawing.Color.LightGray
        Me.ColorWithAlpha4.Parent = Me.AGPHeader
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.Location = New System.Drawing.Point(12, 6)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(103, 13)
        Me.lblHeader.TabIndex = 13
        Me.lblHeader.Text = "Pilot Information"
        '
        'DBCPilotInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.AGPPilotInfo)
        Me.Controls.Add(Me.AGPHeader)
        Me.DoubleBuffered = True
        Me.Name = "DBCPilotInfo"
        Me.Size = New System.Drawing.Size(300, 220)
        Me.AGPPilotInfo.ResumeLayout(False)
        Me.AGPPilotInfo.PerformLayout()
        CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AGPHeader.ResumeLayout(False)
        Me.AGPHeader.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AGPPilotInfo As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha1 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents lblTrainingTime As System.Windows.Forms.Label
    Friend WithEvents lblTrainingEnd As System.Windows.Forms.Label
    Friend WithEvents lblSP As System.Windows.Forms.Label
    Friend WithEvents lblIsk As System.Windows.Forms.Label
    Friend WithEvents lblCorp As System.Windows.Forms.Label
    Friend WithEvents cboPilot As System.Windows.Forms.ComboBox
    Friend WithEvents lblSkillQueueEnd As System.Windows.Forms.Label
    Friend WithEvents lblSkillQueueTime As System.Windows.Forms.Label
    Friend WithEvents tmrSkill As System.Windows.Forms.Timer
    Friend WithEvents lblTraining As System.Windows.Forms.LinkLabel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblPilot As System.Windows.Forms.LinkLabel
    Friend WithEvents pbConfig As System.Windows.Forms.PictureBox
    Friend WithEvents AGPHeader As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha3 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha4 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents lblHeader As System.Windows.Forms.Label

End Class
