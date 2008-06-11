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

Public Class Engine

    Public Shared AffectionsMap As New SortedList
    Public Shared SkillAffectionsTable As New SortedList
    Public Shared ModuleAffectionsTable As New SortedList

    Public Shared Function ApplyFitting(ByVal baseShip As Ship, ByVal shipPilot As HQFPilot) As Ship
        ' Apply the pilot skills to the ship
        Dim newShip As Ship
        newShip = ApplySkillsToShip(CType(baseShip.Clone, Ship), shipPilot)
        newShip = ApplyShipBonusesToShip(CType(newShip.Clone, Ship), shipPilot)
        newShip = ApplyModulesToShip(CType(newShip.Clone, Ship))
        Return newShip
    End Function

    Private Shared Function ApplySkillsToShip(ByVal baseShip As Ship, ByVal shipPilot As HQFPilot) As Ship
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        ' Clear the audit log
        newShip.AuditLog.Clear()
        Dim log As String = ""
        ' Only select relevant skills for the ship

        For Each cSkill As HQFSkill In shipPilot.SkillSet
            Select Case cSkill.Name
                Case "Drones"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Drones: " & newShip.Drones & " --> "
                    newShip.Drones += cSkill.Level
                    log &= newShip.Drones
                    newShip.AuditLog.Add(log)
                Case "Electronics"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): CPU: " & newShip.CPU & " --> "
                    newShip.CPU *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.CPU
                    newShip.AuditLog.Add(log)
                Case "Long Range Targeting"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Max Target Range: " & newShip.MaxTargetRange & " --> "
                    newShip.MaxTargetRange *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.MaxTargetRange
                    newShip.AuditLog.Add(log)
                Case "Multitasking"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Max Locked Targets: " & newShip.MaxLockedTargets & " --> "
                    newShip.MaxLockedTargets += cSkill.Level
                    log &= newShip.MaxLockedTargets
                    newShip.AuditLog.Add(log)
                Case "Signature Analysis"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Scan Resolution: " & newShip.ScanResolution & " --> "
                    newShip.ScanResolution *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.ScanResolution
                    newShip.AuditLog.Add(log)
                Case "Targeting"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Max Locked Targets: " & newShip.MaxLockedTargets & " --> "
                    newShip.MaxLockedTargets += cSkill.Level
                    log &= newShip.MaxLockedTargets
                    newShip.AuditLog.Add(log)
                Case "Energy Management"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Capacitor Capacity: " & newShip.CapCapacity & " --> "
                    newShip.CapCapacity *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.CapCapacity
                    newShip.AuditLog.Add(log)
                Case "Energy Systems Operation"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Capacitor Recharge: " & newShip.CapRecharge & " --> "
                    newShip.CapRecharge *= 1 - (cSkill.Level * 0.05)
                    log &= newShip.CapRecharge
                    newShip.AuditLog.Add(log)
                Case "Engineering"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Powergrid: " & newShip.PG & " --> "
                    newShip.PG *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.PG
                    newShip.AuditLog.Add(log)
                Case "Shield Management"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Shield Capacity: " & newShip.ShieldCapacity & " --> "
                    newShip.ShieldCapacity *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.ShieldCapacity
                    newShip.AuditLog.Add(log)
                Case "Shield Operation"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Shield Recharge: " & newShip.ShieldRecharge & " --> "
                    newShip.ShieldRecharge *= 1 - (cSkill.Level * 0.05)
                    log &= newShip.ShieldRecharge
                    newShip.AuditLog.Add(log)
                Case "Hull Upgrades"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Armor Hipoints: " & newShip.ArmorCapacity & " --> "
                    newShip.ArmorCapacity *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.ArmorCapacity
                    newShip.AuditLog.Add(log)
                Case "Mechanic"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Structure Hitpoints: " & newShip.StructureCapacity & " --> "
                    newShip.StructureCapacity *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.StructureCapacity
                    newShip.AuditLog.Add(log)
                Case "Evasive Maneuvering"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Inertia: " & newShip.Inertia & " --> "
                    newShip.Inertia *= 1 - (cSkill.Level * 0.05)
                    log &= newShip.Inertia
                    newShip.AuditLog.Add(log)
                Case "Navigation"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Max Velocity: " & newShip.MaxVelocity & " --> "
                    newShip.MaxVelocity *= 1 + (cSkill.Level * 0.05)
                    log &= newShip.MaxVelocity
                    newShip.AuditLog.Add(log)
                Case "Advanced Spaceship Command"
                    If newShip.RequiredSkillList.Contains("Advanced Spaceship Command") Then
                        log = cSkill.Name & " skill (" & cSkill.Level & "): Inertia: " & newShip.Inertia & " --> "
                        newShip.Inertia *= 1 - (cSkill.Level * 0.05)
                        log &= newShip.Inertia
                        newShip.AuditLog.Add(log)
                    End If
                Case "Capital Ships"
                    If newShip.RequiredSkillList.Contains("Capital Ships") Then
                        log = cSkill.Name & " skill (" & cSkill.Level & "): Inertia: " & newShip.Inertia & " --> "
                        newShip.Inertia *= 1 - (cSkill.Level * 0.05)
                        log &= newShip.Inertia
                        newShip.AuditLog.Add(log)
                    End If
                Case "Spaceship Command"
                    log = cSkill.Name & " skill (" & cSkill.Level & "): Inertia: " & newShip.Inertia & " --> "
                    newShip.Inertia *= 1 - (cSkill.Level * 0.02)
                    log &= newShip.Inertia
                    newShip.AuditLog.Add(log)
            End Select
        Next
        Return newShip
    End Function

    Private Shared Function ApplyShipBonusesToShip(ByVal baseShip As Ship, ByVal shipPilot As HQFPilot) As Ship
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For Each bonus As ItemBonus In newShip.Bonuses
            'Call ApplyShipBonus(newShip, bonus, shipPilot)
            ' Establish type and value of the bonus to be applied
            Dim bonusValue As Double = 0
            If bonus.BonusIsPerLevel = False Then
                bonusValue = bonus.BonusValue
            Else
                If shipPilot.SkillSet.Contains(bonus.BonusSkillName) = True Then
                    Dim bonusSkill As HQFSkill = CType(shipPilot.SkillSet(bonus.BonusSkillName), HQFSkill)
                    bonusValue = bonus.BonusValue * bonusSkill.Level
                Else
                    bonusValue = 0
                End If
            End If
        Next
        Return newShip
    End Function

    Private Shared Function ApplyModulesToShip(ByVal baseShip As Ship) As Ship
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        Dim sMod As New ShipModule
        For hi As Integer = 1 To newShip.HiSlots
            sMod = newShip.HiSlot(hi)
            If sMod IsNot Nothing Then
                ' Check Ammo first
                If sMod.IsTurret Or sMod.IsLauncher Then
                    If sMod.LoadedCharge IsNot Nothing Then
                        Call CalculateDamage(newShip, sMod)
                    End If
                End If
            End If
        Next
        Return newShip
    End Function

    Private Shared Sub CalculateDamage(ByVal baseShip As Ship, ByVal baseMod As ShipModule)
        For Each bonus As ItemBonus In baseMod.LoadedCharge.Bonuses
            Select Case bonus.BonusName
                Case "EM_DAMAGE", "EXPLOSIVE_DAMAGE", "KINETIC_DAMAGE", "THERMAL_DAMAGE"
                    ' Ignore individual damage type at present
                    If baseMod.IsTurret Then
                        baseShip.TurretVolley += bonus.BonusValue
                    Else
                        baseShip.MissileVolley += bonus.BonusValue
                    End If
            End Select
        Next
    End Sub

