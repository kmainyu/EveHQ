Namespace Controls.DBControls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class DBCPilotInfo
        Inherits Widget

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
            Me.pbPilot = New System.Windows.Forms.PictureBox
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
            Me.tmrSkill = New System.Windows.Forms.Timer(Me.components)
            Me.AGPContent.SuspendLayout()
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblHeader
            '
            '
            '
            '
            Me.lblHeader.BackgroundStyle.Class = ""
            Me.lblHeader.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblHeader.Image = Global.EveHQ.My.Resources.Resources.Aura32
            Me.lblHeader.Text = "Pilot Information"
            '
            'AGPContent
            '
            Me.AGPContent.Controls.Add(Me.pbPilot)
            Me.AGPContent.Controls.Add(Me.lblPilot)
            Me.AGPContent.Controls.Add(Me.lblTraining)
            Me.AGPContent.Controls.Add(Me.lblSkillQueueEnd)
            Me.AGPContent.Controls.Add(Me.lblSkillQueueTime)
            Me.AGPContent.Controls.Add(Me.lblTrainingTime)
            Me.AGPContent.Controls.Add(Me.lblTrainingEnd)
            Me.AGPContent.Controls.Add(Me.lblSP)
            Me.AGPContent.Controls.Add(Me.lblIsk)
            Me.AGPContent.Controls.Add(Me.lblCorp)
            Me.AGPContent.Controls.Add(Me.cboPilot)
            Me.AGPContent.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.AGPContent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.AGPContent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.AGPContent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.AGPContent.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.AGPContent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.AGPContent.Style.GradientAngle = 90
            Me.AGPContent.Controls.SetChildIndex(Me.pbConfig, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblHeader, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.cboPilot, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblCorp, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblIsk, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblSP, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblTrainingEnd, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblTrainingTime, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblSkillQueueTime, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblSkillQueueEnd, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblTraining, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblPilot, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.pbPilot, 0)
            '
            'pbPilot
            '
            Me.pbPilot.Location = New System.Drawing.Point(13, 64)
            Me.pbPilot.Name = "pbPilot"
            Me.pbPilot.Size = New System.Drawing.Size(54, 54)
            Me.pbPilot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.pbPilot.TabIndex = 34
            Me.pbPilot.TabStop = False
            '
            'lblPilot
            '
            Me.lblPilot.AutoSize = True
            Me.lblPilot.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
            Me.lblPilot.Location = New System.Drawing.Point(10, 40)
            Me.lblPilot.Name = "lblPilot"
            Me.lblPilot.Size = New System.Drawing.Size(31, 13)
            Me.lblPilot.TabIndex = 33
            Me.lblPilot.TabStop = True
            Me.lblPilot.Text = "Pilot:"
            '
            'lblTraining
            '
            Me.lblTraining.AutoSize = True
            Me.lblTraining.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
            Me.lblTraining.Location = New System.Drawing.Point(10, 123)
            Me.lblTraining.Name = "lblTraining"
            Me.lblTraining.Size = New System.Drawing.Size(45, 13)
            Me.lblTraining.TabIndex = 32
            Me.lblTraining.TabStop = True
            Me.lblTraining.Text = "Training"
            '
            'lblSkillQueueEnd
            '
            Me.lblSkillQueueEnd.AutoSize = True
            Me.lblSkillQueueEnd.Location = New System.Drawing.Point(10, 199)
            Me.lblSkillQueueEnd.Name = "lblSkillQueueEnd"
            Me.lblSkillQueueEnd.Size = New System.Drawing.Size(80, 13)
            Me.lblSkillQueueEnd.TabIndex = 31
            Me.lblSkillQueueEnd.Text = "Skill Queue End"
            '
            'lblSkillQueueTime
            '
            Me.lblSkillQueueTime.AutoSize = True
            Me.lblSkillQueueTime.Location = New System.Drawing.Point(10, 180)
            Me.lblSkillQueueTime.Name = "lblSkillQueueTime"
            Me.lblSkillQueueTime.Size = New System.Drawing.Size(84, 13)
            Me.lblSkillQueueTime.TabIndex = 30
            Me.lblSkillQueueTime.Text = "Skill Queue Time"
            '
            'lblTrainingTime
            '
            Me.lblTrainingTime.AutoSize = True
            Me.lblTrainingTime.Location = New System.Drawing.Point(10, 161)
            Me.lblTrainingTime.Name = "lblTrainingTime"
            Me.lblTrainingTime.Size = New System.Drawing.Size(70, 13)
            Me.lblTrainingTime.TabIndex = 29
            Me.lblTrainingTime.Text = "Training Time"
            '
            'lblTrainingEnd
            '
            Me.lblTrainingEnd.AutoSize = True
            Me.lblTrainingEnd.Location = New System.Drawing.Point(10, 142)
            Me.lblTrainingEnd.Name = "lblTrainingEnd"
            Me.lblTrainingEnd.Size = New System.Drawing.Size(66, 13)
            Me.lblTrainingEnd.TabIndex = 28
            Me.lblTrainingEnd.Text = "Training End"
            '
            'lblSP
            '
            Me.lblSP.AutoSize = True
            Me.lblSP.Location = New System.Drawing.Point(73, 105)
            Me.lblSP.Name = "lblSP"
            Me.lblSP.Size = New System.Drawing.Size(19, 13)
            Me.lblSP.TabIndex = 27
            Me.lblSP.Text = "SP"
            '
            'lblIsk
            '
            Me.lblIsk.AutoSize = True
            Me.lblIsk.Location = New System.Drawing.Point(73, 86)
            Me.lblIsk.Name = "lblIsk"
            Me.lblIsk.Size = New System.Drawing.Size(21, 13)
            Me.lblIsk.TabIndex = 26
            Me.lblIsk.Text = "Isk"
            '
            'lblCorp
            '
            Me.lblCorp.AutoSize = True
            Me.lblCorp.Location = New System.Drawing.Point(73, 67)
            Me.lblCorp.Name = "lblCorp"
            Me.lblCorp.Size = New System.Drawing.Size(30, 13)
            Me.lblCorp.TabIndex = 25
            Me.lblCorp.Text = "Corp"
            '
            'cboPilot
            '
            Me.cboPilot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboPilot.FormattingEnabled = True
            Me.cboPilot.Location = New System.Drawing.Point(47, 37)
            Me.cboPilot.Name = "cboPilot"
            Me.cboPilot.Size = New System.Drawing.Size(235, 21)
            Me.cboPilot.Sorted = True
            Me.cboPilot.TabIndex = 24
            '
            'tmrSkill
            '
            Me.tmrSkill.Interval = 5000
            '
            'DBCPilotInfo
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "DBCPilotInfo"
            Me.AGPContent.ResumeLayout(False)
            Me.AGPContent.PerformLayout()
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pbPilot As System.Windows.Forms.PictureBox
        Friend WithEvents lblPilot As System.Windows.Forms.LinkLabel
        Friend WithEvents lblTraining As System.Windows.Forms.LinkLabel
        Friend WithEvents lblSkillQueueEnd As System.Windows.Forms.Label
        Friend WithEvents lblSkillQueueTime As System.Windows.Forms.Label
        Friend WithEvents lblTrainingTime As System.Windows.Forms.Label
        Friend WithEvents lblTrainingEnd As System.Windows.Forms.Label
        Friend WithEvents lblSP As System.Windows.Forms.Label
        Friend WithEvents lblIsk As System.Windows.Forms.Label
        Friend WithEvents lblCorp As System.Windows.Forms.Label
        Friend WithEvents cboPilot As System.Windows.Forms.ComboBox
        Friend WithEvents tmrSkill As System.Windows.Forms.Timer

    End Class
End NameSpace