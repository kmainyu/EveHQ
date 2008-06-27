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
        Me.panelSlotInfo = New System.Windows.Forms.Panel
        Me.btnClipboardCopy = New System.Windows.Forms.Button
        Me.btnToggleStorage = New System.Windows.Forms.Button
        Me.lblFittingMarketPrice = New System.Windows.Forms.Label
        Me.lblFittingBasePrice = New System.Windows.Forms.Label
        Me.lblShipMarketPrice = New System.Windows.Forms.Label
        Me.lblShipBasePrice = New System.Windows.Forms.Label
        Me.lblTurretSlots = New System.Windows.Forms.Label
        Me.lblLauncherSlots = New System.Windows.Forms.Label
        Me.lblRigSlots = New System.Windows.Forms.Label
        Me.lblLowSlots = New System.Windows.Forms.Label
        Me.lblMidSlots = New System.Windows.Forms.Label
        Me.lblHighSlots = New System.Windows.Forms.Label
        Me.ctxSlots = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.imgState = New System.Windows.Forms.ImageList(Me.components)
        Me.tabStorage = New System.Windows.Forms.TabControl
        Me.tabDroneBay = New System.Windows.Forms.TabPage
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
        Me.pbDroneBay = New System.Windows.Forms.ProgressBar
        Me.lblDroneBay = New System.Windows.Forms.Label
        Me.tabCargoBay = New System.Windows.Forms.TabPage
        Me.lvwCargoBay = New System.Windows.Forms.ListView
        Me.colCargoBayType = New System.Windows.Forms.ColumnHeader
        Me.colCargoBayQty = New System.Windows.Forms.ColumnHeader
        Me.pbCargoBay = New System.Windows.Forms.ProgressBar
        Me.lblCargoBay = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lvwSlots = New EveHQ.HQF.ListViewNoFlicker
        Me.colModuleName = New System.Windows.Forms.ColumnHeader
        Me.colCharge = New System.Windows.Forms.ColumnHeader
        Me.colCPU = New System.Windows.Forms.ColumnHeader
        Me.colPG = New System.Windows.Forms.ColumnHeader
        Me.colActivationCost = New System.Windows.Forms.ColumnHeader
        Me.colActivationTime = New System.Windows.Forms.ColumnHeader
        Me.colMarketPrice = New System.Windows.Forms.ColumnHeader
        Me.panelSlotInfo.SuspendLayout()
        Me.ctxSlots.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabStorage.SuspendLayout()
        Me.tabDroneBay.SuspendLayout()
        Me.ctxBays.SuspendLayout()
        Me.tabCargoBay.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelSlotInfo
        '
        Me.panelSlotInfo.AutoScroll = True
        Me.panelSlotInfo.BackColor = System.Drawing.Color.Transparent
        Me.panelSlotInfo.Controls.Add(Me.btnClipboardCopy)
        Me.panelSlotInfo.Controls.Add(Me.btnToggleStorage)
        Me.panelSlotInfo.Controls.Add(Me.lblFittingMarketPrice)
        Me.panelSlotInfo.Controls.Add(Me.lblFittingBasePrice)
        Me.panelSlotInfo.Controls.Add(Me.lblShipMarketPrice)
        Me.panelSlotInfo.Controls.Add(Me.lblShipBasePrice)
        Me.panelSlotInfo.Controls.Add(Me.lblTurretSlots)
        Me.panelSlotInfo.Controls.Add(Me.lblLauncherSlots)
        Me.panelSlotInfo.Controls.Add(Me.lblRigSlots)
        Me.panelSlotInfo.Controls.Add(Me.lblLowSlots)
        Me.panelSlotInfo.Controls.Add(Me.lblMidSlots)
        Me.panelSlotInfo.Controls.Add(Me.lblHighSlots)
        Me.panelSlotInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelSlotInfo.Location = New System.Drawing.Point(0, 424)
        Me.panelSlotInfo.Name = "panelSlotInfo"
        Me.panelSlotInfo.Size = New System.Drawing.Size(888, 55)
        Me.panelSlotInfo.TabIndex = 0
        '
        'btnClipboardCopy
        '
        Me.btnClipboardCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClipboardCopy.Image = Global.EveHQ.HQF.My.Resources.Resources.Clipboard1
        Me.btnClipboardCopy.Location = New System.Drawing.Point(814, 18)
        Me.btnClipboardCopy.Name = "btnClipboardCopy"
        Me.btnClipboardCopy.Size = New System.Drawing.Size(32, 32)
        Me.btnClipboardCopy.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.btnClipboardCopy, "Copy To Clipboard")
        Me.btnClipboardCopy.UseVisualStyleBackColor = True
        '
        'btnToggleStorage
        '
        Me.btnToggleStorage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnToggleStorage.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCargo
        Me.btnToggleStorage.Location = New System.Drawing.Point(852, 18)
        Me.btnToggleStorage.Name = "btnToggleStorage"
        Me.btnToggleStorage.Size = New System.Drawing.Size(32, 32)
        Me.btnToggleStorage.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.btnToggleStorage, "Toggle Storage Bays")
        Me.btnToggleStorage.UseVisualStyleBackColor = True
        '
        'lblFittingMarketPrice
        '
        Me.lblFittingMarketPrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFittingMarketPrice.AutoSize = True
        Me.lblFittingMarketPrice.Location = New System.Drawing.Point(237, 37)
        Me.lblFittingMarketPrice.Name = "lblFittingMarketPrice"
        Me.lblFittingMarketPrice.Size = New System.Drawing.Size(208, 13)
        Me.lblFittingMarketPrice.TabIndex = 10
        Me.lblFittingMarketPrice.Text = "Fitting Market Price: 0,000,000,000.00 ISK"
        '
        'lblFittingBasePrice
        '
        Me.lblFittingBasePrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFittingBasePrice.AutoSize = True
        Me.lblFittingBasePrice.Location = New System.Drawing.Point(4, 37)
        Me.lblFittingBasePrice.Name = "lblFittingBasePrice"
        Me.lblFittingBasePrice.Size = New System.Drawing.Size(199, 13)
        Me.lblFittingBasePrice.TabIndex = 9
        Me.lblFittingBasePrice.Text = "Fitting Base Price: 0,000,000,000.00 ISK"
        '
        'lblShipMarketPrice
        '
        Me.lblShipMarketPrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblShipMarketPrice.AutoSize = True
        Me.lblShipMarketPrice.Location = New System.Drawing.Point(237, 23)
        Me.lblShipMarketPrice.Name = "lblShipMarketPrice"
        Me.lblShipMarketPrice.Size = New System.Drawing.Size(201, 13)
        Me.lblShipMarketPrice.TabIndex = 8
        Me.lblShipMarketPrice.Text = "Ship Market Price: 0,000,000,000.00 ISK"
        '
        'lblShipBasePrice
        '
        Me.lblShipBasePrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblShipBasePrice.AutoSize = True
        Me.lblShipBasePrice.Location = New System.Drawing.Point(4, 23)
        Me.lblShipBasePrice.Name = "lblShipBasePrice"
        Me.lblShipBasePrice.Size = New System.Drawing.Size(192, 13)
        Me.lblShipBasePrice.TabIndex = 7
        Me.lblShipBasePrice.Text = "Ship Base Price: 0,000,000,000.00 ISK"
        '
        'lblTurretSlots
        '
        Me.lblTurretSlots.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTurretSlots.AutoSize = True
        Me.lblTurretSlots.Location = New System.Drawing.Point(422, 5)
        Me.lblTurretSlots.Name = "lblTurretSlots"
        Me.lblTurretSlots.Size = New System.Drawing.Size(84, 13)
        Me.lblTurretSlots.TabIndex = 6
        Me.lblTurretSlots.Text = "Turret Slots: 0/0"
        '
        'lblLauncherSlots
        '
        Me.lblLauncherSlots.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLauncherSlots.AutoSize = True
        Me.lblLauncherSlots.Location = New System.Drawing.Point(315, 5)
        Me.lblLauncherSlots.Name = "lblLauncherSlots"
        Me.lblLauncherSlots.Size = New System.Drawing.Size(101, 13)
        Me.lblLauncherSlots.TabIndex = 5
        Me.lblLauncherSlots.Text = "Launcher Slots: 0/0"
        '
        'lblRigSlots
        '
        Me.lblRigSlots.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRigSlots.AutoSize = True
        Me.lblRigSlots.Location = New System.Drawing.Point(237, 5)
        Me.lblRigSlots.Name = "lblRigSlots"
        Me.lblRigSlots.Size = New System.Drawing.Size(72, 13)
        Me.lblRigSlots.TabIndex = 4
        Me.lblRigSlots.Text = "Rig Slots: 0/0"
        '
        'lblLowSlots
        '
        Me.lblLowSlots.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLowSlots.AutoSize = True
        Me.lblLowSlots.Location = New System.Drawing.Point(155, 5)
        Me.lblLowSlots.Name = "lblLowSlots"
        Me.lblLowSlots.Size = New System.Drawing.Size(76, 13)
        Me.lblLowSlots.TabIndex = 3
        Me.lblLowSlots.Text = "Low Slots: 0/0"
        '
        'lblMidSlots
        '
        Me.lblMidSlots.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMidSlots.AutoSize = True
        Me.lblMidSlots.Location = New System.Drawing.Point(76, 5)
        Me.lblMidSlots.Name = "lblMidSlots"
        Me.lblMidSlots.Size = New System.Drawing.Size(73, 13)
        Me.lblMidSlots.TabIndex = 2
        Me.lblMidSlots.Text = "Mid Slots: 0/0"
        '
        'lblHighSlots
        '
        Me.lblHighSlots.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblHighSlots.AutoSize = True
        Me.lblHighSlots.Location = New System.Drawing.Point(4, 5)
        Me.lblHighSlots.Name = "lblHighSlots"
        Me.lblHighSlots.Size = New System.Drawing.Size(66, 13)
        Me.lblHighSlots.TabIndex = 1
        Me.lblHighSlots.Text = "Hi Slots: 0/0"
        '
        'ctxSlots
        '
        Me.ctxSlots.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowInfoToolStripMenuItem})
        Me.ctxSlots.Name = "ctxSlots"
        Me.ctxSlots.Size = New System.Drawing.Size(135, 26)
        '
        'ShowInfoToolStripMenuItem
        '
        Me.ShowInfoToolStripMenuItem.Name = "ShowInfoToolStripMenuItem"
        Me.ShowInfoToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.ShowInfoToolStripMenuItem.Text = "Show Info"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lvwSlots)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.tabStorage)
        Me.SplitContainer1.Panel2MinSize = 10
        Me.SplitContainer1.Size = New System.Drawing.Size(888, 424)
        Me.SplitContainer1.SplitterDistance = 639
        Me.SplitContainer1.TabIndex = 1
        '
        'imgState
        '
        Me.imgState.ImageStream = CType(resources.GetObject("imgState.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgState.TransparentColor = System.Drawing.Color.Transparent
        Me.imgState.Images.SetKeyName(0, "Status_red.gif")
        Me.imgState.Images.SetKeyName(1, "Status_yellow.gif")
        Me.imgState.Images.SetKeyName(2, "Status_green.gif")
        Me.imgState.Images.SetKeyName(3, "icon22_10.png")
        '
        'tabStorage
        '
        Me.tabStorage.Controls.Add(Me.tabDroneBay)
        Me.tabStorage.Controls.Add(Me.tabCargoBay)
        Me.tabStorage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabStorage.Location = New System.Drawing.Point(0, 0)
        Me.tabStorage.Multiline = True
        Me.tabStorage.Name = "tabStorage"
        Me.tabStorage.SelectedIndex = 0
        Me.tabStorage.Size = New System.Drawing.Size(245, 424)
        Me.tabStorage.TabIndex = 0
        '
        'tabDroneBay
        '
        Me.tabDroneBay.Controls.Add(Me.lvwDroneBay)
        Me.tabDroneBay.Controls.Add(Me.pbDroneBay)
        Me.tabDroneBay.Controls.Add(Me.lblDroneBay)
        Me.tabDroneBay.Location = New System.Drawing.Point(4, 22)
        Me.tabDroneBay.Name = "tabDroneBay"
        Me.tabDroneBay.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDroneBay.Size = New System.Drawing.Size(237, 398)
        Me.tabDroneBay.TabIndex = 0
        Me.tabDroneBay.Text = "Drone Bay"
        Me.tabDroneBay.UseVisualStyleBackColor = True
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
        Me.lvwDroneBay.Size = New System.Drawing.Size(224, 359)
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
        Me.ctxBays.Size = New System.Drawing.Size(154, 104)
        '
        'ctxRemoveItem
        '
        Me.ctxRemoveItem.Name = "ctxRemoveItem"
        Me.ctxRemoveItem.Size = New System.Drawing.Size(153, 22)
        Me.ctxRemoveItem.Text = "Remove Item"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(150, 6)
        '
        'ctxAlterQuantity
        '
        Me.ctxAlterQuantity.Name = "ctxAlterQuantity"
        Me.ctxAlterQuantity.Size = New System.Drawing.Size(153, 22)
        Me.ctxAlterQuantity.Text = "Alter Quantity"
        '
        'ctxSplitBatch
        '
        Me.ctxSplitBatch.Name = "ctxSplitBatch"
        Me.ctxSplitBatch.Size = New System.Drawing.Size(153, 22)
        Me.ctxSplitBatch.Text = "Split Batch"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(150, 6)
        '
        'ctxShowBayInfoItem
        '
        Me.ctxShowBayInfoItem.Name = "ctxShowBayInfoItem"
        Me.ctxShowBayInfoItem.Size = New System.Drawing.Size(153, 22)
        Me.ctxShowBayInfoItem.Text = "ShowInfo"
        '
        'pbDroneBay
        '
        Me.pbDroneBay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbDroneBay.Location = New System.Drawing.Point(7, 16)
        Me.pbDroneBay.Name = "pbDroneBay"
        Me.pbDroneBay.Size = New System.Drawing.Size(224, 10)
        Me.pbDroneBay.TabIndex = 1
        '
        'lblDroneBay
        '
        Me.lblDroneBay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDroneBay.AutoSize = True
        Me.lblDroneBay.Location = New System.Drawing.Point(145, 3)
        Me.lblDroneBay.Name = "lblDroneBay"
        Me.lblDroneBay.Size = New System.Drawing.Size(86, 13)
        Me.lblDroneBay.TabIndex = 0
        Me.lblDroneBay.Text = "0.00 / 000.00 m³"
        '
        'tabCargoBay
        '
        Me.tabCargoBay.Controls.Add(Me.lvwCargoBay)
        Me.tabCargoBay.Controls.Add(Me.pbCargoBay)
        Me.tabCargoBay.Controls.Add(Me.lblCargoBay)
        Me.tabCargoBay.Location = New System.Drawing.Point(4, 22)
        Me.tabCargoBay.Name = "tabCargoBay"
        Me.tabCargoBay.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCargoBay.Size = New System.Drawing.Size(237, 398)
        Me.tabCargoBay.TabIndex = 1
        Me.tabCargoBay.Text = "CargoBay"
        Me.tabCargoBay.UseVisualStyleBackColor = True
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
        Me.lvwCargoBay.Size = New System.Drawing.Size(224, 359)
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
        'pbCargoBay
        '
        Me.pbCargoBay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbCargoBay.Location = New System.Drawing.Point(7, 16)
        Me.pbCargoBay.Name = "pbCargoBay"
        Me.pbCargoBay.Size = New System.Drawing.Size(224, 10)
        Me.pbCargoBay.TabIndex = 4
        '
        'lblCargoBay
        '
        Me.lblCargoBay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCargoBay.AutoSize = True
        Me.lblCargoBay.Location = New System.Drawing.Point(145, 3)
        Me.lblCargoBay.Name = "lblCargoBay"
        Me.lblCargoBay.Size = New System.Drawing.Size(86, 13)
        Me.lblCargoBay.TabIndex = 3
        Me.lblCargoBay.Text = "0.00 / 000.00 m³"
        '
        'lvwSlots
        '
        Me.lvwSlots.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colModuleName, Me.colCharge, Me.colCPU, Me.colPG, Me.colActivationCost, Me.colActivationTime, Me.colMarketPrice})
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
        Me.lvwSlots.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4})
        Me.lvwSlots.Location = New System.Drawing.Point(0, 0)
        Me.lvwSlots.Name = "lvwSlots"
        Me.lvwSlots.Size = New System.Drawing.Size(639, 424)
        Me.lvwSlots.SmallImageList = Me.imgState
        Me.lvwSlots.TabIndex = 0
        Me.lvwSlots.UseCompatibleStateImageBehavior = False
        Me.lvwSlots.View = System.Windows.Forms.View.Details
        '
        'colModuleName
        '
        Me.colModuleName.Text = "Module Name"
        Me.colModuleName.Width = 150
        '
        'colCharge
        '
        Me.colCharge.Text = "Charge"
        Me.colCharge.Width = 125
        '
        'colCPU
        '
        Me.colCPU.Text = "CPU"
        Me.colCPU.Width = 40
        '
        'colPG
        '
        Me.colPG.Text = "PG"
        Me.colPG.Width = 40
        '
        'colActivationCost
        '
        Me.colActivationCost.Text = "Cap Cost"
        '
        'colActivationTime
        '
        Me.colActivationTime.Text = "Cap Time"
        '
        'colMarketPrice
        '
        Me.colMarketPrice.Text = "Market Price"
        Me.colMarketPrice.Width = 125
        '
        'ShipSlotControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.panelSlotInfo)
        Me.Name = "ShipSlotControl"
        Me.Size = New System.Drawing.Size(888, 479)
        Me.panelSlotInfo.ResumeLayout(False)
        Me.panelSlotInfo.PerformLayout()
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
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panelSlotInfo As System.Windows.Forms.Panel
    Friend WithEvents lvwSlots As EveHQ.HQF.ListViewNoFlicker
    Friend WithEvents colModuleName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCPU As System.Windows.Forms.ColumnHeader
    Friend WithEvents colPG As System.Windows.Forms.ColumnHeader
    Friend WithEvents colActivationCost As System.Windows.Forms.ColumnHeader
    Friend WithEvents colActivationTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblMidSlots As System.Windows.Forms.Label
    Friend WithEvents lblHighSlots As System.Windows.Forms.Label
    Friend WithEvents lblRigSlots As System.Windows.Forms.Label
    Friend WithEvents lblLowSlots As System.Windows.Forms.Label
    Friend WithEvents lblTurretSlots As System.Windows.Forms.Label
    Friend WithEvents lblLauncherSlots As System.Windows.Forms.Label
    Friend WithEvents lblShipBasePrice As System.Windows.Forms.Label
    Friend WithEvents lblFittingMarketPrice As System.Windows.Forms.Label
    Friend WithEvents lblFittingBasePrice As System.Windows.Forms.Label
    Friend WithEvents lblShipMarketPrice As System.Windows.Forms.Label
    Friend WithEvents colMarketPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxSlots As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ShowInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tabStorage As System.Windows.Forms.TabControl
    Friend WithEvents tabDroneBay As System.Windows.Forms.TabPage
    Friend WithEvents tabCargoBay As System.Windows.Forms.TabPage
    Friend WithEvents lblDroneBay As System.Windows.Forms.Label
    Friend WithEvents pbDroneBay As System.Windows.Forms.ProgressBar
    Friend WithEvents lvwDroneBay As System.Windows.Forms.ListView
    Friend WithEvents pbCargoBay As System.Windows.Forms.ProgressBar
    Friend WithEvents lblCargoBay As System.Windows.Forms.Label
    Friend WithEvents colDroneBayType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDroneBayQty As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwCargoBay As System.Windows.Forms.ListView
    Friend WithEvents colCargoBayType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCargoBayQty As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnToggleStorage As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents colCharge As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxBays As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ctxRemoveItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxAlterQuantity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxSplitBatch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnClipboardCopy As System.Windows.Forms.Button

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
    End Sub
    Friend WithEvents imgState As System.Windows.Forms.ImageList
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxShowBayInfoItem As System.Windows.Forms.ToolStripMenuItem
End Class
