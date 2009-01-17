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
        Me.SuspendLayout()
        '
        'clvRecycle
        '
        Me.clvRecycle.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colItem, Me.colMetaLevel, Me.colQuantity, Me.colBatches, Me.colItemPrice, Me.colTotalPrice, Me.colRefinePrice})
        Me.clvRecycle.DefaultItemHeight = 20
        Me.clvRecycle.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.clvRecycle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvRecycle.Location = New System.Drawing.Point(0, 84)
        Me.clvRecycle.MultipleColumnSort = True
        Me.clvRecycle.Name = "clvRecycle"
        Me.clvRecycle.Size = New System.Drawing.Size(946, 456)
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
        Me.lblPilot.Location = New System.Drawing.Point(12, 15)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(30, 13)
        Me.lblPilot.TabIndex = 1
        Me.lblPilot.Text = "Pilot:"
        '
        'cboPilots
        '
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(48, 12)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(165, 21)
        Me.cboPilots.TabIndex = 2
        '
        'chkPerfectRefine
        '
        Me.chkPerfectRefine.AutoSize = True
        Me.chkPerfectRefine.Location = New System.Drawing.Point(219, 14)
        Me.chkPerfectRefine.Name = "chkPerfectRefine"
        Me.chkPerfectRefine.Size = New System.Drawing.Size(94, 17)
        Me.chkPerfectRefine.TabIndex = 3
        Me.chkPerfectRefine.Text = "Perfect Refine"
        Me.chkPerfectRefine.UseVisualStyleBackColor = True
        '
        'lblBaseYieldLbl
        '
        Me.lblBaseYieldLbl.AutoSize = True
        Me.lblBaseYieldLbl.Location = New System.Drawing.Point(352, 15)
        Me.lblBaseYieldLbl.Name = "lblBaseYieldLbl"
        Me.lblBaseYieldLbl.Size = New System.Drawing.Size(60, 13)
        Me.lblBaseYieldLbl.TabIndex = 4
        Me.lblBaseYieldLbl.Text = "Base Yield:"
        '
        'lblNetYieldLbl
        '
        Me.lblNetYieldLbl.AutoSize = True
        Me.lblNetYieldLbl.Location = New System.Drawing.Point(352, 28)
        Me.lblNetYieldLbl.Name = "lblNetYieldLbl"
        Me.lblNetYieldLbl.Size = New System.Drawing.Size(53, 13)
        Me.lblNetYieldLbl.TabIndex = 5
        Me.lblNetYieldLbl.Text = "Net Yield:"
        '
        'lblStandingsLbl
        '
        Me.lblStandingsLbl.AutoSize = True
        Me.lblStandingsLbl.Location = New System.Drawing.Point(352, 41)
        Me.lblStandingsLbl.Name = "lblStandingsLbl"
        Me.lblStandingsLbl.Size = New System.Drawing.Size(57, 13)
        Me.lblStandingsLbl.TabIndex = 6
        Me.lblStandingsLbl.Text = "Standings:"
        '
        'lblStationTakeLbl
        '
        Me.lblStationTakeLbl.AutoSize = True
        Me.lblStationTakeLbl.Location = New System.Drawing.Point(352, 54)
        Me.lblStationTakeLbl.Name = "lblStationTakeLbl"
        Me.lblStationTakeLbl.Size = New System.Drawing.Size(71, 13)
        Me.lblStationTakeLbl.TabIndex = 7
        Me.lblStationTakeLbl.Text = "Station Take:"
        '
        'lblStationTake
        '
        Me.lblStationTake.AutoSize = True
        Me.lblStationTake.Location = New System.Drawing.Point(429, 54)
        Me.lblStationTake.Name = "lblStationTake"
        Me.lblStationTake.Size = New System.Drawing.Size(36, 13)
        Me.lblStationTake.TabIndex = 8
        Me.lblStationTake.Text = "0.00%"
        '
        'lblStandings
        '
        Me.lblStandings.AutoSize = True
        Me.lblStandings.Location = New System.Drawing.Point(429, 41)
        Me.lblStandings.Name = "lblStandings"
        Me.lblStandings.Size = New System.Drawing.Size(28, 13)
        Me.lblStandings.TabIndex = 9
        Me.lblStandings.Text = "0.00"
        '
        'lblNetYield
        '
        Me.lblNetYield.AutoSize = True
        Me.lblNetYield.Location = New System.Drawing.Point(429, 28)
        Me.lblNetYield.Name = "lblNetYield"
        Me.lblNetYield.Size = New System.Drawing.Size(36, 13)
        Me.lblNetYield.TabIndex = 10
        Me.lblNetYield.Text = "0.00%"
        '
        'lblBaseYield
        '
        Me.lblBaseYield.AutoSize = True
        Me.lblBaseYield.Location = New System.Drawing.Point(429, 15)
        Me.lblBaseYield.Name = "lblBaseYield"
        Me.lblBaseYield.Size = New System.Drawing.Size(36, 13)
        Me.lblBaseYield.TabIndex = 11
        Me.lblBaseYield.Text = "0.00%"
        '
        'lblStationLbl
        '
        Me.lblStationLbl.AutoSize = True
        Me.lblStationLbl.Location = New System.Drawing.Point(12, 41)
        Me.lblStationLbl.Name = "lblStationLbl"
        Me.lblStationLbl.Size = New System.Drawing.Size(43, 13)
        Me.lblStationLbl.TabIndex = 12
        Me.lblStationLbl.Text = "Station:"
        '
        'lblStation
        '
        Me.lblStation.AutoSize = True
        Me.lblStation.Location = New System.Drawing.Point(61, 41)
        Me.lblStation.Name = "lblStation"
        Me.lblStation.Size = New System.Drawing.Size(24, 13)
        Me.lblStation.TabIndex = 13
        Me.lblStation.Text = "n/a"
        '
        'lblCorp
        '
        Me.lblCorp.AutoSize = True
        Me.lblCorp.Location = New System.Drawing.Point(61, 54)
        Me.lblCorp.Name = "lblCorp"
        Me.lblCorp.Size = New System.Drawing.Size(24, 13)
        Me.lblCorp.TabIndex = 15
        Me.lblCorp.Text = "n/a"
        '
        'lblCorpLbl
        '
        Me.lblCorpLbl.AutoSize = True
        Me.lblCorpLbl.Location = New System.Drawing.Point(12, 54)
        Me.lblCorpLbl.Name = "lblCorpLbl"
        Me.lblCorpLbl.Size = New System.Drawing.Size(32, 13)
        Me.lblCorpLbl.TabIndex = 14
        Me.lblCorpLbl.Text = "Corp:"
        '
        'frmRecycleAssets
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(946, 540)
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
        Me.Controls.Add(Me.clvRecycle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmRecycleAssets"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Recycling Profitability Calculations"
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
End Class
