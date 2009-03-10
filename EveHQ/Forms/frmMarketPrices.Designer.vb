<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMarketPrices
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMarketPrices))
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.lblProgress = New System.Windows.Forms.Label
        Me.grpRegions = New System.Windows.Forms.GroupBox
        Me.btnAllRegions = New System.Windows.Forms.Button
        Me.btnEmpireRegions = New System.Windows.Forms.Button
        Me.btnNullRegions = New System.Windows.Forms.Button
        Me.btnNoRegions = New System.Windows.Forms.Button
        Me.tabMarketPrices = New System.Windows.Forms.TabControl
        Me.tabPriceSettings = New System.Windows.Forms.TabPage
        Me.panelPrices = New System.Windows.Forms.Panel
        Me.grpMarketLogWatcher = New System.Windows.Forms.GroupBox
        Me.chkEnableLogWatcher = New System.Windows.Forms.CheckBox
        Me.chkEnableWatcherAtStartup = New System.Windows.Forms.CheckBox
        Me.chkNotifyTray = New System.Windows.Forms.CheckBox
        Me.chkAutoUpdatePriceData = New System.Windows.Forms.CheckBox
        Me.chkNotifyPopup = New System.Windows.Forms.CheckBox
        Me.chkAutoUpdateCurrentPrice = New System.Windows.Forms.CheckBox
        Me.grpParsing = New System.Windows.Forms.GroupBox
        Me.nudIgnoreBuyOrderLimit = New System.Windows.Forms.NumericUpDown
        Me.nudIgnoreSellOrderLimit = New System.Windows.Forms.NumericUpDown
        Me.chkIgnoreBuyOrders = New System.Windows.Forms.CheckBox
        Me.chkIgnoreSellOrders = New System.Windows.Forms.CheckBox
        Me.lblIgnoreSellOrderUnit = New System.Windows.Forms.Label
        Me.lblIgnoreBuyOrderUnit = New System.Windows.Forms.Label
        Me.grpCriteria = New System.Windows.Forms.GroupBox
        Me.chkPriceCriteria9 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria10 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria11 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria8 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria5 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria6 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria7 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria4 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria1 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria2 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria3 = New System.Windows.Forms.CheckBox
        Me.chkPriceCriteria0 = New System.Windows.Forms.CheckBox
        Me.btnAmarr = New System.Windows.Forms.Button
        Me.btnCaldari = New System.Windows.Forms.Button
        Me.btnMinmatar = New System.Windows.Forms.Button
        Me.btnGallente = New System.Windows.Forms.Button
        Me.tabDumps = New System.Windows.Forms.TabPage
        Me.panelECDumps = New System.Windows.Forms.Panel
        Me.gbProcessStatus = New System.Windows.Forms.GroupBox
        Me.pbProgress = New System.Windows.Forms.ProgressBar
        Me.lblProcess = New System.Windows.Forms.Label
        Me.tabMarketLogs = New System.Windows.Forms.TabPage
        Me.panelMarketLog = New System.Windows.Forms.Panel
        Me.clvLogs = New DotNetLib.Windows.Forms.ContainerListView
        Me.colRegion = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colItem = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDate = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ctxMarketExport = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuViewOrders = New System.Windows.Forms.ToolStripMenuItem
        Me.tabCustom = New System.Windows.Forms.TabPage
        Me.panelCustom = New System.Windows.Forms.Panel
        Me.chkShowOnlyCustom = New System.Windows.Forms.CheckBox
        Me.lblCustomPrices = New System.Windows.Forms.Label
        Me.btnResetGrid = New System.Windows.Forms.Button
        Me.txtSearchPrices = New System.Windows.Forms.TextBox
        Me.lblSearchPrices = New System.Windows.Forms.Label
        Me.lvwPrices = New EveHQ.ListViewNoFlicker
        Me.colPriceName = New System.Windows.Forms.ColumnHeader
        Me.colBasePrice = New System.Windows.Forms.ColumnHeader
        Me.colMarketPrice = New System.Windows.Forms.ColumnHeader
        Me.colCustomPrice = New System.Windows.Forms.ColumnHeader
        Me.ctxPrices = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPriceItemName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPriceAdd = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPriceEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPriceDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.tabSnapshot = New System.Windows.Forms.TabPage
        Me.panelSnapshot = New System.Windows.Forms.Panel
        Me.gbMarketPrices = New System.Windows.Forms.GroupBox
        Me.btnResetMarketPriceDate = New System.Windows.Forms.Button
        Me.lblMarketPriceUpdateStatus = New System.Windows.Forms.Label
        Me.btnUpdateMarketPrices = New System.Windows.Forms.Button
        Me.lblMarketPricesBy = New System.Windows.Forms.LinkLabel
        Me.lblMarketPricesByLbl = New System.Windows.Forms.Label
        Me.lblLastMarketPriceUpdate = New System.Windows.Forms.Label
        Me.lblLastMarketPriceUpdateLbl = New System.Windows.Forms.Label
        Me.gbFactionPrices = New System.Windows.Forms.GroupBox
        Me.btnResetFactionPriceData = New System.Windows.Forms.Button
        Me.lblFactionPriceUpdateStatus = New System.Windows.Forms.Label
        Me.btnUpdateFactionPrices = New System.Windows.Forms.Button
        Me.lblFactionPricesBy = New System.Windows.Forms.LinkLabel
        Me.lblFactionPricesByLbl = New System.Windows.Forms.Label
        Me.lblLastFactionPriceUpdate = New System.Windows.Forms.Label
        Me.lblLastFactionPriceUpdateLbl = New System.Windows.Forms.Label
        Me.tmrStart = New System.Windows.Forms.Timer(Me.components)
        Me.tabMarketPrices.SuspendLayout()
        Me.tabPriceSettings.SuspendLayout()
        Me.panelPrices.SuspendLayout()
        Me.grpMarketLogWatcher.SuspendLayout()
        Me.grpParsing.SuspendLayout()
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCriteria.SuspendLayout()
        Me.tabDumps.SuspendLayout()
        Me.panelECDumps.SuspendLayout()
        Me.gbProcessStatus.SuspendLayout()
        Me.tabMarketLogs.SuspendLayout()
        Me.panelMarketLog.SuspendLayout()
        Me.ctxMarketExport.SuspendLayout()
        Me.tabCustom.SuspendLayout()
        Me.panelCustom.SuspendLayout()
        Me.ctxPrices.SuspendLayout()
        Me.tabSnapshot.SuspendLayout()
        Me.panelSnapshot.SuspendLayout()
        Me.gbMarketPrices.SuspendLayout()
        Me.gbFactionPrices.SuspendLayout()
        Me.SuspendLayout()
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(14, 118)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(95, 118)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(16, 51)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(53, 13)
        Me.lblProgress.TabIndex = 10
        Me.lblProgress.Text = "Progress:"
        '
        'grpRegions
        '
        Me.grpRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRegions.Location = New System.Drawing.Point(10, 89)
        Me.grpRegions.Name = "grpRegions"
        Me.grpRegions.Size = New System.Drawing.Size(610, 338)
        Me.grpRegions.TabIndex = 12
        Me.grpRegions.TabStop = False
        Me.grpRegions.Text = "Region Selection"
        '
        'btnAllRegions
        '
        Me.btnAllRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAllRegions.Location = New System.Drawing.Point(626, 115)
        Me.btnAllRegions.Name = "btnAllRegions"
        Me.btnAllRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnAllRegions.TabIndex = 13
        Me.btnAllRegions.Text = "All Regions"
        Me.btnAllRegions.UseVisualStyleBackColor = True
        '
        'btnEmpireRegions
        '
        Me.btnEmpireRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmpireRegions.Location = New System.Drawing.Point(626, 144)
        Me.btnEmpireRegions.Name = "btnEmpireRegions"
        Me.btnEmpireRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnEmpireRegions.TabIndex = 14
        Me.btnEmpireRegions.Text = "Empire"
        Me.btnEmpireRegions.UseVisualStyleBackColor = True
        '
        'btnNullRegions
        '
        Me.btnNullRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNullRegions.Location = New System.Drawing.Point(626, 289)
        Me.btnNullRegions.Name = "btnNullRegions"
        Me.btnNullRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnNullRegions.TabIndex = 15
        Me.btnNullRegions.Text = "0.0"
        Me.btnNullRegions.UseVisualStyleBackColor = True
        '
        'btnNoRegions
        '
        Me.btnNoRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNoRegions.Location = New System.Drawing.Point(626, 318)
        Me.btnNoRegions.Name = "btnNoRegions"
        Me.btnNoRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnNoRegions.TabIndex = 16
        Me.btnNoRegions.Text = "No Regions"
        Me.btnNoRegions.UseVisualStyleBackColor = True
        '
        'tabMarketPrices
        '
        Me.tabMarketPrices.Controls.Add(Me.tabPriceSettings)
        Me.tabMarketPrices.Controls.Add(Me.tabDumps)
        Me.tabMarketPrices.Controls.Add(Me.tabMarketLogs)
        Me.tabMarketPrices.Controls.Add(Me.tabCustom)
        Me.tabMarketPrices.Controls.Add(Me.tabSnapshot)
        Me.tabMarketPrices.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMarketPrices.Location = New System.Drawing.Point(0, 0)
        Me.tabMarketPrices.Name = "tabMarketPrices"
        Me.tabMarketPrices.Padding = New System.Drawing.Point(0, 0)
        Me.tabMarketPrices.SelectedIndex = 0
        Me.tabMarketPrices.Size = New System.Drawing.Size(717, 623)
        Me.tabMarketPrices.TabIndex = 17
        '
        'tabPriceSettings
        '
        Me.tabPriceSettings.Controls.Add(Me.panelPrices)
        Me.tabPriceSettings.Location = New System.Drawing.Point(4, 22)
        Me.tabPriceSettings.Name = "tabPriceSettings"
        Me.tabPriceSettings.Size = New System.Drawing.Size(709, 597)
        Me.tabPriceSettings.TabIndex = 1
        Me.tabPriceSettings.Text = "Price Settings"
        Me.tabPriceSettings.UseVisualStyleBackColor = True
        '
        'panelPrices
        '
        Me.panelPrices.BackColor = System.Drawing.SystemColors.Control
        Me.panelPrices.Controls.Add(Me.btnAllRegions)
        Me.panelPrices.Controls.Add(Me.btnEmpireRegions)
        Me.panelPrices.Controls.Add(Me.grpMarketLogWatcher)
        Me.panelPrices.Controls.Add(Me.btnNullRegions)
        Me.panelPrices.Controls.Add(Me.grpParsing)
        Me.panelPrices.Controls.Add(Me.btnNoRegions)
        Me.panelPrices.Controls.Add(Me.grpCriteria)
        Me.panelPrices.Controls.Add(Me.btnAmarr)
        Me.panelPrices.Controls.Add(Me.grpRegions)
        Me.panelPrices.Controls.Add(Me.btnCaldari)
        Me.panelPrices.Controls.Add(Me.btnMinmatar)
        Me.panelPrices.Controls.Add(Me.btnGallente)
        Me.panelPrices.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelPrices.Location = New System.Drawing.Point(0, 0)
        Me.panelPrices.Margin = New System.Windows.Forms.Padding(0)
        Me.panelPrices.Name = "panelPrices"
        Me.panelPrices.Size = New System.Drawing.Size(709, 597)
        Me.panelPrices.TabIndex = 0
        '
        'grpMarketLogWatcher
        '
        Me.grpMarketLogWatcher.Controls.Add(Me.chkEnableLogWatcher)
        Me.grpMarketLogWatcher.Controls.Add(Me.chkEnableWatcherAtStartup)
        Me.grpMarketLogWatcher.Controls.Add(Me.chkNotifyTray)
        Me.grpMarketLogWatcher.Controls.Add(Me.chkAutoUpdatePriceData)
        Me.grpMarketLogWatcher.Controls.Add(Me.chkNotifyPopup)
        Me.grpMarketLogWatcher.Controls.Add(Me.chkAutoUpdateCurrentPrice)
        Me.grpMarketLogWatcher.Location = New System.Drawing.Point(10, 10)
        Me.grpMarketLogWatcher.Name = "grpMarketLogWatcher"
        Me.grpMarketLogWatcher.Size = New System.Drawing.Size(610, 73)
        Me.grpMarketLogWatcher.TabIndex = 36
        Me.grpMarketLogWatcher.TabStop = False
        Me.grpMarketLogWatcher.Text = "Market Log Watcher Options"
        '
        'chkEnableLogWatcher
        '
        Me.chkEnableLogWatcher.AutoSize = True
        Me.chkEnableLogWatcher.Location = New System.Drawing.Point(8, 19)
        Me.chkEnableLogWatcher.Name = "chkEnableLogWatcher"
        Me.chkEnableLogWatcher.Size = New System.Drawing.Size(102, 17)
        Me.chkEnableLogWatcher.TabIndex = 25
        Me.chkEnableLogWatcher.Text = "Enable Watcher"
        Me.chkEnableLogWatcher.UseVisualStyleBackColor = True
        '
        'chkEnableWatcherAtStartup
        '
        Me.chkEnableWatcherAtStartup.AutoSize = True
        Me.chkEnableWatcherAtStartup.Location = New System.Drawing.Point(8, 42)
        Me.chkEnableWatcherAtStartup.Name = "chkEnableWatcherAtStartup"
        Me.chkEnableWatcherAtStartup.Size = New System.Drawing.Size(184, 17)
        Me.chkEnableWatcherAtStartup.TabIndex = 26
        Me.chkEnableWatcherAtStartup.Text = "Start Watcher on EveHQ Startup"
        Me.chkEnableWatcherAtStartup.UseVisualStyleBackColor = True
        '
        'chkNotifyTray
        '
        Me.chkNotifyTray.AutoSize = True
        Me.chkNotifyTray.Location = New System.Drawing.Point(358, 42)
        Me.chkNotifyTray.Name = "chkNotifyTray"
        Me.chkNotifyTray.Size = New System.Drawing.Size(212, 17)
        Me.chkNotifyTray.TabIndex = 23
        Me.chkNotifyTray.Text = "System Tray Notification on Processing"
        Me.chkNotifyTray.UseVisualStyleBackColor = True
        '
        'chkAutoUpdatePriceData
        '
        Me.chkAutoUpdatePriceData.AutoSize = True
        Me.chkAutoUpdatePriceData.Enabled = False
        Me.chkAutoUpdatePriceData.Location = New System.Drawing.Point(198, 42)
        Me.chkAutoUpdatePriceData.Name = "chkAutoUpdatePriceData"
        Me.chkAutoUpdatePriceData.Size = New System.Drawing.Size(151, 17)
        Me.chkAutoUpdatePriceData.TabIndex = 28
        Me.chkAutoUpdatePriceData.Text = "Auto-Update Price History"
        Me.chkAutoUpdatePriceData.UseVisualStyleBackColor = True
        '
        'chkNotifyPopup
        '
        Me.chkNotifyPopup.AutoSize = True
        Me.chkNotifyPopup.Location = New System.Drawing.Point(358, 19)
        Me.chkNotifyPopup.Name = "chkNotifyPopup"
        Me.chkNotifyPopup.Size = New System.Drawing.Size(182, 17)
        Me.chkNotifyPopup.TabIndex = 24
        Me.chkNotifyPopup.Text = "Popup Notification on Processing"
        Me.chkNotifyPopup.UseVisualStyleBackColor = True
        '
        'chkAutoUpdateCurrentPrice
        '
        Me.chkAutoUpdateCurrentPrice.AutoSize = True
        Me.chkAutoUpdateCurrentPrice.Location = New System.Drawing.Point(198, 19)
        Me.chkAutoUpdateCurrentPrice.Name = "chkAutoUpdateCurrentPrice"
        Me.chkAutoUpdateCurrentPrice.Size = New System.Drawing.Size(154, 17)
        Me.chkAutoUpdateCurrentPrice.TabIndex = 27
        Me.chkAutoUpdateCurrentPrice.Text = "Auto-Update Current Price"
        Me.chkAutoUpdateCurrentPrice.UseVisualStyleBackColor = True
        '
        'grpParsing
        '
        Me.grpParsing.Controls.Add(Me.nudIgnoreBuyOrderLimit)
        Me.grpParsing.Controls.Add(Me.nudIgnoreSellOrderLimit)
        Me.grpParsing.Controls.Add(Me.chkIgnoreBuyOrders)
        Me.grpParsing.Controls.Add(Me.chkIgnoreSellOrders)
        Me.grpParsing.Controls.Add(Me.lblIgnoreSellOrderUnit)
        Me.grpParsing.Controls.Add(Me.lblIgnoreBuyOrderUnit)
        Me.grpParsing.Location = New System.Drawing.Point(10, 533)
        Me.grpParsing.Name = "grpParsing"
        Me.grpParsing.Size = New System.Drawing.Size(610, 49)
        Me.grpParsing.TabIndex = 35
        Me.grpParsing.TabStop = False
        Me.grpParsing.Text = "Market Log Parsing Criteria"
        '
        'nudIgnoreBuyOrderLimit
        '
        Me.nudIgnoreBuyOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreBuyOrderLimit.Location = New System.Drawing.Point(181, 18)
        Me.nudIgnoreBuyOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreBuyOrderLimit.Name = "nudIgnoreBuyOrderLimit"
        Me.nudIgnoreBuyOrderLimit.Size = New System.Drawing.Size(74, 21)
        Me.nudIgnoreBuyOrderLimit.TabIndex = 29
        Me.nudIgnoreBuyOrderLimit.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'nudIgnoreSellOrderLimit
        '
        Me.nudIgnoreSellOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreSellOrderLimit.Location = New System.Drawing.Point(481, 18)
        Me.nudIgnoreSellOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreSellOrderLimit.Name = "nudIgnoreSellOrderLimit"
        Me.nudIgnoreSellOrderLimit.Size = New System.Drawing.Size(74, 21)
        Me.nudIgnoreSellOrderLimit.TabIndex = 30
        Me.nudIgnoreSellOrderLimit.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'chkIgnoreBuyOrders
        '
        Me.chkIgnoreBuyOrders.AutoSize = True
        Me.chkIgnoreBuyOrders.Location = New System.Drawing.Point(8, 19)
        Me.chkIgnoreBuyOrders.Name = "chkIgnoreBuyOrders"
        Me.chkIgnoreBuyOrders.Size = New System.Drawing.Size(170, 17)
        Me.chkIgnoreBuyOrders.TabIndex = 33
        Me.chkIgnoreBuyOrders.Text = "Ignore Buy Orders Less Than:"
        Me.chkIgnoreBuyOrders.UseVisualStyleBackColor = True
        '
        'chkIgnoreSellOrders
        '
        Me.chkIgnoreSellOrders.AutoSize = True
        Me.chkIgnoreSellOrders.Location = New System.Drawing.Point(308, 19)
        Me.chkIgnoreSellOrders.Name = "chkIgnoreSellOrders"
        Me.chkIgnoreSellOrders.Size = New System.Drawing.Size(171, 17)
        Me.chkIgnoreSellOrders.TabIndex = 34
        Me.chkIgnoreSellOrders.Text = "Ignore Sell Orders More Than:"
        Me.chkIgnoreSellOrders.UseVisualStyleBackColor = True
        '
        'lblIgnoreSellOrderUnit
        '
        Me.lblIgnoreSellOrderUnit.AutoSize = True
        Me.lblIgnoreSellOrderUnit.Location = New System.Drawing.Point(561, 20)
        Me.lblIgnoreSellOrderUnit.Name = "lblIgnoreSellOrderUnit"
        Me.lblIgnoreSellOrderUnit.Size = New System.Drawing.Size(39, 13)
        Me.lblIgnoreSellOrderUnit.TabIndex = 32
        Me.lblIgnoreSellOrderUnit.Text = "x Base"
        '
        'lblIgnoreBuyOrderUnit
        '
        Me.lblIgnoreBuyOrderUnit.AutoSize = True
        Me.lblIgnoreBuyOrderUnit.Location = New System.Drawing.Point(261, 20)
        Me.lblIgnoreBuyOrderUnit.Name = "lblIgnoreBuyOrderUnit"
        Me.lblIgnoreBuyOrderUnit.Size = New System.Drawing.Size(23, 13)
        Me.lblIgnoreBuyOrderUnit.TabIndex = 31
        Me.lblIgnoreBuyOrderUnit.Text = "ISK"
        '
        'grpCriteria
        '
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria9)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria10)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria11)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria8)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria5)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria6)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria7)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria4)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria1)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria2)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria3)
        Me.grpCriteria.Controls.Add(Me.chkPriceCriteria0)
        Me.grpCriteria.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCriteria.Location = New System.Drawing.Point(10, 433)
        Me.grpCriteria.Name = "grpCriteria"
        Me.grpCriteria.Size = New System.Drawing.Size(610, 94)
        Me.grpCriteria.TabIndex = 21
        Me.grpCriteria.TabStop = False
        Me.grpCriteria.Text = "Pricing Criteria"
        '
        'chkPriceCriteria9
        '
        Me.chkPriceCriteria9.AutoSize = True
        Me.chkPriceCriteria9.Location = New System.Drawing.Point(158, 66)
        Me.chkPriceCriteria9.Name = "chkPriceCriteria9"
        Me.chkPriceCriteria9.Size = New System.Drawing.Size(108, 17)
        Me.chkPriceCriteria9.TabIndex = 11
        Me.chkPriceCriteria9.Text = "Median Price (All)"
        Me.chkPriceCriteria9.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria10
        '
        Me.chkPriceCriteria10.AutoSize = True
        Me.chkPriceCriteria10.Location = New System.Drawing.Point(308, 66)
        Me.chkPriceCriteria10.Name = "chkPriceCriteria10"
        Me.chkPriceCriteria10.Size = New System.Drawing.Size(90, 17)
        Me.chkPriceCriteria10.TabIndex = 10
        Me.chkPriceCriteria10.Text = "Min Price (All)"
        Me.chkPriceCriteria10.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria11
        '
        Me.chkPriceCriteria11.AutoSize = True
        Me.chkPriceCriteria11.Location = New System.Drawing.Point(458, 66)
        Me.chkPriceCriteria11.Name = "chkPriceCriteria11"
        Me.chkPriceCriteria11.Size = New System.Drawing.Size(94, 17)
        Me.chkPriceCriteria11.TabIndex = 9
        Me.chkPriceCriteria11.Text = "Max Price (All)"
        Me.chkPriceCriteria11.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria8
        '
        Me.chkPriceCriteria8.AutoSize = True
        Me.chkPriceCriteria8.Location = New System.Drawing.Point(8, 66)
        Me.chkPriceCriteria8.Name = "chkPriceCriteria8"
        Me.chkPriceCriteria8.Size = New System.Drawing.Size(100, 17)
        Me.chkPriceCriteria8.TabIndex = 8
        Me.chkPriceCriteria8.Text = "Mean Price (All)"
        Me.chkPriceCriteria8.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria5
        '
        Me.chkPriceCriteria5.AutoSize = True
        Me.chkPriceCriteria5.Location = New System.Drawing.Point(158, 43)
        Me.chkPriceCriteria5.Name = "chkPriceCriteria5"
        Me.chkPriceCriteria5.Size = New System.Drawing.Size(113, 17)
        Me.chkPriceCriteria5.TabIndex = 7
        Me.chkPriceCriteria5.Text = "Median Price (Sell)"
        Me.chkPriceCriteria5.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria6
        '
        Me.chkPriceCriteria6.AutoSize = True
        Me.chkPriceCriteria6.Location = New System.Drawing.Point(308, 43)
        Me.chkPriceCriteria6.Name = "chkPriceCriteria6"
        Me.chkPriceCriteria6.Size = New System.Drawing.Size(95, 17)
        Me.chkPriceCriteria6.TabIndex = 6
        Me.chkPriceCriteria6.Text = "Min Price (Sell)"
        Me.chkPriceCriteria6.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria7
        '
        Me.chkPriceCriteria7.AutoSize = True
        Me.chkPriceCriteria7.Location = New System.Drawing.Point(458, 43)
        Me.chkPriceCriteria7.Name = "chkPriceCriteria7"
        Me.chkPriceCriteria7.Size = New System.Drawing.Size(99, 17)
        Me.chkPriceCriteria7.TabIndex = 5
        Me.chkPriceCriteria7.Text = "Max Price (Sell)"
        Me.chkPriceCriteria7.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria4
        '
        Me.chkPriceCriteria4.AutoSize = True
        Me.chkPriceCriteria4.Location = New System.Drawing.Point(8, 43)
        Me.chkPriceCriteria4.Name = "chkPriceCriteria4"
        Me.chkPriceCriteria4.Size = New System.Drawing.Size(105, 17)
        Me.chkPriceCriteria4.TabIndex = 4
        Me.chkPriceCriteria4.Text = "Mean Price (Sell)"
        Me.chkPriceCriteria4.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria1
        '
        Me.chkPriceCriteria1.AutoSize = True
        Me.chkPriceCriteria1.Location = New System.Drawing.Point(158, 20)
        Me.chkPriceCriteria1.Name = "chkPriceCriteria1"
        Me.chkPriceCriteria1.Size = New System.Drawing.Size(115, 17)
        Me.chkPriceCriteria1.TabIndex = 3
        Me.chkPriceCriteria1.Text = "Median Price (Buy)"
        Me.chkPriceCriteria1.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria2
        '
        Me.chkPriceCriteria2.AutoSize = True
        Me.chkPriceCriteria2.Location = New System.Drawing.Point(308, 20)
        Me.chkPriceCriteria2.Name = "chkPriceCriteria2"
        Me.chkPriceCriteria2.Size = New System.Drawing.Size(97, 17)
        Me.chkPriceCriteria2.TabIndex = 2
        Me.chkPriceCriteria2.Text = "Min Price (Buy)"
        Me.chkPriceCriteria2.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria3
        '
        Me.chkPriceCriteria3.AutoSize = True
        Me.chkPriceCriteria3.Location = New System.Drawing.Point(458, 20)
        Me.chkPriceCriteria3.Name = "chkPriceCriteria3"
        Me.chkPriceCriteria3.Size = New System.Drawing.Size(101, 17)
        Me.chkPriceCriteria3.TabIndex = 1
        Me.chkPriceCriteria3.Text = "Max Price (Buy)"
        Me.chkPriceCriteria3.UseVisualStyleBackColor = True
        '
        'chkPriceCriteria0
        '
        Me.chkPriceCriteria0.AutoSize = True
        Me.chkPriceCriteria0.Location = New System.Drawing.Point(8, 20)
        Me.chkPriceCriteria0.Name = "chkPriceCriteria0"
        Me.chkPriceCriteria0.Size = New System.Drawing.Size(107, 17)
        Me.chkPriceCriteria0.TabIndex = 0
        Me.chkPriceCriteria0.Text = "Mean Price (Buy)"
        Me.chkPriceCriteria0.UseVisualStyleBackColor = True
        '
        'btnAmarr
        '
        Me.btnAmarr.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAmarr.Location = New System.Drawing.Point(626, 173)
        Me.btnAmarr.Name = "btnAmarr"
        Me.btnAmarr.Size = New System.Drawing.Size(70, 23)
        Me.btnAmarr.TabIndex = 17
        Me.btnAmarr.Text = "Amarr"
        Me.btnAmarr.UseVisualStyleBackColor = True
        '
        'btnCaldari
        '
        Me.btnCaldari.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCaldari.Location = New System.Drawing.Point(626, 202)
        Me.btnCaldari.Name = "btnCaldari"
        Me.btnCaldari.Size = New System.Drawing.Size(70, 23)
        Me.btnCaldari.TabIndex = 18
        Me.btnCaldari.Text = "Caldari"
        Me.btnCaldari.UseVisualStyleBackColor = True
        '
        'btnMinmatar
        '
        Me.btnMinmatar.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMinmatar.Location = New System.Drawing.Point(626, 260)
        Me.btnMinmatar.Name = "btnMinmatar"
        Me.btnMinmatar.Size = New System.Drawing.Size(70, 23)
        Me.btnMinmatar.TabIndex = 20
        Me.btnMinmatar.Text = "Minmatar"
        Me.btnMinmatar.UseVisualStyleBackColor = True
        '
        'btnGallente
        '
        Me.btnGallente.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGallente.Location = New System.Drawing.Point(626, 231)
        Me.btnGallente.Name = "btnGallente"
        Me.btnGallente.Size = New System.Drawing.Size(70, 23)
        Me.btnGallente.TabIndex = 19
        Me.btnGallente.Text = "Gallente"
        Me.btnGallente.UseVisualStyleBackColor = True
        '
        'tabDumps
        '
        Me.tabDumps.Controls.Add(Me.panelECDumps)
        Me.tabDumps.Location = New System.Drawing.Point(4, 22)
        Me.tabDumps.Name = "tabDumps"
        Me.tabDumps.Size = New System.Drawing.Size(709, 597)
        Me.tabDumps.TabIndex = 0
        Me.tabDumps.Text = "EC Market Dumps"
        Me.tabDumps.UseVisualStyleBackColor = True
        '
        'panelECDumps
        '
        Me.panelECDumps.BackColor = System.Drawing.SystemColors.Control
        Me.panelECDumps.Controls.Add(Me.gbProcessStatus)
        Me.panelECDumps.Controls.Add(Me.Button1)
        Me.panelECDumps.Controls.Add(Me.Button2)
        Me.panelECDumps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelECDumps.Location = New System.Drawing.Point(0, 0)
        Me.panelECDumps.Name = "panelECDumps"
        Me.panelECDumps.Size = New System.Drawing.Size(709, 597)
        Me.panelECDumps.TabIndex = 0
        '
        'gbProcessStatus
        '
        Me.gbProcessStatus.Controls.Add(Me.pbProgress)
        Me.gbProcessStatus.Controls.Add(Me.lblProcess)
        Me.gbProcessStatus.Controls.Add(Me.lblProgress)
        Me.gbProcessStatus.Location = New System.Drawing.Point(14, 170)
        Me.gbProcessStatus.Name = "gbProcessStatus"
        Me.gbProcessStatus.Size = New System.Drawing.Size(553, 119)
        Me.gbProcessStatus.TabIndex = 12
        Me.gbProcessStatus.TabStop = False
        Me.gbProcessStatus.Text = "Price Processing Status"
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(19, 76)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(528, 23)
        Me.pbProgress.TabIndex = 11
        '
        'lblProcess
        '
        Me.lblProcess.AutoSize = True
        Me.lblProcess.Location = New System.Drawing.Point(16, 27)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.Size = New System.Drawing.Size(186, 13)
        Me.lblProcess.TabIndex = 0
        Me.lblProcess.Text = "Current Process: Awaiting Processing"
        '
        'tabMarketLogs
        '
        Me.tabMarketLogs.Controls.Add(Me.panelMarketLog)
        Me.tabMarketLogs.Location = New System.Drawing.Point(4, 22)
        Me.tabMarketLogs.Name = "tabMarketLogs"
        Me.tabMarketLogs.Size = New System.Drawing.Size(709, 597)
        Me.tabMarketLogs.TabIndex = 2
        Me.tabMarketLogs.Text = "Market Log Import"
        Me.tabMarketLogs.UseVisualStyleBackColor = True
        '
        'panelMarketLog
        '
        Me.panelMarketLog.BackColor = System.Drawing.SystemColors.Control
        Me.panelMarketLog.Controls.Add(Me.clvLogs)
        Me.panelMarketLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelMarketLog.Location = New System.Drawing.Point(0, 0)
        Me.panelMarketLog.Name = "panelMarketLog"
        Me.panelMarketLog.Size = New System.Drawing.Size(709, 597)
        Me.panelMarketLog.TabIndex = 0
        '
        'clvLogs
        '
        Me.clvLogs.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colRegion, Me.colItem, Me.colDate})
        Me.clvLogs.DefaultItemHeight = 18
        Me.clvLogs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvLogs.ItemContextMenu = Me.ctxMarketExport
        Me.clvLogs.Location = New System.Drawing.Point(0, 0)
        Me.clvLogs.Name = "clvLogs"
        Me.clvLogs.Size = New System.Drawing.Size(709, 597)
        Me.clvLogs.TabIndex = 0
        '
        'colRegion
        '
        Me.colRegion.CustomSortTag = Nothing
        Me.colRegion.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colRegion.Tag = Nothing
        Me.colRegion.Text = "Region"
        Me.colRegion.Width = 150
        '
        'colItem
        '
        Me.colItem.CustomSortTag = Nothing
        Me.colItem.DisplayIndex = 1
        Me.colItem.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colItem.Tag = Nothing
        Me.colItem.Text = "Item"
        Me.colItem.Width = 300
        '
        'colDate
        '
        Me.colDate.CustomSortTag = Nothing
        Me.colDate.DisplayIndex = 2
        Me.colDate.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Date]
        Me.colDate.Tag = Nothing
        Me.colDate.Text = "Log Date"
        Me.colDate.Width = 150
        '
        'ctxMarketExport
        '
        Me.ctxMarketExport.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewOrders})
        Me.ctxMarketExport.Name = "ctxPrices"
        Me.ctxMarketExport.Size = New System.Drawing.Size(144, 26)
        '
        'mnuViewOrders
        '
        Me.mnuViewOrders.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.mnuViewOrders.Name = "mnuViewOrders"
        Me.mnuViewOrders.Size = New System.Drawing.Size(143, 22)
        Me.mnuViewOrders.Text = "View Orders"
        '
        'tabCustom
        '
        Me.tabCustom.Controls.Add(Me.panelCustom)
        Me.tabCustom.Location = New System.Drawing.Point(4, 22)
        Me.tabCustom.Name = "tabCustom"
        Me.tabCustom.Size = New System.Drawing.Size(709, 597)
        Me.tabCustom.TabIndex = 3
        Me.tabCustom.Text = "Custom Prices"
        Me.tabCustom.UseVisualStyleBackColor = True
        '
        'panelCustom
        '
        Me.panelCustom.BackColor = System.Drawing.SystemColors.Control
        Me.panelCustom.Controls.Add(Me.chkShowOnlyCustom)
        Me.panelCustom.Controls.Add(Me.lblCustomPrices)
        Me.panelCustom.Controls.Add(Me.btnResetGrid)
        Me.panelCustom.Controls.Add(Me.txtSearchPrices)
        Me.panelCustom.Controls.Add(Me.lblSearchPrices)
        Me.panelCustom.Controls.Add(Me.lvwPrices)
        Me.panelCustom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelCustom.Location = New System.Drawing.Point(0, 0)
        Me.panelCustom.Name = "panelCustom"
        Me.panelCustom.Size = New System.Drawing.Size(709, 597)
        Me.panelCustom.TabIndex = 0
        '
        'chkShowOnlyCustom
        '
        Me.chkShowOnlyCustom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkShowOnlyCustom.AutoSize = True
        Me.chkShowOnlyCustom.Location = New System.Drawing.Point(382, 570)
        Me.chkShowOnlyCustom.Name = "chkShowOnlyCustom"
        Me.chkShowOnlyCustom.Size = New System.Drawing.Size(147, 17)
        Me.chkShowOnlyCustom.TabIndex = 20
        Me.chkShowOnlyCustom.Text = "Show Only Custom Prices"
        Me.chkShowOnlyCustom.UseVisualStyleBackColor = True
        '
        'lblCustomPrices
        '
        Me.lblCustomPrices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCustomPrices.Location = New System.Drawing.Point(8, 9)
        Me.lblCustomPrices.Name = "lblCustomPrices"
        Me.lblCustomPrices.Size = New System.Drawing.Size(693, 31)
        Me.lblCustomPrices.TabIndex = 19
        Me.lblCustomPrices.Text = "Custom Prices enable you to override the Base and Market prices by allowing you t" & _
            "o enter your own individual figures. Right-click an item to edit or delete a Cus" & _
            "tom price." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnResetGrid
        '
        Me.btnResetGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnResetGrid.Location = New System.Drawing.Point(268, 566)
        Me.btnResetGrid.Name = "btnResetGrid"
        Me.btnResetGrid.Size = New System.Drawing.Size(75, 23)
        Me.btnResetGrid.TabIndex = 18
        Me.btnResetGrid.Text = "Reset Grid"
        Me.btnResetGrid.UseVisualStyleBackColor = True
        '
        'txtSearchPrices
        '
        Me.txtSearchPrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSearchPrices.Location = New System.Drawing.Point(83, 568)
        Me.txtSearchPrices.Name = "txtSearchPrices"
        Me.txtSearchPrices.Size = New System.Drawing.Size(177, 21)
        Me.txtSearchPrices.TabIndex = 17
        '
        'lblSearchPrices
        '
        Me.lblSearchPrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSearchPrices.AutoSize = True
        Me.lblSearchPrices.Location = New System.Drawing.Point(8, 572)
        Me.lblSearchPrices.Name = "lblSearchPrices"
        Me.lblSearchPrices.Size = New System.Drawing.Size(74, 13)
        Me.lblSearchPrices.TabIndex = 16
        Me.lblSearchPrices.Text = "Search Items:"
        '
        'lvwPrices
        '
        Me.lvwPrices.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwPrices.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPriceName, Me.colBasePrice, Me.colMarketPrice, Me.colCustomPrice})
        Me.lvwPrices.ContextMenuStrip = Me.ctxPrices
        Me.lvwPrices.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPrices.FullRowSelect = True
        Me.lvwPrices.GridLines = True
        Me.lvwPrices.HideSelection = False
        Me.lvwPrices.Location = New System.Drawing.Point(8, 43)
        Me.lvwPrices.MultiSelect = False
        Me.lvwPrices.Name = "lvwPrices"
        Me.lvwPrices.Size = New System.Drawing.Size(693, 517)
        Me.lvwPrices.TabIndex = 13
        Me.lvwPrices.UseCompatibleStateImageBehavior = False
        Me.lvwPrices.View = System.Windows.Forms.View.Details
        '
        'colPriceName
        '
        Me.colPriceName.Text = "Item Name"
        Me.colPriceName.Width = 300
        '
        'colBasePrice
        '
        Me.colBasePrice.Text = "Base Price"
        Me.colBasePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colBasePrice.Width = 120
        '
        'colMarketPrice
        '
        Me.colMarketPrice.Text = "Market Price"
        Me.colMarketPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colMarketPrice.Width = 120
        '
        'colCustomPrice
        '
        Me.colCustomPrice.Text = "Custom Price"
        Me.colCustomPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colCustomPrice.Width = 120
        '
        'ctxPrices
        '
        Me.ctxPrices.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPriceItemName, Me.ToolStripMenuItem1, Me.mnuPriceAdd, Me.mnuPriceEdit, Me.mnuPriceDelete})
        Me.ctxPrices.Name = "ctxPrices"
        Me.ctxPrices.Size = New System.Drawing.Size(182, 98)
        '
        'mnuPriceItemName
        '
        Me.mnuPriceItemName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.mnuPriceItemName.Name = "mnuPriceItemName"
        Me.mnuPriceItemName.Size = New System.Drawing.Size(181, 22)
        Me.mnuPriceItemName.Text = "Item Name"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPriceAdd
        '
        Me.mnuPriceAdd.Name = "mnuPriceAdd"
        Me.mnuPriceAdd.Size = New System.Drawing.Size(181, 22)
        Me.mnuPriceAdd.Text = "Add Custom Price"
        '
        'mnuPriceEdit
        '
        Me.mnuPriceEdit.Name = "mnuPriceEdit"
        Me.mnuPriceEdit.Size = New System.Drawing.Size(181, 22)
        Me.mnuPriceEdit.Text = "Edit Custom Price"
        '
        'mnuPriceDelete
        '
        Me.mnuPriceDelete.Name = "mnuPriceDelete"
        Me.mnuPriceDelete.Size = New System.Drawing.Size(181, 22)
        Me.mnuPriceDelete.Text = "Delete Custom Price"
        '
        'tabSnapshot
        '
        Me.tabSnapshot.Controls.Add(Me.panelSnapshot)
        Me.tabSnapshot.Location = New System.Drawing.Point(4, 22)
        Me.tabSnapshot.Name = "tabSnapshot"
        Me.tabSnapshot.Size = New System.Drawing.Size(709, 597)
        Me.tabSnapshot.TabIndex = 4
        Me.tabSnapshot.Text = "Price Snapshot"
        Me.tabSnapshot.UseVisualStyleBackColor = True
        '
        'panelSnapshot
        '
        Me.panelSnapshot.BackColor = System.Drawing.SystemColors.Control
        Me.panelSnapshot.Controls.Add(Me.gbMarketPrices)
        Me.panelSnapshot.Controls.Add(Me.gbFactionPrices)
        Me.panelSnapshot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelSnapshot.Location = New System.Drawing.Point(0, 0)
        Me.panelSnapshot.Name = "panelSnapshot"
        Me.panelSnapshot.Size = New System.Drawing.Size(709, 597)
        Me.panelSnapshot.TabIndex = 0
        '
        'gbMarketPrices
        '
        Me.gbMarketPrices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbMarketPrices.Controls.Add(Me.btnResetMarketPriceDate)
        Me.gbMarketPrices.Controls.Add(Me.lblMarketPriceUpdateStatus)
        Me.gbMarketPrices.Controls.Add(Me.btnUpdateMarketPrices)
        Me.gbMarketPrices.Controls.Add(Me.lblMarketPricesBy)
        Me.gbMarketPrices.Controls.Add(Me.lblMarketPricesByLbl)
        Me.gbMarketPrices.Controls.Add(Me.lblLastMarketPriceUpdate)
        Me.gbMarketPrices.Controls.Add(Me.lblLastMarketPriceUpdateLbl)
        Me.gbMarketPrices.Location = New System.Drawing.Point(8, 134)
        Me.gbMarketPrices.Name = "gbMarketPrices"
        Me.gbMarketPrices.Size = New System.Drawing.Size(693, 116)
        Me.gbMarketPrices.TabIndex = 1
        Me.gbMarketPrices.TabStop = False
        Me.gbMarketPrices.Text = "Market Prices"
        '
        'btnResetMarketPriceDate
        '
        Me.btnResetMarketPriceDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnResetMarketPriceDate.Location = New System.Drawing.Point(537, 23)
        Me.btnResetMarketPriceDate.Name = "btnResetMarketPriceDate"
        Me.btnResetMarketPriceDate.Size = New System.Drawing.Size(150, 23)
        Me.btnResetMarketPriceDate.TabIndex = 7
        Me.btnResetMarketPriceDate.Text = "Reset Market Price Date"
        Me.btnResetMarketPriceDate.UseVisualStyleBackColor = True
        '
        'lblMarketPriceUpdateStatus
        '
        Me.lblMarketPriceUpdateStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMarketPriceUpdateStatus.Location = New System.Drawing.Point(179, 58)
        Me.lblMarketPriceUpdateStatus.Name = "lblMarketPriceUpdateStatus"
        Me.lblMarketPriceUpdateStatus.Size = New System.Drawing.Size(508, 13)
        Me.lblMarketPriceUpdateStatus.TabIndex = 5
        Me.lblMarketPriceUpdateStatus.Text = "Status:"
        '
        'btnUpdateMarketPrices
        '
        Me.btnUpdateMarketPrices.Location = New System.Drawing.Point(23, 53)
        Me.btnUpdateMarketPrices.Name = "btnUpdateMarketPrices"
        Me.btnUpdateMarketPrices.Size = New System.Drawing.Size(150, 23)
        Me.btnUpdateMarketPrices.TabIndex = 4
        Me.btnUpdateMarketPrices.Text = "Update Market Prices"
        Me.btnUpdateMarketPrices.UseVisualStyleBackColor = True
        '
        'lblMarketPricesBy
        '
        Me.lblMarketPricesBy.AutoSize = True
        Me.lblMarketPricesBy.Location = New System.Drawing.Point(164, 88)
        Me.lblMarketPricesBy.Name = "lblMarketPricesBy"
        Me.lblMarketPricesBy.Size = New System.Drawing.Size(145, 13)
        Me.lblMarketPricesBy.TabIndex = 3
        Me.lblMarketPricesBy.TabStop = True
        Me.lblMarketPricesBy.Text = "http://www.eve-central.com"
        '
        'lblMarketPricesByLbl
        '
        Me.lblMarketPricesByLbl.AutoSize = True
        Me.lblMarketPricesByLbl.Location = New System.Drawing.Point(20, 88)
        Me.lblMarketPricesByLbl.Name = "lblMarketPricesByLbl"
        Me.lblMarketPricesByLbl.Size = New System.Drawing.Size(136, 13)
        Me.lblMarketPricesByLbl.TabIndex = 2
        Me.lblMarketPricesByLbl.Text = "Market Prices Supplied By: "
        '
        'lblLastMarketPriceUpdate
        '
        Me.lblLastMarketPriceUpdate.AutoSize = True
        Me.lblLastMarketPriceUpdate.Location = New System.Drawing.Point(164, 28)
        Me.lblLastMarketPriceUpdate.Name = "lblLastMarketPriceUpdate"
        Me.lblLastMarketPriceUpdate.Size = New System.Drawing.Size(51, 13)
        Me.lblLastMarketPriceUpdate.TabIndex = 1
        Me.lblLastMarketPriceUpdate.Text = "Unknown"
        '
        'lblLastMarketPriceUpdateLbl
        '
        Me.lblLastMarketPriceUpdateLbl.AutoSize = True
        Me.lblLastMarketPriceUpdateLbl.Location = New System.Drawing.Point(20, 28)
        Me.lblLastMarketPriceUpdateLbl.Name = "lblLastMarketPriceUpdateLbl"
        Me.lblLastMarketPriceUpdateLbl.Size = New System.Drawing.Size(131, 13)
        Me.lblLastMarketPriceUpdateLbl.TabIndex = 0
        Me.lblLastMarketPriceUpdateLbl.Text = "Last Market Price Update:"
        '
        'gbFactionPrices
        '
        Me.gbFactionPrices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFactionPrices.Controls.Add(Me.btnResetFactionPriceData)
        Me.gbFactionPrices.Controls.Add(Me.lblFactionPriceUpdateStatus)
        Me.gbFactionPrices.Controls.Add(Me.btnUpdateFactionPrices)
        Me.gbFactionPrices.Controls.Add(Me.lblFactionPricesBy)
        Me.gbFactionPrices.Controls.Add(Me.lblFactionPricesByLbl)
        Me.gbFactionPrices.Controls.Add(Me.lblLastFactionPriceUpdate)
        Me.gbFactionPrices.Controls.Add(Me.lblLastFactionPriceUpdateLbl)
        Me.gbFactionPrices.Location = New System.Drawing.Point(8, 12)
        Me.gbFactionPrices.Name = "gbFactionPrices"
        Me.gbFactionPrices.Size = New System.Drawing.Size(693, 116)
        Me.gbFactionPrices.TabIndex = 0
        Me.gbFactionPrices.TabStop = False
        Me.gbFactionPrices.Text = "Faction Prices"
        '
        'btnResetFactionPriceData
        '
        Me.btnResetFactionPriceData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnResetFactionPriceData.Location = New System.Drawing.Point(537, 23)
        Me.btnResetFactionPriceData.Name = "btnResetFactionPriceData"
        Me.btnResetFactionPriceData.Size = New System.Drawing.Size(150, 23)
        Me.btnResetFactionPriceData.TabIndex = 6
        Me.btnResetFactionPriceData.Text = "Reset Faction Price Date"
        Me.btnResetFactionPriceData.UseVisualStyleBackColor = True
        '
        'lblFactionPriceUpdateStatus
        '
        Me.lblFactionPriceUpdateStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFactionPriceUpdateStatus.Location = New System.Drawing.Point(179, 58)
        Me.lblFactionPriceUpdateStatus.Name = "lblFactionPriceUpdateStatus"
        Me.lblFactionPriceUpdateStatus.Size = New System.Drawing.Size(508, 13)
        Me.lblFactionPriceUpdateStatus.TabIndex = 5
        Me.lblFactionPriceUpdateStatus.Text = "Status:"
        '
        'btnUpdateFactionPrices
        '
        Me.btnUpdateFactionPrices.Location = New System.Drawing.Point(23, 53)
        Me.btnUpdateFactionPrices.Name = "btnUpdateFactionPrices"
        Me.btnUpdateFactionPrices.Size = New System.Drawing.Size(150, 23)
        Me.btnUpdateFactionPrices.TabIndex = 4
        Me.btnUpdateFactionPrices.Text = "Update Faction Prices"
        Me.btnUpdateFactionPrices.UseVisualStyleBackColor = True
        '
        'lblFactionPricesBy
        '
        Me.lblFactionPricesBy.AutoSize = True
        Me.lblFactionPricesBy.Location = New System.Drawing.Point(164, 88)
        Me.lblFactionPricesBy.Name = "lblFactionPricesBy"
        Me.lblFactionPricesBy.Size = New System.Drawing.Size(137, 13)
        Me.lblFactionPricesBy.TabIndex = 3
        Me.lblFactionPricesBy.TabStop = True
        Me.lblFactionPricesBy.Text = "http://www.eve-prices.net"
        '
        'lblFactionPricesByLbl
        '
        Me.lblFactionPricesByLbl.AutoSize = True
        Me.lblFactionPricesByLbl.Location = New System.Drawing.Point(20, 88)
        Me.lblFactionPricesByLbl.Name = "lblFactionPricesByLbl"
        Me.lblFactionPricesByLbl.Size = New System.Drawing.Size(138, 13)
        Me.lblFactionPricesByLbl.TabIndex = 2
        Me.lblFactionPricesByLbl.Text = "Faction Prices Supplied By: "
        '
        'lblLastFactionPriceUpdate
        '
        Me.lblLastFactionPriceUpdate.AutoSize = True
        Me.lblLastFactionPriceUpdate.Location = New System.Drawing.Point(164, 28)
        Me.lblLastFactionPriceUpdate.Name = "lblLastFactionPriceUpdate"
        Me.lblLastFactionPriceUpdate.Size = New System.Drawing.Size(51, 13)
        Me.lblLastFactionPriceUpdate.TabIndex = 1
        Me.lblLastFactionPriceUpdate.Text = "Unknown"
        '
        'lblLastFactionPriceUpdateLbl
        '
        Me.lblLastFactionPriceUpdateLbl.AutoSize = True
        Me.lblLastFactionPriceUpdateLbl.Location = New System.Drawing.Point(20, 28)
        Me.lblLastFactionPriceUpdateLbl.Name = "lblLastFactionPriceUpdateLbl"
        Me.lblLastFactionPriceUpdateLbl.Size = New System.Drawing.Size(133, 13)
        Me.lblLastFactionPriceUpdateLbl.TabIndex = 0
        Me.lblLastFactionPriceUpdateLbl.Text = "Last Faction Price Update:"
        '
        'tmrStart
        '
        '
        'frmMarketPrices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 623)
        Me.Controls.Add(Me.tabMarketPrices)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketPrices"
        Me.Text = "Market Prices"
        Me.tabMarketPrices.ResumeLayout(False)
        Me.tabPriceSettings.ResumeLayout(False)
        Me.panelPrices.ResumeLayout(False)
        Me.grpMarketLogWatcher.ResumeLayout(False)
        Me.grpMarketLogWatcher.PerformLayout()
        Me.grpParsing.ResumeLayout(False)
        Me.grpParsing.PerformLayout()
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCriteria.ResumeLayout(False)
        Me.grpCriteria.PerformLayout()
        Me.tabDumps.ResumeLayout(False)
        Me.panelECDumps.ResumeLayout(False)
        Me.gbProcessStatus.ResumeLayout(False)
        Me.gbProcessStatus.PerformLayout()
        Me.tabMarketLogs.ResumeLayout(False)
        Me.panelMarketLog.ResumeLayout(False)
        Me.ctxMarketExport.ResumeLayout(False)
        Me.tabCustom.ResumeLayout(False)
        Me.panelCustom.ResumeLayout(False)
        Me.panelCustom.PerformLayout()
        Me.ctxPrices.ResumeLayout(False)
        Me.tabSnapshot.ResumeLayout(False)
        Me.panelSnapshot.ResumeLayout(False)
        Me.gbMarketPrices.ResumeLayout(False)
        Me.gbMarketPrices.PerformLayout()
        Me.gbFactionPrices.ResumeLayout(False)
        Me.gbFactionPrices.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents grpRegions As System.Windows.Forms.GroupBox
    Friend WithEvents btnAllRegions As System.Windows.Forms.Button
    Friend WithEvents btnEmpireRegions As System.Windows.Forms.Button
    Friend WithEvents btnNullRegions As System.Windows.Forms.Button
    Friend WithEvents btnNoRegions As System.Windows.Forms.Button
    Friend WithEvents tabMarketPrices As System.Windows.Forms.TabControl
    Friend WithEvents tabDumps As System.Windows.Forms.TabPage
    Friend WithEvents panelECDumps As System.Windows.Forms.Panel
    Friend WithEvents tabPriceSettings As System.Windows.Forms.TabPage
    Friend WithEvents panelPrices As System.Windows.Forms.Panel
    Friend WithEvents btnMinmatar As System.Windows.Forms.Button
    Friend WithEvents btnGallente As System.Windows.Forms.Button
    Friend WithEvents btnCaldari As System.Windows.Forms.Button
    Friend WithEvents btnAmarr As System.Windows.Forms.Button
    Friend WithEvents grpCriteria As System.Windows.Forms.GroupBox
    Friend WithEvents chkPriceCriteria9 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria10 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria11 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria8 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria5 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria6 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria7 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria4 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkPriceCriteria0 As System.Windows.Forms.CheckBox
    Friend WithEvents tmrStart As System.Windows.Forms.Timer
    Friend WithEvents gbProcessStatus As System.Windows.Forms.GroupBox
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProcess As System.Windows.Forms.Label
    Friend WithEvents tabMarketLogs As System.Windows.Forms.TabPage
    Friend WithEvents panelMarketLog As System.Windows.Forms.Panel
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
    Friend WithEvents grpMarketLogWatcher As System.Windows.Forms.GroupBox
    Friend WithEvents grpParsing As System.Windows.Forms.GroupBox
    Friend WithEvents clvLogs As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colRegion As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colItem As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colDate As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents tabCustom As System.Windows.Forms.TabPage
    Friend WithEvents panelCustom As System.Windows.Forms.Panel
    Friend WithEvents lblCustomPrices As System.Windows.Forms.Label
    Friend WithEvents btnResetGrid As System.Windows.Forms.Button
    Friend WithEvents txtSearchPrices As System.Windows.Forms.TextBox
    Friend WithEvents lblSearchPrices As System.Windows.Forms.Label
    Friend WithEvents lvwPrices As EveHQ.ListViewNoFlicker
    Friend WithEvents colPriceName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBasePrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents colMarketPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCustomPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxPrices As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPriceItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPriceEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkShowOnlyCustom As System.Windows.Forms.CheckBox
    Friend WithEvents ctxMarketExport As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuViewOrders As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabSnapshot As System.Windows.Forms.TabPage
    Friend WithEvents panelSnapshot As System.Windows.Forms.Panel
    Friend WithEvents gbFactionPrices As System.Windows.Forms.GroupBox
    Friend WithEvents lblFactionPricesBy As System.Windows.Forms.LinkLabel
    Friend WithEvents lblFactionPricesByLbl As System.Windows.Forms.Label
    Friend WithEvents lblLastFactionPriceUpdate As System.Windows.Forms.Label
    Friend WithEvents lblLastFactionPriceUpdateLbl As System.Windows.Forms.Label
    Friend WithEvents btnUpdateFactionPrices As System.Windows.Forms.Button
    Friend WithEvents lblFactionPriceUpdateStatus As System.Windows.Forms.Label
    Friend WithEvents gbMarketPrices As System.Windows.Forms.GroupBox
    Friend WithEvents lblMarketPriceUpdateStatus As System.Windows.Forms.Label
    Friend WithEvents btnUpdateMarketPrices As System.Windows.Forms.Button
    Friend WithEvents lblMarketPricesBy As System.Windows.Forms.LinkLabel
    Friend WithEvents lblMarketPricesByLbl As System.Windows.Forms.Label
    Friend WithEvents lblLastMarketPriceUpdate As System.Windows.Forms.Label
    Friend WithEvents lblLastMarketPriceUpdateLbl As System.Windows.Forms.Label
    Friend WithEvents btnResetMarketPriceDate As System.Windows.Forms.Button
    Friend WithEvents btnResetFactionPriceData As System.Windows.Forms.Button
End Class
