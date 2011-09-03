<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBPOreCalculator
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
        Me.lvwOwnedOre = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colOROwnedOre = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colORAmount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colORVolume = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colORSystem = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colORStation = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOROwner = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwExtraOre = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colEOExtraOre = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colEOOreUnits = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colEOOreVolume = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chkMercoxit = New System.Windows.Forms.CheckBox()
        Me.chkArkonor = New System.Windows.Forms.CheckBox()
        Me.chkBistot = New System.Windows.Forms.CheckBox()
        Me.chkCrokite = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkDarkOchre = New System.Windows.Forms.CheckBox()
        Me.chkSpodumain = New System.Windows.Forms.CheckBox()
        Me.chkHerbergite = New System.Windows.Forms.CheckBox()
        Me.chkGneiss = New System.Windows.Forms.CheckBox()
        Me.chkHemorphite = New System.Windows.Forms.CheckBox()
        Me.chkJaspet = New System.Windows.Forms.CheckBox()
        Me.chkKernite = New System.Windows.Forms.CheckBox()
        Me.chkOmber = New System.Windows.Forms.CheckBox()
        Me.chkPyroxeres = New System.Windows.Forms.CheckBox()
        Me.chkPlagioclase = New System.Windows.Forms.CheckBox()
        Me.chkScordite = New System.Windows.Forms.CheckBox()
        Me.chkVeldspar = New System.Windows.Forms.CheckBox()
        Me.lvwNeededVsMined = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colNvMNeededMinerals = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colNvMMineralsUnits1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colNvMMinedMinerals = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colNvMMineralsUnits2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblDurationLbl = New System.Windows.Forms.Label()
        Me.lblDuration = New System.Windows.Forms.Label()
        Me.pnlOreCalc = New DevComponents.DotNetBar.PanelEx()
        Me.chkMineAll = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.chkExtraOre = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.gpOreTypes = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.btnClose = New DevComponents.DotNetBar.ButtonX()
        Me.pnlOreCalc.SuspendLayout()
        Me.gpOreTypes.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvwOwnedOre
        '
        '
        '
        '
        Me.lvwOwnedOre.Border.Class = "ListViewBorder"
        Me.lvwOwnedOre.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwOwnedOre.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colOROwnedOre, Me.colORAmount, Me.colORVolume, Me.colORSystem, Me.colORStation, Me.colOROwner})
        Me.lvwOwnedOre.GridLines = True
        Me.lvwOwnedOre.Location = New System.Drawing.Point(10, 13)
        Me.lvwOwnedOre.Name = "lvwOwnedOre"
        Me.lvwOwnedOre.Size = New System.Drawing.Size(760, 132)
        Me.lvwOwnedOre.TabIndex = 3
        Me.lvwOwnedOre.UseCompatibleStateImageBehavior = False
        Me.lvwOwnedOre.View = System.Windows.Forms.View.Details
        '
        'colOROwnedOre
        '
        Me.colOROwnedOre.Text = "Owned ore"
        Me.colOROwnedOre.Width = 200
        '
        'colORAmount
        '
        Me.colORAmount.Text = "# of units"
        Me.colORAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colORAmount.Width = 75
        '
        'colORVolume
        '
        Me.colORVolume.Text = "Volume"
        Me.colORVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colORVolume.Width = 100
        '
        'colORSystem
        '
        Me.colORSystem.Text = "System"
        Me.colORSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colORSystem.Width = 100
        '
        'colORStation
        '
        Me.colORStation.Text = "Station"
        Me.colORStation.Width = 200
        '
        'colOROwner
        '
        Me.colOROwner.Text = "Owner"
        Me.colOROwner.Width = 75
        '
        'lvwExtraOre
        '
        '
        '
        '
        Me.lvwExtraOre.Border.Class = "ListViewBorder"
        Me.lvwExtraOre.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwExtraOre.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colEOExtraOre, Me.colEOOreUnits, Me.colEOOreVolume})
        Me.lvwExtraOre.GridLines = True
        Me.lvwExtraOre.Location = New System.Drawing.Point(356, 151)
        Me.lvwExtraOre.Name = "lvwExtraOre"
        Me.lvwExtraOre.Size = New System.Drawing.Size(414, 132)
        Me.lvwExtraOre.TabIndex = 4
        Me.lvwExtraOre.UseCompatibleStateImageBehavior = False
        Me.lvwExtraOre.View = System.Windows.Forms.View.Details
        '
        'colEOExtraOre
        '
        Me.colEOExtraOre.Text = "Extra ore needed"
        Me.colEOExtraOre.Width = 225
        '
        'colEOOreUnits
        '
        Me.colEOOreUnits.Text = "# of units"
        Me.colEOOreUnits.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colEOOreUnits.Width = 75
        '
        'colEOOreVolume
        '
        Me.colEOOreVolume.Text = "Volume"
        Me.colEOOreVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colEOOreVolume.Width = 100
        '
        'chkMercoxit
        '
        Me.chkMercoxit.AutoSize = True
        Me.chkMercoxit.Location = New System.Drawing.Point(264, 102)
        Me.chkMercoxit.Name = "chkMercoxit"
        Me.chkMercoxit.Size = New System.Drawing.Size(67, 17)
        Me.chkMercoxit.TabIndex = 16
        Me.chkMercoxit.Text = "Mercoxit"
        Me.chkMercoxit.UseVisualStyleBackColor = True
        '
        'chkArkonor
        '
        Me.chkArkonor.AutoSize = True
        Me.chkArkonor.Location = New System.Drawing.Point(264, 79)
        Me.chkArkonor.Name = "chkArkonor"
        Me.chkArkonor.Size = New System.Drawing.Size(64, 17)
        Me.chkArkonor.TabIndex = 15
        Me.chkArkonor.Text = "Arkonor"
        Me.chkArkonor.UseVisualStyleBackColor = True
        '
        'chkBistot
        '
        Me.chkBistot.AutoSize = True
        Me.chkBistot.Location = New System.Drawing.Point(264, 56)
        Me.chkBistot.Name = "chkBistot"
        Me.chkBistot.Size = New System.Drawing.Size(53, 17)
        Me.chkBistot.TabIndex = 14
        Me.chkBistot.Text = "Bistot"
        Me.chkBistot.UseVisualStyleBackColor = True
        '
        'chkCrokite
        '
        Me.chkCrokite.AutoSize = True
        Me.chkCrokite.Location = New System.Drawing.Point(264, 33)
        Me.chkCrokite.Name = "chkCrokite"
        Me.chkCrokite.Size = New System.Drawing.Size(60, 17)
        Me.chkCrokite.TabIndex = 13
        Me.chkCrokite.Text = "Crokite"
        Me.chkCrokite.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(204, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Check the ore types you have access to:"
        '
        'chkDarkOchre
        '
        Me.chkDarkOchre.AutoSize = True
        Me.chkDarkOchre.Location = New System.Drawing.Point(177, 102)
        Me.chkDarkOchre.Name = "chkDarkOchre"
        Me.chkDarkOchre.Size = New System.Drawing.Size(80, 17)
        Me.chkDarkOchre.TabIndex = 11
        Me.chkDarkOchre.Text = "Dark Ochre"
        Me.chkDarkOchre.UseVisualStyleBackColor = True
        '
        'chkSpodumain
        '
        Me.chkSpodumain.AutoSize = True
        Me.chkSpodumain.Location = New System.Drawing.Point(177, 79)
        Me.chkSpodumain.Name = "chkSpodumain"
        Me.chkSpodumain.Size = New System.Drawing.Size(78, 17)
        Me.chkSpodumain.TabIndex = 10
        Me.chkSpodumain.Text = "Spodumain"
        Me.chkSpodumain.UseVisualStyleBackColor = True
        '
        'chkHerbergite
        '
        Me.chkHerbergite.AutoSize = True
        Me.chkHerbergite.Location = New System.Drawing.Point(177, 56)
        Me.chkHerbergite.Name = "chkHerbergite"
        Me.chkHerbergite.Size = New System.Drawing.Size(77, 17)
        Me.chkHerbergite.TabIndex = 9
        Me.chkHerbergite.Text = "Herbergite"
        Me.chkHerbergite.UseVisualStyleBackColor = True
        '
        'chkGneiss
        '
        Me.chkGneiss.AutoSize = True
        Me.chkGneiss.Location = New System.Drawing.Point(177, 33)
        Me.chkGneiss.Name = "chkGneiss"
        Me.chkGneiss.Size = New System.Drawing.Size(57, 17)
        Me.chkGneiss.TabIndex = 8
        Me.chkGneiss.Text = "Gneiss"
        Me.chkGneiss.UseVisualStyleBackColor = True
        '
        'chkHemorphite
        '
        Me.chkHemorphite.AutoSize = True
        Me.chkHemorphite.Location = New System.Drawing.Point(90, 102)
        Me.chkHemorphite.Name = "chkHemorphite"
        Me.chkHemorphite.Size = New System.Drawing.Size(81, 17)
        Me.chkHemorphite.TabIndex = 7
        Me.chkHemorphite.Text = "Hemorphite"
        Me.chkHemorphite.UseVisualStyleBackColor = True
        '
        'chkJaspet
        '
        Me.chkJaspet.AutoSize = True
        Me.chkJaspet.Location = New System.Drawing.Point(90, 79)
        Me.chkJaspet.Name = "chkJaspet"
        Me.chkJaspet.Size = New System.Drawing.Size(58, 17)
        Me.chkJaspet.TabIndex = 6
        Me.chkJaspet.Text = "Jaspet"
        Me.chkJaspet.UseVisualStyleBackColor = True
        '
        'chkKernite
        '
        Me.chkKernite.AutoSize = True
        Me.chkKernite.Location = New System.Drawing.Point(90, 56)
        Me.chkKernite.Name = "chkKernite"
        Me.chkKernite.Size = New System.Drawing.Size(60, 17)
        Me.chkKernite.TabIndex = 5
        Me.chkKernite.Text = "Kernite"
        Me.chkKernite.UseVisualStyleBackColor = True
        '
        'chkOmber
        '
        Me.chkOmber.AutoSize = True
        Me.chkOmber.Location = New System.Drawing.Point(90, 33)
        Me.chkOmber.Name = "chkOmber"
        Me.chkOmber.Size = New System.Drawing.Size(58, 17)
        Me.chkOmber.TabIndex = 4
        Me.chkOmber.Text = "Omber"
        Me.chkOmber.UseVisualStyleBackColor = True
        '
        'chkPyroxeres
        '
        Me.chkPyroxeres.AutoSize = True
        Me.chkPyroxeres.Location = New System.Drawing.Point(3, 102)
        Me.chkPyroxeres.Name = "chkPyroxeres"
        Me.chkPyroxeres.Size = New System.Drawing.Size(75, 17)
        Me.chkPyroxeres.TabIndex = 3
        Me.chkPyroxeres.Text = "Pyroxeres"
        Me.chkPyroxeres.UseVisualStyleBackColor = True
        '
        'chkPlagioclase
        '
        Me.chkPlagioclase.AutoSize = True
        Me.chkPlagioclase.Location = New System.Drawing.Point(3, 79)
        Me.chkPlagioclase.Name = "chkPlagioclase"
        Me.chkPlagioclase.Size = New System.Drawing.Size(78, 17)
        Me.chkPlagioclase.TabIndex = 2
        Me.chkPlagioclase.Text = "Plagioclase"
        Me.chkPlagioclase.UseVisualStyleBackColor = True
        '
        'chkScordite
        '
        Me.chkScordite.AutoSize = True
        Me.chkScordite.Location = New System.Drawing.Point(3, 56)
        Me.chkScordite.Name = "chkScordite"
        Me.chkScordite.Size = New System.Drawing.Size(65, 17)
        Me.chkScordite.TabIndex = 1
        Me.chkScordite.Text = "Scordite"
        Me.chkScordite.UseVisualStyleBackColor = True
        '
        'chkVeldspar
        '
        Me.chkVeldspar.AutoSize = True
        Me.chkVeldspar.Location = New System.Drawing.Point(3, 33)
        Me.chkVeldspar.Name = "chkVeldspar"
        Me.chkVeldspar.Size = New System.Drawing.Size(67, 17)
        Me.chkVeldspar.TabIndex = 0
        Me.chkVeldspar.Text = "Veldspar"
        Me.chkVeldspar.UseVisualStyleBackColor = True
        '
        'lvwNeededVsMined
        '
        '
        '
        '
        Me.lvwNeededVsMined.Border.Class = "ListViewBorder"
        Me.lvwNeededVsMined.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwNeededVsMined.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colNvMNeededMinerals, Me.colNvMMineralsUnits1, Me.colNvMMinedMinerals, Me.colNvMMineralsUnits2})
        Me.lvwNeededVsMined.GridLines = True
        Me.lvwNeededVsMined.Location = New System.Drawing.Point(356, 306)
        Me.lvwNeededVsMined.Name = "lvwNeededVsMined"
        Me.lvwNeededVsMined.Size = New System.Drawing.Size(414, 132)
        Me.lvwNeededVsMined.TabIndex = 7
        Me.lvwNeededVsMined.UseCompatibleStateImageBehavior = False
        Me.lvwNeededVsMined.View = System.Windows.Forms.View.Details
        '
        'colNvMNeededMinerals
        '
        Me.colNvMNeededMinerals.Text = "Minerals needed"
        Me.colNvMNeededMinerals.Width = 125
        '
        'colNvMMineralsUnits1
        '
        Me.colNvMMineralsUnits1.Text = "# of units"
        Me.colNvMMineralsUnits1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colNvMMineralsUnits1.Width = 75
        '
        'colNvMMinedMinerals
        '
        Me.colNvMMinedMinerals.Text = "Owned/Mined Minerals"
        Me.colNvMMinedMinerals.Width = 125
        '
        'colNvMMineralsUnits2
        '
        Me.colNvMMineralsUnits2.Text = "# of units"
        Me.colNvMMineralsUnits2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colNvMMineralsUnits2.Width = 75
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(353, 290)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(174, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "When I refine the ore I (will) have:"
        '
        'lblDurationLbl
        '
        Me.lblDurationLbl.AutoSize = True
        Me.lblDurationLbl.Location = New System.Drawing.Point(10, 386)
        Me.lblDurationLbl.Name = "lblDurationLbl"
        Me.lblDurationLbl.Size = New System.Drawing.Size(185, 13)
        Me.lblDurationLbl.TabIndex = 10
        Me.lblDurationLbl.Text = "The extra time you'll need for mining:"
        '
        'lblDuration
        '
        Me.lblDuration.AutoSize = True
        Me.lblDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDuration.Location = New System.Drawing.Point(194, 386)
        Me.lblDuration.Name = "lblDuration"
        Me.lblDuration.Size = New System.Drawing.Size(83, 13)
        Me.lblDuration.TabIndex = 11
        Me.lblDuration.Text = "0D 0H 0M 0S"
        '
        'pnlOreCalc
        '
        Me.pnlOreCalc.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlOreCalc.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlOreCalc.Controls.Add(Me.chkMineAll)
        Me.pnlOreCalc.Controls.Add(Me.chkExtraOre)
        Me.pnlOreCalc.Controls.Add(Me.gpOreTypes)
        Me.pnlOreCalc.Controls.Add(Me.btnClose)
        Me.pnlOreCalc.Controls.Add(Me.lvwOwnedOre)
        Me.pnlOreCalc.Controls.Add(Me.lvwExtraOre)
        Me.pnlOreCalc.Controls.Add(Me.lblDuration)
        Me.pnlOreCalc.Controls.Add(Me.lvwNeededVsMined)
        Me.pnlOreCalc.Controls.Add(Me.lblDurationLbl)
        Me.pnlOreCalc.Controls.Add(Me.Label2)
        Me.pnlOreCalc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlOreCalc.Location = New System.Drawing.Point(0, 0)
        Me.pnlOreCalc.Name = "pnlOreCalc"
        Me.pnlOreCalc.Size = New System.Drawing.Size(780, 450)
        Me.pnlOreCalc.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlOreCalc.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlOreCalc.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlOreCalc.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlOreCalc.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlOreCalc.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlOreCalc.Style.GradientAngle = 90
        Me.pnlOreCalc.TabIndex = 14
        '
        'chkMineAll
        '
        Me.chkMineAll.AutoSize = True
        '
        '
        '
        Me.chkMineAll.BackgroundStyle.Class = ""
        Me.chkMineAll.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkMineAll.Location = New System.Drawing.Point(10, 357)
        Me.chkMineAll.Name = "chkMineAll"
        Me.chkMineAll.Size = New System.Drawing.Size(187, 16)
        Me.chkMineAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkMineAll.TabIndex = 17
        Me.chkMineAll.Text = "I want to mine all the needed Ore"
        '
        'chkExtraOre
        '
        Me.chkExtraOre.AutoSize = True
        '
        '
        '
        Me.chkExtraOre.BackgroundStyle.Class = ""
        Me.chkExtraOre.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkExtraOre.Location = New System.Drawing.Point(10, 151)
        Me.chkExtraOre.Name = "chkExtraOre"
        Me.chkExtraOre.Size = New System.Drawing.Size(166, 16)
        Me.chkExtraOre.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkExtraOre.TabIndex = 16
        Me.chkExtraOre.Text = "Tell me how extra Ore I need"
        '
        'gpOreTypes
        '
        Me.gpOreTypes.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpOreTypes.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpOreTypes.Controls.Add(Me.chkMercoxit)
        Me.gpOreTypes.Controls.Add(Me.Label1)
        Me.gpOreTypes.Controls.Add(Me.chkArkonor)
        Me.gpOreTypes.Controls.Add(Me.chkVeldspar)
        Me.gpOreTypes.Controls.Add(Me.chkBistot)
        Me.gpOreTypes.Controls.Add(Me.chkScordite)
        Me.gpOreTypes.Controls.Add(Me.chkCrokite)
        Me.gpOreTypes.Controls.Add(Me.chkPlagioclase)
        Me.gpOreTypes.Controls.Add(Me.chkPyroxeres)
        Me.gpOreTypes.Controls.Add(Me.chkDarkOchre)
        Me.gpOreTypes.Controls.Add(Me.chkOmber)
        Me.gpOreTypes.Controls.Add(Me.chkSpodumain)
        Me.gpOreTypes.Controls.Add(Me.chkKernite)
        Me.gpOreTypes.Controls.Add(Me.chkHerbergite)
        Me.gpOreTypes.Controls.Add(Me.chkJaspet)
        Me.gpOreTypes.Controls.Add(Me.chkGneiss)
        Me.gpOreTypes.Controls.Add(Me.chkHemorphite)
        Me.gpOreTypes.Location = New System.Drawing.Point(10, 174)
        Me.gpOreTypes.Name = "gpOreTypes"
        Me.gpOreTypes.Size = New System.Drawing.Size(340, 152)
        '
        '
        '
        Me.gpOreTypes.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpOreTypes.Style.BackColorGradientAngle = 90
        Me.gpOreTypes.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpOreTypes.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpOreTypes.Style.BorderBottomWidth = 1
        Me.gpOreTypes.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpOreTypes.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpOreTypes.Style.BorderLeftWidth = 1
        Me.gpOreTypes.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpOreTypes.Style.BorderRightWidth = 1
        Me.gpOreTypes.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpOreTypes.Style.BorderTopWidth = 1
        Me.gpOreTypes.Style.Class = ""
        Me.gpOreTypes.Style.CornerDiameter = 4
        Me.gpOreTypes.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpOreTypes.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpOreTypes.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpOreTypes.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpOreTypes.StyleMouseDown.Class = ""
        Me.gpOreTypes.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpOreTypes.StyleMouseOver.Class = ""
        Me.gpOreTypes.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpOreTypes.TabIndex = 15
        Me.gpOreTypes.Text = "Ore Type Availability"
        '
        'btnClose
        '
        Me.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnClose.Location = New System.Drawing.Point(16, 415)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnClose.TabIndex = 14
        Me.btnClose.Text = "Close"
        '
        'frmBPOreCalculator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(780, 450)
        Me.Controls.Add(Me.pnlOreCalc)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.Name = "frmBPOreCalculator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BPO Ore Calculator"
        Me.pnlOreCalc.ResumeLayout(False)
        Me.pnlOreCalc.PerformLayout()
        Me.gpOreTypes.ResumeLayout(False)
        Me.gpOreTypes.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvwOwnedOre As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colOROwnedOre As System.Windows.Forms.ColumnHeader
    Friend WithEvents colORAmount As System.Windows.Forms.ColumnHeader
    Friend WithEvents colORVolume As System.Windows.Forms.ColumnHeader
    Friend WithEvents colORSystem As System.Windows.Forms.ColumnHeader
    Friend WithEvents colORStation As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOROwner As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwExtraOre As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colEOExtraOre As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEOOreUnits As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEOOreVolume As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkHemorphite As System.Windows.Forms.CheckBox
    Friend WithEvents chkJaspet As System.Windows.Forms.CheckBox
    Friend WithEvents chkKernite As System.Windows.Forms.CheckBox
    Friend WithEvents chkOmber As System.Windows.Forms.CheckBox
    Friend WithEvents chkPyroxeres As System.Windows.Forms.CheckBox
    Friend WithEvents chkPlagioclase As System.Windows.Forms.CheckBox
    Friend WithEvents chkScordite As System.Windows.Forms.CheckBox
    Friend WithEvents chkVeldspar As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkDarkOchre As System.Windows.Forms.CheckBox
    Friend WithEvents chkSpodumain As System.Windows.Forms.CheckBox
    Friend WithEvents chkHerbergite As System.Windows.Forms.CheckBox
    Friend WithEvents chkGneiss As System.Windows.Forms.CheckBox
    Friend WithEvents chkCrokite As System.Windows.Forms.CheckBox
    Friend WithEvents lvwNeededVsMined As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colNvMNeededMinerals As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNvMMineralsUnits1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNvMMinedMinerals As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNvMMineralsUnits2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblDurationLbl As System.Windows.Forms.Label
    Friend WithEvents lblDuration As System.Windows.Forms.Label
    Friend WithEvents chkMercoxit As System.Windows.Forms.CheckBox
    Friend WithEvents chkArkonor As System.Windows.Forms.CheckBox
    Friend WithEvents chkBistot As System.Windows.Forms.CheckBox
    Friend WithEvents pnlOreCalc As DevComponents.DotNetBar.PanelEx
    Friend WithEvents chkMineAll As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents chkExtraOre As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents gpOreTypes As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnClose As DevComponents.DotNetBar.ButtonX
End Class
