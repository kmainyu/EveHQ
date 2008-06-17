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

    Public Shared EffectsMap As New SortedList
    Public Shared ShipEffectsMap As New SortedList
    Public Shared SkillEffectsTable As New SortedList
    Public Shared ModuleEffectsTable As New SortedList

#Region "New Routines"
    Public Shared Sub BuildEffectsMap()
        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.Effects.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String
        ' Build the map
        EffectsMap.Clear()
        Dim EffectClassList As New ArrayList
        Dim newEffect As New Effect
        Dim IDs() As String
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                newEffect = New Effect
                If EffectsMap.Contains((EffectData(0))) = True Then
                    EffectClassList = CType(EffectsMap(EffectData(0)), ArrayList)
                Else
                    EffectClassList = New ArrayList
                    EffectsMap.Add(EffectData(0), EffectClassList)
                End If
                newEffect.AffectingAtt = CInt(EffectData(0))
                newEffect.AffectingType = CInt(EffectData(1))
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CInt(EffectData(4))
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CBool(EffectData(6))
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CInt(EffectData(8))
                EffectClassList.Add(newEffect)
            End If
        Next
    End Sub
    Public Shared Sub BuildShipEffectsMap()
        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.ShipEffects.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String
        ' Build the map
        ShipEffectsMap.Clear()
        Dim shipEffectClassList As New ArrayList
        Dim newEffect As New ShipEffect
        Dim IDs() As String
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                If ShipEffectsMap.Contains((EffectData(0))) = True Then
                    shipEffectClassList = CType(ShipEffectsMap(EffectData(0)), ArrayList)
                Else
                    shipEffectClassList = New ArrayList
                    ShipEffectsMap.Add(EffectData(0), shipEffectClassList)
                End If
                newEffect.ShipID = CInt(EffectData(0))
                newEffect.AffectingType = CInt(EffectData(1))
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CInt(EffectData(4))
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CBool(EffectData(6))
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CInt(EffectData(8))
                newEffect.Value = CDbl(EffectData(9))
                shipEffectClassList.Add(newEffect)
            End If
        Next
    End Sub
    Public Shared Sub BuildSkillEffects(ByVal hPilot As HQFPilot)
        Dim sTime, eTime As Date
        sTime = Now
        ' Clear the Effects Table
        SkillEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim aSkill As New Skill
        Dim fEffect As New FinalEffect
        Dim fEffectList As New ArrayList
        For Each hSkill As HQFSkill In hPilot.SkillSet
            If hSkill.Level <> 0 Then
                ' Go through the attributes
                aSkill = CType(SkillLists.SkillList(hSkill.ID), Skill)
                For Each att As String In aSkill.Attributes.Keys
                    If EffectsMap.Contains(att) = True Then
                        For Each chkEffect As Effect In CType(EffectsMap(att), ArrayList)
                            If chkEffect.AffectingType = EffectType.Item And chkEffect.AffectingID = CInt(aSkill.ID) Then
                                fEffect = New FinalEffect
                                fEffect.AffectedAtt = chkEffect.AffectedAtt
                                fEffect.AffectedType = chkEffect.AffectedType
                                fEffect.AffectedID = chkEffect.AffectedID
                                fEffect.AffectedValue = CDbl(aSkill.Attributes(CInt(att))) * hSkill.Level
                                fEffect.StackNerf = chkEffect.StackNerf
                                fEffect.Cause = hSkill.Name & " (Level " & hSkill.Level & ")"
                                fEffect.CalcType = chkEffect.CalcType
                                If SkillEffectsTable.Contains(fEffect.AffectedAtt.ToString) = False Then
                                    fEffectList = New ArrayList
                                    SkillEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                Else
                                    fEffectList = CType(SkillEffectsTable(fEffect.AffectedAtt.ToString), Collections.ArrayList)
                                End If
                                fEffectList.Add(fEffect)
                            End If
                        Next
                    End If
                Next
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Building Skill Effects took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
    End Sub
    Public Shared Sub BuildShipEffects(ByVal hPilot As HQFPilot, ByVal hShip As Ship)
        If hShip IsNot Nothing Then
            Dim sTime, eTime As Date
            sTime = Now
            ' Go through all the skills and see what needs to be mapped
            Dim shipRoles As New ArrayList
            Dim hSkill As New HQFSkill
            Dim fEffect As New FinalEffect
            Dim fEffectList As New ArrayList
            shipRoles = CType(ShipEffectsMap(hShip.ID), ArrayList)
            If shipRoles IsNot Nothing Then
                For Each chkEffect As ShipEffect In shipRoles
                    fEffect = New FinalEffect
                    If hPilot.SkillSet.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))) = True Then
                        hSkill = CType(hPilot.SkillSet(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))), HQFSkill)
                        If chkEffect.IsPerLevel = True Then
                            fEffect.AffectedValue = chkEffect.Value * hSkill.Level
                            fEffect.Cause = "Ship Bonus: " & hSkill.Name & " (Level " & hSkill.Level & ")"
                        Else
                            fEffect.AffectedValue = chkEffect.Value
                            fEffect.Cause = "Ship Role: "
                        End If
                    Else
                        fEffect.AffectedValue = chkEffect.Value
                        fEffect.Cause = "Ship Role: "
                    End If
                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                    fEffect.AffectedType = chkEffect.AffectedType
                    fEffect.AffectedID = chkEffect.AffectedID
                    fEffect.StackNerf = chkEffect.StackNerf
                    fEffect.CalcType = chkEffect.CalcType
                    If SkillEffectsTable.Contains(fEffect.AffectedAtt.ToString) = False Then
                        fEffectList = New ArrayList
                        SkillEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                    Else
                        fEffectList = CType(SkillEffectsTable(fEffect.AffectedAtt.ToString), Collections.ArrayList)
                    End If
                    fEffectList.Add(fEffect)
                Next
            End If
            eTime = Now
            Dim dTime As TimeSpan = eTime - sTime
            'MessageBox.Show("Building Skill Effects took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        End If
    End Sub
    Public Shared Function BuildModuleEffects(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        ' Clear the Effects Table
        ModuleEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim maxSlots As Integer = 0
        Dim aModule As New ShipModule
        Dim attData As New ArrayList
        Dim fEffect As New FinalEffect
        Dim fEffectList As New ArrayList
        Dim processData As Boolean = False
        For slotType As Integer = 1 To 4
            Select Case slotType
                Case 1
                    maxSlots = newShip.HiSlots
                Case 2
                    maxSlots = newShip.MidSlots
                Case 3
                    maxSlots = newShip.LowSlots
                Case 4
                    maxSlots = newShip.RigSlots
            End Select
            For slot As Integer = 1 To maxSlots
                Select Case slotType
                    Case 1
                        aModule = newShip.HiSlot(slot)
                    Case 2
                        aModule = newShip.MidSlot(slot)
                    Case 3
                        aModule = newShip.LowSlot(slot)
                    Case 4
                        aModule = newShip.RigSlot(slot)
                End Select
                If aModule IsNot Nothing Then
                    For Each att As String In aModule.Attributes.Keys
                        If EffectsMap.Contains(att) = True Then
                            For Each chkEffect As Effect In CType(EffectsMap(att), ArrayList)
                                processData = False
                                Select Case chkEffect.AffectingType
                                    Case EffectType.All
                                        processData = True
                                    Case EffectType.Item
                                        If chkEffect.AffectingID.ToString = aModule.ID Then
                                            processData = True
                                        End If
                                    Case EffectType.Group
                                        If chkEffect.AffectingID.ToString = aModule.DatabaseGroup Then
                                            processData = True
                                        End If
                                    Case EffectType.Category
                                        If chkEffect.AffectingID.ToString = aModule.DatabaseCategory Then
                                            processData = True
                                        End If
                                    Case EffectType.MarketGroup
                                        If chkEffect.AffectingID.ToString = aModule.MarketGroup Then
                                            processData = True
                                        End If
                                    Case EffectType.Skill
                                        If aModule.RequiredSkills.Contains(chkEffect.AffectingID.ToString) Then
                                            processData = True
                                        End If
                                End Select
                                If processData = True Then
                                    fEffect = New FinalEffect
                                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                                    fEffect.AffectedType = chkEffect.AffectedType
                                    fEffect.AffectedID = chkEffect.AffectedID
                                    fEffect.AffectedValue = CDbl(aModule.Attributes(att))
                                    fEffect.StackNerf = chkEffect.StackNerf
                                    fEffect.Cause = aModule.Name
                                    fEffect.CalcType = chkEffect.CalcType
                                    If ModuleEffectsTable.Contains(fEffect.AffectedAtt.ToString) = False Then
                                        fEffectList = New ArrayList
                                        ModuleEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                    Else
                                        fEffectList = CType(ModuleEffectsTable(fEffect.AffectedAtt.ToString), Collections.ArrayList)
                                    End If
                                    fEffectList.Add(fEffect)
                                End If
                            Next
                        End If
                    Next
                End If
            Next
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Building Module Effects took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Public Shared Function ApplyFitting(ByVal baseShip As Ship, ByVal shipPilot As HQFPilot) As Ship
        ' Apply the pilot skills to the ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim newShip As Ship
        Engine.BuildShipEffects(shipPilot, baseShip)
        newShip = ApplySkillEffectsToShip(CType(baseShip.Clone, Ship))
        newShip = ApplySkillEffectsToModules(newShip)
        newShip = Engine.BuildModuleEffects(newShip)
        Call Engine.ApplyStackingPenalties()
        newShip = Engine.ApplyModuleEffectsToModules(newShip)
        newShip = Engine.ApplyModuleEffectsToShip(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        MessageBox.Show("Applying the whole fitting took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Private Shared Function ApplySkillEffectsToShip(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim thisEffect As New FinalEffect
        Dim att As String = ""
        Dim processAtt As Boolean = False
        Dim log As String = ""
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = CStr(newShip.Attributes.GetKey(attNo))
            If SkillEffectsTable.Contains(att) = True Then
                For Each fEffect As FinalEffect In CType(SkillEffectsTable(att), ArrayList)
                    processAtt = False
                    log = ""
                    Select Case fEffect.AffectedType
                        Case EffectType.All
                            processAtt = True
                        Case EffectType.Item
                            If fEffect.AffectedID.Contains(newShip.ID) Then
                                processAtt = True
                            End If
                        Case EffectType.Group
                            If fEffect.AffectedID.Contains(newShip.DatabaseGroup) Then
                                processAtt = True
                            End If
                        Case EffectType.Category
                            If fEffect.AffectedID.Contains(newShip.DatabaseCategory) Then
                                processAtt = True
                            End If
                        Case EffectType.MarketGroup
                            If fEffect.AffectedID.Contains(newShip.MarketGroup) Then
                                processAtt = True
                            End If
                        Case EffectType.Skill
                            If newShip.RequiredSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(fEffect.AffectedID(0)))) Then
                                processAtt = True
                            End If
                    End Select
                    If processAtt = True Then
                        log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause & ": " & newShip.Attributes(att).ToString
                        newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                        log &= " --> " & newShip.Attributes(att).ToString
                        newShip.AuditLog.Add(log)
                    End If
                Next
            End If
        Next
        Ship.MapShipAttributes(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Skill Effects to Ship took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Private Shared Function ApplySkillEffectsToModules(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim aModule As New ShipModule
        Dim maxSlots As Integer = 0
        Dim att As String
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        Dim processAtt As Boolean = False
        Dim log As String = ""
        For slotType As Integer = 1 To 4
            Select Case slotType
                Case 1
                    maxSlots = newShip.HiSlots
                Case 2
                    maxSlots = newShip.MidSlots
                Case 3
                    maxSlots = newShip.LowSlots
                Case 4
                    maxSlots = newShip.RigSlots
            End Select
            For slot As Integer = 1 To maxSlots
                Select Case slotType
                    Case 1
                        aModule = newShip.HiSlot(slot)
                    Case 2
                        aModule = newShip.MidSlot(slot)
                    Case 3
                        aModule = newShip.LowSlot(slot)
                    Case 4
                        aModule = newShip.RigSlot(slot)
                End Select
                If aModule IsNot Nothing Then
                    For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                        att = CStr(aModule.Attributes.GetKey(attNo))
                        If SkillEffectsTable.Contains(att) = True Then
                            For Each fEffect As FinalEffect In CType(SkillEffectsTable(att), ArrayList)
                                processAtt = False
                                log = ""
                                Select Case fEffect.AffectedType
                                    Case EffectType.All
                                        processAtt = True
                                    Case EffectType.Item
                                        If fEffect.AffectedID.Contains(aModule.ID) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.Group
                                        If fEffect.AffectedID.Contains(aModule.DatabaseGroup) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.Category
                                        If fEffect.AffectedID.Contains(aModule.DatabaseCategory) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.MarketGroup
                                        If fEffect.AffectedID.Contains(aModule.MarketGroup) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.Skill
                                        If aModule.RequiredSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(fEffect.AffectedID(0)))) Then
                                            processAtt = True
                                        End If
                                End Select
                                If processAtt = True Then
                                    log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause & ": " & aModule.Attributes(att).ToString
                                    aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                                    log &= " --> " & aModule.Attributes(att).ToString
                                    aModule.AuditLog.Add(log)
                                End If
                            Next
                        End If
                    Next
                    'ShipModule.MapModuleAttributes(aModule)
                End If
            Next
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Skill Effects to Modules took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip

    End Function

    Private Shared Function ApplyModuleEffectsToModules(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim aModule As New ShipModule
        Dim maxSlots As Integer = 0
        Dim att As String = ""
        Dim log As String = ""
        Dim processAtt As Boolean = False
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For slotType As Integer = 1 To 4
            Select Case slotType
                Case 1
                    maxSlots = newShip.HiSlots
                Case 2
                    maxSlots = newShip.MidSlots
                Case 3
                    maxSlots = newShip.LowSlots
                Case 4
                    maxSlots = newShip.RigSlots
            End Select
            For slot As Integer = 1 To maxSlots
                Select Case slotType
                    Case 1
                        aModule = newShip.HiSlot(slot)
                    Case 2
                        aModule = newShip.MidSlot(slot)
                    Case 3
                        aModule = newShip.LowSlot(slot)
                    Case 4
                        aModule = newShip.RigSlot(slot)
                End Select
                If aModule IsNot Nothing Then
                    For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                        att = CStr(aModule.Attributes.GetKey(attNo))
                        If ModuleEffectsTable.Contains(att) = True Then
                            For Each fEffect As FinalEffect In CType(ModuleEffectsTable(att), ArrayList)
                                processAtt = False
                                log = ""
                                Select Case fEffect.AffectedType
                                    Case EffectType.All
                                        processAtt = True
                                    Case EffectType.Item
                                        If fEffect.AffectedID.Contains(aModule.ID) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.Group
                                        If fEffect.AffectedID.Contains(aModule.DatabaseGroup) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.Category
                                        If fEffect.AffectedID.Contains(aModule.DatabaseCategory) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.MarketGroup
                                        If fEffect.AffectedID.Contains(aModule.MarketGroup) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.Skill
                                        If aModule.RequiredSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(fEffect.AffectedID(0)))) Then
                                            processAtt = True
                                        End If
                                End Select
                                If processAtt = True Then
                                    log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause & ": " & aModule.Attributes(att).ToString
                                    Select Case fEffect.CalcType
                                        Case EffectCalcType.Percentage
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                                        Case EffectCalcType.Addition
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        Case EffectCalcType.Difference ' Used for resistances
                                            aModule.Attributes(att) = ((100 - CDbl(aModule.Attributes(att))) * fEffect.AffectedValue) + CDbl(aModule.Attributes(att))
                                    End Select
                                    log &= " --> " & aModule.Attributes(att).ToString
                                    aModule.AuditLog.Add(log)
                                End If
                            Next
                        End If
                    Next
                    ShipModule.MapModuleAttributes(aModule)
                End If
            Next
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Module Effects to Ship took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function


    Private Shared Function ApplyModuleEffectsToShip(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim att As String = ""
        Dim processAtt As Boolean = False
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = CStr(newShip.Attributes.GetKey(attNo))
            If ModuleEffectsTable.Contains(att) = True Then
                For Each fEffect As FinalEffect In CType(ModuleEffectsTable(att), ArrayList)
                    processAtt = False
                    Select Case fEffect.AffectedType
                        Case EffectType.All
                            processAtt = True
                        Case EffectType.Item
                            If fEffect.AffectedID.Contains(newShip.ID) Then
                                processAtt = True
                            End If
                        Case EffectType.Group
                            If fEffect.AffectedID.Contains(newShip.DatabaseGroup) Then
                                processAtt = True
                            End If
                        Case EffectType.Category
                            If fEffect.AffectedID.Contains(newShip.DatabaseCategory) Then
                                processAtt = True
                            End If
                        Case EffectType.MarketGroup
                            If fEffect.AffectedID.Contains(newShip.MarketGroup) Then
                                processAtt = True
                            End If
                        Case EffectType.Skill
                            If newShip.RequiredSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(fEffect.AffectedID(0)))) Then
                                processAtt = True
                            End If
                    End Select
                    If processAtt = True Then
                        Select Case fEffect.CalcType
                            Case EffectCalcType.Percentage
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                            Case EffectCalcType.Addition
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fEffect.AffectedValue
                            Case EffectCalcType.Difference ' Used for resistances
                                newShip.Attributes(att) = ((100 - CDbl(newShip.Attributes(att))) * fEffect.AffectedValue) + CDbl(newShip.Attributes(att))
                        End Select
                    End If
                Next
            End If
        Next
        Ship.MapShipAttributes(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Module Effects to Ship took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Private Shared Sub ApplyStackingPenalties()
        Dim baseEffectList As New ArrayList
        Dim finalEffectList As New ArrayList
        Dim tempPEffectList As New SortedList
        Dim tempNEffectList As New SortedList
        Dim attOrder(,) As Double
        Dim att As String
        For attNumber As Integer = 0 To ModuleEffectsTable.Keys.Count - 1
            att = CStr(ModuleEffectsTable.GetKey(attNumber))
            baseEffectList = CType(ModuleEffectsTable(att), ArrayList)
            tempPEffectList.Clear()
            tempNEffectList.Clear()
            finalEffectList = New ArrayList
            For Each fEffect As FinalEffect In baseEffectList
                If fEffect.StackNerf = True Then
                    If fEffect.AffectedValue >= 0 Then
                        tempPEffectList.Add(tempPEffectList.Count.ToString, fEffect)
                    Else
                        tempNEffectList.Add(tempNEffectList.Count.ToString, fEffect)
                    End If
                Else
                    finalEffectList.Add(fEffect)
                End If
            Next
            If tempPEffectList.Count > 0 Then
                ReDim attOrder(tempPEffectList.Count - 1, 1)
                Dim sEffect As FinalEffect
                For Each attNo As String In tempPEffectList.Keys
                    sEffect = CType(tempPEffectList(attNo), FinalEffect)
                    attOrder(CInt(attNo), 0) = CDbl(attNo)
                    attOrder(CInt(attNo), 1) = sEffect.AffectedValue
                Next
                ' Create a tag array ready to sort the skill times
                Dim tagArray(tempPEffectList.Count - 1) As Integer
                For a As Integer = 0 To tempPEffectList.Count - 1
                    tagArray(a) = a
                Next
                ' Initialize the comparer and sort
                Dim myComparer As New EveHQ.Core.Reports.ArrayComparerDouble(attOrder)
                Array.Sort(tagArray, myComparer)
                Array.Reverse(tagArray)
                ' Go through the data and apply the stacking penalty
                Dim idx As Integer = 0
                Dim penalty As Double = 0
                For i As Integer = 0 To tagArray.Length - 1
                    idx = tagArray(i)
                    sEffect = CType(tempPEffectList(idx.ToString), FinalEffect)
                    penalty = Math.Exp(-(i ^ 2 / 7.1289))
                    sEffect.AffectedValue = sEffect.AffectedValue * penalty
                    sEffect.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalEffectList.Add(sEffect)
                Next
            End If
            If tempNEffectList.Count > 0 Then
                ReDim attOrder(tempNEffectList.Count - 1, 1)
                Dim sEffect As FinalEffect
                For Each attNo As String In tempNEffectList.Keys
                    sEffect = CType(tempNEffectList(attNo), FinalEffect)
                    attOrder(CInt(attNo), 0) = CDbl(attNo)
                    attOrder(CInt(attNo), 1) = sEffect.AffectedValue
                Next
                ' Create a tag array ready to sort the skill times
                Dim tagArray(tempNEffectList.Count - 1) As Integer
                For a As Integer = 0 To tempNEffectList.Count - 1
                    tagArray(a) = a
                Next
                ' Initialize the comparer and sort
                Dim myComparer As New EveHQ.Core.Reports.ArrayComparerDouble(attOrder)
                Array.Sort(tagArray, myComparer)
                ' Go through the data and apply the stacking penalty
                Dim idx As Integer = 0
                Dim penalty As Double = 0
                For i As Integer = 0 To tagArray.Length - 1
                    idx = tagArray(i)
                    sEffect = CType(tempNEffectList(idx.ToString), FinalEffect)
                    penalty = Math.Exp(-(i ^ 2 / 7.1289))
                    sEffect.AffectedValue = sEffect.AffectedValue * penalty
                    sEffect.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalEffectList.Add(sEffect)
                Next
            End If
            ModuleEffectsTable(att) = finalEffectList
        Next
    End Sub

#End Region

End Class

Public Class Effect
    Public AffectingAtt As Integer
    Public AffectingType As Integer
    Public AffectingID As Integer
    Public AffectedAtt As Integer
    Public AffectedType As Integer
    Public AffectedID As New ArrayList
    Public StackNerf As Boolean
    Public IsPerLevel As Boolean
    Public CalcType As Integer
End Class

Public Class ShipEffect
    Public ShipID As Integer
    Public AffectingType As Integer
    Public AffectingID As Integer
    Public AffectedAtt As Integer
    Public AffectedType As Integer
    Public AffectedID As New ArrayList
    Public StackNerf As Boolean
    Public IsPerLevel As Boolean
    Public CalcType As Integer
    Public Value As Double
End Class

Public Class FinalEffect
    Public AffectedAtt As Integer
    Public AffectedType As Integer
    Public AffectedID As New ArrayList
    Public AffectedValue As Double
    Public StackNerf As Boolean
    Public Cause As String
    Public CalcType As Integer
End Class

Public Enum EffectType
    All = 0
    Item = 1
    Group = 2
    Category = 3
    MarketGroup = 4
    Skill = 5
End Enum

Public Enum EffectCalcType
    Percentage = 0
    Addition = 1
    Difference = 2
End Enum


