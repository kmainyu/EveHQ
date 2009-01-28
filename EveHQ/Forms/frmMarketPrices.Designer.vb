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
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabDumps = New System.Windows.Forms.TabPage
        Me.panelECDumps = New System.Windows.Forms.Panel
        Me.gbProcessStatus = New System.Windows.Forms.GroupBox
        Me.pbProgress = New System.Windows.Forms.ProgressBar
        Me.lblProcess = New System.Windows.Forms.Label
        Me.tabPrices = New System.Windows.Forms.TabPage
        Me.panelPrices = New System.Windows.Forms.Panel
        Me.chkEnableLogWatcher = New System.Windows.Forms.CheckBox
        Me.chkNotifyPopup = New System.Windows.Forms.CheckBox
        Me.chkNotifyTray = New System.Windows.Forms.CheckBox
        Me.gbCriteria = New System.Windows.Forms.GroupBox
        Me.chkAllMedian = New System.Windows.Forms.CheckBox
        Me.chkAllMin = New System.Windows.Forms.CheckBox
        Me.chkAllMax = New System.Windows.Forms.CheckBox
        Me.chkAllMean = New System.Windows.Forms.CheckBox
        Me.chkSellMedian = New System.Windows.Forms.CheckBox
        Me.chkSellMin = New System.Windows.Forms.CheckBox
        Me.chkSellMax = New System.Windows.Forms.CheckBox
        Me.chkSellMean = New System.Windows.Forms.CheckBox
        Me.chkBuyMedian = New System.Windows.Forms.CheckBox
        Me.chkBuyMin = New System.Windows.Forms.CheckBox
        Me.chkBuyMax = New System.Windows.Forms.CheckBox
        Me.chkBuyMean = New System.Windows.Forms.CheckBox
        Me.btnMinmatar = New System.Windows.Forms.Button
        Me.btnGallente = New System.Windows.Forms.Button
        Me.btnCaldari = New System.Windows.Forms.Button
        Me.btnAmarr = New System.Windows.Forms.Button
        Me.tabMarketLogs = New System.Windows.Forms.TabPage
        Me.panelMarketLog = New System.Windows.Forms.Panel
        Me.lvwLogs = New System.Windows.Forms.ListView
        Me.colFilename = New System.Windows.Forms.ColumnHeader
        Me.colDate = New System.Windows.Forms.ColumnHeader
        Me.tmrStart = New System.Windows.Forms.Timer(Me.components)
        Me.chkEnableWatcherAtStartup = New System.Windows.Forms.CheckBox
        Me.chkAutoUpdateCurrentPrice = New System.Windows.Forms.CheckBox
        Me.chkAutoUpdatePriceData = New System.Windows.Forms.CheckBox
        Me.chkIgnoreBuyOrders = New System.Windows.Forms.CheckBox
        Me.nudIgnoreBuyOrderLimit = New System.Windows.Forms.NumericUpDown
        Me.nudIgnoreSellOrderLimit = New System.Windows.Forms.NumericUpDown
        Me.chkIgnoreSellOrders = New System.Windows.Forms.CheckBox
        Me.lblIgnoreBuyOrderUnit = New System.Windows.Forms.Label
        Me.lblIgnoreSellOrderUnit = New System.Windows.Forms.Label
        Me.grpParsing = New System.Windows.Forms.GroupBox
        Me.grpMarketLogWatcher = New System.Windows.Forms.GroupBox
        Me.TabControl1.SuspendLayout()
        Me.tabDumps.SuspendLayout()
        Me.panelECDumps.SuspendLayout()
        Me.gbProcessStatus.SuspendLayout()
        Me.tabPrices.SuspendLayout()
        Me.panelPrices.SuspendLayout()
        Me.gbCriteria.SuspendLayout()
        Me.tabMarketLogs.SuspendLayout()
        Me.panelMarketLog.SuspendLayout()
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpParsing.SuspendLayout()
        Me.grpMarketLogWatcher.SuspendLayout()
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
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabDumps)
        Me.TabControl1.Controls.Add(Me.tabPrices)
        Me.TabControl1.Controls.Add(Me.tabMarketLogs)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(717, 623)
        Me.TabControl1.TabIndex = 17
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
        'tabPrices
        '
        Me.tabPrices.Controls.Add(Me.panelPrices)
        Me.tabPrices.Location = New System.Drawing.Point(4, 22)
        Me.tabPrices.Name = "tabPrices"
        Me.tabPrices.Size = New System.Drawing.Size(709, 597)
        Me.tabPrices.TabIndex = 1
        Me.tabPrices.Text = "Price Selection"
        Me.tabPrices.UseVisualStyleBackColor = True
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
        Me.panelPrices.Controls.Add(Me.gbCriteria)
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
        'chkEnableLogWatcher
        '
        Me.chkEnableLogWatcher.AutoSize = True
        Me.chkEnableLogWatcher.Location = New System.Drawing.Point(8, 19)
        Me.chkEnableLogWatcher.Name = "chkEnableLogWatcher"
        Me.chkEnableLogWatcher.Size = New System.Drawing.Size(158, 17)
        Me.chkEnableLogWatcher.TabIndex = 25
        Me.chkEnableLogWatcher.Text = "Enable Market Log Watcher"
        Me.chkEnableLogWatcher.UseVisualStyleBackColor = True
        '
        'chkNotifyPopup
        '
        Me.chkNotifyPopup.AutoSize = True
        Me.chkNotifyPopup.Location = New System.Drawing.Point(407, 19)
        Me.chkNotifyPopup.Name = "chkNotifyPopup"
        Me.chkNotifyPopup.Size = New System.Drawing.Size(163, 17)
        Me.chkNotifyPopup.TabIndex = 24
        Me.chkNotifyPopup.Text = "Popup Notification on Import"
        Me.chkNotifyPopup.UseVisualStyleBackColor = True
        '
        'chkNotifyTray
        '
        Me.chkNotifyTray.AutoSize = True
        Me.chkNotifyTray.Location = New System.Drawing.Point(407, 42)
        Me.chkNotifyTray.Name = "chkNotifyTray"
        Me.chkNotifyTray.Size = New System.Drawing.Size(193, 17)
        Me.chkNotifyTray.TabIndex = 23
        Me.chkNotifyTray.Text = "System Tray Notification on Import"
        Me.chkNotifyTray.UseVisualStyleBackColor = True
        '
        'gbCriteria
        '
        Me.gbCriteria.Controls.Add(Me.chkAllMedian)
        Me.gbCriteria.Controls.Add(Me.chkAllMin)
        Me.gbCriteria.Controls.Add(Me.chkAllMax)
        Me.gbCriteria.Controls.Add(Me.chkAllMean)
        Me.gbCriteria.Controls.Add(Me.chkSellMedian)
        Me.gbCriteria.Controls.Add(Me.chkSellMin)
        Me.gbCriteria.Controls.Add(Me.chkSellMax)
        Me.gbCriteria.Controls.Add(Me.chkSellMean)
        Me.gbCriteria.Controls.Add(Me.chkBuyMedian)
        Me.gbCriteria.Controls.Add(Me.chkBuyMin)
        Me.gbCriteria.Controls.Add(Me.chkBuyMax)
        Me.gbCriteria.Controls.Add(Me.chkBuyMean)
        Me.gbCriteria.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbCriteria.Location = New System.Drawing.Point(10, 433)
        Me.gbCriteria.Name = "gbCriteria"
        Me.gbCriteria.Size = New System.Drawing.Size(610, 94)
        Me.gbCriteria.TabIndex = 21
        Me.gbCriteria.TabStop = False
        Me.gbCriteria.Text = "Pricing Criteria"
        '
        'chkAllMedian
        '
        Me.chkAllMedian.AutoSize = True
        Me.chkAllMedian.Location = New System.Drawing.Point(158, 66)
        Me.chkAllMedian.Name = "chkAllMedian"
        Me.chkAllMedian.Size = New System.Drawing.Size(108, 17)
        Me.chkAllMedian.TabIndex = 11
        Me.chkAllMedian.Text = "Median Price (All)"
        Me.chkAllMedian.UseVisualStyleBackColor = True
        '
        'chkAllMin
        '
        Me.chkAllMin.AutoSize = True
        Me.chkAllMin.Location = New System.Drawing.Point(308, 66)
        Me.chkAllMin.Name = "chkAllMin"
        Me.chkAllMin.Size = New System.Drawing.Size(90, 17)
        Me.chkAllMin.TabIndex = 10
        Me.chkAllMin.Text = "Min Price (All)"
        Me.chkAllMin.UseVisualStyleBackColor = True
        '
        'chkAllMax
        '
        Me.chkAllMax.AutoSize = True
        Me.chkAllMax.Location = New System.Drawing.Point(458, 66)
        Me.chkAllMax.Name = "chkAllMax"
        Me.chkAllMax.Size = New System.Drawing.Size(94, 17)
        Me.chkAllMax.TabIndex = 9
        Me.chkAllMax.Text = "Max Price (All)"
        Me.chkAllMax.UseVisualStyleBackColor = True
        '
        'chkAllMean
        '
        Me.chkAllMean.AutoSize = True
        Me.chkAllMean.Location = New System.Drawing.Point(8, 66)
        Me.chkAllMean.Name = "chkAllMean"
        Me.chkAllMean.Size = New System.Drawing.Size(100, 17)
        Me.chkAllMean.TabIndex = 8
        Me.chkAllMean.Text = "Mean Price (All)"
        Me.chkAllMean.UseVisualStyleBackColor = True
        '
        'chkSellMedian
        '
        Me.chkSellMedian.AutoSize = True
        Me.chkSellMedian.Location = New System.Drawing.Point(158, 43)
        Me.chkSellMedian.Name = "chkSellMedian"
        Me.chkSellMedian.Size = New System.Drawing.Size(113, 17)
        Me.chkSellMedian.TabIndex = 7
        Me.chkSellMedian.Text = "Median Price (Sell)"
        Me.chkSellMedian.UseVisualStyleBackColor = True
        '
        'chkSellMin
        '
        Me.chkSellMin.AutoSize = True
        Me.chkSellMin.Location = New System.Drawing.Point(308, 43)
        Me.chkSellMin.Name = "chkSellMin"
        Me.chkSellMin.Size = New System.Drawing.Size(95, 17)
        Me.chkSellMin.TabIndex = 6
        Me.chkSellMin.Text = "Min Price (Sell)"
        Me.chkSellMin.UseVisualStyleBackColor = True
        '
        'chkSellMax
        '
        Me.chkSellMax.AutoSize = True
        Me.chkSellMax.Location = New System.Drawing.Point(458, 43)
        Me.chkSellMax.Name = "chkSellMax"
        Me.chkSellMax.Size = New System.Drawing.Size(99, 17)
        Me.chkSellMax.TabIndex = 5
        Me.chkSellMax.Text = "Max Price (Sell)"
        Me.chkSellMax.UseVisualStyleBackColor = True
        '
        'chkSellMean
        '
        Me.chkSellMean.AutoSize = True
        Me.chkSellMean.Location = New System.Drawing.Point(8, 43)
        Me.chkSellMean.Name = "chkSellMean"
        Me.chkSellMean.Size = New System.Drawing.Size(105, 17)
        Me.chkSellMean.TabIndex = 4
        Me.chkSellMean.Text = "Mean Price (Sell)"
        Me.chkSellMean.UseVisualStyleBackColor = True
        '
        'chkBuyMedian
        '
        Me.chkBuyMedian.AutoSize = True
        Me.chkBuyMedian.Location = New System.Drawing.Point(158, 20)
        Me.chkBuyMedian.Name = "chkBuyMedian"
        Me.chkBuyMedian.Size = New System.Drawing.Size(115, 17)
        Me.chkBuyMedian.TabIndex = 3
        Me.chkBuyMedian.Text = "Median Price (Buy)"
        Me.chkBuyMedian.UseVisualStyleBackColor = True
        '
        'chkBuyMin
        '
        Me.chkBuyMin.AutoSize = True
        Me.chkBuyMin.Location = New System.Drawing.Point(308, 20)
        Me.chkBuyMin.Name = "chkBuyMin"
        Me.chkBuyMin.Size = New System.Drawing.Size(97, 17)
        Me.chkBuyMin.TabIndex = 2
        Me.chkBuyMin.Text = "Min Price (Buy)"
        Me.chkBuyMin.UseVisualStyleBackColor = True
        '
        'chkBuyMax
        '
        Me.chkBuyMax.AutoSize = True
        Me.chkBuyMax.Location = New System.Drawing.Point(458, 20)
        Me.chkBuyMax.Name = "chkBuyMax"
        Me.chkBuyMax.Size = New System.Drawing.Size(101, 17)
        Me.chkBuyMax.TabIndex = 1
        Me.chkBuyMax.Text = "Max Price (Buy)"
        Me.chkBuyMax.UseVisualStyleBackColor = True
        '
        'chkBuyMean
        '
        Me.chkBuyMean.AutoSize = True
        Me.chkBuyMean.Location = New System.Drawing.Point(8, 20)
        Me.chkBuyMean.Name = "chkBuyMean"
        Me.chkBuyMean.Size = New System.Drawing.Size(107, 17)
        Me.chkBuyMean.TabIndex = 0
        Me.chkBuyMean.Text = "Mean Price (Buy)"
        Me.chkBuyMean.UseVisualStyleBackColor = True
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
        Me.panelMarketLog.Controls.Add(Me.lvwLogs)
        Me.panelMarketLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelMarketLog.Location = New System.Drawing.Point(0, 0)
        Me.panelMarketLog.Name = "panelMarketLog"
        Me.panelMarketLog.Size = New System.Drawing.Size(709, 597)
        Me.panelMarketLog.TabIndex = 0
        '
        'lvwLogs
        '
        Me.lvwLogs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFilename, Me.colDate})
        Me.lvwLogs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwLogs.Location = New System.Drawing.Point(0, 0)
        Me.lvwLogs.Name = "lvwLogs"
        Me.lvwLogs.Size = New System.Drawing.Size(709, 597)
        Me.lvwLogs.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwLogs.TabIndex = 7
        Me.lvwLogs.UseCompatibleStateImageBehavior = False
        Me.lvwLogs.View = System.Windows.Forms.View.Details
        '
        'colFilename
        '
        Me.colFilename.Text = "FileName"
        Me.colFilename.Width = 500
        '
        'colDate
        '
        Me.colDate.Text = "Date/Time"
        Me.colDate.Width = 150
        '
        'tmrStart
        '
        '
        'chkEnableWatcherAtStartup
        '
        Me.chkEnableWatcherAtStartup.AutoSize = True
        Me.chkEnableWatcherAtStartup.Location = New System.Drawing.Point(8, 42)
        Me.chkEnableWatcherAtStartup.Name = "chkEnableWatcherAtStartup"
        Me.chkEnableWatcherAtStartup.Size = New System.Drawing.Size(220, 17)
        Me.chkEnableWatcherAtStartup.TabIndex = 26
        Me.chkEnableWatcherAtStartup.Text = "Start Market Watcher on EveHQ Startup"
        Me.chkEnableWatcherAtStartup.UseVisualStyleBackColor = True
        '
        'chkAutoUpdateCurrentPrice
        '
        Me.chkAutoUpdateCurrentPrice.AutoSize = True
        Me.chkAutoUpdateCurrentPrice.Location = New System.Drawing.Point(248, 19)
        Me.chkAutoUpdateCurrentPrice.Name = "chkAutoUpdateCurrentPrice"
        Me.chkAutoUpdateCurrentPrice.Size = New System.Drawing.Size(154, 17)
        Me.chkAutoUpdateCurrentPrice.TabIndex = 27
        Me.chkAutoUpdateCurrentPrice.Text = "Auto-Update Current Price"
        Me.chkAutoUpdateCurrentPrice.UseVisualStyleBackColor = True
        '
        'chkAutoUpdatePriceData
        '
        Me.chkAutoUpdatePriceData.AutoSize = True
        Me.chkAutoUpdatePriceData.Location = New System.Drawing.Point(248, 42)
        Me.chkAutoUpdatePriceData.Name = "chkAutoUpdatePriceData"
        Me.chkAutoUpdatePriceData.Size = New System.Drawing.Size(140, 17)
        Me.chkAutoUpdatePriceData.TabIndex = 28
        Me.chkAutoUpdatePriceData.Text = "Auto-Update Price Data"
        Me.chkAutoUpdatePriceData.UseVisualStyleBackColor = True
        '
        'chkIgnoreBuyOrders
        '
        Me.chkIgnoreBuyOrders.AutoSize = True
        Me.chkIgnoreBuyOrders.Checked = True
        Me.chkIgnoreBuyOrders.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIgnoreBuyOrders.Location = New System.Drawing.Point(8, 19)
        Me.chkIgnoreBuyOrders.Name = "chkIgnoreBuyOrders"
        Me.chkIgnoreBuyOrders.Size = New System.Drawing.Size(170, 17)
        Me.chkIgnoreBuyOrders.TabIndex = 33
        Me.chkIgnoreBuyOrders.Text = "Ignore Buy Orders Less Than:"
        Me.chkIgnoreBuyOrders.UseVisualStyleBackColor = True
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
        Me.nudIgnoreSellOrderLimit.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'chkIgnoreSellOrders
        '
        Me.chkIgnoreSellOrders.AutoSize = True
        Me.chkIgnoreSellOrders.Checked = True
        Me.chkIgnoreSellOrders.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIgnoreSellOrders.Location = New System.Drawing.Point(308, 19)
        Me.chkIgnoreSellOrders.Name = "chkIgnoreSellOrders"
        Me.chkIgnoreSellOrders.Size = New System.Drawing.Size(171, 17)
        Me.chkIgnoreSellOrders.TabIndex = 34
        Me.chkIgnoreSellOrders.Text = "Ignore Sell Orders More Than:"
        Me.chkIgnoreSellOrders.UseVisualStyleBackColor = True
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
        'lblIgnoreSellOrderUnit
        '
        Me.lblIgnoreSellOrderUnit.AutoSize = True
        Me.lblIgnoreSellOrderUnit.Location = New System.Drawing.Point(561, 20)
        Me.lblIgnoreSellOrderUnit.Name = "lblIgnoreSellOrderUnit"
        Me.lblIgnoreSellOrderUnit.Size = New System.Drawing.Size(39, 13)
        Me.lblIgnoreSellOrderUnit.TabIndex = 32
        Me.lblIgnoreSellOrderUnit.Text = "x Base"
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
        'frmMarketPrices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 623)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketPrices"
        Me.Text = "Market Prices"
        Me.TabControl1.ResumeLayout(False)
        Me.tabDumps.ResumeLayout(False)
        Me.panelECDumps.ResumeLayout(False)
        Me.gbProcessStatus.ResumeLayout(False)
        Me.gbProcessStatus.PerformLayout()
        Me.tabPrices.ResumeLayout(False)
        Me.panelPrices.ResumeLayout(False)
        Me.gbCriteria.ResumeLayout(False)
        Me.gbCriteria.PerformLayout()
        Me.tabMarketLogs.ResumeLayout(False)
        Me.panelMarketLog.ResumeLayout(False)
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpParsing.ResumeLayout(False)
        Me.grpParsing.PerformLayout()
        Me.grpMarketLogWatcher.ResumeLayout(False)
        Me.grpMarketLogWatcher.PerformLayout()
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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabDumps As System.Windows.Forms.TabPage
    Friend WithEvents panelECDumps As System.Windows.Forms.Panel
    Friend WithEvents tabPrices As System.Windows.Forms.TabPage
    Friend WithEvents panelPrices As System.Windows.Forms.Panel
    Friend WithEvents btnMinmatar As System.Windows.Forms.Button
    Friend WithEvents btnGallente As System.Windows.Forms.Button
    Friend WithEvents btnCaldari As System.Windows.Forms.Button
    Friend WithEvents btnAmarr As System.Windows.Forms.Button
    Friend WithEvents gbCriteria As System.Windows.Forms.GroupBox
    Friend WithEvents chkAllMedian As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllMin As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllMax As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllMean As System.Windows.Forms.CheckBox
    Friend WithEvents chkSellMedian As System.Windows.Forms.CheckBox
    Friend WithEvents chkSellMin As System.Windows.Forms.CheckBox
    Friend WithEvents chkSellMax As System.Windows.Forms.CheckBox
    Friend WithEvents chkSellMean As System.Windows.Forms.CheckBox
    Friend WithEvents chkBuyMedian As System.Windows.Forms.CheckBox
    Friend WithEvents chkBuyMin As System.Windows.Forms.CheckBox
    Friend WithEvents chkBuyMax As System.Windows.Forms.CheckBox
    Friend WithEvents chkBuyMean As System.Windows.Forms.CheckBox
    Friend WithEvents tmrStart As System.Windows.Forms.Timer
    Friend WithEvents gbProcessStatus As System.Windows.Forms.GroupBox
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProcess As System.Windows.Forms.Label
    Friend WithEvents tabMarketLogs As System.Windows.Forms.TabPage
    Friend WithEvents panelMarketLog As System.Windows.Forms.Panel
    Friend WithEvents lvwLogs As System.Windows.Forms.ListView
    Friend WithEvents colFilename As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDate As System.Windows.Forms.ColumnHeader
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
End Class
