<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBackup
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBackup))
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
        Me.chkAuto = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnRestore = New System.Windows.Forms.Button
        Me.lvwBackups = New System.Windows.Forms.ListView
        Me.BackupDate = New System.Windows.Forms.ColumnHeader
        Me.BackupLoc = New System.Windows.Forms.ColumnHeader
        Me.Path = New System.Windows.Forms.ColumnHeader
        Me.btnScan = New System.Windows.Forms.Button
        Me.gbBackup.SuspendLayout()
        CType(Me.nudDays, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbBackup
        '
        Me.gbBackup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.gbBackup.Controls.Add(Me.chkAuto)
        Me.gbBackup.Location = New System.Drawing.Point(6, 7)
        Me.gbBackup.Name = "gbBackup"
        Me.gbBackup.Size = New System.Drawing.Size(622, 196)
        Me.gbBackup.TabIndex = 0
        Me.gbBackup.TabStop = False
        Me.gbBackup.Text = "Eve Settings Backup"
        '
        'btnResetBackup
        '
        Me.btnResetBackup.Location = New System.Drawing.Point(122, 165)
        Me.btnResetBackup.Name = "btnResetBackup"
        Me.btnResetBackup.Size = New System.Drawing.Size(109, 23)
        Me.btnResetBackup.TabIndex = 13
        Me.btnResetBackup.Text = "Reset Last Backup"
        Me.btnResetBackup.UseVisualStyleBackColor = True
        '
        'btnBackup
        '
        Me.btnBackup.Location = New System.Drawing.Point(24, 165)
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
        Me.lblNextBackupLbl.Location = New System.Drawing.Point(75, 134)
        Me.lblNextBackupLbl.Name = "lblNextBackupLbl"
        Me.lblNextBackupLbl.Size = New System.Drawing.Size(72, 13)
        Me.lblNextBackupLbl.TabIndex = 11
        Me.lblNextBackupLbl.Text = "Next Backup:"
        '
        'lblLastBackup
        '
        Me.lblLastBackup.AutoSize = True
        Me.lblLastBackup.Enabled = False
        Me.lblLastBackup.Location = New System.Drawing.Point(178, 109)
        Me.lblLastBackup.Name = "lblLastBackup"
        Me.lblLastBackup.Size = New System.Drawing.Size(63, 13)
        Me.lblLastBackup.TabIndex = 10
        Me.lblLastBackup.Text = "<unknown>"
        '
        'lblNextBackup
        '
        Me.lblNextBackup.AutoSize = True
        Me.lblNextBackup.Enabled = False
        Me.lblNextBackup.Location = New System.Drawing.Point(178, 134)
        Me.lblNextBackup.Name = "lblNextBackup"
        Me.lblNextBackup.Size = New System.Drawing.Size(63, 13)
        Me.lblNextBackup.TabIndex = 9
        Me.lblNextBackup.Text = "<unknown>"
        '
        'lblLastBackupLbl
        '
        Me.lblLastBackupLbl.AutoSize = True
        Me.lblLastBackupLbl.Enabled = False
        Me.lblLastBackupLbl.Location = New System.Drawing.Point(75, 109)
        Me.lblLastBackupLbl.Name = "lblLastBackupLbl"
        Me.lblLastBackupLbl.Size = New System.Drawing.Size(70, 13)
        Me.lblLastBackupLbl.TabIndex = 8
        Me.lblLastBackupLbl.Text = "Last Backup:"
        '
        'lblStartFormat
        '
        Me.lblStartFormat.AutoSize = True
        Me.lblStartFormat.Enabled = False
        Me.lblStartFormat.Location = New System.Drawing.Point(316, 82)
        Me.lblStartFormat.Name = "lblStartFormat"
        Me.lblStartFormat.Size = New System.Drawing.Size(158, 13)
        Me.lblStartFormat.TabIndex = 7
        Me.lblStartFormat.Text = "(in ""dd/mm/yyyy hh:mm"" format)"
        '
        'dtpStart
        '
        Me.dtpStart.CustomFormat = "dd/MM/yyyy HH:mm"
        Me.dtpStart.Enabled = False
        Me.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStart.Location = New System.Drawing.Point(181, 78)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.ShowUpDown = True
        Me.dtpStart.Size = New System.Drawing.Size(129, 20)
        Me.dtpStart.TabIndex = 6
        Me.dtpStart.Tag = "1"
        Me.dtpStart.Value = New Date(2006, 3, 10, 0, 0, 0, 0)
        '
        'lblBackupStart
        '
        Me.lblBackupStart.AutoSize = True
        Me.lblBackupStart.Enabled = False
        Me.lblBackupStart.Location = New System.Drawing.Point(75, 84)
        Me.lblBackupStart.Name = "lblBackupStart"
        Me.lblBackupStart.Size = New System.Drawing.Size(86, 13)
        Me.lblBackupStart.TabIndex = 4
        Me.lblBackupStart.Text = "Start Date/Time:"
        '
        'lblBackupDays
        '
        Me.lblBackupDays.AutoSize = True
        Me.lblBackupDays.Enabled = False
        Me.lblBackupDays.Location = New System.Drawing.Point(223, 54)
        Me.lblBackupDays.Name = "lblBackupDays"
        Me.lblBackupDays.Size = New System.Drawing.Size(37, 13)
        Me.lblBackupDays.TabIndex = 3
        Me.lblBackupDays.Text = "(Days)"
        '
        'nudDays
        '
        Me.nudDays.Enabled = False
        Me.nudDays.Location = New System.Drawing.Point(181, 52)
        Me.nudDays.Maximum = New Decimal(New Integer() {28, 0, 0, 0})
        Me.nudDays.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDays.Name = "nudDays"
        Me.nudDays.Size = New System.Drawing.Size(36, 20)
        Me.nudDays.TabIndex = 2
        Me.nudDays.Tag = "1"
        Me.nudDays.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblBackupFreq
        '
        Me.lblBackupFreq.AutoSize = True
        Me.lblBackupFreq.Enabled = False
        Me.lblBackupFreq.Location = New System.Drawing.Point(75, 54)
        Me.lblBackupFreq.Name = "lblBackupFreq"
        Me.lblBackupFreq.Size = New System.Drawing.Size(100, 13)
        Me.lblBackupFreq.TabIndex = 1
        Me.lblBackupFreq.Text = "Backup Frequency:"
        '
        'chkAuto
        '
        Me.chkAuto.AutoSize = True
        Me.chkAuto.Location = New System.Drawing.Point(24, 29)
        Me.chkAuto.Name = "chkAuto"
        Me.chkAuto.Size = New System.Drawing.Size(171, 17)
        Me.chkAuto.TabIndex = 0
        Me.chkAuto.Text = "Activate Auto Settings Backup"
        Me.chkAuto.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnRestore)
        Me.GroupBox1.Controls.Add(Me.lvwBackups)
        Me.GroupBox1.Controls.Add(Me.btnScan)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 209)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(622, 313)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Eve Settings Restore"
        '
        'btnRestore
        '
        Me.btnRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRestore.Location = New System.Drawing.Point(24, 279)
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
        Me.lvwBackups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.BackupDate, Me.BackupLoc, Me.Path})
        Me.lvwBackups.FullRowSelect = True
        Me.lvwBackups.GridLines = True
        Me.lvwBackups.Location = New System.Drawing.Point(5, 73)
        Me.lvwBackups.Name = "lvwBackups"
        Me.lvwBackups.Size = New System.Drawing.Size(611, 200)
        Me.lvwBackups.TabIndex = 13
        Me.lvwBackups.UseCompatibleStateImageBehavior = False
        Me.lvwBackups.View = System.Windows.Forms.View.Details
        '
        'BackupDate
        '
        Me.BackupDate.Text = "Backup Date"
        Me.BackupDate.Width = 120
        '
        'BackupLoc
        '
        Me.BackupLoc.Text = "Location"
        Me.BackupLoc.Width = 80
        '
        'Path
        '
        Me.Path.Text = "Backed Up From"
        Me.Path.Width = 380
        '
        'btnScan
        '
        Me.btnScan.Location = New System.Drawing.Point(22, 32)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(137, 23)
        Me.btnScan.TabIndex = 12
        Me.btnScan.Text = "Scan Backup Directory"
        Me.btnScan.UseVisualStyleBackColor = True
        '
        'frmBackup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(638, 534)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbBackup)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(500, 450)
        Me.Name = "frmBackup"
        Me.Text = "Eve Settings Backup"
        Me.gbBackup.ResumeLayout(False)
        Me.gbBackup.PerformLayout()
        CType(Me.nudDays, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbBackup As System.Windows.Forms.GroupBox
    Friend WithEvents lblBackupFreq As System.Windows.Forms.Label
    Friend WithEvents chkAuto As System.Windows.Forms.CheckBox
    Friend WithEvents lblBackupStart As System.Windows.Forms.Label
    Friend WithEvents lblBackupDays As System.Windows.Forms.Label
    Friend WithEvents nudDays As System.Windows.Forms.NumericUpDown
    Friend WithEvents dtpStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblStartFormat As System.Windows.Forms.Label
    Friend WithEvents lblNextBackupLbl As System.Windows.Forms.Label
    Friend WithEvents lblLastBackup As System.Windows.Forms.Label
    Friend WithEvents lblNextBackup As System.Windows.Forms.Label
    Friend WithEvents lblLastBackupLbl As System.Windows.Forms.Label
    Friend WithEvents btnBackup As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lvwBackups As System.Windows.Forms.ListView
    Friend WithEvents btnScan As System.Windows.Forms.Button
    Friend WithEvents BackupDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents BackupLoc As System.Windows.Forms.ColumnHeader
    Friend WithEvents Path As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRestore As System.Windows.Forms.Button
    Friend WithEvents btnResetBackup As System.Windows.Forms.Button
End Class
