<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddDamageProfile
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
        Me.lblProfileName = New System.Windows.Forms.Label()
        Me.txtProfileName = New System.Windows.Forms.TextBox()
        Me.lblProfileType = New System.Windows.Forms.Label()
        Me.lblDamageInfo = New System.Windows.Forms.Label()
        Me.lblPilotName = New System.Windows.Forms.Label()
        Me.lblFittingName = New System.Windows.Forms.Label()
        Me.txtEXDamage = New System.Windows.Forms.TextBox()
        Me.lblEXDamage = New System.Windows.Forms.Label()
        Me.txtKIDamage = New System.Windows.Forms.TextBox()
        Me.lblKIDamage = New System.Windows.Forms.Label()
        Me.txtTHDamage = New System.Windows.Forms.TextBox()
        Me.lblTHDamage = New System.Windows.Forms.Label()
        Me.txtDPS = New System.Windows.Forms.TextBox()
        Me.lblDPS = New System.Windows.Forms.Label()
        Me.txtEMDamage = New System.Windows.Forms.TextBox()
        Me.lblEmDamage = New System.Windows.Forms.Label()
        Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
        Me.cboProfileType = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem1 = New DevComponents.Editors.ComboItem()
        Me.ComboItem2 = New DevComponents.Editors.ComboItem()
        Me.ComboItem3 = New DevComponents.Editors.ComboItem()
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX()
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX()
        Me.gpManualProfile = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.cboPilotName = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem8 = New DevComponents.Editors.ComboItem()
        Me.ComboItem7 = New DevComponents.Editors.ComboItem()
        Me.ComboItem9 = New DevComponents.Editors.ComboItem()
        Me.cboFittingName = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem5 = New DevComponents.Editors.ComboItem()
        Me.ComboItem4 = New DevComponents.Editors.ComboItem()
        Me.ComboItem6 = New DevComponents.Editors.ComboItem()
        Me.PanelEx1.SuspendLayout()
        Me.gpManualProfile.SuspendLayout()
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
        'lblDamageInfo
        '
        Me.lblDamageInfo.AutoSize = True
        Me.lblDamageInfo.Location = New System.Drawing.Point(15, 77)
        Me.lblDamageInfo.Name = "lblDamageInfo"
        Me.lblDamageInfo.Size = New System.Drawing.Size(85, 13)
        Me.lblDamageInfo.TabIndex = 15
        Me.lblDamageInfo.Text = "Damage Details:"
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
        'txtEXDamage
        '
        Me.txtEXDamage.Location = New System.Drawing.Point(145, 124)
        Me.txtEXDamage.Name = "txtEXDamage"
        Me.txtEXDamage.Size = New System.Drawing.Size(75, 21)
        Me.txtEXDamage.TabIndex = 9
        Me.txtEXDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblEXDamage
        '
        Me.lblEXDamage.AutoSize = True
        Me.lblEXDamage.Location = New System.Drawing.Point(33, 127)
        Me.lblEXDamage.Name = "lblEXDamage"
        Me.lblEXDamage.Size = New System.Drawing.Size(98, 13)
        Me.lblEXDamage.TabIndex = 8
        Me.lblEXDamage.Text = "Explosive Damage:"
        '
        'txtKIDamage
        '
        Me.txtKIDamage.Location = New System.Drawing.Point(145, 150)
        Me.txtKIDamage.Name = "txtKIDamage"
        Me.txtKIDamage.Size = New System.Drawing.Size(75, 21)
        Me.txtKIDamage.TabIndex = 7
        Me.txtKIDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblKIDamage
        '
        Me.lblKIDamage.AutoSize = True
        Me.lblKIDamage.Location = New System.Drawing.Point(33, 153)
        Me.lblKIDamage.Name = "lblKIDamage"
        Me.lblKIDamage.Size = New System.Drawing.Size(84, 13)
        Me.lblKIDamage.TabIndex = 6
        Me.lblKIDamage.Text = "Kinetic Damage:"
        '
        'txtTHDamage
        '
        Me.txtTHDamage.Location = New System.Drawing.Point(145, 176)
        Me.txtTHDamage.Name = "txtTHDamage"
        Me.txtTHDamage.Size = New System.Drawing.Size(75, 21)
        Me.txtTHDamage.TabIndex = 5
        Me.txtTHDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTHDamage
        '
        Me.lblTHDamage.AutoSize = True
        Me.lblTHDamage.Location = New System.Drawing.Point(33, 179)
        Me.lblTHDamage.Name = "lblTHDamage"
        Me.lblTHDamage.Size = New System.Drawing.Size(91, 13)
        Me.lblTHDamage.TabIndex = 4
        Me.lblTHDamage.Text = "Thermal Damage:"
        '
        'txtDPS
        '
        Me.txtDPS.Location = New System.Drawing.Point(145, 202)
        Me.txtDPS.Name = "txtDPS"
        Me.txtDPS.Size = New System.Drawing.Size(75, 21)
        Me.txtDPS.TabIndex = 3
        Me.txtDPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDPS
        '
        Me.lblDPS.AutoSize = True
        Me.lblDPS.Location = New System.Drawing.Point(33, 205)
        Me.lblDPS.Name = "lblDPS"
        Me.lblDPS.Size = New System.Drawing.Size(30, 13)
        Me.lblDPS.TabIndex = 2
        Me.lblDPS.Text = "DPS:"
        '
        'txtEMDamage
        '
        Me.txtEMDamage.Location = New System.Drawing.Point(145, 98)
        Me.txtEMDamage.Name = "txtEMDamage"
        Me.txtEMDamage.Size = New System.Drawing.Size(75, 21)
        Me.txtEMDamage.TabIndex = 1
        Me.txtEMDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblEmDamage
        '
        Me.lblEmDamage.AutoSize = True
        Me.lblEmDamage.Location = New System.Drawing.Point(33, 101)
        Me.lblEmDamage.Name = "lblEmDamage"
        Me.lblEmDamage.Size = New System.Drawing.Size(67, 13)
        Me.lblEmDamage.TabIndex = 0
        Me.lblEmDamage.Text = "EM Damage:"
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
        Me.PanelEx1.Size = New System.Drawing.Size(362, 360)
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
        Me.cboProfileType.Items.AddRange(New Object() {Me.ComboItem1, Me.ComboItem2, Me.ComboItem3})
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
        'ComboItem3
        '
        Me.ComboItem3.Text = "NPC"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(272, 329)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Cancel"
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(191, 329)
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
        Me.gpManualProfile.Controls.Add(Me.cboPilotName)
        Me.gpManualProfile.Controls.Add(Me.cboFittingName)
        Me.gpManualProfile.Controls.Add(Me.lblFittingName)
        Me.gpManualProfile.Controls.Add(Me.lblEmDamage)
        Me.gpManualProfile.Controls.Add(Me.txtEMDamage)
        Me.gpManualProfile.Controls.Add(Me.lblDamageInfo)
        Me.gpManualProfile.Controls.Add(Me.lblDPS)
        Me.gpManualProfile.Controls.Add(Me.txtDPS)
        Me.gpManualProfile.Controls.Add(Me.lblTHDamage)
        Me.gpManualProfile.Controls.Add(Me.lblPilotName)
        Me.gpManualProfile.Controls.Add(Me.txtTHDamage)
        Me.gpManualProfile.Controls.Add(Me.lblKIDamage)
        Me.gpManualProfile.Controls.Add(Me.txtKIDamage)
        Me.gpManualProfile.Controls.Add(Me.txtEXDamage)
        Me.gpManualProfile.Controls.Add(Me.lblEXDamage)
        Me.gpManualProfile.IsShadowEnabled = True
        Me.gpManualProfile.Location = New System.Drawing.Point(11, 69)
        Me.gpManualProfile.Name = "gpManualProfile"
        Me.gpManualProfile.Size = New System.Drawing.Size(336, 254)
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
        Me.gpManualProfile.Style.CornerDiameter = 4
        Me.gpManualProfile.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpManualProfile.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpManualProfile.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpManualProfile.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpManualProfile.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
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
        'frmAddDamageProfile
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(362, 360)
        Me.Controls.Add(Me.PanelEx1)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddDamageProfile"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Damage Profile"
        Me.PanelEx1.ResumeLayout(False)
        Me.PanelEx1.PerformLayout()
        Me.gpManualProfile.ResumeLayout(False)
        Me.gpManualProfile.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblProfileName As System.Windows.Forms.Label
    Friend WithEvents txtProfileName As System.Windows.Forms.TextBox
    Friend WithEvents lblProfileType As System.Windows.Forms.Label
    Friend WithEvents txtEMDamage As System.Windows.Forms.TextBox
    Friend WithEvents lblEmDamage As System.Windows.Forms.Label
    Friend WithEvents txtEXDamage As System.Windows.Forms.TextBox
    Friend WithEvents lblEXDamage As System.Windows.Forms.Label
    Friend WithEvents txtKIDamage As System.Windows.Forms.TextBox
    Friend WithEvents lblKIDamage As System.Windows.Forms.Label
    Friend WithEvents txtTHDamage As System.Windows.Forms.TextBox
    Friend WithEvents lblTHDamage As System.Windows.Forms.Label
    Friend WithEvents txtDPS As System.Windows.Forms.TextBox
    Friend WithEvents lblDPS As System.Windows.Forms.Label
    Friend WithEvents lblPilotName As System.Windows.Forms.Label
    Friend WithEvents lblFittingName As System.Windows.Forms.Label
    Friend WithEvents lblDamageInfo As System.Windows.Forms.Label
    Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents gpManualProfile As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboProfileType As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem1 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem2 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem3 As DevComponents.Editors.ComboItem
    Friend WithEvents cboPilotName As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem7 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem8 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem9 As DevComponents.Editors.ComboItem
    Friend WithEvents cboFittingName As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem4 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem5 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem6 As DevComponents.Editors.ComboItem
End Class
