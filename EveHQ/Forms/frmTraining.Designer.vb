<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTraining
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTraining))
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
        Me.tvwSkillList = New System.Windows.Forms.TreeView()
        Me.ctxDetails = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAddToQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddToQueueNext = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAddToQueue1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddToQueue2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddToQueue3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddToQueue4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddToQueue5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddGroupToQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddGroupLevel1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddGroupLevel2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddGroupLevel3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddGroupLevel4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddGroupLevel5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuForceTraining2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewDetails2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExpandAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCollapseAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ctxQueue = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuChangeLevel = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeLevel1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeLevel2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeLevel3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeLevel4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeLevel5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuIncreaseLevel = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDecreaseLevel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMoveUpQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMoveDownQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSeparateLevels = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeparateAllLevels = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeparateTopLevel = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeparateBottomLevel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSplitQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeperateLevelSep = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuDeleteFromQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRemoveTrainedSkills = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuClearTrainingQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEditNote = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuForceTraining = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.tvwReqs = New System.Windows.Forms.TreeView()
        Me.ctxReqs = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuReqsSkillName = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuReqsViewDetailsHere = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReqsViewDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.lvwDepend = New System.Windows.Forms.ListView()
        Me.NeededFor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.NeededLevel = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ctxDepend = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuItemName = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewItemDetailsHere = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewItemDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewItemDetailsInIB = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewItemDetailsInCertScreen = New System.Windows.Forms.ToolStripMenuItem()
        Me.lvwSPs = New System.Windows.Forms.ListView()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblTotalQueueTime = New System.Windows.Forms.Label()
        Me.chkOmitQueuesSkills = New System.Windows.Forms.CheckBox()
        Me.cboCertFilter = New System.Windows.Forms.ComboBox()
        Me.lblCertFilter = New System.Windows.Forms.Label()
        Me.tvwCertList = New System.Windows.Forms.TreeView()
        Me.ctxCertDetails = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCertName = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAddCertToQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertToQueueNext = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAddCertToQueue1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertToQueue2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertToQueue3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertToQueue4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertToQueue5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertGroupToQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertGroupToQueue1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertGroupToQueue2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertGroupToQueue3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertGroupToQueue4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCertGroupToQueue5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewCertDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblPilot = New System.Windows.Forms.Label()
        Me.RibbonBarMergeContainer1 = New DevComponents.DotNetBar.RibbonBarMergeContainer()
        Me.rbImportExport = New DevComponents.DotNetBar.RibbonBar()
        Me.ItemContainer1 = New DevComponents.DotNetBar.ItemContainer()
        Me.btnEveMonImport = New DevComponents.DotNetBar.ButtonItem()
        Me.btnImportEMPFile = New DevComponents.DotNetBar.ButtonItem()
        Me.btnExportEMPFile = New DevComponents.DotNetBar.ButtonItem()
        Me.rbAttributeTools = New DevComponents.DotNetBar.RibbonBar()
        Me.btnImplants = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRemap = New DevComponents.DotNetBar.ButtonItem()
        Me.rbQueueSettings = New DevComponents.DotNetBar.RibbonBar()
        Me.btnQueueSettings = New DevComponents.DotNetBar.ButtonItem()
        Me.rbQueueFunctions = New DevComponents.DotNetBar.RibbonBar()
        Me.btnRBAddSkill = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBDeleteSkill = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBIncreaseLevel = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBDecreaseLevel = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBMoveUpQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBMoveDownQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBClearQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBSplitQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.rbQueueAdmin = New DevComponents.DotNetBar.RibbonBar()
        Me.btnRBAddQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBEditQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBDeleteQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBSetPrimaryQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBMergeQueues = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBCopyQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRBCopyQueueToPilot = New DevComponents.DotNetBar.ButtonItem()
        Me.btnIncTraining = New DevComponents.DotNetBar.ButtonItem()
        Me.SuperTooltip1 = New DevComponents.DotNetBar.SuperTooltip()
        Me.tabQueueMode = New DevComponents.DotNetBar.TabControl()
        Me.tcpSkillPlanning = New DevComponents.DotNetBar.TabControlPanel()
        Me.cboFilter = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.btnCollapseAll = New DevComponents.DotNetBar.ButtonX()
        Me.btnExpandAll = New DevComponents.DotNetBar.ButtonX()
        Me.tiSkillPlanning = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tcpCertPlanning = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiCertPLanning = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tabSkillDetails = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lvwDetails = New EveHQ.ListViewNoFlicker()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiGeneral = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel7 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lvwTimes = New EveHQ.ListViewNoFlicker()
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Standard = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Current = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Cumulative = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabItem6 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel5 = New DevComponents.DotNetBar.TabControlPanel()
        Me.TabItem4 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
        Me.TabItem3 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
        Me.TabItem2 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel()
        Me.TabItem1 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tabQueues = New DevComponents.DotNetBar.TabControl()
        Me.tcpQueue = New DevComponents.DotNetBar.TabControlPanel()
        Me.lvQueues = New EveHQ.ListViewNoFlicker()
        Me.colQName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colQSkills = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colQTimeLeft = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colQQueuedTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colQEndDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ctxQueues = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEditQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeleteQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMergeQueues = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCopyQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCopyQueueToPilot = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSetPrimary = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabSummary = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.panelInfo = New DevComponents.DotNetBar.PanelEx()
        Me.cboPilots = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ExpandableSplitter1 = New DevComponents.DotNetBar.ExpandableSplitter()
        Me.ctxDetails.SuspendLayout()
        Me.ctxQueue.SuspendLayout()
        Me.ctxReqs.SuspendLayout()
        Me.ctxDepend.SuspendLayout()
        Me.ctxCertDetails.SuspendLayout()
        Me.RibbonBarMergeContainer1.SuspendLayout()
        CType(Me.tabQueueMode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabQueueMode.SuspendLayout()
        Me.tcpSkillPlanning.SuspendLayout()
        Me.tcpCertPlanning.SuspendLayout()
        CType(Me.tabSkillDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabSkillDetails.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        Me.TabControlPanel7.SuspendLayout()
        Me.TabControlPanel5.SuspendLayout()
        Me.TabControlPanel4.SuspendLayout()
        Me.TabControlPanel3.SuspendLayout()
        Me.TabControlPanel2.SuspendLayout()
        CType(Me.tabQueues, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabQueues.SuspendLayout()
        Me.tcpQueue.SuspendLayout()
        Me.ctxQueues.SuspendLayout()
        Me.panelInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvwSkillList
        '
        Me.tvwSkillList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwSkillList.ContextMenuStrip = Me.ctxDetails
        Me.tvwSkillList.FullRowSelect = True
        Me.tvwSkillList.HideSelection = False
        Me.tvwSkillList.ImageKey = "Blank.jpg"
        Me.tvwSkillList.ImageList = Me.ImageList1
        Me.tvwSkillList.Indent = 20
        Me.tvwSkillList.Location = New System.Drawing.Point(4, 34)
        Me.tvwSkillList.Name = "tvwSkillList"
        Me.tvwSkillList.SelectedImageIndex = 6
        Me.tvwSkillList.Size = New System.Drawing.Size(422, 418)
        Me.tvwSkillList.TabIndex = 0
        '
        'ctxDetails
        '
        Me.ctxDetails.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxDetails.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName2, Me.ToolStripSeparator5, Me.mnuAddToQueue, Me.mnuAddGroupToQueue, Me.ToolStripSeparator6, Me.mnuForceTraining2, Me.ToolStripMenuItem1, Me.mnuViewDetails2, Me.ToolStripMenuItem7, Me.mnuExpandAll, Me.mnuCollapseAll})
        Me.ctxDetails.Name = "ctxDepend"
        Me.ctxDetails.Size = New System.Drawing.Size(217, 182)
        '
        'mnuSkillName2
        '
        Me.mnuSkillName2.Name = "mnuSkillName2"
        Me.mnuSkillName2.Size = New System.Drawing.Size(216, 22)
        Me.mnuSkillName2.Text = "Skill Name"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(213, 6)
        '
        'mnuAddToQueue
        '
        Me.mnuAddToQueue.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddToQueueNext, Me.ToolStripMenuItem4, Me.mnuAddToQueue1, Me.mnuAddToQueue2, Me.mnuAddToQueue3, Me.mnuAddToQueue4, Me.mnuAddToQueue5})
        Me.mnuAddToQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuAddToQueue.Name = "mnuAddToQueue"
        Me.mnuAddToQueue.Size = New System.Drawing.Size(216, 22)
        Me.mnuAddToQueue.Text = "Add to Training Queue"
        '
        'mnuAddToQueueNext
        '
        Me.mnuAddToQueueNext.Name = "mnuAddToQueueNext"
        Me.mnuAddToQueueNext.Size = New System.Drawing.Size(125, 22)
        Me.mnuAddToQueueNext.Text = "Next Level"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(122, 6)
        '
        'mnuAddToQueue1
        '
        Me.mnuAddToQueue1.Name = "mnuAddToQueue1"
        Me.mnuAddToQueue1.Size = New System.Drawing.Size(125, 22)
        Me.mnuAddToQueue1.Text = "Level 1"
        '
        'mnuAddToQueue2
        '
        Me.mnuAddToQueue2.Name = "mnuAddToQueue2"
        Me.mnuAddToQueue2.Size = New System.Drawing.Size(125, 22)
        Me.mnuAddToQueue2.Text = "Level 2"
        '
        'mnuAddToQueue3
        '
        Me.mnuAddToQueue3.Name = "mnuAddToQueue3"
        Me.mnuAddToQueue3.Size = New System.Drawing.Size(125, 22)
        Me.mnuAddToQueue3.Text = "Level 3"
        '
        'mnuAddToQueue4
        '
        Me.mnuAddToQueue4.Name = "mnuAddToQueue4"
        Me.mnuAddToQueue4.Size = New System.Drawing.Size(125, 22)
        Me.mnuAddToQueue4.Text = "Level 4"
        '
        'mnuAddToQueue5
        '
        Me.mnuAddToQueue5.Name = "mnuAddToQueue5"
        Me.mnuAddToQueue5.Size = New System.Drawing.Size(125, 22)
        Me.mnuAddToQueue5.Text = "Level 5"
        '
        'mnuAddGroupToQueue
        '
        Me.mnuAddGroupToQueue.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddGroupLevel1, Me.mnuAddGroupLevel2, Me.mnuAddGroupLevel3, Me.mnuAddGroupLevel4, Me.mnuAddGroupLevel5})
        Me.mnuAddGroupToQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuAddGroupToQueue.Name = "mnuAddGroupToQueue"
        Me.mnuAddGroupToQueue.Size = New System.Drawing.Size(216, 22)
        Me.mnuAddGroupToQueue.Text = "Add Group To Training Queue"
        '
        'mnuAddGroupLevel1
        '
        Me.mnuAddGroupLevel1.Name = "mnuAddGroupLevel1"
        Me.mnuAddGroupLevel1.Size = New System.Drawing.Size(123, 22)
        Me.mnuAddGroupLevel1.Text = "To Level 1"
        '
        'mnuAddGroupLevel2
        '
        Me.mnuAddGroupLevel2.Name = "mnuAddGroupLevel2"
        Me.mnuAddGroupLevel2.Size = New System.Drawing.Size(123, 22)
        Me.mnuAddGroupLevel2.Text = "To Level 2"
        '
        'mnuAddGroupLevel3
        '
        Me.mnuAddGroupLevel3.Name = "mnuAddGroupLevel3"
        Me.mnuAddGroupLevel3.Size = New System.Drawing.Size(123, 22)
        Me.mnuAddGroupLevel3.Text = "To Level 3"
        '
        'mnuAddGroupLevel4
        '
        Me.mnuAddGroupLevel4.Name = "mnuAddGroupLevel4"
        Me.mnuAddGroupLevel4.Size = New System.Drawing.Size(123, 22)
        Me.mnuAddGroupLevel4.Text = "To Level 4"
        '
        'mnuAddGroupLevel5
        '
        Me.mnuAddGroupLevel5.Name = "mnuAddGroupLevel5"
        Me.mnuAddGroupLevel5.Size = New System.Drawing.Size(123, 22)
        Me.mnuAddGroupLevel5.Text = "To Level 5"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(213, 6)
        '
        'mnuForceTraining2
        '
        Me.mnuForceTraining2.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuForceTraining2.Name = "mnuForceTraining2"
        Me.mnuForceTraining2.Size = New System.Drawing.Size(216, 22)
        Me.mnuForceTraining2.Text = "Force Skill Training"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(213, 6)
        '
        'mnuViewDetails2
        '
        Me.mnuViewDetails2.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewDetails2.Name = "mnuViewDetails2"
        Me.mnuViewDetails2.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewDetails2.Text = "View Details"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(213, 6)
        '
        'mnuExpandAll
        '
        Me.mnuExpandAll.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuExpandAll.Name = "mnuExpandAll"
        Me.mnuExpandAll.Size = New System.Drawing.Size(216, 22)
        Me.mnuExpandAll.Text = "Expand All"
        '
        'mnuCollapseAll
        '
        Me.mnuCollapseAll.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuCollapseAll.Name = "mnuCollapseAll"
        Me.mnuCollapseAll.Size = New System.Drawing.Size(216, 22)
        Me.mnuCollapseAll.Text = "Collapse All"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Level0.jpg")
        Me.ImageList1.Images.SetKeyName(1, "Level1.jpg")
        Me.ImageList1.Images.SetKeyName(2, "Level2.jpg")
        Me.ImageList1.Images.SetKeyName(3, "Level3.jpg")
        Me.ImageList1.Images.SetKeyName(4, "Level4.jpg")
        Me.ImageList1.Images.SetKeyName(5, "Level5.jpg")
        Me.ImageList1.Images.SetKeyName(6, "Blank.jpg")
        Me.ImageList1.Images.SetKeyName(7, "Skillbook_16x16.jpg")
        Me.ImageList1.Images.SetKeyName(8, "Skillbook_24x24.jpg")
        Me.ImageList1.Images.SetKeyName(9, "NoSkillbook_16x16.jpg")
        Me.ImageList1.Images.SetKeyName(10, "NoSkillbook_24x24.jpg")
        '
        'ctxQueue
        '
        Me.ctxQueue.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxQueue.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator1, Me.mnuChangeLevel, Me.mnuIncreaseLevel, Me.mnuDecreaseLevel, Me.ToolStripSeparator3, Me.mnuMoveUpQueue, Me.mnuMoveDownQueue, Me.ToolStripMenuItem3, Me.mnuSeparateLevels, Me.ToolStripMenuItem8, Me.mnuSplitQueue, Me.mnuSeperateLevelSep, Me.mnuDeleteFromQueue, Me.mnuRemoveTrainedSkills, Me.mnuClearTrainingQueue, Me.ToolStripSeparator2, Me.mnuViewDetails, Me.mnuEditNote, Me.ToolStripMenuItem2, Me.mnuForceTraining})
        Me.ctxQueue.Name = "ctxDepend"
        Me.ctxQueue.Size = New System.Drawing.Size(207, 354)
        '
        'mnuSkillName
        '
        Me.mnuSkillName.Name = "mnuSkillName"
        Me.mnuSkillName.Size = New System.Drawing.Size(206, 22)
        Me.mnuSkillName.Text = "Skill Name"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(203, 6)
        '
        'mnuChangeLevel
        '
        Me.mnuChangeLevel.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuChangeLevel1, Me.mnuChangeLevel2, Me.mnuChangeLevel3, Me.mnuChangeLevel4, Me.mnuChangeLevel5})
        Me.mnuChangeLevel.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuChangeLevel.Name = "mnuChangeLevel"
        Me.mnuChangeLevel.Size = New System.Drawing.Size(206, 22)
        Me.mnuChangeLevel.Text = "Change To Level"
        '
        'mnuChangeLevel1
        '
        Me.mnuChangeLevel1.Name = "mnuChangeLevel1"
        Me.mnuChangeLevel1.Size = New System.Drawing.Size(108, 22)
        Me.mnuChangeLevel1.Text = "Level 1"
        '
        'mnuChangeLevel2
        '
        Me.mnuChangeLevel2.Name = "mnuChangeLevel2"
        Me.mnuChangeLevel2.Size = New System.Drawing.Size(108, 22)
        Me.mnuChangeLevel2.Text = "Level 2"
        '
        'mnuChangeLevel3
        '
        Me.mnuChangeLevel3.Name = "mnuChangeLevel3"
        Me.mnuChangeLevel3.Size = New System.Drawing.Size(108, 22)
        Me.mnuChangeLevel3.Text = "Level 3"
        '
        'mnuChangeLevel4
        '
        Me.mnuChangeLevel4.Name = "mnuChangeLevel4"
        Me.mnuChangeLevel4.Size = New System.Drawing.Size(108, 22)
        Me.mnuChangeLevel4.Text = "Level 4"
        '
        'mnuChangeLevel5
        '
        Me.mnuChangeLevel5.Name = "mnuChangeLevel5"
        Me.mnuChangeLevel5.Size = New System.Drawing.Size(108, 22)
        Me.mnuChangeLevel5.Text = "Level 5"
        '
        'mnuIncreaseLevel
        '
        Me.mnuIncreaseLevel.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuIncreaseLevel.Name = "mnuIncreaseLevel"
        Me.mnuIncreaseLevel.Size = New System.Drawing.Size(206, 22)
        Me.mnuIncreaseLevel.Text = "Increase Skill Level"
        '
        'mnuDecreaseLevel
        '
        Me.mnuDecreaseLevel.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuDecreaseLevel.Name = "mnuDecreaseLevel"
        Me.mnuDecreaseLevel.Size = New System.Drawing.Size(206, 22)
        Me.mnuDecreaseLevel.Text = "Decrease Skill Level"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(203, 6)
        '
        'mnuMoveUpQueue
        '
        Me.mnuMoveUpQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuMoveUpQueue.Name = "mnuMoveUpQueue"
        Me.mnuMoveUpQueue.Size = New System.Drawing.Size(206, 22)
        Me.mnuMoveUpQueue.Text = "Move Skill Up Queue"
        '
        'mnuMoveDownQueue
        '
        Me.mnuMoveDownQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuMoveDownQueue.Name = "mnuMoveDownQueue"
        Me.mnuMoveDownQueue.Size = New System.Drawing.Size(206, 22)
        Me.mnuMoveDownQueue.Text = "Move Skill Down Queue"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(203, 6)
        '
        'mnuSeparateLevels
        '
        Me.mnuSeparateLevels.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSeparateAllLevels, Me.mnuSeparateTopLevel, Me.mnuSeparateBottomLevel})
        Me.mnuSeparateLevels.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuSeparateLevels.Name = "mnuSeparateLevels"
        Me.mnuSeparateLevels.Size = New System.Drawing.Size(206, 22)
        Me.mnuSeparateLevels.Text = "Separate Levels"
        '
        'mnuSeparateAllLevels
        '
        Me.mnuSeparateAllLevels.Name = "mnuSeparateAllLevels"
        Me.mnuSeparateAllLevels.Size = New System.Drawing.Size(183, 22)
        Me.mnuSeparateAllLevels.Text = "Separate All Levels"
        '
        'mnuSeparateTopLevel
        '
        Me.mnuSeparateTopLevel.Name = "mnuSeparateTopLevel"
        Me.mnuSeparateTopLevel.Size = New System.Drawing.Size(183, 22)
        Me.mnuSeparateTopLevel.Text = "Separate Top Level"
        '
        'mnuSeparateBottomLevel
        '
        Me.mnuSeparateBottomLevel.Name = "mnuSeparateBottomLevel"
        Me.mnuSeparateBottomLevel.Size = New System.Drawing.Size(183, 22)
        Me.mnuSeparateBottomLevel.Text = "Separate Bottom Level"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(203, 6)
        '
        'mnuSplitQueue
        '
        Me.mnuSplitQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuSplitQueue.Name = "mnuSplitQueue"
        Me.mnuSplitQueue.Size = New System.Drawing.Size(206, 22)
        Me.mnuSplitQueue.Text = "Split Queue"
        '
        'mnuSeperateLevelSep
        '
        Me.mnuSeperateLevelSep.Name = "mnuSeperateLevelSep"
        Me.mnuSeperateLevelSep.Size = New System.Drawing.Size(203, 6)
        '
        'mnuDeleteFromQueue
        '
        Me.mnuDeleteFromQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuDeleteFromQueue.Name = "mnuDeleteFromQueue"
        Me.mnuDeleteFromQueue.Size = New System.Drawing.Size(206, 22)
        Me.mnuDeleteFromQueue.Text = "Delete from Training Queue"
        '
        'mnuRemoveTrainedSkills
        '
        Me.mnuRemoveTrainedSkills.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuRemoveTrainedSkills.Name = "mnuRemoveTrainedSkills"
        Me.mnuRemoveTrainedSkills.Size = New System.Drawing.Size(206, 22)
        Me.mnuRemoveTrainedSkills.Text = "Remove Trained Skills"
        '
        'mnuClearTrainingQueue
        '
        Me.mnuClearTrainingQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuClearTrainingQueue.Name = "mnuClearTrainingQueue"
        Me.mnuClearTrainingQueue.Size = New System.Drawing.Size(206, 22)
        Me.mnuClearTrainingQueue.Text = "Clear Training Queue"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(203, 6)
        '
        'mnuViewDetails
        '
        Me.mnuViewDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewDetails.Name = "mnuViewDetails"
        Me.mnuViewDetails.Size = New System.Drawing.Size(206, 22)
        Me.mnuViewDetails.Text = "View Details"
        '
        'mnuEditNote
        '
        Me.mnuEditNote.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuEditNote.Name = "mnuEditNote"
        Me.mnuEditNote.Size = New System.Drawing.Size(206, 22)
        Me.mnuEditNote.Text = "Edit Note"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(203, 6)
        '
        'mnuForceTraining
        '
        Me.mnuForceTraining.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuForceTraining.Name = "mnuForceTraining"
        Me.mnuForceTraining.Size = New System.Drawing.Size(206, 22)
        Me.mnuForceTraining.Text = "Force Skill Training"
        '
        'lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblFilter.Location = New System.Drawing.Point(13, 10)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(35, 13)
        Me.lblFilter.TabIndex = 13
        Me.lblFilter.Text = "Filter:"
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
        Me.lblDescription.Size = New System.Drawing.Size(429, 256)
        Me.lblDescription.TabIndex = 0
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
        Me.tvwReqs.Size = New System.Drawing.Size(429, 256)
        Me.tvwReqs.TabIndex = 0
        '
        'ctxReqs
        '
        Me.ctxReqs.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxReqs.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReqsSkillName, Me.ToolStripSeparator15, Me.mnuReqsViewDetailsHere, Me.mnuReqsViewDetails})
        Me.ctxReqs.Name = "ctxDepend"
        Me.ctxReqs.Size = New System.Drawing.Size(201, 76)
        '
        'mnuReqsSkillName
        '
        Me.mnuReqsSkillName.Name = "mnuReqsSkillName"
        Me.mnuReqsSkillName.Size = New System.Drawing.Size(200, 22)
        Me.mnuReqsSkillName.Text = "Skill Name"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(197, 6)
        '
        'mnuReqsViewDetailsHere
        '
        Me.mnuReqsViewDetailsHere.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuReqsViewDetailsHere.Name = "mnuReqsViewDetailsHere"
        Me.mnuReqsViewDetailsHere.Size = New System.Drawing.Size(200, 22)
        Me.mnuReqsViewDetailsHere.Text = "View Details Here"
        '
        'mnuReqsViewDetails
        '
        Me.mnuReqsViewDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuReqsViewDetails.Name = "mnuReqsViewDetails"
        Me.mnuReqsViewDetails.Size = New System.Drawing.Size(200, 22)
        Me.mnuReqsViewDetails.Text = "View Details In Skill Screen"
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
        Me.lvwDepend.Size = New System.Drawing.Size(429, 256)
        Me.lvwDepend.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwDepend.TabIndex = 0
        Me.lvwDepend.UseCompatibleStateImageBehavior = False
        Me.lvwDepend.View = System.Windows.Forms.View.Details
        '
        'NeededFor
        '
        Me.NeededFor.Text = "Required For"
        Me.NeededFor.Width = 325
        '
        'NeededLevel
        '
        Me.NeededLevel.Text = "Level"
        Me.NeededLevel.Width = 75
        '
        'ctxDepend
        '
        Me.ctxDepend.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxDepend.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuItemName, Me.ToolStripSeparator4, Me.mnuViewItemDetailsHere, Me.mnuViewItemDetails, Me.mnuViewItemDetailsInIB, Me.mnuViewItemDetailsInCertScreen})
        Me.ctxDepend.Name = "ctxDepend"
        Me.ctxDepend.Size = New System.Drawing.Size(212, 120)
        '
        'mnuItemName
        '
        Me.mnuItemName.Name = "mnuItemName"
        Me.mnuItemName.Size = New System.Drawing.Size(211, 22)
        Me.mnuItemName.Text = "Item Name"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(208, 6)
        '
        'mnuViewItemDetailsHere
        '
        Me.mnuViewItemDetailsHere.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewItemDetailsHere.Name = "mnuViewItemDetailsHere"
        Me.mnuViewItemDetailsHere.Size = New System.Drawing.Size(211, 22)
        Me.mnuViewItemDetailsHere.Text = "View Details Here"
        '
        'mnuViewItemDetails
        '
        Me.mnuViewItemDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewItemDetails.Name = "mnuViewItemDetails"
        Me.mnuViewItemDetails.Size = New System.Drawing.Size(211, 22)
        Me.mnuViewItemDetails.Text = "View Details In Skill Screen"
        '
        'mnuViewItemDetailsInIB
        '
        Me.mnuViewItemDetailsInIB.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewItemDetailsInIB.Name = "mnuViewItemDetailsInIB"
        Me.mnuViewItemDetailsInIB.Size = New System.Drawing.Size(211, 22)
        Me.mnuViewItemDetailsInIB.Text = "View Details In Item Browser"
        '
        'mnuViewItemDetailsInCertScreen
        '
        Me.mnuViewItemDetailsInCertScreen.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewItemDetailsInCertScreen.Name = "mnuViewItemDetailsInCertScreen"
        Me.mnuViewItemDetailsInCertScreen.Size = New System.Drawing.Size(211, 22)
        Me.mnuViewItemDetailsInCertScreen.Text = "View Details In Cert Screen"
        '
        'lvwSPs
        '
        Me.lvwSPs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.lvwSPs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwSPs.FullRowSelect = True
        Me.lvwSPs.GridLines = True
        Me.lvwSPs.Location = New System.Drawing.Point(1, 1)
        Me.lvwSPs.Name = "lvwSPs"
        Me.lvwSPs.Size = New System.Drawing.Size(429, 256)
        Me.lvwSPs.TabIndex = 2
        Me.lvwSPs.UseCompatibleStateImageBehavior = False
        Me.lvwSPs.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Lvl"
        Me.ColumnHeader3.Width = 30
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "SPs for Lvl"
        Me.ColumnHeader4.Width = 75
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Diff from Lvl"
        Me.ColumnHeader5.Width = 75
        '
        'lblTotalQueueTime
        '
        Me.lblTotalQueueTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalQueueTime.AutoSize = True
        Me.lblTotalQueueTime.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalQueueTime.Location = New System.Drawing.Point(6, 785)
        Me.lblTotalQueueTime.Name = "lblTotalQueueTime"
        Me.lblTotalQueueTime.Size = New System.Drawing.Size(95, 13)
        Me.lblTotalQueueTime.TabIndex = 9
        Me.lblTotalQueueTime.Text = "Total Queue Time:"
        '
        'chkOmitQueuesSkills
        '
        Me.chkOmitQueuesSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkOmitQueuesSkills.AutoSize = True
        Me.chkOmitQueuesSkills.BackColor = System.Drawing.Color.Transparent
        Me.chkOmitQueuesSkills.Location = New System.Drawing.Point(4, 458)
        Me.chkOmitQueuesSkills.Name = "chkOmitQueuesSkills"
        Me.chkOmitQueuesSkills.Size = New System.Drawing.Size(114, 17)
        Me.chkOmitQueuesSkills.TabIndex = 18
        Me.chkOmitQueuesSkills.Text = "Omit Queued Skills"
        Me.chkOmitQueuesSkills.UseVisualStyleBackColor = False
        '
        'cboCertFilter
        '
        Me.cboCertFilter.DropDownHeight = 160
        Me.cboCertFilter.FormattingEnabled = True
        Me.cboCertFilter.IntegralHeight = False
        Me.cboCertFilter.Items.AddRange(New Object() {"All", "Owned Certificates", "Missing Certificates", "Basic Certificates - All", "Standard Certificates - All", "Improved Certificates - All", "Advanced Certificates - All", "Elite Certificates - All", "Basic Certificates - Missing", "Standard Certificates - Missing", "Improved Certificates - Missing", "Advanced Certificates - Missing", "Elite Certificates - Missing"})
        Me.cboCertFilter.Location = New System.Drawing.Point(51, 8)
        Me.cboCertFilter.Name = "cboCertFilter"
        Me.cboCertFilter.Size = New System.Drawing.Size(192, 21)
        Me.cboCertFilter.TabIndex = 17
        '
        'lblCertFilter
        '
        Me.lblCertFilter.AutoSize = True
        Me.lblCertFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblCertFilter.Location = New System.Drawing.Point(10, 11)
        Me.lblCertFilter.Name = "lblCertFilter"
        Me.lblCertFilter.Size = New System.Drawing.Size(35, 13)
        Me.lblCertFilter.TabIndex = 16
        Me.lblCertFilter.Text = "Filter:"
        '
        'tvwCertList
        '
        Me.tvwCertList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwCertList.ContextMenuStrip = Me.ctxCertDetails
        Me.tvwCertList.FullRowSelect = True
        Me.tvwCertList.HideSelection = False
        Me.tvwCertList.ImageKey = "Blank.jpg"
        Me.tvwCertList.ImageList = Me.ImageList1
        Me.tvwCertList.Indent = 20
        Me.tvwCertList.Location = New System.Drawing.Point(4, 35)
        Me.tvwCertList.Name = "tvwCertList"
        Me.tvwCertList.SelectedImageIndex = 6
        Me.tvwCertList.Size = New System.Drawing.Size(422, 440)
        Me.tvwCertList.TabIndex = 15
        '
        'ctxCertDetails
        '
        Me.ctxCertDetails.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxCertDetails.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCertName, Me.ToolStripSeparator16, Me.mnuAddCertToQueue, Me.mnuAddCertGroupToQueue, Me.ToolStripSeparator18, Me.mnuViewCertDetails})
        Me.ctxCertDetails.Name = "ctxDepend"
        Me.ctxCertDetails.Size = New System.Drawing.Size(217, 104)
        '
        'mnuCertName
        '
        Me.mnuCertName.Name = "mnuCertName"
        Me.mnuCertName.Size = New System.Drawing.Size(216, 22)
        Me.mnuCertName.Text = "Skill Name"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(213, 6)
        '
        'mnuAddCertToQueue
        '
        Me.mnuAddCertToQueue.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddCertToQueueNext, Me.ToolStripSeparator17, Me.mnuAddCertToQueue1, Me.mnuAddCertToQueue2, Me.mnuAddCertToQueue3, Me.mnuAddCertToQueue4, Me.mnuAddCertToQueue5})
        Me.mnuAddCertToQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuAddCertToQueue.Name = "mnuAddCertToQueue"
        Me.mnuAddCertToQueue.Size = New System.Drawing.Size(216, 22)
        Me.mnuAddCertToQueue.Text = "Add to Training Queue"
        '
        'mnuAddCertToQueueNext
        '
        Me.mnuAddCertToQueueNext.Enabled = False
        Me.mnuAddCertToQueueNext.Name = "mnuAddCertToQueueNext"
        Me.mnuAddCertToQueueNext.Size = New System.Drawing.Size(129, 22)
        Me.mnuAddCertToQueueNext.Text = "Next Grade"
        Me.mnuAddCertToQueueNext.Visible = False
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(126, 6)
        Me.ToolStripSeparator17.Visible = False
        '
        'mnuAddCertToQueue1
        '
        Me.mnuAddCertToQueue1.Enabled = False
        Me.mnuAddCertToQueue1.Name = "mnuAddCertToQueue1"
        Me.mnuAddCertToQueue1.Size = New System.Drawing.Size(129, 22)
        Me.mnuAddCertToQueue1.Tag = "1"
        Me.mnuAddCertToQueue1.Text = "Basic"
        '
        'mnuAddCertToQueue2
        '
        Me.mnuAddCertToQueue2.Enabled = False
        Me.mnuAddCertToQueue2.Name = "mnuAddCertToQueue2"
        Me.mnuAddCertToQueue2.Size = New System.Drawing.Size(129, 22)
        Me.mnuAddCertToQueue2.Tag = "2"
        Me.mnuAddCertToQueue2.Text = "Standard"
        '
        'mnuAddCertToQueue3
        '
        Me.mnuAddCertToQueue3.Enabled = False
        Me.mnuAddCertToQueue3.Name = "mnuAddCertToQueue3"
        Me.mnuAddCertToQueue3.Size = New System.Drawing.Size(129, 22)
        Me.mnuAddCertToQueue3.Tag = "3"
        Me.mnuAddCertToQueue3.Text = "Improved"
        '
        'mnuAddCertToQueue4
        '
        Me.mnuAddCertToQueue4.Enabled = False
        Me.mnuAddCertToQueue4.Name = "mnuAddCertToQueue4"
        Me.mnuAddCertToQueue4.Size = New System.Drawing.Size(129, 22)
        Me.mnuAddCertToQueue4.Tag = "4"
        Me.mnuAddCertToQueue4.Text = "Advanced"
        '
        'mnuAddCertToQueue5
        '
        Me.mnuAddCertToQueue5.Enabled = False
        Me.mnuAddCertToQueue5.Name = "mnuAddCertToQueue5"
        Me.mnuAddCertToQueue5.Size = New System.Drawing.Size(129, 22)
        Me.mnuAddCertToQueue5.Tag = "5"
        Me.mnuAddCertToQueue5.Text = "Elite"
        '
        'mnuAddCertGroupToQueue
        '
        Me.mnuAddCertGroupToQueue.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddCertGroupToQueue1, Me.mnuAddCertGroupToQueue2, Me.mnuAddCertGroupToQueue3, Me.mnuAddCertGroupToQueue4, Me.mnuAddCertGroupToQueue5})
        Me.mnuAddCertGroupToQueue.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuAddCertGroupToQueue.Name = "mnuAddCertGroupToQueue"
        Me.mnuAddCertGroupToQueue.Size = New System.Drawing.Size(216, 22)
        Me.mnuAddCertGroupToQueue.Text = "Add Group To Training Queue"
        '
        'mnuAddCertGroupToQueue1
        '
        Me.mnuAddCertGroupToQueue1.Name = "mnuAddCertGroupToQueue1"
        Me.mnuAddCertGroupToQueue1.Size = New System.Drawing.Size(137, 22)
        Me.mnuAddCertGroupToQueue1.Tag = "1"
        Me.mnuAddCertGroupToQueue1.Text = "To Basic"
        '
        'mnuAddCertGroupToQueue2
        '
        Me.mnuAddCertGroupToQueue2.Name = "mnuAddCertGroupToQueue2"
        Me.mnuAddCertGroupToQueue2.Size = New System.Drawing.Size(137, 22)
        Me.mnuAddCertGroupToQueue2.Tag = "2"
        Me.mnuAddCertGroupToQueue2.Text = "To Standard"
        '
        'mnuAddCertGroupToQueue3
        '
        Me.mnuAddCertGroupToQueue3.Name = "mnuAddCertGroupToQueue3"
        Me.mnuAddCertGroupToQueue3.Size = New System.Drawing.Size(137, 22)
        Me.mnuAddCertGroupToQueue3.Tag = "3"
        Me.mnuAddCertGroupToQueue3.Text = "To Improved"
        '
        'mnuAddCertGroupToQueue4
        '
        Me.mnuAddCertGroupToQueue4.Enabled = False
        Me.mnuAddCertGroupToQueue4.Name = "mnuAddCertGroupToQueue4"
        Me.mnuAddCertGroupToQueue4.Size = New System.Drawing.Size(137, 22)
        Me.mnuAddCertGroupToQueue4.Tag = "4"
        Me.mnuAddCertGroupToQueue4.Text = "To Advanced"
        '
        'mnuAddCertGroupToQueue5
        '
        Me.mnuAddCertGroupToQueue5.Name = "mnuAddCertGroupToQueue5"
        Me.mnuAddCertGroupToQueue5.Size = New System.Drawing.Size(137, 22)
        Me.mnuAddCertGroupToQueue5.Tag = "5"
        Me.mnuAddCertGroupToQueue5.Text = "To Elite"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(213, 6)
        '
        'mnuViewCertDetails
        '
        Me.mnuViewCertDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewCertDetails.Name = "mnuViewCertDetails"
        Me.mnuViewCertDetails.Size = New System.Drawing.Size(216, 22)
        Me.mnuViewCertDetails.Text = "View Details"
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(12, 9)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 42
        Me.lblPilot.Text = "Pilot:"
        '
        'RibbonBarMergeContainer1
        '
        Me.RibbonBarMergeContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbImportExport)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbAttributeTools)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbQueueSettings)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbQueueFunctions)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbQueueAdmin)
        Me.RibbonBarMergeContainer1.Location = New System.Drawing.Point(480, 3)
        Me.RibbonBarMergeContainer1.Name = "RibbonBarMergeContainer1"
        Me.RibbonBarMergeContainer1.RibbonTabText = "Skill Training Planner"
        Me.RibbonBarMergeContainer1.Size = New System.Drawing.Size(1021, 90)
        '
        '
        '
        Me.RibbonBarMergeContainer1.Style.Class = ""
        Me.RibbonBarMergeContainer1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.RibbonBarMergeContainer1.StyleMouseDown.Class = ""
        Me.RibbonBarMergeContainer1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.RibbonBarMergeContainer1.StyleMouseOver.Class = ""
        Me.RibbonBarMergeContainer1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.RibbonBarMergeContainer1.TabIndex = 47
        Me.RibbonBarMergeContainer1.Visible = False
        '
        'rbImportExport
        '
        Me.rbImportExport.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbImportExport.BackgroundMouseOverStyle.Class = ""
        Me.rbImportExport.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbImportExport.BackgroundStyle.Class = ""
        Me.rbImportExport.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbImportExport.ContainerControlProcessDialogKey = True
        Me.rbImportExport.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ItemContainer1})
        Me.rbImportExport.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbImportExport.Location = New System.Drawing.Point(934, 0)
        Me.rbImportExport.Name = "rbImportExport"
        Me.rbImportExport.Size = New System.Drawing.Size(134, 90)
        Me.rbImportExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.rbImportExport.TabIndex = 0
        Me.rbImportExport.Text = "Import/Export"
        '
        '
        '
        Me.rbImportExport.TitleStyle.Class = ""
        Me.rbImportExport.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbImportExport.TitleStyleMouseOver.Class = ""
        Me.rbImportExport.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'ItemContainer1
        '
        '
        '
        '
        Me.ItemContainer1.BackgroundStyle.Class = ""
        Me.ItemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemContainer1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.ItemContainer1.Name = "ItemContainer1"
        Me.ItemContainer1.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnEveMonImport, Me.btnImportEMPFile, Me.btnExportEMPFile})
        Me.ItemContainer1.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Middle
        '
        'btnEveMonImport
        '
        Me.btnEveMonImport.Name = "btnEveMonImport"
        Me.btnEveMonImport.Text = "EveMon Import (Full)"
        '
        'btnImportEMPFile
        '
        Me.btnImportEMPFile.Name = "btnImportEMPFile"
        Me.btnImportEMPFile.Text = "Import EveMon Plan File"
        '
        'btnExportEMPFile
        '
        Me.btnExportEMPFile.Enabled = False
        Me.btnExportEMPFile.Name = "btnExportEMPFile"
        Me.btnExportEMPFile.Text = "Export EveMon Plan File"
        '
        'rbAttributeTools
        '
        Me.rbAttributeTools.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbAttributeTools.BackgroundMouseOverStyle.Class = ""
        Me.rbAttributeTools.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAttributeTools.BackgroundStyle.Class = ""
        Me.rbAttributeTools.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbAttributeTools.ContainerControlProcessDialogKey = True
        Me.rbAttributeTools.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnImplants, Me.btnRemap})
        Me.rbAttributeTools.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbAttributeTools.Location = New System.Drawing.Point(58, 0)
        Me.rbAttributeTools.Name = "rbAttributeTools"
        Me.rbAttributeTools.Size = New System.Drawing.Size(99, 90)
        Me.rbAttributeTools.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.rbAttributeTools.TabIndex = 1
        Me.rbAttributeTools.Text = "Attribute Tools"
        '
        '
        '
        Me.rbAttributeTools.TitleStyle.Class = ""
        Me.rbAttributeTools.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAttributeTools.TitleStyleMouseOver.Class = ""
        Me.rbAttributeTools.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbAttributeTools.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Middle
        '
        'btnImplants
        '
        Me.btnImplants.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnImplants.Enabled = False
        Me.btnImplants.Image = Global.EveHQ.My.Resources.Resources.Implants32
        Me.btnImplants.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnImplants.Name = "btnImplants"
        Me.btnImplants.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnImplants, New DevComponents.DotNetBar.SuperTooltipInfo("Implant Analysis", "", resources.GetString("btnImplants.SuperTooltip"), Global.EveHQ.My.Resources.Resources.ImplantFormSmall, Nothing, DevComponents.DotNetBar.eTooltipColor.Lemon))
        Me.btnImplants.Text = "Implant Analysis"
        '
        'btnRemap
        '
        Me.btnRemap.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRemap.Enabled = False
        Me.btnRemap.Image = Global.EveHQ.My.Resources.Resources.Attributes32
        Me.btnRemap.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRemap.Name = "btnRemap"
        Me.btnRemap.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnRemap, New DevComponents.DotNetBar.SuperTooltipInfo("Neural Remap", "", resources.GetString("btnRemap.SuperTooltip"), Global.EveHQ.My.Resources.Resources.NeuralRemapFormSmall, Nothing, DevComponents.DotNetBar.eTooltipColor.Lemon))
        Me.btnRemap.Text = "Neural Remap"
        '
        'rbQueueSettings
        '
        Me.rbQueueSettings.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbQueueSettings.BackgroundMouseOverStyle.Class = ""
        Me.rbQueueSettings.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbQueueSettings.BackgroundStyle.Class = ""
        Me.rbQueueSettings.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbQueueSettings.ContainerControlProcessDialogKey = True
        Me.rbQueueSettings.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnQueueSettings})
        Me.rbQueueSettings.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbQueueSettings.Location = New System.Drawing.Point(0, 0)
        Me.rbQueueSettings.Name = "rbQueueSettings"
        Me.rbQueueSettings.Size = New System.Drawing.Size(56, 90)
        Me.rbQueueSettings.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.rbQueueSettings.TabIndex = 5
        Me.rbQueueSettings.Text = "Options"
        '
        '
        '
        Me.rbQueueSettings.TitleStyle.Class = ""
        Me.rbQueueSettings.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbQueueSettings.TitleStyleMouseOver.Class = ""
        Me.rbQueueSettings.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbQueueSettings.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Middle
        '
        'btnQueueSettings
        '
        Me.btnQueueSettings.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnQueueSettings.Image = CType(resources.GetObject("btnQueueSettings.Image"), System.Drawing.Image)
        Me.btnQueueSettings.ImageFixedSize = New System.Drawing.Size(36, 36)
        Me.btnQueueSettings.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnQueueSettings.Name = "btnQueueSettings"
        Me.btnQueueSettings.SubItemsExpandWidth = 14
        Me.btnQueueSettings.Text = "Queue Settings"
        '
        'rbQueueFunctions
        '
        Me.rbQueueFunctions.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbQueueFunctions.BackgroundMouseOverStyle.Class = ""
        Me.rbQueueFunctions.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbQueueFunctions.BackgroundStyle.Class = ""
        Me.rbQueueFunctions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbQueueFunctions.ContainerControlProcessDialogKey = True
        Me.rbQueueFunctions.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnRBAddSkill, Me.btnRBDeleteSkill, Me.btnRBIncreaseLevel, Me.btnRBDecreaseLevel, Me.btnRBMoveUpQueue, Me.btnRBMoveDownQueue, Me.btnRBClearQueue, Me.btnRBSplitQueue})
        Me.rbQueueFunctions.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbQueueFunctions.Location = New System.Drawing.Point(543, 0)
        Me.rbQueueFunctions.Name = "rbQueueFunctions"
        Me.rbQueueFunctions.Size = New System.Drawing.Size(389, 90)
        Me.rbQueueFunctions.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.rbQueueFunctions.TabIndex = 4
        Me.rbQueueFunctions.Text = "Queue Functions"
        '
        '
        '
        Me.rbQueueFunctions.TitleStyle.Class = ""
        Me.rbQueueFunctions.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbQueueFunctions.TitleStyleMouseOver.Class = ""
        Me.rbQueueFunctions.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbQueueFunctions.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Middle
        '
        'btnRBAddSkill
        '
        Me.btnRBAddSkill.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBAddSkill.Enabled = False
        Me.btnRBAddSkill.Image = CType(resources.GetObject("btnRBAddSkill.Image"), System.Drawing.Image)
        Me.btnRBAddSkill.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBAddSkill.Name = "btnRBAddSkill"
        Me.btnRBAddSkill.SubItemsExpandWidth = 14
        Me.btnRBAddSkill.Text = "Add Skill"
        '
        'btnRBDeleteSkill
        '
        Me.btnRBDeleteSkill.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBDeleteSkill.Enabled = False
        Me.btnRBDeleteSkill.Image = CType(resources.GetObject("btnRBDeleteSkill.Image"), System.Drawing.Image)
        Me.btnRBDeleteSkill.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBDeleteSkill.Name = "btnRBDeleteSkill"
        Me.btnRBDeleteSkill.SubItemsExpandWidth = 14
        Me.btnRBDeleteSkill.Text = "Delete Skill"
        '
        'btnRBIncreaseLevel
        '
        Me.btnRBIncreaseLevel.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBIncreaseLevel.Enabled = False
        Me.btnRBIncreaseLevel.Image = CType(resources.GetObject("btnRBIncreaseLevel.Image"), System.Drawing.Image)
        Me.btnRBIncreaseLevel.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBIncreaseLevel.Name = "btnRBIncreaseLevel"
        Me.btnRBIncreaseLevel.SubItemsExpandWidth = 14
        Me.btnRBIncreaseLevel.Text = "Increase Level"
        '
        'btnRBDecreaseLevel
        '
        Me.btnRBDecreaseLevel.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBDecreaseLevel.Enabled = False
        Me.btnRBDecreaseLevel.Image = CType(resources.GetObject("btnRBDecreaseLevel.Image"), System.Drawing.Image)
        Me.btnRBDecreaseLevel.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBDecreaseLevel.Name = "btnRBDecreaseLevel"
        Me.btnRBDecreaseLevel.SubItemsExpandWidth = 14
        Me.btnRBDecreaseLevel.Text = "Decrease Level"
        '
        'btnRBMoveUpQueue
        '
        Me.btnRBMoveUpQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBMoveUpQueue.Enabled = False
        Me.btnRBMoveUpQueue.Image = CType(resources.GetObject("btnRBMoveUpQueue.Image"), System.Drawing.Image)
        Me.btnRBMoveUpQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBMoveUpQueue.Name = "btnRBMoveUpQueue"
        Me.btnRBMoveUpQueue.SubItemsExpandWidth = 14
        Me.btnRBMoveUpQueue.Text = "Move Up Queue"
        '
        'btnRBMoveDownQueue
        '
        Me.btnRBMoveDownQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBMoveDownQueue.Enabled = False
        Me.btnRBMoveDownQueue.Image = CType(resources.GetObject("btnRBMoveDownQueue.Image"), System.Drawing.Image)
        Me.btnRBMoveDownQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBMoveDownQueue.Name = "btnRBMoveDownQueue"
        Me.btnRBMoveDownQueue.SubItemsExpandWidth = 14
        Me.btnRBMoveDownQueue.Text = "Move Down Queue"
        '
        'btnRBClearQueue
        '
        Me.btnRBClearQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBClearQueue.Enabled = False
        Me.btnRBClearQueue.Image = Global.EveHQ.My.Resources.Resources.SkillBook32
        Me.btnRBClearQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBClearQueue.Name = "btnRBClearQueue"
        Me.btnRBClearQueue.SubItemsExpandWidth = 14
        Me.btnRBClearQueue.Text = "Clear Queue"
        '
        'btnRBSplitQueue
        '
        Me.btnRBSplitQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBSplitQueue.Enabled = False
        Me.btnRBSplitQueue.Image = Global.EveHQ.My.Resources.Resources.SkillBook32
        Me.btnRBSplitQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBSplitQueue.Name = "btnRBSplitQueue"
        Me.btnRBSplitQueue.SubItemsExpandWidth = 14
        Me.btnRBSplitQueue.Text = "Split Queue"
        '
        'rbQueueAdmin
        '
        Me.rbQueueAdmin.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbQueueAdmin.BackgroundMouseOverStyle.Class = ""
        Me.rbQueueAdmin.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbQueueAdmin.BackgroundStyle.Class = ""
        Me.rbQueueAdmin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbQueueAdmin.ContainerControlProcessDialogKey = True
        Me.rbQueueAdmin.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnRBAddQueue, Me.btnRBEditQueue, Me.btnRBDeleteQueue, Me.btnRBSetPrimaryQueue, Me.btnRBMergeQueues, Me.btnRBCopyQueue, Me.btnRBCopyQueueToPilot, Me.btnIncTraining})
        Me.rbQueueAdmin.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbQueueAdmin.Location = New System.Drawing.Point(159, 0)
        Me.rbQueueAdmin.Name = "rbQueueAdmin"
        Me.rbQueueAdmin.Size = New System.Drawing.Size(382, 90)
        Me.rbQueueAdmin.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.rbQueueAdmin.TabIndex = 2
        Me.rbQueueAdmin.Text = "Queue Admin"
        '
        '
        '
        Me.rbQueueAdmin.TitleStyle.Class = ""
        Me.rbQueueAdmin.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbQueueAdmin.TitleStyleMouseOver.Class = ""
        Me.rbQueueAdmin.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbQueueAdmin.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Middle
        '
        'btnRBAddQueue
        '
        Me.btnRBAddQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBAddQueue.Image = CType(resources.GetObject("btnRBAddQueue.Image"), System.Drawing.Image)
        Me.btnRBAddQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBAddQueue.Name = "btnRBAddQueue"
        Me.btnRBAddQueue.SubItemsExpandWidth = 14
        Me.btnRBAddQueue.Text = "Add Queue"
        '
        'btnRBEditQueue
        '
        Me.btnRBEditQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBEditQueue.Enabled = False
        Me.btnRBEditQueue.Image = CType(resources.GetObject("btnRBEditQueue.Image"), System.Drawing.Image)
        Me.btnRBEditQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBEditQueue.Name = "btnRBEditQueue"
        Me.btnRBEditQueue.SubItemsExpandWidth = 14
        Me.btnRBEditQueue.Text = "Edit Queue"
        '
        'btnRBDeleteQueue
        '
        Me.btnRBDeleteQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBDeleteQueue.Enabled = False
        Me.btnRBDeleteQueue.Image = CType(resources.GetObject("btnRBDeleteQueue.Image"), System.Drawing.Image)
        Me.btnRBDeleteQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBDeleteQueue.Name = "btnRBDeleteQueue"
        Me.btnRBDeleteQueue.SubItemsExpandWidth = 14
        Me.btnRBDeleteQueue.Text = "Delete Queue"
        '
        'btnRBSetPrimaryQueue
        '
        Me.btnRBSetPrimaryQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBSetPrimaryQueue.Enabled = False
        Me.btnRBSetPrimaryQueue.Image = CType(resources.GetObject("btnRBSetPrimaryQueue.Image"), System.Drawing.Image)
        Me.btnRBSetPrimaryQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBSetPrimaryQueue.Name = "btnRBSetPrimaryQueue"
        Me.btnRBSetPrimaryQueue.SubItemsExpandWidth = 14
        Me.btnRBSetPrimaryQueue.Text = "Set As Primary"
        '
        'btnRBMergeQueues
        '
        Me.btnRBMergeQueues.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBMergeQueues.Enabled = False
        Me.btnRBMergeQueues.Image = CType(resources.GetObject("btnRBMergeQueues.Image"), System.Drawing.Image)
        Me.btnRBMergeQueues.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBMergeQueues.Name = "btnRBMergeQueues"
        Me.btnRBMergeQueues.SubItemsExpandWidth = 14
        Me.btnRBMergeQueues.Text = "Merge Queues"
        '
        'btnRBCopyQueue
        '
        Me.btnRBCopyQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBCopyQueue.Enabled = False
        Me.btnRBCopyQueue.Image = CType(resources.GetObject("btnRBCopyQueue.Image"), System.Drawing.Image)
        Me.btnRBCopyQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBCopyQueue.Name = "btnRBCopyQueue"
        Me.btnRBCopyQueue.SubItemsExpandWidth = 14
        Me.btnRBCopyQueue.Text = "Copy Queue"
        '
        'btnRBCopyQueueToPilot
        '
        Me.btnRBCopyQueueToPilot.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRBCopyQueueToPilot.Enabled = False
        Me.btnRBCopyQueueToPilot.Image = CType(resources.GetObject("btnRBCopyQueueToPilot.Image"), System.Drawing.Image)
        Me.btnRBCopyQueueToPilot.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRBCopyQueueToPilot.Name = "btnRBCopyQueueToPilot"
        Me.btnRBCopyQueueToPilot.SubItemsExpandWidth = 14
        Me.btnRBCopyQueueToPilot.Text = "Copy Queue to Pilot"
        '
        'btnIncTraining
        '
        Me.btnIncTraining.AutoCheckOnClick = True
        Me.btnIncTraining.Enabled = False
        Me.btnIncTraining.Image = Global.EveHQ.My.Resources.Resources.SkillBook32
        Me.btnIncTraining.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnIncTraining.Name = "btnIncTraining"
        Me.btnIncTraining.SubItemsExpandWidth = 14
        Me.btnIncTraining.Text = "Include Training"
        '
        'SuperTooltip1
        '
        Me.SuperTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        '
        'tabQueueMode
        '
        Me.tabQueueMode.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabQueueMode.BackColor = System.Drawing.Color.Transparent
        Me.tabQueueMode.CanReorderTabs = True
        Me.tabQueueMode.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tabQueueMode.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tabQueueMode.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tabQueueMode.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tabQueueMode.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tabQueueMode.Controls.Add(Me.tcpSkillPlanning)
        Me.tabQueueMode.Controls.Add(Me.tcpCertPlanning)
        Me.tabQueueMode.Location = New System.Drawing.Point(3, 33)
        Me.tabQueueMode.Name = "tabQueueMode"
        Me.tabQueueMode.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tabQueueMode.SelectedTabIndex = 0
        Me.tabQueueMode.Size = New System.Drawing.Size(430, 502)
        Me.tabQueueMode.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tabQueueMode.TabIndex = 48
        Me.tabQueueMode.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabQueueMode.Tabs.Add(Me.tiSkillPlanning)
        Me.tabQueueMode.Tabs.Add(Me.tiCertPLanning)
        Me.tabQueueMode.Text = "TabControl1"
        '
        'tcpSkillPlanning
        '
        Me.tcpSkillPlanning.Controls.Add(Me.cboFilter)
        Me.tcpSkillPlanning.Controls.Add(Me.btnCollapseAll)
        Me.tcpSkillPlanning.Controls.Add(Me.btnExpandAll)
        Me.tcpSkillPlanning.Controls.Add(Me.lblFilter)
        Me.tcpSkillPlanning.Controls.Add(Me.chkOmitQueuesSkills)
        Me.tcpSkillPlanning.Controls.Add(Me.tvwSkillList)
        Me.tcpSkillPlanning.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpSkillPlanning.Location = New System.Drawing.Point(0, 23)
        Me.tcpSkillPlanning.Name = "tcpSkillPlanning"
        Me.tcpSkillPlanning.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpSkillPlanning.Size = New System.Drawing.Size(430, 479)
        Me.tcpSkillPlanning.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpSkillPlanning.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpSkillPlanning.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpSkillPlanning.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpSkillPlanning.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpSkillPlanning.Style.GradientAngle = 90
        Me.tcpSkillPlanning.TabIndex = 1
        Me.tcpSkillPlanning.TabItem = Me.tiSkillPlanning
        '
        'cboFilter
        '
        Me.cboFilter.DisplayMember = "Text"
        Me.cboFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboFilter.FormattingEnabled = True
        Me.cboFilter.ItemHeight = 15
        Me.cboFilter.Items.AddRange(New Object() {"Published Only", "All", "Non-Published Only", "Owned Skills", "Missing Skills", "New Skills Ready To Train", "Skills Ready To Train to Next Level", "Partially Trained", "Skills at Level 0", "Skills at Level 1", "Skills at Level 2", "Skills at Level 3", "Skills at Level 4", "Rank 1 Skills", "Rank 2 Skills", "Rank 3 Skills", "Rank 4 Skills", "Rank 5 Skills", "Rank 6 Skills", "Rank 7 Skills", "Rank 8 Skills", "Rank 9 Skills", "Rank 10 Skills", "Rank 11 Skills", "Rank 12 Skills", "Rank 13 Skills", "Rank 14 Skills", "Rank 15 Skills", "Rank 16 Skills", "Primary - Charisma", "Primary - Intelligence", "Primary - Memory", "Primary - Perception", "Primary - Willpower", "Secondary - Charisma", "Secondary - Intelligence", "Secondary - Memory", "Secondary - Perception", "Secondary - Willpower"})
        Me.cboFilter.Location = New System.Drawing.Point(51, 7)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.Size = New System.Drawing.Size(209, 21)
        Me.cboFilter.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboFilter.TabIndex = 21
        '
        'btnCollapseAll
        '
        Me.btnCollapseAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCollapseAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCollapseAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCollapseAll.Location = New System.Drawing.Point(195, 455)
        Me.btnCollapseAll.Name = "btnCollapseAll"
        Me.btnCollapseAll.Size = New System.Drawing.Size(65, 20)
        Me.btnCollapseAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCollapseAll.TabIndex = 20
        Me.btnCollapseAll.Text = "Collapse All"
        '
        'btnExpandAll
        '
        Me.btnExpandAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnExpandAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExpandAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnExpandAll.Location = New System.Drawing.Point(124, 455)
        Me.btnExpandAll.Name = "btnExpandAll"
        Me.btnExpandAll.Size = New System.Drawing.Size(65, 20)
        Me.btnExpandAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnExpandAll.TabIndex = 19
        Me.btnExpandAll.Text = "Expand All"
        '
        'tiSkillPlanning
        '
        Me.tiSkillPlanning.AttachedControl = Me.tcpSkillPlanning
        Me.tiSkillPlanning.Name = "tiSkillPlanning"
        Me.tiSkillPlanning.Text = "Skill Planning"
        '
        'tcpCertPlanning
        '
        Me.tcpCertPlanning.Controls.Add(Me.cboCertFilter)
        Me.tcpCertPlanning.Controls.Add(Me.lblCertFilter)
        Me.tcpCertPlanning.Controls.Add(Me.tvwCertList)
        Me.tcpCertPlanning.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpCertPlanning.Location = New System.Drawing.Point(0, 23)
        Me.tcpCertPlanning.Name = "tcpCertPlanning"
        Me.tcpCertPlanning.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpCertPlanning.Size = New System.Drawing.Size(430, 479)
        Me.tcpCertPlanning.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpCertPlanning.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpCertPlanning.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpCertPlanning.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpCertPlanning.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpCertPlanning.Style.GradientAngle = 90
        Me.tcpCertPlanning.TabIndex = 2
        Me.tcpCertPlanning.TabItem = Me.tiCertPLanning
        '
        'tiCertPLanning
        '
        Me.tiCertPLanning.AttachedControl = Me.tcpCertPlanning
        Me.tiCertPLanning.Name = "tiCertPLanning"
        Me.tiCertPLanning.Text = "CertificatePlanning"
        '
        'tabSkillDetails
        '
        Me.tabSkillDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabSkillDetails.BackColor = System.Drawing.Color.Transparent
        Me.tabSkillDetails.CanReorderTabs = True
        Me.tabSkillDetails.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tabSkillDetails.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tabSkillDetails.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tabSkillDetails.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tabSkillDetails.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tabSkillDetails.Controls.Add(Me.TabControlPanel1)
        Me.tabSkillDetails.Controls.Add(Me.TabControlPanel7)
        Me.tabSkillDetails.Controls.Add(Me.TabControlPanel5)
        Me.tabSkillDetails.Controls.Add(Me.TabControlPanel4)
        Me.tabSkillDetails.Controls.Add(Me.TabControlPanel3)
        Me.tabSkillDetails.Controls.Add(Me.TabControlPanel2)
        Me.tabSkillDetails.Location = New System.Drawing.Point(3, 541)
        Me.tabSkillDetails.Name = "tabSkillDetails"
        Me.tabSkillDetails.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tabSkillDetails.SelectedTabIndex = 0
        Me.tabSkillDetails.Size = New System.Drawing.Size(431, 281)
        Me.tabSkillDetails.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tabSkillDetails.TabIndex = 49
        Me.tabSkillDetails.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabSkillDetails.Tabs.Add(Me.tiGeneral)
        Me.tabSkillDetails.Tabs.Add(Me.TabItem1)
        Me.tabSkillDetails.Tabs.Add(Me.TabItem2)
        Me.tabSkillDetails.Tabs.Add(Me.TabItem3)
        Me.tabSkillDetails.Tabs.Add(Me.TabItem4)
        Me.tabSkillDetails.Tabs.Add(Me.TabItem6)
        Me.tabSkillDetails.Text = "TabControl1"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.lvwDetails)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(431, 258)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 1
        Me.TabControlPanel1.TabItem = Me.tiGeneral
        '
        'lvwDetails
        '
        Me.lvwDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvwDetails.Dock = System.Windows.Forms.DockStyle.Fill
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
        Me.lvwDetails.Location = New System.Drawing.Point(1, 1)
        Me.lvwDetails.MultiSelect = False
        Me.lvwDetails.Name = "lvwDetails"
        Me.lvwDetails.Size = New System.Drawing.Size(429, 256)
        Me.lvwDetails.TabIndex = 16
        Me.lvwDetails.UseCompatibleStateImageBehavior = False
        Me.lvwDetails.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Width = 120
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Width = 200
        '
        'tiGeneral
        '
        Me.tiGeneral.AttachedControl = Me.TabControlPanel1
        Me.tiGeneral.Name = "tiGeneral"
        Me.tiGeneral.Text = "General"
        '
        'TabControlPanel7
        '
        Me.TabControlPanel7.Controls.Add(Me.lvwTimes)
        Me.TabControlPanel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel7.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel7.Name = "TabControlPanel7"
        Me.TabControlPanel7.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel7.Size = New System.Drawing.Size(431, 258)
        Me.TabControlPanel7.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel7.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel7.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel7.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel7.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel7.Style.GradientAngle = 90
        Me.TabControlPanel7.TabIndex = 7
        Me.TabControlPanel7.TabItem = Me.TabItem6
        '
        'lvwTimes
        '
        Me.lvwTimes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.Standard, Me.Current, Me.Cumulative})
        Me.lvwTimes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwTimes.FullRowSelect = True
        Me.lvwTimes.GridLines = True
        Me.lvwTimes.Location = New System.Drawing.Point(1, 1)
        Me.lvwTimes.Name = "lvwTimes"
        Me.lvwTimes.Size = New System.Drawing.Size(429, 256)
        Me.lvwTimes.TabIndex = 1
        Me.lvwTimes.UseCompatibleStateImageBehavior = False
        Me.lvwTimes.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Lvl"
        Me.ColumnHeader6.Width = 30
        '
        'Standard
        '
        Me.Standard.Text = "Time to Level Up"
        Me.Standard.Width = 105
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
        'TabItem6
        '
        Me.TabItem6.AttachedControl = Me.TabControlPanel7
        Me.TabItem6.Name = "TabItem6"
        Me.TabItem6.Text = "Training Times"
        '
        'TabControlPanel5
        '
        Me.TabControlPanel5.Controls.Add(Me.lvwSPs)
        Me.TabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel5.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel5.Name = "TabControlPanel5"
        Me.TabControlPanel5.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel5.Size = New System.Drawing.Size(431, 258)
        Me.TabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel5.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel5.Style.GradientAngle = 90
        Me.TabControlPanel5.TabIndex = 5
        Me.TabControlPanel5.TabItem = Me.TabItem4
        '
        'TabItem4
        '
        Me.TabItem4.AttachedControl = Me.TabControlPanel5
        Me.TabItem4.Name = "TabItem4"
        Me.TabItem4.Text = "Skill Points"
        '
        'TabControlPanel4
        '
        Me.TabControlPanel4.Controls.Add(Me.lvwDepend)
        Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel4.Name = "TabControlPanel4"
        Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel4.Size = New System.Drawing.Size(431, 258)
        Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel4.Style.GradientAngle = 90
        Me.TabControlPanel4.TabIndex = 4
        Me.TabControlPanel4.TabItem = Me.TabItem3
        '
        'TabItem3
        '
        Me.TabItem3.AttachedControl = Me.TabControlPanel4
        Me.TabItem3.Name = "TabItem3"
        Me.TabItem3.Text = "Dependancies"
        '
        'TabControlPanel3
        '
        Me.TabControlPanel3.Controls.Add(Me.tvwReqs)
        Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel3.Name = "TabControlPanel3"
        Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel3.Size = New System.Drawing.Size(431, 258)
        Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel3.Style.GradientAngle = 90
        Me.TabControlPanel3.TabIndex = 3
        Me.TabControlPanel3.TabItem = Me.TabItem2
        '
        'TabItem2
        '
        Me.TabItem2.AttachedControl = Me.TabControlPanel3
        Me.TabItem2.Name = "TabItem2"
        Me.TabItem2.Text = "Pre-Reqs"
        '
        'TabControlPanel2
        '
        Me.TabControlPanel2.Controls.Add(Me.lblDescription)
        Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel2.Name = "TabControlPanel2"
        Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel2.Size = New System.Drawing.Size(431, 258)
        Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel2.Style.GradientAngle = 90
        Me.TabControlPanel2.TabIndex = 2
        Me.TabControlPanel2.TabItem = Me.TabItem1
        '
        'TabItem1
        '
        Me.TabItem1.AttachedControl = Me.TabControlPanel2
        Me.TabItem1.Name = "TabItem1"
        Me.TabItem1.Text = "Description"
        '
        'tabQueues
        '
        Me.tabQueues.BackColor = System.Drawing.Color.Transparent
        Me.tabQueues.CanReorderTabs = False
        Me.tabQueues.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tabQueues.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tabQueues.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tabQueues.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tabQueues.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tabQueues.Controls.Add(Me.tcpQueue)
        Me.tabQueues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabQueues.Location = New System.Drawing.Point(446, 0)
        Me.tabQueues.Name = "tabQueues"
        Me.tabQueues.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tabQueues.SelectedTabIndex = 0
        Me.tabQueues.Size = New System.Drawing.Size(838, 825)
        Me.tabQueues.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tabQueues.TabIndex = 50
        Me.tabQueues.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabQueues.Tabs.Add(Me.tabSummary)
        Me.tabQueues.Text = "TabControl1"
        Me.tabQueues.Visible = False
        '
        'tcpQueue
        '
        Me.tcpQueue.Controls.Add(Me.lvQueues)
        Me.tcpQueue.Controls.Add(Me.lblTotalQueueTime)
        Me.tcpQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpQueue.Location = New System.Drawing.Point(0, 23)
        Me.tcpQueue.Name = "tcpQueue"
        Me.tcpQueue.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpQueue.Size = New System.Drawing.Size(838, 802)
        Me.tcpQueue.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpQueue.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpQueue.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpQueue.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpQueue.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpQueue.Style.GradientAngle = 90
        Me.tcpQueue.TabIndex = 1
        Me.tcpQueue.TabItem = Me.tabSummary
        '
        'lvQueues
        '
        Me.lvQueues.AllowColumnReorder = True
        Me.lvQueues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvQueues.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colQName, Me.colQSkills, Me.colQTimeLeft, Me.colQQueuedTime, Me.colQEndDate})
        Me.lvQueues.ContextMenuStrip = Me.ctxQueues
        Me.lvQueues.FullRowSelect = True
        Me.lvQueues.GridLines = True
        Me.lvQueues.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvQueues.HideSelection = False
        Me.lvQueues.Location = New System.Drawing.Point(4, 0)
        Me.lvQueues.Name = "lvQueues"
        Me.lvQueues.Size = New System.Drawing.Size(830, 782)
        Me.lvQueues.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvQueues.TabIndex = 4
        Me.lvQueues.UseCompatibleStateImageBehavior = False
        Me.lvQueues.View = System.Windows.Forms.View.Details
        '
        'colQName
        '
        Me.colQName.Text = "Queue Name"
        Me.colQName.Width = 200
        '
        'colQSkills
        '
        Me.colQSkills.Text = "Skills"
        '
        'colQTimeLeft
        '
        Me.colQTimeLeft.Text = "Total Time"
        Me.colQTimeLeft.Width = 120
        '
        'colQQueuedTime
        '
        Me.colQQueuedTime.Text = "Queued Time"
        Me.colQQueuedTime.Width = 120
        '
        'colQEndDate
        '
        Me.colQEndDate.Text = "End Date"
        Me.colQEndDate.Width = 175
        '
        'ctxQueues
        '
        Me.ctxQueues.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxQueues.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddQueue, Me.mnuEditQueue, Me.mnuDeleteQueue, Me.ToolStripMenuItem5, Me.mnuMergeQueues, Me.mnuCopyQueue, Me.mnuCopyQueueToPilot, Me.ToolStripMenuItem6, Me.mnuSetPrimary})
        Me.ctxQueues.Name = "ctxQueues"
        Me.ctxQueues.Size = New System.Drawing.Size(179, 170)
        '
        'mnuAddQueue
        '
        Me.mnuAddQueue.Name = "mnuAddQueue"
        Me.mnuAddQueue.Size = New System.Drawing.Size(178, 22)
        Me.mnuAddQueue.Text = "Add Queue"
        '
        'mnuEditQueue
        '
        Me.mnuEditQueue.Enabled = False
        Me.mnuEditQueue.Name = "mnuEditQueue"
        Me.mnuEditQueue.Size = New System.Drawing.Size(178, 22)
        Me.mnuEditQueue.Text = "Edit Queue"
        '
        'mnuDeleteQueue
        '
        Me.mnuDeleteQueue.Enabled = False
        Me.mnuDeleteQueue.Name = "mnuDeleteQueue"
        Me.mnuDeleteQueue.Size = New System.Drawing.Size(178, 22)
        Me.mnuDeleteQueue.Text = "Delete Queue"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(175, 6)
        '
        'mnuMergeQueues
        '
        Me.mnuMergeQueues.Enabled = False
        Me.mnuMergeQueues.Name = "mnuMergeQueues"
        Me.mnuMergeQueues.Size = New System.Drawing.Size(178, 22)
        Me.mnuMergeQueues.Text = "Merge Queues"
        '
        'mnuCopyQueue
        '
        Me.mnuCopyQueue.Enabled = False
        Me.mnuCopyQueue.Name = "mnuCopyQueue"
        Me.mnuCopyQueue.Size = New System.Drawing.Size(178, 22)
        Me.mnuCopyQueue.Text = "Copy Queue"
        '
        'mnuCopyQueueToPilot
        '
        Me.mnuCopyQueueToPilot.Enabled = False
        Me.mnuCopyQueueToPilot.Name = "mnuCopyQueueToPilot"
        Me.mnuCopyQueueToPilot.Size = New System.Drawing.Size(178, 22)
        Me.mnuCopyQueueToPilot.Text = "Copy Queue to Pilot"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(175, 6)
        '
        'mnuSetPrimary
        '
        Me.mnuSetPrimary.Enabled = False
        Me.mnuSetPrimary.Name = "mnuSetPrimary"
        Me.mnuSetPrimary.Size = New System.Drawing.Size(178, 22)
        Me.mnuSetPrimary.Text = "Set as Primary Queue"
        '
        'tabSummary
        '
        Me.tabSummary.AttachedControl = Me.tcpQueue
        Me.tabSummary.Name = "tabSummary"
        Me.tabSummary.Text = "Queue Summary"
        '
        'panelInfo
        '
        Me.panelInfo.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelInfo.Controls.Add(Me.cboPilots)
        Me.panelInfo.Controls.Add(Me.lblPilot)
        Me.panelInfo.Controls.Add(Me.tabQueueMode)
        Me.panelInfo.Controls.Add(Me.tabSkillDetails)
        Me.panelInfo.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelInfo.Location = New System.Drawing.Point(0, 0)
        Me.panelInfo.Name = "panelInfo"
        Me.panelInfo.Size = New System.Drawing.Size(440, 825)
        Me.panelInfo.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelInfo.Style.GradientAngle = 90
        Me.panelInfo.TabIndex = 51
        Me.panelInfo.Visible = False
        '
        'cboPilots
        '
        Me.cboPilots.DisplayMember = "Text"
        Me.cboPilots.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.ItemHeight = 15
        Me.cboPilots.Location = New System.Drawing.Point(49, 6)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(214, 21)
        Me.cboPilots.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboPilots.TabIndex = 50
        '
        'ExpandableSplitter1
        '
        Me.ExpandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.ExpandableSplitter1.ExpandableControl = Me.panelInfo
        Me.ExpandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(61, Byte), Integer))
        Me.ExpandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.ExpandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2
        Me.ExpandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground
        Me.ExpandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter1.Location = New System.Drawing.Point(440, 0)
        Me.ExpandableSplitter1.Name = "ExpandableSplitter1"
        Me.ExpandableSplitter1.Size = New System.Drawing.Size(6, 825)
        Me.ExpandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007
        Me.ExpandableSplitter1.TabIndex = 52
        Me.ExpandableSplitter1.TabStop = False
        '
        'frmTraining
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1284, 825)
        Me.Controls.Add(Me.RibbonBarMergeContainer1)
        Me.Controls.Add(Me.tabQueues)
        Me.Controls.Add(Me.ExpandableSplitter1)
        Me.Controls.Add(Me.panelInfo)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmTraining"
        Me.Text = "Skill Training"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ctxDetails.ResumeLayout(False)
        Me.ctxQueue.ResumeLayout(False)
        Me.ctxReqs.ResumeLayout(False)
        Me.ctxDepend.ResumeLayout(False)
        Me.ctxCertDetails.ResumeLayout(False)
        Me.RibbonBarMergeContainer1.ResumeLayout(False)
        CType(Me.tabQueueMode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabQueueMode.ResumeLayout(False)
        Me.tcpSkillPlanning.ResumeLayout(False)
        Me.tcpSkillPlanning.PerformLayout()
        Me.tcpCertPlanning.ResumeLayout(False)
        Me.tcpCertPlanning.PerformLayout()
        CType(Me.tabSkillDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSkillDetails.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        Me.TabControlPanel7.ResumeLayout(False)
        Me.TabControlPanel5.ResumeLayout(False)
        Me.TabControlPanel4.ResumeLayout(False)
        Me.TabControlPanel3.ResumeLayout(False)
        Me.TabControlPanel2.ResumeLayout(False)
        CType(Me.tabQueues, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabQueues.ResumeLayout(False)
        Me.tcpQueue.ResumeLayout(False)
        Me.tcpQueue.PerformLayout()
        Me.ctxQueues.ResumeLayout(False)
        Me.panelInfo.ResumeLayout(False)
        Me.panelInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tvwSkillList As System.Windows.Forms.TreeView
    Friend WithEvents ctxQueue As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDeleteFromQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuIncreaseLevel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDecreaseLevel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuClearTrainingQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxDetails As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSkillName2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAddToQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewDetails2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMoveUpQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMoveDownQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSeperateLevelSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents mnuForceTraining2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuForceTraining As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents lblFilter As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
    Friend WithEvents lvwDepend As System.Windows.Forms.ListView
    Friend WithEvents NeededFor As System.Windows.Forms.ColumnHeader
    Friend WithEvents NeededLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwSPs As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwTimes As EveHQ.ListViewNoFlicker
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Standard As System.Windows.Forms.ColumnHeader
    Friend WithEvents Current As System.Windows.Forms.ColumnHeader
    Friend WithEvents Cumulative As System.Windows.Forms.ColumnHeader
    Friend WithEvents SkillToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ctxDepend As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewItemDetailsInIB As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSeparateLevels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSeparateAllLevels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSeparateTopLevel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSeparateBottomLevel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRemoveTrainedSkills As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lvQueues As EveHQ.ListViewNoFlicker
    Friend WithEvents colQName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQSkills As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQEndDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQTimeLeft As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQQueuedTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkOmitQueuesSkills As System.Windows.Forms.CheckBox
    Friend WithEvents lblTotalQueueTime As System.Windows.Forms.Label
    Friend WithEvents mnuAddGroupToQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddGroupLevel1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddGroupLevel2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddGroupLevel3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddGroupLevel4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddGroupLevel5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewItemDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewItemDetailsHere As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxReqs As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuReqsSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuReqsViewDetailsHere As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReqsViewDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddToQueueNext As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAddToQueue1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddToQueue2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddToQueue3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddToQueue4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddToQueue5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuChangeLevel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuChangeLevel1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuChangeLevel2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuChangeLevel3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuChangeLevel4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuChangeLevel5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboCertFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lblCertFilter As System.Windows.Forms.Label
    Friend WithEvents tvwCertList As System.Windows.Forms.TreeView
    Friend WithEvents ctxCertDetails As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCertName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAddCertToQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertToQueueNext As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAddCertToQueue1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertToQueue2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertToQueue3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertToQueue4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertToQueue5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertGroupToQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertGroupToQueue1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertGroupToQueue2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertGroupToQueue3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertGroupToQueue4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCertGroupToQueue5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewCertDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents mnuViewItemDetailsInCertScreen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditNote As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lvwDetails As EveHQ.ListViewNoFlicker
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents RibbonBarMergeContainer1 As DevComponents.DotNetBar.RibbonBarMergeContainer
    Friend WithEvents rbImportExport As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents ItemContainer1 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents btnEveMonImport As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnImportEMPFile As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnExportEMPFile As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbAttributeTools As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnImplants As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRemap As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents SuperTooltip1 As DevComponents.DotNetBar.SuperTooltip
    Friend WithEvents rbQueueAdmin As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnRBAddQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBEditQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBDeleteQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBSetPrimaryQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBMergeQueues As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBCopyQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBCopyQueueToPilot As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbQueueFunctions As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnRBAddSkill As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBDeleteSkill As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBIncreaseLevel As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBDecreaseLevel As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBMoveUpQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBMoveDownQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRBClearQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnIncTraining As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbQueueSettings As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents tabQueueMode As DevComponents.DotNetBar.TabControl
    Friend WithEvents tcpSkillPlanning As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiSkillPlanning As DevComponents.DotNetBar.TabItem
    Friend WithEvents tcpCertPlanning As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiCertPLanning As DevComponents.DotNetBar.TabItem
    Friend WithEvents tabSkillDetails As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents TabItem3 As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents TabItem2 As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents TabItem1 As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiGeneral As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel7 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents TabItem6 As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel5 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents TabItem4 As DevComponents.DotNetBar.TabItem
    Friend WithEvents tabQueues As DevComponents.DotNetBar.TabControl
    Friend WithEvents tcpQueue As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tabSummary As DevComponents.DotNetBar.TabItem
    Friend WithEvents btnQueueSettings As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ctxQueues As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDeleteQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuMergeQueues As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCopyQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCopyQueueToPilot As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSetPrimary As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnCollapseAll As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnExpandAll As DevComponents.DotNetBar.ButtonX
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExpandAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCollapseAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents panelInfo As DevComponents.DotNetBar.PanelEx
    Friend WithEvents ExpandableSplitter1 As DevComponents.DotNetBar.ExpandableSplitter
    Friend WithEvents cboPilots As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboFilter As DevComponents.DotNetBar.Controls.ComboBoxEx
	Friend WithEvents btnRBSplitQueue As DevComponents.DotNetBar.ButtonItem
	Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripSeparator
	Friend WithEvents mnuSplitQueue As System.Windows.Forms.ToolStripMenuItem
End Class
