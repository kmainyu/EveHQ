Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmNeuralRemap
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmNeuralRemap))
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.PictureBox1 = New System.Windows.Forms.PictureBox()
            Me.PictureBox2 = New System.Windows.Forms.PictureBox()
            Me.PictureBox3 = New System.Windows.Forms.PictureBox()
            Me.PictureBox4 = New System.Windows.Forms.PictureBox()
            Me.PictureBox5 = New System.Windows.Forms.PictureBox()
            Me.lblIntelligence = New System.Windows.Forms.Label()
            Me.lblMemory = New System.Windows.Forms.Label()
            Me.lblWillpower = New System.Windows.Forms.Label()
            Me.lblCharisma = New System.Windows.Forms.Label()
            Me.lblPerception = New System.Windows.Forms.Label()
            Me.nudIBase = New System.Windows.Forms.NumericUpDown()
            Me.lblIImplant = New System.Windows.Forms.Label()
            Me.lblITotal = New System.Windows.Forms.Label()
            Me.lblPTotal = New System.Windows.Forms.Label()
            Me.lblPImplant = New System.Windows.Forms.Label()
            Me.nudPBase = New System.Windows.Forms.NumericUpDown()
            Me.lblCTotal = New System.Windows.Forms.Label()
            Me.lblCImplant = New System.Windows.Forms.Label()
            Me.nudCBase = New System.Windows.Forms.NumericUpDown()
            Me.lblWTotal = New System.Windows.Forms.Label()
            Me.lblWImplant = New System.Windows.Forms.Label()
            Me.nudWBase = New System.Windows.Forms.NumericUpDown()
            Me.lblMTotal = New System.Windows.Forms.Label()
            Me.lblMImplant = New System.Windows.Forms.Label()
            Me.nudMBase = New System.Windows.Forms.NumericUpDown()
            Me.lblUnusedPointsLbl = New System.Windows.Forms.Label()
            Me.lblUnusedPoints = New System.Windows.Forms.Label()
            Me.PictureBox6 = New System.Windows.Forms.PictureBox()
            Me.lblSkillQueueAnalysis = New System.Windows.Forms.Label()
            Me.lblActiveSkillQueueLbl = New System.Windows.Forms.Label()
            Me.lblActiveSkillQueue = New System.Windows.Forms.Label()
            Me.lblActiveQueueTime = New System.Windows.Forms.Label()
            Me.lblRevisedQueueTime = New System.Windows.Forms.Label()
            Me.lblAttribute1 = New System.Windows.Forms.Label()
            Me.lblSkillQueuePointsAnalysis = New System.Windows.Forms.Label()
            Me.lblAttribute2 = New System.Windows.Forms.Label()
            Me.lblAttribute3 = New System.Windows.Forms.Label()
            Me.lblAttribute4 = New System.Windows.Forms.Label()
            Me.lblAttribute5 = New System.Windows.Forms.Label()
            Me.lblAttributePoints5 = New System.Windows.Forms.Label()
            Me.lblAttributePoints4 = New System.Windows.Forms.Label()
            Me.lblAttributePoints3 = New System.Windows.Forms.Label()
            Me.lblAttributePoints2 = New System.Windows.Forms.Label()
            Me.lblAttributePoints1 = New System.Windows.Forms.Label()
            Me.lblTimeSaving = New System.Windows.Forms.Label()
            Me.btnOptimise = New System.Windows.Forms.Button()
            Me.btnReset = New System.Windows.Forms.Button()
            Me.gpAttributes = New DevComponents.DotNetBar.Controls.GroupPanel()
            Me.gpSkillQueue = New DevComponents.DotNetBar.Controls.GroupPanel()
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
            Me.gpAttributes.SuspendLayout()
            Me.gpSkillQueue.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblDescription
            '
            Me.lblDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblDescription.Location = New System.Drawing.Point(15, 9)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(522, 34)
            Me.lblDescription.TabIndex = 0
            Me.lblDescription.Text = "Neural Remapping allows you to respecify your starting base attributes which can " & _
                                     "be used to optimise skill training in a particular area. Attributes cannot excee" & _
                                     "d 27." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            '
            'PictureBox1
            '
            Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
            Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(64, 64)
            Me.PictureBox1.TabIndex = 2
            Me.PictureBox1.TabStop = False
            '
            'PictureBox2
            '
            Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
            Me.PictureBox2.Location = New System.Drawing.Point(12, 292)
            Me.PictureBox2.Name = "PictureBox2"
            Me.PictureBox2.Size = New System.Drawing.Size(64, 64)
            Me.PictureBox2.TabIndex = 3
            Me.PictureBox2.TabStop = False
            '
            'PictureBox3
            '
            Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
            Me.PictureBox3.Location = New System.Drawing.Point(12, 222)
            Me.PictureBox3.Name = "PictureBox3"
            Me.PictureBox3.Size = New System.Drawing.Size(64, 64)
            Me.PictureBox3.TabIndex = 4
            Me.PictureBox3.TabStop = False
            '
            'PictureBox4
            '
            Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
            Me.PictureBox4.Location = New System.Drawing.Point(12, 152)
            Me.PictureBox4.Name = "PictureBox4"
            Me.PictureBox4.Size = New System.Drawing.Size(64, 64)
            Me.PictureBox4.TabIndex = 5
            Me.PictureBox4.TabStop = False
            '
            'PictureBox5
            '
            Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
            Me.PictureBox5.Location = New System.Drawing.Point(12, 82)
            Me.PictureBox5.Name = "PictureBox5"
            Me.PictureBox5.Size = New System.Drawing.Size(64, 64)
            Me.PictureBox5.TabIndex = 6
            Me.PictureBox5.TabStop = False
            '
            'lblIntelligence
            '
            Me.lblIntelligence.AutoSize = True
            Me.lblIntelligence.BackColor = System.Drawing.Color.Transparent
            Me.lblIntelligence.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblIntelligence.Location = New System.Drawing.Point(82, 12)
            Me.lblIntelligence.Name = "lblIntelligence"
            Me.lblIntelligence.Size = New System.Drawing.Size(78, 14)
            Me.lblIntelligence.TabIndex = 7
            Me.lblIntelligence.Text = "Intelligence"
            '
            'lblMemory
            '
            Me.lblMemory.AutoSize = True
            Me.lblMemory.BackColor = System.Drawing.Color.Transparent
            Me.lblMemory.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblMemory.Location = New System.Drawing.Point(82, 292)
            Me.lblMemory.Name = "lblMemory"
            Me.lblMemory.Size = New System.Drawing.Size(56, 14)
            Me.lblMemory.TabIndex = 8
            Me.lblMemory.Text = "Memory"
            '
            'lblWillpower
            '
            Me.lblWillpower.AutoSize = True
            Me.lblWillpower.BackColor = System.Drawing.Color.Transparent
            Me.lblWillpower.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblWillpower.Location = New System.Drawing.Point(82, 222)
            Me.lblWillpower.Name = "lblWillpower"
            Me.lblWillpower.Size = New System.Drawing.Size(67, 14)
            Me.lblWillpower.TabIndex = 9
            Me.lblWillpower.Text = "Willpower"
            '
            'lblCharisma
            '
            Me.lblCharisma.AutoSize = True
            Me.lblCharisma.BackColor = System.Drawing.Color.Transparent
            Me.lblCharisma.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCharisma.Location = New System.Drawing.Point(82, 152)
            Me.lblCharisma.Name = "lblCharisma"
            Me.lblCharisma.Size = New System.Drawing.Size(62, 14)
            Me.lblCharisma.TabIndex = 10
            Me.lblCharisma.Text = "Charisma"
            '
            'lblPerception
            '
            Me.lblPerception.AutoSize = True
            Me.lblPerception.BackColor = System.Drawing.Color.Transparent
            Me.lblPerception.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPerception.Location = New System.Drawing.Point(82, 82)
            Me.lblPerception.Name = "lblPerception"
            Me.lblPerception.Size = New System.Drawing.Size(73, 14)
            Me.lblPerception.TabIndex = 11
            Me.lblPerception.Text = "Perception"
            '
            'nudIBase
            '
            Me.nudIBase.Location = New System.Drawing.Point(85, 35)
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
            Me.lblIImplant.BackColor = System.Drawing.Color.Transparent
            Me.lblIImplant.Location = New System.Drawing.Point(163, 37)
            Me.lblIImplant.Name = "lblIImplant"
            Me.lblIImplant.Size = New System.Drawing.Size(47, 13)
            Me.lblIImplant.TabIndex = 14
            Me.lblIImplant.Text = "Implant:"
            '
            'lblITotal
            '
            Me.lblITotal.AutoSize = True
            Me.lblITotal.BackColor = System.Drawing.Color.Transparent
            Me.lblITotal.Location = New System.Drawing.Point(163, 50)
            Me.lblITotal.Name = "lblITotal"
            Me.lblITotal.Size = New System.Drawing.Size(35, 13)
            Me.lblITotal.TabIndex = 16
            Me.lblITotal.Text = "Total:"
            '
            'lblPTotal
            '
            Me.lblPTotal.AutoSize = True
            Me.lblPTotal.BackColor = System.Drawing.Color.Transparent
            Me.lblPTotal.Location = New System.Drawing.Point(163, 120)
            Me.lblPTotal.Name = "lblPTotal"
            Me.lblPTotal.Size = New System.Drawing.Size(35, 13)
            Me.lblPTotal.TabIndex = 21
            Me.lblPTotal.Text = "Total:"
            '
            'lblPImplant
            '
            Me.lblPImplant.AutoSize = True
            Me.lblPImplant.BackColor = System.Drawing.Color.Transparent
            Me.lblPImplant.Location = New System.Drawing.Point(163, 107)
            Me.lblPImplant.Name = "lblPImplant"
            Me.lblPImplant.Size = New System.Drawing.Size(47, 13)
            Me.lblPImplant.TabIndex = 19
            Me.lblPImplant.Text = "Implant:"
            '
            'nudPBase
            '
            Me.nudPBase.Location = New System.Drawing.Point(85, 105)
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
            Me.lblCTotal.BackColor = System.Drawing.Color.Transparent
            Me.lblCTotal.Location = New System.Drawing.Point(163, 190)
            Me.lblCTotal.Name = "lblCTotal"
            Me.lblCTotal.Size = New System.Drawing.Size(35, 13)
            Me.lblCTotal.TabIndex = 26
            Me.lblCTotal.Text = "Total:"
            '
            'lblCImplant
            '
            Me.lblCImplant.AutoSize = True
            Me.lblCImplant.BackColor = System.Drawing.Color.Transparent
            Me.lblCImplant.Location = New System.Drawing.Point(163, 177)
            Me.lblCImplant.Name = "lblCImplant"
            Me.lblCImplant.Size = New System.Drawing.Size(47, 13)
            Me.lblCImplant.TabIndex = 24
            Me.lblCImplant.Text = "Implant:"
            '
            'nudCBase
            '
            Me.nudCBase.Location = New System.Drawing.Point(85, 175)
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
            Me.lblWTotal.BackColor = System.Drawing.Color.Transparent
            Me.lblWTotal.Location = New System.Drawing.Point(163, 260)
            Me.lblWTotal.Name = "lblWTotal"
            Me.lblWTotal.Size = New System.Drawing.Size(35, 13)
            Me.lblWTotal.TabIndex = 31
            Me.lblWTotal.Text = "Total:"
            '
            'lblWImplant
            '
            Me.lblWImplant.AutoSize = True
            Me.lblWImplant.BackColor = System.Drawing.Color.Transparent
            Me.lblWImplant.Location = New System.Drawing.Point(163, 247)
            Me.lblWImplant.Name = "lblWImplant"
            Me.lblWImplant.Size = New System.Drawing.Size(47, 13)
            Me.lblWImplant.TabIndex = 29
            Me.lblWImplant.Text = "Implant:"
            '
            'nudWBase
            '
            Me.nudWBase.Location = New System.Drawing.Point(85, 245)
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
            Me.lblMTotal.BackColor = System.Drawing.Color.Transparent
            Me.lblMTotal.Location = New System.Drawing.Point(163, 330)
            Me.lblMTotal.Name = "lblMTotal"
            Me.lblMTotal.Size = New System.Drawing.Size(35, 13)
            Me.lblMTotal.TabIndex = 36
            Me.lblMTotal.Text = "Total:"
            '
            'lblMImplant
            '
            Me.lblMImplant.AutoSize = True
            Me.lblMImplant.BackColor = System.Drawing.Color.Transparent
            Me.lblMImplant.Location = New System.Drawing.Point(163, 317)
            Me.lblMImplant.Name = "lblMImplant"
            Me.lblMImplant.Size = New System.Drawing.Size(47, 13)
            Me.lblMImplant.TabIndex = 34
            Me.lblMImplant.Text = "Implant:"
            '
            'nudMBase
            '
            Me.nudMBase.Location = New System.Drawing.Point(85, 315)
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
            Me.lblUnusedPointsLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblUnusedPointsLbl.Location = New System.Drawing.Point(93, 378)
            Me.lblUnusedPointsLbl.Name = "lblUnusedPointsLbl"
            Me.lblUnusedPointsLbl.Size = New System.Drawing.Size(125, 13)
            Me.lblUnusedPointsLbl.TabIndex = 37
            Me.lblUnusedPointsLbl.Text = "Unused Attribute Points:"
            '
            'lblUnusedPoints
            '
            Me.lblUnusedPoints.AutoSize = True
            Me.lblUnusedPoints.BackColor = System.Drawing.Color.Transparent
            Me.lblUnusedPoints.Location = New System.Drawing.Point(224, 378)
            Me.lblUnusedPoints.Name = "lblUnusedPoints"
            Me.lblUnusedPoints.Size = New System.Drawing.Size(13, 13)
            Me.lblUnusedPoints.TabIndex = 38
            Me.lblUnusedPoints.Text = "0"
            '
            'PictureBox6
            '
            Me.PictureBox6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
            Me.PictureBox6.Location = New System.Drawing.Point(543, 3)
            Me.PictureBox6.Name = "PictureBox6"
            Me.PictureBox6.Size = New System.Drawing.Size(40, 40)
            Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureBox6.TabIndex = 39
            Me.PictureBox6.TabStop = False
            '
            'lblSkillQueueAnalysis
            '
            Me.lblSkillQueueAnalysis.AutoSize = True
            Me.lblSkillQueueAnalysis.BackColor = System.Drawing.Color.Transparent
            Me.lblSkillQueueAnalysis.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSkillQueueAnalysis.Location = New System.Drawing.Point(3, 12)
            Me.lblSkillQueueAnalysis.Name = "lblSkillQueueAnalysis"
            Me.lblSkillQueueAnalysis.Size = New System.Drawing.Size(127, 14)
            Me.lblSkillQueueAnalysis.TabIndex = 41
            Me.lblSkillQueueAnalysis.Text = "Skill Queue Analysis"
            '
            'lblActiveSkillQueueLbl
            '
            Me.lblActiveSkillQueueLbl.AutoSize = True
            Me.lblActiveSkillQueueLbl.BackColor = System.Drawing.Color.Transparent
            Me.lblActiveSkillQueueLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblActiveSkillQueueLbl.Location = New System.Drawing.Point(14, 37)
            Me.lblActiveSkillQueueLbl.Name = "lblActiveSkillQueueLbl"
            Me.lblActiveSkillQueueLbl.Size = New System.Drawing.Size(118, 13)
            Me.lblActiveSkillQueueLbl.TabIndex = 42
            Me.lblActiveSkillQueueLbl.Text = "Current Skill Queue:"
            '
            'lblActiveSkillQueue
            '
            Me.lblActiveSkillQueue.AutoSize = True
            Me.lblActiveSkillQueue.BackColor = System.Drawing.Color.Transparent
            Me.lblActiveSkillQueue.Location = New System.Drawing.Point(14, 63)
            Me.lblActiveSkillQueue.Name = "lblActiveSkillQueue"
            Me.lblActiveSkillQueue.Size = New System.Drawing.Size(72, 13)
            Me.lblActiveSkillQueue.TabIndex = 43
            Me.lblActiveSkillQueue.Text = "Active Queue"
            '
            'lblActiveQueueTime
            '
            Me.lblActiveQueueTime.AutoSize = True
            Me.lblActiveQueueTime.BackColor = System.Drawing.Color.Transparent
            Me.lblActiveQueueTime.Location = New System.Drawing.Point(3, 222)
            Me.lblActiveQueueTime.Name = "lblActiveQueueTime"
            Me.lblActiveQueueTime.Size = New System.Drawing.Size(85, 13)
            Me.lblActiveQueueTime.TabIndex = 44
            Me.lblActiveQueueTime.Text = "Time Remaining:"
            '
            'lblRevisedQueueTime
            '
            Me.lblRevisedQueueTime.AutoSize = True
            Me.lblRevisedQueueTime.BackColor = System.Drawing.Color.Transparent
            Me.lblRevisedQueueTime.Location = New System.Drawing.Point(3, 242)
            Me.lblRevisedQueueTime.Name = "lblRevisedQueueTime"
            Me.lblRevisedQueueTime.Size = New System.Drawing.Size(74, 13)
            Me.lblRevisedQueueTime.TabIndex = 45
            Me.lblRevisedQueueTime.Text = "Revised Time:"
            '
            'lblAttribute1
            '
            Me.lblAttribute1.AutoSize = True
            Me.lblAttribute1.BackColor = System.Drawing.Color.Transparent
            Me.lblAttribute1.Location = New System.Drawing.Point(17, 132)
            Me.lblAttribute1.Name = "lblAttribute1"
            Me.lblAttribute1.Size = New System.Drawing.Size(60, 13)
            Me.lblAttribute1.TabIndex = 46
            Me.lblAttribute1.Text = "Attribute1:"
            '
            'lblSkillQueuePointsAnalysis
            '
            Me.lblSkillQueuePointsAnalysis.AutoSize = True
            Me.lblSkillQueuePointsAnalysis.BackColor = System.Drawing.Color.Transparent
            Me.lblSkillQueuePointsAnalysis.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSkillQueuePointsAnalysis.Location = New System.Drawing.Point(14, 105)
            Me.lblSkillQueuePointsAnalysis.Name = "lblSkillQueuePointsAnalysis"
            Me.lblSkillQueuePointsAnalysis.Size = New System.Drawing.Size(160, 13)
            Me.lblSkillQueuePointsAnalysis.TabIndex = 47
            Me.lblSkillQueuePointsAnalysis.Text = "Skill Queue Points Analysis:"
            '
            'lblAttribute2
            '
            Me.lblAttribute2.AutoSize = True
            Me.lblAttribute2.BackColor = System.Drawing.Color.Transparent
            Me.lblAttribute2.Location = New System.Drawing.Point(17, 145)
            Me.lblAttribute2.Name = "lblAttribute2"
            Me.lblAttribute2.Size = New System.Drawing.Size(60, 13)
            Me.lblAttribute2.TabIndex = 48
            Me.lblAttribute2.Text = "Attribute2:"
            '
            'lblAttribute3
            '
            Me.lblAttribute3.AutoSize = True
            Me.lblAttribute3.BackColor = System.Drawing.Color.Transparent
            Me.lblAttribute3.Location = New System.Drawing.Point(17, 158)
            Me.lblAttribute3.Name = "lblAttribute3"
            Me.lblAttribute3.Size = New System.Drawing.Size(60, 13)
            Me.lblAttribute3.TabIndex = 49
            Me.lblAttribute3.Text = "Attribute3:"
            '
            'lblAttribute4
            '
            Me.lblAttribute4.AutoSize = True
            Me.lblAttribute4.BackColor = System.Drawing.Color.Transparent
            Me.lblAttribute4.Location = New System.Drawing.Point(17, 171)
            Me.lblAttribute4.Name = "lblAttribute4"
            Me.lblAttribute4.Size = New System.Drawing.Size(60, 13)
            Me.lblAttribute4.TabIndex = 50
            Me.lblAttribute4.Text = "Attribute4:"
            '
            'lblAttribute5
            '
            Me.lblAttribute5.AutoSize = True
            Me.lblAttribute5.BackColor = System.Drawing.Color.Transparent
            Me.lblAttribute5.Location = New System.Drawing.Point(17, 184)
            Me.lblAttribute5.Name = "lblAttribute5"
            Me.lblAttribute5.Size = New System.Drawing.Size(60, 13)
            Me.lblAttribute5.TabIndex = 51
            Me.lblAttribute5.Text = "Attribute5:"
            '
            'lblAttributePoints5
            '
            Me.lblAttributePoints5.AutoSize = True
            Me.lblAttributePoints5.BackColor = System.Drawing.Color.Transparent
            Me.lblAttributePoints5.Location = New System.Drawing.Point(92, 184)
            Me.lblAttributePoints5.Name = "lblAttributePoints5"
            Me.lblAttributePoints5.Size = New System.Drawing.Size(60, 13)
            Me.lblAttributePoints5.TabIndex = 56
            Me.lblAttributePoints5.Text = "Attribute5:"
            '
            'lblAttributePoints4
            '
            Me.lblAttributePoints4.AutoSize = True
            Me.lblAttributePoints4.BackColor = System.Drawing.Color.Transparent
            Me.lblAttributePoints4.Location = New System.Drawing.Point(92, 171)
            Me.lblAttributePoints4.Name = "lblAttributePoints4"
            Me.lblAttributePoints4.Size = New System.Drawing.Size(60, 13)
            Me.lblAttributePoints4.TabIndex = 55
            Me.lblAttributePoints4.Text = "Attribute4:"
            '
            'lblAttributePoints3
            '
            Me.lblAttributePoints3.AutoSize = True
            Me.lblAttributePoints3.BackColor = System.Drawing.Color.Transparent
            Me.lblAttributePoints3.Location = New System.Drawing.Point(92, 158)
            Me.lblAttributePoints3.Name = "lblAttributePoints3"
            Me.lblAttributePoints3.Size = New System.Drawing.Size(60, 13)
            Me.lblAttributePoints3.TabIndex = 54
            Me.lblAttributePoints3.Text = "Attribute3:"
            '
            'lblAttributePoints2
            '
            Me.lblAttributePoints2.AutoSize = True
            Me.lblAttributePoints2.BackColor = System.Drawing.Color.Transparent
            Me.lblAttributePoints2.Location = New System.Drawing.Point(92, 145)
            Me.lblAttributePoints2.Name = "lblAttributePoints2"
            Me.lblAttributePoints2.Size = New System.Drawing.Size(60, 13)
            Me.lblAttributePoints2.TabIndex = 53
            Me.lblAttributePoints2.Text = "Attribute2:"
            '
            'lblAttributePoints1
            '
            Me.lblAttributePoints1.AutoSize = True
            Me.lblAttributePoints1.BackColor = System.Drawing.Color.Transparent
            Me.lblAttributePoints1.Location = New System.Drawing.Point(92, 132)
            Me.lblAttributePoints1.Name = "lblAttributePoints1"
            Me.lblAttributePoints1.Size = New System.Drawing.Size(60, 13)
            Me.lblAttributePoints1.TabIndex = 52
            Me.lblAttributePoints1.Text = "Attribute1:"
            '
            'lblTimeSaving
            '
            Me.lblTimeSaving.AutoSize = True
            Me.lblTimeSaving.BackColor = System.Drawing.Color.Transparent
            Me.lblTimeSaving.Location = New System.Drawing.Point(3, 262)
            Me.lblTimeSaving.Name = "lblTimeSaving"
            Me.lblTimeSaving.Size = New System.Drawing.Size(68, 13)
            Me.lblTimeSaving.TabIndex = 57
            Me.lblTimeSaving.Text = "Time Saving:"
            '
            'btnOptimise
            '
            Me.btnOptimise.Location = New System.Drawing.Point(6, 373)
            Me.btnOptimise.Name = "btnOptimise"
            Me.btnOptimise.Size = New System.Drawing.Size(150, 23)
            Me.btnOptimise.TabIndex = 58
            Me.btnOptimise.Text = "Optimise Base Attributes"
            Me.btnOptimise.UseVisualStyleBackColor = True
            '
            'btnReset
            '
            Me.btnReset.Location = New System.Drawing.Point(12, 373)
            Me.btnReset.Name = "btnReset"
            Me.btnReset.Size = New System.Drawing.Size(75, 23)
            Me.btnReset.TabIndex = 59
            Me.btnReset.Text = "Reset"
            Me.btnReset.UseVisualStyleBackColor = True
            '
            'gpAttributes
            '
            Me.gpAttributes.CanvasColor = System.Drawing.SystemColors.Control
            Me.gpAttributes.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
            Me.gpAttributes.Controls.Add(Me.PictureBox1)
            Me.gpAttributes.Controls.Add(Me.PictureBox2)
            Me.gpAttributes.Controls.Add(Me.nudMBase)
            Me.gpAttributes.Controls.Add(Me.btnReset)
            Me.gpAttributes.Controls.Add(Me.lblMImplant)
            Me.gpAttributes.Controls.Add(Me.PictureBox3)
            Me.gpAttributes.Controls.Add(Me.lblWTotal)
            Me.gpAttributes.Controls.Add(Me.PictureBox4)
            Me.gpAttributes.Controls.Add(Me.PictureBox5)
            Me.gpAttributes.Controls.Add(Me.lblIntelligence)
            Me.gpAttributes.Controls.Add(Me.lblMTotal)
            Me.gpAttributes.Controls.Add(Me.lblMemory)
            Me.gpAttributes.Controls.Add(Me.lblWImplant)
            Me.gpAttributes.Controls.Add(Me.lblWillpower)
            Me.gpAttributes.Controls.Add(Me.lblUnusedPointsLbl)
            Me.gpAttributes.Controls.Add(Me.lblCharisma)
            Me.gpAttributes.Controls.Add(Me.nudWBase)
            Me.gpAttributes.Controls.Add(Me.lblPerception)
            Me.gpAttributes.Controls.Add(Me.lblUnusedPoints)
            Me.gpAttributes.Controls.Add(Me.nudIBase)
            Me.gpAttributes.Controls.Add(Me.lblCTotal)
            Me.gpAttributes.Controls.Add(Me.lblIImplant)
            Me.gpAttributes.Controls.Add(Me.lblCImplant)
            Me.gpAttributes.Controls.Add(Me.lblITotal)
            Me.gpAttributes.Controls.Add(Me.nudCBase)
            Me.gpAttributes.Controls.Add(Me.nudPBase)
            Me.gpAttributes.Controls.Add(Me.lblPTotal)
            Me.gpAttributes.Controls.Add(Me.lblPImplant)
            Me.gpAttributes.IsShadowEnabled = True
            Me.gpAttributes.Location = New System.Drawing.Point(12, 46)
            Me.gpAttributes.Name = "gpAttributes"
            Me.gpAttributes.Size = New System.Drawing.Size(334, 429)
            '
            '
            '
            Me.gpAttributes.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.gpAttributes.Style.BackColorGradientAngle = 90
            Me.gpAttributes.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.gpAttributes.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpAttributes.Style.BorderBottomWidth = 1
            Me.gpAttributes.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.gpAttributes.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpAttributes.Style.BorderLeftWidth = 1
            Me.gpAttributes.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpAttributes.Style.BorderRightWidth = 1
            Me.gpAttributes.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpAttributes.Style.BorderTopWidth = 1
            Me.gpAttributes.Style.Class = ""
            Me.gpAttributes.Style.CornerDiameter = 4
            Me.gpAttributes.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
            Me.gpAttributes.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.gpAttributes.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.gpAttributes.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.gpAttributes.StyleMouseDown.Class = ""
            Me.gpAttributes.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.gpAttributes.StyleMouseOver.Class = ""
            Me.gpAttributes.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.gpAttributes.TabIndex = 80
            Me.gpAttributes.Text = "Attributes"
            '
            'gpSkillQueue
            '
            Me.gpSkillQueue.CanvasColor = System.Drawing.SystemColors.Control
            Me.gpSkillQueue.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
            Me.gpSkillQueue.Controls.Add(Me.lblSkillQueueAnalysis)
            Me.gpSkillQueue.Controls.Add(Me.lblActiveSkillQueueLbl)
            Me.gpSkillQueue.Controls.Add(Me.lblAttribute5)
            Me.gpSkillQueue.Controls.Add(Me.lblActiveSkillQueue)
            Me.gpSkillQueue.Controls.Add(Me.lblAttribute4)
            Me.gpSkillQueue.Controls.Add(Me.btnOptimise)
            Me.gpSkillQueue.Controls.Add(Me.lblAttributePoints1)
            Me.gpSkillQueue.Controls.Add(Me.lblActiveQueueTime)
            Me.gpSkillQueue.Controls.Add(Me.lblAttribute3)
            Me.gpSkillQueue.Controls.Add(Me.lblTimeSaving)
            Me.gpSkillQueue.Controls.Add(Me.lblAttributePoints2)
            Me.gpSkillQueue.Controls.Add(Me.lblRevisedQueueTime)
            Me.gpSkillQueue.Controls.Add(Me.lblAttribute2)
            Me.gpSkillQueue.Controls.Add(Me.lblAttributePoints5)
            Me.gpSkillQueue.Controls.Add(Me.lblAttributePoints3)
            Me.gpSkillQueue.Controls.Add(Me.lblAttribute1)
            Me.gpSkillQueue.Controls.Add(Me.lblSkillQueuePointsAnalysis)
            Me.gpSkillQueue.Controls.Add(Me.lblAttributePoints4)
            Me.gpSkillQueue.IsShadowEnabled = True
            Me.gpSkillQueue.Location = New System.Drawing.Point(352, 46)
            Me.gpSkillQueue.Name = "gpSkillQueue"
            Me.gpSkillQueue.Size = New System.Drawing.Size(225, 429)
            '
            '
            '
            Me.gpSkillQueue.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.gpSkillQueue.Style.BackColorGradientAngle = 90
            Me.gpSkillQueue.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.gpSkillQueue.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpSkillQueue.Style.BorderBottomWidth = 1
            Me.gpSkillQueue.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.gpSkillQueue.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpSkillQueue.Style.BorderLeftWidth = 1
            Me.gpSkillQueue.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpSkillQueue.Style.BorderRightWidth = 1
            Me.gpSkillQueue.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpSkillQueue.Style.BorderTopWidth = 1
            Me.gpSkillQueue.Style.Class = ""
            Me.gpSkillQueue.Style.CornerDiameter = 4
            Me.gpSkillQueue.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
            Me.gpSkillQueue.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.gpSkillQueue.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.gpSkillQueue.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.gpSkillQueue.StyleMouseDown.Class = ""
            Me.gpSkillQueue.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.gpSkillQueue.StyleMouseOver.Class = ""
            Me.gpSkillQueue.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.gpSkillQueue.TabIndex = 81
            Me.gpSkillQueue.Text = "Skill Queue"
            '
            'frmNeuralRemap
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(588, 488)
            Me.Controls.Add(Me.gpSkillQueue)
            Me.Controls.Add(Me.gpAttributes)
            Me.Controls.Add(Me.PictureBox6)
            Me.Controls.Add(Me.lblDescription)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
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
            Me.gpAttributes.ResumeLayout(False)
            Me.gpAttributes.PerformLayout()
            Me.gpSkillQueue.ResumeLayout(False)
            Me.gpSkillQueue.PerformLayout()
            Me.ResumeLayout(False)

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
        Friend WithEvents lblITotal As System.Windows.Forms.Label
        Friend WithEvents lblPTotal As System.Windows.Forms.Label
        Friend WithEvents lblPImplant As System.Windows.Forms.Label
        Friend WithEvents nudPBase As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblCTotal As System.Windows.Forms.Label
        Friend WithEvents lblCImplant As System.Windows.Forms.Label
        Friend WithEvents nudCBase As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblWTotal As System.Windows.Forms.Label
        Friend WithEvents lblWImplant As System.Windows.Forms.Label
        Friend WithEvents nudWBase As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblMTotal As System.Windows.Forms.Label
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
        Friend WithEvents gpAttributes As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents gpSkillQueue As DevComponents.DotNetBar.Controls.GroupPanel
    End Class
End NameSpace