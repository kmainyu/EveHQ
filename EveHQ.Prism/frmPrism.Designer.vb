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
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Corporation", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Personal", System.Windows.Forms.HorizontalAlignment.Left)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrism))
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
        Me.mnuAddCustomName = New System.Windows.Forms.ToolStripMenuItem
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
        Me.ctxTabPrism = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuClosePrismTab = New System.Windows.Forms.ToolStripMenuItem
        Me.tabAPIStatus = New System.Windows.Forms.TabPage
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
        Me.tabAssets = New System.Windows.Forms.TabPage
        Me.btnFilters = New System.Windows.Forms.Button
        Me.btnRefreshAssets = New System.Windows.Forms.Button
        Me.txtMinSystemValue = New System.Windows.Forms.TextBox
        Me.chkMinSystemValue = New System.Windows.Forms.CheckBox
        Me.chkExcludeItems = New System.Windows.Forms.CheckBox
        Me.chkExcludeInvestments = New System.Windows.Forms.CheckBox
        Me.chkExcludeCash = New System.Windows.Forms.CheckBox
        Me.lblGroupFilters = New System.Windows.Forms.Label
        Me.lblOwnerFilters = New System.Windows.Forms.Label
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.lblSearchAssets = New System.Windows.Forms.Label
        Me.tabAssetFilters = New System.Windows.Forms.TabPage
        Me.btnSelectCorp = New System.Windows.Forms.Button
        Me.btnSelectPersonal = New System.Windows.Forms.Button
        Me.btnAddAllOwners = New System.Windows.Forms.Button
        Me.btnClearAllOwners = New System.Windows.Forms.Button
        Me.btnClearGroupFilters = New System.Windows.Forms.Button
        Me.lvwCharFilter = New System.Windows.Forms.ListView
        Me.colOwnerName = New System.Windows.Forms.ColumnHeader
        Me.lblCharFilter = New System.Windows.Forms.Label
        Me.tabInvestments = New System.Windows.Forms.TabPage
        Me.lblTransactionView = New System.Windows.Forms.Label
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
        Me.btnExportRigList = New System.Windows.Forms.Button
        Me.btnExportRigBuildList = New System.Windows.Forms.Button
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
        Me.tabOrders = New System.Windows.Forms.TabPage
        Me.scMarketOrders = New System.Windows.Forms.SplitContainer
        Me.clvSellOrders = New DotNetLib.Windows.Forms.ContainerListView
        Me.colSOType = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSOQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSOPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSOLocation = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSOExpiresIn = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lblSellOrders = New System.Windows.Forms.Label
        Me.clvBuyOrders = New DotNetLib.Windows.Forms.ContainerListView
        Me.colBOType = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBOQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBOPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBOLocation = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBORange = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBOMinVol = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBOExpiresIn = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lblBuyOrders = New System.Windows.Forms.Label
        Me.panelOrderInfo = New System.Windows.Forms.Panel
        Me.btnExportOrders = New System.Windows.Forms.Button
        Me.lblRemoteRange = New System.Windows.Forms.Label
        Me.lblModRange = New System.Windows.Forms.Label
        Me.lblBidRange = New System.Windows.Forms.Label
        Me.lblAskRange = New System.Windows.Forms.Label
        Me.lblRemoteRangeLbl = New System.Windows.Forms.Label
        Me.lblModRangeLbl = New System.Windows.Forms.Label
        Me.lblBidRangeLbl = New System.Windows.Forms.Label
        Me.lblAskRangeLbl = New System.Windows.Forms.Label
        Me.lblBuyTotal = New System.Windows.Forms.Label
        Me.lblSellTotal = New System.Windows.Forms.Label
        Me.lblTransTax = New System.Windows.Forms.Label
        Me.lblBrokerFee = New System.Windows.Forms.Label
        Me.lblEscrow = New System.Windows.Forms.Label
        Me.lblOrders = New System.Windows.Forms.Label
        Me.lblBuyTotalLbl = New System.Windows.Forms.Label
        Me.lblSellTotalLbl = New System.Windows.Forms.Label
        Me.lblTransTaxLbl = New System.Windows.Forms.Label
        Me.lblBrokerFeeLbl = New System.Windows.Forms.Label
        Me.lblEscrowLbl = New System.Windows.Forms.Label
        Me.lblOrdersLbl = New System.Windows.Forms.Label
        Me.tabTransactions = New System.Windows.Forms.TabPage
        Me.btnExportTransactions = New System.Windows.Forms.Button
        Me.cboWalletTransDivision = New System.Windows.Forms.ComboBox
        Me.lblWalletTransDivision = New System.Windows.Forms.Label
        Me.clvTransactions = New DotNetLib.Windows.Forms.ContainerListView
        Me.colWTransDate = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWTransItem = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWTransQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWTransPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWTransTotal = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWTransLocation = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWTransClient = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.tabJournal = New System.Windows.Forms.TabPage
        Me.btnExportJournal = New System.Windows.Forms.Button
        Me.cboWalletJournalDivision = New System.Windows.Forms.ComboBox
        Me.lblWalletJournalDivision = New System.Windows.Forms.Label
        Me.clvJournal = New DotNetLib.Windows.Forms.ContainerListView
        Me.colWalletJournalDate = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWalletJournalType = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWalletJournalAmount = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWalletJournalBalance = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWalletJournalDescription = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.tabJobs = New System.Windows.Forms.TabPage
        Me.btnExportJobs = New System.Windows.Forms.Button
        Me.clvJobs = New DotNetLib.Windows.Forms.ContainerListView
        Me.colJobsItem = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colJobsActivity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colJobsLocation = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colJobsEndTime = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colJobsStatus = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.tabRecycle = New System.Windows.Forms.TabPage
        Me.chkFeesOnItems = New System.Windows.Forms.CheckBox
        Me.lblPriceTotals = New System.Windows.Forms.Label
        Me.chkFeesOnRefine = New System.Windows.Forms.CheckBox
        Me.lblTotalFees = New System.Windows.Forms.Label
        Me.lblTotalFeesLbl = New System.Windows.Forms.Label
        Me.nudTax = New System.Windows.Forms.NumericUpDown
        Me.nudBrokerFee = New System.Windows.Forms.NumericUpDown
        Me.chkOverrideTax = New System.Windows.Forms.CheckBox
        Me.chkOverrideBrokerFee = New System.Windows.Forms.CheckBox
        Me.lblItems = New System.Windows.Forms.Label
        Me.lblVolume = New System.Windows.Forms.Label
        Me.lblItemsLbl = New System.Windows.Forms.Label
        Me.lblVolumeLbl = New System.Windows.Forms.Label
        Me.cboRefineMode = New System.Windows.Forms.ComboBox
        Me.lblRefineMode = New System.Windows.Forms.Label
        Me.chkOverrideStandings = New System.Windows.Forms.CheckBox
        Me.chkOverrideBaseYield = New System.Windows.Forms.CheckBox
        Me.nudStandings = New System.Windows.Forms.NumericUpDown
        Me.nudBaseYield = New System.Windows.Forms.NumericUpDown
        Me.lblCorp = New System.Windows.Forms.Label
        Me.lblCorpLbl = New System.Windows.Forms.Label
        Me.lblStation = New System.Windows.Forms.Label
        Me.lblStationLbl = New System.Windows.Forms.Label
        Me.lblBaseYield = New System.Windows.Forms.Label
        Me.lblNetYield = New System.Windows.Forms.Label
        Me.lblStandings = New System.Windows.Forms.Label
        Me.lblStationTake = New System.Windows.Forms.Label
        Me.lblStationTakeLbl = New System.Windows.Forms.Label
        Me.lblStandingsLbl = New System.Windows.Forms.Label
        Me.lblNetYieldLbl = New System.Windows.Forms.Label
        Me.lblBaseYieldLbl = New System.Windows.Forms.Label
        Me.chkPerfectRefine = New System.Windows.Forms.CheckBox
        Me.cboRecyclePilots = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabItems = New System.Windows.Forms.TabPage
        Me.clvRecycle = New DotNetLib.Windows.Forms.ContainerListView
        Me.colRecycleItem = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRecycleMetaLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRecycleQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBatches = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colItemPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTotalPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colFees = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSalePrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRefinePrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBestPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ctxRecycleItems = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddRecycleItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxRecycleItem = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAlterRecycleQuantity = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRemoveRecycleItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabTotals = New System.Windows.Forms.TabPage
        Me.clvTotals = New DotNetLib.Windows.Forms.ContainerListView
        Me.colMaterial = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colStationTake = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWaste = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colReceive = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colMatPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colMatTotal = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.tssLabelTotalAssetsLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.tssLabelTotalAssets = New System.Windows.Forms.ToolStripStatusLabel
        Me.tssLabelSelectedAssetsLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.tssLabelSelectedAssets = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsbDownloadData = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.lblOwner = New System.Windows.Forms.ToolStripLabel
        Me.cboOwner = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbAssets = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbInvestments = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbRigBuilder = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbOrders = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbTransactions = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbJournal = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbJobs = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbRecycle = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator
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
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.mnuRemoveCustomName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
        Me.ctxAssets.SuspendLayout()
        Me.ctxFilter.SuspendLayout()
        Me.ctxFilterList.SuspendLayout()
        Me.tabPrism.SuspendLayout()
        Me.ctxTabPrism.SuspendLayout()
        Me.tabAPIStatus.SuspendLayout()
        Me.tabAssets.SuspendLayout()
        Me.tabAssetFilters.SuspendLayout()
        Me.tabInvestments.SuspendLayout()
        Me.tabRigBuilder.SuspendLayout()
        Me.bgAutoRig.SuspendLayout()
        CType(Me.nudRigMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabOrders.SuspendLayout()
        Me.scMarketOrders.Panel1.SuspendLayout()
        Me.scMarketOrders.Panel2.SuspendLayout()
        Me.scMarketOrders.SuspendLayout()
        Me.panelOrderInfo.SuspendLayout()
        Me.tabTransactions.SuspendLayout()
        Me.tabJournal.SuspendLayout()
        Me.tabJobs.SuspendLayout()
        Me.tabRecycle.SuspendLayout()
        CType(Me.nudTax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBrokerFee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStandings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBaseYield, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tabItems.SuspendLayout()
        Me.ctxRecycleItems.SuspendLayout()
        Me.ctxRecycleItem.SuspendLayout()
        Me.tabTotals.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.ctxAssets.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuItemName, Me.ToolStripMenuItem1, Me.mnuAddCustomName, Me.mnuRemoveCustomName, Me.ToolStripMenuItem3, Me.mnuViewInIB, Me.mnuViewInHQF, Me.mnuModifyPrice, Me.mnuToolSep, Me.mnuItemRecycling})
        Me.ctxAssets.Name = "ctxAssets"
        Me.ctxAssets.Size = New System.Drawing.Size(198, 198)
        '
        'mnuItemName
        '
        Me.mnuItemName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.mnuItemName.Name = "mnuItemName"
        Me.mnuItemName.Size = New System.Drawing.Size(197, 22)
        Me.mnuItemName.Text = "Item Name"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(194, 6)
        '
        'mnuAddCustomName
        '
        Me.mnuAddCustomName.Name = "mnuAddCustomName"
        Me.mnuAddCustomName.Size = New System.Drawing.Size(197, 22)
        Me.mnuAddCustomName.Text = "Add Custom Name"
        '
        'mnuViewInIB
        '
        Me.mnuViewInIB.Name = "mnuViewInIB"
        Me.mnuViewInIB.Size = New System.Drawing.Size(197, 22)
        Me.mnuViewInIB.Text = "View In Item Browser"
        '
        'mnuViewInHQF
        '
        Me.mnuViewInHQF.Name = "mnuViewInHQF"
        Me.mnuViewInHQF.Size = New System.Drawing.Size(197, 22)
        Me.mnuViewInHQF.Text = "Copy Setup for HQF"
        '
        'mnuModifyPrice
        '
        Me.mnuModifyPrice.Name = "mnuModifyPrice"
        Me.mnuModifyPrice.Size = New System.Drawing.Size(197, 22)
        Me.mnuModifyPrice.Text = "Modify Custom Price"
        '
        'mnuToolSep
        '
        Me.mnuToolSep.Name = "mnuToolSep"
        Me.mnuToolSep.Size = New System.Drawing.Size(194, 6)
        '
        'mnuItemRecycling
        '
        Me.mnuItemRecycling.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRecycleItem, Me.mnuRecycleContained, Me.mnuRecycleAll})
        Me.mnuItemRecycling.Name = "mnuItemRecycling"
        Me.mnuItemRecycling.Size = New System.Drawing.Size(197, 22)
        Me.mnuItemRecycling.Text = "Recycling Profitability"
        '
        'mnuRecycleItem
        '
        Me.mnuRecycleItem.Name = "mnuRecycleItem"
        Me.mnuRecycleItem.Size = New System.Drawing.Size(169, 22)
        Me.mnuRecycleItem.Text = "Current Item"
        '
        'mnuRecycleContained
        '
        Me.mnuRecycleContained.Enabled = False
        Me.mnuRecycleContained.Name = "mnuRecycleContained"
        Me.mnuRecycleContained.Size = New System.Drawing.Size(169, 22)
        Me.mnuRecycleContained.Text = "Contained Items"
        '
        'mnuRecycleAll
        '
        Me.mnuRecycleAll.Enabled = False
        Me.mnuRecycleAll.Name = "mnuRecycleAll"
        Me.mnuRecycleAll.Size = New System.Drawing.Size(169, 22)
        Me.mnuRecycleAll.Text = "Container + Items"
        '
        'chkExcludeBPs
        '
        Me.chkExcludeBPs.AutoSize = True
        Me.chkExcludeBPs.Location = New System.Drawing.Point(107, 60)
        Me.chkExcludeBPs.Name = "chkExcludeBPs"
        Me.chkExcludeBPs.Size = New System.Drawing.Size(112, 17)
        Me.chkExcludeBPs.TabIndex = 7
        Me.chkExcludeBPs.Text = "Exclude BP Values"
        Me.chkExcludeBPs.UseVisualStyleBackColor = True
        '
        'tvwFilter
        '
        Me.tvwFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
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
        Me.ctxFilter.Size = New System.Drawing.Size(143, 26)
        '
        'AddToFilterToolStripMenuItem
        '
        Me.AddToFilterToolStripMenuItem.Name = "AddToFilterToolStripMenuItem"
        Me.AddToFilterToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.AddToFilterToolStripMenuItem.Text = "Add To Filter"
        '
        'lblGroupFilter
        '
        Me.lblGroupFilter.AutoSize = True
        Me.lblGroupFilter.Location = New System.Drawing.Point(298, 15)
        Me.lblGroupFilter.Name = "lblGroupFilter"
        Me.lblGroupFilter.Size = New System.Drawing.Size(67, 13)
        Me.lblGroupFilter.TabIndex = 11
        Me.lblGroupFilter.Text = "Group Filter:"
        '
        'lstFilters
        '
        Me.lstFilters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
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
        Me.ctxFilterList.Size = New System.Drawing.Size(147, 26)
        '
        'RemoveFilterToolStripMenuItem
        '
        Me.RemoveFilterToolStripMenuItem.Name = "RemoveFilterToolStripMenuItem"
        Me.RemoveFilterToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.RemoveFilterToolStripMenuItem.Text = "Remove Filter"
        '
        'lblSelectedFilters
        '
        Me.lblSelectedFilters.AutoSize = True
        Me.lblSelectedFilters.Location = New System.Drawing.Point(493, 15)
        Me.lblSelectedFilters.Name = "lblSelectedFilters"
        Me.lblSelectedFilters.Size = New System.Drawing.Size(116, 13)
        Me.lblSelectedFilters.TabIndex = 13
        Me.lblSelectedFilters.Text = "Selected Group Filters:"
        '
        'tabPrism
        '
        Me.tabPrism.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabPrism.ContextMenuStrip = Me.ctxTabPrism
        Me.tabPrism.Controls.Add(Me.tabAPIStatus)
        Me.tabPrism.Controls.Add(Me.tabAssets)
        Me.tabPrism.Controls.Add(Me.tabAssetFilters)
        Me.tabPrism.Controls.Add(Me.tabInvestments)
        Me.tabPrism.Controls.Add(Me.tabRigBuilder)
        Me.tabPrism.Controls.Add(Me.tabOrders)
        Me.tabPrism.Controls.Add(Me.tabTransactions)
        Me.tabPrism.Controls.Add(Me.tabJournal)
        Me.tabPrism.Controls.Add(Me.tabJobs)
        Me.tabPrism.Controls.Add(Me.tabRecycle)
        Me.tabPrism.Location = New System.Drawing.Point(0, 28)
        Me.tabPrism.Name = "tabPrism"
        Me.tabPrism.SelectedIndex = 0
        Me.tabPrism.Size = New System.Drawing.Size(1144, 567)
        Me.tabPrism.TabIndex = 14
        '
        'ctxTabPrism
        '
        Me.ctxTabPrism.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClosePrismTab})
        Me.ctxTabPrism.Name = "ctxTabbedMDI"
        Me.ctxTabPrism.Size = New System.Drawing.Size(124, 26)
        '
        'mnuClosePrismTab
        '
        Me.mnuClosePrismTab.Name = "mnuClosePrismTab"
        Me.mnuClosePrismTab.Size = New System.Drawing.Size(123, 22)
        Me.mnuClosePrismTab.Text = "Not Valid"
        '
        'tabAPIStatus
        '
        Me.tabAPIStatus.Controls.Add(Me.lblCurrentAPI)
        Me.tabAPIStatus.Controls.Add(Me.lvwCurrentAPIs)
        Me.tabAPIStatus.Location = New System.Drawing.Point(4, 22)
        Me.tabAPIStatus.Name = "tabAPIStatus"
        Me.tabAPIStatus.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAPIStatus.Size = New System.Drawing.Size(1136, 541)
        Me.tabAPIStatus.TabIndex = 1
        Me.tabAPIStatus.Text = "API Status"
        Me.tabAPIStatus.UseVisualStyleBackColor = True
        '
        'lblCurrentAPI
        '
        Me.lblCurrentAPI.AutoSize = True
        Me.lblCurrentAPI.Location = New System.Drawing.Point(8, 9)
        Me.lblCurrentAPI.Name = "lblCurrentAPI"
        Me.lblCurrentAPI.Size = New System.Drawing.Size(85, 13)
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
        Me.lvwCurrentAPIs.ShowItemToolTips = True
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
        'tabAssets
        '
        Me.tabAssets.Controls.Add(Me.btnFilters)
        Me.tabAssets.Controls.Add(Me.btnRefreshAssets)
        Me.tabAssets.Controls.Add(Me.txtMinSystemValue)
        Me.tabAssets.Controls.Add(Me.chkMinSystemValue)
        Me.tabAssets.Controls.Add(Me.chkExcludeItems)
        Me.tabAssets.Controls.Add(Me.chkExcludeInvestments)
        Me.tabAssets.Controls.Add(Me.chkExcludeCash)
        Me.tabAssets.Controls.Add(Me.lblGroupFilters)
        Me.tabAssets.Controls.Add(Me.lblOwnerFilters)
        Me.tabAssets.Controls.Add(Me.txtSearch)
        Me.tabAssets.Controls.Add(Me.lblSearchAssets)
        Me.tabAssets.Controls.Add(Me.tlvAssets)
        Me.tabAssets.Controls.Add(Me.chkExcludeBPs)
        Me.tabAssets.Location = New System.Drawing.Point(4, 22)
        Me.tabAssets.Name = "tabAssets"
        Me.tabAssets.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAssets.Size = New System.Drawing.Size(1136, 541)
        Me.tabAssets.TabIndex = 0
        Me.tabAssets.Text = "Assets"
        Me.tabAssets.UseVisualStyleBackColor = True
        '
        'btnFilters
        '
        Me.btnFilters.Location = New System.Drawing.Point(90, 6)
        Me.btnFilters.Name = "btnFilters"
        Me.btnFilters.Size = New System.Drawing.Size(75, 23)
        Me.btnFilters.TabIndex = 28
        Me.btnFilters.Text = "Filters"
        Me.btnFilters.UseVisualStyleBackColor = True
        '
        'btnRefreshAssets
        '
        Me.btnRefreshAssets.Location = New System.Drawing.Point(9, 6)
        Me.btnRefreshAssets.Name = "btnRefreshAssets"
        Me.btnRefreshAssets.Size = New System.Drawing.Size(75, 23)
        Me.btnRefreshAssets.TabIndex = 27
        Me.btnRefreshAssets.Text = "Refresh"
        Me.btnRefreshAssets.UseVisualStyleBackColor = True
        '
        'txtMinSystemValue
        '
        Me.txtMinSystemValue.Location = New System.Drawing.Point(572, 57)
        Me.txtMinSystemValue.Name = "txtMinSystemValue"
        Me.txtMinSystemValue.Size = New System.Drawing.Size(135, 21)
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
        Me.chkExcludeItems.Size = New System.Drawing.Size(93, 17)
        Me.chkExcludeItems.TabIndex = 24
        Me.chkExcludeItems.Text = "Exclude Items"
        Me.chkExcludeItems.UseVisualStyleBackColor = True
        '
        'chkExcludeInvestments
        '
        Me.chkExcludeInvestments.AutoSize = True
        Me.chkExcludeInvestments.Location = New System.Drawing.Point(323, 60)
        Me.chkExcludeInvestments.Name = "chkExcludeInvestments"
        Me.chkExcludeInvestments.Size = New System.Drawing.Size(126, 17)
        Me.chkExcludeInvestments.TabIndex = 23
        Me.chkExcludeInvestments.Text = "Exclude Investments"
        Me.chkExcludeInvestments.UseVisualStyleBackColor = True
        '
        'chkExcludeCash
        '
        Me.chkExcludeCash.AutoSize = True
        Me.chkExcludeCash.Location = New System.Drawing.Point(226, 60)
        Me.chkExcludeCash.Name = "chkExcludeCash"
        Me.chkExcludeCash.Size = New System.Drawing.Size(90, 17)
        Me.chkExcludeCash.TabIndex = 22
        Me.chkExcludeCash.Text = "Exclude Cash"
        Me.chkExcludeCash.UseVisualStyleBackColor = True
        '
        'lblGroupFilters
        '
        Me.lblGroupFilters.AutoSize = True
        Me.lblGroupFilters.Location = New System.Drawing.Point(334, 37)
        Me.lblGroupFilters.Name = "lblGroupFilters"
        Me.lblGroupFilters.Size = New System.Drawing.Size(95, 13)
        Me.lblGroupFilters.TabIndex = 21
        Me.lblGroupFilters.Text = "Group Filter: None"
        '
        'lblOwnerFilters
        '
        Me.lblOwnerFilters.AutoSize = True
        Me.lblOwnerFilters.Location = New System.Drawing.Point(334, 11)
        Me.lblOwnerFilters.Name = "lblOwnerFilters"
        Me.lblOwnerFilters.Size = New System.Drawing.Size(98, 13)
        Me.lblOwnerFilters.TabIndex = 20
        Me.lblOwnerFilters.Text = "Owner Filter: None"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(113, 34)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(215, 21)
        Me.txtSearch.TabIndex = 19
        '
        'lblSearchAssets
        '
        Me.lblSearchAssets.AutoSize = True
        Me.lblSearchAssets.Location = New System.Drawing.Point(6, 37)
        Me.lblSearchAssets.Name = "lblSearchAssets"
        Me.lblSearchAssets.Size = New System.Drawing.Size(44, 13)
        Me.lblSearchAssets.TabIndex = 18
        Me.lblSearchAssets.Text = "Search:"
        '
        'tabAssetFilters
        '
        Me.tabAssetFilters.Controls.Add(Me.btnSelectCorp)
        Me.tabAssetFilters.Controls.Add(Me.btnSelectPersonal)
        Me.tabAssetFilters.Controls.Add(Me.btnAddAllOwners)
        Me.tabAssetFilters.Controls.Add(Me.btnClearAllOwners)
        Me.tabAssetFilters.Controls.Add(Me.btnClearGroupFilters)
        Me.tabAssetFilters.Controls.Add(Me.lvwCharFilter)
        Me.tabAssetFilters.Controls.Add(Me.lblCharFilter)
        Me.tabAssetFilters.Controls.Add(Me.lstFilters)
        Me.tabAssetFilters.Controls.Add(Me.lblSelectedFilters)
        Me.tabAssetFilters.Controls.Add(Me.lblGroupFilter)
        Me.tabAssetFilters.Controls.Add(Me.tvwFilter)
        Me.tabAssetFilters.Location = New System.Drawing.Point(4, 22)
        Me.tabAssetFilters.Name = "tabAssetFilters"
        Me.tabAssetFilters.Size = New System.Drawing.Size(1136, 541)
        Me.tabAssetFilters.TabIndex = 2
        Me.tabAssetFilters.Text = "Asset Filters"
        Me.tabAssetFilters.UseVisualStyleBackColor = True
        '
        'btnSelectCorp
        '
        Me.btnSelectCorp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectCorp.Location = New System.Drawing.Point(54, 467)
        Me.btnSelectCorp.Name = "btnSelectCorp"
        Me.btnSelectCorp.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectCorp.TabIndex = 21
        Me.btnSelectCorp.Text = "Corporation"
        Me.btnSelectCorp.UseVisualStyleBackColor = True
        '
        'btnSelectPersonal
        '
        Me.btnSelectPersonal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectPersonal.Location = New System.Drawing.Point(135, 467)
        Me.btnSelectPersonal.Name = "btnSelectPersonal"
        Me.btnSelectPersonal.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectPersonal.TabIndex = 20
        Me.btnSelectPersonal.Text = "Personal"
        Me.btnSelectPersonal.UseVisualStyleBackColor = True
        '
        'btnAddAllOwners
        '
        Me.btnAddAllOwners.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddAllOwners.Location = New System.Drawing.Point(54, 496)
        Me.btnAddAllOwners.Name = "btnAddAllOwners"
        Me.btnAddAllOwners.Size = New System.Drawing.Size(75, 23)
        Me.btnAddAllOwners.TabIndex = 19
        Me.btnAddAllOwners.Text = "Check All"
        Me.btnAddAllOwners.UseVisualStyleBackColor = True
        '
        'btnClearAllOwners
        '
        Me.btnClearAllOwners.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearAllOwners.Location = New System.Drawing.Point(135, 496)
        Me.btnClearAllOwners.Name = "btnClearAllOwners"
        Me.btnClearAllOwners.Size = New System.Drawing.Size(75, 23)
        Me.btnClearAllOwners.TabIndex = 18
        Me.btnClearAllOwners.Text = "Clear All"
        Me.btnClearAllOwners.UseVisualStyleBackColor = True
        '
        'btnClearGroupFilters
        '
        Me.btnClearGroupFilters.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearGroupFilters.Location = New System.Drawing.Point(496, 496)
        Me.btnClearGroupFilters.Name = "btnClearGroupFilters"
        Me.btnClearGroupFilters.Size = New System.Drawing.Size(75, 23)
        Me.btnClearGroupFilters.TabIndex = 17
        Me.btnClearGroupFilters.Text = "Clear All"
        Me.btnClearGroupFilters.UseVisualStyleBackColor = True
        '
        'lvwCharFilter
        '
        Me.lvwCharFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwCharFilter.CheckBoxes = True
        Me.lvwCharFilter.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colOwnerName})
        ListViewGroup3.Header = "Corporation"
        ListViewGroup3.Name = "grpCorporation"
        ListViewGroup4.Header = "Personal"
        ListViewGroup4.Name = "grpPersonal"
        Me.lvwCharFilter.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup3, ListViewGroup4})
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
        Me.lblCharFilter.Size = New System.Drawing.Size(70, 13)
        Me.lblCharFilter.TabIndex = 15
        Me.lblCharFilter.Text = "Owner Filter:"
        '
        'tabInvestments
        '
        Me.tabInvestments.Controls.Add(Me.lblTransactionView)
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
        'lblTransactionView
        '
        Me.lblTransactionView.AutoSize = True
        Me.lblTransactionView.Location = New System.Drawing.Point(8, 257)
        Me.lblTransactionView.Name = "lblTransactionView"
        Me.lblTransactionView.Size = New System.Drawing.Size(139, 13)
        Me.lblTransactionView.TabIndex = 14
        Me.lblTransactionView.Text = "Viewing Transactions: None"
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
        Me.chkViewClosedInvestments.Size = New System.Drawing.Size(146, 17)
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
        Me.btnEditTransaction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditTransaction.Location = New System.Drawing.Point(114, 510)
        Me.btnEditTransaction.Name = "btnEditTransaction"
        Me.btnEditTransaction.Size = New System.Drawing.Size(100, 23)
        Me.btnEditTransaction.TabIndex = 8
        Me.btnEditTransaction.Text = "Edit Transaction"
        Me.btnEditTransaction.UseVisualStyleBackColor = True
        '
        'btnClearTransactions
        '
        Me.btnClearTransactions.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearTransactions.Location = New System.Drawing.Point(220, 510)
        Me.btnClearTransactions.Name = "btnClearTransactions"
        Me.btnClearTransactions.Size = New System.Drawing.Size(104, 23)
        Me.btnClearTransactions.TabIndex = 7
        Me.btnClearTransactions.Text = "Clear Transactions"
        Me.btnClearTransactions.UseVisualStyleBackColor = True
        Me.btnClearTransactions.Visible = False
        '
        'lvwTransactions
        '
        Me.lvwTransactions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwTransactions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTransID, Me.colTransDate, Me.colTransType, Me.colTransQuantity, Me.colTransValue})
        Me.lvwTransactions.FullRowSelect = True
        Me.lvwTransactions.GridLines = True
        Me.lvwTransactions.HideSelection = False
        Me.lvwTransactions.Location = New System.Drawing.Point(8, 273)
        Me.lvwTransactions.Name = "lvwTransactions"
        Me.lvwTransactions.Size = New System.Drawing.Size(1120, 231)
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
        Me.btnAddTransaction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddTransaction.Location = New System.Drawing.Point(8, 510)
        Me.btnAddTransaction.Name = "btnAddTransaction"
        Me.btnAddTransaction.Size = New System.Drawing.Size(100, 23)
        Me.btnAddTransaction.TabIndex = 5
        Me.btnAddTransaction.Text = "Add Transaction"
        Me.btnAddTransaction.UseVisualStyleBackColor = True
        '
        'btnClearInvestments
        '
        Me.btnClearInvestments.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearInvestments.Location = New System.Drawing.Point(1018, 223)
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
        Me.lvwInvestments.Size = New System.Drawing.Size(1120, 214)
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
        Me.tabRigBuilder.Controls.Add(Me.btnExportRigList)
        Me.tabRigBuilder.Controls.Add(Me.btnExportRigBuildList)
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
        'btnExportRigList
        '
        Me.btnExportRigList.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportRigList.Location = New System.Drawing.Point(1028, 48)
        Me.btnExportRigList.Name = "btnExportRigList"
        Me.btnExportRigList.Size = New System.Drawing.Size(100, 23)
        Me.btnExportRigList.TabIndex = 45
        Me.btnExportRigList.Text = "Export Rig List"
        Me.btnExportRigList.UseVisualStyleBackColor = True
        '
        'btnExportRigBuildList
        '
        Me.btnExportRigBuildList.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportRigBuildList.Location = New System.Drawing.Point(1028, 77)
        Me.btnExportRigBuildList.Name = "btnExportRigBuildList"
        Me.btnExportRigBuildList.Size = New System.Drawing.Size(100, 23)
        Me.btnExportRigBuildList.TabIndex = 44
        Me.btnExportRigBuildList.Text = "Export Build List"
        Me.btnExportRigBuildList.UseVisualStyleBackColor = True
        '
        'lblTotalRigMargin
        '
        Me.lblTotalRigMargin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigMargin.AutoSize = True
        Me.lblTotalRigMargin.Location = New System.Drawing.Point(553, 311)
        Me.lblTotalRigMargin.Name = "lblTotalRigMargin"
        Me.lblTotalRigMargin.Size = New System.Drawing.Size(43, 13)
        Me.lblTotalRigMargin.TabIndex = 33
        Me.lblTotalRigMargin.Text = "Margin:"
        '
        'lblTotalRigProfit
        '
        Me.lblTotalRigProfit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigProfit.AutoSize = True
        Me.lblTotalRigProfit.Location = New System.Drawing.Point(350, 311)
        Me.lblTotalRigProfit.Name = "lblTotalRigProfit"
        Me.lblTotalRigProfit.Size = New System.Drawing.Size(64, 13)
        Me.lblTotalRigProfit.TabIndex = 32
        Me.lblTotalRigProfit.Text = "Total Profit:"
        '
        'lblTotalRigSalePrice
        '
        Me.lblTotalRigSalePrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigSalePrice.AutoSize = True
        Me.lblTotalRigSalePrice.Location = New System.Drawing.Point(138, 311)
        Me.lblTotalRigSalePrice.Name = "lblTotalRigSalePrice"
        Me.lblTotalRigSalePrice.Size = New System.Drawing.Size(84, 13)
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
        Me.radTotalProfit.Size = New System.Drawing.Size(78, 17)
        Me.radTotalProfit.TabIndex = 35
        Me.radTotalProfit.Text = "Total Profit"
        Me.radTotalProfit.UseVisualStyleBackColor = True
        '
        'radTotalSalePrice
        '
        Me.radTotalSalePrice.AutoSize = True
        Me.radTotalSalePrice.Location = New System.Drawing.Point(55, 36)
        Me.radTotalSalePrice.Name = "radTotalSalePrice"
        Me.radTotalSalePrice.Size = New System.Drawing.Size(98, 17)
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
        Me.radRigMargin.Size = New System.Drawing.Size(75, 17)
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
        Me.radRigProfit.Size = New System.Drawing.Size(69, 17)
        Me.radRigProfit.TabIndex = 32
        Me.radRigProfit.Text = "Rig Profit"
        Me.radRigProfit.UseVisualStyleBackColor = True
        '
        'radRigSaleprice
        '
        Me.radRigSaleprice.AutoSize = True
        Me.radRigSaleprice.Location = New System.Drawing.Point(55, 18)
        Me.radRigSaleprice.Name = "radRigSaleprice"
        Me.radRigSaleprice.Size = New System.Drawing.Size(89, 17)
        Me.radRigSaleprice.TabIndex = 31
        Me.radRigSaleprice.Text = "Rig Sale Price"
        Me.radRigSaleprice.UseVisualStyleBackColor = True
        '
        'lblAutoRigCriteria
        '
        Me.lblAutoRigCriteria.AutoSize = True
        Me.lblAutoRigCriteria.Location = New System.Drawing.Point(7, 20)
        Me.lblAutoRigCriteria.Name = "lblAutoRigCriteria"
        Me.lblAutoRigCriteria.Size = New System.Drawing.Size(46, 13)
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
        Me.lblRigBuildList.Size = New System.Drawing.Size(70, 13)
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
        Me.lvwRigBuildList.Size = New System.Drawing.Size(1120, 211)
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
        Me.lvwRigs.Size = New System.Drawing.Size(1120, 193)
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
        Me.nudRigMELevel.Size = New System.Drawing.Size(70, 21)
        Me.nudRigMELevel.TabIndex = 25
        '
        'lblRigMELevel
        '
        Me.lblRigMELevel.AutoSize = True
        Me.lblRigMELevel.Location = New System.Drawing.Point(5, 47)
        Me.lblRigMELevel.Name = "lblRigMELevel"
        Me.lblRigMELevel.Size = New System.Drawing.Size(71, 13)
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
        Me.lblRigOwnerFilter.Size = New System.Drawing.Size(98, 13)
        Me.lblRigOwnerFilter.TabIndex = 21
        Me.lblRigOwnerFilter.Text = "Owner Filter: None"
        '
        'tabOrders
        '
        Me.tabOrders.Controls.Add(Me.scMarketOrders)
        Me.tabOrders.Controls.Add(Me.panelOrderInfo)
        Me.tabOrders.Location = New System.Drawing.Point(4, 22)
        Me.tabOrders.Name = "tabOrders"
        Me.tabOrders.Size = New System.Drawing.Size(1136, 541)
        Me.tabOrders.TabIndex = 5
        Me.tabOrders.Text = "Market Orders"
        Me.tabOrders.UseVisualStyleBackColor = True
        '
        'scMarketOrders
        '
        Me.scMarketOrders.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.scMarketOrders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMarketOrders.Location = New System.Drawing.Point(0, 0)
        Me.scMarketOrders.Name = "scMarketOrders"
        Me.scMarketOrders.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMarketOrders.Panel1
        '
        Me.scMarketOrders.Panel1.Controls.Add(Me.clvSellOrders)
        Me.scMarketOrders.Panel1.Controls.Add(Me.lblSellOrders)
        '
        'scMarketOrders.Panel2
        '
        Me.scMarketOrders.Panel2.Controls.Add(Me.clvBuyOrders)
        Me.scMarketOrders.Panel2.Controls.Add(Me.lblBuyOrders)
        Me.scMarketOrders.Size = New System.Drawing.Size(1136, 446)
        Me.scMarketOrders.SplitterDistance = 225
        Me.scMarketOrders.TabIndex = 28
        '
        'clvSellOrders
        '
        Me.clvSellOrders.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvSellOrders.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colSOType, Me.colSOQuantity, Me.colSOPrice, Me.colSOLocation, Me.colSOExpiresIn})
        Me.clvSellOrders.DefaultItemHeight = 20
        Me.clvSellOrders.Location = New System.Drawing.Point(8, 21)
        Me.clvSellOrders.Name = "clvSellOrders"
        Me.clvSellOrders.Size = New System.Drawing.Size(1118, 197)
        Me.clvSellOrders.TabIndex = 26
        '
        'colSOType
        '
        Me.colSOType.CustomSortTag = Nothing
        Me.colSOType.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colSOType.Tag = Nothing
        Me.colSOType.Text = "Type"
        Me.colSOType.Width = 250
        '
        'colSOQuantity
        '
        Me.colSOQuantity.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colSOQuantity.CustomSortTag = Nothing
        Me.colSOQuantity.DisplayIndex = 1
        Me.colSOQuantity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colSOQuantity.Tag = Nothing
        Me.colSOQuantity.Text = "Quantity"
        Me.colSOQuantity.Width = 125
        '
        'colSOPrice
        '
        Me.colSOPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colSOPrice.CustomSortTag = Nothing
        Me.colSOPrice.DisplayIndex = 2
        Me.colSOPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colSOPrice.Tag = Nothing
        Me.colSOPrice.Text = "Price"
        Me.colSOPrice.Width = 125
        '
        'colSOLocation
        '
        Me.colSOLocation.CustomSortTag = Nothing
        Me.colSOLocation.DisplayIndex = 3
        Me.colSOLocation.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colSOLocation.Tag = Nothing
        Me.colSOLocation.Text = "Location"
        Me.colSOLocation.Width = 300
        '
        'colSOExpiresIn
        '
        Me.colSOExpiresIn.CustomSortTag = Nothing
        Me.colSOExpiresIn.DisplayIndex = 4
        Me.colSOExpiresIn.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.colSOExpiresIn.Tag = Nothing
        Me.colSOExpiresIn.Text = "Expires In"
        Me.colSOExpiresIn.Width = 125
        '
        'lblSellOrders
        '
        Me.lblSellOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSellOrders.AutoSize = True
        Me.lblSellOrders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSellOrders.Location = New System.Drawing.Point(5, 5)
        Me.lblSellOrders.Name = "lblSellOrders"
        Me.lblSellOrders.Size = New System.Drawing.Size(47, 13)
        Me.lblSellOrders.TabIndex = 25
        Me.lblSellOrders.Text = "Selling:"
        '
        'clvBuyOrders
        '
        Me.clvBuyOrders.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvBuyOrders.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colBOType, Me.colBOQuantity, Me.colBOPrice, Me.colBOLocation, Me.colBORange, Me.colBOMinVol, Me.colBOExpiresIn})
        Me.clvBuyOrders.DefaultItemHeight = 20
        Me.clvBuyOrders.Location = New System.Drawing.Point(8, 21)
        Me.clvBuyOrders.Name = "clvBuyOrders"
        Me.clvBuyOrders.Size = New System.Drawing.Size(1118, 188)
        Me.clvBuyOrders.TabIndex = 27
        '
        'colBOType
        '
        Me.colBOType.CustomSortTag = Nothing
        Me.colBOType.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colBOType.Tag = Nothing
        Me.colBOType.Text = "Type"
        Me.colBOType.Width = 250
        '
        'colBOQuantity
        '
        Me.colBOQuantity.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBOQuantity.CustomSortTag = Nothing
        Me.colBOQuantity.DisplayIndex = 1
        Me.colBOQuantity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBOQuantity.Tag = Nothing
        Me.colBOQuantity.Text = "Quantity"
        Me.colBOQuantity.Width = 125
        '
        'colBOPrice
        '
        Me.colBOPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBOPrice.CustomSortTag = Nothing
        Me.colBOPrice.DisplayIndex = 2
        Me.colBOPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBOPrice.Tag = Nothing
        Me.colBOPrice.Text = "Price"
        Me.colBOPrice.Width = 125
        '
        'colBOLocation
        '
        Me.colBOLocation.CustomSortTag = Nothing
        Me.colBOLocation.DisplayIndex = 3
        Me.colBOLocation.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colBOLocation.Tag = Nothing
        Me.colBOLocation.Text = "Location"
        Me.colBOLocation.Width = 300
        '
        'colBORange
        '
        Me.colBORange.CustomSortTag = Nothing
        Me.colBORange.DisplayIndex = 4
        Me.colBORange.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.colBORange.Tag = Nothing
        Me.colBORange.Text = "Range"
        Me.colBORange.Width = 75
        '
        'colBOMinVol
        '
        Me.colBOMinVol.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBOMinVol.CustomSortTag = Nothing
        Me.colBOMinVol.DisplayIndex = 5
        Me.colBOMinVol.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBOMinVol.Tag = Nothing
        Me.colBOMinVol.Text = "Min Volume"
        Me.colBOMinVol.Width = 100
        '
        'colBOExpiresIn
        '
        Me.colBOExpiresIn.CustomSortTag = Nothing
        Me.colBOExpiresIn.DisplayIndex = 6
        Me.colBOExpiresIn.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.colBOExpiresIn.Tag = Nothing
        Me.colBOExpiresIn.Text = "Expires In"
        Me.colBOExpiresIn.Width = 125
        '
        'lblBuyOrders
        '
        Me.lblBuyOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBuyOrders.AutoSize = True
        Me.lblBuyOrders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBuyOrders.Location = New System.Drawing.Point(5, 5)
        Me.lblBuyOrders.Name = "lblBuyOrders"
        Me.lblBuyOrders.Size = New System.Drawing.Size(48, 13)
        Me.lblBuyOrders.TabIndex = 26
        Me.lblBuyOrders.Text = "Buying:"
        '
        'panelOrderInfo
        '
        Me.panelOrderInfo.Controls.Add(Me.btnExportOrders)
        Me.panelOrderInfo.Controls.Add(Me.lblRemoteRange)
        Me.panelOrderInfo.Controls.Add(Me.lblModRange)
        Me.panelOrderInfo.Controls.Add(Me.lblBidRange)
        Me.panelOrderInfo.Controls.Add(Me.lblAskRange)
        Me.panelOrderInfo.Controls.Add(Me.lblRemoteRangeLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblModRangeLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblBidRangeLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblAskRangeLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblBuyTotal)
        Me.panelOrderInfo.Controls.Add(Me.lblSellTotal)
        Me.panelOrderInfo.Controls.Add(Me.lblTransTax)
        Me.panelOrderInfo.Controls.Add(Me.lblBrokerFee)
        Me.panelOrderInfo.Controls.Add(Me.lblEscrow)
        Me.panelOrderInfo.Controls.Add(Me.lblOrders)
        Me.panelOrderInfo.Controls.Add(Me.lblBuyTotalLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblSellTotalLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblTransTaxLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblBrokerFeeLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblEscrowLbl)
        Me.panelOrderInfo.Controls.Add(Me.lblOrdersLbl)
        Me.panelOrderInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelOrderInfo.Location = New System.Drawing.Point(0, 446)
        Me.panelOrderInfo.Name = "panelOrderInfo"
        Me.panelOrderInfo.Size = New System.Drawing.Size(1136, 95)
        Me.panelOrderInfo.TabIndex = 27
        '
        'btnExportOrders
        '
        Me.btnExportOrders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportOrders.Location = New System.Drawing.Point(1053, 6)
        Me.btnExportOrders.Name = "btnExportOrders"
        Me.btnExportOrders.Size = New System.Drawing.Size(75, 23)
        Me.btnExportOrders.TabIndex = 43
        Me.btnExportOrders.Text = "Export"
        Me.btnExportOrders.UseVisualStyleBackColor = True
        '
        'lblRemoteRange
        '
        Me.lblRemoteRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRemoteRange.AutoSize = True
        Me.lblRemoteRange.Location = New System.Drawing.Point(412, 47)
        Me.lblRemoteRange.Name = "lblRemoteRange"
        Me.lblRemoteRange.Size = New System.Drawing.Size(62, 13)
        Me.lblRemoteRange.TabIndex = 42
        Me.lblRemoteRange.Text = "placeholder"
        '
        'lblModRange
        '
        Me.lblModRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblModRange.AutoSize = True
        Me.lblModRange.Location = New System.Drawing.Point(412, 34)
        Me.lblModRange.Name = "lblModRange"
        Me.lblModRange.Size = New System.Drawing.Size(62, 13)
        Me.lblModRange.TabIndex = 41
        Me.lblModRange.Text = "placeholder"
        '
        'lblBidRange
        '
        Me.lblBidRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBidRange.AutoSize = True
        Me.lblBidRange.Location = New System.Drawing.Point(412, 21)
        Me.lblBidRange.Name = "lblBidRange"
        Me.lblBidRange.Size = New System.Drawing.Size(62, 13)
        Me.lblBidRange.TabIndex = 40
        Me.lblBidRange.Text = "placeholder"
        '
        'lblAskRange
        '
        Me.lblAskRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAskRange.AutoSize = True
        Me.lblAskRange.Location = New System.Drawing.Point(412, 8)
        Me.lblAskRange.Name = "lblAskRange"
        Me.lblAskRange.Size = New System.Drawing.Size(62, 13)
        Me.lblAskRange.TabIndex = 39
        Me.lblAskRange.Text = "placeholder"
        '
        'lblRemoteRangeLbl
        '
        Me.lblRemoteRangeLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRemoteRangeLbl.AutoSize = True
        Me.lblRemoteRangeLbl.Location = New System.Drawing.Point(304, 47)
        Me.lblRemoteRangeLbl.Name = "lblRemoteRangeLbl"
        Me.lblRemoteRangeLbl.Size = New System.Drawing.Size(99, 13)
        Me.lblRemoteRangeLbl.TabIndex = 38
        Me.lblRemoteRangeLbl.Text = "Remote Bid Range:"
        '
        'lblModRangeLbl
        '
        Me.lblModRangeLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblModRangeLbl.AutoSize = True
        Me.lblModRangeLbl.Location = New System.Drawing.Point(304, 34)
        Me.lblModRangeLbl.Name = "lblModRangeLbl"
        Me.lblModRangeLbl.Size = New System.Drawing.Size(102, 13)
        Me.lblModRangeLbl.TabIndex = 37
        Me.lblModRangeLbl.Text = "Modification Range:"
        '
        'lblBidRangeLbl
        '
        Me.lblBidRangeLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBidRangeLbl.AutoSize = True
        Me.lblBidRangeLbl.Location = New System.Drawing.Point(304, 21)
        Me.lblBidRangeLbl.Name = "lblBidRangeLbl"
        Me.lblBidRangeLbl.Size = New System.Drawing.Size(59, 13)
        Me.lblBidRangeLbl.TabIndex = 36
        Me.lblBidRangeLbl.Text = "Bid Range:"
        '
        'lblAskRangeLbl
        '
        Me.lblAskRangeLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAskRangeLbl.AutoSize = True
        Me.lblAskRangeLbl.Location = New System.Drawing.Point(304, 8)
        Me.lblAskRangeLbl.Name = "lblAskRangeLbl"
        Me.lblAskRangeLbl.Size = New System.Drawing.Size(62, 13)
        Me.lblAskRangeLbl.TabIndex = 35
        Me.lblAskRangeLbl.Text = "Ask Range:"
        '
        'lblBuyTotal
        '
        Me.lblBuyTotal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBuyTotal.AutoSize = True
        Me.lblBuyTotal.Location = New System.Drawing.Point(108, 60)
        Me.lblBuyTotal.Name = "lblBuyTotal"
        Me.lblBuyTotal.Size = New System.Drawing.Size(62, 13)
        Me.lblBuyTotal.TabIndex = 34
        Me.lblBuyTotal.Text = "placeholder"
        '
        'lblSellTotal
        '
        Me.lblSellTotal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSellTotal.AutoSize = True
        Me.lblSellTotal.Location = New System.Drawing.Point(108, 47)
        Me.lblSellTotal.Name = "lblSellTotal"
        Me.lblSellTotal.Size = New System.Drawing.Size(62, 13)
        Me.lblSellTotal.TabIndex = 33
        Me.lblSellTotal.Text = "placeholder"
        '
        'lblTransTax
        '
        Me.lblTransTax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTransTax.AutoSize = True
        Me.lblTransTax.Location = New System.Drawing.Point(108, 34)
        Me.lblTransTax.Name = "lblTransTax"
        Me.lblTransTax.Size = New System.Drawing.Size(62, 13)
        Me.lblTransTax.TabIndex = 32
        Me.lblTransTax.Text = "placeholder"
        '
        'lblBrokerFee
        '
        Me.lblBrokerFee.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBrokerFee.AutoSize = True
        Me.lblBrokerFee.Location = New System.Drawing.Point(108, 21)
        Me.lblBrokerFee.Name = "lblBrokerFee"
        Me.lblBrokerFee.Size = New System.Drawing.Size(62, 13)
        Me.lblBrokerFee.TabIndex = 31
        Me.lblBrokerFee.Text = "placeholder"
        '
        'lblEscrow
        '
        Me.lblEscrow.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEscrow.AutoSize = True
        Me.lblEscrow.Location = New System.Drawing.Point(108, 73)
        Me.lblEscrow.Name = "lblEscrow"
        Me.lblEscrow.Size = New System.Drawing.Size(62, 13)
        Me.lblEscrow.TabIndex = 30
        Me.lblEscrow.Text = "placeholder"
        '
        'lblOrders
        '
        Me.lblOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOrders.AutoSize = True
        Me.lblOrders.Location = New System.Drawing.Point(108, 8)
        Me.lblOrders.Name = "lblOrders"
        Me.lblOrders.Size = New System.Drawing.Size(62, 13)
        Me.lblOrders.TabIndex = 29
        Me.lblOrders.Text = "placeholder"
        '
        'lblBuyTotalLbl
        '
        Me.lblBuyTotalLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBuyTotalLbl.AutoSize = True
        Me.lblBuyTotalLbl.Location = New System.Drawing.Point(8, 60)
        Me.lblBuyTotalLbl.Name = "lblBuyTotalLbl"
        Me.lblBuyTotalLbl.Size = New System.Drawing.Size(92, 13)
        Me.lblBuyTotalLbl.TabIndex = 28
        Me.lblBuyTotalLbl.Text = "Buy Orders Total:"
        '
        'lblSellTotalLbl
        '
        Me.lblSellTotalLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSellTotalLbl.AutoSize = True
        Me.lblSellTotalLbl.Location = New System.Drawing.Point(8, 47)
        Me.lblSellTotalLbl.Name = "lblSellTotalLbl"
        Me.lblSellTotalLbl.Size = New System.Drawing.Size(90, 13)
        Me.lblSellTotalLbl.TabIndex = 27
        Me.lblSellTotalLbl.Text = "Sell Orders Total:"
        '
        'lblTransTaxLbl
        '
        Me.lblTransTaxLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTransTaxLbl.AutoSize = True
        Me.lblTransTaxLbl.Location = New System.Drawing.Point(8, 34)
        Me.lblTransTaxLbl.Name = "lblTransTaxLbl"
        Me.lblTransTaxLbl.Size = New System.Drawing.Size(88, 13)
        Me.lblTransTaxLbl.TabIndex = 26
        Me.lblTransTaxLbl.Text = "Transaction Tax:"
        '
        'lblBrokerFeeLbl
        '
        Me.lblBrokerFeeLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBrokerFeeLbl.AutoSize = True
        Me.lblBrokerFeeLbl.Location = New System.Drawing.Point(8, 21)
        Me.lblBrokerFeeLbl.Name = "lblBrokerFeeLbl"
        Me.lblBrokerFeeLbl.Size = New System.Drawing.Size(89, 13)
        Me.lblBrokerFeeLbl.TabIndex = 25
        Me.lblBrokerFeeLbl.Text = "Base Broker Fee:"
        '
        'lblEscrowLbl
        '
        Me.lblEscrowLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEscrowLbl.AutoSize = True
        Me.lblEscrowLbl.Location = New System.Drawing.Point(8, 73)
        Me.lblEscrowLbl.Name = "lblEscrowLbl"
        Me.lblEscrowLbl.Size = New System.Drawing.Size(83, 13)
        Me.lblEscrowLbl.TabIndex = 24
        Me.lblEscrowLbl.Text = "Total in Escrow:"
        '
        'lblOrdersLbl
        '
        Me.lblOrdersLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOrdersLbl.AutoSize = True
        Me.lblOrdersLbl.Location = New System.Drawing.Point(8, 8)
        Me.lblOrdersLbl.Name = "lblOrdersLbl"
        Me.lblOrdersLbl.Size = New System.Drawing.Size(96, 13)
        Me.lblOrdersLbl.TabIndex = 23
        Me.lblOrdersLbl.Text = "Orders Remaining:"
        '
        'tabTransactions
        '
        Me.tabTransactions.Controls.Add(Me.btnExportTransactions)
        Me.tabTransactions.Controls.Add(Me.cboWalletTransDivision)
        Me.tabTransactions.Controls.Add(Me.lblWalletTransDivision)
        Me.tabTransactions.Controls.Add(Me.clvTransactions)
        Me.tabTransactions.Location = New System.Drawing.Point(4, 22)
        Me.tabTransactions.Name = "tabTransactions"
        Me.tabTransactions.Size = New System.Drawing.Size(1136, 541)
        Me.tabTransactions.TabIndex = 6
        Me.tabTransactions.Text = "Transactions"
        Me.tabTransactions.UseVisualStyleBackColor = True
        '
        'btnExportTransactions
        '
        Me.btnExportTransactions.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportTransactions.Location = New System.Drawing.Point(1053, 5)
        Me.btnExportTransactions.Name = "btnExportTransactions"
        Me.btnExportTransactions.Size = New System.Drawing.Size(75, 23)
        Me.btnExportTransactions.TabIndex = 3
        Me.btnExportTransactions.Text = "Export"
        Me.btnExportTransactions.UseVisualStyleBackColor = True
        '
        'cboWalletTransDivision
        '
        Me.cboWalletTransDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWalletTransDivision.FormattingEnabled = True
        Me.cboWalletTransDivision.Items.AddRange(New Object() {"1000", "1001", "1002", "1003", "1004", "1005", "1006"})
        Me.cboWalletTransDivision.Location = New System.Drawing.Point(94, 7)
        Me.cboWalletTransDivision.Name = "cboWalletTransDivision"
        Me.cboWalletTransDivision.Size = New System.Drawing.Size(150, 21)
        Me.cboWalletTransDivision.TabIndex = 2
        '
        'lblWalletTransDivision
        '
        Me.lblWalletTransDivision.AutoSize = True
        Me.lblWalletTransDivision.Location = New System.Drawing.Point(8, 10)
        Me.lblWalletTransDivision.Name = "lblWalletTransDivision"
        Me.lblWalletTransDivision.Size = New System.Drawing.Size(80, 13)
        Me.lblWalletTransDivision.TabIndex = 1
        Me.lblWalletTransDivision.Text = "Wallet Division:"
        '
        'clvTransactions
        '
        Me.clvTransactions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvTransactions.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colWTransDate, Me.colWTransItem, Me.colWTransQuantity, Me.colWTransPrice, Me.colWTransTotal, Me.colWTransLocation, Me.colWTransClient})
        Me.clvTransactions.ColumnSortColor = System.Drawing.Color.Lavender
        Me.clvTransactions.DefaultItemHeight = 16
        Me.clvTransactions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvTransactions.Location = New System.Drawing.Point(0, 34)
        Me.clvTransactions.MultipleColumnSort = True
        Me.clvTransactions.Name = "clvTransactions"
        Me.clvTransactions.Size = New System.Drawing.Size(1136, 507)
        Me.clvTransactions.TabIndex = 0
        '
        'colWTransDate
        '
        Me.colWTransDate.CustomSortTag = Nothing
        Me.colWTransDate.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Date]
        Me.colWTransDate.Tag = Nothing
        Me.colWTransDate.Text = "Date"
        Me.colWTransDate.Width = 125
        '
        'colWTransItem
        '
        Me.colWTransItem.CustomSortTag = Nothing
        Me.colWTransItem.DisplayIndex = 1
        Me.colWTransItem.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colWTransItem.Tag = Nothing
        Me.colWTransItem.Text = "Item"
        Me.colWTransItem.Width = 175
        '
        'colWTransQuantity
        '
        Me.colWTransQuantity.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colWTransQuantity.CustomSortTag = Nothing
        Me.colWTransQuantity.DisplayIndex = 2
        Me.colWTransQuantity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colWTransQuantity.Tag = Nothing
        Me.colWTransQuantity.Text = "Quantity"
        '
        'colWTransPrice
        '
        Me.colWTransPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colWTransPrice.CustomSortTag = Nothing
        Me.colWTransPrice.DisplayIndex = 3
        Me.colWTransPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colWTransPrice.Tag = Nothing
        Me.colWTransPrice.Text = "Price"
        Me.colWTransPrice.Width = 125
        '
        'colWTransTotal
        '
        Me.colWTransTotal.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colWTransTotal.CustomSortTag = Nothing
        Me.colWTransTotal.DisplayIndex = 4
        Me.colWTransTotal.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colWTransTotal.Tag = Nothing
        Me.colWTransTotal.Text = "Total"
        Me.colWTransTotal.Width = 125
        '
        'colWTransLocation
        '
        Me.colWTransLocation.CustomSortTag = Nothing
        Me.colWTransLocation.DisplayIndex = 5
        Me.colWTransLocation.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colWTransLocation.Tag = Nothing
        Me.colWTransLocation.Text = "Location"
        Me.colWTransLocation.Width = 300
        '
        'colWTransClient
        '
        Me.colWTransClient.CustomSortTag = Nothing
        Me.colWTransClient.DisplayIndex = 6
        Me.colWTransClient.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colWTransClient.Tag = Nothing
        Me.colWTransClient.Text = "Client"
        Me.colWTransClient.Width = 150
        '
        'tabJournal
        '
        Me.tabJournal.Controls.Add(Me.btnExportJournal)
        Me.tabJournal.Controls.Add(Me.cboWalletJournalDivision)
        Me.tabJournal.Controls.Add(Me.lblWalletJournalDivision)
        Me.tabJournal.Controls.Add(Me.clvJournal)
        Me.tabJournal.Location = New System.Drawing.Point(4, 22)
        Me.tabJournal.Name = "tabJournal"
        Me.tabJournal.Size = New System.Drawing.Size(1136, 541)
        Me.tabJournal.TabIndex = 7
        Me.tabJournal.Text = "Journal"
        Me.tabJournal.UseVisualStyleBackColor = True
        '
        'btnExportJournal
        '
        Me.btnExportJournal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportJournal.Location = New System.Drawing.Point(1053, 5)
        Me.btnExportJournal.Name = "btnExportJournal"
        Me.btnExportJournal.Size = New System.Drawing.Size(75, 23)
        Me.btnExportJournal.TabIndex = 6
        Me.btnExportJournal.Text = "Export"
        Me.btnExportJournal.UseVisualStyleBackColor = True
        '
        'cboWalletJournalDivision
        '
        Me.cboWalletJournalDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWalletJournalDivision.FormattingEnabled = True
        Me.cboWalletJournalDivision.Items.AddRange(New Object() {"1000", "1001", "1002", "1003", "1004", "1005", "1006"})
        Me.cboWalletJournalDivision.Location = New System.Drawing.Point(94, 7)
        Me.cboWalletJournalDivision.Name = "cboWalletJournalDivision"
        Me.cboWalletJournalDivision.Size = New System.Drawing.Size(150, 21)
        Me.cboWalletJournalDivision.TabIndex = 5
        '
        'lblWalletJournalDivision
        '
        Me.lblWalletJournalDivision.AutoSize = True
        Me.lblWalletJournalDivision.Location = New System.Drawing.Point(8, 10)
        Me.lblWalletJournalDivision.Name = "lblWalletJournalDivision"
        Me.lblWalletJournalDivision.Size = New System.Drawing.Size(80, 13)
        Me.lblWalletJournalDivision.TabIndex = 4
        Me.lblWalletJournalDivision.Text = "Wallet Division:"
        '
        'clvJournal
        '
        Me.clvJournal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvJournal.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colWalletJournalDate, Me.colWalletJournalType, Me.colWalletJournalAmount, Me.colWalletJournalBalance, Me.colWalletJournalDescription})
        Me.clvJournal.ColumnSortColor = System.Drawing.Color.Lavender
        Me.clvJournal.DefaultItemHeight = 16
        Me.clvJournal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvJournal.Location = New System.Drawing.Point(0, 34)
        Me.clvJournal.MultipleColumnSort = True
        Me.clvJournal.Name = "clvJournal"
        Me.clvJournal.Size = New System.Drawing.Size(1136, 507)
        Me.clvJournal.TabIndex = 3
        '
        'colWalletJournalDate
        '
        Me.colWalletJournalDate.CustomSortTag = Nothing
        Me.colWalletJournalDate.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Date]
        Me.colWalletJournalDate.Tag = Nothing
        Me.colWalletJournalDate.Text = "Date"
        Me.colWalletJournalDate.Width = 125
        '
        'colWalletJournalType
        '
        Me.colWalletJournalType.CustomSortTag = Nothing
        Me.colWalletJournalType.DisplayIndex = 1
        Me.colWalletJournalType.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colWalletJournalType.Tag = Nothing
        Me.colWalletJournalType.Text = "Type"
        Me.colWalletJournalType.Width = 175
        '
        'colWalletJournalAmount
        '
        Me.colWalletJournalAmount.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colWalletJournalAmount.CustomSortTag = Nothing
        Me.colWalletJournalAmount.DisplayIndex = 2
        Me.colWalletJournalAmount.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colWalletJournalAmount.Tag = Nothing
        Me.colWalletJournalAmount.Text = "Amount"
        Me.colWalletJournalAmount.Width = 150
        '
        'colWalletJournalBalance
        '
        Me.colWalletJournalBalance.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colWalletJournalBalance.CustomSortTag = Nothing
        Me.colWalletJournalBalance.DisplayIndex = 3
        Me.colWalletJournalBalance.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colWalletJournalBalance.Tag = Nothing
        Me.colWalletJournalBalance.Text = "Balance"
        Me.colWalletJournalBalance.Width = 150
        '
        'colWalletJournalDescription
        '
        Me.colWalletJournalDescription.CustomSortTag = Nothing
        Me.colWalletJournalDescription.DisplayIndex = 4
        Me.colWalletJournalDescription.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colWalletJournalDescription.Tag = Nothing
        Me.colWalletJournalDescription.Text = "Description"
        Me.colWalletJournalDescription.Width = 500
        '
        'tabJobs
        '
        Me.tabJobs.Controls.Add(Me.btnExportJobs)
        Me.tabJobs.Controls.Add(Me.clvJobs)
        Me.tabJobs.Location = New System.Drawing.Point(4, 22)
        Me.tabJobs.Name = "tabJobs"
        Me.tabJobs.Size = New System.Drawing.Size(1136, 541)
        Me.tabJobs.TabIndex = 8
        Me.tabJobs.Text = "Jobs"
        Me.tabJobs.UseVisualStyleBackColor = True
        '
        'btnExportJobs
        '
        Me.btnExportJobs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportJobs.Location = New System.Drawing.Point(1053, 5)
        Me.btnExportJobs.Name = "btnExportJobs"
        Me.btnExportJobs.Size = New System.Drawing.Size(75, 23)
        Me.btnExportJobs.TabIndex = 5
        Me.btnExportJobs.Text = "Export"
        Me.btnExportJobs.UseVisualStyleBackColor = True
        '
        'clvJobs
        '
        Me.clvJobs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvJobs.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colJobsItem, Me.colJobsActivity, Me.colJobsLocation, Me.colJobsEndTime, Me.colJobsStatus})
        Me.clvJobs.ColumnSortColor = System.Drawing.Color.Lavender
        Me.clvJobs.DefaultItemHeight = 16
        Me.clvJobs.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvJobs.Location = New System.Drawing.Point(0, 34)
        Me.clvJobs.MultipleColumnSort = True
        Me.clvJobs.Name = "clvJobs"
        Me.clvJobs.Size = New System.Drawing.Size(1136, 508)
        Me.clvJobs.TabIndex = 4
        '
        'colJobsItem
        '
        Me.colJobsItem.CustomSortTag = Nothing
        Me.colJobsItem.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colJobsItem.Tag = Nothing
        Me.colJobsItem.Text = "Installed Item"
        Me.colJobsItem.Width = 250
        '
        'colJobsActivity
        '
        Me.colJobsActivity.CustomSortTag = Nothing
        Me.colJobsActivity.DisplayIndex = 1
        Me.colJobsActivity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colJobsActivity.Tag = Nothing
        Me.colJobsActivity.Text = "Activity"
        Me.colJobsActivity.Width = 100
        '
        'colJobsLocation
        '
        Me.colJobsLocation.CustomSortTag = Nothing
        Me.colJobsLocation.DisplayIndex = 2
        Me.colJobsLocation.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colJobsLocation.Tag = Nothing
        Me.colJobsLocation.Text = "Location"
        Me.colJobsLocation.Width = 300
        '
        'colJobsEndTime
        '
        Me.colJobsEndTime.CustomSortTag = Nothing
        Me.colJobsEndTime.DisplayIndex = 3
        Me.colJobsEndTime.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Date]
        Me.colJobsEndTime.Tag = Nothing
        Me.colJobsEndTime.Text = "End Time"
        Me.colJobsEndTime.Width = 125
        '
        'colJobsStatus
        '
        Me.colJobsStatus.CustomSortTag = Nothing
        Me.colJobsStatus.DisplayIndex = 4
        Me.colJobsStatus.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colJobsStatus.Tag = Nothing
        Me.colJobsStatus.Text = "Status"
        Me.colJobsStatus.Width = 100
        '
        'tabRecycle
        '
        Me.tabRecycle.Controls.Add(Me.chkFeesOnItems)
        Me.tabRecycle.Controls.Add(Me.lblPriceTotals)
        Me.tabRecycle.Controls.Add(Me.chkFeesOnRefine)
        Me.tabRecycle.Controls.Add(Me.lblTotalFees)
        Me.tabRecycle.Controls.Add(Me.lblTotalFeesLbl)
        Me.tabRecycle.Controls.Add(Me.nudTax)
        Me.tabRecycle.Controls.Add(Me.nudBrokerFee)
        Me.tabRecycle.Controls.Add(Me.chkOverrideTax)
        Me.tabRecycle.Controls.Add(Me.chkOverrideBrokerFee)
        Me.tabRecycle.Controls.Add(Me.lblItems)
        Me.tabRecycle.Controls.Add(Me.lblVolume)
        Me.tabRecycle.Controls.Add(Me.lblItemsLbl)
        Me.tabRecycle.Controls.Add(Me.lblVolumeLbl)
        Me.tabRecycle.Controls.Add(Me.cboRefineMode)
        Me.tabRecycle.Controls.Add(Me.lblRefineMode)
        Me.tabRecycle.Controls.Add(Me.chkOverrideStandings)
        Me.tabRecycle.Controls.Add(Me.chkOverrideBaseYield)
        Me.tabRecycle.Controls.Add(Me.nudStandings)
        Me.tabRecycle.Controls.Add(Me.nudBaseYield)
        Me.tabRecycle.Controls.Add(Me.lblCorp)
        Me.tabRecycle.Controls.Add(Me.lblCorpLbl)
        Me.tabRecycle.Controls.Add(Me.lblStation)
        Me.tabRecycle.Controls.Add(Me.lblStationLbl)
        Me.tabRecycle.Controls.Add(Me.lblBaseYield)
        Me.tabRecycle.Controls.Add(Me.lblNetYield)
        Me.tabRecycle.Controls.Add(Me.lblStandings)
        Me.tabRecycle.Controls.Add(Me.lblStationTake)
        Me.tabRecycle.Controls.Add(Me.lblStationTakeLbl)
        Me.tabRecycle.Controls.Add(Me.lblStandingsLbl)
        Me.tabRecycle.Controls.Add(Me.lblNetYieldLbl)
        Me.tabRecycle.Controls.Add(Me.lblBaseYieldLbl)
        Me.tabRecycle.Controls.Add(Me.chkPerfectRefine)
        Me.tabRecycle.Controls.Add(Me.cboRecyclePilots)
        Me.tabRecycle.Controls.Add(Me.lblPilot)
        Me.tabRecycle.Controls.Add(Me.TabControl1)
        Me.tabRecycle.Location = New System.Drawing.Point(4, 22)
        Me.tabRecycle.Name = "tabRecycle"
        Me.tabRecycle.Size = New System.Drawing.Size(1136, 541)
        Me.tabRecycle.TabIndex = 9
        Me.tabRecycle.Text = "Recycler"
        Me.tabRecycle.UseVisualStyleBackColor = True
        '
        'chkFeesOnItems
        '
        Me.chkFeesOnItems.AutoSize = True
        Me.chkFeesOnItems.Location = New System.Drawing.Point(517, 43)
        Me.chkFeesOnItems.Name = "chkFeesOnItems"
        Me.chkFeesOnItems.Size = New System.Drawing.Size(94, 17)
        Me.chkFeesOnItems.TabIndex = 61
        Me.chkFeesOnItems.Text = "Fees on Items"
        Me.chkFeesOnItems.UseVisualStyleBackColor = True
        '
        'lblPriceTotals
        '
        Me.lblPriceTotals.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPriceTotals.AutoSize = True
        Me.lblPriceTotals.Location = New System.Drawing.Point(7, 522)
        Me.lblPriceTotals.Name = "lblPriceTotals"
        Me.lblPriceTotals.Size = New System.Drawing.Size(135, 13)
        Me.lblPriceTotals.TabIndex = 60
        Me.lblPriceTotals.Text = "Sale / Refine / Best Totals:"
        '
        'chkFeesOnRefine
        '
        Me.chkFeesOnRefine.AutoSize = True
        Me.chkFeesOnRefine.Location = New System.Drawing.Point(644, 43)
        Me.chkFeesOnRefine.Name = "chkFeesOnRefine"
        Me.chkFeesOnRefine.Size = New System.Drawing.Size(98, 17)
        Me.chkFeesOnRefine.TabIndex = 59
        Me.chkFeesOnRefine.Text = "Fees on Refine"
        Me.chkFeesOnRefine.UseVisualStyleBackColor = True
        '
        'lblTotalFees
        '
        Me.lblTotalFees.AutoSize = True
        Me.lblTotalFees.Location = New System.Drawing.Point(839, 99)
        Me.lblTotalFees.Name = "lblTotalFees"
        Me.lblTotalFees.Size = New System.Drawing.Size(24, 13)
        Me.lblTotalFees.TabIndex = 58
        Me.lblTotalFees.Text = "0%"
        '
        'lblTotalFeesLbl
        '
        Me.lblTotalFeesLbl.AutoSize = True
        Me.lblTotalFeesLbl.Location = New System.Drawing.Point(762, 99)
        Me.lblTotalFeesLbl.Name = "lblTotalFeesLbl"
        Me.lblTotalFeesLbl.Size = New System.Drawing.Size(61, 13)
        Me.lblTotalFeesLbl.TabIndex = 57
        Me.lblTotalFeesLbl.Text = "Total Fees:"
        '
        'nudTax
        '
        Me.nudTax.DecimalPlaces = 4
        Me.nudTax.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudTax.Location = New System.Drawing.Point(668, 95)
        Me.nudTax.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudTax.Name = "nudTax"
        Me.nudTax.Size = New System.Drawing.Size(74, 21)
        Me.nudTax.TabIndex = 56
        '
        'nudBrokerFee
        '
        Me.nudBrokerFee.DecimalPlaces = 4
        Me.nudBrokerFee.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudBrokerFee.Location = New System.Drawing.Point(668, 68)
        Me.nudBrokerFee.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudBrokerFee.Name = "nudBrokerFee"
        Me.nudBrokerFee.Size = New System.Drawing.Size(74, 21)
        Me.nudBrokerFee.TabIndex = 55
        '
        'chkOverrideTax
        '
        Me.chkOverrideTax.AutoSize = True
        Me.chkOverrideTax.Location = New System.Drawing.Point(517, 98)
        Me.chkOverrideTax.Name = "chkOverrideTax"
        Me.chkOverrideTax.Size = New System.Drawing.Size(111, 17)
        Me.chkOverrideTax.TabIndex = 54
        Me.chkOverrideTax.Text = "Override Tax (%)"
        Me.chkOverrideTax.UseVisualStyleBackColor = True
        '
        'chkOverrideBrokerFee
        '
        Me.chkOverrideBrokerFee.AutoSize = True
        Me.chkOverrideBrokerFee.Location = New System.Drawing.Point(517, 69)
        Me.chkOverrideBrokerFee.Name = "chkOverrideBrokerFee"
        Me.chkOverrideBrokerFee.Size = New System.Drawing.Size(145, 17)
        Me.chkOverrideBrokerFee.TabIndex = 53
        Me.chkOverrideBrokerFee.Text = "Override Broker Fee (%)"
        Me.chkOverrideBrokerFee.UseVisualStyleBackColor = True
        '
        'lblItems
        '
        Me.lblItems.AutoSize = True
        Me.lblItems.Location = New System.Drawing.Point(63, 99)
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(13, 13)
        Me.lblItems.TabIndex = 52
        Me.lblItems.Text = "0"
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.Location = New System.Drawing.Point(63, 78)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(29, 13)
        Me.lblVolume.TabIndex = 51
        Me.lblVolume.Text = "0 m³"
        '
        'lblItemsLbl
        '
        Me.lblItemsLbl.AutoSize = True
        Me.lblItemsLbl.Location = New System.Drawing.Point(12, 99)
        Me.lblItemsLbl.Name = "lblItemsLbl"
        Me.lblItemsLbl.Size = New System.Drawing.Size(38, 13)
        Me.lblItemsLbl.TabIndex = 50
        Me.lblItemsLbl.Text = "Items:"
        '
        'lblVolumeLbl
        '
        Me.lblVolumeLbl.AutoSize = True
        Me.lblVolumeLbl.Location = New System.Drawing.Point(12, 78)
        Me.lblVolumeLbl.Name = "lblVolumeLbl"
        Me.lblVolumeLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblVolumeLbl.TabIndex = 49
        Me.lblVolumeLbl.Text = "Volume:"
        '
        'cboRefineMode
        '
        Me.cboRefineMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRefineMode.FormattingEnabled = True
        Me.cboRefineMode.Items.AddRange(New Object() {"Standard (Station)", "Refining Array", "Intensive Refining Array"})
        Me.cboRefineMode.Location = New System.Drawing.Point(48, 13)
        Me.cboRefineMode.Name = "cboRefineMode"
        Me.cboRefineMode.Size = New System.Drawing.Size(165, 21)
        Me.cboRefineMode.TabIndex = 48
        '
        'lblRefineMode
        '
        Me.lblRefineMode.AutoSize = True
        Me.lblRefineMode.Location = New System.Drawing.Point(12, 16)
        Me.lblRefineMode.Name = "lblRefineMode"
        Me.lblRefineMode.Size = New System.Drawing.Size(37, 13)
        Me.lblRefineMode.TabIndex = 47
        Me.lblRefineMode.Text = "Mode:"
        '
        'chkOverrideStandings
        '
        Me.chkOverrideStandings.AutoSize = True
        Me.chkOverrideStandings.Location = New System.Drawing.Point(259, 96)
        Me.chkOverrideStandings.Name = "chkOverrideStandings"
        Me.chkOverrideStandings.Size = New System.Drawing.Size(118, 17)
        Me.chkOverrideStandings.TabIndex = 46
        Me.chkOverrideStandings.Text = "Override Standings"
        Me.chkOverrideStandings.UseVisualStyleBackColor = True
        '
        'chkOverrideBaseYield
        '
        Me.chkOverrideBaseYield.AutoSize = True
        Me.chkOverrideBaseYield.Location = New System.Drawing.Point(259, 69)
        Me.chkOverrideBaseYield.Name = "chkOverrideBaseYield"
        Me.chkOverrideBaseYield.Size = New System.Drawing.Size(119, 17)
        Me.chkOverrideBaseYield.TabIndex = 45
        Me.chkOverrideBaseYield.Text = "Override Base Yield"
        Me.chkOverrideBaseYield.UseVisualStyleBackColor = True
        '
        'nudStandings
        '
        Me.nudStandings.DecimalPlaces = 4
        Me.nudStandings.Location = New System.Drawing.Point(410, 95)
        Me.nudStandings.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudStandings.Name = "nudStandings"
        Me.nudStandings.Size = New System.Drawing.Size(74, 21)
        Me.nudStandings.TabIndex = 44
        '
        'nudBaseYield
        '
        Me.nudBaseYield.DecimalPlaces = 2
        Me.nudBaseYield.Location = New System.Drawing.Point(410, 68)
        Me.nudBaseYield.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudBaseYield.Name = "nudBaseYield"
        Me.nudBaseYield.Size = New System.Drawing.Size(74, 21)
        Me.nudBaseYield.TabIndex = 43
        Me.nudBaseYield.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'lblCorp
        '
        Me.lblCorp.AutoSize = True
        Me.lblCorp.Location = New System.Drawing.Point(305, 21)
        Me.lblCorp.Name = "lblCorp"
        Me.lblCorp.Size = New System.Drawing.Size(23, 13)
        Me.lblCorp.TabIndex = 42
        Me.lblCorp.Text = "n/a"
        '
        'lblCorpLbl
        '
        Me.lblCorpLbl.AutoSize = True
        Me.lblCorpLbl.Location = New System.Drawing.Point(256, 21)
        Me.lblCorpLbl.Name = "lblCorpLbl"
        Me.lblCorpLbl.Size = New System.Drawing.Size(34, 13)
        Me.lblCorpLbl.TabIndex = 41
        Me.lblCorpLbl.Text = "Corp:"
        '
        'lblStation
        '
        Me.lblStation.AutoSize = True
        Me.lblStation.Location = New System.Drawing.Point(305, 8)
        Me.lblStation.Name = "lblStation"
        Me.lblStation.Size = New System.Drawing.Size(23, 13)
        Me.lblStation.TabIndex = 40
        Me.lblStation.Text = "n/a"
        '
        'lblStationLbl
        '
        Me.lblStationLbl.AutoSize = True
        Me.lblStationLbl.Location = New System.Drawing.Point(256, 8)
        Me.lblStationLbl.Name = "lblStationLbl"
        Me.lblStationLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblStationLbl.TabIndex = 39
        Me.lblStationLbl.Text = "Station:"
        '
        'lblBaseYield
        '
        Me.lblBaseYield.AutoSize = True
        Me.lblBaseYield.Location = New System.Drawing.Point(839, 47)
        Me.lblBaseYield.Name = "lblBaseYield"
        Me.lblBaseYield.Size = New System.Drawing.Size(24, 13)
        Me.lblBaseYield.TabIndex = 38
        Me.lblBaseYield.Text = "0%"
        '
        'lblNetYield
        '
        Me.lblNetYield.AutoSize = True
        Me.lblNetYield.Location = New System.Drawing.Point(839, 60)
        Me.lblNetYield.Name = "lblNetYield"
        Me.lblNetYield.Size = New System.Drawing.Size(24, 13)
        Me.lblNetYield.TabIndex = 37
        Me.lblNetYield.Text = "0%"
        '
        'lblStandings
        '
        Me.lblStandings.AutoSize = True
        Me.lblStandings.Location = New System.Drawing.Point(839, 73)
        Me.lblStandings.Name = "lblStandings"
        Me.lblStandings.Size = New System.Drawing.Size(13, 13)
        Me.lblStandings.TabIndex = 36
        Me.lblStandings.Text = "0"
        '
        'lblStationTake
        '
        Me.lblStationTake.AutoSize = True
        Me.lblStationTake.Location = New System.Drawing.Point(839, 86)
        Me.lblStationTake.Name = "lblStationTake"
        Me.lblStationTake.Size = New System.Drawing.Size(24, 13)
        Me.lblStationTake.TabIndex = 35
        Me.lblStationTake.Text = "0%"
        '
        'lblStationTakeLbl
        '
        Me.lblStationTakeLbl.AutoSize = True
        Me.lblStationTakeLbl.Location = New System.Drawing.Point(762, 86)
        Me.lblStationTakeLbl.Name = "lblStationTakeLbl"
        Me.lblStationTakeLbl.Size = New System.Drawing.Size(71, 13)
        Me.lblStationTakeLbl.TabIndex = 34
        Me.lblStationTakeLbl.Text = "Station Take:"
        '
        'lblStandingsLbl
        '
        Me.lblStandingsLbl.AutoSize = True
        Me.lblStandingsLbl.Location = New System.Drawing.Point(762, 73)
        Me.lblStandingsLbl.Name = "lblStandingsLbl"
        Me.lblStandingsLbl.Size = New System.Drawing.Size(58, 13)
        Me.lblStandingsLbl.TabIndex = 33
        Me.lblStandingsLbl.Text = "Standings:"
        '
        'lblNetYieldLbl
        '
        Me.lblNetYieldLbl.AutoSize = True
        Me.lblNetYieldLbl.Location = New System.Drawing.Point(762, 60)
        Me.lblNetYieldLbl.Name = "lblNetYieldLbl"
        Me.lblNetYieldLbl.Size = New System.Drawing.Size(53, 13)
        Me.lblNetYieldLbl.TabIndex = 32
        Me.lblNetYieldLbl.Text = "Net Yield:"
        '
        'lblBaseYieldLbl
        '
        Me.lblBaseYieldLbl.AutoSize = True
        Me.lblBaseYieldLbl.Location = New System.Drawing.Point(762, 47)
        Me.lblBaseYieldLbl.Name = "lblBaseYieldLbl"
        Me.lblBaseYieldLbl.Size = New System.Drawing.Size(59, 13)
        Me.lblBaseYieldLbl.TabIndex = 31
        Me.lblBaseYieldLbl.Text = "Base Yield:"
        '
        'chkPerfectRefine
        '
        Me.chkPerfectRefine.AutoSize = True
        Me.chkPerfectRefine.Location = New System.Drawing.Point(259, 43)
        Me.chkPerfectRefine.Name = "chkPerfectRefine"
        Me.chkPerfectRefine.Size = New System.Drawing.Size(95, 17)
        Me.chkPerfectRefine.TabIndex = 30
        Me.chkPerfectRefine.Text = "Perfect Refine"
        Me.chkPerfectRefine.UseVisualStyleBackColor = True
        '
        'cboRecyclePilots
        '
        Me.cboRecyclePilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRecyclePilots.FormattingEnabled = True
        Me.cboRecyclePilots.Location = New System.Drawing.Point(48, 44)
        Me.cboRecyclePilots.Name = "cboRecyclePilots"
        Me.cboRecyclePilots.Size = New System.Drawing.Size(165, 21)
        Me.cboRecyclePilots.Sorted = True
        Me.cboRecyclePilots.TabIndex = 29
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(12, 47)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 28
        Me.lblPilot.Text = "Pilot:"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.tabItems)
        Me.TabControl1.Controls.Add(Me.tabTotals)
        Me.TabControl1.Location = New System.Drawing.Point(3, 122)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1130, 397)
        Me.TabControl1.TabIndex = 27
        '
        'tabItems
        '
        Me.tabItems.Controls.Add(Me.clvRecycle)
        Me.tabItems.Location = New System.Drawing.Point(4, 22)
        Me.tabItems.Name = "tabItems"
        Me.tabItems.Padding = New System.Windows.Forms.Padding(3)
        Me.tabItems.Size = New System.Drawing.Size(1122, 371)
        Me.tabItems.TabIndex = 0
        Me.tabItems.Text = "Item Analysis"
        Me.tabItems.UseVisualStyleBackColor = True
        '
        'clvRecycle
        '
        Me.clvRecycle.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colRecycleItem, Me.colRecycleMetaLevel, Me.colRecycleQuantity, Me.colBatches, Me.colItemPrice, Me.colTotalPrice, Me.colFees, Me.colSalePrice, Me.colRefinePrice, Me.colBestPrice})
        Me.clvRecycle.ContextMenuStrip = Me.ctxRecycleItems
        Me.clvRecycle.DefaultItemHeight = 20
        Me.clvRecycle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvRecycle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvRecycle.ItemContextMenu = Me.ctxRecycleItem
        Me.clvRecycle.Location = New System.Drawing.Point(3, 3)
        Me.clvRecycle.MultipleColumnSort = True
        Me.clvRecycle.Name = "clvRecycle"
        Me.clvRecycle.Size = New System.Drawing.Size(1116, 365)
        Me.clvRecycle.TabIndex = 0
        '
        'colRecycleItem
        '
        Me.colRecycleItem.CustomSortTag = Nothing
        Me.colRecycleItem.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colRecycleItem.Tag = Nothing
        Me.colRecycleItem.Text = "Item"
        Me.colRecycleItem.Width = 300
        '
        'colRecycleMetaLevel
        '
        Me.colRecycleMetaLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colRecycleMetaLevel.CustomSortTag = Nothing
        Me.colRecycleMetaLevel.DisplayIndex = 1
        Me.colRecycleMetaLevel.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colRecycleMetaLevel.Tag = Nothing
        Me.colRecycleMetaLevel.Text = "Meta Level"
        '
        'colRecycleQuantity
        '
        Me.colRecycleQuantity.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colRecycleQuantity.CustomSortTag = Nothing
        Me.colRecycleQuantity.DisplayIndex = 2
        Me.colRecycleQuantity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colRecycleQuantity.Tag = Nothing
        Me.colRecycleQuantity.Text = "Quantity"
        Me.colRecycleQuantity.Width = 75
        '
        'colBatches
        '
        Me.colBatches.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colBatches.CustomSortTag = Nothing
        Me.colBatches.DisplayIndex = 3
        Me.colBatches.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBatches.Tag = Nothing
        Me.colBatches.Text = "Batches"
        '
        'colItemPrice
        '
        Me.colItemPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colItemPrice.CustomSortTag = Nothing
        Me.colItemPrice.DisplayIndex = 4
        Me.colItemPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colItemPrice.Tag = Nothing
        Me.colItemPrice.Text = "Item Price"
        Me.colItemPrice.Width = 100
        '
        'colTotalPrice
        '
        Me.colTotalPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colTotalPrice.CustomSortTag = Nothing
        Me.colTotalPrice.DisplayIndex = 5
        Me.colTotalPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colTotalPrice.Tag = Nothing
        Me.colTotalPrice.Text = "Total Price"
        Me.colTotalPrice.Width = 100
        '
        'colFees
        '
        Me.colFees.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colFees.CustomSortTag = Nothing
        Me.colFees.DisplayIndex = 6
        Me.colFees.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colFees.Tag = Nothing
        Me.colFees.Text = "Fees"
        Me.colFees.Width = 100
        '
        'colSalePrice
        '
        Me.colSalePrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colSalePrice.CustomSortTag = Nothing
        Me.colSalePrice.DisplayIndex = 7
        Me.colSalePrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colSalePrice.Tag = Nothing
        Me.colSalePrice.Text = "Sale Price"
        Me.colSalePrice.Width = 100
        '
        'colRefinePrice
        '
        Me.colRefinePrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colRefinePrice.CustomSortTag = Nothing
        Me.colRefinePrice.DisplayIndex = 8
        Me.colRefinePrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colRefinePrice.Tag = Nothing
        Me.colRefinePrice.Text = "Refine Price"
        Me.colRefinePrice.Width = 100
        '
        'colBestPrice
        '
        Me.colBestPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBestPrice.CustomSortTag = Nothing
        Me.colBestPrice.DisplayIndex = 9
        Me.colBestPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBestPrice.Tag = Nothing
        Me.colBestPrice.Text = "Best Price"
        Me.colBestPrice.Width = 100
        '
        'ctxRecycleItems
        '
        Me.ctxRecycleItems.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddRecycleItem})
        Me.ctxRecycleItems.Name = "ctxRecycleItem"
        Me.ctxRecycleItems.Size = New System.Drawing.Size(124, 26)
        '
        'mnuAddRecycleItem
        '
        Me.mnuAddRecycleItem.Name = "mnuAddRecycleItem"
        Me.mnuAddRecycleItem.Size = New System.Drawing.Size(123, 22)
        Me.mnuAddRecycleItem.Text = "Add Item"
        '
        'ctxRecycleItem
        '
        Me.ctxRecycleItem.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAlterRecycleQuantity, Me.ToolStripMenuItem2, Me.mnuRemoveRecycleItem})
        Me.ctxRecycleItem.Name = "ctxRecycleItem"
        Me.ctxRecycleItem.Size = New System.Drawing.Size(149, 54)
        '
        'mnuAlterRecycleQuantity
        '
        Me.mnuAlterRecycleQuantity.Name = "mnuAlterRecycleQuantity"
        Me.mnuAlterRecycleQuantity.Size = New System.Drawing.Size(148, 22)
        Me.mnuAlterRecycleQuantity.Text = "Alter Quantity"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(145, 6)
        '
        'mnuRemoveRecycleItem
        '
        Me.mnuRemoveRecycleItem.Name = "mnuRemoveRecycleItem"
        Me.mnuRemoveRecycleItem.Size = New System.Drawing.Size(148, 22)
        Me.mnuRemoveRecycleItem.Text = "Remove Item"
        '
        'tabTotals
        '
        Me.tabTotals.Controls.Add(Me.clvTotals)
        Me.tabTotals.Location = New System.Drawing.Point(4, 22)
        Me.tabTotals.Name = "tabTotals"
        Me.tabTotals.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTotals.Size = New System.Drawing.Size(1122, 371)
        Me.tabTotals.TabIndex = 1
        Me.tabTotals.Text = "Recycling Totals"
        Me.tabTotals.UseVisualStyleBackColor = True
        '
        'clvTotals
        '
        Me.clvTotals.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colMaterial, Me.colStationTake, Me.colWaste, Me.colReceive, Me.colMatPrice, Me.colMatTotal})
        Me.clvTotals.DefaultItemHeight = 20
        Me.clvTotals.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvTotals.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvTotals.Location = New System.Drawing.Point(3, 3)
        Me.clvTotals.MultipleColumnSort = True
        Me.clvTotals.Name = "clvTotals"
        Me.clvTotals.Size = New System.Drawing.Size(1116, 365)
        Me.clvTotals.TabIndex = 1
        '
        'colMaterial
        '
        Me.colMaterial.CustomSortTag = Nothing
        Me.colMaterial.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colMaterial.Tag = Nothing
        Me.colMaterial.Text = "Material"
        Me.colMaterial.Width = 300
        '
        'colStationTake
        '
        Me.colStationTake.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colStationTake.CustomSortTag = Nothing
        Me.colStationTake.DisplayIndex = 1
        Me.colStationTake.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colStationTake.Tag = Nothing
        Me.colStationTake.Text = "Station Take"
        Me.colStationTake.Width = 100
        '
        'colWaste
        '
        Me.colWaste.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colWaste.CustomSortTag = Nothing
        Me.colWaste.DisplayIndex = 2
        Me.colWaste.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colWaste.Tag = Nothing
        Me.colWaste.Text = "Unrecoverable"
        Me.colWaste.Width = 100
        '
        'colReceive
        '
        Me.colReceive.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colReceive.CustomSortTag = Nothing
        Me.colReceive.DisplayIndex = 3
        Me.colReceive.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colReceive.Tag = Nothing
        Me.colReceive.Text = "Receivable"
        Me.colReceive.Width = 100
        '
        'colMatPrice
        '
        Me.colMatPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colMatPrice.CustomSortTag = Nothing
        Me.colMatPrice.DisplayIndex = 4
        Me.colMatPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colMatPrice.Tag = Nothing
        Me.colMatPrice.Text = "Price"
        '
        'colMatTotal
        '
        Me.colMatTotal.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colMatTotal.CustomSortTag = Nothing
        Me.colMatTotal.DisplayIndex = 5
        Me.colMatTotal.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colMatTotal.Tag = Nothing
        Me.colMatTotal.Text = "Total"
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
        Me.tssLabelTotalAssetsLabel.Size = New System.Drawing.Size(154, 17)
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
        Me.tssLabelSelectedAssetsLabel.Size = New System.Drawing.Size(162, 17)
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
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbDownloadData, Me.ToolStripSeparator1, Me.lblOwner, Me.cboOwner, Me.ToolStripSeparator2, Me.tsbAssets, Me.ToolStripSeparator3, Me.tsbInvestments, Me.ToolStripSeparator4, Me.tsbRigBuilder, Me.ToolStripSeparator5, Me.tsbOrders, Me.ToolStripSeparator6, Me.tsbTransactions, Me.ToolStripSeparator7, Me.tsbJournal, Me.ToolStripSeparator8, Me.tsbJobs, Me.ToolStripSeparator9, Me.tsbRecycle, Me.ToolStripSeparator10, Me.tsbReports})
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
        Me.tsbDownloadData.Size = New System.Drawing.Size(113, 22)
        Me.tsbDownloadData.Text = "Download API Data"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'lblOwner
        '
        Me.lblOwner.Name = "lblOwner"
        Me.lblOwner.Size = New System.Drawing.Size(45, 22)
        Me.lblOwner.Text = "Owner:"
        '
        'cboOwner
        '
        Me.cboOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOwner.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.cboOwner.MaxDropDownItems = 12
        Me.cboOwner.Name = "cboOwner"
        Me.cboOwner.Size = New System.Drawing.Size(200, 25)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsbAssets
        '
        Me.tsbAssets.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbAssets.Image = CType(resources.GetObject("tsbAssets.Image"), System.Drawing.Image)
        Me.tsbAssets.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAssets.Name = "tsbAssets"
        Me.tsbAssets.Size = New System.Drawing.Size(44, 22)
        Me.tsbAssets.Text = "Assets"
        Me.tsbAssets.ToolTipText = "Displays the Assets tab"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'tsbInvestments
        '
        Me.tsbInvestments.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbInvestments.Image = CType(resources.GetObject("tsbInvestments.Image"), System.Drawing.Image)
        Me.tsbInvestments.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbInvestments.Name = "tsbInvestments"
        Me.tsbInvestments.Size = New System.Drawing.Size(75, 22)
        Me.tsbInvestments.Text = "Investments"
        Me.tsbInvestments.ToolTipText = "Displays investments"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'tsbRigBuilder
        '
        Me.tsbRigBuilder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbRigBuilder.Image = CType(resources.GetObject("tsbRigBuilder.Image"), System.Drawing.Image)
        Me.tsbRigBuilder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRigBuilder.Name = "tsbRigBuilder"
        Me.tsbRigBuilder.Size = New System.Drawing.Size(68, 22)
        Me.tsbRigBuilder.Text = "Rig Builder"
        Me.tsbRigBuilder.ToolTipText = "Displays the rig builder"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'tsbOrders
        '
        Me.tsbOrders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbOrders.Image = CType(resources.GetObject("tsbOrders.Image"), System.Drawing.Image)
        Me.tsbOrders.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbOrders.Name = "tsbOrders"
        Me.tsbOrders.Size = New System.Drawing.Size(46, 22)
        Me.tsbOrders.Text = "Orders"
        Me.tsbOrders.ToolTipText = "Displays the market orders"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'tsbTransactions
        '
        Me.tsbTransactions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbTransactions.Image = CType(resources.GetObject("tsbTransactions.Image"), System.Drawing.Image)
        Me.tsbTransactions.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbTransactions.Name = "tsbTransactions"
        Me.tsbTransactions.Size = New System.Drawing.Size(78, 22)
        Me.tsbTransactions.Text = "Transactions"
        Me.tsbTransactions.ToolTipText = "Displays the wallet transactions"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'tsbJournal
        '
        Me.tsbJournal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbJournal.Image = CType(resources.GetObject("tsbJournal.Image"), System.Drawing.Image)
        Me.tsbJournal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbJournal.Name = "tsbJournal"
        Me.tsbJournal.Size = New System.Drawing.Size(49, 22)
        Me.tsbJournal.Text = "Journal"
        Me.tsbJournal.ToolTipText = "Displays the wallet journal"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'tsbJobs
        '
        Me.tsbJobs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbJobs.Image = CType(resources.GetObject("tsbJobs.Image"), System.Drawing.Image)
        Me.tsbJobs.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbJobs.Name = "tsbJobs"
        Me.tsbJobs.Size = New System.Drawing.Size(34, 22)
        Me.tsbJobs.Text = "Jobs"
        Me.tsbJobs.ToolTipText = "Displays the Blueprint jobs"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'tsbRecycle
        '
        Me.tsbRecycle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbRecycle.Image = CType(resources.GetObject("tsbRecycle.Image"), System.Drawing.Image)
        Me.tsbRecycle.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRecycle.Name = "tsbRecycle"
        Me.tsbRecycle.Size = New System.Drawing.Size(55, 22)
        Me.tsbRecycle.Text = "Recycler"
        Me.tsbRecycle.ToolTipText = "Displays the Recycler tab"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 25)
        '
        'tsbReports
        '
        Me.tsbReports.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLocation, Me.mnuAssetLists})
        Me.tsbReports.Image = CType(resources.GetObject("tsbReports.Image"), System.Drawing.Image)
        Me.tsbReports.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReports.Name = "tsbReports"
        Me.tsbReports.Size = New System.Drawing.Size(63, 22)
        Me.tsbReports.Text = "Reports"
        '
        'mnuLocation
        '
        Me.mnuLocation.Name = "mnuLocation"
        Me.mnuLocation.Size = New System.Drawing.Size(185, 22)
        Me.mnuLocation.Text = "Grouped by Location"
        '
        'mnuAssetLists
        '
        Me.mnuAssetLists.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAssetListName, Me.mnuAssetListQuantity, Me.mnuAssetListPrice, Me.mnuAssetListValue})
        Me.mnuAssetLists.Name = "mnuAssetLists"
        Me.mnuAssetLists.Size = New System.Drawing.Size(185, 22)
        Me.mnuAssetLists.Text = "Asset Lists"
        '
        'mnuAssetListName
        '
        Me.mnuAssetListName.Name = "mnuAssetListName"
        Me.mnuAssetListName.Size = New System.Drawing.Size(193, 22)
        Me.mnuAssetListName.Text = "Asset List (Name)"
        '
        'mnuAssetListQuantity
        '
        Me.mnuAssetListQuantity.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAssetListQuantityA, Me.mnuAssetListQuantityD})
        Me.mnuAssetListQuantity.Name = "mnuAssetListQuantity"
        Me.mnuAssetListQuantity.Size = New System.Drawing.Size(193, 22)
        Me.mnuAssetListQuantity.Text = "Asset List (Quantity)"
        '
        'mnuAssetListQuantityA
        '
        Me.mnuAssetListQuantityA.Name = "mnuAssetListQuantityA"
        Me.mnuAssetListQuantityA.Size = New System.Drawing.Size(136, 22)
        Me.mnuAssetListQuantityA.Text = "Ascending"
        '
        'mnuAssetListQuantityD
        '
        Me.mnuAssetListQuantityD.Name = "mnuAssetListQuantityD"
        Me.mnuAssetListQuantityD.Size = New System.Drawing.Size(136, 22)
        Me.mnuAssetListQuantityD.Text = "Descending"
        '
        'mnuAssetListPrice
        '
        Me.mnuAssetListPrice.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAssetListPriceA, Me.mnuAssetListPriceD})
        Me.mnuAssetListPrice.Name = "mnuAssetListPrice"
        Me.mnuAssetListPrice.Size = New System.Drawing.Size(193, 22)
        Me.mnuAssetListPrice.Text = "Asset List (Unit Price)"
        '
        'mnuAssetListPriceA
        '
        Me.mnuAssetListPriceA.Name = "mnuAssetListPriceA"
        Me.mnuAssetListPriceA.Size = New System.Drawing.Size(136, 22)
        Me.mnuAssetListPriceA.Text = "Ascending"
        '
        'mnuAssetListPriceD
        '
        Me.mnuAssetListPriceD.Name = "mnuAssetListPriceD"
        Me.mnuAssetListPriceD.Size = New System.Drawing.Size(136, 22)
        Me.mnuAssetListPriceD.Text = "Descending"
        '
        'mnuAssetListValue
        '
        Me.mnuAssetListValue.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAssetListValueA, Me.mnuAssetListValueD})
        Me.mnuAssetListValue.Name = "mnuAssetListValue"
        Me.mnuAssetListValue.Size = New System.Drawing.Size(193, 22)
        Me.mnuAssetListValue.Text = "Asset List (Total Value)"
        '
        'mnuAssetListValueA
        '
        Me.mnuAssetListValueA.Name = "mnuAssetListValueA"
        Me.mnuAssetListValueA.Size = New System.Drawing.Size(136, 22)
        Me.mnuAssetListValueA.Text = "Ascending"
        '
        'mnuAssetListValueD
        '
        Me.mnuAssetListValueD.Name = "mnuAssetListValueD"
        Me.mnuAssetListValueD.Size = New System.Drawing.Size(136, 22)
        Me.mnuAssetListValueD.Text = "Descending"
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Type"
        Me.ColumnHeader1.Width = 222
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Quantity"
        Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader2.Width = 67
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Price"
        Me.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader3.Width = 104
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Location"
        Me.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader4.Width = 194
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Range"
        Me.ColumnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader5.Width = 51
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Min Volume"
        Me.ColumnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Expires In"
        Me.ColumnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Type"
        Me.ColumnHeader8.Width = 219
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Quantity"
        Me.ColumnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader9.Width = 65
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Price"
        Me.ColumnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader10.Width = 107
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Location"
        Me.ColumnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader11.Width = 188
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Expires In"
        Me.ColumnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader12.Width = 119
        '
        'mnuRemoveCustomName
        '
        Me.mnuRemoveCustomName.Name = "mnuRemoveCustomName"
        Me.mnuRemoveCustomName.Size = New System.Drawing.Size(197, 22)
        Me.mnuRemoveCustomName.Text = "Remove Custom Name"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(194, 6)
        '
        'frmPrism
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1144, 620)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.tabPrism)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPrism"
        Me.Text = "EveHQ Prism"
        Me.ctxAssets.ResumeLayout(False)
        Me.ctxFilter.ResumeLayout(False)
        Me.ctxFilterList.ResumeLayout(False)
        Me.tabPrism.ResumeLayout(False)
        Me.ctxTabPrism.ResumeLayout(False)
        Me.tabAPIStatus.ResumeLayout(False)
        Me.tabAPIStatus.PerformLayout()
        Me.tabAssets.ResumeLayout(False)
        Me.tabAssets.PerformLayout()
        Me.tabAssetFilters.ResumeLayout(False)
        Me.tabAssetFilters.PerformLayout()
        Me.tabInvestments.ResumeLayout(False)
        Me.tabInvestments.PerformLayout()
        Me.tabRigBuilder.ResumeLayout(False)
        Me.tabRigBuilder.PerformLayout()
        Me.bgAutoRig.ResumeLayout(False)
        Me.bgAutoRig.PerformLayout()
        CType(Me.nudRigMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabOrders.ResumeLayout(False)
        Me.scMarketOrders.Panel1.ResumeLayout(False)
        Me.scMarketOrders.Panel1.PerformLayout()
        Me.scMarketOrders.Panel2.ResumeLayout(False)
        Me.scMarketOrders.Panel2.PerformLayout()
        Me.scMarketOrders.ResumeLayout(False)
        Me.panelOrderInfo.ResumeLayout(False)
        Me.panelOrderInfo.PerformLayout()
        Me.tabTransactions.ResumeLayout(False)
        Me.tabTransactions.PerformLayout()
        Me.tabJournal.ResumeLayout(False)
        Me.tabJournal.PerformLayout()
        Me.tabJobs.ResumeLayout(False)
        Me.tabRecycle.ResumeLayout(False)
        Me.tabRecycle.PerformLayout()
        CType(Me.nudTax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBrokerFee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStandings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBaseYield, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tabItems.ResumeLayout(False)
        Me.ctxRecycleItems.ResumeLayout(False)
        Me.ctxRecycleItem.ResumeLayout(False)
        Me.tabTotals.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
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
    Friend WithEvents tabAPIStatus As System.Windows.Forms.TabPage
    Friend WithEvents tabAssetFilters As System.Windows.Forms.TabPage
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents lblCurrentAPI As System.Windows.Forms.Label
    Friend WithEvents lvwCurrentAPIs As System.Windows.Forms.ListView
    Friend WithEvents colAPIOwner As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAssetsAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBalancesAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents tsbDownloadData As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblCharFilter As System.Windows.Forms.Label
    Friend WithEvents lvwCharFilter As System.Windows.Forms.ListView
    Friend WithEvents colOwner As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lblSearchAssets As System.Windows.Forms.Label
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
    Friend WithEvents tabOrders As System.Windows.Forms.TabPage
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents panelOrderInfo As System.Windows.Forms.Panel
    Friend WithEvents scMarketOrders As System.Windows.Forms.SplitContainer
    Friend WithEvents lblSellOrders As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrders As System.Windows.Forms.Label
    Friend WithEvents lblRemoteRange As System.Windows.Forms.Label
    Friend WithEvents lblModRange As System.Windows.Forms.Label
    Friend WithEvents lblBidRange As System.Windows.Forms.Label
    Friend WithEvents lblAskRange As System.Windows.Forms.Label
    Friend WithEvents lblRemoteRangeLbl As System.Windows.Forms.Label
    Friend WithEvents lblModRangeLbl As System.Windows.Forms.Label
    Friend WithEvents lblBidRangeLbl As System.Windows.Forms.Label
    Friend WithEvents lblAskRangeLbl As System.Windows.Forms.Label
    Friend WithEvents lblBuyTotal As System.Windows.Forms.Label
    Friend WithEvents lblSellTotal As System.Windows.Forms.Label
    Friend WithEvents lblTransTax As System.Windows.Forms.Label
    Friend WithEvents lblBrokerFee As System.Windows.Forms.Label
    Friend WithEvents lblEscrow As System.Windows.Forms.Label
    Friend WithEvents lblOrders As System.Windows.Forms.Label
    Friend WithEvents lblBuyTotalLbl As System.Windows.Forms.Label
    Friend WithEvents lblSellTotalLbl As System.Windows.Forms.Label
    Friend WithEvents lblTransTaxLbl As System.Windows.Forms.Label
    Friend WithEvents lblBrokerFeeLbl As System.Windows.Forms.Label
    Friend WithEvents lblEscrowLbl As System.Windows.Forms.Label
    Friend WithEvents lblOrdersLbl As System.Windows.Forms.Label
    Friend WithEvents tabTransactions As System.Windows.Forms.TabPage
    Friend WithEvents clvTransactions As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colWTransDate As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWTransItem As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWTransQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWTransPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWTransTotal As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWTransLocation As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWTransClient As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lblOwner As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cboOwner As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents cboWalletTransDivision As System.Windows.Forms.ComboBox
    Friend WithEvents lblWalletTransDivision As System.Windows.Forms.Label
    Friend WithEvents tabJournal As System.Windows.Forms.TabPage
    Friend WithEvents cboWalletJournalDivision As System.Windows.Forms.ComboBox
    Friend WithEvents lblWalletJournalDivision As System.Windows.Forms.Label
    Friend WithEvents clvJournal As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colWalletJournalDate As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWalletJournalType As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWalletJournalAmount As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWalletJournalBalance As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWalletJournalDescription As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents clvSellOrders As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colSOType As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSOQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSOPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSOLocation As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSOExpiresIn As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents clvBuyOrders As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colBOType As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBOQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBOPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBOLocation As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBOExpiresIn As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBORange As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBOMinVol As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnRefreshAssets As System.Windows.Forms.Button
    Friend WithEvents tsbAssets As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbInvestments As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbRigBuilder As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbOrders As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbTransactions As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbJournal As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxTabPrism As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuClosePrismTab As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbJobs As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tabJobs As System.Windows.Forms.TabPage
    Friend WithEvents clvJobs As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colJobsItem As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colJobsActivity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colJobsLocation As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colJobsEndTime As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colJobsStatus As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnFilters As System.Windows.Forms.Button
    Friend WithEvents tabRecycle As System.Windows.Forms.TabPage
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabItems As System.Windows.Forms.TabPage
    Friend WithEvents clvRecycle As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colRecycleItem As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRecycleMetaLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRecycleQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBatches As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colItemPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colTotalPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRefinePrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents tabTotals As System.Windows.Forms.TabPage
    Friend WithEvents clvTotals As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colMaterial As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colStationTake As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWaste As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colReceive As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colMatPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colMatTotal As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lblItems As System.Windows.Forms.Label
    Friend WithEvents lblVolume As System.Windows.Forms.Label
    Friend WithEvents lblItemsLbl As System.Windows.Forms.Label
    Friend WithEvents lblVolumeLbl As System.Windows.Forms.Label
    Friend WithEvents cboRefineMode As System.Windows.Forms.ComboBox
    Friend WithEvents lblRefineMode As System.Windows.Forms.Label
    Friend WithEvents chkOverrideStandings As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverrideBaseYield As System.Windows.Forms.CheckBox
    Friend WithEvents nudStandings As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudBaseYield As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblCorp As System.Windows.Forms.Label
    Friend WithEvents lblCorpLbl As System.Windows.Forms.Label
    Friend WithEvents lblStation As System.Windows.Forms.Label
    Friend WithEvents lblStationLbl As System.Windows.Forms.Label
    Friend WithEvents lblBaseYield As System.Windows.Forms.Label
    Friend WithEvents lblNetYield As System.Windows.Forms.Label
    Friend WithEvents lblStandings As System.Windows.Forms.Label
    Friend WithEvents lblStationTake As System.Windows.Forms.Label
    Friend WithEvents lblStationTakeLbl As System.Windows.Forms.Label
    Friend WithEvents lblStandingsLbl As System.Windows.Forms.Label
    Friend WithEvents lblNetYieldLbl As System.Windows.Forms.Label
    Friend WithEvents lblBaseYieldLbl As System.Windows.Forms.Label
    Friend WithEvents chkPerfectRefine As System.Windows.Forms.CheckBox
    Friend WithEvents cboRecyclePilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents tsbRecycle As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxRecycleItem As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAlterRecycleQuantity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuRemoveRecycleItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxRecycleItems As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddRecycleItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExportTransactions As System.Windows.Forms.Button
    Friend WithEvents btnExportJournal As System.Windows.Forms.Button
    Friend WithEvents btnExportJobs As System.Windows.Forms.Button
    Friend WithEvents btnExportOrders As System.Windows.Forms.Button
    Friend WithEvents btnExportRigList As System.Windows.Forms.Button
    Friend WithEvents btnExportRigBuildList As System.Windows.Forms.Button
    Friend WithEvents lblTransactionView As System.Windows.Forms.Label
    Friend WithEvents lblTotalFees As System.Windows.Forms.Label
    Friend WithEvents lblTotalFeesLbl As System.Windows.Forms.Label
    Friend WithEvents nudTax As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudBrokerFee As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkOverrideTax As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverrideBrokerFee As System.Windows.Forms.CheckBox
    Friend WithEvents colFees As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSalePrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBestPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents chkFeesOnRefine As System.Windows.Forms.CheckBox
    Friend WithEvents lblPriceTotals As System.Windows.Forms.Label
    Friend WithEvents chkFeesOnItems As System.Windows.Forms.CheckBox
    Friend WithEvents mnuAddCustomName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRemoveCustomName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
End Class
