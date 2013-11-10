Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmQuickInventionChance
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
            Me.pnlQIC = New DevComponents.DotNetBar.PanelEx
            Me.lblMetaItem = New DevComponents.DotNetBar.LabelX
            Me.cboItemMetaLevel = New DevComponents.DotNetBar.Controls.ComboBoxEx
            Me.ComboItem35 = New DevComponents.Editors.ComboItem
            Me.ComboItem29 = New DevComponents.Editors.ComboItem
            Me.ComboItem30 = New DevComponents.Editors.ComboItem
            Me.ComboItem31 = New DevComponents.Editors.ComboItem
            Me.ComboItem32 = New DevComponents.Editors.ComboItem
            Me.ComboItem33 = New DevComponents.Editors.ComboItem
            Me.ComboItem34 = New DevComponents.Editors.ComboItem
            Me.ComboItem36 = New DevComponents.Editors.ComboItem
            Me.ComboItem37 = New DevComponents.Editors.ComboItem
            Me.lblInventionChance = New DevComponents.DotNetBar.LabelX
            Me.lblDecryptor = New DevComponents.DotNetBar.LabelX
            Me.cboDecryptor = New DevComponents.DotNetBar.Controls.ComboBoxEx
            Me.ComboItem23 = New DevComponents.Editors.ComboItem
            Me.ComboItem24 = New DevComponents.Editors.ComboItem
            Me.ComboItem25 = New DevComponents.Editors.ComboItem
            Me.ComboItem26 = New DevComponents.Editors.ComboItem
            Me.ComboItem27 = New DevComponents.Editors.ComboItem
            Me.ComboItem28 = New DevComponents.Editors.ComboItem
            Me.LabelX3 = New DevComponents.DotNetBar.LabelX
            Me.LabelX2 = New DevComponents.DotNetBar.LabelX
            Me.lblDecryptorSkill = New DevComponents.DotNetBar.LabelX
            Me.cboSkill2 = New DevComponents.DotNetBar.Controls.ComboBoxEx
            Me.ComboItem17 = New DevComponents.Editors.ComboItem
            Me.ComboItem18 = New DevComponents.Editors.ComboItem
            Me.ComboItem19 = New DevComponents.Editors.ComboItem
            Me.ComboItem20 = New DevComponents.Editors.ComboItem
            Me.ComboItem21 = New DevComponents.Editors.ComboItem
            Me.ComboItem22 = New DevComponents.Editors.ComboItem
            Me.cboSkill3 = New DevComponents.DotNetBar.Controls.ComboBoxEx
            Me.ComboItem11 = New DevComponents.Editors.ComboItem
            Me.ComboItem12 = New DevComponents.Editors.ComboItem
            Me.ComboItem13 = New DevComponents.Editors.ComboItem
            Me.ComboItem14 = New DevComponents.Editors.ComboItem
            Me.ComboItem15 = New DevComponents.Editors.ComboItem
            Me.ComboItem16 = New DevComponents.Editors.ComboItem
            Me.cboSkill1 = New DevComponents.DotNetBar.Controls.ComboBoxEx
            Me.ComboItem5 = New DevComponents.Editors.ComboItem
            Me.ComboItem6 = New DevComponents.Editors.ComboItem
            Me.ComboItem7 = New DevComponents.Editors.ComboItem
            Me.ComboItem8 = New DevComponents.Editors.ComboItem
            Me.ComboItem9 = New DevComponents.Editors.ComboItem
            Me.ComboItem10 = New DevComponents.Editors.ComboItem
            Me.lblBaseChance = New DevComponents.DotNetBar.LabelX
            Me.cboBaseChance = New DevComponents.DotNetBar.Controls.ComboBoxEx
            Me.ComboItem1 = New DevComponents.Editors.ComboItem
            Me.ComboItem2 = New DevComponents.Editors.ComboItem
            Me.ComboItem3 = New DevComponents.Editors.ComboItem
            Me.ComboItem4 = New DevComponents.Editors.ComboItem
            Me.gpProbability = New DevComponents.DotNetBar.Controls.GroupPanel
            Me.lblAttempts = New DevComponents.DotNetBar.LabelX
            Me.lblSuccess = New DevComponents.DotNetBar.LabelX
            Me.nudAttempts = New DevComponents.Editors.IntegerInput
            Me.nudSuccess = New DevComponents.Editors.IntegerInput
            Me.lblProbability = New DevComponents.DotNetBar.LabelX
            Me.pnlQIC.SuspendLayout()
            Me.gpProbability.SuspendLayout()
            CType(Me.nudAttempts, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudSuccess, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlQIC
            '
            Me.pnlQIC.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlQIC.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlQIC.Controls.Add(Me.gpProbability)
            Me.pnlQIC.Controls.Add(Me.lblMetaItem)
            Me.pnlQIC.Controls.Add(Me.cboItemMetaLevel)
            Me.pnlQIC.Controls.Add(Me.lblInventionChance)
            Me.pnlQIC.Controls.Add(Me.lblDecryptor)
            Me.pnlQIC.Controls.Add(Me.cboDecryptor)
            Me.pnlQIC.Controls.Add(Me.LabelX3)
            Me.pnlQIC.Controls.Add(Me.LabelX2)
            Me.pnlQIC.Controls.Add(Me.lblDecryptorSkill)
            Me.pnlQIC.Controls.Add(Me.cboSkill2)
            Me.pnlQIC.Controls.Add(Me.cboSkill3)
            Me.pnlQIC.Controls.Add(Me.cboSkill1)
            Me.pnlQIC.Controls.Add(Me.lblBaseChance)
            Me.pnlQIC.Controls.Add(Me.cboBaseChance)
            Me.pnlQIC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlQIC.Location = New System.Drawing.Point(0, 0)
            Me.pnlQIC.Name = "pnlQIC"
            Me.pnlQIC.Size = New System.Drawing.Size(294, 356)
            Me.pnlQIC.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlQIC.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlQIC.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlQIC.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlQIC.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlQIC.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlQIC.Style.GradientAngle = 90
            Me.pnlQIC.TabIndex = 0
            '
            'lblMetaItem
            '
            Me.lblMetaItem.AutoSize = True
            '
            '
            '
            Me.lblMetaItem.BackgroundStyle.Class = ""
            Me.lblMetaItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblMetaItem.Location = New System.Drawing.Point(12, 125)
            Me.lblMetaItem.Name = "lblMetaItem"
            Me.lblMetaItem.Size = New System.Drawing.Size(81, 16)
            Me.lblMetaItem.TabIndex = 12
            Me.lblMetaItem.Text = "Item Meta Level"
            '
            'cboItemMetaLevel
            '
            Me.cboItemMetaLevel.DisplayMember = "Text"
            Me.cboItemMetaLevel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboItemMetaLevel.FormattingEnabled = True
            Me.cboItemMetaLevel.ItemHeight = 15
            Me.cboItemMetaLevel.Items.AddRange(New Object() {Me.ComboItem35, Me.ComboItem29, Me.ComboItem30, Me.ComboItem31, Me.ComboItem32, Me.ComboItem33})
            Me.cboItemMetaLevel.Location = New System.Drawing.Point(109, 120)
            Me.cboItemMetaLevel.Name = "cboItemMetaLevel"
            Me.cboItemMetaLevel.Size = New System.Drawing.Size(173, 21)
            Me.cboItemMetaLevel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboItemMetaLevel.TabIndex = 11
            '
            'ComboItem35
            '
            Me.ComboItem35.Text = "None"
            '
            'ComboItem29
            '
            Me.ComboItem29.Text = "Meta Level 0"
            '
            'ComboItem30
            '
            Me.ComboItem30.Text = "Meta Level 1"
            '
            'ComboItem31
            '
            Me.ComboItem31.Text = "Meta Level 2"
            '
            'ComboItem32
            '
            Me.ComboItem32.Text = "Meta Level 3"
            '
            'ComboItem33
            '
            Me.ComboItem33.Text = "Meta Level 4"
            '
            'lblInventionChance
            '
            '
            '
            '
            Me.lblInventionChance.BackgroundStyle.Class = ""
            Me.lblInventionChance.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblInventionChance.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblInventionChance.Location = New System.Drawing.Point(12, 187)
            Me.lblInventionChance.Name = "lblInventionChance"
            Me.lblInventionChance.Size = New System.Drawing.Size(276, 23)
            Me.lblInventionChance.TabIndex = 10
            Me.lblInventionChance.Text = "Invention Chance: 0.00%"
            '
            'lblDecryptor
            '
            Me.lblDecryptor.AutoSize = True
            '
            '
            '
            Me.lblDecryptor.BackgroundStyle.Class = ""
            Me.lblDecryptor.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblDecryptor.Location = New System.Drawing.Point(12, 152)
            Me.lblDecryptor.Name = "lblDecryptor"
            Me.lblDecryptor.Size = New System.Drawing.Size(50, 16)
            Me.lblDecryptor.TabIndex = 9
            Me.lblDecryptor.Text = "Decryptor"
            '
            'cboDecryptor
            '
            Me.cboDecryptor.DisplayMember = "Text"
            Me.cboDecryptor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboDecryptor.FormattingEnabled = True
            Me.cboDecryptor.ItemHeight = 15
            Me.cboDecryptor.Items.AddRange(New Object() {Me.ComboItem23, Me.ComboItem24, Me.ComboItem37, Me.ComboItem25, Me.ComboItem26, Me.ComboItem27, Me.ComboItem34, Me.ComboItem28, Me.ComboItem36})
            Me.cboDecryptor.Location = New System.Drawing.Point(109, 147)
            Me.cboDecryptor.Name = "cboDecryptor"
            Me.cboDecryptor.Size = New System.Drawing.Size(173, 21)
            Me.cboDecryptor.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboDecryptor.TabIndex = 8
            '
            'ComboItem23
            '
            Me.ComboItem23.Text = "None (+0r, -4ME, -4PE)"
            '
            'ComboItem24
            '
            Me.ComboItem24.Text = "0.6x (+9r -6ME -3PE)"
            '
            'ComboItem25
            '
            Me.ComboItem25.Text = "1.0x (+2r -3ME +0PE)"
            '
            'ComboItem26
            '
            Me.ComboItem26.Text = "1.1x (+0r -1ME -1PE)"
            '
            'ComboItem27
            '
            Me.ComboItem27.Text = "1.2x (+1r -2ME +1PE)"
            '
            'ComboItem28
            '
            Me.ComboItem28.Text = "1.8x (+4r -5ME -2PE)"
            '
            'ComboItem34
            '
            Me.ComboItem34.Text = "1.5x (+3r +1ME -1PE)"
            '
            'ComboItem36
            '
            Me.ComboItem36.Text = "1.9x (+2r +1ME -1PE)"
            '
            'ComboItem37
            '
            Me.ComboItem37.Text = "0.9x (+7r +2ME +0PE)"
            '
            'LabelX3
            '
            Me.LabelX3.AutoSize = True
            '
            '
            '
            Me.LabelX3.BackgroundStyle.Class = ""
            Me.LabelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.LabelX3.Location = New System.Drawing.Point(12, 98)
            Me.LabelX3.Name = "LabelX3"
            Me.LabelX3.Size = New System.Drawing.Size(78, 16)
            Me.LabelX3.TabIndex = 7
            Me.LabelX3.Text = "Datacore 2 Skill"
            '
            'LabelX2
            '
            Me.LabelX2.AutoSize = True
            '
            '
            '
            Me.LabelX2.BackgroundStyle.Class = ""
            Me.LabelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.LabelX2.Location = New System.Drawing.Point(12, 71)
            Me.LabelX2.Name = "LabelX2"
            Me.LabelX2.Size = New System.Drawing.Size(78, 16)
            Me.LabelX2.TabIndex = 6
            Me.LabelX2.Text = "Datacore 1 Skill"
            '
            'lblDecryptorSkill
            '
            Me.lblDecryptorSkill.AutoSize = True
            '
            '
            '
            Me.lblDecryptorSkill.BackgroundStyle.Class = ""
            Me.lblDecryptorSkill.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblDecryptorSkill.Location = New System.Drawing.Point(12, 44)
            Me.lblDecryptorSkill.Name = "lblDecryptorSkill"
            Me.lblDecryptorSkill.Size = New System.Drawing.Size(76, 16)
            Me.lblDecryptorSkill.TabIndex = 5
            Me.lblDecryptorSkill.Text = "Encryption Skill"
            '
            'cboSkill2
            '
            Me.cboSkill2.DisplayMember = "Text"
            Me.cboSkill2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboSkill2.FormattingEnabled = True
            Me.cboSkill2.ItemHeight = 15
            Me.cboSkill2.Items.AddRange(New Object() {Me.ComboItem17, Me.ComboItem18, Me.ComboItem19, Me.ComboItem20, Me.ComboItem21, Me.ComboItem22})
            Me.cboSkill2.Location = New System.Drawing.Point(109, 66)
            Me.cboSkill2.Name = "cboSkill2"
            Me.cboSkill2.Size = New System.Drawing.Size(173, 21)
            Me.cboSkill2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboSkill2.TabIndex = 4
            '
            'ComboItem17
            '
            Me.ComboItem17.Text = "Level 0"
            '
            'ComboItem18
            '
            Me.ComboItem18.Text = "Level 1"
            '
            'ComboItem19
            '
            Me.ComboItem19.Text = "Level 2"
            '
            'ComboItem20
            '
            Me.ComboItem20.Text = "Level 3"
            '
            'ComboItem21
            '
            Me.ComboItem21.Text = "Level 4"
            '
            'ComboItem22
            '
            Me.ComboItem22.Text = "Level 5"
            '
            'cboSkill3
            '
            Me.cboSkill3.DisplayMember = "Text"
            Me.cboSkill3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboSkill3.FormattingEnabled = True
            Me.cboSkill3.ItemHeight = 15
            Me.cboSkill3.Items.AddRange(New Object() {Me.ComboItem11, Me.ComboItem12, Me.ComboItem13, Me.ComboItem14, Me.ComboItem15, Me.ComboItem16})
            Me.cboSkill3.Location = New System.Drawing.Point(109, 93)
            Me.cboSkill3.Name = "cboSkill3"
            Me.cboSkill3.Size = New System.Drawing.Size(173, 21)
            Me.cboSkill3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboSkill3.TabIndex = 3
            '
            'ComboItem11
            '
            Me.ComboItem11.Text = "Level 0"
            '
            'ComboItem12
            '
            Me.ComboItem12.Text = "Level 1"
            '
            'ComboItem13
            '
            Me.ComboItem13.Text = "Level 2"
            '
            'ComboItem14
            '
            Me.ComboItem14.Text = "Level 3"
            '
            'ComboItem15
            '
            Me.ComboItem15.Text = "Level 4"
            '
            'ComboItem16
            '
            Me.ComboItem16.Text = "Level 5"
            '
            'cboSkill1
            '
            Me.cboSkill1.DisplayMember = "Text"
            Me.cboSkill1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboSkill1.FormattingEnabled = True
            Me.cboSkill1.ItemHeight = 15
            Me.cboSkill1.Items.AddRange(New Object() {Me.ComboItem5, Me.ComboItem6, Me.ComboItem7, Me.ComboItem8, Me.ComboItem9, Me.ComboItem10})
            Me.cboSkill1.Location = New System.Drawing.Point(109, 39)
            Me.cboSkill1.Name = "cboSkill1"
            Me.cboSkill1.Size = New System.Drawing.Size(173, 21)
            Me.cboSkill1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboSkill1.TabIndex = 2
            '
            'ComboItem5
            '
            Me.ComboItem5.Text = "Level 0"
            '
            'ComboItem6
            '
            Me.ComboItem6.Text = "Level 1"
            '
            'ComboItem7
            '
            Me.ComboItem7.Text = "Level 2"
            '
            'ComboItem8
            '
            Me.ComboItem8.Text = "Level 3"
            '
            'ComboItem9
            '
            Me.ComboItem9.Text = "Level 4"
            '
            'ComboItem10
            '
            Me.ComboItem10.Text = "Level 5"
            '
            'lblBaseChance
            '
            Me.lblBaseChance.AutoSize = True
            '
            '
            '
            Me.lblBaseChance.BackgroundStyle.Class = ""
            Me.lblBaseChance.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblBaseChance.Location = New System.Drawing.Point(12, 17)
            Me.lblBaseChance.Name = "lblBaseChance"
            Me.lblBaseChance.Size = New System.Drawing.Size(64, 16)
            Me.lblBaseChance.TabIndex = 1
            Me.lblBaseChance.Text = "Base Chance"
            '
            'cboBaseChance
            '
            Me.cboBaseChance.DisplayMember = "Text"
            Me.cboBaseChance.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboBaseChance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboBaseChance.FormattingEnabled = True
            Me.cboBaseChance.ItemHeight = 15
            Me.cboBaseChance.Items.AddRange(New Object() {Me.ComboItem1, Me.ComboItem2, Me.ComboItem3, Me.ComboItem4})
            Me.cboBaseChance.Location = New System.Drawing.Point(109, 12)
            Me.cboBaseChance.Name = "cboBaseChance"
            Me.cboBaseChance.Size = New System.Drawing.Size(173, 21)
            Me.cboBaseChance.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboBaseChance.TabIndex = 0
            '
            'ComboItem1
            '
            Me.ComboItem1.Text = "20%"
            '
            'ComboItem2
            '
            Me.ComboItem2.Text = "25%"
            '
            'ComboItem3
            '
            Me.ComboItem3.Text = "30%"
            '
            'ComboItem4
            '
            Me.ComboItem4.Text = "40%"
            '
            'gpProbability
            '
            Me.gpProbability.CanvasColor = System.Drawing.SystemColors.Control
            Me.gpProbability.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
            Me.gpProbability.Controls.Add(Me.lblProbability)
            Me.gpProbability.Controls.Add(Me.nudSuccess)
            Me.gpProbability.Controls.Add(Me.nudAttempts)
            Me.gpProbability.Controls.Add(Me.lblSuccess)
            Me.gpProbability.Controls.Add(Me.lblAttempts)
            Me.gpProbability.Location = New System.Drawing.Point(12, 228)
            Me.gpProbability.Name = "gpProbability"
            Me.gpProbability.Size = New System.Drawing.Size(270, 116)
            '
            '
            '
            Me.gpProbability.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.gpProbability.Style.BackColorGradientAngle = 90
            Me.gpProbability.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.gpProbability.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpProbability.Style.BorderBottomWidth = 1
            Me.gpProbability.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.gpProbability.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpProbability.Style.BorderLeftWidth = 1
            Me.gpProbability.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpProbability.Style.BorderRightWidth = 1
            Me.gpProbability.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpProbability.Style.BorderTopWidth = 1
            Me.gpProbability.Style.Class = ""
            Me.gpProbability.Style.CornerDiameter = 4
            Me.gpProbability.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
            Me.gpProbability.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.gpProbability.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.gpProbability.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.gpProbability.StyleMouseDown.Class = ""
            Me.gpProbability.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.gpProbability.StyleMouseOver.Class = ""
            Me.gpProbability.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.gpProbability.TabIndex = 13
            Me.gpProbability.Text = "Cumulative Probability Estimate"
            '
            'lblAttempts
            '
            Me.lblAttempts.AutoSize = True
            '
            '
            '
            Me.lblAttempts.BackgroundStyle.Class = ""
            Me.lblAttempts.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblAttempts.Location = New System.Drawing.Point(6, 15)
            Me.lblAttempts.Name = "lblAttempts"
            Me.lblAttempts.Size = New System.Drawing.Size(78, 16)
            Me.lblAttempts.TabIndex = 0
            Me.lblAttempts.Text = "Total Attempts:"
            '
            'lblSuccess
            '
            Me.lblSuccess.AutoSize = True
            '
            '
            '
            Me.lblSuccess.BackgroundStyle.Class = ""
            Me.lblSuccess.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblSuccess.Location = New System.Drawing.Point(6, 42)
            Me.lblSuccess.Name = "lblSuccess"
            Me.lblSuccess.Size = New System.Drawing.Size(104, 16)
            Me.lblSuccess.TabIndex = 1
            Me.lblSuccess.Text = "Successful Attempts:"
            '
            'nudAttempts
            '
            '
            '
            '
            Me.nudAttempts.BackgroundStyle.Class = "DateTimeInputBackground"
            Me.nudAttempts.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.nudAttempts.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
            Me.nudAttempts.Location = New System.Drawing.Point(136, 12)
            Me.nudAttempts.MaxValue = 1000
            Me.nudAttempts.MinValue = 1
            Me.nudAttempts.Name = "nudAttempts"
            Me.nudAttempts.ShowUpDown = True
            Me.nudAttempts.Size = New System.Drawing.Size(121, 21)
            Me.nudAttempts.TabIndex = 2
            Me.nudAttempts.Value = 1
            '
            'nudSuccess
            '
            '
            '
            '
            Me.nudSuccess.BackgroundStyle.Class = "DateTimeInputBackground"
            Me.nudSuccess.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.nudSuccess.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
            Me.nudSuccess.Location = New System.Drawing.Point(136, 39)
            Me.nudSuccess.MaxValue = 1000
            Me.nudSuccess.MinValue = 0
            Me.nudSuccess.Name = "nudSuccess"
            Me.nudSuccess.ShowUpDown = True
            Me.nudSuccess.Size = New System.Drawing.Size(121, 21)
            Me.nudSuccess.TabIndex = 3
            Me.nudSuccess.Value = 1
            '
            'lblProbability
            '
            '
            '
            '
            Me.lblProbability.BackgroundStyle.Class = ""
            Me.lblProbability.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblProbability.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblProbability.Location = New System.Drawing.Point(6, 66)
            Me.lblProbability.Name = "lblProbability"
            Me.lblProbability.Size = New System.Drawing.Size(251, 23)
            Me.lblProbability.TabIndex = 11
            Me.lblProbability.Text = "Probability: 0.00%"
            '
            'frmQuickInventionChance
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(294, 356)
            Me.Controls.Add(Me.pnlQIC)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmQuickInventionChance"
            Me.ShowIcon = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Quick Invention Chance"
            Me.pnlQIC.ResumeLayout(False)
            Me.pnlQIC.PerformLayout()
            Me.gpProbability.ResumeLayout(False)
            Me.gpProbability.PerformLayout()
            CType(Me.nudAttempts, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudSuccess, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlQIC As DevComponents.DotNetBar.PanelEx
        Friend WithEvents cboBaseChance As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents ComboItem1 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem2 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem3 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem4 As DevComponents.Editors.ComboItem
        Friend WithEvents lblDecryptor As DevComponents.DotNetBar.LabelX
        Friend WithEvents cboDecryptor As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents LabelX3 As DevComponents.DotNetBar.LabelX
        Friend WithEvents LabelX2 As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblDecryptorSkill As DevComponents.DotNetBar.LabelX
        Friend WithEvents cboSkill2 As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents ComboItem17 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem18 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem19 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem20 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem21 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem22 As DevComponents.Editors.ComboItem
        Friend WithEvents cboSkill3 As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents ComboItem11 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem12 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem13 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem14 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem15 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem16 As DevComponents.Editors.ComboItem
        Friend WithEvents cboSkill1 As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents ComboItem5 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem6 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem7 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem8 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem9 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem10 As DevComponents.Editors.ComboItem
        Friend WithEvents lblBaseChance As DevComponents.DotNetBar.LabelX
        Friend WithEvents ComboItem23 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem24 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem25 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem26 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem27 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem28 As DevComponents.Editors.ComboItem
        Friend WithEvents lblInventionChance As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblMetaItem As DevComponents.DotNetBar.LabelX
        Friend WithEvents cboItemMetaLevel As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents ComboItem35 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem29 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem30 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem31 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem32 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem33 As DevComponents.Editors.ComboItem
        Friend WithEvents gpProbability As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents nudSuccess As DevComponents.Editors.IntegerInput
        Friend WithEvents nudAttempts As DevComponents.Editors.IntegerInput
        Friend WithEvents lblSuccess As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblAttempts As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblProbability As DevComponents.DotNetBar.LabelX
        Friend WithEvents ComboItem34 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem36 As DevComponents.Editors.ComboItem
        Friend WithEvents ComboItem37 As DevComponents.Editors.ComboItem
    End Class
End NameSpace