<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemBrowser
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemBrowser))
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Containers", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Materials", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Accessories", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Ships", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Modules", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup6 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Charges", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup7 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Blueprints", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup8 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Skills", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup9 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Drones", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup10 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Implants", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup11 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Mobile Disruptors", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup12 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("POS Equipment", System.Windows.Forms.HorizontalAlignment.Left)
        Me.ctxSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuViewDetails = New System.Windows.Forms.ToolStripMenuItem
        Me.ssData = New System.Windows.Forms.StatusStrip
        Me.ssLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.ssDBLocation = New System.Windows.Forms.ToolStripStatusLabel
        Me.ssLblID = New System.Windows.Forms.ToolStripStatusLabel
        Me.ItemToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.picBP = New System.Windows.Forms.PictureBox
        Me.tabMaterials = New System.Windows.Forms.TabPage
        Me.tabMaterial = New System.Windows.Forms.TabControl
        Me.tabM1 = New System.Windows.Forms.TabPage
        Me.lblMELevel = New System.Windows.Forms.Label
        Me.nudMELevel = New System.Windows.Forms.NumericUpDown
        Me.lstM1 = New System.Windows.Forms.ListView
        Me.colM1M = New System.Windows.Forms.ColumnHeader
        Me.colM1Q = New System.Windows.Forms.ColumnHeader
        Me.colM1ME = New System.Windows.Forms.ColumnHeader
        Me.colM1P = New System.Windows.Forms.ColumnHeader
        Me.tabM2 = New System.Windows.Forms.TabPage
        Me.lstM2 = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.tabM3 = New System.Windows.Forms.TabPage
        Me.lstM3 = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.tabM4 = New System.Windows.Forms.TabPage
        Me.lstM4 = New System.Windows.Forms.ListView
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.tabM5 = New System.Windows.Forms.TabPage
        Me.lstM5 = New System.Windows.Forms.ListView
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.tabM6 = New System.Windows.Forms.TabPage
        Me.lstM6 = New System.Windows.Forms.ListView
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.tabM7 = New System.Windows.Forms.TabPage
        Me.lstM7 = New System.Windows.Forms.ListView
        Me.ColumnHeader13 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader14 = New System.Windows.Forms.ColumnHeader
        Me.tabM8 = New System.Windows.Forms.TabPage
        Me.lstM8 = New System.Windows.Forms.ListView
        Me.ColumnHeader15 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader16 = New System.Windows.Forms.ColumnHeader
        Me.tabM9 = New System.Windows.Forms.TabPage
        Me.lstM9 = New System.Windows.Forms.ListView
        Me.ColumnHeader17 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader18 = New System.Windows.Forms.ColumnHeader
        Me.tabVariations = New System.Windows.Forms.TabPage
        Me.tabVariation = New System.Windows.Forms.TabControl
        Me.tabMetaVariations = New System.Windows.Forms.TabPage
        Me.lstVariations = New System.Windows.Forms.ListView
        Me.colTypeName = New System.Windows.Forms.ColumnHeader
        Me.colMetaTypeName = New System.Windows.Forms.ColumnHeader
        Me.tabComparisons = New System.Windows.Forms.TabPage
        Me.chkShowAllColumns = New System.Windows.Forms.CheckBox
        Me.lstComparisons = New System.Windows.Forms.ListView
        Me.ColumnHeader39 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader40 = New System.Windows.Forms.ColumnHeader
        Me.tabFitting = New System.Windows.Forms.TabPage
        Me.lstFitting = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.tabSkills = New System.Windows.Forms.TabPage
        Me.btnViewSkills = New System.Windows.Forms.Button
        Me.btnAddSkills = New System.Windows.Forms.Button
        Me.tvwReqs = New System.Windows.Forms.TreeView
        Me.tabAttributes = New System.Windows.Forms.TabPage
        Me.lstAttributes = New System.Windows.Forms.ListView
        Me.colAttribute = New System.Windows.Forms.ColumnHeader
        Me.colData = New System.Windows.Forms.ColumnHeader
        Me.tabDescription = New System.Windows.Forms.TabPage
        Me.lblDescription = New System.Windows.Forms.Label
        Me.tabItem = New System.Windows.Forms.TabControl
        Me.tabRecommended = New System.Windows.Forms.TabPage
        Me.lvwRecommended = New System.Windows.Forms.ListView
        Me.ColumnHeader59 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader60 = New System.Windows.Forms.ColumnHeader
        Me.imgListCerts = New System.Windows.Forms.ImageList(Me.components)
        Me.tabComponent = New System.Windows.Forms.TabPage
        Me.tabComponents = New System.Windows.Forms.TabControl
        Me.tabC1 = New System.Windows.Forms.TabPage
        Me.lblMELevelC = New System.Windows.Forms.Label
        Me.nudMELevelC = New System.Windows.Forms.NumericUpDown
        Me.lstC1 = New System.Windows.Forms.ListView
        Me.colC1M = New System.Windows.Forms.ColumnHeader
        Me.colC1Q = New System.Windows.Forms.ColumnHeader
        Me.colC1ME = New System.Windows.Forms.ColumnHeader
        Me.colC1P = New System.Windows.Forms.ColumnHeader
        Me.tabC2 = New System.Windows.Forms.TabPage
        Me.lstC2 = New System.Windows.Forms.ListView
        Me.ColumnHeader43 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader44 = New System.Windows.Forms.ColumnHeader
        Me.tabC3 = New System.Windows.Forms.TabPage
        Me.lstC3 = New System.Windows.Forms.ListView
        Me.ColumnHeader45 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader46 = New System.Windows.Forms.ColumnHeader
        Me.tabC4 = New System.Windows.Forms.TabPage
        Me.lstC4 = New System.Windows.Forms.ListView
        Me.ColumnHeader47 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader48 = New System.Windows.Forms.ColumnHeader
        Me.tabC5 = New System.Windows.Forms.TabPage
        Me.lstC5 = New System.Windows.Forms.ListView
        Me.ColumnHeader49 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader50 = New System.Windows.Forms.ColumnHeader
        Me.tabC6 = New System.Windows.Forms.TabPage
        Me.lstC6 = New System.Windows.Forms.ListView
        Me.ColumnHeader51 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader52 = New System.Windows.Forms.ColumnHeader
        Me.tabC7 = New System.Windows.Forms.TabPage
        Me.lstC7 = New System.Windows.Forms.ListView
        Me.ColumnHeader53 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader54 = New System.Windows.Forms.ColumnHeader
        Me.tabC8 = New System.Windows.Forms.TabPage
        Me.lstC8 = New System.Windows.Forms.ListView
        Me.ColumnHeader55 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader56 = New System.Windows.Forms.ColumnHeader
        Me.tabC9 = New System.Windows.Forms.TabPage
        Me.lstC9 = New System.Windows.Forms.ListView
        Me.ColumnHeader57 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader58 = New System.Windows.Forms.ColumnHeader
        Me.tabDepends = New System.Windows.Forms.TabPage
        Me.lvwDepend = New System.Windows.Forms.ListView
        Me.NeededFor = New System.Windows.Forms.ColumnHeader
        Me.NeededGroup = New System.Windows.Forms.ColumnHeader
        Me.NeededLevel = New System.Windows.Forms.ColumnHeader
        Me.tabEveCentral = New System.Windows.Forms.TabPage
        Me.lstEveCentral = New System.Windows.Forms.ListView
        Me.ColumnHeader41 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader42 = New System.Windows.Forms.ColumnHeader
        Me.tabInsurance = New System.Windows.Forms.TabPage
        Me.lstInsurance = New System.Windows.Forms.ListView
        Me.Type = New System.Windows.Forms.ColumnHeader
        Me.Fee = New System.Windows.Forms.ColumnHeader
        Me.Payout = New System.Windows.Forms.ColumnHeader
        Me.MarketPrice = New System.Windows.Forms.ColumnHeader
        Me.PayoutProfit = New System.Windows.Forms.ColumnHeader
        Me.picItem = New System.Windows.Forms.PictureBox
        Me.lblItem = New System.Windows.Forms.Label
        Me.lblUsable = New System.Windows.Forms.Label
        Me.tabBrowser = New System.Windows.Forms.TabControl
        Me.tabBrowse = New System.Windows.Forms.TabPage
        Me.chkBrowseNonPublished = New System.Windows.Forms.CheckBox
        Me.tvwBrowse = New System.Windows.Forms.TreeView
        Me.tabSearch = New System.Windows.Forms.TabPage
        Me.lstSearch = New System.Windows.Forms.ListBox
        Me.lblSearchCount = New System.Windows.Forms.Label
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.lblSearch = New System.Windows.Forms.Label
        Me.tabAttSearch = New System.Windows.Forms.TabPage
        Me.lstAttSearch = New System.Windows.Forms.ListView
        Me.colAttName = New System.Windows.Forms.ColumnHeader
        Me.colAttValue = New System.Windows.Forms.ColumnHeader
        Me.cboAttSearch = New System.Windows.Forms.ComboBox
        Me.lblAttSearchCount = New System.Windows.Forms.Label
        Me.lblAttSearch = New System.Windows.Forms.Label
        Me.tabWantedList = New System.Windows.Forms.TabPage
        Me.btnRefreshWantedList = New System.Windows.Forms.Button
        Me.btnClearWantedList = New System.Windows.Forms.Button
        Me.btnRemoveWantedItem = New System.Windows.Forms.Button
        Me.lvwWanted = New System.Windows.Forms.ListView
        Me.colWantedName = New System.Windows.Forms.ColumnHeader
        Me.colWantedPrice = New System.Windows.Forms.ColumnHeader
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader19 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader20 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader21 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader22 = New System.Windows.Forms.ColumnHeader
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.ListView2 = New System.Windows.Forms.ListView
        Me.ColumnHeader23 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader24 = New System.Windows.Forms.ColumnHeader
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.ListView3 = New System.Windows.Forms.ListView
        Me.ColumnHeader25 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader26 = New System.Windows.Forms.ColumnHeader
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.ListView4 = New System.Windows.Forms.ListView
        Me.ColumnHeader27 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader28 = New System.Windows.Forms.ColumnHeader
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.ListView5 = New System.Windows.Forms.ListView
        Me.ColumnHeader29 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader30 = New System.Windows.Forms.ColumnHeader
        Me.TabPage7 = New System.Windows.Forms.TabPage
        Me.ListView6 = New System.Windows.Forms.ListView
        Me.ColumnHeader31 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader32 = New System.Windows.Forms.ColumnHeader
        Me.TabPage8 = New System.Windows.Forms.TabPage
        Me.ListView7 = New System.Windows.Forms.ListView
        Me.ColumnHeader33 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader34 = New System.Windows.Forms.ColumnHeader
        Me.TabPage9 = New System.Windows.Forms.TabPage
        Me.ListView8 = New System.Windows.Forms.ListView
        Me.ColumnHeader35 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader36 = New System.Windows.Forms.ColumnHeader
        Me.TabPage10 = New System.Windows.Forms.TabPage
        Me.ListView9 = New System.Windows.Forms.ListView
        Me.ColumnHeader37 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader38 = New System.Windows.Forms.ColumnHeader
        Me.ctxBack = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ctxForward = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.lblUsableTime = New System.Windows.Forms.LinkLabel
        Me.btnWantedAdd = New System.Windows.Forms.Button
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.sbtnBack = New EveHQ.ItemBrowser.SplitButton
        Me.sbtnForward = New EveHQ.ItemBrowser.SplitButton
        Me.ctxSkills.SuspendLayout()
        Me.ssData.SuspendLayout()
        CType(Me.picBP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMaterials.SuspendLayout()
        Me.tabMaterial.SuspendLayout()
        Me.tabM1.SuspendLayout()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabM2.SuspendLayout()
        Me.tabM3.SuspendLayout()
        Me.tabM4.SuspendLayout()
        Me.tabM5.SuspendLayout()
        Me.tabM6.SuspendLayout()
        Me.tabM7.SuspendLayout()
        Me.tabM8.SuspendLayout()
        Me.tabM9.SuspendLayout()
        Me.tabVariations.SuspendLayout()
        Me.tabVariation.SuspendLayout()
        Me.tabMetaVariations.SuspendLayout()
        Me.tabComparisons.SuspendLayout()
        Me.tabFitting.SuspendLayout()
        Me.tabSkills.SuspendLayout()
        Me.tabAttributes.SuspendLayout()
        Me.tabDescription.SuspendLayout()
        Me.tabItem.SuspendLayout()
        Me.tabRecommended.SuspendLayout()
        Me.tabComponent.SuspendLayout()
        Me.tabComponents.SuspendLayout()
        Me.tabC1.SuspendLayout()
        CType(Me.nudMELevelC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabC2.SuspendLayout()
        Me.tabC3.SuspendLayout()
        Me.tabC4.SuspendLayout()
        Me.tabC5.SuspendLayout()
        Me.tabC6.SuspendLayout()
        Me.tabC7.SuspendLayout()
        Me.tabC8.SuspendLayout()
        Me.tabC9.SuspendLayout()
        Me.tabDepends.SuspendLayout()
        Me.tabEveCentral.SuspendLayout()
        Me.tabInsurance.SuspendLayout()
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabBrowser.SuspendLayout()
        Me.tabBrowse.SuspendLayout()
        Me.tabSearch.SuspendLayout()
        Me.tabAttSearch.SuspendLayout()
        Me.tabWantedList.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.TabPage9.SuspendLayout()
        Me.TabPage10.SuspendLayout()
        Me.SuspendLayout()
        '
        'ctxSkills
        '
        Me.ctxSkills.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxSkills.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator1, Me.mnuViewDetails})
        Me.ctxSkills.Name = "ctxDepend"
        Me.ctxSkills.Size = New System.Drawing.Size(144, 54)
        '
        'mnuSkillName
        '
        Me.mnuSkillName.Name = "mnuSkillName"
        Me.mnuSkillName.Size = New System.Drawing.Size(143, 22)
        Me.mnuSkillName.Text = "Skill Name"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(140, 6)
        '
        'mnuViewDetails
        '
        Me.mnuViewDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewDetails.Name = "mnuViewDetails"
        Me.mnuViewDetails.Size = New System.Drawing.Size(143, 22)
        Me.mnuViewDetails.Text = "View Details"
        '
        'ssData
        '
        Me.ssData.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ssLabel, Me.ssDBLocation, Me.ssLblID})
        Me.ssData.Location = New System.Drawing.Point(0, 538)
        Me.ssData.Name = "ssData"
        Me.ssData.Size = New System.Drawing.Size(997, 22)
        Me.ssData.TabIndex = 4
        '
        'ssLabel
        '
        Me.ssLabel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssLabel.Name = "ssLabel"
        Me.ssLabel.Size = New System.Drawing.Size(91, 17)
        Me.ssLabel.Text = "Awaiting query..."
        '
        'ssDBLocation
        '
        Me.ssDBLocation.DoubleClickEnabled = True
        Me.ssDBLocation.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssDBLocation.Name = "ssDBLocation"
        Me.ssDBLocation.Size = New System.Drawing.Size(869, 17)
        Me.ssDBLocation.Spring = True
        Me.ssDBLocation.Text = "Location:"
        '
        'ssLblID
        '
        Me.ssLblID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ssLblID.DoubleClickEnabled = True
        Me.ssLblID.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssLblID.Name = "ssLblID"
        Me.ssLblID.Size = New System.Drawing.Size(22, 17)
        Me.ssLblID.Text = "ID:"
        Me.ssLblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'picBP
        '
        Me.picBP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picBP.Location = New System.Drawing.Point(548, 3)
        Me.picBP.Name = "picBP"
        Me.picBP.Size = New System.Drawing.Size(32, 32)
        Me.picBP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picBP.TabIndex = 3
        Me.picBP.TabStop = False
        Me.picBP.Visible = False
        '
        'tabMaterials
        '
        Me.tabMaterials.Controls.Add(Me.tabMaterial)
        Me.tabMaterials.Location = New System.Drawing.Point(4, 22)
        Me.tabMaterials.Name = "tabMaterials"
        Me.tabMaterials.Size = New System.Drawing.Size(567, 325)
        Me.tabMaterials.TabIndex = 6
        Me.tabMaterials.Text = "Materials"
        Me.tabMaterials.UseVisualStyleBackColor = True
        '
        'tabMaterial
        '
        Me.tabMaterial.Controls.Add(Me.tabM1)
        Me.tabMaterial.Controls.Add(Me.tabM2)
        Me.tabMaterial.Controls.Add(Me.tabM3)
        Me.tabMaterial.Controls.Add(Me.tabM4)
        Me.tabMaterial.Controls.Add(Me.tabM5)
        Me.tabMaterial.Controls.Add(Me.tabM6)
        Me.tabMaterial.Controls.Add(Me.tabM7)
        Me.tabMaterial.Controls.Add(Me.tabM8)
        Me.tabMaterial.Controls.Add(Me.tabM9)
        Me.tabMaterial.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMaterial.Location = New System.Drawing.Point(0, 0)
        Me.tabMaterial.Name = "tabMaterial"
        Me.tabMaterial.SelectedIndex = 0
        Me.tabMaterial.Size = New System.Drawing.Size(567, 325)
        Me.tabMaterial.TabIndex = 0
        '
        'tabM1
        '
        Me.tabM1.Controls.Add(Me.lblMELevel)
        Me.tabM1.Controls.Add(Me.nudMELevel)
        Me.tabM1.Controls.Add(Me.lstM1)
        Me.tabM1.Location = New System.Drawing.Point(4, 22)
        Me.tabM1.Name = "tabM1"
        Me.tabM1.Padding = New System.Windows.Forms.Padding(3)
        Me.tabM1.Size = New System.Drawing.Size(559, 299)
        Me.tabM1.TabIndex = 0
        Me.tabM1.Text = "Manufacturing"
        Me.tabM1.UseVisualStyleBackColor = True
        '
        'lblMELevel
        '
        Me.lblMELevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMELevel.AutoSize = True
        Me.lblMELevel.Location = New System.Drawing.Point(7, 275)
        Me.lblMELevel.Name = "lblMELevel"
        Me.lblMELevel.Size = New System.Drawing.Size(53, 13)
        Me.lblMELevel.TabIndex = 4
        Me.lblMELevel.Text = "ME Level:"
        '
        'nudMELevel
        '
        Me.nudMELevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.nudMELevel.Location = New System.Drawing.Point(68, 273)
        Me.nudMELevel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMELevel.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudMELevel.Name = "nudMELevel"
        Me.nudMELevel.Size = New System.Drawing.Size(73, 21)
        Me.nudMELevel.TabIndex = 3
        Me.nudMELevel.ThousandsSeparator = True
        '
        'lstM1
        '
        Me.lstM1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstM1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colM1M, Me.colM1Q, Me.colM1ME, Me.colM1P})
        Me.lstM1.FullRowSelect = True
        Me.lstM1.GridLines = True
        Me.lstM1.Location = New System.Drawing.Point(3, 3)
        Me.lstM1.Name = "lstM1"
        Me.lstM1.Size = New System.Drawing.Size(553, 264)
        Me.lstM1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM1.TabIndex = 2
        Me.lstM1.UseCompatibleStateImageBehavior = False
        Me.lstM1.View = System.Windows.Forms.View.Details
        '
        'colM1M
        '
        Me.colM1M.Text = "Material"
        Me.colM1M.Width = 175
        '
        'colM1Q
        '
        Me.colM1Q.Text = "Perfect"
        Me.colM1Q.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colM1Q.Width = 75
        '
        'colM1ME
        '
        Me.colM1ME.Text = "ME 0"
        Me.colM1ME.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colM1ME.Width = 75
        '
        'colM1P
        '
        Me.colM1P.Text = "Pilot"
        Me.colM1P.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colM1P.Width = 75
        '
        'tabM2
        '
        Me.tabM2.Controls.Add(Me.lstM2)
        Me.tabM2.Location = New System.Drawing.Point(4, 22)
        Me.tabM2.Name = "tabM2"
        Me.tabM2.Padding = New System.Windows.Forms.Padding(3)
        Me.tabM2.Size = New System.Drawing.Size(559, 299)
        Me.tabM2.TabIndex = 1
        Me.tabM2.Text = "Research Tech"
        Me.tabM2.UseVisualStyleBackColor = True
        '
        'lstM2
        '
        Me.lstM2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lstM2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM2.FullRowSelect = True
        Me.lstM2.GridLines = True
        Me.lstM2.Location = New System.Drawing.Point(3, 3)
        Me.lstM2.Name = "lstM2"
        Me.lstM2.Size = New System.Drawing.Size(553, 293)
        Me.lstM2.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM2.TabIndex = 3
        Me.lstM2.UseCompatibleStateImageBehavior = False
        Me.lstM2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Material"
        Me.ColumnHeader3.Width = 250
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Quantity"
        Me.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader4.Width = 150
        '
        'tabM3
        '
        Me.tabM3.Controls.Add(Me.lstM3)
        Me.tabM3.Location = New System.Drawing.Point(4, 22)
        Me.tabM3.Name = "tabM3"
        Me.tabM3.Size = New System.Drawing.Size(559, 299)
        Me.tabM3.TabIndex = 2
        Me.tabM3.Text = "Research PE"
        Me.tabM3.UseVisualStyleBackColor = True
        '
        'lstM3
        '
        Me.lstM3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lstM3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM3.FullRowSelect = True
        Me.lstM3.GridLines = True
        Me.lstM3.Location = New System.Drawing.Point(0, 0)
        Me.lstM3.Name = "lstM3"
        Me.lstM3.Size = New System.Drawing.Size(559, 299)
        Me.lstM3.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM3.TabIndex = 3
        Me.lstM3.UseCompatibleStateImageBehavior = False
        Me.lstM3.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Material"
        Me.ColumnHeader5.Width = 250
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Quantity"
        Me.ColumnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader6.Width = 150
        '
        'tabM4
        '
        Me.tabM4.Controls.Add(Me.lstM4)
        Me.tabM4.Location = New System.Drawing.Point(4, 22)
        Me.tabM4.Name = "tabM4"
        Me.tabM4.Size = New System.Drawing.Size(559, 299)
        Me.tabM4.TabIndex = 3
        Me.tabM4.Text = "Research ME"
        Me.tabM4.UseVisualStyleBackColor = True
        '
        'lstM4
        '
        Me.lstM4.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader7, Me.ColumnHeader8})
        Me.lstM4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM4.FullRowSelect = True
        Me.lstM4.GridLines = True
        Me.lstM4.Location = New System.Drawing.Point(0, 0)
        Me.lstM4.Name = "lstM4"
        Me.lstM4.Size = New System.Drawing.Size(559, 299)
        Me.lstM4.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM4.TabIndex = 3
        Me.lstM4.UseCompatibleStateImageBehavior = False
        Me.lstM4.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Material"
        Me.ColumnHeader7.Width = 250
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Quantity"
        Me.ColumnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader8.Width = 150
        '
        'tabM5
        '
        Me.tabM5.Controls.Add(Me.lstM5)
        Me.tabM5.Location = New System.Drawing.Point(4, 22)
        Me.tabM5.Name = "tabM5"
        Me.tabM5.Size = New System.Drawing.Size(559, 299)
        Me.tabM5.TabIndex = 4
        Me.tabM5.Text = "Copying"
        Me.tabM5.UseVisualStyleBackColor = True
        '
        'lstM5
        '
        Me.lstM5.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader9, Me.ColumnHeader10})
        Me.lstM5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM5.FullRowSelect = True
        Me.lstM5.GridLines = True
        Me.lstM5.Location = New System.Drawing.Point(0, 0)
        Me.lstM5.Name = "lstM5"
        Me.lstM5.Size = New System.Drawing.Size(559, 299)
        Me.lstM5.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM5.TabIndex = 3
        Me.lstM5.UseCompatibleStateImageBehavior = False
        Me.lstM5.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Material"
        Me.ColumnHeader9.Width = 250
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Quantity"
        Me.ColumnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader10.Width = 150
        '
        'tabM6
        '
        Me.tabM6.Controls.Add(Me.lstM6)
        Me.tabM6.Location = New System.Drawing.Point(4, 22)
        Me.tabM6.Name = "tabM6"
        Me.tabM6.Size = New System.Drawing.Size(559, 299)
        Me.tabM6.TabIndex = 5
        Me.tabM6.Text = "Duplicating"
        Me.tabM6.UseVisualStyleBackColor = True
        '
        'lstM6
        '
        Me.lstM6.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader11, Me.ColumnHeader12})
        Me.lstM6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM6.FullRowSelect = True
        Me.lstM6.GridLines = True
        Me.lstM6.Location = New System.Drawing.Point(0, 0)
        Me.lstM6.Name = "lstM6"
        Me.lstM6.Size = New System.Drawing.Size(559, 299)
        Me.lstM6.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM6.TabIndex = 3
        Me.lstM6.UseCompatibleStateImageBehavior = False
        Me.lstM6.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Material"
        Me.ColumnHeader11.Width = 250
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Quantity"
        Me.ColumnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader12.Width = 150
        '
        'tabM7
        '
        Me.tabM7.Controls.Add(Me.lstM7)
        Me.tabM7.Location = New System.Drawing.Point(4, 22)
        Me.tabM7.Name = "tabM7"
        Me.tabM7.Size = New System.Drawing.Size(559, 299)
        Me.tabM7.TabIndex = 6
        Me.tabM7.Text = "Reverse Eng"
        Me.tabM7.UseVisualStyleBackColor = True
        '
        'lstM7
        '
        Me.lstM7.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader13, Me.ColumnHeader14})
        Me.lstM7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM7.FullRowSelect = True
        Me.lstM7.GridLines = True
        Me.lstM7.Location = New System.Drawing.Point(0, 0)
        Me.lstM7.Name = "lstM7"
        Me.lstM7.Size = New System.Drawing.Size(559, 299)
        Me.lstM7.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM7.TabIndex = 3
        Me.lstM7.UseCompatibleStateImageBehavior = False
        Me.lstM7.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Material"
        Me.ColumnHeader13.Width = 250
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "Quantity"
        Me.ColumnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader14.Width = 150
        '
        'tabM8
        '
        Me.tabM8.Controls.Add(Me.lstM8)
        Me.tabM8.Location = New System.Drawing.Point(4, 22)
        Me.tabM8.Name = "tabM8"
        Me.tabM8.Size = New System.Drawing.Size(559, 299)
        Me.tabM8.TabIndex = 7
        Me.tabM8.Text = "Invention"
        Me.tabM8.UseVisualStyleBackColor = True
        '
        'lstM8
        '
        Me.lstM8.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader15, Me.ColumnHeader16})
        Me.lstM8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM8.FullRowSelect = True
        Me.lstM8.GridLines = True
        Me.lstM8.Location = New System.Drawing.Point(0, 0)
        Me.lstM8.Name = "lstM8"
        Me.lstM8.Size = New System.Drawing.Size(559, 299)
        Me.lstM8.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM8.TabIndex = 3
        Me.lstM8.UseCompatibleStateImageBehavior = False
        Me.lstM8.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "Material"
        Me.ColumnHeader15.Width = 250
        '
        'ColumnHeader16
        '
        Me.ColumnHeader16.Text = "Quantity"
        Me.ColumnHeader16.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader16.Width = 150
        '
        'tabM9
        '
        Me.tabM9.Controls.Add(Me.lstM9)
        Me.tabM9.Location = New System.Drawing.Point(4, 22)
        Me.tabM9.Name = "tabM9"
        Me.tabM9.Size = New System.Drawing.Size(559, 299)
        Me.tabM9.TabIndex = 8
        Me.tabM9.Text = "Recycling"
        Me.tabM9.UseVisualStyleBackColor = True
        '
        'lstM9
        '
        Me.lstM9.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader17, Me.ColumnHeader18})
        Me.lstM9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM9.FullRowSelect = True
        Me.lstM9.GridLines = True
        Me.lstM9.Location = New System.Drawing.Point(0, 0)
        Me.lstM9.Name = "lstM9"
        Me.lstM9.Size = New System.Drawing.Size(559, 299)
        Me.lstM9.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstM9.TabIndex = 4
        Me.lstM9.UseCompatibleStateImageBehavior = False
        Me.lstM9.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader17
        '
        Me.ColumnHeader17.Text = "Material"
        Me.ColumnHeader17.Width = 250
        '
        'ColumnHeader18
        '
        Me.ColumnHeader18.Text = "Quantity"
        Me.ColumnHeader18.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader18.Width = 150
        '
        'tabVariations
        '
        Me.tabVariations.Controls.Add(Me.tabVariation)
        Me.tabVariations.Location = New System.Drawing.Point(4, 22)
        Me.tabVariations.Name = "tabVariations"
        Me.tabVariations.Size = New System.Drawing.Size(567, 325)
        Me.tabVariations.TabIndex = 4
        Me.tabVariations.Text = "Variations"
        Me.tabVariations.UseVisualStyleBackColor = True
        '
        'tabVariation
        '
        Me.tabVariation.Controls.Add(Me.tabMetaVariations)
        Me.tabVariation.Controls.Add(Me.tabComparisons)
        Me.tabVariation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabVariation.Location = New System.Drawing.Point(0, 0)
        Me.tabVariation.Name = "tabVariation"
        Me.tabVariation.SelectedIndex = 0
        Me.tabVariation.Size = New System.Drawing.Size(567, 325)
        Me.tabVariation.TabIndex = 1
        '
        'tabMetaVariations
        '
        Me.tabMetaVariations.Controls.Add(Me.lstVariations)
        Me.tabMetaVariations.Location = New System.Drawing.Point(4, 22)
        Me.tabMetaVariations.Name = "tabMetaVariations"
        Me.tabMetaVariations.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMetaVariations.Size = New System.Drawing.Size(559, 299)
        Me.tabMetaVariations.TabIndex = 0
        Me.tabMetaVariations.Text = "Variations"
        Me.tabMetaVariations.UseVisualStyleBackColor = True
        '
        'lstVariations
        '
        Me.lstVariations.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTypeName, Me.colMetaTypeName})
        Me.lstVariations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstVariations.FullRowSelect = True
        Me.lstVariations.GridLines = True
        Me.lstVariations.Location = New System.Drawing.Point(3, 3)
        Me.lstVariations.Name = "lstVariations"
        Me.lstVariations.Size = New System.Drawing.Size(553, 293)
        Me.lstVariations.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstVariations.TabIndex = 0
        Me.lstVariations.UseCompatibleStateImageBehavior = False
        Me.lstVariations.View = System.Windows.Forms.View.Details
        '
        'colTypeName
        '
        Me.colTypeName.Text = "Item"
        Me.colTypeName.Width = 270
        '
        'colMetaTypeName
        '
        Me.colMetaTypeName.Text = "Meta Type"
        Me.colMetaTypeName.Width = 150
        '
        'tabComparisons
        '
        Me.tabComparisons.Controls.Add(Me.chkShowAllColumns)
        Me.tabComparisons.Controls.Add(Me.lstComparisons)
        Me.tabComparisons.Location = New System.Drawing.Point(4, 22)
        Me.tabComparisons.Name = "tabComparisons"
        Me.tabComparisons.Padding = New System.Windows.Forms.Padding(3)
        Me.tabComparisons.Size = New System.Drawing.Size(559, 299)
        Me.tabComparisons.TabIndex = 1
        Me.tabComparisons.Text = "Comparison"
        Me.tabComparisons.UseVisualStyleBackColor = True
        '
        'chkShowAllColumns
        '
        Me.chkShowAllColumns.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkShowAllColumns.AutoSize = True
        Me.chkShowAllColumns.Location = New System.Drawing.Point(7, 274)
        Me.chkShowAllColumns.Name = "chkShowAllColumns"
        Me.chkShowAllColumns.Size = New System.Drawing.Size(109, 17)
        Me.chkShowAllColumns.TabIndex = 2
        Me.chkShowAllColumns.Text = "Show All Columns"
        Me.chkShowAllColumns.UseVisualStyleBackColor = True
        '
        'lstComparisons
        '
        Me.lstComparisons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstComparisons.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader39, Me.ColumnHeader40})
        Me.lstComparisons.FullRowSelect = True
        Me.lstComparisons.GridLines = True
        Me.lstComparisons.Location = New System.Drawing.Point(3, 3)
        Me.lstComparisons.Name = "lstComparisons"
        Me.lstComparisons.Size = New System.Drawing.Size(553, 264)
        Me.lstComparisons.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstComparisons.TabIndex = 1
        Me.lstComparisons.UseCompatibleStateImageBehavior = False
        Me.lstComparisons.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader39
        '
        Me.ColumnHeader39.Text = "Item"
        Me.ColumnHeader39.Width = 270
        '
        'ColumnHeader40
        '
        Me.ColumnHeader40.Text = "Meta Type"
        Me.ColumnHeader40.Width = 150
        '
        'tabFitting
        '
        Me.tabFitting.Controls.Add(Me.lstFitting)
        Me.tabFitting.Location = New System.Drawing.Point(4, 22)
        Me.tabFitting.Name = "tabFitting"
        Me.tabFitting.Size = New System.Drawing.Size(567, 325)
        Me.tabFitting.TabIndex = 3
        Me.tabFitting.Text = "Fitting"
        Me.tabFitting.UseVisualStyleBackColor = True
        '
        'lstFitting
        '
        Me.lstFitting.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lstFitting.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFitting.FullRowSelect = True
        Me.lstFitting.GridLines = True
        Me.lstFitting.Location = New System.Drawing.Point(0, 0)
        Me.lstFitting.Name = "lstFitting"
        Me.lstFitting.Size = New System.Drawing.Size(567, 325)
        Me.lstFitting.TabIndex = 1
        Me.lstFitting.UseCompatibleStateImageBehavior = False
        Me.lstFitting.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Attribute"
        Me.ColumnHeader1.Width = 210
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Data"
        Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader2.Width = 210
        '
        'tabSkills
        '
        Me.tabSkills.Controls.Add(Me.btnViewSkills)
        Me.tabSkills.Controls.Add(Me.btnAddSkills)
        Me.tabSkills.Controls.Add(Me.tvwReqs)
        Me.tabSkills.Location = New System.Drawing.Point(4, 22)
        Me.tabSkills.Name = "tabSkills"
        Me.tabSkills.Size = New System.Drawing.Size(567, 325)
        Me.tabSkills.TabIndex = 2
        Me.tabSkills.Text = "Skills"
        Me.tabSkills.UseVisualStyleBackColor = True
        '
        'btnViewSkills
        '
        Me.btnViewSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnViewSkills.Location = New System.Drawing.Point(3, 299)
        Me.btnViewSkills.Name = "btnViewSkills"
        Me.btnViewSkills.Size = New System.Drawing.Size(121, 23)
        Me.btnViewSkills.TabIndex = 3
        Me.btnViewSkills.Text = "Show Needed Skills"
        Me.btnViewSkills.UseVisualStyleBackColor = True
        '
        'btnAddSkills
        '
        Me.btnAddSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddSkills.Location = New System.Drawing.Point(301, 299)
        Me.btnAddSkills.Name = "btnAddSkills"
        Me.btnAddSkills.Size = New System.Drawing.Size(155, 23)
        Me.btnAddSkills.TabIndex = 2
        Me.btnAddSkills.Text = "Add Needed Skills to Queue"
        Me.btnAddSkills.UseVisualStyleBackColor = True
        '
        'tvwReqs
        '
        Me.tvwReqs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwReqs.ContextMenuStrip = Me.ctxSkills
        Me.tvwReqs.Indent = 25
        Me.tvwReqs.ItemHeight = 20
        Me.tvwReqs.Location = New System.Drawing.Point(3, 3)
        Me.tvwReqs.Name = "tvwReqs"
        Me.tvwReqs.ShowPlusMinus = False
        Me.tvwReqs.Size = New System.Drawing.Size(561, 290)
        Me.tvwReqs.TabIndex = 1
        '
        'tabAttributes
        '
        Me.tabAttributes.Controls.Add(Me.lstAttributes)
        Me.tabAttributes.Location = New System.Drawing.Point(4, 22)
        Me.tabAttributes.Name = "tabAttributes"
        Me.tabAttributes.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAttributes.Size = New System.Drawing.Size(567, 325)
        Me.tabAttributes.TabIndex = 1
        Me.tabAttributes.Text = "Attributes"
        Me.tabAttributes.UseVisualStyleBackColor = True
        '
        'lstAttributes
        '
        Me.lstAttributes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttribute, Me.colData})
        Me.lstAttributes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstAttributes.FullRowSelect = True
        Me.lstAttributes.GridLines = True
        Me.lstAttributes.Location = New System.Drawing.Point(3, 3)
        Me.lstAttributes.Name = "lstAttributes"
        Me.lstAttributes.ShowItemToolTips = True
        Me.lstAttributes.Size = New System.Drawing.Size(561, 319)
        Me.lstAttributes.TabIndex = 0
        Me.lstAttributes.UseCompatibleStateImageBehavior = False
        Me.lstAttributes.View = System.Windows.Forms.View.Details
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
        Me.colData.Width = 210
        '
        'tabDescription
        '
        Me.tabDescription.Controls.Add(Me.lblDescription)
        Me.tabDescription.Location = New System.Drawing.Point(4, 22)
        Me.tabDescription.Name = "tabDescription"
        Me.tabDescription.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDescription.Size = New System.Drawing.Size(567, 325)
        Me.tabDescription.TabIndex = 0
        Me.tabDescription.Text = "Description"
        Me.tabDescription.UseVisualStyleBackColor = True
        '
        'lblDescription
        '
        Me.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDescription.Location = New System.Drawing.Point(3, 3)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(561, 319)
        Me.lblDescription.TabIndex = 0
        '
        'tabItem
        '
        Me.tabItem.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabItem.Controls.Add(Me.tabDescription)
        Me.tabItem.Controls.Add(Me.tabAttributes)
        Me.tabItem.Controls.Add(Me.tabSkills)
        Me.tabItem.Controls.Add(Me.tabFitting)
        Me.tabItem.Controls.Add(Me.tabVariations)
        Me.tabItem.Controls.Add(Me.tabRecommended)
        Me.tabItem.Controls.Add(Me.tabMaterials)
        Me.tabItem.Controls.Add(Me.tabComponent)
        Me.tabItem.Controls.Add(Me.tabDepends)
        Me.tabItem.Controls.Add(Me.tabEveCentral)
        Me.tabItem.Controls.Add(Me.tabInsurance)
        Me.tabItem.Location = New System.Drawing.Point(411, 177)
        Me.tabItem.Name = "tabItem"
        Me.tabItem.SelectedIndex = 0
        Me.tabItem.Size = New System.Drawing.Size(575, 351)
        Me.tabItem.TabIndex = 1
        '
        'tabRecommended
        '
        Me.tabRecommended.Controls.Add(Me.lvwRecommended)
        Me.tabRecommended.Location = New System.Drawing.Point(4, 22)
        Me.tabRecommended.Name = "tabRecommended"
        Me.tabRecommended.Size = New System.Drawing.Size(567, 325)
        Me.tabRecommended.TabIndex = 11
        Me.tabRecommended.Text = "Recommended"
        Me.tabRecommended.UseVisualStyleBackColor = True
        '
        'lvwRecommended
        '
        Me.lvwRecommended.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader59, Me.ColumnHeader60})
        Me.lvwRecommended.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwRecommended.FullRowSelect = True
        Me.lvwRecommended.GridLines = True
        Me.lvwRecommended.Location = New System.Drawing.Point(0, 0)
        Me.lvwRecommended.Name = "lvwRecommended"
        Me.lvwRecommended.Size = New System.Drawing.Size(567, 325)
        Me.lvwRecommended.SmallImageList = Me.imgListCerts
        Me.lvwRecommended.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwRecommended.TabIndex = 1
        Me.lvwRecommended.UseCompatibleStateImageBehavior = False
        Me.lvwRecommended.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader59
        '
        Me.ColumnHeader59.Text = "Certificate"
        Me.ColumnHeader59.Width = 300
        '
        'ColumnHeader60
        '
        Me.ColumnHeader60.Text = "Level"
        Me.ColumnHeader60.Width = 150
        '
        'imgListCerts
        '
        Me.imgListCerts.ImageStream = CType(resources.GetObject("imgListCerts.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgListCerts.TransparentColor = System.Drawing.Color.Transparent
        Me.imgListCerts.Images.SetKeyName(0, "icon79_01.png")
        Me.imgListCerts.Images.SetKeyName(1, "icon79_02.png")
        Me.imgListCerts.Images.SetKeyName(2, "icon79_03.png")
        Me.imgListCerts.Images.SetKeyName(3, "icon79_04.png")
        Me.imgListCerts.Images.SetKeyName(4, "icon79_05.png")
        Me.imgListCerts.Images.SetKeyName(5, "icon79_06.png")
        '
        'tabComponent
        '
        Me.tabComponent.Controls.Add(Me.tabComponents)
        Me.tabComponent.Location = New System.Drawing.Point(4, 22)
        Me.tabComponent.Name = "tabComponent"
        Me.tabComponent.Size = New System.Drawing.Size(567, 325)
        Me.tabComponent.TabIndex = 7
        Me.tabComponent.Text = "Component Of"
        Me.tabComponent.UseVisualStyleBackColor = True
        '
        'tabComponents
        '
        Me.tabComponents.Controls.Add(Me.tabC1)
        Me.tabComponents.Controls.Add(Me.tabC2)
        Me.tabComponents.Controls.Add(Me.tabC3)
        Me.tabComponents.Controls.Add(Me.tabC4)
        Me.tabComponents.Controls.Add(Me.tabC5)
        Me.tabComponents.Controls.Add(Me.tabC6)
        Me.tabComponents.Controls.Add(Me.tabC7)
        Me.tabComponents.Controls.Add(Me.tabC8)
        Me.tabComponents.Controls.Add(Me.tabC9)
        Me.tabComponents.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabComponents.Location = New System.Drawing.Point(0, 0)
        Me.tabComponents.Name = "tabComponents"
        Me.tabComponents.SelectedIndex = 0
        Me.tabComponents.Size = New System.Drawing.Size(567, 325)
        Me.tabComponents.TabIndex = 1
        '
        'tabC1
        '
        Me.tabC1.Controls.Add(Me.lblMELevelC)
        Me.tabC1.Controls.Add(Me.nudMELevelC)
        Me.tabC1.Controls.Add(Me.lstC1)
        Me.tabC1.Location = New System.Drawing.Point(4, 22)
        Me.tabC1.Name = "tabC1"
        Me.tabC1.Padding = New System.Windows.Forms.Padding(3)
        Me.tabC1.Size = New System.Drawing.Size(559, 299)
        Me.tabC1.TabIndex = 0
        Me.tabC1.Text = "Manufacturing"
        Me.tabC1.UseVisualStyleBackColor = True
        '
        'lblMELevelC
        '
        Me.lblMELevelC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMELevelC.AutoSize = True
        Me.lblMELevelC.Location = New System.Drawing.Point(7, 275)
        Me.lblMELevelC.Name = "lblMELevelC"
        Me.lblMELevelC.Size = New System.Drawing.Size(53, 13)
        Me.lblMELevelC.TabIndex = 4
        Me.lblMELevelC.Text = "ME Level:"
        '
        'nudMELevelC
        '
        Me.nudMELevelC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.nudMELevelC.Location = New System.Drawing.Point(68, 273)
        Me.nudMELevelC.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMELevelC.Name = "nudMELevelC"
        Me.nudMELevelC.Size = New System.Drawing.Size(73, 21)
        Me.nudMELevelC.TabIndex = 3
        Me.nudMELevelC.ThousandsSeparator = True
        '
        'lstC1
        '
        Me.lstC1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstC1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colC1M, Me.colC1Q, Me.colC1ME, Me.colC1P})
        Me.lstC1.FullRowSelect = True
        Me.lstC1.GridLines = True
        Me.lstC1.Location = New System.Drawing.Point(3, 3)
        Me.lstC1.Name = "lstC1"
        Me.lstC1.Size = New System.Drawing.Size(553, 264)
        Me.lstC1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC1.TabIndex = 2
        Me.lstC1.UseCompatibleStateImageBehavior = False
        Me.lstC1.View = System.Windows.Forms.View.Details
        '
        'colC1M
        '
        Me.colC1M.Text = "Material"
        Me.colC1M.Width = 175
        '
        'colC1Q
        '
        Me.colC1Q.Text = "Perfect"
        Me.colC1Q.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colC1Q.Width = 75
        '
        'colC1ME
        '
        Me.colC1ME.Text = "ME 0"
        Me.colC1ME.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colC1ME.Width = 75
        '
        'colC1P
        '
        Me.colC1P.Text = "Pilot"
        Me.colC1P.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colC1P.Width = 75
        '
        'tabC2
        '
        Me.tabC2.Controls.Add(Me.lstC2)
        Me.tabC2.Location = New System.Drawing.Point(4, 22)
        Me.tabC2.Name = "tabC2"
        Me.tabC2.Padding = New System.Windows.Forms.Padding(3)
        Me.tabC2.Size = New System.Drawing.Size(559, 299)
        Me.tabC2.TabIndex = 1
        Me.tabC2.Text = "Research Tech"
        Me.tabC2.UseVisualStyleBackColor = True
        '
        'lstC2
        '
        Me.lstC2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader43, Me.ColumnHeader44})
        Me.lstC2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC2.FullRowSelect = True
        Me.lstC2.GridLines = True
        Me.lstC2.Location = New System.Drawing.Point(3, 3)
        Me.lstC2.Name = "lstC2"
        Me.lstC2.Size = New System.Drawing.Size(553, 293)
        Me.lstC2.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC2.TabIndex = 3
        Me.lstC2.UseCompatibleStateImageBehavior = False
        Me.lstC2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader43
        '
        Me.ColumnHeader43.Text = "Material"
        Me.ColumnHeader43.Width = 250
        '
        'ColumnHeader44
        '
        Me.ColumnHeader44.Text = "Quantity"
        Me.ColumnHeader44.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader44.Width = 150
        '
        'tabC3
        '
        Me.tabC3.Controls.Add(Me.lstC3)
        Me.tabC3.Location = New System.Drawing.Point(4, 22)
        Me.tabC3.Name = "tabC3"
        Me.tabC3.Size = New System.Drawing.Size(559, 299)
        Me.tabC3.TabIndex = 2
        Me.tabC3.Text = "Research PE"
        Me.tabC3.UseVisualStyleBackColor = True
        '
        'lstC3
        '
        Me.lstC3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader45, Me.ColumnHeader46})
        Me.lstC3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC3.FullRowSelect = True
        Me.lstC3.GridLines = True
        Me.lstC3.Location = New System.Drawing.Point(0, 0)
        Me.lstC3.Name = "lstC3"
        Me.lstC3.Size = New System.Drawing.Size(559, 299)
        Me.lstC3.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC3.TabIndex = 3
        Me.lstC3.UseCompatibleStateImageBehavior = False
        Me.lstC3.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader45
        '
        Me.ColumnHeader45.Text = "Material"
        Me.ColumnHeader45.Width = 250
        '
        'ColumnHeader46
        '
        Me.ColumnHeader46.Text = "Quantity"
        Me.ColumnHeader46.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader46.Width = 150
        '
        'tabC4
        '
        Me.tabC4.Controls.Add(Me.lstC4)
        Me.tabC4.Location = New System.Drawing.Point(4, 22)
        Me.tabC4.Name = "tabC4"
        Me.tabC4.Size = New System.Drawing.Size(559, 299)
        Me.tabC4.TabIndex = 3
        Me.tabC4.Text = "Research ME"
        Me.tabC4.UseVisualStyleBackColor = True
        '
        'lstC4
        '
        Me.lstC4.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader47, Me.ColumnHeader48})
        Me.lstC4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC4.FullRowSelect = True
        Me.lstC4.GridLines = True
        Me.lstC4.Location = New System.Drawing.Point(0, 0)
        Me.lstC4.Name = "lstC4"
        Me.lstC4.Size = New System.Drawing.Size(559, 299)
        Me.lstC4.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC4.TabIndex = 3
        Me.lstC4.UseCompatibleStateImageBehavior = False
        Me.lstC4.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader47
        '
        Me.ColumnHeader47.Text = "Material"
        Me.ColumnHeader47.Width = 250
        '
        'ColumnHeader48
        '
        Me.ColumnHeader48.Text = "Quantity"
        Me.ColumnHeader48.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader48.Width = 150
        '
        'tabC5
        '
        Me.tabC5.Controls.Add(Me.lstC5)
        Me.tabC5.Location = New System.Drawing.Point(4, 22)
        Me.tabC5.Name = "tabC5"
        Me.tabC5.Size = New System.Drawing.Size(559, 299)
        Me.tabC5.TabIndex = 4
        Me.tabC5.Text = "Copying"
        Me.tabC5.UseVisualStyleBackColor = True
        '
        'lstC5
        '
        Me.lstC5.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader49, Me.ColumnHeader50})
        Me.lstC5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC5.FullRowSelect = True
        Me.lstC5.GridLines = True
        Me.lstC5.Location = New System.Drawing.Point(0, 0)
        Me.lstC5.Name = "lstC5"
        Me.lstC5.Size = New System.Drawing.Size(559, 299)
        Me.lstC5.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC5.TabIndex = 3
        Me.lstC5.UseCompatibleStateImageBehavior = False
        Me.lstC5.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader49
        '
        Me.ColumnHeader49.Text = "Material"
        Me.ColumnHeader49.Width = 250
        '
        'ColumnHeader50
        '
        Me.ColumnHeader50.Text = "Quantity"
        Me.ColumnHeader50.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader50.Width = 150
        '
        'tabC6
        '
        Me.tabC6.Controls.Add(Me.lstC6)
        Me.tabC6.Location = New System.Drawing.Point(4, 22)
        Me.tabC6.Name = "tabC6"
        Me.tabC6.Size = New System.Drawing.Size(559, 299)
        Me.tabC6.TabIndex = 5
        Me.tabC6.Text = "Duplicating"
        Me.tabC6.UseVisualStyleBackColor = True
        '
        'lstC6
        '
        Me.lstC6.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader51, Me.ColumnHeader52})
        Me.lstC6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC6.FullRowSelect = True
        Me.lstC6.GridLines = True
        Me.lstC6.Location = New System.Drawing.Point(0, 0)
        Me.lstC6.Name = "lstC6"
        Me.lstC6.Size = New System.Drawing.Size(559, 299)
        Me.lstC6.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC6.TabIndex = 3
        Me.lstC6.UseCompatibleStateImageBehavior = False
        Me.lstC6.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader51
        '
        Me.ColumnHeader51.Text = "Material"
        Me.ColumnHeader51.Width = 250
        '
        'ColumnHeader52
        '
        Me.ColumnHeader52.Text = "Quantity"
        Me.ColumnHeader52.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader52.Width = 150
        '
        'tabC7
        '
        Me.tabC7.Controls.Add(Me.lstC7)
        Me.tabC7.Location = New System.Drawing.Point(4, 22)
        Me.tabC7.Name = "tabC7"
        Me.tabC7.Size = New System.Drawing.Size(559, 299)
        Me.tabC7.TabIndex = 6
        Me.tabC7.Text = "Reverse Eng"
        Me.tabC7.UseVisualStyleBackColor = True
        '
        'lstC7
        '
        Me.lstC7.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader53, Me.ColumnHeader54})
        Me.lstC7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC7.FullRowSelect = True
        Me.lstC7.GridLines = True
        Me.lstC7.Location = New System.Drawing.Point(0, 0)
        Me.lstC7.Name = "lstC7"
        Me.lstC7.Size = New System.Drawing.Size(559, 299)
        Me.lstC7.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC7.TabIndex = 3
        Me.lstC7.UseCompatibleStateImageBehavior = False
        Me.lstC7.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader53
        '
        Me.ColumnHeader53.Text = "Material"
        Me.ColumnHeader53.Width = 250
        '
        'ColumnHeader54
        '
        Me.ColumnHeader54.Text = "Quantity"
        Me.ColumnHeader54.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader54.Width = 150
        '
        'tabC8
        '
        Me.tabC8.Controls.Add(Me.lstC8)
        Me.tabC8.Location = New System.Drawing.Point(4, 22)
        Me.tabC8.Name = "tabC8"
        Me.tabC8.Size = New System.Drawing.Size(559, 299)
        Me.tabC8.TabIndex = 7
        Me.tabC8.Text = "Invention"
        Me.tabC8.UseVisualStyleBackColor = True
        '
        'lstC8
        '
        Me.lstC8.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader55, Me.ColumnHeader56})
        Me.lstC8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC8.FullRowSelect = True
        Me.lstC8.GridLines = True
        Me.lstC8.Location = New System.Drawing.Point(0, 0)
        Me.lstC8.Name = "lstC8"
        Me.lstC8.Size = New System.Drawing.Size(559, 299)
        Me.lstC8.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC8.TabIndex = 3
        Me.lstC8.UseCompatibleStateImageBehavior = False
        Me.lstC8.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader55
        '
        Me.ColumnHeader55.Text = "Material"
        Me.ColumnHeader55.Width = 250
        '
        'ColumnHeader56
        '
        Me.ColumnHeader56.Text = "Quantity"
        Me.ColumnHeader56.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader56.Width = 150
        '
        'tabC9
        '
        Me.tabC9.Controls.Add(Me.lstC9)
        Me.tabC9.Location = New System.Drawing.Point(4, 22)
        Me.tabC9.Name = "tabC9"
        Me.tabC9.Size = New System.Drawing.Size(559, 299)
        Me.tabC9.TabIndex = 8
        Me.tabC9.Text = "Composition"
        Me.tabC9.UseVisualStyleBackColor = True
        '
        'lstC9
        '
        Me.lstC9.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader57, Me.ColumnHeader58})
        Me.lstC9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC9.FullRowSelect = True
        Me.lstC9.GridLines = True
        Me.lstC9.Location = New System.Drawing.Point(0, 0)
        Me.lstC9.Name = "lstC9"
        Me.lstC9.Size = New System.Drawing.Size(559, 299)
        Me.lstC9.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC9.TabIndex = 4
        Me.lstC9.UseCompatibleStateImageBehavior = False
        Me.lstC9.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader57
        '
        Me.ColumnHeader57.Text = "Material"
        Me.ColumnHeader57.Width = 250
        '
        'ColumnHeader58
        '
        Me.ColumnHeader58.Text = "Quantity"
        Me.ColumnHeader58.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader58.Width = 150
        '
        'tabDepends
        '
        Me.tabDepends.Controls.Add(Me.lvwDepend)
        Me.tabDepends.Location = New System.Drawing.Point(4, 22)
        Me.tabDepends.Name = "tabDepends"
        Me.tabDepends.Size = New System.Drawing.Size(567, 325)
        Me.tabDepends.TabIndex = 9
        Me.tabDepends.Text = "Dependencies"
        Me.tabDepends.UseVisualStyleBackColor = True
        '
        'lvwDepend
        '
        Me.lvwDepend.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NeededFor, Me.NeededGroup, Me.NeededLevel})
        Me.lvwDepend.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwDepend.FullRowSelect = True
        Me.lvwDepend.GridLines = True
        ListViewGroup1.Header = "Containers"
        ListViewGroup1.Name = "Cat2"
        ListViewGroup2.Header = "Materials"
        ListViewGroup2.Name = "Cat4"
        ListViewGroup3.Header = "Accessories"
        ListViewGroup3.Name = "Cat5"
        ListViewGroup4.Header = "Ships"
        ListViewGroup4.Name = "Cat6"
        ListViewGroup5.Header = "Modules"
        ListViewGroup5.Name = "Cat7"
        ListViewGroup6.Header = "Charges"
        ListViewGroup6.Name = "Cat8"
        ListViewGroup7.Header = "Blueprints"
        ListViewGroup7.Name = "Cat9"
        ListViewGroup8.Header = "Skills"
        ListViewGroup8.Name = "Cat16"
        ListViewGroup9.Header = "Drones"
        ListViewGroup9.Name = "Cat18"
        ListViewGroup10.Header = "Implants"
        ListViewGroup10.Name = "Cat20"
        ListViewGroup11.Header = "Mobile Disruptors"
        ListViewGroup11.Name = "Cat22"
        ListViewGroup12.Header = "POS Equipment"
        ListViewGroup12.Name = "Cat23"
        Me.lvwDepend.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3, ListViewGroup4, ListViewGroup5, ListViewGroup6, ListViewGroup7, ListViewGroup8, ListViewGroup9, ListViewGroup10, ListViewGroup11, ListViewGroup12})
        Me.lvwDepend.Location = New System.Drawing.Point(0, 0)
        Me.lvwDepend.Name = "lvwDepend"
        Me.lvwDepend.ShowItemToolTips = True
        Me.lvwDepend.Size = New System.Drawing.Size(567, 325)
        Me.lvwDepend.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwDepend.TabIndex = 1
        Me.lvwDepend.UseCompatibleStateImageBehavior = False
        Me.lvwDepend.View = System.Windows.Forms.View.Details
        '
        'NeededFor
        '
        Me.NeededFor.Text = "Required For"
        Me.NeededFor.Width = 250
        '
        'NeededGroup
        '
        Me.NeededGroup.Text = "Group"
        Me.NeededGroup.Width = 175
        '
        'NeededLevel
        '
        Me.NeededLevel.Text = "Level"
        Me.NeededLevel.Width = 75
        '
        'tabEveCentral
        '
        Me.tabEveCentral.Controls.Add(Me.lstEveCentral)
        Me.tabEveCentral.Location = New System.Drawing.Point(4, 22)
        Me.tabEveCentral.Name = "tabEveCentral"
        Me.tabEveCentral.Size = New System.Drawing.Size(567, 325)
        Me.tabEveCentral.TabIndex = 8
        Me.tabEveCentral.Text = "Eve Central Data"
        Me.tabEveCentral.UseVisualStyleBackColor = True
        '
        'lstEveCentral
        '
        Me.lstEveCentral.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader41, Me.ColumnHeader42})
        Me.lstEveCentral.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstEveCentral.FullRowSelect = True
        Me.lstEveCentral.GridLines = True
        Me.lstEveCentral.Location = New System.Drawing.Point(0, 0)
        Me.lstEveCentral.Name = "lstEveCentral"
        Me.lstEveCentral.Size = New System.Drawing.Size(567, 325)
        Me.lstEveCentral.TabIndex = 1
        Me.lstEveCentral.UseCompatibleStateImageBehavior = False
        Me.lstEveCentral.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader41
        '
        Me.ColumnHeader41.Text = "Attribute"
        Me.ColumnHeader41.Width = 210
        '
        'ColumnHeader42
        '
        Me.ColumnHeader42.Text = "Data"
        Me.ColumnHeader42.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader42.Width = 210
        '
        'tabInsurance
        '
        Me.tabInsurance.Controls.Add(Me.lstInsurance)
        Me.tabInsurance.Location = New System.Drawing.Point(4, 22)
        Me.tabInsurance.Name = "tabInsurance"
        Me.tabInsurance.Padding = New System.Windows.Forms.Padding(3)
        Me.tabInsurance.Size = New System.Drawing.Size(567, 325)
        Me.tabInsurance.TabIndex = 10
        Me.tabInsurance.Text = "Insurance"
        Me.tabInsurance.UseVisualStyleBackColor = True
        '
        'lstInsurance
        '
        Me.lstInsurance.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Type, Me.Fee, Me.Payout, Me.MarketPrice, Me.PayoutProfit})
        Me.lstInsurance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstInsurance.Location = New System.Drawing.Point(3, 3)
        Me.lstInsurance.Name = "lstInsurance"
        Me.lstInsurance.Size = New System.Drawing.Size(561, 319)
        Me.lstInsurance.TabIndex = 0
        Me.lstInsurance.UseCompatibleStateImageBehavior = False
        Me.lstInsurance.View = System.Windows.Forms.View.Details
        '
        'Type
        '
        Me.Type.Text = "Type"
        Me.Type.Width = 100
        '
        'Fee
        '
        Me.Fee.Text = "Fee"
        Me.Fee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Fee.Width = 100
        '
        'Payout
        '
        Me.Payout.Text = "Payout"
        Me.Payout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Payout.Width = 100
        '
        'MarketPrice
        '
        Me.MarketPrice.Text = "Market Price"
        Me.MarketPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MarketPrice.Width = 100
        '
        'PayoutProfit
        '
        Me.PayoutProfit.Text = "Payout Profit"
        Me.PayoutProfit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PayoutProfit.Width = 100
        '
        'picItem
        '
        Me.picItem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picItem.ErrorImage = Global.EveHQ.ItemBrowser.My.Resources.Resources.noitem
        Me.picItem.InitialImage = Global.EveHQ.ItemBrowser.My.Resources.Resources.noitem
        Me.picItem.Location = New System.Drawing.Point(411, 43)
        Me.picItem.Name = "picItem"
        Me.picItem.Size = New System.Drawing.Size(128, 128)
        Me.picItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picItem.TabIndex = 0
        Me.picItem.TabStop = False
        '
        'lblItem
        '
        Me.lblItem.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItem.Location = New System.Drawing.Point(545, 59)
        Me.lblItem.Name = "lblItem"
        Me.lblItem.Size = New System.Drawing.Size(323, 61)
        Me.lblItem.TabIndex = 2
        '
        'lblUsable
        '
        Me.lblUsable.AutoSize = True
        Me.lblUsable.Location = New System.Drawing.Point(545, 132)
        Me.lblUsable.Name = "lblUsable"
        Me.lblUsable.Size = New System.Drawing.Size(39, 13)
        Me.lblUsable.TabIndex = 6
        Me.lblUsable.Text = "Usable"
        '
        'tabBrowser
        '
        Me.tabBrowser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tabBrowser.Controls.Add(Me.tabBrowse)
        Me.tabBrowser.Controls.Add(Me.tabSearch)
        Me.tabBrowser.Controls.Add(Me.tabAttSearch)
        Me.tabBrowser.Controls.Add(Me.tabWantedList)
        Me.tabBrowser.Location = New System.Drawing.Point(13, 13)
        Me.tabBrowser.Name = "tabBrowser"
        Me.tabBrowser.SelectedIndex = 0
        Me.tabBrowser.Size = New System.Drawing.Size(392, 515)
        Me.tabBrowser.TabIndex = 9
        '
        'tabBrowse
        '
        Me.tabBrowse.Controls.Add(Me.chkBrowseNonPublished)
        Me.tabBrowse.Controls.Add(Me.tvwBrowse)
        Me.tabBrowse.Location = New System.Drawing.Point(4, 22)
        Me.tabBrowse.Name = "tabBrowse"
        Me.tabBrowse.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBrowse.Size = New System.Drawing.Size(384, 489)
        Me.tabBrowse.TabIndex = 1
        Me.tabBrowse.Text = "Browse"
        Me.tabBrowse.UseVisualStyleBackColor = True
        '
        'chkBrowseNonPublished
        '
        Me.chkBrowseNonPublished.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkBrowseNonPublished.AutoSize = True
        Me.chkBrowseNonPublished.Location = New System.Drawing.Point(4, 466)
        Me.chkBrowseNonPublished.Name = "chkBrowseNonPublished"
        Me.chkBrowseNonPublished.Size = New System.Drawing.Size(162, 17)
        Me.chkBrowseNonPublished.TabIndex = 1
        Me.chkBrowseNonPublished.Text = "Include Non-Published Items"
        Me.chkBrowseNonPublished.UseVisualStyleBackColor = True
        '
        'tvwBrowse
        '
        Me.tvwBrowse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tvwBrowse.HideSelection = False
        Me.tvwBrowse.Location = New System.Drawing.Point(4, 7)
        Me.tvwBrowse.Name = "tvwBrowse"
        Me.tvwBrowse.Size = New System.Drawing.Size(374, 453)
        Me.tvwBrowse.TabIndex = 0
        '
        'tabSearch
        '
        Me.tabSearch.Controls.Add(Me.lstSearch)
        Me.tabSearch.Controls.Add(Me.lblSearchCount)
        Me.tabSearch.Controls.Add(Me.txtSearch)
        Me.tabSearch.Controls.Add(Me.lblSearch)
        Me.tabSearch.Location = New System.Drawing.Point(4, 22)
        Me.tabSearch.Name = "tabSearch"
        Me.tabSearch.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSearch.Size = New System.Drawing.Size(384, 489)
        Me.tabSearch.TabIndex = 0
        Me.tabSearch.Text = "Search"
        Me.tabSearch.UseVisualStyleBackColor = True
        '
        'lstSearch
        '
        Me.lstSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstSearch.FormattingEnabled = True
        Me.lstSearch.Location = New System.Drawing.Point(6, 46)
        Me.lstSearch.Name = "lstSearch"
        Me.lstSearch.Size = New System.Drawing.Size(372, 433)
        Me.lstSearch.TabIndex = 5
        '
        'lblSearchCount
        '
        Me.lblSearchCount.Location = New System.Drawing.Point(242, 3)
        Me.lblSearchCount.Name = "lblSearchCount"
        Me.lblSearchCount.Size = New System.Drawing.Size(136, 14)
        Me.lblSearchCount.TabIndex = 4
        Me.lblSearchCount.Text = "0 items found"
        Me.lblSearchCount.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(6, 20)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(372, 21)
        Me.txtSearch.TabIndex = 2
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(6, 3)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblSearch.TabIndex = 1
        Me.lblSearch.Text = "Search:"
        '
        'tabAttSearch
        '
        Me.tabAttSearch.Controls.Add(Me.lstAttSearch)
        Me.tabAttSearch.Controls.Add(Me.cboAttSearch)
        Me.tabAttSearch.Controls.Add(Me.lblAttSearchCount)
        Me.tabAttSearch.Controls.Add(Me.lblAttSearch)
        Me.tabAttSearch.Location = New System.Drawing.Point(4, 22)
        Me.tabAttSearch.Name = "tabAttSearch"
        Me.tabAttSearch.Size = New System.Drawing.Size(384, 489)
        Me.tabAttSearch.TabIndex = 2
        Me.tabAttSearch.Text = "Attr Search"
        Me.tabAttSearch.UseVisualStyleBackColor = True
        '
        'lstAttSearch
        '
        Me.lstAttSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstAttSearch.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttName, Me.colAttValue})
        Me.lstAttSearch.FullRowSelect = True
        Me.lstAttSearch.GridLines = True
        Me.lstAttSearch.Location = New System.Drawing.Point(6, 47)
        Me.lstAttSearch.Name = "lstAttSearch"
        Me.lstAttSearch.ShowItemToolTips = True
        Me.lstAttSearch.Size = New System.Drawing.Size(367, 439)
        Me.lstAttSearch.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstAttSearch.TabIndex = 11
        Me.lstAttSearch.UseCompatibleStateImageBehavior = False
        Me.lstAttSearch.View = System.Windows.Forms.View.Details
        '
        'colAttName
        '
        Me.colAttName.Text = "Item Name"
        Me.colAttName.Width = 250
        '
        'colAttValue
        '
        Me.colAttValue.Text = "Value"
        Me.colAttValue.Width = 75
        '
        'cboAttSearch
        '
        Me.cboAttSearch.FormattingEnabled = True
        Me.cboAttSearch.Location = New System.Drawing.Point(6, 19)
        Me.cboAttSearch.Name = "cboAttSearch"
        Me.cboAttSearch.Size = New System.Drawing.Size(367, 21)
        Me.cboAttSearch.TabIndex = 10
        '
        'lblAttSearchCount
        '
        Me.lblAttSearchCount.Location = New System.Drawing.Point(237, 3)
        Me.lblAttSearchCount.Name = "lblAttSearchCount"
        Me.lblAttSearchCount.Size = New System.Drawing.Size(136, 14)
        Me.lblAttSearchCount.TabIndex = 8
        Me.lblAttSearchCount.Text = "0 items found"
        Me.lblAttSearchCount.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblAttSearch
        '
        Me.lblAttSearch.AutoSize = True
        Me.lblAttSearch.Location = New System.Drawing.Point(6, 3)
        Me.lblAttSearch.Name = "lblAttSearch"
        Me.lblAttSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblAttSearch.TabIndex = 6
        Me.lblAttSearch.Text = "Search:"
        '
        'tabWantedList
        '
        Me.tabWantedList.Controls.Add(Me.btnRefreshWantedList)
        Me.tabWantedList.Controls.Add(Me.btnClearWantedList)
        Me.tabWantedList.Controls.Add(Me.btnRemoveWantedItem)
        Me.tabWantedList.Controls.Add(Me.lvwWanted)
        Me.tabWantedList.Location = New System.Drawing.Point(4, 22)
        Me.tabWantedList.Name = "tabWantedList"
        Me.tabWantedList.Size = New System.Drawing.Size(384, 489)
        Me.tabWantedList.TabIndex = 3
        Me.tabWantedList.Text = "Wanted List"
        Me.tabWantedList.UseVisualStyleBackColor = True
        '
        'btnRefreshWantedList
        '
        Me.btnRefreshWantedList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshWantedList.Location = New System.Drawing.Point(220, 463)
        Me.btnRefreshWantedList.Name = "btnRefreshWantedList"
        Me.btnRefreshWantedList.Size = New System.Drawing.Size(100, 23)
        Me.btnRefreshWantedList.TabIndex = 15
        Me.btnRefreshWantedList.Text = "Refresh List"
        Me.btnRefreshWantedList.UseVisualStyleBackColor = True
        '
        'btnClearWantedList
        '
        Me.btnClearWantedList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearWantedList.Location = New System.Drawing.Point(114, 463)
        Me.btnClearWantedList.Name = "btnClearWantedList"
        Me.btnClearWantedList.Size = New System.Drawing.Size(100, 23)
        Me.btnClearWantedList.TabIndex = 14
        Me.btnClearWantedList.Text = "Clear List"
        Me.btnClearWantedList.UseVisualStyleBackColor = True
        '
        'btnRemoveWantedItem
        '
        Me.btnRemoveWantedItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveWantedItem.Location = New System.Drawing.Point(8, 463)
        Me.btnRemoveWantedItem.Name = "btnRemoveWantedItem"
        Me.btnRemoveWantedItem.Size = New System.Drawing.Size(100, 23)
        Me.btnRemoveWantedItem.TabIndex = 13
        Me.btnRemoveWantedItem.Text = "Remove Item"
        Me.btnRemoveWantedItem.UseVisualStyleBackColor = True
        '
        'lvwWanted
        '
        Me.lvwWanted.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwWanted.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colWantedName, Me.colWantedPrice})
        Me.lvwWanted.FullRowSelect = True
        Me.lvwWanted.GridLines = True
        Me.lvwWanted.Location = New System.Drawing.Point(3, 3)
        Me.lvwWanted.Name = "lvwWanted"
        Me.lvwWanted.ShowItemToolTips = True
        Me.lvwWanted.Size = New System.Drawing.Size(378, 454)
        Me.lvwWanted.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwWanted.TabIndex = 12
        Me.lvwWanted.UseCompatibleStateImageBehavior = False
        Me.lvwWanted.View = System.Windows.Forms.View.Details
        '
        'colWantedName
        '
        Me.colWantedName.Text = "Item Name"
        Me.colWantedName.Width = 210
        '
        'colWantedPrice
        '
        Me.colWantedPrice.Text = "Price"
        Me.colWantedPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colWantedPrice.Width = 120
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage8)
        Me.TabControl1.Controls.Add(Me.TabPage9)
        Me.TabControl1.Controls.Add(Me.TabPage10)
        Me.TabControl1.Location = New System.Drawing.Point(4, 4)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(452, 282)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label1)
        Me.TabPage2.Controls.Add(Me.NumericUpDown1)
        Me.TabPage2.Controls.Add(Me.ListView1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(444, 256)
        Me.TabPage2.TabIndex = 0
        Me.TabPage2.Text = "Manufacturing"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 232)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "ME Level:"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NumericUpDown1.Location = New System.Drawing.Point(68, 230)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(73, 20)
        Me.NumericUpDown1.TabIndex = 3
        Me.NumericUpDown1.ThousandsSeparator = True
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader19, Me.ColumnHeader20, Me.ColumnHeader21, Me.ColumnHeader22})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(6, 6)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(432, 220)
        Me.ListView1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView1.TabIndex = 2
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader19
        '
        Me.ColumnHeader19.Text = "Material"
        Me.ColumnHeader19.Width = 175
        '
        'ColumnHeader20
        '
        Me.ColumnHeader20.Text = "Perfect"
        Me.ColumnHeader20.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader20.Width = 75
        '
        'ColumnHeader21
        '
        Me.ColumnHeader21.Text = "ME 0"
        Me.ColumnHeader21.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader21.Width = 75
        '
        'ColumnHeader22
        '
        Me.ColumnHeader22.Text = "Pilot"
        Me.ColumnHeader22.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader22.Width = 75
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.ListView2)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(444, 256)
        Me.TabPage3.TabIndex = 1
        Me.TabPage3.Text = "Research Tech"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'ListView2
        '
        Me.ListView2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader23, Me.ColumnHeader24})
        Me.ListView2.FullRowSelect = True
        Me.ListView2.GridLines = True
        Me.ListView2.Location = New System.Drawing.Point(6, 6)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(432, 244)
        Me.ListView2.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView2.TabIndex = 3
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader23
        '
        Me.ColumnHeader23.Text = "Material"
        Me.ColumnHeader23.Width = 250
        '
        'ColumnHeader24
        '
        Me.ColumnHeader24.Text = "Quantity"
        Me.ColumnHeader24.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader24.Width = 150
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.ListView3)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(444, 256)
        Me.TabPage4.TabIndex = 2
        Me.TabPage4.Text = "Research PE"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'ListView3
        '
        Me.ListView3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader25, Me.ColumnHeader26})
        Me.ListView3.FullRowSelect = True
        Me.ListView3.GridLines = True
        Me.ListView3.Location = New System.Drawing.Point(6, 6)
        Me.ListView3.Name = "ListView3"
        Me.ListView3.Size = New System.Drawing.Size(435, 244)
        Me.ListView3.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView3.TabIndex = 3
        Me.ListView3.UseCompatibleStateImageBehavior = False
        Me.ListView3.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader25
        '
        Me.ColumnHeader25.Text = "Material"
        Me.ColumnHeader25.Width = 250
        '
        'ColumnHeader26
        '
        Me.ColumnHeader26.Text = "Quantity"
        Me.ColumnHeader26.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader26.Width = 150
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.ListView4)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(444, 256)
        Me.TabPage5.TabIndex = 3
        Me.TabPage5.Text = "Research ME"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'ListView4
        '
        Me.ListView4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView4.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader27, Me.ColumnHeader28})
        Me.ListView4.FullRowSelect = True
        Me.ListView4.GridLines = True
        Me.ListView4.Location = New System.Drawing.Point(6, 6)
        Me.ListView4.Name = "ListView4"
        Me.ListView4.Size = New System.Drawing.Size(435, 244)
        Me.ListView4.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView4.TabIndex = 3
        Me.ListView4.UseCompatibleStateImageBehavior = False
        Me.ListView4.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader27
        '
        Me.ColumnHeader27.Text = "Material"
        Me.ColumnHeader27.Width = 250
        '
        'ColumnHeader28
        '
        Me.ColumnHeader28.Text = "Quantity"
        Me.ColumnHeader28.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader28.Width = 150
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.ListView5)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(444, 256)
        Me.TabPage6.TabIndex = 4
        Me.TabPage6.Text = "Copying"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'ListView5
        '
        Me.ListView5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView5.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader29, Me.ColumnHeader30})
        Me.ListView5.FullRowSelect = True
        Me.ListView5.GridLines = True
        Me.ListView5.Location = New System.Drawing.Point(6, 6)
        Me.ListView5.Name = "ListView5"
        Me.ListView5.Size = New System.Drawing.Size(435, 244)
        Me.ListView5.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView5.TabIndex = 3
        Me.ListView5.UseCompatibleStateImageBehavior = False
        Me.ListView5.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader29
        '
        Me.ColumnHeader29.Text = "Material"
        Me.ColumnHeader29.Width = 250
        '
        'ColumnHeader30
        '
        Me.ColumnHeader30.Text = "Quantity"
        Me.ColumnHeader30.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader30.Width = 150
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.ListView6)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(444, 256)
        Me.TabPage7.TabIndex = 5
        Me.TabPage7.Text = "Duplicating"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'ListView6
        '
        Me.ListView6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView6.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader31, Me.ColumnHeader32})
        Me.ListView6.FullRowSelect = True
        Me.ListView6.GridLines = True
        Me.ListView6.Location = New System.Drawing.Point(6, 6)
        Me.ListView6.Name = "ListView6"
        Me.ListView6.Size = New System.Drawing.Size(435, 244)
        Me.ListView6.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView6.TabIndex = 3
        Me.ListView6.UseCompatibleStateImageBehavior = False
        Me.ListView6.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader31
        '
        Me.ColumnHeader31.Text = "Material"
        Me.ColumnHeader31.Width = 250
        '
        'ColumnHeader32
        '
        Me.ColumnHeader32.Text = "Quantity"
        Me.ColumnHeader32.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader32.Width = 150
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.ListView7)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(444, 256)
        Me.TabPage8.TabIndex = 6
        Me.TabPage8.Text = "Reverse Eng"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'ListView7
        '
        Me.ListView7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView7.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader33, Me.ColumnHeader34})
        Me.ListView7.FullRowSelect = True
        Me.ListView7.GridLines = True
        Me.ListView7.Location = New System.Drawing.Point(6, 6)
        Me.ListView7.Name = "ListView7"
        Me.ListView7.Size = New System.Drawing.Size(435, 244)
        Me.ListView7.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView7.TabIndex = 3
        Me.ListView7.UseCompatibleStateImageBehavior = False
        Me.ListView7.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader33
        '
        Me.ColumnHeader33.Text = "Material"
        Me.ColumnHeader33.Width = 250
        '
        'ColumnHeader34
        '
        Me.ColumnHeader34.Text = "Quantity"
        Me.ColumnHeader34.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader34.Width = 150
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.ListView8)
        Me.TabPage9.Location = New System.Drawing.Point(4, 22)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Size = New System.Drawing.Size(444, 256)
        Me.TabPage9.TabIndex = 7
        Me.TabPage9.Text = "Invention"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'ListView8
        '
        Me.ListView8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView8.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader35, Me.ColumnHeader36})
        Me.ListView8.FullRowSelect = True
        Me.ListView8.GridLines = True
        Me.ListView8.Location = New System.Drawing.Point(6, 6)
        Me.ListView8.Name = "ListView8"
        Me.ListView8.Size = New System.Drawing.Size(435, 244)
        Me.ListView8.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView8.TabIndex = 3
        Me.ListView8.UseCompatibleStateImageBehavior = False
        Me.ListView8.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader35
        '
        Me.ColumnHeader35.Text = "Material"
        Me.ColumnHeader35.Width = 250
        '
        'ColumnHeader36
        '
        Me.ColumnHeader36.Text = "Quantity"
        Me.ColumnHeader36.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader36.Width = 150
        '
        'TabPage10
        '
        Me.TabPage10.Controls.Add(Me.ListView9)
        Me.TabPage10.Location = New System.Drawing.Point(4, 22)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Size = New System.Drawing.Size(444, 256)
        Me.TabPage10.TabIndex = 8
        Me.TabPage10.Text = "Composition"
        Me.TabPage10.UseVisualStyleBackColor = True
        '
        'ListView9
        '
        Me.ListView9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView9.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader37, Me.ColumnHeader38})
        Me.ListView9.FullRowSelect = True
        Me.ListView9.GridLines = True
        Me.ListView9.Location = New System.Drawing.Point(5, 6)
        Me.ListView9.Name = "ListView9"
        Me.ListView9.Size = New System.Drawing.Size(435, 244)
        Me.ListView9.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView9.TabIndex = 4
        Me.ListView9.UseCompatibleStateImageBehavior = False
        Me.ListView9.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader37
        '
        Me.ColumnHeader37.Text = "Material"
        Me.ColumnHeader37.Width = 250
        '
        'ColumnHeader38
        '
        Me.ColumnHeader38.Text = "Quantity"
        Me.ColumnHeader38.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader38.Width = 150
        '
        'ctxBack
        '
        Me.ctxBack.Name = "ctxBack"
        Me.ctxBack.Size = New System.Drawing.Size(61, 4)
        '
        'ctxForward
        '
        Me.ctxForward.Name = "ctxForward"
        Me.ctxForward.Size = New System.Drawing.Size(61, 4)
        '
        'lblUsableTime
        '
        Me.lblUsableTime.AutoSize = True
        Me.lblUsableTime.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsableTime.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblUsableTime.Location = New System.Drawing.Point(545, 150)
        Me.lblUsableTime.Name = "lblUsableTime"
        Me.lblUsableTime.Size = New System.Drawing.Size(68, 13)
        Me.lblUsableTime.TabIndex = 12
        Me.lblUsableTime.TabStop = True
        Me.lblUsableTime.Text = "Usable Time:"
        '
        'btnWantedAdd
        '
        Me.btnWantedAdd.Location = New System.Drawing.Point(823, 12)
        Me.btnWantedAdd.Name = "btnWantedAdd"
        Me.btnWantedAdd.Size = New System.Drawing.Size(100, 23)
        Me.btnWantedAdd.TabIndex = 14
        Me.btnWantedAdd.Text = "Add to Wanted"
        Me.btnWantedAdd.UseVisualStyleBackColor = True
        '
        'cboPilots
        '
        Me.cboPilots.DropDownHeight = 250
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.IntegralHeight = False
        Me.cboPilots.Location = New System.Drawing.Point(632, 14)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(175, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 43
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(596, 17)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 42
        Me.lblPilot.Text = "Pilot:"
        '
        'sbtnBack
        '
        Me.sbtnBack.AutoSize = True
        Me.sbtnBack.ContextMenuStrip = Me.ctxBack
        Me.sbtnBack.Enabled = False
        Me.sbtnBack.Location = New System.Drawing.Point(411, 12)
        Me.sbtnBack.Name = "sbtnBack"
        Me.sbtnBack.Size = New System.Drawing.Size(60, 23)
        Me.sbtnBack.SplitMenu = Me.ctxBack
        Me.sbtnBack.TabIndex = 10
        Me.sbtnBack.Text = "Back"
        Me.sbtnBack.UseVisualStyleBackColor = True
        '
        'sbtnForward
        '
        Me.sbtnForward.AutoSize = True
        Me.sbtnForward.ContextMenuStrip = Me.ctxForward
        Me.sbtnForward.Enabled = False
        Me.sbtnForward.Location = New System.Drawing.Point(477, 12)
        Me.sbtnForward.Name = "sbtnForward"
        Me.sbtnForward.Size = New System.Drawing.Size(62, 23)
        Me.sbtnForward.SplitMenu = Me.ctxForward
        Me.sbtnForward.TabIndex = 11
        Me.sbtnForward.Text = "Forward"
        Me.sbtnForward.UseVisualStyleBackColor = True
        '
        'frmItemBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(997, 560)
        Me.Controls.Add(Me.tabBrowser)
        Me.Controls.Add(Me.ssData)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblPilot)
        Me.Controls.Add(Me.lblUsableTime)
        Me.Controls.Add(Me.sbtnBack)
        Me.Controls.Add(Me.sbtnForward)
        Me.Controls.Add(Me.btnWantedAdd)
        Me.Controls.Add(Me.tabItem)
        Me.Controls.Add(Me.picItem)
        Me.Controls.Add(Me.lblUsable)
        Me.Controls.Add(Me.lblItem)
        Me.Controls.Add(Me.picBP)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(760, 530)
        Me.Name = "frmItemBrowser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EveHQ Item Browser"
        Me.ctxSkills.ResumeLayout(False)
        Me.ssData.ResumeLayout(False)
        Me.ssData.PerformLayout()
        CType(Me.picBP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMaterials.ResumeLayout(False)
        Me.tabMaterial.ResumeLayout(False)
        Me.tabM1.ResumeLayout(False)
        Me.tabM1.PerformLayout()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabM2.ResumeLayout(False)
        Me.tabM3.ResumeLayout(False)
        Me.tabM4.ResumeLayout(False)
        Me.tabM5.ResumeLayout(False)
        Me.tabM6.ResumeLayout(False)
        Me.tabM7.ResumeLayout(False)
        Me.tabM8.ResumeLayout(False)
        Me.tabM9.ResumeLayout(False)
        Me.tabVariations.ResumeLayout(False)
        Me.tabVariation.ResumeLayout(False)
        Me.tabMetaVariations.ResumeLayout(False)
        Me.tabComparisons.ResumeLayout(False)
        Me.tabComparisons.PerformLayout()
        Me.tabFitting.ResumeLayout(False)
        Me.tabSkills.ResumeLayout(False)
        Me.tabAttributes.ResumeLayout(False)
        Me.tabDescription.ResumeLayout(False)
        Me.tabItem.ResumeLayout(False)
        Me.tabRecommended.ResumeLayout(False)
        Me.tabComponent.ResumeLayout(False)
        Me.tabComponents.ResumeLayout(False)
        Me.tabC1.ResumeLayout(False)
        Me.tabC1.PerformLayout()
        CType(Me.nudMELevelC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabC2.ResumeLayout(False)
        Me.tabC3.ResumeLayout(False)
        Me.tabC4.ResumeLayout(False)
        Me.tabC5.ResumeLayout(False)
        Me.tabC6.ResumeLayout(False)
        Me.tabC7.ResumeLayout(False)
        Me.tabC8.ResumeLayout(False)
        Me.tabC9.ResumeLayout(False)
        Me.tabDepends.ResumeLayout(False)
        Me.tabEveCentral.ResumeLayout(False)
        Me.tabInsurance.ResumeLayout(False)
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabBrowser.ResumeLayout(False)
        Me.tabBrowse.ResumeLayout(False)
        Me.tabBrowse.PerformLayout()
        Me.tabSearch.ResumeLayout(False)
        Me.tabSearch.PerformLayout()
        Me.tabAttSearch.ResumeLayout(False)
        Me.tabAttSearch.PerformLayout()
        Me.tabWantedList.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage9.ResumeLayout(False)
        Me.TabPage10.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ctxSkills As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuViewDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ssData As System.Windows.Forms.StatusStrip
    Friend WithEvents ssLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ssLblID As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ItemToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents SkillToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents picBP As System.Windows.Forms.PictureBox
    Friend WithEvents tabMaterials As System.Windows.Forms.TabPage
    Friend WithEvents tabMaterial As System.Windows.Forms.TabControl
    Friend WithEvents tabM1 As System.Windows.Forms.TabPage
    Friend WithEvents lblMELevel As System.Windows.Forms.Label
    Friend WithEvents nudMELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents lstM1 As System.Windows.Forms.ListView
    Friend WithEvents colM1M As System.Windows.Forms.ColumnHeader
    Friend WithEvents colM1Q As System.Windows.Forms.ColumnHeader
    Friend WithEvents colM1ME As System.Windows.Forms.ColumnHeader
    Friend WithEvents colM1P As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabM2 As System.Windows.Forms.TabPage
    Friend WithEvents lstM2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabM3 As System.Windows.Forms.TabPage
    Friend WithEvents lstM3 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabM4 As System.Windows.Forms.TabPage
    Friend WithEvents lstM4 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabM5 As System.Windows.Forms.TabPage
    Friend WithEvents lstM5 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabM6 As System.Windows.Forms.TabPage
    Friend WithEvents lstM6 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabM7 As System.Windows.Forms.TabPage
    Friend WithEvents lstM7 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabM8 As System.Windows.Forms.TabPage
    Friend WithEvents lstM8 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader16 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabM9 As System.Windows.Forms.TabPage
    Friend WithEvents lstM9 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader17 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader18 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabVariations As System.Windows.Forms.TabPage
    Friend WithEvents lstVariations As System.Windows.Forms.ListView
    Friend WithEvents colTypeName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colMetaTypeName As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabFitting As System.Windows.Forms.TabPage
    Friend WithEvents lstFitting As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabSkills As System.Windows.Forms.TabPage
    Friend WithEvents btnViewSkills As System.Windows.Forms.Button
    Friend WithEvents btnAddSkills As System.Windows.Forms.Button
    Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
    Friend WithEvents tabAttributes As System.Windows.Forms.TabPage
    Friend WithEvents lstAttributes As System.Windows.Forms.ListView
    Friend WithEvents colAttribute As System.Windows.Forms.ColumnHeader
    Friend WithEvents colData As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabDescription As System.Windows.Forms.TabPage
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents tabItem As System.Windows.Forms.TabControl
    Friend WithEvents picItem As System.Windows.Forms.PictureBox
    Friend WithEvents lblItem As System.Windows.Forms.Label
    Friend WithEvents lblUsable As System.Windows.Forms.Label
    Friend WithEvents tabBrowser As System.Windows.Forms.TabControl
    Friend WithEvents tabSearch As System.Windows.Forms.TabPage
    Friend WithEvents lstSearch As System.Windows.Forms.ListBox
    Friend WithEvents lblSearchCount As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents tabBrowse As System.Windows.Forms.TabPage
    Friend WithEvents tvwBrowse As System.Windows.Forms.TreeView
    Friend WithEvents ssDBLocation As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tabComponent As System.Windows.Forms.TabPage
    Friend WithEvents tabComponents As System.Windows.Forms.TabControl
    Friend WithEvents tabC1 As System.Windows.Forms.TabPage
    Friend WithEvents lblMELevelC As System.Windows.Forms.Label
    Friend WithEvents nudMELevelC As System.Windows.Forms.NumericUpDown
    Friend WithEvents lstC1 As System.Windows.Forms.ListView
    Friend WithEvents colC1M As System.Windows.Forms.ColumnHeader
    Friend WithEvents colC1Q As System.Windows.Forms.ColumnHeader
    Friend WithEvents colC1ME As System.Windows.Forms.ColumnHeader
    Friend WithEvents colC1P As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabC2 As System.Windows.Forms.TabPage
    Friend WithEvents lstC2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader43 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader44 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabC3 As System.Windows.Forms.TabPage
    Friend WithEvents lstC3 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader45 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader46 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabC4 As System.Windows.Forms.TabPage
    Friend WithEvents lstC4 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader47 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader48 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabC5 As System.Windows.Forms.TabPage
    Friend WithEvents lstC5 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader49 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader50 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabC6 As System.Windows.Forms.TabPage
    Friend WithEvents lstC6 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader51 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader52 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabC7 As System.Windows.Forms.TabPage
    Friend WithEvents lstC7 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader53 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader54 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabC8 As System.Windows.Forms.TabPage
    Friend WithEvents lstC8 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader55 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader56 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabC9 As System.Windows.Forms.TabPage
    Friend WithEvents lstC9 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader57 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader58 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader19 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader20 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader21 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader22 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader23 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader24 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents ListView3 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader25 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader26 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents ListView4 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader27 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader28 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents ListView5 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader29 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader30 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents ListView6 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader31 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader32 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents ListView7 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader33 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader34 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents ListView8 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader35 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader36 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabPage10 As System.Windows.Forms.TabPage
    Friend WithEvents ListView9 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader37 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader38 As System.Windows.Forms.ColumnHeader
    Friend WithEvents sbtnBack As EveHQ.ItemBrowser.SplitButton
    Friend WithEvents sbtnForward As EveHQ.ItemBrowser.SplitButton
    Friend WithEvents ctxBack As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ctxForward As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tabAttSearch As System.Windows.Forms.TabPage
    Friend WithEvents cboAttSearch As System.Windows.Forms.ComboBox
    Friend WithEvents lblAttSearchCount As System.Windows.Forms.Label
    Friend WithEvents lblAttSearch As System.Windows.Forms.Label
    Friend WithEvents lstAttSearch As System.Windows.Forms.ListView
    Friend WithEvents colAttName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAttValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabVariation As System.Windows.Forms.TabControl
    Friend WithEvents tabMetaVariations As System.Windows.Forms.TabPage
    Friend WithEvents tabComparisons As System.Windows.Forms.TabPage
    Friend WithEvents lstComparisons As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader39 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader40 As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkShowAllColumns As System.Windows.Forms.CheckBox
    Friend WithEvents tabEveCentral As System.Windows.Forms.TabPage
    Friend WithEvents lstEveCentral As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader41 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader42 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblUsableTime As System.Windows.Forms.LinkLabel
    Friend WithEvents tabDepends As System.Windows.Forms.TabPage
    Friend WithEvents lvwDepend As System.Windows.Forms.ListView
    Friend WithEvents NeededFor As System.Windows.Forms.ColumnHeader
    Friend WithEvents NeededLevel As System.Windows.Forms.ColumnHeader
    Friend WithEvents NeededGroup As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkBrowseNonPublished As System.Windows.Forms.CheckBox
    Friend WithEvents tabWantedList As System.Windows.Forms.TabPage
    Friend WithEvents lvwWanted As System.Windows.Forms.ListView
    Friend WithEvents colWantedName As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRemoveWantedItem As System.Windows.Forms.Button
    Friend WithEvents btnWantedAdd As System.Windows.Forms.Button
    Friend WithEvents btnClearWantedList As System.Windows.Forms.Button
    Friend WithEvents colWantedPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRefreshWantedList As System.Windows.Forms.Button
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents tabInsurance As System.Windows.Forms.TabPage
    Friend WithEvents lstInsurance As System.Windows.Forms.ListView
    Friend WithEvents Type As System.Windows.Forms.ColumnHeader
    Friend WithEvents Fee As System.Windows.Forms.ColumnHeader
    Friend WithEvents Payout As System.Windows.Forms.ColumnHeader
    Friend WithEvents PayoutProfit As System.Windows.Forms.ColumnHeader
    Friend WithEvents MarketPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabRecommended As System.Windows.Forms.TabPage
    Friend WithEvents lvwRecommended As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader59 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader60 As System.Windows.Forms.ColumnHeader
    Friend WithEvents imgListCerts As System.Windows.Forms.ImageList
End Class
