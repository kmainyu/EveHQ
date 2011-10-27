<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemBrowser
    Inherits DevComponents.DotNetBar.Office2007Form

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemBrowser))
        Me.ctxSkills = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSkillName = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuViewDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.ItemToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SkillToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.picBP = New System.Windows.Forms.PictureBox()
        Me.lblMELevel = New System.Windows.Forms.Label()
        Me.nudMELevel = New System.Windows.Forms.NumericUpDown()
        Me.lstM1 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colM1M = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colM1Q = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colM1ME = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colM1P = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstM2 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstM3 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstM4 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstM5 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstM6 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstM7 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstM8 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstM9 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader18 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstVariations = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colTypeName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colMetaTypeName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chkShowAllColumns = New System.Windows.Forms.CheckBox()
        Me.lstComparisons = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader39 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader40 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstFitting = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tvwReqs = New System.Windows.Forms.TreeView()
        Me.lstAttributes = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colAttribute = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colData = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lvwRecommended = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader59 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader60 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imgListCerts = New System.Windows.Forms.ImageList(Me.components)
        Me.lvwDepend = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.NeededFor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.NeededGroup = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.NeededLevel = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstEveCentral = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader41 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader42 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.picItem = New System.Windows.Forms.PictureBox()
        Me.lblItem = New System.Windows.Forms.Label()
        Me.lblUsable = New System.Windows.Forms.Label()
        Me.chkBrowseNonPublished = New System.Windows.Forms.CheckBox()
        Me.tvwBrowse = New System.Windows.Forms.TreeView()
        Me.lstSearch = New System.Windows.Forms.ListBox()
        Me.lblSearchCount = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.lstAttSearch = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colAttName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colAttValue = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cboAttSearch = New System.Windows.Forms.ComboBox()
        Me.lblAttSearchCount = New System.Windows.Forms.Label()
        Me.lblAttSearch = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.ListView1 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader19 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader20 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader21 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader22 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.ListView2 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader23 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader24 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.ListView3 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader25 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader26 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.ListView4 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader27 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader28 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.ListView5 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader29 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader30 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.ListView6 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader31 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader32 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.ListView7 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader33 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader34 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage9 = New System.Windows.Forms.TabPage()
        Me.ListView8 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader35 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader36 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage10 = New System.Windows.Forms.TabPage()
        Me.ListView9 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader37 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader38 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblUsableTime = New System.Windows.Forms.LinkLabel()
        Me.tabBrowser = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tabSearch = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tabAttSearch = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel35 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstEffectSearch = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader66 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblEffectSearch = New System.Windows.Forms.Label()
        Me.cboEffectSearch = New System.Windows.Forms.ComboBox()
        Me.lblEffectSearchCount = New System.Windows.Forms.Label()
        Me.tabEffectSearch = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tabBrowse = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.cboPilots = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.lblPilot = New DevComponents.DotNetBar.LabelX()
        Me.btnNavForward = New DevComponents.DotNetBar.ButtonX()
        Me.btnRequisition = New DevComponents.DotNetBar.ButtonX()
        Me.barStatus = New DevComponents.DotNetBar.Bar()
        Me.lblStatus = New DevComponents.DotNetBar.LabelItem()
        Me.lblDBLocation = New DevComponents.DotNetBar.LabelItem()
        Me.lblID = New DevComponents.DotNetBar.LabelItem()
        Me.panelIB = New DevComponents.DotNetBar.PanelEx()
        Me.btnNavBack = New DevComponents.DotNetBar.ButtonX()
        Me.tabIB = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiDescription = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel6 = New DevComponents.DotNetBar.TabControlPanel()
        Me.btnAddSkills = New DevComponents.DotNetBar.ButtonX()
        Me.btnViewSkills = New DevComponents.DotNetBar.ButtonX()
        Me.tiSkills = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel34 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstEffects = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader63 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader65 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiEffects = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel5 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiAttributes = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel13 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiEveCentral = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel11 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tcComponents = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel33 = New DevComponents.DotNetBar.TabControlPanel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudMELevelC = New System.Windows.Forms.NumericUpDown()
        Me.lstC1 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader61 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader62 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colC1ME = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader64 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC1 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel26 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstC8 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader45 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader46 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC8 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel27 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstC7 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader47 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader48 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC7 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel25 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstC9 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader43 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader44 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC9 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel28 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstC6 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader49 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader50 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC6 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel29 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstC5 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader51 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader52 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC5 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel30 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstC4 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader53 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader54 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC4 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel31 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstC3 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader55 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader56 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC3 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel32 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lstC2 = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.ColumnHeader57 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader58 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tiC2 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tiComponent = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel10 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tcMaterials = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel16 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM1 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel24 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM9 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel23 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM8 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel22 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM7 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel21 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM6 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel20 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM5 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel19 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM4 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel18 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM3 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel17 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiM2 = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tiMaterials = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel9 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiRecommended = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel8 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tcVariations = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel14 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiVariation = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel15 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiComparison = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tiVariations = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel7 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiFitting = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel12 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiDependencies = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.ctxSkills.SuspendLayout()
        CType(Me.picBP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).BeginInit()
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
        CType(Me.tabBrowser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabBrowser.SuspendLayout()
        Me.TabControlPanel2.SuspendLayout()
        Me.TabControlPanel3.SuspendLayout()
        Me.TabControlPanel35.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        CType(Me.barStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelIB.SuspendLayout()
        CType(Me.tabIB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabIB.SuspendLayout()
        Me.TabControlPanel4.SuspendLayout()
        Me.TabControlPanel6.SuspendLayout()
        Me.TabControlPanel34.SuspendLayout()
        Me.TabControlPanel5.SuspendLayout()
        Me.TabControlPanel13.SuspendLayout()
        Me.TabControlPanel11.SuspendLayout()
        CType(Me.tcComponents, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcComponents.SuspendLayout()
        Me.TabControlPanel33.SuspendLayout()
        CType(Me.nudMELevelC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel26.SuspendLayout()
        Me.TabControlPanel27.SuspendLayout()
        Me.TabControlPanel25.SuspendLayout()
        Me.TabControlPanel28.SuspendLayout()
        Me.TabControlPanel29.SuspendLayout()
        Me.TabControlPanel30.SuspendLayout()
        Me.TabControlPanel31.SuspendLayout()
        Me.TabControlPanel32.SuspendLayout()
        Me.TabControlPanel10.SuspendLayout()
        CType(Me.tcMaterials, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcMaterials.SuspendLayout()
        Me.TabControlPanel16.SuspendLayout()
        Me.TabControlPanel24.SuspendLayout()
        Me.TabControlPanel23.SuspendLayout()
        Me.TabControlPanel22.SuspendLayout()
        Me.TabControlPanel21.SuspendLayout()
        Me.TabControlPanel20.SuspendLayout()
        Me.TabControlPanel19.SuspendLayout()
        Me.TabControlPanel18.SuspendLayout()
        Me.TabControlPanel17.SuspendLayout()
        Me.TabControlPanel9.SuspendLayout()
        Me.TabControlPanel8.SuspendLayout()
        CType(Me.tcVariations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcVariations.SuspendLayout()
        Me.TabControlPanel14.SuspendLayout()
        Me.TabControlPanel15.SuspendLayout()
        Me.TabControlPanel7.SuspendLayout()
        Me.TabControlPanel12.SuspendLayout()
        Me.SuspendLayout()
        '
        'ctxSkills
        '
        Me.ctxSkills.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ctxSkills.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSkillName, Me.ToolStripSeparator1, Me.mnuViewDetails})
        Me.ctxSkills.Name = "ctxDepend"
        Me.ctxSkills.Size = New System.Drawing.Size(133, 54)
        '
        'mnuSkillName
        '
        Me.mnuSkillName.Name = "mnuSkillName"
        Me.mnuSkillName.Size = New System.Drawing.Size(132, 22)
        Me.mnuSkillName.Text = "Skill Name"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(129, 6)
        '
        'mnuViewDetails
        '
        Me.mnuViewDetails.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.mnuViewDetails.Name = "mnuViewDetails"
        Me.mnuViewDetails.Size = New System.Drawing.Size(132, 22)
        Me.mnuViewDetails.Text = "View Details"
        '
        'picBP
        '
        Me.picBP.Location = New System.Drawing.Point(437, 55)
        Me.picBP.Name = "picBP"
        Me.picBP.Size = New System.Drawing.Size(32, 32)
        Me.picBP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picBP.TabIndex = 3
        Me.picBP.TabStop = False
        Me.picBP.Visible = False
        '
        'lblMELevel
        '
        Me.lblMELevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMELevel.AutoSize = True
        Me.lblMELevel.BackColor = System.Drawing.Color.Transparent
        Me.lblMELevel.Location = New System.Drawing.Point(4, 363)
        Me.lblMELevel.Name = "lblMELevel"
        Me.lblMELevel.Size = New System.Drawing.Size(53, 13)
        Me.lblMELevel.TabIndex = 4
        Me.lblMELevel.Text = "ME Level:"
        '
        'nudMELevel
        '
        Me.nudMELevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.nudMELevel.Location = New System.Drawing.Point(65, 361)
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
        '
        '
        '
        Me.lstM1.Border.Class = "ListViewBorder"
        Me.lstM1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colM1M, Me.colM1Q, Me.colM1ME, Me.colM1P})
        Me.lstM1.FullRowSelect = True
        Me.lstM1.GridLines = True
        Me.lstM1.Location = New System.Drawing.Point(4, 1)
        Me.lstM1.Name = "lstM1"
        Me.lstM1.Size = New System.Drawing.Size(901, 356)
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
        'lstM2
        '
        '
        '
        '
        Me.lstM2.Border.Class = "ListViewBorder"
        Me.lstM2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lstM2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM2.FullRowSelect = True
        Me.lstM2.GridLines = True
        Me.lstM2.Location = New System.Drawing.Point(1, 1)
        Me.lstM2.Name = "lstM2"
        Me.lstM2.Size = New System.Drawing.Size(910, 385)
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
        'lstM3
        '
        '
        '
        '
        Me.lstM3.Border.Class = "ListViewBorder"
        Me.lstM3.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lstM3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM3.FullRowSelect = True
        Me.lstM3.GridLines = True
        Me.lstM3.Location = New System.Drawing.Point(1, 1)
        Me.lstM3.Name = "lstM3"
        Me.lstM3.Size = New System.Drawing.Size(910, 385)
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
        'lstM4
        '
        '
        '
        '
        Me.lstM4.Border.Class = "ListViewBorder"
        Me.lstM4.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM4.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader7, Me.ColumnHeader8})
        Me.lstM4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM4.FullRowSelect = True
        Me.lstM4.GridLines = True
        Me.lstM4.Location = New System.Drawing.Point(1, 1)
        Me.lstM4.Name = "lstM4"
        Me.lstM4.Size = New System.Drawing.Size(910, 385)
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
        'lstM5
        '
        '
        '
        '
        Me.lstM5.Border.Class = "ListViewBorder"
        Me.lstM5.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM5.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader9, Me.ColumnHeader10})
        Me.lstM5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM5.FullRowSelect = True
        Me.lstM5.GridLines = True
        Me.lstM5.Location = New System.Drawing.Point(1, 1)
        Me.lstM5.Name = "lstM5"
        Me.lstM5.Size = New System.Drawing.Size(910, 385)
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
        'lstM6
        '
        '
        '
        '
        Me.lstM6.Border.Class = "ListViewBorder"
        Me.lstM6.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM6.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader11, Me.ColumnHeader12})
        Me.lstM6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM6.FullRowSelect = True
        Me.lstM6.GridLines = True
        Me.lstM6.Location = New System.Drawing.Point(1, 1)
        Me.lstM6.Name = "lstM6"
        Me.lstM6.Size = New System.Drawing.Size(910, 385)
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
        'lstM7
        '
        '
        '
        '
        Me.lstM7.Border.Class = "ListViewBorder"
        Me.lstM7.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM7.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader13, Me.ColumnHeader14})
        Me.lstM7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM7.FullRowSelect = True
        Me.lstM7.GridLines = True
        Me.lstM7.Location = New System.Drawing.Point(1, 1)
        Me.lstM7.Name = "lstM7"
        Me.lstM7.Size = New System.Drawing.Size(910, 385)
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
        'lstM8
        '
        '
        '
        '
        Me.lstM8.Border.Class = "ListViewBorder"
        Me.lstM8.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM8.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader15, Me.ColumnHeader16})
        Me.lstM8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM8.FullRowSelect = True
        Me.lstM8.GridLines = True
        Me.lstM8.Location = New System.Drawing.Point(1, 1)
        Me.lstM8.Name = "lstM8"
        Me.lstM8.Size = New System.Drawing.Size(910, 385)
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
        'lstM9
        '
        '
        '
        '
        Me.lstM9.Border.Class = "ListViewBorder"
        Me.lstM9.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstM9.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader17, Me.ColumnHeader18})
        Me.lstM9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstM9.FullRowSelect = True
        Me.lstM9.GridLines = True
        Me.lstM9.Location = New System.Drawing.Point(1, 1)
        Me.lstM9.Name = "lstM9"
        Me.lstM9.Size = New System.Drawing.Size(910, 385)
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
        'lstVariations
        '
        '
        '
        '
        Me.lstVariations.Border.Class = "ListViewBorder"
        Me.lstVariations.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstVariations.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTypeName, Me.colMetaTypeName})
        Me.lstVariations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstVariations.FullRowSelect = True
        Me.lstVariations.GridLines = True
        Me.lstVariations.Location = New System.Drawing.Point(1, 1)
        Me.lstVariations.Name = "lstVariations"
        Me.lstVariations.Size = New System.Drawing.Size(910, 385)
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
        'chkShowAllColumns
        '
        Me.chkShowAllColumns.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkShowAllColumns.AutoSize = True
        Me.chkShowAllColumns.BackColor = System.Drawing.Color.Transparent
        Me.chkShowAllColumns.Location = New System.Drawing.Point(4, 364)
        Me.chkShowAllColumns.Name = "chkShowAllColumns"
        Me.chkShowAllColumns.Size = New System.Drawing.Size(109, 17)
        Me.chkShowAllColumns.TabIndex = 2
        Me.chkShowAllColumns.Text = "Show All Columns"
        Me.chkShowAllColumns.UseVisualStyleBackColor = False
        '
        'lstComparisons
        '
        Me.lstComparisons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lstComparisons.Border.Class = "ListViewBorder"
        Me.lstComparisons.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstComparisons.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader39, Me.ColumnHeader40})
        Me.lstComparisons.FullRowSelect = True
        Me.lstComparisons.GridLines = True
        Me.lstComparisons.Location = New System.Drawing.Point(4, 4)
        Me.lstComparisons.Name = "lstComparisons"
        Me.lstComparisons.Size = New System.Drawing.Size(904, 354)
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
        'lstFitting
        '
        '
        '
        '
        Me.lstFitting.Border.Class = "ListViewBorder"
        Me.lstFitting.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstFitting.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lstFitting.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFitting.FullRowSelect = True
        Me.lstFitting.GridLines = True
        Me.lstFitting.Location = New System.Drawing.Point(1, 1)
        Me.lstFitting.Name = "lstFitting"
        Me.lstFitting.Size = New System.Drawing.Size(912, 410)
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
        'tvwReqs
        '
        Me.tvwReqs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwReqs.ContextMenuStrip = Me.ctxSkills
        Me.tvwReqs.Indent = 25
        Me.tvwReqs.ItemHeight = 20
        Me.tvwReqs.Location = New System.Drawing.Point(4, 4)
        Me.tvwReqs.Name = "tvwReqs"
        Me.tvwReqs.ShowPlusMinus = False
        Me.tvwReqs.Size = New System.Drawing.Size(906, 375)
        Me.tvwReqs.TabIndex = 1
        '
        'lstAttributes
        '
        '
        '
        '
        Me.lstAttributes.Border.Class = "ListViewBorder"
        Me.lstAttributes.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstAttributes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttribute, Me.colData})
        Me.lstAttributes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstAttributes.FullRowSelect = True
        Me.lstAttributes.GridLines = True
        Me.lstAttributes.Location = New System.Drawing.Point(1, 1)
        Me.lstAttributes.Name = "lstAttributes"
        Me.lstAttributes.ShowItemToolTips = True
        Me.lstAttributes.Size = New System.Drawing.Size(912, 410)
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
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.Color.Transparent
        Me.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDescription.Location = New System.Drawing.Point(1, 1)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(912, 410)
        Me.lblDescription.TabIndex = 0
        '
        'lvwRecommended
        '
        '
        '
        '
        Me.lvwRecommended.Border.Class = "ListViewBorder"
        Me.lvwRecommended.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwRecommended.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader59, Me.ColumnHeader60})
        Me.lvwRecommended.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwRecommended.FullRowSelect = True
        Me.lvwRecommended.GridLines = True
        Me.lvwRecommended.Location = New System.Drawing.Point(1, 1)
        Me.lvwRecommended.Name = "lvwRecommended"
        Me.lvwRecommended.Size = New System.Drawing.Size(912, 410)
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
        'lvwDepend
        '
        '
        '
        '
        Me.lvwDepend.Border.Class = "ListViewBorder"
        Me.lvwDepend.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwDepend.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NeededFor, Me.NeededGroup, Me.NeededLevel})
        Me.lvwDepend.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwDepend.FullRowSelect = True
        Me.lvwDepend.GridLines = True
        Me.lvwDepend.Location = New System.Drawing.Point(1, 1)
        Me.lvwDepend.Name = "lvwDepend"
        Me.lvwDepend.ShowItemToolTips = True
        Me.lvwDepend.Size = New System.Drawing.Size(912, 410)
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
        'lstEveCentral
        '
        '
        '
        '
        Me.lstEveCentral.Border.Class = "ListViewBorder"
        Me.lstEveCentral.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstEveCentral.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader41, Me.ColumnHeader42})
        Me.lstEveCentral.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstEveCentral.FullRowSelect = True
        Me.lstEveCentral.GridLines = True
        Me.lstEveCentral.Location = New System.Drawing.Point(1, 1)
        Me.lstEveCentral.Name = "lstEveCentral"
        Me.lstEveCentral.Size = New System.Drawing.Size(912, 410)
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
        'picItem
        '
        Me.picItem.ErrorImage = Global.EveHQ.ItemBrowser.My.Resources.Resources.noitem
        Me.picItem.InitialImage = Global.EveHQ.ItemBrowser.My.Resources.Resources.noitem
        Me.picItem.Location = New System.Drawing.Point(367, 23)
        Me.picItem.Name = "picItem"
        Me.picItem.Size = New System.Drawing.Size(64, 64)
        Me.picItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picItem.TabIndex = 0
        Me.picItem.TabStop = False
        '
        'lblItem
        '
        Me.lblItem.AutoEllipsis = True
        Me.lblItem.AutoSize = True
        Me.lblItem.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblItem.Location = New System.Drawing.Point(437, 23)
        Me.lblItem.Name = "lblItem"
        Me.lblItem.Size = New System.Drawing.Size(0, 18)
        Me.lblItem.TabIndex = 2
        '
        'lblUsable
        '
        Me.lblUsable.AutoSize = True
        Me.lblUsable.Location = New System.Drawing.Point(366, 149)
        Me.lblUsable.Name = "lblUsable"
        Me.lblUsable.Size = New System.Drawing.Size(39, 13)
        Me.lblUsable.TabIndex = 6
        Me.lblUsable.Text = "Usable"
        '
        'chkBrowseNonPublished
        '
        Me.chkBrowseNonPublished.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkBrowseNonPublished.AutoSize = True
        Me.chkBrowseNonPublished.BackColor = System.Drawing.Color.Transparent
        Me.chkBrowseNonPublished.Location = New System.Drawing.Point(4, 582)
        Me.chkBrowseNonPublished.Name = "chkBrowseNonPublished"
        Me.chkBrowseNonPublished.Size = New System.Drawing.Size(162, 17)
        Me.chkBrowseNonPublished.TabIndex = 1
        Me.chkBrowseNonPublished.Text = "Include Non-Published Items"
        Me.chkBrowseNonPublished.UseVisualStyleBackColor = False
        '
        'tvwBrowse
        '
        Me.tvwBrowse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwBrowse.HideSelection = False
        Me.tvwBrowse.Location = New System.Drawing.Point(4, 4)
        Me.tvwBrowse.Name = "tvwBrowse"
        Me.tvwBrowse.Size = New System.Drawing.Size(353, 572)
        Me.tvwBrowse.TabIndex = 0
        '
        'lstSearch
        '
        Me.lstSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSearch.FormattingEnabled = True
        Me.lstSearch.Location = New System.Drawing.Point(4, 47)
        Me.lstSearch.Name = "lstSearch"
        Me.lstSearch.Size = New System.Drawing.Size(352, 537)
        Me.lstSearch.TabIndex = 5
        '
        'lblSearchCount
        '
        Me.lblSearchCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchCount.BackColor = System.Drawing.Color.Transparent
        Me.lblSearchCount.Location = New System.Drawing.Point(220, 4)
        Me.lblSearchCount.Name = "lblSearchCount"
        Me.lblSearchCount.Size = New System.Drawing.Size(136, 14)
        Me.lblSearchCount.TabIndex = 4
        Me.lblSearchCount.Text = "0 items found"
        Me.lblSearchCount.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(4, 20)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(352, 21)
        Me.txtSearch.TabIndex = 2
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Location = New System.Drawing.Point(4, 4)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblSearch.TabIndex = 1
        Me.lblSearch.Text = "Search:"
        '
        'lstAttSearch
        '
        Me.lstAttSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lstAttSearch.Border.Class = "ListViewBorder"
        Me.lstAttSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstAttSearch.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttName, Me.colAttValue})
        Me.lstAttSearch.FullRowSelect = True
        Me.lstAttSearch.GridLines = True
        Me.lstAttSearch.Location = New System.Drawing.Point(4, 48)
        Me.lstAttSearch.Name = "lstAttSearch"
        Me.lstAttSearch.ShowItemToolTips = True
        Me.lstAttSearch.Size = New System.Drawing.Size(353, 551)
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
        Me.cboAttSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAttSearch.FormattingEnabled = True
        Me.cboAttSearch.Location = New System.Drawing.Point(4, 20)
        Me.cboAttSearch.Name = "cboAttSearch"
        Me.cboAttSearch.Size = New System.Drawing.Size(353, 21)
        Me.cboAttSearch.TabIndex = 10
        '
        'lblAttSearchCount
        '
        Me.lblAttSearchCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAttSearchCount.BackColor = System.Drawing.Color.Transparent
        Me.lblAttSearchCount.Location = New System.Drawing.Point(213, 3)
        Me.lblAttSearchCount.Name = "lblAttSearchCount"
        Me.lblAttSearchCount.Size = New System.Drawing.Size(144, 14)
        Me.lblAttSearchCount.TabIndex = 8
        Me.lblAttSearchCount.Text = "0 items found"
        Me.lblAttSearchCount.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblAttSearch
        '
        Me.lblAttSearch.AutoSize = True
        Me.lblAttSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblAttSearch.Location = New System.Drawing.Point(4, 4)
        Me.lblAttSearch.Name = "lblAttSearch"
        Me.lblAttSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblAttSearch.TabIndex = 6
        Me.lblAttSearch.Text = "Search:"
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
        '
        '
        '
        Me.ListView1.Border.Class = "ListViewBorder"
        Me.ListView1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        '
        '
        '
        Me.ListView2.Border.Class = "ListViewBorder"
        Me.ListView2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        '
        '
        '
        Me.ListView3.Border.Class = "ListViewBorder"
        Me.ListView3.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        '
        '
        '
        Me.ListView4.Border.Class = "ListViewBorder"
        Me.ListView4.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        '
        '
        '
        Me.ListView5.Border.Class = "ListViewBorder"
        Me.ListView5.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        '
        '
        '
        Me.ListView6.Border.Class = "ListViewBorder"
        Me.ListView6.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        '
        '
        '
        Me.ListView7.Border.Class = "ListViewBorder"
        Me.ListView7.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        '
        '
        '
        Me.ListView8.Border.Class = "ListViewBorder"
        Me.ListView8.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        '
        '
        '
        Me.ListView9.Border.Class = "ListViewBorder"
        Me.ListView9.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
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
        'lblUsableTime
        '
        Me.lblUsableTime.AutoSize = True
        Me.lblUsableTime.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsableTime.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblUsableTime.Location = New System.Drawing.Point(366, 167)
        Me.lblUsableTime.Name = "lblUsableTime"
        Me.lblUsableTime.Size = New System.Drawing.Size(68, 13)
        Me.lblUsableTime.TabIndex = 12
        Me.lblUsableTime.TabStop = True
        Me.lblUsableTime.Text = "Usable Time:"
        '
        'tabBrowser
        '
        Me.tabBrowser.BackColor = System.Drawing.Color.Transparent
        Me.tabBrowser.CanReorderTabs = True
        Me.tabBrowser.Controls.Add(Me.TabControlPanel2)
        Me.tabBrowser.Controls.Add(Me.TabControlPanel3)
        Me.tabBrowser.Controls.Add(Me.TabControlPanel35)
        Me.tabBrowser.Controls.Add(Me.TabControlPanel1)
        Me.tabBrowser.Dock = System.Windows.Forms.DockStyle.Left
        Me.tabBrowser.Location = New System.Drawing.Point(0, 0)
        Me.tabBrowser.Name = "tabBrowser"
        Me.tabBrowser.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tabBrowser.SelectedTabIndex = 0
        Me.tabBrowser.Size = New System.Drawing.Size(361, 626)
        Me.tabBrowser.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tabBrowser.TabIndex = 44
        Me.tabBrowser.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabBrowser.Tabs.Add(Me.tabSearch)
        Me.tabBrowser.Tabs.Add(Me.tabBrowse)
        Me.tabBrowser.Tabs.Add(Me.tabAttSearch)
        Me.tabBrowser.Tabs.Add(Me.tabEffectSearch)
        Me.tabBrowser.Text = "TabControl2"
        '
        'TabControlPanel2
        '
        Me.TabControlPanel2.Controls.Add(Me.lstSearch)
        Me.TabControlPanel2.Controls.Add(Me.lblSearch)
        Me.TabControlPanel2.Controls.Add(Me.lblSearchCount)
        Me.TabControlPanel2.Controls.Add(Me.txtSearch)
        Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel2.Name = "TabControlPanel2"
        Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel2.Size = New System.Drawing.Size(361, 603)
        Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel2.Style.GradientAngle = 90
        Me.TabControlPanel2.TabIndex = 2
        Me.TabControlPanel2.TabItem = Me.tabSearch
        '
        'tabSearch
        '
        Me.tabSearch.AttachedControl = Me.TabControlPanel2
        Me.tabSearch.Name = "tabSearch"
        Me.tabSearch.Text = "Search"
        '
        'TabControlPanel3
        '
        Me.TabControlPanel3.Controls.Add(Me.lstAttSearch)
        Me.TabControlPanel3.Controls.Add(Me.lblAttSearch)
        Me.TabControlPanel3.Controls.Add(Me.cboAttSearch)
        Me.TabControlPanel3.Controls.Add(Me.lblAttSearchCount)
        Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel3.Name = "TabControlPanel3"
        Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel3.Size = New System.Drawing.Size(361, 603)
        Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel3.Style.GradientAngle = 90
        Me.TabControlPanel3.TabIndex = 3
        Me.TabControlPanel3.TabItem = Me.tabAttSearch
        '
        'tabAttSearch
        '
        Me.tabAttSearch.AttachedControl = Me.TabControlPanel3
        Me.tabAttSearch.Name = "tabAttSearch"
        Me.tabAttSearch.Text = "Attribute Search"
        '
        'TabControlPanel35
        '
        Me.TabControlPanel35.Controls.Add(Me.lstEffectSearch)
        Me.TabControlPanel35.Controls.Add(Me.lblEffectSearch)
        Me.TabControlPanel35.Controls.Add(Me.cboEffectSearch)
        Me.TabControlPanel35.Controls.Add(Me.lblEffectSearchCount)
        Me.TabControlPanel35.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel35.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel35.Name = "TabControlPanel35"
        Me.TabControlPanel35.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel35.Size = New System.Drawing.Size(361, 603)
        Me.TabControlPanel35.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel35.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel35.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel35.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel35.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel35.Style.GradientAngle = 90
        Me.TabControlPanel35.TabIndex = 4
        Me.TabControlPanel35.TabItem = Me.tabEffectSearch
        '
        'lstEffectSearch
        '
        Me.lstEffectSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lstEffectSearch.Border.Class = "ListViewBorder"
        Me.lstEffectSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstEffectSearch.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader66})
        Me.lstEffectSearch.FullRowSelect = True
        Me.lstEffectSearch.GridLines = True
        Me.lstEffectSearch.Location = New System.Drawing.Point(4, 48)
        Me.lstEffectSearch.Name = "lstEffectSearch"
        Me.lstEffectSearch.ShowItemToolTips = True
        Me.lstEffectSearch.Size = New System.Drawing.Size(353, 551)
        Me.lstEffectSearch.TabIndex = 15
        Me.lstEffectSearch.UseCompatibleStateImageBehavior = False
        Me.lstEffectSearch.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader66
        '
        Me.ColumnHeader66.Text = "Item Name"
        Me.ColumnHeader66.Width = 325
        '
        'lblEffectSearch
        '
        Me.lblEffectSearch.AutoSize = True
        Me.lblEffectSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblEffectSearch.Location = New System.Drawing.Point(4, 4)
        Me.lblEffectSearch.Name = "lblEffectSearch"
        Me.lblEffectSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblEffectSearch.TabIndex = 12
        Me.lblEffectSearch.Text = "Search:"
        '
        'cboEffectSearch
        '
        Me.cboEffectSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboEffectSearch.FormattingEnabled = True
        Me.cboEffectSearch.Location = New System.Drawing.Point(4, 20)
        Me.cboEffectSearch.Name = "cboEffectSearch"
        Me.cboEffectSearch.Size = New System.Drawing.Size(353, 21)
        Me.cboEffectSearch.TabIndex = 14
        '
        'lblEffectSearchCount
        '
        Me.lblEffectSearchCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEffectSearchCount.BackColor = System.Drawing.Color.Transparent
        Me.lblEffectSearchCount.Location = New System.Drawing.Point(213, 3)
        Me.lblEffectSearchCount.Name = "lblEffectSearchCount"
        Me.lblEffectSearchCount.Size = New System.Drawing.Size(144, 14)
        Me.lblEffectSearchCount.TabIndex = 13
        Me.lblEffectSearchCount.Text = "0 items found"
        Me.lblEffectSearchCount.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'tabEffectSearch
        '
        Me.tabEffectSearch.AttachedControl = Me.TabControlPanel35
        Me.tabEffectSearch.Name = "tabEffectSearch"
        Me.tabEffectSearch.Text = "Effect Search"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.chkBrowseNonPublished)
        Me.TabControlPanel1.Controls.Add(Me.tvwBrowse)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(361, 603)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 1
        Me.TabControlPanel1.TabItem = Me.tabBrowse
        '
        'tabBrowse
        '
        Me.tabBrowse.AttachedControl = Me.TabControlPanel1
        Me.tabBrowse.Name = "tabBrowse"
        Me.tabBrowse.Text = "Browse"
        '
        'cboPilots
        '
        Me.cboPilots.DisplayMember = "Text"
        Me.cboPilots.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.ItemHeight = 15
        Me.cboPilots.Location = New System.Drawing.Point(399, 121)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(150, 21)
        Me.cboPilots.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboPilots.TabIndex = 45
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        '
        '
        '
        Me.lblPilot.BackgroundStyle.Class = ""
        Me.lblPilot.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblPilot.Location = New System.Drawing.Point(366, 121)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(27, 16)
        Me.lblPilot.TabIndex = 46
        Me.lblPilot.Text = "Pilot:"
        '
        'btnNavForward
        '
        Me.btnNavForward.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnNavForward.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnNavForward.Enabled = False
        Me.btnNavForward.FocusCuesEnabled = False
        Me.btnNavForward.Location = New System.Drawing.Point(448, 93)
        Me.btnNavForward.Name = "btnNavForward"
        Me.btnNavForward.Size = New System.Drawing.Size(75, 20)
        Me.btnNavForward.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnNavForward.TabIndex = 48
        Me.btnNavForward.Text = "Forward"
        '
        'btnRequisition
        '
        Me.btnRequisition.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnRequisition.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnRequisition.Enabled = False
        Me.btnRequisition.FocusCuesEnabled = False
        Me.btnRequisition.Location = New System.Drawing.Point(529, 93)
        Me.btnRequisition.Name = "btnRequisition"
        Me.btnRequisition.Size = New System.Drawing.Size(75, 20)
        Me.btnRequisition.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnRequisition.TabIndex = 49
        Me.btnRequisition.Text = "Requisition"
        '
        'barStatus
        '
        Me.barStatus.AccessibleDescription = "DotNetBar Bar (barStatus)"
        Me.barStatus.AccessibleName = "DotNetBar Bar"
        Me.barStatus.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar
        Me.barStatus.BarType = DevComponents.DotNetBar.eBarType.StatusBar
        Me.barStatus.CanAutoHide = False
        Me.barStatus.CanCustomize = False
        Me.barStatus.CanReorderTabs = False
        Me.barStatus.CanUndock = False
        Me.barStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barStatus.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.ResizeHandle
        Me.barStatus.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.lblStatus, Me.lblDBLocation, Me.lblID})
        Me.barStatus.Location = New System.Drawing.Point(0, 626)
        Me.barStatus.Name = "barStatus"
        Me.barStatus.Size = New System.Drawing.Size(1284, 21)
        Me.barStatus.Stretch = True
        Me.barStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.barStatus.TabIndex = 50
        Me.barStatus.TabStop = False
        '
        'lblStatus
        '
        Me.lblStatus.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.PaddingBottom = 1
        Me.lblStatus.PaddingLeft = 3
        Me.lblStatus.PaddingRight = 3
        Me.lblStatus.PaddingTop = 1
        Me.lblStatus.Text = "Awaiting Query..."
        Me.lblStatus.Tooltip = "Query Status"
        Me.lblStatus.Width = 250
        '
        'lblDBLocation
        '
        Me.lblDBLocation.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.lblDBLocation.Name = "lblDBLocation"
        Me.lblDBLocation.PaddingBottom = 1
        Me.lblDBLocation.PaddingLeft = 3
        Me.lblDBLocation.PaddingRight = 3
        Me.lblDBLocation.PaddingTop = 1
        Me.lblDBLocation.Stretch = True
        Me.lblDBLocation.Text = "Location:"
        Me.lblDBLocation.TextAlignment = System.Drawing.StringAlignment.Center
        Me.lblDBLocation.Tooltip = "Database Location"
        '
        'lblID
        '
        Me.lblID.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.lblID.Name = "lblID"
        Me.lblID.PaddingBottom = 1
        Me.lblID.PaddingLeft = 3
        Me.lblID.PaddingRight = 3
        Me.lblID.PaddingTop = 1
        Me.lblID.Text = "ID:"
        Me.lblID.Tooltip = "Item ID"
        Me.lblID.Width = 150
        '
        'panelIB
        '
        Me.panelIB.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelIB.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelIB.Controls.Add(Me.btnNavBack)
        Me.panelIB.Controls.Add(Me.btnRequisition)
        Me.panelIB.Controls.Add(Me.btnNavForward)
        Me.panelIB.Controls.Add(Me.tabIB)
        Me.panelIB.Controls.Add(Me.tabBrowser)
        Me.panelIB.Controls.Add(Me.picItem)
        Me.panelIB.Controls.Add(Me.lblUsable)
        Me.panelIB.Controls.Add(Me.lblItem)
        Me.panelIB.Controls.Add(Me.lblUsableTime)
        Me.panelIB.Controls.Add(Me.lblPilot)
        Me.panelIB.Controls.Add(Me.picBP)
        Me.panelIB.Controls.Add(Me.cboPilots)
        Me.panelIB.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelIB.Location = New System.Drawing.Point(0, 0)
        Me.panelIB.Name = "panelIB"
        Me.panelIB.Size = New System.Drawing.Size(1284, 626)
        Me.panelIB.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelIB.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelIB.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelIB.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelIB.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelIB.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelIB.Style.GradientAngle = 90
        Me.panelIB.TabIndex = 51
        '
        'btnNavBack
        '
        Me.btnNavBack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnNavBack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnNavBack.Enabled = False
        Me.btnNavBack.FocusCuesEnabled = False
        Me.btnNavBack.Location = New System.Drawing.Point(367, 93)
        Me.btnNavBack.Name = "btnNavBack"
        Me.btnNavBack.Size = New System.Drawing.Size(75, 20)
        Me.btnNavBack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnNavBack.TabIndex = 51
        Me.btnNavBack.Text = "Back"
        '
        'tabIB
        '
        Me.tabIB.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabIB.BackColor = System.Drawing.Color.Transparent
        Me.tabIB.CanReorderTabs = True
        Me.tabIB.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tabIB.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tabIB.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(249, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(248, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(245, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tabIB.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tabIB.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 1.0!)})
        Me.tabIB.Controls.Add(Me.TabControlPanel4)
        Me.tabIB.Controls.Add(Me.TabControlPanel6)
        Me.tabIB.Controls.Add(Me.TabControlPanel34)
        Me.tabIB.Controls.Add(Me.TabControlPanel5)
        Me.tabIB.Controls.Add(Me.TabControlPanel13)
        Me.tabIB.Controls.Add(Me.TabControlPanel11)
        Me.tabIB.Controls.Add(Me.TabControlPanel10)
        Me.tabIB.Controls.Add(Me.TabControlPanel9)
        Me.tabIB.Controls.Add(Me.TabControlPanel8)
        Me.tabIB.Controls.Add(Me.TabControlPanel7)
        Me.tabIB.Controls.Add(Me.TabControlPanel12)
        Me.tabIB.Location = New System.Drawing.Point(367, 188)
        Me.tabIB.Name = "tabIB"
        Me.tabIB.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tabIB.SelectedTabIndex = 1
        Me.tabIB.Size = New System.Drawing.Size(914, 435)
        Me.tabIB.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tabIB.TabIndex = 50
        Me.tabIB.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabIB.Tabs.Add(Me.tiDescription)
        Me.tabIB.Tabs.Add(Me.tiAttributes)
        Me.tabIB.Tabs.Add(Me.tiEffects)
        Me.tabIB.Tabs.Add(Me.tiSkills)
        Me.tabIB.Tabs.Add(Me.tiFitting)
        Me.tabIB.Tabs.Add(Me.tiVariations)
        Me.tabIB.Tabs.Add(Me.tiRecommended)
        Me.tabIB.Tabs.Add(Me.tiMaterials)
        Me.tabIB.Tabs.Add(Me.tiComponent)
        Me.tabIB.Tabs.Add(Me.tiDependencies)
        Me.tabIB.Tabs.Add(Me.tiEveCentral)
        Me.tabIB.Text = "TabControl2"
        '
        'TabControlPanel4
        '
        Me.TabControlPanel4.Controls.Add(Me.lblDescription)
        Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel4.Name = "TabControlPanel4"
        Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel4.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel4.Style.GradientAngle = 90
        Me.TabControlPanel4.TabIndex = 1
        Me.TabControlPanel4.TabItem = Me.tiDescription
        '
        'tiDescription
        '
        Me.tiDescription.AttachedControl = Me.TabControlPanel4
        Me.tiDescription.Name = "tiDescription"
        Me.tiDescription.Text = "Description"
        '
        'TabControlPanel6
        '
        Me.TabControlPanel6.Controls.Add(Me.btnAddSkills)
        Me.TabControlPanel6.Controls.Add(Me.btnViewSkills)
        Me.TabControlPanel6.Controls.Add(Me.tvwReqs)
        Me.TabControlPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel6.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel6.Name = "TabControlPanel6"
        Me.TabControlPanel6.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel6.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel6.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel6.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel6.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel6.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel6.Style.GradientAngle = 90
        Me.TabControlPanel6.TabIndex = 3
        Me.TabControlPanel6.TabItem = Me.tiSkills
        '
        'btnAddSkills
        '
        Me.btnAddSkills.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAddSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddSkills.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAddSkills.Location = New System.Drawing.Point(149, 385)
        Me.btnAddSkills.Name = "btnAddSkills"
        Me.btnAddSkills.Size = New System.Drawing.Size(155, 23)
        Me.btnAddSkills.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAddSkills.TabIndex = 5
        Me.btnAddSkills.Text = "Add Needed Skills to Queue"
        '
        'btnViewSkills
        '
        Me.btnViewSkills.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnViewSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnViewSkills.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnViewSkills.Location = New System.Drawing.Point(8, 385)
        Me.btnViewSkills.Name = "btnViewSkills"
        Me.btnViewSkills.Size = New System.Drawing.Size(121, 23)
        Me.btnViewSkills.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnViewSkills.TabIndex = 4
        Me.btnViewSkills.Text = "Show Needed Skills"
        '
        'tiSkills
        '
        Me.tiSkills.AttachedControl = Me.TabControlPanel6
        Me.tiSkills.Name = "tiSkills"
        Me.tiSkills.Text = "Skills"
        '
        'TabControlPanel34
        '
        Me.TabControlPanel34.Controls.Add(Me.lstEffects)
        Me.TabControlPanel34.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel34.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel34.Name = "TabControlPanel34"
        Me.TabControlPanel34.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel34.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel34.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel34.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel34.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel34.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel34.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel34.Style.GradientAngle = 90
        Me.TabControlPanel34.TabIndex = 11
        Me.TabControlPanel34.TabItem = Me.tiEffects
        '
        'lstEffects
        '
        '
        '
        '
        Me.lstEffects.Border.Class = "ListViewBorder"
        Me.lstEffects.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstEffects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader63, Me.ColumnHeader65})
        Me.lstEffects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstEffects.FullRowSelect = True
        Me.lstEffects.GridLines = True
        Me.lstEffects.Location = New System.Drawing.Point(1, 1)
        Me.lstEffects.Name = "lstEffects"
        Me.lstEffects.ShowItemToolTips = True
        Me.lstEffects.Size = New System.Drawing.Size(912, 410)
        Me.lstEffects.TabIndex = 1
        Me.lstEffects.UseCompatibleStateImageBehavior = False
        Me.lstEffects.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader63
        '
        Me.ColumnHeader63.Text = "Effect"
        Me.ColumnHeader63.Width = 210
        '
        'ColumnHeader65
        '
        Me.ColumnHeader65.Text = "Description"
        Me.ColumnHeader65.Width = 400
        '
        'tiEffects
        '
        Me.tiEffects.AttachedControl = Me.TabControlPanel34
        Me.tiEffects.Name = "tiEffects"
        Me.tiEffects.Text = "Effects"
        '
        'TabControlPanel5
        '
        Me.TabControlPanel5.Controls.Add(Me.lstAttributes)
        Me.TabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel5.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel5.Name = "TabControlPanel5"
        Me.TabControlPanel5.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel5.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel5.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel5.Style.GradientAngle = 90
        Me.TabControlPanel5.TabIndex = 2
        Me.TabControlPanel5.TabItem = Me.tiAttributes
        '
        'tiAttributes
        '
        Me.tiAttributes.AttachedControl = Me.TabControlPanel5
        Me.tiAttributes.Name = "tiAttributes"
        Me.tiAttributes.Text = "Attributes"
        '
        'TabControlPanel13
        '
        Me.TabControlPanel13.Controls.Add(Me.lstEveCentral)
        Me.TabControlPanel13.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel13.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel13.Name = "TabControlPanel13"
        Me.TabControlPanel13.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel13.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel13.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel13.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel13.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel13.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel13.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel13.Style.GradientAngle = 90
        Me.TabControlPanel13.TabIndex = 10
        Me.TabControlPanel13.TabItem = Me.tiEveCentral
        '
        'tiEveCentral
        '
        Me.tiEveCentral.AttachedControl = Me.TabControlPanel13
        Me.tiEveCentral.Name = "tiEveCentral"
        Me.tiEveCentral.Text = "Eve Central"
        '
        'TabControlPanel11
        '
        Me.TabControlPanel11.Controls.Add(Me.tcComponents)
        Me.TabControlPanel11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel11.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel11.Name = "TabControlPanel11"
        Me.TabControlPanel11.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel11.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel11.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel11.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel11.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel11.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel11.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel11.Style.GradientAngle = 90
        Me.TabControlPanel11.TabIndex = 8
        Me.TabControlPanel11.TabItem = Me.tiComponent
        '
        'tcComponents
        '
        Me.tcComponents.BackColor = System.Drawing.Color.Transparent
        Me.tcComponents.CanReorderTabs = True
        Me.tcComponents.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tcComponents.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tcComponents.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(249, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(248, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(245, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tcComponents.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tcComponents.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 1.0!)})
        Me.tcComponents.Controls.Add(Me.TabControlPanel33)
        Me.tcComponents.Controls.Add(Me.TabControlPanel26)
        Me.tcComponents.Controls.Add(Me.TabControlPanel27)
        Me.tcComponents.Controls.Add(Me.TabControlPanel25)
        Me.tcComponents.Controls.Add(Me.TabControlPanel28)
        Me.tcComponents.Controls.Add(Me.TabControlPanel29)
        Me.tcComponents.Controls.Add(Me.TabControlPanel30)
        Me.tcComponents.Controls.Add(Me.TabControlPanel31)
        Me.tcComponents.Controls.Add(Me.TabControlPanel32)
        Me.tcComponents.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcComponents.Location = New System.Drawing.Point(1, 1)
        Me.tcComponents.Name = "tcComponents"
        Me.tcComponents.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tcComponents.SelectedTabIndex = 7
        Me.tcComponents.Size = New System.Drawing.Size(912, 410)
        Me.tcComponents.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tcComponents.TabIndex = 2
        Me.tcComponents.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tcComponents.Tabs.Add(Me.tiC1)
        Me.tcComponents.Tabs.Add(Me.tiC2)
        Me.tcComponents.Tabs.Add(Me.tiC3)
        Me.tcComponents.Tabs.Add(Me.tiC4)
        Me.tcComponents.Tabs.Add(Me.tiC5)
        Me.tcComponents.Tabs.Add(Me.tiC6)
        Me.tcComponents.Tabs.Add(Me.tiC7)
        Me.tcComponents.Tabs.Add(Me.tiC8)
        Me.tcComponents.Tabs.Add(Me.tiC9)
        Me.tcComponents.Text = "TabControl2"
        '
        'TabControlPanel33
        '
        Me.TabControlPanel33.Controls.Add(Me.Label2)
        Me.TabControlPanel33.Controls.Add(Me.nudMELevelC)
        Me.TabControlPanel33.Controls.Add(Me.lstC1)
        Me.TabControlPanel33.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel33.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel33.Name = "TabControlPanel33"
        Me.TabControlPanel33.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel33.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel33.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel33.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel33.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel33.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel33.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel33.Style.GradientAngle = 90
        Me.TabControlPanel33.TabIndex = 1
        Me.TabControlPanel33.TabItem = Me.tiC1
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(4, 363)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "ME Level:"
        '
        'nudMELevelC
        '
        Me.nudMELevelC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.nudMELevelC.Location = New System.Drawing.Point(65, 361)
        Me.nudMELevelC.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMELevelC.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
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
        '
        '
        '
        Me.lstC1.Border.Class = "ListViewBorder"
        Me.lstC1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader61, Me.ColumnHeader62, Me.colC1ME, Me.ColumnHeader64})
        Me.lstC1.FullRowSelect = True
        Me.lstC1.GridLines = True
        Me.lstC1.Location = New System.Drawing.Point(4, 1)
        Me.lstC1.Name = "lstC1"
        Me.lstC1.Size = New System.Drawing.Size(901, 356)
        Me.lstC1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC1.TabIndex = 2
        Me.lstC1.UseCompatibleStateImageBehavior = False
        Me.lstC1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader61
        '
        Me.ColumnHeader61.Text = "Material"
        Me.ColumnHeader61.Width = 175
        '
        'ColumnHeader62
        '
        Me.ColumnHeader62.Text = "Perfect"
        Me.ColumnHeader62.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader62.Width = 75
        '
        'colC1ME
        '
        Me.colC1ME.Text = "ME 0"
        Me.colC1ME.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colC1ME.Width = 75
        '
        'ColumnHeader64
        '
        Me.ColumnHeader64.Text = "Pilot"
        Me.ColumnHeader64.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader64.Width = 75
        '
        'tiC1
        '
        Me.tiC1.AttachedControl = Me.TabControlPanel33
        Me.tiC1.Name = "tiC1"
        Me.tiC1.Text = "Manufacturing"
        '
        'TabControlPanel26
        '
        Me.TabControlPanel26.Controls.Add(Me.lstC8)
        Me.TabControlPanel26.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel26.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel26.Name = "TabControlPanel26"
        Me.TabControlPanel26.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel26.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel26.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel26.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel26.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel26.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel26.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel26.Style.GradientAngle = 90
        Me.TabControlPanel26.TabIndex = 8
        Me.TabControlPanel26.TabItem = Me.tiC8
        '
        'lstC8
        '
        '
        '
        '
        Me.lstC8.Border.Class = "ListViewBorder"
        Me.lstC8.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC8.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader45, Me.ColumnHeader46})
        Me.lstC8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC8.FullRowSelect = True
        Me.lstC8.GridLines = True
        Me.lstC8.Location = New System.Drawing.Point(1, 1)
        Me.lstC8.Name = "lstC8"
        Me.lstC8.Size = New System.Drawing.Size(910, 385)
        Me.lstC8.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC8.TabIndex = 3
        Me.lstC8.UseCompatibleStateImageBehavior = False
        Me.lstC8.View = System.Windows.Forms.View.Details
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
        'tiC8
        '
        Me.tiC8.AttachedControl = Me.TabControlPanel26
        Me.tiC8.Name = "tiC8"
        Me.tiC8.Text = "Invention"
        '
        'TabControlPanel27
        '
        Me.TabControlPanel27.Controls.Add(Me.lstC7)
        Me.TabControlPanel27.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel27.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel27.Name = "TabControlPanel27"
        Me.TabControlPanel27.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel27.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel27.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel27.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel27.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel27.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel27.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel27.Style.GradientAngle = 90
        Me.TabControlPanel27.TabIndex = 7
        Me.TabControlPanel27.TabItem = Me.tiC7
        '
        'lstC7
        '
        '
        '
        '
        Me.lstC7.Border.Class = "ListViewBorder"
        Me.lstC7.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC7.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader47, Me.ColumnHeader48})
        Me.lstC7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC7.FullRowSelect = True
        Me.lstC7.GridLines = True
        Me.lstC7.Location = New System.Drawing.Point(1, 1)
        Me.lstC7.Name = "lstC7"
        Me.lstC7.Size = New System.Drawing.Size(910, 385)
        Me.lstC7.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC7.TabIndex = 3
        Me.lstC7.UseCompatibleStateImageBehavior = False
        Me.lstC7.View = System.Windows.Forms.View.Details
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
        'tiC7
        '
        Me.tiC7.AttachedControl = Me.TabControlPanel27
        Me.tiC7.Name = "tiC7"
        Me.tiC7.Text = "Reverse Eng"
        '
        'TabControlPanel25
        '
        Me.TabControlPanel25.Controls.Add(Me.lstC9)
        Me.TabControlPanel25.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel25.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel25.Name = "TabControlPanel25"
        Me.TabControlPanel25.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel25.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel25.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel25.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel25.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel25.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel25.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel25.Style.GradientAngle = 90
        Me.TabControlPanel25.TabIndex = 9
        Me.TabControlPanel25.TabItem = Me.tiC9
        '
        'lstC9
        '
        '
        '
        '
        Me.lstC9.Border.Class = "ListViewBorder"
        Me.lstC9.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC9.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader43, Me.ColumnHeader44})
        Me.lstC9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC9.FullRowSelect = True
        Me.lstC9.GridLines = True
        Me.lstC9.Location = New System.Drawing.Point(1, 1)
        Me.lstC9.Name = "lstC9"
        Me.lstC9.Size = New System.Drawing.Size(910, 385)
        Me.lstC9.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC9.TabIndex = 4
        Me.lstC9.UseCompatibleStateImageBehavior = False
        Me.lstC9.View = System.Windows.Forms.View.Details
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
        'tiC9
        '
        Me.tiC9.AttachedControl = Me.TabControlPanel25
        Me.tiC9.Name = "tiC9"
        Me.tiC9.Text = "Recycling"
        '
        'TabControlPanel28
        '
        Me.TabControlPanel28.Controls.Add(Me.lstC6)
        Me.TabControlPanel28.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel28.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel28.Name = "TabControlPanel28"
        Me.TabControlPanel28.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel28.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel28.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel28.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel28.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel28.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel28.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel28.Style.GradientAngle = 90
        Me.TabControlPanel28.TabIndex = 6
        Me.TabControlPanel28.TabItem = Me.tiC6
        '
        'lstC6
        '
        '
        '
        '
        Me.lstC6.Border.Class = "ListViewBorder"
        Me.lstC6.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC6.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader49, Me.ColumnHeader50})
        Me.lstC6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC6.FullRowSelect = True
        Me.lstC6.GridLines = True
        Me.lstC6.Location = New System.Drawing.Point(1, 1)
        Me.lstC6.Name = "lstC6"
        Me.lstC6.Size = New System.Drawing.Size(910, 385)
        Me.lstC6.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC6.TabIndex = 3
        Me.lstC6.UseCompatibleStateImageBehavior = False
        Me.lstC6.View = System.Windows.Forms.View.Details
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
        'tiC6
        '
        Me.tiC6.AttachedControl = Me.TabControlPanel28
        Me.tiC6.Name = "tiC6"
        Me.tiC6.Text = "Duplicating"
        '
        'TabControlPanel29
        '
        Me.TabControlPanel29.Controls.Add(Me.lstC5)
        Me.TabControlPanel29.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel29.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel29.Name = "TabControlPanel29"
        Me.TabControlPanel29.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel29.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel29.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel29.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel29.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel29.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel29.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel29.Style.GradientAngle = 90
        Me.TabControlPanel29.TabIndex = 5
        Me.TabControlPanel29.TabItem = Me.tiC5
        '
        'lstC5
        '
        '
        '
        '
        Me.lstC5.Border.Class = "ListViewBorder"
        Me.lstC5.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC5.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader51, Me.ColumnHeader52})
        Me.lstC5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC5.FullRowSelect = True
        Me.lstC5.GridLines = True
        Me.lstC5.Location = New System.Drawing.Point(1, 1)
        Me.lstC5.Name = "lstC5"
        Me.lstC5.Size = New System.Drawing.Size(910, 385)
        Me.lstC5.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC5.TabIndex = 3
        Me.lstC5.UseCompatibleStateImageBehavior = False
        Me.lstC5.View = System.Windows.Forms.View.Details
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
        'tiC5
        '
        Me.tiC5.AttachedControl = Me.TabControlPanel29
        Me.tiC5.Name = "tiC5"
        Me.tiC5.Text = "Copying"
        '
        'TabControlPanel30
        '
        Me.TabControlPanel30.Controls.Add(Me.lstC4)
        Me.TabControlPanel30.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel30.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel30.Name = "TabControlPanel30"
        Me.TabControlPanel30.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel30.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel30.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel30.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel30.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel30.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel30.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel30.Style.GradientAngle = 90
        Me.TabControlPanel30.TabIndex = 4
        Me.TabControlPanel30.TabItem = Me.tiC4
        '
        'lstC4
        '
        '
        '
        '
        Me.lstC4.Border.Class = "ListViewBorder"
        Me.lstC4.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC4.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader53, Me.ColumnHeader54})
        Me.lstC4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC4.FullRowSelect = True
        Me.lstC4.GridLines = True
        Me.lstC4.Location = New System.Drawing.Point(1, 1)
        Me.lstC4.Name = "lstC4"
        Me.lstC4.Size = New System.Drawing.Size(910, 385)
        Me.lstC4.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC4.TabIndex = 3
        Me.lstC4.UseCompatibleStateImageBehavior = False
        Me.lstC4.View = System.Windows.Forms.View.Details
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
        'tiC4
        '
        Me.tiC4.AttachedControl = Me.TabControlPanel30
        Me.tiC4.Name = "tiC4"
        Me.tiC4.Text = "Research ME"
        '
        'TabControlPanel31
        '
        Me.TabControlPanel31.Controls.Add(Me.lstC3)
        Me.TabControlPanel31.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel31.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel31.Name = "TabControlPanel31"
        Me.TabControlPanel31.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel31.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel31.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel31.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel31.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel31.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel31.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel31.Style.GradientAngle = 90
        Me.TabControlPanel31.TabIndex = 3
        Me.TabControlPanel31.TabItem = Me.tiC3
        '
        'lstC3
        '
        '
        '
        '
        Me.lstC3.Border.Class = "ListViewBorder"
        Me.lstC3.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader55, Me.ColumnHeader56})
        Me.lstC3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC3.FullRowSelect = True
        Me.lstC3.GridLines = True
        Me.lstC3.Location = New System.Drawing.Point(1, 1)
        Me.lstC3.Name = "lstC3"
        Me.lstC3.Size = New System.Drawing.Size(910, 385)
        Me.lstC3.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC3.TabIndex = 3
        Me.lstC3.UseCompatibleStateImageBehavior = False
        Me.lstC3.View = System.Windows.Forms.View.Details
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
        'tiC3
        '
        Me.tiC3.AttachedControl = Me.TabControlPanel31
        Me.tiC3.Name = "tiC3"
        Me.tiC3.Text = "Research PE"
        '
        'TabControlPanel32
        '
        Me.TabControlPanel32.Controls.Add(Me.lstC2)
        Me.TabControlPanel32.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel32.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel32.Name = "TabControlPanel32"
        Me.TabControlPanel32.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel32.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel32.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel32.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel32.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel32.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel32.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel32.Style.GradientAngle = 90
        Me.TabControlPanel32.TabIndex = 2
        Me.TabControlPanel32.TabItem = Me.tiC2
        '
        'lstC2
        '
        '
        '
        '
        Me.lstC2.Border.Class = "ListViewBorder"
        Me.lstC2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lstC2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader57, Me.ColumnHeader58})
        Me.lstC2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstC2.FullRowSelect = True
        Me.lstC2.GridLines = True
        Me.lstC2.Location = New System.Drawing.Point(1, 1)
        Me.lstC2.Name = "lstC2"
        Me.lstC2.Size = New System.Drawing.Size(910, 385)
        Me.lstC2.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstC2.TabIndex = 3
        Me.lstC2.UseCompatibleStateImageBehavior = False
        Me.lstC2.View = System.Windows.Forms.View.Details
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
        'tiC2
        '
        Me.tiC2.AttachedControl = Me.TabControlPanel32
        Me.tiC2.Name = "tiC2"
        Me.tiC2.Text = "Research Tech"
        '
        'tiComponent
        '
        Me.tiComponent.AttachedControl = Me.TabControlPanel11
        Me.tiComponent.Name = "tiComponent"
        Me.tiComponent.Text = "Component"
        '
        'TabControlPanel10
        '
        Me.TabControlPanel10.Controls.Add(Me.tcMaterials)
        Me.TabControlPanel10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel10.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel10.Name = "TabControlPanel10"
        Me.TabControlPanel10.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel10.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel10.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel10.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel10.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel10.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel10.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel10.Style.GradientAngle = 90
        Me.TabControlPanel10.TabIndex = 7
        Me.TabControlPanel10.TabItem = Me.tiMaterials
        '
        'tcMaterials
        '
        Me.tcMaterials.BackColor = System.Drawing.Color.Transparent
        Me.tcMaterials.CanReorderTabs = True
        Me.tcMaterials.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tcMaterials.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tcMaterials.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(249, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(248, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(245, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tcMaterials.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tcMaterials.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 1.0!)})
        Me.tcMaterials.Controls.Add(Me.TabControlPanel16)
        Me.tcMaterials.Controls.Add(Me.TabControlPanel24)
        Me.tcMaterials.Controls.Add(Me.TabControlPanel23)
        Me.tcMaterials.Controls.Add(Me.TabControlPanel22)
        Me.tcMaterials.Controls.Add(Me.TabControlPanel21)
        Me.tcMaterials.Controls.Add(Me.TabControlPanel20)
        Me.tcMaterials.Controls.Add(Me.TabControlPanel19)
        Me.tcMaterials.Controls.Add(Me.TabControlPanel18)
        Me.tcMaterials.Controls.Add(Me.TabControlPanel17)
        Me.tcMaterials.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMaterials.Location = New System.Drawing.Point(1, 1)
        Me.tcMaterials.Name = "tcMaterials"
        Me.tcMaterials.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tcMaterials.SelectedTabIndex = 7
        Me.tcMaterials.Size = New System.Drawing.Size(912, 410)
        Me.tcMaterials.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tcMaterials.TabIndex = 1
        Me.tcMaterials.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tcMaterials.Tabs.Add(Me.tiM1)
        Me.tcMaterials.Tabs.Add(Me.tiM2)
        Me.tcMaterials.Tabs.Add(Me.tiM3)
        Me.tcMaterials.Tabs.Add(Me.tiM4)
        Me.tcMaterials.Tabs.Add(Me.tiM5)
        Me.tcMaterials.Tabs.Add(Me.tiM6)
        Me.tcMaterials.Tabs.Add(Me.tiM7)
        Me.tcMaterials.Tabs.Add(Me.tiM8)
        Me.tcMaterials.Tabs.Add(Me.tiM9)
        Me.tcMaterials.Text = "TabControl2"
        '
        'TabControlPanel16
        '
        Me.TabControlPanel16.Controls.Add(Me.lblMELevel)
        Me.TabControlPanel16.Controls.Add(Me.nudMELevel)
        Me.TabControlPanel16.Controls.Add(Me.lstM1)
        Me.TabControlPanel16.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel16.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel16.Name = "TabControlPanel16"
        Me.TabControlPanel16.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel16.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel16.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel16.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel16.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel16.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel16.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel16.Style.GradientAngle = 90
        Me.TabControlPanel16.TabIndex = 1
        Me.TabControlPanel16.TabItem = Me.tiM1
        '
        'tiM1
        '
        Me.tiM1.AttachedControl = Me.TabControlPanel16
        Me.tiM1.Name = "tiM1"
        Me.tiM1.Text = "Manufacturing"
        '
        'TabControlPanel24
        '
        Me.TabControlPanel24.Controls.Add(Me.lstM9)
        Me.TabControlPanel24.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel24.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel24.Name = "TabControlPanel24"
        Me.TabControlPanel24.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel24.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel24.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel24.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel24.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel24.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel24.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel24.Style.GradientAngle = 90
        Me.TabControlPanel24.TabIndex = 9
        Me.TabControlPanel24.TabItem = Me.tiM9
        '
        'tiM9
        '
        Me.tiM9.AttachedControl = Me.TabControlPanel24
        Me.tiM9.Name = "tiM9"
        Me.tiM9.Text = "Recycling"
        '
        'TabControlPanel23
        '
        Me.TabControlPanel23.Controls.Add(Me.lstM8)
        Me.TabControlPanel23.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel23.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel23.Name = "TabControlPanel23"
        Me.TabControlPanel23.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel23.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel23.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel23.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel23.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel23.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel23.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel23.Style.GradientAngle = 90
        Me.TabControlPanel23.TabIndex = 8
        Me.TabControlPanel23.TabItem = Me.tiM8
        '
        'tiM8
        '
        Me.tiM8.AttachedControl = Me.TabControlPanel23
        Me.tiM8.Name = "tiM8"
        Me.tiM8.Text = "Invention"
        '
        'TabControlPanel22
        '
        Me.TabControlPanel22.Controls.Add(Me.lstM7)
        Me.TabControlPanel22.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel22.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel22.Name = "TabControlPanel22"
        Me.TabControlPanel22.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel22.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel22.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel22.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel22.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel22.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel22.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel22.Style.GradientAngle = 90
        Me.TabControlPanel22.TabIndex = 7
        Me.TabControlPanel22.TabItem = Me.tiM7
        '
        'tiM7
        '
        Me.tiM7.AttachedControl = Me.TabControlPanel22
        Me.tiM7.Name = "tiM7"
        Me.tiM7.Text = "Reverse Eng"
        '
        'TabControlPanel21
        '
        Me.TabControlPanel21.Controls.Add(Me.lstM6)
        Me.TabControlPanel21.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel21.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel21.Name = "TabControlPanel21"
        Me.TabControlPanel21.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel21.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel21.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel21.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel21.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel21.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel21.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel21.Style.GradientAngle = 90
        Me.TabControlPanel21.TabIndex = 6
        Me.TabControlPanel21.TabItem = Me.tiM6
        '
        'tiM6
        '
        Me.tiM6.AttachedControl = Me.TabControlPanel21
        Me.tiM6.Name = "tiM6"
        Me.tiM6.Text = "Duplicating"
        '
        'TabControlPanel20
        '
        Me.TabControlPanel20.Controls.Add(Me.lstM5)
        Me.TabControlPanel20.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel20.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel20.Name = "TabControlPanel20"
        Me.TabControlPanel20.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel20.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel20.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel20.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel20.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel20.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel20.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel20.Style.GradientAngle = 90
        Me.TabControlPanel20.TabIndex = 5
        Me.TabControlPanel20.TabItem = Me.tiM5
        '
        'tiM5
        '
        Me.tiM5.AttachedControl = Me.TabControlPanel20
        Me.tiM5.Name = "tiM5"
        Me.tiM5.Text = "Copying"
        '
        'TabControlPanel19
        '
        Me.TabControlPanel19.Controls.Add(Me.lstM4)
        Me.TabControlPanel19.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel19.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel19.Name = "TabControlPanel19"
        Me.TabControlPanel19.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel19.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel19.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel19.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel19.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel19.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel19.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel19.Style.GradientAngle = 90
        Me.TabControlPanel19.TabIndex = 4
        Me.TabControlPanel19.TabItem = Me.tiM4
        '
        'tiM4
        '
        Me.tiM4.AttachedControl = Me.TabControlPanel19
        Me.tiM4.Name = "tiM4"
        Me.tiM4.Text = "Research ME"
        '
        'TabControlPanel18
        '
        Me.TabControlPanel18.Controls.Add(Me.lstM3)
        Me.TabControlPanel18.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel18.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel18.Name = "TabControlPanel18"
        Me.TabControlPanel18.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel18.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel18.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel18.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel18.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel18.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel18.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel18.Style.GradientAngle = 90
        Me.TabControlPanel18.TabIndex = 3
        Me.TabControlPanel18.TabItem = Me.tiM3
        '
        'tiM3
        '
        Me.tiM3.AttachedControl = Me.TabControlPanel18
        Me.tiM3.Name = "tiM3"
        Me.tiM3.Text = "Research PE"
        '
        'TabControlPanel17
        '
        Me.TabControlPanel17.Controls.Add(Me.lstM2)
        Me.TabControlPanel17.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel17.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel17.Name = "TabControlPanel17"
        Me.TabControlPanel17.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel17.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel17.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel17.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel17.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel17.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel17.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel17.Style.GradientAngle = 90
        Me.TabControlPanel17.TabIndex = 2
        Me.TabControlPanel17.TabItem = Me.tiM2
        '
        'tiM2
        '
        Me.tiM2.AttachedControl = Me.TabControlPanel17
        Me.tiM2.Name = "tiM2"
        Me.tiM2.Text = "Research Tech"
        '
        'tiMaterials
        '
        Me.tiMaterials.AttachedControl = Me.TabControlPanel10
        Me.tiMaterials.Name = "tiMaterials"
        Me.tiMaterials.Text = "Materials"
        '
        'TabControlPanel9
        '
        Me.TabControlPanel9.Controls.Add(Me.lvwRecommended)
        Me.TabControlPanel9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel9.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel9.Name = "TabControlPanel9"
        Me.TabControlPanel9.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel9.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel9.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel9.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel9.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel9.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel9.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel9.Style.GradientAngle = 90
        Me.TabControlPanel9.TabIndex = 6
        Me.TabControlPanel9.TabItem = Me.tiRecommended
        '
        'tiRecommended
        '
        Me.tiRecommended.AttachedControl = Me.TabControlPanel9
        Me.tiRecommended.Name = "tiRecommended"
        Me.tiRecommended.Text = "Recommended"
        '
        'TabControlPanel8
        '
        Me.TabControlPanel8.Controls.Add(Me.tcVariations)
        Me.TabControlPanel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel8.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel8.Name = "TabControlPanel8"
        Me.TabControlPanel8.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel8.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel8.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel8.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel8.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel8.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel8.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel8.Style.GradientAngle = 90
        Me.TabControlPanel8.TabIndex = 5
        Me.TabControlPanel8.TabItem = Me.tiVariations
        '
        'tcVariations
        '
        Me.tcVariations.BackColor = System.Drawing.Color.Transparent
        Me.tcVariations.CanReorderTabs = True
        Me.tcVariations.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tcVariations.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tcVariations.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(249, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(248, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(245, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tcVariations.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tcVariations.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 1.0!)})
        Me.tcVariations.Controls.Add(Me.TabControlPanel14)
        Me.tcVariations.Controls.Add(Me.TabControlPanel15)
        Me.tcVariations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcVariations.Location = New System.Drawing.Point(1, 1)
        Me.tcVariations.Name = "tcVariations"
        Me.tcVariations.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tcVariations.SelectedTabIndex = 0
        Me.tcVariations.Size = New System.Drawing.Size(912, 410)
        Me.tcVariations.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tcVariations.TabIndex = 2
        Me.tcVariations.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tcVariations.Tabs.Add(Me.tiVariation)
        Me.tcVariations.Tabs.Add(Me.tiComparison)
        Me.tcVariations.Text = "TabControl2"
        '
        'TabControlPanel14
        '
        Me.TabControlPanel14.Controls.Add(Me.lstVariations)
        Me.TabControlPanel14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel14.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel14.Name = "TabControlPanel14"
        Me.TabControlPanel14.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel14.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel14.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel14.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel14.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel14.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel14.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel14.Style.GradientAngle = 90
        Me.TabControlPanel14.TabIndex = 1
        Me.TabControlPanel14.TabItem = Me.tiVariation
        '
        'tiVariation
        '
        Me.tiVariation.AttachedControl = Me.TabControlPanel14
        Me.tiVariation.Name = "tiVariation"
        Me.tiVariation.Text = "Variations"
        '
        'TabControlPanel15
        '
        Me.TabControlPanel15.Controls.Add(Me.chkShowAllColumns)
        Me.TabControlPanel15.Controls.Add(Me.lstComparisons)
        Me.TabControlPanel15.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel15.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel15.Name = "TabControlPanel15"
        Me.TabControlPanel15.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel15.Size = New System.Drawing.Size(912, 387)
        Me.TabControlPanel15.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel15.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel15.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel15.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel15.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel15.Style.GradientAngle = 90
        Me.TabControlPanel15.TabIndex = 2
        Me.TabControlPanel15.TabItem = Me.tiComparison
        '
        'tiComparison
        '
        Me.tiComparison.AttachedControl = Me.TabControlPanel15
        Me.tiComparison.Name = "tiComparison"
        Me.tiComparison.Text = "Comparison"
        '
        'tiVariations
        '
        Me.tiVariations.AttachedControl = Me.TabControlPanel8
        Me.tiVariations.Name = "tiVariations"
        Me.tiVariations.Text = "Variations"
        '
        'TabControlPanel7
        '
        Me.TabControlPanel7.Controls.Add(Me.lstFitting)
        Me.TabControlPanel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel7.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel7.Name = "TabControlPanel7"
        Me.TabControlPanel7.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel7.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel7.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel7.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel7.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel7.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel7.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel7.Style.GradientAngle = 90
        Me.TabControlPanel7.TabIndex = 4
        Me.TabControlPanel7.TabItem = Me.tiFitting
        '
        'tiFitting
        '
        Me.tiFitting.AttachedControl = Me.TabControlPanel7
        Me.tiFitting.Name = "tiFitting"
        Me.tiFitting.Text = "Fitting"
        '
        'TabControlPanel12
        '
        Me.TabControlPanel12.Controls.Add(Me.lvwDepend)
        Me.TabControlPanel12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel12.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel12.Name = "TabControlPanel12"
        Me.TabControlPanel12.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel12.Size = New System.Drawing.Size(914, 412)
        Me.TabControlPanel12.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel12.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel12.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel12.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel12.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel12.Style.GradientAngle = 90
        Me.TabControlPanel12.TabIndex = 9
        Me.TabControlPanel12.TabItem = Me.tiDependencies
        '
        'tiDependencies
        '
        Me.tiDependencies.AttachedControl = Me.TabControlPanel12
        Me.tiDependencies.Name = "tiDependencies"
        Me.tiDependencies.Text = "Dependencies"
        '
        'frmItemBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1284, 647)
        Me.Controls.Add(Me.panelIB)
        Me.Controls.Add(Me.barStatus)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(760, 530)
        Me.Name = "frmItemBrowser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "EveHQ Item Browser"
        Me.ctxSkills.ResumeLayout(False)
        CType(Me.picBP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).EndInit()
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
        CType(Me.tabBrowser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabBrowser.ResumeLayout(False)
        Me.TabControlPanel2.ResumeLayout(False)
        Me.TabControlPanel2.PerformLayout()
        Me.TabControlPanel3.ResumeLayout(False)
        Me.TabControlPanel3.PerformLayout()
        Me.TabControlPanel35.ResumeLayout(False)
        Me.TabControlPanel35.PerformLayout()
        Me.TabControlPanel1.ResumeLayout(False)
        Me.TabControlPanel1.PerformLayout()
        CType(Me.barStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelIB.ResumeLayout(False)
        Me.panelIB.PerformLayout()
        CType(Me.tabIB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabIB.ResumeLayout(False)
        Me.TabControlPanel4.ResumeLayout(False)
        Me.TabControlPanel6.ResumeLayout(False)
        Me.TabControlPanel34.ResumeLayout(False)
        Me.TabControlPanel5.ResumeLayout(False)
        Me.TabControlPanel13.ResumeLayout(False)
        Me.TabControlPanel11.ResumeLayout(False)
        CType(Me.tcComponents, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcComponents.ResumeLayout(False)
        Me.TabControlPanel33.ResumeLayout(False)
        Me.TabControlPanel33.PerformLayout()
        CType(Me.nudMELevelC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel26.ResumeLayout(False)
        Me.TabControlPanel27.ResumeLayout(False)
        Me.TabControlPanel25.ResumeLayout(False)
        Me.TabControlPanel28.ResumeLayout(False)
        Me.TabControlPanel29.ResumeLayout(False)
        Me.TabControlPanel30.ResumeLayout(False)
        Me.TabControlPanel31.ResumeLayout(False)
        Me.TabControlPanel32.ResumeLayout(False)
        Me.TabControlPanel10.ResumeLayout(False)
        CType(Me.tcMaterials, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcMaterials.ResumeLayout(False)
        Me.TabControlPanel16.ResumeLayout(False)
        Me.TabControlPanel16.PerformLayout()
        Me.TabControlPanel24.ResumeLayout(False)
        Me.TabControlPanel23.ResumeLayout(False)
        Me.TabControlPanel22.ResumeLayout(False)
        Me.TabControlPanel21.ResumeLayout(False)
        Me.TabControlPanel20.ResumeLayout(False)
        Me.TabControlPanel19.ResumeLayout(False)
        Me.TabControlPanel18.ResumeLayout(False)
        Me.TabControlPanel17.ResumeLayout(False)
        Me.TabControlPanel9.ResumeLayout(False)
        Me.TabControlPanel8.ResumeLayout(False)
        CType(Me.tcVariations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcVariations.ResumeLayout(False)
        Me.TabControlPanel14.ResumeLayout(False)
        Me.TabControlPanel15.ResumeLayout(False)
        Me.TabControlPanel15.PerformLayout()
        Me.TabControlPanel7.ResumeLayout(False)
        Me.TabControlPanel12.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Friend WithEvents ctxSkills As System.Windows.Forms.ContextMenuStrip
	Friend WithEvents mnuSkillName As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
	Friend WithEvents mnuViewDetails As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ItemToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents SkillToolTip As System.Windows.Forms.ToolTip
	Friend WithEvents picBP As System.Windows.Forms.PictureBox
	Friend WithEvents lblMELevel As System.Windows.Forms.Label
	Friend WithEvents nudMELevel As System.Windows.Forms.NumericUpDown
	Friend WithEvents lstM1 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents colM1M As System.Windows.Forms.ColumnHeader
	Friend WithEvents colM1Q As System.Windows.Forms.ColumnHeader
	Friend WithEvents colM1ME As System.Windows.Forms.ColumnHeader
	Friend WithEvents colM1P As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstM2 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstM3 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstM4 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstM5 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstM6 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstM7 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstM8 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader16 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstM9 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader17 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader18 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstVariations As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents colTypeName As System.Windows.Forms.ColumnHeader
	Friend WithEvents colMetaTypeName As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstFitting As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tvwReqs As System.Windows.Forms.TreeView
	Friend WithEvents lstAttributes As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents colAttribute As System.Windows.Forms.ColumnHeader
	Friend WithEvents colData As System.Windows.Forms.ColumnHeader
	Friend WithEvents lblDescription As System.Windows.Forms.Label
	Friend WithEvents picItem As System.Windows.Forms.PictureBox
	Friend WithEvents lblItem As System.Windows.Forms.Label
	Friend WithEvents lblUsable As System.Windows.Forms.Label
	Friend WithEvents lstSearch As System.Windows.Forms.ListBox
	Friend WithEvents lblSearchCount As System.Windows.Forms.Label
	Friend WithEvents txtSearch As System.Windows.Forms.TextBox
	Friend WithEvents lblSearch As System.Windows.Forms.Label
	Friend WithEvents tvwBrowse As System.Windows.Forms.TreeView
	Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
	Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
	Friend WithEvents ListView1 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader19 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader20 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader21 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader22 As System.Windows.Forms.ColumnHeader
	Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
	Friend WithEvents ListView2 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader23 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader24 As System.Windows.Forms.ColumnHeader
	Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
	Friend WithEvents ListView3 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader25 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader26 As System.Windows.Forms.ColumnHeader
	Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
	Friend WithEvents ListView4 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader27 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader28 As System.Windows.Forms.ColumnHeader
	Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
	Friend WithEvents ListView5 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader29 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader30 As System.Windows.Forms.ColumnHeader
	Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
	Friend WithEvents ListView6 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader31 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader32 As System.Windows.Forms.ColumnHeader
	Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
	Friend WithEvents ListView7 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader33 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader34 As System.Windows.Forms.ColumnHeader
	Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
	Friend WithEvents ListView8 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader35 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader36 As System.Windows.Forms.ColumnHeader
	Friend WithEvents TabPage10 As System.Windows.Forms.TabPage
	Friend WithEvents ListView9 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader37 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader38 As System.Windows.Forms.ColumnHeader
	Friend WithEvents cboAttSearch As System.Windows.Forms.ComboBox
	Friend WithEvents lblAttSearchCount As System.Windows.Forms.Label
	Friend WithEvents lblAttSearch As System.Windows.Forms.Label
	Friend WithEvents lstAttSearch As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents colAttName As System.Windows.Forms.ColumnHeader
	Friend WithEvents colAttValue As System.Windows.Forms.ColumnHeader
	Friend WithEvents lstComparisons As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader39 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader40 As System.Windows.Forms.ColumnHeader
	Friend WithEvents chkShowAllColumns As System.Windows.Forms.CheckBox
	Friend WithEvents lstEveCentral As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader41 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader42 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lblUsableTime As System.Windows.Forms.LinkLabel
	Friend WithEvents lvwDepend As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents NeededFor As System.Windows.Forms.ColumnHeader
	Friend WithEvents NeededLevel As System.Windows.Forms.ColumnHeader
	Friend WithEvents NeededGroup As System.Windows.Forms.ColumnHeader
	Friend WithEvents chkBrowseNonPublished As System.Windows.Forms.CheckBox
	Friend WithEvents lvwRecommended As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader59 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader60 As System.Windows.Forms.ColumnHeader
	Friend WithEvents imgListCerts As System.Windows.Forms.ImageList
	Friend WithEvents tabBrowser As DevComponents.DotNetBar.TabControl
	Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tabBrowse As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tabAttSearch As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tabSearch As DevComponents.DotNetBar.TabItem
	Friend WithEvents cboPilots As DevComponents.DotNetBar.Controls.ComboBoxEx
	Friend WithEvents lblPilot As DevComponents.DotNetBar.LabelX
    Friend WithEvents btnNavForward As DevComponents.DotNetBar.ButtonX
	Friend WithEvents btnRequisition As DevComponents.DotNetBar.ButtonX
	Friend WithEvents barStatus As DevComponents.DotNetBar.Bar
	Friend WithEvents lblStatus As DevComponents.DotNetBar.LabelItem
	Friend WithEvents lblDBLocation As DevComponents.DotNetBar.LabelItem
	Friend WithEvents lblID As DevComponents.DotNetBar.LabelItem
	Friend WithEvents panelIB As DevComponents.DotNetBar.PanelEx
	Friend WithEvents tabIB As DevComponents.DotNetBar.TabControl
	Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiDescription As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel5 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiAttributes As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel6 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiSkills As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel7 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiFitting As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel9 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiRecommended As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel8 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiVariations As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel13 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiEveCentral As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel12 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiDependencies As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel11 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiComponent As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel10 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiMaterials As DevComponents.DotNetBar.TabItem
	Friend WithEvents btnAddSkills As DevComponents.DotNetBar.ButtonX
	Friend WithEvents btnViewSkills As DevComponents.DotNetBar.ButtonX
	Friend WithEvents tcVariations As DevComponents.DotNetBar.TabControl
	Friend WithEvents TabControlPanel14 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiVariation As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel15 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiComparison As DevComponents.DotNetBar.TabItem
	Friend WithEvents tcMaterials As DevComponents.DotNetBar.TabControl
	Friend WithEvents TabControlPanel23 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM8 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel22 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM7 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel21 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM6 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel20 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM5 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel19 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM4 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel18 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM3 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel17 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM2 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel16 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM1 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel24 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents tiM9 As DevComponents.DotNetBar.TabItem
	Friend WithEvents tcComponents As DevComponents.DotNetBar.TabControl
	Friend WithEvents TabControlPanel25 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents lstC9 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader43 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader44 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tiC9 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel26 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents lstC8 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader45 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader46 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tiC8 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel27 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents lstC7 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader47 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader48 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tiC7 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel28 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents lstC6 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader49 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader50 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tiC6 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel29 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents lstC5 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader51 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader52 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tiC5 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel30 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents lstC4 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader53 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader54 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tiC4 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel31 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents lstC3 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader55 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader56 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tiC3 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel32 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents lstC2 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader57 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader58 As System.Windows.Forms.ColumnHeader
	Friend WithEvents tiC2 As DevComponents.DotNetBar.TabItem
	Friend WithEvents TabControlPanel33 As DevComponents.DotNetBar.TabControlPanel
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents nudMELevelC As System.Windows.Forms.NumericUpDown
	Friend WithEvents lstC1 As DevComponents.DotNetBar.Controls.ListViewEx
	Friend WithEvents ColumnHeader61 As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader62 As System.Windows.Forms.ColumnHeader
	Friend WithEvents colC1ME As System.Windows.Forms.ColumnHeader
	Friend WithEvents ColumnHeader64 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tiC1 As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel34 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents lstEffects As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents ColumnHeader63 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader65 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tiEffects As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel35 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents lstEffectSearch As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents ColumnHeader66 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblEffectSearch As System.Windows.Forms.Label
    Friend WithEvents cboEffectSearch As System.Windows.Forms.ComboBox
    Friend WithEvents lblEffectSearchCount As System.Windows.Forms.Label
    Friend WithEvents tabEffectSearch As DevComponents.DotNetBar.TabItem
    Friend WithEvents btnNavBack As DevComponents.DotNetBar.ButtonX
End Class
