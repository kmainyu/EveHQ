<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModifyPriceGroupItem
	Inherits DevComponents.DotNetBar.Office2007Form

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
        Me.pnlMPGI = New DevComponents.DotNetBar.PanelEx()
        Me.btnDelete = New DevComponents.DotNetBar.ButtonX()
        Me.btnClear = New DevComponents.DotNetBar.ButtonX()
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX()
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX()
        Me.adtSelection = New DevComponents.AdvTree.AdvTree()
        Me.colSelectedItems = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector2 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle2 = New DevComponents.DotNetBar.ElementStyle()
        Me.btnAddGroup = New DevComponents.DotNetBar.ButtonX()
        Me.adtItems = New DevComponents.AdvTree.AdvTree()
        Me.colGroup = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle1 = New DevComponents.DotNetBar.ElementStyle()
        Me.pnlMPGI.SuspendLayout()
        CType(Me.adtSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.adtItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlMPGI
        '
        Me.pnlMPGI.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlMPGI.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlMPGI.Controls.Add(Me.btnDelete)
        Me.pnlMPGI.Controls.Add(Me.btnClear)
        Me.pnlMPGI.Controls.Add(Me.btnCancel)
        Me.pnlMPGI.Controls.Add(Me.btnAccept)
        Me.pnlMPGI.Controls.Add(Me.adtSelection)
        Me.pnlMPGI.Controls.Add(Me.btnAddGroup)
        Me.pnlMPGI.Controls.Add(Me.adtItems)
        Me.pnlMPGI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMPGI.Location = New System.Drawing.Point(0, 0)
        Me.pnlMPGI.Name = "pnlMPGI"
        Me.pnlMPGI.Size = New System.Drawing.Size(864, 576)
        Me.pnlMPGI.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlMPGI.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlMPGI.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlMPGI.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlMPGI.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlMPGI.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlMPGI.Style.GradientAngle = 90
        Me.pnlMPGI.TabIndex = 0
        '
        'btnDelete
        '
        Me.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnDelete.Location = New System.Drawing.Point(500, 548)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnDelete.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnDelete.TabIndex = 6
        Me.btnDelete.Text = "Delete"
        '
        'btnClear
        '
        Me.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnClear.Location = New System.Drawing.Point(581, 548)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnClear.TabIndex = 5
        Me.btnClear.Text = "Clear All"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.Location = New System.Drawing.Point(662, 548)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(777, 548)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAccept.TabIndex = 3
        Me.btnAccept.Text = "Accept"
        '
        'adtSelection
        '
        Me.adtSelection.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtSelection.AllowDrop = True
        Me.adtSelection.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtSelection.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtSelection.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtSelection.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtSelection.Columns.Add(Me.colSelectedItems)
        Me.adtSelection.DragDropEnabled = False
        Me.adtSelection.ExpandWidth = 0
        Me.adtSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.adtSelection.HideSelection = True
        Me.adtSelection.HotTracking = True
        Me.adtSelection.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtSelection.Location = New System.Drawing.Point(500, 0)
        Me.adtSelection.MultiSelect = True
        Me.adtSelection.Name = "adtSelection"
        Me.adtSelection.NodesConnector = Me.NodeConnector2
        Me.adtSelection.NodeStyle = Me.ElementStyle2
        Me.adtSelection.PathSeparator = ";"
        Me.adtSelection.Size = New System.Drawing.Size(366, 544)
        Me.adtSelection.Styles.Add(Me.ElementStyle2)
        Me.adtSelection.TabIndex = 2
        Me.adtSelection.Text = "AdvTree1"
        '
        'colSelectedItems
        '
        Me.colSelectedItems.Name = "colSelectedItems"
        Me.colSelectedItems.SortingEnabled = False
        Me.colSelectedItems.Text = "Selected Items"
        Me.colSelectedItems.Width.Absolute = 340
        '
        'NodeConnector2
        '
        Me.NodeConnector2.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle2
        '
        Me.ElementStyle2.Class = ""
        Me.ElementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle2.Name = "ElementStyle2"
        Me.ElementStyle2.TextColor = System.Drawing.SystemColors.ControlText
        '
        'btnAddGroup
        '
        Me.btnAddGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAddGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAddGroup.Enabled = False
        Me.btnAddGroup.Location = New System.Drawing.Point(47, 548)
        Me.btnAddGroup.Name = "btnAddGroup"
        Me.btnAddGroup.Size = New System.Drawing.Size(400, 23)
        Me.btnAddGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAddGroup.TabIndex = 1
        Me.btnAddGroup.Text = "Selection Required"
        '
        'adtItems
        '
        Me.adtItems.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtItems.AllowDrop = True
        Me.adtItems.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.adtItems.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtItems.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtItems.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtItems.Columns.Add(Me.colGroup)
        Me.adtItems.DragDropEnabled = False
        Me.adtItems.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Triangle
        Me.adtItems.HideSelection = True
        Me.adtItems.HotTracking = True
        Me.adtItems.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtItems.Location = New System.Drawing.Point(0, 0)
        Me.adtItems.MultiSelect = True
        Me.adtItems.Name = "adtItems"
        Me.adtItems.NodesConnector = Me.NodeConnector1
        Me.adtItems.NodeStyle = Me.ElementStyle1
        Me.adtItems.PathSeparator = ";"
        Me.adtItems.Size = New System.Drawing.Size(494, 544)
        Me.adtItems.Styles.Add(Me.ElementStyle1)
        Me.adtItems.TabIndex = 0
        Me.adtItems.Text = "AdvTree1"
        '
        'colGroup
        '
        Me.colGroup.Name = "colGroup"
        Me.colGroup.SortingEnabled = False
        Me.colGroup.Text = "Market Group/Item"
        Me.colGroup.Width.Absolute = 470
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle1
        '
        Me.ElementStyle1.Class = ""
        Me.ElementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle1.Name = "ElementStyle1"
        Me.ElementStyle1.TextColor = System.Drawing.SystemColors.ControlText
        '
        'frmModifyPriceGroupItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(864, 576)
        Me.Controls.Add(Me.pnlMPGI)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmModifyPriceGroupItem"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Market Price Group Item"
        Me.pnlMPGI.ResumeLayout(False)
        CType(Me.adtSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.adtItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlMPGI As DevComponents.DotNetBar.PanelEx
    Friend WithEvents adtItems As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle1 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colGroup As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents btnAddGroup As DevComponents.DotNetBar.ButtonX
    Friend WithEvents adtSelection As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector2 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle2 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents colSelectedItems As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents btnDelete As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnClear As DevComponents.DotNetBar.ButtonX
End Class
