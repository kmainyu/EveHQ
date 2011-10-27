<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShipWidgetModules
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.adtSlots = New DevComponents.AdvTree.AdvTree()
        Me.colModuleName = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.SlotStyle = New DevComponents.DotNetBar.ElementStyle()
        Me.HeaderStyle = New DevComponents.DotNetBar.ElementStyle()
        Me.Cell2 = New DevComponents.AdvTree.Cell()
        Me.Cell3 = New DevComponents.AdvTree.Cell()
        Me.Cell4 = New DevComponents.AdvTree.Cell()
        Me.SlotTip = New DevComponents.DotNetBar.SuperTooltip()
        CType(Me.adtSlots, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'adtSlots
        '
        Me.adtSlots.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtSlots.AllowDrop = True
        Me.adtSlots.AllowUserToResizeColumns = False
        Me.adtSlots.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtSlots.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtSlots.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtSlots.Columns.Add(Me.colModuleName)
        Me.adtSlots.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtSlots.DragDropNodeCopyEnabled = False
        Me.adtSlots.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Ellipse
        Me.adtSlots.ExpandWidth = 0
        Me.adtSlots.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0, Byte))
        Me.adtSlots.Indent = 2
        Me.adtSlots.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtSlots.Location = New System.Drawing.Point(0, 0)
        Me.adtSlots.Margin = New System.Windows.Forms.Padding(2)
        Me.adtSlots.MultiSelect = True
        Me.adtSlots.Name = "adtSlots"
        Me.adtSlots.NodesConnector = Me.NodeConnector1
        Me.adtSlots.NodeSpacing = 2
        Me.adtSlots.PathSeparator = ";"
        Me.adtSlots.SelectionBox = False
        Me.adtSlots.Size = New System.Drawing.Size(210, 150)
        Me.adtSlots.Styles.Add(Me.SlotStyle)
        Me.adtSlots.Styles.Add(Me.HeaderStyle)
        Me.adtSlots.TabIndex = 9
        Me.adtSlots.Text = "AdvTree1"
        '
        'colModuleName
        '
        Me.colModuleName.DisplayIndex = 1
        Me.colModuleName.Name = "colModuleName"
        Me.colModuleName.SortingEnabled = False
        Me.colModuleName.Text = "Module Name"
        Me.colModuleName.Width.Absolute = 190
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        Me.NodeConnector1.LineWidth = 0
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
        Me.SlotStyle.Class = ""
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
        Me.HeaderStyle.Class = ""
        Me.HeaderStyle.CornerDiameter = 4
        Me.HeaderStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.HeaderStyle.Description = "Silver"
        Me.HeaderStyle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HeaderStyle.Name = "HeaderStyle"
        Me.HeaderStyle.TextColor = System.Drawing.Color.Black
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
        'SlotTip
        '
        Me.SlotTip.DefaultFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SlotTip.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.SlotTip.PositionBelowControl = False
        '
        'ShipWidgetModules
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(5.0!, 11.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.adtSlots)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "ShipWidgetModules"
        Me.Size = New System.Drawing.Size(210, 150)
        CType(Me.adtSlots, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents adtSlots As DevComponents.AdvTree.AdvTree
    Friend WithEvents colModuleName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents Cell2 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell3 As DevComponents.AdvTree.Cell
    Friend WithEvents Cell4 As DevComponents.AdvTree.Cell
    Friend WithEvents SlotStyle As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents HeaderStyle As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents SlotTip As DevComponents.DotNetBar.SuperTooltip

End Class
