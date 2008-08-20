<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblEveHQLink = New System.Windows.Forms.LinkLabel
        Me.lblInfo1 = New System.Windows.Forms.Label
        Me.lblVersion = New System.Windows.Forms.Label
        Me.lblInfo2 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = Global.EveHQ.My.Resources.Resources.Splashv5
        Me.Panel1.Controls.Add(Me.lblInfo2)
        Me.Panel1.Controls.Add(Me.lblEveHQLink)
        Me.Panel1.Controls.Add(Me.lblInfo1)
        Me.Panel1.Controls.Add(Me.lblVersion)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(600, 400)
        Me.Panel1.TabIndex = 0
        '
        'lblEveHQLink
        '
        Me.lblEveHQLink.ActiveLinkColor = System.Drawing.Color.DarkTurquoise
        Me.lblEveHQLink.AutoSize = True
        Me.lblEveHQLink.BackColor = System.Drawing.Color.Transparent
        Me.lblEveHQLink.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEveHQLink.ForeColor = System.Drawing.Color.PaleTurquoise
        Me.lblEveHQLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblEveHQLink.LinkColor = System.Drawing.Color.PaleTurquoise
        Me.lblEveHQLink.Location = New System.Drawing.Point(261, 9)
        Me.lblEveHQLink.Name = "lblEveHQLink"
        Me.lblEveHQLink.Size = New System.Drawing.Size(336, 13)
        Me.lblEveHQLink.TabIndex = 4
        Me.lblEveHQLink.TabStop = True
        Me.lblEveHQLink.Text = "Visit the forums at www.evehq.net for bug reporting and comments."
        Me.lblEveHQLink.VisitedLinkColor = System.Drawing.Color.PaleTurquoise
        '
        'lblInfo1
        '
        Me.lblInfo1.AutoSize = True
        Me.lblInfo1.BackColor = System.Drawing.Color.Transparent
        Me.lblInfo1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo1.ForeColor = System.Drawing.Color.PaleTurquoise
        Me.lblInfo1.Location = New System.Drawing.Point(12, 361)
        Me.lblInfo1.Name = "lblInfo1"
        Me.lblInfo1.Size = New System.Drawing.Size(180, 16)
        Me.lblInfo1.TabIndex = 2
        Me.lblInfo1.Text = "Designed && Coded by Vessper" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.Color.PaleTurquoise
        Me.lblVersion.Location = New System.Drawing.Point(140, 197)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(47, 14)
        Me.lblVersion.TabIndex = 1
        Me.lblVersion.Text = "Version"
        '
        'lblInfo2
        '
        Me.lblInfo2.AutoSize = True
        Me.lblInfo2.BackColor = System.Drawing.Color.Transparent
        Me.lblInfo2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo2.ForeColor = System.Drawing.Color.PaleTurquoise
        Me.lblInfo2.Location = New System.Drawing.Point(29, 377)
        Me.lblInfo2.Name = "lblInfo2"
        Me.lblInfo2.Size = New System.Drawing.Size(299, 16)
        Me.lblInfo2.TabIndex = 5
        Me.lblInfo2.Text = "Additional Coding: Darkwolf, Darmed Khan, Mdram"
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(599, 399)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAbout"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About EveHQ"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lblInfo1 As System.Windows.Forms.Label
    Friend WithEvents lblEveHQLink As System.Windows.Forms.LinkLabel
    Friend WithEvents lblInfo2 As System.Windows.Forms.Label
End Class
