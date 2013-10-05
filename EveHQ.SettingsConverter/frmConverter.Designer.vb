<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmConverter
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmConverter))
        Me.btnConvert = New System.Windows.Forms.Button()
        Me.radSQLCE = New System.Windows.Forms.RadioButton()
        Me.radSQL = New System.Windows.Forms.RadioButton()
        Me.lblSelectSource = New System.Windows.Forms.Label()
        Me.lblSQLServer = New System.Windows.Forms.Label()
        Me.txtSQLCEFile = New System.Windows.Forms.TextBox()
        Me.lblSQLCEFile = New System.Windows.Forms.Label()
        Me.lblSQLDatabase = New System.Windows.Forms.Label()
        Me.cboSQLServer = New System.Windows.Forms.ComboBox()
        Me.btnBrowseSQLCE = New System.Windows.Forms.Button()
        Me.gbConversion = New System.Windows.Forms.GroupBox()
        Me.cp1 = New DevComponents.DotNetBar.Controls.CircularProgress()
        Me.lblConversion = New System.Windows.Forms.Label()
        Me.cboSQLDatabase = New System.Windows.Forms.ComboBox()
        Me.gbApplicationSettings = New System.Windows.Forms.GroupBox()
        Me.lblSettingsFolder = New System.Windows.Forms.Label()
        Me.txtSettingsFolder = New System.Windows.Forms.TextBox()
        Me.btnBrowseSettingsFolder = New System.Windows.Forms.Button()
        Me.chkUsingLocalSwitch = New System.Windows.Forms.CheckBox()
        Me.gbDatabaseSettings = New System.Windows.Forms.GroupBox()
        Me.btnRefreshDatabases = New System.Windows.Forms.Button()
        Me.btnRefreshServers = New System.Windows.Forms.Button()
        Me.lblSQLPassword = New System.Windows.Forms.Label()
        Me.txtSQLUsername = New System.Windows.Forms.TextBox()
        Me.txtSQLPassword = New System.Windows.Forms.TextBox()
        Me.lblSQLUsername = New System.Windows.Forms.Label()
        Me.chkUseSQLSecurity = New System.Windows.Forms.CheckBox()
        Me.gbConversion.SuspendLayout()
        Me.gbApplicationSettings.SuspendLayout()
        Me.gbDatabaseSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnConvert
        '
        Me.btnConvert.Location = New System.Drawing.Point(225, 326)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(239, 23)
        Me.btnConvert.TabIndex = 0
        Me.btnConvert.Text = "Convert EveHQ Settings"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'radSQLCE
        '
        Me.radSQLCE.AutoSize = True
        Me.radSQLCE.Checked = True
        Me.radSQLCE.Location = New System.Drawing.Point(147, 25)
        Me.radSQLCE.Name = "radSQLCE"
        Me.radSQLCE.Size = New System.Drawing.Size(57, 17)
        Me.radSQLCE.TabIndex = 2
        Me.radSQLCE.TabStop = True
        Me.radSQLCE.Text = "SQLCE"
        Me.radSQLCE.UseVisualStyleBackColor = True
        '
        'radSQL
        '
        Me.radSQL.AutoSize = True
        Me.radSQL.Location = New System.Drawing.Point(213, 25)
        Me.radSQL.Name = "radSQL"
        Me.radSQL.Size = New System.Drawing.Size(113, 17)
        Me.radSQL.TabIndex = 3
        Me.radSQL.Text = "SQL (Full/Express)"
        Me.radSQL.UseVisualStyleBackColor = True
        '
        'lblSelectSource
        '
        Me.lblSelectSource.AutoSize = True
        Me.lblSelectSource.Location = New System.Drawing.Point(15, 27)
        Me.lblSelectSource.Name = "lblSelectSource"
        Me.lblSelectSource.Size = New System.Drawing.Size(125, 13)
        Me.lblSelectSource.TabIndex = 4
        Me.lblSelectSource.Text = "Select Database Source:"
        '
        'lblSQLServer
        '
        Me.lblSQLServer.AutoSize = True
        Me.lblSQLServer.Enabled = False
        Me.lblSQLServer.Location = New System.Drawing.Point(15, 86)
        Me.lblSQLServer.Name = "lblSQLServer"
        Me.lblSQLServer.Size = New System.Drawing.Size(95, 13)
        Me.lblSQLServer.TabIndex = 5
        Me.lblSQLServer.Text = "SQL Server Name:"
        '
        'txtSQLCEFile
        '
        Me.txtSQLCEFile.Location = New System.Drawing.Point(147, 57)
        Me.txtSQLCEFile.Name = "txtSQLCEFile"
        Me.txtSQLCEFile.Size = New System.Drawing.Size(464, 21)
        Me.txtSQLCEFile.TabIndex = 8
        '
        'lblSQLCEFile
        '
        Me.lblSQLCEFile.AutoSize = True
        Me.lblSQLCEFile.Location = New System.Drawing.Point(15, 60)
        Me.lblSQLCEFile.Name = "lblSQLCEFile"
        Me.lblSQLCEFile.Size = New System.Drawing.Size(88, 13)
        Me.lblSQLCEFile.TabIndex = 7
        Me.lblSQLCEFile.Text = "SQLCE Filename:"
        '
        'lblSQLDatabase
        '
        Me.lblSQLDatabase.AutoSize = True
        Me.lblSQLDatabase.Enabled = False
        Me.lblSQLDatabase.Location = New System.Drawing.Point(15, 113)
        Me.lblSQLDatabase.Name = "lblSQLDatabase"
        Me.lblSQLDatabase.Size = New System.Drawing.Size(109, 13)
        Me.lblSQLDatabase.TabIndex = 9
        Me.lblSQLDatabase.Text = "SQL Database Name:"
        '
        'cboSQLServer
        '
        Me.cboSQLServer.FormattingEnabled = True
        Me.cboSQLServer.Location = New System.Drawing.Point(147, 83)
        Me.cboSQLServer.Name = "cboSQLServer"
        Me.cboSQLServer.Size = New System.Drawing.Size(179, 21)
        Me.cboSQLServer.TabIndex = 11
        '
        'btnBrowseSQLCE
        '
        Me.btnBrowseSQLCE.Location = New System.Drawing.Point(615, 57)
        Me.btnBrowseSQLCE.Name = "btnBrowseSQLCE"
        Me.btnBrowseSQLCE.Size = New System.Drawing.Size(64, 21)
        Me.btnBrowseSQLCE.TabIndex = 12
        Me.btnBrowseSQLCE.Text = "Browse"
        Me.btnBrowseSQLCE.UseVisualStyleBackColor = True
        '
        'gbConversion
        '
        Me.gbConversion.Controls.Add(Me.cp1)
        Me.gbConversion.Controls.Add(Me.lblConversion)
        Me.gbConversion.Enabled = False
        Me.gbConversion.Location = New System.Drawing.Point(12, 355)
        Me.gbConversion.Name = "gbConversion"
        Me.gbConversion.Size = New System.Drawing.Size(685, 57)
        Me.gbConversion.TabIndex = 14
        Me.gbConversion.TabStop = False
        Me.gbConversion.Text = "Conversion Progress"
        '
        'cp1
        '
        '
        '
        '
        Me.cp1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cp1.Location = New System.Drawing.Point(637, 8)
        Me.cp1.Name = "cp1"
        Me.cp1.ProgressColor = System.Drawing.Color.Green
        Me.cp1.Size = New System.Drawing.Size(42, 42)
        Me.cp1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP
        Me.cp1.TabIndex = 1
        '
        'lblConversion
        '
        Me.lblConversion.AutoSize = True
        Me.lblConversion.Location = New System.Drawing.Point(16, 26)
        Me.lblConversion.Name = "lblConversion"
        Me.lblConversion.Size = New System.Drawing.Size(118, 13)
        Me.lblConversion.TabIndex = 0
        Me.lblConversion.Text = "Conversion Status: n/a"
        '
        'cboSQLDatabase
        '
        Me.cboSQLDatabase.FormattingEnabled = True
        Me.cboSQLDatabase.Location = New System.Drawing.Point(147, 110)
        Me.cboSQLDatabase.Name = "cboSQLDatabase"
        Me.cboSQLDatabase.Size = New System.Drawing.Size(179, 21)
        Me.cboSQLDatabase.TabIndex = 15
        '
        'gbApplicationSettings
        '
        Me.gbApplicationSettings.Controls.Add(Me.lblSettingsFolder)
        Me.gbApplicationSettings.Controls.Add(Me.txtSettingsFolder)
        Me.gbApplicationSettings.Controls.Add(Me.btnBrowseSettingsFolder)
        Me.gbApplicationSettings.Controls.Add(Me.chkUsingLocalSwitch)
        Me.gbApplicationSettings.Location = New System.Drawing.Point(12, 12)
        Me.gbApplicationSettings.Name = "gbApplicationSettings"
        Me.gbApplicationSettings.Size = New System.Drawing.Size(685, 71)
        Me.gbApplicationSettings.TabIndex = 16
        Me.gbApplicationSettings.TabStop = False
        Me.gbApplicationSettings.Text = "EveHQ Application Settings"
        '
        'lblSettingsFolder
        '
        Me.lblSettingsFolder.AutoSize = True
        Me.lblSettingsFolder.Location = New System.Drawing.Point(15, 49)
        Me.lblSettingsFolder.Name = "lblSettingsFolder"
        Me.lblSettingsFolder.Size = New System.Drawing.Size(83, 13)
        Me.lblSettingsFolder.TabIndex = 15
        Me.lblSettingsFolder.Text = "Settings Folder:"
        '
        'txtSettingsFolder
        '
        Me.txtSettingsFolder.Location = New System.Drawing.Point(147, 44)
        Me.txtSettingsFolder.Name = "txtSettingsFolder"
        Me.txtSettingsFolder.Size = New System.Drawing.Size(464, 21)
        Me.txtSettingsFolder.TabIndex = 14
        '
        'btnBrowseSettingsFolder
        '
        Me.btnBrowseSettingsFolder.Location = New System.Drawing.Point(615, 44)
        Me.btnBrowseSettingsFolder.Name = "btnBrowseSettingsFolder"
        Me.btnBrowseSettingsFolder.Size = New System.Drawing.Size(64, 21)
        Me.btnBrowseSettingsFolder.TabIndex = 13
        Me.btnBrowseSettingsFolder.Text = "Browse"
        Me.btnBrowseSettingsFolder.UseVisualStyleBackColor = True
        '
        'chkUsingLocalSwitch
        '
        Me.chkUsingLocalSwitch.AutoSize = True
        Me.chkUsingLocalSwitch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUsingLocalSwitch.Location = New System.Drawing.Point(13, 25)
        Me.chkUsingLocalSwitch.Name = "chkUsingLocalSwitch"
        Me.chkUsingLocalSwitch.Size = New System.Drawing.Size(118, 17)
        Me.chkUsingLocalSwitch.TabIndex = 0
        Me.chkUsingLocalSwitch.Text = "Using Local Switch?"
        Me.chkUsingLocalSwitch.UseVisualStyleBackColor = True
        '
        'gbDatabaseSettings
        '
        Me.gbDatabaseSettings.Controls.Add(Me.btnRefreshDatabases)
        Me.gbDatabaseSettings.Controls.Add(Me.btnRefreshServers)
        Me.gbDatabaseSettings.Controls.Add(Me.lblSQLPassword)
        Me.gbDatabaseSettings.Controls.Add(Me.txtSQLUsername)
        Me.gbDatabaseSettings.Controls.Add(Me.txtSQLPassword)
        Me.gbDatabaseSettings.Controls.Add(Me.btnBrowseSQLCE)
        Me.gbDatabaseSettings.Controls.Add(Me.lblSQLUsername)
        Me.gbDatabaseSettings.Controls.Add(Me.chkUseSQLSecurity)
        Me.gbDatabaseSettings.Controls.Add(Me.lblSelectSource)
        Me.gbDatabaseSettings.Controls.Add(Me.radSQLCE)
        Me.gbDatabaseSettings.Controls.Add(Me.cboSQLDatabase)
        Me.gbDatabaseSettings.Controls.Add(Me.radSQL)
        Me.gbDatabaseSettings.Controls.Add(Me.lblSQLServer)
        Me.gbDatabaseSettings.Controls.Add(Me.lblSQLCEFile)
        Me.gbDatabaseSettings.Controls.Add(Me.cboSQLServer)
        Me.gbDatabaseSettings.Controls.Add(Me.txtSQLCEFile)
        Me.gbDatabaseSettings.Controls.Add(Me.lblSQLDatabase)
        Me.gbDatabaseSettings.Location = New System.Drawing.Point(12, 89)
        Me.gbDatabaseSettings.Name = "gbDatabaseSettings"
        Me.gbDatabaseSettings.Size = New System.Drawing.Size(685, 222)
        Me.gbDatabaseSettings.TabIndex = 17
        Me.gbDatabaseSettings.TabStop = False
        Me.gbDatabaseSettings.Text = "EveHQ Database Settings"
        '
        'btnRefreshDatabases
        '
        Me.btnRefreshDatabases.Location = New System.Drawing.Point(332, 110)
        Me.btnRefreshDatabases.Name = "btnRefreshDatabases"
        Me.btnRefreshDatabases.Size = New System.Drawing.Size(150, 21)
        Me.btnRefreshDatabases.TabIndex = 23
        Me.btnRefreshDatabases.Text = "Refresh Databases"
        Me.btnRefreshDatabases.UseVisualStyleBackColor = True
        '
        'btnRefreshServers
        '
        Me.btnRefreshServers.Location = New System.Drawing.Point(332, 83)
        Me.btnRefreshServers.Name = "btnRefreshServers"
        Me.btnRefreshServers.Size = New System.Drawing.Size(150, 21)
        Me.btnRefreshServers.TabIndex = 22
        Me.btnRefreshServers.Text = "Refresh SQL Instances"
        Me.btnRefreshServers.UseVisualStyleBackColor = True
        '
        'lblSQLPassword
        '
        Me.lblSQLPassword.AutoSize = True
        Me.lblSQLPassword.Enabled = False
        Me.lblSQLPassword.Location = New System.Drawing.Point(15, 194)
        Me.lblSQLPassword.Name = "lblSQLPassword"
        Me.lblSQLPassword.Size = New System.Drawing.Size(79, 13)
        Me.lblSQLPassword.TabIndex = 21
        Me.lblSQLPassword.Text = "SQL Password:"
        '
        'txtSQLUsername
        '
        Me.txtSQLUsername.Location = New System.Drawing.Point(147, 164)
        Me.txtSQLUsername.Name = "txtSQLUsername"
        Me.txtSQLUsername.Size = New System.Drawing.Size(179, 21)
        Me.txtSQLUsername.TabIndex = 20
        '
        'txtSQLPassword
        '
        Me.txtSQLPassword.Location = New System.Drawing.Point(147, 191)
        Me.txtSQLPassword.Name = "txtSQLPassword"
        Me.txtSQLPassword.Size = New System.Drawing.Size(179, 21)
        Me.txtSQLPassword.TabIndex = 19
        '
        'lblSQLUsername
        '
        Me.lblSQLUsername.AutoSize = True
        Me.lblSQLUsername.Enabled = False
        Me.lblSQLUsername.Location = New System.Drawing.Point(15, 167)
        Me.lblSQLUsername.Name = "lblSQLUsername"
        Me.lblSQLUsername.Size = New System.Drawing.Size(81, 13)
        Me.lblSQLUsername.TabIndex = 18
        Me.lblSQLUsername.Text = "SQL Username:"
        '
        'chkUseSQLSecurity
        '
        Me.chkUseSQLSecurity.AutoSize = True
        Me.chkUseSQLSecurity.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUseSQLSecurity.Location = New System.Drawing.Point(18, 139)
        Me.chkUseSQLSecurity.Name = "chkUseSQLSecurity"
        Me.chkUseSQLSecurity.Size = New System.Drawing.Size(113, 17)
        Me.chkUseSQLSecurity.TabIndex = 16
        Me.chkUseSQLSecurity.Text = "Use SQL Security?"
        Me.chkUseSQLSecurity.UseVisualStyleBackColor = True
        '
        'FrmConverter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 419)
        Me.Controls.Add(Me.gbDatabaseSettings)
        Me.Controls.Add(Me.gbApplicationSettings)
        Me.Controls.Add(Me.gbConversion)
        Me.Controls.Add(Me.btnConvert)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmConverter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EveHQ v2.12 Settings/Database Converter"
        Me.gbConversion.ResumeLayout(False)
        Me.gbConversion.PerformLayout()
        Me.gbApplicationSettings.ResumeLayout(False)
        Me.gbApplicationSettings.PerformLayout()
        Me.gbDatabaseSettings.ResumeLayout(False)
        Me.gbDatabaseSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnConvert As System.Windows.Forms.Button
    Friend WithEvents radSQLCE As System.Windows.Forms.RadioButton
    Friend WithEvents radSQL As System.Windows.Forms.RadioButton
    Friend WithEvents lblSelectSource As System.Windows.Forms.Label
    Friend WithEvents lblSQLServer As System.Windows.Forms.Label
    Friend WithEvents txtSQLCEFile As System.Windows.Forms.TextBox
    Friend WithEvents lblSQLCEFile As System.Windows.Forms.Label
    Friend WithEvents lblSQLDatabase As System.Windows.Forms.Label
    Friend WithEvents cboSQLServer As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseSQLCE As System.Windows.Forms.Button
    Friend WithEvents gbConversion As System.Windows.Forms.GroupBox
    Friend WithEvents lblConversion As System.Windows.Forms.Label
    Friend WithEvents cp1 As DevComponents.DotNetBar.Controls.CircularProgress
    Friend WithEvents cboSQLDatabase As System.Windows.Forms.ComboBox
    Friend WithEvents gbApplicationSettings As System.Windows.Forms.GroupBox
    Friend WithEvents chkUsingLocalSwitch As System.Windows.Forms.CheckBox
    Friend WithEvents btnBrowseSettingsFolder As System.Windows.Forms.Button
    Friend WithEvents gbDatabaseSettings As System.Windows.Forms.GroupBox
    Friend WithEvents lblSQLPassword As System.Windows.Forms.Label
    Friend WithEvents txtSQLUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtSQLPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblSQLUsername As System.Windows.Forms.Label
    Friend WithEvents chkUseSQLSecurity As System.Windows.Forms.CheckBox
    Friend WithEvents btnRefreshDatabases As System.Windows.Forms.Button
    Friend WithEvents btnRefreshServers As System.Windows.Forms.Button
    Friend WithEvents lblSettingsFolder As System.Windows.Forms.Label
    Friend WithEvents txtSettingsFolder As System.Windows.Forms.TextBox

End Class
