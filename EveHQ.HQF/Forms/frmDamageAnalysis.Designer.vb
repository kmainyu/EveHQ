<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDamageAnalysis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDamageAnalysis))
        Me.cboAttackerFitting = New System.Windows.Forms.ComboBox
        Me.lblAttackerFitting = New System.Windows.Forms.Label
        Me.lblAttackerPilot = New System.Windows.Forms.Label
        Me.cboAttackerPilot = New System.Windows.Forms.ComboBox
        Me.lblAttacker = New System.Windows.Forms.Label
        Me.lblTarget = New System.Windows.Forms.Label
        Me.cboTargetFitting = New System.Windows.Forms.ComboBox
        Me.lblTargetFitting = New System.Windows.Forms.Label
        Me.lblTargetPilot = New System.Windows.Forms.Label
        Me.cboTargetPilot = New System.Windows.Forms.ComboBox
        Me.lblTurretStats = New System.Windows.Forms.Label
        Me.nudVel = New System.Windows.Forms.NumericUpDown
        Me.Label1 = New System.Windows.Forms.Label
        Me.nudRange = New System.Windows.Forms.NumericUpDown
        Me.lblRange = New System.Windows.Forms.Label
        Me.btnRangeVSHitChance = New System.Windows.Forms.Button
        Me.btnOptimalRange = New System.Windows.Forms.Button
        Me.tabStats = New System.Windows.Forms.TabControl
        Me.tabTurretStats = New System.Windows.Forms.TabPage
        Me.tabMissileStats = New System.Windows.Forms.TabPage
        Me.lblMissileStats = New System.Windows.Forms.Label
        Me.tabTurretMethod = New System.Windows.Forms.TabControl
        Me.tabShips = New System.Windows.Forms.TabPage
        Me.nudTargetVel = New System.Windows.Forms.NumericUpDown
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.radMovableVel = New System.Windows.Forms.RadioButton
        Me.nudAttackerVel = New System.Windows.Forms.NumericUpDown
        Me.radManualVelocity = New System.Windows.Forms.RadioButton
        Me.EveSpace1 = New EveHQ.HQF.EveSpace
        CType(Me.nudVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabStats.SuspendLayout()
        Me.tabTurretStats.SuspendLayout()
        Me.tabMissileStats.SuspendLayout()
        Me.tabTurretMethod.SuspendLayout()
        Me.tabShips.SuspendLayout()
        CType(Me.nudTargetVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAttackerVel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboAttackerFitting
        '
        Me.cboAttackerFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAttackerFitting.FormattingEnabled = True
        Me.cboAttackerFitting.Location = New System.Drawing.Point(98, 6)
        Me.cboAttackerFitting.Name = "cboAttackerFitting"
        Me.cboAttackerFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboAttackerFitting.TabIndex = 5
        '
        'lblAttackerFitting
        '
        Me.lblAttackerFitting.AutoSize = True
        Me.lblAttackerFitting.Location = New System.Drawing.Point(55, 9)
        Me.lblAttackerFitting.Name = "lblAttackerFitting"
        Me.lblAttackerFitting.Size = New System.Drawing.Size(41, 13)
        Me.lblAttackerFitting.TabIndex = 4
        Me.lblAttackerFitting.Text = "Fitting:"
        '
        'lblAttackerPilot
        '
        Me.lblAttackerPilot.AutoSize = True
        Me.lblAttackerPilot.Location = New System.Drawing.Point(291, 9)
        Me.lblAttackerPilot.Name = "lblAttackerPilot"
        Me.lblAttackerPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblAttackerPilot.TabIndex = 6
        Me.lblAttackerPilot.Text = "Pilot:"
        '
        'cboAttackerPilot
        '
        Me.cboAttackerPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAttackerPilot.FormattingEnabled = True
        Me.cboAttackerPilot.Location = New System.Drawing.Point(321, 6)
        Me.cboAttackerPilot.Name = "cboAttackerPilot"
        Me.cboAttackerPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboAttackerPilot.TabIndex = 7
        '
        'lblAttacker
        '
        Me.lblAttacker.AutoSize = True
        Me.lblAttacker.Location = New System.Drawing.Point(12, 9)
        Me.lblAttacker.Name = "lblAttacker"
        Me.lblAttacker.Size = New System.Drawing.Size(42, 13)
        Me.lblAttacker.TabIndex = 8
        Me.lblAttacker.Text = "Attack:"
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.Location = New System.Drawing.Point(12, 36)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(43, 13)
        Me.lblTarget.TabIndex = 13
        Me.lblTarget.Text = "Target:"
        '
        'cboTargetFitting
        '
        Me.cboTargetFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTargetFitting.FormattingEnabled = True
        Me.cboTargetFitting.Location = New System.Drawing.Point(98, 33)
        Me.cboTargetFitting.Name = "cboTargetFitting"
        Me.cboTargetFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboTargetFitting.TabIndex = 10
        '
        'lblTargetFitting
        '
        Me.lblTargetFitting.AutoSize = True
        Me.lblTargetFitting.Location = New System.Drawing.Point(55, 36)
        Me.lblTargetFitting.Name = "lblTargetFitting"
        Me.lblTargetFitting.Size = New System.Drawing.Size(41, 13)
        Me.lblTargetFitting.TabIndex = 9
        Me.lblTargetFitting.Text = "Fitting:"
        '
        'lblTargetPilot
        '
        Me.lblTargetPilot.AutoSize = True
        Me.lblTargetPilot.Location = New System.Drawing.Point(292, 36)
        Me.lblTargetPilot.Name = "lblTargetPilot"
        Me.lblTargetPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblTargetPilot.TabIndex = 11
        Me.lblTargetPilot.Text = "Pilot:"
        '
        'cboTargetPilot
        '
        Me.cboTargetPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTargetPilot.FormattingEnabled = True
        Me.cboTargetPilot.Location = New System.Drawing.Point(321, 33)
        Me.cboTargetPilot.Name = "cboTargetPilot"
        Me.cboTargetPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboTargetPilot.TabIndex = 12
        '
        'lblTurretStats
        '
        Me.lblTurretStats.AutoSize = True
        Me.lblTurretStats.Location = New System.Drawing.Point(6, 6)
        Me.lblTurretStats.Name = "lblTurretStats"
        Me.lblTurretStats.Size = New System.Drawing.Size(166, 13)
        Me.lblTurretStats.TabIndex = 15
        Me.lblTurretStats.Text = "Stats: Fittings and Pilots required"
        '
        'nudVel
        '
        Me.nudVel.BackColor = System.Drawing.SystemColors.Window
        Me.nudVel.DecimalPlaces = 2
        Me.nudVel.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudVel.Location = New System.Drawing.Point(302, 360)
        Me.nudVel.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
        Me.nudVel.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudVel.Name = "nudVel"
        Me.nudVel.ReadOnly = True
        Me.nudVel.Size = New System.Drawing.Size(54, 21)
        Me.nudVel.TabIndex = 19
        Me.nudVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudVel.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(243, 362)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Vel Scale:"
        '
        'nudRange
        '
        Me.nudRange.DecimalPlaces = 5
        Me.nudRange.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudRange.Location = New System.Drawing.Point(82, 360)
        Me.nudRange.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudRange.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudRange.Name = "nudRange"
        Me.nudRange.Size = New System.Drawing.Size(75, 21)
        Me.nudRange.TabIndex = 17
        Me.nudRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudRange.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'lblRange
        '
        Me.lblRange.AutoSize = True
        Me.lblRange.Location = New System.Drawing.Point(6, 364)
        Me.lblRange.Name = "lblRange"
        Me.lblRange.Size = New System.Drawing.Size(70, 13)
        Me.lblRange.TabIndex = 16
        Me.lblRange.Text = "Range Scale:"
        '
        'btnRangeVSHitChance
        '
        Me.btnRangeVSHitChance.Location = New System.Drawing.Point(6, 454)
        Me.btnRangeVSHitChance.Name = "btnRangeVSHitChance"
        Me.btnRangeVSHitChance.Size = New System.Drawing.Size(167, 23)
        Me.btnRangeVSHitChance.TabIndex = 20
        Me.btnRangeVSHitChance.Text = "Range vs Hit Chance"
        Me.btnRangeVSHitChance.UseVisualStyleBackColor = True
        '
        'btnOptimalRange
        '
        Me.btnOptimalRange.Location = New System.Drawing.Point(163, 360)
        Me.btnOptimalRange.Name = "btnOptimalRange"
        Me.btnOptimalRange.Size = New System.Drawing.Size(60, 21)
        Me.btnOptimalRange.TabIndex = 21
        Me.btnOptimalRange.Text = "Optimal"
        Me.btnOptimalRange.UseVisualStyleBackColor = True
        '
        'tabStats
        '
        Me.tabStats.Controls.Add(Me.tabTurretStats)
        Me.tabStats.Controls.Add(Me.tabMissileStats)
        Me.tabStats.Location = New System.Drawing.Point(387, 60)
        Me.tabStats.Name = "tabStats"
        Me.tabStats.SelectedIndex = 0
        Me.tabStats.Size = New System.Drawing.Size(452, 506)
        Me.tabStats.TabIndex = 23
        '
        'tabTurretStats
        '
        Me.tabTurretStats.Controls.Add(Me.lblTurretStats)
        Me.tabTurretStats.Location = New System.Drawing.Point(4, 22)
        Me.tabTurretStats.Name = "tabTurretStats"
        Me.tabTurretStats.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTurretStats.Size = New System.Drawing.Size(444, 480)
        Me.tabTurretStats.TabIndex = 0
        Me.tabTurretStats.Text = "Turret Stats"
        Me.tabTurretStats.UseVisualStyleBackColor = True
        '
        'tabMissileStats
        '
        Me.tabMissileStats.Controls.Add(Me.lblMissileStats)
        Me.tabMissileStats.Location = New System.Drawing.Point(4, 22)
        Me.tabMissileStats.Name = "tabMissileStats"
        Me.tabMissileStats.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMissileStats.Size = New System.Drawing.Size(444, 480)
        Me.tabMissileStats.TabIndex = 1
        Me.tabMissileStats.Text = "Missile Stats"
        Me.tabMissileStats.UseVisualStyleBackColor = True
        '
        'lblMissileStats
        '
        Me.lblMissileStats.AutoSize = True
        Me.lblMissileStats.Location = New System.Drawing.Point(6, 6)
        Me.lblMissileStats.Name = "lblMissileStats"
        Me.lblMissileStats.Size = New System.Drawing.Size(166, 13)
        Me.lblMissileStats.TabIndex = 16
        Me.lblMissileStats.Text = "Stats: Fittings and Pilots required"
        '
        'tabTurretMethod
        '
        Me.tabTurretMethod.Controls.Add(Me.tabShips)
        Me.tabTurretMethod.Location = New System.Drawing.Point(15, 60)
        Me.tabTurretMethod.Name = "tabTurretMethod"
        Me.tabTurretMethod.SelectedIndex = 0
        Me.tabTurretMethod.Size = New System.Drawing.Size(370, 506)
        Me.tabTurretMethod.TabIndex = 22
        '
        'tabShips
        '
        Me.tabShips.Controls.Add(Me.nudTargetVel)
        Me.tabShips.Controls.Add(Me.Label3)
        Me.tabShips.Controls.Add(Me.EveSpace1)
        Me.tabShips.Controls.Add(Me.lblRange)
        Me.tabShips.Controls.Add(Me.btnOptimalRange)
        Me.tabShips.Controls.Add(Me.Label2)
        Me.tabShips.Controls.Add(Me.nudRange)
        Me.tabShips.Controls.Add(Me.radMovableVel)
        Me.tabShips.Controls.Add(Me.nudAttackerVel)
        Me.tabShips.Controls.Add(Me.Label1)
        Me.tabShips.Controls.Add(Me.radManualVelocity)
        Me.tabShips.Controls.Add(Me.btnRangeVSHitChance)
        Me.tabShips.Controls.Add(Me.nudVel)
        Me.tabShips.Location = New System.Drawing.Point(4, 22)
        Me.tabShips.Name = "tabShips"
        Me.tabShips.Padding = New System.Windows.Forms.Padding(3)
        Me.tabShips.Size = New System.Drawing.Size(362, 480)
        Me.tabShips.TabIndex = 0
        Me.tabShips.Text = "Ship Placement"
        Me.tabShips.UseVisualStyleBackColor = True
        '
        'nudTargetVel
        '
        Me.nudTargetVel.DecimalPlaces = 2
        Me.nudTargetVel.Location = New System.Drawing.Point(274, 421)
        Me.nudTargetVel.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudTargetVel.Name = "nudTargetVel"
        Me.nudTargetVel.Size = New System.Drawing.Size(75, 21)
        Me.nudTargetVel.TabIndex = 27
        Me.nudTargetVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(222, 423)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "Target -"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(80, 423)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Attacker -"
        '
        'radMovableVel
        '
        Me.radMovableVel.AutoSize = True
        Me.radMovableVel.Checked = True
        Me.radMovableVel.Location = New System.Drawing.Point(6, 387)
        Me.radMovableVel.Name = "radMovableVel"
        Me.radMovableVel.Size = New System.Drawing.Size(129, 17)
        Me.radMovableVel.TabIndex = 22
        Me.radMovableVel.TabStop = True
        Me.radMovableVel.Text = "Use Icons for Velocity"
        Me.radMovableVel.UseVisualStyleBackColor = True
        '
        'nudAttackerVel
        '
        Me.nudAttackerVel.DecimalPlaces = 2
        Me.nudAttackerVel.Location = New System.Drawing.Point(141, 421)
        Me.nudAttackerVel.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudAttackerVel.Name = "nudAttackerVel"
        Me.nudAttackerVel.Size = New System.Drawing.Size(75, 21)
        Me.nudAttackerVel.TabIndex = 25
        Me.nudAttackerVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'radManualVelocity
        '
        Me.radManualVelocity.AutoSize = True
        Me.radManualVelocity.Location = New System.Drawing.Point(6, 403)
        Me.radManualVelocity.Name = "radManualVelocity"
        Me.radManualVelocity.Size = New System.Drawing.Size(99, 17)
        Me.radManualVelocity.TabIndex = 23
        Me.radManualVelocity.Text = "Manual Velocity"
        Me.radManualVelocity.UseVisualStyleBackColor = True
        '
        'EveSpace1
        '
        Me.EveSpace1.BackColor = System.Drawing.Color.Black
        Me.EveSpace1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EveSpace1.Location = New System.Drawing.Point(6, 6)
        Me.EveSpace1.Name = "EveSpace1"
        Me.EveSpace1.RangeScale = 1
        Me.EveSpace1.Size = New System.Drawing.Size(350, 350)
        Me.EveSpace1.SourceShip = Nothing
        Me.EveSpace1.TabIndex = 14
        Me.EveSpace1.TargetShip = Nothing
        Me.EveSpace1.UseIntegratedVelocity = False
        Me.EveSpace1.VelocityScale = 1
        '
        'frmDamageAnalysis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(840, 570)
        Me.Controls.Add(Me.cboTargetPilot)
        Me.Controls.Add(Me.lblAttacker)
        Me.Controls.Add(Me.tabStats)
        Me.Controls.Add(Me.lblTarget)
        Me.Controls.Add(Me.tabTurretMethod)
        Me.Controls.Add(Me.cboTargetFitting)
        Me.Controls.Add(Me.lblTargetFitting)
        Me.Controls.Add(Me.cboAttackerPilot)
        Me.Controls.Add(Me.lblTargetPilot)
        Me.Controls.Add(Me.lblAttackerPilot)
        Me.Controls.Add(Me.lblAttackerFitting)
        Me.Controls.Add(Me.cboAttackerFitting)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDamageAnalysis"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HQF Damage Analysis"
        CType(Me.nudVel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRange, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabStats.ResumeLayout(False)
        Me.tabTurretStats.ResumeLayout(False)
        Me.tabTurretStats.PerformLayout()
        Me.tabMissileStats.ResumeLayout(False)
        Me.tabMissileStats.PerformLayout()
        Me.tabTurretMethod.ResumeLayout(False)
        Me.tabShips.ResumeLayout(False)
        Me.tabShips.PerformLayout()
        CType(Me.nudTargetVel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAttackerVel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboAttackerFitting As System.Windows.Forms.ComboBox
    Friend WithEvents lblAttackerFitting As System.Windows.Forms.Label
    Friend WithEvents lblAttackerPilot As System.Windows.Forms.Label
    Friend WithEvents cboAttackerPilot As System.Windows.Forms.ComboBox
    Friend WithEvents lblAttacker As System.Windows.Forms.Label
    Friend WithEvents lblTarget As System.Windows.Forms.Label
    Friend WithEvents cboTargetFitting As System.Windows.Forms.ComboBox
    Friend WithEvents lblTargetFitting As System.Windows.Forms.Label
    Friend WithEvents lblTargetPilot As System.Windows.Forms.Label
    Friend WithEvents cboTargetPilot As System.Windows.Forms.ComboBox
    Friend WithEvents EveSpace1 As EveHQ.HQF.EveSpace
    Friend WithEvents lblTurretStats As System.Windows.Forms.Label
    Friend WithEvents nudVel As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudRange As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblRange As System.Windows.Forms.Label
    Friend WithEvents btnRangeVSHitChance As System.Windows.Forms.Button
    Friend WithEvents btnOptimalRange As System.Windows.Forms.Button
    Friend WithEvents tabTurretMethod As System.Windows.Forms.TabControl
    Friend WithEvents tabShips As System.Windows.Forms.TabPage
    Friend WithEvents tabStats As System.Windows.Forms.TabControl
    Friend WithEvents tabTurretStats As System.Windows.Forms.TabPage
    Friend WithEvents tabMissileStats As System.Windows.Forms.TabPage
    Friend WithEvents lblMissileStats As System.Windows.Forms.Label
    Friend WithEvents radManualVelocity As System.Windows.Forms.RadioButton
    Friend WithEvents radMovableVel As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudAttackerVel As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudTargetVel As System.Windows.Forms.NumericUpDown
End Class
