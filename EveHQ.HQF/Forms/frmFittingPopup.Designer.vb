Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmFittingPopup
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
            Me.adtFittings = New DevComponents.AdvTree.AdvTree()
            Me.NodeConnector2 = New DevComponents.AdvTree.NodeConnector()
            Me.ElementStyle2 = New DevComponents.DotNetBar.ElementStyle()
            Me.btnAccept = New DevComponents.DotNetBar.ButtonX()
            Me.btnCancel = New DevComponents.DotNetBar.ButtonX()
            Me.pnlFittingPopup = New DevComponents.DotNetBar.PanelEx()
            CType(Me.adtFittings, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlFittingPopup.SuspendLayout()
            Me.SuspendLayout()
            '
            'adtFittings
            '
            Me.adtFittings.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
            Me.adtFittings.AllowDrop = True
            Me.adtFittings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                            Or System.Windows.Forms.AnchorStyles.Left) _
                                           Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.adtFittings.BackColor = System.Drawing.SystemColors.Window
            '
            '
            '
            Me.adtFittings.BackgroundStyle.Class = "TreeBorderKey"
            Me.adtFittings.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.adtFittings.DragDropEnabled = False
            Me.adtFittings.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Triangle
            Me.adtFittings.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
            Me.adtFittings.Location = New System.Drawing.Point(3, 3)
            Me.adtFittings.MultiSelect = True
            Me.adtFittings.MultiSelectRule = DevComponents.AdvTree.eMultiSelectRule.AnyNode
            Me.adtFittings.Name = "adtFittings"
            Me.adtFittings.NodesConnector = Me.NodeConnector2
            Me.adtFittings.NodeSpacing = 1
            Me.adtFittings.NodeStyle = Me.ElementStyle2
            Me.adtFittings.PathSeparator = ";"
            Me.adtFittings.Size = New System.Drawing.Size(277, 384)
            Me.adtFittings.Styles.Add(Me.ElementStyle2)
            Me.adtFittings.TabIndex = 8
            Me.adtFittings.Text = "AdvTree1"
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
            'btnAccept
            '
            Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnAccept.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnAccept.Enabled = False
            Me.btnAccept.Location = New System.Drawing.Point(124, 393)
            Me.btnAccept.Name = "btnAccept"
            Me.btnAccept.Size = New System.Drawing.Size(75, 18)
            Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnAccept.TabIndex = 9
            Me.btnAccept.Text = "Accept"
            '
            'btnCancel
            '
            Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnCancel.Location = New System.Drawing.Point(205, 393)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 18)
            Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnCancel.TabIndex = 10
            Me.btnCancel.Text = "Cancel"
            '
            'pnlFittingPopup
            '
            Me.pnlFittingPopup.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlFittingPopup.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlFittingPopup.Controls.Add(Me.adtFittings)
            Me.pnlFittingPopup.Controls.Add(Me.btnCancel)
            Me.pnlFittingPopup.Controls.Add(Me.btnAccept)
            Me.pnlFittingPopup.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlFittingPopup.Location = New System.Drawing.Point(0, 0)
            Me.pnlFittingPopup.Name = "pnlFittingPopup"
            Me.pnlFittingPopup.Size = New System.Drawing.Size(283, 414)
            Me.pnlFittingPopup.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlFittingPopup.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlFittingPopup.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlFittingPopup.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlFittingPopup.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlFittingPopup.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlFittingPopup.Style.GradientAngle = 90
            Me.pnlFittingPopup.TabIndex = 11
            '
            'frmFittingPopup
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(283, 414)
            Me.Controls.Add(Me.pnlFittingPopup)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmFittingPopup"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.Text = "Select New Fitting..."
            CType(Me.adtFittings, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlFittingPopup.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents adtFittings As DevComponents.AdvTree.AdvTree
        Friend WithEvents NodeConnector2 As DevComponents.AdvTree.NodeConnector
        Friend WithEvents ElementStyle2 As DevComponents.DotNetBar.ElementStyle
        Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
        Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
        Friend WithEvents pnlFittingPopup As DevComponents.DotNetBar.PanelEx
    End Class
End NameSpace