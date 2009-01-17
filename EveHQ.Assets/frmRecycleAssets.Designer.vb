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
        Me.colQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colItemPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRefinePrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBatches = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTotalPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colMetaLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.SuspendLayout()
        '
        'clvRecycle
        '
        Me.clvRecycle.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colItem, Me.colMetaLevel, Me.colQuantity, Me.colBatches, Me.colItemPrice, Me.colTotalPrice, Me.colRefinePrice})
        Me.clvRecycle.DefaultItemHeight = 20
        Me.clvRecycle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvRecycle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvRecycle.Location = New System.Drawing.Point(0, 0)
        Me.clvRecycle.MultipleColumnSort = True
        Me.clvRecycle.Name = "clvRecycle"
        Me.clvRecycle.Size = New System.Drawing.Size(946, 540)
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
        'colBatches
        '
        Me.colBatches.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colBatches.CustomSortTag = Nothing
        Me.colBatches.DisplayIndex = 3
        Me.colBatches.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colBatches.Tag = Nothing
        Me.colBatches.Text = "Batches"
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
        'colMetaLevel
        '
        Me.colMetaLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colMetaLevel.CustomSortTag = Nothing
        Me.colMetaLevel.DisplayIndex = 1
        Me.colMetaLevel.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colMetaLevel.Tag = Nothing
        Me.colMetaLevel.Text = "Meta Level"
        '
        'frmRecycleAssets
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(946, 540)
        Me.Controls.Add(Me.clvRecycle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmRecycleAssets"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Recycling Profitability Calculations"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents clvRecycle As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colItem As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colItemPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRefinePrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBatches As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colTotalPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colMetaLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
End Class
