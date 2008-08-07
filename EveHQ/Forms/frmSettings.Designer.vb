<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmSettings
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
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("General")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Database Format")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Eve Accounts")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Eve Folders")
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Eve API & Server")
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("FTP Accounts")
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("G15 Display")
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("IGB")
        Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Market Prices")
        Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Notifications")
        Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Pilots")
        Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Plug Ins")
        Dim TreeNode13 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Proxy Server")
        Dim TreeNode14 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Training Overlay")
        Dim TreeNode15 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Training Queue")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.gbGeneral = New System.Windows.Forms.GroupBox
        Me.txtUpdateLocation = New System.Windows.Forms.TextBox
        Me.lblUpdateLocation = New System.Windows.Forms.Label
        Me.gbPilotScreenColours = New System.Windows.Forms.GroupBox
        Me.btnResetPilotColours = New System.Windows.Forms.Button
        Me.pbPilotLevel5 = New System.Windows.Forms.PictureBox
        Me.lblLevel5Colour = New System.Windows.Forms.Label
        Me.pbPilotPartial = New System.Windows.Forms.PictureBox
        Me.lblPilotPartiallyTrainedColour = New System.Windows.Forms.Label
        Me.pbPilotCurrent = New System.Windows.Forms.PictureBox
        Me.lblPilotCurrentColour = New System.Windows.Forms.Label
        Me.pbPilotStandard = New System.Windows.Forms.PictureBox
        Me.lblPilotStandardColour = New System.Windows.Forms.Label
        Me.gbPanelColours = New System.Windows.Forms.GroupBox
        Me.btnResetPanelColours = New System.Windows.Forms.Button
        Me.pbPanelHighlight = New System.Windows.Forms.PictureBox
        Me.lblPanelHighlight = New System.Windows.Forms.Label
        Me.pbPanelText = New System.Windows.Forms.PictureBox
        Me.lblPanelText = New System.Windows.Forms.Label
        Me.pbPanelRight = New System.Windows.Forms.PictureBox
        Me.lblPanelRight = New System.Windows.Forms.Label
        Me.pbPanelLeft = New System.Windows.Forms.PictureBox
        Me.lblPanelLeft = New System.Windows.Forms.Label
        Me.pbPanelBottomRight = New System.Windows.Forms.PictureBox
        Me.lblPanelBottomRight = New System.Windows.Forms.Label
        Me.pbPanelTopLeft = New System.Windows.Forms.PictureBox
        Me.lblPanelTopLeft = New System.Windows.Forms.Label
        Me.pbPanelOutline = New System.Windows.Forms.PictureBox
        Me.lblPanelOutline = New System.Windows.Forms.Label
        Me.pbPanelBackground = New System.Windows.Forms.PictureBox
        Me.lblPanelBackground = New System.Windows.Forms.Label
        Me.chkMinimiseOnExit = New System.Windows.Forms.CheckBox
        Me.lblMDITabStyle = New System.Windows.Forms.Label
        Me.cboMDITabStyle = New System.Windows.Forms.ComboBox
        Me.chkEncryptSettings = New System.Windows.Forms.CheckBox
        Me.cboStartupPilot = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cboStartupView = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkAutoCheck = New System.Windows.Forms.CheckBox
        Me.chkAutoMinimise = New System.Windows.Forms.CheckBox
        Me.chkAutoRun = New System.Windows.Forms.CheckBox
        Me.chkAutoHide = New System.Windows.Forms.CheckBox
        Me.gbEveAccounts = New System.Windows.Forms.GroupBox
        Me.btnGetData = New System.Windows.Forms.Button
        Me.btnDeleteAccount = New System.Windows.Forms.Button
        Me.btnEditAccount = New System.Windows.Forms.Button
        Me.btnAddAccount = New System.Windows.Forms.Button
        Me.lvAccounts = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.gbPilots = New System.Windows.Forms.GroupBox
        Me.btnAddPilotFromXML = New System.Windows.Forms.Button
        Me.btnDeletePilot = New System.Windows.Forms.Button
        Me.btnAddPilot = New System.Windows.Forms.Button
        Me.lvwPilots = New System.Windows.Forms.ListView
        Me.colPilot = New System.Windows.Forms.ColumnHeader
        Me.colID = New System.Windows.Forms.ColumnHeader
        Me.colAccount = New System.Windows.Forms.ColumnHeader
        Me.gbIGB = New System.Windows.Forms.GroupBox
        Me.chkStartIGBonLoad = New System.Windows.Forms.CheckBox
        Me.nudIGBPort = New System.Windows.Forms.NumericUpDown
        Me.lblIGBPort = New System.Windows.Forms.Label
        Me.gbFTPAccounts = New System.Windows.Forms.GroupBox
        Me.btnTestUpload = New System.Windows.Forms.Button
        Me.btnDeleteFTP = New System.Windows.Forms.Button
        Me.btnEditFTP = New System.Windows.Forms.Button
        Me.btnAddFTP = New System.Windows.Forms.Button
        Me.lvwFTP = New System.Windows.Forms.ListView
        Me.FTPName = New System.Windows.Forms.ColumnHeader
        Me.Server = New System.Windows.Forms.ColumnHeader
        Me.gbEveFolders = New System.Windows.Forms.GroupBox
        Me.gbLocation4 = New System.Windows.Forms.GroupBox
        Me.lblFriendlyName4 = New System.Windows.Forms.Label
        Me.txtFriendlyName4 = New System.Windows.Forms.TextBox
        Me.lblCacheSize4 = New System.Windows.Forms.Label
        Me.chkLUA4 = New System.Windows.Forms.CheckBox
        Me.lblEveDir4 = New System.Windows.Forms.Label
        Me.btnEveDir4 = New System.Windows.Forms.Button
        Me.btnClear4 = New System.Windows.Forms.Button
        Me.gbLocation3 = New System.Windows.Forms.GroupBox
        Me.lblFriendlyName3 = New System.Windows.Forms.Label
        Me.txtFriendlyName3 = New System.Windows.Forms.TextBox
        Me.lblCacheSize3 = New System.Windows.Forms.Label
        Me.chkLUA3 = New System.Windows.Forms.CheckBox
        Me.lblEveDir3 = New System.Windows.Forms.Label
        Me.btnEveDir3 = New System.Windows.Forms.Button
        Me.btnClear3 = New System.Windows.Forms.Button
        Me.gbLocation2 = New System.Windows.Forms.GroupBox
        Me.lblFriendlyName2 = New System.Windows.Forms.Label
        Me.txtFriendlyName2 = New System.Windows.Forms.TextBox
        Me.lblCacheSize2 = New System.Windows.Forms.Label
        Me.chkLUA2 = New System.Windows.Forms.CheckBox
        Me.lblEveDir2 = New System.Windows.Forms.Label
        Me.btnEveDir2 = New System.Windows.Forms.Button
        Me.btnClear2 = New System.Windows.Forms.Button
        Me.gbLocation1 = New System.Windows.Forms.GroupBox
        Me.lblFriendlyName1 = New System.Windows.Forms.Label
        Me.txtFriendlyName1 = New System.Windows.Forms.TextBox
        Me.lblCacheSize1 = New System.Windows.Forms.Label
        Me.chkLUA1 = New System.Windows.Forms.CheckBox
        Me.lblEveDir1 = New System.Windows.Forms.Label
        Me.btnEveDir1 = New System.Windows.Forms.Button
        Me.btnClear1 = New System.Windows.Forms.Button
        Me.gbTrainingQueue = New System.Windows.Forms.GroupBox
        Me.chkOmitCurrentSkill = New System.Windows.Forms.CheckBox
        Me.pbPartiallyTrainedColour = New System.Windows.Forms.PictureBox
        Me.lblPartiallyTrainedColour = New System.Windows.Forms.Label
        Me.chkDeleteCompletedSkills = New System.Windows.Forms.CheckBox
        Me.pbReadySkillColour = New System.Windows.Forms.PictureBox
        Me.lblReadySkillColour = New System.Windows.Forms.Label
        Me.pbDowntimeClashColour = New System.Windows.Forms.PictureBox
        Me.lblDowntimeClashColour = New System.Windows.Forms.Label
        Me.pbBothPreReqColour = New System.Windows.Forms.PictureBox
        Me.lblBothPreReqColour = New System.Windows.Forms.Label
        Me.pbHasPreReqColour = New System.Windows.Forms.PictureBox
        Me.pbIsPreReqColour = New System.Windows.Forms.PictureBox
        Me.lblHasPreReqColour = New System.Windows.Forms.Label
        Me.lblIsPreReqColour = New System.Windows.Forms.Label
        Me.lblSkillQueueColours = New System.Windows.Forms.Label
        Me.chkContinueTraining = New System.Windows.Forms.CheckBox
        Me.lblQueueColumns = New System.Windows.Forms.Label
        Me.clbColumns = New System.Windows.Forms.CheckedListBox
        Me.gbDatabaseFormat = New System.Windows.Forms.GroupBox
        Me.gbAccess = New System.Windows.Forms.GroupBox
        Me.chkUseAppDirForDB = New System.Windows.Forms.CheckBox
        Me.btnBrowseMDB = New System.Windows.Forms.Button
        Me.txtMDBPassword = New System.Windows.Forms.TextBox
        Me.txtMDBUsername = New System.Windows.Forms.TextBox
        Me.txtMDBServer = New System.Windows.Forms.TextBox
        Me.lblMDBPassword = New System.Windows.Forms.Label
        Me.lblMDBUser = New System.Windows.Forms.Label
        Me.lblMDBFilename = New System.Windows.Forms.Label
        Me.gbMySQL = New System.Windows.Forms.GroupBox
        Me.txtMySQLDatabase = New System.Windows.Forms.TextBox
        Me.lblMySQLDatabase = New System.Windows.Forms.Label
        Me.txtMySQLPassword = New System.Windows.Forms.TextBox
        Me.txtMySQLUsername = New System.Windows.Forms.TextBox
        Me.txtMySQLServer = New System.Windows.Forms.TextBox
        Me.lblMySQLPassword = New System.Windows.Forms.Label
        Me.lblMySQLUser = New System.Windows.Forms.Label
        Me.lblMySQLServer = New System.Windows.Forms.Label
        Me.btnTestDB = New System.Windows.Forms.Button
        Me.gbMSSQL = New System.Windows.Forms.GroupBox
        Me.txtMSSQLDatabase = New System.Windows.Forms.TextBox
        Me.lblMSSQLDatabase = New System.Windows.Forms.Label
        Me.lblMSSQLSecurity = New System.Windows.Forms.Label
        Me.radMSSQLDatabase = New System.Windows.Forms.RadioButton
        Me.radMSSQLWindows = New System.Windows.Forms.RadioButton
        Me.txtMSSQLPassword = New System.Windows.Forms.TextBox
        Me.txtMSSQLUsername = New System.Windows.Forms.TextBox
        Me.txtMSSQLServer = New System.Windows.Forms.TextBox
        Me.lblMSSQLPassword = New System.Windows.Forms.Label
        Me.lblMSSQLUser = New System.Windows.Forms.Label
        Me.lblMSSQLServer = New System.Windows.Forms.Label
        Me.cboFormat = New System.Windows.Forms.ComboBox
        Me.lblFormat = New System.Windows.Forms.Label
        Me.gbProxyServer = New System.Windows.Forms.GroupBox
        Me.gbProxyServerInfo = New System.Windows.Forms.GroupBox
        Me.lblProxyPassword = New System.Windows.Forms.Label
        Me.lblProxyUsername = New System.Windows.Forms.Label
        Me.txtProxyPassword = New System.Windows.Forms.TextBox
        Me.txtProxyUsername = New System.Windows.Forms.TextBox
        Me.radUseSpecifiedCreds = New System.Windows.Forms.RadioButton
        Me.lblProxyServer = New System.Windows.Forms.Label
        Me.txtProxyServer = New System.Windows.Forms.TextBox
        Me.radUseDefaultCreds = New System.Windows.Forms.RadioButton
        Me.chkUseProxy = New System.Windows.Forms.CheckBox
        Me.gbEveServer = New System.Windows.Forms.GroupBox
        Me.chkShowAPIStatusForm = New System.Windows.Forms.CheckBox
        Me.gbAPIServer = New System.Windows.Forms.GroupBox
        Me.chkUseCCPBackup = New System.Windows.Forms.CheckBox
        Me.chkUseAPIRSServer = New System.Windows.Forms.CheckBox
        Me.txtAPIRSServer = New System.Windows.Forms.TextBox
        Me.lblAPIRSServer = New System.Windows.Forms.Label
        Me.txtCCPAPIServer = New System.Windows.Forms.TextBox
        Me.lblCCPAPIServer = New System.Windows.Forms.Label
        Me.chkAutoAPI = New System.Windows.Forms.CheckBox
        Me.gbAPIRelayServer = New System.Windows.Forms.GroupBox
        Me.chkAPIRSAutoStart = New System.Windows.Forms.CheckBox
        Me.nudAPIRSPort = New System.Windows.Forms.NumericUpDown
        Me.lblAPIRSPort = New System.Windows.Forms.Label
        Me.chkActivateAPIRS = New System.Windows.Forms.CheckBox
        Me.chkEnableEveStatus = New System.Windows.Forms.CheckBox
        Me.lblCurrentOffset = New System.Windows.Forms.Label
        Me.lblServerOffset = New System.Windows.Forms.Label
        Me.trackServerOffset = New System.Windows.Forms.TrackBar
        Me.gbPlugIns = New System.Windows.Forms.GroupBox
        Me.btnTidyPlugins = New System.Windows.Forms.Button
        Me.btnRefreshPlugins = New System.Windows.Forms.Button
        Me.lblPlugInInfo = New System.Windows.Forms.Label
        Me.lblDetectedPlugIns = New System.Windows.Forms.Label
        Me.lvwPlugins = New System.Windows.Forms.ListView
        Me.colPlugInName = New System.Windows.Forms.ColumnHeader
        Me.colStatus = New System.Windows.Forms.ColumnHeader
        Me.gbNotifications = New System.Windows.Forms.GroupBox
        Me.chkNotifyEarly = New System.Windows.Forms.CheckBox
        Me.chkNotifyNow = New System.Windows.Forms.CheckBox
        Me.lblNotifyMe = New System.Windows.Forms.Label
        Me.btnSoundTest = New System.Windows.Forms.Button
        Me.btnSelectSoundFile = New System.Windows.Forms.Button
        Me.lblSoundFile = New System.Windows.Forms.Label
        Me.chkNotifySound = New System.Windows.Forms.CheckBox
        Me.lblNotifyOffset = New System.Windows.Forms.Label
        Me.trackNotifyOffset = New System.Windows.Forms.TrackBar
        Me.gbEmailOptions = New System.Windows.Forms.GroupBox
        Me.btnTestEmail = New System.Windows.Forms.Button
        Me.lblEmailPassword = New System.Windows.Forms.Label
        Me.txtEmailPassword = New System.Windows.Forms.TextBox
        Me.txtEmailUsername = New System.Windows.Forms.TextBox
        Me.lblEmailUsername = New System.Windows.Forms.Label
        Me.chkSMTPAuthentication = New System.Windows.Forms.CheckBox
        Me.lblEMailAddress = New System.Windows.Forms.Label
        Me.txtEmailAddress = New System.Windows.Forms.TextBox
        Me.txtSMTPServer = New System.Windows.Forms.TextBox
        Me.lblSMTPServer = New System.Windows.Forms.Label
        Me.chkNotifyEmail = New System.Windows.Forms.CheckBox
        Me.chkNotifyDialog = New System.Windows.Forms.CheckBox
        Me.chkNotifyToolTip = New System.Windows.Forms.CheckBox
        Me.nudShutdownNotifyPeriod = New System.Windows.Forms.NumericUpDown
        Me.lblShutdownNotifyPeriod = New System.Windows.Forms.Label
        Me.chkShutdownNotify = New System.Windows.Forms.CheckBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog
        Me.tvwSettings = New System.Windows.Forms.TreeView
        Me.gbColours = New System.Windows.Forms.GroupBox
        Me.pbPilotTrainingHighlight = New System.Windows.Forms.PictureBox
        Me.lblPilotTrainingHighlight = New System.Windows.Forms.Label
        Me.cd1 = New System.Windows.Forms.ColorDialog
        Me.gbTrainingOverlay = New System.Windows.Forms.GroupBox
        Me.chkClickThroughOverlay = New System.Windows.Forms.CheckBox
        Me.pbFontColour = New System.Windows.Forms.PictureBox
        Me.lblFontColour = New System.Windows.Forms.Label
        Me.chkShowOverlayOnStartup = New System.Windows.Forms.CheckBox
        Me.nudOverlayYOffset = New System.Windows.Forms.NumericUpDown
        Me.nudOverlayXOffset = New System.Windows.Forms.NumericUpDown
        Me.lvlOverlayYOffset = New System.Windows.Forms.Label
        Me.lblOverlayXOffset = New System.Windows.Forms.Label
        Me.lblOverlayOffset = New System.Windows.Forms.Label
        Me.radBottomRight = New System.Windows.Forms.RadioButton
        Me.radBottomLeft = New System.Windows.Forms.RadioButton
        Me.radTopRight = New System.Windows.Forms.RadioButton
        Me.lblOverlayPosition = New System.Windows.Forms.Label
        Me.radTopLeft = New System.Windows.Forms.RadioButton
        Me.pbPanelColour = New System.Windows.Forms.PictureBox
        Me.pbBorderColour = New System.Windows.Forms.PictureBox
        Me.lblTransparancyValue = New System.Windows.Forms.Label
        Me.tbTransparancy = New System.Windows.Forms.TrackBar
        Me.lblTransparancy = New System.Windows.Forms.Label
        Me.lblPanelColour = New System.Windows.Forms.Label
        Me.lblBorderColour = New System.Windows.Forms.Label
        Me.gbG15 = New System.Windows.Forms.GroupBox
        Me.nudCycleTime = New System.Windows.Forms.NumericUpDown
        Me.lblCycleTime = New System.Windows.Forms.Label
        Me.chkCyclePilots = New System.Windows.Forms.CheckBox
        Me.chkActivateG15 = New System.Windows.Forms.CheckBox
        Me.gbMarketPrices = New System.Windows.Forms.GroupBox
        Me.btnResetGrid = New System.Windows.Forms.Button
        Me.txtSearchPrices = New System.Windows.Forms.TextBox
        Me.lblSearchPrices = New System.Windows.Forms.Label
        Me.lblUpdatePrice = New System.Windows.Forms.Label
        Me.txtUpdatePrice = New System.Windows.Forms.TextBox
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
        Me.lblMarketPriceStats = New System.Windows.Forms.Label
        Me.lblUpdateStatus = New System.Windows.Forms.Label
        Me.lblUpdateStatusLabel = New System.Windows.Forms.Label
        Me.lblLastUpdateTime = New System.Windows.Forms.Label
        Me.lblLastUpdate = New System.Windows.Forms.Label
        Me.btnUpdatePrices = New System.Windows.Forms.Button
        Me.gbGeneral.SuspendLayout()
        Me.gbPilotScreenColours.SuspendLayout()
        CType(Me.pbPilotLevel5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotPartial, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotCurrent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotStandard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPanelColours.SuspendLayout()
        CType(Me.pbPanelHighlight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPanelText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPanelRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPanelLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPanelBottomRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPanelTopLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPanelOutline, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPanelBackground, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbEveAccounts.SuspendLayout()
        Me.gbPilots.SuspendLayout()
        Me.gbIGB.SuspendLayout()
        CType(Me.nudIGBPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbFTPAccounts.SuspendLayout()
        Me.gbEveFolders.SuspendLayout()
        Me.gbLocation4.SuspendLayout()
        Me.gbLocation3.SuspendLayout()
        Me.gbLocation2.SuspendLayout()
        Me.gbLocation1.SuspendLayout()
        Me.gbTrainingQueue.SuspendLayout()
        CType(Me.pbPartiallyTrainedColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbReadySkillColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDowntimeClashColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbBothPreReqColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbHasPreReqColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbIsPreReqColour, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDatabaseFormat.SuspendLayout()
        Me.gbAccess.SuspendLayout()
        Me.gbMySQL.SuspendLayout()
        Me.gbMSSQL.SuspendLayout()
        Me.gbProxyServer.SuspendLayout()
        Me.gbProxyServerInfo.SuspendLayout()
        Me.gbEveServer.SuspendLayout()
        Me.gbAPIServer.SuspendLayout()
        Me.gbAPIRelayServer.SuspendLayout()
        CType(Me.nudAPIRSPort, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trackServerOffset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPlugIns.SuspendLayout()
        Me.gbNotifications.SuspendLayout()
        CType(Me.trackNotifyOffset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbEmailOptions.SuspendLayout()
        CType(Me.nudShutdownNotifyPeriod, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbColours.SuspendLayout()
        CType(Me.pbPilotTrainingHighlight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbTrainingOverlay.SuspendLayout()
        CType(Me.pbFontColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudOverlayYOffset, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudOverlayXOffset, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPanelColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbBorderColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbTransparancy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbG15.SuspendLayout()
        CType(Me.nudCycleTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbMarketPrices.SuspendLayout()
        Me.ctxPrices.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbGeneral
        '
        Me.gbGeneral.Controls.Add(Me.txtUpdateLocation)
        Me.gbGeneral.Controls.Add(Me.lblUpdateLocation)
        Me.gbGeneral.Controls.Add(Me.gbPilotScreenColours)
        Me.gbGeneral.Controls.Add(Me.gbPanelColours)
        Me.gbGeneral.Controls.Add(Me.chkMinimiseOnExit)
        Me.gbGeneral.Controls.Add(Me.lblMDITabStyle)
        Me.gbGeneral.Controls.Add(Me.cboMDITabStyle)
        Me.gbGeneral.Controls.Add(Me.chkEncryptSettings)
        Me.gbGeneral.Controls.Add(Me.cboStartupPilot)
        Me.gbGeneral.Controls.Add(Me.Label3)
        Me.gbGeneral.Controls.Add(Me.cboStartupView)
        Me.gbGeneral.Controls.Add(Me.Label2)
        Me.gbGeneral.Controls.Add(Me.chkAutoCheck)
        Me.gbGeneral.Controls.Add(Me.chkAutoMinimise)
        Me.gbGeneral.Controls.Add(Me.chkAutoRun)
        Me.gbGeneral.Controls.Add(Me.chkAutoHide)
        Me.gbGeneral.Location = New System.Drawing.Point(615, 324)
        Me.gbGeneral.Name = "gbGeneral"
        Me.gbGeneral.Size = New System.Drawing.Size(100, 53)
        Me.gbGeneral.TabIndex = 1
        Me.gbGeneral.TabStop = False
        Me.gbGeneral.Text = "General Settings"
        Me.gbGeneral.Visible = False
        '
        'txtUpdateLocation
        '
        Me.txtUpdateLocation.Location = New System.Drawing.Point(116, 182)
        Me.txtUpdateLocation.Name = "txtUpdateLocation"
        Me.txtUpdateLocation.Size = New System.Drawing.Size(449, 20)
        Me.txtUpdateLocation.TabIndex = 39
        '
        'lblUpdateLocation
        '
        Me.lblUpdateLocation.AutoSize = True
        Me.lblUpdateLocation.Location = New System.Drawing.Point(21, 185)
        Me.lblUpdateLocation.Name = "lblUpdateLocation"
        Me.lblUpdateLocation.Size = New System.Drawing.Size(89, 13)
        Me.lblUpdateLocation.TabIndex = 38
        Me.lblUpdateLocation.Text = "Update Location:"
        '
        'gbPilotScreenColours
        '
        Me.gbPilotScreenColours.Controls.Add(Me.btnResetPilotColours)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotLevel5)
        Me.gbPilotScreenColours.Controls.Add(Me.lblLevel5Colour)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotPartial)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotPartiallyTrainedColour)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotCurrent)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotCurrentColour)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotStandard)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotStandardColour)
        Me.gbPilotScreenColours.Location = New System.Drawing.Point(245, 223)
        Me.gbPilotScreenColours.Name = "gbPilotScreenColours"
        Me.gbPilotScreenColours.Size = New System.Drawing.Size(215, 257)
        Me.gbPilotScreenColours.TabIndex = 37
        Me.gbPilotScreenColours.TabStop = False
        Me.gbPilotScreenColours.Text = "Pilot Screen Skill Colours"
        '
        'btnResetPilotColours
        '
        Me.btnResetPilotColours.Location = New System.Drawing.Point(39, 222)
        Me.btnResetPilotColours.Name = "btnResetPilotColours"
        Me.btnResetPilotColours.Size = New System.Drawing.Size(145, 23)
        Me.btnResetPilotColours.TabIndex = 52
        Me.btnResetPilotColours.Text = "Reset To Defaults"
        Me.btnResetPilotColours.UseVisualStyleBackColor = True
        '
        'pbPilotLevel5
        '
        Me.pbPilotLevel5.BackColor = System.Drawing.Color.Thistle
        Me.pbPilotLevel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotLevel5.Location = New System.Drawing.Point(160, 96)
        Me.pbPilotLevel5.Name = "pbPilotLevel5"
        Me.pbPilotLevel5.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotLevel5.TabIndex = 43
        Me.pbPilotLevel5.TabStop = False
        '
        'lblLevel5Colour
        '
        Me.lblLevel5Colour.AutoSize = True
        Me.lblLevel5Colour.Location = New System.Drawing.Point(14, 99)
        Me.lblLevel5Colour.Name = "lblLevel5Colour"
        Me.lblLevel5Colour.Size = New System.Drawing.Size(64, 13)
        Me.lblLevel5Colour.TabIndex = 42
        Me.lblLevel5Colour.Text = "Level 5 Skill"
        '
        'pbPilotPartial
        '
        Me.pbPilotPartial.BackColor = System.Drawing.Color.Gold
        Me.pbPilotPartial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotPartial.Location = New System.Drawing.Point(160, 72)
        Me.pbPilotPartial.Name = "pbPilotPartial"
        Me.pbPilotPartial.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotPartial.TabIndex = 41
        Me.pbPilotPartial.TabStop = False
        '
        'lblPilotPartiallyTrainedColour
        '
        Me.lblPilotPartiallyTrainedColour.AutoSize = True
        Me.lblPilotPartiallyTrainedColour.Location = New System.Drawing.Point(14, 75)
        Me.lblPilotPartiallyTrainedColour.Name = "lblPilotPartiallyTrainedColour"
        Me.lblPilotPartiallyTrainedColour.Size = New System.Drawing.Size(104, 13)
        Me.lblPilotPartiallyTrainedColour.TabIndex = 40
        Me.lblPilotPartiallyTrainedColour.Text = "Partially Trained Skill"
        '
        'pbPilotCurrent
        '
        Me.pbPilotCurrent.BackColor = System.Drawing.Color.LimeGreen
        Me.pbPilotCurrent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotCurrent.Location = New System.Drawing.Point(160, 48)
        Me.pbPilotCurrent.Name = "pbPilotCurrent"
        Me.pbPilotCurrent.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotCurrent.TabIndex = 39
        Me.pbPilotCurrent.TabStop = False
        '
        'lblPilotCurrentColour
        '
        Me.lblPilotCurrentColour.AutoSize = True
        Me.lblPilotCurrentColour.Location = New System.Drawing.Point(14, 51)
        Me.lblPilotCurrentColour.Name = "lblPilotCurrentColour"
        Me.lblPilotCurrentColour.Size = New System.Drawing.Size(104, 13)
        Me.lblPilotCurrentColour.TabIndex = 38
        Me.lblPilotCurrentColour.Text = "Current Training Skill"
        '
        'pbPilotStandard
        '
        Me.pbPilotStandard.BackColor = System.Drawing.Color.White
        Me.pbPilotStandard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotStandard.Location = New System.Drawing.Point(160, 24)
        Me.pbPilotStandard.Name = "pbPilotStandard"
        Me.pbPilotStandard.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotStandard.TabIndex = 37
        Me.pbPilotStandard.TabStop = False
        '
        'lblPilotStandardColour
        '
        Me.lblPilotStandardColour.AutoSize = True
        Me.lblPilotStandardColour.Location = New System.Drawing.Point(14, 27)
        Me.lblPilotStandardColour.Name = "lblPilotStandardColour"
        Me.lblPilotStandardColour.Size = New System.Drawing.Size(72, 13)
        Me.lblPilotStandardColour.TabIndex = 36
        Me.lblPilotStandardColour.Text = "Standard Skill"
        '
        'gbPanelColours
        '
        Me.gbPanelColours.Controls.Add(Me.btnResetPanelColours)
        Me.gbPanelColours.Controls.Add(Me.pbPanelHighlight)
        Me.gbPanelColours.Controls.Add(Me.lblPanelHighlight)
        Me.gbPanelColours.Controls.Add(Me.pbPanelText)
        Me.gbPanelColours.Controls.Add(Me.lblPanelText)
        Me.gbPanelColours.Controls.Add(Me.pbPanelRight)
        Me.gbPanelColours.Controls.Add(Me.lblPanelRight)
        Me.gbPanelColours.Controls.Add(Me.pbPanelLeft)
        Me.gbPanelColours.Controls.Add(Me.lblPanelLeft)
        Me.gbPanelColours.Controls.Add(Me.pbPanelBottomRight)
        Me.gbPanelColours.Controls.Add(Me.lblPanelBottomRight)
        Me.gbPanelColours.Controls.Add(Me.pbPanelTopLeft)
        Me.gbPanelColours.Controls.Add(Me.lblPanelTopLeft)
        Me.gbPanelColours.Controls.Add(Me.pbPanelOutline)
        Me.gbPanelColours.Controls.Add(Me.lblPanelOutline)
        Me.gbPanelColours.Controls.Add(Me.pbPanelBackground)
        Me.gbPanelColours.Controls.Add(Me.lblPanelBackground)
        Me.gbPanelColours.Location = New System.Drawing.Point(24, 223)
        Me.gbPanelColours.Name = "gbPanelColours"
        Me.gbPanelColours.Size = New System.Drawing.Size(215, 257)
        Me.gbPanelColours.TabIndex = 36
        Me.gbPanelColours.TabStop = False
        Me.gbPanelColours.Text = "Panel Colours"
        '
        'btnResetPanelColours
        '
        Me.btnResetPanelColours.Location = New System.Drawing.Point(38, 222)
        Me.btnResetPanelColours.Name = "btnResetPanelColours"
        Me.btnResetPanelColours.Size = New System.Drawing.Size(145, 23)
        Me.btnResetPanelColours.TabIndex = 52
        Me.btnResetPanelColours.Text = "Reset To Defaults"
        Me.btnResetPanelColours.UseVisualStyleBackColor = True
        '
        'pbPanelHighlight
        '
        Me.pbPanelHighlight.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbPanelHighlight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPanelHighlight.Location = New System.Drawing.Point(160, 192)
        Me.pbPanelHighlight.Name = "pbPanelHighlight"
        Me.pbPanelHighlight.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelHighlight.TabIndex = 51
        Me.pbPanelHighlight.TabStop = False
        '
        'lblPanelHighlight
        '
        Me.lblPanelHighlight.AutoSize = True
        Me.lblPanelHighlight.Location = New System.Drawing.Point(14, 195)
        Me.lblPanelHighlight.Name = "lblPanelHighlight"
        Me.lblPanelHighlight.Size = New System.Drawing.Size(111, 13)
        Me.lblPanelHighlight.TabIndex = 50
        Me.lblPanelHighlight.Text = "Caption Text Highlight"
        '
        'pbPanelText
        '
        Me.pbPanelText.BackColor = System.Drawing.Color.Black
        Me.pbPanelText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPanelText.Location = New System.Drawing.Point(160, 168)
        Me.pbPanelText.Name = "pbPanelText"
        Me.pbPanelText.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelText.TabIndex = 49
        Me.pbPanelText.TabStop = False
        '
        'lblPanelText
        '
        Me.lblPanelText.AutoSize = True
        Me.lblPanelText.Location = New System.Drawing.Point(14, 171)
        Me.lblPanelText.Name = "lblPanelText"
        Me.lblPanelText.Size = New System.Drawing.Size(67, 13)
        Me.lblPanelText.TabIndex = 48
        Me.lblPanelText.Text = "Caption Text"
        '
        'pbPanelRight
        '
        Me.pbPanelRight.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbPanelRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPanelRight.Location = New System.Drawing.Point(160, 144)
        Me.pbPanelRight.Name = "pbPanelRight"
        Me.pbPanelRight.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelRight.TabIndex = 47
        Me.pbPanelRight.TabStop = False
        '
        'lblPanelRight
        '
        Me.lblPanelRight.AutoSize = True
        Me.lblPanelRight.Location = New System.Drawing.Point(14, 147)
        Me.lblPanelRight.Name = "lblPanelRight"
        Me.lblPanelRight.Size = New System.Drawing.Size(101, 13)
        Me.lblPanelRight.TabIndex = 46
        Me.lblPanelRight.Text = "Caption Panel Right"
        '
        'pbPanelLeft
        '
        Me.pbPanelLeft.BackColor = System.Drawing.Color.RoyalBlue
        Me.pbPanelLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPanelLeft.Location = New System.Drawing.Point(160, 120)
        Me.pbPanelLeft.Name = "pbPanelLeft"
        Me.pbPanelLeft.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelLeft.TabIndex = 45
        Me.pbPanelLeft.TabStop = False
        '
        'lblPanelLeft
        '
        Me.lblPanelLeft.AutoSize = True
        Me.lblPanelLeft.Location = New System.Drawing.Point(14, 123)
        Me.lblPanelLeft.Name = "lblPanelLeft"
        Me.lblPanelLeft.Size = New System.Drawing.Size(94, 13)
        Me.lblPanelLeft.TabIndex = 44
        Me.lblPanelLeft.Text = "Caption Panel Left"
        '
        'pbPanelBottomRight
        '
        Me.pbPanelBottomRight.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbPanelBottomRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPanelBottomRight.Location = New System.Drawing.Point(160, 96)
        Me.pbPanelBottomRight.Name = "pbPanelBottomRight"
        Me.pbPanelBottomRight.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelBottomRight.TabIndex = 43
        Me.pbPanelBottomRight.TabStop = False
        '
        'lblPanelBottomRight
        '
        Me.lblPanelBottomRight.AutoSize = True
        Me.lblPanelBottomRight.Location = New System.Drawing.Point(14, 99)
        Me.lblPanelBottomRight.Name = "lblPanelBottomRight"
        Me.lblPanelBottomRight.Size = New System.Drawing.Size(124, 13)
        Me.lblPanelBottomRight.TabIndex = 42
        Me.lblPanelBottomRight.Text = "Main Panel Bottom Right"
        '
        'pbPanelTopLeft
        '
        Me.pbPanelTopLeft.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbPanelTopLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPanelTopLeft.Location = New System.Drawing.Point(160, 72)
        Me.pbPanelTopLeft.Name = "pbPanelTopLeft"
        Me.pbPanelTopLeft.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelTopLeft.TabIndex = 41
        Me.pbPanelTopLeft.TabStop = False
        '
        'lblPanelTopLeft
        '
        Me.lblPanelTopLeft.AutoSize = True
        Me.lblPanelTopLeft.Location = New System.Drawing.Point(14, 75)
        Me.lblPanelTopLeft.Name = "lblPanelTopLeft"
        Me.lblPanelTopLeft.Size = New System.Drawing.Size(103, 13)
        Me.lblPanelTopLeft.TabIndex = 40
        Me.lblPanelTopLeft.Text = "Main Panel Top Left"
        '
        'pbPanelOutline
        '
        Me.pbPanelOutline.BackColor = System.Drawing.Color.SteelBlue
        Me.pbPanelOutline.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPanelOutline.Location = New System.Drawing.Point(160, 48)
        Me.pbPanelOutline.Name = "pbPanelOutline"
        Me.pbPanelOutline.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelOutline.TabIndex = 39
        Me.pbPanelOutline.TabStop = False
        '
        'lblPanelOutline
        '
        Me.lblPanelOutline.AutoSize = True
        Me.lblPanelOutline.Location = New System.Drawing.Point(14, 51)
        Me.lblPanelOutline.Name = "lblPanelOutline"
        Me.lblPanelOutline.Size = New System.Drawing.Size(70, 13)
        Me.lblPanelOutline.TabIndex = 38
        Me.lblPanelOutline.Text = "Panel Outline"
        '
        'pbPanelBackground
        '
        Me.pbPanelBackground.BackColor = System.Drawing.Color.Navy
        Me.pbPanelBackground.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPanelBackground.Location = New System.Drawing.Point(160, 24)
        Me.pbPanelBackground.Name = "pbPanelBackground"
        Me.pbPanelBackground.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelBackground.TabIndex = 37
        Me.pbPanelBackground.TabStop = False
        '
        'lblPanelBackground
        '
        Me.lblPanelBackground.AutoSize = True
        Me.lblPanelBackground.Location = New System.Drawing.Point(14, 27)
        Me.lblPanelBackground.Name = "lblPanelBackground"
        Me.lblPanelBackground.Size = New System.Drawing.Size(95, 13)
        Me.lblPanelBackground.TabIndex = 36
        Me.lblPanelBackground.Text = "Panel Background"
        '
        'chkMinimiseOnExit
        '
        Me.chkMinimiseOnExit.AutoSize = True
        Me.chkMinimiseOnExit.Location = New System.Drawing.Point(24, 80)
        Me.chkMinimiseOnExit.Name = "chkMinimiseOnExit"
        Me.chkMinimiseOnExit.Size = New System.Drawing.Size(101, 17)
        Me.chkMinimiseOnExit.TabIndex = 11
        Me.chkMinimiseOnExit.Text = "Minimise on Exit"
        Me.chkMinimiseOnExit.UseVisualStyleBackColor = True
        '
        'lblMDITabStyle
        '
        Me.lblMDITabStyle.AutoSize = True
        Me.lblMDITabStyle.Location = New System.Drawing.Point(313, 89)
        Me.lblMDITabStyle.Name = "lblMDITabStyle"
        Me.lblMDITabStyle.Size = New System.Drawing.Size(78, 13)
        Me.lblMDITabStyle.TabIndex = 10
        Me.lblMDITabStyle.Text = "MDI Tab Style:"
        '
        'cboMDITabStyle
        '
        Me.cboMDITabStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMDITabStyle.FormattingEnabled = True
        Me.cboMDITabStyle.Items.AddRange(New Object() {"Flat Buttons", "Raised Buttons", "Tabs"})
        Me.cboMDITabStyle.Location = New System.Drawing.Point(404, 86)
        Me.cboMDITabStyle.Name = "cboMDITabStyle"
        Me.cboMDITabStyle.Size = New System.Drawing.Size(161, 21)
        Me.cboMDITabStyle.Sorted = True
        Me.cboMDITabStyle.TabIndex = 9
        '
        'chkEncryptSettings
        '
        Me.chkEncryptSettings.AutoSize = True
        Me.chkEncryptSettings.Location = New System.Drawing.Point(24, 126)
        Me.chkEncryptSettings.Name = "chkEncryptSettings"
        Me.chkEncryptSettings.Size = New System.Drawing.Size(122, 17)
        Me.chkEncryptSettings.TabIndex = 8
        Me.chkEncryptSettings.Text = "Encrypt Settings File"
        Me.chkEncryptSettings.UseVisualStyleBackColor = True
        '
        'cboStartupPilot
        '
        Me.cboStartupPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStartupPilot.FormattingEnabled = True
        Me.cboStartupPilot.Location = New System.Drawing.Point(404, 59)
        Me.cboStartupPilot.Name = "cboStartupPilot"
        Me.cboStartupPilot.Size = New System.Drawing.Size(161, 21)
        Me.cboStartupPilot.Sorted = True
        Me.cboStartupPilot.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(313, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Default Pilot:"
        '
        'cboStartupView
        '
        Me.cboStartupView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStartupView.FormattingEnabled = True
        Me.cboStartupView.Items.AddRange(New Object() {"Pilot Information", "Pilot Summary Report", "Skill Training"})
        Me.cboStartupView.Location = New System.Drawing.Point(404, 32)
        Me.cboStartupView.Name = "cboStartupView"
        Me.cboStartupView.Size = New System.Drawing.Size(161, 21)
        Me.cboStartupView.Sorted = True
        Me.cboStartupView.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(313, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "View on Startup:"
        '
        'chkAutoCheck
        '
        Me.chkAutoCheck.AutoSize = True
        Me.chkAutoCheck.Location = New System.Drawing.Point(24, 149)
        Me.chkAutoCheck.Name = "chkAutoCheck"
        Me.chkAutoCheck.Size = New System.Drawing.Size(215, 17)
        Me.chkAutoCheck.TabIndex = 3
        Me.chkAutoCheck.Text = "Check for Updates When EveHQ Starts"
        Me.chkAutoCheck.UseVisualStyleBackColor = True
        '
        'chkAutoMinimise
        '
        Me.chkAutoMinimise.AutoSize = True
        Me.chkAutoMinimise.Location = New System.Drawing.Point(24, 57)
        Me.chkAutoMinimise.Name = "chkAutoMinimise"
        Me.chkAutoMinimise.Size = New System.Drawing.Size(166, 17)
        Me.chkAutoMinimise.TabIndex = 1
        Me.chkAutoMinimise.Text = "Minimise When EveHQ Starts"
        Me.chkAutoMinimise.UseVisualStyleBackColor = True
        '
        'chkAutoRun
        '
        Me.chkAutoRun.AutoSize = True
        Me.chkAutoRun.Location = New System.Drawing.Point(24, 103)
        Me.chkAutoRun.Name = "chkAutoRun"
        Me.chkAutoRun.Size = New System.Drawing.Size(183, 17)
        Me.chkAutoRun.TabIndex = 2
        Me.chkAutoRun.Text = "Run EveHQ on Windows Startup"
        Me.chkAutoRun.UseVisualStyleBackColor = True
        '
        'chkAutoHide
        '
        Me.chkAutoHide.AutoSize = True
        Me.chkAutoHide.Location = New System.Drawing.Point(24, 34)
        Me.chkAutoHide.Name = "chkAutoHide"
        Me.chkAutoHide.Size = New System.Drawing.Size(249, 17)
        Me.chkAutoHide.TabIndex = 0
        Me.chkAutoHide.Text = "Hide EveHQ from the Taskbar when Minimising"
        Me.chkAutoHide.UseVisualStyleBackColor = True
        '
        'gbEveAccounts
        '
        Me.gbEveAccounts.BackColor = System.Drawing.Color.Transparent
        Me.gbEveAccounts.Controls.Add(Me.btnGetData)
        Me.gbEveAccounts.Controls.Add(Me.btnDeleteAccount)
        Me.gbEveAccounts.Controls.Add(Me.btnEditAccount)
        Me.gbEveAccounts.Controls.Add(Me.btnAddAccount)
        Me.gbEveAccounts.Controls.Add(Me.lvAccounts)
        Me.gbEveAccounts.Location = New System.Drawing.Point(659, 130)
        Me.gbEveAccounts.Name = "gbEveAccounts"
        Me.gbEveAccounts.Size = New System.Drawing.Size(93, 45)
        Me.gbEveAccounts.TabIndex = 16
        Me.gbEveAccounts.TabStop = False
        Me.gbEveAccounts.Text = "Accounts"
        Me.gbEveAccounts.Visible = False
        '
        'btnGetData
        '
        Me.btnGetData.Location = New System.Drawing.Point(403, 127)
        Me.btnGetData.Name = "btnGetData"
        Me.btnGetData.Size = New System.Drawing.Size(90, 35)
        Me.btnGetData.TabIndex = 22
        Me.btnGetData.Text = "Retrieve Account Data"
        '
        'btnDeleteAccount
        '
        Me.btnDeleteAccount.Location = New System.Drawing.Point(403, 81)
        Me.btnDeleteAccount.Name = "btnDeleteAccount"
        Me.btnDeleteAccount.Size = New System.Drawing.Size(90, 25)
        Me.btnDeleteAccount.TabIndex = 21
        Me.btnDeleteAccount.Text = "Delete Account"
        '
        'btnEditAccount
        '
        Me.btnEditAccount.Location = New System.Drawing.Point(403, 50)
        Me.btnEditAccount.Name = "btnEditAccount"
        Me.btnEditAccount.Size = New System.Drawing.Size(90, 25)
        Me.btnEditAccount.TabIndex = 20
        Me.btnEditAccount.Text = "Edit Account"
        '
        'btnAddAccount
        '
        Me.btnAddAccount.Location = New System.Drawing.Point(403, 19)
        Me.btnAddAccount.Name = "btnAddAccount"
        Me.btnAddAccount.Size = New System.Drawing.Size(90, 25)
        Me.btnAddAccount.TabIndex = 19
        Me.btnAddAccount.Text = "Add Account"
        '
        'lvAccounts
        '
        Me.lvAccounts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvAccounts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvAccounts.FullRowSelect = True
        Me.lvAccounts.GridLines = True
        Me.lvAccounts.Location = New System.Drawing.Point(12, 19)
        Me.lvAccounts.Name = "lvAccounts"
        Me.lvAccounts.Size = New System.Drawing.Size(383, 16)
        Me.lvAccounts.TabIndex = 18
        Me.lvAccounts.UseCompatibleStateImageBehavior = False
        Me.lvAccounts.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Account Name"
        Me.ColumnHeader1.Width = 200
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "User ID"
        Me.ColumnHeader2.Width = 150
        '
        'gbPilots
        '
        Me.gbPilots.BackColor = System.Drawing.Color.Transparent
        Me.gbPilots.Controls.Add(Me.btnAddPilotFromXML)
        Me.gbPilots.Controls.Add(Me.btnDeletePilot)
        Me.gbPilots.Controls.Add(Me.btnAddPilot)
        Me.gbPilots.Controls.Add(Me.lvwPilots)
        Me.gbPilots.Location = New System.Drawing.Point(194, 12)
        Me.gbPilots.Name = "gbPilots"
        Me.gbPilots.Size = New System.Drawing.Size(701, 498)
        Me.gbPilots.TabIndex = 17
        Me.gbPilots.TabStop = False
        Me.gbPilots.Text = "Pilots"
        Me.gbPilots.Visible = False
        '
        'btnAddPilotFromXML
        '
        Me.btnAddPilotFromXML.Location = New System.Drawing.Point(421, 50)
        Me.btnAddPilotFromXML.Name = "btnAddPilotFromXML"
        Me.btnAddPilotFromXML.Size = New System.Drawing.Size(120, 25)
        Me.btnAddPilotFromXML.TabIndex = 22
        Me.btnAddPilotFromXML.Text = "Add Pilot from XML"
        '
        'btnDeletePilot
        '
        Me.btnDeletePilot.Location = New System.Drawing.Point(421, 81)
        Me.btnDeletePilot.Name = "btnDeletePilot"
        Me.btnDeletePilot.Size = New System.Drawing.Size(120, 25)
        Me.btnDeletePilot.TabIndex = 21
        Me.btnDeletePilot.Text = "Delete Pilot"
        '
        'btnAddPilot
        '
        Me.btnAddPilot.Location = New System.Drawing.Point(421, 19)
        Me.btnAddPilot.Name = "btnAddPilot"
        Me.btnAddPilot.Size = New System.Drawing.Size(120, 25)
        Me.btnAddPilot.TabIndex = 19
        Me.btnAddPilot.Text = "Add Pilot"
        '
        'lvwPilots
        '
        Me.lvwPilots.AllowColumnReorder = True
        Me.lvwPilots.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwPilots.CheckBoxes = True
        Me.lvwPilots.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPilot, Me.colID, Me.colAccount})
        Me.lvwPilots.FullRowSelect = True
        Me.lvwPilots.GridLines = True
        Me.lvwPilots.Location = New System.Drawing.Point(12, 19)
        Me.lvwPilots.Name = "lvwPilots"
        Me.lvwPilots.Size = New System.Drawing.Size(398, 473)
        Me.lvwPilots.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwPilots.TabIndex = 18
        Me.lvwPilots.UseCompatibleStateImageBehavior = False
        Me.lvwPilots.View = System.Windows.Forms.View.Details
        '
        'colPilot
        '
        Me.colPilot.Text = "Pilot Name"
        Me.colPilot.Width = 150
        '
        'colID
        '
        Me.colID.Text = "Pilot ID"
        Me.colID.Width = 90
        '
        'colAccount
        '
        Me.colAccount.Text = "Linked to Account"
        Me.colAccount.Width = 130
        '
        'gbIGB
        '
        Me.gbIGB.Controls.Add(Me.chkStartIGBonLoad)
        Me.gbIGB.Controls.Add(Me.nudIGBPort)
        Me.gbIGB.Controls.Add(Me.lblIGBPort)
        Me.gbIGB.Location = New System.Drawing.Point(213, 244)
        Me.gbIGB.Name = "gbIGB"
        Me.gbIGB.Size = New System.Drawing.Size(81, 32)
        Me.gbIGB.TabIndex = 19
        Me.gbIGB.TabStop = False
        Me.gbIGB.Text = "IGB Settings"
        Me.gbIGB.Visible = False
        '
        'chkStartIGBonLoad
        '
        Me.chkStartIGBonLoad.AutoSize = True
        Me.chkStartIGBonLoad.Location = New System.Drawing.Point(29, 84)
        Me.chkStartIGBonLoad.Name = "chkStartIGBonLoad"
        Me.chkStartIGBonLoad.Size = New System.Drawing.Size(157, 17)
        Me.chkStartIGBonLoad.TabIndex = 2
        Me.chkStartIGBonLoad.Text = "Run IGB on EveHQ Startup"
        Me.chkStartIGBonLoad.UseVisualStyleBackColor = True
        '
        'nudIGBPort
        '
        Me.nudIGBPort.Location = New System.Drawing.Point(98, 42)
        Me.nudIGBPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.nudIGBPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudIGBPort.Name = "nudIGBPort"
        Me.nudIGBPort.Size = New System.Drawing.Size(120, 20)
        Me.nudIGBPort.TabIndex = 1
        Me.nudIGBPort.Value = New Decimal(New Integer() {26001, 0, 0, 0})
        '
        'lblIGBPort
        '
        Me.lblIGBPort.AutoSize = True
        Me.lblIGBPort.Location = New System.Drawing.Point(26, 44)
        Me.lblIGBPort.Name = "lblIGBPort"
        Me.lblIGBPort.Size = New System.Drawing.Size(50, 13)
        Me.lblIGBPort.TabIndex = 0
        Me.lblIGBPort.Text = "IGB Port:"
        '
        'gbFTPAccounts
        '
        Me.gbFTPAccounts.BackColor = System.Drawing.Color.Transparent
        Me.gbFTPAccounts.Controls.Add(Me.btnTestUpload)
        Me.gbFTPAccounts.Controls.Add(Me.btnDeleteFTP)
        Me.gbFTPAccounts.Controls.Add(Me.btnEditFTP)
        Me.gbFTPAccounts.Controls.Add(Me.btnAddFTP)
        Me.gbFTPAccounts.Controls.Add(Me.lvwFTP)
        Me.gbFTPAccounts.Location = New System.Drawing.Point(411, 295)
        Me.gbFTPAccounts.Name = "gbFTPAccounts"
        Me.gbFTPAccounts.Size = New System.Drawing.Size(97, 18)
        Me.gbFTPAccounts.TabIndex = 17
        Me.gbFTPAccounts.TabStop = False
        Me.gbFTPAccounts.Text = "FTP Accounts"
        Me.gbFTPAccounts.Visible = False
        '
        'btnTestUpload
        '
        Me.btnTestUpload.Location = New System.Drawing.Point(337, 249)
        Me.btnTestUpload.Name = "btnTestUpload"
        Me.btnTestUpload.Size = New System.Drawing.Size(75, 25)
        Me.btnTestUpload.TabIndex = 22
        Me.btnTestUpload.Text = "Test Upload"
        Me.btnTestUpload.UseVisualStyleBackColor = True
        Me.btnTestUpload.Visible = False
        '
        'btnDeleteFTP
        '
        Me.btnDeleteFTP.Location = New System.Drawing.Point(204, 249)
        Me.btnDeleteFTP.Name = "btnDeleteFTP"
        Me.btnDeleteFTP.Size = New System.Drawing.Size(90, 25)
        Me.btnDeleteFTP.TabIndex = 21
        Me.btnDeleteFTP.Text = "Delete Account"
        '
        'btnEditFTP
        '
        Me.btnEditFTP.Location = New System.Drawing.Point(108, 249)
        Me.btnEditFTP.Name = "btnEditFTP"
        Me.btnEditFTP.Size = New System.Drawing.Size(90, 25)
        Me.btnEditFTP.TabIndex = 20
        Me.btnEditFTP.Text = "Edit Account"
        '
        'btnAddFTP
        '
        Me.btnAddFTP.Location = New System.Drawing.Point(12, 249)
        Me.btnAddFTP.Name = "btnAddFTP"
        Me.btnAddFTP.Size = New System.Drawing.Size(90, 25)
        Me.btnAddFTP.TabIndex = 19
        Me.btnAddFTP.Text = "Add Account"
        '
        'lvwFTP
        '
        Me.lvwFTP.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.FTPName, Me.Server})
        Me.lvwFTP.FullRowSelect = True
        Me.lvwFTP.GridLines = True
        Me.lvwFTP.Location = New System.Drawing.Point(12, 19)
        Me.lvwFTP.Name = "lvwFTP"
        Me.lvwFTP.Size = New System.Drawing.Size(400, 224)
        Me.lvwFTP.TabIndex = 18
        Me.lvwFTP.UseCompatibleStateImageBehavior = False
        Me.lvwFTP.View = System.Windows.Forms.View.Details
        '
        'FTPName
        '
        Me.FTPName.Text = "FTP Name"
        Me.FTPName.Width = 150
        '
        'Server
        '
        Me.Server.Text = "Server"
        Me.Server.Width = 220
        '
        'gbEveFolders
        '
        Me.gbEveFolders.Controls.Add(Me.gbLocation4)
        Me.gbEveFolders.Controls.Add(Me.gbLocation3)
        Me.gbEveFolders.Controls.Add(Me.gbLocation2)
        Me.gbEveFolders.Controls.Add(Me.gbLocation1)
        Me.gbEveFolders.Location = New System.Drawing.Point(222, 206)
        Me.gbEveFolders.Name = "gbEveFolders"
        Me.gbEveFolders.Size = New System.Drawing.Size(117, 29)
        Me.gbEveFolders.TabIndex = 3
        Me.gbEveFolders.TabStop = False
        Me.gbEveFolders.Text = "Eve Folders"
        Me.gbEveFolders.Visible = False
        '
        'gbLocation4
        '
        Me.gbLocation4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbLocation4.Controls.Add(Me.lblFriendlyName4)
        Me.gbLocation4.Controls.Add(Me.txtFriendlyName4)
        Me.gbLocation4.Controls.Add(Me.lblCacheSize4)
        Me.gbLocation4.Controls.Add(Me.chkLUA4)
        Me.gbLocation4.Controls.Add(Me.lblEveDir4)
        Me.gbLocation4.Controls.Add(Me.btnEveDir4)
        Me.gbLocation4.Controls.Add(Me.btnClear4)
        Me.gbLocation4.Location = New System.Drawing.Point(6, 353)
        Me.gbLocation4.Name = "gbLocation4"
        Me.gbLocation4.Size = New System.Drawing.Size(104, 100)
        Me.gbLocation4.TabIndex = 15
        Me.gbLocation4.TabStop = False
        Me.gbLocation4.Text = "Eve Location 4"
        '
        'lblFriendlyName4
        '
        Me.lblFriendlyName4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFriendlyName4.AutoSize = True
        Me.lblFriendlyName4.Location = New System.Drawing.Point(-208, 72)
        Me.lblFriendlyName4.Name = "lblFriendlyName4"
        Me.lblFriendlyName4.Size = New System.Drawing.Size(77, 13)
        Me.lblFriendlyName4.TabIndex = 15
        Me.lblFriendlyName4.Text = "Friendly Name:"
        '
        'txtFriendlyName4
        '
        Me.txtFriendlyName4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFriendlyName4.Location = New System.Drawing.Point(-125, 69)
        Me.txtFriendlyName4.Name = "txtFriendlyName4"
        Me.txtFriendlyName4.Size = New System.Drawing.Size(150, 20)
        Me.txtFriendlyName4.TabIndex = 14
        '
        'lblCacheSize4
        '
        Me.lblCacheSize4.AutoSize = True
        Me.lblCacheSize4.Location = New System.Drawing.Point(87, 70)
        Me.lblCacheSize4.Name = "lblCacheSize4"
        Me.lblCacheSize4.Size = New System.Drawing.Size(64, 13)
        Me.lblCacheSize4.TabIndex = 13
        Me.lblCacheSize4.Text = "Cache Size:"
        Me.lblCacheSize4.Visible = False
        '
        'chkLUA4
        '
        Me.chkLUA4.AutoSize = True
        Me.chkLUA4.Location = New System.Drawing.Point(6, 69)
        Me.chkLUA4.Name = "chkLUA4"
        Me.chkLUA4.Size = New System.Drawing.Size(75, 17)
        Me.chkLUA4.TabIndex = 12
        Me.chkLUA4.Text = "/LUA Off?"
        Me.chkLUA4.UseVisualStyleBackColor = True
        '
        'lblEveDir4
        '
        Me.lblEveDir4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEveDir4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEveDir4.Location = New System.Drawing.Point(6, 16)
        Me.lblEveDir4.Name = "lblEveDir4"
        Me.lblEveDir4.Padding = New System.Windows.Forms.Padding(2)
        Me.lblEveDir4.Size = New System.Drawing.Size(19, 50)
        Me.lblEveDir4.TabIndex = 9
        Me.lblEveDir4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnEveDir4
        '
        Me.btnEveDir4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEveDir4.Location = New System.Drawing.Point(34, 16)
        Me.btnEveDir4.Name = "btnEveDir4"
        Me.btnEveDir4.Size = New System.Drawing.Size(64, 22)
        Me.btnEveDir4.TabIndex = 10
        Me.btnEveDir4.Text = "Change..."
        Me.btnEveDir4.UseVisualStyleBackColor = True
        '
        'btnClear4
        '
        Me.btnClear4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear4.Location = New System.Drawing.Point(34, 44)
        Me.btnClear4.Name = "btnClear4"
        Me.btnClear4.Size = New System.Drawing.Size(64, 22)
        Me.btnClear4.TabIndex = 11
        Me.btnClear4.Text = "Clear"
        Me.btnClear4.UseVisualStyleBackColor = True
        '
        'gbLocation3
        '
        Me.gbLocation3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbLocation3.Controls.Add(Me.lblFriendlyName3)
        Me.gbLocation3.Controls.Add(Me.txtFriendlyName3)
        Me.gbLocation3.Controls.Add(Me.lblCacheSize3)
        Me.gbLocation3.Controls.Add(Me.chkLUA3)
        Me.gbLocation3.Controls.Add(Me.lblEveDir3)
        Me.gbLocation3.Controls.Add(Me.btnEveDir3)
        Me.gbLocation3.Controls.Add(Me.btnClear3)
        Me.gbLocation3.Location = New System.Drawing.Point(6, 247)
        Me.gbLocation3.Name = "gbLocation3"
        Me.gbLocation3.Size = New System.Drawing.Size(104, 100)
        Me.gbLocation3.TabIndex = 14
        Me.gbLocation3.TabStop = False
        Me.gbLocation3.Text = "Eve Location 3"
        '
        'lblFriendlyName3
        '
        Me.lblFriendlyName3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFriendlyName3.AutoSize = True
        Me.lblFriendlyName3.Location = New System.Drawing.Point(-208, 72)
        Me.lblFriendlyName3.Name = "lblFriendlyName3"
        Me.lblFriendlyName3.Size = New System.Drawing.Size(77, 13)
        Me.lblFriendlyName3.TabIndex = 13
        Me.lblFriendlyName3.Text = "Friendly Name:"
        '
        'txtFriendlyName3
        '
        Me.txtFriendlyName3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFriendlyName3.Location = New System.Drawing.Point(-125, 69)
        Me.txtFriendlyName3.Name = "txtFriendlyName3"
        Me.txtFriendlyName3.Size = New System.Drawing.Size(150, 20)
        Me.txtFriendlyName3.TabIndex = 12
        '
        'lblCacheSize3
        '
        Me.lblCacheSize3.AutoSize = True
        Me.lblCacheSize3.Location = New System.Drawing.Point(87, 70)
        Me.lblCacheSize3.Name = "lblCacheSize3"
        Me.lblCacheSize3.Size = New System.Drawing.Size(64, 13)
        Me.lblCacheSize3.TabIndex = 11
        Me.lblCacheSize3.Text = "Cache Size:"
        Me.lblCacheSize3.Visible = False
        '
        'chkLUA3
        '
        Me.chkLUA3.AutoSize = True
        Me.chkLUA3.Location = New System.Drawing.Point(6, 69)
        Me.chkLUA3.Name = "chkLUA3"
        Me.chkLUA3.Size = New System.Drawing.Size(75, 17)
        Me.chkLUA3.TabIndex = 10
        Me.chkLUA3.Text = "/LUA Off?"
        Me.chkLUA3.UseVisualStyleBackColor = True
        '
        'lblEveDir3
        '
        Me.lblEveDir3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEveDir3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEveDir3.Location = New System.Drawing.Point(6, 16)
        Me.lblEveDir3.Name = "lblEveDir3"
        Me.lblEveDir3.Padding = New System.Windows.Forms.Padding(2)
        Me.lblEveDir3.Size = New System.Drawing.Size(19, 50)
        Me.lblEveDir3.TabIndex = 6
        Me.lblEveDir3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnEveDir3
        '
        Me.btnEveDir3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEveDir3.Location = New System.Drawing.Point(34, 16)
        Me.btnEveDir3.Name = "btnEveDir3"
        Me.btnEveDir3.Size = New System.Drawing.Size(64, 22)
        Me.btnEveDir3.TabIndex = 8
        Me.btnEveDir3.Text = "Change..."
        Me.btnEveDir3.UseVisualStyleBackColor = True
        '
        'btnClear3
        '
        Me.btnClear3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear3.Location = New System.Drawing.Point(34, 44)
        Me.btnClear3.Name = "btnClear3"
        Me.btnClear3.Size = New System.Drawing.Size(64, 22)
        Me.btnClear3.TabIndex = 9
        Me.btnClear3.Text = "Clear"
        Me.btnClear3.UseVisualStyleBackColor = True
        '
        'gbLocation2
        '
        Me.gbLocation2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbLocation2.Controls.Add(Me.lblFriendlyName2)
        Me.gbLocation2.Controls.Add(Me.txtFriendlyName2)
        Me.gbLocation2.Controls.Add(Me.lblCacheSize2)
        Me.gbLocation2.Controls.Add(Me.chkLUA2)
        Me.gbLocation2.Controls.Add(Me.lblEveDir2)
        Me.gbLocation2.Controls.Add(Me.btnEveDir2)
        Me.gbLocation2.Controls.Add(Me.btnClear2)
        Me.gbLocation2.Location = New System.Drawing.Point(6, 141)
        Me.gbLocation2.Name = "gbLocation2"
        Me.gbLocation2.Size = New System.Drawing.Size(104, 100)
        Me.gbLocation2.TabIndex = 13
        Me.gbLocation2.TabStop = False
        Me.gbLocation2.Text = "Eve Location 2"
        '
        'lblFriendlyName2
        '
        Me.lblFriendlyName2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFriendlyName2.AutoSize = True
        Me.lblFriendlyName2.Location = New System.Drawing.Point(-208, 72)
        Me.lblFriendlyName2.Name = "lblFriendlyName2"
        Me.lblFriendlyName2.Size = New System.Drawing.Size(77, 13)
        Me.lblFriendlyName2.TabIndex = 11
        Me.lblFriendlyName2.Text = "Friendly Name:"
        '
        'txtFriendlyName2
        '
        Me.txtFriendlyName2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFriendlyName2.Location = New System.Drawing.Point(-125, 69)
        Me.txtFriendlyName2.Name = "txtFriendlyName2"
        Me.txtFriendlyName2.Size = New System.Drawing.Size(150, 20)
        Me.txtFriendlyName2.TabIndex = 10
        '
        'lblCacheSize2
        '
        Me.lblCacheSize2.AutoSize = True
        Me.lblCacheSize2.Location = New System.Drawing.Point(87, 70)
        Me.lblCacheSize2.Name = "lblCacheSize2"
        Me.lblCacheSize2.Size = New System.Drawing.Size(64, 13)
        Me.lblCacheSize2.TabIndex = 9
        Me.lblCacheSize2.Text = "Cache Size:"
        Me.lblCacheSize2.Visible = False
        '
        'chkLUA2
        '
        Me.chkLUA2.AutoSize = True
        Me.chkLUA2.Location = New System.Drawing.Point(6, 69)
        Me.chkLUA2.Name = "chkLUA2"
        Me.chkLUA2.Size = New System.Drawing.Size(75, 17)
        Me.chkLUA2.TabIndex = 8
        Me.chkLUA2.Text = "/LUA Off?"
        Me.chkLUA2.UseVisualStyleBackColor = True
        '
        'lblEveDir2
        '
        Me.lblEveDir2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEveDir2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEveDir2.Location = New System.Drawing.Point(6, 16)
        Me.lblEveDir2.Name = "lblEveDir2"
        Me.lblEveDir2.Padding = New System.Windows.Forms.Padding(2)
        Me.lblEveDir2.Size = New System.Drawing.Size(19, 50)
        Me.lblEveDir2.TabIndex = 3
        Me.lblEveDir2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnEveDir2
        '
        Me.btnEveDir2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEveDir2.Location = New System.Drawing.Point(34, 16)
        Me.btnEveDir2.Name = "btnEveDir2"
        Me.btnEveDir2.Size = New System.Drawing.Size(64, 22)
        Me.btnEveDir2.TabIndex = 6
        Me.btnEveDir2.Text = "Change..."
        Me.btnEveDir2.UseVisualStyleBackColor = True
        '
        'btnClear2
        '
        Me.btnClear2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear2.Location = New System.Drawing.Point(34, 44)
        Me.btnClear2.Name = "btnClear2"
        Me.btnClear2.Size = New System.Drawing.Size(64, 22)
        Me.btnClear2.TabIndex = 7
        Me.btnClear2.Text = "Clear"
        Me.btnClear2.UseVisualStyleBackColor = True
        '
        'gbLocation1
        '
        Me.gbLocation1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbLocation1.Controls.Add(Me.lblFriendlyName1)
        Me.gbLocation1.Controls.Add(Me.txtFriendlyName1)
        Me.gbLocation1.Controls.Add(Me.lblCacheSize1)
        Me.gbLocation1.Controls.Add(Me.chkLUA1)
        Me.gbLocation1.Controls.Add(Me.lblEveDir1)
        Me.gbLocation1.Controls.Add(Me.btnEveDir1)
        Me.gbLocation1.Controls.Add(Me.btnClear1)
        Me.gbLocation1.Location = New System.Drawing.Point(6, 35)
        Me.gbLocation1.Name = "gbLocation1"
        Me.gbLocation1.Size = New System.Drawing.Size(104, 100)
        Me.gbLocation1.TabIndex = 12
        Me.gbLocation1.TabStop = False
        Me.gbLocation1.Text = "Eve Location 1"
        '
        'lblFriendlyName1
        '
        Me.lblFriendlyName1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFriendlyName1.AutoSize = True
        Me.lblFriendlyName1.Location = New System.Drawing.Point(-208, 72)
        Me.lblFriendlyName1.Name = "lblFriendlyName1"
        Me.lblFriendlyName1.Size = New System.Drawing.Size(77, 13)
        Me.lblFriendlyName1.TabIndex = 9
        Me.lblFriendlyName1.Text = "Friendly Name:"
        '
        'txtFriendlyName1
        '
        Me.txtFriendlyName1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFriendlyName1.Location = New System.Drawing.Point(-125, 69)
        Me.txtFriendlyName1.Name = "txtFriendlyName1"
        Me.txtFriendlyName1.Size = New System.Drawing.Size(150, 20)
        Me.txtFriendlyName1.TabIndex = 8
        '
        'lblCacheSize1
        '
        Me.lblCacheSize1.AutoSize = True
        Me.lblCacheSize1.Location = New System.Drawing.Point(87, 70)
        Me.lblCacheSize1.Name = "lblCacheSize1"
        Me.lblCacheSize1.Size = New System.Drawing.Size(64, 13)
        Me.lblCacheSize1.TabIndex = 7
        Me.lblCacheSize1.Text = "Cache Size:"
        Me.lblCacheSize1.Visible = False
        '
        'chkLUA1
        '
        Me.chkLUA1.AutoSize = True
        Me.chkLUA1.Location = New System.Drawing.Point(6, 69)
        Me.chkLUA1.Name = "chkLUA1"
        Me.chkLUA1.Size = New System.Drawing.Size(75, 17)
        Me.chkLUA1.TabIndex = 6
        Me.chkLUA1.Text = "/LUA Off?"
        Me.chkLUA1.UseVisualStyleBackColor = True
        '
        'lblEveDir1
        '
        Me.lblEveDir1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEveDir1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEveDir1.Location = New System.Drawing.Point(6, 16)
        Me.lblEveDir1.Name = "lblEveDir1"
        Me.lblEveDir1.Padding = New System.Windows.Forms.Padding(2)
        Me.lblEveDir1.Size = New System.Drawing.Size(19, 50)
        Me.lblEveDir1.TabIndex = 0
        Me.lblEveDir1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnEveDir1
        '
        Me.btnEveDir1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEveDir1.Location = New System.Drawing.Point(34, 16)
        Me.btnEveDir1.Name = "btnEveDir1"
        Me.btnEveDir1.Size = New System.Drawing.Size(64, 22)
        Me.btnEveDir1.TabIndex = 4
        Me.btnEveDir1.Text = "Change..."
        Me.btnEveDir1.UseVisualStyleBackColor = True
        '
        'btnClear1
        '
        Me.btnClear1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear1.Location = New System.Drawing.Point(34, 44)
        Me.btnClear1.Name = "btnClear1"
        Me.btnClear1.Size = New System.Drawing.Size(64, 22)
        Me.btnClear1.TabIndex = 5
        Me.btnClear1.Text = "Clear"
        Me.btnClear1.UseVisualStyleBackColor = True
        '
        'gbTrainingQueue
        '
        Me.gbTrainingQueue.Controls.Add(Me.chkOmitCurrentSkill)
        Me.gbTrainingQueue.Controls.Add(Me.pbPartiallyTrainedColour)
        Me.gbTrainingQueue.Controls.Add(Me.lblPartiallyTrainedColour)
        Me.gbTrainingQueue.Controls.Add(Me.chkDeleteCompletedSkills)
        Me.gbTrainingQueue.Controls.Add(Me.pbReadySkillColour)
        Me.gbTrainingQueue.Controls.Add(Me.lblReadySkillColour)
        Me.gbTrainingQueue.Controls.Add(Me.pbDowntimeClashColour)
        Me.gbTrainingQueue.Controls.Add(Me.lblDowntimeClashColour)
        Me.gbTrainingQueue.Controls.Add(Me.pbBothPreReqColour)
        Me.gbTrainingQueue.Controls.Add(Me.lblBothPreReqColour)
        Me.gbTrainingQueue.Controls.Add(Me.pbHasPreReqColour)
        Me.gbTrainingQueue.Controls.Add(Me.pbIsPreReqColour)
        Me.gbTrainingQueue.Controls.Add(Me.lblHasPreReqColour)
        Me.gbTrainingQueue.Controls.Add(Me.lblIsPreReqColour)
        Me.gbTrainingQueue.Controls.Add(Me.lblSkillQueueColours)
        Me.gbTrainingQueue.Controls.Add(Me.chkContinueTraining)
        Me.gbTrainingQueue.Controls.Add(Me.lblQueueColumns)
        Me.gbTrainingQueue.Controls.Add(Me.clbColumns)
        Me.gbTrainingQueue.Location = New System.Drawing.Point(762, 332)
        Me.gbTrainingQueue.Name = "gbTrainingQueue"
        Me.gbTrainingQueue.Size = New System.Drawing.Size(70, 51)
        Me.gbTrainingQueue.TabIndex = 3
        Me.gbTrainingQueue.TabStop = False
        Me.gbTrainingQueue.Text = "Training Queue"
        Me.gbTrainingQueue.Visible = False
        '
        'chkOmitCurrentSkill
        '
        Me.chkOmitCurrentSkill.AutoSize = True
        Me.chkOmitCurrentSkill.Location = New System.Drawing.Point(9, 406)
        Me.chkOmitCurrentSkill.Name = "chkOmitCurrentSkill"
        Me.chkOmitCurrentSkill.Size = New System.Drawing.Size(196, 17)
        Me.chkOmitCurrentSkill.TabIndex = 33
        Me.chkOmitCurrentSkill.Text = "Omit current skill from training queue"
        Me.chkOmitCurrentSkill.UseVisualStyleBackColor = True
        '
        'pbPartiallyTrainedColour
        '
        Me.pbPartiallyTrainedColour.BackColor = System.Drawing.Color.White
        Me.pbPartiallyTrainedColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPartiallyTrainedColour.Location = New System.Drawing.Point(350, 205)
        Me.pbPartiallyTrainedColour.Name = "pbPartiallyTrainedColour"
        Me.pbPartiallyTrainedColour.Size = New System.Drawing.Size(24, 24)
        Me.pbPartiallyTrainedColour.TabIndex = 32
        Me.pbPartiallyTrainedColour.TabStop = False
        '
        'lblPartiallyTrainedColour
        '
        Me.lblPartiallyTrainedColour.AutoSize = True
        Me.lblPartiallyTrainedColour.Location = New System.Drawing.Point(223, 212)
        Me.lblPartiallyTrainedColour.Name = "lblPartiallyTrainedColour"
        Me.lblPartiallyTrainedColour.Size = New System.Drawing.Size(82, 13)
        Me.lblPartiallyTrainedColour.TabIndex = 31
        Me.lblPartiallyTrainedColour.Text = "Partially Trained"
        '
        'chkDeleteCompletedSkills
        '
        Me.chkDeleteCompletedSkills.AutoSize = True
        Me.chkDeleteCompletedSkills.Location = New System.Drawing.Point(9, 383)
        Me.chkDeleteCompletedSkills.Name = "chkDeleteCompletedSkills"
        Me.chkDeleteCompletedSkills.Size = New System.Drawing.Size(258, 17)
        Me.chkDeleteCompletedSkills.TabIndex = 30
        Me.chkDeleteCompletedSkills.Text = "Automatically delete completed skills from queues"
        Me.chkDeleteCompletedSkills.UseVisualStyleBackColor = True
        '
        'pbReadySkillColour
        '
        Me.pbReadySkillColour.BackColor = System.Drawing.Color.White
        Me.pbReadySkillColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbReadySkillColour.Location = New System.Drawing.Point(350, 55)
        Me.pbReadySkillColour.Name = "pbReadySkillColour"
        Me.pbReadySkillColour.Size = New System.Drawing.Size(24, 24)
        Me.pbReadySkillColour.TabIndex = 29
        Me.pbReadySkillColour.TabStop = False
        '
        'lblReadySkillColour
        '
        Me.lblReadySkillColour.AutoSize = True
        Me.lblReadySkillColour.Location = New System.Drawing.Point(223, 62)
        Me.lblReadySkillColour.Name = "lblReadySkillColour"
        Me.lblReadySkillColour.Size = New System.Drawing.Size(89, 13)
        Me.lblReadySkillColour.TabIndex = 28
        Me.lblReadySkillColour.Text = "Independent Skill"
        '
        'pbDowntimeClashColour
        '
        Me.pbDowntimeClashColour.BackColor = System.Drawing.Color.Red
        Me.pbDowntimeClashColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbDowntimeClashColour.Location = New System.Drawing.Point(350, 175)
        Me.pbDowntimeClashColour.Name = "pbDowntimeClashColour"
        Me.pbDowntimeClashColour.Size = New System.Drawing.Size(24, 24)
        Me.pbDowntimeClashColour.TabIndex = 27
        Me.pbDowntimeClashColour.TabStop = False
        '
        'lblDowntimeClashColour
        '
        Me.lblDowntimeClashColour.AutoSize = True
        Me.lblDowntimeClashColour.Location = New System.Drawing.Point(223, 182)
        Me.lblDowntimeClashColour.Name = "lblDowntimeClashColour"
        Me.lblDowntimeClashColour.Size = New System.Drawing.Size(107, 13)
        Me.lblDowntimeClashColour.TabIndex = 26
        Me.lblDowntimeClashColour.Text = "Downtime Clash Text"
        '
        'pbBothPreReqColour
        '
        Me.pbBothPreReqColour.BackColor = System.Drawing.Color.Gold
        Me.pbBothPreReqColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbBothPreReqColour.Location = New System.Drawing.Point(350, 145)
        Me.pbBothPreReqColour.Name = "pbBothPreReqColour"
        Me.pbBothPreReqColour.Size = New System.Drawing.Size(24, 24)
        Me.pbBothPreReqColour.TabIndex = 25
        Me.pbBothPreReqColour.TabStop = False
        '
        'lblBothPreReqColour
        '
        Me.lblBothPreReqColour.AutoSize = True
        Me.lblBothPreReqColour.Location = New System.Drawing.Point(223, 152)
        Me.lblBothPreReqColour.Name = "lblBothPreReqColour"
        Me.lblBothPreReqColour.Size = New System.Drawing.Size(62, 13)
        Me.lblBothPreReqColour.TabIndex = 24
        Me.lblBothPreReqColour.Text = "Skill Is Both"
        '
        'pbHasPreReqColour
        '
        Me.pbHasPreReqColour.BackColor = System.Drawing.Color.Plum
        Me.pbHasPreReqColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbHasPreReqColour.Location = New System.Drawing.Point(350, 115)
        Me.pbHasPreReqColour.Name = "pbHasPreReqColour"
        Me.pbHasPreReqColour.Size = New System.Drawing.Size(24, 24)
        Me.pbHasPreReqColour.TabIndex = 23
        Me.pbHasPreReqColour.TabStop = False
        '
        'pbIsPreReqColour
        '
        Me.pbIsPreReqColour.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbIsPreReqColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbIsPreReqColour.Location = New System.Drawing.Point(350, 85)
        Me.pbIsPreReqColour.Name = "pbIsPreReqColour"
        Me.pbIsPreReqColour.Size = New System.Drawing.Size(24, 24)
        Me.pbIsPreReqColour.TabIndex = 22
        Me.pbIsPreReqColour.TabStop = False
        '
        'lblHasPreReqColour
        '
        Me.lblHasPreReqColour.AutoSize = True
        Me.lblHasPreReqColour.Location = New System.Drawing.Point(223, 122)
        Me.lblHasPreReqColour.Name = "lblHasPreReqColour"
        Me.lblHasPreReqColour.Size = New System.Drawing.Size(90, 13)
        Me.lblHasPreReqColour.TabIndex = 21
        Me.lblHasPreReqColour.Text = "Skill HAS PreReq"
        '
        'lblIsPreReqColour
        '
        Me.lblIsPreReqColour.AutoSize = True
        Me.lblIsPreReqColour.Location = New System.Drawing.Point(223, 92)
        Me.lblIsPreReqColour.Name = "lblIsPreReqColour"
        Me.lblIsPreReqColour.Size = New System.Drawing.Size(78, 13)
        Me.lblIsPreReqColour.TabIndex = 20
        Me.lblIsPreReqColour.Text = "Skill IS PreReq"
        '
        'lblSkillQueueColours
        '
        Me.lblSkillQueueColours.AutoSize = True
        Me.lblSkillQueueColours.Location = New System.Drawing.Point(208, 25)
        Me.lblSkillQueueColours.Name = "lblSkillQueueColours"
        Me.lblSkillQueueColours.Size = New System.Drawing.Size(102, 13)
        Me.lblSkillQueueColours.TabIndex = 4
        Me.lblSkillQueueColours.Text = "Skill Queue Colours:"
        '
        'chkContinueTraining
        '
        Me.chkContinueTraining.AutoSize = True
        Me.chkContinueTraining.Location = New System.Drawing.Point(9, 359)
        Me.chkContinueTraining.Name = "chkContinueTraining"
        Me.chkContinueTraining.Size = New System.Drawing.Size(247, 17)
        Me.chkContinueTraining.TabIndex = 3
        Me.chkContinueTraining.Text = "Continue training skill queue on skill completion"
        Me.chkContinueTraining.UseVisualStyleBackColor = True
        '
        'lblQueueColumns
        '
        Me.lblQueueColumns.AutoSize = True
        Me.lblQueueColumns.Location = New System.Drawing.Point(6, 25)
        Me.lblQueueColumns.Name = "lblQueueColumns"
        Me.lblQueueColumns.Size = New System.Drawing.Size(127, 13)
        Me.lblQueueColumns.TabIndex = 1
        Me.lblQueueColumns.Text = "Queue Column Selection:"
        '
        'clbColumns
        '
        Me.clbColumns.CheckOnClick = True
        Me.clbColumns.FormattingEnabled = True
        Me.clbColumns.Items.AddRange(New Object() {"Skill Name", "Current Level", "From Level", "To Level", "% Complete", "Training Time", "Date Completed", "Rank", "Primary Attribute", "Secondary Attribute", "SP Rate/Hour", "SP Rate/Day", "SP Rate/Week", "SP Rate/Month", "SP Rate/Year", "SP Earned", "SP Total"})
        Me.clbColumns.Location = New System.Drawing.Point(9, 47)
        Me.clbColumns.Name = "clbColumns"
        Me.clbColumns.Size = New System.Drawing.Size(157, 259)
        Me.clbColumns.TabIndex = 2
        '
        'gbDatabaseFormat
        '
        Me.gbDatabaseFormat.Controls.Add(Me.gbAccess)
        Me.gbDatabaseFormat.Controls.Add(Me.gbMySQL)
        Me.gbDatabaseFormat.Controls.Add(Me.btnTestDB)
        Me.gbDatabaseFormat.Controls.Add(Me.gbMSSQL)
        Me.gbDatabaseFormat.Controls.Add(Me.cboFormat)
        Me.gbDatabaseFormat.Controls.Add(Me.lblFormat)
        Me.gbDatabaseFormat.Location = New System.Drawing.Point(619, 234)
        Me.gbDatabaseFormat.Name = "gbDatabaseFormat"
        Me.gbDatabaseFormat.Size = New System.Drawing.Size(41, 34)
        Me.gbDatabaseFormat.TabIndex = 18
        Me.gbDatabaseFormat.TabStop = False
        Me.gbDatabaseFormat.Text = "Database Format"
        Me.gbDatabaseFormat.Visible = False
        '
        'gbAccess
        '
        Me.gbAccess.Controls.Add(Me.chkUseAppDirForDB)
        Me.gbAccess.Controls.Add(Me.btnBrowseMDB)
        Me.gbAccess.Controls.Add(Me.txtMDBPassword)
        Me.gbAccess.Controls.Add(Me.txtMDBUsername)
        Me.gbAccess.Controls.Add(Me.txtMDBServer)
        Me.gbAccess.Controls.Add(Me.lblMDBPassword)
        Me.gbAccess.Controls.Add(Me.lblMDBUser)
        Me.gbAccess.Controls.Add(Me.lblMDBFilename)
        Me.gbAccess.Location = New System.Drawing.Point(6, 80)
        Me.gbAccess.Name = "gbAccess"
        Me.gbAccess.Size = New System.Drawing.Size(513, 170)
        Me.gbAccess.TabIndex = 37
        Me.gbAccess.TabStop = False
        Me.gbAccess.Text = "Access (MDB) Options"
        '
        'chkUseAppDirForDB
        '
        Me.chkUseAppDirForDB.AutoSize = True
        Me.chkUseAppDirForDB.Location = New System.Drawing.Point(73, 142)
        Me.chkUseAppDirForDB.Name = "chkUseAppDirForDB"
        Me.chkUseAppDirForDB.Size = New System.Drawing.Size(247, 17)
        Me.chkUseAppDirForDB.TabIndex = 7
        Me.chkUseAppDirForDB.Text = "Use EveHQ Application Directory for Database"
        Me.chkUseAppDirForDB.UseVisualStyleBackColor = True
        '
        'btnBrowseMDB
        '
        Me.btnBrowseMDB.Location = New System.Drawing.Point(7, 46)
        Me.btnBrowseMDB.Name = "btnBrowseMDB"
        Me.btnBrowseMDB.Size = New System.Drawing.Size(51, 23)
        Me.btnBrowseMDB.TabIndex = 6
        Me.btnBrowseMDB.Text = "Browse"
        Me.btnBrowseMDB.UseVisualStyleBackColor = True
        '
        'txtMDBPassword
        '
        Me.txtMDBPassword.Location = New System.Drawing.Point(73, 115)
        Me.txtMDBPassword.Name = "txtMDBPassword"
        Me.txtMDBPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMDBPassword.Size = New System.Drawing.Size(230, 20)
        Me.txtMDBPassword.TabIndex = 5
        '
        'txtMDBUsername
        '
        Me.txtMDBUsername.Location = New System.Drawing.Point(73, 89)
        Me.txtMDBUsername.Name = "txtMDBUsername"
        Me.txtMDBUsername.Size = New System.Drawing.Size(230, 20)
        Me.txtMDBUsername.TabIndex = 4
        '
        'txtMDBServer
        '
        Me.txtMDBServer.Location = New System.Drawing.Point(73, 27)
        Me.txtMDBServer.Multiline = True
        Me.txtMDBServer.Name = "txtMDBServer"
        Me.txtMDBServer.Size = New System.Drawing.Size(324, 56)
        Me.txtMDBServer.TabIndex = 3
        '
        'lblMDBPassword
        '
        Me.lblMDBPassword.AutoSize = True
        Me.lblMDBPassword.Location = New System.Drawing.Point(6, 118)
        Me.lblMDBPassword.Name = "lblMDBPassword"
        Me.lblMDBPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblMDBPassword.TabIndex = 2
        Me.lblMDBPassword.Text = "Password:"
        '
        'lblMDBUser
        '
        Me.lblMDBUser.AutoSize = True
        Me.lblMDBUser.Location = New System.Drawing.Point(6, 92)
        Me.lblMDBUser.Name = "lblMDBUser"
        Me.lblMDBUser.Size = New System.Drawing.Size(32, 13)
        Me.lblMDBUser.TabIndex = 1
        Me.lblMDBUser.Text = "User:"
        '
        'lblMDBFilename
        '
        Me.lblMDBFilename.AutoSize = True
        Me.lblMDBFilename.Location = New System.Drawing.Point(6, 30)
        Me.lblMDBFilename.Name = "lblMDBFilename"
        Me.lblMDBFilename.Size = New System.Drawing.Size(52, 13)
        Me.lblMDBFilename.TabIndex = 0
        Me.lblMDBFilename.Text = "Filename:"
        '
        'gbMySQL
        '
        Me.gbMySQL.Controls.Add(Me.txtMySQLDatabase)
        Me.gbMySQL.Controls.Add(Me.lblMySQLDatabase)
        Me.gbMySQL.Controls.Add(Me.txtMySQLPassword)
        Me.gbMySQL.Controls.Add(Me.txtMySQLUsername)
        Me.gbMySQL.Controls.Add(Me.txtMySQLServer)
        Me.gbMySQL.Controls.Add(Me.lblMySQLPassword)
        Me.gbMySQL.Controls.Add(Me.lblMySQLUser)
        Me.gbMySQL.Controls.Add(Me.lblMySQLServer)
        Me.gbMySQL.Location = New System.Drawing.Point(6, 151)
        Me.gbMySQL.Name = "gbMySQL"
        Me.gbMySQL.Size = New System.Drawing.Size(403, 63)
        Me.gbMySQL.TabIndex = 36
        Me.gbMySQL.TabStop = False
        Me.gbMySQL.Text = "MySQL Options"
        Me.gbMySQL.Visible = False
        '
        'txtMySQLDatabase
        '
        Me.txtMySQLDatabase.Location = New System.Drawing.Point(73, 53)
        Me.txtMySQLDatabase.Name = "txtMySQLDatabase"
        Me.txtMySQLDatabase.Size = New System.Drawing.Size(230, 20)
        Me.txtMySQLDatabase.TabIndex = 7
        '
        'lblMySQLDatabase
        '
        Me.lblMySQLDatabase.AutoSize = True
        Me.lblMySQLDatabase.Location = New System.Drawing.Point(6, 56)
        Me.lblMySQLDatabase.Name = "lblMySQLDatabase"
        Me.lblMySQLDatabase.Size = New System.Drawing.Size(56, 13)
        Me.lblMySQLDatabase.TabIndex = 6
        Me.lblMySQLDatabase.Text = "Database:"
        '
        'txtMySQLPassword
        '
        Me.txtMySQLPassword.Location = New System.Drawing.Point(73, 105)
        Me.txtMySQLPassword.Name = "txtMySQLPassword"
        Me.txtMySQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMySQLPassword.Size = New System.Drawing.Size(230, 20)
        Me.txtMySQLPassword.TabIndex = 5
        '
        'txtMySQLUsername
        '
        Me.txtMySQLUsername.Location = New System.Drawing.Point(73, 79)
        Me.txtMySQLUsername.Name = "txtMySQLUsername"
        Me.txtMySQLUsername.Size = New System.Drawing.Size(230, 20)
        Me.txtMySQLUsername.TabIndex = 4
        '
        'txtMySQLServer
        '
        Me.txtMySQLServer.Location = New System.Drawing.Point(73, 27)
        Me.txtMySQLServer.Name = "txtMySQLServer"
        Me.txtMySQLServer.Size = New System.Drawing.Size(230, 20)
        Me.txtMySQLServer.TabIndex = 3
        '
        'lblMySQLPassword
        '
        Me.lblMySQLPassword.AutoSize = True
        Me.lblMySQLPassword.Location = New System.Drawing.Point(6, 108)
        Me.lblMySQLPassword.Name = "lblMySQLPassword"
        Me.lblMySQLPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblMySQLPassword.TabIndex = 2
        Me.lblMySQLPassword.Text = "Password:"
        '
        'lblMySQLUser
        '
        Me.lblMySQLUser.AutoSize = True
        Me.lblMySQLUser.Location = New System.Drawing.Point(6, 82)
        Me.lblMySQLUser.Name = "lblMySQLUser"
        Me.lblMySQLUser.Size = New System.Drawing.Size(32, 13)
        Me.lblMySQLUser.TabIndex = 1
        Me.lblMySQLUser.Text = "User:"
        '
        'lblMySQLServer
        '
        Me.lblMySQLServer.AutoSize = True
        Me.lblMySQLServer.Location = New System.Drawing.Point(6, 30)
        Me.lblMySQLServer.Name = "lblMySQLServer"
        Me.lblMySQLServer.Size = New System.Drawing.Size(41, 13)
        Me.lblMySQLServer.TabIndex = 0
        Me.lblMySQLServer.Text = "Server:"
        '
        'btnTestDB
        '
        Me.btnTestDB.Location = New System.Drawing.Point(6, 425)
        Me.btnTestDB.Name = "btnTestDB"
        Me.btnTestDB.Size = New System.Drawing.Size(162, 23)
        Me.btnTestDB.TabIndex = 38
        Me.btnTestDB.Text = "Test Database Connection"
        Me.btnTestDB.UseVisualStyleBackColor = True
        '
        'gbMSSQL
        '
        Me.gbMSSQL.Controls.Add(Me.txtMSSQLDatabase)
        Me.gbMSSQL.Controls.Add(Me.lblMSSQLDatabase)
        Me.gbMSSQL.Controls.Add(Me.lblMSSQLSecurity)
        Me.gbMSSQL.Controls.Add(Me.radMSSQLDatabase)
        Me.gbMSSQL.Controls.Add(Me.radMSSQLWindows)
        Me.gbMSSQL.Controls.Add(Me.txtMSSQLPassword)
        Me.gbMSSQL.Controls.Add(Me.txtMSSQLUsername)
        Me.gbMSSQL.Controls.Add(Me.txtMSSQLServer)
        Me.gbMSSQL.Controls.Add(Me.lblMSSQLPassword)
        Me.gbMSSQL.Controls.Add(Me.lblMSSQLUser)
        Me.gbMSSQL.Controls.Add(Me.lblMSSQLServer)
        Me.gbMSSQL.Location = New System.Drawing.Point(6, 233)
        Me.gbMSSQL.Name = "gbMSSQL"
        Me.gbMSSQL.Size = New System.Drawing.Size(403, 177)
        Me.gbMSSQL.TabIndex = 35
        Me.gbMSSQL.TabStop = False
        Me.gbMSSQL.Text = "MS SQL Options"
        Me.gbMSSQL.Visible = False
        '
        'txtMSSQLDatabase
        '
        Me.txtMSSQLDatabase.Location = New System.Drawing.Point(73, 79)
        Me.txtMSSQLDatabase.Name = "txtMSSQLDatabase"
        Me.txtMSSQLDatabase.Size = New System.Drawing.Size(230, 20)
        Me.txtMSSQLDatabase.TabIndex = 10
        '
        'lblMSSQLDatabase
        '
        Me.lblMSSQLDatabase.AutoSize = True
        Me.lblMSSQLDatabase.Location = New System.Drawing.Point(6, 82)
        Me.lblMSSQLDatabase.Name = "lblMSSQLDatabase"
        Me.lblMSSQLDatabase.Size = New System.Drawing.Size(56, 13)
        Me.lblMSSQLDatabase.TabIndex = 9
        Me.lblMSSQLDatabase.Text = "Database:"
        '
        'lblMSSQLSecurity
        '
        Me.lblMSSQLSecurity.AutoSize = True
        Me.lblMSSQLSecurity.Location = New System.Drawing.Point(6, 32)
        Me.lblMSSQLSecurity.Name = "lblMSSQLSecurity"
        Me.lblMSSQLSecurity.Size = New System.Drawing.Size(45, 13)
        Me.lblMSSQLSecurity.TabIndex = 8
        Me.lblMSSQLSecurity.Text = "Security"
        '
        'radMSSQLDatabase
        '
        Me.radMSSQLDatabase.AutoSize = True
        Me.radMSSQLDatabase.Location = New System.Drawing.Point(73, 30)
        Me.radMSSQLDatabase.Name = "radMSSQLDatabase"
        Me.radMSSQLDatabase.Size = New System.Drawing.Size(46, 17)
        Me.radMSSQLDatabase.TabIndex = 7
        Me.radMSSQLDatabase.Text = "SQL"
        Me.radMSSQLDatabase.UseVisualStyleBackColor = True
        '
        'radMSSQLWindows
        '
        Me.radMSSQLWindows.AutoSize = True
        Me.radMSSQLWindows.Location = New System.Drawing.Point(139, 30)
        Me.radMSSQLWindows.Name = "radMSSQLWindows"
        Me.radMSSQLWindows.Size = New System.Drawing.Size(69, 17)
        Me.radMSSQLWindows.TabIndex = 6
        Me.radMSSQLWindows.Text = "Windows"
        Me.radMSSQLWindows.UseVisualStyleBackColor = True
        '
        'txtMSSQLPassword
        '
        Me.txtMSSQLPassword.Location = New System.Drawing.Point(73, 131)
        Me.txtMSSQLPassword.Name = "txtMSSQLPassword"
        Me.txtMSSQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMSSQLPassword.Size = New System.Drawing.Size(230, 20)
        Me.txtMSSQLPassword.TabIndex = 5
        '
        'txtMSSQLUsername
        '
        Me.txtMSSQLUsername.Location = New System.Drawing.Point(73, 105)
        Me.txtMSSQLUsername.Name = "txtMSSQLUsername"
        Me.txtMSSQLUsername.Size = New System.Drawing.Size(230, 20)
        Me.txtMSSQLUsername.TabIndex = 4
        '
        'txtMSSQLServer
        '
        Me.txtMSSQLServer.Location = New System.Drawing.Point(73, 53)
        Me.txtMSSQLServer.Name = "txtMSSQLServer"
        Me.txtMSSQLServer.Size = New System.Drawing.Size(230, 20)
        Me.txtMSSQLServer.TabIndex = 3
        '
        'lblMSSQLPassword
        '
        Me.lblMSSQLPassword.AutoSize = True
        Me.lblMSSQLPassword.Location = New System.Drawing.Point(6, 134)
        Me.lblMSSQLPassword.Name = "lblMSSQLPassword"
        Me.lblMSSQLPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblMSSQLPassword.TabIndex = 2
        Me.lblMSSQLPassword.Text = "Password:"
        '
        'lblMSSQLUser
        '
        Me.lblMSSQLUser.AutoSize = True
        Me.lblMSSQLUser.Location = New System.Drawing.Point(6, 108)
        Me.lblMSSQLUser.Name = "lblMSSQLUser"
        Me.lblMSSQLUser.Size = New System.Drawing.Size(32, 13)
        Me.lblMSSQLUser.TabIndex = 1
        Me.lblMSSQLUser.Text = "User:"
        '
        'lblMSSQLServer
        '
        Me.lblMSSQLServer.AutoSize = True
        Me.lblMSSQLServer.Location = New System.Drawing.Point(6, 56)
        Me.lblMSSQLServer.Name = "lblMSSQLServer"
        Me.lblMSSQLServer.Size = New System.Drawing.Size(41, 13)
        Me.lblMSSQLServer.TabIndex = 0
        Me.lblMSSQLServer.Text = "Server:"
        '
        'cboFormat
        '
        Me.cboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFormat.FormattingEnabled = True
        Me.cboFormat.Items.AddRange(New Object() {"Access Database (.MDB)", "MS SQL Server", "MS SQL 2005 Express", "MySQL 5.0"})
        Me.cboFormat.Location = New System.Drawing.Point(103, 45)
        Me.cboFormat.Name = "cboFormat"
        Me.cboFormat.Size = New System.Drawing.Size(309, 21)
        Me.cboFormat.TabIndex = 34
        '
        'lblFormat
        '
        Me.lblFormat.AutoSize = True
        Me.lblFormat.Location = New System.Drawing.Point(6, 48)
        Me.lblFormat.Name = "lblFormat"
        Me.lblFormat.Size = New System.Drawing.Size(91, 13)
        Me.lblFormat.TabIndex = 33
        Me.lblFormat.Text = "Database Format:"
        '
        'gbProxyServer
        '
        Me.gbProxyServer.Controls.Add(Me.gbProxyServerInfo)
        Me.gbProxyServer.Controls.Add(Me.chkUseProxy)
        Me.gbProxyServer.Location = New System.Drawing.Point(490, 335)
        Me.gbProxyServer.Name = "gbProxyServer"
        Me.gbProxyServer.Size = New System.Drawing.Size(90, 30)
        Me.gbProxyServer.TabIndex = 2
        Me.gbProxyServer.TabStop = False
        Me.gbProxyServer.Text = "Proxy Server"
        Me.gbProxyServer.Visible = False
        '
        'gbProxyServerInfo
        '
        Me.gbProxyServerInfo.Controls.Add(Me.lblProxyPassword)
        Me.gbProxyServerInfo.Controls.Add(Me.lblProxyUsername)
        Me.gbProxyServerInfo.Controls.Add(Me.txtProxyPassword)
        Me.gbProxyServerInfo.Controls.Add(Me.txtProxyUsername)
        Me.gbProxyServerInfo.Controls.Add(Me.radUseSpecifiedCreds)
        Me.gbProxyServerInfo.Controls.Add(Me.lblProxyServer)
        Me.gbProxyServerInfo.Controls.Add(Me.txtProxyServer)
        Me.gbProxyServerInfo.Controls.Add(Me.radUseDefaultCreds)
        Me.gbProxyServerInfo.Location = New System.Drawing.Point(29, 68)
        Me.gbProxyServerInfo.Name = "gbProxyServerInfo"
        Me.gbProxyServerInfo.Size = New System.Drawing.Size(349, 187)
        Me.gbProxyServerInfo.TabIndex = 1
        Me.gbProxyServerInfo.TabStop = False
        Me.gbProxyServerInfo.Text = "Proxy Server Information"
        Me.gbProxyServerInfo.Visible = False
        '
        'lblProxyPassword
        '
        Me.lblProxyPassword.AutoSize = True
        Me.lblProxyPassword.Enabled = False
        Me.lblProxyPassword.Location = New System.Drawing.Point(38, 145)
        Me.lblProxyPassword.Name = "lblProxyPassword"
        Me.lblProxyPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblProxyPassword.TabIndex = 9
        Me.lblProxyPassword.Text = "Password:"
        '
        'lblProxyUsername
        '
        Me.lblProxyUsername.AutoSize = True
        Me.lblProxyUsername.Enabled = False
        Me.lblProxyUsername.Location = New System.Drawing.Point(36, 119)
        Me.lblProxyUsername.Name = "lblProxyUsername"
        Me.lblProxyUsername.Size = New System.Drawing.Size(58, 13)
        Me.lblProxyUsername.TabIndex = 8
        Me.lblProxyUsername.Text = "Username:"
        '
        'txtProxyPassword
        '
        Me.txtProxyPassword.Enabled = False
        Me.txtProxyPassword.Location = New System.Drawing.Point(100, 142)
        Me.txtProxyPassword.Name = "txtProxyPassword"
        Me.txtProxyPassword.Size = New System.Drawing.Size(234, 20)
        Me.txtProxyPassword.TabIndex = 7
        '
        'txtProxyUsername
        '
        Me.txtProxyUsername.Enabled = False
        Me.txtProxyUsername.Location = New System.Drawing.Point(100, 116)
        Me.txtProxyUsername.Name = "txtProxyUsername"
        Me.txtProxyUsername.Size = New System.Drawing.Size(234, 20)
        Me.txtProxyUsername.TabIndex = 6
        '
        'radUseSpecifiedCreds
        '
        Me.radUseSpecifiedCreds.AutoSize = True
        Me.radUseSpecifiedCreds.Location = New System.Drawing.Point(27, 93)
        Me.radUseSpecifiedCreds.Name = "radUseSpecifiedCreds"
        Me.radUseSpecifiedCreds.Size = New System.Drawing.Size(167, 17)
        Me.radUseSpecifiedCreds.TabIndex = 5
        Me.radUseSpecifiedCreds.Text = "Use the Following Credentials:"
        Me.radUseSpecifiedCreds.UseVisualStyleBackColor = True
        '
        'lblProxyServer
        '
        Me.lblProxyServer.AutoSize = True
        Me.lblProxyServer.Location = New System.Drawing.Point(24, 33)
        Me.lblProxyServer.Name = "lblProxyServer"
        Me.lblProxyServer.Size = New System.Drawing.Size(70, 13)
        Me.lblProxyServer.TabIndex = 3
        Me.lblProxyServer.Text = "Proxy Server:"
        '
        'txtProxyServer
        '
        Me.txtProxyServer.Location = New System.Drawing.Point(100, 30)
        Me.txtProxyServer.Name = "txtProxyServer"
        Me.txtProxyServer.Size = New System.Drawing.Size(234, 20)
        Me.txtProxyServer.TabIndex = 1
        '
        'radUseDefaultCreds
        '
        Me.radUseDefaultCreds.AutoSize = True
        Me.radUseDefaultCreds.Checked = True
        Me.radUseDefaultCreds.Location = New System.Drawing.Point(27, 70)
        Me.radUseDefaultCreds.Name = "radUseDefaultCreds"
        Me.radUseDefaultCreds.Size = New System.Drawing.Size(181, 17)
        Me.radUseDefaultCreds.TabIndex = 0
        Me.radUseDefaultCreds.TabStop = True
        Me.radUseDefaultCreds.Text = "Use Existing Network Credentials"
        Me.radUseDefaultCreds.UseVisualStyleBackColor = True
        '
        'chkUseProxy
        '
        Me.chkUseProxy.AutoSize = True
        Me.chkUseProxy.Location = New System.Drawing.Point(29, 31)
        Me.chkUseProxy.Name = "chkUseProxy"
        Me.chkUseProxy.Size = New System.Drawing.Size(108, 17)
        Me.chkUseProxy.TabIndex = 0
        Me.chkUseProxy.Text = "Use Proxy Server"
        Me.chkUseProxy.UseVisualStyleBackColor = True
        '
        'gbEveServer
        '
        Me.gbEveServer.Controls.Add(Me.chkShowAPIStatusForm)
        Me.gbEveServer.Controls.Add(Me.gbAPIServer)
        Me.gbEveServer.Controls.Add(Me.gbAPIRelayServer)
        Me.gbEveServer.Controls.Add(Me.chkEnableEveStatus)
        Me.gbEveServer.Controls.Add(Me.lblCurrentOffset)
        Me.gbEveServer.Controls.Add(Me.lblServerOffset)
        Me.gbEveServer.Controls.Add(Me.trackServerOffset)
        Me.gbEveServer.Location = New System.Drawing.Point(262, 316)
        Me.gbEveServer.Name = "gbEveServer"
        Me.gbEveServer.Size = New System.Drawing.Size(100, 35)
        Me.gbEveServer.TabIndex = 2
        Me.gbEveServer.TabStop = False
        Me.gbEveServer.Text = "Eve API && Server Options"
        Me.gbEveServer.Visible = False
        '
        'chkShowAPIStatusForm
        '
        Me.chkShowAPIStatusForm.AutoSize = True
        Me.chkShowAPIStatusForm.Location = New System.Drawing.Point(19, 171)
        Me.chkShowAPIStatusForm.Name = "chkShowAPIStatusForm"
        Me.chkShowAPIStatusForm.Size = New System.Drawing.Size(277, 17)
        Me.chkShowAPIStatusForm.TabIndex = 21
        Me.chkShowAPIStatusForm.Text = "Show API Status Form When Fetching Character API"
        Me.chkShowAPIStatusForm.UseVisualStyleBackColor = True
        '
        'gbAPIServer
        '
        Me.gbAPIServer.Controls.Add(Me.chkUseCCPBackup)
        Me.gbAPIServer.Controls.Add(Me.chkUseAPIRSServer)
        Me.gbAPIServer.Controls.Add(Me.txtAPIRSServer)
        Me.gbAPIServer.Controls.Add(Me.lblAPIRSServer)
        Me.gbAPIServer.Controls.Add(Me.txtCCPAPIServer)
        Me.gbAPIServer.Controls.Add(Me.lblCCPAPIServer)
        Me.gbAPIServer.Controls.Add(Me.chkAutoAPI)
        Me.gbAPIServer.Location = New System.Drawing.Point(6, 210)
        Me.gbAPIServer.Name = "gbAPIServer"
        Me.gbAPIServer.Size = New System.Drawing.Size(668, 132)
        Me.gbAPIServer.TabIndex = 20
        Me.gbAPIServer.TabStop = False
        Me.gbAPIServer.Text = "API Server"
        '
        'chkUseCCPBackup
        '
        Me.chkUseCCPBackup.AutoSize = True
        Me.chkUseCCPBackup.Location = New System.Drawing.Point(152, 52)
        Me.chkUseCCPBackup.Name = "chkUseCCPBackup"
        Me.chkUseCCPBackup.Size = New System.Drawing.Size(177, 17)
        Me.chkUseCCPBackup.TabIndex = 26
        Me.chkUseCCPBackup.Text = "Use CCP API Server as Backup"
        Me.chkUseCCPBackup.UseVisualStyleBackColor = True
        '
        'chkUseAPIRSServer
        '
        Me.chkUseAPIRSServer.AutoSize = True
        Me.chkUseAPIRSServer.Location = New System.Drawing.Point(12, 52)
        Me.chkUseAPIRSServer.Name = "chkUseAPIRSServer"
        Me.chkUseAPIRSServer.Size = New System.Drawing.Size(129, 17)
        Me.chkUseAPIRSServer.TabIndex = 25
        Me.chkUseAPIRSServer.Text = "Use API Relay Server"
        Me.chkUseAPIRSServer.UseVisualStyleBackColor = True
        '
        'txtAPIRSServer
        '
        Me.txtAPIRSServer.Location = New System.Drawing.Point(152, 75)
        Me.txtAPIRSServer.Name = "txtAPIRSServer"
        Me.txtAPIRSServer.Size = New System.Drawing.Size(332, 20)
        Me.txtAPIRSServer.TabIndex = 24
        '
        'lblAPIRSServer
        '
        Me.lblAPIRSServer.AutoSize = True
        Me.lblAPIRSServer.Location = New System.Drawing.Point(9, 78)
        Me.lblAPIRSServer.Name = "lblAPIRSServer"
        Me.lblAPIRSServer.Size = New System.Drawing.Size(137, 13)
        Me.lblAPIRSServer.TabIndex = 23
        Me.lblAPIRSServer.Text = "APIRS API Server Address:"
        '
        'txtCCPAPIServer
        '
        Me.txtCCPAPIServer.Location = New System.Drawing.Point(152, 26)
        Me.txtCCPAPIServer.Name = "txtCCPAPIServer"
        Me.txtCCPAPIServer.Size = New System.Drawing.Size(332, 20)
        Me.txtCCPAPIServer.TabIndex = 22
        '
        'lblCCPAPIServer
        '
        Me.lblCCPAPIServer.AutoSize = True
        Me.lblCCPAPIServer.Location = New System.Drawing.Point(9, 29)
        Me.lblCCPAPIServer.Name = "lblCCPAPIServer"
        Me.lblCCPAPIServer.Size = New System.Drawing.Size(126, 13)
        Me.lblCCPAPIServer.TabIndex = 21
        Me.lblCCPAPIServer.Text = "CCP API Server Address:"
        '
        'chkAutoAPI
        '
        Me.chkAutoAPI.AutoSize = True
        Me.chkAutoAPI.Location = New System.Drawing.Point(12, 101)
        Me.chkAutoAPI.Name = "chkAutoAPI"
        Me.chkAutoAPI.Size = New System.Drawing.Size(309, 17)
        Me.chkAutoAPI.TabIndex = 20
        Me.chkAutoAPI.Text = "Automatically Retrieve Character Data When Cache Expires"
        Me.chkAutoAPI.UseVisualStyleBackColor = True
        '
        'gbAPIRelayServer
        '
        Me.gbAPIRelayServer.Controls.Add(Me.chkAPIRSAutoStart)
        Me.gbAPIRelayServer.Controls.Add(Me.nudAPIRSPort)
        Me.gbAPIRelayServer.Controls.Add(Me.lblAPIRSPort)
        Me.gbAPIRelayServer.Controls.Add(Me.chkActivateAPIRS)
        Me.gbAPIRelayServer.Location = New System.Drawing.Point(6, 348)
        Me.gbAPIRelayServer.Name = "gbAPIRelayServer"
        Me.gbAPIRelayServer.Size = New System.Drawing.Size(668, 114)
        Me.gbAPIRelayServer.TabIndex = 15
        Me.gbAPIRelayServer.TabStop = False
        Me.gbAPIRelayServer.Text = "API Relay Server"
        '
        'chkAPIRSAutoStart
        '
        Me.chkAPIRSAutoStart.AutoSize = True
        Me.chkAPIRSAutoStart.Location = New System.Drawing.Point(12, 79)
        Me.chkAPIRSAutoStart.Name = "chkAPIRSAutoStart"
        Me.chkAPIRSAutoStart.Size = New System.Drawing.Size(182, 17)
        Me.chkAPIRSAutoStart.TabIndex = 5
        Me.chkAPIRSAutoStart.Text = "Run API Relay Server on Startup"
        Me.chkAPIRSAutoStart.UseVisualStyleBackColor = True
        '
        'nudAPIRSPort
        '
        Me.nudAPIRSPort.Location = New System.Drawing.Point(126, 53)
        Me.nudAPIRSPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.nudAPIRSPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudAPIRSPort.Name = "nudAPIRSPort"
        Me.nudAPIRSPort.Size = New System.Drawing.Size(90, 20)
        Me.nudAPIRSPort.TabIndex = 4
        Me.nudAPIRSPort.Value = New Decimal(New Integer() {26002, 0, 0, 0})
        '
        'lblAPIRSPort
        '
        Me.lblAPIRSPort.AutoSize = True
        Me.lblAPIRSPort.Location = New System.Drawing.Point(10, 55)
        Me.lblAPIRSPort.Name = "lblAPIRSPort"
        Me.lblAPIRSPort.Size = New System.Drawing.Size(110, 13)
        Me.lblAPIRSPort.TabIndex = 3
        Me.lblAPIRSPort.Text = "API Relay Server Port"
        '
        'chkActivateAPIRS
        '
        Me.chkActivateAPIRS.AutoSize = True
        Me.chkActivateAPIRS.Location = New System.Drawing.Point(13, 30)
        Me.chkActivateAPIRS.Name = "chkActivateAPIRS"
        Me.chkActivateAPIRS.Size = New System.Drawing.Size(149, 17)
        Me.chkActivateAPIRS.TabIndex = 0
        Me.chkActivateAPIRS.Text = "Activate API Relay Server"
        Me.chkActivateAPIRS.UseVisualStyleBackColor = True
        '
        'chkEnableEveStatus
        '
        Me.chkEnableEveStatus.AutoSize = True
        Me.chkEnableEveStatus.Location = New System.Drawing.Point(19, 41)
        Me.chkEnableEveStatus.Name = "chkEnableEveStatus"
        Me.chkEnableEveStatus.Size = New System.Drawing.Size(126, 17)
        Me.chkEnableEveStatus.TabIndex = 13
        Me.chkEnableEveStatus.Text = "Enable Server Status"
        Me.chkEnableEveStatus.UseVisualStyleBackColor = True
        '
        'lblCurrentOffset
        '
        Me.lblCurrentOffset.AutoSize = True
        Me.lblCurrentOffset.Location = New System.Drawing.Point(16, 138)
        Me.lblCurrentOffset.Name = "lblCurrentOffset"
        Me.lblCurrentOffset.Size = New System.Drawing.Size(75, 13)
        Me.lblCurrentOffset.TabIndex = 12
        Me.lblCurrentOffset.Text = "Current Offset:"
        '
        'lblServerOffset
        '
        Me.lblServerOffset.AutoSize = True
        Me.lblServerOffset.Location = New System.Drawing.Point(16, 74)
        Me.lblServerOffset.Name = "lblServerOffset"
        Me.lblServerOffset.Size = New System.Drawing.Size(147, 13)
        Me.lblServerOffset.TabIndex = 11
        Me.lblServerOffset.Text = "Tranquiity Server Time Offset:"
        '
        'trackServerOffset
        '
        Me.trackServerOffset.BackColor = System.Drawing.SystemColors.Control
        Me.trackServerOffset.Location = New System.Drawing.Point(19, 90)
        Me.trackServerOffset.Maximum = 600
        Me.trackServerOffset.Minimum = -600
        Me.trackServerOffset.Name = "trackServerOffset"
        Me.trackServerOffset.Size = New System.Drawing.Size(390, 45)
        Me.trackServerOffset.TabIndex = 10
        Me.trackServerOffset.TickFrequency = 30
        Me.trackServerOffset.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'gbPlugIns
        '
        Me.gbPlugIns.Controls.Add(Me.btnTidyPlugins)
        Me.gbPlugIns.Controls.Add(Me.btnRefreshPlugins)
        Me.gbPlugIns.Controls.Add(Me.lblPlugInInfo)
        Me.gbPlugIns.Controls.Add(Me.lblDetectedPlugIns)
        Me.gbPlugIns.Controls.Add(Me.lvwPlugins)
        Me.gbPlugIns.Location = New System.Drawing.Point(427, 106)
        Me.gbPlugIns.Name = "gbPlugIns"
        Me.gbPlugIns.Size = New System.Drawing.Size(70, 26)
        Me.gbPlugIns.TabIndex = 18
        Me.gbPlugIns.TabStop = False
        Me.gbPlugIns.Text = "EveHQ Plug-Ins"
        Me.gbPlugIns.Visible = False
        '
        'btnTidyPlugins
        '
        Me.btnTidyPlugins.Location = New System.Drawing.Point(257, 291)
        Me.btnTidyPlugins.Name = "btnTidyPlugins"
        Me.btnTidyPlugins.Size = New System.Drawing.Size(75, 23)
        Me.btnTidyPlugins.TabIndex = 4
        Me.btnTidyPlugins.Text = "Tidy"
        Me.btnTidyPlugins.UseVisualStyleBackColor = True
        '
        'btnRefreshPlugins
        '
        Me.btnRefreshPlugins.Location = New System.Drawing.Point(338, 291)
        Me.btnRefreshPlugins.Name = "btnRefreshPlugins"
        Me.btnRefreshPlugins.Size = New System.Drawing.Size(75, 23)
        Me.btnRefreshPlugins.TabIndex = 3
        Me.btnRefreshPlugins.Text = "Refresh"
        Me.btnRefreshPlugins.UseVisualStyleBackColor = True
        '
        'lblPlugInInfo
        '
        Me.lblPlugInInfo.AutoSize = True
        Me.lblPlugInInfo.Location = New System.Drawing.Point(6, 339)
        Me.lblPlugInInfo.Name = "lblPlugInInfo"
        Me.lblPlugInInfo.Size = New System.Drawing.Size(323, 13)
        Me.lblPlugInInfo.TabIndex = 2
        Me.lblPlugInInfo.Text = "Changes to the PlugIns will not take effect until EveHQ is restarted."
        '
        'lblDetectedPlugIns
        '
        Me.lblDetectedPlugIns.AutoSize = True
        Me.lblDetectedPlugIns.Location = New System.Drawing.Point(6, 24)
        Me.lblDetectedPlugIns.Name = "lblDetectedPlugIns"
        Me.lblDetectedPlugIns.Size = New System.Drawing.Size(78, 13)
        Me.lblDetectedPlugIns.TabIndex = 1
        Me.lblDetectedPlugIns.Text = "Active PlugIns:"
        '
        'lvwPlugins
        '
        Me.lvwPlugins.CheckBoxes = True
        Me.lvwPlugins.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPlugInName, Me.colStatus})
        Me.lvwPlugins.GridLines = True
        Me.lvwPlugins.Location = New System.Drawing.Point(6, 43)
        Me.lvwPlugins.Name = "lvwPlugins"
        Me.lvwPlugins.Size = New System.Drawing.Size(407, 242)
        Me.lvwPlugins.TabIndex = 0
        Me.lvwPlugins.UseCompatibleStateImageBehavior = False
        Me.lvwPlugins.View = System.Windows.Forms.View.Details
        '
        'colPlugInName
        '
        Me.colPlugInName.Text = "Plug-In"
        Me.colPlugInName.Width = 230
        '
        'colStatus
        '
        Me.colStatus.Text = "Status"
        Me.colStatus.Width = 150
        '
        'gbNotifications
        '
        Me.gbNotifications.Controls.Add(Me.chkNotifyEarly)
        Me.gbNotifications.Controls.Add(Me.chkNotifyNow)
        Me.gbNotifications.Controls.Add(Me.lblNotifyMe)
        Me.gbNotifications.Controls.Add(Me.btnSoundTest)
        Me.gbNotifications.Controls.Add(Me.btnSelectSoundFile)
        Me.gbNotifications.Controls.Add(Me.lblSoundFile)
        Me.gbNotifications.Controls.Add(Me.chkNotifySound)
        Me.gbNotifications.Controls.Add(Me.lblNotifyOffset)
        Me.gbNotifications.Controls.Add(Me.trackNotifyOffset)
        Me.gbNotifications.Controls.Add(Me.gbEmailOptions)
        Me.gbNotifications.Controls.Add(Me.chkNotifyEmail)
        Me.gbNotifications.Controls.Add(Me.chkNotifyDialog)
        Me.gbNotifications.Controls.Add(Me.chkNotifyToolTip)
        Me.gbNotifications.Controls.Add(Me.nudShutdownNotifyPeriod)
        Me.gbNotifications.Controls.Add(Me.lblShutdownNotifyPeriod)
        Me.gbNotifications.Controls.Add(Me.chkShutdownNotify)
        Me.gbNotifications.Location = New System.Drawing.Point(411, 376)
        Me.gbNotifications.Name = "gbNotifications"
        Me.gbNotifications.Size = New System.Drawing.Size(104, 29)
        Me.gbNotifications.TabIndex = 20
        Me.gbNotifications.TabStop = False
        Me.gbNotifications.Text = "Notifications"
        Me.gbNotifications.Visible = False
        '
        'chkNotifyEarly
        '
        Me.chkNotifyEarly.AutoSize = True
        Me.chkNotifyEarly.Location = New System.Drawing.Point(236, 215)
        Me.chkNotifyEarly.Name = "chkNotifyEarly"
        Me.chkNotifyEarly.Size = New System.Drawing.Size(115, 17)
        Me.chkNotifyEarly.TabIndex = 20
        Me.chkNotifyEarly.Text = "Before skill finishes"
        Me.chkNotifyEarly.UseVisualStyleBackColor = True
        '
        'chkNotifyNow
        '
        Me.chkNotifyNow.AutoSize = True
        Me.chkNotifyNow.Location = New System.Drawing.Point(98, 215)
        Me.chkNotifyNow.Name = "chkNotifyNow"
        Me.chkNotifyNow.Size = New System.Drawing.Size(113, 17)
        Me.chkNotifyNow.TabIndex = 19
        Me.chkNotifyNow.Text = "When skill finishes"
        Me.chkNotifyNow.UseVisualStyleBackColor = True
        '
        'lblNotifyMe
        '
        Me.lblNotifyMe.AutoSize = True
        Me.lblNotifyMe.Location = New System.Drawing.Point(18, 216)
        Me.lblNotifyMe.Name = "lblNotifyMe"
        Me.lblNotifyMe.Size = New System.Drawing.Size(55, 13)
        Me.lblNotifyMe.TabIndex = 18
        Me.lblNotifyMe.Text = "Notify Me:"
        '
        'btnSoundTest
        '
        Me.btnSoundTest.Location = New System.Drawing.Point(422, 182)
        Me.btnSoundTest.Name = "btnSoundTest"
        Me.btnSoundTest.Size = New System.Drawing.Size(36, 20)
        Me.btnSoundTest.TabIndex = 17
        Me.btnSoundTest.Text = "Test"
        Me.btnSoundTest.UseVisualStyleBackColor = True
        '
        'btnSelectSoundFile
        '
        Me.btnSelectSoundFile.Location = New System.Drawing.Point(367, 182)
        Me.btnSelectSoundFile.Name = "btnSelectSoundFile"
        Me.btnSelectSoundFile.Size = New System.Drawing.Size(49, 20)
        Me.btnSelectSoundFile.TabIndex = 16
        Me.btnSelectSoundFile.Text = "Select"
        Me.btnSelectSoundFile.UseVisualStyleBackColor = True
        '
        'lblSoundFile
        '
        Me.lblSoundFile.AutoEllipsis = True
        Me.lblSoundFile.BackColor = System.Drawing.SystemColors.Window
        Me.lblSoundFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSoundFile.Location = New System.Drawing.Point(38, 182)
        Me.lblSoundFile.Name = "lblSoundFile"
        Me.lblSoundFile.Size = New System.Drawing.Size(323, 20)
        Me.lblSoundFile.TabIndex = 15
        '
        'chkNotifySound
        '
        Me.chkNotifySound.AutoSize = True
        Me.chkNotifySound.Location = New System.Drawing.Point(21, 158)
        Me.chkNotifySound.Name = "chkNotifySound"
        Me.chkNotifySound.Size = New System.Drawing.Size(136, 17)
        Me.chkNotifySound.TabIndex = 14
        Me.chkNotifySound.Text = "Play Sound Notification"
        Me.chkNotifySound.UseVisualStyleBackColor = True
        '
        'lblNotifyOffset
        '
        Me.lblNotifyOffset.AutoSize = True
        Me.lblNotifyOffset.Location = New System.Drawing.Point(18, 246)
        Me.lblNotifyOffset.Name = "lblNotifyOffset"
        Me.lblNotifyOffset.Size = New System.Drawing.Size(120, 13)
        Me.lblNotifyOffset.TabIndex = 13
        Me.lblNotifyOffset.Text = "Early Notification Offset:"
        '
        'trackNotifyOffset
        '
        Me.trackNotifyOffset.BackColor = System.Drawing.SystemColors.Control
        Me.trackNotifyOffset.Location = New System.Drawing.Point(21, 262)
        Me.trackNotifyOffset.Maximum = 900
        Me.trackNotifyOffset.Name = "trackNotifyOffset"
        Me.trackNotifyOffset.Size = New System.Drawing.Size(450, 45)
        Me.trackNotifyOffset.TabIndex = 12
        Me.trackNotifyOffset.TickFrequency = 30
        Me.trackNotifyOffset.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'gbEmailOptions
        '
        Me.gbEmailOptions.Controls.Add(Me.btnTestEmail)
        Me.gbEmailOptions.Controls.Add(Me.lblEmailPassword)
        Me.gbEmailOptions.Controls.Add(Me.txtEmailPassword)
        Me.gbEmailOptions.Controls.Add(Me.txtEmailUsername)
        Me.gbEmailOptions.Controls.Add(Me.lblEmailUsername)
        Me.gbEmailOptions.Controls.Add(Me.chkSMTPAuthentication)
        Me.gbEmailOptions.Controls.Add(Me.lblEMailAddress)
        Me.gbEmailOptions.Controls.Add(Me.txtEmailAddress)
        Me.gbEmailOptions.Controls.Add(Me.txtSMTPServer)
        Me.gbEmailOptions.Controls.Add(Me.lblSMTPServer)
        Me.gbEmailOptions.Location = New System.Drawing.Point(21, 324)
        Me.gbEmailOptions.Name = "gbEmailOptions"
        Me.gbEmailOptions.Size = New System.Drawing.Size(452, 154)
        Me.gbEmailOptions.TabIndex = 6
        Me.gbEmailOptions.TabStop = False
        Me.gbEmailOptions.Text = "E-Mail Options"
        Me.gbEmailOptions.Visible = False
        '
        'btnTestEmail
        '
        Me.btnTestEmail.Location = New System.Drawing.Point(371, 118)
        Me.btnTestEmail.Name = "btnTestEmail"
        Me.btnTestEmail.Size = New System.Drawing.Size(75, 23)
        Me.btnTestEmail.TabIndex = 9
        Me.btnTestEmail.Text = "Test E-Mail"
        Me.btnTestEmail.UseVisualStyleBackColor = True
        '
        'lblEmailPassword
        '
        Me.lblEmailPassword.AutoSize = True
        Me.lblEmailPassword.Enabled = False
        Me.lblEmailPassword.Location = New System.Drawing.Point(7, 124)
        Me.lblEmailPassword.Name = "lblEmailPassword"
        Me.lblEmailPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblEmailPassword.TabIndex = 8
        Me.lblEmailPassword.Text = "Password:"
        '
        'txtEmailPassword
        '
        Me.txtEmailPassword.Enabled = False
        Me.txtEmailPassword.Location = New System.Drawing.Point(106, 121)
        Me.txtEmailPassword.Name = "txtEmailPassword"
        Me.txtEmailPassword.Size = New System.Drawing.Size(250, 20)
        Me.txtEmailPassword.TabIndex = 7
        '
        'txtEmailUsername
        '
        Me.txtEmailUsername.Enabled = False
        Me.txtEmailUsername.Location = New System.Drawing.Point(106, 95)
        Me.txtEmailUsername.Name = "txtEmailUsername"
        Me.txtEmailUsername.Size = New System.Drawing.Size(250, 20)
        Me.txtEmailUsername.TabIndex = 6
        '
        'lblEmailUsername
        '
        Me.lblEmailUsername.AutoSize = True
        Me.lblEmailUsername.Enabled = False
        Me.lblEmailUsername.Location = New System.Drawing.Point(7, 98)
        Me.lblEmailUsername.Name = "lblEmailUsername"
        Me.lblEmailUsername.Size = New System.Drawing.Size(58, 13)
        Me.lblEmailUsername.TabIndex = 5
        Me.lblEmailUsername.Text = "Username:"
        '
        'chkSMTPAuthentication
        '
        Me.chkSMTPAuthentication.AutoSize = True
        Me.chkSMTPAuthentication.Location = New System.Drawing.Point(10, 72)
        Me.chkSMTPAuthentication.Name = "chkSMTPAuthentication"
        Me.chkSMTPAuthentication.Size = New System.Drawing.Size(149, 17)
        Me.chkSMTPAuthentication.TabIndex = 4
        Me.chkSMTPAuthentication.Text = "Use SMTP Authentication"
        Me.chkSMTPAuthentication.UseVisualStyleBackColor = True
        '
        'lblEMailAddress
        '
        Me.lblEMailAddress.AutoSize = True
        Me.lblEMailAddress.Location = New System.Drawing.Point(7, 49)
        Me.lblEMailAddress.Name = "lblEMailAddress"
        Me.lblEMailAddress.Size = New System.Drawing.Size(80, 13)
        Me.lblEMailAddress.TabIndex = 3
        Me.lblEMailAddress.Text = "E-Mail Address:"
        '
        'txtEmailAddress
        '
        Me.txtEmailAddress.Location = New System.Drawing.Point(106, 46)
        Me.txtEmailAddress.Name = "txtEmailAddress"
        Me.txtEmailAddress.Size = New System.Drawing.Size(331, 20)
        Me.txtEmailAddress.TabIndex = 2
        '
        'txtSMTPServer
        '
        Me.txtSMTPServer.Location = New System.Drawing.Point(106, 20)
        Me.txtSMTPServer.Name = "txtSMTPServer"
        Me.txtSMTPServer.Size = New System.Drawing.Size(331, 20)
        Me.txtSMTPServer.TabIndex = 1
        '
        'lblSMTPServer
        '
        Me.lblSMTPServer.AutoSize = True
        Me.lblSMTPServer.Location = New System.Drawing.Point(7, 23)
        Me.lblSMTPServer.Name = "lblSMTPServer"
        Me.lblSMTPServer.Size = New System.Drawing.Size(74, 13)
        Me.lblSMTPServer.TabIndex = 0
        Me.lblSMTPServer.Text = "SMTP Server:"
        '
        'chkNotifyEmail
        '
        Me.chkNotifyEmail.AutoSize = True
        Me.chkNotifyEmail.Location = New System.Drawing.Point(21, 135)
        Me.chkNotifyEmail.Name = "chkNotifyEmail"
        Me.chkNotifyEmail.Size = New System.Drawing.Size(137, 17)
        Me.chkNotifyEmail.TabIndex = 5
        Me.chkNotifyEmail.Text = "Send E-Mail Notifcation"
        Me.chkNotifyEmail.UseVisualStyleBackColor = True
        '
        'chkNotifyDialog
        '
        Me.chkNotifyDialog.AutoSize = True
        Me.chkNotifyDialog.Location = New System.Drawing.Point(21, 112)
        Me.chkNotifyDialog.Name = "chkNotifyDialog"
        Me.chkNotifyDialog.Size = New System.Drawing.Size(163, 17)
        Me.chkNotifyDialog.TabIndex = 4
        Me.chkNotifyDialog.Text = "Show Dialog Box Notification"
        Me.chkNotifyDialog.UseVisualStyleBackColor = True
        '
        'chkNotifyToolTip
        '
        Me.chkNotifyToolTip.AutoSize = True
        Me.chkNotifyToolTip.Location = New System.Drawing.Point(21, 89)
        Me.chkNotifyToolTip.Name = "chkNotifyToolTip"
        Me.chkNotifyToolTip.Size = New System.Drawing.Size(204, 17)
        Me.chkNotifyToolTip.TabIndex = 3
        Me.chkNotifyToolTip.Text = "Show System Tray Popup Notification"
        Me.chkNotifyToolTip.UseVisualStyleBackColor = True
        '
        'nudShutdownNotifyPeriod
        '
        Me.nudShutdownNotifyPeriod.Location = New System.Drawing.Point(219, 53)
        Me.nudShutdownNotifyPeriod.Maximum = New Decimal(New Integer() {72, 0, 0, 0})
        Me.nudShutdownNotifyPeriod.Name = "nudShutdownNotifyPeriod"
        Me.nudShutdownNotifyPeriod.Size = New System.Drawing.Size(68, 20)
        Me.nudShutdownNotifyPeriod.TabIndex = 2
        Me.nudShutdownNotifyPeriod.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'lblShutdownNotifyPeriod
        '
        Me.lblShutdownNotifyPeriod.AutoSize = True
        Me.lblShutdownNotifyPeriod.Location = New System.Drawing.Point(71, 55)
        Me.lblShutdownNotifyPeriod.Name = "lblShutdownNotifyPeriod"
        Me.lblShutdownNotifyPeriod.Size = New System.Drawing.Size(131, 13)
        Me.lblShutdownNotifyPeriod.TabIndex = 1
        Me.lblShutdownNotifyPeriod.Text = "Notification Period (hours):"
        '
        'chkShutdownNotify
        '
        Me.chkShutdownNotify.AutoSize = True
        Me.chkShutdownNotify.Location = New System.Drawing.Point(21, 30)
        Me.chkShutdownNotify.Name = "chkShutdownNotify"
        Me.chkShutdownNotify.Size = New System.Drawing.Size(334, 17)
        Me.chkShutdownNotify.TabIndex = 0
        Me.chkShutdownNotify.Text = "Notify of imminent completion of skill training on EveHQ shutdown"
        Me.chkShutdownNotify.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(12, 492)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 25)
        Me.btnClose.TabIndex = 26
        Me.btnClose.Text = "&OK"
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'tvwSettings
        '
        Me.tvwSettings.Location = New System.Drawing.Point(12, 12)
        Me.tvwSettings.Name = "tvwSettings"
        TreeNode1.Name = "nodeGeneral"
        TreeNode1.Text = "General"
        TreeNode2.Name = "nodeDatabaseFormat"
        TreeNode2.Text = "Database Format"
        TreeNode3.Name = "nodeEveAccounts"
        TreeNode3.Text = "Eve Accounts"
        TreeNode4.Name = "nodeEveFolders"
        TreeNode4.Text = "Eve Folders"
        TreeNode5.Name = "nodeEveServer"
        TreeNode5.Text = "Eve API & Server"
        TreeNode6.Name = "nodeFTPAccounts"
        TreeNode6.Text = "FTP Accounts"
        TreeNode7.Name = "nodeG15"
        TreeNode7.Text = "G15 Display"
        TreeNode8.Name = "nodeIGB"
        TreeNode8.Text = "IGB"
        TreeNode9.Name = "nodeMarketPrices"
        TreeNode9.Text = "Market Prices"
        TreeNode10.Name = "nodeNotifications"
        TreeNode10.Text = "Notifications"
        TreeNode11.Name = "nodePilots"
        TreeNode11.Text = "Pilots"
        TreeNode12.Name = "nodePlugins"
        TreeNode12.Text = "Plug Ins"
        TreeNode13.Name = "nodeProxyServer"
        TreeNode13.Text = "Proxy Server"
        TreeNode14.Name = "nodeTrainingOverlay"
        TreeNode14.Text = "Training Overlay"
        TreeNode15.Name = "nodeTrainingQueue"
        TreeNode15.Text = "Training Queue"
        Me.tvwSettings.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6, TreeNode7, TreeNode8, TreeNode9, TreeNode10, TreeNode11, TreeNode12, TreeNode13, TreeNode14, TreeNode15})
        Me.tvwSettings.Size = New System.Drawing.Size(176, 473)
        Me.tvwSettings.TabIndex = 27
        '
        'gbColours
        '
        Me.gbColours.Controls.Add(Me.pbPilotTrainingHighlight)
        Me.gbColours.Controls.Add(Me.lblPilotTrainingHighlight)
        Me.gbColours.Location = New System.Drawing.Point(200, 90)
        Me.gbColours.Name = "gbColours"
        Me.gbColours.Size = New System.Drawing.Size(60, 37)
        Me.gbColours.TabIndex = 28
        Me.gbColours.TabStop = False
        Me.gbColours.Text = "Colours"
        Me.gbColours.Visible = False
        '
        'pbPilotTrainingHighlight
        '
        Me.pbPilotTrainingHighlight.BackColor = System.Drawing.Color.Lime
        Me.pbPilotTrainingHighlight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotTrainingHighlight.Location = New System.Drawing.Point(204, 24)
        Me.pbPilotTrainingHighlight.Name = "pbPilotTrainingHighlight"
        Me.pbPilotTrainingHighlight.Size = New System.Drawing.Size(46, 19)
        Me.pbPilotTrainingHighlight.TabIndex = 1
        Me.pbPilotTrainingHighlight.TabStop = False
        '
        'lblPilotTrainingHighlight
        '
        Me.lblPilotTrainingHighlight.AutoSize = True
        Me.lblPilotTrainingHighlight.Location = New System.Drawing.Point(19, 30)
        Me.lblPilotTrainingHighlight.Name = "lblPilotTrainingHighlight"
        Me.lblPilotTrainingHighlight.Size = New System.Drawing.Size(160, 13)
        Me.lblPilotTrainingHighlight.TabIndex = 0
        Me.lblPilotTrainingHighlight.Text = "Pilot Form Skill Training Highlight"
        '
        'gbTrainingOverlay
        '
        Me.gbTrainingOverlay.Controls.Add(Me.chkClickThroughOverlay)
        Me.gbTrainingOverlay.Controls.Add(Me.pbFontColour)
        Me.gbTrainingOverlay.Controls.Add(Me.lblFontColour)
        Me.gbTrainingOverlay.Controls.Add(Me.chkShowOverlayOnStartup)
        Me.gbTrainingOverlay.Controls.Add(Me.nudOverlayYOffset)
        Me.gbTrainingOverlay.Controls.Add(Me.nudOverlayXOffset)
        Me.gbTrainingOverlay.Controls.Add(Me.lvlOverlayYOffset)
        Me.gbTrainingOverlay.Controls.Add(Me.lblOverlayXOffset)
        Me.gbTrainingOverlay.Controls.Add(Me.lblOverlayOffset)
        Me.gbTrainingOverlay.Controls.Add(Me.radBottomRight)
        Me.gbTrainingOverlay.Controls.Add(Me.radBottomLeft)
        Me.gbTrainingOverlay.Controls.Add(Me.radTopRight)
        Me.gbTrainingOverlay.Controls.Add(Me.lblOverlayPosition)
        Me.gbTrainingOverlay.Controls.Add(Me.radTopLeft)
        Me.gbTrainingOverlay.Controls.Add(Me.pbPanelColour)
        Me.gbTrainingOverlay.Controls.Add(Me.pbBorderColour)
        Me.gbTrainingOverlay.Controls.Add(Me.lblTransparancyValue)
        Me.gbTrainingOverlay.Controls.Add(Me.tbTransparancy)
        Me.gbTrainingOverlay.Controls.Add(Me.lblTransparancy)
        Me.gbTrainingOverlay.Controls.Add(Me.lblPanelColour)
        Me.gbTrainingOverlay.Controls.Add(Me.lblBorderColour)
        Me.gbTrainingOverlay.Location = New System.Drawing.Point(407, 245)
        Me.gbTrainingOverlay.Name = "gbTrainingOverlay"
        Me.gbTrainingOverlay.Size = New System.Drawing.Size(101, 23)
        Me.gbTrainingOverlay.TabIndex = 29
        Me.gbTrainingOverlay.TabStop = False
        Me.gbTrainingOverlay.Text = "Training Overlay"
        Me.gbTrainingOverlay.Visible = False
        '
        'chkClickThroughOverlay
        '
        Me.chkClickThroughOverlay.AutoSize = True
        Me.chkClickThroughOverlay.Location = New System.Drawing.Point(10, 365)
        Me.chkClickThroughOverlay.Name = "chkClickThroughOverlay"
        Me.chkClickThroughOverlay.Size = New System.Drawing.Size(141, 17)
        Me.chkClickThroughOverlay.TabIndex = 20
        Me.chkClickThroughOverlay.Text = """Click-Through"" Overlay"
        Me.chkClickThroughOverlay.UseVisualStyleBackColor = True
        '
        'pbFontColour
        '
        Me.pbFontColour.BackColor = System.Drawing.Color.Black
        Me.pbFontColour.Location = New System.Drawing.Point(95, 91)
        Me.pbFontColour.Name = "pbFontColour"
        Me.pbFontColour.Size = New System.Drawing.Size(24, 24)
        Me.pbFontColour.TabIndex = 19
        Me.pbFontColour.TabStop = False
        '
        'lblFontColour
        '
        Me.lblFontColour.AutoSize = True
        Me.lblFontColour.Location = New System.Drawing.Point(6, 99)
        Me.lblFontColour.Name = "lblFontColour"
        Me.lblFontColour.Size = New System.Drawing.Size(61, 13)
        Me.lblFontColour.TabIndex = 18
        Me.lblFontColour.Text = "Font Colour"
        '
        'chkShowOverlayOnStartup
        '
        Me.chkShowOverlayOnStartup.AutoSize = True
        Me.chkShowOverlayOnStartup.Location = New System.Drawing.Point(10, 342)
        Me.chkShowOverlayOnStartup.Name = "chkShowOverlayOnStartup"
        Me.chkShowOverlayOnStartup.Size = New System.Drawing.Size(146, 17)
        Me.chkShowOverlayOnStartup.TabIndex = 17
        Me.chkShowOverlayOnStartup.Text = "Show Overlay On Startup"
        Me.chkShowOverlayOnStartup.UseVisualStyleBackColor = True
        '
        'nudOverlayYOffset
        '
        Me.nudOverlayYOffset.Location = New System.Drawing.Point(239, 256)
        Me.nudOverlayYOffset.Name = "nudOverlayYOffset"
        Me.nudOverlayYOffset.Size = New System.Drawing.Size(57, 20)
        Me.nudOverlayYOffset.TabIndex = 16
        Me.nudOverlayYOffset.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'nudOverlayXOffset
        '
        Me.nudOverlayXOffset.Location = New System.Drawing.Point(239, 233)
        Me.nudOverlayXOffset.Name = "nudOverlayXOffset"
        Me.nudOverlayXOffset.Size = New System.Drawing.Size(57, 20)
        Me.nudOverlayXOffset.TabIndex = 15
        Me.nudOverlayXOffset.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lvlOverlayYOffset
        '
        Me.lvlOverlayYOffset.AutoSize = True
        Me.lvlOverlayYOffset.Location = New System.Drawing.Point(183, 258)
        Me.lvlOverlayYOffset.Name = "lvlOverlayYOffset"
        Me.lvlOverlayYOffset.Size = New System.Drawing.Size(45, 13)
        Me.lvlOverlayYOffset.TabIndex = 14
        Me.lvlOverlayYOffset.Text = "Y Offset"
        '
        'lblOverlayXOffset
        '
        Me.lblOverlayXOffset.AutoSize = True
        Me.lblOverlayXOffset.Location = New System.Drawing.Point(183, 235)
        Me.lblOverlayXOffset.Name = "lblOverlayXOffset"
        Me.lblOverlayXOffset.Size = New System.Drawing.Size(45, 13)
        Me.lblOverlayXOffset.TabIndex = 13
        Me.lblOverlayXOffset.Text = "X Offset"
        '
        'lblOverlayOffset
        '
        Me.lblOverlayOffset.AutoSize = True
        Me.lblOverlayOffset.Location = New System.Drawing.Point(166, 208)
        Me.lblOverlayOffset.Name = "lblOverlayOffset"
        Me.lblOverlayOffset.Size = New System.Drawing.Size(77, 13)
        Me.lblOverlayOffset.TabIndex = 12
        Me.lblOverlayOffset.Text = "Overlay Offset:"
        '
        'radBottomRight
        '
        Me.radBottomRight.AutoSize = True
        Me.radBottomRight.Checked = True
        Me.radBottomRight.Location = New System.Drawing.Point(22, 302)
        Me.radBottomRight.Name = "radBottomRight"
        Me.radBottomRight.Size = New System.Drawing.Size(86, 17)
        Me.radBottomRight.TabIndex = 11
        Me.radBottomRight.TabStop = True
        Me.radBottomRight.Text = "Bottom Right"
        Me.radBottomRight.UseVisualStyleBackColor = True
        '
        'radBottomLeft
        '
        Me.radBottomLeft.AutoSize = True
        Me.radBottomLeft.Location = New System.Drawing.Point(22, 279)
        Me.radBottomLeft.Name = "radBottomLeft"
        Me.radBottomLeft.Size = New System.Drawing.Size(79, 17)
        Me.radBottomLeft.TabIndex = 10
        Me.radBottomLeft.Text = "Bottom Left"
        Me.radBottomLeft.UseVisualStyleBackColor = True
        '
        'radTopRight
        '
        Me.radTopRight.AutoSize = True
        Me.radTopRight.Location = New System.Drawing.Point(22, 256)
        Me.radTopRight.Name = "radTopRight"
        Me.radTopRight.Size = New System.Drawing.Size(72, 17)
        Me.radTopRight.TabIndex = 9
        Me.radTopRight.Text = "Top Right"
        Me.radTopRight.UseVisualStyleBackColor = True
        '
        'lblOverlayPosition
        '
        Me.lblOverlayPosition.AutoSize = True
        Me.lblOverlayPosition.Location = New System.Drawing.Point(7, 208)
        Me.lblOverlayPosition.Name = "lblOverlayPosition"
        Me.lblOverlayPosition.Size = New System.Drawing.Size(86, 13)
        Me.lblOverlayPosition.TabIndex = 8
        Me.lblOverlayPosition.Text = "Overlay Position:"
        '
        'radTopLeft
        '
        Me.radTopLeft.AutoSize = True
        Me.radTopLeft.Location = New System.Drawing.Point(22, 233)
        Me.radTopLeft.Name = "radTopLeft"
        Me.radTopLeft.Size = New System.Drawing.Size(65, 17)
        Me.radTopLeft.TabIndex = 7
        Me.radTopLeft.Text = "Top Left"
        Me.radTopLeft.UseVisualStyleBackColor = True
        '
        'pbPanelColour
        '
        Me.pbPanelColour.BackColor = System.Drawing.Color.White
        Me.pbPanelColour.Location = New System.Drawing.Point(95, 61)
        Me.pbPanelColour.Name = "pbPanelColour"
        Me.pbPanelColour.Size = New System.Drawing.Size(24, 24)
        Me.pbPanelColour.TabIndex = 6
        Me.pbPanelColour.TabStop = False
        '
        'pbBorderColour
        '
        Me.pbBorderColour.BackColor = System.Drawing.Color.Black
        Me.pbBorderColour.Location = New System.Drawing.Point(95, 31)
        Me.pbBorderColour.Name = "pbBorderColour"
        Me.pbBorderColour.Size = New System.Drawing.Size(24, 24)
        Me.pbBorderColour.TabIndex = 5
        Me.pbBorderColour.TabStop = False
        '
        'lblTransparancyValue
        '
        Me.lblTransparancyValue.AutoSize = True
        Me.lblTransparancyValue.Location = New System.Drawing.Point(369, 162)
        Me.lblTransparancyValue.Name = "lblTransparancyValue"
        Me.lblTransparancyValue.Size = New System.Drawing.Size(33, 13)
        Me.lblTransparancyValue.TabIndex = 4
        Me.lblTransparancyValue.Text = "100%"
        '
        'tbTransparancy
        '
        Me.tbTransparancy.Location = New System.Drawing.Point(85, 148)
        Me.tbTransparancy.Maximum = 100
        Me.tbTransparancy.Name = "tbTransparancy"
        Me.tbTransparancy.Size = New System.Drawing.Size(283, 45)
        Me.tbTransparancy.TabIndex = 3
        Me.tbTransparancy.TickFrequency = 5
        Me.tbTransparancy.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'lblTransparancy
        '
        Me.lblTransparancy.AutoSize = True
        Me.lblTransparancy.Location = New System.Drawing.Point(7, 162)
        Me.lblTransparancy.Name = "lblTransparancy"
        Me.lblTransparancy.Size = New System.Drawing.Size(72, 13)
        Me.lblTransparancy.TabIndex = 2
        Me.lblTransparancy.Text = "Transparancy"
        '
        'lblPanelColour
        '
        Me.lblPanelColour.AutoSize = True
        Me.lblPanelColour.Location = New System.Drawing.Point(6, 67)
        Me.lblPanelColour.Name = "lblPanelColour"
        Me.lblPanelColour.Size = New System.Drawing.Size(67, 13)
        Me.lblPanelColour.TabIndex = 1
        Me.lblPanelColour.Text = "Panel Colour"
        '
        'lblBorderColour
        '
        Me.lblBorderColour.AutoSize = True
        Me.lblBorderColour.Location = New System.Drawing.Point(6, 37)
        Me.lblBorderColour.Name = "lblBorderColour"
        Me.lblBorderColour.Size = New System.Drawing.Size(71, 13)
        Me.lblBorderColour.TabIndex = 0
        Me.lblBorderColour.Text = "Border Colour"
        '
        'gbG15
        '
        Me.gbG15.Controls.Add(Me.nudCycleTime)
        Me.gbG15.Controls.Add(Me.lblCycleTime)
        Me.gbG15.Controls.Add(Me.chkCyclePilots)
        Me.gbG15.Controls.Add(Me.chkActivateG15)
        Me.gbG15.Location = New System.Drawing.Point(308, 66)
        Me.gbG15.Name = "gbG15"
        Me.gbG15.Size = New System.Drawing.Size(91, 35)
        Me.gbG15.TabIndex = 30
        Me.gbG15.TabStop = False
        Me.gbG15.Text = "G15 Display"
        Me.gbG15.Visible = False
        '
        'nudCycleTime
        '
        Me.nudCycleTime.Location = New System.Drawing.Point(301, 71)
        Me.nudCycleTime.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.nudCycleTime.Name = "nudCycleTime"
        Me.nudCycleTime.Size = New System.Drawing.Size(73, 20)
        Me.nudCycleTime.TabIndex = 5
        '
        'lblCycleTime
        '
        Me.lblCycleTime.AutoSize = True
        Me.lblCycleTime.Location = New System.Drawing.Point(213, 73)
        Me.lblCycleTime.Name = "lblCycleTime"
        Me.lblCycleTime.Size = New System.Drawing.Size(76, 13)
        Me.lblCycleTime.TabIndex = 4
        Me.lblCycleTime.Text = "Cycle Time (s):"
        '
        'chkCyclePilots
        '
        Me.chkCyclePilots.AutoSize = True
        Me.chkCyclePilots.Location = New System.Drawing.Point(47, 72)
        Me.chkCyclePilots.Name = "chkCyclePilots"
        Me.chkCyclePilots.Size = New System.Drawing.Size(121, 17)
        Me.chkCyclePilots.TabIndex = 3
        Me.chkCyclePilots.Text = "Cycle Training Pilots"
        Me.chkCyclePilots.UseVisualStyleBackColor = True
        '
        'chkActivateG15
        '
        Me.chkActivateG15.AutoSize = True
        Me.chkActivateG15.Location = New System.Drawing.Point(19, 35)
        Me.chkActivateG15.Name = "chkActivateG15"
        Me.chkActivateG15.Size = New System.Drawing.Size(125, 17)
        Me.chkActivateG15.TabIndex = 2
        Me.chkActivateG15.Text = "Activate G15 Display"
        Me.chkActivateG15.UseVisualStyleBackColor = True
        '
        'gbMarketPrices
        '
        Me.gbMarketPrices.Controls.Add(Me.btnResetGrid)
        Me.gbMarketPrices.Controls.Add(Me.txtSearchPrices)
        Me.gbMarketPrices.Controls.Add(Me.lblSearchPrices)
        Me.gbMarketPrices.Controls.Add(Me.lblUpdatePrice)
        Me.gbMarketPrices.Controls.Add(Me.txtUpdatePrice)
        Me.gbMarketPrices.Controls.Add(Me.lvwPrices)
        Me.gbMarketPrices.Controls.Add(Me.lblMarketPriceStats)
        Me.gbMarketPrices.Controls.Add(Me.lblUpdateStatus)
        Me.gbMarketPrices.Controls.Add(Me.lblUpdateStatusLabel)
        Me.gbMarketPrices.Controls.Add(Me.lblLastUpdateTime)
        Me.gbMarketPrices.Controls.Add(Me.lblLastUpdate)
        Me.gbMarketPrices.Controls.Add(Me.btnUpdatePrices)
        Me.gbMarketPrices.Location = New System.Drawing.Point(729, 218)
        Me.gbMarketPrices.Name = "gbMarketPrices"
        Me.gbMarketPrices.Size = New System.Drawing.Size(94, 58)
        Me.gbMarketPrices.TabIndex = 31
        Me.gbMarketPrices.TabStop = False
        Me.gbMarketPrices.Text = "Market Prices"
        Me.gbMarketPrices.Visible = False
        '
        'btnResetGrid
        '
        Me.btnResetGrid.Location = New System.Drawing.Point(266, 83)
        Me.btnResetGrid.Name = "btnResetGrid"
        Me.btnResetGrid.Size = New System.Drawing.Size(75, 23)
        Me.btnResetGrid.TabIndex = 12
        Me.btnResetGrid.Text = "Reset Grid"
        Me.btnResetGrid.UseVisualStyleBackColor = True
        '
        'txtSearchPrices
        '
        Me.txtSearchPrices.Location = New System.Drawing.Point(81, 85)
        Me.txtSearchPrices.Name = "txtSearchPrices"
        Me.txtSearchPrices.Size = New System.Drawing.Size(177, 20)
        Me.txtSearchPrices.TabIndex = 11
        '
        'lblSearchPrices
        '
        Me.lblSearchPrices.AutoSize = True
        Me.lblSearchPrices.Location = New System.Drawing.Point(6, 89)
        Me.lblSearchPrices.Name = "lblSearchPrices"
        Me.lblSearchPrices.Size = New System.Drawing.Size(72, 13)
        Me.lblSearchPrices.TabIndex = 10
        Me.lblSearchPrices.Text = "Search Items:"
        '
        'lblUpdatePrice
        '
        Me.lblUpdatePrice.AutoSize = True
        Me.lblUpdatePrice.Location = New System.Drawing.Point(473, 88)
        Me.lblUpdatePrice.Name = "lblUpdatePrice"
        Me.lblUpdatePrice.Size = New System.Drawing.Size(72, 13)
        Me.lblUpdatePrice.TabIndex = 9
        Me.lblUpdatePrice.Text = "Update Price:"
        Me.lblUpdatePrice.Visible = False
        '
        'txtUpdatePrice
        '
        Me.txtUpdatePrice.Location = New System.Drawing.Point(549, 85)
        Me.txtUpdatePrice.Name = "txtUpdatePrice"
        Me.txtUpdatePrice.Size = New System.Drawing.Size(118, 20)
        Me.txtUpdatePrice.TabIndex = 8
        Me.txtUpdatePrice.Visible = False
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
        Me.lvwPrices.Location = New System.Drawing.Point(6, 110)
        Me.lvwPrices.MultiSelect = False
        Me.lvwPrices.Name = "lvwPrices"
        Me.lvwPrices.Size = New System.Drawing.Size(82, 0)
        Me.lvwPrices.TabIndex = 7
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
        'lblMarketPriceStats
        '
        Me.lblMarketPriceStats.AutoSize = True
        Me.lblMarketPriceStats.Location = New System.Drawing.Point(6, 65)
        Me.lblMarketPriceStats.Name = "lblMarketPriceStats"
        Me.lblMarketPriceStats.Size = New System.Drawing.Size(97, 13)
        Me.lblMarketPriceStats.TabIndex = 6
        Me.lblMarketPriceStats.Text = "Market Price Stats:"
        '
        'lblUpdateStatus
        '
        Me.lblUpdateStatus.Location = New System.Drawing.Point(90, 41)
        Me.lblUpdateStatus.Name = "lblUpdateStatus"
        Me.lblUpdateStatus.Size = New System.Drawing.Size(396, 24)
        Me.lblUpdateStatus.TabIndex = 5
        Me.lblUpdateStatus.Text = "n/a"
        '
        'lblUpdateStatusLabel
        '
        Me.lblUpdateStatusLabel.AutoSize = True
        Me.lblUpdateStatusLabel.Location = New System.Drawing.Point(6, 41)
        Me.lblUpdateStatusLabel.Name = "lblUpdateStatusLabel"
        Me.lblUpdateStatusLabel.Size = New System.Drawing.Size(78, 13)
        Me.lblUpdateStatusLabel.TabIndex = 4
        Me.lblUpdateStatusLabel.Text = "Update Status:"
        '
        'lblLastUpdateTime
        '
        Me.lblLastUpdateTime.AutoSize = True
        Me.lblLastUpdateTime.Location = New System.Drawing.Point(80, 19)
        Me.lblLastUpdateTime.Name = "lblLastUpdateTime"
        Me.lblLastUpdateTime.Size = New System.Drawing.Size(24, 13)
        Me.lblLastUpdateTime.TabIndex = 3
        Me.lblLastUpdateTime.Text = "n/a"
        '
        'lblLastUpdate
        '
        Me.lblLastUpdate.AutoSize = True
        Me.lblLastUpdate.Location = New System.Drawing.Point(6, 19)
        Me.lblLastUpdate.Name = "lblLastUpdate"
        Me.lblLastUpdate.Size = New System.Drawing.Size(68, 13)
        Me.lblLastUpdate.TabIndex = 2
        Me.lblLastUpdate.Text = "Last Update:"
        '
        'btnUpdatePrices
        '
        Me.btnUpdatePrices.Location = New System.Drawing.Point(568, 19)
        Me.btnUpdatePrices.Name = "btnUpdatePrices"
        Me.btnUpdatePrices.Size = New System.Drawing.Size(125, 23)
        Me.btnUpdatePrices.TabIndex = 0
        Me.btnUpdatePrices.Text = "Update Market Prices"
        Me.btnUpdatePrices.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(899, 524)
        Me.Controls.Add(Me.gbPilots)
        Me.Controls.Add(Me.gbMarketPrices)
        Me.Controls.Add(Me.gbEveAccounts)
        Me.Controls.Add(Me.gbDatabaseFormat)
        Me.Controls.Add(Me.gbEveServer)
        Me.Controls.Add(Me.gbTrainingQueue)
        Me.Controls.Add(Me.gbGeneral)
        Me.Controls.Add(Me.gbEveFolders)
        Me.Controls.Add(Me.gbPlugIns)
        Me.Controls.Add(Me.gbNotifications)
        Me.Controls.Add(Me.gbG15)
        Me.Controls.Add(Me.gbProxyServer)
        Me.Controls.Add(Me.gbTrainingOverlay)
        Me.Controls.Add(Me.gbIGB)
        Me.Controls.Add(Me.gbColours)
        Me.Controls.Add(Me.tvwSettings)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.gbFTPAccounts)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EveHQ Settings"
        Me.gbGeneral.ResumeLayout(False)
        Me.gbGeneral.PerformLayout()
        Me.gbPilotScreenColours.ResumeLayout(False)
        Me.gbPilotScreenColours.PerformLayout()
        CType(Me.pbPilotLevel5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotPartial, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotCurrent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotStandard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbPanelColours.ResumeLayout(False)
        Me.gbPanelColours.PerformLayout()
        CType(Me.pbPanelHighlight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPanelText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPanelRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPanelLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPanelBottomRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPanelTopLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPanelOutline, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPanelBackground, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbEveAccounts.ResumeLayout(False)
        Me.gbPilots.ResumeLayout(False)
        Me.gbIGB.ResumeLayout(False)
        Me.gbIGB.PerformLayout()
        CType(Me.nudIGBPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbFTPAccounts.ResumeLayout(False)
        Me.gbEveFolders.ResumeLayout(False)
        Me.gbLocation4.ResumeLayout(False)
        Me.gbLocation4.PerformLayout()
        Me.gbLocation3.ResumeLayout(False)
        Me.gbLocation3.PerformLayout()
        Me.gbLocation2.ResumeLayout(False)
        Me.gbLocation2.PerformLayout()
        Me.gbLocation1.ResumeLayout(False)
        Me.gbLocation1.PerformLayout()
        Me.gbTrainingQueue.ResumeLayout(False)
        Me.gbTrainingQueue.PerformLayout()
        CType(Me.pbPartiallyTrainedColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbReadySkillColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbDowntimeClashColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbBothPreReqColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbHasPreReqColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbIsPreReqColour, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDatabaseFormat.ResumeLayout(False)
        Me.gbDatabaseFormat.PerformLayout()
        Me.gbAccess.ResumeLayout(False)
        Me.gbAccess.PerformLayout()
        Me.gbMySQL.ResumeLayout(False)
        Me.gbMySQL.PerformLayout()
        Me.gbMSSQL.ResumeLayout(False)
        Me.gbMSSQL.PerformLayout()
        Me.gbProxyServer.ResumeLayout(False)
        Me.gbProxyServer.PerformLayout()
        Me.gbProxyServerInfo.ResumeLayout(False)
        Me.gbProxyServerInfo.PerformLayout()
        Me.gbEveServer.ResumeLayout(False)
        Me.gbEveServer.PerformLayout()
        Me.gbAPIServer.ResumeLayout(False)
        Me.gbAPIServer.PerformLayout()
        Me.gbAPIRelayServer.ResumeLayout(False)
        Me.gbAPIRelayServer.PerformLayout()
        CType(Me.nudAPIRSPort, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trackServerOffset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbPlugIns.ResumeLayout(False)
        Me.gbPlugIns.PerformLayout()
        Me.gbNotifications.ResumeLayout(False)
        Me.gbNotifications.PerformLayout()
        CType(Me.trackNotifyOffset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbEmailOptions.ResumeLayout(False)
        Me.gbEmailOptions.PerformLayout()
        CType(Me.nudShutdownNotifyPeriod, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbColours.ResumeLayout(False)
        Me.gbColours.PerformLayout()
        CType(Me.pbPilotTrainingHighlight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbTrainingOverlay.ResumeLayout(False)
        Me.gbTrainingOverlay.PerformLayout()
        CType(Me.pbFontColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudOverlayYOffset, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudOverlayXOffset, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPanelColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbBorderColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbTransparancy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbG15.ResumeLayout(False)
        Me.gbG15.PerformLayout()
        CType(Me.nudCycleTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbMarketPrices.ResumeLayout(False)
        Me.gbMarketPrices.PerformLayout()
        Me.ctxPrices.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbEveAccounts As System.Windows.Forms.GroupBox
    Friend WithEvents btnDeleteAccount As System.Windows.Forms.Button
    Friend WithEvents btnEditAccount As System.Windows.Forms.Button
    Friend WithEvents btnAddAccount As System.Windows.Forms.Button
    Friend WithEvents lvAccounts As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbPilots As System.Windows.Forms.GroupBox
    Friend WithEvents btnDeletePilot As System.Windows.Forms.Button
    Friend WithEvents btnAddPilot As System.Windows.Forms.Button
    Friend WithEvents lvwPilots As System.Windows.Forms.ListView
    Friend WithEvents colPilot As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAccount As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents colID As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbFTPAccounts As System.Windows.Forms.GroupBox
    Friend WithEvents btnDeleteFTP As System.Windows.Forms.Button
    Friend WithEvents btnEditFTP As System.Windows.Forms.Button
    Friend WithEvents btnAddFTP As System.Windows.Forms.Button
    Friend WithEvents lvwFTP As System.Windows.Forms.ListView
    Friend WithEvents FTPName As System.Windows.Forms.ColumnHeader
    Friend WithEvents Server As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnTestUpload As System.Windows.Forms.Button
    Friend WithEvents gbGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents chkAutoRun As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoHide As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoMinimise As System.Windows.Forms.CheckBox
    Friend WithEvents fbd1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents chkAutoCheck As System.Windows.Forms.CheckBox
    Friend WithEvents gbEveFolders As System.Windows.Forms.GroupBox
    Friend WithEvents btnClear4 As System.Windows.Forms.Button
    Friend WithEvents btnClear3 As System.Windows.Forms.Button
    Friend WithEvents btnClear2 As System.Windows.Forms.Button
    Friend WithEvents btnClear1 As System.Windows.Forms.Button
    Friend WithEvents btnEveDir4 As System.Windows.Forms.Button
    Friend WithEvents lblEveDir4 As System.Windows.Forms.Label
    Friend WithEvents btnEveDir3 As System.Windows.Forms.Button
    Friend WithEvents lblEveDir3 As System.Windows.Forms.Label
    Friend WithEvents btnEveDir2 As System.Windows.Forms.Button
    Friend WithEvents lblEveDir2 As System.Windows.Forms.Label
    Friend WithEvents btnEveDir1 As System.Windows.Forms.Button
    Friend WithEvents lblEveDir1 As System.Windows.Forms.Label
    Friend WithEvents cboStartupView As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboStartupPilot As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnGetData As System.Windows.Forms.Button
    Friend WithEvents lblQueueColumns As System.Windows.Forms.Label
    Friend WithEvents clbColumns As System.Windows.Forms.CheckedListBox
    Friend WithEvents gbDatabaseFormat As System.Windows.Forms.GroupBox
    Friend WithEvents gbMSSQL As System.Windows.Forms.GroupBox
    Friend WithEvents lblMSSQLSecurity As System.Windows.Forms.Label
    Friend WithEvents radMSSQLDatabase As System.Windows.Forms.RadioButton
    Friend WithEvents radMSSQLWindows As System.Windows.Forms.RadioButton
    Friend WithEvents txtMSSQLPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtMSSQLUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtMSSQLServer As System.Windows.Forms.TextBox
    Friend WithEvents lblMSSQLPassword As System.Windows.Forms.Label
    Friend WithEvents lblMSSQLUser As System.Windows.Forms.Label
    Friend WithEvents lblMSSQLServer As System.Windows.Forms.Label
    Friend WithEvents cboFormat As System.Windows.Forms.ComboBox
    Friend WithEvents lblFormat As System.Windows.Forms.Label
    Friend WithEvents gbMySQL As System.Windows.Forms.GroupBox
    Friend WithEvents txtMySQLPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtMySQLUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtMySQLServer As System.Windows.Forms.TextBox
    Friend WithEvents lblMySQLPassword As System.Windows.Forms.Label
    Friend WithEvents lblMySQLUser As System.Windows.Forms.Label
    Friend WithEvents lblMySQLServer As System.Windows.Forms.Label
    Friend WithEvents gbAccess As System.Windows.Forms.GroupBox
    Friend WithEvents txtMDBPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtMDBUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtMDBServer As System.Windows.Forms.TextBox
    Friend WithEvents lblMDBPassword As System.Windows.Forms.Label
    Friend WithEvents lblMDBUser As System.Windows.Forms.Label
    Friend WithEvents lblMDBFilename As System.Windows.Forms.Label
    Friend WithEvents btnBrowseMDB As System.Windows.Forms.Button
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnTestDB As System.Windows.Forms.Button
    Friend WithEvents btnAddPilotFromXML As System.Windows.Forms.Button
    Friend WithEvents gbProxyServer As System.Windows.Forms.GroupBox
    Friend WithEvents chkUseProxy As System.Windows.Forms.CheckBox
    Friend WithEvents gbProxyServerInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblProxyServer As System.Windows.Forms.Label
    Friend WithEvents txtProxyServer As System.Windows.Forms.TextBox
    Friend WithEvents radUseDefaultCreds As System.Windows.Forms.RadioButton
    Friend WithEvents radUseSpecifiedCreds As System.Windows.Forms.RadioButton
    Friend WithEvents lblProxyPassword As System.Windows.Forms.Label
    Friend WithEvents lblProxyUsername As System.Windows.Forms.Label
    Friend WithEvents txtProxyPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtProxyUsername As System.Windows.Forms.TextBox
    Friend WithEvents chkEncryptSettings As System.Windows.Forms.CheckBox
    Friend WithEvents gbEveServer As System.Windows.Forms.GroupBox
    Friend WithEvents lblCurrentOffset As System.Windows.Forms.Label
    Friend WithEvents lblServerOffset As System.Windows.Forms.Label
    Friend WithEvents trackServerOffset As System.Windows.Forms.TrackBar
    Friend WithEvents chkEnableEveStatus As System.Windows.Forms.CheckBox
    Friend WithEvents gbPlugIns As System.Windows.Forms.GroupBox
    Friend WithEvents lblDetectedPlugIns As System.Windows.Forms.Label
    Friend WithEvents lvwPlugins As System.Windows.Forms.ListView
    Friend WithEvents colPlugInName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbIGB As System.Windows.Forms.GroupBox
    Friend WithEvents lblIGBPort As System.Windows.Forms.Label
    Friend WithEvents nudIGBPort As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkStartIGBonLoad As System.Windows.Forms.CheckBox
    Friend WithEvents lblPlugInInfo As System.Windows.Forms.Label
    Friend WithEvents gbTrainingQueue As System.Windows.Forms.GroupBox
    Friend WithEvents gbNotifications As System.Windows.Forms.GroupBox
    Friend WithEvents lblShutdownNotifyPeriod As System.Windows.Forms.Label
    Friend WithEvents chkShutdownNotify As System.Windows.Forms.CheckBox
    Friend WithEvents nudShutdownNotifyPeriod As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkNotifyEmail As System.Windows.Forms.CheckBox
    Friend WithEvents chkNotifyDialog As System.Windows.Forms.CheckBox
    Friend WithEvents chkNotifyToolTip As System.Windows.Forms.CheckBox
    Friend WithEvents gbEmailOptions As System.Windows.Forms.GroupBox
    Friend WithEvents lblEMailAddress As System.Windows.Forms.Label
    Friend WithEvents txtEmailAddress As System.Windows.Forms.TextBox
    Friend WithEvents txtSMTPServer As System.Windows.Forms.TextBox
    Friend WithEvents lblSMTPServer As System.Windows.Forms.Label
    Friend WithEvents lblEmailPassword As System.Windows.Forms.Label
    Friend WithEvents txtEmailPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtEmailUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblEmailUsername As System.Windows.Forms.Label
    Friend WithEvents chkSMTPAuthentication As System.Windows.Forms.CheckBox
    Friend WithEvents lblNotifyOffset As System.Windows.Forms.Label
    Friend WithEvents trackNotifyOffset As System.Windows.Forms.TrackBar
    Friend WithEvents btnTestEmail As System.Windows.Forms.Button
    Friend WithEvents chkContinueTraining As System.Windows.Forms.CheckBox
    Friend WithEvents tvwSettings As System.Windows.Forms.TreeView
    Friend WithEvents gbColours As System.Windows.Forms.GroupBox
    Friend WithEvents lblPilotTrainingHighlight As System.Windows.Forms.Label
    Friend WithEvents cd1 As System.Windows.Forms.ColorDialog
    Friend WithEvents pbPilotTrainingHighlight As System.Windows.Forms.PictureBox
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRefreshPlugins As System.Windows.Forms.Button
    Friend WithEvents gbTrainingOverlay As System.Windows.Forms.GroupBox
    Friend WithEvents tbTransparancy As System.Windows.Forms.TrackBar
    Friend WithEvents lblTransparancy As System.Windows.Forms.Label
    Friend WithEvents lblPanelColour As System.Windows.Forms.Label
    Friend WithEvents lblBorderColour As System.Windows.Forms.Label
    Friend WithEvents lblTransparancyValue As System.Windows.Forms.Label
    Friend WithEvents pbBorderColour As System.Windows.Forms.PictureBox
    Friend WithEvents pbPanelColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblOverlayXOffset As System.Windows.Forms.Label
    Friend WithEvents lblOverlayOffset As System.Windows.Forms.Label
    Friend WithEvents radBottomRight As System.Windows.Forms.RadioButton
    Friend WithEvents radBottomLeft As System.Windows.Forms.RadioButton
    Friend WithEvents radTopRight As System.Windows.Forms.RadioButton
    Friend WithEvents lblOverlayPosition As System.Windows.Forms.Label
    Friend WithEvents radTopLeft As System.Windows.Forms.RadioButton
    Friend WithEvents nudOverlayYOffset As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudOverlayXOffset As System.Windows.Forms.NumericUpDown
    Friend WithEvents lvlOverlayYOffset As System.Windows.Forms.Label
    Friend WithEvents chkShowOverlayOnStartup As System.Windows.Forms.CheckBox
    Friend WithEvents pbFontColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblFontColour As System.Windows.Forms.Label
    Friend WithEvents chkClickThroughOverlay As System.Windows.Forms.CheckBox
    Friend WithEvents lblMDITabStyle As System.Windows.Forms.Label
    Friend WithEvents cboMDITabStyle As System.Windows.Forms.ComboBox
    Friend WithEvents chkMinimiseOnExit As System.Windows.Forms.CheckBox
    Friend WithEvents pbBothPreReqColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblBothPreReqColour As System.Windows.Forms.Label
    Friend WithEvents pbHasPreReqColour As System.Windows.Forms.PictureBox
    Friend WithEvents pbIsPreReqColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblHasPreReqColour As System.Windows.Forms.Label
    Friend WithEvents lblIsPreReqColour As System.Windows.Forms.Label
    Friend WithEvents lblSkillQueueColours As System.Windows.Forms.Label
    Friend WithEvents pbDowntimeClashColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblDowntimeClashColour As System.Windows.Forms.Label
    Friend WithEvents pbReadySkillColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblReadySkillColour As System.Windows.Forms.Label
    Friend WithEvents chkNotifySound As System.Windows.Forms.CheckBox
    Friend WithEvents lblSoundFile As System.Windows.Forms.Label
    Friend WithEvents btnSelectSoundFile As System.Windows.Forms.Button
    Friend WithEvents lblNotifyMe As System.Windows.Forms.Label
    Friend WithEvents btnSoundTest As System.Windows.Forms.Button
    Friend WithEvents chkNotifyEarly As System.Windows.Forms.CheckBox
    Friend WithEvents chkNotifyNow As System.Windows.Forms.CheckBox
    Friend WithEvents chkDeleteCompletedSkills As System.Windows.Forms.CheckBox
    Friend WithEvents pbPartiallyTrainedColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblPartiallyTrainedColour As System.Windows.Forms.Label
    Friend WithEvents txtMySQLDatabase As System.Windows.Forms.TextBox
    Friend WithEvents lblMySQLDatabase As System.Windows.Forms.Label
    Friend WithEvents txtMSSQLDatabase As System.Windows.Forms.TextBox
    Friend WithEvents lblMSSQLDatabase As System.Windows.Forms.Label
    Friend WithEvents gbG15 As System.Windows.Forms.GroupBox
    Friend WithEvents chkActivateG15 As System.Windows.Forms.CheckBox
    Friend WithEvents nudCycleTime As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblCycleTime As System.Windows.Forms.Label
    Friend WithEvents chkCyclePilots As System.Windows.Forms.CheckBox
    Friend WithEvents gbLocation4 As System.Windows.Forms.GroupBox
    Friend WithEvents gbLocation3 As System.Windows.Forms.GroupBox
    Friend WithEvents gbLocation2 As System.Windows.Forms.GroupBox
    Friend WithEvents gbLocation1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkLUA4 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLUA3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLUA2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLUA1 As System.Windows.Forms.CheckBox
    Friend WithEvents lblCacheSize4 As System.Windows.Forms.Label
    Friend WithEvents lblCacheSize3 As System.Windows.Forms.Label
    Friend WithEvents lblCacheSize2 As System.Windows.Forms.Label
    Friend WithEvents lblCacheSize1 As System.Windows.Forms.Label
    Friend WithEvents btnTidyPlugins As System.Windows.Forms.Button
    Friend WithEvents gbMarketPrices As System.Windows.Forms.GroupBox
    Friend WithEvents lblMarketPriceStats As System.Windows.Forms.Label
    Friend WithEvents lblUpdateStatus As System.Windows.Forms.Label
    Friend WithEvents lblUpdateStatusLabel As System.Windows.Forms.Label
    Friend WithEvents lblLastUpdateTime As System.Windows.Forms.Label
    Friend WithEvents lblLastUpdate As System.Windows.Forms.Label
    Friend WithEvents btnUpdatePrices As System.Windows.Forms.Button
    Friend WithEvents lvwPrices As EveHQ.ListViewNoFlicker
    Friend WithEvents colPriceName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBasePrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents colMarketPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCustomPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxPrices As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPriceItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPriceAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtUpdatePrice As System.Windows.Forms.TextBox
    Friend WithEvents lblUpdatePrice As System.Windows.Forms.Label
    Friend WithEvents gbPanelColours As System.Windows.Forms.GroupBox
    Friend WithEvents pbPanelHighlight As System.Windows.Forms.PictureBox
    Friend WithEvents lblPanelHighlight As System.Windows.Forms.Label
    Friend WithEvents pbPanelText As System.Windows.Forms.PictureBox
    Friend WithEvents lblPanelText As System.Windows.Forms.Label
    Friend WithEvents pbPanelRight As System.Windows.Forms.PictureBox
    Friend WithEvents lblPanelRight As System.Windows.Forms.Label
    Friend WithEvents pbPanelLeft As System.Windows.Forms.PictureBox
    Friend WithEvents lblPanelLeft As System.Windows.Forms.Label
    Friend WithEvents pbPanelBottomRight As System.Windows.Forms.PictureBox
    Friend WithEvents lblPanelBottomRight As System.Windows.Forms.Label
    Friend WithEvents pbPanelTopLeft As System.Windows.Forms.PictureBox
    Friend WithEvents lblPanelTopLeft As System.Windows.Forms.Label
    Friend WithEvents pbPanelOutline As System.Windows.Forms.PictureBox
    Friend WithEvents lblPanelOutline As System.Windows.Forms.Label
    Friend WithEvents pbPanelBackground As System.Windows.Forms.PictureBox
    Friend WithEvents lblPanelBackground As System.Windows.Forms.Label
    Friend WithEvents btnResetPanelColours As System.Windows.Forms.Button
    Friend WithEvents gbPilotScreenColours As System.Windows.Forms.GroupBox
    Friend WithEvents btnResetPilotColours As System.Windows.Forms.Button
    Friend WithEvents pbPilotLevel5 As System.Windows.Forms.PictureBox
    Friend WithEvents lblLevel5Colour As System.Windows.Forms.Label
    Friend WithEvents pbPilotPartial As System.Windows.Forms.PictureBox
    Friend WithEvents lblPilotPartiallyTrainedColour As System.Windows.Forms.Label
    Friend WithEvents pbPilotCurrent As System.Windows.Forms.PictureBox
    Friend WithEvents lblPilotCurrentColour As System.Windows.Forms.Label
    Friend WithEvents pbPilotStandard As System.Windows.Forms.PictureBox
    Friend WithEvents lblPilotStandardColour As System.Windows.Forms.Label
    Friend WithEvents lblFriendlyName1 As System.Windows.Forms.Label
    Friend WithEvents txtFriendlyName1 As System.Windows.Forms.TextBox
    Friend WithEvents lblFriendlyName4 As System.Windows.Forms.Label
    Friend WithEvents txtFriendlyName4 As System.Windows.Forms.TextBox
    Friend WithEvents lblFriendlyName3 As System.Windows.Forms.Label
    Friend WithEvents txtFriendlyName3 As System.Windows.Forms.TextBox
    Friend WithEvents lblFriendlyName2 As System.Windows.Forms.Label
    Friend WithEvents txtFriendlyName2 As System.Windows.Forms.TextBox
    Friend WithEvents txtSearchPrices As System.Windows.Forms.TextBox
    Friend WithEvents lblSearchPrices As System.Windows.Forms.Label
    Friend WithEvents btnResetGrid As System.Windows.Forms.Button
    Friend WithEvents gbAPIRelayServer As System.Windows.Forms.GroupBox
    Friend WithEvents nudAPIRSPort As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblAPIRSPort As System.Windows.Forms.Label
    Friend WithEvents chkActivateAPIRS As System.Windows.Forms.CheckBox
    Friend WithEvents chkAPIRSAutoStart As System.Windows.Forms.CheckBox
    Friend WithEvents gbAPIServer As System.Windows.Forms.GroupBox
    Friend WithEvents txtAPIRSServer As System.Windows.Forms.TextBox
    Friend WithEvents lblAPIRSServer As System.Windows.Forms.Label
    Friend WithEvents txtCCPAPIServer As System.Windows.Forms.TextBox
    Friend WithEvents lblCCPAPIServer As System.Windows.Forms.Label
    Friend WithEvents chkAutoAPI As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseCCPBackup As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseAPIRSServer As System.Windows.Forms.CheckBox
    Friend WithEvents lblUpdateLocation As System.Windows.Forms.Label
    Friend WithEvents txtUpdateLocation As System.Windows.Forms.TextBox
    Friend WithEvents chkOmitCurrentSkill As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowAPIStatusForm As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseAppDirForDB As System.Windows.Forms.CheckBox
End Class
