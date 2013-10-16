' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
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
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevComponents.DotNetBar
Imports EveHQ.Core
Imports EveHQ.HQF.Forms

Namespace Controls

    Public Class ShipInfoControl

#Region "Other Variables"
        Private _reqSkillsCollection As New NeededSkillsCollection ' Lists the skills required for the ship/mods/drones
        Private ReadOnly _startUp As Boolean = False
#End Region

#Region "Property Variables"
        Private ReadOnly _parentFitting As Fitting ' Stores the fitting to which this control is attached to
#End Region

#Region "Properties"

        Public ReadOnly Property ParentFitting() As Fitting
            Get
                Return _parentFitting
            End Get
        End Property

#End Region

#Region "Constructor"

        Public Sub New(ByVal shipFit As Fitting)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Set the parent fitting
            _parentFitting = ShipFit

            ' Set the startup flag
            _startUp = True

            ' Add the current list of pilots to the combobox
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
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
                If cboPilots.Items.Contains(PluginSettings.HQFSettings.DefaultPilot) = True Then
                    cboPilots.SelectedItem = PluginSettings.HQFSettings.DefaultPilot

                ElseIf cboPilots.Items.Count > 0 Then 'There are pilots, but not the default pilot
                    cboPilots.SelectedItem = cboPilots.Items(0) ' select the first pilot.

                End If

            End If

            ' Add the current list of implants to the combobox
            Call UpdateImplantList()

            Call LoadDamageProfiles()
            ' Set the default damage profile
            If ParentFitting.DamageProfileName <> "" Then
                cboDamageProfiles.SelectedItem = ParentFitting.DamageProfileName
            Else
                cboDamageProfiles.SelectedItem = "<Omni-Damage>"
            End If

            Call LoadDefenceProfiles()
            cboDefenceProfiles.SelectedItem = "<No Resists>"

            ' Set collapsed panels
            epDefence.Expanded = Not PluginSettings.HQFSettings.DefensePanelIsCollapsed
            epCapacitor.Expanded = Not PluginSettings.HQFSettings.CapacitorPanelIsCollapsed
            epDamage.Expanded = Not PluginSettings.HQFSettings.DamagePanelIsCollapsed
            epTargeting.Expanded = Not PluginSettings.HQFSettings.TargetingPanelIsCollapsed
            epPropulsion.Expanded = Not PluginSettings.HQFSettings.PropulsionPanelIsCollapsed
            epCargo.Expanded = Not PluginSettings.HQFSettings.CargoPanelIsCollapsed

            ' Reset the startup flag
            _startUp = False
        End Sub

#End Region

