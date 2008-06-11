<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSkillDetails
    Inherits System.Windows.Forms.Form

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
        Dim ListViewGroup13 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("General", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup14 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Pilot Specific", System.Windows.Forms.HorizontalAlignment.Left)
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
        Me.tvwReqs = New System.Windows.Forms.TreeView
        Me.ctxReqs = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewSkillDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxDepend = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuItemName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewItemDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewItemDetailsInIB = New System.Windows.Forms.ToolStripMenuItem
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.lblDescription = New System.Windows.Forms.Label
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.lvwDepend = New System.Windows.Forms.ListView
        Me.NeededFor = New System.Windows.Forms.ColumnHeader
        Me.NeededLevel = New System.Windows.Forms.ColumnHeader
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.lvwSPs = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.lvwTimes = New EveHQ.ListViewNoFlicker
        Me.ToLevel = New System.Windows.Forms.ColumnHeader
        Me.Standard = New System.Windows.Forms.ColumnHeader
        Me.Current = New System.Windows.Forms.ColumnHeader
        Me.Cumulative = New System.Windows.Forms.ColumnHeader
        Me.lvwDetails = New EveHQ.ListViewNoFlicker
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ctxReqs.SuspendLayout()
        Me.ctxDepend.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvwReqs
        '
        Me.tvwReqs.ContextMenuStrip = Me.ctxReqs
        Me.tvwReqs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwReqs.Indent = 25
        Me.tvwReqs.ItemHeight = 20
        Me.tvwReqs.Location = New System.Drawing.Point(3, 3)
        Me.tvwReqs.Name = "tvwReqs"
        Me.tvwReqs.ShowPlusMinus = False
        Me.tvwReqs.Size = New System.Drawing.Size(452, 323)
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
        Me.ctxDepend.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuItemName, Me.ToolStripSeparator1, Me.mnuViewItemDetails, Me.mnuViewItemDetailsInIB})
        Me.ctxDepend.Name = "ctxDepend"
        Me.ctxDepend.Size = New System.Drawing.Size(212, 76)
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
        'mnuViewItemDetailsInIB
        '
        Me.mnuViewItemDetailsInIB.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewItemDetailsInIB.Name = "mnuViewItemDetailsInIB"
        Me.mnuViewItemDetailsInIB.Size = New System.Drawing.Size(211, 22)
        Me.mnuViewItemDetailsInIB.Text = "View Details In Item Browser"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Location = New System.Drawing.Point(12, 253)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(466, 355)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.lblDescription)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(458, 329)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Description"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.Color.White
        Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDescription.Location = New System.Drawing.Point(0, 0)
        Me.lblDescription.Margin = New System.Windows.Forms.Padding(5)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Padding = New System.Windows.Forms.Padding(5)
        Me.lblDescription.Size = New System.Drawing.Size(458, 329)
        Me.lblDescription.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.tvwReqs)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(458, 329)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Pre-requisites"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.lvwDepend)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(458, 329)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Dependancies"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'lvwDepend
        '
        Me.lvwDepend.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NeededFor, Me.NeededLevel})
        Me.lvwDepend.ContextMenuStrip = Me.ctxDepend
        Me.lvwDepend.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwDepend.FullRowSelect = True
        Me.lvwDepend.GridLines = True
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
        Me.lvwDepend.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4, ListViewGroup5, ListViewGroup6, ListViewGroup7, ListViewGroup8, ListViewGroup9, ListViewGroup10, ListViewGroup11, ListViewGroup12})
        Me.lvwDepend.Location = New System.Drawing.Point(3, 3)
        Me.lvwDepend.Name = "lvwDepend"
        Me.lvwDepend.ShowItemToolTips = True
        Me.lvwDepend.Size = New System.Drawing.Size(452, 323)
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
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.lvwSPs)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(458, 329)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Skill Points"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'lvwSPs
        '
        Me.lvwSPs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.lvwSPs.ContextMenuStrip = Me.ctxDepend
        Me.lvwSPs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwSPs.FullRowSelect = True
        Me.lvwSPs.GridLines = True
        Me.lvwSPs.Location = New System.Drawing.Point(0, 0)
        Me.lvwSPs.Name = "lvwSPs"
        Me.lvwSPs.Size = New System.Drawing.Size(458, 329)
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
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.lvwTimes)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(458, 329)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Training Times"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'lvwTimes
        '
        Me.lvwTimes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ToLevel, Me.Standard, Me.Current, Me.Cumulative})
        Me.lvwTimes.ContextMenuStrip = Me.ctxDepend
        Me.lvwTimes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwTimes.FullRowSelect = True
        Me.lvwTimes.GridLines = True
        Me.lvwTimes.Location = New System.Drawing.Point(0, 0)
        Me.lvwTimes.Name = "lvwTimes"
        Me.lvwTimes.Size = New System.Drawing.Size(458, 329)
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
        ListViewGroup13.Header = "General"
        ListViewGroup13.Name = "General"
        ListViewGroup14.Header = "Pilot Specific"
        ListViewGroup14.Name = "Specific"
        Me.lvwDetails.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup13, ListViewGroup14})
        Me.lvwDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        ListViewItem1.Group = ListViewGroup13
        ListViewItem2.Group = ListViewGroup13
        ListViewItem3.Group = ListViewGroup13
        ListViewItem4.Group = ListViewGroup13
        ListViewItem5.Group = ListViewGroup13
        ListViewItem6.Group = ListViewGroup13
        ListViewItem7.Group = ListViewGroup14
        ListViewItem8.Group = ListViewGroup14
        ListViewItem9.Group = ListViewGroup14
        ListViewItem10.Group = ListViewGroup14
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
        'frmSkillDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(490, 614)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.lvwDetails)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSkillDetails"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Skill Details"
        Me.ctxReqs.ResumeLayout(False)
        Me.ctxDepend.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
    Friend WithEvents lvwDetails As EveHQ.ListViewNoFlicker
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents lvwDepend As System.Windows.Forms.ListView
    Friend WithEvents NeededFor As System.Windows.Forms.ColumnHeader
    Friend WithEvents NeededLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxDepend As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewItemDetailsInIB As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents lvwTimes As EveHQ.ListViewNoFlicker
    Friend WithEvents ToLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents Standard As System.Windows.Forms.ColumnHeader
    Friend WithEvents Current As System.Windows.Forms.ColumnHeader
    Friend WithEvents Cumulative As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
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
End Class
