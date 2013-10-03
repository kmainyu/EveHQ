Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmCharCreate
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCharCreate))
            Me.btnAddPilot = New System.Windows.Forms.Button()
            Me.lvwSkills = New System.Windows.Forms.ListView()
            Me.colSkill = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.colLevel = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.colSP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.lblSP = New System.Windows.Forms.Label()
            Me.cboRace = New System.Windows.Forms.ComboBox()
            Me.lblStep1 = New System.Windows.Forms.Label()
            Me.txtCharName = New System.Windows.Forms.TextBox()
            Me.lblCharID = New System.Windows.Forms.Label()
            Me.lblCharacterIDLabel = New System.Windows.Forms.Label()
            Me.lblAttTotal = New System.Windows.Forms.Label()
            Me.lblAttributesTotalLabel = New System.Windows.Forms.Label()
            Me.nudW = New System.Windows.Forms.NumericUpDown()
            Me.nudP = New System.Windows.Forms.NumericUpDown()
            Me.nudM = New System.Windows.Forms.NumericUpDown()
            Me.nudI = New System.Windows.Forms.NumericUpDown()
            Me.nudC = New System.Windows.Forms.NumericUpDown()
            Me.lblW = New System.Windows.Forms.Label()
            Me.lblP = New System.Windows.Forms.Label()
            Me.lblM = New System.Windows.Forms.Label()
            Me.lblI = New System.Windows.Forms.Label()
            Me.lblC = New System.Windows.Forms.Label()
            Me.grpSelection = New DevComponents.DotNetBar.Controls.GroupPanel()
            Me.lblSelectChar = New System.Windows.Forms.Label()
            Me.lblSelectAttributes = New System.Windows.Forms.Label()
            Me.grpSkills = New DevComponents.DotNetBar.Controls.GroupPanel()
            CType(Me.nudW, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudP, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudM, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudI, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudC, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpSelection.SuspendLayout()
            Me.grpSkills.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnAddPilot
            '
            Me.btnAddPilot.Enabled = False
            Me.btnAddPilot.Location = New System.Drawing.Point(15, 333)
            Me.btnAddPilot.Name = "btnAddPilot"
            Me.btnAddPilot.Size = New System.Drawing.Size(180, 23)
            Me.btnAddPilot.TabIndex = 24
            Me.btnAddPilot.Text = "Add Character to EveHQ Pilots"
            Me.btnAddPilot.UseVisualStyleBackColor = True
            '
            'lvwSkills
            '
            Me.lvwSkills.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lvwSkills.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSkill, Me.colLevel, Me.colSP})
            Me.lvwSkills.Location = New System.Drawing.Point(3, 8)
            Me.lvwSkills.Name = "lvwSkills"
            Me.lvwSkills.Size = New System.Drawing.Size(328, 329)
            Me.lvwSkills.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvwSkills.TabIndex = 0
            Me.lvwSkills.UseCompatibleStateImageBehavior = False
            Me.lvwSkills.View = System.Windows.Forms.View.Details
            '
            'colSkill
            '
            Me.colSkill.Text = "Skill Name"
            Me.colSkill.Width = 200
            '
            'colLevel
            '
            Me.colLevel.Text = "Level"
            Me.colLevel.Width = 50
            '
            'colSP
            '
            Me.colSP.Text = "Skillpoints"
            Me.colSP.Width = 70
            '
            'lblSP
            '
            Me.lblSP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblSP.AutoSize = True
            Me.lblSP.BackColor = System.Drawing.Color.Transparent
            Me.lblSP.Location = New System.Drawing.Point(3, 340)
            Me.lblSP.Name = "lblSP"
            Me.lblSP.Size = New System.Drawing.Size(76, 13)
            Me.lblSP.TabIndex = 5
            Me.lblSP.Text = "Skillpoints: n/a"
            '
            'cboRace
            '
            Me.cboRace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboRace.FormattingEnabled = True
            Me.cboRace.Location = New System.Drawing.Point(173, 5)
            Me.cboRace.Name = "cboRace"
            Me.cboRace.Size = New System.Drawing.Size(183, 21)
            Me.cboRace.Sorted = True
            Me.cboRace.TabIndex = 13
            '
            'lblStep1
            '
            Me.lblStep1.AutoSize = True
            Me.lblStep1.BackColor = System.Drawing.Color.Transparent
            Me.lblStep1.Location = New System.Drawing.Point(12, 8)
            Me.lblStep1.Name = "lblStep1"
            Me.lblStep1.Size = New System.Drawing.Size(108, 13)
            Me.lblStep1.TabIndex = 12
            Me.lblStep1.Text = "Step 1: Choose Race"
            '
            'txtCharName
            '
            Me.txtCharName.Enabled = False
            Me.txtCharName.Location = New System.Drawing.Point(173, 244)
            Me.txtCharName.Name = "txtCharName"
            Me.txtCharName.Size = New System.Drawing.Size(183, 21)
            Me.txtCharName.TabIndex = 42
            '
            'lblCharID
            '
            Me.lblCharID.AutoSize = True
            Me.lblCharID.BackColor = System.Drawing.Color.Transparent
            Me.lblCharID.Location = New System.Drawing.Point(190, 279)
            Me.lblCharID.Name = "lblCharID"
            Me.lblCharID.Size = New System.Drawing.Size(0, 13)
            Me.lblCharID.TabIndex = 41
            '
            'lblCharacterIDLabel
            '
            Me.lblCharacterIDLabel.AutoSize = True
            Me.lblCharacterIDLabel.BackColor = System.Drawing.Color.Transparent
            Me.lblCharacterIDLabel.Location = New System.Drawing.Point(47, 279)
            Me.lblCharacterIDLabel.Name = "lblCharacterIDLabel"
            Me.lblCharacterIDLabel.Size = New System.Drawing.Size(73, 13)
            Me.lblCharacterIDLabel.TabIndex = 39
            Me.lblCharacterIDLabel.Text = "Character ID:"
            '
            'lblAttTotal
            '
            Me.lblAttTotal.AutoSize = True
            Me.lblAttTotal.BackColor = System.Drawing.Color.Transparent
            Me.lblAttTotal.Location = New System.Drawing.Point(190, 197)
            Me.lblAttTotal.Name = "lblAttTotal"
            Me.lblAttTotal.Size = New System.Drawing.Size(19, 13)
            Me.lblAttTotal.TabIndex = 38
            Me.lblAttTotal.Text = "39"
            '
            'lblAttributesTotalLabel
            '
            Me.lblAttributesTotalLabel.AutoSize = True
            Me.lblAttributesTotalLabel.BackColor = System.Drawing.Color.Transparent
            Me.lblAttributesTotalLabel.Location = New System.Drawing.Point(45, 197)
            Me.lblAttributesTotalLabel.Name = "lblAttributesTotalLabel"
            Me.lblAttributesTotalLabel.Size = New System.Drawing.Size(86, 13)
            Me.lblAttributesTotalLabel.TabIndex = 37
            Me.lblAttributesTotalLabel.Text = "Attributes Total:"
            '
            'nudW
            '
            Me.nudW.Enabled = False
            Me.nudW.Location = New System.Drawing.Point(173, 165)
            Me.nudW.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
            Me.nudW.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
            Me.nudW.Name = "nudW"
            Me.nudW.Size = New System.Drawing.Size(70, 21)
            Me.nudW.TabIndex = 34
            Me.nudW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudW.Value = New Decimal(New Integer() {8, 0, 0, 0})
            '
            'nudP
            '
            Me.nudP.Enabled = False
            Me.nudP.Location = New System.Drawing.Point(173, 139)
            Me.nudP.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
            Me.nudP.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
            Me.nudP.Name = "nudP"
            Me.nudP.Size = New System.Drawing.Size(70, 21)
            Me.nudP.TabIndex = 33
            Me.nudP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudP.Value = New Decimal(New Integer() {8, 0, 0, 0})
            '
            'nudM
            '
            Me.nudM.Enabled = False
            Me.nudM.Location = New System.Drawing.Point(173, 113)
            Me.nudM.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
            Me.nudM.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
            Me.nudM.Name = "nudM"
            Me.nudM.Size = New System.Drawing.Size(70, 21)
            Me.nudM.TabIndex = 32
            Me.nudM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudM.Value = New Decimal(New Integer() {8, 0, 0, 0})
            '
            'nudI
            '
            Me.nudI.Enabled = False
            Me.nudI.Location = New System.Drawing.Point(173, 87)
            Me.nudI.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
            Me.nudI.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
            Me.nudI.Name = "nudI"
            Me.nudI.Size = New System.Drawing.Size(70, 21)
            Me.nudI.TabIndex = 31
            Me.nudI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudI.Value = New Decimal(New Integer() {8, 0, 0, 0})
            '
            'nudC
            '
            Me.nudC.Enabled = False
            Me.nudC.Location = New System.Drawing.Point(173, 61)
            Me.nudC.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
            Me.nudC.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
            Me.nudC.Name = "nudC"
            Me.nudC.Size = New System.Drawing.Size(70, 21)
            Me.nudC.TabIndex = 30
            Me.nudC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudC.Value = New Decimal(New Integer() {7, 0, 0, 0})
            '
            'lblW
            '
            Me.lblW.AutoSize = True
            Me.lblW.BackColor = System.Drawing.Color.Transparent
            Me.lblW.Location = New System.Drawing.Point(45, 167)
            Me.lblW.Name = "lblW"
            Me.lblW.Size = New System.Drawing.Size(98, 13)
            Me.lblW.TabIndex = 29
            Me.lblW.Text = "Willpower (5 to 15)"
            '
            'lblP
            '
            Me.lblP.AutoSize = True
            Me.lblP.BackColor = System.Drawing.Color.Transparent
            Me.lblP.Location = New System.Drawing.Point(45, 141)
            Me.lblP.Name = "lblP"
            Me.lblP.Size = New System.Drawing.Size(103, 13)
            Me.lblP.TabIndex = 28
            Me.lblP.Text = "Perception (5 to 15)"
            '
            'lblM
            '
            Me.lblM.AutoSize = True
            Me.lblM.BackColor = System.Drawing.Color.Transparent
            Me.lblM.Location = New System.Drawing.Point(45, 115)
            Me.lblM.Name = "lblM"
            Me.lblM.Size = New System.Drawing.Size(90, 13)
            Me.lblM.TabIndex = 27
            Me.lblM.Text = "Memory (5 to 15)"
            '
            'lblI
            '
            Me.lblI.AutoSize = True
            Me.lblI.BackColor = System.Drawing.Color.Transparent
            Me.lblI.Location = New System.Drawing.Point(45, 89)
            Me.lblI.Name = "lblI"
            Me.lblI.Size = New System.Drawing.Size(107, 13)
            Me.lblI.TabIndex = 26
            Me.lblI.Text = "Intelligence (5 to 15)"
            '
            'lblC
            '
            Me.lblC.AutoSize = True
            Me.lblC.BackColor = System.Drawing.Color.Transparent
            Me.lblC.Location = New System.Drawing.Point(45, 63)
            Me.lblC.Name = "lblC"
            Me.lblC.Size = New System.Drawing.Size(96, 13)
            Me.lblC.TabIndex = 25
            Me.lblC.Text = "Charisma (5 to 15)"
            '
            'grpSelection
            '
            Me.grpSelection.CanvasColor = System.Drawing.SystemColors.Control
            Me.grpSelection.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
            Me.grpSelection.Controls.Add(Me.lblSelectChar)
            Me.grpSelection.Controls.Add(Me.lblCharID)
            Me.grpSelection.Controls.Add(Me.txtCharName)
            Me.grpSelection.Controls.Add(Me.btnAddPilot)
            Me.grpSelection.Controls.Add(Me.lblCharacterIDLabel)
            Me.grpSelection.Controls.Add(Me.lblSelectAttributes)
            Me.grpSelection.Controls.Add(Me.lblStep1)
            Me.grpSelection.Controls.Add(Me.cboRace)
            Me.grpSelection.Controls.Add(Me.lblAttTotal)
            Me.grpSelection.Controls.Add(Me.lblAttributesTotalLabel)
            Me.grpSelection.Controls.Add(Me.nudW)
            Me.grpSelection.Controls.Add(Me.lblC)
            Me.grpSelection.Controls.Add(Me.nudP)
            Me.grpSelection.Controls.Add(Me.lblM)
            Me.grpSelection.Controls.Add(Me.lblP)
            Me.grpSelection.Controls.Add(Me.nudM)
            Me.grpSelection.Controls.Add(Me.lblI)
            Me.grpSelection.Controls.Add(Me.lblW)
            Me.grpSelection.Controls.Add(Me.nudI)
            Me.grpSelection.Controls.Add(Me.nudC)
            Me.grpSelection.Location = New System.Drawing.Point(12, 12)
            Me.grpSelection.Name = "grpSelection"
            Me.grpSelection.Size = New System.Drawing.Size(379, 385)
            '
            '
            '
            Me.grpSelection.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.grpSelection.Style.BackColorGradientAngle = 90
            Me.grpSelection.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.grpSelection.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.grpSelection.Style.BorderBottomWidth = 1
            Me.grpSelection.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.grpSelection.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.grpSelection.Style.BorderLeftWidth = 1
            Me.grpSelection.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.grpSelection.Style.BorderRightWidth = 1
            Me.grpSelection.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.grpSelection.Style.BorderTopWidth = 1
            Me.grpSelection.Style.CornerDiameter = 4
            Me.grpSelection.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
            Me.grpSelection.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.grpSelection.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.grpSelection.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.grpSelection.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.grpSelection.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.grpSelection.TabIndex = 43
            Me.grpSelection.Text = "Character Selection"
            '
            'lblSelectChar
            '
            Me.lblSelectChar.AutoSize = True
            Me.lblSelectChar.BackColor = System.Drawing.Color.Transparent
            Me.lblSelectChar.Enabled = False
            Me.lblSelectChar.Location = New System.Drawing.Point(12, 247)
            Me.lblSelectChar.Name = "lblSelectChar"
            Me.lblSelectChar.Size = New System.Drawing.Size(155, 13)
            Me.lblSelectChar.TabIndex = 39
            Me.lblSelectChar.Text = "Step 3: Select Character Name"
            '
            'lblSelectAttributes
            '
            Me.lblSelectAttributes.AutoSize = True
            Me.lblSelectAttributes.BackColor = System.Drawing.Color.Transparent
            Me.lblSelectAttributes.Enabled = False
            Me.lblSelectAttributes.Location = New System.Drawing.Point(12, 38)
            Me.lblSelectAttributes.Name = "lblSelectAttributes"
            Me.lblSelectAttributes.Size = New System.Drawing.Size(129, 13)
            Me.lblSelectAttributes.TabIndex = 18
            Me.lblSelectAttributes.Text = "Step 2: Amend Attributes"
            '
            'grpSkills
            '
            Me.grpSkills.CanvasColor = System.Drawing.SystemColors.Control
            Me.grpSkills.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
            Me.grpSkills.Controls.Add(Me.lblSP)
            Me.grpSkills.Controls.Add(Me.lvwSkills)
            Me.grpSkills.Location = New System.Drawing.Point(407, 12)
            Me.grpSkills.Name = "grpSkills"
            Me.grpSkills.Size = New System.Drawing.Size(335, 385)
            '
            '
            '
            Me.grpSkills.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.grpSkills.Style.BackColorGradientAngle = 90
            Me.grpSkills.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.grpSkills.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.grpSkills.Style.BorderBottomWidth = 1
            Me.grpSkills.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.grpSkills.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.grpSkills.Style.BorderLeftWidth = 1
            Me.grpSkills.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.grpSkills.Style.BorderRightWidth = 1
            Me.grpSkills.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.grpSkills.Style.BorderTopWidth = 1
            Me.grpSkills.Style.CornerDiameter = 4
            Me.grpSkills.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
            Me.grpSkills.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.grpSkills.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.grpSkills.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.grpSkills.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.grpSkills.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.grpSkills.TabIndex = 44
            Me.grpSkills.Text = "Character Skills"
            '
            'FrmCharCreate
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(754, 407)
            Me.Controls.Add(Me.grpSkills)
            Me.Controls.Add(Me.grpSelection)
            Me.DoubleBuffered = True
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "FrmCharCreate"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Character Creation Tool"
            CType(Me.nudW, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudP, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudM, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudI, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudC, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpSelection.ResumeLayout(False)
            Me.grpSelection.PerformLayout()
            Me.grpSkills.ResumeLayout(False)
            Me.grpSkills.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lvwSkills As System.Windows.Forms.ListView
        Friend WithEvents colSkill As System.Windows.Forms.ColumnHeader
        Friend WithEvents colLevel As System.Windows.Forms.ColumnHeader
        Friend WithEvents colSP As System.Windows.Forms.ColumnHeader
        Friend WithEvents lblSP As System.Windows.Forms.Label
        Friend WithEvents cboRace As System.Windows.Forms.ComboBox
        Friend WithEvents lblStep1 As System.Windows.Forms.Label
        Friend WithEvents btnAddPilot As System.Windows.Forms.Button
        Friend WithEvents txtCharName As System.Windows.Forms.TextBox
        Friend WithEvents lblCharID As System.Windows.Forms.Label
        Friend WithEvents lblCharacterIDLabel As System.Windows.Forms.Label
        Friend WithEvents lblAttTotal As System.Windows.Forms.Label
        Friend WithEvents lblAttributesTotalLabel As System.Windows.Forms.Label
        Friend WithEvents nudW As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudP As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudM As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudI As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudC As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblW As System.Windows.Forms.Label
        Friend WithEvents lblP As System.Windows.Forms.Label
        Friend WithEvents lblM As System.Windows.Forms.Label
        Friend WithEvents lblI As System.Windows.Forms.Label
        Friend WithEvents lblC As System.Windows.Forms.Label
        Friend WithEvents grpSelection As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents lblSelectAttributes As System.Windows.Forms.Label
        Friend WithEvents lblSelectChar As System.Windows.Forms.Label
        Friend WithEvents grpSkills As DevComponents.DotNetBar.Controls.GroupPanel
    End Class
End Namespace