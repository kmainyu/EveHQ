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
        Me.lblCopyright = New System.Windows.Forms.Label
        Me.lblDate = New System.Windows.Forms.Label
        Me.lblVersion = New System.Windows.Forms.Label
        Me.lblEveHQLink = New System.Windows.Forms.LinkLabel
        Me.wbCredits = New System.Windows.Forms.WebBrowser
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Black
        Me.Panel1.BackgroundImage = Global.EveHQ.My.Resources.Resources.EveHQSplash1_14
        Me.Panel1.Controls.Add(Me.lblCopyright)
        Me.Panel1.Controls.Add(Me.lblDate)
        Me.Panel1.Controls.Add(Me.lblVersion)
        Me.Panel1.Controls.Add(Me.lblEveHQLink)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(600, 400)
        Me.Panel1.TabIndex = 0
        '
        'lblCopyright
        '
        Me.lblCopyright.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCopyright.BackColor = System.Drawing.Color.Transparent
        Me.lblCopyright.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0, Byte))
        Me.lblCopyright.ForeColor = System.Drawing.Color.Turquoise
        Me.lblCopyright.Location = New System.Drawing.Point(398, 385)
        Me.lblCopyright.Name = "lblCopyright"
        Me.lblCopyright.Size = New System.Drawing.Size(200, 13)
        Me.lblCopyright.TabIndex = 7
        Me.lblCopyright.Text = "Copyright"
        Me.lblCopyright.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDate
        '
        Me.lblDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDate.BackColor = System.Drawing.Color.Transparent
        Me.lblDate.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.Color.White
        Me.lblDate.Location = New System.Drawing.Point(398, 372)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(200, 13)
        Me.lblDate.TabIndex = 6
        Me.lblDate.Text = "Date"
        Me.lblDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblVersion
        '
        Me.lblVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.Color.White
        Me.lblVersion.Location = New System.Drawing.Point(398, 359)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(200, 13)
        Me.lblVersion.TabIndex = 5
        Me.lblVersion.Text = "Version"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.lblEveHQLink.Location = New System.Drawing.Point(12, 365)
        Me.lblEveHQLink.Name = "lblEveHQLink"
        Me.lblEveHQLink.Size = New System.Drawing.Size(336, 13)
        Me.lblEveHQLink.TabIndex = 4
        Me.lblEveHQLink.TabStop = True
        Me.lblEveHQLink.Text = "Visit the forums at www.evehq.net for bug reporting and comments."
        Me.lblEveHQLink.VisitedLinkColor = System.Drawing.Color.PaleTurquoise
        '
        'wbCredits
        '
        Me.wbCredits.AllowNavigation = False
        Me.wbCredits.AllowWebBrowserDrop = False
        Me.wbCredits.Dock = System.Windows.Forms.DockStyle.Right
        Me.wbCredits.IsWebBrowserContextMenuEnabled = False
        Me.wbCredits.Location = New System.Drawing.Point(603, 0)
        Me.wbCredits.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbCredits.Name = "wbCredits"
        Me.wbCredits.ScriptErrorsSuppressed = True
        Me.wbCredits.ScrollBarsEnabled = False
        Me.wbCredits.Size = New System.Drawing.Size(196, 399)
        Me.wbCredits.TabIndex = 1
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 399)
        Me.Controls.Add(Me.wbCredits)
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
    Friend WithEvents lblEveHQLink As System.Windows.Forms.LinkLabel
    Friend WithEvents wbCredits As System.Windows.Forms.WebBrowser
    Friend WithEvents lblCopyright As System.Windows.Forms.Label
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
End Class
