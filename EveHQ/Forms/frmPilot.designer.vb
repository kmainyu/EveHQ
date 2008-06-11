<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmPilot
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPilot))
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Corporation Management", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Drones", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Electronics", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Engineering", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Gunnery", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup6 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Industry", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup7 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Leadership", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup8 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Learning", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup9 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mechanic", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup10 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Missiles", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup11 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Navigation", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup12 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Science", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup13 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Social", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup14 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Spaceship Command", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup15 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Trade", System.Windows.Forms.HorizontalAlignment.Left)
        Me.ctxSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuForceTraining = New System.Windows.Forms.ToolStripMenuItem
        Me.picPilot = New System.Windows.Forms.PictureBox
        Me.ctxPic = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCtxPicGetPortraitFromServer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCtxPicGetPortraitFromLocal = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSavePortrait = New System.Windows.Forms.ToolStripMenuItem
        Me.lvImplants = New System.Windows.Forms.ListView
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnCharXML = New System.Windows.Forms.Button
        Me.btnTrainingXML = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lvAttributes = New System.Windows.Forms.ListView
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.Label7 = New System.Windows.Forms.Label
        Me.chkManualImplants = New System.Windows.Forms.CheckBox
        Me.btnEditImplants = New System.Windows.Forms.Button
        Me.chkSkillGroups = New System.Windows.Forms.CheckBox
        Me.chkGraphicalView = New System.Windows.Forms.CheckBox
        Me.lvSkills = New EveHQ.ListViewNoFlicker
        Me.Skill = New System.Windows.Forms.ColumnHeader
        Me.Rank = New System.Windows.Forms.ColumnHeader
        Me.Level = New System.Windows.Forms.ColumnHeader
        Me.Percent = New System.Windows.Forms.ColumnHeader
        Me.Skillpoints = New System.Windows.Forms.ColumnHeader
        Me.LevelUp = New System.Windows.Forms.ColumnHeader
        Me.lvTraining = New EveHQ.ListViewNoFlicker
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.lvPilot = New EveHQ.ListViewNoFlicker
        Me.Category = New System.Windows.Forms.ColumnHeader
        Me.Data = New System.Windows.Forms.ColumnHeader
        Me.ctxSkills.SuspendLayout()
        CType(Me.picPilot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxPic.SuspendLayout()
        Me.SuspendLayout()
        '
        'ctxSkills
        '
        Me.ctxSkills.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator1, Me.mnuViewDetails, Me.ToolStripMenuItem1, Me.mnuForceTraining})
        Me.ctxSkills.Name = "ctxSkills"
        Me.ctxSkills.Size = New System.Drawing.Size(175, 82)
        '
        'mnuSkillName
        '
        Me.mnuSkillName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.mnuSkillName.Name = "mnuSkillName"
        Me.mnuSkillName.Size = New System.Drawing.Size(174, 22)
        Me.mnuSkillName.Text = "Skill Name"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(171, 6)
        '
        'mnuViewDetails
        '
        Me.mnuViewDetails.Name = "mnuViewDetails"
        Me.mnuViewDetails.Size = New System.Drawing.Size(174, 22)
        Me.mnuViewDetails.Text = "View Details"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(171, 6)
        '
        'mnuForceTraining
        '
        Me.mnuForceTraining.Name = "mnuForceTraining"
        Me.mnuForceTraining.Size = New System.Drawing.Size(174, 22)
        Me.mnuForceTraining.Text = "Force Skill Training"
        '
        'picPilot
        '
        Me.picPilot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picPilot.ContextMenuStrip = Me.ctxPic
        Me.picPilot.ImageLocation = ""
        Me.picPilot.Location = New System.Drawing.Point(8, 12)
        Me.picPilot.Margin = New System.Windows.Forms.Padding(0)
        Me.picPilot.Name = "picPilot"
        Me.picPilot.Size = New System.Drawing.Size(128, 128)
        Me.picPilot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picPilot.TabIndex = 11
        Me.picPilot.TabStop = False
        '
        'ctxPic
        '
        Me.ctxPic.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCtxPicGetPortraitFromServer, Me.mnuCtxPicGetPortraitFromLocal, Me.mnuSavePortrait})
        Me.ctxPic.Name = "ctxPic"
        Me.ctxPic.Size = New System.Drawing.Size(246, 70)
        '
        'mnuCtxPicGetPortraitFromServer
        '
        Me.mnuCtxPicGetPortraitFromServer.Name = "mnuCtxPicGetPortraitFromServer"
        Me.mnuCtxPicGetPortraitFromServer.Size = New System.Drawing.Size(245, 22)
        Me.mnuCtxPicGetPortraitFromServer.Text = "Get Portrait from Eve Server"
        '
        'mnuCtxPicGetPortraitFromLocal
        '
        Me.mnuCtxPicGetPortraitFromLocal.Name = "mnuCtxPicGetPortraitFromLocal"
        Me.mnuCtxPicGetPortraitFromLocal.Size = New System.Drawing.Size(245, 22)
        Me.mnuCtxPicGetPortraitFromLocal.Text = "Get Portrait from Eve Installation"
        '
        'mnuSavePortrait
        '
        Me.mnuSavePortrait.Name = "mnuSavePortrait"
        Me.mnuSavePortrait.Size = New System.Drawing.Size(245, 22)
        Me.mnuSavePortrait.Text = "Save Portrait into EveHQ Cache"
        '
        'lvImplants
        '
        Me.lvImplants.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lvImplants.FullRowSelect = True
        Me.lvImplants.GridLines = True
        Me.lvImplants.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvImplants.Location = New System.Drawing.Point(8, 310)
        Me.lvImplants.MultiSelect = False
        Me.lvImplants.Name = "lvImplants"
        Me.lvImplants.Size = New System.Drawing.Size(141, 115)
        Me.lvImplants.TabIndex = 23
        Me.lvImplants.UseCompatibleStateImageBehavior = False
        Me.lvImplants.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Attribute"
        Me.ColumnHeader2.Width = 75
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Modifier"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(5, 294)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Implants:"
        '
        'btnCharXML
        '
        Me.btnCharXML.Image = CType(resources.GetObject("btnCharXML.Image"), System.Drawing.Image)
        Me.btnCharXML.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCharXML.Location = New System.Drawing.Point(8, 486)
        Me.btnCharXML.Name = "btnCharXML"
        Me.btnCharXML.Size = New System.Drawing.Size(98, 22)
        Me.btnCharXML.TabIndex = 27
        Me.btnCharXML.Text = "Character"
        Me.btnCharXML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnTrainingXML
        '
        Me.btnTrainingXML.Image = CType(resources.GetObject("btnTrainingXML.Image"), System.Drawing.Image)
        Me.btnTrainingXML.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnTrainingXML.Location = New System.Drawing.Point(8, 514)
        Me.btnTrainingXML.Name = "btnTrainingXML"
        Me.btnTrainingXML.Size = New System.Drawing.Size(98, 22)
        Me.btnTrainingXML.TabIndex = 28
        Me.btnTrainingXML.Text = "Training"
        Me.btnTrainingXML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(151, 145)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 13)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "Skill Training:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(151, 294)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 30
        Me.Label6.Text = "Skills:"
        '
        'lvAttributes
        '
        Me.lvAttributes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.ColumnHeader7})
        Me.lvAttributes.FullRowSelect = True
        Me.lvAttributes.GridLines = True
        Me.lvAttributes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvAttributes.Location = New System.Drawing.Point(8, 161)
        Me.lvAttributes.MultiSelect = False
        Me.lvAttributes.Name = "lvAttributes"
        Me.lvAttributes.ShowItemToolTips = True
        Me.lvAttributes.Size = New System.Drawing.Size(141, 114)
        Me.lvAttributes.TabIndex = 31
        Me.lvAttributes.UseCompatibleStateImageBehavior = False
        Me.lvAttributes.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Attribute"
        Me.ColumnHeader6.Width = 75
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Value"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 145)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "Attributes:"
        '
        'chkManualImplants
        '
        Me.chkManualImplants.Location = New System.Drawing.Point(8, 431)
        Me.chkManualImplants.Name = "chkManualImplants"
        Me.chkManualImplants.Size = New System.Drawing.Size(84, 30)
        Me.chkManualImplants.TabIndex = 33
        Me.chkManualImplants.Text = "Manual Implants"
        Me.chkManualImplants.UseVisualStyleBackColor = True
        '
        'btnEditImplants
        '
        Me.btnEditImplants.Enabled = False
        Me.btnEditImplants.Location = New System.Drawing.Point(85, 431)
        Me.btnEditImplants.Name = "btnEditImplants"
        Me.btnEditImplants.Size = New System.Drawing.Size(64, 34)
        Me.btnEditImplants.TabIndex = 34
        Me.btnEditImplants.Text = "Edit Implants"
        Me.btnEditImplants.UseVisualStyleBackColor = True
        '
        'chkSkillGroups
        '
        Me.chkSkillGroups.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkSkillGroups.AutoSize = True
        Me.chkSkillGroups.Checked = True
        Me.chkSkillGroups.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSkillGroups.Location = New System.Drawing.Point(745, 287)
        Me.chkSkillGroups.Name = "chkSkillGroups"
        Me.chkSkillGroups.Size = New System.Drawing.Size(128, 17)
        Me.chkSkillGroups.TabIndex = 35
        Me.chkSkillGroups.Text = "Show Skills in Groups"
        Me.chkSkillGroups.UseVisualStyleBackColor = True
        '
        'chkGraphicalView
        '
        Me.chkGraphicalView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkGraphicalView.AutoSize = True
        Me.chkGraphicalView.Checked = True
        Me.chkGraphicalView.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGraphicalView.Location = New System.Drawing.Point(612, 287)
        Me.chkGraphicalView.Name = "chkGraphicalView"
        Me.chkGraphicalView.Size = New System.Drawing.Size(112, 17)
        Me.chkGraphicalView.TabIndex = 36
        Me.chkGraphicalView.Text = "Graphical Skill List"
        Me.chkGraphicalView.UseVisualStyleBackColor = True
        Me.chkGraphicalView.Visible = False
        '
        'lvSkills
        '
        Me.lvSkills.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSkills.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Skill, Me.Rank, Me.Level, Me.Percent, Me.Skillpoints, Me.LevelUp})
        Me.lvSkills.ContextMenuStrip = Me.ctxSkills
        Me.lvSkills.FullRowSelect = True
        ListViewGroup1.Header = "Corporation Management"
        ListViewGroup1.Name = "grpCorp"
        ListViewGroup2.Header = "Drones"
        ListViewGroup2.Name = "grpDron"
        ListViewGroup3.Header = "Electronics"
        ListViewGroup3.Name = "grpElec"
        ListViewGroup4.Header = "Engineering"
        ListViewGroup4.Name = "grpEngi"
        ListViewGroup5.Header = "Gunnery"
        ListViewGroup5.Name = "grpGunn"
        ListViewGroup6.Header = "Industry"
        ListViewGroup6.Name = "grpIndu"
        ListViewGroup7.Header = "Leadership"
        ListViewGroup7.Name = "grpLead"
        ListViewGroup8.Header = "Learning"
        ListViewGroup8.Name = "grpLear"
        ListViewGroup9.Header = "Mechanic"
        ListViewGroup9.Name = "grpMech"
        ListViewGroup10.Header = "Missiles"
        ListViewGroup10.Name = "grpMiss"
        ListViewGroup11.Header = "Navigation"
        ListViewGroup11.Name = "grpNavi"
        ListViewGroup12.Header = "Science"
        ListViewGroup12.Name = "grpScie"
        ListViewGroup13.Header = "Social"
        ListViewGroup13.Name = "grpSoci"
        ListViewGroup14.Header = "Spaceship Command"
        ListViewGroup14.Name = "grpSpac"
        ListViewGroup15.Header = "Trade"
        ListViewGroup15.Name = "grpTrad"
        Me.lvSkills.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4, ListViewGroup5, ListViewGroup6, ListViewGroup7, ListViewGroup8, ListViewGroup9, ListViewGroup10, ListViewGroup11, ListViewGroup12, ListViewGroup13, ListViewGroup14, ListViewGroup15})
        Me.lvSkills.Location = New System.Drawing.Point(152, 310)
        Me.lvSkills.MultiSelect = False
        Me.lvSkills.Name = "lvSkills"
        Me.lvSkills.OwnerDraw = True
        Me.lvSkills.Size = New System.Drawing.Size(721, 244)
        Me.lvSkills.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvSkills.TabIndex = 29
        Me.lvSkills.UseCompatibleStateImageBehavior = False
        Me.lvSkills.View = System.Windows.Forms.View.Details
        '
        'Skill
        '
        Me.Skill.Text = "Skill"
        Me.Skill.Width = 210
        '
        'Rank
        '
        Me.Rank.Text = "Rank"
        Me.Rank.Width = 50
        '
        'Level
        '
        Me.Level.Text = "Level"
        Me.Level.Width = 50
        '
        'Percent
        '
        Me.Percent.Text = "% Done"
        '
        'Skillpoints
        '
        Me.Skillpoints.Text = "Skillpoints"
        Me.Skillpoints.Width = 100
        '
        'LevelUp
        '
        Me.LevelUp.Text = "Time to Level Up"
        Me.LevelUp.Width = 120
        '
        'lvTraining
        '
        Me.lvTraining.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvTraining.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader4, Me.ColumnHeader5})
        Me.lvTraining.FullRowSelect = True
        Me.lvTraining.GridLines = True
        Me.lvTraining.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvTraining.Location = New System.Drawing.Point(152, 161)
        Me.lvTraining.MultiSelect = False
        Me.lvTraining.Name = "lvTraining"
        Me.lvTraining.Size = New System.Drawing.Size(721, 114)
        Me.lvTraining.TabIndex = 25
        Me.lvTraining.UseCompatibleStateImageBehavior = False
        Me.lvTraining.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Attribute"
        Me.ColumnHeader4.Width = 150
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Description"
        Me.ColumnHeader5.Width = 300
        '
        'lvPilot
        '
        Me.lvPilot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvPilot.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Category, Me.Data})
        Me.lvPilot.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvPilot.FullRowSelect = True
        Me.lvPilot.GridLines = True
        Me.lvPilot.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvPilot.Location = New System.Drawing.Point(152, 12)
        Me.lvPilot.MultiSelect = False
        Me.lvPilot.Name = "lvPilot"
        Me.lvPilot.Size = New System.Drawing.Size(721, 126)
        Me.lvPilot.TabIndex = 22
        Me.lvPilot.UseCompatibleStateImageBehavior = False
        Me.lvPilot.View = System.Windows.Forms.View.Details
        '
        'Category
        '
        Me.Category.Text = "Category"
        Me.Category.Width = 150
        '
        'Data
        '
        Me.Data.Text = "Data"
        Me.Data.Width = 260
        '
        'frmPilot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(885, 566)
        Me.Controls.Add(Me.chkGraphicalView)
        Me.Controls.Add(Me.lvSkills)
        Me.Controls.Add(Me.lvTraining)
        Me.Controls.Add(Me.btnEditImplants)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.chkManualImplants)
        Me.Controls.Add(Me.lvPilot)
        Me.Controls.Add(Me.lvAttributes)
        Me.Controls.Add(Me.btnTrainingXML)
        Me.Controls.Add(Me.btnCharXML)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.picPilot)
        Me.Controls.Add(Me.lvImplants)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.chkSkillGroups)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPilot"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EveHQ Pilot Data"
        Me.ctxSkills.ResumeLayout(False)
        CType(Me.picPilot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxPic.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lvSkills As EveHQ.ListViewNoFlicker
    Friend WithEvents Skill As System.Windows.Forms.ColumnHeader
    Friend WithEvents Rank As System.Windows.Forms.ColumnHeader
    Friend WithEvents Level As System.Windows.Forms.ColumnHeader
    Friend WithEvents Skillpoints As System.Windows.Forms.ColumnHeader
    Friend WithEvents picPilot As System.Windows.Forms.PictureBox
    Friend WithEvents lvPilot As EveHQ.ListViewNoFlicker
    Friend WithEvents Category As System.Windows.Forms.ColumnHeader
    Friend WithEvents Data As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvImplants As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnCharXML As System.Windows.Forms.Button
    Friend WithEvents btnTrainingXML As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lvAttributes As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lvTraining As EveHQ.ListViewNoFlicker
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents LevelUp As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxSkills As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkManualImplants As System.Windows.Forms.CheckBox
    Friend WithEvents btnEditImplants As System.Windows.Forms.Button
    Friend WithEvents chkSkillGroups As System.Windows.Forms.CheckBox
    Friend WithEvents ctxPic As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCtxPicGetPortraitFromServer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCtxPicGetPortraitFromLocal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSavePortrait As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuForceTraining As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkGraphicalView As System.Windows.Forms.CheckBox
    Friend WithEvents Percent As System.Windows.Forms.ColumnHeader

End Class
