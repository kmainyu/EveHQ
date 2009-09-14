<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFleetPilot
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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAccept = New System.Windows.Forms.Button
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblPilot = New System.Windows.Forms.Label
        Me.lblFitting = New System.Windows.Forms.Label
        Me.cboFitting = New System.Windows.Forms.ComboBox
        Me.cboPilot = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(245, 123)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(164, 123)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 11
        Me.btnAccept.Text = "Create"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(12, 9)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(308, 23)
        Me.lblDescription.TabIndex = 13
        Me.lblDescription.Text = "Please select a pilot and a fitting to include in the fleet..."
        Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(15, 49)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 14
        Me.lblPilot.Text = "Pilot:"
        '
        'lblFitting
        '
        Me.lblFitting.AutoSize = True
        Me.lblFitting.Location = New System.Drawing.Point(15, 78)
        Me.lblFitting.Name = "lblFitting"
        Me.lblFitting.Size = New System.Drawing.Size(41, 13)
        Me.lblFitting.TabIndex = 15
        Me.lblFitting.Text = "Fitting:"
        '
        'cboFitting
        '
        Me.cboFitting.FormattingEnabled = True
        Me.cboFitting.Location = New System.Drawing.Point(62, 75)
        Me.cboFitting.Name = "cboFitting"
        Me.cboFitting.Size = New System.Drawing.Size(258, 21)
        Me.cboFitting.Sorted = True
        Me.cboFitting.TabIndex = 16
        '
        'cboPilot
        '
        Me.cboPilot.FormattingEnabled = True
        Me.cboPilot.Location = New System.Drawing.Point(62, 46)
        Me.cboPilot.Name = "cboPilot"
        Me.cboPilot.Size = New System.Drawing.Size(258, 21)
        Me.cboPilot.Sorted = True
        Me.cboPilot.TabIndex = 17
        '
        'frmFleetPilot
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(332, 157)
        Me.Controls.Add(Me.cboPilot)
        Me.Controls.Add(Me.cboFitting)
        Me.Controls.Add(Me.lblFitting)
        Me.Controls.Add(Me.lblPilot)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFleetPilot"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Create New Pilot for Fleet"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents lblFitting As System.Windows.Forms.Label
    Friend WithEvents cboFitting As System.Windows.Forms.ComboBox
    Friend WithEvents cboPilot As System.Windows.Forms.ComboBox
End Class
