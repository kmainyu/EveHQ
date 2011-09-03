<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrismSelectionControl
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
        Me.lvwItems = New DevComponents.DotNetBar.Controls.ListViewEx
        Me.colOwner = New System.Windows.Forms.ColumnHeader
        Me.btnAll = New DevComponents.DotNetBar.ButtonX
        Me.btnClear = New DevComponents.DotNetBar.ButtonX
        Me.pnlOwnerSelectionButtons = New DevComponents.DotNetBar.PanelEx
        Me.btnClose = New DevComponents.DotNetBar.ButtonX
        Me.pnlOwnerSelectionButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvwItems
        '
        '
        '
        '
        Me.lvwItems.Border.Class = "ListViewBorder"
        Me.lvwItems.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwItems.CheckBoxes = True
        Me.lvwItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colOwner})
        Me.lvwItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwItems.Location = New System.Drawing.Point(0, 0)
        Me.lvwItems.Name = "lvwItems"
        Me.lvwItems.ShowGroups = False
        Me.lvwItems.Size = New System.Drawing.Size(250, 215)
        Me.lvwItems.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwItems.TabIndex = 0
        Me.lvwItems.UseCompatibleStateImageBehavior = False
        Me.lvwItems.View = System.Windows.Forms.View.Details
        '
        'colOwner
        '
        Me.colOwner.Text = "Owner"
        Me.colOwner.Width = 240
        '
        'btnAll
        '
        Me.btnAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAll.FocusCuesEnabled = False
        Me.btnAll.Location = New System.Drawing.Point(14, 6)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(70, 23)
        Me.btnAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAll.TabIndex = 3
        Me.btnAll.Text = "All"
        '
        'btnClear
        '
        Me.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnClear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnClear.FocusCuesEnabled = False
        Me.btnClear.Location = New System.Drawing.Point(90, 6)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(70, 23)
        Me.btnClear.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnClear.TabIndex = 4
        Me.btnClear.Text = "Clear"
        '
        'pnlOwnerSelectionButtons
        '
        Me.pnlOwnerSelectionButtons.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlOwnerSelectionButtons.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlOwnerSelectionButtons.Controls.Add(Me.btnClose)
        Me.pnlOwnerSelectionButtons.Controls.Add(Me.btnAll)
        Me.pnlOwnerSelectionButtons.Controls.Add(Me.btnClear)
        Me.pnlOwnerSelectionButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlOwnerSelectionButtons.Location = New System.Drawing.Point(0, 215)
        Me.pnlOwnerSelectionButtons.Name = "pnlOwnerSelectionButtons"
        Me.pnlOwnerSelectionButtons.Size = New System.Drawing.Size(250, 35)
        Me.pnlOwnerSelectionButtons.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlOwnerSelectionButtons.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlOwnerSelectionButtons.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlOwnerSelectionButtons.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlOwnerSelectionButtons.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlOwnerSelectionButtons.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlOwnerSelectionButtons.Style.GradientAngle = 90
        Me.pnlOwnerSelectionButtons.TabIndex = 7
        '
        'btnClose
        '
        Me.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnClose.FocusCuesEnabled = False
        Me.btnClose.Location = New System.Drawing.Point(166, 6)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(70, 23)
        Me.btnClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close"
        '
        'PrismSelectionControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lvwItems)
        Me.Controls.Add(Me.pnlOwnerSelectionButtons)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "PrismSelectionControl"
        Me.Size = New System.Drawing.Size(250, 250)
        Me.pnlOwnerSelectionButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvwItems As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents btnAll As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnClear As DevComponents.DotNetBar.ButtonX
    Friend WithEvents pnlOwnerSelectionButtons As DevComponents.DotNetBar.PanelEx
    Friend WithEvents colOwner As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClose As DevComponents.DotNetBar.ButtonX

End Class
