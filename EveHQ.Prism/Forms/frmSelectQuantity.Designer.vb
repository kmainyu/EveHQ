Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmSelectQuantity
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
            Me.lblDetails = New System.Windows.Forms.Label
            Me.pnlSelectQuantity = New DevComponents.DotNetBar.PanelEx
            Me.nudQuantity = New DevComponents.Editors.IntegerInput
            Me.btnCancel = New DevComponents.DotNetBar.ButtonX
            Me.btnAccept = New DevComponents.DotNetBar.ButtonX
            Me.pnlSelectQuantity.SuspendLayout()
            CType(Me.nudQuantity, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblDetails
            '
            Me.lblDetails.Location = New System.Drawing.Point(12, 10)
            Me.lblDetails.Name = "lblDetails"
            Me.lblDetails.Size = New System.Drawing.Size(236, 24)
            Me.lblDetails.TabIndex = 0
            Me.lblDetails.Text = "Please enter the new quantity:"
            '
            'pnlSelectQuantity
            '
            Me.pnlSelectQuantity.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlSelectQuantity.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlSelectQuantity.Controls.Add(Me.nudQuantity)
            Me.pnlSelectQuantity.Controls.Add(Me.btnCancel)
            Me.pnlSelectQuantity.Controls.Add(Me.btnAccept)
            Me.pnlSelectQuantity.Controls.Add(Me.lblDetails)
            Me.pnlSelectQuantity.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlSelectQuantity.Location = New System.Drawing.Point(0, 0)
            Me.pnlSelectQuantity.Name = "pnlSelectQuantity"
            Me.pnlSelectQuantity.Size = New System.Drawing.Size(252, 104)
            Me.pnlSelectQuantity.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlSelectQuantity.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlSelectQuantity.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlSelectQuantity.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlSelectQuantity.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlSelectQuantity.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlSelectQuantity.Style.GradientAngle = 90
            Me.pnlSelectQuantity.TabIndex = 4
            '
            'nudQuantity
            '
            '
            '
            '
            Me.nudQuantity.BackgroundStyle.Class = "DateTimeInputBackground"
            Me.nudQuantity.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.nudQuantity.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
            Me.nudQuantity.Location = New System.Drawing.Point(70, 37)
            Me.nudQuantity.MaxValue = 999999999
            Me.nudQuantity.MinValue = 0
            Me.nudQuantity.Name = "nudQuantity"
            Me.nudQuantity.ShowUpDown = True
            Me.nudQuantity.Size = New System.Drawing.Size(112, 21)
            Me.nudQuantity.TabIndex = 6
            Me.nudQuantity.Value = 1
            '
            'btnCancel
            '
            Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnCancel.CallBasePaintBackground = True
            Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(173, 73)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "Cancel"
            '
            'btnAccept
            '
            Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnAccept.CallBasePaintBackground = True
            Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnAccept.Location = New System.Drawing.Point(92, 73)
            Me.btnAccept.Name = "btnAccept"
            Me.btnAccept.Size = New System.Drawing.Size(75, 23)
            Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnAccept.TabIndex = 4
            Me.btnAccept.Text = "Accept"
            '
            'frmSelectQuantity
            '
            Me.AcceptButton = Me.btnAccept
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(252, 104)
            Me.Controls.Add(Me.pnlSelectQuantity)
            Me.DoubleBuffered = True
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmSelectQuantity"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Alter Item Quantity"
            Me.pnlSelectQuantity.ResumeLayout(False)
            CType(Me.nudQuantity, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblDetails As System.Windows.Forms.Label
        Friend WithEvents pnlSelectQuantity As DevComponents.DotNetBar.PanelEx
        Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
        Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
        Friend WithEvents nudQuantity As DevComponents.Editors.IntegerInput
    End Class
End NameSpace