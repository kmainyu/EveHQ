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
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

Public Class Engine

    Public Shared EffectsMap As New SortedList
    Public Shared ShipEffectsMap As New SortedList
    Public Shared ImplantEffectsMap As New SortedList
    Public Shared SkillEffectsTable As New SortedList
    Public Shared BaseSkillEffectsTable As New SortedList
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
                newEffect.Status = CInt(EffectData(9))
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
    Public Shared Sub BuildImplantEffectsMap()
        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.ImplantEffects.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String
        ' Build the map
        ImplantEffectsMap.Clear()
        Dim ImplantEffectClassList As New ArrayList
        Dim newEffect As New ImplantEffect
        Dim IDs() As String
        Dim AttIDs() As String
        Dim Atts As New ArrayList
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                Atts.Clear()
                If EffectData(2).Contains(";") Then
                    AttIDs = EffectData(2).Split(";".ToCharArray)
                    For Each AttID As String In AttIDs
                        Atts.Add(AttID)
                    Next
                Else
                    Atts.Add(EffectData(2))
                End If
                For Each att As String In Atts
                    newEffect = New ImplantEffect
                    If ImplantEffectsMap.Contains((EffectData(0))) = True Then
                        ImplantEffectClassList = CType(ImplantEffectsMap(EffectData(0)), ArrayList)
                    Else
                        ImplantEffectClassList = New ArrayList
                        ImplantEffectsMap.Add(EffectData(0), ImplantEffectClassList)
                    End If
                    newEffect.ImplantName = CStr(EffectData(0))
                    newEffect.AffectingAtt = CInt(EffectData(1))
                    newEffect.AffectedAtt = CInt(att)
                    newEffect.AffectedType = CInt(EffectData(3))
                    If EffectData(4).Contains(";") = True Then
                        IDs = EffectData(4).Split(";".ToCharArray)
                        For Each ID As String In IDs
                            newEffect.AffectedID.Add(ID)
                        Next
                    Else
                        newEffect.AffectedID.Add(EffectData(4))
                    End If
                    newEffect.CalcType = CInt(EffectData(5))
                    newEffect.Value = CDbl(EffectData(6))
                    newEffect.IsGang = CBool(EffectData(7))
                    If EffectData(8).Contains(";") = True Then
                        IDs = EffectData(8).Split(";".ToCharArray)
                        For Each ID As String In IDs
                            newEffect.Groups.Add(ID)
                        Next
                    Else
                        newEffect.Groups.Add(EffectData(8))
                    End If
                    ImplantEffectClassList.Add(newEffect)
                Next
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
                                fEffect.Status = chkEffect.Status
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
    Public Shared Sub BuildImplantEffects(ByVal hPilot As HQFPilot)
        Dim sTime, eTime As Date
        sTime = Now
        ' Go through all the implants and see what needs to be mapped
        Dim fEffect As New FinalEffect
        Dim fEffectList As New ArrayList
        Dim hImplant As String = ""
        Dim aImplant As ShipModule
        For slotNo As Integer = 1 To 10
            hImplant = hPilot.ImplantName(slotNo)
            If hImplant <> "" Then
                ' Go through the attributes
                aImplant = CType(ModuleLists.moduleList(ModuleLists.moduleListName(hImplant)), ShipModule)
                If ImplantEffectsMap.Contains(hImplant) = True Then
                    For Each chkEffect As ImplantEffect In CType(ImplantEffectsMap(hImplant), ArrayList)
                        If aImplant.Attributes.Contains(chkEffect.AffectingAtt.ToString) = True Then
                            fEffect = New FinalEffect
                            fEffect.AffectedAtt = chkEffect.AffectedAtt
                            fEffect.AffectedType = chkEffect.AffectedType
                            fEffect.AffectedID = chkEffect.AffectedID
                            fEffect.AffectedValue = CDbl(aImplant.Attributes(chkEffect.AffectingAtt.ToString))
                            fEffect.StackNerf = False
                            fEffect.Cause = aImplant.Name
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
            End If
        Next
        BaseSkillEffectsTable = Engine.CloneSortedList(SkillEffectsTable)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Building Skill Effects took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
    End Sub
    Public Shared Sub BuildShipEffects(ByVal hPilot As HQFPilot, ByVal hShip As Ship)
        If hShip IsNot Nothing Then
            Dim sTime, eTime As Date
            sTime = Now
            ' Go through all the skills and see what needs to be mapped
            ' Reset the SkillEffectsTable
            SkillEffectsTable = Engine.CloneSortedList(BaseSkillEffectsTable)
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
            'MessageBox.Show("Building Ship Effects took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
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
                                    Case EffectType.Slot
                                        processData = True
                                End Select
                                If processData = True And (aModule.ModuleState And chkEffect.Status) = aModule.ModuleState Then
                                    fEffect = New FinalEffect
                                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                                    fEffect.AffectedType = chkEffect.AffectedType
                                    If chkEffect.AffectedType = EffectType.Slot Then
                                        fEffect.AffectedID.Add(aModule.SlotNo)
                                    Else
                                        fEffect.AffectedID = chkEffect.AffectedID
                                    End If
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
                    If aModule.LoadedCharge IsNot Nothing Then
                        For Each att As String In aModule.LoadedCharge.Attributes.Keys
                            If EffectsMap.Contains(att) = True Then
                                For Each chkEffect As Effect In CType(EffectsMap(att), ArrayList)
                                    processData = False
                                    Select Case chkEffect.AffectingType
                                        Case EffectType.All
                                            processData = True
                                        Case EffectType.Item
                                            If chkEffect.AffectingID.ToString = aModule.LoadedCharge.ID Then
                                                processData = True
                                            End If
                                        Case EffectType.Group
                                            If chkEffect.AffectingID.ToString = aModule.LoadedCharge.DatabaseGroup Then
                                                processData = True
                                            End If
                                        Case EffectType.Category
                                            If chkEffect.AffectingID.ToString = aModule.LoadedCharge.DatabaseCategory Then
                                                processData = True
                                            End If
                                        Case EffectType.MarketGroup
                                            If chkEffect.AffectingID.ToString = aModule.LoadedCharge.MarketGroup Then
                                                processData = True
                                            End If
                                        Case EffectType.Skill
                                            If aModule.LoadedCharge.RequiredSkills.Contains(chkEffect.AffectingID.ToString) Then
                                                processData = True
                                            End If
                                        Case EffectType.Slot
                                            processData = True
                                    End Select
                                    If processData = True And (aModule.LoadedCharge.ModuleState And chkEffect.Status) = aModule.LoadedCharge.ModuleState Then
                                        fEffect = New FinalEffect
                                        fEffect.AffectedAtt = chkEffect.AffectedAtt
                                        fEffect.AffectedType = chkEffect.AffectedType
                                        If chkEffect.AffectedType = EffectType.Slot Then
                                            fEffect.AffectedID.Add(aModule.SlotNo)
                                        Else
                                            fEffect.AffectedID = chkEffect.AffectedID
                                        End If
                                        fEffect.AffectedValue = CDbl(aModule.LoadedCharge.Attributes(att))
                                        fEffect.StackNerf = chkEffect.StackNerf
                                        fEffect.Cause = aModule.LoadedCharge.Name
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
                    End If ' End of LoadedCharge checking
                End If ' End of Module checking
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
        newShip = Engine.BuildModuleEffects(newShip)
        Call Engine.ApplyStackingPenalties()
        newShip = Engine.ApplyModuleEffectsToShip(newShip)
        newShip = Engine.CalculateDamageStatistics(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying the whole fitting took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Ship.MapShipAttributes(newShip)
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
                        Select Case fEffect.CalcType
                            Case EffectCalcType.Percentage
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                            Case EffectCalcType.Addition
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fEffect.AffectedValue
                            Case EffectCalcType.Difference ' Used for resistances
                                newShip.Attributes(att) = ((100 - CDbl(newShip.Attributes(att))) * (fEffect.AffectedValue / 100)) + CDbl(newShip.Attributes(att))
                            Case EffectCalcType.Absolute
                                newShip.Attributes(att) = fEffect.AffectedValue
                            Case EffectCalcType.Multiplier
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * fEffect.AffectedValue
                        End Select
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
        Dim att As String = ""
        Dim oldAtt As String = ""
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
                                    Case EffectType.Slot
                                        If fEffect.AffectedID.Contains(aModule.SlotNo) Then
                                            processAtt = True
                                        End If
                                End Select
                                If processAtt = True Then
                                    oldatt = aModule.Attributes(att).ToString
                                    log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause & ": " & oldatt
                                    Select Case fEffect.CalcType
                                        Case EffectCalcType.Percentage
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                                        Case EffectCalcType.Addition
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        Case EffectCalcType.Difference ' Used for resistances
                                            aModule.Attributes(att) = ((100 - CDbl(aModule.Attributes(att))) * (fEffect.AffectedValue / 100)) + CDbl(aModule.Attributes(att))
                                        Case EffectCalcType.Absolute
                                            aModule.Attributes(att) = fEffect.AffectedValue
                                        Case EffectCalcType.Multiplier
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * fEffect.AffectedValue
                                    End Select
                                    log &= " --> " & aModule.Attributes(att).ToString
                                    If oldatt <> aModule.Attributes(att).ToString Then
                                        aModule.AuditLog.Add(log)
                                    End If
                                End If
                            Next
                        End If
                    Next
                    If aModule.LoadedCharge IsNot Nothing Then
                        For attNo As Integer = 0 To aModule.LoadedCharge.Attributes.Keys.Count - 1
                            att = CStr(aModule.LoadedCharge.Attributes.GetKey(attNo))
                            If SkillEffectsTable.Contains(att) = True Then
                                For Each fEffect As FinalEffect In CType(SkillEffectsTable(att), ArrayList)
                                    processAtt = False
                                    log = ""
                                    Select Case fEffect.AffectedType
                                        Case EffectType.All
                                            processAtt = True
                                        Case EffectType.Item
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.ID) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.Group
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.DatabaseGroup) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.Category
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.DatabaseCategory) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.MarketGroup
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.MarketGroup) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.Skill
                                            If aModule.LoadedCharge.RequiredSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(fEffect.AffectedID(0)))) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.Slot
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.SlotNo) Then
                                                processAtt = True
                                            End If
                                    End Select
                                    If processAtt = True Then
                                        oldAtt = aModule.LoadedCharge.Attributes(att).ToString
                                        log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause & ": " & oldAtt
                                        Select Case fEffect.CalcType
                                            Case EffectCalcType.Percentage
                                                aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                                            Case EffectCalcType.Addition
                                                aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) + fEffect.AffectedValue
                                            Case EffectCalcType.Difference ' Used for resistances
                                                aModule.LoadedCharge.Attributes(att) = ((100 - CDbl(aModule.LoadedCharge.Attributes(att))) * (fEffect.AffectedValue / 100)) + CDbl(aModule.LoadedCharge.Attributes(att))
                                            Case EffectCalcType.Absolute
                                                aModule.LoadedCharge.Attributes(att) = fEffect.AffectedValue
                                            Case EffectCalcType.Multiplier
                                                aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) * fEffect.AffectedValue
                                        End Select
                                        log &= " --> " & aModule.LoadedCharge.Attributes(att).ToString
                                        If oldAtt <> aModule.LoadedCharge.Attributes(att).ToString Then
                                            aModule.LoadedCharge.AuditLog.Add(log)
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    End If
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
        Dim oldAtt As String = ""
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
                                    Case EffectType.Slot
                                        If fEffect.AffectedID.Contains(aModule.SlotNo) Then
                                            processAtt = True
                                        End If
                                End Select
                                If processAtt = True Then
                                    log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause
                                    If aModule.Name = fEffect.Cause Then
                                        log &= " (Overloading)"
                                    End If
                                    oldAtt = aModule.Attributes(att).ToString()
                                    log &= ": " & oldAtt
                                    Select Case fEffect.CalcType
                                        Case EffectCalcType.Percentage
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                                        Case EffectCalcType.Addition
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        Case EffectCalcType.Difference ' Used for resistances
                                            aModule.Attributes(att) = ((100 - CDbl(aModule.Attributes(att))) * (fEffect.AffectedValue / 100)) + CDbl(aModule.Attributes(att))
                                        Case EffectCalcType.Absolute
                                            aModule.Attributes(att) = fEffect.AffectedValue
                                        Case EffectCalcType.Multiplier
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * fEffect.AffectedValue
                                    End Select
                                    log &= " --> " & aModule.Attributes(att).ToString
                                    If oldatt <> aModule.Attributes(att).ToString Then
                                        aModule.AuditLog.Add(log)
                                    End If
                                End If
                            Next
                        End If
                    Next
                    If aModule.LoadedCharge IsNot Nothing Then
                        For attNo As Integer = 0 To aModule.LoadedCharge.Attributes.Keys.Count - 1
                            att = CStr(aModule.LoadedCharge.Attributes.GetKey(attNo))
                            If ModuleEffectsTable.Contains(att) = True Then
                                For Each fEffect As FinalEffect In CType(ModuleEffectsTable(att), ArrayList)
                                    processAtt = False
                                    log = ""
                                    Select Case fEffect.AffectedType
                                        Case EffectType.All
                                            processAtt = True
                                        Case EffectType.Item
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.ID) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.Group
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.DatabaseGroup) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.Category
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.DatabaseCategory) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.MarketGroup
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.MarketGroup) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.Skill
                                            If aModule.LoadedCharge.RequiredSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(fEffect.AffectedID(0)))) Then
                                                processAtt = True
                                            End If
                                        Case EffectType.Slot
                                            If fEffect.AffectedID.Contains(aModule.LoadedCharge.SlotNo) Then
                                                processAtt = True
                                            End If
                                    End Select
                                    If processAtt = True Then
                                        log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause
                                        If aModule.LoadedCharge.Name = fEffect.Cause Then
                                            log &= " (Overloading)"
                                        End If
                                        oldAtt = aModule.LoadedCharge.Attributes(att).ToString()
                                        log &= ": " & oldAtt
                                        Select Case fEffect.CalcType
                                            Case EffectCalcType.Percentage
                                                aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) * (1 + (fEffect.AffectedValue / 100))
                                            Case EffectCalcType.Addition
                                                aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) + fEffect.AffectedValue
                                            Case EffectCalcType.Difference ' Used for resistances
                                                aModule.LoadedCharge.Attributes(att) = ((100 - CDbl(aModule.LoadedCharge.Attributes(att))) * (fEffect.AffectedValue / 100)) + CDbl(aModule.LoadedCharge.Attributes(att))
                                            Case EffectCalcType.Absolute
                                                aModule.LoadedCharge.Attributes(att) = fEffect.AffectedValue
                                            Case EffectCalcType.Multiplier
                                                aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) * fEffect.AffectedValue
                                        End Select
                                        log &= " --> " & aModule.LoadedCharge.Attributes(att).ToString
                                        If oldAtt <> aModule.LoadedCharge.Attributes(att).ToString Then
                                            aModule.LoadedCharge.AuditLog.Add(log)
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    End If
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
                                newShip.Attributes(att) = ((100 - CDbl(newShip.Attributes(att))) * (-fEffect.AffectedValue / 100)) + CDbl(newShip.Attributes(att))
                            Case EffectCalcType.Velocity
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + (CDbl(newShip.Attributes(att)) * (CDbl(newShip.Attributes("10010")) / CDbl(newShip.Attributes("10002")) * (fEffect.AffectedValue / 100)))
                            Case EffectCalcType.Absolute
                                newShip.Attributes(att) = fEffect.AffectedValue
                            Case EffectCalcType.Multiplier
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * fEffect.AffectedValue
                        End Select
                    End If
                Next
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Module Effects to Ship took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Private Shared Sub ApplyStackingPenalties()
        Dim sTime, eTime As Date
        sTime = Now
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
                    Select Case sEffect.CalcType
                        Case EffectCalcType.Multiplier
                            sEffect.AffectedValue = ((sEffect.AffectedValue - 1) * penalty) + 1
                        Case Else
                            sEffect.AffectedValue = sEffect.AffectedValue * penalty
                    End Select
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
                    Select Case sEffect.CalcType
                        Case EffectCalcType.Multiplier
                            sEffect.AffectedValue = ((sEffect.AffectedValue - 1) * penalty) + 1
                        Case Else
                            sEffect.AffectedValue = sEffect.AffectedValue * penalty
                    End Select
                    sEffect.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalEffectList.Add(sEffect)
                Next
            End If
            ModuleEffectsTable(att) = finalEffectList
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Stacking Penalties took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
    End Sub

    Private Shared Function CalculateDamageStatistics(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        Dim cModule As New ShipModule
        Dim dgmMod As Double = 1
        Dim ROF As Double = 1
        For slot As Integer = 1 To newShip.HiSlots
            cModule = newShip.HiSlot(slot)
            If cModule IsNot Nothing Then
                If cModule.IsTurret Or cModule.IsLauncher Then
                    If cModule.LoadedCharge IsNot Nothing Then
                        cModule.Attributes("10030") = CInt(cModule.LoadedCharge.ID)
                        cModule.Attributes("10017") = CDbl(cModule.LoadedCharge.Attributes("114")) + CDbl(cModule.LoadedCharge.Attributes("116")) + CDbl(cModule.LoadedCharge.Attributes("117")) + CDbl(cModule.LoadedCharge.Attributes("118"))
                        If cModule.IsTurret = True Then
                            dgmMod = CDbl(cModule.Attributes("10014")) + CDbl(cModule.Attributes("10015")) + CDbl(cModule.Attributes("10016"))
                            ROF = CDbl(cModule.Attributes("10011")) + CDbl(cModule.Attributes("10012")) + CDbl(cModule.Attributes("10013"))
                            cModule.Attributes("10018") = dgmMod * CDbl(cModule.Attributes("10017"))
                            cModule.Attributes("10019") = CDbl(cModule.Attributes("10018")) / ROF
                            newShip.Attributes("10020") = CDbl(newShip.Attributes("10020")) + CDbl(cModule.Attributes("10018"))
                            newShip.Attributes("10024") = CDbl(newShip.Attributes("10024")) + CDbl(cModule.Attributes("10019"))
                            newShip.Attributes("10028") = CDbl(newShip.Attributes("10028")) + CDbl(cModule.Attributes("10018"))
                            newShip.Attributes("10029") = CDbl(newShip.Attributes("10029")) + CDbl(cModule.Attributes("10019"))
                        Else
                            dgmMod = 1
                            ROF = CDbl(cModule.Attributes("51"))
                            cModule.Attributes("10018") = dgmMod * CDbl(cModule.Attributes("10017"))
                            cModule.Attributes("10019") = CDbl(cModule.Attributes("10018")) / ROF
                            newShip.Attributes("10021") = CDbl(newShip.Attributes("10021")) + CDbl(cModule.Attributes("10018"))
                            newShip.Attributes("10025") = CDbl(newShip.Attributes("10025")) + CDbl(cModule.Attributes("10019"))
                            newShip.Attributes("10028") = CDbl(newShip.Attributes("10028")) + CDbl(cModule.Attributes("10018"))
                            newShip.Attributes("10029") = CDbl(newShip.Attributes("10029")) + CDbl(cModule.Attributes("10019"))
                        End If
                    End If
                Else
                    If cModule.DatabaseGroup = "72" Then
                        ' Do smartbomb code
                        cModule.Attributes("10017") = CDbl(cModule.Attributes("114")) + CDbl(cModule.Attributes("116")) + CDbl(cModule.Attributes("117")) + CDbl(cModule.Attributes("118"))
                        cModule.Attributes("10018") = CDbl(cModule.Attributes("10017"))
                        cModule.Attributes("10019") = CDbl(cModule.Attributes("10018")) / CDbl(cModule.Attributes("73"))
                        newShip.Attributes("10022") = CDbl(newShip.Attributes("10022")) + CDbl(cModule.Attributes("10018"))
                        newShip.Attributes("10026") = CDbl(newShip.Attributes("10026")) + CDbl(cModule.Attributes("10019"))
                        newShip.Attributes("10028") = CDbl(newShip.Attributes("10028")) + CDbl(cModule.Attributes("10018"))
                        newShip.Attributes("10029") = CDbl(newShip.Attributes("10029")) + CDbl(cModule.Attributes("10019"))
                    End If
                End If
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Calculating Damage Effects took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

#End Region

#Region "Cloning"
    Public Shared Function CloneSortedList(ByVal oldSortedList As SortedList) As SortedList
        Dim myMemoryStream As New MemoryStream(10000)
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(myMemoryStream, oldSortedList)
        myMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newSortedList As SortedList = CType(objBinaryFormatter.Deserialize(myMemoryStream), SortedList)
        myMemoryStream.Close()
        Return newSortedList
    End Function
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
    Public Status As Integer
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
    Public Status As Integer
    Public Value As Double
End Class

Public Class ImplantEffect
    Public ImplantName As String
    Public AffectingAtt As Integer
    Public AffectedAtt As Integer
    Public AffectedType As Integer
    Public AffectedID As New ArrayList
    Public CalcType As Integer
    Public Status As Integer
    Public Value As Double
    Public IsGang As Boolean
    Public Groups As New ArrayList
End Class

<Serializable()> Public Class FinalEffect
    Public AffectedAtt As Integer
    Public AffectedType As Integer
    Public AffectedID As New ArrayList
    Public AffectedValue As Double
    Public StackNerf As Boolean
    Public Cause As String
    Public CalcType As Integer
    Public Status As Integer
End Class

Public Enum EffectType
    All = 0
    Item = 1
    Group = 2
    Category = 3
    MarketGroup = 4
    Skill = 5
    Slot = 6
End Enum

Public Enum EffectCalcType
    Percentage = 0
    Addition = 1 ' For adding values
    Difference = 2 ' For resistances
    Velocity = 3 ' For AB/MWD calculations
    Absolute = 4 ' For setting values
    Multiplier = 5 ' Damage multiplier
End Enum


