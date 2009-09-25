' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
    Private fittedShip As Ship ' Should be the modified ship after passing through the engine
    Private reqSkills As New SortedList ' Lists the skills required for the ship/mods/drones
#End Region

#Region "Property Variables"
    Private currentShip As Ship ' Should be the base ship only
    Private currentSlot As ShipSlotControl
    Private cBuildMethod As Integer = 0 ' Engine build method
#End Region

#Region "Properties"

    Public Property ShipType() As Ship
        Get
            Return currentShip
        End Get
        Set(ByVal value As Ship)
            If value IsNot Nothing Then ' May have been triggered by an initial change in the pilot on form startup
                currentShip = value
            End If
        End Set
    End Property

    Public Property ShipSlot() As ShipSlotControl
        Get
            Return currentSlot
        End Get
        Set(ByVal value As ShipSlotControl)
            currentSlot = value
        End Set
    End Property

    Public Property BuildMethod() As Integer
        Get
            Return cBuildMethod
        End Get
        Set(ByVal value As Integer)
            cBuildMethod = value
            If currentShip IsNot Nothing Then
                currentShip = Engine.BuildSubSystemEffects(currentShip)
                ShipSlot.ShipCurrent = currentShip
                If cboPilots.SelectedItem IsNot Nothing Then
                    Select Case cBuildMethod
                        Case BuildType.BuildEverything
                            Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
                            If cboImplants.SelectedItem IsNot Nothing Then
                                shipPilot.ImplantName(0) = cboImplants.SelectedItem.ToString
                            End If
                            fittedShip = Engine.ApplyFitting(CType(currentShip.Clone, Ship), shipPilot, cBuildMethod)
                            Call UpdateInfoDisplay()
                            currentSlot.ShipFitted = fittedShip
                        Case BuildType.BuildFromEffectsMaps
                            Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
                            If cboImplants.SelectedItem IsNot Nothing Then
                                shipPilot.ImplantName(0) = cboImplants.SelectedItem.ToString
                            End If
                            fittedShip = Engine.ApplyFitting(CType(currentShip.Clone, Ship), shipPilot, cBuildMethod)
                            Call UpdateInfoDisplay()
                            currentSlot.ShipFitted = fittedShip
                        Case BuildType.BuildEffectsMaps
                            Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
                            If cboImplants.SelectedItem IsNot Nothing Then
                                shipPilot.ImplantName(0) = cboImplants.SelectedItem.ToString
                            End If
                            Engine.ApplyFitting(CType(currentShip.Clone, Ship), shipPilot, cBuildMethod)
                    End Select
                End If
            End If
        End Set
    End Property

