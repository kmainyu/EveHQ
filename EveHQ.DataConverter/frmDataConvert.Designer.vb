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
        Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher()
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog()
        Me.tabSQLCEVersion = New System.Windows.Forms.TabPage()
        Me.pnlDBVersion = New System.Windows.Forms.Panel()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btnBrowseDB = New System.Windows.Forms.Button()
        Me.lblSourceDB = New System.Windows.Forms.Label()
        Me.tabWH = New System.Windows.Forms.TabPage()
        Me.panelWormholes = New System.Windows.Forms.Panel()
        Me.gbClassLocations = New System.Windows.Forms.GroupBox()
        Me.btnGenerateWHClassLocations = New System.Windows.Forms.Button()
        Me.btnGenerateWHAttribs = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabSQLCEVersion.SuspendLayout()
        Me.pnlDBVersion.SuspendLayout()
        Me.tabWH.SuspendLayout()
        Me.panelWormholes.SuspendLayout()
        Me.gbClassLocations.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'tabSQLCEVersion
        '
        Me.tabSQLCEVersion.Controls.Add(Me.pnlDBVersion)
        Me.tabSQLCEVersion.Location = New System.Drawing.Point(4, 22)
        Me.tabSQLCEVersion.Name = "tabSQLCEVersion"
        Me.tabSQLCEVersion.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSQLCEVersion.Size = New System.Drawing.Size(645, 235)
        Me.tabSQLCEVersion.TabIndex = 7
        Me.tabSQLCEVersion.Text = "SQLCE Version"
        Me.tabSQLCEVersion.UseVisualStyleBackColor = True
        '
        'pnlDBVersion
        '
        Me.pnlDBVersion.BackColor = System.Drawing.SystemColors.Control
        Me.pnlDBVersion.Controls.Add(Me.lblSourceDB)
        Me.pnlDBVersion.Controls.Add(Me.btnBrowseDB)
        Me.pnlDBVersion.Controls.Add(Me.TextBox1)
        Me.pnlDBVersion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDBVersion.Location = New System.Drawing.Point(3, 3)
        Me.pnlDBVersion.Name = "pnlDBVersion"
        Me.pnlDBVersion.Size = New System.Drawing.Size(639, 229)
        Me.pnlDBVersion.TabIndex = 0
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(78, 20)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(464, 21)
        Me.TextBox1.TabIndex = 5
        '
        'btnBrowseDB
        '
        Me.btnBrowseDB.Location = New System.Drawing.Point(548, 18)
        Me.btnBrowseDB.Name = "btnBrowseDB"
        Me.btnBrowseDB.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseDB.TabIndex = 6
        Me.btnBrowseDB.Text = "Browse DB"
        Me.btnBrowseDB.UseVisualStyleBackColor = True
        '
        'lblSourceDB
        '
        Me.lblSourceDB.AutoSize = True
        Me.lblSourceDB.Location = New System.Drawing.Point(12, 23)
        Me.lblSourceDB.Name = "lblSourceDB"
        Me.lblSourceDB.Size = New System.Drawing.Size(60, 13)
        Me.lblSourceDB.TabIndex = 7
        Me.lblSourceDB.Text = "Source DB:"
        '
        'tabWH
        '
        Me.tabWH.Controls.Add(Me.panelWormholes)
        Me.tabWH.Location = New System.Drawing.Point(4, 22)
        Me.tabWH.Name = "tabWH"
        Me.tabWH.Size = New System.Drawing.Size(645, 235)
        Me.tabWH.TabIndex = 6
        Me.tabWH.Text = "Wormholes"
        Me.tabWH.UseVisualStyleBackColor = True
        '
        'panelWormholes
        '
        Me.panelWormholes.BackColor = System.Drawing.SystemColors.Control
        Me.panelWormholes.Controls.Add(Me.gbClassLocations)
        Me.panelWormholes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelWormholes.Location = New System.Drawing.Point(0, 0)
        Me.panelWormholes.Name = "panelWormholes"
        Me.panelWormholes.Size = New System.Drawing.Size(645, 235)
        Me.panelWormholes.TabIndex = 0
        '
        'gbClassLocations
        '
        Me.gbClassLocations.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbClassLocations.Controls.Add(Me.btnGenerateWHAttribs)
        Me.gbClassLocations.Controls.Add(Me.btnGenerateWHClassLocations)
        Me.gbClassLocations.Location = New System.Drawing.Point(8, 12)
        Me.gbClassLocations.Name = "gbClassLocations"
        Me.gbClassLocations.Size = New System.Drawing.Size(629, 215)
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
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabSQLCEVersion)
        Me.TabControl1.Controls.Add(Me.tabWH)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(653, 261)
        Me.TabControl1.TabIndex = 57
        '
        'frmDataConvert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(653, 261)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDataConvert"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EveHQ Data Converter"
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSQLCEVersion.ResumeLayout(False)
        Me.pnlDBVersion.ResumeLayout(False)
        Me.pnlDBVersion.PerformLayout()
        Me.tabWH.ResumeLayout(False)
        Me.panelWormholes.ResumeLayout(False)
        Me.gbClassLocations.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fbd1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FileSystemWatcher1 As System.IO.FileSystemWatcher
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabSQLCEVersion As System.Windows.Forms.TabPage
    Friend WithEvents pnlDBVersion As System.Windows.Forms.Panel
    Friend WithEvents lblSourceDB As System.Windows.Forms.Label
    Friend WithEvents btnBrowseDB As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents tabWH As System.Windows.Forms.TabPage
    Friend WithEvents panelWormholes As System.Windows.Forms.Panel
    Friend WithEvents gbClassLocations As System.Windows.Forms.GroupBox
    Friend WithEvents btnGenerateWHAttribs As System.Windows.Forms.Button
    Friend WithEvents btnGenerateWHClassLocations As System.Windows.Forms.Button
End Class
