<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCMarketOrders
    Inherits Widget

    'UserControl overrides dispose to clean up the component list.
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
        Me.tbcOrders = New DevComponents.DotNetBar.TabControl
        Me.tabSellOrders = New DevComponents.DotNetBar.TabControlPanel
        Me.clvSellOrders = New DevComponents.DotNetBar.Controls.ListViewEx
        Me.colSellType = New System.Windows.Forms.ColumnHeader
        Me.colSellQuantity = New System.Windows.Forms.ColumnHeader
        Me.colSellPrice = New System.Windows.Forms.ColumnHeader
        Me.colSellLocation = New System.Windows.Forms.ColumnHeader
        Me.colSellExpires = New System.Windows.Forms.ColumnHeader
        Me.tbiSellOrders = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tabBuyOrders = New DevComponents.DotNetBar.TabControlPanel
        Me.clvBuyOrders = New DevComponents.DotNetBar.Controls.ListViewEx
        Me.colBuyType = New System.Windows.Forms.ColumnHeader
        Me.colBuyQuantity = New System.Windows.Forms.ColumnHeader
        Me.colBuyPrice = New System.Windows.Forms.ColumnHeader
        Me.colBuyLocation = New System.Windows.Forms.ColumnHeader
        Me.colBuyExpires = New System.Windows.Forms.ColumnHeader
        Me.tbiBuyOrders = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel
        Me.clvRecentlySold = New DevComponents.DotNetBar.Controls.ListViewEx
        Me.colRecentlySoldType = New System.Windows.Forms.ColumnHeader
        Me.colRecentlySoldQuantity = New System.Windows.Forms.ColumnHeader
        Me.colRecentlySoldPrice = New System.Windows.Forms.ColumnHeader
        Me.colRecentlySoldLocation = New System.Windows.Forms.ColumnHeader
        Me.tabRecentlySold = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel
        Me.clvRecentlyBought = New DevComponents.DotNetBar.Controls.ListViewEx
        Me.colRecentlyBoughtType = New System.Windows.Forms.ColumnHeader
        Me.colRecentlyBoughtQuantity = New System.Windows.Forms.ColumnHeader
        Me.colRecentlyBoughtPrice = New System.Windows.Forms.ColumnHeader
        Me.colRecentlyBoughtLocation = New System.Windows.Forms.ColumnHeader
        Me.tabRecentlyBought = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.tabOrdersOverview = New DevComponents.DotNetBar.TabControlPanel
        Me.lblTotalOrdersLbl = New DevComponents.DotNetBar.LabelX
        Me.lblTotalOrders = New DevComponents.DotNetBar.LabelX
        Me.lblRemoteRange = New DevComponents.DotNetBar.LabelX
        Me.lblModRange = New DevComponents.DotNetBar.LabelX
        Me.lblBidRange = New DevComponents.DotNetBar.LabelX
        Me.lblAskRange = New DevComponents.DotNetBar.LabelX
        Me.lblEscrow = New DevComponents.DotNetBar.LabelX
        Me.lblBuyTotal = New DevComponents.DotNetBar.LabelX
        Me.lblSellTotal = New DevComponents.DotNetBar.LabelX
        Me.lblTransTax = New DevComponents.DotNetBar.LabelX
        Me.lblBrokerFee = New DevComponents.DotNetBar.LabelX
        Me.lblOrders = New DevComponents.DotNetBar.LabelX
        Me.lblRemoteRangeLbl = New DevComponents.DotNetBar.LabelX
        Me.lblModRangeLbl = New DevComponents.DotNetBar.LabelX
        Me.lblBidRangeLbl = New DevComponents.DotNetBar.LabelX
        Me.lblAskRangeLbl = New DevComponents.DotNetBar.LabelX
        Me.lblEscrowLbl = New DevComponents.DotNetBar.LabelX
        Me.lblBuyTotalLbl = New DevComponents.DotNetBar.LabelX
        Me.lblSellTotalLbl = New DevComponents.DotNetBar.LabelX
        Me.lblTransTaxLbl = New DevComponents.DotNetBar.LabelX
        Me.lblBrokerFeeLbl = New DevComponents.DotNetBar.LabelX
        Me.lblOrdersLbl = New DevComponents.DotNetBar.LabelX
        Me.cboOwner = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.lblOwner = New DevComponents.DotNetBar.LabelX
        Me.tbiOverview = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.btnRefresh = New DevComponents.DotNetBar.ButtonX
        Me.AGPContent.SuspendLayout()
        CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbcOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbcOrders.SuspendLayout()
        Me.tabSellOrders.SuspendLayout()
        Me.tabBuyOrders.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        Me.TabControlPanel2.SuspendLayout()
        Me.tabOrdersOverview.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblHeader
        '
        '
        '
        '
        Me.lblHeader.BackgroundStyle.Class = ""
        Me.lblHeader.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblHeader.Image = Global.EveHQ.My.Resources.Resources.Orders32
        Me.lblHeader.Size = New System.Drawing.Size(438, 23)
        Me.lblHeader.Text = "Market Orders"
        '
        'AGPContent
        '
        Me.AGPContent.Controls.Add(Me.tbcOrders)
        Me.AGPContent.Size = New System.Drawing.Size(450, 300)
        Me.AGPContent.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.AGPContent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.AGPContent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.AGPContent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.AGPContent.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.AGPContent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.AGPContent.Style.GradientAngle = 90
        Me.AGPContent.Controls.SetChildIndex(Me.lblHeader, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.pbConfig, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.tbcOrders, 0)
        '
        'pbConfig
        '
        Me.pbConfig.Location = New System.Drawing.Point(424, 6)
        '
        'tbcOrders
        '
        Me.tbcOrders.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbcOrders.BackColor = System.Drawing.Color.Transparent
        Me.tbcOrders.CanReorderTabs = True
        Me.tbcOrders.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tbcOrders.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tbcOrders.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tbcOrders.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tbcOrders.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tbcOrders.Controls.Add(Me.tabOrdersOverview)
        Me.tbcOrders.Controls.Add(Me.tabSellOrders)
        Me.tbcOrders.Controls.Add(Me.tabBuyOrders)
        Me.tbcOrders.Controls.Add(Me.TabControlPanel1)
        Me.tbcOrders.Controls.Add(Me.TabControlPanel2)
        Me.tbcOrders.Location = New System.Drawing.Point(6, 39)
        Me.tbcOrders.Name = "tbcOrders"
        Me.tbcOrders.SelectedTabFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tbcOrders.SelectedTabIndex = 0
        Me.tbcOrders.Size = New System.Drawing.Size(438, 256)
        Me.tbcOrders.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tbcOrders.TabIndex = 15
        Me.tbcOrders.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tbcOrders.Tabs.Add(Me.tbiOverview)
        Me.tbcOrders.Tabs.Add(Me.tbiSellOrders)
        Me.tbcOrders.Tabs.Add(Me.tbiBuyOrders)
        Me.tbcOrders.Tabs.Add(Me.tabRecentlySold)
        Me.tbcOrders.Tabs.Add(Me.tabRecentlyBought)
        Me.tbcOrders.Text = "TabControl1"
        '
        'tabSellOrders
        '
        Me.tabSellOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.tabSellOrders.Controls.Add(Me.clvSellOrders)
        Me.tabSellOrders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabSellOrders.Location = New System.Drawing.Point(0, 23)
        Me.tabSellOrders.Name = "tabSellOrders"
        Me.tabSellOrders.Padding = New System.Windows.Forms.Padding(1)
        Me.tabSellOrders.Size = New System.Drawing.Size(438, 233)
        Me.tabSellOrders.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tabSellOrders.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tabSellOrders.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tabSellOrders.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tabSellOrders.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                    Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tabSellOrders.Style.GradientAngle = 90
        Me.tabSellOrders.TabIndex = 2
        Me.tabSellOrders.TabItem = Me.tbiSellOrders
        '
        'clvSellOrders
        '
        Me.clvSellOrders.BackColor = System.Drawing.SystemColors.Control
        '
        '
        '
        Me.clvSellOrders.Border.Class = "ListViewBorder"
        Me.clvSellOrders.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.clvSellOrders.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSellType, Me.colSellQuantity, Me.colSellPrice, Me.colSellLocation, Me.colSellExpires})
        Me.clvSellOrders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvSellOrders.Location = New System.Drawing.Point(1, 1)
        Me.clvSellOrders.MultiSelect = False
        Me.clvSellOrders.Name = "clvSellOrders"
        Me.clvSellOrders.Size = New System.Drawing.Size(436, 231)
        Me.clvSellOrders.TabIndex = 0
        Me.clvSellOrders.UseCompatibleStateImageBehavior = False
        Me.clvSellOrders.View = System.Windows.Forms.View.Details
        '
        'colSellType
        '
        Me.colSellType.Text = "Type"
        Me.colSellType.Width = 125
        '
        'colSellQuantity
        '
        Me.colSellQuantity.Text = "Quantity"
        Me.colSellQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colSellQuantity.Width = 75
        '
        'colSellPrice
        '
        Me.colSellPrice.Text = "Price"
        Me.colSellPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'colSellLocation
        '
        Me.colSellLocation.Text = "Location"
        Me.colSellLocation.Width = 100
        '
        'colSellExpires
        '
        Me.colSellExpires.Text = "Expires In"
        '
        'tbiSellOrders
        '
        Me.tbiSellOrders.AttachedControl = Me.tabSellOrders
        Me.tbiSellOrders.Name = "tbiSellOrders"
        Me.tbiSellOrders.Text = "Selling"
        '
        'tabBuyOrders
        '
        Me.tabBuyOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.tabBuyOrders.Controls.Add(Me.clvBuyOrders)
        Me.tabBuyOrders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabBuyOrders.Location = New System.Drawing.Point(0, 23)
        Me.tabBuyOrders.Name = "tabBuyOrders"
        Me.tabBuyOrders.Padding = New System.Windows.Forms.Padding(1)
        Me.tabBuyOrders.Size = New System.Drawing.Size(438, 233)
        Me.tabBuyOrders.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tabBuyOrders.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tabBuyOrders.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tabBuyOrders.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tabBuyOrders.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                    Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tabBuyOrders.Style.GradientAngle = 90
        Me.tabBuyOrders.TabIndex = 3
        Me.tabBuyOrders.TabItem = Me.tbiBuyOrders
        '
        'clvBuyOrders
        '
        Me.clvBuyOrders.BackColor = System.Drawing.SystemColors.Control
        '
        '
        '
        Me.clvBuyOrders.Border.Class = "ListViewBorder"
        Me.clvBuyOrders.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.clvBuyOrders.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colBuyType, Me.colBuyQuantity, Me.colBuyPrice, Me.colBuyLocation, Me.colBuyExpires})
        Me.clvBuyOrders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvBuyOrders.Location = New System.Drawing.Point(1, 1)
        Me.clvBuyOrders.MultiSelect = False
        Me.clvBuyOrders.Name = "clvBuyOrders"
        Me.clvBuyOrders.Size = New System.Drawing.Size(436, 231)
        Me.clvBuyOrders.TabIndex = 1
        Me.clvBuyOrders.UseCompatibleStateImageBehavior = False
        Me.clvBuyOrders.View = System.Windows.Forms.View.Details
        '
        'colBuyType
        '
        Me.colBuyType.Text = "Type"
        Me.colBuyType.Width = 125
        '
        'colBuyQuantity
        '
        Me.colBuyQuantity.Text = "Quantity"
        Me.colBuyQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colBuyQuantity.Width = 75
        '
        'colBuyPrice
        '
        Me.colBuyPrice.Text = "Price"
        Me.colBuyPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'colBuyLocation
        '
        Me.colBuyLocation.Text = "Location"
        Me.colBuyLocation.Width = 100
        '
        'colBuyExpires
        '
        Me.colBuyExpires.Text = "Expires In"
        '
        'tbiBuyOrders
        '
        Me.tbiBuyOrders.AttachedControl = Me.tabBuyOrders
        Me.tbiBuyOrders.Name = "tbiBuyOrders"
        Me.tbiBuyOrders.Text = "Buying"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.clvRecentlySold)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(438, 233)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                    Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 4
        Me.TabControlPanel1.TabItem = Me.tabRecentlySold
        '
        'clvRecentlySold
        '
        Me.clvRecentlySold.BackColor = System.Drawing.SystemColors.Control
        '
        '
        '
        Me.clvRecentlySold.Border.Class = "ListViewBorder"
        Me.clvRecentlySold.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.clvRecentlySold.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colRecentlySoldType, Me.colRecentlySoldQuantity, Me.colRecentlySoldPrice, Me.colRecentlySoldLocation})
        Me.clvRecentlySold.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvRecentlySold.Location = New System.Drawing.Point(1, 1)
        Me.clvRecentlySold.MultiSelect = False
        Me.clvRecentlySold.Name = "clvRecentlySold"
        Me.clvRecentlySold.Size = New System.Drawing.Size(436, 231)
        Me.clvRecentlySold.TabIndex = 2
        Me.clvRecentlySold.UseCompatibleStateImageBehavior = False
        Me.clvRecentlySold.View = System.Windows.Forms.View.Details
        '
        'colRecentlySoldType
        '
        Me.colRecentlySoldType.Text = "Type"
        Me.colRecentlySoldType.Width = 125
        '
        'colRecentlySoldQuantity
        '
        Me.colRecentlySoldQuantity.Text = "Quantity"
        Me.colRecentlySoldQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colRecentlySoldQuantity.Width = 75
        '
        'colRecentlySoldPrice
        '
        Me.colRecentlySoldPrice.Text = "Price"
        Me.colRecentlySoldPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'colRecentlySoldLocation
        '
        Me.colRecentlySoldLocation.Text = "Location"
        Me.colRecentlySoldLocation.Width = 100
        '
        'tabRecentlySold
        '
        Me.tabRecentlySold.AttachedControl = Me.TabControlPanel1
        Me.tabRecentlySold.Name = "tabRecentlySold"
        Me.tabRecentlySold.Text = "Recently sold"
        '
        'TabControlPanel2
        '
        Me.TabControlPanel2.Controls.Add(Me.clvRecentlyBought)
        Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel2.Name = "TabControlPanel2"
        Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel2.Size = New System.Drawing.Size(438, 233)
        Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                    Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel2.Style.GradientAngle = 90
        Me.TabControlPanel2.TabIndex = 5
        Me.TabControlPanel2.TabItem = Me.tabRecentlyBought
        '
        'clvRecentlyBought
        '
        Me.clvRecentlyBought.BackColor = System.Drawing.SystemColors.Control
        '
        '
        '
        Me.clvRecentlyBought.Border.Class = "ListViewBorder"
        Me.clvRecentlyBought.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.clvRecentlyBought.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colRecentlyBoughtType, Me.colRecentlyBoughtQuantity, Me.colRecentlyBoughtPrice, Me.colRecentlyBoughtLocation})
        Me.clvRecentlyBought.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvRecentlyBought.Location = New System.Drawing.Point(1, 1)
        Me.clvRecentlyBought.MultiSelect = False
        Me.clvRecentlyBought.Name = "clvRecentlyBought"
        Me.clvRecentlyBought.Size = New System.Drawing.Size(436, 231)
        Me.clvRecentlyBought.TabIndex = 2
        Me.clvRecentlyBought.UseCompatibleStateImageBehavior = False
        Me.clvRecentlyBought.View = System.Windows.Forms.View.Details
        '
        'colRecentlyBoughtType
        '
        Me.colRecentlyBoughtType.Text = "Type"
        Me.colRecentlyBoughtType.Width = 125
        '
        'colRecentlyBoughtQuantity
        '
        Me.colRecentlyBoughtQuantity.Text = "Quantity"
        Me.colRecentlyBoughtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colRecentlyBoughtQuantity.Width = 75
        '
        'colRecentlyBoughtPrice
        '
        Me.colRecentlyBoughtPrice.Text = "Price"
        Me.colRecentlyBoughtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'colRecentlyBoughtLocation
        '
        Me.colRecentlyBoughtLocation.Text = "Location"
        Me.colRecentlyBoughtLocation.Width = 100
        '
        'tabRecentlyBought
        '
        Me.tabRecentlyBought.AttachedControl = Me.TabControlPanel2
        Me.tabRecentlyBought.Name = "tabRecentlyBought"
        Me.tabRecentlyBought.Text = "Recently bought"
        '
        'tabOrdersOverview
        '
        Me.tabOrdersOverview.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.tabOrdersOverview.Controls.Add(Me.btnRefresh)
        Me.tabOrdersOverview.Controls.Add(Me.lblTotalOrdersLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblTotalOrders)
        Me.tabOrdersOverview.Controls.Add(Me.lblRemoteRange)
        Me.tabOrdersOverview.Controls.Add(Me.lblModRange)
        Me.tabOrdersOverview.Controls.Add(Me.lblBidRange)
        Me.tabOrdersOverview.Controls.Add(Me.lblAskRange)
        Me.tabOrdersOverview.Controls.Add(Me.lblEscrow)
        Me.tabOrdersOverview.Controls.Add(Me.lblBuyTotal)
        Me.tabOrdersOverview.Controls.Add(Me.lblSellTotal)
        Me.tabOrdersOverview.Controls.Add(Me.lblTransTax)
        Me.tabOrdersOverview.Controls.Add(Me.lblBrokerFee)
        Me.tabOrdersOverview.Controls.Add(Me.lblOrders)
        Me.tabOrdersOverview.Controls.Add(Me.lblRemoteRangeLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblModRangeLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblBidRangeLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblAskRangeLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblEscrowLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblBuyTotalLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblSellTotalLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblTransTaxLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblBrokerFeeLbl)
        Me.tabOrdersOverview.Controls.Add(Me.lblOrdersLbl)
        Me.tabOrdersOverview.Controls.Add(Me.cboOwner)
        Me.tabOrdersOverview.Controls.Add(Me.lblOwner)
        Me.tabOrdersOverview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabOrdersOverview.Location = New System.Drawing.Point(0, 23)
        Me.tabOrdersOverview.Name = "tabOrdersOverview"
        Me.tabOrdersOverview.Padding = New System.Windows.Forms.Padding(1)
        Me.tabOrdersOverview.Size = New System.Drawing.Size(438, 233)
        Me.tabOrdersOverview.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.tabOrdersOverview.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.tabOrdersOverview.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.tabOrdersOverview.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.tabOrdersOverview.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                    Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.tabOrdersOverview.Style.GradientAngle = 90
        Me.tabOrdersOverview.TabIndex = 1
        Me.tabOrdersOverview.TabItem = Me.tbiOverview
        '
        'lblTotalOrdersLbl
        '
        Me.lblTotalOrdersLbl.AutoSize = True
        Me.lblTotalOrdersLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblTotalOrdersLbl.BackgroundStyle.Class = ""
        Me.lblTotalOrdersLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblTotalOrdersLbl.Location = New System.Drawing.Point(15, 41)
        Me.lblTotalOrdersLbl.Name = "lblTotalOrdersLbl"
        Me.lblTotalOrdersLbl.Size = New System.Drawing.Size(89, 16)
        Me.lblTotalOrdersLbl.TabIndex = 23
        Me.lblTotalOrdersLbl.Text = "Orders permitted:"
        '
        'lblTotalOrders
        '
        Me.lblTotalOrders.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblTotalOrders.BackgroundStyle.Class = ""
        Me.lblTotalOrders.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblTotalOrders.Location = New System.Drawing.Point(122, 41)
        Me.lblTotalOrders.Name = "lblTotalOrders"
        Me.lblTotalOrders.Size = New System.Drawing.Size(67, 15)
        Me.lblTotalOrders.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblTotalOrders.TabIndex = 22
        Me.lblTotalOrders.Text = "placeholder"
        '
        'lblRemoteRange
        '
        Me.lblRemoteRange.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblRemoteRange.BackgroundStyle.Class = ""
        Me.lblRemoteRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblRemoteRange.Location = New System.Drawing.Point(300, 104)
        Me.lblRemoteRange.Name = "lblRemoteRange"
        Me.lblRemoteRange.Size = New System.Drawing.Size(90, 15)
        Me.lblRemoteRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblRemoteRange.TabIndex = 21
        Me.lblRemoteRange.Text = "placeholder"
        '
        'lblModRange
        '
        Me.lblModRange.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblModRange.BackgroundStyle.Class = ""
        Me.lblModRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblModRange.Location = New System.Drawing.Point(300, 83)
        Me.lblModRange.Name = "lblModRange"
        Me.lblModRange.Size = New System.Drawing.Size(90, 15)
        Me.lblModRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblModRange.TabIndex = 20
        Me.lblModRange.Text = "placeholder"
        '
        'lblBidRange
        '
        Me.lblBidRange.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblBidRange.BackgroundStyle.Class = ""
        Me.lblBidRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblBidRange.Location = New System.Drawing.Point(300, 62)
        Me.lblBidRange.Name = "lblBidRange"
        Me.lblBidRange.Size = New System.Drawing.Size(90, 15)
        Me.lblBidRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblBidRange.TabIndex = 19
        Me.lblBidRange.Text = "placeholder"
        '
        'lblAskRange
        '
        Me.lblAskRange.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblAskRange.BackgroundStyle.Class = ""
        Me.lblAskRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblAskRange.Location = New System.Drawing.Point(300, 41)
        Me.lblAskRange.Name = "lblAskRange"
        Me.lblAskRange.Size = New System.Drawing.Size(90, 15)
        Me.lblAskRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblAskRange.TabIndex = 18
        Me.lblAskRange.Text = "placeholder"
        '
        'lblEscrow
        '
        Me.lblEscrow.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblEscrow.BackgroundStyle.Class = ""
        Me.lblEscrow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblEscrow.Location = New System.Drawing.Point(122, 167)
        Me.lblEscrow.Name = "lblEscrow"
        Me.lblEscrow.Size = New System.Drawing.Size(205, 15)
        Me.lblEscrow.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblEscrow.TabIndex = 17
        Me.lblEscrow.Text = "placeholder"
        '
        'lblBuyTotal
        '
        Me.lblBuyTotal.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblBuyTotal.BackgroundStyle.Class = ""
        Me.lblBuyTotal.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblBuyTotal.Location = New System.Drawing.Point(122, 146)
        Me.lblBuyTotal.Name = "lblBuyTotal"
        Me.lblBuyTotal.Size = New System.Drawing.Size(205, 15)
        Me.lblBuyTotal.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblBuyTotal.TabIndex = 16
        Me.lblBuyTotal.Text = "placeholder"
        '
        'lblSellTotal
        '
        Me.lblSellTotal.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblSellTotal.BackgroundStyle.Class = ""
        Me.lblSellTotal.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblSellTotal.Location = New System.Drawing.Point(122, 125)
        Me.lblSellTotal.Name = "lblSellTotal"
        Me.lblSellTotal.Size = New System.Drawing.Size(205, 15)
        Me.lblSellTotal.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblSellTotal.TabIndex = 15
        Me.lblSellTotal.Text = "placeholder"
        '
        'lblTransTax
        '
        Me.lblTransTax.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblTransTax.BackgroundStyle.Class = ""
        Me.lblTransTax.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblTransTax.Location = New System.Drawing.Point(122, 104)
        Me.lblTransTax.Name = "lblTransTax"
        Me.lblTransTax.Size = New System.Drawing.Size(67, 15)
        Me.lblTransTax.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblTransTax.TabIndex = 14
        Me.lblTransTax.Text = "placeholder"
        '
        'lblBrokerFee
        '
        Me.lblBrokerFee.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblBrokerFee.BackgroundStyle.Class = ""
        Me.lblBrokerFee.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblBrokerFee.Location = New System.Drawing.Point(122, 83)
        Me.lblBrokerFee.Name = "lblBrokerFee"
        Me.lblBrokerFee.Size = New System.Drawing.Size(67, 15)
        Me.lblBrokerFee.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblBrokerFee.TabIndex = 13
        Me.lblBrokerFee.Text = "placeholder"
        '
        'lblOrders
        '
        Me.lblOrders.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblOrders.BackgroundStyle.Class = ""
        Me.lblOrders.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblOrders.Location = New System.Drawing.Point(122, 62)
        Me.lblOrders.Name = "lblOrders"
        Me.lblOrders.Size = New System.Drawing.Size(67, 15)
        Me.lblOrders.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblOrders.TabIndex = 12
        Me.lblOrders.Text = "placeholder"
        '
        'lblRemoteRangeLbl
        '
        Me.lblRemoteRangeLbl.AutoSize = True
        Me.lblRemoteRangeLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblRemoteRangeLbl.BackgroundStyle.Class = ""
        Me.lblRemoteRangeLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblRemoteRangeLbl.Location = New System.Drawing.Point(195, 104)
        Me.lblRemoteRangeLbl.Name = "lblRemoteRangeLbl"
        Me.lblRemoteRangeLbl.Size = New System.Drawing.Size(93, 16)
        Me.lblRemoteRangeLbl.TabIndex = 11
        Me.lblRemoteRangeLbl.Text = "Remote bid range:"
        '
        'lblModRangeLbl
        '
        Me.lblModRangeLbl.AutoSize = True
        Me.lblModRangeLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblModRangeLbl.BackgroundStyle.Class = ""
        Me.lblModRangeLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblModRangeLbl.Location = New System.Drawing.Point(195, 83)
        Me.lblModRangeLbl.Name = "lblModRangeLbl"
        Me.lblModRangeLbl.Size = New System.Drawing.Size(96, 16)
        Me.lblModRangeLbl.TabIndex = 10
        Me.lblModRangeLbl.Text = "Modification range:"
        '
        'lblBidRangeLbl
        '
        Me.lblBidRangeLbl.AutoSize = True
        Me.lblBidRangeLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblBidRangeLbl.BackgroundStyle.Class = ""
        Me.lblBidRangeLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblBidRangeLbl.Location = New System.Drawing.Point(195, 62)
        Me.lblBidRangeLbl.Name = "lblBidRangeLbl"
        Me.lblBidRangeLbl.Size = New System.Drawing.Size(53, 16)
        Me.lblBidRangeLbl.TabIndex = 9
        Me.lblBidRangeLbl.Text = "Bid range:"
        '
        'lblAskRangeLbl
        '
        Me.lblAskRangeLbl.AutoSize = True
        Me.lblAskRangeLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblAskRangeLbl.BackgroundStyle.Class = ""
        Me.lblAskRangeLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblAskRangeLbl.Location = New System.Drawing.Point(195, 41)
        Me.lblAskRangeLbl.Name = "lblAskRangeLbl"
        Me.lblAskRangeLbl.Size = New System.Drawing.Size(55, 16)
        Me.lblAskRangeLbl.TabIndex = 8
        Me.lblAskRangeLbl.Text = "Ask range:"
        '
        'lblEscrowLbl
        '
        Me.lblEscrowLbl.AutoSize = True
        Me.lblEscrowLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblEscrowLbl.BackgroundStyle.Class = ""
        Me.lblEscrowLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblEscrowLbl.Location = New System.Drawing.Point(15, 167)
        Me.lblEscrowLbl.Name = "lblEscrowLbl"
        Me.lblEscrowLbl.Size = New System.Drawing.Size(80, 16)
        Me.lblEscrowLbl.TabIndex = 7
        Me.lblEscrowLbl.Text = "Total in escrow:"
        '
        'lblBuyTotalLbl
        '
        Me.lblBuyTotalLbl.AutoSize = True
        Me.lblBuyTotalLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblBuyTotalLbl.BackgroundStyle.Class = ""
        Me.lblBuyTotalLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblBuyTotalLbl.Location = New System.Drawing.Point(15, 146)
        Me.lblBuyTotalLbl.Name = "lblBuyTotalLbl"
        Me.lblBuyTotalLbl.Size = New System.Drawing.Size(84, 16)
        Me.lblBuyTotalLbl.TabIndex = 6
        Me.lblBuyTotalLbl.Text = "Buy orders total:"
        '
        'lblSellTotalLbl
        '
        Me.lblSellTotalLbl.AutoSize = True
        Me.lblSellTotalLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblSellTotalLbl.BackgroundStyle.Class = ""
        Me.lblSellTotalLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblSellTotalLbl.Location = New System.Drawing.Point(15, 125)
        Me.lblSellTotalLbl.Name = "lblSellTotalLbl"
        Me.lblSellTotalLbl.Size = New System.Drawing.Size(83, 16)
        Me.lblSellTotalLbl.TabIndex = 5
        Me.lblSellTotalLbl.Text = "Sell orders total:"
        '
        'lblTransTaxLbl
        '
        Me.lblTransTaxLbl.AutoSize = True
        Me.lblTransTaxLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblTransTaxLbl.BackgroundStyle.Class = ""
        Me.lblTransTaxLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblTransTaxLbl.Location = New System.Drawing.Point(15, 104)
        Me.lblTransTaxLbl.Name = "lblTransTaxLbl"
        Me.lblTransTaxLbl.Size = New System.Drawing.Size(81, 16)
        Me.lblTransTaxLbl.TabIndex = 4
        Me.lblTransTaxLbl.Text = "Transaction tax:"
        '
        'lblBrokerFeeLbl
        '
        Me.lblBrokerFeeLbl.AutoSize = True
        Me.lblBrokerFeeLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblBrokerFeeLbl.BackgroundStyle.Class = ""
        Me.lblBrokerFeeLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblBrokerFeeLbl.Location = New System.Drawing.Point(15, 83)
        Me.lblBrokerFeeLbl.Name = "lblBrokerFeeLbl"
        Me.lblBrokerFeeLbl.Size = New System.Drawing.Size(83, 16)
        Me.lblBrokerFeeLbl.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblBrokerFeeLbl.TabIndex = 3
        Me.lblBrokerFeeLbl.Text = "Base broker fee:"
        '
        'lblOrdersLbl
        '
        Me.lblOrdersLbl.AutoSize = True
        Me.lblOrdersLbl.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblOrdersLbl.BackgroundStyle.Class = ""
        Me.lblOrdersLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblOrdersLbl.Location = New System.Drawing.Point(15, 62)
        Me.lblOrdersLbl.Name = "lblOrdersLbl"
        Me.lblOrdersLbl.Size = New System.Drawing.Size(90, 16)
        Me.lblOrdersLbl.TabIndex = 2
        Me.lblOrdersLbl.Text = "Orders remaining:"
        '
        'cboOwner
        '
        Me.cboOwner.DisplayMember = "Text"
        Me.cboOwner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOwner.FormattingEnabled = True
        Me.cboOwner.ItemHeight = 15
        Me.cboOwner.Location = New System.Drawing.Point(59, 15)
        Me.cboOwner.Name = "cboOwner"
        Me.cboOwner.Size = New System.Drawing.Size(130, 21)
        Me.cboOwner.Sorted = True
        Me.cboOwner.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboOwner.TabIndex = 1
        '
        'lblOwner
        '
        Me.lblOwner.AutoSize = True
        Me.lblOwner.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblOwner.BackgroundStyle.Class = ""
        Me.lblOwner.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblOwner.Location = New System.Drawing.Point(15, 15)
        Me.lblOwner.Name = "lblOwner"
        Me.lblOwner.Size = New System.Drawing.Size(38, 16)
        Me.lblOwner.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.lblOwner.TabIndex = 0
        Me.lblOwner.Text = "Owner:"
        '
        'tbiOverview
        '
        Me.tbiOverview.AttachedControl = Me.tabOrdersOverview
        Me.tbiOverview.Name = "tbiOverview"
        Me.tbiOverview.Text = "Overview"
        '
        'btnRefresh
        '
        Me.btnRefresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnRefresh.CallBasePaintBackground = True
        Me.btnRefresh.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnRefresh.Location = New System.Drawing.Point(195, 15)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(66, 21)
        Me.btnRefresh.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnRefresh.TabIndex = 24
        Me.btnRefresh.Text = "Refresh"
        '
        'DBCMarketOrders
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.MinimumSize = New System.Drawing.Size(300, 200)
        Me.Name = "DBCMarketOrders"
        Me.Size = New System.Drawing.Size(450, 300)
        Me.AGPContent.ResumeLayout(False)
        CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbcOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbcOrders.ResumeLayout(False)
        Me.tabSellOrders.ResumeLayout(False)
        Me.tabBuyOrders.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        Me.TabControlPanel2.ResumeLayout(False)
        Me.tabOrdersOverview.ResumeLayout(False)
        Me.tabOrdersOverview.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tbcOrders As DevComponents.DotNetBar.TabControl
    Friend WithEvents tabOrdersOverview As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents lblRemoteRange As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblModRange As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblBidRange As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblAskRange As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblEscrow As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblBuyTotal As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblSellTotal As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblTransTax As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblBrokerFee As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblOrders As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblRemoteRangeLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblModRangeLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblBidRangeLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblAskRangeLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblEscrowLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblBuyTotalLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblSellTotalLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblTransTaxLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblBrokerFeeLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblOrdersLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents cboOwner As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblOwner As DevComponents.DotNetBar.LabelX
    Friend WithEvents tbiOverview As DevComponents.DotNetBar.TabItem
    Friend WithEvents tabSellOrders As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents clvSellOrders As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colSellType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSellQuantity As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSellPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSellLocation As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSellExpires As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbiSellOrders As DevComponents.DotNetBar.TabItem
    Friend WithEvents tabBuyOrders As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents clvBuyOrders As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colBuyType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBuyQuantity As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBuyPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBuyLocation As System.Windows.Forms.ColumnHeader
    Friend WithEvents colBuyExpires As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbiBuyOrders As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tabRecentlySold As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tabRecentlyBought As DevComponents.DotNetBar.TabItem
    Friend WithEvents clvRecentlySold As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colRecentlySoldType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecentlySoldQuantity As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecentlySoldPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecentlySoldLocation As System.Windows.Forms.ColumnHeader
    Friend WithEvents clvRecentlyBought As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colRecentlyBoughtType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecentlyBoughtQuantity As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecentlyBoughtPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecentlyBoughtLocation As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblTotalOrdersLbl As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblTotalOrders As DevComponents.DotNetBar.LabelX
    Friend WithEvents btnRefresh As DevComponents.DotNetBar.ButtonX

End Class
