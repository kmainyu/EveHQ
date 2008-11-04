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

Public Class frmGunnery

    Shared myGunnerySkills As New GunnerySkills
    Shared currentDM As Gunnery.DamageModClass
    Shared currentTC As Gunnery.TrackingModClass
    Shared currentTE As Gunnery.TrackingModClass
    Const StackNerf As Double = 0.755369541025
    Shared DamageBonus As Double = 1
    Shared ROFBonus As Double = 1
    Shared TrackingBonus As Double = 1
    Shared RangeBonus As Double = 1

    Private Sub frmWeapons_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Load Skills
        Call Me.LoadSkills()
        Call Me.LoadMods()
        radEnergy.Checked = True
    End Sub

    Private Sub LoadSkills()
        myGunnerySkills = New GunnerySkills
        For Each mySkill As EveHQ.Core.Skills In EveHQ.Core.HQ.myPilot.PilotSkills
            Select Case mySkill.Name
                Case "Advanced Weapon Upgrades"
                    myGunnerySkills.AWU = mySkill.Level
                Case "Capital Energy Turret"
                    myGunnerySkills.CapEnergy = mySkill.Level
                Case "Capital Hybrid Turret"
                    myGunnerySkills.CapHybrid = mySkill.Level
                Case "Capital Projectile Turret"
                    myGunnerySkills.CapProjectile = mySkill.Level
                Case "Controlled Bursts"
                    myGunnerySkills.ControlledBursts = mySkill.Level
                Case "Gunnery"
                    myGunnerySkills.Gunnery = mySkill.Level
                Case "Large Artillery Specialization"
                    myGunnerySkills.LargeArtSpec = mySkill.Level
                Case "Large Autocannon Specialization"
                    myGunnerySkills.LargeACSpec = mySkill.Level
                Case "Large Beam Laser Specialization"
                    myGunnerySkills.LargeBLSpec = mySkill.Level
                Case "Large Blaster Specialization"
                    myGunnerySkills.LargeBlasterSpec = mySkill.Level
                Case "Large Pulse Laser Specialization"
                    myGunnerySkills.LargePLSpec = mySkill.Level
                Case "Large Railgun Specialization"
                    myGunnerySkills.LargeRailSpec = mySkill.Level
                Case "Large Energy Turret"
                    myGunnerySkills.LargeEnergy = mySkill.Level
                Case "Large Hybrid Turret"
                    myGunnerySkills.LargeHybrid = mySkill.Level
                Case "Large Projectile Turret"
                    myGunnerySkills.LargeProjectile = mySkill.Level
                Case "Medium Artillery Specialization"
                    myGunnerySkills.MedArtSpec = mySkill.Level
                Case "Medium Autocannon Specialization"
                    myGunnerySkills.MedACSpec = mySkill.Level
                Case "Medium Beam Laser Specialization"
                    myGunnerySkills.MedBLSpec = mySkill.Level
                Case "Medium Blaster Specialization"
                    myGunnerySkills.MedBlasterSpec = mySkill.Level
                Case "Medium Pulse Laser Specialization"
                    myGunnerySkills.MedPLSpec = mySkill.Level
                Case "Medium Railgun Specialization"
                    myGunnerySkills.MedRailSpec = mySkill.Level
                Case "Medium Energy Turret"
                    myGunnerySkills.MedEnergy = mySkill.Level
                Case "Medium Hybrid Turret"
                    myGunnerySkills.MedHybrid = mySkill.Level
                Case "Medium Projectile Turret"
                    myGunnerySkills.MedProjectile = mySkill.Level
                Case "Motion Prediction"
                    myGunnerySkills.MotionPredict = mySkill.Level
                Case "Rapid Firing"
                    myGunnerySkills.RapidFiring = mySkill.Level
                Case "Sharpshooter"
                    myGunnerySkills.Sharpshooter = mySkill.Level
                Case "Small Artillery Specialization"
                    myGunnerySkills.SmallArtSpec = mySkill.Level
                Case "Small Autocannon Specialization"
                    myGunnerySkills.SmallACSpec = mySkill.Level
                Case "Small Beam Laser Specialization"
                    myGunnerySkills.SmallBLSpec = mySkill.Level
                Case "Small Blaster Specialization"
                    myGunnerySkills.SmallBlasterSpec = mySkill.Level
                Case "Small Pulse Laser Specialization"
                    myGunnerySkills.SmallPLSpec = mySkill.Level
                Case "Small Railgun Specialization"
                    myGunnerySkills.SmallRailSpec = mySkill.Level
                Case "Small Energy Turret"
                    myGunnerySkills.SmallEnergy = mySkill.Level
                Case "Small Hybrid Turret"
                    myGunnerySkills.SmallHybrid = mySkill.Level
                Case "Small Projectile Turret"
                    myGunnerySkills.SmallProjectile = mySkill.Level
                Case "Surgical Strike"
                    myGunnerySkills.SurgicalStrike = mySkill.Level
                Case "Tactical Weapon Reconfiguration"
                    myGunnerySkills.TWR = mySkill.Level
                Case "Trajectory Analysis"
                    myGunnerySkills.TrajectoryAnal = mySkill.Level
                Case "Weapon Upgrades"
                    myGunnerySkills.WU = mySkill.Level
            End Select
        Next
    End Sub
    Private Sub LoadMods()
        ' Load Tracking Mods
        Me.cboTC.BeginUpdate()
        Me.cboTE.BeginUpdate()
        Me.cboTC.Items.Clear()
        Me.cboTE.Items.Clear()
        For Each TC As TrackingModClass In PlugInData.TrackingMods.Values
            If TC.groupID = "213" Then
                Me.cboTC.Items.Add(TC.typeName)
            Else
                Me.cboTE.Items.Add(TC.typeName)
            End If
        Next
        Me.cboTC.EndUpdate()
        Me.cboTE.EndUpdate()

    End Sub
    Private Sub cboWeapon_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWeapon.SelectedIndexChanged
        Call Me.UpdateAmmoList()
        cboDM.Enabled = True
        If cboDM.Text <> "" Then
            nudDM.Enabled = True
        End If
        cboTC.Enabled = True
        If cboTC.Text <> "" Then
            nudTC.Enabled = True
        End If
        cboTE.Enabled = True
        If cboTE.Text <> "" Then
            nudTE.Enabled = True
        End If
    End Sub
    Private Sub UpdateAmmoList()
        lvGuns.Items.Clear()
        lvGuns.BeginUpdate()
        Dim defGun As New GunClass
        If radHybrid.Checked = True Then
            defGun = CType(PlugInData.HybridGuns(cboWeapon.SelectedItem), GunClass)
        Else
            If radEnergy.Checked = True Then
                defGun = CType(PlugInData.EnergyGuns(cboWeapon.SelectedItem), GunClass)
            Else
                defGun = CType(PlugInData.ProjectileGuns(cboWeapon.SelectedItem), GunClass)
            End If
        End If
        Dim pilotGun As GunClass = ApplyGunnerySkills(defGun)
        Dim moddedGun As GunClass = ApplyGunneryMods(pilotGun)

        ' Load ammo into guns!
        Dim cAmmoType As New SortedList
        If radHybrid.Checked = True Then
            cAmmoType = CType(PlugInData.HybridAmmo.Clone, Collections.SortedList)
        Else
            If radEnergy.Checked = True Then
                cAmmoType = CType(PlugInData.EnergyAmmo.Clone, Collections.SortedList)
            Else
                cAmmoType = CType(PlugInData.ProjectileAmmo.Clone, Collections.SortedList)
            End If
        End If
        ' State Standard Weapon Attributes
        lblPG.Text = "PG: " & FormatNumber(moddedGun.PGUsage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCPU.Text = "CPU: " & FormatNumber(moddedGun.CPUUsage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblDmgMod.Text = "Damage Mod: " & FormatNumber(moddedGun.DamageModifier, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblROF.Text = "ROF: " & FormatNumber(moddedGun.RateOfFire, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        For Each ammo As AmmoClass In cAmmoType.Values
            If ammo.Size = pilotGun.Size Then
                Dim ammoGun As GunClass = LoadAmmoIntoGun(moddedGun, ammo)
                Dim ammoLI As New ListViewItem
                ammoLI.Text = ammo.typeName
                ammoLI.Name = ammo.typeName
                ammoLI.SubItems.Add(FormatNumber(ammoGun.CapUsage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.OptimalRange, 0, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.AccuracyFalloff, 0, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.TrackingSpeed, 4, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.EMDamage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.ExDamage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.KiDamage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.ThDamage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.Damage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.DPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvGuns.Items.Add(ammoLI)
            End If
        Next

        ' Update the list
        lvGuns.EndUpdate()
    End Sub
    Private Function ApplyGunnerySkills(ByVal stdGun As GunClass) As GunClass
        Dim pilotGun As GunClass = CType(stdGun.Clone, GunClass)
        pilotGun.PGUsage = pilotGun.PGUsage * (1 - (0.02 * myGunnerySkills.AWU))
        pilotGun.CPUUsage = pilotGun.CPUUsage * (1 - (0.05 * myGunnerySkills.WU))
        pilotGun.CapUsage = pilotGun.CapUsage * (1 - (0.05 * myGunnerySkills.ControlledBursts))
        pilotGun.RateOfFire = pilotGun.RateOfFire * (1 - (0.02 * myGunnerySkills.Gunnery))
        pilotGun.RateOfFire = pilotGun.RateOfFire * (1 - (0.04 * myGunnerySkills.RapidFiring))
        pilotGun.TrackingSpeed = pilotGun.TrackingSpeed * (1 + (0.05 * myGunnerySkills.MotionPredict))
        pilotGun.OptimalRange = pilotGun.OptimalRange * (1 + (0.05 * myGunnerySkills.Sharpshooter))
        pilotGun.AccuracyFalloff = pilotGun.AccuracyFalloff * (1 + (0.05 * myGunnerySkills.TrajectoryAnal))
        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.03 * myGunnerySkills.SurgicalStrike))
        Select Case pilotGun.Size
            Case 1  ' Small
                Select Case pilotGun.groupID
                    Case "53"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.SmallEnergy))
                    Case "55"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.SmallProjectile))
                    Case "74"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.SmallHybrid))
                End Select
            Case 2  ' Medium
                Select Case pilotGun.groupID
                    Case "53"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.MedEnergy))
                    Case "55"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.MedProjectile))
                    Case "74"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.MedHybrid))
                End Select
            Case 3  ' Large
                Select Case pilotGun.groupID
                    Case "53"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.LargeEnergy))
                    Case "55"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.LargeProjectile))
                    Case "74"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.LargeHybrid))
                End Select
            Case 4  ' XLarge
                Select Case pilotGun.groupID
                    Case "53"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.CapEnergy))
                    Case "55"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.CapProjectile))
                    Case "74"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.CapHybrid))
                End Select
        End Select
        Select Case pilotGun.TertiarySkill
            Case "Small Artillery Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallArtSpec))
            Case "Small Autocannon Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallACSpec))
            Case "Small Beam Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallBLSpec))
            Case "Small Blaster Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallBlasterSpec))
            Case "Small Pulse Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallPLSpec))
            Case "Small Railgun Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallRailSpec))

            Case "Medium Artillery Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedArtSpec))
            Case "Medium Autocannon Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedACSpec))
            Case "Medium Beam Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedBLSpec))
            Case "Medium Blaster Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedBlasterSpec))
            Case "Medium Pulse Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedPLSpec))
            Case "Medium Railgun Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedRailSpec))

            Case "Large Artillery Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeArtSpec))
            Case "Large Autocannon Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeACSpec))
            Case "Large Beam Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeBLSpec))
            Case "Large Blaster Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeBlasterSpec))
            Case "Large Pulse Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargePLSpec))
            Case "Large Railgun Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeRailSpec))

        End Select

        Return pilotGun
    End Function
    Private Function ApplyGunneryMods(ByVal stdGun As GunClass) As GunClass
        Dim moddedGun As GunClass = CType(stdGun.Clone, GunClass)
        moddedGun.DamageModifier *= DamageBonus
        moddedGun.OptimalRange *= RangeBonus
        moddedGun.RateOfFire *= ROFBonus
        moddedGun.TrackingSpeed *= TrackingBonus
        Return moddedGun
    End Function
    Private Function LoadAmmoIntoGun(ByVal pilotGun As GunClass, ByVal ammo As AmmoClass) As GunClass
        ' Clone the gun
        Dim ammoGun As GunClass = CType(pilotGun.Clone, GunClass)
        ' Calcualte the range bonus
        ammoGun.OptimalRange *= ammo.RangeBonus
        ' Calculate the falloff bonus
        If ammo.FalloffBonus <> 0 Then
            ammoGun.AccuracyFalloff *= ammo.FalloffBonus
        End If
        ' Caluclate the cap usage bonus
        ammoGun.CapUsage += ammoGun.CapUsage * ammo.CapBonus / 100
        ' Calculate any ROF Bonus
        If ammo.ROFBonus <> 0 Then
            ammoGun.RateOfFire = ammoGun.RateOfFire * ammo.ROFBonus
        End If
        ' Calculate any Tracking Bonus
        If ammo.TrackingBonus <> 0 Then
            ammoGun.TrackingSpeed = ammoGun.TrackingSpeed * ammo.TrackingBonus
        End If
        ' Calculate the separate damages
        ammoGun.EMDamage = ammo.EMDamage * ammoGun.DamageModifier
        ammoGun.ExDamage = ammo.ExDamage * ammoGun.DamageModifier
        ammoGun.KiDamage = ammo.KiDamage * ammoGun.DamageModifier
        ammoGun.ThDamage = ammo.ThDamage * ammoGun.DamageModifier
        ' Calculate the total damage and DPS
        ammoGun.Damage = ammoGun.EMDamage + ammoGun.ExDamage + ammoGun.KiDamage + ammoGun.ThDamage
        ammoGun.DPS = ammoGun.Damage / ammoGun.RateOfFire
        Return ammoGun
    End Function
    Private Sub WeaponType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEnergy.CheckedChanged, radHybrid.CheckedChanged, radProjectile.CheckedChanged
        If radHybrid.Checked = True Then
            Call Me.LoadHybridWeapons()
            Call Me.UpdateDM(302)
        Else
            If radEnergy.Checked = True Then
                Call Me.LoadEnergyWeapons()
                Call Me.UpdateDM(205)
            Else
                Call Me.LoadProjectileWeapons()
                Call Me.UpdateDM(59)
            End If
        End If
        cboDM.Enabled = False : nudDM.Enabled = False
        cboTC.Enabled = False : nudTC.Enabled = False
        cboTE.Enabled = False : nudTC.Enabled = False
    End Sub
    Private Sub UpdateDM(ByVal DMtype As Integer)
        Me.cboDM.BeginUpdate()
        Me.cboDM.Items.Clear()
        For Each DM As DamageModClass In PlugInData.DamageMods.Values
            If DMtype = CInt(DM.groupID) Then
                Me.cboDM.Items.Add(DM.typeName)
            End If
        Next
        Me.cboDM.EndUpdate()
    End Sub
    Private Sub LoadEnergyWeapons()
        cboWeapon.BeginUpdate()
        cboWeapon.Items.Clear()
        For Each energyGun As String In PlugInData.EnergyGuns.Keys
            cboWeapon.Items.Add(energyGun)
        Next
        cboWeapon.EndUpdate()
    End Sub
    Private Sub LoadHybridWeapons()
        cboWeapon.BeginUpdate()
        cboWeapon.Items.Clear()
        For Each hybridGun As String In PlugInData.HybridGuns.Keys
            cboWeapon.Items.Add(hybridGun)
        Next
        cboWeapon.EndUpdate()
    End Sub
    Private Sub LoadProjectileWeapons()
        cboWeapon.BeginUpdate()
        cboWeapon.Items.Clear()
        For Each projectileGun As String In PlugInData.ProjectileGuns.Keys
            cboWeapon.Items.Add(projectileGun)
        Next
        cboWeapon.EndUpdate()
    End Sub

    Private Sub cboDM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDM.SelectedIndexChanged
        nudDM.Enabled = True
        currentDM = CType(PlugInData.DamageMods.Item(cboDM.SelectedItem), DamageModClass)
        Call Me.CalcDMBonus()
    End Sub
    Private Sub cboTC_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTC.SelectedIndexChanged
        nudTC.Enabled = True
        currentTC = CType(PlugInData.TrackingMods.Item(cboTC.SelectedItem), TrackingModClass)
        Call Me.CalcTMBonus()
    End Sub
    Private Sub cboTE_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTE.SelectedIndexChanged
        nudTE.Enabled = True
        currentTE = CType(PlugInData.TrackingMods.Item(cboTE.SelectedItem), TrackingModClass)
        Call Me.CalcTMBonus()
    End Sub
    Private Sub nudDM_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDM.ValueChanged
        Call Me.CalcDMBonus()
    End Sub
    Private Sub nudTC_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTC.ValueChanged
        Call Me.CalcTMBonus()
    End Sub
    Private Sub nudTE_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTE.ValueChanged
        Call Me.CalcTMBonus()
    End Sub
    Private Sub CalcDMBonus()
        Dim DB, RB As Double
        DB = 1
        RB = 1
        If nudDM.Value <> 0 Then
            Dim effect As Double = 0
            For mods As Double = 1 To CInt(nudDM.Value)
                effect = StackNerf ^ (((mods - 1) ^ 2) / 2)         ' 1, 0.86912, 0.570583 etc
                DB *= 1 + ((currentDM.DamageBonus - 1) * effect)
                RB *= 1 + ((currentDM.ROFBonus - 1) * effect)
            Next
        End If
        DamageBonus = DB
        ROFBonus = RB
        lblDMBonus.Text = "Effective Dmg Bonus: " & FormatNumber(DB, 8, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblROFBonus.Text = "Effective ROF Bonus: " & FormatNumber(RB, 8, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Call Me.UpdateAmmoList()
    End Sub
    Private Sub CalcTMBonus()
        Dim TB, RB As Double
        TB = 1
        RB = 1
        If (nudTC.Value + nudTE.Value) <> 0 Then
            Dim effect As Double = 0
            For mods As Integer = 1 To CInt(nudTC.Value)
                effect = StackNerf ^ (((mods - 1) ^ 2) / 2)         ' 1, 0.86912, 0.570583 etc
                TB *= 1 + ((currentTC.TrackingBonus / 100) * effect)
                RB *= 1 + ((currentTC.RangeBonus / 100) * effect)
            Next
            For mods As Integer = CInt(nudTC.Value) + 1 To CInt(nudTC.Value) + CInt(nudTE.Value)
                effect = StackNerf ^ (((mods - 1) ^ 2) / 2)         ' 1, 0.86912, 0.570583 etc
                TB *= 1 + ((currentTE.TrackingBonus / 100) * effect)
                RB *= 1 + ((currentTE.RangeBonus / 100) * effect)
            Next
        End If
        TrackingBonus = TB
        RangeBonus = RB
        lblTCBonus.Text = "Effective Tracking Bonus: " & FormatNumber(TB, 8, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblRangeBonus.Text = "Effective Range Bonus: " & FormatNumber(RB, 8, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Call Me.UpdateAmmoList()
    End Sub

    Private Sub lvGuns_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvGuns.ColumnClick
        If lvGuns.Tag IsNot Nothing Then
            If CDbl(lvGuns.Tag.ToString) = e.Column Then
                Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
                lvGuns.Tag = -1
            Else
                Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
                lvGuns.Tag = e.Column
            End If
        Else
            Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvGuns.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvGuns.Sort()
    End Sub
End Class