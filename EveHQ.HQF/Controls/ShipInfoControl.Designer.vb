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
        Me.btnLog = New System.Windows.Forms.Button
        Me.btnSkills = New System.Windows.Forms.Button
        Me.gbDamage = New System.Windows.Forms.GroupBox
        Me.lblMissileVolley = New System.Windows.Forms.Label
        Me.lblTurretVolley = New System.Windows.Forms.Label
        Me.btnTargetSpeed = New System.Windows.Forms.Button
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.line1 = New System.Windows.Forms.Label
        Me.lblEffectiveHP = New System.Windows.Forms.Label
        Me.progCalibration = New System.Windows.Forms.ProgressBar
        Me.progCPU = New System.Windows.Forms.ProgressBar
        Me.progPG = New System.Windows.Forms.ProgressBar
        Me.btnDoomsdayCheck = New System.Windows.Forms.Button
        Me.gbCargo = New System.Windows.Forms.GroupBox
        Me.lblDroneBandwidth = New System.Windows.Forms.Label
        Me.progDroneBandwidth = New System.Windows.Forms.ProgressBar
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
        Me.lblCapPeak = New System.Windows.Forms.Label
        Me.pbCapPeak = New System.Windows.Forms.PictureBox
        Me.lblCapRecharge = New System.Windows.Forms.Label
        Me.pbCapRecharge = New System.Windows.Forms.PictureBox
        Me.lblCapAverage = New System.Windows.Forms.Label
        Me.pbCapAverage = New System.Windows.Forms.PictureBox
        Me.lblCapacitor = New System.Windows.Forms.Label
        Me.pbCapacitor = New System.Windows.Forms.PictureBox
        Me.gbTargeting = New System.Windows.Forms.GroupBox
        Me.lblSensorStrength = New System.Windows.Forms.Label
        Me.pbSensorStrength = New System.Windows.Forms.PictureBox
        Me.lblScanResolution = New System.Windows.Forms.Label
        Me.pbScanResolution = New System.Windows.Forms.PictureBox
        Me.lblMaxTargets = New System.Windows.Forms.Label
        Me.pbMaxTargets = New System.Windows.Forms.PictureBox
        Me.lblTargetRange = New System.Windows.Forms.Label
        Me.pbTargetRange = New System.Windows.Forms.PictureBox
        Me.lblCalibration = New System.Windows.Forms.Label
        Me.pbCalibration = New System.Windows.Forms.PictureBox
        Me.gbStructure = New System.Windows.Forms.GroupBox
        Me.lblSigRadius = New System.Windows.Forms.Label
        Me.pbSigRadius = New System.Windows.Forms.PictureBox
        Me.progStructureThermal = New System.Windows.Forms.ProgressBar
        Me.progStructureExplosive = New System.Windows.Forms.ProgressBar
        Me.progStructureKinetic = New System.Windows.Forms.ProgressBar
        Me.progStructureEM = New System.Windows.Forms.ProgressBar
        Me.lblStructureHP = New System.Windows.Forms.Label
        Me.pbStructureHP = New System.Windows.Forms.PictureBox
        Me.lblStructureThermal = New System.Windows.Forms.Label
        Me.pbStructureThermal = New System.Windows.Forms.PictureBox
        Me.lblStructureExplosive = New System.Windows.Forms.Label
        Me.pbStructureExplosive = New System.Windows.Forms.PictureBox
        Me.lblStructureKinetic = New System.Windows.Forms.Label
        Me.pbStructureKinetic = New System.Windows.Forms.PictureBox
        Me.lblStructureEM = New System.Windows.Forms.Label
        Me.pbStructureEM = New System.Windows.Forms.PictureBox
        Me.gbArmor = New System.Windows.Forms.GroupBox
        Me.progArmorThermal = New System.Windows.Forms.ProgressBar
        Me.progArmorExplosive = New System.Windows.Forms.ProgressBar
        Me.progArmorKinetic = New System.Windows.Forms.ProgressBar
        Me.progArmorEM = New System.Windows.Forms.ProgressBar
        Me.lblArmorHP = New System.Windows.Forms.Label
        Me.pbArmorHP = New System.Windows.Forms.PictureBox
        Me.lblArmorThermal = New System.Windows.Forms.Label
        Me.pbArmorThermal = New System.Windows.Forms.PictureBox
        Me.lblArmorExplosive = New System.Windows.Forms.Label
        Me.pbArmorExplosive = New System.Windows.Forms.PictureBox
        Me.lblArmorKinetic = New System.Windows.Forms.Label
        Me.pbArmorKinetic = New System.Windows.Forms.PictureBox
        Me.lblArmorEM = New System.Windows.Forms.Label
        Me.pbArmorEM = New System.Windows.Forms.PictureBox
        Me.gbShield = New System.Windows.Forms.GroupBox
        Me.progShieldThermal = New System.Windows.Forms.ProgressBar
        Me.progShieldExp = New System.Windows.Forms.ProgressBar
        Me.progShieldKinetic = New System.Windows.Forms.ProgressBar
        Me.progShieldEM = New System.Windows.Forms.ProgressBar
        Me.lblShieldRecharge = New System.Windows.Forms.Label
        Me.lblShieldHP = New System.Windows.Forms.Label
        Me.pbShieldRecharge = New System.Windows.Forms.PictureBox
        Me.pbShieldHP = New System.Windows.Forms.PictureBox
        Me.lblShieldThermal = New System.Windows.Forms.Label
        Me.pbShieldThermal = New System.Windows.Forms.PictureBox
        Me.lblShieldExplosive = New System.Windows.Forms.Label
        Me.pbShieldExplosive = New System.Windows.Forms.PictureBox
        Me.lblShieldKinetic = New System.Windows.Forms.Label
        Me.pbShieldKinetic = New System.Windows.Forms.PictureBox
        Me.lblShieldEM = New System.Windows.Forms.Label
        Me.pbShieldEM = New System.Windows.Forms.PictureBox
        Me.lblPG = New System.Windows.Forms.Label
        Me.lblCPU = New System.Windows.Forms.Label
        Me.pbPG = New System.Windows.Forms.PictureBox
        Me.pbCPU = New System.Windows.Forms.PictureBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.gbDamage.SuspendLayout()
        Me.gbCargo.SuspendLayout()
        CType(Me.pbDroneBandwidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDroneBay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCargoBay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPropulsion.SuspendLayout()
        CType(Me.pbAlignTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbInertia, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbWarpSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbCapacitor.SuspendLayout()
        CType(Me.pbCapPeak, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCapRecharge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCapAverage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCapacitor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbTargeting.SuspendLayout()
        CType(Me.pbSensorStrength, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbScanResolution, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbMaxTargets, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbTargetRange, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCalibration, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbStructure.SuspendLayout()
        CType(Me.pbSigRadius, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructureHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructureThermal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructureExplosive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructureKinetic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructureEM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbArmor.SuspendLayout()
        CType(Me.pbArmorHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorThermal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorExplosive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorKinetic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorEM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbShield.SuspendLayout()
        CType(Me.pbShieldRecharge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldThermal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldExplosive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldKinetic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldEM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCPU, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnLog)
        Me.Panel1.Controls.Add(Me.btnSkills)
        Me.Panel1.Controls.Add(Me.gbDamage)
        Me.Panel1.Controls.Add(Me.btnTargetSpeed)
        Me.Panel1.Controls.Add(Me.cboPilots)
        Me.Panel1.Controls.Add(Me.lblPilot)
        Me.Panel1.Controls.Add(Me.line1)
        Me.Panel1.Controls.Add(Me.lblEffectiveHP)
        Me.Panel1.Controls.Add(Me.progCalibration)
        Me.Panel1.Controls.Add(Me.progCPU)
        Me.Panel1.Controls.Add(Me.progPG)
        Me.Panel1.Controls.Add(Me.btnDoomsdayCheck)
        Me.Panel1.Controls.Add(Me.gbCargo)
        Me.Panel1.Controls.Add(Me.gbPropulsion)
        Me.Panel1.Controls.Add(Me.gbCapacitor)
        Me.Panel1.Controls.Add(Me.gbTargeting)
        Me.Panel1.Controls.Add(Me.lblCalibration)
        Me.Panel1.Controls.Add(Me.pbCalibration)
        Me.Panel1.Controls.Add(Me.gbStructure)
        Me.Panel1.Controls.Add(Me.gbArmor)
        Me.Panel1.Controls.Add(Me.gbShield)
        Me.Panel1.Controls.Add(Me.lblPG)
        Me.Panel1.Controls.Add(Me.lblCPU)
        Me.Panel1.Controls.Add(Me.pbPG)
        Me.Panel1.Controls.Add(Me.pbCPU)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(248, 750)
        Me.Panel1.TabIndex = 0
        '
        'btnLog
        '
        Me.btnLog.Image = Global.EveHQ.HQF.My.Resources.Resources.imgLog
        Me.btnLog.Location = New System.Drawing.Point(85, 53)
        Me.btnLog.Name = "btnLog"
        Me.btnLog.Size = New System.Drawing.Size(32, 32)
        Me.btnLog.TabIndex = 28
        Me.ToolTip1.SetToolTip(Me.btnLog, "Audit Log")
        Me.btnLog.UseVisualStyleBackColor = True
        '
        'btnSkills
        '
        Me.btnSkills.Image = CType(resources.GetObject("btnSkills.Image"), System.Drawing.Image)
        Me.btnSkills.Location = New System.Drawing.Point(203, 8)
        Me.btnSkills.Name = "btnSkills"
        Me.btnSkills.Size = New System.Drawing.Size(32, 32)
        Me.btnSkills.TabIndex = 27
        Me.ToolTip1.SetToolTip(Me.btnSkills, "Skills")
        Me.btnSkills.UseVisualStyleBackColor = True
        '
        'gbDamage
        '
        Me.gbDamage.Controls.Add(Me.lblMissileVolley)
        Me.gbDamage.Controls.Add(Me.lblTurretVolley)
        Me.gbDamage.Location = New System.Drawing.Point(155, 101)
        Me.gbDamage.Name = "gbDamage"
        Me.gbDamage.Size = New System.Drawing.Size(88, 64)
        Me.gbDamage.TabIndex = 26
        Me.gbDamage.TabStop = False
        Me.gbDamage.Text = "Damage"
        '
        'lblMissileVolley
        '
        Me.lblMissileVolley.AutoSize = True
        Me.lblMissileVolley.Location = New System.Drawing.Point(7, 35)
        Me.lblMissileVolley.Name = "lblMissileVolley"
        Me.lblMissileVolley.Size = New System.Drawing.Size(26, 13)
        Me.lblMissileVolley.TabIndex = 1
        Me.lblMissileVolley.Text = "MV:"
        '
        'lblTurretVolley
        '
        Me.lblTurretVolley.AutoSize = True
        Me.lblTurretVolley.Location = New System.Drawing.Point(7, 20)
        Me.lblTurretVolley.Name = "lblTurretVolley"
        Me.lblTurretVolley.Size = New System.Drawing.Size(24, 13)
        Me.lblTurretVolley.TabIndex = 0
        Me.lblTurretVolley.Text = "TV:"
        '
        'btnTargetSpeed
        '
        Me.btnTargetSpeed.Image = Global.EveHQ.HQF.My.Resources.Resources.imgScanResolution
        Me.btnTargetSpeed.Location = New System.Drawing.Point(47, 53)
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
        Me.cboPilots.Location = New System.Drawing.Point(42, 15)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(155, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 24
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(6, 18)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(30, 13)
        Me.lblPilot.TabIndex = 23
        Me.lblPilot.Text = "Pilot:"
        '
        'line1
        '
        Me.line1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.line1.Location = New System.Drawing.Point(7, 48)
        Me.line1.Name = "line1"
        Me.line1.Size = New System.Drawing.Size(238, 2)
        Me.line1.TabIndex = 22
        '
        'lblEffectiveHP
        '
        Me.lblEffectiveHP.AutoSize = True
        Me.lblEffectiveHP.Location = New System.Drawing.Point(6, 168)
        Me.lblEffectiveHP.Name = "lblEffectiveHP"
        Me.lblEffectiveHP.Size = New System.Drawing.Size(112, 13)
        Me.lblEffectiveHP.TabIndex = 21
        Me.lblEffectiveHP.Text = "Effective HP: 000,000"
        '
        'progCalibration
        '
        Me.progCalibration.Location = New System.Drawing.Point(35, 155)
        Me.progCalibration.Name = "progCalibration"
        Me.progCalibration.Size = New System.Drawing.Size(101, 10)
        Me.progCalibration.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.progCalibration, "Calibration")
        Me.progCalibration.Value = 75
        '
        'progCPU
        '
        Me.progCPU.Location = New System.Drawing.Point(35, 107)
        Me.progCPU.Name = "progCPU"
        Me.progCPU.Size = New System.Drawing.Size(100, 10)
        Me.progCPU.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.progCPU, "CPU")
        Me.progCPU.Value = 25
        '
        'progPG
        '
        Me.progPG.Location = New System.Drawing.Point(35, 131)
        Me.progPG.Name = "progPG"
        Me.progPG.Size = New System.Drawing.Size(101, 10)
        Me.progPG.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.progPG, "Powergrid")
        Me.progPG.Value = 50
        '
        'btnDoomsdayCheck
        '
        Me.btnDoomsdayCheck.Image = CType(resources.GetObject("btnDoomsdayCheck.Image"), System.Drawing.Image)
        Me.btnDoomsdayCheck.Location = New System.Drawing.Point(9, 53)
        Me.btnDoomsdayCheck.Name = "btnDoomsdayCheck"
        Me.btnDoomsdayCheck.Size = New System.Drawing.Size(32, 32)
        Me.btnDoomsdayCheck.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.btnDoomsdayCheck, "Doomsday Resistance Check")
        Me.btnDoomsdayCheck.UseVisualStyleBackColor = True
        '
        'gbCargo
        '
        Me.gbCargo.Controls.Add(Me.lblDroneBandwidth)
        Me.gbCargo.Controls.Add(Me.progDroneBandwidth)
        Me.gbCargo.Controls.Add(Me.pbDroneBandwidth)
        Me.gbCargo.Controls.Add(Me.lblDroneBay)
        Me.gbCargo.Controls.Add(Me.pbDroneBay)
        Me.gbCargo.Controls.Add(Me.lblCargoBay)
        Me.gbCargo.Controls.Add(Me.pbCargoBay)
        Me.gbCargo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbCargo.Location = New System.Drawing.Point(3, 427)
        Me.gbCargo.Name = "gbCargo"
        Me.gbCargo.Size = New System.Drawing.Size(240, 75)
        Me.gbCargo.TabIndex = 19
        Me.gbCargo.TabStop = False
        Me.gbCargo.Text = "Cargo and Drones"
        Me.ToolTip1.SetToolTip(Me.gbCargo, "Cargo and Drone Information")
        '
        'lblDroneBandwidth
        '
        Me.lblDroneBandwidth.AutoSize = True
        Me.lblDroneBandwidth.Location = New System.Drawing.Point(37, 45)
        Me.lblDroneBandwidth.Name = "lblDroneBandwidth"
        Me.lblDroneBandwidth.Size = New System.Drawing.Size(54, 13)
        Me.lblDroneBandwidth.TabIndex = 10
        Me.lblDroneBandwidth.Text = "000 / 000"
        Me.ToolTip1.SetToolTip(Me.lblDroneBandwidth, "Drone Bandwidth")
        '
        'progDroneBandwidth
        '
        Me.progDroneBandwidth.Location = New System.Drawing.Point(27, 58)
        Me.progDroneBandwidth.Name = "progDroneBandwidth"
        Me.progDroneBandwidth.Size = New System.Drawing.Size(170, 10)
        Me.progDroneBandwidth.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.progDroneBandwidth, "Drone Bandwidth")
        Me.progDroneBandwidth.Value = 50
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
        Me.lblDroneBay.Size = New System.Drawing.Size(57, 13)
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
        Me.lblCargoBay.Size = New System.Drawing.Size(63, 13)
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
        Me.gbPropulsion.Location = New System.Drawing.Point(3, 586)
        Me.gbPropulsion.Name = "gbPropulsion"
        Me.gbPropulsion.Size = New System.Drawing.Size(240, 75)
        Me.gbPropulsion.TabIndex = 18
        Me.gbPropulsion.TabStop = False
        Me.gbPropulsion.Text = "Propulsion"
        Me.ToolTip1.SetToolTip(Me.gbPropulsion, "Propulsion Information")
        '
        'lblAlignTime
        '
        Me.lblAlignTime.AutoSize = True
        Me.lblAlignTime.Location = New System.Drawing.Point(138, 45)
        Me.lblAlignTime.Name = "lblAlignTime"
        Me.lblAlignTime.Size = New System.Drawing.Size(42, 13)
        Me.lblAlignTime.TabIndex = 11
        Me.lblAlignTime.Text = "00.00 s"
        Me.ToolTip1.SetToolTip(Me.lblAlignTime, "Warp Align Time")
        '
        'pbAlignTime
        '
        Me.pbAlignTime.Image = Global.EveHQ.HQF.My.Resources.Resources.imgWarpAlign
        Me.pbAlignTime.Location = New System.Drawing.Point(108, 42)
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
        Me.lblInertia.Location = New System.Drawing.Point(36, 45)
        Me.lblInertia.Name = "lblInertia"
        Me.lblInertia.Size = New System.Drawing.Size(40, 13)
        Me.lblInertia.TabIndex = 8
        Me.lblInertia.Text = "0.0000"
        Me.ToolTip1.SetToolTip(Me.lblInertia, "Inertia")
        '
        'pbInertia
        '
        Me.pbInertia.Image = Global.EveHQ.HQF.My.Resources.Resources.imgInertia
        Me.pbInertia.Location = New System.Drawing.Point(6, 42)
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
        Me.lblWarpSpeed.Location = New System.Drawing.Point(138, 22)
        Me.lblWarpSpeed.Name = "lblWarpSpeed"
        Me.lblWarpSpeed.Size = New System.Drawing.Size(59, 13)
        Me.lblWarpSpeed.TabIndex = 5
        Me.lblWarpSpeed.Text = "00.00 au/s"
        Me.ToolTip1.SetToolTip(Me.lblWarpSpeed, "Warp Speed")
        '
        'pbWarpSpeed
        '
        Me.pbWarpSpeed.Image = Global.EveHQ.HQF.My.Resources.Resources.imgWarpSpeed
        Me.pbWarpSpeed.Location = New System.Drawing.Point(108, 19)
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
        Me.lblSpeed.Location = New System.Drawing.Point(36, 22)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(61, 13)
        Me.lblSpeed.TabIndex = 2
        Me.lblSpeed.Text = "00,000 m/s"
        Me.ToolTip1.SetToolTip(Me.lblSpeed, "Max Velocity")
        '
        'pbSpeed
        '
        Me.pbSpeed.Image = Global.EveHQ.HQF.My.Resources.Resources.imgSpeed
        Me.pbSpeed.Location = New System.Drawing.Point(6, 19)
        Me.pbSpeed.Name = "pbSpeed"
        Me.pbSpeed.Size = New System.Drawing.Size(24, 24)
        Me.pbSpeed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbSpeed.TabIndex = 0
        Me.pbSpeed.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbSpeed, "Max Velocity")
        '
        'gbCapacitor
        '
        Me.gbCapacitor.Controls.Add(Me.lblCapPeak)
        Me.gbCapacitor.Controls.Add(Me.pbCapPeak)
        Me.gbCapacitor.Controls.Add(Me.lblCapRecharge)
        Me.gbCapacitor.Controls.Add(Me.pbCapRecharge)
        Me.gbCapacitor.Controls.Add(Me.lblCapAverage)
        Me.gbCapacitor.Controls.Add(Me.pbCapAverage)
        Me.gbCapacitor.Controls.Add(Me.lblCapacitor)
        Me.gbCapacitor.Controls.Add(Me.pbCapacitor)
        Me.gbCapacitor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbCapacitor.Location = New System.Drawing.Point(3, 508)
        Me.gbCapacitor.Name = "gbCapacitor"
        Me.gbCapacitor.Size = New System.Drawing.Size(240, 75)
        Me.gbCapacitor.TabIndex = 17
        Me.gbCapacitor.TabStop = False
        Me.gbCapacitor.Text = "Capacitor"
        Me.ToolTip1.SetToolTip(Me.gbCapacitor, "Capacitor Information")
        '
        'lblCapPeak
        '
        Me.lblCapPeak.AutoSize = True
        Me.lblCapPeak.Location = New System.Drawing.Point(139, 45)
        Me.lblCapPeak.Name = "lblCapPeak"
        Me.lblCapPeak.Size = New System.Drawing.Size(40, 13)
        Me.lblCapPeak.TabIndex = 11
        Me.lblCapPeak.Text = "000.00"
        Me.ToolTip1.SetToolTip(Me.lblCapPeak, "Peak Recharge Rate")
        '
        'pbCapPeak
        '
        Me.pbCapPeak.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCapPeak
        Me.pbCapPeak.Location = New System.Drawing.Point(109, 42)
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
        Me.lblCapRecharge.Location = New System.Drawing.Point(36, 45)
        Me.lblCapRecharge.Name = "lblCapRecharge"
        Me.lblCapRecharge.Size = New System.Drawing.Size(42, 13)
        Me.lblCapRecharge.TabIndex = 8
        Me.lblCapRecharge.Text = "0,000 s"
        Me.ToolTip1.SetToolTip(Me.lblCapRecharge, "Recharge Time")
        '
        'pbCapRecharge
        '
        Me.pbCapRecharge.Image = Global.EveHQ.HQF.My.Resources.Resources.imgTimer
        Me.pbCapRecharge.Location = New System.Drawing.Point(6, 42)
        Me.pbCapRecharge.Name = "pbCapRecharge"
        Me.pbCapRecharge.Size = New System.Drawing.Size(24, 24)
        Me.pbCapRecharge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbCapRecharge.TabIndex = 6
        Me.pbCapRecharge.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCapRecharge, "Recharge Time")
        '
        'lblCapAverage
        '
        Me.lblCapAverage.AutoSize = True
        Me.lblCapAverage.Location = New System.Drawing.Point(139, 22)
        Me.lblCapAverage.Name = "lblCapAverage"
        Me.lblCapAverage.Size = New System.Drawing.Size(40, 13)
        Me.lblCapAverage.TabIndex = 5
        Me.lblCapAverage.Text = "000.00"
        Me.ToolTip1.SetToolTip(Me.lblCapAverage, "Average Recharge Rate")
        '
        'pbCapAverage
        '
        Me.pbCapAverage.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCapAverage
        Me.pbCapAverage.Location = New System.Drawing.Point(109, 19)
        Me.pbCapAverage.Name = "pbCapAverage"
        Me.pbCapAverage.Size = New System.Drawing.Size(24, 24)
        Me.pbCapAverage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCapAverage.TabIndex = 3
        Me.pbCapAverage.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCapAverage, "Average Recharge Rate")
        '
        'lblCapacitor
        '
        Me.lblCapacitor.AutoSize = True
        Me.lblCapacitor.Location = New System.Drawing.Point(36, 22)
        Me.lblCapacitor.Name = "lblCapacitor"
        Me.lblCapacitor.Size = New System.Drawing.Size(40, 13)
        Me.lblCapacitor.TabIndex = 2
        Me.lblCapacitor.Text = "00,000"
        Me.ToolTip1.SetToolTip(Me.lblCapacitor, "Capacity")
        '
        'pbCapacitor
        '
        Me.pbCapacitor.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCap
        Me.pbCapacitor.Location = New System.Drawing.Point(6, 19)
        Me.pbCapacitor.Name = "pbCapacitor"
        Me.pbCapacitor.Size = New System.Drawing.Size(24, 24)
        Me.pbCapacitor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbCapacitor.TabIndex = 0
        Me.pbCapacitor.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCapacitor, "Capacity")
        '
        'gbTargeting
        '
        Me.gbTargeting.Controls.Add(Me.lblSensorStrength)
        Me.gbTargeting.Controls.Add(Me.pbSensorStrength)
        Me.gbTargeting.Controls.Add(Me.lblScanResolution)
        Me.gbTargeting.Controls.Add(Me.pbScanResolution)
        Me.gbTargeting.Controls.Add(Me.lblMaxTargets)
        Me.gbTargeting.Controls.Add(Me.pbMaxTargets)
        Me.gbTargeting.Controls.Add(Me.lblTargetRange)
        Me.gbTargeting.Controls.Add(Me.pbTargetRange)
        Me.gbTargeting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbTargeting.Location = New System.Drawing.Point(3, 667)
        Me.gbTargeting.Name = "gbTargeting"
        Me.gbTargeting.Size = New System.Drawing.Size(240, 75)
        Me.gbTargeting.TabIndex = 14
        Me.gbTargeting.TabStop = False
        Me.gbTargeting.Text = "Targeting"
        Me.ToolTip1.SetToolTip(Me.gbTargeting, "Targeting Information")
        '
        'lblSensorStrength
        '
        Me.lblSensorStrength.AutoSize = True
        Me.lblSensorStrength.Location = New System.Drawing.Point(139, 45)
        Me.lblSensorStrength.Name = "lblSensorStrength"
        Me.lblSensorStrength.Size = New System.Drawing.Size(19, 13)
        Me.lblSensorStrength.TabIndex = 11
        Me.lblSensorStrength.Text = "00"
        Me.ToolTip1.SetToolTip(Me.lblSensorStrength, "Sensor Strength")
        '
        'pbSensorStrength
        '
        Me.pbSensorStrength.Image = Global.EveHQ.HQF.My.Resources.Resources.imgSensorStregthL
        Me.pbSensorStrength.Location = New System.Drawing.Point(109, 42)
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
        'lblMaxTargets
        '
        Me.lblMaxTargets.AutoSize = True
        Me.lblMaxTargets.Location = New System.Drawing.Point(139, 22)
        Me.lblMaxTargets.Name = "lblMaxTargets"
        Me.lblMaxTargets.Size = New System.Drawing.Size(19, 13)
        Me.lblMaxTargets.TabIndex = 5
        Me.lblMaxTargets.Text = "00"
        Me.ToolTip1.SetToolTip(Me.lblMaxTargets, "Max Locked Targets")
        '
        'pbMaxTargets
        '
        Me.pbMaxTargets.Image = Global.EveHQ.HQF.My.Resources.Resources.imgMaxTargets
        Me.pbMaxTargets.Location = New System.Drawing.Point(109, 19)
        Me.pbMaxTargets.Name = "pbMaxTargets"
        Me.pbMaxTargets.Size = New System.Drawing.Size(24, 24)
        Me.pbMaxTargets.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbMaxTargets.TabIndex = 3
        Me.pbMaxTargets.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbMaxTargets, "Max Locked Targets")
        '
        'lblTargetRange
        '
        Me.lblTargetRange.AutoSize = True
        Me.lblTargetRange.Location = New System.Drawing.Point(36, 22)
        Me.lblTargetRange.Name = "lblTargetRange"
        Me.lblTargetRange.Size = New System.Drawing.Size(57, 13)
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
        Me.lblCalibration.Location = New System.Drawing.Point(35, 143)
        Me.lblCalibration.Name = "lblCalibration"
        Me.lblCalibration.Size = New System.Drawing.Size(30, 13)
        Me.lblCalibration.TabIndex = 13
        Me.lblCalibration.Text = "0 / 0"
        Me.ToolTip1.SetToolTip(Me.lblCalibration, "Calibration")
        '
        'pbCalibration
        '
        Me.pbCalibration.Image = Global.EveHQ.HQF.My.Resources.Resources.imgCalibration
        Me.pbCalibration.Location = New System.Drawing.Point(7, 141)
        Me.pbCalibration.Name = "pbCalibration"
        Me.pbCalibration.Size = New System.Drawing.Size(24, 24)
        Me.pbCalibration.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCalibration.TabIndex = 11
        Me.pbCalibration.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCalibration, "Calibration")
        '
        'gbStructure
        '
        Me.gbStructure.Controls.Add(Me.lblSigRadius)
        Me.gbStructure.Controls.Add(Me.pbSigRadius)
        Me.gbStructure.Controls.Add(Me.progStructureThermal)
        Me.gbStructure.Controls.Add(Me.progStructureExplosive)
        Me.gbStructure.Controls.Add(Me.progStructureKinetic)
        Me.gbStructure.Controls.Add(Me.progStructureEM)
        Me.gbStructure.Controls.Add(Me.lblStructureHP)
        Me.gbStructure.Controls.Add(Me.pbStructureHP)
        Me.gbStructure.Controls.Add(Me.lblStructureThermal)
        Me.gbStructure.Controls.Add(Me.pbStructureThermal)
        Me.gbStructure.Controls.Add(Me.lblStructureExplosive)
        Me.gbStructure.Controls.Add(Me.pbStructureExplosive)
        Me.gbStructure.Controls.Add(Me.lblStructureKinetic)
        Me.gbStructure.Controls.Add(Me.pbStructureKinetic)
        Me.gbStructure.Controls.Add(Me.lblStructureEM)
        Me.gbStructure.Controls.Add(Me.pbStructureEM)
        Me.gbStructure.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbStructure.Location = New System.Drawing.Point(3, 346)
        Me.gbStructure.Name = "gbStructure"
        Me.gbStructure.Size = New System.Drawing.Size(240, 75)
        Me.gbStructure.TabIndex = 10
        Me.gbStructure.TabStop = False
        Me.gbStructure.Text = "Structure"
        Me.ToolTip1.SetToolTip(Me.gbStructure, "Structure Information")
        '
        'lblSigRadius
        '
        Me.lblSigRadius.AutoSize = True
        Me.lblSigRadius.Location = New System.Drawing.Point(27, 49)
        Me.lblSigRadius.Name = "lblSigRadius"
        Me.lblSigRadius.Size = New System.Drawing.Size(45, 13)
        Me.lblSigRadius.TabIndex = 16
        Me.lblSigRadius.Text = "0,000 m"
        Me.ToolTip1.SetToolTip(Me.lblSigRadius, "Signature Radius")
        '
        'pbSigRadius
        '
        Me.pbSigRadius.Image = Global.EveHQ.HQF.My.Resources.Resources.imgSigRadius
        Me.pbSigRadius.Location = New System.Drawing.Point(6, 43)
        Me.pbSigRadius.Name = "pbSigRadius"
        Me.pbSigRadius.Size = New System.Drawing.Size(24, 24)
        Me.pbSigRadius.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbSigRadius.TabIndex = 15
        Me.pbSigRadius.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbSigRadius, "Signature Radius")
        '
        'progStructureThermal
        '
        Me.progStructureThermal.Location = New System.Drawing.Point(183, 54)
        Me.progStructureThermal.Name = "progStructureThermal"
        Me.progStructureThermal.Size = New System.Drawing.Size(50, 10)
        Me.progStructureThermal.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.progStructureThermal, "Thermal Resistance")
        Me.progStructureThermal.Value = 50
        '
        'progStructureExplosive
        '
        Me.progStructureExplosive.Location = New System.Drawing.Point(107, 54)
        Me.progStructureExplosive.Name = "progStructureExplosive"
        Me.progStructureExplosive.Size = New System.Drawing.Size(50, 10)
        Me.progStructureExplosive.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.progStructureExplosive, "Explosive Resistance")
        Me.progStructureExplosive.Value = 50
        '
        'progStructureKinetic
        '
        Me.progStructureKinetic.Location = New System.Drawing.Point(183, 31)
        Me.progStructureKinetic.Name = "progStructureKinetic"
        Me.progStructureKinetic.Size = New System.Drawing.Size(50, 10)
        Me.progStructureKinetic.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.progStructureKinetic, "Kinetic Resistance")
        Me.progStructureKinetic.Value = 50
        '
        'progStructureEM
        '
        Me.progStructureEM.Location = New System.Drawing.Point(107, 31)
        Me.progStructureEM.Name = "progStructureEM"
        Me.progStructureEM.Size = New System.Drawing.Size(50, 10)
        Me.progStructureEM.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.progStructureEM, "EM Resistance")
        Me.progStructureEM.Value = 50
        '
        'lblStructureHP
        '
        Me.lblStructureHP.AutoSize = True
        Me.lblStructureHP.Location = New System.Drawing.Point(27, 23)
        Me.lblStructureHP.Name = "lblStructureHP"
        Me.lblStructureHP.Size = New System.Drawing.Size(46, 13)
        Me.lblStructureHP.TabIndex = 14
        Me.lblStructureHP.Text = "000,000"
        Me.ToolTip1.SetToolTip(Me.lblStructureHP, "Hitpoints")
        '
        'pbStructureHP
        '
        Me.pbStructureHP.Image = Global.EveHQ.HQF.My.Resources.Resources.imgStructure
        Me.pbStructureHP.Location = New System.Drawing.Point(6, 17)
        Me.pbStructureHP.Name = "pbStructureHP"
        Me.pbStructureHP.Size = New System.Drawing.Size(24, 24)
        Me.pbStructureHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbStructureHP.TabIndex = 12
        Me.pbStructureHP.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbStructureHP, "Hitpoints")
        '
        'lblStructureThermal
        '
        Me.lblStructureThermal.AutoSize = True
        Me.lblStructureThermal.Location = New System.Drawing.Point(187, 42)
        Me.lblStructureThermal.Name = "lblStructureThermal"
        Me.lblStructureThermal.Size = New System.Drawing.Size(21, 13)
        Me.lblStructureThermal.TabIndex = 11
        Me.lblStructureThermal.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblStructureThermal, "Thermal Resistance")
        '
        'pbStructureThermal
        '
        Me.pbStructureThermal.Image = Global.EveHQ.HQF.My.Resources.Resources.imgThermalResist
        Me.pbStructureThermal.Location = New System.Drawing.Point(163, 40)
        Me.pbStructureThermal.Name = "pbStructureThermal"
        Me.pbStructureThermal.Size = New System.Drawing.Size(24, 24)
        Me.pbStructureThermal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbStructureThermal.TabIndex = 9
        Me.pbStructureThermal.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbStructureThermal, "Thermal Resistance")
        '
        'lblStructureExplosive
        '
        Me.lblStructureExplosive.AutoSize = True
        Me.lblStructureExplosive.Location = New System.Drawing.Point(111, 42)
        Me.lblStructureExplosive.Name = "lblStructureExplosive"
        Me.lblStructureExplosive.Size = New System.Drawing.Size(21, 13)
        Me.lblStructureExplosive.TabIndex = 8
        Me.lblStructureExplosive.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblStructureExplosive, "Explosive Resistance")
        '
        'pbStructureExplosive
        '
        Me.pbStructureExplosive.Image = Global.EveHQ.HQF.My.Resources.Resources.imgExplosiveResist
        Me.pbStructureExplosive.Location = New System.Drawing.Point(87, 40)
        Me.pbStructureExplosive.Name = "pbStructureExplosive"
        Me.pbStructureExplosive.Size = New System.Drawing.Size(24, 24)
        Me.pbStructureExplosive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbStructureExplosive.TabIndex = 6
        Me.pbStructureExplosive.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbStructureExplosive, "Explosive Resistance")
        '
        'lblStructureKinetic
        '
        Me.lblStructureKinetic.AutoSize = True
        Me.lblStructureKinetic.Location = New System.Drawing.Point(187, 19)
        Me.lblStructureKinetic.Name = "lblStructureKinetic"
        Me.lblStructureKinetic.Size = New System.Drawing.Size(21, 13)
        Me.lblStructureKinetic.TabIndex = 5
        Me.lblStructureKinetic.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblStructureKinetic, "Kinetic Resistance")
        '
        'pbStructureKinetic
        '
        Me.pbStructureKinetic.Image = Global.EveHQ.HQF.My.Resources.Resources.imgKineticResist
        Me.pbStructureKinetic.Location = New System.Drawing.Point(163, 17)
        Me.pbStructureKinetic.Name = "pbStructureKinetic"
        Me.pbStructureKinetic.Size = New System.Drawing.Size(24, 24)
        Me.pbStructureKinetic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbStructureKinetic.TabIndex = 3
        Me.pbStructureKinetic.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbStructureKinetic, "Kinetic Resistance")
        '
        'lblStructureEM
        '
        Me.lblStructureEM.AutoSize = True
        Me.lblStructureEM.Location = New System.Drawing.Point(111, 19)
        Me.lblStructureEM.Name = "lblStructureEM"
        Me.lblStructureEM.Size = New System.Drawing.Size(21, 13)
        Me.lblStructureEM.TabIndex = 2
        Me.lblStructureEM.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblStructureEM, "EM Resistance")
        '
        'pbStructureEM
        '
        Me.pbStructureEM.Image = Global.EveHQ.HQF.My.Resources.Resources.imgEMResist
        Me.pbStructureEM.Location = New System.Drawing.Point(87, 17)
        Me.pbStructureEM.Name = "pbStructureEM"
        Me.pbStructureEM.Size = New System.Drawing.Size(24, 24)
        Me.pbStructureEM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbStructureEM.TabIndex = 0
        Me.pbStructureEM.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbStructureEM, "EM Resistance")
        '
        'gbArmor
        '
        Me.gbArmor.Controls.Add(Me.progArmorThermal)
        Me.gbArmor.Controls.Add(Me.progArmorExplosive)
        Me.gbArmor.Controls.Add(Me.progArmorKinetic)
        Me.gbArmor.Controls.Add(Me.progArmorEM)
        Me.gbArmor.Controls.Add(Me.lblArmorHP)
        Me.gbArmor.Controls.Add(Me.pbArmorHP)
        Me.gbArmor.Controls.Add(Me.lblArmorThermal)
        Me.gbArmor.Controls.Add(Me.pbArmorThermal)
        Me.gbArmor.Controls.Add(Me.lblArmorExplosive)
        Me.gbArmor.Controls.Add(Me.pbArmorExplosive)
        Me.gbArmor.Controls.Add(Me.lblArmorKinetic)
        Me.gbArmor.Controls.Add(Me.pbArmorKinetic)
        Me.gbArmor.Controls.Add(Me.lblArmorEM)
        Me.gbArmor.Controls.Add(Me.pbArmorEM)
        Me.gbArmor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbArmor.Location = New System.Drawing.Point(3, 265)
        Me.gbArmor.Name = "gbArmor"
        Me.gbArmor.Size = New System.Drawing.Size(240, 75)
        Me.gbArmor.TabIndex = 9
        Me.gbArmor.TabStop = False
        Me.gbArmor.Text = "Armor"
        Me.ToolTip1.SetToolTip(Me.gbArmor, "Armor Information")
        '
        'progArmorThermal
        '
        Me.progArmorThermal.Location = New System.Drawing.Point(183, 54)
        Me.progArmorThermal.Name = "progArmorThermal"
        Me.progArmorThermal.Size = New System.Drawing.Size(50, 10)
        Me.progArmorThermal.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.progArmorThermal, "Thermal Resistance")
        Me.progArmorThermal.Value = 50
        '
        'progArmorExplosive
        '
        Me.progArmorExplosive.Location = New System.Drawing.Point(107, 54)
        Me.progArmorExplosive.Name = "progArmorExplosive"
        Me.progArmorExplosive.Size = New System.Drawing.Size(50, 10)
        Me.progArmorExplosive.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.progArmorExplosive, "Explosive Resistance")
        Me.progArmorExplosive.Value = 50
        '
        'progArmorKinetic
        '
        Me.progArmorKinetic.Location = New System.Drawing.Point(183, 31)
        Me.progArmorKinetic.Name = "progArmorKinetic"
        Me.progArmorKinetic.Size = New System.Drawing.Size(50, 10)
        Me.progArmorKinetic.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.progArmorKinetic, "Kinetic Resistance")
        Me.progArmorKinetic.Value = 50
        '
        'progArmorEM
        '
        Me.progArmorEM.Location = New System.Drawing.Point(107, 31)
        Me.progArmorEM.Name = "progArmorEM"
        Me.progArmorEM.Size = New System.Drawing.Size(50, 10)
        Me.progArmorEM.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.progArmorEM, "EM Resistance")
        Me.progArmorEM.Value = 50
        '
        'lblArmorHP
        '
        Me.lblArmorHP.AutoSize = True
        Me.lblArmorHP.Location = New System.Drawing.Point(27, 23)
        Me.lblArmorHP.Name = "lblArmorHP"
        Me.lblArmorHP.Size = New System.Drawing.Size(46, 13)
        Me.lblArmorHP.TabIndex = 14
        Me.lblArmorHP.Text = "000,000"
        Me.ToolTip1.SetToolTip(Me.lblArmorHP, "Hitpoints")
        '
        'pbArmorHP
        '
        Me.pbArmorHP.Image = Global.EveHQ.HQF.My.Resources.Resources.imgArmor
        Me.pbArmorHP.Location = New System.Drawing.Point(6, 17)
        Me.pbArmorHP.Name = "pbArmorHP"
        Me.pbArmorHP.Size = New System.Drawing.Size(24, 24)
        Me.pbArmorHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbArmorHP.TabIndex = 12
        Me.pbArmorHP.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbArmorHP, "Hitpoints")
        '
        'lblArmorThermal
        '
        Me.lblArmorThermal.AutoSize = True
        Me.lblArmorThermal.Location = New System.Drawing.Point(187, 42)
        Me.lblArmorThermal.Name = "lblArmorThermal"
        Me.lblArmorThermal.Size = New System.Drawing.Size(21, 13)
        Me.lblArmorThermal.TabIndex = 11
        Me.lblArmorThermal.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblArmorThermal, "Thermal Resistance")
        '
        'pbArmorThermal
        '
        Me.pbArmorThermal.Image = Global.EveHQ.HQF.My.Resources.Resources.imgThermalResist
        Me.pbArmorThermal.Location = New System.Drawing.Point(163, 40)
        Me.pbArmorThermal.Name = "pbArmorThermal"
        Me.pbArmorThermal.Size = New System.Drawing.Size(24, 24)
        Me.pbArmorThermal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbArmorThermal.TabIndex = 9
        Me.pbArmorThermal.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbArmorThermal, "Thermal Resistance")
        '
        'lblArmorExplosive
        '
        Me.lblArmorExplosive.AutoSize = True
        Me.lblArmorExplosive.Location = New System.Drawing.Point(111, 42)
        Me.lblArmorExplosive.Name = "lblArmorExplosive"
        Me.lblArmorExplosive.Size = New System.Drawing.Size(21, 13)
        Me.lblArmorExplosive.TabIndex = 8
        Me.lblArmorExplosive.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblArmorExplosive, "Explosive Resistance")
        '
        'pbArmorExplosive
        '
        Me.pbArmorExplosive.Image = Global.EveHQ.HQF.My.Resources.Resources.imgExplosiveResist
        Me.pbArmorExplosive.Location = New System.Drawing.Point(87, 40)
        Me.pbArmorExplosive.Name = "pbArmorExplosive"
        Me.pbArmorExplosive.Size = New System.Drawing.Size(24, 24)
        Me.pbArmorExplosive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbArmorExplosive.TabIndex = 6
        Me.pbArmorExplosive.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbArmorExplosive, "Explosive Resistance")
        '
        'lblArmorKinetic
        '
        Me.lblArmorKinetic.AutoSize = True
        Me.lblArmorKinetic.Location = New System.Drawing.Point(187, 19)
        Me.lblArmorKinetic.Name = "lblArmorKinetic"
        Me.lblArmorKinetic.Size = New System.Drawing.Size(21, 13)
        Me.lblArmorKinetic.TabIndex = 5
        Me.lblArmorKinetic.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblArmorKinetic, "Kinetic Resistance")
        '
        'pbArmorKinetic
        '
        Me.pbArmorKinetic.Image = Global.EveHQ.HQF.My.Resources.Resources.imgKineticResist
        Me.pbArmorKinetic.Location = New System.Drawing.Point(163, 17)
        Me.pbArmorKinetic.Name = "pbArmorKinetic"
        Me.pbArmorKinetic.Size = New System.Drawing.Size(24, 24)
        Me.pbArmorKinetic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbArmorKinetic.TabIndex = 3
        Me.pbArmorKinetic.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbArmorKinetic, "Kinetic Resistance")
        '
        'lblArmorEM
        '
        Me.lblArmorEM.AutoSize = True
        Me.lblArmorEM.Location = New System.Drawing.Point(111, 19)
        Me.lblArmorEM.Name = "lblArmorEM"
        Me.lblArmorEM.Size = New System.Drawing.Size(21, 13)
        Me.lblArmorEM.TabIndex = 2
        Me.lblArmorEM.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblArmorEM, "EM Resistance")
        '
        'pbArmorEM
        '
        Me.pbArmorEM.Image = Global.EveHQ.HQF.My.Resources.Resources.imgEMResist
        Me.pbArmorEM.Location = New System.Drawing.Point(87, 17)
        Me.pbArmorEM.Name = "pbArmorEM"
        Me.pbArmorEM.Size = New System.Drawing.Size(24, 24)
        Me.pbArmorEM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbArmorEM.TabIndex = 0
        Me.pbArmorEM.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbArmorEM, "EM Resistance")
        '
        'gbShield
        '
        Me.gbShield.Controls.Add(Me.progShieldThermal)
        Me.gbShield.Controls.Add(Me.progShieldExp)
        Me.gbShield.Controls.Add(Me.progShieldKinetic)
        Me.gbShield.Controls.Add(Me.progShieldEM)
        Me.gbShield.Controls.Add(Me.lblShieldRecharge)
        Me.gbShield.Controls.Add(Me.lblShieldHP)
        Me.gbShield.Controls.Add(Me.pbShieldRecharge)
        Me.gbShield.Controls.Add(Me.pbShieldHP)
        Me.gbShield.Controls.Add(Me.lblShieldThermal)
        Me.gbShield.Controls.Add(Me.pbShieldThermal)
        Me.gbShield.Controls.Add(Me.lblShieldExplosive)
        Me.gbShield.Controls.Add(Me.pbShieldExplosive)
        Me.gbShield.Controls.Add(Me.lblShieldKinetic)
        Me.gbShield.Controls.Add(Me.pbShieldKinetic)
        Me.gbShield.Controls.Add(Me.lblShieldEM)
        Me.gbShield.Controls.Add(Me.pbShieldEM)
        Me.gbShield.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbShield.Location = New System.Drawing.Point(3, 184)
        Me.gbShield.Name = "gbShield"
        Me.gbShield.Size = New System.Drawing.Size(240, 75)
        Me.gbShield.TabIndex = 8
        Me.gbShield.TabStop = False
        Me.gbShield.Text = "Shield"
        Me.ToolTip1.SetToolTip(Me.gbShield, "Shield Information")
        '
        'progShieldThermal
        '
        Me.progShieldThermal.Location = New System.Drawing.Point(183, 54)
        Me.progShieldThermal.Name = "progShieldThermal"
        Me.progShieldThermal.Size = New System.Drawing.Size(50, 10)
        Me.progShieldThermal.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.progShieldThermal, "Thermal Resistance")
        Me.progShieldThermal.Value = 50
        '
        'progShieldExp
        '
        Me.progShieldExp.Location = New System.Drawing.Point(107, 54)
        Me.progShieldExp.Name = "progShieldExp"
        Me.progShieldExp.Size = New System.Drawing.Size(50, 10)
        Me.progShieldExp.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.progShieldExp, "Explosive Resistance")
        Me.progShieldExp.Value = 50
        '
        'progShieldKinetic
        '
        Me.progShieldKinetic.Location = New System.Drawing.Point(183, 31)
        Me.progShieldKinetic.Name = "progShieldKinetic"
        Me.progShieldKinetic.Size = New System.Drawing.Size(50, 10)
        Me.progShieldKinetic.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.progShieldKinetic, "Kinetic Resistance")
        Me.progShieldKinetic.Value = 50
        '
        'progShieldEM
        '
        Me.progShieldEM.Location = New System.Drawing.Point(107, 31)
        Me.progShieldEM.Name = "progShieldEM"
        Me.progShieldEM.Size = New System.Drawing.Size(50, 10)
        Me.progShieldEM.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.progShieldEM, "EM Resistance")
        Me.progShieldEM.Value = 50
        '
        'lblShieldRecharge
        '
        Me.lblShieldRecharge.AutoSize = True
        Me.lblShieldRecharge.Location = New System.Drawing.Point(28, 47)
        Me.lblShieldRecharge.Name = "lblShieldRecharge"
        Me.lblShieldRecharge.Size = New System.Drawing.Size(21, 13)
        Me.lblShieldRecharge.TabIndex = 15
        Me.lblShieldRecharge.Text = "0 s"
        Me.ToolTip1.SetToolTip(Me.lblShieldRecharge, "Recharge Time")
        '
        'lblShieldHP
        '
        Me.lblShieldHP.AutoSize = True
        Me.lblShieldHP.Location = New System.Drawing.Point(28, 23)
        Me.lblShieldHP.Name = "lblShieldHP"
        Me.lblShieldHP.Size = New System.Drawing.Size(46, 13)
        Me.lblShieldHP.TabIndex = 14
        Me.lblShieldHP.Text = "000,000"
        Me.ToolTip1.SetToolTip(Me.lblShieldHP, "Hitpoints")
        '
        'pbShieldRecharge
        '
        Me.pbShieldRecharge.Image = Global.EveHQ.HQF.My.Resources.Resources.imgTimer
        Me.pbShieldRecharge.Location = New System.Drawing.Point(7, 41)
        Me.pbShieldRecharge.Name = "pbShieldRecharge"
        Me.pbShieldRecharge.Size = New System.Drawing.Size(24, 24)
        Me.pbShieldRecharge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbShieldRecharge.TabIndex = 13
        Me.pbShieldRecharge.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShieldRecharge, "Recharge Time")
        '
        'pbShieldHP
        '
        Me.pbShieldHP.Image = Global.EveHQ.HQF.My.Resources.Resources.imgShield
        Me.pbShieldHP.Location = New System.Drawing.Point(7, 17)
        Me.pbShieldHP.Name = "pbShieldHP"
        Me.pbShieldHP.Size = New System.Drawing.Size(24, 24)
        Me.pbShieldHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbShieldHP.TabIndex = 12
        Me.pbShieldHP.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShieldHP, "Hitpoints")
        '
        'lblShieldThermal
        '
        Me.lblShieldThermal.AutoSize = True
        Me.lblShieldThermal.Location = New System.Drawing.Point(187, 42)
        Me.lblShieldThermal.Name = "lblShieldThermal"
        Me.lblShieldThermal.Size = New System.Drawing.Size(21, 13)
        Me.lblShieldThermal.TabIndex = 11
        Me.lblShieldThermal.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblShieldThermal, "Thermal Resistance")
        '
        'pbShieldThermal
        '
        Me.pbShieldThermal.Image = Global.EveHQ.HQF.My.Resources.Resources.imgThermalResist
        Me.pbShieldThermal.Location = New System.Drawing.Point(163, 40)
        Me.pbShieldThermal.Name = "pbShieldThermal"
        Me.pbShieldThermal.Size = New System.Drawing.Size(24, 24)
        Me.pbShieldThermal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbShieldThermal.TabIndex = 9
        Me.pbShieldThermal.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShieldThermal, "Thermal Resistance")
        '
        'lblShieldExplosive
        '
        Me.lblShieldExplosive.AutoSize = True
        Me.lblShieldExplosive.Location = New System.Drawing.Point(111, 42)
        Me.lblShieldExplosive.Name = "lblShieldExplosive"
        Me.lblShieldExplosive.Size = New System.Drawing.Size(21, 13)
        Me.lblShieldExplosive.TabIndex = 8
        Me.lblShieldExplosive.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblShieldExplosive, "Explosive Resistance")
        '
        'pbShieldExplosive
        '
        Me.pbShieldExplosive.Image = Global.EveHQ.HQF.My.Resources.Resources.imgExplosiveResist
        Me.pbShieldExplosive.Location = New System.Drawing.Point(87, 40)
        Me.pbShieldExplosive.Name = "pbShieldExplosive"
        Me.pbShieldExplosive.Size = New System.Drawing.Size(24, 24)
        Me.pbShieldExplosive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbShieldExplosive.TabIndex = 6
        Me.pbShieldExplosive.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShieldExplosive, "Explosive Resistance")
        '
        'lblShieldKinetic
        '
        Me.lblShieldKinetic.AutoSize = True
        Me.lblShieldKinetic.Location = New System.Drawing.Point(187, 19)
        Me.lblShieldKinetic.Name = "lblShieldKinetic"
        Me.lblShieldKinetic.Size = New System.Drawing.Size(21, 13)
        Me.lblShieldKinetic.TabIndex = 5
        Me.lblShieldKinetic.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblShieldKinetic, "Kinetic Resistance")
        '
        'pbShieldKinetic
        '
        Me.pbShieldKinetic.Image = Global.EveHQ.HQF.My.Resources.Resources.imgKineticResist
        Me.pbShieldKinetic.Location = New System.Drawing.Point(163, 17)
        Me.pbShieldKinetic.Name = "pbShieldKinetic"
        Me.pbShieldKinetic.Size = New System.Drawing.Size(24, 24)
        Me.pbShieldKinetic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbShieldKinetic.TabIndex = 3
        Me.pbShieldKinetic.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShieldKinetic, "Kinetic Resistance")
        '
        'lblShieldEM
        '
        Me.lblShieldEM.AutoSize = True
        Me.lblShieldEM.Location = New System.Drawing.Point(111, 19)
        Me.lblShieldEM.Name = "lblShieldEM"
        Me.lblShieldEM.Size = New System.Drawing.Size(21, 13)
        Me.lblShieldEM.TabIndex = 2
        Me.lblShieldEM.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.lblShieldEM, "EM Resistance")
        '
        'pbShieldEM
        '
        Me.pbShieldEM.Image = Global.EveHQ.HQF.My.Resources.Resources.imgEMResist
        Me.pbShieldEM.Location = New System.Drawing.Point(87, 17)
        Me.pbShieldEM.Name = "pbShieldEM"
        Me.pbShieldEM.Size = New System.Drawing.Size(24, 24)
        Me.pbShieldEM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbShieldEM.TabIndex = 0
        Me.pbShieldEM.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbShieldEM, "EM Resistance")
        '
        'lblPG
        '
        Me.lblPG.AutoSize = True
        Me.lblPG.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPG.Location = New System.Drawing.Point(35, 119)
        Me.lblPG.Name = "lblPG"
        Me.lblPG.Size = New System.Drawing.Size(30, 13)
        Me.lblPG.TabIndex = 7
        Me.lblPG.Text = "0 / 0"
        Me.ToolTip1.SetToolTip(Me.lblPG, "Powergrid")
        '
        'lblCPU
        '
        Me.lblCPU.AutoSize = True
        Me.lblCPU.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCPU.Location = New System.Drawing.Point(35, 95)
        Me.lblCPU.Name = "lblCPU"
        Me.lblCPU.Size = New System.Drawing.Size(30, 13)
        Me.lblCPU.TabIndex = 6
        Me.lblCPU.Text = "0 / 0"
        Me.ToolTip1.SetToolTip(Me.lblCPU, "CPU")
        '
        'pbPG
        '
        Me.pbPG.Image = Global.EveHQ.HQF.My.Resources.Resources.imgPG
        Me.pbPG.Location = New System.Drawing.Point(7, 117)
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
        Me.pbCPU.Location = New System.Drawing.Point(7, 93)
        Me.pbCPU.Name = "pbCPU"
        Me.pbCPU.Size = New System.Drawing.Size(24, 24)
        Me.pbCPU.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCPU.TabIndex = 1
        Me.pbCPU.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCPU, "CPU")
        '
        'Label1
        '
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(7, 88)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(238, 2)
        Me.Label1.TabIndex = 29
        '
        'ShipInfoControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Controls.Add(Me.Panel1)
        Me.Name = "ShipInfoControl"
        Me.Size = New System.Drawing.Size(248, 750)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.gbDamage.ResumeLayout(False)
        Me.gbDamage.PerformLayout()
        Me.gbCargo.ResumeLayout(False)
        Me.gbCargo.PerformLayout()
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
        CType(Me.pbCapPeak, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCapRecharge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCapAverage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCapacitor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbTargeting.ResumeLayout(False)
        Me.gbTargeting.PerformLayout()
        CType(Me.pbSensorStrength, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbScanResolution, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbMaxTargets, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbTargetRange, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCalibration, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbStructure.ResumeLayout(False)
        Me.gbStructure.PerformLayout()
        CType(Me.pbSigRadius, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructureHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructureThermal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructureExplosive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructureKinetic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructureEM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbArmor.ResumeLayout(False)
        Me.gbArmor.PerformLayout()
        CType(Me.pbArmorHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorThermal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorExplosive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorKinetic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorEM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbShield.ResumeLayout(False)
        Me.gbShield.PerformLayout()
        CType(Me.pbShieldRecharge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldThermal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldExplosive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldKinetic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldEM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCPU, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pbCPU As System.Windows.Forms.PictureBox
    Friend WithEvents pbPG As System.Windows.Forms.PictureBox
    Friend WithEvents progCPU As System.Windows.Forms.ProgressBar
    Friend WithEvents progPG As System.Windows.Forms.ProgressBar
    Friend WithEvents lblPG As System.Windows.Forms.Label
    Friend WithEvents lblCPU As System.Windows.Forms.Label
    Friend WithEvents gbShield As System.Windows.Forms.GroupBox
    Friend WithEvents pbShieldEM As System.Windows.Forms.PictureBox
    Friend WithEvents lblShieldEM As System.Windows.Forms.Label
    Friend WithEvents progShieldEM As System.Windows.Forms.ProgressBar
    Friend WithEvents pbShieldHP As System.Windows.Forms.PictureBox
    Friend WithEvents lblShieldThermal As System.Windows.Forms.Label
    Friend WithEvents progShieldThermal As System.Windows.Forms.ProgressBar
    Friend WithEvents pbShieldThermal As System.Windows.Forms.PictureBox
    Friend WithEvents lblShieldExplosive As System.Windows.Forms.Label
    Friend WithEvents progShieldExp As System.Windows.Forms.ProgressBar
    Friend WithEvents pbShieldExplosive As System.Windows.Forms.PictureBox
    Friend WithEvents lblShieldKinetic As System.Windows.Forms.Label
    Friend WithEvents progShieldKinetic As System.Windows.Forms.ProgressBar
    Friend WithEvents pbShieldKinetic As System.Windows.Forms.PictureBox
    Friend WithEvents pbShieldRecharge As System.Windows.Forms.PictureBox
    Friend WithEvents lblShieldRecharge As System.Windows.Forms.Label
    Friend WithEvents lblShieldHP As System.Windows.Forms.Label
    Friend WithEvents gbStructure As System.Windows.Forms.GroupBox
    Friend WithEvents lblStructureHP As System.Windows.Forms.Label
    Friend WithEvents pbStructureHP As System.Windows.Forms.PictureBox
    Friend WithEvents lblStructureThermal As System.Windows.Forms.Label
    Friend WithEvents progStructureThermal As System.Windows.Forms.ProgressBar
    Friend WithEvents pbStructureThermal As System.Windows.Forms.PictureBox
    Friend WithEvents lblStructureExplosive As System.Windows.Forms.Label
    Friend WithEvents progStructureExplosive As System.Windows.Forms.ProgressBar
    Friend WithEvents pbStructureExplosive As System.Windows.Forms.PictureBox
    Friend WithEvents lblStructureKinetic As System.Windows.Forms.Label
    Friend WithEvents progStructureKinetic As System.Windows.Forms.ProgressBar
    Friend WithEvents pbStructureKinetic As System.Windows.Forms.PictureBox
    Friend WithEvents lblStructureEM As System.Windows.Forms.Label
    Friend WithEvents progStructureEM As System.Windows.Forms.ProgressBar
    Friend WithEvents pbStructureEM As System.Windows.Forms.PictureBox
    Friend WithEvents gbArmor As System.Windows.Forms.GroupBox
    Friend WithEvents lblArmorHP As System.Windows.Forms.Label
    Friend WithEvents pbArmorHP As System.Windows.Forms.PictureBox
    Friend WithEvents lblArmorThermal As System.Windows.Forms.Label
    Friend WithEvents progArmorThermal As System.Windows.Forms.ProgressBar
    Friend WithEvents pbArmorThermal As System.Windows.Forms.PictureBox
    Friend WithEvents lblArmorExplosive As System.Windows.Forms.Label
    Friend WithEvents progArmorExplosive As System.Windows.Forms.ProgressBar
    Friend WithEvents pbArmorExplosive As System.Windows.Forms.PictureBox
    Friend WithEvents lblArmorKinetic As System.Windows.Forms.Label
    Friend WithEvents progArmorKinetic As System.Windows.Forms.ProgressBar
    Friend WithEvents pbArmorKinetic As System.Windows.Forms.PictureBox
    Friend WithEvents lblArmorEM As System.Windows.Forms.Label
    Friend WithEvents progArmorEM As System.Windows.Forms.ProgressBar
    Friend WithEvents pbArmorEM As System.Windows.Forms.PictureBox
    Friend WithEvents lblCalibration As System.Windows.Forms.Label
    Friend WithEvents progCalibration As System.Windows.Forms.ProgressBar
    Friend WithEvents pbCalibration As System.Windows.Forms.PictureBox
    Friend WithEvents gbTargeting As System.Windows.Forms.GroupBox
    Friend WithEvents lblSensorStrength As System.Windows.Forms.Label
    Friend WithEvents pbSensorStrength As System.Windows.Forms.PictureBox
    Friend WithEvents lblScanResolution As System.Windows.Forms.Label
    Friend WithEvents pbScanResolution As System.Windows.Forms.PictureBox
    Friend WithEvents lblMaxTargets As System.Windows.Forms.Label
    Friend WithEvents pbMaxTargets As System.Windows.Forms.PictureBox
    Friend WithEvents lblTargetRange As System.Windows.Forms.Label
    Friend WithEvents pbTargetRange As System.Windows.Forms.PictureBox
    Friend WithEvents gbCapacitor As System.Windows.Forms.GroupBox
    Friend WithEvents lblCapPeak As System.Windows.Forms.Label
    Friend WithEvents pbCapPeak As System.Windows.Forms.PictureBox
    Friend WithEvents lblCapRecharge As System.Windows.Forms.Label
    Friend WithEvents pbCapRecharge As System.Windows.Forms.PictureBox
    Friend WithEvents lblCapAverage As System.Windows.Forms.Label
    Friend WithEvents pbCapAverage As System.Windows.Forms.PictureBox
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
    Friend WithEvents progDroneBandwidth As System.Windows.Forms.ProgressBar
    Friend WithEvents btnDoomsdayCheck As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblEffectiveHP As System.Windows.Forms.Label
    Friend WithEvents line1 As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents lblSigRadius As System.Windows.Forms.Label
    Friend WithEvents pbSigRadius As System.Windows.Forms.PictureBox
    Friend WithEvents btnTargetSpeed As System.Windows.Forms.Button
    Friend WithEvents gbDamage As System.Windows.Forms.GroupBox
    Friend WithEvents lblMissileVolley As System.Windows.Forms.Label
    Friend WithEvents lblTurretVolley As System.Windows.Forms.Label
    Friend WithEvents btnSkills As System.Windows.Forms.Button
    Friend WithEvents btnLog As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
