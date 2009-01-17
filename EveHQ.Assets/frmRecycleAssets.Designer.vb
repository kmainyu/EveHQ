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
        Me.SuspendLayout()
        '
        'clvRecycle
        '
        Me.clvRecycle.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colItem, Me.colQuantity, Me.colItemPrice, Me.colRefinePrice})
        Me.clvRecycle.DefaultItemHeight = 20
        Me.clvRecycle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvRecycle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvRecycle.Location = New System.Drawing.Point(0, 0)
        Me.clvRecycle.Name = "clvRecycle"
        Me.clvRecycle.Size = New System.Drawing.Size(728, 520)
        Me.clvRecycle.TabIndex = 0
        '
        'colItem
        '
        Me.colItem.CustomSortTag = Nothing
        Me.colItem.Tag = Nothing
        Me.colItem.Text = "Item"
        Me.colItem.Width = 300
        '
        'colQuantity
        '
        Me.colQuantity.CustomSortTag = Nothing
        Me.colQuantity.DisplayIndex = 1
        Me.colQuantity.Tag = Nothing
        Me.colQuantity.Text = "Quantity"
        '
        'colItemPrice
        '
        Me.colItemPrice.CustomSortTag = Nothing
        Me.colItemPrice.DisplayIndex = 2
        Me.colItemPrice.Tag = Nothing
        Me.colItemPrice.Text = "Item Price"
        Me.colItemPrice.Width = 150
        '
        'colRefinePrice
        '
        Me.colRefinePrice.CustomSortTag = Nothing
        Me.colRefinePrice.DisplayIndex = 3
        Me.colRefinePrice.Tag = Nothing
        Me.colRefinePrice.Text = "Refine Price"
        Me.colRefinePrice.Width = 150
        '
        'frmRecycleAssets
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(728, 520)
        Me.Controls.Add(Me.clvRecycle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmRecycleAssets"
        Me.Text = "Recycling Profitability Calculations"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents clvRecycle As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colItem As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colItemPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRefinePrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
End Class
