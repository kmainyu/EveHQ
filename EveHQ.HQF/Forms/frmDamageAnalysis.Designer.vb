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
        Me.EveSpace1 = New EveHQ.HQF.EveSpace
        CType(Me.nudVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboAttackerFitting
        '
        Me.cboAttackerFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAttackerFitting.FormattingEnabled = True
        Me.cboAttackerFitting.Location = New System.Drawing.Point(117, 12)
        Me.cboAttackerFitting.Name = "cboAttackerFitting"
        Me.cboAttackerFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboAttackerFitting.TabIndex = 5
        '
        'lblAttackerFitting
        '
        Me.lblAttackerFitting.AutoSize = True
        Me.lblAttackerFitting.Location = New System.Drawing.Point(73, 15)
        Me.lblAttackerFitting.Name = "lblAttackerFitting"
        Me.lblAttackerFitting.Size = New System.Drawing.Size(41, 13)
        Me.lblAttackerFitting.TabIndex = 4
        Me.lblAttackerFitting.Text = "Fitting:"
        '
        'lblAttackerPilot
        '
        Me.lblAttackerPilot.AutoSize = True
        Me.lblAttackerPilot.Location = New System.Drawing.Point(310, 15)
        Me.lblAttackerPilot.Name = "lblAttackerPilot"
        Me.lblAttackerPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblAttackerPilot.TabIndex = 6
        Me.lblAttackerPilot.Text = "Pilot:"
        '
        'cboAttackerPilot
        '
        Me.cboAttackerPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAttackerPilot.FormattingEnabled = True
        Me.cboAttackerPilot.Location = New System.Drawing.Point(346, 12)
        Me.cboAttackerPilot.Name = "cboAttackerPilot"
        Me.cboAttackerPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboAttackerPilot.TabIndex = 7
        '
        'lblAttacker
        '
        Me.lblAttacker.AutoSize = True
        Me.lblAttacker.Location = New System.Drawing.Point(12, 15)
        Me.lblAttacker.Name = "lblAttacker"
        Me.lblAttacker.Size = New System.Drawing.Size(55, 13)
        Me.lblAttacker.TabIndex = 8
        Me.lblAttacker.Text = "Attacker -"
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.Location = New System.Drawing.Point(12, 42)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(46, 13)
        Me.lblTarget.TabIndex = 13
        Me.lblTarget.Text = "Target -"
        '
        'cboTargetFitting
        '
        Me.cboTargetFitting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTargetFitting.FormattingEnabled = True
        Me.cboTargetFitting.Location = New System.Drawing.Point(117, 39)
        Me.cboTargetFitting.Name = "cboTargetFitting"
        Me.cboTargetFitting.Size = New System.Drawing.Size(187, 21)
        Me.cboTargetFitting.TabIndex = 10
        '
        'lblTargetFitting
        '
        Me.lblTargetFitting.AutoSize = True
        Me.lblTargetFitting.Location = New System.Drawing.Point(73, 42)
        Me.lblTargetFitting.Name = "lblTargetFitting"
        Me.lblTargetFitting.Size = New System.Drawing.Size(41, 13)
        Me.lblTargetFitting.TabIndex = 9
        Me.lblTargetFitting.Text = "Fitting:"
        '
        'lblTargetPilot
        '
        Me.lblTargetPilot.AutoSize = True
        Me.lblTargetPilot.Location = New System.Drawing.Point(310, 42)
        Me.lblTargetPilot.Name = "lblTargetPilot"
        Me.lblTargetPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblTargetPilot.TabIndex = 11
        Me.lblTargetPilot.Text = "Pilot:"
        '
        'cboTargetPilot
        '
        Me.cboTargetPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTargetPilot.FormattingEnabled = True
        Me.cboTargetPilot.Location = New System.Drawing.Point(346, 39)
        Me.cboTargetPilot.Name = "cboTargetPilot"
        Me.cboTargetPilot.Size = New System.Drawing.Size(148, 21)
        Me.cboTargetPilot.TabIndex = 12
        '
        'lblStats
        '
        Me.lblStats.AutoSize = True
        Me.lblStats.Location = New System.Drawing.Point(419, 76)
        Me.lblStats.Name = "lblStats"
        Me.lblStats.Size = New System.Drawing.Size(166, 13)
        Me.lblStats.TabIndex = 15
        Me.lblStats.Text = "Stats: Fittings and Pilots required"
        '
        'nudVel
        '
        Me.nudVel.DecimalPlaces = 2
        Me.nudVel.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudVel.Location = New System.Drawing.Point(266, 482)
        Me.nudVel.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
        Me.nudVel.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudVel.Name = "nudVel"
        Me.nudVel.ReadOnly = True
        Me.nudVel.Size = New System.Drawing.Size(54, 21)
        Me.nudVel.TabIndex = 19
        Me.nudVel.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(190, 484)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Vel Scale:"
        '
        'nudRange
        '
        Me.nudRange.DecimalPlaces = 2
        Me.nudRange.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudRange.Location = New System.Drawing.Point(89, 482)
        Me.nudRange.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudRange.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudRange.Name = "nudRange"
        Me.nudRange.ReadOnly = True
        Me.nudRange.Size = New System.Drawing.Size(54, 21)
        Me.nudRange.TabIndex = 17
        Me.nudRange.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'lblRange
        '
        Me.lblRange.AutoSize = True
        Me.lblRange.Location = New System.Drawing.Point(13, 484)
        Me.lblRange.Name = "lblRange"
        Me.lblRange.Size = New System.Drawing.Size(70, 13)
        Me.lblRange.TabIndex = 16
        Me.lblRange.Text = "Range Scale:"
        '
        'btnRangeVSHitChance
        '
        Me.btnRangeVSHitChance.Location = New System.Drawing.Point(12, 509)
        Me.btnRangeVSHitChance.Name = "btnRangeVSHitChance"
        Me.btnRangeVSHitChance.Size = New System.Drawing.Size(167, 23)
        Me.btnRangeVSHitChance.TabIndex = 20
        Me.btnRangeVSHitChance.Text = "Range vs Hit Chance"
        Me.btnRangeVSHitChance.UseVisualStyleBackColor = True
        '
        'EveSpace1
        '
        Me.EveSpace1.BackColor = System.Drawing.Color.Black
        Me.EveSpace1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EveSpace1.Location = New System.Drawing.Point(12, 76)
        Me.EveSpace1.Name = "EveSpace1"
        Me.EveSpace1.RangeScale = 1
        Me.EveSpace1.Size = New System.Drawing.Size(400, 400)
        Me.EveSpace1.SourceShip = Nothing
        Me.EveSpace1.TabIndex = 14
        Me.EveSpace1.TargetShip = Nothing
        Me.EveSpace1.VelocityScale = 1
        '
        'frmDamageAnalysis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(908, 595)
        Me.Controls.Add(Me.btnRangeVSHitChance)
        Me.Controls.Add(Me.nudVel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.nudRange)
        Me.Controls.Add(Me.lblRange)
        Me.Controls.Add(Me.lblStats)
        Me.Controls.Add(Me.EveSpace1)
        Me.Controls.Add(Me.lblTarget)
        Me.Controls.Add(Me.cboTargetFitting)
        Me.Controls.Add(Me.lblTargetFitting)
        Me.Controls.Add(Me.lblTargetPilot)
        Me.Controls.Add(Me.cboTargetPilot)
        Me.Controls.Add(Me.lblAttacker)
        Me.Controls.Add(Me.cboAttackerFitting)
        Me.Controls.Add(Me.lblAttackerFitting)
        Me.Controls.Add(Me.lblAttackerPilot)
        Me.Controls.Add(Me.cboAttackerPilot)
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
    Friend WithEvents lblStats As System.Windows.Forms.Label
    Friend WithEvents nudVel As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudRange As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblRange As System.Windows.Forms.Label
    Friend WithEvents btnRangeVSHitChance As System.Windows.Forms.Button
End Class
