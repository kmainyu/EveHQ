<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShowInfo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmShowInfo))
        Me.lvwAffects = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colTypeName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colTypeAtttribute = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwAudit = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colAudit = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwAttributes = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colAttribute = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colStandardValue = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colPilotValue = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblUsableTime = New System.Windows.Forms.LinkLabel()
        Me.lblUsable = New System.Windows.Forms.Label()
        Me.picItem = New System.Windows.Forms.PictureBox()
        Me.lblItemName = New System.Windows.Forms.Label()
        Me.pbPilot = New System.Windows.Forms.PictureBox()
        Me.tabShowInfo = New DevComponents.DotNetBar.TabControl()
        Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
        Me.lblDescription = New DevComponents.DotNetBar.LabelX()
        Me.tabDescription = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel5 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tabAudit = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel4 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tabAffects = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel3 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tvwReqs = New DevComponents.AdvTree.AdvTree()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle1 = New DevComponents.DotNetBar.ElementStyle()
        Me.tabSkills = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel()
        Me.tabAttributes = New DevComponents.DotNetBar.TabItem(Me.components)
        Me.SuperTooltip1 = New DevComponents.DotNetBar.SuperTooltip()
        Me.pnlInfo = New DevComponents.DotNetBar.PanelEx()
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tabShowInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabShowInfo.SuspendLayout()
        Me.TabControlPanel1.SuspendLayout()
        Me.TabControlPanel5.SuspendLayout()
        Me.TabControlPanel4.SuspendLayout()
        Me.TabControlPanel3.SuspendLayout()
        CType(Me.tvwReqs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlPanel2.SuspendLayout()
        Me.pnlInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvwAffects
        '
        '
        '
        '
        Me.lvwAffects.Border.Class = "ListViewBorder"
        Me.lvwAffects.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwAffects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTypeName, Me.colType, Me.colTypeAtttribute})
        Me.lvwAffects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwAffects.FullRowSelect = True
        Me.lvwAffects.GridLines = True
        Me.lvwAffects.Location = New System.Drawing.Point(1, 1)
        Me.lvwAffects.Name = "lvwAffects"
        Me.lvwAffects.Size = New System.Drawing.Size(559, 439)
        Me.lvwAffects.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwAffects.TabIndex = 4
        Me.lvwAffects.UseCompatibleStateImageBehavior = False
        Me.lvwAffects.View = System.Windows.Forms.View.Details
        '
        'colTypeName
        '
        Me.colTypeName.Text = "Affected By"
        Me.colTypeName.Width = 250
        '
        'colType
        '
        Me.colType.Text = "Type"
        Me.colType.Width = 100
        '
        'colTypeAtttribute
        '
        Me.colTypeAtttribute.Text = "Attribute"
        Me.colTypeAtttribute.Width = 180
        '
        'lvwAudit
        '
        '
        '
        '
        Me.lvwAudit.Border.Class = "ListViewBorder"
        Me.lvwAudit.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwAudit.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAudit})
        Me.lvwAudit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwAudit.FullRowSelect = True
        Me.lvwAudit.GridLines = True
        Me.lvwAudit.Location = New System.Drawing.Point(1, 1)
        Me.lvwAudit.Name = "lvwAudit"
        Me.lvwAudit.Size = New System.Drawing.Size(559, 439)
        Me.lvwAudit.TabIndex = 3
        Me.lvwAudit.UseCompatibleStateImageBehavior = False
        Me.lvwAudit.View = System.Windows.Forms.View.Details
        '
        'colAudit
        '
        Me.colAudit.Text = "Details"
        Me.colAudit.Width = 540
        '
        'lvwAttributes
        '
        '
        '
        '
        Me.lvwAttributes.Border.Class = "ListViewBorder"
        Me.lvwAttributes.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwAttributes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttribute, Me.colStandardValue, Me.colPilotValue})
        Me.lvwAttributes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwAttributes.FullRowSelect = True
        Me.lvwAttributes.GridLines = True
        Me.lvwAttributes.Location = New System.Drawing.Point(1, 1)
        Me.lvwAttributes.Name = "lvwAttributes"
        Me.lvwAttributes.Size = New System.Drawing.Size(559, 439)
        Me.lvwAttributes.TabIndex = 1
        Me.lvwAttributes.UseCompatibleStateImageBehavior = False
        Me.lvwAttributes.View = System.Windows.Forms.View.Details
        '
        'colAttribute
        '
        Me.colAttribute.Text = "Attribute"
        Me.colAttribute.Width = 240
        '
        'colStandardValue
        '
        Me.colStandardValue.Text = "Base Value"
        Me.colStandardValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colStandardValue.Width = 150
        '
        'colPilotValue
        '
        Me.colPilotValue.Text = "Actual Value"
        Me.colPilotValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colPilotValue.Width = 150
        '
        'lblUsableTime
        '
        Me.lblUsableTime.AutoSize = True
        Me.lblUsableTime.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsableTime.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblUsableTime.Location = New System.Drawing.Point(120, 60)
        Me.lblUsableTime.Name = "lblUsableTime"
        Me.lblUsableTime.Size = New System.Drawing.Size(68, 13)
        Me.lblUsableTime.TabIndex = 15
        Me.lblUsableTime.TabStop = True
        Me.lblUsableTime.Text = "Usable Time:"
        '
        'lblUsable
        '
        Me.lblUsable.AutoSize = True
        Me.lblUsable.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsable.Location = New System.Drawing.Point(120, 41)
        Me.lblUsable.Name = "lblUsable"
        Me.lblUsable.Size = New System.Drawing.Size(39, 13)
        Me.lblUsable.TabIndex = 14
        Me.lblUsable.Text = "Usable"
        '
        'picItem
        '
        Me.picItem.Location = New System.Drawing.Point(12, 32)
        Me.picItem.Name = "picItem"
        Me.picItem.Size = New System.Drawing.Size(48, 48)
        Me.picItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picItem.TabIndex = 13
        Me.picItem.TabStop = False
        '
        'lblItemName
        '
        Me.lblItemName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblItemName.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemName.Location = New System.Drawing.Point(12, 9)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(559, 21)
        Me.lblItemName.TabIndex = 16
        Me.lblItemName.Text = "Item Label"
        '
        'pbPilot
        '
        Me.pbPilot.Image = Global.EveHQ.HQF.My.Resources.Resources.noitem
        Me.pbPilot.Location = New System.Drawing.Point(82, 41)
        Me.pbPilot.Name = "pbPilot"
        Me.pbPilot.Size = New System.Drawing.Size(32, 32)
        Me.pbPilot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbPilot.TabIndex = 17
        Me.pbPilot.TabStop = False
        '
        'tabShowInfo
        '
        Me.tabShowInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabShowInfo.BackColor = System.Drawing.Color.Transparent
        Me.tabShowInfo.CanReorderTabs = True
        Me.tabShowInfo.ColorScheme.TabBackground = System.Drawing.Color.Transparent
        Me.tabShowInfo.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
        Me.tabShowInfo.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
        Me.tabShowInfo.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
        Me.tabShowInfo.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
        Me.tabShowInfo.Controls.Add(Me.TabControlPanel1)
        Me.tabShowInfo.Controls.Add(Me.TabControlPanel5)
        Me.tabShowInfo.Controls.Add(Me.TabControlPanel4)
        Me.tabShowInfo.Controls.Add(Me.TabControlPanel3)
        Me.tabShowInfo.Controls.Add(Me.TabControlPanel2)
        Me.tabShowInfo.Location = New System.Drawing.Point(11, 86)
        Me.tabShowInfo.Name = "tabShowInfo"
        Me.tabShowInfo.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tabShowInfo.SelectedTabIndex = 1
        Me.tabShowInfo.Size = New System.Drawing.Size(561, 464)
        Me.tabShowInfo.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
        Me.tabShowInfo.TabIndex = 18
        Me.tabShowInfo.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabShowInfo.Tabs.Add(Me.tabDescription)
        Me.tabShowInfo.Tabs.Add(Me.tabAttributes)
        Me.tabShowInfo.Tabs.Add(Me.tabSkills)
        Me.tabShowInfo.Tabs.Add(Me.tabAffects)
        Me.tabShowInfo.Tabs.Add(Me.tabAudit)
        Me.tabShowInfo.Text = "TabControl1"
        '
        'TabControlPanel1
        '
        Me.TabControlPanel1.Controls.Add(Me.lblDescription)
        Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel1.Name = "TabControlPanel1"
        Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel1.Size = New System.Drawing.Size(561, 441)
        Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel1.Style.GradientAngle = 90
        Me.TabControlPanel1.TabIndex = 1
        Me.TabControlPanel1.TabItem = Me.tabDescription
        '
        'lblDescription
        '
        '
        '
        '
        Me.lblDescription.BackgroundStyle.Class = ""
        Me.lblDescription.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDescription.Location = New System.Drawing.Point(1, 1)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.PaddingBottom = 10
        Me.lblDescription.Size = New System.Drawing.Size(559, 439)
        Me.lblDescription.TabIndex = 0
        Me.lblDescription.TextLineAlignment = System.Drawing.StringAlignment.Near
        Me.lblDescription.WordWrap = True
        '
        'tabDescription
        '
        Me.tabDescription.AttachedControl = Me.TabControlPanel1
        Me.tabDescription.Name = "tabDescription"
        Me.tabDescription.Text = "Description"
        '
        'TabControlPanel5
        '
        Me.TabControlPanel5.Controls.Add(Me.lvwAudit)
        Me.TabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel5.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel5.Name = "TabControlPanel5"
        Me.TabControlPanel5.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel5.Size = New System.Drawing.Size(561, 441)
        Me.TabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel5.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel5.Style.GradientAngle = 90
        Me.TabControlPanel5.TabIndex = 5
        Me.TabControlPanel5.TabItem = Me.tabAudit
        '
        'tabAudit
        '
        Me.tabAudit.AttachedControl = Me.TabControlPanel5
        Me.tabAudit.Name = "tabAudit"
        Me.tabAudit.Text = "Audit"
        '
        'TabControlPanel4
        '
        Me.TabControlPanel4.Controls.Add(Me.lvwAffects)
        Me.TabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel4.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel4.Name = "TabControlPanel4"
        Me.TabControlPanel4.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel4.Size = New System.Drawing.Size(561, 441)
        Me.TabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel4.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel4.Style.GradientAngle = 90
        Me.TabControlPanel4.TabIndex = 4
        Me.TabControlPanel4.TabItem = Me.tabAffects
        '
        'tabAffects
        '
        Me.tabAffects.AttachedControl = Me.TabControlPanel4
        Me.tabAffects.Name = "tabAffects"
        Me.tabAffects.Text = "Affected By"
        '
        'TabControlPanel3
        '
        Me.TabControlPanel3.Controls.Add(Me.tvwReqs)
        Me.TabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel3.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel3.Name = "TabControlPanel3"
        Me.TabControlPanel3.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel3.Size = New System.Drawing.Size(561, 441)
        Me.TabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel3.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel3.Style.GradientAngle = 90
        Me.TabControlPanel3.TabIndex = 3
        Me.TabControlPanel3.TabItem = Me.tabSkills
        '
        'tvwReqs
        '
        Me.tvwReqs.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.tvwReqs.AllowDrop = True
        Me.tvwReqs.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.tvwReqs.BackgroundStyle.Class = "TreeBorderKey"
        Me.tvwReqs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.tvwReqs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwReqs.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Triangle
        Me.tvwReqs.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.tvwReqs.Location = New System.Drawing.Point(1, 1)
        Me.tvwReqs.Name = "tvwReqs"
        Me.tvwReqs.NodesConnector = Me.NodeConnector1
        Me.tvwReqs.NodeSpacing = 5
        Me.tvwReqs.NodeStyle = Me.ElementStyle1
        Me.tvwReqs.PathSeparator = ";"
        Me.tvwReqs.Size = New System.Drawing.Size(559, 439)
        Me.tvwReqs.Styles.Add(Me.ElementStyle1)
        Me.tvwReqs.TabIndex = 1
        Me.tvwReqs.Text = "AdvTree1"
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
        'tabSkills
        '
        Me.tabSkills.AttachedControl = Me.TabControlPanel3
        Me.tabSkills.Name = "tabSkills"
        Me.tabSkills.Text = "Required Skills"
        '
        'TabControlPanel2
        '
        Me.TabControlPanel2.Controls.Add(Me.lvwAttributes)
        Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
        Me.TabControlPanel2.Name = "TabControlPanel2"
        Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
        Me.TabControlPanel2.Size = New System.Drawing.Size(561, 441)
        Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
        Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
            Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
        Me.TabControlPanel2.Style.GradientAngle = 90
        Me.TabControlPanel2.TabIndex = 2
        Me.TabControlPanel2.TabItem = Me.tabAttributes
        '
        'tabAttributes
        '
        Me.tabAttributes.AttachedControl = Me.TabControlPanel2
        Me.tabAttributes.Name = "tabAttributes"
        Me.tabAttributes.Text = "Attributes"
        '
        'SuperTooltip1
        '
        Me.SuperTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.SuperTooltip1.PositionBelowControl = False
        '
        'pnlInfo
        '
        Me.pnlInfo.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlInfo.Controls.Add(Me.lblItemName)
        Me.pnlInfo.Controls.Add(Me.tabShowInfo)
        Me.pnlInfo.Controls.Add(Me.picItem)
        Me.pnlInfo.Controls.Add(Me.pbPilot)
        Me.pnlInfo.Controls.Add(Me.lblUsable)
        Me.pnlInfo.Controls.Add(Me.lblUsableTime)
        Me.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlInfo.Location = New System.Drawing.Point(0, 0)
        Me.pnlInfo.Name = "pnlInfo"
        Me.pnlInfo.Size = New System.Drawing.Size(584, 562)
        Me.pnlInfo.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlInfo.Style.GradientAngle = 90
        Me.pnlInfo.TabIndex = 19
        '
        'frmShowInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 562)
        Me.Controls.Add(Me.pnlInfo)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmShowInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Show Info"
        CType(Me.picItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tabShowInfo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabShowInfo.ResumeLayout(False)
        Me.TabControlPanel1.ResumeLayout(False)
        Me.TabControlPanel5.ResumeLayout(False)
        Me.TabControlPanel4.ResumeLayout(False)
        Me.TabControlPanel3.ResumeLayout(False)
        CType(Me.tvwReqs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlPanel2.ResumeLayout(False)
        Me.pnlInfo.ResumeLayout(False)
        Me.pnlInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblUsableTime As System.Windows.Forms.LinkLabel
    Friend WithEvents lblUsable As System.Windows.Forms.Label
    Friend WithEvents picItem As System.Windows.Forms.PictureBox
    Friend WithEvents colAttribute As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStandardValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblItemName As System.Windows.Forms.Label
    Friend WithEvents pbPilot As System.Windows.Forms.PictureBox
    Friend WithEvents colAudit As System.Windows.Forms.ColumnHeader
    Friend WithEvents colPilotValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTypeName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTypeAtttribute As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabShowInfo As DevComponents.DotNetBar.TabControl
    Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tabDescription As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tabAttributes As DevComponents.DotNetBar.TabItem
    Friend WithEvents lblDescription As DevComponents.DotNetBar.LabelX
    Friend WithEvents TabControlPanel5 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tabAudit As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel4 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tabAffects As DevComponents.DotNetBar.TabItem
    Friend WithEvents TabControlPanel3 As DevComponents.DotNetBar.TabControlPanel
    Friend WithEvents tabSkills As DevComponents.DotNetBar.TabItem
    Friend WithEvents lvwAttributes As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents tvwReqs As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents lvwAudit As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents lvwAffects As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents SuperTooltip1 As DevComponents.DotNetBar.SuperTooltip
    Friend WithEvents ElementStyle1 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents pnlInfo As DevComponents.DotNetBar.PanelEx
End Class
