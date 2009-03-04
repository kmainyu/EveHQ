<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBCBrowser
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
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("High Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mid Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Low Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Rig Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBCBrowser))
        Me.lblShipType = New System.Windows.Forms.Label
        Me.pbShip = New System.Windows.Forms.PictureBox
        Me.clvLoadouts = New DotNetLib.Windows.Forms.ContainerListView
        Me.colName = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colAuthor = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRating = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDate = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ctxLoadout = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuViewLoadout = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCopyURL = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.lblBCStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblTopicAddress = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblLoadoutNameLbl = New System.Windows.Forms.Label
        Me.lblLoadoutAuthorLbl = New System.Windows.Forms.Label
        Me.lblLoadoutScoreLbl = New System.Windows.Forms.Label
        Me.lblLoadoutDateLbl = New System.Windows.Forms.Label
        Me.LblLoadoutTopicLbl = New System.Windows.Forms.Label
        Me.lblLoadoutName = New System.Windows.Forms.Label
        Me.lblLoadoutAuthor = New System.Windows.Forms.Label
        Me.lblLoadoutScore = New System.Windows.Forms.Label
        Me.lblLoadoutDate = New System.Windows.Forms.Label
        Me.lblLoadoutTopic = New System.Windows.Forms.LinkLabel
        Me.gbStatistics = New System.Windows.Forms.GroupBox
        Me.lblOptimalRange = New System.Windows.Forms.Label
        Me.lblOptimalRangeLbl = New System.Windows.Forms.Label
        Me.lblMaxRange = New System.Windows.Forms.Label
        Me.LblMaxRangeLbl = New System.Windows.Forms.Label
        Me.lblVelocity = New System.Windows.Forms.Label
        Me.lblVelocityLbl = New System.Windows.Forms.Label
        Me.lblCapacitor = New System.Windows.Forms.Label
        Me.lblCapLbl = New System.Windows.Forms.Label
        Me.lblArmorResists = New System.Windows.Forms.Label
        Me.lblShieldResists = New System.Windows.Forms.Label
        Me.lblArmorResistsLbl = New System.Windows.Forms.Label
        Me.lblShieldResistsLbl = New System.Windows.Forms.Label
        Me.lblVolley = New System.Windows.Forms.Label
        Me.lblDPS = New System.Windows.Forms.Label
        Me.lblDPSLbl = New System.Windows.Forms.Label
        Me.lblVolleyLbl = New System.Windows.Forms.Label
        Me.lblTank = New System.Windows.Forms.Label
        Me.lblEHP = New System.Windows.Forms.Label
        Me.lblTankLbl = New System.Windows.Forms.Label
        Me.lblEHPLbl = New System.Windows.Forms.Label
        Me.cboProfiles = New System.Windows.Forms.ComboBox
        Me.lblProfileName = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.btnImport = New System.Windows.Forms.Button
        Me.lvwSlots = New EveHQ.HQF.ListViewNoFlicker
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxLoadout.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.gbStatistics.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblShipType
        '
        Me.lblShipType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblShipType.Font = New System.Drawing.Font("Tahoma", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipType.Location = New System.Drawing.Point(146, 12)
        Me.lblShipType.Name = "lblShipType"
        Me.lblShipType.Size = New System.Drawing.Size(758, 33)
        Me.lblShipType.TabIndex = 4
        Me.lblShipType.Text = "Ship Type"
        '
        'pbShip
        '
        Me.pbShip.Location = New System.Drawing.Point(12, 12)
        Me.pbShip.Name = "pbShip"
        Me.pbShip.Size = New System.Drawing.Size(128, 128)
        Me.pbShip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbShip.TabIndex = 3
        Me.pbShip.TabStop = False
        '
        'clvLoadouts
        '
        Me.clvLoadouts.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colName, Me.colAuthor, Me.colRating, Me.colDate})
        Me.clvLoadouts.DefaultItemHeight = 20
        Me.clvLoadouts.ItemContextMenu = Me.ctxLoadout
        Me.clvLoadouts.Location = New System.Drawing.Point(13, 147)
        Me.clvLoadouts.Name = "clvLoadouts"
        Me.clvLoadouts.Size = New System.Drawing.Size(490, 463)
        Me.clvLoadouts.TabIndex = 6
        '
        'colName
        '
        Me.colName.CustomSortTag = Nothing
        Me.colName.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colName.Tag = Nothing
        Me.colName.Text = "Loadout Name"
        Me.colName.Width = 200
        '
        'colAuthor
        '
        Me.colAuthor.CustomSortTag = Nothing
        Me.colAuthor.DisplayIndex = 1
        Me.colAuthor.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colAuthor.Tag = Nothing
        Me.colAuthor.Text = "Author"
        Me.colAuthor.Width = 100
        '
        'colRating
        '
        Me.colRating.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colRating.CustomSortTag = Nothing
        Me.colRating.DisplayIndex = 2
        Me.colRating.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colRating.Tag = Nothing
        Me.colRating.Text = "Score"
        Me.colRating.Width = 60
        '
        'colDate
        '
        Me.colDate.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colDate.CustomSortTag = Nothing
        Me.colDate.DisplayIndex = 3
        Me.colDate.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Date]
        Me.colDate.Tag = Nothing
        Me.colDate.Text = "Date"
        Me.colDate.Width = 100
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
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblBCStatus, Me.lblTopicAddress})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 613)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(916, 22)
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
        Me.lblTopicAddress.Size = New System.Drawing.Size(859, 17)
        Me.lblTopicAddress.Spring = True
        Me.lblTopicAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLoadoutNameLbl
        '
        Me.lblLoadoutNameLbl.AutoSize = True
        Me.lblLoadoutNameLbl.Location = New System.Drawing.Point(149, 57)
        Me.lblLoadoutNameLbl.Name = "lblLoadoutNameLbl"
        Me.lblLoadoutNameLbl.Size = New System.Drawing.Size(80, 13)
        Me.lblLoadoutNameLbl.TabIndex = 9
        Me.lblLoadoutNameLbl.Text = "Loadout Name:"
        Me.lblLoadoutNameLbl.Visible = False
        '
        'lblLoadoutAuthorLbl
        '
        Me.lblLoadoutAuthorLbl.AutoSize = True
        Me.lblLoadoutAuthorLbl.Location = New System.Drawing.Point(149, 70)
        Me.lblLoadoutAuthorLbl.Name = "lblLoadoutAuthorLbl"
        Me.lblLoadoutAuthorLbl.Size = New System.Drawing.Size(44, 13)
        Me.lblLoadoutAuthorLbl.TabIndex = 10
        Me.lblLoadoutAuthorLbl.Text = "Author:"
        Me.lblLoadoutAuthorLbl.Visible = False
        '
        'lblLoadoutScoreLbl
        '
        Me.lblLoadoutScoreLbl.AutoSize = True
        Me.lblLoadoutScoreLbl.Location = New System.Drawing.Point(149, 83)
        Me.lblLoadoutScoreLbl.Name = "lblLoadoutScoreLbl"
        Me.lblLoadoutScoreLbl.Size = New System.Drawing.Size(38, 13)
        Me.lblLoadoutScoreLbl.TabIndex = 11
        Me.lblLoadoutScoreLbl.Text = "Score:"
        Me.lblLoadoutScoreLbl.Visible = False
        '
        'lblLoadoutDateLbl
        '
        Me.lblLoadoutDateLbl.AutoSize = True
        Me.lblLoadoutDateLbl.Location = New System.Drawing.Point(149, 96)
        Me.lblLoadoutDateLbl.Name = "lblLoadoutDateLbl"
        Me.lblLoadoutDateLbl.Size = New System.Drawing.Size(34, 13)
        Me.lblLoadoutDateLbl.TabIndex = 12
        Me.lblLoadoutDateLbl.Text = "Date:"
        Me.lblLoadoutDateLbl.Visible = False
        '
        'LblLoadoutTopicLbl
        '
        Me.LblLoadoutTopicLbl.AutoSize = True
        Me.LblLoadoutTopicLbl.Location = New System.Drawing.Point(149, 109)
        Me.LblLoadoutTopicLbl.Name = "LblLoadoutTopicLbl"
        Me.LblLoadoutTopicLbl.Size = New System.Drawing.Size(71, 13)
        Me.LblLoadoutTopicLbl.TabIndex = 13
        Me.LblLoadoutTopicLbl.Text = "Website Link:"
        Me.LblLoadoutTopicLbl.Visible = False
        '
        'lblLoadoutName
        '
        Me.lblLoadoutName.AutoSize = True
        Me.lblLoadoutName.Location = New System.Drawing.Point(252, 57)
        Me.lblLoadoutName.Name = "lblLoadoutName"
        Me.lblLoadoutName.Size = New System.Drawing.Size(38, 13)
        Me.lblLoadoutName.TabIndex = 14
        Me.lblLoadoutName.Text = "Label1"
        Me.lblLoadoutName.Visible = False
        '
        'lblLoadoutAuthor
        '
        Me.lblLoadoutAuthor.AutoSize = True
        Me.lblLoadoutAuthor.Location = New System.Drawing.Point(252, 70)
        Me.lblLoadoutAuthor.Name = "lblLoadoutAuthor"
        Me.lblLoadoutAuthor.Size = New System.Drawing.Size(38, 13)
        Me.lblLoadoutAuthor.TabIndex = 15
        Me.lblLoadoutAuthor.Text = "Label1"
        Me.lblLoadoutAuthor.Visible = False
        '
        'lblLoadoutScore
        '
        Me.lblLoadoutScore.AutoSize = True
        Me.lblLoadoutScore.Location = New System.Drawing.Point(252, 83)
        Me.lblLoadoutScore.Name = "lblLoadoutScore"
        Me.lblLoadoutScore.Size = New System.Drawing.Size(38, 13)
        Me.lblLoadoutScore.TabIndex = 16
        Me.lblLoadoutScore.Text = "Label1"
        Me.lblLoadoutScore.Visible = False
        '
        'lblLoadoutDate
        '
        Me.lblLoadoutDate.AutoSize = True
        Me.lblLoadoutDate.Location = New System.Drawing.Point(252, 96)
        Me.lblLoadoutDate.Name = "lblLoadoutDate"
        Me.lblLoadoutDate.Size = New System.Drawing.Size(38, 13)
        Me.lblLoadoutDate.TabIndex = 17
        Me.lblLoadoutDate.Text = "Label1"
        Me.lblLoadoutDate.Visible = False
        '
        'lblLoadoutTopic
        '
        Me.lblLoadoutTopic.AutoSize = True
        Me.lblLoadoutTopic.Location = New System.Drawing.Point(252, 109)
        Me.lblLoadoutTopic.Name = "lblLoadoutTopic"
        Me.lblLoadoutTopic.Size = New System.Drawing.Size(38, 13)
        Me.lblLoadoutTopic.TabIndex = 18
        Me.lblLoadoutTopic.TabStop = True
        Me.lblLoadoutTopic.Text = "Label1"
        Me.lblLoadoutTopic.Visible = False
        '
        'gbStatistics
        '
        Me.gbStatistics.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbStatistics.Controls.Add(Me.lblOptimalRange)
        Me.gbStatistics.Controls.Add(Me.lblOptimalRangeLbl)
        Me.gbStatistics.Controls.Add(Me.lblMaxRange)
        Me.gbStatistics.Controls.Add(Me.LblMaxRangeLbl)
        Me.gbStatistics.Controls.Add(Me.lblVelocity)
        Me.gbStatistics.Controls.Add(Me.lblVelocityLbl)
        Me.gbStatistics.Controls.Add(Me.lblCapacitor)
        Me.gbStatistics.Controls.Add(Me.lblCapLbl)
        Me.gbStatistics.Controls.Add(Me.lblArmorResists)
        Me.gbStatistics.Controls.Add(Me.lblShieldResists)
        Me.gbStatistics.Controls.Add(Me.lblArmorResistsLbl)
        Me.gbStatistics.Controls.Add(Me.lblShieldResistsLbl)
        Me.gbStatistics.Controls.Add(Me.lblVolley)
        Me.gbStatistics.Controls.Add(Me.lblDPS)
        Me.gbStatistics.Controls.Add(Me.lblDPSLbl)
        Me.gbStatistics.Controls.Add(Me.lblVolleyLbl)
        Me.gbStatistics.Controls.Add(Me.lblTank)
        Me.gbStatistics.Controls.Add(Me.lblEHP)
        Me.gbStatistics.Controls.Add(Me.lblTankLbl)
        Me.gbStatistics.Controls.Add(Me.lblEHPLbl)
        Me.gbStatistics.Controls.Add(Me.cboProfiles)
        Me.gbStatistics.Controls.Add(Me.lblProfileName)
        Me.gbStatistics.Controls.Add(Me.cboPilots)
        Me.gbStatistics.Controls.Add(Me.lblPilot)
        Me.gbStatistics.Location = New System.Drawing.Point(509, 12)
        Me.gbStatistics.Name = "gbStatistics"
        Me.gbStatistics.Size = New System.Drawing.Size(395, 128)
        Me.gbStatistics.TabIndex = 19
        Me.gbStatistics.TabStop = False
        Me.gbStatistics.Text = "Statistics"
        Me.gbStatistics.Visible = False
        '
        'lblOptimalRange
        '
        Me.lblOptimalRange.AutoSize = True
        Me.lblOptimalRange.Location = New System.Drawing.Point(278, 110)
        Me.lblOptimalRange.Name = "lblOptimalRange"
        Me.lblOptimalRange.Size = New System.Drawing.Size(13, 13)
        Me.lblOptimalRange.TabIndex = 23
        Me.lblOptimalRange.Text = "0"
        '
        'lblOptimalRangeLbl
        '
        Me.lblOptimalRangeLbl.AutoSize = True
        Me.lblOptimalRangeLbl.Location = New System.Drawing.Point(191, 110)
        Me.lblOptimalRangeLbl.Name = "lblOptimalRangeLbl"
        Me.lblOptimalRangeLbl.Size = New System.Drawing.Size(81, 13)
        Me.lblOptimalRangeLbl.TabIndex = 22
        Me.lblOptimalRangeLbl.Text = "Optimal Range:"
        '
        'lblMaxRange
        '
        Me.lblMaxRange.AutoSize = True
        Me.lblMaxRange.Location = New System.Drawing.Point(93, 110)
        Me.lblMaxRange.Name = "lblMaxRange"
        Me.lblMaxRange.Size = New System.Drawing.Size(13, 13)
        Me.lblMaxRange.TabIndex = 21
        Me.lblMaxRange.Text = "0"
        '
        'LblMaxRangeLbl
        '
        Me.LblMaxRangeLbl.AutoSize = True
        Me.LblMaxRangeLbl.Location = New System.Drawing.Point(6, 110)
        Me.LblMaxRangeLbl.Name = "LblMaxRangeLbl"
        Me.LblMaxRangeLbl.Size = New System.Drawing.Size(77, 13)
        Me.LblMaxRangeLbl.TabIndex = 20
        Me.LblMaxRangeLbl.Text = "Target Range:"
        '
        'lblVelocity
        '
        Me.lblVelocity.AutoSize = True
        Me.lblVelocity.Location = New System.Drawing.Point(278, 97)
        Me.lblVelocity.Name = "lblVelocity"
        Me.lblVelocity.Size = New System.Drawing.Size(13, 13)
        Me.lblVelocity.TabIndex = 19
        Me.lblVelocity.Text = "0"
        '
        'lblVelocityLbl
        '
        Me.lblVelocityLbl.AutoSize = True
        Me.lblVelocityLbl.Location = New System.Drawing.Point(191, 97)
        Me.lblVelocityLbl.Name = "lblVelocityLbl"
        Me.lblVelocityLbl.Size = New System.Drawing.Size(48, 13)
        Me.lblVelocityLbl.TabIndex = 18
        Me.lblVelocityLbl.Text = "Velocity:"
        '
        'lblCapacitor
        '
        Me.lblCapacitor.AutoSize = True
        Me.lblCapacitor.Location = New System.Drawing.Point(93, 97)
        Me.lblCapacitor.Name = "lblCapacitor"
        Me.lblCapacitor.Size = New System.Drawing.Size(13, 13)
        Me.lblCapacitor.TabIndex = 17
        Me.lblCapacitor.Text = "0"
        '
        'lblCapLbl
        '
        Me.lblCapLbl.AutoSize = True
        Me.lblCapLbl.Location = New System.Drawing.Point(6, 97)
        Me.lblCapLbl.Name = "lblCapLbl"
        Me.lblCapLbl.Size = New System.Drawing.Size(57, 13)
        Me.lblCapLbl.TabIndex = 16
        Me.lblCapLbl.Text = "Capacitor:"
        '
        'lblArmorResists
        '
        Me.lblArmorResists.AutoSize = True
        Me.lblArmorResists.Location = New System.Drawing.Point(278, 84)
        Me.lblArmorResists.Name = "lblArmorResists"
        Me.lblArmorResists.Size = New System.Drawing.Size(13, 13)
        Me.lblArmorResists.TabIndex = 15
        Me.lblArmorResists.Text = "0"
        '
        'lblShieldResists
        '
        Me.lblShieldResists.AutoSize = True
        Me.lblShieldResists.Location = New System.Drawing.Point(93, 84)
        Me.lblShieldResists.Name = "lblShieldResists"
        Me.lblShieldResists.Size = New System.Drawing.Size(13, 13)
        Me.lblShieldResists.TabIndex = 14
        Me.lblShieldResists.Text = "0"
        '
        'lblArmorResistsLbl
        '
        Me.lblArmorResistsLbl.AutoSize = True
        Me.lblArmorResistsLbl.Location = New System.Drawing.Point(191, 84)
        Me.lblArmorResistsLbl.Name = "lblArmorResistsLbl"
        Me.lblArmorResistsLbl.Size = New System.Drawing.Size(77, 13)
        Me.lblArmorResistsLbl.TabIndex = 13
        Me.lblArmorResistsLbl.Text = "Armor Resists:"
        '
        'lblShieldResistsLbl
        '
        Me.lblShieldResistsLbl.AutoSize = True
        Me.lblShieldResistsLbl.Location = New System.Drawing.Point(6, 84)
        Me.lblShieldResistsLbl.Name = "lblShieldResistsLbl"
        Me.lblShieldResistsLbl.Size = New System.Drawing.Size(76, 13)
        Me.lblShieldResistsLbl.TabIndex = 12
        Me.lblShieldResistsLbl.Text = "Shield Resists:"
        '
        'lblVolley
        '
        Me.lblVolley.AutoSize = True
        Me.lblVolley.Location = New System.Drawing.Point(93, 71)
        Me.lblVolley.Name = "lblVolley"
        Me.lblVolley.Size = New System.Drawing.Size(13, 13)
        Me.lblVolley.TabIndex = 11
        Me.lblVolley.Text = "0"
        '
        'lblDPS
        '
        Me.lblDPS.AutoSize = True
        Me.lblDPS.Location = New System.Drawing.Point(278, 71)
        Me.lblDPS.Name = "lblDPS"
        Me.lblDPS.Size = New System.Drawing.Size(13, 13)
        Me.lblDPS.TabIndex = 10
        Me.lblDPS.Text = "0"
        '
        'lblDPSLbl
        '
        Me.lblDPSLbl.AutoSize = True
        Me.lblDPSLbl.Location = New System.Drawing.Point(191, 71)
        Me.lblDPSLbl.Name = "lblDPSLbl"
        Me.lblDPSLbl.Size = New System.Drawing.Size(58, 13)
        Me.lblDPSLbl.TabIndex = 9
        Me.lblDPSLbl.Text = "DPS Dealt:"
        '
        'lblVolleyLbl
        '
        Me.lblVolleyLbl.AutoSize = True
        Me.lblVolleyLbl.Location = New System.Drawing.Point(6, 71)
        Me.lblVolleyLbl.Name = "lblVolleyLbl"
        Me.lblVolleyLbl.Size = New System.Drawing.Size(81, 13)
        Me.lblVolleyLbl.TabIndex = 8
        Me.lblVolleyLbl.Text = "Volley Damage:"
        '
        'lblTank
        '
        Me.lblTank.AutoSize = True
        Me.lblTank.Location = New System.Drawing.Point(278, 56)
        Me.lblTank.Name = "lblTank"
        Me.lblTank.Size = New System.Drawing.Size(13, 13)
        Me.lblTank.TabIndex = 7
        Me.lblTank.Text = "0"
        '
        'lblEHP
        '
        Me.lblEHP.AutoSize = True
        Me.lblEHP.Location = New System.Drawing.Point(93, 58)
        Me.lblEHP.Name = "lblEHP"
        Me.lblEHP.Size = New System.Drawing.Size(13, 13)
        Me.lblEHP.TabIndex = 6
        Me.lblEHP.Text = "0"
        '
        'lblTankLbl
        '
        Me.lblTankLbl.AutoSize = True
        Me.lblTankLbl.Location = New System.Drawing.Point(191, 58)
        Me.lblTankLbl.Name = "lblTankLbl"
        Me.lblTankLbl.Size = New System.Drawing.Size(57, 13)
        Me.lblTankLbl.TabIndex = 5
        Me.lblTankLbl.Text = "Max Tank:"
        '
        'lblEHPLbl
        '
        Me.lblEHPLbl.AutoSize = True
        Me.lblEHPLbl.Location = New System.Drawing.Point(6, 58)
        Me.lblEHPLbl.Name = "lblEHPLbl"
        Me.lblEHPLbl.Size = New System.Drawing.Size(70, 13)
        Me.lblEHPLbl.TabIndex = 4
        Me.lblEHPLbl.Text = "Effective HP:"
        '
        'cboProfiles
        '
        Me.cboProfiles.FormattingEnabled = True
        Me.cboProfiles.Location = New System.Drawing.Point(194, 32)
        Me.cboProfiles.Name = "cboProfiles"
        Me.cboProfiles.Size = New System.Drawing.Size(146, 21)
        Me.cboProfiles.TabIndex = 3
        '
        'lblProfileName
        '
        Me.lblProfileName.AutoSize = True
        Me.lblProfileName.Location = New System.Drawing.Point(191, 16)
        Me.lblProfileName.Name = "lblProfileName"
        Me.lblProfileName.Size = New System.Drawing.Size(71, 13)
        Me.lblProfileName.TabIndex = 2
        Me.lblProfileName.Text = "Profile Name:"
        '
        'cboPilots
        '
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(9, 32)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(146, 21)
        Me.cboPilots.TabIndex = 1
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(6, 16)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(61, 13)
        Me.lblPilot.TabIndex = 0
        Me.lblPilot.Text = "Pilot Name:"
        '
        'btnImport
        '
        Me.btnImport.Enabled = False
        Me.btnImport.Location = New System.Drawing.Point(428, 117)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(75, 23)
        Me.btnImport.TabIndex = 20
        Me.btnImport.Text = "Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'lvwSlots
        '
        Me.lvwSlots.AllowDrop = True
        Me.lvwSlots.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwSlots.FullRowSelect = True
        ListViewGroup1.Header = "High Slots"
        ListViewGroup1.Name = "lvwgHighSlots"
        ListViewGroup2.Header = "Mid Slots"
        ListViewGroup2.Name = "lvwgMidSlots"
        ListViewGroup3.Header = "Low Slots"
        ListViewGroup3.Name = "lvwgLowSlots"
        ListViewGroup4.Header = "Rig Slots"
        ListViewGroup4.Name = "lvwgRigSlots"
        Me.lvwSlots.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4})
        Me.lvwSlots.Location = New System.Drawing.Point(509, 147)
        Me.lvwSlots.Name = "lvwSlots"
        Me.lvwSlots.Size = New System.Drawing.Size(395, 463)
        Me.lvwSlots.TabIndex = 7
        Me.lvwSlots.Tag = ""
        Me.lvwSlots.UseCompatibleStateImageBehavior = False
        Me.lvwSlots.View = System.Windows.Forms.View.Details
        '
        'frmBCBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(916, 635)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.gbStatistics)
        Me.Controls.Add(Me.lblLoadoutTopic)
        Me.Controls.Add(Me.lblLoadoutDate)
        Me.Controls.Add(Me.lblLoadoutScore)
        Me.Controls.Add(Me.lblLoadoutAuthor)
        Me.Controls.Add(Me.lblLoadoutName)
        Me.Controls.Add(Me.LblLoadoutTopicLbl)
        Me.Controls.Add(Me.lblLoadoutDateLbl)
        Me.Controls.Add(Me.lblLoadoutScoreLbl)
        Me.Controls.Add(Me.lblLoadoutAuthorLbl)
        Me.Controls.Add(Me.lblLoadoutNameLbl)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.lvwSlots)
        Me.Controls.Add(Me.clvLoadouts)
        Me.Controls.Add(Me.lblShipType)
        Me.Controls.Add(Me.pbShip)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBCBrowser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BattleClinic Browser"
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxLoadout.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.gbStatistics.ResumeLayout(False)
        Me.gbStatistics.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblShipType As System.Windows.Forms.Label
    Friend WithEvents pbShip As System.Windows.Forms.PictureBox
    Friend WithEvents clvLoadouts As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colName As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colAuthor As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRating As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colDate As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lvwSlots As EveHQ.HQF.ListViewNoFlicker
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
    Friend WithEvents gbStatistics As System.Windows.Forms.GroupBox
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
    Friend WithEvents btnImport As System.Windows.Forms.Button
End Class
