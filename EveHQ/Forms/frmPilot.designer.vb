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
        Me.lblImplants = New System.Windows.Forms.Label
        Me.btnCharXML = New System.Windows.Forms.Button
        Me.btnTrainingXML = New System.Windows.Forms.Button
        Me.lblTraining = New System.Windows.Forms.Label
        Me.lblSkills = New System.Windows.Forms.Label
        Me.lvAttributes = New System.Windows.Forms.ListView
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.lblAttributes = New System.Windows.Forms.Label
        Me.chkManualImplants = New System.Windows.Forms.CheckBox
        Me.btnEditImplants = New System.Windows.Forms.Button
        Me.chkGroupSkills = New System.Windows.Forms.CheckBox
        Me.tcSkills = New System.Windows.Forms.TabControl
        Me.tabSkills = New System.Windows.Forms.TabPage
        Me.clvSkills = New DotNetLib.Windows.Forms.ContainerListView
        Me.ContainerListViewColumnHeader1 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader2 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader3 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader4 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader5 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader6 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.tabCerts = New System.Windows.Forms.TabPage
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.clvCerts = New DotNetLib.Windows.Forms.ContainerListView
        Me.colCertificate = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colCertGrade = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colCertLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ctxCerts = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCertName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewCertDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.tabSkillQueue = New System.Windows.Forms.TabPage
        Me.clvQueue = New DotNetLib.Windows.Forms.ContainerListView
        Me.colSkillName = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colToLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colStartTime = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colEndTime = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lblPilot = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lvPilot = New EveHQ.ListViewNoFlicker
        Me.Category = New System.Windows.Forms.ColumnHeader
        Me.Data = New System.Windows.Forms.ColumnHeader
        Me.lvTraining = New EveHQ.ListViewNoFlicker
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ctxSkills.SuspendLayout()
        CType(Me.picPilot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxPic.SuspendLayout()
        Me.tcSkills.SuspendLayout()
        Me.tabSkills.SuspendLayout()
        Me.tabCerts.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ctxCerts.SuspendLayout()
        Me.tabSkillQueue.SuspendLayout()
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
        Me.picPilot.Location = New System.Drawing.Point(8, 50)
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
        Me.mnuSavePortrait.Text = "Save Portrait into Image Cache"
        '
        'lvImplants
        '
        Me.lvImplants.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lvImplants.FullRowSelect = True
        Me.lvImplants.GridLines = True
        Me.lvImplants.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvImplants.Location = New System.Drawing.Point(8, 337)
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
        'lblImplants
        '
        Me.lblImplants.AutoSize = True
        Me.lblImplants.Location = New System.Drawing.Point(5, 321)
        Me.lblImplants.Name = "lblImplants"
        Me.lblImplants.Size = New System.Drawing.Size(52, 13)
        Me.lblImplants.TabIndex = 24
        Me.lblImplants.Text = "Implants:"
        '
        'btnCharXML
        '
        Me.btnCharXML.Image = CType(resources.GetObject("btnCharXML.Image"), System.Drawing.Image)
        Me.btnCharXML.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCharXML.Location = New System.Drawing.Point(8, 513)
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
        Me.btnTrainingXML.Location = New System.Drawing.Point(8, 541)
        Me.btnTrainingXML.Name = "btnTrainingXML"
        Me.btnTrainingXML.Size = New System.Drawing.Size(98, 22)
        Me.btnTrainingXML.TabIndex = 28
        Me.btnTrainingXML.Text = "Training"
        Me.btnTrainingXML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTraining
        '
        Me.lblTraining.AutoSize = True
        Me.lblTraining.Location = New System.Drawing.Point(151, 183)
        Me.lblTraining.Name = "lblTraining"
        Me.lblTraining.Size = New System.Drawing.Size(69, 13)
        Me.lblTraining.TabIndex = 26
        Me.lblTraining.Text = "Skill Training:"
        '
        'lblSkills
        '
        Me.lblSkills.AutoSize = True
        Me.lblSkills.Location = New System.Drawing.Point(151, 321)
        Me.lblSkills.Name = "lblSkills"
        Me.lblSkills.Size = New System.Drawing.Size(92, 13)
        Me.lblSkills.TabIndex = 30
        Me.lblSkills.Text = "Skills/Certificates:"
        '
        'lvAttributes
        '
        Me.lvAttributes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.ColumnHeader7})
        Me.lvAttributes.FullRowSelect = True
        Me.lvAttributes.GridLines = True
        Me.lvAttributes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvAttributes.Location = New System.Drawing.Point(8, 199)
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
        'lblAttributes
        '
        Me.lblAttributes.AutoSize = True
        Me.lblAttributes.Location = New System.Drawing.Point(7, 183)
        Me.lblAttributes.Name = "lblAttributes"
        Me.lblAttributes.Size = New System.Drawing.Size(59, 13)
        Me.lblAttributes.TabIndex = 32
        Me.lblAttributes.Text = "Attributes:"
        '
        'chkManualImplants
        '
        Me.chkManualImplants.Location = New System.Drawing.Point(8, 458)
        Me.chkManualImplants.Name = "chkManualImplants"
        Me.chkManualImplants.Size = New System.Drawing.Size(84, 30)
        Me.chkManualImplants.TabIndex = 33
        Me.chkManualImplants.Text = "Manual Implants"
        Me.chkManualImplants.UseVisualStyleBackColor = True
        '
        'btnEditImplants
        '
        Me.btnEditImplants.Enabled = False
        Me.btnEditImplants.Location = New System.Drawing.Point(85, 458)
        Me.btnEditImplants.Name = "btnEditImplants"
        Me.btnEditImplants.Size = New System.Drawing.Size(64, 34)
        Me.btnEditImplants.TabIndex = 34
        Me.btnEditImplants.Text = "Edit Implants"
        Me.btnEditImplants.UseVisualStyleBackColor = True
        '
        'chkGroupSkills
        '
        Me.chkGroupSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkGroupSkills.AutoSize = True
        Me.chkGroupSkills.Checked = True
        Me.chkGroupSkills.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGroupSkills.Location = New System.Drawing.Point(750, 320)
        Me.chkGroupSkills.Name = "chkGroupSkills"
        Me.chkGroupSkills.Size = New System.Drawing.Size(139, 17)
        Me.chkGroupSkills.TabIndex = 38
        Me.chkGroupSkills.Text = "Group Skills/Certificates"
        Me.chkGroupSkills.UseVisualStyleBackColor = True
        '
        'tcSkills
        '
        Me.tcSkills.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcSkills.Controls.Add(Me.tabSkills)
        Me.tcSkills.Controls.Add(Me.tabCerts)
        Me.tcSkills.Controls.Add(Me.tabSkillQueue)
        Me.tcSkills.Location = New System.Drawing.Point(156, 337)
        Me.tcSkills.Name = "tcSkills"
        Me.tcSkills.SelectedIndex = 0
        Me.tcSkills.ShowToolTips = True
        Me.tcSkills.Size = New System.Drawing.Size(733, 301)
        Me.tcSkills.TabIndex = 39
        '
        'tabSkills
        '
        Me.tabSkills.Controls.Add(Me.clvSkills)
        Me.tabSkills.Location = New System.Drawing.Point(4, 22)
        Me.tabSkills.Name = "tabSkills"
        Me.tabSkills.Size = New System.Drawing.Size(725, 275)
        Me.tabSkills.TabIndex = 0
        Me.tabSkills.Text = "Skills"
        Me.tabSkills.ToolTipText = "Shows the pilots skills with levels and skillpoints"
        Me.tabSkills.UseVisualStyleBackColor = True
        '
        'clvSkills
        '
        Me.clvSkills.AllowColumnResize = False
        Me.clvSkills.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.ContainerListViewColumnHeader1, Me.ContainerListViewColumnHeader2, Me.ContainerListViewColumnHeader3, Me.ContainerListViewColumnHeader4, Me.ContainerListViewColumnHeader5, Me.ContainerListViewColumnHeader6})
        Me.clvSkills.DefaultItemHeight = 18
        Me.clvSkills.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvSkills.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvSkills.ItemContextMenu = Me.ctxSkills
        Me.clvSkills.Location = New System.Drawing.Point(0, 0)
        Me.clvSkills.Name = "clvSkills"
        Me.clvSkills.Size = New System.Drawing.Size(725, 275)
        Me.clvSkills.TabIndex = 37
        '
        'ContainerListViewColumnHeader1
        '
        Me.ContainerListViewColumnHeader1.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader1.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.ContainerListViewColumnHeader1.Tag = Nothing
        Me.ContainerListViewColumnHeader1.Text = "Skill"
        Me.ContainerListViewColumnHeader1.Width = 280
        '
        'ContainerListViewColumnHeader2
        '
        Me.ContainerListViewColumnHeader2.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ContainerListViewColumnHeader2.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader2.DisplayIndex = 1
        Me.ContainerListViewColumnHeader2.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.ContainerListViewColumnHeader2.Tag = Nothing
        Me.ContainerListViewColumnHeader2.Text = "Rank"
        Me.ContainerListViewColumnHeader2.Width = 60
        '
        'ContainerListViewColumnHeader3
        '
        Me.ContainerListViewColumnHeader3.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ContainerListViewColumnHeader3.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader3.DisplayIndex = 2
        Me.ContainerListViewColumnHeader3.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.ContainerListViewColumnHeader3.Tag = Nothing
        Me.ContainerListViewColumnHeader3.Text = "Level"
        Me.ContainerListViewColumnHeader3.Width = 60
        '
        'ContainerListViewColumnHeader4
        '
        Me.ContainerListViewColumnHeader4.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ContainerListViewColumnHeader4.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader4.DisplayIndex = 3
        Me.ContainerListViewColumnHeader4.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.ContainerListViewColumnHeader4.Tag = Nothing
        Me.ContainerListViewColumnHeader4.Text = "% Done"
        Me.ContainerListViewColumnHeader4.Width = 70
        '
        'ContainerListViewColumnHeader5
        '
        Me.ContainerListViewColumnHeader5.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader5.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader5.DisplayIndex = 4
        Me.ContainerListViewColumnHeader5.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader5.Tag = Nothing
        Me.ContainerListViewColumnHeader5.Text = "Skillpoints"
        Me.ContainerListViewColumnHeader5.Width = 100
        '
        'ContainerListViewColumnHeader6
        '
        Me.ContainerListViewColumnHeader6.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader6.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader6.DisplayIndex = 5
        Me.ContainerListViewColumnHeader6.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.ContainerListViewColumnHeader6.Tag = Nothing
        Me.ContainerListViewColumnHeader6.Text = "Time To Level Up"
        Me.ContainerListViewColumnHeader6.Width = 125
        '
        'tabCerts
        '
        Me.tabCerts.Controls.Add(Me.Panel1)
        Me.tabCerts.Location = New System.Drawing.Point(4, 22)
        Me.tabCerts.Name = "tabCerts"
        Me.tabCerts.Size = New System.Drawing.Size(725, 275)
        Me.tabCerts.TabIndex = 1
        Me.tabCerts.Text = "Certificates"
        Me.tabCerts.ToolTipText = "Shows the pilot's claimed certificates"
        Me.tabCerts.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.clvCerts)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(725, 275)
        Me.Panel1.TabIndex = 1
        '
        'clvCerts
        '
        Me.clvCerts.AllowColumnResize = False
        Me.clvCerts.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colCertificate, Me.colCertGrade, Me.colCertLevel})
        Me.clvCerts.DefaultItemHeight = 18
        Me.clvCerts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvCerts.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvCerts.ItemContextMenu = Me.ctxCerts
        Me.clvCerts.Location = New System.Drawing.Point(0, 0)
        Me.clvCerts.MultipleColumnSort = True
        Me.clvCerts.Name = "clvCerts"
        Me.clvCerts.Size = New System.Drawing.Size(725, 275)
        Me.clvCerts.TabIndex = 50
        '
        'colCertificate
        '
        Me.colCertificate.CustomSortTag = Nothing
        Me.colCertificate.MinimumWidth = 50
        Me.colCertificate.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colCertificate.Tag = Nothing
        Me.colCertificate.Text = "Certificate"
        Me.colCertificate.Width = 400
        '
        'colCertGrade
        '
        Me.colCertGrade.CustomSortTag = Nothing
        Me.colCertGrade.DisplayIndex = 1
        Me.colCertGrade.MinimumWidth = 50
        Me.colCertGrade.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.colCertGrade.Tag = Nothing
        Me.colCertGrade.Text = "Grade"
        Me.colCertGrade.Width = 100
        '
        'colCertLevel
        '
        Me.colCertLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colCertLevel.CustomSortTag = Nothing
        Me.colCertLevel.DisplayIndex = 2
        Me.colCertLevel.MinimumWidth = 50
        Me.colCertLevel.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.colCertLevel.Tag = Nothing
        Me.colCertLevel.Text = "Level"
        Me.colCertLevel.Width = 100
        '
        'ctxCerts
        '
        Me.ctxCerts.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCertName, Me.ToolStripSeparator2, Me.mnuViewCertDetails})
        Me.ctxCerts.Name = "ctxSkills"
        Me.ctxCerts.Size = New System.Drawing.Size(138, 54)
        '
        'mnuCertName
        '
        Me.mnuCertName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.mnuCertName.Name = "mnuCertName"
        Me.mnuCertName.Size = New System.Drawing.Size(137, 22)
        Me.mnuCertName.Text = "Skill Name"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(134, 6)
        '
        'mnuViewCertDetails
        '
        Me.mnuViewCertDetails.Name = "mnuViewCertDetails"
        Me.mnuViewCertDetails.Size = New System.Drawing.Size(137, 22)
        Me.mnuViewCertDetails.Text = "View Details"
        '
        'tabSkillQueue
        '
        Me.tabSkillQueue.Controls.Add(Me.clvQueue)
        Me.tabSkillQueue.Location = New System.Drawing.Point(4, 22)
        Me.tabSkillQueue.Name = "tabSkillQueue"
        Me.tabSkillQueue.Size = New System.Drawing.Size(725, 275)
        Me.tabSkillQueue.TabIndex = 2
        Me.tabSkillQueue.Text = "Eve Skill Queue"
        Me.tabSkillQueue.ToolTipText = "Shows the current in-game skill queue"
        Me.tabSkillQueue.UseVisualStyleBackColor = True
        '
        'clvQueue
        '
        Me.clvQueue.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colSkillName, Me.colToLevel, Me.colStartTime, Me.colEndTime})
        Me.clvQueue.DefaultItemHeight = 18
        Me.clvQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvQueue.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvQueue.ItemContextMenu = Me.ctxCerts
        Me.clvQueue.Location = New System.Drawing.Point(0, 0)
        Me.clvQueue.Name = "clvQueue"
        Me.clvQueue.Size = New System.Drawing.Size(725, 275)
        Me.clvQueue.TabIndex = 51
        '
        'colSkillName
        '
        Me.colSkillName.CustomSortTag = Nothing
        Me.colSkillName.MinimumWidth = 50
        Me.colSkillName.Tag = Nothing
        Me.colSkillName.Text = "Skill Name"
        Me.colSkillName.Width = 300
        '
        'colToLevel
        '
        Me.colToLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colToLevel.CustomSortTag = Nothing
        Me.colToLevel.DisplayIndex = 1
        Me.colToLevel.MinimumWidth = 50
        Me.colToLevel.Tag = Nothing
        Me.colToLevel.Text = "Level"
        Me.colToLevel.Width = 60
        '
        'colStartTime
        '
        Me.colStartTime.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colStartTime.CustomSortTag = Nothing
        Me.colStartTime.DisplayIndex = 2
        Me.colStartTime.MinimumWidth = 50
        Me.colStartTime.Tag = Nothing
        Me.colStartTime.Text = "Start Time"
        Me.colStartTime.Width = 150
        Me.colStartTime.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'colEndTime
        '
        Me.colEndTime.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colEndTime.CustomSortTag = Nothing
        Me.colEndTime.DisplayIndex = 3
        Me.colEndTime.Tag = Nothing
        Me.colEndTime.Text = "End Time"
        Me.colEndTime.Width = 150
        Me.colEndTime.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(7, 14)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 40
        Me.lblPilot.Text = "Pilot:"
        '
        'cboPilots
        '
        Me.cboPilots.DropDownHeight = 250
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.IntegralHeight = False
        Me.cboPilots.Location = New System.Drawing.Point(43, 11)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(175, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 41
        '
        'lvPilot
        '
        Me.lvPilot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvPilot.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Category, Me.Data})
        Me.lvPilot.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvPilot.FullRowSelect = True
        Me.lvPilot.GridLines = True
        Me.lvPilot.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvPilot.Location = New System.Drawing.Point(152, 50)
        Me.lvPilot.MultiSelect = False
        Me.lvPilot.Name = "lvPilot"
        Me.lvPilot.Size = New System.Drawing.Size(737, 128)
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
        'lvTraining
        '
        Me.lvTraining.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvTraining.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader4, Me.ColumnHeader5})
        Me.lvTraining.FullRowSelect = True
        Me.lvTraining.GridLines = True
        Me.lvTraining.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvTraining.Location = New System.Drawing.Point(152, 199)
        Me.lvTraining.MultiSelect = False
        Me.lvTraining.Name = "lvTraining"
        Me.lvTraining.Size = New System.Drawing.Size(737, 114)
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
        'frmPilot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(901, 643)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblPilot)
        Me.Controls.Add(Me.tcSkills)
        Me.Controls.Add(Me.chkGroupSkills)
        Me.Controls.Add(Me.btnEditImplants)
        Me.Controls.Add(Me.lvPilot)
        Me.Controls.Add(Me.lvTraining)
        Me.Controls.Add(Me.lblAttributes)
        Me.Controls.Add(Me.chkManualImplants)
        Me.Controls.Add(Me.btnTrainingXML)
        Me.Controls.Add(Me.lvAttributes)
        Me.Controls.Add(Me.btnCharXML)
        Me.Controls.Add(Me.lblTraining)
        Me.Controls.Add(Me.lblSkills)
        Me.Controls.Add(Me.picPilot)
        Me.Controls.Add(Me.lblImplants)
        Me.Controls.Add(Me.lvImplants)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPilot"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EveHQ Pilot Data"
        Me.ctxSkills.ResumeLayout(False)
        CType(Me.picPilot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxPic.ResumeLayout(False)
        Me.tcSkills.ResumeLayout(False)
        Me.tabSkills.ResumeLayout(False)
        Me.tabCerts.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ctxCerts.ResumeLayout(False)
        Me.tabSkillQueue.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picPilot As System.Windows.Forms.PictureBox
    Friend WithEvents lvPilot As EveHQ.ListViewNoFlicker
    Friend WithEvents Category As System.Windows.Forms.ColumnHeader
    Friend WithEvents Data As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvImplants As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblImplants As System.Windows.Forms.Label
    Friend WithEvents btnCharXML As System.Windows.Forms.Button
    Friend WithEvents btnTrainingXML As System.Windows.Forms.Button
    Friend WithEvents lblTraining As System.Windows.Forms.Label
    Friend WithEvents lblSkills As System.Windows.Forms.Label
    Friend WithEvents lvAttributes As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblAttributes As System.Windows.Forms.Label
    Friend WithEvents lvTraining As EveHQ.ListViewNoFlicker
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxSkills As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkManualImplants As System.Windows.Forms.CheckBox
    Friend WithEvents btnEditImplants As System.Windows.Forms.Button
    Friend WithEvents ctxPic As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCtxPicGetPortraitFromServer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCtxPicGetPortraitFromLocal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSavePortrait As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuForceTraining As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents clvSkills As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents ContainerListViewColumnHeader1 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader2 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader3 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader4 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader5 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader6 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents chkGroupSkills As System.Windows.Forms.CheckBox
    Friend WithEvents tcSkills As System.Windows.Forms.TabControl
    Friend WithEvents tabSkills As System.Windows.Forms.TabPage
    Friend WithEvents tabCerts As System.Windows.Forms.TabPage
    Friend WithEvents clvCerts As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colCertificate As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colCertGrade As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colCertLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ctxCerts As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCertName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewCertDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabSkillQueue As System.Windows.Forms.TabPage
    Friend WithEvents clvQueue As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colSkillName As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colToLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colStartTime As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colEndTime As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox

End Class
