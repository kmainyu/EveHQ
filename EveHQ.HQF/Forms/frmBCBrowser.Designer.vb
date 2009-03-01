<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBCBrowser
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
        Me.components = New System.ComponentModel.Container
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("High Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mid Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Low Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Rig Slots", System.Windows.Forms.HorizontalAlignment.Left)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBCBrowser))
        Me.lblShipType = New System.Windows.Forms.Label
        Me.pbShip = New System.Windows.Forms.PictureBox
        Me.clvLoadouts = New DotNetLib.Windows.Forms.ContainerListView
        Me.colName = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colAuthor = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRating = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDate = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ctxLoadout = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuViewLoadout = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.lblBCStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwSlots = New EveHQ.HQF.ListViewNoFlicker
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxLoadout.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblShipType
        '
        Me.lblShipType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblShipType.Font = New System.Drawing.Font("Tahoma", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipType.Location = New System.Drawing.Point(146, 12)
        Me.lblShipType.Name = "lblShipType"
        Me.lblShipType.Size = New System.Drawing.Size(826, 33)
        Me.lblShipType.TabIndex = 4
        Me.lblShipType.Text = "Ship Type"
        '
        'pbShip
        '
        Me.pbShip.Location = New System.Drawing.Point(12, 12)
        Me.pbShip.Name = "pbShip"
        Me.pbShip.Size = New System.Drawing.Size(128, 128)
        Me.pbShip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbShip.TabIndex = 3
        Me.pbShip.TabStop = False
        '
        'clvLoadouts
        '
        Me.clvLoadouts.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colName, Me.colAuthor, Me.colRating, Me.colDate})
        Me.clvLoadouts.DefaultItemHeight = 20
        Me.clvLoadouts.ItemContextMenu = Me.ctxLoadout
        Me.clvLoadouts.Location = New System.Drawing.Point(13, 147)
        Me.clvLoadouts.Name = "clvLoadouts"
        Me.clvLoadouts.Size = New System.Drawing.Size(490, 463)
        Me.clvLoadouts.TabIndex = 6
        '
        'colName
        '
        Me.colName.CustomSortTag = Nothing
        Me.colName.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colName.Tag = Nothing
        Me.colName.Text = "Loadout Name"
        Me.colName.Width = 200
        '
        'colAuthor
        '
        Me.colAuthor.CustomSortTag = Nothing
        Me.colAuthor.DisplayIndex = 1
        Me.colAuthor.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colAuthor.Tag = Nothing
        Me.colAuthor.Text = "Author"
        Me.colAuthor.Width = 100
        '
        'colRating
        '
        Me.colRating.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colRating.CustomSortTag = Nothing
        Me.colRating.DisplayIndex = 2
        Me.colRating.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Integer]
        Me.colRating.Tag = Nothing
        Me.colRating.Text = "Score"
        Me.colRating.Width = 60
        '
        'colDate
        '
        Me.colDate.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colDate.CustomSortTag = Nothing
        Me.colDate.DisplayIndex = 3
        Me.colDate.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Date]
        Me.colDate.Tag = Nothing
        Me.colDate.Text = "Date"
        Me.colDate.Width = 100
        '
        'ctxLoadout
        '
        Me.ctxLoadout.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewLoadout})
        Me.ctxLoadout.Name = "ctxLoadout"
        Me.ctxLoadout.Size = New System.Drawing.Size(147, 26)
        '
        'mnuViewLoadout
        '
        Me.mnuViewLoadout.Name = "mnuViewLoadout"
        Me.mnuViewLoadout.Size = New System.Drawing.Size(146, 22)
        Me.mnuViewLoadout.Text = "View Loadout"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblBCStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 613)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(984, 22)
        Me.StatusStrip1.TabIndex = 8
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblBCStatus
        '
        Me.lblBCStatus.Name = "lblBCStatus"
        Me.lblBCStatus.Size = New System.Drawing.Size(42, 17)
        Me.lblBCStatus.Text = "Status:"
        '
        'lvwSlots
        '
        Me.lvwSlots.AllowDrop = True
        Me.lvwSlots.FullRowSelect = True
        ListViewGroup1.Header = "High Slots"
        ListViewGroup1.Name = "lvwgHighSlots"
        ListViewGroup2.Header = "Mid Slots"
        ListViewGroup2.Name = "lvwgMidSlots"
        ListViewGroup3.Header = "Low Slots"
        ListViewGroup3.Name = "lvwgLowSlots"
        ListViewGroup4.Header = "Rig Slots"
        ListViewGroup4.Name = "lvwgRigSlots"
        Me.lvwSlots.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4})
        Me.lvwSlots.Location = New System.Drawing.Point(509, 147)
        Me.lvwSlots.Name = "lvwSlots"
        Me.lvwSlots.Size = New System.Drawing.Size(463, 463)
        Me.lvwSlots.TabIndex = 7
        Me.lvwSlots.Tag = ""
        Me.lvwSlots.UseCompatibleStateImageBehavior = False
        Me.lvwSlots.View = System.Windows.Forms.View.Details
        '
        'frmBCBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 635)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.lvwSlots)
        Me.Controls.Add(Me.clvLoadouts)
        Me.Controls.Add(Me.lblShipType)
        Me.Controls.Add(Me.pbShip)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBCBrowser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BattleClinic Browser"
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxLoadout.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblShipType As System.Windows.Forms.Label
    Friend WithEvents pbShip As System.Windows.Forms.PictureBox
    Friend WithEvents clvLoadouts As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colName As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colAuthor As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRating As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colDate As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lvwSlots As EveHQ.HQF.ListViewNoFlicker
    Friend WithEvents ctxLoadout As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuViewLoadout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblBCStatus As System.Windows.Forms.ToolStripStatusLabel
End Class