#Region "Update Routines"

        Public Sub UpdateInfoDisplay()

            ' Update the display with the information about the (fitted) ship
            Dim ttt As String ' For tool tip text!
            Dim turretShip As Boolean = False
            Dim missileShip As Boolean = False

            ' CPU
            If ParentFitting.FittedShip.CPU > 0 Then
                pbxCPU.Maximum = CInt(ParentFitting.FittedShip.CPU)
            Else
                pbxCPU.Maximum = 1
            End If
            pbxCPU.Value = Math.Min(CInt(ParentFitting.FittedShip.CPUUsed), CInt(ParentFitting.FittedShip.CPU))
            Select Case Math.Round(ParentFitting.FittedShip.CPUUsed, 4) / ParentFitting.FittedShip.CPU
                Case Is > 1
                    pbxCPU.ColorTable = eProgressBarItemColor.Error
                    lblCPUReqd.Text = ((ParentFitting.FittedShip.CPUUsed - ParentFitting.FittedShip.CPU) / ParentFitting.FittedShip.CPU * 100).ToString("N2") & "%"
                    lblCPUReqd.ForeColor = Color.Red
                Case Is < 0.9
                    pbxCPU.ColorTable = eProgressBarItemColor.Normal
                    lblCPUReqd.Text = ""
                Case Else
                    pbxCPU.ColorTable = eProgressBarItemColor.Paused
                    lblCPUReqd.Text = ""
            End Select
            lblCPU.Text = ParentFitting.FittedShip.CPUUsed.ToString("N2") & " / " & ParentFitting.FittedShip.CPU.ToString("N2")

            ' Powergrid
            If ParentFitting.FittedShip.PG > 0 Then
                pbxPG.Maximum = CInt(ParentFitting.FittedShip.PG)
            Else
                pbxPG.Maximum = 1
            End If
            pbxPG.Value = Math.Min(CInt(ParentFitting.FittedShip.PGUsed), CInt(ParentFitting.FittedShip.PG))
            Select Case Math.Round(ParentFitting.FittedShip.PGUsed, 4) / ParentFitting.FittedShip.PG
                Case Is > 1
                    pbxPG.ColorTable = eProgressBarItemColor.Error
                    lblPGReqd.Text = ((ParentFitting.FittedShip.PGUsed - ParentFitting.FittedShip.PG) / ParentFitting.FittedShip.PG * 100).ToString("N2") & "%"
                    lblPGReqd.ForeColor = Color.Red
                Case Is < 0.9
                    pbxPG.ColorTable = eProgressBarItemColor.Normal
                    lblPGReqd.Text = ""
                Case Else
                    pbxPG.ColorTable = eProgressBarItemColor.Paused
                    lblPGReqd.Text = ""
            End Select
            lblPG.Text = ParentFitting.FittedShip.PGUsed.ToString("N2") & " / " & ParentFitting.FittedShip.PG.ToString("N2")

            ' Calibration
            If ParentFitting.FittedShip.Calibration > 0 Then
                pbxCalibration.Maximum = CInt(ParentFitting.FittedShip.Calibration)
            Else
                pbxCalibration.Maximum = 1
            End If
            pbxCalibration.Value = Math.Min(CInt(ParentFitting.FittedShip.CalibrationUsed), CInt(ParentFitting.FittedShip.Calibration))
            If ParentFitting.FittedShip.Calibration > 0 Then
                Select Case CDbl(Math.Round(ParentFitting.FittedShip.CalibrationUsed, 4) / ParentFitting.FittedShip.Calibration)
                    Case Is > 1
                        pbxCalibration.ColorTable = eProgressBarItemColor.Error
                    Case Is < 0.9
                        pbxCalibration.ColorTable = eProgressBarItemColor.Normal
                    Case Else
                        pbxCalibration.ColorTable = eProgressBarItemColor.Paused
                End Select
            Else
                pbxCalibration.ColorTable = eProgressBarItemColor.Normal
            End If
            lblCalibration.Text = ParentFitting.FittedShip.CalibrationUsed.ToString("N2") & " / " & ParentFitting.FittedShip.Calibration.ToString("N2")

            ' Shield
            lblShieldHP.Text = ParentFitting.FittedShip.ShieldCapacity.ToString("N0")
            lblShieldEM.Text = ParentFitting.FittedShip.ShieldEMResist.ToString("N2") & "%"
            lblShieldExplosive.Text = ParentFitting.FittedShip.ShieldExResist.ToString("N2") & "%"
            lblShieldKinetic.Text = ParentFitting.FittedShip.ShieldKiResist.ToString("N2") & "%"
            lblShieldThermal.Text = ParentFitting.FittedShip.ShieldThResist.ToString("N2") & "%"
            ttt = "Shield Hitpoints: " & lblShieldHP.Text & ControlChars.CrLf
            ttt &= "Effective Hitpoints: " & ParentFitting.FittedShip.EffectiveShieldHP.ToString("N0") & ControlChars.CrLf
            ttt &= "Recharge Time: " & ParentFitting.FittedShip.ShieldRecharge.ToString("N2") & " s" & ControlChars.CrLf
            ttt &= "Average Recharge Rate: " & (ParentFitting.FittedShip.ShieldCapacity / ParentFitting.FittedShip.ShieldRecharge).ToString("N2") & " HP/s" & ControlChars.CrLf
            ttt &= "Peak Recharge Rate: " & (PluginSettings.HQFSettings.ShieldRechargeConstant * ParentFitting.FittedShip.ShieldCapacity / ParentFitting.FittedShip.ShieldRecharge).ToString("N2") & " HP/s"
            ToolTip1.SetToolTip(pbShieldHP, ttt)
            ToolTip1.SetToolTip(lblShieldHP, ttt)

            ' Armor
            lblArmorHP.Text = ParentFitting.FittedShip.ArmorCapacity.ToString("N0")
            lblArmorEM.Text = ParentFitting.FittedShip.ArmorEMResist.ToString("N2") & "%"
            lblArmorExplosive.Text = ParentFitting.FittedShip.ArmorExResist.ToString("N2") & "%"
            lblArmorKinetic.Text = ParentFitting.FittedShip.ArmorKiResist.ToString("N2") & "%"
            lblArmorThermal.Text = ParentFitting.FittedShip.ArmorThResist.ToString("N2") & "%"
            ttt = "Armor Hitpoints: " & lblArmorHP.Text & ControlChars.CrLf
            ttt &= "Effective Hitpoints: " & ParentFitting.FittedShip.EffectiveArmorHP.ToString("N0")
            ToolTip1.SetToolTip(pbArmorHP, ttt)
            ToolTip1.SetToolTip(lblArmorHP, ttt)

            ' Structure
            lblStructureHP.Text = ParentFitting.FittedShip.StructureCapacity.ToString("N0")
            lblStructureEM.Text = ParentFitting.FittedShip.StructureEMResist.ToString("N2") & "%"
            lblStructureExplosive.Text = ParentFitting.FittedShip.StructureExResist.ToString("N2") & "%"
            lblStructureKinetic.Text = ParentFitting.FittedShip.StructureKiResist.ToString("N2") & "%"
            lblStructureThermal.Text = ParentFitting.FittedShip.StructureThResist.ToString("N2") & "%"
            ttt = "Structure Hitpoints: " & lblStructureHP.Text & ControlChars.CrLf
            ttt &= "Effective Hitpoints: " & ParentFitting.FittedShip.EffectiveStructureHP.ToString("N0")
            ToolTip1.SetToolTip(pbStructureHP, ttt)
            ToolTip1.SetToolTip(lblStructureHP, ttt)

            ' EffectiveHP
            lblEffectiveHP.Text = "Effective HP: " & ParentFitting.FittedShip.EffectiveHP.ToString("N0") & " (Eve: " & ParentFitting.FittedShip.EveEffectiveHP.ToString("N0") & ")"
            epDefence.TitleText = "Defense (EHP: " & ParentFitting.FittedShip.EffectiveHP.ToString("N0") & ")"

            ' Tank Ability
            lblTankAbility.Text = "Tank Ability: " & CDbl(ParentFitting.FittedShip.Attributes(10062)).ToString("N2") & " DPS"
            ttt = "Passive Tank: " & CDbl(ParentFitting.FittedShip.Attributes(10069)).ToString("N2") & " DPS" & ControlChars.CrLf
            ttt &= "Shield Tank: " & CDbl(ParentFitting.FittedShip.Attributes(10059)).ToString("N2") & " DPS" & ControlChars.CrLf
            ttt &= "Armor Tank: " & CDbl(ParentFitting.FittedShip.Attributes(10060)).ToString("N2") & " DPS" & ControlChars.CrLf
            ttt &= "Structure Tank: " & CDbl(ParentFitting.FittedShip.Attributes(10061)).ToString("N2") & " DPS" & ControlChars.CrLf
            ttt &= ControlChars.CrLf
            ttt &= "Damage Profile DPS: " & ParentFitting.FittedShip.DamageProfile.DPS.ToString("N2") & " DPS" & ControlChars.CrLf
            If CDbl(ParentFitting.FittedShip.Attributes(10062)) >= ParentFitting.FittedShip.DamageProfile.DPS Then
                ttt &= "Can tank damage profile"
                lblTankAbility.ForeColor = Color.Green
            Else
                ttt &= "Not able to tank damage profile"
                lblTankAbility.ForeColor = Color.Red
            End If
            ToolTip1.SetToolTip(lblTankAbility, ttt)

            ' Capacitor
            lblCapacitor.Text = ParentFitting.FittedShip.CapCapacity.ToString("N2") & " GJ"
            lblCapRecharge.Text = ParentFitting.FittedShip.CapRecharge.ToString("N2") & " s"
            lblCapPeak.Text = (PluginSettings.HQFSettings.CapRechargeConstant * ParentFitting.FittedShip.CapCapacity / ParentFitting.FittedShip.CapRecharge).ToString("N2")
            Dim capBalP As Double = (CDbl(ParentFitting.FittedShip.Attributes(10050)) * -1) + (PluginSettings.HQFSettings.CapRechargeConstant * ParentFitting.FittedShip.CapCapacity / ParentFitting.FittedShip.CapRecharge)
            Dim capBalN As Double = ParentFitting.FittedShip.Attributes(10049)
            lblCapBalP.Text = "+" & capBalP.ToString("N2")
            lblCapBalN.Text = "- " & capBalN.ToString("N2")
            lblCapBal.Text = "Δ " & (capBalP - capBalN).ToString("N2")

            Dim csr As CapSimResults = Capacitor.CalculateCapStatistics(ParentFitting.FittedShip, False)
            If csr.CapIsDrained = False Then
                epCapacitor.TitleText = "Capacitor (Stable at " & (csr.MinimumCap / ParentFitting.FittedShip.CapCapacity * 100).ToString("N2") & "%)"
            Else
                epCapacitor.TitleText = "Capacitor (Lasts " & SkillFunctions.TimeToString(csr.TimeToDrain, False) & ")"
            End If

            ' Propulsion
            lblSpeed.Text = ParentFitting.FittedShip.MaxVelocity.ToString("N2") & " m/s"
            lblWarpSpeed.Text = ParentFitting.FittedShip.WarpSpeed.ToString("N2") & " au/s"
            ttt = "Warp Capacitor Need: " & ParentFitting.FittedShip.WarpCapNeed & ControlChars.CrLf
            ttt &= "Max Warp Distance: " & (ParentFitting.FittedShip.CapCapacity / (ParentFitting.FittedShip.WarpCapNeed * ParentFitting.FittedShip.Mass)).ToString("N2") & " au"
            ToolTip1.SetToolTip(lblWarpSpeed, ttt)
            lblInertia.Text = ParentFitting.FittedShip.Inertia.ToString("N6")
            ' Time to warp based on calculation from http://myeve.eve-online.com/ingameboard.asp?a=topic&threadID=502836
            ' Time to warp (in seconds) = Inertial Modifier * (Mass / 1.000.000) * 1.61
            '-ln(0.25) * Inertia Modifier * Mass / 1.000.000
            lblAlignTime.Text = (-Math.Log(0.25) * ParentFitting.FittedShip.Inertia * ParentFitting.FittedShip.Mass / 1000000).ToString("N2") & " s"
            epPropulsion.TitleText = "Propulsion (Speed: " & ParentFitting.FittedShip.MaxVelocity.ToString("N2") & " m/s)"

            ' Targeting
            lblTargetRange.Text = ParentFitting.FittedShip.MaxTargetRange.ToString("N0") & " m"
            lblTargets.Text = ParentFitting.FittedShip.MaxLockedTargets.ToString("N0") & " / " & ParentFitting.FittedShip.Attributes(10064).ToString("N0")
            ttt = "Max Ship Targets: " & ParentFitting.FittedShip.MaxLockedTargets.ToString("N0") & ControlChars.CrLf
            ttt &= "Max Pilot Targets: " & ParentFitting.FittedShip.Attributes(10064).ToString("N0")
            ToolTip1.SetToolTip(lblTargets, ttt)
            lblScanResolution.Text = ParentFitting.FittedShip.ScanResolution.ToString("N2") & " mm"
            Dim sensorStrength As Double = Math.Max(Math.Max(Math.Max(ParentFitting.FittedShip.GravSensorStrenth, ParentFitting.FittedShip.LadarSensorStrenth), ParentFitting.FittedShip.MagSensorStrenth), ParentFitting.FittedShip.RadarSensorStrenth)
            lblSensorStrength.Text = sensorStrength.ToString("N2")
            ttt = "Gravimetric Strength: " & ParentFitting.FittedShip.GravSensorStrenth.ToString("N2") & ControlChars.CrLf
            ttt &= "Ladar Strength: " & ParentFitting.FittedShip.LadarSensorStrenth.ToString("N2") & ControlChars.CrLf
            ttt &= "Magnetometric Strength: " & ParentFitting.FittedShip.MagSensorStrenth.ToString("N2") & ControlChars.CrLf
            ttt &= "Radar Strength: " & ParentFitting.FittedShip.RadarSensorStrenth.ToString("N2") & ControlChars.CrLf
            ToolTip1.SetToolTip(lblSensorStrength, ttt)
            lblSigRadius.Text = ParentFitting.FittedShip.SigRadius.ToString("N0") & " m"
            Dim probeableIndicator As Double = ParentFitting.FittedShip.SigRadius / sensorStrength
            lblProbeable.Text = probeableIndicator.ToString("N2")
            Select Case probeableIndicator
                Case Is <= 1.2
                    lblProbeable.ForeColor = Color.LimeGreen
                    ToolTip1.SetToolTip(lblProbeable, "Ship is very hard to probe (<1.2)")
                Case Is > 2
                    lblProbeable.ForeColor = Color.Red
                    ToolTip1.SetToolTip(lblProbeable, "Ship can be probed easily (>2.0)")
                Case Else
                    lblProbeable.ForeColor = Color.Yellow
                    ToolTip1.SetToolTip(lblProbeable, "Ship is difficult to probe (1.2-2.0)")
            End Select
            epTargeting.TitleText = "Targeting (Range: " & ParentFitting.FittedShip.MaxTargetRange.ToString("N0") & "m)"

            ' Cargo and Drones
            Dim mainBaySize As Double = ParentFitting.FittedShip.CargoBay
            ttt = "Cargo Bay Capacity: " & ParentFitting.FittedShip.CargoBay.ToString("N0") & " m3"
            ' Check for specialty bays
            Dim bays As Dictionary(Of String, Double) = New Dictionary(Of String, Double)()
            If ParentFitting.FittedShip.Attributes.ContainsKey(AttributeEnum.ShipFuelBay) Then
                bays.Add(Attributes.AttributeList(AttributeEnum.ShipFuelBay).DisplayName, ParentFitting.FittedShip.Attributes(AttributeEnum.ShipFuelBay))
            End If
            If ParentFitting.FittedShip.Attributes.ContainsKey(AttributeEnum.ShipFleetHangar) Then
                bays.Add(Attributes.AttributeList(AttributeEnum.ShipFleetHangar).DisplayName, ParentFitting.FittedShip.Attributes(AttributeEnum.ShipFleetHangar))
            End If
            If ParentFitting.FittedShip.Attributes.ContainsKey(AttributeEnum.ShipOreHold) Then
                If ParentFitting.FittedShip.Attributes(AttributeEnum.ShipOreHold) > mainBaySize Then
                    If ParentFitting.FittedShip.MarketGroup <> ModuleEnum.MarketgroupORECapitalIndustrials Then
                        mainBaySize = ParentFitting.FittedShip.Attributes(AttributeEnum.ShipOreHold)
                    End If
                End If
                bays.Add(Attributes.AttributeList(AttributeEnum.ShipOreHold).DisplayName, ParentFitting.FittedShip.Attributes(AttributeEnum.ShipOreHold))
            End If
            If ParentFitting.FittedShip.Attributes.ContainsKey(AttributeEnum.ShipMineralHold) Then
                If ParentFitting.FittedShip.Attributes(AttributeEnum.ShipMineralHold) > mainBaySize Then
                    mainBaySize = ParentFitting.FittedShip.Attributes(AttributeEnum.ShipMineralHold)
                End If
                bays.Add(Attributes.AttributeList(AttributeEnum.ShipMineralHold).DisplayName, ParentFitting.FittedShip.Attributes(AttributeEnum.ShipMineralHold))
            End If
            If ParentFitting.FittedShip.Attributes.ContainsKey(AttributeEnum.ShipAmmoHold) Then
                If ParentFitting.FittedShip.Attributes(AttributeEnum.ShipAmmoHold) > mainBaySize Then
                    mainBaySize = ParentFitting.FittedShip.Attributes(AttributeEnum.ShipAmmoHold)
                End If
                bays.Add(Attributes.AttributeList(AttributeEnum.ShipAmmoHold).DisplayName, ParentFitting.FittedShip.Attributes(AttributeEnum.ShipAmmoHold))
            End If
            If ParentFitting.FittedShip.Attributes.ContainsKey(AttributeEnum.ShipPICommoditiesHold) Then
                If ParentFitting.FittedShip.Attributes(AttributeEnum.ShipPICommoditiesHold) > mainBaySize Then
                    mainBaySize = ParentFitting.FittedShip.Attributes(AttributeEnum.ShipPICommoditiesHold)
                End If
                bays.Add(Attributes.AttributeList(AttributeEnum.ShipPICommoditiesHold).DisplayName, ParentFitting.FittedShip.Attributes(AttributeEnum.ShipPICommoditiesHold))
            End If
            If ParentFitting.FittedShip.Attributes.ContainsKey(AttributeEnum.ShipCommandCenterHold) Then
                If ParentFitting.FittedShip.Attributes(AttributeEnum.ShipCommandCenterHold) > mainBaySize Then
                    mainBaySize = ParentFitting.FittedShip.Attributes(AttributeEnum.ShipCommandCenterHold)
                End If
                bays.Add(Attributes.AttributeList(AttributeEnum.ShipCommandCenterHold).DisplayName, ParentFitting.FittedShip.Attributes(AttributeEnum.ShipCommandCenterHold))
            End If
            If ParentFitting.FittedShip.ShipBay > 0 Then
                bays.Add("Ship Maintenance Bay Capacity", ParentFitting.FittedShip.ShipBay)
            End If
            For Each bay As KeyValuePair(Of String, Double) In bays
                ttt &= ControlChars.CrLf & bay.Key & ": " & bay.Value.ToString("N0") & " m3"
            Next
            lblCargoBay.Text = mainBaySize.ToString("N0") & " m3"
            ToolTip1.SetToolTip(lblCargoBay, ttt)
            ' Drones
            lblDroneBay.Text = ParentFitting.FittedShip.DroneBay.ToString("N0") & " m3"
            Select Case ParentFitting.FittedShip.DatabaseGroup
                Case ModuleEnum.GroupIndustrials, ModuleEnum.GroupDeepSpaceTransports, ModuleEnum.GroupMiningBarges, ModuleEnum.GroupFreighters, ModuleEnum.GroupExhumers, ModuleEnum.GroupJumpFreighters, ModuleEnum.GroupIndustrialCommandShips, ModuleEnum.GroupBlockadeRunners
                    epCargo.TitleText = "Storage (Cargo: " & mainBaySize.ToString("N0") & " m3)"
                Case Else
                    epCargo.TitleText = "Storage (Drone: " & ParentFitting.FittedShip.DroneBay.ToString("N0") & " m3)"
            End Select
            UpdateDroneUsage()

            ' Damage, Mining & Logistics
            If ParentFitting.FittedShip.TotalVolley >= ParentFitting.FittedShip.TransferAmount Then
                If ParentFitting.FittedShip.TotalVolley >= ParentFitting.FittedShip.OreTotalAmount + ParentFitting.FittedShip.IceTotalAmount + ParentFitting.FittedShip.GasTotalAmount Then
                    epDamage.TitleText = "Damage (DPS: " & ParentFitting.FittedShip.TotalDPS.ToString("N2") & ")"
                Else
                    epDamage.TitleText = "Mining (Rate: " & (ParentFitting.FittedShip.OreTotalRate + ParentFitting.FittedShip.IceTotalRate + ParentFitting.FittedShip.GasTotalRate).ToString("N2") & ")"
                End If
            ElseIf ParentFitting.FittedShip.TransferAmount > ParentFitting.FittedShip.OreTotalAmount + ParentFitting.FittedShip.IceTotalAmount + ParentFitting.FittedShip.GasTotalAmount Then
                epDamage.TitleText = "Logistics (Rate: " & ParentFitting.FittedShip.TransferRate.ToString("N2") & ")"
            Else
                epDamage.TitleText = "Mining (Rate: " & (ParentFitting.FittedShip.OreTotalRate + ParentFitting.FittedShip.IceTotalRate + ParentFitting.FittedShip.GasTotalRate).ToString("N2") & ")"
            End If
            ' Damage info
            lblDamage.Text = ParentFitting.FittedShip.TotalVolley.ToString("N2") & " / " & ParentFitting.FittedShip.TotalDPS.ToString("N2") & " DPS"
            If ParentFitting.FittedShip.TotalVolley > 0 Then
                ttt = ""
                If ParentFitting.FittedShip.TurretVolley > 0 Then
                    ttt &= "Turret Volley: " & ParentFitting.FittedShip.TurretVolley.ToString("N2")
                    ttt &= " (DPS: " & ParentFitting.FittedShip.TurretDPS.ToString("N2") & ")" & ControlChars.CrLf
                    turretShip = True
                End If
                If ParentFitting.FittedShip.MissileVolley > 0 Then
                    ttt &= "Missile Volley: " & ParentFitting.FittedShip.MissileVolley.ToString("N2")
                    ttt &= " (DPS: " & ParentFitting.FittedShip.MissileDPS.ToString("N2") & ")" & ControlChars.CrLf
                    missileShip = True
                End If
                If ParentFitting.FittedShip.SmartbombVolley > 0 Then
                    ttt &= "Smartbomb Volley: " & ParentFitting.FittedShip.SmartbombVolley.ToString("N2")
                    ttt &= " (DPS: " & ParentFitting.FittedShip.SmartbombDPS.ToString("N2") & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.DroneVolley > 0 Then
                    ttt &= "Drone Volley: " & ParentFitting.FittedShip.DroneVolley.ToString("N2")
                    ttt &= " (DPS: " & ParentFitting.FittedShip.DroneDPS.ToString("N2") & ")" & ControlChars.CrLf
                End If
                ToolTip1.SetToolTip(lblDamage, ttt)
                Dim dpr As HQFDefenceProfileResults = ParentFitting.CalculateDamageStatsForDefenceProfile(ParentFitting.FittedShip)
                lblDPR.Text = dpr.ShieldDPS.ToString("N2") & " / " & dpr.ArmorDPS.ToString("N2") & " / " & dpr.HullDPS.ToString("N2")
            End If
            ' Mining info
            lblMining.Text = (ParentFitting.FittedShip.OreTotalAmount + ParentFitting.FittedShip.IceTotalAmount + ParentFitting.FittedShip.GasTotalAmount).ToString("N2") & " m3 / " & (ParentFitting.FittedShip.OreTotalRate + ParentFitting.FittedShip.IceTotalRate + ParentFitting.FittedShip.GasTotalRate).ToString("N2") & " m3/s"
            If ParentFitting.FittedShip.OreTotalAmount > 0 Or ParentFitting.FittedShip.IceTotalAmount > 0 Or ParentFitting.FittedShip.GasTotalAmount > 0 Then
                ttt = ""
                If ParentFitting.FittedShip.OreTurretAmount > 0 Then
                    ttt &= "Mining Turret Yield: " & ParentFitting.FittedShip.OreTurretAmount.ToString("N2")
                    ttt &= " (m3/s: " & ParentFitting.FittedShip.OreTurretRate.ToString("N2") & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.OreDroneAmount > 0 Then
                    ttt &= "Mining Drone Yield: " & ParentFitting.FittedShip.OreDroneAmount.ToString("N2")
                    ttt &= " (m3/s: " & ParentFitting.FittedShip.OreDroneRate.ToString("N2") & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.IceTurretAmount > 0 Then
                    ttt &= "Ice Turret Yield: " & ParentFitting.FittedShip.IceTurretAmount.ToString("N2")
                    ttt &= " (m3/s: " & ParentFitting.FittedShip.IceTurretRate.ToString("N2") & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.IceDroneAmount > 0 Then
                    ttt &= "Ice Drone Yield: " & ParentFitting.FittedShip.IceDroneAmount.ToString("N2")
                    ttt &= " (m3/s: " & ParentFitting.FittedShip.IceDroneRate.ToString("N2") & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.GasTotalAmount > 0 Then
                    ttt &= "Gas Harvester Yield: " & ParentFitting.FittedShip.GasTotalAmount.ToString("N2")
                    ttt &= " (m3/s: " & ParentFitting.FittedShip.GasTotalRate.ToString("N2") & ")" & ControlChars.CrLf
                End If
                ToolTip1.SetToolTip(lblMining, ttt)
            End If
            ' Logistics info
            lblLogistics.Text = ParentFitting.FittedShip.TransferAmount.ToString("N2") & " HP / " & ParentFitting.FittedShip.TransferRate.ToString("N2") & " HP/s"
            If ParentFitting.FittedShip.TransferAmount > 0 Then
                ttt = ""
                If ParentFitting.FittedShip.ModuleTransferAmount > 0 Then
                    ttt &= "Module Transfer: " & ParentFitting.FittedShip.ModuleTransferAmount.ToString("N2")
                    ttt &= " (HP/s: " & ParentFitting.FittedShip.ModuleTransferRate.ToString("N2") & ")" & ControlChars.CrLf
                End If
                If ParentFitting.FittedShip.DroneTransferAmount > 0 Then
                    ttt &= "Drone Transfer: " & ParentFitting.FittedShip.DroneTransferAmount.ToString("N2")
                    ttt &= " (HP/s: " & ParentFitting.FittedShip.DroneTransferRate.ToString("N2") & ")" & ControlChars.CrLf
                End If
                ToolTip1.SetToolTip(lblLogistics, ttt)
            End If

            ' Collect List of Needed Skills
            _reqSkillsCollection = ParentFitting.CalculateNeededSkills(cboPilots.SelectedItem.ToString)
            If _reqSkillsCollection.TruePilotSkills.Count = 0 Then
                btnSkills.Image = My.Resources.Skills1
                ToolTip1.SetToolTip(btnSkills, "No additional skills required")
            Else
                If _reqSkillsCollection.ShipPilotSkills.Count = 0 Then
                    btnSkills.Image = My.Resources.Skills2
                    ToolTip1.SetToolTip(btnSkills, "HQF skills match, additional actual skills required - click to view")
                Else
                    btnSkills.Image = My.Resources.Skills0
                    ToolTip1.SetToolTip(btnSkills, "Additional skills required - click to view")
                End If
            End If

            ' Set buttons
            btnDamageAnalysis.Enabled = (turretShip Or missileShip)

            'Me.Refresh()
            
        End Sub

        Public Sub UpdateImplantList()
            If cboPilots.SelectedItem IsNot Nothing Then
                cboImplants.Tag = "Updating"
                Dim oldImplants As String
                Dim shipPilot As FittingPilot = FittingPilots.HQFPilots(cboPilots.SelectedItem.ToString)
                oldImplants = shipPilot.ImplantName(0)
                cboImplants.BeginUpdate()
                cboImplants.Items.Clear()
                cboImplants.Items.Add("*Custom*")
                For Each cImplantSet As String In PluginSettings.HQFSettings.ImplantGroups.Keys
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
            ParentFitting.FittedShip.DroneBandwidthUsed = 0
            For Each dbi As DroneBayItem In ParentFitting.FittedShip.DroneBayItems.Values
                If dbi.IsActive = True Then
                    ParentFitting.FittedShip.DroneBandwidthUsed += CDbl(dbi.DroneType.Attributes(1272)) * dbi.Quantity
                End If
            Next
            lblDroneBandwidth.Text = ParentFitting.FittedShip.DroneBandwidthUsed.ToString("N0") & " / " & ParentFitting.FittedShip.DroneBandwidth.ToString("N0")
            lblDroneControl.Text = ParentFitting.FittedShip.UsedDrones & " / " & ParentFitting.FittedShip.MaxDrones
            Dim ttt As String = "Drone Control Range: " & ParentFitting.FittedShip.Attributes(10007).ToString("N0") & "m"
            ToolTip1.SetToolTip(lblDroneControl, ttt)
            ToolTip1.SetToolTip(pbDroneControl, ttt)
        End Sub

