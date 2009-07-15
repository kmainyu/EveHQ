<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPatcher
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
        Me.components = New System.ComponentModel.Container
        Me.lblStatus = New System.Windows.Forms.Label
        Me.tmrDownload = New System.Windows.Forms.Timer(Me.components)
        Me.lblCurrentStatus = New System.Windows.Forms.Label
        Me.lblLocalFolders = New System.Windows.Forms.Label
        Me.lblDatabaseLocation = New System.Windows.Forms.Label
        Me.lblEveHQLocation = New System.Windows.Forms.Label
        Me.lblUpdateLocation = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(10, 12)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(187, 13)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Please wait while EveHQ is updated..."
        '
        'tmrDownload
        '
        Me.tmrDownload.Enabled = True
        Me.tmrDownload.Interval = 1000
        '
        'lblCurrentStatus
        '
        Me.lblCurrentStatus.AutoSize = True
        Me.lblCurrentStatus.Location = New System.Drawing.Point(12, 162)
        Me.lblCurrentStatus.Name = "lblCurrentStatus"
        Me.lblCurrentStatus.Size = New System.Drawing.Size(77, 13)
        Me.lblCurrentStatus.TabIndex = 1
        Me.lblCurrentStatus.Text = "Current Status:"
        '
        'lblLocalFolders
        '
        Me.lblLocalFolders.AutoSize = True
        Me.lblLocalFolders.Location = New System.Drawing.Point(10, 102)
        Me.lblLocalFolders.Name = "lblLocalFolders"
        Me.lblLocalFolders.Size = New System.Drawing.Size(106, 13)
        Me.lblLocalFolders.TabIndex = 2
        Me.lblLocalFolders.Text = "Using Local Folders?"
        '
        'lblDatabaseLocation
        '
        Me.lblDatabaseLocation.AutoSize = True
        Me.lblDatabaseLocation.Location = New System.Drawing.Point(10, 132)
        Me.lblDatabaseLocation.Name = "lblDatabaseLocation"
        Me.lblDatabaseLocation.Size = New System.Drawing.Size(100, 13)
        Me.lblDatabaseLocation.TabIndex = 3
        Me.lblDatabaseLocation.Text = "Database Location:"
        '
        'lblEveHQLocation
        '
        Me.lblEveHQLocation.AutoSize = True
        Me.lblEveHQLocation.Location = New System.Drawing.Point(10, 72)
        Me.lblEveHQLocation.Name = "lblEveHQLocation"
        Me.lblEveHQLocation.Size = New System.Drawing.Size(89, 13)
        Me.lblEveHQLocation.TabIndex = 4
        Me.lblEveHQLocation.Text = "EveHQ Location:"
        '
        'lblUpdateLocation
        '
        Me.lblUpdateLocation.AutoSize = True
        Me.lblUpdateLocation.Location = New System.Drawing.Point(10, 42)
        Me.lblUpdateLocation.Name = "lblUpdateLocation"
        Me.lblUpdateLocation.Size = New System.Drawing.Size(89, 13)
        Me.lblUpdateLocation.TabIndex = 5
        Me.lblUpdateLocation.Text = "Update Location:"
        '
        'frmPatcher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(544, 205)
        Me.Controls.Add(Me.lblUpdateLocation)
        Me.Controls.Add(Me.lblEveHQLocation)
        Me.Controls.Add(Me.lblDatabaseLocation)
        Me.Controls.Add(Me.lblLocalFolders)
        Me.Controls.Add(Me.lblCurrentStatus)
        Me.Controls.Add(Me.lblStatus)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmPatcher"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EveHQ Software Update"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents tmrDownload As System.Windows.Forms.Timer
    Friend WithEvents lblCurrentStatus As System.Windows.Forms.Label
    Friend WithEvents lblLocalFolders As System.Windows.Forms.Label
    Friend WithEvents lblDatabaseLocation As System.Windows.Forms.Label
    Friend WithEvents lblEveHQLocation As System.Windows.Forms.Label
    Friend WithEvents lblUpdateLocation As System.Windows.Forms.Label

End Class
