<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShipWidget
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShipWidget))
        Me.pnlShipWidget = New DevComponents.DotNetBar.PanelEx()
        Me.SwitchButton3 = New DevComponents.DotNetBar.Controls.SwitchButton()
        Me.pbGangLinks = New System.Windows.Forms.PictureBox()
        Me.btnBooster = New DevComponents.DotNetBar.ButtonX()
        Me.btnSetSquadBooster = New DevComponents.DotNetBar.ButtonItem()
        Me.btnSetWingBooster = New DevComponents.DotNetBar.ButtonItem()
        Me.btnSetFleetBooster = New DevComponents.DotNetBar.ButtonItem()
        Me.lblFleetPosition = New DevComponents.DotNetBar.LabelX()
        Me.SwitchButton2 = New DevComponents.DotNetBar.Controls.SwitchButton()
        Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
        Me.pbInfo = New System.Windows.Forms.PictureBox()
        Me.pbBooster = New System.Windows.Forms.PictureBox()
        Me.pbImplants = New System.Windows.Forms.PictureBox()
        Me.pbPilot = New System.Windows.Forms.PictureBox()
        Me.ctxPilot = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.pbFitting = New System.Windows.Forms.PictureBox()
        Me.ctxFitting = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.pnlModules = New DevComponents.DotNetBar.ExpandablePanel()
        Me.pnlHeader = New DevComponents.DotNetBar.PanelEx()
        Me.pbSkillsStability = New System.Windows.Forms.PictureBox()
        Me.pbCapStability = New System.Windows.Forms.PictureBox()
        Me.pbPGStability = New System.Windows.Forms.PictureBox()
        Me.pbCPUStability = New System.Windows.Forms.PictureBox()
        Me.pbRemove = New System.Windows.Forms.PictureBox()
        Me.STT = New DevComponents.DotNetBar.SuperTooltip()
        Me.pnlShipWidget.SuspendLayout()
        CType(Me.pbGangLinks, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbBooster, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbImplants, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbFitting, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeader.SuspendLayout()
        CType(Me.pbSkillsStability, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCapStability, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPGStability, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCPUStability, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbRemove, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlShipWidget
        '
        Me.pnlShipWidget.AllowDrop = True
        Me.pnlShipWidget.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlShipWidget.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlShipWidget.Controls.Add(Me.SwitchButton3)
        Me.pnlShipWidget.Controls.Add(Me.pbGangLinks)
        Me.pnlShipWidget.Controls.Add(Me.btnBooster)
        Me.pnlShipWidget.Controls.Add(Me.lblFleetPosition)
        Me.pnlShipWidget.Controls.Add(Me.SwitchButton2)
        Me.pnlShipWidget.Controls.Add(Me.SwitchButton1)
        Me.pnlShipWidget.Controls.Add(Me.pbInfo)
        Me.pnlShipWidget.Controls.Add(Me.pbBooster)
        Me.pnlShipWidget.Controls.Add(Me.pbImplants)
        Me.pnlShipWidget.Controls.Add(Me.pbPilot)
        Me.pnlShipWidget.Controls.Add(Me.pbFitting)
        Me.pnlShipWidget.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlShipWidget.Location = New System.Drawing.Point(0, 18)
        Me.pnlShipWidget.Name = "pnlShipWidget"
        Me.pnlShipWidget.Size = New System.Drawing.Size(210, 56)
        Me.pnlShipWidget.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlShipWidget.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlShipWidget.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlShipWidget.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlShipWidget.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlShipWidget.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlShipWidget.Style.GradientAngle = 90
        Me.pnlShipWidget.TabIndex = 0
        '
        'SwitchButton3
        '
        '
        '
        '
        Me.SwitchButton3.BackgroundStyle.Class = ""
        Me.SwitchButton3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.SwitchButton3.FocusCuesEnabled = False
        Me.SwitchButton3.Location = New System.Drawing.Point(140, 40)
        Me.SwitchButton3.Name = "SwitchButton3"
        Me.SwitchButton3.OffBackColor = System.Drawing.Color.Red
        Me.SwitchButton3.OffText = ""
        Me.SwitchButton3.OnBackColor = System.Drawing.Color.Lime
        Me.SwitchButton3.OnText = ""
        Me.SwitchButton3.Size = New System.Drawing.Size(32, 10)
        Me.SwitchButton3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.SwitchButton3.SwitchWidth = 16
        Me.SwitchButton3.TabIndex = 10
        Me.SwitchButton3.Value = True
        '
        'pbGangLinks
        '
        Me.pbGangLinks.Image = Global.EveHQ.HQF.My.Resources.Resources.imgGanglink
        Me.pbGangLinks.Location = New System.Drawing.Point(140, 2)
        Me.pbGangLinks.Name = "pbGangLinks"
        Me.pbGangLinks.Size = New System.Drawing.Size(32, 32)
        Me.pbGangLinks.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbGangLinks.TabIndex = 9
        Me.pbGangLinks.TabStop = False
        '
        'btnBooster
        '
        Me.btnBooster.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnBooster.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnBooster.FocusCuesEnabled = False
        Me.btnBooster.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBooster.Location = New System.Drawing.Point(36, 37)
        Me.btnBooster.Name = "btnBooster"
        Me.btnBooster.Size = New System.Drawing.Size(32, 13)
        Me.btnBooster.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnBooster.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnSetSquadBooster, Me.btnSetWingBooster, Me.btnSetFleetBooster})
        Me.btnBooster.TabIndex = 8
        Me.btnBooster.Text = "-"
        '
        'btnSetSquadBooster
        '
        Me.btnSetSquadBooster.Name = "btnSetSquadBooster"
        Me.btnSetSquadBooster.Text = "Set as Squad Booster"
        '
        'btnSetWingBooster
        '
        Me.btnSetWingBooster.Name = "btnSetWingBooster"
        Me.btnSetWingBooster.Text = "Set as Wing Booster"
        '
        'btnSetFleetBooster
        '
        Me.btnSetFleetBooster.Name = "btnSetFleetBooster"
        Me.btnSetFleetBooster.Text = "Set as Fleet Booster"
        '
        'lblFleetPosition
        '
        '
        '
        '
        Me.lblFleetPosition.BackgroundStyle.Class = ""
        Me.lblFleetPosition.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblFleetPosition.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFleetPosition.Location = New System.Drawing.Point(2, 37)
        Me.lblFleetPosition.Name = "lblFleetPosition"
        Me.lblFleetPosition.Size = New System.Drawing.Size(32, 13)
        Me.lblFleetPosition.TabIndex = 7
        Me.lblFleetPosition.Text = "W5 S5"
        Me.lblFleetPosition.TextAlignment = System.Drawing.StringAlignment.Center
        '
        'SwitchButton2
        '
        '
        '
        '
        Me.SwitchButton2.BackgroundStyle.Class = ""
        Me.SwitchButton2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.SwitchButton2.FocusCuesEnabled = False
        Me.SwitchButton2.Location = New System.Drawing.Point(104, 40)
        Me.SwitchButton2.Name = "SwitchButton2"
        Me.SwitchButton2.OffBackColor = System.Drawing.Color.Red
        Me.SwitchButton2.OffText = ""
        Me.SwitchButton2.OnBackColor = System.Drawing.Color.Lime
        Me.SwitchButton2.OnText = ""
        Me.SwitchButton2.Size = New System.Drawing.Size(32, 10)
        Me.SwitchButton2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.SwitchButton2.SwitchWidth = 16
        Me.SwitchButton2.TabIndex = 6
        Me.SwitchButton2.Value = True
        '
        'SwitchButton1
        '
        '
        '
        '
        Me.SwitchButton1.BackgroundStyle.Class = ""
        Me.SwitchButton1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.SwitchButton1.FocusCuesEnabled = False
        Me.SwitchButton1.Location = New System.Drawing.Point(70, 40)
        Me.SwitchButton1.Name = "SwitchButton1"
        Me.SwitchButton1.OffBackColor = System.Drawing.Color.Red
        Me.SwitchButton1.OffText = ""
        Me.SwitchButton1.OnBackColor = System.Drawing.Color.Lime
        Me.SwitchButton1.OnText = ""
        Me.SwitchButton1.Size = New System.Drawing.Size(32, 10)
        Me.SwitchButton1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.SwitchButton1.SwitchWidth = 16
        Me.SwitchButton1.TabIndex = 5
        Me.SwitchButton1.Value = True
        '
        'pbInfo
        '
        Me.pbInfo.Image = Global.EveHQ.HQF.My.Resources.Resources.imgInfo2
        Me.pbInfo.Location = New System.Drawing.Point(176, 2)
        Me.pbInfo.Name = "pbInfo"
        Me.pbInfo.Size = New System.Drawing.Size(32, 32)
        Me.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbInfo.TabIndex = 4
        Me.pbInfo.TabStop = False
        '
        'pbBooster
        '
        Me.pbBooster.Image = CType(resources.GetObject("pbBooster.Image"), System.Drawing.Image)
        Me.pbBooster.Location = New System.Drawing.Point(104, 2)
        Me.pbBooster.Name = "pbBooster"
        Me.pbBooster.Size = New System.Drawing.Size(32, 32)
        Me.pbBooster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbBooster.TabIndex = 3
        Me.pbBooster.TabStop = False
        '
        'pbImplants
        '
        Me.pbImplants.Enabled = False
        Me.pbImplants.Image = CType(resources.GetObject("pbImplants.Image"), System.Drawing.Image)
        Me.pbImplants.Location = New System.Drawing.Point(70, 2)
        Me.pbImplants.Name = "pbImplants"
        Me.pbImplants.Size = New System.Drawing.Size(32, 32)
        Me.pbImplants.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbImplants.TabIndex = 2
        Me.pbImplants.TabStop = False
        '
        'pbPilot
        '
        Me.pbPilot.ContextMenuStrip = Me.ctxPilot
        Me.pbPilot.Image = CType(resources.GetObject("pbPilot.Image"), System.Drawing.Image)
        Me.pbPilot.Location = New System.Drawing.Point(36, 2)
        Me.pbPilot.Name = "pbPilot"
        Me.pbPilot.Size = New System.Drawing.Size(32, 32)
        Me.pbPilot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbPilot.TabIndex = 1
        Me.pbPilot.TabStop = False
        '
        'ctxPilot
        '
        Me.ctxPilot.Name = "ctxPilot"
        Me.ctxPilot.Size = New System.Drawing.Size(61, 4)
        '
        'pbFitting
        '
        Me.pbFitting.ContextMenuStrip = Me.ctxFitting
        Me.pbFitting.Image = CType(resources.GetObject("pbFitting.Image"), System.Drawing.Image)
        Me.pbFitting.Location = New System.Drawing.Point(2, 2)
        Me.pbFitting.Name = "pbFitting"
        Me.pbFitting.Size = New System.Drawing.Size(32, 32)
        Me.pbFitting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbFitting.TabIndex = 0
        Me.pbFitting.TabStop = False
        '
        'ctxFitting
        '
        Me.ctxFitting.Name = "ctxFitting"
        Me.ctxFitting.Size = New System.Drawing.Size(61, 4)
        '
        'pnlModules
        '
        Me.pnlModules.AllowDrop = True
        Me.pnlModules.AnimationTime = 0
        Me.pnlModules.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlModules.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlModules.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlModules.Expanded = False
        Me.pnlModules.ExpandedBounds = New System.Drawing.Rectangle(0, -76, 210, 166)
        Me.pnlModules.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0, Byte))
        Me.pnlModules.Location = New System.Drawing.Point(0, 74)
        Me.pnlModules.Name = "pnlModules"
        Me.pnlModules.Size = New System.Drawing.Size(210, 16)
        Me.pnlModules.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlModules.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlModules.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlModules.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlModules.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.pnlModules.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.pnlModules.Style.GradientAngle = 90
        Me.pnlModules.TabIndex = 12
        Me.pnlModules.TitleHeight = 16
        Me.pnlModules.TitleStyle.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlModules.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlModules.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlModules.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner
        Me.pnlModules.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlModules.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlModules.TitleStyle.GradientAngle = 90
        Me.pnlModules.TitleText = "Remote Module List"
        '
        'pnlHeader
        '
        Me.pnlHeader.AllowDrop = True
        Me.pnlHeader.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlHeader.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlHeader.Controls.Add(Me.pbSkillsStability)
        Me.pnlHeader.Controls.Add(Me.pbCapStability)
        Me.pnlHeader.Controls.Add(Me.pbPGStability)
        Me.pnlHeader.Controls.Add(Me.pbCPUStability)
        Me.pnlHeader.Controls.Add(Me.pbRemove)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(210, 18)
        Me.pnlHeader.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlHeader.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlHeader.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlHeader.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlHeader.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlHeader.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlHeader.Style.GradientAngle = 90
        Me.pnlHeader.TabIndex = 1
        '
        'pbSkillsStability
        '
        Me.pbSkillsStability.Image = CType(resources.GetObject("pbSkillsStability.Image"), System.Drawing.Image)
        Me.pbSkillsStability.Location = New System.Drawing.Point(50, 1)
        Me.pbSkillsStability.Name = "pbSkillsStability"
        Me.pbSkillsStability.Size = New System.Drawing.Size(16, 16)
        Me.pbSkillsStability.TabIndex = 9
        Me.pbSkillsStability.TabStop = False
        '
        'pbCapStability
        '
        Me.pbCapStability.Image = CType(resources.GetObject("pbCapStability.Image"), System.Drawing.Image)
        Me.pbCapStability.Location = New System.Drawing.Point(34, 1)
        Me.pbCapStability.Name = "pbCapStability"
        Me.pbCapStability.Size = New System.Drawing.Size(16, 16)
        Me.pbCapStability.TabIndex = 8
        Me.pbCapStability.TabStop = False
        '
        'pbPGStability
        '
        Me.pbPGStability.Image = CType(resources.GetObject("pbPGStability.Image"), System.Drawing.Image)
        Me.pbPGStability.Location = New System.Drawing.Point(18, 1)
        Me.pbPGStability.Name = "pbPGStability"
        Me.pbPGStability.Size = New System.Drawing.Size(16, 16)
        Me.pbPGStability.TabIndex = 7
        Me.pbPGStability.TabStop = False
        '
        'pbCPUStability
        '
        Me.pbCPUStability.Image = CType(resources.GetObject("pbCPUStability.Image"), System.Drawing.Image)
        Me.pbCPUStability.Location = New System.Drawing.Point(2, 1)
        Me.pbCPUStability.Name = "pbCPUStability"
        Me.pbCPUStability.Size = New System.Drawing.Size(16, 16)
        Me.pbCPUStability.TabIndex = 6
        Me.pbCPUStability.TabStop = False
        '
        'pbRemove
        '
        Me.pbRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbRemove.Image = Global.EveHQ.HQF.My.Resources.Resources.cross_small
        Me.pbRemove.Location = New System.Drawing.Point(189, 1)
        Me.pbRemove.Name = "pbRemove"
        Me.pbRemove.Size = New System.Drawing.Size(16, 16)
        Me.pbRemove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbRemove.TabIndex = 5
        Me.pbRemove.TabStop = False
        '
        'STT
        '
        Me.STT.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        '
        'ShipWidget
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlShipWidget)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlModules)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ShipWidget"
        Me.Size = New System.Drawing.Size(210, 90)
        Me.pnlShipWidget.ResumeLayout(False)
        CType(Me.pbGangLinks, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbBooster, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbImplants, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbFitting, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeader.ResumeLayout(False)
        CType(Me.pbSkillsStability, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCapStability, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPGStability, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCPUStability, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbRemove, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlShipWidget As DevComponents.DotNetBar.PanelEx
    Friend WithEvents pbPilot As System.Windows.Forms.PictureBox
    Friend WithEvents pbFitting As System.Windows.Forms.PictureBox
    Friend WithEvents pnlHeader As DevComponents.DotNetBar.PanelEx
    Friend WithEvents pbInfo As System.Windows.Forms.PictureBox
    Friend WithEvents pbBooster As System.Windows.Forms.PictureBox
    Friend WithEvents pbImplants As System.Windows.Forms.PictureBox
    Friend WithEvents pbRemove As System.Windows.Forms.PictureBox
    Friend WithEvents SwitchButton2 As DevComponents.DotNetBar.Controls.SwitchButton
    Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton
    Friend WithEvents btnBooster As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnSetSquadBooster As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnSetWingBooster As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents btnSetFleetBooster As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents lblFleetPosition As DevComponents.DotNetBar.LabelX
    Friend WithEvents pbGangLinks As System.Windows.Forms.PictureBox
    Friend WithEvents SwitchButton3 As DevComponents.DotNetBar.Controls.SwitchButton
    Friend WithEvents pbSkillsStability As System.Windows.Forms.PictureBox
    Friend WithEvents pbCapStability As System.Windows.Forms.PictureBox
    Friend WithEvents pbPGStability As System.Windows.Forms.PictureBox
    Friend WithEvents pbCPUStability As System.Windows.Forms.PictureBox
    Friend WithEvents pnlModules As DevComponents.DotNetBar.ExpandablePanel
    Friend WithEvents STT As DevComponents.DotNetBar.SuperTooltip
    Friend WithEvents ctxFitting As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ctxPilot As System.Windows.Forms.ContextMenuStrip

End Class
