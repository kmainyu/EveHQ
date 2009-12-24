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
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Colours & Styles")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Dashboard")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Database Format")
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("E-Mail")
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Eve Accounts")
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Eve Folders")
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Eve API & Server")
        Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("FTP Accounts")
        Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("G15 Display")
        Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("IGB")
        Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Notifications")
        Dim TreeNode13 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Pilots")
        Dim TreeNode14 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Plug Ins")
        Dim TreeNode15 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Proxy Server")
        Dim TreeNode16 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Taskbar Icon")
        Dim TreeNode17 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Training Queue")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.gbGeneral = New System.Windows.Forms.GroupBox
        Me.chkDisableAutoConnections = New System.Windows.Forms.CheckBox
        Me.lblTrainingBarPosition = New System.Windows.Forms.Label
        Me.cboTrainingBarPosition = New System.Windows.Forms.ComboBox
        Me.lblToolbarPosition = New System.Windows.Forms.Label
        Me.cboToolbarPosition = New System.Windows.Forms.ComboBox
        Me.lblMDITabPosition = New System.Windows.Forms.Label
        Me.cboMDITabPosition = New System.Windows.Forms.ComboBox
        Me.txtErrorRepEmail = New System.Windows.Forms.TextBox
        Me.lblErrorRepEmail = New System.Windows.Forms.Label
        Me.txtErrorRepName = New System.Windows.Forms.TextBox
        Me.lblErrorRepName = New System.Windows.Forms.Label
        Me.chkErrorReporting = New System.Windows.Forms.CheckBox
        Me.txtUpdateLocation = New System.Windows.Forms.TextBox
        Me.lblUpdateLocation = New System.Windows.Forms.Label
        Me.chkMinimiseOnExit = New System.Windows.Forms.CheckBox
        Me.lblMDITabStyle = New System.Windows.Forms.Label
        Me.cboMDITabStyle = New System.Windows.Forms.ComboBox
        Me.chkEncryptSettings = New System.Windows.Forms.CheckBox
        Me.cboStartupPilot = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cboStartupView = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkAutoMinimise = New System.Windows.Forms.CheckBox
        Me.chkAutoRun = New System.Windows.Forms.CheckBox
        Me.chkAutoHide = New System.Windows.Forms.CheckBox
        Me.gbPilotScreenColours = New System.Windows.Forms.GroupBox
        Me.pbPilotSkillHighlight = New System.Windows.Forms.PictureBox
        Me.lblPilotSkillHighlight = New System.Windows.Forms.Label
        Me.pbPilotSkillText = New System.Windows.Forms.PictureBox
        Me.lblPilotSkillText = New System.Windows.Forms.Label
        Me.pbPilotGroupText = New System.Windows.Forms.PictureBox
        Me.lblPilotGroupText = New System.Windows.Forms.Label
        Me.pbPilotGroupBG = New System.Windows.Forms.PictureBox
        Me.lblPilotGroupBG = New System.Windows.Forms.Label
        Me.btnResetPilotColours = New System.Windows.Forms.Button
        Me.pbPilotLevel5 = New System.Windows.Forms.PictureBox
        Me.lblLevel5Colour = New System.Windows.Forms.Label
        Me.pbPilotPartial = New System.Windows.Forms.PictureBox
        Me.lblPilotPartiallyTrainedColour = New System.Windows.Forms.Label
        Me.pbPilotCurrent = New System.Windows.Forms.PictureBox
        Me.lblPilotCurrentColour = New System.Windows.Forms.Label
        Me.pbPilotStandard = New System.Windows.Forms.PictureBox
        Me.lblPilotStandardColour = New System.Windows.Forms.Label
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
        Me.btnMoveDown = New System.Windows.Forms.Button
        Me.btnMoveUp = New System.Windows.Forms.Button
        Me.lvwColumns = New System.Windows.Forms.ListView
        Me.colQueueColumns = New System.Windows.Forms.ColumnHeader
        Me.chkShowCompletedSkills = New System.Windows.Forms.CheckBox
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
        Me.lblQueueColumns = New System.Windows.Forms.Label
        Me.gbDatabaseFormat = New System.Windows.Forms.GroupBox
        Me.nudDBTimeout = New System.Windows.Forms.NumericUpDown
        Me.lblDatabaseTimeout = New System.Windows.Forms.Label
        Me.btnTestDB = New System.Windows.Forms.Button
        Me.cboFormat = New System.Windows.Forms.ComboBox
        Me.lblFormat = New System.Windows.Forms.Label
        Me.gbAccess = New System.Windows.Forms.GroupBox
        Me.chkUseAppDirForDB = New System.Windows.Forms.CheckBox
        Me.btnBrowseMDB = New System.Windows.Forms.Button
        Me.txtMDBPassword = New System.Windows.Forms.TextBox
        Me.txtMDBUsername = New System.Windows.Forms.TextBox
        Me.txtMDBServer = New System.Windows.Forms.TextBox
        Me.lblMDBPassword = New System.Windows.Forms.Label
        Me.lblMDBUser = New System.Windows.Forms.Label
        Me.lblMDBFilename = New System.Windows.Forms.Label
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
        Me.chkAutoMailAPI = New System.Windows.Forms.CheckBox
        Me.gbAPIServer = New System.Windows.Forms.GroupBox
        Me.txtAPIFileExtension = New System.Windows.Forms.TextBox
        Me.lblAPIFileExtension = New System.Windows.Forms.Label
        Me.chkUseCCPBackup = New System.Windows.Forms.CheckBox
        Me.chkUseAPIRSServer = New System.Windows.Forms.CheckBox
        Me.txtAPIRSServer = New System.Windows.Forms.TextBox
        Me.lblAPIRSServer = New System.Windows.Forms.Label
        Me.txtCCPAPIServer = New System.Windows.Forms.TextBox
        Me.lblCCPAPIServer = New System.Windows.Forms.Label
        Me.gbAPIRelayServer = New System.Windows.Forms.GroupBox
        Me.chkAPIRSAutoStart = New System.Windows.Forms.CheckBox
        Me.nudAPIRSPort = New System.Windows.Forms.NumericUpDown
        Me.lblAPIRSPort = New System.Windows.Forms.Label
        Me.chkActivateAPIRS = New System.Windows.Forms.CheckBox
        Me.chkEnableEveStatus = New System.Windows.Forms.CheckBox
        Me.lblCurrentOffset = New System.Windows.Forms.Label
        Me.lblServerOffset = New System.Windows.Forms.Label
        Me.trackServerOffset = New System.Windows.Forms.TrackBar
        Me.chkAutoAPI = New System.Windows.Forms.CheckBox
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
        Me.chkNotifyEmail = New System.Windows.Forms.CheckBox
        Me.chkNotifyDialog = New System.Windows.Forms.CheckBox
        Me.chkNotifyToolTip = New System.Windows.Forms.CheckBox
        Me.nudShutdownNotifyPeriod = New System.Windows.Forms.NumericUpDown
        Me.lblShutdownNotifyPeriod = New System.Windows.Forms.Label
        Me.chkShutdownNotify = New System.Windows.Forms.CheckBox
        Me.gbEmail = New System.Windows.Forms.GroupBox
        Me.lblSenderAddress = New System.Windows.Forms.Label
        Me.txtSenderAddress = New System.Windows.Forms.TextBox
        Me.txtSMTPPort = New System.Windows.Forms.TextBox
        Me.lblSMTPPort = New System.Windows.Forms.Label
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
        Me.btnClose = New System.Windows.Forms.Button
        Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog
        Me.tvwSettings = New System.Windows.Forms.TreeView
        Me.gbColours = New System.Windows.Forms.GroupBox
        Me.txtCSVSeparator = New System.Windows.Forms.TextBox
        Me.lblCSVSeparatorChar = New System.Windows.Forms.Label
        Me.chkDisableVisualStyles = New System.Windows.Forms.CheckBox
        Me.cd1 = New System.Windows.Forms.ColorDialog
        Me.gbG15 = New System.Windows.Forms.GroupBox
        Me.nudCycleTime = New System.Windows.Forms.NumericUpDown
        Me.lblCycleTime = New System.Windows.Forms.Label
        Me.chkCyclePilots = New System.Windows.Forms.CheckBox
        Me.chkActivateG15 = New System.Windows.Forms.CheckBox
        Me.ctxPrices = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPriceItemName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPriceAdd = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPriceEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPriceDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.gbTaskbarIcon = New System.Windows.Forms.GroupBox
        Me.cboTaskbarIconMode = New System.Windows.Forms.ComboBox
        Me.lblTaskbarIconMode = New System.Windows.Forms.Label
        Me.gbDashboard = New System.Windows.Forms.GroupBox
        Me.gbOtherDBOptions = New System.Windows.Forms.GroupBox
        Me.cboTickerLocation = New System.Windows.Forms.ComboBox
        Me.lblTickerLocation = New System.Windows.Forms.Label
        Me.chkShowPriceTicker = New System.Windows.Forms.CheckBox
        Me.dbDashboardConfig = New System.Windows.Forms.GroupBox
        Me.lblWidgetTypes = New System.Windows.Forms.Label
        Me.cboWidgets = New System.Windows.Forms.ComboBox
        Me.btnAddWidget = New System.Windows.Forms.Button
        Me.btnRemoveWidget = New System.Windows.Forms.Button
        Me.lvWidgets = New System.Windows.Forms.ListView
        Me.colWidgetType = New System.Windows.Forms.ColumnHeader
        Me.colWidgetInfo = New System.Windows.Forms.ColumnHeader
        Me.lblCurrentWidgets = New System.Windows.Forms.Label
        Me.gbDashboardColours = New System.Windows.Forms.GroupBox
        Me.pbWidgetHeader2 = New System.Windows.Forms.PictureBox
        Me.lblWidgetHeader2 = New System.Windows.Forms.Label
        Me.pbWidgetHeader1 = New System.Windows.Forms.PictureBox
        Me.lblWidgetHeader1 = New System.Windows.Forms.Label
        Me.pbWidgetBorder = New System.Windows.Forms.PictureBox
        Me.lblWidgetBorder = New System.Windows.Forms.Label
        Me.pbDBColor = New System.Windows.Forms.PictureBox
        Me.lblDBColor = New System.Windows.Forms.Label
        Me.btnResetDBColors = New System.Windows.Forms.Button
        Me.pbWidgetMain2 = New System.Windows.Forms.PictureBox
        Me.lblWidgetMain2 = New System.Windows.Forms.Label
        Me.pbWidgetMain1 = New System.Windows.Forms.PictureBox
        Me.lblWidgetMain1 = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkNotifyEveMail = New System.Windows.Forms.CheckBox
        Me.chkNotifyNotification = New System.Windows.Forms.CheckBox
        Me.gbGeneral.SuspendLayout()
        Me.gbPilotScreenColours.SuspendLayout()
        CType(Me.pbPilotSkillHighlight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotSkillText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotGroupText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotGroupBG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotLevel5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotPartial, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotCurrent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilotStandard, System.ComponentModel.ISupportInitialize).BeginInit()
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
        CType(Me.nudDBTimeout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbAccess.SuspendLayout()
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
        CType(Me.nudShutdownNotifyPeriod, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbEmail.SuspendLayout()
        Me.gbColours.SuspendLayout()
        Me.gbG15.SuspendLayout()
        CType(Me.nudCycleTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxPrices.SuspendLayout()
        Me.gbTaskbarIcon.SuspendLayout()
        Me.gbDashboard.SuspendLayout()
        Me.gbOtherDBOptions.SuspendLayout()
        Me.dbDashboardConfig.SuspendLayout()
        Me.gbDashboardColours.SuspendLayout()
        CType(Me.pbWidgetHeader2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbWidgetHeader1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbWidgetBorder, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDBColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbWidgetMain2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbWidgetMain1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbGeneral
        '
        Me.gbGeneral.Controls.Add(Me.chkDisableAutoConnections)
        Me.gbGeneral.Controls.Add(Me.lblTrainingBarPosition)
        Me.gbGeneral.Controls.Add(Me.cboTrainingBarPosition)
        Me.gbGeneral.Controls.Add(Me.lblToolbarPosition)
        Me.gbGeneral.Controls.Add(Me.cboToolbarPosition)
        Me.gbGeneral.Controls.Add(Me.lblMDITabPosition)
        Me.gbGeneral.Controls.Add(Me.cboMDITabPosition)
        Me.gbGeneral.Controls.Add(Me.txtErrorRepEmail)
        Me.gbGeneral.Controls.Add(Me.lblErrorRepEmail)
        Me.gbGeneral.Controls.Add(Me.txtErrorRepName)
        Me.gbGeneral.Controls.Add(Me.lblErrorRepName)
        Me.gbGeneral.Controls.Add(Me.chkErrorReporting)
        Me.gbGeneral.Controls.Add(Me.txtUpdateLocation)
        Me.gbGeneral.Controls.Add(Me.lblUpdateLocation)
        Me.gbGeneral.Controls.Add(Me.chkMinimiseOnExit)
        Me.gbGeneral.Controls.Add(Me.lblMDITabStyle)
        Me.gbGeneral.Controls.Add(Me.cboMDITabStyle)
        Me.gbGeneral.Controls.Add(Me.chkEncryptSettings)
        Me.gbGeneral.Controls.Add(Me.cboStartupPilot)
        Me.gbGeneral.Controls.Add(Me.Label3)
        Me.gbGeneral.Controls.Add(Me.cboStartupView)
        Me.gbGeneral.Controls.Add(Me.Label2)
        Me.gbGeneral.Controls.Add(Me.chkAutoMinimise)
        Me.gbGeneral.Controls.Add(Me.chkAutoRun)
        Me.gbGeneral.Controls.Add(Me.chkAutoHide)
        Me.gbGeneral.Location = New System.Drawing.Point(517, 184)
        Me.gbGeneral.Name = "gbGeneral"
        Me.gbGeneral.Size = New System.Drawing.Size(113, 36)
        Me.gbGeneral.TabIndex = 1
        Me.gbGeneral.TabStop = False
        Me.gbGeneral.Text = "General Settings"
        Me.gbGeneral.Visible = False
        '
        'chkDisableAutoConnections
        '
        Me.chkDisableAutoConnections.AutoSize = True
        Me.chkDisableAutoConnections.Location = New System.Drawing.Point(24, 149)
        Me.chkDisableAutoConnections.Name = "chkDisableAutoConnections"
        Me.chkDisableAutoConnections.Size = New System.Drawing.Size(198, 17)
        Me.chkDisableAutoConnections.TabIndex = 51
        Me.chkDisableAutoConnections.Text = "Disable Automatic Web Connections"
        Me.chkDisableAutoConnections.UseVisualStyleBackColor = True
        '
        'lblTrainingBarPosition
        '
        Me.lblTrainingBarPosition.AutoSize = True
        Me.lblTrainingBarPosition.Location = New System.Drawing.Point(313, 170)
        Me.lblTrainingBarPosition.Name = "lblTrainingBarPosition"
        Me.lblTrainingBarPosition.Size = New System.Drawing.Size(89, 13)
        Me.lblTrainingBarPosition.TabIndex = 50
        Me.lblTrainingBarPosition.Text = "Training Position:"
        '
        'cboTrainingBarPosition
        '
        Me.cboTrainingBarPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTrainingBarPosition.FormattingEnabled = True
        Me.cboTrainingBarPosition.Items.AddRange(New Object() {"Bottom", "Left", "None", "Right", "Top"})
        Me.cboTrainingBarPosition.Location = New System.Drawing.Point(411, 167)
        Me.cboTrainingBarPosition.Name = "cboTrainingBarPosition"
        Me.cboTrainingBarPosition.Size = New System.Drawing.Size(161, 21)
        Me.cboTrainingBarPosition.Sorted = True
        Me.cboTrainingBarPosition.TabIndex = 49
        '
        'lblToolbarPosition
        '
        Me.lblToolbarPosition.AutoSize = True
        Me.lblToolbarPosition.Location = New System.Drawing.Point(313, 143)
        Me.lblToolbarPosition.Name = "lblToolbarPosition"
        Me.lblToolbarPosition.Size = New System.Drawing.Size(87, 13)
        Me.lblToolbarPosition.TabIndex = 48
        Me.lblToolbarPosition.Text = "Toolbar Position:"
        '
        'cboToolbarPosition
        '
        Me.cboToolbarPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboToolbarPosition.FormattingEnabled = True
        Me.cboToolbarPosition.Items.AddRange(New Object() {"Bottom", "Left", "Right", "Top"})
        Me.cboToolbarPosition.Location = New System.Drawing.Point(411, 140)
        Me.cboToolbarPosition.Name = "cboToolbarPosition"
        Me.cboToolbarPosition.Size = New System.Drawing.Size(161, 21)
        Me.cboToolbarPosition.Sorted = True
        Me.cboToolbarPosition.TabIndex = 47
        '
        'lblMDITabPosition
        '
        Me.lblMDITabPosition.AutoSize = True
        Me.lblMDITabPosition.Location = New System.Drawing.Point(313, 116)
        Me.lblMDITabPosition.Name = "lblMDITabPosition"
        Me.lblMDITabPosition.Size = New System.Drawing.Size(91, 13)
        Me.lblMDITabPosition.TabIndex = 46
        Me.lblMDITabPosition.Text = "MDI Tab Position:"
        '
        'cboMDITabPosition
        '
        Me.cboMDITabPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMDITabPosition.FormattingEnabled = True
        Me.cboMDITabPosition.Items.AddRange(New Object() {"Bottom", "Top"})
        Me.cboMDITabPosition.Location = New System.Drawing.Point(411, 113)
        Me.cboMDITabPosition.Name = "cboMDITabPosition"
        Me.cboMDITabPosition.Size = New System.Drawing.Size(161, 21)
        Me.cboMDITabPosition.Sorted = True
        Me.cboMDITabPosition.TabIndex = 45
        '
        'txtErrorRepEmail
        '
        Me.txtErrorRepEmail.Enabled = False
        Me.txtErrorRepEmail.Location = New System.Drawing.Point(144, 317)
        Me.txtErrorRepEmail.Name = "txtErrorRepEmail"
        Me.txtErrorRepEmail.Size = New System.Drawing.Size(247, 21)
        Me.txtErrorRepEmail.TabIndex = 44
        '
        'lblErrorRepEmail
        '
        Me.lblErrorRepEmail.AutoSize = True
        Me.lblErrorRepEmail.Enabled = False
        Me.lblErrorRepEmail.Location = New System.Drawing.Point(56, 320)
        Me.lblErrorRepEmail.Name = "lblErrorRepEmail"
        Me.lblErrorRepEmail.Size = New System.Drawing.Size(86, 13)
        Me.lblErrorRepEmail.TabIndex = 43
        Me.lblErrorRepEmail.Text = "Email (Optional):"
        '
        'txtErrorRepName
        '
        Me.txtErrorRepName.Enabled = False
        Me.txtErrorRepName.Location = New System.Drawing.Point(144, 291)
        Me.txtErrorRepName.Name = "txtErrorRepName"
        Me.txtErrorRepName.Size = New System.Drawing.Size(247, 21)
        Me.txtErrorRepName.TabIndex = 42
        '
        'lblErrorRepName
        '
        Me.lblErrorRepName.AutoSize = True
        Me.lblErrorRepName.Enabled = False
        Me.lblErrorRepName.Location = New System.Drawing.Point(56, 294)
        Me.lblErrorRepName.Name = "lblErrorRepName"
        Me.lblErrorRepName.Size = New System.Drawing.Size(89, 13)
        Me.lblErrorRepName.TabIndex = 41
        Me.lblErrorRepName.Text = "Name (Optional):"
        '
        'chkErrorReporting
        '
        Me.chkErrorReporting.AutoSize = True
        Me.chkErrorReporting.Location = New System.Drawing.Point(24, 268)
        Me.chkErrorReporting.Name = "chkErrorReporting"
        Me.chkErrorReporting.Size = New System.Drawing.Size(190, 17)
        Me.chkErrorReporting.TabIndex = 40
        Me.chkErrorReporting.Text = "Enable Integrated Error Reporting"
        Me.chkErrorReporting.UseVisualStyleBackColor = True
        '
        'txtUpdateLocation
        '
        Me.txtUpdateLocation.Location = New System.Drawing.Point(116, 226)
        Me.txtUpdateLocation.Name = "txtUpdateLocation"
        Me.txtUpdateLocation.Size = New System.Drawing.Size(449, 21)
        Me.txtUpdateLocation.TabIndex = 39
        '
        'lblUpdateLocation
        '
        Me.lblUpdateLocation.AutoSize = True
        Me.lblUpdateLocation.Location = New System.Drawing.Point(21, 229)
        Me.lblUpdateLocation.Name = "lblUpdateLocation"
        Me.lblUpdateLocation.Size = New System.Drawing.Size(89, 13)
        Me.lblUpdateLocation.TabIndex = 38
        Me.lblUpdateLocation.Text = "Update Location:"
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
        Me.cboMDITabStyle.Location = New System.Drawing.Point(411, 86)
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
        Me.chkEncryptSettings.Size = New System.Drawing.Size(124, 17)
        Me.chkEncryptSettings.TabIndex = 8
        Me.chkEncryptSettings.Text = "Encrypt Settings File"
        Me.chkEncryptSettings.UseVisualStyleBackColor = True
        '
        'cboStartupPilot
        '
        Me.cboStartupPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStartupPilot.FormattingEnabled = True
        Me.cboStartupPilot.Location = New System.Drawing.Point(411, 59)
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
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Default Pilot:"
        '
        'cboStartupView
        '
        Me.cboStartupView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStartupView.FormattingEnabled = True
        Me.cboStartupView.Items.AddRange(New Object() {"EveHQ Dashboard", "Pilot Information", "Pilot Summary Report", "Skill Training"})
        Me.cboStartupView.Location = New System.Drawing.Point(411, 32)
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
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "View on Startup:"
        '
        'chkAutoMinimise
        '
        Me.chkAutoMinimise.AutoSize = True
        Me.chkAutoMinimise.Location = New System.Drawing.Point(24, 57)
        Me.chkAutoMinimise.Name = "chkAutoMinimise"
        Me.chkAutoMinimise.Size = New System.Drawing.Size(164, 17)
        Me.chkAutoMinimise.TabIndex = 1
        Me.chkAutoMinimise.Text = "Minimise When EveHQ Starts"
        Me.chkAutoMinimise.UseVisualStyleBackColor = True
        '
        'chkAutoRun
        '
        Me.chkAutoRun.AutoSize = True
        Me.chkAutoRun.Location = New System.Drawing.Point(24, 103)
        Me.chkAutoRun.Name = "chkAutoRun"
        Me.chkAutoRun.Size = New System.Drawing.Size(181, 17)
        Me.chkAutoRun.TabIndex = 2
        Me.chkAutoRun.Text = "Run EveHQ on Windows Startup"
        Me.chkAutoRun.UseVisualStyleBackColor = True
        '
        'chkAutoHide
        '
        Me.chkAutoHide.AutoSize = True
        Me.chkAutoHide.Location = New System.Drawing.Point(24, 34)
        Me.chkAutoHide.Name = "chkAutoHide"
        Me.chkAutoHide.Size = New System.Drawing.Size(247, 17)
        Me.chkAutoHide.TabIndex = 0
        Me.chkAutoHide.Text = "Hide EveHQ from the Taskbar when Minimising"
        Me.chkAutoHide.UseVisualStyleBackColor = True
        '
        'gbPilotScreenColours
        '
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotSkillHighlight)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotSkillHighlight)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotSkillText)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotSkillText)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotGroupText)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotGroupText)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotGroupBG)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotGroupBG)
        Me.gbPilotScreenColours.Controls.Add(Me.btnResetPilotColours)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotLevel5)
        Me.gbPilotScreenColours.Controls.Add(Me.lblLevel5Colour)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotPartial)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotPartiallyTrainedColour)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotCurrent)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotCurrentColour)
        Me.gbPilotScreenColours.Controls.Add(Me.pbPilotStandard)
        Me.gbPilotScreenColours.Controls.Add(Me.lblPilotStandardColour)
        Me.gbPilotScreenColours.Location = New System.Drawing.Point(17, 26)
        Me.gbPilotScreenColours.Name = "gbPilotScreenColours"
        Me.gbPilotScreenColours.Size = New System.Drawing.Size(215, 257)
        Me.gbPilotScreenColours.TabIndex = 37
        Me.gbPilotScreenColours.TabStop = False
        Me.gbPilotScreenColours.Text = "Pilot Screen Skill Colours"
        '
        'pbPilotSkillHighlight
        '
        Me.pbPilotSkillHighlight.BackColor = System.Drawing.Color.DodgerBlue
        Me.pbPilotSkillHighlight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotSkillHighlight.Location = New System.Drawing.Point(159, 96)
        Me.pbPilotSkillHighlight.Name = "pbPilotSkillHighlight"
        Me.pbPilotSkillHighlight.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotSkillHighlight.TabIndex = 60
        Me.pbPilotSkillHighlight.TabStop = False
        '
        'lblPilotSkillHighlight
        '
        Me.lblPilotSkillHighlight.AutoSize = True
        Me.lblPilotSkillHighlight.Location = New System.Drawing.Point(13, 99)
        Me.lblPilotSkillHighlight.Name = "lblPilotSkillHighlight"
        Me.lblPilotSkillHighlight.Size = New System.Drawing.Size(68, 13)
        Me.lblPilotSkillHighlight.TabIndex = 59
        Me.lblPilotSkillHighlight.Text = "Skill Highlight"
        '
        'pbPilotSkillText
        '
        Me.pbPilotSkillText.BackColor = System.Drawing.Color.Black
        Me.pbPilotSkillText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotSkillText.Location = New System.Drawing.Point(159, 72)
        Me.pbPilotSkillText.Name = "pbPilotSkillText"
        Me.pbPilotSkillText.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotSkillText.TabIndex = 58
        Me.pbPilotSkillText.TabStop = False
        '
        'lblPilotSkillText
        '
        Me.lblPilotSkillText.AutoSize = True
        Me.lblPilotSkillText.Location = New System.Drawing.Point(13, 75)
        Me.lblPilotSkillText.Name = "lblPilotSkillText"
        Me.lblPilotSkillText.Size = New System.Drawing.Size(49, 13)
        Me.lblPilotSkillText.TabIndex = 57
        Me.lblPilotSkillText.Text = "Skill Text"
        '
        'pbPilotGroupText
        '
        Me.pbPilotGroupText.BackColor = System.Drawing.Color.White
        Me.pbPilotGroupText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotGroupText.Location = New System.Drawing.Point(159, 48)
        Me.pbPilotGroupText.Name = "pbPilotGroupText"
        Me.pbPilotGroupText.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotGroupText.TabIndex = 56
        Me.pbPilotGroupText.TabStop = False
        '
        'lblPilotGroupText
        '
        Me.lblPilotGroupText.AutoSize = True
        Me.lblPilotGroupText.Location = New System.Drawing.Point(13, 51)
        Me.lblPilotGroupText.Name = "lblPilotGroupText"
        Me.lblPilotGroupText.Size = New System.Drawing.Size(61, 13)
        Me.lblPilotGroupText.TabIndex = 55
        Me.lblPilotGroupText.Text = "Group Text"
        '
        'pbPilotGroupBG
        '
        Me.pbPilotGroupBG.BackColor = System.Drawing.Color.DimGray
        Me.pbPilotGroupBG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotGroupBG.Location = New System.Drawing.Point(159, 24)
        Me.pbPilotGroupBG.Name = "pbPilotGroupBG"
        Me.pbPilotGroupBG.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotGroupBG.TabIndex = 54
        Me.pbPilotGroupBG.TabStop = False
        '
        'lblPilotGroupBG
        '
        Me.lblPilotGroupBG.AutoSize = True
        Me.lblPilotGroupBG.Location = New System.Drawing.Point(13, 27)
        Me.lblPilotGroupBG.Name = "lblPilotGroupBG"
        Me.lblPilotGroupBG.Size = New System.Drawing.Size(95, 13)
        Me.lblPilotGroupBG.TabIndex = 53
        Me.lblPilotGroupBG.Text = "Group Background"
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
        Me.pbPilotLevel5.Location = New System.Drawing.Point(159, 192)
        Me.pbPilotLevel5.Name = "pbPilotLevel5"
        Me.pbPilotLevel5.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotLevel5.TabIndex = 43
        Me.pbPilotLevel5.TabStop = False
        '
        'lblLevel5Colour
        '
        Me.lblLevel5Colour.AutoSize = True
        Me.lblLevel5Colour.Location = New System.Drawing.Point(13, 195)
        Me.lblLevel5Colour.Name = "lblLevel5Colour"
        Me.lblLevel5Colour.Size = New System.Drawing.Size(61, 13)
        Me.lblLevel5Colour.TabIndex = 42
        Me.lblLevel5Colour.Text = "Level 5 Skill"
        '
        'pbPilotPartial
        '
        Me.pbPilotPartial.BackColor = System.Drawing.Color.Gold
        Me.pbPilotPartial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotPartial.Location = New System.Drawing.Point(159, 168)
        Me.pbPilotPartial.Name = "pbPilotPartial"
        Me.pbPilotPartial.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotPartial.TabIndex = 41
        Me.pbPilotPartial.TabStop = False
        '
        'lblPilotPartiallyTrainedColour
        '
        Me.lblPilotPartiallyTrainedColour.AutoSize = True
        Me.lblPilotPartiallyTrainedColour.Location = New System.Drawing.Point(13, 171)
        Me.lblPilotPartiallyTrainedColour.Name = "lblPilotPartiallyTrainedColour"
        Me.lblPilotPartiallyTrainedColour.Size = New System.Drawing.Size(104, 13)
        Me.lblPilotPartiallyTrainedColour.TabIndex = 40
        Me.lblPilotPartiallyTrainedColour.Text = "Partially Trained Skill"
        '
        'pbPilotCurrent
        '
        Me.pbPilotCurrent.BackColor = System.Drawing.Color.LimeGreen
        Me.pbPilotCurrent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotCurrent.Location = New System.Drawing.Point(159, 144)
        Me.pbPilotCurrent.Name = "pbPilotCurrent"
        Me.pbPilotCurrent.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotCurrent.TabIndex = 39
        Me.pbPilotCurrent.TabStop = False
        '
        'lblPilotCurrentColour
        '
        Me.lblPilotCurrentColour.AutoSize = True
        Me.lblPilotCurrentColour.Location = New System.Drawing.Point(13, 147)
        Me.lblPilotCurrentColour.Name = "lblPilotCurrentColour"
        Me.lblPilotCurrentColour.Size = New System.Drawing.Size(105, 13)
        Me.lblPilotCurrentColour.TabIndex = 38
        Me.lblPilotCurrentColour.Text = "Current Training Skill"
        '
        'pbPilotStandard
        '
        Me.pbPilotStandard.BackColor = System.Drawing.Color.White
        Me.pbPilotStandard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPilotStandard.Location = New System.Drawing.Point(159, 120)
        Me.pbPilotStandard.Name = "pbPilotStandard"
        Me.pbPilotStandard.Size = New System.Drawing.Size(24, 24)
        Me.pbPilotStandard.TabIndex = 37
        Me.pbPilotStandard.TabStop = False
        '
        'lblPilotStandardColour
        '
        Me.lblPilotStandardColour.AutoSize = True
        Me.lblPilotStandardColour.Location = New System.Drawing.Point(13, 123)
        Me.lblPilotStandardColour.Name = "lblPilotStandardColour"
        Me.lblPilotStandardColour.Size = New System.Drawing.Size(71, 13)
        Me.lblPilotStandardColour.TabIndex = 36
        Me.lblPilotStandardColour.Text = "Standard Skill"
        '
        'gbEveAccounts
        '
        Me.gbEveAccounts.BackColor = System.Drawing.Color.Transparent
        Me.gbEveAccounts.Controls.Add(Me.btnGetData)
        Me.gbEveAccounts.Controls.Add(Me.btnDeleteAccount)
        Me.gbEveAccounts.Controls.Add(Me.btnEditAccount)
        Me.gbEveAccounts.Controls.Add(Me.btnAddAccount)
        Me.gbEveAccounts.Controls.Add(Me.lvAccounts)
        Me.gbEveAccounts.Location = New System.Drawing.Point(615, 104)
        Me.gbEveAccounts.Name = "gbEveAccounts"
        Me.gbEveAccounts.Size = New System.Drawing.Size(132, 47)
        Me.gbEveAccounts.TabIndex = 16
        Me.gbEveAccounts.TabStop = False
        Me.gbEveAccounts.Text = "API Account Management"
        Me.gbEveAccounts.Visible = False
        '
        'btnGetData
        '
        Me.btnGetData.Location = New System.Drawing.Point(403, 127)
        Me.btnGetData.Name = "btnGetData"
        Me.btnGetData.Size = New System.Drawing.Size(90, 35)
        Me.btnGetData.TabIndex = 22
        Me.btnGetData.Text = "Retrieve Account Data"
        Me.ToolTip1.SetToolTip(Me.btnGetData, "Retrieves API data for all listed accounts")
        '
        'btnDeleteAccount
        '
        Me.btnDeleteAccount.Location = New System.Drawing.Point(403, 81)
        Me.btnDeleteAccount.Name = "btnDeleteAccount"
        Me.btnDeleteAccount.Size = New System.Drawing.Size(90, 25)
        Me.btnDeleteAccount.TabIndex = 21
        Me.btnDeleteAccount.Text = "Delete Account"
        Me.ToolTip1.SetToolTip(Me.btnDeleteAccount, "Removes an API account from EveHQ")
        '
        'btnEditAccount
        '
        Me.btnEditAccount.Location = New System.Drawing.Point(403, 50)
        Me.btnEditAccount.Name = "btnEditAccount"
        Me.btnEditAccount.Size = New System.Drawing.Size(90, 25)
        Me.btnEditAccount.TabIndex = 20
        Me.btnEditAccount.Text = "Edit Account"
        Me.ToolTip1.SetToolTip(Me.btnEditAccount, "Allows the API Key or friendly name of an account to be modified")
        '
        'btnAddAccount
        '
        Me.btnAddAccount.Location = New System.Drawing.Point(403, 19)
        Me.btnAddAccount.Name = "btnAddAccount"
        Me.btnAddAccount.Size = New System.Drawing.Size(90, 25)
        Me.btnAddAccount.TabIndex = 19
        Me.btnAddAccount.Text = "Add Account"
        Me.ToolTip1.SetToolTip(Me.btnAddAccount, "Adds a new API account to EveHQ")
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
        Me.lvAccounts.Size = New System.Drawing.Size(383, 18)
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
        Me.gbPilots.Location = New System.Drawing.Point(247, 372)
        Me.gbPilots.Name = "gbPilots"
        Me.gbPilots.Size = New System.Drawing.Size(115, 48)
        Me.gbPilots.TabIndex = 17
        Me.gbPilots.TabStop = False
        Me.gbPilots.Text = "Pilot Management"
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
        Me.lvwPilots.Size = New System.Drawing.Size(398, 23)
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
        Me.chkStartIGBonLoad.Size = New System.Drawing.Size(155, 17)
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
        Me.nudIGBPort.Size = New System.Drawing.Size(120, 21)
        Me.nudIGBPort.TabIndex = 1
        Me.nudIGBPort.Value = New Decimal(New Integer() {26001, 0, 0, 0})
        '
        'lblIGBPort
        '
        Me.lblIGBPort.AutoSize = True
        Me.lblIGBPort.Location = New System.Drawing.Point(26, 44)
        Me.lblIGBPort.Name = "lblIGBPort"
        Me.lblIGBPort.Size = New System.Drawing.Size(51, 13)
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
        Me.lblFriendlyName4.Size = New System.Drawing.Size(79, 13)
        Me.lblFriendlyName4.TabIndex = 15
        Me.lblFriendlyName4.Text = "Friendly Name:"
        '
        'txtFriendlyName4
        '
        Me.txtFriendlyName4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFriendlyName4.Location = New System.Drawing.Point(-125, 69)
        Me.txtFriendlyName4.Name = "txtFriendlyName4"
        Me.txtFriendlyName4.Size = New System.Drawing.Size(150, 21)
        Me.txtFriendlyName4.TabIndex = 14
        '
        'lblCacheSize4
        '
        Me.lblCacheSize4.AutoSize = True
        Me.lblCacheSize4.Location = New System.Drawing.Point(87, 70)
        Me.lblCacheSize4.Name = "lblCacheSize4"
        Me.lblCacheSize4.Size = New System.Drawing.Size(63, 13)
        Me.lblCacheSize4.TabIndex = 13
        Me.lblCacheSize4.Text = "Cache Size:"
        Me.lblCacheSize4.Visible = False
        '
        'chkLUA4
        '
        Me.chkLUA4.AutoSize = True
        Me.chkLUA4.Location = New System.Drawing.Point(6, 69)
        Me.chkLUA4.Name = "chkLUA4"
        Me.chkLUA4.Size = New System.Drawing.Size(73, 17)
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
        Me.lblFriendlyName3.Size = New System.Drawing.Size(79, 13)
        Me.lblFriendlyName3.TabIndex = 13
        Me.lblFriendlyName3.Text = "Friendly Name:"
        '
        'txtFriendlyName3
        '
        Me.txtFriendlyName3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFriendlyName3.Location = New System.Drawing.Point(-125, 69)
        Me.txtFriendlyName3.Name = "txtFriendlyName3"
        Me.txtFriendlyName3.Size = New System.Drawing.Size(150, 21)
        Me.txtFriendlyName3.TabIndex = 12
        '
        'lblCacheSize3
        '
        Me.lblCacheSize3.AutoSize = True
        Me.lblCacheSize3.Location = New System.Drawing.Point(87, 70)
        Me.lblCacheSize3.Name = "lblCacheSize3"
        Me.lblCacheSize3.Size = New System.Drawing.Size(63, 13)
        Me.lblCacheSize3.TabIndex = 11
        Me.lblCacheSize3.Text = "Cache Size:"
        Me.lblCacheSize3.Visible = False
        '
        'chkLUA3
        '
        Me.chkLUA3.AutoSize = True
        Me.chkLUA3.Location = New System.Drawing.Point(6, 69)
        Me.chkLUA3.Name = "chkLUA3"
        Me.chkLUA3.Size = New System.Drawing.Size(73, 17)
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
        Me.lblFriendlyName2.Size = New System.Drawing.Size(79, 13)
        Me.lblFriendlyName2.TabIndex = 11
        Me.lblFriendlyName2.Text = "Friendly Name:"
        '
        'txtFriendlyName2
        '
        Me.txtFriendlyName2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFriendlyName2.Location = New System.Drawing.Point(-125, 69)
        Me.txtFriendlyName2.Name = "txtFriendlyName2"
        Me.txtFriendlyName2.Size = New System.Drawing.Size(150, 21)
        Me.txtFriendlyName2.TabIndex = 10
        '
        'lblCacheSize2
        '
        Me.lblCacheSize2.AutoSize = True
        Me.lblCacheSize2.Location = New System.Drawing.Point(87, 70)
        Me.lblCacheSize2.Name = "lblCacheSize2"
        Me.lblCacheSize2.Size = New System.Drawing.Size(63, 13)
        Me.lblCacheSize2.TabIndex = 9
        Me.lblCacheSize2.Text = "Cache Size:"
        Me.lblCacheSize2.Visible = False
        '
        'chkLUA2
        '
        Me.chkLUA2.AutoSize = True
        Me.chkLUA2.Location = New System.Drawing.Point(6, 69)
        Me.chkLUA2.Name = "chkLUA2"
        Me.chkLUA2.Size = New System.Drawing.Size(73, 17)
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
        Me.lblFriendlyName1.Size = New System.Drawing.Size(79, 13)
        Me.lblFriendlyName1.TabIndex = 9
        Me.lblFriendlyName1.Text = "Friendly Name:"
        '
        'txtFriendlyName1
        '
        Me.txtFriendlyName1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFriendlyName1.Location = New System.Drawing.Point(-125, 69)
        Me.txtFriendlyName1.Name = "txtFriendlyName1"
        Me.txtFriendlyName1.Size = New System.Drawing.Size(150, 21)
        Me.txtFriendlyName1.TabIndex = 8
        '
        'lblCacheSize1
        '
        Me.lblCacheSize1.AutoSize = True
        Me.lblCacheSize1.Location = New System.Drawing.Point(87, 70)
        Me.lblCacheSize1.Name = "lblCacheSize1"
        Me.lblCacheSize1.Size = New System.Drawing.Size(63, 13)
        Me.lblCacheSize1.TabIndex = 7
        Me.lblCacheSize1.Text = "Cache Size:"
        Me.lblCacheSize1.Visible = False
        '
        'chkLUA1
        '
        Me.chkLUA1.AutoSize = True
        Me.chkLUA1.Location = New System.Drawing.Point(6, 69)
        Me.chkLUA1.Name = "chkLUA1"
        Me.chkLUA1.Size = New System.Drawing.Size(73, 17)
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
        Me.gbTrainingQueue.Controls.Add(Me.btnMoveDown)
        Me.gbTrainingQueue.Controls.Add(Me.btnMoveUp)
        Me.gbTrainingQueue.Controls.Add(Me.lvwColumns)
        Me.gbTrainingQueue.Controls.Add(Me.chkShowCompletedSkills)
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
        Me.gbTrainingQueue.Controls.Add(Me.lblQueueColumns)
        Me.gbTrainingQueue.Location = New System.Drawing.Point(695, 208)
        Me.gbTrainingQueue.Name = "gbTrainingQueue"
        Me.gbTrainingQueue.Size = New System.Drawing.Size(113, 57)
        Me.gbTrainingQueue.TabIndex = 3
        Me.gbTrainingQueue.TabStop = False
        Me.gbTrainingQueue.Text = "Training Queue"
        Me.gbTrainingQueue.Visible = False
        '
        'btnMoveDown
        '
        Me.btnMoveDown.Location = New System.Drawing.Point(92, 429)
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(80, 23)
        Me.btnMoveDown.TabIndex = 36
        Me.btnMoveDown.Text = "Move Down"
        Me.btnMoveDown.UseVisualStyleBackColor = True
        '
        'btnMoveUp
        '
        Me.btnMoveUp.Location = New System.Drawing.Point(6, 429)
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.Size = New System.Drawing.Size(80, 23)
        Me.btnMoveUp.TabIndex = 35
        Me.btnMoveUp.Text = "Move Up"
        Me.btnMoveUp.UseVisualStyleBackColor = True
        '
        'lvwColumns
        '
        Me.lvwColumns.CheckBoxes = True
        Me.lvwColumns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colQueueColumns})
        Me.lvwColumns.FullRowSelect = True
        Me.lvwColumns.HideSelection = False
        Me.lvwColumns.Location = New System.Drawing.Point(6, 41)
        Me.lvwColumns.Name = "lvwColumns"
        Me.lvwColumns.Size = New System.Drawing.Size(222, 382)
        Me.lvwColumns.TabIndex = 34
        Me.lvwColumns.UseCompatibleStateImageBehavior = False
        Me.lvwColumns.View = System.Windows.Forms.View.Details
        '
        'colQueueColumns
        '
        Me.colQueueColumns.Text = "Queue Columns"
        Me.colQueueColumns.Width = 200
        '
        'chkShowCompletedSkills
        '
        Me.chkShowCompletedSkills.AutoSize = True
        Me.chkShowCompletedSkills.Location = New System.Drawing.Point(287, 312)
        Me.chkShowCompletedSkills.Name = "chkShowCompletedSkills"
        Me.chkShowCompletedSkills.Size = New System.Drawing.Size(191, 17)
        Me.chkShowCompletedSkills.TabIndex = 33
        Me.chkShowCompletedSkills.Text = "Show completed skills in skill queue"
        Me.chkShowCompletedSkills.UseVisualStyleBackColor = True
        '
        'pbPartiallyTrainedColour
        '
        Me.pbPartiallyTrainedColour.BackColor = System.Drawing.Color.White
        Me.pbPartiallyTrainedColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbPartiallyTrainedColour.Location = New System.Drawing.Point(426, 205)
        Me.pbPartiallyTrainedColour.Name = "pbPartiallyTrainedColour"
        Me.pbPartiallyTrainedColour.Size = New System.Drawing.Size(24, 24)
        Me.pbPartiallyTrainedColour.TabIndex = 32
        Me.pbPartiallyTrainedColour.TabStop = False
        '
        'lblPartiallyTrainedColour
        '
        Me.lblPartiallyTrainedColour.AutoSize = True
        Me.lblPartiallyTrainedColour.Location = New System.Drawing.Point(299, 212)
        Me.lblPartiallyTrainedColour.Name = "lblPartiallyTrainedColour"
        Me.lblPartiallyTrainedColour.Size = New System.Drawing.Size(84, 13)
        Me.lblPartiallyTrainedColour.TabIndex = 31
        Me.lblPartiallyTrainedColour.Text = "Partially Trained"
        '
        'chkDeleteCompletedSkills
        '
        Me.chkDeleteCompletedSkills.AutoSize = True
        Me.chkDeleteCompletedSkills.Location = New System.Drawing.Point(287, 289)
        Me.chkDeleteCompletedSkills.Name = "chkDeleteCompletedSkills"
        Me.chkDeleteCompletedSkills.Size = New System.Drawing.Size(262, 17)
        Me.chkDeleteCompletedSkills.TabIndex = 30
        Me.chkDeleteCompletedSkills.Text = "Automatically delete completed skills from queues"
        Me.chkDeleteCompletedSkills.UseVisualStyleBackColor = True
        '
        'pbReadySkillColour
        '
        Me.pbReadySkillColour.BackColor = System.Drawing.Color.White
        Me.pbReadySkillColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbReadySkillColour.Location = New System.Drawing.Point(426, 55)
        Me.pbReadySkillColour.Name = "pbReadySkillColour"
        Me.pbReadySkillColour.Size = New System.Drawing.Size(24, 24)
        Me.pbReadySkillColour.TabIndex = 29
        Me.pbReadySkillColour.TabStop = False
        '
        'lblReadySkillColour
        '
        Me.lblReadySkillColour.AutoSize = True
        Me.lblReadySkillColour.Location = New System.Drawing.Point(299, 62)
        Me.lblReadySkillColour.Name = "lblReadySkillColour"
        Me.lblReadySkillColour.Size = New System.Drawing.Size(89, 13)
        Me.lblReadySkillColour.TabIndex = 28
        Me.lblReadySkillColour.Text = "Independent Skill"
        '
        'pbDowntimeClashColour
        '
        Me.pbDowntimeClashColour.BackColor = System.Drawing.Color.Red
        Me.pbDowntimeClashColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbDowntimeClashColour.Location = New System.Drawing.Point(426, 175)
        Me.pbDowntimeClashColour.Name = "pbDowntimeClashColour"
        Me.pbDowntimeClashColour.Size = New System.Drawing.Size(24, 24)
        Me.pbDowntimeClashColour.TabIndex = 27
        Me.pbDowntimeClashColour.TabStop = False
        '
        'lblDowntimeClashColour
        '
        Me.lblDowntimeClashColour.AutoSize = True
        Me.lblDowntimeClashColour.Location = New System.Drawing.Point(299, 182)
        Me.lblDowntimeClashColour.Name = "lblDowntimeClashColour"
        Me.lblDowntimeClashColour.Size = New System.Drawing.Size(108, 13)
        Me.lblDowntimeClashColour.TabIndex = 26
        Me.lblDowntimeClashColour.Text = "Downtime Clash Text"
        '
        'pbBothPreReqColour
        '
        Me.pbBothPreReqColour.BackColor = System.Drawing.Color.Gold
        Me.pbBothPreReqColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbBothPreReqColour.Location = New System.Drawing.Point(426, 145)
        Me.pbBothPreReqColour.Name = "pbBothPreReqColour"
        Me.pbBothPreReqColour.Size = New System.Drawing.Size(24, 24)
        Me.pbBothPreReqColour.TabIndex = 25
        Me.pbBothPreReqColour.TabStop = False
        '
        'lblBothPreReqColour
        '
        Me.lblBothPreReqColour.AutoSize = True
        Me.lblBothPreReqColour.Location = New System.Drawing.Point(299, 152)
        Me.lblBothPreReqColour.Name = "lblBothPreReqColour"
        Me.lblBothPreReqColour.Size = New System.Drawing.Size(61, 13)
        Me.lblBothPreReqColour.TabIndex = 24
        Me.lblBothPreReqColour.Text = "Skill Is Both"
        '
        'pbHasPreReqColour
        '
        Me.pbHasPreReqColour.BackColor = System.Drawing.Color.Plum
        Me.pbHasPreReqColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbHasPreReqColour.Location = New System.Drawing.Point(426, 115)
        Me.pbHasPreReqColour.Name = "pbHasPreReqColour"
        Me.pbHasPreReqColour.Size = New System.Drawing.Size(24, 24)
        Me.pbHasPreReqColour.TabIndex = 23
        Me.pbHasPreReqColour.TabStop = False
        '
        'pbIsPreReqColour
        '
        Me.pbIsPreReqColour.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbIsPreReqColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbIsPreReqColour.Location = New System.Drawing.Point(426, 85)
        Me.pbIsPreReqColour.Name = "pbIsPreReqColour"
        Me.pbIsPreReqColour.Size = New System.Drawing.Size(24, 24)
        Me.pbIsPreReqColour.TabIndex = 22
        Me.pbIsPreReqColour.TabStop = False
        '
        'lblHasPreReqColour
        '
        Me.lblHasPreReqColour.AutoSize = True
        Me.lblHasPreReqColour.Location = New System.Drawing.Point(299, 122)
        Me.lblHasPreReqColour.Name = "lblHasPreReqColour"
        Me.lblHasPreReqColour.Size = New System.Drawing.Size(85, 13)
        Me.lblHasPreReqColour.TabIndex = 21
        Me.lblHasPreReqColour.Text = "Skill HAS PreReq"
        '
        'lblIsPreReqColour
        '
        Me.lblIsPreReqColour.AutoSize = True
        Me.lblIsPreReqColour.Location = New System.Drawing.Point(299, 92)
        Me.lblIsPreReqColour.Name = "lblIsPreReqColour"
        Me.lblIsPreReqColour.Size = New System.Drawing.Size(75, 13)
        Me.lblIsPreReqColour.TabIndex = 20
        Me.lblIsPreReqColour.Text = "Skill IS PreReq"
        '
        'lblSkillQueueColours
        '
        Me.lblSkillQueueColours.AutoSize = True
        Me.lblSkillQueueColours.Location = New System.Drawing.Point(284, 25)
        Me.lblSkillQueueColours.Name = "lblSkillQueueColours"
        Me.lblSkillQueueColours.Size = New System.Drawing.Size(102, 13)
        Me.lblSkillQueueColours.TabIndex = 4
        Me.lblSkillQueueColours.Text = "Skill Queue Colours:"
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
        'gbDatabaseFormat
        '
        Me.gbDatabaseFormat.Controls.Add(Me.nudDBTimeout)
        Me.gbDatabaseFormat.Controls.Add(Me.lblDatabaseTimeout)
        Me.gbDatabaseFormat.Controls.Add(Me.btnTestDB)
        Me.gbDatabaseFormat.Controls.Add(Me.cboFormat)
        Me.gbDatabaseFormat.Controls.Add(Me.lblFormat)
        Me.gbDatabaseFormat.Controls.Add(Me.gbAccess)
        Me.gbDatabaseFormat.Controls.Add(Me.gbMSSQL)
        Me.gbDatabaseFormat.Location = New System.Drawing.Point(449, 77)
        Me.gbDatabaseFormat.Name = "gbDatabaseFormat"
        Me.gbDatabaseFormat.Size = New System.Drawing.Size(113, 47)
        Me.gbDatabaseFormat.TabIndex = 18
        Me.gbDatabaseFormat.TabStop = False
        Me.gbDatabaseFormat.Text = "Database Format"
        Me.gbDatabaseFormat.Visible = False
        '
        'nudDBTimeout
        '
        Me.nudDBTimeout.Location = New System.Drawing.Point(135, 360)
        Me.nudDBTimeout.Name = "nudDBTimeout"
        Me.nudDBTimeout.Size = New System.Drawing.Size(50, 21)
        Me.nudDBTimeout.TabIndex = 40
        '
        'lblDatabaseTimeout
        '
        Me.lblDatabaseTimeout.AutoSize = True
        Me.lblDatabaseTimeout.Location = New System.Drawing.Point(6, 363)
        Me.lblDatabaseTimeout.Name = "lblDatabaseTimeout"
        Me.lblDatabaseTimeout.Size = New System.Drawing.Size(125, 13)
        Me.lblDatabaseTimeout.TabIndex = 39
        Me.lblDatabaseTimeout.Text = "Database Timeout (sec):"
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
        'cboFormat
        '
        Me.cboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFormat.FormattingEnabled = True
        Me.cboFormat.Items.AddRange(New Object() {"Access Database (.MDB)", "MS SQL Server", "MS SQL 2005 Express"})
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
        Me.lblFormat.Size = New System.Drawing.Size(94, 13)
        Me.lblFormat.TabIndex = 33
        Me.lblFormat.Text = "Database Format:"
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
        Me.gbAccess.Location = New System.Drawing.Point(9, 91)
        Me.gbAccess.Name = "gbAccess"
        Me.gbAccess.Size = New System.Drawing.Size(500, 250)
        Me.gbAccess.TabIndex = 37
        Me.gbAccess.TabStop = False
        Me.gbAccess.Text = "Access (MDB) Options"
        '
        'chkUseAppDirForDB
        '
        Me.chkUseAppDirForDB.AutoSize = True
        Me.chkUseAppDirForDB.Location = New System.Drawing.Point(99, 128)
        Me.chkUseAppDirForDB.Name = "chkUseAppDirForDB"
        Me.chkUseAppDirForDB.Size = New System.Drawing.Size(248, 17)
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
        Me.txtMDBPassword.Location = New System.Drawing.Point(99, 101)
        Me.txtMDBPassword.Name = "txtMDBPassword"
        Me.txtMDBPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMDBPassword.Size = New System.Drawing.Size(230, 21)
        Me.txtMDBPassword.TabIndex = 5
        '
        'txtMDBUsername
        '
        Me.txtMDBUsername.Location = New System.Drawing.Point(99, 75)
        Me.txtMDBUsername.Name = "txtMDBUsername"
        Me.txtMDBUsername.Size = New System.Drawing.Size(230, 21)
        Me.txtMDBUsername.TabIndex = 4
        '
        'txtMDBServer
        '
        Me.txtMDBServer.Location = New System.Drawing.Point(99, 27)
        Me.txtMDBServer.Multiline = True
        Me.txtMDBServer.Name = "txtMDBServer"
        Me.txtMDBServer.Size = New System.Drawing.Size(324, 42)
        Me.txtMDBServer.TabIndex = 3
        '
        'lblMDBPassword
        '
        Me.lblMDBPassword.AutoSize = True
        Me.lblMDBPassword.Location = New System.Drawing.Point(7, 104)
        Me.lblMDBPassword.Name = "lblMDBPassword"
        Me.lblMDBPassword.Size = New System.Drawing.Size(57, 13)
        Me.lblMDBPassword.TabIndex = 2
        Me.lblMDBPassword.Text = "Password:"
        '
        'lblMDBUser
        '
        Me.lblMDBUser.AutoSize = True
        Me.lblMDBUser.Location = New System.Drawing.Point(7, 78)
        Me.lblMDBUser.Name = "lblMDBUser"
        Me.lblMDBUser.Size = New System.Drawing.Size(33, 13)
        Me.lblMDBUser.TabIndex = 1
        Me.lblMDBUser.Text = "User:"
        '
        'lblMDBFilename
        '
        Me.lblMDBFilename.AutoSize = True
        Me.lblMDBFilename.Location = New System.Drawing.Point(6, 30)
        Me.lblMDBFilename.Name = "lblMDBFilename"
        Me.lblMDBFilename.Size = New System.Drawing.Size(82, 13)
        Me.lblMDBFilename.TabIndex = 0
        Me.lblMDBFilename.Text = "Item Database:"
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
        Me.gbMSSQL.Location = New System.Drawing.Point(9, 155)
        Me.gbMSSQL.Name = "gbMSSQL"
        Me.gbMSSQL.Size = New System.Drawing.Size(513, 160)
        Me.gbMSSQL.TabIndex = 35
        Me.gbMSSQL.TabStop = False
        Me.gbMSSQL.Text = "MS SQL Options"
        Me.gbMSSQL.Visible = False
        '
        'txtMSSQLDatabase
        '
        Me.txtMSSQLDatabase.Location = New System.Drawing.Point(105, 77)
        Me.txtMSSQLDatabase.Name = "txtMSSQLDatabase"
        Me.txtMSSQLDatabase.Size = New System.Drawing.Size(230, 21)
        Me.txtMSSQLDatabase.TabIndex = 10
        '
        'lblMSSQLDatabase
        '
        Me.lblMSSQLDatabase.AutoSize = True
        Me.lblMSSQLDatabase.Location = New System.Drawing.Point(6, 82)
        Me.lblMSSQLDatabase.Name = "lblMSSQLDatabase"
        Me.lblMSSQLDatabase.Size = New System.Drawing.Size(82, 13)
        Me.lblMSSQLDatabase.TabIndex = 9
        Me.lblMSSQLDatabase.Text = "Item Database:"
        '
        'lblMSSQLSecurity
        '
        Me.lblMSSQLSecurity.AutoSize = True
        Me.lblMSSQLSecurity.Location = New System.Drawing.Point(6, 32)
        Me.lblMSSQLSecurity.Name = "lblMSSQLSecurity"
        Me.lblMSSQLSecurity.Size = New System.Drawing.Size(46, 13)
        Me.lblMSSQLSecurity.TabIndex = 8
        Me.lblMSSQLSecurity.Text = "Security"
        '
        'radMSSQLDatabase
        '
        Me.radMSSQLDatabase.AutoSize = True
        Me.radMSSQLDatabase.Location = New System.Drawing.Point(105, 29)
        Me.radMSSQLDatabase.Name = "radMSSQLDatabase"
        Me.radMSSQLDatabase.Size = New System.Drawing.Size(44, 17)
        Me.radMSSQLDatabase.TabIndex = 7
        Me.radMSSQLDatabase.Text = "SQL"
        Me.radMSSQLDatabase.UseVisualStyleBackColor = True
        '
        'radMSSQLWindows
        '
        Me.radMSSQLWindows.AutoSize = True
        Me.radMSSQLWindows.Location = New System.Drawing.Point(173, 29)
        Me.radMSSQLWindows.Name = "radMSSQLWindows"
        Me.radMSSQLWindows.Size = New System.Drawing.Size(68, 17)
        Me.radMSSQLWindows.TabIndex = 6
        Me.radMSSQLWindows.Text = "Windows"
        Me.radMSSQLWindows.UseVisualStyleBackColor = True
        '
        'txtMSSQLPassword
        '
        Me.txtMSSQLPassword.Location = New System.Drawing.Point(105, 130)
        Me.txtMSSQLPassword.Name = "txtMSSQLPassword"
        Me.txtMSSQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMSSQLPassword.Size = New System.Drawing.Size(230, 21)
        Me.txtMSSQLPassword.TabIndex = 5
        '
        'txtMSSQLUsername
        '
        Me.txtMSSQLUsername.Location = New System.Drawing.Point(105, 104)
        Me.txtMSSQLUsername.Name = "txtMSSQLUsername"
        Me.txtMSSQLUsername.Size = New System.Drawing.Size(230, 21)
        Me.txtMSSQLUsername.TabIndex = 4
        '
        'txtMSSQLServer
        '
        Me.txtMSSQLServer.Location = New System.Drawing.Point(105, 51)
        Me.txtMSSQLServer.Name = "txtMSSQLServer"
        Me.txtMSSQLServer.Size = New System.Drawing.Size(230, 21)
        Me.txtMSSQLServer.TabIndex = 3
        '
        'lblMSSQLPassword
        '
        Me.lblMSSQLPassword.AutoSize = True
        Me.lblMSSQLPassword.Location = New System.Drawing.Point(6, 133)
        Me.lblMSSQLPassword.Name = "lblMSSQLPassword"
        Me.lblMSSQLPassword.Size = New System.Drawing.Size(57, 13)
        Me.lblMSSQLPassword.TabIndex = 2
        Me.lblMSSQLPassword.Text = "Password:"
        '
        'lblMSSQLUser
        '
        Me.lblMSSQLUser.AutoSize = True
        Me.lblMSSQLUser.Location = New System.Drawing.Point(6, 107)
        Me.lblMSSQLUser.Name = "lblMSSQLUser"
        Me.lblMSSQLUser.Size = New System.Drawing.Size(33, 13)
        Me.lblMSSQLUser.TabIndex = 1
        Me.lblMSSQLUser.Text = "User:"
        '
        'lblMSSQLServer
        '
        Me.lblMSSQLServer.AutoSize = True
        Me.lblMSSQLServer.Location = New System.Drawing.Point(6, 56)
        Me.lblMSSQLServer.Name = "lblMSSQLServer"
        Me.lblMSSQLServer.Size = New System.Drawing.Size(43, 13)
        Me.lblMSSQLServer.TabIndex = 0
        Me.lblMSSQLServer.Text = "Server:"
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
        Me.lblProxyPassword.Size = New System.Drawing.Size(57, 13)
        Me.lblProxyPassword.TabIndex = 9
        Me.lblProxyPassword.Text = "Password:"
        '
        'lblProxyUsername
        '
        Me.lblProxyUsername.AutoSize = True
        Me.lblProxyUsername.Enabled = False
        Me.lblProxyUsername.Location = New System.Drawing.Point(36, 119)
        Me.lblProxyUsername.Name = "lblProxyUsername"
        Me.lblProxyUsername.Size = New System.Drawing.Size(59, 13)
        Me.lblProxyUsername.TabIndex = 8
        Me.lblProxyUsername.Text = "Username:"
        '
        'txtProxyPassword
        '
        Me.txtProxyPassword.Enabled = False
        Me.txtProxyPassword.Location = New System.Drawing.Point(100, 142)
        Me.txtProxyPassword.Name = "txtProxyPassword"
        Me.txtProxyPassword.Size = New System.Drawing.Size(234, 21)
        Me.txtProxyPassword.TabIndex = 7
        '
        'txtProxyUsername
        '
        Me.txtProxyUsername.Enabled = False
        Me.txtProxyUsername.Location = New System.Drawing.Point(100, 116)
        Me.txtProxyUsername.Name = "txtProxyUsername"
        Me.txtProxyUsername.Size = New System.Drawing.Size(234, 21)
        Me.txtProxyUsername.TabIndex = 6
        '
        'radUseSpecifiedCreds
        '
        Me.radUseSpecifiedCreds.AutoSize = True
        Me.radUseSpecifiedCreds.Location = New System.Drawing.Point(27, 93)
        Me.radUseSpecifiedCreds.Name = "radUseSpecifiedCreds"
        Me.radUseSpecifiedCreds.Size = New System.Drawing.Size(170, 17)
        Me.radUseSpecifiedCreds.TabIndex = 5
        Me.radUseSpecifiedCreds.Text = "Use the Following Credentials:"
        Me.radUseSpecifiedCreds.UseVisualStyleBackColor = True
        '
        'lblProxyServer
        '
        Me.lblProxyServer.AutoSize = True
        Me.lblProxyServer.Location = New System.Drawing.Point(24, 33)
        Me.lblProxyServer.Name = "lblProxyServer"
        Me.lblProxyServer.Size = New System.Drawing.Size(74, 13)
        Me.lblProxyServer.TabIndex = 3
        Me.lblProxyServer.Text = "Proxy Server:"
        '
        'txtProxyServer
        '
        Me.txtProxyServer.Location = New System.Drawing.Point(100, 30)
        Me.txtProxyServer.Name = "txtProxyServer"
        Me.txtProxyServer.Size = New System.Drawing.Size(234, 21)
        Me.txtProxyServer.TabIndex = 1
        '
        'radUseDefaultCreds
        '
        Me.radUseDefaultCreds.AutoSize = True
        Me.radUseDefaultCreds.Checked = True
        Me.radUseDefaultCreds.Location = New System.Drawing.Point(27, 70)
        Me.radUseDefaultCreds.Name = "radUseDefaultCreds"
        Me.radUseDefaultCreds.Size = New System.Drawing.Size(183, 17)
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
        Me.chkUseProxy.Size = New System.Drawing.Size(110, 17)
        Me.chkUseProxy.TabIndex = 0
        Me.chkUseProxy.Text = "Use Proxy Server"
        Me.chkUseProxy.UseVisualStyleBackColor = True
        '
        'gbEveServer
        '
        Me.gbEveServer.Controls.Add(Me.chkAutoMailAPI)
        Me.gbEveServer.Controls.Add(Me.gbAPIServer)
        Me.gbEveServer.Controls.Add(Me.gbAPIRelayServer)
        Me.gbEveServer.Controls.Add(Me.chkEnableEveStatus)
        Me.gbEveServer.Controls.Add(Me.lblCurrentOffset)
        Me.gbEveServer.Controls.Add(Me.lblServerOffset)
        Me.gbEveServer.Controls.Add(Me.trackServerOffset)
        Me.gbEveServer.Controls.Add(Me.chkAutoAPI)
        Me.gbEveServer.Location = New System.Drawing.Point(213, 298)
        Me.gbEveServer.Name = "gbEveServer"
        Me.gbEveServer.Size = New System.Drawing.Size(184, 47)
        Me.gbEveServer.TabIndex = 2
        Me.gbEveServer.TabStop = False
        Me.gbEveServer.Text = "Eve API && Server Options"
        Me.gbEveServer.Visible = False
        '
        'chkAutoMailAPI
        '
        Me.chkAutoMailAPI.AutoSize = True
        Me.chkAutoMailAPI.Location = New System.Drawing.Point(18, 420)
        Me.chkAutoMailAPI.Name = "chkAutoMailAPI"
        Me.chkAutoMailAPI.Size = New System.Drawing.Size(303, 17)
        Me.chkAutoMailAPI.TabIndex = 21
        Me.chkAutoMailAPI.Text = "Automatically Check for Mail and Notification XML Updates"
        Me.chkAutoMailAPI.UseVisualStyleBackColor = True
        '
        'gbAPIServer
        '
        Me.gbAPIServer.Controls.Add(Me.txtAPIFileExtension)
        Me.gbAPIServer.Controls.Add(Me.lblAPIFileExtension)
        Me.gbAPIServer.Controls.Add(Me.chkUseCCPBackup)
        Me.gbAPIServer.Controls.Add(Me.chkUseAPIRSServer)
        Me.gbAPIServer.Controls.Add(Me.txtAPIRSServer)
        Me.gbAPIServer.Controls.Add(Me.lblAPIRSServer)
        Me.gbAPIServer.Controls.Add(Me.txtCCPAPIServer)
        Me.gbAPIServer.Controls.Add(Me.lblCCPAPIServer)
        Me.gbAPIServer.Location = New System.Drawing.Point(6, 152)
        Me.gbAPIServer.Name = "gbAPIServer"
        Me.gbAPIServer.Size = New System.Drawing.Size(668, 133)
        Me.gbAPIServer.TabIndex = 20
        Me.gbAPIServer.TabStop = False
        Me.gbAPIServer.Text = "API Server"
        '
        'txtAPIFileExtension
        '
        Me.txtAPIFileExtension.Location = New System.Drawing.Point(152, 101)
        Me.txtAPIFileExtension.Name = "txtAPIFileExtension"
        Me.txtAPIFileExtension.Size = New System.Drawing.Size(98, 21)
        Me.txtAPIFileExtension.TabIndex = 28
        '
        'lblAPIFileExtension
        '
        Me.lblAPIFileExtension.AutoSize = True
        Me.lblAPIFileExtension.Location = New System.Drawing.Point(9, 104)
        Me.lblAPIFileExtension.Name = "lblAPIFileExtension"
        Me.lblAPIFileExtension.Size = New System.Drawing.Size(96, 13)
        Me.lblAPIFileExtension.TabIndex = 27
        Me.lblAPIFileExtension.Text = "API File Extention:"
        '
        'chkUseCCPBackup
        '
        Me.chkUseCCPBackup.AutoSize = True
        Me.chkUseCCPBackup.Location = New System.Drawing.Point(152, 52)
        Me.chkUseCCPBackup.Name = "chkUseCCPBackup"
        Me.chkUseCCPBackup.Size = New System.Drawing.Size(173, 17)
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
        Me.txtAPIRSServer.Size = New System.Drawing.Size(332, 21)
        Me.txtAPIRSServer.TabIndex = 24
        '
        'lblAPIRSServer
        '
        Me.lblAPIRSServer.AutoSize = True
        Me.lblAPIRSServer.Location = New System.Drawing.Point(9, 78)
        Me.lblAPIRSServer.Name = "lblAPIRSServer"
        Me.lblAPIRSServer.Size = New System.Drawing.Size(138, 13)
        Me.lblAPIRSServer.TabIndex = 23
        Me.lblAPIRSServer.Text = "APIRS API Server Address:"
        '
        'txtCCPAPIServer
        '
        Me.txtCCPAPIServer.Location = New System.Drawing.Point(152, 26)
        Me.txtCCPAPIServer.Name = "txtCCPAPIServer"
        Me.txtCCPAPIServer.Size = New System.Drawing.Size(332, 21)
        Me.txtCCPAPIServer.TabIndex = 22
        '
        'lblCCPAPIServer
        '
        Me.lblCCPAPIServer.AutoSize = True
        Me.lblCCPAPIServer.Location = New System.Drawing.Point(9, 29)
        Me.lblCCPAPIServer.Name = "lblCCPAPIServer"
        Me.lblCCPAPIServer.Size = New System.Drawing.Size(128, 13)
        Me.lblCCPAPIServer.TabIndex = 21
        Me.lblCCPAPIServer.Text = "CCP API Server Address:"
        '
        'gbAPIRelayServer
        '
        Me.gbAPIRelayServer.Controls.Add(Me.chkAPIRSAutoStart)
        Me.gbAPIRelayServer.Controls.Add(Me.nudAPIRSPort)
        Me.gbAPIRelayServer.Controls.Add(Me.lblAPIRSPort)
        Me.gbAPIRelayServer.Controls.Add(Me.chkActivateAPIRS)
        Me.gbAPIRelayServer.Location = New System.Drawing.Point(6, 291)
        Me.gbAPIRelayServer.Name = "gbAPIRelayServer"
        Me.gbAPIRelayServer.Size = New System.Drawing.Size(668, 83)
        Me.gbAPIRelayServer.TabIndex = 15
        Me.gbAPIRelayServer.TabStop = False
        Me.gbAPIRelayServer.Text = "API Relay Server"
        '
        'chkAPIRSAutoStart
        '
        Me.chkAPIRSAutoStart.AutoSize = True
        Me.chkAPIRSAutoStart.Location = New System.Drawing.Point(223, 30)
        Me.chkAPIRSAutoStart.Name = "chkAPIRSAutoStart"
        Me.chkAPIRSAutoStart.Size = New System.Drawing.Size(184, 17)
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
        Me.nudAPIRSPort.Size = New System.Drawing.Size(90, 21)
        Me.nudAPIRSPort.TabIndex = 4
        Me.nudAPIRSPort.Value = New Decimal(New Integer() {26002, 0, 0, 0})
        '
        'lblAPIRSPort
        '
        Me.lblAPIRSPort.AutoSize = True
        Me.lblAPIRSPort.Location = New System.Drawing.Point(10, 55)
        Me.lblAPIRSPort.Name = "lblAPIRSPort"
        Me.lblAPIRSPort.Size = New System.Drawing.Size(112, 13)
        Me.lblAPIRSPort.TabIndex = 3
        Me.lblAPIRSPort.Text = "API Relay Server Port"
        '
        'chkActivateAPIRS
        '
        Me.chkActivateAPIRS.AutoSize = True
        Me.chkActivateAPIRS.Location = New System.Drawing.Point(13, 30)
        Me.chkActivateAPIRS.Name = "chkActivateAPIRS"
        Me.chkActivateAPIRS.Size = New System.Drawing.Size(151, 17)
        Me.chkActivateAPIRS.TabIndex = 0
        Me.chkActivateAPIRS.Text = "Activate API Relay Server"
        Me.chkActivateAPIRS.UseVisualStyleBackColor = True
        '
        'chkEnableEveStatus
        '
        Me.chkEnableEveStatus.AutoSize = True
        Me.chkEnableEveStatus.Location = New System.Drawing.Point(19, 31)
        Me.chkEnableEveStatus.Name = "chkEnableEveStatus"
        Me.chkEnableEveStatus.Size = New System.Drawing.Size(127, 17)
        Me.chkEnableEveStatus.TabIndex = 13
        Me.chkEnableEveStatus.Text = "Enable Server Status"
        Me.chkEnableEveStatus.UseVisualStyleBackColor = True
        '
        'lblCurrentOffset
        '
        Me.lblCurrentOffset.AutoSize = True
        Me.lblCurrentOffset.Location = New System.Drawing.Point(16, 118)
        Me.lblCurrentOffset.Name = "lblCurrentOffset"
        Me.lblCurrentOffset.Size = New System.Drawing.Size(82, 13)
        Me.lblCurrentOffset.TabIndex = 12
        Me.lblCurrentOffset.Text = "Current Offset:"
        '
        'lblServerOffset
        '
        Me.lblServerOffset.AutoSize = True
        Me.lblServerOffset.Location = New System.Drawing.Point(16, 54)
        Me.lblServerOffset.Name = "lblServerOffset"
        Me.lblServerOffset.Size = New System.Drawing.Size(153, 13)
        Me.lblServerOffset.TabIndex = 11
        Me.lblServerOffset.Text = "Tranquiity Server Time Offset:"
        '
        'trackServerOffset
        '
        Me.trackServerOffset.BackColor = System.Drawing.SystemColors.Control
        Me.trackServerOffset.Location = New System.Drawing.Point(19, 70)
        Me.trackServerOffset.Maximum = 600
        Me.trackServerOffset.Minimum = -600
        Me.trackServerOffset.Name = "trackServerOffset"
        Me.trackServerOffset.Size = New System.Drawing.Size(390, 45)
        Me.trackServerOffset.TabIndex = 10
        Me.trackServerOffset.TickFrequency = 30
        Me.trackServerOffset.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'chkAutoAPI
        '
        Me.chkAutoAPI.AutoSize = True
        Me.chkAutoAPI.Location = New System.Drawing.Point(18, 397)
        Me.chkAutoAPI.Name = "chkAutoAPI"
        Me.chkAutoAPI.Size = New System.Drawing.Size(255, 17)
        Me.chkAutoAPI.TabIndex = 20
        Me.chkAutoAPI.Text = "Automatically Check for Character XML Updates"
        Me.chkAutoAPI.UseVisualStyleBackColor = True
        '
        'gbPlugIns
        '
        Me.gbPlugIns.Controls.Add(Me.btnTidyPlugins)
        Me.gbPlugIns.Controls.Add(Me.btnRefreshPlugins)
        Me.gbPlugIns.Controls.Add(Me.lblPlugInInfo)
        Me.gbPlugIns.Controls.Add(Me.lblDetectedPlugIns)
        Me.gbPlugIns.Controls.Add(Me.lvwPlugins)
        Me.gbPlugIns.Location = New System.Drawing.Point(443, 235)
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
        Me.lblPlugInInfo.Size = New System.Drawing.Size(332, 13)
        Me.lblPlugInInfo.TabIndex = 2
        Me.lblPlugInInfo.Text = "Changes to the PlugIns will not take effect until EveHQ is restarted."
        '
        'lblDetectedPlugIns
        '
        Me.lblDetectedPlugIns.AutoSize = True
        Me.lblDetectedPlugIns.Location = New System.Drawing.Point(6, 24)
        Me.lblDetectedPlugIns.Name = "lblDetectedPlugIns"
        Me.lblDetectedPlugIns.Size = New System.Drawing.Size(79, 13)
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
        Me.gbNotifications.Controls.Add(Me.chkNotifyNotification)
        Me.gbNotifications.Controls.Add(Me.chkNotifyEveMail)
        Me.gbNotifications.Controls.Add(Me.chkNotifyEarly)
        Me.gbNotifications.Controls.Add(Me.chkNotifyNow)
        Me.gbNotifications.Controls.Add(Me.lblNotifyMe)
        Me.gbNotifications.Controls.Add(Me.btnSoundTest)
        Me.gbNotifications.Controls.Add(Me.btnSelectSoundFile)
        Me.gbNotifications.Controls.Add(Me.lblSoundFile)
        Me.gbNotifications.Controls.Add(Me.chkNotifySound)
        Me.gbNotifications.Controls.Add(Me.lblNotifyOffset)
        Me.gbNotifications.Controls.Add(Me.trackNotifyOffset)
        Me.gbNotifications.Controls.Add(Me.chkNotifyEmail)
        Me.gbNotifications.Controls.Add(Me.chkNotifyDialog)
        Me.gbNotifications.Controls.Add(Me.chkNotifyToolTip)
        Me.gbNotifications.Controls.Add(Me.nudShutdownNotifyPeriod)
        Me.gbNotifications.Controls.Add(Me.lblShutdownNotifyPeriod)
        Me.gbNotifications.Controls.Add(Me.chkShutdownNotify)
        Me.gbNotifications.Location = New System.Drawing.Point(213, 125)
        Me.gbNotifications.Name = "gbNotifications"
        Me.gbNotifications.Size = New System.Drawing.Size(162, 47)
        Me.gbNotifications.TabIndex = 20
        Me.gbNotifications.TabStop = False
        Me.gbNotifications.Text = "Notifications"
        Me.gbNotifications.Visible = False
        '
        'chkNotifyEarly
        '
        Me.chkNotifyEarly.AutoSize = True
        Me.chkNotifyEarly.Location = New System.Drawing.Point(236, 153)
        Me.chkNotifyEarly.Name = "chkNotifyEarly"
        Me.chkNotifyEarly.Size = New System.Drawing.Size(116, 17)
        Me.chkNotifyEarly.TabIndex = 20
        Me.chkNotifyEarly.Text = "Before skill finishes"
        Me.chkNotifyEarly.UseVisualStyleBackColor = True
        '
        'chkNotifyNow
        '
        Me.chkNotifyNow.AutoSize = True
        Me.chkNotifyNow.Location = New System.Drawing.Point(98, 153)
        Me.chkNotifyNow.Name = "chkNotifyNow"
        Me.chkNotifyNow.Size = New System.Drawing.Size(112, 17)
        Me.chkNotifyNow.TabIndex = 19
        Me.chkNotifyNow.Text = "When skill finishes"
        Me.chkNotifyNow.UseVisualStyleBackColor = True
        '
        'lblNotifyMe
        '
        Me.lblNotifyMe.AutoSize = True
        Me.lblNotifyMe.Location = New System.Drawing.Point(18, 154)
        Me.lblNotifyMe.Name = "lblNotifyMe"
        Me.lblNotifyMe.Size = New System.Drawing.Size(57, 13)
        Me.lblNotifyMe.TabIndex = 18
        Me.lblNotifyMe.Text = "Notify Me:"
        '
        'btnSoundTest
        '
        Me.btnSoundTest.Location = New System.Drawing.Point(547, 117)
        Me.btnSoundTest.Name = "btnSoundTest"
        Me.btnSoundTest.Size = New System.Drawing.Size(36, 20)
        Me.btnSoundTest.TabIndex = 17
        Me.btnSoundTest.Text = "Test"
        Me.btnSoundTest.UseVisualStyleBackColor = True
        '
        'btnSelectSoundFile
        '
        Me.btnSelectSoundFile.Location = New System.Drawing.Point(492, 117)
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
        Me.lblSoundFile.Location = New System.Drawing.Point(163, 117)
        Me.lblSoundFile.Name = "lblSoundFile"
        Me.lblSoundFile.Size = New System.Drawing.Size(323, 20)
        Me.lblSoundFile.TabIndex = 15
        '
        'chkNotifySound
        '
        Me.chkNotifySound.AutoSize = True
        Me.chkNotifySound.Location = New System.Drawing.Point(21, 117)
        Me.chkNotifySound.Name = "chkNotifySound"
        Me.chkNotifySound.Size = New System.Drawing.Size(136, 17)
        Me.chkNotifySound.TabIndex = 14
        Me.chkNotifySound.Text = "Play Sound Notification"
        Me.chkNotifySound.UseVisualStyleBackColor = True
        '
        'lblNotifyOffset
        '
        Me.lblNotifyOffset.AutoSize = True
        Me.lblNotifyOffset.Location = New System.Drawing.Point(18, 184)
        Me.lblNotifyOffset.Name = "lblNotifyOffset"
        Me.lblNotifyOffset.Size = New System.Drawing.Size(126, 13)
        Me.lblNotifyOffset.TabIndex = 13
        Me.lblNotifyOffset.Text = "Early Notification Offset:"
        '
        'trackNotifyOffset
        '
        Me.trackNotifyOffset.BackColor = System.Drawing.SystemColors.Control
        Me.trackNotifyOffset.Location = New System.Drawing.Point(21, 200)
        Me.trackNotifyOffset.Maximum = 1800
        Me.trackNotifyOffset.Name = "trackNotifyOffset"
        Me.trackNotifyOffset.Size = New System.Drawing.Size(450, 45)
        Me.trackNotifyOffset.TabIndex = 12
        Me.trackNotifyOffset.TickFrequency = 30
        Me.trackNotifyOffset.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'chkNotifyEmail
        '
        Me.chkNotifyEmail.AutoSize = True
        Me.chkNotifyEmail.Location = New System.Drawing.Point(400, 89)
        Me.chkNotifyEmail.Name = "chkNotifyEmail"
        Me.chkNotifyEmail.Size = New System.Drawing.Size(136, 17)
        Me.chkNotifyEmail.TabIndex = 5
        Me.chkNotifyEmail.Text = "Send E-Mail Notifcation"
        Me.chkNotifyEmail.UseVisualStyleBackColor = True
        '
        'chkNotifyDialog
        '
        Me.chkNotifyDialog.AutoSize = True
        Me.chkNotifyDialog.Location = New System.Drawing.Point(231, 89)
        Me.chkNotifyDialog.Name = "chkNotifyDialog"
        Me.chkNotifyDialog.Size = New System.Drawing.Size(162, 17)
        Me.chkNotifyDialog.TabIndex = 4
        Me.chkNotifyDialog.Text = "Show Dialog Box Notification"
        Me.chkNotifyDialog.UseVisualStyleBackColor = True
        '
        'chkNotifyToolTip
        '
        Me.chkNotifyToolTip.AutoSize = True
        Me.chkNotifyToolTip.Location = New System.Drawing.Point(21, 89)
        Me.chkNotifyToolTip.Name = "chkNotifyToolTip"
        Me.chkNotifyToolTip.Size = New System.Drawing.Size(205, 17)
        Me.chkNotifyToolTip.TabIndex = 3
        Me.chkNotifyToolTip.Text = "Show System Tray Popup Notification"
        Me.chkNotifyToolTip.UseVisualStyleBackColor = True
        '
        'nudShutdownNotifyPeriod
        '
        Me.nudShutdownNotifyPeriod.Location = New System.Drawing.Point(219, 53)
        Me.nudShutdownNotifyPeriod.Maximum = New Decimal(New Integer() {72, 0, 0, 0})
        Me.nudShutdownNotifyPeriod.Name = "nudShutdownNotifyPeriod"
        Me.nudShutdownNotifyPeriod.Size = New System.Drawing.Size(68, 21)
        Me.nudShutdownNotifyPeriod.TabIndex = 2
        Me.nudShutdownNotifyPeriod.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'lblShutdownNotifyPeriod
        '
        Me.lblShutdownNotifyPeriod.AutoSize = True
        Me.lblShutdownNotifyPeriod.Location = New System.Drawing.Point(71, 55)
        Me.lblShutdownNotifyPeriod.Name = "lblShutdownNotifyPeriod"
        Me.lblShutdownNotifyPeriod.Size = New System.Drawing.Size(136, 13)
        Me.lblShutdownNotifyPeriod.TabIndex = 1
        Me.lblShutdownNotifyPeriod.Text = "Notification Period (hours):"
        '
        'chkShutdownNotify
        '
        Me.chkShutdownNotify.AutoSize = True
        Me.chkShutdownNotify.Location = New System.Drawing.Point(21, 30)
        Me.chkShutdownNotify.Name = "chkShutdownNotify"
        Me.chkShutdownNotify.Size = New System.Drawing.Size(339, 17)
        Me.chkShutdownNotify.TabIndex = 0
        Me.chkShutdownNotify.Text = "Notify of imminent completion of skill training on EveHQ shutdown"
        Me.chkShutdownNotify.UseVisualStyleBackColor = True
        '
        'gbEmail
        '
        Me.gbEmail.Controls.Add(Me.lblSenderAddress)
        Me.gbEmail.Controls.Add(Me.txtSenderAddress)
        Me.gbEmail.Controls.Add(Me.txtSMTPPort)
        Me.gbEmail.Controls.Add(Me.lblSMTPPort)
        Me.gbEmail.Controls.Add(Me.btnTestEmail)
        Me.gbEmail.Controls.Add(Me.lblEmailPassword)
        Me.gbEmail.Controls.Add(Me.txtEmailPassword)
        Me.gbEmail.Controls.Add(Me.txtEmailUsername)
        Me.gbEmail.Controls.Add(Me.lblEmailUsername)
        Me.gbEmail.Controls.Add(Me.chkSMTPAuthentication)
        Me.gbEmail.Controls.Add(Me.lblEMailAddress)
        Me.gbEmail.Controls.Add(Me.txtEmailAddress)
        Me.gbEmail.Controls.Add(Me.txtSMTPServer)
        Me.gbEmail.Controls.Add(Me.lblSMTPServer)
        Me.gbEmail.Location = New System.Drawing.Point(194, 12)
        Me.gbEmail.Name = "gbEmail"
        Me.gbEmail.Size = New System.Drawing.Size(693, 489)
        Me.gbEmail.TabIndex = 6
        Me.gbEmail.TabStop = False
        Me.gbEmail.Text = "E-Mail Options"
        Me.gbEmail.Visible = False
        '
        'lblSenderAddress
        '
        Me.lblSenderAddress.AutoSize = True
        Me.lblSenderAddress.Location = New System.Drawing.Point(18, 82)
        Me.lblSenderAddress.Name = "lblSenderAddress"
        Me.lblSenderAddress.Size = New System.Drawing.Size(87, 13)
        Me.lblSenderAddress.TabIndex = 13
        Me.lblSenderAddress.Text = "Sender Address:"
        '
        'txtSenderAddress
        '
        Me.txtSenderAddress.Location = New System.Drawing.Point(117, 79)
        Me.txtSenderAddress.Name = "txtSenderAddress"
        Me.txtSenderAddress.Size = New System.Drawing.Size(331, 21)
        Me.txtSenderAddress.TabIndex = 12
        '
        'txtSMTPPort
        '
        Me.txtSMTPPort.Location = New System.Drawing.Point(117, 52)
        Me.txtSMTPPort.Name = "txtSMTPPort"
        Me.txtSMTPPort.Size = New System.Drawing.Size(86, 21)
        Me.txtSMTPPort.TabIndex = 11
        '
        'lblSMTPPort
        '
        Me.lblSMTPPort.AutoSize = True
        Me.lblSMTPPort.Location = New System.Drawing.Point(18, 55)
        Me.lblSMTPPort.Name = "lblSMTPPort"
        Me.lblSMTPPort.Size = New System.Drawing.Size(60, 13)
        Me.lblSMTPPort.TabIndex = 10
        Me.lblSMTPPort.Text = "SMTP Port:"
        '
        'btnTestEmail
        '
        Me.btnTestEmail.Location = New System.Drawing.Point(19, 438)
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
        Me.lblEmailPassword.Location = New System.Drawing.Point(52, 327)
        Me.lblEmailPassword.Name = "lblEmailPassword"
        Me.lblEmailPassword.Size = New System.Drawing.Size(57, 13)
        Me.lblEmailPassword.TabIndex = 8
        Me.lblEmailPassword.Text = "Password:"
        '
        'txtEmailPassword
        '
        Me.txtEmailPassword.Enabled = False
        Me.txtEmailPassword.Location = New System.Drawing.Point(117, 323)
        Me.txtEmailPassword.Name = "txtEmailPassword"
        Me.txtEmailPassword.Size = New System.Drawing.Size(250, 21)
        Me.txtEmailPassword.TabIndex = 7
        '
        'txtEmailUsername
        '
        Me.txtEmailUsername.Enabled = False
        Me.txtEmailUsername.Location = New System.Drawing.Point(117, 297)
        Me.txtEmailUsername.Name = "txtEmailUsername"
        Me.txtEmailUsername.Size = New System.Drawing.Size(250, 21)
        Me.txtEmailUsername.TabIndex = 6
        '
        'lblEmailUsername
        '
        Me.lblEmailUsername.AutoSize = True
        Me.lblEmailUsername.Enabled = False
        Me.lblEmailUsername.Location = New System.Drawing.Point(52, 301)
        Me.lblEmailUsername.Name = "lblEmailUsername"
        Me.lblEmailUsername.Size = New System.Drawing.Size(59, 13)
        Me.lblEmailUsername.TabIndex = 5
        Me.lblEmailUsername.Text = "Username:"
        '
        'chkSMTPAuthentication
        '
        Me.chkSMTPAuthentication.AutoSize = True
        Me.chkSMTPAuthentication.Location = New System.Drawing.Point(21, 274)
        Me.chkSMTPAuthentication.Name = "chkSMTPAuthentication"
        Me.chkSMTPAuthentication.Size = New System.Drawing.Size(146, 17)
        Me.chkSMTPAuthentication.TabIndex = 4
        Me.chkSMTPAuthentication.Text = "Use SMTP Authentication"
        Me.chkSMTPAuthentication.UseVisualStyleBackColor = True
        '
        'lblEMailAddress
        '
        Me.lblEMailAddress.Location = New System.Drawing.Point(18, 109)
        Me.lblEMailAddress.Name = "lblEMailAddress"
        Me.lblEMailAddress.Size = New System.Drawing.Size(93, 82)
        Me.lblEMailAddress.TabIndex = 3
        Me.lblEMailAddress.Text = "E-Mail Addresses:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Separate with ;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'txtEmailAddress
        '
        Me.txtEmailAddress.Location = New System.Drawing.Point(117, 106)
        Me.txtEmailAddress.Multiline = True
        Me.txtEmailAddress.Name = "txtEmailAddress"
        Me.txtEmailAddress.Size = New System.Drawing.Size(331, 158)
        Me.txtEmailAddress.TabIndex = 2
        '
        'txtSMTPServer
        '
        Me.txtSMTPServer.Location = New System.Drawing.Point(117, 26)
        Me.txtSMTPServer.Name = "txtSMTPServer"
        Me.txtSMTPServer.Size = New System.Drawing.Size(331, 21)
        Me.txtSMTPServer.TabIndex = 1
        '
        'lblSMTPServer
        '
        Me.lblSMTPServer.AutoSize = True
        Me.lblSMTPServer.Location = New System.Drawing.Point(18, 29)
        Me.lblSMTPServer.Name = "lblSMTPServer"
        Me.lblSMTPServer.Size = New System.Drawing.Size(72, 13)
        Me.lblSMTPServer.TabIndex = 0
        Me.lblSMTPServer.Text = "SMTP Server:"
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
        TreeNode2.Name = "nodeColours"
        TreeNode2.Text = "Colours & Styles"
        TreeNode3.Name = "nodeDashboard"
        TreeNode3.Text = "Dashboard"
        TreeNode4.Name = "nodeDatabaseFormat"
        TreeNode4.Text = "Database Format"
        TreeNode5.Name = "nodeEmail"
        TreeNode5.Text = "E-Mail"
        TreeNode6.Name = "nodeEveAccounts"
        TreeNode6.Text = "Eve Accounts"
        TreeNode7.Name = "nodeEveFolders"
        TreeNode7.Text = "Eve Folders"
        TreeNode8.Name = "nodeEveServer"
        TreeNode8.Text = "Eve API & Server"
        TreeNode9.Name = "nodeFTPAccounts"
        TreeNode9.Text = "FTP Accounts"
        TreeNode10.Name = "nodeG15"
        TreeNode10.Text = "G15 Display"
        TreeNode11.Name = "nodeIGB"
        TreeNode11.Text = "IGB"
        TreeNode12.Name = "nodeNotifications"
        TreeNode12.Text = "Notifications"
        TreeNode13.Name = "nodePilots"
        TreeNode13.Text = "Pilots"
        TreeNode14.Name = "nodePlugins"
        TreeNode14.Text = "Plug Ins"
        TreeNode15.Name = "nodeProxyServer"
        TreeNode15.Text = "Proxy Server"
        TreeNode16.Name = "nodeTaskBarIcon"
        TreeNode16.Text = "Taskbar Icon"
        TreeNode17.Name = "nodeTrainingQueue"
        TreeNode17.Text = "Training Queue"
        Me.tvwSettings.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6, TreeNode7, TreeNode8, TreeNode9, TreeNode10, TreeNode11, TreeNode12, TreeNode13, TreeNode14, TreeNode15, TreeNode16, TreeNode17})
        Me.tvwSettings.Size = New System.Drawing.Size(176, 473)
        Me.tvwSettings.TabIndex = 27
        '
        'gbColours
        '
        Me.gbColours.Controls.Add(Me.txtCSVSeparator)
        Me.gbColours.Controls.Add(Me.lblCSVSeparatorChar)
        Me.gbColours.Controls.Add(Me.chkDisableVisualStyles)
        Me.gbColours.Controls.Add(Me.gbPilotScreenColours)
        Me.gbColours.Location = New System.Drawing.Point(398, 378)
        Me.gbColours.Name = "gbColours"
        Me.gbColours.Size = New System.Drawing.Size(131, 41)
        Me.gbColours.TabIndex = 28
        Me.gbColours.TabStop = False
        Me.gbColours.Text = "Colours"
        Me.gbColours.Visible = False
        '
        'txtCSVSeparator
        '
        Me.txtCSVSeparator.Location = New System.Drawing.Point(99, 329)
        Me.txtCSVSeparator.MaxLength = 1
        Me.txtCSVSeparator.Name = "txtCSVSeparator"
        Me.txtCSVSeparator.ShortcutsEnabled = False
        Me.txtCSVSeparator.Size = New System.Drawing.Size(46, 21)
        Me.txtCSVSeparator.TabIndex = 40
        '
        'lblCSVSeparatorChar
        '
        Me.lblCSVSeparatorChar.AutoSize = True
        Me.lblCSVSeparatorChar.Location = New System.Drawing.Point(16, 332)
        Me.lblCSVSeparatorChar.Name = "lblCSVSeparatorChar"
        Me.lblCSVSeparatorChar.Size = New System.Drawing.Size(77, 13)
        Me.lblCSVSeparatorChar.TabIndex = 39
        Me.lblCSVSeparatorChar.Text = "CSV Separator"
        '
        'chkDisableVisualStyles
        '
        Me.chkDisableVisualStyles.AutoSize = True
        Me.chkDisableVisualStyles.Location = New System.Drawing.Point(19, 305)
        Me.chkDisableVisualStyles.Name = "chkDisableVisualStyles"
        Me.chkDisableVisualStyles.Size = New System.Drawing.Size(122, 17)
        Me.chkDisableVisualStyles.TabIndex = 38
        Me.chkDisableVisualStyles.Text = "Disable Visual Styles"
        Me.chkDisableVisualStyles.UseVisualStyleBackColor = True
        '
        'gbG15
        '
        Me.gbG15.Controls.Add(Me.nudCycleTime)
        Me.gbG15.Controls.Add(Me.lblCycleTime)
        Me.gbG15.Controls.Add(Me.chkCyclePilots)
        Me.gbG15.Controls.Add(Me.chkActivateG15)
        Me.gbG15.Location = New System.Drawing.Point(385, 183)
        Me.gbG15.Name = "gbG15"
        Me.gbG15.Size = New System.Drawing.Size(95, 37)
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
        Me.nudCycleTime.Size = New System.Drawing.Size(73, 21)
        Me.nudCycleTime.TabIndex = 5
        '
        'lblCycleTime
        '
        Me.lblCycleTime.AutoSize = True
        Me.lblCycleTime.Location = New System.Drawing.Point(213, 73)
        Me.lblCycleTime.Name = "lblCycleTime"
        Me.lblCycleTime.Size = New System.Drawing.Size(78, 13)
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
        'gbTaskbarIcon
        '
        Me.gbTaskbarIcon.Controls.Add(Me.cboTaskbarIconMode)
        Me.gbTaskbarIcon.Controls.Add(Me.lblTaskbarIconMode)
        Me.gbTaskbarIcon.Location = New System.Drawing.Point(634, 414)
        Me.gbTaskbarIcon.Name = "gbTaskbarIcon"
        Me.gbTaskbarIcon.Size = New System.Drawing.Size(100, 39)
        Me.gbTaskbarIcon.TabIndex = 32
        Me.gbTaskbarIcon.TabStop = False
        Me.gbTaskbarIcon.Text = "Taskbar Icon Settings"
        Me.gbTaskbarIcon.Visible = False
        '
        'cboTaskbarIconMode
        '
        Me.cboTaskbarIconMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaskbarIconMode.FormattingEnabled = True
        Me.cboTaskbarIconMode.Items.AddRange(New Object() {"Simple (Tooltip)", "Enhanced (Form)"})
        Me.cboTaskbarIconMode.Location = New System.Drawing.Point(125, 35)
        Me.cboTaskbarIconMode.Name = "cboTaskbarIconMode"
        Me.cboTaskbarIconMode.Size = New System.Drawing.Size(175, 21)
        Me.cboTaskbarIconMode.TabIndex = 1
        '
        'lblTaskbarIconMode
        '
        Me.lblTaskbarIconMode.AutoSize = True
        Me.lblTaskbarIconMode.Location = New System.Drawing.Point(16, 38)
        Me.lblTaskbarIconMode.Name = "lblTaskbarIconMode"
        Me.lblTaskbarIconMode.Size = New System.Drawing.Size(102, 13)
        Me.lblTaskbarIconMode.TabIndex = 0
        Me.lblTaskbarIconMode.Text = "Taskbar Icon Mode:"
        '
        'gbDashboard
        '
        Me.gbDashboard.Controls.Add(Me.gbOtherDBOptions)
        Me.gbDashboard.Controls.Add(Me.dbDashboardConfig)
        Me.gbDashboard.Controls.Add(Me.gbDashboardColours)
        Me.gbDashboard.Location = New System.Drawing.Point(615, 33)
        Me.gbDashboard.Name = "gbDashboard"
        Me.gbDashboard.Size = New System.Drawing.Size(96, 26)
        Me.gbDashboard.TabIndex = 33
        Me.gbDashboard.TabStop = False
        Me.gbDashboard.Text = "Dashboard"
        Me.gbDashboard.Visible = False
        '
        'gbOtherDBOptions
        '
        Me.gbOtherDBOptions.Controls.Add(Me.cboTickerLocation)
        Me.gbOtherDBOptions.Controls.Add(Me.lblTickerLocation)
        Me.gbOtherDBOptions.Controls.Add(Me.chkShowPriceTicker)
        Me.gbOtherDBOptions.Location = New System.Drawing.Point(17, 259)
        Me.gbOtherDBOptions.Name = "gbOtherDBOptions"
        Me.gbOtherDBOptions.Size = New System.Drawing.Size(215, 229)
        Me.gbOtherDBOptions.TabIndex = 40
        Me.gbOtherDBOptions.TabStop = False
        Me.gbOtherDBOptions.Text = "Other Dashboard Options"
        '
        'cboTickerLocation
        '
        Me.cboTickerLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTickerLocation.FormattingEnabled = True
        Me.cboTickerLocation.Items.AddRange(New Object() {"Top", "Bottom"})
        Me.cboTickerLocation.Location = New System.Drawing.Point(111, 46)
        Me.cboTickerLocation.Name = "cboTickerLocation"
        Me.cboTickerLocation.Size = New System.Drawing.Size(83, 21)
        Me.cboTickerLocation.TabIndex = 2
        '
        'lblTickerLocation
        '
        Me.lblTickerLocation.AutoSize = True
        Me.lblTickerLocation.Location = New System.Drawing.Point(23, 49)
        Me.lblTickerLocation.Name = "lblTickerLocation"
        Me.lblTickerLocation.Size = New System.Drawing.Size(82, 13)
        Me.lblTickerLocation.TabIndex = 1
        Me.lblTickerLocation.Text = "Ticker Location:"
        '
        'chkShowPriceTicker
        '
        Me.chkShowPriceTicker.AutoSize = True
        Me.chkShowPriceTicker.Location = New System.Drawing.Point(20, 24)
        Me.chkShowPriceTicker.Name = "chkShowPriceTicker"
        Me.chkShowPriceTicker.Size = New System.Drawing.Size(109, 17)
        Me.chkShowPriceTicker.TabIndex = 0
        Me.chkShowPriceTicker.Text = "Show Price Ticker"
        Me.chkShowPriceTicker.UseVisualStyleBackColor = True
        '
        'dbDashboardConfig
        '
        Me.dbDashboardConfig.Controls.Add(Me.lblWidgetTypes)
        Me.dbDashboardConfig.Controls.Add(Me.cboWidgets)
        Me.dbDashboardConfig.Controls.Add(Me.btnAddWidget)
        Me.dbDashboardConfig.Controls.Add(Me.btnRemoveWidget)
        Me.dbDashboardConfig.Controls.Add(Me.lvWidgets)
        Me.dbDashboardConfig.Controls.Add(Me.lblCurrentWidgets)
        Me.dbDashboardConfig.Location = New System.Drawing.Point(238, 27)
        Me.dbDashboardConfig.Name = "dbDashboardConfig"
        Me.dbDashboardConfig.Size = New System.Drawing.Size(449, 461)
        Me.dbDashboardConfig.TabIndex = 39
        Me.dbDashboardConfig.TabStop = False
        Me.dbDashboardConfig.Text = "Dashboard Configuration"
        '
        'lblWidgetTypes
        '
        Me.lblWidgetTypes.AutoSize = True
        Me.lblWidgetTypes.Location = New System.Drawing.Point(14, 435)
        Me.lblWidgetTypes.Name = "lblWidgetTypes"
        Me.lblWidgetTypes.Size = New System.Drawing.Size(77, 13)
        Me.lblWidgetTypes.TabIndex = 5
        Me.lblWidgetTypes.Text = "Widget Types:"
        '
        'cboWidgets
        '
        Me.cboWidgets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWidgets.FormattingEnabled = True
        Me.cboWidgets.Items.AddRange(New Object() {"Pilot Information", "Skill Queue Information"})
        Me.cboWidgets.Location = New System.Drawing.Point(93, 432)
        Me.cboWidgets.Name = "cboWidgets"
        Me.cboWidgets.Size = New System.Drawing.Size(184, 21)
        Me.cboWidgets.Sorted = True
        Me.cboWidgets.TabIndex = 4
        '
        'btnAddWidget
        '
        Me.btnAddWidget.Location = New System.Drawing.Point(283, 430)
        Me.btnAddWidget.Name = "btnAddWidget"
        Me.btnAddWidget.Size = New System.Drawing.Size(75, 23)
        Me.btnAddWidget.TabIndex = 3
        Me.btnAddWidget.Text = "Add Widget"
        Me.btnAddWidget.UseVisualStyleBackColor = True
        '
        'btnRemoveWidget
        '
        Me.btnRemoveWidget.Location = New System.Drawing.Point(364, 430)
        Me.btnRemoveWidget.Name = "btnRemoveWidget"
        Me.btnRemoveWidget.Size = New System.Drawing.Size(75, 23)
        Me.btnRemoveWidget.TabIndex = 2
        Me.btnRemoveWidget.Text = "Remove"
        Me.btnRemoveWidget.UseVisualStyleBackColor = True
        '
        'lvWidgets
        '
        Me.lvWidgets.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colWidgetType, Me.colWidgetInfo})
        Me.lvWidgets.FullRowSelect = True
        Me.lvWidgets.GridLines = True
        Me.lvWidgets.Location = New System.Drawing.Point(11, 40)
        Me.lvWidgets.Name = "lvWidgets"
        Me.lvWidgets.Size = New System.Drawing.Size(428, 386)
        Me.lvWidgets.TabIndex = 1
        Me.lvWidgets.UseCompatibleStateImageBehavior = False
        Me.lvWidgets.View = System.Windows.Forms.View.Details
        '
        'colWidgetType
        '
        Me.colWidgetType.Text = "Widget Type"
        Me.colWidgetType.Width = 100
        '
        'colWidgetInfo
        '
        Me.colWidgetInfo.Text = "Widget information"
        Me.colWidgetInfo.Width = 300
        '
        'lblCurrentWidgets
        '
        Me.lblCurrentWidgets.AutoSize = True
        Me.lblCurrentWidgets.Location = New System.Drawing.Point(8, 24)
        Me.lblCurrentWidgets.Name = "lblCurrentWidgets"
        Me.lblCurrentWidgets.Size = New System.Drawing.Size(90, 13)
        Me.lblCurrentWidgets.TabIndex = 0
        Me.lblCurrentWidgets.Text = "Current Widgets:"
        '
        'gbDashboardColours
        '
        Me.gbDashboardColours.Controls.Add(Me.pbWidgetHeader2)
        Me.gbDashboardColours.Controls.Add(Me.lblWidgetHeader2)
        Me.gbDashboardColours.Controls.Add(Me.pbWidgetHeader1)
        Me.gbDashboardColours.Controls.Add(Me.lblWidgetHeader1)
        Me.gbDashboardColours.Controls.Add(Me.pbWidgetBorder)
        Me.gbDashboardColours.Controls.Add(Me.lblWidgetBorder)
        Me.gbDashboardColours.Controls.Add(Me.pbDBColor)
        Me.gbDashboardColours.Controls.Add(Me.lblDBColor)
        Me.gbDashboardColours.Controls.Add(Me.btnResetDBColors)
        Me.gbDashboardColours.Controls.Add(Me.pbWidgetMain2)
        Me.gbDashboardColours.Controls.Add(Me.lblWidgetMain2)
        Me.gbDashboardColours.Controls.Add(Me.pbWidgetMain1)
        Me.gbDashboardColours.Controls.Add(Me.lblWidgetMain1)
        Me.gbDashboardColours.Location = New System.Drawing.Point(17, 27)
        Me.gbDashboardColours.Name = "gbDashboardColours"
        Me.gbDashboardColours.Size = New System.Drawing.Size(215, 226)
        Me.gbDashboardColours.TabIndex = 38
        Me.gbDashboardColours.TabStop = False
        Me.gbDashboardColours.Text = "Dashboard Colours"
        '
        'pbWidgetHeader2
        '
        Me.pbWidgetHeader2.BackColor = System.Drawing.Color.LightGray
        Me.pbWidgetHeader2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbWidgetHeader2.Location = New System.Drawing.Point(159, 96)
        Me.pbWidgetHeader2.Name = "pbWidgetHeader2"
        Me.pbWidgetHeader2.Size = New System.Drawing.Size(24, 24)
        Me.pbWidgetHeader2.TabIndex = 60
        Me.pbWidgetHeader2.TabStop = False
        '
        'lblWidgetHeader2
        '
        Me.lblWidgetHeader2.AutoSize = True
        Me.lblWidgetHeader2.Location = New System.Drawing.Point(13, 99)
        Me.lblWidgetHeader2.Name = "lblWidgetHeader2"
        Me.lblWidgetHeader2.Size = New System.Drawing.Size(88, 13)
        Me.lblWidgetHeader2.TabIndex = 59
        Me.lblWidgetHeader2.Text = "Widget Header 2"
        '
        'pbWidgetHeader1
        '
        Me.pbWidgetHeader1.BackColor = System.Drawing.Color.DimGray
        Me.pbWidgetHeader1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbWidgetHeader1.Location = New System.Drawing.Point(159, 72)
        Me.pbWidgetHeader1.Name = "pbWidgetHeader1"
        Me.pbWidgetHeader1.Size = New System.Drawing.Size(24, 24)
        Me.pbWidgetHeader1.TabIndex = 58
        Me.pbWidgetHeader1.TabStop = False
        '
        'lblWidgetHeader1
        '
        Me.lblWidgetHeader1.AutoSize = True
        Me.lblWidgetHeader1.Location = New System.Drawing.Point(13, 75)
        Me.lblWidgetHeader1.Name = "lblWidgetHeader1"
        Me.lblWidgetHeader1.Size = New System.Drawing.Size(88, 13)
        Me.lblWidgetHeader1.TabIndex = 57
        Me.lblWidgetHeader1.Text = "Widget Header 1"
        '
        'pbWidgetBorder
        '
        Me.pbWidgetBorder.BackColor = System.Drawing.Color.Black
        Me.pbWidgetBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbWidgetBorder.Location = New System.Drawing.Point(159, 48)
        Me.pbWidgetBorder.Name = "pbWidgetBorder"
        Me.pbWidgetBorder.Size = New System.Drawing.Size(24, 24)
        Me.pbWidgetBorder.TabIndex = 56
        Me.pbWidgetBorder.TabStop = False
        '
        'lblWidgetBorder
        '
        Me.lblWidgetBorder.AutoSize = True
        Me.lblWidgetBorder.Location = New System.Drawing.Point(13, 51)
        Me.lblWidgetBorder.Name = "lblWidgetBorder"
        Me.lblWidgetBorder.Size = New System.Drawing.Size(76, 13)
        Me.lblWidgetBorder.TabIndex = 55
        Me.lblWidgetBorder.Text = "Widget Border"
        '
        'pbDBColor
        '
        Me.pbDBColor.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbDBColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbDBColor.Location = New System.Drawing.Point(159, 24)
        Me.pbDBColor.Name = "pbDBColor"
        Me.pbDBColor.Size = New System.Drawing.Size(24, 24)
        Me.pbDBColor.TabIndex = 54
        Me.pbDBColor.TabStop = False
        '
        'lblDBColor
        '
        Me.lblDBColor.AutoSize = True
        Me.lblDBColor.Location = New System.Drawing.Point(13, 27)
        Me.lblDBColor.Name = "lblDBColor"
        Me.lblDBColor.Size = New System.Drawing.Size(118, 13)
        Me.lblDBColor.TabIndex = 53
        Me.lblDBColor.Text = "Dashboard Background"
        '
        'btnResetDBColors
        '
        Me.btnResetDBColors.Location = New System.Drawing.Point(38, 190)
        Me.btnResetDBColors.Name = "btnResetDBColors"
        Me.btnResetDBColors.Size = New System.Drawing.Size(145, 23)
        Me.btnResetDBColors.TabIndex = 52
        Me.btnResetDBColors.Text = "Reset To Defaults"
        Me.btnResetDBColors.UseVisualStyleBackColor = True
        '
        'pbWidgetMain2
        '
        Me.pbWidgetMain2.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbWidgetMain2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbWidgetMain2.Location = New System.Drawing.Point(159, 144)
        Me.pbWidgetMain2.Name = "pbWidgetMain2"
        Me.pbWidgetMain2.Size = New System.Drawing.Size(24, 24)
        Me.pbWidgetMain2.TabIndex = 39
        Me.pbWidgetMain2.TabStop = False
        '
        'lblWidgetMain2
        '
        Me.lblWidgetMain2.AutoSize = True
        Me.lblWidgetMain2.Location = New System.Drawing.Point(13, 147)
        Me.lblWidgetMain2.Name = "lblWidgetMain2"
        Me.lblWidgetMain2.Size = New System.Drawing.Size(75, 13)
        Me.lblWidgetMain2.TabIndex = 38
        Me.lblWidgetMain2.Text = "Widget Main 2"
        '
        'pbWidgetMain1
        '
        Me.pbWidgetMain1.BackColor = System.Drawing.Color.White
        Me.pbWidgetMain1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbWidgetMain1.Location = New System.Drawing.Point(159, 120)
        Me.pbWidgetMain1.Name = "pbWidgetMain1"
        Me.pbWidgetMain1.Size = New System.Drawing.Size(24, 24)
        Me.pbWidgetMain1.TabIndex = 37
        Me.pbWidgetMain1.TabStop = False
        '
        'lblWidgetMain1
        '
        Me.lblWidgetMain1.AutoSize = True
        Me.lblWidgetMain1.Location = New System.Drawing.Point(13, 123)
        Me.lblWidgetMain1.Name = "lblWidgetMain1"
        Me.lblWidgetMain1.Size = New System.Drawing.Size(75, 13)
        Me.lblWidgetMain1.TabIndex = 36
        Me.lblWidgetMain1.Text = "Widget Main 1"
        '
        'chkNotifyEveMail
        '
        Me.chkNotifyEveMail.AutoSize = True
        Me.chkNotifyEveMail.Location = New System.Drawing.Point(21, 270)
        Me.chkNotifyEveMail.Name = "chkNotifyEveMail"
        Me.chkNotifyEveMail.Size = New System.Drawing.Size(133, 17)
        Me.chkNotifyEveMail.TabIndex = 21
        Me.chkNotifyEveMail.Text = "Notify on New EveMail"
        Me.chkNotifyEveMail.UseVisualStyleBackColor = True
        '
        'chkNotifyNotification
        '
        Me.chkNotifyNotification.AutoSize = True
        Me.chkNotifyNotification.Location = New System.Drawing.Point(231, 270)
        Me.chkNotifyNotification.Name = "chkNotifyNotification"
        Me.chkNotifyNotification.Size = New System.Drawing.Size(172, 17)
        Me.chkNotifyNotification.TabIndex = 22
        Me.chkNotifyNotification.Text = "Notify on New Eve Notification"
        Me.chkNotifyNotification.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(899, 524)
        Me.Controls.Add(Me.gbNotifications)
        Me.Controls.Add(Me.gbEmail)
        Me.Controls.Add(Me.gbEveServer)
        Me.Controls.Add(Me.gbDashboard)
        Me.Controls.Add(Me.gbDatabaseFormat)
        Me.Controls.Add(Me.gbEveAccounts)
        Me.Controls.Add(Me.gbTrainingQueue)
        Me.Controls.Add(Me.gbGeneral)
        Me.Controls.Add(Me.gbColours)
        Me.Controls.Add(Me.gbG15)
        Me.Controls.Add(Me.gbTaskbarIcon)
        Me.Controls.Add(Me.gbPilots)
        Me.Controls.Add(Me.gbEveFolders)
        Me.Controls.Add(Me.gbProxyServer)
        Me.Controls.Add(Me.gbPlugIns)
        Me.Controls.Add(Me.gbIGB)
        Me.Controls.Add(Me.tvwSettings)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.gbFTPAccounts)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        CType(Me.pbPilotSkillHighlight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotSkillText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotGroupText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotGroupBG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotLevel5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotPartial, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotCurrent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilotStandard, System.ComponentModel.ISupportInitialize).EndInit()
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
        CType(Me.nudDBTimeout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbAccess.ResumeLayout(False)
        Me.gbAccess.PerformLayout()
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
        CType(Me.nudShutdownNotifyPeriod, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbEmail.ResumeLayout(False)
        Me.gbEmail.PerformLayout()
        Me.gbColours.ResumeLayout(False)
        Me.gbColours.PerformLayout()
        Me.gbG15.ResumeLayout(False)
        Me.gbG15.PerformLayout()
        CType(Me.nudCycleTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxPrices.ResumeLayout(False)
        Me.gbTaskbarIcon.ResumeLayout(False)
        Me.gbTaskbarIcon.PerformLayout()
        Me.gbDashboard.ResumeLayout(False)
        Me.gbOtherDBOptions.ResumeLayout(False)
        Me.gbOtherDBOptions.PerformLayout()
        Me.dbDashboardConfig.ResumeLayout(False)
        Me.dbDashboardConfig.PerformLayout()
        Me.gbDashboardColours.ResumeLayout(False)
        Me.gbDashboardColours.PerformLayout()
        CType(Me.pbWidgetHeader2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbWidgetHeader1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbWidgetBorder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbDBColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbWidgetMain2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbWidgetMain1, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents gbEmail As System.Windows.Forms.GroupBox
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
    Friend WithEvents tvwSettings As System.Windows.Forms.TreeView
    Friend WithEvents gbColours As System.Windows.Forms.GroupBox
    Friend WithEvents cd1 As System.Windows.Forms.ColorDialog
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRefreshPlugins As System.Windows.Forms.Button
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
    Friend WithEvents ctxPrices As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPriceItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPriceAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceDelete As System.Windows.Forms.ToolStripMenuItem
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
    Friend WithEvents chkUseAppDirForDB As System.Windows.Forms.CheckBox
    Friend WithEvents txtAPIFileExtension As System.Windows.Forms.TextBox
    Friend WithEvents lblAPIFileExtension As System.Windows.Forms.Label
    Friend WithEvents txtSMTPPort As System.Windows.Forms.TextBox
    Friend WithEvents lblSMTPPort As System.Windows.Forms.Label
    Friend WithEvents gbTaskbarIcon As System.Windows.Forms.GroupBox
    Friend WithEvents cboTaskbarIconMode As System.Windows.Forms.ComboBox
    Friend WithEvents lblTaskbarIconMode As System.Windows.Forms.Label
    Friend WithEvents chkErrorReporting As System.Windows.Forms.CheckBox
    Friend WithEvents txtErrorRepEmail As System.Windows.Forms.TextBox
    Friend WithEvents lblErrorRepEmail As System.Windows.Forms.Label
    Friend WithEvents txtErrorRepName As System.Windows.Forms.TextBox
    Friend WithEvents lblErrorRepName As System.Windows.Forms.Label
    Friend WithEvents pbPilotSkillHighlight As System.Windows.Forms.PictureBox
    Friend WithEvents lblPilotSkillHighlight As System.Windows.Forms.Label
    Friend WithEvents pbPilotSkillText As System.Windows.Forms.PictureBox
    Friend WithEvents lblPilotSkillText As System.Windows.Forms.Label
    Friend WithEvents pbPilotGroupText As System.Windows.Forms.PictureBox
    Friend WithEvents lblPilotGroupText As System.Windows.Forms.Label
    Friend WithEvents pbPilotGroupBG As System.Windows.Forms.PictureBox
    Friend WithEvents lblPilotGroupBG As System.Windows.Forms.Label
    Friend WithEvents lblDatabaseTimeout As System.Windows.Forms.Label
    Friend WithEvents nudDBTimeout As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkShowCompletedSkills As System.Windows.Forms.CheckBox
    Friend WithEvents lblMDITabPosition As System.Windows.Forms.Label
    Friend WithEvents cboMDITabPosition As System.Windows.Forms.ComboBox
    Friend WithEvents lblTrainingBarPosition As System.Windows.Forms.Label
    Friend WithEvents cboTrainingBarPosition As System.Windows.Forms.ComboBox
    Friend WithEvents lblToolbarPosition As System.Windows.Forms.Label
    Friend WithEvents cboToolbarPosition As System.Windows.Forms.ComboBox
    Friend WithEvents chkDisableAutoConnections As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisableVisualStyles As System.Windows.Forms.CheckBox
    Friend WithEvents lblCSVSeparatorChar As System.Windows.Forms.Label
    Friend WithEvents txtCSVSeparator As System.Windows.Forms.TextBox
    Friend WithEvents gbDashboard As System.Windows.Forms.GroupBox
    Friend WithEvents gbDashboardColours As System.Windows.Forms.GroupBox
    Friend WithEvents pbWidgetHeader2 As System.Windows.Forms.PictureBox
    Friend WithEvents lblWidgetHeader2 As System.Windows.Forms.Label
    Friend WithEvents pbWidgetHeader1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblWidgetHeader1 As System.Windows.Forms.Label
    Friend WithEvents pbWidgetBorder As System.Windows.Forms.PictureBox
    Friend WithEvents lblWidgetBorder As System.Windows.Forms.Label
    Friend WithEvents pbDBColor As System.Windows.Forms.PictureBox
    Friend WithEvents lblDBColor As System.Windows.Forms.Label
    Friend WithEvents btnResetDBColors As System.Windows.Forms.Button
    Friend WithEvents pbWidgetMain2 As System.Windows.Forms.PictureBox
    Friend WithEvents lblWidgetMain2 As System.Windows.Forms.Label
    Friend WithEvents pbWidgetMain1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblWidgetMain1 As System.Windows.Forms.Label
    Friend WithEvents dbDashboardConfig As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemoveWidget As System.Windows.Forms.Button
    Friend WithEvents lvWidgets As System.Windows.Forms.ListView
    Friend WithEvents colWidgetType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colWidgetInfo As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblCurrentWidgets As System.Windows.Forms.Label
    Friend WithEvents lblWidgetTypes As System.Windows.Forms.Label
    Friend WithEvents cboWidgets As System.Windows.Forms.ComboBox
    Friend WithEvents btnAddWidget As System.Windows.Forms.Button
    Friend WithEvents gbOtherDBOptions As System.Windows.Forms.GroupBox
    Friend WithEvents cboTickerLocation As System.Windows.Forms.ComboBox
    Friend WithEvents lblTickerLocation As System.Windows.Forms.Label
    Friend WithEvents chkShowPriceTicker As System.Windows.Forms.CheckBox
    Friend WithEvents btnMoveDown As System.Windows.Forms.Button
    Friend WithEvents btnMoveUp As System.Windows.Forms.Button
    Friend WithEvents lvwColumns As System.Windows.Forms.ListView
    Friend WithEvents colQueueColumns As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblSenderAddress As System.Windows.Forms.Label
    Friend WithEvents txtSenderAddress As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chkAutoMailAPI As System.Windows.Forms.CheckBox
    Friend WithEvents chkNotifyNotification As System.Windows.Forms.CheckBox
    Friend WithEvents chkNotifyEveMail As System.Windows.Forms.CheckBox
End Class
