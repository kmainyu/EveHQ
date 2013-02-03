<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMarketPrices
    Inherits DevComponents.DotNetBar.Office2007Form

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMarketPrices))
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog()
        Me.lblHighlightOldLogsText = New System.Windows.Forms.Label()
        Me.ctxMarketExport = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuViewOrders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeleteLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.nudAge = New System.Windows.Forms.NumericUpDown()
        Me.lblHighlightLogsHours = New System.Windows.Forms.Label()
        Me.chkShowOnlyCustom = New System.Windows.Forms.CheckBox()
        Me.lblCustomPrices = New System.Windows.Forms.Label()
        Me.btnResetGrid = New System.Windows.Forms.Button()
        Me.txtSearchPrices = New System.Windows.Forms.TextBox()
        Me.lblSearchPrices = New System.Windows.Forms.Label()
        Me.ctxPrices = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPriceItemName = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPriceAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPriceEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPriceDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabControl1 = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
        Me.gpMarketPrices = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.openMarketSettings = New System.Windows.Forms.Button()
        Me._getMarketOrders = New System.Windows.Forms.Button()
        Me._sellOrders = New DevComponents.AdvTree.AdvTree()
        Me.Location = New DevComponents.AdvTree.ColumnHeader()
        Me.Quantity = New DevComponents.AdvTree.ColumnHeader()
        Me.Price = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector6 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle5 = New DevComponents.DotNetBar.ElementStyle()
        Me.Label1 = New System.Windows.Forms.Label()
        Me._itemsList = New System.Windows.Forms.ComboBox()
        Me.tiPriceSettings = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel6 = New DevComponents.DotNetBar.TabControlPanel()
        Me.gpSelectedPriceGroup = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.btnClearItems = New DevComponents.DotNetBar.ButtonX()
        Me.btnDeleteItem = New DevComponents.DotNetBar.ButtonX()
        Me.btnAddItem = New DevComponents.DotNetBar.ButtonX()
        Me.adtPriceGroupItems = New DevComponents.AdvTree.AdvTree()
        Me.colPriceItemName = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector4 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle3 = New DevComponents.DotNetBar.ElementStyle()
        Me.lblTypeIDs = New DevComponents.DotNetBar.LabelX()
        Me.lblSelectedPriceGroup = New DevComponents.DotNetBar.LabelX()
        Me.lblRegions = New DevComponents.DotNetBar.LabelX()
        Me.adtPriceGroupRegions = New DevComponents.AdvTree.AdvTree()
        Me.colPriceRegions = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector3 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle2 = New DevComponents.DotNetBar.ElementStyle()
        Me.GroupPanel1 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.chkPGMedBuy = New System.Windows.Forms.CheckBox()
        Me.chkPGMedAll = New System.Windows.Forms.CheckBox()
        Me.chkPGAvgBuy = New System.Windows.Forms.CheckBox()
        Me.chkPGMinAll = New System.Windows.Forms.CheckBox()
        Me.chkPGMaxBuy = New System.Windows.Forms.CheckBox()
        Me.chkPGMaxAll = New System.Windows.Forms.CheckBox()
        Me.chkPGMinBuy = New System.Windows.Forms.CheckBox()
        Me.chkPGAvgAll = New System.Windows.Forms.CheckBox()
        Me.chkPGMedSell = New System.Windows.Forms.CheckBox()
        Me.chkPGAvgSell = New System.Windows.Forms.CheckBox()
        Me.chkPGMinSell = New System.Windows.Forms.CheckBox()
        Me.chkPGMaxSell = New System.Windows.Forms.CheckBox()
        Me.btnDeletePriceGroup = New DevComponents.DotNetBar.ButtonX()
        Me.btnEditPriceGroup = New DevComponents.DotNetBar.ButtonX()
        Me.btnAddPriceGroup = New DevComponents.DotNetBar.ButtonX()
        Me.LabelX1 = New DevComponents.DotNetBar.LabelX()
        Me.adtPriceGroups = New DevComponents.AdvTree.AdvTree()
        Me.colPriceGroupName = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector2 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle1 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiPriceGroups = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtLogs = New DevComponents.AdvTree.AdvTree()
        Me.colRegion = New DevComponents.AdvTree.ColumnHeader()
        Me.colItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colDate = New DevComponents.AdvTree.ColumnHeader()
        Me.colAge = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.Log = New DevComponents.DotNetBar.ElementStyle()
        Me.panelLogSettings = New DevComponents.DotNetBar.PanelEx()
        Me.btnRefreshLogs = New DevComponents.DotNetBar.ButtonX()
        Me.tiMarketLogs = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtPrices = New DevComponents.AdvTree.AdvTree()
        Me.colCustomItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomBase = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomMarket = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomCustom = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector5 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle4 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiCustomPrices = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me._buyOrders = New DevComponents.AdvTree.AdvTree()
        Me.ColumnHeader1 = New DevComponents.AdvTree.ColumnHeader()
        Me.ColumnHeader2 = New DevComponents.AdvTree.ColumnHeader()
        Me.ColumnHeader3 = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector7 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle6 = New DevComponents.DotNetBar.ElementStyle()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ColumnHeader4 = New DevComponents.AdvTree.ColumnHeader()
        Me.ColumnHeader5 = New DevComponents.AdvTree.ColumnHeader()
        Me.ctxMarketExport.SuspendLayout()
        CType(Me.nudAge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxPrices.SuspendLayout()
        CType(Me.TabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        Me.gpMarketPrices.SuspendLayout()
        CType(Me._sellOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel6.SuspendLayout()
        Me.gpSelectedPriceGroup.SuspendLayout()
        CType(Me.adtPriceGroupItems, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.adtPriceGroupRegions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupPanel1.SuspendLayout()
        CType(Me.adtPriceGroups, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel3.SuspendLayout()
        CType(Me.adtLogs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelLogSettings.SuspendLayout()
        Me.TabControlPanel4.SuspendLayout()
        CType(Me.adtPrices, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._buyOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'lblHighlightOldLogsText
        '
        Me.lblHighlightOldLogsText.AutoSize = True
        Me.lblHighlightOldLogsText.Location = New System.Drawing.Point(12, 7)
        Me.lblHighlightOldLogsText.Name = "lblHighlightOldLogsText"
        Me.lblHighlightOldLogsText.Size = New System.Drawing.Size(122, 13)
        Me.lblHighlightOldLogsText.TabIndex = 2
        Me.lblHighlightOldLogsText.Text = "Highlight logs older than"
        '
        'ctxMarketExport
        '
        Me.ctxMarketExport.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxMarketExport.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewOrders, Me.mnuDeleteLog})
        Me.ctxMarketExport.Name = "ctxPrices"
        Me.ctxMarketExport.Size = New System.Drawing.Size(142, 48)
        '
        'mnuViewOrders
        '
        Me.mnuViewOrders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuViewOrders.Name = "mnuViewOrders"
        Me.mnuViewOrders.Size = New System.Drawing.Size(141, 22)
        Me.mnuViewOrders.Text = "View Orders"
        '
        'mnuDeleteLog
        '
        Me.mnuDeleteLog.Name = "mnuDeleteLog"
        Me.mnuDeleteLog.Size = New System.Drawing.Size(141, 22)
        Me.mnuDeleteLog.Text = "Delete log"
        '
        'nudAge
        '
        Me.nudAge.Location = New System.Drawing.Point(140, 5)
        Me.nudAge.Maximum = New Decimal(New Integer() {336, 0, 0, 0})
        Me.nudAge.Name = "nudAge"
        Me.nudAge.Size = New System.Drawing.Size(70, 21)
        Me.nudAge.TabIndex = 4
        Me.nudAge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudAge.Value = New Decimal(New Integer() {24, 0, 0, 0})
        '
        'lblHighlightLogsHours
        '
        Me.lblHighlightLogsHours.AutoSize = True
        Me.lblHighlightLogsHours.Location = New System.Drawing.Point(216, 7)
        Me.lblHighlightLogsHours.Name = "lblHighlightLogsHours"
        Me.lblHighlightLogsHours.Size = New System.Drawing.Size(38, 13)
        Me.lblHighlightLogsHours.TabIndex = 3
        Me.lblHighlightLogsHours.Text = "hours."
        '
        'chkShowOnlyCustom
        '
        Me.chkShowOnlyCustom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkShowOnlyCustom.AutoSize = True
        Me.chkShowOnlyCustom.BackColor = System.Drawing.Color.Transparent
        Me.chkShowOnlyCustom.Location = New System.Drawing.Point(375, 706)
        Me.chkShowOnlyCustom.Name = "chkShowOnlyCustom"
        Me.chkShowOnlyCustom.Size = New System.Drawing.Size(147, 17)
        Me.chkShowOnlyCustom.TabIndex = 20
        Me.chkShowOnlyCustom.Text = "Show Only Custom Prices"
        Me.chkShowOnlyCustom.UseVisualStyleBackColor = False
        '
        'lblCustomPrices
        '
        Me.lblCustomPrices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCustomPrices.BackColor = System.Drawing.Color.Transparent
        Me.lblCustomPrices.Location = New System.Drawing.Point(4, 5)
        Me.lblCustomPrices.Name = "lblCustomPrices"
        Me.lblCustomPrices.Size = New System.Drawing.Size(874, 31)
        Me.lblCustomPrices.TabIndex = 19
        Me.lblCustomPrices.Text = "Custom Prices enable you to override the Base and Market prices by allowing you t" & _
    "o enter your own individual figures. Right-click an item to edit or delete a Cus" & _
    "tom price." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnResetGrid
        '
        Me.btnResetGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnResetGrid.Location = New System.Drawing.Point(272, 703)
        Me.btnResetGrid.Name = "btnResetGrid"
        Me.btnResetGrid.Size = New System.Drawing.Size(85, 23)
        Me.btnResetGrid.TabIndex = 18
        Me.btnResetGrid.Text = "Reset Grid"
        Me.btnResetGrid.UseVisualStyleBackColor = True
        '
        'txtSearchPrices
        '
        Me.txtSearchPrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSearchPrices.Location = New System.Drawing.Point(79, 704)
        Me.txtSearchPrices.Name = "txtSearchPrices"
        Me.txtSearchPrices.Size = New System.Drawing.Size(187, 21)
        Me.txtSearchPrices.TabIndex = 17
        '
        'lblSearchPrices
        '
        Me.lblSearchPrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSearchPrices.AutoSize = True
        Me.lblSearchPrices.BackColor = System.Drawing.Color.Transparent
        Me.lblSearchPrices.Location = New System.Drawing.Point(4, 708)
        Me.lblSearchPrices.Name = "lblSearchPrices"
        Me.lblSearchPrices.Size = New System.Drawing.Size(74, 13)
        Me.lblSearchPrices.TabIndex = 16
        Me.lblSearchPrices.Text = "Search Items:"
        '
        'ctxPrices
        '
        Me.ctxPrices.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxPrices.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPriceItemName, Me.ToolStripMenuItem1, Me.mnuPriceAdd, Me.mnuPriceEdit, Me.mnuPriceDelete})
        Me.ctxPrices.Name = "ctxPrices"
        Me.ctxPrices.Size = New System.Drawing.Size(171, 98)
        '
        'mnuPriceItemName
        '
        Me.mnuPriceItemName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuPriceItemName.Name = "mnuPriceItemName"
        Me.mnuPriceItemName.Size = New System.Drawing.Size(170, 22)
        Me.mnuPriceItemName.Text = "Item Name"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(167, 6)
        '
        'mnuPriceAdd
        '
        Me.mnuPriceAdd.Name = "mnuPriceAdd"
        Me.mnuPriceAdd.Size = New System.Drawing.Size(170, 22)
        Me.mnuPriceAdd.Text = "Add Custom Price"
        '
        'mnuPriceEdit
        '
        Me.mnuPriceEdit.Name = "mnuPriceEdit"
        Me.mnuPriceEdit.Size = New System.Drawing.Size(170, 22)
        Me.mnuPriceEdit.Text = "Edit Custom Price"
        '
        'mnuPriceDelete
        '
        Me.mnuPriceDelete.Name = "mnuPriceDelete"
        Me.mnuPriceDelete.Size = New System.Drawing.Size(170, 22)
        Me.mnuPriceDelete.Text = "Delete Custom Price"
        '
        'TabControl1
        '
        Me.TabControl1.BackColor = System.Drawing.Color.Transparent
        Me.TabControl1.CanReorderTabs = True
        Me.TabControl1.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.TabControl1.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.TabControl1.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(249, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(248, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(245, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.TabControl1.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.TabControl1.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer)), 1.0!)})
        Me.TabControl1.Controls.Add(Me.TabControlPanel1)
        Me.TabControl1.Controls.Add(Me.TabControlPanel6)
        Me.TabControl1.Controls.Add(Me.TabControlPanel3)
        Me.TabControl1.Controls.Add(Me.TabControlPanel4)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.TabControl1.SelectedTabIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(882, 759)
        Me.TabControl1.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.TabControl1.TabIndex = 18
        Me.TabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.TabControl1.Tabs.Add(Me.tiPriceSettings)
        Me.TabControl1.Tabs.Add(Me.tiPriceGroups)
        Me.TabControl1.Tabs.Add(Me.tiMarketLogs)
        Me.TabControl1.Tabs.Add(Me.tiCustomPrices)
        Me.TabControl1.Text = "TabControl1"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.gpMarketPrices)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(882, 736)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 1
        Me.TabControlPanel1.TabItem = Me.tiPriceSettings
        '
        'gpMarketPrices
        '
        Me.gpMarketPrices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gpMarketPrices.BackColor = System.Drawing.Color.Transparent
        Me.gpMarketPrices.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpMarketPrices.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpMarketPrices.Controls.Add(Me.Label4)
        Me.gpMarketPrices.Controls.Add(Me.Label2)
        Me.gpMarketPrices.Controls.Add(Me._buyOrders)
        Me.gpMarketPrices.Controls.Add(Me.openMarketSettings)
        Me.gpMarketPrices.Controls.Add(Me._getMarketOrders)
        Me.gpMarketPrices.Controls.Add(Me._sellOrders)
        Me.gpMarketPrices.Controls.Add(Me.Label1)
        Me.gpMarketPrices.Controls.Add(Me._itemsList)
        Me.gpMarketPrices.Location = New System.Drawing.Point(7, 5)
        Me.gpMarketPrices.Name = "gpMarketPrices"
        Me.gpMarketPrices.Size = New System.Drawing.Size(616, 700)
        '
        '
        '
        Me.gpMarketPrices.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpMarketPrices.Style.BackColorGradientAngle = 90
        Me.gpMarketPrices.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpMarketPrices.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMarketPrices.Style.BorderBottomWidth = 1
        Me.gpMarketPrices.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpMarketPrices.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMarketPrices.Style.BorderLeftWidth = 1
        Me.gpMarketPrices.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMarketPrices.Style.BorderRightWidth = 1
        Me.gpMarketPrices.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpMarketPrices.Style.BorderTopWidth = 1
        Me.gpMarketPrices.Style.CornerDiameter = 4
        Me.gpMarketPrices.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpMarketPrices.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpMarketPrices.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpMarketPrices.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpMarketPrices.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpMarketPrices.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpMarketPrices.TabIndex = 3
        Me.gpMarketPrices.Text = "Market Price Check"
        '
        'openMarketSettings
        '
        Me.openMarketSettings.Location = New System.Drawing.Point(210, 645)
        Me.openMarketSettings.Name = "openMarketSettings"
        Me.openMarketSettings.Size = New System.Drawing.Size(169, 24)
        Me.openMarketSettings.TabIndex = 4
        Me.openMarketSettings.Text = "Market Settings"
        Me.openMarketSettings.UseVisualStyleBackColor = True
        '
        '_getMarketOrders
        '
        Me._getMarketOrders.Location = New System.Drawing.Point(457, 45)
        Me._getMarketOrders.Name = "_getMarketOrders"
        Me._getMarketOrders.Size = New System.Drawing.Size(120, 23)
        Me._getMarketOrders.TabIndex = 3
        Me._getMarketOrders.Text = "Get Market Orders"
        Me._getMarketOrders.UseVisualStyleBackColor = True
        '
        '_sellOrders
        '
        Me._sellOrders.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me._sellOrders.AllowDrop = True
        Me._sellOrders.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me._sellOrders.BackgroundStyle.Class = "TreeBorderKey"
        Me._sellOrders.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me._sellOrders.Columns.Add(Me.Location)
        Me._sellOrders.Columns.Add(Me.Quantity)
        Me._sellOrders.Columns.Add(Me.Price)
        Me._sellOrders.Columns.Add(Me.ColumnHeader5)
        Me._sellOrders.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me._sellOrders.Location = New System.Drawing.Point(10, 124)
        Me._sellOrders.Name = "_sellOrders"
        Me._sellOrders.NodesConnector = Me.NodeConnector6
        Me._sellOrders.NodeStyle = Me.ElementStyle5
        Me._sellOrders.PathSeparator = ";"
        Me._sellOrders.Size = New System.Drawing.Size(585, 232)
        Me._sellOrders.Styles.Add(Me.ElementStyle5)
        Me._sellOrders.TabIndex = 2
        Me._sellOrders.Text = "AdvTree1"
        '
        'Location
        '
        Me.Location.Name = "Location"
        Me.Location.Text = "Location"
        Me.Location.Width.Absolute = 150
        '
        'Quantity
        '
        Me.Quantity.Name = "Quantity"
        Me.Quantity.ShowToolTips = False
        Me.Quantity.Text = "Quantity"
        Me.Quantity.Width.Absolute = 150
        '
        'Price
        '
        Me.Price.Name = "Price"
        Me.Price.Text = "Price"
        Me.Price.Width.Absolute = 150
        '
        'NodeConnector6
        '
        Me.NodeConnector6.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle5
        '
        Me.ElementStyle5.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle5.Name = "ElementStyle5"
        Me.ElementStyle5.TextColor = System.Drawing.SystemColors.ControlText
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Choose Item :"
        '
        '_itemsList
        '
        Me._itemsList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me._itemsList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._itemsList.FormattingEnabled = True
        Me._itemsList.Location = New System.Drawing.Point(91, 48)
        Me._itemsList.Name = "_itemsList"
        Me._itemsList.Size = New System.Drawing.Size(348, 21)
        Me._itemsList.TabIndex = 0
        '
        'tiPriceSettings
        '
        Me.tiPriceSettings.AttachedControl = Me.TabControlPanel1
        Me.tiPriceSettings.Name = "tiPriceSettings"
        Me.tiPriceSettings.Text = "Market Prices"
        '
        'TabControlPanel6
        '
        Me.TabControlPanel6.Controls.Add(Me.gpSelectedPriceGroup)
        Me.TabControlPanel6.Controls.Add(Me.btnDeletePriceGroup)
        Me.TabControlPanel6.Controls.Add(Me.btnEditPriceGroup)
        Me.TabControlPanel6.Controls.Add(Me.btnAddPriceGroup)
        Me.TabControlPanel6.Controls.Add(Me.LabelX1)
        Me.TabControlPanel6.Controls.Add(Me.adtPriceGroups)
        Me.TabControlPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel6.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel6.Name = "TabControlPanel6"
        Me.TabControlPanel6.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel6.Size = New System.Drawing.Size(882, 736)
        Me.TabControlPanel6.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel6.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel6.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel6.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel6.Style.GradientAngle = 90
        Me.TabControlPanel6.TabIndex = 6
        Me.TabControlPanel6.TabItem = Me.tiPriceGroups
        '
        'gpSelectedPriceGroup
        '
        Me.gpSelectedPriceGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpSelectedPriceGroup.BackColor = System.Drawing.Color.Transparent
        Me.gpSelectedPriceGroup.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpSelectedPriceGroup.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpSelectedPriceGroup.Controls.Add(Me.btnClearItems)
        Me.gpSelectedPriceGroup.Controls.Add(Me.btnDeleteItem)
        Me.gpSelectedPriceGroup.Controls.Add(Me.btnAddItem)
        Me.gpSelectedPriceGroup.Controls.Add(Me.adtPriceGroupItems)
        Me.gpSelectedPriceGroup.Controls.Add(Me.lblTypeIDs)
        Me.gpSelectedPriceGroup.Controls.Add(Me.lblSelectedPriceGroup)
        Me.gpSelectedPriceGroup.Controls.Add(Me.lblRegions)
        Me.gpSelectedPriceGroup.Controls.Add(Me.adtPriceGroupRegions)
        Me.gpSelectedPriceGroup.Controls.Add(Me.GroupPanel1)
        Me.gpSelectedPriceGroup.Enabled = False
        Me.gpSelectedPriceGroup.Location = New System.Drawing.Point(247, 26)
        Me.gpSelectedPriceGroup.Name = "gpSelectedPriceGroup"
        Me.gpSelectedPriceGroup.Size = New System.Drawing.Size(631, 706)
        '
        '
        '
        Me.gpSelectedPriceGroup.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpSelectedPriceGroup.Style.BackColorGradientAngle = 90
        Me.gpSelectedPriceGroup.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpSelectedPriceGroup.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSelectedPriceGroup.Style.BorderBottomWidth = 1
        Me.gpSelectedPriceGroup.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpSelectedPriceGroup.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSelectedPriceGroup.Style.BorderLeftWidth = 1
        Me.gpSelectedPriceGroup.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSelectedPriceGroup.Style.BorderRightWidth = 1
        Me.gpSelectedPriceGroup.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSelectedPriceGroup.Style.BorderTopWidth = 1
        Me.gpSelectedPriceGroup.Style.CornerDiameter = 4
        Me.gpSelectedPriceGroup.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpSelectedPriceGroup.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpSelectedPriceGroup.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpSelectedPriceGroup.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpSelectedPriceGroup.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpSelectedPriceGroup.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpSelectedPriceGroup.TabIndex = 30
        Me.gpSelectedPriceGroup.Text = "Group Price Details"
        '
        'btnClearItems
        '
        Me.btnClearItems.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnClearItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearItems.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnClearItems.Location = New System.Drawing.Point(352, 656)
        Me.btnClearItems.Name = "btnClearItems"
        Me.btnClearItems.Size = New System.Drawing.Size(75, 23)
        Me.btnClearItems.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnClearItems.TabIndex = 34
        Me.btnClearItems.Text = "Clear All"
        '
        'btnDeleteItem
        '
        Me.btnDeleteItem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnDeleteItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteItem.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnDeleteItem.Enabled = False
        Me.btnDeleteItem.Location = New System.Drawing.Point(271, 656)
        Me.btnDeleteItem.Name = "btnDeleteItem"
        Me.btnDeleteItem.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnDeleteItem.TabIndex = 33
        Me.btnDeleteItem.Text = "Delete Item"
        '
        'btnAddItem
        '
        Me.btnAddItem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAddItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddItem.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAddItem.Location = New System.Drawing.Point(190, 656)
        Me.btnAddItem.Name = "btnAddItem"
        Me.btnAddItem.Size = New System.Drawing.Size(75, 23)
        Me.btnAddItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAddItem.TabIndex = 32
        Me.btnAddItem.Text = "Add Item"
        '
        'adtPriceGroupItems
        '
        Me.adtPriceGroupItems.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtPriceGroupItems.AllowDrop = True
        Me.adtPriceGroupItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtPriceGroupItems.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtPriceGroupItems.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtPriceGroupItems.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtPriceGroupItems.Columns.Add(Me.colPriceItemName)
        Me.adtPriceGroupItems.DragDropEnabled = False
        Me.adtPriceGroupItems.DragDropNodeCopyEnabled = False
        Me.adtPriceGroupItems.ExpandWidth = 0
        Me.adtPriceGroupItems.GridRowLines = True
        Me.adtPriceGroupItems.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtPriceGroupItems.Location = New System.Drawing.Point(190, 173)
        Me.adtPriceGroupItems.MultiSelect = True
        Me.adtPriceGroupItems.Name = "adtPriceGroupItems"
        Me.adtPriceGroupItems.NodesConnector = Me.NodeConnector4
        Me.adtPriceGroupItems.NodeStyle = Me.ElementStyle3
        Me.adtPriceGroupItems.PathSeparator = ";"
        Me.adtPriceGroupItems.SearchBufferExpireTimeout = 3000
        Me.adtPriceGroupItems.Size = New System.Drawing.Size(430, 477)
        Me.adtPriceGroupItems.Styles.Add(Me.ElementStyle3)
        Me.adtPriceGroupItems.TabIndex = 31
        Me.adtPriceGroupItems.Text = "AdvTree1"
        '
        'colPriceItemName
        '
        Me.colPriceItemName.Name = "colPriceItemName"
        Me.colPriceItemName.Text = "Item Name"
        Me.colPriceItemName.Width.Absolute = 330
        '
        'NodeConnector4
        '
        Me.NodeConnector4.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle3
        '
        Me.ElementStyle3.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle3.Name = "ElementStyle3"
        Me.ElementStyle3.TextColor = System.Drawing.SystemColors.ControlText
        '
        'lblTypeIDs
        '
        Me.lblTypeIDs.AutoSize = True
        Me.lblTypeIDs.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblTypeIDs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblTypeIDs.Location = New System.Drawing.Point(190, 151)
        Me.lblTypeIDs.Name = "lblTypeIDs"
        Me.lblTypeIDs.Size = New System.Drawing.Size(78, 16)
        Me.lblTypeIDs.TabIndex = 30
        Me.lblTypeIDs.Text = "Selected Items:"
        '
        'lblSelectedPriceGroup
        '
        Me.lblSelectedPriceGroup.AutoSize = True
        Me.lblSelectedPriceGroup.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblSelectedPriceGroup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblSelectedPriceGroup.Location = New System.Drawing.Point(3, 3)
        Me.lblSelectedPriceGroup.Name = "lblSelectedPriceGroup"
        Me.lblSelectedPriceGroup.Size = New System.Drawing.Size(150, 16)
        Me.lblSelectedPriceGroup.TabIndex = 26
        Me.lblSelectedPriceGroup.Text = "Selected Price Group: <none>"
        '
        'lblRegions
        '
        Me.lblRegions.AutoSize = True
        Me.lblRegions.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblRegions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblRegions.Location = New System.Drawing.Point(3, 151)
        Me.lblRegions.Name = "lblRegions"
        Me.lblRegions.Size = New System.Drawing.Size(89, 16)
        Me.lblRegions.TabIndex = 29
        Me.lblRegions.Text = "Selected Regions:"
        '
        'adtPriceGroupRegions
        '
        Me.adtPriceGroupRegions.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtPriceGroupRegions.AllowDrop = True
        Me.adtPriceGroupRegions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.adtPriceGroupRegions.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtPriceGroupRegions.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtPriceGroupRegions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtPriceGroupRegions.Columns.Add(Me.colPriceRegions)
        Me.adtPriceGroupRegions.DragDropEnabled = False
        Me.adtPriceGroupRegions.DragDropNodeCopyEnabled = False
        Me.adtPriceGroupRegions.ExpandWidth = 0
        Me.adtPriceGroupRegions.GridRowLines = True
        Me.adtPriceGroupRegions.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtPriceGroupRegions.Location = New System.Drawing.Point(3, 173)
        Me.adtPriceGroupRegions.Name = "adtPriceGroupRegions"
        Me.adtPriceGroupRegions.NodesConnector = Me.NodeConnector3
        Me.adtPriceGroupRegions.NodeStyle = Me.ElementStyle2
        Me.adtPriceGroupRegions.PathSeparator = ";"
        Me.adtPriceGroupRegions.Size = New System.Drawing.Size(178, 506)
        Me.adtPriceGroupRegions.Styles.Add(Me.ElementStyle2)
        Me.adtPriceGroupRegions.TabIndex = 28
        Me.adtPriceGroupRegions.Text = "AdvTree1"
        '
        'colPriceRegions
        '
        Me.colPriceRegions.Name = "colPriceRegions"
        Me.colPriceRegions.Text = "EveGalaticRegion Name"
        Me.colPriceRegions.Width.Absolute = 155
        '
        'NodeConnector3
        '
        Me.NodeConnector3.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle2
        '
        Me.ElementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle2.Name = "ElementStyle2"
        Me.ElementStyle2.TextColor = System.Drawing.SystemColors.ControlText
        '
        'GroupPanel1
        '
        Me.GroupPanel1.BackColor = System.Drawing.Color.Transparent
        Me.GroupPanel1.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.GroupPanel1.Controls.Add(Me.chkPGMedBuy)
        Me.GroupPanel1.Controls.Add(Me.chkPGMedAll)
        Me.GroupPanel1.Controls.Add(Me.chkPGAvgBuy)
        Me.GroupPanel1.Controls.Add(Me.chkPGMinAll)
        Me.GroupPanel1.Controls.Add(Me.chkPGMaxBuy)
        Me.GroupPanel1.Controls.Add(Me.chkPGMaxAll)
        Me.GroupPanel1.Controls.Add(Me.chkPGMinBuy)
        Me.GroupPanel1.Controls.Add(Me.chkPGAvgAll)
        Me.GroupPanel1.Controls.Add(Me.chkPGMedSell)
        Me.GroupPanel1.Controls.Add(Me.chkPGAvgSell)
        Me.GroupPanel1.Controls.Add(Me.chkPGMinSell)
        Me.GroupPanel1.Controls.Add(Me.chkPGMaxSell)
        Me.GroupPanel1.Location = New System.Drawing.Point(3, 25)
        Me.GroupPanel1.Name = "GroupPanel1"
        Me.GroupPanel1.Size = New System.Drawing.Size(363, 120)
        '
        '
        '
        Me.GroupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.GroupPanel1.Style.BackColorGradientAngle = 90
        Me.GroupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.GroupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderBottomWidth = 1
        Me.GroupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderLeftWidth = 1
        Me.GroupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderRightWidth = 1
        Me.GroupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderTopWidth = 1
        Me.GroupPanel1.Style.CornerDiameter = 4
        Me.GroupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.GroupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.GroupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel1.TabIndex = 27
        Me.GroupPanel1.Text = "Price Criteria Flags"
        '
        'chkPGMedBuy
        '
        Me.chkPGMedBuy.AutoSize = True
        Me.chkPGMedBuy.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMedBuy.Location = New System.Drawing.Point(117, 72)
        Me.chkPGMedBuy.Name = "chkPGMedBuy"
        Me.chkPGMedBuy.Size = New System.Drawing.Size(115, 17)
        Me.chkPGMedBuy.TabIndex = 3
        Me.chkPGMedBuy.Tag = "1024"
        Me.chkPGMedBuy.Text = "Median Price (Buy)"
        Me.chkPGMedBuy.UseVisualStyleBackColor = False
        '
        'chkPGMedAll
        '
        Me.chkPGMedAll.AutoSize = True
        Me.chkPGMedAll.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMedAll.Location = New System.Drawing.Point(3, 72)
        Me.chkPGMedAll.Name = "chkPGMedAll"
        Me.chkPGMedAll.Size = New System.Drawing.Size(108, 17)
        Me.chkPGMedAll.TabIndex = 11
        Me.chkPGMedAll.Tag = "512"
        Me.chkPGMedAll.Text = "Median Price (All)"
        Me.chkPGMedAll.UseVisualStyleBackColor = False
        '
        'chkPGAvgBuy
        '
        Me.chkPGAvgBuy.AutoSize = True
        Me.chkPGAvgBuy.BackColor = System.Drawing.Color.Transparent
        Me.chkPGAvgBuy.Location = New System.Drawing.Point(117, 49)
        Me.chkPGAvgBuy.Name = "chkPGAvgBuy"
        Me.chkPGAvgBuy.Size = New System.Drawing.Size(107, 17)
        Me.chkPGAvgBuy.TabIndex = 0
        Me.chkPGAvgBuy.Tag = "128"
        Me.chkPGAvgBuy.Text = "Mean Price (Buy)"
        Me.chkPGAvgBuy.UseVisualStyleBackColor = False
        '
        'chkPGMinAll
        '
        Me.chkPGMinAll.AutoSize = True
        Me.chkPGMinAll.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMinAll.Location = New System.Drawing.Point(3, 3)
        Me.chkPGMinAll.Name = "chkPGMinAll"
        Me.chkPGMinAll.Size = New System.Drawing.Size(90, 17)
        Me.chkPGMinAll.TabIndex = 10
        Me.chkPGMinAll.Tag = "1"
        Me.chkPGMinAll.Text = "Min Price (All)"
        Me.chkPGMinAll.UseVisualStyleBackColor = False
        '
        'chkPGMaxBuy
        '
        Me.chkPGMaxBuy.AutoSize = True
        Me.chkPGMaxBuy.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMaxBuy.Location = New System.Drawing.Point(117, 26)
        Me.chkPGMaxBuy.Name = "chkPGMaxBuy"
        Me.chkPGMaxBuy.Size = New System.Drawing.Size(101, 17)
        Me.chkPGMaxBuy.TabIndex = 1
        Me.chkPGMaxBuy.Tag = "16"
        Me.chkPGMaxBuy.Text = "Max Price (Buy)"
        Me.chkPGMaxBuy.UseVisualStyleBackColor = False
        '
        'chkPGMaxAll
        '
        Me.chkPGMaxAll.AutoSize = True
        Me.chkPGMaxAll.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMaxAll.Location = New System.Drawing.Point(3, 26)
        Me.chkPGMaxAll.Name = "chkPGMaxAll"
        Me.chkPGMaxAll.Size = New System.Drawing.Size(94, 17)
        Me.chkPGMaxAll.TabIndex = 9
        Me.chkPGMaxAll.Tag = "8"
        Me.chkPGMaxAll.Text = "Max Price (All)"
        Me.chkPGMaxAll.UseVisualStyleBackColor = False
        '
        'chkPGMinBuy
        '
        Me.chkPGMinBuy.AutoSize = True
        Me.chkPGMinBuy.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMinBuy.Location = New System.Drawing.Point(117, 3)
        Me.chkPGMinBuy.Name = "chkPGMinBuy"
        Me.chkPGMinBuy.Size = New System.Drawing.Size(97, 17)
        Me.chkPGMinBuy.TabIndex = 2
        Me.chkPGMinBuy.Tag = "2"
        Me.chkPGMinBuy.Text = "Min Price (Buy)"
        Me.chkPGMinBuy.UseVisualStyleBackColor = False
        '
        'chkPGAvgAll
        '
        Me.chkPGAvgAll.AutoSize = True
        Me.chkPGAvgAll.BackColor = System.Drawing.Color.Transparent
        Me.chkPGAvgAll.Location = New System.Drawing.Point(3, 49)
        Me.chkPGAvgAll.Name = "chkPGAvgAll"
        Me.chkPGAvgAll.Size = New System.Drawing.Size(100, 17)
        Me.chkPGAvgAll.TabIndex = 8
        Me.chkPGAvgAll.Tag = "64"
        Me.chkPGAvgAll.Text = "Mean Price (All)"
        Me.chkPGAvgAll.UseVisualStyleBackColor = False
        '
        'chkPGMedSell
        '
        Me.chkPGMedSell.AutoSize = True
        Me.chkPGMedSell.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMedSell.Location = New System.Drawing.Point(238, 72)
        Me.chkPGMedSell.Name = "chkPGMedSell"
        Me.chkPGMedSell.Size = New System.Drawing.Size(113, 17)
        Me.chkPGMedSell.TabIndex = 7
        Me.chkPGMedSell.Tag = "2048"
        Me.chkPGMedSell.Text = "Median Price (Sell)"
        Me.chkPGMedSell.UseVisualStyleBackColor = False
        '
        'chkPGAvgSell
        '
        Me.chkPGAvgSell.AutoSize = True
        Me.chkPGAvgSell.BackColor = System.Drawing.Color.Transparent
        Me.chkPGAvgSell.Location = New System.Drawing.Point(238, 49)
        Me.chkPGAvgSell.Name = "chkPGAvgSell"
        Me.chkPGAvgSell.Size = New System.Drawing.Size(105, 17)
        Me.chkPGAvgSell.TabIndex = 4
        Me.chkPGAvgSell.Tag = "256"
        Me.chkPGAvgSell.Text = "Mean Price (Sell)"
        Me.chkPGAvgSell.UseVisualStyleBackColor = False
        '
        'chkPGMinSell
        '
        Me.chkPGMinSell.AutoSize = True
        Me.chkPGMinSell.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMinSell.Location = New System.Drawing.Point(238, 3)
        Me.chkPGMinSell.Name = "chkPGMinSell"
        Me.chkPGMinSell.Size = New System.Drawing.Size(95, 17)
        Me.chkPGMinSell.TabIndex = 6
        Me.chkPGMinSell.Tag = "4"
        Me.chkPGMinSell.Text = "Min Price (Sell)"
        Me.chkPGMinSell.UseVisualStyleBackColor = False
        '
        'chkPGMaxSell
        '
        Me.chkPGMaxSell.AutoSize = True
        Me.chkPGMaxSell.BackColor = System.Drawing.Color.Transparent
        Me.chkPGMaxSell.Location = New System.Drawing.Point(238, 26)
        Me.chkPGMaxSell.Name = "chkPGMaxSell"
        Me.chkPGMaxSell.Size = New System.Drawing.Size(99, 17)
        Me.chkPGMaxSell.TabIndex = 5
        Me.chkPGMaxSell.Tag = "32"
        Me.chkPGMaxSell.Text = "Max Price (Sell)"
        Me.chkPGMaxSell.UseVisualStyleBackColor = False
        '
        'btnDeletePriceGroup
        '
        Me.btnDeletePriceGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnDeletePriceGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeletePriceGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnDeletePriceGroup.Location = New System.Drawing.Point(166, 709)
        Me.btnDeletePriceGroup.Name = "btnDeletePriceGroup"
        Me.btnDeletePriceGroup.Size = New System.Drawing.Size(75, 23)
        Me.btnDeletePriceGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnDeletePriceGroup.TabIndex = 25
        Me.btnDeletePriceGroup.Text = "Delete Group"
        '
        'btnEditPriceGroup
        '
        Me.btnEditPriceGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnEditPriceGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditPriceGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnEditPriceGroup.Location = New System.Drawing.Point(85, 709)
        Me.btnEditPriceGroup.Name = "btnEditPriceGroup"
        Me.btnEditPriceGroup.Size = New System.Drawing.Size(75, 23)
        Me.btnEditPriceGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnEditPriceGroup.TabIndex = 24
        Me.btnEditPriceGroup.Text = "Edit Group"
        '
        'btnAddPriceGroup
        '
        Me.btnAddPriceGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAddPriceGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddPriceGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAddPriceGroup.Location = New System.Drawing.Point(4, 709)
        Me.btnAddPriceGroup.Name = "btnAddPriceGroup"
        Me.btnAddPriceGroup.Size = New System.Drawing.Size(75, 23)
        Me.btnAddPriceGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAddPriceGroup.TabIndex = 23
        Me.btnAddPriceGroup.Text = "Add Group"
        '
        'LabelX1
        '
        Me.LabelX1.AutoSize = True
        Me.LabelX1.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX1.Location = New System.Drawing.Point(4, 4)
        Me.LabelX1.Name = "LabelX1"
        Me.LabelX1.Size = New System.Drawing.Size(109, 16)
        Me.LabelX1.TabIndex = 22
        Me.LabelX1.Text = "Existing Price Groups:"
        '
        'adtPriceGroups
        '
        Me.adtPriceGroups.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtPriceGroups.AllowDrop = True
        Me.adtPriceGroups.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.adtPriceGroups.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtPriceGroups.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtPriceGroups.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtPriceGroups.Columns.Add(Me.colPriceGroupName)
        Me.adtPriceGroups.ExpandWidth = 0
        Me.adtPriceGroups.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtPriceGroups.Location = New System.Drawing.Point(4, 26)
        Me.adtPriceGroups.Name = "adtPriceGroups"
        Me.adtPriceGroups.NodesConnector = Me.NodeConnector2
        Me.adtPriceGroups.NodeStyle = Me.ElementStyle1
        Me.adtPriceGroups.PathSeparator = ";"
        Me.adtPriceGroups.Size = New System.Drawing.Size(237, 677)
        Me.adtPriceGroups.Styles.Add(Me.ElementStyle1)
        Me.adtPriceGroups.TabIndex = 21
        Me.adtPriceGroups.Text = "AdvTree1"
        '
        'colPriceGroupName
        '
        Me.colPriceGroupName.Name = "colPriceGroupName"
        Me.colPriceGroupName.SortingEnabled = False
        Me.colPriceGroupName.Text = "Price Group Name"
        Me.colPriceGroupName.Width.Absolute = 215
        '
        'NodeConnector2
        '
        Me.NodeConnector2.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle1
        '
        Me.ElementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle1.Name = "ElementStyle1"
        Me.ElementStyle1.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiPriceGroups
        '
        Me.tiPriceGroups.AttachedControl = Me.TabControlPanel6
        Me.tiPriceGroups.Name = "tiPriceGroups"
        Me.tiPriceGroups.Text = "Price Groups"
        '
        'TabControlPanel3
        '
        Me.TabControlPanel3.Controls.Add(Me.adtLogs)
        Me.TabControlPanel3.Controls.Add(Me.panelLogSettings)
        Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel3.Name = "TabControlPanel3"
        Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel3.Size = New System.Drawing.Size(882, 736)
        Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel3.Style.GradientAngle = 90
        Me.TabControlPanel3.TabIndex = 3
        Me.TabControlPanel3.TabItem = Me.tiMarketLogs
        '
        'adtLogs
        '
        Me.adtLogs.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtLogs.AllowDrop = True
        Me.adtLogs.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtLogs.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtLogs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtLogs.Columns.Add(Me.colRegion)
        Me.adtLogs.Columns.Add(Me.colItem)
        Me.adtLogs.Columns.Add(Me.colDate)
        Me.adtLogs.Columns.Add(Me.colAge)
        Me.adtLogs.ContextMenuStrip = Me.ctxMarketExport
        Me.adtLogs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtLogs.DragDropEnabled = False
        Me.adtLogs.DragDropNodeCopyEnabled = False
        Me.adtLogs.ExpandWidth = 0
        Me.adtLogs.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtLogs.Location = New System.Drawing.Point(1, 31)
        Me.adtLogs.MultiSelect = True
        Me.adtLogs.Name = "adtLogs"
        Me.adtLogs.NodesConnector = Me.NodeConnector1
        Me.adtLogs.NodeStyle = Me.Log
        Me.adtLogs.PathSeparator = ";"
        Me.adtLogs.Size = New System.Drawing.Size(880, 704)
        Me.adtLogs.Styles.Add(Me.Log)
        Me.adtLogs.TabIndex = 2
        '
        'colRegion
        '
        Me.colRegion.DisplayIndex = 1
        Me.colRegion.Name = "colRegion"
        Me.colRegion.SortingEnabled = False
        Me.colRegion.Text = "EveGalaticRegion"
        Me.colRegion.Width.Absolute = 200
        '
        'colItem
        '
        Me.colItem.DisplayIndex = 2
        Me.colItem.Name = "colItem"
        Me.colItem.SortingEnabled = False
        Me.colItem.Text = "Item Name"
        Me.colItem.Width.Absolute = 400
        '
        'colDate
        '
        Me.colDate.DisplayIndex = 3
        Me.colDate.Name = "colDate"
        Me.colDate.SortingEnabled = False
        Me.colDate.Text = "Log Date"
        Me.colDate.Width.Absolute = 120
        '
        'colAge
        '
        Me.colAge.DisplayIndex = 4
        Me.colAge.Name = "colAge"
        Me.colAge.SortingEnabled = False
        Me.colAge.Text = "Log Age (Hrs)"
        Me.colAge.Width.Absolute = 120
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        '
        'Log
        '
        Me.Log.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Log.Name = "Log"
        Me.Log.TextColor = System.Drawing.SystemColors.ControlText
        '
        'panelLogSettings
        '
        Me.panelLogSettings.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelLogSettings.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelLogSettings.Controls.Add(Me.btnRefreshLogs)
        Me.panelLogSettings.Controls.Add(Me.nudAge)
        Me.panelLogSettings.Controls.Add(Me.lblHighlightLogsHours)
        Me.panelLogSettings.Controls.Add(Me.lblHighlightOldLogsText)
        Me.panelLogSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelLogSettings.Location = New System.Drawing.Point(1, 1)
        Me.panelLogSettings.Name = "panelLogSettings"
        Me.panelLogSettings.Size = New System.Drawing.Size(880, 30)
        Me.panelLogSettings.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelLogSettings.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelLogSettings.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelLogSettings.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelLogSettings.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelLogSettings.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelLogSettings.Style.GradientAngle = 90
        Me.panelLogSettings.TabIndex = 0
        '
        'btnRefreshLogs
        '
        Me.btnRefreshLogs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnRefreshLogs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnRefreshLogs.Location = New System.Drawing.Point(266, 4)
        Me.btnRefreshLogs.Name = "btnRefreshLogs"
        Me.btnRefreshLogs.Size = New System.Drawing.Size(100, 23)
        Me.btnRefreshLogs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnRefreshLogs.TabIndex = 5
        Me.btnRefreshLogs.Text = "Refresh Logs"
        '
        'tiMarketLogs
        '
        Me.tiMarketLogs.AttachedControl = Me.TabControlPanel3
        Me.tiMarketLogs.Name = "tiMarketLogs"
        Me.tiMarketLogs.Text = "Market Log Import"
        '
        'TabControlPanel4
        '
        Me.TabControlPanel4.Controls.Add(Me.adtPrices)
        Me.TabControlPanel4.Controls.Add(Me.chkShowOnlyCustom)
        Me.TabControlPanel4.Controls.Add(Me.lblCustomPrices)
        Me.TabControlPanel4.Controls.Add(Me.lblSearchPrices)
        Me.TabControlPanel4.Controls.Add(Me.txtSearchPrices)
        Me.TabControlPanel4.Controls.Add(Me.btnResetGrid)
        Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel4.Name = "TabControlPanel4"
        Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel4.Size = New System.Drawing.Size(882, 736)
        Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(165, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel4.Style.GradientAngle = 90
        Me.TabControlPanel4.TabIndex = 4
        Me.TabControlPanel4.TabItem = Me.tiCustomPrices
        '
        'adtPrices
        '
        Me.adtPrices.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtPrices.AllowDrop = True
        Me.adtPrices.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtPrices.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtPrices.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtPrices.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtPrices.Columns.Add(Me.colCustomItem)
        Me.adtPrices.Columns.Add(Me.colCustomBase)
        Me.adtPrices.Columns.Add(Me.colCustomMarket)
        Me.adtPrices.Columns.Add(Me.colCustomCustom)
        Me.adtPrices.ContextMenuStrip = Me.ctxPrices
        Me.adtPrices.ExpandWidth = 0
        Me.adtPrices.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtPrices.Location = New System.Drawing.Point(4, 39)
        Me.adtPrices.MultiSelect = True
        Me.adtPrices.Name = "adtPrices"
        Me.adtPrices.NodesConnector = Me.NodeConnector5
        Me.adtPrices.NodeSpacing = 1
        Me.adtPrices.NodeStyle = Me.ElementStyle4
        Me.adtPrices.PathSeparator = ";"
        Me.adtPrices.Size = New System.Drawing.Size(874, 659)
        Me.adtPrices.Styles.Add(Me.ElementStyle4)
        Me.adtPrices.TabIndex = 21
        Me.adtPrices.Text = "AdvTree1"
        '
        'colCustomItem
        '
        Me.colCustomItem.DisplayIndex = 1
        Me.colCustomItem.Name = "colCustomItem"
        Me.colCustomItem.Text = "Item Name"
        Me.colCustomItem.Width.Absolute = 300
        '
        'colCustomBase
        '
        Me.colCustomBase.DisplayIndex = 2
        Me.colCustomBase.Name = "colCustomBase"
        Me.colCustomBase.SortingEnabled = False
        Me.colCustomBase.Text = "Base Price"
        Me.colCustomBase.Width.Absolute = 150
        '
        'colCustomMarket
        '
        Me.colCustomMarket.DisplayIndex = 3
        Me.colCustomMarket.Name = "colCustomMarket"
        Me.colCustomMarket.SortingEnabled = False
        Me.colCustomMarket.Text = "Market Price"
        Me.colCustomMarket.Width.Absolute = 150
        '
        'colCustomCustom
        '
        Me.colCustomCustom.DisplayIndex = 4
        Me.colCustomCustom.Name = "colCustomCustom"
        Me.colCustomCustom.SortingEnabled = False
        Me.colCustomCustom.Text = "Custom Price"
        Me.colCustomCustom.Width.Absolute = 150
        '
        'NodeConnector5
        '
        Me.NodeConnector5.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle4
        '
        Me.ElementStyle4.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle4.Name = "ElementStyle4"
        Me.ElementStyle4.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiCustomPrices
        '
        Me.tiCustomPrices.AttachedControl = Me.TabControlPanel4
        Me.tiCustomPrices.Name = "tiCustomPrices"
        Me.tiCustomPrices.Text = "Custom Prices"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 108)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Sell Orders"
        '
        '_buyOrders
        '
        Me._buyOrders.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me._buyOrders.AllowDrop = True
        Me._buyOrders.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me._buyOrders.BackgroundStyle.Class = "TreeBorderKey"
        Me._buyOrders.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me._buyOrders.Columns.Add(Me.ColumnHeader1)
        Me._buyOrders.Columns.Add(Me.ColumnHeader2)
        Me._buyOrders.Columns.Add(Me.ColumnHeader3)
        Me._buyOrders.Columns.Add(Me.ColumnHeader4)
        Me._buyOrders.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me._buyOrders.Location = New System.Drawing.Point(10, 395)
        Me._buyOrders.Name = "_buyOrders"
        Me._buyOrders.NodesConnector = Me.NodeConnector7
        Me._buyOrders.NodeStyle = Me.ElementStyle6
        Me._buyOrders.PathSeparator = ";"
        Me._buyOrders.Size = New System.Drawing.Size(585, 169)
        Me._buyOrders.Styles.Add(Me.ElementStyle6)
        Me._buyOrders.TabIndex = 6
        Me._buyOrders.Text = "AdvTree1"
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Name = "ColumnHeader1"
        Me.ColumnHeader1.Text = "Location"
        Me.ColumnHeader1.Width.Absolute = 150
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Name = "ColumnHeader2"
        Me.ColumnHeader2.ShowToolTips = False
        Me.ColumnHeader2.Text = "Quantity"
        Me.ColumnHeader2.Width.Absolute = 150
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Name = "ColumnHeader3"
        Me.ColumnHeader3.Text = "Price"
        Me.ColumnHeader3.Width.Absolute = 150
        '
        'NodeConnector7
        '
        Me.NodeConnector7.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle6
        '
        Me.ElementStyle6.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle6.Name = "ElementStyle6"
        Me.ElementStyle6.TextColor = System.Drawing.SystemColors.ControlText
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 377)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Buy Orders"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Name = "ColumnHeader4"
        Me.ColumnHeader4.Text = "Expires"
        Me.ColumnHeader4.Width.Absolute = 150
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Name = "ColumnHeader5"
        Me.ColumnHeader5.Text = "Expires"
        Me.ColumnHeader5.Width.Absolute = 150
        '
        'frmMarketPrices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(882, 759)
        Me.Controls.Add(Me.TabControl1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketPrices"
        Me.Text = "Market Prices"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ctxMarketExport.ResumeLayout(False)
        CType(Me.nudAge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxPrices.ResumeLayout(False)
        CType(Me.TabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        Me.gpMarketPrices.ResumeLayout(False)
        Me.gpMarketPrices.PerformLayout()
        CType(Me._sellOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel6.ResumeLayout(False)
        Me.TabControlPanel6.PerformLayout()
        Me.gpSelectedPriceGroup.ResumeLayout(False)
        Me.gpSelectedPriceGroup.PerformLayout()
        CType(Me.adtPriceGroupItems, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.adtPriceGroupRegions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupPanel1.ResumeLayout(False)
        Me.GroupPanel1.PerformLayout()
        CType(Me.adtPriceGroups, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel3.ResumeLayout(False)
        CType(Me.adtLogs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelLogSettings.ResumeLayout(False)
        Me.panelLogSettings.PerformLayout()
        Me.TabControlPanel4.ResumeLayout(False)
        Me.TabControlPanel4.PerformLayout()
        CType(Me.adtPrices, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._buyOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblCustomPrices As System.Windows.Forms.Label
    Friend WithEvents btnResetGrid As System.Windows.Forms.Button
    Friend WithEvents txtSearchPrices As System.Windows.Forms.TextBox
    Friend WithEvents lblSearchPrices As System.Windows.Forms.Label
    Friend WithEvents ctxPrices As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPriceItemName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPriceEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPriceAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkShowOnlyCustom As System.Windows.Forms.CheckBox
    Friend WithEvents ctxMarketExport As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuViewOrders As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblHighlightOldLogsText As System.Windows.Forms.Label
    Friend WithEvents mnuDeleteLog As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblHighlightLogsHours As System.Windows.Forms.Label
    Friend WithEvents nudAge As System.Windows.Forms.NumericUpDown
    Friend WithEvents TabControl1 As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiPriceSettings As DevComponents.DotNetBar.TabItem
    Friend WithEvents gpMarketPrices As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiCustomPrices As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiMarketLogs As DevComponents.DotNetBar.TabItem
    Friend WithEvents panelLogSettings As DevComponents.DotNetBar.PanelEx
    Friend WithEvents adtLogs As DevComponents.AdvTree.AdvTree
    Friend WithEvents colRegion As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colDate As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colAge As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents Log As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents TabControlPanel6 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents btnDeletePriceGroup As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnEditPriceGroup As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAddPriceGroup As DevComponents.DotNetBar.ButtonX
    Friend WithEvents LabelX1 As DevComponents.DotNetBar.LabelX
    Friend WithEvents adtPriceGroups As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector2 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle1 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents tiPriceGroups As DevComponents.DotNetBar.TabItem
    Friend WithEvents lblSelectedPriceGroup As DevComponents.DotNetBar.LabelX
    Friend WithEvents colPriceGroupName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents GroupPanel1 As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents chkPGMedAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGAvgBuy As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMinAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMaxBuy As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMaxAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMinBuy As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGAvgAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMedBuy As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMedSell As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGAvgSell As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMinSell As System.Windows.Forms.CheckBox
    Friend WithEvents chkPGMaxSell As System.Windows.Forms.CheckBox
    Friend WithEvents gpSelectedPriceGroup As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lblRegions As DevComponents.DotNetBar.LabelX
    Friend WithEvents adtPriceGroupRegions As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector3 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle2 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents adtPriceGroupItems As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector4 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle3 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents lblTypeIDs As DevComponents.DotNetBar.LabelX
    Friend WithEvents btnDeleteItem As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAddItem As DevComponents.DotNetBar.ButtonX
    Friend WithEvents colPriceItemName As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colPriceRegions As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents btnClearItems As DevComponents.DotNetBar.ButtonX
    Friend WithEvents adtPrices As DevComponents.AdvTree.AdvTree
    Friend WithEvents colCustomItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomBase As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomMarket As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomCustom As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector5 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle4 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents btnRefreshLogs As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents openMarketSettings As System.Windows.Forms.Button
    Friend WithEvents _sellOrders As DevComponents.AdvTree.AdvTree
    Friend WithEvents Location As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents Quantity As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents Price As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector6 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle5 As DevComponents.DotNetBar.ElementStyle
    Private WithEvents _getMarketOrders As System.Windows.Forms.Button
    Private WithEvents _itemsList As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents _buyOrders As DevComponents.AdvTree.AdvTree
    Friend WithEvents ColumnHeader1 As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents ColumnHeader2 As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents ColumnHeader3 As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector7 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle6 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents ColumnHeader4 As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents ColumnHeader5 As DevComponents.AdvTree.ColumnHeader
End Class
