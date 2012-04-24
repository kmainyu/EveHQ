Imports System.Windows.Forms

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShipSlotControl))
        Me.lblFittingMarketPrice = New System.Windows.Forms.Label()
        Me.lblShipMarketPrice = New System.Windows.Forms.Label()
        Me.ctxSlots = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxBays = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ctxRemoveItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ctxAlterQuantity = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxSplitBatch = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ctxShowBayInfoItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxRemoteFittings = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveFittingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxRemoteModule = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuShowRemoteModInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pbShip = New System.Windows.Forms.PictureBox()
        Me.ctxShipSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAlterRelevantSkills = New System.Windows.Forms.ToolStripMenuItem()
        Me.pbShipInfo = New System.Windows.Forms.PictureBox()
        Me.pbDroneBay = New DevComponents.DotNetBar.Controls.ProgressBarX()
        Me.lblDroneBay = New System.Windows.Forms.Label()
        Me.lvwDroneBay = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colDroneBayType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDroneBayQty = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.pbCargoBay = New DevComponents.DotNetBar.Controls.ProgressBarX()
        Me.lblCargoBay = New System.Windows.Forms.Label()
        Me.lvwCargoBay = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colCargoBayType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCargoBayQty = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblFitting = New System.Windows.Forms.Label()
        Me.lvwRemoteFittings = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblPilot = New System.Windows.Forms.Label()
        Me.lvwRemoteEffects = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colModule = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chkFCActive = New System.Windows.Forms.CheckBox()
        Me.chkWCActive = New System.Windows.Forms.CheckBox()
        Me.chkSCActive = New System.Windows.Forms.CheckBox()
        Me.lblSCShip = New System.Windows.Forms.Label()
        Me.lblWC = New System.Windows.Forms.Label()
        Me.lblFC = New System.Windows.Forms.Label()
        Me.lblFCShip = New System.Windows.Forms.Label()
        Me.lblSC = New System.Windows.Forms.Label()
        Me.lblFleetStatus = New System.Windows.Forms.Label()
        Me.lblWCShip = New System.Windows.Forms.Label()
        Me.lblFleetStatusLabel = New System.Windows.Forms.Label()
        Me.lblBoosterSlot3 = New System.Windows.Forms.Label()
        Me.lblBoosterSlot2 = New System.Windows.Forms.Label()
        Me.lblBoosterSlot1 = New System.Windows.Forms.Label()
        Me.lblWHClass = New System.Windows.Forms.Label()
        Me.lblWHEffect = New System.Windows.Forms.Label()
        Me.pbShipBay = New DevComponents.DotNetBar.Controls.ProgressBarX()
        Me.lblShipBay = New System.Windows.Forms.Label()
        Me.lvwShipBay = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colShipBayShip = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colShipBayQuantity = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colShipBayVolume = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colShipBayTotalVolume = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwHistory = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colTransaction = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colSlotType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOldSlotNo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOldModName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOldChargeName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colNewSlotNo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colNewModName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colNewChargeName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colChargeOnly = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ExpandableSplitter1 = New DevComponents.DotNetBar.ExpandableSplitter()
        Me.tcStorage = New DevComponents.DotNetBar.TabControl()
        Me.tcpDroneBay = New DevComponents.DotNetBar.TabControlPanel()
        Me.btnMergeDrones = New DevComponents.DotNetBar.ButtonX()
        Me.tiDroneBay = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tcpBoosters = New DevComponents.DotNetBar.TabControlPanel()
        Me.btnBoosterSlot3 = New DevComponents.DotNetBar.ButtonX()
        Me.btnShowInfo3 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnAlterSkills3 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRemoveBooster3 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnBoosterSlot2 = New DevComponents.DotNetBar.ButtonX()
        Me.btnShowInfo2 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnAlterSkills2 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRemoveBooster2 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnBoosterSlot1 = New DevComponents.DotNetBar.ButtonX()
        Me.btnShowInfo1 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnAlterSkills1 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnRemoveBooster1 = New DevComponents.DotNetBar.ButtonItem()
        Me.cboBoosterSlot2 = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboBoosterSlot3 = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboBoosterSlot1 = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.tiBoosters = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tcpFleetEffects = New DevComponents.DotNetBar.TabControlPanel()
        Me.cboFCShip = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboWCShip = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboSCShip = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboFCPilot = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboWCPilot = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboSCPilot = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.btnLeaveFleet = New DevComponents.DotNetBar.ButtonX()
        Me.tiFleetEffects = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tcpCargoBay = New DevComponents.DotNetBar.TabControlPanel()
        Me.btnMergeCargo = New DevComponents.DotNetBar.ButtonX()
        Me.tiCargoBay = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tcpRemoteEffects = New DevComponents.DotNetBar.TabControlPanel()
        Me.cboPilot = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.cboFitting = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.btnUpdateRemoteEffects = New DevComponents.DotNetBar.ButtonX()
        Me.btnAddRemoteFitting = New DevComponents.DotNetBar.ButtonX()
        Me.tiRemoteEffects = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tcpHistory = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiHistory = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tcpShipBay = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiShipBay = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tcpWHEffects = New DevComponents.DotNetBar.TabControlPanel()
        Me.cboWHClass = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem1 = New DevComponents.Editors.ComboItem()
        Me.ComboItem2 = New DevComponents.Editors.ComboItem()
        Me.ComboItem3 = New DevComponents.Editors.ComboItem()
        Me.ComboItem4 = New DevComponents.Editors.ComboItem()
        Me.ComboItem5 = New DevComponents.Editors.ComboItem()
        Me.ComboItem6 = New DevComponents.Editors.ComboItem()
        Me.cboWHEffect = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem7 = New DevComponents.Editors.ComboItem()
        Me.ComboItem8 = New DevComponents.Editors.ComboItem()
        Me.ComboItem9 = New DevComponents.Editors.ComboItem()
        Me.ComboItem10 = New DevComponents.Editors.ComboItem()
        Me.ComboItem11 = New DevComponents.Editors.ComboItem()
        Me.ComboItem12 = New DevComponents.Editors.ComboItem()
        Me.ComboItem13 = New DevComponents.Editors.ComboItem()
        Me.tiWHEffects = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.panelFunctions = New DevComponents.DotNetBar.PanelEx()
        Me.btnAutoSize = New DevComponents.DotNetBar.ButtonX()
        Me.btnRedo = New DevComponents.DotNetBar.ButtonX()
        Me.btnUndo = New DevComponents.DotNetBar.ButtonX()
        Me.adtSlots = New DevComponents.AdvTree.AdvTree()
        Me.colTestHeader1 = New DevComponents.AdvTree.ColumnHeader()
        Me.Node1 = New DevComponents.AdvTree.Node()
        Me.Node2 = New DevComponents.AdvTree.Node()
        Me.Cell2 = New DevComponents.AdvTree.Cell()
        Me.Cell3 = New DevComponents.AdvTree.Cell()
        Me.Cell4 = New DevComponents.AdvTree.Cell()
        Me.SlotStyle = New DevComponents.DotNetBar.ElementStyle()
        Me.HeaderStyle = New DevComponents.DotNetBar.ElementStyle()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.Cell7 = New DevComponents.AdvTree.Cell()
        Me.Cell8 = New DevComponents.AdvTree.Cell()
        Me.Cell9 = New DevComponents.AdvTree.Cell()
        Me.Cell13 = New DevComponents.AdvTree.Cell()
        Me.Cell14 = New DevComponents.AdvTree.Cell()
        Me.Cell15 = New DevComponents.AdvTree.Cell()
        Me.Cell1 = New DevComponents.AdvTree.Cell()
        Me.Cell5 = New DevComponents.AdvTree.Cell()
        Me.Cell6 = New DevComponents.AdvTree.Cell()
        Me.Cell10 = New DevComponents.AdvTree.Cell()
        Me.Cell11 = New DevComponents.AdvTree.Cell()
        Me.Cell12 = New DevComponents.AdvTree.Cell()
        Me.SlotTip = New DevComponents.DotNetBar.SuperTooltip()
        Me.ctxSlots.SuspendLayout()
        Me.ctxBays.SuspendLayout()
        Me.ctxRemoteFittings.SuspendLayout()
        Me.ctxRemoteModule.SuspendLayout()
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxShipSkills.SuspendLayout()
        CType(Me.pbShipInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tcStorage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcStorage.SuspendLayout()
        Me.tcpDroneBay.SuspendLayout()
        Me.tcpBoosters.SuspendLayout()
        Me.tcpFleetEffects.SuspendLayout()
        Me.tcpCargoBay.SuspendLayout()
        Me.tcpRemoteEffects.SuspendLayout()
        Me.tcpHistory.SuspendLayout()
        Me.tcpShipBay.SuspendLayout()
        Me.tcpWHEffects.SuspendLayout()
        Me.panelFunctions.SuspendLayout()
        CType(Me.adtSlots, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblFittingMarketPrice
        '
        Me.lblFittingMarketPrice.AutoSize = True
        Me.lblFittingMarketPrice.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFittingMarketPrice.Location = New System.Drawing.Point(76, 19)
        Me.lblFittingMarketPrice.Name = "lblFittingMarketPrice"
        Me.lblFittingMarketPrice.Size = New System.Drawing.Size(171, 13)
        Me.lblFittingMarketPrice.TabIndex = 10
        Me.lblFittingMarketPrice.Text = "Total Price: 0,000,000,000.00 ISK"
        '
        'lblShipMarketPrice
        '
        Me.lblShipMarketPrice.AutoSize = True
        Me.lblShipMarketPrice.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipMarketPrice.Location = New System.Drawing.Point(76, 6)
        Me.lblShipMarketPrice.Name = "lblShipMarketPrice"
        Me.lblShipMarketPrice.Size = New System.Drawing.Size(167, 13)
        Me.lblShipMarketPrice.TabIndex = 8
        Me.lblShipMarketPrice.Text = "Ship Price: 0,000,000,000.00 ISK"
        '
        'ctxSlots
        '
        Me.ctxSlots.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxSlots.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowInfoToolStripMenuItem})
        Me.ctxSlots.Name = "ctxSlots"
        Me.ctxSlots.Size = New System.Drawing.Size(124, 26)
        Me.ctxSlots.Tag = " "
        '
        'ShowInfoToolStripMenuItem
        '
        Me.ShowInfoToolStripMenuItem.Name = "ShowInfoToolStripMenuItem"
        Me.ShowInfoToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.ShowInfoToolStripMenuItem.Text = "Show Info"
        '
        'ctxBays
        '
        Me.ctxBays.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxBays.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctxRemoveItem, Me.ToolStripMenuItem1, Me.ctxAlterQuantity, Me.ctxSplitBatch, Me.ToolStripMenuItem2, Me.ctxShowBayInfoItem})
        Me.ctxBays.Name = "ctx"
        Me.ctxBays.Size = New System.Drawing.Size(156, 104)
        '
        'ctxRemoveItem
        '
        Me.ctxRemoveItem.Name = "ctxRemoveItem"
        Me.ctxRemoveItem.Size = New System.Drawing.Size(155, 22)
        Me.ctxRemoveItem.Text = "Remove Item"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(152, 6)
        '
        'ctxAlterQuantity
        '
        Me.ctxAlterQuantity.Name = "ctxAlterQuantity"
        Me.ctxAlterQuantity.Size = New System.Drawing.Size(155, 22)
        Me.ctxAlterQuantity.Text = "Alter Quantity"
        '
        'ctxSplitBatch
        '
        Me.ctxSplitBatch.Name = "ctxSplitBatch"
        Me.ctxSplitBatch.Size = New System.Drawing.Size(155, 22)
        Me.ctxSplitBatch.Text = "Split Batch"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(152, 6)
        '
        'ctxShowBayInfoItem
        '
        Me.ctxShowBayInfoItem.Name = "ctxShowBayInfoItem"
        Me.ctxShowBayInfoItem.Size = New System.Drawing.Size(155, 22)
        Me.ctxShowBayInfoItem.Text = "Show Drone Info"
        '
        'ctxRemoteFittings
        '
        Me.ctxRemoteFittings.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxRemoteFittings.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveFittingToolStripMenuItem})
        Me.ctxRemoteFittings.Name = "ctxRemoteFittings"
        Me.ctxRemoteFittings.Size = New System.Drawing.Size(169, 26)
        '
        'RemoveFittingToolStripMenuItem
        '
        Me.RemoveFittingToolStripMenuItem.Name = "RemoveFittingToolStripMenuItem"
        Me.RemoveFittingToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.RemoveFittingToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.RemoveFittingToolStripMenuItem.Text = "Remove Fitting"
        '
        'ctxRemoteModule
        '
        Me.ctxRemoteModule.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxRemoteModule.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowRemoteModInfo})
        Me.ctxRemoteModule.Name = "ctxRemoteModule"
        Me.ctxRemoteModule.Size = New System.Drawing.Size(124, 26)
        '
        'mnuShowRemoteModInfo
        '
        Me.mnuShowRemoteModInfo.Name = "mnuShowRemoteModInfo"
        Me.mnuShowRemoteModInfo.Size = New System.Drawing.Size(123, 22)
        Me.mnuShowRemoteModInfo.Text = "Show Info"
        '
        'ToolTip1
        '
        Me.ToolTip1.IsBalloon = True
        '
        'pbShip
        '
        Me.pbShip.ContextMenuStrip = Me.ctxShipSkills
        Me.pbShip.InitialImage = Global.EveHQ.HQF.My.Resources.Resources.imgInfo2
        Me.pbShip.Location = New System.Drawing.Point(3, 3)
        Me.pbShip.Name = "pbShip"
        Me.pbShip.Size = New System.Drawing.Size(32, 32)
        Me.pbShip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbShip.TabIndex = 13
        Me.pbShip.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShip, "Right-click to set relevant skills provided by bonuses on this ship")
        '
        'ctxShipSkills
        '
        Me.ctxShipSkills.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxShipSkills.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAlterRelevantSkills})
        Me.ctxShipSkills.Name = "ctxShipSkills"
        Me.ctxShipSkills.Size = New System.Drawing.Size(169, 26)
        '
        'mnuAlterRelevantSkills
        '
        Me.mnuAlterRelevantSkills.Name = "mnuAlterRelevantSkills"
        Me.mnuAlterRelevantSkills.Size = New System.Drawing.Size(168, 22)
        Me.mnuAlterRelevantSkills.Text = "Alter Relevant Skills"
        '
        'pbShipInfo
        '
        Me.pbShipInfo.ContextMenuStrip = Me.ctxShipSkills
        Me.pbShipInfo.Image = Global.EveHQ.HQF.My.Resources.Resources.imgInfo1
        Me.pbShipInfo.Location = New System.Drawing.Point(38, 3)
        Me.pbShipInfo.Name = "pbShipInfo"
        Me.pbShipInfo.Size = New System.Drawing.Size(32, 32)
        Me.pbShipInfo.TabIndex = 0
        Me.pbShipInfo.TabStop = False
        '
        'pbDroneBay
        '
        Me.pbDroneBay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.pbDroneBay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.pbDroneBay.Location = New System.Drawing.Point(4, 17)
        Me.pbDroneBay.Name = "pbDroneBay"
        Me.pbDroneBay.Size = New System.Drawing.Size(967, 10)
        Me.pbDroneBay.TabIndex = 4
        Me.pbDroneBay.Text = "ProgressBarX1"
        Me.pbDroneBay.Value = 50
        '
        'lblDroneBay
        '
        Me.lblDroneBay.AutoSize = True
        Me.lblDroneBay.BackColor = System.Drawing.Color.Transparent
        Me.lblDroneBay.Location = New System.Drawing.Point(4, 1)
        Me.lblDroneBay.Name = "lblDroneBay"
        Me.lblDroneBay.Size = New System.Drawing.Size(89, 13)
        Me.lblDroneBay.TabIndex = 0
        Me.lblDroneBay.Text = "0.00 / 000.00 m³"
        '
        'lvwDroneBay
        '
        Me.lvwDroneBay.AllowDrop = True
        Me.lvwDroneBay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lvwDroneBay.Border.Class = "ListViewBorder"
        Me.lvwDroneBay.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwDroneBay.CheckBoxes = True
        Me.lvwDroneBay.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colDroneBayType, Me.colDroneBayQty})
        Me.lvwDroneBay.ContextMenuStrip = Me.ctxBays
        Me.lvwDroneBay.FullRowSelect = True
        Me.lvwDroneBay.GridLines = True
        Me.lvwDroneBay.Location = New System.Drawing.Point(4, 31)
        Me.lvwDroneBay.Name = "lvwDroneBay"
        Me.lvwDroneBay.Size = New System.Drawing.Size(1053, 200)
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
        Me.colDroneBayQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colDroneBayQty.Width = 50
        '
        'pbCargoBay
        '
        Me.pbCargoBay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.pbCargoBay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.pbCargoBay.Location = New System.Drawing.Point(4, 17)
        Me.pbCargoBay.Name = "pbCargoBay"
        Me.pbCargoBay.Size = New System.Drawing.Size(967, 10)
        Me.pbCargoBay.TabIndex = 7
        Me.pbCargoBay.Text = "ProgressBarX1"
        Me.pbCargoBay.Value = 50
        '
        'lblCargoBay
        '
        Me.lblCargoBay.AutoSize = True
        Me.lblCargoBay.BackColor = System.Drawing.Color.Transparent
        Me.lblCargoBay.Location = New System.Drawing.Point(4, 1)
        Me.lblCargoBay.Name = "lblCargoBay"
        Me.lblCargoBay.Size = New System.Drawing.Size(89, 13)
        Me.lblCargoBay.TabIndex = 3
        Me.lblCargoBay.Text = "0.00 / 000.00 m³"
        '
        'lvwCargoBay
        '
        Me.lvwCargoBay.AllowDrop = True
        Me.lvwCargoBay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lvwCargoBay.Border.Class = "ListViewBorder"
        Me.lvwCargoBay.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwCargoBay.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colCargoBayType, Me.colCargoBayQty})
        Me.lvwCargoBay.ContextMenuStrip = Me.ctxBays
        Me.lvwCargoBay.FullRowSelect = True
        Me.lvwCargoBay.GridLines = True
        Me.lvwCargoBay.Location = New System.Drawing.Point(4, 31)
        Me.lvwCargoBay.Name = "lvwCargoBay"
        Me.lvwCargoBay.Size = New System.Drawing.Size(1053, 200)
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
        Me.colCargoBayQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colCargoBayQty.Width = 50
        '
        'lblFitting
        '
        Me.lblFitting.AutoSize = True
        Me.lblFitting.BackColor = System.Drawing.Color.Transparent
        Me.lblFitting.Location = New System.Drawing.Point(12, 7)
        Me.lblFitting.Name = "lblFitting"
        Me.lblFitting.Size = New System.Drawing.Size(41, 13)
        Me.lblFitting.TabIndex = 0
        Me.lblFitting.Text = "Fitting:"
        '
        'lvwRemoteFittings
        '
        Me.lvwRemoteFittings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lvwRemoteFittings.Border.Class = "ListViewBorder"
        Me.lvwRemoteFittings.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwRemoteFittings.CheckBoxes = True
        Me.lvwRemoteFittings.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lvwRemoteFittings.ContextMenuStrip = Me.ctxRemoteFittings
        Me.lvwRemoteFittings.FullRowSelect = True
        Me.lvwRemoteFittings.GridLines = True
        Me.lvwRemoteFittings.Location = New System.Drawing.Point(7, 31)
        Me.lvwRemoteFittings.Name = "lvwRemoteFittings"
        Me.lvwRemoteFittings.Size = New System.Drawing.Size(319, 200)
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
        Me.lblPilot.BackColor = System.Drawing.Color.Transparent
        Me.lblPilot.Location = New System.Drawing.Point(252, 7)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 2
        Me.lblPilot.Text = "Pilot:"
        '
        'lvwRemoteEffects
        '
        Me.lvwRemoteEffects.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lvwRemoteEffects.Border.Class = "ListViewBorder"
        Me.lvwRemoteEffects.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwRemoteEffects.CheckBoxes = True
        Me.lvwRemoteEffects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colModule})
        Me.lvwRemoteEffects.ContextMenuStrip = Me.ctxRemoteModule
        Me.lvwRemoteEffects.FullRowSelect = True
        Me.lvwRemoteEffects.GridLines = True
        Me.lvwRemoteEffects.Location = New System.Drawing.Point(332, 33)
        Me.lvwRemoteEffects.Name = "lvwRemoteEffects"
        Me.lvwRemoteEffects.Size = New System.Drawing.Size(725, 198)
        Me.lvwRemoteEffects.TabIndex = 5
        Me.lvwRemoteEffects.UseCompatibleStateImageBehavior = False
        Me.lvwRemoteEffects.View = System.Windows.Forms.View.Details
        '
        'colModule
        '
        Me.colModule.Text = "Remote Modules"
        Me.colModule.Width = 300
        '
        'chkFCActive
        '
        Me.chkFCActive.AutoSize = True
        Me.chkFCActive.BackColor = System.Drawing.Color.Transparent
        Me.chkFCActive.Checked = True
        Me.chkFCActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFCActive.Location = New System.Drawing.Point(483, 66)
        Me.chkFCActive.Name = "chkFCActive"
        Me.chkFCActive.Size = New System.Drawing.Size(61, 17)
        Me.chkFCActive.TabIndex = 26
        Me.chkFCActive.Text = "Active?"
        Me.chkFCActive.UseVisualStyleBackColor = False
        '
        'chkWCActive
        '
        Me.chkWCActive.AutoSize = True
        Me.chkWCActive.BackColor = System.Drawing.Color.Transparent
        Me.chkWCActive.Checked = True
        Me.chkWCActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWCActive.Location = New System.Drawing.Point(483, 39)
        Me.chkWCActive.Name = "chkWCActive"
        Me.chkWCActive.Size = New System.Drawing.Size(61, 17)
        Me.chkWCActive.TabIndex = 25
        Me.chkWCActive.Text = "Active?"
        Me.chkWCActive.UseVisualStyleBackColor = False
        '
        'chkSCActive
        '
        Me.chkSCActive.AutoSize = True
        Me.chkSCActive.BackColor = System.Drawing.Color.Transparent
        Me.chkSCActive.Checked = True
        Me.chkSCActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSCActive.Location = New System.Drawing.Point(483, 12)
        Me.chkSCActive.Name = "chkSCActive"
        Me.chkSCActive.Size = New System.Drawing.Size(61, 17)
        Me.chkSCActive.TabIndex = 24
        Me.chkSCActive.Text = "Active?"
        Me.chkSCActive.UseVisualStyleBackColor = False
        '
        'lblSCShip
        '
        Me.lblSCShip.AutoSize = True
        Me.lblSCShip.BackColor = System.Drawing.Color.Transparent
        Me.lblSCShip.Location = New System.Drawing.Point(253, 13)
        Me.lblSCShip.Name = "lblSCShip"
        Me.lblSCShip.Size = New System.Drawing.Size(31, 13)
        Me.lblSCShip.TabIndex = 9
        Me.lblSCShip.Text = "Ship:"
        '
        'lblWC
        '
        Me.lblWC.AutoSize = True
        Me.lblWC.BackColor = System.Drawing.Color.Transparent
        Me.lblWC.Location = New System.Drawing.Point(13, 40)
        Me.lblWC.Name = "lblWC"
        Me.lblWC.Size = New System.Drawing.Size(75, 13)
        Me.lblWC.TabIndex = 14
        Me.lblWC.Text = "Wing Booster:"
        '
        'lblFC
        '
        Me.lblFC.AutoSize = True
        Me.lblFC.BackColor = System.Drawing.Color.Transparent
        Me.lblFC.Location = New System.Drawing.Point(13, 67)
        Me.lblFC.Name = "lblFC"
        Me.lblFC.Size = New System.Drawing.Size(75, 13)
        Me.lblFC.TabIndex = 13
        Me.lblFC.Text = "Fleet Booster:"
        '
        'lblFCShip
        '
        Me.lblFCShip.AutoSize = True
        Me.lblFCShip.BackColor = System.Drawing.Color.Transparent
        Me.lblFCShip.Location = New System.Drawing.Point(253, 67)
        Me.lblFCShip.Name = "lblFCShip"
        Me.lblFCShip.Size = New System.Drawing.Size(31, 13)
        Me.lblFCShip.TabIndex = 17
        Me.lblFCShip.Text = "Ship:"
        '
        'lblSC
        '
        Me.lblSC.AutoSize = True
        Me.lblSC.BackColor = System.Drawing.Color.Transparent
        Me.lblSC.Location = New System.Drawing.Point(13, 13)
        Me.lblSC.Name = "lblSC"
        Me.lblSC.Size = New System.Drawing.Size(81, 13)
        Me.lblSC.TabIndex = 12
        Me.lblSC.Text = "Squad Booster:"
        '
        'lblFleetStatus
        '
        Me.lblFleetStatus.AutoSize = True
        Me.lblFleetStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblFleetStatus.Location = New System.Drawing.Point(85, 96)
        Me.lblFleetStatus.Name = "lblFleetStatus"
        Me.lblFleetStatus.Size = New System.Drawing.Size(46, 13)
        Me.lblFleetStatus.TabIndex = 22
        Me.lblFleetStatus.Text = "Inactive"
        '
        'lblWCShip
        '
        Me.lblWCShip.AutoSize = True
        Me.lblWCShip.BackColor = System.Drawing.Color.Transparent
        Me.lblWCShip.Location = New System.Drawing.Point(253, 40)
        Me.lblWCShip.Name = "lblWCShip"
        Me.lblWCShip.Size = New System.Drawing.Size(31, 13)
        Me.lblWCShip.TabIndex = 18
        Me.lblWCShip.Text = "Ship:"
        '
        'lblFleetStatusLabel
        '
        Me.lblFleetStatusLabel.AutoSize = True
        Me.lblFleetStatusLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblFleetStatusLabel.Location = New System.Drawing.Point(13, 96)
        Me.lblFleetStatusLabel.Name = "lblFleetStatusLabel"
        Me.lblFleetStatusLabel.Size = New System.Drawing.Size(69, 13)
        Me.lblFleetStatusLabel.TabIndex = 21
        Me.lblFleetStatusLabel.Text = "Fleet Status:"
        '
        'lblBoosterSlot3
        '
        Me.lblBoosterSlot3.AutoSize = True
        Me.lblBoosterSlot3.BackColor = System.Drawing.Color.Transparent
        Me.lblBoosterSlot3.Location = New System.Drawing.Point(21, 75)
        Me.lblBoosterSlot3.Name = "lblBoosterSlot3"
        Me.lblBoosterSlot3.Size = New System.Drawing.Size(38, 13)
        Me.lblBoosterSlot3.TabIndex = 5
        Me.lblBoosterSlot3.Text = "Slot 3:"
        '
        'lblBoosterSlot2
        '
        Me.lblBoosterSlot2.AutoSize = True
        Me.lblBoosterSlot2.BackColor = System.Drawing.Color.Transparent
        Me.lblBoosterSlot2.Location = New System.Drawing.Point(22, 48)
        Me.lblBoosterSlot2.Name = "lblBoosterSlot2"
        Me.lblBoosterSlot2.Size = New System.Drawing.Size(38, 13)
        Me.lblBoosterSlot2.TabIndex = 3
        Me.lblBoosterSlot2.Text = "Slot 2:"
        '
        'lblBoosterSlot1
        '
        Me.lblBoosterSlot1.AutoSize = True
        Me.lblBoosterSlot1.BackColor = System.Drawing.Color.Transparent
        Me.lblBoosterSlot1.Location = New System.Drawing.Point(22, 21)
        Me.lblBoosterSlot1.Name = "lblBoosterSlot1"
        Me.lblBoosterSlot1.Size = New System.Drawing.Size(38, 13)
        Me.lblBoosterSlot1.TabIndex = 1
        Me.lblBoosterSlot1.Text = "Slot 1:"
        '
        'lblWHClass
        '
        Me.lblWHClass.AutoSize = True
        Me.lblWHClass.BackColor = System.Drawing.Color.Transparent
        Me.lblWHClass.Location = New System.Drawing.Point(50, 53)
        Me.lblWHClass.Name = "lblWHClass"
        Me.lblWHClass.Size = New System.Drawing.Size(87, 13)
        Me.lblWHClass.TabIndex = 14
        Me.lblWHClass.Text = "Wormhole Class:"
        '
        'lblWHEffect
        '
        Me.lblWHEffect.AutoSize = True
        Me.lblWHEffect.BackColor = System.Drawing.Color.Transparent
        Me.lblWHEffect.Location = New System.Drawing.Point(26, 26)
        Me.lblWHEffect.Name = "lblWHEffect"
        Me.lblWHEffect.Size = New System.Drawing.Size(111, 13)
        Me.lblWHEffect.TabIndex = 12
        Me.lblWHEffect.Text = "Enviornmental Effect:"
        '
        'pbShipBay
        '
        Me.pbShipBay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.pbShipBay.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.pbShipBay.Location = New System.Drawing.Point(4, 17)
        Me.pbShipBay.Name = "pbShipBay"
        Me.pbShipBay.Size = New System.Drawing.Size(1047, 10)
        Me.pbShipBay.TabIndex = 6
        Me.pbShipBay.Text = "ProgressBarX1"
        Me.pbShipBay.Value = 50
        '
        'lblShipBay
        '
        Me.lblShipBay.AutoSize = True
        Me.lblShipBay.BackColor = System.Drawing.Color.Transparent
        Me.lblShipBay.Location = New System.Drawing.Point(4, 1)
        Me.lblShipBay.Name = "lblShipBay"
        Me.lblShipBay.Size = New System.Drawing.Size(89, 13)
        Me.lblShipBay.TabIndex = 3
        Me.lblShipBay.Text = "0.00 / 000.00 m³"
        '
        'lvwShipBay
        '
        Me.lvwShipBay.AllowDrop = True
        Me.lvwShipBay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lvwShipBay.Border.Class = "ListViewBorder"
        Me.lvwShipBay.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwShipBay.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colShipBayShip, Me.colShipBayQuantity, Me.colShipBayVolume, Me.colShipBayTotalVolume})
        Me.lvwShipBay.ContextMenuStrip = Me.ctxBays
        Me.lvwShipBay.FullRowSelect = True
        Me.lvwShipBay.GridLines = True
        Me.lvwShipBay.Location = New System.Drawing.Point(4, 31)
        Me.lvwShipBay.Name = "lvwShipBay"
        Me.lvwShipBay.Size = New System.Drawing.Size(1053, 200)
        Me.lvwShipBay.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwShipBay.TabIndex = 5
        Me.lvwShipBay.UseCompatibleStateImageBehavior = False
        Me.lvwShipBay.View = System.Windows.Forms.View.Details
        '
        'colShipBayShip
        '
        Me.colShipBayShip.Text = "Ship Type"
        Me.colShipBayShip.Width = 225
        '
        'colShipBayQuantity
        '
        Me.colShipBayQuantity.Text = "Qty"
        Me.colShipBayQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colShipBayQuantity.Width = 50
        '
        'colShipBayVolume
        '
        Me.colShipBayVolume.Text = "Ship Volume (m³)"
        Me.colShipBayVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colShipBayVolume.Width = 100
        '
        'colShipBayTotalVolume
        '
        Me.colShipBayTotalVolume.Text = "Total Volume (m³)"
        Me.colShipBayTotalVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colShipBayTotalVolume.Width = 100
        '
        'lvwHistory
        '
        '
        '
        '
        Me.lvwHistory.Border.Class = "ListViewBorder"
        Me.lvwHistory.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwHistory.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTransaction, Me.colSlotType, Me.colOldSlotNo, Me.colOldModName, Me.colOldChargeName, Me.colNewSlotNo, Me.colNewModName, Me.colNewChargeName, Me.colChargeOnly})
        Me.lvwHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwHistory.Location = New System.Drawing.Point(1, 1)
        Me.lvwHistory.Name = "lvwHistory"
        Me.lvwHistory.Size = New System.Drawing.Size(1059, 233)
        Me.lvwHistory.TabIndex = 0
        Me.lvwHistory.UseCompatibleStateImageBehavior = False
        Me.lvwHistory.View = System.Windows.Forms.View.Details
        '
        'colTransaction
        '
        Me.colTransaction.Text = "Transaction"
        Me.colTransaction.Width = 100
        '
        'colSlotType
        '
        Me.colSlotType.Text = "Type"
        '
        'colOldSlotNo
        '
        Me.colOldSlotNo.Text = "Old Slot"
        '
        'colOldModName
        '
        Me.colOldModName.Text = "Old Module"
        '
        'colOldChargeName
        '
        Me.colOldChargeName.Text = "Old Charge"
        '
        'colNewSlotNo
        '
        Me.colNewSlotNo.Text = "New Slot"
        '
        'colNewModName
        '
        Me.colNewModName.Text = "New Module"
        '
        'colNewChargeName
        '
        Me.colNewChargeName.Text = "New Charge"
        '
        'colChargeOnly
        '
        Me.colChargeOnly.Text = "Chg Only?"
        '
        'ExpandableSplitter1
        '
        Me.ExpandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.ExpandableSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ExpandableSplitter1.ExpandableControl = Me.tcStorage
        Me.ExpandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(61, Byte), Integer))
        Me.ExpandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.ExpandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2
        Me.ExpandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground
        Me.ExpandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter1.Location = New System.Drawing.Point(0, 400)
        Me.ExpandableSplitter1.Name = "ExpandableSplitter1"
        Me.ExpandableSplitter1.Size = New System.Drawing.Size(1061, 6)
        Me.ExpandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007
        Me.ExpandableSplitter1.TabIndex = 6
        Me.ExpandableSplitter1.TabStop = False
        '
        'tcStorage
        '
        Me.tcStorage.CanReorderTabs = True
        Me.tcStorage.Controls.Add(Me.tcpWHEffects)
        Me.tcStorage.Controls.Add(Me.tcpDroneBay)
        Me.tcStorage.Controls.Add(Me.tcpBoosters)
        Me.tcStorage.Controls.Add(Me.tcpFleetEffects)
        Me.tcStorage.Controls.Add(Me.tcpCargoBay)
        Me.tcStorage.Controls.Add(Me.tcpRemoteEffects)
        Me.tcStorage.Controls.Add(Me.tcpHistory)
        Me.tcStorage.Controls.Add(Me.tcpShipBay)
        Me.tcStorage.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tcStorage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcStorage.Location = New System.Drawing.Point(0, 406)
        Me.tcStorage.Name = "tcStorage"
        Me.tcStorage.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tcStorage.SelectedTabIndex = 0
        Me.tcStorage.Size = New System.Drawing.Size(1061, 258)
        Me.tcStorage.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tcStorage.TabIndex = 9
        Me.tcStorage.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tcStorage.Tabs.Add(Me.tiDroneBay)
        Me.tcStorage.Tabs.Add(Me.tiCargoBay)
        Me.tcStorage.Tabs.Add(Me.tiRemoteEffects)
        Me.tcStorage.Tabs.Add(Me.tiFleetEffects)
        Me.tcStorage.Tabs.Add(Me.tiBoosters)
        Me.tcStorage.Tabs.Add(Me.tiWHEffects)
        Me.tcStorage.Tabs.Add(Me.tiShipBay)
        Me.tcStorage.Tabs.Add(Me.tiHistory)
        Me.tcStorage.Text = "TabControl1"
        '
        'tcpDroneBay
        '
        Me.tcpDroneBay.Controls.Add(Me.btnMergeDrones)
        Me.tcpDroneBay.Controls.Add(Me.pbDroneBay)
        Me.tcpDroneBay.Controls.Add(Me.lblDroneBay)
        Me.tcpDroneBay.Controls.Add(Me.lvwDroneBay)
        Me.tcpDroneBay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpDroneBay.Location = New System.Drawing.Point(0, 23)
        Me.tcpDroneBay.Name = "tcpDroneBay"
        Me.tcpDroneBay.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpDroneBay.Size = New System.Drawing.Size(1061, 235)
        Me.tcpDroneBay.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpDroneBay.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpDroneBay.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpDroneBay.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpDroneBay.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpDroneBay.Style.GradientAngle = 90
        Me.tcpDroneBay.TabIndex = 1
        Me.tcpDroneBay.TabItem = Me.tiDroneBay
        '
        'btnMergeDrones
        '
        Me.btnMergeDrones.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnMergeDrones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMergeDrones.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnMergeDrones.Location = New System.Drawing.Point(977, 7)
        Me.btnMergeDrones.Name = "btnMergeDrones"
        Me.btnMergeDrones.Size = New System.Drawing.Size(80, 20)
        Me.btnMergeDrones.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnMergeDrones.TabIndex = 5
        Me.btnMergeDrones.Text = "Merge Drones"
        '
        'tiDroneBay
        '
        Me.tiDroneBay.AttachedControl = Me.tcpDroneBay
        Me.tiDroneBay.Name = "tiDroneBay"
        Me.tiDroneBay.Text = "Drone Bay"
        '
        'tcpBoosters
        '
        Me.tcpBoosters.Controls.Add(Me.btnBoosterSlot3)
        Me.tcpBoosters.Controls.Add(Me.btnBoosterSlot2)
        Me.tcpBoosters.Controls.Add(Me.btnBoosterSlot1)
        Me.tcpBoosters.Controls.Add(Me.cboBoosterSlot2)
        Me.tcpBoosters.Controls.Add(Me.cboBoosterSlot3)
        Me.tcpBoosters.Controls.Add(Me.cboBoosterSlot1)
        Me.tcpBoosters.Controls.Add(Me.lblBoosterSlot1)
        Me.tcpBoosters.Controls.Add(Me.lblBoosterSlot3)
        Me.tcpBoosters.Controls.Add(Me.lblBoosterSlot2)
        Me.tcpBoosters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpBoosters.Location = New System.Drawing.Point(0, 23)
        Me.tcpBoosters.Name = "tcpBoosters"
        Me.tcpBoosters.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpBoosters.Size = New System.Drawing.Size(1061, 235)
        Me.tcpBoosters.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpBoosters.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpBoosters.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpBoosters.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpBoosters.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpBoosters.Style.GradientAngle = 90
        Me.tcpBoosters.TabIndex = 5
        Me.tcpBoosters.TabItem = Me.tiBoosters
        '
        'btnBoosterSlot3
        '
        Me.btnBoosterSlot3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnBoosterSlot3.AutoExpandOnClick = True
        Me.btnBoosterSlot3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnBoosterSlot3.Enabled = False
        Me.btnBoosterSlot3.Location = New System.Drawing.Point(256, 72)
        Me.btnBoosterSlot3.Name = "btnBoosterSlot3"
        Me.btnBoosterSlot3.Size = New System.Drawing.Size(125, 21)
        Me.btnBoosterSlot3.SplitButton = True
        Me.btnBoosterSlot3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnBoosterSlot3.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnShowInfo3, Me.btnAlterSkills3, Me.btnRemoveBooster3})
        Me.btnBoosterSlot3.TabIndex = 14
        Me.btnBoosterSlot3.Text = "Booster Options"
        '
        'btnShowInfo3
        '
        Me.btnShowInfo3.Name = "btnShowInfo3"
        Me.btnShowInfo3.Text = "Show Booster Info"
        '
        'btnAlterSkills3
        '
        Me.btnAlterSkills3.BeginGroup = True
        Me.btnAlterSkills3.Name = "btnAlterSkills3"
        Me.btnAlterSkills3.Text = "Alter Skills"
        '
        'btnRemoveBooster3
        '
        Me.btnRemoveBooster3.BeginGroup = True
        Me.btnRemoveBooster3.Name = "btnRemoveBooster3"
        Me.btnRemoveBooster3.Text = "Remove Booster"
        '
        'btnBoosterSlot2
        '
        Me.btnBoosterSlot2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnBoosterSlot2.AutoExpandOnClick = True
        Me.btnBoosterSlot2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnBoosterSlot2.Enabled = False
        Me.btnBoosterSlot2.Location = New System.Drawing.Point(256, 45)
        Me.btnBoosterSlot2.Name = "btnBoosterSlot2"
        Me.btnBoosterSlot2.Size = New System.Drawing.Size(125, 21)
        Me.btnBoosterSlot2.SplitButton = True
        Me.btnBoosterSlot2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnBoosterSlot2.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnShowInfo2, Me.btnAlterSkills2, Me.btnRemoveBooster2})
        Me.btnBoosterSlot2.TabIndex = 13
        Me.btnBoosterSlot2.Text = "Booster Options"
        '
        'btnShowInfo2
        '
        Me.btnShowInfo2.Name = "btnShowInfo2"
        Me.btnShowInfo2.Text = "Show Booster Info"
        '
        'btnAlterSkills2
        '
        Me.btnAlterSkills2.BeginGroup = True
        Me.btnAlterSkills2.Name = "btnAlterSkills2"
        Me.btnAlterSkills2.Text = "Alter Skills"
        '
        'btnRemoveBooster2
        '
        Me.btnRemoveBooster2.BeginGroup = True
        Me.btnRemoveBooster2.Name = "btnRemoveBooster2"
        Me.btnRemoveBooster2.Text = "Remove Booster"
        '
        'btnBoosterSlot1
        '
        Me.btnBoosterSlot1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnBoosterSlot1.AutoExpandOnClick = True
        Me.btnBoosterSlot1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnBoosterSlot1.Enabled = False
        Me.btnBoosterSlot1.Location = New System.Drawing.Point(256, 18)
        Me.btnBoosterSlot1.Name = "btnBoosterSlot1"
        Me.btnBoosterSlot1.Size = New System.Drawing.Size(125, 21)
        Me.btnBoosterSlot1.SplitButton = True
        Me.btnBoosterSlot1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnBoosterSlot1.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnShowInfo1, Me.btnAlterSkills1, Me.btnRemoveBooster1})
        Me.btnBoosterSlot1.TabIndex = 12
        Me.btnBoosterSlot1.Text = "Booster Options"
        '
        'btnShowInfo1
        '
        Me.btnShowInfo1.Name = "btnShowInfo1"
        Me.btnShowInfo1.Text = "Show Booster Info"
        '
        'btnAlterSkills1
        '
        Me.btnAlterSkills1.BeginGroup = True
        Me.btnAlterSkills1.Name = "btnAlterSkills1"
        Me.btnAlterSkills1.Text = "Alter Skills"
        '
        'btnRemoveBooster1
        '
        Me.btnRemoveBooster1.BeginGroup = True
        Me.btnRemoveBooster1.Name = "btnRemoveBooster1"
        Me.btnRemoveBooster1.Text = "Remove Booster"
        '
        'cboBoosterSlot2
        '
        Me.cboBoosterSlot2.DisplayMember = "Text"
        Me.cboBoosterSlot2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboBoosterSlot2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBoosterSlot2.FormattingEnabled = True
        Me.cboBoosterSlot2.ItemHeight = 15
        Me.cboBoosterSlot2.Location = New System.Drawing.Point(66, 45)
        Me.cboBoosterSlot2.Name = "cboBoosterSlot2"
        Me.cboBoosterSlot2.Size = New System.Drawing.Size(163, 21)
        Me.cboBoosterSlot2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboBoosterSlot2.TabIndex = 11
        '
        'cboBoosterSlot3
        '
        Me.cboBoosterSlot3.DisplayMember = "Text"
        Me.cboBoosterSlot3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboBoosterSlot3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBoosterSlot3.FormattingEnabled = True
        Me.cboBoosterSlot3.ItemHeight = 15
        Me.cboBoosterSlot3.Location = New System.Drawing.Point(65, 72)
        Me.cboBoosterSlot3.Name = "cboBoosterSlot3"
        Me.cboBoosterSlot3.Size = New System.Drawing.Size(163, 21)
        Me.cboBoosterSlot3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboBoosterSlot3.TabIndex = 10
        '
        'cboBoosterSlot1
        '
        Me.cboBoosterSlot1.DisplayMember = "Text"
        Me.cboBoosterSlot1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboBoosterSlot1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBoosterSlot1.FormattingEnabled = True
        Me.cboBoosterSlot1.ItemHeight = 15
        Me.cboBoosterSlot1.Location = New System.Drawing.Point(66, 18)
        Me.cboBoosterSlot1.Name = "cboBoosterSlot1"
        Me.cboBoosterSlot1.Size = New System.Drawing.Size(163, 21)
        Me.cboBoosterSlot1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboBoosterSlot1.TabIndex = 9
        '
        'tiBoosters
        '
        Me.tiBoosters.AttachedControl = Me.tcpBoosters
        Me.tiBoosters.Name = "tiBoosters"
        Me.tiBoosters.Text = "Boosters"
        '
        'tcpFleetEffects
        '
        Me.tcpFleetEffects.Controls.Add(Me.cboFCShip)
        Me.tcpFleetEffects.Controls.Add(Me.cboWCShip)
        Me.tcpFleetEffects.Controls.Add(Me.cboSCShip)
        Me.tcpFleetEffects.Controls.Add(Me.cboFCPilot)
        Me.tcpFleetEffects.Controls.Add(Me.cboWCPilot)
        Me.tcpFleetEffects.Controls.Add(Me.cboSCPilot)
        Me.tcpFleetEffects.Controls.Add(Me.btnLeaveFleet)
        Me.tcpFleetEffects.Controls.Add(Me.chkFCActive)
        Me.tcpFleetEffects.Controls.Add(Me.lblSC)
        Me.tcpFleetEffects.Controls.Add(Me.chkWCActive)
        Me.tcpFleetEffects.Controls.Add(Me.chkSCActive)
        Me.tcpFleetEffects.Controls.Add(Me.lblFleetStatusLabel)
        Me.tcpFleetEffects.Controls.Add(Me.lblWCShip)
        Me.tcpFleetEffects.Controls.Add(Me.lblFleetStatus)
        Me.tcpFleetEffects.Controls.Add(Me.lblSCShip)
        Me.tcpFleetEffects.Controls.Add(Me.lblWC)
        Me.tcpFleetEffects.Controls.Add(Me.lblFCShip)
        Me.tcpFleetEffects.Controls.Add(Me.lblFC)
        Me.tcpFleetEffects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpFleetEffects.Location = New System.Drawing.Point(0, 23)
        Me.tcpFleetEffects.Name = "tcpFleetEffects"
        Me.tcpFleetEffects.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpFleetEffects.Size = New System.Drawing.Size(1061, 235)
        Me.tcpFleetEffects.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpFleetEffects.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpFleetEffects.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpFleetEffects.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpFleetEffects.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpFleetEffects.Style.GradientAngle = 90
        Me.tcpFleetEffects.TabIndex = 4
        Me.tcpFleetEffects.TabItem = Me.tiFleetEffects
        '
        'cboFCShip
        '
        Me.cboFCShip.DisplayMember = "Text"
        Me.cboFCShip.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboFCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFCShip.FormattingEnabled = True
        Me.cboFCShip.ItemHeight = 15
        Me.cboFCShip.Location = New System.Drawing.Point(290, 64)
        Me.cboFCShip.Name = "cboFCShip"
        Me.cboFCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboFCShip.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboFCShip.TabIndex = 33
        '
        'cboWCShip
        '
        Me.cboWCShip.DisplayMember = "Text"
        Me.cboWCShip.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboWCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWCShip.FormattingEnabled = True
        Me.cboWCShip.ItemHeight = 15
        Me.cboWCShip.Location = New System.Drawing.Point(290, 37)
        Me.cboWCShip.Name = "cboWCShip"
        Me.cboWCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboWCShip.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWCShip.TabIndex = 32
        '
        'cboSCShip
        '
        Me.cboSCShip.DisplayMember = "Text"
        Me.cboSCShip.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboSCShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSCShip.FormattingEnabled = True
        Me.cboSCShip.ItemHeight = 15
        Me.cboSCShip.Location = New System.Drawing.Point(290, 10)
        Me.cboSCShip.Name = "cboSCShip"
        Me.cboSCShip.Size = New System.Drawing.Size(187, 21)
        Me.cboSCShip.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboSCShip.TabIndex = 31
        '
        'cboFCPilot
        '
        Me.cboFCPilot.DisplayMember = "Text"
        Me.cboFCPilot.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboFCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFCPilot.FormattingEnabled = True
        Me.cboFCPilot.ItemHeight = 15
        Me.cboFCPilot.Location = New System.Drawing.Point(100, 64)
        Me.cboFCPilot.Name = "cboFCPilot"
        Me.cboFCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboFCPilot.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboFCPilot.TabIndex = 30
        '
        'cboWCPilot
        '
        Me.cboWCPilot.DisplayMember = "Text"
        Me.cboWCPilot.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboWCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWCPilot.FormattingEnabled = True
        Me.cboWCPilot.ItemHeight = 15
        Me.cboWCPilot.Location = New System.Drawing.Point(100, 37)
        Me.cboWCPilot.Name = "cboWCPilot"
        Me.cboWCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboWCPilot.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWCPilot.TabIndex = 29
        '
        'cboSCPilot
        '
        Me.cboSCPilot.DisplayMember = "Text"
        Me.cboSCPilot.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboSCPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSCPilot.FormattingEnabled = True
        Me.cboSCPilot.ItemHeight = 15
        Me.cboSCPilot.Location = New System.Drawing.Point(100, 10)
        Me.cboSCPilot.Name = "cboSCPilot"
        Me.cboSCPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboSCPilot.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboSCPilot.TabIndex = 28
        '
        'btnLeaveFleet
        '
        Me.btnLeaveFleet.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnLeaveFleet.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnLeaveFleet.Location = New System.Drawing.Point(172, 91)
        Me.btnLeaveFleet.Name = "btnLeaveFleet"
        Me.btnLeaveFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnLeaveFleet.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnLeaveFleet.TabIndex = 27
        Me.btnLeaveFleet.Text = "Leave Fleet"
        '
        'tiFleetEffects
        '
        Me.tiFleetEffects.AttachedControl = Me.tcpFleetEffects
        Me.tiFleetEffects.Name = "tiFleetEffects"
        Me.tiFleetEffects.Text = "Fleet Effects"
        '
        'tcpCargoBay
        '
        Me.tcpCargoBay.Controls.Add(Me.btnMergeCargo)
        Me.tcpCargoBay.Controls.Add(Me.pbCargoBay)
        Me.tcpCargoBay.Controls.Add(Me.lblCargoBay)
        Me.tcpCargoBay.Controls.Add(Me.lvwCargoBay)
        Me.tcpCargoBay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpCargoBay.Location = New System.Drawing.Point(0, 23)
        Me.tcpCargoBay.Name = "tcpCargoBay"
        Me.tcpCargoBay.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpCargoBay.Size = New System.Drawing.Size(1061, 235)
        Me.tcpCargoBay.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpCargoBay.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpCargoBay.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpCargoBay.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpCargoBay.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpCargoBay.Style.GradientAngle = 90
        Me.tcpCargoBay.TabIndex = 2
        Me.tcpCargoBay.TabItem = Me.tiCargoBay
        '
        'btnMergeCargo
        '
        Me.btnMergeCargo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnMergeCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMergeCargo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnMergeCargo.Location = New System.Drawing.Point(977, 7)
        Me.btnMergeCargo.Name = "btnMergeCargo"
        Me.btnMergeCargo.Size = New System.Drawing.Size(80, 20)
        Me.btnMergeCargo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnMergeCargo.TabIndex = 8
        Me.btnMergeCargo.Text = "Merge Cargo"
        '
        'tiCargoBay
        '
        Me.tiCargoBay.AttachedControl = Me.tcpCargoBay
        Me.tiCargoBay.Name = "tiCargoBay"
        Me.tiCargoBay.Text = "Cargo Bay"
        '
        'tcpRemoteEffects
        '
        Me.tcpRemoteEffects.Controls.Add(Me.cboPilot)
        Me.tcpRemoteEffects.Controls.Add(Me.cboFitting)
        Me.tcpRemoteEffects.Controls.Add(Me.btnUpdateRemoteEffects)
        Me.tcpRemoteEffects.Controls.Add(Me.btnAddRemoteFitting)
        Me.tcpRemoteEffects.Controls.Add(Me.lblFitting)
        Me.tcpRemoteEffects.Controls.Add(Me.lvwRemoteFittings)
        Me.tcpRemoteEffects.Controls.Add(Me.lvwRemoteEffects)
        Me.tcpRemoteEffects.Controls.Add(Me.lblPilot)
        Me.tcpRemoteEffects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpRemoteEffects.Location = New System.Drawing.Point(0, 23)
        Me.tcpRemoteEffects.Name = "tcpRemoteEffects"
        Me.tcpRemoteEffects.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpRemoteEffects.Size = New System.Drawing.Size(1061, 235)
        Me.tcpRemoteEffects.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpRemoteEffects.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpRemoteEffects.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpRemoteEffects.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpRemoteEffects.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpRemoteEffects.Style.GradientAngle = 90
        Me.tcpRemoteEffects.TabIndex = 3
        Me.tcpRemoteEffects.TabItem = Me.tiRemoteEffects
        '
        'cboPilot
        '
        Me.cboPilot.DisplayMember = "Text"
        Me.cboPilot.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilot.FormattingEnabled = True
        Me.cboPilot.ItemHeight = 15
        Me.cboPilot.Location = New System.Drawing.Point(289, 4)
        Me.cboPilot.Name = "cboPilot"
        Me.cboPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboPilot.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboPilot.TabIndex = 12
        '
        'cboFitting
        '
        Me.cboFitting.DisplayMember = "Text"
        Me.cboFitting.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFitting.FormattingEnabled = True
        Me.cboFitting.ItemHeight = 15
        Me.cboFitting.Location = New System.Drawing.Point(59, 4)
        Me.cboFitting.Name = "cboFitting"
        Me.cboFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboFitting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboFitting.TabIndex = 11
        '
        'btnUpdateRemoteEffects
        '
        Me.btnUpdateRemoteEffects.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnUpdateRemoteEffects.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnUpdateRemoteEffects.Location = New System.Drawing.Point(552, 4)
        Me.btnUpdateRemoteEffects.Name = "btnUpdateRemoteEffects"
        Me.btnUpdateRemoteEffects.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdateRemoteEffects.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnUpdateRemoteEffects.TabIndex = 10
        Me.btnUpdateRemoteEffects.Text = "Update"
        '
        'btnAddRemoteFitting
        '
        Me.btnAddRemoteFitting.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAddRemoteFitting.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAddRemoteFitting.Location = New System.Drawing.Point(471, 4)
        Me.btnAddRemoteFitting.Name = "btnAddRemoteFitting"
        Me.btnAddRemoteFitting.Size = New System.Drawing.Size(75, 23)
        Me.btnAddRemoteFitting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAddRemoteFitting.TabIndex = 9
        Me.btnAddRemoteFitting.Text = "Add"
        '
        'tiRemoteEffects
        '
        Me.tiRemoteEffects.AttachedControl = Me.tcpRemoteEffects
        Me.tiRemoteEffects.Name = "tiRemoteEffects"
        Me.tiRemoteEffects.Text = "Remote Effects"
        '
        'tcpHistory
        '
        Me.tcpHistory.Controls.Add(Me.lvwHistory)
        Me.tcpHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpHistory.Location = New System.Drawing.Point(0, 23)
        Me.tcpHistory.Name = "tcpHistory"
        Me.tcpHistory.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpHistory.Size = New System.Drawing.Size(1061, 235)
        Me.tcpHistory.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpHistory.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpHistory.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpHistory.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpHistory.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpHistory.Style.GradientAngle = 90
        Me.tcpHistory.TabIndex = 8
        Me.tcpHistory.TabItem = Me.tiHistory
        '
        'tiHistory
        '
        Me.tiHistory.AttachedControl = Me.tcpHistory
        Me.tiHistory.Name = "tiHistory"
        Me.tiHistory.Text = "History"
        '
        'tcpShipBay
        '
        Me.tcpShipBay.Controls.Add(Me.pbShipBay)
        Me.tcpShipBay.Controls.Add(Me.lblShipBay)
        Me.tcpShipBay.Controls.Add(Me.lvwShipBay)
        Me.tcpShipBay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpShipBay.Location = New System.Drawing.Point(0, 23)
        Me.tcpShipBay.Name = "tcpShipBay"
        Me.tcpShipBay.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpShipBay.Size = New System.Drawing.Size(1061, 235)
        Me.tcpShipBay.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpShipBay.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpShipBay.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpShipBay.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpShipBay.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpShipBay.Style.GradientAngle = 90
        Me.tcpShipBay.TabIndex = 7
        Me.tcpShipBay.TabItem = Me.tiShipBay
        '
        'tiShipBay
        '
        Me.tiShipBay.AttachedControl = Me.tcpShipBay
        Me.tiShipBay.Name = "tiShipBay"
        Me.tiShipBay.Text = "Ship Maintenance Bay"
        '
        'tcpWHEffects
        '
        Me.tcpWHEffects.Controls.Add(Me.cboWHClass)
        Me.tcpWHEffects.Controls.Add(Me.cboWHEffect)
        Me.tcpWHEffects.Controls.Add(Me.lblWHClass)
        Me.tcpWHEffects.Controls.Add(Me.lblWHEffect)
        Me.tcpWHEffects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcpWHEffects.Location = New System.Drawing.Point(0, 23)
        Me.tcpWHEffects.Name = "tcpWHEffects"
        Me.tcpWHEffects.Padding = New System.Windows.Forms.Padding(1)
        Me.tcpWHEffects.Size = New System.Drawing.Size(1061, 235)
        Me.tcpWHEffects.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tcpWHEffects.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tcpWHEffects.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tcpWHEffects.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tcpWHEffects.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tcpWHEffects.Style.GradientAngle = 90
        Me.tcpWHEffects.TabIndex = 6
        Me.tcpWHEffects.TabItem = Me.tiWHEffects
        '
        'cboWHClass
        '
        Me.cboWHClass.DisplayMember = "Text"
        Me.cboWHClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboWHClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWHClass.FormattingEnabled = True
        Me.cboWHClass.ItemHeight = 15
        Me.cboWHClass.Items.AddRange(New Object() {Me.ComboItem1, Me.ComboItem2, Me.ComboItem3, Me.ComboItem4, Me.ComboItem5, Me.ComboItem6})
        Me.cboWHClass.Location = New System.Drawing.Point(143, 48)
        Me.cboWHClass.Name = "cboWHClass"
        Me.cboWHClass.Size = New System.Drawing.Size(59, 21)
        Me.cboWHClass.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWHClass.TabIndex = 16
        '
        'ComboItem1
        '
        Me.ComboItem1.Text = "1"
        '
        'ComboItem2
        '
        Me.ComboItem2.Text = "2"
        '
        'ComboItem3
        '
        Me.ComboItem3.Text = "3"
        '
        'ComboItem4
        '
        Me.ComboItem4.Text = "4"
        '
        'ComboItem5
        '
        Me.ComboItem5.Text = "5"
        '
        'ComboItem6
        '
        Me.ComboItem6.Text = "6"
        '
        'cboWHEffect
        '
        Me.cboWHEffect.DisplayMember = "Text"
        Me.cboWHEffect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboWHEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWHEffect.FormattingEnabled = True
        Me.cboWHEffect.ItemHeight = 15
        Me.cboWHEffect.Items.AddRange(New Object() {Me.ComboItem7, Me.ComboItem8, Me.ComboItem9, Me.ComboItem10, Me.ComboItem11, Me.ComboItem12, Me.ComboItem13})
        Me.cboWHEffect.Location = New System.Drawing.Point(143, 21)
        Me.cboWHEffect.Name = "cboWHEffect"
        Me.cboWHEffect.Size = New System.Drawing.Size(148, 21)
        Me.cboWHEffect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWHEffect.TabIndex = 15
        '
        'ComboItem7
        '
        Me.ComboItem7.Text = "<None>"
        '
        'ComboItem8
        '
        Me.ComboItem8.Text = "Black Hole"
        '
        'ComboItem9
        '
        Me.ComboItem9.Text = "Cataclysmic Variable"
        '
        'ComboItem10
        '
        Me.ComboItem10.Text = "Magnetar"
        '
        'ComboItem11
        '
        Me.ComboItem11.Text = "Pulsar"
        '
        'ComboItem12
        '
        Me.ComboItem12.Text = "Red Giant"
        '
        'ComboItem13
        '
        Me.ComboItem13.Text = "Wolf Rayet"
        '
        'tiWHEffects
        '
        Me.tiWHEffects.AttachedControl = Me.tcpWHEffects
        Me.tiWHEffects.Name = "tiWHEffects"
        Me.tiWHEffects.Text = "Environment"
        '
        'panelFunctions
        '
        Me.panelFunctions.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelFunctions.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelFunctions.Controls.Add(Me.btnAutoSize)
        Me.panelFunctions.Controls.Add(Me.btnRedo)
        Me.panelFunctions.Controls.Add(Me.btnUndo)
        Me.panelFunctions.Controls.Add(Me.pbShip)
        Me.panelFunctions.Controls.Add(Me.lblFittingMarketPrice)
        Me.panelFunctions.Controls.Add(Me.pbShipInfo)
        Me.panelFunctions.Controls.Add(Me.lblShipMarketPrice)
        Me.panelFunctions.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelFunctions.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panelFunctions.Location = New System.Drawing.Point(0, 0)
        Me.panelFunctions.Name = "panelFunctions"
        Me.panelFunctions.Size = New System.Drawing.Size(1061, 38)
        Me.panelFunctions.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelFunctions.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelFunctions.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelFunctions.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelFunctions.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelFunctions.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelFunctions.Style.GradientAngle = 90
        Me.panelFunctions.TabIndex = 7
        '
        'btnAutoSize
        '
        Me.btnAutoSize.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAutoSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAutoSize.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAutoSize.FocusCuesEnabled = False
        Me.btnAutoSize.Image = CType(resources.GetObject("btnAutoSize.Image"), System.Drawing.Image)
        Me.btnAutoSize.ImageFixedSize = New System.Drawing.Size(32, 32)
        Me.btnAutoSize.Location = New System.Drawing.Point(887, 3)
        Me.btnAutoSize.Name = "btnAutoSize"
        Me.btnAutoSize.Size = New System.Drawing.Size(48, 32)
        Me.btnAutoSize.SplitButton = True
        Me.btnAutoSize.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.SlotTip.SetSuperTooltip(Me.btnAutoSize, New DevComponents.DotNetBar.SuperTooltipInfo("", "Autosize Slot Columns", "Automatically resizes all columns to match the size of the contents that they con" & _
            "tain." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Does not affect the user-selected size.", Nothing, CType(resources.GetObject("btnAutoSize.SuperTooltip"), System.Drawing.Image), DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnAutoSize.TabIndex = 16
        '
        'btnRedo
        '
        Me.btnRedo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnRedo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRedo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnRedo.FocusCuesEnabled = False
        Me.btnRedo.Image = Global.EveHQ.HQF.My.Resources.Resources.redo
        Me.btnRedo.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnRedo.Location = New System.Drawing.Point(1009, 3)
        Me.btnRedo.Name = "btnRedo"
        Me.btnRedo.Size = New System.Drawing.Size(48, 32)
        Me.btnRedo.SplitButton = True
        Me.btnRedo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.SlotTip.SetSuperTooltip(Me.btnRedo, New DevComponents.DotNetBar.SuperTooltipInfo("", "Redo Operation", "This button will redo the most recent changes undone." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Use the split button to " & _
            "undo multiple operations at once.", Nothing, Global.EveHQ.HQF.My.Resources.Resources.redo, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnRedo.TabIndex = 15
        '
        'btnUndo
        '
        Me.btnUndo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnUndo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUndo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnUndo.FocusCuesEnabled = False
        Me.btnUndo.Image = Global.EveHQ.HQF.My.Resources.Resources.undo
        Me.btnUndo.ImageFixedSize = New System.Drawing.Size(40, 40)
        Me.btnUndo.Location = New System.Drawing.Point(955, 3)
        Me.btnUndo.Name = "btnUndo"
        Me.btnUndo.Size = New System.Drawing.Size(48, 32)
        Me.btnUndo.SplitButton = True
        Me.btnUndo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.SlotTip.SetSuperTooltip(Me.btnUndo, New DevComponents.DotNetBar.SuperTooltipInfo("", "Undo Operation", "This button will undo the most recent loading or unloading of a module or charge." & _
            "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Use the split button to undo multiple operations at once." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10), Nothing, Global.EveHQ.HQF.My.Resources.Resources.undo, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnUndo.TabIndex = 14
        '
        'adtSlots
        '
        Me.adtSlots.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtSlots.AllowDrop = True
        Me.adtSlots.AllowUserToReorderColumns = True
        Me.adtSlots.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtSlots.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtSlots.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtSlots.Columns.Add(Me.colTestHeader1)
        Me.adtSlots.ContextMenuStrip = Me.ctxSlots
        Me.adtSlots.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtSlots.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Ellipse
        Me.adtSlots.ExpandWidth = 8
        Me.adtSlots.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.adtSlots.Indent = 2
        Me.adtSlots.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtSlots.Location = New System.Drawing.Point(0, 38)
        Me.adtSlots.MultiSelect = True
        Me.adtSlots.Name = "adtSlots"
        Me.adtSlots.Nodes.AddRange(New DevComponents.AdvTree.Node() {Me.Node1})
        Me.adtSlots.NodesConnector = Me.NodeConnector1
        Me.adtSlots.NodeSpacing = 2
        Me.adtSlots.PathSeparator = ";"
        Me.adtSlots.SelectionBox = False
        Me.adtSlots.Size = New System.Drawing.Size(1061, 362)
        Me.adtSlots.Styles.Add(Me.SlotStyle)
        Me.adtSlots.Styles.Add(Me.HeaderStyle)
        Me.adtSlots.TabIndex = 8
        Me.adtSlots.Text = "AdvTree1"
        '
        'colTestHeader1
        '
        Me.colTestHeader1.Name = "colTestHeader1"
        Me.colTestHeader1.SortingEnabled = False
        Me.colTestHeader1.Text = "Module Name"
        Me.colTestHeader1.Width.Absolute = 150
        Me.colTestHeader1.Width.AutoSizeMinHeader = True
        '
        'Node1
        '
        Me.Node1.Expanded = True
        Me.Node1.FullRowBackground = True
        Me.Node1.Name = "Node1"
        Me.Node1.Nodes.AddRange(New DevComponents.AdvTree.Node() {Me.Node2})
        Me.Node1.Style = Me.HeaderStyle
        Me.Node1.Text = "High Slots"
        '
        'Node2
        '
        Me.Node2.Cells.Add(Me.Cell2)
        Me.Node2.Cells.Add(Me.Cell3)
        Me.Node2.Cells.Add(Me.Cell4)
        Me.Node2.Expanded = True
        Me.Node2.Image = Global.EveHQ.HQF.My.Resources.Resources.Mod04
        Me.Node2.Name = "Node2"
        Me.Node2.Style = Me.SlotStyle
        Me.Node2.Text = "Cruise Missile Launcher II"
        '
        'Cell2
        '
        Me.Cell2.Name = "Cell2"
        Me.Cell2.StyleMouseOver = Nothing
        Me.Cell2.Text = "Paradise Fury Cruise Missile"
        '
        'Cell3
        '
        Me.Cell3.Name = "Cell3"
        Me.Cell3.StyleMouseOver = Nothing
        Me.Cell3.Text = "80"
        '
        'Cell4
        '
        Me.Cell4.Name = "Cell4"
        Me.Cell4.StyleMouseOver = Nothing
        Me.Cell4.Text = "1800"
        '
        'SlotStyle
        '
        Me.SlotStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.SlotStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(155, Byte), Integer), CType(CType(187, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.SlotStyle.BackColorGradientAngle = 90
        Me.SlotStyle.BackColorGradientType = DevComponents.DotNetBar.eGradientType.Radial
        Me.SlotStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.SlotStyle.BorderBottomWidth = 1
        Me.SlotStyle.BorderColor = System.Drawing.Color.DarkGray
        Me.SlotStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.SlotStyle.BorderLeftWidth = 1
        Me.SlotStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.SlotStyle.BorderRightWidth = 1
        Me.SlotStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.SlotStyle.BorderTopWidth = 1
        Me.SlotStyle.CornerDiameter = 4
        Me.SlotStyle.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.SlotStyle.Description = "BlueMist"
        Me.SlotStyle.Name = "SlotStyle"
        Me.SlotStyle.TextColor = System.Drawing.Color.Black
        '
        'HeaderStyle
        '
        Me.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.HeaderStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(149, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.HeaderStyle.BackColorGradientAngle = 90
        Me.HeaderStyle.BackColorGradientType = DevComponents.DotNetBar.eGradientType.Radial
        Me.HeaderStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.HeaderStyle.BorderBottomWidth = 1
        Me.HeaderStyle.BorderColor = System.Drawing.Color.DarkGray
        Me.HeaderStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.HeaderStyle.BorderLeftWidth = 1
        Me.HeaderStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.HeaderStyle.BorderRightWidth = 1
        Me.HeaderStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.HeaderStyle.BorderTopWidth = 1
        Me.HeaderStyle.CornerDiameter = 4
        Me.HeaderStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.HeaderStyle.Description = "Silver"
        Me.HeaderStyle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HeaderStyle.Name = "HeaderStyle"
        Me.HeaderStyle.TextColor = System.Drawing.Color.Black
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        Me.NodeConnector1.LineWidth = 0
        '
        'Cell7
        '
        Me.Cell7.Name = "Cell7"
        Me.Cell7.StyleMouseOver = Nothing
        Me.Cell7.Text = "Wrath Fury Cruise Missile"
        '
        'Cell8
        '
        Me.Cell8.Name = "Cell8"
        Me.Cell8.StyleMouseOver = Nothing
        Me.Cell8.Text = "80"
        '
        'Cell9
        '
        Me.Cell9.Name = "Cell9"
        Me.Cell9.StyleMouseOver = Nothing
        Me.Cell9.Text = "1800"
        '
        'Cell13
        '
        Me.Cell13.Name = "Cell13"
        Me.Cell13.StyleMouseOver = Nothing
        Me.Cell13.Text = "Cataclysm Fury Cruise Missile"
        '
        'Cell14
        '
        Me.Cell14.Name = "Cell14"
        Me.Cell14.StyleMouseOver = Nothing
        Me.Cell14.Text = "80"
        '
        'Cell15
        '
        Me.Cell15.Name = "Cell15"
        Me.Cell15.StyleMouseOver = Nothing
        Me.Cell15.Text = "1800"
        '
        'Cell1
        '
        Me.Cell1.Name = "Cell1"
        Me.Cell1.StyleMouseOver = Nothing
        Me.Cell1.Text = "Wrath Fury Cruise Missile"
        '
        'Cell5
        '
        Me.Cell5.Name = "Cell5"
        Me.Cell5.StyleMouseOver = Nothing
        Me.Cell5.Text = "80"
        '
        'Cell6
        '
        Me.Cell6.Name = "Cell6"
        Me.Cell6.StyleMouseOver = Nothing
        Me.Cell6.Text = "1800"
        '
        'Cell10
        '
        Me.Cell10.Name = "Cell10"
        Me.Cell10.StyleMouseOver = Nothing
        Me.Cell10.Text = "Cataclysm Fury Cruise Missile"
        '
        'Cell11
        '
        Me.Cell11.Name = "Cell11"
        Me.Cell11.StyleMouseOver = Nothing
        Me.Cell11.Text = "80"
        '
        'Cell12
        '
        Me.Cell12.Name = "Cell12"
        Me.Cell12.StyleMouseOver = Nothing
        Me.Cell12.Text = "1800"
        '
        'SlotTip
        '
        Me.SlotTip.DefaultFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SlotTip.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.SlotTip.PositionBelowControl = False
        '
        'ShipSlotControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.adtSlots)
        Me.Controls.Add(Me.panelFunctions)
        Me.Controls.Add(Me.ExpandableSplitter1)
        Me.Controls.Add(Me.tcStorage)
        Me.Name = "ShipSlotControl"
        Me.Size = New System.Drawing.Size(1061, 664)
        Me.ctxSlots.ResumeLayout(False)
        Me.ctxBays.ResumeLayout(False)
        Me.ctxRemoteFittings.ResumeLayout(False)
        Me.ctxRemoteModule.ResumeLayout(False)
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxShipSkills.ResumeLayout(False)
        CType(Me.pbShipInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tcStorage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcStorage.ResumeLayout(False)
        Me.tcpDroneBay.ResumeLayout(False)
        Me.tcpDroneBay.PerformLayout()
        Me.tcpBoosters.ResumeLayout(False)
        Me.tcpBoosters.PerformLayout()
        Me.tcpFleetEffects.ResumeLayout(False)
        Me.tcpFleetEffects.PerformLayout()
        Me.tcpCargoBay.ResumeLayout(False)
        Me.tcpCargoBay.PerformLayout()
        Me.tcpRemoteEffects.ResumeLayout(False)
        Me.tcpRemoteEffects.PerformLayout()
        Me.tcpHistory.ResumeLayout(False)
        Me.tcpShipBay.ResumeLayout(False)
        Me.tcpShipBay.PerformLayout()
        Me.tcpWHEffects.ResumeLayout(False)
        Me.tcpWHEffects.PerformLayout()
        Me.panelFunctions.ResumeLayout(False)
        Me.panelFunctions.PerformLayout()
        CType(Me.adtSlots, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblFittingMarketPrice As System.Windows.Forms.Label
    Friend WithEvents lblShipMarketPrice As System.Windows.Forms.Label
    Friend WithEvents ctxSlots As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ShowInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblDroneBay As System.Windows.Forms.Label
    Friend WithEvents lvwDroneBay As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents lblCargoBay As System.Windows.Forms.Label
    Friend WithEvents colDroneBayType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDroneBayQty As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwCargoBay As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colCargoBayType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCargoBayQty As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ctxBays As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ctxRemoveItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxAlterQuantity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxSplitBatch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxShowBayInfoItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pbShipInfo As System.Windows.Forms.PictureBox
    Friend WithEvents lvwRemoteEffects As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents lblFitting As System.Windows.Forms.Label
    Friend WithEvents colModule As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxRemoteModule As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuShowRemoteModInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lvwRemoteFittings As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxRemoteFittings As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveFittingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblWC As System.Windows.Forms.Label
    Friend WithEvents lblFC As System.Windows.Forms.Label
    Friend WithEvents lblSC As System.Windows.Forms.Label
    Friend WithEvents lblSCShip As System.Windows.Forms.Label
    Friend WithEvents lblWCShip As System.Windows.Forms.Label
    Friend WithEvents lblFCShip As System.Windows.Forms.Label
    Friend WithEvents lblFleetStatus As System.Windows.Forms.Label
    Friend WithEvents lblFleetStatusLabel As System.Windows.Forms.Label
    Friend WithEvents chkFCActive As System.Windows.Forms.CheckBox
    Friend WithEvents chkWCActive As System.Windows.Forms.CheckBox
    Friend WithEvents chkSCActive As System.Windows.Forms.CheckBox
    Friend WithEvents pbShip As System.Windows.Forms.PictureBox
    Friend WithEvents ctxShipSkills As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAlterRelevantSkills As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblShipBay As System.Windows.Forms.Label
    Friend WithEvents lvwShipBay As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colShipBayShip As System.Windows.Forms.ColumnHeader
    Friend WithEvents colShipBayQuantity As System.Windows.Forms.ColumnHeader
    Friend WithEvents colShipBayVolume As System.Windows.Forms.ColumnHeader
    Friend WithEvents colShipBayTotalVolume As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblWHClass As System.Windows.Forms.Label
    Friend WithEvents lblWHEffect As System.Windows.Forms.Label
    Friend WithEvents lblBoosterSlot3 As System.Windows.Forms.Label
    Friend WithEvents lblBoosterSlot2 As System.Windows.Forms.Label
    Friend WithEvents lblBoosterSlot1 As System.Windows.Forms.Label
    Friend WithEvents ExpandableSplitter1 As DevComponents.DotNetBar.ExpandableSplitter
    Friend WithEvents panelFunctions As DevComponents.DotNetBar.PanelEx
    Friend WithEvents adtSlots As DevComponents.AdvTree.AdvTree
    Friend WithEvents Node1 As DevComponents.AdvTree.Node
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents colTestHeader1 As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents Cell2 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell1 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell3 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell4 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell5 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell6 As DevComponents.AdvTree.Cell
    Friend WithEvents SlotStyle As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents Cell7 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell8 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell9 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell10 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell11 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell12 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell13 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell14 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell15 As DevComponents.AdvTree.Cell
    Friend WithEvents HeaderStyle As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents SlotTip As DevComponents.DotNetBar.SuperTooltip
    Friend WithEvents lvwHistory As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colTransaction As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSlotType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOldSlotNo As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOldModName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOldChargeName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNewSlotNo As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNewModName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNewChargeName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colChargeOnly As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnUndo As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnRedo As DevComponents.DotNetBar.ButtonX
    Friend WithEvents pbDroneBay As DevComponents.DotNetBar.Controls.ProgressBarX
    Friend WithEvents pbCargoBay As DevComponents.DotNetBar.Controls.ProgressBarX
    Friend WithEvents pbShipBay As DevComponents.DotNetBar.Controls.ProgressBarX
    Friend WithEvents tcStorage As DevComponents.DotNetBar.TabControl
    Friend WithEvents tcpCargoBay As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiCargoBay As DevComponents.DotNetBar.TabItem
    Friend WithEvents tcpDroneBay As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiDroneBay As DevComponents.DotNetBar.TabItem
    Friend WithEvents tcpRemoteEffects As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiRemoteEffects As DevComponents.DotNetBar.TabItem
    Friend WithEvents tcpFleetEffects As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiFleetEffects As DevComponents.DotNetBar.TabItem
    Friend WithEvents tcpBoosters As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiBoosters As DevComponents.DotNetBar.TabItem
    Friend WithEvents tcpWHEffects As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiWHEffects As DevComponents.DotNetBar.TabItem
    Friend WithEvents tcpShipBay As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiShipBay As DevComponents.DotNetBar.TabItem
    Friend WithEvents tcpHistory As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiHistory As DevComponents.DotNetBar.TabItem
    Friend WithEvents btnMergeCargo As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnMergeDrones As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboPilot As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboFitting As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents btnUpdateRemoteEffects As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAddRemoteFitting As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboBoosterSlot2 As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboBoosterSlot3 As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboBoosterSlot1 As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboFCShip As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboWCShip As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboSCShip As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboFCPilot As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboWCPilot As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents cboSCPilot As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents btnLeaveFleet As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboWHClass As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem1 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem2 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem3 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem4 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem5 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem6 As DevComponents.Editors.ComboItem
    Friend WithEvents cboWHEffect As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem7 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem8 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem9 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem10 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem11 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem12 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem13 As DevComponents.Editors.ComboItem
    Friend WithEvents Node2 As DevComponents.AdvTree.Node
    Friend WithEvents btnBoosterSlot1 As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnShowInfo1 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnAlterSkills1 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRemoveBooster1 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnBoosterSlot3 As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnShowInfo3 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnAlterSkills3 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRemoveBooster3 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnBoosterSlot2 As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnShowInfo2 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnAlterSkills2 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnRemoveBooster2 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnAutoSize As DevComponents.DotNetBar.ButtonX
End Class
