<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDamageProfiles
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
        Me.lvwProfiles = New System.Windows.Forms.ListView
        Me.colProfileName = New System.Windows.Forms.ColumnHeader
        Me.colProfileType = New System.Windows.Forms.ColumnHeader
        Me.gbProfileInfo = New System.Windows.Forms.GroupBox
        Me.lblNPCName = New System.Windows.Forms.Label
        Me.lblNPCNameLbl = New System.Windows.Forms.Label
        Me.lblPilotName = New System.Windows.Forms.Label
        Me.lblPilotNameLbl = New System.Windows.Forms.Label
        Me.lblFittingName = New System.Windows.Forms.Label
        Me.lblFittingNameLbl = New System.Windows.Forms.Label
        Me.lblDPS = New System.Windows.Forms.Label
        Me.lblDPSLbl = New System.Windows.Forms.Label
        Me.lblProfileType = New System.Windows.Forms.Label
        Me.lblProfileName = New System.Windows.Forms.Label
        Me.lblTotalDamagePercentage = New System.Windows.Forms.Label
        Me.lblTotalDamageAmount = New System.Windows.Forms.Label
        Me.line2 = New System.Windows.Forms.Label
        Me.lblTHDamagePercentage = New System.Windows.Forms.Label
        Me.lblTHDamageAmount = New System.Windows.Forms.Label
        Me.lblTHDamage = New System.Windows.Forms.Label
        Me.lblKIDamagePercentage = New System.Windows.Forms.Label
        Me.lblKIDamageAmount = New System.Windows.Forms.Label
        Me.lblKIDamage = New System.Windows.Forms.Label
        Me.lblEXDamagePercentage = New System.Windows.Forms.Label
        Me.lblEXDamageAmount = New System.Windows.Forms.Label
        Me.lblEXDamage = New System.Windows.Forms.Label
        Me.lblEMDamagePercentage = New System.Windows.Forms.Label
        Me.lblEMDamageAmount = New System.Windows.Forms.Label
        Me.lblEMDamage = New System.Windows.Forms.Label
        Me.lblDamageTypes = New System.Windows.Forms.Label
        Me.lblProfileTypeLbl = New System.Windows.Forms.Label
        Me.lblProfileNameLbl = New System.Windows.Forms.Label
        Me.btnAddProfile = New System.Windows.Forms.Button
        Me.btnEditProfile = New System.Windows.Forms.Button
        Me.btnDeleteProfile = New System.Windows.Forms.Button
        Me.btnResetProfiles = New System.Windows.Forms.Button
        Me.gbProfileInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvwProfiles
        '
        Me.lvwProfiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colProfileName, Me.colProfileType})
        Me.lvwProfiles.FullRowSelect = True
        Me.lvwProfiles.GridLines = True
        Me.lvwProfiles.Location = New System.Drawing.Point(12, 12)
        Me.lvwProfiles.Name = "lvwProfiles"
        Me.lvwProfiles.Size = New System.Drawing.Size(379, 519)
        Me.lvwProfiles.TabIndex = 0
        Me.lvwProfiles.UseCompatibleStateImageBehavior = False
        Me.lvwProfiles.View = System.Windows.Forms.View.Details
        '
        'colProfileName
        '
        Me.colProfileName.Text = "Profile Name"
        Me.colProfileName.Width = 250
        '
        'colProfileType
        '
        Me.colProfileType.Text = "Profile Type"
        Me.colProfileType.Width = 100
        '
        'gbProfileInfo
        '
        Me.gbProfileInfo.Controls.Add(Me.lblNPCName)
        Me.gbProfileInfo.Controls.Add(Me.lblNPCNameLbl)
        Me.gbProfileInfo.Controls.Add(Me.lblPilotName)
        Me.gbProfileInfo.Controls.Add(Me.lblPilotNameLbl)
        Me.gbProfileInfo.Controls.Add(Me.lblFittingName)
        Me.gbProfileInfo.Controls.Add(Me.lblFittingNameLbl)
        Me.gbProfileInfo.Controls.Add(Me.lblDPS)
        Me.gbProfileInfo.Controls.Add(Me.lblDPSLbl)
        Me.gbProfileInfo.Controls.Add(Me.lblProfileType)
        Me.gbProfileInfo.Controls.Add(Me.lblProfileName)
        Me.gbProfileInfo.Controls.Add(Me.lblTotalDamagePercentage)
        Me.gbProfileInfo.Controls.Add(Me.lblTotalDamageAmount)
        Me.gbProfileInfo.Controls.Add(Me.line2)
        Me.gbProfileInfo.Controls.Add(Me.lblTHDamagePercentage)
        Me.gbProfileInfo.Controls.Add(Me.lblTHDamageAmount)
        Me.gbProfileInfo.Controls.Add(Me.lblTHDamage)
        Me.gbProfileInfo.Controls.Add(Me.lblKIDamagePercentage)
        Me.gbProfileInfo.Controls.Add(Me.lblKIDamageAmount)
        Me.gbProfileInfo.Controls.Add(Me.lblKIDamage)
        Me.gbProfileInfo.Controls.Add(Me.lblEXDamagePercentage)
        Me.gbProfileInfo.Controls.Add(Me.lblEXDamageAmount)
        Me.gbProfileInfo.Controls.Add(Me.lblEXDamage)
        Me.gbProfileInfo.Controls.Add(Me.lblEMDamagePercentage)
        Me.gbProfileInfo.Controls.Add(Me.lblEMDamageAmount)
        Me.gbProfileInfo.Controls.Add(Me.lblEMDamage)
        Me.gbProfileInfo.Controls.Add(Me.lblDamageTypes)
        Me.gbProfileInfo.Controls.Add(Me.lblProfileTypeLbl)
        Me.gbProfileInfo.Controls.Add(Me.lblProfileNameLbl)
        Me.gbProfileInfo.Location = New System.Drawing.Point(397, 149)
        Me.gbProfileInfo.Name = "gbProfileInfo"
        Me.gbProfileInfo.Size = New System.Drawing.Size(253, 382)
        Me.gbProfileInfo.TabIndex = 1
        Me.gbProfileInfo.TabStop = False
        Me.gbProfileInfo.Text = "Selected Profile Information"
        Me.gbProfileInfo.Visible = False
        '
        'lblNPCName
        '
        Me.lblNPCName.AutoSize = True
        Me.lblNPCName.Location = New System.Drawing.Point(82, 290)
        Me.lblNPCName.Name = "lblNPCName"
        Me.lblNPCName.Size = New System.Drawing.Size(24, 13)
        Me.lblNPCName.TabIndex = 42
        Me.lblNPCName.Text = "n/a"
        '
        'lblNPCNameLbl
        '
        Me.lblNPCNameLbl.AutoSize = True
        Me.lblNPCNameLbl.Location = New System.Drawing.Point(6, 290)
        Me.lblNPCNameLbl.Name = "lblNPCNameLbl"
        Me.lblNPCNameLbl.Size = New System.Drawing.Size(74, 13)
        Me.lblNPCNameLbl.TabIndex = 41
        Me.lblNPCNameLbl.Text = "NPC Name(s):"
        '
        'lblPilotName
        '
        Me.lblPilotName.AutoSize = True
        Me.lblPilotName.Location = New System.Drawing.Point(82, 270)
        Me.lblPilotName.Name = "lblPilotName"
        Me.lblPilotName.Size = New System.Drawing.Size(24, 13)
        Me.lblPilotName.TabIndex = 40
        Me.lblPilotName.Text = "n/a"
        '
        'lblPilotNameLbl
        '
        Me.lblPilotNameLbl.AutoSize = True
        Me.lblPilotNameLbl.Location = New System.Drawing.Point(6, 270)
        Me.lblPilotNameLbl.Name = "lblPilotNameLbl"
        Me.lblPilotNameLbl.Size = New System.Drawing.Size(61, 13)
        Me.lblPilotNameLbl.TabIndex = 39
        Me.lblPilotNameLbl.Text = "Pilot Name:"
        '
        'lblFittingName
        '
        Me.lblFittingName.AutoSize = True
        Me.lblFittingName.Location = New System.Drawing.Point(82, 250)
        Me.lblFittingName.Name = "lblFittingName"
        Me.lblFittingName.Size = New System.Drawing.Size(24, 13)
        Me.lblFittingName.TabIndex = 38
        Me.lblFittingName.Text = "n/a"
        '
        'lblFittingNameLbl
        '
        Me.lblFittingNameLbl.AutoSize = True
        Me.lblFittingNameLbl.Location = New System.Drawing.Point(6, 250)
        Me.lblFittingNameLbl.Name = "lblFittingNameLbl"
        Me.lblFittingNameLbl.Size = New System.Drawing.Size(69, 13)
        Me.lblFittingNameLbl.TabIndex = 37
        Me.lblFittingNameLbl.Text = "Fitting Name:"
        '
        'lblDPS
        '
        Me.lblDPS.AutoSize = True
        Me.lblDPS.Location = New System.Drawing.Point(82, 230)
        Me.lblDPS.Name = "lblDPS"
        Me.lblDPS.Size = New System.Drawing.Size(46, 13)
        Me.lblDPS.TabIndex = 36
        Me.lblDPS.Text = "1000.00"
        '
        'lblDPSLbl
        '
        Me.lblDPSLbl.AutoSize = True
        Me.lblDPSLbl.Location = New System.Drawing.Point(6, 230)
        Me.lblDPSLbl.Name = "lblDPSLbl"
        Me.lblDPSLbl.Size = New System.Drawing.Size(32, 13)
        Me.lblDPSLbl.TabIndex = 35
        Me.lblDPSLbl.Text = "DPS:"
        '
        'lblProfileType
        '
        Me.lblProfileType.AutoSize = True
        Me.lblProfileType.Location = New System.Drawing.Point(82, 46)
        Me.lblProfileType.Name = "lblProfileType"
        Me.lblProfileType.Size = New System.Drawing.Size(42, 13)
        Me.lblProfileType.TabIndex = 34
        Me.lblProfileType.Text = "Manual"
        '
        'lblProfileName
        '
        Me.lblProfileName.AutoSize = True
        Me.lblProfileName.Location = New System.Drawing.Point(82, 26)
        Me.lblProfileName.Name = "lblProfileName"
        Me.lblProfileName.Size = New System.Drawing.Size(86, 13)
        Me.lblProfileName.TabIndex = 33
        Me.lblProfileName.Text = "<Omni-Damage>"
        '
        'lblTotalDamagePercentage
        '
        Me.lblTotalDamagePercentage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTotalDamagePercentage.Location = New System.Drawing.Point(173, 188)
        Me.lblTotalDamagePercentage.Name = "lblTotalDamagePercentage"
        Me.lblTotalDamagePercentage.Size = New System.Drawing.Size(67, 20)
        Me.lblTotalDamagePercentage.TabIndex = 32
        Me.lblTotalDamagePercentage.Text = "= 100.00%"
        Me.lblTotalDamagePercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTotalDamageAmount
        '
        Me.lblTotalDamageAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTotalDamageAmount.Location = New System.Drawing.Point(108, 188)
        Me.lblTotalDamageAmount.Name = "lblTotalDamageAmount"
        Me.lblTotalDamageAmount.Size = New System.Drawing.Size(59, 20)
        Me.lblTotalDamageAmount.TabIndex = 31
        Me.lblTotalDamageAmount.Text = "100.00"
        Me.lblTotalDamageAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'line2
        '
        Me.line2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.line2.Location = New System.Drawing.Point(108, 177)
        Me.line2.Name = "line2"
        Me.line2.Size = New System.Drawing.Size(135, 2)
        Me.line2.TabIndex = 30
        '
        'lblTHDamagePercentage
        '
        Me.lblTHDamagePercentage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTHDamagePercentage.Location = New System.Drawing.Point(173, 148)
        Me.lblTHDamagePercentage.Name = "lblTHDamagePercentage"
        Me.lblTHDamagePercentage.Size = New System.Drawing.Size(67, 20)
        Me.lblTHDamagePercentage.TabIndex = 14
        Me.lblTHDamagePercentage.Text = "= 25.00%"
        Me.lblTHDamagePercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTHDamageAmount
        '
        Me.lblTHDamageAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTHDamageAmount.Location = New System.Drawing.Point(108, 148)
        Me.lblTHDamageAmount.Name = "lblTHDamageAmount"
        Me.lblTHDamageAmount.Size = New System.Drawing.Size(59, 20)
        Me.lblTHDamageAmount.TabIndex = 13
        Me.lblTHDamageAmount.Text = "25.00"
        Me.lblTHDamageAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTHDamage
        '
        Me.lblTHDamage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTHDamage.Location = New System.Drawing.Point(19, 148)
        Me.lblTHDamage.Name = "lblTHDamage"
        Me.lblTHDamage.Size = New System.Drawing.Size(83, 20)
        Me.lblTHDamage.TabIndex = 12
        Me.lblTHDamage.Text = "Thermal"
        Me.lblTHDamage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblKIDamagePercentage
        '
        Me.lblKIDamagePercentage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblKIDamagePercentage.Location = New System.Drawing.Point(173, 128)
        Me.lblKIDamagePercentage.Name = "lblKIDamagePercentage"
        Me.lblKIDamagePercentage.Size = New System.Drawing.Size(67, 20)
        Me.lblKIDamagePercentage.TabIndex = 11
        Me.lblKIDamagePercentage.Text = "= 25.00%"
        Me.lblKIDamagePercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblKIDamageAmount
        '
        Me.lblKIDamageAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblKIDamageAmount.Location = New System.Drawing.Point(108, 128)
        Me.lblKIDamageAmount.Name = "lblKIDamageAmount"
        Me.lblKIDamageAmount.Size = New System.Drawing.Size(59, 20)
        Me.lblKIDamageAmount.TabIndex = 10
        Me.lblKIDamageAmount.Text = "25.00"
        Me.lblKIDamageAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblKIDamage
        '
        Me.lblKIDamage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblKIDamage.Location = New System.Drawing.Point(19, 128)
        Me.lblKIDamage.Name = "lblKIDamage"
        Me.lblKIDamage.Size = New System.Drawing.Size(83, 20)
        Me.lblKIDamage.TabIndex = 9
        Me.lblKIDamage.Text = "Kinetic"
        Me.lblKIDamage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEXDamagePercentage
        '
        Me.lblEXDamagePercentage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEXDamagePercentage.Location = New System.Drawing.Point(173, 108)
        Me.lblEXDamagePercentage.Name = "lblEXDamagePercentage"
        Me.lblEXDamagePercentage.Size = New System.Drawing.Size(67, 20)
        Me.lblEXDamagePercentage.TabIndex = 8
        Me.lblEXDamagePercentage.Text = "= 25.00%"
        Me.lblEXDamagePercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblEXDamageAmount
        '
        Me.lblEXDamageAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEXDamageAmount.Location = New System.Drawing.Point(108, 108)
        Me.lblEXDamageAmount.Name = "lblEXDamageAmount"
        Me.lblEXDamageAmount.Size = New System.Drawing.Size(59, 20)
        Me.lblEXDamageAmount.TabIndex = 7
        Me.lblEXDamageAmount.Text = "25.00"
        Me.lblEXDamageAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblEXDamage
        '
        Me.lblEXDamage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEXDamage.Location = New System.Drawing.Point(19, 108)
        Me.lblEXDamage.Name = "lblEXDamage"
        Me.lblEXDamage.Size = New System.Drawing.Size(83, 20)
        Me.lblEXDamage.TabIndex = 6
        Me.lblEXDamage.Text = "Explosive"
        Me.lblEXDamage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEMDamagePercentage
        '
        Me.lblEMDamagePercentage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEMDamagePercentage.Location = New System.Drawing.Point(173, 88)
        Me.lblEMDamagePercentage.Name = "lblEMDamagePercentage"
        Me.lblEMDamagePercentage.Size = New System.Drawing.Size(67, 20)
        Me.lblEMDamagePercentage.TabIndex = 5
        Me.lblEMDamagePercentage.Text = "= 25.00%"
        Me.lblEMDamagePercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblEMDamageAmount
        '
        Me.lblEMDamageAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEMDamageAmount.Location = New System.Drawing.Point(108, 88)
        Me.lblEMDamageAmount.Name = "lblEMDamageAmount"
        Me.lblEMDamageAmount.Size = New System.Drawing.Size(59, 20)
        Me.lblEMDamageAmount.TabIndex = 4
        Me.lblEMDamageAmount.Text = "25.00"
        Me.lblEMDamageAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblEMDamage
        '
        Me.lblEMDamage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEMDamage.Location = New System.Drawing.Point(19, 88)
        Me.lblEMDamage.Name = "lblEMDamage"
        Me.lblEMDamage.Size = New System.Drawing.Size(83, 20)
        Me.lblEMDamage.TabIndex = 3
        Me.lblEMDamage.Text = "EM"
        Me.lblEMDamage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDamageTypes
        '
        Me.lblDamageTypes.AutoSize = True
        Me.lblDamageTypes.Location = New System.Drawing.Point(6, 66)
        Me.lblDamageTypes.Name = "lblDamageTypes"
        Me.lblDamageTypes.Size = New System.Drawing.Size(82, 13)
        Me.lblDamageTypes.TabIndex = 2
        Me.lblDamageTypes.Text = "Damage Types:"
        '
        'lblProfileTypeLbl
        '
        Me.lblProfileTypeLbl.AutoSize = True
        Me.lblProfileTypeLbl.Location = New System.Drawing.Point(6, 46)
        Me.lblProfileTypeLbl.Name = "lblProfileTypeLbl"
        Me.lblProfileTypeLbl.Size = New System.Drawing.Size(66, 13)
        Me.lblProfileTypeLbl.TabIndex = 1
        Me.lblProfileTypeLbl.Text = "Profile Type:"
        '
        'lblProfileNameLbl
        '
        Me.lblProfileNameLbl.AutoSize = True
        Me.lblProfileNameLbl.Location = New System.Drawing.Point(6, 26)
        Me.lblProfileNameLbl.Name = "lblProfileNameLbl"
        Me.lblProfileNameLbl.Size = New System.Drawing.Size(70, 13)
        Me.lblProfileNameLbl.TabIndex = 0
        Me.lblProfileNameLbl.Text = "Profile Name:"
        '
        'btnAddProfile
        '
        Me.btnAddProfile.Location = New System.Drawing.Point(398, 13)
        Me.btnAddProfile.Name = "btnAddProfile"
        Me.btnAddProfile.Size = New System.Drawing.Size(87, 23)
        Me.btnAddProfile.TabIndex = 2
        Me.btnAddProfile.Text = "Add Profile"
        Me.btnAddProfile.UseVisualStyleBackColor = True
        '
        'btnEditProfile
        '
        Me.btnEditProfile.Location = New System.Drawing.Point(398, 42)
        Me.btnEditProfile.Name = "btnEditProfile"
        Me.btnEditProfile.Size = New System.Drawing.Size(87, 23)
        Me.btnEditProfile.TabIndex = 3
        Me.btnEditProfile.Text = "Edit Profile"
        Me.btnEditProfile.UseVisualStyleBackColor = True
        '
        'btnDeleteProfile
        '
        Me.btnDeleteProfile.Location = New System.Drawing.Point(398, 71)
        Me.btnDeleteProfile.Name = "btnDeleteProfile"
        Me.btnDeleteProfile.Size = New System.Drawing.Size(87, 23)
        Me.btnDeleteProfile.TabIndex = 4
        Me.btnDeleteProfile.Text = "Delete Profile"
        Me.btnDeleteProfile.UseVisualStyleBackColor = True
        '
        'btnResetProfiles
        '
        Me.btnResetProfiles.Location = New System.Drawing.Point(397, 100)
        Me.btnResetProfiles.Name = "btnResetProfiles"
        Me.btnResetProfiles.Size = New System.Drawing.Size(88, 23)
        Me.btnResetProfiles.TabIndex = 5
        Me.btnResetProfiles.Text = "Reset Profiles"
        Me.btnResetProfiles.UseVisualStyleBackColor = True
        '
        'frmDamageProfiles
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(660, 543)
        Me.Controls.Add(Me.btnResetProfiles)
        Me.Controls.Add(Me.btnDeleteProfile)
        Me.Controls.Add(Me.btnEditProfile)
        Me.Controls.Add(Me.btnAddProfile)
        Me.Controls.Add(Me.gbProfileInfo)
        Me.Controls.Add(Me.lvwProfiles)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDamageProfiles"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Damage Profiles"
        Me.gbProfileInfo.ResumeLayout(False)
        Me.gbProfileInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvwProfiles As System.Windows.Forms.ListView
    Friend WithEvents colProfileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colProfileType As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbProfileInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblEMDamagePercentage As System.Windows.Forms.Label
    Friend WithEvents lblEMDamageAmount As System.Windows.Forms.Label
    Friend WithEvents lblEMDamage As System.Windows.Forms.Label
    Friend WithEvents lblDamageTypes As System.Windows.Forms.Label
    Friend WithEvents lblProfileTypeLbl As System.Windows.Forms.Label
    Friend WithEvents lblProfileNameLbl As System.Windows.Forms.Label
    Friend WithEvents lblTHDamagePercentage As System.Windows.Forms.Label
    Friend WithEvents lblTHDamageAmount As System.Windows.Forms.Label
    Friend WithEvents lblTHDamage As System.Windows.Forms.Label
    Friend WithEvents lblKIDamagePercentage As System.Windows.Forms.Label
    Friend WithEvents lblKIDamageAmount As System.Windows.Forms.Label
    Friend WithEvents lblKIDamage As System.Windows.Forms.Label
    Friend WithEvents lblEXDamagePercentage As System.Windows.Forms.Label
    Friend WithEvents lblEXDamageAmount As System.Windows.Forms.Label
    Friend WithEvents lblEXDamage As System.Windows.Forms.Label
    Friend WithEvents lblTotalDamagePercentage As System.Windows.Forms.Label
    Friend WithEvents lblTotalDamageAmount As System.Windows.Forms.Label
    Friend WithEvents line2 As System.Windows.Forms.Label
    Friend WithEvents lblProfileType As System.Windows.Forms.Label
    Friend WithEvents lblProfileName As System.Windows.Forms.Label
    Friend WithEvents lblDPS As System.Windows.Forms.Label
    Friend WithEvents lblDPSLbl As System.Windows.Forms.Label
    Friend WithEvents lblNPCName As System.Windows.Forms.Label
    Friend WithEvents lblNPCNameLbl As System.Windows.Forms.Label
    Friend WithEvents lblPilotName As System.Windows.Forms.Label
    Friend WithEvents lblPilotNameLbl As System.Windows.Forms.Label
    Friend WithEvents lblFittingName As System.Windows.Forms.Label
    Friend WithEvents lblFittingNameLbl As System.Windows.Forms.Label
    Friend WithEvents btnAddProfile As System.Windows.Forms.Button
    Friend WithEvents btnEditProfile As System.Windows.Forms.Button
    Friend WithEvents btnDeleteProfile As System.Windows.Forms.Button
    Friend WithEvents btnResetProfiles As System.Windows.Forms.Button
End Class
