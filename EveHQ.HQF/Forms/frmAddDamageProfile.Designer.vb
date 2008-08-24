<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddDamageProfile
    Inherits System.Windows.Forms.Form

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
        Me.cboProfileType = New System.Windows.Forms.ComboBox
        Me.gbManualProfile = New System.Windows.Forms.GroupBox
        Me.cboNPC = New System.Windows.Forms.ComboBox
        Me.btnAddNPC = New System.Windows.Forms.Button
        Me.lblNPCs = New System.Windows.Forms.Label
        Me.lblDamageInfo = New System.Windows.Forms.Label
        Me.lvwNPCs = New System.Windows.Forms.ListView
        Me.colNPCs = New System.Windows.Forms.ColumnHeader
        Me.cboPilotName = New System.Windows.Forms.ComboBox
        Me.lblPilotName = New System.Windows.Forms.Label
        Me.cboFittingName = New System.Windows.Forms.ComboBox
        Me.lblFittingName = New System.Windows.Forms.Label
        Me.txtEXDamage = New System.Windows.Forms.TextBox
        Me.lblEXDamage = New System.Windows.Forms.Label
        Me.txtKIDamage = New System.Windows.Forms.TextBox
        Me.lblKIDamage = New System.Windows.Forms.Label
        Me.txtTHDamage = New System.Windows.Forms.TextBox
        Me.lblTHDamage = New System.Windows.Forms.Label
        Me.txtDPS = New System.Windows.Forms.TextBox
        Me.lblDPS = New System.Windows.Forms.Label
        Me.txtEMDamage = New System.Windows.Forms.TextBox
        Me.lblEmDamage = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAccept = New System.Windows.Forms.Button
        Me.gbManualProfile.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblProfileName
        '
        Me.lblProfileName.AutoSize = True
        Me.lblProfileName.Location = New System.Drawing.Point(13, 13)
        Me.lblProfileName.Name = "lblProfileName"
        Me.lblProfileName.Size = New System.Drawing.Size(71, 13)
        Me.lblProfileName.TabIndex = 0
        Me.lblProfileName.Text = "Profile Name:"
        '
        'txtProfileName
        '
        Me.txtProfileName.Location = New System.Drawing.Point(107, 10)
        Me.txtProfileName.Name = "txtProfileName"
        Me.txtProfileName.Size = New System.Drawing.Size(212, 21)
        Me.txtProfileName.TabIndex = 1
        '
        'lblProfileType
        '
        Me.lblProfileType.AutoSize = True
        Me.lblProfileType.Location = New System.Drawing.Point(13, 39)
        Me.lblProfileType.Name = "lblProfileType"
        Me.lblProfileType.Size = New System.Drawing.Size(68, 13)
        Me.lblProfileType.TabIndex = 2
        Me.lblProfileType.Text = "Profile Type:"
        '
        'cboProfileType
        '
        Me.cboProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProfileType.FormattingEnabled = True
        Me.cboProfileType.Items.AddRange(New Object() {"Manual", "Fitting", "NPC"})
        Me.cboProfileType.Location = New System.Drawing.Point(107, 36)
        Me.cboProfileType.Name = "cboProfileType"
        Me.cboProfileType.Size = New System.Drawing.Size(212, 21)
        Me.cboProfileType.TabIndex = 3
        '
        'gbManualProfile
        '
        Me.gbManualProfile.Controls.Add(Me.cboNPC)
        Me.gbManualProfile.Controls.Add(Me.btnAddNPC)
        Me.gbManualProfile.Controls.Add(Me.lblNPCs)
        Me.gbManualProfile.Controls.Add(Me.lblDamageInfo)
        Me.gbManualProfile.Controls.Add(Me.lvwNPCs)
        Me.gbManualProfile.Controls.Add(Me.cboPilotName)
        Me.gbManualProfile.Controls.Add(Me.lblPilotName)
        Me.gbManualProfile.Controls.Add(Me.cboFittingName)
        Me.gbManualProfile.Controls.Add(Me.lblFittingName)
        Me.gbManualProfile.Controls.Add(Me.txtEXDamage)
        Me.gbManualProfile.Controls.Add(Me.lblEXDamage)
        Me.gbManualProfile.Controls.Add(Me.txtKIDamage)
        Me.gbManualProfile.Controls.Add(Me.lblKIDamage)
        Me.gbManualProfile.Controls.Add(Me.txtTHDamage)
        Me.gbManualProfile.Controls.Add(Me.lblTHDamage)
        Me.gbManualProfile.Controls.Add(Me.txtDPS)
        Me.gbManualProfile.Controls.Add(Me.lblDPS)
        Me.gbManualProfile.Controls.Add(Me.txtEMDamage)
        Me.gbManualProfile.Controls.Add(Me.lblEmDamage)
        Me.gbManualProfile.Location = New System.Drawing.Point(12, 78)
        Me.gbManualProfile.Name = "gbManualProfile"
        Me.gbManualProfile.Size = New System.Drawing.Size(627, 353)
        Me.gbManualProfile.TabIndex = 4
        Me.gbManualProfile.TabStop = False
        Me.gbManualProfile.Text = "Profile Information"
        '
        'cboNPC
        '
        Me.cboNPC.FormattingEnabled = True
        Me.cboNPC.Location = New System.Drawing.Point(44, 317)
        Me.cboNPC.Name = "cboNPC"
        Me.cboNPC.Size = New System.Drawing.Size(263, 21)
        Me.cboNPC.TabIndex = 19
        '
        'btnAddNPC
        '
        Me.btnAddNPC.Location = New System.Drawing.Point(310, 317)
        Me.btnAddNPC.Name = "btnAddNPC"
        Me.btnAddNPC.Size = New System.Drawing.Size(62, 23)
        Me.btnAddNPC.TabIndex = 18
        Me.btnAddNPC.Text = "Add NPC"
        Me.btnAddNPC.UseVisualStyleBackColor = True
        '
        'lblNPCs
        '
        Me.lblNPCs.AutoSize = True
        Me.lblNPCs.Location = New System.Drawing.Point(25, 125)
        Me.lblNPCs.Name = "lblNPCs"
        Me.lblNPCs.Size = New System.Drawing.Size(77, 13)
        Me.lblNPCs.TabIndex = 16
        Me.lblNPCs.Text = "NPC Selection:"
        '
        'lblDamageInfo
        '
        Me.lblDamageInfo.AutoSize = True
        Me.lblDamageInfo.Location = New System.Drawing.Point(399, 44)
        Me.lblDamageInfo.Name = "lblDamageInfo"
        Me.lblDamageInfo.Size = New System.Drawing.Size(85, 13)
        Me.lblDamageInfo.TabIndex = 15
        Me.lblDamageInfo.Text = "Damage Details:"
        '
        'lvwNPCs
        '
        Me.lvwNPCs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colNPCs})
        Me.lvwNPCs.Location = New System.Drawing.Point(44, 147)
        Me.lvwNPCs.Name = "lvwNPCs"
        Me.lvwNPCs.Size = New System.Drawing.Size(328, 167)
        Me.lvwNPCs.TabIndex = 14
        Me.lvwNPCs.UseCompatibleStateImageBehavior = False
        Me.lvwNPCs.View = System.Windows.Forms.View.Details
        '
        'colNPCs
        '
        Me.colNPCs.Text = "NPC Name"
        Me.colNPCs.Width = 300
        '
        'cboPilotName
        '
        Me.cboPilotName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilotName.FormattingEnabled = True
        Me.cboPilotName.Location = New System.Drawing.Point(119, 69)
        Me.cboPilotName.Name = "cboPilotName"
        Me.cboPilotName.Size = New System.Drawing.Size(214, 21)
        Me.cboPilotName.TabIndex = 13
        '
        'lblPilotName
        '
        Me.lblPilotName.AutoSize = True
        Me.lblPilotName.Location = New System.Drawing.Point(25, 72)
        Me.lblPilotName.Name = "lblPilotName"
        Me.lblPilotName.Size = New System.Drawing.Size(61, 13)
        Me.lblPilotName.TabIndex = 12
        Me.lblPilotName.Text = "Pilot Name:"
        '
        'cboFittingName
        '
        Me.cboFittingName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFittingName.FormattingEnabled = True
        Me.cboFittingName.Location = New System.Drawing.Point(119, 42)
        Me.cboFittingName.Name = "cboFittingName"
        Me.cboFittingName.Size = New System.Drawing.Size(214, 21)
        Me.cboFittingName.TabIndex = 11
        '
        'lblFittingName
        '
        Me.lblFittingName.AutoSize = True
        Me.lblFittingName.Location = New System.Drawing.Point(25, 45)
        Me.lblFittingName.Name = "lblFittingName"
        Me.lblFittingName.Size = New System.Drawing.Size(71, 13)
        Me.lblFittingName.TabIndex = 10
        Me.lblFittingName.Text = "Fitting Name:"
        '
        'txtEXDamage
        '
        Me.txtEXDamage.Location = New System.Drawing.Point(529, 91)
        Me.txtEXDamage.Name = "txtEXDamage"
        Me.txtEXDamage.Size = New System.Drawing.Size(75, 21)
        Me.txtEXDamage.TabIndex = 9
        Me.txtEXDamage.Text = "0.00"
        Me.txtEXDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblEXDamage
        '
        Me.lblEXDamage.AutoSize = True
        Me.lblEXDamage.Location = New System.Drawing.Point(417, 94)
        Me.lblEXDamage.Name = "lblEXDamage"
        Me.lblEXDamage.Size = New System.Drawing.Size(98, 13)
        Me.lblEXDamage.TabIndex = 8
        Me.lblEXDamage.Text = "Explosive Damage:"
        '
        'txtKIDamage
        '
        Me.txtKIDamage.Location = New System.Drawing.Point(529, 117)
        Me.txtKIDamage.Name = "txtKIDamage"
        Me.txtKIDamage.Size = New System.Drawing.Size(75, 21)
        Me.txtKIDamage.TabIndex = 7
        Me.txtKIDamage.Text = "0.00"
        Me.txtKIDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblKIDamage
        '
        Me.lblKIDamage.AutoSize = True
        Me.lblKIDamage.Location = New System.Drawing.Point(417, 120)
        Me.lblKIDamage.Name = "lblKIDamage"
        Me.lblKIDamage.Size = New System.Drawing.Size(84, 13)
        Me.lblKIDamage.TabIndex = 6
        Me.lblKIDamage.Text = "Kinetic Damage:"
        '
        'txtTHDamage
        '
        Me.txtTHDamage.Location = New System.Drawing.Point(529, 143)
        Me.txtTHDamage.Name = "txtTHDamage"
        Me.txtTHDamage.Size = New System.Drawing.Size(75, 21)
        Me.txtTHDamage.TabIndex = 5
        Me.txtTHDamage.Text = "0.00"
        Me.txtTHDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTHDamage
        '
        Me.lblTHDamage.AutoSize = True
        Me.lblTHDamage.Location = New System.Drawing.Point(417, 146)
        Me.lblTHDamage.Name = "lblTHDamage"
        Me.lblTHDamage.Size = New System.Drawing.Size(91, 13)
        Me.lblTHDamage.TabIndex = 4
        Me.lblTHDamage.Text = "Thermal Damage:"
        '
        'txtDPS
        '
        Me.txtDPS.Location = New System.Drawing.Point(529, 169)
        Me.txtDPS.Name = "txtDPS"
        Me.txtDPS.Size = New System.Drawing.Size(75, 21)
        Me.txtDPS.TabIndex = 3
        Me.txtDPS.Text = "0.00"
        Me.txtDPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDPS
        '
        Me.lblDPS.AutoSize = True
        Me.lblDPS.Location = New System.Drawing.Point(417, 172)
        Me.lblDPS.Name = "lblDPS"
        Me.lblDPS.Size = New System.Drawing.Size(30, 13)
        Me.lblDPS.TabIndex = 2
        Me.lblDPS.Text = "DPS:"
        '
        'txtEMDamage
        '
        Me.txtEMDamage.Location = New System.Drawing.Point(529, 65)
        Me.txtEMDamage.Name = "txtEMDamage"
        Me.txtEMDamage.Size = New System.Drawing.Size(75, 21)
        Me.txtEMDamage.TabIndex = 1
        Me.txtEMDamage.Text = "0.00"
        Me.txtEMDamage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblEmDamage
        '
        Me.lblEmDamage.AutoSize = True
        Me.lblEmDamage.Location = New System.Drawing.Point(417, 68)
        Me.lblEmDamage.Name = "lblEmDamage"
        Me.lblEmDamage.Size = New System.Drawing.Size(67, 13)
        Me.lblEmDamage.TabIndex = 0
        Me.lblEmDamage.Text = "EM Damage:"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(564, 437)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(483, 437)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 6
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'frmAddDamageProfile
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(646, 468)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.gbManualProfile)
        Me.Controls.Add(Me.cboProfileType)
        Me.Controls.Add(Me.lblProfileType)
        Me.Controls.Add(Me.txtProfileName)
        Me.Controls.Add(Me.lblProfileName)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddDamageProfile"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Damage Profile"
        Me.gbManualProfile.ResumeLayout(False)
        Me.gbManualProfile.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblProfileName As System.Windows.Forms.Label
    Friend WithEvents txtProfileName As System.Windows.Forms.TextBox
    Friend WithEvents lblProfileType As System.Windows.Forms.Label
    Friend WithEvents cboProfileType As System.Windows.Forms.ComboBox
    Friend WithEvents gbManualProfile As System.Windows.Forms.GroupBox
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
    Friend WithEvents lvwNPCs As System.Windows.Forms.ListView
    Friend WithEvents cboPilotName As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilotName As System.Windows.Forms.Label
    Friend WithEvents cboFittingName As System.Windows.Forms.ComboBox
    Friend WithEvents lblFittingName As System.Windows.Forms.Label
    Friend WithEvents lblNPCs As System.Windows.Forms.Label
    Friend WithEvents lblDamageInfo As System.Windows.Forms.Label
    Friend WithEvents btnAddNPC As System.Windows.Forms.Button
    Friend WithEvents colNPCs As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents cboNPC As System.Windows.Forms.ComboBox
End Class
