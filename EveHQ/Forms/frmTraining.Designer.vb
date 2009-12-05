<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTraining
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTraining))
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Containers", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Materials", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Accessories", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Ships", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Modules", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup6 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Charges", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup7 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Blueprints", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup8 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Skills", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup9 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Commodities", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup10 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Drones", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup11 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Implants", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup12 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mobile Disruptors", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup13 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("POS Equipment", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup14 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Certificates", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup15 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("General", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup16 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Pilot Specific", System.Windows.Forms.HorizontalAlignment.Left)
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
        Me.tvwSkillList = New System.Windows.Forms.TreeView
        Me.ctxDetails = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName2 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuAddToQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddToQueueNext = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuAddToQueue1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddToQueue2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddToQueue3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddToQueue4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddToQueue5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddGroupToQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddGroupLevel1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddGroupLevel2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddGroupLevel3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddGroupLevel4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddGroupLevel5 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuForceTraining2 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewDetails2 = New System.Windows.Forms.ToolStripMenuItem
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ctxQueue = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuChangeLevel = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuIncreaseLevel = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDecreaseLevel = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuMoveUpQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMoveDownQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuSeparateLevels = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeparateAllLevels = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeparateTopLevel = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeparateBottomLevel = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeperateLevelSep = New System.Windows.Forms.ToolStripSeparator
        Me.mnuDeleteFromQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRemoveTrainedSkills = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClearTrainingQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditNote = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuForceTraining = New System.Windows.Forms.ToolStripMenuItem
        Me.tsQueueOptions = New System.Windows.Forms.ToolStrip
        Me.btnImportExport = New System.Windows.Forms.ToolStripSplitButton
        Me.mnuEveMonImport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuImportEMP = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExportEMP = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator
        Me.btnICT = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbNeuralRemap = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbImplants = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator
        Me.btnShowDetails = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.btnAddSkill = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.btnDeleteSkill = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.btnLevelUp = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator
        Me.btnLevelDown = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator
        Me.btnMoveUp = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator
        Me.btnMoveDown = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator
        Me.btnClearQueue = New System.Windows.Forms.ToolStripButton
        Me.lblFilter = New System.Windows.Forms.Label
        Me.cboFilter = New System.Windows.Forms.ComboBox
        Me.tabSkillDetails = New System.Windows.Forms.TabControl
        Me.tabDescription = New System.Windows.Forms.TabPage
        Me.lblDescription = New System.Windows.Forms.Label
        Me.tabPreReqs = New System.Windows.Forms.TabPage
        Me.tvwReqs = New System.Windows.Forms.TreeView
        Me.ctxReqs = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuReqsSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuReqsViewDetailsHere = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReqsViewDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.tabDepends = New System.Windows.Forms.TabPage
        Me.lvwDepend = New System.Windows.Forms.ListView
        Me.NeededFor = New System.Windows.Forms.ColumnHeader
        Me.NeededLevel = New System.Windows.Forms.ColumnHeader
        Me.ctxDepend = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuItemName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewItemDetailsHere = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewItemDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewItemDetailsInIB = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewItemDetailsInCertScreen = New System.Windows.Forms.ToolStripMenuItem
        Me.tabSP = New System.Windows.Forms.TabPage
        Me.lvwSPs = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.tabTimes = New System.Windows.Forms.TabPage
        Me.lvwTimes = New EveHQ.ListViewNoFlicker
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.Standard = New System.Windows.Forms.ColumnHeader
        Me.Current = New System.Windows.Forms.ColumnHeader
        Me.Cumulative = New System.Windows.Forms.ColumnHeader
        Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabQueues = New System.Windows.Forms.TabControl
        Me.tabSummary = New System.Windows.Forms.TabPage
        Me.lblTotalQueueTime = New System.Windows.Forms.Label
        Me.btnCopyToPilot = New System.Windows.Forms.Button
        Me.btnSetPrimary = New System.Windows.Forms.Button
        Me.btnCopyQueue = New System.Windows.Forms.Button
        Me.lvQueues = New EveHQ.ListViewNoFlicker
        Me.colQName = New System.Windows.Forms.ColumnHeader
        Me.colQSkills = New System.Windows.Forms.ColumnHeader
        Me.colQTimeLeft = New System.Windows.Forms.ColumnHeader
        Me.colQQueuedTime = New System.Windows.Forms.ColumnHeader
        Me.colQEndDate = New System.Windows.Forms.ColumnHeader
        Me.btnEditQueue = New System.Windows.Forms.Button
        Me.btnDeleteQueue = New System.Windows.Forms.Button
        Me.btnMergeQueues = New System.Windows.Forms.Button
        Me.btnAddQueue = New System.Windows.Forms.Button
        Me.chkOmitQueuesSkills = New System.Windows.Forms.CheckBox
        Me.tabQueueMode = New System.Windows.Forms.TabControl
        Me.tabSkillMode = New System.Windows.Forms.TabPage
        Me.panelSkillPlanning = New System.Windows.Forms.Panel
        Me.tabCertMode = New System.Windows.Forms.TabPage
        Me.panelCertPlanning = New System.Windows.Forms.Panel
        Me.cboCertFilter = New System.Windows.Forms.ComboBox
        Me.lblCertFilter = New System.Windows.Forms.Label
        Me.tvwCertList = New System.Windows.Forms.TreeView
        Me.ctxCertDetails = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCertName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuAddCertToQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertToQueueNext = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuAddCertToQueue1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertToQueue2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertToQueue3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertToQueue4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertToQueue5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertGroupToQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertGroupToQueue1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertGroupToQueue2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertGroupToQueue3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertGroupToQueue4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddCertGroupToQueue5 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewCertDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.lvwDetails = New EveHQ.ListViewNoFlicker
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ctxDetails.SuspendLayout()
        Me.ctxQueue.SuspendLayout()
        Me.tsQueueOptions.SuspendLayout()
        Me.tabSkillDetails.SuspendLayout()
        Me.tabDescription.SuspendLayout()
        Me.tabPreReqs.SuspendLayout()
        Me.ctxReqs.SuspendLayout()
        Me.tabDepends.SuspendLayout()
        Me.ctxDepend.SuspendLayout()
        Me.tabSP.SuspendLayout()
        Me.tabTimes.SuspendLayout()
        Me.tabQueues.SuspendLayout()
        Me.tabSummary.SuspendLayout()
        Me.tabQueueMode.SuspendLayout()
        Me.tabSkillMode.SuspendLayout()
        Me.panelSkillPlanning.SuspendLayout()
        Me.tabCertMode.SuspendLayout()
        Me.panelCertPlanning.SuspendLayout()
        Me.ctxCertDetails.SuspendLayout()
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
        Me.tvwSkillList.Location = New System.Drawing.Point(9, 32)
        Me.tvwSkillList.Name = "tvwSkillList"
        Me.tvwSkillList.SelectedImageIndex = 6
        Me.tvwSkillList.Size = New System.Drawing.Size(239, 274)
        Me.tvwSkillList.TabIndex = 0
        '
        'ctxDetails
        '
        Me.ctxDetails.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxDetails.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName2, Me.ToolStripSeparator5, Me.mnuAddToQueue, Me.mnuAddGroupToQueue, Me.ToolStripSeparator6, Me.mnuForceTraining2, Me.ToolStripMenuItem1, Me.mnuViewDetails2})
        Me.ctxDetails.Name = "ctxDepend"
        Me.ctxDetails.Size = New System.Drawing.Size(217, 132)
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
        Me.ctxQueue.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator1, Me.mnuChangeLevel, Me.mnuIncreaseLevel, Me.mnuDecreaseLevel, Me.ToolStripSeparator3, Me.mnuMoveUpQueue, Me.mnuMoveDownQueue, Me.ToolStripMenuItem3, Me.mnuSeparateLevels, Me.mnuSeperateLevelSep, Me.mnuDeleteFromQueue, Me.mnuRemoveTrainedSkills, Me.mnuClearTrainingQueue, Me.ToolStripSeparator2, Me.mnuViewDetails, Me.mnuEditNote, Me.ToolStripMenuItem2, Me.mnuForceTraining})
        Me.ctxQueue.Name = "ctxDepend"
        Me.ctxQueue.Size = New System.Drawing.Size(207, 326)
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
        'tsQueueOptions
        '
        Me.tsQueueOptions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsQueueOptions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnImportExport, Me.ToolStripSeparator21, Me.btnICT, Me.ToolStripSeparator14, Me.tsbNeuralRemap, Me.ToolStripSeparator19, Me.tsbImplants, Me.ToolStripSeparator20, Me.btnShowDetails, Me.ToolStripSeparator7, Me.btnAddSkill, Me.ToolStripSeparator8, Me.btnDeleteSkill, Me.ToolStripSeparator9, Me.btnLevelUp, Me.ToolStripSeparator10, Me.btnLevelDown, Me.ToolStripSeparator11, Me.btnMoveUp, Me.ToolStripSeparator13, Me.btnMoveDown, Me.ToolStripSeparator12, Me.btnClearQueue})
        Me.tsQueueOptions.Location = New System.Drawing.Point(0, 0)
        Me.tsQueueOptions.Name = "tsQueueOptions"
        Me.tsQueueOptions.Size = New System.Drawing.Size(1079, 39)
        Me.tsQueueOptions.TabIndex = 12
        Me.tsQueueOptions.Text = "ToolStrip1"
        '
        'btnImportExport
        '
        Me.btnImportExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnImportExport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEveMonImport, Me.mnuImportEMP, Me.ToolStripMenuItem5, Me.mnuExportEMP})
        Me.btnImportExport.Image = CType(resources.GetObject("btnImportExport.Image"), System.Drawing.Image)
        Me.btnImportExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImportExport.Name = "btnImportExport"
        Me.btnImportExport.Size = New System.Drawing.Size(91, 36)
        Me.btnImportExport.Text = "Import/Export"
        '
        'mnuEveMonImport
        '
        Me.mnuEveMonImport.Name = "mnuEveMonImport"
        Me.mnuEveMonImport.Size = New System.Drawing.Size(189, 22)
        Me.mnuEveMonImport.Text = "EveMon Import (Full)"
        '
        'mnuImportEMP
        '
        Me.mnuImportEMP.Name = "mnuImportEMP"
        Me.mnuImportEMP.Size = New System.Drawing.Size(189, 22)
        Me.mnuImportEMP.Text = "Import EveMon Plan File"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(186, 6)
        '
        'mnuExportEMP
        '
        Me.mnuExportEMP.Name = "mnuExportEMP"
        Me.mnuExportEMP.Size = New System.Drawing.Size(189, 22)
        Me.mnuExportEMP.Text = "Export EveMon Plan File"
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(6, 39)
        '
        'btnICT
        '
        Me.btnICT.CheckOnClick = True
        Me.btnICT.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnICT.Image = CType(resources.GetObject("btnICT.Image"), System.Drawing.Image)
        Me.btnICT.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnICT.Name = "btnICT"
        Me.btnICT.Size = New System.Drawing.Size(87, 36)
        Me.btnICT.Text = "Include Training"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 39)
        '
        'tsbNeuralRemap
        '
        Me.tsbNeuralRemap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNeuralRemap.Image = CType(resources.GetObject("tsbNeuralRemap.Image"), System.Drawing.Image)
        Me.tsbNeuralRemap.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbNeuralRemap.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNeuralRemap.Name = "tsbNeuralRemap"
        Me.tsbNeuralRemap.Size = New System.Drawing.Size(36, 36)
        Me.tsbNeuralRemap.Text = "ToolStripButton1"
        Me.tsbNeuralRemap.ToolTipText = "Neural Remapping"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(6, 39)
        '
        'tsbImplants
        '
        Me.tsbImplants.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbImplants.Image = CType(resources.GetObject("tsbImplants.Image"), System.Drawing.Image)
        Me.tsbImplants.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbImplants.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbImplants.Name = "tsbImplants"
        Me.tsbImplants.Size = New System.Drawing.Size(36, 36)
        Me.tsbImplants.Text = "ToolStripButton1"
        Me.tsbImplants.ToolTipText = "Implants"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(6, 39)
        '
        'btnShowDetails
        '
        Me.btnShowDetails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnShowDetails.Enabled = False
        Me.btnShowDetails.Image = CType(resources.GetObject("btnShowDetails.Image"), System.Drawing.Image)
        Me.btnShowDetails.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnShowDetails.Name = "btnShowDetails"
        Me.btnShowDetails.Size = New System.Drawing.Size(92, 36)
        Me.btnShowDetails.Text = "Show Skill Details"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 39)
        '
        'btnAddSkill
        '
        Me.btnAddSkill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnAddSkill.Enabled = False
        Me.btnAddSkill.Image = CType(resources.GetObject("btnAddSkill.Image"), System.Drawing.Image)
        Me.btnAddSkill.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAddSkill.Name = "btnAddSkill"
        Me.btnAddSkill.Size = New System.Drawing.Size(50, 36)
        Me.btnAddSkill.Text = "Add Skill"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 39)
        '
        'btnDeleteSkill
        '
        Me.btnDeleteSkill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnDeleteSkill.Enabled = False
        Me.btnDeleteSkill.Image = CType(resources.GetObject("btnDeleteSkill.Image"), System.Drawing.Image)
        Me.btnDeleteSkill.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDeleteSkill.Name = "btnDeleteSkill"
        Me.btnDeleteSkill.Size = New System.Drawing.Size(62, 36)
        Me.btnDeleteSkill.Text = "Delete Skill"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 39)
        '
        'btnLevelUp
        '
        Me.btnLevelUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnLevelUp.Enabled = False
        Me.btnLevelUp.Image = CType(resources.GetObject("btnLevelUp.Image"), System.Drawing.Image)
        Me.btnLevelUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLevelUp.Name = "btnLevelUp"
        Me.btnLevelUp.Size = New System.Drawing.Size(81, 36)
        Me.btnLevelUp.Text = "Increase Level"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 39)
        '
        'btnLevelDown
        '
        Me.btnLevelDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnLevelDown.Enabled = False
        Me.btnLevelDown.Image = CType(resources.GetObject("btnLevelDown.Image"), System.Drawing.Image)
        Me.btnLevelDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLevelDown.Name = "btnLevelDown"
        Me.btnLevelDown.Size = New System.Drawing.Size(84, 36)
        Me.btnLevelDown.Text = "Decrease Level"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 39)
        '
        'btnMoveUp
        '
        Me.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnMoveUp.Enabled = False
        Me.btnMoveUp.Image = CType(resources.GetObject("btnMoveUp.Image"), System.Drawing.Image)
        Me.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.Size = New System.Drawing.Size(88, 36)
        Me.btnMoveUp.Text = "Move Up Queue"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 39)
        '
        'btnMoveDown
        '
        Me.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnMoveDown.Enabled = False
        Me.btnMoveDown.Image = CType(resources.GetObject("btnMoveDown.Image"), System.Drawing.Image)
        Me.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(102, 36)
        Me.btnMoveDown.Text = "Move Down Queue"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(6, 39)
        '
        'btnClearQueue
        '
        Me.btnClearQueue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnClearQueue.Enabled = False
        Me.btnClearQueue.Image = CType(resources.GetObject("btnClearQueue.Image"), System.Drawing.Image)
        Me.btnClearQueue.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClearQueue.Name = "btnClearQueue"
        Me.btnClearQueue.Size = New System.Drawing.Size(71, 36)
        Me.btnClearQueue.Text = "Clear Queue"
        '
        'lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.Location = New System.Drawing.Point(9, 8)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(35, 13)
        Me.lblFilter.TabIndex = 13
        Me.lblFilter.Text = "Filter:"
        '
        'cboFilter
        '
        Me.cboFilter.DropDownHeight = 160
        Me.cboFilter.FormattingEnabled = True
        Me.cboFilter.IntegralHeight = False
        Me.cboFilter.Items.AddRange(New Object() {"Published Only", "All", "Non-Published Only", "Owned Skills", "Missing Skills", "New Skills Ready To Train", "Skills Ready To Train to Next Level", "Partially Trained", "Skills at Level 0", "Skills at Level 1", "Skills at Level 2", "Skills at Level 3", "Skills at Level 4", "Rank 1 Skills", "Rank 2 Skills", "Rank 3 Skills", "Rank 4 Skills", "Rank 5 Skills", "Rank 6 Skills", "Rank 7 Skills", "Rank 8 Skills", "Rank 9 Skills", "Rank 10 Skills", "Rank 11 Skills", "Rank 12 Skills", "Rank 13 Skills", "Rank 14 Skills", "Rank 15 Skills", "Rank 16 Skills", "Primary - Charisma", "Primary - Intelligence", "Primary - Memory", "Primary - Perception", "Primary - Willpower", "Secondary - Charisma", "Secondary - Intelligence", "Secondary - Memory", "Secondary - Perception", "Secondary - Willpower"})
        Me.cboFilter.Location = New System.Drawing.Point(56, 5)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.Size = New System.Drawing.Size(192, 21)
        Me.cboFilter.TabIndex = 14
        '
        'tabSkillDetails
        '
        Me.tabSkillDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tabSkillDetails.Appearance = System.Windows.Forms.TabAppearance.Buttons
        Me.tabSkillDetails.Controls.Add(Me.tabDescription)
        Me.tabSkillDetails.Controls.Add(Me.tabPreReqs)
        Me.tabSkillDetails.Controls.Add(Me.tabDepends)
        Me.tabSkillDetails.Controls.Add(Me.tabSP)
        Me.tabSkillDetails.Controls.Add(Me.tabTimes)
        Me.tabSkillDetails.Location = New System.Drawing.Point(369, 443)
        Me.tabSkillDetails.Name = "tabSkillDetails"
        Me.tabSkillDetails.SelectedIndex = 0
        Me.tabSkillDetails.Size = New System.Drawing.Size(470, 238)
        Me.tabSkillDetails.TabIndex = 16
        '
        'tabDescription
        '
        Me.tabDescription.Controls.Add(Me.lblDescription)
        Me.tabDescription.Location = New System.Drawing.Point(4, 25)
        Me.tabDescription.Name = "tabDescription"
        Me.tabDescription.Size = New System.Drawing.Size(462, 209)
        Me.tabDescription.TabIndex = 2
        Me.tabDescription.Text = "Description"
        Me.tabDescription.UseVisualStyleBackColor = True
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.Color.White
        Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDescription.Location = New System.Drawing.Point(5, 5)
        Me.lblDescription.Margin = New System.Windows.Forms.Padding(5)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Padding = New System.Windows.Forms.Padding(5)
        Me.lblDescription.Size = New System.Drawing.Size(452, 199)
        Me.lblDescription.TabIndex = 0
        '
        'tabPreReqs
        '
        Me.tabPreReqs.Controls.Add(Me.tvwReqs)
        Me.tabPreReqs.Location = New System.Drawing.Point(4, 25)
        Me.tabPreReqs.Name = "tabPreReqs"
        Me.tabPreReqs.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPreReqs.Size = New System.Drawing.Size(462, 209)
        Me.tabPreReqs.TabIndex = 0
        Me.tabPreReqs.Text = "Pre-requisites"
        Me.tabPreReqs.UseVisualStyleBackColor = True
        '
        'tvwReqs
        '
        Me.tvwReqs.ContextMenuStrip = Me.ctxReqs
        Me.tvwReqs.Indent = 25
        Me.tvwReqs.ItemHeight = 20
        Me.tvwReqs.Location = New System.Drawing.Point(3, 3)
        Me.tvwReqs.Name = "tvwReqs"
        Me.tvwReqs.ShowPlusMinus = False
        Me.tvwReqs.Size = New System.Drawing.Size(453, 200)
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
        'tabDepends
        '
        Me.tabDepends.Controls.Add(Me.lvwDepend)
        Me.tabDepends.Location = New System.Drawing.Point(4, 25)
        Me.tabDepends.Name = "tabDepends"
        Me.tabDepends.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDepends.Size = New System.Drawing.Size(462, 209)
        Me.tabDepends.TabIndex = 1
        Me.tabDepends.Text = "Dependancies"
        Me.tabDepends.UseVisualStyleBackColor = True
        '
        'lvwDepend
        '
        Me.lvwDepend.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NeededFor, Me.NeededLevel})
        Me.lvwDepend.ContextMenuStrip = Me.ctxDepend
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
        ListViewGroup9.Header = "Commodities"
        ListViewGroup9.Name = "Cat17"
        ListViewGroup10.Header = "Drones"
        ListViewGroup10.Name = "Cat18"
        ListViewGroup11.Header = "Implants"
        ListViewGroup11.Name = "Cat20"
        ListViewGroup12.Header = "Mobile Disruptors"
        ListViewGroup12.Name = "Cat22"
        ListViewGroup13.Header = "POS Equipment"
        ListViewGroup13.Name = "Cat23"
        ListViewGroup14.Header = "Certificates"
        ListViewGroup14.Name = "CatCerts"
        Me.lvwDepend.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4, ListViewGroup5, ListViewGroup6, ListViewGroup7, ListViewGroup8, ListViewGroup9, ListViewGroup10, ListViewGroup11, ListViewGroup12, ListViewGroup13, ListViewGroup14})
        Me.lvwDepend.Location = New System.Drawing.Point(3, 3)
        Me.lvwDepend.Name = "lvwDepend"
        Me.lvwDepend.ShowItemToolTips = True
        Me.lvwDepend.Size = New System.Drawing.Size(453, 200)
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
        'tabSP
        '
        Me.tabSP.Controls.Add(Me.lvwSPs)
        Me.tabSP.Location = New System.Drawing.Point(4, 25)
        Me.tabSP.Name = "tabSP"
        Me.tabSP.Size = New System.Drawing.Size(462, 209)
        Me.tabSP.TabIndex = 4
        Me.tabSP.Text = "Skill Points"
        Me.tabSP.UseVisualStyleBackColor = True
        '
        'lvwSPs
        '
        Me.lvwSPs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.lvwSPs.FullRowSelect = True
        Me.lvwSPs.GridLines = True
        Me.lvwSPs.Location = New System.Drawing.Point(3, 3)
        Me.lvwSPs.Name = "lvwSPs"
        Me.lvwSPs.Size = New System.Drawing.Size(456, 203)
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
        'tabTimes
        '
        Me.tabTimes.Controls.Add(Me.lvwTimes)
        Me.tabTimes.Location = New System.Drawing.Point(4, 25)
        Me.tabTimes.Name = "tabTimes"
        Me.tabTimes.Size = New System.Drawing.Size(462, 209)
        Me.tabTimes.TabIndex = 3
        Me.tabTimes.Text = "Training Times"
        Me.tabTimes.UseVisualStyleBackColor = True
        '
        'lvwTimes
        '
        Me.lvwTimes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.Standard, Me.Current, Me.Cumulative})
        Me.lvwTimes.FullRowSelect = True
        Me.lvwTimes.GridLines = True
        Me.lvwTimes.Location = New System.Drawing.Point(3, 3)
        Me.lvwTimes.Name = "lvwTimes"
        Me.lvwTimes.Size = New System.Drawing.Size(456, 203)
        Me.lvwTimes.TabIndex = 1
        Me.lvwTimes.UseCompatibleStateImageBehavior = False
        Me.lvwTimes.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "To Level"
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
        'tabQueues
        '
        Me.tabQueues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabQueues.Controls.Add(Me.tabSummary)
        Me.tabQueues.Location = New System.Drawing.Point(285, 42)
        Me.tabQueues.Multiline = True
        Me.tabQueues.Name = "tabQueues"
        Me.tabQueues.SelectedIndex = 0
        Me.tabQueues.Size = New System.Drawing.Size(782, 395)
        Me.tabQueues.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabQueues.TabIndex = 17
        '
        'tabSummary
        '
        Me.tabSummary.Controls.Add(Me.lblTotalQueueTime)
        Me.tabSummary.Controls.Add(Me.btnCopyToPilot)
        Me.tabSummary.Controls.Add(Me.btnSetPrimary)
        Me.tabSummary.Controls.Add(Me.btnCopyQueue)
        Me.tabSummary.Controls.Add(Me.lvQueues)
        Me.tabSummary.Controls.Add(Me.btnEditQueue)
        Me.tabSummary.Controls.Add(Me.btnDeleteQueue)
        Me.tabSummary.Controls.Add(Me.btnMergeQueues)
        Me.tabSummary.Controls.Add(Me.btnAddQueue)
        Me.tabSummary.Location = New System.Drawing.Point(4, 22)
        Me.tabSummary.Name = "tabSummary"
        Me.tabSummary.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSummary.Size = New System.Drawing.Size(774, 369)
        Me.tabSummary.TabIndex = 1
        Me.tabSummary.Text = "Queue Summary"
        Me.tabSummary.UseVisualStyleBackColor = True
        '
        'lblTotalQueueTime
        '
        Me.lblTotalQueueTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalQueueTime.AutoSize = True
        Me.lblTotalQueueTime.Location = New System.Drawing.Point(105, 351)
        Me.lblTotalQueueTime.Name = "lblTotalQueueTime"
        Me.lblTotalQueueTime.Size = New System.Drawing.Size(95, 13)
        Me.lblTotalQueueTime.TabIndex = 9
        Me.lblTotalQueueTime.Text = "Total Queue Time:"
        '
        'btnCopyToPilot
        '
        Me.btnCopyToPilot.Enabled = False
        Me.btnCopyToPilot.Location = New System.Drawing.Point(6, 180)
        Me.btnCopyToPilot.Name = "btnCopyToPilot"
        Me.btnCopyToPilot.Size = New System.Drawing.Size(96, 23)
        Me.btnCopyToPilot.TabIndex = 8
        Me.btnCopyToPilot.Text = "Copy To Pilot"
        Me.btnCopyToPilot.UseVisualStyleBackColor = True
        '
        'btnSetPrimary
        '
        Me.btnSetPrimary.Enabled = False
        Me.btnSetPrimary.Location = New System.Drawing.Point(6, 93)
        Me.btnSetPrimary.Name = "btnSetPrimary"
        Me.btnSetPrimary.Size = New System.Drawing.Size(96, 23)
        Me.btnSetPrimary.TabIndex = 7
        Me.btnSetPrimary.Text = "Set As Primary"
        Me.btnSetPrimary.UseVisualStyleBackColor = True
        '
        'btnCopyQueue
        '
        Me.btnCopyQueue.Enabled = False
        Me.btnCopyQueue.Location = New System.Drawing.Point(6, 151)
        Me.btnCopyQueue.Name = "btnCopyQueue"
        Me.btnCopyQueue.Size = New System.Drawing.Size(96, 23)
        Me.btnCopyQueue.TabIndex = 5
        Me.btnCopyQueue.Text = "Copy Queue"
        Me.btnCopyQueue.UseVisualStyleBackColor = True
        '
        'lvQueues
        '
        Me.lvQueues.AllowColumnReorder = True
        Me.lvQueues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvQueues.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colQName, Me.colQSkills, Me.colQTimeLeft, Me.colQQueuedTime, Me.colQEndDate})
        Me.lvQueues.FullRowSelect = True
        Me.lvQueues.GridLines = True
        Me.lvQueues.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvQueues.HideSelection = False
        Me.lvQueues.Location = New System.Drawing.Point(108, 6)
        Me.lvQueues.Name = "lvQueues"
        Me.lvQueues.Size = New System.Drawing.Size(659, 341)
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
        'btnEditQueue
        '
        Me.btnEditQueue.Enabled = False
        Me.btnEditQueue.Location = New System.Drawing.Point(6, 35)
        Me.btnEditQueue.Name = "btnEditQueue"
        Me.btnEditQueue.Size = New System.Drawing.Size(96, 23)
        Me.btnEditQueue.TabIndex = 3
        Me.btnEditQueue.Text = "Edit Queue"
        Me.btnEditQueue.UseVisualStyleBackColor = True
        '
        'btnDeleteQueue
        '
        Me.btnDeleteQueue.Enabled = False
        Me.btnDeleteQueue.Location = New System.Drawing.Point(6, 64)
        Me.btnDeleteQueue.Name = "btnDeleteQueue"
        Me.btnDeleteQueue.Size = New System.Drawing.Size(96, 23)
        Me.btnDeleteQueue.TabIndex = 2
        Me.btnDeleteQueue.Text = "Delete Queue"
        Me.btnDeleteQueue.UseVisualStyleBackColor = True
        '
        'btnMergeQueues
        '
        Me.btnMergeQueues.Enabled = False
        Me.btnMergeQueues.Location = New System.Drawing.Point(6, 122)
        Me.btnMergeQueues.Name = "btnMergeQueues"
        Me.btnMergeQueues.Size = New System.Drawing.Size(96, 23)
        Me.btnMergeQueues.TabIndex = 1
        Me.btnMergeQueues.Text = "Merge Queues"
        Me.btnMergeQueues.UseVisualStyleBackColor = True
        '
        'btnAddQueue
        '
        Me.btnAddQueue.Location = New System.Drawing.Point(6, 6)
        Me.btnAddQueue.Name = "btnAddQueue"
        Me.btnAddQueue.Size = New System.Drawing.Size(96, 23)
        Me.btnAddQueue.TabIndex = 0
        Me.btnAddQueue.Text = "Add Queue"
        Me.btnAddQueue.UseVisualStyleBackColor = True
        '
        'chkOmitQueuesSkills
        '
        Me.chkOmitQueuesSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkOmitQueuesSkills.AutoSize = True
        Me.chkOmitQueuesSkills.Location = New System.Drawing.Point(9, 312)
        Me.chkOmitQueuesSkills.Name = "chkOmitQueuesSkills"
        Me.chkOmitQueuesSkills.Size = New System.Drawing.Size(114, 17)
        Me.chkOmitQueuesSkills.TabIndex = 18
        Me.chkOmitQueuesSkills.Text = "Omit Queued Skills"
        Me.chkOmitQueuesSkills.UseVisualStyleBackColor = True
        '
        'tabQueueMode
        '
        Me.tabQueueMode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tabQueueMode.Controls.Add(Me.tabSkillMode)
        Me.tabQueueMode.Controls.Add(Me.tabCertMode)
        Me.tabQueueMode.Location = New System.Drawing.Point(12, 70)
        Me.tabQueueMode.Name = "tabQueueMode"
        Me.tabQueueMode.SelectedIndex = 0
        Me.tabQueueMode.Size = New System.Drawing.Size(271, 367)
        Me.tabQueueMode.TabIndex = 19
        '
        'tabSkillMode
        '
        Me.tabSkillMode.BackColor = System.Drawing.Color.Transparent
        Me.tabSkillMode.Controls.Add(Me.panelSkillPlanning)
        Me.tabSkillMode.Location = New System.Drawing.Point(4, 22)
        Me.tabSkillMode.Name = "tabSkillMode"
        Me.tabSkillMode.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSkillMode.Size = New System.Drawing.Size(263, 341)
        Me.tabSkillMode.TabIndex = 0
        Me.tabSkillMode.Text = "Skill Planning"
        Me.tabSkillMode.UseVisualStyleBackColor = True
        '
        'panelSkillPlanning
        '
        Me.panelSkillPlanning.BackColor = System.Drawing.SystemColors.Window
        Me.panelSkillPlanning.Controls.Add(Me.cboFilter)
        Me.panelSkillPlanning.Controls.Add(Me.lblFilter)
        Me.panelSkillPlanning.Controls.Add(Me.tvwSkillList)
        Me.panelSkillPlanning.Controls.Add(Me.chkOmitQueuesSkills)
        Me.panelSkillPlanning.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelSkillPlanning.Location = New System.Drawing.Point(3, 3)
        Me.panelSkillPlanning.Name = "panelSkillPlanning"
        Me.panelSkillPlanning.Size = New System.Drawing.Size(257, 335)
        Me.panelSkillPlanning.TabIndex = 0
        '
        'tabCertMode
        '
        Me.tabCertMode.BackColor = System.Drawing.Color.Transparent
        Me.tabCertMode.Controls.Add(Me.panelCertPlanning)
        Me.tabCertMode.Location = New System.Drawing.Point(4, 22)
        Me.tabCertMode.Name = "tabCertMode"
        Me.tabCertMode.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCertMode.Size = New System.Drawing.Size(263, 341)
        Me.tabCertMode.TabIndex = 1
        Me.tabCertMode.Text = "Certificate Planning"
        Me.tabCertMode.UseVisualStyleBackColor = True
        '
        'panelCertPlanning
        '
        Me.panelCertPlanning.BackColor = System.Drawing.SystemColors.Window
        Me.panelCertPlanning.Controls.Add(Me.cboCertFilter)
        Me.panelCertPlanning.Controls.Add(Me.lblCertFilter)
        Me.panelCertPlanning.Controls.Add(Me.tvwCertList)
        Me.panelCertPlanning.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelCertPlanning.Location = New System.Drawing.Point(3, 3)
        Me.panelCertPlanning.Name = "panelCertPlanning"
        Me.panelCertPlanning.Size = New System.Drawing.Size(257, 335)
        Me.panelCertPlanning.TabIndex = 0
        '
        'cboCertFilter
        '
        Me.cboCertFilter.DropDownHeight = 160
        Me.cboCertFilter.FormattingEnabled = True
        Me.cboCertFilter.IntegralHeight = False
        Me.cboCertFilter.Items.AddRange(New Object() {"All", "Owned Certificates", "Missing Certificates", "Basic Certificates - All", "Standard Certificates - All", "Improved Certificates - All", "Advanced Certificates - All", "Elite Certificates - All", "Basic Certificates - Missing", "Standard Certificates - Missing", "Improved Certificates - Missing", "Advanced Certificates - Missing", "Elite Certificates - Missing"})
        Me.cboCertFilter.Location = New System.Drawing.Point(56, 5)
        Me.cboCertFilter.Name = "cboCertFilter"
        Me.cboCertFilter.Size = New System.Drawing.Size(192, 21)
        Me.cboCertFilter.TabIndex = 17
        '
        'lblCertFilter
        '
        Me.lblCertFilter.AutoSize = True
        Me.lblCertFilter.Location = New System.Drawing.Point(9, 8)
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
        Me.tvwCertList.Location = New System.Drawing.Point(9, 32)
        Me.tvwCertList.Name = "tvwCertList"
        Me.tvwCertList.SelectedImageIndex = 6
        Me.tvwCertList.Size = New System.Drawing.Size(239, 300)
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
        'cboPilots
        '
        Me.cboPilots.DropDownHeight = 250
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.IntegralHeight = False
        Me.cboPilots.Location = New System.Drawing.Point(45, 42)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(175, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 43
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(9, 45)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 42
        Me.lblPilot.Text = "Pilot:"
        '
        'lvwDetails
        '
        Me.lvwDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvwDetails.FullRowSelect = True
        Me.lvwDetails.GridLines = True
        ListViewGroup15.Header = "General"
        ListViewGroup15.Name = "General"
        ListViewGroup16.Header = "Pilot Specific"
        ListViewGroup16.Name = "Specific"
        Me.lvwDetails.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup15, ListViewGroup16})
        Me.lvwDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        ListViewItem1.Group = ListViewGroup15
        ListViewItem2.Group = ListViewGroup15
        ListViewItem3.Group = ListViewGroup15
        ListViewItem4.Group = ListViewGroup15
        ListViewItem5.Group = ListViewGroup15
        ListViewItem6.Group = ListViewGroup15
        ListViewItem7.Group = ListViewGroup16
        ListViewItem8.Group = ListViewGroup16
        ListViewItem9.Group = ListViewGroup16
        ListViewItem10.Group = ListViewGroup16
        Me.lvwDetails.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6, ListViewItem7, ListViewItem8, ListViewItem9, ListViewItem10})
        Me.lvwDetails.Location = New System.Drawing.Point(12, 443)
        Me.lvwDetails.MultiSelect = False
        Me.lvwDetails.Name = "lvwDetails"
        Me.lvwDetails.Scrollable = False
        Me.lvwDetails.Size = New System.Drawing.Size(351, 233)
        Me.lvwDetails.TabIndex = 15
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
        'frmTraining
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1079, 688)
        Me.Controls.Add(Me.tabQueueMode)
        Me.Controls.Add(Me.tabQueues)
        Me.Controls.Add(Me.lvwDetails)
        Me.Controls.Add(Me.tabSkillDetails)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblPilot)
        Me.Controls.Add(Me.tsQueueOptions)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmTraining"
        Me.Text = "Skill Training"
        Me.ctxDetails.ResumeLayout(False)
        Me.ctxQueue.ResumeLayout(False)
        Me.tsQueueOptions.ResumeLayout(False)
        Me.tsQueueOptions.PerformLayout()
        Me.tabSkillDetails.ResumeLayout(False)
        Me.tabDescription.ResumeLayout(False)
        Me.tabPreReqs.ResumeLayout(False)
        Me.ctxReqs.ResumeLayout(False)
        Me.tabDepends.ResumeLayout(False)
        Me.ctxDepend.ResumeLayout(False)
        Me.tabSP.ResumeLayout(False)
        Me.tabTimes.ResumeLayout(False)
        Me.tabQueues.ResumeLayout(False)
        Me.tabSummary.ResumeLayout(False)
        Me.tabSummary.PerformLayout()
        Me.tabQueueMode.ResumeLayout(False)
        Me.tabSkillMode.ResumeLayout(False)
        Me.panelSkillPlanning.ResumeLayout(False)
        Me.panelSkillPlanning.PerformLayout()
        Me.tabCertMode.ResumeLayout(False)
        Me.panelCertPlanning.ResumeLayout(False)
        Me.panelCertPlanning.PerformLayout()
        Me.ctxCertDetails.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
    Friend WithEvents tsQueueOptions As System.Windows.Forms.ToolStrip
    Friend WithEvents btnShowDetails As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnAddSkill As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnDeleteSkill As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnLevelUp As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnLevelDown As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnClearQueue As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnMoveUp As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnMoveDown As System.Windows.Forms.ToolStripButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents mnuForceTraining2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuForceTraining As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents lblFilter As System.Windows.Forms.Label
    Friend WithEvents cboFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lvwDetails As EveHQ.ListViewNoFlicker
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabSkillDetails As System.Windows.Forms.TabControl
    Friend WithEvents tabDescription As System.Windows.Forms.TabPage
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents tabPreReqs As System.Windows.Forms.TabPage
    Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
    Friend WithEvents tabDepends As System.Windows.Forms.TabPage
    Friend WithEvents lvwDepend As System.Windows.Forms.ListView
    Friend WithEvents NeededFor As System.Windows.Forms.ColumnHeader
    Friend WithEvents NeededLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabSP As System.Windows.Forms.TabPage
    Friend WithEvents lvwSPs As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabTimes As System.Windows.Forms.TabPage
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
    Friend WithEvents tabQueues As System.Windows.Forms.TabControl
    Friend WithEvents tabSummary As System.Windows.Forms.TabPage
    Friend WithEvents lvQueues As EveHQ.ListViewNoFlicker
    Friend WithEvents btnEditQueue As System.Windows.Forms.Button
    Friend WithEvents btnDeleteQueue As System.Windows.Forms.Button
    Friend WithEvents btnMergeQueues As System.Windows.Forms.Button
    Friend WithEvents btnAddQueue As System.Windows.Forms.Button
    Friend WithEvents colQName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQSkills As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQEndDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQTimeLeft As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnCopyQueue As System.Windows.Forms.Button
    Friend WithEvents colQQueuedTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnSetPrimary As System.Windows.Forms.Button
    Friend WithEvents btnCopyToPilot As System.Windows.Forms.Button
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
    Friend WithEvents btnICT As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tabQueueMode As System.Windows.Forms.TabControl
    Friend WithEvents tabSkillMode As System.Windows.Forms.TabPage
    Friend WithEvents panelSkillPlanning As System.Windows.Forms.Panel
    Friend WithEvents tabCertMode As System.Windows.Forms.TabPage
    Friend WithEvents panelCertPlanning As System.Windows.Forms.Panel
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
    Friend WithEvents tsbNeuralRemap As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbImplants As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator20 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents mnuViewItemDetailsInCertScreen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnImportExport As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents mnuEveMonImport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuImportEMP As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExportEMP As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator21 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuEditNote As System.Windows.Forms.ToolStripMenuItem
End Class
