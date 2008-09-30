<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataConvert
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDataConvert))
        Me.cboConvertType = New System.Windows.Forms.ComboBox
        Me.lblConvertType = New System.Windows.Forms.Label
        Me.grpMSSQL = New System.Windows.Forms.GroupBox
        Me.lblMSSQLSecurity = New System.Windows.Forms.Label
        Me.radMSSQLDatabase = New System.Windows.Forms.RadioButton
        Me.radMSSQLWindows = New System.Windows.Forms.RadioButton
        Me.txtMSSQLPassword = New System.Windows.Forms.TextBox
        Me.txtMSSQLUser = New System.Windows.Forms.TextBox
        Me.txtMSSQLServer = New System.Windows.Forms.TextBox
        Me.lblMSSQLPassword = New System.Windows.Forms.Label
        Me.lblMSSQLUser = New System.Windows.Forms.Label
        Me.lblMSSQLServer = New System.Windows.Forms.Label
        Me.grpMySQL = New System.Windows.Forms.GroupBox
        Me.txtMySQLPassword = New System.Windows.Forms.TextBox
        Me.txtMySQLUser = New System.Windows.Forms.TextBox
        Me.txtMySQLServer = New System.Windows.Forms.TextBox
        Me.lblMySQLPassword = New System.Windows.Forms.Label
        Me.lblMySQLUser = New System.Windows.Forms.Label
        Me.lblMySQLServer = New System.Windows.Forms.Label
        Me.cboConvert = New System.Windows.Forms.ComboBox
        Me.lblConvertTo = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblProgress = New System.Windows.Forms.Label
        Me.pbProgress = New System.Windows.Forms.ProgressBar
        Me.btnConvert = New System.Windows.Forms.Button
        Me.lblFiles = New System.Windows.Forms.Label
        Me.btnTarget = New System.Windows.Forms.Button
        Me.txtTarget = New System.Windows.Forms.TextBox
        Me.lblTarget = New System.Windows.Forms.Label
        Me.btnSource = New System.Windows.Forms.Button
        Me.txtSource = New System.Windows.Forms.TextBox
        Me.lblSource = New System.Windows.Forms.Label
        Me.lblPurpose = New System.Windows.Forms.Label
        Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog
        Me.btnMap = New System.Windows.Forms.Button
        Me.btnBPS = New System.Windows.Forms.Button
        Me.btnCSVDump = New System.Windows.Forms.Button
        Me.btnAddToMySQL = New System.Windows.Forms.Button
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.txtCompressFile = New System.Windows.Forms.TextBox
        Me.btnFileSource = New System.Windows.Forms.Button
        Me.lblCompress = New System.Windows.Forms.Label
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog
        Me.lblSourceFileInfo = New System.Windows.Forms.Label
        Me.lblCompressedFileInfo = New System.Windows.Forms.Label
        Me.brnStartCompression = New System.Windows.Forms.Button
        Me.grpMSSQL.SuspendLayout()
        Me.grpMySQL.SuspendLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboConvertType
        '
        Me.cboConvertType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboConvertType.FormattingEnabled = True
        Me.cboConvertType.Items.AddRange(New Object() {"Full", "Basic (Items)", "Standard (Items & Map Routes)"})
        Me.cboConvertType.Location = New System.Drawing.Point(402, 50)
        Me.cboConvertType.Name = "cboConvertType"
        Me.cboConvertType.Size = New System.Drawing.Size(188, 21)
        Me.cboConvertType.TabIndex = 52
        '
        'lblConvertType
        '
        Me.lblConvertType.AutoSize = True
        Me.lblConvertType.Location = New System.Drawing.Point(308, 53)
        Me.lblConvertType.Name = "lblConvertType"
        Me.lblConvertType.Size = New System.Drawing.Size(63, 13)
        Me.lblConvertType.TabIndex = 51
        Me.lblConvertType.Text = "Convert To:"
        '
        'grpMSSQL
        '
        Me.grpMSSQL.Controls.Add(Me.lblMSSQLSecurity)
        Me.grpMSSQL.Controls.Add(Me.radMSSQLDatabase)
        Me.grpMSSQL.Controls.Add(Me.radMSSQLWindows)
        Me.grpMSSQL.Controls.Add(Me.txtMSSQLPassword)
        Me.grpMSSQL.Controls.Add(Me.txtMSSQLUser)
        Me.grpMSSQL.Controls.Add(Me.txtMSSQLServer)
        Me.grpMSSQL.Controls.Add(Me.lblMSSQLPassword)
        Me.grpMSSQL.Controls.Add(Me.lblMSSQLUser)
        Me.grpMSSQL.Controls.Add(Me.lblMSSQLServer)
        Me.grpMSSQL.Location = New System.Drawing.Point(103, 77)
        Me.grpMSSQL.Name = "grpMSSQL"
        Me.grpMSSQL.Size = New System.Drawing.Size(309, 145)
        Me.grpMSSQL.TabIndex = 50
        Me.grpMSSQL.TabStop = False
        Me.grpMSSQL.Text = "MS SQL Options"
        Me.grpMSSQL.Visible = False
        '
        'lblMSSQLSecurity
        '
        Me.lblMSSQLSecurity.AutoSize = True
        Me.lblMSSQLSecurity.Location = New System.Drawing.Point(6, 32)
        Me.lblMSSQLSecurity.Name = "lblMSSQLSecurity"
        Me.lblMSSQLSecurity.Size = New System.Drawing.Size(45, 13)
        Me.lblMSSQLSecurity.TabIndex = 8
        Me.lblMSSQLSecurity.Text = "Security"
        '
        'radMSSQLDatabase
        '
        Me.radMSSQLDatabase.AutoSize = True
        Me.radMSSQLDatabase.Checked = True
        Me.radMSSQLDatabase.Location = New System.Drawing.Point(73, 30)
        Me.radMSSQLDatabase.Name = "radMSSQLDatabase"
        Me.radMSSQLDatabase.Size = New System.Drawing.Size(46, 17)
        Me.radMSSQLDatabase.TabIndex = 7
        Me.radMSSQLDatabase.TabStop = True
        Me.radMSSQLDatabase.Text = "SQL"
        Me.radMSSQLDatabase.UseVisualStyleBackColor = True
        '
        'radMSSQLWindows
        '
        Me.radMSSQLWindows.AutoSize = True
        Me.radMSSQLWindows.Location = New System.Drawing.Point(139, 30)
        Me.radMSSQLWindows.Name = "radMSSQLWindows"
        Me.radMSSQLWindows.Size = New System.Drawing.Size(69, 17)
        Me.radMSSQLWindows.TabIndex = 6
        Me.radMSSQLWindows.Text = "Windows"
        Me.radMSSQLWindows.UseVisualStyleBackColor = True
        '
        'txtMSSQLPassword
        '
        Me.txtMSSQLPassword.Location = New System.Drawing.Point(73, 105)
        Me.txtMSSQLPassword.Name = "txtMSSQLPassword"
        Me.txtMSSQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMSSQLPassword.Size = New System.Drawing.Size(230, 20)
        Me.txtMSSQLPassword.TabIndex = 5
        '
        'txtMSSQLUser
        '
        Me.txtMSSQLUser.Location = New System.Drawing.Point(73, 79)
        Me.txtMSSQLUser.Name = "txtMSSQLUser"
        Me.txtMSSQLUser.Size = New System.Drawing.Size(230, 20)
        Me.txtMSSQLUser.TabIndex = 4
        '
        'txtMSSQLServer
        '
        Me.txtMSSQLServer.Location = New System.Drawing.Point(73, 53)
        Me.txtMSSQLServer.Name = "txtMSSQLServer"
        Me.txtMSSQLServer.Size = New System.Drawing.Size(230, 20)
        Me.txtMSSQLServer.TabIndex = 3
        '
        'lblMSSQLPassword
        '
        Me.lblMSSQLPassword.AutoSize = True
        Me.lblMSSQLPassword.Location = New System.Drawing.Point(6, 108)
        Me.lblMSSQLPassword.Name = "lblMSSQLPassword"
        Me.lblMSSQLPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblMSSQLPassword.TabIndex = 2
        Me.lblMSSQLPassword.Text = "Password:"
        '
        'lblMSSQLUser
        '
        Me.lblMSSQLUser.AutoSize = True
        Me.lblMSSQLUser.Location = New System.Drawing.Point(6, 82)
        Me.lblMSSQLUser.Name = "lblMSSQLUser"
        Me.lblMSSQLUser.Size = New System.Drawing.Size(32, 13)
        Me.lblMSSQLUser.TabIndex = 1
        Me.lblMSSQLUser.Text = "User:"
        '
        'lblMSSQLServer
        '
        Me.lblMSSQLServer.AutoSize = True
        Me.lblMSSQLServer.Location = New System.Drawing.Point(6, 56)
        Me.lblMSSQLServer.Name = "lblMSSQLServer"
        Me.lblMSSQLServer.Size = New System.Drawing.Size(41, 13)
        Me.lblMSSQLServer.TabIndex = 0
        Me.lblMSSQLServer.Text = "Server:"
        '
        'grpMySQL
        '
        Me.grpMySQL.Controls.Add(Me.txtMySQLPassword)
        Me.grpMySQL.Controls.Add(Me.txtMySQLUser)
        Me.grpMySQL.Controls.Add(Me.txtMySQLServer)
        Me.grpMySQL.Controls.Add(Me.lblMySQLPassword)
        Me.grpMySQL.Controls.Add(Me.lblMySQLUser)
        Me.grpMySQL.Controls.Add(Me.lblMySQLServer)
        Me.grpMySQL.Location = New System.Drawing.Point(103, 77)
        Me.grpMySQL.Name = "grpMySQL"
        Me.grpMySQL.Size = New System.Drawing.Size(309, 145)
        Me.grpMySQL.TabIndex = 49
        Me.grpMySQL.TabStop = False
        Me.grpMySQL.Text = "MySQL Options"
        Me.grpMySQL.Visible = False
        '
        'txtMySQLPassword
        '
        Me.txtMySQLPassword.Location = New System.Drawing.Point(73, 79)
        Me.txtMySQLPassword.Name = "txtMySQLPassword"
        Me.txtMySQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMySQLPassword.Size = New System.Drawing.Size(230, 20)
        Me.txtMySQLPassword.TabIndex = 5
        '
        'txtMySQLUser
        '
        Me.txtMySQLUser.Location = New System.Drawing.Point(73, 53)
        Me.txtMySQLUser.Name = "txtMySQLUser"
        Me.txtMySQLUser.Size = New System.Drawing.Size(230, 20)
        Me.txtMySQLUser.TabIndex = 4
        '
        'txtMySQLServer
        '
        Me.txtMySQLServer.Location = New System.Drawing.Point(73, 27)
        Me.txtMySQLServer.Name = "txtMySQLServer"
        Me.txtMySQLServer.Size = New System.Drawing.Size(230, 20)
        Me.txtMySQLServer.TabIndex = 3
        '
        'lblMySQLPassword
        '
        Me.lblMySQLPassword.AutoSize = True
        Me.lblMySQLPassword.Location = New System.Drawing.Point(6, 82)
        Me.lblMySQLPassword.Name = "lblMySQLPassword"
        Me.lblMySQLPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblMySQLPassword.TabIndex = 2
        Me.lblMySQLPassword.Text = "Password:"
        '
        'lblMySQLUser
        '
        Me.lblMySQLUser.AutoSize = True
        Me.lblMySQLUser.Location = New System.Drawing.Point(6, 56)
        Me.lblMySQLUser.Name = "lblMySQLUser"
        Me.lblMySQLUser.Size = New System.Drawing.Size(32, 13)
        Me.lblMySQLUser.TabIndex = 1
        Me.lblMySQLUser.Text = "User:"
        '
        'lblMySQLServer
        '
        Me.lblMySQLServer.AutoSize = True
        Me.lblMySQLServer.Location = New System.Drawing.Point(6, 30)
        Me.lblMySQLServer.Name = "lblMySQLServer"
        Me.lblMySQLServer.Size = New System.Drawing.Size(41, 13)
        Me.lblMySQLServer.TabIndex = 0
        Me.lblMySQLServer.Text = "Server:"
        '
        'cboConvert
        '
        Me.cboConvert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboConvert.FormattingEnabled = True
        Me.cboConvert.Items.AddRange(New Object() {"Access Database (.MDB)", "Comma Separated Files (.CSV)", "MS SQL Server", "MS SQL 2005 Express", "MySQL 5.0"})
        Me.cboConvert.Location = New System.Drawing.Point(103, 50)
        Me.cboConvert.Name = "cboConvert"
        Me.cboConvert.Size = New System.Drawing.Size(188, 21)
        Me.cboConvert.TabIndex = 48
        '
        'lblConvertTo
        '
        Me.lblConvertTo.AutoSize = True
        Me.lblConvertTo.Location = New System.Drawing.Point(9, 53)
        Me.lblConvertTo.Name = "lblConvertTo"
        Me.lblConvertTo.Size = New System.Drawing.Size(63, 13)
        Me.lblConvertTo.TabIndex = 47
        Me.lblConvertTo.Text = "Convert To:"
        '
        'btnCancel
        '
        Me.btnCancel.Enabled = False
        Me.btnCancel.Location = New System.Drawing.Point(118, 383)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(106, 32)
        Me.btnCancel.TabIndex = 46
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(6, 341)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(48, 13)
        Me.lblProgress.TabIndex = 45
        Me.lblProgress.Text = "Progress"
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(6, 357)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(609, 20)
        Me.pbProgress.Step = 1
        Me.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbProgress.TabIndex = 44
        '
        'btnConvert
        '
        Me.btnConvert.Enabled = False
        Me.btnConvert.Location = New System.Drawing.Point(6, 383)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(106, 32)
        Me.btnConvert.TabIndex = 43
        Me.btnConvert.Text = "Convert!"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'lblFiles
        '
        Me.lblFiles.AutoSize = True
        Me.lblFiles.Location = New System.Drawing.Point(100, 251)
        Me.lblFiles.Name = "lblFiles"
        Me.lblFiles.Size = New System.Drawing.Size(154, 13)
        Me.lblFiles.TabIndex = 42
        Me.lblFiles.Text = "Number of files in directory: n/a"
        '
        'btnTarget
        '
        Me.btnTarget.Enabled = False
        Me.btnTarget.Location = New System.Drawing.Point(596, 276)
        Me.btnTarget.Name = "btnTarget"
        Me.btnTarget.Size = New System.Drawing.Size(24, 23)
        Me.btnTarget.TabIndex = 41
        Me.btnTarget.Text = "..."
        Me.btnTarget.UseVisualStyleBackColor = True
        '
        'txtTarget
        '
        Me.txtTarget.Enabled = False
        Me.txtTarget.Location = New System.Drawing.Point(103, 278)
        Me.txtTarget.Name = "txtTarget"
        Me.txtTarget.Size = New System.Drawing.Size(487, 20)
        Me.txtTarget.TabIndex = 40
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.Enabled = False
        Me.lblTarget.Location = New System.Drawing.Point(8, 281)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(86, 13)
        Me.lblTarget.TabIndex = 39
        Me.lblTarget.Text = "Target Directory:"
        '
        'btnSource
        '
        Me.btnSource.Location = New System.Drawing.Point(596, 226)
        Me.btnSource.Name = "btnSource"
        Me.btnSource.Size = New System.Drawing.Size(24, 23)
        Me.btnSource.TabIndex = 38
        Me.btnSource.Text = "..."
        Me.btnSource.UseVisualStyleBackColor = True
        '
        'txtSource
        '
        Me.txtSource.Location = New System.Drawing.Point(103, 228)
        Me.txtSource.Name = "txtSource"
        Me.txtSource.Size = New System.Drawing.Size(487, 20)
        Me.txtSource.TabIndex = 37
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Location = New System.Drawing.Point(8, 231)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(89, 13)
        Me.lblSource.TabIndex = 36
        Me.lblSource.Text = "Source Directory:"
        '
        'lblPurpose
        '
        Me.lblPurpose.AutoSize = True
        Me.lblPurpose.Location = New System.Drawing.Point(6, 14)
        Me.lblPurpose.Name = "lblPurpose"
        Me.lblPurpose.Size = New System.Drawing.Size(365, 13)
        Me.lblPurpose.TabIndex = 35
        Me.lblPurpose.Text = "Please select the type of data you wish to convert the Eve Data Export files." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnMap
        '
        Me.btnMap.Location = New System.Drawing.Point(544, 391)
        Me.btnMap.Name = "btnMap"
        Me.btnMap.Size = New System.Drawing.Size(75, 23)
        Me.btnMap.TabIndex = 53
        Me.btnMap.Text = "Map Test"
        Me.btnMap.UseVisualStyleBackColor = True
        Me.btnMap.Visible = False
        '
        'btnBPS
        '
        Me.btnBPS.Location = New System.Drawing.Point(463, 391)
        Me.btnBPS.Name = "btnBPS"
        Me.btnBPS.Size = New System.Drawing.Size(75, 23)
        Me.btnBPS.TabIndex = 54
        Me.btnBPS.Text = "BP Dump"
        Me.btnBPS.UseVisualStyleBackColor = True
        Me.btnBPS.Visible = False
        '
        'btnCSVDump
        '
        Me.btnCSVDump.Location = New System.Drawing.Point(382, 391)
        Me.btnCSVDump.Name = "btnCSVDump"
        Me.btnCSVDump.Size = New System.Drawing.Size(75, 23)
        Me.btnCSVDump.TabIndex = 55
        Me.btnCSVDump.Text = "CSV Dump"
        Me.btnCSVDump.UseVisualStyleBackColor = True
        Me.btnCSVDump.Visible = False
        '
        'btnAddToMySQL
        '
        Me.btnAddToMySQL.Location = New System.Drawing.Point(289, 391)
        Me.btnAddToMySQL.Name = "btnAddToMySQL"
        Me.btnAddToMySQL.Size = New System.Drawing.Size(87, 23)
        Me.btnAddToMySQL.TabIndex = 56
        Me.btnAddToMySQL.Text = "Add to MySQL"
        Me.btnAddToMySQL.UseVisualStyleBackColor = True
        Me.btnAddToMySQL.Visible = False
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(661, 483)
        Me.TabControl1.TabIndex = 57
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lblPurpose)
        Me.TabPage1.Controls.Add(Me.btnAddToMySQL)
        Me.TabPage1.Controls.Add(Me.lblSource)
        Me.TabPage1.Controls.Add(Me.btnCSVDump)
        Me.TabPage1.Controls.Add(Me.txtSource)
        Me.TabPage1.Controls.Add(Me.btnBPS)
        Me.TabPage1.Controls.Add(Me.btnSource)
        Me.TabPage1.Controls.Add(Me.btnMap)
        Me.TabPage1.Controls.Add(Me.lblTarget)
        Me.TabPage1.Controls.Add(Me.cboConvertType)
        Me.TabPage1.Controls.Add(Me.txtTarget)
        Me.TabPage1.Controls.Add(Me.lblConvertType)
        Me.TabPage1.Controls.Add(Me.btnTarget)
        Me.TabPage1.Controls.Add(Me.grpMSSQL)
        Me.TabPage1.Controls.Add(Me.lblFiles)
        Me.TabPage1.Controls.Add(Me.grpMySQL)
        Me.TabPage1.Controls.Add(Me.btnConvert)
        Me.TabPage1.Controls.Add(Me.cboConvert)
        Me.TabPage1.Controls.Add(Me.pbProgress)
        Me.TabPage1.Controls.Add(Me.lblConvertTo)
        Me.TabPage1.Controls.Add(Me.lblProgress)
        Me.TabPage1.Controls.Add(Me.btnCancel)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(653, 457)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Data Converter"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.brnStartCompression)
        Me.TabPage2.Controls.Add(Me.lblCompressedFileInfo)
        Me.TabPage2.Controls.Add(Me.lblSourceFileInfo)
        Me.TabPage2.Controls.Add(Me.txtCompressFile)
        Me.TabPage2.Controls.Add(Me.btnFileSource)
        Me.TabPage2.Controls.Add(Me.lblCompress)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(653, 457)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "File Compression"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'txtCompressFile
        '
        Me.txtCompressFile.Location = New System.Drawing.Point(21, 49)
        Me.txtCompressFile.Name = "txtCompressFile"
        Me.txtCompressFile.Size = New System.Drawing.Size(581, 20)
        Me.txtCompressFile.TabIndex = 40
        '
        'btnFileSource
        '
        Me.btnFileSource.Location = New System.Drawing.Point(608, 47)
        Me.btnFileSource.Name = "btnFileSource"
        Me.btnFileSource.Size = New System.Drawing.Size(24, 23)
        Me.btnFileSource.TabIndex = 41
        Me.btnFileSource.Text = "..."
        Me.btnFileSource.UseVisualStyleBackColor = True
        '
        'lblCompress
        '
        Me.lblCompress.AutoSize = True
        Me.lblCompress.Location = New System.Drawing.Point(18, 20)
        Me.lblCompress.Name = "lblCompress"
        Me.lblCompress.Size = New System.Drawing.Size(282, 13)
        Me.lblCompress.TabIndex = 0
        Me.lblCompress.Text = "Please enter the file name of the file you wish to compress:"
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'lblSourceFileInfo
        '
        Me.lblSourceFileInfo.AutoSize = True
        Me.lblSourceFileInfo.Location = New System.Drawing.Point(18, 166)
        Me.lblSourceFileInfo.Name = "lblSourceFileInfo"
        Me.lblSourceFileInfo.Size = New System.Drawing.Size(84, 13)
        Me.lblSourceFileInfo.TabIndex = 42
        Me.lblSourceFileInfo.Text = "Source File Info:"
        '
        'lblCompressedFileInfo
        '
        Me.lblCompressedFileInfo.AutoSize = True
        Me.lblCompressedFileInfo.Location = New System.Drawing.Point(18, 194)
        Me.lblCompressedFileInfo.Name = "lblCompressedFileInfo"
        Me.lblCompressedFileInfo.Size = New System.Drawing.Size(108, 13)
        Me.lblCompressedFileInfo.TabIndex = 43
        Me.lblCompressedFileInfo.Text = "Compressed File Info:"
        '
        'brnStartCompression
        '
        Me.brnStartCompression.Location = New System.Drawing.Point(478, 98)
        Me.brnStartCompression.Name = "brnStartCompression"
        Me.brnStartCompression.Size = New System.Drawing.Size(124, 23)
        Me.brnStartCompression.TabIndex = 44
        Me.brnStartCompression.Text = "Start Compression"
        Me.brnStartCompression.UseVisualStyleBackColor = True
        '
        'frmDataConvert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(661, 483)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDataConvert"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EveHQ Data Converter"
        Me.grpMSSQL.ResumeLayout(False)
        Me.grpMSSQL.PerformLayout()
        Me.grpMySQL.ResumeLayout(False)
        Me.grpMySQL.PerformLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboConvertType As System.Windows.Forms.ComboBox
    Friend WithEvents lblConvertType As System.Windows.Forms.Label
    Friend WithEvents grpMSSQL As System.Windows.Forms.GroupBox
    Friend WithEvents lblMSSQLSecurity As System.Windows.Forms.Label
    Friend WithEvents radMSSQLDatabase As System.Windows.Forms.RadioButton
    Friend WithEvents radMSSQLWindows As System.Windows.Forms.RadioButton
    Friend WithEvents txtMSSQLPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtMSSQLUser As System.Windows.Forms.TextBox
    Friend WithEvents txtMSSQLServer As System.Windows.Forms.TextBox
    Friend WithEvents lblMSSQLPassword As System.Windows.Forms.Label
    Friend WithEvents lblMSSQLUser As System.Windows.Forms.Label
    Friend WithEvents lblMSSQLServer As System.Windows.Forms.Label
    Friend WithEvents grpMySQL As System.Windows.Forms.GroupBox
    Friend WithEvents txtMySQLPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtMySQLUser As System.Windows.Forms.TextBox
    Friend WithEvents txtMySQLServer As System.Windows.Forms.TextBox
    Friend WithEvents lblMySQLPassword As System.Windows.Forms.Label
    Friend WithEvents lblMySQLUser As System.Windows.Forms.Label
    Friend WithEvents lblMySQLServer As System.Windows.Forms.Label
    Friend WithEvents cboConvert As System.Windows.Forms.ComboBox
    Friend WithEvents lblConvertTo As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents btnConvert As System.Windows.Forms.Button
    Friend WithEvents lblFiles As System.Windows.Forms.Label
    Friend WithEvents btnTarget As System.Windows.Forms.Button
    Friend WithEvents txtTarget As System.Windows.Forms.TextBox
    Friend WithEvents lblTarget As System.Windows.Forms.Label
    Friend WithEvents btnSource As System.Windows.Forms.Button
    Friend WithEvents txtSource As System.Windows.Forms.TextBox
    Friend WithEvents lblSource As System.Windows.Forms.Label
    Friend WithEvents lblPurpose As System.Windows.Forms.Label
    Friend WithEvents fbd1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnMap As System.Windows.Forms.Button
    Friend WithEvents btnBPS As System.Windows.Forms.Button
    Friend WithEvents btnCSVDump As System.Windows.Forms.Button
    Friend WithEvents btnAddToMySQL As System.Windows.Forms.Button
    Friend WithEvents FileSystemWatcher1 As System.IO.FileSystemWatcher
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents txtCompressFile As System.Windows.Forms.TextBox
    Friend WithEvents btnFileSource As System.Windows.Forms.Button
    Friend WithEvents lblCompress As System.Windows.Forms.Label
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents brnStartCompression As System.Windows.Forms.Button
    Friend WithEvents lblCompressedFileInfo As System.Windows.Forms.Label
    Friend WithEvents lblSourceFileInfo As System.Windows.Forms.Label
End Class
