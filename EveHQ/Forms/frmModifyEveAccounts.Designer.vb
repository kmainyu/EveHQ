<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModifyEveAccounts
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModifyEveAccounts))
        Me.lblUserID = New System.Windows.Forms.Label
        Me.lblAPIKey = New System.Windows.Forms.Label
        Me.txtUserID = New System.Windows.Forms.TextBox
        Me.txtAPIKey = New System.Windows.Forms.TextBox
        Me.btnAccept = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtAccountName = New System.Windows.Forms.TextBox
        Me.lblAccountName = New System.Windows.Forms.Label
        Me.lblGetAPIKey = New System.Windows.Forms.LinkLabel
        Me.lblFindAPIKey = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblUserID
        '
        Me.lblUserID.AutoSize = True
        Me.lblUserID.Location = New System.Drawing.Point(12, 137)
        Me.lblUserID.Name = "lblUserID"
        Me.lblUserID.Size = New System.Drawing.Size(43, 13)
        Me.lblUserID.TabIndex = 0
        Me.lblUserID.Text = "UserID:"
        '
        'lblAPIKey
        '
        Me.lblAPIKey.AutoSize = True
        Me.lblAPIKey.Location = New System.Drawing.Point(12, 163)
        Me.lblAPIKey.Name = "lblAPIKey"
        Me.lblAPIKey.Size = New System.Drawing.Size(45, 13)
        Me.lblAPIKey.TabIndex = 1
        Me.lblAPIKey.Text = "APIKey:"
        '
        'txtUserID
        '
        Me.txtUserID.Location = New System.Drawing.Point(99, 134)
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.Size = New System.Drawing.Size(107, 20)
        Me.txtUserID.TabIndex = 0
        '
        'txtAPIKey
        '
        Me.txtAPIKey.Location = New System.Drawing.Point(99, 160)
        Me.txtAPIKey.Name = "txtAPIKey"
        Me.txtAPIKey.Size = New System.Drawing.Size(416, 20)
        Me.txtAPIKey.TabIndex = 1
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(359, 212)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 2
        Me.btnAccept.Text = "Add"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(440, 212)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(503, 66)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = resources.GetString("Label1.Text")
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAccountName
        '
        Me.txtAccountName.Location = New System.Drawing.Point(99, 186)
        Me.txtAccountName.Name = "txtAccountName"
        Me.txtAccountName.Size = New System.Drawing.Size(107, 20)
        Me.txtAccountName.TabIndex = 8
        '
        'lblAccountName
        '
        Me.lblAccountName.AutoSize = True
        Me.lblAccountName.Location = New System.Drawing.Point(12, 189)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.Size = New System.Drawing.Size(81, 13)
        Me.lblAccountName.TabIndex = 7
        Me.lblAccountName.Text = "Account Name:"
        '
        'lblGetAPIKey
        '
        Me.lblGetAPIKey.AutoSize = True
        Me.lblGetAPIKey.Location = New System.Drawing.Point(216, 91)
        Me.lblGetAPIKey.Name = "lblGetAPIKey"
        Me.lblGetAPIKey.Size = New System.Drawing.Size(220, 13)
        Me.lblGetAPIKey.TabIndex = 9
        Me.lblGetAPIKey.TabStop = True
        Me.lblGetAPIKey.Text = "http://myeve.eve-online.com/api/default.asp"
        '
        'lblFindAPIKey
        '
        Me.lblFindAPIKey.AutoSize = True
        Me.lblFindAPIKey.Location = New System.Drawing.Point(12, 91)
        Me.lblFindAPIKey.Name = "lblFindAPIKey"
        Me.lblFindAPIKey.Size = New System.Drawing.Size(198, 13)
        Me.lblFindAPIKey.TabIndex = 10
        Me.lblFindAPIKey.Text = "You can find your userID and APIKey at:"
        '
        'frmModifyEveAccounts
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(527, 248)
        Me.Controls.Add(Me.lblFindAPIKey)
        Me.Controls.Add(Me.lblGetAPIKey)
        Me.Controls.Add(Me.txtAccountName)
        Me.Controls.Add(Me.lblAccountName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.txtAPIKey)
        Me.Controls.Add(Me.txtUserID)
        Me.Controls.Add(Me.lblAPIKey)
        Me.Controls.Add(Me.lblUserID)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmModifyEveAccounts"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Modify Eve Account"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblUserID As System.Windows.Forms.Label
    Friend WithEvents lblAPIKey As System.Windows.Forms.Label
    Friend WithEvents txtUserID As System.Windows.Forms.TextBox
    Friend WithEvents txtAPIKey As System.Windows.Forms.TextBox
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAccountName As System.Windows.Forms.TextBox
    Friend WithEvents lblAccountName As System.Windows.Forms.Label
    Friend WithEvents lblGetAPIKey As System.Windows.Forms.LinkLabel
    Friend WithEvents lblFindAPIKey As System.Windows.Forms.Label
End Class
