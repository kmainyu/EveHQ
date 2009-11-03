<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBackupEveHQ
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBackupEveHQ))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnRestore = New System.Windows.Forms.Button
        Me.lvwBackups = New System.Windows.Forms.ListView
        Me.BackupData = New System.Windows.Forms.ColumnHeader
        Me.btnScan = New System.Windows.Forms.Button
        Me.gbBackup = New System.Windows.Forms.GroupBox
        Me.btnResetBackup = New System.Windows.Forms.Button
        Me.btnBackup = New System.Windows.Forms.Button
        Me.lblNextBackupLbl = New System.Windows.Forms.Label
        Me.lblLastBackup = New System.Windows.Forms.Label
        Me.lblNextBackup = New System.Windows.Forms.Label
        Me.lblLastBackupLbl = New System.Windows.Forms.Label
        Me.lblStartFormat = New System.Windows.Forms.Label
        Me.dtpStart = New System.Windows.Forms.DateTimePicker
        Me.lblBackupStart = New System.Windows.Forms.Label
        Me.lblBackupDays = New System.Windows.Forms.Label
        Me.nudDays = New System.Windows.Forms.NumericUpDown
        Me.lblBackupFreq = New System.Windows.Forms.Label
        Me.radManualBackup = New System.Windows.Forms.RadioButton
        Me.radPromptBackup = New System.Windows.Forms.RadioButton
        Me.lblBackupWarningDays = New System.Windows.Forms.Label
        Me.nudBackupWarning = New System.Windows.Forms.NumericUpDown
        Me.lblBackupWarning = New System.Windows.Forms.Label
        Me.RadAutoBackup = New System.Windows.Forms.RadioButton
        Me.GroupBox1.SuspendLayout()
        Me.gbBackup.SuspendLayout()
        CType(Me.nudDays, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBackupWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnRestore)
        Me.GroupBox1.Controls.Add(Me.lvwBackups)
        Me.GroupBox1.Controls.Add(Me.btnScan)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 266)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(612, 292)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "EveHQ Settings Restore"
        '
        'btnRestore
        '
        Me.btnRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRestore.Location = New System.Drawing.Point(5, 258)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(92, 23)
        Me.btnRestore.TabIndex = 14
        Me.btnRestore.Text = "Restore Now!"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'lvwBackups
        '
        Me.lvwBackups.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwBackups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.BackupData})
        Me.lvwBackups.FullRowSelect = True
        Me.lvwBackups.GridLines = True
        Me.lvwBackups.Location = New System.Drawing.Point(5, 73)
        Me.lvwBackups.Name = "lvwBackups"
        Me.lvwBackups.Size = New System.Drawing.Size(601, 179)
        Me.lvwBackups.TabIndex = 13
        Me.lvwBackups.UseCompatibleStateImageBehavior = False
        Me.lvwBackups.View = System.Windows.Forms.View.Details
        '
        'BackupData
        '
        Me.BackupData.Text = "Backup Details"
        Me.BackupData.Width = 500
        '
        'btnScan
        '
        Me.btnScan.Location = New System.Drawing.Point(5, 31)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(137, 23)
        Me.btnScan.TabIndex = 12
        Me.btnScan.Text = "Scan Backup Directory"
        Me.btnScan.UseVisualStyleBackColor = True
        '
        'gbBackup
        '
        Me.gbBackup.Controls.Add(Me.RadAutoBackup)
        Me.gbBackup.Controls.Add(Me.lblBackupWarningDays)
        Me.gbBackup.Controls.Add(Me.nudBackupWarning)
        Me.gbBackup.Controls.Add(Me.lblBackupWarning)
        Me.gbBackup.Controls.Add(Me.radPromptBackup)
        Me.gbBackup.Controls.Add(Me.radManualBackup)
        Me.gbBackup.Controls.Add(Me.btnResetBackup)
        Me.gbBackup.Controls.Add(Me.btnBackup)
        Me.gbBackup.Controls.Add(Me.lblNextBackupLbl)
        Me.gbBackup.Controls.Add(Me.lblLastBackup)
        Me.gbBackup.Controls.Add(Me.lblNextBackup)
        Me.gbBackup.Controls.Add(Me.lblLastBackupLbl)
        Me.gbBackup.Controls.Add(Me.lblStartFormat)
        Me.gbBackup.Controls.Add(Me.dtpStart)
        Me.gbBackup.Controls.Add(Me.lblBackupStart)
        Me.gbBackup.Controls.Add(Me.lblBackupDays)
        Me.gbBackup.Controls.Add(Me.nudDays)
        Me.gbBackup.Controls.Add(Me.lblBackupFreq)
        Me.gbBackup.Location = New System.Drawing.Point(12, 12)
        Me.gbBackup.Name = "gbBackup"
        Me.gbBackup.Size = New System.Drawing.Size(612, 248)
        Me.gbBackup.TabIndex = 2
        Me.gbBackup.TabStop = False
        Me.gbBackup.Text = "EveHQ Settings Backup"
        '
        'btnResetBackup
        '
        Me.btnResetBackup.Location = New System.Drawing.Point(497, 20)
        Me.btnResetBackup.Name = "btnResetBackup"
        Me.btnResetBackup.Size = New System.Drawing.Size(109, 23)
        Me.btnResetBackup.TabIndex = 13
        Me.btnResetBackup.Text = "Reset Last Backup"
        Me.btnResetBackup.UseVisualStyleBackColor = True
        '
        'btnBackup
        '
        Me.btnBackup.Location = New System.Drawing.Point(399, 20)
        Me.btnBackup.Name = "btnBackup"
        Me.btnBackup.Size = New System.Drawing.Size(92, 23)
        Me.btnBackup.TabIndex = 12
        Me.btnBackup.Text = "Backup Now!"
        Me.btnBackup.UseVisualStyleBackColor = True
        '
        'lblNextBackupLbl
        '
        Me.lblNextBackupLbl.AutoSize = True
        Me.lblNextBackupLbl.Enabled = False
        Me.lblNextBackupLbl.Location = New System.Drawing.Point(54, 219)
        Me.lblNextBackupLbl.Name = "lblNextBackupLbl"
        Me.lblNextBackupLbl.Size = New System.Drawing.Size(71, 13)
        Me.lblNextBackupLbl.TabIndex = 11
        Me.lblNextBackupLbl.Text = "Next Backup:"
        '
        'lblLastBackup
        '
        Me.lblLastBackup.AutoSize = True
        Me.lblLastBackup.Location = New System.Drawing.Point(138, 25)
        Me.lblLastBackup.Name = "lblLastBackup"
        Me.lblLastBackup.Size = New System.Drawing.Size(66, 13)
        Me.lblLastBackup.TabIndex = 10
        Me.lblLastBackup.Text = "<unknown>"
        '
        'lblNextBackup
        '
        Me.lblNextBackup.AutoSize = True
        Me.lblNextBackup.Enabled = False
        Me.lblNextBackup.Location = New System.Drawing.Point(157, 219)
        Me.lblNextBackup.Name = "lblNextBackup"
        Me.lblNextBackup.Size = New System.Drawing.Size(66, 13)
        Me.lblNextBackup.TabIndex = 9
        Me.lblNextBackup.Text = "<unknown>"
        '
        'lblLastBackupLbl
        '
        Me.lblLastBackupLbl.AutoSize = True
        Me.lblLastBackupLbl.Location = New System.Drawing.Point(15, 25)
        Me.lblLastBackupLbl.Name = "lblLastBackupLbl"
        Me.lblLastBackupLbl.Size = New System.Drawing.Size(117, 13)
        Me.lblLastBackupLbl.TabIndex = 8
        Me.lblLastBackupLbl.Text = "Last Recorded Backup:"
        '
        'lblStartFormat
        '
        Me.lblStartFormat.AutoSize = True
        Me.lblStartFormat.Enabled = False
        Me.lblStartFormat.Location = New System.Drawing.Point(295, 191)
        Me.lblStartFormat.Name = "lblStartFormat"
        Me.lblStartFormat.Size = New System.Drawing.Size(164, 13)
        Me.lblStartFormat.TabIndex = 7
        Me.lblStartFormat.Text = "(in ""dd/mm/yyyy hh:mm"" format)"
        '
        'dtpStart
        '
        Me.dtpStart.CustomFormat = "dd/MM/yyyy HH:mm"
        Me.dtpStart.Enabled = False
        Me.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStart.Location = New System.Drawing.Point(160, 186)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.ShowUpDown = True
        Me.dtpStart.Size = New System.Drawing.Size(129, 21)
        Me.dtpStart.TabIndex = 6
        Me.dtpStart.Tag = "1"
        Me.dtpStart.Value = New Date(2006, 3, 10, 0, 0, 0, 0)
        '
        'lblBackupStart
        '
        Me.lblBackupStart.AutoSize = True
        Me.lblBackupStart.Enabled = False
        Me.lblBackupStart.Location = New System.Drawing.Point(54, 191)
        Me.lblBackupStart.Name = "lblBackupStart"
        Me.lblBackupStart.Size = New System.Drawing.Size(87, 13)
        Me.lblBackupStart.TabIndex = 4
        Me.lblBackupStart.Text = "Start Date/Time:"
        '
        'lblBackupDays
        '
        Me.lblBackupDays.AutoSize = True
        Me.lblBackupDays.Enabled = False
        Me.lblBackupDays.Location = New System.Drawing.Point(202, 161)
        Me.lblBackupDays.Name = "lblBackupDays"
        Me.lblBackupDays.Size = New System.Drawing.Size(39, 13)
        Me.lblBackupDays.TabIndex = 3
        Me.lblBackupDays.Text = "(Days)"
        '
        'nudDays
        '
        Me.nudDays.Enabled = False
        Me.nudDays.Location = New System.Drawing.Point(160, 159)
        Me.nudDays.Maximum = New Decimal(New Integer() {28, 0, 0, 0})
        Me.nudDays.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDays.Name = "nudDays"
        Me.nudDays.Size = New System.Drawing.Size(36, 21)
        Me.nudDays.TabIndex = 2
        Me.nudDays.Tag = "1"
        Me.nudDays.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblBackupFreq
        '
        Me.lblBackupFreq.AutoSize = True
        Me.lblBackupFreq.Enabled = False
        Me.lblBackupFreq.Location = New System.Drawing.Point(54, 161)
        Me.lblBackupFreq.Name = "lblBackupFreq"
        Me.lblBackupFreq.Size = New System.Drawing.Size(99, 13)
        Me.lblBackupFreq.TabIndex = 1
        Me.lblBackupFreq.Text = "Backup Frequency:"
        '
        'radManualBackup
        '
        Me.radManualBackup.AutoSize = True
        Me.radManualBackup.Location = New System.Drawing.Point(18, 54)
        Me.radManualBackup.Name = "radManualBackup"
        Me.radManualBackup.Size = New System.Drawing.Size(125, 17)
        Me.radManualBackup.TabIndex = 14
        Me.radManualBackup.Text = "Manual Backup Mode"
        Me.radManualBackup.UseVisualStyleBackColor = True
        '
        'radPromptBackup
        '
        Me.radPromptBackup.AutoSize = True
        Me.radPromptBackup.Location = New System.Drawing.Point(18, 79)
        Me.radPromptBackup.Name = "radPromptBackup"
        Me.radPromptBackup.Size = New System.Drawing.Size(131, 17)
        Me.radPromptBackup.TabIndex = 15
        Me.radPromptBackup.Text = "Backup Warning Mode"
        Me.radPromptBackup.UseVisualStyleBackColor = True
        '
        'lblBackupWarningDays
        '
        Me.lblBackupWarningDays.AutoSize = True
        Me.lblBackupWarningDays.Enabled = False
        Me.lblBackupWarningDays.Location = New System.Drawing.Point(202, 104)
        Me.lblBackupWarningDays.Name = "lblBackupWarningDays"
        Me.lblBackupWarningDays.Size = New System.Drawing.Size(39, 13)
        Me.lblBackupWarningDays.TabIndex = 18
        Me.lblBackupWarningDays.Text = "(Days)"
        '
        'nudBackupWarning
        '
        Me.nudBackupWarning.Enabled = False
        Me.nudBackupWarning.Location = New System.Drawing.Point(160, 102)
        Me.nudBackupWarning.Maximum = New Decimal(New Integer() {28, 0, 0, 0})
        Me.nudBackupWarning.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudBackupWarning.Name = "nudBackupWarning"
        Me.nudBackupWarning.Size = New System.Drawing.Size(36, 21)
        Me.nudBackupWarning.TabIndex = 17
        Me.nudBackupWarning.Tag = "1"
        Me.nudBackupWarning.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'lblBackupWarning
        '
        Me.lblBackupWarning.AutoSize = True
        Me.lblBackupWarning.Enabled = False
        Me.lblBackupWarning.Location = New System.Drawing.Point(49, 104)
        Me.lblBackupWarning.Name = "lblBackupWarning"
        Me.lblBackupWarning.Size = New System.Drawing.Size(105, 13)
        Me.lblBackupWarning.TabIndex = 16
        Me.lblBackupWarning.Text = "Warning Frequency:"
        '
        'RadAutoBackup
        '
        Me.RadAutoBackup.AutoSize = True
        Me.RadAutoBackup.Location = New System.Drawing.Point(18, 133)
        Me.RadAutoBackup.Name = "RadAutoBackup"
        Me.RadAutoBackup.Size = New System.Drawing.Size(139, 17)
        Me.RadAutoBackup.TabIndex = 19
        Me.RadAutoBackup.Text = "Automatic Backup Mode"
        Me.RadAutoBackup.UseVisualStyleBackColor = True
        '
        'frmBackupEveHQ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(637, 565)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbBackup)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBackupEveHQ"
        Me.Text = "EveHQ Settings Backup"
        Me.GroupBox1.ResumeLayout(False)
        Me.gbBackup.ResumeLayout(False)
        Me.gbBackup.PerformLayout()
        CType(Me.nudDays, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBackupWarning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRestore As System.Windows.Forms.Button
    Friend WithEvents lvwBackups As System.Windows.Forms.ListView
    Friend WithEvents BackupData As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnScan As System.Windows.Forms.Button
    Friend WithEvents gbBackup As System.Windows.Forms.GroupBox
    Friend WithEvents btnResetBackup As System.Windows.Forms.Button
    Friend WithEvents btnBackup As System.Windows.Forms.Button
    Friend WithEvents lblNextBackupLbl As System.Windows.Forms.Label
    Friend WithEvents lblLastBackup As System.Windows.Forms.Label
    Friend WithEvents lblNextBackup As System.Windows.Forms.Label
    Friend WithEvents lblLastBackupLbl As System.Windows.Forms.Label
    Friend WithEvents lblStartFormat As System.Windows.Forms.Label
    Friend WithEvents dtpStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblBackupStart As System.Windows.Forms.Label
    Friend WithEvents lblBackupDays As System.Windows.Forms.Label
    Friend WithEvents nudDays As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblBackupFreq As System.Windows.Forms.Label
    Friend WithEvents RadAutoBackup As System.Windows.Forms.RadioButton
    Friend WithEvents lblBackupWarningDays As System.Windows.Forms.Label
    Friend WithEvents nudBackupWarning As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblBackupWarning As System.Windows.Forms.Label
    Friend WithEvents radPromptBackup As System.Windows.Forms.RadioButton
    Friend WithEvents radManualBackup As System.Windows.Forms.RadioButton
End Class
