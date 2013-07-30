<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectLocation
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
        Me.pnlLocation = New DevComponents.DotNetBar.PanelEx
        Me.cboLocations = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX
        Me.chkIncludeBPOs = New System.Windows.Forms.CheckBox
        Me.pnlLocation.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblDetails
        '
        Me.lblDetails.Location = New System.Drawing.Point(12, 10)
        Me.lblDetails.Name = "lblDetails"
        Me.lblDetails.Size = New System.Drawing.Size(236, 30)
        Me.lblDetails.TabIndex = 0
        Me.lblDetails.Text = "Please select the location of the blueprints:"
        '
        'pnlLocation
        '
        Me.pnlLocation.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlLocation.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlLocation.Controls.Add(Me.cboLocations)
        Me.pnlLocation.Controls.Add(Me.btnCancel)
        Me.pnlLocation.Controls.Add(Me.btnAccept)
        Me.pnlLocation.Controls.Add(Me.lblDetails)
        Me.pnlLocation.Controls.Add(Me.chkIncludeBPOs)
        Me.pnlLocation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlLocation.Location = New System.Drawing.Point(0, 0)
        Me.pnlLocation.Name = "pnlLocation"
        Me.pnlLocation.Size = New System.Drawing.Size(331, 130)
        Me.pnlLocation.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlLocation.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlLocation.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlLocation.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlLocation.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlLocation.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlLocation.Style.GradientAngle = 90
        Me.pnlLocation.TabIndex = 5
        '
        'cboLocations
        '
        Me.cboLocations.DisplayMember = "Text"
        Me.cboLocations.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboLocations.FormattingEnabled = True
        Me.cboLocations.ItemHeight = 15
        Me.cboLocations.Location = New System.Drawing.Point(15, 38)
        Me.cboLocations.Name = "cboLocations"
        Me.cboLocations.Size = New System.Drawing.Size(304, 21)
        Me.cboLocations.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboLocations.TabIndex = 7
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.CallBasePaintBackground = True
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(244, 95)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.CallBasePaintBackground = True
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(163, 95)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAccept.TabIndex = 5
        Me.btnAccept.Text = "Accept"
        '
        'chkIncludeBPOs
        '
        Me.chkIncludeBPOs.AutoSize = True
        Me.chkIncludeBPOs.Location = New System.Drawing.Point(15, 73)
        Me.chkIncludeBPOs.Name = "chkIncludeBPOs"
        Me.chkIncludeBPOs.Size = New System.Drawing.Size(150, 17)
        Me.chkIncludeBPOs.TabIndex = 8
        Me.chkIncludeBPOs.Text = "Include BPOs in update"
        Me.chkIncludeBPOs.UseVisualStyleBackColor = True
        '
        'frmSelectItem
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(331, 130)
        Me.Controls.Add(Me.pnlLocation)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectLocation"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select Location"
        Me.pnlLocation.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblDetails As System.Windows.Forms.Label
    Friend WithEvents pnlLocation As DevComponents.DotNetBar.PanelEx
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboLocations As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents chkIncludeBPOs As System.Windows.Forms.CheckBox
End Class
