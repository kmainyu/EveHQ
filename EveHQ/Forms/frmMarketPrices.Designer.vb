<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMarketPrices
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMarketPrices))
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog()
        Me.chkEnableLogWatcher = New System.Windows.Forms.CheckBox()
        Me.chkEnableWatcherAtStartup = New System.Windows.Forms.CheckBox()
        Me.chkNotifyTray = New System.Windows.Forms.CheckBox()
        Me.chkAutoUpdatePriceData = New System.Windows.Forms.CheckBox()
        Me.chkNotifyPopup = New System.Windows.Forms.CheckBox()
        Me.chkAutoUpdateCurrentPrice = New System.Windows.Forms.CheckBox()
        Me.nudIgnoreBuyOrderLimit = New System.Windows.Forms.NumericUpDown()
        Me.nudIgnoreSellOrderLimit = New System.Windows.Forms.NumericUpDown()
        Me.chkIgnoreBuyOrders = New System.Windows.Forms.CheckBox()
        Me.chkIgnoreSellOrders = New System.Windows.Forms.CheckBox()
        Me.lblIgnoreSellOrderUnit = New System.Windows.Forms.Label()
        Me.lblIgnoreBuyOrderUnit = New System.Windows.Forms.Label()
        Me.lblHighlightOldLogsText = New System.Windows.Forms.Label()
        Me.ctxMarketExport = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuViewOrders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeleteLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.nudAge = New System.Windows.Forms.NumericUpDown()
        Me.lblHighlightLogsHours = New System.Windows.Forms.Label()
        Me.chkShowOnlyCustom = New System.Windows.Forms.CheckBox()
        Me.lblCustomPrices = New System.Windows.Forms.Label()
        Me.btnResetGrid = New System.Windows.Forms.Button()
        Me.txtSearchPrices = New System.Windows.Forms.TextBox()
        Me.lblSearchPrices = New System.Windows.Forms.Label()
        Me.ctxPrices = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPriceItemName = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPriceAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPriceEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPriceDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblMarketPriceUpdateStatus = New System.Windows.Forms.Label()
        Me.btnDownloadMarketPrices = New System.Windows.Forms.Button()
        Me.lblBattleclinicLink = New System.Windows.Forms.LinkLabel()
        Me.lblFactionPriceUpdateStatus = New System.Windows.Forms.Label()
        Me.btnUpdateFactionPrices = New System.Windows.Forms.Button()
        Me.lblFactionPricesBy = New System.Windows.Forms.LinkLabel()
        Me.lblFactionPricesByLbl = New System.Windows.Forms.Label()
        Me.lblLastFactionPriceUpdate = New System.Windows.Forms.Label()
        Me.lblLastFactionPriceUpdateLbl = New System.Windows.Forms.Label()
        Me.TabControl1 = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
        Me.btnDropNewTables = New System.Windows.Forms.Button()
        Me.gpLogParsing = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.gpMLWOptions = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.tiPriceSettings = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtLogs = New DevComponents.AdvTree.AdvTree()
        Me.colRegion = New DevComponents.AdvTree.ColumnHeader()
        Me.colItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colDate = New DevComponents.AdvTree.ColumnHeader()
        Me.colAge = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.Log = New DevComponents.DotNetBar.ElementStyle()
        Me.panelLogSettings = New DevComponents.DotNetBar.PanelEx()
        Me.tiMarketLogs = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel5 = New DevComponents.DotNetBar.TabControlPanel()
        Me.gpMarketPrices = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.lblEveMarketeerLink = New System.Windows.Forms.LinkLabel()
        Me.radEveMarketeer = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.radBattleclinic = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.lblMarketSource = New System.Windows.Forms.Label()
        Me.lblMarketCacheFiles = New System.Windows.Forms.Label()
        Me.adtMarketCache = New DevComponents.AdvTree.AdvTree()
        Me.colRegionName = New DevComponents.AdvTree.ColumnHeader()
        Me.colFileDate = New DevComponents.AdvTree.ColumnHeader()
        Me.colCacheDate = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector6 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle5 = New DevComponents.DotNetBar.ElementStyle()
        Me.btnUpdateMarketPrices = New System.Windows.Forms.Button()
        Me.gpFactionPrices = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.tiPriceUpdates = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel6 = New DevComponents.DotNetBar.TabControlPanel()
        Me.gpSelectedPriceGroup = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.btnClearItems = New DevComponents.DotNetBar.ButtonX()
        Me.btnDeleteItem = New DevComponents.DotNetBar.ButtonX()
        Me.btnAddItem = New DevComponents.DotNetBar.ButtonX()
        Me.adtPriceGroupItems = New DevComponents.AdvTree.AdvTree()
        Me.colPriceItemName = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector4 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle3 = New DevComponents.DotNetBar.ElementStyle()
        Me.lblTypeIDs = New DevComponents.DotNetBar.LabelX()
        Me.lblSelectedPriceGroup = New DevComponents.DotNetBar.LabelX()
        Me.lblRegions = New DevComponents.DotNetBar.LabelX()
        Me.adtPriceGroupRegions = New DevComponents.AdvTree.AdvTree()
        Me.colPriceRegions = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector3 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle2 = New DevComponents.DotNetBar.ElementStyle()
        Me.GroupPanel1 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.chkPGMedBuy = New System.Windows.Forms.CheckBox()
        Me.chkPGMedAll = New System.Windows.Forms.CheckBox()
        Me.chkPGAvgBuy = New System.Windows.Forms.CheckBox()
        Me.chkPGMinAll = New System.Windows.Forms.CheckBox()
        Me.chkPGMaxBuy = New System.Windows.Forms.CheckBox()
        Me.chkPGMaxAll = New System.Windows.Forms.CheckBox()
        Me.chkPGMinBuy = New System.Windows.Forms.CheckBox()
        Me.chkPGAvgAll = New System.Windows.Forms.CheckBox()
        Me.chkPGMedSell = New System.Windows.Forms.CheckBox()
        Me.chkPGAvgSell = New System.Windows.Forms.CheckBox()
        Me.chkPGMinSell = New System.Windows.Forms.CheckBox()
        Me.chkPGMaxSell = New System.Windows.Forms.CheckBox()
        Me.btnDeletePriceGroup = New DevComponents.DotNetBar.ButtonX()
        Me.btnEditPriceGroup = New DevComponents.DotNetBar.ButtonX()
        Me.btnAddPriceGroup = New DevComponents.DotNetBar.ButtonX()
        Me.LabelX1 = New DevComponents.DotNetBar.LabelX()
        Me.adtPriceGroups = New DevComponents.AdvTree.AdvTree()
        Me.colPriceGroupName = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector2 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle1 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiPriceGroups = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtPrices = New DevComponents.AdvTree.AdvTree()
        Me.colCustomItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomBase = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomMarket = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomCustom = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector5 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle4 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiCustomPrices = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.btnRefreshLogs = New DevComponents.DotNetBar.ButtonX()
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxMarketExport.SuspendLayout()
        CType(Me.nudAge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxPrices.SuspendLayout()
        CType(Me.TabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        Me.gpLogParsing.SuspendLayout()
        Me.gpMLWOptions.SuspendLayout()
        Me.TabControlPanel3.SuspendLayout()
        CType(Me.adtLogs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelLogSettings.SuspendLayout()
        Me.TabControlPanel5.SuspendLayout()
        Me.gpMarketPrices.SuspendLayout()
        CType(Me.adtMarketCache, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gpFactionPrices.SuspendLayout()
        Me.TabControlPanel6.SuspendLayout()
        Me.gpSelectedPriceGroup.SuspendLayout()
        CType(Me.adtPriceGroupItems, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.adtPriceGroupRegions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupPanel1.SuspendLayout()
        CType(Me.adtPriceGroups, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel4.SuspendLayout()
        CType(Me.adtPrices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'chkEnableLogWatcher
        '
        Me.chkEnableLogWatcher.AutoSize = True
        Me.chkEnableLogWatcher.BackColor = System.Drawing.Color.Transparent
        Me.chkEnableLogWatcher.Location = New System.Drawing.Point(3, 10)
        Me.chkEnableLogWatcher.Name = "chkEnableLogWatcher"
        Me.chkEnableLogWatcher.Size = New System.Drawing.Size(102, 17)
        Me.chkEnableLogWatcher.TabIndex = 25
        Me.chkEnableLogWatcher.Text = "Enable Watcher"
        Me.chkEnableLogWatcher.UseVisualStyleBackColor = False
        '
        'chkEnableWatcherAtStartup
        '
        Me.chkEnableWatcherAtStartup.AutoSize = True
        Me.chkEnableWatcherAtStartup.BackColor = System.Drawing.Color.Transparent
        Me.chkEnableWatcherAtStartup.Location = New System.Drawing.Point(3, 33)
        Me.chkEnableWatcherAtStartup.Name = "chkEnableWatcherAtStartup"
        Me.chkEnableWatcherAtStartup.Size = New System.Drawing.Size(184, 17)
        Me.chkEnableWatcherAtStartup.TabIndex = 26
        Me.chkEnableWatcherAtStartup.Text = "Start Watcher on EveHQ Startup"
        Me.chkEnableWatcherAtStartup.UseVisualStyleBackColor = False
        '
        'chkNotifyTray
        '
        Me.chkNotifyTray.AutoSize = True
        Me.chkNotifyTray.BackColor = System.Drawing.Color.Transparent
        Me.chkNotifyTray.Location = New System.Drawing.Point(353, 33)
        Me.chkNotifyTray.Name = "chkNotifyTray"
        Me.chkNotifyTray.Size = New System.Drawing.Size(212, 17)
        Me.chkNotifyTray.TabIndex = 23
        Me.chkNotifyTray.Text = "System Tray Notification on Processing"
        Me.chkNotifyTray.UseVisualStyleBackColor = False
        '
        'chkAutoUpdatePriceData
        '
        Me.chkAutoUpdatePriceData.AutoSize = True
        Me.chkAutoUpdatePriceData.BackColor = System.Drawing.Color.Transparent
        Me.chkAutoUpdatePriceData.Enabled = False
        Me.chkAutoUpdatePriceData.Location = New System.Drawing.Point(193, 33)
        Me.chkAutoUpdatePriceData.Name = "chkAutoUpdatePriceData"
        Me.chkAutoUpdatePriceData.Size = New System.Drawing.Size(151, 17)
        Me.chkAutoUpdatePriceData.TabIndex = 28
        Me.chkAutoUpdatePriceData.Text = "Auto-Update Price History"
        Me.chkAutoUpdatePriceData.UseVisualStyleBackColor = False
        '
        'chkNotifyPopup
        '
        Me.chkNotifyPopup.AutoSize = True
        Me.chkNotifyPopup.BackColor = System.Drawing.Color.Transparent
        Me.chkNotifyPopup.Location = New System.Drawing.Point(353, 10)
        Me.chkNotifyPopup.Name = "chkNotifyPopup"
        Me.chkNotifyPopup.Size = New System.Drawing.Size(182, 17)
        Me.chkNotifyPopup.TabIndex = 24
        Me.chkNotifyPopup.Text = "Popup Notification on Processing"
        Me.chkNotifyPopup.UseVisualStyleBackColor = False
        '
        'chkAutoUpdateCurrentPrice
        '
        Me.chkAutoUpdateCurrentPrice.AutoSize = True
        Me.chkAutoUpdateCurrentPrice.BackColor = System.Drawing.Color.Transparent
        Me.chkAutoUpdateCurrentPrice.Location = New System.Drawing.Point(193, 10)
        Me.chkAutoUpdateCurrentPrice.Name = "chkAutoUpdateCurrentPrice"
        Me.chkAutoUpdateCurrentPrice.Size = New System.Drawing.Size(154, 17)
        Me.chkAutoUpdateCurrentPrice.TabIndex = 27
        Me.chkAutoUpdateCurrentPrice.Text = "Auto-Update Current Price"
        Me.chkAutoUpdateCurrentPrice.UseVisualStyleBackColor = False
        '
        'nudIgnoreBuyOrderLimit
        '
        Me.nudIgnoreBuyOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreBuyOrderLimit.Location = New System.Drawing.Point(176, 2)
        Me.nudIgnoreBuyOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreBuyOrderLimit.Name = "nudIgnoreBuyOrderLimit"
        Me.nudIgnoreBuyOrderLimit.Size = New System.Drawing.Size(74, 21)
        Me.nudIgnoreBuyOrderLimit.TabIndex = 29
        Me.nudIgnoreBuyOrderLimit.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'nudIgnoreSellOrderLimit
        '
        Me.nudIgnoreSellOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreSellOrderLimit.Location = New System.Drawing.Point(476, 2)
        Me.nudIgnoreSellOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreSellOrderLimit.Name = "nudIgnoreSellOrderLimit"
        Me.nudIgnoreSellOrderLimit.Size = New System.Drawing.Size(74, 21)
        Me.nudIgnoreSellOrderLimit.TabIndex = 30
        Me.nudIgnoreSellOrderLimit.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'chkIgnoreBuyOrders
        '
        Me.chkIgnoreBuyOrders.AutoSize = True
        Me.chkIgnoreBuyOrders.BackColor = System.Drawing.Color.Transparent
        Me.chkIgnoreBuyOrders.Location = New System.Drawing.Point(3, 3)
        Me.chkIgnoreBuyOrders.Name = "chkIgnoreBuyOrders"
        Me.chkIgnoreBuyOrders.Size = New System.Drawing.Size(170, 17)
        Me.chkIgnoreBuyOrders.TabIndex = 33
        Me.chkIgnoreBuyOrders.Text = "Ignore Buy Orders Less Than:"
        Me.chkIgnoreBuyOrders.UseVisualStyleBackColor = False
        '
        'chkIgnoreSellOrders
        '
        Me.chkIgnoreSellOrders.AutoSize = True
        Me.chkIgnoreSellOrders.BackColor = System.Drawing.Color.Transparent
        Me.chkIgnoreSellOrders.Location = New System.Drawing.Point(303, 3)
        Me.chkIgnoreSellOrders.Name = "chkIgnoreSellOrders"
        Me.chkIgnoreSellOrders.Size = New System.Drawing.Size(171, 17)
        Me.chkIgnoreSellOrders.TabIndex = 34
        Me.chkIgnoreSellOrders.Text = "Ignore Sell Orders More Than:"
        Me.chkIgnoreSellOrders.UseVisualStyleBackColor = False
        '
        'lblIgnoreSellOrderUnit
        '
        Me.lblIgnoreSellOrderUnit.AutoSize = True
        Me.lblIgnoreSellOrderUnit.BackColor = System.Drawing.Color.Transparent
        Me.lblIgnoreSellOrderUnit.Location = New System.Drawing.Point(556, 4)
        Me.lblIgnoreSellOrderUnit.Name = "lblIgnoreSellOrderUnit"
        Me.lblIgnoreSellOrderUnit.Size = New System.Drawing.Size(39, 13)
        Me.lblIgnoreSellOrderUnit.TabIndex = 32
        Me.lblIgnoreSellOrderUnit.Text = "x Base"
        '
        'lblIgnoreBuyOrderUnit
        '
        Me.lblIgnoreBuyOrderUnit.AutoSize = True
        Me.lblIgnoreBuyOrderUnit.BackColor = System.Drawing.Color.Transparent
        Me.lblIgnoreBuyOrderUnit.Location = New System.Drawing.Point(256, 4)
        Me.lblIgnoreBuyOrderUnit.Name = "lblIgnoreBuyOrderUnit"
        Me.lblIgnoreBuyOrderUnit.Size = New System.Drawing.Size(23, 13)
        Me.lblIgnoreBuyOrderUnit.TabIndex = 31
        Me.lblIgnoreBuyOrderUnit.Text = "ISK"
        '
        'lblHighlightOldLogsText
        '
        Me.lblHighlightOldLogsText.AutoSize = True
        Me.lblHighlightOldLogsText.Location = New System.Drawing.Point(12, 7)
        Me.lblHighlightOldLogsText.Name = "lblHighlightOldLogsText"
        Me.lblHighlightOldLogsText.Size = New System.Drawing.Size(122, 13)
        Me.lblHighlightOldLogsText.TabIndex = 2
        Me.lblHighlightOldLogsText.Text = "Highlight logs older than"
        '
        'ctxMarketExport
        '
        Me.ctxMarketExport.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxMarketExport.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewOrders, Me.mnuDeleteLog})
        Me.ctxMarketExport.Name = "ctxPrices"
        Me.ctxMarketExport.Size = New System.Drawing.Size(142, 48)
        '
        'mnuViewOrders
        '
        Me.mnuViewOrders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuViewOrders.Name = "mnuViewOrders"
        Me.mnuViewOrders.Size = New System.Drawing.Size(141, 22)
        Me.mnuViewOrders.Text = "View Orders"
        '
        'mnuDeleteLog
        '
        Me.mnuDeleteLog.Name = "mnuDeleteLog"
        Me.mnuDeleteLog.Size = New System.Drawing.Size(141, 22)
        Me.mnuDeleteLog.Text = "Delete log"
        '
        'nudAge
        '
        Me.nudAge.Location = New System.Drawing.Point(140, 5)
        Me.nudAge.Maximum = New Decimal(New Integer() {336, 0, 0, 0})
        Me.nudAge.Name = "nudAge"
        Me.nudAge.Size = New System.Drawing.Size(70, 21)
        Me.nudAge.TabIndex = 4
        Me.nudAge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudAge.Value = New Decimal(New Integer() {24, 0, 0, 0})
        '
        'lblHighlightLogsHours
        '
        Me.lblHighlightLogsHours.AutoSize = True
        Me.lblHighlightLogsHours.Location = New System.Drawing.Point(216, 7)
        Me.lblHighlightLogsHours.Name = "lblHighlightLogsHours"
        Me.lblHighlightLogsHours.Size = New System.Drawing.Size(38, 13)
        Me.lblHighlightLogsHours.TabIndex = 3
        Me.lblHighlightLogsHours.Text = "hours."
        '
        'chkShowOnlyCustom
        '
        Me.chkShowOnlyCustom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkShowOnlyCustom.AutoSize = True
        Me.chkShowOnlyCustom.BackColor = System.Drawing.Color.Transparent
        Me.chkShowOnlyCustom.Location = New System.Drawing.Point(375, 653)
        Me.chkShowOnlyCustom.Name = "chkShowOnlyCustom"
        Me.chkShowOnlyCustom.Size = New System.Drawing.Size(147, 17)
        Me.chkShowOnlyCustom.TabIndex = 20
        Me.chkShowOnlyCustom.Text = "Show Only Custom Prices"
        Me.chkShowOnlyCustom.UseVisualStyleBackColor = False
        '
        'lblCustomPrices
        '
        Me.lblCustomPrices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCustomPrices.BackColor = System.Drawing.Color.Transparent
        Me.lblCustomPrices.Location = New System.Drawing.Point(4, 5)
        Me.lblCustomPrices.Name = "lblCustomPrices"
        Me.lblCustomPrices.Size = New System.Drawing.Size(808, 31)
        Me.lblCustomPrices.TabIndex = 19
        Me.lblCustomPrices.Text = "Custom Prices enable you to override the Base and Market prices by allowing you t" & _
    "o enter your own individual figures. Right-click an item to edit or delete a Cus" & _
    "tom price." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnResetGrid
        '
        Me.btnResetGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnResetGrid.Location = New System.Drawing.Point(272, 650)
        Me.btnResetGrid.Name = "btnResetGrid"
        Me.btnResetGrid.Size = New System.Drawing.Size(85, 23)
        Me.btnResetGrid.TabIndex = 18
        Me.btnResetGrid.Text = "Reset Grid"
        Me.btnResetGrid.UseVisualStyleBackColor = True
        '
        'txtSearchPrices
        '
        Me.txtSearchPrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSearchPrices.Location = New System.Drawing.Point(79, 651)
        Me.txtSearchPrices.Name = "txtSearchPrices"
        Me.txtSearchPrices.Size = New System.Drawing.Size(187, 21)
        Me.txtSearchPrices.TabIndex = 17
        '
        'lblSearchPrices
        '
        Me.lblSearchPrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSearchPrices.AutoSize = True
        Me.lblSearchPrices.BackColor = System.Drawing.Color.Transparent
        Me.lblSearchPrices.Location = New System.Drawing.Point(4, 655)
        Me.lblSearchPrices.Name = "lblSearchPrices"
        Me.lblSearchPrices.Size = New System.Drawing.Size(74, 13)
        Me.lblSearchPrices.TabIndex = 16
        Me.lblSearchPrices.Text = "Search Items:"
        '
        'ctxPrices
        '
        Me.ctxPrices.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxPrices.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPriceItemName, Me.ToolStripMenuItem1, Me.mnuPriceAdd, Me.mnuPriceEdit, Me.mnuPriceDelete})
        Me.ctxPrices.Name = "ctxPrices"
        Me.ctxPrices.Size = New System.Drawing.Size(171, 98)
        '
        'mnuPriceItemName
        '
        Me.mnuPriceItemName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuPriceItemName.Name = "mnuPriceItemName"
        Me.mnuPriceItemName.Size = New System.Drawing.Size(170, 22)
        Me.mnuPriceItemName.Text = "Item Name"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(167, 6)
        '
        'mnuPriceAdd
        '
        Me.mnuPriceAdd.Name = "mnuPriceAdd"
        Me.mnuPriceAdd.Size = New System.Drawing.Size(170, 22)
        Me.mnuPriceAdd.Text = "Add Custom Price"
        '
        'mnuPriceEdit
        '
        Me.mnuPriceEdit.Name = "mnuPriceEdit"
        Me.mnuPriceEdit.Size = New System.Drawing.Size(170, 22)
        Me.mnuPriceEdit.Text = "Edit Custom Price"
        '
        'mnuPriceDelete
        '
        Me.mnuPriceDelete.Name = "mnuPriceDelete"
        Me.mnuPriceDelete.Size = New System.Drawing.Size(170, 22)
        Me.mnuPriceDelete.Text = "Delete Custom Price"
        '
        'lblMarketPriceUpdateStatus
        '
        Me.lblMarketPriceUpdateStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMarketPriceUpdateStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblMarketPriceUpdateStatus.Location = New System.Drawing.Point(4, 126)
        Me.lblMarketPriceUpdateStatus.Name = "lblMarketPriceUpdateStatus"
        Me.lblMarketPriceUpdateStatus.Size = New System.Drawing.Size(583, 13)
        Me.lblMarketPriceUpdateStatus.TabIndex = 5
        Me.lblMarketPriceUpdateStatus.Text = "Status:"
        '
        'btnDownloadMarketPrices
        '
        Me.btnDownloadMarketPrices.Location = New System.Drawing.Point(34, 90)
        Me.btnDownloadMarketPrices.Name = "btnDownloadMarketPrices"
        Me.btnDownloadMarketPrices.Size = New System.Drawing.Size(150, 23)
        Me.btnDownloadMarketPrices.TabIndex = 4
        Me.btnDownloadMarketPrices.Text = "Download Market Prices"
        Me.btnDownloadMarketPrices.UseVisualStyleBackColor = True
        '
        'lblBattleclinicLink
        '
        Me.lblBattleclinicLink.AutoSize = True
        Me.lblBattleclinicLink.BackColor = System.Drawing.Color.Transparent
        Me.lblBattleclinicLink.Location = New System.Drawing.Point(163, 32)
        Me.lblBattleclinicLink.Name = "lblBattleclinicLink"
        Me.lblBattleclinicLink.Size = New System.Drawing.Size(140, 13)
        Me.lblBattleclinicLink.TabIndex = 3
        Me.lblBattleclinicLink.TabStop = True
        Me.lblBattleclinicLink.Text = "http://www.battleclinic.com"
        '
        'lblFactionPriceUpdateStatus
        '
        Me.lblFactionPriceUpdateStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFactionPriceUpdateStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblFactionPriceUpdateStatus.Location = New System.Drawing.Point(162, 60)
        Me.lblFactionPriceUpdateStatus.Name = "lblFactionPriceUpdateStatus"
        Me.lblFactionPriceUpdateStatus.Size = New System.Drawing.Size(427, 13)
        Me.lblFactionPriceUpdateStatus.TabIndex = 5
        Me.lblFactionPriceUpdateStatus.Text = "Status:"
        '
        'btnUpdateFactionPrices
        '
        Me.btnUpdateFactionPrices.Location = New System.Drawing.Point(6, 55)
        Me.btnUpdateFactionPrices.Name = "btnUpdateFactionPrices"
        Me.btnUpdateFactionPrices.Size = New System.Drawing.Size(150, 23)
        Me.btnUpdateFactionPrices.TabIndex = 4
        Me.btnUpdateFactionPrices.Text = "Update Faction Prices"
        Me.btnUpdateFactionPrices.UseVisualStyleBackColor = True
        '
        'lblFactionPricesBy
        '
        Me.lblFactionPricesBy.AutoSize = True
        Me.lblFactionPricesBy.BackColor = System.Drawing.Color.Transparent
        Me.lblFactionPricesBy.Location = New System.Drawing.Point(147, 8)
        Me.lblFactionPricesBy.Name = "lblFactionPricesBy"
        Me.lblFactionPricesBy.Size = New System.Drawing.Size(149, 13)
        Me.lblFactionPricesBy.TabIndex = 3
        Me.lblFactionPricesBy.TabStop = True
        Me.lblFactionPricesBy.Text = "http://prices.c0rporation.com"
        '
        'lblFactionPricesByLbl
        '
        Me.lblFactionPricesByLbl.AutoSize = True
        Me.lblFactionPricesByLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblFactionPricesByLbl.Location = New System.Drawing.Point(3, 8)
        Me.lblFactionPricesByLbl.Name = "lblFactionPricesByLbl"
        Me.lblFactionPricesByLbl.Size = New System.Drawing.Size(138, 13)
        Me.lblFactionPricesByLbl.TabIndex = 2
        Me.lblFactionPricesByLbl.Text = "Faction Prices Supplied By: "
        '
        'lblLastFactionPriceUpdate
        '
        Me.lblLastFactionPriceUpdate.AutoSize = True
        Me.lblLastFactionPriceUpdate.BackColor = System.Drawing.Color.Transparent
        Me.lblLastFactionPriceUpdate.Location = New System.Drawing.Point(147, 30)
        Me.lblLastFactionPriceUpdate.Name = "lblLastFactionPriceUpdate"
        Me.lblLastFactionPriceUpdate.Size = New System.Drawing.Size(51, 13)
        Me.lblLastFactionPriceUpdate.TabIndex = 1
        Me.lblLastFactionPriceUpdate.Text = "Unknown"
        '
        'lblLastFactionPriceUpdateLbl
        '
        Me.lblLastFactionPriceUpdateLbl.AutoSize = True
        Me.lblLastFactionPriceUpdateLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblLastFactionPriceUpdateLbl.Location = New System.Drawing.Point(3, 30)
        Me.lblLastFactionPriceUpdateLbl.Name = "lblLastFactionPriceUpdateLbl"
        Me.lblLastFactionPriceUpdateLbl.Size = New System.Drawing.Size(133, 13)
        Me.lblLastFactionPriceUpdateLbl.TabIndex = 0
        Me.lblLastFactionPriceUpdateLbl.Text = "Last Faction Price Update:"
        '
        'TabControl1
        '
        Me.TabControl1.BackColor = System.Drawing.Color.Transparent
        Me.TabControl1.CanReorderTabs = True
        Me.TabControl1.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.TabControl1.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.TabControl1.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.TabControl1.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.TabControl1.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.TabControl1.Controls.Add(Me.TabControlPanel3)
        Me.TabControl1.Controls.Add(Me.TabControlPanel1)
        Me.TabControl1.Controls.Add(Me.TabControlPanel5)
        Me.TabControl1.Controls.Add(Me.TabControlPanel6)
        Me.TabControl1.Controls.Add(Me.TabControlPanel4)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.TabControl1.SelectedTabIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(816, 706)
        Me.TabControl1.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.TabControl1.TabIndex = 18
        Me.TabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.TabControl1.Tabs.Add(Me.tiPriceSettings)
        Me.TabControl1.Tabs.Add(Me.tiPriceGroups)
        Me.TabControl1.Tabs.Add(Me.tiMarketLogs)
        Me.TabControl1.Tabs.Add(Me.tiPriceUpdates)
        Me.TabControl1.Tabs.Add(Me.tiCustomPrices)
        Me.TabControl1.Text = "TabControl1"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.btnDropNewTables)
        Me.TabControlPanel1.Controls.Add(Me.gpLogParsing)
        Me.TabControlPanel1.Controls.Add(Me.gpMLWOptions)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(816, 683)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 1
        Me.TabControlPanel1.TabItem = Me.tiPriceSettings
        '
        'btnDropNewTables
        '
        Me.btnDropNewTables.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDropNewTables.Location = New System.Drawing.Point(7, 153)
        Me.btnDropNewTables.Name = "btnDropNewTables"
        Me.btnDropNewTables.Size = New System.Drawing.Size(180, 23)
        Me.btnDropNewTables.TabIndex = 23
        Me.btnDropNewTables.Text = "Delete New Database Tables"
        Me.btnDropNewTables.UseVisualStyleBackColor = True
        '
        'gpLogParsing
        '
        Me.gpLogParsing.BackColor = System.Drawing.Color.Transparent
        Me.gpLogParsing.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpLogParsing.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpLogParsing.Controls.Add(Me.nudIgnoreBuyOrderLimit)
        Me.gpLogParsing.Controls.Add(Me.chkIgnoreBuyOrders)
        Me.gpLogParsing.Controls.Add(Me.nudIgnoreSellOrderLimit)
        Me.gpLogParsing.Controls.Add(Me.lblIgnoreBuyOrderUnit)
        Me.gpLogParsing.Controls.Add(Me.lblIgnoreSellOrderUnit)
        Me.gpLogParsing.Controls.Add(Me.chkIgnoreSellOrders)
        Me.gpLogParsing.Location = New System.Drawing.Point(4, 88)
        Me.gpLogParsing.Name = "gpLogParsing"
        Me.gpLogParsing.Size = New System.Drawing.Size(616, 49)
        '
        '
        '
        Me.gpLogParsing.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpLogParsing.Style.BackColorGradientAngle = 90
        Me.gpLogParsing.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpLogParsing.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpLogParsing.Style.BorderBottomWidth = 1
        Me.gpLogParsing.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpLogParsing.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpLogParsing.Style.BorderLeftWidth = 1
        Me.gpLogParsing.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpLogParsing.Style.BorderRightWidth = 1
        Me.gpLogParsing.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpLogParsing.Style.BorderTopWidth = 1
        Me.gpLogParsing.Style.Class = ""
        Me.gpLogParsing.Style.CornerDiameter = 4
        Me.gpLogParsing.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpLogParsing.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpLogParsing.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpLogParsing.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpLogParsing.StyleMouseDown.Class = ""
        Me.gpLogParsing.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpLogParsing.StyleMouseOver.Class = ""
        Me.gpLogParsing.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpLogParsing.TabIndex = 22
        Me.gpLogParsing.Text = "Market Log Parsing Criteria"
        '
        'gpMLWOptions
        '
        Me.gpMLWOptions.BackColor = System.Drawing.Color.Transparent
        Me.gpMLWOptions.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpMLWOptions.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpMLWOptions.Controls.Add(Me.chkEnableLogWatcher)
        Me.gpMLWOptions.Controls.Add(Me.chkEnableWatcherAtStartup)
        Me.gpMLWOptions.Controls.Add(Me.chkAutoUpdateCurrentPrice)
        Me.gpMLWOptions.Controls.Add(Me.chkNotifyTray)
        Me.gpMLWOptions.Controls.Add(Me.chkNotifyPopup)
        Me.gpMLWOptions.Controls.Add(Me.chkAutoUpdatePriceData)
        Me.gpMLWOptions.Location = New System.Drawing.Point(4, 4)
        Me.gpMLWOptions.Name = "gpMLWOptions"
        Me.gpMLWOptions.Size = New System.Drawing.Size(616, 78)
        '
        '
        '
        Me.gpMLWOptions.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpMLWOptions.Style.BackColorGradientAngle = 90
        Me.gpMLWOptions.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpMLWOptions.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMLWOptions.Style.BorderBottomWidth = 1
        Me.gpMLWOptions.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpMLWOptions.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMLWOptions.Style.BorderLeftWidth = 1
        Me.gpMLWOptions.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMLWOptions.Style.BorderRightWidth = 1
        Me.gpMLWOptions.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMLWOptions.Style.BorderTopWidth = 1
        Me.gpMLWOptions.Style.Class = ""
        Me.gpMLWOptions.Style.CornerDiameter = 4
        Me.gpMLWOptions.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpMLWOptions.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpMLWOptions.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpMLWOptions.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpMLWOptions.StyleMouseDown.Class = ""
        Me.gpMLWOptions.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpMLWOptions.StyleMouseOver.Class = ""
        Me.gpMLWOptions.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpMLWOptions.TabIndex = 0
        Me.gpMLWOptions.Text = "Market Log Watcher Options"
        '
        'tiPriceSettings
        '
        Me.tiPriceSettings.AttachedControl = Me.TabControlPanel1
        Me.tiPriceSettings.Name = "tiPriceSettings"
        Me.tiPriceSettings.Text = "Market Price Settings"
        '
        'TabControlPanel3
        '
        Me.TabControlPanel3.Controls.Add(Me.adtLogs)
        Me.TabControlPanel3.Controls.Add(Me.panelLogSettings)
        Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel3.Name = "TabControlPanel3"
        Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel3.Size = New System.Drawing.Size(816, 683)
        Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel3.Style.GradientAngle = 90
        Me.TabControlPanel3.TabIndex = 3
        Me.TabControlPanel3.TabItem = Me.tiMarketLogs
        '
        'adtLogs
        '
        Me.adtLogs.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtLogs.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtLogs.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtLogs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtLogs.Columns.Add(Me.colRegion)
        Me.adtLogs.Columns.Add(Me.colItem)
        Me.adtLogs.Columns.Add(Me.colDate)
        Me.adtLogs.Columns.Add(Me.colAge)
        Me.adtLogs.ContextMenuStrip = Me.ctxMarketExport
        Me.adtLogs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtLogs.DragDropEnabled = False
        Me.adtLogs.DragDropNodeCopyEnabled = False
        Me.adtLogs.ExpandWidth = 0
        Me.adtLogs.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtLogs.Location = New System.Drawing.Point(1, 31)
        Me.adtLogs.MultiSelect = True
        Me.adtLogs.Name = "adtLogs"
        Me.adtLogs.NodesConnector = Me.NodeConnector1
        Me.adtLogs.NodeStyle = Me.Log
        Me.adtLogs.PathSeparator = ";"
        Me.adtLogs.Size = New System.Drawing.Size(814, 651)
        Me.adtLogs.Styles.Add(Me.Log)
        Me.adtLogs.TabIndex = 2
        '
        'colRegion
        '
        Me.colRegion.DisplayIndex = 1
        Me.colRegion.Name = "colRegion"
        Me.colRegion.Text = "Region"
        Me.colRegion.Width.Absolute = 200
        '
        'colItem
        '
        Me.colItem.DisplayIndex = 2
        Me.colItem.Name = "colItem"
        Me.colItem.Text = "Item Name"
        Me.colItem.Width.Absolute = 400
        '
        'colDate
        '
        Me.colDate.DisplayIndex = 3
        Me.colDate.Name = "colDate"
        Me.colDate.Text = "Log Date"
        Me.colDate.Width.Absolute = 120
        '
        'colAge
        '
        Me.colAge.DisplayIndex = 4
        Me.colAge.Name = "colAge"
        Me.colAge.Text = "Log Age (Hrs)"
        Me.colAge.Width.Absolute = 120
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        '
        'Log
        '
        Me.Log.Class = ""
        Me.Log.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Log.Name = "Log"
        Me.Log.TextColor = System.Drawing.SystemColors.ControlText
        '
        'panelLogSettings
        '
        Me.panelLogSettings.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelLogSettings.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelLogSettings.Controls.Add(Me.btnRefreshLogs)
        Me.panelLogSettings.Controls.Add(Me.nudAge)
        Me.panelLogSettings.Controls.Add(Me.lblHighlightLogsHours)
        Me.panelLogSettings.Controls.Add(Me.lblHighlightOldLogsText)
        Me.panelLogSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelLogSettings.Location = New System.Drawing.Point(1, 1)
        Me.panelLogSettings.Name = "panelLogSettings"
        Me.panelLogSettings.Size = New System.Drawing.Size(814, 30)
        Me.panelLogSettings.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelLogSettings.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelLogSettings.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelLogSettings.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelLogSettings.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelLogSettings.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelLogSettings.Style.GradientAngle = 90
        Me.panelLogSettings.TabIndex = 0
        '
        'tiMarketLogs
        '
        Me.tiMarketLogs.AttachedControl = Me.TabControlPanel3
        Me.tiMarketLogs.Name = "tiMarketLogs"
        Me.tiMarketLogs.Text = "Market Log Import"
        '
        'TabControlPanel5
        '
        Me.TabControlPanel5.Controls.Add(Me.gpMarketPrices)
        Me.TabControlPanel5.Controls.Add(Me.gpFactionPrices)
        Me.TabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel5.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel5.Name = "TabControlPanel5"
        Me.TabControlPanel5.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel5.Size = New System.Drawing.Size(816, 683)
        Me.TabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel5.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel5.Style.GradientAngle = 90
        Me.TabControlPanel5.TabIndex = 5
        Me.TabControlPanel5.TabItem = Me.tiPriceUpdates
        '
        'gpMarketPrices
        '
        Me.gpMarketPrices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gpMarketPrices.BackColor = System.Drawing.Color.Transparent
        Me.gpMarketPrices.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpMarketPrices.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpMarketPrices.Controls.Add(Me.lblEveMarketeerLink)
        Me.gpMarketPrices.Controls.Add(Me.radEveMarketeer)
        Me.gpMarketPrices.Controls.Add(Me.radBattleclinic)
        Me.gpMarketPrices.Controls.Add(Me.lblMarketSource)
        Me.gpMarketPrices.Controls.Add(Me.lblMarketCacheFiles)
        Me.gpMarketPrices.Controls.Add(Me.adtMarketCache)
        Me.gpMarketPrices.Controls.Add(Me.btnUpdateMarketPrices)
        Me.gpMarketPrices.Controls.Add(Me.lblMarketPriceUpdateStatus)
        Me.gpMarketPrices.Controls.Add(Me.btnDownloadMarketPrices)
        Me.gpMarketPrices.Controls.Add(Me.lblBattleclinicLink)
        Me.gpMarketPrices.Location = New System.Drawing.Point(3, 135)
        Me.gpMarketPrices.Name = "gpMarketPrices"
        Me.gpMarketPrices.Size = New System.Drawing.Size(606, 541)
        '
        '
        '
        Me.gpMarketPrices.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpMarketPrices.Style.BackColorGradientAngle = 90
        Me.gpMarketPrices.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpMarketPrices.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMarketPrices.Style.BorderBottomWidth = 1
        Me.gpMarketPrices.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpMarketPrices.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMarketPrices.Style.BorderLeftWidth = 1
        Me.gpMarketPrices.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMarketPrices.Style.BorderRightWidth = 1
        Me.gpMarketPrices.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMarketPrices.Style.BorderTopWidth = 1
        Me.gpMarketPrices.Style.Class = ""
        Me.gpMarketPrices.Style.CornerDiameter = 4
        Me.gpMarketPrices.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpMarketPrices.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpMarketPrices.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpMarketPrices.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpMarketPrices.StyleMouseDown.Class = ""
        Me.gpMarketPrices.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpMarketPrices.StyleMouseOver.Class = ""
        Me.gpMarketPrices.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpMarketPrices.TabIndex = 3
        Me.gpMarketPrices.Text = "Market Prices"
        '
        'lblEveMarketeerLink
        '
        Me.lblEveMarketeerLink.AutoSize = True
        Me.lblEveMarketeerLink.BackColor = System.Drawing.Color.Transparent
        Me.lblEveMarketeerLink.Location = New System.Drawing.Point(163, 54)
        Me.lblEveMarketeerLink.Name = "lblEveMarketeerLink"
        Me.lblEveMarketeerLink.Size = New System.Drawing.Size(157, 13)
        Me.lblEveMarketeerLink.TabIndex = 14
        Me.lblEveMarketeerLink.TabStop = True
        Me.lblEveMarketeerLink.Text = "http://www.evemarketeer.com"
        '
        'radEveMarketeer
        '
        Me.radEveMarketeer.AutoSize = True
        '
        '
        '
        Me.radEveMarketeer.BackgroundStyle.Class = ""
        Me.radEveMarketeer.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.radEveMarketeer.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.radEveMarketeer.Location = New System.Drawing.Point(34, 51)
        Me.radEveMarketeer.Name = "radEveMarketeer"
        Me.radEveMarketeer.Size = New System.Drawing.Size(92, 16)
        Me.radEveMarketeer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.radEveMarketeer.TabIndex = 13
        Me.radEveMarketeer.Text = "Eve Marketeer"
        '
        'radBattleclinic
        '
        Me.radBattleclinic.AutoSize = True
        '
        '
        '
        Me.radBattleclinic.BackgroundStyle.Class = ""
        Me.radBattleclinic.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.radBattleclinic.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.radBattleclinic.Checked = True
        Me.radBattleclinic.CheckState = System.Windows.Forms.CheckState.Checked
        Me.radBattleclinic.CheckValue = "Y"
        Me.radBattleclinic.Location = New System.Drawing.Point(34, 29)
        Me.radBattleclinic.Name = "radBattleclinic"
        Me.radBattleclinic.Size = New System.Drawing.Size(74, 16)
        Me.radBattleclinic.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.radBattleclinic.TabIndex = 12
        Me.radBattleclinic.Text = "Battleclinic"
        '
        'lblMarketSource
        '
        Me.lblMarketSource.AutoSize = True
        Me.lblMarketSource.BackColor = System.Drawing.Color.Transparent
        Me.lblMarketSource.Location = New System.Drawing.Point(6, 7)
        Me.lblMarketSource.Name = "lblMarketSource"
        Me.lblMarketSource.Size = New System.Drawing.Size(138, 13)
        Me.lblMarketSource.TabIndex = 11
        Me.lblMarketSource.Text = "Select Market Data Source:"
        '
        'lblMarketCacheFiles
        '
        Me.lblMarketCacheFiles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMarketCacheFiles.BackColor = System.Drawing.Color.Transparent
        Me.lblMarketCacheFiles.Location = New System.Drawing.Point(3, 154)
        Me.lblMarketCacheFiles.Name = "lblMarketCacheFiles"
        Me.lblMarketCacheFiles.Size = New System.Drawing.Size(583, 13)
        Me.lblMarketCacheFiles.TabIndex = 10
        Me.lblMarketCacheFiles.Text = "Existing Cache Files:"
        '
        'adtMarketCache
        '
        Me.adtMarketCache.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtMarketCache.AllowDrop = True
        Me.adtMarketCache.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.adtMarketCache.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtMarketCache.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtMarketCache.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtMarketCache.Columns.Add(Me.colRegionName)
        Me.adtMarketCache.Columns.Add(Me.colFileDate)
        Me.adtMarketCache.Columns.Add(Me.colCacheDate)
        Me.adtMarketCache.ExpandWidth = 0
        Me.adtMarketCache.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtMarketCache.Location = New System.Drawing.Point(3, 170)
        Me.adtMarketCache.Name = "adtMarketCache"
        Me.adtMarketCache.NodesConnector = Me.NodeConnector6
        Me.adtMarketCache.NodeStyle = Me.ElementStyle5
        Me.adtMarketCache.PathSeparator = ";"
        Me.adtMarketCache.Size = New System.Drawing.Size(594, 344)
        Me.adtMarketCache.Styles.Add(Me.ElementStyle5)
        Me.adtMarketCache.TabIndex = 9
        Me.adtMarketCache.Text = "AdvTree1"
        '
        'colRegionName
        '
        Me.colRegionName.Name = "colRegionName"
        Me.colRegionName.Text = "Region Name"
        Me.colRegionName.Width.Absolute = 300
        '
        'colFileDate
        '
        Me.colFileDate.Name = "colFileDate"
        Me.colFileDate.Text = "File Date"
        Me.colFileDate.Width.Absolute = 125
        '
        'colCacheDate
        '
        Me.colCacheDate.Name = "colCacheDate"
        Me.colCacheDate.Text = "Cache Expires"
        Me.colCacheDate.Width.Absolute = 125
        '
        'NodeConnector6
        '
        Me.NodeConnector6.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle5
        '
        Me.ElementStyle5.Class = ""
        Me.ElementStyle5.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle5.Name = "ElementStyle5"
        Me.ElementStyle5.TextColor = System.Drawing.SystemColors.ControlText
        '
        'btnUpdateMarketPrices
        '
        Me.btnUpdateMarketPrices.Location = New System.Drawing.Point(190, 90)
        Me.btnUpdateMarketPrices.Name = "btnUpdateMarketPrices"
        Me.btnUpdateMarketPrices.Size = New System.Drawing.Size(150, 23)
        Me.btnUpdateMarketPrices.TabIndex = 8
        Me.btnUpdateMarketPrices.Text = "Update Market Prices"
        Me.btnUpdateMarketPrices.UseVisualStyleBackColor = True
        '
        'gpFactionPrices
        '
        Me.gpFactionPrices.BackColor = System.Drawing.Color.Transparent
        Me.gpFactionPrices.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpFactionPrices.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpFactionPrices.Controls.Add(Me.lblLastFactionPriceUpdateLbl)
        Me.gpFactionPrices.Controls.Add(Me.lblFactionPriceUpdateStatus)
        Me.gpFactionPrices.Controls.Add(Me.lblLastFactionPriceUpdate)
        Me.gpFactionPrices.Controls.Add(Me.btnUpdateFactionPrices)
        Me.gpFactionPrices.Controls.Add(Me.lblFactionPricesByLbl)
        Me.gpFactionPrices.Controls.Add(Me.lblFactionPricesBy)
        Me.gpFactionPrices.Location = New System.Drawing.Point(4, 12)
        Me.gpFactionPrices.Name = "gpFactionPrices"
        Me.gpFactionPrices.Size = New System.Drawing.Size(606, 117)
        '
        '
        '
        Me.gpFactionPrices.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpFactionPrices.Style.BackColorGradientAngle = 90
        Me.gpFactionPrices.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpFactionPrices.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpFactionPrices.Style.BorderBottomWidth = 1
        Me.gpFactionPrices.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpFactionPrices.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpFactionPrices.Style.BorderLeftWidth = 1
        Me.gpFactionPrices.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpFactionPrices.Style.BorderRightWidth = 1
        Me.gpFactionPrices.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpFactionPrices.Style.BorderTopWidth = 1
        Me.gpFactionPrices.Style.Class = ""
        Me.gpFactionPrices.Style.CornerDiameter = 4
        Me.gpFactionPrices.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpFactionPrices.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpFactionPrices.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpFactionPrices.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpFactionPrices.StyleMouseDown.Class = ""
        Me.gpFactionPrices.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpFactionPrices.StyleMouseOver.Class = ""
        Me.gpFactionPrices.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpFactionPrices.TabIndex = 2
        Me.gpFactionPrices.Text = "Faction Prices"
        '
        'tiPriceUpdates
        '
        Me.tiPriceUpdates.AttachedControl = Me.TabControlPanel5
        Me.tiPriceUpdates.Name = "tiPriceUpdates"
        Me.tiPriceUpdates.Text = "Market Price Update"
        '
        'TabControlPanel6
        '
        Me.TabControlPanel6.Controls.Add(Me.gpSelectedPriceGroup)
        Me.TabControlPanel6.Controls.Add(Me.btnDeletePriceGroup)
        Me.TabControlPanel6.Controls.Add(Me.btnEditPriceGroup)
        Me.TabControlPanel6.Controls.Add(Me.btnAddPriceGroup)
        Me.TabControlPanel6.Controls.Add(Me.LabelX1)
        Me.TabControlPanel6.Controls.Add(Me.adtPriceGroups)
        Me.TabControlPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel6.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel6.Name = "TabControlPanel6"
        Me.TabControlPanel6.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel6.Size = New System.Drawing.Size(816, 683)
        Me.TabControlPanel6.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel6.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel6.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel6.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel6.Style.GradientAngle = 90
        Me.TabControlPanel6.TabIndex = 6
        Me.TabControlPanel6.TabItem = Me.tiPriceGroups
        '
        'gpSelectedPriceGroup
        '
        Me.gpSelectedPriceGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpSelectedPriceGroup.BackColor = System.Drawing.Color.Transparent
        Me.gpSelectedPriceGroup.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpSelectedPriceGroup.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpSelectedPriceGroup.Controls.Add(Me.btnClearItems)
        Me.gpSelectedPriceGroup.Controls.Add(Me.btnDeleteItem)
        Me.gpSelectedPriceGroup.Controls.Add(Me.btnAddItem)
        Me.gpSelectedPriceGroup.Controls.Add(Me.adtPriceGroupItems)
        Me.gpSelectedPriceGroup.Controls.Add(Me.lblTypeIDs)
        Me.gpSelectedPriceGroup.Controls.Add(Me.lblSelectedPriceGroup)
        Me.gpSelectedPriceGroup.Controls.Add(Me.lblRegions)
        Me.gpSelectedPriceGroup.Controls.Add(Me.adtPriceGroupRegions)
        Me.gpSelectedPriceGroup.Controls.Add(Me.GroupPanel1)
        Me.gpSelectedPriceGroup.Enabled = False
        Me.gpSelectedPriceGroup.Location = New System.Drawing.Point(247, 26)
        Me.gpSelectedPriceGroup.Name = "gpSelectedPriceGroup"
        Me.gpSelectedPriceGroup.Size = New System.Drawing.Size(565, 653)
        '
        '
        '
        Me.gpSelectedPriceGroup.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpSelectedPriceGroup.Style.BackColorGradientAngle = 90
        Me.gpSelectedPriceGroup.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpSelectedPriceGroup.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSelectedPriceGroup.Style.BorderBottomWidth = 1
        Me.gpSelectedPriceGroup.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpSelectedPriceGroup.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSelectedPriceGroup.Style.BorderLeftWidth = 1
        Me.gpSelectedPriceGroup.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSelectedPriceGroup.Style.BorderRightWidth = 1
        Me.gpSelectedPriceGroup.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSelectedPriceGroup.Style.BorderTopWidth = 1
        Me.gpSelectedPriceGroup.Style.Class = ""
        Me.gpSelectedPriceGroup.Style.CornerDiameter = 4
        Me.gpSelectedPriceGroup.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpSelectedPriceGroup.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpSelectedPriceGroup.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpSelectedPriceGroup.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpSelectedPriceGroup.StyleMouseDown.Class = ""
        Me.gpSelectedPriceGroup.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpSelectedPriceGroup.StyleMouseOver.Class = ""
        Me.gpSelectedPriceGroup.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpSelectedPriceGroup.TabIndex = 30
        Me.gpSelectedPriceGroup.Text = "Group Price Details"
        '
        'btnClearItems
        '
        Me.btnClearItems.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnClearItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearItems.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnClearItems.Location = New System.Drawing.Point(352, 603)
        Me.btnClearItems.Name = "btnClearItems"
        Me.btnClearItems.Size = New System.Drawing.Size(75, 23)
        Me.btnClearItems.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnClearItems.TabIndex = 34
        Me.btnClearItems.Text = "Clear All"
        '
        'btnDeleteItem
        '
        Me.btnDeleteItem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnDeleteItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteItem.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnDeleteItem.Enabled = False
        Me.btnDeleteItem.Location = New System.Drawing.Point(271, 603)
        Me.btnDeleteItem.Name = "btnDeleteItem"
        Me.btnDeleteItem.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnDeleteItem.TabIndex = 33
        Me.btnDeleteItem.Text = "Delete Item"
        '
        'btnAddItem
        '
        Me.btnAddItem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAddItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddItem.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAddItem.Location = New System.Drawing.Point(190, 603)
        Me.btnAddItem.Name = "btnAddItem"
        Me.btnAddItem.Size = New System.Drawing.Size(75, 23)
        Me.btnAddItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAddItem.TabIndex = 32
        Me.btnAddItem.Text = "Add Item"
        '
        'adtPriceGroupItems
        '
        Me.adtPriceGroupItems.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtPriceGroupItems.AllowDrop = True
        Me.adtPriceGroupItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtPriceGroupItems.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtPriceGroupItems.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtPriceGroupItems.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtPriceGroupItems.Columns.Add(Me.colPriceItemName)
        Me.adtPriceGroupItems.DragDropEnabled = False
        Me.adtPriceGroupItems.DragDropNodeCopyEnabled = False
        Me.adtPriceGroupItems.ExpandWidth = 0
        Me.adtPriceGroupItems.GridRowLines = True
        Me.adtPriceGroupItems.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtPriceGroupItems.Location = New System.Drawing.Point(190, 173)
        Me.adtPriceGroupItems.MultiSelect = True
        Me.adtPriceGroupItems.Name = "adtPriceGroupItems"
        Me.adtPriceGroupItems.NodesConnector = Me.NodeConnector4
        Me.adtPriceGroupItems.NodeStyle = Me.ElementStyle3
        Me.adtPriceGroupItems.PathSeparator = ";"
        Me.adtPriceGroupItems.SearchBufferExpireTimeout = 3000
        Me.adtPriceGroupItems.Size = New System.Drawing.Size(364, 424)
        Me.adtPriceGroupItems.Styles.Add(Me.ElementStyle3)
        Me.adtPriceGroupItems.TabIndex = 31
        Me.adtPriceGroupItems.Text = "AdvTree1"
        '
        'colPriceItemName
        '
        Me.colPriceItemName.Name = "colPriceItemName"
        Me.colPriceItemName.Text = "Item Name"
        Me.colPriceItemName.Width.Absolute = 330
        '
        'NodeConnector4
        '
        Me.NodeConnector4.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle3
        '
        Me.ElementStyle3.Class = ""
        Me.ElementStyle3.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle3.Name = "ElementStyle3"
        Me.ElementStyle3.TextColor = System.Drawing.SystemColors.ControlText
        '
        'lblTypeIDs
        '
        Me.lblTypeIDs.AutoSize = True
        Me.lblTypeIDs.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblTypeIDs.BackgroundStyle.Class = ""
        Me.lblTypeIDs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblTypeIDs.Location = New System.Drawing.Point(190, 151)
        Me.lblTypeIDs.Name = "lblTypeIDs"
        Me.lblTypeIDs.Size = New System.Drawing.Size(78, 16)
        Me.lblTypeIDs.TabIndex = 30
        Me.lblTypeIDs.Text = "Selected Items:"
        '
        'lblSelectedPriceGroup
        '
        Me.lblSelectedPriceGroup.AutoSize = True
        Me.lblSelectedPriceGroup.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblSelectedPriceGroup.BackgroundStyle.Class = ""
        Me.lblSelectedPriceGroup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblSelectedPriceGroup.Location = New System.Drawing.Point(3, 3)
        Me.lblSelectedPriceGroup.Name = "lblSelectedPriceGroup"
        Me.lblSelectedPriceGroup.Size = New System.Drawing.Size(150, 16)
        Me.lblSelectedPriceGroup.TabIndex = 26
        Me.lblSelectedPriceGroup.Text = "Selected Price Group: <none>"
        '
        'lblRegions
        '
        Me.lblRegions.AutoSize = True
        Me.lblRegions.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblRegions.BackgroundStyle.Class = ""
        Me.lblRegions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblRegions.Location = New System.Drawing.Point(3, 151)
        Me.lblRegions.Name = "lblRegions"
        Me.lblRegions.Size = New System.Drawing.Size(89, 16)
        Me.lblRegions.TabIndex = 29
        Me.lblRegions.Text = "Selected Regions:"
        '
        'adtPriceGroupRegions
        '
        Me.adtPriceGroupRegions.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtPriceGroupRegions.AllowDrop = True
        Me.adtPriceGroupRegions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.adtPriceGroupRegions.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtPriceGroupRegions.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtPriceGroupRegions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtPriceGroupRegions.Columns.Add(Me.colPriceRegions)
        Me.adtPriceGroupRegions.DragDropEnabled = False
        Me.adtPriceGroupRegions.DragDropNodeCopyEnabled = False
        Me.adtPriceGroupRegions.ExpandWidth = 0
        Me.adtPriceGroupRegions.GridRowLines = True
        Me.adtPriceGroupRegions.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtPriceGroupRegions.Location = New System.Drawing.Point(3, 173)
        Me.adtPriceGroupRegions.Name = "adtPriceGroupRegions"
        Me.adtPriceGroupRegions.NodesConnector = Me.NodeConnector3
        Me.adtPriceGroupRegions.NodeStyle = Me.ElementStyle2
        Me.adtPriceGroupRegions.PathSeparator = ";"
        Me.adtPriceGroupRegions.Size = New System.Drawing.Size(178, 453)
        Me.adtPriceGroupRegions.Styles.Add(Me.ElementStyle2)
        Me.adtPriceGroupRegions.TabIndex = 28
        Me.adtPriceGroupRegions.Text = "AdvTree1"
        '
        'colPriceRegions
        '
        Me.colPriceRegions.Name = "colPriceRegions"
        Me.colPriceRegions.Text = "Region Name"
        Me.colPriceRegions.Width.Absolute = 155
        '
        'NodeConnector3
        '
        Me.NodeConnector3.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle2
        '
        Me.ElementStyle2.Class = ""
        Me.ElementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle2.Name = "ElementStyle2"
        Me.ElementStyle2.TextColor = System.Drawing.SystemColors.ControlText
        '
        'GroupPanel1
        '
        Me.GroupPanel1.BackColor = System.Drawing.Color.Transparent
        Me.GroupPanel1.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.GroupPanel1.Controls.Add(Me.chkPGMedBuy)
        Me.GroupPanel1.Controls.Add(Me.chkPGMedAll)
        Me.GroupPanel1.Controls.Add(Me.chkPGAvgBuy)
        Me.GroupPanel1.Controls.Add(Me.chkPGMinAll)
        Me.GroupPanel1.Controls.Add(Me.chkPGMaxBuy)
        Me.GroupPanel1.Controls.Add(Me.chkPGMaxAll)
        Me.GroupPanel1.Controls.Add(Me.chkPGMinBuy)
        Me.GroupPanel1.Controls.Add(Me.chkPGAvgAll)
        Me.GroupPanel1.Controls.Add(Me.chkPGMedSell)
        Me.GroupPanel1.Controls.Add(Me.chkPGAvgSell)
        Me.GroupPanel1.Controls.Add(Me.chkPGMinSell)
        Me.GroupPanel1.Controls.Add(Me.chkPGMaxSell)
        Me.GroupPanel1.Location = New System.Drawing.Point(3, 25)
        Me.GroupPanel1.Name = "GroupPanel1"
        Me.GroupPanel1.Size = New System.Drawing.Size(363, 120)
        '
        '
        '
        Me.GroupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.GroupPanel1.Style.BackColorGradientAngle = 90
        Me.GroupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.GroupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderBottomWidth = 1
        Me.GroupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderLeftWidth = 1
        Me.GroupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderRightWidth = 1
        Me.GroupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderTopWidth = 1
        Me.GroupPanel1.Style.Class = ""
        Me.GroupPanel1.Style.CornerDiameter = 4
        Me.GroupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.GroupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.GroupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel1.StyleMouseDown.Class = ""
        Me.GroupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel1.StyleMouseOver.Class = ""
        Me.GroupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel1.TabIndex = 27
        Me.GroupPanel1.Text = "Price Criteria Flags"
        '
        'chkPGMedBuy
        '
        Me.chkPGMedBuy.AutoSize = True
        Me.chkPGMedBuy.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMedBuy.Location = New System.Drawing.Point(117, 72)
        Me.chkPGMedBuy.Name = "chkPGMedBuy"
        Me.chkPGMedBuy.Size = New System.Drawing.Size(115, 17)
        Me.chkPGMedBuy.TabIndex = 3
        Me.chkPGMedBuy.Tag = "1024"
        Me.chkPGMedBuy.Text = "Median Price (Buy)"
        Me.chkPGMedBuy.UseVisualStyleBackColor = False
        '
        'chkPGMedAll
        '
        Me.chkPGMedAll.AutoSize = True
        Me.chkPGMedAll.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMedAll.Location = New System.Drawing.Point(3, 72)
        Me.chkPGMedAll.Name = "chkPGMedAll"
        Me.chkPGMedAll.Size = New System.Drawing.Size(108, 17)
        Me.chkPGMedAll.TabIndex = 11
        Me.chkPGMedAll.Tag = "512"
        Me.chkPGMedAll.Text = "Median Price (All)"
        Me.chkPGMedAll.UseVisualStyleBackColor = False
        '
        'chkPGAvgBuy
        '
        Me.chkPGAvgBuy.AutoSize = True
        Me.chkPGAvgBuy.BackColor = System.Drawing.Color.Transparent
        Me.chkPGAvgBuy.Location = New System.Drawing.Point(117, 49)
        Me.chkPGAvgBuy.Name = "chkPGAvgBuy"
        Me.chkPGAvgBuy.Size = New System.Drawing.Size(107, 17)
        Me.chkPGAvgBuy.TabIndex = 0
        Me.chkPGAvgBuy.Tag = "128"
        Me.chkPGAvgBuy.Text = "Mean Price (Buy)"
        Me.chkPGAvgBuy.UseVisualStyleBackColor = False
        '
        'chkPGMinAll
        '
        Me.chkPGMinAll.AutoSize = True
        Me.chkPGMinAll.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMinAll.Location = New System.Drawing.Point(3, 3)
        Me.chkPGMinAll.Name = "chkPGMinAll"
        Me.chkPGMinAll.Size = New System.Drawing.Size(90, 17)
        Me.chkPGMinAll.TabIndex = 10
        Me.chkPGMinAll.Tag = "1"
        Me.chkPGMinAll.Text = "Min Price (All)"
        Me.chkPGMinAll.UseVisualStyleBackColor = False
        '
        'chkPGMaxBuy
        '
        Me.chkPGMaxBuy.AutoSize = True
        Me.chkPGMaxBuy.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMaxBuy.Location = New System.Drawing.Point(117, 26)
        Me.chkPGMaxBuy.Name = "chkPGMaxBuy"
        Me.chkPGMaxBuy.Size = New System.Drawing.Size(101, 17)
        Me.chkPGMaxBuy.TabIndex = 1
        Me.chkPGMaxBuy.Tag = "16"
        Me.chkPGMaxBuy.Text = "Max Price (Buy)"
        Me.chkPGMaxBuy.UseVisualStyleBackColor = False
        '
        'chkPGMaxAll
        '
        Me.chkPGMaxAll.AutoSize = True
        Me.chkPGMaxAll.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMaxAll.Location = New System.Drawing.Point(3, 26)
        Me.chkPGMaxAll.Name = "chkPGMaxAll"
        Me.chkPGMaxAll.Size = New System.Drawing.Size(94, 17)
        Me.chkPGMaxAll.TabIndex = 9
        Me.chkPGMaxAll.Tag = "8"
        Me.chkPGMaxAll.Text = "Max Price (All)"
        Me.chkPGMaxAll.UseVisualStyleBackColor = False
        '
        'chkPGMinBuy
        '
        Me.chkPGMinBuy.AutoSize = True
        Me.chkPGMinBuy.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMinBuy.Location = New System.Drawing.Point(117, 3)
        Me.chkPGMinBuy.Name = "chkPGMinBuy"
        Me.chkPGMinBuy.Size = New System.Drawing.Size(97, 17)
        Me.chkPGMinBuy.TabIndex = 2
        Me.chkPGMinBuy.Tag = "2"
        Me.chkPGMinBuy.Text = "Min Price (Buy)"
        Me.chkPGMinBuy.UseVisualStyleBackColor = False
        '
        'chkPGAvgAll
        '
        Me.chkPGAvgAll.AutoSize = True
        Me.chkPGAvgAll.BackColor = System.Drawing.Color.Transparent
        Me.chkPGAvgAll.Location = New System.Drawing.Point(3, 49)
        Me.chkPGAvgAll.Name = "chkPGAvgAll"
        Me.chkPGAvgAll.Size = New System.Drawing.Size(100, 17)
        Me.chkPGAvgAll.TabIndex = 8
        Me.chkPGAvgAll.Tag = "64"
        Me.chkPGAvgAll.Text = "Mean Price (All)"
        Me.chkPGAvgAll.UseVisualStyleBackColor = False
        '
        'chkPGMedSell
        '
        Me.chkPGMedSell.AutoSize = True
        Me.chkPGMedSell.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMedSell.Location = New System.Drawing.Point(238, 72)
        Me.chkPGMedSell.Name = "chkPGMedSell"
        Me.chkPGMedSell.Size = New System.Drawing.Size(113, 17)
        Me.chkPGMedSell.TabIndex = 7
        Me.chkPGMedSell.Tag = "2048"
        Me.chkPGMedSell.Text = "Median Price (Sell)"
        Me.chkPGMedSell.UseVisualStyleBackColor = False
        '
        'chkPGAvgSell
        '
        Me.chkPGAvgSell.AutoSize = True
        Me.chkPGAvgSell.BackColor = System.Drawing.Color.Transparent
        Me.chkPGAvgSell.Location = New System.Drawing.Point(238, 49)
        Me.chkPGAvgSell.Name = "chkPGAvgSell"
        Me.chkPGAvgSell.Size = New System.Drawing.Size(105, 17)
        Me.chkPGAvgSell.TabIndex = 4
        Me.chkPGAvgSell.Tag = "256"
        Me.chkPGAvgSell.Text = "Mean Price (Sell)"
        Me.chkPGAvgSell.UseVisualStyleBackColor = False
        '
        'chkPGMinSell
        '
        Me.chkPGMinSell.AutoSize = True
        Me.chkPGMinSell.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMinSell.Location = New System.Drawing.Point(238, 3)
        Me.chkPGMinSell.Name = "chkPGMinSell"
        Me.chkPGMinSell.Size = New System.Drawing.Size(95, 17)
        Me.chkPGMinSell.TabIndex = 6
        Me.chkPGMinSell.Tag = "4"
        Me.chkPGMinSell.Text = "Min Price (Sell)"
        Me.chkPGMinSell.UseVisualStyleBackColor = False
        '
        'chkPGMaxSell
        '
        Me.chkPGMaxSell.AutoSize = True
        Me.chkPGMaxSell.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMaxSell.Location = New System.Drawing.Point(238, 26)
        Me.chkPGMaxSell.Name = "chkPGMaxSell"
        Me.chkPGMaxSell.Size = New System.Drawing.Size(99, 17)
        Me.chkPGMaxSell.TabIndex = 5
        Me.chkPGMaxSell.Tag = "32"
        Me.chkPGMaxSell.Text = "Max Price (Sell)"
        Me.chkPGMaxSell.UseVisualStyleBackColor = False
        '
        'btnDeletePriceGroup
        '
        Me.btnDeletePriceGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnDeletePriceGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeletePriceGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnDeletePriceGroup.Location = New System.Drawing.Point(166, 656)
        Me.btnDeletePriceGroup.Name = "btnDeletePriceGroup"
        Me.btnDeletePriceGroup.Size = New System.Drawing.Size(75, 23)
        Me.btnDeletePriceGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnDeletePriceGroup.TabIndex = 25
        Me.btnDeletePriceGroup.Text = "Delete Group"
        '
        'btnEditPriceGroup
        '
        Me.btnEditPriceGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnEditPriceGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditPriceGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnEditPriceGroup.Location = New System.Drawing.Point(85, 656)
        Me.btnEditPriceGroup.Name = "btnEditPriceGroup"
        Me.btnEditPriceGroup.Size = New System.Drawing.Size(75, 23)
        Me.btnEditPriceGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnEditPriceGroup.TabIndex = 24
        Me.btnEditPriceGroup.Text = "Edit Group"
        '
        'btnAddPriceGroup
        '
        Me.btnAddPriceGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAddPriceGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddPriceGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAddPriceGroup.Location = New System.Drawing.Point(4, 656)
        Me.btnAddPriceGroup.Name = "btnAddPriceGroup"
        Me.btnAddPriceGroup.Size = New System.Drawing.Size(75, 23)
        Me.btnAddPriceGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAddPriceGroup.TabIndex = 23
        Me.btnAddPriceGroup.Text = "Add Group"
        '
        'LabelX1
        '
        Me.LabelX1.AutoSize = True
        Me.LabelX1.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX1.BackgroundStyle.Class = ""
        Me.LabelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX1.Location = New System.Drawing.Point(4, 4)
        Me.LabelX1.Name = "LabelX1"
        Me.LabelX1.Size = New System.Drawing.Size(109, 16)
        Me.LabelX1.TabIndex = 22
        Me.LabelX1.Text = "Existing Price Groups:"
        '
        'adtPriceGroups
        '
        Me.adtPriceGroups.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtPriceGroups.AllowDrop = True
        Me.adtPriceGroups.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.adtPriceGroups.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtPriceGroups.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtPriceGroups.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtPriceGroups.Columns.Add(Me.colPriceGroupName)
        Me.adtPriceGroups.ExpandWidth = 0
        Me.adtPriceGroups.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtPriceGroups.Location = New System.Drawing.Point(4, 26)
        Me.adtPriceGroups.Name = "adtPriceGroups"
        Me.adtPriceGroups.NodesConnector = Me.NodeConnector2
        Me.adtPriceGroups.NodeStyle = Me.ElementStyle1
        Me.adtPriceGroups.PathSeparator = ";"
        Me.adtPriceGroups.Size = New System.Drawing.Size(237, 624)
        Me.adtPriceGroups.Styles.Add(Me.ElementStyle1)
        Me.adtPriceGroups.TabIndex = 21
        Me.adtPriceGroups.Text = "AdvTree1"
        '
        'colPriceGroupName
        '
        Me.colPriceGroupName.Name = "colPriceGroupName"
        Me.colPriceGroupName.Text = "Price Group Name"
        Me.colPriceGroupName.Width.Absolute = 215
        '
        'NodeConnector2
        '
        Me.NodeConnector2.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle1
        '
        Me.ElementStyle1.Class = ""
        Me.ElementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle1.Name = "ElementStyle1"
        Me.ElementStyle1.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiPriceGroups
        '
        Me.tiPriceGroups.AttachedControl = Me.TabControlPanel6
        Me.tiPriceGroups.Name = "tiPriceGroups"
        Me.tiPriceGroups.Text = "Price Groups"
        '
        'TabControlPanel4
        '
        Me.TabControlPanel4.Controls.Add(Me.adtPrices)
        Me.TabControlPanel4.Controls.Add(Me.chkShowOnlyCustom)
        Me.TabControlPanel4.Controls.Add(Me.lblCustomPrices)
        Me.TabControlPanel4.Controls.Add(Me.lblSearchPrices)
        Me.TabControlPanel4.Controls.Add(Me.txtSearchPrices)
        Me.TabControlPanel4.Controls.Add(Me.btnResetGrid)
        Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel4.Name = "TabControlPanel4"
        Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel4.Size = New System.Drawing.Size(816, 683)
        Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel4.Style.GradientAngle = 90
        Me.TabControlPanel4.TabIndex = 4
        Me.TabControlPanel4.TabItem = Me.tiCustomPrices
        '
        'adtPrices
        '
        Me.adtPrices.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtPrices.AllowDrop = True
        Me.adtPrices.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtPrices.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtPrices.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtPrices.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtPrices.Columns.Add(Me.colCustomItem)
        Me.adtPrices.Columns.Add(Me.colCustomBase)
        Me.adtPrices.Columns.Add(Me.colCustomMarket)
        Me.adtPrices.Columns.Add(Me.colCustomCustom)
        Me.adtPrices.ContextMenuStrip = Me.ctxPrices
        Me.adtPrices.ExpandWidth = 0
        Me.adtPrices.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtPrices.Location = New System.Drawing.Point(4, 39)
        Me.adtPrices.MultiSelect = True
        Me.adtPrices.Name = "adtPrices"
        Me.adtPrices.NodesConnector = Me.NodeConnector5
        Me.adtPrices.NodeSpacing = 1
        Me.adtPrices.NodeStyle = Me.ElementStyle4
        Me.adtPrices.PathSeparator = ";"
        Me.adtPrices.Size = New System.Drawing.Size(808, 606)
        Me.adtPrices.Styles.Add(Me.ElementStyle4)
        Me.adtPrices.TabIndex = 21
        Me.adtPrices.Text = "AdvTree1"
        '
        'colCustomItem
        '
        Me.colCustomItem.DisplayIndex = 1
        Me.colCustomItem.Name = "colCustomItem"
        Me.colCustomItem.Text = "Item Name"
        Me.colCustomItem.Width.Absolute = 300
        '
        'colCustomBase
        '
        Me.colCustomBase.DisplayIndex = 2
        Me.colCustomBase.Name = "colCustomBase"
        Me.colCustomBase.Text = "Base Price"
        Me.colCustomBase.Width.Absolute = 150
        '
        'colCustomMarket
        '
        Me.colCustomMarket.DisplayIndex = 3
        Me.colCustomMarket.Name = "colCustomMarket"
        Me.colCustomMarket.Text = "Market Price"
        Me.colCustomMarket.Width.Absolute = 150
        '
        'colCustomCustom
        '
        Me.colCustomCustom.DisplayIndex = 4
        Me.colCustomCustom.Name = "colCustomCustom"
        Me.colCustomCustom.Text = "Custom Price"
        Me.colCustomCustom.Width.Absolute = 150
        '
        'NodeConnector5
        '
        Me.NodeConnector5.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle4
        '
        Me.ElementStyle4.Class = ""
        Me.ElementStyle4.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle4.Name = "ElementStyle4"
        Me.ElementStyle4.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiCustomPrices
        '
        Me.tiCustomPrices.AttachedControl = Me.TabControlPanel4
        Me.tiCustomPrices.Name = "tiCustomPrices"
        Me.tiCustomPrices.Text = "Custom Prices"
        '
        'btnRefreshLogs
        '
        Me.btnRefreshLogs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnRefreshLogs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnRefreshLogs.Location = New System.Drawing.Point(266, 4)
        Me.btnRefreshLogs.Name = "btnRefreshLogs"
        Me.btnRefreshLogs.Size = New System.Drawing.Size(100, 23)
        Me.btnRefreshLogs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnRefreshLogs.TabIndex = 5
        Me.btnRefreshLogs.Text = "Refresh Logs"
        '
        'frmMarketPrices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(816, 706)
        Me.Controls.Add(Me.TabControl1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketPrices"
        Me.Text = "Market Prices"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxMarketExport.ResumeLayout(False)
        CType(Me.nudAge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxPrices.ResumeLayout(False)
        CType(Me.TabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        Me.gpLogParsing.ResumeLayout(False)
        Me.gpLogParsing.PerformLayout()
        Me.gpMLWOptions.ResumeLayout(False)
        Me.gpMLWOptions.PerformLayout()
        Me.TabControlPanel3.ResumeLayout(False)
        CType(Me.adtLogs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelLogSettings.ResumeLayout(False)
        Me.panelLogSettings.PerformLayout()
        Me.TabControlPanel5.ResumeLayout(False)
        Me.gpMarketPrices.ResumeLayout(False)
        Me.gpMarketPrices.PerformLayout()
        CType(Me.adtMarketCache, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gpFactionPrices.ResumeLayout(False)
        Me.gpFactionPrices.PerformLayout()
        Me.TabControlPanel6.ResumeLayout(False)
        Me.TabControlPanel6.PerformLayout()
        Me.gpSelectedPriceGroup.ResumeLayout(False)
        Me.gpSelectedPriceGroup.PerformLayout()
        CType(Me.adtPriceGroupItems, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.adtPriceGroupRegions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupPanel1.ResumeLayout(False)
        Me.GroupPanel1.PerformLayout()
        CType(Me.adtPriceGroups, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel4.ResumeLayout(False)
        Me.TabControlPanel4.PerformLayout()
        CType(Me.adtPrices, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents chkEnableLogWatcher As System.Windows.Forms.CheckBox
    Friend WithEvents chkNotifyPopup As System.Windows.Forms.CheckBox
    Friend WithEvents chkNotifyTray As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoUpdatePriceData As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoUpdateCurrentPrice As System.Windows.Forms.CheckBox
    Friend WithEvents chkEnableWatcherAtStartup As System.Windows.Forms.CheckBox
    Friend WithEvents chkIgnoreBuyOrders As System.Windows.Forms.CheckBox
    Friend WithEvents nudIgnoreBuyOrderLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudIgnoreSellOrderLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkIgnoreSellOrders As System.Windows.Forms.CheckBox
    Friend WithEvents lblIgnoreBuyOrderUnit As System.Windows.Forms.Label
    Friend WithEvents lblIgnoreSellOrderUnit As System.Windows.Forms.Label
    Friend WithEvents lblCustomPrices As System.Windows.Forms.Label
    Friend WithEvents btnResetGrid As System.Windows.Forms.Button
    Friend WithEvents txtSearchPrices As System.Windows.Forms.TextBox
    Friend WithEvents lblSearchPrices As System.Windows.Forms.Label
    Friend WithEvents ctxPrices As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPriceItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPriceEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkShowOnlyCustom As System.Windows.Forms.CheckBox
    Friend WithEvents ctxMarketExport As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuViewOrders As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblFactionPricesBy As System.Windows.Forms.LinkLabel
    Friend WithEvents lblFactionPricesByLbl As System.Windows.Forms.Label
    Friend WithEvents lblLastFactionPriceUpdate As System.Windows.Forms.Label
    Friend WithEvents lblLastFactionPriceUpdateLbl As System.Windows.Forms.Label
    Friend WithEvents btnUpdateFactionPrices As System.Windows.Forms.Button
    Friend WithEvents lblFactionPriceUpdateStatus As System.Windows.Forms.Label
    Friend WithEvents lblMarketPriceUpdateStatus As System.Windows.Forms.Label
    Friend WithEvents btnDownloadMarketPrices As System.Windows.Forms.Button
    Friend WithEvents lblBattleclinicLink As System.Windows.Forms.LinkLabel
    Friend WithEvents lblHighlightOldLogsText As System.Windows.Forms.Label
    Friend WithEvents mnuDeleteLog As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblHighlightLogsHours As System.Windows.Forms.Label
    Friend WithEvents nudAge As System.Windows.Forms.NumericUpDown
    Friend WithEvents TabControl1 As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiPriceSettings As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel5 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents gpMarketPrices As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents gpFactionPrices As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents tiPriceUpdates As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiCustomPrices As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiMarketLogs As DevComponents.DotNetBar.TabItem
    Friend WithEvents gpMLWOptions As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents panelLogSettings As DevComponents.DotNetBar.PanelEx
    Friend WithEvents gpLogParsing As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnDropNewTables As System.Windows.Forms.Button
    Friend WithEvents adtLogs As DevComponents.AdvTree.AdvTree
    Friend WithEvents colRegion As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colDate As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colAge As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents Log As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents TabControlPanel6 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents btnDeletePriceGroup As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnEditPriceGroup As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAddPriceGroup As DevComponents.DotNetBar.ButtonX
    Friend WithEvents LabelX1 As DevComponents.DotNetBar.LabelX
    Friend WithEvents adtPriceGroups As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector2 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle1 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents tiPriceGroups As DevComponents.DotNetBar.TabItem
    Friend WithEvents lblSelectedPriceGroup As DevComponents.DotNetBar.LabelX
    Friend WithEvents colPriceGroupName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents GroupPanel1 As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents chkPGMedAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGAvgBuy As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMinAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMaxBuy As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMaxAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMinBuy As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGAvgAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMedBuy As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMedSell As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGAvgSell As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMinSell As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMaxSell As System.Windows.Forms.CheckBox
    Friend WithEvents gpSelectedPriceGroup As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lblRegions As DevComponents.DotNetBar.LabelX
    Friend WithEvents adtPriceGroupRegions As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector3 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle2 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents adtPriceGroupItems As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector4 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle3 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents lblTypeIDs As DevComponents.DotNetBar.LabelX
    Friend WithEvents btnDeleteItem As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAddItem As DevComponents.DotNetBar.ButtonX
    Friend WithEvents colPriceItemName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colPriceRegions As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents btnClearItems As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnUpdateMarketPrices As System.Windows.Forms.Button
    Friend WithEvents adtPrices As DevComponents.AdvTree.AdvTree
    Friend WithEvents colCustomItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomBase As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomMarket As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomCustom As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector5 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle4 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents lblMarketCacheFiles As System.Windows.Forms.Label
    Friend WithEvents adtMarketCache As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector6 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle5 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colRegionName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colFileDate As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCacheDate As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents lblEveMarketeerLink As System.Windows.Forms.LinkLabel
    Friend WithEvents radEveMarketeer As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents radBattleclinic As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents lblMarketSource As System.Windows.Forms.Label
    Friend WithEvents btnRefreshLogs As DevComponents.DotNetBar.ButtonX
End Class
