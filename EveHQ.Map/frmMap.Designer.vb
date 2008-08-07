<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMap
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMap))
        Me.lblSystemMain = New System.Windows.Forms.Label
        Me.cboSystem = New System.Windows.Forms.ComboBox
        Me.gbSystemInfo = New System.Windows.Forms.GroupBox
        Me.lblStations = New System.Windows.Forms.Label
        Me.lblIBelts = New System.Windows.Forms.Label
        Me.lblABelts = New System.Windows.Forms.Label
        Me.lblMoons = New System.Windows.Forms.Label
        Me.lblPlanets = New System.Windows.Forms.Label
        Me.lblStationsLbl = New System.Windows.Forms.Label
        Me.lblIBeltsLbl = New System.Windows.Forms.Label
        Me.lblABeltsLbl = New System.Windows.Forms.Label
        Me.lblMoonsLbl = New System.Windows.Forms.Label
        Me.lblPlanetsLbl = New System.Windows.Forms.Label
        Me.lblSovereigntyLevel = New System.Windows.Forms.Label
        Me.lblSovereigntyLevellbl = New System.Windows.Forms.Label
        Me.lblSovHolder = New System.Windows.Forms.Label
        Me.lblSovHolderLbl = New System.Windows.Forms.Label
        Me.lblRegion = New System.Windows.Forms.Label
        Me.lblRegionlbl = New System.Windows.Forms.Label
        Me.lblConst = New System.Windows.Forms.Label
        Me.lblConstlbl = New System.Windows.Forms.Label
        Me.lblGates = New System.Windows.Forms.Label
        Me.lblNoGates = New System.Windows.Forms.Label
        Me.lblNoGateslbl = New System.Windows.Forms.Label
        Me.lblEveSec = New System.Windows.Forms.Label
        Me.lblEveSeclbl = New System.Windows.Forms.Label
        Me.lblID = New System.Windows.Forms.Label
        Me.lblIDlbl = New System.Windows.Forms.Label
        Me.lblSecurity = New System.Windows.Forms.Label
        Me.lblSecuritylbl = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblNamelbl = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAddEnd = New System.Windows.Forms.Button
        Me.btnAddStart = New System.Windows.Forms.Button
        Me.lblEndSystem = New System.Windows.Forms.Label
        Me.lblStartSystem = New System.Windows.Forms.Label
        Me.chkAutoCalcRoute = New System.Windows.Forms.CheckBox
        Me.btnOptimalWP = New System.Windows.Forms.Button
        Me.btnRemoveWaypoint = New System.Windows.Forms.Button
        Me.lstWaypoints = New System.Windows.Forms.ListBox
        Me.btnAddWaypoint = New System.Windows.Forms.Button
        Me.tabMap = New System.Windows.Forms.TabPage
        Me.lblZoom = New System.Windows.Forms.Label
        Me.pbMap = New System.Windows.Forms.PictureBox
        Me.btnShowRoute = New System.Windows.Forms.Button
        Me.chkRoute = New System.Windows.Forms.CheckBox
        Me.lblPointerAccuracy = New System.Windows.Forms.Label
        Me.nudAccuracy = New System.Windows.Forms.NumericUpDown
        Me.btnReset = New System.Windows.Forms.Button
        Me.tabRoute = New System.Windows.Forms.TabPage
        Me.lblTotalFuel = New System.Windows.Forms.Label
        Me.lblEuclideanDistance = New System.Windows.Forms.Label
        Me.gbJumpDrive = New System.Windows.Forms.GroupBox
        Me.chkOverrideJF = New System.Windows.Forms.CheckBox
        Me.lblJF = New System.Windows.Forms.Label
        Me.cboJF = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.cboPilot = New System.Windows.Forms.ComboBox
        Me.chkOverrideJFC = New System.Windows.Forms.CheckBox
        Me.chkOverrideJDC = New System.Windows.Forms.CheckBox
        Me.lblJFC = New System.Windows.Forms.Label
        Me.lblJDC = New System.Windows.Forms.Label
        Me.lblShips = New System.Windows.Forms.Label
        Me.cboJFC = New System.Windows.Forms.ComboBox
        Me.cboJDC = New System.Windows.Forms.ComboBox
        Me.cboShips = New System.Windows.Forms.ComboBox
        Me.lblTotalDistance = New System.Windows.Forms.Label
        Me.lvwRoute = New System.Windows.Forms.ListView
        Me.colNo = New System.Windows.Forms.ColumnHeader
        Me.colSystem = New System.Windows.Forms.ColumnHeader
        Me.colConstellation = New System.Windows.Forms.ColumnHeader
        Me.colRegion = New System.Windows.Forms.ColumnHeader
        Me.colSecurity = New System.Windows.Forms.ColumnHeader
        Me.colDistance = New System.Windows.Forms.ColumnHeader
        Me.colFuel = New System.Windows.Forms.ColumnHeader
        Me.colCargo = New System.Windows.Forms.ColumnHeader
        Me.colSov = New System.Windows.Forms.ColumnHeader
        Me.ctxRoute = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCopyToClipboard = New System.Windows.Forms.ToolStripMenuItem
        Me.lblTimeTaken = New System.Windows.Forms.Label
        Me.lblJumps = New System.Windows.Forms.Label
        Me.lblRouteMode = New System.Windows.Forms.Label
        Me.btnCalculate = New System.Windows.Forms.Button
        Me.nudJumps = New System.Windows.Forms.NumericUpDown
        Me.cboRouteMode = New System.Windows.Forms.ComboBox
        Me.nudMaxSec = New System.Windows.Forms.NumericUpDown
        Me.nudMinSec = New System.Windows.Forms.NumericUpDown
        Me.lblMaxSec = New System.Windows.Forms.Label
        Me.lblMinSec = New System.Windows.Forms.Label
        Me.tabMapTool = New System.Windows.Forms.TabControl
        Me.tabCelestial = New System.Windows.Forms.TabPage
        Me.radCelRegion = New System.Windows.Forms.RadioButton
        Me.radCelConst = New System.Windows.Forms.RadioButton
        Me.radCelSystem = New System.Windows.Forms.RadioButton
        Me.SearchCelestial = New System.Windows.Forms.Button
        Me.lblCelBelts = New System.Windows.Forms.Label
        Me.lblCelMoons = New System.Windows.Forms.Label
        Me.lblCelPlanets = New System.Windows.Forms.Label
        Me.lvBelts = New System.Windows.Forms.ListView
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.lvMoons = New System.Windows.Forms.ListView
        Me.CoplMoonsName = New System.Windows.Forms.ColumnHeader
        Me.lvPlanets = New System.Windows.Forms.ListView
        Me.CoPlName = New System.Windows.Forms.ColumnHeader
        Me.tabStations = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboStation = New System.Windows.Forms.ComboBox
        Me.lvagnts = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.lblStationAgents = New System.Windows.Forms.Label
        Me.lblStationServicesLbl = New System.Windows.Forms.Label
        Me.lblStationServices = New System.Windows.Forms.Label
        Me.lblStationFaction = New System.Windows.Forms.Label
        Me.lblStationCorpLbl = New System.Windows.Forms.Label
        Me.lblStationFactionLbl = New System.Windows.Forms.Label
        Me.lblStationCorp = New System.Windows.Forms.Label
        Me.tabStationSearch = New System.Windows.Forms.TabPage
        Me.lvsts = New System.Windows.Forms.ListView
        Me.costssec = New System.Windows.Forms.ColumnHeader
        Me.costsname = New System.Windows.Forms.ColumnHeader
        Me.costscorp = New System.Windows.Forms.ColumnHeader
        Me.costsfact = New System.Windows.Forms.ColumnHeader
        Me.costsdist = New System.Windows.Forms.ColumnHeader
        Me.costsreg = New System.Windows.Forms.ColumnHeader
        Me.costsconst = New System.Windows.Forms.ColumnHeader
        Me.costssys = New System.Windows.Forms.ColumnHeader
        Me.cbstsins = New System.Windows.Forms.CheckBox
        Me.cbstslps = New System.Windows.Forms.CheckBox
        Me.cbstsfit = New System.Windows.Forms.CheckBox
        Me.cbstslab = New System.Windows.Forms.CheckBox
        Me.cbstsfac = New System.Windows.Forms.CheckBox
        Me.cbstsclon = New System.Windows.Forms.CheckBox
        Me.cbstsref = New System.Windows.Forms.CheckBox
        Me.cbstsrep = New System.Windows.Forms.CheckBox
        Me.cbstsreg = New System.Windows.Forms.CheckBox
        Me.cbstsconst = New System.Windows.Forms.CheckBox
        Me.cbstssys = New System.Windows.Forms.CheckBox
        Me.tabAgentSearch = New System.Windows.Forms.TabPage
        Me.btnSearchAgents = New System.Windows.Forms.Button
        Me.chkLevel3Agent = New System.Windows.Forms.CheckBox
        Me.chkLevel4Agent = New System.Windows.Forms.CheckBox
        Me.chkLevel1Agent = New System.Windows.Forms.CheckBox
        Me.chkLevel5Agent = New System.Windows.Forms.CheckBox
        Me.chkLevel2Agent = New System.Windows.Forms.CheckBox
        Me.lblagsdiv = New System.Windows.Forms.Label
        Me.lblagscorp = New System.Windows.Forms.Label
        Me.lblagsfact = New System.Windows.Forms.Label
        Me.cboAgentDivision = New System.Windows.Forms.ComboBox
        Me.cboAgentCorp = New System.Windows.Forms.ComboBox
        Me.cboAgentFaction = New System.Windows.Forms.ComboBox
        Me.chkAgentNullSec = New System.Windows.Forms.CheckBox
        Me.chkAgentHighQ = New System.Windows.Forms.CheckBox
        Me.chkAgentRegion = New System.Windows.Forms.CheckBox
        Me.chkAgentConst = New System.Windows.Forms.CheckBox
        Me.chkAgentSystem = New System.Windows.Forms.CheckBox
        Me.chkAgentEmpire = New System.Windows.Forms.CheckBox
        Me.chkAgentLowQ = New System.Windows.Forms.CheckBox
        Me.chkAgentLowSec = New System.Windows.Forms.CheckBox
        Me.lvwAgents = New System.Windows.Forms.ListView
        Me.coagsname = New System.Windows.Forms.ColumnHeader
        Me.lgagscorp = New System.Windows.Forms.ColumnHeader
        Me.lgagsfact = New System.Windows.Forms.ColumnHeader
        Me.lgagslev = New System.Windows.Forms.ColumnHeader
        Me.lgagsqual = New System.Windows.Forms.ColumnHeader
        Me.lgagsdist = New System.Windows.Forms.ColumnHeader
        Me.lgagssec = New System.Windows.Forms.ColumnHeader
        Me.lgagsreg = New System.Windows.Forms.ColumnHeader
        Me.lgagsconst = New System.Windows.Forms.ColumnHeader
        Me.lgagssys = New System.Windows.Forms.ColumnHeader
        Me.lgagsstat = New System.Windows.Forms.ColumnHeader
        Me.lgagstype = New System.Windows.Forms.ColumnHeader
        Me.pbInfo = New System.Windows.Forms.PictureBox
        Me.lvwExclusions = New System.Windows.Forms.ListView
        Me.colExcludedName = New System.Windows.Forms.ColumnHeader
        Me.colExcludedType = New System.Windows.Forms.ColumnHeader
        Me.ctxExclude = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuExcludeSystem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExcludeConstellation = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExcludeRegion = New System.Windows.Forms.ToolStripMenuItem
        Me.btnRemoveExclusion = New System.Windows.Forms.Button
        Me.tabWaypointExclusions = New System.Windows.Forms.TabControl
        Me.tabSystem = New System.Windows.Forms.TabPage
        Me.tabExclusions = New System.Windows.Forms.TabPage
        Me.tabWaypoints = New System.Windows.Forms.TabPage
        Me.btnClearWP = New System.Windows.Forms.Button
        Me.lblConstMain = New System.Windows.Forms.Label
        Me.lblRegionMain = New System.Windows.Forms.Label
        Me.cboConst = New System.Windows.Forms.ComboBox
        Me.cboRegion = New System.Windows.Forms.ComboBox
        Me.btnExclude = New EveHQ.Map.SplitButton
        Me.gbSystemInfo.SuspendLayout()
        Me.tabMap.SuspendLayout()
        CType(Me.pbMap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAccuracy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabRoute.SuspendLayout()
        Me.gbJumpDrive.SuspendLayout()
        Me.ctxRoute.SuspendLayout()
        CType(Me.nudJumps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMaxSec, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMinSec, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMapTool.SuspendLayout()
        Me.tabCelestial.SuspendLayout()
        Me.tabStations.SuspendLayout()
        Me.tabStationSearch.SuspendLayout()
        Me.tabAgentSearch.SuspendLayout()
        CType(Me.pbInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxExclude.SuspendLayout()
        Me.tabWaypointExclusions.SuspendLayout()
        Me.tabSystem.SuspendLayout()
        Me.tabExclusions.SuspendLayout()
        Me.tabWaypoints.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSystemMain
        '
        Me.lblSystemMain.AutoSize = True
        Me.lblSystemMain.Location = New System.Drawing.Point(13, 69)
        Me.lblSystemMain.Name = "lblSystemMain"
        Me.lblSystemMain.Size = New System.Drawing.Size(44, 13)
        Me.lblSystemMain.TabIndex = 0
        Me.lblSystemMain.Text = "System:"
        '
        'cboSystem
        '
        Me.cboSystem.FormattingEnabled = True
        Me.cboSystem.Location = New System.Drawing.Point(63, 66)
        Me.cboSystem.Name = "cboSystem"
        Me.cboSystem.Size = New System.Drawing.Size(176, 21)
        Me.cboSystem.Sorted = True
        Me.cboSystem.TabIndex = 1
        '
        'gbSystemInfo
        '
        Me.gbSystemInfo.Controls.Add(Me.lblStations)
        Me.gbSystemInfo.Controls.Add(Me.lblIBelts)
        Me.gbSystemInfo.Controls.Add(Me.lblABelts)
        Me.gbSystemInfo.Controls.Add(Me.lblMoons)
        Me.gbSystemInfo.Controls.Add(Me.lblPlanets)
        Me.gbSystemInfo.Controls.Add(Me.lblStationsLbl)
        Me.gbSystemInfo.Controls.Add(Me.lblIBeltsLbl)
        Me.gbSystemInfo.Controls.Add(Me.lblABeltsLbl)
        Me.gbSystemInfo.Controls.Add(Me.lblMoonsLbl)
        Me.gbSystemInfo.Controls.Add(Me.lblPlanetsLbl)
        Me.gbSystemInfo.Controls.Add(Me.lblSovereigntyLevel)
        Me.gbSystemInfo.Controls.Add(Me.lblSovereigntyLevellbl)
        Me.gbSystemInfo.Controls.Add(Me.lblSovHolder)
        Me.gbSystemInfo.Controls.Add(Me.lblSovHolderLbl)
        Me.gbSystemInfo.Controls.Add(Me.lblRegion)
        Me.gbSystemInfo.Controls.Add(Me.lblRegionlbl)
        Me.gbSystemInfo.Controls.Add(Me.lblConst)
        Me.gbSystemInfo.Controls.Add(Me.lblConstlbl)
        Me.gbSystemInfo.Controls.Add(Me.lblGates)
        Me.gbSystemInfo.Controls.Add(Me.lblNoGates)
        Me.gbSystemInfo.Controls.Add(Me.lblNoGateslbl)
        Me.gbSystemInfo.Controls.Add(Me.lblEveSec)
        Me.gbSystemInfo.Controls.Add(Me.lblEveSeclbl)
        Me.gbSystemInfo.Controls.Add(Me.lblID)
        Me.gbSystemInfo.Controls.Add(Me.lblIDlbl)
        Me.gbSystemInfo.Controls.Add(Me.lblSecurity)
        Me.gbSystemInfo.Controls.Add(Me.lblSecuritylbl)
        Me.gbSystemInfo.Controls.Add(Me.lblName)
        Me.gbSystemInfo.Controls.Add(Me.lblNamelbl)
        Me.gbSystemInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSystemInfo.Location = New System.Drawing.Point(0, 0)
        Me.gbSystemInfo.Name = "gbSystemInfo"
        Me.gbSystemInfo.Size = New System.Drawing.Size(239, 413)
        Me.gbSystemInfo.TabIndex = 2
        Me.gbSystemInfo.TabStop = False
        Me.gbSystemInfo.Text = "System Information"
        '
        'lblStations
        '
        Me.lblStations.AutoSize = True
        Me.lblStations.Location = New System.Drawing.Point(82, 154)
        Me.lblStations.Name = "lblStations"
        Me.lblStations.Size = New System.Drawing.Size(63, 13)
        Me.lblStations.TabIndex = 52
        Me.lblStations.Text = "Placeholder"
        '
        'lblIBelts
        '
        Me.lblIBelts.AutoSize = True
        Me.lblIBelts.Location = New System.Drawing.Point(82, 141)
        Me.lblIBelts.Name = "lblIBelts"
        Me.lblIBelts.Size = New System.Drawing.Size(63, 13)
        Me.lblIBelts.TabIndex = 51
        Me.lblIBelts.Text = "Placeholder"
        '
        'lblABelts
        '
        Me.lblABelts.AutoSize = True
        Me.lblABelts.Location = New System.Drawing.Point(82, 128)
        Me.lblABelts.Name = "lblABelts"
        Me.lblABelts.Size = New System.Drawing.Size(63, 13)
        Me.lblABelts.TabIndex = 50
        Me.lblABelts.Text = "Placeholder"
        '
        'lblMoons
        '
        Me.lblMoons.AutoSize = True
        Me.lblMoons.Location = New System.Drawing.Point(82, 115)
        Me.lblMoons.Name = "lblMoons"
        Me.lblMoons.Size = New System.Drawing.Size(63, 13)
        Me.lblMoons.TabIndex = 49
        Me.lblMoons.Text = "Placeholder"
        '
        'lblPlanets
        '
        Me.lblPlanets.AutoSize = True
        Me.lblPlanets.Location = New System.Drawing.Point(82, 102)
        Me.lblPlanets.Name = "lblPlanets"
        Me.lblPlanets.Size = New System.Drawing.Size(63, 13)
        Me.lblPlanets.TabIndex = 48
        Me.lblPlanets.Text = "Placeholder"
        '
        'lblStationsLbl
        '
        Me.lblStationsLbl.AutoSize = True
        Me.lblStationsLbl.Location = New System.Drawing.Point(10, 154)
        Me.lblStationsLbl.Name = "lblStationsLbl"
        Me.lblStationsLbl.Size = New System.Drawing.Size(48, 13)
        Me.lblStationsLbl.TabIndex = 47
        Me.lblStationsLbl.Text = "Stations:"
        '
        'lblIBeltsLbl
        '
        Me.lblIBeltsLbl.AutoSize = True
        Me.lblIBeltsLbl.Location = New System.Drawing.Point(10, 141)
        Me.lblIBeltsLbl.Name = "lblIBeltsLbl"
        Me.lblIBeltsLbl.Size = New System.Drawing.Size(51, 13)
        Me.lblIBeltsLbl.TabIndex = 46
        Me.lblIBeltsLbl.Text = "Ice Belts:"
        '
        'lblABeltsLbl
        '
        Me.lblABeltsLbl.AutoSize = True
        Me.lblABeltsLbl.Location = New System.Drawing.Point(10, 128)
        Me.lblABeltsLbl.Name = "lblABeltsLbl"
        Me.lblABeltsLbl.Size = New System.Drawing.Size(33, 13)
        Me.lblABeltsLbl.TabIndex = 45
        Me.lblABeltsLbl.Text = "Belts:"
        '
        'lblMoonsLbl
        '
        Me.lblMoonsLbl.AutoSize = True
        Me.lblMoonsLbl.Location = New System.Drawing.Point(10, 115)
        Me.lblMoonsLbl.Name = "lblMoonsLbl"
        Me.lblMoonsLbl.Size = New System.Drawing.Size(42, 13)
        Me.lblMoonsLbl.TabIndex = 44
        Me.lblMoonsLbl.Text = "Moons:"
        '
        'lblPlanetsLbl
        '
        Me.lblPlanetsLbl.AutoSize = True
        Me.lblPlanetsLbl.Location = New System.Drawing.Point(10, 102)
        Me.lblPlanetsLbl.Name = "lblPlanetsLbl"
        Me.lblPlanetsLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblPlanetsLbl.TabIndex = 43
        Me.lblPlanetsLbl.Text = "Planets:"
        '
        'lblSovereigntyLevel
        '
        Me.lblSovereigntyLevel.AutoSize = True
        Me.lblSovereigntyLevel.Location = New System.Drawing.Point(82, 180)
        Me.lblSovereigntyLevel.Name = "lblSovereigntyLevel"
        Me.lblSovereigntyLevel.Size = New System.Drawing.Size(63, 13)
        Me.lblSovereigntyLevel.TabIndex = 41
        Me.lblSovereigntyLevel.Text = "Placeholder"
        '
        'lblSovereigntyLevellbl
        '
        Me.lblSovereigntyLevellbl.AutoSize = True
        Me.lblSovereigntyLevellbl.Location = New System.Drawing.Point(10, 181)
        Me.lblSovereigntyLevellbl.Name = "lblSovereigntyLevellbl"
        Me.lblSovereigntyLevellbl.Size = New System.Drawing.Size(58, 13)
        Me.lblSovereigntyLevellbl.TabIndex = 39
        Me.lblSovereigntyLevellbl.Text = "Sov Level:"
        '
        'lblSovHolder
        '
        Me.lblSovHolder.AutoSize = True
        Me.lblSovHolder.Location = New System.Drawing.Point(82, 167)
        Me.lblSovHolder.Name = "lblSovHolder"
        Me.lblSovHolder.Size = New System.Drawing.Size(63, 13)
        Me.lblSovHolder.TabIndex = 38
        Me.lblSovHolder.Text = "Placeholder"
        '
        'lblSovHolderLbl
        '
        Me.lblSovHolderLbl.AutoSize = True
        Me.lblSovHolderLbl.Location = New System.Drawing.Point(10, 168)
        Me.lblSovHolderLbl.Name = "lblSovHolderLbl"
        Me.lblSovHolderLbl.Size = New System.Drawing.Size(63, 13)
        Me.lblSovHolderLbl.TabIndex = 37
        Me.lblSovHolderLbl.Text = "Sov Holder:"
        '
        'lblRegion
        '
        Me.lblRegion.AutoSize = True
        Me.lblRegion.Location = New System.Drawing.Point(82, 50)
        Me.lblRegion.Name = "lblRegion"
        Me.lblRegion.Size = New System.Drawing.Size(63, 13)
        Me.lblRegion.TabIndex = 22
        Me.lblRegion.Text = "Placeholder"
        '
        'lblRegionlbl
        '
        Me.lblRegionlbl.AutoSize = True
        Me.lblRegionlbl.Location = New System.Drawing.Point(10, 50)
        Me.lblRegionlbl.Name = "lblRegionlbl"
        Me.lblRegionlbl.Size = New System.Drawing.Size(44, 13)
        Me.lblRegionlbl.TabIndex = 21
        Me.lblRegionlbl.Text = "Region:"
        '
        'lblConst
        '
        Me.lblConst.AutoSize = True
        Me.lblConst.Location = New System.Drawing.Point(82, 37)
        Me.lblConst.Name = "lblConst"
        Me.lblConst.Size = New System.Drawing.Size(63, 13)
        Me.lblConst.TabIndex = 20
        Me.lblConst.Text = "Placeholder"
        '
        'lblConstlbl
        '
        Me.lblConstlbl.AutoSize = True
        Me.lblConstlbl.Location = New System.Drawing.Point(10, 37)
        Me.lblConstlbl.Name = "lblConstlbl"
        Me.lblConstlbl.Size = New System.Drawing.Size(70, 13)
        Me.lblConstlbl.TabIndex = 19
        Me.lblConstlbl.Text = "Constellation:"
        '
        'lblGates
        '
        Me.lblGates.AutoSize = True
        Me.lblGates.Location = New System.Drawing.Point(82, 207)
        Me.lblGates.Name = "lblGates"
        Me.lblGates.Size = New System.Drawing.Size(63, 13)
        Me.lblGates.TabIndex = 18
        Me.lblGates.Text = "Placeholder"
        '
        'lblNoGates
        '
        Me.lblNoGates.AutoSize = True
        Me.lblNoGates.Location = New System.Drawing.Point(82, 194)
        Me.lblNoGates.Name = "lblNoGates"
        Me.lblNoGates.Size = New System.Drawing.Size(63, 13)
        Me.lblNoGates.TabIndex = 17
        Me.lblNoGates.Text = "Placeholder"
        '
        'lblNoGateslbl
        '
        Me.lblNoGateslbl.AutoSize = True
        Me.lblNoGateslbl.Location = New System.Drawing.Point(10, 194)
        Me.lblNoGateslbl.Name = "lblNoGateslbl"
        Me.lblNoGateslbl.Size = New System.Drawing.Size(38, 13)
        Me.lblNoGateslbl.TabIndex = 16
        Me.lblNoGateslbl.Text = "Gates:"
        '
        'lblEveSec
        '
        Me.lblEveSec.AutoSize = True
        Me.lblEveSec.Location = New System.Drawing.Point(82, 76)
        Me.lblEveSec.Name = "lblEveSec"
        Me.lblEveSec.Size = New System.Drawing.Size(63, 13)
        Me.lblEveSec.TabIndex = 15
        Me.lblEveSec.Text = "Placeholder"
        '
        'lblEveSeclbl
        '
        Me.lblEveSeclbl.AutoSize = True
        Me.lblEveSeclbl.Location = New System.Drawing.Point(10, 76)
        Me.lblEveSeclbl.Name = "lblEveSeclbl"
        Me.lblEveSeclbl.Size = New System.Drawing.Size(70, 13)
        Me.lblEveSeclbl.TabIndex = 14
        Me.lblEveSeclbl.Text = "Eve Security:"
        '
        'lblID
        '
        Me.lblID.AutoSize = True
        Me.lblID.Location = New System.Drawing.Point(82, 63)
        Me.lblID.Name = "lblID"
        Me.lblID.Size = New System.Drawing.Size(63, 13)
        Me.lblID.TabIndex = 11
        Me.lblID.Text = "Placeholder"
        '
        'lblIDlbl
        '
        Me.lblIDlbl.AutoSize = True
        Me.lblIDlbl.Location = New System.Drawing.Point(10, 63)
        Me.lblIDlbl.Name = "lblIDlbl"
        Me.lblIDlbl.Size = New System.Drawing.Size(21, 13)
        Me.lblIDlbl.TabIndex = 10
        Me.lblIDlbl.Text = "ID:"
        '
        'lblSecurity
        '
        Me.lblSecurity.AutoSize = True
        Me.lblSecurity.Location = New System.Drawing.Point(82, 89)
        Me.lblSecurity.Name = "lblSecurity"
        Me.lblSecurity.Size = New System.Drawing.Size(63, 13)
        Me.lblSecurity.TabIndex = 3
        Me.lblSecurity.Text = "Placeholder"
        '
        'lblSecuritylbl
        '
        Me.lblSecuritylbl.AutoSize = True
        Me.lblSecuritylbl.Location = New System.Drawing.Point(10, 89)
        Me.lblSecuritylbl.Name = "lblSecuritylbl"
        Me.lblSecuritylbl.Size = New System.Drawing.Size(73, 13)
        Me.lblSecuritylbl.TabIndex = 2
        Me.lblSecuritylbl.Text = "True Security:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(82, 24)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(63, 13)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Placeholder"
        '
        'lblNamelbl
        '
        Me.lblNamelbl.AutoSize = True
        Me.lblNamelbl.Location = New System.Drawing.Point(10, 24)
        Me.lblNamelbl.Name = "lblNamelbl"
        Me.lblNamelbl.Size = New System.Drawing.Size(38, 13)
        Me.lblNamelbl.TabIndex = 0
        Me.lblNamelbl.Text = "Name:"
        '
        'btnAddEnd
        '
        Me.btnAddEnd.Location = New System.Drawing.Point(67, 99)
        Me.btnAddEnd.Name = "btnAddEnd"
        Me.btnAddEnd.Size = New System.Drawing.Size(48, 40)
        Me.btnAddEnd.TabIndex = 63
        Me.btnAddEnd.Text = "Add as End"
        Me.btnAddEnd.UseVisualStyleBackColor = True
        '
        'btnAddStart
        '
        Me.btnAddStart.Location = New System.Drawing.Point(12, 99)
        Me.btnAddStart.Name = "btnAddStart"
        Me.btnAddStart.Size = New System.Drawing.Size(48, 40)
        Me.btnAddStart.TabIndex = 62
        Me.btnAddStart.Text = "Add as Start"
        Me.btnAddStart.UseVisualStyleBackColor = True
        '
        'lblEndSystem
        '
        Me.lblEndSystem.AutoSize = True
        Me.lblEndSystem.Location = New System.Drawing.Point(9, 167)
        Me.lblEndSystem.Name = "lblEndSystem"
        Me.lblEndSystem.Size = New System.Drawing.Size(66, 13)
        Me.lblEndSystem.TabIndex = 65
        Me.lblEndSystem.Text = "End System:"
        '
        'lblStartSystem
        '
        Me.lblStartSystem.AutoSize = True
        Me.lblStartSystem.Location = New System.Drawing.Point(9, 150)
        Me.lblStartSystem.Name = "lblStartSystem"
        Me.lblStartSystem.Size = New System.Drawing.Size(69, 13)
        Me.lblStartSystem.TabIndex = 64
        Me.lblStartSystem.Text = "Start System:"
        '
        'chkAutoCalcRoute
        '
        Me.chkAutoCalcRoute.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkAutoCalcRoute.AutoSize = True
        Me.chkAutoCalcRoute.Checked = True
        Me.chkAutoCalcRoute.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoCalcRoute.Location = New System.Drawing.Point(4, 391)
        Me.chkAutoCalcRoute.Name = "chkAutoCalcRoute"
        Me.chkAutoCalcRoute.Size = New System.Drawing.Size(167, 17)
        Me.chkAutoCalcRoute.TabIndex = 6
        Me.chkAutoCalcRoute.Text = "Automatically Calculate Route"
        Me.chkAutoCalcRoute.UseVisualStyleBackColor = True
        '
        'btnOptimalWP
        '
        Me.btnOptimalWP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOptimalWP.Location = New System.Drawing.Point(175, 366)
        Me.btnOptimalWP.Name = "btnOptimalWP"
        Me.btnOptimalWP.Size = New System.Drawing.Size(58, 23)
        Me.btnOptimalWP.TabIndex = 5
        Me.btnOptimalWP.Text = "Optimise WP"
        Me.btnOptimalWP.UseVisualStyleBackColor = True
        Me.btnOptimalWP.Visible = False
        '
        'btnRemoveWaypoint
        '
        Me.btnRemoveWaypoint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveWaypoint.Location = New System.Drawing.Point(3, 366)
        Me.btnRemoveWaypoint.Name = "btnRemoveWaypoint"
        Me.btnRemoveWaypoint.Size = New System.Drawing.Size(80, 23)
        Me.btnRemoveWaypoint.TabIndex = 4
        Me.btnRemoveWaypoint.Text = "Remove WP"
        Me.btnRemoveWaypoint.UseVisualStyleBackColor = True
        '
        'lstWaypoints
        '
        Me.lstWaypoints.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstWaypoints.FormattingEnabled = True
        Me.lstWaypoints.Location = New System.Drawing.Point(4, 6)
        Me.lstWaypoints.Name = "lstWaypoints"
        Me.lstWaypoints.Size = New System.Drawing.Size(229, 355)
        Me.lstWaypoints.TabIndex = 3
        '
        'btnAddWaypoint
        '
        Me.btnAddWaypoint.Location = New System.Drawing.Point(191, 99)
        Me.btnAddWaypoint.Name = "btnAddWaypoint"
        Me.btnAddWaypoint.Size = New System.Drawing.Size(48, 40)
        Me.btnAddWaypoint.TabIndex = 28
        Me.btnAddWaypoint.Text = "Add as WP"
        Me.btnAddWaypoint.UseVisualStyleBackColor = True
        '
        'tabMap
        '
        Me.tabMap.Controls.Add(Me.lblZoom)
        Me.tabMap.Controls.Add(Me.pbMap)
        Me.tabMap.Controls.Add(Me.btnShowRoute)
        Me.tabMap.Controls.Add(Me.chkRoute)
        Me.tabMap.Controls.Add(Me.lblPointerAccuracy)
        Me.tabMap.Controls.Add(Me.nudAccuracy)
        Me.tabMap.Controls.Add(Me.btnReset)
        Me.tabMap.Location = New System.Drawing.Point(4, 22)
        Me.tabMap.Name = "tabMap"
        Me.tabMap.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMap.Size = New System.Drawing.Size(657, 626)
        Me.tabMap.TabIndex = 0
        Me.tabMap.Text = "Map View"
        Me.tabMap.UseVisualStyleBackColor = True
        '
        'lblZoom
        '
        Me.lblZoom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblZoom.AutoSize = True
        Me.lblZoom.Location = New System.Drawing.Point(387, 603)
        Me.lblZoom.Name = "lblZoom"
        Me.lblZoom.Size = New System.Drawing.Size(81, 13)
        Me.lblZoom.TabIndex = 22
        Me.lblZoom.Text = "Zoom: 100.00%"
        '
        'pbMap
        '
        Me.pbMap.BackColor = System.Drawing.Color.Black
        Me.pbMap.Location = New System.Drawing.Point(7, 7)
        Me.pbMap.Name = "pbMap"
        Me.pbMap.Size = New System.Drawing.Size(500, 500)
        Me.pbMap.TabIndex = 21
        Me.pbMap.TabStop = False
        '
        'btnShowRoute
        '
        Me.btnShowRoute.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnShowRoute.Location = New System.Drawing.Point(3, 598)
        Me.btnShowRoute.Name = "btnShowRoute"
        Me.btnShowRoute.Size = New System.Drawing.Size(97, 22)
        Me.btnShowRoute.TabIndex = 20
        Me.btnShowRoute.Text = "Zoom to Route"
        Me.btnShowRoute.UseVisualStyleBackColor = True
        '
        'chkRoute
        '
        Me.chkRoute.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkRoute.AutoSize = True
        Me.chkRoute.Checked = True
        Me.chkRoute.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRoute.Location = New System.Drawing.Point(106, 602)
        Me.chkRoute.Name = "chkRoute"
        Me.chkRoute.Size = New System.Drawing.Size(85, 17)
        Me.chkRoute.TabIndex = 19
        Me.chkRoute.Text = "Show Route"
        Me.chkRoute.UseVisualStyleBackColor = True
        '
        'lblPointerAccuracy
        '
        Me.lblPointerAccuracy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPointerAccuracy.AutoSize = True
        Me.lblPointerAccuracy.Location = New System.Drawing.Point(282, 603)
        Me.lblPointerAccuracy.Name = "lblPointerAccuracy"
        Me.lblPointerAccuracy.Size = New System.Drawing.Size(55, 13)
        Me.lblPointerAccuracy.TabIndex = 18
        Me.lblPointerAccuracy.Text = "Accuracy:"
        '
        'nudAccuracy
        '
        Me.nudAccuracy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.nudAccuracy.Location = New System.Drawing.Point(343, 601)
        Me.nudAccuracy.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudAccuracy.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudAccuracy.Name = "nudAccuracy"
        Me.nudAccuracy.Size = New System.Drawing.Size(38, 20)
        Me.nudAccuracy.TabIndex = 17
        Me.nudAccuracy.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'btnReset
        '
        Me.btnReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReset.Location = New System.Drawing.Point(201, 597)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(75, 23)
        Me.btnReset.TabIndex = 16
        Me.btnReset.Text = "Reset"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'tabRoute
        '
        Me.tabRoute.Controls.Add(Me.lblTotalFuel)
        Me.tabRoute.Controls.Add(Me.lblEuclideanDistance)
        Me.tabRoute.Controls.Add(Me.gbJumpDrive)
        Me.tabRoute.Controls.Add(Me.lblTotalDistance)
        Me.tabRoute.Controls.Add(Me.lvwRoute)
        Me.tabRoute.Controls.Add(Me.lblTimeTaken)
        Me.tabRoute.Controls.Add(Me.lblJumps)
        Me.tabRoute.Controls.Add(Me.lblRouteMode)
        Me.tabRoute.Controls.Add(Me.btnCalculate)
        Me.tabRoute.Controls.Add(Me.nudJumps)
        Me.tabRoute.Controls.Add(Me.cboRouteMode)
        Me.tabRoute.Controls.Add(Me.nudMaxSec)
        Me.tabRoute.Controls.Add(Me.nudMinSec)
        Me.tabRoute.Controls.Add(Me.lblMaxSec)
        Me.tabRoute.Controls.Add(Me.lblMinSec)
        Me.tabRoute.Location = New System.Drawing.Point(4, 22)
        Me.tabRoute.Name = "tabRoute"
        Me.tabRoute.Padding = New System.Windows.Forms.Padding(3)
        Me.tabRoute.Size = New System.Drawing.Size(657, 626)
        Me.tabRoute.TabIndex = 1
        Me.tabRoute.Text = "Route Calculator"
        Me.tabRoute.UseVisualStyleBackColor = True
        '
        'lblTotalFuel
        '
        Me.lblTotalFuel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalFuel.AutoSize = True
        Me.lblTotalFuel.Location = New System.Drawing.Point(269, 605)
        Me.lblTotalFuel.Name = "lblTotalFuel"
        Me.lblTotalFuel.Size = New System.Drawing.Size(57, 13)
        Me.lblTotalFuel.TabIndex = 66
        Me.lblTotalFuel.Text = "Total Fuel:"
        '
        'lblEuclideanDistance
        '
        Me.lblEuclideanDistance.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblEuclideanDistance.AutoSize = True
        Me.lblEuclideanDistance.Location = New System.Drawing.Point(6, 605)
        Me.lblEuclideanDistance.Name = "lblEuclideanDistance"
        Me.lblEuclideanDistance.Size = New System.Drawing.Size(102, 13)
        Me.lblEuclideanDistance.TabIndex = 65
        Me.lblEuclideanDistance.Text = "Euclidean Distance:"
        '
        'gbJumpDrive
        '
        Me.gbJumpDrive.Controls.Add(Me.chkOverrideJF)
        Me.gbJumpDrive.Controls.Add(Me.lblJF)
        Me.gbJumpDrive.Controls.Add(Me.cboJF)
        Me.gbJumpDrive.Controls.Add(Me.lblPilot)
        Me.gbJumpDrive.Controls.Add(Me.cboPilot)
        Me.gbJumpDrive.Controls.Add(Me.chkOverrideJFC)
        Me.gbJumpDrive.Controls.Add(Me.chkOverrideJDC)
        Me.gbJumpDrive.Controls.Add(Me.lblJFC)
        Me.gbJumpDrive.Controls.Add(Me.lblJDC)
        Me.gbJumpDrive.Controls.Add(Me.lblShips)
        Me.gbJumpDrive.Controls.Add(Me.cboJFC)
        Me.gbJumpDrive.Controls.Add(Me.cboJDC)
        Me.gbJumpDrive.Controls.Add(Me.cboShips)
        Me.gbJumpDrive.Location = New System.Drawing.Point(225, 11)
        Me.gbJumpDrive.Name = "gbJumpDrive"
        Me.gbJumpDrive.Size = New System.Drawing.Size(368, 191)
        Me.gbJumpDrive.TabIndex = 64
        Me.gbJumpDrive.TabStop = False
        Me.gbJumpDrive.Text = "Jump Drive Specific Details"
        Me.gbJumpDrive.Visible = False
        '
        'chkOverrideJF
        '
        Me.chkOverrideJF.AutoSize = True
        Me.chkOverrideJF.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOverrideJF.Location = New System.Drawing.Point(265, 141)
        Me.chkOverrideJF.Name = "chkOverrideJF"
        Me.chkOverrideJF.Size = New System.Drawing.Size(80, 17)
        Me.chkOverrideJF.TabIndex = 68
        Me.chkOverrideJF.Text = "Override JF"
        Me.chkOverrideJF.UseVisualStyleBackColor = True
        '
        'lblJF
        '
        Me.lblJF.AutoSize = True
        Me.lblJF.Location = New System.Drawing.Point(9, 142)
        Me.lblJF.Name = "lblJF"
        Me.lblJF.Size = New System.Drawing.Size(81, 13)
        Me.lblJF.TabIndex = 67
        Me.lblJF.Text = "Jump Freighters"
        '
        'cboJF
        '
        Me.cboJF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboJF.FormattingEnabled = True
        Me.cboJF.Location = New System.Drawing.Point(138, 139)
        Me.cboJF.Name = "cboJF"
        Me.cboJF.Size = New System.Drawing.Size(121, 21)
        Me.cboJF.TabIndex = 66
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(9, 35)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(30, 13)
        Me.lblPilot.TabIndex = 65
        Me.lblPilot.Text = "Pilot:"
        '
        'cboPilot
        '
        Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilot.FormattingEnabled = True
        Me.cboPilot.Location = New System.Drawing.Point(138, 32)
        Me.cboPilot.Name = "cboPilot"
        Me.cboPilot.Size = New System.Drawing.Size(121, 21)
        Me.cboPilot.TabIndex = 64
        '
        'chkOverrideJFC
        '
        Me.chkOverrideJFC.AutoSize = True
        Me.chkOverrideJFC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOverrideJFC.Location = New System.Drawing.Point(265, 114)
        Me.chkOverrideJFC.Name = "chkOverrideJFC"
        Me.chkOverrideJFC.Size = New System.Drawing.Size(87, 17)
        Me.chkOverrideJFC.TabIndex = 63
        Me.chkOverrideJFC.Text = "Override JFC"
        Me.chkOverrideJFC.UseVisualStyleBackColor = True
        '
        'chkOverrideJDC
        '
        Me.chkOverrideJDC.AutoSize = True
        Me.chkOverrideJDC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOverrideJDC.Location = New System.Drawing.Point(265, 87)
        Me.chkOverrideJDC.Name = "chkOverrideJDC"
        Me.chkOverrideJDC.Size = New System.Drawing.Size(89, 17)
        Me.chkOverrideJDC.TabIndex = 62
        Me.chkOverrideJDC.Text = "Override JDC"
        Me.chkOverrideJDC.UseVisualStyleBackColor = True
        '
        'lblJFC
        '
        Me.lblJFC.AutoSize = True
        Me.lblJFC.Location = New System.Drawing.Point(9, 115)
        Me.lblJFC.Name = "lblJFC"
        Me.lblJFC.Size = New System.Drawing.Size(123, 13)
        Me.lblJFC.TabIndex = 61
        Me.lblJFC.Text = "Jump Fuel Conservation:"
        '
        'lblJDC
        '
        Me.lblJDC.AutoSize = True
        Me.lblJDC.Location = New System.Drawing.Point(9, 88)
        Me.lblJDC.Name = "lblJDC"
        Me.lblJDC.Size = New System.Drawing.Size(115, 13)
        Me.lblJDC.TabIndex = 60
        Me.lblJDC.Text = "Jump Drive Calibration:"
        '
        'lblShips
        '
        Me.lblShips.AutoSize = True
        Me.lblShips.Location = New System.Drawing.Point(9, 62)
        Me.lblShips.Name = "lblShips"
        Me.lblShips.Size = New System.Drawing.Size(58, 13)
        Me.lblShips.TabIndex = 59
        Me.lblShips.Text = "Ship Type:"
        '
        'cboJFC
        '
        Me.cboJFC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboJFC.FormattingEnabled = True
        Me.cboJFC.Location = New System.Drawing.Point(138, 112)
        Me.cboJFC.Name = "cboJFC"
        Me.cboJFC.Size = New System.Drawing.Size(121, 21)
        Me.cboJFC.TabIndex = 58
        '
        'cboJDC
        '
        Me.cboJDC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboJDC.FormattingEnabled = True
        Me.cboJDC.Location = New System.Drawing.Point(138, 85)
        Me.cboJDC.Name = "cboJDC"
        Me.cboJDC.Size = New System.Drawing.Size(121, 21)
        Me.cboJDC.TabIndex = 57
        '
        'cboShips
        '
        Me.cboShips.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboShips.FormattingEnabled = True
        Me.cboShips.Location = New System.Drawing.Point(138, 59)
        Me.cboShips.Name = "cboShips"
        Me.cboShips.Size = New System.Drawing.Size(121, 21)
        Me.cboShips.TabIndex = 56
        '
        'lblTotalDistance
        '
        Me.lblTotalDistance.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalDistance.AutoSize = True
        Me.lblTotalDistance.Location = New System.Drawing.Point(6, 589)
        Me.lblTotalDistance.Name = "lblTotalDistance"
        Me.lblTotalDistance.Size = New System.Drawing.Size(79, 13)
        Me.lblTotalDistance.TabIndex = 59
        Me.lblTotalDistance.Text = "Total Distance:"
        '
        'lvwRoute
        '
        Me.lvwRoute.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwRoute.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRoute.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colNo, Me.colSystem, Me.colConstellation, Me.colRegion, Me.colSecurity, Me.colDistance, Me.colFuel, Me.colCargo, Me.colSov})
        Me.lvwRoute.ContextMenuStrip = Me.ctxRoute
        Me.lvwRoute.FullRowSelect = True
        Me.lvwRoute.GridLines = True
        Me.lvwRoute.Location = New System.Drawing.Point(6, 215)
        Me.lvwRoute.Name = "lvwRoute"
        Me.lvwRoute.Size = New System.Drawing.Size(598, 371)
        Me.lvwRoute.TabIndex = 58
        Me.lvwRoute.UseCompatibleStateImageBehavior = False
        Me.lvwRoute.View = System.Windows.Forms.View.Details
        '
        'colNo
        '
        Me.colNo.Text = "Jump"
        Me.colNo.Width = 40
        '
        'colSystem
        '
        Me.colSystem.Text = "System Name"
        Me.colSystem.Width = 150
        '
        'colConstellation
        '
        Me.colConstellation.Text = "Constellation"
        Me.colConstellation.Width = 150
        '
        'colRegion
        '
        Me.colRegion.Text = "Region"
        Me.colRegion.Width = 150
        '
        'colSecurity
        '
        Me.colSecurity.Text = "Sec"
        Me.colSecurity.Width = 40
        '
        'colDistance
        '
        Me.colDistance.Text = "Distance (ly)"
        Me.colDistance.Width = 100
        '
        'colFuel
        '
        Me.colFuel.Text = "Fuel Req"
        '
        'colCargo
        '
        Me.colCargo.Text = "Cargo (m3)"
        Me.colCargo.Width = 75
        '
        'colSov
        '
        Me.colSov.Text = "Sovereignty"
        Me.colSov.Width = 180
        '
        'ctxRoute
        '
        Me.ctxRoute.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopyToClipboard})
        Me.ctxRoute.Name = "ctxRoute"
        Me.ctxRoute.Size = New System.Drawing.Size(175, 26)
        '
        'mnuCopyToClipboard
        '
        Me.mnuCopyToClipboard.Name = "mnuCopyToClipboard"
        Me.mnuCopyToClipboard.Size = New System.Drawing.Size(174, 22)
        Me.mnuCopyToClipboard.Text = "Copy To Clipboard"
        '
        'lblTimeTaken
        '
        Me.lblTimeTaken.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTimeTaken.AutoSize = True
        Me.lblTimeTaken.Location = New System.Drawing.Point(269, 589)
        Me.lblTimeTaken.Name = "lblTimeTaken"
        Me.lblTimeTaken.Size = New System.Drawing.Size(67, 13)
        Me.lblTimeTaken.TabIndex = 57
        Me.lblTimeTaken.Text = "Time Taken:"
        '
        'lblJumps
        '
        Me.lblJumps.AutoSize = True
        Me.lblJumps.Location = New System.Drawing.Point(11, 118)
        Me.lblJumps.Name = "lblJumps"
        Me.lblJumps.Size = New System.Drawing.Size(99, 13)
        Me.lblJumps.TabIndex = 56
        Me.lblJumps.Text = "Gate/Jump Radius:"
        '
        'lblRouteMode
        '
        Me.lblRouteMode.AutoSize = True
        Me.lblRouteMode.Location = New System.Drawing.Point(12, 25)
        Me.lblRouteMode.Name = "lblRouteMode"
        Me.lblRouteMode.Size = New System.Drawing.Size(37, 13)
        Me.lblRouteMode.TabIndex = 53
        Me.lblRouteMode.Text = "Mode:"
        '
        'btnCalculate
        '
        Me.btnCalculate.Location = New System.Drawing.Point(6, 183)
        Me.btnCalculate.Name = "btnCalculate"
        Me.btnCalculate.Size = New System.Drawing.Size(75, 23)
        Me.btnCalculate.TabIndex = 51
        Me.btnCalculate.Text = "Calculate"
        Me.btnCalculate.UseVisualStyleBackColor = True
        '
        'nudJumps
        '
        Me.nudJumps.Location = New System.Drawing.Point(135, 116)
        Me.nudJumps.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
        Me.nudJumps.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudJumps.Name = "nudJumps"
        Me.nudJumps.Size = New System.Drawing.Size(84, 20)
        Me.nudJumps.TabIndex = 50
        Me.nudJumps.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cboRouteMode
        '
        Me.cboRouteMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRouteMode.FormattingEnabled = True
        Me.cboRouteMode.Items.AddRange(New Object() {"Gate Route", "Jump Route", "Systems in Gate Radius", "Systems in Jump Radius"})
        Me.cboRouteMode.Location = New System.Drawing.Point(55, 22)
        Me.cboRouteMode.Name = "cboRouteMode"
        Me.cboRouteMode.Size = New System.Drawing.Size(164, 21)
        Me.cboRouteMode.TabIndex = 46
        '
        'nudMaxSec
        '
        Me.nudMaxSec.DecimalPlaces = 1
        Me.nudMaxSec.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudMaxSec.Location = New System.Drawing.Point(69, 85)
        Me.nudMaxSec.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMaxSec.Name = "nudMaxSec"
        Me.nudMaxSec.Size = New System.Drawing.Size(46, 20)
        Me.nudMaxSec.TabIndex = 45
        Me.nudMaxSec.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'nudMinSec
        '
        Me.nudMinSec.DecimalPlaces = 1
        Me.nudMinSec.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudMinSec.Location = New System.Drawing.Point(69, 57)
        Me.nudMinSec.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMinSec.Name = "nudMinSec"
        Me.nudMinSec.Size = New System.Drawing.Size(46, 20)
        Me.nudMinSec.TabIndex = 44
        '
        'lblMaxSec
        '
        Me.lblMaxSec.AutoSize = True
        Me.lblMaxSec.Location = New System.Drawing.Point(14, 87)
        Me.lblMaxSec.Name = "lblMaxSec"
        Me.lblMaxSec.Size = New System.Drawing.Size(49, 13)
        Me.lblMaxSec.TabIndex = 43
        Me.lblMaxSec.Text = "Max Sec"
        '
        'lblMinSec
        '
        Me.lblMinSec.AutoSize = True
        Me.lblMinSec.Location = New System.Drawing.Point(14, 59)
        Me.lblMinSec.Name = "lblMinSec"
        Me.lblMinSec.Size = New System.Drawing.Size(46, 13)
        Me.lblMinSec.TabIndex = 42
        Me.lblMinSec.Text = "Min Sec"
        '
        'tabMapTool
        '
        Me.tabMapTool.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMapTool.Controls.Add(Me.tabRoute)
        Me.tabMapTool.Controls.Add(Me.tabMap)
        Me.tabMapTool.Controls.Add(Me.tabCelestial)
        Me.tabMapTool.Controls.Add(Me.tabStations)
        Me.tabMapTool.Controls.Add(Me.tabStationSearch)
        Me.tabMapTool.Controls.Add(Me.tabAgentSearch)
        Me.tabMapTool.Location = New System.Drawing.Point(261, 10)
        Me.tabMapTool.Name = "tabMapTool"
        Me.tabMapTool.SelectedIndex = 0
        Me.tabMapTool.Size = New System.Drawing.Size(665, 652)
        Me.tabMapTool.TabIndex = 37
        '
        'tabCelestial
        '
        Me.tabCelestial.Controls.Add(Me.radCelRegion)
        Me.tabCelestial.Controls.Add(Me.radCelConst)
        Me.tabCelestial.Controls.Add(Me.radCelSystem)
        Me.tabCelestial.Controls.Add(Me.SearchCelestial)
        Me.tabCelestial.Controls.Add(Me.lblCelBelts)
        Me.tabCelestial.Controls.Add(Me.lblCelMoons)
        Me.tabCelestial.Controls.Add(Me.lblCelPlanets)
        Me.tabCelestial.Controls.Add(Me.lvBelts)
        Me.tabCelestial.Controls.Add(Me.lvMoons)
        Me.tabCelestial.Controls.Add(Me.lvPlanets)
        Me.tabCelestial.Location = New System.Drawing.Point(4, 22)
        Me.tabCelestial.Name = "tabCelestial"
        Me.tabCelestial.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCelestial.Size = New System.Drawing.Size(657, 626)
        Me.tabCelestial.TabIndex = 2
        Me.tabCelestial.Text = "Celestial"
        Me.tabCelestial.UseVisualStyleBackColor = True
        '
        'radCelRegion
        '
        Me.radCelRegion.AutoSize = True
        Me.radCelRegion.Location = New System.Drawing.Point(335, 13)
        Me.radCelRegion.Name = "radCelRegion"
        Me.radCelRegion.Size = New System.Drawing.Size(96, 17)
        Me.radCelRegion.TabIndex = 32
        Me.radCelRegion.Text = "Search Region"
        Me.radCelRegion.UseVisualStyleBackColor = True
        '
        'radCelConst
        '
        Me.radCelConst.AutoSize = True
        Me.radCelConst.Location = New System.Drawing.Point(160, 13)
        Me.radCelConst.Name = "radCelConst"
        Me.radCelConst.Size = New System.Drawing.Size(122, 17)
        Me.radCelConst.TabIndex = 31
        Me.radCelConst.Text = "Search Constellation"
        Me.radCelConst.UseVisualStyleBackColor = True
        '
        'radCelSystem
        '
        Me.radCelSystem.AutoSize = True
        Me.radCelSystem.Checked = True
        Me.radCelSystem.Location = New System.Drawing.Point(17, 13)
        Me.radCelSystem.Name = "radCelSystem"
        Me.radCelSystem.Size = New System.Drawing.Size(96, 17)
        Me.radCelSystem.TabIndex = 30
        Me.radCelSystem.TabStop = True
        Me.radCelSystem.Text = "Search System"
        Me.radCelSystem.UseVisualStyleBackColor = True
        '
        'SearchCelestial
        '
        Me.SearchCelestial.Location = New System.Drawing.Point(455, 10)
        Me.SearchCelestial.Name = "SearchCelestial"
        Me.SearchCelestial.Size = New System.Drawing.Size(75, 23)
        Me.SearchCelestial.TabIndex = 29
        Me.SearchCelestial.Text = "Search"
        Me.SearchCelestial.UseVisualStyleBackColor = True
        '
        'lblCelBelts
        '
        Me.lblCelBelts.AutoSize = True
        Me.lblCelBelts.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCelBelts.Location = New System.Drawing.Point(14, 369)
        Me.lblCelBelts.Name = "lblCelBelts"
        Me.lblCelBelts.Size = New System.Drawing.Size(38, 16)
        Me.lblCelBelts.TabIndex = 28
        Me.lblCelBelts.Text = "Belts"
        Me.lblCelBelts.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'lblCelMoons
        '
        Me.lblCelMoons.AutoSize = True
        Me.lblCelMoons.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCelMoons.Location = New System.Drawing.Point(13, 204)
        Me.lblCelMoons.Name = "lblCelMoons"
        Me.lblCelMoons.Size = New System.Drawing.Size(49, 16)
        Me.lblCelMoons.TabIndex = 27
        Me.lblCelMoons.Text = "Moons"
        Me.lblCelMoons.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'lblCelPlanets
        '
        Me.lblCelPlanets.AutoSize = True
        Me.lblCelPlanets.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCelPlanets.Location = New System.Drawing.Point(14, 56)
        Me.lblCelPlanets.Name = "lblCelPlanets"
        Me.lblCelPlanets.Size = New System.Drawing.Size(53, 16)
        Me.lblCelPlanets.TabIndex = 26
        Me.lblCelPlanets.Text = "Planets"
        Me.lblCelPlanets.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'lvBelts
        '
        Me.lvBelts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader4})
        Me.lvBelts.GridLines = True
        Me.lvBelts.Location = New System.Drawing.Point(17, 388)
        Me.lvBelts.Name = "lvBelts"
        Me.lvBelts.Size = New System.Drawing.Size(514, 137)
        Me.lvBelts.TabIndex = 25
        Me.lvBelts.UseCompatibleStateImageBehavior = False
        Me.lvBelts.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Name"
        Me.ColumnHeader4.Width = 297
        '
        'lvMoons
        '
        Me.lvMoons.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CoplMoonsName})
        Me.lvMoons.GridLines = True
        Me.lvMoons.Location = New System.Drawing.Point(16, 223)
        Me.lvMoons.Name = "lvMoons"
        Me.lvMoons.Size = New System.Drawing.Size(514, 143)
        Me.lvMoons.TabIndex = 24
        Me.lvMoons.UseCompatibleStateImageBehavior = False
        Me.lvMoons.View = System.Windows.Forms.View.Details
        '
        'CoplMoonsName
        '
        Me.CoplMoonsName.Text = "Name"
        Me.CoplMoonsName.Width = 297
        '
        'lvPlanets
        '
        Me.lvPlanets.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CoPlName})
        Me.lvPlanets.GridLines = True
        Me.lvPlanets.Location = New System.Drawing.Point(16, 75)
        Me.lvPlanets.Name = "lvPlanets"
        Me.lvPlanets.Size = New System.Drawing.Size(514, 126)
        Me.lvPlanets.TabIndex = 23
        Me.lvPlanets.UseCompatibleStateImageBehavior = False
        Me.lvPlanets.View = System.Windows.Forms.View.Details
        '
        'CoPlName
        '
        Me.CoPlName.Text = "Name"
        Me.CoPlName.Width = 297
        '
        'tabStations
        '
        Me.tabStations.Controls.Add(Me.Label1)
        Me.tabStations.Controls.Add(Me.cboStation)
        Me.tabStations.Controls.Add(Me.lvagnts)
        Me.tabStations.Controls.Add(Me.lblStationAgents)
        Me.tabStations.Controls.Add(Me.lblStationServicesLbl)
        Me.tabStations.Controls.Add(Me.lblStationServices)
        Me.tabStations.Controls.Add(Me.lblStationFaction)
        Me.tabStations.Controls.Add(Me.lblStationCorpLbl)
        Me.tabStations.Controls.Add(Me.lblStationFactionLbl)
        Me.tabStations.Controls.Add(Me.lblStationCorp)
        Me.tabStations.Location = New System.Drawing.Point(4, 22)
        Me.tabStations.Name = "tabStations"
        Me.tabStations.Size = New System.Drawing.Size(657, 626)
        Me.tabStations.TabIndex = 3
        Me.tabStations.Text = "Stations"
        Me.tabStations.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 92
        Me.Label1.Text = "Station:"
        '
        'cboStation
        '
        Me.cboStation.FormattingEnabled = True
        Me.cboStation.Location = New System.Drawing.Point(64, 11)
        Me.cboStation.Name = "cboStation"
        Me.cboStation.Size = New System.Drawing.Size(404, 21)
        Me.cboStation.TabIndex = 91
        '
        'lvagnts
        '
        Me.lvagnts.AllowColumnReorder = True
        Me.lvagnts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvagnts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader7, Me.ColumnHeader8})
        Me.lvagnts.FullRowSelect = True
        Me.lvagnts.GridLines = True
        Me.lvagnts.Location = New System.Drawing.Point(3, 280)
        Me.lvagnts.MultiSelect = False
        Me.lvagnts.Name = "lvagnts"
        Me.lvagnts.Size = New System.Drawing.Size(651, 342)
        Me.lvagnts.TabIndex = 100
        Me.lvagnts.UseCompatibleStateImageBehavior = False
        Me.lvagnts.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 138
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Corporation"
        Me.ColumnHeader2.Width = 128
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Faction"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Level"
        Me.ColumnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader7.Width = 50
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Quality"
        Me.ColumnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader8.Width = 52
        '
        'lblStationAgents
        '
        Me.lblStationAgents.AutoSize = True
        Me.lblStationAgents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStationAgents.Location = New System.Drawing.Point(3, 264)
        Me.lblStationAgents.Name = "lblStationAgents"
        Me.lblStationAgents.Size = New System.Drawing.Size(43, 13)
        Me.lblStationAgents.TabIndex = 99
        Me.lblStationAgents.Text = "Agents:"
        '
        'lblStationServicesLbl
        '
        Me.lblStationServicesLbl.AutoSize = True
        Me.lblStationServicesLbl.Location = New System.Drawing.Point(15, 97)
        Me.lblStationServicesLbl.Name = "lblStationServicesLbl"
        Me.lblStationServicesLbl.Size = New System.Drawing.Size(87, 13)
        Me.lblStationServicesLbl.TabIndex = 93
        Me.lblStationServicesLbl.Text = "Station Services:"
        '
        'lblStationServices
        '
        Me.lblStationServices.AutoSize = True
        Me.lblStationServices.Location = New System.Drawing.Point(109, 98)
        Me.lblStationServices.Name = "lblStationServices"
        Me.lblStationServices.Size = New System.Drawing.Size(63, 13)
        Me.lblStationServices.TabIndex = 94
        Me.lblStationServices.Text = "Placeholder"
        '
        'lblStationFaction
        '
        Me.lblStationFaction.AutoSize = True
        Me.lblStationFaction.Location = New System.Drawing.Point(109, 40)
        Me.lblStationFaction.Name = "lblStationFaction"
        Me.lblStationFaction.Size = New System.Drawing.Size(65, 13)
        Me.lblStationFaction.TabIndex = 98
        Me.lblStationFaction.Text = "PlaceHolder"
        '
        'lblStationCorpLbl
        '
        Me.lblStationCorpLbl.AutoSize = True
        Me.lblStationCorpLbl.Location = New System.Drawing.Point(15, 55)
        Me.lblStationCorpLbl.Name = "lblStationCorpLbl"
        Me.lblStationCorpLbl.Size = New System.Drawing.Size(64, 13)
        Me.lblStationCorpLbl.TabIndex = 95
        Me.lblStationCorpLbl.Text = "Corporation:"
        '
        'lblStationFactionLbl
        '
        Me.lblStationFactionLbl.AutoSize = True
        Me.lblStationFactionLbl.Location = New System.Drawing.Point(15, 40)
        Me.lblStationFactionLbl.Name = "lblStationFactionLbl"
        Me.lblStationFactionLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblStationFactionLbl.TabIndex = 97
        Me.lblStationFactionLbl.Text = "Faction:"
        '
        'lblStationCorp
        '
        Me.lblStationCorp.AutoSize = True
        Me.lblStationCorp.Location = New System.Drawing.Point(109, 55)
        Me.lblStationCorp.Name = "lblStationCorp"
        Me.lblStationCorp.Size = New System.Drawing.Size(63, 13)
        Me.lblStationCorp.TabIndex = 96
        Me.lblStationCorp.Text = "Placeholder"
        '
        'tabStationSearch
        '
        Me.tabStationSearch.Controls.Add(Me.lvsts)
        Me.tabStationSearch.Controls.Add(Me.cbstsins)
        Me.tabStationSearch.Controls.Add(Me.cbstslps)
        Me.tabStationSearch.Controls.Add(Me.cbstsfit)
        Me.tabStationSearch.Controls.Add(Me.cbstslab)
        Me.tabStationSearch.Controls.Add(Me.cbstsfac)
        Me.tabStationSearch.Controls.Add(Me.cbstsclon)
        Me.tabStationSearch.Controls.Add(Me.cbstsref)
        Me.tabStationSearch.Controls.Add(Me.cbstsrep)
        Me.tabStationSearch.Controls.Add(Me.cbstsreg)
        Me.tabStationSearch.Controls.Add(Me.cbstsconst)
        Me.tabStationSearch.Controls.Add(Me.cbstssys)
        Me.tabStationSearch.Location = New System.Drawing.Point(4, 22)
        Me.tabStationSearch.Name = "tabStationSearch"
        Me.tabStationSearch.Size = New System.Drawing.Size(657, 626)
        Me.tabStationSearch.TabIndex = 4
        Me.tabStationSearch.Text = "Station Search"
        Me.tabStationSearch.UseVisualStyleBackColor = True
        '
        'lvsts
        '
        Me.lvsts.AllowColumnReorder = True
        Me.lvsts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvsts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.costssec, Me.costsname, Me.costscorp, Me.costsfact, Me.costsdist, Me.costsreg, Me.costsconst, Me.costssys})
        Me.lvsts.FullRowSelect = True
        Me.lvsts.GridLines = True
        Me.lvsts.Location = New System.Drawing.Point(3, 162)
        Me.lvsts.MultiSelect = False
        Me.lvsts.Name = "lvsts"
        Me.lvsts.Size = New System.Drawing.Size(651, 460)
        Me.lvsts.TabIndex = 124
        Me.lvsts.UseCompatibleStateImageBehavior = False
        Me.lvsts.View = System.Windows.Forms.View.Details
        '
        'costssec
        '
        Me.costssec.Text = "Security"
        '
        'costsname
        '
        Me.costsname.Text = "Name"
        Me.costsname.Width = 138
        '
        'costscorp
        '
        Me.costscorp.Text = "Corporation"
        Me.costscorp.Width = 92
        '
        'costsfact
        '
        Me.costsfact.Text = "Faction"
        '
        'costsdist
        '
        Me.costsdist.Text = "Distance"
        '
        'costsreg
        '
        Me.costsreg.Text = "Region"
        '
        'costsconst
        '
        Me.costsconst.Text = "Constellation"
        '
        'costssys
        '
        Me.costssys.Text = "System"
        '
        'cbstsins
        '
        Me.cbstsins.AutoSize = True
        Me.cbstsins.Location = New System.Drawing.Point(356, 59)
        Me.cbstsins.Name = "cbstsins"
        Me.cbstsins.Size = New System.Drawing.Size(73, 17)
        Me.cbstsins.TabIndex = 123
        Me.cbstsins.Text = "Insurance"
        Me.cbstsins.UseVisualStyleBackColor = True
        '
        'cbstslps
        '
        Me.cbstslps.AutoSize = True
        Me.cbstslps.Location = New System.Drawing.Point(356, 33)
        Me.cbstslps.Name = "cbstslps"
        Me.cbstslps.Size = New System.Drawing.Size(119, 17)
        Me.cbstslps.TabIndex = 122
        Me.cbstslps.Text = "Loyalty Points Store"
        Me.cbstslps.UseVisualStyleBackColor = True
        '
        'cbstsfit
        '
        Me.cbstsfit.AutoSize = True
        Me.cbstsfit.Location = New System.Drawing.Point(128, 59)
        Me.cbstsfit.Name = "cbstsfit"
        Me.cbstsfit.Size = New System.Drawing.Size(54, 17)
        Me.cbstsfit.TabIndex = 121
        Me.cbstsfit.Text = "Fitting"
        Me.cbstsfit.UseVisualStyleBackColor = True
        '
        'cbstslab
        '
        Me.cbstslab.AutoSize = True
        Me.cbstslab.Location = New System.Drawing.Point(238, 59)
        Me.cbstslab.Name = "cbstslab"
        Me.cbstslab.Size = New System.Drawing.Size(76, 17)
        Me.cbstslab.TabIndex = 120
        Me.cbstslab.Text = "Laboratory"
        Me.cbstslab.UseVisualStyleBackColor = True
        '
        'cbstsfac
        '
        Me.cbstsfac.AutoSize = True
        Me.cbstsfac.Location = New System.Drawing.Point(238, 33)
        Me.cbstsfac.Name = "cbstsfac"
        Me.cbstsfac.Size = New System.Drawing.Size(61, 17)
        Me.cbstsfac.TabIndex = 119
        Me.cbstsfac.Text = "Factory"
        Me.cbstsfac.UseVisualStyleBackColor = True
        '
        'cbstsclon
        '
        Me.cbstsclon.AutoSize = True
        Me.cbstsclon.Location = New System.Drawing.Point(128, 33)
        Me.cbstsclon.Name = "cbstsclon"
        Me.cbstsclon.Size = New System.Drawing.Size(61, 17)
        Me.cbstsclon.TabIndex = 118
        Me.cbstsclon.Text = "Cloning"
        Me.cbstsclon.UseVisualStyleBackColor = True
        '
        'cbstsref
        '
        Me.cbstsref.AutoSize = True
        Me.cbstsref.Location = New System.Drawing.Point(12, 29)
        Me.cbstsref.Name = "cbstsref"
        Me.cbstsref.Size = New System.Drawing.Size(65, 17)
        Me.cbstsref.TabIndex = 117
        Me.cbstsref.Text = "Refining"
        Me.cbstsref.UseVisualStyleBackColor = True
        '
        'cbstsrep
        '
        Me.cbstsrep.AutoSize = True
        Me.cbstsrep.Location = New System.Drawing.Point(12, 59)
        Me.cbstsrep.Name = "cbstsrep"
        Me.cbstsrep.Size = New System.Drawing.Size(91, 17)
        Me.cbstsrep.TabIndex = 116
        Me.cbstsrep.Text = "Reprocessing"
        Me.cbstsrep.UseVisualStyleBackColor = True
        '
        'cbstsreg
        '
        Me.cbstsreg.AutoSize = True
        Me.cbstsreg.Location = New System.Drawing.Point(356, 99)
        Me.cbstsreg.Name = "cbstsreg"
        Me.cbstsreg.Size = New System.Drawing.Size(100, 17)
        Me.cbstsreg.TabIndex = 115
        Me.cbstsreg.Text = "Limit To Region"
        Me.cbstsreg.UseVisualStyleBackColor = True
        '
        'cbstsconst
        '
        Me.cbstsconst.AutoSize = True
        Me.cbstsconst.Location = New System.Drawing.Point(173, 99)
        Me.cbstsconst.Name = "cbstsconst"
        Me.cbstsconst.Size = New System.Drawing.Size(126, 17)
        Me.cbstsconst.TabIndex = 114
        Me.cbstsconst.Text = "Limit To Constellation"
        Me.cbstsconst.UseVisualStyleBackColor = True
        '
        'cbstssys
        '
        Me.cbstssys.AutoSize = True
        Me.cbstssys.Location = New System.Drawing.Point(11, 99)
        Me.cbstssys.Name = "cbstssys"
        Me.cbstssys.Size = New System.Drawing.Size(100, 17)
        Me.cbstssys.TabIndex = 113
        Me.cbstssys.Text = "Limit To System"
        Me.cbstssys.UseVisualStyleBackColor = True
        '
        'tabAgentSearch
        '
        Me.tabAgentSearch.Controls.Add(Me.btnSearchAgents)
        Me.tabAgentSearch.Controls.Add(Me.chkLevel3Agent)
        Me.tabAgentSearch.Controls.Add(Me.chkLevel4Agent)
        Me.tabAgentSearch.Controls.Add(Me.chkLevel1Agent)
        Me.tabAgentSearch.Controls.Add(Me.chkLevel5Agent)
        Me.tabAgentSearch.Controls.Add(Me.chkLevel2Agent)
        Me.tabAgentSearch.Controls.Add(Me.lblagsdiv)
        Me.tabAgentSearch.Controls.Add(Me.lblagscorp)
        Me.tabAgentSearch.Controls.Add(Me.lblagsfact)
        Me.tabAgentSearch.Controls.Add(Me.cboAgentDivision)
        Me.tabAgentSearch.Controls.Add(Me.cboAgentCorp)
        Me.tabAgentSearch.Controls.Add(Me.cboAgentFaction)
        Me.tabAgentSearch.Controls.Add(Me.chkAgentNullSec)
        Me.tabAgentSearch.Controls.Add(Me.chkAgentHighQ)
        Me.tabAgentSearch.Controls.Add(Me.chkAgentRegion)
        Me.tabAgentSearch.Controls.Add(Me.chkAgentConst)
        Me.tabAgentSearch.Controls.Add(Me.chkAgentSystem)
        Me.tabAgentSearch.Controls.Add(Me.chkAgentEmpire)
        Me.tabAgentSearch.Controls.Add(Me.chkAgentLowQ)
        Me.tabAgentSearch.Controls.Add(Me.chkAgentLowSec)
        Me.tabAgentSearch.Controls.Add(Me.lvwAgents)
        Me.tabAgentSearch.Location = New System.Drawing.Point(4, 22)
        Me.tabAgentSearch.Name = "tabAgentSearch"
        Me.tabAgentSearch.Size = New System.Drawing.Size(657, 626)
        Me.tabAgentSearch.TabIndex = 5
        Me.tabAgentSearch.Text = "Agent Search"
        Me.tabAgentSearch.UseVisualStyleBackColor = True
        '
        'btnSearchAgents
        '
        Me.btnSearchAgents.Location = New System.Drawing.Point(482, 35)
        Me.btnSearchAgents.Name = "btnSearchAgents"
        Me.btnSearchAgents.Size = New System.Drawing.Size(59, 23)
        Me.btnSearchAgents.TabIndex = 145
        Me.btnSearchAgents.Text = "Search"
        Me.btnSearchAgents.UseVisualStyleBackColor = True
        '
        'chkLevel3Agent
        '
        Me.chkLevel3Agent.AutoSize = True
        Me.chkLevel3Agent.Location = New System.Drawing.Point(247, 108)
        Me.chkLevel3Agent.Name = "chkLevel3Agent"
        Me.chkLevel3Agent.Size = New System.Drawing.Size(61, 17)
        Me.chkLevel3Agent.TabIndex = 144
        Me.chkLevel3Agent.Text = "Level 3"
        Me.chkLevel3Agent.UseVisualStyleBackColor = True
        '
        'chkLevel4Agent
        '
        Me.chkLevel4Agent.AutoSize = True
        Me.chkLevel4Agent.Location = New System.Drawing.Point(345, 108)
        Me.chkLevel4Agent.Name = "chkLevel4Agent"
        Me.chkLevel4Agent.Size = New System.Drawing.Size(61, 17)
        Me.chkLevel4Agent.TabIndex = 143
        Me.chkLevel4Agent.Text = "Level 4"
        Me.chkLevel4Agent.UseVisualStyleBackColor = True
        '
        'chkLevel1Agent
        '
        Me.chkLevel1Agent.AutoSize = True
        Me.chkLevel1Agent.Location = New System.Drawing.Point(53, 108)
        Me.chkLevel1Agent.Name = "chkLevel1Agent"
        Me.chkLevel1Agent.Size = New System.Drawing.Size(61, 17)
        Me.chkLevel1Agent.TabIndex = 142
        Me.chkLevel1Agent.Text = "Level 1"
        Me.chkLevel1Agent.UseVisualStyleBackColor = True
        '
        'chkLevel5Agent
        '
        Me.chkLevel5Agent.AutoSize = True
        Me.chkLevel5Agent.Location = New System.Drawing.Point(441, 108)
        Me.chkLevel5Agent.Name = "chkLevel5Agent"
        Me.chkLevel5Agent.Size = New System.Drawing.Size(61, 17)
        Me.chkLevel5Agent.TabIndex = 141
        Me.chkLevel5Agent.Text = "Level 5"
        Me.chkLevel5Agent.UseVisualStyleBackColor = True
        '
        'chkLevel2Agent
        '
        Me.chkLevel2Agent.AutoSize = True
        Me.chkLevel2Agent.Location = New System.Drawing.Point(150, 107)
        Me.chkLevel2Agent.Name = "chkLevel2Agent"
        Me.chkLevel2Agent.Size = New System.Drawing.Size(61, 17)
        Me.chkLevel2Agent.TabIndex = 140
        Me.chkLevel2Agent.Text = "Level 2"
        Me.chkLevel2Agent.UseVisualStyleBackColor = True
        '
        'lblagsdiv
        '
        Me.lblagsdiv.AutoSize = True
        Me.lblagsdiv.Location = New System.Drawing.Point(53, 67)
        Me.lblagsdiv.Name = "lblagsdiv"
        Me.lblagsdiv.Size = New System.Drawing.Size(47, 13)
        Me.lblagsdiv.TabIndex = 139
        Me.lblagsdiv.Text = "Division:"
        '
        'lblagscorp
        '
        Me.lblagscorp.AutoSize = True
        Me.lblagscorp.Location = New System.Drawing.Point(36, 40)
        Me.lblagscorp.Name = "lblagscorp"
        Me.lblagscorp.Size = New System.Drawing.Size(64, 13)
        Me.lblagscorp.TabIndex = 138
        Me.lblagscorp.Text = "Corporation:"
        '
        'lblagsfact
        '
        Me.lblagsfact.AutoSize = True
        Me.lblagsfact.Location = New System.Drawing.Point(55, 15)
        Me.lblagsfact.Name = "lblagsfact"
        Me.lblagsfact.Size = New System.Drawing.Size(45, 13)
        Me.lblagsfact.TabIndex = 137
        Me.lblagsfact.Text = "Faction:"
        '
        'cboAgentDivision
        '
        Me.cboAgentDivision.FormattingEnabled = True
        Me.cboAgentDivision.Location = New System.Drawing.Point(106, 64)
        Me.cboAgentDivision.Name = "cboAgentDivision"
        Me.cboAgentDivision.Size = New System.Drawing.Size(365, 21)
        Me.cboAgentDivision.Sorted = True
        Me.cboAgentDivision.TabIndex = 136
        '
        'cboAgentCorp
        '
        Me.cboAgentCorp.FormattingEnabled = True
        Me.cboAgentCorp.Location = New System.Drawing.Point(106, 37)
        Me.cboAgentCorp.Name = "cboAgentCorp"
        Me.cboAgentCorp.Size = New System.Drawing.Size(365, 21)
        Me.cboAgentCorp.Sorted = True
        Me.cboAgentCorp.TabIndex = 135
        '
        'cboAgentFaction
        '
        Me.cboAgentFaction.FormattingEnabled = True
        Me.cboAgentFaction.Location = New System.Drawing.Point(106, 11)
        Me.cboAgentFaction.Name = "cboAgentFaction"
        Me.cboAgentFaction.Size = New System.Drawing.Size(365, 21)
        Me.cboAgentFaction.Sorted = True
        Me.cboAgentFaction.TabIndex = 134
        '
        'chkAgentNullSec
        '
        Me.chkAgentNullSec.AutoSize = True
        Me.chkAgentNullSec.Location = New System.Drawing.Point(247, 137)
        Me.chkAgentNullSec.Name = "chkAgentNullSec"
        Me.chkAgentNullSec.Size = New System.Drawing.Size(41, 17)
        Me.chkAgentNullSec.TabIndex = 133
        Me.chkAgentNullSec.Text = "0.0"
        Me.chkAgentNullSec.UseVisualStyleBackColor = True
        '
        'chkAgentHighQ
        '
        Me.chkAgentHighQ.AutoSize = True
        Me.chkAgentHighQ.Location = New System.Drawing.Point(345, 137)
        Me.chkAgentHighQ.Name = "chkAgentHighQ"
        Me.chkAgentHighQ.Size = New System.Drawing.Size(73, 17)
        Me.chkAgentHighQ.TabIndex = 132
        Me.chkAgentHighQ.Text = "Quality >0"
        Me.chkAgentHighQ.UseVisualStyleBackColor = True
        '
        'chkAgentRegion
        '
        Me.chkAgentRegion.AutoSize = True
        Me.chkAgentRegion.Location = New System.Drawing.Point(441, 171)
        Me.chkAgentRegion.Name = "chkAgentRegion"
        Me.chkAgentRegion.Size = New System.Drawing.Size(100, 17)
        Me.chkAgentRegion.TabIndex = 131
        Me.chkAgentRegion.Text = "Limit To Region"
        Me.chkAgentRegion.UseVisualStyleBackColor = True
        '
        'chkAgentConst
        '
        Me.chkAgentConst.AutoSize = True
        Me.chkAgentConst.Location = New System.Drawing.Point(247, 171)
        Me.chkAgentConst.Name = "chkAgentConst"
        Me.chkAgentConst.Size = New System.Drawing.Size(126, 17)
        Me.chkAgentConst.TabIndex = 130
        Me.chkAgentConst.Text = "Limit To Constellation"
        Me.chkAgentConst.UseVisualStyleBackColor = True
        '
        'chkAgentSystem
        '
        Me.chkAgentSystem.AutoSize = True
        Me.chkAgentSystem.Location = New System.Drawing.Point(53, 174)
        Me.chkAgentSystem.Name = "chkAgentSystem"
        Me.chkAgentSystem.Size = New System.Drawing.Size(100, 17)
        Me.chkAgentSystem.TabIndex = 129
        Me.chkAgentSystem.Text = "Limit To System"
        Me.chkAgentSystem.UseVisualStyleBackColor = True
        '
        'chkAgentEmpire
        '
        Me.chkAgentEmpire.AutoSize = True
        Me.chkAgentEmpire.Location = New System.Drawing.Point(53, 137)
        Me.chkAgentEmpire.Name = "chkAgentEmpire"
        Me.chkAgentEmpire.Size = New System.Drawing.Size(58, 17)
        Me.chkAgentEmpire.TabIndex = 128
        Me.chkAgentEmpire.Text = "Empire"
        Me.chkAgentEmpire.UseVisualStyleBackColor = True
        '
        'chkAgentLowQ
        '
        Me.chkAgentLowQ.AutoSize = True
        Me.chkAgentLowQ.Location = New System.Drawing.Point(441, 137)
        Me.chkAgentLowQ.Name = "chkAgentLowQ"
        Me.chkAgentLowQ.Size = New System.Drawing.Size(73, 17)
        Me.chkAgentLowQ.TabIndex = 127
        Me.chkAgentLowQ.Text = "Quality <0"
        Me.chkAgentLowQ.UseVisualStyleBackColor = True
        '
        'chkAgentLowSec
        '
        Me.chkAgentLowSec.AutoSize = True
        Me.chkAgentLowSec.Location = New System.Drawing.Point(150, 136)
        Me.chkAgentLowSec.Name = "chkAgentLowSec"
        Me.chkAgentLowSec.Size = New System.Drawing.Size(65, 17)
        Me.chkAgentLowSec.TabIndex = 126
        Me.chkAgentLowSec.Text = "LowSec"
        Me.chkAgentLowSec.UseVisualStyleBackColor = True
        '
        'lvwAgents
        '
        Me.lvwAgents.AllowColumnReorder = True
        Me.lvwAgents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwAgents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.coagsname, Me.lgagscorp, Me.lgagsfact, Me.lgagslev, Me.lgagsqual, Me.lgagsdist, Me.lgagssec, Me.lgagsreg, Me.lgagsconst, Me.lgagssys, Me.lgagsstat, Me.lgagstype})
        Me.lvwAgents.FullRowSelect = True
        Me.lvwAgents.GridLines = True
        Me.lvwAgents.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lvwAgents.Location = New System.Drawing.Point(3, 197)
        Me.lvwAgents.MultiSelect = False
        Me.lvwAgents.Name = "lvwAgents"
        Me.lvwAgents.Size = New System.Drawing.Size(651, 425)
        Me.lvwAgents.TabIndex = 125
        Me.lvwAgents.UseCompatibleStateImageBehavior = False
        Me.lvwAgents.View = System.Windows.Forms.View.Details
        '
        'coagsname
        '
        Me.coagsname.Text = "Name"
        Me.coagsname.Width = 138
        '
        'lgagscorp
        '
        Me.lgagscorp.Text = "Corporation"
        Me.lgagscorp.Width = 92
        '
        'lgagsfact
        '
        Me.lgagsfact.Text = "Faction"
        '
        'lgagslev
        '
        Me.lgagslev.Text = "Level"
        Me.lgagslev.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.lgagslev.Width = 50
        '
        'lgagsqual
        '
        Me.lgagsqual.Text = "Quality"
        Me.lgagsqual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.lgagsqual.Width = 52
        '
        'lgagsdist
        '
        Me.lgagsdist.Text = "Distance"
        Me.lgagsdist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lgagssec
        '
        Me.lgagssec.Text = "Security"
        Me.lgagssec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lgagsreg
        '
        Me.lgagsreg.Text = "Region"
        '
        'lgagsconst
        '
        Me.lgagsconst.Text = "Constellation"
        '
        'lgagssys
        '
        Me.lgagssys.Text = "System"
        '
        'lgagsstat
        '
        Me.lgagsstat.Text = "Station"
        '
        'lgagstype
        '
        Me.lgagstype.Text = "Agent Type"
        '
        'pbInfo
        '
        Me.pbInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pbInfo.Image = CType(resources.GetObject("pbInfo.Image"), System.Drawing.Image)
        Me.pbInfo.Location = New System.Drawing.Point(8, 630)
        Me.pbInfo.Name = "pbInfo"
        Me.pbInfo.Size = New System.Drawing.Size(32, 32)
        Me.pbInfo.TabIndex = 66
        Me.pbInfo.TabStop = False
        '
        'lvwExclusions
        '
        Me.lvwExclusions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwExclusions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colExcludedName, Me.colExcludedType})
        Me.lvwExclusions.FullRowSelect = True
        Me.lvwExclusions.GridLines = True
        Me.lvwExclusions.Location = New System.Drawing.Point(3, 6)
        Me.lvwExclusions.Name = "lvwExclusions"
        Me.lvwExclusions.Size = New System.Drawing.Size(231, 372)
        Me.lvwExclusions.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwExclusions.TabIndex = 68
        Me.lvwExclusions.UseCompatibleStateImageBehavior = False
        Me.lvwExclusions.View = System.Windows.Forms.View.Details
        '
        'colExcludedName
        '
        Me.colExcludedName.Text = "Excluded Name"
        Me.colExcludedName.Width = 150
        '
        'colExcludedType
        '
        Me.colExcludedType.Text = "Type"
        Me.colExcludedType.Width = 50
        '
        'ctxExclude
        '
        Me.ctxExclude.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExcludeSystem, Me.mnuExcludeConstellation, Me.mnuExcludeRegion})
        Me.ctxExclude.Name = "ctxExclude"
        Me.ctxExclude.Size = New System.Drawing.Size(145, 70)
        '
        'mnuExcludeSystem
        '
        Me.mnuExcludeSystem.Name = "mnuExcludeSystem"
        Me.mnuExcludeSystem.Size = New System.Drawing.Size(144, 22)
        Me.mnuExcludeSystem.Text = "Solar System"
        '
        'mnuExcludeConstellation
        '
        Me.mnuExcludeConstellation.Name = "mnuExcludeConstellation"
        Me.mnuExcludeConstellation.Size = New System.Drawing.Size(144, 22)
        Me.mnuExcludeConstellation.Text = "Constellation"
        '
        'mnuExcludeRegion
        '
        Me.mnuExcludeRegion.Name = "mnuExcludeRegion"
        Me.mnuExcludeRegion.Size = New System.Drawing.Size(144, 22)
        Me.mnuExcludeRegion.Text = "Region"
        '
        'btnRemoveExclusion
        '
        Me.btnRemoveExclusion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveExclusion.Location = New System.Drawing.Point(127, 384)
        Me.btnRemoveExclusion.Name = "btnRemoveExclusion"
        Me.btnRemoveExclusion.Size = New System.Drawing.Size(106, 23)
        Me.btnRemoveExclusion.TabIndex = 69
        Me.btnRemoveExclusion.Text = "Remove Exclusion"
        Me.btnRemoveExclusion.UseVisualStyleBackColor = True
        '
        'tabWaypointExclusions
        '
        Me.tabWaypointExclusions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tabWaypointExclusions.Controls.Add(Me.tabSystem)
        Me.tabWaypointExclusions.Controls.Add(Me.tabExclusions)
        Me.tabWaypointExclusions.Controls.Add(Me.tabWaypoints)
        Me.tabWaypointExclusions.Location = New System.Drawing.Point(8, 185)
        Me.tabWaypointExclusions.Name = "tabWaypointExclusions"
        Me.tabWaypointExclusions.SelectedIndex = 0
        Me.tabWaypointExclusions.Size = New System.Drawing.Size(247, 439)
        Me.tabWaypointExclusions.TabIndex = 68
        '
        'tabSystem
        '
        Me.tabSystem.Controls.Add(Me.gbSystemInfo)
        Me.tabSystem.Location = New System.Drawing.Point(4, 22)
        Me.tabSystem.Name = "tabSystem"
        Me.tabSystem.Size = New System.Drawing.Size(239, 413)
        Me.tabSystem.TabIndex = 2
        Me.tabSystem.Text = "System Info"
        Me.tabSystem.UseVisualStyleBackColor = True
        '
        'tabExclusions
        '
        Me.tabExclusions.Controls.Add(Me.lvwExclusions)
        Me.tabExclusions.Controls.Add(Me.btnRemoveExclusion)
        Me.tabExclusions.Location = New System.Drawing.Point(4, 22)
        Me.tabExclusions.Name = "tabExclusions"
        Me.tabExclusions.Padding = New System.Windows.Forms.Padding(3)
        Me.tabExclusions.Size = New System.Drawing.Size(239, 413)
        Me.tabExclusions.TabIndex = 0
        Me.tabExclusions.Text = "Exclusions"
        Me.tabExclusions.UseVisualStyleBackColor = True
        '
        'tabWaypoints
        '
        Me.tabWaypoints.Controls.Add(Me.btnClearWP)
        Me.tabWaypoints.Controls.Add(Me.chkAutoCalcRoute)
        Me.tabWaypoints.Controls.Add(Me.lstWaypoints)
        Me.tabWaypoints.Controls.Add(Me.btnOptimalWP)
        Me.tabWaypoints.Controls.Add(Me.btnRemoveWaypoint)
        Me.tabWaypoints.Location = New System.Drawing.Point(4, 22)
        Me.tabWaypoints.Name = "tabWaypoints"
        Me.tabWaypoints.Padding = New System.Windows.Forms.Padding(3)
        Me.tabWaypoints.Size = New System.Drawing.Size(239, 413)
        Me.tabWaypoints.TabIndex = 1
        Me.tabWaypoints.Text = "Waypoints"
        Me.tabWaypoints.UseVisualStyleBackColor = True
        '
        'btnClearWP
        '
        Me.btnClearWP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearWP.Location = New System.Drawing.Point(89, 366)
        Me.btnClearWP.Name = "btnClearWP"
        Me.btnClearWP.Size = New System.Drawing.Size(76, 23)
        Me.btnClearWP.TabIndex = 7
        Me.btnClearWP.Text = "Clear WP"
        Me.btnClearWP.UseVisualStyleBackColor = True
        '
        'lblConstMain
        '
        Me.lblConstMain.AutoSize = True
        Me.lblConstMain.Location = New System.Drawing.Point(13, 42)
        Me.lblConstMain.Name = "lblConstMain"
        Me.lblConstMain.Size = New System.Drawing.Size(37, 13)
        Me.lblConstMain.TabIndex = 80
        Me.lblConstMain.Text = "Const:"
        '
        'lblRegionMain
        '
        Me.lblRegionMain.AutoSize = True
        Me.lblRegionMain.Location = New System.Drawing.Point(13, 15)
        Me.lblRegionMain.Name = "lblRegionMain"
        Me.lblRegionMain.Size = New System.Drawing.Size(44, 13)
        Me.lblRegionMain.TabIndex = 79
        Me.lblRegionMain.Text = "Region:"
        '
        'cboConst
        '
        Me.cboConst.FormattingEnabled = True
        Me.cboConst.Location = New System.Drawing.Point(63, 39)
        Me.cboConst.Name = "cboConst"
        Me.cboConst.Size = New System.Drawing.Size(176, 21)
        Me.cboConst.TabIndex = 78
        '
        'cboRegion
        '
        Me.cboRegion.FormattingEnabled = True
        Me.cboRegion.Location = New System.Drawing.Point(63, 12)
        Me.cboRegion.Name = "cboRegion"
        Me.cboRegion.Size = New System.Drawing.Size(176, 21)
        Me.cboRegion.TabIndex = 77
        '
        'btnExclude
        '
        Me.btnExclude.AutoSize = True
        Me.btnExclude.ContextMenuStrip = Me.ctxExclude
        Me.btnExclude.Location = New System.Drawing.Point(121, 99)
        Me.btnExclude.Name = "btnExclude"
        Me.btnExclude.Size = New System.Drawing.Size(64, 40)
        Me.btnExclude.SplitMenu = Me.ctxExclude
        Me.btnExclude.TabIndex = 67
        Me.btnExclude.Text = "Exclude"
        Me.btnExclude.UseCompatibleTextRendering = True
        Me.btnExclude.UseVisualStyleBackColor = True
        '
        'frmMap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(938, 666)
        Me.Controls.Add(Me.lblConstMain)
        Me.Controls.Add(Me.lblRegionMain)
        Me.Controls.Add(Me.cboConst)
        Me.Controls.Add(Me.cboRegion)
        Me.Controls.Add(Me.tabWaypointExclusions)
        Me.Controls.Add(Me.pbInfo)
        Me.Controls.Add(Me.tabMapTool)
        Me.Controls.Add(Me.lblEndSystem)
        Me.Controls.Add(Me.btnAddStart)
        Me.Controls.Add(Me.cboSystem)
        Me.Controls.Add(Me.lblStartSystem)
        Me.Controls.Add(Me.lblSystemMain)
        Me.Controls.Add(Me.btnAddEnd)
        Me.Controls.Add(Me.btnAddWaypoint)
        Me.Controls.Add(Me.btnExclude)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMap"
        Me.Text = "EveHQ Map Tool"
        Me.gbSystemInfo.ResumeLayout(False)
        Me.gbSystemInfo.PerformLayout()
        Me.tabMap.ResumeLayout(False)
        Me.tabMap.PerformLayout()
        CType(Me.pbMap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAccuracy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabRoute.ResumeLayout(False)
        Me.tabRoute.PerformLayout()
        Me.gbJumpDrive.ResumeLayout(False)
        Me.gbJumpDrive.PerformLayout()
        Me.ctxRoute.ResumeLayout(False)
        CType(Me.nudJumps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMaxSec, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMinSec, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMapTool.ResumeLayout(False)
        Me.tabCelestial.ResumeLayout(False)
        Me.tabCelestial.PerformLayout()
        Me.tabStations.ResumeLayout(False)
        Me.tabStations.PerformLayout()
        Me.tabStationSearch.ResumeLayout(False)
        Me.tabStationSearch.PerformLayout()
        Me.tabAgentSearch.ResumeLayout(False)
        Me.tabAgentSearch.PerformLayout()
        CType(Me.pbInfo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxExclude.ResumeLayout(False)
        Me.tabWaypointExclusions.ResumeLayout(False)
        Me.tabSystem.ResumeLayout(False)
        Me.tabExclusions.ResumeLayout(False)
        Me.tabWaypoints.ResumeLayout(False)
        Me.tabWaypoints.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSystemMain As System.Windows.Forms.Label
    Friend WithEvents cboSystem As System.Windows.Forms.ComboBox
    Friend WithEvents gbSystemInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblRegion As System.Windows.Forms.Label
    Friend WithEvents lblRegionlbl As System.Windows.Forms.Label
    Friend WithEvents lblConst As System.Windows.Forms.Label
    Friend WithEvents lblConstlbl As System.Windows.Forms.Label
    Friend WithEvents lblGates As System.Windows.Forms.Label
    Friend WithEvents lblNoGates As System.Windows.Forms.Label
    Friend WithEvents lblNoGateslbl As System.Windows.Forms.Label
    Friend WithEvents lblEveSec As System.Windows.Forms.Label
    Friend WithEvents lblEveSeclbl As System.Windows.Forms.Label
    Friend WithEvents lblID As System.Windows.Forms.Label
    Friend WithEvents lblIDlbl As System.Windows.Forms.Label
    Friend WithEvents lblSecurity As System.Windows.Forms.Label
    Friend WithEvents lblSecuritylbl As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblNamelbl As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnAddEnd As System.Windows.Forms.Button
    Friend WithEvents btnAddStart As System.Windows.Forms.Button
    Friend WithEvents lblEndSystem As System.Windows.Forms.Label
    Friend WithEvents lblStartSystem As System.Windows.Forms.Label
    Friend WithEvents btnAddWaypoint As System.Windows.Forms.Button
    Friend WithEvents chkAutoCalcRoute As System.Windows.Forms.CheckBox
    Friend WithEvents btnOptimalWP As System.Windows.Forms.Button
    Friend WithEvents btnRemoveWaypoint As System.Windows.Forms.Button
    Friend WithEvents lstWaypoints As System.Windows.Forms.ListBox
    Friend WithEvents tabMap As System.Windows.Forms.TabPage
    Friend WithEvents pbMap As System.Windows.Forms.PictureBox
    Friend WithEvents btnShowRoute As System.Windows.Forms.Button
    Friend WithEvents chkRoute As System.Windows.Forms.CheckBox
    Friend WithEvents lblPointerAccuracy As System.Windows.Forms.Label
    Friend WithEvents nudAccuracy As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents tabRoute As System.Windows.Forms.TabPage
    Friend WithEvents gbJumpDrive As System.Windows.Forms.GroupBox
    Friend WithEvents chkOverrideJFC As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverrideJDC As System.Windows.Forms.CheckBox
    Friend WithEvents lblJFC As System.Windows.Forms.Label
    Friend WithEvents lblJDC As System.Windows.Forms.Label
    Friend WithEvents lblShips As System.Windows.Forms.Label
    Friend WithEvents cboJFC As System.Windows.Forms.ComboBox
    Friend WithEvents cboJDC As System.Windows.Forms.ComboBox
    Friend WithEvents cboShips As System.Windows.Forms.ComboBox
    Friend WithEvents lblTotalDistance As System.Windows.Forms.Label
    Friend WithEvents lvwRoute As System.Windows.Forms.ListView
    Friend WithEvents colSystem As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSecurity As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDistance As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblTimeTaken As System.Windows.Forms.Label
    Friend WithEvents lblJumps As System.Windows.Forms.Label
    Friend WithEvents lblRouteMode As System.Windows.Forms.Label
    Friend WithEvents btnCalculate As System.Windows.Forms.Button
    Friend WithEvents nudJumps As System.Windows.Forms.NumericUpDown
    Friend WithEvents cboRouteMode As System.Windows.Forms.ComboBox
    Friend WithEvents nudMaxSec As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudMinSec As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblMaxSec As System.Windows.Forms.Label
    Friend WithEvents lblMinSec As System.Windows.Forms.Label
    Friend WithEvents tabMapTool As System.Windows.Forms.TabControl
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents cboPilot As System.Windows.Forms.ComboBox
    Friend WithEvents colFuel As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNo As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblEuclideanDistance As System.Windows.Forms.Label
    Friend WithEvents lblTotalFuel As System.Windows.Forms.Label
    Friend WithEvents colCargo As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblZoom As System.Windows.Forms.Label
    Friend WithEvents pbInfo As System.Windows.Forms.PictureBox
    Friend WithEvents ctxRoute As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCopyToClipboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents colConstellation As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRegion As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkOverrideJF As System.Windows.Forms.CheckBox
    Friend WithEvents lblJF As System.Windows.Forms.Label
    Friend WithEvents cboJF As System.Windows.Forms.ComboBox
    Friend WithEvents btnExclude As EveHQ.Map.SplitButton
    Friend WithEvents lvwExclusions As System.Windows.Forms.ListView
    Friend WithEvents colExcludedName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colExcludedType As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxExclude As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuExcludeSystem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExcludeConstellation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExcludeRegion As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnRemoveExclusion As System.Windows.Forms.Button
    Friend WithEvents tabWaypointExclusions As System.Windows.Forms.TabControl
    Friend WithEvents tabExclusions As System.Windows.Forms.TabPage
    Friend WithEvents tabWaypoints As System.Windows.Forms.TabPage
    Friend WithEvents btnClearWP As System.Windows.Forms.Button
    Friend WithEvents tabCelestial As System.Windows.Forms.TabPage
    Friend WithEvents radCelRegion As System.Windows.Forms.RadioButton
    Friend WithEvents radCelConst As System.Windows.Forms.RadioButton
    Friend WithEvents radCelSystem As System.Windows.Forms.RadioButton
    Friend WithEvents SearchCelestial As System.Windows.Forms.Button
    Friend WithEvents lblCelBelts As System.Windows.Forms.Label
    Friend WithEvents lblCelMoons As System.Windows.Forms.Label
    Friend WithEvents lblCelPlanets As System.Windows.Forms.Label
    Friend WithEvents lvBelts As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvMoons As System.Windows.Forms.ListView
    Friend WithEvents CoplMoonsName As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvPlanets As System.Windows.Forms.ListView
    Friend WithEvents CoPlName As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabStations As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboStation As System.Windows.Forms.ComboBox
    Friend WithEvents lvagnts As System.Windows.Forms.ListView
    Private WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblStationAgents As System.Windows.Forms.Label
    Friend WithEvents lblStationServicesLbl As System.Windows.Forms.Label
    Friend WithEvents lblStationServices As System.Windows.Forms.Label
    Friend WithEvents lblStationFaction As System.Windows.Forms.Label
    Friend WithEvents lblStationCorpLbl As System.Windows.Forms.Label
    Friend WithEvents lblStationFactionLbl As System.Windows.Forms.Label
    Friend WithEvents lblStationCorp As System.Windows.Forms.Label
    Friend WithEvents tabStationSearch As System.Windows.Forms.TabPage
    Friend WithEvents lvsts As System.Windows.Forms.ListView
    Friend WithEvents costssec As System.Windows.Forms.ColumnHeader
    Private WithEvents costsname As System.Windows.Forms.ColumnHeader
    Friend WithEvents costscorp As System.Windows.Forms.ColumnHeader
    Friend WithEvents costsfact As System.Windows.Forms.ColumnHeader
    Friend WithEvents costsdist As System.Windows.Forms.ColumnHeader
    Friend WithEvents costsreg As System.Windows.Forms.ColumnHeader
    Friend WithEvents costsconst As System.Windows.Forms.ColumnHeader
    Friend WithEvents costssys As System.Windows.Forms.ColumnHeader
    Friend WithEvents cbstsins As System.Windows.Forms.CheckBox
    Friend WithEvents cbstslps As System.Windows.Forms.CheckBox
    Friend WithEvents cbstsfit As System.Windows.Forms.CheckBox
    Friend WithEvents cbstslab As System.Windows.Forms.CheckBox
    Friend WithEvents cbstsfac As System.Windows.Forms.CheckBox
    Friend WithEvents cbstsclon As System.Windows.Forms.CheckBox
    Friend WithEvents cbstsref As System.Windows.Forms.CheckBox
    Friend WithEvents cbstsrep As System.Windows.Forms.CheckBox
    Friend WithEvents cbstsreg As System.Windows.Forms.CheckBox
    Friend WithEvents cbstsconst As System.Windows.Forms.CheckBox
    Friend WithEvents cbstssys As System.Windows.Forms.CheckBox
    Friend WithEvents tabAgentSearch As System.Windows.Forms.TabPage
    Friend WithEvents btnSearchAgents As System.Windows.Forms.Button
    Friend WithEvents chkLevel3Agent As System.Windows.Forms.CheckBox
    Friend WithEvents chkLevel4Agent As System.Windows.Forms.CheckBox
    Friend WithEvents chkLevel1Agent As System.Windows.Forms.CheckBox
    Friend WithEvents chkLevel5Agent As System.Windows.Forms.CheckBox
    Friend WithEvents chkLevel2Agent As System.Windows.Forms.CheckBox
    Friend WithEvents lblagsdiv As System.Windows.Forms.Label
    Friend WithEvents lblagscorp As System.Windows.Forms.Label
    Friend WithEvents lblagsfact As System.Windows.Forms.Label
    Friend WithEvents cboAgentDivision As System.Windows.Forms.ComboBox
    Friend WithEvents cboAgentCorp As System.Windows.Forms.ComboBox
    Friend WithEvents cboAgentFaction As System.Windows.Forms.ComboBox
    Friend WithEvents chkAgentNullSec As System.Windows.Forms.CheckBox
    Friend WithEvents chkAgentHighQ As System.Windows.Forms.CheckBox
    Friend WithEvents chkAgentRegion As System.Windows.Forms.CheckBox
    Friend WithEvents chkAgentConst As System.Windows.Forms.CheckBox
    Friend WithEvents chkAgentSystem As System.Windows.Forms.CheckBox
    Friend WithEvents chkAgentEmpire As System.Windows.Forms.CheckBox
    Friend WithEvents chkAgentLowQ As System.Windows.Forms.CheckBox
    Friend WithEvents chkAgentLowSec As System.Windows.Forms.CheckBox
    Friend WithEvents lvwAgents As System.Windows.Forms.ListView
    Private WithEvents coagsname As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagscorp As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagsfact As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagslev As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagsqual As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagsdist As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagssec As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagsreg As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagsconst As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagssys As System.Windows.Forms.ColumnHeader
    Friend WithEvents lgagsstat As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblConstMain As System.Windows.Forms.Label
    Friend WithEvents lblRegionMain As System.Windows.Forms.Label
    Friend WithEvents cboConst As System.Windows.Forms.ComboBox
    Friend WithEvents cboRegion As System.Windows.Forms.ComboBox
    Friend WithEvents tabSystem As System.Windows.Forms.TabPage
    Friend WithEvents lblSovereigntyLevel As System.Windows.Forms.Label
    Friend WithEvents lblSovereigntyLevellbl As System.Windows.Forms.Label
    Friend WithEvents lblSovHolder As System.Windows.Forms.Label
    Friend WithEvents lblSovHolderLbl As System.Windows.Forms.Label
    Friend WithEvents lblStations As System.Windows.Forms.Label
    Friend WithEvents lblIBelts As System.Windows.Forms.Label
    Friend WithEvents lblABelts As System.Windows.Forms.Label
    Friend WithEvents lblMoons As System.Windows.Forms.Label
    Friend WithEvents lblPlanets As System.Windows.Forms.Label
    Friend WithEvents lblStationsLbl As System.Windows.Forms.Label
    Friend WithEvents lblIBeltsLbl As System.Windows.Forms.Label
    Friend WithEvents lblABeltsLbl As System.Windows.Forms.Label
    Friend WithEvents lblMoonsLbl As System.Windows.Forms.Label
    Friend WithEvents lblPlanetsLbl As System.Windows.Forms.Label
    Friend WithEvents lgagstype As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSov As System.Windows.Forms.ColumnHeader
End Class
