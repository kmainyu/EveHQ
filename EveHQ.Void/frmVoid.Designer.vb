<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmVoid
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmVoid))
        Me.lblMaxJumpableMass = New System.Windows.Forms.Label()
        Me.lblMaxJumpableMassLbl = New System.Windows.Forms.Label()
        Me.lblMaxTotalMass = New System.Windows.Forms.Label()
        Me.lblMaxTotalMassLbl = New System.Windows.Forms.Label()
        Me.lblStabilityWindow = New System.Windows.Forms.Label()
        Me.lblStabilityWindowLbl = New System.Windows.Forms.Label()
        Me.lblTargetSystemClass = New System.Windows.Forms.Label()
        Me.lblTargetSystemClassLbl = New System.Windows.Forms.Label()
        Me.cboWHType = New System.Windows.Forms.ComboBox()
        Me.lblWHType = New System.Windows.Forms.Label()
        Me.lvwEffects = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblAnomalyName = New System.Windows.Forms.Label()
        Me.lblAnomalyNameLbl = New System.Windows.Forms.Label()
        Me.lblSystemClass = New System.Windows.Forms.Label()
        Me.lblSystemClassLbl = New System.Windows.Forms.Label()
        Me.cboWHSystem = New System.Windows.Forms.ComboBox()
        Me.lblLocus = New System.Windows.Forms.Label()
        Me.gpWHInfo = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.gpWHSystemInfo = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.gpWHInfo.SuspendLayout()
        Me.gpWHSystemInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblMaxJumpableMass
        '
        Me.lblMaxJumpableMass.AutoSize = True
        Me.lblMaxJumpableMass.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxJumpableMass.Location = New System.Drawing.Point(149, 131)
        Me.lblMaxJumpableMass.Name = "lblMaxJumpableMass"
        Me.lblMaxJumpableMass.Size = New System.Drawing.Size(23, 13)
        Me.lblMaxJumpableMass.TabIndex = 9
        Me.lblMaxJumpableMass.Text = "n/a"
        '
        'lblMaxJumpableMassLbl
        '
        Me.lblMaxJumpableMassLbl.AutoSize = True
        Me.lblMaxJumpableMassLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxJumpableMassLbl.Location = New System.Drawing.Point(13, 131)
        Me.lblMaxJumpableMassLbl.Name = "lblMaxJumpableMassLbl"
        Me.lblMaxJumpableMassLbl.Size = New System.Drawing.Size(130, 13)
        Me.lblMaxJumpableMassLbl.TabIndex = 8
        Me.lblMaxJumpableMassLbl.Text = "Maximum Jumpable Mass:"
        '
        'lblMaxTotalMass
        '
        Me.lblMaxTotalMass.AutoSize = True
        Me.lblMaxTotalMass.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxTotalMass.Location = New System.Drawing.Point(149, 102)
        Me.lblMaxTotalMass.Name = "lblMaxTotalMass"
        Me.lblMaxTotalMass.Size = New System.Drawing.Size(23, 13)
        Me.lblMaxTotalMass.TabIndex = 7
        Me.lblMaxTotalMass.Text = "n/a"
        '
        'lblMaxTotalMassLbl
        '
        Me.lblMaxTotalMassLbl.AutoSize = True
        Me.lblMaxTotalMassLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxTotalMassLbl.Location = New System.Drawing.Point(13, 102)
        Me.lblMaxTotalMassLbl.Name = "lblMaxTotalMassLbl"
        Me.lblMaxTotalMassLbl.Size = New System.Drawing.Size(109, 13)
        Me.lblMaxTotalMassLbl.TabIndex = 6
        Me.lblMaxTotalMassLbl.Text = "Maximum Total Mass:"
        '
        'lblStabilityWindow
        '
        Me.lblStabilityWindow.AutoSize = True
        Me.lblStabilityWindow.BackColor = System.Drawing.Color.Transparent
        Me.lblStabilityWindow.Location = New System.Drawing.Point(149, 73)
        Me.lblStabilityWindow.Name = "lblStabilityWindow"
        Me.lblStabilityWindow.Size = New System.Drawing.Size(23, 13)
        Me.lblStabilityWindow.TabIndex = 5
        Me.lblStabilityWindow.Text = "n/a"
        '
        'lblStabilityWindowLbl
        '
        Me.lblStabilityWindowLbl.AutoSize = True
        Me.lblStabilityWindowLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblStabilityWindowLbl.Location = New System.Drawing.Point(13, 73)
        Me.lblStabilityWindowLbl.Name = "lblStabilityWindowLbl"
        Me.lblStabilityWindowLbl.Size = New System.Drawing.Size(87, 13)
        Me.lblStabilityWindowLbl.TabIndex = 4
        Me.lblStabilityWindowLbl.Text = "Life Expectancy:"
        '
        'lblTargetSystemClass
        '
        Me.lblTargetSystemClass.AutoSize = True
        Me.lblTargetSystemClass.BackColor = System.Drawing.Color.Transparent
        Me.lblTargetSystemClass.Location = New System.Drawing.Point(149, 44)
        Me.lblTargetSystemClass.Name = "lblTargetSystemClass"
        Me.lblTargetSystemClass.Size = New System.Drawing.Size(23, 13)
        Me.lblTargetSystemClass.TabIndex = 3
        Me.lblTargetSystemClass.Text = "n/a"
        '
        'lblTargetSystemClassLbl
        '
        Me.lblTargetSystemClassLbl.AutoSize = True
        Me.lblTargetSystemClassLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblTargetSystemClassLbl.Location = New System.Drawing.Point(13, 44)
        Me.lblTargetSystemClassLbl.Name = "lblTargetSystemClassLbl"
        Me.lblTargetSystemClassLbl.Size = New System.Drawing.Size(109, 13)
        Me.lblTargetSystemClassLbl.TabIndex = 2
        Me.lblTargetSystemClassLbl.Text = "Target System Class:"
        '
        'cboWHType
        '
        Me.cboWHType.FormattingEnabled = True
        Me.cboWHType.Location = New System.Drawing.Point(102, 10)
        Me.cboWHType.Name = "cboWHType"
        Me.cboWHType.Size = New System.Drawing.Size(151, 21)
        Me.cboWHType.Sorted = True
        Me.cboWHType.TabIndex = 1
        '
        'lblWHType
        '
        Me.lblWHType.AutoSize = True
        Me.lblWHType.BackColor = System.Drawing.Color.Transparent
        Me.lblWHType.Location = New System.Drawing.Point(13, 13)
        Me.lblWHType.Name = "lblWHType"
        Me.lblWHType.Size = New System.Drawing.Size(82, 13)
        Me.lblWHType.TabIndex = 0
        Me.lblWHType.Text = "Wormhole Type"
        '
        'lvwEffects
        '
        Me.lvwEffects.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwEffects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvwEffects.FullRowSelect = True
        Me.lvwEffects.GridLines = True
        Me.lvwEffects.Location = New System.Drawing.Point(2, 98)
        Me.lvwEffects.Name = "lvwEffects"
        Me.lvwEffects.Size = New System.Drawing.Size(342, 268)
        Me.lvwEffects.TabIndex = 6
        Me.lvwEffects.UseCompatibleStateImageBehavior = False
        Me.lvwEffects.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Effect"
        Me.ColumnHeader1.Width = 225
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Value"
        Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader2.Width = 80
        '
        'lblAnomalyName
        '
        Me.lblAnomalyName.AutoSize = True
        Me.lblAnomalyName.BackColor = System.Drawing.Color.Transparent
        Me.lblAnomalyName.Location = New System.Drawing.Point(149, 73)
        Me.lblAnomalyName.Name = "lblAnomalyName"
        Me.lblAnomalyName.Size = New System.Drawing.Size(23, 13)
        Me.lblAnomalyName.TabIndex = 5
        Me.lblAnomalyName.Text = "n/a"
        '
        'lblAnomalyNameLbl
        '
        Me.lblAnomalyNameLbl.AutoSize = True
        Me.lblAnomalyNameLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblAnomalyNameLbl.Location = New System.Drawing.Point(13, 73)
        Me.lblAnomalyNameLbl.Name = "lblAnomalyNameLbl"
        Me.lblAnomalyNameLbl.Size = New System.Drawing.Size(82, 13)
        Me.lblAnomalyNameLbl.TabIndex = 4
        Me.lblAnomalyNameLbl.Text = "Anomaly Name:"
        '
        'lblSystemClass
        '
        Me.lblSystemClass.AutoSize = True
        Me.lblSystemClass.BackColor = System.Drawing.Color.Transparent
        Me.lblSystemClass.Location = New System.Drawing.Point(149, 44)
        Me.lblSystemClass.Name = "lblSystemClass"
        Me.lblSystemClass.Size = New System.Drawing.Size(23, 13)
        Me.lblSystemClass.TabIndex = 3
        Me.lblSystemClass.Text = "n/a"
        '
        'lblSystemClassLbl
        '
        Me.lblSystemClassLbl.AutoSize = True
        Me.lblSystemClassLbl.BackColor = System.Drawing.Color.Transparent
        Me.lblSystemClassLbl.Location = New System.Drawing.Point(13, 44)
        Me.lblSystemClassLbl.Name = "lblSystemClassLbl"
        Me.lblSystemClassLbl.Size = New System.Drawing.Size(109, 13)
        Me.lblSystemClassLbl.TabIndex = 2
        Me.lblSystemClassLbl.Text = "Target System Class:"
        '
        'cboWHSystem
        '
        Me.cboWHSystem.FormattingEnabled = True
        Me.cboWHSystem.Location = New System.Drawing.Point(102, 10)
        Me.cboWHSystem.Name = "cboWHSystem"
        Me.cboWHSystem.Size = New System.Drawing.Size(151, 21)
        Me.cboWHSystem.Sorted = True
        Me.cboWHSystem.TabIndex = 1
        '
        'lblLocus
        '
        Me.lblLocus.AutoSize = True
        Me.lblLocus.BackColor = System.Drawing.Color.Transparent
        Me.lblLocus.Location = New System.Drawing.Point(13, 13)
        Me.lblLocus.Name = "lblLocus"
        Me.lblLocus.Size = New System.Drawing.Size(83, 13)
        Me.lblLocus.TabIndex = 0
        Me.lblLocus.Text = "Locus Signature"
        '
        'gpWHInfo
        '
        Me.gpWHInfo.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpWHInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpWHInfo.Controls.Add(Me.lblMaxJumpableMass)
        Me.gpWHInfo.Controls.Add(Me.lblWHType)
        Me.gpWHInfo.Controls.Add(Me.lblMaxJumpableMassLbl)
        Me.gpWHInfo.Controls.Add(Me.cboWHType)
        Me.gpWHInfo.Controls.Add(Me.lblMaxTotalMass)
        Me.gpWHInfo.Controls.Add(Me.lblTargetSystemClassLbl)
        Me.gpWHInfo.Controls.Add(Me.lblMaxTotalMassLbl)
        Me.gpWHInfo.Controls.Add(Me.lblTargetSystemClass)
        Me.gpWHInfo.Controls.Add(Me.lblStabilityWindow)
        Me.gpWHInfo.Controls.Add(Me.lblStabilityWindowLbl)
        Me.gpWHInfo.IsShadowEnabled = True
        Me.gpWHInfo.Location = New System.Drawing.Point(12, 12)
        Me.gpWHInfo.Name = "gpWHInfo"
        Me.gpWHInfo.Size = New System.Drawing.Size(354, 185)
        '
        '
        '
        Me.gpWHInfo.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpWHInfo.Style.BackColorGradientAngle = 90
        Me.gpWHInfo.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpWHInfo.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpWHInfo.Style.BorderBottomWidth = 1
        Me.gpWHInfo.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpWHInfo.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpWHInfo.Style.BorderLeftWidth = 1
        Me.gpWHInfo.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpWHInfo.Style.BorderRightWidth = 1
        Me.gpWHInfo.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpWHInfo.Style.BorderTopWidth = 1
        Me.gpWHInfo.Style.Class = ""
        Me.gpWHInfo.Style.CornerDiameter = 4
        Me.gpWHInfo.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpWHInfo.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpWHInfo.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpWHInfo.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpWHInfo.StyleMouseDown.Class = ""
        Me.gpWHInfo.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpWHInfo.StyleMouseOver.Class = ""
        Me.gpWHInfo.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpWHInfo.TabIndex = 2
        Me.gpWHInfo.Text = "Wormhole Information"
        '
        'gpWHSystemInfo
        '
        Me.gpWHSystemInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gpWHSystemInfo.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpWHSystemInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpWHSystemInfo.Controls.Add(Me.lvwEffects)
        Me.gpWHSystemInfo.Controls.Add(Me.lblLocus)
        Me.gpWHSystemInfo.Controls.Add(Me.lblAnomalyName)
        Me.gpWHSystemInfo.Controls.Add(Me.cboWHSystem)
        Me.gpWHSystemInfo.Controls.Add(Me.lblAnomalyNameLbl)
        Me.gpWHSystemInfo.Controls.Add(Me.lblSystemClassLbl)
        Me.gpWHSystemInfo.Controls.Add(Me.lblSystemClass)
        Me.gpWHSystemInfo.IsShadowEnabled = True
        Me.gpWHSystemInfo.Location = New System.Drawing.Point(12, 213)
        Me.gpWHSystemInfo.Name = "gpWHSystemInfo"
        Me.gpWHSystemInfo.Size = New System.Drawing.Size(354, 392)
        '
        '
        '
        Me.gpWHSystemInfo.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpWHSystemInfo.Style.BackColorGradientAngle = 90
        Me.gpWHSystemInfo.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpWHSystemInfo.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpWHSystemInfo.Style.BorderBottomWidth = 1
        Me.gpWHSystemInfo.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpWHSystemInfo.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpWHSystemInfo.Style.BorderLeftWidth = 1
        Me.gpWHSystemInfo.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpWHSystemInfo.Style.BorderRightWidth = 1
        Me.gpWHSystemInfo.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpWHSystemInfo.Style.BorderTopWidth = 1
        Me.gpWHSystemInfo.Style.Class = ""
        Me.gpWHSystemInfo.Style.CornerDiameter = 4
        Me.gpWHSystemInfo.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpWHSystemInfo.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpWHSystemInfo.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpWHSystemInfo.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpWHSystemInfo.StyleMouseDown.Class = ""
        Me.gpWHSystemInfo.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpWHSystemInfo.StyleMouseOver.Class = ""
        Me.gpWHSystemInfo.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpWHSystemInfo.TabIndex = 3
        Me.gpWHSystemInfo.Text = "W-Space System Information"
        '
        'frmVoid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1014, 617)
        Me.Controls.Add(Me.gpWHSystemInfo)
        Me.Controls.Add(Me.gpWHInfo)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmVoid"
        Me.Text = "EveHQ Void"
        Me.gpWHInfo.ResumeLayout(False)
        Me.gpWHInfo.PerformLayout()
        Me.gpWHSystemInfo.ResumeLayout(False)
        Me.gpWHSystemInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTargetSystemClassLbl As System.Windows.Forms.Label
    Friend WithEvents cboWHType As System.Windows.Forms.ComboBox
    Friend WithEvents lblWHType As System.Windows.Forms.Label
    Friend WithEvents lblMaxJumpableMass As System.Windows.Forms.Label
    Friend WithEvents lblMaxJumpableMassLbl As System.Windows.Forms.Label
    Friend WithEvents lblMaxTotalMass As System.Windows.Forms.Label
    Friend WithEvents lblMaxTotalMassLbl As System.Windows.Forms.Label
    Friend WithEvents lblStabilityWindow As System.Windows.Forms.Label
    Friend WithEvents lblStabilityWindowLbl As System.Windows.Forms.Label
    Friend WithEvents lblTargetSystemClass As System.Windows.Forms.Label
    Friend WithEvents lblAnomalyName As System.Windows.Forms.Label
    Friend WithEvents lblAnomalyNameLbl As System.Windows.Forms.Label
    Friend WithEvents lblSystemClass As System.Windows.Forms.Label
    Friend WithEvents lblSystemClassLbl As System.Windows.Forms.Label
    Friend WithEvents cboWHSystem As System.Windows.Forms.ComboBox
    Friend WithEvents lblLocus As System.Windows.Forms.Label
    Friend WithEvents lvwEffects As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents gpWHInfo As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents gpWHSystemInfo As DevComponents.DotNetBar.Controls.GroupPanel
End Class
