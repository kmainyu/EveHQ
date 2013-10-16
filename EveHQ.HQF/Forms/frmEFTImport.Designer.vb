Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmEftImport
        Inherits DevComponents.DotNetBar.Office2007Form

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
            Me.lblScan = New System.Windows.Forms.Label
            Me.lblStartDir = New System.Windows.Forms.Label
            Me.txtStartDir = New System.Windows.Forms.TextBox
            Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog
            Me.lvwFiles = New DevComponents.DotNetBar.Controls.ListViewEx
            Me.colFileName = New System.Windows.Forms.ColumnHeader
            Me.colSetups = New System.Windows.Forms.ColumnHeader
            Me.btnScan = New DevComponents.DotNetBar.ButtonX
            Me.btnBrowse = New DevComponents.DotNetBar.ButtonX
            Me.SuspendLayout()
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
            'lvwFiles
            '
            '
            '
            '
            Me.lvwFiles.Border.Class = "ListViewBorder"
            Me.lvwFiles.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
            'btnScan
            '
            Me.btnScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnScan.Location = New System.Drawing.Point(12, 48)
            Me.btnScan.Name = "btnScan"
            Me.btnScan.Size = New System.Drawing.Size(75, 23)
            Me.btnScan.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnScan.TabIndex = 6
            Me.btnScan.Text = "Import"
            '
            'btnBrowse
            '
            Me.btnBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnBrowse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnBrowse.Location = New System.Drawing.Point(564, 10)
            Me.btnBrowse.Name = "btnBrowse"
            Me.btnBrowse.Size = New System.Drawing.Size(75, 21)
            Me.btnBrowse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnBrowse.TabIndex = 7
            Me.btnBrowse.Text = "Browse"
            '
            'frmEFTImport
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(650, 520)
            Me.Controls.Add(Me.btnBrowse)
            Me.Controls.Add(Me.btnScan)
            Me.Controls.Add(Me.lvwFiles)
            Me.Controls.Add(Me.txtStartDir)
            Me.Controls.Add(Me.lblStartDir)
            Me.Controls.Add(Me.lblScan)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
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
        Friend WithEvents lblScan As System.Windows.Forms.Label
        Friend WithEvents lblStartDir As System.Windows.Forms.Label
        Friend WithEvents txtStartDir As System.Windows.Forms.TextBox
        Friend WithEvents fbd1 As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents lvwFiles As DevComponents.DotNetBar.Controls.ListViewEx
        Friend WithEvents colFileName As System.Windows.Forms.ColumnHeader
        Friend WithEvents colSetups As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnScan As DevComponents.DotNetBar.ButtonX
        Friend WithEvents btnBrowse As DevComponents.DotNetBar.ButtonX
    End Class
End NameSpace