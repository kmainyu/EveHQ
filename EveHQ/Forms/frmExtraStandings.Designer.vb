Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmExtraStandings
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmExtraStandings))
            Me.lblCurrentStandingLbl = New System.Windows.Forms.Label()
            Me.lblRequiredStanding = New System.Windows.Forms.Label()
            Me.nudReqStanding = New System.Windows.Forms.NumericUpDown()
            Me.nudMissionGain = New System.Windows.Forms.NumericUpDown()
            Me.lblAvgMission = New System.Windows.Forms.Label()
            Me.lblCurrentStanding = New System.Windows.Forms.Label()
            Me.lblMissionsRequired = New System.Windows.Forms.Label()
            Me.lblMissionGainType = New System.Windows.Forms.Label()
            Me.radDirect = New System.Windows.Forms.RadioButton()
            Me.radCalculated = New System.Windows.Forms.RadioButton()
            Me.txtGains = New System.Windows.Forms.TextBox()
            Me.lblGainAverage = New System.Windows.Forms.Label()
            Me.lblStandingProgression = New System.Windows.Forms.Label()
            Me.lvwStandings = New System.Windows.Forms.ListView()
            Me.colNo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.colStartStanding = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.colStandingGain = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.colEndStanding = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chkUseBaseOnly = New System.Windows.Forms.CheckBox()
            Me.lblCurrentBaseStanding = New System.Windows.Forms.Label()
            Me.lblCurrentBaseStandingLbl = New System.Windows.Forms.Label()
            Me.panelStandings = New DevComponents.DotNetBar.PanelEx()
            CType(Me.nudReqStanding, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudMissionGain, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.panelStandings.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblCurrentStandingLbl
            '
            Me.lblCurrentStandingLbl.AutoSize = True
            Me.lblCurrentStandingLbl.Location = New System.Drawing.Point(13, 28)
            Me.lblCurrentStandingLbl.Name = "lblCurrentStandingLbl"
            Me.lblCurrentStandingLbl.Size = New System.Drawing.Size(93, 13)
            Me.lblCurrentStandingLbl.TabIndex = 0
            Me.lblCurrentStandingLbl.Text = "Current Standing:"
            '
            'lblRequiredStanding
            '
            Me.lblRequiredStanding.AutoSize = True
            Me.lblRequiredStanding.Location = New System.Drawing.Point(13, 48)
            Me.lblRequiredStanding.Name = "lblRequiredStanding"
            Me.lblRequiredStanding.Size = New System.Drawing.Size(99, 13)
            Me.lblRequiredStanding.TabIndex = 1
            Me.lblRequiredStanding.Text = "Required Standing:"
            '
            'nudReqStanding
            '
            Me.nudReqStanding.DecimalPlaces = 3
            Me.nudReqStanding.Location = New System.Drawing.Point(117, 46)
            Me.nudReqStanding.Maximum = New Decimal(New Integer() {9999, 0, 0, 196608})
            Me.nudReqStanding.Name = "nudReqStanding"
            Me.nudReqStanding.Size = New System.Drawing.Size(73, 21)
            Me.nudReqStanding.TabIndex = 2
            Me.nudReqStanding.Value = New Decimal(New Integer() {9, 0, 0, 0})
            '
            'nudMissionGain
            '
            Me.nudMissionGain.DecimalPlaces = 3
            Me.nudMissionGain.Location = New System.Drawing.Point(164, 141)
            Me.nudMissionGain.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
            Me.nudMissionGain.Name = "nudMissionGain"
            Me.nudMissionGain.Size = New System.Drawing.Size(73, 21)
            Me.nudMissionGain.TabIndex = 4
            Me.nudMissionGain.Value = New Decimal(New Integer() {5, 0, 0, 0})
            '
            'lblAvgMission
            '
            Me.lblAvgMission.AutoSize = True
            Me.lblAvgMission.Location = New System.Drawing.Point(43, 144)
            Me.lblAvgMission.Name = "lblAvgMission"
            Me.lblAvgMission.Size = New System.Drawing.Size(115, 13)
            Me.lblAvgMission.TabIndex = 3
            Me.lblAvgMission.Text = "Average Increase (%)"
            '
            'lblCurrentStanding
            '
            Me.lblCurrentStanding.AutoSize = True
            Me.lblCurrentStanding.Location = New System.Drawing.Point(130, 28)
            Me.lblCurrentStanding.Name = "lblCurrentStanding"
            Me.lblCurrentStanding.Size = New System.Drawing.Size(29, 13)
            Me.lblCurrentStanding.TabIndex = 6
            Me.lblCurrentStanding.Text = "0.00"
            '
            'lblMissionsRequired
            '
            Me.lblMissionsRequired.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblMissionsRequired.AutoSize = True
            Me.lblMissionsRequired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblMissionsRequired.Location = New System.Drawing.Point(13, 509)
            Me.lblMissionsRequired.Name = "lblMissionsRequired"
            Me.lblMissionsRequired.Size = New System.Drawing.Size(114, 13)
            Me.lblMissionsRequired.TabIndex = 7
            Me.lblMissionsRequired.Text = "Missions Required:"
            '
            'lblMissionGainType
            '
            Me.lblMissionGainType.AutoSize = True
            Me.lblMissionGainType.Location = New System.Drawing.Point(13, 103)
            Me.lblMissionGainType.Name = "lblMissionGainType"
            Me.lblMissionGainType.Size = New System.Drawing.Size(124, 13)
            Me.lblMissionGainType.TabIndex = 9
            Me.lblMissionGainType.Text = "Mission Gain Calculation:"
            '
            'radDirect
            '
            Me.radDirect.AutoSize = True
            Me.radDirect.Checked = True
            Me.radDirect.Location = New System.Drawing.Point(22, 122)
            Me.radDirect.Name = "radDirect"
            Me.radDirect.Size = New System.Drawing.Size(95, 17)
            Me.radDirect.TabIndex = 10
            Me.radDirect.TabStop = True
            Me.radDirect.Text = "Input Average"
            Me.radDirect.UseVisualStyleBackColor = True
            '
            'radCalculated
            '
            Me.radCalculated.AutoSize = True
            Me.radCalculated.Location = New System.Drawing.Point(22, 160)
            Me.radCalculated.Name = "radCalculated"
            Me.radCalculated.Size = New System.Drawing.Size(119, 17)
            Me.radCalculated.TabIndex = 11
            Me.radCalculated.Text = "Calculated Average"
            Me.radCalculated.UseVisualStyleBackColor = True
            '
            'txtGains
            '
            Me.txtGains.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.txtGains.Location = New System.Drawing.Point(53, 183)
            Me.txtGains.Multiline = True
            Me.txtGains.Name = "txtGains"
            Me.txtGains.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtGains.Size = New System.Drawing.Size(184, 296)
            Me.txtGains.TabIndex = 12
            '
            'lblGainAverage
            '
            Me.lblGainAverage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblGainAverage.AutoSize = True
            Me.lblGainAverage.Location = New System.Drawing.Point(50, 482)
            Me.lblGainAverage.Name = "lblGainAverage"
            Me.lblGainAverage.Size = New System.Drawing.Size(52, 13)
            Me.lblGainAverage.TabIndex = 13
            Me.lblGainAverage.Text = "Average:"
            '
            'lblStandingProgression
            '
            Me.lblStandingProgression.AutoSize = True
            Me.lblStandingProgression.Location = New System.Drawing.Point(257, 12)
            Me.lblStandingProgression.Name = "lblStandingProgression"
            Me.lblStandingProgression.Size = New System.Drawing.Size(117, 13)
            Me.lblStandingProgression.TabIndex = 14
            Me.lblStandingProgression.Text = "Standings Progression:"
            '
            'lvwStandings
            '
            Me.lvwStandings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                             Or System.Windows.Forms.AnchorStyles.Left) _
                                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvwStandings.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colNo, Me.colStartStanding, Me.colStandingGain, Me.colEndStanding})
            Me.lvwStandings.FullRowSelect = True
            Me.lvwStandings.GridLines = True
            Me.lvwStandings.Location = New System.Drawing.Point(260, 28)
            Me.lvwStandings.Name = "lvwStandings"
            Me.lvwStandings.Size = New System.Drawing.Size(393, 494)
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
            'chkUseBaseOnly
            '
            Me.chkUseBaseOnly.AutoSize = True
            Me.chkUseBaseOnly.Location = New System.Drawing.Point(70, 72)
            Me.chkUseBaseOnly.Name = "chkUseBaseOnly"
            Me.chkUseBaseOnly.Size = New System.Drawing.Size(119, 17)
            Me.chkUseBaseOnly.TabIndex = 16
            Me.chkUseBaseOnly.Text = "Use Base standings"
            Me.chkUseBaseOnly.UseVisualStyleBackColor = True
            '
            'lblCurrentBaseStanding
            '
            Me.lblCurrentBaseStanding.AutoSize = True
            Me.lblCurrentBaseStanding.Location = New System.Drawing.Point(130, 12)
            Me.lblCurrentBaseStanding.Name = "lblCurrentBaseStanding"
            Me.lblCurrentBaseStanding.Size = New System.Drawing.Size(29, 13)
            Me.lblCurrentBaseStanding.TabIndex = 18
            Me.lblCurrentBaseStanding.Text = "0.00"
            '
            'lblCurrentBaseStandingLbl
            '
            Me.lblCurrentBaseStandingLbl.AutoSize = True
            Me.lblCurrentBaseStandingLbl.Location = New System.Drawing.Point(13, 12)
            Me.lblCurrentBaseStandingLbl.Name = "lblCurrentBaseStandingLbl"
            Me.lblCurrentBaseStandingLbl.Size = New System.Drawing.Size(119, 13)
            Me.lblCurrentBaseStandingLbl.TabIndex = 17
            Me.lblCurrentBaseStandingLbl.Text = "Current Base Standing:"
            '
            'panelStandings
            '
            Me.panelStandings.CanvasColor = System.Drawing.SystemColors.Control
            Me.panelStandings.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.panelStandings.Controls.Add(Me.lblCurrentBaseStandingLbl)
            Me.panelStandings.Controls.Add(Me.lblCurrentBaseStanding)
            Me.panelStandings.Controls.Add(Me.lblCurrentStandingLbl)
            Me.panelStandings.Controls.Add(Me.lblRequiredStanding)
            Me.panelStandings.Controls.Add(Me.chkUseBaseOnly)
            Me.panelStandings.Controls.Add(Me.nudReqStanding)
            Me.panelStandings.Controls.Add(Me.lvwStandings)
            Me.panelStandings.Controls.Add(Me.lblAvgMission)
            Me.panelStandings.Controls.Add(Me.lblStandingProgression)
            Me.panelStandings.Controls.Add(Me.nudMissionGain)
            Me.panelStandings.Controls.Add(Me.lblGainAverage)
            Me.panelStandings.Controls.Add(Me.lblCurrentStanding)
            Me.panelStandings.Controls.Add(Me.txtGains)
            Me.panelStandings.Controls.Add(Me.lblMissionsRequired)
            Me.panelStandings.Controls.Add(Me.radCalculated)
            Me.panelStandings.Controls.Add(Me.radDirect)
            Me.panelStandings.Controls.Add(Me.lblMissionGainType)
            Me.panelStandings.Dock = System.Windows.Forms.DockStyle.Fill
            Me.panelStandings.Location = New System.Drawing.Point(0, 0)
            Me.panelStandings.Name = "panelStandings"
            Me.panelStandings.Size = New System.Drawing.Size(661, 531)
            Me.panelStandings.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.panelStandings.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.panelStandings.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.panelStandings.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.panelStandings.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.panelStandings.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.panelStandings.Style.GradientAngle = 90
            Me.panelStandings.TabIndex = 19
            '
            'frmExtraStandings
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(661, 531)
            Me.Controls.Add(Me.panelStandings)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmExtraStandings"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Standings Extrapolation"
            CType(Me.nudReqStanding, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudMissionGain, System.ComponentModel.ISupportInitialize).EndInit()
            Me.panelStandings.ResumeLayout(False)
            Me.panelStandings.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblCurrentStandingLbl As System.Windows.Forms.Label
        Friend WithEvents lblRequiredStanding As System.Windows.Forms.Label
        Friend WithEvents nudReqStanding As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudMissionGain As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblAvgMission As System.Windows.Forms.Label
        Friend WithEvents lblCurrentStanding As System.Windows.Forms.Label
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
        Friend WithEvents chkUseBaseOnly As System.Windows.Forms.CheckBox
        Friend WithEvents lblCurrentBaseStanding As System.Windows.Forms.Label
        Friend WithEvents lblCurrentBaseStandingLbl As System.Windows.Forms.Label
        Friend WithEvents panelStandings As DevComponents.DotNetBar.PanelEx
    End Class
End NameSpace