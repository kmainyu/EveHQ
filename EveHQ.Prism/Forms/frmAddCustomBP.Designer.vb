<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddCustomBP
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
        Me.nudPELevel = New System.Windows.Forms.NumericUpDown
        Me.nudMELevel = New System.Windows.Forms.NumericUpDown
        Me.lblPELevel = New System.Windows.Forms.Label
        Me.lblMELevel = New System.Windows.Forms.Label
        Me.cboBPs = New System.Windows.Forms.ComboBox
        Me.pbBP = New System.Windows.Forms.PictureBox
        CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(313, 174)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 48
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(232, 174)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 47
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'nudPELevel
        '
        Me.nudPELevel.Location = New System.Drawing.Point(272, 117)
        Me.nudPELevel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudPELevel.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.nudPELevel.Name = "nudPELevel"
        Me.nudPELevel.Size = New System.Drawing.Size(87, 20)
        Me.nudPELevel.TabIndex = 40
        Me.nudPELevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudMELevel
        '
        Me.nudMELevel.Location = New System.Drawing.Point(82, 117)
        Me.nudMELevel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMELevel.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.nudMELevel.Name = "nudMELevel"
        Me.nudMELevel.Size = New System.Drawing.Size(87, 20)
        Me.nudMELevel.TabIndex = 39
        Me.nudMELevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblPELevel
        '
        Me.lblPELevel.AutoSize = True
        Me.lblPELevel.Location = New System.Drawing.Point(213, 119)
        Me.lblPELevel.Name = "lblPELevel"
        Me.lblPELevel.Size = New System.Drawing.Size(53, 13)
        Me.lblPELevel.TabIndex = 36
        Me.lblPELevel.Text = "PE Level:"
        '
        'lblMELevel
        '
        Me.lblMELevel.AutoSize = True
        Me.lblMELevel.Location = New System.Drawing.Point(21, 119)
        Me.lblMELevel.Name = "lblMELevel"
        Me.lblMELevel.Size = New System.Drawing.Size(55, 13)
        Me.lblMELevel.TabIndex = 33
        Me.lblMELevel.Text = "ME Level:"
        '
        'cboBPs
        '
        Me.cboBPs.FormattingEnabled = True
        Me.cboBPs.Location = New System.Drawing.Point(82, 29)
        Me.cboBPs.Name = "cboBPs"
        Me.cboBPs.Size = New System.Drawing.Size(306, 21)
        Me.cboBPs.TabIndex = 49
        '
        'pbBP
        '
        Me.pbBP.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.pbBP.Location = New System.Drawing.Point(12, 12)
        Me.pbBP.Name = "pbBP"
        Me.pbBP.Size = New System.Drawing.Size(64, 64)
        Me.pbBP.TabIndex = 30
        Me.pbBP.TabStop = False
        '
        'frmAddCustomBP
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(400, 213)
        Me.Controls.Add(Me.cboBPs)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.nudPELevel)
        Me.Controls.Add(Me.nudMELevel)
        Me.Controls.Add(Me.lblPELevel)
        Me.Controls.Add(Me.lblMELevel)
        Me.Controls.Add(Me.pbBP)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddCustomBP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Custom Blueprint"
        CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents nudPELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudMELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblPELevel As System.Windows.Forms.Label
    Friend WithEvents lblMELevel As System.Windows.Forms.Label
    Friend WithEvents pbBP As System.Windows.Forms.PictureBox
    Friend WithEvents cboBPs As System.Windows.Forms.ComboBox
End Class
