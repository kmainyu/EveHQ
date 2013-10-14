Imports EveHQ.Classes

Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmSkillDetails
        Inherits DevComponents.DotNetBar.Office2007Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("General", System.Windows.Forms.HorizontalAlignment.Left)
            Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Pilot Specific", System.Windows.Forms.HorizontalAlignment.Left)
            Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Skill Name", ""}, -1)
            Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Skill Rank", ""}, -1)
            Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Skill Group", ""}, -1)
            Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Skill Price", ""}, -1)
            Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Primary Attribute", ""}, -1)
            Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Secondary Attribute", ""}, -1)
            Dim ListViewItem7 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Current Level", ""}, -1)
            Dim ListViewItem8 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Current SP", ""}, -1)
            Dim ListViewItem9 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Time to Next Level", ""}, -1)
            Dim ListViewItem10 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Training Rate (SP/Hr)", ""}, -1)
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSkillDetails))
            Me.tvwReqs = New System.Windows.Forms.TreeView()
            Me.ctxReqs = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuViewSkillDetails = New System.Windows.Forms.ToolStripMenuItem()
            Me.ctxDepend = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuItemName = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuViewItemDetails = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuViewCertDetails = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuViewItemDetailsInIB = New System.Windows.Forms.ToolStripMenuItem()
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.lvwDepend = New System.Windows.Forms.ListView()
            Me.NeededFor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.NeededLevel = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.lvwSPs = New System.Windows.Forms.ListView()
            Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.lvwTimes = New ListViewNoFlicker()
            Me.ToLevel = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.Standard = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.Current = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.Cumulative = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
            Me.lvwDetails = New ListViewNoFlicker()
            Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.TabControl2 = New DevComponents.DotNetBar.TabControl()
            Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
            Me.tiDepends = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
            Me.tiDescription = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel5 = New DevComponents.DotNetBar.TabControlPanel()
            Me.tiTrainingTimes = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel()
            Me.tiPreReqs = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
            Me.tiSkillPoints = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.tiQueues = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel6 = New DevComponents.DotNetBar.TabControlPanel()
            Me.lvwQueues = New System.Windows.Forms.ListView()
            Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.ctxReqs.SuspendLayout()
            Me.ctxDepend.SuspendLayout()
            CType(Me.TabControl2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabControl2.SuspendLayout()
            Me.TabControlPanel3.SuspendLayout()
            Me.TabControlPanel1.SuspendLayout()
            Me.TabControlPanel5.SuspendLayout()
            Me.TabControlPanel2.SuspendLayout()
            Me.TabControlPanel4.SuspendLayout()
            Me.TabControlPanel6.SuspendLayout()
            Me.SuspendLayout()
            '
            'tvwReqs
            '
            Me.tvwReqs.ContextMenuStrip = Me.ctxReqs
            Me.tvwReqs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tvwReqs.Indent = 25
            Me.tvwReqs.ItemHeight = 20
            Me.tvwReqs.Location = New System.Drawing.Point(1, 1)
            Me.tvwReqs.Name = "tvwReqs"
            Me.tvwReqs.ShowPlusMinus = False
            Me.tvwReqs.Size = New System.Drawing.Size(464, 330)
            Me.tvwReqs.TabIndex = 0
            '
            'ctxReqs
            '
            Me.ctxReqs.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.ctxReqs.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator2, Me.mnuViewSkillDetails})
            Me.ctxReqs.Name = "ctxDepend"
            Me.ctxReqs.Size = New System.Drawing.Size(133, 54)
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
            'ctxDepend
            '
            Me.ctxDepend.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.ctxDepend.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuItemName, Me.ToolStripSeparator1, Me.mnuViewItemDetails, Me.mnuViewCertDetails, Me.mnuViewItemDetailsInIB})
            Me.ctxDepend.Name = "ctxDepend"
            Me.ctxDepend.Size = New System.Drawing.Size(212, 98)
            '
            'mnuItemName
            '
            Me.mnuItemName.Name = "mnuItemName"
            Me.mnuItemName.Size = New System.Drawing.Size(211, 22)
            Me.mnuItemName.Text = "Item Name"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(208, 6)
            '
            'mnuViewItemDetails
            '
            Me.mnuViewItemDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.mnuViewItemDetails.Name = "mnuViewItemDetails"
            Me.mnuViewItemDetails.Size = New System.Drawing.Size(211, 22)
            Me.mnuViewItemDetails.Text = "View Details"
            '
            'mnuViewCertDetails
            '
            Me.mnuViewCertDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.mnuViewCertDetails.Name = "mnuViewCertDetails"
            Me.mnuViewCertDetails.Size = New System.Drawing.Size(211, 22)
            Me.mnuViewCertDetails.Text = "View Certificate Details"
            '
            'mnuViewItemDetailsInIB
            '
            Me.mnuViewItemDetailsInIB.Font = New System.Drawing.Font("Tahoma", 8.25!)
            Me.mnuViewItemDetailsInIB.Name = "mnuViewItemDetailsInIB"
            Me.mnuViewItemDetailsInIB.Size = New System.Drawing.Size(211, 22)
            Me.mnuViewItemDetailsInIB.Text = "View Details In Item Browser"
            '
            'lblDescription
            '
            Me.lblDescription.BackColor = System.Drawing.Color.White
            Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblDescription.Location = New System.Drawing.Point(1, 1)
            Me.lblDescription.Margin = New System.Windows.Forms.Padding(5)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Padding = New System.Windows.Forms.Padding(5)
            Me.lblDescription.Size = New System.Drawing.Size(464, 330)
            Me.lblDescription.TabIndex = 0
            '
            'lvwDepend
            '
            Me.lvwDepend.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NeededFor, Me.NeededLevel})
            Me.lvwDepend.ContextMenuStrip = Me.ctxDepend
            Me.lvwDepend.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lvwDepend.FullRowSelect = True
            Me.lvwDepend.GridLines = True
            Me.lvwDepend.Location = New System.Drawing.Point(1, 1)
            Me.lvwDepend.Name = "lvwDepend"
            Me.lvwDepend.ShowItemToolTips = True
            Me.lvwDepend.Size = New System.Drawing.Size(464, 330)
            Me.lvwDepend.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvwDepend.TabIndex = 0
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
            Me.NeededLevel.Text = "Level"
            Me.NeededLevel.Width = 75
            '
            'lvwSPs
            '
            Me.lvwSPs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
            Me.lvwSPs.ContextMenuStrip = Me.ctxDepend
            Me.lvwSPs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lvwSPs.FullRowSelect = True
            Me.lvwSPs.GridLines = True
            Me.lvwSPs.Location = New System.Drawing.Point(1, 1)
            Me.lvwSPs.Name = "lvwSPs"
            Me.lvwSPs.Size = New System.Drawing.Size(464, 330)
            Me.lvwSPs.TabIndex = 2
            Me.lvwSPs.UseCompatibleStateImageBehavior = False
            Me.lvwSPs.View = System.Windows.Forms.View.Details
            '
            'ColumnHeader3
            '
            Me.ColumnHeader3.Text = "To Level"
            '
            'ColumnHeader4
            '
            Me.ColumnHeader4.Text = "SPs to Level Up"
            Me.ColumnHeader4.Width = 125
            '
            'ColumnHeader5
            '
            Me.ColumnHeader5.Text = "Diff from Last Level"
            Me.ColumnHeader5.Width = 125
            '
            'lvwTimes
            '
            Me.lvwTimes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ToLevel, Me.Standard, Me.Current, Me.Cumulative})
            Me.lvwTimes.ContextMenuStrip = Me.ctxDepend
            Me.lvwTimes.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lvwTimes.FullRowSelect = True
            Me.lvwTimes.GridLines = True
            Me.lvwTimes.Location = New System.Drawing.Point(1, 1)
            Me.lvwTimes.Name = "lvwTimes"
            Me.lvwTimes.Size = New System.Drawing.Size(464, 330)
            Me.lvwTimes.TabIndex = 1
            Me.lvwTimes.UseCompatibleStateImageBehavior = False
            Me.lvwTimes.View = System.Windows.Forms.View.Details
            '
            'ToLevel
            '
            Me.ToLevel.Text = "To Level"
            '
            'Standard
            '
            Me.Standard.Text = "Time to Level Up"
            Me.Standard.Width = 125
            '
            'Current
            '
            Me.Current.Text = "Cumulative From 0 SP"
            Me.Current.Width = 125
            '
            'Cumulative
            '
            Me.Cumulative.Text = "Cumulative From Now"
            Me.Cumulative.Width = 125
            '
            'lvwDetails
            '
            Me.lvwDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
            Me.lvwDetails.FullRowSelect = True
            Me.lvwDetails.GridLines = True
            ListViewGroup1.Header = "General"
            ListViewGroup1.Name = "General"
            ListViewGroup2.Header = "Pilot Specific"
            ListViewGroup2.Name = "Specific"
            Me.lvwDetails.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2})
            Me.lvwDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
            ListViewItem1.Group = ListViewGroup1
            ListViewItem2.Group = ListViewGroup1
            ListViewItem3.Group = ListViewGroup1
            ListViewItem4.Group = ListViewGroup1
            ListViewItem5.Group = ListViewGroup1
            ListViewItem6.Group = ListViewGroup1
            ListViewItem7.Group = ListViewGroup2
            ListViewItem8.Group = ListViewGroup2
            ListViewItem9.Group = ListViewGroup2
            ListViewItem10.Group = ListViewGroup2
            Me.lvwDetails.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6, ListViewItem7, ListViewItem8, ListViewItem9, ListViewItem10})
            Me.lvwDetails.Location = New System.Drawing.Point(12, 12)
            Me.lvwDetails.MultiSelect = False
            Me.lvwDetails.Name = "lvwDetails"
            Me.lvwDetails.Scrollable = False
            Me.lvwDetails.Size = New System.Drawing.Size(466, 235)
            Me.lvwDetails.TabIndex = 1
            Me.lvwDetails.UseCompatibleStateImageBehavior = False
            Me.lvwDetails.View = System.Windows.Forms.View.Details
            '
            'ColumnHeader1
            '
            Me.ColumnHeader1.Width = 150
            '
            'ColumnHeader2
            '
            Me.ColumnHeader2.Width = 200
            '
            'TabControl2
            '
            Me.TabControl2.BackColor = System.Drawing.Color.Transparent
            Me.TabControl2.CanReorderTabs = True
            Me.TabControl2.ColorScheme.TabBackground = System.Drawing.Color.Transparent
            Me.TabControl2.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
            Me.TabControl2.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
            Me.TabControl2.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
            Me.TabControl2.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
            Me.TabControl2.Controls.Add(Me.TabControlPanel6)
            Me.TabControl2.Controls.Add(Me.TabControlPanel3)
            Me.TabControl2.Controls.Add(Me.TabControlPanel5)
            Me.TabControl2.Controls.Add(Me.TabControlPanel1)
            Me.TabControl2.Controls.Add(Me.TabControlPanel2)
            Me.TabControl2.Controls.Add(Me.TabControlPanel4)
            Me.TabControl2.Location = New System.Drawing.Point(12, 253)
            Me.TabControl2.Name = "TabControl2"
            Me.TabControl2.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.TabControl2.SelectedTabIndex = 0
            Me.TabControl2.Size = New System.Drawing.Size(466, 355)
            Me.TabControl2.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
            Me.TabControl2.TabIndex = 3
            Me.TabControl2.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
            Me.TabControl2.Tabs.Add(Me.tiDescription)
            Me.TabControl2.Tabs.Add(Me.tiPreReqs)
            Me.TabControl2.Tabs.Add(Me.tiDepends)
            Me.TabControl2.Tabs.Add(Me.tiQueues)
            Me.TabControl2.Tabs.Add(Me.tiSkillPoints)
            Me.TabControl2.Tabs.Add(Me.tiTrainingTimes)
            Me.TabControl2.Text = "TabControl2"
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
            Me.TabControlPanel3.TabItem = Me.tiDepends
            '
            'tiDepends
            '
            Me.tiDepends.AttachedControl = Me.TabControlPanel3
            Me.tiDepends.Name = "tiDepends"
            Me.tiDepends.Text = "Dependancies"
            '
            'TabControlPanel1
            '
            Me.TabControlPanel1.Controls.Add(Me.lblDescription)
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
            Me.TabControlPanel1.TabItem = Me.tiDescription
            '
            'tiDescription
            '
            Me.tiDescription.AttachedControl = Me.TabControlPanel1
            Me.tiDescription.Name = "tiDescription"
            Me.tiDescription.Text = "Description"
            '
            'TabControlPanel5
            '
            Me.TabControlPanel5.Controls.Add(Me.lvwTimes)
            Me.TabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel5.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel5.Name = "TabControlPanel5"
            Me.TabControlPanel5.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel5.Size = New System.Drawing.Size(466, 332)
            Me.TabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
            Me.TabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
            Me.TabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
            Me.TabControlPanel5.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                                                          Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel5.Style.GradientAngle = 90
            Me.TabControlPanel5.TabIndex = 5
            Me.TabControlPanel5.TabItem = Me.tiTrainingTimes
            '
            'tiTrainingTimes
            '
            Me.tiTrainingTimes.AttachedControl = Me.TabControlPanel5
            Me.tiTrainingTimes.Name = "tiTrainingTimes"
            Me.tiTrainingTimes.Text = "Times"
            '
            'TabControlPanel2
            '
            Me.TabControlPanel2.Controls.Add(Me.tvwReqs)
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
            Me.TabControlPanel2.TabItem = Me.tiPreReqs
            '
            'tiPreReqs
            '
            Me.tiPreReqs.AttachedControl = Me.TabControlPanel2
            Me.tiPreReqs.Name = "tiPreReqs"
            Me.tiPreReqs.Text = "Pre-Requisites"
            '
            'TabControlPanel4
            '
            Me.TabControlPanel4.Controls.Add(Me.lvwSPs)
            Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel4.Name = "TabControlPanel4"
            Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel4.Size = New System.Drawing.Size(466, 332)
            Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
            Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
            Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
            Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                                                          Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel4.Style.GradientAngle = 90
            Me.TabControlPanel4.TabIndex = 4
            Me.TabControlPanel4.TabItem = Me.tiSkillPoints
            '
            'tiSkillPoints
            '
            Me.tiSkillPoints.AttachedControl = Me.TabControlPanel4
            Me.tiSkillPoints.Name = "tiSkillPoints"
            Me.tiSkillPoints.Text = "Skill Points"
            '
            'tiQueues
            '
            Me.tiQueues.AttachedControl = Me.TabControlPanel6
            Me.tiQueues.Name = "tiQueues"
            Me.tiQueues.Text = "Queues"
            '
            'TabControlPanel6
            '
            Me.TabControlPanel6.Controls.Add(Me.lvwQueues)
            Me.TabControlPanel6.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel6.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel6.Name = "TabControlPanel6"
            Me.TabControlPanel6.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel6.Size = New System.Drawing.Size(466, 332)
            Me.TabControlPanel6.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
            Me.TabControlPanel6.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
            Me.TabControlPanel6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel6.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
            Me.TabControlPanel6.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                                                          Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel6.Style.GradientAngle = 90
            Me.TabControlPanel6.TabIndex = 6
            Me.TabControlPanel6.TabItem = Me.tiQueues
            '
            'lvwQueues
            '
            Me.lvwQueues.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader7, Me.ColumnHeader8})
            Me.lvwQueues.ContextMenuStrip = Me.ctxDepend
            Me.lvwQueues.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lvwQueues.FullRowSelect = True
            Me.lvwQueues.GridLines = True
            Me.lvwQueues.Location = New System.Drawing.Point(1, 1)
            Me.lvwQueues.Name = "lvwQueues"
            Me.lvwQueues.ShowItemToolTips = True
            Me.lvwQueues.Size = New System.Drawing.Size(464, 330)
            Me.lvwQueues.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvwQueues.TabIndex = 2
            Me.lvwQueues.UseCompatibleStateImageBehavior = False
            Me.lvwQueues.View = System.Windows.Forms.View.Details
            '
            'ColumnHeader7
            '
            Me.ColumnHeader7.Text = "Queue Name"
            Me.ColumnHeader7.Width = 325
            '
            'ColumnHeader8
            '
            Me.ColumnHeader8.Text = "Queued Level"
            Me.ColumnHeader8.Width = 100
            '
            'frmSkillDetails
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(490, 613)
            Me.Controls.Add(Me.TabControl2)
            Me.Controls.Add(Me.lvwDetails)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmSkillDetails"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Skill Details"
            Me.ctxReqs.ResumeLayout(False)
            Me.ctxDepend.ResumeLayout(False)
            CType(Me.TabControl2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabControl2.ResumeLayout(False)
            Me.TabControlPanel3.ResumeLayout(False)
            Me.TabControlPanel1.ResumeLayout(False)
            Me.TabControlPanel5.ResumeLayout(False)
            Me.TabControlPanel2.ResumeLayout(False)
            Me.TabControlPanel4.ResumeLayout(False)
            Me.TabControlPanel6.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
        Friend WithEvents lvwDetails As ListViewNoFlicker
        Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
        Friend WithEvents lvwDepend As System.Windows.Forms.ListView
        Friend WithEvents NeededFor As System.Windows.Forms.ColumnHeader
        Friend WithEvents NeededLevel As System.Windows.Forms.ColumnHeader
        Friend WithEvents ctxDepend As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mnuItemName As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mnuViewItemDetailsInIB As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents lvwTimes As ListViewNoFlicker
        Friend WithEvents ToLevel As System.Windows.Forms.ColumnHeader
        Friend WithEvents Standard As System.Windows.Forms.ColumnHeader
        Friend WithEvents Current As System.Windows.Forms.ColumnHeader
        Friend WithEvents Cumulative As System.Windows.Forms.ColumnHeader
        Friend WithEvents lvwSPs As System.Windows.Forms.ListView
        Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
        Friend WithEvents SkillToolTip As System.Windows.Forms.ToolTip
        Friend WithEvents ctxReqs As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mnuViewSkillDetails As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuViewItemDetails As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuViewCertDetails As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents TabControl2 As DevComponents.DotNetBar.TabControl
        Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents tiSkillPoints As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel5 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents tiTrainingTimes As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents tiDepends As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents tiPreReqs As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents tiDescription As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel6 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents tiQueues As DevComponents.DotNetBar.TabItem
        Friend WithEvents lvwQueues As System.Windows.Forms.ListView
        Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    End Class
End Namespace