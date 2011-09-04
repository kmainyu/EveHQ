' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
' 
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================
Imports System.Windows.Forms

Public Class ShipInfoControl

#Region "Other Variables"
    Private reqSkillsCollection As New NeededSkillsCollection ' Lists the skills required for the ship/mods/drones
    Private StartUp As Boolean = False
#End Region

#Region "Property Variables"
    Private cParentFitting As Fitting ' Stores the fitting to which this control is attached to
#End Region

#Region "Properties"

    Public ReadOnly Property ParentFitting() As Fitting
        Get
            Return cParentFitting
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal ShipFit As Fitting)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the parent fitting
        cParentFitting = ShipFit

        ' Set the startup flag
        StartUp = True

        ' Add the current list of pilots to the combobox
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next
        cboPilots.EndUpdate()

        ' Can we get the pilot from the fitting?
        If cboPilots.Items.Contains(ShipFit.PilotName) = True Then
            cboPilots.SelectedItem = ShipFit.PilotName
        Else
            ' Look at the settings for default pilot
            If cboPilots.Items.Contains(HQF.Settings.HQFSettings.DefaultPilot) = True Then
                cboPilots.SelectedItem = HQF.Settings.HQFSettings.DefaultPilot
            Else
                cboPilots.SelectedIndex = 0
            End If
        End If

        ' Add the current list of implants to the combobox
        Call Me.UpdateImplantList()

        Call Me.LoadDamageProfiles()
        ' Set the default damage profile
        If ParentFitting.DamageProfileName <> "" Then
            cboDamageProfiles.SelectedItem = ParentFitting.DamageProfileName
        Else
            cboDamageProfiles.SelectedItem = "<Omni-Damage>"
        End If

        Call Me.LoadDefenceProfiles()
        cboDefenceProfiles.SelectedItem = "<No Resists>"

        ' Set collapsed panels
        epDefence.Expanded = Not HQF.Settings.HQFSettings.DefensePanelIsCollapsed
        epCapacitor.Expanded = Not HQF.Settings.HQFSettings.CapacitorPanelIsCollapsed
        epDamage.Expanded = Not HQF.Settings.HQFSettings.DamagePanelIsCollapsed
        epTargeting.Expanded = Not HQF.Settings.HQFSettings.TargetingPanelIsCollapsed
        epPropulsion.Expanded = Not HQF.Settings.HQFSettings.PropulsionPanelIsCollapsed
        epCargo.Expanded = Not HQF.Settings.HQFSettings.CargoPanelIsCollapsed

        ' Reset the startup flag
        StartUp = False
    End Sub

#End Region

