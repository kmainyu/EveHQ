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
        Me.tabShowInfo = New System.Windows.Forms.TabControl
        Me.tabSIDescription = New System.Windows.Forms.TabPage
        Me.lblDescription = New System.Windows.Forms.Label
        Me.tabSIAttributes = New System.Windows.Forms.TabPage
        Me.lvwAttributes = New System.Windows.Forms.ListView
        Me.colAttribute = New System.Windows.Forms.ColumnHeader
        Me.colStandardValue = New System.Windows.Forms.ColumnHeader
        Me.colPilotValue = New System.Windows.Forms.ColumnHeader
        Me.tabSISkills = New System.Windows.Forms.TabPage
        Me.btnViewSkills = New System.Windows.Forms.Button
        Me.btnAddSkills = New System.Windows.Forms.Button
        Me.tabAudit = New System.Windows.Forms.TabPage
        Me.lvwAudit = New System.Windows.Forms.ListView
        Me.colAudit = New System.Windows.Forms.ColumnHeader
        Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblUsableTime = New System.Windows.Forms.LinkLabel
        Me.lblUsable = New System.Windows.Forms.Label
        Me.picItem = New System.Windows.Forms.PictureBox
        Me.lblItemName = New System.Windows.Forms.Label
        Me.pbPilot = New System.Windows.Forms.PictureBox
        Me.ctxReqs.SuspendLayout()
        Me.ctxDepend.SuspendLayout()
        Me.tabShowInfo.SuspendLayout()
        Me.tabSIDescription.SuspendLayout()
        Me.tabSIAttributes.SuspendLayout()
        Me.tabSISkills.SuspendLayout()
        Me.tabAudit.SuspendLayout()
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
        Me.tvwReqs.Size = New System.Drawing.Size(456, 319)
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
        'tabShowInfo
        '
        Me.tabShowInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabShowInfo.Controls.Add(Me.tabSIDescription)
        Me.tabShowInfo.Controls.Add(Me.tabSIAttributes)
        Me.tabShowInfo.Controls.Add(Me.tabSISkills)
        Me.tabShowInfo.Controls.Add(Me.tabAudit)
        Me.tabShowInfo.Location = New System.Drawing.Point(12, 146)
        Me.tabShowInfo.Name = "tabShowInfo"
        Me.tabShowInfo.SelectedIndex = 0
        Me.tabShowInfo.Size = New System.Drawing.Size(573, 419)
        Me.tabShowInfo.TabIndex = 2
        '
        'tabSIDescription
        '
        Me.tabSIDescription.Controls.Add(Me.lblDescription)
        Me.tabSIDescription.Location = New System.Drawing.Point(4, 22)
        Me.tabSIDescription.Name = "tabSIDescription"
        Me.tabSIDescription.Size = New System.Drawing.Size(565, 393)
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
        Me.lblDescription.Size = New System.Drawing.Size(565, 393)
        Me.lblDescription.TabIndex = 0
        '
        'tabSIAttributes
        '
        Me.tabSIAttributes.Controls.Add(Me.lvwAttributes)
        Me.tabSIAttributes.Location = New System.Drawing.Point(4, 22)
        Me.tabSIAttributes.Name = "tabSIAttributes"
        Me.tabSIAttributes.Size = New System.Drawing.Size(565, 393)
        Me.tabSIAttributes.TabIndex = 3
        Me.tabSIAttributes.Text = "Attributes"
        Me.tabSIAttributes.UseVisualStyleBackColor = True
        '
        'lvwAttributes
        '
        Me.lvwAttributes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttribute, Me.colStandardValue, Me.colPilotValue})
        Me.lvwAttributes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwAttributes.FullRowSelect = True
        Me.lvwAttributes.GridLines = True
        Me.lvwAttributes.Location = New System.Drawing.Point(0, 0)
        Me.lvwAttributes.Name = "lvwAttributes"
        Me.lvwAttributes.Size = New System.Drawing.Size(565, 393)
        Me.lvwAttributes.TabIndex = 1
        Me.lvwAttributes.UseCompatibleStateImageBehavior = False
        Me.lvwAttributes.View = System.Windows.Forms.View.Details
        '
        'colAttribute
        '
        Me.colAttribute.Text = "Attribute"
        Me.colAttribute.Width = 240
        '
        'colStandardValue
        '
        Me.colStandardValue.Text = "Base Value"
        Me.colStandardValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colStandardValue.Width = 150
        '
        'colPilotValue
        '
        Me.colPilotValue.Text = "Actual Value"
        Me.colPilotValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colPilotValue.Width = 150
        '
        'tabSISkills
        '
        Me.tabSISkills.Controls.Add(Me.btnViewSkills)
        Me.tabSISkills.Controls.Add(Me.btnAddSkills)
        Me.tabSISkills.Controls.Add(Me.tvwReqs)
        Me.tabSISkills.Location = New System.Drawing.Point(4, 22)
        Me.tabSISkills.Name = "tabSISkills"
        Me.tabSISkills.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSISkills.Size = New System.Drawing.Size(565, 393)
        Me.tabSISkills.TabIndex = 0
        Me.tabSISkills.Text = "Req Skills"
        Me.tabSISkills.UseVisualStyleBackColor = True
        '
        'btnViewSkills
        '
        Me.btnViewSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnViewSkills.Location = New System.Drawing.Point(3, 324)
        Me.btnViewSkills.Name = "btnViewSkills"
        Me.btnViewSkills.Size = New System.Drawing.Size(121, 23)
        Me.btnViewSkills.TabIndex = 5
        Me.btnViewSkills.Text = "Show Needed Skills"
        Me.btnViewSkills.UseVisualStyleBackColor = True
        '
        'btnAddSkills
        '
        Me.btnAddSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddSkills.Location = New System.Drawing.Point(304, 324)
        Me.btnAddSkills.Name = "btnAddSkills"
        Me.btnAddSkills.Size = New System.Drawing.Size(155, 23)
        Me.btnAddSkills.TabIndex = 4
        Me.btnAddSkills.Text = "Add Needed Skills to Queue"
        Me.btnAddSkills.UseVisualStyleBackColor = True
        '
        'tabAudit
        '
        Me.tabAudit.Controls.Add(Me.lvwAudit)
        Me.tabAudit.Location = New System.Drawing.Point(4, 22)
        Me.tabAudit.Name = "tabAudit"
        Me.tabAudit.Size = New System.Drawing.Size(565, 393)
        Me.tabAudit.TabIndex = 4
        Me.tabAudit.Text = "Audit"
        Me.tabAudit.UseVisualStyleBackColor = True
        '
        'lvwAudit
        '
        Me.lvwAudit.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAudit})
        Me.lvwAudit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwAudit.FullRowSelect = True
        Me.lvwAudit.GridLines = True
        Me.lvwAudit.Location = New System.Drawing.Point(0, 0)
        Me.lvwAudit.Name = "lvwAudit"
        Me.lvwAudit.Size = New System.Drawing.Size(565, 393)
        Me.lvwAudit.TabIndex = 3
        Me.lvwAudit.UseCompatibleStateImageBehavior = False
        Me.lvwAudit.View = System.Windows.Forms.View.Details
        '
        'colAudit
        '
        Me.colAudit.Text = "Details"
        Me.colAudit.Width = 540
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
        Me.lblItemName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblItemName.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemName.Location = New System.Drawing.Point(147, 13)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(434, 35)
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
        Me.ClientSize = New System.Drawing.Size(590, 570)
        Me.Controls.Add(Me.pbPilot)
        Me.Controls.Add(Me.lblItemName)
        Me.Controls.Add(Me.lblUsableTime)
        Me.Controls.Add(Me.lblUsable)
        Me.Controls.Add(Me.picItem)
        Me.Controls.Add(Me.tabShowInfo)
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
        Me.tabShowInfo.ResumeLayout(False)
        Me.tabSIDescription.ResumeLayout(False)
        Me.tabSIAttributes.ResumeLayout(False)
        Me.tabSISkills.ResumeLayout(False)
        Me.tabAudit.ResumeLayout(False)
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
    Friend WithEvents tabShowInfo As System.Windows.Forms.TabControl
    Friend WithEvents tabSISkills As System.Windows.Forms.TabPage
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
    Friend WithEvents colStandardValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblItemName As System.Windows.Forms.Label
    Friend WithEvents pbPilot As System.Windows.Forms.PictureBox
    Friend WithEvents tabAudit As System.Windows.Forms.TabPage
    Friend WithEvents lvwAudit As System.Windows.Forms.ListView
    Friend WithEvents colAudit As System.Windows.Forms.ColumnHeader
    Friend WithEvents colPilotValue As System.Windows.Forms.ColumnHeader
End Class
