<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrismResources
    Inherits System.Windows.Forms.UserControl

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PrismResources))
        Me.pnlResources = New DevComponents.DotNetBar.PanelEx()
        Me.btnAlterResourcePrices = New DevComponents.DotNetBar.ButtonX()
        Me.chkUseStandardCosting = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.chkShowSkills = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.adtResources = New DevComponents.AdvTree.AdvTree()
        Me.colMaterial = New DevComponents.AdvTree.ColumnHeader()
        Me.colBP = New DevComponents.AdvTree.ColumnHeader()
        Me.colPerfectUnits = New DevComponents.AdvTree.ColumnHeader()
        Me.colWasteUnits = New DevComponents.AdvTree.ColumnHeader()
        Me.colTotalUnits = New DevComponents.AdvTree.ColumnHeader()
        Me.colUnitPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colTotalPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colIdealML = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle1 = New DevComponents.DotNetBar.ElementStyle()
        Me.tcResources = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tiProductionResources = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtInventionResources = New DevComponents.AdvTree.AdvTree()
        Me.colIRItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colIRQuantity = New DevComponents.AdvTree.ColumnHeader()
        Me.colIRPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colIRValue = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector2 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle2 = New DevComponents.DotNetBar.ElementStyle()
        Me.tiInvention = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtOwnedResources = New DevComponents.AdvTree.AdvTree()
        Me.colROMaterial = New DevComponents.AdvTree.ColumnHeader()
        Me.colORQuantityRequired = New DevComponents.AdvTree.ColumnHeader()
        Me.colORQuantityOwned = New DevComponents.AdvTree.ColumnHeader()
        Me.colORSurplus = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector3 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle3 = New DevComponents.DotNetBar.ElementStyle()
        Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
        Me.chkAdvancedResourceAllocation = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.cboAssetSelection = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
        Me.btnExport = New DevComponents.DotNetBar.ButtonX()
        Me.btnExportToCSV = New DevComponents.DotNetBar.ButtonItem()
        Me.btnExportToTSV = New DevComponents.DotNetBar.ButtonItem()
        Me.btnAddShortfallToReq = New DevComponents.DotNetBar.ButtonItem()
        Me.btnAddAllToReq = New DevComponents.DotNetBar.ButtonItem()
        Me.lblMaxUnits = New System.Windows.Forms.Label()
        Me.lblAssetSelection = New System.Windows.Forms.Label()
        Me.tiResourcesOwned = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
        Me.adtBatchResources = New DevComponents.AdvTree.AdvTree()
        Me.colBatchItem = New DevComponents.AdvTree.ColumnHeader()
        Me.colBatchQuantity = New DevComponents.AdvTree.ColumnHeader()
        Me.colBatchPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colBatchValue = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector4 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle4 = New DevComponents.DotNetBar.ElementStyle()
        Me.pnlBatch = New DevComponents.DotNetBar.PanelEx()
        Me.btnUpdateBatchPrices = New DevComponents.DotNetBar.ButtonX()
        Me.lblBatchName = New System.Windows.Forms.Label()
        Me.tiBatchResources = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.STT = New DevComponents.DotNetBar.SuperTooltip()
        Me.colBatchVolume = New DevComponents.AdvTree.ColumnHeader()
        Me.lblBatchTotals = New System.Windows.Forms.Label()
        Me.pnlResources.SuspendLayout()
        CType(Me.adtResources, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tcResources, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcResources.SuspendLayout()
        Me.TabControlPanel2.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        CType(Me.adtInventionResources, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel3.SuspendLayout()
        CType(Me.adtOwnedResources, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelEx1.SuspendLayout()
        Me.TabControlPanel4.SuspendLayout()
        CType(Me.adtBatchResources, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBatch.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlResources
        '
        Me.pnlResources.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlResources.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlResources.Controls.Add(Me.btnAlterResourcePrices)
        Me.pnlResources.Controls.Add(Me.chkUseStandardCosting)
        Me.pnlResources.Controls.Add(Me.chkShowSkills)
        Me.pnlResources.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlResources.Location = New System.Drawing.Point(1, 1)
        Me.pnlResources.Name = "pnlResources"
        Me.pnlResources.Size = New System.Drawing.Size(866, 24)
        Me.pnlResources.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlResources.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlResources.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlResources.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlResources.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlResources.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlResources.Style.GradientAngle = 90
        Me.STT.SetSuperTooltip(Me.pnlResources, New DevComponents.DotNetBar.SuperTooltipInfo("", "Update Prices", "Provides a single form to update all prices of items listed in the Production Res" & _
            "ources panel.", Nothing, Global.EveHQ.Prism.My.Resources.Resources.Question32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.pnlResources.TabIndex = 0
        '
        'btnAlterResourcePrices
        '
        Me.btnAlterResourcePrices.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAlterResourcePrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAlterResourcePrices.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAlterResourcePrices.Location = New System.Drawing.Point(763, 2)
        Me.btnAlterResourcePrices.Name = "btnAlterResourcePrices"
        Me.btnAlterResourcePrices.Size = New System.Drawing.Size(100, 20)
        Me.btnAlterResourcePrices.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAlterResourcePrices.TabIndex = 2
        Me.btnAlterResourcePrices.Text = "Update Prices"
        '
        'chkUseStandardCosting
        '
        Me.chkUseStandardCosting.AutoSize = True
        '
        '
        '
        Me.chkUseStandardCosting.BackgroundStyle.Class = ""
        Me.chkUseStandardCosting.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkUseStandardCosting.Location = New System.Drawing.Point(150, 4)
        Me.chkUseStandardCosting.Name = "chkUseStandardCosting"
        Me.chkUseStandardCosting.Size = New System.Drawing.Size(167, 16)
        Me.chkUseStandardCosting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.STT.SetSuperTooltip(Me.chkUseStandardCosting, New DevComponents.DotNetBar.SuperTooltipInfo("", "Build All Resources If Possible", resources.GetString("chkUseStandardCosting.SuperTooltip"), Global.EveHQ.Prism.My.Resources.Resources.Question32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.chkUseStandardCosting.TabIndex = 1
        Me.chkUseStandardCosting.Text = "Build All Resources If Possible"
        '
        'chkShowSkills
        '
        Me.chkShowSkills.AutoSize = True
        '
        '
        '
        Me.chkShowSkills.BackgroundStyle.Class = ""
        Me.chkShowSkills.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkShowSkills.Location = New System.Drawing.Point(4, 4)
        Me.chkShowSkills.Name = "chkShowSkills"
        Me.chkShowSkills.Size = New System.Drawing.Size(123, 16)
        Me.chkShowSkills.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.STT.SetSuperTooltip(Me.chkShowSkills, New DevComponents.DotNetBar.SuperTooltipInfo("", "Show Required Skills", "Shows the skills required to produce the required object in the resource list." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Skill costs are not counted as part of the production cost.", Global.EveHQ.Prism.My.Resources.Resources.Question32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.chkShowSkills.TabIndex = 0
        Me.chkShowSkills.Text = "Show Required Skills"
        '
        'adtResources
        '
        Me.adtResources.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtResources.AllowDrop = True
        Me.adtResources.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtResources.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtResources.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtResources.Columns.Add(Me.colMaterial)
        Me.adtResources.Columns.Add(Me.colBP)
        Me.adtResources.Columns.Add(Me.colPerfectUnits)
        Me.adtResources.Columns.Add(Me.colWasteUnits)
        Me.adtResources.Columns.Add(Me.colTotalUnits)
        Me.adtResources.Columns.Add(Me.colUnitPrice)
        Me.adtResources.Columns.Add(Me.colTotalPrice)
        Me.adtResources.Columns.Add(Me.colIdealML)
        Me.adtResources.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtResources.DragDropEnabled = False
        Me.adtResources.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtResources.Location = New System.Drawing.Point(1, 25)
        Me.adtResources.Name = "adtResources"
        Me.adtResources.NodesConnector = Me.NodeConnector1
        Me.adtResources.NodeStyle = Me.ElementStyle1
        Me.adtResources.PathSeparator = ";"
        Me.adtResources.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.FullRowSelect
        Me.adtResources.Size = New System.Drawing.Size(866, 308)
        Me.adtResources.Styles.Add(Me.ElementStyle1)
        Me.adtResources.TabIndex = 0
        Me.adtResources.Text = "AdvTree1"
        '
        'colMaterial
        '
        Me.colMaterial.DisplayIndex = 1
        Me.colMaterial.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colMaterial.Name = "colMaterial"
        Me.colMaterial.Text = "Required Material"
        Me.colMaterial.Width.Absolute = 250
        '
        'colBP
        '
        Me.colBP.DisplayIndex = 2
        Me.colBP.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colBP.Name = "colBP"
        Me.colBP.Text = "BP ME Lvl"
        Me.colBP.Width.Absolute = 90
        '
        'colPerfectUnits
        '
        Me.colPerfectUnits.DisplayIndex = 3
        Me.colPerfectUnits.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colPerfectUnits.Name = "colPerfectUnits"
        Me.colPerfectUnits.Text = "Perfect Qty"
        Me.colPerfectUnits.Width.Absolute = 80
        '
        'colWasteUnits
        '
        Me.colWasteUnits.DisplayIndex = 4
        Me.colWasteUnits.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colWasteUnits.Name = "colWasteUnits"
        Me.colWasteUnits.Text = "Waste Qty"
        Me.colWasteUnits.Width.Absolute = 80
        '
        'colTotalUnits
        '
        Me.colTotalUnits.DisplayIndex = 5
        Me.colTotalUnits.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colTotalUnits.Name = "colTotalUnits"
        Me.colTotalUnits.Text = "Total Qty"
        Me.colTotalUnits.Width.Absolute = 80
        '
        'colUnitPrice
        '
        Me.colUnitPrice.DisplayIndex = 6
        Me.colUnitPrice.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colUnitPrice.Name = "colUnitPrice"
        Me.colUnitPrice.Text = "Unit Price"
        Me.colUnitPrice.Width.Absolute = 80
        '
        'colTotalPrice
        '
        Me.colTotalPrice.DisplayIndex = 7
        Me.colTotalPrice.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colTotalPrice.Name = "colTotalPrice"
        Me.colTotalPrice.Text = "Total Price"
        Me.colTotalPrice.Width.Absolute = 80
        '
        'colIdealML
        '
        Me.colIdealML.DisplayIndex = 8
        Me.colIdealML.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colIdealML.Name = "colIdealML"
        Me.colIdealML.Text = "Ideal ML"
        Me.colIdealML.Width.Absolute = 60
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle1
        '
        Me.ElementStyle1.Class = ""
        Me.ElementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle1.Name = "ElementStyle1"
        Me.ElementStyle1.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tcResources
        '
        Me.tcResources.BackColor = System.Drawing.Color.Transparent
        Me.tcResources.CanReorderTabs = True
        Me.tcResources.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tcResources.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tcResources.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tcResources.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tcResources.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tcResources.Controls.Add(Me.TabControlPanel4)
        Me.tcResources.Controls.Add(Me.TabControlPanel1)
        Me.tcResources.Controls.Add(Me.TabControlPanel2)
        Me.tcResources.Controls.Add(Me.TabControlPanel3)
        Me.tcResources.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcResources.Location = New System.Drawing.Point(0, 0)
        Me.tcResources.Name = "tcResources"
        Me.tcResources.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tcResources.SelectedTabIndex = 1
        Me.tcResources.Size = New System.Drawing.Size(868, 357)
        Me.tcResources.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tcResources.TabIndex = 1
        Me.tcResources.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tcResources.Tabs.Add(Me.tiInvention)
        Me.tcResources.Tabs.Add(Me.tiProductionResources)
        Me.tcResources.Tabs.Add(Me.tiBatchResources)
        Me.tcResources.Tabs.Add(Me.tiResourcesOwned)
        Me.tcResources.Text = "TabControl1"
        '
        'TabControlPanel2
        '
        Me.TabControlPanel2.Controls.Add(Me.adtResources)
        Me.TabControlPanel2.Controls.Add(Me.pnlResources)
        Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel2.Name = "TabControlPanel2"
        Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel2.Size = New System.Drawing.Size(868, 334)
        Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel2.Style.GradientAngle = 90
        Me.TabControlPanel2.TabIndex = 2
        Me.TabControlPanel2.TabItem = Me.tiProductionResources
        '
        'tiProductionResources
        '
        Me.tiProductionResources.AttachedControl = Me.TabControlPanel2
        Me.tiProductionResources.Name = "tiProductionResources"
        Me.tiProductionResources.Text = "Production Resources"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.adtInventionResources)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(868, 334)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 1
        Me.TabControlPanel1.TabItem = Me.tiInvention
        '
        'adtInventionResources
        '
        Me.adtInventionResources.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtInventionResources.AllowDrop = True
        Me.adtInventionResources.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtInventionResources.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtInventionResources.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtInventionResources.Columns.Add(Me.colIRItem)
        Me.adtInventionResources.Columns.Add(Me.colIRQuantity)
        Me.adtInventionResources.Columns.Add(Me.colIRPrice)
        Me.adtInventionResources.Columns.Add(Me.colIRValue)
        Me.adtInventionResources.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtInventionResources.ExpandWidth = 0
        Me.adtInventionResources.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtInventionResources.Location = New System.Drawing.Point(1, 1)
        Me.adtInventionResources.Name = "adtInventionResources"
        Me.adtInventionResources.NodesConnector = Me.NodeConnector2
        Me.adtInventionResources.NodeStyle = Me.ElementStyle2
        Me.adtInventionResources.PathSeparator = ";"
        Me.adtInventionResources.Size = New System.Drawing.Size(866, 332)
        Me.adtInventionResources.Styles.Add(Me.ElementStyle2)
        Me.adtInventionResources.TabIndex = 1
        Me.adtInventionResources.Text = "AdvTree1"
        '
        'colIRItem
        '
        Me.colIRItem.DisplayIndex = 1
        Me.colIRItem.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colIRItem.Name = "colIRItem"
        Me.colIRItem.Text = "Item Type"
        Me.colIRItem.Width.Absolute = 250
        '
        'colIRQuantity
        '
        Me.colIRQuantity.DisplayIndex = 2
        Me.colIRQuantity.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colIRQuantity.Name = "colIRQuantity"
        Me.colIRQuantity.Text = "Quantity"
        Me.colIRQuantity.Width.Absolute = 100
        '
        'colIRPrice
        '
        Me.colIRPrice.DisplayIndex = 3
        Me.colIRPrice.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colIRPrice.Name = "colIRPrice"
        Me.colIRPrice.Text = "Price"
        Me.colIRPrice.Width.Absolute = 100
        '
        'colIRValue
        '
        Me.colIRValue.DisplayIndex = 4
        Me.colIRValue.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colIRValue.Name = "colIRValue"
        Me.colIRValue.Text = "Value"
        Me.colIRValue.Width.Absolute = 100
        '
        'NodeConnector2
        '
        Me.NodeConnector2.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle2
        '
        Me.ElementStyle2.Class = ""
        Me.ElementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle2.Name = "ElementStyle2"
        Me.ElementStyle2.TextColor = System.Drawing.SystemColors.ControlText
        '
        'tiInvention
        '
        Me.tiInvention.AttachedControl = Me.TabControlPanel1
        Me.tiInvention.Name = "tiInvention"
        Me.tiInvention.Text = "Invention Resources"
        Me.tiInvention.Visible = False
        '
        'TabControlPanel3
        '
        Me.TabControlPanel3.Controls.Add(Me.adtOwnedResources)
        Me.TabControlPanel3.Controls.Add(Me.PanelEx1)
        Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel3.Name = "TabControlPanel3"
        Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel3.Size = New System.Drawing.Size(868, 334)
        Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel3.Style.GradientAngle = 90
        Me.TabControlPanel3.TabIndex = 3
        Me.TabControlPanel3.TabItem = Me.tiResourcesOwned
        '
        'adtOwnedResources
        '
        Me.adtOwnedResources.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtOwnedResources.AllowDrop = True
        Me.adtOwnedResources.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtOwnedResources.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtOwnedResources.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtOwnedResources.Columns.Add(Me.colROMaterial)
        Me.adtOwnedResources.Columns.Add(Me.colORQuantityRequired)
        Me.adtOwnedResources.Columns.Add(Me.colORQuantityOwned)
        Me.adtOwnedResources.Columns.Add(Me.colORSurplus)
        Me.adtOwnedResources.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtOwnedResources.ExpandWidth = 20
        Me.adtOwnedResources.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtOwnedResources.Location = New System.Drawing.Point(1, 25)
        Me.adtOwnedResources.Name = "adtOwnedResources"
        Me.adtOwnedResources.NodesConnector = Me.NodeConnector3
        Me.adtOwnedResources.NodeStyle = Me.ElementStyle3
        Me.adtOwnedResources.PathSeparator = ";"
        Me.adtOwnedResources.Size = New System.Drawing.Size(866, 308)
        Me.adtOwnedResources.Styles.Add(Me.ElementStyle3)
        Me.adtOwnedResources.TabIndex = 18
        Me.adtOwnedResources.Text = "AdvTree1"
        '
        'colROMaterial
        '
        Me.colROMaterial.DisplayIndex = 1
        Me.colROMaterial.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colROMaterial.Name = "colROMaterial"
        Me.colROMaterial.Text = "Required Material"
        Me.colROMaterial.Width.Absolute = 450
        '
        'colORQuantityRequired
        '
        Me.colORQuantityRequired.DisplayIndex = 2
        Me.colORQuantityRequired.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colORQuantityRequired.Name = "colORQuantityRequired"
        Me.colORQuantityRequired.Text = "Qty Required"
        Me.colORQuantityRequired.Width.Absolute = 90
        '
        'colORQuantityOwned
        '
        Me.colORQuantityOwned.DisplayIndex = 3
        Me.colORQuantityOwned.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colORQuantityOwned.Name = "colORQuantityOwned"
        Me.colORQuantityOwned.Text = "Qty Owned"
        Me.colORQuantityOwned.Width.Absolute = 90
        '
        'colORSurplus
        '
        Me.colORSurplus.DisplayIndex = 4
        Me.colORSurplus.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colORSurplus.Name = "colORSurplus"
        Me.colORSurplus.Text = "Surplus"
        Me.colORSurplus.Width.Absolute = 90
        '
        'NodeConnector3
        '
        Me.NodeConnector3.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle3
        '
        Me.ElementStyle3.Class = ""
        Me.ElementStyle3.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle3.Name = "ElementStyle3"
        Me.ElementStyle3.TextColor = System.Drawing.SystemColors.ControlText
        '
        'PanelEx1
        '
        Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx1.Controls.Add(Me.chkAdvancedResourceAllocation)
        Me.PanelEx1.Controls.Add(Me.cboAssetSelection)
        Me.PanelEx1.Controls.Add(Me.btnExport)
        Me.PanelEx1.Controls.Add(Me.lblMaxUnits)
        Me.PanelEx1.Controls.Add(Me.lblAssetSelection)
        Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelEx1.Location = New System.Drawing.Point(1, 1)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.Size = New System.Drawing.Size(866, 24)
        Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx1.Style.GradientAngle = 90
        Me.PanelEx1.TabIndex = 17
        '
        'chkAdvancedResourceAllocation
        '
        Me.chkAdvancedResourceAllocation.AutoSize = True
        '
        '
        '
        Me.chkAdvancedResourceAllocation.BackgroundStyle.Class = ""
        Me.chkAdvancedResourceAllocation.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkAdvancedResourceAllocation.Location = New System.Drawing.Point(273, 4)
        Me.chkAdvancedResourceAllocation.Name = "chkAdvancedResourceAllocation"
        Me.chkAdvancedResourceAllocation.Size = New System.Drawing.Size(168, 16)
        Me.chkAdvancedResourceAllocation.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.STT.SetSuperTooltip(Me.chkAdvancedResourceAllocation, New DevComponents.DotNetBar.SuperTooltipInfo("", "Advanced Resource Allocation", resources.GetString("chkAdvancedResourceAllocation.SuperTooltip"), Nothing, Global.EveHQ.Prism.My.Resources.Resources.Question32, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.chkAdvancedResourceAllocation.TabIndex = 19
        Me.chkAdvancedResourceAllocation.Text = "Advanced Resource Allocation"
        '
        'cboAssetSelection
        '
        Me.cboAssetSelection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
        Me.cboAssetSelection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
        '
        '
        '
        Me.cboAssetSelection.BackgroundStyle.Class = "TextBoxBorder"
        Me.cboAssetSelection.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.cboAssetSelection.ButtonDropDown.Visible = True
        Me.cboAssetSelection.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.cboAssetSelection.Location = New System.Drawing.Point(93, 2)
        Me.cboAssetSelection.Name = "cboAssetSelection"
        Me.cboAssetSelection.Size = New System.Drawing.Size(174, 20)
        Me.cboAssetSelection.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboAssetSelection.TabIndex = 18
        Me.cboAssetSelection.Text = ""
        Me.cboAssetSelection.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        '
        'btnExport
        '
        Me.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnExport.Location = New System.Drawing.Point(788, 2)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 20)
        Me.btnExport.SplitButton = True
        Me.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnExport.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnExportToCSV, Me.btnExportToTSV, Me.btnAddShortfallToReq, Me.btnAddAllToReq})
        Me.STT.SetSuperTooltip(Me.btnExport, New DevComponents.DotNetBar.SuperTooltipInfo("", "Resources Export", "Exports resources to a variety of targets, including Excel and EveHQ Requisitions" & _
            ".", Global.EveHQ.Prism.My.Resources.Resources.Question32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.btnExport.TabIndex = 15
        Me.btnExport.Text = "Export"
        '
        'btnExportToCSV
        '
        Me.btnExportToCSV.Name = "btnExportToCSV"
        Me.btnExportToCSV.Text = "Export to CSV"
        '
        'btnExportToTSV
        '
        Me.btnExportToTSV.Name = "btnExportToTSV"
        Me.btnExportToTSV.Text = "Export To TSV (Excel)"
        '
        'btnAddShortfallToReq
        '
        Me.btnAddShortfallToReq.GlobalItem = False
        Me.btnAddShortfallToReq.Name = "btnAddShortfallToReq"
        Me.btnAddShortfallToReq.Text = "Add To Requisition (Shortfall)"
        '
        'btnAddAllToReq
        '
        Me.btnAddAllToReq.GlobalItem = False
        Me.btnAddAllToReq.Name = "btnAddAllToReq"
        Me.btnAddAllToReq.Text = "Add to Requisition (All)"
        '
        'lblMaxUnits
        '
        Me.lblMaxUnits.AutoSize = True
        Me.lblMaxUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxUnits.Location = New System.Drawing.Point(464, 5)
        Me.lblMaxUnits.Name = "lblMaxUnits"
        Me.lblMaxUnits.Size = New System.Drawing.Size(138, 13)
        Me.STT.SetSuperTooltip(Me.lblMaxUnits, New DevComponents.DotNetBar.SuperTooltipInfo("", "Maximum Producable Units", "Shows the maximum number of producable units based on the available resources for" & _
            " the selected owners.", Global.EveHQ.Prism.My.Resources.Resources.Question32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
        Me.lblMaxUnits.TabIndex = 12
        Me.lblMaxUnits.Text = "Maximum Producable Units:"
        '
        'lblAssetSelection
        '
        Me.lblAssetSelection.AutoSize = True
        Me.lblAssetSelection.BackColor = System.Drawing.Color.Transparent
        Me.lblAssetSelection.Location = New System.Drawing.Point(2, 5)
        Me.lblAssetSelection.Name = "lblAssetSelection"
        Me.lblAssetSelection.Size = New System.Drawing.Size(84, 13)
        Me.lblAssetSelection.TabIndex = 14
        Me.lblAssetSelection.Text = "Asset Selection:"
        '
        'tiResourcesOwned
        '
        Me.tiResourcesOwned.AttachedControl = Me.TabControlPanel3
        Me.tiResourcesOwned.Name = "tiResourcesOwned"
        Me.tiResourcesOwned.Text = "Resources Owned"
        '
        'TabControlPanel4
        '
        Me.TabControlPanel4.Controls.Add(Me.adtBatchResources)
        Me.TabControlPanel4.Controls.Add(Me.pnlBatch)
        Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel4.Name = "TabControlPanel4"
        Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel4.Size = New System.Drawing.Size(868, 334)
        Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel4.Style.GradientAngle = 90
        Me.TabControlPanel4.TabIndex = 4
        Me.TabControlPanel4.TabItem = Me.tiBatchResources
        '
        'adtBatchResources
        '
        Me.adtBatchResources.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtBatchResources.AllowDrop = True
        Me.adtBatchResources.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtBatchResources.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtBatchResources.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtBatchResources.Columns.Add(Me.colBatchItem)
        Me.adtBatchResources.Columns.Add(Me.colBatchQuantity)
        Me.adtBatchResources.Columns.Add(Me.colBatchPrice)
        Me.adtBatchResources.Columns.Add(Me.colBatchValue)
        Me.adtBatchResources.Columns.Add(Me.colBatchVolume)
        Me.adtBatchResources.Dock = System.Windows.Forms.DockStyle.Fill
        Me.adtBatchResources.ExpandWidth = 0
        Me.adtBatchResources.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtBatchResources.Location = New System.Drawing.Point(1, 25)
        Me.adtBatchResources.Name = "adtBatchResources"
        Me.adtBatchResources.NodesConnector = Me.NodeConnector4
        Me.adtBatchResources.NodeStyle = Me.ElementStyle4
        Me.adtBatchResources.PathSeparator = ";"
        Me.adtBatchResources.Size = New System.Drawing.Size(866, 308)
        Me.adtBatchResources.Styles.Add(Me.ElementStyle4)
        Me.adtBatchResources.TabIndex = 2
        Me.adtBatchResources.Text = "AdvTree1"
        '
        'colBatchItem
        '
        Me.colBatchItem.DisplayIndex = 1
        Me.colBatchItem.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colBatchItem.Name = "colBatchItem"
        Me.colBatchItem.Text = "Item Type"
        Me.colBatchItem.Width.Absolute = 250
        '
        'colBatchQuantity
        '
        Me.colBatchQuantity.DisplayIndex = 2
        Me.colBatchQuantity.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colBatchQuantity.Name = "colBatchQuantity"
        Me.colBatchQuantity.Text = "Quantity"
        Me.colBatchQuantity.Width.Absolute = 100
        '
        'colBatchPrice
        '
        Me.colBatchPrice.DisplayIndex = 3
        Me.colBatchPrice.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colBatchPrice.Name = "colBatchPrice"
        Me.colBatchPrice.Text = "Price"
        Me.colBatchPrice.Width.Absolute = 100
        '
        'colBatchValue
        '
        Me.colBatchValue.DisplayIndex = 4
        Me.colBatchValue.ImageAlignment = DevComponents.AdvTree.eColumnImageAlignment.Right
        Me.colBatchValue.Name = "colBatchValue"
        Me.colBatchValue.Text = "Value"
        Me.colBatchValue.Width.Absolute = 100
        '
        'NodeConnector4
        '
        Me.NodeConnector4.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle4
        '
        Me.ElementStyle4.Class = ""
        Me.ElementStyle4.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle4.Name = "ElementStyle4"
        Me.ElementStyle4.TextColor = System.Drawing.SystemColors.ControlText
        '
        'pnlBatch
        '
        Me.pnlBatch.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlBatch.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlBatch.Controls.Add(Me.lblBatchTotals)
        Me.pnlBatch.Controls.Add(Me.btnUpdateBatchPrices)
        Me.pnlBatch.Controls.Add(Me.lblBatchName)
        Me.pnlBatch.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlBatch.Location = New System.Drawing.Point(1, 1)
        Me.pnlBatch.Name = "pnlBatch"
        Me.pnlBatch.Size = New System.Drawing.Size(866, 24)
        Me.pnlBatch.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlBatch.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlBatch.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlBatch.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlBatch.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlBatch.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlBatch.Style.GradientAngle = 90
        Me.pnlBatch.TabIndex = 3
        '
        'btnUpdateBatchPrices
        '
        Me.btnUpdateBatchPrices.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnUpdateBatchPrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdateBatchPrices.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnUpdateBatchPrices.Location = New System.Drawing.Point(763, 2)
        Me.btnUpdateBatchPrices.Name = "btnUpdateBatchPrices"
        Me.btnUpdateBatchPrices.Size = New System.Drawing.Size(100, 20)
        Me.btnUpdateBatchPrices.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnUpdateBatchPrices.TabIndex = 3
        Me.btnUpdateBatchPrices.Text = "Update Prices"
        '
        'lblBatchName
        '
        Me.lblBatchName.AutoSize = True
        Me.lblBatchName.Location = New System.Drawing.Point(3, 5)
        Me.lblBatchName.Name = "lblBatchName"
        Me.lblBatchName.Size = New System.Drawing.Size(82, 13)
        Me.lblBatchName.TabIndex = 0
        Me.lblBatchName.Text = "Batch: <None>"
        '
        'tiBatchResources
        '
        Me.tiBatchResources.AttachedControl = Me.TabControlPanel4
        Me.tiBatchResources.Name = "tiBatchResources"
        Me.tiBatchResources.Text = "Batch Resources"
        '
        'STT
        '
        Me.STT.DefaultFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.STT.IgnoreFormActiveState = True
        Me.STT.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.STT.PositionBelowControl = False
        '
        'colBatchVolume
        '
        Me.colBatchVolume.DisplayIndex = 5
        Me.colBatchVolume.Name = "colBatchVolume"
        Me.colBatchVolume.Text = "Volume (m�)"
        Me.colBatchVolume.Width.Absolute = 100
        '
        'lblBatchTotals
        '
        Me.lblBatchTotals.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBatchTotals.Location = New System.Drawing.Point(249, 5)
        Me.lblBatchTotals.Name = "lblBatchTotals"
        Me.lblBatchTotals.Size = New System.Drawing.Size(508, 13)
        Me.lblBatchTotals.TabIndex = 4
        Me.lblBatchTotals.Text = "Batch Value: n/a, Batch Volume: n/a"
        Me.lblBatchTotals.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'PrismResources
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tcResources)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "PrismResources"
        Me.Size = New System.Drawing.Size(868, 357)
        Me.pnlResources.ResumeLayout(False)
        Me.pnlResources.PerformLayout()
        CType(Me.adtResources, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tcResources, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcResources.ResumeLayout(False)
        Me.TabControlPanel2.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        CType(Me.adtInventionResources, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel3.ResumeLayout(False)
        CType(Me.adtOwnedResources, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelEx1.ResumeLayout(False)
        Me.PanelEx1.PerformLayout()
        Me.TabControlPanel4.ResumeLayout(False)
        CType(Me.adtBatchResources, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBatch.ResumeLayout(False)
        Me.pnlBatch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlResources As DevComponents.DotNetBar.PanelEx
    Friend WithEvents adtResources As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle1 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents chkUseStandardCosting As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents chkShowSkills As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents colMaterial As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colPerfectUnits As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colWasteUnits As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colTotalUnits As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colUnitPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colTotalPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIdealML As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents tcResources As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiProductionResources As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiInvention As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiResourcesOwned As DevComponents.DotNetBar.TabItem
    Friend WithEvents adtInventionResources As DevComponents.AdvTree.AdvTree
    Friend WithEvents colIRItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIRQuantity As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIRPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colIRValue As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector2 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle2 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents btnExport As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnExportToCSV As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnExportToTSV As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents lblAssetSelection As System.Windows.Forms.Label
    Friend WithEvents lblMaxUnits As System.Windows.Forms.Label
    Friend WithEvents adtOwnedResources As DevComponents.AdvTree.AdvTree
    Friend WithEvents colROMaterial As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colORQuantityRequired As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colORQuantityOwned As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colORSurplus As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector3 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle3 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents cboAssetSelection As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Friend WithEvents btnAddShortfallToReq As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnAddAllToReq As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents STT As DevComponents.DotNetBar.SuperTooltip
    Friend WithEvents colBP As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents chkAdvancedResourceAllocation As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tiBatchResources As DevComponents.DotNetBar.TabItem
    Friend WithEvents adtBatchResources As DevComponents.AdvTree.AdvTree
    Friend WithEvents colBatchItem As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBatchQuantity As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBatchPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBatchValue As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector4 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle4 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents pnlBatch As DevComponents.DotNetBar.PanelEx
    Friend WithEvents lblBatchName As System.Windows.Forms.Label
    Friend WithEvents btnAlterResourcePrices As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnUpdateBatchPrices As DevComponents.DotNetBar.ButtonX
    Friend WithEvents colBatchVolume As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents lblBatchTotals As System.Windows.Forms.Label

End Class
