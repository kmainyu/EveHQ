<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMail
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
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.btnDownloadMail = New System.Windows.Forms.Button
        Me.clvMail = New DotNetLib.Windows.Forms.ContainerListView
        Me.btnGetEveIDs = New System.Windows.Forms.Button
        Me.colFrom = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSubject = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDateTime = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.SuspendLayout()
        '
        'cboPilots
        '
        Me.cboPilots.DropDownHeight = 250
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.IntegralHeight = False
        Me.cboPilots.Location = New System.Drawing.Point(48, 12)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(175, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 43
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(12, 15)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(30, 13)
        Me.lblPilot.TabIndex = 42
        Me.lblPilot.Text = "Pilot:"
        '
        'btnDownloadMail
        '
        Me.btnDownloadMail.Location = New System.Drawing.Point(268, 10)
        Me.btnDownloadMail.Name = "btnDownloadMail"
        Me.btnDownloadMail.Size = New System.Drawing.Size(100, 23)
        Me.btnDownloadMail.TabIndex = 44
        Me.btnDownloadMail.Text = "Download Mail"
        Me.btnDownloadMail.UseVisualStyleBackColor = True
        '
        'clvMail
        '
        Me.clvMail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvMail.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colFrom, Me.colSubject, Me.colDateTime})
        Me.clvMail.DefaultItemHeight = 20
        Me.clvMail.Location = New System.Drawing.Point(12, 48)
        Me.clvMail.Name = "clvMail"
        Me.clvMail.Size = New System.Drawing.Size(860, 484)
        Me.clvMail.TabIndex = 45
        '
        'btnGetEveIDs
        '
        Me.btnGetEveIDs.Location = New System.Drawing.Point(374, 10)
        Me.btnGetEveIDs.Name = "btnGetEveIDs"
        Me.btnGetEveIDs.Size = New System.Drawing.Size(100, 23)
        Me.btnGetEveIDs.TabIndex = 46
        Me.btnGetEveIDs.Text = "Get Eve IDs"
        Me.btnGetEveIDs.UseVisualStyleBackColor = True
        '
        'colFrom
        '
        Me.colFrom.CustomSortTag = Nothing
        Me.colFrom.Tag = Nothing
        Me.colFrom.Text = "From"
        Me.colFrom.Width = 200
        '
        'colSubject
        '
        Me.colSubject.CustomSortTag = Nothing
        Me.colSubject.DisplayIndex = 1
        Me.colSubject.Tag = Nothing
        Me.colSubject.Text = "Subject"
        Me.colSubject.Width = 500
        '
        'colDateTime
        '
        Me.colDateTime.CustomSortTag = Nothing
        Me.colDateTime.DisplayIndex = 2
        Me.colDateTime.Tag = Nothing
        Me.colDateTime.Text = "Date"
        Me.colDateTime.Width = 125
        '
        'frmMail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 544)
        Me.Controls.Add(Me.btnGetEveIDs)
        Me.Controls.Add(Me.clvMail)
        Me.Controls.Add(Me.btnDownloadMail)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblPilot)
        Me.Name = "frmMail"
        Me.Text = "EveHQ Mail Viewer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents btnDownloadMail As System.Windows.Forms.Button
    Friend WithEvents clvMail As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents btnGetEveIDs As System.Windows.Forms.Button
    Friend WithEvents colFrom As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSubject As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colDateTime As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
End Class
