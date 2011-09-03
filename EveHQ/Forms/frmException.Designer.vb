<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmException
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
        Me.panelHeader = New System.Windows.Forms.Panel
        Me.lblError = New System.Windows.Forms.Label
        Me.lblVersion = New System.Windows.Forms.Label
        Me.lblEveHQ = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnSend = New System.Windows.Forms.Button
        Me.btnCopyText = New System.Windows.Forms.Button
        Me.txtStackTrace = New System.Windows.Forms.TextBox
        Me.btnContinue = New System.Windows.Forms.Button
        Me.lblWarning = New System.Windows.Forms.Label
        Me.panelHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelHeader
        '
        Me.panelHeader.BackColor = System.Drawing.Color.White
        Me.panelHeader.BackgroundImage = Global.EveHQ.My.Resources.Resources.EveHQErrorProf
        Me.panelHeader.Controls.Add(Me.lblError)
        Me.panelHeader.Controls.Add(Me.lblVersion)
        Me.panelHeader.Controls.Add(Me.lblEveHQ)
        Me.panelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelHeader.Location = New System.Drawing.Point(0, 0)
        Me.panelHeader.Name = "panelHeader"
        Me.panelHeader.Size = New System.Drawing.Size(550, 96)
        Me.panelHeader.TabIndex = 0
        '
        'lblError
        '
        Me.lblError.BackColor = System.Drawing.Color.Black
        Me.lblError.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblError.ForeColor = System.Drawing.Color.White
        Me.lblError.Location = New System.Drawing.Point(101, 54)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(407, 39)
        Me.lblError.TabIndex = 3
        Me.lblError.Text = "Error text..."
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.BackColor = System.Drawing.Color.Black
        Me.lblVersion.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.Color.White
        Me.lblVersion.Location = New System.Drawing.Point(447, 23)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(39, 11)
        Me.lblVersion.TabIndex = 2
        Me.lblVersion.Text = "Version:"
        '
        'lblEveHQ
        '
        Me.lblEveHQ.AutoSize = True
        Me.lblEveHQ.BackColor = System.Drawing.Color.Black
        Me.lblEveHQ.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEveHQ.ForeColor = System.Drawing.Color.White
        Me.lblEveHQ.Location = New System.Drawing.Point(446, 9)
        Me.lblEveHQ.Name = "lblEveHQ"
        Me.lblEveHQ.Size = New System.Drawing.Size(46, 14)
        Me.lblEveHQ.TabIndex = 1
        Me.lblEveHQ.Text = "EveHQ"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 96)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(550, 2)
        Me.Panel1.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(463, 380)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Exit"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(128, 380)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(110, 23)
        Me.btnSend.TabIndex = 3
        Me.btnSend.Text = "Send Error Report"
        Me.btnSend.UseVisualStyleBackColor = True
        Me.btnSend.Visible = False
        '
        'btnCopyText
        '
        Me.btnCopyText.Location = New System.Drawing.Point(12, 380)
        Me.btnCopyText.Name = "btnCopyText"
        Me.btnCopyText.Size = New System.Drawing.Size(110, 23)
        Me.btnCopyText.TabIndex = 5
        Me.btnCopyText.Text = "Copy For Forum"
        Me.btnCopyText.UseVisualStyleBackColor = True
        '
        'txtStackTrace
        '
        Me.txtStackTrace.BackColor = System.Drawing.Color.White
        Me.txtStackTrace.Location = New System.Drawing.Point(12, 104)
        Me.txtStackTrace.Multiline = True
        Me.txtStackTrace.Name = "txtStackTrace"
        Me.txtStackTrace.ReadOnly = True
        Me.txtStackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtStackTrace.Size = New System.Drawing.Size(526, 245)
        Me.txtStackTrace.TabIndex = 6
        '
        'btnContinue
        '
        Me.btnContinue.Location = New System.Drawing.Point(382, 380)
        Me.btnContinue.Name = "btnContinue"
        Me.btnContinue.Size = New System.Drawing.Size(75, 23)
        Me.btnContinue.TabIndex = 7
        Me.btnContinue.Text = "Continue"
        Me.btnContinue.UseVisualStyleBackColor = True
        '
        'lblWarning
        '
        Me.lblWarning.AutoSize = True
        Me.lblWarning.Location = New System.Drawing.Point(12, 356)
        Me.lblWarning.Name = "lblWarning"
        Me.lblWarning.Size = New System.Drawing.Size(372, 13)
        Me.lblWarning.TabIndex = 8
        Me.lblWarning.Text = "Warning: Trying to continue the application may lead to unexpected results."
        '
        'frmException
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(550, 411)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblWarning)
        Me.Controls.Add(Me.btnContinue)
        Me.Controls.Add(Me.txtStackTrace)
        Me.Controls.Add(Me.btnCopyText)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.panelHeader)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmException"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EveHQ Unhandled Exception"
        Me.panelHeader.ResumeLayout(False)
        Me.panelHeader.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents panelHeader As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblEveHQ As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents btnCopyText As System.Windows.Forms.Button
    Friend WithEvents txtStackTrace As System.Windows.Forms.TextBox
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents btnContinue As System.Windows.Forms.Button
    Friend WithEvents lblWarning As System.Windows.Forms.Label
End Class