#End Region

#Region "Damage Profile Routines"

        Private Sub cboDamageProfiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDamageProfiles.SelectedIndexChanged
            If cboDamageProfiles.SelectedItem IsNot Nothing Then
                ParentFitting.DamageProfileName = cboDamageProfiles.SelectedItem.ToString
            End If
            ' State damage profile info
            Dim info As New StringBuilder
            info.AppendLine(ParentFitting.BaseShip.DamageProfile.Name & " Profile Info:")
            info.AppendLine("EM: " & ParentFitting.BaseShip.DamageProfile.EM.ToString("N2"))
            info.AppendLine("Explosive: " & ParentFitting.BaseShip.DamageProfile.Explosive.ToString("N2"))
            info.AppendLine("Kinetic: " & ParentFitting.BaseShip.DamageProfile.Kinetic.ToString("N2"))
            info.AppendLine("Thermal: " & ParentFitting.BaseShip.DamageProfile.Thermal.ToString("N2"))
            info.AppendLine("DPS: " & ParentFitting.BaseShip.DamageProfile.DPS.ToString("N2"))
            ToolTip1.SetToolTip(cboDamageProfiles, info.ToString)
            ' Only perform this if we aren't setting the item at startup
            If _startUp = False Then
                ParentFitting.FittedShip.RecalculateEffectiveHP()
                ParentFitting.CalculateDefenceStatistics(ParentFitting.FittedShip)
                ParentFitting.ShipInfoCtrl.UpdateInfoDisplay()
            End If
        End Sub

        Private Sub btnEditProfiles_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditProfiles.Click
            ' Open options form
            Dim mySettings As New frmHQFSettings
            mySettings.Tag = "nodeDamageProfiles"
            mySettings.ShowDialog()
            mySettings.Dispose()
            Dim oldProfile As String = cboDamageProfiles.SelectedItem.ToString
            Call LoadDamageProfiles()
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
            For Each cProfile As String In HQFDamageProfiles.ProfileList.Keys
                cboDamageProfiles.Items.Add(cProfile)
            Next
            cboDamageProfiles.EndUpdate()
        End Sub

