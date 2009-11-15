<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEFTImport
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
        Me.btnScan = New System.Windows.Forms.Button
        Me.lblScan = New System.Windows.Forms.Label
        Me.lblStartDir = New System.Windows.Forms.Label
        Me.txtStartDir = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog
        Me.lvwFiles = New System.Windows.Forms.ListView
        Me.colFileName = New System.Windows.Forms.ColumnHeader
        Me.colSetups = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'btnScan
        '
        Me.btnScan.Location = New System.Drawing.Point(12, 39)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(75, 23)
        Me.btnScan.TabIndex = 0
        Me.btnScan.Text = "Import"
        Me.btnScan.UseVisualStyleBackColor = True
        '
        'lblScan
        '
        Me.lblScan.AutoSize = True
        Me.lblScan.Location = New System.Drawing.Point(9, 74)
        Me.lblScan.Name = "lblScan"
        Me.lblScan.Size = New System.Drawing.Size(105, 13)
        Me.lblScan.TabIndex = 1
        Me.lblScan.Text = "Currently Scanning: "
        '
        'lblStartDir
        '
        Me.lblStartDir.AutoSize = True
        Me.lblStartDir.Location = New System.Drawing.Point(13, 13)
        Me.lblStartDir.Name = "lblStartDir"
        Me.lblStartDir.Size = New System.Drawing.Size(82, 13)
        Me.lblStartDir.TabIndex = 2
        Me.lblStartDir.Text = "Start Directory:"
        '
        'txtStartDir
        '
        Me.txtStartDir.Location = New System.Drawing.Point(96, 10)
        Me.txtStartDir.Name = "txtStartDir"
        Me.txtStartDir.Size = New System.Drawing.Size(462, 21)
        Me.txtStartDir.TabIndex = 3
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(564, 8)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 4
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'lvwFiles
        '
        Me.lvwFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFileName, Me.colSetups})
        Me.lvwFiles.FullRowSelect = True
        Me.lvwFiles.GridLines = True
        Me.lvwFiles.Location = New System.Drawing.Point(12, 110)
        Me.lvwFiles.Name = "lvwFiles"
        Me.lvwFiles.Size = New System.Drawing.Size(627, 398)
        Me.lvwFiles.TabIndex = 5
        Me.lvwFiles.UseCompatibleStateImageBehavior = False
        Me.lvwFiles.View = System.Windows.Forms.View.Details
        '
        'colFileName
        '
        Me.colFileName.Text = "Filename"
        Me.colFileName.Width = 550
        '
        'colSetups
        '
        Me.colSetups.Text = "Setups"
        Me.colSetups.Width = 50
        '
        'frmEFTImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(650, 520)
        Me.Controls.Add(Me.lvwFiles)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtStartDir)
        Me.Controls.Add(Me.lblStartDir)
        Me.Controls.Add(Me.lblScan)
        Me.Controls.Add(Me.btnScan)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEFTImport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import EFT Setups"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnScan As System.Windows.Forms.Button
    Friend WithEvents lblScan As System.Windows.Forms.Label
    Friend WithEvents lblStartDir As System.Windows.Forms.Label
    Friend WithEvents txtStartDir As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents fbd1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents lvwFiles As System.Windows.Forms.ListView
    Friend WithEvents colFileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSetups As System.Windows.Forms.ColumnHeader
End Class
