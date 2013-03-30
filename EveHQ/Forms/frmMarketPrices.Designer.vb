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
        Me.ctxMarketExport = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuViewOrders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeleteLog = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me._buyOrders = New DevComponents.AdvTree.AdvTree()
        Me._buyLocation = New DevComponents.AdvTree.ColumnHeader()
        Me._buyQuantity = New DevComponents.AdvTree.ColumnHeader()
        Me._buyPrice = New DevComponents.AdvTree.ColumnHeader()
        Me._buyExpires = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector7 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle6 = New DevComponents.DotNetBar.ElementStyle()
        Me.openMarketSettings = New System.Windows.Forms.Button()
        Me._getMarketOrders = New System.Windows.Forms.Button()
        Me._sellOrders = New DevComponents.AdvTree.AdvTree()
        Me._sellLocation = New DevComponents.AdvTree.ColumnHeader()
        Me._sellQuantity = New DevComponents.AdvTree.ColumnHeader()
        Me._sellPrice = New DevComponents.AdvTree.ColumnHeader()
        Me._sellExpires = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector6 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle5 = New DevComponents.DotNetBar.ElementStyle()
        Me.Label1 = New System.Windows.Forms.Label()
        Me._itemsList = New System.Windows.Forms.ComboBox()
        Me.tiPriceSettings = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtPrices = New DevComponents.AdvTree.AdvTree()
        Me.colCustomItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomBase = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomMarket = New DevComponents.AdvTree.ColumnHeader()
        Me.colCustomCustom = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector5 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle4 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiCustomPrices = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.ctxMarketExport.SuspendLayout()
        Me.ctxPrices.SuspendLayout()
        CType(Me.TabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        Me.gpMarketPrices.SuspendLayout()
        CType(Me._buyOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._sellOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel4.SuspendLayout()
        CType(Me.adtPrices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
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
        Me.gpMarketPrices.AutoSize = True
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
        Me.gpMarketPrices.Size = New System.Drawing.Size(863, 700)
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
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 377)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Buy Orders"
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
        Me._buyOrders.Columns.Add(Me._buyLocation)
        Me._buyOrders.Columns.Add(Me._buyQuantity)
        Me._buyOrders.Columns.Add(Me._buyPrice)
        Me._buyOrders.Columns.Add(Me._buyExpires)
        Me._buyOrders.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me._buyOrders.Location = New System.Drawing.Point(10, 395)
        Me._buyOrders.Name = "_buyOrders"
        Me._buyOrders.NodesConnector = Me.NodeConnector7
        Me._buyOrders.NodeStyle = Me.ElementStyle6
        Me._buyOrders.PathSeparator = ";"
        Me._buyOrders.Size = New System.Drawing.Size(836, 226)
        Me._buyOrders.Styles.Add(Me.ElementStyle6)
        Me._buyOrders.TabIndex = 6
        Me._buyOrders.Text = "AdvTree1"
        '
        '_buyLocation
        '
        Me._buyLocation.Name = "_buyLocation"
        Me._buyLocation.StretchToFill = True
        Me._buyLocation.Text = "Location"
        Me._buyLocation.Width.Absolute = 150
        '
        '_buyQuantity
        '
        Me._buyQuantity.Name = "_buyQuantity"
        Me._buyQuantity.ShowToolTips = False
        Me._buyQuantity.Text = "Quantity"
        Me._buyQuantity.Width.Absolute = 150
        '
        '_buyPrice
        '
        Me._buyPrice.Name = "_buyPrice"
        Me._buyPrice.Text = "Price"
        Me._buyPrice.Width.Absolute = 150
        '
        '_buyExpires
        '
        Me._buyExpires.Name = "_buyExpires"
        Me._buyExpires.Text = "Expires"
        Me._buyExpires.Width.Absolute = 150
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
        'openMarketSettings
        '
        Me.openMarketSettings.Location = New System.Drawing.Point(343, 644)
        Me.openMarketSettings.Name = "openMarketSettings"
        Me.openMarketSettings.Size = New System.Drawing.Size(169, 24)
        Me.openMarketSettings.TabIndex = 4
        Me.openMarketSettings.Text = "Market Settings"
        Me.openMarketSettings.UseVisualStyleBackColor = True
        '
        '_getMarketOrders
        '
        Me._getMarketOrders.Location = New System.Drawing.Point(615, 38)
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
        Me._sellOrders.Columns.Add(Me._sellLocation)
        Me._sellOrders.Columns.Add(Me._sellQuantity)
        Me._sellOrders.Columns.Add(Me._sellPrice)
        Me._sellOrders.Columns.Add(Me._sellExpires)
        Me._sellOrders.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me._sellOrders.Location = New System.Drawing.Point(10, 124)
        Me._sellOrders.Name = "_sellOrders"
        Me._sellOrders.NodesConnector = Me.NodeConnector6
        Me._sellOrders.NodeStyle = Me.ElementStyle5
        Me._sellOrders.PathSeparator = ";"
        Me._sellOrders.Size = New System.Drawing.Size(836, 232)
        Me._sellOrders.Styles.Add(Me.ElementStyle5)
        Me._sellOrders.TabIndex = 2
        Me._sellOrders.Text = "AdvTree1"
        '
        '_sellLocation
        '
        Me._sellLocation.Editable = False
        Me._sellLocation.Name = "_sellLocation"
        Me._sellLocation.StretchToFill = True
        Me._sellLocation.Text = "Location"
        Me._sellLocation.Width.Absolute = 150
        '
        '_sellQuantity
        '
        Me._sellQuantity.Editable = False
        Me._sellQuantity.Name = "_sellQuantity"
        Me._sellQuantity.ShowToolTips = False
        Me._sellQuantity.Text = "Quantity"
        Me._sellQuantity.Width.Absolute = 150
        '
        '_sellPrice
        '
        Me._sellPrice.Editable = False
        Me._sellPrice.Name = "_sellPrice"
        Me._sellPrice.SortDirection = DevComponents.AdvTree.eSortDirection.Ascending
        Me._sellPrice.Text = "Price"
        Me._sellPrice.Width.Absolute = 150
        '
        '_sellExpires
        '
        Me._sellExpires.Editable = False
        Me._sellExpires.Name = "_sellExpires"
        Me._sellExpires.Text = "Expires"
        Me._sellExpires.Width.Absolute = 150
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
        Me.Label1.Location = New System.Drawing.Point(168, 44)
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
        Me._itemsList.Location = New System.Drawing.Point(249, 41)
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
        Me.ctxPrices.ResumeLayout(False)
        CType(Me.TabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        Me.TabControlPanel1.PerformLayout()
        Me.gpMarketPrices.ResumeLayout(False)
        Me.gpMarketPrices.PerformLayout()
        CType(Me._buyOrders, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._sellOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel4.ResumeLayout(False)
        Me.TabControlPanel4.PerformLayout()
        CType(Me.adtPrices, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents mnuDeleteLog As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabControl1 As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiPriceSettings As DevComponents.DotNetBar.TabItem
    Friend WithEvents gpMarketPrices As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiCustomPrices As DevComponents.DotNetBar.TabItem
    Friend WithEvents adtPrices As DevComponents.AdvTree.AdvTree
    Friend WithEvents colCustomItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomBase As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomMarket As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colCustomCustom As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector5 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle4 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents openMarketSettings As System.Windows.Forms.Button
    Friend WithEvents _sellOrders As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector6 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle5 As DevComponents.DotNetBar.ElementStyle
    Private WithEvents _getMarketOrders As System.Windows.Forms.Button
    Private WithEvents _itemsList As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents _buyOrders As DevComponents.AdvTree.AdvTree
    Friend WithEvents _buyLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector7 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle6 As DevComponents.DotNetBar.ElementStyle
    Private WithEvents _buyQuantity As DevComponents.AdvTree.ColumnHeader
    Private WithEvents _buyPrice As DevComponents.AdvTree.ColumnHeader
    Private WithEvents _buyExpires As DevComponents.AdvTree.ColumnHeader
    Private WithEvents _sellLocation As DevComponents.AdvTree.ColumnHeader
    Private WithEvents _sellQuantity As DevComponents.AdvTree.ColumnHeader
    Private WithEvents _sellPrice As DevComponents.AdvTree.ColumnHeader
    Private WithEvents _sellExpires As DevComponents.AdvTree.ColumnHeader
End Class
