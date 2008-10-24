﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.btnToggleStorage = New System.Windows.Forms.Button
        Me.panelFunctions = New System.Windows.Forms.Panel
        Me.pbShipInfo = New System.Windows.Forms.PictureBox
        Me.btnMergeDrones = New System.Windows.Forms.Button
        Me.btnMergeCargo = New System.Windows.Forms.Button
        Me.lvwSlots = New EveHQ.HQF.ListViewNoFlicker
        Me.ctxSlots.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabStorage.SuspendLayout()
        Me.tabDroneBay.SuspendLayout()
        Me.ctxBays.SuspendLayout()
        Me.tabCargoBay.SuspendLayout()
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
        Me.SplitContainer1.Size = New System.Drawing.Size(599, 437)
        Me.SplitContainer1.SplitterDistance = 231
        Me.SplitContainer1.TabIndex = 1
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
        Me.tabStorage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabStorage.Location = New System.Drawing.Point(0, 0)
        Me.tabStorage.Multiline = True
        Me.tabStorage.Name = "tabStorage"
        Me.tabStorage.SelectedIndex = 0
        Me.tabStorage.Size = New System.Drawing.Size(599, 202)
        Me.tabStorage.TabIndex = 0
        '
        'tabDroneBay
        '
        Me.tabDroneBay.Controls.Add(Me.btnMergeDrones)
        Me.tabDroneBay.Controls.Add(Me.lvwDroneBay)
        Me.tabDroneBay.Controls.Add(Me.pbDroneBay)
        Me.tabDroneBay.Controls.Add(Me.lblDroneBay)
        Me.tabDroneBay.Location = New System.Drawing.Point(4, 22)
        Me.tabDroneBay.Name = "tabDroneBay"
        Me.tabDroneBay.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDroneBay.Size = New System.Drawing.Size(591, 176)
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
        Me.lvwDroneBay.Size = New System.Drawing.Size(578, 137)
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
        'pbDroneBay
        '
        Me.pbDroneBay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbDroneBay.Location = New System.Drawing.Point(7, 19)
        Me.pbDroneBay.Name = "pbDroneBay"
        Me.pbDroneBay.Size = New System.Drawing.Size(483, 10)
        Me.pbDroneBay.TabIndex = 1
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
        Me.tabCargoBay.Controls.Add(Me.btnMergeCargo)
        Me.tabCargoBay.Controls.Add(Me.lvwCargoBay)
        Me.tabCargoBay.Controls.Add(Me.pbCargoBay)
        Me.tabCargoBay.Controls.Add(Me.lblCargoBay)
        Me.tabCargoBay.Location = New System.Drawing.Point(4, 22)
        Me.tabCargoBay.Name = "tabCargoBay"
        Me.tabCargoBay.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCargoBay.Size = New System.Drawing.Size(591, 176)
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
        Me.lvwCargoBay.Size = New System.Drawing.Size(578, 135)
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
        Me.pbCargoBay.Location = New System.Drawing.Point(6, 19)
        Me.pbCargoBay.Name = "pbCargoBay"
        Me.pbCargoBay.Size = New System.Drawing.Size(482, 10)
        Me.pbCargoBay.TabIndex = 4
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
        Me.panelFunctions.Size = New System.Drawing.Size(599, 38)
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
        'btnMergeDrones
        '
        Me.btnMergeDrones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMergeDrones.Location = New System.Drawing.Point(496, 6)
        Me.btnMergeDrones.Name = "btnMergeDrones"
        Me.btnMergeDrones.Size = New System.Drawing.Size(90, 23)
        Me.btnMergeDrones.TabIndex = 3
        Me.btnMergeDrones.Text = "Merge Drones"
        Me.btnMergeDrones.UseVisualStyleBackColor = True
        '
        'btnMergeCargo
        '
        Me.btnMergeCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMergeCargo.Location = New System.Drawing.Point(495, 6)
        Me.btnMergeCargo.Name = "btnMergeCargo"
        Me.btnMergeCargo.Size = New System.Drawing.Size(90, 23)
        Me.btnMergeCargo.TabIndex = 6
        Me.btnMergeCargo.Text = "Merge Cargo"
        Me.btnMergeCargo.UseVisualStyleBackColor = True
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
        Me.lvwSlots.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4})
        Me.lvwSlots.Location = New System.Drawing.Point(0, 0)
        Me.lvwSlots.Name = "lvwSlots"
        Me.lvwSlots.Size = New System.Drawing.Size(599, 231)
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
        Me.Size = New System.Drawing.Size(599, 479)
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
    Friend WithEvents pbDroneBay As System.Windows.Forms.ProgressBar
    Friend WithEvents lvwDroneBay As System.Windows.Forms.ListView
    Friend WithEvents pbCargoBay As System.Windows.Forms.ProgressBar
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
    End Sub
    Friend WithEvents imgState As System.Windows.Forms.ImageList
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxShowBayInfoItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents panelFunctions As System.Windows.Forms.Panel
    Friend WithEvents pbShipInfo As System.Windows.Forms.PictureBox
    Friend WithEvents btnToggleStorage As System.Windows.Forms.Button
    Friend WithEvents btnMergeDrones As System.Windows.Forms.Button
    Friend WithEvents btnMergeCargo As System.Windows.Forms.Button
End Class
