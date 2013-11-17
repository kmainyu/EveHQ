Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmCertificateDetails
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
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCertificateDetails))
            Me.tvwBasicReqs = New System.Windows.Forms.TreeView()
            Me.ctxSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuViewSkillDetails = New System.Windows.Forms.ToolStripMenuItem()
            Me.ctxCerts = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuCertName = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuViewCertDetails = New System.Windows.Forms.ToolStripMenuItem()
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.tcCerts = New DevComponents.DotNetBar.TabControl()
            Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel()
            Me.tvwStandardReqs = New System.Windows.Forms.TreeView()
            Me.standardSkillsTab = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
            Me.basicSkillsTab = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel5 = New DevComponents.DotNetBar.TabControlPanel()
            Me.eliteSkillsTab = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
            Me.advancedSkillsTab = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
            Me.improvedSkillsTab = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.tvwImprovedReqs = New System.Windows.Forms.TreeView()
            Me.tvwAdvancedReqs = New System.Windows.Forms.TreeView()
            Me.tvwEliteReqs = New System.Windows.Forms.TreeView()
            Me.ctxSkills.SuspendLayout()
            Me.ctxCerts.SuspendLayout()
            CType(Me.tcCerts, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tcCerts.SuspendLayout()
            Me.TabControlPanel2.SuspendLayout()
            Me.TabControlPanel1.SuspendLayout()
            Me.TabControlPanel5.SuspendLayout()
            Me.TabControlPanel4.SuspendLayout()
            Me.TabControlPanel3.SuspendLayout()
            Me.SuspendLayout()
            '
            'tvwBasicReqs
            '
            Me.tvwBasicReqs.ContextMenuStrip = Me.ctxSkills
            Me.tvwBasicReqs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tvwBasicReqs.Indent = 25
            Me.tvwBasicReqs.ItemHeight = 20
            Me.tvwBasicReqs.Location = New System.Drawing.Point(1, 1)
            Me.tvwBasicReqs.Name = "tvwBasicReqs"
            Me.tvwBasicReqs.ShowPlusMinus = False
            Me.tvwBasicReqs.Size = New System.Drawing.Size(464, 330)
            Me.tvwBasicReqs.TabIndex = 0
            '
            'ctxSkills
            '
            Me.ctxSkills.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.ctxSkills.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator2, Me.mnuViewSkillDetails})
            Me.ctxSkills.Name = "ctxDepend"
            Me.ctxSkills.Size = New System.Drawing.Size(133, 54)
            '
            'mnuSkillName
            '
            Me.mnuSkillName.Name = "mnuSkillName"
            Me.mnuSkillName.Size = New System.Drawing.Size(132, 22)
            Me.mnuSkillName.Text = "Skill Name"
            '
            'ToolStripSeparator2
            '
            Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
            Me.ToolStripSeparator2.Size = New System.Drawing.Size(129, 6)
            '
            'mnuViewSkillDetails
            '
            Me.mnuViewSkillDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.mnuViewSkillDetails.Name = "mnuViewSkillDetails"
            Me.mnuViewSkillDetails.Size = New System.Drawing.Size(132, 22)
            Me.mnuViewSkillDetails.Text = "View Details"
            '
            'ctxCerts
            '
            Me.ctxCerts.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.ctxCerts.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCertName, Me.ToolStripSeparator1, Me.mnuViewCertDetails})
            Me.ctxCerts.Name = "ctxDepend"
            Me.ctxCerts.Size = New System.Drawing.Size(133, 54)
            '
            'mnuCertName
            '
            Me.mnuCertName.Name = "mnuCertName"
            Me.mnuCertName.Size = New System.Drawing.Size(132, 22)
            Me.mnuCertName.Text = "Skill Name"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(129, 6)
            '
            'mnuViewCertDetails
            '
            Me.mnuViewCertDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.mnuViewCertDetails.Name = "mnuViewCertDetails"
            Me.mnuViewCertDetails.Size = New System.Drawing.Size(132, 22)
            Me.mnuViewCertDetails.Text = "View Details"
            '
            'lblDescription
            '
            Me.lblDescription.BackColor = System.Drawing.Color.White
            Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDescription.Location = New System.Drawing.Point(12, 14)
            Me.lblDescription.Margin = New System.Windows.Forms.Padding(5)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Padding = New System.Windows.Forms.Padding(5)
            Me.lblDescription.Size = New System.Drawing.Size(466, 231)
            Me.lblDescription.TabIndex = 0
            '
            'tcCerts
            '
            Me.tcCerts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tcCerts.BackColor = System.Drawing.Color.Transparent
            Me.tcCerts.CanReorderTabs = True
            Me.tcCerts.ColorScheme.TabBackground = System.Drawing.Color.Transparent
            Me.tcCerts.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
            Me.tcCerts.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(249, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(248, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(245, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
            Me.tcCerts.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
            Me.tcCerts.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 1.0!)})
            Me.tcCerts.Controls.Add(Me.TabControlPanel5)
            Me.tcCerts.Controls.Add(Me.TabControlPanel4)
            Me.tcCerts.Controls.Add(Me.TabControlPanel3)
            Me.tcCerts.Controls.Add(Me.TabControlPanel2)
            Me.tcCerts.Controls.Add(Me.TabControlPanel1)
            Me.tcCerts.Location = New System.Drawing.Point(12, 253)
            Me.tcCerts.Name = "tcCerts"
            Me.tcCerts.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.tcCerts.SelectedTabIndex = 0
            Me.tcCerts.Size = New System.Drawing.Size(466, 355)
            Me.tcCerts.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
            Me.tcCerts.TabIndex = 4
            Me.tcCerts.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
            Me.tcCerts.Tabs.Add(Me.basicSkillsTab)
            Me.tcCerts.Tabs.Add(Me.standardSkillsTab)
            Me.tcCerts.Tabs.Add(Me.improvedSkillsTab)
            Me.tcCerts.Tabs.Add(Me.advancedSkillsTab)
            Me.tcCerts.Tabs.Add(Me.eliteSkillsTab)
            Me.tcCerts.Text = "TabControl2"
            '
            'TabControlPanel2
            '
            Me.TabControlPanel2.Controls.Add(Me.tvwStandardReqs)
            Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel2.Name = "TabControlPanel2"
            Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel2.Size = New System.Drawing.Size(466, 332)
            Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
            Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
            Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
            Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel2.Style.GradientAngle = 90
            Me.TabControlPanel2.TabIndex = 2
            Me.TabControlPanel2.TabItem = Me.standardSkillsTab
            '
            'tvwStandardReqs
            '
            Me.tvwStandardReqs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tvwStandardReqs.Location = New System.Drawing.Point(0, 0)
            Me.tvwStandardReqs.Name = "tvwStandardReqs"
            Me.tvwStandardReqs.Size = New System.Drawing.Size(466, 332)
            Me.tvwStandardReqs.TabIndex = 0
            '
            'standardSkillsTab
            '
            Me.standardSkillsTab.AttachedControl = Me.TabControlPanel2
            Me.standardSkillsTab.Name = "standardSkillsTab"
            Me.standardSkillsTab.Text = "Standard"
            '
            'TabControlPanel1
            '
            Me.TabControlPanel1.Controls.Add(Me.tvwBasicReqs)
            Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel1.Name = "TabControlPanel1"
            Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel1.Size = New System.Drawing.Size(466, 332)
            Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
            Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
            Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
            Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel1.Style.GradientAngle = 90
            Me.TabControlPanel1.TabIndex = 1
            Me.TabControlPanel1.TabItem = Me.basicSkillsTab
            '
            'basicSkillsTab
            '
            Me.basicSkillsTab.AttachedControl = Me.TabControlPanel1
            Me.basicSkillsTab.Name = "basicSkillsTab"
            Me.basicSkillsTab.Text = "Basic"
            '
            'TabControlPanel5
            '
            Me.TabControlPanel5.Controls.Add(Me.tvwEliteReqs)
            Me.TabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel5.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel5.Name = "TabControlPanel5"
            Me.TabControlPanel5.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel5.Size = New System.Drawing.Size(466, 332)
            Me.TabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
            Me.TabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
            Me.TabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
            Me.TabControlPanel5.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel5.Style.GradientAngle = 90
            Me.TabControlPanel5.TabIndex = 5
            Me.TabControlPanel5.TabItem = Me.eliteSkillsTab
            '
            'eliteSkillsTab
            '
            Me.eliteSkillsTab.AttachedControl = Me.TabControlPanel5
            Me.eliteSkillsTab.Name = "eliteSkillsTab"
            Me.eliteSkillsTab.Text = "Elite"
            '
            'TabControlPanel4
            '
            Me.TabControlPanel4.Controls.Add(Me.tvwAdvancedReqs)
            Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel4.Name = "TabControlPanel4"
            Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel4.Size = New System.Drawing.Size(466, 332)
            Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
            Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
            Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
            Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel4.Style.GradientAngle = 90
            Me.TabControlPanel4.TabIndex = 4
            Me.TabControlPanel4.TabItem = Me.advancedSkillsTab
            '
            'advancedSkillsTab
            '
            Me.advancedSkillsTab.AttachedControl = Me.TabControlPanel4
            Me.advancedSkillsTab.Name = "advancedSkillsTab"
            Me.advancedSkillsTab.Text = "Advanced"
            '
            'TabControlPanel3
            '
            Me.TabControlPanel3.Controls.Add(Me.tvwImprovedReqs)
            Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel3.Name = "TabControlPanel3"
            Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel3.Size = New System.Drawing.Size(466, 332)
            Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
            Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
            Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
            Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel3.Style.GradientAngle = 90
            Me.TabControlPanel3.TabIndex = 3
            Me.TabControlPanel3.TabItem = Me.improvedSkillsTab
            '
            'improvedSkillsTab
            '
            Me.improvedSkillsTab.AttachedControl = Me.TabControlPanel3
            Me.improvedSkillsTab.Name = "improvedSkillsTab"
            Me.improvedSkillsTab.Text = "Improved"
            '
            'tvwImprovedReqs
            '
            Me.tvwImprovedReqs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tvwImprovedReqs.Location = New System.Drawing.Point(0, 0)
            Me.tvwImprovedReqs.Name = "tvwImprovedReqs"
            Me.tvwImprovedReqs.Size = New System.Drawing.Size(466, 332)
            Me.tvwImprovedReqs.TabIndex = 1
            '
            'tvwAdvancedReqs
            '
            Me.tvwAdvancedReqs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tvwAdvancedReqs.Location = New System.Drawing.Point(0, 0)
            Me.tvwAdvancedReqs.Name = "tvwAdvancedReqs"
            Me.tvwAdvancedReqs.Size = New System.Drawing.Size(466, 332)
            Me.tvwAdvancedReqs.TabIndex = 1
            '
            'tvwEliteReqs
            '
            Me.tvwEliteReqs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tvwEliteReqs.Location = New System.Drawing.Point(0, 0)
            Me.tvwEliteReqs.Name = "tvwEliteReqs"
            Me.tvwEliteReqs.Size = New System.Drawing.Size(466, 336)
            Me.tvwEliteReqs.TabIndex = 1
            '
            'FrmCertificateDetails
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(490, 614)
            Me.Controls.Add(Me.tcCerts)
            Me.Controls.Add(Me.lblDescription)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FrmCertificateDetails"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Certificate Details"
            Me.ctxSkills.ResumeLayout(False)
            Me.ctxCerts.ResumeLayout(False)
            CType(Me.tcCerts, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tcCerts.ResumeLayout(False)
            Me.TabControlPanel2.ResumeLayout(False)
            Me.TabControlPanel1.ResumeLayout(False)
            Me.TabControlPanel5.ResumeLayout(False)
            Me.TabControlPanel4.ResumeLayout(False)
            Me.TabControlPanel3.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents tvwBasicReqs As System.Windows.Forms.TreeView
        Friend WithEvents ctxSkills As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mnuViewSkillDetails As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
        Friend WithEvents ctxCerts As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mnuCertName As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mnuViewCertDetails As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tcCerts As DevComponents.DotNetBar.TabControl
        Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents basicSkillsTab As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents improvedSkillsTab As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents standardSkillsTab As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents advancedSkillsTab As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel5 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents eliteSkillsTab As DevComponents.DotNetBar.TabItem
        Friend WithEvents tvwStandardReqs As System.Windows.Forms.TreeView
        Friend WithEvents tvwEliteReqs As System.Windows.Forms.TreeView
        Friend WithEvents tvwAdvancedReqs As System.Windows.Forms.TreeView
        Friend WithEvents tvwImprovedReqs As System.Windows.Forms.TreeView
    End Class
End NameSpace