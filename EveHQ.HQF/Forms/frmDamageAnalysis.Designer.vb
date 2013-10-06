Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmDamageAnalysis
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDamageAnalysis))
            Me.lblAttackerFitting = New System.Windows.Forms.Label()
            Me.lblAttackerPilot = New System.Windows.Forms.Label()
            Me.lblAttacker = New System.Windows.Forms.Label()
            Me.lblTarget = New System.Windows.Forms.Label()
            Me.lblTargetFitting = New System.Windows.Forms.Label()
            Me.lblTargetPilot = New System.Windows.Forms.Label()
            Me.nudVel = New System.Windows.Forms.NumericUpDown()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.nudRange = New System.Windows.Forms.NumericUpDown()
            Me.lblRange = New System.Windows.Forms.Label()
            Me.nudTargetVel = New System.Windows.Forms.NumericUpDown()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.EveSpace1 = New HQF.EveSpace()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.radMovableVel = New System.Windows.Forms.RadioButton()
            Me.nudAttackerVel = New System.Windows.Forms.NumericUpDown()
            Me.radManualVelocity = New System.Windows.Forms.RadioButton()
            Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
            Me.GroupPanel1 = New DevComponents.DotNetBar.Controls.GroupPanel()
            Me.btnOptimalRange = New DevComponents.DotNetBar.ButtonX()
            Me.btnRangeVsHitChance = New DevComponents.DotNetBar.ButtonX()
            Me.cboTargetFitting = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.cboAttackerFitting = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.cboTargetPilot = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.cboAttackerPilot = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.tabStats = New DevComponents.DotNetBar.TabControl()
            Me.TabControlPanel1 = New DevComponents.DotNetBar.TabControlPanel()
            Me.lblTurretStats = New DevComponents.DotNetBar.LabelX()
            Me.tabTurretStats = New DevComponents.DotNetBar.TabItem(Me.components)
            Me.TabControlPanel2 = New DevComponents.DotNetBar.TabControlPanel()
            Me.lblMissileStats = New DevComponents.DotNetBar.LabelX()
            Me.tabMissileStats = New DevComponents.DotNetBar.TabItem(Me.components)
            CType(Me.nudVel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudRange, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudTargetVel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudAttackerVel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.PanelEx1.SuspendLayout()
            Me.GroupPanel1.SuspendLayout()
            CType(Me.tabStats, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabStats.SuspendLayout()
            Me.TabControlPanel1.SuspendLayout()
            Me.TabControlPanel2.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblAttackerFitting
            '
            Me.lblAttackerFitting.AutoSize = True
            Me.lblAttackerFitting.Location = New System.Drawing.Point(56, 12)
            Me.lblAttackerFitting.Name = "lblAttackerFitting"
            Me.lblAttackerFitting.Size = New System.Drawing.Size(41, 13)
            Me.lblAttackerFitting.TabIndex = 4
            Me.lblAttackerFitting.Text = "Fitting:"
            '
            'lblAttackerPilot
            '
            Me.lblAttackerPilot.AutoSize = True
            Me.lblAttackerPilot.Location = New System.Drawing.Point(292, 12)
            Me.lblAttackerPilot.Name = "lblAttackerPilot"
            Me.lblAttackerPilot.Size = New System.Drawing.Size(31, 13)
            Me.lblAttackerPilot.TabIndex = 6
            Me.lblAttackerPilot.Text = "Pilot:"
            '
            'lblAttacker
            '
            Me.lblAttacker.AutoSize = True
            Me.lblAttacker.Location = New System.Drawing.Point(13, 12)
            Me.lblAttacker.Name = "lblAttacker"
            Me.lblAttacker.Size = New System.Drawing.Size(42, 13)
            Me.lblAttacker.TabIndex = 8
            Me.lblAttacker.Text = "Attack:"
            '
            'lblTarget
            '
            Me.lblTarget.AutoSize = True
            Me.lblTarget.Location = New System.Drawing.Point(13, 39)
            Me.lblTarget.Name = "lblTarget"
            Me.lblTarget.Size = New System.Drawing.Size(43, 13)
            Me.lblTarget.TabIndex = 13
            Me.lblTarget.Text = "Target:"
            '
            'lblTargetFitting
            '
            Me.lblTargetFitting.AutoSize = True
            Me.lblTargetFitting.Location = New System.Drawing.Point(56, 39)
            Me.lblTargetFitting.Name = "lblTargetFitting"
            Me.lblTargetFitting.Size = New System.Drawing.Size(41, 13)
            Me.lblTargetFitting.TabIndex = 9
            Me.lblTargetFitting.Text = "Fitting:"
            '
            'lblTargetPilot
            '
            Me.lblTargetPilot.AutoSize = True
            Me.lblTargetPilot.Location = New System.Drawing.Point(293, 39)
            Me.lblTargetPilot.Name = "lblTargetPilot"
            Me.lblTargetPilot.Size = New System.Drawing.Size(31, 13)
            Me.lblTargetPilot.TabIndex = 11
            Me.lblTargetPilot.Text = "Pilot:"
            '
            'nudVel
            '
            Me.nudVel.BackColor = System.Drawing.SystemColors.Window
            Me.nudVel.DecimalPlaces = 2
            Me.nudVel.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
            Me.nudVel.Location = New System.Drawing.Point(299, 359)
            Me.nudVel.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
            Me.nudVel.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
            Me.nudVel.Name = "nudVel"
            Me.nudVel.ReadOnly = True
            Me.nudVel.Size = New System.Drawing.Size(54, 21)
            Me.nudVel.TabIndex = 19
            Me.nudVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.nudVel.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(240, 361)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(53, 13)
            Me.Label1.TabIndex = 18
            Me.Label1.Text = "Vel Scale:"
            '
            'nudRange
            '
            Me.nudRange.DecimalPlaces = 5
            Me.nudRange.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
            Me.nudRange.Location = New System.Drawing.Point(79, 359)
            Me.nudRange.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.nudRange.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
            Me.nudRange.Name = "nudRange"
            Me.nudRange.Size = New System.Drawing.Size(75, 21)
            Me.nudRange.TabIndex = 17
            Me.nudRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.nudRange.Value = New Decimal(New Integer() {5, 0, 0, 65536})
            '
            'lblRange
            '
            Me.lblRange.AutoSize = True
            Me.lblRange.Location = New System.Drawing.Point(3, 363)
            Me.lblRange.Name = "lblRange"
            Me.lblRange.Size = New System.Drawing.Size(70, 13)
            Me.lblRange.TabIndex = 16
            Me.lblRange.Text = "Range Scale:"
            '
            'nudTargetVel
            '
            Me.nudTargetVel.DecimalPlaces = 2
            Me.nudTargetVel.Location = New System.Drawing.Point(271, 420)
            Me.nudTargetVel.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.nudTargetVel.Name = "nudTargetVel"
            Me.nudTargetVel.Size = New System.Drawing.Size(75, 21)
            Me.nudTargetVel.TabIndex = 27
            Me.nudTargetVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(219, 422)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(46, 13)
            Me.Label3.TabIndex = 26
            Me.Label3.Text = "Target -"
            '
            'EveSpace1
            '
            Me.EveSpace1.BackColor = System.Drawing.Color.Black
            Me.EveSpace1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.EveSpace1.Location = New System.Drawing.Point(3, 5)
            Me.EveSpace1.Name = "EveSpace1"
            Me.EveSpace1.RangeScale = 1.0R
            Me.EveSpace1.Size = New System.Drawing.Size(350, 350)
            Me.EveSpace1.SourceShip = Nothing
            Me.EveSpace1.TabIndex = 14
            Me.EveSpace1.TargetShip = Nothing
            Me.EveSpace1.UseIntegratedVelocity = False
            Me.EveSpace1.VelocityScale = 1.0R
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(77, 422)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(55, 13)
            Me.Label2.TabIndex = 26
            Me.Label2.Text = "Attacker -"
            '
            'radMovableVel
            '
            Me.radMovableVel.AutoSize = True
            Me.radMovableVel.Checked = True
            Me.radMovableVel.Location = New System.Drawing.Point(3, 386)
            Me.radMovableVel.Name = "radMovableVel"
            Me.radMovableVel.Size = New System.Drawing.Size(129, 17)
            Me.radMovableVel.TabIndex = 22
            Me.radMovableVel.TabStop = True
            Me.radMovableVel.Text = "Use Icons for Velocity"
            Me.radMovableVel.UseVisualStyleBackColor = True
            '
            'nudAttackerVel
            '
            Me.nudAttackerVel.DecimalPlaces = 2
            Me.nudAttackerVel.Location = New System.Drawing.Point(138, 420)
            Me.nudAttackerVel.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.nudAttackerVel.Name = "nudAttackerVel"
            Me.nudAttackerVel.Size = New System.Drawing.Size(75, 21)
            Me.nudAttackerVel.TabIndex = 25
            Me.nudAttackerVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'radManualVelocity
            '
            Me.radManualVelocity.AutoSize = True
            Me.radManualVelocity.Location = New System.Drawing.Point(3, 402)
            Me.radManualVelocity.Name = "radManualVelocity"
            Me.radManualVelocity.Size = New System.Drawing.Size(99, 17)
            Me.radManualVelocity.TabIndex = 23
            Me.radManualVelocity.Text = "Manual Velocity"
            Me.radManualVelocity.UseVisualStyleBackColor = True
            '
            'PanelEx1
            '
            Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
            Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.PanelEx1.Controls.Add(Me.GroupPanel1)
            Me.PanelEx1.Controls.Add(Me.cboTargetFitting)
            Me.PanelEx1.Controls.Add(Me.cboAttackerFitting)
            Me.PanelEx1.Controls.Add(Me.cboTargetPilot)
            Me.PanelEx1.Controls.Add(Me.cboAttackerPilot)
            Me.PanelEx1.Controls.Add(Me.tabStats)
            Me.PanelEx1.Controls.Add(Me.lblAttacker)
            Me.PanelEx1.Controls.Add(Me.lblAttackerFitting)
            Me.PanelEx1.Controls.Add(Me.lblAttackerPilot)
            Me.PanelEx1.Controls.Add(Me.lblTarget)
            Me.PanelEx1.Controls.Add(Me.lblTargetPilot)
            Me.PanelEx1.Controls.Add(Me.lblTargetFitting)
            Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
            Me.PanelEx1.Name = "PanelEx1"
            Me.PanelEx1.Size = New System.Drawing.Size(832, 575)
            Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.PanelEx1.Style.GradientAngle = 90
            Me.PanelEx1.TabIndex = 24
            '
            'GroupPanel1
            '
            Me.GroupPanel1.CanvasColor = System.Drawing.SystemColors.Control
            Me.GroupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
            Me.GroupPanel1.Controls.Add(Me.btnOptimalRange)
            Me.GroupPanel1.Controls.Add(Me.btnRangeVsHitChance)
            Me.GroupPanel1.Controls.Add(Me.nudTargetVel)
            Me.GroupPanel1.Controls.Add(Me.EveSpace1)
            Me.GroupPanel1.Controls.Add(Me.Label3)
            Me.GroupPanel1.Controls.Add(Me.nudVel)
            Me.GroupPanel1.Controls.Add(Me.lblRange)
            Me.GroupPanel1.Controls.Add(Me.radManualVelocity)
            Me.GroupPanel1.Controls.Add(Me.Label1)
            Me.GroupPanel1.Controls.Add(Me.Label2)
            Me.GroupPanel1.Controls.Add(Me.nudAttackerVel)
            Me.GroupPanel1.Controls.Add(Me.nudRange)
            Me.GroupPanel1.Controls.Add(Me.radMovableVel)
            Me.GroupPanel1.IsShadowEnabled = True
            Me.GroupPanel1.Location = New System.Drawing.Point(12, 63)
            Me.GroupPanel1.Name = "GroupPanel1"
            Me.GroupPanel1.Size = New System.Drawing.Size(362, 506)
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
            Me.GroupPanel1.TabIndex = 29
            Me.GroupPanel1.Text = "Spatial Configuration"
            '
            'btnOptimalRange
            '
            Me.btnOptimalRange.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnOptimalRange.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnOptimalRange.Location = New System.Drawing.Point(159, 359)
            Me.btnOptimalRange.Name = "btnOptimalRange"
            Me.btnOptimalRange.Size = New System.Drawing.Size(54, 21)
            Me.btnOptimalRange.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnOptimalRange.TabIndex = 29
            Me.btnOptimalRange.Text = "Optimal"
            '
            'btnRangeVsHitChance
            '
            Me.btnRangeVsHitChance.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnRangeVsHitChance.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnRangeVsHitChance.Location = New System.Drawing.Point(6, 458)
            Me.btnRangeVsHitChance.Name = "btnRangeVsHitChance"
            Me.btnRangeVsHitChance.Size = New System.Drawing.Size(169, 23)
            Me.btnRangeVsHitChance.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnRangeVsHitChance.TabIndex = 28
            Me.btnRangeVsHitChance.Text = "Range vs Hit Chance Graph"
            '
            'cboTargetFitting
            '
            Me.cboTargetFitting.DisplayMember = "Text"
            Me.cboTargetFitting.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboTargetFitting.FormattingEnabled = True
            Me.cboTargetFitting.ItemHeight = 15
            Me.cboTargetFitting.Location = New System.Drawing.Point(103, 36)
            Me.cboTargetFitting.Name = "cboTargetFitting"
            Me.cboTargetFitting.Size = New System.Drawing.Size(187, 21)
            Me.cboTargetFitting.Sorted = True
            Me.cboTargetFitting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboTargetFitting.TabIndex = 28
            '
            'cboAttackerFitting
            '
            Me.cboAttackerFitting.DisplayMember = "Text"
            Me.cboAttackerFitting.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboAttackerFitting.FormattingEnabled = True
            Me.cboAttackerFitting.ItemHeight = 15
            Me.cboAttackerFitting.Location = New System.Drawing.Point(103, 9)
            Me.cboAttackerFitting.Name = "cboAttackerFitting"
            Me.cboAttackerFitting.Size = New System.Drawing.Size(187, 21)
            Me.cboAttackerFitting.Sorted = True
            Me.cboAttackerFitting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboAttackerFitting.TabIndex = 27
            '
            'cboTargetPilot
            '
            Me.cboTargetPilot.DisplayMember = "Text"
            Me.cboTargetPilot.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboTargetPilot.FormattingEnabled = True
            Me.cboTargetPilot.ItemHeight = 15
            Me.cboTargetPilot.Location = New System.Drawing.Point(329, 36)
            Me.cboTargetPilot.Name = "cboTargetPilot"
            Me.cboTargetPilot.Size = New System.Drawing.Size(148, 21)
            Me.cboTargetPilot.Sorted = True
            Me.cboTargetPilot.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboTargetPilot.TabIndex = 26
            '
            'cboAttackerPilot
            '
            Me.cboAttackerPilot.DisplayMember = "Text"
            Me.cboAttackerPilot.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboAttackerPilot.FormattingEnabled = True
            Me.cboAttackerPilot.ItemHeight = 15
            Me.cboAttackerPilot.Location = New System.Drawing.Point(329, 9)
            Me.cboAttackerPilot.Name = "cboAttackerPilot"
            Me.cboAttackerPilot.Size = New System.Drawing.Size(148, 21)
            Me.cboAttackerPilot.Sorted = True
            Me.cboAttackerPilot.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboAttackerPilot.TabIndex = 25
            '
            'tabStats
            '
            Me.tabStats.BackColor = System.Drawing.Color.Transparent
            Me.tabStats.CanReorderTabs = True
            Me.tabStats.ColorScheme.TabBackground = System.Drawing.Color.Transparent
            Me.tabStats.ColorScheme.TabBackground2 = System.Drawing.Color.Transparent
            Me.tabStats.ColorScheme.TabItemBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(226, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(199, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(212, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(223, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer)), 1.0!)})
            Me.tabStats.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(235, Byte), Integer)), 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(168, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(89, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(141, Byte), Integer)), 1.0!)})
            Me.tabStats.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(New DevComponents.DotNetBar.BackgroundColorBlend() {New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.White, 0.0!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 0.45!), New DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer)), 1.0!)})
            Me.tabStats.Controls.Add(Me.TabControlPanel1)
            Me.tabStats.Controls.Add(Me.TabControlPanel2)
            Me.tabStats.Location = New System.Drawing.Point(380, 63)
            Me.tabStats.Name = "tabStats"
            Me.tabStats.SelectedTabFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.tabStats.SelectedTabIndex = 1
            Me.tabStats.Size = New System.Drawing.Size(447, 506)
            Me.tabStats.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document
            Me.tabStats.TabIndex = 24
            Me.tabStats.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
            Me.tabStats.Tabs.Add(Me.tabTurretStats)
            Me.tabStats.Tabs.Add(Me.tabMissileStats)
            Me.tabStats.Text = "TabControl1"
            '
            'TabControlPanel1
            '
            Me.TabControlPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.TabControlPanel1.Controls.Add(Me.lblTurretStats)
            Me.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel1.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel1.Name = "TabControlPanel1"
            Me.TabControlPanel1.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel1.Size = New System.Drawing.Size(447, 483)
            Me.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
            Me.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
            Me.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
            Me.TabControlPanel1.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel1.Style.GradientAngle = 90
            Me.TabControlPanel1.TabIndex = 1
            Me.TabControlPanel1.TabItem = Me.tabTurretStats
            '
            'lblTurretStats
            '
            Me.lblTurretStats.AutoSize = True
            Me.lblTurretStats.BackColor = System.Drawing.Color.Transparent
            '
            '
            '
            Me.lblTurretStats.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblTurretStats.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblTurretStats.Location = New System.Drawing.Point(1, 1)
            Me.lblTurretStats.Name = "lblTurretStats"
            Me.lblTurretStats.Size = New System.Drawing.Size(166, 16)
            Me.lblTurretStats.TabIndex = 0
            Me.lblTurretStats.Text = "Stats: Fittings and Pilots Required"
            '
            'tabTurretStats
            '
            Me.tabTurretStats.AttachedControl = Me.TabControlPanel1
            Me.tabTurretStats.Name = "tabTurretStats"
            Me.tabTurretStats.Text = "Turret Stats"
            '
            'TabControlPanel2
            '
            Me.TabControlPanel2.Controls.Add(Me.lblMissileStats)
            Me.TabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControlPanel2.Location = New System.Drawing.Point(0, 23)
            Me.TabControlPanel2.Name = "TabControlPanel2"
            Me.TabControlPanel2.Padding = New System.Windows.Forms.Padding(1)
            Me.TabControlPanel2.Size = New System.Drawing.Size(447, 483)
            Me.TabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
            Me.TabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer), CType(CType(195, Byte), Integer))
            Me.TabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.TabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
            Me.TabControlPanel2.Style.BorderSide = CType(((DevComponents.DotNetBar.eBorderSide.Left Or DevComponents.DotNetBar.eBorderSide.Right) _
                Or DevComponents.DotNetBar.eBorderSide.Bottom), DevComponents.DotNetBar.eBorderSide)
            Me.TabControlPanel2.Style.GradientAngle = 90
            Me.TabControlPanel2.TabIndex = 2
            Me.TabControlPanel2.TabItem = Me.tabMissileStats
            '
            'lblMissileStats
            '
            Me.lblMissileStats.AutoSize = True
            Me.lblMissileStats.BackColor = System.Drawing.Color.Transparent
            '
            '
            '
            Me.lblMissileStats.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblMissileStats.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblMissileStats.Location = New System.Drawing.Point(1, 1)
            Me.lblMissileStats.Name = "lblMissileStats"
            Me.lblMissileStats.Size = New System.Drawing.Size(166, 16)
            Me.lblMissileStats.TabIndex = 0
            Me.lblMissileStats.Text = "Stats: Fittings and Pilots Required"
            '
            'tabMissileStats
            '
            Me.tabMissileStats.AttachedControl = Me.TabControlPanel2
            Me.tabMissileStats.Name = "tabMissileStats"
            Me.tabMissileStats.Text = "Missile Stats"
            '
            'FrmDamageAnalysis
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(832, 575)
            Me.Controls.Add(Me.PanelEx1)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FrmDamageAnalysis"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "HQF Damage Analysis"
            CType(Me.nudVel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudRange, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudTargetVel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudAttackerVel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.PanelEx1.ResumeLayout(False)
            Me.PanelEx1.PerformLayout()
            Me.GroupPanel1.ResumeLayout(False)
            Me.GroupPanel1.PerformLayout()
            CType(Me.tabStats, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabStats.ResumeLayout(False)
            Me.TabControlPanel1.ResumeLayout(False)
            Me.TabControlPanel1.PerformLayout()
            Me.TabControlPanel2.ResumeLayout(False)
            Me.TabControlPanel2.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblAttackerFitting As System.Windows.Forms.Label
        Friend WithEvents lblAttackerPilot As System.Windows.Forms.Label
        Friend WithEvents lblAttacker As System.Windows.Forms.Label
        Friend WithEvents lblTarget As System.Windows.Forms.Label
        Friend WithEvents lblTargetFitting As System.Windows.Forms.Label
        Friend WithEvents lblTargetPilot As System.Windows.Forms.Label
        Friend WithEvents EveSpace1 As EveSpace
        Friend WithEvents nudVel As System.Windows.Forms.NumericUpDown
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents nudRange As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblRange As System.Windows.Forms.Label
        Friend WithEvents radManualVelocity As System.Windows.Forms.RadioButton
        Friend WithEvents radMovableVel As System.Windows.Forms.RadioButton
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents nudAttackerVel As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudTargetVel As System.Windows.Forms.NumericUpDown
        Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
        Friend WithEvents tabStats As DevComponents.DotNetBar.TabControl
        Friend WithEvents TabControlPanel1 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents tabTurretStats As DevComponents.DotNetBar.TabItem
        Friend WithEvents TabControlPanel2 As DevComponents.DotNetBar.TabControlPanel
        Friend WithEvents tabMissileStats As DevComponents.DotNetBar.TabItem
        Friend WithEvents GroupPanel1 As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents cboTargetFitting As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents cboAttackerFitting As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents cboTargetPilot As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents cboAttackerPilot As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents lblMissileStats As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblTurretStats As DevComponents.DotNetBar.LabelX
        Friend WithEvents btnOptimalRange As DevComponents.DotNetBar.ButtonX
        Friend WithEvents btnRangeVsHitChance As DevComponents.DotNetBar.ButtonX
    End Class
End Namespace