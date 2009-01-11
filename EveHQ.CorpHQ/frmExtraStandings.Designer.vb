<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExtraStandings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExtraStandings))
        Me.lblCurrentStandingLbl = New System.Windows.Forms.Label
        Me.lblRequiredStanding = New System.Windows.Forms.Label
        Me.nudReqStanding = New System.Windows.Forms.NumericUpDown
        Me.nudMissionGain = New System.Windows.Forms.NumericUpDown
        Me.lblAvgMission = New System.Windows.Forms.Label
        Me.lblCurrentStanding = New System.Windows.Forms.Label
        Me.lblMissionsRequiredLabel = New System.Windows.Forms.Label
        Me.lblMissionsRequired = New System.Windows.Forms.Label
        Me.lblMissionGainType = New System.Windows.Forms.Label
        Me.radDirect = New System.Windows.Forms.RadioButton
        Me.radCalculated = New System.Windows.Forms.RadioButton
        Me.txtGains = New System.Windows.Forms.TextBox
        Me.lblGainAverage = New System.Windows.Forms.Label
        Me.lblStandingProgression = New System.Windows.Forms.Label
        Me.lvwStandings = New System.Windows.Forms.ListView
        Me.colNo = New System.Windows.Forms.ColumnHeader
        Me.colStartStanding = New System.Windows.Forms.ColumnHeader
        Me.colStandingGain = New System.Windows.Forms.ColumnHeader
        Me.colEndStanding = New System.Windows.Forms.ColumnHeader
        CType(Me.nudReqStanding, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMissionGain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblCurrentStandingLbl
        '
        Me.lblCurrentStandingLbl.AutoSize = True
        Me.lblCurrentStandingLbl.Location = New System.Drawing.Point(12, 20)
        Me.lblCurrentStandingLbl.Name = "lblCurrentStandingLbl"
        Me.lblCurrentStandingLbl.Size = New System.Drawing.Size(89, 13)
        Me.lblCurrentStandingLbl.TabIndex = 0
        Me.lblCurrentStandingLbl.Text = "Current Standing:"
        '
        'lblRequiredStanding
        '
        Me.lblRequiredStanding.AutoSize = True
        Me.lblRequiredStanding.Location = New System.Drawing.Point(12, 46)
        Me.lblRequiredStanding.Name = "lblRequiredStanding"
        Me.lblRequiredStanding.Size = New System.Drawing.Size(98, 13)
        Me.lblRequiredStanding.TabIndex = 1
        Me.lblRequiredStanding.Text = "Required Standing:"
        '
        'nudReqStanding
        '
        Me.nudReqStanding.DecimalPlaces = 3
        Me.nudReqStanding.Location = New System.Drawing.Point(116, 44)
        Me.nudReqStanding.Maximum = New Decimal(New Integer() {9999, 0, 0, 196608})
        Me.nudReqStanding.Name = "nudReqStanding"
        Me.nudReqStanding.Size = New System.Drawing.Size(73, 20)
        Me.nudReqStanding.TabIndex = 2
        Me.nudReqStanding.Value = New Decimal(New Integer() {9, 0, 0, 0})
        '
        'nudMissionGain
        '
        Me.nudMissionGain.DecimalPlaces = 3
        Me.nudMissionGain.Location = New System.Drawing.Point(163, 119)
        Me.nudMissionGain.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudMissionGain.Name = "nudMissionGain"
        Me.nudMissionGain.Size = New System.Drawing.Size(73, 20)
        Me.nudMissionGain.TabIndex = 4
        Me.nudMissionGain.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblAvgMission
        '
        Me.lblAvgMission.AutoSize = True
        Me.lblAvgMission.Location = New System.Drawing.Point(49, 121)
        Me.lblAvgMission.Name = "lblAvgMission"
        Me.lblAvgMission.Size = New System.Drawing.Size(108, 13)
        Me.lblAvgMission.TabIndex = 3
        Me.lblAvgMission.Text = "Average Increase (%)"
        '
        'lblCurrentStanding
        '
        Me.lblCurrentStanding.AutoSize = True
        Me.lblCurrentStanding.Location = New System.Drawing.Point(113, 20)
        Me.lblCurrentStanding.Name = "lblCurrentStanding"
        Me.lblCurrentStanding.Size = New System.Drawing.Size(28, 13)
        Me.lblCurrentStanding.TabIndex = 6
        Me.lblCurrentStanding.Text = "0.00"
        '
        'lblMissionsRequiredLabel
        '
        Me.lblMissionsRequiredLabel.AutoSize = True
        Me.lblMissionsRequiredLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMissionsRequiredLabel.Location = New System.Drawing.Point(14, 469)
        Me.lblMissionsRequiredLabel.Name = "lblMissionsRequiredLabel"
        Me.lblMissionsRequiredLabel.Size = New System.Drawing.Size(114, 13)
        Me.lblMissionsRequiredLabel.TabIndex = 7
        Me.lblMissionsRequiredLabel.Text = "Missions Required:"
        '
        'lblMissionsRequired
        '
        Me.lblMissionsRequired.AutoSize = True
        Me.lblMissionsRequired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMissionsRequired.Location = New System.Drawing.Point(160, 469)
        Me.lblMissionsRequired.Name = "lblMissionsRequired"
        Me.lblMissionsRequired.Size = New System.Drawing.Size(0, 13)
        Me.lblMissionsRequired.TabIndex = 8
        '
        'lblMissionGainType
        '
        Me.lblMissionGainType.AutoSize = True
        Me.lblMissionGainType.Location = New System.Drawing.Point(12, 81)
        Me.lblMissionGainType.Name = "lblMissionGainType"
        Me.lblMissionGainType.Size = New System.Drawing.Size(125, 13)
        Me.lblMissionGainType.TabIndex = 9
        Me.lblMissionGainType.Text = "Mission Gain Calculation:"
        '
        'radDirect
        '
        Me.radDirect.AutoSize = True
        Me.radDirect.Checked = True
        Me.radDirect.Location = New System.Drawing.Point(21, 100)
        Me.radDirect.Name = "radDirect"
        Me.radDirect.Size = New System.Drawing.Size(92, 17)
        Me.radDirect.TabIndex = 10
        Me.radDirect.TabStop = True
        Me.radDirect.Text = "Input Average"
        Me.radDirect.UseVisualStyleBackColor = True
        '
        'radCalculated
        '
        Me.radCalculated.AutoSize = True
        Me.radCalculated.Location = New System.Drawing.Point(21, 148)
        Me.radCalculated.Name = "radCalculated"
        Me.radCalculated.Size = New System.Drawing.Size(118, 17)
        Me.radCalculated.TabIndex = 11
        Me.radCalculated.Text = "Calculated Average"
        Me.radCalculated.UseVisualStyleBackColor = True
        '
        'txtGains
        '
        Me.txtGains.Location = New System.Drawing.Point(52, 172)
        Me.txtGains.Multiline = True
        Me.txtGains.Name = "txtGains"
        Me.txtGains.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtGains.Size = New System.Drawing.Size(132, 253)
        Me.txtGains.TabIndex = 12
        '
        'lblGainAverage
        '
        Me.lblGainAverage.AutoSize = True
        Me.lblGainAverage.Location = New System.Drawing.Point(49, 428)
        Me.lblGainAverage.Name = "lblGainAverage"
        Me.lblGainAverage.Size = New System.Drawing.Size(50, 13)
        Me.lblGainAverage.TabIndex = 13
        Me.lblGainAverage.Text = "Average:"
        '
        'lblStandingProgression
        '
        Me.lblStandingProgression.AutoSize = True
        Me.lblStandingProgression.Location = New System.Drawing.Point(256, 20)
        Me.lblStandingProgression.Name = "lblStandingProgression"
        Me.lblStandingProgression.Size = New System.Drawing.Size(115, 13)
        Me.lblStandingProgression.TabIndex = 14
        Me.lblStandingProgression.Text = "Standings Progression:"
        '
        'lvwStandings
        '
        Me.lvwStandings.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colNo, Me.colStartStanding, Me.colStandingGain, Me.colEndStanding})
        Me.lvwStandings.FullRowSelect = True
        Me.lvwStandings.GridLines = True
        Me.lvwStandings.Location = New System.Drawing.Point(259, 36)
        Me.lvwStandings.Name = "lvwStandings"
        Me.lvwStandings.Size = New System.Drawing.Size(414, 461)
        Me.lvwStandings.TabIndex = 15
        Me.lvwStandings.UseCompatibleStateImageBehavior = False
        Me.lvwStandings.View = System.Windows.Forms.View.Details
        '
        'colNo
        '
        Me.colNo.Text = "No"
        Me.colNo.Width = 50
        '
        'colStartStanding
        '
        Me.colStartStanding.Text = "Starting Standing"
        Me.colStartStanding.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colStartStanding.Width = 125
        '
        'colStandingGain
        '
        Me.colStandingGain.Text = "Gain (%)"
        Me.colStandingGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'colEndStanding
        '
        Me.colEndStanding.Text = "End Standing"
        Me.colEndStanding.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colEndStanding.Width = 125
        '
        'frmExtraStandings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(678, 509)
        Me.Controls.Add(Me.lvwStandings)
        Me.Controls.Add(Me.lblStandingProgression)
        Me.Controls.Add(Me.lblGainAverage)
        Me.Controls.Add(Me.txtGains)
        Me.Controls.Add(Me.radCalculated)
        Me.Controls.Add(Me.radDirect)
        Me.Controls.Add(Me.lblMissionGainType)
        Me.Controls.Add(Me.lblMissionsRequired)
        Me.Controls.Add(Me.lblMissionsRequiredLabel)
        Me.Controls.Add(Me.lblCurrentStanding)
        Me.Controls.Add(Me.nudMissionGain)
        Me.Controls.Add(Me.lblAvgMission)
        Me.Controls.Add(Me.nudReqStanding)
        Me.Controls.Add(Me.lblRequiredStanding)
        Me.Controls.Add(Me.lblCurrentStandingLbl)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmExtraStandings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Standings Extrapolation"
        CType(Me.nudReqStanding, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMissionGain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCurrentStandingLbl As System.Windows.Forms.Label
    Friend WithEvents lblRequiredStanding As System.Windows.Forms.Label
    Friend WithEvents nudReqStanding As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudMissionGain As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblAvgMission As System.Windows.Forms.Label
    Friend WithEvents lblCurrentStanding As System.Windows.Forms.Label
    Friend WithEvents lblMissionsRequiredLabel As System.Windows.Forms.Label
    Friend WithEvents lblMissionsRequired As System.Windows.Forms.Label
    Friend WithEvents lblMissionGainType As System.Windows.Forms.Label
    Friend WithEvents radDirect As System.Windows.Forms.RadioButton
    Friend WithEvents radCalculated As System.Windows.Forms.RadioButton
    Friend WithEvents txtGains As System.Windows.Forms.TextBox
    Friend WithEvents lblGainAverage As System.Windows.Forms.Label
    Friend WithEvents lblStandingProgression As System.Windows.Forms.Label
    Friend WithEvents lvwStandings As System.Windows.Forms.ListView
    Friend WithEvents colNo As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStartStanding As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStandingGain As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEndStanding As System.Windows.Forms.ColumnHeader
End Class
