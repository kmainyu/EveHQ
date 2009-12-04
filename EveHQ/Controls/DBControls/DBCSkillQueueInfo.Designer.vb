<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCSkillQueueInfo
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
        Me.AGPSkillInfo = New EveHQ.Core.AlphaGradientPanel
        Me.ColorWithAlpha1 = New EveHQ.Core.ColorWithAlpha
        Me.ColorWithAlpha2 = New EveHQ.Core.ColorWithAlpha
        Me.cboSkillQueue = New System.Windows.Forms.ComboBox
        Me.lvwSkills = New System.Windows.Forms.ListView
        Me.colSkillName = New System.Windows.Forms.ColumnHeader
        Me.radEveHQQueue = New System.Windows.Forms.RadioButton
        Me.radEveQueue = New System.Windows.Forms.RadioButton
        Me.lblPilot = New System.Windows.Forms.LinkLabel
        Me.cboPilot = New System.Windows.Forms.ComboBox
        Me.AGPHeader = New EveHQ.Core.AlphaGradientPanel
        Me.ColorWithAlpha3 = New EveHQ.Core.ColorWithAlpha
        Me.ColorWithAlpha4 = New EveHQ.Core.ColorWithAlpha
        Me.lblHeader = New System.Windows.Forms.Label
        Me.pbConfig = New System.Windows.Forms.PictureBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.AGPSkillInfo.SuspendLayout()
        Me.AGPHeader.SuspendLayout()
        CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AGPSkillInfo
        '
        Me.AGPSkillInfo.BackColor = System.Drawing.Color.Transparent
        Me.AGPSkillInfo.Border = True
        Me.AGPSkillInfo.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AGPSkillInfo.Colors.Add(Me.ColorWithAlpha1)
        Me.AGPSkillInfo.Colors.Add(Me.ColorWithAlpha2)
        Me.AGPSkillInfo.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AGPSkillInfo.Controls.Add(Me.cboSkillQueue)
        Me.AGPSkillInfo.Controls.Add(Me.lvwSkills)
        Me.AGPSkillInfo.Controls.Add(Me.radEveHQQueue)
        Me.AGPSkillInfo.Controls.Add(Me.radEveQueue)
        Me.AGPSkillInfo.Controls.Add(Me.lblPilot)
        Me.AGPSkillInfo.Controls.Add(Me.cboPilot)
        Me.AGPSkillInfo.CornerRadius = 10
        Me.AGPSkillInfo.Corners = CType((EveHQ.Core.Corner.BottomLeft Or EveHQ.Core.Corner.BottomRight), EveHQ.Core.Corner)
        Me.AGPSkillInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AGPSkillInfo.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AGPSkillInfo.Gradient = True
        Me.AGPSkillInfo.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.AGPSkillInfo.GradientOffset = 1.0!
        Me.AGPSkillInfo.GradientSize = New System.Drawing.Size(0, 0)
        Me.AGPSkillInfo.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AGPSkillInfo.Grayscale = False
        Me.AGPSkillInfo.Image = Global.EveHQ.My.Resources.Resources.noitem
        Me.AGPSkillInfo.ImageAlpha = 16
        Me.AGPSkillInfo.ImagePadding = New System.Windows.Forms.Padding(15)
        Me.AGPSkillInfo.ImagePosition = EveHQ.Core.ImagePosition.BottomRight
        Me.AGPSkillInfo.ImageSize = New System.Drawing.Size(0, 0)
        Me.AGPSkillInfo.Location = New System.Drawing.Point(0, 22)
        Me.AGPSkillInfo.Name = "AGPSkillInfo"
        Me.AGPSkillInfo.Rounded = True
        Me.AGPSkillInfo.Size = New System.Drawing.Size(300, 198)
        Me.AGPSkillInfo.TabIndex = 2
        '
        'ColorWithAlpha1
        '
        Me.ColorWithAlpha1.Alpha = 255
        Me.ColorWithAlpha1.Color = System.Drawing.SystemColors.Control
        Me.ColorWithAlpha1.Parent = Me.AGPSkillInfo
        '
        'ColorWithAlpha2
        '
        Me.ColorWithAlpha2.Alpha = 255
        Me.ColorWithAlpha2.Color = System.Drawing.SystemColors.Control
        Me.ColorWithAlpha2.Parent = Me.AGPSkillInfo
        '
        'cboSkillQueue
        '
        Me.cboSkillQueue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSkillQueue.FormattingEnabled = True
        Me.cboSkillQueue.Location = New System.Drawing.Point(132, 34)
        Me.cboSkillQueue.Name = "cboSkillQueue"
        Me.cboSkillQueue.Size = New System.Drawing.Size(151, 21)
        Me.cboSkillQueue.TabIndex = 16
        '
        'lvwSkills
        '
        Me.lvwSkills.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwSkills.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSkillName})
        Me.lvwSkills.FullRowSelect = True
        Me.lvwSkills.GridLines = True
        Me.lvwSkills.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvwSkills.Location = New System.Drawing.Point(5, 58)
        Me.lvwSkills.Name = "lvwSkills"
        Me.lvwSkills.ShowItemToolTips = True
        Me.lvwSkills.Size = New System.Drawing.Size(290, 132)
        Me.lvwSkills.TabIndex = 15
        Me.lvwSkills.UseCompatibleStateImageBehavior = False
        Me.lvwSkills.View = System.Windows.Forms.View.Details
        '
        'colSkillName
        '
        Me.colSkillName.Text = "Skill Name"
        Me.colSkillName.Width = 260
        '
        'radEveHQQueue
        '
        Me.radEveHQQueue.AutoSize = True
        Me.radEveHQQueue.Location = New System.Drawing.Point(64, 35)
        Me.radEveHQQueue.Name = "radEveHQQueue"
        Me.radEveHQQueue.Size = New System.Drawing.Size(62, 17)
        Me.radEveHQQueue.TabIndex = 14
        Me.radEveHQQueue.TabStop = True
        Me.radEveHQQueue.Text = "EveHQ:"
        Me.radEveHQQueue.UseVisualStyleBackColor = True
        '
        'radEveQueue
        '
        Me.radEveQueue.AutoSize = True
        Me.radEveQueue.Location = New System.Drawing.Point(15, 35)
        Me.radEveQueue.Name = "radEveQueue"
        Me.radEveQueue.Size = New System.Drawing.Size(43, 17)
        Me.radEveQueue.TabIndex = 13
        Me.radEveQueue.TabStop = True
        Me.radEveQueue.Text = "Eve"
        Me.radEveQueue.UseVisualStyleBackColor = True
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
        Me.AGPHeader.TabIndex = 3
        '
        'ColorWithAlpha3
        '
        Me.ColorWithAlpha3.Alpha = 255
        Me.ColorWithAlpha3.Color = System.Drawing.SystemColors.Control
        Me.ColorWithAlpha3.Parent = Me.AGPHeader
        '
        'ColorWithAlpha4
        '
        Me.ColorWithAlpha4.Alpha = 255
        Me.ColorWithAlpha4.Color = System.Drawing.SystemColors.Control
        Me.ColorWithAlpha4.Parent = Me.AGPHeader
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.Location = New System.Drawing.Point(12, 6)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(140, 13)
        Me.lblHeader.TabIndex = 13
        Me.lblHeader.Text = "Skill Queue Information"
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
        'DBCSkillQueueInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.AGPSkillInfo)
        Me.Controls.Add(Me.AGPHeader)
        Me.Name = "DBCSkillQueueInfo"
        Me.Size = New System.Drawing.Size(300, 220)
        Me.AGPSkillInfo.ResumeLayout(False)
        Me.AGPSkillInfo.PerformLayout()
        Me.AGPHeader.ResumeLayout(False)
        Me.AGPHeader.PerformLayout()
        CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AGPSkillInfo As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents lblPilot As System.Windows.Forms.LinkLabel
    Friend WithEvents cboPilot As System.Windows.Forms.ComboBox
    Friend WithEvents AGPHeader As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents pbConfig As System.Windows.Forms.PictureBox
    Friend WithEvents ColorWithAlpha1 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha3 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha4 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents radEveHQQueue As System.Windows.Forms.RadioButton
    Friend WithEvents radEveQueue As System.Windows.Forms.RadioButton
    Friend WithEvents lvwSkills As System.Windows.Forms.ListView
    Friend WithEvents colSkillName As System.Windows.Forms.ColumnHeader
    Friend WithEvents cboSkillQueue As System.Windows.Forms.ComboBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
