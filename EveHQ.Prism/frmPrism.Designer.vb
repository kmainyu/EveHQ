<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrism
    Inherits System.Windows.Forms.Form

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
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Corporation", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup6 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Personal", System.Windows.Forms.HorizontalAlignment.Left)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrism))
        Me.lblSelectChar = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.tlvAssets = New DotNetLib.Windows.Forms.ContainerListView
        Me.colItem = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colOwner = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colGroup = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colCategory = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colLocation = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colMetaLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colVolume = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colValue = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ctxAssets = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuItemName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewInIB = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuViewInHQF = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuModifyPrice = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolSep = New System.Windows.Forms.ToolStripSeparator
        Me.mnuItemRecycling = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRecycleItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRecycleContained = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRecycleAll = New System.Windows.Forms.ToolStripMenuItem
        Me.chkExcludeBPs = New System.Windows.Forms.CheckBox
        Me.tvwFilter = New System.Windows.Forms.TreeView
        Me.ctxFilter = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddToFilterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblGroupFilter = New System.Windows.Forms.Label
        Me.lstFilters = New System.Windows.Forms.ListBox
        Me.ctxFilterList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveFilterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblSelectedFilters = New System.Windows.Forms.Label
        Me.tabPrism = New System.Windows.Forms.TabControl
        Me.tabAssets = New System.Windows.Forms.TabPage
        Me.txtMinSystemValue = New System.Windows.Forms.TextBox
        Me.chkMinSystemValue = New System.Windows.Forms.CheckBox
        Me.chkExcludeItems = New System.Windows.Forms.CheckBox
        Me.chkExcludeInvestments = New System.Windows.Forms.CheckBox
        Me.chkExcludeCash = New System.Windows.Forms.CheckBox
        Me.lblGroupFilters = New System.Windows.Forms.Label
        Me.lblOwnerFilters = New System.Windows.Forms.Label
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabFilters = New System.Windows.Forms.TabPage
        Me.btnSelectCorp = New System.Windows.Forms.Button
        Me.btnSelectPersonal = New System.Windows.Forms.Button
        Me.btnAddAllOwners = New System.Windows.Forms.Button
        Me.btnClearAllOwners = New System.Windows.Forms.Button
        Me.btnClearGroupFilters = New System.Windows.Forms.Button
        Me.lvwCharFilter = New System.Windows.Forms.ListView
        Me.colOwnerName = New System.Windows.Forms.ColumnHeader
        Me.lblCharFilter = New System.Windows.Forms.Label
        Me.tabAssetsAPI = New System.Windows.Forms.TabPage
        Me.lblCurrentAPI = New System.Windows.Forms.Label
        Me.lvwCurrentAPIs = New System.Windows.Forms.ListView
        Me.colAPIOwner = New System.Windows.Forms.ColumnHeader
        Me.colOwnerType = New System.Windows.Forms.ColumnHeader
        Me.colAssetsAPI = New System.Windows.Forms.ColumnHeader
        Me.colBalancesAPI = New System.Windows.Forms.ColumnHeader
        Me.colJobsAPI = New System.Windows.Forms.ColumnHeader
        Me.colJournalAPI = New System.Windows.Forms.ColumnHeader
        Me.colOrdersAPI = New System.Windows.Forms.ColumnHeader
        Me.colTransAPI = New System.Windows.Forms.ColumnHeader
        Me.colCorpSheetAPI = New System.Windows.Forms.ColumnHeader
        Me.tabInvestments = New System.Windows.Forms.TabPage
        Me.btnReOpenInvestment = New System.Windows.Forms.Button
        Me.chkViewClosedInvestments = New System.Windows.Forms.CheckBox
        Me.btnCloseInvestment = New System.Windows.Forms.Button
        Me.btnEditInvestment = New System.Windows.Forms.Button
        Me.btnAuditInvestment = New System.Windows.Forms.Button
        Me.btnEditTransaction = New System.Windows.Forms.Button
        Me.btnClearTransactions = New System.Windows.Forms.Button
        Me.lvwTransactions = New System.Windows.Forms.ListView
        Me.colTransID = New System.Windows.Forms.ColumnHeader
        Me.colTransDate = New System.Windows.Forms.ColumnHeader
        Me.colTransType = New System.Windows.Forms.ColumnHeader
        Me.colTransQuantity = New System.Windows.Forms.ColumnHeader
        Me.colTransValue = New System.Windows.Forms.ColumnHeader
        Me.btnAddTransaction = New System.Windows.Forms.Button
        Me.btnClearInvestments = New System.Windows.Forms.Button
        Me.btnAddInvestment = New System.Windows.Forms.Button
        Me.lvwInvestments = New System.Windows.Forms.ListView
        Me.colInvID = New System.Windows.Forms.ColumnHeader
        Me.colInvName = New System.Windows.Forms.ColumnHeader
        Me.colInvOwner = New System.Windows.Forms.ColumnHeader
        Me.colInvCQuantity = New System.Windows.Forms.ColumnHeader
        Me.colInvCCost = New System.Windows.Forms.ColumnHeader
        Me.colInvCValue = New System.Windows.Forms.ColumnHeader
        Me.colInvCTotalValue = New System.Windows.Forms.ColumnHeader
        Me.colInvCPotProfit = New System.Windows.Forms.ColumnHeader
        Me.colInvCActProfit = New System.Windows.Forms.ColumnHeader
        Me.colInvCIncome = New System.Windows.Forms.ColumnHeader
        Me.colInvCYield = New System.Windows.Forms.ColumnHeader
        Me.tabRigBuilder = New System.Windows.Forms.TabPage
        Me.lblTotalRigMargin = New System.Windows.Forms.Label
        Me.lblTotalRigProfit = New System.Windows.Forms.Label
        Me.lblTotalRigSalePrice = New System.Windows.Forms.Label
        Me.bgAutoRig = New System.Windows.Forms.GroupBox
        Me.radTotalProfit = New System.Windows.Forms.RadioButton
        Me.radTotalSalePrice = New System.Windows.Forms.RadioButton
        Me.radRigMargin = New System.Windows.Forms.RadioButton
        Me.radRigProfit = New System.Windows.Forms.RadioButton
        Me.radRigSaleprice = New System.Windows.Forms.RadioButton
        Me.lblAutoRigCriteria = New System.Windows.Forms.Label
        Me.btnAutoRig = New System.Windows.Forms.Button
        Me.lblRigBuildList = New System.Windows.Forms.Label
        Me.lvwRigBuildList = New DotNetLib.Windows.Forms.ContainerListView
        Me.ContainerListViewColumnHeader9 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader10 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader11 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader12 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader13 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader14 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader15 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader16 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader17 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lvwRigs = New DotNetLib.Windows.Forms.ContainerListView
        Me.colRigType = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRigQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRigMarketPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSalvageMarketPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBuildBenefit = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTotalRigValue = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTotalSalvageValue = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTotalBuildBenefit = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colMargin = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.nudRigMELevel = New System.Windows.Forms.NumericUpDown
        Me.lblRigMELevel = New System.Windows.Forms.Label
        Me.btnBuildRigs = New System.Windows.Forms.Button
        Me.lblRigOwnerFilter = New System.Windows.Forms.Label
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.tssLabelTotalAssetsLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.tssLabelTotalAssets = New System.Windows.Forms.ToolStripStatusLabel
        Me.tssLabelSelectedAssetsLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.tssLabelSelectedAssets = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsbDownloadData = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbDownloadOutposts = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbRefreshAssets = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbReports = New System.Windows.Forms.ToolStripSplitButton
        Me.mnuLocation = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetLists = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListName = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListQuantity = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListQuantityA = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListQuantityD = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListPrice = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListPriceA = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListPriceD = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListValue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListValueA = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAssetListValueD = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxAssets.SuspendLayout()
        Me.ctxFilter.SuspendLayout()
        Me.ctxFilterList.SuspendLayout()
        Me.tabPrism.SuspendLayout()
        Me.tabAssets.SuspendLayout()
        Me.tabFilters.SuspendLayout()
        Me.tabAssetsAPI.SuspendLayout()
        Me.tabInvestments.SuspendLayout()
        Me.tabRigBuilder.SuspendLayout()
        Me.bgAutoRig.SuspendLayout()
        CType(Me.nudRigMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSelectChar
        '
        Me.lblSelectChar.AutoSize = True
        Me.lblSelectChar.Location = New System.Drawing.Point(6, 9)
        Me.lblSelectChar.Name = "lblSelectChar"
        Me.lblSelectChar.Size = New System.Drawing.Size(89, 13)
        Me.lblSelectChar.TabIndex = 1
        Me.lblSelectChar.Text = "Select Character:"
        '
        'cboPilots
        '
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(113, 6)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(215, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 2
        '
        'tlvAssets
        '
        Me.tlvAssets.AllowMultiSelect = True
        Me.tlvAssets.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlvAssets.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colItem, Me.colOwner, Me.colGroup, Me.colCategory, Me.colLocation, Me.colMetaLevel, Me.colVolume, Me.colQuantity, Me.colPrice, Me.colValue})
        Me.tlvAssets.ColumnSortColor = System.Drawing.Color.AliceBlue
        Me.tlvAssets.ColumnTracking = True
        Me.tlvAssets.ColumnTrackingColor = System.Drawing.Color.LightCyan
        Me.tlvAssets.DefaultItemHeight = 20
        Me.tlvAssets.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tlvAssets.ItemContextMenu = Me.ctxAssets
        Me.tlvAssets.ItemSelectedColor = System.Drawing.Color.LimeGreen
        Me.tlvAssets.ItemTracking = True
        Me.tlvAssets.ItemTrackingColor = System.Drawing.Color.PaleGreen
        Me.tlvAssets.Location = New System.Drawing.Point(3, 83)
        Me.tlvAssets.MultipleColumnSort = True
        Me.tlvAssets.Name = "tlvAssets"
        Me.tlvAssets.ShowPlusMinus = True
        Me.tlvAssets.ShowRootTreeLines = True
        Me.tlvAssets.ShowTreeLines = True
        Me.tlvAssets.Size = New System.Drawing.Size(1130, 455)
        Me.tlvAssets.TabIndex = 6
        '
        'colItem
        '
        Me.colItem.CustomSortTag = Nothing
        Me.colItem.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colItem.Tag = Nothing
        Me.colItem.Text = "Location/ItemName"
        Me.colItem.Width = 300
        Me.colItem.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'colOwner
        '
        Me.colOwner.CustomSortTag = Nothing
        Me.colOwner.DisplayIndex = 1
        Me.colOwner.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colOwner.Tag = Nothing
        Me.colOwner.Text = "Owner"
        Me.colOwner.Width = 100
        '
        'colGroup
        '
        Me.colGroup.CustomSortTag = Nothing
        Me.colGroup.DisplayIndex = 2
        Me.colGroup.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colGroup.Tag = Nothing
        Me.colGroup.Text = "Group"
        '
        'colCategory
        '
        Me.colCategory.CustomSortTag = Nothing
        Me.colCategory.DisplayIndex = 3
        Me.colCategory.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colCategory.Tag = Nothing
        Me.colCategory.Text = "Category"
        '
        'colLocation
        '
        Me.colLocation.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colLocation.CustomSortTag = Nothing
        Me.colLocation.DisplayIndex = 4
        Me.colLocation.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colLocation.Tag = Nothing
        Me.colLocation.Text = "Specific Location"
        Me.colLocation.Width = 150
        '
        'colMetaLevel
        '
        Me.colMetaLevel.CustomSortTag = Nothing
        Me.colMetaLevel.DisplayIndex = 5
        Me.colMetaLevel.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colMetaLevel.Tag = Nothing
        Me.colMetaLevel.Text = "Meta"
        Me.colMetaLevel.Width = 60
        '
        'colVolume
        '
        Me.colVolume.CustomSortTag = Nothing
        Me.colVolume.DisplayIndex = 6
        Me.colVolume.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colVolume.Tag = Nothing
        Me.colVolume.Text = "Volume"
        '
        'colQuantity
        '
        Me.colQuantity.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colQuantity.CustomSortTag = Nothing
        Me.colQuantity.DisplayIndex = 7
        Me.colQuantity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colQuantity.Tag = Nothing
        Me.colQuantity.Text = "Quantity"
        Me.colQuantity.Width = 100
        Me.colQuantity.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'colPrice
        '
        Me.colPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colPrice.CustomSortTag = Nothing
        Me.colPrice.DisplayIndex = 8
        Me.colPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colPrice.Tag = Nothing
        Me.colPrice.Text = "Price"
        Me.colPrice.Width = 125
        '
        'colValue
        '
        Me.colValue.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colValue.CustomSortTag = Nothing
        Me.colValue.DisplayIndex = 9
        Me.colValue.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colValue.Tag = Nothing
        Me.colValue.Text = "Total Value"
        Me.colValue.Width = 125
        '
        'ctxAssets
        '
        Me.ctxAssets.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuItemName, Me.ToolStripMenuItem1, Me.mnuViewInIB, Me.mnuViewInHQF, Me.mnuModifyPrice, Me.mnuToolSep, Me.mnuItemRecycling})
        Me.ctxAssets.Name = "ctxAssets"
        Me.ctxAssets.Size = New System.Drawing.Size(188, 126)
        '
        'mnuItemName
        '
        Me.mnuItemName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.mnuItemName.Name = "mnuItemName"
        Me.mnuItemName.Size = New System.Drawing.Size(187, 22)
        Me.mnuItemName.Text = "Item Name"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(184, 6)
        '
        'mnuViewInIB
        '
        Me.mnuViewInIB.Name = "mnuViewInIB"
        Me.mnuViewInIB.Size = New System.Drawing.Size(187, 22)
        Me.mnuViewInIB.Text = "View In Item Browser"
        '
        'mnuViewInHQF
        '
        Me.mnuViewInHQF.Name = "mnuViewInHQF"
        Me.mnuViewInHQF.Size = New System.Drawing.Size(187, 22)
        Me.mnuViewInHQF.Text = "Copy Setup for HQF"
        '
        'mnuModifyPrice
        '
        Me.mnuModifyPrice.Name = "mnuModifyPrice"
        Me.mnuModifyPrice.Size = New System.Drawing.Size(187, 22)
        Me.mnuModifyPrice.Text = "Modify Custom Price"
        '
        'mnuToolSep
        '
        Me.mnuToolSep.Name = "mnuToolSep"
        Me.mnuToolSep.Size = New System.Drawing.Size(184, 6)
        '
        'mnuItemRecycling
        '
        Me.mnuItemRecycling.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRecycleItem, Me.mnuRecycleContained, Me.mnuRecycleAll})
        Me.mnuItemRecycling.Name = "mnuItemRecycling"
        Me.mnuItemRecycling.Size = New System.Drawing.Size(187, 22)
        Me.mnuItemRecycling.Text = "Recycling Profitability"
        '
        'mnuRecycleItem
        '
        Me.mnuRecycleItem.Name = "mnuRecycleItem"
        Me.mnuRecycleItem.Size = New System.Drawing.Size(173, 22)
        Me.mnuRecycleItem.Text = "Current Item"
        '
        'mnuRecycleContained
        '
        Me.mnuRecycleContained.Enabled = False
        Me.mnuRecycleContained.Name = "mnuRecycleContained"
        Me.mnuRecycleContained.Size = New System.Drawing.Size(173, 22)
        Me.mnuRecycleContained.Text = "Contained Items"
        '
        'mnuRecycleAll
        '
        Me.mnuRecycleAll.Enabled = False
        Me.mnuRecycleAll.Name = "mnuRecycleAll"
        Me.mnuRecycleAll.Size = New System.Drawing.Size(173, 22)
        Me.mnuRecycleAll.Text = "Container + Items"
        '
        'chkExcludeBPs
        '
        Me.chkExcludeBPs.AutoSize = True
        Me.chkExcludeBPs.Location = New System.Drawing.Point(107, 60)
        Me.chkExcludeBPs.Name = "chkExcludeBPs"
        Me.chkExcludeBPs.Size = New System.Drawing.Size(113, 17)
        Me.chkExcludeBPs.TabIndex = 7
        Me.chkExcludeBPs.Text = "Exclude Blueprints"
        Me.chkExcludeBPs.UseVisualStyleBackColor = True
        '
        'tvwFilter
        '
        Me.tvwFilter.ContextMenuStrip = Me.ctxFilter
        Me.tvwFilter.Location = New System.Drawing.Point(301, 31)
        Me.tvwFilter.Name = "tvwFilter"
        Me.tvwFilter.Size = New System.Drawing.Size(189, 488)
        Me.tvwFilter.TabIndex = 10
        '
        'ctxFilter
        '
        Me.ctxFilter.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToFilterToolStripMenuItem})
        Me.ctxFilter.Name = "ctxFilter"
        Me.ctxFilter.Size = New System.Drawing.Size(147, 26)
        '
        'AddToFilterToolStripMenuItem
        '
        Me.AddToFilterToolStripMenuItem.Name = "AddToFilterToolStripMenuItem"
        Me.AddToFilterToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.AddToFilterToolStripMenuItem.Text = "Add To Filter"
        '
        'lblGroupFilter
        '
        Me.lblGroupFilter.AutoSize = True
        Me.lblGroupFilter.Location = New System.Drawing.Point(298, 15)
        Me.lblGroupFilter.Name = "lblGroupFilter"
        Me.lblGroupFilter.Size = New System.Drawing.Size(64, 13)
        Me.lblGroupFilter.TabIndex = 11
        Me.lblGroupFilter.Text = "Group Filter:"
        '
        'lstFilters
        '
        Me.lstFilters.ContextMenuStrip = Me.ctxFilterList
        Me.lstFilters.FormattingEnabled = True
        Me.lstFilters.Location = New System.Drawing.Point(496, 31)
        Me.lstFilters.Name = "lstFilters"
        Me.lstFilters.Size = New System.Drawing.Size(167, 459)
        Me.lstFilters.Sorted = True
        Me.lstFilters.TabIndex = 12
        '
        'ctxFilterList
        '
        Me.ctxFilterList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveFilterToolStripMenuItem})
        Me.ctxFilterList.Name = "ctxFilterList"
        Me.ctxFilterList.Size = New System.Drawing.Size(152, 26)
        '
        'RemoveFilterToolStripMenuItem
        '
        Me.RemoveFilterToolStripMenuItem.Name = "RemoveFilterToolStripMenuItem"
        Me.RemoveFilterToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.RemoveFilterToolStripMenuItem.Text = "Remove Filter"
        '
        'lblSelectedFilters
        '
        Me.lblSelectedFilters.AutoSize = True
        Me.lblSelectedFilters.Location = New System.Drawing.Point(493, 15)
        Me.lblSelectedFilters.Name = "lblSelectedFilters"
        Me.lblSelectedFilters.Size = New System.Drawing.Size(114, 13)
        Me.lblSelectedFilters.TabIndex = 13
        Me.lblSelectedFilters.Text = "Selected Group Filters:"
        '
        'tabPrism
        '
        Me.tabPrism.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabPrism.Controls.Add(Me.tabAssetsAPI)
        Me.tabPrism.Controls.Add(Me.tabAssets)
        Me.tabPrism.Controls.Add(Me.tabFilters)
        Me.tabPrism.Controls.Add(Me.tabInvestments)
        Me.tabPrism.Controls.Add(Me.tabRigBuilder)
        Me.tabPrism.Location = New System.Drawing.Point(0, 28)
        Me.tabPrism.Name = "tabPrism"
        Me.tabPrism.SelectedIndex = 0
        Me.tabPrism.Size = New System.Drawing.Size(1144, 567)
        Me.tabPrism.TabIndex = 14
        '
        'tabAssets
        '
        Me.tabAssets.Controls.Add(Me.txtMinSystemValue)
        Me.tabAssets.Controls.Add(Me.chkMinSystemValue)
        Me.tabAssets.Controls.Add(Me.chkExcludeItems)
        Me.tabAssets.Controls.Add(Me.chkExcludeInvestments)
        Me.tabAssets.Controls.Add(Me.chkExcludeCash)
        Me.tabAssets.Controls.Add(Me.lblGroupFilters)
        Me.tabAssets.Controls.Add(Me.lblOwnerFilters)
        Me.tabAssets.Controls.Add(Me.txtSearch)
        Me.tabAssets.Controls.Add(Me.Label1)
        Me.tabAssets.Controls.Add(Me.tlvAssets)
        Me.tabAssets.Controls.Add(Me.cboPilots)
        Me.tabAssets.Controls.Add(Me.lblSelectChar)
        Me.tabAssets.Controls.Add(Me.chkExcludeBPs)
        Me.tabAssets.Location = New System.Drawing.Point(4, 22)
        Me.tabAssets.Name = "tabAssets"
        Me.tabAssets.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAssets.Size = New System.Drawing.Size(1136, 541)
        Me.tabAssets.TabIndex = 0
        Me.tabAssets.Text = "Assets"
        Me.tabAssets.UseVisualStyleBackColor = True
        '
        'txtMinSystemValue
        '
        Me.txtMinSystemValue.Location = New System.Drawing.Point(572, 57)
        Me.txtMinSystemValue.Name = "txtMinSystemValue"
        Me.txtMinSystemValue.Size = New System.Drawing.Size(135, 20)
        Me.txtMinSystemValue.TabIndex = 26
        Me.txtMinSystemValue.Text = "0.00"
        Me.txtMinSystemValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkMinSystemValue
        '
        Me.chkMinSystemValue.AutoSize = True
        Me.chkMinSystemValue.Location = New System.Drawing.Point(453, 60)
        Me.chkMinSystemValue.Name = "chkMinSystemValue"
        Me.chkMinSystemValue.Size = New System.Drawing.Size(113, 17)
        Me.chkMinSystemValue.TabIndex = 25
        Me.chkMinSystemValue.Text = "Min. System Value"
        Me.chkMinSystemValue.UseVisualStyleBackColor = True
        '
        'chkExcludeItems
        '
        Me.chkExcludeItems.AutoSize = True
        Me.chkExcludeItems.Location = New System.Drawing.Point(9, 60)
        Me.chkExcludeItems.Name = "chkExcludeItems"
        Me.chkExcludeItems.Size = New System.Drawing.Size(92, 17)
        Me.chkExcludeItems.TabIndex = 24
        Me.chkExcludeItems.Text = "Exclude Items"
        Me.chkExcludeItems.UseVisualStyleBackColor = True
        '
        'chkExcludeInvestments
        '
        Me.chkExcludeInvestments.AutoSize = True
        Me.chkExcludeInvestments.Location = New System.Drawing.Point(323, 60)
        Me.chkExcludeInvestments.Name = "chkExcludeInvestments"
        Me.chkExcludeInvestments.Size = New System.Drawing.Size(124, 17)
        Me.chkExcludeInvestments.TabIndex = 23
        Me.chkExcludeInvestments.Text = "Exclude Investments"
        Me.chkExcludeInvestments.UseVisualStyleBackColor = True
        '
        'chkExcludeCash
        '
        Me.chkExcludeCash.AutoSize = True
        Me.chkExcludeCash.Location = New System.Drawing.Point(226, 60)
        Me.chkExcludeCash.Name = "chkExcludeCash"
        Me.chkExcludeCash.Size = New System.Drawing.Size(91, 17)
        Me.chkExcludeCash.TabIndex = 22
        Me.chkExcludeCash.Text = "Exclude Cash"
        Me.chkExcludeCash.UseVisualStyleBackColor = True
        '
        'lblGroupFilters
        '
        Me.lblGroupFilters.AutoSize = True
        Me.lblGroupFilters.Location = New System.Drawing.Point(334, 37)
        Me.lblGroupFilters.Name = "lblGroupFilters"
        Me.lblGroupFilters.Size = New System.Drawing.Size(93, 13)
        Me.lblGroupFilters.TabIndex = 21
        Me.lblGroupFilters.Text = "Group Filter: None"
        '
        'lblOwnerFilters
        '
        Me.lblOwnerFilters.AutoSize = True
        Me.lblOwnerFilters.Location = New System.Drawing.Point(334, 9)
        Me.lblOwnerFilters.Name = "lblOwnerFilters"
        Me.lblOwnerFilters.Size = New System.Drawing.Size(95, 13)
        Me.lblOwnerFilters.TabIndex = 20
        Me.lblOwnerFilters.Text = "Owner Filter: None"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(113, 34)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(215, 20)
        Me.txtSearch.TabIndex = 19
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Search:"
        '
        'tabFilters
        '
        Me.tabFilters.Controls.Add(Me.btnSelectCorp)
        Me.tabFilters.Controls.Add(Me.btnSelectPersonal)
        Me.tabFilters.Controls.Add(Me.btnAddAllOwners)
        Me.tabFilters.Controls.Add(Me.btnClearAllOwners)
        Me.tabFilters.Controls.Add(Me.btnClearGroupFilters)
        Me.tabFilters.Controls.Add(Me.lvwCharFilter)
        Me.tabFilters.Controls.Add(Me.lblCharFilter)
        Me.tabFilters.Controls.Add(Me.lstFilters)
        Me.tabFilters.Controls.Add(Me.lblSelectedFilters)
        Me.tabFilters.Controls.Add(Me.lblGroupFilter)
        Me.tabFilters.Controls.Add(Me.tvwFilter)
        Me.tabFilters.Location = New System.Drawing.Point(4, 22)
        Me.tabFilters.Name = "tabFilters"
        Me.tabFilters.Size = New System.Drawing.Size(1136, 541)
        Me.tabFilters.TabIndex = 2
        Me.tabFilters.Text = "Filters"
        Me.tabFilters.UseVisualStyleBackColor = True
        '
        'btnSelectCorp
        '
        Me.btnSelectCorp.Location = New System.Drawing.Point(54, 467)
        Me.btnSelectCorp.Name = "btnSelectCorp"
        Me.btnSelectCorp.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectCorp.TabIndex = 21
        Me.btnSelectCorp.Text = "Corporation"
        Me.btnSelectCorp.UseVisualStyleBackColor = True
        '
        'btnSelectPersonal
        '
        Me.btnSelectPersonal.Location = New System.Drawing.Point(135, 467)
        Me.btnSelectPersonal.Name = "btnSelectPersonal"
        Me.btnSelectPersonal.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectPersonal.TabIndex = 20
        Me.btnSelectPersonal.Text = "Personal"
        Me.btnSelectPersonal.UseVisualStyleBackColor = True
        '
        'btnAddAllOwners
        '
        Me.btnAddAllOwners.Location = New System.Drawing.Point(54, 496)
        Me.btnAddAllOwners.Name = "btnAddAllOwners"
        Me.btnAddAllOwners.Size = New System.Drawing.Size(75, 23)
        Me.btnAddAllOwners.TabIndex = 19
        Me.btnAddAllOwners.Text = "Check All"
        Me.btnAddAllOwners.UseVisualStyleBackColor = True
        '
        'btnClearAllOwners
        '
        Me.btnClearAllOwners.Location = New System.Drawing.Point(135, 496)
        Me.btnClearAllOwners.Name = "btnClearAllOwners"
        Me.btnClearAllOwners.Size = New System.Drawing.Size(75, 23)
        Me.btnClearAllOwners.TabIndex = 18
        Me.btnClearAllOwners.Text = "Clear All"
        Me.btnClearAllOwners.UseVisualStyleBackColor = True
        '
        'btnClearGroupFilters
        '
        Me.btnClearGroupFilters.Location = New System.Drawing.Point(496, 496)
        Me.btnClearGroupFilters.Name = "btnClearGroupFilters"
        Me.btnClearGroupFilters.Size = New System.Drawing.Size(75, 23)
        Me.btnClearGroupFilters.TabIndex = 17
        Me.btnClearGroupFilters.Text = "Clear All"
        Me.btnClearGroupFilters.UseVisualStyleBackColor = True
        '
        'lvwCharFilter
        '
        Me.lvwCharFilter.CheckBoxes = True
        Me.lvwCharFilter.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colOwnerName})
        ListViewGroup5.Header = "Corporation"
        ListViewGroup5.Name = "grpCorporation"
        ListViewGroup6.Header = "Personal"
        ListViewGroup6.Name = "grpPersonal"
        Me.lvwCharFilter.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup5, ListViewGroup6})
        Me.lvwCharFilter.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwCharFilter.Location = New System.Drawing.Point(32, 31)
        Me.lvwCharFilter.Name = "lvwCharFilter"
        Me.lvwCharFilter.Size = New System.Drawing.Size(198, 430)
        Me.lvwCharFilter.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwCharFilter.TabIndex = 16
        Me.lvwCharFilter.UseCompatibleStateImageBehavior = False
        Me.lvwCharFilter.View = System.Windows.Forms.View.Details
        '
        'colOwnerName
        '
        Me.colOwnerName.Text = "Owner Name"
        Me.colOwnerName.Width = 150
        '
        'lblCharFilter
        '
        Me.lblCharFilter.AutoSize = True
        Me.lblCharFilter.Location = New System.Drawing.Point(29, 15)
        Me.lblCharFilter.Name = "lblCharFilter"
        Me.lblCharFilter.Size = New System.Drawing.Size(66, 13)
        Me.lblCharFilter.TabIndex = 15
        Me.lblCharFilter.Text = "Owner Filter:"
        '
        'tabAssetsAPI
        '
        Me.tabAssetsAPI.Controls.Add(Me.lblCurrentAPI)
        Me.tabAssetsAPI.Controls.Add(Me.lvwCurrentAPIs)
        Me.tabAssetsAPI.Location = New System.Drawing.Point(4, 22)
        Me.tabAssetsAPI.Name = "tabAssetsAPI"
        Me.tabAssetsAPI.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAssetsAPI.Size = New System.Drawing.Size(1136, 541)
        Me.tabAssetsAPI.TabIndex = 1
        Me.tabAssetsAPI.Text = "API Status"
        Me.tabAssetsAPI.UseVisualStyleBackColor = True
        '
        'lblCurrentAPI
        '
        Me.lblCurrentAPI.AutoSize = True
        Me.lblCurrentAPI.Location = New System.Drawing.Point(8, 9)
        Me.lblCurrentAPI.Name = "lblCurrentAPI"
        Me.lblCurrentAPI.Size = New System.Drawing.Size(81, 13)
        Me.lblCurrentAPI.TabIndex = 1
        Me.lblCurrentAPI.Text = "Currently Using:"
        '
        'lvwCurrentAPIs
        '
        Me.lvwCurrentAPIs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwCurrentAPIs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAPIOwner, Me.colOwnerType, Me.colAssetsAPI, Me.colBalancesAPI, Me.colJobsAPI, Me.colJournalAPI, Me.colOrdersAPI, Me.colTransAPI, Me.colCorpSheetAPI})
        Me.lvwCurrentAPIs.FullRowSelect = True
        Me.lvwCurrentAPIs.GridLines = True
        Me.lvwCurrentAPIs.Location = New System.Drawing.Point(6, 25)
        Me.lvwCurrentAPIs.Name = "lvwCurrentAPIs"
        Me.lvwCurrentAPIs.Size = New System.Drawing.Size(1122, 510)
        Me.lvwCurrentAPIs.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwCurrentAPIs.TabIndex = 0
        Me.lvwCurrentAPIs.UseCompatibleStateImageBehavior = False
        Me.lvwCurrentAPIs.View = System.Windows.Forms.View.Details
        '
        'colAPIOwner
        '
        Me.colAPIOwner.Text = "API Owner"
        Me.colAPIOwner.Width = 150
        '
        'colOwnerType
        '
        Me.colOwnerType.Text = "Owner Type"
        Me.colOwnerType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colOwnerType.Width = 80
        '
        'colAssetsAPI
        '
        Me.colAssetsAPI.Text = "Assets API"
        Me.colAssetsAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colAssetsAPI.Width = 120
        '
        'colBalancesAPI
        '
        Me.colBalancesAPI.Text = "Balances API"
        Me.colBalancesAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colBalancesAPI.Width = 120
        '
        'colJobsAPI
        '
        Me.colJobsAPI.Text = "Jobs API"
        Me.colJobsAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colJobsAPI.Width = 120
        '
        'colJournalAPI
        '
        Me.colJournalAPI.Text = "Journal API"
        Me.colJournalAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colJournalAPI.Width = 120
        '
        'colOrdersAPI
        '
        Me.colOrdersAPI.Text = "Orders API"
        Me.colOrdersAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colOrdersAPI.Width = 120
        '
        'colTransAPI
        '
        Me.colTransAPI.Text = "Transaction API"
        Me.colTransAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colTransAPI.Width = 120
        '
        'colCorpSheetAPI
        '
        Me.colCorpSheetAPI.Text = "Corp Sheet API"
        Me.colCorpSheetAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colCorpSheetAPI.Width = 120
        '
        'tabInvestments
        '
        Me.tabInvestments.Controls.Add(Me.btnReOpenInvestment)
        Me.tabInvestments.Controls.Add(Me.chkViewClosedInvestments)
        Me.tabInvestments.Controls.Add(Me.btnCloseInvestment)
        Me.tabInvestments.Controls.Add(Me.btnEditInvestment)
        Me.tabInvestments.Controls.Add(Me.btnAuditInvestment)
        Me.tabInvestments.Controls.Add(Me.btnEditTransaction)
        Me.tabInvestments.Controls.Add(Me.btnClearTransactions)
        Me.tabInvestments.Controls.Add(Me.lvwTransactions)
        Me.tabInvestments.Controls.Add(Me.btnAddTransaction)
        Me.tabInvestments.Controls.Add(Me.btnClearInvestments)
        Me.tabInvestments.Controls.Add(Me.btnAddInvestment)
        Me.tabInvestments.Controls.Add(Me.lvwInvestments)
        Me.tabInvestments.Location = New System.Drawing.Point(4, 22)
        Me.tabInvestments.Name = "tabInvestments"
        Me.tabInvestments.Size = New System.Drawing.Size(1136, 541)
        Me.tabInvestments.TabIndex = 3
        Me.tabInvestments.Text = "Investments"
        Me.tabInvestments.UseVisualStyleBackColor = True
        '
        'btnReOpenInvestment
        '
        Me.btnReOpenInvestment.Location = New System.Drawing.Point(645, 223)
        Me.btnReOpenInvestment.Name = "btnReOpenInvestment"
        Me.btnReOpenInvestment.Size = New System.Drawing.Size(110, 23)
        Me.btnReOpenInvestment.TabIndex = 13
        Me.btnReOpenInvestment.Text = "Reopen Investment"
        Me.btnReOpenInvestment.UseVisualStyleBackColor = True
        Me.btnReOpenInvestment.Visible = False
        '
        'chkViewClosedInvestments
        '
        Me.chkViewClosedInvestments.AutoSize = True
        Me.chkViewClosedInvestments.Location = New System.Drawing.Point(495, 227)
        Me.chkViewClosedInvestments.Name = "chkViewClosedInvestments"
        Me.chkViewClosedInvestments.Size = New System.Drawing.Size(144, 17)
        Me.chkViewClosedInvestments.TabIndex = 12
        Me.chkViewClosedInvestments.Text = "View Closed Investments"
        Me.chkViewClosedInvestments.UseVisualStyleBackColor = True
        '
        'btnCloseInvestment
        '
        Me.btnCloseInvestment.Location = New System.Drawing.Point(240, 223)
        Me.btnCloseInvestment.Name = "btnCloseInvestment"
        Me.btnCloseInvestment.Size = New System.Drawing.Size(110, 23)
        Me.btnCloseInvestment.TabIndex = 11
        Me.btnCloseInvestment.Text = "Close Investment"
        Me.btnCloseInvestment.UseVisualStyleBackColor = True
        '
        'btnEditInvestment
        '
        Me.btnEditInvestment.Location = New System.Drawing.Point(124, 223)
        Me.btnEditInvestment.Name = "btnEditInvestment"
        Me.btnEditInvestment.Size = New System.Drawing.Size(110, 23)
        Me.btnEditInvestment.TabIndex = 10
        Me.btnEditInvestment.Text = "Edit Investment"
        Me.btnEditInvestment.UseVisualStyleBackColor = True
        '
        'btnAuditInvestment
        '
        Me.btnAuditInvestment.Location = New System.Drawing.Point(356, 223)
        Me.btnAuditInvestment.Name = "btnAuditInvestment"
        Me.btnAuditInvestment.Size = New System.Drawing.Size(110, 23)
        Me.btnAuditInvestment.TabIndex = 9
        Me.btnAuditInvestment.Text = "Audit Investment"
        Me.btnAuditInvestment.UseVisualStyleBackColor = True
        '
        'btnEditTransaction
        '
        Me.btnEditTransaction.Location = New System.Drawing.Point(114, 472)
        Me.btnEditTransaction.Name = "btnEditTransaction"
        Me.btnEditTransaction.Size = New System.Drawing.Size(100, 23)
        Me.btnEditTransaction.TabIndex = 8
        Me.btnEditTransaction.Text = "EditTransaction"
        Me.btnEditTransaction.UseVisualStyleBackColor = True
        '
        'btnClearTransactions
        '
        Me.btnClearTransactions.Location = New System.Drawing.Point(220, 472)
        Me.btnClearTransactions.Name = "btnClearTransactions"
        Me.btnClearTransactions.Size = New System.Drawing.Size(104, 23)
        Me.btnClearTransactions.TabIndex = 7
        Me.btnClearTransactions.Text = "Clear Transactions"
        Me.btnClearTransactions.UseVisualStyleBackColor = True
        Me.btnClearTransactions.Visible = False
        '
        'lvwTransactions
        '
        Me.lvwTransactions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwTransactions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTransID, Me.colTransDate, Me.colTransType, Me.colTransQuantity, Me.colTransValue})
        Me.lvwTransactions.FullRowSelect = True
        Me.lvwTransactions.GridLines = True
        Me.lvwTransactions.HideSelection = False
        Me.lvwTransactions.Location = New System.Drawing.Point(8, 252)
        Me.lvwTransactions.Name = "lvwTransactions"
        Me.lvwTransactions.Size = New System.Drawing.Size(1102, 214)
        Me.lvwTransactions.TabIndex = 6
        Me.lvwTransactions.UseCompatibleStateImageBehavior = False
        Me.lvwTransactions.View = System.Windows.Forms.View.Details
        '
        'colTransID
        '
        Me.colTransID.Text = "ID"
        Me.colTransID.Width = 40
        '
        'colTransDate
        '
        Me.colTransDate.Text = "Date"
        Me.colTransDate.Width = 125
        '
        'colTransType
        '
        Me.colTransType.Text = "Type"
        Me.colTransType.Width = 100
        '
        'colTransQuantity
        '
        Me.colTransQuantity.Text = "Quantity"
        Me.colTransQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colTransQuantity.Width = 100
        '
        'colTransValue
        '
        Me.colTransValue.Text = "Unit Value"
        Me.colTransValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colTransValue.Width = 100
        '
        'btnAddTransaction
        '
        Me.btnAddTransaction.Location = New System.Drawing.Point(8, 472)
        Me.btnAddTransaction.Name = "btnAddTransaction"
        Me.btnAddTransaction.Size = New System.Drawing.Size(100, 23)
        Me.btnAddTransaction.TabIndex = 5
        Me.btnAddTransaction.Text = "Add Transaction"
        Me.btnAddTransaction.UseVisualStyleBackColor = True
        '
        'btnClearInvestments
        '
        Me.btnClearInvestments.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearInvestments.Location = New System.Drawing.Point(1000, 223)
        Me.btnClearInvestments.Name = "btnClearInvestments"
        Me.btnClearInvestments.Size = New System.Drawing.Size(110, 23)
        Me.btnClearInvestments.TabIndex = 2
        Me.btnClearInvestments.Text = "Clear Investments"
        Me.btnClearInvestments.UseVisualStyleBackColor = True
        '
        'btnAddInvestment
        '
        Me.btnAddInvestment.Location = New System.Drawing.Point(8, 223)
        Me.btnAddInvestment.Name = "btnAddInvestment"
        Me.btnAddInvestment.Size = New System.Drawing.Size(110, 23)
        Me.btnAddInvestment.TabIndex = 1
        Me.btnAddInvestment.Text = "Add Investment"
        Me.btnAddInvestment.UseVisualStyleBackColor = True
        '
        'lvwInvestments
        '
        Me.lvwInvestments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwInvestments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colInvID, Me.colInvName, Me.colInvOwner, Me.colInvCQuantity, Me.colInvCCost, Me.colInvCValue, Me.colInvCTotalValue, Me.colInvCPotProfit, Me.colInvCActProfit, Me.colInvCIncome, Me.colInvCYield})
        Me.lvwInvestments.FullRowSelect = True
        Me.lvwInvestments.GridLines = True
        Me.lvwInvestments.HideSelection = False
        Me.lvwInvestments.Location = New System.Drawing.Point(8, 3)
        Me.lvwInvestments.Name = "lvwInvestments"
        Me.lvwInvestments.Size = New System.Drawing.Size(1102, 214)
        Me.lvwInvestments.TabIndex = 0
        Me.lvwInvestments.UseCompatibleStateImageBehavior = False
        Me.lvwInvestments.View = System.Windows.Forms.View.Details
        '
        'colInvID
        '
        Me.colInvID.Text = "ID"
        Me.colInvID.Width = 30
        '
        'colInvName
        '
        Me.colInvName.Text = "Investment Name"
        Me.colInvName.Width = 175
        '
        'colInvOwner
        '
        Me.colInvOwner.Text = "Owner"
        Me.colInvOwner.Width = 150
        '
        'colInvCQuantity
        '
        Me.colInvCQuantity.Text = "Quantity"
        Me.colInvCQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colInvCQuantity.Width = 100
        '
        'colInvCCost
        '
        Me.colInvCCost.Text = "Current Cost"
        Me.colInvCCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colInvCCost.Width = 100
        '
        'colInvCValue
        '
        Me.colInvCValue.Text = "Current Value"
        Me.colInvCValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colInvCValue.Width = 100
        '
        'colInvCTotalValue
        '
        Me.colInvCTotalValue.Text = "Total Value"
        Me.colInvCTotalValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colInvCTotalValue.Width = 100
        '
        'colInvCPotProfit
        '
        Me.colInvCPotProfit.Text = "Potential Profit"
        Me.colInvCPotProfit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colInvCPotProfit.Width = 100
        '
        'colInvCActProfit
        '
        Me.colInvCActProfit.Text = "Gross Profit"
        Me.colInvCActProfit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colInvCActProfit.Width = 100
        '
        'colInvCIncome
        '
        Me.colInvCIncome.Text = "Income"
        Me.colInvCIncome.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colInvCIncome.Width = 100
        '
        'colInvCYield
        '
        Me.colInvCYield.Text = "Yield"
        Me.colInvCYield.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colInvCYield.Width = 100
        '
        'tabRigBuilder
        '
        Me.tabRigBuilder.Controls.Add(Me.lblTotalRigMargin)
        Me.tabRigBuilder.Controls.Add(Me.lblTotalRigProfit)
        Me.tabRigBuilder.Controls.Add(Me.lblTotalRigSalePrice)
        Me.tabRigBuilder.Controls.Add(Me.bgAutoRig)
        Me.tabRigBuilder.Controls.Add(Me.lblRigBuildList)
        Me.tabRigBuilder.Controls.Add(Me.lvwRigBuildList)
        Me.tabRigBuilder.Controls.Add(Me.lvwRigs)
        Me.tabRigBuilder.Controls.Add(Me.nudRigMELevel)
        Me.tabRigBuilder.Controls.Add(Me.lblRigMELevel)
        Me.tabRigBuilder.Controls.Add(Me.btnBuildRigs)
        Me.tabRigBuilder.Controls.Add(Me.lblRigOwnerFilter)
        Me.tabRigBuilder.Location = New System.Drawing.Point(4, 22)
        Me.tabRigBuilder.Name = "tabRigBuilder"
        Me.tabRigBuilder.Size = New System.Drawing.Size(1136, 541)
        Me.tabRigBuilder.TabIndex = 4
        Me.tabRigBuilder.Text = "Rig Builder"
        Me.tabRigBuilder.UseVisualStyleBackColor = True
        '
        'lblTotalRigMargin
        '
        Me.lblTotalRigMargin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigMargin.AutoSize = True
        Me.lblTotalRigMargin.Location = New System.Drawing.Point(553, 311)
        Me.lblTotalRigMargin.Name = "lblTotalRigMargin"
        Me.lblTotalRigMargin.Size = New System.Drawing.Size(42, 13)
        Me.lblTotalRigMargin.TabIndex = 33
        Me.lblTotalRigMargin.Text = "Margin:"
        '
        'lblTotalRigProfit
        '
        Me.lblTotalRigProfit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigProfit.AutoSize = True
        Me.lblTotalRigProfit.Location = New System.Drawing.Point(350, 311)
        Me.lblTotalRigProfit.Name = "lblTotalRigProfit"
        Me.lblTotalRigProfit.Size = New System.Drawing.Size(61, 13)
        Me.lblTotalRigProfit.TabIndex = 32
        Me.lblTotalRigProfit.Text = "Total Profit:"
        '
        'lblTotalRigSalePrice
        '
        Me.lblTotalRigSalePrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigSalePrice.AutoSize = True
        Me.lblTotalRigSalePrice.Location = New System.Drawing.Point(138, 311)
        Me.lblTotalRigSalePrice.Name = "lblTotalRigSalePrice"
        Me.lblTotalRigSalePrice.Size = New System.Drawing.Size(85, 13)
        Me.lblTotalRigSalePrice.TabIndex = 31
        Me.lblTotalRigSalePrice.Text = "Total Sale Price:"
        '
        'bgAutoRig
        '
        Me.bgAutoRig.Controls.Add(Me.radTotalProfit)
        Me.bgAutoRig.Controls.Add(Me.radTotalSalePrice)
        Me.bgAutoRig.Controls.Add(Me.radRigMargin)
        Me.bgAutoRig.Controls.Add(Me.radRigProfit)
        Me.bgAutoRig.Controls.Add(Me.radRigSaleprice)
        Me.bgAutoRig.Controls.Add(Me.lblAutoRigCriteria)
        Me.bgAutoRig.Controls.Add(Me.btnAutoRig)
        Me.bgAutoRig.Location = New System.Drawing.Point(216, 39)
        Me.bgAutoRig.Name = "bgAutoRig"
        Me.bgAutoRig.Size = New System.Drawing.Size(469, 61)
        Me.bgAutoRig.TabIndex = 30
        Me.bgAutoRig.TabStop = False
        Me.bgAutoRig.Text = "Automatic Rig Availability Options"
        '
        'radTotalProfit
        '
        Me.radTotalProfit.AutoSize = True
        Me.radTotalProfit.Location = New System.Drawing.Point(161, 36)
        Me.radTotalProfit.Name = "radTotalProfit"
        Me.radTotalProfit.Size = New System.Drawing.Size(76, 17)
        Me.radTotalProfit.TabIndex = 35
        Me.radTotalProfit.Text = "Total Profit"
        Me.radTotalProfit.UseVisualStyleBackColor = True
        '
        'radTotalSalePrice
        '
        Me.radTotalSalePrice.AutoSize = True
        Me.radTotalSalePrice.Location = New System.Drawing.Point(55, 36)
        Me.radTotalSalePrice.Name = "radTotalSalePrice"
        Me.radTotalSalePrice.Size = New System.Drawing.Size(100, 17)
        Me.radTotalSalePrice.TabIndex = 34
        Me.radTotalSalePrice.Text = "Total Sale Price"
        Me.radTotalSalePrice.UseVisualStyleBackColor = True
        '
        'radRigMargin
        '
        Me.radRigMargin.AutoSize = True
        Me.radRigMargin.Checked = True
        Me.radRigMargin.Location = New System.Drawing.Point(235, 18)
        Me.radRigMargin.Name = "radRigMargin"
        Me.radRigMargin.Size = New System.Drawing.Size(76, 17)
        Me.radRigMargin.TabIndex = 33
        Me.radRigMargin.TabStop = True
        Me.radRigMargin.Text = "Rig Margin"
        Me.radRigMargin.UseVisualStyleBackColor = True
        '
        'radRigProfit
        '
        Me.radRigProfit.AutoSize = True
        Me.radRigProfit.Location = New System.Drawing.Point(161, 18)
        Me.radRigProfit.Name = "radRigProfit"
        Me.radRigProfit.Size = New System.Drawing.Size(68, 17)
        Me.radRigProfit.TabIndex = 32
        Me.radRigProfit.Text = "Rig Profit"
        Me.radRigProfit.UseVisualStyleBackColor = True
        '
        'radRigSaleprice
        '
        Me.radRigSaleprice.AutoSize = True
        Me.radRigSaleprice.Location = New System.Drawing.Point(55, 18)
        Me.radRigSaleprice.Name = "radRigSaleprice"
        Me.radRigSaleprice.Size = New System.Drawing.Size(92, 17)
        Me.radRigSaleprice.TabIndex = 31
        Me.radRigSaleprice.Text = "Rig Sale Price"
        Me.radRigSaleprice.UseVisualStyleBackColor = True
        '
        'lblAutoRigCriteria
        '
        Me.lblAutoRigCriteria.AutoSize = True
        Me.lblAutoRigCriteria.Location = New System.Drawing.Point(7, 20)
        Me.lblAutoRigCriteria.Name = "lblAutoRigCriteria"
        Me.lblAutoRigCriteria.Size = New System.Drawing.Size(42, 13)
        Me.lblAutoRigCriteria.TabIndex = 30
        Me.lblAutoRigCriteria.Text = "Criteria:"
        '
        'btnAutoRig
        '
        Me.btnAutoRig.Location = New System.Drawing.Point(334, 15)
        Me.btnAutoRig.Name = "btnAutoRig"
        Me.btnAutoRig.Size = New System.Drawing.Size(125, 23)
        Me.btnAutoRig.TabIndex = 29
        Me.btnAutoRig.Text = "Auto Rig Availability"
        Me.btnAutoRig.UseVisualStyleBackColor = True
        '
        'lblRigBuildList
        '
        Me.lblRigBuildList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRigBuildList.AutoSize = True
        Me.lblRigBuildList.Location = New System.Drawing.Point(5, 311)
        Me.lblRigBuildList.Name = "lblRigBuildList"
        Me.lblRigBuildList.Size = New System.Drawing.Size(71, 13)
        Me.lblRigBuildList.TabIndex = 28
        Me.lblRigBuildList.Text = "Rig Build List:"
        '
        'lvwRigBuildList
        '
        Me.lvwRigBuildList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwRigBuildList.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.ContainerListViewColumnHeader9, Me.ContainerListViewColumnHeader10, Me.ContainerListViewColumnHeader11, Me.ContainerListViewColumnHeader12, Me.ContainerListViewColumnHeader13, Me.ContainerListViewColumnHeader14, Me.ContainerListViewColumnHeader15, Me.ContainerListViewColumnHeader16, Me.ContainerListViewColumnHeader17})
        Me.lvwRigBuildList.ColumnSortColor = System.Drawing.Color.AliceBlue
        Me.lvwRigBuildList.ColumnTracking = True
        Me.lvwRigBuildList.ColumnTrackingColor = System.Drawing.Color.LightCyan
        Me.lvwRigBuildList.DefaultItemHeight = 20
        Me.lvwRigBuildList.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRigBuildList.ItemContextMenu = Me.ctxAssets
        Me.lvwRigBuildList.ItemSelectedColor = System.Drawing.Color.LimeGreen
        Me.lvwRigBuildList.ItemTracking = True
        Me.lvwRigBuildList.ItemTrackingColor = System.Drawing.Color.PaleGreen
        Me.lvwRigBuildList.Location = New System.Drawing.Point(8, 327)
        Me.lvwRigBuildList.MultipleColumnSort = True
        Me.lvwRigBuildList.Name = "lvwRigBuildList"
        Me.lvwRigBuildList.Size = New System.Drawing.Size(1102, 211)
        Me.lvwRigBuildList.TabIndex = 27
        '
        'ContainerListViewColumnHeader9
        '
        Me.ContainerListViewColumnHeader9.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader9.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.ContainerListViewColumnHeader9.Tag = Nothing
        Me.ContainerListViewColumnHeader9.Text = "Rig Type"
        Me.ContainerListViewColumnHeader9.Width = 200
        Me.ContainerListViewColumnHeader9.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'ContainerListViewColumnHeader10
        '
        Me.ContainerListViewColumnHeader10.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader10.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader10.DisplayIndex = 1
        Me.ContainerListViewColumnHeader10.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader10.Tag = Nothing
        Me.ContainerListViewColumnHeader10.Text = "Quantity"
        Me.ContainerListViewColumnHeader10.Width = 100
        '
        'ContainerListViewColumnHeader11
        '
        Me.ContainerListViewColumnHeader11.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader11.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader11.DisplayIndex = 2
        Me.ContainerListViewColumnHeader11.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader11.Tag = Nothing
        Me.ContainerListViewColumnHeader11.Text = "Rig Market Price"
        Me.ContainerListViewColumnHeader11.Width = 120
        '
        'ContainerListViewColumnHeader12
        '
        Me.ContainerListViewColumnHeader12.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader12.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader12.DisplayIndex = 3
        Me.ContainerListViewColumnHeader12.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader12.Tag = Nothing
        Me.ContainerListViewColumnHeader12.Text = "Salv. Market Price"
        Me.ContainerListViewColumnHeader12.Width = 120
        '
        'ContainerListViewColumnHeader13
        '
        Me.ContainerListViewColumnHeader13.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader13.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader13.DisplayIndex = 4
        Me.ContainerListViewColumnHeader13.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader13.Tag = Nothing
        Me.ContainerListViewColumnHeader13.Text = "Build Benefit"
        Me.ContainerListViewColumnHeader13.Width = 120
        '
        'ContainerListViewColumnHeader14
        '
        Me.ContainerListViewColumnHeader14.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader14.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader14.DisplayIndex = 5
        Me.ContainerListViewColumnHeader14.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader14.Tag = Nothing
        Me.ContainerListViewColumnHeader14.Text = "Total Rig Value"
        Me.ContainerListViewColumnHeader14.Width = 120
        Me.ContainerListViewColumnHeader14.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'ContainerListViewColumnHeader15
        '
        Me.ContainerListViewColumnHeader15.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader15.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader15.DisplayIndex = 6
        Me.ContainerListViewColumnHeader15.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader15.Tag = Nothing
        Me.ContainerListViewColumnHeader15.Text = "Total Salv. Value"
        Me.ContainerListViewColumnHeader15.Width = 120
        '
        'ContainerListViewColumnHeader16
        '
        Me.ContainerListViewColumnHeader16.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader16.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader16.DisplayIndex = 7
        Me.ContainerListViewColumnHeader16.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader16.Tag = Nothing
        Me.ContainerListViewColumnHeader16.Text = "Total Build Benefit"
        Me.ContainerListViewColumnHeader16.Width = 120
        '
        'ContainerListViewColumnHeader17
        '
        Me.ContainerListViewColumnHeader17.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader17.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader17.DisplayIndex = 8
        Me.ContainerListViewColumnHeader17.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader17.Tag = Nothing
        Me.ContainerListViewColumnHeader17.Text = "% Margin"
        Me.ContainerListViewColumnHeader17.Width = 100
        '
        'lvwRigs
        '
        Me.lvwRigs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwRigs.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colRigType, Me.colRigQuantity, Me.colRigMarketPrice, Me.colSalvageMarketPrice, Me.colBuildBenefit, Me.colTotalRigValue, Me.colTotalSalvageValue, Me.colTotalBuildBenefit, Me.colMargin})
        Me.lvwRigs.ColumnSortColor = System.Drawing.Color.AliceBlue
        Me.lvwRigs.ColumnTracking = True
        Me.lvwRigs.ColumnTrackingColor = System.Drawing.Color.LightCyan
        Me.lvwRigs.DefaultItemHeight = 20
        Me.lvwRigs.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRigs.ItemContextMenu = Me.ctxAssets
        Me.lvwRigs.ItemSelectedColor = System.Drawing.Color.LimeGreen
        Me.lvwRigs.ItemTracking = True
        Me.lvwRigs.ItemTrackingColor = System.Drawing.Color.PaleGreen
        Me.lvwRigs.Location = New System.Drawing.Point(8, 106)
        Me.lvwRigs.MultipleColumnSort = True
        Me.lvwRigs.Name = "lvwRigs"
        Me.lvwRigs.Size = New System.Drawing.Size(1102, 193)
        Me.lvwRigs.TabIndex = 26
        '
        'colRigType
        '
        Me.colRigType.CustomSortTag = Nothing
        Me.colRigType.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colRigType.Tag = Nothing
        Me.colRigType.Text = "Rig Type"
        Me.colRigType.Width = 200
        Me.colRigType.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'colRigQuantity
        '
        Me.colRigQuantity.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colRigQuantity.CustomSortTag = Nothing
        Me.colRigQuantity.DisplayIndex = 1
        Me.colRigQuantity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colRigQuantity.Tag = Nothing
        Me.colRigQuantity.Text = "Quantity"
        Me.colRigQuantity.Width = 100
        '
        'colRigMarketPrice
        '
        Me.colRigMarketPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colRigMarketPrice.CustomSortTag = Nothing
        Me.colRigMarketPrice.DisplayIndex = 2
        Me.colRigMarketPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colRigMarketPrice.Tag = Nothing
        Me.colRigMarketPrice.Text = "Rig Market Price"
        Me.colRigMarketPrice.Width = 120
        '
        'colSalvageMarketPrice
        '
        Me.colSalvageMarketPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colSalvageMarketPrice.CustomSortTag = Nothing
        Me.colSalvageMarketPrice.DisplayIndex = 3
        Me.colSalvageMarketPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colSalvageMarketPrice.Tag = Nothing
        Me.colSalvageMarketPrice.Text = "Salv. Market Price"
        Me.colSalvageMarketPrice.Width = 120
        '
        'colBuildBenefit
        '
        Me.colBuildBenefit.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBuildBenefit.CustomSortTag = Nothing
        Me.colBuildBenefit.DisplayIndex = 4
        Me.colBuildBenefit.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBuildBenefit.Tag = Nothing
        Me.colBuildBenefit.Text = "Build Benefit"
        Me.colBuildBenefit.Width = 120
        '
        'colTotalRigValue
        '
        Me.colTotalRigValue.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colTotalRigValue.CustomSortTag = Nothing
        Me.colTotalRigValue.DisplayIndex = 5
        Me.colTotalRigValue.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colTotalRigValue.Tag = Nothing
        Me.colTotalRigValue.Text = "Total Rig Value"
        Me.colTotalRigValue.Width = 120
        Me.colTotalRigValue.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'colTotalSalvageValue
        '
        Me.colTotalSalvageValue.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colTotalSalvageValue.CustomSortTag = Nothing
        Me.colTotalSalvageValue.DisplayIndex = 6
        Me.colTotalSalvageValue.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colTotalSalvageValue.Tag = Nothing
        Me.colTotalSalvageValue.Text = "Total Salv. Value"
        Me.colTotalSalvageValue.Width = 120
        '
        'colTotalBuildBenefit
        '
        Me.colTotalBuildBenefit.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colTotalBuildBenefit.CustomSortTag = Nothing
        Me.colTotalBuildBenefit.DisplayIndex = 7
        Me.colTotalBuildBenefit.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colTotalBuildBenefit.Tag = Nothing
        Me.colTotalBuildBenefit.Text = "Total Build Benefit"
        Me.colTotalBuildBenefit.Width = 120
        '
        'colMargin
        '
        Me.colMargin.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colMargin.CustomSortTag = Nothing
        Me.colMargin.DisplayIndex = 8
        Me.colMargin.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colMargin.Tag = Nothing
        Me.colMargin.Text = "% Margin"
        Me.colMargin.Width = 100
        '
        'nudRigMELevel
        '
        Me.nudRigMELevel.Location = New System.Drawing.Point(85, 45)
        Me.nudRigMELevel.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudRigMELevel.Name = "nudRigMELevel"
        Me.nudRigMELevel.Size = New System.Drawing.Size(70, 20)
        Me.nudRigMELevel.TabIndex = 25
        '
        'lblRigMELevel
        '
        Me.lblRigMELevel.AutoSize = True
        Me.lblRigMELevel.Location = New System.Drawing.Point(5, 47)
        Me.lblRigMELevel.Name = "lblRigMELevel"
        Me.lblRigMELevel.Size = New System.Drawing.Size(74, 13)
        Me.lblRigMELevel.TabIndex = 24
        Me.lblRigMELevel.Text = "Rig ME Level:"
        '
        'btnBuildRigs
        '
        Me.btnBuildRigs.Location = New System.Drawing.Point(8, 71)
        Me.btnBuildRigs.Name = "btnBuildRigs"
        Me.btnBuildRigs.Size = New System.Drawing.Size(125, 23)
        Me.btnBuildRigs.TabIndex = 23
        Me.btnBuildRigs.Text = "Display Rig Availability"
        Me.btnBuildRigs.UseVisualStyleBackColor = True
        '
        'lblRigOwnerFilter
        '
        Me.lblRigOwnerFilter.AutoSize = True
        Me.lblRigOwnerFilter.Location = New System.Drawing.Point(5, 20)
        Me.lblRigOwnerFilter.Name = "lblRigOwnerFilter"
        Me.lblRigOwnerFilter.Size = New System.Drawing.Size(95, 13)
        Me.lblRigOwnerFilter.TabIndex = 21
        Me.lblRigOwnerFilter.Text = "Owner Filter: None"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tssLabelTotalAssetsLabel, Me.tssLabelTotalAssets, Me.tssLabelSelectedAssetsLabel, Me.tssLabelSelectedAssets})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 598)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1144, 22)
        Me.StatusStrip1.TabIndex = 15
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tssLabelTotalAssetsLabel
        '
        Me.tssLabelTotalAssetsLabel.Name = "tssLabelTotalAssetsLabel"
        Me.tssLabelTotalAssetsLabel.Size = New System.Drawing.Size(143, 17)
        Me.tssLabelTotalAssetsLabel.Text = "Total Displayed Asset Value:"
        '
        'tssLabelTotalAssets
        '
        Me.tssLabelTotalAssets.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tssLabelTotalAssets.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.tssLabelTotalAssets.Name = "tssLabelTotalAssets"
        Me.tssLabelTotalAssets.Size = New System.Drawing.Size(4, 17)
        '
        'tssLabelSelectedAssetsLabel
        '
        Me.tssLabelSelectedAssetsLabel.Name = "tssLabelSelectedAssetsLabel"
        Me.tssLabelSelectedAssetsLabel.Size = New System.Drawing.Size(153, 17)
        Me.tssLabelSelectedAssetsLabel.Text = "     Total Selected Asset Value:"
        '
        'tssLabelSelectedAssets
        '
        Me.tssLabelSelectedAssets.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tssLabelSelectedAssets.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.tssLabelSelectedAssets.Name = "tssLabelSelectedAssets"
        Me.tssLabelSelectedAssets.Size = New System.Drawing.Size(4, 17)
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbDownloadData, Me.ToolStripSeparator1, Me.tsbDownloadOutposts, Me.ToolStripSeparator3, Me.tsbRefreshAssets, Me.ToolStripSeparator2, Me.tsbReports})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1144, 25)
        Me.ToolStrip1.TabIndex = 16
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbDownloadData
        '
        Me.tsbDownloadData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbDownloadData.Image = CType(resources.GetObject("tsbDownloadData.Image"), System.Drawing.Image)
        Me.tsbDownloadData.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDownloadData.Name = "tsbDownloadData"
        Me.tsbDownloadData.Size = New System.Drawing.Size(104, 22)
        Me.tsbDownloadData.Text = "Download API Data"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsbDownloadOutposts
        '
        Me.tsbDownloadOutposts.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbDownloadOutposts.Image = CType(resources.GetObject("tsbDownloadOutposts.Image"), System.Drawing.Image)
        Me.tsbDownloadOutposts.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDownloadOutposts.Name = "tsbDownloadOutposts"
        Me.tsbDownloadOutposts.Size = New System.Drawing.Size(105, 22)
        Me.tsbDownloadOutposts.Text = "Download Outposts"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'tsbRefreshAssets
        '
        Me.tsbRefreshAssets.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbRefreshAssets.Image = CType(resources.GetObject("tsbRefreshAssets.Image"), System.Drawing.Image)
        Me.tsbRefreshAssets.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRefreshAssets.Name = "tsbRefreshAssets"
        Me.tsbRefreshAssets.Size = New System.Drawing.Size(68, 22)
        Me.tsbRefreshAssets.Text = "View Assets"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsbReports
        '
        Me.tsbReports.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLocation, Me.mnuAssetLists})
        Me.tsbReports.Image = CType(resources.GetObject("tsbReports.Image"), System.Drawing.Image)
        Me.tsbReports.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReports.Name = "tsbReports"
        Me.tsbReports.Size = New System.Drawing.Size(61, 22)
        Me.tsbReports.Text = "Reports"
        '
        'mnuLocation
        '
        Me.mnuLocation.Name = "mnuLocation"
        Me.mnuLocation.Size = New System.Drawing.Size(184, 22)
        Me.mnuLocation.Text = "Grouped by Location"
        '
        'mnuAssetLists
        '
        Me.mnuAssetLists.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAssetListName, Me.mnuAssetListQuantity, Me.mnuAssetListPrice, Me.mnuAssetListValue})
        Me.mnuAssetLists.Name = "mnuAssetLists"
        Me.mnuAssetLists.Size = New System.Drawing.Size(184, 22)
        Me.mnuAssetLists.Text = "Asset Lists"
        '
        'mnuAssetListName
        '
        Me.mnuAssetListName.Name = "mnuAssetListName"
        Me.mnuAssetListName.Size = New System.Drawing.Size(195, 22)
        Me.mnuAssetListName.Text = "Asset List (Name)"
        '
        'mnuAssetListQuantity
        '
        Me.mnuAssetListQuantity.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAssetListQuantityA, Me.mnuAssetListQuantityD})
        Me.mnuAssetListQuantity.Name = "mnuAssetListQuantity"
        Me.mnuAssetListQuantity.Size = New System.Drawing.Size(195, 22)
        Me.mnuAssetListQuantity.Text = "Asset List (Quantity)"
        '
        'mnuAssetListQuantityA
        '
        Me.mnuAssetListQuantityA.Name = "mnuAssetListQuantityA"
        Me.mnuAssetListQuantityA.Size = New System.Drawing.Size(140, 22)
        Me.mnuAssetListQuantityA.Text = "Ascending"
        '
        'mnuAssetListQuantityD
        '
        Me.mnuAssetListQuantityD.Name = "mnuAssetListQuantityD"
        Me.mnuAssetListQuantityD.Size = New System.Drawing.Size(140, 22)
        Me.mnuAssetListQuantityD.Text = "Descending"
        '
        'mnuAssetListPrice
        '
        Me.mnuAssetListPrice.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAssetListPriceA, Me.mnuAssetListPriceD})
        Me.mnuAssetListPrice.Name = "mnuAssetListPrice"
        Me.mnuAssetListPrice.Size = New System.Drawing.Size(195, 22)
        Me.mnuAssetListPrice.Text = "Asset List (Unit Price)"
        '
        'mnuAssetListPriceA
        '
        Me.mnuAssetListPriceA.Name = "mnuAssetListPriceA"
        Me.mnuAssetListPriceA.Size = New System.Drawing.Size(140, 22)
        Me.mnuAssetListPriceA.Text = "Ascending"
        '
        'mnuAssetListPriceD
        '
        Me.mnuAssetListPriceD.Name = "mnuAssetListPriceD"
        Me.mnuAssetListPriceD.Size = New System.Drawing.Size(140, 22)
        Me.mnuAssetListPriceD.Text = "Descending"
        '
        'mnuAssetListValue
        '
        Me.mnuAssetListValue.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAssetListValueA, Me.mnuAssetListValueD})
        Me.mnuAssetListValue.Name = "mnuAssetListValue"
        Me.mnuAssetListValue.Size = New System.Drawing.Size(195, 22)
        Me.mnuAssetListValue.Text = "Asset List (Total Value)"
        '
        'mnuAssetListValueA
        '
        Me.mnuAssetListValueA.Name = "mnuAssetListValueA"
        Me.mnuAssetListValueA.Size = New System.Drawing.Size(140, 22)
        Me.mnuAssetListValueA.Text = "Ascending"
        '
        'mnuAssetListValueD
        '
        Me.mnuAssetListValueD.Name = "mnuAssetListValueD"
        Me.mnuAssetListValueD.Size = New System.Drawing.Size(140, 22)
        Me.mnuAssetListValueD.Text = "Descending"
        '
        'frmPrism
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1144, 620)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.tabPrism)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPrism"
        Me.Text = "Prism"
        Me.ctxAssets.ResumeLayout(False)
        Me.ctxFilter.ResumeLayout(False)
        Me.ctxFilterList.ResumeLayout(False)
        Me.tabPrism.ResumeLayout(False)
        Me.tabAssets.ResumeLayout(False)
        Me.tabAssets.PerformLayout()
        Me.tabFilters.ResumeLayout(False)
        Me.tabFilters.PerformLayout()
        Me.tabAssetsAPI.ResumeLayout(False)
        Me.tabAssetsAPI.PerformLayout()
        Me.tabInvestments.ResumeLayout(False)
        Me.tabInvestments.PerformLayout()
        Me.tabRigBuilder.ResumeLayout(False)
        Me.tabRigBuilder.PerformLayout()
        Me.bgAutoRig.ResumeLayout(False)
        Me.bgAutoRig.PerformLayout()
        CType(Me.nudRigMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSelectChar As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents tlvAssets As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colItem As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colLocation As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colValue As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents chkExcludeBPs As System.Windows.Forms.CheckBox
    Friend WithEvents colGroup As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colCategory As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents tvwFilter As System.Windows.Forms.TreeView
    Friend WithEvents lblGroupFilter As System.Windows.Forms.Label
    Friend WithEvents lstFilters As System.Windows.Forms.ListBox
    Friend WithEvents ctxFilter As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddToFilterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxFilterList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveFilterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblSelectedFilters As System.Windows.Forms.Label
    Friend WithEvents tabPrism As System.Windows.Forms.TabControl
    Friend WithEvents tabAssets As System.Windows.Forms.TabPage
    Friend WithEvents tabAssetsAPI As System.Windows.Forms.TabPage
    Friend WithEvents tabFilters As System.Windows.Forms.TabPage
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbRefreshAssets As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblCurrentAPI As System.Windows.Forms.Label
    Friend WithEvents lvwCurrentAPIs As System.Windows.Forms.ListView
    Friend WithEvents colAPIOwner As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAssetsAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBalancesAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents tsbDownloadData As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblCharFilter As System.Windows.Forms.Label
    Friend WithEvents lvwCharFilter As System.Windows.Forms.ListView
    Friend WithEvents colOwner As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents colOwnerName As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClearGroupFilters As System.Windows.Forms.Button
    Friend WithEvents btnSelectCorp As System.Windows.Forms.Button
    Friend WithEvents btnSelectPersonal As System.Windows.Forms.Button
    Friend WithEvents btnAddAllOwners As System.Windows.Forms.Button
    Friend WithEvents btnClearAllOwners As System.Windows.Forms.Button
    Friend WithEvents ctxAssets As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewInIB As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewInHQF As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tssLabelTotalAssetsLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssLabelTotalAssets As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents lblOwnerFilters As System.Windows.Forms.Label
    Friend WithEvents lblGroupFilters As System.Windows.Forms.Label
    Friend WithEvents tsbReports As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents mnuLocation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAssetLists As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListQuantity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListQuantityA As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListQuantityD As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListPrice As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListPriceA As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListPriceD As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListValue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListValueA As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAssetListValueD As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbDownloadOutposts As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tabInvestments As System.Windows.Forms.TabPage
    Friend WithEvents lvwInvestments As System.Windows.Forms.ListView
    Friend WithEvents colInvName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInvOwner As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInvCQuantity As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInvCCost As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInvCValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInvCPotProfit As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInvCActProfit As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInvID As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnAddInvestment As System.Windows.Forms.Button
    Friend WithEvents btnClearInvestments As System.Windows.Forms.Button
    Friend WithEvents btnAddTransaction As System.Windows.Forms.Button
    Friend WithEvents lvwTransactions As System.Windows.Forms.ListView
    Friend WithEvents colTransID As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTransDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTransType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTransQuantity As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTransValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClearTransactions As System.Windows.Forms.Button
    Friend WithEvents colInvCIncome As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInvCYield As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnEditTransaction As System.Windows.Forms.Button
    Friend WithEvents btnAuditInvestment As System.Windows.Forms.Button
    Friend WithEvents mnuModifyPrice As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents colOwnerType As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkExcludeCash As System.Windows.Forms.CheckBox
    Friend WithEvents chkExcludeInvestments As System.Windows.Forms.CheckBox
    Friend WithEvents chkExcludeItems As System.Windows.Forms.CheckBox
    Friend WithEvents btnEditInvestment As System.Windows.Forms.Button
    Friend WithEvents colInvCTotalValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabRigBuilder As System.Windows.Forms.TabPage
    Friend WithEvents lblRigOwnerFilter As System.Windows.Forms.Label
    Friend WithEvents btnBuildRigs As System.Windows.Forms.Button
    Friend WithEvents nudRigMELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblRigMELevel As System.Windows.Forms.Label
    Friend WithEvents lvwRigs As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colRigType As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRigQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRigMarketPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSalvageMarketPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBuildBenefit As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colTotalRigValue As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colTotalSalvageValue As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colTotalBuildBenefit As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colMargin As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnCloseInvestment As System.Windows.Forms.Button
    Friend WithEvents chkViewClosedInvestments As System.Windows.Forms.CheckBox
    Friend WithEvents chkMinSystemValue As System.Windows.Forms.CheckBox
    Friend WithEvents txtMinSystemValue As System.Windows.Forms.TextBox
    Friend WithEvents lblRigBuildList As System.Windows.Forms.Label
    Friend WithEvents lvwRigBuildList As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents ContainerListViewColumnHeader9 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader10 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader11 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader12 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader13 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader14 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader15 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader16 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader17 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnAutoRig As System.Windows.Forms.Button
    Friend WithEvents bgAutoRig As System.Windows.Forms.GroupBox
    Friend WithEvents radRigSaleprice As System.Windows.Forms.RadioButton
    Friend WithEvents lblAutoRigCriteria As System.Windows.Forms.Label
    Friend WithEvents radRigMargin As System.Windows.Forms.RadioButton
    Friend WithEvents radRigProfit As System.Windows.Forms.RadioButton
    Friend WithEvents radTotalProfit As System.Windows.Forms.RadioButton
    Friend WithEvents radTotalSalePrice As System.Windows.Forms.RadioButton
    Friend WithEvents lblTotalRigProfit As System.Windows.Forms.Label
    Friend WithEvents lblTotalRigSalePrice As System.Windows.Forms.Label
    Friend WithEvents lblTotalRigMargin As System.Windows.Forms.Label
    Friend WithEvents mnuToolSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuItemRecycling As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecycleItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecycleContained As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecycleAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents colMetaLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colVolume As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents tssLabelSelectedAssets As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssLabelSelectedAssetsLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnReOpenInvestment As System.Windows.Forms.Button
    Friend WithEvents colJobsAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colJournalAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOrdersAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTransAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCorpSheetAPI As System.Windows.Forms.ColumnHeader
End Class
