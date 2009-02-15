<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImplants
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImplants))
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblIBase = New System.Windows.Forms.Label
        Me.lblISkills = New System.Windows.Forms.Label
        Me.lblITotal = New System.Windows.Forms.Label
        Me.lblPTotal = New System.Windows.Forms.Label
        Me.lblPSkills = New System.Windows.Forms.Label
        Me.lblPBase = New System.Windows.Forms.Label
        Me.lblCTotal = New System.Windows.Forms.Label
        Me.lblCSkills = New System.Windows.Forms.Label
        Me.lblCBase = New System.Windows.Forms.Label
        Me.lblWTotal = New System.Windows.Forms.Label
        Me.lblWSkills = New System.Windows.Forms.Label
        Me.lblWBase = New System.Windows.Forms.Label
        Me.lblMTotal = New System.Windows.Forms.Label
        Me.lblMSkills = New System.Windows.Forms.Label
        Me.lblMBase = New System.Windows.Forms.Label
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
        Me.btnResetImplants = New System.Windows.Forms.Button
        Me.nudMImplant = New System.Windows.Forms.NumericUpDown
        Me.nudWImplant = New System.Windows.Forms.NumericUpDown
        Me.nudCImplant = New System.Windows.Forms.NumericUpDown
        Me.nudPImplant = New System.Windows.Forms.NumericUpDown
        Me.nudIImplant = New System.Windows.Forms.NumericUpDown
        Me.lblPerceptionImplant = New System.Windows.Forms.Label
        Me.lblCharismaImplant = New System.Windows.Forms.Label
        Me.lblWillpowerImplant = New System.Windows.Forms.Label
        Me.lblMemoryImplant = New System.Windows.Forms.Label
        Me.lblIntelligenceImplant = New System.Windows.Forms.Label
        Me.PictureBox7 = New System.Windows.Forms.PictureBox
        Me.PictureBox8 = New System.Windows.Forms.PictureBox
        Me.PictureBox9 = New System.Windows.Forms.PictureBox
        Me.PictureBox10 = New System.Windows.Forms.PictureBox
        Me.PictureBox11 = New System.Windows.Forms.PictureBox
        Me.gbImplants = New System.Windows.Forms.GroupBox
        Me.gbSkillQueue = New System.Windows.Forms.GroupBox
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMImplant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudWImplant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCImplant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPImplant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIImplant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbImplants.SuspendLayout()
        Me.gbSkillQueue.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblDescription
        '
        Me.lblDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDescription.Location = New System.Drawing.Point(15, 9)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(450, 34)
        Me.lblDescription.TabIndex = 0
        Me.lblDescription.Text = "This form allows you to see the effect of changing your implants on a particular " & _
            "skill queue. It can also be used to see the effect of increasing skills." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lblIBase
        '
        Me.lblIBase.AutoSize = True
        Me.lblIBase.Location = New System.Drawing.Point(170, 56)
        Me.lblIBase.Name = "lblIBase"
        Me.lblIBase.Size = New System.Drawing.Size(34, 13)
        Me.lblIBase.TabIndex = 14
        Me.lblIBase.Text = "Base:"
        '
        'lblISkills
        '
        Me.lblISkills.AutoSize = True
        Me.lblISkills.Location = New System.Drawing.Point(170, 69)
        Me.lblISkills.Name = "lblISkills"
        Me.lblISkills.Size = New System.Drawing.Size(33, 13)
        Me.lblISkills.TabIndex = 15
        Me.lblISkills.Text = "Skills:"
        '
        'lblITotal
        '
        Me.lblITotal.AutoSize = True
        Me.lblITotal.Location = New System.Drawing.Point(170, 82)
        Me.lblITotal.Name = "lblITotal"
        Me.lblITotal.Size = New System.Drawing.Size(35, 13)
        Me.lblITotal.TabIndex = 16
        Me.lblITotal.Text = "Total:"
        '
        'lblPTotal
        '
        Me.lblPTotal.AutoSize = True
        Me.lblPTotal.Location = New System.Drawing.Point(170, 152)
        Me.lblPTotal.Name = "lblPTotal"
        Me.lblPTotal.Size = New System.Drawing.Size(35, 13)
        Me.lblPTotal.TabIndex = 21
        Me.lblPTotal.Text = "Total:"
        '
        'lblPSkills
        '
        Me.lblPSkills.AutoSize = True
        Me.lblPSkills.Location = New System.Drawing.Point(170, 139)
        Me.lblPSkills.Name = "lblPSkills"
        Me.lblPSkills.Size = New System.Drawing.Size(33, 13)
        Me.lblPSkills.TabIndex = 20
        Me.lblPSkills.Text = "Skills:"
        '
        'lblPBase
        '
        Me.lblPBase.AutoSize = True
        Me.lblPBase.Location = New System.Drawing.Point(170, 126)
        Me.lblPBase.Name = "lblPBase"
        Me.lblPBase.Size = New System.Drawing.Size(34, 13)
        Me.lblPBase.TabIndex = 19
        Me.lblPBase.Text = "Base:"
        '
        'lblCTotal
        '
        Me.lblCTotal.AutoSize = True
        Me.lblCTotal.Location = New System.Drawing.Point(170, 222)
        Me.lblCTotal.Name = "lblCTotal"
        Me.lblCTotal.Size = New System.Drawing.Size(35, 13)
        Me.lblCTotal.TabIndex = 26
        Me.lblCTotal.Text = "Total:"
        '
        'lblCSkills
        '
        Me.lblCSkills.AutoSize = True
        Me.lblCSkills.Location = New System.Drawing.Point(170, 209)
        Me.lblCSkills.Name = "lblCSkills"
        Me.lblCSkills.Size = New System.Drawing.Size(33, 13)
        Me.lblCSkills.TabIndex = 25
        Me.lblCSkills.Text = "Skills:"
        '
        'lblCBase
        '
        Me.lblCBase.AutoSize = True
        Me.lblCBase.Location = New System.Drawing.Point(170, 196)
        Me.lblCBase.Name = "lblCBase"
        Me.lblCBase.Size = New System.Drawing.Size(34, 13)
        Me.lblCBase.TabIndex = 24
        Me.lblCBase.Text = "Base:"
        '
        'lblWTotal
        '
        Me.lblWTotal.AutoSize = True
        Me.lblWTotal.Location = New System.Drawing.Point(170, 292)
        Me.lblWTotal.Name = "lblWTotal"
        Me.lblWTotal.Size = New System.Drawing.Size(35, 13)
        Me.lblWTotal.TabIndex = 31
        Me.lblWTotal.Text = "Total:"
        '
        'lblWSkills
        '
        Me.lblWSkills.AutoSize = True
        Me.lblWSkills.Location = New System.Drawing.Point(170, 279)
        Me.lblWSkills.Name = "lblWSkills"
        Me.lblWSkills.Size = New System.Drawing.Size(33, 13)
        Me.lblWSkills.TabIndex = 30
        Me.lblWSkills.Text = "Skills:"
        '
        'lblWBase
        '
        Me.lblWBase.AutoSize = True
        Me.lblWBase.Location = New System.Drawing.Point(170, 266)
        Me.lblWBase.Name = "lblWBase"
        Me.lblWBase.Size = New System.Drawing.Size(34, 13)
        Me.lblWBase.TabIndex = 29
        Me.lblWBase.Text = "Base:"
        '
        'lblMTotal
        '
        Me.lblMTotal.AutoSize = True
        Me.lblMTotal.Location = New System.Drawing.Point(170, 362)
        Me.lblMTotal.Name = "lblMTotal"
        Me.lblMTotal.Size = New System.Drawing.Size(35, 13)
        Me.lblMTotal.TabIndex = 36
        Me.lblMTotal.Text = "Total:"
        '
        'lblMSkills
        '
        Me.lblMSkills.AutoSize = True
        Me.lblMSkills.Location = New System.Drawing.Point(170, 349)
        Me.lblMSkills.Name = "lblMSkills"
        Me.lblMSkills.Size = New System.Drawing.Size(33, 13)
        Me.lblMSkills.TabIndex = 35
        Me.lblMSkills.Text = "Skills:"
        '
        'lblMBase
        '
        Me.lblMBase.AutoSize = True
        Me.lblMBase.Location = New System.Drawing.Point(170, 336)
        Me.lblMBase.Name = "lblMBase"
        Me.lblMBase.Size = New System.Drawing.Size(34, 13)
        Me.lblMBase.TabIndex = 34
        Me.lblMBase.Text = "Base:"
        '
        'PictureBox6
        '
        Me.PictureBox6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(471, 3)
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
        Me.lblSkillQueueAnalysis.Location = New System.Drawing.Point(16, 31)
        Me.lblSkillQueueAnalysis.Name = "lblSkillQueueAnalysis"
        Me.lblSkillQueueAnalysis.Size = New System.Drawing.Size(127, 14)
        Me.lblSkillQueueAnalysis.TabIndex = 41
        Me.lblSkillQueueAnalysis.Text = "Skill Queue Analysis"
        '
        'lblActiveSkillQueueLbl
        '
        Me.lblActiveSkillQueueLbl.AutoSize = True
        Me.lblActiveSkillQueueLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActiveSkillQueueLbl.Location = New System.Drawing.Point(27, 56)
        Me.lblActiveSkillQueueLbl.Name = "lblActiveSkillQueueLbl"
        Me.lblActiveSkillQueueLbl.Size = New System.Drawing.Size(118, 13)
        Me.lblActiveSkillQueueLbl.TabIndex = 42
        Me.lblActiveSkillQueueLbl.Text = "Current Skill Queue:"
        '
        'lblActiveSkillQueue
        '
        Me.lblActiveSkillQueue.AutoSize = True
        Me.lblActiveSkillQueue.Location = New System.Drawing.Point(27, 82)
        Me.lblActiveSkillQueue.Name = "lblActiveSkillQueue"
        Me.lblActiveSkillQueue.Size = New System.Drawing.Size(72, 13)
        Me.lblActiveSkillQueue.TabIndex = 43
        Me.lblActiveSkillQueue.Text = "Active Queue"
        '
        'lblActiveQueueTime
        '
        Me.lblActiveQueueTime.AutoSize = True
        Me.lblActiveQueueTime.Location = New System.Drawing.Point(16, 241)
        Me.lblActiveQueueTime.Name = "lblActiveQueueTime"
        Me.lblActiveQueueTime.Size = New System.Drawing.Size(85, 13)
        Me.lblActiveQueueTime.TabIndex = 44
        Me.lblActiveQueueTime.Text = "Time Remaining:"
        '
        'lblRevisedQueueTime
        '
        Me.lblRevisedQueueTime.AutoSize = True
        Me.lblRevisedQueueTime.Location = New System.Drawing.Point(16, 261)
        Me.lblRevisedQueueTime.Name = "lblRevisedQueueTime"
        Me.lblRevisedQueueTime.Size = New System.Drawing.Size(74, 13)
        Me.lblRevisedQueueTime.TabIndex = 45
        Me.lblRevisedQueueTime.Text = "Revised Time:"
        '
        'lblAttribute1
        '
        Me.lblAttribute1.AutoSize = True
        Me.lblAttribute1.Location = New System.Drawing.Point(30, 151)
        Me.lblAttribute1.Name = "lblAttribute1"
        Me.lblAttribute1.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute1.TabIndex = 46
        Me.lblAttribute1.Text = "Attribute1:"
        '
        'lblSkillQueuePointsAnalysis
        '
        Me.lblSkillQueuePointsAnalysis.AutoSize = True
        Me.lblSkillQueuePointsAnalysis.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSkillQueuePointsAnalysis.Location = New System.Drawing.Point(27, 124)
        Me.lblSkillQueuePointsAnalysis.Name = "lblSkillQueuePointsAnalysis"
        Me.lblSkillQueuePointsAnalysis.Size = New System.Drawing.Size(160, 13)
        Me.lblSkillQueuePointsAnalysis.TabIndex = 47
        Me.lblSkillQueuePointsAnalysis.Text = "Skill Queue Points Analysis:"
        '
        'lblAttribute2
        '
        Me.lblAttribute2.AutoSize = True
        Me.lblAttribute2.Location = New System.Drawing.Point(30, 164)
        Me.lblAttribute2.Name = "lblAttribute2"
        Me.lblAttribute2.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute2.TabIndex = 48
        Me.lblAttribute2.Text = "Attribute2:"
        '
        'lblAttribute3
        '
        Me.lblAttribute3.AutoSize = True
        Me.lblAttribute3.Location = New System.Drawing.Point(30, 177)
        Me.lblAttribute3.Name = "lblAttribute3"
        Me.lblAttribute3.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute3.TabIndex = 49
        Me.lblAttribute3.Text = "Attribute3:"
        '
        'lblAttribute4
        '
        Me.lblAttribute4.AutoSize = True
        Me.lblAttribute4.Location = New System.Drawing.Point(30, 190)
        Me.lblAttribute4.Name = "lblAttribute4"
        Me.lblAttribute4.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute4.TabIndex = 50
        Me.lblAttribute4.Text = "Attribute4:"
        '
        'lblAttribute5
        '
        Me.lblAttribute5.AutoSize = True
        Me.lblAttribute5.Location = New System.Drawing.Point(30, 203)
        Me.lblAttribute5.Name = "lblAttribute5"
        Me.lblAttribute5.Size = New System.Drawing.Size(60, 13)
        Me.lblAttribute5.TabIndex = 51
        Me.lblAttribute5.Text = "Attribute5:"
        '
        'lblAttributePoints5
        '
        Me.lblAttributePoints5.AutoSize = True
        Me.lblAttributePoints5.Location = New System.Drawing.Point(105, 203)
        Me.lblAttributePoints5.Name = "lblAttributePoints5"
        Me.lblAttributePoints5.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints5.TabIndex = 56
        Me.lblAttributePoints5.Text = "Attribute5:"
        '
        'lblAttributePoints4
        '
        Me.lblAttributePoints4.AutoSize = True
        Me.lblAttributePoints4.Location = New System.Drawing.Point(105, 190)
        Me.lblAttributePoints4.Name = "lblAttributePoints4"
        Me.lblAttributePoints4.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints4.TabIndex = 55
        Me.lblAttributePoints4.Text = "Attribute4:"
        '
        'lblAttributePoints3
        '
        Me.lblAttributePoints3.AutoSize = True
        Me.lblAttributePoints3.Location = New System.Drawing.Point(105, 177)
        Me.lblAttributePoints3.Name = "lblAttributePoints3"
        Me.lblAttributePoints3.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints3.TabIndex = 54
        Me.lblAttributePoints3.Text = "Attribute3:"
        '
        'lblAttributePoints2
        '
        Me.lblAttributePoints2.AutoSize = True
        Me.lblAttributePoints2.Location = New System.Drawing.Point(105, 164)
        Me.lblAttributePoints2.Name = "lblAttributePoints2"
        Me.lblAttributePoints2.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints2.TabIndex = 53
        Me.lblAttributePoints2.Text = "Attribute2:"
        '
        'lblAttributePoints1
        '
        Me.lblAttributePoints1.AutoSize = True
        Me.lblAttributePoints1.Location = New System.Drawing.Point(105, 151)
        Me.lblAttributePoints1.Name = "lblAttributePoints1"
        Me.lblAttributePoints1.Size = New System.Drawing.Size(60, 13)
        Me.lblAttributePoints1.TabIndex = 52
        Me.lblAttributePoints1.Text = "Attribute1:"
        '
        'lblTimeSaving
        '
        Me.lblTimeSaving.AutoSize = True
        Me.lblTimeSaving.Location = New System.Drawing.Point(16, 281)
        Me.lblTimeSaving.Name = "lblTimeSaving"
        Me.lblTimeSaving.Size = New System.Drawing.Size(68, 13)
        Me.lblTimeSaving.TabIndex = 57
        Me.lblTimeSaving.Text = "Time Saving:"
        '
        'btnResetImplants
        '
        Me.btnResetImplants.Location = New System.Drawing.Point(19, 392)
        Me.btnResetImplants.Name = "btnResetImplants"
        Me.btnResetImplants.Size = New System.Drawing.Size(75, 23)
        Me.btnResetImplants.TabIndex = 76
        Me.btnResetImplants.Text = "Reset"
        Me.btnResetImplants.UseVisualStyleBackColor = True
        '
        'nudMImplant
        '
        Me.nudMImplant.Location = New System.Drawing.Point(92, 334)
        Me.nudMImplant.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.nudMImplant.Name = "nudMImplant"
        Me.nudMImplant.Size = New System.Drawing.Size(72, 21)
        Me.nudMImplant.TabIndex = 74
        Me.nudMImplant.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'nudWImplant
        '
        Me.nudWImplant.Location = New System.Drawing.Point(92, 264)
        Me.nudWImplant.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.nudWImplant.Name = "nudWImplant"
        Me.nudWImplant.Size = New System.Drawing.Size(72, 21)
        Me.nudWImplant.TabIndex = 73
        Me.nudWImplant.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'nudCImplant
        '
        Me.nudCImplant.Location = New System.Drawing.Point(92, 194)
        Me.nudCImplant.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.nudCImplant.Name = "nudCImplant"
        Me.nudCImplant.Size = New System.Drawing.Size(72, 21)
        Me.nudCImplant.TabIndex = 72
        Me.nudCImplant.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'nudPImplant
        '
        Me.nudPImplant.Location = New System.Drawing.Point(92, 124)
        Me.nudPImplant.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.nudPImplant.Name = "nudPImplant"
        Me.nudPImplant.Size = New System.Drawing.Size(72, 21)
        Me.nudPImplant.TabIndex = 71
        Me.nudPImplant.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'nudIImplant
        '
        Me.nudIImplant.Location = New System.Drawing.Point(92, 54)
        Me.nudIImplant.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.nudIImplant.Name = "nudIImplant"
        Me.nudIImplant.Size = New System.Drawing.Size(72, 21)
        Me.nudIImplant.TabIndex = 70
        Me.nudIImplant.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblPerceptionImplant
        '
        Me.lblPerceptionImplant.AutoSize = True
        Me.lblPerceptionImplant.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPerceptionImplant.Location = New System.Drawing.Point(89, 101)
        Me.lblPerceptionImplant.Name = "lblPerceptionImplant"
        Me.lblPerceptionImplant.Size = New System.Drawing.Size(73, 14)
        Me.lblPerceptionImplant.TabIndex = 69
        Me.lblPerceptionImplant.Text = "Perception"
        '
        'lblCharismaImplant
        '
        Me.lblCharismaImplant.AutoSize = True
        Me.lblCharismaImplant.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCharismaImplant.Location = New System.Drawing.Point(89, 171)
        Me.lblCharismaImplant.Name = "lblCharismaImplant"
        Me.lblCharismaImplant.Size = New System.Drawing.Size(62, 14)
        Me.lblCharismaImplant.TabIndex = 68
        Me.lblCharismaImplant.Text = "Charisma"
        '
        'lblWillpowerImplant
        '
        Me.lblWillpowerImplant.AutoSize = True
        Me.lblWillpowerImplant.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWillpowerImplant.Location = New System.Drawing.Point(89, 241)
        Me.lblWillpowerImplant.Name = "lblWillpowerImplant"
        Me.lblWillpowerImplant.Size = New System.Drawing.Size(67, 14)
        Me.lblWillpowerImplant.TabIndex = 67
        Me.lblWillpowerImplant.Text = "Willpower"
        '
        'lblMemoryImplant
        '
        Me.lblMemoryImplant.AutoSize = True
        Me.lblMemoryImplant.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMemoryImplant.Location = New System.Drawing.Point(89, 311)
        Me.lblMemoryImplant.Name = "lblMemoryImplant"
        Me.lblMemoryImplant.Size = New System.Drawing.Size(56, 14)
        Me.lblMemoryImplant.TabIndex = 66
        Me.lblMemoryImplant.Text = "Memory"
        '
        'lblIntelligenceImplant
        '
        Me.lblIntelligenceImplant.AutoSize = True
        Me.lblIntelligenceImplant.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIntelligenceImplant.Location = New System.Drawing.Point(89, 31)
        Me.lblIntelligenceImplant.Name = "lblIntelligenceImplant"
        Me.lblIntelligenceImplant.Size = New System.Drawing.Size(78, 14)
        Me.lblIntelligenceImplant.TabIndex = 65
        Me.lblIntelligenceImplant.Text = "Intelligence"
        '
        'PictureBox7
        '
        Me.PictureBox7.Image = CType(resources.GetObject("PictureBox7.Image"), System.Drawing.Image)
        Me.PictureBox7.Location = New System.Drawing.Point(19, 101)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox7.TabIndex = 64
        Me.PictureBox7.TabStop = False
        '
        'PictureBox8
        '
        Me.PictureBox8.Image = CType(resources.GetObject("PictureBox8.Image"), System.Drawing.Image)
        Me.PictureBox8.Location = New System.Drawing.Point(19, 171)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox8.TabIndex = 63
        Me.PictureBox8.TabStop = False
        '
        'PictureBox9
        '
        Me.PictureBox9.Image = CType(resources.GetObject("PictureBox9.Image"), System.Drawing.Image)
        Me.PictureBox9.Location = New System.Drawing.Point(19, 241)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox9.TabIndex = 62
        Me.PictureBox9.TabStop = False
        '
        'PictureBox10
        '
        Me.PictureBox10.Image = CType(resources.GetObject("PictureBox10.Image"), System.Drawing.Image)
        Me.PictureBox10.Location = New System.Drawing.Point(19, 311)
        Me.PictureBox10.Name = "PictureBox10"
        Me.PictureBox10.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox10.TabIndex = 61
        Me.PictureBox10.TabStop = False
        '
        'PictureBox11
        '
        Me.PictureBox11.Image = CType(resources.GetObject("PictureBox11.Image"), System.Drawing.Image)
        Me.PictureBox11.Location = New System.Drawing.Point(19, 31)
        Me.PictureBox11.Name = "PictureBox11"
        Me.PictureBox11.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox11.TabIndex = 60
        Me.PictureBox11.TabStop = False
        '
        'gbImplants
        '
        Me.gbImplants.Controls.Add(Me.PictureBox11)
        Me.gbImplants.Controls.Add(Me.btnResetImplants)
        Me.gbImplants.Controls.Add(Me.PictureBox10)
        Me.gbImplants.Controls.Add(Me.nudMImplant)
        Me.gbImplants.Controls.Add(Me.PictureBox9)
        Me.gbImplants.Controls.Add(Me.nudWImplant)
        Me.gbImplants.Controls.Add(Me.PictureBox8)
        Me.gbImplants.Controls.Add(Me.nudCImplant)
        Me.gbImplants.Controls.Add(Me.PictureBox7)
        Me.gbImplants.Controls.Add(Me.nudPImplant)
        Me.gbImplants.Controls.Add(Me.lblIntelligenceImplant)
        Me.gbImplants.Controls.Add(Me.nudIImplant)
        Me.gbImplants.Controls.Add(Me.lblIBase)
        Me.gbImplants.Controls.Add(Me.lblISkills)
        Me.gbImplants.Controls.Add(Me.lblMemoryImplant)
        Me.gbImplants.Controls.Add(Me.lblITotal)
        Me.gbImplants.Controls.Add(Me.lblPerceptionImplant)
        Me.gbImplants.Controls.Add(Me.lblWillpowerImplant)
        Me.gbImplants.Controls.Add(Me.lblPBase)
        Me.gbImplants.Controls.Add(Me.lblCharismaImplant)
        Me.gbImplants.Controls.Add(Me.lblPSkills)
        Me.gbImplants.Controls.Add(Me.lblMBase)
        Me.gbImplants.Controls.Add(Me.lblPTotal)
        Me.gbImplants.Controls.Add(Me.lblWTotal)
        Me.gbImplants.Controls.Add(Me.lblMSkills)
        Me.gbImplants.Controls.Add(Me.lblCBase)
        Me.gbImplants.Controls.Add(Me.lblWSkills)
        Me.gbImplants.Controls.Add(Me.lblCSkills)
        Me.gbImplants.Controls.Add(Me.lblMTotal)
        Me.gbImplants.Controls.Add(Me.lblCTotal)
        Me.gbImplants.Controls.Add(Me.lblWBase)
        Me.gbImplants.Location = New System.Drawing.Point(18, 46)
        Me.gbImplants.Name = "gbImplants"
        Me.gbImplants.Size = New System.Drawing.Size(250, 429)
        Me.gbImplants.TabIndex = 77
        Me.gbImplants.TabStop = False
        Me.gbImplants.Text = "Implants"
        '
        'gbSkillQueue
        '
        Me.gbSkillQueue.Controls.Add(Me.lblSkillQueueAnalysis)
        Me.gbSkillQueue.Controls.Add(Me.lblActiveSkillQueueLbl)
        Me.gbSkillQueue.Controls.Add(Me.lblActiveSkillQueue)
        Me.gbSkillQueue.Controls.Add(Me.lblActiveQueueTime)
        Me.gbSkillQueue.Controls.Add(Me.lblTimeSaving)
        Me.gbSkillQueue.Controls.Add(Me.lblRevisedQueueTime)
        Me.gbSkillQueue.Controls.Add(Me.lblAttributePoints5)
        Me.gbSkillQueue.Controls.Add(Me.lblAttribute1)
        Me.gbSkillQueue.Controls.Add(Me.lblAttributePoints4)
        Me.gbSkillQueue.Controls.Add(Me.lblSkillQueuePointsAnalysis)
        Me.gbSkillQueue.Controls.Add(Me.lblAttributePoints3)
        Me.gbSkillQueue.Controls.Add(Me.lblAttribute2)
        Me.gbSkillQueue.Controls.Add(Me.lblAttributePoints2)
        Me.gbSkillQueue.Controls.Add(Me.lblAttribute3)
        Me.gbSkillQueue.Controls.Add(Me.lblAttributePoints1)
        Me.gbSkillQueue.Controls.Add(Me.lblAttribute4)
        Me.gbSkillQueue.Controls.Add(Me.lblAttribute5)
        Me.gbSkillQueue.Location = New System.Drawing.Point(283, 46)
        Me.gbSkillQueue.Name = "gbSkillQueue"
        Me.gbSkillQueue.Size = New System.Drawing.Size(222, 429)
        Me.gbSkillQueue.TabIndex = 79
        Me.gbSkillQueue.TabStop = False
        Me.gbSkillQueue.Text = "Skill Queue"
        '
        'frmImplants
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(516, 485)
        Me.Controls.Add(Me.gbSkillQueue)
        Me.Controls.Add(Me.gbImplants)
        Me.Controls.Add(Me.PictureBox6)
        Me.Controls.Add(Me.lblDescription)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmImplants"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Implants"
        Me.TopMost = True
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMImplant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudWImplant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCImplant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPImplant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIImplant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbImplants.ResumeLayout(False)
        Me.gbImplants.PerformLayout()
        Me.gbSkillQueue.ResumeLayout(False)
        Me.gbSkillQueue.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents lblIBase As System.Windows.Forms.Label
    Friend WithEvents lblISkills As System.Windows.Forms.Label
    Friend WithEvents lblITotal As System.Windows.Forms.Label
    Friend WithEvents lblPTotal As System.Windows.Forms.Label
    Friend WithEvents lblPSkills As System.Windows.Forms.Label
    Friend WithEvents lblPBase As System.Windows.Forms.Label
    Friend WithEvents lblCTotal As System.Windows.Forms.Label
    Friend WithEvents lblCSkills As System.Windows.Forms.Label
    Friend WithEvents lblCBase As System.Windows.Forms.Label
    Friend WithEvents lblWTotal As System.Windows.Forms.Label
    Friend WithEvents lblWSkills As System.Windows.Forms.Label
    Friend WithEvents lblWBase As System.Windows.Forms.Label
    Friend WithEvents lblMTotal As System.Windows.Forms.Label
    Friend WithEvents lblMSkills As System.Windows.Forms.Label
    Friend WithEvents lblMBase As System.Windows.Forms.Label
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
    Friend WithEvents btnResetImplants As System.Windows.Forms.Button
    Friend WithEvents nudMImplant As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudWImplant As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudCImplant As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudPImplant As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudIImplant As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblPerceptionImplant As System.Windows.Forms.Label
    Friend WithEvents lblCharismaImplant As System.Windows.Forms.Label
    Friend WithEvents lblWillpowerImplant As System.Windows.Forms.Label
    Friend WithEvents lblMemoryImplant As System.Windows.Forms.Label
    Friend WithEvents lblIntelligenceImplant As System.Windows.Forms.Label
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox9 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox10 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox11 As System.Windows.Forms.PictureBox
    Friend WithEvents gbImplants As System.Windows.Forms.GroupBox
    Friend WithEvents gbSkillQueue As System.Windows.Forms.GroupBox
End Class
