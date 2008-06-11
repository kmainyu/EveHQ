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
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Containers", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup6 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Materials", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup7 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Accessories", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup8 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Ships", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup9 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Modules", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup10 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Charges", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup11 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Blueprints", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup12 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Skills", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup27 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Drones", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup28 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Implants", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup31 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mobile Disruptors", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup32 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("POS Equipment", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup17 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("General", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup18 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Pilot Specific", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewItem41 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Skill Name", ""}, -1)
        Dim ListViewItem42 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Skill Rank", ""}, -1)
        Dim ListViewItem43 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Skill Group", ""}, -1)
        Dim ListViewItem44 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Skill Price", ""}, -1)
        Dim ListViewItem45 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Primary Attribute", ""}, -1)
        Dim ListViewItem46 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Secondary Attribute", ""}, -1)
        Dim ListViewItem47 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Current Level", ""}, -1)
        Dim ListViewItem48 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Current SP", ""}, -1)
        Dim ListViewItem49 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Time to Next Level", ""}, -1)
        Dim ListViewItem50 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Training Rate (SP/Hr)", ""}, -1)
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
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuForceTraining = New System.Windows.Forms.ToolStripMenuItem
        Me.tsQueueOptions = New System.Windows.Forms.ToolStrip
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
        Me.tabSP = New System.Windows.Forms.TabPage
        Me.lvwSPs = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.tabTimes = New System.Windows.Forms.TabPage
        Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabQueues = New System.Windows.Forms.TabControl
        Me.tabSummary = New System.Windows.Forms.TabPage
        Me.lblTotalQueueTime = New System.Windows.Forms.Label
        Me.btnCopyToPilot = New System.Windows.Forms.Button
        Me.btnSetPrimary = New System.Windows.Forms.Button
        Me.btnImportEveMon = New System.Windows.Forms.Button
        Me.btnCopyQueue = New System.Windows.Forms.Button
        Me.btnEditQueue = New System.Windows.Forms.Button
        Me.btnDeleteQueue = New System.Windows.Forms.Button
        Me.btnMergeQueues = New System.Windows.Forms.Button
        Me.btnAddQueue = New System.Windows.Forms.Button
        Me.chkOmitQueuesSkills = New System.Windows.Forms.CheckBox
        Me.mnuChangeLevel = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuChangeLevel5 = New System.Windows.Forms.ToolStripMenuItem
        Me.lvQueues = New EveHQ.ListViewNoFlicker
        Me.colQName = New System.Windows.Forms.ColumnHeader
        Me.colQSkills = New System.Windows.Forms.ColumnHeader
        Me.colQTimeLeft = New System.Windows.Forms.ColumnHeader
        Me.colQQueuedTime = New System.Windows.Forms.ColumnHeader
        Me.colQEndDate = New System.Windows.Forms.ColumnHeader
        Me.lvwDetails = New EveHQ.ListViewNoFlicker
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.lvwTimes = New EveHQ.ListViewNoFlicker
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.Standard = New System.Windows.Forms.ColumnHeader
        Me.Current = New System.Windows.Forms.ColumnHeader
        Me.Cumulative = New System.Windows.Forms.ColumnHeader
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
        Me.SuspendLayout()
        '
        'tvwSkillList
        '
        Me.tvwSkillList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tvwSkillList.ContextMenuStrip = Me.ctxDetails
        Me.tvwSkillList.FullRowSelect = True
        Me.tvwSkillList.HideSelection = False
        Me.tvwSkillList.ImageKey = "Blank.jpg"
        Me.tvwSkillList.ImageList = Me.ImageList1
        Me.tvwSkillList.Indent = 20
        Me.tvwSkillList.Location = New System.Drawing.Point(12, 63)
        Me.tvwSkillList.Name = "tvwSkillList"
        Me.tvwSkillList.SelectedImageIndex = 6
        Me.tvwSkillList.Size = New System.Drawing.Size(239, 347)
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
        Me.ctxQueue.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator1, Me.mnuChangeLevel, Me.mnuIncreaseLevel, Me.mnuDecreaseLevel, Me.ToolStripSeparator3, Me.mnuMoveUpQueue, Me.mnuMoveDownQueue, Me.ToolStripMenuItem3, Me.mnuSeparateLevels, Me.mnuSeperateLevelSep, Me.mnuDeleteFromQueue, Me.mnuRemoveTrainedSkills, Me.mnuClearTrainingQueue, Me.ToolStripSeparator2, Me.mnuViewDetails, Me.ToolStripMenuItem2, Me.mnuForceTraining})
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
        Me.tsQueueOptions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnShowDetails, Me.ToolStripSeparator7, Me.btnAddSkill, Me.ToolStripSeparator8, Me.btnDeleteSkill, Me.ToolStripSeparator9, Me.btnLevelUp, Me.ToolStripSeparator10, Me.btnLevelDown, Me.ToolStripSeparator11, Me.btnMoveUp, Me.ToolStripSeparator13, Me.btnMoveDown, Me.ToolStripSeparator12, Me.btnClearQueue})
        Me.tsQueueOptions.Location = New System.Drawing.Point(0, 0)
        Me.tsQueueOptions.Name = "tsQueueOptions"
        Me.tsQueueOptions.Size = New System.Drawing.Size(912, 25)
        Me.tsQueueOptions.TabIndex = 12
        Me.tsQueueOptions.Text = "ToolStrip1"
        '
        'btnShowDetails
        '
        Me.btnShowDetails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnShowDetails.Image = CType(resources.GetObject("btnShowDetails.Image"), System.Drawing.Image)
        Me.btnShowDetails.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnShowDetails.Name = "btnShowDetails"
        Me.btnShowDetails.Size = New System.Drawing.Size(102, 22)
        Me.btnShowDetails.Text = "Show Skill Details"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'btnAddSkill
        '
        Me.btnAddSkill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnAddSkill.Image = CType(resources.GetObject("btnAddSkill.Image"), System.Drawing.Image)
        Me.btnAddSkill.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAddSkill.Name = "btnAddSkill"
        Me.btnAddSkill.Size = New System.Drawing.Size(57, 22)
        Me.btnAddSkill.Text = "Add Skill"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'btnDeleteSkill
        '
        Me.btnDeleteSkill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnDeleteSkill.Enabled = False
        Me.btnDeleteSkill.Image = CType(resources.GetObject("btnDeleteSkill.Image"), System.Drawing.Image)
        Me.btnDeleteSkill.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDeleteSkill.Name = "btnDeleteSkill"
        Me.btnDeleteSkill.Size = New System.Drawing.Size(68, 22)
        Me.btnDeleteSkill.Text = "Delete Skill"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'btnLevelUp
        '
        Me.btnLevelUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnLevelUp.Enabled = False
        Me.btnLevelUp.Image = CType(resources.GetObject("btnLevelUp.Image"), System.Drawing.Image)
        Me.btnLevelUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLevelUp.Name = "btnLevelUp"
        Me.btnLevelUp.Size = New System.Drawing.Size(84, 22)
        Me.btnLevelUp.Text = "Increase Level"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 25)
        '
        'btnLevelDown
        '
        Me.btnLevelDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnLevelDown.Enabled = False
        Me.btnLevelDown.Image = CType(resources.GetObject("btnLevelDown.Image"), System.Drawing.Image)
        Me.btnLevelDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLevelDown.Name = "btnLevelDown"
        Me.btnLevelDown.Size = New System.Drawing.Size(88, 22)
        Me.btnLevelDown.Text = "Decrease Level"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 25)
        '
        'btnMoveUp
        '
        Me.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnMoveUp.Enabled = False
        Me.btnMoveUp.Image = CType(resources.GetObject("btnMoveUp.Image"), System.Drawing.Image)
        Me.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.Size = New System.Drawing.Size(97, 22)
        Me.btnMoveUp.Text = "Move Up Queue"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 25)
        '
        'btnMoveDown
        '
        Me.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnMoveDown.Enabled = False
        Me.btnMoveDown.Image = CType(resources.GetObject("btnMoveDown.Image"), System.Drawing.Image)
        Me.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(113, 22)
        Me.btnMoveDown.Text = "Move Down Queue"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(6, 25)
        '
        'btnClearQueue
        '
        Me.btnClearQueue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnClearQueue.Image = CType(resources.GetObject("btnClearQueue.Image"), System.Drawing.Image)
        Me.btnClearQueue.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClearQueue.Name = "btnClearQueue"
        Me.btnClearQueue.Size = New System.Drawing.Size(76, 22)
        Me.btnClearQueue.Text = "Clear Queue"
        '
        'lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.Location = New System.Drawing.Point(12, 38)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(32, 13)
        Me.lblFilter.TabIndex = 13
        Me.lblFilter.Text = "Filter:"
        '
        'cboFilter
        '
        Me.cboFilter.DropDownHeight = 160
        Me.cboFilter.FormattingEnabled = True
        Me.cboFilter.IntegralHeight = False
        Me.cboFilter.Items.AddRange(New Object() {"Published Only", "All", "Non-Published Only", "Owned Skills", "Missing Skills", "New Skills Ready To Train", "Skills Ready To Train to Next Level", "Partially Trained", "Skills at Level 0", "Skills at Level 1", "Skills at Level 2", "Skills at Level 3", "Skills at Level 4", "Rank 1 Skills", "Rank 2 Skills", "Rank 3 Skills", "Rank 4 Skills", "Rank 5 Skills", "Rank 6 Skills", "Rank 7 Skills", "Rank 8 Skills", "Rank 9 Skills", "Rank 10 Skills", "Rank 11 Skills", "Rank 12 Skills", "Rank 13 Skills", "Rank 14 Skills", "Rank 15 Skills", "Rank 16 Skills", "Primary - Charisma", "Primary - Intelligence", "Primary - Memory", "Primary - Perception", "Primary - Willpower", "Secondary - Charisma", "Secondary - Intelligence", "Secondary - Memory", "Secondary - Perception", "Secondary - Willpower"})
        Me.cboFilter.Location = New System.Drawing.Point(59, 35)
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
        Me.lblDescription.Size = New System.Drawing.Size(452, 189)
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
        Me.tvwReqs.Size = New System.Drawing.Size(453, 191)
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
        ListViewGroup5.Header = "Containers"
        ListViewGroup5.Name = "Cat2"
        ListViewGroup6.Header = "Materials"
        ListViewGroup6.Name = "Cat4"
        ListViewGroup7.Header = "Accessories"
        ListViewGroup7.Name = "Cat5"
        ListViewGroup8.Header = "Ships"
        ListViewGroup8.Name = "Cat6"
        ListViewGroup9.Header = "Modules"
        ListViewGroup9.Name = "Cat7"
        ListViewGroup10.Header = "Charges"
        ListViewGroup10.Name = "Cat8"
        ListViewGroup11.Header = "Blueprints"
        ListViewGroup11.Name = "Cat9"
        ListViewGroup12.Header = "Skills"
        ListViewGroup12.Name = "Cat16"
        ListViewGroup27.Header = "Drones"
        ListViewGroup27.Name = "Cat18"
        ListViewGroup28.Header = "Implants"
        ListViewGroup28.Name = "Cat20"
        ListViewGroup31.Header = "Mobile Disruptors"
        ListViewGroup31.Name = "Cat22"
        ListViewGroup32.Header = "POS Equipment"
        ListViewGroup32.Name = "Cat23"
        Me.lvwDepend.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup5, ListViewGroup6, ListViewGroup7, ListViewGroup8, ListViewGroup9, ListViewGroup10, ListViewGroup11, ListViewGroup12, ListViewGroup27, ListViewGroup28, ListViewGroup31, ListViewGroup32})
        Me.lvwDepend.Location = New System.Drawing.Point(3, 3)
        Me.lvwDepend.Name = "lvwDepend"
        Me.lvwDepend.ShowItemToolTips = True
        Me.lvwDepend.Size = New System.Drawing.Size(453, 191)
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
        Me.ctxDepend.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuItemName, Me.ToolStripSeparator4, Me.mnuViewItemDetailsHere, Me.mnuViewItemDetails, Me.mnuViewItemDetailsInIB})
        Me.ctxDepend.Name = "ctxDepend"
        Me.ctxDepend.Size = New System.Drawing.Size(212, 98)
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
        Me.lvwSPs.Size = New System.Drawing.Size(456, 191)
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
        'tabQueues
        '
        Me.tabQueues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabQueues.Controls.Add(Me.tabSummary)
        Me.tabQueues.Location = New System.Drawing.Point(257, 35)
        Me.tabQueues.Name = "tabQueues"
        Me.tabQueues.SelectedIndex = 0
        Me.tabQueues.Size = New System.Drawing.Size(643, 402)
        Me.tabQueues.TabIndex = 17
        '
        'tabSummary
        '
        Me.tabSummary.Controls.Add(Me.lblTotalQueueTime)
        Me.tabSummary.Controls.Add(Me.btnCopyToPilot)
        Me.tabSummary.Controls.Add(Me.btnSetPrimary)
        Me.tabSummary.Controls.Add(Me.btnImportEveMon)
        Me.tabSummary.Controls.Add(Me.btnCopyQueue)
        Me.tabSummary.Controls.Add(Me.lvQueues)
        Me.tabSummary.Controls.Add(Me.btnEditQueue)
        Me.tabSummary.Controls.Add(Me.btnDeleteQueue)
        Me.tabSummary.Controls.Add(Me.btnMergeQueues)
        Me.tabSummary.Controls.Add(Me.btnAddQueue)
        Me.tabSummary.Location = New System.Drawing.Point(4, 22)
        Me.tabSummary.Name = "tabSummary"
        Me.tabSummary.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSummary.Size = New System.Drawing.Size(635, 376)
        Me.tabSummary.TabIndex = 1
        Me.tabSummary.Text = "Queue Summary"
        Me.tabSummary.UseVisualStyleBackColor = True
        '
        'lblTotalQueueTime
        '
        Me.lblTotalQueueTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalQueueTime.AutoSize = True
        Me.lblTotalQueueTime.Location = New System.Drawing.Point(105, 358)
        Me.lblTotalQueueTime.Name = "lblTotalQueueTime"
        Me.lblTotalQueueTime.Size = New System.Drawing.Size(95, 13)
        Me.lblTotalQueueTime.TabIndex = 9
        Me.lblTotalQueueTime.Text = "Total Queue Time:"
        '
        'btnCopyToPilot
        '
        Me.btnCopyToPilot.Enabled = False
        Me.btnCopyToPilot.Location = New System.Drawing.Point(6, 264)
        Me.btnCopyToPilot.Name = "btnCopyToPilot"
        Me.btnCopyToPilot.Size = New System.Drawing.Size(85, 23)
        Me.btnCopyToPilot.TabIndex = 8
        Me.btnCopyToPilot.Text = "Copy To Pilot"
        Me.btnCopyToPilot.UseVisualStyleBackColor = True
        '
        'btnSetPrimary
        '
        Me.btnSetPrimary.Enabled = False
        Me.btnSetPrimary.Location = New System.Drawing.Point(6, 141)
        Me.btnSetPrimary.Name = "btnSetPrimary"
        Me.btnSetPrimary.Size = New System.Drawing.Size(85, 23)
        Me.btnSetPrimary.TabIndex = 7
        Me.btnSetPrimary.Text = "Set As Primary"
        Me.btnSetPrimary.UseVisualStyleBackColor = True
        '
        'btnImportEveMon
        '
        Me.btnImportEveMon.Location = New System.Drawing.Point(6, 323)
        Me.btnImportEveMon.Name = "btnImportEveMon"
        Me.btnImportEveMon.Size = New System.Drawing.Size(85, 45)
        Me.btnImportEveMon.TabIndex = 6
        Me.btnImportEveMon.Text = "Import Evemon Plans"
        Me.btnImportEveMon.UseVisualStyleBackColor = True
        '
        'btnCopyQueue
        '
        Me.btnCopyQueue.Enabled = False
        Me.btnCopyQueue.Location = New System.Drawing.Point(6, 235)
        Me.btnCopyQueue.Name = "btnCopyQueue"
        Me.btnCopyQueue.Size = New System.Drawing.Size(85, 23)
        Me.btnCopyQueue.TabIndex = 5
        Me.btnCopyQueue.Text = "Copy Queue"
        Me.btnCopyQueue.UseVisualStyleBackColor = True
        '
        'btnEditQueue
        '
        Me.btnEditQueue.Enabled = False
        Me.btnEditQueue.Location = New System.Drawing.Point(6, 70)
        Me.btnEditQueue.Name = "btnEditQueue"
        Me.btnEditQueue.Size = New System.Drawing.Size(85, 23)
        Me.btnEditQueue.TabIndex = 3
        Me.btnEditQueue.Text = "Edit Queue"
        Me.btnEditQueue.UseVisualStyleBackColor = True
        '
        'btnDeleteQueue
        '
        Me.btnDeleteQueue.Enabled = False
        Me.btnDeleteQueue.Location = New System.Drawing.Point(6, 99)
        Me.btnDeleteQueue.Name = "btnDeleteQueue"
        Me.btnDeleteQueue.Size = New System.Drawing.Size(85, 23)
        Me.btnDeleteQueue.TabIndex = 2
        Me.btnDeleteQueue.Text = "Delete Queue"
        Me.btnDeleteQueue.UseVisualStyleBackColor = True
        '
        'btnMergeQueues
        '
        Me.btnMergeQueues.Enabled = False
        Me.btnMergeQueues.Location = New System.Drawing.Point(6, 206)
        Me.btnMergeQueues.Name = "btnMergeQueues"
        Me.btnMergeQueues.Size = New System.Drawing.Size(85, 23)
        Me.btnMergeQueues.TabIndex = 1
        Me.btnMergeQueues.Text = "Merge Queues"
        Me.btnMergeQueues.UseVisualStyleBackColor = True
        '
        'btnAddQueue
        '
        Me.btnAddQueue.Location = New System.Drawing.Point(6, 41)
        Me.btnAddQueue.Name = "btnAddQueue"
        Me.btnAddQueue.Size = New System.Drawing.Size(85, 23)
        Me.btnAddQueue.TabIndex = 0
        Me.btnAddQueue.Text = "Add Queue"
        Me.btnAddQueue.UseVisualStyleBackColor = True
        '
        'chkOmitQueuesSkills
        '
        Me.chkOmitQueuesSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkOmitQueuesSkills.AutoSize = True
        Me.chkOmitQueuesSkills.Location = New System.Drawing.Point(12, 416)
        Me.chkOmitQueuesSkills.Name = "chkOmitQueuesSkills"
        Me.chkOmitQueuesSkills.Size = New System.Drawing.Size(115, 17)
        Me.chkOmitQueuesSkills.TabIndex = 18
        Me.chkOmitQueuesSkills.Text = "Omit Queued Skills"
        Me.chkOmitQueuesSkills.UseVisualStyleBackColor = True
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
        Me.mnuChangeLevel1.Size = New System.Drawing.Size(152, 22)
        Me.mnuChangeLevel1.Text = "Level 1"
        '
        'mnuChangeLevel2
        '
        Me.mnuChangeLevel2.Name = "mnuChangeLevel2"
        Me.mnuChangeLevel2.Size = New System.Drawing.Size(152, 22)
        Me.mnuChangeLevel2.Text = "Level 2"
        '
        'mnuChangeLevel3
        '
        Me.mnuChangeLevel3.Name = "mnuChangeLevel3"
        Me.mnuChangeLevel3.Size = New System.Drawing.Size(152, 22)
        Me.mnuChangeLevel3.Text = "Level 3"
        '
        'mnuChangeLevel4
        '
        Me.mnuChangeLevel4.Name = "mnuChangeLevel4"
        Me.mnuChangeLevel4.Size = New System.Drawing.Size(152, 22)
        Me.mnuChangeLevel4.Text = "Level 4"
        '
        'mnuChangeLevel5
        '
        Me.mnuChangeLevel5.Name = "mnuChangeLevel5"
        Me.mnuChangeLevel5.Size = New System.Drawing.Size(152, 22)
        Me.mnuChangeLevel5.Text = "Level 5"
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
        Me.lvQueues.Location = New System.Drawing.Point(97, 6)
        Me.lvQueues.Name = "lvQueues"
        Me.lvQueues.Size = New System.Drawing.Size(531, 348)
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
        'lvwDetails
        '
        Me.lvwDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvwDetails.FullRowSelect = True
        Me.lvwDetails.GridLines = True
        ListViewGroup17.Header = "General"
        ListViewGroup17.Name = "General"
        ListViewGroup18.Header = "Pilot Specific"
        ListViewGroup18.Name = "Specific"
        Me.lvwDetails.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup17, ListViewGroup18})
        Me.lvwDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        ListViewItem41.Group = ListViewGroup17
        ListViewItem42.Group = ListViewGroup17
        ListViewItem43.Group = ListViewGroup17
        ListViewItem44.Group = ListViewGroup17
        ListViewItem45.Group = ListViewGroup17
        ListViewItem46.Group = ListViewGroup17
        ListViewItem47.Group = ListViewGroup18
        ListViewItem48.Group = ListViewGroup18
        ListViewItem49.Group = ListViewGroup18
        ListViewItem50.Group = ListViewGroup18
        Me.lvwDetails.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem41, ListViewItem42, ListViewItem43, ListViewItem44, ListViewItem45, ListViewItem46, ListViewItem47, ListViewItem48, ListViewItem49, ListViewItem50})
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
        'lvwTimes
        '
        Me.lvwTimes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.Standard, Me.Current, Me.Cumulative})
        Me.lvwTimes.FullRowSelect = True
        Me.lvwTimes.GridLines = True
        Me.lvwTimes.Location = New System.Drawing.Point(3, 3)
        Me.lvwTimes.Name = "lvwTimes"
        Me.lvwTimes.Size = New System.Drawing.Size(456, 191)
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
        'frmTraining
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(912, 688)
        Me.Controls.Add(Me.tabQueues)
        Me.Controls.Add(Me.lvwDetails)
        Me.Controls.Add(Me.cboFilter)
        Me.Controls.Add(Me.lblFilter)
        Me.Controls.Add(Me.tabSkillDetails)
        Me.Controls.Add(Me.tsQueueOptions)
        Me.Controls.Add(Me.tvwSkillList)
        Me.Controls.Add(Me.chkOmitQueuesSkills)
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
    Friend WithEvents btnImportEveMon As System.Windows.Forms.Button
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
End Class
