<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCertificateDetails
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
        Me.components = New System.ComponentModel.Container
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Containers", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Materials", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Accessories", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Ships", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Modules", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup6 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Charges", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup7 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Blueprints", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup8 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Skills", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup9 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Drones", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup10 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Implants", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup11 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mobile Disruptors", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup12 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("POS Equipment", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup13 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Containers", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup14 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Materials", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup15 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Accessories", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup16 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Ships", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup17 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Modules", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup18 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Charges", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup19 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Blueprints", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup20 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Skills", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup21 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Drones", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup22 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Implants", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup23 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mobile Disruptors", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup24 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("POS Equipment", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup25 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Certificates", System.Windows.Forms.HorizontalAlignment.Left)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCertificateDetails))
        Me.tvwReqs = New System.Windows.Forms.TreeView
        Me.ctxSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewSkillDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.lvwCerts = New System.Windows.Forms.ListView
        Me.ReqCert = New System.Windows.Forms.ColumnHeader
        Me.ReqCertLevel = New System.Windows.Forms.ColumnHeader
        Me.ctxCerts = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCertName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewCertDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.lvwDepend = New System.Windows.Forms.ListView
        Me.NeededFor = New System.Windows.Forms.ColumnHeader
        Me.NeededLevel = New System.Windows.Forms.ColumnHeader
        Me.lblDescription = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tcCerts = New DevComponents.DotNetBar.TabControl
        Me.tiPreReqSkills = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel
        Me.tiPreReqCerts = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel
        Me.tiRequiredFor = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel
        Me.ctxSkills.SuspendLayout()
        Me.ctxCerts.SuspendLayout()
        CType(Me.tcCerts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcCerts.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        Me.TabControlPanel2.SuspendLayout()
        Me.TabControlPanel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvwReqs
        '
        Me.tvwReqs.ContextMenuStrip = Me.ctxSkills
        Me.tvwReqs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwReqs.Indent = 25
        Me.tvwReqs.ItemHeight = 20
        Me.tvwReqs.Location = New System.Drawing.Point(1, 1)
        Me.tvwReqs.Name = "tvwReqs"
        Me.tvwReqs.ShowPlusMinus = False
        Me.tvwReqs.Size = New System.Drawing.Size(464, 330)
        Me.tvwReqs.TabIndex = 0
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
        'lvwCerts
        '
        Me.lvwCerts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ReqCert, Me.ReqCertLevel})
        Me.lvwCerts.ContextMenuStrip = Me.ctxCerts
        Me.lvwCerts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwCerts.FullRowSelect = True
        Me.lvwCerts.GridLines = True
        ListViewGroup1.Header = "Containers"
        ListViewGroup1.Name = "Cat2"
        ListViewGroup2.Header = "Materials"
        ListViewGroup2.Name = "Cat4"
        ListViewGroup3.Header = "Accessories"
        ListViewGroup3.Name = "Cat5"
        ListViewGroup4.Header = "Ships"
        ListViewGroup4.Name = "Cat6"
        ListViewGroup5.Header = "Modules"
        ListViewGroup5.Name = "Cat7"
        ListViewGroup6.Header = "Charges"
        ListViewGroup6.Name = "Cat8"
        ListViewGroup7.Header = "Blueprints"
        ListViewGroup7.Name = "Cat9"
        ListViewGroup8.Header = "Skills"
        ListViewGroup8.Name = "Cat16"
        ListViewGroup9.Header = "Drones"
        ListViewGroup9.Name = "Cat18"
        ListViewGroup10.Header = "Implants"
        ListViewGroup10.Name = "Cat20"
        ListViewGroup11.Header = "Mobile Disruptors"
        ListViewGroup11.Name = "Cat22"
        ListViewGroup12.Header = "POS Equipment"
        ListViewGroup12.Name = "Cat23"
        Me.lvwCerts.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4, ListViewGroup5, ListViewGroup6, ListViewGroup7, ListViewGroup8, ListViewGroup9, ListViewGroup10, ListViewGroup11, ListViewGroup12})
        Me.lvwCerts.Location = New System.Drawing.Point(1, 1)
        Me.lvwCerts.Name = "lvwCerts"
        Me.lvwCerts.ShowGroups = False
        Me.lvwCerts.ShowItemToolTips = True
        Me.lvwCerts.Size = New System.Drawing.Size(464, 330)
        Me.lvwCerts.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwCerts.TabIndex = 0
        Me.lvwCerts.UseCompatibleStateImageBehavior = False
        Me.lvwCerts.View = System.Windows.Forms.View.Details
        '
        'ReqCert
        '
        Me.ReqCert.Text = "Certificate"
        Me.ReqCert.Width = 350
        '
        'ReqCertLevel
        '
        Me.ReqCertLevel.Text = "Grade"
        Me.ReqCertLevel.Width = 75
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
        'lvwDepend
        '
        Me.lvwDepend.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NeededFor, Me.NeededLevel})
        Me.lvwDepend.ContextMenuStrip = Me.ctxCerts
        Me.lvwDepend.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwDepend.FullRowSelect = True
        Me.lvwDepend.GridLines = True
        ListViewGroup13.Header = "Containers"
        ListViewGroup13.Name = "Cat2"
        ListViewGroup14.Header = "Materials"
        ListViewGroup14.Name = "Cat4"
        ListViewGroup15.Header = "Accessories"
        ListViewGroup15.Name = "Cat5"
        ListViewGroup16.Header = "Ships"
        ListViewGroup16.Name = "Cat6"
        ListViewGroup17.Header = "Modules"
        ListViewGroup17.Name = "Cat7"
        ListViewGroup18.Header = "Charges"
        ListViewGroup18.Name = "Cat8"
        ListViewGroup19.Header = "Blueprints"
        ListViewGroup19.Name = "Cat9"
        ListViewGroup20.Header = "Skills"
        ListViewGroup20.Name = "Cat16"
        ListViewGroup21.Header = "Drones"
        ListViewGroup21.Name = "Cat18"
        ListViewGroup22.Header = "Implants"
        ListViewGroup22.Name = "Cat20"
        ListViewGroup23.Header = "Mobile Disruptors"
        ListViewGroup23.Name = "Cat22"
        ListViewGroup24.Header = "POS Equipment"
        ListViewGroup24.Name = "Cat23"
        ListViewGroup25.Header = "Certificates"
        ListViewGroup25.Name = "CatCerts"
        Me.lvwDepend.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup13, ListViewGroup14, ListViewGroup15, ListViewGroup16, ListViewGroup17, ListViewGroup18, ListViewGroup19, ListViewGroup20, ListViewGroup21, ListViewGroup22, ListViewGroup23, ListViewGroup24, ListViewGroup25})
        Me.lvwDepend.Location = New System.Drawing.Point(1, 1)
        Me.lvwDepend.Name = "lvwDepend"
        Me.lvwDepend.ShowItemToolTips = True
        Me.lvwDepend.Size = New System.Drawing.Size(464, 330)
        Me.lvwDepend.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwDepend.TabIndex = 1
        Me.lvwDepend.UseCompatibleStateImageBehavior = False
        Me.lvwDepend.View = System.Windows.Forms.View.Details
        '
        'NeededFor
        '
        Me.NeededFor.Text = "Required For"
        Me.NeededFor.Width = 350
        '
        'NeededLevel
        '
        Me.NeededLevel.Text = "Grade"
        Me.NeededLevel.Width = 75
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
        Me.tcCerts.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tcCerts.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tcCerts.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tcCerts.Controls.Add(Me.TabControlPanel1)
        Me.tcCerts.Controls.Add(Me.TabControlPanel3)
        Me.tcCerts.Controls.Add(Me.TabControlPanel2)
        Me.tcCerts.Location = New System.Drawing.Point(12, 253)
        Me.tcCerts.Name = "tcCerts"
        Me.tcCerts.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tcCerts.SelectedTabIndex = 0
        Me.tcCerts.Size = New System.Drawing.Size(466, 355)
        Me.tcCerts.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tcCerts.TabIndex = 4
        Me.tcCerts.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tcCerts.Tabs.Add(Me.tiPreReqSkills)
        Me.tcCerts.Tabs.Add(Me.tiPreReqCerts)
        Me.tcCerts.Tabs.Add(Me.tiRequiredFor)
        Me.tcCerts.Text = "TabControl2"
        '
        'tiPreReqSkills
        '
        Me.tiPreReqSkills.AttachedControl = Me.TabControlPanel1
        Me.tiPreReqSkills.Name = "tiPreReqSkills"
        Me.tiPreReqSkills.Text = "Pre-Requisite Skills"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.tvwReqs)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(466, 332)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                    Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 1
        Me.TabControlPanel1.TabItem = Me.tiPreReqSkills
        '
        'tiPreReqCerts
        '
        Me.tiPreReqCerts.AttachedControl = Me.TabControlPanel2
        Me.tiPreReqCerts.Name = "tiPreReqCerts"
        Me.tiPreReqCerts.Text = "Pre-Requisite Certificates"
        '
        'TabControlPanel2
        '
        Me.TabControlPanel2.Controls.Add(Me.lvwCerts)
        Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel2.Name = "TabControlPanel2"
        Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel2.Size = New System.Drawing.Size(466, 332)
        Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                    Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel2.Style.GradientAngle = 90
        Me.TabControlPanel2.TabIndex = 2
        Me.TabControlPanel2.TabItem = Me.tiPreReqCerts
        '
        'tiRequiredFor
        '
        Me.tiRequiredFor.AttachedControl = Me.TabControlPanel3
        Me.tiRequiredFor.Name = "tiRequiredFor"
        Me.tiRequiredFor.Text = "Required For"
        '
        'TabControlPanel3
        '
        Me.TabControlPanel3.Controls.Add(Me.lvwDepend)
        Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel3.Name = "TabControlPanel3"
        Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel3.Size = New System.Drawing.Size(466, 332)
        Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                    Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel3.Style.GradientAngle = 90
        Me.TabControlPanel3.TabIndex = 3
        Me.TabControlPanel3.TabItem = Me.tiRequiredFor
        '
        'frmCertificateDetails
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
        Me.Name = "frmCertificateDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Certificate Details"
        Me.ctxSkills.ResumeLayout(False)
        Me.ctxCerts.ResumeLayout(False)
        CType(Me.tcCerts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcCerts.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        Me.TabControlPanel2.ResumeLayout(False)
        Me.TabControlPanel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
    Friend WithEvents lvwCerts As System.Windows.Forms.ListView
    Friend WithEvents ReqCert As System.Windows.Forms.ColumnHeader
    Friend WithEvents ReqCertLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxSkills As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewSkillDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ctxCerts As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCertName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewCertDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lvwDepend As System.Windows.Forms.ListView
    Friend WithEvents NeededFor As System.Windows.Forms.ColumnHeader
    Friend WithEvents NeededLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents tcCerts As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiPreReqSkills As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiRequiredFor As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiPreReqCerts As DevComponents.DotNetBar.TabItem
End Class