#Region "Calculate Ship Bonuses"
    Private Sub ApplyShipBonus(ByVal baseShip As Ship, ByVal bonus As ItemBonus, ByVal shipPilot As HQFPilot)
        ' Calculate Bonus Value
        Dim bonusValue As Double = 0
        If bonus.BonusIsPerLevel = False Then
            bonusValue = bonus.BonusValue
        Else
            If shipPilot.SkillSet.Contains(bonus.BonusSkillName) = True Then
                Dim bonusSkill As EveHQ.Core.Skills = CType(shipPilot.SkillSet(bonus.BonusSkillName), Core.Skills)
                bonusValue = bonus.BonusValue * bonusSkill.Level
            Else
                bonusValue = 0
            End If
        End If
        Select Case Bonuses.BonusGroups.Item(bonus.BonusName).ToString
            Case "Armor"

        End Select
    End Sub


#End Region

#Region "New Routines"
    Public Shared Sub BuildSkillAffections(ByVal hPilot As HQFPilot)
        Dim sTime, eTime As Date
        sTime = Now
        ' Clear the Affections Table
        SkillAffectionsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim aSkill As New Skill
        For Each hSkill As HQFSkill In hPilot.SkillSet
            If hSkill.Level <> 0 Then
                ' Go through the attributes
                aSkill = CType(SkillLists.SkillList(hSkill.ID), Skill)
                For Each att As String In aSkill.Attributes.Keys
                    If AffectionsMap.Contains(att & "_" & aSkill.ID) = True Then
                        If SkillAffectionsTable.Contains(AffectionsMap(att & "_" & aSkill.ID)) = False Then
                            SkillAffectionsTable.Add(AffectionsMap(att & "_" & aSkill.ID), CDbl(aSkill.Attributes(CInt(att))) * hSkill.Level)
                        End If
                    End If
                Next
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Building Skill Affections took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
    End Sub

    Public Shared Function BuildModuleAffections(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        ' Clear the Affections Table
        ModuleAffectionsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim aModule As ShipModule
        Dim newData() As String
        Dim attData As New ArrayList
        For slot As Integer = 1 To newShip.HiSlots
            aModule = newShip.HiSlot(slot)
            If aModule IsNot Nothing Then
                For Each att As String In aModule.Attributes.Keys
                    If AffectionsMap.Contains(att & "_" & aModule.DatabaseGroup) = True Then
                        newData = AffectionsMap(att & "_" & aModule.DatabaseGroup).ToString.Split("_".ToCharArray)
                        If ModuleAffectionsTable.Contains(newData(0)) = False Then
                            attData = New ArrayList
                            ModuleAffectionsTable.Add(newData(0), attData)
                        Else
                            attData = CType(ModuleAffectionsTable(newData(0)), Collections.ArrayList)
                        End If
                        attData.Add(newData(1) & "_" & CDbl(aModule.Attributes(att)))
                    End If
                Next
            End If
        Next
        For slot As Integer = 1 To newShip.MidSlots
            aModule = newShip.MidSlot(slot)
            If aModule IsNot Nothing Then
                For Each att As String In aModule.Attributes.Keys
                    If AffectionsMap.Contains(att & "_" & aModule.DatabaseGroup) = True Then
                        newData = AffectionsMap(att & "_" & aModule.DatabaseGroup).ToString.Split("_".ToCharArray)
                        If ModuleAffectionsTable.Contains(newData(0)) = False Then
                            attData = New ArrayList
                            ModuleAffectionsTable.Add(newData(0), attData)
                        Else
                            attData = CType(ModuleAffectionsTable(newData(0)), Collections.ArrayList)
                        End If
                        attData.Add(newData(1) & "_" & CDbl(aModule.Attributes(att)))
                    End If
                Next
            End If
        Next
        For slot As Integer = 1 To newShip.LowSlots
            aModule = newShip.LowSlot(slot)
            If aModule IsNot Nothing Then
                For Each att As String In aModule.Attributes.Keys
                    If AffectionsMap.Contains(att & "_" & aModule.DatabaseGroup) = True Then
                        newData = AffectionsMap(att & "_" & aModule.DatabaseGroup).ToString.Split("_".ToCharArray)
                        If ModuleAffectionsTable.Contains(newData(0)) = False Then
                            attData = New ArrayList
                            ModuleAffectionsTable.Add(newData(0), attData)
                        Else
                            attData = CType(ModuleAffectionsTable(newData(0)), Collections.ArrayList)
                        End If
                        attData.Add(newData(1) & "_" & CDbl(aModule.Attributes(att)))
                    End If
                Next
            End If
        Next
        For slot As Integer = 1 To newShip.RigSlots
            aModule = newShip.RigSlot(slot)
            If aModule IsNot Nothing Then
                For Each att As String In aModule.Attributes.Keys
                    If AffectionsMap.Contains(att & "_" & aModule.DatabaseGroup) = True Then
                        newData = AffectionsMap(att & "_" & aModule.DatabaseGroup).ToString.Split("_".ToCharArray)
                        If ModuleAffectionsTable.Contains(newData(0)) = False Then
                            attData = New ArrayList
                            ModuleAffectionsTable.Add(newData(0), attData)
                        Else
                            attData = CType(ModuleAffectionsTable(newData(0)), Collections.ArrayList)
                        End If
                        attData.Add(newData(1) & "_" & CDbl(aModule.Attributes(att)))
                    End If
                Next
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Building Module Affections took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Public Shared Sub BuildAffectionsMap()
        ' Fetch the affections list
        Dim AffectionFile As String = My.Resources.Affections.ToString
        ' Break the affections down into separate lines
        Dim AffectionLines() As String = AffectionFile.Split(ControlChars.Cr)
        ' Go through lines and break each one down
        Dim AffectionData() As String
        ' Build the map
        AffectionsMap.Clear()
        For Each AffectionLine As String In AffectionLines
            If AffectionLine.Trim <> "" Then
                AffectionData = AffectionLine.Split(",".ToCharArray)
                AffectionsMap.Add(AffectionData(0).Trim & "_" & AffectionData(1).Trim, AffectionData(2).Trim & "_" & AffectionData(3).Trim)
            End If
        Next
    End Sub

    Public Shared Function ApplyFitting2(ByVal baseShip As Ship, ByVal shipPilot As HQFPilot) As Ship
        ' Apply the pilot skills to the ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim newShip As Ship
        newShip = ApplySkillAffectionsToShip(CType(baseShip.Clone, Ship))
        newShip = ApplySkillAffectionsToModules(CType(newShip.Clone, Ship))
        newShip = Engine.BuildModuleAffections(newShip)
        newShip = ApplyModuleAffectionsToShip(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying the whole fitting took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Private Shared Function ApplySkillAffectionsToShip(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim mapValue As Double
        Dim mapData() As String
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For Each map As String In SkillAffectionsTable.Keys
            mapValue = CDbl(SkillAffectionsTable(map))
            mapData = map.Split("_".ToCharArray)
            If newShip.Attributes.Contains(mapData(0)) = True Then
                newShip.Attributes(mapData(0)) = CDbl(newShip.Attributes(mapData(0))) * (1 + (mapValue / 100))
            End If
        Next
        Ship.MapShipAttributes(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Skill Affections to Ship took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Private Shared Function ApplySkillAffectionsToModules(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim mapValue As Double
        Dim mapData() As String
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For Each map As String In SkillAffectionsTable.Keys
            mapValue = CDbl(SkillAffectionsTable(map))
            mapData = map.Split("_".ToCharArray)
            For slot As Integer = 1 To newShip.HiSlots
                If newShip.HiSlot(slot) IsNot Nothing Then
                    If newShip.HiSlot(slot).Attributes.Contains(mapData(0)) = True Then
                        newShip.HiSlot(slot).Attributes(mapData(0)) = CDbl(newShip.HiSlot(slot).Attributes(mapData(0))) * (1 + (mapValue / 100))
                    End If
                End If
            Next
            For slot As Integer = 1 To newShip.MidSlots
                If newShip.MidSlot(slot) IsNot Nothing Then
                    If newShip.MidSlot(slot).Attributes.Contains(mapData(0)) = True Then
                        newShip.MidSlot(slot).Attributes(mapData(0)) = CDbl(newShip.MidSlot(slot).Attributes(mapData(0))) * (1 + (mapValue / 100))
                    End If
                End If
            Next
            For slot As Integer = 1 To newShip.LowSlots
                If newShip.LowSlot(slot) IsNot Nothing Then
                    If newShip.LowSlot(slot).Attributes.Contains(mapData(0)) = True Then
                        newShip.LowSlot(slot).Attributes(mapData(0)) = CDbl(newShip.LowSlot(slot).Attributes(mapData(0))) * (1 + (mapValue / 100))
                    End If
                End If
            Next
            For slot As Integer = 1 To newShip.RigSlots
                If newShip.RigSlot(slot) IsNot Nothing Then
                    If newShip.RigSlot(slot).Attributes.Contains(mapData(0)) = True Then
                        newShip.RigSlot(slot).Attributes(mapData(0)) = CDbl(newShip.RigSlot(slot).Attributes(mapData(0))) * (1 + (mapValue / 100))
                    End If
                End If
            Next
        Next
        Ship.MapShipAttributes(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Skill Affections to Modules took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip

    End Function

    Private Shared Function ApplyModuleAffectionsToShip(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim att As String = ""
        Dim map As String
        Dim mapData() As String
        Dim mapValue As Double = 0
        Dim mapGroup As String = ""
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = CStr(newShip.Attributes.GetKey(attNo))
            If ModuleAffectionsTable.Contains(att) = True Then
                For Each map In CType(ModuleAffectionsTable(att), ArrayList)
                    mapData = map.Split("_".ToCharArray)
                    mapGroup = mapData(0) : mapValue = CDbl(mapData(1))
                    If CDbl(mapGroup) = 0 Then
                        Select Case CInt(att)
                            Case 49
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + mapValue
                            Case Else
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (mapValue / 100))
                        End Select
                    End If
                Next
            End If
        Next
        Ship.MapShipAttributes(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Module Affections to Ship took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function


#End Region

End Class


