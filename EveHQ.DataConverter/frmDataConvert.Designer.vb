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
        Me.panelConvert = New System.Windows.Forms.Panel
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.panelCompress = New System.Windows.Forms.Panel
        Me.lblCompress = New System.Windows.Forms.Label
        Me.brnStartCompression = New System.Windows.Forms.Button
        Me.btnFileSource = New System.Windows.Forms.Button
        Me.lblCompressedFileInfo = New System.Windows.Forms.Label
        Me.txtCompressFile = New System.Windows.Forms.TextBox
        Me.lblSourceFileInfo = New System.Windows.Forms.Label
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.panelSQL2TSQL = New System.Windows.Forms.Panel
        Me.lblSQLServer = New System.Windows.Forms.Label
        Me.lblSQLConversionProgress = New System.Windows.Forms.Label
        Me.txtSQLServer = New System.Windows.Forms.TextBox
        Me.btnConvertSQL = New System.Windows.Forms.Button
        Me.lblDatabase = New System.Windows.Forms.Label
        Me.btnTestConnection = New System.Windows.Forms.Button
        Me.txtDatabase = New System.Windows.Forms.TextBox
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.panelCerts = New System.Windows.Forms.Panel
        Me.lblCertLocation = New System.Windows.Forms.Label
        Me.btnDecodeCertificates = New System.Windows.Forms.Button
        Me.btnFindCertficates = New System.Windows.Forms.Button
        Me.txtCertificates = New System.Windows.Forms.TextBox
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.panelImport = New System.Windows.Forms.Panel
        Me.btnStartImport = New System.Windows.Forms.Button
        Me.txtImportSource = New System.Windows.Forms.TextBox
        Me.btnImportSource = New System.Windows.Forms.Button
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.panelZip = New System.Windows.Forms.Panel
        Me.lblZipFile = New System.Windows.Forms.Label
        Me.lblSourceDir = New System.Windows.Forms.Label
        Me.txtZipFile = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtSourceDir = New System.Windows.Forms.TextBox
        Me.btnZipIt = New System.Windows.Forms.Button
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog
        Me.TabPage7 = New System.Windows.Forms.TabPage
        Me.panelWormholes = New System.Windows.Forms.Panel
        Me.gbClassLocations = New System.Windows.Forms.GroupBox
        Me.btnGenerateWHClassLocations = New System.Windows.Forms.Button
        Me.btnGenerateWHAttribs = New System.Windows.Forms.Button
        Me.grpMSSQL.SuspendLayout()
        Me.grpMySQL.SuspendLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.panelConvert.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.panelCompress.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.panelSQL2TSQL.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.panelCerts.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.panelImport.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.panelZip.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.panelWormholes.SuspendLayout()
        Me.gbClassLocations.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboConvertType
        '
        Me.cboConvertType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboConvertType.FormattingEnabled = True
        Me.cboConvertType.Items.AddRange(New Object() {"Full", "Basic (Items)", "Standard (Items & Map Routes)"})
        Me.cboConvertType.Location = New System.Drawing.Point(409, 50)
        Me.cboConvertType.Name = "cboConvertType"
        Me.cboConvertType.Size = New System.Drawing.Size(188, 21)
        Me.cboConvertType.TabIndex = 52
        '
        'lblConvertType
        '
        Me.lblConvertType.AutoSize = True
        Me.lblConvertType.Location = New System.Drawing.Point(315, 53)
        Me.lblConvertType.Name = "lblConvertType"
        Me.lblConvertType.Size = New System.Drawing.Size(65, 13)
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
        Me.grpMSSQL.Location = New System.Drawing.Point(110, 77)
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
        Me.lblMSSQLSecurity.Size = New System.Drawing.Size(46, 13)
        Me.lblMSSQLSecurity.TabIndex = 8
        Me.lblMSSQLSecurity.Text = "Security"
        '
        'radMSSQLDatabase
        '
        Me.radMSSQLDatabase.AutoSize = True
        Me.radMSSQLDatabase.Checked = True
        Me.radMSSQLDatabase.Location = New System.Drawing.Point(73, 30)
        Me.radMSSQLDatabase.Name = "radMSSQLDatabase"
        Me.radMSSQLDatabase.Size = New System.Drawing.Size(44, 17)
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
        Me.radMSSQLWindows.Size = New System.Drawing.Size(68, 17)
        Me.radMSSQLWindows.TabIndex = 6
        Me.radMSSQLWindows.Text = "Windows"
        Me.radMSSQLWindows.UseVisualStyleBackColor = True
        '
        'txtMSSQLPassword
        '
        Me.txtMSSQLPassword.Location = New System.Drawing.Point(73, 105)
        Me.txtMSSQLPassword.Name = "txtMSSQLPassword"
        Me.txtMSSQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMSSQLPassword.Size = New System.Drawing.Size(230, 21)
        Me.txtMSSQLPassword.TabIndex = 5
        '
        'txtMSSQLUser
        '
        Me.txtMSSQLUser.Location = New System.Drawing.Point(73, 79)
        Me.txtMSSQLUser.Name = "txtMSSQLUser"
        Me.txtMSSQLUser.Size = New System.Drawing.Size(230, 21)
        Me.txtMSSQLUser.TabIndex = 4
        '
        'txtMSSQLServer
        '
        Me.txtMSSQLServer.Location = New System.Drawing.Point(73, 53)
        Me.txtMSSQLServer.Name = "txtMSSQLServer"
        Me.txtMSSQLServer.Size = New System.Drawing.Size(230, 21)
        Me.txtMSSQLServer.TabIndex = 3
        '
        'lblMSSQLPassword
        '
        Me.lblMSSQLPassword.AutoSize = True
        Me.lblMSSQLPassword.Location = New System.Drawing.Point(6, 108)
        Me.lblMSSQLPassword.Name = "lblMSSQLPassword"
        Me.lblMSSQLPassword.Size = New System.Drawing.Size(57, 13)
        Me.lblMSSQLPassword.TabIndex = 2
        Me.lblMSSQLPassword.Text = "Password:"
        '
        'lblMSSQLUser
        '
        Me.lblMSSQLUser.AutoSize = True
        Me.lblMSSQLUser.Location = New System.Drawing.Point(6, 82)
        Me.lblMSSQLUser.Name = "lblMSSQLUser"
        Me.lblMSSQLUser.Size = New System.Drawing.Size(33, 13)
        Me.lblMSSQLUser.TabIndex = 1
        Me.lblMSSQLUser.Text = "User:"
        '
        'lblMSSQLServer
        '
        Me.lblMSSQLServer.AutoSize = True
        Me.lblMSSQLServer.Location = New System.Drawing.Point(6, 56)
        Me.lblMSSQLServer.Name = "lblMSSQLServer"
        Me.lblMSSQLServer.Size = New System.Drawing.Size(43, 13)
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
        Me.grpMySQL.Location = New System.Drawing.Point(110, 77)
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
        Me.txtMySQLPassword.Size = New System.Drawing.Size(230, 21)
        Me.txtMySQLPassword.TabIndex = 5
        '
        'txtMySQLUser
        '
        Me.txtMySQLUser.Location = New System.Drawing.Point(73, 53)
        Me.txtMySQLUser.Name = "txtMySQLUser"
        Me.txtMySQLUser.Size = New System.Drawing.Size(230, 21)
        Me.txtMySQLUser.TabIndex = 4
        '
        'txtMySQLServer
        '
        Me.txtMySQLServer.Location = New System.Drawing.Point(73, 27)
        Me.txtMySQLServer.Name = "txtMySQLServer"
        Me.txtMySQLServer.Size = New System.Drawing.Size(230, 21)
        Me.txtMySQLServer.TabIndex = 3
        '
        'lblMySQLPassword
        '
        Me.lblMySQLPassword.AutoSize = True
        Me.lblMySQLPassword.Location = New System.Drawing.Point(6, 82)
        Me.lblMySQLPassword.Name = "lblMySQLPassword"
        Me.lblMySQLPassword.Size = New System.Drawing.Size(57, 13)
        Me.lblMySQLPassword.TabIndex = 2
        Me.lblMySQLPassword.Text = "Password:"
        '
        'lblMySQLUser
        '
        Me.lblMySQLUser.AutoSize = True
        Me.lblMySQLUser.Location = New System.Drawing.Point(6, 56)
        Me.lblMySQLUser.Name = "lblMySQLUser"
        Me.lblMySQLUser.Size = New System.Drawing.Size(33, 13)
        Me.lblMySQLUser.TabIndex = 1
        Me.lblMySQLUser.Text = "User:"
        '
        'lblMySQLServer
        '
        Me.lblMySQLServer.AutoSize = True
        Me.lblMySQLServer.Location = New System.Drawing.Point(6, 30)
        Me.lblMySQLServer.Name = "lblMySQLServer"
        Me.lblMySQLServer.Size = New System.Drawing.Size(43, 13)
        Me.lblMySQLServer.TabIndex = 0
        Me.lblMySQLServer.Text = "Server:"
        '
        'cboConvert
        '
        Me.cboConvert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboConvert.FormattingEnabled = True
        Me.cboConvert.Items.AddRange(New Object() {"Access Database (.MDB)", "Comma Separated Files (.CSV)", "MS SQL Server", "MS SQL 2005 Express", "MySQL 5.0"})
        Me.cboConvert.Location = New System.Drawing.Point(110, 50)
        Me.cboConvert.Name = "cboConvert"
        Me.cboConvert.Size = New System.Drawing.Size(188, 21)
        Me.cboConvert.TabIndex = 48
        '
        'lblConvertTo
        '
        Me.lblConvertTo.AutoSize = True
        Me.lblConvertTo.Location = New System.Drawing.Point(16, 53)
        Me.lblConvertTo.Name = "lblConvertTo"
        Me.lblConvertTo.Size = New System.Drawing.Size(65, 13)
        Me.lblConvertTo.TabIndex = 47
        Me.lblConvertTo.Text = "Convert To:"
        '
        'btnCancel
        '
        Me.btnCancel.Enabled = False
        Me.btnCancel.Location = New System.Drawing.Point(125, 383)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(106, 32)
        Me.btnCancel.TabIndex = 46
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(13, 341)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(49, 13)
        Me.lblProgress.TabIndex = 45
        Me.lblProgress.Text = "Progress"
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(13, 357)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(609, 20)
        Me.pbProgress.Step = 1
        Me.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbProgress.TabIndex = 44
        '
        'btnConvert
        '
        Me.btnConvert.Enabled = False
        Me.btnConvert.Location = New System.Drawing.Point(13, 383)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(106, 32)
        Me.btnConvert.TabIndex = 43
        Me.btnConvert.Text = "Convert!"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'lblFiles
        '
        Me.lblFiles.AutoSize = True
        Me.lblFiles.Location = New System.Drawing.Point(107, 251)
        Me.lblFiles.Name = "lblFiles"
        Me.lblFiles.Size = New System.Drawing.Size(159, 13)
        Me.lblFiles.TabIndex = 42
        Me.lblFiles.Text = "Number of files in directory: n/a"
        '
        'btnTarget
        '
        Me.btnTarget.Enabled = False
        Me.btnTarget.Location = New System.Drawing.Point(603, 276)
        Me.btnTarget.Name = "btnTarget"
        Me.btnTarget.Size = New System.Drawing.Size(24, 23)
        Me.btnTarget.TabIndex = 41
        Me.btnTarget.Text = "..."
        Me.btnTarget.UseVisualStyleBackColor = True
        '
        'txtTarget
        '
        Me.txtTarget.Enabled = False
        Me.txtTarget.Location = New System.Drawing.Point(110, 278)
        Me.txtTarget.Name = "txtTarget"
        Me.txtTarget.Size = New System.Drawing.Size(487, 21)
        Me.txtTarget.TabIndex = 40
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.Enabled = False
        Me.lblTarget.Location = New System.Drawing.Point(15, 281)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(90, 13)
        Me.lblTarget.TabIndex = 39
        Me.lblTarget.Text = "Target Directory:"
        '
        'btnSource
        '
        Me.btnSource.Location = New System.Drawing.Point(603, 226)
        Me.btnSource.Name = "btnSource"
        Me.btnSource.Size = New System.Drawing.Size(24, 23)
        Me.btnSource.TabIndex = 38
        Me.btnSource.Text = "..."
        Me.btnSource.UseVisualStyleBackColor = True
        '
        'txtSource
        '
        Me.txtSource.Location = New System.Drawing.Point(110, 228)
        Me.txtSource.Name = "txtSource"
        Me.txtSource.Size = New System.Drawing.Size(487, 21)
        Me.txtSource.TabIndex = 37
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Location = New System.Drawing.Point(15, 231)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(91, 13)
        Me.lblSource.TabIndex = 36
        Me.lblSource.Text = "Source Directory:"
        '
        'lblPurpose
        '
        Me.lblPurpose.AutoSize = True
        Me.lblPurpose.Location = New System.Drawing.Point(13, 14)
        Me.lblPurpose.Name = "lblPurpose"
        Me.lblPurpose.Size = New System.Drawing.Size(376, 13)
        Me.lblPurpose.TabIndex = 35
        Me.lblPurpose.Text = "Please select the type of data you wish to convert the Eve Data Export files." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnMap
        '
        Me.btnMap.Location = New System.Drawing.Point(551, 391)
        Me.btnMap.Name = "btnMap"
        Me.btnMap.Size = New System.Drawing.Size(75, 23)
        Me.btnMap.TabIndex = 53
        Me.btnMap.Text = "Map Test"
        Me.btnMap.UseVisualStyleBackColor = True
        Me.btnMap.Visible = False
        '
        'btnBPS
        '
        Me.btnBPS.Location = New System.Drawing.Point(470, 391)
        Me.btnBPS.Name = "btnBPS"
        Me.btnBPS.Size = New System.Drawing.Size(75, 23)
        Me.btnBPS.TabIndex = 54
        Me.btnBPS.Text = "BP Dump"
        Me.btnBPS.UseVisualStyleBackColor = True
        '
        'btnCSVDump
        '
        Me.btnCSVDump.Location = New System.Drawing.Point(389, 391)
        Me.btnCSVDump.Name = "btnCSVDump"
        Me.btnCSVDump.Size = New System.Drawing.Size(75, 23)
        Me.btnCSVDump.TabIndex = 55
        Me.btnCSVDump.Text = "CSV Dump"
        Me.btnCSVDump.UseVisualStyleBackColor = True
        Me.btnCSVDump.Visible = False
        '
        'btnAddToMySQL
        '
        Me.btnAddToMySQL.Location = New System.Drawing.Point(296, 391)
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
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(654, 463)
        Me.TabControl1.TabIndex = 57
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.panelConvert)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(646, 437)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Data Converter"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'panelConvert
        '
        Me.panelConvert.BackColor = System.Drawing.SystemColors.Control
        Me.panelConvert.Controls.Add(Me.lblPurpose)
        Me.panelConvert.Controls.Add(Me.btnCancel)
        Me.panelConvert.Controls.Add(Me.btnAddToMySQL)
        Me.panelConvert.Controls.Add(Me.lblProgress)
        Me.panelConvert.Controls.Add(Me.lblSource)
        Me.panelConvert.Controls.Add(Me.lblConvertTo)
        Me.panelConvert.Controls.Add(Me.btnCSVDump)
        Me.panelConvert.Controls.Add(Me.pbProgress)
        Me.panelConvert.Controls.Add(Me.txtSource)
        Me.panelConvert.Controls.Add(Me.cboConvert)
        Me.panelConvert.Controls.Add(Me.btnBPS)
        Me.panelConvert.Controls.Add(Me.btnConvert)
        Me.panelConvert.Controls.Add(Me.btnSource)
        Me.panelConvert.Controls.Add(Me.grpMySQL)
        Me.panelConvert.Controls.Add(Me.btnMap)
        Me.panelConvert.Controls.Add(Me.lblFiles)
        Me.panelConvert.Controls.Add(Me.lblTarget)
        Me.panelConvert.Controls.Add(Me.grpMSSQL)
        Me.panelConvert.Controls.Add(Me.cboConvertType)
        Me.panelConvert.Controls.Add(Me.btnTarget)
        Me.panelConvert.Controls.Add(Me.txtTarget)
        Me.panelConvert.Controls.Add(Me.lblConvertType)
        Me.panelConvert.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelConvert.Location = New System.Drawing.Point(3, 3)
        Me.panelConvert.Name = "panelConvert"
        Me.panelConvert.Size = New System.Drawing.Size(640, 431)
        Me.panelConvert.TabIndex = 57
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.panelCompress)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(646, 437)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "File Compression"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'panelCompress
        '
        Me.panelCompress.BackColor = System.Drawing.SystemColors.Control
        Me.panelCompress.Controls.Add(Me.lblCompress)
        Me.panelCompress.Controls.Add(Me.brnStartCompression)
        Me.panelCompress.Controls.Add(Me.btnFileSource)
        Me.panelCompress.Controls.Add(Me.lblCompressedFileInfo)
        Me.panelCompress.Controls.Add(Me.txtCompressFile)
        Me.panelCompress.Controls.Add(Me.lblSourceFileInfo)
        Me.panelCompress.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelCompress.Location = New System.Drawing.Point(3, 3)
        Me.panelCompress.Name = "panelCompress"
        Me.panelCompress.Size = New System.Drawing.Size(640, 431)
        Me.panelCompress.TabIndex = 45
        '
        'lblCompress
        '
        Me.lblCompress.AutoSize = True
        Me.lblCompress.Location = New System.Drawing.Point(5, 10)
        Me.lblCompress.Name = "lblCompress"
        Me.lblCompress.Size = New System.Drawing.Size(291, 13)
        Me.lblCompress.TabIndex = 0
        Me.lblCompress.Text = "Please enter the file name of the file you wish to compress:"
        '
        'brnStartCompression
        '
        Me.brnStartCompression.Location = New System.Drawing.Point(465, 88)
        Me.brnStartCompression.Name = "brnStartCompression"
        Me.brnStartCompression.Size = New System.Drawing.Size(124, 23)
        Me.brnStartCompression.TabIndex = 44
        Me.brnStartCompression.Text = "Start Compression"
        Me.brnStartCompression.UseVisualStyleBackColor = True
        '
        'btnFileSource
        '
        Me.btnFileSource.Location = New System.Drawing.Point(595, 37)
        Me.btnFileSource.Name = "btnFileSource"
        Me.btnFileSource.Size = New System.Drawing.Size(24, 23)
        Me.btnFileSource.TabIndex = 41
        Me.btnFileSource.Text = "..."
        Me.btnFileSource.UseVisualStyleBackColor = True
        '
        'lblCompressedFileInfo
        '
        Me.lblCompressedFileInfo.AutoSize = True
        Me.lblCompressedFileInfo.Location = New System.Drawing.Point(5, 184)
        Me.lblCompressedFileInfo.Name = "lblCompressedFileInfo"
        Me.lblCompressedFileInfo.Size = New System.Drawing.Size(112, 13)
        Me.lblCompressedFileInfo.TabIndex = 43
        Me.lblCompressedFileInfo.Text = "Compressed File Info:"
        '
        'txtCompressFile
        '
        Me.txtCompressFile.Location = New System.Drawing.Point(8, 39)
        Me.txtCompressFile.Name = "txtCompressFile"
        Me.txtCompressFile.Size = New System.Drawing.Size(581, 21)
        Me.txtCompressFile.TabIndex = 40
        '
        'lblSourceFileInfo
        '
        Me.lblSourceFileInfo.AutoSize = True
        Me.lblSourceFileInfo.Location = New System.Drawing.Point(5, 156)
        Me.lblSourceFileInfo.Name = "lblSourceFileInfo"
        Me.lblSourceFileInfo.Size = New System.Drawing.Size(86, 13)
        Me.lblSourceFileInfo.TabIndex = 42
        Me.lblSourceFileInfo.Text = "Source File Info:"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.panelSQL2TSQL)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(646, 437)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "MSSQL To TSQL"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'panelSQL2TSQL
        '
        Me.panelSQL2TSQL.BackColor = System.Drawing.SystemColors.Control
        Me.panelSQL2TSQL.Controls.Add(Me.lblSQLServer)
        Me.panelSQL2TSQL.Controls.Add(Me.lblSQLConversionProgress)
        Me.panelSQL2TSQL.Controls.Add(Me.txtSQLServer)
        Me.panelSQL2TSQL.Controls.Add(Me.btnConvertSQL)
        Me.panelSQL2TSQL.Controls.Add(Me.lblDatabase)
        Me.panelSQL2TSQL.Controls.Add(Me.btnTestConnection)
        Me.panelSQL2TSQL.Controls.Add(Me.txtDatabase)
        Me.panelSQL2TSQL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelSQL2TSQL.Location = New System.Drawing.Point(0, 0)
        Me.panelSQL2TSQL.Name = "panelSQL2TSQL"
        Me.panelSQL2TSQL.Size = New System.Drawing.Size(646, 437)
        Me.panelSQL2TSQL.TabIndex = 14
        '
        'lblSQLServer
        '
        Me.lblSQLServer.AutoSize = True
        Me.lblSQLServer.Location = New System.Drawing.Point(22, 17)
        Me.lblSQLServer.Name = "lblSQLServer"
        Me.lblSQLServer.Size = New System.Drawing.Size(65, 13)
        Me.lblSQLServer.TabIndex = 7
        Me.lblSQLServer.Text = "SQL Server:"
        '
        'lblSQLConversionProgress
        '
        Me.lblSQLConversionProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSQLConversionProgress.Location = New System.Drawing.Point(25, 116)
        Me.lblSQLConversionProgress.Name = "lblSQLConversionProgress"
        Me.lblSQLConversionProgress.Size = New System.Drawing.Size(315, 42)
        Me.lblSQLConversionProgress.TabIndex = 13
        '
        'txtSQLServer
        '
        Me.txtSQLServer.Location = New System.Drawing.Point(93, 14)
        Me.txtSQLServer.Name = "txtSQLServer"
        Me.txtSQLServer.Size = New System.Drawing.Size(247, 21)
        Me.txtSQLServer.TabIndex = 8
        Me.txtSQLServer.Text = "localhost"
        '
        'btnConvertSQL
        '
        Me.btnConvertSQL.Location = New System.Drawing.Point(234, 75)
        Me.btnConvertSQL.Name = "btnConvertSQL"
        Me.btnConvertSQL.Size = New System.Drawing.Size(106, 23)
        Me.btnConvertSQL.TabIndex = 12
        Me.btnConvertSQL.Text = "Convert to T-SQL"
        Me.btnConvertSQL.UseVisualStyleBackColor = True
        '
        'lblDatabase
        '
        Me.lblDatabase.AutoSize = True
        Me.lblDatabase.Location = New System.Drawing.Point(22, 43)
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Size = New System.Drawing.Size(57, 13)
        Me.lblDatabase.TabIndex = 9
        Me.lblDatabase.Text = "Database:"
        '
        'btnTestConnection
        '
        Me.btnTestConnection.Location = New System.Drawing.Point(122, 75)
        Me.btnTestConnection.Name = "btnTestConnection"
        Me.btnTestConnection.Size = New System.Drawing.Size(106, 23)
        Me.btnTestConnection.TabIndex = 11
        Me.btnTestConnection.Text = "Test Connection"
        Me.btnTestConnection.UseVisualStyleBackColor = True
        '
        'txtDatabase
        '
        Me.txtDatabase.Location = New System.Drawing.Point(93, 40)
        Me.txtDatabase.Name = "txtDatabase"
        Me.txtDatabase.Size = New System.Drawing.Size(247, 21)
        Me.txtDatabase.TabIndex = 10
        Me.txtDatabase.Text = "EveHQ"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.panelCerts)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(646, 437)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Certificates"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'panelCerts
        '
        Me.panelCerts.BackColor = System.Drawing.SystemColors.Control
        Me.panelCerts.Controls.Add(Me.lblCertLocation)
        Me.panelCerts.Controls.Add(Me.btnDecodeCertificates)
        Me.panelCerts.Controls.Add(Me.btnFindCertficates)
        Me.panelCerts.Controls.Add(Me.txtCertificates)
        Me.panelCerts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelCerts.Location = New System.Drawing.Point(0, 0)
        Me.panelCerts.Name = "panelCerts"
        Me.panelCerts.Size = New System.Drawing.Size(646, 437)
        Me.panelCerts.TabIndex = 0
        '
        'lblCertLocation
        '
        Me.lblCertLocation.AutoSize = True
        Me.lblCertLocation.Location = New System.Drawing.Point(16, 14)
        Me.lblCertLocation.Name = "lblCertLocation"
        Me.lblCertLocation.Size = New System.Drawing.Size(261, 13)
        Me.lblCertLocation.TabIndex = 45
        Me.lblCertLocation.Text = "Please enter the file name of the Certificates XML file" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnDecodeCertificates
        '
        Me.btnDecodeCertificates.Location = New System.Drawing.Point(476, 74)
        Me.btnDecodeCertificates.Name = "btnDecodeCertificates"
        Me.btnDecodeCertificates.Size = New System.Drawing.Size(124, 23)
        Me.btnDecodeCertificates.TabIndex = 48
        Me.btnDecodeCertificates.Text = "Start Decoding"
        Me.btnDecodeCertificates.UseVisualStyleBackColor = True
        '
        'btnFindCertficates
        '
        Me.btnFindCertficates.Location = New System.Drawing.Point(606, 37)
        Me.btnFindCertficates.Name = "btnFindCertficates"
        Me.btnFindCertficates.Size = New System.Drawing.Size(24, 23)
        Me.btnFindCertficates.TabIndex = 47
        Me.btnFindCertficates.Text = "..."
        Me.btnFindCertficates.UseVisualStyleBackColor = True
        '
        'txtCertificates
        '
        Me.txtCertificates.Location = New System.Drawing.Point(19, 39)
        Me.txtCertificates.Name = "txtCertificates"
        Me.txtCertificates.Size = New System.Drawing.Size(581, 21)
        Me.txtCertificates.TabIndex = 46
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.panelImport)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(646, 437)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Import CSV"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'panelImport
        '
        Me.panelImport.BackColor = System.Drawing.SystemColors.Control
        Me.panelImport.Controls.Add(Me.btnStartImport)
        Me.panelImport.Controls.Add(Me.txtImportSource)
        Me.panelImport.Controls.Add(Me.btnImportSource)
        Me.panelImport.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelImport.Location = New System.Drawing.Point(0, 0)
        Me.panelImport.Name = "panelImport"
        Me.panelImport.Size = New System.Drawing.Size(646, 437)
        Me.panelImport.TabIndex = 0
        '
        'btnStartImport
        '
        Me.btnStartImport.Location = New System.Drawing.Point(8, 39)
        Me.btnStartImport.Name = "btnStartImport"
        Me.btnStartImport.Size = New System.Drawing.Size(75, 23)
        Me.btnStartImport.TabIndex = 41
        Me.btnStartImport.Text = "Start Import"
        Me.btnStartImport.UseVisualStyleBackColor = True
        '
        'txtImportSource
        '
        Me.txtImportSource.Location = New System.Drawing.Point(8, 13)
        Me.txtImportSource.Name = "txtImportSource"
        Me.txtImportSource.Size = New System.Drawing.Size(552, 21)
        Me.txtImportSource.TabIndex = 39
        '
        'btnImportSource
        '
        Me.btnImportSource.Location = New System.Drawing.Point(566, 11)
        Me.btnImportSource.Name = "btnImportSource"
        Me.btnImportSource.Size = New System.Drawing.Size(24, 23)
        Me.btnImportSource.TabIndex = 40
        Me.btnImportSource.Text = "..."
        Me.btnImportSource.UseVisualStyleBackColor = True
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.panelZip)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(646, 437)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Zip"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'panelZip
        '
        Me.panelZip.BackColor = System.Drawing.SystemColors.Control
        Me.panelZip.Controls.Add(Me.lblZipFile)
        Me.panelZip.Controls.Add(Me.lblSourceDir)
        Me.panelZip.Controls.Add(Me.txtZipFile)
        Me.panelZip.Controls.Add(Me.btnBrowse)
        Me.panelZip.Controls.Add(Me.txtSourceDir)
        Me.panelZip.Controls.Add(Me.btnZipIt)
        Me.panelZip.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelZip.Location = New System.Drawing.Point(0, 0)
        Me.panelZip.Name = "panelZip"
        Me.panelZip.Size = New System.Drawing.Size(646, 437)
        Me.panelZip.TabIndex = 0
        '
        'lblZipFile
        '
        Me.lblZipFile.AutoSize = True
        Me.lblZipFile.Location = New System.Drawing.Point(8, 57)
        Me.lblZipFile.Name = "lblZipFile"
        Me.lblZipFile.Size = New System.Drawing.Size(44, 13)
        Me.lblZipFile.TabIndex = 5
        Me.lblZipFile.Text = "Zip File:"
        '
        'lblSourceDir
        '
        Me.lblSourceDir.AutoSize = True
        Me.lblSourceDir.Location = New System.Drawing.Point(8, 31)
        Me.lblSourceDir.Name = "lblSourceDir"
        Me.lblSourceDir.Size = New System.Drawing.Size(60, 13)
        Me.lblSourceDir.TabIndex = 4
        Me.lblSourceDir.Text = "Source Dir:"
        '
        'txtZipFile
        '
        Me.txtZipFile.Location = New System.Drawing.Point(74, 54)
        Me.txtZipFile.Name = "txtZipFile"
        Me.txtZipFile.Size = New System.Drawing.Size(464, 21)
        Me.txtZipFile.TabIndex = 3
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(544, 26)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtSourceDir
        '
        Me.txtSourceDir.Location = New System.Drawing.Point(74, 28)
        Me.txtSourceDir.Name = "txtSourceDir"
        Me.txtSourceDir.Size = New System.Drawing.Size(464, 21)
        Me.txtSourceDir.TabIndex = 1
        '
        'btnZipIt
        '
        Me.btnZipIt.Location = New System.Drawing.Point(544, 52)
        Me.btnZipIt.Name = "btnZipIt"
        Me.btnZipIt.Size = New System.Drawing.Size(75, 23)
        Me.btnZipIt.TabIndex = 0
        Me.btnZipIt.Text = "Zip It !!"
        Me.btnZipIt.UseVisualStyleBackColor = True
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.panelWormholes)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(646, 437)
        Me.TabPage7.TabIndex = 6
        Me.TabPage7.Text = "Wormholes"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'panelWormholes
        '
        Me.panelWormholes.BackColor = System.Drawing.SystemColors.Control
        Me.panelWormholes.Controls.Add(Me.gbClassLocations)
        Me.panelWormholes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelWormholes.Location = New System.Drawing.Point(0, 0)
        Me.panelWormholes.Name = "panelWormholes"
        Me.panelWormholes.Size = New System.Drawing.Size(646, 437)
        Me.panelWormholes.TabIndex = 0
        '
        'gbClassLocations
        '
        Me.gbClassLocations.Controls.Add(Me.btnGenerateWHAttribs)
        Me.gbClassLocations.Controls.Add(Me.btnGenerateWHClassLocations)
        Me.gbClassLocations.Location = New System.Drawing.Point(8, 12)
        Me.gbClassLocations.Name = "gbClassLocations"
        Me.gbClassLocations.Size = New System.Drawing.Size(630, 214)
        Me.gbClassLocations.TabIndex = 0
        Me.gbClassLocations.TabStop = False
        Me.gbClassLocations.Text = "Wormhole Class Locations"
        '
        'btnGenerateWHClassLocations
        '
        Me.btnGenerateWHClassLocations.Location = New System.Drawing.Point(19, 31)
        Me.btnGenerateWHClassLocations.Name = "btnGenerateWHClassLocations"
        Me.btnGenerateWHClassLocations.Size = New System.Drawing.Size(200, 23)
        Me.btnGenerateWHClassLocations.TabIndex = 0
        Me.btnGenerateWHClassLocations.Text = "Generate Class Locations"
        Me.btnGenerateWHClassLocations.UseVisualStyleBackColor = True
        '
        'btnGenerateWHAttribs
        '
        Me.btnGenerateWHAttribs.Location = New System.Drawing.Point(19, 60)
        Me.btnGenerateWHAttribs.Name = "btnGenerateWHAttribs"
        Me.btnGenerateWHAttribs.Size = New System.Drawing.Size(200, 23)
        Me.btnGenerateWHAttribs.TabIndex = 1
        Me.btnGenerateWHAttribs.Text = "Generate WH Attribs"
        Me.btnGenerateWHAttribs.UseVisualStyleBackColor = True
        '
        'frmDataConvert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(654, 463)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.panelConvert.ResumeLayout(False)
        Me.panelConvert.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.panelCompress.ResumeLayout(False)
        Me.panelCompress.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.panelSQL2TSQL.ResumeLayout(False)
        Me.panelSQL2TSQL.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.panelCerts.ResumeLayout(False)
        Me.panelCerts.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.panelImport.ResumeLayout(False)
        Me.panelImport.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.panelZip.ResumeLayout(False)
        Me.panelZip.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.panelWormholes.ResumeLayout(False)
        Me.gbClassLocations.ResumeLayout(False)
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
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents lblSQLConversionProgress As System.Windows.Forms.Label
    Friend WithEvents btnConvertSQL As System.Windows.Forms.Button
    Friend WithEvents btnTestConnection As System.Windows.Forms.Button
    Friend WithEvents txtDatabase As System.Windows.Forms.TextBox
    Friend WithEvents lblDatabase As System.Windows.Forms.Label
    Friend WithEvents txtSQLServer As System.Windows.Forms.TextBox
    Friend WithEvents lblSQLServer As System.Windows.Forms.Label
    Friend WithEvents panelCompress As System.Windows.Forms.Panel
    Friend WithEvents panelSQL2TSQL As System.Windows.Forms.Panel
    Friend WithEvents panelConvert As System.Windows.Forms.Panel
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents panelCerts As System.Windows.Forms.Panel
    Friend WithEvents lblCertLocation As System.Windows.Forms.Label
    Friend WithEvents btnDecodeCertificates As System.Windows.Forms.Button
    Friend WithEvents btnFindCertficates As System.Windows.Forms.Button
    Friend WithEvents txtCertificates As System.Windows.Forms.TextBox
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents panelImport As System.Windows.Forms.Panel
    Friend WithEvents txtImportSource As System.Windows.Forms.TextBox
    Friend WithEvents btnImportSource As System.Windows.Forms.Button
    Friend WithEvents btnStartImport As System.Windows.Forms.Button
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents panelZip As System.Windows.Forms.Panel
    Friend WithEvents btnZipIt As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtSourceDir As System.Windows.Forms.TextBox
    Friend WithEvents lblZipFile As System.Windows.Forms.Label
    Friend WithEvents lblSourceDir As System.Windows.Forms.Label
    Friend WithEvents txtZipFile As System.Windows.Forms.TextBox
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents panelWormholes As System.Windows.Forms.Panel
    Friend WithEvents gbClassLocations As System.Windows.Forms.GroupBox
    Friend WithEvents btnGenerateWHClassLocations As System.Windows.Forms.Button
    Friend WithEvents btnGenerateWHAttribs As System.Windows.Forms.Button
End Class
