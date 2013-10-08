Namespace Controls.DBControls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class DBCEveSkillQueue
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
            Me.lblPilot = New System.Windows.Forms.LinkLabel
            Me.cboPilot = New System.Windows.Forms.ComboBox
            Me.sqcEveQueue = New EveHQ.Core.SkillQueueControl
            Me.tmrSkill = New System.Windows.Forms.Timer(Me.components)
            Me.AGPContent.SuspendLayout()
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblHeader
            '
            '
            '
            '
            Me.lblHeader.BackgroundStyle.Class = ""
            Me.lblHeader.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblHeader.Image = Global.EveHQ.My.Resources.Resources.SkillBook32
            Me.lblHeader.Size = New System.Drawing.Size(313, 23)
            Me.lblHeader.Text = "Eve Skill Queue Information"
            '
            'AGPContent
            '
            Me.AGPContent.Controls.Add(Me.lblPilot)
            Me.AGPContent.Controls.Add(Me.cboPilot)
            Me.AGPContent.Controls.Add(Me.sqcEveQueue)
            Me.AGPContent.Size = New System.Drawing.Size(325, 220)
            Me.AGPContent.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.AGPContent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.AGPContent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ExplorerBarBackground2
            Me.AGPContent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.AGPContent.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.AGPContent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.AGPContent.Style.GradientAngle = 90
            Me.AGPContent.Controls.SetChildIndex(Me.lblHeader, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.pbConfig, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.sqcEveQueue, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.cboPilot, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblPilot, 0)
            '
            'pbConfig
            '
            Me.pbConfig.Location = New System.Drawing.Point(295, 8)
            '
            'lblPilot
            '
            Me.lblPilot.AutoSize = True
            Me.lblPilot.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
            Me.lblPilot.Location = New System.Drawing.Point(9, 40)
            Me.lblPilot.Name = "lblPilot"
            Me.lblPilot.Size = New System.Drawing.Size(31, 13)
            Me.lblPilot.TabIndex = 25
            Me.lblPilot.TabStop = True
            Me.lblPilot.Text = "Pilot:"
            '
            'cboPilot
            '
            Me.cboPilot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboPilot.FormattingEnabled = True
            Me.cboPilot.Location = New System.Drawing.Point(45, 37)
            Me.cboPilot.Name = "cboPilot"
            Me.cboPilot.Size = New System.Drawing.Size(259, 21)
            Me.cboPilot.Sorted = True
            Me.cboPilot.TabIndex = 24
            '
            'sqcEveQueue
            '
            Me.sqcEveQueue.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                            Or System.Windows.Forms.AnchorStyles.Left) _
                                           Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.sqcEveQueue.BackColor = System.Drawing.Color.Transparent
            Me.sqcEveQueue.Location = New System.Drawing.Point(6, 64)
            Me.sqcEveQueue.Name = "sqcEveQueue"
            Me.sqcEveQueue.PilotName = ""
            Me.sqcEveQueue.Size = New System.Drawing.Size(313, 150)
            Me.sqcEveQueue.TabIndex = 26
            '
            'tmrSkill
            '
            Me.tmrSkill.Interval = 5000
            '
            'DBCEveSkillQueue
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "DBCEveSkillQueue"
            Me.Size = New System.Drawing.Size(325, 220)
            Me.AGPContent.ResumeLayout(False)
            Me.AGPContent.PerformLayout()
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblPilot As System.Windows.Forms.LinkLabel
        Friend WithEvents cboPilot As System.Windows.Forms.ComboBox
        Friend WithEvents sqcEveQueue As EveHQ.Core.SkillQueueControl
        Friend WithEvents tmrSkill As System.Windows.Forms.Timer

    End Class
End NameSpace