#End Region

#Region "Defence Profile Routines"

        Private Sub cboDefenceProfiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDefenceProfiles.SelectedIndexChanged
            If cboDefenceProfiles.SelectedItem IsNot Nothing Then
                ParentFitting.DefenceProfileName = cboDefenceProfiles.SelectedItem.ToString
            End If
            ' Only perform this if we aren't setting the item at startup
            If _startUp = False Then
                Dim dpr As HQFDefenceProfileResults = ParentFitting.CalculateDamageStatsForDefenceProfile(ParentFitting.FittedShip)
                lblDPR.Text = dpr.ShieldDPS.ToString("N2") & " / " & dpr.ArmorDPS.ToString("N2") & " / " & dpr.HullDPS.ToString("N2")
            End If
        End Sub

        Private Sub btnEditDefenceProfiles_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditDefenceProfiles.Click
            ' Open options form
            Dim mySettings As New frmHQFSettings
            mySettings.Tag = "nodeDefenceProfiles"
            mySettings.ShowDialog()
            mySettings.Dispose()
            Dim oldProfile As String = ""
            If cboDefenceProfiles.SelectedItem IsNot Nothing Then
                oldProfile = cboDefenceProfiles.SelectedItem.ToString
            End If
            Call LoadDefenceProfiles()
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
            For Each cProfile As String In HQFDefenceProfiles.ProfileList.Keys
                cboDefenceProfiles.Items.Add(cProfile)
            Next
            cboDefenceProfiles.EndUpdate()
        End Sub

