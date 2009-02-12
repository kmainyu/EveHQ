<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNeuralRemap
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNeuralRemap))
        Me.lblDescription = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.PictureBox5 = New System.Windows.Forms.PictureBox
        Me.lblIntelligence = New System.Windows.Forms.Label
        Me.lblMemory = New System.Windows.Forms.Label
        Me.lblWillpower = New System.Windows.Forms.Label
        Me.lblCharisma = New System.Windows.Forms.Label
        Me.lblPerception = New System.Windows.Forms.Label
        Me.nudIBase = New System.Windows.Forms.NumericUpDown
        Me.lblIImplant = New System.Windows.Forms.Label
        Me.lblISkills = New System.Windows.Forms.Label
        Me.lblITotal = New System.Windows.Forms.Label
        Me.lblPTotal = New System.Windows.Forms.Label
        Me.lblPSkills = New System.Windows.Forms.Label
        Me.lblPImplant = New System.Windows.Forms.Label
        Me.nudPBase = New System.Windows.Forms.NumericUpDown
        Me.lblCTotal = New System.Windows.Forms.Label
        Me.lblCSkills = New System.Windows.Forms.Label
        Me.lblCImplant = New System.Windows.Forms.Label
        Me.nudCBase = New System.Windows.Forms.NumericUpDown
        Me.lblWTotal = New System.Windows.Forms.Label
        Me.lblWSkills = New System.Windows.Forms.Label
        Me.lblWImplant = New System.Windows.Forms.Label
        Me.nudWBase = New System.Windows.Forms.NumericUpDown
        Me.lblMTotal = New System.Windows.Forms.Label
        Me.lblMSkills = New System.Windows.Forms.Label
        Me.lblMImplant = New System.Windows.Forms.Label
        Me.nudMBase = New System.Windows.Forms.NumericUpDown
        Me.lblUnusedPointsLbl = New System.Windows.Forms.Label
        Me.lblUnusedPoints = New System.Windows.Forms.Label
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.lblSkillQueueAnalysis = New System.Windows.Forms.Label
        Me.lblActiveSkillQueueLbl = New System.Windows.Forms.Label
        Me.lblActiveSkillQueue = New System.Windows.Forms.Label
        Me.lblActiveQueueTime = New System.Windows.Forms.Label
        Me.lblRevisedQueueTime = New System.Windows.Forms.Label
        Me.lblAttribute1 = New System.Windows.Forms.Label
        Me.lblSkillQueuePointsAnalysis = New System.Windows.Forms.Label
        Me.lblAttribute2 = New System.Windows.Forms.Label
        Me.lblAttribute3 = New System.Windows.Forms.Label
        Me.lblAttribute4 = New System.Windows.Forms.Label
        Me.lblAttribute5 = New System.Windows.Forms.Label
        Me.lblAttributePoints5 = New System.Windows.Forms.Label
        Me.lblAttributePoints4 = New System.Windows.Forms.Label
        Me.lblAttributePoints3 = New System.Windows.Forms.Label
        Me.lblAttributePoints2 = New System.Windows.Forms.Label
        Me.lblAttributePoints1 = New System.Windows.Forms.Label
        Me.lblTimeSaving = New System.Windows.Forms.Label
        Me.btnOptimise = New System.Windows.Forms.Button
        Me.btnReset = New System.Windows.Forms.Button
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIBase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPBase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCBase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudWBase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMBase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(15, 9)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(517, 34)
        Me.lblDescription.TabIndex = 0
        Me.lblDescription.Text = "Neural Remapping allows you to respecify your starting base attributes which can " & _
            "be used to optimise skill training in a particular area. Attributes must be betw" & _
            "een 5 and 15." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(15, 59)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(15, 339)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox2.TabIndex = 3
        Me.PictureBox2.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(15, 269)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox3.TabIndex = 4
        Me.PictureBox3.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(15, 199)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox4.TabIndex = 5
        Me.PictureBox4.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(15, 129)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox5.TabIndex = 6
        Me.PictureBox5.TabStop = False
        '
        'lblIntelligence
        '
        Me.lblIntelligence.AutoSize = True
        Me.lblIntelligence.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIntelligence.Location = New System.Drawing.Point(85, 59)
        Me.lblIntelligence.Name = "lblIntelligence"
        Me.lblIntelligence.Size = New System.Drawing.Size(78, 14)
        Me.lblIntelligence.TabIndex = 7
        Me.lblIntelligence.Text = "Intelligence"
        '
        'lblMemory
        '
        Me.lblMemory.AutoSize = True
        Me.lblMemory.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMemory.Location = New System.Drawing.Point(85, 339)
        Me.lblMemory.Name = "lblMemory"
        Me.lblMemory.Size = New System.Drawing.Size(56, 14)
        Me.lblMemory.TabIndex = 8
        Me.lblMemory.Text = "Memory"
        '
        'lblWillpower
        '
        Me.lblWillpower.AutoSize = True
        Me.lblWillpower.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWillpower.Location = New System.Drawing.Point(85, 269)
        Me.lblWillpower.Name = "lblWillpower"
        Me.lblWillpower.Size = New System.Drawing.Size(67, 14)
        Me.lblWillpower.TabIndex = 9
        Me.lblWillpower.Text = "Willpower"
        '
        'lblCharisma
        '
        Me.lblCharisma.AutoSize = True
        Me.lblCharisma.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCharisma.Location = New System.Drawing.Point(85, 199)
        Me.lblCharisma.Name = "lblCharisma"
        Me.lblCharisma.Size = New System.Drawing.Size(62, 14)
        Me.lblCharisma.TabIndex = 10
        Me.lblCharisma.Text = "Charisma"
        '
        'lblPerception
        '
        Me.lblPerception.AutoSize = True
        Me.lblPerception.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPerception.Location = New System.Drawing.Point(85, 129)
        Me.lblPerception.Name = "lblPerception"
        Me.lblPerception.Size = New System.Drawing.Size(73, 14)
        Me.lblPerception.TabIndex = 11
        Me.lblPerception.Text = "Perception"
        '
        'nudIBase
        '
        Me.nudIBase.Location = New System.Drawing.Point(88, 82)
        Me.nudIBase.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.nudIBase.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudIBase.Name = "nudIBase"
        Me.nudIBase.Size = New System.Drawing.Size(72, 21)
        Me.nudIBase.TabIndex = 13
        Me.nudIBase.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblIImplant
        '
        Me.lblIImplant.AutoSize = True
        Me.lblIImplant.Location = New System.Drawing.Point(166, 84)
        Me.lblIImplant.Name = "lblIImplant"
        Me.lblIImplant.Size = New System.Drawing.Size(47, 13)
        Me.lblIImplant.TabIndex = 14
        Me.lblIImplant.Text = "Implant:"
        '
        'lblISkills
        '
        Me.lblISkills.AutoSize = True
        Me.lblISkills.Location = New System.Drawing.Point(166, 97)
        Me.lblISkills.Name = "lblISkills"
        Me.lblISkills.Size = New System.Drawing.Size(33, 13)
        Me.lblISkills.TabIndex = 15
        Me.lblISkills.Text = "Skills:"
        '
        'lblITotal
        '
        Me.lblITotal.AutoSize = True
        Me.lblITotal.Location = New System.Drawing.Point(166, 110)
        Me.lblITotal.Name = "lblITotal"
        Me.lblITotal.Size = New System.Drawing.Size(35, 13)
        Me.lblITotal.TabIndex = 16
        Me.lblITotal.Text = "Total:"
        '
        'lblPTotal
        '
        Me.lblPTotal.AutoSize = True
        Me.lblPTotal.Location = New System.Drawing.Point(166, 180)
        Me.lblPTotal.Name = "lblPTotal"
        Me.lblPTotal.Size = New System.Drawing.Size(35, 13)
        Me.lblPTotal.TabIndex = 21
        Me.lblPTotal.Text = "Total:"
        '
        'lblPSkills
        '
        Me.lblPSkills.AutoSize = True
        Me.lblPSkills.Location = New System.Drawing.Point(166, 167)
        Me.lblPSkills.Name = "lblPSkills"
        Me.lblPSkills.Size = New System.Drawing.Size(33, 13)
        Me.lblPSkills.TabIndex = 20
        Me.lblPSkills.Text = "Skills:"
        '
        'lblPImplant
        '
        Me.lblPImplant.AutoSize = True
        Me.lblPImplant.Location = New System.Drawing.Point(166, 154)
        Me.lblPImplant.Name = "lblPImplant"
        Me.lblPImplant.Size = New System.Drawing.Size(47, 13)
        Me.lblPImplant.TabIndex = 19
        Me.lblPImplant.Text = "Implant:"
        '
        'nudPBase
        '
        Me.nudPBase.Location = New System.Drawing.Point(88, 152)
        Me.nudPBase.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.nudPBase.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudPBase.Name = "nudPBase"
        Me.nudPBase.Size = New System.Drawing.Size(72, 21)
        Me.nudPBase.TabIndex = 18
        Me.nudPBase.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblCTotal
        '
        Me.lblCTotal.AutoSize = True
        Me.lblCTotal.Location = New System.Drawing.Point(166, 250)
        Me.lblCTotal.Name = "lblCTotal"
        Me.lblCTotal.Size = New System.Drawing.Size(35, 13)
        Me.lblCTotal.TabIndex = 26
        Me.lblCTotal.Text = "Total:"
        '
        'lblCSkills
        '
        Me.lblCSkills.AutoSize = True
        Me.lblCSkills.Location = New System.Drawing.Point(166, 237)
        Me.lblCSkills.Name = "lblCSkills"
        Me.lblCSkills.Size = New System.Drawing.Size(33, 13)
        Me.lblCSkills.TabIndex = 25
        Me.lblCSkills.Text = "Skills:"
        '
        'lblCImplant
        '
        Me.lblCImplant.AutoSize = True
        Me.lblCImplant.Location = New System.Drawing.Point(166, 224)
        Me.lblCImplant.Name = "lblCImplant"
        Me.lblCImplant.Size = New System.Drawing.Size(47, 13)
        Me.lblCImplant.TabIndex = 24
        Me.lblCImplant.Text = "Implant:"
        '
        'nudCBase
        '
        Me.nudCBase.Location = New System.Drawing.Point(88, 222)
        Me.nudCBase.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.nudCBase.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudCBase.Name = "nudCBase"
        Me.nudCBase.Size = New System.Drawing.Size(72, 21)
        Me.nudCBase.TabIndex = 23
        Me.nudCBase.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblWTotal
        '
        Me.lblWTotal.AutoSize = True
        Me.lblWTotal.Location = New System.Drawing.Point(166, 320)
        Me.lblWTotal.Name = "lblWTotal"
        Me.lblWTotal.Size = New System.Drawing.Size(35, 13)
        Me.lblWTotal.TabIndex = 31
        Me.lblWTotal.Text = "Total:"
        '
        'lblWSkills
        '
        Me.lblWSkills.AutoSize = True
        Me.lblWSkills.Location = New System.Drawing.Point(166, 307)
        Me.lblWSkills.Name = "lblWSkills"
        Me.lblWSkills.Size = New System.Drawing.Size(33, 13)
        Me.lblWSkills.TabIndex = 30
        Me.lblWSkills.Text = "Skills:"
        '
        'lblWImplant
        '
        Me.lblWImplant.AutoSize = True
        Me.lblWImplant.Location = New System.Drawing.Point(166, 294)
        Me.lblWImplant.Name = "lblWImplant"
        Me.lblWImplant.Size = New System.Drawing.Size(47, 13)
        Me.lblWImplant.TabIndex = 29
        Me.lblWImplant.Text = "Implant:"
        '
        'nudWBase
        '
        Me.nudWBase.Location = New System.Drawing.Point(88, 292)
        Me.nudWBase.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.nudWBase.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudWBase.Name = "nudWBase"
        Me.nudWBase.Size = New System.Drawing.Size(72, 21)
        Me.nudWBase.TabIndex = 28
        Me.nudWBase.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblMTotal
        '
        Me.lblMTotal.AutoSize = True
        Me.lblMTotal.Location = New System.Drawing.Point(166, 390)
        Me.lblMTotal.Name = "lblMTotal"
        Me.lblMTotal.Size = New System.Drawing.Size(35, 13)
        Me.lblMTotal.TabIndex = 36
        Me.lblMTotal.Text = "Total:"
        '
        'lblMSkills
        '
        Me.lblMSkills.AutoSize = True
        Me.lblMSkills.Location = New System.Drawing.Point(166, 377)
        Me.lblMSkills.Name = "lblMSkills"
        Me.lblMSkills.Size = New System.Drawing.Size(33, 13)
        Me.lblMSkills.TabIndex = 35
        Me.lblMSkills.Text = "Skills:"
        '
        'lblMImplant
        '
        Me.lblMImplant.AutoSize = True
        Me.lblMImplant.Location = New System.Drawing.Point(166, 364)
        Me.lblMImplant.Name = "lblMImplant"
        Me.lblMImplant.Size = New System.Drawing.Size(47, 13)
        Me.lblMImplant.TabIndex = 34
        Me.lblMImplant.Text = "Implant:"
        '
        'nudMBase
        '
        Me.nudMBase.Location = New System.Drawing.Point(88, 362)
        Me.nudMBase.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.nudMBase.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudMBase.Name = "nudMBase"
        Me.nudMBase.Size = New System.Drawing.Size(72, 21)
        Me.nudMBase.TabIndex = 33
        Me.nudMBase.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblUnusedPointsLbl
        '
        Me.lblUnusedPointsLbl.AutoSize = True
        Me.lblUnusedPointsLbl.Location = New System.Drawing.Point(117, 425)
        Me.lblUnusedPointsLbl.Name = "lblUnusedPointsLbl"
        Me.lblUnusedPointsLbl.Size = New System.Drawing.Size(125, 13)
        Me.lblUnusedPointsLbl.TabIndex = 37
        Me.lblUnusedPointsLbl.Text = "Unused Attribute Points:"
        '
        'lblUnusedPoints
        '
        Me.lblUnusedPoints.AutoSize = True
        Me.lblUnusedPoints.Location = New System.Drawing.Point(248, 425)
        Me.lblUnusedPoints.Name = "lblUnusedPoints"
        Me.lblUnusedPoints.Size = New System.Drawing.Size(13, 13)
        Me.lblUnusedPoints.TabIndex = 38
        Me.lblUnusedPoints.Text = "0"
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(538, 3)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(40, 40)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox6.TabIndex = 39
        Me.PictureBox6.TabStop = False
        '
        'lblSkillQueueAnalysis
        '
        Me.lblSkillQueueAnalysis.AutoSize = True
        Me.lblSkillQueueAnalysis.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSkillQueueAnalysis.Location = New System.Drawing.Point(310, 59)
        Me.lblSkillQueueAnalysis.Name = "lblSkillQueueAnalysis"
        Me.lblSkillQueueAnalysis.Size = New System.Drawing.Size(127, 14)
        Me.lblSkillQueueAnalysis.TabIndex = 41
        Me.lblSkillQueueAnalysis.Text = "Skill Queue Analysis"
        '
        'lblActiveSkillQueueLbl
        '
        Me.lblActiveSkillQueueLbl.AutoSize = True
        Me.lblActiveSkillQueueLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActiveSkillQueueLbl.Location = New System.Drawing.Point(321, 84)
        Me.lblActiveSkillQueueLbl.Name = "lblActiveSkillQueueLbl"
        Me.lblActiveSkillQueueLbl.Size = New System.Drawing.Size(118, 13)
        Me.lblActiveSkillQueueLbl.TabIndex = 42
        Me.lblActiveSkillQueueLbl.Text = "Current Skill Queue:"
        '
        'lblActiveSkillQueue
        '
        Me.lblActiveSkillQueue.AutoSize = True
        Me.lblActiveSkillQueue.Location = New System.Drawing.Point(321, 110)
        Me.lblActiveSkillQueue.Name = "lblActiveSkillQueue"
        Me.lblActiveSkillQueue.Size = New System.Drawing.Size(72, 13)
        Me.lblActiveSkillQueue.TabIndex = 43
        Me.lblActiveSkillQueue.Text = "Active Queue"
        '
        'lblActiveQueueTime
        '
        Me.lblActiveQueueTime.AutoSize = True
        Me.lblActiveQueueTime.Location = New System.Drawing.Point(321, 270)
        Me.lblActiveQueueTime.Name = "lblActiveQueueTime"
        Me.lblActiveQueueTime.Size = New System.Drawing.Size(85, 13)
        Me.lblActiveQueueTime.TabIndex = 44
        Me.lblActiveQueueTime.Text = "Time Remaining:"
        '
        'lblRevisedQueueTime
        '
        Me.lblRevisedQueueTime.AutoSize = True
        Me.lblRevisedQueueTime.Location = New System.Drawing.Point(321, 290)
        Me.lblRevisedQueueTime.Name = "lblRevisedQueueTime"
        Me.lblRevisedQueueTime.Size = New System.Drawing.Size(74, 13)
        Me.lblRevisedQueueTime.TabIndex = 45
        Me.lblRevisedQueueTime.Text = "Revised Time:"
        '
        'lblAttribute1
        '
        Me.lblAttribute1.AutoSize = True
        Me.lblAttribute1.Location = New System.Drawing.Point(324, 179)
        Me.lblAttribute1.Name = "lblAttribute1"
        Me.lblAttribute1.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute1.TabIndex = 46
        Me.lblAttribute1.Text = "Attribute1:"
        '
        'lblSkillQueuePointsAnalysis
        '
        Me.lblSkillQueuePointsAnalysis.AutoSize = True
        Me.lblSkillQueuePointsAnalysis.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSkillQueuePointsAnalysis.Location = New System.Drawing.Point(321, 152)
        Me.lblSkillQueuePointsAnalysis.Name = "lblSkillQueuePointsAnalysis"
        Me.lblSkillQueuePointsAnalysis.Size = New System.Drawing.Size(160, 13)
        Me.lblSkillQueuePointsAnalysis.TabIndex = 47
        Me.lblSkillQueuePointsAnalysis.Text = "Skill Queue Points Analysis:"
        '
        'lblAttribute2
        '
        Me.lblAttribute2.AutoSize = True
        Me.lblAttribute2.Location = New System.Drawing.Point(324, 192)
        Me.lblAttribute2.Name = "lblAttribute2"
        Me.lblAttribute2.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute2.TabIndex = 48
        Me.lblAttribute2.Text = "Attribute2:"
        '
        'lblAttribute3
        '
        Me.lblAttribute3.AutoSize = True
        Me.lblAttribute3.Location = New System.Drawing.Point(324, 205)
        Me.lblAttribute3.Name = "lblAttribute3"
        Me.lblAttribute3.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute3.TabIndex = 49
        Me.lblAttribute3.Text = "Attribute3:"
        '
        'lblAttribute4
        '
        Me.lblAttribute4.AutoSize = True
        Me.lblAttribute4.Location = New System.Drawing.Point(324, 218)
        Me.lblAttribute4.Name = "lblAttribute4"
        Me.lblAttribute4.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute4.TabIndex = 50
        Me.lblAttribute4.Text = "Attribute4:"
        '
        'lblAttribute5
        '
        Me.lblAttribute5.AutoSize = True
        Me.lblAttribute5.Location = New System.Drawing.Point(324, 231)
        Me.lblAttribute5.Name = "lblAttribute5"
        Me.lblAttribute5.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute5.TabIndex = 51
        Me.lblAttribute5.Text = "Attribute5:"
        '
        'lblAttributePoints5
        '
        Me.lblAttributePoints5.AutoSize = True
        Me.lblAttributePoints5.Location = New System.Drawing.Point(399, 231)
        Me.lblAttributePoints5.Name = "lblAttributePoints5"
        Me.lblAttributePoints5.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints5.TabIndex = 56
        Me.lblAttributePoints5.Text = "Attribute5:"
        '
        'lblAttributePoints4
        '
        Me.lblAttributePoints4.AutoSize = True
        Me.lblAttributePoints4.Location = New System.Drawing.Point(399, 218)
        Me.lblAttributePoints4.Name = "lblAttributePoints4"
        Me.lblAttributePoints4.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints4.TabIndex = 55
        Me.lblAttributePoints4.Text = "Attribute4:"
        '
        'lblAttributePoints3
        '
        Me.lblAttributePoints3.AutoSize = True
        Me.lblAttributePoints3.Location = New System.Drawing.Point(399, 205)
        Me.lblAttributePoints3.Name = "lblAttributePoints3"
        Me.lblAttributePoints3.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints3.TabIndex = 54
        Me.lblAttributePoints3.Text = "Attribute3:"
        '
        'lblAttributePoints2
        '
        Me.lblAttributePoints2.AutoSize = True
        Me.lblAttributePoints2.Location = New System.Drawing.Point(399, 192)
        Me.lblAttributePoints2.Name = "lblAttributePoints2"
        Me.lblAttributePoints2.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints2.TabIndex = 53
        Me.lblAttributePoints2.Text = "Attribute2:"
        '
        'lblAttributePoints1
        '
        Me.lblAttributePoints1.AutoSize = True
        Me.lblAttributePoints1.Location = New System.Drawing.Point(399, 179)
        Me.lblAttributePoints1.Name = "lblAttributePoints1"
        Me.lblAttributePoints1.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints1.TabIndex = 52
        Me.lblAttributePoints1.Text = "Attribute1:"
        '
        'lblTimeSaving
        '
        Me.lblTimeSaving.AutoSize = True
        Me.lblTimeSaving.Location = New System.Drawing.Point(321, 310)
        Me.lblTimeSaving.Name = "lblTimeSaving"
        Me.lblTimeSaving.Size = New System.Drawing.Size(68, 13)
        Me.lblTimeSaving.TabIndex = 57
        Me.lblTimeSaving.Text = "Time Saving:"
        '
        'btnOptimise
        '
        Me.btnOptimise.Location = New System.Drawing.Point(318, 420)
        Me.btnOptimise.Name = "btnOptimise"
        Me.btnOptimise.Size = New System.Drawing.Size(75, 23)
        Me.btnOptimise.TabIndex = 58
        Me.btnOptimise.Text = "Optimise"
        Me.btnOptimise.UseVisualStyleBackColor = True
        '
        'btnReset
        '
        Me.btnReset.Location = New System.Drawing.Point(15, 420)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(75, 23)
        Me.btnReset.TabIndex = 59
        Me.btnReset.Text = "Reset"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'frmNeuralRemap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(590, 470)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.btnOptimise)
        Me.Controls.Add(Me.lblTimeSaving)
        Me.Controls.Add(Me.lblAttributePoints5)
        Me.Controls.Add(Me.lblAttributePoints4)
        Me.Controls.Add(Me.lblAttributePoints3)
        Me.Controls.Add(Me.lblAttributePoints2)
        Me.Controls.Add(Me.lblAttributePoints1)
        Me.Controls.Add(Me.lblAttribute5)
        Me.Controls.Add(Me.lblAttribute4)
        Me.Controls.Add(Me.lblAttribute3)
        Me.Controls.Add(Me.lblAttribute2)
        Me.Controls.Add(Me.lblSkillQueuePointsAnalysis)
        Me.Controls.Add(Me.lblAttribute1)
        Me.Controls.Add(Me.lblRevisedQueueTime)
        Me.Controls.Add(Me.lblActiveQueueTime)
        Me.Controls.Add(Me.lblActiveSkillQueue)
        Me.Controls.Add(Me.lblActiveSkillQueueLbl)
        Me.Controls.Add(Me.lblSkillQueueAnalysis)
        Me.Controls.Add(Me.PictureBox6)
        Me.Controls.Add(Me.lblUnusedPoints)
        Me.Controls.Add(Me.lblUnusedPointsLbl)
        Me.Controls.Add(Me.lblMTotal)
        Me.Controls.Add(Me.lblMSkills)
        Me.Controls.Add(Me.lblMImplant)
        Me.Controls.Add(Me.nudMBase)
        Me.Controls.Add(Me.lblWTotal)
        Me.Controls.Add(Me.lblWSkills)
        Me.Controls.Add(Me.lblWImplant)
        Me.Controls.Add(Me.nudWBase)
        Me.Controls.Add(Me.lblCTotal)
        Me.Controls.Add(Me.lblCSkills)
        Me.Controls.Add(Me.lblCImplant)
        Me.Controls.Add(Me.nudCBase)
        Me.Controls.Add(Me.lblPTotal)
        Me.Controls.Add(Me.lblPSkills)
        Me.Controls.Add(Me.lblPImplant)
        Me.Controls.Add(Me.nudPBase)
        Me.Controls.Add(Me.lblITotal)
        Me.Controls.Add(Me.lblISkills)
        Me.Controls.Add(Me.lblIImplant)
        Me.Controls.Add(Me.nudIBase)
        Me.Controls.Add(Me.lblPerception)
        Me.Controls.Add(Me.lblCharisma)
        Me.Controls.Add(Me.lblWillpower)
        Me.Controls.Add(Me.lblMemory)
        Me.Controls.Add(Me.lblIntelligence)
        Me.Controls.Add(Me.PictureBox5)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblDescription)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmNeuralRemap"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Neural Remapping"
        Me.TopMost = True
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIBase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPBase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCBase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudWBase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMBase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents lblIntelligence As System.Windows.Forms.Label
    Friend WithEvents lblMemory As System.Windows.Forms.Label
    Friend WithEvents lblWillpower As System.Windows.Forms.Label
    Friend WithEvents lblCharisma As System.Windows.Forms.Label
    Friend WithEvents lblPerception As System.Windows.Forms.Label
    Friend WithEvents nudIBase As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblIImplant As System.Windows.Forms.Label
    Friend WithEvents lblISkills As System.Windows.Forms.Label
    Friend WithEvents lblITotal As System.Windows.Forms.Label
    Friend WithEvents lblPTotal As System.Windows.Forms.Label
    Friend WithEvents lblPSkills As System.Windows.Forms.Label
    Friend WithEvents lblPImplant As System.Windows.Forms.Label
    Friend WithEvents nudPBase As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblCTotal As System.Windows.Forms.Label
    Friend WithEvents lblCSkills As System.Windows.Forms.Label
    Friend WithEvents lblCImplant As System.Windows.Forms.Label
    Friend WithEvents nudCBase As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblWTotal As System.Windows.Forms.Label
    Friend WithEvents lblWSkills As System.Windows.Forms.Label
    Friend WithEvents lblWImplant As System.Windows.Forms.Label
    Friend WithEvents nudWBase As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblMTotal As System.Windows.Forms.Label
    Friend WithEvents lblMSkills As System.Windows.Forms.Label
    Friend WithEvents lblMImplant As System.Windows.Forms.Label
    Friend WithEvents nudMBase As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblUnusedPointsLbl As System.Windows.Forms.Label
    Friend WithEvents lblUnusedPoints As System.Windows.Forms.Label
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents lblSkillQueueAnalysis As System.Windows.Forms.Label
    Friend WithEvents lblActiveSkillQueueLbl As System.Windows.Forms.Label
    Friend WithEvents lblActiveSkillQueue As System.Windows.Forms.Label
    Friend WithEvents lblActiveQueueTime As System.Windows.Forms.Label
    Friend WithEvents lblRevisedQueueTime As System.Windows.Forms.Label
    Friend WithEvents lblAttribute1 As System.Windows.Forms.Label
    Friend WithEvents lblSkillQueuePointsAnalysis As System.Windows.Forms.Label
    Friend WithEvents lblAttribute2 As System.Windows.Forms.Label
    Friend WithEvents lblAttribute3 As System.Windows.Forms.Label
    Friend WithEvents lblAttribute4 As System.Windows.Forms.Label
    Friend WithEvents lblAttribute5 As System.Windows.Forms.Label
    Friend WithEvents lblAttributePoints5 As System.Windows.Forms.Label
    Friend WithEvents lblAttributePoints4 As System.Windows.Forms.Label
    Friend WithEvents lblAttributePoints3 As System.Windows.Forms.Label
    Friend WithEvents lblAttributePoints2 As System.Windows.Forms.Label
    Friend WithEvents lblAttributePoints1 As System.Windows.Forms.Label
    Friend WithEvents lblTimeSaving As System.Windows.Forms.Label
    Friend WithEvents btnOptimise As System.Windows.Forms.Button
    Friend WithEvents btnReset As System.Windows.Forms.Button
End Class
