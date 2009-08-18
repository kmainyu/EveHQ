<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShipInfoControl
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShipInfoControl))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblImplants = New System.Windows.Forms.Label
        Me.cboImplants = New System.Windows.Forms.ComboBox
        Me.gbDefence = New System.Windows.Forms.GroupBox
        Me.lblTankAbility = New System.Windows.Forms.Label
        Me.btnEditProfiles = New System.Windows.Forms.Button
        Me.cboDamageProfiles = New System.Windows.Forms.ComboBox
        Me.lblEffectiveHP = New System.Windows.Forms.Label
        Me.lblStructureHP = New System.Windows.Forms.Label
        Me.pbStructureHP = New System.Windows.Forms.PictureBox
        Me.lblStructureThermal = New System.Windows.Forms.Label
        Me.lblStructureExplosive = New System.Windows.Forms.Label
        Me.lblArmorHP = New System.Windows.Forms.Label
        Me.lblStructureKinetic = New System.Windows.Forms.Label
        Me.pbArmorHP = New System.Windows.Forms.PictureBox
        Me.lblStructureEM = New System.Windows.Forms.Label
        Me.lblArmorThermal = New System.Windows.Forms.Label
        Me.lblShieldEM = New System.Windows.Forms.Label
        Me.lblArmorExplosive = New System.Windows.Forms.Label
        Me.lblArmorKinetic = New System.Windows.Forms.Label
        Me.lblShieldKinetic = New System.Windows.Forms.Label
        Me.lblArmorEM = New System.Windows.Forms.Label
        Me.lblShieldExplosive = New System.Windows.Forms.Label
        Me.lblShieldThermal = New System.Windows.Forms.Label
        Me.lblShieldHP = New System.Windows.Forms.Label
        Me.pbShieldHP = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.gbDamage = New System.Windows.Forms.GroupBox
        Me.lblMining = New System.Windows.Forms.Label
        Me.pbMining = New System.Windows.Forms.PictureBox
        Me.lblDamage = New System.Windows.Forms.Label
        Me.pbDamage = New System.Windows.Forms.PictureBox
        Me.progCalibration = New VistaStyleProgressBar.ProgressBar
        Me.progPG = New VistaStyleProgressBar.ProgressBar
        Me.progCPU = New VistaStyleProgressBar.ProgressBar
        Me.line2 = New System.Windows.Forms.Label
        Me.btnLog = New System.Windows.Forms.Button
        Me.btnSkills = New System.Windows.Forms.Button
        Me.btnTargetSpeed = New System.Windows.Forms.Button
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.line1 = New System.Windows.Forms.Label
        Me.btnDoomsdayCheck = New System.Windows.Forms.Button
        Me.gbCargo = New System.Windows.Forms.GroupBox
        Me.lblDroneControl = New System.Windows.Forms.Label
        Me.pbDroneControl = New System.Windows.Forms.PictureBox
        Me.lblDroneBandwidth = New System.Windows.Forms.Label
        Me.pbDroneBandwidth = New System.Windows.Forms.PictureBox
        Me.lblDroneBay = New System.Windows.Forms.Label
        Me.pbDroneBay = New System.Windows.Forms.PictureBox
        Me.lblCargoBay = New System.Windows.Forms.Label
        Me.pbCargoBay = New System.Windows.Forms.PictureBox
        Me.gbPropulsion = New System.Windows.Forms.GroupBox
        Me.lblAlignTime = New System.Windows.Forms.Label
        Me.pbAlignTime = New System.Windows.Forms.PictureBox
        Me.lblInertia = New System.Windows.Forms.Label
        Me.pbInertia = New System.Windows.Forms.PictureBox
        Me.lblWarpSpeed = New System.Windows.Forms.Label
        Me.pbWarpSpeed = New System.Windows.Forms.PictureBox
        Me.lblSpeed = New System.Windows.Forms.Label
        Me.pbSpeed = New System.Windows.Forms.PictureBox
        Me.gbCapacitor = New System.Windows.Forms.GroupBox
        Me.lblCapBalN = New System.Windows.Forms.Label
        Me.lblCapBalP = New System.Windows.Forms.Label
        Me.pbCapBal = New System.Windows.Forms.PictureBox
        Me.lblCapPeak = New System.Windows.Forms.Label
        Me.pbCapPeak = New System.Windows.Forms.PictureBox
        Me.lblCapRecharge = New System.Windows.Forms.Label
        Me.pbCapRecharge = New System.Windows.Forms.PictureBox
        Me.lblCapacitor = New System.Windows.Forms.Label
        Me.pbCapacitor = New System.Windows.Forms.PictureBox
        Me.gbTargeting = New System.Windows.Forms.GroupBox
        Me.lblTargets = New System.Windows.Forms.Label
        Me.pbTargets = New System.Windows.Forms.PictureBox
        Me.lblSigRadius = New System.Windows.Forms.Label
        Me.pbSigRadius = New System.Windows.Forms.PictureBox
        Me.lblSensorStrength = New System.Windows.Forms.Label
        Me.pbSensorStrength = New System.Windows.Forms.PictureBox
        Me.lblScanResolution = New System.Windows.Forms.Label
        Me.pbScanResolution = New System.Windows.Forms.PictureBox
        Me.lblTargetRange = New System.Windows.Forms.Label
        Me.pbTargetRange = New System.Windows.Forms.PictureBox
        Me.lblCalibration = New System.Windows.Forms.Label
        Me.pbCalibration = New System.Windows.Forms.PictureBox
        Me.lblPG = New System.Windows.Forms.Label
        Me.lblCPU = New System.Windows.Forms.Label
        Me.pbPG = New System.Windows.Forms.PictureBox
        Me.pbCPU = New System.Windows.Forms.PictureBox
        Me.lblPGReqd = New System.Windows.Forms.Label
        Me.lblCPUReqd = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        Me.gbDefence.SuspendLayout()
        CType(Me.pbStructureHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDamage.SuspendLayout()
        CType(Me.pbMining, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDamage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbCargo.SuspendLayout()
        CType(Me.pbDroneControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDroneBandwidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDroneBay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCargoBay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPropulsion.SuspendLayout()
        CType(Me.pbAlignTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbInertia, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbWarpSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbCapacitor.SuspendLayout()
        CType(Me.pbCapBal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCapPeak, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCapRecharge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCapacitor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbTargeting.SuspendLayout()
        CType(Me.pbTargets, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSigRadius, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSensorStrength, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbScanResolution, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbTargetRange, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCalibration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCPU, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.lblImplants)
        Me.Panel1.Controls.Add(Me.cboImplants)
        Me.Panel1.Controls.Add(Me.gbDefence)
        Me.Panel1.Controls.Add(Me.gbDamage)
        Me.Panel1.Controls.Add(Me.progCalibration)
        Me.Panel1.Controls.Add(Me.progPG)
        Me.Panel1.Controls.Add(Me.progCPU)
        Me.Panel1.Controls.Add(Me.line2)
        Me.Panel1.Controls.Add(Me.btnLog)
        Me.Panel1.Controls.Add(Me.btnSkills)
        Me.Panel1.Controls.Add(Me.btnTargetSpeed)
        Me.Panel1.Controls.Add(Me.cboPilots)
        Me.Panel1.Controls.Add(Me.lblPilot)
        Me.Panel1.Controls.Add(Me.line1)
        Me.Panel1.Controls.Add(Me.btnDoomsdayCheck)
        Me.Panel1.Controls.Add(Me.gbCargo)
        Me.Panel1.Controls.Add(Me.gbPropulsion)
        Me.Panel1.Controls.Add(Me.gbCapacitor)
        Me.Panel1.Controls.Add(Me.gbTargeting)
        Me.Panel1.Controls.Add(Me.lblCalibration)
        Me.Panel1.Controls.Add(Me.pbCalibration)
        Me.Panel1.Controls.Add(Me.lblPG)
        Me.Panel1.Controls.Add(Me.lblCPU)
        Me.Panel1.Controls.Add(Me.pbPG)
        Me.Panel1.Controls.Add(Me.pbCPU)
        Me.Panel1.Controls.Add(Me.lblPGReqd)
        Me.Panel1.Controls.Add(Me.lblCPUReqd)
        Me.Panel1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(270, 732)
        Me.Panel1.TabIndex = 0
        '
        'lblImplants
        '
        Me.lblImplants.AutoSize = True
        Me.lblImplants.Location = New System.Drawing.Point(6, 38)
        Me.lblImplants.Name = "lblImplants"
        Me.lblImplants.Size = New System.Drawing.Size(34, 13)
        Me.lblImplants.TabIndex = 36
        Me.lblImplants.Text = "Imps:"
        '
        'cboImplants
        '
        Me.cboImplants.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboImplants.FormattingEnabled = True
        Me.cboImplants.Location = New System.Drawing.Point(42, 35)
        Me.cboImplants.Name = "cboImplants"
        Me.cboImplants.Size = New System.Drawing.Size(165, 21)
        Me.cboImplants.Sorted = True
        Me.cboImplants.TabIndex = 35
        '
        'gbDefence
        '
        Me.gbDefence.Controls.Add(Me.lblTankAbility)
        Me.gbDefence.Controls.Add(Me.btnEditProfiles)
        Me.gbDefence.Controls.Add(Me.cboDamageProfiles)
        Me.gbDefence.Controls.Add(Me.lblEffectiveHP)
        Me.gbDefence.Controls.Add(Me.lblStructureHP)
        Me.gbDefence.Controls.Add(Me.pbStructureHP)
        Me.gbDefence.Controls.Add(Me.lblStructureThermal)
        Me.gbDefence.Controls.Add(Me.lblStructureExplosive)
        Me.gbDefence.Controls.Add(Me.lblArmorHP)
        Me.gbDefence.Controls.Add(Me.lblStructureKinetic)
        Me.gbDefence.Controls.Add(Me.pbArmorHP)
        Me.gbDefence.Controls.Add(Me.lblStructureEM)
        Me.gbDefence.Controls.Add(Me.lblArmorThermal)
        Me.gbDefence.Controls.Add(Me.lblShieldEM)
        Me.gbDefence.Controls.Add(Me.lblArmorExplosive)
        Me.gbDefence.Controls.Add(Me.lblArmorKinetic)
        Me.gbDefence.Controls.Add(Me.lblShieldKinetic)
        Me.gbDefence.Controls.Add(Me.lblArmorEM)
        Me.gbDefence.Controls.Add(Me.lblShieldExplosive)
        Me.gbDefence.Controls.Add(Me.lblShieldThermal)
        Me.gbDefence.Controls.Add(Me.lblShieldHP)
        Me.gbDefence.Controls.Add(Me.pbShieldHP)
        Me.gbDefence.Controls.Add(Me.PictureBox1)
        Me.gbDefence.Controls.Add(Me.PictureBox2)
        Me.gbDefence.Controls.Add(Me.PictureBox3)
        Me.gbDefence.Controls.Add(Me.PictureBox4)
        Me.gbDefence.Location = New System.Drawing.Point(6, 187)
        Me.gbDefence.Name = "gbDefence"
        Me.gbDefence.Size = New System.Drawing.Size(240, 162)
        Me.gbDefence.TabIndex = 34
        Me.gbDefence.TabStop = False
        Me.gbDefence.Text = "Ship Defence"
        '
        'lblTankAbility
        '
        Me.lblTankAbility.AutoSize = True
        Me.lblTankAbility.Location = New System.Drawing.Point(11, 146)
        Me.lblTankAbility.Name = "lblTankAbility"
        Me.lblTankAbility.Size = New System.Drawing.Size(97, 13)
        Me.lblTankAbility.TabIndex = 29
        Me.lblTankAbility.Text = "Tank Ability: 0,000"
        Me.ToolTip1.SetToolTip(Me.lblTankAbility, "Effective Hitpoints")
        '
        'btnEditProfiles
        '
        Me.btnEditProfiles.Location = New System.Drawing.Point(202, 17)
        Me.btnEditProfiles.Margin = New System.Windows.Forms.Padding(1)
        Me.btnEditProfiles.Name = "btnEditProfiles"
        Me.btnEditProfiles.Size = New System.Drawing.Size(34, 21)
        Me.btnEditProfiles.TabIndex = 28
        Me.btnEditProfiles.Text = "Edit"
        Me.btnEditProfiles.UseVisualStyleBackColor = True
        '
        'cboDamageProfiles
        '
        Me.cboDamageProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDamageProfiles.FormattingEnabled = True
        Me.cboDamageProfiles.Location = New System.Drawing.Point(7, 17)
        Me.cboDamageProfiles.Name = "cboDamageProfiles"
        Me.cboDamageProfiles.Size = New System.Drawing.Size(195, 21)
        Me.cboDamageProfiles.TabIndex = 27
        '
        'lblEffectiveHP
        '
        Me.lblEffectiveHP.AutoSize = True
        Me.lblEffectiveHP.Location = New System.Drawing.Point(11, 133)
        Me.lblEffectiveHP.Name = "lblEffectiveHP"
        Me.lblEffectiveHP.Size = New System.Drawing.Size(113, 13)
        Me.lblEffectiveHP.TabIndex = 21
        Me.lblEffectiveHP.Text = "Effective HP: 000,000"
        Me.ToolTip1.SetToolTip(Me.lblEffectiveHP, "Effective Hitpoints")
        '
        'lblStructureHP
        '
        Me.lblStructureHP.AutoSize = True
        Me.lblStructureHP.ForeColor = System.Drawing.Color.Maroon
        Me.lblStructureHP.Location = New System.Drawing.Point(28, 112)
        Me.lblStructureHP.Name = "lblStructureHP"
        Me.lblStructureHP.Size = New System.Drawing.Size(47, 13)
        Me.lblStructureHP.TabIndex = 14
        Me.lblStructureHP.Text = "000,000"
        Me.ToolTip1.SetToolTip(Me.lblStructureHP, "Structure Hitpoints")
        '
        'pbStructureHP
        '
        Me.pbStructureHP.Image = Global.EveHQ.HQF.My.Resources.Resources.imgStructure
        Me.pbStructureHP.Location = New System.Drawing.Point(7, 106)
        Me.pbStructureHP.Name = "pbStructureHP"
        Me.pbStructureHP.Size = New System.Drawing.Size(24, 24)
        Me.pbStructureHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbStructureHP.TabIndex = 12
        Me.pbStructureHP.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbStructureHP, "Structure Hitpoints")
        '
        'lblStructureThermal
        '
        Me.lblStructureThermal.AutoSize = True
        Me.lblStructureThermal.ForeColor = System.Drawing.Color.Maroon
        Me.lblStructureThermal.Location = New System.Drawing.Point(185, 116)
        Me.lblStructureThermal.Name = "lblStructureThermal"
        Me.lblStructureThermal.Size = New System.Drawing.Size(24, 13)
        Me.lblStructureThermal.TabIndex = 11
        Me.lblStructureThermal.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblStructureThermal, "Structure Thermal Resistance")
        '
        'lblStructureExplosive
        '
        Me.lblStructureExplosive.AutoSize = True
        Me.lblStructureExplosive.ForeColor = System.Drawing.Color.Maroon
        Me.lblStructureExplosive.Location = New System.Drawing.Point(107, 117)
        Me.lblStructureExplosive.Name = "lblStructureExplosive"
        Me.lblStructureExplosive.Size = New System.Drawing.Size(24, 13)
        Me.lblStructureExplosive.TabIndex = 8
        Me.lblStructureExplosive.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblStructureExplosive, "Structure Explosive Resistance")
        '
        'lblArmorHP
        '
        Me.lblArmorHP.AutoSize = True
        Me.lblArmorHP.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblArmorHP.Location = New System.Drawing.Point(28, 82)
        Me.lblArmorHP.Name = "lblArmorHP"
        Me.lblArmorHP.Size = New System.Drawing.Size(47, 13)
        Me.lblArmorHP.TabIndex = 14
        Me.lblArmorHP.Text = "000,000"
        Me.ToolTip1.SetToolTip(Me.lblArmorHP, "Armor Hitpoints")
        '
        'lblStructureKinetic
        '
        Me.lblStructureKinetic.AutoSize = True
        Me.lblStructureKinetic.ForeColor = System.Drawing.Color.Maroon
        Me.lblStructureKinetic.Location = New System.Drawing.Point(185, 71)
        Me.lblStructureKinetic.Name = "lblStructureKinetic"
        Me.lblStructureKinetic.Size = New System.Drawing.Size(24, 13)
        Me.lblStructureKinetic.TabIndex = 5
        Me.lblStructureKinetic.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblStructureKinetic, "Structure Kinetic Resistance")
        '
        'pbArmorHP
        '
        Me.pbArmorHP.Image = Global.EveHQ.HQF.My.Resources.Resources.imgArmor
        Me.pbArmorHP.Location = New System.Drawing.Point(7, 76)
        Me.pbArmorHP.Name = "pbArmorHP"
        Me.pbArmorHP.Size = New System.Drawing.Size(24, 24)
        Me.pbArmorHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbArmorHP.TabIndex = 12
        Me.pbArmorHP.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbArmorHP, "Armor Hitpoints")
        '
        'lblStructureEM
        '
        Me.lblStructureEM.AutoSize = True
        Me.lblStructureEM.ForeColor = System.Drawing.Color.Maroon
        Me.lblStructureEM.Location = New System.Drawing.Point(107, 71)
        Me.lblStructureEM.Name = "lblStructureEM"
        Me.lblStructureEM.Size = New System.Drawing.Size(24, 13)
        Me.lblStructureEM.TabIndex = 2
        Me.lblStructureEM.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblStructureEM, "Structure EM Resistance")
        '
        'lblArmorThermal
        '
        Me.lblArmorThermal.AutoSize = True
        Me.lblArmorThermal.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblArmorThermal.Location = New System.Drawing.Point(185, 103)
        Me.lblArmorThermal.Name = "lblArmorThermal"
        Me.lblArmorThermal.Size = New System.Drawing.Size(24, 13)
        Me.lblArmorThermal.TabIndex = 11
        Me.lblArmorThermal.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblArmorThermal, "Armor Thermal Resistance")
        '
        'lblShieldEM
        '
        Me.lblShieldEM.AutoSize = True
        Me.lblShieldEM.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblShieldEM.Location = New System.Drawing.Point(107, 45)
        Me.lblShieldEM.Name = "lblShieldEM"
        Me.lblShieldEM.Size = New System.Drawing.Size(24, 13)
        Me.lblShieldEM.TabIndex = 2
        Me.lblShieldEM.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblShieldEM, "Shield EM Resistance")
        '
        'lblArmorExplosive
        '
        Me.lblArmorExplosive.AutoSize = True
        Me.lblArmorExplosive.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblArmorExplosive.Location = New System.Drawing.Point(107, 103)
        Me.lblArmorExplosive.Name = "lblArmorExplosive"
        Me.lblArmorExplosive.Size = New System.Drawing.Size(24, 13)
        Me.lblArmorExplosive.TabIndex = 8
        Me.lblArmorExplosive.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblArmorExplosive, "Armor Explosive Resistance")
        '
        'lblArmorKinetic
        '
        Me.lblArmorKinetic.AutoSize = True
        Me.lblArmorKinetic.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblArmorKinetic.Location = New System.Drawing.Point(185, 58)
        Me.lblArmorKinetic.Name = "lblArmorKinetic"
        Me.lblArmorKinetic.Size = New System.Drawing.Size(24, 13)
        Me.lblArmorKinetic.TabIndex = 5
        Me.lblArmorKinetic.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblArmorKinetic, "Armor Kinetic Resistance")
        '
        'lblShieldKinetic
        '
        Me.lblShieldKinetic.AutoSize = True
        Me.lblShieldKinetic.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblShieldKinetic.Location = New System.Drawing.Point(185, 44)
        Me.lblShieldKinetic.Name = "lblShieldKinetic"
        Me.lblShieldKinetic.Size = New System.Drawing.Size(24, 13)
        Me.lblShieldKinetic.TabIndex = 5
        Me.lblShieldKinetic.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblShieldKinetic, "Shield Kinetic Resistance")
        '
        'lblArmorEM
        '
        Me.lblArmorEM.AutoSize = True
        Me.lblArmorEM.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblArmorEM.Location = New System.Drawing.Point(107, 58)
        Me.lblArmorEM.Name = "lblArmorEM"
        Me.lblArmorEM.Size = New System.Drawing.Size(24, 13)
        Me.lblArmorEM.TabIndex = 2
        Me.lblArmorEM.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblArmorEM, "Armor EM Resistance")
        '
        'lblShieldExplosive
        '
        Me.lblShieldExplosive.AutoSize = True
        Me.lblShieldExplosive.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblShieldExplosive.Location = New System.Drawing.Point(107, 90)
        Me.lblShieldExplosive.Name = "lblShieldExplosive"
        Me.lblShieldExplosive.Size = New System.Drawing.Size(24, 13)
        Me.lblShieldExplosive.TabIndex = 8
        Me.lblShieldExplosive.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblShieldExplosive, "Shield Explosive Resistance")
        '
        'lblShieldThermal
        '
        Me.lblShieldThermal.AutoSize = True
        Me.lblShieldThermal.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblShieldThermal.Location = New System.Drawing.Point(185, 90)
        Me.lblShieldThermal.Name = "lblShieldThermal"
        Me.lblShieldThermal.Size = New System.Drawing.Size(24, 13)
        Me.lblShieldThermal.TabIndex = 11
        Me.lblShieldThermal.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblShieldThermal, "Shield Thermal Resistance")
        '
        'lblShieldHP
        '
        Me.lblShieldHP.AutoSize = True
        Me.lblShieldHP.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblShieldHP.Location = New System.Drawing.Point(28, 51)
        Me.lblShieldHP.Name = "lblShieldHP"
        Me.lblShieldHP.Size = New System.Drawing.Size(47, 13)
        Me.lblShieldHP.TabIndex = 14
        Me.lblShieldHP.Text = "000,000"
        Me.ToolTip1.SetToolTip(Me.lblShieldHP, "Shield Hitpoints")
        '
        'pbShieldHP
        '
        Me.pbShieldHP.Image = Global.EveHQ.HQF.My.Resources.Resources.imgShield
        Me.pbShieldHP.Location = New System.Drawing.Point(7, 45)
        Me.pbShieldHP.Name = "pbShieldHP"
        Me.pbShieldHP.Size = New System.Drawing.Size(24, 24)
        Me.pbShieldHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbShieldHP.TabIndex = 12
        Me.pbShieldHP.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShieldHP, "Shield Hitpoints")
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.EveHQ.HQF.My.Resources.Resources.imgEMResist
        Me.PictureBox1.Location = New System.Drawing.Point(85, 45)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 40)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 22
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, "EM Resistance")
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.EveHQ.HQF.My.Resources.Resources.imgKineticResist
        Me.PictureBox2.Location = New System.Drawing.Point(161, 44)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(32, 40)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 23
        Me.PictureBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox2, "Kinetic Resistance")
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.EveHQ.HQF.My.Resources.Resources.imgExplosiveResist
        Me.PictureBox3.Location = New System.Drawing.Point(85, 90)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(32, 40)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 24
        Me.PictureBox3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox3, "Explosive Resistance")
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.EveHQ.HQF.My.Resources.Resources.imgThermalResist
        Me.PictureBox4.Location = New System.Drawing.Point(161, 90)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(32, 40)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 25
        Me.PictureBox4.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox4, "Thermal Resistance")
        '
        'gbDamage
        '
        Me.gbDamage.Controls.Add(Me.lblMining)
        Me.gbDamage.Controls.Add(Me.pbMining)
        Me.gbDamage.Controls.Add(Me.lblDamage)
        Me.gbDamage.Controls.Add(Me.pbDamage)
        Me.gbDamage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbDamage.Location = New System.Drawing.Point(6, 423)
        Me.gbDamage.Name = "gbDamage"
        Me.gbDamage.Size = New System.Drawing.Size(240, 70)
        Me.gbDamage.TabIndex = 33
        Me.gbDamage.TabStop = False
        Me.gbDamage.Text = "Damage / Mining"
        Me.ToolTip1.SetToolTip(Me.gbDamage, "Damage Information")
        '
        'lblMining
        '
        Me.lblMining.AutoSize = True
        Me.lblMining.Location = New System.Drawing.Point(29, 47)
        Me.lblMining.Name = "lblMining"
        Me.lblMining.Size = New System.Drawing.Size(53, 13)
        Me.lblMining.TabIndex = 12
        Me.lblMining.Text = "000 / 000"
        Me.ToolTip1.SetToolTip(Me.lblMining, "Mining (Cycle / Rate)")
        '
        'pbMining
        '
        Me.pbMining.Image = Global.EveHQ.HQF.My.Resources.Resources.imgMining
        Me.pbMining.Location = New System.Drawing.Point(5, 42)
        Me.pbMining.Name = "pbMining"
        Me.pbMining.Size = New System.Drawing.Size(24, 24)
        Me.pbMining.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbMining.TabIndex = 11
        Me.pbMining.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbMining, "Mining (Cycle / Rate)")
        '
        'lblDamage
        '
        Me.lblDamage.AutoSize = True
        Me.lblDamage.Location = New System.Drawing.Point(29, 24)
        Me.lblDamage.Name = "lblDamage"
        Me.lblDamage.Size = New System.Drawing.Size(53, 13)
        Me.lblDamage.TabIndex = 10
        Me.lblDamage.Text = "000 / 000"
        Me.ToolTip1.SetToolTip(Me.lblDamage, "Damage (Volley / DPS)")
        '
        'pbDamage
        '
        Me.pbDamage.Image = Global.EveHQ.HQF.My.Resources.Resources.imgTurretSlots
        Me.pbDamage.Location = New System.Drawing.Point(5, 19)
        Me.pbDamage.Name = "pbDamage"
        Me.pbDamage.Size = New System.Drawing.Size(24, 24)
        Me.pbDamage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbDamage.TabIndex = 0
        Me.pbDamage.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbDamage, "Damage (Volley / DPS)")
        '
        'progCalibration
        '
        Me.progCalibration.BackColor = System.Drawing.Color.Transparent
        Me.progCalibration.EndColor = System.Drawing.Color.LimeGreen
        Me.progCalibration.GlowColor = System.Drawing.Color.LightGreen
        Me.progCalibration.Location = New System.Drawing.Point(36, 171)
        Me.progCalibration.Name = "progCalibration"
        Me.progCalibration.Size = New System.Drawing.Size(206, 10)
        Me.progCalibration.StartColor = System.Drawing.Color.LimeGreen
        Me.progCalibration.TabIndex = 32
        Me.progCalibration.Value = 50
        '
        'progPG
        '
        Me.progPG.BackColor = System.Drawing.Color.Transparent
        Me.progPG.EndColor = System.Drawing.Color.LimeGreen
        Me.progPG.GlowColor = System.Drawing.Color.LightGreen
        Me.progPG.Location = New System.Drawing.Point(36, 147)
        Me.progPG.Name = "progPG"
        Me.progPG.Size = New System.Drawing.Size(206, 10)
        Me.progPG.StartColor = System.Drawing.Color.LimeGreen
        Me.progPG.TabIndex = 31
        Me.progPG.Value = 50
        '
        'progCPU
        '
        Me.progCPU.BackColor = System.Drawing.Color.Transparent
        Me.progCPU.EndColor = System.Drawing.Color.LimeGreen
        Me.progCPU.GlowColor = System.Drawing.Color.LightGreen
        Me.progCPU.Location = New System.Drawing.Point(36, 123)
        Me.progCPU.Name = "progCPU"
        Me.progCPU.Size = New System.Drawing.Size(206, 10)
        Me.progCPU.StartColor = System.Drawing.Color.LimeGreen
        Me.progCPU.TabIndex = 30
        Me.progCPU.Value = 50
        '
        'line2
        '
        Me.line2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.line2.Location = New System.Drawing.Point(5, 104)
        Me.line2.Name = "line2"
        Me.line2.Size = New System.Drawing.Size(242, 2)
        Me.line2.TabIndex = 29
        '
        'btnLog
        '
        Me.btnLog.Image = Global.EveHQ.HQF.My.Resources.Resources.imgLog
        Me.btnLog.Location = New System.Drawing.Point(86, 69)
        Me.btnLog.Name = "btnLog"
        Me.btnLog.Size = New System.Drawing.Size(32, 32)
        Me.btnLog.TabIndex = 28
        Me.ToolTip1.SetToolTip(Me.btnLog, "Audit Log")
        Me.btnLog.UseVisualStyleBackColor = True
        '
        'btnSkills
        '
        Me.btnSkills.Image = CType(resources.GetObject("btnSkills.Image"), System.Drawing.Image)
        Me.btnSkills.Location = New System.Drawing.Point(214, 16)
        Me.btnSkills.Name = "btnSkills"
        Me.btnSkills.Size = New System.Drawing.Size(32, 32)
        Me.btnSkills.TabIndex = 27
        Me.ToolTip1.SetToolTip(Me.btnSkills, "Skills")
        Me.btnSkills.UseVisualStyleBackColor = True
        '
        'btnTargetSpeed
        '
        Me.btnTargetSpeed.Image = Global.EveHQ.HQF.My.Resources.Resources.imgScanResolution
        Me.btnTargetSpeed.Location = New System.Drawing.Point(48, 69)
        Me.btnTargetSpeed.Name = "btnTargetSpeed"
        Me.btnTargetSpeed.Size = New System.Drawing.Size(32, 32)
        Me.btnTargetSpeed.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.btnTargetSpeed, "Targeting Speed Analysis")
        Me.btnTargetSpeed.UseVisualStyleBackColor = True
        '
        'cboPilots
        '
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(42, 8)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(165, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 24
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(6, 11)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 23
        Me.lblPilot.Text = "Pilot:"
        '
        'line1
        '
        Me.line1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.line1.Location = New System.Drawing.Point(5, 64)
        Me.line1.Name = "line1"
        Me.line1.Size = New System.Drawing.Size(242, 2)
        Me.line1.TabIndex = 22
        '
        'btnDoomsdayCheck
        '
        Me.btnDoomsdayCheck.Image = CType(resources.GetObject("btnDoomsdayCheck.Image"), System.Drawing.Image)
        Me.btnDoomsdayCheck.Location = New System.Drawing.Point(10, 69)
        Me.btnDoomsdayCheck.Name = "btnDoomsdayCheck"
        Me.btnDoomsdayCheck.Size = New System.Drawing.Size(32, 32)
        Me.btnDoomsdayCheck.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.btnDoomsdayCheck, "Doomsday Resistance Check")
        Me.btnDoomsdayCheck.UseVisualStyleBackColor = True
        '
        'gbCargo
        '
        Me.gbCargo.Controls.Add(Me.lblDroneControl)
        Me.gbCargo.Controls.Add(Me.pbDroneControl)
        Me.gbCargo.Controls.Add(Me.lblDroneBandwidth)
        Me.gbCargo.Controls.Add(Me.pbDroneBandwidth)
        Me.gbCargo.Controls.Add(Me.lblDroneBay)
        Me.gbCargo.Controls.Add(Me.pbDroneBay)
        Me.gbCargo.Controls.Add(Me.lblCargoBay)
        Me.gbCargo.Controls.Add(Me.pbCargoBay)
        Me.gbCargo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbCargo.Location = New System.Drawing.Point(5, 639)
        Me.gbCargo.Name = "gbCargo"
        Me.gbCargo.Size = New System.Drawing.Size(240, 70)
        Me.gbCargo.TabIndex = 19
        Me.gbCargo.TabStop = False
        Me.gbCargo.Text = "Cargo and Drones"
        Me.ToolTip1.SetToolTip(Me.gbCargo, "Cargo and Drone Information")
        '
        'lblDroneControl
        '
        Me.lblDroneControl.AutoSize = True
        Me.lblDroneControl.Location = New System.Drawing.Point(138, 45)
        Me.lblDroneControl.Name = "lblDroneControl"
        Me.lblDroneControl.Size = New System.Drawing.Size(29, 13)
        Me.lblDroneControl.TabIndex = 12
        Me.lblDroneControl.Text = "0 / 0"
        Me.ToolTip1.SetToolTip(Me.lblDroneControl, "Drone Control")
        '
        'pbDroneControl
        '
        Me.pbDroneControl.Image = Global.EveHQ.HQF.My.Resources.Resources.imgDroneControl
        Me.pbDroneControl.Location = New System.Drawing.Point(108, 42)
        Me.pbDroneControl.Name = "pbDroneControl"
        Me.pbDroneControl.Size = New System.Drawing.Size(24, 24)
        Me.pbDroneControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbDroneControl.TabIndex = 11
        Me.pbDroneControl.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbDroneControl, "Drone Control")
        '
        'lblDroneBandwidth
        '
        Me.lblDroneBandwidth.AutoSize = True
        Me.lblDroneBandwidth.Location = New System.Drawing.Point(36, 45)
        Me.lblDroneBandwidth.Name = "lblDroneBandwidth"
        Me.lblDroneBandwidth.Size = New System.Drawing.Size(53, 13)
        Me.lblDroneBandwidth.TabIndex = 10
        Me.lblDroneBandwidth.Text = "000 / 000"
        Me.ToolTip1.SetToolTip(Me.lblDroneBandwidth, "Drone Bandwidth")
        '
        'pbDroneBandwidth
        '
        Me.pbDroneBandwidth.Image = Global.EveHQ.HQF.My.Resources.Resources.imgDrone
        Me.pbDroneBandwidth.Location = New System.Drawing.Point(6, 42)
        Me.pbDroneBandwidth.Name = "pbDroneBandwidth"
        Me.pbDroneBandwidth.Size = New System.Drawing.Size(24, 24)
        Me.pbDroneBandwidth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbDroneBandwidth.TabIndex = 6
        Me.pbDroneBandwidth.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbDroneBandwidth, "Drone Bandwidth")
        '
        'lblDroneBay
        '
        Me.lblDroneBay.AutoSize = True
        Me.lblDroneBay.Location = New System.Drawing.Point(138, 22)
        Me.lblDroneBay.Name = "lblDroneBay"
        Me.lblDroneBay.Size = New System.Drawing.Size(58, 13)
        Me.lblDroneBay.TabIndex = 5
        Me.lblDroneBay.Text = "00,000 m3"
        Me.ToolTip1.SetToolTip(Me.lblDroneBay, "Drone Bay")
        '
        'pbDroneBay
        '
        Me.pbDroneBay.Image = Global.EveHQ.HQF.My.Resources.Resources.imgDroneBay
        Me.pbDroneBay.Location = New System.Drawing.Point(108, 19)
        Me.pbDroneBay.Name = "pbDroneBay"
        Me.pbDroneBay.Size = New System.Drawing.Size(24, 24)
        Me.pbDroneBay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbDroneBay.TabIndex = 3
        Me.pbDroneBay.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbDroneBay, "Drone Bay")
        '
        'lblCargoBay
        '
        Me.lblCargoBay.AutoSize = True
        Me.lblCargoBay.Location = New System.Drawing.Point(36, 22)
        Me.lblCargoBay.Name = "lblCargoBay"
        Me.lblCargoBay.Size = New System.Drawing.Size(64, 13)
        Me.lblCargoBay.TabIndex = 2
        Me.lblCargoBay.Text = "000,000 m3"
        Me.ToolTip1.SetToolTip(Me.lblCargoBay, "Cargo Bay")
        '
        'pbCargoBay
        '
        Me.pbCargoBay.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCargo
        Me.pbCargoBay.Location = New System.Drawing.Point(6, 19)
        Me.pbCargoBay.Name = "pbCargoBay"
        Me.pbCargoBay.Size = New System.Drawing.Size(24, 24)
        Me.pbCargoBay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbCargoBay.TabIndex = 0
        Me.pbCargoBay.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCargoBay, "Cargo Bay")
        '
        'gbPropulsion
        '
        Me.gbPropulsion.Controls.Add(Me.lblAlignTime)
        Me.gbPropulsion.Controls.Add(Me.pbAlignTime)
        Me.gbPropulsion.Controls.Add(Me.lblInertia)
        Me.gbPropulsion.Controls.Add(Me.pbInertia)
        Me.gbPropulsion.Controls.Add(Me.lblWarpSpeed)
        Me.gbPropulsion.Controls.Add(Me.pbWarpSpeed)
        Me.gbPropulsion.Controls.Add(Me.lblSpeed)
        Me.gbPropulsion.Controls.Add(Me.pbSpeed)
        Me.gbPropulsion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbPropulsion.Location = New System.Drawing.Point(6, 567)
        Me.gbPropulsion.Name = "gbPropulsion"
        Me.gbPropulsion.Size = New System.Drawing.Size(240, 70)
        Me.gbPropulsion.TabIndex = 18
        Me.gbPropulsion.TabStop = False
        Me.gbPropulsion.Text = "Propulsion"
        Me.ToolTip1.SetToolTip(Me.gbPropulsion, "Propulsion Information")
        '
        'lblAlignTime
        '
        Me.lblAlignTime.AutoSize = True
        Me.lblAlignTime.Location = New System.Drawing.Point(124, 45)
        Me.lblAlignTime.Name = "lblAlignTime"
        Me.lblAlignTime.Size = New System.Drawing.Size(43, 13)
        Me.lblAlignTime.TabIndex = 11
        Me.lblAlignTime.Text = "00.00 s"
        Me.ToolTip1.SetToolTip(Me.lblAlignTime, "Warp Align Time")
        '
        'pbAlignTime
        '
        Me.pbAlignTime.Image = Global.EveHQ.HQF.My.Resources.Resources.imgWarpAlign
        Me.pbAlignTime.Location = New System.Drawing.Point(100, 42)
        Me.pbAlignTime.Name = "pbAlignTime"
        Me.pbAlignTime.Size = New System.Drawing.Size(24, 24)
        Me.pbAlignTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbAlignTime.TabIndex = 9
        Me.pbAlignTime.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbAlignTime, "Warp Align Time")
        '
        'lblInertia
        '
        Me.lblInertia.AutoSize = True
        Me.lblInertia.Location = New System.Drawing.Point(28, 45)
        Me.lblInertia.Name = "lblInertia"
        Me.lblInertia.Size = New System.Drawing.Size(41, 13)
        Me.lblInertia.TabIndex = 8
        Me.lblInertia.Text = "0.0000"
        Me.ToolTip1.SetToolTip(Me.lblInertia, "Inertia")
        '
        'pbInertia
        '
        Me.pbInertia.Image = Global.EveHQ.HQF.My.Resources.Resources.imgInertia
        Me.pbInertia.Location = New System.Drawing.Point(4, 42)
        Me.pbInertia.Name = "pbInertia"
        Me.pbInertia.Size = New System.Drawing.Size(24, 24)
        Me.pbInertia.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbInertia.TabIndex = 6
        Me.pbInertia.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbInertia, "Inertia")
        '
        'lblWarpSpeed
        '
        Me.lblWarpSpeed.AutoSize = True
        Me.lblWarpSpeed.Location = New System.Drawing.Point(124, 22)
        Me.lblWarpSpeed.Name = "lblWarpSpeed"
        Me.lblWarpSpeed.Size = New System.Drawing.Size(59, 13)
        Me.lblWarpSpeed.TabIndex = 5
        Me.lblWarpSpeed.Text = "00.00 au/s"
        Me.ToolTip1.SetToolTip(Me.lblWarpSpeed, "Warp Speed")
        '
        'pbWarpSpeed
        '
        Me.pbWarpSpeed.Image = Global.EveHQ.HQF.My.Resources.Resources.imgWarpSpeed
        Me.pbWarpSpeed.Location = New System.Drawing.Point(100, 19)
        Me.pbWarpSpeed.Name = "pbWarpSpeed"
        Me.pbWarpSpeed.Size = New System.Drawing.Size(24, 24)
        Me.pbWarpSpeed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbWarpSpeed.TabIndex = 3
        Me.pbWarpSpeed.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbWarpSpeed, "Warp Speed")
        '
        'lblSpeed
        '
        Me.lblSpeed.AutoSize = True
        Me.lblSpeed.Location = New System.Drawing.Point(29, 22)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(61, 13)
        Me.lblSpeed.TabIndex = 2
        Me.lblSpeed.Text = "00,000 m/s"
        Me.ToolTip1.SetToolTip(Me.lblSpeed, "Max Velocity")
        '
        'pbSpeed
        '
        Me.pbSpeed.Image = Global.EveHQ.HQF.My.Resources.Resources.imgSpeed
        Me.pbSpeed.Location = New System.Drawing.Point(4, 19)
        Me.pbSpeed.Name = "pbSpeed"
        Me.pbSpeed.Size = New System.Drawing.Size(24, 24)
        Me.pbSpeed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbSpeed.TabIndex = 0
        Me.pbSpeed.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbSpeed, "Max Velocity")
        '
        'gbCapacitor
        '
        Me.gbCapacitor.Controls.Add(Me.lblCapBalN)
        Me.gbCapacitor.Controls.Add(Me.lblCapBalP)
        Me.gbCapacitor.Controls.Add(Me.pbCapBal)
        Me.gbCapacitor.Controls.Add(Me.lblCapPeak)
        Me.gbCapacitor.Controls.Add(Me.pbCapPeak)
        Me.gbCapacitor.Controls.Add(Me.lblCapRecharge)
        Me.gbCapacitor.Controls.Add(Me.pbCapRecharge)
        Me.gbCapacitor.Controls.Add(Me.lblCapacitor)
        Me.gbCapacitor.Controls.Add(Me.pbCapacitor)
        Me.gbCapacitor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbCapacitor.Location = New System.Drawing.Point(6, 349)
        Me.gbCapacitor.Name = "gbCapacitor"
        Me.gbCapacitor.Size = New System.Drawing.Size(240, 72)
        Me.gbCapacitor.TabIndex = 17
        Me.gbCapacitor.TabStop = False
        Me.gbCapacitor.Text = "Capacitor"
        Me.ToolTip1.SetToolTip(Me.gbCapacitor, "Capacitor Information")
        '
        'lblCapBalN
        '
        Me.lblCapBalN.AutoSize = True
        Me.lblCapBalN.Location = New System.Drawing.Point(29, 56)
        Me.lblCapBalN.Name = "lblCapBalN"
        Me.lblCapBalN.Size = New System.Drawing.Size(17, 13)
        Me.lblCapBalN.TabIndex = 14
        Me.lblCapBalN.Text = "-0"
        Me.ToolTip1.SetToolTip(Me.lblCapBalN, "Total Consumption Rate (F/s)")
        '
        'lblCapBalP
        '
        Me.lblCapBalP.AutoSize = True
        Me.lblCapBalP.Location = New System.Drawing.Point(29, 44)
        Me.lblCapBalP.Name = "lblCapBalP"
        Me.lblCapBalP.Size = New System.Drawing.Size(21, 13)
        Me.lblCapBalP.TabIndex = 13
        Me.lblCapBalP.Text = "+0"
        Me.ToolTip1.SetToolTip(Me.lblCapBalP, "Peak Injection Rate (F/s)")
        '
        'pbCapBal
        '
        Me.pbCapBal.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCapBal
        Me.pbCapBal.Location = New System.Drawing.Point(5, 44)
        Me.pbCapBal.Name = "pbCapBal"
        Me.pbCapBal.Size = New System.Drawing.Size(24, 24)
        Me.pbCapBal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbCapBal.TabIndex = 12
        Me.pbCapBal.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCapBal, "Usage/Injection Rates")
        '
        'lblCapPeak
        '
        Me.lblCapPeak.AutoSize = True
        Me.lblCapPeak.Location = New System.Drawing.Point(124, 47)
        Me.lblCapPeak.Name = "lblCapPeak"
        Me.lblCapPeak.Size = New System.Drawing.Size(41, 13)
        Me.lblCapPeak.TabIndex = 11
        Me.lblCapPeak.Text = "000.00"
        Me.ToolTip1.SetToolTip(Me.lblCapPeak, "Peak Recharge Rate")
        '
        'pbCapPeak
        '
        Me.pbCapPeak.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCapPeak
        Me.pbCapPeak.Location = New System.Drawing.Point(100, 44)
        Me.pbCapPeak.Name = "pbCapPeak"
        Me.pbCapPeak.Size = New System.Drawing.Size(24, 24)
        Me.pbCapPeak.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCapPeak.TabIndex = 9
        Me.pbCapPeak.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCapPeak, "Peak Recharge Rate")
        '
        'lblCapRecharge
        '
        Me.lblCapRecharge.AutoSize = True
        Me.lblCapRecharge.Location = New System.Drawing.Point(124, 22)
        Me.lblCapRecharge.Name = "lblCapRecharge"
        Me.lblCapRecharge.Size = New System.Drawing.Size(43, 13)
        Me.lblCapRecharge.TabIndex = 8
        Me.lblCapRecharge.Text = "0,000 s"
        Me.ToolTip1.SetToolTip(Me.lblCapRecharge, "Recharge Time")
        '
        'pbCapRecharge
        '
        Me.pbCapRecharge.Image = Global.EveHQ.HQF.My.Resources.Resources.imgTimer
        Me.pbCapRecharge.Location = New System.Drawing.Point(100, 19)
        Me.pbCapRecharge.Name = "pbCapRecharge"
        Me.pbCapRecharge.Size = New System.Drawing.Size(24, 24)
        Me.pbCapRecharge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbCapRecharge.TabIndex = 6
        Me.pbCapRecharge.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCapRecharge, "Recharge Time")
        '
        'lblCapacitor
        '
        Me.lblCapacitor.AutoSize = True
        Me.lblCapacitor.Location = New System.Drawing.Point(29, 22)
        Me.lblCapacitor.Name = "lblCapacitor"
        Me.lblCapacitor.Size = New System.Drawing.Size(41, 13)
        Me.lblCapacitor.TabIndex = 2
        Me.lblCapacitor.Text = "00,000"
        Me.ToolTip1.SetToolTip(Me.lblCapacitor, "Capacity")
        '
        'pbCapacitor
        '
        Me.pbCapacitor.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCap
        Me.pbCapacitor.Location = New System.Drawing.Point(5, 19)
        Me.pbCapacitor.Name = "pbCapacitor"
        Me.pbCapacitor.Size = New System.Drawing.Size(24, 24)
        Me.pbCapacitor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbCapacitor.TabIndex = 0
        Me.pbCapacitor.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCapacitor, "Capacity")
        '
        'gbTargeting
        '
        Me.gbTargeting.Controls.Add(Me.lblTargets)
        Me.gbTargeting.Controls.Add(Me.pbTargets)
        Me.gbTargeting.Controls.Add(Me.lblSigRadius)
        Me.gbTargeting.Controls.Add(Me.pbSigRadius)
        Me.gbTargeting.Controls.Add(Me.lblSensorStrength)
        Me.gbTargeting.Controls.Add(Me.pbSensorStrength)
        Me.gbTargeting.Controls.Add(Me.lblScanResolution)
        Me.gbTargeting.Controls.Add(Me.pbScanResolution)
        Me.gbTargeting.Controls.Add(Me.lblTargetRange)
        Me.gbTargeting.Controls.Add(Me.pbTargetRange)
        Me.gbTargeting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbTargeting.Location = New System.Drawing.Point(6, 495)
        Me.gbTargeting.Name = "gbTargeting"
        Me.gbTargeting.Size = New System.Drawing.Size(240, 70)
        Me.gbTargeting.TabIndex = 14
        Me.gbTargeting.TabStop = False
        Me.gbTargeting.Text = "Targeting"
        Me.ToolTip1.SetToolTip(Me.gbTargeting, "Targeting Information")
        '
        'lblTargets
        '
        Me.lblTargets.AutoSize = True
        Me.lblTargets.Location = New System.Drawing.Point(185, 22)
        Me.lblTargets.Name = "lblTargets"
        Me.lblTargets.Size = New System.Drawing.Size(29, 13)
        Me.lblTargets.TabIndex = 20
        Me.lblTargets.Text = "0 / 0"
        Me.ToolTip1.SetToolTip(Me.lblTargets, "Sensor Strength")
        '
        'pbTargets
        '
        Me.pbTargets.Image = Global.EveHQ.HQF.My.Resources.Resources.imgMaxTargets
        Me.pbTargets.Location = New System.Drawing.Point(161, 19)
        Me.pbTargets.Name = "pbTargets"
        Me.pbTargets.Size = New System.Drawing.Size(24, 24)
        Me.pbTargets.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbTargets.TabIndex = 19
        Me.pbTargets.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbTargets, "Max Locked Targets")
        '
        'lblSigRadius
        '
        Me.lblSigRadius.AutoSize = True
        Me.lblSigRadius.Location = New System.Drawing.Point(130, 45)
        Me.lblSigRadius.Name = "lblSigRadius"
        Me.lblSigRadius.Size = New System.Drawing.Size(46, 13)
        Me.lblSigRadius.TabIndex = 18
        Me.lblSigRadius.Text = "0,000 m"
        Me.ToolTip1.SetToolTip(Me.lblSigRadius, "Signature Radius")
        '
        'pbSigRadius
        '
        Me.pbSigRadius.Image = Global.EveHQ.HQF.My.Resources.Resources.imgSigRadius
        Me.pbSigRadius.Location = New System.Drawing.Point(107, 42)
        Me.pbSigRadius.Name = "pbSigRadius"
        Me.pbSigRadius.Size = New System.Drawing.Size(24, 24)
        Me.pbSigRadius.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbSigRadius.TabIndex = 17
        Me.pbSigRadius.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbSigRadius, "Signature Radius")
        '
        'lblSensorStrength
        '
        Me.lblSensorStrength.AutoSize = True
        Me.lblSensorStrength.Location = New System.Drawing.Point(130, 22)
        Me.lblSensorStrength.Name = "lblSensorStrength"
        Me.lblSensorStrength.Size = New System.Drawing.Size(19, 13)
        Me.lblSensorStrength.TabIndex = 11
        Me.lblSensorStrength.Text = "00"
        Me.ToolTip1.SetToolTip(Me.lblSensorStrength, "Sensor Strength")
        '
        'pbSensorStrength
        '
        Me.pbSensorStrength.Image = Global.EveHQ.HQF.My.Resources.Resources.imgSensorStregthL
        Me.pbSensorStrength.Location = New System.Drawing.Point(107, 19)
        Me.pbSensorStrength.Name = "pbSensorStrength"
        Me.pbSensorStrength.Size = New System.Drawing.Size(24, 24)
        Me.pbSensorStrength.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbSensorStrength.TabIndex = 9
        Me.pbSensorStrength.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbSensorStrength, "Sensor Strength")
        '
        'lblScanResolution
        '
        Me.lblScanResolution.AutoSize = True
        Me.lblScanResolution.Location = New System.Drawing.Point(36, 45)
        Me.lblScanResolution.Name = "lblScanResolution"
        Me.lblScanResolution.Size = New System.Drawing.Size(44, 13)
        Me.lblScanResolution.TabIndex = 8
        Me.lblScanResolution.Text = "000 mm"
        Me.ToolTip1.SetToolTip(Me.lblScanResolution, "Scan Resolution")
        '
        'pbScanResolution
        '
        Me.pbScanResolution.Image = Global.EveHQ.HQF.My.Resources.Resources.imgScanResolution
        Me.pbScanResolution.Location = New System.Drawing.Point(6, 42)
        Me.pbScanResolution.Name = "pbScanResolution"
        Me.pbScanResolution.Size = New System.Drawing.Size(24, 24)
        Me.pbScanResolution.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbScanResolution.TabIndex = 6
        Me.pbScanResolution.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbScanResolution, "Scan Resolution")
        '
        'lblTargetRange
        '
        Me.lblTargetRange.AutoSize = True
        Me.lblTargetRange.Location = New System.Drawing.Point(36, 22)
        Me.lblTargetRange.Name = "lblTargetRange"
        Me.lblTargetRange.Size = New System.Drawing.Size(58, 13)
        Me.lblTargetRange.TabIndex = 2
        Me.lblTargetRange.Text = "000,000 m"
        Me.ToolTip1.SetToolTip(Me.lblTargetRange, "Max Targeting Range")
        '
        'pbTargetRange
        '
        Me.pbTargetRange.Image = Global.EveHQ.HQF.My.Resources.Resources.imgTargetRange
        Me.pbTargetRange.Location = New System.Drawing.Point(6, 19)
        Me.pbTargetRange.Name = "pbTargetRange"
        Me.pbTargetRange.Size = New System.Drawing.Size(24, 24)
        Me.pbTargetRange.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbTargetRange.TabIndex = 0
        Me.pbTargetRange.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbTargetRange, "Max Targeting Range")
        '
        'lblCalibration
        '
        Me.lblCalibration.AutoSize = True
        Me.lblCalibration.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCalibration.Location = New System.Drawing.Point(36, 159)
        Me.lblCalibration.Name = "lblCalibration"
        Me.lblCalibration.Size = New System.Drawing.Size(29, 13)
        Me.lblCalibration.TabIndex = 13
        Me.lblCalibration.Text = "0 / 0"
        Me.ToolTip1.SetToolTip(Me.lblCalibration, "Calibration")
        '
        'pbCalibration
        '
        Me.pbCalibration.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCalibration
        Me.pbCalibration.Location = New System.Drawing.Point(8, 157)
        Me.pbCalibration.Name = "pbCalibration"
        Me.pbCalibration.Size = New System.Drawing.Size(24, 24)
        Me.pbCalibration.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCalibration.TabIndex = 11
        Me.pbCalibration.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCalibration, "Calibration")
        '
        'lblPG
        '
        Me.lblPG.AutoSize = True
        Me.lblPG.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPG.Location = New System.Drawing.Point(36, 135)
        Me.lblPG.Name = "lblPG"
        Me.lblPG.Size = New System.Drawing.Size(29, 13)
        Me.lblPG.TabIndex = 7
        Me.lblPG.Text = "0 / 0"
        Me.ToolTip1.SetToolTip(Me.lblPG, "Powergrid")
        '
        'lblCPU
        '
        Me.lblCPU.AutoSize = True
        Me.lblCPU.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCPU.Location = New System.Drawing.Point(36, 111)
        Me.lblCPU.Name = "lblCPU"
        Me.lblCPU.Size = New System.Drawing.Size(29, 13)
        Me.lblCPU.TabIndex = 6
        Me.lblCPU.Text = "0 / 0"
        Me.ToolTip1.SetToolTip(Me.lblCPU, "CPU")
        '
        'pbPG
        '
        Me.pbPG.Image = Global.EveHQ.HQF.My.Resources.Resources.imgPG
        Me.pbPG.Location = New System.Drawing.Point(8, 133)
        Me.pbPG.Name = "pbPG"
        Me.pbPG.Size = New System.Drawing.Size(24, 24)
        Me.pbPG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbPG.TabIndex = 3
        Me.pbPG.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbPG, "Powergrid")
        '
        'pbCPU
        '
        Me.pbCPU.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCPU
        Me.pbCPU.Location = New System.Drawing.Point(8, 109)
        Me.pbCPU.Name = "pbCPU"
        Me.pbCPU.Size = New System.Drawing.Size(24, 24)
        Me.pbCPU.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCPU.TabIndex = 1
        Me.pbCPU.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCPU, "CPU")
        '
        'lblPGReqd
        '
        Me.lblPGReqd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPGReqd.Location = New System.Drawing.Point(167, 135)
        Me.lblPGReqd.Name = "lblPGReqd"
        Me.lblPGReqd.Size = New System.Drawing.Size(79, 13)
        Me.lblPGReqd.TabIndex = 38
        Me.lblPGReqd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.lblPGReqd, "Powergrid Required")
        '
        'lblCPUReqd
        '
        Me.lblCPUReqd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCPUReqd.Location = New System.Drawing.Point(167, 111)
        Me.lblCPUReqd.Name = "lblCPUReqd"
        Me.lblCPUReqd.Size = New System.Drawing.Size(79, 13)
        Me.lblCPUReqd.TabIndex = 37
        Me.lblCPUReqd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.lblCPUReqd, "CPU Required")
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 20000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 100
        Me.ToolTip1.ShowAlways = True
        '
        'ShipInfoControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Controls.Add(Me.Panel1)
        Me.Name = "ShipInfoControl"
        Me.Size = New System.Drawing.Size(270, 732)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.gbDefence.ResumeLayout(False)
        Me.gbDefence.PerformLayout()
        CType(Me.pbStructureHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDamage.ResumeLayout(False)
        Me.gbDamage.PerformLayout()
        CType(Me.pbMining, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbDamage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbCargo.ResumeLayout(False)
        Me.gbCargo.PerformLayout()
        CType(Me.pbDroneControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbDroneBandwidth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbDroneBay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCargoBay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbPropulsion.ResumeLayout(False)
        Me.gbPropulsion.PerformLayout()
        CType(Me.pbAlignTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbInertia, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbWarpSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbCapacitor.ResumeLayout(False)
        Me.gbCapacitor.PerformLayout()
        CType(Me.pbCapBal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCapPeak, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCapRecharge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCapacitor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbTargeting.ResumeLayout(False)
        Me.gbTargeting.PerformLayout()
        CType(Me.pbTargets, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSigRadius, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSensorStrength, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbScanResolution, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbTargetRange, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCalibration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCPU, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pbCPU As System.Windows.Forms.PictureBox
    Friend WithEvents pbPG As System.Windows.Forms.PictureBox
    Friend WithEvents lblPG As System.Windows.Forms.Label
    Friend WithEvents lblCPU As System.Windows.Forms.Label
    Friend WithEvents lblShieldEM As System.Windows.Forms.Label
    Friend WithEvents pbShieldHP As System.Windows.Forms.PictureBox
    Friend WithEvents lblShieldThermal As System.Windows.Forms.Label
    Friend WithEvents lblShieldExplosive As System.Windows.Forms.Label
    Friend WithEvents lblShieldKinetic As System.Windows.Forms.Label
    Friend WithEvents lblShieldHP As System.Windows.Forms.Label
    Friend WithEvents lblStructureHP As System.Windows.Forms.Label
    Friend WithEvents pbStructureHP As System.Windows.Forms.PictureBox
    Friend WithEvents lblStructureThermal As System.Windows.Forms.Label
    Friend WithEvents lblStructureExplosive As System.Windows.Forms.Label
    Friend WithEvents lblStructureKinetic As System.Windows.Forms.Label
    Friend WithEvents lblStructureEM As System.Windows.Forms.Label
    Friend WithEvents lblArmorHP As System.Windows.Forms.Label
    Friend WithEvents pbArmorHP As System.Windows.Forms.PictureBox
    Friend WithEvents lblArmorThermal As System.Windows.Forms.Label
    Friend WithEvents lblArmorExplosive As System.Windows.Forms.Label
    Friend WithEvents lblArmorKinetic As System.Windows.Forms.Label
    Friend WithEvents lblArmorEM As System.Windows.Forms.Label
    Friend WithEvents lblCalibration As System.Windows.Forms.Label
    Friend WithEvents pbCalibration As System.Windows.Forms.PictureBox
    Friend WithEvents gbTargeting As System.Windows.Forms.GroupBox
    Friend WithEvents lblSensorStrength As System.Windows.Forms.Label
    Friend WithEvents pbSensorStrength As System.Windows.Forms.PictureBox
    Friend WithEvents lblScanResolution As System.Windows.Forms.Label
    Friend WithEvents pbScanResolution As System.Windows.Forms.PictureBox
    Friend WithEvents lblTargetRange As System.Windows.Forms.Label
    Friend WithEvents pbTargetRange As System.Windows.Forms.PictureBox
    Friend WithEvents gbCapacitor As System.Windows.Forms.GroupBox
    Friend WithEvents lblCapPeak As System.Windows.Forms.Label
    Friend WithEvents pbCapPeak As System.Windows.Forms.PictureBox
    Friend WithEvents lblCapRecharge As System.Windows.Forms.Label
    Friend WithEvents pbCapRecharge As System.Windows.Forms.PictureBox
    Friend WithEvents lblCapacitor As System.Windows.Forms.Label
    Friend WithEvents pbCapacitor As System.Windows.Forms.PictureBox
    Friend WithEvents gbPropulsion As System.Windows.Forms.GroupBox
    Friend WithEvents lblAlignTime As System.Windows.Forms.Label
    Friend WithEvents pbAlignTime As System.Windows.Forms.PictureBox
    Friend WithEvents lblInertia As System.Windows.Forms.Label
    Friend WithEvents pbInertia As System.Windows.Forms.PictureBox
    Friend WithEvents lblWarpSpeed As System.Windows.Forms.Label
    Friend WithEvents pbWarpSpeed As System.Windows.Forms.PictureBox
    Friend WithEvents lblSpeed As System.Windows.Forms.Label
    Friend WithEvents pbSpeed As System.Windows.Forms.PictureBox
    Friend WithEvents gbCargo As System.Windows.Forms.GroupBox
    Friend WithEvents pbDroneBandwidth As System.Windows.Forms.PictureBox
    Friend WithEvents lblDroneBay As System.Windows.Forms.Label
    Friend WithEvents pbDroneBay As System.Windows.Forms.PictureBox
    Friend WithEvents lblCargoBay As System.Windows.Forms.Label
    Friend WithEvents pbCargoBay As System.Windows.Forms.PictureBox
    Friend WithEvents lblDroneBandwidth As System.Windows.Forms.Label
    Friend WithEvents btnDoomsdayCheck As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents line1 As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents btnTargetSpeed As System.Windows.Forms.Button
    Friend WithEvents btnSkills As System.Windows.Forms.Button
    Friend WithEvents btnLog As System.Windows.Forms.Button
    Friend WithEvents line2 As System.Windows.Forms.Label
    Friend WithEvents progCPU As VistaStyleProgressBar.ProgressBar
    Friend WithEvents progCalibration As VistaStyleProgressBar.ProgressBar
    Friend WithEvents progPG As VistaStyleProgressBar.ProgressBar
    Friend WithEvents gbDamage As System.Windows.Forms.GroupBox
    Friend WithEvents lblDamage As System.Windows.Forms.Label
    Friend WithEvents pbDamage As System.Windows.Forms.PictureBox
    Friend WithEvents lblSigRadius As System.Windows.Forms.Label
    Friend WithEvents pbSigRadius As System.Windows.Forms.PictureBox
    Friend WithEvents lblDroneControl As System.Windows.Forms.Label
    Friend WithEvents pbDroneControl As System.Windows.Forms.PictureBox
    Friend WithEvents lblMining As System.Windows.Forms.Label
    Friend WithEvents pbMining As System.Windows.Forms.PictureBox
    Friend WithEvents lblCapBalP As System.Windows.Forms.Label
    Friend WithEvents pbCapBal As System.Windows.Forms.PictureBox
    Friend WithEvents lblCapBalN As System.Windows.Forms.Label
    Friend WithEvents gbDefence As System.Windows.Forms.GroupBox
    Friend WithEvents lblEffectiveHP As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents cboDamageProfiles As System.Windows.Forms.ComboBox
    Friend WithEvents btnEditProfiles As System.Windows.Forms.Button
    Friend WithEvents lblTankAbility As System.Windows.Forms.Label
    Friend WithEvents lblImplants As System.Windows.Forms.Label
    Friend WithEvents cboImplants As System.Windows.Forms.ComboBox
    Friend WithEvents lblTargets As System.Windows.Forms.Label
    Friend WithEvents pbTargets As System.Windows.Forms.PictureBox
    Friend WithEvents lblPGReqd As System.Windows.Forms.Label
    Friend WithEvents lblCPUReqd As System.Windows.Forms.Label

End Class
