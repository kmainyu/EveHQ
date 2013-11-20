Namespace Controls.DBControls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class DBCSkillQueueInfo
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
            Me.cboSkillQueue = New System.Windows.Forms.ComboBox
            Me.lvwSkills = New DevComponents.DotNetBar.Controls.ListViewEx
            Me.colSkillName = New System.Windows.Forms.ColumnHeader
            Me.radEveHQQueue = New System.Windows.Forms.RadioButton
            Me.radEveQueue = New System.Windows.Forms.RadioButton
            Me.lblPilot = New System.Windows.Forms.LinkLabel
            Me.cboPilot = New System.Windows.Forms.ComboBox
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
            Me.lblHeader.Text = "Skill Queue Information"
            '
            'AGPContent
            '
            Me.AGPContent.Controls.Add(Me.cboSkillQueue)
            Me.AGPContent.Controls.Add(Me.lvwSkills)
            Me.AGPContent.Controls.Add(Me.radEveHQQueue)
            Me.AGPContent.Controls.Add(Me.radEveQueue)
            Me.AGPContent.Controls.Add(Me.lblPilot)
            Me.AGPContent.Controls.Add(Me.cboPilot)
            Me.AGPContent.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.AGPContent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.AGPContent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.AGPContent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.AGPContent.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.AGPContent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.AGPContent.Style.GradientAngle = 90
            Me.AGPContent.Controls.SetChildIndex(Me.lblHeader, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.cboPilot, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblPilot, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.radEveQueue, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.radEveHQQueue, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lvwSkills, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.cboSkillQueue, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.pbConfig, 0)
            '
            'cboSkillQueue
            '
            Me.cboSkillQueue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                             Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboSkillQueue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboSkillQueue.FormattingEnabled = True
            Me.cboSkillQueue.Location = New System.Drawing.Point(132, 60)
            Me.cboSkillQueue.Name = "cboSkillQueue"
            Me.cboSkillQueue.Size = New System.Drawing.Size(151, 21)
            Me.cboSkillQueue.TabIndex = 28
            '
            'lvwSkills
            '
            Me.lvwSkills.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                          Or System.Windows.Forms.AnchorStyles.Left) _
                                         Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            '
            '
            '
            Me.lvwSkills.Border.Class = "ListViewBorder"
            Me.lvwSkills.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lvwSkills.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSkillName})
            Me.lvwSkills.FullRowSelect = True
            Me.lvwSkills.GridLines = True
            Me.lvwSkills.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lvwSkills.Location = New System.Drawing.Point(5, 84)
            Me.lvwSkills.Name = "lvwSkills"
            Me.lvwSkills.ShowItemToolTips = True
            Me.lvwSkills.Size = New System.Drawing.Size(290, 132)
            Me.lvwSkills.TabIndex = 27
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
            Me.radEveHQQueue.Location = New System.Drawing.Point(64, 61)
            Me.radEveHQQueue.Name = "radEveHQQueue"
            Me.radEveHQQueue.Size = New System.Drawing.Size(62, 17)
            Me.radEveHQQueue.TabIndex = 26
            Me.radEveHQQueue.TabStop = True
            Me.radEveHQQueue.Text = "EveHQ:"
            Me.radEveHQQueue.UseVisualStyleBackColor = True
            '
            'radEveQueue
            '
            Me.radEveQueue.AutoSize = True
            Me.radEveQueue.Location = New System.Drawing.Point(15, 61)
            Me.radEveQueue.Name = "radEveQueue"
            Me.radEveQueue.Size = New System.Drawing.Size(43, 17)
            Me.radEveQueue.TabIndex = 25
            Me.radEveQueue.TabStop = True
            Me.radEveQueue.Text = "Eve"
            Me.radEveQueue.UseVisualStyleBackColor = True
            '
            'lblPilot
            '
            Me.lblPilot.AutoSize = True
            Me.lblPilot.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
            Me.lblPilot.Location = New System.Drawing.Point(12, 37)
            Me.lblPilot.Name = "lblPilot"
            Me.lblPilot.Size = New System.Drawing.Size(31, 13)
            Me.lblPilot.TabIndex = 24
            Me.lblPilot.TabStop = True
            Me.lblPilot.Text = "Pilot:"
            '
            'cboPilot
            '
            Me.cboPilot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboPilot.FormattingEnabled = True
            Me.cboPilot.Location = New System.Drawing.Point(49, 34)
            Me.cboPilot.Name = "cboPilot"
            Me.cboPilot.Size = New System.Drawing.Size(234, 21)
            Me.cboPilot.Sorted = True
            Me.cboPilot.TabIndex = 23
            '
            'DBCSkillQueueInfo
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "DBCSkillQueueInfo"
            Me.AGPContent.ResumeLayout(False)
            Me.AGPContent.PerformLayout()
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents cboSkillQueue As System.Windows.Forms.ComboBox
        Friend WithEvents lvwSkills As DevComponents.DotNetBar.Controls.ListViewEx
        Friend WithEvents colSkillName As System.Windows.Forms.ColumnHeader
        Friend WithEvents radEveHQQueue As System.Windows.Forms.RadioButton
        Friend WithEvents radEveQueue As System.Windows.Forms.RadioButton
        Friend WithEvents lblPilot As System.Windows.Forms.LinkLabel
        Friend WithEvents cboPilot As System.Windows.Forms.ComboBox

    End Class
End NameSpace