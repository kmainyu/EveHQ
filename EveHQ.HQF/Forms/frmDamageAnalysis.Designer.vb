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
        Me.lblStats = New System.Windows.Forms.Label
        Me.nudVel = New System.Windows.Forms.NumericUpDown
        Me.Label1 = New System.Windows.Forms.Label
        Me.nudRange = New System.Windows.Forms.NumericUpDown
        Me.lblRange = New System.Windows.Forms.Label
        Me.btnRangeVSHitChance = New System.Windows.Forms.Button
        Me.btnOptimalRange = New System.Windows.Forms.Button
        Me.EveSpace1 = New EveHQ.HQF.EveSpace
        Me.tabDamage = New System.Windows.Forms.TabControl
        Me.tabTurrets = New System.Windows.Forms.TabPage
        Me.tabMissiles = New System.Windows.Forms.TabPage
        CType(Me.nudVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDamage.SuspendLayout()
        Me.tabTurrets.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboAttackerFitting
        '
        Me.cboAttackerFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAttackerFitting.FormattingEnabled = True
        Me.cboAttackerFitting.Location = New System.Drawing.Point(111, 10)
        Me.cboAttackerFitting.Name = "cboAttackerFitting"
        Me.cboAttackerFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboAttackerFitting.TabIndex = 5
        '
        'lblAttackerFitting
        '
        Me.lblAttackerFitting.AutoSize = True
        Me.lblAttackerFitting.Location = New System.Drawing.Point(67, 13)
        Me.lblAttackerFitting.Name = "lblAttackerFitting"
        Me.lblAttackerFitting.Size = New System.Drawing.Size(41, 13)
        Me.lblAttackerFitting.TabIndex = 4
        Me.lblAttackerFitting.Text = "Fitting:"
        '
        'lblAttackerPilot
        '
        Me.lblAttackerPilot.AutoSize = True
        Me.lblAttackerPilot.Location = New System.Drawing.Point(304, 13)
        Me.lblAttackerPilot.Name = "lblAttackerPilot"
        Me.lblAttackerPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblAttackerPilot.TabIndex = 6
        Me.lblAttackerPilot.Text = "Pilot:"
        '
        'cboAttackerPilot
        '
        Me.cboAttackerPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAttackerPilot.FormattingEnabled = True
        Me.cboAttackerPilot.Location = New System.Drawing.Point(340, 10)
        Me.cboAttackerPilot.Name = "cboAttackerPilot"
        Me.cboAttackerPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboAttackerPilot.TabIndex = 7
        '
        'lblAttacker
        '
        Me.lblAttacker.AutoSize = True
        Me.lblAttacker.Location = New System.Drawing.Point(6, 13)
        Me.lblAttacker.Name = "lblAttacker"
        Me.lblAttacker.Size = New System.Drawing.Size(55, 13)
        Me.lblAttacker.TabIndex = 8
        Me.lblAttacker.Text = "Attacker -"
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.Location = New System.Drawing.Point(6, 40)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(46, 13)
        Me.lblTarget.TabIndex = 13
        Me.lblTarget.Text = "Target -"
        '
        'cboTargetFitting
        '
        Me.cboTargetFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTargetFitting.FormattingEnabled = True
        Me.cboTargetFitting.Location = New System.Drawing.Point(111, 37)
        Me.cboTargetFitting.Name = "cboTargetFitting"
        Me.cboTargetFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboTargetFitting.TabIndex = 10
        '
        'lblTargetFitting
        '
        Me.lblTargetFitting.AutoSize = True
        Me.lblTargetFitting.Location = New System.Drawing.Point(67, 40)
        Me.lblTargetFitting.Name = "lblTargetFitting"
        Me.lblTargetFitting.Size = New System.Drawing.Size(41, 13)
        Me.lblTargetFitting.TabIndex = 9
        Me.lblTargetFitting.Text = "Fitting:"
        '
        'lblTargetPilot
        '
        Me.lblTargetPilot.AutoSize = True
        Me.lblTargetPilot.Location = New System.Drawing.Point(304, 40)
        Me.lblTargetPilot.Name = "lblTargetPilot"
        Me.lblTargetPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblTargetPilot.TabIndex = 11
        Me.lblTargetPilot.Text = "Pilot:"
        '
        'cboTargetPilot
        '
        Me.cboTargetPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTargetPilot.FormattingEnabled = True
        Me.cboTargetPilot.Location = New System.Drawing.Point(340, 37)
        Me.cboTargetPilot.Name = "cboTargetPilot"
        Me.cboTargetPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboTargetPilot.TabIndex = 12
        '
        'lblStats
        '
        Me.lblStats.AutoSize = True
        Me.lblStats.Location = New System.Drawing.Point(413, 74)
        Me.lblStats.Name = "lblStats"
        Me.lblStats.Size = New System.Drawing.Size(166, 13)
        Me.lblStats.TabIndex = 15
        Me.lblStats.Text = "Stats: Fittings and Pilots required"
        '
        'nudVel
        '
        Me.nudVel.DecimalPlaces = 2
        Me.nudVel.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudVel.Location = New System.Drawing.Point(352, 480)
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
        Me.Label1.Location = New System.Drawing.Point(293, 482)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Vel Scale:"
        '
        'nudRange
        '
        Me.nudRange.DecimalPlaces = 5
        Me.nudRange.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudRange.Location = New System.Drawing.Point(83, 480)
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
        Me.lblRange.Location = New System.Drawing.Point(7, 482)
        Me.lblRange.Name = "lblRange"
        Me.lblRange.Size = New System.Drawing.Size(70, 13)
        Me.lblRange.TabIndex = 16
        Me.lblRange.Text = "Range Scale:"
        '
        'btnRangeVSHitChance
        '
        Me.btnRangeVSHitChance.Location = New System.Drawing.Point(6, 522)
        Me.btnRangeVSHitChance.Name = "btnRangeVSHitChance"
        Me.btnRangeVSHitChance.Size = New System.Drawing.Size(167, 23)
        Me.btnRangeVSHitChance.TabIndex = 20
        Me.btnRangeVSHitChance.Text = "Range vs Hit Chance"
        Me.btnRangeVSHitChance.UseVisualStyleBackColor = True
        '
        'btnOptimalRange
        '
        Me.btnOptimalRange.Location = New System.Drawing.Point(164, 480)
        Me.btnOptimalRange.Name = "btnOptimalRange"
        Me.btnOptimalRange.Size = New System.Drawing.Size(60, 21)
        Me.btnOptimalRange.TabIndex = 21
        Me.btnOptimalRange.Text = "Optimal"
        Me.btnOptimalRange.UseVisualStyleBackColor = True
        '
        'EveSpace1
        '
        Me.EveSpace1.BackColor = System.Drawing.Color.Black
        Me.EveSpace1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EveSpace1.Location = New System.Drawing.Point(6, 74)
        Me.EveSpace1.Name = "EveSpace1"
        Me.EveSpace1.RangeScale = 1
        Me.EveSpace1.Size = New System.Drawing.Size(400, 400)
        Me.EveSpace1.SourceShip = Nothing
        Me.EveSpace1.TabIndex = 14
        Me.EveSpace1.TargetShip = Nothing
        Me.EveSpace1.VelocityScale = 1
        '
        'tabDamage
        '
        Me.tabDamage.Controls.Add(Me.tabTurrets)
        Me.tabDamage.Controls.Add(Me.tabMissiles)
        Me.tabDamage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabDamage.Location = New System.Drawing.Point(0, 0)
        Me.tabDamage.Name = "tabDamage"
        Me.tabDamage.SelectedIndex = 0
        Me.tabDamage.Size = New System.Drawing.Size(818, 590)
        Me.tabDamage.TabIndex = 22
        '
        'tabTurrets
        '
        Me.tabTurrets.Controls.Add(Me.lblAttacker)
        Me.tabTurrets.Controls.Add(Me.btnOptimalRange)
        Me.tabTurrets.Controls.Add(Me.cboAttackerPilot)
        Me.tabTurrets.Controls.Add(Me.btnRangeVSHitChance)
        Me.tabTurrets.Controls.Add(Me.lblAttackerPilot)
        Me.tabTurrets.Controls.Add(Me.nudVel)
        Me.tabTurrets.Controls.Add(Me.lblAttackerFitting)
        Me.tabTurrets.Controls.Add(Me.Label1)
        Me.tabTurrets.Controls.Add(Me.cboAttackerFitting)
        Me.tabTurrets.Controls.Add(Me.nudRange)
        Me.tabTurrets.Controls.Add(Me.cboTargetPilot)
        Me.tabTurrets.Controls.Add(Me.lblRange)
        Me.tabTurrets.Controls.Add(Me.lblTargetPilot)
        Me.tabTurrets.Controls.Add(Me.lblStats)
        Me.tabTurrets.Controls.Add(Me.lblTargetFitting)
        Me.tabTurrets.Controls.Add(Me.EveSpace1)
        Me.tabTurrets.Controls.Add(Me.cboTargetFitting)
        Me.tabTurrets.Controls.Add(Me.lblTarget)
        Me.tabTurrets.Location = New System.Drawing.Point(4, 22)
        Me.tabTurrets.Name = "tabTurrets"
        Me.tabTurrets.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTurrets.Size = New System.Drawing.Size(810, 564)
        Me.tabTurrets.TabIndex = 0
        Me.tabTurrets.Text = "Turret Damage"
        Me.tabTurrets.UseVisualStyleBackColor = True
        '
        'tabMissiles
        '
        Me.tabMissiles.Location = New System.Drawing.Point(4, 22)
        Me.tabMissiles.Name = "tabMissiles"
        Me.tabMissiles.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMissiles.Size = New System.Drawing.Size(708, 591)
        Me.tabMissiles.TabIndex = 1
        Me.tabMissiles.Text = "Missile Damage"
        Me.tabMissiles.UseVisualStyleBackColor = True
        '
        'frmDamageAnalysis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(818, 590)
        Me.Controls.Add(Me.tabDamage)
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
        Me.tabDamage.ResumeLayout(False)
        Me.tabTurrets.ResumeLayout(False)
        Me.tabTurrets.PerformLayout()
        Me.ResumeLayout(False)

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
    Friend WithEvents lblStats As System.Windows.Forms.Label
    Friend WithEvents nudVel As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudRange As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblRange As System.Windows.Forms.Label
    Friend WithEvents btnRangeVSHitChance As System.Windows.Forms.Button
    Friend WithEvents btnOptimalRange As System.Windows.Forms.Button
    Friend WithEvents tabDamage As System.Windows.Forms.TabControl
    Friend WithEvents tabTurrets As System.Windows.Forms.TabPage
    Friend WithEvents tabMissiles As System.Windows.Forms.TabPage
End Class
