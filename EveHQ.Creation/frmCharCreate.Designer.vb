<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCharCreate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCharCreate))
        Me.tabCreation = New System.Windows.Forms.TabControl
        Me.tabSelection = New System.Windows.Forms.TabPage
        Me.btnAddPilot = New System.Windows.Forms.Button
        Me.gbSkills = New System.Windows.Forms.GroupBox
        Me.lvwSkills = New System.Windows.Forms.ListView
        Me.colSkill = New System.Windows.Forms.ColumnHeader
        Me.colLevel = New System.Windows.Forms.ColumnHeader
        Me.colSP = New System.Windows.Forms.ColumnHeader
        Me.gbAttributes = New System.Windows.Forms.GroupBox
        Me.lblSP = New System.Windows.Forms.Label
        Me.lblMem = New System.Windows.Forms.Label
        Me.lblWil = New System.Windows.Forms.Label
        Me.lblPer = New System.Windows.Forms.Label
        Me.lblCha = New System.Windows.Forms.Label
        Me.lblInt = New System.Windows.Forms.Label
        Me.cboAncestry = New System.Windows.Forms.ComboBox
        Me.lblStep3 = New System.Windows.Forms.Label
        Me.cboBloodline = New System.Windows.Forms.ComboBox
        Me.lblStep2 = New System.Windows.Forms.Label
        Me.cboRace = New System.Windows.Forms.ComboBox
        Me.lblStep1 = New System.Windows.Forms.Label
        Me.tabVariations = New System.Windows.Forms.TabPage
        Me.lvwChars = New System.Windows.Forms.ListView
        Me.colNo = New System.Windows.Forms.ColumnHeader
        Me.colRace = New System.Windows.Forms.ColumnHeader
        Me.colBlood = New System.Windows.Forms.ColumnHeader
        Me.colAncestry = New System.Windows.Forms.ColumnHeader
        Me.colC = New System.Windows.Forms.ColumnHeader
        Me.colI = New System.Windows.Forms.ColumnHeader
        Me.colM = New System.Windows.Forms.ColumnHeader
        Me.colP = New System.Windows.Forms.ColumnHeader
        Me.colW = New System.Windows.Forms.ColumnHeader
        Me.colSkillpoints = New System.Windows.Forms.ColumnHeader
        Me.ctxChars = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddCharacter = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExportToCSV = New System.Windows.Forms.ToolStripMenuItem
        Me.tabStartSkills = New System.Windows.Forms.TabPage
        Me.lblSelectedSkills = New System.Windows.Forms.Label
        Me.lvwChars2 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.lstStartSkills = New System.Windows.Forms.ListBox
        Me.tabGoalSeek = New System.Windows.Forms.TabPage
        Me.lblMethod = New System.Windows.Forms.Label
        Me.cboMethod = New System.Windows.Forms.ComboBox
        Me.btnCancelSeek = New System.Windows.Forms.Button
        Me.btnRemoveGoal = New System.Windows.Forms.Button
        Me.btnDecreaseLevel = New System.Windows.Forms.Button
        Me.btnClearGoals = New System.Windows.Forms.Button
        Me.btnIncreaseLevel = New System.Windows.Forms.Button
        Me.lblAvailableGoals = New System.Windows.Forms.Label
        Me.tvwSkillList = New System.Windows.Forms.TreeView
        Me.ctxAddSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuAddLevel1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddLevel2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddLevel3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddLevel4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddLevel5 = New System.Windows.Forms.ToolStripMenuItem
        Me.lblSelectedGoals = New System.Windows.Forms.Label
        Me.lstChars = New System.Windows.Forms.ListView
        Me.colCharacterNo = New System.Windows.Forms.ColumnHeader
        Me.colCharRace = New System.Windows.Forms.ColumnHeader
        Me.colCharBlood = New System.Windows.Forms.ColumnHeader
        Me.colCharAncestry = New System.Windows.Forms.ColumnHeader
        Me.colCharC = New System.Windows.Forms.ColumnHeader
        Me.colCharI = New System.Windows.Forms.ColumnHeader
        Me.colCharM = New System.Windows.Forms.ColumnHeader
        Me.colCharP = New System.Windows.Forms.ColumnHeader
        Me.colCharW = New System.Windows.Forms.ColumnHeader
        Me.colOTime = New System.Windows.Forms.ColumnHeader
        Me.colNTime = New System.Windows.Forms.ColumnHeader
        Me.pbPilots = New System.Windows.Forms.ProgressBar
        Me.btnSeek = New System.Windows.Forms.Button
        Me.lvwSkillSelection = New System.Windows.Forms.ListView
        Me.colSkillName = New System.Windows.Forms.ColumnHeader
        Me.colSkillLevel = New System.Windows.Forms.ColumnHeader
        Me.ctxEditSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuEditSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuEditLevel1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditLevel2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditLevel3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditLevel4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditLevel5 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuDeleteSkill = New System.Windows.Forms.ToolStripMenuItem
        Me.sfd1 = New System.Windows.Forms.SaveFileDialog
        Me.tabCreation.SuspendLayout()
        Me.tabSelection.SuspendLayout()
        Me.gbSkills.SuspendLayout()
        Me.gbAttributes.SuspendLayout()
        Me.tabVariations.SuspendLayout()
        Me.ctxChars.SuspendLayout()
        Me.tabStartSkills.SuspendLayout()
        Me.tabGoalSeek.SuspendLayout()
        Me.ctxAddSkills.SuspendLayout()
        Me.ctxEditSkills.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabCreation
        '
        Me.tabCreation.Controls.Add(Me.tabSelection)
        Me.tabCreation.Controls.Add(Me.tabVariations)
        Me.tabCreation.Controls.Add(Me.tabStartSkills)
        Me.tabCreation.Controls.Add(Me.tabGoalSeek)
        Me.tabCreation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabCreation.Location = New System.Drawing.Point(0, 0)
        Me.tabCreation.Name = "tabCreation"
        Me.tabCreation.SelectedIndex = 0
        Me.tabCreation.Size = New System.Drawing.Size(969, 683)
        Me.tabCreation.TabIndex = 17
        '
        'tabSelection
        '
        Me.tabSelection.Controls.Add(Me.btnAddPilot)
        Me.tabSelection.Controls.Add(Me.gbSkills)
        Me.tabSelection.Controls.Add(Me.gbAttributes)
        Me.tabSelection.Controls.Add(Me.cboAncestry)
        Me.tabSelection.Controls.Add(Me.lblStep3)
        Me.tabSelection.Controls.Add(Me.cboBloodline)
        Me.tabSelection.Controls.Add(Me.lblStep2)
        Me.tabSelection.Controls.Add(Me.cboRace)
        Me.tabSelection.Controls.Add(Me.lblStep1)
        Me.tabSelection.Location = New System.Drawing.Point(4, 22)
        Me.tabSelection.Name = "tabSelection"
        Me.tabSelection.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSelection.Size = New System.Drawing.Size(961, 657)
        Me.tabSelection.TabIndex = 0
        Me.tabSelection.Text = "Character Selection"
        Me.tabSelection.UseVisualStyleBackColor = True
        '
        'btnAddPilot
        '
        Me.btnAddPilot.Enabled = False
        Me.btnAddPilot.Location = New System.Drawing.Point(394, 256)
        Me.btnAddPilot.Name = "btnAddPilot"
        Me.btnAddPilot.Size = New System.Drawing.Size(180, 23)
        Me.btnAddPilot.TabIndex = 24
        Me.btnAddPilot.Text = "Add Character to EveHQ Pilots"
        Me.btnAddPilot.UseVisualStyleBackColor = True
        '
        'gbSkills
        '
        Me.gbSkills.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbSkills.Controls.Add(Me.lvwSkills)
        Me.gbSkills.Location = New System.Drawing.Point(12, 118)
        Me.gbSkills.Name = "gbSkills"
        Me.gbSkills.Size = New System.Drawing.Size(341, 531)
        Me.gbSkills.TabIndex = 23
        Me.gbSkills.TabStop = False
        Me.gbSkills.Text = "Character Skills"
        '
        'lvwSkills
        '
        Me.lvwSkills.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwSkills.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSkill, Me.colLevel, Me.colSP})
        Me.lvwSkills.Location = New System.Drawing.Point(7, 20)
        Me.lvwSkills.Name = "lvwSkills"
        Me.lvwSkills.Size = New System.Drawing.Size(328, 505)
        Me.lvwSkills.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSkills.TabIndex = 0
        Me.lvwSkills.UseCompatibleStateImageBehavior = False
        Me.lvwSkills.View = System.Windows.Forms.View.Details
        '
        'colSkill
        '
        Me.colSkill.Text = "Skill Name"
        Me.colSkill.Width = 200
        '
        'colLevel
        '
        Me.colLevel.Text = "Level"
        Me.colLevel.Width = 50
        '
        'colSP
        '
        Me.colSP.Text = "Skillpoints"
        Me.colSP.Width = 70
        '
        'gbAttributes
        '
        Me.gbAttributes.Controls.Add(Me.lblSP)
        Me.gbAttributes.Controls.Add(Me.lblMem)
        Me.gbAttributes.Controls.Add(Me.lblWil)
        Me.gbAttributes.Controls.Add(Me.lblPer)
        Me.gbAttributes.Controls.Add(Me.lblCha)
        Me.gbAttributes.Controls.Add(Me.lblInt)
        Me.gbAttributes.Location = New System.Drawing.Point(394, 20)
        Me.gbAttributes.Name = "gbAttributes"
        Me.gbAttributes.Size = New System.Drawing.Size(180, 193)
        Me.gbAttributes.TabIndex = 22
        Me.gbAttributes.TabStop = False
        Me.gbAttributes.Text = "Character Attributes && Skillpoints"
        '
        'lblSP
        '
        Me.lblSP.AutoSize = True
        Me.lblSP.Location = New System.Drawing.Point(6, 160)
        Me.lblSP.Name = "lblSP"
        Me.lblSP.Size = New System.Drawing.Size(76, 13)
        Me.lblSP.TabIndex = 5
        Me.lblSP.Text = "Skillpoints: n/a"
        '
        'lblMem
        '
        Me.lblMem.AutoSize = True
        Me.lblMem.Location = New System.Drawing.Point(6, 80)
        Me.lblMem.Name = "lblMem"
        Me.lblMem.Size = New System.Drawing.Size(68, 13)
        Me.lblMem.TabIndex = 4
        Me.lblMem.Text = "Memory: n/a"
        '
        'lblWil
        '
        Me.lblWil.AutoSize = True
        Me.lblWil.Location = New System.Drawing.Point(6, 130)
        Me.lblWil.Name = "lblWil"
        Me.lblWil.Size = New System.Drawing.Size(76, 13)
        Me.lblWil.TabIndex = 3
        Me.lblWil.Text = "Willpower: n/a"
        '
        'lblPer
        '
        Me.lblPer.AutoSize = True
        Me.lblPer.Location = New System.Drawing.Point(6, 105)
        Me.lblPer.Name = "lblPer"
        Me.lblPer.Size = New System.Drawing.Size(81, 13)
        Me.lblPer.TabIndex = 2
        Me.lblPer.Text = "Perception: n/a"
        '
        'lblCha
        '
        Me.lblCha.AutoSize = True
        Me.lblCha.Location = New System.Drawing.Point(6, 30)
        Me.lblCha.Name = "lblCha"
        Me.lblCha.Size = New System.Drawing.Size(74, 13)
        Me.lblCha.TabIndex = 1
        Me.lblCha.Text = "Charisma: n/a"
        '
        'lblInt
        '
        Me.lblInt.AutoSize = True
        Me.lblInt.Location = New System.Drawing.Point(6, 55)
        Me.lblInt.Name = "lblInt"
        Me.lblInt.Size = New System.Drawing.Size(85, 13)
        Me.lblInt.TabIndex = 0
        Me.lblInt.Text = "Intelligence: n/a"
        '
        'cboAncestry
        '
        Me.cboAncestry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAncestry.Enabled = False
        Me.cboAncestry.FormattingEnabled = True
        Me.cboAncestry.Location = New System.Drawing.Point(163, 74)
        Me.cboAncestry.Name = "cboAncestry"
        Me.cboAncestry.Size = New System.Drawing.Size(190, 21)
        Me.cboAncestry.TabIndex = 17
        '
        'lblStep3
        '
        Me.lblStep3.AutoSize = True
        Me.lblStep3.Enabled = False
        Me.lblStep3.Location = New System.Drawing.Point(9, 77)
        Me.lblStep3.Name = "lblStep3"
        Me.lblStep3.Size = New System.Drawing.Size(127, 13)
        Me.lblStep3.TabIndex = 16
        Me.lblStep3.Text = "Step 3: Choose Ancestry"
        '
        'cboBloodline
        '
        Me.cboBloodline.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBloodline.Enabled = False
        Me.cboBloodline.FormattingEnabled = True
        Me.cboBloodline.Location = New System.Drawing.Point(163, 47)
        Me.cboBloodline.Name = "cboBloodline"
        Me.cboBloodline.Size = New System.Drawing.Size(190, 21)
        Me.cboBloodline.Sorted = True
        Me.cboBloodline.TabIndex = 15
        '
        'lblStep2
        '
        Me.lblStep2.AutoSize = True
        Me.lblStep2.Enabled = False
        Me.lblStep2.Location = New System.Drawing.Point(9, 50)
        Me.lblStep2.Name = "lblStep2"
        Me.lblStep2.Size = New System.Drawing.Size(126, 13)
        Me.lblStep2.TabIndex = 14
        Me.lblStep2.Text = "Step 2: Choose Bloodline"
        '
        'cboRace
        '
        Me.cboRace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRace.FormattingEnabled = True
        Me.cboRace.Location = New System.Drawing.Point(163, 20)
        Me.cboRace.Name = "cboRace"
        Me.cboRace.Size = New System.Drawing.Size(190, 21)
        Me.cboRace.Sorted = True
        Me.cboRace.TabIndex = 13
        '
        'lblStep1
        '
        Me.lblStep1.AutoSize = True
        Me.lblStep1.Location = New System.Drawing.Point(9, 23)
        Me.lblStep1.Name = "lblStep1"
        Me.lblStep1.Size = New System.Drawing.Size(108, 13)
        Me.lblStep1.TabIndex = 12
        Me.lblStep1.Text = "Step 1: Choose Race"
        '
        'tabVariations
        '
        Me.tabVariations.Controls.Add(Me.lvwChars)
        Me.tabVariations.Location = New System.Drawing.Point(4, 22)
        Me.tabVariations.Name = "tabVariations"
        Me.tabVariations.Padding = New System.Windows.Forms.Padding(3)
        Me.tabVariations.Size = New System.Drawing.Size(961, 657)
        Me.tabVariations.TabIndex = 1
        Me.tabVariations.Text = "Character Variations"
        Me.tabVariations.UseVisualStyleBackColor = True
        '
        'lvwChars
        '
        Me.lvwChars.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colNo, Me.colRace, Me.colBlood, Me.colAncestry, Me.colC, Me.colI, Me.colM, Me.colP, Me.colW, Me.colSkillpoints})
        Me.lvwChars.ContextMenuStrip = Me.ctxChars
        Me.lvwChars.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwChars.FullRowSelect = True
        Me.lvwChars.Location = New System.Drawing.Point(3, 3)
        Me.lvwChars.Name = "lvwChars"
        Me.lvwChars.ShowItemToolTips = True
        Me.lvwChars.Size = New System.Drawing.Size(955, 651)
        Me.lvwChars.TabIndex = 1
        Me.lvwChars.UseCompatibleStateImageBehavior = False
        Me.lvwChars.View = System.Windows.Forms.View.Details
        '
        'colNo
        '
        Me.colNo.Text = "No"
        Me.colNo.Width = 30
        '
        'colRace
        '
        Me.colRace.Text = "Race"
        '
        'colBlood
        '
        Me.colBlood.Text = "Bloodline"
        '
        'colAncestry
        '
        Me.colAncestry.Text = "Ancestry"
        Me.colAncestry.Width = 120
        '
        'colC
        '
        Me.colC.Text = "C"
        Me.colC.Width = 25
        '
        'colI
        '
        Me.colI.Text = "I"
        Me.colI.Width = 25
        '
        'colM
        '
        Me.colM.Text = "M"
        Me.colM.Width = 25
        '
        'colP
        '
        Me.colP.Text = "P"
        Me.colP.Width = 25
        '
        'colW
        '
        Me.colW.Text = "W"
        Me.colW.Width = 25
        '
        'colSkillpoints
        '
        Me.colSkillpoints.Text = "Skillpoints"
        Me.colSkillpoints.Width = 75
        '
        'ctxChars
        '
        Me.ctxChars.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddCharacter, Me.ToolStripMenuItem3, Me.mnuExportToCSV})
        Me.ctxChars.Name = "ctxChars"
        Me.ctxChars.Size = New System.Drawing.Size(236, 54)
        '
        'mnuAddCharacter
        '
        Me.mnuAddCharacter.Name = "mnuAddCharacter"
        Me.mnuAddCharacter.Size = New System.Drawing.Size(235, 22)
        Me.mnuAddCharacter.Text = "Add Character to EveHQ Pilots"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(232, 6)
        '
        'mnuExportToCSV
        '
        Me.mnuExportToCSV.Name = "mnuExportToCSV"
        Me.mnuExportToCSV.Size = New System.Drawing.Size(235, 22)
        Me.mnuExportToCSV.Text = "Export List to CSV"
        '
        'tabStartSkills
        '
        Me.tabStartSkills.Controls.Add(Me.lblSelectedSkills)
        Me.tabStartSkills.Controls.Add(Me.lvwChars2)
        Me.tabStartSkills.Controls.Add(Me.lstStartSkills)
        Me.tabStartSkills.Location = New System.Drawing.Point(4, 22)
        Me.tabStartSkills.Name = "tabStartSkills"
        Me.tabStartSkills.Size = New System.Drawing.Size(961, 657)
        Me.tabStartSkills.TabIndex = 2
        Me.tabStartSkills.Text = "Starting Skills"
        Me.tabStartSkills.UseVisualStyleBackColor = True
        '
        'lblSelectedSkills
        '
        Me.lblSelectedSkills.AutoSize = True
        Me.lblSelectedSkills.Location = New System.Drawing.Point(216, 32)
        Me.lblSelectedSkills.Name = "lblSelectedSkills"
        Me.lblSelectedSkills.Size = New System.Drawing.Size(77, 39)
        Me.lblSelectedSkills.TabIndex = 3
        Me.lblSelectedSkills.Text = "Selected Skills:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<None>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lvwChars2
        '
        Me.lvwChars2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwChars2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader11, Me.ColumnHeader12})
        Me.lvwChars2.ContextMenuStrip = Me.ctxChars
        Me.lvwChars2.FullRowSelect = True
        Me.lvwChars2.Location = New System.Drawing.Point(9, 274)
        Me.lvwChars2.Name = "lvwChars2"
        Me.lvwChars2.ShowItemToolTips = True
        Me.lvwChars2.Size = New System.Drawing.Size(672, 375)
        Me.lvwChars2.TabIndex = 2
        Me.lvwChars2.UseCompatibleStateImageBehavior = False
        Me.lvwChars2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "No"
        Me.ColumnHeader1.Width = 30
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Race"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Bloodline"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Ancestry"
        Me.ColumnHeader4.Width = 120
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "C"
        Me.ColumnHeader7.Width = 25
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "I"
        Me.ColumnHeader8.Width = 25
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "M"
        Me.ColumnHeader9.Width = 25
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "P"
        Me.ColumnHeader10.Width = 25
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "W"
        Me.ColumnHeader11.Width = 25
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Skillpoints"
        Me.ColumnHeader12.Width = 75
        '
        'lstStartSkills
        '
        Me.lstStartSkills.FormattingEnabled = True
        Me.lstStartSkills.Location = New System.Drawing.Point(9, 4)
        Me.lstStartSkills.Name = "lstStartSkills"
        Me.lstStartSkills.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstStartSkills.Size = New System.Drawing.Size(200, 264)
        Me.lstStartSkills.Sorted = True
        Me.lstStartSkills.TabIndex = 0
        '
        'tabGoalSeek
        '
        Me.tabGoalSeek.Controls.Add(Me.lblMethod)
        Me.tabGoalSeek.Controls.Add(Me.cboMethod)
        Me.tabGoalSeek.Controls.Add(Me.btnCancelSeek)
        Me.tabGoalSeek.Controls.Add(Me.btnRemoveGoal)
        Me.tabGoalSeek.Controls.Add(Me.btnDecreaseLevel)
        Me.tabGoalSeek.Controls.Add(Me.btnClearGoals)
        Me.tabGoalSeek.Controls.Add(Me.btnIncreaseLevel)
        Me.tabGoalSeek.Controls.Add(Me.lblAvailableGoals)
        Me.tabGoalSeek.Controls.Add(Me.tvwSkillList)
        Me.tabGoalSeek.Controls.Add(Me.lblSelectedGoals)
        Me.tabGoalSeek.Controls.Add(Me.lstChars)
        Me.tabGoalSeek.Controls.Add(Me.pbPilots)
        Me.tabGoalSeek.Controls.Add(Me.btnSeek)
        Me.tabGoalSeek.Controls.Add(Me.lvwSkillSelection)
        Me.tabGoalSeek.Location = New System.Drawing.Point(4, 22)
        Me.tabGoalSeek.Name = "tabGoalSeek"
        Me.tabGoalSeek.Size = New System.Drawing.Size(961, 657)
        Me.tabGoalSeek.TabIndex = 3
        Me.tabGoalSeek.Text = "Character Goal Seek"
        Me.tabGoalSeek.UseVisualStyleBackColor = True
        '
        'lblMethod
        '
        Me.lblMethod.AutoSize = True
        Me.lblMethod.Location = New System.Drawing.Point(619, 280)
        Me.lblMethod.Name = "lblMethod"
        Me.lblMethod.Size = New System.Drawing.Size(142, 13)
        Me.lblMethod.TabIndex = 14
        Me.lblMethod.Text = "Attribute Allocation Method:"
        '
        'cboMethod
        '
        Me.cboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMethod.FormattingEnabled = True
        Me.cboMethod.Items.AddRange(New Object() {"Quick Optimal", "Long Optimal", "Even Spread"})
        Me.cboMethod.Location = New System.Drawing.Point(619, 299)
        Me.cboMethod.Name = "cboMethod"
        Me.cboMethod.Size = New System.Drawing.Size(165, 21)
        Me.cboMethod.TabIndex = 13
        '
        'btnCancelSeek
        '
        Me.btnCancelSeek.Enabled = False
        Me.btnCancelSeek.Location = New System.Drawing.Point(704, 344)
        Me.btnCancelSeek.Name = "btnCancelSeek"
        Me.btnCancelSeek.Size = New System.Drawing.Size(80, 23)
        Me.btnCancelSeek.TabIndex = 12
        Me.btnCancelSeek.Text = "Cancel Seek"
        Me.btnCancelSeek.UseVisualStyleBackColor = True
        '
        'btnRemoveGoal
        '
        Me.btnRemoveGoal.Location = New System.Drawing.Point(618, 171)
        Me.btnRemoveGoal.Name = "btnRemoveGoal"
        Me.btnRemoveGoal.Size = New System.Drawing.Size(80, 23)
        Me.btnRemoveGoal.TabIndex = 10
        Me.btnRemoveGoal.Text = "Remove Skill"
        Me.btnRemoveGoal.UseVisualStyleBackColor = True
        '
        'btnDecreaseLevel
        '
        Me.btnDecreaseLevel.Location = New System.Drawing.Point(618, 142)
        Me.btnDecreaseLevel.Name = "btnDecreaseLevel"
        Me.btnDecreaseLevel.Size = New System.Drawing.Size(80, 23)
        Me.btnDecreaseLevel.TabIndex = 9
        Me.btnDecreaseLevel.Text = "Level Down"
        Me.btnDecreaseLevel.UseVisualStyleBackColor = True
        '
        'btnClearGoals
        '
        Me.btnClearGoals.Location = New System.Drawing.Point(617, 200)
        Me.btnClearGoals.Name = "btnClearGoals"
        Me.btnClearGoals.Size = New System.Drawing.Size(81, 23)
        Me.btnClearGoals.TabIndex = 8
        Me.btnClearGoals.Text = "Clear All Skills"
        Me.btnClearGoals.UseVisualStyleBackColor = True
        '
        'btnIncreaseLevel
        '
        Me.btnIncreaseLevel.Location = New System.Drawing.Point(618, 113)
        Me.btnIncreaseLevel.Name = "btnIncreaseLevel"
        Me.btnIncreaseLevel.Size = New System.Drawing.Size(80, 23)
        Me.btnIncreaseLevel.TabIndex = 7
        Me.btnIncreaseLevel.Text = "Level Up"
        Me.btnIncreaseLevel.UseVisualStyleBackColor = True
        '
        'lblAvailableGoals
        '
        Me.lblAvailableGoals.AutoSize = True
        Me.lblAvailableGoals.Location = New System.Drawing.Point(17, 30)
        Me.lblAvailableGoals.Name = "lblAvailableGoals"
        Me.lblAvailableGoals.Size = New System.Drawing.Size(79, 13)
        Me.lblAvailableGoals.TabIndex = 6
        Me.lblAvailableGoals.Text = "Available Skills:"
        '
        'tvwSkillList
        '
        Me.tvwSkillList.ContextMenuStrip = Me.ctxAddSkills
        Me.tvwSkillList.Location = New System.Drawing.Point(20, 49)
        Me.tvwSkillList.Name = "tvwSkillList"
        Me.tvwSkillList.Size = New System.Drawing.Size(247, 318)
        Me.tvwSkillList.TabIndex = 5
        '
        'ctxAddSkills
        '
        Me.ctxAddSkills.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddSkillName, Me.ToolStripMenuItem1, Me.mnuAddLevel1, Me.mnuAddLevel2, Me.mnuAddLevel3, Me.mnuAddLevel4, Me.mnuAddLevel5})
        Me.ctxAddSkills.Name = "ctxAddSkills"
        Me.ctxAddSkills.Size = New System.Drawing.Size(175, 142)
        '
        'mnuAddSkillName
        '
        Me.mnuAddSkillName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuAddSkillName.Name = "mnuAddSkillName"
        Me.mnuAddSkillName.Size = New System.Drawing.Size(174, 22)
        Me.mnuAddSkillName.Text = "Skill Name"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(171, 6)
        '
        'mnuAddLevel1
        '
        Me.mnuAddLevel1.Name = "mnuAddLevel1"
        Me.mnuAddLevel1.Size = New System.Drawing.Size(174, 22)
        Me.mnuAddLevel1.Text = "Add Skill At Level 1"
        '
        'mnuAddLevel2
        '
        Me.mnuAddLevel2.Name = "mnuAddLevel2"
        Me.mnuAddLevel2.Size = New System.Drawing.Size(174, 22)
        Me.mnuAddLevel2.Text = "Add Skill At Level 2"
        '
        'mnuAddLevel3
        '
        Me.mnuAddLevel3.Name = "mnuAddLevel3"
        Me.mnuAddLevel3.Size = New System.Drawing.Size(174, 22)
        Me.mnuAddLevel3.Text = "Add Skill At Level 3"
        '
        'mnuAddLevel4
        '
        Me.mnuAddLevel4.Name = "mnuAddLevel4"
        Me.mnuAddLevel4.Size = New System.Drawing.Size(174, 22)
        Me.mnuAddLevel4.Text = "Add Skill At Level 4"
        '
        'mnuAddLevel5
        '
        Me.mnuAddLevel5.Name = "mnuAddLevel5"
        Me.mnuAddLevel5.Size = New System.Drawing.Size(174, 22)
        Me.mnuAddLevel5.Text = "Add Skill At Level 5"
        '
        'lblSelectedGoals
        '
        Me.lblSelectedGoals.AutoSize = True
        Me.lblSelectedGoals.Location = New System.Drawing.Point(279, 30)
        Me.lblSelectedGoals.Name = "lblSelectedGoals"
        Me.lblSelectedGoals.Size = New System.Drawing.Size(77, 13)
        Me.lblSelectedGoals.TabIndex = 4
        Me.lblSelectedGoals.Text = "Selected Skills:"
        '
        'lstChars
        '
        Me.lstChars.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstChars.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colCharacterNo, Me.colCharRace, Me.colCharBlood, Me.colCharAncestry, Me.colCharC, Me.colCharI, Me.colCharM, Me.colCharP, Me.colCharW, Me.colOTime, Me.colNTime})
        Me.lstChars.ContextMenuStrip = Me.ctxChars
        Me.lstChars.FullRowSelect = True
        Me.lstChars.GridLines = True
        Me.lstChars.Location = New System.Drawing.Point(20, 402)
        Me.lstChars.Name = "lstChars"
        Me.lstChars.ShowItemToolTips = True
        Me.lstChars.Size = New System.Drawing.Size(933, 247)
        Me.lstChars.TabIndex = 3
        Me.lstChars.UseCompatibleStateImageBehavior = False
        Me.lstChars.View = System.Windows.Forms.View.Details
        '
        'colCharacterNo
        '
        Me.colCharacterNo.Text = "No"
        Me.colCharacterNo.Width = 30
        '
        'colCharRace
        '
        Me.colCharRace.Text = "Race"
        '
        'colCharBlood
        '
        Me.colCharBlood.Text = "Bloodline"
        '
        'colCharAncestry
        '
        Me.colCharAncestry.Text = "Ancestry"
        Me.colCharAncestry.Width = 100
        '
        'colCharC
        '
        Me.colCharC.Text = "C"
        Me.colCharC.Width = 30
        '
        'colCharI
        '
        Me.colCharI.Text = "I"
        Me.colCharI.Width = 30
        '
        'colCharM
        '
        Me.colCharM.Text = "M"
        Me.colCharM.Width = 30
        '
        'colCharP
        '
        Me.colCharP.Text = "P"
        Me.colCharP.Width = 30
        '
        'colCharW
        '
        Me.colCharW.Text = "W"
        Me.colCharW.Width = 30
        '
        'colOTime
        '
        Me.colOTime.Text = "Std Queue Time"
        Me.colOTime.Width = 150
        '
        'colNTime
        '
        Me.colNTime.Text = "Opt Queue Time"
        Me.colNTime.Width = 150
        '
        'pbPilots
        '
        Me.pbPilots.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbPilots.Location = New System.Drawing.Point(20, 373)
        Me.pbPilots.Name = "pbPilots"
        Me.pbPilots.Size = New System.Drawing.Size(933, 23)
        Me.pbPilots.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbPilots.TabIndex = 2
        '
        'btnSeek
        '
        Me.btnSeek.Location = New System.Drawing.Point(618, 344)
        Me.btnSeek.Name = "btnSeek"
        Me.btnSeek.Size = New System.Drawing.Size(80, 23)
        Me.btnSeek.TabIndex = 1
        Me.btnSeek.Text = "Seek Goal"
        Me.btnSeek.UseVisualStyleBackColor = True
        '
        'lvwSkillSelection
        '
        Me.lvwSkillSelection.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSkillName, Me.colSkillLevel})
        Me.lvwSkillSelection.ContextMenuStrip = Me.ctxEditSkills
        Me.lvwSkillSelection.FullRowSelect = True
        Me.lvwSkillSelection.GridLines = True
        Me.lvwSkillSelection.HideSelection = False
        Me.lvwSkillSelection.Location = New System.Drawing.Point(273, 49)
        Me.lvwSkillSelection.Name = "lvwSkillSelection"
        Me.lvwSkillSelection.Size = New System.Drawing.Size(339, 318)
        Me.lvwSkillSelection.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSkillSelection.TabIndex = 0
        Me.lvwSkillSelection.UseCompatibleStateImageBehavior = False
        Me.lvwSkillSelection.View = System.Windows.Forms.View.Details
        '
        'colSkillName
        '
        Me.colSkillName.Text = "Skill Name"
        Me.colSkillName.Width = 240
        '
        'colSkillLevel
        '
        Me.colSkillLevel.Text = "Skill Level"
        Me.colSkillLevel.Width = 70
        '
        'ctxEditSkills
        '
        Me.ctxEditSkills.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEditSkillName, Me.ToolStripSeparator1, Me.mnuEditLevel1, Me.mnuEditLevel2, Me.mnuEditLevel3, Me.mnuEditLevel4, Me.mnuEditLevel5, Me.ToolStripMenuItem2, Me.mnuDeleteSkill})
        Me.ctxEditSkills.Name = "ctxAddSkills"
        Me.ctxEditSkills.Size = New System.Drawing.Size(169, 170)
        '
        'mnuEditSkillName
        '
        Me.mnuEditSkillName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuEditSkillName.Name = "mnuEditSkillName"
        Me.mnuEditSkillName.Size = New System.Drawing.Size(168, 22)
        Me.mnuEditSkillName.Text = "Skill Name"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(165, 6)
        '
        'mnuEditLevel1
        '
        Me.mnuEditLevel1.Name = "mnuEditLevel1"
        Me.mnuEditLevel1.Size = New System.Drawing.Size(168, 22)
        Me.mnuEditLevel1.Text = "Set Skill At Level 1"
        '
        'mnuEditLevel2
        '
        Me.mnuEditLevel2.Name = "mnuEditLevel2"
        Me.mnuEditLevel2.Size = New System.Drawing.Size(168, 22)
        Me.mnuEditLevel2.Text = "Set Skill At Level 2"
        '
        'mnuEditLevel3
        '
        Me.mnuEditLevel3.Name = "mnuEditLevel3"
        Me.mnuEditLevel3.Size = New System.Drawing.Size(168, 22)
        Me.mnuEditLevel3.Text = "Set Skill At Level 3"
        '
        'mnuEditLevel4
        '
        Me.mnuEditLevel4.Name = "mnuEditLevel4"
        Me.mnuEditLevel4.Size = New System.Drawing.Size(168, 22)
        Me.mnuEditLevel4.Text = "Set Skill At Level 4"
        '
        'mnuEditLevel5
        '
        Me.mnuEditLevel5.Name = "mnuEditLevel5"
        Me.mnuEditLevel5.Size = New System.Drawing.Size(168, 22)
        Me.mnuEditLevel5.Text = "Set Skill At Level 5"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(165, 6)
        '
        'mnuDeleteSkill
        '
        Me.mnuDeleteSkill.Name = "mnuDeleteSkill"
        Me.mnuDeleteSkill.Size = New System.Drawing.Size(168, 22)
        Me.mnuDeleteSkill.Text = "Delete Skill"
        '
        'frmCharCreate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(969, 683)
        Me.Controls.Add(Me.tabCreation)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCharCreate"
        Me.Text = "Character Creation Tool"
        Me.tabCreation.ResumeLayout(False)
        Me.tabSelection.ResumeLayout(False)
        Me.tabSelection.PerformLayout()
        Me.gbSkills.ResumeLayout(False)
        Me.gbAttributes.ResumeLayout(False)
        Me.gbAttributes.PerformLayout()
        Me.tabVariations.ResumeLayout(False)
        Me.ctxChars.ResumeLayout(False)
        Me.tabStartSkills.ResumeLayout(False)
        Me.tabStartSkills.PerformLayout()
        Me.tabGoalSeek.ResumeLayout(False)
        Me.tabGoalSeek.PerformLayout()
        Me.ctxAddSkills.ResumeLayout(False)
        Me.ctxEditSkills.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabCreation As System.Windows.Forms.TabControl
    Friend WithEvents tabSelection As System.Windows.Forms.TabPage
    Friend WithEvents tabVariations As System.Windows.Forms.TabPage
    Friend WithEvents gbSkills As System.Windows.Forms.GroupBox
    Friend WithEvents lvwSkills As System.Windows.Forms.ListView
    Friend WithEvents colSkill As System.Windows.Forms.ColumnHeader
    Friend WithEvents colLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSP As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbAttributes As System.Windows.Forms.GroupBox
    Friend WithEvents lblSP As System.Windows.Forms.Label
    Friend WithEvents lblMem As System.Windows.Forms.Label
    Friend WithEvents lblWil As System.Windows.Forms.Label
    Friend WithEvents lblPer As System.Windows.Forms.Label
    Friend WithEvents lblCha As System.Windows.Forms.Label
    Friend WithEvents lblInt As System.Windows.Forms.Label
    Friend WithEvents cboAncestry As System.Windows.Forms.ComboBox
    Friend WithEvents lblStep3 As System.Windows.Forms.Label
    Friend WithEvents cboBloodline As System.Windows.Forms.ComboBox
    Friend WithEvents lblStep2 As System.Windows.Forms.Label
    Friend WithEvents cboRace As System.Windows.Forms.ComboBox
    Friend WithEvents lblStep1 As System.Windows.Forms.Label
    Friend WithEvents lvwChars As System.Windows.Forms.ListView
    Friend WithEvents colNo As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRace As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBlood As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAncestry As System.Windows.Forms.ColumnHeader
    Friend WithEvents colC As System.Windows.Forms.ColumnHeader
    Friend WithEvents colI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colM As System.Windows.Forms.ColumnHeader
    Friend WithEvents colP As System.Windows.Forms.ColumnHeader
    Friend WithEvents colW As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSkillpoints As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabStartSkills As System.Windows.Forms.TabPage
    Friend WithEvents lstStartSkills As System.Windows.Forms.ListBox
    Friend WithEvents lvwChars2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblSelectedSkills As System.Windows.Forms.Label
    Friend WithEvents btnAddPilot As System.Windows.Forms.Button
    Friend WithEvents ctxChars As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddCharacter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabGoalSeek As System.Windows.Forms.TabPage
    Friend WithEvents lvwSkillSelection As System.Windows.Forms.ListView
    Friend WithEvents colSkillName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSkillLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnSeek As System.Windows.Forms.Button
    Friend WithEvents pbPilots As System.Windows.Forms.ProgressBar
    Friend WithEvents lstChars As System.Windows.Forms.ListView
    Friend WithEvents colCharacterNo As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCharRace As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCharBlood As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCharAncestry As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCharC As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCharI As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCharM As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCharP As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCharW As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents tvwSkillList As System.Windows.Forms.TreeView
    Friend WithEvents lblSelectedGoals As System.Windows.Forms.Label
    Friend WithEvents lblAvailableGoals As System.Windows.Forms.Label
    Friend WithEvents btnRemoveGoal As System.Windows.Forms.Button
    Friend WithEvents btnDecreaseLevel As System.Windows.Forms.Button
    Friend WithEvents btnClearGoals As System.Windows.Forms.Button
    Friend WithEvents btnIncreaseLevel As System.Windows.Forms.Button
    Friend WithEvents btnCancelSeek As System.Windows.Forms.Button
    Friend WithEvents ctxAddSkills As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddLevel1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddLevel2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddLevel3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddLevel4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddLevel5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ctxEditSkills As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuEditSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuEditLevel1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditLevel2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditLevel3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditLevel4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditLevel5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuDeleteSkill As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExportToCSV As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sfd1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lblMethod As System.Windows.Forms.Label
    Friend WithEvents cboMethod As System.Windows.Forms.ComboBox
End Class
