<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShowInfo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmShowInfo))
        Me.tvwReqs = New System.Windows.Forms.TreeView
        Me.ctxReqs = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewSkillDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.ctxDepend = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuItemName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewItemDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabSIDescription = New System.Windows.Forms.TabPage
        Me.lblDescription = New System.Windows.Forms.Label
        Me.tabSIAttributes = New System.Windows.Forms.TabPage
        Me.lvwAttributes = New System.Windows.Forms.ListView
        Me.colAttribute = New System.Windows.Forms.ColumnHeader
        Me.colData = New System.Windows.Forms.ColumnHeader
        Me.tabSISkills = New System.Windows.Forms.TabPage
        Me.btnViewSkills = New System.Windows.Forms.Button
        Me.btnAddSkills = New System.Windows.Forms.Button
        Me.tabSIBonuses = New System.Windows.Forms.TabPage
        Me.lvwBonuses = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblUsableTime = New System.Windows.Forms.LinkLabel
        Me.lblUsable = New System.Windows.Forms.Label
        Me.picItem = New System.Windows.Forms.PictureBox
        Me.lblItemName = New System.Windows.Forms.Label
        Me.pbPilot = New System.Windows.Forms.PictureBox
        Me.ctxReqs.SuspendLayout()
        Me.ctxDepend.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabSIDescription.SuspendLayout()
        Me.tabSIAttributes.SuspendLayout()
        Me.tabSISkills.SuspendLayout()
        Me.tabSIBonuses.SuspendLayout()
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tvwReqs
        '
        Me.tvwReqs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwReqs.ContextMenuStrip = Me.ctxReqs
        Me.tvwReqs.Indent = 25
        Me.tvwReqs.ItemHeight = 20
        Me.tvwReqs.Location = New System.Drawing.Point(3, 3)
        Me.tvwReqs.Name = "tvwReqs"
        Me.tvwReqs.ShowPlusMinus = False
        Me.tvwReqs.Size = New System.Drawing.Size(362, 316)
        Me.tvwReqs.TabIndex = 0
        '
        'ctxReqs
        '
        Me.ctxReqs.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxReqs.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator2, Me.mnuViewSkillDetails})
        Me.ctxReqs.Name = "ctxDepend"
        Me.ctxReqs.Size = New System.Drawing.Size(133, 54)
        '
        'mnuSkillName
        '
        Me.mnuSkillName.Name = "mnuSkillName"
        Me.mnuSkillName.Size = New System.Drawing.Size(132, 22)
        Me.mnuSkillName.Text = "Skill Name"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(129, 6)
        '
        'mnuViewSkillDetails
        '
        Me.mnuViewSkillDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewSkillDetails.Name = "mnuViewSkillDetails"
        Me.mnuViewSkillDetails.Size = New System.Drawing.Size(132, 22)
        Me.mnuViewSkillDetails.Text = "View Details"
        '
        'ctxDepend
        '
        Me.ctxDepend.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxDepend.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuItemName, Me.ToolStripSeparator1, Me.mnuViewItemDetails})
        Me.ctxDepend.Name = "ctxDepend"
        Me.ctxDepend.Size = New System.Drawing.Size(138, 54)
        '
        'mnuItemName
        '
        Me.mnuItemName.Name = "mnuItemName"
        Me.mnuItemName.Size = New System.Drawing.Size(137, 22)
        Me.mnuItemName.Text = "Item Name"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(134, 6)
        '
        'mnuViewItemDetails
        '
        Me.mnuViewItemDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewItemDetails.Name = "mnuViewItemDetails"
        Me.mnuViewItemDetails.Size = New System.Drawing.Size(137, 22)
        Me.mnuViewItemDetails.Text = "View Details"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabSIDescription)
        Me.TabControl1.Controls.Add(Me.tabSIAttributes)
        Me.TabControl1.Controls.Add(Me.tabSISkills)
        Me.TabControl1.Controls.Add(Me.tabSIBonuses)
        Me.TabControl1.Location = New System.Drawing.Point(12, 146)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(379, 376)
        Me.TabControl1.TabIndex = 2
        '
        'tabSIDescription
        '
        Me.tabSIDescription.Controls.Add(Me.lblDescription)
        Me.tabSIDescription.Location = New System.Drawing.Point(4, 22)
        Me.tabSIDescription.Name = "tabSIDescription"
        Me.tabSIDescription.Size = New System.Drawing.Size(371, 350)
        Me.tabSIDescription.TabIndex = 2
        Me.tabSIDescription.Text = "Description"
        Me.tabSIDescription.UseVisualStyleBackColor = True
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.Color.White
        Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDescription.Location = New System.Drawing.Point(0, 0)
        Me.lblDescription.Margin = New System.Windows.Forms.Padding(5)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Padding = New System.Windows.Forms.Padding(5)
        Me.lblDescription.Size = New System.Drawing.Size(371, 350)
        Me.lblDescription.TabIndex = 0
        '
        'tabSIAttributes
        '
        Me.tabSIAttributes.Controls.Add(Me.lvwAttributes)
        Me.tabSIAttributes.Location = New System.Drawing.Point(4, 22)
        Me.tabSIAttributes.Name = "tabSIAttributes"
        Me.tabSIAttributes.Size = New System.Drawing.Size(371, 350)
        Me.tabSIAttributes.TabIndex = 3
        Me.tabSIAttributes.Text = "Attributes"
        Me.tabSIAttributes.UseVisualStyleBackColor = True
        '
        'lvwAttributes
        '
        Me.lvwAttributes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttribute, Me.colData})
        Me.lvwAttributes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwAttributes.FullRowSelect = True
        Me.lvwAttributes.GridLines = True
        Me.lvwAttributes.Location = New System.Drawing.Point(0, 0)
        Me.lvwAttributes.Name = "lvwAttributes"
        Me.lvwAttributes.Size = New System.Drawing.Size(371, 350)
        Me.lvwAttributes.TabIndex = 1
        Me.lvwAttributes.UseCompatibleStateImageBehavior = False
        Me.lvwAttributes.View = System.Windows.Forms.View.Details
        '
        'colAttribute
        '
        Me.colAttribute.Text = "Attribute"
        Me.colAttribute.Width = 210
        '
        'colData
        '
        Me.colData.Text = "Data"
        Me.colData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colData.Width = 135
        '
        'tabSISkills
        '
        Me.tabSISkills.Controls.Add(Me.btnViewSkills)
        Me.tabSISkills.Controls.Add(Me.btnAddSkills)
        Me.tabSISkills.Controls.Add(Me.tvwReqs)
        Me.tabSISkills.Location = New System.Drawing.Point(4, 22)
        Me.tabSISkills.Name = "tabSISkills"
        Me.tabSISkills.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSISkills.Size = New System.Drawing.Size(371, 350)
        Me.tabSISkills.TabIndex = 0
        Me.tabSISkills.Text = "Req Skills"
        Me.tabSISkills.UseVisualStyleBackColor = True
        '
        'btnViewSkills
        '
        Me.btnViewSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnViewSkills.Location = New System.Drawing.Point(3, 321)
        Me.btnViewSkills.Name = "btnViewSkills"
        Me.btnViewSkills.Size = New System.Drawing.Size(121, 23)
        Me.btnViewSkills.TabIndex = 5
        Me.btnViewSkills.Text = "Show Needed Skills"
        Me.btnViewSkills.UseVisualStyleBackColor = True
        '
        'btnAddSkills
        '
        Me.btnAddSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddSkills.Location = New System.Drawing.Point(210, 321)
        Me.btnAddSkills.Name = "btnAddSkills"
        Me.btnAddSkills.Size = New System.Drawing.Size(155, 23)
        Me.btnAddSkills.TabIndex = 4
        Me.btnAddSkills.Text = "Add Needed Skills to Queue"
        Me.btnAddSkills.UseVisualStyleBackColor = True
        '
        'tabSIBonuses
        '
        Me.tabSIBonuses.Controls.Add(Me.lvwBonuses)
        Me.tabSIBonuses.Location = New System.Drawing.Point(4, 22)
        Me.tabSIBonuses.Name = "tabSIBonuses"
        Me.tabSIBonuses.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSIBonuses.Size = New System.Drawing.Size(371, 350)
        Me.tabSIBonuses.TabIndex = 1
        Me.tabSIBonuses.Text = "Bonuses"
        Me.tabSIBonuses.UseVisualStyleBackColor = True
        '
        'lvwBonuses
        '
        Me.lvwBonuses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvwBonuses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwBonuses.FullRowSelect = True
        Me.lvwBonuses.GridLines = True
        Me.lvwBonuses.Location = New System.Drawing.Point(3, 3)
        Me.lvwBonuses.Name = "lvwBonuses"
        Me.lvwBonuses.Size = New System.Drawing.Size(365, 344)
        Me.lvwBonuses.TabIndex = 2
        Me.lvwBonuses.UseCompatibleStateImageBehavior = False
        Me.lvwBonuses.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Bonuses"
        Me.ColumnHeader1.Width = 210
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Data"
        Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader2.Width = 135
        '
        'lblUsableTime
        '
        Me.lblUsableTime.AutoSize = True
        Me.lblUsableTime.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsableTime.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblUsableTime.Location = New System.Drawing.Point(146, 127)
        Me.lblUsableTime.Name = "lblUsableTime"
        Me.lblUsableTime.Size = New System.Drawing.Size(68, 13)
        Me.lblUsableTime.TabIndex = 15
        Me.lblUsableTime.TabStop = True
        Me.lblUsableTime.Text = "Usable Time:"
        '
        'lblUsable
        '
        Me.lblUsable.AutoSize = True
        Me.lblUsable.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsable.Location = New System.Drawing.Point(146, 112)
        Me.lblUsable.Name = "lblUsable"
        Me.lblUsable.Size = New System.Drawing.Size(39, 13)
        Me.lblUsable.TabIndex = 14
        Me.lblUsable.Text = "Usable"
        '
        'picItem
        '
        Me.picItem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picItem.Location = New System.Drawing.Point(12, 12)
        Me.picItem.Name = "picItem"
        Me.picItem.Size = New System.Drawing.Size(128, 128)
        Me.picItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picItem.TabIndex = 13
        Me.picItem.TabStop = False
        '
        'lblItemName
        '
        Me.lblItemName.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemName.Location = New System.Drawing.Point(147, 13)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(244, 35)
        Me.lblItemName.TabIndex = 16
        Me.lblItemName.Text = "Item Label That Spans At Least 2 Lines"
        '
        'pbPilot
        '
        Me.pbPilot.Image = Global.EveHQ.HQF.My.Resources.Resources.noitem
        Me.pbPilot.Location = New System.Drawing.Point(147, 52)
        Me.pbPilot.Name = "pbPilot"
        Me.pbPilot.Size = New System.Drawing.Size(48, 48)
        Me.pbPilot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbPilot.TabIndex = 17
        Me.pbPilot.TabStop = False
        '
        'frmShowInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(396, 527)
        Me.Controls.Add(Me.pbPilot)
        Me.Controls.Add(Me.lblItemName)
        Me.Controls.Add(Me.lblUsableTime)
        Me.Controls.Add(Me.lblUsable)
        Me.Controls.Add(Me.picItem)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmShowInfo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Show Info"
        Me.ctxReqs.ResumeLayout(False)
        Me.ctxDepend.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.tabSIDescription.ResumeLayout(False)
        Me.tabSIAttributes.ResumeLayout(False)
        Me.tabSISkills.ResumeLayout(False)
        Me.tabSIBonuses.ResumeLayout(False)
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabSISkills As System.Windows.Forms.TabPage
    Friend WithEvents tabSIBonuses As System.Windows.Forms.TabPage
    Friend WithEvents ctxDepend As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tabSIDescription As System.Windows.Forms.TabPage
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents SkillToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ctxReqs As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewSkillDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewItemDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblUsableTime As System.Windows.Forms.LinkLabel
    Friend WithEvents lblUsable As System.Windows.Forms.Label
    Friend WithEvents picItem As System.Windows.Forms.PictureBox
    Friend WithEvents btnViewSkills As System.Windows.Forms.Button
    Friend WithEvents btnAddSkills As System.Windows.Forms.Button
    Friend WithEvents tabSIAttributes As System.Windows.Forms.TabPage
    Friend WithEvents lvwAttributes As System.Windows.Forms.ListView
    Friend WithEvents colAttribute As System.Windows.Forms.ColumnHeader
    Friend WithEvents colData As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwBonuses As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblItemName As System.Windows.Forms.Label
    Friend WithEvents pbPilot As System.Windows.Forms.PictureBox
End Class
