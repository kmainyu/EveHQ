<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class newUpdater
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
        Me._statusLabel = New System.Windows.Forms.Label()
        Me._continueButton = New System.Windows.Forms.Button()
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
        '_statusLabel
        '
        Me._statusLabel.AutoSize = True
        Me._statusLabel.Location = New System.Drawing.Point(37, 77)
        Me._statusLabel.Name = "_statusLabel"
        Me._statusLabel.Size = New System.Drawing.Size(348, 13)
        Me._statusLabel.TabIndex = 1
        Me._statusLabel.Text = "Requesting http://www.evehq.net/updates/evehq-setup-2.12.2170.exe"
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
        'newUpdater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(423, 148)
        Me.Controls.Add(Me._continueButton)
        Me.Controls.Add(Me._statusLabel)
        Me.Controls.Add(Me._downloadProgress)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "newUpdater"
        Me.ShowInTaskbar = False
        Me.Text = "EveHQ Updater"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _downloadProgress As DevComponents.DotNetBar.Controls.ProgressBarX
    Friend WithEvents _statusLabel As System.Windows.Forms.Label
    Friend WithEvents _continueButton As System.Windows.Forms.Button
End Class