#Region "Update Routines"

    Public Sub UpdateInfoDisplay()

        Dim sTime, eTime As Date
        sTime = Now

        ' Update the display with the information about the (fitted) ship
        Dim ttt As String = "" ' Fot tool tip text!
        Dim turretShip As Boolean = False
        Dim missileShip As Boolean = False

        ' CPU
        If ParentFitting.FittedShip.CPU > 0 Then
            pbxCPU.Maximum = CInt(ParentFitting.FittedShip.CPU)
        Else
            pbxCPU.Maximum = 1
        End If
        pbxCPU.Value = Math.Min(CInt(ParentFitting.FittedShip.CPU_Used), CInt(ParentFitting.FittedShip.CPU))
        Select Case Math.Round(ParentFitting.FittedShip.CPU_Used, 4) / ParentFitting.FittedShip.CPU
            Case Is > 1
                pbxCPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error
                lblCPUReqd.Text = FormatNumber((ParentFitting.FittedShip.CPU_Used - ParentFitting.FittedShip.CPU) / ParentFitting.FittedShip.CPU * 100, 2) & "%"
                lblCPUReqd.ForeColor = Drawing.Color.Red
            Case Is < 0.9
                pbxCPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal
                lblCPUReqd.Text = ""
            Case Else
                pbxCPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Paused
                lblCPUReqd.Text = ""
        End Select
        lblCPU.Text = FormatNumber(ParentFitting.FittedShip.CPU_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(ParentFitting.FittedShip.CPU, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Powergrid
        If ParentFitting.FittedShip.PG > 0 Then
            pbxPG.Maximum = CInt(ParentFitting.FittedShip.PG)
        Else
            pbxPG.Maximum = 1
        End If
        pbxPG.Value = Math.Min(CInt(ParentFitting.FittedShip.PG_Used), CInt(ParentFitting.FittedShip.PG))
        Select Case Math.Round(ParentFitting.FittedShip.PG_Used, 4) / ParentFitting.FittedShip.PG
            Case Is > 1
                pbxPG.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error
                lblPGReqd.Text = FormatNumber((ParentFitting.FittedShip.PG_Used - ParentFitting.FittedShip.PG) / ParentFitting.FittedShip.PG * 100, 2) & "%"
                lblPGReqd.ForeColor = Drawing.Color.Red
            Case Is < 0.9
                pbxPG.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal
                lblPGReqd.Text = ""
            Case Else
                pbxPG.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Paused
                lblPGReqd.Text = ""
        End Select
        lblPG.Text = FormatNumber(ParentFitting.FittedShip.PG_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(ParentFitting.FittedShip.PG, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Calibration
        If ParentFitting.FittedShip.Calibration > 0 Then
            pbxCalibration.Maximum = CInt(ParentFitting.FittedShip.Calibration)
        Else
            pbxCalibration.Maximum = 1
        End If
        pbxCalibration.Value = Math.Min(CInt(ParentFitting.FittedShip.Calibration_Used), CInt(ParentFitting.FittedShip.Calibration))
        If ParentFitting.FittedShip.Calibration > 0 Then
            Select Case CDbl(Math.Round(ParentFitting.FittedShip.Calibration_Used, 4) / ParentFitting.FittedShip.Calibration)
                Case Is > 1
                    pbxCalibration.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error
                Case Is < 0.9
                    pbxCalibration.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal
                Case Else
                    pbxCalibration.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Paused
            End Select
        Else
            pbxCalibration.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal
        End If
        lblCalibration.Text = FormatNumber(ParentFitting.FittedShip.Calibration_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(ParentFitting.FittedShip.Calibration, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Shield
        lblShieldHP.Text = FormatNumber(ParentFitting.FittedShip.ShieldCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblShieldEM.Text = FormatNumber(ParentFitting.FittedShip.ShieldEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldExplosive.Text = FormatNumber(ParentFitting.FittedShip.ShieldExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldKinetic.Text = FormatNumber(ParentFitting.FittedShip.ShieldKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldThermal.Text = FormatNumber(ParentFitting.FittedShip.ShieldThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        ttt = "Shield Hitpoints: " & lblShieldHP.Text & ControlChars.CrLf
        ttt &= "Effective Hitpoints: " & FormatNumber(ParentFitting.FittedShip.EffectiveShieldHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ControlChars.CrLf
        ttt &= "Recharge Time: " & FormatNumber(ParentFitting.FittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s" & ControlChars.CrLf
        ttt &= "Average Recharge Rate: " & FormatNumber(ParentFitting.FittedShip.ShieldCapacity / ParentFitting.FittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP/s" & ControlChars.CrLf
        ttt &= "Peak Recharge Rate: " & FormatNumber(HQF.Settings.HQFSettings.ShieldRechargeConstant * ParentFitting.FittedShip.ShieldCapacity / ParentFitting.FittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP/s"
        ToolTip1.SetToolTip(pbShieldHP, ttt)
        ToolTip1.SetToolTip(lblShieldHP, ttt)

        ' Armor
        lblArmorHP.Text = FormatNumber(ParentFitting.FittedShip.ArmorCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblArmorEM.Text = FormatNumber(ParentFitting.FittedShip.ArmorEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorExplosive.Text = FormatNumber(ParentFitting.FittedShip.ArmorExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorKinetic.Text = FormatNumber(ParentFitting.FittedShip.ArmorKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorThermal.Text = FormatNumber(ParentFitting.FittedShip.ArmorThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        ttt = "Armor Hitpoints: " & lblArmorHP.Text & ControlChars.CrLf
        ttt &= "Effective Hitpoints: " & FormatNumber(ParentFitting.FittedShip.EffectiveArmorHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ToolTip1.SetToolTip(pbArmorHP, ttt)
        ToolTip1.SetToolTip(lblArmorHP, ttt)

        ' Structure
        lblStructureHP.Text = FormatNumber(ParentFitting.FittedShip.StructureCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblStructureEM.Text = FormatNumber(ParentFitting.FittedShip.StructureEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureExplosive.Text = FormatNumber(ParentFitting.FittedShip.StructureExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureKinetic.Text = FormatNumber(ParentFitting.FittedShip.StructureKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureThermal.Text = FormatNumber(ParentFitting.FittedShip.StructureThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        ttt = "Structure Hitpoints: " & lblStructureHP.Text & ControlChars.CrLf
        ttt &= "Effective Hitpoints: " & FormatNumber(ParentFitting.FittedShip.EffectiveStructureHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ToolTip1.SetToolTip(pbStructureHP, ttt)
        ToolTip1.SetToolTip(lblStructureHP, ttt)

        ' EffectiveHP
        lblEffectiveHP.Text = "Effective HP: " & FormatNumber(ParentFitting.FittedShip.EffectiveHP, 0) & " (Eve: " & FormatNumber(ParentFitting.FittedShip.EveEffectiveHP, 0) & ")"
        epDefence.TitleText = "Defense (EHP: " & FormatNumber(ParentFitting.FittedShip.EffectiveHP, 0) & ")"

        ' Tank Ability
        lblTankAbility.Text = "Tank Ability: " & CDbl(ParentFitting.FittedShip.Attributes("10062")).ToString("N2") & " DPS"
        ttt = "Passive Tank: " & CDbl(ParentFitting.FittedShip.Attributes("10069")).ToString("N2") & " DPS" & ControlChars.CrLf
        ttt &= "Shield Tank: " & CDbl(ParentFitting.FittedShip.Attributes("10059")).ToString("N2") & " DPS" & ControlChars.CrLf
        ttt &= "Armor Tank: " & CDbl(ParentFitting.FittedShip.Attributes("10060")).ToString("N2") & " DPS" & ControlChars.CrLf
        ttt &= "Structure Tank: " & CDbl(ParentFitting.FittedShip.Attributes("10061")).ToString("N2") & " DPS" & ControlChars.CrLf
        ttt &= ControlChars.CrLf
        ttt &= "Damage Profile DPS: " & FormatNumber(ParentFitting.FittedShip.DamageProfile.DPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " DPS" & ControlChars.CrLf
        If CDbl(ParentFitting.FittedShip.Attributes("10062")) >= ParentFitting.FittedShip.DamageProfile.DPS Then
            ttt &= "Can tank damage profile"
            lblTankAbility.ForeColor = Drawing.Color.Green
        Else
            ttt &= "Not able to tank damage profile"
            lblTankAbility.ForeColor = Drawing.Color.Red
        End If
        ToolTip1.SetToolTip(lblTankAbility, ttt)

        ' Capacitor
        lblCapacitor.Text = FormatNumber(ParentFitting.FittedShip.CapCapacity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " GJ"
        lblCapRecharge.Text = FormatNumber(ParentFitting.FittedShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        'lblCapAverage.Text = FormatNumber(parentfitting.fittedship.CapCapacity / parentfitting.fittedship.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " GJ/s"
        lblCapPeak.Text = FormatNumber(HQF.Settings.HQFSettings.CapRechargeConstant * ParentFitting.FittedShip.CapCapacity / ParentFitting.FittedShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " GJ/s"
        lblCapBalP.Text = "+" & FormatNumber((CDbl(ParentFitting.FittedShip.Attributes("10050")) * -1) + (HQF.Settings.HQFSettings.CapRechargeConstant * ParentFitting.FittedShip.CapCapacity / ParentFitting.FittedShip.CapRecharge), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCapBalN.Text = "- " & FormatNumber(ParentFitting.FittedShip.Attributes("10049"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Dim csr As CapSimResults = Capacitor.CalculateCapStatistics(ParentFitting.FittedShip, False)
        If csr.CapIsDrained = False Then
            epCapacitor.TitleText = "Capacitor (Stable at " & FormatNumber(csr.MinimumCap / ParentFitting.FittedShip.CapCapacity * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
        Else
            epCapacitor.TitleText = "Capacitor (Lasts " & EveHQ.Core.SkillFunctions.TimeToString(csr.TimeToDrain, False) & ")"
        End If

        ' Propulsion
        lblSpeed.Text = FormatNumber(ParentFitting.FittedShip.MaxVelocity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m/s"
        lblWarpSpeed.Text = FormatNumber(ParentFitting.FittedShip.WarpSpeed, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " au/s"
        ttt = "Warp Capacitor Need: " & ParentFitting.FittedShip.WarpCapNeed & ControlChars.CrLf
        ttt &= "Max Warp Distance: " & FormatNumber(ParentFitting.FittedShip.CapCapacity / (ParentFitting.FittedShip.WarpCapNeed * ParentFitting.FittedShip.Mass), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " au"
        ToolTip1.SetToolTip(lblWarpSpeed, ttt)
        lblInertia.Text = FormatNumber(ParentFitting.FittedShip.Inertia, 6, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ' Time to warp based on calculation from http://myeve.eve-online.com/ingameboard.asp?a=topic&threadID=502836
        ' Time to warp (in seconds) = Inertial Modifier * (Mass / 1.000.000) * 1.61
        '-ln(0.25) * Inertia Modifier * Mass / 1.000.000
        lblAlignTime.Text = FormatNumber(-Math.Log(0.25) * ParentFitting.FittedShip.Inertia * ParentFitting.FittedShip.Mass / 1000000, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        epPropulsion.TitleText = "Propulsion (Speed: " & ParentFitting.FittedShip.MaxVelocity.ToString("N2") & " m/s)"

        ' Targeting
        lblTargetRange.Text = FormatNumber(ParentFitting.FittedShip.MaxTargetRange, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m"
        lblTargets.Text = FormatNumber(ParentFitting.FittedShip.MaxLockedTargets, 0) & " / " & FormatNumber(CDbl(ParentFitting.FittedShip.Attributes("10064")), 0)
        ttt = "Max Ship Targets: " & FormatNumber(ParentFitting.FittedShip.MaxLockedTargets, 0) & ControlChars.CrLf
        ttt &= "Max Pilot Targets: " & FormatNumber(CDbl(ParentFitting.FittedShip.Attributes("10064")), 0)
        ToolTip1.SetToolTip(lblTargets, ttt)
        lblScanResolution.Text = FormatNumber(ParentFitting.FittedShip.ScanResolution, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " mm"
        Dim SensorStrength As Double = Math.Max(Math.Max(Math.Max(ParentFitting.FittedShip.GravSensorStrenth, ParentFitting.FittedShip.LadarSensorStrenth), ParentFitting.FittedShip.MagSensorStrenth), ParentFitting.FittedShip.RadarSensorStrenth)
        lblSensorStrength.Text = FormatNumber(SensorStrength, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ttt = "Gravimetric Strength: " & FormatNumber(ParentFitting.FittedShip.GravSensorStrenth, 2) & ControlChars.CrLf
        ttt &= "Ladar Strength: " & FormatNumber(ParentFitting.FittedShip.LadarSensorStrenth, 2) & ControlChars.CrLf
        ttt &= "Magnetometric Strength: " & FormatNumber(ParentFitting.FittedShip.MagSensorStrenth, 2) & ControlChars.CrLf
        ttt &= "Radar Strength: " & FormatNumber(ParentFitting.FittedShip.RadarSensorStrenth, 2) & ControlChars.CrLf
        ToolTip1.SetToolTip(lblSensorStrength, ttt)
        lblSigRadius.Text = FormatNumber(ParentFitting.FittedShip.SigRadius, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m"
        Dim ProbeableIndicator As Double = SensorStrength / ParentFitting.FittedShip.SigRadius
        lblProbeable.Text = FormatNumber(ProbeableIndicator, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Select Case ProbeableIndicator
            Case Is >= 1.08
                lblProbeable.ForeColor = Drawing.Color.LimeGreen
                ToolTip1.SetToolTip(lblProbeable, "Ship is unprobeable (>1.08)")
            Case Is < 1
                lblProbeable.ForeColor = Drawing.Color.Red
                ToolTip1.SetToolTip(lblProbeable, "Ship can be probed (<1.08)")
            Case Else
                lblProbeable.ForeColor = Drawing.Color.OrangeRed
                ToolTip1.SetToolTip(lblProbeable, "Ship may be unprobeable (1.00-1.08)")
        End Select
        epTargeting.TitleText = "Targeting (Range: " & ParentFitting.FittedShip.MaxTargetRange.ToString("N0") & "m)"

        ' Cargo and Drones
        lblCargoBay.Text = FormatNumber(ParentFitting.FittedShip.CargoBay, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"
        lblDroneBay.Text = FormatNumber(ParentFitting.FittedShip.DroneBay, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"
        Select Case ParentFitting.FittedShip.DatabaseGroup
            Case "28", "380", "463", "513", "543", "902", "941"
                epCargo.TitleText = "Storage (Cargo: " & ParentFitting.FittedShip.CargoBay.ToString("N0") & " m3)"
            Case Else
                epCargo.TitleText = "Storage (Drone: " & ParentFitting.FittedShip.DroneBay.ToString("N0") & " m3)"
        End Select

        UpdateDroneUsage()

        ' Damage & Mining
        If ParentFitting.FittedShip.OreTotalAmount > 0 Or ParentFitting.FittedShip.IceTotalAmount > 0 Then
            ' Only show mining information here
            pbDamage.Image = My.Resources.imgMining
            cboDefenceProfiles.Enabled = False
            btnEditDefenceProfiles.Enabled = False
            lblDamage.Text = FormatNumber(ParentFitting.FittedShip.OreTotalAmount + ParentFitting.FittedShip.IceTotalAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3 / " & FormatNumber(ParentFitting.FittedShip.OreTotalRate + ParentFitting.FittedShip.IceTotalRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3/s"
            epDamage.TitleText = "Mining (Rate: " & (ParentFitting.FittedShip.OreTotalRate + ParentFitting.FittedShip.IceTotalRate).ToString("N2") & ")"
            If ParentFitting.FittedShip.OreTotalAmount > 0 Or ParentFitting.FittedShip.IceTotalAmount > 0 Then
                ttt = ""
                If ParentFitting.FittedShip.OreTurretAmount > 0 Then
                    ttt &= "Mining Turret Yield: " & FormatNumber(ParentFitting.FittedShip.OreTurretAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    ttt &= " (m3/s: " & FormatNumber(ParentFitting.FittedShip.OreTurretRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.OreDroneAmount > 0 Then
                    ttt &= "Mining Drone Yield: " & FormatNumber(ParentFitting.FittedShip.OreDroneAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    ttt &= " (m3/s: " & FormatNumber(ParentFitting.FittedShip.OreDroneRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.IceTurretAmount > 0 Then
                    ttt &= "Ice Turret Yield: " & FormatNumber(ParentFitting.FittedShip.IceTurretAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    ttt &= " (m3/s: " & FormatNumber(ParentFitting.FittedShip.IceTurretRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.IceDroneAmount > 0 Then
                    ttt &= "Ice Drone Yield: " & FormatNumber(ParentFitting.FittedShip.IceDroneAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    ttt &= " (m3/s: " & FormatNumber(ParentFitting.FittedShip.IceDroneRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                End If
                ToolTip1.SetToolTip(lblDamage, ttt)
            End If
        Else
            ' Show damage information
            pbDamage.Image = My.Resources.imgTurretSlots
            cboDefenceProfiles.Enabled = True
            btnEditDefenceProfiles.Enabled = True
            lblDamage.Text = FormatNumber(ParentFitting.FittedShip.TotalVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(ParentFitting.FittedShip.TotalDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " DPS"
            epDamage.TitleText = "Damage (DPS: " & ParentFitting.FittedShip.TotalDPS.ToString("N2") & ")"
            If ParentFitting.FittedShip.TotalVolley > 0 Then
                ttt = ""
                If ParentFitting.FittedShip.TurretVolley > 0 Then
                    ttt &= "Turret Volley: " & FormatNumber(ParentFitting.FittedShip.TurretVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    ttt &= " (DPS: " & FormatNumber(ParentFitting.FittedShip.TurretDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                    turretShip = True
                End If
                If ParentFitting.FittedShip.MissileVolley > 0 Then
                    ttt &= "Missile Volley: " & FormatNumber(ParentFitting.FittedShip.MissileVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    ttt &= " (DPS: " & FormatNumber(ParentFitting.FittedShip.MissileDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                    missileShip = True
                End If
                If ParentFitting.FittedShip.SBVolley > 0 Then
                    ttt &= "Smartbomb Volley: " & FormatNumber(ParentFitting.FittedShip.SBVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    ttt &= " (DPS: " & FormatNumber(ParentFitting.FittedShip.SBDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.DroneVolley > 0 Then
                    ttt &= "Drone Volley: " & FormatNumber(ParentFitting.FittedShip.DroneVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    ttt &= " (DPS: " & FormatNumber(ParentFitting.FittedShip.DroneDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                End If
                ToolTip1.SetToolTip(lblDamage, ttt)
                Dim dpr As DefenceProfileResults = ParentFitting.CalculateDamageStatsForDefenceProfile(ParentFitting.FittedShip)
                lblDPR.Text = dpr.ShieldDPS.ToString("N2") & " / " & dpr.ArmorDPS.ToString("N2") & " / " & dpr.HullDPS.ToString("N2")
            End If
        End If

        ' Collect List of Needed Skills
        reqSkillsCollection = ParentFitting.CalculateNeededSkills(cboPilots.SelectedItem.ToString)
        If reqSkillsCollection.TruePilotSkills.Count = 0 Then
            btnSkills.Image = My.Resources.Skills1
            ToolTip1.SetToolTip(btnSkills, "No additional skills required")
        Else
            If reqSkillsCollection.ShipPilotSkills.Count = 0 Then
                btnSkills.Image = My.Resources.Skills2
                ToolTip1.SetToolTip(btnSkills, "HQF skills match, additional actual skills required - click to view")
            Else
                btnSkills.Image = My.Resources.Skills0
                ToolTip1.SetToolTip(btnSkills, "Additional skills required - click to view")
            End If
        End If

        ' Set buttons
        btnDamageAnalysis.Enabled = (turretShip Or missileShip)

        Me.Refresh()

        eTime = Now
        'MessageBox.Show((eTime - sTime).TotalMilliseconds.ToString & "ms", "Ship Info Control Update")
    End Sub

    Public Sub UpdateImplantList()
        If cboPilots.SelectedItem IsNot Nothing Then
            cboImplants.Tag = "Updating"
            Dim oldImplants As String
            Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
            oldImplants = shipPilot.ImplantName(0)
            cboImplants.BeginUpdate()
            cboImplants.Items.Clear()
            cboImplants.Items.Add("*Custom*")
            For Each cImplantSet As String In HQF.Settings.HQFSettings.ImplantGroups.Keys
                cboImplants.Items.Add(cImplantSet)
            Next
            If cboImplants.Items.Contains(oldImplants) Then
                cboImplants.SelectedItem = oldImplants
            End If
            cboImplants.EndUpdate()
            cboImplants.Tag = Nothing
        End If
    End Sub

    Public Sub UpdateDroneUsage()
        ParentFitting.FittedShip.DroneBandwidth_Used = 0
        For Each DBI As DroneBayItem In ParentFitting.FittedShip.DroneBayItems.Values
            If DBI.IsActive = True Then
                ParentFitting.FittedShip.DroneBandwidth_Used += CDbl(DBI.DroneType.Attributes("1272")) * DBI.Quantity
            End If
        Next
        lblDroneBandwidth.Text = FormatNumber(ParentFitting.FittedShip.DroneBandwidth_Used, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(ParentFitting.FittedShip.DroneBandwidth, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblDroneControl.Text = ParentFitting.FittedShip.UsedDrones & " / " & ParentFitting.FittedShip.MaxDrones
        Dim ttt As String = "Drone Control Range: " & FormatNumber(ParentFitting.FittedShip.Attributes("10007"), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "m"
        ToolTip1.SetToolTip(lblDroneControl, ttt)
        ToolTip1.SetToolTip(pbDroneControl, ttt)
    End Sub

#End Region

#Region "Damage Profile Routines"

    Private Sub cboDamageProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDamageProfiles.SelectedIndexChanged
        If cboDamageProfiles.SelectedItem IsNot Nothing Then
            ParentFitting.DamageProfileName = cboDamageProfiles.SelectedItem.ToString
        End If
        ' State damage profile info
        Dim info As New System.Text.StringBuilder
        info.AppendLine(ParentFitting.BaseShip.DamageProfile.Name & " Profile Info:")
        info.AppendLine("EM: " & FormatNumber(ParentFitting.BaseShip.DamageProfile.EM, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        info.AppendLine("Explosive: " & FormatNumber(ParentFitting.BaseShip.DamageProfile.Explosive, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        info.AppendLine("Kinetic: " & FormatNumber(ParentFitting.BaseShip.DamageProfile.Kinetic, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        info.AppendLine("Thermal: " & FormatNumber(ParentFitting.BaseShip.DamageProfile.Thermal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        info.AppendLine("DPS: " & FormatNumber(ParentFitting.BaseShip.DamageProfile.DPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        ToolTip1.SetToolTip(cboDamageProfiles, info.ToString)
        ' Only perform this if we aren't setting the item at startup
        If StartUp = False Then
            ParentFitting.FittedShip.RecalculateEffectiveHP()
            ParentFitting.CalculateDefenceStatistics(ParentFitting.FittedShip)
            ParentFitting.ShipInfoCtrl.UpdateInfoDisplay()
        End If
    End Sub

    Private Sub btnEditProfiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditProfiles.Click
        ' Open options form
        Dim mySettings As New frmHQFSettings
        mySettings.Tag = "nodeDamageProfiles"
        mySettings.ShowDialog()
        mySettings = Nothing
        Dim oldProfile As String = cboDamageProfiles.SelectedItem.ToString
        Call Me.LoadDamageProfiles()
        If cboDamageProfiles.Items.Contains(oldProfile) = True Then
            cboDamageProfiles.SelectedItem = oldProfile
        Else
            cboDamageProfiles.SelectedItem = "<Omni-Damage>"
        End If
    End Sub

    Private Sub LoadDamageProfiles()
        ' Add the list of profiles to the combo box
        cboDamageProfiles.BeginUpdate()
        cboDamageProfiles.Items.Clear()
        For Each cProfile As String In DamageProfiles.ProfileList.Keys
            cboDamageProfiles.Items.Add(cProfile)
        Next
        cboDamageProfiles.EndUpdate()
    End Sub

#End Region

#Region "Defence Profile Routines"

    Private Sub cboDefenceProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDefenceProfiles.SelectedIndexChanged
        If cboDefenceProfiles.SelectedItem IsNot Nothing Then
            ParentFitting.DefenceProfileName = cboDefenceProfiles.SelectedItem.ToString
        End If
        ' Only perform this if we aren't setting the item at startup
        If StartUp = False Then
            Dim dpr As DefenceProfileResults = ParentFitting.CalculateDamageStatsForDefenceProfile(ParentFitting.FittedShip)
            lblDPR.Text = dpr.ShieldDPS.ToString("N2") & " / " & dpr.ArmorDPS.ToString("N2") & " / " & dpr.HullDPS.ToString("N2")
        End If
    End Sub

    Private Sub btnEditDefenceProfiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditDefenceProfiles.Click
        ' Open options form
        Dim mySettings As New frmHQFSettings
        mySettings.Tag = "nodeDefenceProfiles"
        mySettings.ShowDialog()
        mySettings = Nothing
        Dim oldProfile As String = ""
        If cboDefenceProfiles.SelectedItem IsNot Nothing Then
            oldProfile = cboDefenceProfiles.SelectedItem.ToString
        End If
        Call Me.LoadDefenceProfiles()
        If cboDefenceProfiles.Items.Contains(oldProfile) = True Then
            cboDefenceProfiles.SelectedItem = oldProfile
        Else
            cboDefenceProfiles.SelectedItem = "<No Resists>"
        End If
    End Sub

    Private Sub LoadDefenceProfiles()
        ' Add the list of profiles to the combo box
        cboDefenceProfiles.BeginUpdate()
        cboDefenceProfiles.Items.Clear()
        For Each cProfile As String In DefenceProfiles.ProfileList.Keys
            cboDefenceProfiles.Items.Add(cProfile)
        Next
        cboDefenceProfiles.EndUpdate()
    End Sub

#End Region

#Region "UI Routines"

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        ' Build the Affections data for this pilot
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
        ParentFitting.PilotName = shipPilot.PilotName
        cboImplants.Tag = "Updating"
        If shipPilot.ImplantName(0) IsNot Nothing Then
            cboImplants.SelectedItem = shipPilot.ImplantName(0)
        End If
        cboImplants.Tag = Nothing
        ' Only perform this if we aren't setting the item at startup
        If StartUp = False Then
            ParentFitting.ApplyFitting(BuildType.BuildEverything)
            HQFEvents.StartUpdateModuleList = True
        End If
    End Sub

    Private Sub cboImplants_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboImplants.SelectedIndexChanged
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
        ' Update the pilot's implants?
        shipPilot.ImplantName(0) = cboImplants.SelectedItem.ToString
        Dim implantList As New System.Text.StringBuilder
        If cboImplants.SelectedItem.ToString <> "*Custom*" Then
            If HQF.Settings.HQFSettings.ImplantGroups.ContainsKey(cboImplants.SelectedItem.ToString) Then
                Dim currentImplantGroup As ImplantGroup = CType(HQF.Settings.HQFSettings.ImplantGroups(cboImplants.SelectedItem.ToString), ImplantGroup)
                For imp As Integer = 1 To 10
                    If currentImplantGroup.ImplantName(imp) = "" Then
                        implantList.AppendLine("Slot " & imp.ToString & ": <Empty>")
                    Else
                        implantList.AppendLine("Slot " & imp.ToString & ": " & currentImplantGroup.ImplantName(imp))
                    End If
                Next
            End If
        Else
            For imp As Integer = 1 To 10
                If shipPilot.ImplantName(imp) = "" Then
                    implantList.AppendLine("Slot " & imp.ToString & ": <Empty>")
                Else
                    implantList.AppendLine("Slot " & imp.ToString & ": " & shipPilot.ImplantName(imp))
                End If
            Next
        End If
        ToolTip1.SetToolTip(cboImplants, implantList.ToString)
        ' Only perform this if we aren't setting the item at startup
        If StartUp = False Then
            If cboImplants.Tag Is Nothing Then
                ParentFitting.ApplyFitting(BuildType.BuildEverything)
                HQFEvents.StartUpdateModuleList = True
            End If
        End If
    End Sub

    Private Sub btnSkills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSkills.Click
        If reqSkillsCollection.TruePilotSkills.Count > 0 Then
            Dim myRequiredSkills As New frmRequiredSkills
            myRequiredSkills.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem), EveHQ.Core.Pilot)
            myRequiredSkills.Skills = reqSkillsCollection.TruePilotSkills
            myRequiredSkills.ShowDialog()
        End If
    End Sub

    Private Sub btnLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLog.Click
        Dim myAuditLog As New frmShipAudit
        Dim logData() As String
        Dim newLog As New ListViewItem
        myAuditLog.lvwAudit.BeginUpdate()
        For Each log As String In ParentFitting.FittedShip.AuditLog
            logData = log.Split("#".ToCharArray)
            'If logData(2).Trim <> logData(3).Trim Then
            newLog = New ListViewItem
            newLog.Text = logData(0).Trim
            newLog.SubItems.Add(logData(1).Trim)
            newLog.SubItems.Add(FormatNumber(logData(2).Trim, 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newLog.SubItems.Add(FormatNumber(logData(3).Trim, 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            myAuditLog.lvwAudit.Items.Add(newLog)
            'End If
        Next
        myAuditLog.lvwAudit.EndUpdate()
        myAuditLog.ShowDialog()
        myAuditLog = Nothing
    End Sub

    Private Sub btnTargetSpeed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTargetSpeed.Click
        Dim targetSpeed As New frmTargetSpeed
        targetSpeed.ShipType = ParentFitting.FittedShip
        targetSpeed = Nothing
    End Sub

    Private Sub btnDamageAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDamageAnalysis.Click
        Dim myDA As New frmDamageAnalysis
        If cboPilots.SelectedItem IsNot Nothing Then
            myDA.PilotName = cboPilots.SelectedItem.ToString
        End If
        If ParentFitting.ShipSlotCtrl IsNot Nothing Then
            myDA.FittingName = ParentFitting.FittingName
        End If
        myDA.ShowDialog()
        myDA.Dispose()
    End Sub

    Private Sub btnCapSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCapSim.Click
        Dim CapSimForm As New frmCapSim(ParentFitting.FittedShip)
        CapSimForm.ShowDialog()
        CapSimForm.Dispose()
    End Sub

    Private Sub epDefence_ExpandedChanged(sender As Object, e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles epDefence.ExpandedChanged
        If StartUp = False Then
            HQF.Settings.HQFSettings.DefensePanelIsCollapsed = Not epDefence.Expanded
        End If
    End Sub

    Private Sub epCapacitor_ExpandedChanged(sender As Object, e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles epCapacitor.ExpandedChanged
        If StartUp = False Then
            HQF.Settings.HQFSettings.CapacitorPanelIsCollapsed = Not epCapacitor.Expanded
        End If
    End Sub

    Private Sub epDamage_ExpandedChanged(sender As Object, e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles epDamage.ExpandedChanged
        If StartUp = False Then
            HQF.Settings.HQFSettings.DamagePanelIsCollapsed = Not epDamage.Expanded
        End If
    End Sub

    Private Sub epTargeting_ExpandedChanged(sender As Object, e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles epTargeting.ExpandedChanged
        If StartUp = False Then
            HQF.Settings.HQFSettings.TargetingPanelIsCollapsed = Not epTargeting.Expanded
        End If
    End Sub

    Private Sub epPropulsion_ExpandedChanged(sender As Object, e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles epPropulsion.ExpandedChanged
        If StartUp = False Then
            HQF.Settings.HQFSettings.PropulsionPanelIsCollapsed = Not epPropulsion.Expanded
        End If
    End Sub

    Private Sub epCargo_ExpandedChanged(sender As Object, e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles epCargo.ExpandedChanged
        If StartUp = False Then
            HQF.Settings.HQFSettings.CargoPanelIsCollapsed = Not epCargo.Expanded
        End If
    End Sub

#End Region

End Class
