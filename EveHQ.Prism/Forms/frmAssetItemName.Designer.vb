Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmAssetItemName
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
            Me.lblDescription = New System.Windows.Forms.Label
            Me.txtAssetItemName = New System.Windows.Forms.TextBox
            Me.pnlCustomAssetName = New DevComponents.DotNetBar.PanelEx
            Me.btnAccept = New DevComponents.DotNetBar.ButtonX
            Me.btnCancel = New DevComponents.DotNetBar.ButtonX
            Me.pnlCustomAssetName.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblDescription
            '
            Me.lblDescription.Location = New System.Drawing.Point(12, 9)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(213, 44)
            Me.lblDescription.TabIndex = 0
            Me.lblDescription.Text = "Please enter a name for assetID #"
            '
            'txtAssetItemName
            '
            Me.txtAssetItemName.Location = New System.Drawing.Point(12, 56)
            Me.txtAssetItemName.MaxLength = 50
            Me.txtAssetItemName.Name = "txtAssetItemName"
            Me.txtAssetItemName.Size = New System.Drawing.Size(213, 21)
            Me.txtAssetItemName.TabIndex = 1
            '
            'pnlCustomAssetName
            '
            Me.pnlCustomAssetName.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlCustomAssetName.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlCustomAssetName.Controls.Add(Me.btnCancel)
            Me.pnlCustomAssetName.Controls.Add(Me.btnAccept)
            Me.pnlCustomAssetName.Controls.Add(Me.lblDescription)
            Me.pnlCustomAssetName.Controls.Add(Me.txtAssetItemName)
            Me.pnlCustomAssetName.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlCustomAssetName.Location = New System.Drawing.Point(0, 0)
            Me.pnlCustomAssetName.Name = "pnlCustomAssetName"
            Me.pnlCustomAssetName.Size = New System.Drawing.Size(232, 130)
            Me.pnlCustomAssetName.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlCustomAssetName.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlCustomAssetName.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlCustomAssetName.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlCustomAssetName.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlCustomAssetName.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlCustomAssetName.Style.GradientAngle = 90
            Me.pnlCustomAssetName.TabIndex = 4
            '
            'btnAccept
            '
            Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnAccept.CallBasePaintBackground = True
            Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnAccept.Location = New System.Drawing.Point(69, 92)
            Me.btnAccept.Name = "btnAccept"
            Me.btnAccept.Size = New System.Drawing.Size(75, 23)
            Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnAccept.TabIndex = 4
            Me.btnAccept.Text = "Accept"
            '
            'btnCancel
            '
            Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnCancel.CallBasePaintBackground = True
            Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(150, 92)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "Cancel"
            '
            'frmAssetItemName
            '
            Me.AcceptButton = Me.btnAccept
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(232, 130)
            Me.Controls.Add(Me.pnlCustomAssetName)
            Me.DoubleBuffered = True
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmAssetItemName"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Add Custom Asset Name"
            Me.pnlCustomAssetName.ResumeLayout(False)
            Me.pnlCustomAssetName.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents txtAssetItemName As System.Windows.Forms.TextBox
        Friend WithEvents pnlCustomAssetName As DevComponents.DotNetBar.PanelEx
        Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
        Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    End Class
End NameSpace