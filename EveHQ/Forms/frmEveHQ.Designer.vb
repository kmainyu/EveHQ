<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmEveHQ
    Inherits DevComponents.DotNetBar.Office2007RibbonForm

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEveHQ))
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.rpPlugins = New DevComponents.DotNetBar.RibbonPanel()
        Me.rbPlugins = New DevComponents.DotNetBar.RibbonBar()
        Me.tmrEve = New System.Windows.Forms.Timer(Me.components)
        Me.EveIconMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ctxmnuLaunchEve1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxmnuLaunchEve2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxmnuLaunchEve3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxmnuLaunchEve4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ForceServerCheckToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideWhenMinimisedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ctxAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmrSkillUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.tmrSave = New System.Windows.Forms.Timer(Me.components)
        Me.tmrBackup = New System.Windows.Forms.Timer(Me.components)
        Me.tmrModules = New System.Windows.Forms.Timer(Me.components)
        Me.ctxTabbedMDI = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCloseMDITab = New System.Windows.Forms.ToolStripMenuItem()
        Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.iconEveHQMLW = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tmrMemory = New System.Windows.Forms.Timer(Me.components)
        Me.RibbonControl1 = New DevComponents.DotNetBar.RibbonControl()
        Me.rpCore = New DevComponents.DotNetBar.RibbonPanel()
        Me.rbHelp = New DevComponents.DotNetBar.RibbonBar()
        Me.btnInfoHelp = New DevComponents.DotNetBar.ButtonItem()
        Me.rbSQLTools = New DevComponents.DotNetBar.RibbonBar()
        Me.ItemContainer3 = New DevComponents.DotNetBar.ItemContainer()
        Me.btnSQLQueryTool = New DevComponents.DotNetBar.ButtonItem()
        Me.rbAPITools = New DevComponents.DotNetBar.RibbonBar()
        Me.ItemContainer2 = New DevComponents.DotNetBar.ItemContainer()
        Me.btnAPIChecker = New DevComponents.DotNetBar.ButtonItem()
        Me.btnOpenCacheFolder = New DevComponents.DotNetBar.ButtonItem()
        Me.btnClearCache = New DevComponents.DotNetBar.ButtonItem()
        Me.btnClearCharacterCache = New DevComponents.DotNetBar.ButtonItem()
        Me.btnClearImageCache = New DevComponents.DotNetBar.ButtonItem()
        Me.btnClearAllCache = New DevComponents.DotNetBar.ButtonItem()
        Me.rbBackup = New DevComponents.DotNetBar.RibbonBar()
        Me.ItemContainer5 = New DevComponents.DotNetBar.ItemContainer()
        Me.btnBackupEveHQ = New DevComponents.DotNetBar.ButtonItem()
        Me.btnBackupEve = New DevComponents.DotNetBar.ButtonItem()
        Me.rbIGB = New DevComponents.DotNetBar.RibbonBar()
        Me.btnIGB = New DevComponents.DotNetBar.ButtonItem()
        Me.lblIGB = New DevComponents.DotNetBar.LabelItem()
        Me.rbEveMail = New DevComponents.DotNetBar.RibbonBar()
        Me.btnEveMail = New DevComponents.DotNetBar.ButtonItem()
        Me.lblEveMail = New DevComponents.DotNetBar.LabelItem()
        Me.rbView = New DevComponents.DotNetBar.RibbonBar()
        Me.btnViewPilotInfo = New DevComponents.DotNetBar.ButtonItem()
        Me.btnViewSkillTraining = New DevComponents.DotNetBar.ButtonItem()
        Me.btnViewPrices = New DevComponents.DotNetBar.ButtonItem()
        Me.btnViewDashboard = New DevComponents.DotNetBar.ButtonItem()
        Me.btnViewReqs = New DevComponents.DotNetBar.ButtonItem()
        Me.rbAPI = New DevComponents.DotNetBar.RibbonBar()
        Me.btnManageAPI = New DevComponents.DotNetBar.ButtonItem()
        Me.btnQueryAPI = New DevComponents.DotNetBar.ButtonItem()
        Me.rbOptions = New DevComponents.DotNetBar.RibbonBar()
        Me.btnSave = New DevComponents.DotNetBar.ButtonItem()
        Me.rpReports = New DevComponents.DotNetBar.RibbonPanel()
        Me.rbCharts = New DevComponents.DotNetBar.RibbonBar()
        Me.btnChartSkillGroup = New DevComponents.DotNetBar.ButtonItem()
        Me.btnChartSkillCost = New DevComponents.DotNetBar.ButtonItem()
        Me.rbPHPBB = New DevComponents.DotNetBar.RibbonBar()
        Me.btnPHPBBCharacterSheet = New DevComponents.DotNetBar.ButtonItem()
        Me.rbXML = New DevComponents.DotNetBar.RibbonBar()
        Me.btnXMLCharacterXML = New DevComponents.DotNetBar.ButtonItem()
        Me.btnXMLTrainingXML = New DevComponents.DotNetBar.ButtonItem()
        Me.btnXMLCurrentCharOld = New DevComponents.DotNetBar.ButtonItem()
        Me.btnXMLCurrentCharNew = New DevComponents.DotNetBar.ButtonItem()
        Me.btnXMLCurrentTrainingOld = New DevComponents.DotNetBar.ButtonItem()
        Me.btnXMLECMExport = New DevComponents.DotNetBar.ButtonItem()
        Me.rbText = New DevComponents.DotNetBar.RibbonBar()
        Me.btnTextCharacterSheet = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextTrainingTimes = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextTimeToLvl5 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextSkillLevels = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextTrainingQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextQueueShoppingList = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextSkillsAvailable = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextSkillsNotTrained = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextPartiallyTrained = New DevComponents.DotNetBar.ButtonItem()
        Me.btnTextSkillsCost = New DevComponents.DotNetBar.ButtonItem()
        Me.rbHTML = New DevComponents.DotNetBar.RibbonBar()
        Me.btnHTMLCharSheet = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLTrainingTimes = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLTimeToLvl5 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLSkillLevels = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLTrainingQueue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLQueueShoppingList = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLSkillsAvailable = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLSkillsNotTrained = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLPartiallyTrained = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHTMLSkillsCost = New DevComponents.DotNetBar.ButtonItem()
        Me.rbStandard = New DevComponents.DotNetBar.RibbonBar()
        Me.btnStdCharSummary = New DevComponents.DotNetBar.ButtonItem()
        Me.btnStdSkillLevels = New DevComponents.DotNetBar.ButtonItem()
        Me.btnStdAlloyReport = New DevComponents.DotNetBar.ButtonItem()
        Me.btnStdAsteroidReport = New DevComponents.DotNetBar.ButtonItem()
        Me.btnStdIceReport = New DevComponents.DotNetBar.ButtonItem()
        Me.rbReportOptions = New DevComponents.DotNetBar.RibbonBar()
        Me.icReportOptions = New DevComponents.DotNetBar.ItemContainer()
        Me.icReportPilot = New DevComponents.DotNetBar.ItemContainer()
        Me.lblReportPilot = New DevComponents.DotNetBar.LabelItem()
        Me.cboReportPilot = New DevComponents.DotNetBar.ComboBoxItem()
        Me.icReportFormat = New DevComponents.DotNetBar.ItemContainer()
        Me.lblReportFormat = New DevComponents.DotNetBar.LabelItem()
        Me.cboReportFormat = New DevComponents.DotNetBar.ComboBoxItem()
        Me.ciReportHTML = New DevComponents.Editors.ComboItem()
        Me.ciReportText = New DevComponents.Editors.ComboItem()
        Me.ciReportXML = New DevComponents.Editors.ComboItem()
        Me.ciReportPHPBB = New DevComponents.Editors.ComboItem()
        Me.ciReportChart = New DevComponents.Editors.ComboItem()
        Me.btnOpenReportFolder = New DevComponents.DotNetBar.ButtonItem()
        Me.rtiCore = New DevComponents.DotNetBar.RibbonTabItem()
        Me.rtiPlugins = New DevComponents.DotNetBar.RibbonTabItem()
        Me.rtiReports = New DevComponents.DotNetBar.RibbonTabItem()
        Me.btnTheme = New DevComponents.DotNetBar.ButtonItem()
        Me.btnOffice2007Black = New DevComponents.DotNetBar.ButtonItem()
        Me.AppCommandTheme = New DevComponents.DotNetBar.Command(Me.components)
        Me.btnOffice2007Blue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnOffice2007Silver = New DevComponents.DotNetBar.ButtonItem()
        Me.btnOffice2010Black = New DevComponents.DotNetBar.ButtonItem()
        Me.btnOffice2010Blue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnOffice2010Silver = New DevComponents.DotNetBar.ButtonItem()
        Me.btnOffice2007VistaGlass = New DevComponents.DotNetBar.ButtonItem()
        Me.btnWindows7Blue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnVisualStudio2010Blue = New DevComponents.DotNetBar.ButtonItem()
        Me.btnCustomTheme = New DevComponents.DotNetBar.ColorPickerDropDown()
        Me.Office2007StartButton1 = New DevComponents.DotNetBar.Office2007StartButton()
        Me.ItemContainer1 = New DevComponents.DotNetBar.ItemContainer()
        Me.btnFileSettings = New DevComponents.DotNetBar.ButtonItem()
        Me.btnHelp = New DevComponents.DotNetBar.ButtonItem()
        Me.btnCheckForUpdates = New DevComponents.DotNetBar.ButtonItem()
        Me.btnUpdateEveHQ = New DevComponents.DotNetBar.ButtonItem()
        Me.btnViewHistory = New DevComponents.DotNetBar.ButtonItem()
        Me.btnAbout = New DevComponents.DotNetBar.ButtonItem()
        Me.btnFileExit = New DevComponents.DotNetBar.ButtonItem()
        Me.QatCustomizeItem1 = New DevComponents.DotNetBar.QatCustomizeItem()
        Me.DotNetBarManager1 = New DevComponents.DotNetBar.DotNetBarManager(Me.components)
        Me.DockSite4 = New DevComponents.DotNetBar.DockSite()
        Me.Bar1 = New DevComponents.DotNetBar.Bar()
        Me.pdc1 = New DevComponents.DotNetBar.PanelDockContainer()
        Me.DockContainerItem1 = New DevComponents.DotNetBar.DockContainerItem()
        Me.DockSite1 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite2 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite8 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite5 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite6 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite7 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite3 = New DevComponents.DotNetBar.DockSite()
        Me.tabEveHQMDI = New DevComponents.DotNetBar.TabStrip()
        Me.TabItem1 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.barStatus = New DevComponents.DotNetBar.Bar()
        Me.lblTQStatus = New DevComponents.DotNetBar.LabelItem()
        Me.lblAPIStatus = New DevComponents.DotNetBar.LabelItem()
        Me.lblMailAPITime = New DevComponents.DotNetBar.LabelItem()
        Me.lblCharAPITime = New DevComponents.DotNetBar.LabelItem()
        Me.lblEveTime = New DevComponents.DotNetBar.LabelItem()
        Me.SuperTooltip1 = New DevComponents.DotNetBar.SuperTooltip()
        Me.StyleManager1 = New DevComponents.DotNetBar.StyleManager(Me.components)
        Me.EveStatusIcon = New EveHQ.Core.EveHQIcon(Me.components)
        Me.rpPlugins.SuspendLayout()
        Me.EveIconMenu.SuspendLayout()
        Me.ctxTabbedMDI.SuspendLayout()
        Me.RibbonControl1.SuspendLayout()
        Me.rpCore.SuspendLayout()
        Me.rpReports.SuspendLayout()
        Me.DockSite4.SuspendLayout()
        CType(Me.Bar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Bar1.SuspendLayout()
        CType(Me.barStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rpPlugins
        '
        Me.rpPlugins.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rpPlugins.Controls.Add(Me.rbPlugins)
        Me.rpPlugins.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rpPlugins.Location = New System.Drawing.Point(0, 56)
        Me.rpPlugins.Name = "rpPlugins"
        Me.rpPlugins.Padding = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.rpPlugins.Size = New System.Drawing.Size(1231, 96)
        Me.rpPlugins.StretchLastRibbonBar = True
        '
        '
        '
        Me.rpPlugins.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rpPlugins.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rpPlugins.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rpPlugins.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.rpPlugins, "Shows all available Plug-ins")
        Me.rpPlugins.Visible = False
        '
        'rbPlugins
        '
        Me.rbPlugins.AutoOverflowEnabled = False
        Me.rbPlugins.AutoSizeItems = False
        '
        '
        '
        Me.rbPlugins.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbPlugins.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbPlugins.ContainerControlProcessDialogKey = True
        Me.rbPlugins.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbPlugins.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbPlugins.Location = New System.Drawing.Point(3, 0)
        Me.rbPlugins.Name = "rbPlugins"
        Me.rbPlugins.OverflowButtonText = "Click for more plug-ins!"
        Me.rbPlugins.Size = New System.Drawing.Size(1017, 93)
        Me.rbPlugins.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbPlugins.TabIndex = 0
        Me.rbPlugins.Text = "EveHQ Plug-ins"
        '
        '
        '
        Me.rbPlugins.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbPlugins.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'tmrEve
        '
        Me.tmrEve.Interval = 3000
        '
        'EveIconMenu
        '
        Me.EveIconMenu.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EveIconMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxmnuLaunchEve1, Me.ctxmnuLaunchEve2, Me.ctxmnuLaunchEve3, Me.ctxmnuLaunchEve4, Me.ToolStripSeparator4, Me.ForceServerCheckToolStripMenuItem, Me.RestoreWindowToolStripMenuItem, Me.HideWhenMinimisedToolStripMenuItem, Me.ToolStripSeparator5, Me.ctxAbout, Me.ctxExit})
        Me.EveIconMenu.Name = "ContextMenuStrip1"
        Me.EveIconMenu.Size = New System.Drawing.Size(175, 214)
        '
        'ctxmnuLaunchEve1
        '
        Me.ctxmnuLaunchEve1.Name = "ctxmnuLaunchEve1"
        Me.ctxmnuLaunchEve1.Size = New System.Drawing.Size(174, 22)
        Me.ctxmnuLaunchEve1.Text = "Launch Eve (1)"
        '
        'ctxmnuLaunchEve2
        '
        Me.ctxmnuLaunchEve2.Name = "ctxmnuLaunchEve2"
        Me.ctxmnuLaunchEve2.Size = New System.Drawing.Size(174, 22)
        Me.ctxmnuLaunchEve2.Text = "Launch Eve (2)"
        '
        'ctxmnuLaunchEve3
        '
        Me.ctxmnuLaunchEve3.Name = "ctxmnuLaunchEve3"
        Me.ctxmnuLaunchEve3.Size = New System.Drawing.Size(174, 22)
        Me.ctxmnuLaunchEve3.Text = "Launch Eve (3)"
        '
        'ctxmnuLaunchEve4
        '
        Me.ctxmnuLaunchEve4.Name = "ctxmnuLaunchEve4"
        Me.ctxmnuLaunchEve4.Size = New System.Drawing.Size(174, 22)
        Me.ctxmnuLaunchEve4.Text = "Launch Eve (4)"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(171, 6)
        '
        'ForceServerCheckToolStripMenuItem
        '
        Me.ForceServerCheckToolStripMenuItem.Name = "ForceServerCheckToolStripMenuItem"
        Me.ForceServerCheckToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.ForceServerCheckToolStripMenuItem.Text = "Force Server Check"
        '
        'RestoreWindowToolStripMenuItem
        '
        Me.RestoreWindowToolStripMenuItem.Name = "RestoreWindowToolStripMenuItem"
        Me.RestoreWindowToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.RestoreWindowToolStripMenuItem.Text = "Restore Window"
        '
        'HideWhenMinimisedToolStripMenuItem
        '
        Me.HideWhenMinimisedToolStripMenuItem.Checked = True
        Me.HideWhenMinimisedToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.HideWhenMinimisedToolStripMenuItem.Name = "HideWhenMinimisedToolStripMenuItem"
        Me.HideWhenMinimisedToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.HideWhenMinimisedToolStripMenuItem.Text = "Hide When Minimised"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(171, 6)
        '
        'ctxAbout
        '
        Me.ctxAbout.Name = "ctxAbout"
        Me.ctxAbout.Size = New System.Drawing.Size(174, 22)
        Me.ctxAbout.Text = "About"
        '
        'ctxExit
        '
        Me.ctxExit.Name = "ctxExit"
        Me.ctxExit.Size = New System.Drawing.Size(174, 22)
        Me.ctxExit.Text = "Exit"
        '
        'tmrSkillUpdate
        '
        Me.tmrSkillUpdate.Interval = 1
        '
        'tmrSave
        '
        Me.tmrSave.Interval = 3600000
        '
        'tmrBackup
        '
        Me.tmrBackup.Enabled = True
        Me.tmrBackup.Interval = 60000
        '
        'tmrModules
        '
        Me.tmrModules.Interval = 10
        '
        'ctxTabbedMDI
        '
        Me.ctxTabbedMDI.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxTabbedMDI.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCloseMDITab})
        Me.ctxTabbedMDI.Name = "ctxTabbedMDI"
        Me.ctxTabbedMDI.Size = New System.Drawing.Size(122, 26)
        '
        'mnuCloseMDITab
        '
        Me.mnuCloseMDITab.Name = "mnuCloseMDITab"
        Me.mnuCloseMDITab.Size = New System.Drawing.Size(121, 22)
        Me.mnuCloseMDITab.Text = "Close Tab"
        '
        'iconEveHQMLW
        '
        Me.iconEveHQMLW.Icon = CType(resources.GetObject("iconEveHQMLW.Icon"), System.Drawing.Icon)
        '
        'tmrMemory
        '
        Me.tmrMemory.Interval = 300000
        '
        'RibbonControl1
        '
        '
        '
        '
        Me.RibbonControl1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.RibbonControl1.CaptionVisible = True
        Me.RibbonControl1.Controls.Add(Me.rpCore)
        Me.RibbonControl1.Controls.Add(Me.rpPlugins)
        Me.RibbonControl1.Controls.Add(Me.rpReports)
        Me.RibbonControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RibbonControl1.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.rtiCore, Me.rtiPlugins, Me.rtiReports, Me.btnTheme})
        Me.RibbonControl1.KeyTipsFont = New System.Drawing.Font("Tahoma", 7.0!)
        Me.RibbonControl1.Location = New System.Drawing.Point(5, 1)
        Me.RibbonControl1.Name = "RibbonControl1"
        Me.RibbonControl1.Padding = New System.Windows.Forms.Padding(0, 0, 0, 3)
        Me.RibbonControl1.QuickToolbarItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.Office2007StartButton1, Me.QatCustomizeItem1})
        Me.RibbonControl1.Size = New System.Drawing.Size(1231, 154)
        Me.RibbonControl1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.RibbonControl1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon"
        Me.RibbonControl1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon"
        Me.RibbonControl1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar"
        Me.RibbonControl1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>"
        Me.RibbonControl1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar..."
        Me.RibbonControl1.SystemText.QatDialogAddButton = "&Add >>"
        Me.RibbonControl1.SystemText.QatDialogCancelButton = "Cancel"
        Me.RibbonControl1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar"
        Me.RibbonControl1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:"
        Me.RibbonControl1.SystemText.QatDialogOkButton = "OK"
        Me.RibbonControl1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon"
        Me.RibbonControl1.SystemText.QatDialogRemoveButton = "&Remove"
        Me.RibbonControl1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon"
        Me.RibbonControl1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon"
        Me.RibbonControl1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar"
        Me.RibbonControl1.TabGroupHeight = 14
        Me.RibbonControl1.TabIndex = 22
        Me.RibbonControl1.Text = "RibbonControl1"
        '
        'rpCore
        '
        Me.rpCore.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rpCore.Controls.Add(Me.rbHelp)
        Me.rpCore.Controls.Add(Me.rbSQLTools)
        Me.rpCore.Controls.Add(Me.rbAPITools)
        Me.rpCore.Controls.Add(Me.rbBackup)
        Me.rpCore.Controls.Add(Me.rbIGB)
        Me.rpCore.Controls.Add(Me.rbEveMail)
        Me.rpCore.Controls.Add(Me.rbView)
        Me.rpCore.Controls.Add(Me.rbAPI)
        Me.rpCore.Controls.Add(Me.rbOptions)
        Me.rpCore.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rpCore.Location = New System.Drawing.Point(0, 56)
        Me.rpCore.Name = "rpCore"
        Me.rpCore.Padding = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.rpCore.Size = New System.Drawing.Size(1231, 95)
        '
        '
        '
        Me.rpCore.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rpCore.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rpCore.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rpCore.TabIndex = 1
        '
        'rbHelp
        '
        Me.rbHelp.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbHelp.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbHelp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbHelp.ContainerControlProcessDialogKey = True
        Me.rbHelp.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbHelp.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnInfoHelp})
        Me.rbHelp.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbHelp.Location = New System.Drawing.Point(1049, 0)
        Me.rbHelp.Name = "rbHelp"
        Me.rbHelp.Size = New System.Drawing.Size(73, 92)
        Me.rbHelp.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbHelp.TabIndex = 9
        Me.rbHelp.Text = "Info and Help"
        '
        '
        '
        Me.rbHelp.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbHelp.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnInfoHelp
        '
        Me.btnInfoHelp.Image = CType(resources.GetObject("btnInfoHelp.Image"), System.Drawing.Image)
        Me.btnInfoHelp.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnInfoHelp.Name = "btnInfoHelp"
        Me.btnInfoHelp.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnInfoHelp, New DevComponents.DotNetBar.SuperTooltipInfo("", "View EveHQ Info Centre", "Provides links and information relating to support on EveHQ." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.SkillBook32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnInfoHelp.Text = "EveHQ Info Centre"
        '
        'rbSQLTools
        '
        Me.rbSQLTools.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbSQLTools.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbSQLTools.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbSQLTools.ContainerControlProcessDialogKey = True
        Me.rbSQLTools.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbSQLTools.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ItemContainer3})
        Me.rbSQLTools.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbSQLTools.Location = New System.Drawing.Point(957, 0)
        Me.rbSQLTools.Name = "rbSQLTools"
        Me.rbSQLTools.Size = New System.Drawing.Size(92, 92)
        Me.rbSQLTools.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbSQLTools.TabIndex = 8
        Me.rbSQLTools.Text = "SQL Tools"
        '
        '
        '
        Me.rbSQLTools.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbSQLTools.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'ItemContainer3
        '
        '
        '
        '
        Me.ItemContainer3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemContainer3.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.ItemContainer3.Name = "ItemContainer3"
        Me.ItemContainer3.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnSQLQueryTool})
        '
        '
        '
        Me.ItemContainer3.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemContainer3.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Middle
        '
        'btnSQLQueryTool
        '
        Me.btnSQLQueryTool.Name = "btnSQLQueryTool"
        Me.SuperTooltip1.SetSuperTooltip(Me.btnSQLQueryTool, New DevComponents.DotNetBar.SuperTooltipInfo("", "SQL Query Tools", "Allows the static and custom databases to be queried using SQL statements.", Nothing, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnSQLQueryTool.Text = "SQL Query Tool"
        '
        'rbAPITools
        '
        Me.rbAPITools.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbAPITools.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAPITools.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbAPITools.ContainerControlProcessDialogKey = True
        Me.rbAPITools.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbAPITools.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ItemContainer2})
        Me.rbAPITools.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbAPITools.Location = New System.Drawing.Point(837, 0)
        Me.rbAPITools.Name = "rbAPITools"
        Me.rbAPITools.Size = New System.Drawing.Size(120, 92)
        Me.rbAPITools.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbAPITools.TabIndex = 6
        Me.rbAPITools.Text = "API Tools"
        '
        '
        '
        Me.rbAPITools.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAPITools.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'ItemContainer2
        '
        '
        '
        '
        Me.ItemContainer2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemContainer2.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.ItemContainer2.Name = "ItemContainer2"
        Me.ItemContainer2.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnAPIChecker, Me.btnOpenCacheFolder, Me.btnClearCache})
        '
        '
        '
        Me.ItemContainer2.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemContainer2.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Middle
        '
        'btnAPIChecker
        '
        Me.btnAPIChecker.Name = "btnAPIChecker"
        Me.btnAPIChecker.Text = "API Checker"
        Me.btnAPIChecker.Tooltip = "API Checker Tool"
        '
        'btnOpenCacheFolder
        '
        Me.btnOpenCacheFolder.Name = "btnOpenCacheFolder"
        Me.btnOpenCacheFolder.Text = "Open AppData Folder"
        '
        'btnClearCache
        '
        Me.btnClearCache.Name = "btnClearCache"
        Me.btnClearCache.SplitButton = True
        Me.btnClearCache.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnClearCharacterCache, Me.btnClearImageCache, Me.btnClearAllCache})
        Me.btnClearCache.Text = "Clear Cache"
        Me.btnClearCache.Tooltip = "Clear Various Aspects of the API Cache"
        '
        'btnClearCharacterCache
        '
        Me.btnClearCharacterCache.Name = "btnClearCharacterCache"
        Me.btnClearCharacterCache.Text = "Clear Character Cache"
        '
        'btnClearImageCache
        '
        Me.btnClearImageCache.Name = "btnClearImageCache"
        Me.btnClearImageCache.Text = "Clear Image Cache"
        '
        'btnClearAllCache
        '
        Me.btnClearAllCache.Name = "btnClearAllCache"
        Me.btnClearAllCache.Text = "Clear All Cached XML Data"
        '
        'rbBackup
        '
        Me.rbBackup.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbBackup.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbBackup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbBackup.ContainerControlProcessDialogKey = True
        Me.rbBackup.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbBackup.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ItemContainer5})
        Me.rbBackup.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbBackup.Location = New System.Drawing.Point(723, 0)
        Me.rbBackup.Name = "rbBackup"
        Me.rbBackup.Size = New System.Drawing.Size(114, 92)
        Me.rbBackup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbBackup.TabIndex = 5
        Me.rbBackup.Text = "Backup Tools"
        '
        '
        '
        Me.rbBackup.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbBackup.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'ItemContainer5
        '
        '
        '
        '
        Me.ItemContainer5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemContainer5.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.ItemContainer5.Name = "ItemContainer5"
        Me.ItemContainer5.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnBackupEveHQ, Me.btnBackupEve})
        '
        '
        '
        Me.ItemContainer5.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemContainer5.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Middle
        '
        'btnBackupEveHQ
        '
        Me.btnBackupEveHQ.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnBackupEveHQ.Image = Global.EveHQ.My.Resources.Resources.SecureCan32
        Me.btnBackupEveHQ.ImageFixedSize = New System.Drawing.Size(24, 24)
        Me.btnBackupEveHQ.Name = "btnBackupEveHQ"
        Me.btnBackupEveHQ.SubItemsExpandWidth = 14
        Me.btnBackupEveHQ.Text = "Backup EveHQ"
        Me.btnBackupEveHQ.Tooltip = "Backup EveHQ Settings"
        '
        'btnBackupEve
        '
        Me.btnBackupEve.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnBackupEve.Image = Global.EveHQ.My.Resources.Resources.Database32
        Me.btnBackupEve.ImageFixedSize = New System.Drawing.Size(24, 24)
        Me.btnBackupEve.Name = "btnBackupEve"
        Me.btnBackupEve.Text = "Backup Eve"
        Me.btnBackupEve.Tooltip = "Backup Eve Settings"
        '
        'rbIGB
        '
        Me.rbIGB.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbIGB.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbIGB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbIGB.ContainerControlProcessDialogKey = True
        Me.rbIGB.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbIGB.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnIGB, Me.lblIGB})
        Me.rbIGB.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbIGB.Location = New System.Drawing.Point(590, 0)
        Me.rbIGB.Name = "rbIGB"
        Me.rbIGB.Size = New System.Drawing.Size(133, 92)
        Me.rbIGB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbIGB.TabIndex = 4
        Me.rbIGB.Text = "IGB Server"
        '
        '
        '
        Me.rbIGB.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbIGB.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnIGB
        '
        Me.btnIGB.AutoCheckOnClick = True
        Me.btnIGB.Image = Global.EveHQ.My.Resources.Resources.IGB32
        Me.btnIGB.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnIGB.Name = "btnIGB"
        Me.btnIGB.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnIGB, New DevComponents.DotNetBar.SuperTooltipInfo("", "Toggle EveHQ IGB Server", "Toggles the IGB Server between active/inactive states.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.IGB32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnIGB.Text = "Toggle IGB Server"
        '
        'lblIGB
        '
        Me.lblIGB.CanCustomize = False
        Me.lblIGB.Name = "lblIGB"
        Me.SuperTooltip1.SetSuperTooltip(Me.lblIGB, New DevComponents.DotNetBar.SuperTooltipInfo("", "EveHQ IGB Server Status", "Provides the status of the EveHQ IGB Server including the port number to which th" & _
            "e server is assigned.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.IGB32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.lblIGB.Text = "Port: 26001" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Status: Off"
        Me.lblIGB.TextAlignment = System.Drawing.StringAlignment.Center
        '
        'rbEveMail
        '
        Me.rbEveMail.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbEveMail.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbEveMail.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbEveMail.ContainerControlProcessDialogKey = True
        Me.rbEveMail.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbEveMail.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnEveMail, Me.lblEveMail})
        Me.rbEveMail.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbEveMail.Location = New System.Drawing.Point(460, 0)
        Me.rbEveMail.Name = "rbEveMail"
        Me.rbEveMail.Size = New System.Drawing.Size(130, 92)
        Me.rbEveMail.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbEveMail.TabIndex = 3
        Me.rbEveMail.Text = "EveMail"
        '
        '
        '
        Me.rbEveMail.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbEveMail.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnEveMail
        '
        Me.btnEveMail.Image = Global.EveHQ.My.Resources.Resources.EveMail32
        Me.btnEveMail.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnEveMail.Name = "btnEveMail"
        Me.btnEveMail.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnEveMail, New DevComponents.DotNetBar.SuperTooltipInfo("", "View EveMail and Eve Notifications", "Displays EveMail and Eve Notifications based on data provided by the API.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.EveMail32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnEveMail.Text = "View EveMail"
        '
        'lblEveMail
        '
        Me.lblEveMail.CanCustomize = False
        Me.lblEveMail.Name = "lblEveMail"
        Me.lblEveMail.PaddingBottom = 3
        Me.lblEveMail.PaddingLeft = 5
        Me.lblEveMail.PaddingRight = 3
        Me.lblEveMail.PaddingTop = 3
        Me.lblEveMail.Stretch = True
        Me.SuperTooltip1.SetSuperTooltip(Me.lblEveMail, New DevComponents.DotNetBar.SuperTooltipInfo("", "Unread Mail and Notifications", "This is the number of unread EveMail and Eve Notifications that have been downloa" & _
            "ded via the API." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This only refers to what has not been read in EveHQ, not in-" & _
            "game.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.EveMail32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.lblEveMail.Text = "EveMail: 0" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Notices: 0"
        Me.lblEveMail.Width = 75
        '
        'rbView
        '
        Me.rbView.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbView.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbView.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbView.ContainerControlProcessDialogKey = True
        Me.rbView.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbView.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnViewPilotInfo, Me.btnViewSkillTraining, Me.btnViewPrices, Me.btnViewDashboard, Me.btnViewReqs})
        Me.rbView.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbView.Location = New System.Drawing.Point(191, 0)
        Me.rbView.Name = "rbView"
        Me.rbView.Size = New System.Drawing.Size(269, 92)
        Me.rbView.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbView.TabIndex = 2
        Me.rbView.Text = "View"
        '
        '
        '
        Me.rbView.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbView.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnViewPilotInfo
        '
        Me.btnViewPilotInfo.Image = Global.EveHQ.My.Resources.Resources.Aura32
        Me.btnViewPilotInfo.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnViewPilotInfo.Name = "btnViewPilotInfo"
        Me.btnViewPilotInfo.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnViewPilotInfo, New DevComponents.DotNetBar.SuperTooltipInfo("", "View Pilot Information", "Opens the pilot information screen which provides basic details, as well as skill" & _
            "s, certificates and standings.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Aura32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnViewPilotInfo.Text = "Pilot Info"
        '
        'btnViewSkillTraining
        '
        Me.btnViewSkillTraining.Image = Global.EveHQ.My.Resources.Resources.SkillBook32
        Me.btnViewSkillTraining.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnViewSkillTraining.Name = "btnViewSkillTraining"
        Me.btnViewSkillTraining.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnViewSkillTraining, New DevComponents.DotNetBar.SuperTooltipInfo("", "View Skill Planner", "Opens the skill planning feature which allows you to plan the future training for" & _
            " each of the pilots.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.SkillBook32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnViewSkillTraining.Text = "Skill Planner"
        '
        'btnViewPrices
        '
        Me.btnViewPrices.Image = Global.EveHQ.My.Resources.Resources.Market32
        Me.btnViewPrices.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnViewPrices.Name = "btnViewPrices"
        Me.btnViewPrices.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnViewPrices, New DevComponents.DotNetBar.SuperTooltipInfo("", "View Prices System", "The prices system allows you to download market and faction prices from third par" & _
            "ty sites, as well as providing your own custom prices.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Market32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnViewPrices.Text = "Market Prices"
        '
        'btnViewDashboard
        '
        Me.btnViewDashboard.Image = Global.EveHQ.My.Resources.Resources.Dashboard32
        Me.btnViewDashboard.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnViewDashboard.Name = "btnViewDashboard"
        Me.btnViewDashboard.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnViewDashboard, New DevComponents.DotNetBar.SuperTooltipInfo("", "View EveHQ Dashboard", "The EveHQ Dashboard provides users with a collection of widgets which display rel" & _
            "evant data about pilots skills and training, along with some additional useful i" & _
            "nformation.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Dashboard32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnViewDashboard.Text = "EveHQ Dashboard"
        '
        'btnViewReqs
        '
        Me.btnViewReqs.Image = Global.EveHQ.My.Resources.Resources.Orders32
        Me.btnViewReqs.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnViewReqs.Name = "btnViewReqs"
        Me.btnViewReqs.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnViewReqs, New DevComponents.DotNetBar.SuperTooltipInfo("", "View EveHQ Requisitions", "Allows the creation, editing and viewing of requisitions (also known as Shopping " & _
            "Lists)." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Requisitions created in other parts of EveHQ will be visible here.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Orders32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnViewReqs.Text = "EveHQ Requisitions"
        '
        'rbAPI
        '
        Me.rbAPI.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbAPI.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAPI.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbAPI.ContainerControlProcessDialogKey = True
        Me.rbAPI.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbAPI.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnManageAPI, Me.btnQueryAPI})
        Me.rbAPI.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbAPI.Location = New System.Drawing.Point(97, 0)
        Me.rbAPI.Name = "rbAPI"
        Me.rbAPI.Size = New System.Drawing.Size(94, 92)
        Me.rbAPI.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbAPI.TabIndex = 1
        Me.rbAPI.Text = "API Functions"
        '
        '
        '
        Me.rbAPI.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbAPI.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnManageAPI
        '
        Me.btnManageAPI.Image = Global.EveHQ.My.Resources.Resources.ManageAPI32
        Me.btnManageAPI.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnManageAPI.Name = "btnManageAPI"
        Me.btnManageAPI.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnManageAPI, New DevComponents.DotNetBar.SuperTooltipInfo("", "Manage API", "Opens the EveHQ Settings so that API Accounts can be added, edited or removed." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.ManageAPI32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnManageAPI.Text = "Manage API"
        '
        'btnQueryAPI
        '
        Me.btnQueryAPI.Image = Global.EveHQ.My.Resources.Resources.QueryAPI32
        Me.btnQueryAPI.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnQueryAPI.Name = "btnQueryAPI"
        Me.btnQueryAPI.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnQueryAPI, New DevComponents.DotNetBar.SuperTooltipInfo("", "Update Character API", "Updates character associated APIs, including character sheet, training, standings" & _
            " and certificates." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If the full API has been entered, the account status and d" & _
            "etails will also be retrieved.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.QueryAPI32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnQueryAPI.Text = "Update API"
        '
        'rbOptions
        '
        Me.rbOptions.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbOptions.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbOptions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbOptions.ContainerControlProcessDialogKey = True
        Me.rbOptions.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbOptions.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnSave})
        Me.rbOptions.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbOptions.Location = New System.Drawing.Point(3, 0)
        Me.rbOptions.Name = "rbOptions"
        Me.rbOptions.Size = New System.Drawing.Size(94, 92)
        Me.rbOptions.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbOptions.TabIndex = 0
        Me.rbOptions.Text = "Options"
        '
        '
        '
        Me.rbOptions.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbOptions.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnSave
        '
        Me.btnSave.Image = Global.EveHQ.My.Resources.Resources.AssetsSafe32
        Me.btnSave.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnSave.Name = "btnSave"
        Me.btnSave.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnSave, New DevComponents.DotNetBar.SuperTooltipInfo("", "Save Data", "Saves settings and data of EveHQ Core and open plug-ins." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.AssetsSafe32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnSave.Text = "Save Data"
        '
        'rpReports
        '
        Me.rpReports.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rpReports.Controls.Add(Me.rbCharts)
        Me.rpReports.Controls.Add(Me.rbPHPBB)
        Me.rpReports.Controls.Add(Me.rbXML)
        Me.rpReports.Controls.Add(Me.rbText)
        Me.rpReports.Controls.Add(Me.rbHTML)
        Me.rpReports.Controls.Add(Me.rbStandard)
        Me.rpReports.Controls.Add(Me.rbReportOptions)
        Me.rpReports.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rpReports.Location = New System.Drawing.Point(0, 56)
        Me.rpReports.Name = "rpReports"
        Me.rpReports.Padding = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.rpReports.Size = New System.Drawing.Size(1231, 96)
        '
        '
        '
        Me.rpReports.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rpReports.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rpReports.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rpReports.TabIndex = 3
        Me.rpReports.Visible = False
        '
        'rbCharts
        '
        Me.rbCharts.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbCharts.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbCharts.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbCharts.ContainerControlProcessDialogKey = True
        Me.rbCharts.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbCharts.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnChartSkillGroup, Me.btnChartSkillCost})
        Me.rbCharts.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbCharts.Location = New System.Drawing.Point(2043, 0)
        Me.rbCharts.Name = "rbCharts"
        Me.rbCharts.Size = New System.Drawing.Size(123, 93)
        Me.rbCharts.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbCharts.TabIndex = 5
        Me.rbCharts.Text = "Charts && Graphs"
        '
        '
        '
        Me.rbCharts.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbCharts.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbCharts.Visible = False
        '
        'btnChartSkillGroup
        '
        Me.btnChartSkillGroup.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnChartSkillGroup.Image = Global.EveHQ.My.Resources.Resources.Chart32
        Me.btnChartSkillGroup.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnChartSkillGroup.Name = "btnChartSkillGroup"
        Me.btnChartSkillGroup.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnChartSkillGroup, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skill Group Chart", "Produces a pie-chart showing the split of skills trained by the various skill gro" & _
            "ups.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Chart32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnChartSkillGroup.Text = "Skill Group Chart"
        '
        'btnChartSkillCost
        '
        Me.btnChartSkillCost.Image = Global.EveHQ.My.Resources.Resources.Chart32
        Me.btnChartSkillCost.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnChartSkillCost.Name = "btnChartSkillCost"
        Me.btnChartSkillCost.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnChartSkillCost, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skill Cost Chart", "Produces a chart showing the costs of each of the various skill groups.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Chart32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnChartSkillCost.Text = "Skill Cost Chart"
        '
        'rbPHPBB
        '
        Me.rbPHPBB.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbPHPBB.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbPHPBB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbPHPBB.ContainerControlProcessDialogKey = True
        Me.rbPHPBB.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbPHPBB.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnPHPBBCharacterSheet})
        Me.rbPHPBB.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbPHPBB.Location = New System.Drawing.Point(1979, 0)
        Me.rbPHPBB.Name = "rbPHPBB"
        Me.rbPHPBB.Size = New System.Drawing.Size(64, 93)
        Me.rbPHPBB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbPHPBB.TabIndex = 4
        Me.rbPHPBB.Text = "PHPBB Reports"
        '
        '
        '
        Me.rbPHPBB.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbPHPBB.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbPHPBB.Visible = False
        '
        'btnPHPBBCharacterSheet
        '
        Me.btnPHPBBCharacterSheet.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnPHPBBCharacterSheet.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnPHPBBCharacterSheet.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnPHPBBCharacterSheet.Name = "btnPHPBBCharacterSheet"
        Me.btnPHPBBCharacterSheet.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnPHPBBCharacterSheet, New DevComponents.DotNetBar.SuperTooltipInfo("", "Character Sheet Report (PHPBB)", "Produces a Character Sheet based text file that can be copied and pasted into a P" & _
            "HPBB forum.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnPHPBBCharacterSheet.Text = "Character Sheet"
        '
        'rbXML
        '
        Me.rbXML.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbXML.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbXML.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbXML.ContainerControlProcessDialogKey = True
        Me.rbXML.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbXML.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnXMLCharacterXML, Me.btnXMLTrainingXML, Me.btnXMLCurrentCharOld, Me.btnXMLCurrentCharNew, Me.btnXMLCurrentTrainingOld, Me.btnXMLECMExport})
        Me.rbXML.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbXML.Location = New System.Drawing.Point(1643, 0)
        Me.rbXML.Name = "rbXML"
        Me.rbXML.Size = New System.Drawing.Size(336, 93)
        Me.rbXML.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbXML.TabIndex = 3
        Me.rbXML.Text = "XML Reports"
        '
        '
        '
        Me.rbXML.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbXML.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbXML.Visible = False
        '
        'btnXMLCharacterXML
        '
        Me.btnXMLCharacterXML.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnXMLCharacterXML.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnXMLCharacterXML.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnXMLCharacterXML.Name = "btnXMLCharacterXML"
        Me.btnXMLCharacterXML.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnXMLCharacterXML, New DevComponents.DotNetBar.SuperTooltipInfo("", "Character XML Report (XML)", "Displays an XML report that is a copy of the pilot's CharacterSheet API.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnXMLCharacterXML.Text = "Character XML"
        '
        'btnXMLTrainingXML
        '
        Me.btnXMLTrainingXML.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnXMLTrainingXML.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnXMLTrainingXML.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnXMLTrainingXML.Name = "btnXMLTrainingXML"
        Me.btnXMLTrainingXML.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnXMLTrainingXML, New DevComponents.DotNetBar.SuperTooltipInfo("", "Training XML Report (XML)", "Displays an XML report that is a copy of the pilot's SkillQueue API.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnXMLTrainingXML.Text = "Training XML"
        '
        'btnXMLCurrentCharOld
        '
        Me.btnXMLCurrentCharOld.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnXMLCurrentCharOld.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnXMLCurrentCharOld.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnXMLCurrentCharOld.Name = "btnXMLCurrentCharOld"
        Me.btnXMLCurrentCharOld.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnXMLCurrentCharOld, New DevComponents.DotNetBar.SuperTooltipInfo("", "Character (Old Style) Report (XML)", "Produces a copy of the old style XML character sheet report prior to the introduc" & _
            "tion of the API." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "The report is updated to include the total skill points for " & _
            "the current skill in training.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnXMLCurrentCharOld.Text = "Character (Old Style)"
        '
        'btnXMLCurrentCharNew
        '
        Me.btnXMLCurrentCharNew.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnXMLCurrentCharNew.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnXMLCurrentCharNew.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnXMLCurrentCharNew.Name = "btnXMLCurrentCharNew"
        Me.btnXMLCurrentCharNew.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnXMLCurrentCharNew, New DevComponents.DotNetBar.SuperTooltipInfo("", "Character (New Style) Report (XML)", "Produces a copy of the Character XML report but is updated to include the total s" & _
            "kill points for the current skill in training." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnXMLCurrentCharNew.Text = "Character (New Style)"
        '
        'btnXMLCurrentTrainingOld
        '
        Me.btnXMLCurrentTrainingOld.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnXMLCurrentTrainingOld.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnXMLCurrentTrainingOld.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnXMLCurrentTrainingOld.Name = "btnXMLCurrentTrainingOld"
        Me.btnXMLCurrentTrainingOld.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnXMLCurrentTrainingOld, New DevComponents.DotNetBar.SuperTooltipInfo("", "Training (Old Style) Report (XML)", "Produces a copy of the old style XML training report prior to the introduction of" & _
            " the API." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnXMLCurrentTrainingOld.Text = "Training (Old Style)"
        '
        'btnXMLECMExport
        '
        Me.btnXMLECMExport.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnXMLECMExport.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnXMLECMExport.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnXMLECMExport.Name = "btnXMLECMExport"
        Me.btnXMLECMExport.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnXMLECMExport, New DevComponents.DotNetBar.SuperTooltipInfo("", "ECM Export (XML)", "Exports data in a format support by the now obsolete Eve Character Manager.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnXMLECMExport.Text = "ECM Export"
        '
        'rbText
        '
        Me.rbText.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbText.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbText.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbText.ContainerControlProcessDialogKey = True
        Me.rbText.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbText.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnTextCharacterSheet, Me.btnTextTrainingTimes, Me.btnTextTimeToLvl5, Me.btnTextSkillLevels, Me.btnTextTrainingQueue, Me.btnTextQueueShoppingList, Me.btnTextSkillsAvailable, Me.btnTextSkillsNotTrained, Me.btnTextPartiallyTrained, Me.btnTextSkillsCost})
        Me.rbText.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbText.Location = New System.Drawing.Point(1070, 0)
        Me.rbText.Name = "rbText"
        Me.rbText.Size = New System.Drawing.Size(573, 93)
        Me.rbText.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbText.TabIndex = 2
        Me.rbText.Text = "Text Reports"
        '
        '
        '
        Me.rbText.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbText.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbText.Visible = False
        '
        'btnTextCharacterSheet
        '
        Me.btnTextCharacterSheet.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextCharacterSheet.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextCharacterSheet.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextCharacterSheet.Name = "btnTextCharacterSheet"
        Me.btnTextCharacterSheet.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextCharacterSheet, New DevComponents.DotNetBar.SuperTooltipInfo("", "Character Sheet Report (Text)", "Produces a standard character sheet showing basic details, attributes and skills " & _
            "for the selected character.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextCharacterSheet.Text = "Character Sheet"
        '
        'btnTextTrainingTimes
        '
        Me.btnTextTrainingTimes.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextTrainingTimes.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextTrainingTimes.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextTrainingTimes.Name = "btnTextTrainingTimes"
        Me.btnTextTrainingTimes.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextTrainingTimes, New DevComponents.DotNetBar.SuperTooltipInfo("", "Training Times Report (Text)", "Displays the time required to train each of a pilot's skills to the next level.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextTrainingTimes.Text = "Training Times"
        '
        'btnTextTimeToLvl5
        '
        Me.btnTextTimeToLvl5.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextTimeToLvl5.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextTimeToLvl5.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextTimeToLvl5.Name = "btnTextTimeToLvl5"
        Me.btnTextTimeToLvl5.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextTimeToLvl5, New DevComponents.DotNetBar.SuperTooltipInfo("", "Time to Level 5 Report (Text)", "Displays a report showing the time taken for each skill to be trained to the maxi" & _
            "mum level 5.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextTimeToLvl5.Text = "TimeTo Level 5"
        '
        'btnTextSkillLevels
        '
        Me.btnTextSkillLevels.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextSkillLevels.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextSkillLevels.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextSkillLevels.Name = "btnTextSkillLevels"
        Me.btnTextSkillLevels.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextSkillLevels, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skill Levels Report (Text)", "Displays a report grouping skills according to the level currently trained.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextSkillLevels.Text = "Skill Levels"
        '
        'btnTextTrainingQueue
        '
        Me.btnTextTrainingQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextTrainingQueue.CanCustomize = False
        Me.btnTextTrainingQueue.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextTrainingQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextTrainingQueue.Name = "btnTextTrainingQueue"
        Me.btnTextTrainingQueue.SplitButton = True
        Me.btnTextTrainingQueue.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextTrainingQueue, New DevComponents.DotNetBar.SuperTooltipInfo("", "Training Queue Report (Text)", "Displays the contents of a specific training queue for the selected pilot." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Thi" & _
            "s report cannot be added to the QAT.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextTrainingQueue.Text = "Training Queue"
        '
        'btnTextQueueShoppingList
        '
        Me.btnTextQueueShoppingList.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextQueueShoppingList.CanCustomize = False
        Me.btnTextQueueShoppingList.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextQueueShoppingList.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextQueueShoppingList.Name = "btnTextQueueShoppingList"
        Me.btnTextQueueShoppingList.SplitButton = True
        Me.btnTextQueueShoppingList.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextQueueShoppingList, New DevComponents.DotNetBar.SuperTooltipInfo("", "Queue Shopping List Report (Text)", "Displays the skills required to be purchased for a selected training queue." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Th" & _
            "is report cannot be added to the QAT.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextQueueShoppingList.Text = "Queue Shopping List"
        '
        'btnTextSkillsAvailable
        '
        Me.btnTextSkillsAvailable.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextSkillsAvailable.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextSkillsAvailable.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextSkillsAvailable.Name = "btnTextSkillsAvailable"
        Me.btnTextSkillsAvailable.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextSkillsAvailable, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skills Available To Train Report (Text)", "Lists the skills that the pilot does not own, but where all the pre-requisites fo" & _
            "r those skills have already been met.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextSkillsAvailable.Text = "Skills Available To Train"
        '
        'btnTextSkillsNotTrained
        '
        Me.btnTextSkillsNotTrained.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextSkillsNotTrained.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextSkillsNotTrained.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextSkillsNotTrained.Name = "btnTextSkillsNotTrained"
        Me.btnTextSkillsNotTrained.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextSkillsNotTrained, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skills Not Trained Report (Text)", "Displays a list of all skills that the pilot has not yet trained, whether the pre" & _
            "-requisite skills have been met or not.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextSkillsNotTrained.Text = "Skills Not Trained"
        '
        'btnTextPartiallyTrained
        '
        Me.btnTextPartiallyTrained.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextPartiallyTrained.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextPartiallyTrained.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextPartiallyTrained.Name = "btnTextPartiallyTrained"
        Me.btnTextPartiallyTrained.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextPartiallyTrained, New DevComponents.DotNetBar.SuperTooltipInfo("", "Partially Trained Skills Report (Text)", "Lists those skills that have been partially trained between levels.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextPartiallyTrained.Text = "Partially Trained Skills"
        '
        'btnTextSkillsCost
        '
        Me.btnTextSkillsCost.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnTextSkillsCost.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnTextSkillsCost.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnTextSkillsCost.Name = "btnTextSkillsCost"
        Me.btnTextSkillsCost.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnTextSkillsCost, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skills Cost Report (Text)", "Displays a report showing the cost of acquiring all the skill books currently tra" & _
            "ined by the pilot." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "The values are taken from ""base"" cost.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnTextSkillsCost.Text = "Skills Cost"
        '
        'rbHTML
        '
        Me.rbHTML.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbHTML.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbHTML.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbHTML.ContainerControlProcessDialogKey = True
        Me.rbHTML.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbHTML.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnHTMLCharSheet, Me.btnHTMLTrainingTimes, Me.btnHTMLTimeToLvl5, Me.btnHTMLSkillLevels, Me.btnHTMLTrainingQueue, Me.btnHTMLQueueShoppingList, Me.btnHTMLSkillsAvailable, Me.btnHTMLSkillsNotTrained, Me.btnHTMLPartiallyTrained, Me.btnHTMLSkillsCost})
        Me.rbHTML.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbHTML.Location = New System.Drawing.Point(497, 0)
        Me.rbHTML.Name = "rbHTML"
        Me.rbHTML.Size = New System.Drawing.Size(573, 93)
        Me.rbHTML.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbHTML.TabIndex = 1
        Me.rbHTML.Text = "HTML Reports"
        '
        '
        '
        Me.rbHTML.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbHTML.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbHTML.Visible = False
        '
        'btnHTMLCharSheet
        '
        Me.btnHTMLCharSheet.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLCharSheet.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLCharSheet.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLCharSheet.Name = "btnHTMLCharSheet"
        Me.btnHTMLCharSheet.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLCharSheet, New DevComponents.DotNetBar.SuperTooltipInfo("", "Character Sheet Report (HTML)", "Produces a standard character sheet showing basic details, attributes and skills " & _
            "for the selected character.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLCharSheet.Text = "Character Sheet"
        '
        'btnHTMLTrainingTimes
        '
        Me.btnHTMLTrainingTimes.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLTrainingTimes.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLTrainingTimes.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLTrainingTimes.Name = "btnHTMLTrainingTimes"
        Me.btnHTMLTrainingTimes.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLTrainingTimes, New DevComponents.DotNetBar.SuperTooltipInfo("", "Training Times Report (HTML)", "Displays the time required to train each of a pilot's skills to the next level.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLTrainingTimes.Text = "Training Times"
        '
        'btnHTMLTimeToLvl5
        '
        Me.btnHTMLTimeToLvl5.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLTimeToLvl5.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLTimeToLvl5.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLTimeToLvl5.Name = "btnHTMLTimeToLvl5"
        Me.btnHTMLTimeToLvl5.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLTimeToLvl5, New DevComponents.DotNetBar.SuperTooltipInfo("", "Time to Level 5 Report (HTML)", "Displays a report showing the time taken for each skill to be trained to the maxi" & _
            "mum level 5.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLTimeToLvl5.Text = "TimeTo Level 5"
        '
        'btnHTMLSkillLevels
        '
        Me.btnHTMLSkillLevels.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLSkillLevels.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLSkillLevels.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLSkillLevels.Name = "btnHTMLSkillLevels"
        Me.btnHTMLSkillLevels.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLSkillLevels, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skill Levels Report (HTML)", "Displays a report grouping skills according to the level currently trained.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLSkillLevels.Text = "Skill Levels"
        '
        'btnHTMLTrainingQueue
        '
        Me.btnHTMLTrainingQueue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLTrainingQueue.CanCustomize = False
        Me.btnHTMLTrainingQueue.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLTrainingQueue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLTrainingQueue.Name = "btnHTMLTrainingQueue"
        Me.btnHTMLTrainingQueue.SplitButton = True
        Me.btnHTMLTrainingQueue.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLTrainingQueue, New DevComponents.DotNetBar.SuperTooltipInfo("", "Training Queue Report (HTML)", "Displays the contents of a specific training queue for the selected pilot." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Thi" & _
            "s report cannot be added to the QAT.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLTrainingQueue.Text = "Training Queue"
        '
        'btnHTMLQueueShoppingList
        '
        Me.btnHTMLQueueShoppingList.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLQueueShoppingList.CanCustomize = False
        Me.btnHTMLQueueShoppingList.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLQueueShoppingList.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLQueueShoppingList.Name = "btnHTMLQueueShoppingList"
        Me.btnHTMLQueueShoppingList.SplitButton = True
        Me.btnHTMLQueueShoppingList.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLQueueShoppingList, New DevComponents.DotNetBar.SuperTooltipInfo("", "Queue Shopping List Report (HTML)", "Displays the skills required to be purchased for a selected training queue." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Th" & _
            "is report cannot be added to the QAT.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLQueueShoppingList.Text = "Queue Shopping List"
        '
        'btnHTMLSkillsAvailable
        '
        Me.btnHTMLSkillsAvailable.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLSkillsAvailable.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLSkillsAvailable.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLSkillsAvailable.Name = "btnHTMLSkillsAvailable"
        Me.btnHTMLSkillsAvailable.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLSkillsAvailable, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skills Available to Train Report (HTML)", "Lists the skills that the pilot does not own, but where all the pre-requisites fo" & _
            "r those skills have already been met.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLSkillsAvailable.Text = "Skills Available To Train"
        '
        'btnHTMLSkillsNotTrained
        '
        Me.btnHTMLSkillsNotTrained.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLSkillsNotTrained.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLSkillsNotTrained.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLSkillsNotTrained.Name = "btnHTMLSkillsNotTrained"
        Me.btnHTMLSkillsNotTrained.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLSkillsNotTrained, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skills Not Trained Report (HTML)", "Displays a list of all skills that the pilot has not yet trained, whether the pre" & _
            "-requisite skills have been met or not.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLSkillsNotTrained.Text = "Skills Not Trained"
        '
        'btnHTMLPartiallyTrained
        '
        Me.btnHTMLPartiallyTrained.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLPartiallyTrained.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLPartiallyTrained.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLPartiallyTrained.Name = "btnHTMLPartiallyTrained"
        Me.btnHTMLPartiallyTrained.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLPartiallyTrained, New DevComponents.DotNetBar.SuperTooltipInfo("", "Partially Trained Skills Report (HTML)", "Lists those skills that have been partially trained between levels.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLPartiallyTrained.Text = "Partially Trained Skills"
        '
        'btnHTMLSkillsCost
        '
        Me.btnHTMLSkillsCost.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHTMLSkillsCost.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnHTMLSkillsCost.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnHTMLSkillsCost.Name = "btnHTMLSkillsCost"
        Me.btnHTMLSkillsCost.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnHTMLSkillsCost, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skills Cost Report (HTML)", "Displays a report showing the cost of acquiring all the skill books currently tra" & _
            "ined by the pilot." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "The values are taken from ""base"" cost.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnHTMLSkillsCost.Text = "Skills Cost"
        '
        'rbStandard
        '
        Me.rbStandard.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbStandard.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbStandard.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbStandard.ContainerControlProcessDialogKey = True
        Me.rbStandard.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbStandard.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnStdCharSummary, Me.btnStdSkillLevels, Me.btnStdAlloyReport, Me.btnStdAsteroidReport, Me.btnStdIceReport})
        Me.rbStandard.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbStandard.Location = New System.Drawing.Point(169, 0)
        Me.rbStandard.Name = "rbStandard"
        Me.rbStandard.Size = New System.Drawing.Size(328, 93)
        Me.rbStandard.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbStandard.TabIndex = 7
        Me.rbStandard.Text = "Standard Reports"
        '
        '
        '
        Me.rbStandard.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbStandard.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnStdCharSummary
        '
        Me.btnStdCharSummary.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnStdCharSummary.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnStdCharSummary.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnStdCharSummary.Name = "btnStdCharSummary"
        Me.btnStdCharSummary.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnStdCharSummary, New DevComponents.DotNetBar.SuperTooltipInfo("", "Character Summary Report (HTML)", "Displays a summary of all active pilots in EveHQ, giving skillpoints, Isk balance" & _
            " and current training skill.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnStdCharSummary.Text = "Character Summary"
        '
        'btnStdSkillLevels
        '
        Me.btnStdSkillLevels.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnStdSkillLevels.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnStdSkillLevels.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnStdSkillLevels.Name = "btnStdSkillLevels"
        Me.btnStdSkillLevels.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnStdSkillLevels, New DevComponents.DotNetBar.SuperTooltipInfo("", "Skill Level Table (HTML)", "Displays a table showing the skillpoints required to train certain levels of skil" & _
            "ls at varying ranks.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnStdSkillLevels.Text = "Skill Level Table"
        '
        'btnStdAlloyReport
        '
        Me.btnStdAlloyReport.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnStdAlloyReport.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnStdAlloyReport.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnStdAlloyReport.Name = "btnStdAlloyReport"
        Me.btnStdAlloyReport.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnStdAlloyReport, New DevComponents.DotNetBar.SuperTooltipInfo("", "Alloy Composition Report (HTML)", "Shows the mineral breakdown of drone alloys." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnStdAlloyReport.Text = "Alloy Composition"
        '
        'btnStdAsteroidReport
        '
        Me.btnStdAsteroidReport.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnStdAsteroidReport.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnStdAsteroidReport.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnStdAsteroidReport.Name = "btnStdAsteroidReport"
        Me.btnStdAsteroidReport.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnStdAsteroidReport, New DevComponents.DotNetBar.SuperTooltipInfo("", "Asteroid Composition Report (HTML)", "Shows the mineral composition of various asteroids. Also includes details of comp" & _
            "ressed ores.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnStdAsteroidReport.Text = "Asteroid Composition"
        '
        'btnStdIceReport
        '
        Me.btnStdIceReport.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnStdIceReport.Image = Global.EveHQ.My.Resources.Resources.Document32
        Me.btnStdIceReport.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnStdIceReport.Name = "btnStdIceReport"
        Me.btnStdIceReport.SubItemsExpandWidth = 14
        Me.SuperTooltip1.SetSuperTooltip(Me.btnStdIceReport, New DevComponents.DotNetBar.SuperTooltipInfo("", "Ice Composition Report (HTML)", "Shows the breakdown of each of the various Ice blocks. Also contains details of c" & _
            "ompressed Ice.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnStdIceReport.Text = "Ice Composition"
        '
        'rbReportOptions
        '
        Me.rbReportOptions.AutoOverflowEnabled = True
        '
        '
        '
        Me.rbReportOptions.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbReportOptions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.rbReportOptions.ContainerControlProcessDialogKey = True
        Me.rbReportOptions.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbReportOptions.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.icReportOptions})
        Me.rbReportOptions.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.rbReportOptions.Location = New System.Drawing.Point(3, 0)
        Me.rbReportOptions.Name = "rbReportOptions"
        Me.rbReportOptions.Size = New System.Drawing.Size(166, 93)
        Me.rbReportOptions.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.rbReportOptions.TabIndex = 0
        Me.rbReportOptions.Text = "Report Options"
        '
        '
        '
        Me.rbReportOptions.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.rbReportOptions.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'icReportOptions
        '
        '
        '
        '
        Me.icReportOptions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.icReportOptions.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.icReportOptions.Name = "icReportOptions"
        Me.icReportOptions.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.icReportPilot, Me.icReportFormat, Me.btnOpenReportFolder})
        '
        '
        '
        Me.icReportOptions.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'icReportPilot
        '
        '
        '
        '
        Me.icReportPilot.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.icReportPilot.Name = "icReportPilot"
        Me.icReportPilot.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.lblReportPilot, Me.cboReportPilot})
        '
        '
        '
        Me.icReportPilot.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'lblReportPilot
        '
        Me.lblReportPilot.CanCustomize = False
        Me.lblReportPilot.Name = "lblReportPilot"
        Me.lblReportPilot.Text = "Pilot:"
        '
        'cboReportPilot
        '
        Me.cboReportPilot.ComboWidth = 125
        Me.cboReportPilot.DropDownHeight = 106
        Me.cboReportPilot.Name = "cboReportPilot"
        Me.cboReportPilot.Stretch = True
        Me.SuperTooltip1.SetSuperTooltip(Me.cboReportPilot, New DevComponents.DotNetBar.SuperTooltipInfo("", "Pilot Selection", "Chooses the pilot who will be the focus of the selected report.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Aura32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.cboReportPilot.Text = "Report Pilot"
        Me.cboReportPilot.WatermarkFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReportPilot.WatermarkText = "Report pilot..."
        '
        'icReportFormat
        '
        '
        '
        '
        Me.icReportFormat.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.icReportFormat.Name = "icReportFormat"
        Me.icReportFormat.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.lblReportFormat, Me.cboReportFormat})
        '
        '
        '
        Me.icReportFormat.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'lblReportFormat
        '
        Me.lblReportFormat.CanCustomize = False
        Me.lblReportFormat.Name = "lblReportFormat"
        Me.lblReportFormat.Text = "Format:"
        '
        'cboReportFormat
        '
        Me.cboReportFormat.CanCustomize = False
        Me.cboReportFormat.ComboWidth = 111
        Me.cboReportFormat.DropDownHeight = 106
        Me.cboReportFormat.Items.AddRange(New Object() {Me.ciReportHTML, Me.ciReportText, Me.ciReportXML, Me.ciReportPHPBB, Me.ciReportChart})
        Me.cboReportFormat.Name = "cboReportFormat"
        Me.SuperTooltip1.SetSuperTooltip(Me.cboReportFormat, New DevComponents.DotNetBar.SuperTooltipInfo("", "Report Format Style", "Choose between HTML, Text, XML and PHPBB report formats. The ribbon will be updat" & _
            "ed with the available reports for that style." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Document32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.cboReportFormat.Text = "ComboBoxItem2"
        '
        'ciReportHTML
        '
        Me.ciReportHTML.Text = "HTML"
        '
        'ciReportText
        '
        Me.ciReportText.Text = "Text"
        '
        'ciReportXML
        '
        Me.ciReportXML.Text = "XML"
        '
        'ciReportPHPBB
        '
        Me.ciReportPHPBB.Text = "PHPBB"
        '
        'ciReportChart
        '
        Me.ciReportChart.Text = "Charts"
        '
        'btnOpenReportFolder
        '
        Me.btnOpenReportFolder.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnOpenReportFolder.Image = Global.EveHQ.My.Resources.Resources.Folder32
        Me.btnOpenReportFolder.ImageFixedSize = New System.Drawing.Size(20, 20)
        Me.btnOpenReportFolder.ImagePaddingVertical = 3
        Me.btnOpenReportFolder.Name = "btnOpenReportFolder"
        Me.SuperTooltip1.SetSuperTooltip(Me.btnOpenReportFolder, New DevComponents.DotNetBar.SuperTooltipInfo("", "Open Report Folder", "Opens the report folder in Windows Explorer so that reports can be accessed from " & _
            "outside EveHQ.", Global.EveHQ.My.Resources.Resources.Info32, Global.EveHQ.My.Resources.Resources.Folder32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnOpenReportFolder.Text = "Open Report Folder"
        '
        'rtiCore
        '
        Me.rtiCore.Checked = True
        Me.rtiCore.Name = "rtiCore"
        Me.rtiCore.Panel = Me.rpCore
        Me.rtiCore.Text = "Core"
        '
        'rtiPlugins
        '
        Me.rtiPlugins.Name = "rtiPlugins"
        Me.rtiPlugins.Panel = Me.rpPlugins
        Me.rtiPlugins.Text = "Plug-ins"
        '
        'rtiReports
        '
        Me.rtiReports.Name = "rtiReports"
        Me.rtiReports.Panel = Me.rpReports
        Me.rtiReports.Text = "Reports"
        '
        'btnTheme
        '
        Me.btnTheme.AutoExpandOnClick = True
        Me.btnTheme.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far
        Me.btnTheme.Name = "btnTheme"
        Me.btnTheme.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnOffice2007Black, Me.btnOffice2007Blue, Me.btnOffice2007Silver, Me.btnOffice2010Black, Me.btnOffice2010Blue, Me.btnOffice2010Silver, Me.btnOffice2007VistaGlass, Me.btnWindows7Blue, Me.btnVisualStudio2010Blue, Me.btnCustomTheme})
        Me.btnTheme.Text = "Theme"
        '
        'btnOffice2007Black
        '
        Me.btnOffice2007Black.Checked = True
        Me.btnOffice2007Black.Command = Me.AppCommandTheme
        Me.btnOffice2007Black.CommandParameter = "Office2007Black"
        Me.btnOffice2007Black.Name = "btnOffice2007Black"
        Me.btnOffice2007Black.OptionGroup = "Style"
        Me.btnOffice2007Black.Text = "Office 2007 Black"
        '
        'AppCommandTheme
        '
        Me.AppCommandTheme.Name = "AppCommandTheme"
        '
        'btnOffice2007Blue
        '
        Me.btnOffice2007Blue.Command = Me.AppCommandTheme
        Me.btnOffice2007Blue.CommandParameter = "Office2007Blue"
        Me.btnOffice2007Blue.Name = "btnOffice2007Blue"
        Me.btnOffice2007Blue.OptionGroup = "Style"
        Me.btnOffice2007Blue.Text = "Office 2007 Blue"
        '
        'btnOffice2007Silver
        '
        Me.btnOffice2007Silver.Command = Me.AppCommandTheme
        Me.btnOffice2007Silver.CommandParameter = "Office2007Silver"
        Me.btnOffice2007Silver.Name = "btnOffice2007Silver"
        Me.btnOffice2007Silver.OptionGroup = "Style"
        Me.btnOffice2007Silver.Text = "Office 2007 Silver"
        '
        'btnOffice2010Black
        '
        Me.btnOffice2010Black.Command = Me.AppCommandTheme
        Me.btnOffice2010Black.CommandParameter = "Office2010Black"
        Me.btnOffice2010Black.Name = "btnOffice2010Black"
        Me.btnOffice2010Black.OptionGroup = "Style"
        Me.btnOffice2010Black.Text = "Office 2010 Black"
        '
        'btnOffice2010Blue
        '
        Me.btnOffice2010Blue.Command = Me.AppCommandTheme
        Me.btnOffice2010Blue.CommandParameter = "Office2010Blue"
        Me.btnOffice2010Blue.Name = "btnOffice2010Blue"
        Me.btnOffice2010Blue.OptionGroup = "Style"
        Me.btnOffice2010Blue.Text = "Office 2010 Blue"
        '
        'btnOffice2010Silver
        '
        Me.btnOffice2010Silver.Command = Me.AppCommandTheme
        Me.btnOffice2010Silver.CommandParameter = "Office2010Silver"
        Me.btnOffice2010Silver.Name = "btnOffice2010Silver"
        Me.btnOffice2010Silver.OptionGroup = "Style"
        Me.btnOffice2010Silver.Text = "Office 2010 Silver"
        '
        'btnOffice2007VistaGlass
        '
        Me.btnOffice2007VistaGlass.Command = Me.AppCommandTheme
        Me.btnOffice2007VistaGlass.CommandParameter = "Office2007VistaGlass"
        Me.btnOffice2007VistaGlass.Name = "btnOffice2007VistaGlass"
        Me.btnOffice2007VistaGlass.OptionGroup = "Style"
        Me.btnOffice2007VistaGlass.Text = "Vista Glass"
        '
        'btnWindows7Blue
        '
        Me.btnWindows7Blue.Command = Me.AppCommandTheme
        Me.btnWindows7Blue.CommandParameter = "Windows7Blue"
        Me.btnWindows7Blue.Name = "btnWindows7Blue"
        Me.btnWindows7Blue.OptionGroup = "Style"
        Me.btnWindows7Blue.Text = "Windows 7 Blue"
        '
        'btnVisualStudio2010Blue
        '
        Me.btnVisualStudio2010Blue.Command = Me.AppCommandTheme
        Me.btnVisualStudio2010Blue.CommandParameter = "VisualStudio2010Blue"
        Me.btnVisualStudio2010Blue.Name = "btnVisualStudio2010Blue"
        Me.btnVisualStudio2010Blue.OptionGroup = "Style"
        Me.btnVisualStudio2010Blue.Text = "VS 2010 Blue"
        '
        'btnCustomTheme
        '
        Me.btnCustomTheme.BeginGroup = True
        Me.btnCustomTheme.Command = Me.AppCommandTheme
        Me.btnCustomTheme.Name = "btnCustomTheme"
        Me.btnCustomTheme.Text = "Custom Theme"
        '
        'Office2007StartButton1
        '
        Me.Office2007StartButton1.AutoExpandOnClick = True
        Me.Office2007StartButton1.CanCustomize = False
        Me.Office2007StartButton1.HotTrackingStyle = DevComponents.DotNetBar.eHotTrackingStyle.Image
        Me.Office2007StartButton1.Image = Global.EveHQ.My.Resources.Resources.Aura32
        Me.Office2007StartButton1.ImagePaddingHorizontal = 2
        Me.Office2007StartButton1.ImagePaddingVertical = 2
        Me.Office2007StartButton1.Name = "Office2007StartButton1"
        Me.Office2007StartButton1.ShowSubItems = False
        Me.Office2007StartButton1.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ItemContainer1})
        Me.SuperTooltip1.SetSuperTooltip(Me.Office2007StartButton1, New DevComponents.DotNetBar.SuperTooltipInfo("EveHQ Help and Options", "", "Click here to show the menu for EveHQ Settings, Help and Updates." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Nothing, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow, True, False, New System.Drawing.Size(0, 0)))
        Me.Office2007StartButton1.Text = "&EveHQ"
        '
        'ItemContainer1
        '
        '
        '
        '
        Me.ItemContainer1.BackgroundStyle.Class = "RibbonFileMenuContainer"
        Me.ItemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ItemContainer1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.ItemContainer1.Name = "ItemContainer1"
        Me.ItemContainer1.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnFileSettings, Me.btnHelp, Me.btnFileExit})
        '
        '
        '
        Me.ItemContainer1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        'btnFileSettings
        '
        Me.btnFileSettings.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnFileSettings.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnFileSettings.Image = Global.EveHQ.My.Resources.Resources.Settings32
        Me.btnFileSettings.Name = "btnFileSettings"
        Me.btnFileSettings.SubItemsExpandWidth = 24
        Me.btnFileSettings.Text = "&Settings"
        Me.btnFileSettings.Tooltip = "Open the Settings form"
        '
        'btnHelp
        '
        Me.btnHelp.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnHelp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnHelp.Image = Global.EveHQ.My.Resources.Resources.Help32
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnCheckForUpdates, Me.btnUpdateEveHQ, Me.btnViewHistory, Me.btnAbout})
        Me.btnHelp.SubItemsExpandWidth = 24
        Me.btnHelp.Text = "&Help"
        Me.btnHelp.Tooltip = "Open the Help Menu"
        '
        'btnCheckForUpdates
        '
        Me.btnCheckForUpdates.Name = "btnCheckForUpdates"
        Me.btnCheckForUpdates.Text = "Check for Updates"
        Me.btnCheckForUpdates.Tooltip = "Check for available updates for EveHQ"
        '
        'btnUpdateEveHQ
        '
        Me.btnUpdateEveHQ.Enabled = False
        Me.btnUpdateEveHQ.Name = "btnUpdateEveHQ"
        Me.btnUpdateEveHQ.Text = "Update EveHQ"
        Me.btnUpdateEveHQ.Tooltip = "Update EveHQ Now"
        '
        'btnViewHistory
        '
        Me.btnViewHistory.Name = "btnViewHistory"
        Me.btnViewHistory.Text = "Version History"
        Me.btnViewHistory.Tooltip = "View the release history for EveHQ"
        '
        'btnAbout
        '
        Me.btnAbout.Image = Global.EveHQ.My.Resources.Resources.Info32
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Text = "About EveHQ..."
        Me.btnAbout.Tooltip = "Who, why and what..."
        '
        'btnFileExit
        '
        Me.btnFileExit.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.btnFileExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnFileExit.Image = Global.EveHQ.My.Resources.Resources.Exit32
        Me.btnFileExit.Name = "btnFileExit"
        Me.btnFileExit.SubItemsExpandWidth = 24
        Me.btnFileExit.Text = "E&xit"
        Me.btnFileExit.Tooltip = "Exit EveHQ"
        '
        'QatCustomizeItem1
        '
        Me.QatCustomizeItem1.Name = "QatCustomizeItem1"
        '
        'DotNetBarManager1
        '
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlY)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins)
        Me.DotNetBarManager1.BottomDockSite = Me.DockSite4
        Me.DotNetBarManager1.LeftDockSite = Me.DockSite1
        Me.DotNetBarManager1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.DotNetBarManager1.ParentForm = Me
        Me.DotNetBarManager1.RightDockSite = Me.DockSite2
        Me.DotNetBarManager1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.DotNetBarManager1.ToolbarBottomDockSite = Me.DockSite8
        Me.DotNetBarManager1.ToolbarLeftDockSite = Me.DockSite5
        Me.DotNetBarManager1.ToolbarRightDockSite = Me.DockSite6
        Me.DotNetBarManager1.ToolbarTopDockSite = Me.DockSite7
        Me.DotNetBarManager1.TopDockSite = Me.DockSite3
        '
        'DockSite4
        '
        Me.DockSite4.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite4.Controls.Add(Me.Bar1)
        Me.DockSite4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DockSite4.DocumentDockContainer = New DevComponents.DotNetBar.DocumentDockContainer(New DevComponents.DotNetBar.DocumentBaseContainer() {CType(New DevComponents.DotNetBar.DocumentBarContainer(Me.Bar1, 1231, 99), DevComponents.DotNetBar.DocumentBaseContainer)}, DevComponents.DotNetBar.eOrientation.Vertical)
        Me.DockSite4.Location = New System.Drawing.Point(5, 651)
        Me.DockSite4.Name = "DockSite4"
        Me.DockSite4.Size = New System.Drawing.Size(1231, 102)
        Me.DockSite4.TabIndex = 29
        Me.DockSite4.TabStop = False
        '
        'Bar1
        '
        Me.Bar1.AccessibleDescription = "DotNetBar Bar (Bar1)"
        Me.Bar1.AccessibleName = "DotNetBar Bar"
        Me.Bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar
        Me.Bar1.AutoHideTabTextAlwaysVisible = True
        Me.Bar1.CloseSingleTab = True
        Me.Bar1.Controls.Add(Me.pdc1)
        Me.Bar1.FadeEffect = True
        Me.Bar1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bar1.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.CaptionDotted
        Me.Bar1.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.DockContainerItem1})
        Me.Bar1.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer
        Me.Bar1.Location = New System.Drawing.Point(0, 3)
        Me.Bar1.Name = "Bar1"
        Me.Bar1.Size = New System.Drawing.Size(1231, 99)
        Me.Bar1.Stretch = True
        Me.Bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Bar1.TabIndex = 0
        Me.Bar1.TabStop = False
        Me.Bar1.Text = "Training Bar"
        '
        'pdc1
        '
        Me.pdc1.AutoScroll = True
        Me.pdc1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pdc1.Location = New System.Drawing.Point(3, 26)
        Me.pdc1.Name = "pdc1"
        Me.pdc1.Size = New System.Drawing.Size(1225, 70)
        Me.pdc1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pdc1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.pdc1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.pdc1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.pdc1.Style.GradientAngle = 90
        Me.pdc1.TabIndex = 0
        '
        'DockContainerItem1
        '
        Me.DockContainerItem1.Control = Me.pdc1
        Me.DockContainerItem1.DefaultFloatingSize = New System.Drawing.Size(320, 400)
        Me.DockContainerItem1.Name = "DockContainerItem1"
        Me.DockContainerItem1.Text = "Training Bar"
        '
        'DockSite1
        '
        Me.DockSite1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite1.Dock = System.Windows.Forms.DockStyle.Left
        Me.DockSite1.DocumentDockContainer = New DevComponents.DotNetBar.DocumentDockContainer()
        Me.DockSite1.Location = New System.Drawing.Point(5, 155)
        Me.DockSite1.Name = "DockSite1"
        Me.DockSite1.Size = New System.Drawing.Size(0, 496)
        Me.DockSite1.TabIndex = 26
        Me.DockSite1.TabStop = False
        '
        'DockSite2
        '
        Me.DockSite2.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite2.Dock = System.Windows.Forms.DockStyle.Right
        Me.DockSite2.DocumentDockContainer = New DevComponents.DotNetBar.DocumentDockContainer()
        Me.DockSite2.Location = New System.Drawing.Point(1236, 155)
        Me.DockSite2.Name = "DockSite2"
        Me.DockSite2.Size = New System.Drawing.Size(0, 496)
        Me.DockSite2.TabIndex = 27
        Me.DockSite2.TabStop = False
        '
        'DockSite8
        '
        Me.DockSite8.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite8.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DockSite8.Location = New System.Drawing.Point(5, 753)
        Me.DockSite8.Name = "DockSite8"
        Me.DockSite8.Size = New System.Drawing.Size(1231, 0)
        Me.DockSite8.TabIndex = 33
        Me.DockSite8.TabStop = False
        '
        'DockSite5
        '
        Me.DockSite5.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite5.Dock = System.Windows.Forms.DockStyle.Left
        Me.DockSite5.Location = New System.Drawing.Point(5, 155)
        Me.DockSite5.Name = "DockSite5"
        Me.DockSite5.Size = New System.Drawing.Size(0, 598)
        Me.DockSite5.TabIndex = 30
        Me.DockSite5.TabStop = False
        '
        'DockSite6
        '
        Me.DockSite6.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite6.Dock = System.Windows.Forms.DockStyle.Right
        Me.DockSite6.Location = New System.Drawing.Point(1236, 155)
        Me.DockSite6.Name = "DockSite6"
        Me.DockSite6.Size = New System.Drawing.Size(0, 598)
        Me.DockSite6.TabIndex = 31
        Me.DockSite6.TabStop = False
        '
        'DockSite7
        '
        Me.DockSite7.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite7.Dock = System.Windows.Forms.DockStyle.Top
        Me.DockSite7.Location = New System.Drawing.Point(5, 155)
        Me.DockSite7.Name = "DockSite7"
        Me.DockSite7.Size = New System.Drawing.Size(1231, 0)
        Me.DockSite7.TabIndex = 32
        Me.DockSite7.TabStop = False
        '
        'DockSite3
        '
        Me.DockSite3.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite3.Dock = System.Windows.Forms.DockStyle.Top
        Me.DockSite3.DocumentDockContainer = New DevComponents.DotNetBar.DocumentDockContainer()
        Me.DockSite3.Location = New System.Drawing.Point(5, 155)
        Me.DockSite3.Name = "DockSite3"
        Me.DockSite3.Size = New System.Drawing.Size(1231, 0)
        Me.DockSite3.TabIndex = 28
        Me.DockSite3.TabStop = False
        '
        'tabEveHQMDI
        '
        Me.tabEveHQMDI.AntiAlias = True
        Me.tabEveHQMDI.AutoSelectAttachedControl = True
        Me.tabEveHQMDI.CanReorderTabs = True
        Me.tabEveHQMDI.CloseButtonOnTabsVisible = True
        Me.tabEveHQMDI.CloseButtonVisible = True
        Me.tabEveHQMDI.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tabEveHQMDI.Location = New System.Drawing.Point(5, 628)
        Me.tabEveHQMDI.MdiForm = Me
        Me.tabEveHQMDI.MdiTabbedDocuments = True
        Me.tabEveHQMDI.Name = "tabEveHQMDI"
        Me.tabEveHQMDI.SelectedTab = Me.TabItem1
        Me.tabEveHQMDI.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tabEveHQMDI.ShowMdiChildIcon = False
        Me.tabEveHQMDI.Size = New System.Drawing.Size(1231, 23)
        Me.tabEveHQMDI.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tabEveHQMDI.TabIndex = 35
        Me.tabEveHQMDI.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabEveHQMDI.Tabs.Add(Me.TabItem1)
        '
        'TabItem1
        '
        Me.TabItem1.Name = "TabItem1"
        Me.TabItem1.Text = "TabItem1"
        '
        'barStatus
        '
        Me.barStatus.AccessibleDescription = "DotNetBar Bar (barStatus)"
        Me.barStatus.AccessibleName = "DotNetBar Bar"
        Me.barStatus.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar
        Me.barStatus.BarType = DevComponents.DotNetBar.eBarType.StatusBar
        Me.barStatus.CanAutoHide = False
        Me.barStatus.CanCustomize = False
        Me.barStatus.CanReorderTabs = False
        Me.barStatus.CanUndock = False
        Me.barStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barStatus.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.ResizeHandle
        Me.barStatus.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.lblTQStatus, Me.lblAPIStatus, Me.lblMailAPITime, Me.lblCharAPITime, Me.lblEveTime})
        Me.barStatus.Location = New System.Drawing.Point(5, 753)
        Me.barStatus.Name = "barStatus"
        Me.barStatus.Size = New System.Drawing.Size(1231, 22)
        Me.barStatus.Stretch = True
        Me.barStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.barStatus.TabIndex = 37
        Me.barStatus.TabStop = False
        '
        'lblTQStatus
        '
        Me.lblTQStatus.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.lblTQStatus.Name = "lblTQStatus"
        Me.lblTQStatus.PaddingBottom = 1
        Me.lblTQStatus.PaddingLeft = 3
        Me.lblTQStatus.PaddingRight = 3
        Me.lblTQStatus.PaddingTop = 1
        Me.lblTQStatus.Text = "Tranquility Status: Not Updated"
        Me.lblTQStatus.Tooltip = "Tranquility Server Status"
        Me.lblTQStatus.Width = 250
        '
        'lblAPIStatus
        '
        Me.lblAPIStatus.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.lblAPIStatus.Name = "lblAPIStatus"
        Me.lblAPIStatus.PaddingBottom = 1
        Me.lblAPIStatus.PaddingLeft = 3
        Me.lblAPIStatus.PaddingRight = 3
        Me.lblAPIStatus.PaddingTop = 1
        Me.lblAPIStatus.Stretch = True
        Me.lblAPIStatus.Text = "API Status: Not Updated This Session"
        Me.lblAPIStatus.Tooltip = "Last Character API download status"
        '
        'lblMailAPITime
        '
        Me.lblMailAPITime.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.lblMailAPITime.Name = "lblMailAPITime"
        Me.lblMailAPITime.PaddingBottom = 1
        Me.lblMailAPITime.PaddingLeft = 3
        Me.lblMailAPITime.PaddingRight = 3
        Me.lblMailAPITime.PaddingTop = 1
        Me.lblMailAPITime.Text = "Mail API"
        Me.lblMailAPITime.TextAlignment = System.Drawing.StringAlignment.Center
        Me.lblMailAPITime.Tooltip = "Time until next EveMail API download"
        Me.lblMailAPITime.Width = 80
        '
        'lblCharAPITime
        '
        Me.lblCharAPITime.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.lblCharAPITime.Name = "lblCharAPITime"
        Me.lblCharAPITime.PaddingBottom = 1
        Me.lblCharAPITime.PaddingLeft = 3
        Me.lblCharAPITime.PaddingRight = 3
        Me.lblCharAPITime.PaddingTop = 1
        Me.lblCharAPITime.Text = "Char API"
        Me.lblCharAPITime.TextAlignment = System.Drawing.StringAlignment.Center
        Me.lblCharAPITime.Tooltip = "Time until next Character API download"
        Me.lblCharAPITime.Width = 80
        '
        'lblEveTime
        '
        Me.lblEveTime.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.lblEveTime.Name = "lblEveTime"
        Me.lblEveTime.PaddingBottom = 1
        Me.lblEveTime.PaddingLeft = 3
        Me.lblEveTime.PaddingRight = 3
        Me.lblEveTime.PaddingTop = 1
        Me.lblEveTime.Text = "EveTime:"
        Me.lblEveTime.TextAlignment = System.Drawing.StringAlignment.Center
        Me.lblEveTime.Tooltip = "Eve Time"
        Me.lblEveTime.Width = 175
        '
        'SuperTooltip1
        '
        Me.SuperTooltip1.DefaultFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SuperTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.SuperTooltip1.PositionBelowControl = False
        '
        'StyleManager1
        '
        Me.StyleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007Black
        Me.StyleManager1.MetroColorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(26, Byte), Integer)))
        '
        'EveStatusIcon
        '
        Me.EveStatusIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.None
        Me.EveStatusIcon.BalloonTipText = ""
        Me.EveStatusIcon.BalloonTipTitle = ""
        Me.EveStatusIcon.ContextMenuStrip = Me.EveIconMenu
        Me.EveStatusIcon.Icon = CType(resources.GetObject("EveStatusIcon.Icon"), System.Drawing.Icon)
        Me.EveStatusIcon.Text = ""
        '
        'frmEveHQ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1241, 777)
        Me.Controls.Add(Me.tabEveHQMDI)
        Me.Controls.Add(Me.DockSite2)
        Me.Controls.Add(Me.DockSite1)
        Me.Controls.Add(Me.DockSite3)
        Me.Controls.Add(Me.DockSite4)
        Me.Controls.Add(Me.DockSite5)
        Me.Controls.Add(Me.DockSite6)
        Me.Controls.Add(Me.DockSite7)
        Me.Controls.Add(Me.DockSite8)
        Me.Controls.Add(Me.RibbonControl1)
        Me.Controls.Add(Me.barStatus)
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "frmEveHQ"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EveHQ"
        Me.rpPlugins.ResumeLayout(False)
        Me.EveIconMenu.ResumeLayout(False)
        Me.ctxTabbedMDI.ResumeLayout(False)
        Me.RibbonControl1.ResumeLayout(False)
        Me.RibbonControl1.PerformLayout()
        Me.rpCore.ResumeLayout(False)
        Me.rpReports.ResumeLayout(False)
        Me.DockSite4.ResumeLayout(False)
        CType(Me.Bar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Bar1.ResumeLayout(False)
        CType(Me.barStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents tmrEve As System.Windows.Forms.Timer
    Friend WithEvents EveIconMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ForceServerCheckToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RestoreWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideWhenMinimisedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrSkillUpdate As System.Windows.Forms.Timer
    Friend WithEvents ctxmnuLaunchEve1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxmnuLaunchEve2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxmnuLaunchEve3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxmnuLaunchEve4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrSave As System.Windows.Forms.Timer
    Friend WithEvents tmrBackup As System.Windows.Forms.Timer
    Friend WithEvents tmrModules As System.Windows.Forms.Timer
    Friend WithEvents ctxTabbedMDI As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCloseMDITab As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents fbd1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents iconEveHQMLW As System.Windows.Forms.NotifyIcon
    Friend WithEvents tmrMemory As System.Windows.Forms.Timer
    Friend WithEvents EveStatusIcon As EveHQ.Core.EveHQIcon
    Friend WithEvents RibbonControl1 As DevComponents.DotNetBar.RibbonControl
    Friend WithEvents rpCore As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents rbView As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents rpPlugins As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents rtiCore As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents rtiPlugins As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents Office2007StartButton1 As DevComponents.DotNetBar.Office2007StartButton
    Friend WithEvents ItemContainer1 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents btnFileSettings As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnFileExit As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents QatCustomizeItem1 As DevComponents.DotNetBar.QatCustomizeItem
    Friend WithEvents rbEveMail As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnViewPilotInfo As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnViewSkillTraining As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnViewPrices As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnViewDashboard As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnEveMail As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents lblEveMail As DevComponents.DotNetBar.LabelItem
    Friend WithEvents rbIGB As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnIGB As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents lblIGB As DevComponents.DotNetBar.LabelItem
    Friend WithEvents rbBackup As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnBackupEveHQ As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ItemContainer5 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents btnBackupEve As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbOptions As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnSave As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbAPI As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnManageAPI As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnQueryAPI As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbAPITools As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents ItemContainer2 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents btnAPIChecker As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnClearCache As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnClearCharacterCache As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnClearImageCache As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnClearAllCache As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHelp As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnCheckForUpdates As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnUpdateEveHQ As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnViewHistory As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnAbout As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbPlugins As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents rpReports As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents rtiReports As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents rbReportOptions As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents icReportOptions As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents icReportPilot As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents lblReportPilot As DevComponents.DotNetBar.LabelItem
    Friend WithEvents cboReportPilot As DevComponents.DotNetBar.ComboBoxItem
    Friend WithEvents icReportFormat As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents lblReportFormat As DevComponents.DotNetBar.LabelItem
    Friend WithEvents cboReportFormat As DevComponents.DotNetBar.ComboBoxItem
    Friend WithEvents ciReportHTML As DevComponents.Editors.ComboItem
    Friend WithEvents ciReportText As DevComponents.Editors.ComboItem
    Friend WithEvents ciReportXML As DevComponents.Editors.ComboItem
    Friend WithEvents ciReportPHPBB As DevComponents.Editors.ComboItem
    Friend WithEvents ciReportChart As DevComponents.Editors.ComboItem
    Friend WithEvents rbXML As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents rbText As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents rbHTML As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents rbCharts As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents rbPHPBB As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnHTMLCharSheet As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLTrainingTimes As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLTimeToLvl5 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLSkillLevels As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLTrainingQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLQueueShoppingList As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLSkillsAvailable As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLSkillsNotTrained As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLPartiallyTrained As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnHTMLSkillsCost As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnPHPBBCharacterSheet As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextCharacterSheet As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextTrainingTimes As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextTimeToLvl5 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextSkillLevels As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextTrainingQueue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextQueueShoppingList As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextSkillsAvailable As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextSkillsNotTrained As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextPartiallyTrained As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTextSkillsCost As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnChartSkillGroup As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnXMLCharacterXML As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnXMLTrainingXML As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnXMLCurrentCharOld As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnXMLCurrentCharNew As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnXMLCurrentTrainingOld As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnXMLECMExport As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbStandard As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnStdCharSummary As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnStdSkillLevels As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnStdAlloyReport As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnStdAsteroidReport As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnStdIceReport As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnOpenReportFolder As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents DotNetBarManager1 As DevComponents.DotNetBar.DotNetBarManager
    Friend WithEvents DockSite4 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite1 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite2 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite3 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite5 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite6 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite7 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite8 As DevComponents.DotNetBar.DockSite
    Friend WithEvents Bar1 As DevComponents.DotNetBar.Bar
    Friend WithEvents pdc1 As DevComponents.DotNetBar.PanelDockContainer
    Friend WithEvents DockContainerItem1 As DevComponents.DotNetBar.DockContainerItem
    Friend WithEvents btnChartSkillCost As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnViewReqs As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbSQLTools As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents ItemContainer3 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents btnSQLQueryTool As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnTheme As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnOffice2007Black As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnOffice2007Blue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnOffice2007Silver As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnOffice2010Blue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnOffice2010Silver As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnOffice2007VistaGlass As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnWindows7Blue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents AppCommandTheme As DevComponents.DotNetBar.Command
    Friend WithEvents btnCustomTheme As DevComponents.DotNetBar.ColorPickerDropDown
    Friend WithEvents tabEveHQMDI As DevComponents.DotNetBar.TabStrip
    Friend WithEvents TabItem1 As DevComponents.DotNetBar.TabItem
    Friend WithEvents btnOffice2010Black As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents barStatus As DevComponents.DotNetBar.Bar
    Friend WithEvents lblTQStatus As DevComponents.DotNetBar.LabelItem
    Friend WithEvents lblAPIStatus As DevComponents.DotNetBar.LabelItem
    Friend WithEvents lblMailAPITime As DevComponents.DotNetBar.LabelItem
    Friend WithEvents lblCharAPITime As DevComponents.DotNetBar.LabelItem
    Friend WithEvents lblEveTime As DevComponents.DotNetBar.LabelItem
    Friend WithEvents SuperTooltip1 As DevComponents.DotNetBar.SuperTooltip
    Friend WithEvents btnOpenCacheFolder As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents rbHelp As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents btnInfoHelp As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnVisualStudio2010Blue As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents StyleManager1 As DevComponents.DotNetBar.StyleManager

End Class
