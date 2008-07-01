Imports System.Windows.Forms

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
Public Class ShipInfoControl

#Region "Other Variables"
    Private fittedShip As Ship ' Should be the modified ship after passing through the engine
    Private reqSkills As New SortedList ' Lists the skills required for the ship/mods/drones
#End Region

#Region "Property Variables"
    Private currentShip As Ship ' Should be the base ship only
    Private currentSlot As ShipSlotControl
#End Region

#Region "Properties"

    Public Property ShipType() As Ship
        Get
            Return currentShip
        End Get
        Set(ByVal value As Ship)
            If value IsNot Nothing Then ' May have been triggered by an initial change in the pilot on form startup
                currentShip = value
                If cboPilots.SelectedItem IsNot Nothing Then
                    Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
                    fittedShip = Engine.ApplyFitting(CType(currentShip.Clone, Ship), shipPilot)
                    Call UpdateInfoDisplay()
                    currentSlot.ShipFitted = fittedShip
                End If
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

#End Region

    Private Sub UpdateInfoDisplay()
        ' Update the display with the information about the (fitted) ship

        ' CPU
        If fittedShip.CPU > 0 Then
            progCPU.MaxValue = CInt(fittedShip.CPU)
        Else
            progCPU.MaxValue = 1
        End If
        progCPU.Value = Math.Min(CInt(fittedShip.CPU_Used), CInt(fittedShip.CPU))
        If fittedShip.CPU_Used > fittedShip.CPU Then
            progCPU.StartColor = Drawing.Color.Red
            progCPU.EndColor = Drawing.Color.Red
            progCPU.HighlightColor = Drawing.Color.White
            progCPU.GlowColor = Drawing.Color.LightPink
        Else
            progCPU.StartColor = Drawing.Color.LimeGreen
            progCPU.EndColor = Drawing.Color.LimeGreen
            progCPU.HighlightColor = Drawing.Color.White
            progCPU.GlowColor = Drawing.Color.LightGreen
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
        If fittedShip.PG_Used > fittedShip.PG Then
            progPG.StartColor = Drawing.Color.Red
            progPG.EndColor = Drawing.Color.Red
            progPG.HighlightColor = Drawing.Color.White
            progPG.GlowColor = Drawing.Color.LightPink
        Else
            progPG.StartColor = Drawing.Color.LimeGreen
            progPG.EndColor = Drawing.Color.LimeGreen
            progPG.HighlightColor = Drawing.Color.White
            progPG.GlowColor = Drawing.Color.LightGreen
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
        lblShieldRecharge.Text = FormatNumber(fittedShip.ShieldRecharge, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        lblShieldEM.Text = FormatNumber(fittedShip.ShieldEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldExplosive.Text = FormatNumber(fittedShip.ShieldExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldKinetic.Text = FormatNumber(fittedShip.ShieldKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblShieldThermal.Text = FormatNumber(fittedShip.ShieldThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        progShieldEM.Value = CInt(fittedShip.ShieldEMResist)
        progShieldExp.Value = CInt(fittedShip.ShieldExResist)
        progShieldKinetic.Value = CInt(fittedShip.ShieldKiResist)
        progShieldThermal.Value = CInt(fittedShip.ShieldThResist)
        gbShield.Text = "Shield  (Effective HP: " & FormatNumber(fittedShip.EffectiveShieldHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")"
        lblShieldAverage.Text = FormatNumber(fittedShip.ShieldCapacity / fittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP/s"
        lblShieldPeak.Text = FormatNumber(HQF.Settings.HQFSettings.ShieldRechargeConstant * fittedShip.ShieldCapacity / fittedShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP/s"

        ' Armor
        lblArmorHP.Text = FormatNumber(fittedShip.ArmorCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblArmorEM.Text = FormatNumber(fittedShip.ArmorEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorExplosive.Text = FormatNumber(fittedShip.ArmorExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorKinetic.Text = FormatNumber(fittedShip.ArmorKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblArmorThermal.Text = FormatNumber(fittedShip.ArmorThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        progArmorEM.Value = CInt(fittedShip.ArmorEMResist)
        progArmorExplosive.Value = CInt(fittedShip.ArmorExResist)
        progArmorKinetic.Value = CInt(fittedShip.ArmorKiResist)
        progArmorThermal.Value = CInt(fittedShip.ArmorThResist)
        gbArmor.Text = "Armor  (Effective HP: " & FormatNumber(fittedShip.EffectiveArmorHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")"

        ' Structure
        lblStructureHP.Text = FormatNumber(fittedShip.StructureCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblStructureEM.Text = FormatNumber(fittedShip.StructureEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureExplosive.Text = FormatNumber(fittedShip.StructureExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureKinetic.Text = FormatNumber(fittedShip.StructureKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblStructureThermal.Text = FormatNumber(fittedShip.StructureThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        progStructureEM.Value = CInt(fittedShip.StructureEMResist)
        progStructureExplosive.Value = CInt(fittedShip.StructureExResist)
        progStructureKinetic.Value = CInt(fittedShip.StructureKiResist)
        progStructureThermal.Value = CInt(fittedShip.StructureThResist)
        gbStructure.Text = "Structure  (Effective HP: " & FormatNumber(fittedShip.EffectiveStructureHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")"

        ' EffectiveHP
        lblEffectiveHP.Text = "Effective HP: " & FormatNumber(fittedShip.EffectiveHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Capacitor
        lblCapacitor.Text = FormatNumber(fittedShip.CapCapacity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " F"
        lblCapRecharge.Text = FormatNumber(fittedShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        lblCapAverage.Text = FormatNumber(fittedShip.CapCapacity / fittedShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " F/s"
        lblCapPeak.Text = FormatNumber(HQF.Settings.HQFSettings.CapRechargeConstant * fittedShip.CapCapacity / fittedShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " F/s"

        ' Propulsion
        lblSpeed.Text = FormatNumber(fittedShip.MaxVelocity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m/s"
        lblWarpSpeed.Text = FormatNumber(fittedShip.WarpSpeed, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " au/s"
        lblInertia.Text = FormatNumber(fittedShip.Inertia, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ' Time to warp based on calculation from http://myeve.eve-online.com/ingameboard.asp?a=topic&threadID=502836
        ' Time to warp (in seconds) = Inertial Modifier * (Mass / 1.000.000) * 1.61
        lblAlignTime.Text = FormatNumber(1.61 * fittedShip.Inertia * fittedShip.Mass / 1000000, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"

        ' Targeting
        lblTargetRange.Text = FormatNumber(fittedShip.MaxTargetRange, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m"
        'lblMaxTargets.Text = FormatNumber(fittedShip.MaxLockedTargets, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblScanResolution.Text = FormatNumber(fittedShip.ScanResolution, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " mm"
        lblSensorStrength.Text = FormatNumber(Math.Max(Math.Max(Math.Max(fittedShip.GravSensorStrenth, fittedShip.LadarSensorStrenth), fittedShip.MagSensorStrenth), fittedShip.RadarSensorStrenth), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblSigRadius.Text = FormatNumber(fittedShip.SigRadius, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m"

        ' Cargo and Drones
        lblCargoBay.Text = FormatNumber(fittedShip.CargoBay, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"
        lblDroneBay.Text = FormatNumber(fittedShip.DroneBay, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"
        progDroneBandwidth.Maximum = CInt(fittedShip.DroneBandwidth)
        progDroneBandwidth.Value = CInt(fittedShip.DroneBandwidth_Used)
        UpdateDroneUsage()

        ' Damage
        lblTurretVolleyDamage.Text = FormatNumber(fittedShip.TurretVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.TurretDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblMissileVolleyDamage.Text = FormatNumber(fittedShip.MissileVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.MissileDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblSmartbombVolleyDamage.Text = FormatNumber(fittedShip.SBVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.SBDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblDroneVolleyDamage.Text = FormatNumber(fittedShip.DroneVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.DroneDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        gbDamage.Text = "Damage (" & FormatNumber(fittedShip.TotalVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.TotalDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " DPS)"

        ' Collect List of Needed Skills
        reqSkills = NeededSkills()
        If reqSkills.Count = 0 Then
            btnSkills.Image = My.Resources.Skills1
        Else
            btnSkills.Image = My.Resources.Skills0
        End If

    End Sub

    Private Sub btnDoomsdayCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoomsdayCheck.Click
        Dim DDCheck As New frmDoomsday
        DDCheck.ShipType = fittedShip
        DDCheck.ShowDialog()
        DDCheck = Nothing
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add the current list of pilots to the combobox
        cboPilots.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
            cboPilots.Items.Add(cPilot.Name)
        Next
        ' Look at the settings for default pilot
        If cboPilots.Items.Count > 0 Then
            If HQF.Settings.HQFSettings.DefaultPilot <> "" Then
                cboPilots.SelectedItem = HQF.Settings.HQFSettings.DefaultPilot
            Else
                cboPilots.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        ' Build the Affections data for this pilot
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem), HQFPilot)
        Engine.BuildSkillEffects(shipPilot)
        Engine.BuildImplantEffects(shipPilot)
        ' Call the property modifier again which will trigger the fitting routines and update all slots for the new pilot
        If currentSlot IsNot Nothing Then
            currentSlot.UpdateAllSlots = True
        End If
        ShipType = currentShip
        If currentSlot IsNot Nothing Then
            currentSlot.UpdateAllSlots = False
        End If
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
            myRequiredSkills.Pilot = CType(EveHQ.Core.HQ.Pilots(cboPilots.SelectedItem), EveHQ.Core.Pilot)
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
        progDroneBandwidth.Value = CInt(fittedShip.DroneBandwidth_Used)
        lblDroneBandwidth.Text = FormatNumber(fittedShip.DroneBandwidth_Used, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.DroneBandwidth, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblDroneControl.Text = fittedShip.UsedDrones & " / " & fittedShip.MaxDrones
    End Sub

#Region "Audit Log Routines"
    Private Sub btnLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLog.Click
        Dim msg As String = ""
        For Each log As String In fittedShip.AuditLog
            msg &= log & ControlChars.CrLf
        Next
        MessageBox.Show(msg, "Audit Log", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

End Class
