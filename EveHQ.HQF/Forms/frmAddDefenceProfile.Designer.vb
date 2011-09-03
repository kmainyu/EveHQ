<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddDefenceProfile
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
        Me.lblProfileName = New System.Windows.Forms.Label
        Me.txtProfileName = New System.Windows.Forms.TextBox
        Me.lblProfileType = New System.Windows.Forms.Label
        Me.lblResistInfo = New System.Windows.Forms.Label
        Me.lblPilotName = New System.Windows.Forms.Label
        Me.lblFittingName = New System.Windows.Forms.Label
        Me.txtSEx = New System.Windows.Forms.TextBox
        Me.lblEXDamage = New System.Windows.Forms.Label
        Me.txtSKi = New System.Windows.Forms.TextBox
        Me.lblKIDamage = New System.Windows.Forms.Label
        Me.txtSTh = New System.Windows.Forms.TextBox
        Me.lblTHDamage = New System.Windows.Forms.Label
        Me.txtSEM = New System.Windows.Forms.TextBox
        Me.lblEmDamage = New System.Windows.Forms.Label
        Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx
        Me.cboProfileType = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.ComboItem1 = New DevComponents.Editors.ComboItem
        Me.ComboItem2 = New DevComponents.Editors.ComboItem
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX
        Me.gpManualProfile = New DevComponents.DotNetBar.Controls.GroupPanel
        Me.cboPilotName = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.ComboItem8 = New DevComponents.Editors.ComboItem
        Me.ComboItem7 = New DevComponents.Editors.ComboItem
        Me.ComboItem9 = New DevComponents.Editors.ComboItem
        Me.cboFittingName = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.ComboItem5 = New DevComponents.Editors.ComboItem
        Me.ComboItem4 = New DevComponents.Editors.ComboItem
        Me.ComboItem6 = New DevComponents.Editors.ComboItem
        Me.pbHullDefence = New System.Windows.Forms.PictureBox
        Me.pbArmorDefence = New System.Windows.Forms.PictureBox
        Me.pbShieldDefence = New System.Windows.Forms.PictureBox
        Me.txtAEM = New System.Windows.Forms.TextBox
        Me.txtATh = New System.Windows.Forms.TextBox
        Me.txtAKi = New System.Windows.Forms.TextBox
        Me.txtAEx = New System.Windows.Forms.TextBox
        Me.txtHEM = New System.Windows.Forms.TextBox
        Me.txtHTh = New System.Windows.Forms.TextBox
        Me.txtHKi = New System.Windows.Forms.TextBox
        Me.txtHEx = New System.Windows.Forms.TextBox
        Me.PanelEx1.SuspendLayout()
        Me.gpManualProfile.SuspendLayout()
        CType(Me.pbHullDefence, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorDefence, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldDefence, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblProfileName
        '
        Me.lblProfileName.AutoSize = True
        Me.lblProfileName.Location = New System.Drawing.Point(12, 9)
        Me.lblProfileName.Name = "lblProfileName"
        Me.lblProfileName.Size = New System.Drawing.Size(71, 13)
        Me.lblProfileName.TabIndex = 0
        Me.lblProfileName.Text = "Profile Name:"
        '
        'txtProfileName
        '
        Me.txtProfileName.Location = New System.Drawing.Point(106, 6)
        Me.txtProfileName.Name = "txtProfileName"
        Me.txtProfileName.Size = New System.Drawing.Size(212, 21)
        Me.txtProfileName.TabIndex = 1
        '
        'lblProfileType
        '
        Me.lblProfileType.AutoSize = True
        Me.lblProfileType.Location = New System.Drawing.Point(12, 35)
        Me.lblProfileType.Name = "lblProfileType"
        Me.lblProfileType.Size = New System.Drawing.Size(68, 13)
        Me.lblProfileType.TabIndex = 2
        Me.lblProfileType.Text = "Profile Type:"
        '
        'lblResistInfo
        '
        Me.lblResistInfo.AutoSize = True
        Me.lblResistInfo.Location = New System.Drawing.Point(15, 77)
        Me.lblResistInfo.Name = "lblResistInfo"
        Me.lblResistInfo.Size = New System.Drawing.Size(98, 13)
        Me.lblResistInfo.TabIndex = 15
        Me.lblResistInfo.Text = "Resistance Details:"
        '
        'lblPilotName
        '
        Me.lblPilotName.AutoSize = True
        Me.lblPilotName.Location = New System.Drawing.Point(15, 39)
        Me.lblPilotName.Name = "lblPilotName"
        Me.lblPilotName.Size = New System.Drawing.Size(61, 13)
        Me.lblPilotName.TabIndex = 12
        Me.lblPilotName.Text = "Pilot Name:"
        '
        'lblFittingName
        '
        Me.lblFittingName.AutoSize = True
        Me.lblFittingName.Location = New System.Drawing.Point(15, 12)
        Me.lblFittingName.Name = "lblFittingName"
        Me.lblFittingName.Size = New System.Drawing.Size(71, 13)
        Me.lblFittingName.TabIndex = 10
        Me.lblFittingName.Text = "Fitting Name:"
        '
        'txtSEx
        '
        Me.txtSEx.Location = New System.Drawing.Point(132, 149)
        Me.txtSEx.Name = "txtSEx"
        Me.txtSEx.Size = New System.Drawing.Size(75, 21)
        Me.txtSEx.TabIndex = 9
        Me.txtSEx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblEXDamage
        '
        Me.lblEXDamage.AutoSize = True
        Me.lblEXDamage.Location = New System.Drawing.Point(20, 152)
        Me.lblEXDamage.Name = "lblEXDamage"
        Me.lblEXDamage.Size = New System.Drawing.Size(93, 13)
        Me.lblEXDamage.TabIndex = 8
        Me.lblEXDamage.Text = "Explosive Resists:"
        '
        'txtSKi
        '
        Me.txtSKi.Location = New System.Drawing.Point(132, 175)
        Me.txtSKi.Name = "txtSKi"
        Me.txtSKi.Size = New System.Drawing.Size(75, 21)
        Me.txtSKi.TabIndex = 7
        Me.txtSKi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblKIDamage
        '
        Me.lblKIDamage.AutoSize = True
        Me.lblKIDamage.Location = New System.Drawing.Point(20, 178)
        Me.lblKIDamage.Name = "lblKIDamage"
        Me.lblKIDamage.Size = New System.Drawing.Size(79, 13)
        Me.lblKIDamage.TabIndex = 6
        Me.lblKIDamage.Text = "Kinetic Resists:"
        '
        'txtSTh
        '
        Me.txtSTh.Location = New System.Drawing.Point(132, 201)
        Me.txtSTh.Name = "txtSTh"
        Me.txtSTh.Size = New System.Drawing.Size(75, 21)
        Me.txtSTh.TabIndex = 5
        Me.txtSTh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTHDamage
        '
        Me.lblTHDamage.AutoSize = True
        Me.lblTHDamage.Location = New System.Drawing.Point(20, 204)
        Me.lblTHDamage.Name = "lblTHDamage"
        Me.lblTHDamage.Size = New System.Drawing.Size(86, 13)
        Me.lblTHDamage.TabIndex = 4
        Me.lblTHDamage.Text = "Thermal Resists:"
        '
        'txtSEM
        '
        Me.txtSEM.Location = New System.Drawing.Point(132, 123)
        Me.txtSEM.Name = "txtSEM"
        Me.txtSEM.Size = New System.Drawing.Size(75, 21)
        Me.txtSEM.TabIndex = 1
        Me.txtSEM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblEmDamage
        '
        Me.lblEmDamage.AutoSize = True
        Me.lblEmDamage.Location = New System.Drawing.Point(20, 126)
        Me.lblEmDamage.Name = "lblEmDamage"
        Me.lblEmDamage.Size = New System.Drawing.Size(62, 13)
        Me.lblEmDamage.TabIndex = 0
        Me.lblEmDamage.Text = "EM Resists:"
        '
        'PanelEx1
        '
        Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx1.Controls.Add(Me.cboProfileType)
        Me.PanelEx1.Controls.Add(Me.btnCancel)
        Me.PanelEx1.Controls.Add(Me.btnAccept)
        Me.PanelEx1.Controls.Add(Me.gpManualProfile)
        Me.PanelEx1.Controls.Add(Me.lblProfileName)
        Me.PanelEx1.Controls.Add(Me.txtProfileName)
        Me.PanelEx1.Controls.Add(Me.lblProfileType)
        Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.Size = New System.Drawing.Size(416, 364)
        Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx1.Style.GradientAngle = 90
        Me.PanelEx1.TabIndex = 7
        '
        'cboProfileType
        '
        Me.cboProfileType.DisplayMember = "Text"
        Me.cboProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboProfileType.FormattingEnabled = True
        Me.cboProfileType.ItemHeight = 15
        Me.cboProfileType.Items.AddRange(New Object() {Me.ComboItem1, Me.ComboItem2})
        Me.cboProfileType.Location = New System.Drawing.Point(106, 33)
        Me.cboProfileType.Name = "cboProfileType"
        Me.cboProfileType.Size = New System.Drawing.Size(212, 21)
        Me.cboProfileType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboProfileType.TabIndex = 11
        '
        'ComboItem1
        '
        Me.ComboItem1.Text = "Manual"
        '
        'ComboItem2
        '
        Me.ComboItem2.Text = "Fitting"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.CallBasePaintBackground = True
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.Location = New System.Drawing.Point(332, 334)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Cancel"
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.CallBasePaintBackground = True
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(251, 334)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAccept.TabIndex = 8
        Me.btnAccept.Text = "Accept"
        '
        'gpManualProfile
        '
        Me.gpManualProfile.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpManualProfile.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpManualProfile.Controls.Add(Me.txtHEM)
        Me.gpManualProfile.Controls.Add(Me.txtHTh)
        Me.gpManualProfile.Controls.Add(Me.txtHKi)
        Me.gpManualProfile.Controls.Add(Me.txtHEx)
        Me.gpManualProfile.Controls.Add(Me.txtAEM)
        Me.gpManualProfile.Controls.Add(Me.txtATh)
        Me.gpManualProfile.Controls.Add(Me.txtAKi)
        Me.gpManualProfile.Controls.Add(Me.txtAEx)
        Me.gpManualProfile.Controls.Add(Me.pbHullDefence)
        Me.gpManualProfile.Controls.Add(Me.pbArmorDefence)
        Me.gpManualProfile.Controls.Add(Me.pbShieldDefence)
        Me.gpManualProfile.Controls.Add(Me.cboPilotName)
        Me.gpManualProfile.Controls.Add(Me.cboFittingName)
        Me.gpManualProfile.Controls.Add(Me.lblFittingName)
        Me.gpManualProfile.Controls.Add(Me.lblEmDamage)
        Me.gpManualProfile.Controls.Add(Me.txtSEM)
        Me.gpManualProfile.Controls.Add(Me.lblResistInfo)
        Me.gpManualProfile.Controls.Add(Me.lblTHDamage)
        Me.gpManualProfile.Controls.Add(Me.lblPilotName)
        Me.gpManualProfile.Controls.Add(Me.txtSTh)
        Me.gpManualProfile.Controls.Add(Me.lblKIDamage)
        Me.gpManualProfile.Controls.Add(Me.txtSKi)
        Me.gpManualProfile.Controls.Add(Me.txtSEx)
        Me.gpManualProfile.Controls.Add(Me.lblEXDamage)
        Me.gpManualProfile.Location = New System.Drawing.Point(11, 69)
        Me.gpManualProfile.Name = "gpManualProfile"
        Me.gpManualProfile.Size = New System.Drawing.Size(396, 259)
        '
        '
        '
        Me.gpManualProfile.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpManualProfile.Style.BackColorGradientAngle = 90
        Me.gpManualProfile.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpManualProfile.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpManualProfile.Style.BorderBottomWidth = 1
        Me.gpManualProfile.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpManualProfile.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpManualProfile.Style.BorderLeftWidth = 1
        Me.gpManualProfile.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpManualProfile.Style.BorderRightWidth = 1
        Me.gpManualProfile.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpManualProfile.Style.BorderTopWidth = 1
        Me.gpManualProfile.Style.Class = ""
        Me.gpManualProfile.Style.CornerDiameter = 4
        Me.gpManualProfile.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpManualProfile.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpManualProfile.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpManualProfile.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpManualProfile.StyleMouseDown.Class = ""
        Me.gpManualProfile.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpManualProfile.StyleMouseOver.Class = ""
        Me.gpManualProfile.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpManualProfile.TabIndex = 7
        Me.gpManualProfile.Text = "Profile Information"
        '
        'cboPilotName
        '
        Me.cboPilotName.DisplayMember = "Text"
        Me.cboPilotName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboPilotName.FormattingEnabled = True
        Me.cboPilotName.ItemHeight = 15
        Me.cboPilotName.Items.AddRange(New Object() {Me.ComboItem8, Me.ComboItem7, Me.ComboItem9})
        Me.cboPilotName.Location = New System.Drawing.Point(109, 36)
        Me.cboPilotName.Name = "cboPilotName"
        Me.cboPilotName.Size = New System.Drawing.Size(214, 21)
        Me.cboPilotName.Sorted = True
        Me.cboPilotName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboPilotName.TabIndex = 22
        '
        'ComboItem8
        '
        Me.ComboItem8.Text = "Fitting"
        '
        'ComboItem7
        '
        Me.ComboItem7.Text = "Manual"
        '
        'ComboItem9
        '
        Me.ComboItem9.Text = "NPC"
        '
        'cboFittingName
        '
        Me.cboFittingName.DisplayMember = "Text"
        Me.cboFittingName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboFittingName.FormattingEnabled = True
        Me.cboFittingName.ItemHeight = 15
        Me.cboFittingName.Items.AddRange(New Object() {Me.ComboItem5, Me.ComboItem4, Me.ComboItem6})
        Me.cboFittingName.Location = New System.Drawing.Point(109, 9)
        Me.cboFittingName.Name = "cboFittingName"
        Me.cboFittingName.Size = New System.Drawing.Size(214, 21)
        Me.cboFittingName.Sorted = True
        Me.cboFittingName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboFittingName.TabIndex = 21
        '
        'ComboItem5
        '
        Me.ComboItem5.Text = "Fitting"
        '
        'ComboItem4
        '
        Me.ComboItem4.Text = "Manual"
        '
        'ComboItem6
        '
        Me.ComboItem6.Text = "NPC"
        '
        'pbHullDefence
        '
        Me.pbHullDefence.Image = Global.EveHQ.HQF.My.Resources.Resources.imgStructure
        Me.pbHullDefence.Location = New System.Drawing.Point(313, 85)
        Me.pbHullDefence.Name = "pbHullDefence"
        Me.pbHullDefence.Size = New System.Drawing.Size(32, 32)
        Me.pbHullDefence.TabIndex = 56
        Me.pbHullDefence.TabStop = False
        '
        'pbArmorDefence
        '
        Me.pbArmorDefence.Image = Global.EveHQ.HQF.My.Resources.Resources.imgArmor
        Me.pbArmorDefence.Location = New System.Drawing.Point(233, 85)
        Me.pbArmorDefence.Name = "pbArmorDefence"
        Me.pbArmorDefence.Size = New System.Drawing.Size(32, 32)
        Me.pbArmorDefence.TabIndex = 55
        Me.pbArmorDefence.TabStop = False
        '
        'pbShieldDefence
        '
        Me.pbShieldDefence.Image = Global.EveHQ.HQF.My.Resources.Resources.imgShield
        Me.pbShieldDefence.Location = New System.Drawing.Point(154, 85)
        Me.pbShieldDefence.Name = "pbShieldDefence"
        Me.pbShieldDefence.Size = New System.Drawing.Size(32, 32)
        Me.pbShieldDefence.TabIndex = 54
        Me.pbShieldDefence.TabStop = False
        '
        'txtAEM
        '
        Me.txtAEM.Location = New System.Drawing.Point(213, 123)
        Me.txtAEM.Name = "txtAEM"
        Me.txtAEM.Size = New System.Drawing.Size(75, 21)
        Me.txtAEM.TabIndex = 57
        Me.txtAEM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtATh
        '
        Me.txtATh.Location = New System.Drawing.Point(213, 201)
        Me.txtATh.Name = "txtATh"
        Me.txtATh.Size = New System.Drawing.Size(75, 21)
        Me.txtATh.TabIndex = 59
        Me.txtATh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAKi
        '
        Me.txtAKi.Location = New System.Drawing.Point(213, 175)
        Me.txtAKi.Name = "txtAKi"
        Me.txtAKi.Size = New System.Drawing.Size(75, 21)
        Me.txtAKi.TabIndex = 60
        Me.txtAKi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAEx
        '
        Me.txtAEx.Location = New System.Drawing.Point(213, 149)
        Me.txtAEx.Name = "txtAEx"
        Me.txtAEx.Size = New System.Drawing.Size(75, 21)
        Me.txtAEx.TabIndex = 61
        Me.txtAEx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtHEM
        '
        Me.txtHEM.Location = New System.Drawing.Point(294, 123)
        Me.txtHEM.Name = "txtHEM"
        Me.txtHEM.Size = New System.Drawing.Size(75, 21)
        Me.txtHEM.TabIndex = 62
        Me.txtHEM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtHTh
        '
        Me.txtHTh.Location = New System.Drawing.Point(294, 201)
        Me.txtHTh.Name = "txtHTh"
        Me.txtHTh.Size = New System.Drawing.Size(75, 21)
        Me.txtHTh.TabIndex = 64
        Me.txtHTh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtHKi
        '
        Me.txtHKi.Location = New System.Drawing.Point(294, 175)
        Me.txtHKi.Name = "txtHKi"
        Me.txtHKi.Size = New System.Drawing.Size(75, 21)
        Me.txtHKi.TabIndex = 65
        Me.txtHKi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtHEx
        '
        Me.txtHEx.Location = New System.Drawing.Point(294, 149)
        Me.txtHEx.Name = "txtHEx"
        Me.txtHEx.Size = New System.Drawing.Size(75, 21)
        Me.txtHEx.TabIndex = 66
        Me.txtHEx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmAddDefenceProfile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(416, 364)
        Me.Controls.Add(Me.PanelEx1)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddDefenceProfile"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Defence Profile"
        Me.PanelEx1.ResumeLayout(False)
        Me.PanelEx1.PerformLayout()
        Me.gpManualProfile.ResumeLayout(False)
        Me.gpManualProfile.PerformLayout()
        CType(Me.pbHullDefence, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorDefence, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldDefence, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblProfileName As System.Windows.Forms.Label
    Friend WithEvents txtProfileName As System.Windows.Forms.TextBox
    Friend WithEvents lblProfileType As System.Windows.Forms.Label
    Friend WithEvents txtSEM As System.Windows.Forms.TextBox
    Friend WithEvents lblEmDamage As System.Windows.Forms.Label
    Friend WithEvents txtSEx As System.Windows.Forms.TextBox
    Friend WithEvents lblEXDamage As System.Windows.Forms.Label
    Friend WithEvents txtSKi As System.Windows.Forms.TextBox
    Friend WithEvents lblKIDamage As System.Windows.Forms.Label
    Friend WithEvents txtSTh As System.Windows.Forms.TextBox
    Friend WithEvents lblTHDamage As System.Windows.Forms.Label
    Friend WithEvents lblPilotName As System.Windows.Forms.Label
    Friend WithEvents lblFittingName As System.Windows.Forms.Label
    Friend WithEvents lblResistInfo As System.Windows.Forms.Label
    Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents gpManualProfile As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboProfileType As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem1 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem2 As DevComponents.Editors.ComboItem
    Friend WithEvents cboPilotName As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem7 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem8 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem9 As DevComponents.Editors.ComboItem
    Friend WithEvents cboFittingName As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem4 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem5 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem6 As DevComponents.Editors.ComboItem
    Friend WithEvents txtHEM As System.Windows.Forms.TextBox
    Friend WithEvents txtHTh As System.Windows.Forms.TextBox
    Friend WithEvents txtHKi As System.Windows.Forms.TextBox
    Friend WithEvents txtHEx As System.Windows.Forms.TextBox
    Friend WithEvents txtAEM As System.Windows.Forms.TextBox
    Friend WithEvents txtATh As System.Windows.Forms.TextBox
    Friend WithEvents txtAKi As System.Windows.Forms.TextBox
    Friend WithEvents txtAEx As System.Windows.Forms.TextBox
    Friend WithEvents pbHullDefence As System.Windows.Forms.PictureBox
    Friend WithEvents pbArmorDefence As System.Windows.Forms.PictureBox
    Friend WithEvents pbShieldDefence As System.Windows.Forms.PictureBox
End Class
