<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmEveHQ
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEveHQ))
        Me.ArrangeIconsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CloseAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WindowsMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.CascadeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TileVerticalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TileHorizontalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolStrip = New System.Windows.Forms.ToolStrip
        Me.btnTogglePanel = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripLabel
        Me.cboPilots = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbRetrieveData = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbPilotInfo = New System.Windows.Forms.ToolStripButton
        Me.tsbSkillTraining = New System.Windows.Forms.ToolStripButton
        Me.tsbWebBrowser = New System.Windows.Forms.ToolStripButton
        Me.tsbTrainingOverlay = New System.Windows.Forms.ToolStripButton
        Me.tsbSettingsBackup = New System.Windows.Forms.ToolStripButton
        Me.tsbSettings = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbIGB = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbCheckUpdates = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbAbout = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip = New System.Windows.Forms.MenuStrip
        Me.ViewMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.PilotInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WebBrowserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SkillTrainingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TrainingInformationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolsMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.RunIGBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolsGetAccountInfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuBackup = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolsAPIChecker = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuToolsTriggerError = New System.Windows.Forms.ToolStripMenuItem
        Me.ClearEveHQCache = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuModules = New System.Windows.Forms.ToolStripMenuItem
        Me.NoModulesLoadedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsHTML = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRepCharSummary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsHTMLChar = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsCharCharsheet = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportCharTraintimes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportTimeToLevel5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnureportCharSkillLevels = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportTrainingQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportQueueShoppingList = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportSkillsAvailable = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportSkillsNotTrained = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportPartiallyTrainedSkills = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportAsteroids = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportAsteroidAlloys = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportAsteroidRocks = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportAsteroidIce = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportSPSummary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsText = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextChar = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextCharSheet = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextTrainTimes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextTimeToLevel5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextSkillLevels = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextTrainingQueue = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextShoppingList = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextSkillsAvailable = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextSkillsNotTrained = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsTextPartiallyTrainedSkills = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsXML = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsXMLChar = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportCharXML = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportTrainXML = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportCurrentCharXMLOld = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportCurrentCharXMLNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportCurrentTrainingXMLOld = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuECMExport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsCharts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsChartsChar = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportSkillGroupChart = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuReportOpenfolder = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpCheckUpdates = New System.Windows.Forms.ToolStripMenuItem
        Me.VersionHistoryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.tmrEve = New System.Windows.Forms.Timer(Me.components)
        Me.EveStatusIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.EveIconMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ctxmnuLaunchEve1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxLaunchEve1Normal = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxLaunchEve1Full = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxmnuLaunchEve2 = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxLaunchEve2Normal = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxLaunchEve2Full = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxmnuLaunchEve3 = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxLaunchEve3Normal = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxLaunchEve3Full = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxmnuLaunchEve4 = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxLaunchEve4Normal = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxLaunchEve4Full = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.ForceServerCheckToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RestoreWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HideWhenMinimisedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.ctxAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxExit = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.tsTQStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.tsSisiStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.tsLogonStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.tsProgramStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.tmrSkillUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.tmrBackup = New System.Windows.Forms.Timer(Me.components)
        Me.tmrModules = New System.Windows.Forms.Timer(Me.components)
        Me.tabMDI = New System.Windows.Forms.TabControl
        Me.ctxTabbedMDI = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCloseMDITab = New System.Windows.Forms.ToolStripMenuItem
        Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog
        Me.tmrEveWindow = New System.Windows.Forms.Timer(Me.components)
        Me.ctxPlugin = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuLoadPlugin = New System.Windows.Forms.ToolStripMenuItem
        Me.XPanderList1 = New EveHQ.XPanderList
        Me.XPPilots = New EveHQ.XPander
        Me.XPTraining = New EveHQ.XPander
        Me.lblTrainingStatus = New System.Windows.Forms.Label
        Me.XPModules = New EveHQ.XPander
        Me.btnAddAccount = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStrip.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.EveIconMenu.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.ctxTabbedMDI.SuspendLayout()
        Me.ctxPlugin.SuspendLayout()
        Me.XPanderList1.SuspendLayout()
        Me.XPTraining.SuspendLayout()
        Me.SuspendLayout()
        '
        'ArrangeIconsToolStripMenuItem
        '
        Me.ArrangeIconsToolStripMenuItem.Name = "ArrangeIconsToolStripMenuItem"
        Me.ArrangeIconsToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.ArrangeIconsToolStripMenuItem.Text = "&Arrange Icons"
        Me.ArrangeIconsToolStripMenuItem.Visible = False
        '
        'CloseAllToolStripMenuItem
        '
        Me.CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        Me.CloseAllToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.CloseAllToolStripMenuItem.Text = "C&lose All"
        '
        'NewWindowToolStripMenuItem
        '
        Me.NewWindowToolStripMenuItem.Name = "NewWindowToolStripMenuItem"
        Me.NewWindowToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.NewWindowToolStripMenuItem.Text = "&New Window"
        '
        'WindowsMenu
        '
        Me.WindowsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewWindowToolStripMenuItem, Me.CascadeToolStripMenuItem, Me.TileVerticalToolStripMenuItem, Me.TileHorizontalToolStripMenuItem, Me.CloseAllToolStripMenuItem, Me.ArrangeIconsToolStripMenuItem, Me.ToolStripSeparator11})
        Me.WindowsMenu.Name = "WindowsMenu"
        Me.WindowsMenu.Size = New System.Drawing.Size(68, 20)
        Me.WindowsMenu.Text = "&Windows"
        '
        'CascadeToolStripMenuItem
        '
        Me.CascadeToolStripMenuItem.Name = "CascadeToolStripMenuItem"
        Me.CascadeToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.CascadeToolStripMenuItem.Text = "&Cascade"
        '
        'TileVerticalToolStripMenuItem
        '
        Me.TileVerticalToolStripMenuItem.Name = "TileVerticalToolStripMenuItem"
        Me.TileVerticalToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.TileVerticalToolStripMenuItem.Text = "Tile &Vertical"
        '
        'TileHorizontalToolStripMenuItem
        '
        Me.TileHorizontalToolStripMenuItem.Name = "TileHorizontalToolStripMenuItem"
        Me.TileHorizontalToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.TileHorizontalToolStripMenuItem.Text = "Tile &Horizontal"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(148, 6)
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Image = CType(resources.GetObject("OptionsToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.OptionsToolStripMenuItem.Text = "&Settings"
        '
        'ToolStrip
        '
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnTogglePanel, Me.ToolStripSeparator10, Me.btnAddAccount, Me.ToolStripSeparator14, Me.ToolStripLabel1, Me.ToolStripSeparator12, Me.cboPilots, Me.ToolStripSeparator13, Me.tsbRetrieveData, Me.ToolStripSeparator8, Me.tsbPilotInfo, Me.tsbSkillTraining, Me.tsbWebBrowser, Me.tsbTrainingOverlay, Me.tsbSettingsBackup, Me.tsbSettings, Me.ToolStripSeparator2, Me.tsbIGB, Me.ToolStripSeparator6, Me.tsbCheckUpdates, Me.ToolStripSeparator9, Me.tsbAbout, Me.ToolStripSeparator7})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(917, 25)
        Me.ToolStrip.TabIndex = 6
        Me.ToolStrip.Text = "ToolStrip"
        '
        'btnTogglePanel
        '
        Me.btnTogglePanel.Checked = True
        Me.btnTogglePanel.CheckOnClick = True
        Me.btnTogglePanel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.btnTogglePanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnTogglePanel.Image = CType(resources.GetObject("btnTogglePanel.Image"), System.Drawing.Image)
        Me.btnTogglePanel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnTogglePanel.Name = "btnTogglePanel"
        Me.btnTogglePanel.Size = New System.Drawing.Size(23, 22)
        Me.btnTogglePanel.Text = "Toggle InfoPanel"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(70, 22)
        Me.ToolStripLabel1.Text = "Active Pilot:"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(0, 22)
        '
        'cboPilots
        '
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Padding = New System.Windows.Forms.Padding(0, 1, 0, 0)
        Me.cboPilots.Size = New System.Drawing.Size(180, 25)
        Me.cboPilots.Sorted = True
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 25)
        '
        'tsbRetrieveData
        '
        Me.tsbRetrieveData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRetrieveData.Image = CType(resources.GetObject("tsbRetrieveData.Image"), System.Drawing.Image)
        Me.tsbRetrieveData.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRetrieveData.Name = "tsbRetrieveData"
        Me.tsbRetrieveData.Size = New System.Drawing.Size(23, 22)
        Me.tsbRetrieveData.Text = "ToolStripButton1"
        Me.tsbRetrieveData.ToolTipText = "Retrieve Character Data"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'tsbPilotInfo
        '
        Me.tsbPilotInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPilotInfo.Image = CType(resources.GetObject("tsbPilotInfo.Image"), System.Drawing.Image)
        Me.tsbPilotInfo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPilotInfo.Name = "tsbPilotInfo"
        Me.tsbPilotInfo.Size = New System.Drawing.Size(23, 22)
        Me.tsbPilotInfo.Text = "ToolStripButton2"
        Me.tsbPilotInfo.ToolTipText = "View Pilot Information"
        '
        'tsbSkillTraining
        '
        Me.tsbSkillTraining.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSkillTraining.Image = CType(resources.GetObject("tsbSkillTraining.Image"), System.Drawing.Image)
        Me.tsbSkillTraining.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSkillTraining.Name = "tsbSkillTraining"
        Me.tsbSkillTraining.Size = New System.Drawing.Size(23, 22)
        Me.tsbSkillTraining.Text = "ToolStripButton3"
        Me.tsbSkillTraining.ToolTipText = "View Skill Training"
        '
        'tsbWebBrowser
        '
        Me.tsbWebBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbWebBrowser.Image = CType(resources.GetObject("tsbWebBrowser.Image"), System.Drawing.Image)
        Me.tsbWebBrowser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbWebBrowser.Name = "tsbWebBrowser"
        Me.tsbWebBrowser.Size = New System.Drawing.Size(23, 22)
        Me.tsbWebBrowser.Text = "ToolStripButton4"
        Me.tsbWebBrowser.ToolTipText = "View Web Browser"
        '
        'tsbTrainingOverlay
        '
        Me.tsbTrainingOverlay.CheckOnClick = True
        Me.tsbTrainingOverlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbTrainingOverlay.Image = CType(resources.GetObject("tsbTrainingOverlay.Image"), System.Drawing.Image)
        Me.tsbTrainingOverlay.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbTrainingOverlay.Name = "tsbTrainingOverlay"
        Me.tsbTrainingOverlay.Size = New System.Drawing.Size(23, 22)
        Me.tsbTrainingOverlay.Text = "ToolStripButton5"
        Me.tsbTrainingOverlay.ToolTipText = "Toggle Training Overlay"
        '
        'tsbSettingsBackup
        '
        Me.tsbSettingsBackup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSettingsBackup.Image = CType(resources.GetObject("tsbSettingsBackup.Image"), System.Drawing.Image)
        Me.tsbSettingsBackup.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSettingsBackup.Name = "tsbSettingsBackup"
        Me.tsbSettingsBackup.Size = New System.Drawing.Size(23, 22)
        Me.tsbSettingsBackup.Text = "Eve Settings Backup Tool"
        '
        'tsbSettings
        '
        Me.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSettings.Image = CType(resources.GetObject("tsbSettings.Image"), System.Drawing.Image)
        Me.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSettings.Name = "tsbSettings"
        Me.tsbSettings.Size = New System.Drawing.Size(23, 22)
        Me.tsbSettings.Text = "ToolStripButton7"
        Me.tsbSettings.ToolTipText = "EveHQ Settings"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsbIGB
        '
        Me.tsbIGB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbIGB.Image = CType(resources.GetObject("tsbIGB.Image"), System.Drawing.Image)
        Me.tsbIGB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbIGB.Name = "tsbIGB"
        Me.tsbIGB.Size = New System.Drawing.Size(23, 22)
        Me.tsbIGB.Text = "ToolStripButton1"
        Me.tsbIGB.ToolTipText = "Toggle IGB Server"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'tsbCheckUpdates
        '
        Me.tsbCheckUpdates.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCheckUpdates.Image = CType(resources.GetObject("tsbCheckUpdates.Image"), System.Drawing.Image)
        Me.tsbCheckUpdates.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCheckUpdates.Name = "tsbCheckUpdates"
        Me.tsbCheckUpdates.Size = New System.Drawing.Size(23, 22)
        Me.tsbCheckUpdates.Text = "ToolStripButton1"
        Me.tsbCheckUpdates.ToolTipText = "Check For EveHQ Updates"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'tsbAbout
        '
        Me.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAbout.Image = CType(resources.GetObject("tsbAbout.Image"), System.Drawing.Image)
        Me.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAbout.Name = "tsbAbout"
        Me.tsbAbout.Size = New System.Drawing.Size(23, 22)
        Me.tsbAbout.Text = "ToolStripButton8"
        Me.tsbAbout.ToolTipText = "About EveHQ"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(133, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'FileMenu
        '
        Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder
        Me.FileMenu.Name = "FileMenu"
        Me.FileMenu.Size = New System.Drawing.Size(37, 20)
        Me.FileMenu.Text = "&File"
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.ViewMenu, Me.ToolsMenu, Me.mnuModules, Me.mnuReports, Me.WindowsMenu, Me.mnuHelp})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.MdiWindowListItem = Me.WindowsMenu
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(917, 24)
        Me.MenuStrip.TabIndex = 5
        Me.MenuStrip.Text = "MenuStrip"
        '
        'ViewMenu
        '
        Me.ViewMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PilotInfoToolStripMenuItem, Me.WebBrowserToolStripMenuItem, Me.SkillTrainingToolStripMenuItem, Me.TrainingInformationToolStripMenuItem})
        Me.ViewMenu.Name = "ViewMenu"
        Me.ViewMenu.Size = New System.Drawing.Size(44, 20)
        Me.ViewMenu.Text = "&View"
        '
        'PilotInfoToolStripMenuItem
        '
        Me.PilotInfoToolStripMenuItem.Image = CType(resources.GetObject("PilotInfoToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PilotInfoToolStripMenuItem.Name = "PilotInfoToolStripMenuItem"
        Me.PilotInfoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PilotInfoToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.PilotInfoToolStripMenuItem.Text = "Pilot Info"
        '
        'WebBrowserToolStripMenuItem
        '
        Me.WebBrowserToolStripMenuItem.Image = CType(resources.GetObject("WebBrowserToolStripMenuItem.Image"), System.Drawing.Image)
        Me.WebBrowserToolStripMenuItem.Name = "WebBrowserToolStripMenuItem"
        Me.WebBrowserToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.WebBrowserToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.WebBrowserToolStripMenuItem.Text = "Web Browser"
        '
        'SkillTrainingToolStripMenuItem
        '
        Me.SkillTrainingToolStripMenuItem.Image = CType(resources.GetObject("SkillTrainingToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SkillTrainingToolStripMenuItem.Name = "SkillTrainingToolStripMenuItem"
        Me.SkillTrainingToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.SkillTrainingToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.SkillTrainingToolStripMenuItem.Text = "Skill Training"
        '
        'TrainingInformationToolStripMenuItem
        '
        Me.TrainingInformationToolStripMenuItem.CheckOnClick = True
        Me.TrainingInformationToolStripMenuItem.Image = CType(resources.GetObject("TrainingInformationToolStripMenuItem.Image"), System.Drawing.Image)
        Me.TrainingInformationToolStripMenuItem.Name = "TrainingInformationToolStripMenuItem"
        Me.TrainingInformationToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.TrainingInformationToolStripMenuItem.Text = "Training Overlay"
        '
        'ToolsMenu
        '
        Me.ToolsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionsToolStripMenuItem, Me.RunIGBToolStripMenuItem, Me.mnuToolsGetAccountInfo, Me.mnuBackup, Me.mnuToolsAPIChecker, Me.ToolStripMenuItem1, Me.mnuToolsTriggerError, Me.ClearEveHQCache})
        Me.ToolsMenu.Name = "ToolsMenu"
        Me.ToolsMenu.Size = New System.Drawing.Size(48, 20)
        Me.ToolsMenu.Text = "&Tools"
        '
        'RunIGBToolStripMenuItem
        '
        Me.RunIGBToolStripMenuItem.Image = CType(resources.GetObject("RunIGBToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RunIGBToolStripMenuItem.Name = "RunIGBToolStripMenuItem"
        Me.RunIGBToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.RunIGBToolStripMenuItem.Text = "Run IGB"
        '
        'mnuToolsGetAccountInfo
        '
        Me.mnuToolsGetAccountInfo.Enabled = False
        Me.mnuToolsGetAccountInfo.Image = CType(resources.GetObject("mnuToolsGetAccountInfo.Image"), System.Drawing.Image)
        Me.mnuToolsGetAccountInfo.Name = "mnuToolsGetAccountInfo"
        Me.mnuToolsGetAccountInfo.Size = New System.Drawing.Size(202, 22)
        Me.mnuToolsGetAccountInfo.Text = "Retrieve Account Data"
        '
        'mnuBackup
        '
        Me.mnuBackup.Image = CType(resources.GetObject("mnuBackup.Image"), System.Drawing.Image)
        Me.mnuBackup.Name = "mnuBackup"
        Me.mnuBackup.Size = New System.Drawing.Size(202, 22)
        Me.mnuBackup.Text = "Settings Backup/Restore"
        '
        'mnuToolsAPIChecker
        '
        Me.mnuToolsAPIChecker.Name = "mnuToolsAPIChecker"
        Me.mnuToolsAPIChecker.Size = New System.Drawing.Size(202, 22)
        Me.mnuToolsAPIChecker.Text = "API Checker"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(199, 6)
        '
        'mnuToolsTriggerError
        '
        Me.mnuToolsTriggerError.Name = "mnuToolsTriggerError"
        Me.mnuToolsTriggerError.Size = New System.Drawing.Size(202, 22)
        Me.mnuToolsTriggerError.Text = "Trigger Error"
        Me.mnuToolsTriggerError.Visible = False
        '
        'ClearEveHQCache
        '
        Me.ClearEveHQCache.Name = "ClearEveHQCache"
        Me.ClearEveHQCache.Size = New System.Drawing.Size(202, 22)
        Me.ClearEveHQCache.Text = "Clear EveHQ Cache"
        '
        'mnuModules
        '
        Me.mnuModules.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NoModulesLoadedToolStripMenuItem})
        Me.mnuModules.Name = "mnuModules"
        Me.mnuModules.Size = New System.Drawing.Size(63, 20)
        Me.mnuModules.Text = "Plug-Ins"
        '
        'NoModulesLoadedToolStripMenuItem
        '
        Me.NoModulesLoadedToolStripMenuItem.Enabled = False
        Me.NoModulesLoadedToolStripMenuItem.Name = "NoModulesLoadedToolStripMenuItem"
        Me.NoModulesLoadedToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.NoModulesLoadedToolStripMenuItem.Text = "No Modules Loaded"
        '
        'mnuReports
        '
        Me.mnuReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportsHTML, Me.mnuReportsText, Me.mnuReportsXML, Me.mnuReportsCharts, Me.ToolStripSeparator3, Me.mnuReportOpenfolder})
        Me.mnuReports.Name = "mnuReports"
        Me.mnuReports.Size = New System.Drawing.Size(109, 20)
        Me.mnuReports.Text = "Reports && Charts"
        '
        'mnuReportsHTML
        '
        Me.mnuReportsHTML.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRepCharSummary, Me.mnuReportsHTMLChar, Me.mnuReportAsteroids, Me.mnuReportSPSummary})
        Me.mnuReportsHTML.Name = "mnuReportsHTML"
        Me.mnuReportsHTML.Size = New System.Drawing.Size(177, 22)
        Me.mnuReportsHTML.Text = "HTML Reports"
        '
        'mnuRepCharSummary
        '
        Me.mnuRepCharSummary.Name = "mnuRepCharSummary"
        Me.mnuRepCharSummary.Size = New System.Drawing.Size(189, 22)
        Me.mnuRepCharSummary.Text = "Character Summary"
        '
        'mnuReportsHTMLChar
        '
        Me.mnuReportsHTMLChar.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportsCharCharsheet, Me.mnuReportCharTraintimes, Me.mnuReportTimeToLevel5, Me.mnureportCharSkillLevels, Me.mnuReportTrainingQueue, Me.mnuReportQueueShoppingList, Me.mnuReportSkillsAvailable, Me.mnuReportSkillsNotTrained, Me.mnuReportPartiallyTrainedSkills})
        Me.mnuReportsHTMLChar.Enabled = False
        Me.mnuReportsHTMLChar.Name = "mnuReportsHTMLChar"
        Me.mnuReportsHTMLChar.Size = New System.Drawing.Size(189, 22)
        Me.mnuReportsHTMLChar.Text = "Character"
        '
        'mnuReportsCharCharsheet
        '
        Me.mnuReportsCharCharsheet.Name = "mnuReportsCharCharsheet"
        Me.mnuReportsCharCharsheet.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsCharCharsheet.Text = "Character Sheet"
        '
        'mnuReportCharTraintimes
        '
        Me.mnuReportCharTraintimes.Name = "mnuReportCharTraintimes"
        Me.mnuReportCharTraintimes.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportCharTraintimes.Text = "Training Times"
        '
        'mnuReportTimeToLevel5
        '
        Me.mnuReportTimeToLevel5.Name = "mnuReportTimeToLevel5"
        Me.mnuReportTimeToLevel5.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportTimeToLevel5.Text = "Time To Level 5"
        '
        'mnureportCharSkillLevels
        '
        Me.mnureportCharSkillLevels.Name = "mnureportCharSkillLevels"
        Me.mnureportCharSkillLevels.Size = New System.Drawing.Size(198, 22)
        Me.mnureportCharSkillLevels.Text = "Skill Levels"
        '
        'mnuReportTrainingQueue
        '
        Me.mnuReportTrainingQueue.Name = "mnuReportTrainingQueue"
        Me.mnuReportTrainingQueue.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportTrainingQueue.Text = "Training Queue"
        '
        'mnuReportQueueShoppingList
        '
        Me.mnuReportQueueShoppingList.Name = "mnuReportQueueShoppingList"
        Me.mnuReportQueueShoppingList.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportQueueShoppingList.Text = "Queue Shopping List"
        '
        'mnuReportSkillsAvailable
        '
        Me.mnuReportSkillsAvailable.Name = "mnuReportSkillsAvailable"
        Me.mnuReportSkillsAvailable.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportSkillsAvailable.Text = "Skills Available To Train"
        '
        'mnuReportSkillsNotTrained
        '
        Me.mnuReportSkillsNotTrained.Name = "mnuReportSkillsNotTrained"
        Me.mnuReportSkillsNotTrained.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportSkillsNotTrained.Text = "Skills Not Trained"
        '
        'mnuReportPartiallyTrainedSkills
        '
        Me.mnuReportPartiallyTrainedSkills.Name = "mnuReportPartiallyTrainedSkills"
        Me.mnuReportPartiallyTrainedSkills.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportPartiallyTrainedSkills.Text = "Partially Trained Skills"
        '
        'mnuReportAsteroids
        '
        Me.mnuReportAsteroids.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportAsteroidAlloys, Me.mnuReportAsteroidRocks, Me.mnuReportAsteroidIce})
        Me.mnuReportAsteroids.Name = "mnuReportAsteroids"
        Me.mnuReportAsteroids.Size = New System.Drawing.Size(189, 22)
        Me.mnuReportAsteroids.Text = "Material Composition"
        '
        'mnuReportAsteroidAlloys
        '
        Me.mnuReportAsteroidAlloys.Name = "mnuReportAsteroidAlloys"
        Me.mnuReportAsteroidAlloys.Size = New System.Drawing.Size(123, 22)
        Me.mnuReportAsteroidAlloys.Text = "Alloys"
        '
        'mnuReportAsteroidRocks
        '
        Me.mnuReportAsteroidRocks.Name = "mnuReportAsteroidRocks"
        Me.mnuReportAsteroidRocks.Size = New System.Drawing.Size(123, 22)
        Me.mnuReportAsteroidRocks.Text = "Asteroids"
        '
        'mnuReportAsteroidIce
        '
        Me.mnuReportAsteroidIce.Name = "mnuReportAsteroidIce"
        Me.mnuReportAsteroidIce.Size = New System.Drawing.Size(123, 22)
        Me.mnuReportAsteroidIce.Text = "Ice"
        '
        'mnuReportSPSummary
        '
        Me.mnuReportSPSummary.Name = "mnuReportSPSummary"
        Me.mnuReportSPSummary.Size = New System.Drawing.Size(189, 22)
        Me.mnuReportSPSummary.Text = "Skill Level Table"
        '
        'mnuReportsText
        '
        Me.mnuReportsText.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportsTextChar})
        Me.mnuReportsText.Name = "mnuReportsText"
        Me.mnuReportsText.Size = New System.Drawing.Size(177, 22)
        Me.mnuReportsText.Text = "Text Reports"
        '
        'mnuReportsTextChar
        '
        Me.mnuReportsTextChar.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportsTextCharSheet, Me.mnuReportsTextTrainTimes, Me.mnuReportsTextTimeToLevel5, Me.mnuReportsTextSkillLevels, Me.mnuReportsTextTrainingQueue, Me.mnuReportsTextShoppingList, Me.mnuReportsTextSkillsAvailable, Me.mnuReportsTextSkillsNotTrained, Me.mnuReportsTextPartiallyTrainedSkills})
        Me.mnuReportsTextChar.Name = "mnuReportsTextChar"
        Me.mnuReportsTextChar.Size = New System.Drawing.Size(125, 22)
        Me.mnuReportsTextChar.Text = "Character"
        '
        'mnuReportsTextCharSheet
        '
        Me.mnuReportsTextCharSheet.Name = "mnuReportsTextCharSheet"
        Me.mnuReportsTextCharSheet.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextCharSheet.Text = "Character Sheet"
        '
        'mnuReportsTextTrainTimes
        '
        Me.mnuReportsTextTrainTimes.Name = "mnuReportsTextTrainTimes"
        Me.mnuReportsTextTrainTimes.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextTrainTimes.Text = "Training Times"
        '
        'mnuReportsTextTimeToLevel5
        '
        Me.mnuReportsTextTimeToLevel5.Name = "mnuReportsTextTimeToLevel5"
        Me.mnuReportsTextTimeToLevel5.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextTimeToLevel5.Text = "Time To Level 5"
        '
        'mnuReportsTextSkillLevels
        '
        Me.mnuReportsTextSkillLevels.Name = "mnuReportsTextSkillLevels"
        Me.mnuReportsTextSkillLevels.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextSkillLevels.Text = "Skill Levels"
        '
        'mnuReportsTextTrainingQueue
        '
        Me.mnuReportsTextTrainingQueue.Name = "mnuReportsTextTrainingQueue"
        Me.mnuReportsTextTrainingQueue.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextTrainingQueue.Text = "Training Queue"
        '
        'mnuReportsTextShoppingList
        '
        Me.mnuReportsTextShoppingList.Name = "mnuReportsTextShoppingList"
        Me.mnuReportsTextShoppingList.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextShoppingList.Text = "Queue Shopping List"
        '
        'mnuReportsTextSkillsAvailable
        '
        Me.mnuReportsTextSkillsAvailable.Name = "mnuReportsTextSkillsAvailable"
        Me.mnuReportsTextSkillsAvailable.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextSkillsAvailable.Text = "Skills Available To Train"
        '
        'mnuReportsTextSkillsNotTrained
        '
        Me.mnuReportsTextSkillsNotTrained.Name = "mnuReportsTextSkillsNotTrained"
        Me.mnuReportsTextSkillsNotTrained.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextSkillsNotTrained.Text = "Skills Not Trained"
        '
        'mnuReportsTextPartiallyTrainedSkills
        '
        Me.mnuReportsTextPartiallyTrainedSkills.Name = "mnuReportsTextPartiallyTrainedSkills"
        Me.mnuReportsTextPartiallyTrainedSkills.Size = New System.Drawing.Size(198, 22)
        Me.mnuReportsTextPartiallyTrainedSkills.Text = "Partially Trained Skills"
        '
        'mnuReportsXML
        '
        Me.mnuReportsXML.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportsXMLChar, Me.mnuECMExport})
        Me.mnuReportsXML.Name = "mnuReportsXML"
        Me.mnuReportsXML.Size = New System.Drawing.Size(177, 22)
        Me.mnuReportsXML.Text = "XML Reports"
        '
        'mnuReportsXMLChar
        '
        Me.mnuReportsXMLChar.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportCharXML, Me.mnuReportTrainXML, Me.mnuReportCurrentCharXMLOld, Me.mnuReportCurrentCharXMLNew, Me.mnuReportCurrentTrainingXMLOld})
        Me.mnuReportsXMLChar.Name = "mnuReportsXMLChar"
        Me.mnuReportsXMLChar.Size = New System.Drawing.Size(214, 22)
        Me.mnuReportsXMLChar.Text = "Character"
        '
        'mnuReportCharXML
        '
        Me.mnuReportCharXML.Name = "mnuReportCharXML"
        Me.mnuReportCharXML.Size = New System.Drawing.Size(258, 22)
        Me.mnuReportCharXML.Text = "Character XML"
        '
        'mnuReportTrainXML
        '
        Me.mnuReportTrainXML.Name = "mnuReportTrainXML"
        Me.mnuReportTrainXML.Size = New System.Drawing.Size(258, 22)
        Me.mnuReportTrainXML.Text = "Training XML"
        '
        'mnuReportCurrentCharXMLOld
        '
        Me.mnuReportCurrentCharXMLOld.Name = "mnuReportCurrentCharXMLOld"
        Me.mnuReportCurrentCharXMLOld.Size = New System.Drawing.Size(258, 22)
        Me.mnuReportCurrentCharXMLOld.Text = "Current Character XML (Old Style)"
        '
        'mnuReportCurrentCharXMLNew
        '
        Me.mnuReportCurrentCharXMLNew.Name = "mnuReportCurrentCharXMLNew"
        Me.mnuReportCurrentCharXMLNew.Size = New System.Drawing.Size(258, 22)
        Me.mnuReportCurrentCharXMLNew.Text = "Current Character XML (New Style)"
        '
        'mnuReportCurrentTrainingXMLOld
        '
        Me.mnuReportCurrentTrainingXMLOld.Name = "mnuReportCurrentTrainingXMLOld"
        Me.mnuReportCurrentTrainingXMLOld.Size = New System.Drawing.Size(258, 22)
        Me.mnuReportCurrentTrainingXMLOld.Text = "Current Training XML (Old Style)"
        '
        'mnuECMExport
        '
        Me.mnuECMExport.Name = "mnuECMExport"
        Me.mnuECMExport.Size = New System.Drawing.Size(214, 22)
        Me.mnuECMExport.Text = "ECM Export (Current Char)"
        '
        'mnuReportsCharts
        '
        Me.mnuReportsCharts.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportsChartsChar})
        Me.mnuReportsCharts.Name = "mnuReportsCharts"
        Me.mnuReportsCharts.Size = New System.Drawing.Size(177, 22)
        Me.mnuReportsCharts.Text = "Charts && Graphs"
        '
        'mnuReportsChartsChar
        '
        Me.mnuReportsChartsChar.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportSkillGroupChart})
        Me.mnuReportsChartsChar.Name = "mnuReportsChartsChar"
        Me.mnuReportsChartsChar.Size = New System.Drawing.Size(125, 22)
        Me.mnuReportsChartsChar.Text = "Character"
        '
        'mnuReportSkillGroupChart
        '
        Me.mnuReportSkillGroupChart.Name = "mnuReportSkillGroupChart"
        Me.mnuReportSkillGroupChart.Size = New System.Drawing.Size(163, 22)
        Me.mnuReportSkillGroupChart.Text = "Skill Group Chart"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(174, 6)
        '
        'mnuReportOpenfolder
        '
        Me.mnuReportOpenfolder.Name = "mnuReportOpenfolder"
        Me.mnuReportOpenfolder.Size = New System.Drawing.Size(177, 22)
        Me.mnuReportOpenfolder.Text = "Open Report Folder"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpCheckUpdates, Me.VersionHistoryToolStripMenuItem, Me.ToolStripSeparator1, Me.mnuHelpAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpCheckUpdates
        '
        Me.mnuHelpCheckUpdates.Image = CType(resources.GetObject("mnuHelpCheckUpdates.Image"), System.Drawing.Image)
        Me.mnuHelpCheckUpdates.Name = "mnuHelpCheckUpdates"
        Me.mnuHelpCheckUpdates.Size = New System.Drawing.Size(171, 22)
        Me.mnuHelpCheckUpdates.Text = "&Check for Updates"
        '
        'VersionHistoryToolStripMenuItem
        '
        Me.VersionHistoryToolStripMenuItem.Name = "VersionHistoryToolStripMenuItem"
        Me.VersionHistoryToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.VersionHistoryToolStripMenuItem.Text = "Version History"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(168, 6)
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Image = CType(resources.GetObject("mnuHelpAbout.Image"), System.Drawing.Image)
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(171, 22)
        Me.mnuHelpAbout.Text = "&About..."
        '
        'tmrEve
        '
        Me.tmrEve.Interval = 3000
        '
        'EveStatusIcon
        '
        Me.EveStatusIcon.ContextMenuStrip = Me.EveIconMenu
        Me.EveStatusIcon.Icon = CType(resources.GetObject("EveStatusIcon.Icon"), System.Drawing.Icon)
        Me.EveStatusIcon.Visible = True
        '
        'EveIconMenu
        '
        Me.EveIconMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxmnuLaunchEve1, Me.ctxmnuLaunchEve2, Me.ctxmnuLaunchEve3, Me.ctxmnuLaunchEve4, Me.ToolStripSeparator4, Me.ForceServerCheckToolStripMenuItem, Me.RestoreWindowToolStripMenuItem, Me.HideWhenMinimisedToolStripMenuItem, Me.ToolStripSeparator5, Me.ctxAbout, Me.ctxExit})
        Me.EveIconMenu.Name = "ContextMenuStrip1"
        Me.EveIconMenu.Size = New System.Drawing.Size(193, 214)
        '
        'ctxmnuLaunchEve1
        '
        Me.ctxmnuLaunchEve1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxLaunchEve1Normal, Me.ctxLaunchEve1Full})
        Me.ctxmnuLaunchEve1.Name = "ctxmnuLaunchEve1"
        Me.ctxmnuLaunchEve1.Size = New System.Drawing.Size(192, 22)
        Me.ctxmnuLaunchEve1.Text = "Launch Eve (1)"
        '
        'ctxLaunchEve1Normal
        '
        Me.ctxLaunchEve1Normal.Name = "ctxLaunchEve1Normal"
        Me.ctxLaunchEve1Normal.Size = New System.Drawing.Size(182, 22)
        Me.ctxLaunchEve1Normal.Text = "Launch Normal"
        '
        'ctxLaunchEve1Full
        '
        Me.ctxLaunchEve1Full.Name = "ctxLaunchEve1Full"
        Me.ctxLaunchEve1Full.Size = New System.Drawing.Size(182, 22)
        Me.ctxLaunchEve1Full.Text = "Launch Full Window"
        '
        'ctxmnuLaunchEve2
        '
        Me.ctxmnuLaunchEve2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxLaunchEve2Normal, Me.ctxLaunchEve2Full})
        Me.ctxmnuLaunchEve2.Name = "ctxmnuLaunchEve2"
        Me.ctxmnuLaunchEve2.Size = New System.Drawing.Size(192, 22)
        Me.ctxmnuLaunchEve2.Text = "Launch Eve (2)"
        '
        'ctxLaunchEve2Normal
        '
        Me.ctxLaunchEve2Normal.Name = "ctxLaunchEve2Normal"
        Me.ctxLaunchEve2Normal.Size = New System.Drawing.Size(182, 22)
        Me.ctxLaunchEve2Normal.Text = "Launch Normal"
        '
        'ctxLaunchEve2Full
        '
        Me.ctxLaunchEve2Full.Name = "ctxLaunchEve2Full"
        Me.ctxLaunchEve2Full.Size = New System.Drawing.Size(182, 22)
        Me.ctxLaunchEve2Full.Text = "Launch Full Window"
        '
        'ctxmnuLaunchEve3
        '
        Me.ctxmnuLaunchEve3.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxLaunchEve3Normal, Me.ctxLaunchEve3Full})
        Me.ctxmnuLaunchEve3.Name = "ctxmnuLaunchEve3"
        Me.ctxmnuLaunchEve3.Size = New System.Drawing.Size(192, 22)
        Me.ctxmnuLaunchEve3.Text = "Launch Eve (3)"
        '
        'ctxLaunchEve3Normal
        '
        Me.ctxLaunchEve3Normal.Name = "ctxLaunchEve3Normal"
        Me.ctxLaunchEve3Normal.Size = New System.Drawing.Size(182, 22)
        Me.ctxLaunchEve3Normal.Text = "Launch Normal"
        '
        'ctxLaunchEve3Full
        '
        Me.ctxLaunchEve3Full.Name = "ctxLaunchEve3Full"
        Me.ctxLaunchEve3Full.Size = New System.Drawing.Size(182, 22)
        Me.ctxLaunchEve3Full.Text = "Launch Full Window"
        '
        'ctxmnuLaunchEve4
        '
        Me.ctxmnuLaunchEve4.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxLaunchEve4Normal, Me.ctxLaunchEve4Full})
        Me.ctxmnuLaunchEve4.Name = "ctxmnuLaunchEve4"
        Me.ctxmnuLaunchEve4.Size = New System.Drawing.Size(192, 22)
        Me.ctxmnuLaunchEve4.Text = "Launch Eve (4)"
        '
        'ctxLaunchEve4Normal
        '
        Me.ctxLaunchEve4Normal.Name = "ctxLaunchEve4Normal"
        Me.ctxLaunchEve4Normal.Size = New System.Drawing.Size(182, 22)
        Me.ctxLaunchEve4Normal.Text = "Launch Normal"
        '
        'ctxLaunchEve4Full
        '
        Me.ctxLaunchEve4Full.Name = "ctxLaunchEve4Full"
        Me.ctxLaunchEve4Full.Size = New System.Drawing.Size(182, 22)
        Me.ctxLaunchEve4Full.Text = "Launch Full Window"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(189, 6)
        '
        'ForceServerCheckToolStripMenuItem
        '
        Me.ForceServerCheckToolStripMenuItem.Name = "ForceServerCheckToolStripMenuItem"
        Me.ForceServerCheckToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.ForceServerCheckToolStripMenuItem.Text = "Force Server Check"
        '
        'RestoreWindowToolStripMenuItem
        '
        Me.RestoreWindowToolStripMenuItem.Name = "RestoreWindowToolStripMenuItem"
        Me.RestoreWindowToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.RestoreWindowToolStripMenuItem.Text = "Restore Window"
        '
        'HideWhenMinimisedToolStripMenuItem
        '
        Me.HideWhenMinimisedToolStripMenuItem.Checked = True
        Me.HideWhenMinimisedToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.HideWhenMinimisedToolStripMenuItem.Name = "HideWhenMinimisedToolStripMenuItem"
        Me.HideWhenMinimisedToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.HideWhenMinimisedToolStripMenuItem.Text = "Hide When Minimised"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(189, 6)
        '
        'ctxAbout
        '
        Me.ctxAbout.Name = "ctxAbout"
        Me.ctxAbout.Size = New System.Drawing.Size(192, 22)
        Me.ctxAbout.Text = "About"
        '
        'ctxExit
        '
        Me.ctxExit.Name = "ctxExit"
        Me.ctxExit.Size = New System.Drawing.Size(192, 22)
        Me.ctxExit.Text = "Exit"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsTQStatus, Me.tsSisiStatus, Me.tsLogonStatus, Me.tsProgramStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 631)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(917, 25)
        Me.StatusStrip1.TabIndex = 10
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tsTQStatus
        '
        Me.tsTQStatus.AutoSize = False
        Me.tsTQStatus.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsTQStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.tsTQStatus.Name = "tsTQStatus"
        Me.tsTQStatus.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.tsTQStatus.Size = New System.Drawing.Size(250, 20)
        Me.tsTQStatus.Text = "Status"
        Me.tsTQStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsSisiStatus
        '
        Me.tsSisiStatus.AutoSize = False
        Me.tsSisiStatus.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsSisiStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.tsSisiStatus.Name = "tsSisiStatus"
        Me.tsSisiStatus.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.tsSisiStatus.Size = New System.Drawing.Size(250, 20)
        Me.tsSisiStatus.Text = "Status"
        Me.tsSisiStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsLogonStatus
        '
        Me.tsLogonStatus.AutoSize = False
        Me.tsLogonStatus.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsLogonStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.tsLogonStatus.Name = "tsLogonStatus"
        Me.tsLogonStatus.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.tsLogonStatus.Size = New System.Drawing.Size(300, 20)
        Me.tsLogonStatus.Text = "Logon Status:"
        Me.tsLogonStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsProgramStatus
        '
        Me.tsProgramStatus.AutoSize = False
        Me.tsProgramStatus.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsProgramStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.tsProgramStatus.Name = "tsProgramStatus"
        Me.tsProgramStatus.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.tsProgramStatus.Size = New System.Drawing.Size(150, 20)
        Me.tsProgramStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tmrSkillUpdate
        '
        Me.tmrSkillUpdate.Interval = 1
        '
        'tmrBackup
        '
        Me.tmrBackup.Enabled = True
        Me.tmrBackup.Interval = 60000
        '
        'tmrModules
        '
        Me.tmrModules.Interval = 250
        '
        'tabMDI
        '
        Me.tabMDI.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMDI.ContextMenuStrip = Me.ctxTabbedMDI
        Me.tabMDI.Dock = System.Windows.Forms.DockStyle.Top
        Me.tabMDI.Location = New System.Drawing.Point(200, 49)
        Me.tabMDI.Name = "tabMDI"
        Me.tabMDI.SelectedIndex = 0
        Me.tabMDI.Size = New System.Drawing.Size(717, 22)
        Me.tabMDI.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabMDI.TabIndex = 16
        '
        'ctxTabbedMDI
        '
        Me.ctxTabbedMDI.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCloseMDITab})
        Me.ctxTabbedMDI.Name = "ctxTabbedMDI"
        Me.ctxTabbedMDI.Size = New System.Drawing.Size(127, 26)
        '
        'mnuCloseMDITab
        '
        Me.mnuCloseMDITab.Name = "mnuCloseMDITab"
        Me.mnuCloseMDITab.Size = New System.Drawing.Size(126, 22)
        Me.mnuCloseMDITab.Text = "Close Tab"
        '
        'tmrEveWindow
        '
        Me.tmrEveWindow.Interval = 500
        '
        'ctxPlugin
        '
        Me.ctxPlugin.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLoadPlugin})
        Me.ctxPlugin.Name = "ctxPlugin"
        Me.ctxPlugin.Size = New System.Drawing.Size(143, 26)
        '
        'mnuLoadPlugin
        '
        Me.mnuLoadPlugin.Name = "mnuLoadPlugin"
        Me.mnuLoadPlugin.Size = New System.Drawing.Size(142, 22)
        Me.mnuLoadPlugin.Text = "Load Plug-in"
        '
        'XPanderList1
        '
        Me.XPanderList1.AutoScroll = True
        Me.XPanderList1.BackColor = System.Drawing.Color.Navy
        Me.XPanderList1.BackColorDark = System.Drawing.Color.Navy
        Me.XPanderList1.BackColorLight = System.Drawing.Color.Navy
        Me.XPanderList1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.XPanderList1.Controls.Add(Me.XPPilots)
        Me.XPanderList1.Controls.Add(Me.XPTraining)
        Me.XPanderList1.Controls.Add(Me.XPModules)
        Me.XPanderList1.Dock = System.Windows.Forms.DockStyle.Left
        Me.XPanderList1.Location = New System.Drawing.Point(0, 49)
        Me.XPanderList1.Name = "XPanderList1"
        Me.XPanderList1.Size = New System.Drawing.Size(200, 582)
        Me.XPanderList1.TabIndex = 14
        '
        'XPPilots
        '
        Me.XPPilots.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XPPilots.Animated = True
        Me.XPPilots.AnimationTime = 50
        Me.XPPilots.BackColor = System.Drawing.Color.Transparent
        Me.XPPilots.BorderStyle = System.Windows.Forms.Border3DStyle.Flat
        Me.XPPilots.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.XPPilots.CaptionFormatFlag = EveHQ.XPander.FormatFlag.NoWrap
        Me.XPPilots.CaptionLeftColor = System.Drawing.Color.RoyalBlue
        Me.XPPilots.CaptionRightColor = System.Drawing.Color.LightSteelBlue
        Me.XPPilots.CaptionStyle = EveHQ.XPander.CaptionStyleEnum.Normal
        Me.XPPilots.CaptionText = "Available Pilots"
        Me.XPPilots.CaptionTextAlign = EveHQ.XPander.CaptionTextAlignment.Left
        Me.XPPilots.CaptionTextColor = System.Drawing.Color.Black
        Me.XPPilots.CaptionTextHighlightColor = System.Drawing.Color.LightSteelBlue
        Me.XPPilots.ChevronStyle = EveHQ.XPander.ChevronStyleEnum.Image
        Me.XPPilots.CollapsedHighlightImage = CType(resources.GetObject("XPPilots.CollapsedHighlightImage"), System.Drawing.Bitmap)
        Me.XPPilots.CollapsedImage = CType(resources.GetObject("XPPilots.CollapsedImage"), System.Drawing.Bitmap)
        Me.XPPilots.ExpandedHighlightImage = CType(resources.GetObject("XPPilots.ExpandedHighlightImage"), System.Drawing.Bitmap)
        Me.XPPilots.ExpandedImage = CType(resources.GetObject("XPPilots.ExpandedImage"), System.Drawing.Bitmap)
        Me.XPPilots.Location = New System.Drawing.Point(4, 273)
        Me.XPPilots.Name = "XPPilots"
        Me.XPPilots.Padding = New System.Windows.Forms.Padding(0, 25, 0, 0)
        Me.XPPilots.PaneBottomRightColor = System.Drawing.Color.LightSteelBlue
        Me.XPPilots.PaneOutlineColor = System.Drawing.Color.SteelBlue
        Me.XPPilots.PaneTopLeftColor = System.Drawing.Color.LightSteelBlue
        Me.XPPilots.Size = New System.Drawing.Size(189, 177)
        Me.XPPilots.TabIndex = 2
        Me.XPPilots.Tag = 0
        Me.XPPilots.TooltipText = Nothing
        '
        'XPTraining
        '
        Me.XPTraining.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XPTraining.Animated = True
        Me.XPTraining.AnimationTime = 50
        Me.XPTraining.BackColor = System.Drawing.Color.Transparent
        Me.XPTraining.BorderStyle = System.Windows.Forms.Border3DStyle.Flat
        Me.XPTraining.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.XPTraining.CaptionFormatFlag = EveHQ.XPander.FormatFlag.NoWrap
        Me.XPTraining.CaptionLeftColor = System.Drawing.Color.RoyalBlue
        Me.XPTraining.CaptionRightColor = System.Drawing.Color.LightSteelBlue
        Me.XPTraining.CaptionStyle = EveHQ.XPander.CaptionStyleEnum.Normal
        Me.XPTraining.CaptionText = "Training Status"
        Me.XPTraining.CaptionTextAlign = EveHQ.XPander.CaptionTextAlignment.Left
        Me.XPTraining.CaptionTextColor = System.Drawing.Color.Black
        Me.XPTraining.CaptionTextHighlightColor = System.Drawing.Color.LightSteelBlue
        Me.XPTraining.ChevronStyle = EveHQ.XPander.ChevronStyleEnum.Image
        Me.XPTraining.CollapsedHighlightImage = CType(resources.GetObject("XPTraining.CollapsedHighlightImage"), System.Drawing.Bitmap)
        Me.XPTraining.CollapsedImage = CType(resources.GetObject("XPTraining.CollapsedImage"), System.Drawing.Bitmap)
        Me.XPTraining.Controls.Add(Me.lblTrainingStatus)
        Me.XPTraining.ExpandedHighlightImage = CType(resources.GetObject("XPTraining.ExpandedHighlightImage"), System.Drawing.Bitmap)
        Me.XPTraining.ExpandedImage = CType(resources.GetObject("XPTraining.ExpandedImage"), System.Drawing.Bitmap)
        Me.XPTraining.Location = New System.Drawing.Point(4, 160)
        Me.XPTraining.Name = "XPTraining"
        Me.XPTraining.Padding = New System.Windows.Forms.Padding(0, 25, 0, 0)
        Me.XPTraining.PaneBottomRightColor = System.Drawing.Color.LightSteelBlue
        Me.XPTraining.PaneOutlineColor = System.Drawing.Color.SteelBlue
        Me.XPTraining.PaneTopLeftColor = System.Drawing.Color.LightSteelBlue
        Me.XPTraining.Size = New System.Drawing.Size(189, 107)
        Me.XPTraining.TabIndex = 1
        Me.XPTraining.Tag = 1
        Me.XPTraining.TooltipText = Nothing
        '
        'lblTrainingStatus
        '
        Me.lblTrainingStatus.AutoSize = True
        Me.lblTrainingStatus.Location = New System.Drawing.Point(6, 36)
        Me.lblTrainingStatus.MaximumSize = New System.Drawing.Size(175, 0)
        Me.lblTrainingStatus.Name = "lblTrainingStatus"
        Me.lblTrainingStatus.Size = New System.Drawing.Size(68, 13)
        Me.lblTrainingStatus.TabIndex = 1
        Me.lblTrainingStatus.Text = "Calculating..."
        '
        'XPModules
        '
        Me.XPModules.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XPModules.Animated = True
        Me.XPModules.AnimationTime = 50
        Me.XPModules.BackColor = System.Drawing.Color.Transparent
        Me.XPModules.BorderStyle = System.Windows.Forms.Border3DStyle.Flat
        Me.XPModules.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.XPModules.CaptionFormatFlag = EveHQ.XPander.FormatFlag.NoWrap
        Me.XPModules.CaptionLeftColor = System.Drawing.Color.RoyalBlue
        Me.XPModules.CaptionRightColor = System.Drawing.Color.LightSteelBlue
        Me.XPModules.CaptionStyle = EveHQ.XPander.CaptionStyleEnum.Normal
        Me.XPModules.CaptionText = "EveHQ Module Status"
        Me.XPModules.CaptionTextAlign = EveHQ.XPander.CaptionTextAlignment.Left
        Me.XPModules.CaptionTextColor = System.Drawing.Color.Black
        Me.XPModules.CaptionTextHighlightColor = System.Drawing.Color.LightSteelBlue
        Me.XPModules.ChevronStyle = EveHQ.XPander.ChevronStyleEnum.Image
        Me.XPModules.CollapsedHighlightImage = CType(resources.GetObject("XPModules.CollapsedHighlightImage"), System.Drawing.Bitmap)
        Me.XPModules.CollapsedImage = CType(resources.GetObject("XPModules.CollapsedImage"), System.Drawing.Bitmap)
        Me.XPModules.ExpandedHighlightImage = CType(resources.GetObject("XPModules.ExpandedHighlightImage"), System.Drawing.Bitmap)
        Me.XPModules.ExpandedImage = CType(resources.GetObject("XPModules.ExpandedImage"), System.Drawing.Bitmap)
        Me.XPModules.Location = New System.Drawing.Point(4, 4)
        Me.XPModules.Name = "XPModules"
        Me.XPModules.Padding = New System.Windows.Forms.Padding(0, 25, 0, 0)
        Me.XPModules.PaneBottomRightColor = System.Drawing.Color.LightSteelBlue
        Me.XPModules.PaneOutlineColor = System.Drawing.Color.SteelBlue
        Me.XPModules.PaneTopLeftColor = System.Drawing.Color.LightSteelBlue
        Me.XPModules.Size = New System.Drawing.Size(189, 150)
        Me.XPModules.TabIndex = 0
        Me.XPModules.Tag = 2
        Me.XPModules.TooltipText = Nothing
        '
        'btnAddAccount
        '
        Me.btnAddAccount.Image = CType(resources.GetObject("btnAddAccount.Image"), System.Drawing.Image)
        Me.btnAddAccount.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAddAccount.Name = "btnAddAccount"
        Me.btnAddAccount.Size = New System.Drawing.Size(118, 22)
        Me.btnAddAccount.Text = "Add API Account"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 25)
        '
        'frmEveHQ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(917, 656)
        Me.Controls.Add(Me.tabMDI)
        Me.Controls.Add(Me.XPanderList1)
        Me.Controls.Add(Me.ToolStrip)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Name = "frmEveHQ"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EveHQ"
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.EveIconMenu.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ctxTabbedMDI.ResumeLayout(False)
        Me.ctxPlugin.ResumeLayout(False)
        Me.XPanderList1.ResumeLayout(False)
        Me.XPTraining.ResumeLayout(False)
        Me.XPTraining.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ArrangeIconsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CascadeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileVerticalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileHorizontalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ViewMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrEve As System.Windows.Forms.Timer
    Friend WithEvents EveStatusIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents EveIconMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ForceServerCheckToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RestoreWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideWhenMinimisedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents tsTQStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsSisiStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsLogonStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents PilotInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RunIGBToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrSkillUpdate As System.Windows.Forms.Timer
    Friend WithEvents WebBrowserToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsProgramStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents cboPilots As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuToolsGetAccountInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SkillTrainingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsHTMLChar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsCharCharsheet As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuReportCharTraintimes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportOpenfolder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnureportCharSkillLevels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportCharXML As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportTrainXML As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportTrainingQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportAsteroids As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportAsteroidRocks As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportAsteroidIce As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxmnuLaunchEve1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxmnuLaunchEve2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxmnuLaunchEve3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxmnuLaunchEve4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuBackup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrBackup As System.Windows.Forms.Timer
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRepCharSummary As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpCheckUpdates As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VersionHistoryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuReportSPSummary As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportSkillsAvailable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrModules As System.Windows.Forms.Timer
    Friend WithEvents mnuModules As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NoModulesLoadedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportCurrentCharXMLOld As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents XPanderList1 As XPanderList
    Friend WithEvents XPModules As XPander
    Friend WithEvents XPTraining As XPander
    Friend WithEvents lblTrainingStatus As System.Windows.Forms.Label
    Friend WithEvents XPPilots As EveHQ.XPander
    Friend WithEvents mnuReportCurrentCharXMLNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportTimeToLevel5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMDI As System.Windows.Forms.TabControl
    Friend WithEvents TrainingInformationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbRetrieveData As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPilotInfo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSkillTraining As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbWebBrowser As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbTrainingOverlay As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSettingsBackup As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSettings As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbAbout As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbIGB As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCheckUpdates As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnTogglePanel As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxTabbedMDI As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCloseMDITab As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportCurrentTrainingXMLOld As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuECMExport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents fbd1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents mnuReportSkillGroupChart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportSkillsNotTrained As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsHTML As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsXML As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsText As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsCharts As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsXMLChar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextChar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsChartsChar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextCharSheet As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextTrainTimes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextTimeToLevel5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextSkillLevels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextTrainingQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextSkillsAvailable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextSkillsNotTrained As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxLaunchEve1Normal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxLaunchEve1Full As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxLaunchEve2Normal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxLaunchEve2Full As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxLaunchEve3Normal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxLaunchEve3Full As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxLaunchEve4Normal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxLaunchEve4Full As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrEveWindow As System.Windows.Forms.Timer
    Friend WithEvents mnuReportQueueShoppingList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextShoppingList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxPlugin As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuLoadPlugin As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolsAPIChecker As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportPartiallyTrainedSkills As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportsTextPartiallyTrainedSkills As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ClearEveHQCache As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolsTriggerError As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReportAsteroidAlloys As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnAddAccount As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator

End Class
