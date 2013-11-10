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
            Me.nudQuantity = New System.Windows.Forms.NumericUpDown
            Me.btnAccept = New DevComponents.DotNetBar.ButtonX
            Me.btnCancel = New DevComponents.DotNetBar.ButtonX
            Me.pnlQuantity = New DevComponents.DotNetBar.PanelEx
            CType(Me.nudQuantity, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlQuantity.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblDetails
            '
            Me.lblDetails.Location = New System.Drawing.Point(12, 10)
            Me.lblDetails.Name = "lblDetails"
            Me.lblDetails.Size = New System.Drawing.Size(236, 30)
            Me.lblDetails.TabIndex = 0
            Me.lblDetails.Text = "Please enter the new quantity:"
            '
            'nudQuantity
            '
            Me.nudQuantity.Location = New System.Drawing.Point(82, 50)
            Me.nudQuantity.Name = "nudQuantity"
            Me.nudQuantity.Size = New System.Drawing.Size(92, 21)
            Me.nudQuantity.TabIndex = 1
            Me.nudQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'btnAccept
            '
            Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnAccept.Location = New System.Drawing.Point(88, 84)
            Me.btnAccept.Name = "btnAccept"
            Me.btnAccept.Size = New System.Drawing.Size(75, 23)
            Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnAccept.TabIndex = 2
            Me.btnAccept.Text = "Accept"
            '
            'btnCancel
            '
            Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(169, 84)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnCancel.TabIndex = 3
            Me.btnCancel.Text = "Cancel"
            '
            'pnlQuantity
            '
            Me.pnlQuantity.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlQuantity.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlQuantity.Controls.Add(Me.lblDetails)
            Me.pnlQuantity.Controls.Add(Me.btnCancel)
            Me.pnlQuantity.Controls.Add(Me.nudQuantity)
            Me.pnlQuantity.Controls.Add(Me.btnAccept)
            Me.pnlQuantity.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlQuantity.Location = New System.Drawing.Point(0, 0)
            Me.pnlQuantity.Name = "pnlQuantity"
            Me.pnlQuantity.Size = New System.Drawing.Size(251, 118)
            Me.pnlQuantity.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlQuantity.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlQuantity.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlQuantity.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlQuantity.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlQuantity.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlQuantity.Style.GradientAngle = 90
            Me.pnlQuantity.TabIndex = 4
            '
            'frmSelectQuantity
            '
            Me.AcceptButton = Me.btnAccept
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(251, 118)
            Me.Controls.Add(Me.pnlQuantity)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmSelectQuantity"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Alter Bay Quantity"
            CType(Me.nudQuantity, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlQuantity.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblDetails As System.Windows.Forms.Label
        Friend WithEvents nudQuantity As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
        Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
        Friend WithEvents pnlQuantity As DevComponents.DotNetBar.PanelEx
    End Class
End NameSpace