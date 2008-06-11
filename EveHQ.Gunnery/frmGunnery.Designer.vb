<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGunnery
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
        Me.cboWeapon = New System.Windows.Forms.ComboBox
        Me.lvGuns = New System.Windows.Forms.ListView
        Me.colName = New System.Windows.Forms.ColumnHeader
        Me.colCap = New System.Windows.Forms.ColumnHeader
        Me.colOptimal = New System.Windows.Forms.ColumnHeader
        Me.colFalloff = New System.Windows.Forms.ColumnHeader
        Me.colTracking = New System.Windows.Forms.ColumnHeader
        Me.colEMDamage = New System.Windows.Forms.ColumnHeader
        Me.colExDamage = New System.Windows.Forms.ColumnHeader
        Me.colKiDamage = New System.Windows.Forms.ColumnHeader
        Me.colThDamage = New System.Windows.Forms.ColumnHeader
        Me.colDamage = New System.Windows.Forms.ColumnHeader
        Me.colDPS = New System.Windows.Forms.ColumnHeader
        Me.radEnergy = New System.Windows.Forms.RadioButton
        Me.radHybrid = New System.Windows.Forms.RadioButton
        Me.radProjectile = New System.Windows.Forms.RadioButton
        Me.lblWeaponType = New System.Windows.Forms.Label
        Me.lblWeapon = New System.Windows.Forms.Label
        Me.lblDamageMod = New System.Windows.Forms.Label
        Me.cboDM = New System.Windows.Forms.ComboBox
        Me.lblTrackingComputer = New System.Windows.Forms.Label
        Me.cboTC = New System.Windows.Forms.ComboBox
        Me.lblTrackingEnhancer = New System.Windows.Forms.Label
        Me.cboTE = New System.Windows.Forms.ComboBox
        Me.nudDM = New System.Windows.Forms.NumericUpDown
        Me.lblQDM = New System.Windows.Forms.Label
        Me.lblQTC = New System.Windows.Forms.Label
        Me.lblQTE = New System.Windows.Forms.Label
        Me.nudTC = New System.Windows.Forms.NumericUpDown
        Me.nudTE = New System.Windows.Forms.NumericUpDown
        Me.lblDMBonus = New System.Windows.Forms.Label
        Me.lblTCBonus = New System.Windows.Forms.Label
        Me.lblROFBonus = New System.Windows.Forms.Label
        Me.lblRangeBonus = New System.Windows.Forms.Label
        Me.lblCPU = New System.Windows.Forms.Label
        Me.lblPG = New System.Windows.Forms.Label
        Me.lblDmgMod = New System.Windows.Forms.Label
        Me.lblROF = New System.Windows.Forms.Label
        Me.gbStandardInfo = New System.Windows.Forms.GroupBox
        CType(Me.nudDM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTE, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbStandardInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboWeapon
        '
        Me.cboWeapon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWeapon.FormattingEnabled = True
        Me.cboWeapon.Location = New System.Drawing.Point(334, 10)
        Me.cboWeapon.Name = "cboWeapon"
        Me.cboWeapon.Size = New System.Drawing.Size(286, 21)
        Me.cboWeapon.Sorted = True
        Me.cboWeapon.TabIndex = 0
        '
        'lvGuns
        '
        Me.lvGuns.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvGuns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colCap, Me.colOptimal, Me.colFalloff, Me.colTracking, Me.colEMDamage, Me.colExDamage, Me.colKiDamage, Me.colThDamage, Me.colDamage, Me.colDPS})
        Me.lvGuns.FullRowSelect = True
        Me.lvGuns.Location = New System.Drawing.Point(12, 197)
        Me.lvGuns.Name = "lvGuns"
        Me.lvGuns.Size = New System.Drawing.Size(1039, 475)
        Me.lvGuns.TabIndex = 1
        Me.lvGuns.UseCompatibleStateImageBehavior = False
        Me.lvGuns.View = System.Windows.Forms.View.Details
        '
        'colName
        '
        Me.colName.Text = "Ammo"
        Me.colName.Width = 200
        '
        'colCap
        '
        Me.colCap.Text = "Cap Use"
        '
        'colOptimal
        '
        Me.colOptimal.Text = "Optimal"
        '
        'colFalloff
        '
        Me.colFalloff.Text = "Falloff"
        '
        'colTracking
        '
        Me.colTracking.Text = "Tracking"
        '
        'colEMDamage
        '
        Me.colEMDamage.Text = "EM"
        '
        'colExDamage
        '
        Me.colExDamage.Text = "Explosive"
        '
        'colKiDamage
        '
        Me.colKiDamage.Text = "Kinetic"
        '
        'colThDamage
        '
        Me.colThDamage.Text = "Thermal"
        '
        'colDamage
        '
        Me.colDamage.Text = "Damage"
        '
        'colDPS
        '
        Me.colDPS.Text = "DPS"
        '
        'radEnergy
        '
        Me.radEnergy.AutoSize = True
        Me.radEnergy.Location = New System.Drawing.Point(35, 38)
        Me.radEnergy.Name = "radEnergy"
        Me.radEnergy.Size = New System.Drawing.Size(107, 17)
        Me.radEnergy.TabIndex = 2
        Me.radEnergy.TabStop = True
        Me.radEnergy.Text = "Energy Weapons"
        Me.radEnergy.UseVisualStyleBackColor = True
        '
        'radHybrid
        '
        Me.radHybrid.AutoSize = True
        Me.radHybrid.Location = New System.Drawing.Point(35, 61)
        Me.radHybrid.Name = "radHybrid"
        Me.radHybrid.Size = New System.Drawing.Size(104, 17)
        Me.radHybrid.TabIndex = 3
        Me.radHybrid.TabStop = True
        Me.radHybrid.Text = "Hybrid Weapons"
        Me.radHybrid.UseVisualStyleBackColor = True
        '
        'radProjectile
        '
        Me.radProjectile.AutoSize = True
        Me.radProjectile.Location = New System.Drawing.Point(35, 84)
        Me.radProjectile.Name = "radProjectile"
        Me.radProjectile.Size = New System.Drawing.Size(117, 17)
        Me.radProjectile.TabIndex = 4
        Me.radProjectile.TabStop = True
        Me.radProjectile.Text = "Projectile Weapons"
        Me.radProjectile.UseVisualStyleBackColor = True
        '
        'lblWeaponType
        '
        Me.lblWeaponType.AutoSize = True
        Me.lblWeaponType.Location = New System.Drawing.Point(13, 13)
        Me.lblWeaponType.Name = "lblWeaponType"
        Me.lblWeaponType.Size = New System.Drawing.Size(111, 13)
        Me.lblWeaponType.TabIndex = 5
        Me.lblWeaponType.Text = "Select Weapon Type:"
        '
        'lblWeapon
        '
        Me.lblWeapon.AutoSize = True
        Me.lblWeapon.Location = New System.Drawing.Point(195, 13)
        Me.lblWeapon.Name = "lblWeapon"
        Me.lblWeapon.Size = New System.Drawing.Size(84, 13)
        Me.lblWeapon.TabIndex = 6
        Me.lblWeapon.Text = "Select Weapon:"
        '
        'lblDamageMod
        '
        Me.lblDamageMod.AutoSize = True
        Me.lblDamageMod.Location = New System.Drawing.Point(195, 41)
        Me.lblDamageMod.Name = "lblDamageMod"
        Me.lblDamageMod.Size = New System.Drawing.Size(107, 13)
        Me.lblDamageMod.TabIndex = 8
        Me.lblDamageMod.Text = "Select Damage Mod:"
        '
        'cboDM
        '
        Me.cboDM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDM.Enabled = False
        Me.cboDM.FormattingEnabled = True
        Me.cboDM.Location = New System.Drawing.Point(334, 38)
        Me.cboDM.Name = "cboDM"
        Me.cboDM.Size = New System.Drawing.Size(286, 21)
        Me.cboDM.Sorted = True
        Me.cboDM.TabIndex = 7
        '
        'lblTrackingComputer
        '
        Me.lblTrackingComputer.AutoSize = True
        Me.lblTrackingComputer.Location = New System.Drawing.Point(195, 68)
        Me.lblTrackingComputer.Name = "lblTrackingComputer"
        Me.lblTrackingComputer.Size = New System.Drawing.Size(133, 13)
        Me.lblTrackingComputer.TabIndex = 10
        Me.lblTrackingComputer.Text = "Select Tracking Computer:"
        '
        'cboTC
        '
        Me.cboTC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTC.Enabled = False
        Me.cboTC.FormattingEnabled = True
        Me.cboTC.Location = New System.Drawing.Point(334, 65)
        Me.cboTC.Name = "cboTC"
        Me.cboTC.Size = New System.Drawing.Size(286, 21)
        Me.cboTC.Sorted = True
        Me.cboTC.TabIndex = 9
        '
        'lblTrackingEnhancer
        '
        Me.lblTrackingEnhancer.AutoSize = True
        Me.lblTrackingEnhancer.Location = New System.Drawing.Point(195, 95)
        Me.lblTrackingEnhancer.Name = "lblTrackingEnhancer"
        Me.lblTrackingEnhancer.Size = New System.Drawing.Size(134, 13)
        Me.lblTrackingEnhancer.TabIndex = 12
        Me.lblTrackingEnhancer.Text = "Select Tracking Enhancer:"
        '
        'cboTE
        '
        Me.cboTE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTE.Enabled = False
        Me.cboTE.FormattingEnabled = True
        Me.cboTE.Location = New System.Drawing.Point(334, 92)
        Me.cboTE.Name = "cboTE"
        Me.cboTE.Size = New System.Drawing.Size(286, 21)
        Me.cboTE.Sorted = True
        Me.cboTE.TabIndex = 11
        '
        'nudDM
        '
        Me.nudDM.Enabled = False
        Me.nudDM.Location = New System.Drawing.Point(709, 38)
        Me.nudDM.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.nudDM.Name = "nudDM"
        Me.nudDM.Size = New System.Drawing.Size(40, 20)
        Me.nudDM.TabIndex = 13
        '
        'lblQDM
        '
        Me.lblQDM.AutoSize = True
        Me.lblQDM.Location = New System.Drawing.Point(654, 40)
        Me.lblQDM.Name = "lblQDM"
        Me.lblQDM.Size = New System.Drawing.Size(49, 13)
        Me.lblQDM.TabIndex = 14
        Me.lblQDM.Text = "Quantity:"
        '
        'lblQTC
        '
        Me.lblQTC.AutoSize = True
        Me.lblQTC.Location = New System.Drawing.Point(654, 68)
        Me.lblQTC.Name = "lblQTC"
        Me.lblQTC.Size = New System.Drawing.Size(49, 13)
        Me.lblQTC.TabIndex = 15
        Me.lblQTC.Text = "Quantity:"
        '
        'lblQTE
        '
        Me.lblQTE.AutoSize = True
        Me.lblQTE.Location = New System.Drawing.Point(654, 95)
        Me.lblQTE.Name = "lblQTE"
        Me.lblQTE.Size = New System.Drawing.Size(49, 13)
        Me.lblQTE.TabIndex = 16
        Me.lblQTE.Text = "Quantity:"
        '
        'nudTC
        '
        Me.nudTC.Enabled = False
        Me.nudTC.Location = New System.Drawing.Point(709, 66)
        Me.nudTC.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.nudTC.Name = "nudTC"
        Me.nudTC.Size = New System.Drawing.Size(40, 20)
        Me.nudTC.TabIndex = 17
        '
        'nudTE
        '
        Me.nudTE.Enabled = False
        Me.nudTE.Location = New System.Drawing.Point(709, 93)
        Me.nudTE.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.nudTE.Name = "nudTE"
        Me.nudTE.Size = New System.Drawing.Size(40, 20)
        Me.nudTE.TabIndex = 18
        '
        'lblDMBonus
        '
        Me.lblDMBonus.AutoSize = True
        Me.lblDMBonus.Location = New System.Drawing.Point(777, 40)
        Me.lblDMBonus.Name = "lblDMBonus"
        Me.lblDMBonus.Size = New System.Drawing.Size(110, 13)
        Me.lblDMBonus.TabIndex = 19
        Me.lblDMBonus.Text = "Effective Dmg Bonus:"
        '
        'lblTCBonus
        '
        Me.lblTCBonus.AutoSize = True
        Me.lblTCBonus.Location = New System.Drawing.Point(777, 80)
        Me.lblTCBonus.Name = "lblTCBonus"
        Me.lblTCBonus.Size = New System.Drawing.Size(130, 13)
        Me.lblTCBonus.TabIndex = 20
        Me.lblTCBonus.Text = "Effective Tracking Bonus:"
        '
        'lblROFBonus
        '
        Me.lblROFBonus.AutoSize = True
        Me.lblROFBonus.Location = New System.Drawing.Point(777, 60)
        Me.lblROFBonus.Name = "lblROFBonus"
        Me.lblROFBonus.Size = New System.Drawing.Size(110, 13)
        Me.lblROFBonus.TabIndex = 21
        Me.lblROFBonus.Text = "Effective ROF Bonus:"
        '
        'lblRangeBonus
        '
        Me.lblRangeBonus.AutoSize = True
        Me.lblRangeBonus.Location = New System.Drawing.Point(777, 100)
        Me.lblRangeBonus.Name = "lblRangeBonus"
        Me.lblRangeBonus.Size = New System.Drawing.Size(120, 13)
        Me.lblRangeBonus.TabIndex = 22
        Me.lblRangeBonus.Text = "Effective Range Bonus:"
        '
        'lblCPU
        '
        Me.lblCPU.AutoSize = True
        Me.lblCPU.Location = New System.Drawing.Point(20, 27)
        Me.lblCPU.Name = "lblCPU"
        Me.lblCPU.Size = New System.Drawing.Size(32, 13)
        Me.lblCPU.TabIndex = 23
        Me.lblCPU.Text = "CPU:"
        '
        'lblPG
        '
        Me.lblPG.AutoSize = True
        Me.lblPG.Location = New System.Drawing.Point(183, 27)
        Me.lblPG.Name = "lblPG"
        Me.lblPG.Size = New System.Drawing.Size(25, 13)
        Me.lblPG.TabIndex = 24
        Me.lblPG.Text = "PG:"
        '
        'lblDmgMod
        '
        Me.lblDmgMod.AutoSize = True
        Me.lblDmgMod.Location = New System.Drawing.Point(319, 27)
        Me.lblDmgMod.Name = "lblDmgMod"
        Me.lblDmgMod.Size = New System.Drawing.Size(74, 13)
        Me.lblDmgMod.TabIndex = 26
        Me.lblDmgMod.Text = "Damage Mod:"
        '
        'lblROF
        '
        Me.lblROF.AutoSize = True
        Me.lblROF.Location = New System.Drawing.Point(514, 27)
        Me.lblROF.Name = "lblROF"
        Me.lblROF.Size = New System.Drawing.Size(32, 13)
        Me.lblROF.TabIndex = 27
        Me.lblROF.Text = "ROF:"
        '
        'gbStandardInfo
        '
        Me.gbStandardInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbStandardInfo.Controls.Add(Me.lblPG)
        Me.gbStandardInfo.Controls.Add(Me.lblROF)
        Me.gbStandardInfo.Controls.Add(Me.lblCPU)
        Me.gbStandardInfo.Controls.Add(Me.lblDmgMod)
        Me.gbStandardInfo.Location = New System.Drawing.Point(12, 133)
        Me.gbStandardInfo.Name = "gbStandardInfo"
        Me.gbStandardInfo.Size = New System.Drawing.Size(1039, 58)
        Me.gbStandardInfo.TabIndex = 28
        Me.gbStandardInfo.TabStop = False
        Me.gbStandardInfo.Text = "Standard Weapon Attributes"
        '
        'frmGunnery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1063, 684)
        Me.Controls.Add(Me.gbStandardInfo)
        Me.Controls.Add(Me.lblRangeBonus)
        Me.Controls.Add(Me.lblROFBonus)
        Me.Controls.Add(Me.lblTCBonus)
        Me.Controls.Add(Me.lblDMBonus)
        Me.Controls.Add(Me.nudTE)
        Me.Controls.Add(Me.nudTC)
        Me.Controls.Add(Me.lblQTE)
        Me.Controls.Add(Me.lblQTC)
        Me.Controls.Add(Me.lblQDM)
        Me.Controls.Add(Me.nudDM)
        Me.Controls.Add(Me.lblTrackingEnhancer)
        Me.Controls.Add(Me.cboTE)
        Me.Controls.Add(Me.lblTrackingComputer)
        Me.Controls.Add(Me.cboTC)
        Me.Controls.Add(Me.lblDamageMod)
        Me.Controls.Add(Me.cboDM)
        Me.Controls.Add(Me.lblWeapon)
        Me.Controls.Add(Me.lblWeaponType)
        Me.Controls.Add(Me.radProjectile)
        Me.Controls.Add(Me.radHybrid)
        Me.Controls.Add(Me.radEnergy)
        Me.Controls.Add(Me.lvGuns)
        Me.Controls.Add(Me.cboWeapon)
        Me.Name = "frmGunnery"
        Me.Text = "EveHQ Gunnery Tool"
        CType(Me.nudDM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTE, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbStandardInfo.ResumeLayout(False)
        Me.gbStandardInfo.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboWeapon As System.Windows.Forms.ComboBox
    Friend WithEvents lvGuns As System.Windows.Forms.ListView
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCap As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOptimal As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTracking As System.Windows.Forms.ColumnHeader
    Friend WithEvents radEnergy As System.Windows.Forms.RadioButton
    Friend WithEvents radHybrid As System.Windows.Forms.RadioButton
    Friend WithEvents radProjectile As System.Windows.Forms.RadioButton
    Friend WithEvents lblWeaponType As System.Windows.Forms.Label
    Friend WithEvents lblWeapon As System.Windows.Forms.Label
    Friend WithEvents colEMDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colExDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colKiDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colThDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDPS As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblDamageMod As System.Windows.Forms.Label
    Friend WithEvents cboDM As System.Windows.Forms.ComboBox
    Friend WithEvents lblTrackingComputer As System.Windows.Forms.Label
    Friend WithEvents cboTC As System.Windows.Forms.ComboBox
    Friend WithEvents lblTrackingEnhancer As System.Windows.Forms.Label
    Friend WithEvents cboTE As System.Windows.Forms.ComboBox
    Friend WithEvents nudDM As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblQDM As System.Windows.Forms.Label
    Friend WithEvents lblQTC As System.Windows.Forms.Label
    Friend WithEvents lblQTE As System.Windows.Forms.Label
    Friend WithEvents nudTC As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudTE As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblDMBonus As System.Windows.Forms.Label
    Friend WithEvents lblTCBonus As System.Windows.Forms.Label
    Friend WithEvents lblROFBonus As System.Windows.Forms.Label
    Friend WithEvents lblRangeBonus As System.Windows.Forms.Label
    Friend WithEvents lblCPU As System.Windows.Forms.Label
    Friend WithEvents lblPG As System.Windows.Forms.Label
    Friend WithEvents lblDmgMod As System.Windows.Forms.Label
    Friend WithEvents lblROF As System.Windows.Forms.Label
    Friend WithEvents gbStandardInfo As System.Windows.Forms.GroupBox
    Friend WithEvents colFalloff As System.Windows.Forms.ColumnHeader
End Class
