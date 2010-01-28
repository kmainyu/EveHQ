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
        Me.colFrom = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTo = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSubject = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDateTime = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnGetEveIDs = New System.Windows.Forms.Button
        Me.clvNotifications = New DotNetLib.Windows.Forms.ContainerListView
        Me.ContainerListViewColumnHeader1 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader4 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader2 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader3 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lblEveMail = New System.Windows.Forms.Label
        Me.panelNotifications = New System.Windows.Forms.Panel
        Me.lblEveNotifications = New System.Windows.Forms.Label
        Me.CollapsibleSplitter1 = New NJFLib.Controls.CollapsibleSplitter
        Me.panelMails = New System.Windows.Forms.Panel
        Me.lblDownloadMailStatus = New System.Windows.Forms.Label
        Me.panelNotifications.SuspendLayout()
        Me.panelMails.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboPilots
        '
        Me.cboPilots.DropDownHeight = 250
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.IntegralHeight = False
        Me.cboPilots.Location = New System.Drawing.Point(44, 9)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(175, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 43
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(8, 12)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 42
        Me.lblPilot.Text = "Pilot:"
        '
        'btnDownloadMail
        '
        Me.btnDownloadMail.Location = New System.Drawing.Point(225, 7)
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
        Me.clvMail.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colFrom, Me.colTo, Me.colSubject, Me.colDateTime})
        Me.clvMail.DefaultItemHeight = 20
        Me.clvMail.Location = New System.Drawing.Point(0, 56)
        Me.clvMail.Name = "clvMail"
        Me.clvMail.Size = New System.Drawing.Size(881, 263)
        Me.clvMail.TabIndex = 45
        '
        'colFrom
        '
        Me.colFrom.CustomSortTag = Nothing
        Me.colFrom.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colFrom.Tag = Nothing
        Me.colFrom.Text = "From"
        Me.colFrom.Width = 160
        '
        'colTo
        '
        Me.colTo.CustomSortTag = Nothing
        Me.colTo.DisplayIndex = 1
        Me.colTo.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colTo.Tag = Nothing
        Me.colTo.Text = "To"
        Me.colTo.Width = 160
        '
        'colSubject
        '
        Me.colSubject.CustomSortTag = Nothing
        Me.colSubject.DisplayIndex = 2
        Me.colSubject.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colSubject.Tag = Nothing
        Me.colSubject.Text = "Subject"
        Me.colSubject.Width = 400
        '
        'colDateTime
        '
        Me.colDateTime.CustomSortTag = Nothing
        Me.colDateTime.DisplayIndex = 3
        Me.colDateTime.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Date]
        Me.colDateTime.Tag = Nothing
        Me.colDateTime.Text = "Date"
        Me.colDateTime.Width = 200
        '
        'btnGetEveIDs
        '
        Me.btnGetEveIDs.Location = New System.Drawing.Point(331, 7)
        Me.btnGetEveIDs.Name = "btnGetEveIDs"
        Me.btnGetEveIDs.Size = New System.Drawing.Size(100, 23)
        Me.btnGetEveIDs.TabIndex = 46
        Me.btnGetEveIDs.Text = "Get Eve IDs"
        Me.btnGetEveIDs.UseVisualStyleBackColor = True
        Me.btnGetEveIDs.Visible = False
        '
        'clvNotifications
        '
        Me.clvNotifications.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvNotifications.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.ContainerListViewColumnHeader1, Me.ContainerListViewColumnHeader4, Me.ContainerListViewColumnHeader2, Me.ContainerListViewColumnHeader3})
        Me.clvNotifications.DefaultItemHeight = 20
        Me.clvNotifications.Location = New System.Drawing.Point(0, 25)
        Me.clvNotifications.Name = "clvNotifications"
        Me.clvNotifications.Size = New System.Drawing.Size(884, 186)
        Me.clvNotifications.TabIndex = 47
        '
        'ContainerListViewColumnHeader1
        '
        Me.ContainerListViewColumnHeader1.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader1.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.ContainerListViewColumnHeader1.Tag = Nothing
        Me.ContainerListViewColumnHeader1.Text = "From"
        Me.ContainerListViewColumnHeader1.Width = 160
        '
        'ContainerListViewColumnHeader4
        '
        Me.ContainerListViewColumnHeader4.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader4.DisplayIndex = 1
        Me.ContainerListViewColumnHeader4.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.ContainerListViewColumnHeader4.Tag = Nothing
        Me.ContainerListViewColumnHeader4.Text = "To"
        Me.ContainerListViewColumnHeader4.Width = 160
        '
        'ContainerListViewColumnHeader2
        '
        Me.ContainerListViewColumnHeader2.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader2.DisplayIndex = 2
        Me.ContainerListViewColumnHeader2.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.ContainerListViewColumnHeader2.Tag = Nothing
        Me.ContainerListViewColumnHeader2.Text = "Subject"
        Me.ContainerListViewColumnHeader2.Width = 400
        '
        'ContainerListViewColumnHeader3
        '
        Me.ContainerListViewColumnHeader3.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader3.DisplayIndex = 3
        Me.ContainerListViewColumnHeader3.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Date]
        Me.ContainerListViewColumnHeader3.Tag = Nothing
        Me.ContainerListViewColumnHeader3.Text = "Date"
        Me.ContainerListViewColumnHeader3.Width = 200
        '
        'lblEveMail
        '
        Me.lblEveMail.AutoSize = True
        Me.lblEveMail.Location = New System.Drawing.Point(3, 40)
        Me.lblEveMail.Name = "lblEveMail"
        Me.lblEveMail.Size = New System.Drawing.Size(55, 13)
        Me.lblEveMail.TabIndex = 49
        Me.lblEveMail.Text = "Eve Mails:"
        '
        'panelNotifications
        '
        Me.panelNotifications.Controls.Add(Me.lblEveNotifications)
        Me.panelNotifications.Controls.Add(Me.clvNotifications)
        Me.panelNotifications.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelNotifications.Location = New System.Drawing.Point(0, 333)
        Me.panelNotifications.Name = "panelNotifications"
        Me.panelNotifications.Size = New System.Drawing.Size(884, 211)
        Me.panelNotifications.TabIndex = 50
        '
        'lblEveNotifications
        '
        Me.lblEveNotifications.AutoSize = True
        Me.lblEveNotifications.Location = New System.Drawing.Point(3, 9)
        Me.lblEveNotifications.Name = "lblEveNotifications"
        Me.lblEveNotifications.Size = New System.Drawing.Size(91, 13)
        Me.lblEveNotifications.TabIndex = 50
        Me.lblEveNotifications.Text = "Eve Notifications:"
        '
        'CollapsibleSplitter1
        '
        Me.CollapsibleSplitter1.AnimationDelay = 20
        Me.CollapsibleSplitter1.AnimationStep = 20
        Me.CollapsibleSplitter1.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.CollapsibleSplitter1.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat
        Me.CollapsibleSplitter1.ControlToHide = Me.panelNotifications
        Me.CollapsibleSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.CollapsibleSplitter1.ExpandParentForm = False
        Me.CollapsibleSplitter1.Location = New System.Drawing.Point(0, 325)
        Me.CollapsibleSplitter1.Name = "CollapsibleSplitter1"
        Me.CollapsibleSplitter1.TabIndex = 51
        Me.CollapsibleSplitter1.TabStop = False
        Me.CollapsibleSplitter1.UseAnimations = False
        Me.CollapsibleSplitter1.VisualStyle = NJFLib.Controls.VisualStyles.XP
        '
        'panelMails
        '
        Me.panelMails.Controls.Add(Me.lblDownloadMailStatus)
        Me.panelMails.Controls.Add(Me.btnGetEveIDs)
        Me.panelMails.Controls.Add(Me.btnDownloadMail)
        Me.panelMails.Controls.Add(Me.lblPilot)
        Me.panelMails.Controls.Add(Me.lblEveMail)
        Me.panelMails.Controls.Add(Me.clvMail)
        Me.panelMails.Controls.Add(Me.cboPilots)
        Me.panelMails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelMails.Location = New System.Drawing.Point(0, 0)
        Me.panelMails.Name = "panelMails"
        Me.panelMails.Size = New System.Drawing.Size(884, 325)
        Me.panelMails.TabIndex = 52
        '
        'lblDownloadMailStatus
        '
        Me.lblDownloadMailStatus.AutoSize = True
        Me.lblDownloadMailStatus.Location = New System.Drawing.Point(65, 40)
        Me.lblDownloadMailStatus.Name = "lblDownloadMailStatus"
        Me.lblDownloadMailStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblDownloadMailStatus.TabIndex = 50
        '
        'frmMail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 544)
        Me.Controls.Add(Me.panelMails)
        Me.Controls.Add(Me.CollapsibleSplitter1)
        Me.Controls.Add(Me.panelNotifications)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmMail"
        Me.Text = "EveHQ Mail Viewer"
        Me.panelNotifications.ResumeLayout(False)
        Me.panelNotifications.PerformLayout()
        Me.panelMails.ResumeLayout(False)
        Me.panelMails.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents btnDownloadMail As System.Windows.Forms.Button
    Friend WithEvents clvMail As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents btnGetEveIDs As System.Windows.Forms.Button
    Friend WithEvents colFrom As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSubject As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colDateTime As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents clvNotifications As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents ContainerListViewColumnHeader1 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader2 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader3 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lblEveMail As System.Windows.Forms.Label
    Friend WithEvents panelNotifications As System.Windows.Forms.Panel
    Friend WithEvents lblEveNotifications As System.Windows.Forms.Label
    Friend WithEvents CollapsibleSplitter1 As NJFLib.Controls.CollapsibleSplitter
    Friend WithEvents panelMails As System.Windows.Forms.Panel
    Friend WithEvents colTo As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader4 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lblDownloadMailStatus As System.Windows.Forms.Label
End Class
