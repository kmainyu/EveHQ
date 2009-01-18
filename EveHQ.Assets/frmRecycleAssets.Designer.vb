<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRecycleAssets
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRecycleAssets))
        Me.clvRecycle = New DotNetLib.Windows.Forms.ContainerListView
        Me.colItem = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colMetaLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBatches = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colItemPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTotalPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRefinePrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lblPilot = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.chkPerfectRefine = New System.Windows.Forms.CheckBox
        Me.lblBaseYieldLbl = New System.Windows.Forms.Label
        Me.lblNetYieldLbl = New System.Windows.Forms.Label
        Me.lblStandingsLbl = New System.Windows.Forms.Label
        Me.lblStationTakeLbl = New System.Windows.Forms.Label
        Me.lblStationTake = New System.Windows.Forms.Label
        Me.lblStandings = New System.Windows.Forms.Label
        Me.lblNetYield = New System.Windows.Forms.Label
        Me.lblBaseYield = New System.Windows.Forms.Label
        Me.lblStationLbl = New System.Windows.Forms.Label
        Me.lblStation = New System.Windows.Forms.Label
        Me.lblCorp = New System.Windows.Forms.Label
        Me.lblCorpLbl = New System.Windows.Forms.Label
        Me.nudBaseYield = New System.Windows.Forms.NumericUpDown
        Me.nudStandings = New System.Windows.Forms.NumericUpDown
        Me.chkOverrideBaseYield = New System.Windows.Forms.CheckBox
        Me.chkOverrideStandings = New System.Windows.Forms.CheckBox
        Me.lblRefineMode = New System.Windows.Forms.Label
        Me.cboRefineMode = New System.Windows.Forms.ComboBox
        Me.lblVolumeLbl = New System.Windows.Forms.Label
        Me.lblItemsLbl = New System.Windows.Forms.Label
        Me.lblVolume = New System.Windows.Forms.Label
        Me.lblItems = New System.Windows.Forms.Label
        Me.tabRecycle = New System.Windows.Forms.TabControl
        Me.tabItems = New System.Windows.Forms.TabPage
        Me.tabTotals = New System.Windows.Forms.TabPage
        Me.clvTotals = New DotNetLib.Windows.Forms.ContainerListView
        Me.colMaterial = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colReceive = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colStationTake = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colWaste = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colMatPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colMatTotal = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        CType(Me.nudBaseYield, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStandings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabRecycle.SuspendLayout()
        Me.tabItems.SuspendLayout()
        Me.tabTotals.SuspendLayout()
        Me.SuspendLayout()
        '
        'clvRecycle
        '
        Me.clvRecycle.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colItem, Me.colMetaLevel, Me.colQuantity, Me.colBatches, Me.colItemPrice, Me.colTotalPrice, Me.colRefinePrice})
        Me.clvRecycle.DefaultItemHeight = 20
        Me.clvRecycle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvRecycle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvRecycle.Location = New System.Drawing.Point(3, 3)
        Me.clvRecycle.MultipleColumnSort = True
        Me.clvRecycle.Name = "clvRecycle"
        Me.clvRecycle.Size = New System.Drawing.Size(932, 384)
        Me.clvRecycle.TabIndex = 0
        '
        'colItem
        '
        Me.colItem.CustomSortTag = Nothing
        Me.colItem.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colItem.Tag = Nothing
        Me.colItem.Text = "Item"
        Me.colItem.Width = 300
        '
        'colMetaLevel
        '
        Me.colMetaLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colMetaLevel.CustomSortTag = Nothing
        Me.colMetaLevel.DisplayIndex = 1
        Me.colMetaLevel.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colMetaLevel.Tag = Nothing
        Me.colMetaLevel.Text = "Meta Level"
        '
        'colQuantity
        '
        Me.colQuantity.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colQuantity.CustomSortTag = Nothing
        Me.colQuantity.DisplayIndex = 2
        Me.colQuantity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colQuantity.Tag = Nothing
        Me.colQuantity.Text = "Quantity"
        Me.colQuantity.Width = 75
        '
        'colBatches
        '
        Me.colBatches.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colBatches.CustomSortTag = Nothing
        Me.colBatches.DisplayIndex = 3
        Me.colBatches.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colBatches.Tag = Nothing
        Me.colBatches.Text = "Batches"
        '
        'colItemPrice
        '
        Me.colItemPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colItemPrice.CustomSortTag = Nothing
        Me.colItemPrice.DisplayIndex = 4
        Me.colItemPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colItemPrice.Tag = Nothing
        Me.colItemPrice.Text = "Item Price"
        Me.colItemPrice.Width = 100
        '
        'colTotalPrice
        '
        Me.colTotalPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colTotalPrice.CustomSortTag = Nothing
        Me.colTotalPrice.DisplayIndex = 5
        Me.colTotalPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colTotalPrice.Tag = Nothing
        Me.colTotalPrice.Text = "Total Price"
        Me.colTotalPrice.Width = 100
        '
        'colRefinePrice
        '
        Me.colRefinePrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colRefinePrice.CustomSortTag = Nothing
        Me.colRefinePrice.DisplayIndex = 6
        Me.colRefinePrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colRefinePrice.Tag = Nothing
        Me.colRefinePrice.Text = "Refine Price"
        Me.colRefinePrice.Width = 100
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(12, 48)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(30, 13)
        Me.lblPilot.TabIndex = 1
        Me.lblPilot.Text = "Pilot:"
        '
        'cboPilots
        '
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(48, 45)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(165, 21)
        Me.cboPilots.TabIndex = 2
        '
        'chkPerfectRefine
        '
        Me.chkPerfectRefine.AutoSize = True
        Me.chkPerfectRefine.Location = New System.Drawing.Point(259, 47)
        Me.chkPerfectRefine.Name = "chkPerfectRefine"
        Me.chkPerfectRefine.Size = New System.Drawing.Size(94, 17)
        Me.chkPerfectRefine.TabIndex = 3
        Me.chkPerfectRefine.Text = "Perfect Refine"
        Me.chkPerfectRefine.UseVisualStyleBackColor = True
        '
        'lblBaseYieldLbl
        '
        Me.lblBaseYieldLbl.AutoSize = True
        Me.lblBaseYieldLbl.Location = New System.Drawing.Point(500, 61)
        Me.lblBaseYieldLbl.Name = "lblBaseYieldLbl"
        Me.lblBaseYieldLbl.Size = New System.Drawing.Size(60, 13)
        Me.lblBaseYieldLbl.TabIndex = 4
        Me.lblBaseYieldLbl.Text = "Base Yield:"
        '
        'lblNetYieldLbl
        '
        Me.lblNetYieldLbl.AutoSize = True
        Me.lblNetYieldLbl.Location = New System.Drawing.Point(500, 74)
        Me.lblNetYieldLbl.Name = "lblNetYieldLbl"
        Me.lblNetYieldLbl.Size = New System.Drawing.Size(53, 13)
        Me.lblNetYieldLbl.TabIndex = 5
        Me.lblNetYieldLbl.Text = "Net Yield:"
        '
        'lblStandingsLbl
        '
        Me.lblStandingsLbl.AutoSize = True
        Me.lblStandingsLbl.Location = New System.Drawing.Point(500, 87)
        Me.lblStandingsLbl.Name = "lblStandingsLbl"
        Me.lblStandingsLbl.Size = New System.Drawing.Size(57, 13)
        Me.lblStandingsLbl.TabIndex = 6
        Me.lblStandingsLbl.Text = "Standings:"
        '
        'lblStationTakeLbl
        '
        Me.lblStationTakeLbl.AutoSize = True
        Me.lblStationTakeLbl.Location = New System.Drawing.Point(500, 100)
        Me.lblStationTakeLbl.Name = "lblStationTakeLbl"
        Me.lblStationTakeLbl.Size = New System.Drawing.Size(71, 13)
        Me.lblStationTakeLbl.TabIndex = 7
        Me.lblStationTakeLbl.Text = "Station Take:"
        '
        'lblStationTake
        '
        Me.lblStationTake.AutoSize = True
        Me.lblStationTake.Location = New System.Drawing.Point(577, 100)
        Me.lblStationTake.Name = "lblStationTake"
        Me.lblStationTake.Size = New System.Drawing.Size(36, 13)
        Me.lblStationTake.TabIndex = 8
        Me.lblStationTake.Text = "0.00%"
        '
        'lblStandings
        '
        Me.lblStandings.AutoSize = True
        Me.lblStandings.Location = New System.Drawing.Point(577, 87)
        Me.lblStandings.Name = "lblStandings"
        Me.lblStandings.Size = New System.Drawing.Size(28, 13)
        Me.lblStandings.TabIndex = 9
        Me.lblStandings.Text = "0.00"
        '
        'lblNetYield
        '
        Me.lblNetYield.AutoSize = True
        Me.lblNetYield.Location = New System.Drawing.Point(577, 74)
        Me.lblNetYield.Name = "lblNetYield"
        Me.lblNetYield.Size = New System.Drawing.Size(36, 13)
        Me.lblNetYield.TabIndex = 10
        Me.lblNetYield.Text = "0.00%"
        '
        'lblBaseYield
        '
        Me.lblBaseYield.AutoSize = True
        Me.lblBaseYield.Location = New System.Drawing.Point(577, 61)
        Me.lblBaseYield.Name = "lblBaseYield"
        Me.lblBaseYield.Size = New System.Drawing.Size(36, 13)
        Me.lblBaseYield.TabIndex = 11
        Me.lblBaseYield.Text = "0.00%"
        '
        'lblStationLbl
        '
        Me.lblStationLbl.AutoSize = True
        Me.lblStationLbl.Location = New System.Drawing.Point(256, 9)
        Me.lblStationLbl.Name = "lblStationLbl"
        Me.lblStationLbl.Size = New System.Drawing.Size(43, 13)
        Me.lblStationLbl.TabIndex = 12
        Me.lblStationLbl.Text = "Station:"
        '
        'lblStation
        '
        Me.lblStation.AutoSize = True
        Me.lblStation.Location = New System.Drawing.Point(305, 9)
        Me.lblStation.Name = "lblStation"
        Me.lblStation.Size = New System.Drawing.Size(24, 13)
        Me.lblStation.TabIndex = 13
        Me.lblStation.Text = "n/a"
        '
        'lblCorp
        '
        Me.lblCorp.AutoSize = True
        Me.lblCorp.Location = New System.Drawing.Point(305, 22)
        Me.lblCorp.Name = "lblCorp"
        Me.lblCorp.Size = New System.Drawing.Size(24, 13)
        Me.lblCorp.TabIndex = 15
        Me.lblCorp.Text = "n/a"
        '
        'lblCorpLbl
        '
        Me.lblCorpLbl.AutoSize = True
        Me.lblCorpLbl.Location = New System.Drawing.Point(256, 22)
        Me.lblCorpLbl.Name = "lblCorpLbl"
        Me.lblCorpLbl.Size = New System.Drawing.Size(32, 13)
        Me.lblCorpLbl.TabIndex = 14
        Me.lblCorpLbl.Text = "Corp:"
        '
        'nudBaseYield
        '
        Me.nudBaseYield.DecimalPlaces = 2
        Me.nudBaseYield.Location = New System.Drawing.Point(384, 72)
        Me.nudBaseYield.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudBaseYield.Name = "nudBaseYield"
        Me.nudBaseYield.Size = New System.Drawing.Size(74, 20)
        Me.nudBaseYield.TabIndex = 16
        Me.nudBaseYield.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'nudStandings
        '
        Me.nudStandings.DecimalPlaces = 4
        Me.nudStandings.Location = New System.Drawing.Point(384, 98)
        Me.nudStandings.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudStandings.Name = "nudStandings"
        Me.nudStandings.Size = New System.Drawing.Size(74, 20)
        Me.nudStandings.TabIndex = 17
        '
        'chkOverrideBaseYield
        '
        Me.chkOverrideBaseYield.AutoSize = True
        Me.chkOverrideBaseYield.Location = New System.Drawing.Point(259, 73)
        Me.chkOverrideBaseYield.Name = "chkOverrideBaseYield"
        Me.chkOverrideBaseYield.Size = New System.Drawing.Size(119, 17)
        Me.chkOverrideBaseYield.TabIndex = 18
        Me.chkOverrideBaseYield.Text = "Override Base Yield"
        Me.chkOverrideBaseYield.UseVisualStyleBackColor = True
        '
        'chkOverrideStandings
        '
        Me.chkOverrideStandings.AutoSize = True
        Me.chkOverrideStandings.Location = New System.Drawing.Point(259, 99)
        Me.chkOverrideStandings.Name = "chkOverrideStandings"
        Me.chkOverrideStandings.Size = New System.Drawing.Size(116, 17)
        Me.chkOverrideStandings.TabIndex = 19
        Me.chkOverrideStandings.Text = "Override Standings"
        Me.chkOverrideStandings.UseVisualStyleBackColor = True
        '
        'lblRefineMode
        '
        Me.lblRefineMode.AutoSize = True
        Me.lblRefineMode.Location = New System.Drawing.Point(12, 17)
        Me.lblRefineMode.Name = "lblRefineMode"
        Me.lblRefineMode.Size = New System.Drawing.Size(37, 13)
        Me.lblRefineMode.TabIndex = 20
        Me.lblRefineMode.Text = "Mode:"
        '
        'cboRefineMode
        '
        Me.cboRefineMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRefineMode.FormattingEnabled = True
        Me.cboRefineMode.Items.AddRange(New Object() {"Standard (Station)", "Refining Array", "Intensive Refining Array"})
        Me.cboRefineMode.Location = New System.Drawing.Point(48, 14)
        Me.cboRefineMode.Name = "cboRefineMode"
        Me.cboRefineMode.Size = New System.Drawing.Size(165, 21)
        Me.cboRefineMode.TabIndex = 21
        '
        'lblVolumeLbl
        '
        Me.lblVolumeLbl.AutoSize = True
        Me.lblVolumeLbl.Location = New System.Drawing.Point(12, 79)
        Me.lblVolumeLbl.Name = "lblVolumeLbl"
        Me.lblVolumeLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblVolumeLbl.TabIndex = 22
        Me.lblVolumeLbl.Text = "Volume:"
        '
        'lblItemsLbl
        '
        Me.lblItemsLbl.AutoSize = True
        Me.lblItemsLbl.Location = New System.Drawing.Point(12, 100)
        Me.lblItemsLbl.Name = "lblItemsLbl"
        Me.lblItemsLbl.Size = New System.Drawing.Size(35, 13)
        Me.lblItemsLbl.TabIndex = 23
        Me.lblItemsLbl.Text = "Items:"
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.Location = New System.Drawing.Point(63, 79)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(42, 13)
        Me.lblVolume.TabIndex = 24
        Me.lblVolume.Text = "0.00 m³"
        '
        'lblItems
        '
        Me.lblItems.AutoSize = True
        Me.lblItems.Location = New System.Drawing.Point(63, 100)
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(13, 13)
        Me.lblItems.TabIndex = 25
        Me.lblItems.Text = "0"
        '
        'tabRecycle
        '
        Me.tabRecycle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabRecycle.Controls.Add(Me.tabItems)
        Me.tabRecycle.Controls.Add(Me.tabTotals)
        Me.tabRecycle.Location = New System.Drawing.Point(0, 124)
        Me.tabRecycle.Name = "tabRecycle"
        Me.tabRecycle.SelectedIndex = 0
        Me.tabRecycle.Size = New System.Drawing.Size(946, 416)
        Me.tabRecycle.TabIndex = 26
        '
        'tabItems
        '
        Me.tabItems.Controls.Add(Me.clvRecycle)
        Me.tabItems.Location = New System.Drawing.Point(4, 22)
        Me.tabItems.Name = "tabItems"
        Me.tabItems.Padding = New System.Windows.Forms.Padding(3)
        Me.tabItems.Size = New System.Drawing.Size(938, 390)
        Me.tabItems.TabIndex = 0
        Me.tabItems.Text = "Item Analysis"
        Me.tabItems.UseVisualStyleBackColor = True
        '
        'tabTotals
        '
        Me.tabTotals.Controls.Add(Me.clvTotals)
        Me.tabTotals.Location = New System.Drawing.Point(4, 22)
        Me.tabTotals.Name = "tabTotals"
        Me.tabTotals.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTotals.Size = New System.Drawing.Size(938, 390)
        Me.tabTotals.TabIndex = 1
        Me.tabTotals.Text = "Recycling Totals"
        Me.tabTotals.UseVisualStyleBackColor = True
        '
        'clvTotals
        '
        Me.clvTotals.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colMaterial, Me.colStationTake, Me.colWaste, Me.colReceive, Me.colMatPrice, Me.colMatTotal})
        Me.clvTotals.DefaultItemHeight = 20
        Me.clvTotals.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvTotals.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvTotals.Location = New System.Drawing.Point(3, 3)
        Me.clvTotals.MultipleColumnSort = True
        Me.clvTotals.Name = "clvTotals"
        Me.clvTotals.Size = New System.Drawing.Size(932, 384)
        Me.clvTotals.TabIndex = 1
        '
        'colMaterial
        '
        Me.colMaterial.CustomSortTag = Nothing
        Me.colMaterial.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colMaterial.Tag = Nothing
        Me.colMaterial.Text = "Material"
        Me.colMaterial.Width = 300
        '
        'colReceive
        '
        Me.colReceive.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colReceive.CustomSortTag = Nothing
        Me.colReceive.DisplayIndex = 3
        Me.colReceive.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colReceive.Tag = Nothing
        Me.colReceive.Text = "Receivable"
        Me.colReceive.Width = 100
        '
        'colStationTake
        '
        Me.colStationTake.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colStationTake.CustomSortTag = Nothing
        Me.colStationTake.DisplayIndex = 1
        Me.colStationTake.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colStationTake.Tag = Nothing
        Me.colStationTake.Text = "Station Take"
        Me.colStationTake.Width = 100
        '
        'colWaste
        '
        Me.colWaste.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colWaste.CustomSortTag = Nothing
        Me.colWaste.DisplayIndex = 2
        Me.colWaste.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colWaste.Tag = Nothing
        Me.colWaste.Text = "Unrecoverable"
        Me.colWaste.Width = 100
        '
        'colMatPrice
        '
        Me.colMatPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colMatPrice.CustomSortTag = Nothing
        Me.colMatPrice.DisplayIndex = 4
        Me.colMatPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colMatPrice.Tag = Nothing
        Me.colMatPrice.Text = "Price"
        '
        'colMatTotal
        '
        Me.colMatTotal.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colMatTotal.CustomSortTag = Nothing
        Me.colMatTotal.DisplayIndex = 5
        Me.colMatTotal.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colMatTotal.Tag = Nothing
        Me.colMatTotal.Text = "Total"
        '
        'frmRecycleAssets
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(946, 540)
        Me.Controls.Add(Me.tabRecycle)
        Me.Controls.Add(Me.lblItems)
        Me.Controls.Add(Me.lblVolume)
        Me.Controls.Add(Me.lblItemsLbl)
        Me.Controls.Add(Me.lblVolumeLbl)
        Me.Controls.Add(Me.cboRefineMode)
        Me.Controls.Add(Me.lblRefineMode)
        Me.Controls.Add(Me.chkOverrideStandings)
        Me.Controls.Add(Me.chkOverrideBaseYield)
        Me.Controls.Add(Me.nudStandings)
        Me.Controls.Add(Me.nudBaseYield)
        Me.Controls.Add(Me.lblCorp)
        Me.Controls.Add(Me.lblCorpLbl)
        Me.Controls.Add(Me.lblStation)
        Me.Controls.Add(Me.lblStationLbl)
        Me.Controls.Add(Me.lblBaseYield)
        Me.Controls.Add(Me.lblNetYield)
        Me.Controls.Add(Me.lblStandings)
        Me.Controls.Add(Me.lblStationTake)
        Me.Controls.Add(Me.lblStationTakeLbl)
        Me.Controls.Add(Me.lblStandingsLbl)
        Me.Controls.Add(Me.lblNetYieldLbl)
        Me.Controls.Add(Me.lblBaseYieldLbl)
        Me.Controls.Add(Me.chkPerfectRefine)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblPilot)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmRecycleAssets"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Recycling Profitability Calculations"
        CType(Me.nudBaseYield, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStandings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabRecycle.ResumeLayout(False)
        Me.tabItems.ResumeLayout(False)
        Me.tabTotals.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents clvRecycle As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colItem As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colItemPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRefinePrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBatches As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colTotalPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colMetaLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents chkPerfectRefine As System.Windows.Forms.CheckBox
    Friend WithEvents lblBaseYieldLbl As System.Windows.Forms.Label
    Friend WithEvents lblNetYieldLbl As System.Windows.Forms.Label
    Friend WithEvents lblStandingsLbl As System.Windows.Forms.Label
    Friend WithEvents lblStationTakeLbl As System.Windows.Forms.Label
    Friend WithEvents lblStationTake As System.Windows.Forms.Label
    Friend WithEvents lblStandings As System.Windows.Forms.Label
    Friend WithEvents lblNetYield As System.Windows.Forms.Label
    Friend WithEvents lblBaseYield As System.Windows.Forms.Label
    Friend WithEvents lblStationLbl As System.Windows.Forms.Label
    Friend WithEvents lblStation As System.Windows.Forms.Label
    Friend WithEvents lblCorp As System.Windows.Forms.Label
    Friend WithEvents lblCorpLbl As System.Windows.Forms.Label
    Friend WithEvents nudBaseYield As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudStandings As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkOverrideBaseYield As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverrideStandings As System.Windows.Forms.CheckBox
    Friend WithEvents lblRefineMode As System.Windows.Forms.Label
    Friend WithEvents cboRefineMode As System.Windows.Forms.ComboBox
    Friend WithEvents lblVolumeLbl As System.Windows.Forms.Label
    Friend WithEvents lblItemsLbl As System.Windows.Forms.Label
    Friend WithEvents lblVolume As System.Windows.Forms.Label
    Friend WithEvents lblItems As System.Windows.Forms.Label
    Friend WithEvents tabRecycle As System.Windows.Forms.TabControl
    Friend WithEvents tabItems As System.Windows.Forms.TabPage
    Friend WithEvents tabTotals As System.Windows.Forms.TabPage
    Friend WithEvents clvTotals As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colMaterial As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colReceive As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colStationTake As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colWaste As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colMatPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colMatTotal As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
End Class
