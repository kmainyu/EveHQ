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
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("High Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup6 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mid Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup7 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Low Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup8 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Rig Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShipSlotControl))
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.lvwSlots = New EveHQ.HQF.ListViewNoFlicker
        Me.imgState = New System.Windows.Forms.ImageList(Me.components)
        Me.tabStorage = New System.Windows.Forms.TabControl
        Me.tabDroneBay = New System.Windows.Forms.TabPage
        Me.pbDroneBay = New VistaStyleProgressBar.ProgressBar
        Me.btnMergeDrones = New System.Windows.Forms.Button
        Me.lvwDroneBay = New System.Windows.Forms.ListView
        Me.colDroneBayType = New System.Windows.Forms.ColumnHeader
        Me.colDroneBayQty = New System.Windows.Forms.ColumnHeader
        Me.ctxBays = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ctxRemoveItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.ctxAlterQuantity = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxSplitBatch = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.ctxShowBayInfoItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblDroneBay = New System.Windows.Forms.Label
        Me.tabCargoBay = New System.Windows.Forms.TabPage
        Me.pbCargoBay = New VistaStyleProgressBar.ProgressBar
        Me.btnMergeCargo = New System.Windows.Forms.Button
        Me.lvwCargoBay = New System.Windows.Forms.ListView
        Me.colCargoBayType = New System.Windows.Forms.ColumnHeader
        Me.colCargoBayQty = New System.Windows.Forms.ColumnHeader
        Me.lblCargoBay = New System.Windows.Forms.Label
        Me.tabRemote = New System.Windows.Forms.TabPage
        Me.btnAddRemoteFitting = New System.Windows.Forms.Button
        Me.lvwRemoteFittings = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ctxRemoteFittings = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveFittingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lvwRemoteEffects = New System.Windows.Forms.ListView
        Me.colModule = New System.Windows.Forms.ColumnHeader
        Me.ctxRemoteModule = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuShowRemoteModInfo = New System.Windows.Forms.ToolStripMenuItem
        Me.btnUpdateRemoteEffects = New System.Windows.Forms.Button
        Me.cboPilot = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.cboFitting = New System.Windows.Forms.ComboBox
        Me.lblFitting = New System.Windows.Forms.Label
        Me.tabFleet = New System.Windows.Forms.TabPage
        Me.btnLeaveFleet = New System.Windows.Forms.Button
        Me.lblFleetStatus = New System.Windows.Forms.Label
        Me.lblFleetStatusLabel = New System.Windows.Forms.Label
        Me.cboFCShip = New System.Windows.Forms.ComboBox
        Me.cboWCShip = New System.Windows.Forms.ComboBox
        Me.lblWCShip = New System.Windows.Forms.Label
        Me.lblFCShip = New System.Windows.Forms.Label
        Me.cboFCPilot = New System.Windows.Forms.ComboBox
        Me.cboWCPilot = New System.Windows.Forms.ComboBox
        Me.lblWC = New System.Windows.Forms.Label
        Me.lblFC = New System.Windows.Forms.Label
        Me.lblSC = New System.Windows.Forms.Label
        Me.cboSCPilot = New System.Windows.Forms.ComboBox
        Me.lblSCShip = New System.Windows.Forms.Label
        Me.cboSCShip = New System.Windows.Forms.ComboBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnToggleStorage = New System.Windows.Forms.Button
        Me.panelFunctions = New System.Windows.Forms.Panel
        Me.pbShipInfo = New System.Windows.Forms.PictureBox
        Me.lblFleetData = New System.Windows.Forms.Label
        Me.ctxSlots.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabStorage.SuspendLayout()
        Me.tabDroneBay.SuspendLayout()
        Me.ctxBays.SuspendLayout()
        Me.tabCargoBay.SuspendLayout()
        Me.tabRemote.SuspendLayout()
        Me.ctxRemoteFittings.SuspendLayout()
        Me.ctxRemoteModule.SuspendLayout()
        Me.tabFleet.SuspendLayout()
        Me.panelFunctions.SuspendLayout()
        CType(Me.pbShipInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblFittingMarketPrice
        '
        Me.lblFittingMarketPrice.AutoSize = True
        Me.lblFittingMarketPrice.Location = New System.Drawing.Point(78, 18)
        Me.lblFittingMarketPrice.Name = "lblFittingMarketPrice"
        Me.lblFittingMarketPrice.Size = New System.Drawing.Size(208, 13)
        Me.lblFittingMarketPrice.TabIndex = 10
        Me.lblFittingMarketPrice.Text = "Fitting Market Price: 0,000,000,000.00 ISK"
        '
        'lblShipMarketPrice
        '
        Me.lblShipMarketPrice.AutoSize = True
        Me.lblShipMarketPrice.Location = New System.Drawing.Point(292, 18)
        Me.lblShipMarketPrice.Name = "lblShipMarketPrice"
        Me.lblShipMarketPrice.Size = New System.Drawing.Size(201, 13)
        Me.lblShipMarketPrice.TabIndex = 8
        Me.lblShipMarketPrice.Text = "Ship Market Price: 0,000,000,000.00 ISK"
        '
        'lblTurretSlots
        '
        Me.lblTurretSlots.AutoSize = True
        Me.lblTurretSlots.Location = New System.Drawing.Point(371, 5)
        Me.lblTurretSlots.Name = "lblTurretSlots"
        Me.lblTurretSlots.Size = New System.Drawing.Size(63, 13)
        Me.lblTurretSlots.TabIndex = 6
        Me.lblTurretSlots.Text = "Turrets: 0/0"
        '
        'lblLauncherSlots
        '
        Me.lblLauncherSlots.AutoSize = True
        Me.lblLauncherSlots.Location = New System.Drawing.Point(285, 5)
        Me.lblLauncherSlots.Name = "lblLauncherSlots"
        Me.lblLauncherSlots.Size = New System.Drawing.Size(80, 13)
        Me.lblLauncherSlots.TabIndex = 5
        Me.lblLauncherSlots.Text = "Launchers: 0/0"
        '
        'lblRigSlots
        '
        Me.lblRigSlots.AutoSize = True
        Me.lblRigSlots.Location = New System.Drawing.Point(233, 5)
        Me.lblRigSlots.Name = "lblRigSlots"
        Me.lblRigSlots.Size = New System.Drawing.Size(46, 13)
        Me.lblRigSlots.TabIndex = 4
        Me.lblRigSlots.Text = "Rig: 0/0"
        '
        'lblLowSlots
        '
        Me.lblLowSlots.AutoSize = True
        Me.lblLowSlots.Location = New System.Drawing.Point(177, 5)
        Me.lblLowSlots.Name = "lblLowSlots"
        Me.lblLowSlots.Size = New System.Drawing.Size(50, 13)
        Me.lblLowSlots.TabIndex = 3
        Me.lblLowSlots.Text = "Low: 0/0"
        '
        'lblMidSlots
        '
        Me.lblMidSlots.AutoSize = True
        Me.lblMidSlots.Location = New System.Drawing.Point(124, 5)
        Me.lblMidSlots.Name = "lblMidSlots"
        Me.lblMidSlots.Size = New System.Drawing.Size(47, 13)
        Me.lblMidSlots.TabIndex = 2
        Me.lblMidSlots.Text = "Mid: 0/0"
        '
        'lblHighSlots
        '
        Me.lblHighSlots.AutoSize = True
        Me.lblHighSlots.Location = New System.Drawing.Point(78, 5)
        Me.lblHighSlots.Name = "lblHighSlots"
        Me.lblHighSlots.Size = New System.Drawing.Size(40, 13)
        Me.lblHighSlots.TabIndex = 1
        Me.lblHighSlots.Text = "Hi: 0/0"
        '
        'ctxSlots
        '
        Me.ctxSlots.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowInfoToolStripMenuItem})
        Me.ctxSlots.Name = "ctxSlots"
        Me.ctxSlots.Size = New System.Drawing.Size(135, 26)
        Me.ctxSlots.Tag = " "
        '
        'ShowInfoToolStripMenuItem
        '
        Me.ShowInfoToolStripMenuItem.Name = "ShowInfoToolStripMenuItem"
        Me.ShowInfoToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.ShowInfoToolStripMenuItem.Text = "Show Info"
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
        'lvwSlots
        '
        Me.lvwSlots.AllowDrop = True
        Me.lvwSlots.ContextMenuStrip = Me.ctxSlots
        Me.lvwSlots.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwSlots.FullRowSelect = True
        ListViewGroup5.Header = "High Slots"
        ListViewGroup5.Name = "lvwgHighSlots"
        ListViewGroup6.Header = "Mid Slots"
        ListViewGroup6.Name = "lvwgMidSlots"
        ListViewGroup7.Header = "Low Slots"
        ListViewGroup7.Name = "lvwgLowSlots"
        ListViewGroup8.Header = "Rig Slots"
        ListViewGroup8.Name = "lvwgRigSlots"
        Me.lvwSlots.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup5, ListViewGroup6, ListViewGroup7, ListViewGroup8})
        Me.lvwSlots.Location = New System.Drawing.Point(0, 0)
        Me.lvwSlots.Name = "lvwSlots"
        Me.lvwSlots.Size = New System.Drawing.Size(676, 275)
        Me.lvwSlots.SmallImageList = Me.imgState
        Me.lvwSlots.TabIndex = 0
        Me.lvwSlots.Tag = ""
        Me.lvwSlots.UseCompatibleStateImageBehavior = False
        Me.lvwSlots.View = System.Windows.Forms.View.Details
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
        Me.tabDroneBay.Controls.Add(Me.pbDroneBay)
        Me.tabDroneBay.Controls.Add(Me.btnMergeDrones)
        Me.tabDroneBay.Controls.Add(Me.lvwDroneBay)
        Me.tabDroneBay.Controls.Add(Me.lblDroneBay)
        Me.tabDroneBay.Location = New System.Drawing.Point(4, 22)
        Me.tabDroneBay.Name = "tabDroneBay"
        Me.tabDroneBay.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDroneBay.Size = New System.Drawing.Size(668, 216)
        Me.tabDroneBay.TabIndex = 0
        Me.tabDroneBay.Text = "Drone Bay"
        Me.tabDroneBay.UseVisualStyleBackColor = True
        '
        'pbDroneBay
        '
        Me.pbDroneBay.BackColor = System.Drawing.Color.Transparent
        Me.pbDroneBay.EndColor = System.Drawing.Color.LimeGreen
        Me.pbDroneBay.GlowColor = System.Drawing.Color.LightGreen
        Me.pbDroneBay.Location = New System.Drawing.Point(7, 19)
        Me.pbDroneBay.Name = "pbDroneBay"
        Me.pbDroneBay.Size = New System.Drawing.Size(560, 10)
        Me.pbDroneBay.StartColor = System.Drawing.Color.LimeGreen
        Me.pbDroneBay.TabIndex = 31
        Me.pbDroneBay.Value = 50
        '
        'btnMergeDrones
        '
        Me.btnMergeDrones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMergeDrones.Location = New System.Drawing.Point(573, 6)
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
        Me.lvwDroneBay.Location = New System.Drawing.Point(7, 33)
        Me.lvwDroneBay.Name = "lvwDroneBay"
        Me.lvwDroneBay.Size = New System.Drawing.Size(655, 177)
        Me.lvwDroneBay.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwDroneBay.TabIndex = 2
        Me.lvwDroneBay.UseCompatibleStateImageBehavior = False
        Me.lvwDroneBay.View = System.Windows.Forms.View.Details
        '
        'colDroneBayType
        '
        Me.colDroneBayType.Text = "Drone Type"
        Me.colDroneBayType.Width = 165
        '
        'colDroneBayQty
        '
        Me.colDroneBayQty.Text = "Qty"
        Me.colDroneBayQty.Width = 35
        '
        'ctxBays
        '
        Me.ctxBays.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxRemoveItem, Me.ToolStripMenuItem1, Me.ctxAlterQuantity, Me.ctxSplitBatch, Me.ToolStripMenuItem2, Me.ctxShowBayInfoItem})
        Me.ctxBays.Name = "ctx"
        Me.ctxBays.Size = New System.Drawing.Size(167, 104)
        '
        'ctxRemoveItem
        '
        Me.ctxRemoveItem.Name = "ctxRemoveItem"
        Me.ctxRemoveItem.Size = New System.Drawing.Size(166, 22)
        Me.ctxRemoveItem.Text = "Remove Item"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(163, 6)
        '
        'ctxAlterQuantity
        '
        Me.ctxAlterQuantity.Name = "ctxAlterQuantity"
        Me.ctxAlterQuantity.Size = New System.Drawing.Size(166, 22)
        Me.ctxAlterQuantity.Text = "Alter Quantity"
        '
        'ctxSplitBatch
        '
        Me.ctxSplitBatch.Name = "ctxSplitBatch"
        Me.ctxSplitBatch.Size = New System.Drawing.Size(166, 22)
        Me.ctxSplitBatch.Text = "Split Batch"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(163, 6)
        '
        'ctxShowBayInfoItem
        '
        Me.ctxShowBayInfoItem.Name = "ctxShowBayInfoItem"
        Me.ctxShowBayInfoItem.Size = New System.Drawing.Size(166, 22)
        Me.ctxShowBayInfoItem.Text = "Show Drone Info"
        '
        'lblDroneBay
        '
        Me.lblDroneBay.AutoSize = True
        Me.lblDroneBay.Location = New System.Drawing.Point(6, 3)
        Me.lblDroneBay.Name = "lblDroneBay"
        Me.lblDroneBay.Size = New System.Drawing.Size(86, 13)
        Me.lblDroneBay.TabIndex = 0
        Me.lblDroneBay.Text = "0.00 / 000.00 m³"
        '
        'tabCargoBay
        '
        Me.tabCargoBay.Controls.Add(Me.pbCargoBay)
        Me.tabCargoBay.Controls.Add(Me.btnMergeCargo)
        Me.tabCargoBay.Controls.Add(Me.lvwCargoBay)
        Me.tabCargoBay.Controls.Add(Me.lblCargoBay)
        Me.tabCargoBay.Location = New System.Drawing.Point(4, 22)
        Me.tabCargoBay.Name = "tabCargoBay"
        Me.tabCargoBay.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCargoBay.Size = New System.Drawing.Size(668, 216)
        Me.tabCargoBay.TabIndex = 1
        Me.tabCargoBay.Text = "CargoBay"
        Me.tabCargoBay.UseVisualStyleBackColor = True
        '
        'pbCargoBay
        '
        Me.pbCargoBay.BackColor = System.Drawing.Color.Transparent
        Me.pbCargoBay.EndColor = System.Drawing.Color.LimeGreen
        Me.pbCargoBay.GlowColor = System.Drawing.Color.LightGreen
        Me.pbCargoBay.Location = New System.Drawing.Point(7, 19)
        Me.pbCargoBay.Name = "pbCargoBay"
        Me.pbCargoBay.Size = New System.Drawing.Size(559, 10)
        Me.pbCargoBay.StartColor = System.Drawing.Color.LimeGreen
        Me.pbCargoBay.TabIndex = 32
        Me.pbCargoBay.Value = 50
        '
        'btnMergeCargo
        '
        Me.btnMergeCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMergeCargo.Location = New System.Drawing.Point(572, 6)
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
        Me.lvwCargoBay.Location = New System.Drawing.Point(7, 33)
        Me.lvwCargoBay.Name = "lvwCargoBay"
        Me.lvwCargoBay.Size = New System.Drawing.Size(655, 175)
        Me.lvwCargoBay.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwCargoBay.TabIndex = 5
        Me.lvwCargoBay.UseCompatibleStateImageBehavior = False
        Me.lvwCargoBay.View = System.Windows.Forms.View.Details
        '
        'colCargoBayType
        '
        Me.colCargoBayType.Text = "Item Type"
        Me.colCargoBayType.Width = 165
        '
        'colCargoBayQty
        '
        Me.colCargoBayQty.Text = "Qty"
        Me.colCargoBayQty.Width = 35
        '
        'lblCargoBay
        '
        Me.lblCargoBay.AutoSize = True
        Me.lblCargoBay.Location = New System.Drawing.Point(6, 3)
        Me.lblCargoBay.Name = "lblCargoBay"
        Me.lblCargoBay.Size = New System.Drawing.Size(86, 13)
        Me.lblCargoBay.TabIndex = 3
        Me.lblCargoBay.Text = "0.00 / 000.00 m³"
        '
        'tabRemote
        '
        Me.tabRemote.Controls.Add(Me.btnAddRemoteFitting)
        Me.tabRemote.Controls.Add(Me.lvwRemoteFittings)
        Me.tabRemote.Controls.Add(Me.lvwRemoteEffects)
        Me.tabRemote.Controls.Add(Me.btnUpdateRemoteEffects)
        Me.tabRemote.Controls.Add(Me.cboPilot)
        Me.tabRemote.Controls.Add(Me.lblPilot)
        Me.tabRemote.Controls.Add(Me.cboFitting)
        Me.tabRemote.Controls.Add(Me.lblFitting)
        Me.tabRemote.Location = New System.Drawing.Point(4, 22)
        Me.tabRemote.Name = "tabRemote"
        Me.tabRemote.Size = New System.Drawing.Size(668, 216)
        Me.tabRemote.TabIndex = 2
        Me.tabRemote.Text = "Remote Effects"
        Me.tabRemote.UseVisualStyleBackColor = True
        '
        'btnAddRemoteFitting
        '
        Me.btnAddRemoteFitting.Location = New System.Drawing.Point(435, 5)
        Me.btnAddRemoteFitting.Name = "btnAddRemoteFitting"
        Me.btnAddRemoteFitting.Size = New System.Drawing.Size(75, 23)
        Me.btnAddRemoteFitting.TabIndex = 8
        Me.btnAddRemoteFitting.Text = "Add"
        Me.btnAddRemoteFitting.UseVisualStyleBackColor = True
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
        Me.lvwRemoteFittings.Location = New System.Drawing.Point(3, 34)
        Me.lvwRemoteFittings.Name = "lvwRemoteFittings"
        Me.lvwRemoteFittings.Size = New System.Drawing.Size(319, 179)
        Me.lvwRemoteFittings.TabIndex = 7
        Me.lvwRemoteFittings.UseCompatibleStateImageBehavior = False
        Me.lvwRemoteFittings.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Remote Fittings"
        Me.ColumnHeader1.Width = 280
        '
        'ctxRemoteFittings
        '
        Me.ctxRemoteFittings.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveFittingToolStripMenuItem})
        Me.ctxRemoteFittings.Name = "ctxRemoteFittings"
        Me.ctxRemoteFittings.Size = New System.Drawing.Size(180, 26)
        '
        'RemoveFittingToolStripMenuItem
        '
        Me.RemoveFittingToolStripMenuItem.Name = "RemoveFittingToolStripMenuItem"
        Me.RemoveFittingToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.RemoveFittingToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.RemoveFittingToolStripMenuItem.Text = "Remove Fitting"
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
        Me.lvwRemoteEffects.Location = New System.Drawing.Point(328, 34)
        Me.lvwRemoteEffects.Name = "lvwRemoteEffects"
        Me.lvwRemoteEffects.Size = New System.Drawing.Size(337, 179)
        Me.lvwRemoteEffects.TabIndex = 5
        Me.lvwRemoteEffects.UseCompatibleStateImageBehavior = False
        Me.lvwRemoteEffects.View = System.Windows.Forms.View.Details
        '
        'colModule
        '
        Me.colModule.Text = "Remote Modules"
        Me.colModule.Width = 300
        '
        'ctxRemoteModule
        '
        Me.ctxRemoteModule.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowRemoteModInfo})
        Me.ctxRemoteModule.Name = "ctxRemoteModule"
        Me.ctxRemoteModule.Size = New System.Drawing.Size(135, 26)
        '
        'mnuShowRemoteModInfo
        '
        Me.mnuShowRemoteModInfo.Name = "mnuShowRemoteModInfo"
        Me.mnuShowRemoteModInfo.Size = New System.Drawing.Size(134, 22)
        Me.mnuShowRemoteModInfo.Text = "Show Info"
        '
        'btnUpdateRemoteEffects
        '
        Me.btnUpdateRemoteEffects.Location = New System.Drawing.Point(516, 5)
        Me.btnUpdateRemoteEffects.Name = "btnUpdateRemoteEffects"
        Me.btnUpdateRemoteEffects.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdateRemoteEffects.TabIndex = 4
        Me.btnUpdateRemoteEffects.Text = "Update"
        Me.btnUpdateRemoteEffects.UseVisualStyleBackColor = True
        '
        'cboPilot
        '
        Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilot.FormattingEnabled = True
        Me.cboPilot.Location = New System.Drawing.Point(281, 7)
        Me.cboPilot.Name = "cboPilot"
        Me.cboPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboPilot.TabIndex = 3
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(245, 10)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(30, 13)
        Me.lblPilot.TabIndex = 2
        Me.lblPilot.Text = "Pilot:"
        '
        'cboFitting
        '
        Me.cboFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFitting.FormattingEnabled = True
        Me.cboFitting.Location = New System.Drawing.Point(52, 7)
        Me.cboFitting.Name = "cboFitting"
        Me.cboFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboFitting.TabIndex = 1
        '
        'lblFitting
        '
        Me.lblFitting.AutoSize = True
        Me.lblFitting.Location = New System.Drawing.Point(8, 10)
        Me.lblFitting.Name = "lblFitting"
        Me.lblFitting.Size = New System.Drawing.Size(38, 13)
        Me.lblFitting.TabIndex = 0
        Me.lblFitting.Text = "Fitting:"
        '
        'tabFleet
        '
        Me.tabFleet.Controls.Add(Me.lblFleetData)
        Me.tabFleet.Controls.Add(Me.btnLeaveFleet)
        Me.tabFleet.Controls.Add(Me.lblFleetStatus)
        Me.tabFleet.Controls.Add(Me.lblFleetStatusLabel)
        Me.tabFleet.Controls.Add(Me.cboFCShip)
        Me.tabFleet.Controls.Add(Me.cboWCShip)
        Me.tabFleet.Controls.Add(Me.lblWCShip)
        Me.tabFleet.Controls.Add(Me.lblFCShip)
        Me.tabFleet.Controls.Add(Me.cboFCPilot)
        Me.tabFleet.Controls.Add(Me.cboWCPilot)
        Me.tabFleet.Controls.Add(Me.lblWC)
        Me.tabFleet.Controls.Add(Me.lblFC)
        Me.tabFleet.Controls.Add(Me.lblSC)
        Me.tabFleet.Controls.Add(Me.cboSCPilot)
        Me.tabFleet.Controls.Add(Me.lblSCShip)
        Me.tabFleet.Controls.Add(Me.cboSCShip)
        Me.tabFleet.Location = New System.Drawing.Point(4, 22)
        Me.tabFleet.Name = "tabFleet"
        Me.tabFleet.Size = New System.Drawing.Size(668, 216)
        Me.tabFleet.TabIndex = 3
        Me.tabFleet.Text = "Fleet Effects"
        Me.tabFleet.UseVisualStyleBackColor = True
        '
        'btnLeaveFleet
        '
        Me.btnLeaveFleet.Enabled = False
        Me.btnLeaveFleet.Location = New System.Drawing.Point(188, 102)
        Me.btnLeaveFleet.Name = "btnLeaveFleet"
        Me.btnLeaveFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnLeaveFleet.TabIndex = 23
        Me.btnLeaveFleet.Text = "Leave Fleet"
        Me.btnLeaveFleet.UseVisualStyleBackColor = True
        '
        'lblFleetStatus
        '
        Me.lblFleetStatus.AutoSize = True
        Me.lblFleetStatus.Location = New System.Drawing.Point(81, 107)
        Me.lblFleetStatus.Name = "lblFleetStatus"
        Me.lblFleetStatus.Size = New System.Drawing.Size(45, 13)
        Me.lblFleetStatus.TabIndex = 22
        Me.lblFleetStatus.Text = "Inactive"
        '
        'lblFleetStatusLabel
        '
        Me.lblFleetStatusLabel.AutoSize = True
        Me.lblFleetStatusLabel.Location = New System.Drawing.Point(9, 107)
        Me.lblFleetStatusLabel.Name = "lblFleetStatusLabel"
        Me.lblFleetStatusLabel.Size = New System.Drawing.Size(66, 13)
        Me.lblFleetStatusLabel.TabIndex = 21
        Me.lblFleetStatusLabel.Text = "Fleet Status:"
        '
        'cboFCShip
        '
        Me.cboFCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFCShip.Enabled = False
        Me.cboFCShip.FormattingEnabled = True
        Me.cboFCShip.Location = New System.Drawing.Point(319, 68)
        Me.cboFCShip.Name = "cboFCShip"
        Me.cboFCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboFCShip.TabIndex = 20
        '
        'cboWCShip
        '
        Me.cboWCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWCShip.Enabled = False
        Me.cboWCShip.FormattingEnabled = True
        Me.cboWCShip.Location = New System.Drawing.Point(319, 41)
        Me.cboWCShip.Name = "cboWCShip"
        Me.cboWCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboWCShip.TabIndex = 19
        '
        'lblWCShip
        '
        Me.lblWCShip.AutoSize = True
        Me.lblWCShip.Location = New System.Drawing.Point(269, 44)
        Me.lblWCShip.Name = "lblWCShip"
        Me.lblWCShip.Size = New System.Drawing.Size(44, 13)
        Me.lblWCShip.TabIndex = 18
        Me.lblWCShip.Text = "Piloting:"
        '
        'lblFCShip
        '
        Me.lblFCShip.AutoSize = True
        Me.lblFCShip.Location = New System.Drawing.Point(269, 71)
        Me.lblFCShip.Name = "lblFCShip"
        Me.lblFCShip.Size = New System.Drawing.Size(44, 13)
        Me.lblFCShip.TabIndex = 17
        Me.lblFCShip.Text = "Piloting:"
        '
        'cboFCPilot
        '
        Me.cboFCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFCPilot.FormattingEnabled = True
        Me.cboFCPilot.Location = New System.Drawing.Point(115, 68)
        Me.cboFCPilot.Name = "cboFCPilot"
        Me.cboFCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboFCPilot.TabIndex = 16
        '
        'cboWCPilot
        '
        Me.cboWCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWCPilot.FormattingEnabled = True
        Me.cboWCPilot.Location = New System.Drawing.Point(115, 41)
        Me.cboWCPilot.Name = "cboWCPilot"
        Me.cboWCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboWCPilot.TabIndex = 15
        '
        'lblWC
        '
        Me.lblWC.AutoSize = True
        Me.lblWC.Location = New System.Drawing.Point(9, 44)
        Me.lblWC.Name = "lblWC"
        Me.lblWC.Size = New System.Drawing.Size(94, 13)
        Me.lblWC.TabIndex = 14
        Me.lblWC.Text = "Wing Commander:"
        '
        'lblFC
        '
        Me.lblFC.AutoSize = True
        Me.lblFC.Location = New System.Drawing.Point(9, 71)
        Me.lblFC.Name = "lblFC"
        Me.lblFC.Size = New System.Drawing.Size(92, 13)
        Me.lblFC.TabIndex = 13
        Me.lblFC.Text = "Fleet Commander:"
        '
        'lblSC
        '
        Me.lblSC.AutoSize = True
        Me.lblSC.Location = New System.Drawing.Point(9, 17)
        Me.lblSC.Name = "lblSC"
        Me.lblSC.Size = New System.Drawing.Size(100, 13)
        Me.lblSC.TabIndex = 12
        Me.lblSC.Text = "Squad Commander:"
        '
        'cboSCPilot
        '
        Me.cboSCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSCPilot.FormattingEnabled = True
        Me.cboSCPilot.Location = New System.Drawing.Point(115, 14)
        Me.cboSCPilot.Name = "cboSCPilot"
        Me.cboSCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboSCPilot.TabIndex = 10
        '
        'lblSCShip
        '
        Me.lblSCShip.AutoSize = True
        Me.lblSCShip.Location = New System.Drawing.Point(269, 17)
        Me.lblSCShip.Name = "lblSCShip"
        Me.lblSCShip.Size = New System.Drawing.Size(44, 13)
        Me.lblSCShip.TabIndex = 9
        Me.lblSCShip.Text = "Piloting:"
        '
        'cboSCShip
        '
        Me.cboSCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSCShip.Enabled = False
        Me.cboSCShip.FormattingEnabled = True
        Me.cboSCShip.Location = New System.Drawing.Point(319, 14)
        Me.cboSCShip.Name = "cboSCShip"
        Me.cboSCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboSCShip.TabIndex = 8
        '
        'btnToggleStorage
        '
        Me.btnToggleStorage.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCargo
        Me.btnToggleStorage.Location = New System.Drawing.Point(40, 2)
        Me.btnToggleStorage.Name = "btnToggleStorage"
        Me.btnToggleStorage.Size = New System.Drawing.Size(32, 32)
        Me.btnToggleStorage.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.btnToggleStorage, "Toggle Storage Bays")
        Me.btnToggleStorage.UseVisualStyleBackColor = True
        '
        'panelFunctions
        '
        Me.panelFunctions.BackColor = System.Drawing.Color.Transparent
        Me.panelFunctions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
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
        'pbShipInfo
        '
        Me.pbShipInfo.Image = Global.EveHQ.HQF.My.Resources.Resources.imgInfo1
        Me.pbShipInfo.Location = New System.Drawing.Point(2, 2)
        Me.pbShipInfo.Name = "pbShipInfo"
        Me.pbShipInfo.Size = New System.Drawing.Size(32, 32)
        Me.pbShipInfo.TabIndex = 0
        Me.pbShipInfo.TabStop = False
        '
        'lblFleetData
        '
        Me.lblFleetData.AutoSize = True
        Me.lblFleetData.Location = New System.Drawing.Point(9, 133)
        Me.lblFleetData.Name = "lblFleetData"
        Me.lblFleetData.Size = New System.Drawing.Size(59, 13)
        Me.lblFleetData.TabIndex = 24
        Me.lblFleetData.Text = "Fleet Data:"
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
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.tabStorage.ResumeLayout(False)
        Me.tabDroneBay.ResumeLayout(False)
        Me.tabDroneBay.PerformLayout()
        Me.ctxBays.ResumeLayout(False)
        Me.tabCargoBay.ResumeLayout(False)
        Me.tabCargoBay.PerformLayout()
        Me.tabRemote.ResumeLayout(False)
        Me.tabRemote.PerformLayout()
        Me.ctxRemoteFittings.ResumeLayout(False)
        Me.ctxRemoteModule.ResumeLayout(False)
        Me.tabFleet.ResumeLayout(False)
        Me.tabFleet.PerformLayout()
        Me.panelFunctions.ResumeLayout(False)
        Me.panelFunctions.PerformLayout()
        CType(Me.pbShipInfo, System.ComponentModel.ISupportInitialize).EndInit()
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
        remoteGroups.Add(290)
        remoteGroups.Add(208)
        remoteGroups.Add(379)
        remoteGroups.Add(544)
        remoteGroups.Add(641)
        remoteGroups.Add(640)
        remoteGroups.Add(639)
        fleetGroups.Add("Armored Warfare")
        fleetGroups.Add("Information Warfare")
        fleetGroups.Add("Leadership")
        fleetGroups.Add("Mining Foreman")
        fleetGroups.Add("Siege Warfare")
        fleetGroups.Add("Skirmish Warfare")

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
    Friend WithEvents lblFleetData As System.Windows.Forms.Label
End Class
