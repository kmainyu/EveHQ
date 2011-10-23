<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrism
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrism))
        Me.lblCurrentAPI = New System.Windows.Forms.Label()
        Me.lvwCurrentAPIs = New System.Windows.Forms.ListView()
        Me.colAPIOwner = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOwnerType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colAssetsAPI = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colBalancesAPI = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colJobsAPI = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colJournalAPI = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOrdersAPI = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colTransAPI = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colContractsAPI = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCorpSheetAPI = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblSellOrders = New System.Windows.Forms.Label()
        Me.lblBuyOrders = New System.Windows.Forms.Label()
        Me.lblRemoteRange = New System.Windows.Forms.Label()
        Me.lblModRange = New System.Windows.Forms.Label()
        Me.lblBidRange = New System.Windows.Forms.Label()
        Me.lblAskRange = New System.Windows.Forms.Label()
        Me.lblRemoteRangeLbl = New System.Windows.Forms.Label()
        Me.lblModRangeLbl = New System.Windows.Forms.Label()
        Me.lblBidRangeLbl = New System.Windows.Forms.Label()
        Me.lblAskRangeLbl = New System.Windows.Forms.Label()
        Me.lblBuyTotal = New System.Windows.Forms.Label()
        Me.lblSellTotal = New System.Windows.Forms.Label()
        Me.lblTransTax = New System.Windows.Forms.Label()
        Me.lblBrokerFee = New System.Windows.Forms.Label()
        Me.lblEscrow = New System.Windows.Forms.Label()
        Me.lblOrders = New System.Windows.Forms.Label()
        Me.lblBuyTotalLbl = New System.Windows.Forms.Label()
        Me.lblSellTotalLbl = New System.Windows.Forms.Label()
        Me.lblTransTaxLbl = New System.Windows.Forms.Label()
        Me.lblBrokerFeeLbl = New System.Windows.Forms.Label()
        Me.lblEscrowLbl = New System.Windows.Forms.Label()
        Me.lblOrdersLbl = New System.Windows.Forms.Label()
        Me.lblType = New System.Windows.Forms.Label()
        Me.btnExportTransactions = New System.Windows.Forms.Button()
        Me.lblWalletTransDivision = New System.Windows.Forms.Label()
        Me.ctxTransactions = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuTransactionModifyPrice = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnResetJournal = New DevComponents.DotNetBar.ButtonX()
        Me.lblAlwaysShowEveBalance = New System.Windows.Forms.Label()
        Me.sbShowEveBalance = New DevComponents.DotNetBar.Controls.SwitchButton()
        Me.btnJournalQuery = New DevComponents.DotNetBar.ButtonX()
        Me.cboJournalRefTypes = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.cboJournalOwners = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.lblJournalEndDate = New System.Windows.Forms.Label()
        Me.lblJournalStartDate = New System.Windows.Forms.Label()
        Me.dtiJournalEndDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.dtiJournalStartDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.cboWalletJournalDivision = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.btnExportJournal = New System.Windows.Forms.Button()
        Me.lblWalletJournalDivision = New System.Windows.Forms.Label()
        Me.lblJobInstallerFilter = New System.Windows.Forms.Label()
        Me.btnExportJobs = New System.Windows.Forms.Button()
        Me.ctxRecycleExport = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuExportToCSV = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExportToTSV = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExportTotalsToCSV = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExportTotalsToTSV = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkFeesOnItems = New System.Windows.Forms.CheckBox()
        Me.lblPriceTotals = New System.Windows.Forms.Label()
        Me.chkFeesOnRefine = New System.Windows.Forms.CheckBox()
        Me.lblTotalFees = New System.Windows.Forms.Label()
        Me.lblTotalFeesLbl = New System.Windows.Forms.Label()
        Me.nudTax = New System.Windows.Forms.NumericUpDown()
        Me.nudBrokerFee = New System.Windows.Forms.NumericUpDown()
        Me.chkOverrideTax = New System.Windows.Forms.CheckBox()
        Me.chkOverrideBrokerFee = New System.Windows.Forms.CheckBox()
        Me.lblItems = New System.Windows.Forms.Label()
        Me.lblVolume = New System.Windows.Forms.Label()
        Me.lblItemsLbl = New System.Windows.Forms.Label()
        Me.lblVolumeLbl = New System.Windows.Forms.Label()
        Me.cboRefineMode = New System.Windows.Forms.ComboBox()
        Me.lblRefineMode = New System.Windows.Forms.Label()
        Me.chkOverrideStandings = New System.Windows.Forms.CheckBox()
        Me.chkOverrideBaseYield = New System.Windows.Forms.CheckBox()
        Me.nudStandings = New System.Windows.Forms.NumericUpDown()
        Me.nudBaseYield = New System.Windows.Forms.NumericUpDown()
        Me.lblCorp = New System.Windows.Forms.Label()
        Me.lblCorpLbl = New System.Windows.Forms.Label()
        Me.lblStation = New System.Windows.Forms.Label()
        Me.lblStationLbl = New System.Windows.Forms.Label()
        Me.lblBaseYield = New System.Windows.Forms.Label()
        Me.lblNetYield = New System.Windows.Forms.Label()
        Me.lblStandings = New System.Windows.Forms.Label()
        Me.lblStationTake = New System.Windows.Forms.Label()
        Me.lblStationTakeLbl = New System.Windows.Forms.Label()
        Me.lblStandingsLbl = New System.Windows.Forms.Label()
        Me.lblNetYieldLbl = New System.Windows.Forms.Label()
        Me.lblBaseYieldLbl = New System.Windows.Forms.Label()
        Me.chkPerfectRefine = New System.Windows.Forms.CheckBox()
        Me.cboRecyclePilots = New System.Windows.Forms.ComboBox()
        Me.lblPilot = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabItems = New System.Windows.Forms.TabPage()
        Me.adtRecycle = New DevComponents.AdvTree.AdvTree()
        Me.colRecItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecMeta = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecQty = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecBatches = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecValue = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecFees = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecSalePrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecRefinePrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecBestPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecTotalBen = New DevComponents.AdvTree.ColumnHeader()
        Me.colRecUnitBen = New DevComponents.AdvTree.ColumnHeader()
        Me.ctxRecycleItem = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddRecycleItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAlterRecycleQuantity = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRemoveRecycleItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NodeConnector12 = New DevComponents.AdvTree.NodeConnector()
        Me.ItemNormal = New DevComponents.DotNetBar.ElementStyle()
        Me.ItemGood = New DevComponents.DotNetBar.ElementStyle()
        Me.tabTotals = New System.Windows.Forms.TabPage()
        Me.adtTotals = New DevComponents.AdvTree.AdvTree()
        Me.colRTMaterial = New DevComponents.AdvTree.ColumnHeader()
        Me.colRTStationTake = New DevComponents.AdvTree.ColumnHeader()
        Me.colRTUnrecoverable = New DevComponents.AdvTree.ColumnHeader()
        Me.colRTReceivable = New DevComponents.AdvTree.ColumnHeader()
        Me.colRTPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRTTotal = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector11 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle8 = New DevComponents.DotNetBar.ElementStyle()
        Me.cboCategoryFilter = New System.Windows.Forms.ComboBox()
        Me.lblBPCatFilter = New System.Windows.Forms.Label()
        Me.cboTypeFilter = New System.Windows.Forms.ComboBox()
        Me.lblTypeFilter = New System.Windows.Forms.Label()
        Me.cboTechFilter = New System.Windows.Forms.ComboBox()
        Me.lblTechFilter = New System.Windows.Forms.Label()
        Me.lblUserBP = New System.Windows.Forms.Label()
        Me.btnAddCustomBP = New System.Windows.Forms.Button()
        Me.lblExhausted = New System.Windows.Forms.Label()
        Me.lblUnknown = New System.Windows.Forms.Label()
        Me.lblMissing = New System.Windows.Forms.Label()
        Me.lblBPC = New System.Windows.Forms.Label()
        Me.lblBPO = New System.Windows.Forms.Label()
        Me.btnResetBPSearch = New System.Windows.Forms.Button()
        Me.txtBPSearch = New System.Windows.Forms.TextBox()
        Me.lblBPSearch = New System.Windows.Forms.Label()
        Me.btnGetBPJobInfo = New System.Windows.Forms.Button()
        Me.btnUpdateBPsFromAssets = New System.Windows.Forms.Button()
        Me.btnBPCalc = New System.Windows.Forms.Button()
        Me.chkShowOwnedBPs = New System.Windows.Forms.CheckBox()
        Me.ctxBPManager = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSendToBPCalc = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAmendBPDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRemoveCustomBP = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCopyListToClipboard = New System.Windows.Forms.Button()
        Me.RibbonBarMergeContainer1 = New DevComponents.DotNetBar.RibbonBarMergeContainer()
        Me.rbProduction = New DevComponents.DotNetBar.RibbonBar()
        Me.btnProductionManager = New DevComponents.DotNetBar.ButtonItem()
        Me.btnInventionManager = New DevComponents.DotNetBar.ButtonItem()
        Me.btnBlueprintCalc = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRigBuilder = New DevComponents.DotNetBar.ButtonItem()
        Me.rbQuickCalcs = New DevComponents.DotNetBar.RibbonBar()
        Me.btnInventionChance = New DevComponents.DotNetBar.ButtonItem()
        Me.btnQuickProduction = New DevComponents.DotNetBar.ButtonItem()
        Me.rbAnalysisTools = New DevComponents.DotNetBar.RibbonBar()
        Me.btnReports = New DevComponents.DotNetBar.ButtonItem()
        Me.btnCharts = New DevComponents.DotNetBar.ButtonItem()
        Me.btnInventionResults = New DevComponents.DotNetBar.ButtonItem()
        Me.rbMarketTools = New DevComponents.DotNetBar.RibbonBar()
        Me.btnOrders = New DevComponents.DotNetBar.ButtonItem()
        Me.btnJobs = New DevComponents.DotNetBar.ButtonItem()
        Me.btnContracts = New DevComponents.DotNetBar.ButtonItem()
        Me.rbAssetManagement = New DevComponents.DotNetBar.RibbonBar()
        Me.btnAssets = New DevComponents.DotNetBar.ButtonItem()
        Me.btnBPManager = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRecycler = New DevComponents.DotNetBar.ButtonItem()
        Me.rbWallet = New DevComponents.DotNetBar.RibbonBar()
        Me.btnWalletJournal = New DevComponents.DotNetBar.ButtonItem()
        Me.btnWalletTransactions = New DevComponents.DotNetBar.ButtonItem()
        Me.rbData = New DevComponents.DotNetBar.RibbonBar()
        Me.btnOptions = New DevComponents.DotNetBar.ButtonItem()
        Me.btnDownloadAPIData = New DevComponents.DotNetBar.ButtonItem()
        Me.pnlPrism = New DevComponents.DotNetBar.PanelEx()
        Me.tabPrism = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
        Me.btnRefreshAPI = New DevComponents.DotNetBar.ButtonX()
        Me.btnLinkRequisition = New DevComponents.DotNetBar.ButtonX()
        Me.btnLinkProduction = New DevComponents.DotNetBar.ButtonX()
        Me.btnLinkBPCalc = New DevComponents.DotNetBar.ButtonX()
        Me.lblSelectedBP = New System.Windows.Forms.Label()
        Me.lblSelectedItem = New System.Windows.Forms.Label()
        Me.adtSearch = New DevComponents.AdvTree.AdvTree()
        Me.colItemSearch = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector2 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle2 = New DevComponents.DotNetBar.ElementStyle()
        Me.txtItemSearch = New DevComponents.DotNetBar.Controls.TextBoxX()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.tiPrismHome = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel6 = New DevComponents.DotNetBar.TabControlPanel()
        Me.btnImportEntries = New DevComponents.DotNetBar.ButtonX()
        Me.btnExportEntries = New DevComponents.DotNetBar.ButtonX()
        Me.btnCheckJournalOmissions = New DevComponents.DotNetBar.ButtonX()
        Me.adtJournal = New DevComponents.AdvTree.AdvTree()
        Me.colJournalDate = New DevComponents.AdvTree.ColumnHeader()
        Me.colJournalType = New DevComponents.AdvTree.ColumnHeader()
        Me.colJournalAmount = New DevComponents.AdvTree.ColumnHeader()
        Me.colJournalBalance = New DevComponents.AdvTree.ColumnHeader()
        Me.colJournalDescription = New DevComponents.AdvTree.ColumnHeader()
        Me.ElementStyle1 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiJournal = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel17 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtInventionJobs = New DevComponents.AdvTree.AdvTree()
        Me.colInvJobName = New DevComponents.AdvTree.ColumnHeader()
        Me.colInvJobItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colSuccessChance = New DevComponents.AdvTree.ColumnHeader()
        Me.colSuccessCost = New DevComponents.AdvTree.ColumnHeader()
        Me.colInvProductionCost = New DevComponents.AdvTree.ColumnHeader()
        Me.colInvSalesPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colInvUnitProfit = New DevComponents.AdvTree.ColumnHeader()
        Me.colInvProfitMargin = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector17 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle14 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiInventionManager = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
        Me.wbReport = New System.Windows.Forms.WebBrowser()
        Me.pnlReportControls = New DevComponents.DotNetBar.PanelEx()
        Me.cboReport = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.lblReportStartDate = New System.Windows.Forms.Label()
        Me.btnGenerateReport = New DevComponents.DotNetBar.ButtonX()
        Me.lblReportEndDate = New System.Windows.Forms.Label()
        Me.dtiReportEndDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.dtiReportStartDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.cboReportOwners = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.tiReports = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel11 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tcPM = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel12 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtProdJobs = New DevComponents.AdvTree.AdvTree()
        Me.colJobName = New DevComponents.AdvTree.ColumnHeader()
        Me.colJobItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colJobUnitProfit = New DevComponents.AdvTree.ColumnHeader()
        Me.colJobProfitRate = New DevComponents.AdvTree.ColumnHeader()
        Me.colJobMargin = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector3 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle3 = New DevComponents.DotNetBar.ElementStyle()
        Me.pnlJobs = New DevComponents.DotNetBar.PanelEx()
        Me.btnClearAllJobs = New DevComponents.DotNetBar.ButtonX()
        Me.btnMakeBatch = New DevComponents.DotNetBar.ButtonX()
        Me.btnRefreshJobs = New DevComponents.DotNetBar.ButtonX()
        Me.btnDeleteJob = New DevComponents.DotNetBar.ButtonX()
        Me.tiProductionJobs = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel13 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtBatches = New DevComponents.AdvTree.AdvTree()
        Me.colBatchName = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector4 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle4 = New DevComponents.DotNetBar.ElementStyle()
        Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
        Me.btnClearBatches = New DevComponents.DotNetBar.ButtonX()
        Me.tiBatchJobs = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.PRPM = New EveHQ.Prism.PrismResources()
        Me.tiProductionManager = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel16 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lblInventionItems = New System.Windows.Forms.Label()
        Me.lblInventionInstallers = New System.Windows.Forms.Label()
        Me.adtInventionStats = New DevComponents.AdvTree.AdvTree()
        Me.NodeConnector16 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle13 = New DevComponents.DotNetBar.ElementStyle()
        Me.adtInventionResults = New DevComponents.AdvTree.AdvTree()
        Me.colInventionDate = New DevComponents.AdvTree.ColumnHeader()
        Me.colInventionItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colInventionInstaller = New DevComponents.AdvTree.ColumnHeader()
        Me.colInventionResult = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector15 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle12 = New DevComponents.DotNetBar.ElementStyle()
        Me.cboInventionItems = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.cboInventionInstallers = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.btnGetResults = New DevComponents.DotNetBar.ButtonX()
        Me.dtiInventionEndDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.lblInvEndDate = New System.Windows.Forms.Label()
        Me.dtiInventionStartDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.lblInvStartDate = New System.Windows.Forms.Label()
        Me.tiInventionResults = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel7 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lblStatusFilter = New System.Windows.Forms.Label()
        Me.cboStatusFilter = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.lblActivityFilter = New System.Windows.Forms.Label()
        Me.cboActivityFilter = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboInstallerFilter = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboJobOwner = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.lblJobOwner = New System.Windows.Forms.Label()
        Me.adtJobs = New DevComponents.AdvTree.AdvTree()
        Me.colIJobsItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colIJobsActivity = New DevComponents.AdvTree.ColumnHeader()
        Me.colIJobsRuns = New DevComponents.AdvTree.ColumnHeader()
        Me.colIJobsInstaller = New DevComponents.AdvTree.ColumnHeader()
        Me.colIJobsLocation = New DevComponents.AdvTree.ColumnHeader()
        Me.colIJobsEndTime = New DevComponents.AdvTree.ColumnHeader()
        Me.colIJobsStatus = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector7 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle5 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiJobs = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel5 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lblTransProfitRatio = New System.Windows.Forms.Label()
        Me.lblTransProfitValue = New System.Windows.Forms.Label()
        Me.lblTransSellValue = New System.Windows.Forms.Label()
        Me.lblTransBuyValue = New System.Windows.Forms.Label()
        Me.cboWalletTransItem = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.lblTransItemType = New System.Windows.Forms.Label()
        Me.cboTransactionOwner = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.cboWalletTransDivision = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboWalletTransType = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboTransAll = New DevComponents.Editors.ComboItem()
        Me.cboTransBuy = New DevComponents.Editors.ComboItem()
        Me.cboTransSell = New DevComponents.Editors.ComboItem()
        Me.btnGetTransactions = New DevComponents.DotNetBar.ButtonX()
        Me.dtiTransEndDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtiTransStartDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.adtTransactions = New DevComponents.AdvTree.AdvTree()
        Me.colTransDate = New DevComponents.AdvTree.ColumnHeader()
        Me.colTransItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colTransQuantity = New DevComponents.AdvTree.ColumnHeader()
        Me.colTransPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colTransValue = New DevComponents.AdvTree.ColumnHeader()
        Me.colTransLocation = New DevComponents.AdvTree.ColumnHeader()
        Me.colTransClient = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector10 = New DevComponents.AdvTree.NodeConnector()
        Me.Personal = New DevComponents.DotNetBar.ElementStyle()
        Me.Corp = New DevComponents.DotNetBar.ElementStyle()
        Me.Buy = New DevComponents.DotNetBar.ElementStyle()
        Me.Sell = New DevComponents.DotNetBar.ElementStyle()
        Me.Numeric = New DevComponents.DotNetBar.ElementStyle()
        Me.tiTransactions = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel10 = New DevComponents.DotNetBar.TabControlPanel()
        Me.zgcPrism = New ZedGraph.ZedGraphControl()
        Me.PanelEx2 = New DevComponents.DotNetBar.PanelEx()
        Me.cboChart = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnGenerateChart = New DevComponents.DotNetBar.ButtonX()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtiChartEndDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.dtiChartStartDate = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.cboChartOwners = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.tiCharts = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel15 = New DevComponents.DotNetBar.TabControlPanel()
        Me.pnlRigs = New DevComponents.DotNetBar.PanelEx()
        Me.adtRigs = New DevComponents.AdvTree.AdvTree()
        Me.colRigListType = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigListQuantity = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigListRigPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigListSalvagePrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigListBuildBenefit = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigListRigValue = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigListSalvageValue = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigListTotalBuildBenefit = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigListMargin = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector14 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle11 = New DevComponents.DotNetBar.ElementStyle()
        Me.ExpandableSplitter1 = New DevComponents.DotNetBar.ExpandableSplitter()
        Me.adtRigBuildList = New DevComponents.AdvTree.AdvTree()
        Me.colRigBuildType = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigBuildQuantity = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigBuidRigPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigBuildSalvagePrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigBuildBenefit = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigBuildRigValue = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigBuildSalvageValue = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigBuildTotalBenefit = New DevComponents.AdvTree.ColumnHeader()
        Me.colRigBuildMargin = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector13 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle10 = New DevComponents.DotNetBar.ElementStyle()
        Me.PSCRigOwners = New EveHQ.Prism.PrismSelectionHostControl()
        Me.lblTotalRigMargin = New System.Windows.Forms.Label()
        Me.lblTotalRigProfit = New System.Windows.Forms.Label()
        Me.lblTotalRigSalePrice = New System.Windows.Forms.Label()
        Me.gpAutoRig = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.btnAutoRig = New DevComponents.DotNetBar.ButtonX()
        Me.chkRigMargin = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.chkRigSalePrice = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.chkTotalProfit = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.chkTotalSalePrice = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.chkRigProfit = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.btnBuildRigs = New DevComponents.DotNetBar.ButtonX()
        Me.btnExportRigBuildList = New DevComponents.DotNetBar.ButtonX()
        Me.btnExportRigList = New DevComponents.DotNetBar.ButtonX()
        Me.nudRigMELevel = New System.Windows.Forms.NumericUpDown()
        Me.lblRigMELevel = New System.Windows.Forms.Label()
        Me.tiRigBuilder = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
        Me.pnlSellOrders = New DevComponents.DotNetBar.PanelEx()
        Me.cboOrdersOwner = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.lblOrdersOwner = New System.Windows.Forms.Label()
        Me.adtSellOrders = New DevComponents.AdvTree.AdvTree()
        Me.colSellType = New DevComponents.AdvTree.ColumnHeader()
        Me.colSellQty = New DevComponents.AdvTree.ColumnHeader()
        Me.colSellPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colSellLocation = New DevComponents.AdvTree.ColumnHeader()
        Me.colSellExpires = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector9 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle7 = New DevComponents.DotNetBar.ElementStyle()
        Me.splitterMarketOrders = New DevComponents.DotNetBar.ExpandableSplitter()
        Me.pnlBuyOrders = New DevComponents.DotNetBar.PanelEx()
        Me.adtBuyOrders = New DevComponents.AdvTree.AdvTree()
        Me.colBuyType = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyQty = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyLocation = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyRange = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyVolume = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyExpires = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector8 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle6 = New DevComponents.DotNetBar.ElementStyle()
        Me.pnlOrderStats = New DevComponents.DotNetBar.PanelEx()
        Me.tiMarketOrders = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel14 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtContracts = New DevComponents.AdvTree.AdvTree()
        Me.colContractTitle = New DevComponents.AdvTree.ColumnHeader()
        Me.colContractLocation = New DevComponents.AdvTree.ColumnHeader()
        Me.colContractTransaction = New DevComponents.AdvTree.ColumnHeader()
        Me.colContractType = New DevComponents.AdvTree.ColumnHeader()
        Me.colContractStatus = New DevComponents.AdvTree.ColumnHeader()
        Me.colContractDateIssued = New DevComponents.AdvTree.ColumnHeader()
        Me.colContractDateExpired = New DevComponents.AdvTree.ColumnHeader()
        Me.colContractPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colContractVolume = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector6 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle9 = New DevComponents.DotNetBar.ElementStyle()
        Me.cboContractOwner = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.lblContractOwner = New System.Windows.Forms.Label()
        Me.tiContracts = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel8 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiRecycler = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel9 = New DevComponents.DotNetBar.TabControlPanel()
        Me.cboBPOwner = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.lblBPOwner = New System.Windows.Forms.Label()
        Me.adtBlueprints = New DevComponents.AdvTree.AdvTree()
        Me.colBPMBlueprint = New DevComponents.AdvTree.ColumnHeader()
        Me.colBPMLocation = New DevComponents.AdvTree.ColumnHeader()
        Me.colBPMLocation2 = New DevComponents.AdvTree.ColumnHeader()
        Me.colBPMTechLevel = New DevComponents.AdvTree.ColumnHeader()
        Me.colBPMME = New DevComponents.AdvTree.ColumnHeader()
        Me.colBPMPE = New DevComponents.AdvTree.ColumnHeader()
        Me.colBPMRuns = New DevComponents.AdvTree.ColumnHeader()
        Me.colBPMStatus = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector5 = New DevComponents.AdvTree.NodeConnector()
        Me.BP = New DevComponents.DotNetBar.ElementStyle()
        Me.pbBPO = New System.Windows.Forms.PictureBox()
        Me.pbBPC = New System.Windows.Forms.PictureBox()
        Me.pbMissing = New System.Windows.Forms.PictureBox()
        Me.pbUnknown = New System.Windows.Forms.PictureBox()
        Me.pbExhausted = New System.Windows.Forms.PictureBox()
        Me.pbUserBP = New System.Windows.Forms.PictureBox()
        Me.tiBPManager = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel()
        Me.PAC = New EveHQ.Prism.PrismAssetsControl()
        Me.tiAssets = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.ctxTransactions.SuspendLayout()
        CType(Me.dtiJournalEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtiJournalStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxRecycleExport.SuspendLayout()
        CType(Me.nudTax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBrokerFee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStandings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBaseYield, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tabItems.SuspendLayout()
        CType(Me.adtRecycle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxRecycleItem.SuspendLayout()
        Me.tabTotals.SuspendLayout()
        CType(Me.adtTotals, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxBPManager.SuspendLayout()
        Me.RibbonBarMergeContainer1.SuspendLayout()
        Me.pnlPrism.SuspendLayout()
        CType(Me.tabPrism, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPrism.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        CType(Me.adtSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel6.SuspendLayout()
        CType(Me.adtJournal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel17.SuspendLayout()
        CType(Me.adtInventionJobs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel3.SuspendLayout()
        Me.pnlReportControls.SuspendLayout()
        CType(Me.dtiReportEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtiReportStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel11.SuspendLayout()
        CType(Me.tcPM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcPM.SuspendLayout()
        Me.TabControlPanel12.SuspendLayout()
        CType(Me.adtProdJobs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlJobs.SuspendLayout()
        Me.TabControlPanel13.SuspendLayout()
        CType(Me.adtBatches, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelEx1.SuspendLayout()
        Me.TabControlPanel16.SuspendLayout()
        CType(Me.adtInventionStats, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.adtInventionResults, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtiInventionEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtiInventionStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel7.SuspendLayout()
        CType(Me.adtJobs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel5.SuspendLayout()
        CType(Me.dtiTransEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtiTransStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.adtTransactions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel10.SuspendLayout()
        Me.PanelEx2.SuspendLayout()
        CType(Me.dtiChartEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtiChartStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel15.SuspendLayout()
        Me.pnlRigs.SuspendLayout()
        CType(Me.adtRigs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.adtRigBuildList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gpAutoRig.SuspendLayout()
        CType(Me.nudRigMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel4.SuspendLayout()
        Me.pnlSellOrders.SuspendLayout()
        CType(Me.adtSellOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBuyOrders.SuspendLayout()
        CType(Me.adtBuyOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlOrderStats.SuspendLayout()
        Me.TabControlPanel14.SuspendLayout()
        CType(Me.adtContracts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel8.SuspendLayout()
        Me.TabControlPanel9.SuspendLayout()
        CType(Me.adtBlueprints, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbBPO, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbBPC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbMissing, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbUnknown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbExhausted, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbUserBP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblCurrentAPI
        '
        Me.lblCurrentAPI.AutoSize = True
        Me.lblCurrentAPI.BackColor = System.Drawing.Color.Transparent
        Me.lblCurrentAPI.Location = New System.Drawing.Point(402, 9)
        Me.lblCurrentAPI.Name = "lblCurrentAPI"
        Me.lblCurrentAPI.Size = New System.Drawing.Size(110, 13)
        Me.lblCurrentAPI.TabIndex = 1
        Me.lblCurrentAPI.Text = "Cached APIs Loaded:"
        '
        'lvwCurrentAPIs
        '
        Me.lvwCurrentAPIs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwCurrentAPIs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAPIOwner, Me.colOwnerType, Me.colAssetsAPI, Me.colBalancesAPI, Me.colJobsAPI, Me.colJournalAPI, Me.colOrdersAPI, Me.colTransAPI, Me.colContractsAPI, Me.colCorpSheetAPI})
        Me.lvwCurrentAPIs.FullRowSelect = True
        Me.lvwCurrentAPIs.GridLines = True
        Me.lvwCurrentAPIs.Location = New System.Drawing.Point(405, 25)
        Me.lvwCurrentAPIs.Name = "lvwCurrentAPIs"
        Me.lvwCurrentAPIs.ShowItemToolTips = True
        Me.lvwCurrentAPIs.Size = New System.Drawing.Size(875, 583)
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
        Me.colAssetsAPI.Width = 90
        '
        'colBalancesAPI
        '
        Me.colBalancesAPI.Text = "Balances API"
        Me.colBalancesAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colBalancesAPI.Width = 90
        '
        'colJobsAPI
        '
        Me.colJobsAPI.Text = "Jobs API"
        Me.colJobsAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colJobsAPI.Width = 90
        '
        'colJournalAPI
        '
        Me.colJournalAPI.Text = "Journal API"
        Me.colJournalAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colJournalAPI.Width = 90
        '
        'colOrdersAPI
        '
        Me.colOrdersAPI.Text = "Orders API"
        Me.colOrdersAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colOrdersAPI.Width = 90
        '
        'colTransAPI
        '
        Me.colTransAPI.Text = "Transaction API"
        Me.colTransAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colTransAPI.Width = 90
        '
        'colContractsAPI
        '
        Me.colContractsAPI.Text = "Contracts API"
        Me.colContractsAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colContractsAPI.Width = 90
        '
        'colCorpSheetAPI
        '
        Me.colCorpSheetAPI.Text = "Corp Sheet API"
        Me.colCorpSheetAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colCorpSheetAPI.Width = 90
        '
        'lblSellOrders
        '
        Me.lblSellOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSellOrders.AutoSize = True
        Me.lblSellOrders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSellOrders.Location = New System.Drawing.Point(3, 33)
        Me.lblSellOrders.Name = "lblSellOrders"
        Me.lblSellOrders.Size = New System.Drawing.Size(47, 13)
        Me.lblSellOrders.TabIndex = 25
        Me.lblSellOrders.Text = "Selling:"
        '
        'lblBuyOrders
        '
        Me.lblBuyOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBuyOrders.AutoSize = True
        Me.lblBuyOrders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBuyOrders.Location = New System.Drawing.Point(3, 3)
        Me.lblBuyOrders.Name = "lblBuyOrders"
        Me.lblBuyOrders.Size = New System.Drawing.Size(48, 13)
        Me.lblBuyOrders.TabIndex = 26
        Me.lblBuyOrders.Text = "Buying:"
        '
        'lblRemoteRange
        '
        Me.lblRemoteRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRemoteRange.AutoSize = True
        Me.lblRemoteRange.Location = New System.Drawing.Point(414, 48)
        Me.lblRemoteRange.Name = "lblRemoteRange"
        Me.lblRemoteRange.Size = New System.Drawing.Size(23, 13)
        Me.lblRemoteRange.TabIndex = 42
        Me.lblRemoteRange.Text = "n/a"
        '
        'lblModRange
        '
        Me.lblModRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblModRange.AutoSize = True
        Me.lblModRange.Location = New System.Drawing.Point(414, 35)
        Me.lblModRange.Name = "lblModRange"
        Me.lblModRange.Size = New System.Drawing.Size(23, 13)
        Me.lblModRange.TabIndex = 41
        Me.lblModRange.Text = "n/a"
        '
        'lblBidRange
        '
        Me.lblBidRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBidRange.AutoSize = True
        Me.lblBidRange.Location = New System.Drawing.Point(414, 22)
        Me.lblBidRange.Name = "lblBidRange"
        Me.lblBidRange.Size = New System.Drawing.Size(23, 13)
        Me.lblBidRange.TabIndex = 40
        Me.lblBidRange.Text = "n/a"
        '
        'lblAskRange
        '
        Me.lblAskRange.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAskRange.AutoSize = True
        Me.lblAskRange.Location = New System.Drawing.Point(414, 9)
        Me.lblAskRange.Name = "lblAskRange"
        Me.lblAskRange.Size = New System.Drawing.Size(23, 13)
        Me.lblAskRange.TabIndex = 39
        Me.lblAskRange.Text = "n/a"
        '
        'lblRemoteRangeLbl
        '
        Me.lblRemoteRangeLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRemoteRangeLbl.AutoSize = True
        Me.lblRemoteRangeLbl.Location = New System.Drawing.Point(306, 48)
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
        Me.lblModRangeLbl.Location = New System.Drawing.Point(306, 35)
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
        Me.lblBidRangeLbl.Location = New System.Drawing.Point(306, 22)
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
        Me.lblAskRangeLbl.Location = New System.Drawing.Point(306, 9)
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
        Me.lblBuyTotal.Location = New System.Drawing.Point(110, 61)
        Me.lblBuyTotal.Name = "lblBuyTotal"
        Me.lblBuyTotal.Size = New System.Drawing.Size(23, 13)
        Me.lblBuyTotal.TabIndex = 34
        Me.lblBuyTotal.Text = "n/a"
        '
        'lblSellTotal
        '
        Me.lblSellTotal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSellTotal.AutoSize = True
        Me.lblSellTotal.Location = New System.Drawing.Point(110, 48)
        Me.lblSellTotal.Name = "lblSellTotal"
        Me.lblSellTotal.Size = New System.Drawing.Size(23, 13)
        Me.lblSellTotal.TabIndex = 33
        Me.lblSellTotal.Text = "n/a"
        '
        'lblTransTax
        '
        Me.lblTransTax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTransTax.AutoSize = True
        Me.lblTransTax.Location = New System.Drawing.Point(110, 35)
        Me.lblTransTax.Name = "lblTransTax"
        Me.lblTransTax.Size = New System.Drawing.Size(23, 13)
        Me.lblTransTax.TabIndex = 32
        Me.lblTransTax.Text = "n/a"
        '
        'lblBrokerFee
        '
        Me.lblBrokerFee.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBrokerFee.AutoSize = True
        Me.lblBrokerFee.Location = New System.Drawing.Point(110, 22)
        Me.lblBrokerFee.Name = "lblBrokerFee"
        Me.lblBrokerFee.Size = New System.Drawing.Size(23, 13)
        Me.lblBrokerFee.TabIndex = 31
        Me.lblBrokerFee.Text = "n/a"
        '
        'lblEscrow
        '
        Me.lblEscrow.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEscrow.AutoSize = True
        Me.lblEscrow.Location = New System.Drawing.Point(110, 74)
        Me.lblEscrow.Name = "lblEscrow"
        Me.lblEscrow.Size = New System.Drawing.Size(23, 13)
        Me.lblEscrow.TabIndex = 30
        Me.lblEscrow.Text = "n/a"
        '
        'lblOrders
        '
        Me.lblOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOrders.AutoSize = True
        Me.lblOrders.Location = New System.Drawing.Point(110, 9)
        Me.lblOrders.Name = "lblOrders"
        Me.lblOrders.Size = New System.Drawing.Size(23, 13)
        Me.lblOrders.TabIndex = 29
        Me.lblOrders.Text = "n/a"
        '
        'lblBuyTotalLbl
        '
        Me.lblBuyTotalLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBuyTotalLbl.AutoSize = True
        Me.lblBuyTotalLbl.Location = New System.Drawing.Point(10, 61)
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
        Me.lblSellTotalLbl.Location = New System.Drawing.Point(10, 48)
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
        Me.lblTransTaxLbl.Location = New System.Drawing.Point(10, 35)
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
        Me.lblBrokerFeeLbl.Location = New System.Drawing.Point(10, 22)
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
        Me.lblEscrowLbl.Location = New System.Drawing.Point(10, 74)
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
        Me.lblOrdersLbl.Location = New System.Drawing.Point(10, 9)
        Me.lblOrdersLbl.Name = "lblOrdersLbl"
        Me.lblOrdersLbl.Size = New System.Drawing.Size(96, 13)
        Me.lblOrdersLbl.TabIndex = 23
        Me.lblOrdersLbl.Text = "Orders Remaining:"
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.BackColor = System.Drawing.Color.Transparent
        Me.lblType.Location = New System.Drawing.Point(314, 38)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(65, 13)
        Me.lblType.TabIndex = 5
        Me.lblType.Text = "Trans Type:"
        '
        'btnExportTransactions
        '
        Me.btnExportTransactions.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportTransactions.Location = New System.Drawing.Point(1197, 7)
        Me.btnExportTransactions.Name = "btnExportTransactions"
        Me.btnExportTransactions.Size = New System.Drawing.Size(75, 23)
        Me.btnExportTransactions.TabIndex = 4
        Me.btnExportTransactions.Text = "Export"
        Me.btnExportTransactions.UseVisualStyleBackColor = True
        '
        'lblWalletTransDivision
        '
        Me.lblWalletTransDivision.AutoSize = True
        Me.lblWalletTransDivision.BackColor = System.Drawing.Color.Transparent
        Me.lblWalletTransDivision.Location = New System.Drawing.Point(314, 11)
        Me.lblWalletTransDivision.Name = "lblWalletTransDivision"
        Me.lblWalletTransDivision.Size = New System.Drawing.Size(80, 13)
        Me.lblWalletTransDivision.TabIndex = 1
        Me.lblWalletTransDivision.Text = "Wallet Division:"
        '
        'ctxTransactions
        '
        Me.ctxTransactions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxTransactions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTransactionModifyPrice})
        Me.ctxTransactions.Name = "ctxTransactions"
        Me.ctxTransactions.Size = New System.Drawing.Size(133, 26)
        '
        'mnuTransactionModifyPrice
        '
        Me.mnuTransactionModifyPrice.Name = "mnuTransactionModifyPrice"
        Me.mnuTransactionModifyPrice.Size = New System.Drawing.Size(132, 22)
        Me.mnuTransactionModifyPrice.Text = "Modify Price"
        '
        'btnResetJournal
        '
        Me.btnResetJournal.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnResetJournal.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnResetJournal.Location = New System.Drawing.Point(651, 37)
        Me.btnResetJournal.Name = "btnResetJournal"
        Me.btnResetJournal.Size = New System.Drawing.Size(100, 23)
        Me.btnResetJournal.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnResetJournal.TabIndex = 17
        Me.btnResetJournal.Text = "Reset Journal DB"
        Me.btnResetJournal.Visible = False
        '
        'lblAlwaysShowEveBalance
        '
        Me.lblAlwaysShowEveBalance.AutoSize = True
        Me.lblAlwaysShowEveBalance.BackColor = System.Drawing.Color.Transparent
        Me.lblAlwaysShowEveBalance.Location = New System.Drawing.Point(304, 66)
        Me.lblAlwaysShowEveBalance.Name = "lblAlwaysShowEveBalance"
        Me.lblAlwaysShowEveBalance.Size = New System.Drawing.Size(136, 13)
        Me.lblAlwaysShowEveBalance.TabIndex = 16
        Me.lblAlwaysShowEveBalance.Text = "Always Show Eve Balance?"
        '
        'sbShowEveBalance
        '
        '
        '
        '
        Me.sbShowEveBalance.BackgroundStyle.Class = ""
        Me.sbShowEveBalance.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.sbShowEveBalance.Location = New System.Drawing.Point(490, 62)
        Me.sbShowEveBalance.Name = "sbShowEveBalance"
        Me.sbShowEveBalance.OffBackColor = System.Drawing.Color.Red
        Me.sbShowEveBalance.OnBackColor = System.Drawing.Color.LimeGreen
        Me.sbShowEveBalance.Size = New System.Drawing.Size(74, 21)
        Me.sbShowEveBalance.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.sbShowEveBalance.TabIndex = 15
        Me.sbShowEveBalance.Value = True
        '
        'btnJournalQuery
        '
        Me.btnJournalQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnJournalQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnJournalQuery.Location = New System.Drawing.Point(570, 8)
        Me.btnJournalQuery.Name = "btnJournalQuery"
        Me.btnJournalQuery.Size = New System.Drawing.Size(75, 23)
        Me.btnJournalQuery.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnJournalQuery.TabIndex = 14
        Me.btnJournalQuery.Text = "Get Journal"
        '
        'cboJournalRefTypes
        '
        Me.cboJournalRefTypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboJournalRefTypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboJournalRefTypes.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboJournalRefTypes.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboJournalRefTypes.ButtonDropDown.Visible = True
        Me.cboJournalRefTypes.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboJournalRefTypes.Location = New System.Drawing.Point(304, 35)
        Me.cboJournalRefTypes.Name = "cboJournalRefTypes"
        Me.cboJournalRefTypes.Size = New System.Drawing.Size(260, 21)
        Me.cboJournalRefTypes.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboJournalRefTypes.TabIndex = 13
        Me.cboJournalRefTypes.Text = ""
        Me.cboJournalRefTypes.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.cboJournalRefTypes.WatermarkColor = System.Drawing.Color.Silver
        Me.cboJournalRefTypes.WatermarkText = "Select journal category..."
        '
        'cboJournalOwners
        '
        Me.cboJournalOwners.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboJournalOwners.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboJournalOwners.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboJournalOwners.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboJournalOwners.ButtonDropDown.Visible = True
        Me.cboJournalOwners.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboJournalOwners.Location = New System.Drawing.Point(11, 9)
        Me.cboJournalOwners.Name = "cboJournalOwners"
        Me.cboJournalOwners.Size = New System.Drawing.Size(287, 21)
        Me.cboJournalOwners.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboJournalOwners.TabIndex = 12
        Me.cboJournalOwners.Text = ""
        Me.cboJournalOwners.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.cboJournalOwners.WatermarkColor = System.Drawing.Color.Silver
        Me.cboJournalOwners.WatermarkText = "Select owners..."
        '
        'lblJournalEndDate
        '
        Me.lblJournalEndDate.AutoSize = True
        Me.lblJournalEndDate.BackColor = System.Drawing.Color.Transparent
        Me.lblJournalEndDate.Location = New System.Drawing.Point(12, 65)
        Me.lblJournalEndDate.Name = "lblJournalEndDate"
        Me.lblJournalEndDate.Size = New System.Drawing.Size(55, 13)
        Me.lblJournalEndDate.TabIndex = 11
        Me.lblJournalEndDate.Text = "End Date:"
        '
        'lblJournalStartDate
        '
        Me.lblJournalStartDate.AutoSize = True
        Me.lblJournalStartDate.BackColor = System.Drawing.Color.Transparent
        Me.lblJournalStartDate.Location = New System.Drawing.Point(12, 38)
        Me.lblJournalStartDate.Name = "lblJournalStartDate"
        Me.lblJournalStartDate.Size = New System.Drawing.Size(61, 13)
        Me.lblJournalStartDate.TabIndex = 10
        Me.lblJournalStartDate.Text = "Start Date:"
        '
        'dtiJournalEndDate
        '
        '
        '
        '
        Me.dtiJournalEndDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiJournalEndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiJournalEndDate.ButtonCustom.Text = "Now"
        Me.dtiJournalEndDate.ButtonCustom.Visible = True
        Me.dtiJournalEndDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiJournalEndDate.ButtonCustom2.Text = "SoD"
        Me.dtiJournalEndDate.ButtonCustom2.Visible = True
        Me.dtiJournalEndDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiJournalEndDate.ButtonDropDown.Visible = True
        Me.dtiJournalEndDate.CustomFormat = "yyyy-MM-dd HH-mm-ss"
        Me.dtiJournalEndDate.Format = DevComponents.Editors.eDateTimePickerFormat.Custom
        Me.dtiJournalEndDate.Location = New System.Drawing.Point(98, 61)
        '
        '
        '
        Me.dtiJournalEndDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiJournalEndDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiJournalEndDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiJournalEndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiJournalEndDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiJournalEndDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiJournalEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiJournalEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiJournalEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiJournalEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiJournalEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiJournalEndDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiJournalEndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiJournalEndDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiJournalEndDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiJournalEndDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiJournalEndDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiJournalEndDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiJournalEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiJournalEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiJournalEndDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiJournalEndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiJournalEndDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiJournalEndDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiJournalEndDate.Name = "dtiJournalEndDate"
        Me.dtiJournalEndDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiJournalEndDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiJournalEndDate.TabIndex = 9
        Me.dtiJournalEndDate.Value = New Date(2010, 9, 15, 20, 35, 1, 0)
        '
        'dtiJournalStartDate
        '
        '
        '
        '
        Me.dtiJournalStartDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiJournalStartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiJournalStartDate.ButtonCustom.Text = "Now"
        Me.dtiJournalStartDate.ButtonCustom.Visible = True
        Me.dtiJournalStartDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiJournalStartDate.ButtonCustom2.Text = "SoD"
        Me.dtiJournalStartDate.ButtonCustom2.Visible = True
        Me.dtiJournalStartDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiJournalStartDate.ButtonDropDown.Visible = True
        Me.dtiJournalStartDate.CustomFormat = "yyyy-MM-dd HH-mm-ss"
        Me.dtiJournalStartDate.Format = DevComponents.Editors.eDateTimePickerFormat.Custom
        Me.dtiJournalStartDate.Location = New System.Drawing.Point(98, 34)
        '
        '
        '
        Me.dtiJournalStartDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiJournalStartDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiJournalStartDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiJournalStartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiJournalStartDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiJournalStartDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiJournalStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiJournalStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiJournalStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiJournalStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiJournalStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiJournalStartDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiJournalStartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiJournalStartDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiJournalStartDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiJournalStartDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiJournalStartDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiJournalStartDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiJournalStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiJournalStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiJournalStartDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiJournalStartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiJournalStartDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiJournalStartDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiJournalStartDate.Name = "dtiJournalStartDate"
        Me.dtiJournalStartDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiJournalStartDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiJournalStartDate.TabIndex = 8
        Me.dtiJournalStartDate.Value = New Date(2010, 9, 15, 20, 34, 46, 0)
        '
        'cboWalletJournalDivision
        '
        Me.cboWalletJournalDivision.DisplayMember = "Text"
        Me.cboWalletJournalDivision.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboWalletJournalDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWalletJournalDivision.FormattingEnabled = True
        Me.cboWalletJournalDivision.ItemHeight = 15
        Me.cboWalletJournalDivision.Location = New System.Drawing.Point(390, 9)
        Me.cboWalletJournalDivision.Name = "cboWalletJournalDivision"
        Me.cboWalletJournalDivision.Size = New System.Drawing.Size(174, 21)
        Me.cboWalletJournalDivision.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWalletJournalDivision.TabIndex = 7
        '
        'btnExportJournal
        '
        Me.btnExportJournal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportJournal.Location = New System.Drawing.Point(1202, 4)
        Me.btnExportJournal.Name = "btnExportJournal"
        Me.btnExportJournal.Size = New System.Drawing.Size(75, 23)
        Me.btnExportJournal.TabIndex = 6
        Me.btnExportJournal.Text = "Export"
        Me.btnExportJournal.UseVisualStyleBackColor = True
        '
        'lblWalletJournalDivision
        '
        Me.lblWalletJournalDivision.AutoSize = True
        Me.lblWalletJournalDivision.BackColor = System.Drawing.Color.Transparent
        Me.lblWalletJournalDivision.Location = New System.Drawing.Point(304, 13)
        Me.lblWalletJournalDivision.Name = "lblWalletJournalDivision"
        Me.lblWalletJournalDivision.Size = New System.Drawing.Size(80, 13)
        Me.lblWalletJournalDivision.TabIndex = 4
        Me.lblWalletJournalDivision.Text = "Wallet Division:"
        '
        'lblJobInstallerFilter
        '
        Me.lblJobInstallerFilter.AutoSize = True
        Me.lblJobInstallerFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblJobInstallerFilter.Location = New System.Drawing.Point(288, 10)
        Me.lblJobInstallerFilter.Name = "lblJobInstallerFilter"
        Me.lblJobInstallerFilter.Size = New System.Drawing.Size(92, 13)
        Me.lblJobInstallerFilter.TabIndex = 6
        Me.lblJobInstallerFilter.Text = "Filter By Installer:"
        '
        'btnExportJobs
        '
        Me.btnExportJobs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportJobs.Location = New System.Drawing.Point(1197, 4)
        Me.btnExportJobs.Name = "btnExportJobs"
        Me.btnExportJobs.Size = New System.Drawing.Size(75, 23)
        Me.btnExportJobs.TabIndex = 5
        Me.btnExportJobs.Text = "Export"
        Me.btnExportJobs.UseVisualStyleBackColor = True
        '
        'ctxRecycleExport
        '
        Me.ctxRecycleExport.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxRecycleExport.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExportToCSV, Me.mnuExportToTSV, Me.ToolStripMenuItem5, Me.mnuExportTotalsToCSV, Me.mnuExportTotalsToTSV})
        Me.ctxRecycleExport.Name = "ctxResourceExport"
        Me.ctxRecycleExport.Size = New System.Drawing.Size(271, 98)
        '
        'mnuExportToCSV
        '
        Me.mnuExportToCSV.Name = "mnuExportToCSV"
        Me.mnuExportToCSV.Size = New System.Drawing.Size(270, 22)
        Me.mnuExportToCSV.Text = "Copy Recycling Data to Clipboard (CSV)"
        '
        'mnuExportToTSV
        '
        Me.mnuExportToTSV.Name = "mnuExportToTSV"
        Me.mnuExportToTSV.Size = New System.Drawing.Size(270, 22)
        Me.mnuExportToTSV.Text = "Copy Recycling Data to Clipboard (TSV)"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(267, 6)
        '
        'mnuExportTotalsToCSV
        '
        Me.mnuExportTotalsToCSV.Name = "mnuExportTotalsToCSV"
        Me.mnuExportTotalsToCSV.Size = New System.Drawing.Size(270, 22)
        Me.mnuExportTotalsToCSV.Text = "Copy Recycling Totals to Clipboard (CSV)"
        '
        'mnuExportTotalsToTSV
        '
        Me.mnuExportTotalsToTSV.Name = "mnuExportTotalsToTSV"
        Me.mnuExportTotalsToTSV.Size = New System.Drawing.Size(270, 22)
        Me.mnuExportTotalsToTSV.Text = "Copy Recycling Totals to Clipboard (TSV)"
        '
        'chkFeesOnItems
        '
        Me.chkFeesOnItems.AutoSize = True
        Me.chkFeesOnItems.BackColor = System.Drawing.Color.Transparent
        Me.chkFeesOnItems.Location = New System.Drawing.Point(517, 39)
        Me.chkFeesOnItems.Name = "chkFeesOnItems"
        Me.chkFeesOnItems.Size = New System.Drawing.Size(94, 17)
        Me.chkFeesOnItems.TabIndex = 61
        Me.chkFeesOnItems.Text = "Fees on Items"
        Me.chkFeesOnItems.UseVisualStyleBackColor = False
        '
        'lblPriceTotals
        '
        Me.lblPriceTotals.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPriceTotals.AutoSize = True
        Me.lblPriceTotals.BackColor = System.Drawing.Color.Transparent
        Me.lblPriceTotals.Location = New System.Drawing.Point(7, 626)
        Me.lblPriceTotals.Name = "lblPriceTotals"
        Me.lblPriceTotals.Size = New System.Drawing.Size(135, 13)
        Me.lblPriceTotals.TabIndex = 60
        Me.lblPriceTotals.Text = "Sale / Refine / Best Totals:"
        '
        'chkFeesOnRefine
        '
        Me.chkFeesOnRefine.AutoSize = True
        Me.chkFeesOnRefine.BackColor = System.Drawing.Color.Transparent
        Me.chkFeesOnRefine.Location = New System.Drawing.Point(644, 39)
        Me.chkFeesOnRefine.Name = "chkFeesOnRefine"
        Me.chkFeesOnRefine.Size = New System.Drawing.Size(98, 17)
        Me.chkFeesOnRefine.TabIndex = 59
        Me.chkFeesOnRefine.Text = "Fees on Refine"
        Me.chkFeesOnRefine.UseVisualStyleBackColor = False
        '
        'lblTotalFees
        '
        Me.lblTotalFees.AutoSize = True
        Me.lblTotalFees.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalFees.Location = New System.Drawing.Point(839, 95)
        Me.lblTotalFees.Name = "lblTotalFees"
        Me.lblTotalFees.Size = New System.Drawing.Size(24, 13)
        Me.lblTotalFees.TabIndex = 58
        Me.lblTotalFees.Text = "0%"
        '
        'lblTotalFeesLbl
        '
        Me.lblTotalFeesLbl.AutoSize = True
        Me.lblTotalFeesLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalFeesLbl.Location = New System.Drawing.Point(762, 95)
        Me.lblTotalFeesLbl.Name = "lblTotalFeesLbl"
        Me.lblTotalFeesLbl.Size = New System.Drawing.Size(61, 13)
        Me.lblTotalFeesLbl.TabIndex = 57
        Me.lblTotalFeesLbl.Text = "Total Fees:"
        '
        'nudTax
        '
        Me.nudTax.DecimalPlaces = 4
        Me.nudTax.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudTax.Location = New System.Drawing.Point(668, 91)
        Me.nudTax.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudTax.Name = "nudTax"
        Me.nudTax.Size = New System.Drawing.Size(74, 21)
        Me.nudTax.TabIndex = 56
        '
        'nudBrokerFee
        '
        Me.nudBrokerFee.DecimalPlaces = 4
        Me.nudBrokerFee.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudBrokerFee.Location = New System.Drawing.Point(668, 64)
        Me.nudBrokerFee.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudBrokerFee.Name = "nudBrokerFee"
        Me.nudBrokerFee.Size = New System.Drawing.Size(74, 21)
        Me.nudBrokerFee.TabIndex = 55
        '
        'chkOverrideTax
        '
        Me.chkOverrideTax.AutoSize = True
        Me.chkOverrideTax.BackColor = System.Drawing.Color.Transparent
        Me.chkOverrideTax.Location = New System.Drawing.Point(517, 94)
        Me.chkOverrideTax.Name = "chkOverrideTax"
        Me.chkOverrideTax.Size = New System.Drawing.Size(111, 17)
        Me.chkOverrideTax.TabIndex = 54
        Me.chkOverrideTax.Text = "Override Tax (%)"
        Me.chkOverrideTax.UseVisualStyleBackColor = False
        '
        'chkOverrideBrokerFee
        '
        Me.chkOverrideBrokerFee.AutoSize = True
        Me.chkOverrideBrokerFee.BackColor = System.Drawing.Color.Transparent
        Me.chkOverrideBrokerFee.Location = New System.Drawing.Point(517, 65)
        Me.chkOverrideBrokerFee.Name = "chkOverrideBrokerFee"
        Me.chkOverrideBrokerFee.Size = New System.Drawing.Size(145, 17)
        Me.chkOverrideBrokerFee.TabIndex = 53
        Me.chkOverrideBrokerFee.Text = "Override Broker Fee (%)"
        Me.chkOverrideBrokerFee.UseVisualStyleBackColor = False
        '
        'lblItems
        '
        Me.lblItems.AutoSize = True
        Me.lblItems.BackColor = System.Drawing.Color.Transparent
        Me.lblItems.Location = New System.Drawing.Point(63, 95)
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(13, 13)
        Me.lblItems.TabIndex = 52
        Me.lblItems.Text = "0"
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.BackColor = System.Drawing.Color.Transparent
        Me.lblVolume.Location = New System.Drawing.Point(63, 74)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(29, 13)
        Me.lblVolume.TabIndex = 51
        Me.lblVolume.Text = "0 m�"
        '
        'lblItemsLbl
        '
        Me.lblItemsLbl.AutoSize = True
        Me.lblItemsLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblItemsLbl.Location = New System.Drawing.Point(12, 95)
        Me.lblItemsLbl.Name = "lblItemsLbl"
        Me.lblItemsLbl.Size = New System.Drawing.Size(38, 13)
        Me.lblItemsLbl.TabIndex = 50
        Me.lblItemsLbl.Text = "Items:"
        '
        'lblVolumeLbl
        '
        Me.lblVolumeLbl.AutoSize = True
        Me.lblVolumeLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblVolumeLbl.Location = New System.Drawing.Point(12, 74)
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
        Me.cboRefineMode.Location = New System.Drawing.Point(55, 9)
        Me.cboRefineMode.Name = "cboRefineMode"
        Me.cboRefineMode.Size = New System.Drawing.Size(165, 21)
        Me.cboRefineMode.TabIndex = 48
        '
        'lblRefineMode
        '
        Me.lblRefineMode.AutoSize = True
        Me.lblRefineMode.BackColor = System.Drawing.Color.Transparent
        Me.lblRefineMode.Location = New System.Drawing.Point(12, 12)
        Me.lblRefineMode.Name = "lblRefineMode"
        Me.lblRefineMode.Size = New System.Drawing.Size(37, 13)
        Me.lblRefineMode.TabIndex = 47
        Me.lblRefineMode.Text = "Mode:"
        '
        'chkOverrideStandings
        '
        Me.chkOverrideStandings.AutoSize = True
        Me.chkOverrideStandings.BackColor = System.Drawing.Color.Transparent
        Me.chkOverrideStandings.Location = New System.Drawing.Point(259, 92)
        Me.chkOverrideStandings.Name = "chkOverrideStandings"
        Me.chkOverrideStandings.Size = New System.Drawing.Size(118, 17)
        Me.chkOverrideStandings.TabIndex = 46
        Me.chkOverrideStandings.Text = "Override Standings"
        Me.chkOverrideStandings.UseVisualStyleBackColor = False
        '
        'chkOverrideBaseYield
        '
        Me.chkOverrideBaseYield.AutoSize = True
        Me.chkOverrideBaseYield.BackColor = System.Drawing.Color.Transparent
        Me.chkOverrideBaseYield.Location = New System.Drawing.Point(259, 65)
        Me.chkOverrideBaseYield.Name = "chkOverrideBaseYield"
        Me.chkOverrideBaseYield.Size = New System.Drawing.Size(119, 17)
        Me.chkOverrideBaseYield.TabIndex = 45
        Me.chkOverrideBaseYield.Text = "Override Base Yield"
        Me.chkOverrideBaseYield.UseVisualStyleBackColor = False
        '
        'nudStandings
        '
        Me.nudStandings.DecimalPlaces = 4
        Me.nudStandings.Location = New System.Drawing.Point(410, 91)
        Me.nudStandings.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudStandings.Name = "nudStandings"
        Me.nudStandings.Size = New System.Drawing.Size(74, 21)
        Me.nudStandings.TabIndex = 44
        '
        'nudBaseYield
        '
        Me.nudBaseYield.DecimalPlaces = 2
        Me.nudBaseYield.Location = New System.Drawing.Point(410, 64)
        Me.nudBaseYield.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudBaseYield.Name = "nudBaseYield"
        Me.nudBaseYield.Size = New System.Drawing.Size(74, 21)
        Me.nudBaseYield.TabIndex = 43
        Me.nudBaseYield.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'lblCorp
        '
        Me.lblCorp.AutoSize = True
        Me.lblCorp.BackColor = System.Drawing.Color.Transparent
        Me.lblCorp.Location = New System.Drawing.Point(305, 17)
        Me.lblCorp.Name = "lblCorp"
        Me.lblCorp.Size = New System.Drawing.Size(23, 13)
        Me.lblCorp.TabIndex = 42
        Me.lblCorp.Text = "n/a"
        '
        'lblCorpLbl
        '
        Me.lblCorpLbl.AutoSize = True
        Me.lblCorpLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblCorpLbl.Location = New System.Drawing.Point(256, 17)
        Me.lblCorpLbl.Name = "lblCorpLbl"
        Me.lblCorpLbl.Size = New System.Drawing.Size(34, 13)
        Me.lblCorpLbl.TabIndex = 41
        Me.lblCorpLbl.Text = "Corp:"
        '
        'lblStation
        '
        Me.lblStation.AutoSize = True
        Me.lblStation.BackColor = System.Drawing.Color.Transparent
        Me.lblStation.Location = New System.Drawing.Point(305, 4)
        Me.lblStation.Name = "lblStation"
        Me.lblStation.Size = New System.Drawing.Size(23, 13)
        Me.lblStation.TabIndex = 40
        Me.lblStation.Text = "n/a"
        '
        'lblStationLbl
        '
        Me.lblStationLbl.AutoSize = True
        Me.lblStationLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblStationLbl.Location = New System.Drawing.Point(256, 4)
        Me.lblStationLbl.Name = "lblStationLbl"
        Me.lblStationLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblStationLbl.TabIndex = 39
        Me.lblStationLbl.Text = "Station:"
        '
        'lblBaseYield
        '
        Me.lblBaseYield.AutoSize = True
        Me.lblBaseYield.BackColor = System.Drawing.Color.Transparent
        Me.lblBaseYield.Location = New System.Drawing.Point(839, 43)
        Me.lblBaseYield.Name = "lblBaseYield"
        Me.lblBaseYield.Size = New System.Drawing.Size(24, 13)
        Me.lblBaseYield.TabIndex = 38
        Me.lblBaseYield.Text = "0%"
        '
        'lblNetYield
        '
        Me.lblNetYield.AutoSize = True
        Me.lblNetYield.Location = New System.Drawing.Point(839, 56)
        Me.lblNetYield.Name = "lblNetYield"
        Me.lblNetYield.Size = New System.Drawing.Size(24, 13)
        Me.lblNetYield.TabIndex = 37
        Me.lblNetYield.Text = "0%"
        '
        'lblStandings
        '
        Me.lblStandings.AutoSize = True
        Me.lblStandings.BackColor = System.Drawing.Color.Transparent
        Me.lblStandings.Location = New System.Drawing.Point(839, 69)
        Me.lblStandings.Name = "lblStandings"
        Me.lblStandings.Size = New System.Drawing.Size(13, 13)
        Me.lblStandings.TabIndex = 36
        Me.lblStandings.Text = "0"
        '
        'lblStationTake
        '
        Me.lblStationTake.AutoSize = True
        Me.lblStationTake.BackColor = System.Drawing.Color.Transparent
        Me.lblStationTake.Location = New System.Drawing.Point(839, 82)
        Me.lblStationTake.Name = "lblStationTake"
        Me.lblStationTake.Size = New System.Drawing.Size(24, 13)
        Me.lblStationTake.TabIndex = 35
        Me.lblStationTake.Text = "0%"
        '
        'lblStationTakeLbl
        '
        Me.lblStationTakeLbl.AutoSize = True
        Me.lblStationTakeLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblStationTakeLbl.Location = New System.Drawing.Point(762, 82)
        Me.lblStationTakeLbl.Name = "lblStationTakeLbl"
        Me.lblStationTakeLbl.Size = New System.Drawing.Size(71, 13)
        Me.lblStationTakeLbl.TabIndex = 34
        Me.lblStationTakeLbl.Text = "Station Take:"
        '
        'lblStandingsLbl
        '
        Me.lblStandingsLbl.AutoSize = True
        Me.lblStandingsLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblStandingsLbl.Location = New System.Drawing.Point(762, 69)
        Me.lblStandingsLbl.Name = "lblStandingsLbl"
        Me.lblStandingsLbl.Size = New System.Drawing.Size(58, 13)
        Me.lblStandingsLbl.TabIndex = 33
        Me.lblStandingsLbl.Text = "Standings:"
        '
        'lblNetYieldLbl
        '
        Me.lblNetYieldLbl.AutoSize = True
        Me.lblNetYieldLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblNetYieldLbl.Location = New System.Drawing.Point(762, 56)
        Me.lblNetYieldLbl.Name = "lblNetYieldLbl"
        Me.lblNetYieldLbl.Size = New System.Drawing.Size(53, 13)
        Me.lblNetYieldLbl.TabIndex = 32
        Me.lblNetYieldLbl.Text = "Net Yield:"
        '
        'lblBaseYieldLbl
        '
        Me.lblBaseYieldLbl.AutoSize = True
        Me.lblBaseYieldLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblBaseYieldLbl.Location = New System.Drawing.Point(762, 43)
        Me.lblBaseYieldLbl.Name = "lblBaseYieldLbl"
        Me.lblBaseYieldLbl.Size = New System.Drawing.Size(59, 13)
        Me.lblBaseYieldLbl.TabIndex = 31
        Me.lblBaseYieldLbl.Text = "Base Yield:"
        '
        'chkPerfectRefine
        '
        Me.chkPerfectRefine.AutoSize = True
        Me.chkPerfectRefine.BackColor = System.Drawing.Color.Transparent
        Me.chkPerfectRefine.Location = New System.Drawing.Point(259, 39)
        Me.chkPerfectRefine.Name = "chkPerfectRefine"
        Me.chkPerfectRefine.Size = New System.Drawing.Size(95, 17)
        Me.chkPerfectRefine.TabIndex = 30
        Me.chkPerfectRefine.Text = "Perfect Refine"
        Me.chkPerfectRefine.UseVisualStyleBackColor = False
        '
        'cboRecyclePilots
        '
        Me.cboRecyclePilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRecyclePilots.FormattingEnabled = True
        Me.cboRecyclePilots.Location = New System.Drawing.Point(55, 40)
        Me.cboRecyclePilots.Name = "cboRecyclePilots"
        Me.cboRecyclePilots.Size = New System.Drawing.Size(165, 21)
        Me.cboRecyclePilots.Sorted = True
        Me.cboRecyclePilots.TabIndex = 29
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.BackColor = System.Drawing.Color.Transparent
        Me.lblPilot.Location = New System.Drawing.Point(12, 43)
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
        Me.TabControl1.Location = New System.Drawing.Point(3, 118)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1270, 497)
        Me.TabControl1.TabIndex = 27
        '
        'tabItems
        '
        Me.tabItems.Controls.Add(Me.adtRecycle)
        Me.tabItems.Location = New System.Drawing.Point(4, 22)
        Me.tabItems.Name = "tabItems"
        Me.tabItems.Padding = New System.Windows.Forms.Padding(3)
        Me.tabItems.Size = New System.Drawing.Size(1262, 471)
        Me.tabItems.TabIndex = 0
        Me.tabItems.Text = "Item Analysis"
        Me.tabItems.UseVisualStyleBackColor = True
        '
        'adtRecycle
        '
        Me.adtRecycle.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtRecycle.AllowDrop = True
        Me.adtRecycle.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtRecycle.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtRecycle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtRecycle.Columns.Add(Me.colRecItem)
        Me.adtRecycle.Columns.Add(Me.colRecMeta)
        Me.adtRecycle.Columns.Add(Me.colRecQty)
        Me.adtRecycle.Columns.Add(Me.colRecBatches)
        Me.adtRecycle.Columns.Add(Me.colRecPrice)
        Me.adtRecycle.Columns.Add(Me.colRecValue)
        Me.adtRecycle.Columns.Add(Me.colRecFees)
        Me.adtRecycle.Columns.Add(Me.colRecSalePrice)
        Me.adtRecycle.Columns.Add(Me.colRecRefinePrice)
        Me.adtRecycle.Columns.Add(Me.colRecBestPrice)
        Me.adtRecycle.Columns.Add(Me.colRecTotalBen)
        Me.adtRecycle.Columns.Add(Me.colRecUnitBen)
        Me.adtRecycle.ContextMenuStrip = Me.ctxRecycleItem
        Me.adtRecycle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtRecycle.DragDropEnabled = False
        Me.adtRecycle.DragDropNodeCopyEnabled = False
        Me.adtRecycle.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtRecycle.Location = New System.Drawing.Point(3, 3)
        Me.adtRecycle.Name = "adtRecycle"
        Me.adtRecycle.NodesConnector = Me.NodeConnector12
        Me.adtRecycle.NodeStyle = Me.ItemNormal
        Me.adtRecycle.PathSeparator = ";"
        Me.adtRecycle.Size = New System.Drawing.Size(1256, 465)
        Me.adtRecycle.Styles.Add(Me.ItemNormal)
        Me.adtRecycle.Styles.Add(Me.ItemGood)
        Me.adtRecycle.TabIndex = 0
        Me.adtRecycle.Text = "AdvTree1"
        '
        'colRecItem
        '
        Me.colRecItem.DisplayIndex = 1
        Me.colRecItem.Name = "colRecItem"
        Me.colRecItem.Text = "Item"
        Me.colRecItem.Width.Absolute = 250
        '
        'colRecMeta
        '
        Me.colRecMeta.DisplayIndex = 2
        Me.colRecMeta.Name = "colRecMeta"
        Me.colRecMeta.Text = "Meta"
        Me.colRecMeta.Width.Absolute = 50
        '
        'colRecQty
        '
        Me.colRecQty.DisplayIndex = 3
        Me.colRecQty.Name = "colRecQty"
        Me.colRecQty.Text = "Quantity"
        Me.colRecQty.Width.Absolute = 75
        '
        'colRecBatches
        '
        Me.colRecBatches.DisplayIndex = 4
        Me.colRecBatches.Name = "colRecBatches"
        Me.colRecBatches.Text = "Batches"
        Me.colRecBatches.Width.Absolute = 50
        '
        'colRecPrice
        '
        Me.colRecPrice.DisplayIndex = 5
        Me.colRecPrice.Name = "colRecPrice"
        Me.colRecPrice.Text = "Item Price"
        Me.colRecPrice.Width.Absolute = 125
        '
        'colRecValue
        '
        Me.colRecValue.DisplayIndex = 6
        Me.colRecValue.Name = "colRecValue"
        Me.colRecValue.Text = "Total Price"
        Me.colRecValue.Width.Absolute = 125
        '
        'colRecFees
        '
        Me.colRecFees.DisplayIndex = 7
        Me.colRecFees.Name = "colRecFees"
        Me.colRecFees.Text = "Sale Fees"
        Me.colRecFees.Width.Absolute = 125
        '
        'colRecSalePrice
        '
        Me.colRecSalePrice.DisplayIndex = 8
        Me.colRecSalePrice.Name = "colRecSalePrice"
        Me.colRecSalePrice.Text = "Sale Price"
        Me.colRecSalePrice.Width.Absolute = 125
        '
        'colRecRefinePrice
        '
        Me.colRecRefinePrice.DisplayIndex = 9
        Me.colRecRefinePrice.Name = "colRecRefinePrice"
        Me.colRecRefinePrice.Text = "Refine Price"
        Me.colRecRefinePrice.Width.Absolute = 125
        '
        'colRecBestPrice
        '
        Me.colRecBestPrice.DisplayIndex = 10
        Me.colRecBestPrice.Name = "colRecBestPrice"
        Me.colRecBestPrice.Text = "Best Price"
        Me.colRecBestPrice.Width.Absolute = 125
        '
        'colRecTotalBen
        '
        Me.colRecTotalBen.DisplayIndex = 11
        Me.colRecTotalBen.Name = "colRecTotalBen"
        Me.colRecTotalBen.Text = "Total Benefit"
        Me.colRecTotalBen.Width.Absolute = 125
        '
        'colRecUnitBen
        '
        Me.colRecUnitBen.DisplayIndex = 12
        Me.colRecUnitBen.Name = "colRecUnitBen"
        Me.colRecUnitBen.Text = "Unit Benefit"
        Me.colRecUnitBen.Width.Absolute = 125
        '
        'ctxRecycleItem
        '
        Me.ctxRecycleItem.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxRecycleItem.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddRecycleItem, Me.ToolStripMenuItem2, Me.mnuAlterRecycleQuantity, Me.mnuRemoveRecycleItem})
        Me.ctxRecycleItem.Name = "ctxRecycleItem"
        Me.ctxRecycleItem.Size = New System.Drawing.Size(143, 76)
        '
        'mnuAddRecycleItem
        '
        Me.mnuAddRecycleItem.Name = "mnuAddRecycleItem"
        Me.mnuAddRecycleItem.Size = New System.Drawing.Size(142, 22)
        Me.mnuAddRecycleItem.Text = "Add Item"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(139, 6)
        '
        'mnuAlterRecycleQuantity
        '
        Me.mnuAlterRecycleQuantity.Name = "mnuAlterRecycleQuantity"
        Me.mnuAlterRecycleQuantity.Size = New System.Drawing.Size(142, 22)
        Me.mnuAlterRecycleQuantity.Text = "Alter Quantity"
        '
        'mnuRemoveRecycleItem
        '
        Me.mnuRemoveRecycleItem.Name = "mnuRemoveRecycleItem"
        Me.mnuRemoveRecycleItem.Size = New System.Drawing.Size(142, 22)
        Me.mnuRemoveRecycleItem.Text = "Remove Item"
        '
        'NodeConnector12
        '
        Me.NodeConnector12.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ItemNormal
        '
        Me.ItemNormal.Class = ""
        Me.ItemNormal.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemNormal.Name = "ItemNormal"
        Me.ItemNormal.TextColor = System.Drawing.SystemColors.ControlText
        '
        'ItemGood
        '
        Me.ItemGood.BackColor = System.Drawing.Color.LightGreen
        Me.ItemGood.BackColor2 = System.Drawing.Color.LimeGreen
        Me.ItemGood.BackColorGradientAngle = 90
        Me.ItemGood.BackColorGradientType = DevComponents.DotNetBar.eGradientType.Radial
        Me.ItemGood.Class = ""
        Me.ItemGood.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemGood.Name = "ItemGood"
        Me.ItemGood.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tabTotals
        '
        Me.tabTotals.Controls.Add(Me.adtTotals)
        Me.tabTotals.Location = New System.Drawing.Point(4, 22)
        Me.tabTotals.Name = "tabTotals"
        Me.tabTotals.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTotals.Size = New System.Drawing.Size(1262, 471)
        Me.tabTotals.TabIndex = 1
        Me.tabTotals.Text = "Recycling Totals"
        Me.tabTotals.UseVisualStyleBackColor = True
        '
        'adtTotals
        '
        Me.adtTotals.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtTotals.AllowDrop = True
        Me.adtTotals.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtTotals.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtTotals.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtTotals.Columns.Add(Me.colRTMaterial)
        Me.adtTotals.Columns.Add(Me.colRTStationTake)
        Me.adtTotals.Columns.Add(Me.colRTUnrecoverable)
        Me.adtTotals.Columns.Add(Me.colRTReceivable)
        Me.adtTotals.Columns.Add(Me.colRTPrice)
        Me.adtTotals.Columns.Add(Me.colRTTotal)
        Me.adtTotals.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtTotals.DragDropEnabled = False
        Me.adtTotals.DragDropNodeCopyEnabled = False
        Me.adtTotals.ExpandWidth = 0
        Me.adtTotals.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtTotals.Location = New System.Drawing.Point(3, 3)
        Me.adtTotals.Name = "adtTotals"
        Me.adtTotals.NodesConnector = Me.NodeConnector11
        Me.adtTotals.NodeStyle = Me.ElementStyle8
        Me.adtTotals.PathSeparator = ";"
        Me.adtTotals.Size = New System.Drawing.Size(1256, 465)
        Me.adtTotals.Styles.Add(Me.ElementStyle8)
        Me.adtTotals.TabIndex = 0
        Me.adtTotals.Text = "AdvTree1"
        '
        'colRTMaterial
        '
        Me.colRTMaterial.DisplayIndex = 1
        Me.colRTMaterial.Name = "colRTMaterial"
        Me.colRTMaterial.Text = "Material"
        Me.colRTMaterial.Width.Absolute = 300
        '
        'colRTStationTake
        '
        Me.colRTStationTake.DisplayIndex = 2
        Me.colRTStationTake.Name = "colRTStationTake"
        Me.colRTStationTake.Text = "Station Take"
        Me.colRTStationTake.Width.Absolute = 150
        '
        'colRTUnrecoverable
        '
        Me.colRTUnrecoverable.DisplayIndex = 3
        Me.colRTUnrecoverable.Name = "colRTUnrecoverable"
        Me.colRTUnrecoverable.Text = "Unrecoverable"
        Me.colRTUnrecoverable.Width.Absolute = 150
        '
        'colRTReceivable
        '
        Me.colRTReceivable.DisplayIndex = 4
        Me.colRTReceivable.Name = "colRTReceivable"
        Me.colRTReceivable.Text = "Receivable"
        Me.colRTReceivable.Width.Absolute = 150
        '
        'colRTPrice
        '
        Me.colRTPrice.DisplayIndex = 5
        Me.colRTPrice.Name = "colRTPrice"
        Me.colRTPrice.Text = "Price"
        Me.colRTPrice.Width.Absolute = 150
        '
        'colRTTotal
        '
        Me.colRTTotal.DisplayIndex = 6
        Me.colRTTotal.Name = "colRTTotal"
        Me.colRTTotal.Text = "Total"
        Me.colRTTotal.Width.Absolute = 150
        '
        'NodeConnector11
        '
        Me.NodeConnector11.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle8
        '
        Me.ElementStyle8.Class = ""
        Me.ElementStyle8.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle8.Name = "ElementStyle8"
        Me.ElementStyle8.TextColor = System.Drawing.SystemColors.ControlText
        '
        'cboCategoryFilter
        '
        Me.cboCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategoryFilter.FormattingEnabled = True
        Me.cboCategoryFilter.Items.AddRange(New Object() {"All", "Unknown", "BPO", "BPC", "Custom"})
        Me.cboCategoryFilter.Location = New System.Drawing.Point(786, 2)
        Me.cboCategoryFilter.Name = "cboCategoryFilter"
        Me.cboCategoryFilter.Size = New System.Drawing.Size(100, 21)
        Me.cboCategoryFilter.TabIndex = 53
        '
        'lblBPCatFilter
        '
        Me.lblBPCatFilter.AutoSize = True
        Me.lblBPCatFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblBPCatFilter.Location = New System.Drawing.Point(697, 6)
        Me.lblBPCatFilter.Name = "lblBPCatFilter"
        Me.lblBPCatFilter.Size = New System.Drawing.Size(83, 13)
        Me.lblBPCatFilter.TabIndex = 52
        Me.lblBPCatFilter.Text = "Category Filter:"
        '
        'cboTypeFilter
        '
        Me.cboTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTypeFilter.FormattingEnabled = True
        Me.cboTypeFilter.Items.AddRange(New Object() {"All", "Unknown", "BPO", "BPC", "Custom"})
        Me.cboTypeFilter.Location = New System.Drawing.Point(611, 2)
        Me.cboTypeFilter.Name = "cboTypeFilter"
        Me.cboTypeFilter.Size = New System.Drawing.Size(80, 21)
        Me.cboTypeFilter.TabIndex = 51
        '
        'lblTypeFilter
        '
        Me.lblTypeFilter.AutoSize = True
        Me.lblTypeFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblTypeFilter.Location = New System.Drawing.Point(543, 5)
        Me.lblTypeFilter.Name = "lblTypeFilter"
        Me.lblTypeFilter.Size = New System.Drawing.Size(62, 13)
        Me.lblTypeFilter.TabIndex = 50
        Me.lblTypeFilter.Text = "Type Filter:"
        '
        'cboTechFilter
        '
        Me.cboTechFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTechFilter.FormattingEnabled = True
        Me.cboTechFilter.Items.AddRange(New Object() {"All", "1", "2", "3"})
        Me.cboTechFilter.Location = New System.Drawing.Point(487, 2)
        Me.cboTechFilter.Name = "cboTechFilter"
        Me.cboTechFilter.Size = New System.Drawing.Size(50, 21)
        Me.cboTechFilter.TabIndex = 49
        '
        'lblTechFilter
        '
        Me.lblTechFilter.AutoSize = True
        Me.lblTechFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblTechFilter.Location = New System.Drawing.Point(420, 5)
        Me.lblTechFilter.Name = "lblTechFilter"
        Me.lblTechFilter.Size = New System.Drawing.Size(61, 13)
        Me.lblTechFilter.TabIndex = 48
        Me.lblTechFilter.Text = "Tech Filter:"
        '
        'lblUserBP
        '
        Me.lblUserBP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblUserBP.AutoSize = True
        Me.lblUserBP.BackColor = System.Drawing.Color.Transparent
        Me.lblUserBP.Location = New System.Drawing.Point(1214, 543)
        Me.lblUserBP.Name = "lblUserBP"
        Me.lblUserBP.Size = New System.Drawing.Size(43, 13)
        Me.lblUserBP.TabIndex = 47
        Me.lblUserBP.Text = "Custom"
        '
        'btnAddCustomBP
        '
        Me.btnAddCustomBP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddCustomBP.Location = New System.Drawing.Point(1192, 165)
        Me.btnAddCustomBP.Name = "btnAddCustomBP"
        Me.btnAddCustomBP.Size = New System.Drawing.Size(80, 40)
        Me.btnAddCustomBP.TabIndex = 45
        Me.btnAddCustomBP.Text = "Add Custom Blueprint"
        Me.ToolTip1.SetToolTip(Me.btnAddCustomBP, "Add a custom defined blueprint for theoretical work")
        Me.btnAddCustomBP.UseVisualStyleBackColor = True
        '
        'lblExhausted
        '
        Me.lblExhausted.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblExhausted.AutoSize = True
        Me.lblExhausted.BackColor = System.Drawing.Color.Transparent
        Me.lblExhausted.Location = New System.Drawing.Point(1214, 587)
        Me.lblExhausted.Name = "lblExhausted"
        Me.lblExhausted.Size = New System.Drawing.Size(58, 13)
        Me.lblExhausted.TabIndex = 44
        Me.lblExhausted.Text = "Exhausted"
        '
        'lblUnknown
        '
        Me.lblUnknown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblUnknown.AutoSize = True
        Me.lblUnknown.BackColor = System.Drawing.Color.Transparent
        Me.lblUnknown.Location = New System.Drawing.Point(1214, 609)
        Me.lblUnknown.Name = "lblUnknown"
        Me.lblUnknown.Size = New System.Drawing.Size(51, 13)
        Me.lblUnknown.TabIndex = 42
        Me.lblUnknown.Text = "Unknown"
        '
        'lblMissing
        '
        Me.lblMissing.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMissing.AutoSize = True
        Me.lblMissing.BackColor = System.Drawing.Color.Transparent
        Me.lblMissing.Location = New System.Drawing.Point(1214, 565)
        Me.lblMissing.Name = "lblMissing"
        Me.lblMissing.Size = New System.Drawing.Size(41, 13)
        Me.lblMissing.TabIndex = 40
        Me.lblMissing.Text = "Missing"
        '
        'lblBPC
        '
        Me.lblBPC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBPC.AutoSize = True
        Me.lblBPC.BackColor = System.Drawing.Color.Transparent
        Me.lblBPC.Location = New System.Drawing.Point(1214, 521)
        Me.lblBPC.Name = "lblBPC"
        Me.lblBPC.Size = New System.Drawing.Size(26, 13)
        Me.lblBPC.TabIndex = 38
        Me.lblBPC.Text = "BPC"
        '
        'lblBPO
        '
        Me.lblBPO.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBPO.AutoSize = True
        Me.lblBPO.BackColor = System.Drawing.Color.Transparent
        Me.lblBPO.Location = New System.Drawing.Point(1214, 499)
        Me.lblBPO.Name = "lblBPO"
        Me.lblBPO.Size = New System.Drawing.Size(27, 13)
        Me.lblBPO.TabIndex = 36
        Me.lblBPO.Text = "BPO"
        '
        'btnResetBPSearch
        '
        Me.btnResetBPSearch.Location = New System.Drawing.Point(1063, 3)
        Me.btnResetBPSearch.Name = "btnResetBPSearch"
        Me.btnResetBPSearch.Size = New System.Drawing.Size(50, 21)
        Me.btnResetBPSearch.TabIndex = 34
        Me.btnResetBPSearch.Text = "Reset"
        Me.btnResetBPSearch.UseVisualStyleBackColor = True
        '
        'txtBPSearch
        '
        Me.txtBPSearch.Location = New System.Drawing.Point(942, 2)
        Me.txtBPSearch.Name = "txtBPSearch"
        Me.txtBPSearch.Size = New System.Drawing.Size(115, 21)
        Me.txtBPSearch.TabIndex = 33
        '
        'lblBPSearch
        '
        Me.lblBPSearch.AutoSize = True
        Me.lblBPSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblBPSearch.Location = New System.Drawing.Point(892, 6)
        Me.lblBPSearch.Name = "lblBPSearch"
        Me.lblBPSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblBPSearch.TabIndex = 32
        Me.lblBPSearch.Text = "Search:"
        '
        'btnGetBPJobInfo
        '
        Me.btnGetBPJobInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGetBPJobInfo.Location = New System.Drawing.Point(1192, 119)
        Me.btnGetBPJobInfo.Name = "btnGetBPJobInfo"
        Me.btnGetBPJobInfo.Size = New System.Drawing.Size(80, 40)
        Me.btnGetBPJobInfo.TabIndex = 31
        Me.btnGetBPJobInfo.Text = "Get BP Info From Jobs"
        Me.ToolTip1.SetToolTip(Me.btnGetBPJobInfo, "Imports Blueprint details from the Assets API")
        Me.btnGetBPJobInfo.UseVisualStyleBackColor = True
        '
        'btnUpdateBPsFromAssets
        '
        Me.btnUpdateBPsFromAssets.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdateBPsFromAssets.Location = New System.Drawing.Point(1192, 73)
        Me.btnUpdateBPsFromAssets.Name = "btnUpdateBPsFromAssets"
        Me.btnUpdateBPsFromAssets.Size = New System.Drawing.Size(80, 40)
        Me.btnUpdateBPsFromAssets.TabIndex = 30
        Me.btnUpdateBPsFromAssets.Text = "Update BPs From Assets"
        Me.ToolTip1.SetToolTip(Me.btnUpdateBPsFromAssets, "Imports Blueprint details from the Assets API")
        Me.btnUpdateBPsFromAssets.UseVisualStyleBackColor = True
        '
        'btnBPCalc
        '
        Me.btnBPCalc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBPCalc.Location = New System.Drawing.Point(1192, 27)
        Me.btnBPCalc.Name = "btnBPCalc"
        Me.btnBPCalc.Size = New System.Drawing.Size(80, 40)
        Me.btnBPCalc.TabIndex = 28
        Me.btnBPCalc.Text = "Blueprint Calculator"
        Me.ToolTip1.SetToolTip(Me.btnBPCalc, "Starts the Blueprint production and research calculator")
        Me.btnBPCalc.UseVisualStyleBackColor = True
        '
        'chkShowOwnedBPs
        '
        Me.chkShowOwnedBPs.AutoSize = True
        Me.chkShowOwnedBPs.BackColor = System.Drawing.Color.Transparent
        Me.chkShowOwnedBPs.Checked = True
        Me.chkShowOwnedBPs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowOwnedBPs.Location = New System.Drawing.Point(280, 5)
        Me.chkShowOwnedBPs.Name = "chkShowOwnedBPs"
        Me.chkShowOwnedBPs.Size = New System.Drawing.Size(134, 17)
        Me.chkShowOwnedBPs.TabIndex = 27
        Me.chkShowOwnedBPs.Text = "Only Show Owned BPs"
        Me.chkShowOwnedBPs.UseVisualStyleBackColor = False
        '
        'ctxBPManager
        '
        Me.ctxBPManager.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxBPManager.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSendToBPCalc, Me.ToolStripMenuItem4, Me.mnuAmendBPDetails, Me.mnuRemoveCustomBP})
        Me.ctxBPManager.Name = "ctxBPManager"
        Me.ctxBPManager.Size = New System.Drawing.Size(208, 76)
        '
        'mnuSendToBPCalc
        '
        Me.mnuSendToBPCalc.Name = "mnuSendToBPCalc"
        Me.mnuSendToBPCalc.Size = New System.Drawing.Size(207, 22)
        Me.mnuSendToBPCalc.Text = "Send to Blueprint Calculator"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(204, 6)
        '
        'mnuAmendBPDetails
        '
        Me.mnuAmendBPDetails.Name = "mnuAmendBPDetails"
        Me.mnuAmendBPDetails.Size = New System.Drawing.Size(207, 22)
        Me.mnuAmendBPDetails.Text = "Amend Blueprint Details"
        '
        'mnuRemoveCustomBP
        '
        Me.mnuRemoveCustomBP.Name = "mnuRemoveCustomBP"
        Me.mnuRemoveCustomBP.Size = New System.Drawing.Size(207, 22)
        Me.mnuRemoveCustomBP.Text = "Remove Custom Blueprint"
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
        'btnCopyListToClipboard
        '
        Me.btnCopyListToClipboard.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopyListToClipboard.Location = New System.Drawing.Point(1192, 211)
        Me.btnCopyListToClipboard.Name = "btnCopyListToClipboard"
        Me.btnCopyListToClipboard.Size = New System.Drawing.Size(80, 40)
        Me.btnCopyListToClipboard.TabIndex = 58
        Me.btnCopyListToClipboard.Text = "Copy BP List to Clipboard"
        Me.ToolTip1.SetToolTip(Me.btnCopyListToClipboard, "Copies the current BP list to the clipboard")
        Me.btnCopyListToClipboard.UseVisualStyleBackColor = True
        '
        'RibbonBarMergeContainer1
        '
        Me.RibbonBarMergeContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbProduction)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbQuickCalcs)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbAnalysisTools)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbMarketTools)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbAssetManagement)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbWallet)
        Me.RibbonBarMergeContainer1.Controls.Add(Me.rbData)
        Me.RibbonBarMergeContainer1.Location = New System.Drawing.Point(15, 12)
        Me.RibbonBarMergeContainer1.Name = "RibbonBarMergeContainer1"
        Me.RibbonBarMergeContainer1.RibbonTabText = "Prism"
        Me.RibbonBarMergeContainer1.Size = New System.Drawing.Size(1242, 100)
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
        Me.RibbonBarMergeContainer1.TabIndex = 17
        Me.RibbonBarMergeContainer1.Visible = False
        '
        'rbProduction
        '
        Me.rbProduction.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbProduction.BackgroundMouseOverStyle.Class = ""
        Me.rbProduction.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbProduction.BackgroundStyle.Class = ""
        Me.rbProduction.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbProduction.ContainerControlProcessDialogKey = True
        Me.rbProduction.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnProductionManager, Me.btnInventionManager, Me.btnBlueprintCalc, Me.btnRigBuilder})
        Me.rbProduction.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbProduction.Location = New System.Drawing.Point(859, 0)
        Me.rbProduction.Name = "rbProduction"
        Me.rbProduction.Size = New System.Drawing.Size(231, 100)
        Me.rbProduction.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbProduction.TabIndex = 8
        Me.rbProduction.Text = "Production Tools"
        '
        '
        '
        Me.rbProduction.TitleStyle.Class = ""
        Me.rbProduction.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbProduction.TitleStyleMouseOver.Class = ""
        Me.rbProduction.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnProductionManager
        '
        Me.btnProductionManager.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnProductionManager.Image = CType(resources.GetObject("btnProductionManager.Image"), System.Drawing.Image)
        Me.btnProductionManager.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnProductionManager.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnProductionManager.Name = "btnProductionManager"
        Me.btnProductionManager.SubItemsExpandWidth = 14
        Me.btnProductionManager.Text = "Production Manager"
        '
        'btnInventionManager
        '
        Me.btnInventionManager.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnInventionManager.Image = CType(resources.GetObject("btnInventionManager.Image"), System.Drawing.Image)
        Me.btnInventionManager.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnInventionManager.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnInventionManager.Name = "btnInventionManager"
        Me.btnInventionManager.SubItemsExpandWidth = 14
        Me.btnInventionManager.Text = "Invention Manager"
        '
        'btnBlueprintCalc
        '
        Me.btnBlueprintCalc.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnBlueprintCalc.Image = CType(resources.GetObject("btnBlueprintCalc.Image"), System.Drawing.Image)
        Me.btnBlueprintCalc.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnBlueprintCalc.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnBlueprintCalc.Name = "btnBlueprintCalc"
        Me.btnBlueprintCalc.SubItemsExpandWidth = 14
        Me.btnBlueprintCalc.Text = "Blueprint Calculator"
        '
        'btnRigBuilder
        '
        Me.btnRigBuilder.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRigBuilder.Image = CType(resources.GetObject("btnRigBuilder.Image"), System.Drawing.Image)
        Me.btnRigBuilder.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnRigBuilder.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRigBuilder.Name = "btnRigBuilder"
        Me.btnRigBuilder.SubItemsExpandWidth = 14
        Me.btnRigBuilder.Text = "Rig Builder"
        '
        'rbQuickCalcs
        '
        Me.rbQuickCalcs.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbQuickCalcs.BackgroundMouseOverStyle.Class = ""
        Me.rbQuickCalcs.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbQuickCalcs.BackgroundStyle.Class = ""
        Me.rbQuickCalcs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbQuickCalcs.ContainerControlProcessDialogKey = True
        Me.rbQuickCalcs.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnInventionChance, Me.btnQuickProduction})
        Me.rbQuickCalcs.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbQuickCalcs.Location = New System.Drawing.Point(745, 0)
        Me.rbQuickCalcs.Name = "rbQuickCalcs"
        Me.rbQuickCalcs.Size = New System.Drawing.Size(112, 100)
        Me.rbQuickCalcs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbQuickCalcs.TabIndex = 7
        Me.rbQuickCalcs.Text = "Quick Calcs"
        '
        '
        '
        Me.rbQuickCalcs.TitleStyle.Class = ""
        Me.rbQuickCalcs.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbQuickCalcs.TitleStyleMouseOver.Class = ""
        Me.rbQuickCalcs.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnInventionChance
        '
        Me.btnInventionChance.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnInventionChance.Image = CType(resources.GetObject("btnInventionChance.Image"), System.Drawing.Image)
        Me.btnInventionChance.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnInventionChance.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnInventionChance.Name = "btnInventionChance"
        Me.btnInventionChance.SubItemsExpandWidth = 14
        Me.btnInventionChance.Text = "Invention Chance"
        '
        'btnQuickProduction
        '
        Me.btnQuickProduction.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnQuickProduction.Image = CType(resources.GetObject("btnQuickProduction.Image"), System.Drawing.Image)
        Me.btnQuickProduction.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnQuickProduction.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnQuickProduction.Name = "btnQuickProduction"
        Me.btnQuickProduction.SubItemsExpandWidth = 14
        Me.btnQuickProduction.Text = "Quick Quote"
        '
        'rbAnalysisTools
        '
        Me.rbAnalysisTools.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbAnalysisTools.BackgroundMouseOverStyle.Class = ""
        Me.rbAnalysisTools.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAnalysisTools.BackgroundStyle.Class = ""
        Me.rbAnalysisTools.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbAnalysisTools.ContainerControlProcessDialogKey = True
        Me.rbAnalysisTools.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnReports, Me.btnCharts, Me.btnInventionResults})
        Me.rbAnalysisTools.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbAnalysisTools.Location = New System.Drawing.Point(582, 0)
        Me.rbAnalysisTools.Name = "rbAnalysisTools"
        Me.rbAnalysisTools.Size = New System.Drawing.Size(161, 100)
        Me.rbAnalysisTools.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbAnalysisTools.TabIndex = 6
        Me.rbAnalysisTools.Text = "Analysis Tools"
        '
        '
        '
        Me.rbAnalysisTools.TitleStyle.Class = ""
        Me.rbAnalysisTools.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAnalysisTools.TitleStyleMouseOver.Class = ""
        Me.rbAnalysisTools.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnReports
        '
        Me.btnReports.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnReports.Image = CType(resources.GetObject("btnReports.Image"), System.Drawing.Image)
        Me.btnReports.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnReports.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnReports.Name = "btnReports"
        Me.btnReports.SubItemsExpandWidth = 14
        Me.btnReports.Text = "Prism" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Reports"
        '
        'btnCharts
        '
        Me.btnCharts.Image = CType(resources.GetObject("btnCharts.Image"), System.Drawing.Image)
        Me.btnCharts.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnCharts.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnCharts.Name = "btnCharts"
        Me.btnCharts.SubItemsExpandWidth = 14
        Me.btnCharts.Text = "Prism" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Charts"
        '
        'btnInventionResults
        '
        Me.btnInventionResults.Image = CType(resources.GetObject("btnInventionResults.Image"), System.Drawing.Image)
        Me.btnInventionResults.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnInventionResults.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnInventionResults.Name = "btnInventionResults"
        Me.btnInventionResults.SubItemsExpandWidth = 14
        Me.btnInventionResults.Text = "Invention Results"
        '
        'rbMarketTools
        '
        Me.rbMarketTools.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbMarketTools.BackgroundMouseOverStyle.Class = ""
        Me.rbMarketTools.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbMarketTools.BackgroundStyle.Class = ""
        Me.rbMarketTools.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbMarketTools.ContainerControlProcessDialogKey = True
        Me.rbMarketTools.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnOrders, Me.btnJobs, Me.btnContracts})
        Me.rbMarketTools.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbMarketTools.Location = New System.Drawing.Point(414, 0)
        Me.rbMarketTools.Name = "rbMarketTools"
        Me.rbMarketTools.Size = New System.Drawing.Size(166, 100)
        Me.rbMarketTools.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbMarketTools.TabIndex = 4
        Me.rbMarketTools.Text = "Market Tools"
        '
        '
        '
        Me.rbMarketTools.TitleStyle.Class = ""
        Me.rbMarketTools.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbMarketTools.TitleStyleMouseOver.Class = ""
        Me.rbMarketTools.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnOrders
        '
        Me.btnOrders.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnOrders.Image = CType(resources.GetObject("btnOrders.Image"), System.Drawing.Image)
        Me.btnOrders.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnOrders.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnOrders.Name = "btnOrders"
        Me.btnOrders.SubItemsExpandWidth = 14
        Me.btnOrders.Text = "Market" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Orders"
        '
        'btnJobs
        '
        Me.btnJobs.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnJobs.Image = CType(resources.GetObject("btnJobs.Image"), System.Drawing.Image)
        Me.btnJobs.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnJobs.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnJobs.Name = "btnJobs"
        Me.btnJobs.SubItemsExpandWidth = 14
        Me.btnJobs.Text = "Research" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Jobs"
        '
        'btnContracts
        '
        Me.btnContracts.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnContracts.Image = CType(resources.GetObject("btnContracts.Image"), System.Drawing.Image)
        Me.btnContracts.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnContracts.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnContracts.Name = "btnContracts"
        Me.btnContracts.SubItemsExpandWidth = 14
        Me.btnContracts.Text = "Contracts"
        '
        'rbAssetManagement
        '
        Me.rbAssetManagement.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbAssetManagement.BackgroundMouseOverStyle.Class = ""
        Me.rbAssetManagement.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAssetManagement.BackgroundStyle.Class = ""
        Me.rbAssetManagement.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbAssetManagement.ContainerControlProcessDialogKey = True
        Me.rbAssetManagement.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnAssets, Me.btnBPManager, Me.btnRecycler})
        Me.rbAssetManagement.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbAssetManagement.Location = New System.Drawing.Point(247, 0)
        Me.rbAssetManagement.Name = "rbAssetManagement"
        Me.rbAssetManagement.Size = New System.Drawing.Size(165, 100)
        Me.rbAssetManagement.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbAssetManagement.TabIndex = 3
        Me.rbAssetManagement.Text = "Asset Management"
        '
        '
        '
        Me.rbAssetManagement.TitleStyle.Class = ""
        Me.rbAssetManagement.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAssetManagement.TitleStyleMouseOver.Class = ""
        Me.rbAssetManagement.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnAssets
        '
        Me.btnAssets.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnAssets.Image = CType(resources.GetObject("btnAssets.Image"), System.Drawing.Image)
        Me.btnAssets.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnAssets.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnAssets.Name = "btnAssets"
        Me.btnAssets.SubItemsExpandWidth = 14
        Me.btnAssets.Text = "Asset" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Viewer"
        '
        'btnBPManager
        '
        Me.btnBPManager.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnBPManager.Image = CType(resources.GetObject("btnBPManager.Image"), System.Drawing.Image)
        Me.btnBPManager.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnBPManager.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnBPManager.Name = "btnBPManager"
        Me.btnBPManager.SubItemsExpandWidth = 14
        Me.btnBPManager.Text = "Blueprint" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Manager"
        '
        'btnRecycler
        '
        Me.btnRecycler.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnRecycler.Image = CType(resources.GetObject("btnRecycler.Image"), System.Drawing.Image)
        Me.btnRecycler.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnRecycler.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnRecycler.Name = "btnRecycler"
        Me.btnRecycler.SubItemsExpandWidth = 14
        Me.btnRecycler.Text = "Recycling" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Analysis"
        '
        'rbWallet
        '
        Me.rbWallet.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbWallet.BackgroundMouseOverStyle.Class = ""
        Me.rbWallet.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbWallet.BackgroundStyle.Class = ""
        Me.rbWallet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbWallet.ContainerControlProcessDialogKey = True
        Me.rbWallet.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnWalletJournal, Me.btnWalletTransactions})
        Me.rbWallet.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbWallet.Location = New System.Drawing.Point(117, 0)
        Me.rbWallet.Name = "rbWallet"
        Me.rbWallet.Size = New System.Drawing.Size(128, 100)
        Me.rbWallet.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbWallet.TabIndex = 1
        Me.rbWallet.Text = "Wallet Features"
        '
        '
        '
        Me.rbWallet.TitleStyle.Class = ""
        Me.rbWallet.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbWallet.TitleStyleMouseOver.Class = ""
        Me.rbWallet.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnWalletJournal
        '
        Me.btnWalletJournal.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnWalletJournal.Image = CType(resources.GetObject("btnWalletJournal.Image"), System.Drawing.Image)
        Me.btnWalletJournal.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnWalletJournal.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnWalletJournal.Name = "btnWalletJournal"
        Me.btnWalletJournal.SubItemsExpandWidth = 14
        Me.btnWalletJournal.Text = "Wallet" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Journal"
        '
        'btnWalletTransactions
        '
        Me.btnWalletTransactions.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnWalletTransactions.Image = CType(resources.GetObject("btnWalletTransactions.Image"), System.Drawing.Image)
        Me.btnWalletTransactions.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnWalletTransactions.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnWalletTransactions.Name = "btnWalletTransactions"
        Me.btnWalletTransactions.SubItemsExpandWidth = 14
        Me.btnWalletTransactions.Text = "Wallet" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Transactions"
        '
        'rbData
        '
        Me.rbData.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbData.BackgroundMouseOverStyle.Class = ""
        Me.rbData.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbData.BackgroundStyle.Class = ""
        Me.rbData.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbData.ContainerControlProcessDialogKey = True
        Me.rbData.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnOptions, Me.btnDownloadAPIData})
        Me.rbData.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbData.Location = New System.Drawing.Point(0, 0)
        Me.rbData.Name = "rbData"
        Me.rbData.Size = New System.Drawing.Size(115, 100)
        Me.rbData.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbData.TabIndex = 0
        Me.rbData.Text = "Settings && Data"
        '
        '
        '
        Me.rbData.TitleStyle.Class = ""
        Me.rbData.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbData.TitleStyleMouseOver.Class = ""
        Me.rbData.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnOptions
        '
        Me.btnOptions.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnOptions.Image = CType(resources.GetObject("btnOptions.Image"), System.Drawing.Image)
        Me.btnOptions.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnOptions.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnOptions.Name = "btnOptions"
        Me.btnOptions.SubItemsExpandWidth = 14
        Me.btnOptions.Text = "Prism Options"
        '
        'btnDownloadAPIData
        '
        Me.btnDownloadAPIData.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnDownloadAPIData.Image = CType(resources.GetObject("btnDownloadAPIData.Image"), System.Drawing.Image)
        Me.btnDownloadAPIData.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnDownloadAPIData.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnDownloadAPIData.Name = "btnDownloadAPIData"
        Me.btnDownloadAPIData.Text = "Download API Data"
        '
        'pnlPrism
        '
        Me.pnlPrism.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlPrism.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlPrism.Controls.Add(Me.tabPrism)
        Me.pnlPrism.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlPrism.Location = New System.Drawing.Point(0, 0)
        Me.pnlPrism.Name = "pnlPrism"
        Me.pnlPrism.Size = New System.Drawing.Size(1284, 803)
        Me.pnlPrism.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlPrism.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlPrism.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlPrism.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlPrism.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlPrism.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlPrism.Style.GradientAngle = 90
        Me.pnlPrism.TabIndex = 18
        '
        'tabPrism
        '
        Me.tabPrism.AutoCloseTabs = True
        Me.tabPrism.BackColor = System.Drawing.Color.Transparent
        Me.tabPrism.CanReorderTabs = True
        Me.tabPrism.CloseButtonOnTabsVisible = True
        Me.tabPrism.CloseButtonPosition = DevComponents.DotNetBar.eTabCloseButtonPosition.Right
        Me.tabPrism.CloseButtonVisible = True
        Me.tabPrism.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tabPrism.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tabPrism.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tabPrism.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tabPrism.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tabPrism.Controls.Add(Me.TabControlPanel1)
        Me.tabPrism.Controls.Add(Me.TabControlPanel6)
        Me.tabPrism.Controls.Add(Me.TabControlPanel17)
        Me.tabPrism.Controls.Add(Me.TabControlPanel3)
        Me.tabPrism.Controls.Add(Me.TabControlPanel11)
        Me.tabPrism.Controls.Add(Me.TabControlPanel16)
        Me.tabPrism.Controls.Add(Me.TabControlPanel7)
        Me.tabPrism.Controls.Add(Me.TabControlPanel5)
        Me.tabPrism.Controls.Add(Me.TabControlPanel10)
        Me.tabPrism.Controls.Add(Me.TabControlPanel15)
        Me.tabPrism.Controls.Add(Me.TabControlPanel4)
        Me.tabPrism.Controls.Add(Me.TabControlPanel14)
        Me.tabPrism.Controls.Add(Me.TabControlPanel8)
        Me.tabPrism.Controls.Add(Me.TabControlPanel9)
        Me.tabPrism.Controls.Add(Me.TabControlPanel2)
        Me.tabPrism.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tabPrism.Location = New System.Drawing.Point(0, 130)
        Me.tabPrism.Name = "tabPrism"
        Me.tabPrism.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tabPrism.SelectedTabIndex = 0
        Me.tabPrism.Size = New System.Drawing.Size(1284, 673)
        Me.tabPrism.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tabPrism.TabIndex = 15
        Me.tabPrism.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabPrism.Tabs.Add(Me.tiPrismHome)
        Me.tabPrism.Tabs.Add(Me.tiAssets)
        Me.tabPrism.Tabs.Add(Me.tiMarketOrders)
        Me.tabPrism.Tabs.Add(Me.tiJournal)
        Me.tabPrism.Tabs.Add(Me.tiTransactions)
        Me.tabPrism.Tabs.Add(Me.tiJobs)
        Me.tabPrism.Tabs.Add(Me.tiRecycler)
        Me.tabPrism.Tabs.Add(Me.tiBPManager)
        Me.tabPrism.Tabs.Add(Me.tiProductionManager)
        Me.tabPrism.Tabs.Add(Me.tiInventionManager)
        Me.tabPrism.Tabs.Add(Me.tiReports)
        Me.tabPrism.Tabs.Add(Me.tiCharts)
        Me.tabPrism.Tabs.Add(Me.tiContracts)
        Me.tabPrism.Tabs.Add(Me.tiRigBuilder)
        Me.tabPrism.Tabs.Add(Me.tiInventionResults)
        Me.tabPrism.Text = "TabControl2"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.btnRefreshAPI)
        Me.TabControlPanel1.Controls.Add(Me.btnLinkRequisition)
        Me.TabControlPanel1.Controls.Add(Me.btnLinkProduction)
        Me.TabControlPanel1.Controls.Add(Me.btnLinkBPCalc)
        Me.TabControlPanel1.Controls.Add(Me.lblSelectedBP)
        Me.TabControlPanel1.Controls.Add(Me.lblSelectedItem)
        Me.TabControlPanel1.Controls.Add(Me.adtSearch)
        Me.TabControlPanel1.Controls.Add(Me.txtItemSearch)
        Me.TabControlPanel1.Controls.Add(Me.lblSearch)
        Me.TabControlPanel1.Controls.Add(Me.lblCurrentAPI)
        Me.TabControlPanel1.Controls.Add(Me.lvwCurrentAPIs)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 1
        Me.TabControlPanel1.TabItem = Me.tiPrismHome
        '
        'btnRefreshAPI
        '
        Me.btnRefreshAPI.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnRefreshAPI.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshAPI.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnRefreshAPI.Image = CType(resources.GetObject("btnRefreshAPI.Image"), System.Drawing.Image)
        Me.btnRefreshAPI.ImageFixedSize = New System.Drawing.Size(24, 24)
        Me.btnRefreshAPI.Location = New System.Drawing.Point(405, 614)
        Me.btnRefreshAPI.Name = "btnRefreshAPI"
        Me.btnRefreshAPI.Size = New System.Drawing.Size(145, 28)
        Me.btnRefreshAPI.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnRefreshAPI.TabIndex = 16
        Me.btnRefreshAPI.Text = "Refresh API Status"
        Me.btnRefreshAPI.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left
        '
        'btnLinkRequisition
        '
        Me.btnLinkRequisition.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnLinkRequisition.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnLinkRequisition.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnLinkRequisition.Enabled = False
        Me.btnLinkRequisition.Image = CType(resources.GetObject("btnLinkRequisition.Image"), System.Drawing.Image)
        Me.btnLinkRequisition.ImageFixedSize = New System.Drawing.Size(24, 24)
        Me.btnLinkRequisition.Location = New System.Drawing.Point(231, 614)
        Me.btnLinkRequisition.Name = "btnLinkRequisition"
        Me.btnLinkRequisition.Size = New System.Drawing.Size(106, 28)
        Me.btnLinkRequisition.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnLinkRequisition.TabIndex = 15
        Me.btnLinkRequisition.Text = "Requisition"
        Me.btnLinkRequisition.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left
        '
        'btnLinkProduction
        '
        Me.btnLinkProduction.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnLinkProduction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnLinkProduction.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnLinkProduction.Enabled = False
        Me.btnLinkProduction.Image = CType(resources.GetObject("btnLinkProduction.Image"), System.Drawing.Image)
        Me.btnLinkProduction.ImageFixedSize = New System.Drawing.Size(24, 24)
        Me.btnLinkProduction.Location = New System.Drawing.Point(119, 614)
        Me.btnLinkProduction.Name = "btnLinkProduction"
        Me.btnLinkProduction.Size = New System.Drawing.Size(106, 28)
        Me.btnLinkProduction.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnLinkProduction.TabIndex = 14
        Me.btnLinkProduction.Text = "Quick Quote"
        Me.btnLinkProduction.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left
        '
        'btnLinkBPCalc
        '
        Me.btnLinkBPCalc.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnLinkBPCalc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnLinkBPCalc.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnLinkBPCalc.Enabled = False
        Me.btnLinkBPCalc.Image = CType(resources.GetObject("btnLinkBPCalc.Image"), System.Drawing.Image)
        Me.btnLinkBPCalc.ImageFixedSize = New System.Drawing.Size(24, 24)
        Me.btnLinkBPCalc.Location = New System.Drawing.Point(7, 614)
        Me.btnLinkBPCalc.Name = "btnLinkBPCalc"
        Me.btnLinkBPCalc.Size = New System.Drawing.Size(106, 28)
        Me.btnLinkBPCalc.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnLinkBPCalc.TabIndex = 12
        Me.btnLinkBPCalc.Text = "BP Calc"
        Me.btnLinkBPCalc.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left
        '
        'lblSelectedBP
        '
        Me.lblSelectedBP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSelectedBP.AutoSize = True
        Me.lblSelectedBP.BackColor = System.Drawing.Color.Transparent
        Me.lblSelectedBP.Location = New System.Drawing.Point(7, 592)
        Me.lblSelectedBP.Name = "lblSelectedBP"
        Me.lblSelectedBP.Size = New System.Drawing.Size(96, 13)
        Me.lblSelectedBP.TabIndex = 11
        Me.lblSelectedBP.Text = "Blueprint: <none>"
        '
        'lblSelectedItem
        '
        Me.lblSelectedItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSelectedItem.AutoSize = True
        Me.lblSelectedItem.BackColor = System.Drawing.Color.Transparent
        Me.lblSelectedItem.Location = New System.Drawing.Point(7, 575)
        Me.lblSelectedItem.Name = "lblSelectedItem"
        Me.lblSelectedItem.Size = New System.Drawing.Size(76, 13)
        Me.lblSelectedItem.TabIndex = 10
        Me.lblSelectedItem.Text = "Item: <none>"
        '
        'adtSearch
        '
        Me.adtSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtSearch.AllowDrop = True
        Me.adtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.adtSearch.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtSearch.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtSearch.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtSearch.Columns.Add(Me.colItemSearch)
        Me.adtSearch.ExpandWidth = 0
        Me.adtSearch.GridLinesColor = System.Drawing.Color.Gainsboro
        Me.adtSearch.GridRowLines = True
        Me.adtSearch.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtSearch.Location = New System.Drawing.Point(7, 48)
        Me.adtSearch.Name = "adtSearch"
        Me.adtSearch.NodesConnector = Me.NodeConnector2
        Me.adtSearch.NodeStyle = Me.ElementStyle2
        Me.adtSearch.PathSeparator = ";"
        Me.adtSearch.Size = New System.Drawing.Size(387, 524)
        Me.adtSearch.Styles.Add(Me.ElementStyle2)
        Me.adtSearch.TabIndex = 8
        Me.adtSearch.Text = "AdvTree1"
        '
        'colItemSearch
        '
        Me.colItemSearch.Name = "colItemSearch"
        Me.colItemSearch.Text = "Item Name"
        Me.colItemSearch.Width.Absolute = 360
        '
        'NodeConnector2
        '
        Me.NodeConnector2.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle2
        '
        Me.ElementStyle2.Class = ""
        Me.ElementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle2.Name = "ElementStyle2"
        Me.ElementStyle2.TextColor = System.Drawing.SystemColors.ControlText
        '
        'txtItemSearch
        '
        '
        '
        '
        Me.txtItemSearch.Border.Class = "TextBoxBorder"
        Me.txtItemSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.txtItemSearch.Location = New System.Drawing.Point(7, 25)
        Me.txtItemSearch.Name = "txtItemSearch"
        Me.txtItemSearch.Size = New System.Drawing.Size(387, 21)
        Me.txtItemSearch.TabIndex = 7
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Location = New System.Drawing.Point(4, 9)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblSearch.TabIndex = 6
        Me.lblSearch.Text = "Search:"
        '
        'tiPrismHome
        '
        Me.tiPrismHome.AttachedControl = Me.TabControlPanel1
        Me.tiPrismHome.Name = "tiPrismHome"
        Me.tiPrismHome.Text = "Prism Home"
        '
        'TabControlPanel6
        '
        Me.TabControlPanel6.Controls.Add(Me.btnImportEntries)
        Me.TabControlPanel6.Controls.Add(Me.btnExportEntries)
        Me.TabControlPanel6.Controls.Add(Me.btnCheckJournalOmissions)
        Me.TabControlPanel6.Controls.Add(Me.adtJournal)
        Me.TabControlPanel6.Controls.Add(Me.btnResetJournal)
        Me.TabControlPanel6.Controls.Add(Me.lblWalletJournalDivision)
        Me.TabControlPanel6.Controls.Add(Me.lblAlwaysShowEveBalance)
        Me.TabControlPanel6.Controls.Add(Me.sbShowEveBalance)
        Me.TabControlPanel6.Controls.Add(Me.btnExportJournal)
        Me.TabControlPanel6.Controls.Add(Me.btnJournalQuery)
        Me.TabControlPanel6.Controls.Add(Me.cboWalletJournalDivision)
        Me.TabControlPanel6.Controls.Add(Me.cboJournalRefTypes)
        Me.TabControlPanel6.Controls.Add(Me.dtiJournalStartDate)
        Me.TabControlPanel6.Controls.Add(Me.cboJournalOwners)
        Me.TabControlPanel6.Controls.Add(Me.dtiJournalEndDate)
        Me.TabControlPanel6.Controls.Add(Me.lblJournalEndDate)
        Me.TabControlPanel6.Controls.Add(Me.lblJournalStartDate)
        Me.TabControlPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel6.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel6.Name = "TabControlPanel6"
        Me.TabControlPanel6.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel6.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel6.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel6.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel6.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel6.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel6.Style.GradientAngle = 90
        Me.TabControlPanel6.TabIndex = 6
        Me.TabControlPanel6.TabItem = Me.tiJournal
        '
        'btnImportEntries
        '
        Me.btnImportEntries.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnImportEntries.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnImportEntries.Location = New System.Drawing.Point(757, 38)
        Me.btnImportEntries.Name = "btnImportEntries"
        Me.btnImportEntries.Size = New System.Drawing.Size(100, 23)
        Me.btnImportEntries.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnImportEntries.TabIndex = 21
        Me.btnImportEntries.Text = "Import Entries"
        '
        'btnExportEntries
        '
        Me.btnExportEntries.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnExportEntries.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnExportEntries.Location = New System.Drawing.Point(757, 8)
        Me.btnExportEntries.Name = "btnExportEntries"
        Me.btnExportEntries.Size = New System.Drawing.Size(100, 23)
        Me.btnExportEntries.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnExportEntries.TabIndex = 20
        Me.btnExportEntries.Text = "Export Entries"
        '
        'btnCheckJournalOmissions
        '
        Me.btnCheckJournalOmissions.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCheckJournalOmissions.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCheckJournalOmissions.Location = New System.Drawing.Point(651, 8)
        Me.btnCheckJournalOmissions.Name = "btnCheckJournalOmissions"
        Me.btnCheckJournalOmissions.Size = New System.Drawing.Size(100, 23)
        Me.btnCheckJournalOmissions.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCheckJournalOmissions.TabIndex = 19
        Me.btnCheckJournalOmissions.Text = "Check Omissions"
        '
        'adtJournal
        '
        Me.adtJournal.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtJournal.AllowDrop = True
        Me.adtJournal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtJournal.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtJournal.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtJournal.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtJournal.Columns.Add(Me.colJournalDate)
        Me.adtJournal.Columns.Add(Me.colJournalType)
        Me.adtJournal.Columns.Add(Me.colJournalAmount)
        Me.adtJournal.Columns.Add(Me.colJournalBalance)
        Me.adtJournal.Columns.Add(Me.colJournalDescription)
        Me.adtJournal.DragDropEnabled = False
        Me.adtJournal.DragDropNodeCopyEnabled = False
        Me.adtJournal.ExpandWidth = 16
        Me.adtJournal.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtJournal.Location = New System.Drawing.Point(4, 88)
        Me.adtJournal.Name = "adtJournal"
        Me.adtJournal.NodeStyle = Me.ElementStyle1
        Me.adtJournal.PathSeparator = ";"
        Me.adtJournal.Size = New System.Drawing.Size(1276, 558)
        Me.adtJournal.Styles.Add(Me.ElementStyle1)
        Me.adtJournal.TabIndex = 18
        Me.adtJournal.Text = "AdvTree1"
        '
        'colJournalDate
        '
        Me.colJournalDate.DisplayIndex = 1
        Me.colJournalDate.Name = "colJournalDate"
        Me.colJournalDate.Text = "Date"
        Me.colJournalDate.Width.Absolute = 150
        '
        'colJournalType
        '
        Me.colJournalType.DisplayIndex = 2
        Me.colJournalType.Name = "colJournalType"
        Me.colJournalType.Text = "Type"
        Me.colJournalType.Width.Absolute = 250
        '
        'colJournalAmount
        '
        Me.colJournalAmount.DisplayIndex = 3
        Me.colJournalAmount.Name = "colJournalAmount"
        Me.colJournalAmount.Text = "Amount"
        Me.colJournalAmount.Width.Absolute = 125
        '
        'colJournalBalance
        '
        Me.colJournalBalance.DisplayIndex = 4
        Me.colJournalBalance.Name = "colJournalBalance"
        Me.colJournalBalance.Text = "Balance"
        Me.colJournalBalance.Width.Absolute = 125
        '
        'colJournalDescription
        '
        Me.colJournalDescription.DisplayIndex = 5
        Me.colJournalDescription.Name = "colJournalDescription"
        Me.colJournalDescription.Text = "Description"
        Me.colJournalDescription.Width.Absolute = 500
        '
        'ElementStyle1
        '
        Me.ElementStyle1.Class = ""
        Me.ElementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle1.Name = "ElementStyle1"
        Me.ElementStyle1.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiJournal
        '
        Me.tiJournal.AttachedControl = Me.TabControlPanel6
        Me.tiJournal.Name = "tiJournal"
        Me.tiJournal.Text = "Journal"
        '
        'TabControlPanel17
        '
        Me.TabControlPanel17.Controls.Add(Me.adtInventionJobs)
        Me.TabControlPanel17.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel17.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel17.Name = "TabControlPanel17"
        Me.TabControlPanel17.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel17.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel17.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel17.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel17.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel17.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel17.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel17.Style.GradientAngle = 90
        Me.TabControlPanel17.TabIndex = 17
        Me.TabControlPanel17.TabItem = Me.tiInventionManager
        '
        'adtInventionJobs
        '
        Me.adtInventionJobs.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtInventionJobs.AllowDrop = True
        Me.adtInventionJobs.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtInventionJobs.BackgroundStyle.Class = ""
        Me.adtInventionJobs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtInventionJobs.Columns.Add(Me.colInvJobName)
        Me.adtInventionJobs.Columns.Add(Me.colInvJobItem)
        Me.adtInventionJobs.Columns.Add(Me.colSuccessChance)
        Me.adtInventionJobs.Columns.Add(Me.colSuccessCost)
        Me.adtInventionJobs.Columns.Add(Me.colInvProductionCost)
        Me.adtInventionJobs.Columns.Add(Me.colInvSalesPrice)
        Me.adtInventionJobs.Columns.Add(Me.colInvUnitProfit)
        Me.adtInventionJobs.Columns.Add(Me.colInvProfitMargin)
        Me.adtInventionJobs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtInventionJobs.DragDropEnabled = False
        Me.adtInventionJobs.DragDropNodeCopyEnabled = False
        Me.adtInventionJobs.ExpandWidth = 0
        Me.adtInventionJobs.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtInventionJobs.Location = New System.Drawing.Point(1, 1)
        Me.adtInventionJobs.MultiSelect = True
        Me.adtInventionJobs.Name = "adtInventionJobs"
        Me.adtInventionJobs.NodesConnector = Me.NodeConnector17
        Me.adtInventionJobs.NodeStyle = Me.ElementStyle14
        Me.adtInventionJobs.PathSeparator = ";"
        Me.adtInventionJobs.Size = New System.Drawing.Size(1282, 648)
        Me.adtInventionJobs.Styles.Add(Me.ElementStyle14)
        Me.adtInventionJobs.TabIndex = 1
        Me.adtInventionJobs.Text = "AdvTree1"
        '
        'colInvJobName
        '
        Me.colInvJobName.DisplayIndex = 1
        Me.colInvJobName.Name = "colInvJobName"
        Me.colInvJobName.Text = "Invention Job Name"
        Me.colInvJobName.Width.Absolute = 150
        '
        'colInvJobItem
        '
        Me.colInvJobItem.DisplayIndex = 2
        Me.colInvJobItem.Name = "colInvJobItem"
        Me.colInvJobItem.Text = "Invented BP"
        Me.colInvJobItem.Width.Absolute = 150
        '
        'colSuccessChance
        '
        Me.colSuccessChance.DisplayIndex = 3
        Me.colSuccessChance.Name = "colSuccessChance"
        Me.colSuccessChance.Text = "% Success"
        Me.colSuccessChance.Width.Absolute = 75
        '
        'colSuccessCost
        '
        Me.colSuccessCost.DisplayIndex = 4
        Me.colSuccessCost.Name = "colSuccessCost"
        Me.colSuccessCost.Text = "Success Cost"
        Me.colSuccessCost.Width.Absolute = 150
        '
        'colInvProductionCost
        '
        Me.colInvProductionCost.DisplayIndex = 5
        Me.colInvProductionCost.Name = "colInvProductionCost"
        Me.colInvProductionCost.Text = "Production Cost"
        Me.colInvProductionCost.Width.Absolute = 150
        '
        'colInvSalesPrice
        '
        Me.colInvSalesPrice.DisplayIndex = 6
        Me.colInvSalesPrice.Name = "colInvSalesPrice"
        Me.colInvSalesPrice.Text = "Sales Price"
        Me.colInvSalesPrice.Width.Absolute = 120
        '
        'colInvUnitProfit
        '
        Me.colInvUnitProfit.DisplayIndex = 7
        Me.colInvUnitProfit.Name = "colInvUnitProfit"
        Me.colInvUnitProfit.Text = "Unit Profit"
        Me.colInvUnitProfit.Width.Absolute = 120
        '
        'colInvProfitMargin
        '
        Me.colInvProfitMargin.DisplayIndex = 8
        Me.colInvProfitMargin.Name = "colInvProfitMargin"
        Me.colInvProfitMargin.Text = "Margin (%)"
        Me.colInvProfitMargin.Width.Absolute = 75
        '
        'NodeConnector17
        '
        Me.NodeConnector17.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle14
        '
        Me.ElementStyle14.Class = ""
        Me.ElementStyle14.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle14.Name = "ElementStyle14"
        Me.ElementStyle14.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiInventionManager
        '
        Me.tiInventionManager.AttachedControl = Me.TabControlPanel17
        Me.tiInventionManager.Name = "tiInventionManager"
        Me.tiInventionManager.Text = "Invention Manager"
        '
        'TabControlPanel3
        '
        Me.TabControlPanel3.Controls.Add(Me.wbReport)
        Me.TabControlPanel3.Controls.Add(Me.pnlReportControls)
        Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel3.Name = "TabControlPanel3"
        Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel3.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel3.Style.GradientAngle = 90
        Me.TabControlPanel3.TabIndex = 12
        Me.TabControlPanel3.TabItem = Me.tiReports
        '
        'wbReport
        '
        Me.wbReport.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wbReport.Location = New System.Drawing.Point(309, 1)
        Me.wbReport.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbReport.Name = "wbReport"
        Me.wbReport.Size = New System.Drawing.Size(974, 648)
        Me.wbReport.TabIndex = 35
        '
        'pnlReportControls
        '
        Me.pnlReportControls.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlReportControls.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlReportControls.Controls.Add(Me.cboReport)
        Me.pnlReportControls.Controls.Add(Me.lblReportStartDate)
        Me.pnlReportControls.Controls.Add(Me.btnGenerateReport)
        Me.pnlReportControls.Controls.Add(Me.lblReportEndDate)
        Me.pnlReportControls.Controls.Add(Me.dtiReportEndDate)
        Me.pnlReportControls.Controls.Add(Me.dtiReportStartDate)
        Me.pnlReportControls.Controls.Add(Me.cboReportOwners)
        Me.pnlReportControls.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlReportControls.Location = New System.Drawing.Point(1, 1)
        Me.pnlReportControls.Name = "pnlReportControls"
        Me.pnlReportControls.Size = New System.Drawing.Size(308, 648)
        Me.pnlReportControls.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlReportControls.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlReportControls.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlReportControls.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlReportControls.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlReportControls.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlReportControls.Style.GradientAngle = 90
        Me.pnlReportControls.TabIndex = 34
        '
        'cboReport
        '
        Me.cboReport.DisplayMember = "Text"
        Me.cboReport.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReport.FormattingEnabled = True
        Me.cboReport.ItemHeight = 15
        Me.cboReport.Location = New System.Drawing.Point(12, 14)
        Me.cboReport.Name = "cboReport"
        Me.cboReport.Size = New System.Drawing.Size(236, 21)
        Me.cboReport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboReport.TabIndex = 33
        Me.cboReport.WatermarkColor = System.Drawing.Color.Silver
        Me.cboReport.WatermarkText = "Select report..."
        '
        'lblReportStartDate
        '
        Me.lblReportStartDate.AutoSize = True
        Me.lblReportStartDate.BackColor = System.Drawing.Color.Transparent
        Me.lblReportStartDate.Location = New System.Drawing.Point(13, 58)
        Me.lblReportStartDate.Name = "lblReportStartDate"
        Me.lblReportStartDate.Size = New System.Drawing.Size(61, 13)
        Me.lblReportStartDate.TabIndex = 24
        Me.lblReportStartDate.Text = "Start Date:"
        '
        'btnGenerateReport
        '
        Me.btnGenerateReport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnGenerateReport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnGenerateReport.Location = New System.Drawing.Point(12, 191)
        Me.btnGenerateReport.Name = "btnGenerateReport"
        Me.btnGenerateReport.Size = New System.Drawing.Size(100, 23)
        Me.btnGenerateReport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnGenerateReport.TabIndex = 28
        Me.btnGenerateReport.Text = "Generate Report"
        '
        'lblReportEndDate
        '
        Me.lblReportEndDate.AutoSize = True
        Me.lblReportEndDate.BackColor = System.Drawing.Color.Transparent
        Me.lblReportEndDate.Location = New System.Drawing.Point(13, 85)
        Me.lblReportEndDate.Name = "lblReportEndDate"
        Me.lblReportEndDate.Size = New System.Drawing.Size(55, 13)
        Me.lblReportEndDate.TabIndex = 25
        Me.lblReportEndDate.Text = "End Date:"
        '
        'dtiReportEndDate
        '
        '
        '
        '
        Me.dtiReportEndDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiReportEndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiReportEndDate.ButtonCustom.Text = "Now"
        Me.dtiReportEndDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiReportEndDate.ButtonCustom2.Text = "SoD"
        Me.dtiReportEndDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiReportEndDate.ButtonDropDown.Visible = True
        Me.dtiReportEndDate.CustomFormat = "yyyy-MM-dd"
        Me.dtiReportEndDate.Format = DevComponents.Editors.eDateTimePickerFormat.[Long]
        Me.dtiReportEndDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Right
        Me.dtiReportEndDate.Location = New System.Drawing.Point(99, 81)
        '
        '
        '
        Me.dtiReportEndDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiReportEndDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiReportEndDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiReportEndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiReportEndDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiReportEndDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiReportEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiReportEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiReportEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiReportEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiReportEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiReportEndDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiReportEndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiReportEndDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiReportEndDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiReportEndDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiReportEndDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiReportEndDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiReportEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiReportEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiReportEndDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiReportEndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiReportEndDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiReportEndDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiReportEndDate.Name = "dtiReportEndDate"
        Me.dtiReportEndDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiReportEndDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiReportEndDate.TabIndex = 23
        Me.dtiReportEndDate.Value = New Date(2011, 6, 9, 0, 0, 0, 0)
        '
        'dtiReportStartDate
        '
        '
        '
        '
        Me.dtiReportStartDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiReportStartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiReportStartDate.ButtonCustom.Text = "Now"
        Me.dtiReportStartDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiReportStartDate.ButtonCustom2.Text = "SoD"
        Me.dtiReportStartDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiReportStartDate.ButtonDropDown.Visible = True
        Me.dtiReportStartDate.CustomFormat = "yyyy-MM-dd"
        Me.dtiReportStartDate.Format = DevComponents.Editors.eDateTimePickerFormat.[Long]
        Me.dtiReportStartDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Right
        Me.dtiReportStartDate.Location = New System.Drawing.Point(99, 54)
        '
        '
        '
        Me.dtiReportStartDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiReportStartDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiReportStartDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiReportStartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiReportStartDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiReportStartDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiReportStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiReportStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiReportStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiReportStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiReportStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiReportStartDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiReportStartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiReportStartDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiReportStartDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiReportStartDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiReportStartDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiReportStartDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiReportStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiReportStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiReportStartDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiReportStartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiReportStartDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiReportStartDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiReportStartDate.Name = "dtiReportStartDate"
        Me.dtiReportStartDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiReportStartDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiReportStartDate.TabIndex = 22
        Me.dtiReportStartDate.Value = New Date(2011, 6, 9, 0, 0, 0, 0)
        '
        'cboReportOwners
        '
        Me.cboReportOwners.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboReportOwners.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboReportOwners.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboReportOwners.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboReportOwners.ButtonDropDown.Visible = True
        Me.cboReportOwners.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboReportOwners.Location = New System.Drawing.Point(12, 108)
        Me.cboReportOwners.Name = "cboReportOwners"
        Me.cboReportOwners.Size = New System.Drawing.Size(287, 21)
        Me.cboReportOwners.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboReportOwners.TabIndex = 26
        Me.cboReportOwners.Text = ""
        Me.cboReportOwners.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.cboReportOwners.WatermarkColor = System.Drawing.Color.Silver
        Me.cboReportOwners.WatermarkText = "Select owners..."
        '
        'tiReports
        '
        Me.tiReports.AttachedControl = Me.TabControlPanel3
        Me.tiReports.Name = "tiReports"
        Me.tiReports.Text = "Prism Reports"
        '
        'TabControlPanel11
        '
        Me.TabControlPanel11.Controls.Add(Me.tcPM)
        Me.TabControlPanel11.Controls.Add(Me.PRPM)
        Me.TabControlPanel11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel11.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel11.Name = "TabControlPanel11"
        Me.TabControlPanel11.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel11.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel11.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel11.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel11.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel11.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel11.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel11.Style.GradientAngle = 90
        Me.TabControlPanel11.TabIndex = 11
        Me.TabControlPanel11.TabItem = Me.tiProductionManager
        '
        'tcPM
        '
        Me.tcPM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tcPM.BackColor = System.Drawing.Color.Transparent
        Me.tcPM.CanReorderTabs = True
        Me.tcPM.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tcPM.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tcPM.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tcPM.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tcPM.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tcPM.Controls.Add(Me.TabControlPanel12)
        Me.tcPM.Controls.Add(Me.TabControlPanel13)
        Me.tcPM.Location = New System.Drawing.Point(7, 5)
        Me.tcPM.Name = "tcPM"
        Me.tcPM.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tcPM.SelectedTabIndex = 0
        Me.tcPM.Size = New System.Drawing.Size(664, 641)
        Me.tcPM.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tcPM.TabIndex = 7
        Me.tcPM.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tcPM.Tabs.Add(Me.tiProductionJobs)
        Me.tcPM.Tabs.Add(Me.tiBatchJobs)
        Me.tcPM.Text = "TabControl2"
        '
        'TabControlPanel12
        '
        Me.TabControlPanel12.Controls.Add(Me.adtProdJobs)
        Me.TabControlPanel12.Controls.Add(Me.pnlJobs)
        Me.TabControlPanel12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel12.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel12.Name = "TabControlPanel12"
        Me.TabControlPanel12.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel12.Size = New System.Drawing.Size(664, 618)
        Me.TabControlPanel12.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel12.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel12.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel12.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel12.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel12.Style.GradientAngle = 90
        Me.TabControlPanel12.TabIndex = 1
        Me.TabControlPanel12.TabItem = Me.tiProductionJobs
        '
        'adtProdJobs
        '
        Me.adtProdJobs.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtProdJobs.AllowDrop = True
        Me.adtProdJobs.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtProdJobs.BackgroundStyle.Class = ""
        Me.adtProdJobs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtProdJobs.Columns.Add(Me.colJobName)
        Me.adtProdJobs.Columns.Add(Me.colJobItem)
        Me.adtProdJobs.Columns.Add(Me.colJobUnitProfit)
        Me.adtProdJobs.Columns.Add(Me.colJobProfitRate)
        Me.adtProdJobs.Columns.Add(Me.colJobMargin)
        Me.adtProdJobs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtProdJobs.DragDropEnabled = False
        Me.adtProdJobs.DragDropNodeCopyEnabled = False
        Me.adtProdJobs.ExpandWidth = 0
        Me.adtProdJobs.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtProdJobs.Location = New System.Drawing.Point(1, 1)
        Me.adtProdJobs.MultiSelect = True
        Me.adtProdJobs.Name = "adtProdJobs"
        Me.adtProdJobs.NodesConnector = Me.NodeConnector3
        Me.adtProdJobs.NodeStyle = Me.ElementStyle3
        Me.adtProdJobs.PathSeparator = ";"
        Me.adtProdJobs.Size = New System.Drawing.Size(662, 589)
        Me.adtProdJobs.Styles.Add(Me.ElementStyle3)
        Me.adtProdJobs.TabIndex = 0
        Me.adtProdJobs.Text = "AdvTree1"
        '
        'colJobName
        '
        Me.colJobName.DisplayIndex = 1
        Me.colJobName.Name = "colJobName"
        Me.colJobName.Text = "Job Name"
        Me.colJobName.Width.Absolute = 150
        '
        'colJobItem
        '
        Me.colJobItem.DisplayIndex = 2
        Me.colJobItem.Name = "colJobItem"
        Me.colJobItem.Text = "Produced Item"
        Me.colJobItem.Width.Absolute = 150
        '
        'colJobUnitProfit
        '
        Me.colJobUnitProfit.DisplayIndex = 3
        Me.colJobUnitProfit.Name = "colJobUnitProfit"
        Me.colJobUnitProfit.Text = "Unit Profit"
        Me.colJobUnitProfit.Width.Absolute = 120
        '
        'colJobProfitRate
        '
        Me.colJobProfitRate.DisplayIndex = 4
        Me.colJobProfitRate.Name = "colJobProfitRate"
        Me.colJobProfitRate.Text = "Profit Rate (isk/hr)"
        Me.colJobProfitRate.Width.Absolute = 120
        '
        'colJobMargin
        '
        Me.colJobMargin.DisplayIndex = 5
        Me.colJobMargin.Name = "colJobMargin"
        Me.colJobMargin.Text = "Margin (%)"
        Me.colJobMargin.Width.Absolute = 75
        '
        'NodeConnector3
        '
        Me.NodeConnector3.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle3
        '
        Me.ElementStyle3.Class = ""
        Me.ElementStyle3.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle3.Name = "ElementStyle3"
        Me.ElementStyle3.TextColor = System.Drawing.SystemColors.ControlText
        '
        'pnlJobs
        '
        Me.pnlJobs.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlJobs.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlJobs.Controls.Add(Me.btnClearAllJobs)
        Me.pnlJobs.Controls.Add(Me.btnMakeBatch)
        Me.pnlJobs.Controls.Add(Me.btnRefreshJobs)
        Me.pnlJobs.Controls.Add(Me.btnDeleteJob)
        Me.pnlJobs.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlJobs.Location = New System.Drawing.Point(1, 590)
        Me.pnlJobs.Name = "pnlJobs"
        Me.pnlJobs.Size = New System.Drawing.Size(662, 27)
        Me.pnlJobs.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlJobs.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlJobs.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlJobs.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlJobs.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlJobs.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlJobs.Style.GradientAngle = 90
        Me.pnlJobs.TabIndex = 1
        '
        'btnClearAllJobs
        '
        Me.btnClearAllJobs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnClearAllJobs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearAllJobs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnClearAllJobs.Location = New System.Drawing.Point(3, 3)
        Me.btnClearAllJobs.Name = "btnClearAllJobs"
        Me.btnClearAllJobs.Size = New System.Drawing.Size(75, 21)
        Me.btnClearAllJobs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnClearAllJobs.TabIndex = 4
        Me.btnClearAllJobs.Text = "Clear Jobs"
        '
        'btnMakeBatch
        '
        Me.btnMakeBatch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnMakeBatch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnMakeBatch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnMakeBatch.Enabled = False
        Me.btnMakeBatch.Location = New System.Drawing.Point(165, 3)
        Me.btnMakeBatch.Name = "btnMakeBatch"
        Me.btnMakeBatch.Size = New System.Drawing.Size(75, 21)
        Me.btnMakeBatch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnMakeBatch.TabIndex = 6
        Me.btnMakeBatch.Text = "Make Batch"
        '
        'btnRefreshJobs
        '
        Me.btnRefreshJobs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnRefreshJobs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshJobs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnRefreshJobs.Location = New System.Drawing.Point(454, 3)
        Me.btnRefreshJobs.Name = "btnRefreshJobs"
        Me.btnRefreshJobs.Size = New System.Drawing.Size(200, 21)
        Me.btnRefreshJobs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnRefreshJobs.TabIndex = 5
        Me.btnRefreshJobs.Text = "Refresh Job Costings and Profits"
        '
        'btnDeleteJob
        '
        Me.btnDeleteJob.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnDeleteJob.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteJob.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnDeleteJob.Enabled = False
        Me.btnDeleteJob.Location = New System.Drawing.Point(84, 3)
        Me.btnDeleteJob.Name = "btnDeleteJob"
        Me.btnDeleteJob.Size = New System.Drawing.Size(75, 21)
        Me.btnDeleteJob.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnDeleteJob.TabIndex = 1
        Me.btnDeleteJob.Text = "Delete Job"
        '
        'tiProductionJobs
        '
        Me.tiProductionJobs.AttachedControl = Me.TabControlPanel12
        Me.tiProductionJobs.Name = "tiProductionJobs"
        Me.tiProductionJobs.Text = "Production Jobs"
        '
        'TabControlPanel13
        '
        Me.TabControlPanel13.Controls.Add(Me.adtBatches)
        Me.TabControlPanel13.Controls.Add(Me.PanelEx1)
        Me.TabControlPanel13.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel13.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel13.Name = "TabControlPanel13"
        Me.TabControlPanel13.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel13.Size = New System.Drawing.Size(664, 618)
        Me.TabControlPanel13.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel13.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel13.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel13.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel13.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel13.Style.GradientAngle = 90
        Me.TabControlPanel13.TabIndex = 2
        Me.TabControlPanel13.TabItem = Me.tiBatchJobs
        '
        'adtBatches
        '
        Me.adtBatches.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtBatches.AllowDrop = True
        Me.adtBatches.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtBatches.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtBatches.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtBatches.Columns.Add(Me.colBatchName)
        Me.adtBatches.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtBatches.DragDropEnabled = False
        Me.adtBatches.DragDropNodeCopyEnabled = False
        Me.adtBatches.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtBatches.Location = New System.Drawing.Point(1, 1)
        Me.adtBatches.MultiSelect = True
        Me.adtBatches.MultiSelectRule = DevComponents.AdvTree.eMultiSelectRule.AnyNode
        Me.adtBatches.Name = "adtBatches"
        Me.adtBatches.NodesConnector = Me.NodeConnector4
        Me.adtBatches.NodeStyle = Me.ElementStyle4
        Me.adtBatches.PathSeparator = ";"
        Me.adtBatches.Size = New System.Drawing.Size(662, 589)
        Me.adtBatches.Styles.Add(Me.ElementStyle4)
        Me.adtBatches.TabIndex = 1
        Me.adtBatches.Text = "AdvTree1"
        '
        'colBatchName
        '
        Me.colBatchName.Name = "colBatchName"
        Me.colBatchName.Text = "Batch Name"
        Me.colBatchName.Width.Absolute = 300
        '
        'NodeConnector4
        '
        Me.NodeConnector4.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle4
        '
        Me.ElementStyle4.Class = ""
        Me.ElementStyle4.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle4.Name = "ElementStyle4"
        Me.ElementStyle4.TextColor = System.Drawing.SystemColors.ControlText
        '
        'PanelEx1
        '
        Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx1.Controls.Add(Me.btnClearBatches)
        Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelEx1.Location = New System.Drawing.Point(1, 590)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.Size = New System.Drawing.Size(662, 27)
        Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx1.Style.GradientAngle = 90
        Me.PanelEx1.TabIndex = 2
        '
        'btnClearBatches
        '
        Me.btnClearBatches.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnClearBatches.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearBatches.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnClearBatches.Location = New System.Drawing.Point(3, 3)
        Me.btnClearBatches.Name = "btnClearBatches"
        Me.btnClearBatches.Size = New System.Drawing.Size(75, 21)
        Me.btnClearBatches.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnClearBatches.TabIndex = 4
        Me.btnClearBatches.Text = "Clear Batches"
        '
        'tiBatchJobs
        '
        Me.tiBatchJobs.AttachedControl = Me.TabControlPanel13
        Me.tiBatchJobs.Name = "tiBatchJobs"
        Me.tiBatchJobs.Text = "Batch Jobs"
        '
        'PRPM
        '
        Me.PRPM.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PRPM.BatchJob = Nothing
        Me.PRPM.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PRPM.InventionBP = Nothing
        Me.PRPM.Location = New System.Drawing.Point(677, 5)
        Me.PRPM.Name = "PRPM"
        Me.PRPM.ProductionJob = Nothing
        Me.PRPM.Size = New System.Drawing.Size(600, 641)
        Me.PRPM.TabIndex = 3
        '
        'tiProductionManager
        '
        Me.tiProductionManager.AttachedControl = Me.TabControlPanel11
        Me.tiProductionManager.Name = "tiProductionManager"
        Me.tiProductionManager.Text = "Production Manager"
        '
        'TabControlPanel16
        '
        Me.TabControlPanel16.Controls.Add(Me.lblInventionItems)
        Me.TabControlPanel16.Controls.Add(Me.lblInventionInstallers)
        Me.TabControlPanel16.Controls.Add(Me.adtInventionStats)
        Me.TabControlPanel16.Controls.Add(Me.adtInventionResults)
        Me.TabControlPanel16.Controls.Add(Me.cboInventionItems)
        Me.TabControlPanel16.Controls.Add(Me.cboInventionInstallers)
        Me.TabControlPanel16.Controls.Add(Me.btnGetResults)
        Me.TabControlPanel16.Controls.Add(Me.dtiInventionEndDate)
        Me.TabControlPanel16.Controls.Add(Me.lblInvEndDate)
        Me.TabControlPanel16.Controls.Add(Me.dtiInventionStartDate)
        Me.TabControlPanel16.Controls.Add(Me.lblInvStartDate)
        Me.TabControlPanel16.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel16.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel16.Name = "TabControlPanel16"
        Me.TabControlPanel16.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel16.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel16.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel16.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel16.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel16.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel16.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel16.Style.GradientAngle = 90
        Me.TabControlPanel16.TabIndex = 16
        Me.TabControlPanel16.TabItem = Me.tiInventionResults
        '
        'lblInventionItems
        '
        Me.lblInventionItems.AutoSize = True
        Me.lblInventionItems.BackColor = System.Drawing.Color.Transparent
        Me.lblInventionItems.Location = New System.Drawing.Point(12, 36)
        Me.lblInventionItems.Name = "lblInventionItems"
        Me.lblInventionItems.Size = New System.Drawing.Size(38, 13)
        Me.lblInventionItems.TabIndex = 58
        Me.lblInventionItems.Text = "Items:"
        '
        'lblInventionInstallers
        '
        Me.lblInventionInstallers.AutoSize = True
        Me.lblInventionInstallers.BackColor = System.Drawing.Color.Transparent
        Me.lblInventionInstallers.Location = New System.Drawing.Point(12, 12)
        Me.lblInventionInstallers.Name = "lblInventionInstallers"
        Me.lblInventionInstallers.Size = New System.Drawing.Size(55, 13)
        Me.lblInventionInstallers.TabIndex = 57
        Me.lblInventionInstallers.Text = "Installers:"
        '
        'adtInventionStats
        '
        Me.adtInventionStats.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtInventionStats.AllowDrop = True
        Me.adtInventionStats.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtInventionStats.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtInventionStats.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtInventionStats.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtInventionStats.DragDropEnabled = False
        Me.adtInventionStats.DragDropNodeCopyEnabled = False
        Me.adtInventionStats.DropAsChildOffset = 0
        Me.adtInventionStats.ExpandWidth = 0
        Me.adtInventionStats.GridLinesColor = System.Drawing.Color.Silver
        Me.adtInventionStats.GridRowLines = True
        Me.adtInventionStats.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtInventionStats.Location = New System.Drawing.Point(10, 330)
        Me.adtInventionStats.Name = "adtInventionStats"
        Me.adtInventionStats.NodesConnector = Me.NodeConnector16
        Me.adtInventionStats.NodeStyle = Me.ElementStyle13
        Me.adtInventionStats.PathSeparator = ";"
        Me.adtInventionStats.Size = New System.Drawing.Size(1260, 315)
        Me.adtInventionStats.Styles.Add(Me.ElementStyle13)
        Me.adtInventionStats.TabIndex = 56
        Me.adtInventionStats.Text = "AdvTree1"
        '
        'NodeConnector16
        '
        Me.NodeConnector16.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle13
        '
        Me.ElementStyle13.Class = ""
        Me.ElementStyle13.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle13.Name = "ElementStyle13"
        Me.ElementStyle13.TextColor = System.Drawing.SystemColors.ControlText
        '
        'adtInventionResults
        '
        Me.adtInventionResults.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtInventionResults.AllowDrop = True
        Me.adtInventionResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtInventionResults.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtInventionResults.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtInventionResults.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtInventionResults.Columns.Add(Me.colInventionDate)
        Me.adtInventionResults.Columns.Add(Me.colInventionItem)
        Me.adtInventionResults.Columns.Add(Me.colInventionInstaller)
        Me.adtInventionResults.Columns.Add(Me.colInventionResult)
        Me.adtInventionResults.DragDropEnabled = False
        Me.adtInventionResults.DragDropNodeCopyEnabled = False
        Me.adtInventionResults.DropAsChildOffset = 0
        Me.adtInventionResults.ExpandWidth = 0
        Me.adtInventionResults.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtInventionResults.Location = New System.Drawing.Point(10, 62)
        Me.adtInventionResults.Name = "adtInventionResults"
        Me.adtInventionResults.NodesConnector = Me.NodeConnector15
        Me.adtInventionResults.NodeStyle = Me.ElementStyle12
        Me.adtInventionResults.PathSeparator = ";"
        Me.adtInventionResults.Size = New System.Drawing.Size(1260, 262)
        Me.adtInventionResults.Styles.Add(Me.ElementStyle12)
        Me.adtInventionResults.TabIndex = 55
        Me.adtInventionResults.Text = "AdvTree1"
        '
        'colInventionDate
        '
        Me.colInventionDate.DisplayIndex = 1
        Me.colInventionDate.Name = "colInventionDate"
        Me.colInventionDate.Text = "Date"
        Me.colInventionDate.Width.Absolute = 150
        '
        'colInventionItem
        '
        Me.colInventionItem.DisplayIndex = 2
        Me.colInventionItem.Name = "colInventionItem"
        Me.colInventionItem.Text = "Item Type"
        Me.colInventionItem.Width.Absolute = 300
        '
        'colInventionInstaller
        '
        Me.colInventionInstaller.DisplayIndex = 3
        Me.colInventionInstaller.Name = "colInventionInstaller"
        Me.colInventionInstaller.Text = "Installer"
        Me.colInventionInstaller.Width.Absolute = 200
        '
        'colInventionResult
        '
        Me.colInventionResult.DisplayIndex = 4
        Me.colInventionResult.Name = "colInventionResult"
        Me.colInventionResult.Text = "Result"
        Me.colInventionResult.Width.Absolute = 100
        '
        'NodeConnector15
        '
        Me.NodeConnector15.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle12
        '
        Me.ElementStyle12.Class = ""
        Me.ElementStyle12.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle12.Name = "ElementStyle12"
        Me.ElementStyle12.TextColor = System.Drawing.SystemColors.ControlText
        '
        'cboInventionItems
        '
        Me.cboInventionItems.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboInventionItems.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboInventionItems.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboInventionItems.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboInventionItems.ButtonDropDown.Visible = True
        Me.cboInventionItems.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboInventionItems.Location = New System.Drawing.Point(79, 35)
        Me.cboInventionItems.Name = "cboInventionItems"
        Me.cboInventionItems.Size = New System.Drawing.Size(287, 21)
        Me.cboInventionItems.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboInventionItems.TabIndex = 54
        Me.cboInventionItems.Text = ""
        Me.cboInventionItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.cboInventionItems.WatermarkColor = System.Drawing.Color.Silver
        Me.cboInventionItems.WatermarkText = "Select items..."
        '
        'cboInventionInstallers
        '
        Me.cboInventionInstallers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboInventionInstallers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboInventionInstallers.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboInventionInstallers.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboInventionInstallers.ButtonDropDown.Visible = True
        Me.cboInventionInstallers.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboInventionInstallers.Location = New System.Drawing.Point(79, 8)
        Me.cboInventionInstallers.Name = "cboInventionInstallers"
        Me.cboInventionInstallers.Size = New System.Drawing.Size(287, 21)
        Me.cboInventionInstallers.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboInventionInstallers.TabIndex = 52
        Me.cboInventionInstallers.Text = ""
        Me.cboInventionInstallers.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.cboInventionInstallers.WatermarkColor = System.Drawing.Color.Silver
        Me.cboInventionInstallers.WatermarkText = "Select installers..."
        '
        'btnGetResults
        '
        Me.btnGetResults.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnGetResults.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnGetResults.Location = New System.Drawing.Point(686, 8)
        Me.btnGetResults.Name = "btnGetResults"
        Me.btnGetResults.Size = New System.Drawing.Size(98, 23)
        Me.btnGetResults.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnGetResults.TabIndex = 49
        Me.btnGetResults.Text = "Get results"
        '
        'dtiInventionEndDate
        '
        '
        '
        '
        Me.dtiInventionEndDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiInventionEndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiInventionEndDate.ButtonCustom.Text = "Now"
        Me.dtiInventionEndDate.ButtonCustom.Visible = True
        Me.dtiInventionEndDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiInventionEndDate.ButtonCustom2.Text = "SoD"
        Me.dtiInventionEndDate.ButtonCustom2.Visible = True
        Me.dtiInventionEndDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiInventionEndDate.ButtonDropDown.Visible = True
        Me.dtiInventionEndDate.CustomFormat = "yyyy-MM-dd HH-mm-ss"
        Me.dtiInventionEndDate.Format = DevComponents.Editors.eDateTimePickerFormat.Custom
        Me.dtiInventionEndDate.Location = New System.Drawing.Point(465, 35)
        '
        '
        '
        Me.dtiInventionEndDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiInventionEndDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiInventionEndDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiInventionEndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiInventionEndDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiInventionEndDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiInventionEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiInventionEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiInventionEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiInventionEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiInventionEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiInventionEndDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiInventionEndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiInventionEndDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiInventionEndDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiInventionEndDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiInventionEndDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiInventionEndDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiInventionEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiInventionEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiInventionEndDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiInventionEndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiInventionEndDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiInventionEndDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiInventionEndDate.Name = "dtiInventionEndDate"
        Me.dtiInventionEndDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiInventionEndDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiInventionEndDate.TabIndex = 47
        Me.dtiInventionEndDate.Value = New Date(2010, 9, 15, 20, 35, 1, 0)
        '
        'lblInvEndDate
        '
        Me.lblInvEndDate.AutoSize = True
        Me.lblInvEndDate.BackColor = System.Drawing.Color.Transparent
        Me.lblInvEndDate.Location = New System.Drawing.Point(398, 39)
        Me.lblInvEndDate.Name = "lblInvEndDate"
        Me.lblInvEndDate.Size = New System.Drawing.Size(55, 13)
        Me.lblInvEndDate.TabIndex = 48
        Me.lblInvEndDate.Text = "End Date:"
        '
        'dtiInventionStartDate
        '
        '
        '
        '
        Me.dtiInventionStartDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiInventionStartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiInventionStartDate.ButtonCustom.Text = "Now"
        Me.dtiInventionStartDate.ButtonCustom.Visible = True
        Me.dtiInventionStartDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiInventionStartDate.ButtonCustom2.Text = "SoD"
        Me.dtiInventionStartDate.ButtonCustom2.Visible = True
        Me.dtiInventionStartDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiInventionStartDate.ButtonDropDown.Visible = True
        Me.dtiInventionStartDate.CustomFormat = "yyyy-MM-dd HH-mm-ss"
        Me.dtiInventionStartDate.Format = DevComponents.Editors.eDateTimePickerFormat.Custom
        Me.dtiInventionStartDate.Location = New System.Drawing.Point(465, 8)
        '
        '
        '
        Me.dtiInventionStartDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiInventionStartDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiInventionStartDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiInventionStartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiInventionStartDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiInventionStartDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiInventionStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiInventionStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiInventionStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiInventionStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiInventionStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiInventionStartDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiInventionStartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiInventionStartDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiInventionStartDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiInventionStartDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiInventionStartDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiInventionStartDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiInventionStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiInventionStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiInventionStartDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiInventionStartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiInventionStartDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiInventionStartDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiInventionStartDate.Name = "dtiInventionStartDate"
        Me.dtiInventionStartDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiInventionStartDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiInventionStartDate.TabIndex = 45
        Me.dtiInventionStartDate.Value = New Date(2010, 9, 15, 20, 34, 46, 0)
        '
        'lblInvStartDate
        '
        Me.lblInvStartDate.AutoSize = True
        Me.lblInvStartDate.BackColor = System.Drawing.Color.Transparent
        Me.lblInvStartDate.Location = New System.Drawing.Point(398, 14)
        Me.lblInvStartDate.Name = "lblInvStartDate"
        Me.lblInvStartDate.Size = New System.Drawing.Size(61, 13)
        Me.lblInvStartDate.TabIndex = 46
        Me.lblInvStartDate.Text = "Start Date:"
        '
        'tiInventionResults
        '
        Me.tiInventionResults.AttachedControl = Me.TabControlPanel16
        Me.tiInventionResults.Name = "tiInventionResults"
        Me.tiInventionResults.Text = "Invention Results"
        '
        'TabControlPanel7
        '
        Me.TabControlPanel7.Controls.Add(Me.lblStatusFilter)
        Me.TabControlPanel7.Controls.Add(Me.cboStatusFilter)
        Me.TabControlPanel7.Controls.Add(Me.lblActivityFilter)
        Me.TabControlPanel7.Controls.Add(Me.cboActivityFilter)
        Me.TabControlPanel7.Controls.Add(Me.cboInstallerFilter)
        Me.TabControlPanel7.Controls.Add(Me.cboJobOwner)
        Me.TabControlPanel7.Controls.Add(Me.lblJobOwner)
        Me.TabControlPanel7.Controls.Add(Me.adtJobs)
        Me.TabControlPanel7.Controls.Add(Me.lblJobInstallerFilter)
        Me.TabControlPanel7.Controls.Add(Me.btnExportJobs)
        Me.TabControlPanel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel7.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel7.Name = "TabControlPanel7"
        Me.TabControlPanel7.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel7.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel7.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel7.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel7.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel7.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel7.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel7.Style.GradientAngle = 90
        Me.TabControlPanel7.TabIndex = 7
        Me.TabControlPanel7.TabItem = Me.tiJobs
        '
        'lblStatusFilter
        '
        Me.lblStatusFilter.AutoSize = True
        Me.lblStatusFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblStatusFilter.Location = New System.Drawing.Point(801, 11)
        Me.lblStatusFilter.Name = "lblStatusFilter"
        Me.lblStatusFilter.Size = New System.Drawing.Size(42, 13)
        Me.lblStatusFilter.TabIndex = 37
        Me.lblStatusFilter.Text = "Status:"
        '
        'cboStatusFilter
        '
        Me.cboStatusFilter.DisplayMember = "Text"
        Me.cboStatusFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatusFilter.FormattingEnabled = True
        Me.cboStatusFilter.ItemHeight = 15
        Me.cboStatusFilter.Location = New System.Drawing.Point(854, 7)
        Me.cboStatusFilter.Name = "cboStatusFilter"
        Me.cboStatusFilter.Size = New System.Drawing.Size(164, 21)
        Me.cboStatusFilter.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboStatusFilter.TabIndex = 36
        '
        'lblActivityFilter
        '
        Me.lblActivityFilter.AutoSize = True
        Me.lblActivityFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblActivityFilter.Location = New System.Drawing.Point(567, 11)
        Me.lblActivityFilter.Name = "lblActivityFilter"
        Me.lblActivityFilter.Size = New System.Drawing.Size(47, 13)
        Me.lblActivityFilter.TabIndex = 35
        Me.lblActivityFilter.Text = "Activity:"
        '
        'cboActivityFilter
        '
        Me.cboActivityFilter.DisplayMember = "Text"
        Me.cboActivityFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboActivityFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboActivityFilter.FormattingEnabled = True
        Me.cboActivityFilter.ItemHeight = 15
        Me.cboActivityFilter.Location = New System.Drawing.Point(620, 7)
        Me.cboActivityFilter.Name = "cboActivityFilter"
        Me.cboActivityFilter.Size = New System.Drawing.Size(164, 21)
        Me.cboActivityFilter.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboActivityFilter.TabIndex = 34
        '
        'cboInstallerFilter
        '
        Me.cboInstallerFilter.DisplayMember = "Text"
        Me.cboInstallerFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboInstallerFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboInstallerFilter.FormattingEnabled = True
        Me.cboInstallerFilter.ItemHeight = 15
        Me.cboInstallerFilter.Location = New System.Drawing.Point(386, 7)
        Me.cboInstallerFilter.Name = "cboInstallerFilter"
        Me.cboInstallerFilter.Size = New System.Drawing.Size(164, 21)
        Me.cboInstallerFilter.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboInstallerFilter.TabIndex = 33
        '
        'cboJobOwner
        '
        Me.cboJobOwner.DisplayMember = "Text"
        Me.cboJobOwner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboJobOwner.FormattingEnabled = True
        Me.cboJobOwner.ItemHeight = 15
        Me.cboJobOwner.Location = New System.Drawing.Point(64, 7)
        Me.cboJobOwner.Name = "cboJobOwner"
        Me.cboJobOwner.Size = New System.Drawing.Size(210, 21)
        Me.cboJobOwner.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboJobOwner.TabIndex = 32
        '
        'lblJobOwner
        '
        Me.lblJobOwner.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblJobOwner.AutoSize = True
        Me.lblJobOwner.BackColor = System.Drawing.Color.Transparent
        Me.lblJobOwner.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblJobOwner.Location = New System.Drawing.Point(12, 11)
        Me.lblJobOwner.Name = "lblJobOwner"
        Me.lblJobOwner.Size = New System.Drawing.Size(43, 13)
        Me.lblJobOwner.TabIndex = 31
        Me.lblJobOwner.Text = "Owner:"
        '
        'adtJobs
        '
        Me.adtJobs.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtJobs.AllowDrop = True
        Me.adtJobs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtJobs.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtJobs.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtJobs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtJobs.Columns.Add(Me.colIJobsItem)
        Me.adtJobs.Columns.Add(Me.colIJobsActivity)
        Me.adtJobs.Columns.Add(Me.colIJobsRuns)
        Me.adtJobs.Columns.Add(Me.colIJobsInstaller)
        Me.adtJobs.Columns.Add(Me.colIJobsLocation)
        Me.adtJobs.Columns.Add(Me.colIJobsEndTime)
        Me.adtJobs.Columns.Add(Me.colIJobsStatus)
        Me.adtJobs.DragDropEnabled = False
        Me.adtJobs.DragDropNodeCopyEnabled = False
        Me.adtJobs.DropAsChildOffset = 0
        Me.adtJobs.ExpandWidth = 0
        Me.adtJobs.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtJobs.Location = New System.Drawing.Point(12, 34)
        Me.adtJobs.Name = "adtJobs"
        Me.adtJobs.NodesConnector = Me.NodeConnector7
        Me.adtJobs.NodeStyle = Me.ElementStyle5
        Me.adtJobs.PathSeparator = ";"
        Me.adtJobs.Size = New System.Drawing.Size(1260, 611)
        Me.adtJobs.Styles.Add(Me.ElementStyle5)
        Me.adtJobs.TabIndex = 8
        Me.adtJobs.Text = "AdvTree1"
        '
        'colIJobsItem
        '
        Me.colIJobsItem.DisplayIndex = 1
        Me.colIJobsItem.Name = "colIJobsItem"
        Me.colIJobsItem.Text = "Installed Item"
        Me.colIJobsItem.Width.Absolute = 300
        '
        'colIJobsActivity
        '
        Me.colIJobsActivity.DisplayIndex = 2
        Me.colIJobsActivity.Name = "colIJobsActivity"
        Me.colIJobsActivity.Text = "Activity"
        Me.colIJobsActivity.Width.Absolute = 100
        '
        'colIJobsRuns
        '
        Me.colIJobsRuns.DisplayIndex = 3
        Me.colIJobsRuns.Name = "colIJobsRuns"
        Me.colIJobsRuns.Text = "Runs"
        Me.colIJobsRuns.Width.Absolute = 50
        '
        'colIJobsInstaller
        '
        Me.colIJobsInstaller.DisplayIndex = 4
        Me.colIJobsInstaller.Name = "colIJobsInstaller"
        Me.colIJobsInstaller.Text = "Installer"
        Me.colIJobsInstaller.Width.Absolute = 150
        '
        'colIJobsLocation
        '
        Me.colIJobsLocation.DisplayIndex = 5
        Me.colIJobsLocation.Name = "colIJobsLocation"
        Me.colIJobsLocation.Text = "Location"
        Me.colIJobsLocation.Width.Absolute = 300
        '
        'colIJobsEndTime
        '
        Me.colIJobsEndTime.DisplayIndex = 6
        Me.colIJobsEndTime.Name = "colIJobsEndTime"
        Me.colIJobsEndTime.Text = "End Time"
        Me.colIJobsEndTime.Width.Absolute = 100
        '
        'colIJobsStatus
        '
        Me.colIJobsStatus.DisplayIndex = 7
        Me.colIJobsStatus.Name = "colIJobsStatus"
        Me.colIJobsStatus.Text = "Status"
        Me.colIJobsStatus.Width.Absolute = 100
        '
        'NodeConnector7
        '
        Me.NodeConnector7.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle5
        '
        Me.ElementStyle5.Class = ""
        Me.ElementStyle5.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle5.Name = "ElementStyle5"
        Me.ElementStyle5.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiJobs
        '
        Me.tiJobs.AttachedControl = Me.TabControlPanel7
        Me.tiJobs.Name = "tiJobs"
        Me.tiJobs.Text = "Jobs"
        '
        'TabControlPanel5
        '
        Me.TabControlPanel5.Controls.Add(Me.lblTransProfitRatio)
        Me.TabControlPanel5.Controls.Add(Me.lblTransProfitValue)
        Me.TabControlPanel5.Controls.Add(Me.lblTransSellValue)
        Me.TabControlPanel5.Controls.Add(Me.lblTransBuyValue)
        Me.TabControlPanel5.Controls.Add(Me.cboWalletTransItem)
        Me.TabControlPanel5.Controls.Add(Me.lblTransItemType)
        Me.TabControlPanel5.Controls.Add(Me.cboTransactionOwner)
        Me.TabControlPanel5.Controls.Add(Me.cboWalletTransDivision)
        Me.TabControlPanel5.Controls.Add(Me.cboWalletTransType)
        Me.TabControlPanel5.Controls.Add(Me.btnGetTransactions)
        Me.TabControlPanel5.Controls.Add(Me.dtiTransEndDate)
        Me.TabControlPanel5.Controls.Add(Me.Label2)
        Me.TabControlPanel5.Controls.Add(Me.dtiTransStartDate)
        Me.TabControlPanel5.Controls.Add(Me.Label1)
        Me.TabControlPanel5.Controls.Add(Me.adtTransactions)
        Me.TabControlPanel5.Controls.Add(Me.lblType)
        Me.TabControlPanel5.Controls.Add(Me.lblWalletTransDivision)
        Me.TabControlPanel5.Controls.Add(Me.btnExportTransactions)
        Me.TabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel5.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel5.Name = "TabControlPanel5"
        Me.TabControlPanel5.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel5.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel5.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel5.Style.GradientAngle = 90
        Me.TabControlPanel5.TabIndex = 5
        Me.TabControlPanel5.TabItem = Me.tiTransactions
        '
        'lblTransProfitRatio
        '
        Me.lblTransProfitRatio.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTransProfitRatio.AutoSize = True
        Me.lblTransProfitRatio.BackColor = System.Drawing.Color.Transparent
        Me.lblTransProfitRatio.Location = New System.Drawing.Point(333, 633)
        Me.lblTransProfitRatio.Name = "lblTransProfitRatio"
        Me.lblTransProfitRatio.Size = New System.Drawing.Size(51, 13)
        Me.lblTransProfitRatio.TabIndex = 46
        Me.lblTransProfitRatio.Text = "Profit %:"
        '
        'lblTransProfitValue
        '
        Me.lblTransProfitValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTransProfitValue.AutoSize = True
        Me.lblTransProfitValue.BackColor = System.Drawing.Color.Transparent
        Me.lblTransProfitValue.Location = New System.Drawing.Point(333, 618)
        Me.lblTransProfitValue.Name = "lblTransProfitValue"
        Me.lblTransProfitValue.Size = New System.Drawing.Size(66, 13)
        Me.lblTransProfitValue.TabIndex = 45
        Me.lblTransProfitValue.Text = "Profit Value:"
        '
        'lblTransSellValue
        '
        Me.lblTransSellValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTransSellValue.AutoSize = True
        Me.lblTransSellValue.BackColor = System.Drawing.Color.Transparent
        Me.lblTransSellValue.Location = New System.Drawing.Point(7, 633)
        Me.lblTransSellValue.Name = "lblTransSellValue"
        Me.lblTransSellValue.Size = New System.Drawing.Size(56, 13)
        Me.lblTransSellValue.TabIndex = 44
        Me.lblTransSellValue.Text = "Sell Value:"
        '
        'lblTransBuyValue
        '
        Me.lblTransBuyValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTransBuyValue.AutoSize = True
        Me.lblTransBuyValue.BackColor = System.Drawing.Color.Transparent
        Me.lblTransBuyValue.Location = New System.Drawing.Point(7, 618)
        Me.lblTransBuyValue.Name = "lblTransBuyValue"
        Me.lblTransBuyValue.Size = New System.Drawing.Size(58, 13)
        Me.lblTransBuyValue.TabIndex = 43
        Me.lblTransBuyValue.Text = "Buy Value:"
        '
        'cboWalletTransItem
        '
        Me.cboWalletTransItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboWalletTransItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboWalletTransItem.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboWalletTransItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboWalletTransItem.ButtonDropDown.Visible = True
        Me.cboWalletTransItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboWalletTransItem.Location = New System.Drawing.Point(400, 60)
        Me.cboWalletTransItem.Name = "cboWalletTransItem"
        Me.cboWalletTransItem.Size = New System.Drawing.Size(271, 21)
        Me.cboWalletTransItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWalletTransItem.TabIndex = 42
        Me.cboWalletTransItem.Text = ""
        Me.cboWalletTransItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.cboWalletTransItem.WatermarkColor = System.Drawing.Color.Silver
        Me.cboWalletTransItem.WatermarkText = "Select items..."
        '
        'lblTransItemType
        '
        Me.lblTransItemType.AutoSize = True
        Me.lblTransItemType.BackColor = System.Drawing.Color.Transparent
        Me.lblTransItemType.Location = New System.Drawing.Point(314, 65)
        Me.lblTransItemType.Name = "lblTransItemType"
        Me.lblTransItemType.Size = New System.Drawing.Size(60, 13)
        Me.lblTransItemType.TabIndex = 41
        Me.lblTransItemType.Text = "Item Type:"
        '
        'cboTransactionOwner
        '
        Me.cboTransactionOwner.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboTransactionOwner.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboTransactionOwner.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboTransactionOwner.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboTransactionOwner.ButtonDropDown.Visible = True
        Me.cboTransactionOwner.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboTransactionOwner.Location = New System.Drawing.Point(12, 7)
        Me.cboTransactionOwner.Name = "cboTransactionOwner"
        Me.cboTransactionOwner.Size = New System.Drawing.Size(287, 21)
        Me.cboTransactionOwner.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboTransactionOwner.TabIndex = 40
        Me.cboTransactionOwner.Text = ""
        Me.cboTransactionOwner.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.cboTransactionOwner.WatermarkColor = System.Drawing.Color.Silver
        Me.cboTransactionOwner.WatermarkText = "Select owners..."
        '
        'cboWalletTransDivision
        '
        Me.cboWalletTransDivision.DisplayMember = "Text"
        Me.cboWalletTransDivision.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboWalletTransDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWalletTransDivision.FormattingEnabled = True
        Me.cboWalletTransDivision.ItemHeight = 15
        Me.cboWalletTransDivision.Location = New System.Drawing.Point(400, 6)
        Me.cboWalletTransDivision.Name = "cboWalletTransDivision"
        Me.cboWalletTransDivision.Size = New System.Drawing.Size(150, 21)
        Me.cboWalletTransDivision.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWalletTransDivision.TabIndex = 39
        '
        'cboWalletTransType
        '
        Me.cboWalletTransType.DisplayMember = "Text"
        Me.cboWalletTransType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboWalletTransType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWalletTransType.FormattingEnabled = True
        Me.cboWalletTransType.ItemHeight = 15
        Me.cboWalletTransType.Items.AddRange(New Object() {Me.cboTransAll, Me.cboTransBuy, Me.cboTransSell})
        Me.cboWalletTransType.Location = New System.Drawing.Point(400, 33)
        Me.cboWalletTransType.Name = "cboWalletTransType"
        Me.cboWalletTransType.Size = New System.Drawing.Size(150, 21)
        Me.cboWalletTransType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWalletTransType.TabIndex = 38
        '
        'cboTransAll
        '
        Me.cboTransAll.Text = "Show All"
        '
        'cboTransBuy
        '
        Me.cboTransBuy.Text = "Buy"
        '
        'cboTransSell
        '
        Me.cboTransSell.Text = "Sell"
        '
        'btnGetTransactions
        '
        Me.btnGetTransactions.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnGetTransactions.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnGetTransactions.Location = New System.Drawing.Point(573, 13)
        Me.btnGetTransactions.Name = "btnGetTransactions"
        Me.btnGetTransactions.Size = New System.Drawing.Size(98, 23)
        Me.btnGetTransactions.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnGetTransactions.TabIndex = 37
        Me.btnGetTransactions.Text = "Get Transactions"
        '
        'dtiTransEndDate
        '
        '
        '
        '
        Me.dtiTransEndDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiTransEndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiTransEndDate.ButtonCustom.Text = "Now"
        Me.dtiTransEndDate.ButtonCustom.Visible = True
        Me.dtiTransEndDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiTransEndDate.ButtonCustom2.Text = "SoD"
        Me.dtiTransEndDate.ButtonCustom2.Visible = True
        Me.dtiTransEndDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiTransEndDate.ButtonDropDown.Visible = True
        Me.dtiTransEndDate.CustomFormat = "yyyy-MM-dd HH-mm-ss"
        Me.dtiTransEndDate.Format = DevComponents.Editors.eDateTimePickerFormat.Custom
        Me.dtiTransEndDate.Location = New System.Drawing.Point(99, 60)
        '
        '
        '
        Me.dtiTransEndDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiTransEndDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiTransEndDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiTransEndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiTransEndDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiTransEndDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiTransEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiTransEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiTransEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiTransEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiTransEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiTransEndDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiTransEndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiTransEndDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiTransEndDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiTransEndDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiTransEndDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiTransEndDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiTransEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiTransEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiTransEndDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiTransEndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiTransEndDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiTransEndDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiTransEndDate.Name = "dtiTransEndDate"
        Me.dtiTransEndDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiTransEndDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiTransEndDate.TabIndex = 35
        Me.dtiTransEndDate.Value = New Date(2010, 9, 15, 20, 35, 1, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(12, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 36
        Me.Label2.Text = "End Date:"
        '
        'dtiTransStartDate
        '
        '
        '
        '
        Me.dtiTransStartDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiTransStartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiTransStartDate.ButtonCustom.Text = "Now"
        Me.dtiTransStartDate.ButtonCustom.Visible = True
        Me.dtiTransStartDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiTransStartDate.ButtonCustom2.Text = "SoD"
        Me.dtiTransStartDate.ButtonCustom2.Visible = True
        Me.dtiTransStartDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiTransStartDate.ButtonDropDown.Visible = True
        Me.dtiTransStartDate.CustomFormat = "yyyy-MM-dd HH-mm-ss"
        Me.dtiTransStartDate.Format = DevComponents.Editors.eDateTimePickerFormat.Custom
        Me.dtiTransStartDate.Location = New System.Drawing.Point(99, 33)
        '
        '
        '
        Me.dtiTransStartDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiTransStartDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiTransStartDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiTransStartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiTransStartDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiTransStartDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiTransStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiTransStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiTransStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiTransStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiTransStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiTransStartDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiTransStartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiTransStartDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiTransStartDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiTransStartDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiTransStartDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiTransStartDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiTransStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiTransStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiTransStartDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiTransStartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiTransStartDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiTransStartDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiTransStartDate.Name = "dtiTransStartDate"
        Me.dtiTransStartDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiTransStartDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiTransStartDate.TabIndex = 33
        Me.dtiTransStartDate.Value = New Date(2010, 9, 15, 20, 34, 46, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(12, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Start Date:"
        '
        'adtTransactions
        '
        Me.adtTransactions.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtTransactions.AllowDrop = True
        Me.adtTransactions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtTransactions.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtTransactions.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtTransactions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtTransactions.Columns.Add(Me.colTransDate)
        Me.adtTransactions.Columns.Add(Me.colTransItem)
        Me.adtTransactions.Columns.Add(Me.colTransQuantity)
        Me.adtTransactions.Columns.Add(Me.colTransPrice)
        Me.adtTransactions.Columns.Add(Me.colTransValue)
        Me.adtTransactions.Columns.Add(Me.colTransLocation)
        Me.adtTransactions.Columns.Add(Me.colTransClient)
        Me.adtTransactions.ContextMenuStrip = Me.ctxTransactions
        Me.adtTransactions.DragDropEnabled = False
        Me.adtTransactions.DragDropNodeCopyEnabled = False
        Me.adtTransactions.ExpandWidth = 0
        Me.adtTransactions.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtTransactions.Location = New System.Drawing.Point(7, 87)
        Me.adtTransactions.Name = "adtTransactions"
        Me.adtTransactions.NodesConnector = Me.NodeConnector10
        Me.adtTransactions.NodeStyle = Me.Personal
        Me.adtTransactions.PathSeparator = ";"
        Me.adtTransactions.Size = New System.Drawing.Size(1265, 528)
        Me.adtTransactions.Styles.Add(Me.Personal)
        Me.adtTransactions.Styles.Add(Me.Corp)
        Me.adtTransactions.Styles.Add(Me.Buy)
        Me.adtTransactions.Styles.Add(Me.Sell)
        Me.adtTransactions.Styles.Add(Me.Numeric)
        Me.adtTransactions.TabIndex = 6
        Me.adtTransactions.Text = "AdvTree1"
        '
        'colTransDate
        '
        Me.colTransDate.DisplayIndex = 1
        Me.colTransDate.Name = "colTransDate"
        Me.colTransDate.Text = "Date"
        Me.colTransDate.Width.Absolute = 120
        '
        'colTransItem
        '
        Me.colTransItem.DisplayIndex = 2
        Me.colTransItem.Name = "colTransItem"
        Me.colTransItem.Text = "Item"
        Me.colTransItem.Width.Absolute = 300
        '
        'colTransQuantity
        '
        Me.colTransQuantity.DisplayIndex = 3
        Me.colTransQuantity.Name = "colTransQuantity"
        Me.colTransQuantity.StyleNormal = "Numeric"
        Me.colTransQuantity.Text = "Quantity"
        Me.colTransQuantity.Width.Absolute = 75
        '
        'colTransPrice
        '
        Me.colTransPrice.DisplayIndex = 4
        Me.colTransPrice.Name = "colTransPrice"
        Me.colTransPrice.StyleNormal = "Numeric"
        Me.colTransPrice.Text = "Price"
        Me.colTransPrice.Width.Absolute = 120
        '
        'colTransValue
        '
        Me.colTransValue.DisplayIndex = 5
        Me.colTransValue.Name = "colTransValue"
        Me.colTransValue.StyleNormal = "Numeric"
        Me.colTransValue.Text = "Total Value"
        Me.colTransValue.Width.Absolute = 120
        '
        'colTransLocation
        '
        Me.colTransLocation.DisplayIndex = 6
        Me.colTransLocation.Name = "colTransLocation"
        Me.colTransLocation.Text = "Location"
        Me.colTransLocation.Width.Absolute = 300
        '
        'colTransClient
        '
        Me.colTransClient.DisplayIndex = 7
        Me.colTransClient.Name = "colTransClient"
        Me.colTransClient.Text = "Client"
        Me.colTransClient.Width.Absolute = 150
        '
        'NodeConnector10
        '
        Me.NodeConnector10.LineColor = System.Drawing.SystemColors.ControlText
        '
        'Personal
        '
        Me.Personal.Class = ""
        Me.Personal.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Personal.Name = "Personal"
        Me.Personal.TextColor = System.Drawing.SystemColors.ControlText
        '
        'Corp
        '
        Me.Corp.Class = ""
        Me.Corp.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Corp.Name = "Corp"
        Me.Corp.TextColor = System.Drawing.Color.SlateBlue
        '
        'Buy
        '
        Me.Buy.Class = ""
        Me.Buy.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Buy.Name = "Buy"
        Me.Buy.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far
        Me.Buy.TextColor = System.Drawing.Color.Red
        '
        'Sell
        '
        Me.Sell.Class = ""
        Me.Sell.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Sell.Name = "Sell"
        Me.Sell.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far
        Me.Sell.TextColor = System.Drawing.Color.LimeGreen
        '
        'Numeric
        '
        Me.Numeric.Class = ""
        Me.Numeric.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Numeric.Name = "Numeric"
        Me.Numeric.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far
        Me.Numeric.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiTransactions
        '
        Me.tiTransactions.AttachedControl = Me.TabControlPanel5
        Me.tiTransactions.Name = "tiTransactions"
        Me.tiTransactions.Text = "Transactions"
        '
        'TabControlPanel10
        '
        Me.TabControlPanel10.Controls.Add(Me.zgcPrism)
        Me.TabControlPanel10.Controls.Add(Me.PanelEx2)
        Me.TabControlPanel10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel10.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel10.Name = "TabControlPanel10"
        Me.TabControlPanel10.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel10.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel10.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel10.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel10.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel10.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel10.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel10.Style.GradientAngle = 90
        Me.TabControlPanel10.TabIndex = 13
        Me.TabControlPanel10.TabItem = Me.tiCharts
        '
        'zgcPrism
        '
        Me.zgcPrism.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgcPrism.Location = New System.Drawing.Point(309, 1)
        Me.zgcPrism.Name = "zgcPrism"
        Me.zgcPrism.ScrollGrace = 0.0R
        Me.zgcPrism.ScrollMaxX = 0.0R
        Me.zgcPrism.ScrollMaxY = 0.0R
        Me.zgcPrism.ScrollMaxY2 = 0.0R
        Me.zgcPrism.ScrollMinX = 0.0R
        Me.zgcPrism.ScrollMinY = 0.0R
        Me.zgcPrism.ScrollMinY2 = 0.0R
        Me.zgcPrism.Size = New System.Drawing.Size(974, 648)
        Me.zgcPrism.TabIndex = 36
        Me.zgcPrism.Visible = False
        '
        'PanelEx2
        '
        Me.PanelEx2.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx2.Controls.Add(Me.cboChart)
        Me.PanelEx2.Controls.Add(Me.Label3)
        Me.PanelEx2.Controls.Add(Me.btnGenerateChart)
        Me.PanelEx2.Controls.Add(Me.Label4)
        Me.PanelEx2.Controls.Add(Me.dtiChartEndDate)
        Me.PanelEx2.Controls.Add(Me.dtiChartStartDate)
        Me.PanelEx2.Controls.Add(Me.cboChartOwners)
        Me.PanelEx2.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelEx2.Location = New System.Drawing.Point(1, 1)
        Me.PanelEx2.Name = "PanelEx2"
        Me.PanelEx2.Size = New System.Drawing.Size(308, 648)
        Me.PanelEx2.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx2.Style.GradientAngle = 90
        Me.PanelEx2.TabIndex = 35
        '
        'cboChart
        '
        Me.cboChart.DisplayMember = "Text"
        Me.cboChart.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboChart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboChart.FormattingEnabled = True
        Me.cboChart.ItemHeight = 15
        Me.cboChart.Location = New System.Drawing.Point(12, 14)
        Me.cboChart.Name = "cboChart"
        Me.cboChart.Size = New System.Drawing.Size(236, 21)
        Me.cboChart.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboChart.TabIndex = 33
        Me.cboChart.WatermarkColor = System.Drawing.Color.Silver
        Me.cboChart.WatermarkText = "Select chart..."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(13, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Start Date:"
        '
        'btnGenerateChart
        '
        Me.btnGenerateChart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnGenerateChart.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnGenerateChart.Location = New System.Drawing.Point(12, 191)
        Me.btnGenerateChart.Name = "btnGenerateChart"
        Me.btnGenerateChart.Size = New System.Drawing.Size(100, 23)
        Me.btnGenerateChart.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnGenerateChart.TabIndex = 28
        Me.btnGenerateChart.Text = "Generate Chart"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(13, 85)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "End Date:"
        '
        'dtiChartEndDate
        '
        '
        '
        '
        Me.dtiChartEndDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiChartEndDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiChartEndDate.ButtonCustom.Text = "Now"
        Me.dtiChartEndDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiChartEndDate.ButtonCustom2.Text = "SoD"
        Me.dtiChartEndDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiChartEndDate.ButtonDropDown.Visible = True
        Me.dtiChartEndDate.CustomFormat = "yyyy-MM-dd"
        Me.dtiChartEndDate.Format = DevComponents.Editors.eDateTimePickerFormat.[Long]
        Me.dtiChartEndDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Right
        Me.dtiChartEndDate.Location = New System.Drawing.Point(99, 81)
        '
        '
        '
        Me.dtiChartEndDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiChartEndDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiChartEndDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiChartEndDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiChartEndDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiChartEndDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiChartEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiChartEndDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiChartEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiChartEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiChartEndDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiChartEndDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiChartEndDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiChartEndDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiChartEndDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiChartEndDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiChartEndDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiChartEndDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiChartEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiChartEndDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiChartEndDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiChartEndDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiChartEndDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiChartEndDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiChartEndDate.Name = "dtiChartEndDate"
        Me.dtiChartEndDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiChartEndDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiChartEndDate.TabIndex = 23
        Me.dtiChartEndDate.Value = New Date(2011, 6, 9, 0, 0, 0, 0)
        '
        'dtiChartStartDate
        '
        '
        '
        '
        Me.dtiChartStartDate.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.dtiChartStartDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiChartStartDate.ButtonCustom.Text = "Now"
        Me.dtiChartStartDate.ButtonCustom2.DisplayPosition = 1
        Me.dtiChartStartDate.ButtonCustom2.Text = "SoD"
        Me.dtiChartStartDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.dtiChartStartDate.ButtonDropDown.Visible = True
        Me.dtiChartStartDate.CustomFormat = "yyyy-MM-dd"
        Me.dtiChartStartDate.Format = DevComponents.Editors.eDateTimePickerFormat.[Long]
        Me.dtiChartStartDate.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Right
        Me.dtiChartStartDate.Location = New System.Drawing.Point(99, 54)
        '
        '
        '
        Me.dtiChartStartDate.MonthCalendar.AnnuallyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiChartStartDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window
        Me.dtiChartStartDate.MonthCalendar.BackgroundStyle.Class = ""
        Me.dtiChartStartDate.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiChartStartDate.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.dtiChartStartDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.dtiChartStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiChartStartDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.dtiChartStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.dtiChartStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.dtiChartStartDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.dtiChartStartDate.MonthCalendar.CommandsBackgroundStyle.Class = ""
        Me.dtiChartStartDate.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiChartStartDate.MonthCalendar.DisplayMonth = New Date(2010, 9, 1, 0, 0, 0, 0)
        Me.dtiChartStartDate.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        Me.dtiChartStartDate.MonthCalendar.MarkedDates = New Date(-1) {}
        Me.dtiChartStartDate.MonthCalendar.MonthlyMarkedDates = New Date(-1) {}
        '
        '
        '
        Me.dtiChartStartDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.dtiChartStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.dtiChartStartDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.dtiChartStartDate.MonthCalendar.NavigationBackgroundStyle.Class = ""
        Me.dtiChartStartDate.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.dtiChartStartDate.MonthCalendar.TodayButtonVisible = True
        Me.dtiChartStartDate.MonthCalendar.WeeklyMarkedDays = New System.DayOfWeek(-1) {}
        Me.dtiChartStartDate.Name = "dtiChartStartDate"
        Me.dtiChartStartDate.Size = New System.Drawing.Size(200, 21)
        Me.dtiChartStartDate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.dtiChartStartDate.TabIndex = 22
        Me.dtiChartStartDate.Value = New Date(2011, 6, 9, 0, 0, 0, 0)
        '
        'cboChartOwners
        '
        Me.cboChartOwners.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboChartOwners.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboChartOwners.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboChartOwners.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboChartOwners.ButtonDropDown.Visible = True
        Me.cboChartOwners.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboChartOwners.Location = New System.Drawing.Point(12, 108)
        Me.cboChartOwners.Name = "cboChartOwners"
        Me.cboChartOwners.Size = New System.Drawing.Size(287, 21)
        Me.cboChartOwners.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboChartOwners.TabIndex = 26
        Me.cboChartOwners.Text = ""
        Me.cboChartOwners.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.cboChartOwners.WatermarkColor = System.Drawing.Color.Silver
        Me.cboChartOwners.WatermarkText = "Select owners..."
        '
        'tiCharts
        '
        Me.tiCharts.AttachedControl = Me.TabControlPanel10
        Me.tiCharts.Name = "tiCharts"
        Me.tiCharts.Text = "Prism Charts"
        '
        'TabControlPanel15
        '
        Me.TabControlPanel15.Controls.Add(Me.pnlRigs)
        Me.TabControlPanel15.Controls.Add(Me.PSCRigOwners)
        Me.TabControlPanel15.Controls.Add(Me.lblTotalRigMargin)
        Me.TabControlPanel15.Controls.Add(Me.lblTotalRigProfit)
        Me.TabControlPanel15.Controls.Add(Me.lblTotalRigSalePrice)
        Me.TabControlPanel15.Controls.Add(Me.gpAutoRig)
        Me.TabControlPanel15.Controls.Add(Me.btnBuildRigs)
        Me.TabControlPanel15.Controls.Add(Me.btnExportRigBuildList)
        Me.TabControlPanel15.Controls.Add(Me.btnExportRigList)
        Me.TabControlPanel15.Controls.Add(Me.nudRigMELevel)
        Me.TabControlPanel15.Controls.Add(Me.lblRigMELevel)
        Me.TabControlPanel15.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel15.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel15.Name = "TabControlPanel15"
        Me.TabControlPanel15.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel15.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel15.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel15.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel15.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel15.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel15.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel15.Style.GradientAngle = 90
        Me.TabControlPanel15.TabIndex = 15
        Me.TabControlPanel15.TabItem = Me.tiRigBuilder
        '
        'pnlRigs
        '
        Me.pnlRigs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlRigs.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlRigs.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlRigs.Controls.Add(Me.adtRigs)
        Me.pnlRigs.Controls.Add(Me.ExpandableSplitter1)
        Me.pnlRigs.Controls.Add(Me.adtRigBuildList)
        Me.pnlRigs.Location = New System.Drawing.Point(4, 115)
        Me.pnlRigs.Name = "pnlRigs"
        Me.pnlRigs.Size = New System.Drawing.Size(1276, 530)
        Me.pnlRigs.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlRigs.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlRigs.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlRigs.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlRigs.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlRigs.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlRigs.Style.GradientAngle = 90
        Me.pnlRigs.TabIndex = 71
        Me.pnlRigs.Text = "PanelEx3"
        '
        'adtRigs
        '
        Me.adtRigs.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtRigs.AllowDrop = True
        Me.adtRigs.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtRigs.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtRigs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtRigs.Columns.Add(Me.colRigListType)
        Me.adtRigs.Columns.Add(Me.colRigListQuantity)
        Me.adtRigs.Columns.Add(Me.colRigListRigPrice)
        Me.adtRigs.Columns.Add(Me.colRigListSalvagePrice)
        Me.adtRigs.Columns.Add(Me.colRigListBuildBenefit)
        Me.adtRigs.Columns.Add(Me.colRigListRigValue)
        Me.adtRigs.Columns.Add(Me.colRigListSalvageValue)
        Me.adtRigs.Columns.Add(Me.colRigListTotalBuildBenefit)
        Me.adtRigs.Columns.Add(Me.colRigListMargin)
        Me.adtRigs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtRigs.DragDropEnabled = False
        Me.adtRigs.DragDropNodeCopyEnabled = False
        Me.adtRigs.ExpandWidth = 0
        Me.adtRigs.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtRigs.Location = New System.Drawing.Point(0, 0)
        Me.adtRigs.Name = "adtRigs"
        Me.adtRigs.NodesConnector = Me.NodeConnector14
        Me.adtRigs.NodeStyle = Me.ElementStyle11
        Me.adtRigs.PathSeparator = ";"
        Me.adtRigs.Size = New System.Drawing.Size(1276, 259)
        Me.adtRigs.Styles.Add(Me.ElementStyle11)
        Me.adtRigs.TabIndex = 66
        Me.adtRigs.Text = "AdvTree1"
        '
        'colRigListType
        '
        Me.colRigListType.DisplayIndex = 1
        Me.colRigListType.Name = "colRigListType"
        Me.colRigListType.Text = "Rig Type"
        Me.colRigListType.Width.Absolute = 200
        '
        'colRigListQuantity
        '
        Me.colRigListQuantity.DisplayIndex = 2
        Me.colRigListQuantity.Name = "colRigListQuantity"
        Me.colRigListQuantity.Text = "Quantity"
        Me.colRigListQuantity.Width.Absolute = 100
        '
        'colRigListRigPrice
        '
        Me.colRigListRigPrice.DisplayIndex = 3
        Me.colRigListRigPrice.Name = "colRigListRigPrice"
        Me.colRigListRigPrice.Text = "Rig Price"
        Me.colRigListRigPrice.Width.Absolute = 120
        '
        'colRigListSalvagePrice
        '
        Me.colRigListSalvagePrice.DisplayIndex = 4
        Me.colRigListSalvagePrice.Name = "colRigListSalvagePrice"
        Me.colRigListSalvagePrice.Text = "Salvage Price"
        Me.colRigListSalvagePrice.Width.Absolute = 120
        '
        'colRigListBuildBenefit
        '
        Me.colRigListBuildBenefit.DisplayIndex = 5
        Me.colRigListBuildBenefit.Name = "colRigListBuildBenefit"
        Me.colRigListBuildBenefit.Text = "Build Benefit"
        Me.colRigListBuildBenefit.Width.Absolute = 120
        '
        'colRigListRigValue
        '
        Me.colRigListRigValue.DisplayIndex = 6
        Me.colRigListRigValue.Name = "colRigListRigValue"
        Me.colRigListRigValue.Text = "Total Rig Value"
        Me.colRigListRigValue.Width.Absolute = 120
        '
        'colRigListSalvageValue
        '
        Me.colRigListSalvageValue.DisplayIndex = 7
        Me.colRigListSalvageValue.Name = "colRigListSalvageValue"
        Me.colRigListSalvageValue.Text = "Total Salvage Value"
        Me.colRigListSalvageValue.Width.Absolute = 120
        '
        'colRigListTotalBuildBenefit
        '
        Me.colRigListTotalBuildBenefit.DisplayIndex = 8
        Me.colRigListTotalBuildBenefit.Name = "colRigListTotalBuildBenefit"
        Me.colRigListTotalBuildBenefit.Text = "Total Build Benefit"
        Me.colRigListTotalBuildBenefit.Width.Absolute = 120
        '
        'colRigListMargin
        '
        Me.colRigListMargin.DisplayIndex = 9
        Me.colRigListMargin.Name = "colRigListMargin"
        Me.colRigListMargin.Text = "% Margin"
        Me.colRigListMargin.Width.Absolute = 100
        '
        'NodeConnector14
        '
        Me.NodeConnector14.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle11
        '
        Me.ElementStyle11.Class = ""
        Me.ElementStyle11.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle11.Name = "ElementStyle11"
        Me.ElementStyle11.TextColor = System.Drawing.SystemColors.ControlText
        '
        'ExpandableSplitter1
        '
        Me.ExpandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.ExpandableSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom
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
        Me.ExpandableSplitter1.Location = New System.Drawing.Point(0, 259)
        Me.ExpandableSplitter1.Name = "ExpandableSplitter1"
        Me.ExpandableSplitter1.Size = New System.Drawing.Size(1276, 6)
        Me.ExpandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007
        Me.ExpandableSplitter1.TabIndex = 65
        Me.ExpandableSplitter1.TabStop = False
        '
        'adtRigBuildList
        '
        Me.adtRigBuildList.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtRigBuildList.AllowDrop = True
        Me.adtRigBuildList.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtRigBuildList.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtRigBuildList.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtRigBuildList.Columns.Add(Me.colRigBuildType)
        Me.adtRigBuildList.Columns.Add(Me.colRigBuildQuantity)
        Me.adtRigBuildList.Columns.Add(Me.colRigBuidRigPrice)
        Me.adtRigBuildList.Columns.Add(Me.colRigBuildSalvagePrice)
        Me.adtRigBuildList.Columns.Add(Me.colRigBuildBenefit)
        Me.adtRigBuildList.Columns.Add(Me.colRigBuildRigValue)
        Me.adtRigBuildList.Columns.Add(Me.colRigBuildSalvageValue)
        Me.adtRigBuildList.Columns.Add(Me.colRigBuildTotalBenefit)
        Me.adtRigBuildList.Columns.Add(Me.colRigBuildMargin)
        Me.adtRigBuildList.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.adtRigBuildList.DragDropEnabled = False
        Me.adtRigBuildList.DragDropNodeCopyEnabled = False
        Me.adtRigBuildList.ExpandWidth = 0
        Me.adtRigBuildList.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtRigBuildList.Location = New System.Drawing.Point(0, 265)
        Me.adtRigBuildList.Name = "adtRigBuildList"
        Me.adtRigBuildList.NodesConnector = Me.NodeConnector13
        Me.adtRigBuildList.NodeStyle = Me.ElementStyle10
        Me.adtRigBuildList.PathSeparator = ";"
        Me.adtRigBuildList.Size = New System.Drawing.Size(1276, 265)
        Me.adtRigBuildList.Styles.Add(Me.ElementStyle10)
        Me.adtRigBuildList.TabIndex = 64
        Me.adtRigBuildList.Text = "AdvTree1"
        '
        'colRigBuildType
        '
        Me.colRigBuildType.DisplayIndex = 1
        Me.colRigBuildType.Name = "colRigBuildType"
        Me.colRigBuildType.Text = "Rig Type"
        Me.colRigBuildType.Width.Absolute = 200
        '
        'colRigBuildQuantity
        '
        Me.colRigBuildQuantity.DisplayIndex = 2
        Me.colRigBuildQuantity.Name = "colRigBuildQuantity"
        Me.colRigBuildQuantity.Text = "Quantity"
        Me.colRigBuildQuantity.Width.Absolute = 100
        '
        'colRigBuidRigPrice
        '
        Me.colRigBuidRigPrice.DisplayIndex = 3
        Me.colRigBuidRigPrice.Name = "colRigBuidRigPrice"
        Me.colRigBuidRigPrice.Text = "Rig Price"
        Me.colRigBuidRigPrice.Width.Absolute = 120
        '
        'colRigBuildSalvagePrice
        '
        Me.colRigBuildSalvagePrice.DisplayIndex = 4
        Me.colRigBuildSalvagePrice.Name = "colRigBuildSalvagePrice"
        Me.colRigBuildSalvagePrice.Text = "Salvage Price"
        Me.colRigBuildSalvagePrice.Width.Absolute = 120
        '
        'colRigBuildBenefit
        '
        Me.colRigBuildBenefit.DisplayIndex = 5
        Me.colRigBuildBenefit.Name = "colRigBuildBenefit"
        Me.colRigBuildBenefit.Text = "Build Benefit"
        Me.colRigBuildBenefit.Width.Absolute = 120
        '
        'colRigBuildRigValue
        '
        Me.colRigBuildRigValue.DisplayIndex = 6
        Me.colRigBuildRigValue.Name = "colRigBuildRigValue"
        Me.colRigBuildRigValue.Text = "Total Rig Value"
        Me.colRigBuildRigValue.Width.Absolute = 120
        '
        'colRigBuildSalvageValue
        '
        Me.colRigBuildSalvageValue.DisplayIndex = 7
        Me.colRigBuildSalvageValue.Name = "colRigBuildSalvageValue"
        Me.colRigBuildSalvageValue.Text = "Total Salvage Value"
        Me.colRigBuildSalvageValue.Width.Absolute = 120
        '
        'colRigBuildTotalBenefit
        '
        Me.colRigBuildTotalBenefit.DisplayIndex = 8
        Me.colRigBuildTotalBenefit.Name = "colRigBuildTotalBenefit"
        Me.colRigBuildTotalBenefit.Text = "Total Build Benefit"
        Me.colRigBuildTotalBenefit.Width.Absolute = 120
        '
        'colRigBuildMargin
        '
        Me.colRigBuildMargin.DisplayIndex = 9
        Me.colRigBuildMargin.Name = "colRigBuildMargin"
        Me.colRigBuildMargin.Text = "% Margin"
        Me.colRigBuildMargin.Width.Absolute = 100
        '
        'NodeConnector13
        '
        Me.NodeConnector13.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle10
        '
        Me.ElementStyle10.Class = ""
        Me.ElementStyle10.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle10.Name = "ElementStyle10"
        Me.ElementStyle10.TextColor = System.Drawing.SystemColors.ControlText
        '
        'PSCRigOwners
        '
        Me.PSCRigOwners.AllowMultipleSelections = True
        Me.PSCRigOwners.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PSCRigOwners.ListType = EveHQ.Prism.PrismSelectionType.AllOwners
        Me.PSCRigOwners.Location = New System.Drawing.Point(10, 8)
        Me.PSCRigOwners.Name = "PSCRigOwners"
        Me.PSCRigOwners.Size = New System.Drawing.Size(278, 21)
        Me.PSCRigOwners.TabIndex = 70
        '
        'lblTotalRigMargin
        '
        Me.lblTotalRigMargin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigMargin.AutoSize = True
        Me.lblTotalRigMargin.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalRigMargin.Location = New System.Drawing.Point(422, 93)
        Me.lblTotalRigMargin.Name = "lblTotalRigMargin"
        Me.lblTotalRigMargin.Size = New System.Drawing.Size(43, 13)
        Me.lblTotalRigMargin.TabIndex = 69
        Me.lblTotalRigMargin.Text = "Margin:"
        '
        'lblTotalRigProfit
        '
        Me.lblTotalRigProfit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigProfit.AutoSize = True
        Me.lblTotalRigProfit.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalRigProfit.Location = New System.Drawing.Point(222, 93)
        Me.lblTotalRigProfit.Name = "lblTotalRigProfit"
        Me.lblTotalRigProfit.Size = New System.Drawing.Size(64, 13)
        Me.lblTotalRigProfit.TabIndex = 68
        Me.lblTotalRigProfit.Text = "Total Profit:"
        '
        'lblTotalRigSalePrice
        '
        Me.lblTotalRigSalePrice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRigSalePrice.AutoSize = True
        Me.lblTotalRigSalePrice.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalRigSalePrice.Location = New System.Drawing.Point(13, 93)
        Me.lblTotalRigSalePrice.Name = "lblTotalRigSalePrice"
        Me.lblTotalRigSalePrice.Size = New System.Drawing.Size(84, 13)
        Me.lblTotalRigSalePrice.TabIndex = 67
        Me.lblTotalRigSalePrice.Text = "Total Sale Price:"
        '
        'gpAutoRig
        '
        Me.gpAutoRig.BackColor = System.Drawing.Color.Transparent
        Me.gpAutoRig.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpAutoRig.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.gpAutoRig.Controls.Add(Me.btnAutoRig)
        Me.gpAutoRig.Controls.Add(Me.chkRigMargin)
        Me.gpAutoRig.Controls.Add(Me.chkRigSalePrice)
        Me.gpAutoRig.Controls.Add(Me.chkTotalProfit)
        Me.gpAutoRig.Controls.Add(Me.chkTotalSalePrice)
        Me.gpAutoRig.Controls.Add(Me.chkRigProfit)
        Me.gpAutoRig.Location = New System.Drawing.Point(318, 8)
        Me.gpAutoRig.Name = "gpAutoRig"
        Me.gpAutoRig.Size = New System.Drawing.Size(515, 76)
        '
        '
        '
        Me.gpAutoRig.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpAutoRig.Style.BackColorGradientAngle = 90
        Me.gpAutoRig.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpAutoRig.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpAutoRig.Style.BorderBottomWidth = 1
        Me.gpAutoRig.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpAutoRig.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpAutoRig.Style.BorderLeftWidth = 1
        Me.gpAutoRig.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpAutoRig.Style.BorderRightWidth = 1
        Me.gpAutoRig.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpAutoRig.Style.BorderTopWidth = 1
        Me.gpAutoRig.Style.Class = ""
        Me.gpAutoRig.Style.CornerDiameter = 4
        Me.gpAutoRig.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpAutoRig.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpAutoRig.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpAutoRig.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpAutoRig.StyleMouseDown.Class = ""
        Me.gpAutoRig.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpAutoRig.StyleMouseOver.Class = ""
        Me.gpAutoRig.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpAutoRig.TabIndex = 63
        Me.gpAutoRig.Text = "Automatic Rig Availability Options"
        '
        'btnAutoRig
        '
        Me.btnAutoRig.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAutoRig.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAutoRig.Location = New System.Drawing.Point(350, 6)
        Me.btnAutoRig.Name = "btnAutoRig"
        Me.btnAutoRig.Size = New System.Drawing.Size(150, 23)
        Me.btnAutoRig.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAutoRig.TabIndex = 64
        Me.btnAutoRig.Text = "Auto Rig Availability"
        '
        'chkRigMargin
        '
        Me.chkRigMargin.AutoSize = True
        Me.chkRigMargin.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.chkRigMargin.BackgroundStyle.Class = ""
        Me.chkRigMargin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkRigMargin.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.chkRigMargin.Checked = True
        Me.chkRigMargin.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRigMargin.CheckValue = "Y"
        Me.chkRigMargin.Location = New System.Drawing.Point(246, 6)
        Me.chkRigMargin.Name = "chkRigMargin"
        Me.chkRigMargin.Size = New System.Drawing.Size(74, 16)
        Me.chkRigMargin.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkRigMargin.TabIndex = 69
        Me.chkRigMargin.Text = "Rig Margin"
        '
        'chkRigSalePrice
        '
        Me.chkRigSalePrice.AutoSize = True
        Me.chkRigSalePrice.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.chkRigSalePrice.BackgroundStyle.Class = ""
        Me.chkRigSalePrice.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkRigSalePrice.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.chkRigSalePrice.Location = New System.Drawing.Point(11, 6)
        Me.chkRigSalePrice.Name = "chkRigSalePrice"
        Me.chkRigSalePrice.Size = New System.Drawing.Size(88, 16)
        Me.chkRigSalePrice.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkRigSalePrice.TabIndex = 65
        Me.chkRigSalePrice.Text = "Rig Sale Price"
        '
        'chkTotalProfit
        '
        Me.chkTotalProfit.AutoSize = True
        Me.chkTotalProfit.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.chkTotalProfit.BackgroundStyle.Class = ""
        Me.chkTotalProfit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkTotalProfit.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.chkTotalProfit.Location = New System.Drawing.Point(136, 27)
        Me.chkTotalProfit.Name = "chkTotalProfit"
        Me.chkTotalProfit.Size = New System.Drawing.Size(76, 16)
        Me.chkTotalProfit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkTotalProfit.TabIndex = 68
        Me.chkTotalProfit.Text = "Total Profit"
        '
        'chkTotalSalePrice
        '
        Me.chkTotalSalePrice.AutoSize = True
        Me.chkTotalSalePrice.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.chkTotalSalePrice.BackgroundStyle.Class = ""
        Me.chkTotalSalePrice.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkTotalSalePrice.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.chkTotalSalePrice.Location = New System.Drawing.Point(11, 27)
        Me.chkTotalSalePrice.Name = "chkTotalSalePrice"
        Me.chkTotalSalePrice.Size = New System.Drawing.Size(97, 16)
        Me.chkTotalSalePrice.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkTotalSalePrice.TabIndex = 66
        Me.chkTotalSalePrice.Text = "Total Sale Price"
        '
        'chkRigProfit
        '
        Me.chkRigProfit.AutoSize = True
        Me.chkRigProfit.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.chkRigProfit.BackgroundStyle.Class = ""
        Me.chkRigProfit.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkRigProfit.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.chkRigProfit.Location = New System.Drawing.Point(136, 6)
        Me.chkRigProfit.Name = "chkRigProfit"
        Me.chkRigProfit.Size = New System.Drawing.Size(67, 16)
        Me.chkRigProfit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkRigProfit.TabIndex = 67
        Me.chkRigProfit.Text = "Rig Profit"
        '
        'btnBuildRigs
        '
        Me.btnBuildRigs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnBuildRigs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnBuildRigs.Location = New System.Drawing.Point(12, 61)
        Me.btnBuildRigs.Name = "btnBuildRigs"
        Me.btnBuildRigs.Size = New System.Drawing.Size(150, 23)
        Me.btnBuildRigs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnBuildRigs.TabIndex = 62
        Me.btnBuildRigs.Text = "Display Rig Availability"
        '
        'btnExportRigBuildList
        '
        Me.btnExportRigBuildList.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnExportRigBuildList.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportRigBuildList.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnExportRigBuildList.Location = New System.Drawing.Point(1173, 37)
        Me.btnExportRigBuildList.Name = "btnExportRigBuildList"
        Me.btnExportRigBuildList.Size = New System.Drawing.Size(100, 23)
        Me.btnExportRigBuildList.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnExportRigBuildList.TabIndex = 61
        Me.btnExportRigBuildList.Text = "Export Build List"
        '
        'btnExportRigList
        '
        Me.btnExportRigList.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnExportRigList.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportRigList.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnExportRigList.Location = New System.Drawing.Point(1172, 10)
        Me.btnExportRigList.Name = "btnExportRigList"
        Me.btnExportRigList.Size = New System.Drawing.Size(100, 23)
        Me.btnExportRigList.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnExportRigList.TabIndex = 60
        Me.btnExportRigList.Text = "Export Rig List"
        '
        'nudRigMELevel
        '
        Me.nudRigMELevel.Location = New System.Drawing.Point(92, 34)
        Me.nudRigMELevel.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudRigMELevel.Name = "nudRigMELevel"
        Me.nudRigMELevel.Size = New System.Drawing.Size(70, 21)
        Me.nudRigMELevel.TabIndex = 56
        '
        'lblRigMELevel
        '
        Me.lblRigMELevel.AutoSize = True
        Me.lblRigMELevel.BackColor = System.Drawing.Color.Transparent
        Me.lblRigMELevel.Location = New System.Drawing.Point(12, 36)
        Me.lblRigMELevel.Name = "lblRigMELevel"
        Me.lblRigMELevel.Size = New System.Drawing.Size(71, 13)
        Me.lblRigMELevel.TabIndex = 55
        Me.lblRigMELevel.Text = "Rig ME Level:"
        '
        'tiRigBuilder
        '
        Me.tiRigBuilder.AttachedControl = Me.TabControlPanel15
        Me.tiRigBuilder.Name = "tiRigBuilder"
        Me.tiRigBuilder.Text = "Rig Builder"
        '
        'TabControlPanel4
        '
        Me.TabControlPanel4.Controls.Add(Me.pnlSellOrders)
        Me.TabControlPanel4.Controls.Add(Me.splitterMarketOrders)
        Me.TabControlPanel4.Controls.Add(Me.pnlBuyOrders)
        Me.TabControlPanel4.Controls.Add(Me.pnlOrderStats)
        Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel4.Name = "TabControlPanel4"
        Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel4.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel4.Style.GradientAngle = 90
        Me.TabControlPanel4.TabIndex = 4
        Me.TabControlPanel4.TabItem = Me.tiMarketOrders
        '
        'pnlSellOrders
        '
        Me.pnlSellOrders.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlSellOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlSellOrders.Controls.Add(Me.cboOrdersOwner)
        Me.pnlSellOrders.Controls.Add(Me.lblOrdersOwner)
        Me.pnlSellOrders.Controls.Add(Me.adtSellOrders)
        Me.pnlSellOrders.Controls.Add(Me.lblSellOrders)
        Me.pnlSellOrders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSellOrders.Location = New System.Drawing.Point(1, 1)
        Me.pnlSellOrders.Name = "pnlSellOrders"
        Me.pnlSellOrders.Size = New System.Drawing.Size(1282, 283)
        Me.pnlSellOrders.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlSellOrders.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlSellOrders.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlSellOrders.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlSellOrders.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlSellOrders.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlSellOrders.Style.GradientAngle = 90
        Me.pnlSellOrders.TabIndex = 3
        '
        'cboOrdersOwner
        '
        Me.cboOrdersOwner.DisplayMember = "Text"
        Me.cboOrdersOwner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboOrdersOwner.FormattingEnabled = True
        Me.cboOrdersOwner.ItemHeight = 15
        Me.cboOrdersOwner.Location = New System.Drawing.Point(55, 5)
        Me.cboOrdersOwner.Name = "cboOrdersOwner"
        Me.cboOrdersOwner.Size = New System.Drawing.Size(210, 21)
        Me.cboOrdersOwner.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboOrdersOwner.TabIndex = 30
        '
        'lblOrdersOwner
        '
        Me.lblOrdersOwner.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOrdersOwner.AutoSize = True
        Me.lblOrdersOwner.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrdersOwner.Location = New System.Drawing.Point(3, 9)
        Me.lblOrdersOwner.Name = "lblOrdersOwner"
        Me.lblOrdersOwner.Size = New System.Drawing.Size(43, 13)
        Me.lblOrdersOwner.TabIndex = 29
        Me.lblOrdersOwner.Text = "Owner:"
        '
        'adtSellOrders
        '
        Me.adtSellOrders.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtSellOrders.AllowDrop = True
        Me.adtSellOrders.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtSellOrders.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtSellOrders.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtSellOrders.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtSellOrders.Columns.Add(Me.colSellType)
        Me.adtSellOrders.Columns.Add(Me.colSellQty)
        Me.adtSellOrders.Columns.Add(Me.colSellPrice)
        Me.adtSellOrders.Columns.Add(Me.colSellLocation)
        Me.adtSellOrders.Columns.Add(Me.colSellExpires)
        Me.adtSellOrders.DragDropEnabled = False
        Me.adtSellOrders.DragDropNodeCopyEnabled = False
        Me.adtSellOrders.ExpandWidth = 0
        Me.adtSellOrders.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtSellOrders.Location = New System.Drawing.Point(6, 50)
        Me.adtSellOrders.Name = "adtSellOrders"
        Me.adtSellOrders.NodesConnector = Me.NodeConnector9
        Me.adtSellOrders.NodeStyle = Me.ElementStyle7
        Me.adtSellOrders.PathSeparator = ";"
        Me.adtSellOrders.Size = New System.Drawing.Size(1265, 227)
        Me.adtSellOrders.Styles.Add(Me.ElementStyle7)
        Me.adtSellOrders.TabIndex = 28
        Me.adtSellOrders.Text = "AdvTree1"
        '
        'colSellType
        '
        Me.colSellType.DisplayIndex = 1
        Me.colSellType.Name = "colSellType"
        Me.colSellType.Text = "Type"
        Me.colSellType.Width.Absolute = 250
        '
        'colSellQty
        '
        Me.colSellQty.DisplayIndex = 2
        Me.colSellQty.Name = "colSellQty"
        Me.colSellQty.Text = "Quantity"
        Me.colSellQty.Width.Absolute = 125
        '
        'colSellPrice
        '
        Me.colSellPrice.DisplayIndex = 3
        Me.colSellPrice.Name = "colSellPrice"
        Me.colSellPrice.Text = "Price"
        Me.colSellPrice.Width.Absolute = 125
        '
        'colSellLocation
        '
        Me.colSellLocation.DisplayIndex = 4
        Me.colSellLocation.Name = "colSellLocation"
        Me.colSellLocation.Text = "Location"
        Me.colSellLocation.Width.Absolute = 300
        '
        'colSellExpires
        '
        Me.colSellExpires.DisplayIndex = 5
        Me.colSellExpires.Name = "colSellExpires"
        Me.colSellExpires.Text = "Expires In"
        Me.colSellExpires.Width.Absolute = 125
        '
        'NodeConnector9
        '
        Me.NodeConnector9.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle7
        '
        Me.ElementStyle7.Class = ""
        Me.ElementStyle7.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle7.Name = "ElementStyle7"
        Me.ElementStyle7.TextColor = System.Drawing.SystemColors.ControlText
        '
        'splitterMarketOrders
        '
        Me.splitterMarketOrders.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.splitterMarketOrders.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.splitterMarketOrders.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.splitterMarketOrders.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.splitterMarketOrders.ExpandableControl = Me.pnlBuyOrders
        Me.splitterMarketOrders.ExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.splitterMarketOrders.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.splitterMarketOrders.ExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.splitterMarketOrders.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.splitterMarketOrders.GripDarkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.splitterMarketOrders.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.splitterMarketOrders.GripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.splitterMarketOrders.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.splitterMarketOrders.HotBackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(61, Byte), Integer))
        Me.splitterMarketOrders.HotBackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.splitterMarketOrders.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2
        Me.splitterMarketOrders.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground
        Me.splitterMarketOrders.HotExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.splitterMarketOrders.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.splitterMarketOrders.HotExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.splitterMarketOrders.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.splitterMarketOrders.HotGripDarkColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.splitterMarketOrders.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.splitterMarketOrders.HotGripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.splitterMarketOrders.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.splitterMarketOrders.Location = New System.Drawing.Point(1, 284)
        Me.splitterMarketOrders.Name = "splitterMarketOrders"
        Me.splitterMarketOrders.Size = New System.Drawing.Size(1282, 6)
        Me.splitterMarketOrders.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007
        Me.splitterMarketOrders.TabIndex = 2
        Me.splitterMarketOrders.TabStop = False
        '
        'pnlBuyOrders
        '
        Me.pnlBuyOrders.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlBuyOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlBuyOrders.Controls.Add(Me.adtBuyOrders)
        Me.pnlBuyOrders.Controls.Add(Me.lblBuyOrders)
        Me.pnlBuyOrders.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBuyOrders.Location = New System.Drawing.Point(1, 290)
        Me.pnlBuyOrders.Name = "pnlBuyOrders"
        Me.pnlBuyOrders.Size = New System.Drawing.Size(1282, 263)
        Me.pnlBuyOrders.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlBuyOrders.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlBuyOrders.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlBuyOrders.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlBuyOrders.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlBuyOrders.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlBuyOrders.Style.GradientAngle = 90
        Me.pnlBuyOrders.TabIndex = 1
        '
        'adtBuyOrders
        '
        Me.adtBuyOrders.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtBuyOrders.AllowDrop = True
        Me.adtBuyOrders.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtBuyOrders.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtBuyOrders.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtBuyOrders.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtBuyOrders.Columns.Add(Me.colBuyType)
        Me.adtBuyOrders.Columns.Add(Me.colBuyQty)
        Me.adtBuyOrders.Columns.Add(Me.colBuyPrice)
        Me.adtBuyOrders.Columns.Add(Me.colBuyLocation)
        Me.adtBuyOrders.Columns.Add(Me.colBuyRange)
        Me.adtBuyOrders.Columns.Add(Me.colBuyVolume)
        Me.adtBuyOrders.Columns.Add(Me.colBuyExpires)
        Me.adtBuyOrders.DragDropEnabled = False
        Me.adtBuyOrders.DragDropNodeCopyEnabled = False
        Me.adtBuyOrders.ExpandWidth = 0
        Me.adtBuyOrders.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtBuyOrders.Location = New System.Drawing.Point(6, 19)
        Me.adtBuyOrders.Name = "adtBuyOrders"
        Me.adtBuyOrders.NodesConnector = Me.NodeConnector8
        Me.adtBuyOrders.NodeStyle = Me.ElementStyle6
        Me.adtBuyOrders.PathSeparator = ";"
        Me.adtBuyOrders.Size = New System.Drawing.Size(1265, 238)
        Me.adtBuyOrders.Styles.Add(Me.ElementStyle6)
        Me.adtBuyOrders.TabIndex = 27
        Me.adtBuyOrders.Text = "AdvTree1"
        '
        'colBuyType
        '
        Me.colBuyType.DisplayIndex = 1
        Me.colBuyType.Name = "colBuyType"
        Me.colBuyType.Text = "Type"
        Me.colBuyType.Width.Absolute = 250
        '
        'colBuyQty
        '
        Me.colBuyQty.DisplayIndex = 2
        Me.colBuyQty.Name = "colBuyQty"
        Me.colBuyQty.Text = "Quantity"
        Me.colBuyQty.Width.Absolute = 125
        '
        'colBuyPrice
        '
        Me.colBuyPrice.DisplayIndex = 3
        Me.colBuyPrice.Name = "colBuyPrice"
        Me.colBuyPrice.Text = "Price"
        Me.colBuyPrice.Width.Absolute = 125
        '
        'colBuyLocation
        '
        Me.colBuyLocation.DisplayIndex = 4
        Me.colBuyLocation.Name = "colBuyLocation"
        Me.colBuyLocation.Text = "Location"
        Me.colBuyLocation.Width.Absolute = 300
        '
        'colBuyRange
        '
        Me.colBuyRange.DisplayIndex = 5
        Me.colBuyRange.Name = "colBuyRange"
        Me.colBuyRange.Text = "Range"
        Me.colBuyRange.Width.Absolute = 50
        '
        'colBuyVolume
        '
        Me.colBuyVolume.DisplayIndex = 6
        Me.colBuyVolume.Name = "colBuyVolume"
        Me.colBuyVolume.Text = "Min Volume"
        Me.colBuyVolume.Width.Absolute = 100
        '
        'colBuyExpires
        '
        Me.colBuyExpires.DisplayIndex = 7
        Me.colBuyExpires.Name = "colBuyExpires"
        Me.colBuyExpires.Text = "Expires In"
        Me.colBuyExpires.Width.Absolute = 125
        '
        'NodeConnector8
        '
        Me.NodeConnector8.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle6
        '
        Me.ElementStyle6.Class = ""
        Me.ElementStyle6.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle6.Name = "ElementStyle6"
        Me.ElementStyle6.TextColor = System.Drawing.SystemColors.ControlText
        '
        'pnlOrderStats
        '
        Me.pnlOrderStats.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlOrderStats.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlOrderStats.Controls.Add(Me.lblOrdersLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblRemoteRange)
        Me.pnlOrderStats.Controls.Add(Me.lblEscrowLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblModRange)
        Me.pnlOrderStats.Controls.Add(Me.lblBrokerFeeLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblBidRange)
        Me.pnlOrderStats.Controls.Add(Me.lblTransTaxLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblAskRange)
        Me.pnlOrderStats.Controls.Add(Me.lblSellTotalLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblRemoteRangeLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblBuyTotalLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblModRangeLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblOrders)
        Me.pnlOrderStats.Controls.Add(Me.lblBidRangeLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblEscrow)
        Me.pnlOrderStats.Controls.Add(Me.lblAskRangeLbl)
        Me.pnlOrderStats.Controls.Add(Me.lblBrokerFee)
        Me.pnlOrderStats.Controls.Add(Me.lblBuyTotal)
        Me.pnlOrderStats.Controls.Add(Me.lblTransTax)
        Me.pnlOrderStats.Controls.Add(Me.lblSellTotal)
        Me.pnlOrderStats.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlOrderStats.Location = New System.Drawing.Point(1, 553)
        Me.pnlOrderStats.Name = "pnlOrderStats"
        Me.pnlOrderStats.Size = New System.Drawing.Size(1282, 96)
        Me.pnlOrderStats.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlOrderStats.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlOrderStats.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlOrderStats.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlOrderStats.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlOrderStats.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlOrderStats.Style.GradientAngle = 90
        Me.pnlOrderStats.TabIndex = 0
        '
        'tiMarketOrders
        '
        Me.tiMarketOrders.AttachedControl = Me.TabControlPanel4
        Me.tiMarketOrders.Name = "tiMarketOrders"
        Me.tiMarketOrders.Text = "Market Orders"
        '
        'TabControlPanel14
        '
        Me.TabControlPanel14.Controls.Add(Me.adtContracts)
        Me.TabControlPanel14.Controls.Add(Me.cboContractOwner)
        Me.TabControlPanel14.Controls.Add(Me.lblContractOwner)
        Me.TabControlPanel14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel14.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel14.Name = "TabControlPanel14"
        Me.TabControlPanel14.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel14.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel14.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel14.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel14.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel14.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel14.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel14.Style.GradientAngle = 90
        Me.TabControlPanel14.TabIndex = 14
        Me.TabControlPanel14.TabItem = Me.tiContracts
        '
        'adtContracts
        '
        Me.adtContracts.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtContracts.AllowDrop = True
        Me.adtContracts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtContracts.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtContracts.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtContracts.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtContracts.Columns.Add(Me.colContractTitle)
        Me.adtContracts.Columns.Add(Me.colContractLocation)
        Me.adtContracts.Columns.Add(Me.colContractTransaction)
        Me.adtContracts.Columns.Add(Me.colContractType)
        Me.adtContracts.Columns.Add(Me.colContractStatus)
        Me.adtContracts.Columns.Add(Me.colContractDateIssued)
        Me.adtContracts.Columns.Add(Me.colContractDateExpired)
        Me.adtContracts.Columns.Add(Me.colContractPrice)
        Me.adtContracts.Columns.Add(Me.colContractVolume)
        Me.adtContracts.DragDropEnabled = False
        Me.adtContracts.DragDropNodeCopyEnabled = False
        Me.adtContracts.DropAsChildOffset = 0
        Me.adtContracts.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtContracts.Location = New System.Drawing.Point(12, 33)
        Me.adtContracts.Name = "adtContracts"
        Me.adtContracts.NodesConnector = Me.NodeConnector6
        Me.adtContracts.NodeStyle = Me.ElementStyle9
        Me.adtContracts.PathSeparator = ";"
        Me.adtContracts.Size = New System.Drawing.Size(1260, 611)
        Me.adtContracts.Styles.Add(Me.ElementStyle9)
        Me.adtContracts.TabIndex = 33
        Me.adtContracts.Text = "AdvTree1"
        '
        'colContractTitle
        '
        Me.colContractTitle.DisplayIndex = 1
        Me.colContractTitle.Name = "colContractTitle"
        Me.colContractTitle.Text = "Contract Title (or ID)"
        Me.colContractTitle.Width.Absolute = 250
        '
        'colContractLocation
        '
        Me.colContractLocation.DisplayIndex = 2
        Me.colContractLocation.Name = "colContractLocation"
        Me.colContractLocation.Text = "Contract Location"
        Me.colContractLocation.Width.Absolute = 350
        '
        'colContractTransaction
        '
        Me.colContractTransaction.DisplayIndex = 3
        Me.colContractTransaction.Name = "colContractTransaction"
        Me.colContractTransaction.Text = "Transaction"
        Me.colContractTransaction.Width.Absolute = 80
        '
        'colContractType
        '
        Me.colContractType.DisplayIndex = 4
        Me.colContractType.Name = "colContractType"
        Me.colContractType.Text = "Type"
        Me.colContractType.Width.Absolute = 100
        '
        'colContractStatus
        '
        Me.colContractStatus.DisplayIndex = 5
        Me.colContractStatus.Name = "colContractStatus"
        Me.colContractStatus.Text = "Status"
        Me.colContractStatus.Width.Absolute = 150
        '
        'colContractDateIssued
        '
        Me.colContractDateIssued.DisplayIndex = 6
        Me.colContractDateIssued.Name = "colContractDateIssued"
        Me.colContractDateIssued.Text = "Date Issued"
        Me.colContractDateIssued.Width.Absolute = 125
        '
        'colContractDateExpired
        '
        Me.colContractDateExpired.DisplayIndex = 7
        Me.colContractDateExpired.Name = "colContractDateExpired"
        Me.colContractDateExpired.Text = "Expiry Date"
        Me.colContractDateExpired.Width.Absolute = 125
        '
        'colContractPrice
        '
        Me.colContractPrice.DisplayIndex = 8
        Me.colContractPrice.Name = "colContractPrice"
        Me.colContractPrice.Text = "Price/Reward"
        Me.colContractPrice.Width.Absolute = 125
        '
        'colContractVolume
        '
        Me.colContractVolume.DisplayIndex = 9
        Me.colContractVolume.Name = "colContractVolume"
        Me.colContractVolume.Text = "Volume"
        Me.colContractVolume.Width.Absolute = 100
        '
        'NodeConnector6
        '
        Me.NodeConnector6.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle9
        '
        Me.ElementStyle9.Class = ""
        Me.ElementStyle9.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle9.Name = "ElementStyle9"
        Me.ElementStyle9.TextColor = System.Drawing.SystemColors.ControlText
        '
        'cboContractOwner
        '
        Me.cboContractOwner.DisplayMember = "Text"
        Me.cboContractOwner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboContractOwner.FormattingEnabled = True
        Me.cboContractOwner.ItemHeight = 15
        Me.cboContractOwner.Location = New System.Drawing.Point(64, 6)
        Me.cboContractOwner.Name = "cboContractOwner"
        Me.cboContractOwner.Size = New System.Drawing.Size(210, 21)
        Me.cboContractOwner.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboContractOwner.TabIndex = 32
        '
        'lblContractOwner
        '
        Me.lblContractOwner.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblContractOwner.AutoSize = True
        Me.lblContractOwner.BackColor = System.Drawing.Color.Transparent
        Me.lblContractOwner.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContractOwner.Location = New System.Drawing.Point(12, 10)
        Me.lblContractOwner.Name = "lblContractOwner"
        Me.lblContractOwner.Size = New System.Drawing.Size(43, 13)
        Me.lblContractOwner.TabIndex = 31
        Me.lblContractOwner.Text = "Owner:"
        '
        'tiContracts
        '
        Me.tiContracts.AttachedControl = Me.TabControlPanel14
        Me.tiContracts.Name = "tiContracts"
        Me.tiContracts.Text = "Contracts"
        '
        'TabControlPanel8
        '
        Me.TabControlPanel8.Controls.Add(Me.lblRefineMode)
        Me.TabControlPanel8.Controls.Add(Me.chkFeesOnItems)
        Me.TabControlPanel8.Controls.Add(Me.TabControl1)
        Me.TabControlPanel8.Controls.Add(Me.lblPriceTotals)
        Me.TabControlPanel8.Controls.Add(Me.lblPilot)
        Me.TabControlPanel8.Controls.Add(Me.chkFeesOnRefine)
        Me.TabControlPanel8.Controls.Add(Me.cboRecyclePilots)
        Me.TabControlPanel8.Controls.Add(Me.lblTotalFees)
        Me.TabControlPanel8.Controls.Add(Me.chkPerfectRefine)
        Me.TabControlPanel8.Controls.Add(Me.lblTotalFeesLbl)
        Me.TabControlPanel8.Controls.Add(Me.lblBaseYieldLbl)
        Me.TabControlPanel8.Controls.Add(Me.nudTax)
        Me.TabControlPanel8.Controls.Add(Me.lblNetYieldLbl)
        Me.TabControlPanel8.Controls.Add(Me.nudBrokerFee)
        Me.TabControlPanel8.Controls.Add(Me.lblStandingsLbl)
        Me.TabControlPanel8.Controls.Add(Me.chkOverrideTax)
        Me.TabControlPanel8.Controls.Add(Me.lblStationTakeLbl)
        Me.TabControlPanel8.Controls.Add(Me.chkOverrideBrokerFee)
        Me.TabControlPanel8.Controls.Add(Me.lblStationTake)
        Me.TabControlPanel8.Controls.Add(Me.lblItems)
        Me.TabControlPanel8.Controls.Add(Me.lblStandings)
        Me.TabControlPanel8.Controls.Add(Me.lblVolume)
        Me.TabControlPanel8.Controls.Add(Me.lblNetYield)
        Me.TabControlPanel8.Controls.Add(Me.lblItemsLbl)
        Me.TabControlPanel8.Controls.Add(Me.lblBaseYield)
        Me.TabControlPanel8.Controls.Add(Me.lblVolumeLbl)
        Me.TabControlPanel8.Controls.Add(Me.lblStationLbl)
        Me.TabControlPanel8.Controls.Add(Me.cboRefineMode)
        Me.TabControlPanel8.Controls.Add(Me.lblStation)
        Me.TabControlPanel8.Controls.Add(Me.lblCorpLbl)
        Me.TabControlPanel8.Controls.Add(Me.chkOverrideStandings)
        Me.TabControlPanel8.Controls.Add(Me.lblCorp)
        Me.TabControlPanel8.Controls.Add(Me.chkOverrideBaseYield)
        Me.TabControlPanel8.Controls.Add(Me.nudBaseYield)
        Me.TabControlPanel8.Controls.Add(Me.nudStandings)
        Me.TabControlPanel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel8.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel8.Name = "TabControlPanel8"
        Me.TabControlPanel8.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel8.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel8.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel8.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel8.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel8.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel8.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel8.Style.GradientAngle = 90
        Me.TabControlPanel8.TabIndex = 8
        Me.TabControlPanel8.TabItem = Me.tiRecycler
        '
        'tiRecycler
        '
        Me.tiRecycler.AttachedControl = Me.TabControlPanel8
        Me.tiRecycler.Name = "tiRecycler"
        Me.tiRecycler.Text = "Recycler"
        '
        'TabControlPanel9
        '
        Me.TabControlPanel9.Controls.Add(Me.btnCopyListToClipboard)
        Me.TabControlPanel9.Controls.Add(Me.cboBPOwner)
        Me.TabControlPanel9.Controls.Add(Me.lblBPOwner)
        Me.TabControlPanel9.Controls.Add(Me.adtBlueprints)
        Me.TabControlPanel9.Controls.Add(Me.cboCategoryFilter)
        Me.TabControlPanel9.Controls.Add(Me.chkShowOwnedBPs)
        Me.TabControlPanel9.Controls.Add(Me.lblBPCatFilter)
        Me.TabControlPanel9.Controls.Add(Me.pbBPO)
        Me.TabControlPanel9.Controls.Add(Me.cboTypeFilter)
        Me.TabControlPanel9.Controls.Add(Me.pbBPC)
        Me.TabControlPanel9.Controls.Add(Me.lblTypeFilter)
        Me.TabControlPanel9.Controls.Add(Me.pbMissing)
        Me.TabControlPanel9.Controls.Add(Me.cboTechFilter)
        Me.TabControlPanel9.Controls.Add(Me.pbUnknown)
        Me.TabControlPanel9.Controls.Add(Me.lblTechFilter)
        Me.TabControlPanel9.Controls.Add(Me.pbExhausted)
        Me.TabControlPanel9.Controls.Add(Me.lblUserBP)
        Me.TabControlPanel9.Controls.Add(Me.pbUserBP)
        Me.TabControlPanel9.Controls.Add(Me.btnAddCustomBP)
        Me.TabControlPanel9.Controls.Add(Me.lblExhausted)
        Me.TabControlPanel9.Controls.Add(Me.btnBPCalc)
        Me.TabControlPanel9.Controls.Add(Me.lblUnknown)
        Me.TabControlPanel9.Controls.Add(Me.btnUpdateBPsFromAssets)
        Me.TabControlPanel9.Controls.Add(Me.lblMissing)
        Me.TabControlPanel9.Controls.Add(Me.btnGetBPJobInfo)
        Me.TabControlPanel9.Controls.Add(Me.lblBPC)
        Me.TabControlPanel9.Controls.Add(Me.lblBPSearch)
        Me.TabControlPanel9.Controls.Add(Me.lblBPO)
        Me.TabControlPanel9.Controls.Add(Me.txtBPSearch)
        Me.TabControlPanel9.Controls.Add(Me.btnResetBPSearch)
        Me.TabControlPanel9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel9.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel9.Name = "TabControlPanel9"
        Me.TabControlPanel9.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel9.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel9.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel9.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel9.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel9.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel9.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel9.Style.GradientAngle = 90
        Me.TabControlPanel9.TabIndex = 9
        Me.TabControlPanel9.TabItem = Me.tiBPManager
        '
        'cboBPOwner
        '
        Me.cboBPOwner.DisplayMember = "Text"
        Me.cboBPOwner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboBPOwner.FormattingEnabled = True
        Me.cboBPOwner.ItemHeight = 15
        Me.cboBPOwner.Location = New System.Drawing.Point(64, 2)
        Me.cboBPOwner.Name = "cboBPOwner"
        Me.cboBPOwner.Size = New System.Drawing.Size(210, 21)
        Me.cboBPOwner.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboBPOwner.TabIndex = 56
        '
        'lblBPOwner
        '
        Me.lblBPOwner.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBPOwner.AutoSize = True
        Me.lblBPOwner.BackColor = System.Drawing.Color.Transparent
        Me.lblBPOwner.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBPOwner.Location = New System.Drawing.Point(12, 6)
        Me.lblBPOwner.Name = "lblBPOwner"
        Me.lblBPOwner.Size = New System.Drawing.Size(43, 13)
        Me.lblBPOwner.TabIndex = 55
        Me.lblBPOwner.Text = "Owner:"
        '
        'adtBlueprints
        '
        Me.adtBlueprints.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtBlueprints.AllowDrop = True
        Me.adtBlueprints.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtBlueprints.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtBlueprints.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtBlueprints.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtBlueprints.Columns.Add(Me.colBPMBlueprint)
        Me.adtBlueprints.Columns.Add(Me.colBPMLocation)
        Me.adtBlueprints.Columns.Add(Me.colBPMLocation2)
        Me.adtBlueprints.Columns.Add(Me.colBPMTechLevel)
        Me.adtBlueprints.Columns.Add(Me.colBPMME)
        Me.adtBlueprints.Columns.Add(Me.colBPMPE)
        Me.adtBlueprints.Columns.Add(Me.colBPMRuns)
        Me.adtBlueprints.Columns.Add(Me.colBPMStatus)
        Me.adtBlueprints.ContextMenuStrip = Me.ctxBPManager
        Me.adtBlueprints.DragDropEnabled = False
        Me.adtBlueprints.DragDropNodeCopyEnabled = False
        Me.adtBlueprints.ExpandWidth = 0
        Me.adtBlueprints.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtBlueprints.Location = New System.Drawing.Point(10, 26)
        Me.adtBlueprints.MultiSelect = True
        Me.adtBlueprints.Name = "adtBlueprints"
        Me.adtBlueprints.NodesConnector = Me.NodeConnector5
        Me.adtBlueprints.NodeStyle = Me.BP
        Me.adtBlueprints.PathSeparator = ";"
        Me.adtBlueprints.Size = New System.Drawing.Size(1176, 618)
        Me.adtBlueprints.Styles.Add(Me.BP)
        Me.adtBlueprints.TabIndex = 54
        Me.adtBlueprints.Text = "AdvTree1"
        '
        'colBPMBlueprint
        '
        Me.colBPMBlueprint.DisplayIndex = 1
        Me.colBPMBlueprint.Name = "colBPMBlueprint"
        Me.colBPMBlueprint.Text = "Blueprint"
        Me.colBPMBlueprint.Width.Absolute = 250
        '
        'colBPMLocation
        '
        Me.colBPMLocation.DisplayIndex = 2
        Me.colBPMLocation.Name = "colBPMLocation"
        Me.colBPMLocation.Text = "Location"
        Me.colBPMLocation.Width.Absolute = 300
        '
        'colBPMLocation2
        '
        Me.colBPMLocation2.DisplayIndex = 3
        Me.colBPMLocation2.Name = "colBPMLocation2"
        Me.colBPMLocation2.Text = "Specific Location"
        Me.colBPMLocation2.Width.Absolute = 300
        '
        'colBPMTechLevel
        '
        Me.colBPMTechLevel.DisplayIndex = 4
        Me.colBPMTechLevel.Name = "colBPMTechLevel"
        Me.colBPMTechLevel.Text = "Tech"
        Me.colBPMTechLevel.Width.Absolute = 40
        '
        'colBPMME
        '
        Me.colBPMME.DisplayIndex = 5
        Me.colBPMME.Name = "colBPMME"
        Me.colBPMME.Text = "ME"
        Me.colBPMME.Width.Absolute = 40
        '
        'colBPMPE
        '
        Me.colBPMPE.DisplayIndex = 6
        Me.colBPMPE.Name = "colBPMPE"
        Me.colBPMPE.Text = "PE"
        Me.colBPMPE.Width.Absolute = 40
        '
        'colBPMRuns
        '
        Me.colBPMRuns.DisplayIndex = 7
        Me.colBPMRuns.EditorType = DevComponents.AdvTree.eCellEditorType.Custom
        Me.colBPMRuns.Name = "colBPMRuns"
        Me.colBPMRuns.Text = "Runs"
        Me.colBPMRuns.Width.Absolute = 75
        '
        'colBPMStatus
        '
        Me.colBPMStatus.DisplayIndex = 8
        Me.colBPMStatus.Name = "colBPMStatus"
        Me.colBPMStatus.Text = "Status"
        Me.colBPMStatus.Width.Absolute = 100
        '
        'NodeConnector5
        '
        Me.NodeConnector5.LineColor = System.Drawing.SystemColors.ControlText
        '
        'BP
        '
        Me.BP.BackColorGradientAngle = 90
        Me.BP.BackColorGradientType = DevComponents.DotNetBar.eGradientType.Radial
        Me.BP.Class = ""
        Me.BP.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.BP.Name = "BP"
        Me.BP.TextColor = System.Drawing.SystemColors.ControlText
        '
        'pbBPO
        '
        Me.pbBPO.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbBPO.BackColor = System.Drawing.Color.LightGreen
        Me.pbBPO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbBPO.Location = New System.Drawing.Point(1192, 499)
        Me.pbBPO.Name = "pbBPO"
        Me.pbBPO.Size = New System.Drawing.Size(16, 16)
        Me.pbBPO.TabIndex = 35
        Me.pbBPO.TabStop = False
        '
        'pbBPC
        '
        Me.pbBPC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbBPC.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbBPC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbBPC.Location = New System.Drawing.Point(1192, 521)
        Me.pbBPC.Name = "pbBPC"
        Me.pbBPC.Size = New System.Drawing.Size(16, 16)
        Me.pbBPC.TabIndex = 37
        Me.pbBPC.TabStop = False
        '
        'pbMissing
        '
        Me.pbMissing.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbMissing.BackColor = System.Drawing.Color.LightCoral
        Me.pbMissing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbMissing.Location = New System.Drawing.Point(1192, 565)
        Me.pbMissing.Name = "pbMissing"
        Me.pbMissing.Size = New System.Drawing.Size(16, 16)
        Me.pbMissing.TabIndex = 39
        Me.pbMissing.TabStop = False
        '
        'pbUnknown
        '
        Me.pbUnknown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbUnknown.BackColor = System.Drawing.Color.LightGray
        Me.pbUnknown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbUnknown.Location = New System.Drawing.Point(1192, 609)
        Me.pbUnknown.Name = "pbUnknown"
        Me.pbUnknown.Size = New System.Drawing.Size(16, 16)
        Me.pbUnknown.TabIndex = 41
        Me.pbUnknown.TabStop = False
        '
        'pbExhausted
        '
        Me.pbExhausted.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbExhausted.BackColor = System.Drawing.Color.Orange
        Me.pbExhausted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbExhausted.Location = New System.Drawing.Point(1192, 587)
        Me.pbExhausted.Name = "pbExhausted"
        Me.pbExhausted.Size = New System.Drawing.Size(16, 16)
        Me.pbExhausted.TabIndex = 43
        Me.pbExhausted.TabStop = False
        '
        'pbUserBP
        '
        Me.pbUserBP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbUserBP.BackColor = System.Drawing.Color.Yellow
        Me.pbUserBP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbUserBP.Location = New System.Drawing.Point(1192, 543)
        Me.pbUserBP.Name = "pbUserBP"
        Me.pbUserBP.Size = New System.Drawing.Size(16, 16)
        Me.pbUserBP.TabIndex = 46
        Me.pbUserBP.TabStop = False
        '
        'tiBPManager
        '
        Me.tiBPManager.AttachedControl = Me.TabControlPanel9
        Me.tiBPManager.Name = "tiBPManager"
        Me.tiBPManager.Text = "BP Manager"
        '
        'TabControlPanel2
        '
        Me.TabControlPanel2.Controls.Add(Me.PAC)
        Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel2.Name = "TabControlPanel2"
        Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel2.Size = New System.Drawing.Size(1284, 650)
        Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel2.Style.GradientAngle = 90
        Me.TabControlPanel2.TabIndex = 2
        Me.TabControlPanel2.TabItem = Me.tiAssets
        '
        'PAC
        '
        Me.PAC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PAC.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PAC.Location = New System.Drawing.Point(1, 1)
        Me.PAC.Name = "PAC"
        Me.PAC.Size = New System.Drawing.Size(1282, 648)
        Me.PAC.TabIndex = 0
        '
        'tiAssets
        '
        Me.tiAssets.AttachedControl = Me.TabControlPanel2
        Me.tiAssets.Name = "tiAssets"
        Me.tiAssets.Text = "Assets"
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        Me.NodeConnector1.LineWidth = 0
        '
        'frmPrism
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1284, 803)
        Me.Controls.Add(Me.RibbonBarMergeContainer1)
        Me.Controls.Add(Me.pnlPrism)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPrism"
        Me.Text = "EveHQ Prism"
        Me.ctxTransactions.ResumeLayout(False)
        CType(Me.dtiJournalEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtiJournalStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxRecycleExport.ResumeLayout(False)
        CType(Me.nudTax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBrokerFee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStandings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBaseYield, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tabItems.ResumeLayout(False)
        CType(Me.adtRecycle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxRecycleItem.ResumeLayout(False)
        Me.tabTotals.ResumeLayout(False)
        CType(Me.adtTotals, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxBPManager.ResumeLayout(False)
        Me.RibbonBarMergeContainer1.ResumeLayout(False)
        Me.pnlPrism.ResumeLayout(False)
        CType(Me.tabPrism, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPrism.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        Me.TabControlPanel1.PerformLayout()
        CType(Me.adtSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel6.ResumeLayout(False)
        Me.TabControlPanel6.PerformLayout()
        CType(Me.adtJournal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel17.ResumeLayout(False)
        CType(Me.adtInventionJobs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel3.ResumeLayout(False)
        Me.pnlReportControls.ResumeLayout(False)
        Me.pnlReportControls.PerformLayout()
        CType(Me.dtiReportEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtiReportStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel11.ResumeLayout(False)
        CType(Me.tcPM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcPM.ResumeLayout(False)
        Me.TabControlPanel12.ResumeLayout(False)
        CType(Me.adtProdJobs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlJobs.ResumeLayout(False)
        Me.TabControlPanel13.ResumeLayout(False)
        CType(Me.adtBatches, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelEx1.ResumeLayout(False)
        Me.TabControlPanel16.ResumeLayout(False)
        Me.TabControlPanel16.PerformLayout()
        CType(Me.adtInventionStats, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.adtInventionResults, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtiInventionEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtiInventionStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel7.ResumeLayout(False)
        Me.TabControlPanel7.PerformLayout()
        CType(Me.adtJobs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel5.ResumeLayout(False)
        Me.TabControlPanel5.PerformLayout()
        CType(Me.dtiTransEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtiTransStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.adtTransactions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel10.ResumeLayout(False)
        Me.PanelEx2.ResumeLayout(False)
        Me.PanelEx2.PerformLayout()
        CType(Me.dtiChartEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtiChartStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel15.ResumeLayout(False)
        Me.TabControlPanel15.PerformLayout()
        Me.pnlRigs.ResumeLayout(False)
        CType(Me.adtRigs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.adtRigBuildList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gpAutoRig.ResumeLayout(False)
        Me.gpAutoRig.PerformLayout()
        CType(Me.nudRigMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel4.ResumeLayout(False)
        Me.pnlSellOrders.ResumeLayout(False)
        Me.pnlSellOrders.PerformLayout()
        CType(Me.adtSellOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBuyOrders.ResumeLayout(False)
        Me.pnlBuyOrders.PerformLayout()
        CType(Me.adtBuyOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlOrderStats.ResumeLayout(False)
        Me.pnlOrderStats.PerformLayout()
        Me.TabControlPanel14.ResumeLayout(False)
        Me.TabControlPanel14.PerformLayout()
        CType(Me.adtContracts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel8.ResumeLayout(False)
        Me.TabControlPanel8.PerformLayout()
        Me.TabControlPanel9.ResumeLayout(False)
        Me.TabControlPanel9.PerformLayout()
        CType(Me.adtBlueprints, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbBPO, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbBPC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbMissing, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbUnknown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbExhausted, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbUserBP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblCurrentAPI As System.Windows.Forms.Label
    Friend WithEvents lvwCurrentAPIs As System.Windows.Forms.ListView
    Friend WithEvents colAPIOwner As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAssetsAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBalancesAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOwnerType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colJobsAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colJournalAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOrdersAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTransAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCorpSheetAPI As System.Windows.Forms.ColumnHeader
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
    Friend WithEvents lblWalletTransDivision As System.Windows.Forms.Label
    Friend WithEvents lblWalletJournalDivision As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabItems As System.Windows.Forms.TabPage
    Friend WithEvents tabTotals As System.Windows.Forms.TabPage
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
    Friend WithEvents ctxRecycleItem As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAlterRecycleQuantity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuRemoveRecycleItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExportTransactions As System.Windows.Forms.Button
    Friend WithEvents btnExportJournal As System.Windows.Forms.Button
    Friend WithEvents btnExportJobs As System.Windows.Forms.Button
    Friend WithEvents lblTotalFees As System.Windows.Forms.Label
    Friend WithEvents lblTotalFeesLbl As System.Windows.Forms.Label
    Friend WithEvents nudTax As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudBrokerFee As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkOverrideTax As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverrideBrokerFee As System.Windows.Forms.CheckBox
    Friend WithEvents chkFeesOnRefine As System.Windows.Forms.CheckBox
    Friend WithEvents lblPriceTotals As System.Windows.Forms.Label
    Friend WithEvents chkFeesOnItems As System.Windows.Forms.CheckBox
    Friend WithEvents lblJobInstallerFilter As System.Windows.Forms.Label
    Friend WithEvents chkShowOwnedBPs As System.Windows.Forms.CheckBox
    Friend WithEvents btnBPCalc As System.Windows.Forms.Button
    Friend WithEvents btnUpdateBPsFromAssets As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnGetBPJobInfo As System.Windows.Forms.Button
    Friend WithEvents ctxBPManager As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSendToBPCalc As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAmendBPDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblBPSearch As System.Windows.Forms.Label
    Friend WithEvents txtBPSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnResetBPSearch As System.Windows.Forms.Button
    Friend WithEvents lblUnknown As System.Windows.Forms.Label
    Friend WithEvents pbUnknown As System.Windows.Forms.PictureBox
    Friend WithEvents lblMissing As System.Windows.Forms.Label
    Friend WithEvents pbMissing As System.Windows.Forms.PictureBox
    Friend WithEvents lblBPC As System.Windows.Forms.Label
    Friend WithEvents pbBPC As System.Windows.Forms.PictureBox
    Friend WithEvents lblBPO As System.Windows.Forms.Label
    Friend WithEvents pbBPO As System.Windows.Forms.PictureBox
    Friend WithEvents lblExhausted As System.Windows.Forms.Label
    Friend WithEvents pbExhausted As System.Windows.Forms.PictureBox
    Friend WithEvents btnAddCustomBP As System.Windows.Forms.Button
    Friend WithEvents lblUserBP As System.Windows.Forms.Label
    Friend WithEvents pbUserBP As System.Windows.Forms.PictureBox
    Friend WithEvents mnuRemoveCustomBP As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboTechFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lblTechFilter As System.Windows.Forms.Label
    Friend WithEvents cboTypeFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lblTypeFilter As System.Windows.Forms.Label
    Friend WithEvents cboCategoryFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lblBPCatFilter As System.Windows.Forms.Label
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents ctxRecycleExport As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuExportToCSV As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExportToTSV As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExportTotalsToCSV As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExportTotalsToTSV As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxTransactions As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuTransactionModifyPrice As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RibbonBarMergeContainer1 As DevComponents.DotNetBar.RibbonBarMergeContainer
    Friend WithEvents rbData As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnDownloadAPIData As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbWallet As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnWalletJournal As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnWalletTransactions As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbAssetManagement As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnAssets As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnBPManager As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRecycler As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbMarketTools As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnOrders As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnJobs As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbAnalysisTools As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnReports As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnCharts As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents pnlPrism As DevComponents.DotNetBar.PanelEx
    Friend WithEvents cboWalletJournalDivision As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents dtiJournalEndDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents dtiJournalStartDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents lblJournalEndDate As System.Windows.Forms.Label
    Friend WithEvents lblJournalStartDate As System.Windows.Forms.Label
    Friend WithEvents cboJournalOwners As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents cboJournalRefTypes As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents btnJournalQuery As DevComponents.DotNetBar.ButtonX
    Friend WithEvents sbShowEveBalance As DevComponents.DotNetBar.Controls.SwitchButton
    Friend WithEvents lblAlwaysShowEveBalance As System.Windows.Forms.Label
    Friend WithEvents btnResetJournal As DevComponents.DotNetBar.ButtonX
    Friend WithEvents tabPrism As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiPrismHome As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiAssets As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiMarketOrders As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel5 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiTransactions As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel6 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiJournal As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel7 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiJobs As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel8 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiRecycler As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel9 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiBPManager As DevComponents.DotNetBar.TabItem
    Friend WithEvents pnlOrderStats As DevComponents.DotNetBar.PanelEx
    Friend WithEvents pnlSellOrders As DevComponents.DotNetBar.PanelEx
    Friend WithEvents splitterMarketOrders As DevComponents.DotNetBar.ExpandableSplitter
    Friend WithEvents pnlBuyOrders As DevComponents.DotNetBar.PanelEx
    Friend WithEvents adtJournal As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle1 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colJournalDate As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colJournalType As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colJournalAmount As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colJournalBalance As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colJournalDescription As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents rbQuickCalcs As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnInventionChance As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnOptions As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents txtItemSearch As DevComponents.DotNetBar.Controls.TextBoxX
    Friend WithEvents adtSearch As DevComponents.AdvTree.AdvTree
    Friend WithEvents colItemSearch As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector2 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle2 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents lblSelectedBP As System.Windows.Forms.Label
    Friend WithEvents lblSelectedItem As System.Windows.Forms.Label
    Friend WithEvents TabControlPanel11 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiProductionManager As DevComponents.DotNetBar.TabItem
    Friend WithEvents adtProdJobs As DevComponents.AdvTree.AdvTree
    Friend WithEvents colJobName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colJobItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector3 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle3 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents rbProduction As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnProductionManager As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnBlueprintCalc As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnDeleteJob As DevComponents.DotNetBar.ButtonX
    Friend WithEvents colJobUnitProfit As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colJobProfitRate As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents PRPM As EveHQ.Prism.PrismResources
    Friend WithEvents btnClearAllJobs As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnRefreshJobs As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnMakeBatch As DevComponents.DotNetBar.ButtonX
    Friend WithEvents tcPM As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel12 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiProductionJobs As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel13 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiBatchJobs As DevComponents.DotNetBar.TabItem
    Friend WithEvents adtBatches As DevComponents.AdvTree.AdvTree
    Friend WithEvents colBatchName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector4 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle4 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents pnlJobs As DevComponents.DotNetBar.PanelEx
    Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents btnClearBatches As DevComponents.DotNetBar.ButtonX
    Friend WithEvents adtBlueprints As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector5 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents BP As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colBPMBlueprint As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBPMLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBPMLocation2 As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBPMTechLevel As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBPMME As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBPMPE As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBPMRuns As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBPMStatus As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents adtJobs As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector7 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle5 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colIJobsItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIJobsActivity As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIJobsRuns As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIJobsInstaller As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIJobsLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIJobsEndTime As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIJobsStatus As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents adtBuyOrders As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector8 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle6 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colBuyType As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyQty As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyRange As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyVolume As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyExpires As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents adtSellOrders As DevComponents.AdvTree.AdvTree
    Friend WithEvents colSellType As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colSellQty As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colSellPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colSellLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colSellExpires As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector9 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle7 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents adtTransactions As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector10 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents Personal As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colTransDate As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colTransItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colTransQuantity As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colTransPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colTransValue As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colTransLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colTransClient As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents Corp As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents Buy As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents Sell As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents Numeric As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents adtRecycle As DevComponents.AdvTree.AdvTree
    Friend WithEvents colRecItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecMeta As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecQty As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecBatches As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecValue As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecFees As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecSalePrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecRefinePrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecBestPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecTotalBen As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRecUnitBen As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector12 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ItemNormal As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents adtTotals As DevComponents.AdvTree.AdvTree
    Friend WithEvents colRTMaterial As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRTStationTake As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRTUnrecoverable As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRTReceivable As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRTPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRTTotal As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector11 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle8 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents ItemGood As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents cboOrdersOwner As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblOrdersOwner As System.Windows.Forms.Label
    Friend WithEvents cboJobOwner As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblJobOwner As System.Windows.Forms.Label
    Friend WithEvents cboBPOwner As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblBPOwner As System.Windows.Forms.Label
    Friend WithEvents PAC As EveHQ.Prism.PrismAssetsControl
    Friend WithEvents cboWalletTransType As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboTransAll As DevComponents.Editors.ComboItem
    Friend WithEvents cboTransBuy As DevComponents.Editors.ComboItem
    Friend WithEvents cboTransSell As DevComponents.Editors.ComboItem
    Friend WithEvents btnGetTransactions As DevComponents.DotNetBar.ButtonX
    Friend WithEvents dtiTransEndDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtiTransStartDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboWalletTransDivision As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboTransactionOwner As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents lblTransItemType As System.Windows.Forms.Label
    Friend WithEvents cboWalletTransItem As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents lblTransProfitRatio As System.Windows.Forms.Label
    Friend WithEvents lblTransProfitValue As System.Windows.Forms.Label
    Friend WithEvents lblTransSellValue As System.Windows.Forms.Label
    Friend WithEvents lblTransBuyValue As System.Windows.Forms.Label
    Friend WithEvents btnCheckJournalOmissions As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboInstallerFilter As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblActivityFilter As System.Windows.Forms.Label
    Friend WithEvents cboActivityFilter As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblStatusFilter As System.Windows.Forms.Label
    Friend WithEvents cboStatusFilter As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents wbReport As System.Windows.Forms.WebBrowser
    Friend WithEvents pnlReportControls As DevComponents.DotNetBar.PanelEx
    Friend WithEvents cboReport As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblReportStartDate As System.Windows.Forms.Label
    Friend WithEvents btnGenerateReport As DevComponents.DotNetBar.ButtonX
    Friend WithEvents lblReportEndDate As System.Windows.Forms.Label
    Friend WithEvents dtiReportEndDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents dtiReportStartDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents cboReportOwners As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents tiReports As DevComponents.DotNetBar.TabItem
    Friend WithEvents btnExportEntries As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnImportEntries As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnLinkRequisition As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnLinkProduction As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnLinkBPCalc As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnQuickProduction As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents TabControlPanel10 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents PanelEx2 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents cboChart As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnGenerateChart As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dtiChartEndDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents dtiChartStartDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents cboChartOwners As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents tiCharts As DevComponents.DotNetBar.TabItem
    Friend WithEvents zgcPrism As ZedGraph.ZedGraphControl
    Friend WithEvents colContractsAPI As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnContracts As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents colJobMargin As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents btnCopyListToClipboard As System.Windows.Forms.Button
    Friend WithEvents TabControlPanel14 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiContracts As DevComponents.DotNetBar.TabItem
    Friend WithEvents cboContractOwner As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblContractOwner As System.Windows.Forms.Label
    Friend WithEvents adtContracts As DevComponents.AdvTree.AdvTree
    Friend WithEvents colContractTitle As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colContractLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colContractType As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colContractStatus As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colContractDateIssued As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colContractDateExpired As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colContractPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector6 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle9 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colContractVolume As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents btnRefreshAPI As DevComponents.DotNetBar.ButtonX
    Friend WithEvents colContractTransaction As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents mnuAddRecycleItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tiRigBuilder As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel15 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents gpAutoRig As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnAutoRig As DevComponents.DotNetBar.ButtonX
    Friend WithEvents chkRigMargin As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents chkRigSalePrice As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents chkTotalProfit As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents chkTotalSalePrice As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents chkRigProfit As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents btnBuildRigs As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnExportRigBuildList As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnExportRigList As DevComponents.DotNetBar.ButtonX
    Friend WithEvents nudRigMELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblRigMELevel As System.Windows.Forms.Label
    Friend WithEvents adtRigs As DevComponents.AdvTree.AdvTree
    Friend WithEvents colRigListType As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigListQuantity As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigListRigPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigListSalvagePrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigListBuildBenefit As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigListRigValue As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigListSalvageValue As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigListTotalBuildBenefit As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigListMargin As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector14 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle11 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents adtRigBuildList As DevComponents.AdvTree.AdvTree
    Friend WithEvents colRigBuildType As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigBuildQuantity As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigBuidRigPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigBuildSalvagePrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigBuildBenefit As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigBuildRigValue As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigBuildSalvageValue As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigBuildTotalBenefit As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colRigBuildMargin As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector13 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle10 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents lblTotalRigMargin As System.Windows.Forms.Label
    Friend WithEvents lblTotalRigProfit As System.Windows.Forms.Label
    Friend WithEvents lblTotalRigSalePrice As System.Windows.Forms.Label
    Friend WithEvents PSCRigOwners As EveHQ.Prism.PrismSelectionHostControl
    Friend WithEvents btnRigBuilder As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents pnlRigs As DevComponents.DotNetBar.PanelEx
    Friend WithEvents ExpandableSplitter1 As DevComponents.DotNetBar.ExpandableSplitter
    Friend WithEvents TabControlPanel16 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiInventionResults As DevComponents.DotNetBar.TabItem
    Friend WithEvents btnInventionResults As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents cboInventionItems As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents cboInventionInstallers As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents btnGetResults As DevComponents.DotNetBar.ButtonX
    Friend WithEvents dtiInventionEndDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents lblInvEndDate As System.Windows.Forms.Label
    Friend WithEvents dtiInventionStartDate As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents lblInvStartDate As System.Windows.Forms.Label
    Friend WithEvents adtInventionResults As DevComponents.AdvTree.AdvTree
    Friend WithEvents colInventionDate As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colInventionItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colInventionInstaller As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colInventionResult As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector15 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle12 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents adtInventionStats As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector16 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle13 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents lblInventionItems As System.Windows.Forms.Label
    Friend WithEvents lblInventionInstallers As System.Windows.Forms.Label
    Friend WithEvents btnInventionManager As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents TabControlPanel17 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents adtInventionJobs As DevComponents.AdvTree.AdvTree
    Friend WithEvents colInvJobName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colInvJobItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colInvUnitProfit As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colInvSalesPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colInvProfitMargin As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector17 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle14 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents tiInventionManager As DevComponents.DotNetBar.TabItem
    Friend WithEvents colSuccessChance As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colSuccessCost As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colInvProductionCost As DevComponents.AdvTree.ColumnHeader
End Class
