<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPilotManager
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPilotManager))
        Me.blbPilots = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ctxHQFLevel = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSetSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuSetLevel0 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSetLevel1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSetLevel2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSetLevel3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSetLevel4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSetLevel5 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuSetDefault = New System.Windows.Forms.ToolStripMenuItem
        Me.clvSkills = New DotNetLib.Windows.Forms.ContainerListView
        Me.colName = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colActualLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colHQFLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnResetAll = New System.Windows.Forms.Button
        Me.btnSetAllToLevel5 = New System.Windows.Forms.Button
        Me.lblSkillsModified = New System.Windows.Forms.Label
        Me.chkShowModifiedSkills = New System.Windows.Forms.CheckBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabSkills = New System.Windows.Forms.TabPage
        Me.lblSkillQueue = New System.Windows.Forms.Label
        Me.cboSkillQueue = New System.Windows.Forms.ComboBox
        Me.btnSetToSkillQueue = New System.Windows.Forms.Button
        Me.btnUpdateSkills = New System.Windows.Forms.Button
        Me.tabImplants = New System.Windows.Forms.TabPage
        Me.btnSaveGroup = New System.Windows.Forms.Button
        Me.cboImplantGroup = New System.Windows.Forms.ComboBox
        Me.lblUseImplantGroup = New System.Windows.Forms.Label
        Me.btnCollapseAll = New System.Windows.Forms.Button
        Me.lblImplantFilter = New System.Windows.Forms.Label
        Me.cboImplantFilter = New System.Windows.Forms.ComboBox
        Me.lblImplantDescription = New System.Windows.Forms.Label
        Me.tvwImplants = New System.Windows.Forms.TreeView
        Me.tabImplantManager = New System.Windows.Forms.TabPage
        Me.lblImplantDescriptionM = New System.Windows.Forms.TextBox
        Me.lblCurrentGroup = New System.Windows.Forms.Label
        Me.btnCollapseAllM = New System.Windows.Forms.Button
        Me.lblImplantFilterM = New System.Windows.Forms.Label
        Me.cboImplantGroupsM = New System.Windows.Forms.ComboBox
        Me.tvwImplantsM = New System.Windows.Forms.TreeView
        Me.btnRemoveImplantGroup = New System.Windows.Forms.Button
        Me.btnEditImplantGroup = New System.Windows.Forms.Button
        Me.btnAddImplantGroup = New System.Windows.Forms.Button
        Me.lstImplantGroups = New System.Windows.Forms.ListBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAddHQFSkillstoQueue = New System.Windows.Forms.Button
        Me.ctxHQFLevel.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabSkills.SuspendLayout()
        Me.tabImplants.SuspendLayout()
        Me.tabImplantManager.SuspendLayout()
        Me.SuspendLayout()
        '
        'blbPilots
        '
        Me.blbPilots.AutoSize = True
        Me.blbPilots.Location = New System.Drawing.Point(13, 13)
        Me.blbPilots.Name = "blbPilots"
        Me.blbPilots.Size = New System.Drawing.Size(36, 13)
        Me.blbPilots.TabIndex = 0
        Me.blbPilots.Text = "Pilots:"
        '
        'cboPilots
        '
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(54, 10)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(174, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 1
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Level0.jpg")
        Me.ImageList1.Images.SetKeyName(1, "Level1.jpg")
        Me.ImageList1.Images.SetKeyName(2, "Level2.jpg")
        Me.ImageList1.Images.SetKeyName(3, "Level3.jpg")
        Me.ImageList1.Images.SetKeyName(4, "Level4.jpg")
        Me.ImageList1.Images.SetKeyName(5, "Level5.jpg")
        Me.ImageList1.Images.SetKeyName(6, "Blank.jpg")
        Me.ImageList1.Images.SetKeyName(7, "Skillbook_16x16.jpg")
        Me.ImageList1.Images.SetKeyName(8, "Skillbook_24x24.jpg")
        Me.ImageList1.Images.SetKeyName(9, "NoSkillbook_16x16.jpg")
        Me.ImageList1.Images.SetKeyName(10, "NoSkillbook_24x24.jpg")
        '
        'ctxHQFLevel
        '
        Me.ctxHQFLevel.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSetSkillName, Me.ToolStripMenuItem1, Me.mnuSetLevel0, Me.mnuSetLevel1, Me.mnuSetLevel2, Me.mnuSetLevel3, Me.mnuSetLevel4, Me.mnuSetLevel5, Me.ToolStripMenuItem2, Me.mnuSetDefault})
        Me.ctxHQFLevel.Name = "ctxHQFLevel"
        Me.ctxHQFLevel.Size = New System.Drawing.Size(149, 192)
        '
        'mnuSetSkillName
        '
        Me.mnuSetSkillName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.mnuSetSkillName.Name = "mnuSetSkillName"
        Me.mnuSetSkillName.Size = New System.Drawing.Size(148, 22)
        Me.mnuSetSkillName.Text = "Skill Name"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(145, 6)
        '
        'mnuSetLevel0
        '
        Me.mnuSetLevel0.Name = "mnuSetLevel0"
        Me.mnuSetLevel0.Size = New System.Drawing.Size(148, 22)
        Me.mnuSetLevel0.Text = "Set To Level 0"
        '
        'mnuSetLevel1
        '
        Me.mnuSetLevel1.Name = "mnuSetLevel1"
        Me.mnuSetLevel1.Size = New System.Drawing.Size(148, 22)
        Me.mnuSetLevel1.Text = "Set To Level 1"
        '
        'mnuSetLevel2
        '
        Me.mnuSetLevel2.Name = "mnuSetLevel2"
        Me.mnuSetLevel2.Size = New System.Drawing.Size(148, 22)
        Me.mnuSetLevel2.Text = "Set To Level 2"
        '
        'mnuSetLevel3
        '
        Me.mnuSetLevel3.Name = "mnuSetLevel3"
        Me.mnuSetLevel3.Size = New System.Drawing.Size(148, 22)
        Me.mnuSetLevel3.Text = "Set To Level 3"
        '
        'mnuSetLevel4
        '
        Me.mnuSetLevel4.Name = "mnuSetLevel4"
        Me.mnuSetLevel4.Size = New System.Drawing.Size(148, 22)
        Me.mnuSetLevel4.Text = "Set To Level 4"
        '
        'mnuSetLevel5
        '
        Me.mnuSetLevel5.Name = "mnuSetLevel5"
        Me.mnuSetLevel5.Size = New System.Drawing.Size(148, 22)
        Me.mnuSetLevel5.Text = "Set To Level 5"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(145, 6)
        '
        'mnuSetDefault
        '
        Me.mnuSetDefault.Name = "mnuSetDefault"
        Me.mnuSetDefault.Size = New System.Drawing.Size(148, 22)
        Me.mnuSetDefault.Text = "Set To Default"
        '
        'clvSkills
        '
        Me.clvSkills.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvSkills.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colName, Me.colActualLevel, Me.colHQFLevel})
        Me.clvSkills.DefaultItemHeight = 20
        Me.clvSkills.ItemContextMenu = Me.ctxHQFLevel
        Me.clvSkills.Location = New System.Drawing.Point(16, 43)
        Me.clvSkills.Name = "clvSkills"
        Me.clvSkills.ShowPlusMinus = True
        Me.clvSkills.ShowRootTreeLines = True
        Me.clvSkills.ShowTreeLines = True
        Me.clvSkills.Size = New System.Drawing.Size(638, 451)
        Me.clvSkills.SmallImageList = Me.ImageList1
        Me.clvSkills.TabIndex = 2
        '
        'colName
        '
        Me.colName.CustomSortTag = Nothing
        Me.colName.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colName.Tag = Nothing
        Me.colName.Text = "Group/Skill Name"
        Me.colName.Width = 300
        '
        'colActualLevel
        '
        Me.colActualLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colActualLevel.CustomSortTag = Nothing
        Me.colActualLevel.DisplayIndex = 1
        Me.colActualLevel.Tag = Nothing
        Me.colActualLevel.Text = "Actual Level"
        '
        'colHQFLevel
        '
        Me.colHQFLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colHQFLevel.CustomSortTag = Nothing
        Me.colHQFLevel.DisplayIndex = 2
        Me.colHQFLevel.Tag = Nothing
        Me.colHQFLevel.Text = "HQF Level"
        '
        'btnResetAll
        '
        Me.btnResetAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnResetAll.Location = New System.Drawing.Point(481, 500)
        Me.btnResetAll.Name = "btnResetAll"
        Me.btnResetAll.Size = New System.Drawing.Size(85, 36)
        Me.btnResetAll.TabIndex = 3
        Me.btnResetAll.Text = "Reset All To Actual"
        Me.ToolTip1.SetToolTip(Me.btnResetAll, "Sets all the skills of the current pilot to the actual skills as per the API")
        Me.btnResetAll.UseVisualStyleBackColor = True
        '
        'btnSetAllToLevel5
        '
        Me.btnSetAllToLevel5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSetAllToLevel5.Location = New System.Drawing.Point(572, 500)
        Me.btnSetAllToLevel5.Name = "btnSetAllToLevel5"
        Me.btnSetAllToLevel5.Size = New System.Drawing.Size(82, 36)
        Me.btnSetAllToLevel5.TabIndex = 4
        Me.btnSetAllToLevel5.Text = "Set All Skills To Level 5"
        Me.ToolTip1.SetToolTip(Me.btnSetAllToLevel5, "Sets all the skill of the current pilot to level 5")
        Me.btnSetAllToLevel5.UseVisualStyleBackColor = True
        '
        'lblSkillsModified
        '
        Me.lblSkillsModified.AutoSize = True
        Me.lblSkillsModified.Location = New System.Drawing.Point(234, 13)
        Me.lblSkillsModified.Name = "lblSkillsModified"
        Me.lblSkillsModified.Size = New System.Drawing.Size(80, 13)
        Me.lblSkillsModified.TabIndex = 5
        Me.lblSkillsModified.Text = "(Skills Modified)"
        '
        'chkShowModifiedSkills
        '
        Me.chkShowModifiedSkills.AutoSize = True
        Me.chkShowModifiedSkills.Location = New System.Drawing.Point(16, 18)
        Me.chkShowModifiedSkills.Name = "chkShowModifiedSkills"
        Me.chkShowModifiedSkills.Size = New System.Drawing.Size(145, 17)
        Me.chkShowModifiedSkills.TabIndex = 6
        Me.chkShowModifiedSkills.Text = "Show Only Modified Skills"
        Me.chkShowModifiedSkills.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.tabSkills)
        Me.TabControl1.Controls.Add(Me.tabImplants)
        Me.TabControl1.Controls.Add(Me.tabImplantManager)
        Me.TabControl1.Location = New System.Drawing.Point(12, 37)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(668, 575)
        Me.TabControl1.TabIndex = 7
        '
        'tabSkills
        '
        Me.tabSkills.Controls.Add(Me.btnAddHQFSkillstoQueue)
        Me.tabSkills.Controls.Add(Me.lblSkillQueue)
        Me.tabSkills.Controls.Add(Me.cboSkillQueue)
        Me.tabSkills.Controls.Add(Me.btnSetToSkillQueue)
        Me.tabSkills.Controls.Add(Me.btnUpdateSkills)
        Me.tabSkills.Controls.Add(Me.chkShowModifiedSkills)
        Me.tabSkills.Controls.Add(Me.clvSkills)
        Me.tabSkills.Controls.Add(Me.btnSetAllToLevel5)
        Me.tabSkills.Controls.Add(Me.btnResetAll)
        Me.tabSkills.Location = New System.Drawing.Point(4, 22)
        Me.tabSkills.Name = "tabSkills"
        Me.tabSkills.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSkills.Size = New System.Drawing.Size(660, 549)
        Me.tabSkills.TabIndex = 0
        Me.tabSkills.Text = "Skills"
        Me.tabSkills.UseVisualStyleBackColor = True
        '
        'lblSkillQueue
        '
        Me.lblSkillQueue.AutoSize = True
        Me.lblSkillQueue.Location = New System.Drawing.Point(13, 500)
        Me.lblSkillQueue.Name = "lblSkillQueue"
        Me.lblSkillQueue.Size = New System.Drawing.Size(63, 13)
        Me.lblSkillQueue.TabIndex = 10
        Me.lblSkillQueue.Text = "Skill Queue:"
        '
        'cboSkillQueue
        '
        Me.cboSkillQueue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSkillQueue.FormattingEnabled = True
        Me.cboSkillQueue.Location = New System.Drawing.Point(16, 515)
        Me.cboSkillQueue.Name = "cboSkillQueue"
        Me.cboSkillQueue.Size = New System.Drawing.Size(183, 21)
        Me.cboSkillQueue.Sorted = True
        Me.cboSkillQueue.TabIndex = 9
        '
        'btnSetToSkillQueue
        '
        Me.btnSetToSkillQueue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSetToSkillQueue.Enabled = False
        Me.btnSetToSkillQueue.Location = New System.Drawing.Point(205, 499)
        Me.btnSetToSkillQueue.Name = "btnSetToSkillQueue"
        Me.btnSetToSkillQueue.Size = New System.Drawing.Size(75, 37)
        Me.btnSetToSkillQueue.TabIndex = 8
        Me.btnSetToSkillQueue.Text = "Set Skills to Skill Queue"
        Me.ToolTip1.SetToolTip(Me.btnSetToSkillQueue, "Increases skills to levels based on the selected skill queue. Does not affect ski" & _
                "lls not in the skill queue therefore you can apply multiple skill queues if requ" & _
                "ired.")
        Me.btnSetToSkillQueue.UseVisualStyleBackColor = True
        '
        'btnUpdateSkills
        '
        Me.btnUpdateSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdateSkills.Location = New System.Drawing.Point(390, 500)
        Me.btnUpdateSkills.Name = "btnUpdateSkills"
        Me.btnUpdateSkills.Size = New System.Drawing.Size(85, 36)
        Me.btnUpdateSkills.TabIndex = 7
        Me.btnUpdateSkills.Text = "Update HQF Skills"
        Me.ToolTip1.SetToolTip(Me.btnUpdateSkills, "Updates all skills less than actual to actual but leaves skills manually set to h" & _
                "igher skill levels")
        Me.btnUpdateSkills.UseVisualStyleBackColor = True
        '
        'tabImplants
        '
        Me.tabImplants.Controls.Add(Me.btnSaveGroup)
        Me.tabImplants.Controls.Add(Me.cboImplantGroup)
        Me.tabImplants.Controls.Add(Me.lblUseImplantGroup)
        Me.tabImplants.Controls.Add(Me.btnCollapseAll)
        Me.tabImplants.Controls.Add(Me.lblImplantFilter)
        Me.tabImplants.Controls.Add(Me.cboImplantFilter)
        Me.tabImplants.Controls.Add(Me.lblImplantDescription)
        Me.tabImplants.Controls.Add(Me.tvwImplants)
        Me.tabImplants.Location = New System.Drawing.Point(4, 22)
        Me.tabImplants.Name = "tabImplants"
        Me.tabImplants.Padding = New System.Windows.Forms.Padding(3)
        Me.tabImplants.Size = New System.Drawing.Size(660, 549)
        Me.tabImplants.TabIndex = 1
        Me.tabImplants.Text = "Implants"
        Me.tabImplants.UseVisualStyleBackColor = True
        '
        'btnSaveGroup
        '
        Me.btnSaveGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveGroup.Location = New System.Drawing.Point(498, 432)
        Me.btnSaveGroup.Name = "btnSaveGroup"
        Me.btnSaveGroup.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveGroup.TabIndex = 16
        Me.btnSaveGroup.Text = "Save Group"
        Me.btnSaveGroup.UseVisualStyleBackColor = True
        '
        'cboImplantGroup
        '
        Me.cboImplantGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboImplantGroup.FormattingEnabled = True
        Me.cboImplantGroup.Items.AddRange(New Object() {"<custom>"})
        Me.cboImplantGroup.Location = New System.Drawing.Point(115, 25)
        Me.cboImplantGroup.Name = "cboImplantGroup"
        Me.cboImplantGroup.Size = New System.Drawing.Size(235, 21)
        Me.cboImplantGroup.Sorted = True
        Me.cboImplantGroup.TabIndex = 15
        '
        'lblUseImplantGroup
        '
        Me.lblUseImplantGroup.AutoSize = True
        Me.lblUseImplantGroup.Location = New System.Drawing.Point(13, 28)
        Me.lblUseImplantGroup.Name = "lblUseImplantGroup"
        Me.lblUseImplantGroup.Size = New System.Drawing.Size(96, 13)
        Me.lblUseImplantGroup.TabIndex = 14
        Me.lblUseImplantGroup.Text = "Use Implant Group"
        '
        'btnCollapseAll
        '
        Me.btnCollapseAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCollapseAll.Location = New System.Drawing.Point(579, 432)
        Me.btnCollapseAll.Name = "btnCollapseAll"
        Me.btnCollapseAll.Size = New System.Drawing.Size(75, 23)
        Me.btnCollapseAll.TabIndex = 13
        Me.btnCollapseAll.Text = "Collapse All"
        Me.btnCollapseAll.UseVisualStyleBackColor = True
        '
        'lblImplantFilter
        '
        Me.lblImplantFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblImplantFilter.AutoSize = True
        Me.lblImplantFilter.Location = New System.Drawing.Point(13, 435)
        Me.lblImplantFilter.Name = "lblImplantFilter"
        Me.lblImplantFilter.Size = New System.Drawing.Size(102, 13)
        Me.lblImplantFilter.TabIndex = 12
        Me.lblImplantFilter.Text = "Implant Group Filter"
        '
        'cboImplantFilter
        '
        Me.cboImplantFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboImplantFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboImplantFilter.FormattingEnabled = True
        Me.cboImplantFilter.Location = New System.Drawing.Point(121, 432)
        Me.cboImplantFilter.Name = "cboImplantFilter"
        Me.cboImplantFilter.Size = New System.Drawing.Size(150, 21)
        Me.cboImplantFilter.Sorted = True
        Me.cboImplantFilter.TabIndex = 11
        '
        'lblImplantDescription
        '
        Me.lblImplantDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblImplantDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblImplantDescription.Location = New System.Drawing.Point(16, 466)
        Me.lblImplantDescription.Name = "lblImplantDescription"
        Me.lblImplantDescription.Size = New System.Drawing.Size(638, 70)
        Me.lblImplantDescription.TabIndex = 10
        '
        'tvwImplants
        '
        Me.tvwImplants.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwImplants.Location = New System.Drawing.Point(16, 52)
        Me.tvwImplants.Name = "tvwImplants"
        Me.tvwImplants.Size = New System.Drawing.Size(638, 374)
        Me.tvwImplants.TabIndex = 9
        '
        'tabImplantManager
        '
        Me.tabImplantManager.Controls.Add(Me.lblImplantDescriptionM)
        Me.tabImplantManager.Controls.Add(Me.lblCurrentGroup)
        Me.tabImplantManager.Controls.Add(Me.btnCollapseAllM)
        Me.tabImplantManager.Controls.Add(Me.lblImplantFilterM)
        Me.tabImplantManager.Controls.Add(Me.cboImplantGroupsM)
        Me.tabImplantManager.Controls.Add(Me.tvwImplantsM)
        Me.tabImplantManager.Controls.Add(Me.btnRemoveImplantGroup)
        Me.tabImplantManager.Controls.Add(Me.btnEditImplantGroup)
        Me.tabImplantManager.Controls.Add(Me.btnAddImplantGroup)
        Me.tabImplantManager.Controls.Add(Me.lstImplantGroups)
        Me.tabImplantManager.Location = New System.Drawing.Point(4, 22)
        Me.tabImplantManager.Name = "tabImplantManager"
        Me.tabImplantManager.Size = New System.Drawing.Size(660, 549)
        Me.tabImplantManager.TabIndex = 2
        Me.tabImplantManager.Text = "Implant Manager"
        Me.tabImplantManager.UseVisualStyleBackColor = True
        '
        'lblImplantDescriptionM
        '
        Me.lblImplantDescriptionM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblImplantDescriptionM.Location = New System.Drawing.Point(221, 464)
        Me.lblImplantDescriptionM.Multiline = True
        Me.lblImplantDescriptionM.Name = "lblImplantDescriptionM"
        Me.lblImplantDescriptionM.ReadOnly = True
        Me.lblImplantDescriptionM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.lblImplantDescriptionM.Size = New System.Drawing.Size(428, 73)
        Me.lblImplantDescriptionM.TabIndex = 19
        '
        'lblCurrentGroup
        '
        Me.lblCurrentGroup.AutoSize = True
        Me.lblCurrentGroup.Location = New System.Drawing.Point(218, 13)
        Me.lblCurrentGroup.Name = "lblCurrentGroup"
        Me.lblCurrentGroup.Size = New System.Drawing.Size(80, 13)
        Me.lblCurrentGroup.TabIndex = 18
        Me.lblCurrentGroup.Text = "Current Group:"
        '
        'btnCollapseAllM
        '
        Me.btnCollapseAllM.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCollapseAllM.Location = New System.Drawing.Point(574, 430)
        Me.btnCollapseAllM.Name = "btnCollapseAllM"
        Me.btnCollapseAllM.Size = New System.Drawing.Size(75, 23)
        Me.btnCollapseAllM.TabIndex = 17
        Me.btnCollapseAllM.Text = "Collapse All"
        Me.btnCollapseAllM.UseVisualStyleBackColor = True
        '
        'lblImplantFilterM
        '
        Me.lblImplantFilterM.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblImplantFilterM.AutoSize = True
        Me.lblImplantFilterM.Location = New System.Drawing.Point(218, 435)
        Me.lblImplantFilterM.Name = "lblImplantFilterM"
        Me.lblImplantFilterM.Size = New System.Drawing.Size(102, 13)
        Me.lblImplantFilterM.TabIndex = 16
        Me.lblImplantFilterM.Text = "Implant Group Filter"
        '
        'cboImplantGroupsM
        '
        Me.cboImplantGroupsM.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboImplantGroupsM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboImplantGroupsM.FormattingEnabled = True
        Me.cboImplantGroupsM.Location = New System.Drawing.Point(326, 432)
        Me.cboImplantGroupsM.Name = "cboImplantGroupsM"
        Me.cboImplantGroupsM.Size = New System.Drawing.Size(131, 21)
        Me.cboImplantGroupsM.Sorted = True
        Me.cboImplantGroupsM.TabIndex = 15
        '
        'tvwImplantsM
        '
        Me.tvwImplantsM.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwImplantsM.Location = New System.Drawing.Point(221, 29)
        Me.tvwImplantsM.Name = "tvwImplantsM"
        Me.tvwImplantsM.Size = New System.Drawing.Size(428, 395)
        Me.tvwImplantsM.TabIndex = 13
        '
        'btnRemoveImplantGroup
        '
        Me.btnRemoveImplantGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveImplantGroup.Location = New System.Drawing.Point(143, 497)
        Me.btnRemoveImplantGroup.Name = "btnRemoveImplantGroup"
        Me.btnRemoveImplantGroup.Size = New System.Drawing.Size(60, 40)
        Me.btnRemoveImplantGroup.TabIndex = 12
        Me.btnRemoveImplantGroup.Text = "Remove Group"
        Me.btnRemoveImplantGroup.UseVisualStyleBackColor = True
        '
        'btnEditImplantGroup
        '
        Me.btnEditImplantGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditImplantGroup.Location = New System.Drawing.Point(77, 497)
        Me.btnEditImplantGroup.Name = "btnEditImplantGroup"
        Me.btnEditImplantGroup.Size = New System.Drawing.Size(60, 40)
        Me.btnEditImplantGroup.TabIndex = 11
        Me.btnEditImplantGroup.Text = "Edit Group"
        Me.btnEditImplantGroup.UseVisualStyleBackColor = True
        '
        'btnAddImplantGroup
        '
        Me.btnAddImplantGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddImplantGroup.Location = New System.Drawing.Point(11, 497)
        Me.btnAddImplantGroup.Name = "btnAddImplantGroup"
        Me.btnAddImplantGroup.Size = New System.Drawing.Size(60, 40)
        Me.btnAddImplantGroup.TabIndex = 10
        Me.btnAddImplantGroup.Text = "Add Group"
        Me.btnAddImplantGroup.UseVisualStyleBackColor = True
        '
        'lstImplantGroups
        '
        Me.lstImplantGroups.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstImplantGroups.FormattingEnabled = True
        Me.lstImplantGroups.Location = New System.Drawing.Point(11, 13)
        Me.lstImplantGroups.Name = "lstImplantGroups"
        Me.lstImplantGroups.Size = New System.Drawing.Size(192, 472)
        Me.lstImplantGroups.TabIndex = 9
        '
        'btnAddHQFSkillstoQueue
        '
        Me.btnAddHQFSkillstoQueue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddHQFSkillstoQueue.Location = New System.Drawing.Point(299, 500)
        Me.btnAddHQFSkillstoQueue.Name = "btnAddHQFSkillstoQueue"
        Me.btnAddHQFSkillstoQueue.Size = New System.Drawing.Size(85, 36)
        Me.btnAddHQFSkillstoQueue.TabIndex = 11
        Me.btnAddHQFSkillstoQueue.Text = "Add HQF Skills to Queue"
        Me.ToolTip1.SetToolTip(Me.btnAddHQFSkillstoQueue, "Updates all skills less than actual to actual but leaves skills manually set to h" & _
                "igher skill levels")
        Me.btnAddHQFSkillstoQueue.UseVisualStyleBackColor = True
        '
        'frmPilotManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 624)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.lblSkillsModified)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.blbPilots)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmPilotManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HQF Pilot Manager"
        Me.ctxHQFLevel.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.tabSkills.ResumeLayout(False)
        Me.tabSkills.PerformLayout()
        Me.tabImplants.ResumeLayout(False)
        Me.tabImplants.PerformLayout()
        Me.tabImplantManager.ResumeLayout(False)
        Me.tabImplantManager.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents blbPilots As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents clvSkills As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colName As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colActualLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colHQFLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ctxHQFLevel As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSetSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSetLevel0 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSetLevel1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSetLevel2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSetLevel3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSetLevel4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSetLevel5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSetDefault As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnResetAll As System.Windows.Forms.Button
    Friend WithEvents btnSetAllToLevel5 As System.Windows.Forms.Button
    Friend WithEvents lblSkillsModified As System.Windows.Forms.Label
    Friend WithEvents chkShowModifiedSkills As System.Windows.Forms.CheckBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabSkills As System.Windows.Forms.TabPage
    Friend WithEvents tabImplants As System.Windows.Forms.TabPage
    Friend WithEvents btnCollapseAll As System.Windows.Forms.Button
    Friend WithEvents lblImplantFilter As System.Windows.Forms.Label
    Friend WithEvents cboImplantFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lblImplantDescription As System.Windows.Forms.Label
    Friend WithEvents tvwImplants As System.Windows.Forms.TreeView
    Friend WithEvents cboImplantGroup As System.Windows.Forms.ComboBox
    Friend WithEvents lblUseImplantGroup As System.Windows.Forms.Label
    Friend WithEvents btnUpdateSkills As System.Windows.Forms.Button
    Friend WithEvents btnSaveGroup As System.Windows.Forms.Button
    Friend WithEvents tabImplantManager As System.Windows.Forms.TabPage
    Friend WithEvents btnCollapseAllM As System.Windows.Forms.Button
    Friend WithEvents lblImplantFilterM As System.Windows.Forms.Label
    Friend WithEvents cboImplantGroupsM As System.Windows.Forms.ComboBox
    Friend WithEvents tvwImplantsM As System.Windows.Forms.TreeView
    Friend WithEvents btnRemoveImplantGroup As System.Windows.Forms.Button
    Friend WithEvents btnEditImplantGroup As System.Windows.Forms.Button
    Friend WithEvents btnAddImplantGroup As System.Windows.Forms.Button
    Friend WithEvents lstImplantGroups As System.Windows.Forms.ListBox
    Friend WithEvents lblCurrentGroup As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnSetToSkillQueue As System.Windows.Forms.Button
    Friend WithEvents lblSkillQueue As System.Windows.Forms.Label
    Friend WithEvents cboSkillQueue As System.Windows.Forms.ComboBox
    Friend WithEvents lblImplantDescriptionM As System.Windows.Forms.TextBox
    Friend WithEvents btnAddHQFSkillstoQueue As System.Windows.Forms.Button
End Class
