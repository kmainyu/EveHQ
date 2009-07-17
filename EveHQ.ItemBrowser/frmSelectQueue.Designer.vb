<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectQueue
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
        Me.lblDescription = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAccept = New System.Windows.Forms.Button
        Me.lblQueueName = New System.Windows.Forms.Label
        Me.radNewQueue = New System.Windows.Forms.RadioButton
        Me.radExistingQueue = New System.Windows.Forms.RadioButton
        Me.txtQueueName = New System.Windows.Forms.TextBox
        Me.cboQueueName = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(12, 9)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(259, 36)
        Me.lblDescription.TabIndex = 13
        Me.lblDescription.Text = "Please choose a method of entering the skills onto a skill queue..."
        Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(202, 161)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(121, 161)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 11
        Me.btnAccept.Text = "Add"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'lblQueueName
        '
        Me.lblQueueName.AutoSize = True
        Me.lblQueueName.Location = New System.Drawing.Point(10, 125)
        Me.lblQueueName.Name = "lblQueueName"
        Me.lblQueueName.Size = New System.Drawing.Size(73, 13)
        Me.lblQueueName.TabIndex = 9
        Me.lblQueueName.Text = "Queue Name:"
        '
        'radNewQueue
        '
        Me.radNewQueue.AutoSize = True
        Me.radNewQueue.Location = New System.Drawing.Point(15, 86)
        Me.radNewQueue.Name = "radNewQueue"
        Me.radNewQueue.Size = New System.Drawing.Size(101, 17)
        Me.radNewQueue.TabIndex = 14
        Me.radNewQueue.Text = "New Skill Queue"
        Me.radNewQueue.UseVisualStyleBackColor = True
        '
        'radExistingQueue
        '
        Me.radExistingQueue.AutoSize = True
        Me.radExistingQueue.Checked = True
        Me.radExistingQueue.Location = New System.Drawing.Point(15, 63)
        Me.radExistingQueue.Name = "radExistingQueue"
        Me.radExistingQueue.Size = New System.Drawing.Size(117, 17)
        Me.radExistingQueue.TabIndex = 15
        Me.radExistingQueue.TabStop = True
        Me.radExistingQueue.Text = "Existing Skill Queue"
        Me.radExistingQueue.UseVisualStyleBackColor = True
        '
        'txtQueueName
        '
        Me.txtQueueName.Location = New System.Drawing.Point(89, 122)
        Me.txtQueueName.Name = "txtQueueName"
        Me.txtQueueName.Size = New System.Drawing.Size(188, 21)
        Me.txtQueueName.TabIndex = 8
        '
        'cboQueueName
        '
        Me.cboQueueName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboQueueName.FormattingEnabled = True
        Me.cboQueueName.Location = New System.Drawing.Point(89, 122)
        Me.cboQueueName.Name = "cboQueueName"
        Me.cboQueueName.Size = New System.Drawing.Size(188, 21)
        Me.cboQueueName.TabIndex = 16
        '
        'frmSelectQueue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(289, 193)
        Me.Controls.Add(Me.cboQueueName)
        Me.Controls.Add(Me.radExistingQueue)
        Me.Controls.Add(Me.radNewQueue)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.txtQueueName)
        Me.Controls.Add(Me.lblQueueName)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectQueue"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add to Skill Queue"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents lblQueueName As System.Windows.Forms.Label
    Friend WithEvents radNewQueue As System.Windows.Forms.RadioButton
    Friend WithEvents radExistingQueue As System.Windows.Forms.RadioButton
    Friend WithEvents txtQueueName As System.Windows.Forms.TextBox
    Friend WithEvents cboQueueName As System.Windows.Forms.ComboBox
End Class
