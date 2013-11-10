Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmBcBrowser
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
            Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("High Slots", System.Windows.Forms.HorizontalAlignment.Left)
            Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mid Slots", System.Windows.Forms.HorizontalAlignment.Left)
            Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Low Slots", System.Windows.Forms.HorizontalAlignment.Left)
            Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Rig Slots", System.Windows.Forms.HorizontalAlignment.Left)
            Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Subsystems", System.Windows.Forms.HorizontalAlignment.Left)
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmBcBrowser))
            Me.lblShipType = New System.Windows.Forms.Label()
            Me.pbShip = New System.Windows.Forms.PictureBox()
            Me.ctxLoadout = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuViewLoadout = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuCopyURL = New System.Windows.Forms.ToolStripMenuItem()
            Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
            Me.lblBCStatus = New System.Windows.Forms.ToolStripStatusLabel()
            Me.lblTopicAddress = New System.Windows.Forms.ToolStripStatusLabel()
            Me.lblLoadoutNameLbl = New System.Windows.Forms.Label()
            Me.lblLoadoutAuthorLbl = New System.Windows.Forms.Label()
            Me.lblLoadoutScoreLbl = New System.Windows.Forms.Label()
            Me.lblLoadoutDateLbl = New System.Windows.Forms.Label()
            Me.LblLoadoutTopicLbl = New System.Windows.Forms.Label()
            Me.lblLoadoutName = New System.Windows.Forms.Label()
            Me.lblLoadoutAuthor = New System.Windows.Forms.Label()
            Me.lblLoadoutScore = New System.Windows.Forms.Label()
            Me.lblLoadoutDate = New System.Windows.Forms.Label()
            Me.lblLoadoutTopic = New System.Windows.Forms.LinkLabel()
            Me.lblPG = New System.Windows.Forms.Label()
            Me.lblOptimalRange = New System.Windows.Forms.Label()
            Me.lblOptimalRangeLbl = New System.Windows.Forms.Label()
            Me.lblMaxRange = New System.Windows.Forms.Label()
            Me.lblPGLbl = New System.Windows.Forms.Label()
            Me.LblMaxRangeLbl = New System.Windows.Forms.Label()
            Me.lblCPU = New System.Windows.Forms.Label()
            Me.lblVelocity = New System.Windows.Forms.Label()
            Me.lblVelocityLbl = New System.Windows.Forms.Label()
            Me.lblCPULbl = New System.Windows.Forms.Label()
            Me.lblCapacitor = New System.Windows.Forms.Label()
            Me.lblCapLbl = New System.Windows.Forms.Label()
            Me.lblArmorResists = New System.Windows.Forms.Label()
            Me.lblShieldResists = New System.Windows.Forms.Label()
            Me.lblArmorResistsLbl = New System.Windows.Forms.Label()
            Me.lblShieldResistsLbl = New System.Windows.Forms.Label()
            Me.lblVolley = New System.Windows.Forms.Label()
            Me.lblDPS = New System.Windows.Forms.Label()
            Me.lblDPSLbl = New System.Windows.Forms.Label()
            Me.lblVolleyLbl = New System.Windows.Forms.Label()
            Me.lblTank = New System.Windows.Forms.Label()
            Me.lblEHP = New System.Windows.Forms.Label()
            Me.lblTankLbl = New System.Windows.Forms.Label()
            Me.lblEHPLbl = New System.Windows.Forms.Label()
            Me.cboProfiles = New System.Windows.Forms.ComboBox()
            Me.lblProfileName = New System.Windows.Forms.Label()
            Me.cboPilots = New System.Windows.Forms.ComboBox()
            Me.lblPilot = New System.Windows.Forms.Label()
            Me.lvwSlots = New DevComponents.DotNetBar.Controls.ListViewEx()
            Me.ctxSlots = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuShowInfo = New System.Windows.Forms.ToolStripMenuItem()
            Me.gpStatistics = New DevComponents.DotNetBar.Controls.GroupPanel()
            Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
            Me.adtLoadouts = New DevComponents.AdvTree.AdvTree()
            Me.colLoadout = New DevComponents.AdvTree.ColumnHeader()
            Me.colAuthor = New DevComponents.AdvTree.ColumnHeader()
            Me.colScore = New DevComponents.AdvTree.ColumnHeader()
            Me.colDate = New DevComponents.AdvTree.ColumnHeader()
            Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
            Me.ElementStyle1 = New DevComponents.DotNetBar.ElementStyle()
            Me.btnImport = New DevComponents.DotNetBar.ButtonX()
            CType(Me.pbShip, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.ctxLoadout.SuspendLayout()
            Me.StatusStrip1.SuspendLayout()
            Me.ctxSlots.SuspendLayout()
            Me.gpStatistics.SuspendLayout()
            Me.PanelEx1.SuspendLayout()
            CType(Me.adtLoadouts, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblShipType
            '
            Me.lblShipType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                           Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblShipType.Font = New System.Drawing.Font("Tahoma", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblShipType.Location = New System.Drawing.Point(137, 3)
            Me.lblShipType.Name = "lblShipType"
            Me.lblShipType.Size = New System.Drawing.Size(765, 33)
            Me.lblShipType.TabIndex = 4
            Me.lblShipType.Text = "Ship Type"
            '
            'pbShip
            '
            Me.pbShip.Location = New System.Drawing.Point(3, 3)
            Me.pbShip.Name = "pbShip"
            Me.pbShip.Size = New System.Drawing.Size(128, 128)
            Me.pbShip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.pbShip.TabIndex = 3
            Me.pbShip.TabStop = False
            '
            'ctxLoadout
            '
            Me.ctxLoadout.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewLoadout, Me.mnuCopyURL})
            Me.ctxLoadout.Name = "ctxLoadout"
            Me.ctxLoadout.Size = New System.Drawing.Size(243, 48)
            '
            'mnuViewLoadout
            '
            Me.mnuViewLoadout.Name = "mnuViewLoadout"
            Me.mnuViewLoadout.Size = New System.Drawing.Size(242, 22)
            Me.mnuViewLoadout.Text = "View Loadout"
            '
            'mnuCopyURL
            '
            Me.mnuCopyURL.Name = "mnuCopyURL"
            Me.mnuCopyURL.Size = New System.Drawing.Size(242, 22)
            Me.mnuCopyURL.Text = "Copy Loadout URL to Clipboard"
            '
            'StatusStrip1
            '
            Me.StatusStrip1.BackColor = System.Drawing.Color.Transparent
            Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblBCStatus, Me.lblTopicAddress})
            Me.StatusStrip1.Location = New System.Drawing.Point(0, 613)
            Me.StatusStrip1.Name = "StatusStrip1"
            Me.StatusStrip1.Size = New System.Drawing.Size(899, 22)
            Me.StatusStrip1.TabIndex = 8
            Me.StatusStrip1.Text = "StatusStrip1"
            '
            'lblBCStatus
            '
            Me.lblBCStatus.Name = "lblBCStatus"
            Me.lblBCStatus.Size = New System.Drawing.Size(42, 17)
            Me.lblBCStatus.Text = "Status:"
            '
            'lblTopicAddress
            '
            Me.lblTopicAddress.Name = "lblTopicAddress"
            Me.lblTopicAddress.Size = New System.Drawing.Size(842, 17)
            Me.lblTopicAddress.Spring = True
            Me.lblTopicAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblLoadoutNameLbl
            '
            Me.lblLoadoutNameLbl.AutoSize = True
            Me.lblLoadoutNameLbl.Location = New System.Drawing.Point(140, 48)
            Me.lblLoadoutNameLbl.Name = "lblLoadoutNameLbl"
            Me.lblLoadoutNameLbl.Size = New System.Drawing.Size(80, 13)
            Me.lblLoadoutNameLbl.TabIndex = 9
            Me.lblLoadoutNameLbl.Text = "Loadout Name:"
            Me.lblLoadoutNameLbl.Visible = False
            '
            'lblLoadoutAuthorLbl
            '
            Me.lblLoadoutAuthorLbl.AutoSize = True
            Me.lblLoadoutAuthorLbl.Location = New System.Drawing.Point(140, 61)
            Me.lblLoadoutAuthorLbl.Name = "lblLoadoutAuthorLbl"
            Me.lblLoadoutAuthorLbl.Size = New System.Drawing.Size(44, 13)
            Me.lblLoadoutAuthorLbl.TabIndex = 10
            Me.lblLoadoutAuthorLbl.Text = "Author:"
            Me.lblLoadoutAuthorLbl.Visible = False
            '
            'lblLoadoutScoreLbl
            '
            Me.lblLoadoutScoreLbl.AutoSize = True
            Me.lblLoadoutScoreLbl.Location = New System.Drawing.Point(140, 74)
            Me.lblLoadoutScoreLbl.Name = "lblLoadoutScoreLbl"
            Me.lblLoadoutScoreLbl.Size = New System.Drawing.Size(38, 13)
            Me.lblLoadoutScoreLbl.TabIndex = 11
            Me.lblLoadoutScoreLbl.Text = "Score:"
            Me.lblLoadoutScoreLbl.Visible = False
            '
            'lblLoadoutDateLbl
            '
            Me.lblLoadoutDateLbl.AutoSize = True
            Me.lblLoadoutDateLbl.Location = New System.Drawing.Point(140, 87)
            Me.lblLoadoutDateLbl.Name = "lblLoadoutDateLbl"
            Me.lblLoadoutDateLbl.Size = New System.Drawing.Size(34, 13)
            Me.lblLoadoutDateLbl.TabIndex = 12
            Me.lblLoadoutDateLbl.Text = "Date:"
            Me.lblLoadoutDateLbl.Visible = False
            '
            'LblLoadoutTopicLbl
            '
            Me.LblLoadoutTopicLbl.AutoSize = True
            Me.LblLoadoutTopicLbl.Location = New System.Drawing.Point(140, 100)
            Me.LblLoadoutTopicLbl.Name = "LblLoadoutTopicLbl"
            Me.LblLoadoutTopicLbl.Size = New System.Drawing.Size(71, 13)
            Me.LblLoadoutTopicLbl.TabIndex = 13
            Me.LblLoadoutTopicLbl.Text = "Website Link:"
            Me.LblLoadoutTopicLbl.Visible = False
            '
            'lblLoadoutName
            '
            Me.lblLoadoutName.AutoSize = True
            Me.lblLoadoutName.Location = New System.Drawing.Point(243, 48)
            Me.lblLoadoutName.Name = "lblLoadoutName"
            Me.lblLoadoutName.Size = New System.Drawing.Size(38, 13)
            Me.lblLoadoutName.TabIndex = 14
            Me.lblLoadoutName.Text = "Label1"
            Me.lblLoadoutName.Visible = False
            '
            'lblLoadoutAuthor
            '
            Me.lblLoadoutAuthor.AutoSize = True
            Me.lblLoadoutAuthor.Location = New System.Drawing.Point(243, 61)
            Me.lblLoadoutAuthor.Name = "lblLoadoutAuthor"
            Me.lblLoadoutAuthor.Size = New System.Drawing.Size(38, 13)
            Me.lblLoadoutAuthor.TabIndex = 15
            Me.lblLoadoutAuthor.Text = "Label1"
            Me.lblLoadoutAuthor.Visible = False
            '
            'lblLoadoutScore
            '
            Me.lblLoadoutScore.AutoSize = True
            Me.lblLoadoutScore.Location = New System.Drawing.Point(243, 74)
            Me.lblLoadoutScore.Name = "lblLoadoutScore"
            Me.lblLoadoutScore.Size = New System.Drawing.Size(38, 13)
            Me.lblLoadoutScore.TabIndex = 16
            Me.lblLoadoutScore.Text = "Label1"
            Me.lblLoadoutScore.Visible = False
            '
            'lblLoadoutDate
            '
            Me.lblLoadoutDate.AutoSize = True
            Me.lblLoadoutDate.Location = New System.Drawing.Point(243, 87)
            Me.lblLoadoutDate.Name = "lblLoadoutDate"
            Me.lblLoadoutDate.Size = New System.Drawing.Size(38, 13)
            Me.lblLoadoutDate.TabIndex = 17
            Me.lblLoadoutDate.Text = "Label1"
            Me.lblLoadoutDate.Visible = False
            '
            'lblLoadoutTopic
            '
            Me.lblLoadoutTopic.AutoSize = True
            Me.lblLoadoutTopic.Location = New System.Drawing.Point(243, 100)
            Me.lblLoadoutTopic.Name = "lblLoadoutTopic"
            Me.lblLoadoutTopic.Size = New System.Drawing.Size(38, 13)
            Me.lblLoadoutTopic.TabIndex = 18
            Me.lblLoadoutTopic.TabStop = True
            Me.lblLoadoutTopic.Text = "Label1"
            Me.lblLoadoutTopic.Visible = False
            '
            'lblPG
            '
            Me.lblPG.AutoSize = True
            Me.lblPG.BackColor = System.Drawing.Color.Transparent
            Me.lblPG.Location = New System.Drawing.Point(275, 40)
            Me.lblPG.Name = "lblPG"
            Me.lblPG.Size = New System.Drawing.Size(13, 13)
            Me.lblPG.TabIndex = 27
            Me.lblPG.Text = "0"
            '
            'lblOptimalRange
            '
            Me.lblOptimalRange.AutoSize = True
            Me.lblOptimalRange.BackColor = System.Drawing.Color.Transparent
            Me.lblOptimalRange.Location = New System.Drawing.Point(275, 105)
            Me.lblOptimalRange.Name = "lblOptimalRange"
            Me.lblOptimalRange.Size = New System.Drawing.Size(13, 13)
            Me.lblOptimalRange.TabIndex = 23
            Me.lblOptimalRange.Text = "0"
            '
            'lblOptimalRangeLbl
            '
            Me.lblOptimalRangeLbl.AutoSize = True
            Me.lblOptimalRangeLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblOptimalRangeLbl.Location = New System.Drawing.Point(188, 105)
            Me.lblOptimalRangeLbl.Name = "lblOptimalRangeLbl"
            Me.lblOptimalRangeLbl.Size = New System.Drawing.Size(81, 13)
            Me.lblOptimalRangeLbl.TabIndex = 22
            Me.lblOptimalRangeLbl.Text = "Optimal Range:"
            '
            'lblMaxRange
            '
            Me.lblMaxRange.AutoSize = True
            Me.lblMaxRange.BackColor = System.Drawing.Color.Transparent
            Me.lblMaxRange.Location = New System.Drawing.Point(90, 105)
            Me.lblMaxRange.Name = "lblMaxRange"
            Me.lblMaxRange.Size = New System.Drawing.Size(13, 13)
            Me.lblMaxRange.TabIndex = 21
            Me.lblMaxRange.Text = "0"
            '
            'lblPGLbl
            '
            Me.lblPGLbl.AutoSize = True
            Me.lblPGLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblPGLbl.Location = New System.Drawing.Point(188, 40)
            Me.lblPGLbl.Name = "lblPGLbl"
            Me.lblPGLbl.Size = New System.Drawing.Size(59, 13)
            Me.lblPGLbl.TabIndex = 26
            Me.lblPGLbl.Text = "Powergrid:"
            '
            'LblMaxRangeLbl
            '
            Me.LblMaxRangeLbl.AutoSize = True
            Me.LblMaxRangeLbl.BackColor = System.Drawing.Color.Transparent
            Me.LblMaxRangeLbl.Location = New System.Drawing.Point(3, 105)
            Me.LblMaxRangeLbl.Name = "LblMaxRangeLbl"
            Me.LblMaxRangeLbl.Size = New System.Drawing.Size(77, 13)
            Me.LblMaxRangeLbl.TabIndex = 20
            Me.LblMaxRangeLbl.Text = "Target Range:"
            '
            'lblCPU
            '
            Me.lblCPU.AutoSize = True
            Me.lblCPU.BackColor = System.Drawing.Color.Transparent
            Me.lblCPU.Location = New System.Drawing.Point(90, 41)
            Me.lblCPU.Name = "lblCPU"
            Me.lblCPU.Size = New System.Drawing.Size(13, 13)
            Me.lblCPU.TabIndex = 25
            Me.lblCPU.Text = "0"
            '
            'lblVelocity
            '
            Me.lblVelocity.AutoSize = True
            Me.lblVelocity.BackColor = System.Drawing.Color.Transparent
            Me.lblVelocity.Location = New System.Drawing.Point(275, 92)
            Me.lblVelocity.Name = "lblVelocity"
            Me.lblVelocity.Size = New System.Drawing.Size(13, 13)
            Me.lblVelocity.TabIndex = 19
            Me.lblVelocity.Text = "0"
            '
            'lblVelocityLbl
            '
            Me.lblVelocityLbl.AutoSize = True
            Me.lblVelocityLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblVelocityLbl.Location = New System.Drawing.Point(188, 92)
            Me.lblVelocityLbl.Name = "lblVelocityLbl"
            Me.lblVelocityLbl.Size = New System.Drawing.Size(48, 13)
            Me.lblVelocityLbl.TabIndex = 18
            Me.lblVelocityLbl.Text = "Velocity:"
            '
            'lblCPULbl
            '
            Me.lblCPULbl.AutoSize = True
            Me.lblCPULbl.BackColor = System.Drawing.Color.Transparent
            Me.lblCPULbl.Location = New System.Drawing.Point(3, 40)
            Me.lblCPULbl.Name = "lblCPULbl"
            Me.lblCPULbl.Size = New System.Drawing.Size(31, 13)
            Me.lblCPULbl.TabIndex = 24
            Me.lblCPULbl.Text = "CPU:"
            '
            'lblCapacitor
            '
            Me.lblCapacitor.AutoSize = True
            Me.lblCapacitor.BackColor = System.Drawing.Color.Transparent
            Me.lblCapacitor.Location = New System.Drawing.Point(90, 92)
            Me.lblCapacitor.Name = "lblCapacitor"
            Me.lblCapacitor.Size = New System.Drawing.Size(13, 13)
            Me.lblCapacitor.TabIndex = 17
            Me.lblCapacitor.Text = "0"
            '
            'lblCapLbl
            '
            Me.lblCapLbl.AutoSize = True
            Me.lblCapLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblCapLbl.Location = New System.Drawing.Point(3, 92)
            Me.lblCapLbl.Name = "lblCapLbl"
            Me.lblCapLbl.Size = New System.Drawing.Size(57, 13)
            Me.lblCapLbl.TabIndex = 16
            Me.lblCapLbl.Text = "Capacitor:"
            '
            'lblArmorResists
            '
            Me.lblArmorResists.AutoSize = True
            Me.lblArmorResists.BackColor = System.Drawing.Color.Transparent
            Me.lblArmorResists.Location = New System.Drawing.Point(275, 79)
            Me.lblArmorResists.Name = "lblArmorResists"
            Me.lblArmorResists.Size = New System.Drawing.Size(13, 13)
            Me.lblArmorResists.TabIndex = 15
            Me.lblArmorResists.Text = "0"
            '
            'lblShieldResists
            '
            Me.lblShieldResists.AutoSize = True
            Me.lblShieldResists.BackColor = System.Drawing.Color.Transparent
            Me.lblShieldResists.Location = New System.Drawing.Point(90, 79)
            Me.lblShieldResists.Name = "lblShieldResists"
            Me.lblShieldResists.Size = New System.Drawing.Size(13, 13)
            Me.lblShieldResists.TabIndex = 14
            Me.lblShieldResists.Text = "0"
            '
            'lblArmorResistsLbl
            '
            Me.lblArmorResistsLbl.AutoSize = True
            Me.lblArmorResistsLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblArmorResistsLbl.Location = New System.Drawing.Point(188, 79)
            Me.lblArmorResistsLbl.Name = "lblArmorResistsLbl"
            Me.lblArmorResistsLbl.Size = New System.Drawing.Size(77, 13)
            Me.lblArmorResistsLbl.TabIndex = 13
            Me.lblArmorResistsLbl.Text = "Armor Resists:"
            '
            'lblShieldResistsLbl
            '
            Me.lblShieldResistsLbl.AutoSize = True
            Me.lblShieldResistsLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblShieldResistsLbl.Location = New System.Drawing.Point(3, 79)
            Me.lblShieldResistsLbl.Name = "lblShieldResistsLbl"
            Me.lblShieldResistsLbl.Size = New System.Drawing.Size(76, 13)
            Me.lblShieldResistsLbl.TabIndex = 12
            Me.lblShieldResistsLbl.Text = "Shield Resists:"
            '
            'lblVolley
            '
            Me.lblVolley.AutoSize = True
            Me.lblVolley.BackColor = System.Drawing.Color.Transparent
            Me.lblVolley.Location = New System.Drawing.Point(90, 66)
            Me.lblVolley.Name = "lblVolley"
            Me.lblVolley.Size = New System.Drawing.Size(13, 13)
            Me.lblVolley.TabIndex = 11
            Me.lblVolley.Text = "0"
            '
            'lblDPS
            '
            Me.lblDPS.AutoSize = True
            Me.lblDPS.BackColor = System.Drawing.Color.Transparent
            Me.lblDPS.Location = New System.Drawing.Point(275, 66)
            Me.lblDPS.Name = "lblDPS"
            Me.lblDPS.Size = New System.Drawing.Size(13, 13)
            Me.lblDPS.TabIndex = 10
            Me.lblDPS.Text = "0"
            '
            'lblDPSLbl
            '
            Me.lblDPSLbl.AutoSize = True
            Me.lblDPSLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblDPSLbl.Location = New System.Drawing.Point(188, 66)
            Me.lblDPSLbl.Name = "lblDPSLbl"
            Me.lblDPSLbl.Size = New System.Drawing.Size(58, 13)
            Me.lblDPSLbl.TabIndex = 9
            Me.lblDPSLbl.Text = "DPS Dealt:"
            '
            'lblVolleyLbl
            '
            Me.lblVolleyLbl.AutoSize = True
            Me.lblVolleyLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblVolleyLbl.Location = New System.Drawing.Point(3, 66)
            Me.lblVolleyLbl.Name = "lblVolleyLbl"
            Me.lblVolleyLbl.Size = New System.Drawing.Size(81, 13)
            Me.lblVolleyLbl.TabIndex = 8
            Me.lblVolleyLbl.Text = "Volley Damage:"
            '
            'lblTank
            '
            Me.lblTank.AutoSize = True
            Me.lblTank.BackColor = System.Drawing.Color.Transparent
            Me.lblTank.Location = New System.Drawing.Point(275, 51)
            Me.lblTank.Name = "lblTank"
            Me.lblTank.Size = New System.Drawing.Size(13, 13)
            Me.lblTank.TabIndex = 7
            Me.lblTank.Text = "0"
            '
            'lblEHP
            '
            Me.lblEHP.AutoSize = True
            Me.lblEHP.BackColor = System.Drawing.Color.Transparent
            Me.lblEHP.Location = New System.Drawing.Point(90, 53)
            Me.lblEHP.Name = "lblEHP"
            Me.lblEHP.Size = New System.Drawing.Size(13, 13)
            Me.lblEHP.TabIndex = 6
            Me.lblEHP.Text = "0"
            '
            'lblTankLbl
            '
            Me.lblTankLbl.AutoSize = True
            Me.lblTankLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblTankLbl.Location = New System.Drawing.Point(188, 53)
            Me.lblTankLbl.Name = "lblTankLbl"
            Me.lblTankLbl.Size = New System.Drawing.Size(57, 13)
            Me.lblTankLbl.TabIndex = 5
            Me.lblTankLbl.Text = "Max Tank:"
            '
            'lblEHPLbl
            '
            Me.lblEHPLbl.AutoSize = True
            Me.lblEHPLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblEHPLbl.Location = New System.Drawing.Point(3, 53)
            Me.lblEHPLbl.Name = "lblEHPLbl"
            Me.lblEHPLbl.Size = New System.Drawing.Size(70, 13)
            Me.lblEHPLbl.TabIndex = 4
            Me.lblEHPLbl.Text = "Effective HP:"
            '
            'cboProfiles
            '
            Me.cboProfiles.FormattingEnabled = True
            Me.cboProfiles.Location = New System.Drawing.Point(191, 16)
            Me.cboProfiles.Name = "cboProfiles"
            Me.cboProfiles.Size = New System.Drawing.Size(146, 21)
            Me.cboProfiles.TabIndex = 3
            '
            'lblProfileName
            '
            Me.lblProfileName.AutoSize = True
            Me.lblProfileName.BackColor = System.Drawing.Color.Transparent
            Me.lblProfileName.Location = New System.Drawing.Point(188, 0)
            Me.lblProfileName.Name = "lblProfileName"
            Me.lblProfileName.Size = New System.Drawing.Size(71, 13)
            Me.lblProfileName.TabIndex = 2
            Me.lblProfileName.Text = "Profile Name:"
            '
            'cboPilots
            '
            Me.cboPilots.FormattingEnabled = True
            Me.cboPilots.Location = New System.Drawing.Point(6, 16)
            Me.cboPilots.Name = "cboPilots"
            Me.cboPilots.Size = New System.Drawing.Size(146, 21)
            Me.cboPilots.TabIndex = 1
            '
            'lblPilot
            '
            Me.lblPilot.AutoSize = True
            Me.lblPilot.BackColor = System.Drawing.Color.Transparent
            Me.lblPilot.Location = New System.Drawing.Point(3, 0)
            Me.lblPilot.Name = "lblPilot"
            Me.lblPilot.Size = New System.Drawing.Size(61, 13)
            Me.lblPilot.TabIndex = 0
            Me.lblPilot.Text = "Pilot Name:"
            '
            'lvwSlots
            '
            Me.lvwSlots.AllowDrop = True
            Me.lvwSlots.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                         Or System.Windows.Forms.AnchorStyles.Left) _
                                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            '
            '
            '
            Me.lvwSlots.Border.Class = "ListViewBorder"
            Me.lvwSlots.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lvwSlots.ContextMenuStrip = Me.ctxSlots
            Me.lvwSlots.FullRowSelect = True
            ListViewGroup1.Header = "High Slots"
            ListViewGroup1.Name = "lvwgHighSlots"
            ListViewGroup2.Header = "Mid Slots"
            ListViewGroup2.Name = "lvwgMidSlots"
            ListViewGroup3.Header = "Low Slots"
            ListViewGroup3.Name = "lvwgLowSlots"
            ListViewGroup4.Header = "Rig Slots"
            ListViewGroup4.Name = "lvwgRigSlots"
            ListViewGroup5.Header = "Subsystems"
            ListViewGroup5.Name = "lvwgSubSlots"
            Me.lvwSlots.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4, ListViewGroup5})
            Me.lvwSlots.Location = New System.Drawing.Point(500, 154)
            Me.lvwSlots.Name = "lvwSlots"
            Me.lvwSlots.Size = New System.Drawing.Size(395, 456)
            Me.lvwSlots.TabIndex = 7
            Me.lvwSlots.Tag = ""
            Me.lvwSlots.UseCompatibleStateImageBehavior = False
            Me.lvwSlots.View = System.Windows.Forms.View.Details
            '
            'ctxSlots
            '
            Me.ctxSlots.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowInfo})
            Me.ctxSlots.Name = "ctxSlots"
            Me.ctxSlots.Size = New System.Drawing.Size(128, 26)
            '
            'mnuShowInfo
            '
            Me.mnuShowInfo.Name = "mnuShowInfo"
            Me.mnuShowInfo.Size = New System.Drawing.Size(127, 22)
            Me.mnuShowInfo.Text = "Show Info"
            '
            'gpStatistics
            '
            Me.gpStatistics.CanvasColor = System.Drawing.SystemColors.Control
            Me.gpStatistics.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
            Me.gpStatistics.Controls.Add(Me.lblPG)
            Me.gpStatistics.Controls.Add(Me.lblPilot)
            Me.gpStatistics.Controls.Add(Me.lblOptimalRange)
            Me.gpStatistics.Controls.Add(Me.cboPilots)
            Me.gpStatistics.Controls.Add(Me.lblOptimalRangeLbl)
            Me.gpStatistics.Controls.Add(Me.lblProfileName)
            Me.gpStatistics.Controls.Add(Me.lblMaxRange)
            Me.gpStatistics.Controls.Add(Me.cboProfiles)
            Me.gpStatistics.Controls.Add(Me.lblPGLbl)
            Me.gpStatistics.Controls.Add(Me.lblEHPLbl)
            Me.gpStatistics.Controls.Add(Me.LblMaxRangeLbl)
            Me.gpStatistics.Controls.Add(Me.lblTankLbl)
            Me.gpStatistics.Controls.Add(Me.lblCPU)
            Me.gpStatistics.Controls.Add(Me.lblEHP)
            Me.gpStatistics.Controls.Add(Me.lblVelocity)
            Me.gpStatistics.Controls.Add(Me.lblTank)
            Me.gpStatistics.Controls.Add(Me.lblVelocityLbl)
            Me.gpStatistics.Controls.Add(Me.lblVolleyLbl)
            Me.gpStatistics.Controls.Add(Me.lblCPULbl)
            Me.gpStatistics.Controls.Add(Me.lblDPSLbl)
            Me.gpStatistics.Controls.Add(Me.lblCapacitor)
            Me.gpStatistics.Controls.Add(Me.lblDPS)
            Me.gpStatistics.Controls.Add(Me.lblCapLbl)
            Me.gpStatistics.Controls.Add(Me.lblVolley)
            Me.gpStatistics.Controls.Add(Me.lblArmorResists)
            Me.gpStatistics.Controls.Add(Me.lblShieldResistsLbl)
            Me.gpStatistics.Controls.Add(Me.lblShieldResists)
            Me.gpStatistics.Controls.Add(Me.lblArmorResistsLbl)
            Me.gpStatistics.IsShadowEnabled = True
            Me.gpStatistics.Location = New System.Drawing.Point(500, 3)
            Me.gpStatistics.Name = "gpStatistics"
            Me.gpStatistics.Size = New System.Drawing.Size(395, 145)
            '
            '
            '
            Me.gpStatistics.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.gpStatistics.Style.BackColorGradientAngle = 90
            Me.gpStatistics.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.gpStatistics.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpStatistics.Style.BorderBottomWidth = 1
            Me.gpStatistics.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.gpStatistics.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpStatistics.Style.BorderLeftWidth = 1
            Me.gpStatistics.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpStatistics.Style.BorderRightWidth = 1
            Me.gpStatistics.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpStatistics.Style.BorderTopWidth = 1
            Me.gpStatistics.Style.Class = ""
            Me.gpStatistics.Style.CornerDiameter = 4
            Me.gpStatistics.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
            Me.gpStatistics.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.gpStatistics.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.gpStatistics.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.gpStatistics.StyleMouseDown.Class = ""
            Me.gpStatistics.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.gpStatistics.StyleMouseOver.Class = ""
            Me.gpStatistics.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.gpStatistics.TabIndex = 21
            Me.gpStatistics.Text = "Statistics"
            '
            'PanelEx1
            '
            Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
            Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.PanelEx1.Controls.Add(Me.adtLoadouts)
            Me.PanelEx1.Controls.Add(Me.btnImport)
            Me.PanelEx1.Controls.Add(Me.pbShip)
            Me.PanelEx1.Controls.Add(Me.gpStatistics)
            Me.PanelEx1.Controls.Add(Me.lblShipType)
            Me.PanelEx1.Controls.Add(Me.lvwSlots)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutTopic)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutNameLbl)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutDate)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutAuthorLbl)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutScore)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutScoreLbl)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutAuthor)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutDateLbl)
            Me.PanelEx1.Controls.Add(Me.lblLoadoutName)
            Me.PanelEx1.Controls.Add(Me.LblLoadoutTopicLbl)
            Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
            Me.PanelEx1.Name = "PanelEx1"
            Me.PanelEx1.Size = New System.Drawing.Size(899, 613)
            Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.PanelEx1.Style.GradientAngle = 90
            Me.PanelEx1.TabIndex = 22
            '
            'adtLoadouts
            '
            Me.adtLoadouts.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
            Me.adtLoadouts.AllowDrop = True
            Me.adtLoadouts.BackColor = System.Drawing.SystemColors.Window
            '
            '
            '
            Me.adtLoadouts.BackgroundStyle.Class = "TreeBorderKey"
            Me.adtLoadouts.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.adtLoadouts.Columns.Add(Me.colLoadout)
            Me.adtLoadouts.Columns.Add(Me.colAuthor)
            Me.adtLoadouts.Columns.Add(Me.colScore)
            Me.adtLoadouts.Columns.Add(Me.colDate)
            Me.adtLoadouts.DragDropEnabled = False
            Me.adtLoadouts.DragDropNodeCopyEnabled = False
            Me.adtLoadouts.ExpandWidth = 0
            Me.adtLoadouts.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
            Me.adtLoadouts.Location = New System.Drawing.Point(3, 154)
            Me.adtLoadouts.Name = "adtLoadouts"
            Me.adtLoadouts.NodesConnector = Me.NodeConnector1
            Me.adtLoadouts.NodeStyle = Me.ElementStyle1
            Me.adtLoadouts.PathSeparator = ";"
            Me.adtLoadouts.Size = New System.Drawing.Size(491, 456)
            Me.adtLoadouts.Styles.Add(Me.ElementStyle1)
            Me.adtLoadouts.TabIndex = 23
            Me.adtLoadouts.Text = "AdvTree1"
            '
            'colLoadout
            '
            Me.colLoadout.DisplayIndex = 1
            Me.colLoadout.Name = "colLoadout"
            Me.colLoadout.SortingEnabled = False
            Me.colLoadout.Text = "Loadout Name"
            Me.colLoadout.Width.Absolute = 200
            '
            'colAuthor
            '
            Me.colAuthor.DisplayIndex = 2
            Me.colAuthor.Name = "colAuthor"
            Me.colAuthor.SortingEnabled = False
            Me.colAuthor.Text = "Author"
            Me.colAuthor.Width.Absolute = 120
            '
            'colScore
            '
            Me.colScore.DisplayIndex = 3
            Me.colScore.Name = "colScore"
            Me.colScore.SortingEnabled = False
            Me.colScore.Text = "Score"
            Me.colScore.Width.Absolute = 50
            '
            'colDate
            '
            Me.colDate.DisplayIndex = 4
            Me.colDate.Name = "colDate"
            Me.colDate.SortingEnabled = False
            Me.colDate.Text = "Date"
            Me.colDate.Width.Absolute = 80
            '
            'NodeConnector1
            '
            Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
            '
            'ElementStyle1
            '
            Me.ElementStyle1.Class = ""
            Me.ElementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.ElementStyle1.Name = "ElementStyle1"
            Me.ElementStyle1.TextColor = System.Drawing.SystemColors.ControlText
            '
            'btnImport
            '
            Me.btnImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnImport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnImport.Location = New System.Drawing.Point(419, 125)
            Me.btnImport.Name = "btnImport"
            Me.btnImport.Size = New System.Drawing.Size(75, 23)
            Me.btnImport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnImport.TabIndex = 22
            Me.btnImport.Text = "Import"
            '
            'frmBCBrowser
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(899, 635)
            Me.Controls.Add(Me.PanelEx1)
            Me.Controls.Add(Me.StatusStrip1)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "frmBCBrowser"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "BattleClinic Browser"
            CType(Me.pbShip, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ctxLoadout.ResumeLayout(False)
            Me.StatusStrip1.ResumeLayout(False)
            Me.StatusStrip1.PerformLayout()
            Me.ctxSlots.ResumeLayout(False)
            Me.gpStatistics.ResumeLayout(False)
            Me.gpStatistics.PerformLayout()
            Me.PanelEx1.ResumeLayout(False)
            Me.PanelEx1.PerformLayout()
            CType(Me.adtLoadouts, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents lblShipType As System.Windows.Forms.Label
        Friend WithEvents pbShip As System.Windows.Forms.PictureBox
        Friend WithEvents lvwSlots As DevComponents.DotNetBar.Controls.ListViewEx
        Friend WithEvents ctxLoadout As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mnuViewLoadout As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
        Friend WithEvents lblBCStatus As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents lblLoadoutNameLbl As System.Windows.Forms.Label
        Friend WithEvents lblLoadoutAuthorLbl As System.Windows.Forms.Label
        Friend WithEvents lblLoadoutScoreLbl As System.Windows.Forms.Label
        Friend WithEvents lblLoadoutDateLbl As System.Windows.Forms.Label
        Friend WithEvents LblLoadoutTopicLbl As System.Windows.Forms.Label
        Friend WithEvents lblLoadoutName As System.Windows.Forms.Label
        Friend WithEvents lblLoadoutAuthor As System.Windows.Forms.Label
        Friend WithEvents lblLoadoutScore As System.Windows.Forms.Label
        Friend WithEvents lblLoadoutDate As System.Windows.Forms.Label
        Friend WithEvents lblLoadoutTopic As System.Windows.Forms.LinkLabel
        Friend WithEvents lblTopicAddress As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents mnuCopyURL As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
        Friend WithEvents lblPilot As System.Windows.Forms.Label
        Friend WithEvents cboProfiles As System.Windows.Forms.ComboBox
        Friend WithEvents lblProfileName As System.Windows.Forms.Label
        Friend WithEvents lblEHPLbl As System.Windows.Forms.Label
        Friend WithEvents lblTank As System.Windows.Forms.Label
        Friend WithEvents lblEHP As System.Windows.Forms.Label
        Friend WithEvents lblTankLbl As System.Windows.Forms.Label
        Friend WithEvents lblVolleyLbl As System.Windows.Forms.Label
        Friend WithEvents lblVelocity As System.Windows.Forms.Label
        Friend WithEvents lblVelocityLbl As System.Windows.Forms.Label
        Friend WithEvents lblCapacitor As System.Windows.Forms.Label
        Friend WithEvents lblCapLbl As System.Windows.Forms.Label
        Friend WithEvents lblArmorResists As System.Windows.Forms.Label
        Friend WithEvents lblShieldResists As System.Windows.Forms.Label
        Friend WithEvents lblArmorResistsLbl As System.Windows.Forms.Label
        Friend WithEvents lblShieldResistsLbl As System.Windows.Forms.Label
        Friend WithEvents lblVolley As System.Windows.Forms.Label
        Friend WithEvents lblDPS As System.Windows.Forms.Label
        Friend WithEvents lblDPSLbl As System.Windows.Forms.Label
        Friend WithEvents lblOptimalRange As System.Windows.Forms.Label
        Friend WithEvents lblOptimalRangeLbl As System.Windows.Forms.Label
        Friend WithEvents lblMaxRange As System.Windows.Forms.Label
        Friend WithEvents LblMaxRangeLbl As System.Windows.Forms.Label
        Friend WithEvents lblCPU As System.Windows.Forms.Label
        Friend WithEvents lblCPULbl As System.Windows.Forms.Label
        Friend WithEvents lblPG As System.Windows.Forms.Label
        Friend WithEvents lblPGLbl As System.Windows.Forms.Label
        Friend WithEvents gpStatistics As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
        Friend WithEvents btnImport As DevComponents.DotNetBar.ButtonX
        Friend WithEvents ctxSlots As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mnuShowInfo As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents adtLoadouts As DevComponents.AdvTree.AdvTree
        Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
        Friend WithEvents ElementStyle1 As DevComponents.DotNetBar.ElementStyle
        Friend WithEvents colLoadout As DevComponents.AdvTree.ColumnHeader
        Friend WithEvents colAuthor As DevComponents.AdvTree.ColumnHeader
        Friend WithEvents colScore As DevComponents.AdvTree.ColumnHeader
        Friend WithEvents colDate As DevComponents.AdvTree.ColumnHeader
    End Class
End NameSpace