#End Region

    Private Sub UpdateInfoDisplay()
        ' Update the display with the information about the (fitted) ship
        Dim ttt As String = "" ' Fot tool tip text!
        Dim ccc As Double = 0 ' For capacitor sustainability
        Dim turretShip As Boolean = False
        Dim missileShip As Boolean = False

        ' CPU
        If fittedShip.CPU > 0 Then
            progCPU.MaxValue = CInt(fittedShip.CPU)
        Else
            progCPU.MaxValue = 1
        End If
        progCPU.Value = Math.Min(CInt(fittedShip.CPU_Used), CInt(fittedShip.CPU))
        If Math.Round(fittedShip.CPU_Used, 3) > fittedShip.CPU Then
            progCPU.StartColor = Drawing.Color.Red
            progCPU.EndColor = Drawing.Color.Red
            progCPU.HighlightColor = Drawing.Color.White
            progCPU.GlowColor = Drawing.Color.LightPink
            lblCPUReqd.Text = FormatNumber((fittedShip.CPU_Used - fittedShip.CPU) / fittedShip.CPU * 100, 2) & "%"
            lblCPUReqd.ForeColor = Drawing.Color.Red
        Else
            progCPU.StartColor = Drawing.Color.LimeGreen
            progCPU.EndColor = Drawing.Color.LimeGreen
            progCPU.HighlightColor = Drawing.Color.White
            progCPU.GlowColor = Drawing.Color.LightGreen
            lblCPUReqd.Text = ""
        End If
        progCPU.Refresh()
        lblCPU.Text = FormatNumber(fittedShip.CPU_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.CPU, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Powergrid
        If fittedShip.PG > 0 Then
            progPG.MaxValue = CInt(fittedShip.PG)
        Else
            progPG.MaxValue = 1
        End If
        progPG.Value = Math.Min(CInt(fittedShip.PG_Used), CInt(fittedShip.PG))
        If Math.Round(fittedShip.PG_Used, 3) > fittedShip.PG Then
            progPG.StartColor = Drawing.Color.Red
            progPG.EndColor = Drawing.Color.Red
            progPG.HighlightColor = Drawing.Color.White
            progPG.GlowColor = Drawing.Color.LightPink
            lblPGReqd.Text = FormatNumber((fittedShip.PG_Used - fittedShip.PG) / fittedShip.PG * 100, 2) & "%"
            lblPGReqd.ForeColor = Drawing.Color.Red
        Else
            progPG.StartColor = Drawing.Color.LimeGreen
            progPG.EndColor = Drawing.Color.LimeGreen
            progPG.HighlightColor = Drawing.Color.White
            progPG.GlowColor = Drawing.Color.LightGreen
            lblPGReqd.Text = ""
        End If
        progPG.Refresh()
        lblPG.Text = FormatNumber(fittedShip.PG_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.PG, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Calibration
        If fittedShip.Calibration > 0 Then
            progCalibration.MaxValue = CInt(fittedShip.Calibration)
        Else
            progCalibration.MaxValue = 1
        End If
        progCalibration.Value = Math.Min(CInt(fittedShip.Calibration_Used), CInt(fittedShip.Calibration))
        If fittedShip.Calibration_Used > fittedShip.Calibration Then
            progCalibration.StartColor = Drawing.Color.Red
            progCalibration.EndColor = Drawing.Color.Red
            progCalibration.HighlightColor = Drawing.Color.White
            progCalibration.GlowColor = Drawing.Color.LightPink
        Else
            progCalibration.StartColor = Drawing.Color.LimeGreen
            progCalibration.EndColor = Drawing.Color.LimeGreen
            progCalibration.HighlightColor = Drawing.Color.White
            progCalibration.GlowColor = Drawing.Color.LightGreen
        End If
        progCalibration.Refresh()
        lblCalibration.Text = FormatNumber(fittedShip.Calibration_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.Calibration, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Shield
        lblShieldHP.Text = FormatNumber(fittedShip.ShieldCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        'lblShieldRecharge.Text = FormatNumber(fittedShip.ShieldRecharge, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        lblShieldEM.Text = FormatNumber(fittedShip.ShieldEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldExplosive.Text = FormatNumber(fittedShip.ShieldExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldKinetic.Text = FormatNumber(fittedShip.ShieldKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldThermal.Text = FormatNumber(fittedShip.ShieldThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        'gbShield.Text = "Shield  (Effective HP: " & FormatNumber(fittedShip.EffectiveShieldHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")"
        'lblShieldAverage.Text = FormatNumber(fittedShip.ShieldCapacity / fittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP/s"
        'lblShieldPeak.Text = FormatNumber(HQF.Settings.HQFSettings.ShieldRechargeConstant * fittedShip.ShieldCapacity / fittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP/s"
        ttt = "Shield Hitpoints: " & lblShieldHP.Text & ControlChars.CrLf
        ttt &= "Effective Hitpoints: " & FormatNumber(fittedShip.EffectiveShieldHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ControlChars.CrLf
        ttt &= "Recharge Time: " & FormatNumber(fittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s" & ControlChars.CrLf
        ttt &= "Average Recharge Rate: " & FormatNumber(fittedShip.ShieldCapacity / fittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP/s" & ControlChars.CrLf
        ttt &= "Peak Recharge Rate: " & FormatNumber(HQF.Settings.HQFSettings.ShieldRechargeConstant * fittedShip.ShieldCapacity / fittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP/s"
        ToolTip1.SetToolTip(pbShieldHP, ttt)
        ToolTip1.SetToolTip(lblShieldHP, ttt)

        ' Armor
        lblArmorHP.Text = FormatNumber(fittedShip.ArmorCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblArmorEM.Text = FormatNumber(fittedShip.ArmorEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorExplosive.Text = FormatNumber(fittedShip.ArmorExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorKinetic.Text = FormatNumber(fittedShip.ArmorKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorThermal.Text = FormatNumber(fittedShip.ArmorThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        ttt = "Armor Hitpoints: " & lblArmorHP.Text & ControlChars.CrLf
        ttt &= "Effective Hitpoints: " & FormatNumber(fittedShip.EffectiveArmorHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ToolTip1.SetToolTip(pbArmorHP, ttt)
        ToolTip1.SetToolTip(lblArmorHP, ttt)

        ' Structure
        lblStructureHP.Text = FormatNumber(fittedShip.StructureCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblStructureEM.Text = FormatNumber(fittedShip.StructureEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureExplosive.Text = FormatNumber(fittedShip.StructureExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureKinetic.Text = FormatNumber(fittedShip.StructureKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureThermal.Text = FormatNumber(fittedShip.StructureThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        ttt = "Structure Hitpoints: " & lblStructureHP.Text & ControlChars.CrLf
        ttt &= "Effective Hitpoints: " & FormatNumber(fittedShip.EffectiveStructureHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ToolTip1.SetToolTip(pbStructureHP, ttt)
        ToolTip1.SetToolTip(lblStructureHP, ttt)

        ' EffectiveHP
        lblEffectiveHP.Text = "Effective HP: " & FormatNumber(fittedShip.EffectiveHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Tank Ability
        lblTankAbility.Text = "Tank Ability: " & CDbl(fittedShip.Attributes("10062")).ToString("N2") & " DPS"
        ttt = "Passive Tank: " & CDbl(fittedShip.Attributes("10069")).ToString("N2") & " DPS" & ControlChars.CrLf
        ttt &= "Shield Tank: " & CDbl(fittedShip.Attributes("10059")).ToString("N2") & " DPS" & ControlChars.CrLf
        ttt &= "Armor Tank: " & CDbl(fittedShip.Attributes("10060")).ToString("N2") & " DPS" & ControlChars.CrLf
        ttt &= "Structure Tank: " & CDbl(fittedShip.Attributes("10061")).ToString("N2") & " DPS" & ControlChars.CrLf
        ttt &= ControlChars.CrLf
        ttt &= "Damage Profile DPS: " & FormatNumber(fittedShip.DamageProfile.DPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " DPS" & ControlChars.CrLf
        If CDbl(fittedShip.Attributes("10062")) >= fittedShip.DamageProfile.DPS Then
            ttt &= "Can tank damage profile"
            lblTankAbility.ForeColor = Drawing.Color.LimeGreen
        Else
            ttt &= "Not able to tank damage profile"
            lblTankAbility.ForeColor = Drawing.Color.Red
        End If
        ToolTip1.SetToolTip(lblTankAbility, ttt)

        ' Capacitor
        lblCapacitor.Text = FormatNumber(fittedShip.CapCapacity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " GJ"
        lblCapRecharge.Text = FormatNumber(fittedShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        'lblCapAverage.Text = FormatNumber(fittedShip.CapCapacity / fittedShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " GJ/s"
        lblCapPeak.Text = FormatNumber(HQF.Settings.HQFSettings.CapRechargeConstant * fittedShip.CapCapacity / fittedShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " GJ/s"
        lblCapBalP.Text = "+" & FormatNumber((CDbl(fittedShip.Attributes("10050")) * -1) + (HQF.Settings.HQFSettings.CapRechargeConstant * fittedShip.CapCapacity / fittedShip.CapRecharge), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCapBalN.Text = "- " & FormatNumber(fittedShip.Attributes("10049"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ccc = Engine.CalculateCapStatistics(fittedShip)
        If ccc > 0 Then
            gbCapacitor.Text = "Capacitor: Stable at " & FormatNumber(ccc / fittedShip.CapCapacity * 100, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        Else
            gbCapacitor.Text = "Capacitor: Lasts " & EveHQ.Core.SkillFunctions.TimeToString(-ccc, False)
        End If

        ' Propulsion
        lblSpeed.Text = FormatNumber(fittedShip.MaxVelocity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m/s"
        lblWarpSpeed.Text = FormatNumber(fittedShip.WarpSpeed, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " au/s"
        ttt = "Warp Capacitor Need: " & fittedShip.WarpCapNeed & ControlChars.CrLf
        ttt &= "Max Warp Distance: " & FormatNumber(fittedShip.CapCapacity / (fittedShip.WarpCapNeed * fittedShip.Mass), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " au"
        ToolTip1.SetToolTip(lblWarpSpeed, ttt)
        lblInertia.Text = FormatNumber(fittedShip.Inertia, 6, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ' Time to warp based on calculation from http://myeve.eve-online.com/ingameboard.asp?a=topic&threadID=502836
        ' Time to warp (in seconds) = Inertial Modifier * (Mass / 1.000.000) * 1.61
        '-ln(0.25) * Inertia Modifier * Mass / 1.000.000
        lblAlignTime.Text = FormatNumber(-Math.Log(0.25) * fittedShip.Inertia * fittedShip.Mass / 1000000, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"

        ' Targeting
        lblTargetRange.Text = FormatNumber(fittedShip.MaxTargetRange, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m"
        lblTargets.Text = FormatNumber(fittedShip.MaxLockedTargets, 0) & " / " & FormatNumber(CDbl(fittedShip.Attributes("10064")), 0)
        ttt = "Max Ship Targets: " & FormatNumber(fittedShip.MaxLockedTargets, 0) & ControlChars.CrLf
        ttt &= "Max Pilot Targets: " & FormatNumber(CDbl(fittedShip.Attributes("10064")), 0)
        ToolTip1.SetToolTip(lblTargets, ttt)
        lblScanResolution.Text = FormatNumber(fittedShip.ScanResolution, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " mm"
        lblSensorStrength.Text = FormatNumber(Math.Max(Math.Max(Math.Max(fittedShip.GravSensorStrenth, fittedShip.LadarSensorStrenth), fittedShip.MagSensorStrenth), fittedShip.RadarSensorStrenth), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ttt = "Gravimetric Strength: " & FormatNumber(fittedShip.GravSensorStrenth, 2) & ControlChars.CrLf
        ttt &= "Ladar Strength: " & FormatNumber(fittedShip.LadarSensorStrenth, 2) & ControlChars.CrLf
        ttt &= "Magnetometric Strength: " & FormatNumber(fittedShip.MagSensorStrenth, 2) & ControlChars.CrLf
        ttt &= "Radar Strength: " & FormatNumber(fittedShip.RadarSensorStrenth, 2) & ControlChars.CrLf
        ToolTip1.SetToolTip(lblSensorStrength, ttt)
        lblSigRadius.Text = FormatNumber(fittedShip.SigRadius, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m"
        ' Cargo and Drones
        lblCargoBay.Text = FormatNumber(fittedShip.CargoBay, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"
        lblDroneBay.Text = FormatNumber(fittedShip.DroneBay, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"
        UpdateDroneUsage()

        ' Damage & Mining
        lblDamage.Text = FormatNumber(fittedShip.TotalVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.TotalDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " DPS"
        If fittedShip.TotalVolley > 0 Then
            ttt = ""
            If fittedShip.TurretVolley > 0 Then
                ttt &= "Turret Volley: " & FormatNumber(fittedShip.TurretVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ttt &= " (DPS: " & FormatNumber(fittedShip.TurretDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                turretShip = True
            End If
            If fittedShip.MissileVolley > 0 Then
                ttt &= "Missile Volley: " & FormatNumber(fittedShip.MissileVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ttt &= " (DPS: " & FormatNumber(fittedShip.MissileDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
                missileShip = True
            End If
            If fittedShip.SBVolley > 0 Then
                ttt &= "Smartbomb Volley: " & FormatNumber(fittedShip.SBVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ttt &= " (DPS: " & FormatNumber(fittedShip.SBDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
            End If
            If fittedShip.DroneVolley > 0 Then
                ttt &= "Drone Volley: " & FormatNumber(fittedShip.DroneVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ttt &= " (DPS: " & FormatNumber(fittedShip.DroneDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
            End If
            ToolTip1.SetToolTip(lblDamage, ttt)
        End If

        ' Mining
        lblMining.Text = FormatNumber(fittedShip.OreTotalAmount + fittedShip.IceTotalAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3 / " & FormatNumber(fittedShip.OreTotalRate + fittedShip.IceTotalRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3/s"
        If fittedShip.OreTotalAmount > 0 Or fittedShip.IceTotalAmount > 0 Then
            ttt = ""
            If fittedShip.OreTurretAmount > 0 Then
                ttt &= "Mining Turret Yield: " & FormatNumber(fittedShip.OreTurretAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ttt &= " (m3/s: " & FormatNumber(fittedShip.OreTurretRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
            End If
            If fittedShip.OreDroneAmount > 0 Then
                ttt &= "Mining Drone Yield: " & FormatNumber(fittedShip.OreDroneAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ttt &= " (m3/s: " & FormatNumber(fittedShip.OreDroneRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
            End If
            If fittedShip.IceTurretAmount > 0 Then
                ttt &= "Ice Turret Yield: " & FormatNumber(fittedShip.IceTurretAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ttt &= " (m3/s: " & FormatNumber(fittedShip.IceTurretRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
            End If
            If fittedShip.IceDroneAmount > 0 Then
                ttt &= "Ice Drone Yield: " & FormatNumber(fittedShip.IceDroneAmount, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ttt &= " (m3/s: " & FormatNumber(fittedShip.IceDroneRate, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")" & ControlChars.CrLf
            End If
            ToolTip1.SetToolTip(lblMining, ttt)
        End If

        ' Collect List of Needed Skills
        reqSkills = NeededSkills()
        If reqSkills.Count = 0 Then
            btnSkills.Image = My.Resources.Skills1
        Else
            btnSkills.Image = My.Resources.Skills0
        End If

        ' Set buttons
        btnDamageAnalysis.Enabled = (turretShip Or missileShip)

    End Sub

    Private Sub btnDoomsdayCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoomsdayCheck.Click
        Dim DDCheck As New frmDoomsday
        DDCheck.ShipType = fittedShip
        DDCheck.ShowDialog()
        DDCheck = Nothing
    End Sub

    Public Sub New(ByVal ShipFit As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

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
        Dim fit As ArrayList = CType(Fittings.FittingList(ShipFit), ArrayList)
        If cboPilots.Items.Count > 0 Then
            If fit.Count > 0 Then
                If fit(0).ToString.StartsWith("#Pilot#") And Settings.HQFSettings.UseLastPilot = True Then
                    Dim selPilot As String = fit(0).ToString.TrimStart("#Pilot#".ToCharArray)
                    If cboPilots.Items.Contains(selPilot) = True Then
                        cboPilots.SelectedItem = selPilot
                    Else
                        If cboPilots.Items.Contains(HQF.Settings.HQFSettings.DefaultPilot) = True Then
                            cboPilots.SelectedItem = HQF.Settings.HQFSettings.DefaultPilot
                        Else
                            cboPilots.SelectedIndex = 0
                        End If
                    End If
                Else
                    ' Look at the settings for default pilot
                    If cboPilots.Items.Contains(HQF.Settings.HQFSettings.DefaultPilot) = True Then
                        cboPilots.SelectedItem = HQF.Settings.HQFSettings.DefaultPilot
                    Else
                        cboPilots.SelectedIndex = 0
                    End If
                End If
            Else
                ' Look at the settings for default pilot
                If cboPilots.Items.Contains(HQF.Settings.HQFSettings.DefaultPilot) = True Then
                    cboPilots.SelectedItem = HQF.Settings.HQFSettings.DefaultPilot
                Else
                    cboPilots.SelectedIndex = 0
                End If
            End If
        End If

        ' Add the current list of implants to the combobox
        Call Me.UpdateImplantList()

        Call Me.LoadProfiles()
        ' Set the default damage profile
        cboDamageProfiles.SelectedItem = "<Omni-Damage>"
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
            For Each cImplantSet As String In HQF.Implants.implantGroups.Keys
                cboImplants.Items.Add(cImplantSet)
            Next
            If cboImplants.Items.Contains(oldImplants) Then
                cboImplants.SelectedItem = oldImplants
            End If
            cboImplants.EndUpdate()
            cboImplants.Tag = Nothing
        End If
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        ' Build the Affections data for this pilot
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
        ' Call the property modifier again which will trigger the fitting routines and update all slots for the new pilot
        If currentSlot IsNot Nothing Then
            currentSlot.UpdateAllSlots = True
        End If
        cboImplants.Tag = "Updating"
        If shipPilot.ImplantName(0) IsNot Nothing Then
            cboImplants.SelectedItem = shipPilot.ImplantName(0)
        End If
        cboImplants.Tag = Nothing
        BuildMethod = BuildType.BuildEverything
        If currentSlot IsNot Nothing Then
            currentSlot.UpdateAllSlots = False
        End If
        HQFEvents.StartUpdateModuleList = True
    End Sub

    Private Sub cboImplants_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboImplants.SelectedIndexChanged
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
        ' Update the pilot's implants?
        'shipPilot.ImplantName(0) = cboImplants.SelectedItem.ToString
        Dim implantList As New System.Text.StringBuilder
        If cboImplants.SelectedItem.ToString <> "*Custom*" Then
            Dim currentImplantGroup As ImplantGroup = CType(Implants.implantGroups(cboImplants.SelectedItem.ToString), ImplantGroup)
            For imp As Integer = 1 To 10
                If currentImplantGroup.ImplantName(imp) = "" Then
                    implantList.AppendLine("Slot " & imp.ToString & ": <Empty>")
                Else
                    implantList.AppendLine("Slot " & imp.ToString & ": " & currentImplantGroup.ImplantName(imp))
                End If
            Next
        Else
            For imp As Integer = 1 To 10
                If shipPilot.ImplantName(imp) = "" Then
                    implantList.AppendLine("Slot " & imp.ToString & ": <Empty>")
                Else
                    implantList.AppendLine("Slot " & imp.ToString & ": " & shipPilot.ImplantName(imp))
                End If
            Next
        End If
        If cboImplants.Tag Is Nothing Then
            If currentSlot IsNot Nothing Then
                currentSlot.UpdateAllSlots = True
            End If
            BuildMethod = BuildType.BuildEverything
            If currentSlot IsNot Nothing Then
                currentSlot.UpdateAllSlots = False
            End If
            HQFEvents.StartUpdateModuleList = True
        End If
        ToolTip1.SetToolTip(cboImplants, implantList.ToString)
    End Sub

    Private Sub btnTargetSpeed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTargetSpeed.Click
        Dim targetSpeed As New frmTargetSpeed
        targetSpeed.ShipType = fittedShip
        targetSpeed = Nothing
    End Sub

    Private Function NeededSkills() As SortedList
        Dim nSkills As New SortedList
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
        Dim mySkill As HQFSkill
        Dim rSkill As ReqSkill
        Dim count As Integer = 0
        For Each nSkill As ItemSkills In fittedShip.RequiredSkills.Values
            count += 1
            If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                If mySkill.Level < nSkill.Level Then
                    rSkill = New ReqSkill
                    rSkill.Name = nSkill.Name
                    rSkill.ID = nSkill.ID
                    rSkill.ReqLevel = nSkill.Level
                    rSkill.CurLevel = mySkill.Level
                    rSkill.NeededFor = fittedShip.Name
                    nSkills.Add("Ship" & count.ToString, rSkill)
                End If
            Else
                rSkill = New ReqSkill
                rSkill.Name = nSkill.Name
                rSkill.ID = nSkill.ID
                rSkill.ReqLevel = nSkill.Level
                rSkill.CurLevel = 0
                rSkill.NeededFor = fittedShip.Name
                nSkills.Add("Ship" & count.ToString, rSkill)
            End If
        Next
        For slot As Integer = 1 To fittedShip.HiSlots
            If fittedShip.HiSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In fittedShip.HiSlot(slot).RequiredSkills.Values
                    count += 1
                    If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                        mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                        If mySkill.Level < nSkill.Level Then
                            rSkill = New ReqSkill
                            rSkill.Name = nSkill.Name
                            rSkill.ID = nSkill.ID
                            rSkill.ReqLevel = nSkill.Level
                            rSkill.CurLevel = mySkill.Level
                            rSkill.NeededFor = fittedShip.HiSlot(slot).Name
                            nSkills.Add("HiSlot" & slot.ToString & count.ToString, rSkill)
                        End If
                    Else
                        rSkill = New ReqSkill
                        rSkill.Name = nSkill.Name
                        rSkill.ID = nSkill.ID
                        rSkill.ReqLevel = nSkill.Level
                        rSkill.CurLevel = 0
                        rSkill.NeededFor = fittedShip.HiSlot(slot).Name
                        nSkills.Add("HiSlot" & slot.ToString & count.ToString, rSkill)
                    End If
                Next
                If fittedShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    count = 0
                    For Each nSkill As ItemSkills In fittedShip.HiSlot(slot).LoadedCharge.RequiredSkills.Values
                        count += 1
                        If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                            mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                            If mySkill.Level < nSkill.Level Then
                                rSkill = New ReqSkill
                                rSkill.Name = nSkill.Name
                                rSkill.ID = nSkill.ID
                                rSkill.ReqLevel = nSkill.Level
                                rSkill.CurLevel = mySkill.Level
                                rSkill.NeededFor = fittedShip.HiSlot(slot).LoadedCharge.Name
                                nSkills.Add("HiSlot Charge" & slot.ToString & count.ToString, rSkill)
                            End If
                        Else
                            rSkill = New ReqSkill
                            rSkill.Name = nSkill.Name
                            rSkill.ID = nSkill.ID
                            rSkill.ReqLevel = nSkill.Level
                            rSkill.CurLevel = 0
                            rSkill.NeededFor = fittedShip.HiSlot(slot).LoadedCharge.Name
                            nSkills.Add("HiSlot Charge" & slot.ToString & count.ToString, rSkill)
                        End If
                    Next
                End If
            End If
        Next
        For slot As Integer = 1 To fittedShip.MidSlots
            If fittedShip.MidSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In fittedShip.MidSlot(slot).RequiredSkills.Values
                    count += 1
                    If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                        mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                        If mySkill.Level < nSkill.Level Then
                            rSkill = New ReqSkill
                            rSkill.Name = nSkill.Name
                            rSkill.ID = nSkill.ID
                            rSkill.ReqLevel = nSkill.Level
                            rSkill.CurLevel = mySkill.Level
                            rSkill.NeededFor = fittedShip.MidSlot(slot).Name
                            nSkills.Add("MidSlot" & slot.ToString & count.ToString, rSkill)
                        End If
                    Else
                        rSkill = New ReqSkill
                        rSkill.Name = nSkill.Name
                        rSkill.ID = nSkill.ID
                        rSkill.ReqLevel = nSkill.Level
                        rSkill.CurLevel = 0
                        rSkill.NeededFor = fittedShip.MidSlot(slot).Name
                        nSkills.Add("MidSlot" & slot.ToString & count.ToString, rSkill)
                    End If
                Next
                If fittedShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    count = 0
                    For Each nSkill As ItemSkills In fittedShip.MidSlot(slot).LoadedCharge.RequiredSkills.Values
                        count += 1
                        If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                            mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                            If mySkill.Level < nSkill.Level Then
                                rSkill = New ReqSkill
                                rSkill.Name = nSkill.Name
                                rSkill.ID = nSkill.ID
                                rSkill.ReqLevel = nSkill.Level
                                rSkill.CurLevel = mySkill.Level
                                rSkill.NeededFor = fittedShip.MidSlot(slot).LoadedCharge.Name
                                nSkills.Add("MidSlot Charge" & slot.ToString & count.ToString, rSkill)
                            End If
                        Else
                            rSkill = New ReqSkill
                            rSkill.Name = nSkill.Name
                            rSkill.ID = nSkill.ID
                            rSkill.ReqLevel = nSkill.Level
                            rSkill.CurLevel = 0
                            rSkill.NeededFor = fittedShip.MidSlot(slot).LoadedCharge.Name
                            nSkills.Add("MidSlot Charge" & slot.ToString & count.ToString, rSkill)
                        End If
                    Next
                End If
            End If
        Next
        For slot As Integer = 1 To fittedShip.LowSlots
            If fittedShip.LowSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In fittedShip.LowSlot(slot).RequiredSkills.Values
                    count += 1
                    If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                        mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                        If mySkill.Level < nSkill.Level Then
                            rSkill = New ReqSkill
                            rSkill.Name = nSkill.Name
                            rSkill.ID = nSkill.ID
                            rSkill.ReqLevel = nSkill.Level
                            rSkill.CurLevel = mySkill.Level
                            rSkill.NeededFor = fittedShip.LowSlot(slot).Name
                            nSkills.Add("LowSlot" & slot.ToString & count.ToString, rSkill)
                        End If
                    Else
                        rSkill = New ReqSkill
                        rSkill.Name = nSkill.Name
                        rSkill.ID = nSkill.ID
                        rSkill.ReqLevel = nSkill.Level
                        rSkill.CurLevel = 0
                        rSkill.NeededFor = fittedShip.LowSlot(slot).Name
                        nSkills.Add("LowSlot" & slot.ToString & count.ToString, rSkill)
                    End If
                Next
                If fittedShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    count = 0
                    For Each nSkill As ItemSkills In fittedShip.LowSlot(slot).LoadedCharge.RequiredSkills.Values
                        count += 1
                        If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                            mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                            If mySkill.Level < nSkill.Level Then
                                rSkill = New ReqSkill
                                rSkill.Name = nSkill.Name
                                rSkill.ID = nSkill.ID
                                rSkill.ReqLevel = nSkill.Level
                                rSkill.CurLevel = mySkill.Level
                                rSkill.NeededFor = fittedShip.LowSlot(slot).LoadedCharge.Name
                                nSkills.Add("LowSlot Charge" & slot.ToString & count.ToString, rSkill)
                            End If
                        Else
                            rSkill = New ReqSkill
                            rSkill.Name = nSkill.Name
                            rSkill.ID = nSkill.ID
                            rSkill.ReqLevel = nSkill.Level
                            rSkill.CurLevel = 0
                            rSkill.NeededFor = fittedShip.LowSlot(slot).LoadedCharge.Name
                            nSkills.Add("LowSlot Charge" & slot.ToString & count.ToString, rSkill)
                        End If
                    Next
                End If
            End If
        Next
        For slot As Integer = 1 To fittedShip.RigSlots
            If fittedShip.RigSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In fittedShip.RigSlot(slot).RequiredSkills.Values
                    count += 1
                    If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                        mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                        If mySkill.Level < nSkill.Level Then
                            rSkill = New ReqSkill
                            rSkill.Name = nSkill.Name
                            rSkill.ID = nSkill.ID
                            rSkill.ReqLevel = nSkill.Level
                            rSkill.CurLevel = mySkill.Level
                            rSkill.NeededFor = fittedShip.RigSlot(slot).Name
                            nSkills.Add("RigSlot" & slot.ToString & count.ToString, rSkill)
                        End If
                    Else
                        rSkill = New ReqSkill
                        rSkill.Name = nSkill.Name
                        rSkill.ID = nSkill.ID
                        rSkill.ReqLevel = nSkill.Level
                        rSkill.CurLevel = 0
                        rSkill.NeededFor = fittedShip.RigSlot(slot).Name
                        nSkills.Add("RigSlot" & slot.ToString & count.ToString, rSkill)
                    End If
                Next
            End If
        Next
        Dim droneCount As Integer = 0
        For Each droneItem As DroneBayItem In fittedShip.DroneBayItems.Values
            droneCount += 1
            Dim drone As ShipModule = droneItem.DroneType
            count = 0
            For Each nSkill As ItemSkills In drone.RequiredSkills.Values
                count += 1
                If shipPilot.SkillSet.Contains(nSkill.Name) = True Then
                    mySkill = CType(shipPilot.SkillSet(nSkill.Name), HQFSkill)
                    If mySkill.Level < nSkill.Level Then
                        rSkill = New ReqSkill
                        rSkill.Name = nSkill.Name
                        rSkill.ID = nSkill.ID
                        rSkill.ReqLevel = nSkill.Level
                        rSkill.CurLevel = mySkill.Level
                        rSkill.NeededFor = drone.Name
                        nSkills.Add("Drone" & droneCount.ToString & count.ToString, rSkill)
                    End If
                Else
                    rSkill = New ReqSkill
                    rSkill.Name = nSkill.Name
                    rSkill.ID = nSkill.ID
                    rSkill.ReqLevel = nSkill.Level
                    rSkill.CurLevel = 0
                    rSkill.NeededFor = drone.Name
                    nSkills.Add("Drone" & droneCount.ToString & count.ToString, rSkill)
                End If
            Next
        Next

        Return nSkills
    End Function

    Private Sub btnSkills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSkills.Click
        If reqSkills.Count > 0 Then
            Dim myRequiredSkills As New frmRequiredSkills
            myRequiredSkills.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem), EveHQ.Core.Pilot)
            myRequiredSkills.Skills = reqSkills
            myRequiredSkills.ShowDialog()
        End If
    End Sub

    Public Sub UpdateDroneUsage()
        fittedShip.DroneBandwidth_Used = 0
        For Each DBI As DroneBayItem In fittedShip.DroneBayItems.Values
            If DBI.IsActive = True Then
                fittedShip.DroneBandwidth_Used += CDbl(DBI.DroneType.Attributes("1272")) * DBI.Quantity
            End If
        Next
        lblDroneBandwidth.Text = FormatNumber(fittedShip.DroneBandwidth_Used, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.DroneBandwidth, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblDroneControl.Text = fittedShip.UsedDrones & " / " & fittedShip.MaxDrones
        Dim ttt As String = "Drone Control Range: " & FormatNumber(fittedShip.Attributes("10007"), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "m"
        ToolTip1.SetToolTip(lblDroneControl, ttt)
        ToolTip1.SetToolTip(pbDroneControl, ttt)
    End Sub

#Region "Audit Log Routines"
    Private Sub btnLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLog.Click
        Dim myAuditLog As New frmShipAudit
        Dim logData() As String
        Dim newLog As New ListViewItem
        myAuditLog.lvwAudit.BeginUpdate()
        For Each log As String In fittedShip.AuditLog
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
#End Region

#Region "Profile Routines"

    Private Sub cboDamageProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDamageProfiles.SelectedIndexChanged
        Dim curProfile As DamageProfile = CType(DamageProfiles.ProfileList.Item(cboDamageProfiles.SelectedItem), DamageProfile)
        If cboDamageProfiles.SelectedItem IsNot Nothing Then
            If currentShip IsNot Nothing Then
                currentShip.DamageProfile = curProfile
            End If
        End If
        ' State damage profile info
        Dim info As New System.Text.StringBuilder
        info.AppendLine(curProfile.Name & " Profile Info:")
        info.AppendLine("EM: " & FormatNumber(curProfile.EM, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        info.AppendLine("Explosive: " & FormatNumber(curProfile.Explosive, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        info.AppendLine("Kinetic: " & FormatNumber(curProfile.Kinetic, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        info.AppendLine("Thermal: " & FormatNumber(curProfile.Thermal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        info.AppendLine("DPS: " & FormatNumber(curProfile.DPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        ToolTip1.SetToolTip(cboDamageProfiles, info.ToString)
        ' Kick off a rebuild
        BuildMethod = BuildType.BuildFromEffectsMaps
    End Sub

    Private Sub btnEditProfiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditProfiles.Click
        Dim profiles As New frmDamageProfiles
        profiles.ShowDialog()
        Dim oldProfile As String = cboDamageProfiles.SelectedItem.ToString
        Call Me.LoadProfiles()
        If cboDamageProfiles.Items.Contains(oldProfile) = True Then
            cboDamageProfiles.SelectedItem = oldProfile
        Else
            cboDamageProfiles.SelectedItem = "<Omni-Damage>"
        End If
    End Sub

    Private Sub LoadProfiles()
        ' Add the list of profiles to the combo box
        cboDamageProfiles.BeginUpdate()
        cboDamageProfiles.Items.Clear()
        For Each cProfile As String In DamageProfiles.ProfileList.Keys
            cboDamageProfiles.Items.Add(cProfile)
        Next
        cboDamageProfiles.EndUpdate()
    End Sub

#End Region

#Region "Damage Analysis Routines"
    Private Sub btnDamageAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDamageAnalysis.Click
        Dim myDA As New frmDamageAnalysis
        If cboPilots.SelectedItem IsNot Nothing Then
            myDA.PilotName = cboPilots.SelectedItem.ToString
        End If
        If currentSlot IsNot Nothing Then
            myDA.FittingName = currentSlot.ShipFit
        End If
        myDA.ShowDialog()
        myDA.Dispose()
        ' Update the mapping effects back to the current pilot
        Me.BuildMethod = BuildType.BuildEffectsMaps
    End Sub
#End Region

   
   
End Class
