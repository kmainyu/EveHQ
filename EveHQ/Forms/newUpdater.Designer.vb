<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewUpdater
    Inherits DevComponents.DotNetBar.OfficeForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(newUpdater))
        Me._downloadProgress = New DevComponents.DotNetBar.Controls.ProgressBarX()
        Me._statusDetail = New System.Windows.Forms.Label()
        Me._continueButton = New System.Windows.Forms.Button()
        Me._statusHeader = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        '_downloadProgress
        '
        '
        '
        '
        Me._downloadProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me._downloadProgress.Location = New System.Drawing.Point(95, 27)
        Me._downloadProgress.Name = "_downloadProgress"
        Me._downloadProgress.Size = New System.Drawing.Size(232, 26)
        Me._downloadProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me._downloadProgress.TabIndex = 0
        Me._downloadProgress.Text = "0%"
        Me._downloadProgress.TextVisible = True
        '
        '_statusDetail
        '
        Me._statusDetail.AutoSize = True
        Me._statusDetail.Location = New System.Drawing.Point(21, 85)
        Me._statusDetail.Name = "_statusDetail"
        Me._statusDetail.Size = New System.Drawing.Size(380, 13)
        Me._statusDetail.TabIndex = 1
        Me._statusDetail.Text = "https://bitbucket.org/EveHQ/evehq/downloads/evehq-2.12.3.2256-setup.exe"
        '
        '_continueButton
        '
        Me._continueButton.Location = New System.Drawing.Point(142, 111)
        Me._continueButton.Name = "_continueButton"
        Me._continueButton.Size = New System.Drawing.Size(138, 27)
        Me._continueButton.TabIndex = 2
        Me._continueButton.Text = "Continue"
        Me._continueButton.UseVisualStyleBackColor = True
        Me._continueButton.Visible = False
        '
        '_statusHeader
        '
        Me._statusHeader.AutoSize = True
        Me._statusHeader.Location = New System.Drawing.Point(177, 63)
        Me._statusHeader.Name = "_statusHeader"
        Me._statusHeader.Size = New System.Drawing.Size(69, 13)
        Me._statusHeader.TabIndex = 3
        Me._statusHeader.Text = "Downloading"
        '
        'newUpdater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(423, 148)
        Me.Controls.Add(Me._statusHeader)
        Me.Controls.Add(Me._continueButton)
        Me.Controls.Add(Me._statusDetail)
        Me.Controls.Add(Me._downloadProgress)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "newUpdater"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EveHQ Updater"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _downloadProgress As DevComponents.DotNetBar.Controls.ProgressBarX
    Friend WithEvents _statusDetail As System.Windows.Forms.Label
    Friend WithEvents _continueButton As System.Windows.Forms.Button
    Friend WithEvents _statusHeader As System.Windows.Forms.Label
End Class
