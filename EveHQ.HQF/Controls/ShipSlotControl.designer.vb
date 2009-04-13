<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShipSlotControl
    Inherits System.Windows.Forms.UserControl

    'UserControl1 overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShipSlotControl))
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("High Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mid Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Low Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Rig Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Subsystem Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Me.lblFittingMarketPrice = New System.Windows.Forms.Label
        Me.lblShipMarketPrice = New System.Windows.Forms.Label
        Me.lblTurretSlots = New System.Windows.Forms.Label
        Me.lblLauncherSlots = New System.Windows.Forms.Label
        Me.lblRigSlots = New System.Windows.Forms.Label
        Me.lblLowSlots = New System.Windows.Forms.Label
        Me.lblMidSlots = New System.Windows.Forms.Label
        Me.lblHighSlots = New System.Windows.Forms.Label
        Me.ctxSlots = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.imgState = New System.Windows.Forms.ImageList(Me.components)
        Me.ctxBays = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ctxRemoveItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.ctxAlterQuantity = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxSplitBatch = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.ctxShowBayInfoItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxRemoteFittings = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveFittingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxRemoteModule = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuShowRemoteModInfo = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnToggleStorage = New System.Windows.Forms.Button
        Me.pbShip = New System.Windows.Forms.PictureBox
        Me.ctxShipSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAlterRelevantSkills = New System.Windows.Forms.ToolStripMenuItem
        Me.panelFunctions = New System.Windows.Forms.Panel
        Me.lblSubSlots = New System.Windows.Forms.Label
        Me.pbShipInfo = New System.Windows.Forms.PictureBox
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.tabStorage = New System.Windows.Forms.TabControl
        Me.tabDroneBay = New System.Windows.Forms.TabPage
        Me.panelDrone = New System.Windows.Forms.Panel
        Me.pbDroneBay = New VistaStyleProgressBar.ProgressBar
        Me.lblDroneBay = New System.Windows.Forms.Label
        Me.btnMergeDrones = New System.Windows.Forms.Button
        Me.lvwDroneBay = New System.Windows.Forms.ListView
        Me.colDroneBayType = New System.Windows.Forms.ColumnHeader
        Me.colDroneBayQty = New System.Windows.Forms.ColumnHeader
        Me.tabCargoBay = New System.Windows.Forms.TabPage
        Me.panelCargo = New System.Windows.Forms.Panel
        Me.pbCargoBay = New VistaStyleProgressBar.ProgressBar
        Me.lblCargoBay = New System.Windows.Forms.Label
        Me.btnMergeCargo = New System.Windows.Forms.Button
        Me.lvwCargoBay = New System.Windows.Forms.ListView
        Me.colCargoBayType = New System.Windows.Forms.ColumnHeader
        Me.colCargoBayQty = New System.Windows.Forms.ColumnHeader
        Me.tabRemote = New System.Windows.Forms.TabPage
        Me.panelRemote = New System.Windows.Forms.Panel
        Me.cboFitting = New System.Windows.Forms.ComboBox
        Me.btnAddRemoteFitting = New System.Windows.Forms.Button
        Me.lblFitting = New System.Windows.Forms.Label
        Me.lvwRemoteFittings = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.lblPilot = New System.Windows.Forms.Label
        Me.lvwRemoteEffects = New System.Windows.Forms.ListView
        Me.colModule = New System.Windows.Forms.ColumnHeader
        Me.cboPilot = New System.Windows.Forms.ComboBox
        Me.btnUpdateRemoteEffects = New System.Windows.Forms.Button
        Me.tabFleet = New System.Windows.Forms.TabPage
        Me.panelFleet = New System.Windows.Forms.Panel
        Me.chkFCActive = New System.Windows.Forms.CheckBox
        Me.chkWCActive = New System.Windows.Forms.CheckBox
        Me.chkSCActive = New System.Windows.Forms.CheckBox
        Me.cboSCShip = New System.Windows.Forms.ComboBox
        Me.cboFCPilot = New System.Windows.Forms.ComboBox
        Me.cboWCPilot = New System.Windows.Forms.ComboBox
        Me.lblSCShip = New System.Windows.Forms.Label
        Me.lblWC = New System.Windows.Forms.Label
        Me.btnLeaveFleet = New System.Windows.Forms.Button
        Me.lblFC = New System.Windows.Forms.Label
        Me.lblFCShip = New System.Windows.Forms.Label
        Me.lblSC = New System.Windows.Forms.Label
        Me.cboSCPilot = New System.Windows.Forms.ComboBox
        Me.lblFleetStatus = New System.Windows.Forms.Label
        Me.lblWCShip = New System.Windows.Forms.Label
        Me.lblFleetStatusLabel = New System.Windows.Forms.Label
        Me.cboWCShip = New System.Windows.Forms.ComboBox
        Me.cboFCShip = New System.Windows.Forms.ComboBox
        Me.lvwSlots = New EveHQ.HQF.ListViewNoFlicker
        Me.ctxSlots.SuspendLayout()
        Me.ctxBays.SuspendLayout()
        Me.ctxRemoteFittings.SuspendLayout()
        Me.ctxRemoteModule.SuspendLayout()
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxShipSkills.SuspendLayout()
        Me.panelFunctions.SuspendLayout()
        CType(Me.pbShipInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabStorage.SuspendLayout()
        Me.tabDroneBay.SuspendLayout()
        Me.panelDrone.SuspendLayout()
        Me.tabCargoBay.SuspendLayout()
        Me.panelCargo.SuspendLayout()
        Me.tabRemote.SuspendLayout()
        Me.panelRemote.SuspendLayout()
        Me.tabFleet.SuspendLayout()
        Me.panelFleet.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblFittingMarketPrice
        '
        Me.lblFittingMarketPrice.AutoSize = True
        Me.lblFittingMarketPrice.Location = New System.Drawing.Point(117, 18)
        Me.lblFittingMarketPrice.Name = "lblFittingMarketPrice"
        Me.lblFittingMarketPrice.Size = New System.Drawing.Size(208, 13)
        Me.lblFittingMarketPrice.TabIndex = 10
        Me.lblFittingMarketPrice.Text = "Fitting Market Price: 0,000,000,000.00 ISK"
        '
        'lblShipMarketPrice
        '
        Me.lblShipMarketPrice.AutoSize = True
        Me.lblShipMarketPrice.Location = New System.Drawing.Point(331, 18)
        Me.lblShipMarketPrice.Name = "lblShipMarketPrice"
        Me.lblShipMarketPrice.Size = New System.Drawing.Size(201, 13)
        Me.lblShipMarketPrice.TabIndex = 8
        Me.lblShipMarketPrice.Text = "Ship Market Price: 0,000,000,000.00 ISK"
        '
        'lblTurretSlots
        '
        Me.lblTurretSlots.AutoSize = True
        Me.lblTurretSlots.Location = New System.Drawing.Point(462, 5)
        Me.lblTurretSlots.Name = "lblTurretSlots"
        Me.lblTurretSlots.Size = New System.Drawing.Size(63, 13)
        Me.lblTurretSlots.TabIndex = 6
        Me.lblTurretSlots.Text = "Turrets: 0/0"
        '
        'lblLauncherSlots
        '
        Me.lblLauncherSlots.AutoSize = True
        Me.lblLauncherSlots.Location = New System.Drawing.Point(376, 5)
        Me.lblLauncherSlots.Name = "lblLauncherSlots"
        Me.lblLauncherSlots.Size = New System.Drawing.Size(80, 13)
        Me.lblLauncherSlots.TabIndex = 5
        Me.lblLauncherSlots.Text = "Launchers: 0/0"
        '
        'lblRigSlots
        '
        Me.lblRigSlots.AutoSize = True
        Me.lblRigSlots.Location = New System.Drawing.Point(272, 5)
        Me.lblRigSlots.Name = "lblRigSlots"
        Me.lblRigSlots.Size = New System.Drawing.Size(46, 13)
        Me.lblRigSlots.TabIndex = 4
        Me.lblRigSlots.Text = "Rig: 0/0"
        '
        'lblLowSlots
        '
        Me.lblLowSlots.AutoSize = True
        Me.lblLowSlots.Location = New System.Drawing.Point(216, 5)
        Me.lblLowSlots.Name = "lblLowSlots"
        Me.lblLowSlots.Size = New System.Drawing.Size(50, 13)
        Me.lblLowSlots.TabIndex = 3
        Me.lblLowSlots.Text = "Low: 0/0"
        '
        'lblMidSlots
        '
        Me.lblMidSlots.AutoSize = True
        Me.lblMidSlots.Location = New System.Drawing.Point(163, 5)
        Me.lblMidSlots.Name = "lblMidSlots"
        Me.lblMidSlots.Size = New System.Drawing.Size(47, 13)
        Me.lblMidSlots.TabIndex = 2
        Me.lblMidSlots.Text = "Mid: 0/0"
        '
        'lblHighSlots
        '
        Me.lblHighSlots.AutoSize = True
        Me.lblHighSlots.Location = New System.Drawing.Point(117, 5)
        Me.lblHighSlots.Name = "lblHighSlots"
        Me.lblHighSlots.Size = New System.Drawing.Size(40, 13)
        Me.lblHighSlots.TabIndex = 1
        Me.lblHighSlots.Text = "Hi: 0/0"
        '
        'ctxSlots
        '
        Me.ctxSlots.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowInfoToolStripMenuItem})
        Me.ctxSlots.Name = "ctxSlots"
        Me.ctxSlots.Size = New System.Drawing.Size(128, 26)
        Me.ctxSlots.Tag = " "
        '
        'ShowInfoToolStripMenuItem
        '
        Me.ShowInfoToolStripMenuItem.Name = "ShowInfoToolStripMenuItem"
        Me.ShowInfoToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.ShowInfoToolStripMenuItem.Text = "Show Info"
        '
        'imgState
        '
        Me.imgState.ImageStream = CType(resources.GetObject("imgState.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgState.TransparentColor = System.Drawing.Color.Transparent
        Me.imgState.Images.SetKeyName(0, "Status_grey2.gif")
        Me.imgState.Images.SetKeyName(1, "Status_yellow.gif")
        Me.imgState.Images.SetKeyName(2, "Status_green.gif")
        Me.imgState.Images.SetKeyName(3, "icon22_10.png")
        Me.imgState.Images.SetKeyName(4, "Status_red.gif")
        Me.imgState.Images.SetKeyName(5, "Status_grey.gif")
        '
        'ctxBays
        '
        Me.ctxBays.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxRemoveItem, Me.ToolStripMenuItem1, Me.ctxAlterQuantity, Me.ctxSplitBatch, Me.ToolStripMenuItem2, Me.ctxShowBayInfoItem})
        Me.ctxBays.Name = "ctx"
        Me.ctxBays.Size = New System.Drawing.Size(163, 104)
        '
        'ctxRemoveItem
        '
        Me.ctxRemoveItem.Name = "ctxRemoveItem"
        Me.ctxRemoveItem.Size = New System.Drawing.Size(162, 22)
        Me.ctxRemoveItem.Text = "Remove Item"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(159, 6)
        '
        'ctxAlterQuantity
        '
        Me.ctxAlterQuantity.Name = "ctxAlterQuantity"
        Me.ctxAlterQuantity.Size = New System.Drawing.Size(162, 22)
        Me.ctxAlterQuantity.Text = "Alter Quantity"
        '
        'ctxSplitBatch
        '
        Me.ctxSplitBatch.Name = "ctxSplitBatch"
        Me.ctxSplitBatch.Size = New System.Drawing.Size(162, 22)
        Me.ctxSplitBatch.Text = "Split Batch"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(159, 6)
        '
        'ctxShowBayInfoItem
        '
        Me.ctxShowBayInfoItem.Name = "ctxShowBayInfoItem"
        Me.ctxShowBayInfoItem.Size = New System.Drawing.Size(162, 22)
        Me.ctxShowBayInfoItem.Text = "Show Drone Info"
        '
        'ctxRemoteFittings
        '
        Me.ctxRemoteFittings.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveFittingToolStripMenuItem})
        Me.ctxRemoteFittings.Name = "ctxRemoteFittings"
        Me.ctxRemoteFittings.Size = New System.Drawing.Size(179, 26)
        '
        'RemoveFittingToolStripMenuItem
        '
        Me.RemoveFittingToolStripMenuItem.Name = "RemoveFittingToolStripMenuItem"
        Me.RemoveFittingToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.RemoveFittingToolStripMenuItem.Size = New System.Drawing.Size(178, 22)
        Me.RemoveFittingToolStripMenuItem.Text = "Remove Fitting"
        '
        'ctxRemoteModule
        '
        Me.ctxRemoteModule.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowRemoteModInfo})
        Me.ctxRemoteModule.Name = "ctxRemoteModule"
        Me.ctxRemoteModule.Size = New System.Drawing.Size(128, 26)
        '
        'mnuShowRemoteModInfo
        '
        Me.mnuShowRemoteModInfo.Name = "mnuShowRemoteModInfo"
        Me.mnuShowRemoteModInfo.Size = New System.Drawing.Size(127, 22)
        Me.mnuShowRemoteModInfo.Text = "Show Info"
        '
        'btnToggleStorage
        '
        Me.btnToggleStorage.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCargo
        Me.btnToggleStorage.Location = New System.Drawing.Point(79, 2)
        Me.btnToggleStorage.Name = "btnToggleStorage"
        Me.btnToggleStorage.Size = New System.Drawing.Size(32, 32)
        Me.btnToggleStorage.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.btnToggleStorage, "Toggle Storage Bays")
        Me.btnToggleStorage.UseVisualStyleBackColor = True
        '
        'pbShip
        '
        Me.pbShip.ContextMenuStrip = Me.ctxShipSkills
        Me.pbShip.InitialImage = Global.EveHQ.HQF.My.Resources.Resources.imgInfo2
        Me.pbShip.Location = New System.Drawing.Point(3, 2)
        Me.pbShip.Name = "pbShip"
        Me.pbShip.Size = New System.Drawing.Size(32, 32)
        Me.pbShip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbShip.TabIndex = 13
        Me.pbShip.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShip, "Right-click to set relevant skills provided by bonuses on this ship")
        '
        'ctxShipSkills
        '
        Me.ctxShipSkills.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAlterRelevantSkills})
        Me.ctxShipSkills.Name = "ctxShipSkills"
        Me.ctxShipSkills.Size = New System.Drawing.Size(177, 26)
        '
        'mnuAlterRelevantSkills
        '
        Me.mnuAlterRelevantSkills.Name = "mnuAlterRelevantSkills"
        Me.mnuAlterRelevantSkills.Size = New System.Drawing.Size(176, 22)
        Me.mnuAlterRelevantSkills.Text = "Alter Relevant Skills"
        '
        'panelFunctions
        '
        Me.panelFunctions.BackColor = System.Drawing.Color.Transparent
        Me.panelFunctions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelFunctions.Controls.Add(Me.pbShip)
        Me.panelFunctions.Controls.Add(Me.lblSubSlots)
        Me.panelFunctions.Controls.Add(Me.lblFittingMarketPrice)
        Me.panelFunctions.Controls.Add(Me.pbShipInfo)
        Me.panelFunctions.Controls.Add(Me.lblShipMarketPrice)
        Me.panelFunctions.Controls.Add(Me.btnToggleStorage)
        Me.panelFunctions.Controls.Add(Me.lblTurretSlots)
        Me.panelFunctions.Controls.Add(Me.lblMidSlots)
        Me.panelFunctions.Controls.Add(Me.lblLauncherSlots)
        Me.panelFunctions.Controls.Add(Me.lblHighSlots)
        Me.panelFunctions.Controls.Add(Me.lblRigSlots)
        Me.panelFunctions.Controls.Add(Me.lblLowSlots)
        Me.panelFunctions.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelFunctions.Location = New System.Drawing.Point(0, 0)
        Me.panelFunctions.Name = "panelFunctions"
        Me.panelFunctions.Size = New System.Drawing.Size(676, 38)
        Me.panelFunctions.TabIndex = 2
        '
        'lblSubSlots
        '
        Me.lblSubSlots.AutoSize = True
        Me.lblSubSlots.Location = New System.Drawing.Point(324, 5)
        Me.lblSubSlots.Name = "lblSubSlots"
        Me.lblSubSlots.Size = New System.Drawing.Size(46, 13)
        Me.lblSubSlots.TabIndex = 12
        Me.lblSubSlots.Text = "Rig: 0/0"
        '
        'pbShipInfo
        '
        Me.pbShipInfo.ContextMenuStrip = Me.ctxShipSkills
        Me.pbShipInfo.Image = Global.EveHQ.HQF.My.Resources.Resources.imgInfo1
        Me.pbShipInfo.Location = New System.Drawing.Point(41, 2)
        Me.pbShipInfo.Name = "pbShipInfo"
        Me.pbShipInfo.Size = New System.Drawing.Size(32, 32)
        Me.pbShipInfo.TabIndex = 0
        Me.pbShipInfo.TabStop = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 39)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lvwSlots)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.tabStorage)
        Me.SplitContainer1.Panel2MinSize = 10
        Me.SplitContainer1.Size = New System.Drawing.Size(676, 521)
        Me.SplitContainer1.SplitterDistance = 275
        Me.SplitContainer1.TabIndex = 1
        '
        'tabStorage
        '
        Me.tabStorage.Controls.Add(Me.tabDroneBay)
        Me.tabStorage.Controls.Add(Me.tabCargoBay)
        Me.tabStorage.Controls.Add(Me.tabRemote)
        Me.tabStorage.Controls.Add(Me.tabFleet)
        Me.tabStorage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabStorage.Location = New System.Drawing.Point(0, 0)
        Me.tabStorage.Multiline = True
        Me.tabStorage.Name = "tabStorage"
        Me.tabStorage.SelectedIndex = 0
        Me.tabStorage.Size = New System.Drawing.Size(676, 242)
        Me.tabStorage.TabIndex = 0
        '
        'tabDroneBay
        '
        Me.tabDroneBay.Controls.Add(Me.panelDrone)
        Me.tabDroneBay.Location = New System.Drawing.Point(4, 22)
        Me.tabDroneBay.Name = "tabDroneBay"
        Me.tabDroneBay.Size = New System.Drawing.Size(668, 216)
        Me.tabDroneBay.TabIndex = 0
        Me.tabDroneBay.Text = "Drone Bay"
        Me.tabDroneBay.UseVisualStyleBackColor = True
        '
        'panelDrone
        '
        Me.panelDrone.BackColor = System.Drawing.SystemColors.Control
        Me.panelDrone.Controls.Add(Me.pbDroneBay)
        Me.panelDrone.Controls.Add(Me.lblDroneBay)
        Me.panelDrone.Controls.Add(Me.btnMergeDrones)
        Me.panelDrone.Controls.Add(Me.lvwDroneBay)
        Me.panelDrone.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelDrone.Location = New System.Drawing.Point(0, 0)
        Me.panelDrone.Name = "panelDrone"
        Me.panelDrone.Size = New System.Drawing.Size(668, 216)
        Me.panelDrone.TabIndex = 32
        '
        'pbDroneBay
        '
        Me.pbDroneBay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbDroneBay.BackColor = System.Drawing.Color.Transparent
        Me.pbDroneBay.EndColor = System.Drawing.Color.LimeGreen
        Me.pbDroneBay.GlowColor = System.Drawing.Color.LightGreen
        Me.pbDroneBay.Location = New System.Drawing.Point(3, 21)
        Me.pbDroneBay.Name = "pbDroneBay"
        Me.pbDroneBay.Size = New System.Drawing.Size(566, 10)
        Me.pbDroneBay.StartColor = System.Drawing.Color.LimeGreen
        Me.pbDroneBay.TabIndex = 31
        Me.pbDroneBay.Value = 50
        '
        'lblDroneBay
        '
        Me.lblDroneBay.AutoSize = True
        Me.lblDroneBay.Location = New System.Drawing.Point(3, 5)
        Me.lblDroneBay.Name = "lblDroneBay"
        Me.lblDroneBay.Size = New System.Drawing.Size(86, 13)
        Me.lblDroneBay.TabIndex = 0
        Me.lblDroneBay.Text = "0.00 / 000.00 m³"
        '
        'btnMergeDrones
        '
        Me.btnMergeDrones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMergeDrones.Location = New System.Drawing.Point(575, 8)
        Me.btnMergeDrones.Name = "btnMergeDrones"
        Me.btnMergeDrones.Size = New System.Drawing.Size(90, 23)
        Me.btnMergeDrones.TabIndex = 3
        Me.btnMergeDrones.Text = "Merge Drones"
        Me.btnMergeDrones.UseVisualStyleBackColor = True
        '
        'lvwDroneBay
        '
        Me.lvwDroneBay.AllowDrop = True
        Me.lvwDroneBay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwDroneBay.CheckBoxes = True
        Me.lvwDroneBay.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colDroneBayType, Me.colDroneBayQty})
        Me.lvwDroneBay.ContextMenuStrip = Me.ctxBays
        Me.lvwDroneBay.FullRowSelect = True
        Me.lvwDroneBay.GridLines = True
        Me.lvwDroneBay.Location = New System.Drawing.Point(3, 35)
        Me.lvwDroneBay.Name = "lvwDroneBay"
        Me.lvwDroneBay.Size = New System.Drawing.Size(662, 178)
        Me.lvwDroneBay.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwDroneBay.TabIndex = 2
        Me.lvwDroneBay.UseCompatibleStateImageBehavior = False
        Me.lvwDroneBay.View = System.Windows.Forms.View.Details
        '
        'colDroneBayType
        '
        Me.colDroneBayType.Text = "Drone Type"
        Me.colDroneBayType.Width = 225
        '
        'colDroneBayQty
        '
        Me.colDroneBayQty.Text = "Qty"
        Me.colDroneBayQty.Width = 35
        '
        'tabCargoBay
        '
        Me.tabCargoBay.Controls.Add(Me.panelCargo)
        Me.tabCargoBay.Location = New System.Drawing.Point(4, 22)
        Me.tabCargoBay.Name = "tabCargoBay"
        Me.tabCargoBay.Size = New System.Drawing.Size(668, 216)
        Me.tabCargoBay.TabIndex = 1
        Me.tabCargoBay.Text = "CargoBay"
        Me.tabCargoBay.UseVisualStyleBackColor = True
        '
        'panelCargo
        '
        Me.panelCargo.BackColor = System.Drawing.SystemColors.Control
        Me.panelCargo.Controls.Add(Me.pbCargoBay)
        Me.panelCargo.Controls.Add(Me.lblCargoBay)
        Me.panelCargo.Controls.Add(Me.btnMergeCargo)
        Me.panelCargo.Controls.Add(Me.lvwCargoBay)
        Me.panelCargo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelCargo.Location = New System.Drawing.Point(0, 0)
        Me.panelCargo.Name = "panelCargo"
        Me.panelCargo.Size = New System.Drawing.Size(668, 216)
        Me.panelCargo.TabIndex = 33
        '
        'pbCargoBay
        '
        Me.pbCargoBay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbCargoBay.BackColor = System.Drawing.Color.Transparent
        Me.pbCargoBay.EndColor = System.Drawing.Color.LimeGreen
        Me.pbCargoBay.GlowColor = System.Drawing.Color.LightGreen
        Me.pbCargoBay.Location = New System.Drawing.Point(3, 21)
        Me.pbCargoBay.Name = "pbCargoBay"
        Me.pbCargoBay.Size = New System.Drawing.Size(566, 10)
        Me.pbCargoBay.StartColor = System.Drawing.Color.LimeGreen
        Me.pbCargoBay.TabIndex = 32
        Me.pbCargoBay.Value = 50
        '
        'lblCargoBay
        '
        Me.lblCargoBay.AutoSize = True
        Me.lblCargoBay.Location = New System.Drawing.Point(3, 5)
        Me.lblCargoBay.Name = "lblCargoBay"
        Me.lblCargoBay.Size = New System.Drawing.Size(86, 13)
        Me.lblCargoBay.TabIndex = 3
        Me.lblCargoBay.Text = "0.00 / 000.00 m³"
        '
        'btnMergeCargo
        '
        Me.btnMergeCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMergeCargo.Location = New System.Drawing.Point(575, 8)
        Me.btnMergeCargo.Name = "btnMergeCargo"
        Me.btnMergeCargo.Size = New System.Drawing.Size(90, 23)
        Me.btnMergeCargo.TabIndex = 6
        Me.btnMergeCargo.Text = "Merge Cargo"
        Me.btnMergeCargo.UseVisualStyleBackColor = True
        '
        'lvwCargoBay
        '
        Me.lvwCargoBay.AllowDrop = True
        Me.lvwCargoBay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwCargoBay.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colCargoBayType, Me.colCargoBayQty})
        Me.lvwCargoBay.ContextMenuStrip = Me.ctxBays
        Me.lvwCargoBay.FullRowSelect = True
        Me.lvwCargoBay.GridLines = True
        Me.lvwCargoBay.Location = New System.Drawing.Point(3, 35)
        Me.lvwCargoBay.Name = "lvwCargoBay"
        Me.lvwCargoBay.Size = New System.Drawing.Size(662, 178)
        Me.lvwCargoBay.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwCargoBay.TabIndex = 5
        Me.lvwCargoBay.UseCompatibleStateImageBehavior = False
        Me.lvwCargoBay.View = System.Windows.Forms.View.Details
        '
        'colCargoBayType
        '
        Me.colCargoBayType.Text = "Item Type"
        Me.colCargoBayType.Width = 225
        '
        'colCargoBayQty
        '
        Me.colCargoBayQty.Text = "Qty"
        Me.colCargoBayQty.Width = 35
        '
        'tabRemote
        '
        Me.tabRemote.Controls.Add(Me.panelRemote)
        Me.tabRemote.Location = New System.Drawing.Point(4, 22)
        Me.tabRemote.Name = "tabRemote"
        Me.tabRemote.Size = New System.Drawing.Size(668, 216)
        Me.tabRemote.TabIndex = 2
        Me.tabRemote.Text = "Remote Effects"
        Me.tabRemote.UseVisualStyleBackColor = True
        '
        'panelRemote
        '
        Me.panelRemote.BackColor = System.Drawing.SystemColors.Control
        Me.panelRemote.Controls.Add(Me.cboFitting)
        Me.panelRemote.Controls.Add(Me.btnAddRemoteFitting)
        Me.panelRemote.Controls.Add(Me.lblFitting)
        Me.panelRemote.Controls.Add(Me.lvwRemoteFittings)
        Me.panelRemote.Controls.Add(Me.lblPilot)
        Me.panelRemote.Controls.Add(Me.lvwRemoteEffects)
        Me.panelRemote.Controls.Add(Me.cboPilot)
        Me.panelRemote.Controls.Add(Me.btnUpdateRemoteEffects)
        Me.panelRemote.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelRemote.Location = New System.Drawing.Point(0, 0)
        Me.panelRemote.Name = "panelRemote"
        Me.panelRemote.Size = New System.Drawing.Size(668, 216)
        Me.panelRemote.TabIndex = 9
        '
        'cboFitting
        '
        Me.cboFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFitting.FormattingEnabled = True
        Me.cboFitting.Location = New System.Drawing.Point(55, 9)
        Me.cboFitting.Name = "cboFitting"
        Me.cboFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboFitting.TabIndex = 1
        '
        'btnAddRemoteFitting
        '
        Me.btnAddRemoteFitting.Location = New System.Drawing.Point(438, 7)
        Me.btnAddRemoteFitting.Name = "btnAddRemoteFitting"
        Me.btnAddRemoteFitting.Size = New System.Drawing.Size(75, 23)
        Me.btnAddRemoteFitting.TabIndex = 8
        Me.btnAddRemoteFitting.Text = "Add"
        Me.btnAddRemoteFitting.UseVisualStyleBackColor = True
        '
        'lblFitting
        '
        Me.lblFitting.AutoSize = True
        Me.lblFitting.Location = New System.Drawing.Point(11, 12)
        Me.lblFitting.Name = "lblFitting"
        Me.lblFitting.Size = New System.Drawing.Size(38, 13)
        Me.lblFitting.TabIndex = 0
        Me.lblFitting.Text = "Fitting:"
        '
        'lvwRemoteFittings
        '
        Me.lvwRemoteFittings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwRemoteFittings.CheckBoxes = True
        Me.lvwRemoteFittings.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lvwRemoteFittings.ContextMenuStrip = Me.ctxRemoteFittings
        Me.lvwRemoteFittings.FullRowSelect = True
        Me.lvwRemoteFittings.GridLines = True
        Me.lvwRemoteFittings.Location = New System.Drawing.Point(6, 36)
        Me.lvwRemoteFittings.Name = "lvwRemoteFittings"
        Me.lvwRemoteFittings.Size = New System.Drawing.Size(319, 177)
        Me.lvwRemoteFittings.TabIndex = 7
        Me.lvwRemoteFittings.UseCompatibleStateImageBehavior = False
        Me.lvwRemoteFittings.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Remote Fittings"
        Me.ColumnHeader1.Width = 280
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(248, 12)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(30, 13)
        Me.lblPilot.TabIndex = 2
        Me.lblPilot.Text = "Pilot:"
        '
        'lvwRemoteEffects
        '
        Me.lvwRemoteEffects.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwRemoteEffects.CheckBoxes = True
        Me.lvwRemoteEffects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colModule})
        Me.lvwRemoteEffects.ContextMenuStrip = Me.ctxRemoteModule
        Me.lvwRemoteEffects.FullRowSelect = True
        Me.lvwRemoteEffects.GridLines = True
        Me.lvwRemoteEffects.Location = New System.Drawing.Point(331, 36)
        Me.lvwRemoteEffects.Name = "lvwRemoteEffects"
        Me.lvwRemoteEffects.Size = New System.Drawing.Size(334, 177)
        Me.lvwRemoteEffects.TabIndex = 5
        Me.lvwRemoteEffects.UseCompatibleStateImageBehavior = False
        Me.lvwRemoteEffects.View = System.Windows.Forms.View.Details
        '
        'colModule
        '
        Me.colModule.Text = "Remote Modules"
        Me.colModule.Width = 300
        '
        'cboPilot
        '
        Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilot.FormattingEnabled = True
        Me.cboPilot.Location = New System.Drawing.Point(284, 9)
        Me.cboPilot.Name = "cboPilot"
        Me.cboPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboPilot.TabIndex = 3
        '
        'btnUpdateRemoteEffects
        '
        Me.btnUpdateRemoteEffects.Location = New System.Drawing.Point(519, 7)
        Me.btnUpdateRemoteEffects.Name = "btnUpdateRemoteEffects"
        Me.btnUpdateRemoteEffects.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdateRemoteEffects.TabIndex = 4
        Me.btnUpdateRemoteEffects.Text = "Update"
        Me.btnUpdateRemoteEffects.UseVisualStyleBackColor = True
        Me.btnUpdateRemoteEffects.Visible = False
        '
        'tabFleet
        '
        Me.tabFleet.Controls.Add(Me.panelFleet)
        Me.tabFleet.Location = New System.Drawing.Point(4, 22)
        Me.tabFleet.Name = "tabFleet"
        Me.tabFleet.Size = New System.Drawing.Size(668, 216)
        Me.tabFleet.TabIndex = 3
        Me.tabFleet.Text = "Fleet Effects"
        Me.tabFleet.UseVisualStyleBackColor = True
        '
        'panelFleet
        '
        Me.panelFleet.BackColor = System.Drawing.SystemColors.Control
        Me.panelFleet.Controls.Add(Me.chkFCActive)
        Me.panelFleet.Controls.Add(Me.chkWCActive)
        Me.panelFleet.Controls.Add(Me.chkSCActive)
        Me.panelFleet.Controls.Add(Me.cboSCShip)
        Me.panelFleet.Controls.Add(Me.cboFCPilot)
        Me.panelFleet.Controls.Add(Me.cboWCPilot)
        Me.panelFleet.Controls.Add(Me.lblSCShip)
        Me.panelFleet.Controls.Add(Me.lblWC)
        Me.panelFleet.Controls.Add(Me.btnLeaveFleet)
        Me.panelFleet.Controls.Add(Me.lblFC)
        Me.panelFleet.Controls.Add(Me.lblFCShip)
        Me.panelFleet.Controls.Add(Me.lblSC)
        Me.panelFleet.Controls.Add(Me.cboSCPilot)
        Me.panelFleet.Controls.Add(Me.lblFleetStatus)
        Me.panelFleet.Controls.Add(Me.lblWCShip)
        Me.panelFleet.Controls.Add(Me.lblFleetStatusLabel)
        Me.panelFleet.Controls.Add(Me.cboWCShip)
        Me.panelFleet.Controls.Add(Me.cboFCShip)
        Me.panelFleet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelFleet.Location = New System.Drawing.Point(0, 0)
        Me.panelFleet.Name = "panelFleet"
        Me.panelFleet.Size = New System.Drawing.Size(668, 216)
        Me.panelFleet.TabIndex = 25
        '
        'chkFCActive
        '
        Me.chkFCActive.AutoSize = True
        Me.chkFCActive.Checked = True
        Me.chkFCActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFCActive.Location = New System.Drawing.Point(480, 68)
        Me.chkFCActive.Name = "chkFCActive"
        Me.chkFCActive.Size = New System.Drawing.Size(62, 17)
        Me.chkFCActive.TabIndex = 26
        Me.chkFCActive.Text = "Active?"
        Me.chkFCActive.UseVisualStyleBackColor = True
        '
        'chkWCActive
        '
        Me.chkWCActive.AutoSize = True
        Me.chkWCActive.Checked = True
        Me.chkWCActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWCActive.Location = New System.Drawing.Point(480, 41)
        Me.chkWCActive.Name = "chkWCActive"
        Me.chkWCActive.Size = New System.Drawing.Size(62, 17)
        Me.chkWCActive.TabIndex = 25
        Me.chkWCActive.Text = "Active?"
        Me.chkWCActive.UseVisualStyleBackColor = True
        '
        'chkSCActive
        '
        Me.chkSCActive.AutoSize = True
        Me.chkSCActive.Checked = True
        Me.chkSCActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSCActive.Location = New System.Drawing.Point(480, 14)
        Me.chkSCActive.Name = "chkSCActive"
        Me.chkSCActive.Size = New System.Drawing.Size(62, 17)
        Me.chkSCActive.TabIndex = 24
        Me.chkSCActive.Text = "Active?"
        Me.chkSCActive.UseVisualStyleBackColor = True
        '
        'cboSCShip
        '
        Me.cboSCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSCShip.Enabled = False
        Me.cboSCShip.FormattingEnabled = True
        Me.cboSCShip.Location = New System.Drawing.Point(287, 12)
        Me.cboSCShip.Name = "cboSCShip"
        Me.cboSCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboSCShip.TabIndex = 8
        '
        'cboFCPilot
        '
        Me.cboFCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFCPilot.FormattingEnabled = True
        Me.cboFCPilot.Location = New System.Drawing.Point(96, 66)
        Me.cboFCPilot.Name = "cboFCPilot"
        Me.cboFCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboFCPilot.TabIndex = 16
        '
        'cboWCPilot
        '
        Me.cboWCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWCPilot.FormattingEnabled = True
        Me.cboWCPilot.Location = New System.Drawing.Point(96, 39)
        Me.cboWCPilot.Name = "cboWCPilot"
        Me.cboWCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboWCPilot.TabIndex = 15
        '
        'lblSCShip
        '
        Me.lblSCShip.AutoSize = True
        Me.lblSCShip.Location = New System.Drawing.Point(250, 15)
        Me.lblSCShip.Name = "lblSCShip"
        Me.lblSCShip.Size = New System.Drawing.Size(31, 13)
        Me.lblSCShip.TabIndex = 9
        Me.lblSCShip.Text = "Ship:"
        '
        'lblWC
        '
        Me.lblWC.AutoSize = True
        Me.lblWC.Location = New System.Drawing.Point(10, 42)
        Me.lblWC.Name = "lblWC"
        Me.lblWC.Size = New System.Drawing.Size(74, 13)
        Me.lblWC.TabIndex = 14
        Me.lblWC.Text = "Wing Booster:"
        '
        'btnLeaveFleet
        '
        Me.btnLeaveFleet.Enabled = False
        Me.btnLeaveFleet.Location = New System.Drawing.Point(189, 93)
        Me.btnLeaveFleet.Name = "btnLeaveFleet"
        Me.btnLeaveFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnLeaveFleet.TabIndex = 23
        Me.btnLeaveFleet.Text = "Leave Fleet"
        Me.btnLeaveFleet.UseVisualStyleBackColor = True
        '
        'lblFC
        '
        Me.lblFC.AutoSize = True
        Me.lblFC.Location = New System.Drawing.Point(10, 69)
        Me.lblFC.Name = "lblFC"
        Me.lblFC.Size = New System.Drawing.Size(72, 13)
        Me.lblFC.TabIndex = 13
        Me.lblFC.Text = "Fleet Booster:"
        '
        'lblFCShip
        '
        Me.lblFCShip.AutoSize = True
        Me.lblFCShip.Location = New System.Drawing.Point(250, 69)
        Me.lblFCShip.Name = "lblFCShip"
        Me.lblFCShip.Size = New System.Drawing.Size(31, 13)
        Me.lblFCShip.TabIndex = 17
        Me.lblFCShip.Text = "Ship:"
        '
        'lblSC
        '
        Me.lblSC.AutoSize = True
        Me.lblSC.Location = New System.Drawing.Point(10, 15)
        Me.lblSC.Name = "lblSC"
        Me.lblSC.Size = New System.Drawing.Size(80, 13)
        Me.lblSC.TabIndex = 12
        Me.lblSC.Text = "Squad Booster:"
        '
        'cboSCPilot
        '
        Me.cboSCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSCPilot.FormattingEnabled = True
        Me.cboSCPilot.Location = New System.Drawing.Point(96, 12)
        Me.cboSCPilot.Name = "cboSCPilot"
        Me.cboSCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboSCPilot.TabIndex = 10
        '
        'lblFleetStatus
        '
        Me.lblFleetStatus.AutoSize = True
        Me.lblFleetStatus.Location = New System.Drawing.Point(82, 98)
        Me.lblFleetStatus.Name = "lblFleetStatus"
        Me.lblFleetStatus.Size = New System.Drawing.Size(45, 13)
        Me.lblFleetStatus.TabIndex = 22
        Me.lblFleetStatus.Text = "Inactive"
        '
        'lblWCShip
        '
        Me.lblWCShip.AutoSize = True
        Me.lblWCShip.Location = New System.Drawing.Point(250, 42)
        Me.lblWCShip.Name = "lblWCShip"
        Me.lblWCShip.Size = New System.Drawing.Size(31, 13)
        Me.lblWCShip.TabIndex = 18
        Me.lblWCShip.Text = "Ship:"
        '
        'lblFleetStatusLabel
        '
        Me.lblFleetStatusLabel.AutoSize = True
        Me.lblFleetStatusLabel.Location = New System.Drawing.Point(10, 98)
        Me.lblFleetStatusLabel.Name = "lblFleetStatusLabel"
        Me.lblFleetStatusLabel.Size = New System.Drawing.Size(66, 13)
        Me.lblFleetStatusLabel.TabIndex = 21
        Me.lblFleetStatusLabel.Text = "Fleet Status:"
        '
        'cboWCShip
        '
        Me.cboWCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWCShip.Enabled = False
        Me.cboWCShip.FormattingEnabled = True
        Me.cboWCShip.Location = New System.Drawing.Point(287, 39)
        Me.cboWCShip.Name = "cboWCShip"
        Me.cboWCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboWCShip.TabIndex = 19
        '
        'cboFCShip
        '
        Me.cboFCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFCShip.Enabled = False
        Me.cboFCShip.FormattingEnabled = True
        Me.cboFCShip.Location = New System.Drawing.Point(287, 66)
        Me.cboFCShip.Name = "cboFCShip"
        Me.cboFCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboFCShip.TabIndex = 20
        '
        'lvwSlots
        '
        Me.lvwSlots.AllowDrop = True
        Me.lvwSlots.ContextMenuStrip = Me.ctxSlots
        Me.lvwSlots.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwSlots.FullRowSelect = True
        ListViewGroup1.Header = "High Slots"
        ListViewGroup1.Name = "lvwgHighSlots"
        ListViewGroup2.Header = "Mid Slots"
        ListViewGroup2.Name = "lvwgMidSlots"
        ListViewGroup3.Header = "Low Slots"
        ListViewGroup3.Name = "lvwgLowSlots"
        ListViewGroup4.Header = "Rig Slots"
        ListViewGroup4.Name = "lvwgRigSlots"
        ListViewGroup5.Header = "Subsystem Slots"
        ListViewGroup5.Name = "lvwgSubSlots"
        Me.lvwSlots.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4, ListViewGroup5})
        Me.lvwSlots.Location = New System.Drawing.Point(0, 0)
        Me.lvwSlots.Name = "lvwSlots"
        Me.lvwSlots.Size = New System.Drawing.Size(676, 275)
        Me.lvwSlots.SmallImageList = Me.imgState
        Me.lvwSlots.TabIndex = 0
        Me.lvwSlots.Tag = ""
        Me.lvwSlots.UseCompatibleStateImageBehavior = False
        Me.lvwSlots.View = System.Windows.Forms.View.Details
        '
        'ShipSlotControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.panelFunctions)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "ShipSlotControl"
        Me.Size = New System.Drawing.Size(676, 563)
        Me.ctxSlots.ResumeLayout(False)
        Me.ctxBays.ResumeLayout(False)
        Me.ctxRemoteFittings.ResumeLayout(False)
        Me.ctxRemoteModule.ResumeLayout(False)
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxShipSkills.ResumeLayout(False)
        Me.panelFunctions.ResumeLayout(False)
        Me.panelFunctions.PerformLayout()
        CType(Me.pbShipInfo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.tabStorage.ResumeLayout(False)
        Me.tabDroneBay.ResumeLayout(False)
        Me.panelDrone.ResumeLayout(False)
        Me.panelDrone.PerformLayout()
        Me.tabCargoBay.ResumeLayout(False)
        Me.panelCargo.ResumeLayout(False)
        Me.panelCargo.PerformLayout()
        Me.tabRemote.ResumeLayout(False)
        Me.panelRemote.ResumeLayout(False)
        Me.panelRemote.PerformLayout()
        Me.tabFleet.ResumeLayout(False)
        Me.panelFleet.ResumeLayout(False)
        Me.panelFleet.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvwSlots As EveHQ.HQF.ListViewNoFlicker
    Friend WithEvents lblMidSlots As System.Windows.Forms.Label
    Friend WithEvents lblHighSlots As System.Windows.Forms.Label
    Friend WithEvents lblRigSlots As System.Windows.Forms.Label
    Friend WithEvents lblLowSlots As System.Windows.Forms.Label
    Friend WithEvents lblTurretSlots As System.Windows.Forms.Label
    Friend WithEvents lblLauncherSlots As System.Windows.Forms.Label
    Friend WithEvents lblFittingMarketPrice As System.Windows.Forms.Label
    Friend WithEvents lblShipMarketPrice As System.Windows.Forms.Label
    Friend WithEvents ctxSlots As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ShowInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tabStorage As System.Windows.Forms.TabControl
    Friend WithEvents tabDroneBay As System.Windows.Forms.TabPage
    Friend WithEvents tabCargoBay As System.Windows.Forms.TabPage
    Friend WithEvents lblDroneBay As System.Windows.Forms.Label
    Friend WithEvents lvwDroneBay As System.Windows.Forms.ListView
    Friend WithEvents lblCargoBay As System.Windows.Forms.Label
    Friend WithEvents colDroneBayType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDroneBayQty As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwCargoBay As System.Windows.Forms.ListView
    Friend WithEvents colCargoBayType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCargoBayQty As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ctxBays As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ctxRemoveItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxAlterQuantity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxSplitBatch As System.Windows.Forms.ToolStripMenuItem

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SplitContainer1.Panel2Collapsed = True
        rigGroups.Add(773)
        rigGroups.Add(782)
        rigGroups.Add(778)
        rigGroups.Add(780)
        rigGroups.Add(786)
        rigGroups.Add(781)
        rigGroups.Add(775)
        rigGroups.Add(776)
        rigGroups.Add(779)
        rigGroups.Add(904)
        rigGroups.Add(777)
        rigGroups.Add(896)
        rigGroups.Add(774)
        remoteGroups.Add(41)
        remoteGroups.Add(325)
        remoteGroups.Add(585)
        remoteGroups.Add(67)
        remoteGroups.Add(65)
        remoteGroups.Add(68)
        remoteGroups.Add(71)
        remoteGroups.Add(291)
        remoteGroups.Add(209)
        remoteGroups.Add(289)
        remoteGroups.Add(290)
        remoteGroups.Add(208)
        remoteGroups.Add(379)
        remoteGroups.Add(544)
        remoteGroups.Add(641)
        remoteGroups.Add(640)
        remoteGroups.Add(639)
        fleetGroups.Add(316)
        fleetSkills.Add("Armored Warfare")
        fleetSkills.Add("Information Warfare")
        fleetSkills.Add("Leadership")
        fleetSkills.Add("Mining Foreman")
        fleetSkills.Add("Siege Warfare")
        fleetSkills.Add("Skirmish Warfare")

        ' Load the remote and fleet info
        Call LoadRemoteFleetInfo()
    End Sub
    Friend WithEvents imgState As System.Windows.Forms.ImageList
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxShowBayInfoItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents panelFunctions As System.Windows.Forms.Panel
    Friend WithEvents pbShipInfo As System.Windows.Forms.PictureBox
    Friend WithEvents btnToggleStorage As System.Windows.Forms.Button
    Friend WithEvents btnMergeDrones As System.Windows.Forms.Button
    Friend WithEvents btnMergeCargo As System.Windows.Forms.Button
    Friend WithEvents tabRemote As System.Windows.Forms.TabPage
    Friend WithEvents lvwRemoteEffects As System.Windows.Forms.ListView
    Friend WithEvents btnUpdateRemoteEffects As System.Windows.Forms.Button
    Friend WithEvents cboPilot As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents cboFitting As System.Windows.Forms.ComboBox
    Friend WithEvents lblFitting As System.Windows.Forms.Label
    Friend WithEvents colModule As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxRemoteModule As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuShowRemoteModInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnAddRemoteFitting As System.Windows.Forms.Button
    Friend WithEvents lvwRemoteFittings As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents pbDroneBay As VistaStyleProgressBar.ProgressBar
    Friend WithEvents pbCargoBay As VistaStyleProgressBar.ProgressBar
    Friend WithEvents ctxRemoteFittings As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveFittingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabFleet As System.Windows.Forms.TabPage
    Friend WithEvents lblWC As System.Windows.Forms.Label
    Friend WithEvents lblFC As System.Windows.Forms.Label
    Friend WithEvents lblSC As System.Windows.Forms.Label
    Friend WithEvents cboSCPilot As System.Windows.Forms.ComboBox
    Friend WithEvents lblSCShip As System.Windows.Forms.Label
    Friend WithEvents cboSCShip As System.Windows.Forms.ComboBox
    Friend WithEvents cboFCPilot As System.Windows.Forms.ComboBox
    Friend WithEvents cboWCPilot As System.Windows.Forms.ComboBox
    Friend WithEvents lblWCShip As System.Windows.Forms.Label
    Friend WithEvents lblFCShip As System.Windows.Forms.Label
    Friend WithEvents cboFCShip As System.Windows.Forms.ComboBox
    Friend WithEvents cboWCShip As System.Windows.Forms.ComboBox
    Friend WithEvents lblFleetStatus As System.Windows.Forms.Label
    Friend WithEvents lblFleetStatusLabel As System.Windows.Forms.Label
    Friend WithEvents btnLeaveFleet As System.Windows.Forms.Button
    Friend WithEvents panelFleet As System.Windows.Forms.Panel
    Friend WithEvents panelRemote As System.Windows.Forms.Panel
    Friend WithEvents panelDrone As System.Windows.Forms.Panel
    Friend WithEvents panelCargo As System.Windows.Forms.Panel
    Friend WithEvents chkFCActive As System.Windows.Forms.CheckBox
    Friend WithEvents chkWCActive As System.Windows.Forms.CheckBox
    Friend WithEvents chkSCActive As System.Windows.Forms.CheckBox
    Friend WithEvents lblSubSlots As System.Windows.Forms.Label
    Friend WithEvents pbShip As System.Windows.Forms.PictureBox
    Friend WithEvents ctxShipSkills As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAlterRelevantSkills As System.Windows.Forms.ToolStripMenuItem
End Class
