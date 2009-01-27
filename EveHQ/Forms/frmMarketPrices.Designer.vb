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
        Me.nudIgnoreBuyOrderLimit = New System.Windows.Forms.NumericUpDown
        Me.nudIgnoreSellOrderLimit = New System.Windows.Forms.NumericUpDown
        Me.lblIgnoreBuyOrderUnit = New System.Windows.Forms.Label
        Me.lblIgnoreSellOrderUnit = New System.Windows.Forms.Label
        Me.chkIgnoreBuyOrders = New System.Windows.Forms.CheckBox
        Me.chkIgnoreSellOrders = New System.Windows.Forms.CheckBox
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
        Me.MonthCalendar1 = New System.Windows.Forms.MonthCalendar
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
        Me.tmrStart = New System.Windows.Forms.Timer(Me.components)
        Me.tabMarketLogs = New System.Windows.Forms.TabPage
        Me.panelMarketLog = New System.Windows.Forms.Panel
        Me.chkNotifyPopup = New System.Windows.Forms.CheckBox
        Me.chkNotifyTray = New System.Windows.Forms.CheckBox
        Me.chkEnableLogWatcher = New System.Windows.Forms.CheckBox
        Me.iconEveHQMLW = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.lvwLogs = New System.Windows.Forms.ListView
        Me.colFilename = New System.Windows.Forms.ColumnHeader
        Me.colDate = New System.Windows.Forms.ColumnHeader
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tabDumps.SuspendLayout()
        Me.panelECDumps.SuspendLayout()
        Me.gbProcessStatus.SuspendLayout()
        Me.tabPrices.SuspendLayout()
        Me.panelPrices.SuspendLayout()
        Me.gbCriteria.SuspendLayout()
        Me.tabMarketLogs.SuspendLayout()
        Me.panelMarketLog.SuspendLayout()
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
        'nudIgnoreBuyOrderLimit
        '
        Me.nudIgnoreBuyOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreBuyOrderLimit.Location = New System.Drawing.Point(187, 19)
        Me.nudIgnoreBuyOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreBuyOrderLimit.Name = "nudIgnoreBuyOrderLimit"
        Me.nudIgnoreBuyOrderLimit.Size = New System.Drawing.Size(74, 20)
        Me.nudIgnoreBuyOrderLimit.TabIndex = 2
        Me.nudIgnoreBuyOrderLimit.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'nudIgnoreSellOrderLimit
        '
        Me.nudIgnoreSellOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreSellOrderLimit.Location = New System.Drawing.Point(187, 45)
        Me.nudIgnoreSellOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreSellOrderLimit.Name = "nudIgnoreSellOrderLimit"
        Me.nudIgnoreSellOrderLimit.Size = New System.Drawing.Size(74, 20)
        Me.nudIgnoreSellOrderLimit.TabIndex = 4
        Me.nudIgnoreSellOrderLimit.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'lblIgnoreBuyOrderUnit
        '
        Me.lblIgnoreBuyOrderUnit.AutoSize = True
        Me.lblIgnoreBuyOrderUnit.Location = New System.Drawing.Point(267, 21)
        Me.lblIgnoreBuyOrderUnit.Name = "lblIgnoreBuyOrderUnit"
        Me.lblIgnoreBuyOrderUnit.Size = New System.Drawing.Size(24, 13)
        Me.lblIgnoreBuyOrderUnit.TabIndex = 5
        Me.lblIgnoreBuyOrderUnit.Text = "ISK"
        '
        'lblIgnoreSellOrderUnit
        '
        Me.lblIgnoreSellOrderUnit.AutoSize = True
        Me.lblIgnoreSellOrderUnit.Location = New System.Drawing.Point(267, 47)
        Me.lblIgnoreSellOrderUnit.Name = "lblIgnoreSellOrderUnit"
        Me.lblIgnoreSellOrderUnit.Size = New System.Drawing.Size(39, 13)
        Me.lblIgnoreSellOrderUnit.TabIndex = 6
        Me.lblIgnoreSellOrderUnit.Text = "x Base"
        '
        'chkIgnoreBuyOrders
        '
        Me.chkIgnoreBuyOrders.AutoSize = True
        Me.chkIgnoreBuyOrders.Checked = True
        Me.chkIgnoreBuyOrders.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIgnoreBuyOrders.Location = New System.Drawing.Point(14, 20)
        Me.chkIgnoreBuyOrders.Name = "chkIgnoreBuyOrders"
        Me.chkIgnoreBuyOrders.Size = New System.Drawing.Size(167, 17)
        Me.chkIgnoreBuyOrders.TabIndex = 7
        Me.chkIgnoreBuyOrders.Text = "Ignore Buy Orders Less Than:"
        Me.chkIgnoreBuyOrders.UseVisualStyleBackColor = True
        '
        'chkIgnoreSellOrders
        '
        Me.chkIgnoreSellOrders.AutoSize = True
        Me.chkIgnoreSellOrders.Checked = True
        Me.chkIgnoreSellOrders.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIgnoreSellOrders.Location = New System.Drawing.Point(14, 46)
        Me.chkIgnoreSellOrders.Name = "chkIgnoreSellOrders"
        Me.chkIgnoreSellOrders.Size = New System.Drawing.Size(168, 17)
        Me.chkIgnoreSellOrders.TabIndex = 8
        Me.chkIgnoreSellOrders.Text = "Ignore Sell Orders More Than:"
        Me.chkIgnoreSellOrders.UseVisualStyleBackColor = True
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
        Me.lblProgress.Size = New System.Drawing.Size(51, 13)
        Me.lblProgress.TabIndex = 10
        Me.lblProgress.Text = "Progress:"
        '
        'grpRegions
        '
        Me.grpRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRegions.Location = New System.Drawing.Point(17, 46)
        Me.grpRegions.Name = "grpRegions"
        Me.grpRegions.Size = New System.Drawing.Size(610, 350)
        Me.grpRegions.TabIndex = 12
        Me.grpRegions.TabStop = False
        Me.grpRegions.Text = "Regions"
        '
        'btnAllRegions
        '
        Me.btnAllRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAllRegions.Location = New System.Drawing.Point(17, 419)
        Me.btnAllRegions.Name = "btnAllRegions"
        Me.btnAllRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnAllRegions.TabIndex = 13
        Me.btnAllRegions.Text = "All Regions"
        Me.btnAllRegions.UseVisualStyleBackColor = True
        '
        'btnEmpireRegions
        '
        Me.btnEmpireRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmpireRegions.Location = New System.Drawing.Point(93, 419)
        Me.btnEmpireRegions.Name = "btnEmpireRegions"
        Me.btnEmpireRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnEmpireRegions.TabIndex = 14
        Me.btnEmpireRegions.Text = "Empire"
        Me.btnEmpireRegions.UseVisualStyleBackColor = True
        '
        'btnNullRegions
        '
        Me.btnNullRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNullRegions.Location = New System.Drawing.Point(169, 419)
        Me.btnNullRegions.Name = "btnNullRegions"
        Me.btnNullRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnNullRegions.TabIndex = 15
        Me.btnNullRegions.Text = "0.0"
        Me.btnNullRegions.UseVisualStyleBackColor = True
        '
        'btnNoRegions
        '
        Me.btnNoRegions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNoRegions.Location = New System.Drawing.Point(245, 419)
        Me.btnNoRegions.Name = "btnNoRegions"
        Me.btnNoRegions.Size = New System.Drawing.Size(75, 23)
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
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(789, 679)
        Me.TabControl1.TabIndex = 17
        '
        'tabDumps
        '
        Me.tabDumps.Controls.Add(Me.panelECDumps)
        Me.tabDumps.Location = New System.Drawing.Point(4, 22)
        Me.tabDumps.Name = "tabDumps"
        Me.tabDumps.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDumps.Size = New System.Drawing.Size(781, 653)
        Me.tabDumps.TabIndex = 0
        Me.tabDumps.Text = "EC Market Dumps"
        Me.tabDumps.UseVisualStyleBackColor = True
        '
        'panelECDumps
        '
        Me.panelECDumps.BackColor = System.Drawing.SystemColors.Control
        Me.panelECDumps.Controls.Add(Me.gbProcessStatus)
        Me.panelECDumps.Controls.Add(Me.chkIgnoreBuyOrders)
        Me.panelECDumps.Controls.Add(Me.Button1)
        Me.panelECDumps.Controls.Add(Me.nudIgnoreBuyOrderLimit)
        Me.panelECDumps.Controls.Add(Me.Button2)
        Me.panelECDumps.Controls.Add(Me.nudIgnoreSellOrderLimit)
        Me.panelECDumps.Controls.Add(Me.chkIgnoreSellOrders)
        Me.panelECDumps.Controls.Add(Me.lblIgnoreBuyOrderUnit)
        Me.panelECDumps.Controls.Add(Me.lblIgnoreSellOrderUnit)
        Me.panelECDumps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelECDumps.Location = New System.Drawing.Point(3, 3)
        Me.panelECDumps.Name = "panelECDumps"
        Me.panelECDumps.Size = New System.Drawing.Size(775, 647)
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
        Me.lblProcess.Size = New System.Drawing.Size(183, 13)
        Me.lblProcess.TabIndex = 0
        Me.lblProcess.Text = "Current Process: Awaiting Processing"
        '
        'tabPrices
        '
        Me.tabPrices.Controls.Add(Me.panelPrices)
        Me.tabPrices.Location = New System.Drawing.Point(4, 22)
        Me.tabPrices.Name = "tabPrices"
        Me.tabPrices.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPrices.Size = New System.Drawing.Size(781, 653)
        Me.tabPrices.TabIndex = 1
        Me.tabPrices.Text = "Price Selection"
        Me.tabPrices.UseVisualStyleBackColor = True
        '
        'panelPrices
        '
        Me.panelPrices.BackColor = System.Drawing.SystemColors.Control
        Me.panelPrices.Controls.Add(Me.MonthCalendar1)
        Me.panelPrices.Controls.Add(Me.gbCriteria)
        Me.panelPrices.Controls.Add(Me.btnMinmatar)
        Me.panelPrices.Controls.Add(Me.btnGallente)
        Me.panelPrices.Controls.Add(Me.btnCaldari)
        Me.panelPrices.Controls.Add(Me.btnAmarr)
        Me.panelPrices.Controls.Add(Me.grpRegions)
        Me.panelPrices.Controls.Add(Me.btnNoRegions)
        Me.panelPrices.Controls.Add(Me.btnAllRegions)
        Me.panelPrices.Controls.Add(Me.btnNullRegions)
        Me.panelPrices.Controls.Add(Me.btnEmpireRegions)
        Me.panelPrices.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelPrices.Location = New System.Drawing.Point(3, 3)
        Me.panelPrices.Name = "panelPrices"
        Me.panelPrices.Size = New System.Drawing.Size(775, 647)
        Me.panelPrices.TabIndex = 0
        '
        'MonthCalendar1
        '
        Me.MonthCalendar1.BoldedDates = New Date() {New Date(2009, 1, 28, 0, 0, 0, 0)}
        Me.MonthCalendar1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MonthCalendar1.Location = New System.Drawing.Point(453, 448)
        Me.MonthCalendar1.MaxSelectionCount = 1
        Me.MonthCalendar1.Name = "MonthCalendar1"
        Me.MonthCalendar1.TabIndex = 22
        Me.MonthCalendar1.TitleBackColor = System.Drawing.Color.MidnightBlue
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
        Me.gbCriteria.Location = New System.Drawing.Point(14, 448)
        Me.gbCriteria.Name = "gbCriteria"
        Me.gbCriteria.Size = New System.Drawing.Size(425, 126)
        Me.gbCriteria.TabIndex = 21
        Me.gbCriteria.TabStop = False
        Me.gbCriteria.Text = "Pricing Criteria"
        '
        'chkAllMedian
        '
        Me.chkAllMedian.AutoSize = True
        Me.chkAllMedian.Location = New System.Drawing.Point(309, 43)
        Me.chkAllMedian.Name = "chkAllMedian"
        Me.chkAllMedian.Size = New System.Drawing.Size(108, 17)
        Me.chkAllMedian.TabIndex = 11
        Me.chkAllMedian.Text = "Median Price (All)"
        Me.chkAllMedian.UseVisualStyleBackColor = True
        '
        'chkAllMin
        '
        Me.chkAllMin.AutoSize = True
        Me.chkAllMin.Location = New System.Drawing.Point(309, 66)
        Me.chkAllMin.Name = "chkAllMin"
        Me.chkAllMin.Size = New System.Drawing.Size(90, 17)
        Me.chkAllMin.TabIndex = 10
        Me.chkAllMin.Text = "Min Price (All)"
        Me.chkAllMin.UseVisualStyleBackColor = True
        '
        'chkAllMax
        '
        Me.chkAllMax.AutoSize = True
        Me.chkAllMax.Location = New System.Drawing.Point(309, 89)
        Me.chkAllMax.Name = "chkAllMax"
        Me.chkAllMax.Size = New System.Drawing.Size(94, 17)
        Me.chkAllMax.TabIndex = 9
        Me.chkAllMax.Text = "Max Price (All)"
        Me.chkAllMax.UseVisualStyleBackColor = True
        '
        'chkAllMean
        '
        Me.chkAllMean.AutoSize = True
        Me.chkAllMean.Location = New System.Drawing.Point(309, 20)
        Me.chkAllMean.Name = "chkAllMean"
        Me.chkAllMean.Size = New System.Drawing.Size(100, 17)
        Me.chkAllMean.TabIndex = 8
        Me.chkAllMean.Text = "Mean Price (All)"
        Me.chkAllMean.UseVisualStyleBackColor = True
        '
        'chkSellMedian
        '
        Me.chkSellMedian.AutoSize = True
        Me.chkSellMedian.Location = New System.Drawing.Point(152, 43)
        Me.chkSellMedian.Name = "chkSellMedian"
        Me.chkSellMedian.Size = New System.Drawing.Size(113, 17)
        Me.chkSellMedian.TabIndex = 7
        Me.chkSellMedian.Text = "Median Price (Sell)"
        Me.chkSellMedian.UseVisualStyleBackColor = True
        '
        'chkSellMin
        '
        Me.chkSellMin.AutoSize = True
        Me.chkSellMin.Location = New System.Drawing.Point(152, 66)
        Me.chkSellMin.Name = "chkSellMin"
        Me.chkSellMin.Size = New System.Drawing.Size(95, 17)
        Me.chkSellMin.TabIndex = 6
        Me.chkSellMin.Text = "Min Price (Sell)"
        Me.chkSellMin.UseVisualStyleBackColor = True
        '
        'chkSellMax
        '
        Me.chkSellMax.AutoSize = True
        Me.chkSellMax.Location = New System.Drawing.Point(152, 89)
        Me.chkSellMax.Name = "chkSellMax"
        Me.chkSellMax.Size = New System.Drawing.Size(99, 17)
        Me.chkSellMax.TabIndex = 5
        Me.chkSellMax.Text = "Max Price (Sell)"
        Me.chkSellMax.UseVisualStyleBackColor = True
        '
        'chkSellMean
        '
        Me.chkSellMean.AutoSize = True
        Me.chkSellMean.Location = New System.Drawing.Point(152, 20)
        Me.chkSellMean.Name = "chkSellMean"
        Me.chkSellMean.Size = New System.Drawing.Size(105, 17)
        Me.chkSellMean.TabIndex = 4
        Me.chkSellMean.Text = "Mean Price (Sell)"
        Me.chkSellMean.UseVisualStyleBackColor = True
        '
        'chkBuyMedian
        '
        Me.chkBuyMedian.AutoSize = True
        Me.chkBuyMedian.Location = New System.Drawing.Point(6, 43)
        Me.chkBuyMedian.Name = "chkBuyMedian"
        Me.chkBuyMedian.Size = New System.Drawing.Size(115, 17)
        Me.chkBuyMedian.TabIndex = 3
        Me.chkBuyMedian.Text = "Median Price (Buy)"
        Me.chkBuyMedian.UseVisualStyleBackColor = True
        '
        'chkBuyMin
        '
        Me.chkBuyMin.AutoSize = True
        Me.chkBuyMin.Location = New System.Drawing.Point(6, 66)
        Me.chkBuyMin.Name = "chkBuyMin"
        Me.chkBuyMin.Size = New System.Drawing.Size(97, 17)
        Me.chkBuyMin.TabIndex = 2
        Me.chkBuyMin.Text = "Min Price (Buy)"
        Me.chkBuyMin.UseVisualStyleBackColor = True
        '
        'chkBuyMax
        '
        Me.chkBuyMax.AutoSize = True
        Me.chkBuyMax.Location = New System.Drawing.Point(6, 89)
        Me.chkBuyMax.Name = "chkBuyMax"
        Me.chkBuyMax.Size = New System.Drawing.Size(101, 17)
        Me.chkBuyMax.TabIndex = 1
        Me.chkBuyMax.Text = "Max Price (Buy)"
        Me.chkBuyMax.UseVisualStyleBackColor = True
        '
        'chkBuyMean
        '
        Me.chkBuyMean.AutoSize = True
        Me.chkBuyMean.Location = New System.Drawing.Point(6, 20)
        Me.chkBuyMean.Name = "chkBuyMean"
        Me.chkBuyMean.Size = New System.Drawing.Size(107, 17)
        Me.chkBuyMean.TabIndex = 0
        Me.chkBuyMean.Text = "Mean Price (Buy)"
        Me.chkBuyMean.UseVisualStyleBackColor = True
        '
        'btnMinmatar
        '
        Me.btnMinmatar.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMinmatar.Location = New System.Drawing.Point(554, 419)
        Me.btnMinmatar.Name = "btnMinmatar"
        Me.btnMinmatar.Size = New System.Drawing.Size(70, 23)
        Me.btnMinmatar.TabIndex = 20
        Me.btnMinmatar.Text = "Minmatar"
        Me.btnMinmatar.UseVisualStyleBackColor = True
        '
        'btnGallente
        '
        Me.btnGallente.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGallente.Location = New System.Drawing.Point(478, 419)
        Me.btnGallente.Name = "btnGallente"
        Me.btnGallente.Size = New System.Drawing.Size(70, 23)
        Me.btnGallente.TabIndex = 19
        Me.btnGallente.Text = "Gallente"
        Me.btnGallente.UseVisualStyleBackColor = True
        '
        'btnCaldari
        '
        Me.btnCaldari.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCaldari.Location = New System.Drawing.Point(402, 419)
        Me.btnCaldari.Name = "btnCaldari"
        Me.btnCaldari.Size = New System.Drawing.Size(70, 23)
        Me.btnCaldari.TabIndex = 18
        Me.btnCaldari.Text = "Caldari"
        Me.btnCaldari.UseVisualStyleBackColor = True
        '
        'btnAmarr
        '
        Me.btnAmarr.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAmarr.Location = New System.Drawing.Point(326, 419)
        Me.btnAmarr.Name = "btnAmarr"
        Me.btnAmarr.Size = New System.Drawing.Size(70, 23)
        Me.btnAmarr.TabIndex = 17
        Me.btnAmarr.Text = "Amarr"
        Me.btnAmarr.UseVisualStyleBackColor = True
        '
        'tmrStart
        '
        '
        'tabMarketLogs
        '
        Me.tabMarketLogs.Controls.Add(Me.panelMarketLog)
        Me.tabMarketLogs.Location = New System.Drawing.Point(4, 22)
        Me.tabMarketLogs.Name = "tabMarketLogs"
        Me.tabMarketLogs.Size = New System.Drawing.Size(781, 653)
        Me.tabMarketLogs.TabIndex = 2
        Me.tabMarketLogs.Text = "Market Log Import"
        Me.tabMarketLogs.UseVisualStyleBackColor = True
        '
        'panelMarketLog
        '
        Me.panelMarketLog.BackColor = System.Drawing.SystemColors.Control
        Me.panelMarketLog.Controls.Add(Me.lvwLogs)
        Me.panelMarketLog.Controls.Add(Me.chkEnableLogWatcher)
        Me.panelMarketLog.Controls.Add(Me.chkNotifyPopup)
        Me.panelMarketLog.Controls.Add(Me.chkNotifyTray)
        Me.panelMarketLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelMarketLog.Location = New System.Drawing.Point(0, 0)
        Me.panelMarketLog.Name = "panelMarketLog"
        Me.panelMarketLog.Size = New System.Drawing.Size(781, 653)
        Me.panelMarketLog.TabIndex = 0
        '
        'chkNotifyPopup
        '
        Me.chkNotifyPopup.AutoSize = True
        Me.chkNotifyPopup.Location = New System.Drawing.Point(19, 65)
        Me.chkNotifyPopup.Name = "chkNotifyPopup"
        Me.chkNotifyPopup.Size = New System.Drawing.Size(198, 17)
        Me.chkNotifyPopup.TabIndex = 5
        Me.chkNotifyPopup.Text = "Popup Notification on Upload Result"
        Me.chkNotifyPopup.UseVisualStyleBackColor = True
        '
        'chkNotifyTray
        '
        Me.chkNotifyTray.AutoSize = True
        Me.chkNotifyTray.Location = New System.Drawing.Point(19, 42)
        Me.chkNotifyTray.Name = "chkNotifyTray"
        Me.chkNotifyTray.Size = New System.Drawing.Size(225, 17)
        Me.chkNotifyTray.TabIndex = 4
        Me.chkNotifyTray.Text = "System Tray Notification on Upload Result"
        Me.chkNotifyTray.UseVisualStyleBackColor = True
        '
        'chkEnableLogWatcher
        '
        Me.chkEnableLogWatcher.AutoSize = True
        Me.chkEnableLogWatcher.Location = New System.Drawing.Point(19, 19)
        Me.chkEnableLogWatcher.Name = "chkEnableLogWatcher"
        Me.chkEnableLogWatcher.Size = New System.Drawing.Size(160, 17)
        Me.chkEnableLogWatcher.TabIndex = 6
        Me.chkEnableLogWatcher.Text = "Enable Market Log Watcher"
        Me.chkEnableLogWatcher.UseVisualStyleBackColor = True
        '
        'iconEveHQMLW
        '
        Me.iconEveHQMLW.Icon = CType(resources.GetObject("iconEveHQMLW.Icon"), System.Drawing.Icon)
        '
        'lvwLogs
        '
        Me.lvwLogs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFilename, Me.colDate})
        Me.lvwLogs.Location = New System.Drawing.Point(8, 88)
        Me.lvwLogs.Name = "lvwLogs"
        Me.lvwLogs.Size = New System.Drawing.Size(765, 557)
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
        'frmMarketPrices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(789, 679)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketPrices"
        Me.Text = "Market Prices"
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tabDumps.ResumeLayout(False)
        Me.panelECDumps.ResumeLayout(False)
        Me.panelECDumps.PerformLayout()
        Me.gbProcessStatus.ResumeLayout(False)
        Me.gbProcessStatus.PerformLayout()
        Me.tabPrices.ResumeLayout(False)
        Me.panelPrices.ResumeLayout(False)
        Me.gbCriteria.ResumeLayout(False)
        Me.gbCriteria.PerformLayout()
        Me.tabMarketLogs.ResumeLayout(False)
        Me.panelMarketLog.ResumeLayout(False)
        Me.panelMarketLog.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents nudIgnoreBuyOrderLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudIgnoreSellOrderLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblIgnoreBuyOrderUnit As System.Windows.Forms.Label
    Friend WithEvents lblIgnoreSellOrderUnit As System.Windows.Forms.Label
    Friend WithEvents chkIgnoreBuyOrders As System.Windows.Forms.CheckBox
    Friend WithEvents chkIgnoreSellOrders As System.Windows.Forms.CheckBox
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
    Friend WithEvents MonthCalendar1 As System.Windows.Forms.MonthCalendar
    Friend WithEvents tmrStart As System.Windows.Forms.Timer
    Friend WithEvents gbProcessStatus As System.Windows.Forms.GroupBox
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProcess As System.Windows.Forms.Label
    Friend WithEvents tabMarketLogs As System.Windows.Forms.TabPage
    Friend WithEvents panelMarketLog As System.Windows.Forms.Panel
    Friend WithEvents chkNotifyPopup As System.Windows.Forms.CheckBox
    Friend WithEvents chkNotifyTray As System.Windows.Forms.CheckBox
    Friend WithEvents chkEnableLogWatcher As System.Windows.Forms.CheckBox
    Friend WithEvents iconEveHQMLW As System.Windows.Forms.NotifyIcon
    Friend WithEvents lvwLogs As System.Windows.Forms.ListView
    Friend WithEvents colFilename As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDate As System.Windows.Forms.ColumnHeader
End Class