#End Region

#Region "UI Routines"

        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilots.SelectedIndexChanged
            ' Build the Affections data for this pilot
            Dim shipPilot As FittingPilot = FittingPilots.HQFPilots(cboPilots.SelectedItem.ToString)
            ParentFitting.PilotName = shipPilot.PilotName
            cboImplants.Tag = "Updating"
            If shipPilot.ImplantName(0) IsNot Nothing Then
                cboImplants.SelectedItem = shipPilot.ImplantName(0)
            End If
            cboImplants.Tag = Nothing
            ' Only perform this if we aren't setting the item at startup
            If _startUp = False Then
                ParentFitting.ApplyFitting(BuildType.BuildEverything)
                HQFEvents.StartUpdateModuleList = True
            End If
        End Sub

        Private Sub cboImplants_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboImplants.SelectedIndexChanged
            Dim shipPilot As FittingPilot = FittingPilots.HQFPilots(cboPilots.SelectedItem.ToString)
            ' Update the pilot's implants?
            shipPilot.ImplantName(0) = cboImplants.SelectedItem.ToString
            Dim implantList As New StringBuilder
            If cboImplants.SelectedItem.ToString <> "*Custom*" Then
                If PluginSettings.HQFSettings.ImplantGroups.ContainsKey(cboImplants.SelectedItem.ToString) Then
                    Dim currentImplantGroup As ImplantCollection = PluginSettings.HQFSettings.ImplantGroups(cboImplants.SelectedItem.ToString)
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
            If _startUp = False Then
                If cboImplants.Tag Is Nothing Then
                    ParentFitting.ApplyFitting(BuildType.BuildEverything)
                    HQFEvents.StartUpdateModuleList = True
                End If
            End If
        End Sub

        Private Sub btnSkills_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSkills.Click
            If _reqSkillsCollection.TruePilotSkills.Count > 0 Then
                Dim myRequiredSkills As New frmRequiredSkills(ParentFitting.FittingName)
                myRequiredSkills.Pilot = HQ.Settings.Pilots(cboPilots.SelectedItem.ToString)
                myRequiredSkills.Skills = _reqSkillsCollection.TruePilotSkills
                myRequiredSkills.ShowDialog()
            End If
        End Sub

        Private Sub btnLog_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLog.Click
            Dim myAuditLog As New frmShipAudit
            Dim logData() As String
            Dim newLog As ListViewItem
            myAuditLog.lvwAudit.BeginUpdate()
            For Each log As String In ParentFitting.FittedShip.AuditLog
                logData = log.Split("#".ToCharArray)
                'If logData(2).Trim <> logData(3).Trim Then
                newLog = New ListViewItem
                newLog.Text = logData(0).Trim
                newLog.SubItems.Add(logData(1).Trim)
                newLog.SubItems.Add(CDbl(logData(2).Trim).ToString("N3"))
                newLog.SubItems.Add(CDbl(logData(3).Trim).ToString("N3"))
                myAuditLog.lvwAudit.Items.Add(newLog)
                'End If
            Next
            myAuditLog.lvwAudit.EndUpdate()
            myAuditLog.ShowDialog()
            myAuditLog.Dispose()
        End Sub

        Private Sub btnTargetSpeed_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTargetSpeed.Click
            Dim targetSpeed As New FrmTargetSpeed
            targetSpeed.ShipType = ParentFitting.FittedShip
            targetSpeed.Dispose()
        End Sub

        Private Sub btnDamageAnalysis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDamageAnalysis.Click
            Dim myDa As New FrmDamageAnalysis
            If cboPilots.SelectedItem IsNot Nothing Then
                myDa.PilotName = cboPilots.SelectedItem.ToString
            End If
            If ParentFitting.ShipSlotCtrl IsNot Nothing Then
                myDa.FittingName = ParentFitting.FittingName
            End If
            myDa.ShowDialog()
            myDa.Dispose()
        End Sub

        Private Sub btnCapSim_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCapSim.Click
            Dim capSimForm As New frmCapSim(ParentFitting.FittedShip)
            capSimForm.ShowDialog()
            capSimForm.Dispose()
        End Sub

        Private Sub epDefence_ExpandedChanged(sender As Object, e As ExpandedChangeEventArgs) Handles epDefence.ExpandedChanged
            If _startUp = False Then
                PluginSettings.HQFSettings.DefensePanelIsCollapsed = Not epDefence.Expanded
                pnlInfo.Invalidate()
            End If
        End Sub

        Private Sub epCapacitor_ExpandedChanged(sender As Object, e As ExpandedChangeEventArgs) Handles epCapacitor.ExpandedChanged
            If _startUp = False Then
                PluginSettings.HQFSettings.CapacitorPanelIsCollapsed = Not epCapacitor.Expanded
                pnlInfo.Invalidate()
            End If
        End Sub

        Private Sub epDamage_ExpandedChanged(sender As Object, e As ExpandedChangeEventArgs) Handles epDamage.ExpandedChanged
            If _startUp = False Then
                PluginSettings.HQFSettings.DamagePanelIsCollapsed = Not epDamage.Expanded
                pnlInfo.Invalidate()
            End If
        End Sub

        Private Sub epTargeting_ExpandedChanged(sender As Object, e As ExpandedChangeEventArgs) Handles epTargeting.ExpandedChanged
            If _startUp = False Then
                PluginSettings.HQFSettings.TargetingPanelIsCollapsed = Not epTargeting.Expanded
                pnlInfo.Invalidate()
            End If
        End Sub

        Private Sub epPropulsion_ExpandedChanged(sender As Object, e As ExpandedChangeEventArgs) Handles epPropulsion.ExpandedChanged
            If _startUp = False Then
                PluginSettings.HQFSettings.PropulsionPanelIsCollapsed = Not epPropulsion.Expanded
                pnlInfo.Invalidate()
            End If
        End Sub

        Private Sub epCargo_ExpandedChanged(sender As Object, e As ExpandedChangeEventArgs) Handles epCargo.ExpandedChanged
            If _startUp = False Then
                PluginSettings.HQFSettings.CargoPanelIsCollapsed = Not epCargo.Expanded
                pnlInfo.Invalidate()
            End If
        End Sub

#End Region

    End Class
End NameSpace