﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.tmrStart = New System.Windows.Forms.Timer(Me.components)
        Me.clvLogs = New DotNetLib.Windows.Forms.ContainerListView
        Me.colRegion = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colItem = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDate = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.TabControl1.SuspendLayout()
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
        Me.TabControl1.Controls.Add(Me.tabPriceSettings)
        Me.TabControl1.Controls.Add(Me.tabDumps)
        Me.TabControl1.Controls.Add(Me.tabMarketLogs)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(717, 623)
        Me.TabControl1.TabIndex = 17
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
        Me.chkAutoUpdatePriceData.Location = New System.Drawing.Point(198, 42)
        Me.chkAutoUpdatePriceData.Name = "chkAutoUpdatePriceData"
        Me.chkAutoUpdatePriceData.Size = New System.Drawing.Size(140, 17)
        Me.chkAutoUpdatePriceData.TabIndex = 28
        Me.chkAutoUpdatePriceData.Text = "Auto-Update Price Data"
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
        Me.nudIgnoreSellOrderLimit.Value = New Decimal(New Integer() {10, 0, 0, 0})
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
        'tmrStart
        '
        '
        'clvLogs
        '
        Me.clvLogs.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colRegion, Me.colItem, Me.colDate})
        Me.clvLogs.DefaultItemHeight = 18
        Me.clvLogs.Dock = System.Windows.Forms.DockStyle.Fill
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
End Class
