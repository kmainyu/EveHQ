<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModifyFTPAccounts
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAccept = New System.Windows.Forms.Button
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.txtFTPName = New System.Windows.Forms.TextBox
        Me.lblServer = New System.Windows.Forms.Label
        Me.lvlFTPName = New System.Windows.Forms.Label
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.lblPath = New System.Windows.Forms.Label
        Me.lblPort = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.lblPassword = New System.Windows.Forms.Label
        Me.lblUsername = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.NumericUpDown
        CType(Me.txtPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(259, 34)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Please enter your FTP account information in the boxes below." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(196, 241)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(115, 241)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 6
        Me.btnAccept.Text = "Add"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(83, 92)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(188, 21)
        Me.txtServer.TabIndex = 1
        '
        'txtFTPName
        '
        Me.txtFTPName.Location = New System.Drawing.Point(83, 66)
        Me.txtFTPName.Name = "txtFTPName"
        Me.txtFTPName.Size = New System.Drawing.Size(188, 21)
        Me.txtFTPName.TabIndex = 0
        '
        'lblServer
        '
        Me.lblServer.AutoSize = True
        Me.lblServer.Location = New System.Drawing.Point(10, 95)
        Me.lblServer.Name = "lblServer"
        Me.lblServer.Size = New System.Drawing.Size(43, 13)
        Me.lblServer.TabIndex = 8
        Me.lblServer.Text = "Server:"
        '
        'lvlFTPName
        '
        Me.lvlFTPName.AutoSize = True
        Me.lvlFTPName.Location = New System.Drawing.Point(10, 69)
        Me.lvlFTPName.Name = "lvlFTPName"
        Me.lvlFTPName.Size = New System.Drawing.Size(59, 13)
        Me.lvlFTPName.TabIndex = 7
        Me.lvlFTPName.Text = "FTP Name:"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(83, 144)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(188, 21)
        Me.txtPath.TabIndex = 3
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.Location = New System.Drawing.Point(10, 147)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(33, 13)
        Me.lblPath.TabIndex = 15
        Me.lblPath.Text = "Path:"
        '
        'lblPort
        '
        Me.lblPort.AutoSize = True
        Me.lblPort.Location = New System.Drawing.Point(10, 121)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(31, 13)
        Me.lblPort.TabIndex = 14
        Me.lblPort.Text = "Port:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(83, 196)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(188, 21)
        Me.txtPassword.TabIndex = 5
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(83, 170)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(188, 21)
        Me.txtUsername.TabIndex = 4
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(10, 199)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(57, 13)
        Me.lblPassword.TabIndex = 19
        Me.lblPassword.Text = "Password:"
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(10, 173)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(59, 13)
        Me.lblUsername.TabIndex = 18
        Me.lblUsername.Text = "Username:"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(83, 119)
        Me.txtPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.txtPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(107, 21)
        Me.txtPort.TabIndex = 2
        Me.txtPort.ThousandsSeparator = True
        Me.txtPort.Value = New Decimal(New Integer() {21, 0, 0, 0})
        '
        'frmModifyFTPAccounts
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.lblPath)
        Me.Controls.Add(Me.lblPort)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.txtServer)
        Me.Controls.Add(Me.txtFTPName)
        Me.Controls.Add(Me.lblServer)
        Me.Controls.Add(Me.lvlFTPName)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmModifyFTPAccounts"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Modify FTP Account"
        CType(Me.txtPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents txtFTPName As System.Windows.Forms.TextBox
    Friend WithEvents lblServer As System.Windows.Forms.Label
    Friend WithEvents lvlFTPName As System.Windows.Forms.Label
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents lblPath As System.Windows.Forms.Label
    Friend WithEvents lblPort As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.NumericUpDown
End Class
