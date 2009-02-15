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
    Public Shared FleetEffectsMap As New SortedList
    Public Shared ImplantEffectsMap As New SortedList
    Public Shared SkillEffectsTable As New SortedList
    Public Shared BaseSkillEffectsTable As New SortedList
    Public Shared ModuleEffectsTable As New SortedList
    Public Shared ChargeEffectsTable As New SortedList
    Public Shared PirateImplants As New SortedList
    Public Shared PirateImplantGroups As New SortedList

    Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

#Region "New Routines"
    Public Shared Sub BuildPirateImplants()
        Dim PirateImplantComponents As New ArrayList
        PirateImplantGroups.Clear()
        PirateImplantGroups.Add("Crystal", 1)
        PirateImplantGroups.Add("Halo", 1)
        PirateImplantGroups.Add("Slave", 1)
        PirateImplantGroups.Add("Snake", 1)
        PirateImplantGroups.Add("Talisman", 1)
        PirateImplantGroups.Add("Low-grade Centurion", 1)
        PirateImplantGroups.Add("Low-grade Crystal", 1)
        PirateImplantGroups.Add("Low-grade Edge", 1)
        PirateImplantGroups.Add("Low-grade Halo", 1)
        PirateImplantGroups.Add("Low-grade Harvest", 1)
        PirateImplantGroups.Add("Low-grade Nomad", 1)
        PirateImplantGroups.Add("Low-grade Slave", 1)
        PirateImplantGroups.Add("Low-grade Snake", 1)
        PirateImplantGroups.Add("Low-grade Talisman", 1)
        PirateImplantGroups.Add("Low-grade Virtue", 1)
        PirateImplantComponents.Clear()
        PirateImplantComponents.Add(" Alpha")
        PirateImplantComponents.Add(" Beta")
        PirateImplantComponents.Add(" Delta")
        PirateImplantComponents.Add(" Epsilon")
        PirateImplantComponents.Add(" Gamma")
        PirateImplantComponents.Add(" Omega")
        PirateImplants.Clear()
        For Each group As String In PirateImplantGroups.Keys
            For Each component As String In PirateImplantComponents
                PirateImplants.Add(group & component, group)
            Next
        Next
    End Sub
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
                newEffect.Value = Double.Parse(EffectData(9), Globalization.NumberStyles.Number, culture)
                newEffect.Status = CInt(EffectData(10))
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
                If EffectData(3).Contains(";") Then
                    AttIDs = EffectData(3).Split(";".ToCharArray)
                    For Each AttID As String In AttIDs
                        Atts.Add(AttID)
                    Next
                Else
                    Atts.Add(EffectData(3))
                End If
                For Each att As String In Atts
                    newEffect = New ImplantEffect
                    If ImplantEffectsMap.Contains((EffectData(10))) = True Then
                        ImplantEffectClassList = CType(ImplantEffectsMap(EffectData(10)), ArrayList)
                    Else
                        ImplantEffectClassList = New ArrayList
                        ImplantEffectsMap.Add(EffectData(10), ImplantEffectClassList)
                    End If
                    newEffect.ImplantName = CStr(EffectData(10))
                    newEffect.AffectingAtt = CInt(EffectData(0))
                    newEffect.AffectedAtt = CInt(att)
                    newEffect.AffectedType = CInt(EffectData(4))
                    If EffectData(5).Contains(";") = True Then
                        IDs = EffectData(5).Split(";".ToCharArray)
                        For Each ID As String In IDs
                            newEffect.AffectedID.Add(ID)
                        Next
                    Else
                        newEffect.AffectedID.Add(EffectData(5))
                    End If
                    newEffect.CalcType = CInt(EffectData(6))
                    Dim cImplant As ShipModule = CType(Implants.implantList(newEffect.ImplantName), ShipModule)
                    newEffect.Value = CDbl(cImplant.Attributes(EffectData(0)))
                    newEffect.IsGang = CBool(EffectData(8))
                    If EffectData(9).Contains(";") = True Then
                        IDs = EffectData(9).Split(";".ToCharArray)
                        For Each ID As String In IDs
                            newEffect.Groups.Add(ID)
                        Next
                    Else
                        newEffect.Groups.Add(EffectData(9))
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
                Try
                    For Each att As String In aSkill.Attributes.Keys
                        If EffectsMap.Contains(att) = True Then
                            For Each chkEffect As Effect In CType(EffectsMap(att), ArrayList)
                                If chkEffect.AffectingType = EffectType.Item And chkEffect.AffectingID = CInt(aSkill.ID) Then
                                    fEffect = New FinalEffect
                                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                                    fEffect.AffectedType = chkEffect.AffectedType
                                    fEffect.AffectedID = chkEffect.AffectedID
                                    fEffect.AffectedValue = CDbl(aSkill.Attributes(att)) * hSkill.Level
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
                Catch e As Exception
                    MessageBox.Show("An error occured trying to process the skill effects for " & hSkill.Name & "!", "HQF Engine Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub
    Public Shared Sub BuildImplantEffects(ByVal hPilot As HQFPilot)
        Dim sTime, eTime As Date
        sTime = Now
        ' Run through the implants and see if we have any pirate implants
        Dim hImplant As String = ""
        Dim aImplant As ShipModule
        Dim PIGroup As String = ""
        Dim cPirateImplantGroups As SortedList = CType(PirateImplantGroups.Clone, Collections.SortedList)
        For slotNo As Integer = 1 To 10
            hImplant = hPilot.ImplantName(slotNo)
            If PirateImplants.Contains(hImplant) = True Then
                ' We have a pirate implant so let's work out the group and the set bonus
                PIGroup = CStr(PirateImplants.Item(hImplant))
                aImplant = CType(ModuleLists.moduleList(ModuleLists.moduleListName(hImplant)), ShipModule)
                Select Case PIGroup
                    Case "Low-grade Centurion"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1293"))
                    Case "Crystal", "Low-grade Crystal"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("838"))
                    Case "Low-grade Edge"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1291"))
                    Case "Halo", "Low-grade Halo"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("863"))
                    Case "Low-grade Harvest"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1292"))
                    Case "Low-grade Nomad"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1282"))
                    Case "Slave", "Low-grade Slave"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("864"))
                    Case "Snake", "Low-grade Snake"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("802"))
                    Case "Talisman", "Low-grade Talisman"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("799"))
                    Case "Low-grade Virtue"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1284"))
                End Select

            End If
        Next

        ' Go through all the implants and see what needs to be mapped
        Dim fEffect As New FinalEffect
        Dim fEffectList As New ArrayList
        
        For slotNo As Integer = 1 To 10
            hImplant = hPilot.ImplantName(slotNo)
            If hImplant <> "" Then
                ' Go through the attributes
                aImplant = CType(ModuleLists.moduleList(ModuleLists.moduleListName(hImplant)), ShipModule)
                If ImplantEffectsMap.Contains(hImplant) = True Then
                    For Each chkEffect As ImplantEffect In CType(ImplantEffectsMap(hImplant), ArrayList)
                        If chkEffect.IsGang = False Then
                            If aImplant.Attributes.Contains(chkEffect.AffectingAtt.ToString) = True Then
                                fEffect = New FinalEffect
                                fEffect.AffectedAtt = chkEffect.AffectedAtt
                                fEffect.AffectedType = chkEffect.AffectedType
                                fEffect.AffectedID = chkEffect.AffectedID
                                If PirateImplants.Contains(aImplant.Name) = True Then
                                    PIGroup = CStr(PirateImplants.Item(hImplant))
                                    fEffect.AffectedValue = CDbl(aImplant.Attributes(chkEffect.AffectingAtt.ToString)) * CDbl(cPirateImplantGroups.Item(PIGroup))
                                    fEffect.Cause = aImplant.Name & " (Set Bonus: " & FormatNumber(cPirateImplantGroups.Item(PIGroup), 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "x)"
                                Else
                                    fEffect.AffectedValue = CDbl(aImplant.Attributes(chkEffect.AffectingAtt.ToString))
                                    fEffect.Cause = aImplant.Name
                                End If
                                fEffect.StackNerf = False
                                fEffect.CalcType = chkEffect.CalcType
                                If SkillEffectsTable.Contains(fEffect.AffectedAtt.ToString) = False Then
                                    fEffectList = New ArrayList
                                    SkillEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                Else
                                    fEffectList = CType(SkillEffectsTable(fEffect.AffectedAtt.ToString), Collections.ArrayList)
                                End If
                                fEffectList.Add(fEffect)
                            End If
                        End If
                    Next
                End If
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub
    Public Shared Sub BuildShipEffects(ByVal hPilot As HQFPilot, ByVal hShip As Ship)
        If hShip IsNot Nothing Then
            Dim sTime, eTime As Date
            sTime = Now
            ' Go through all the skills and see what needs to be mapped
            ' Reset the SkillEffectsTable
            Dim shipRoles As New ArrayList
            Dim hSkill As New HQFSkill
            Dim fEffect As New FinalEffect
            Dim fEffectList As New ArrayList
            shipRoles = CType(ShipEffectsMap(hShip.ID), ArrayList)
            If shipRoles IsNot Nothing Then
                For Each chkEffect As ShipEffect In shipRoles
                    If chkEffect.Status <> 16 Then
                        fEffect = New FinalEffect
                        If hPilot.SkillSet.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))) = True Then
                            hSkill = CType(hPilot.SkillSet(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))), HQFSkill)
                            If chkEffect.IsPerLevel = True Then
                                fEffect.AffectedValue = chkEffect.Value * hSkill.Level
                                fEffect.Cause = "Ship Bonus - " & hSkill.Name & " (Level " & hSkill.Level & ")"
                            Else
                                fEffect.AffectedValue = chkEffect.Value
                                fEffect.Cause = "Ship Role - "
                            End If
                        Else
                            fEffect.AffectedValue = chkEffect.Value
                            fEffect.Cause = "Ship Role - "
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
                    End If
                Next
            End If
            BaseSkillEffectsTable = Engine.CloneSortedList(SkillEffectsTable)
            eTime = Now
            Dim dTime As TimeSpan = eTime - sTime
        End If
    End Sub
    Public Shared Sub BuildModuleEffects(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        ' Clear the Effects Table
        ModuleEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim attData As New ArrayList
        Dim fEffect As New FinalEffect
        Dim fEffectList As New ArrayList
        Dim processData As Boolean = False
        For Each aModule As ShipModule In newShip.SlotCollection
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
                            Case EffectType.Attribute
                                If aModule.Attributes.Contains(chkEffect.AffectingID.ToString) Then
                                    processData = True
                                End If
                        End Select
                        If processData = True And (aModule.ModuleState And chkEffect.Status) = aModule.ModuleState Then
                            fEffect = New FinalEffect
                            fEffect.AffectedAtt = chkEffect.AffectedAtt
                            fEffect.AffectedType = chkEffect.AffectedType
                            If chkEffect.AffectedType = EffectType.Slot Then
                                fEffect.AffectedID.Add(aModule.SlotType & aModule.SlotNo)
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
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub
    Public Shared Sub BuildChargeEffects(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        ' Clear the Effects Table
        ChargeEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim attData As New ArrayList
        Dim fEffect As New FinalEffect
        Dim fEffectList As New ArrayList
        Dim processData As Boolean = False
        For Each aModule As ShipModule In newShip.SlotCollection
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
                                Case EffectType.Attribute
                                    If aModule.LoadedCharge.Attributes.Contains(chkEffect.AffectingID.ToString) Then
                                        processData = True
                                    End If
                            End Select
                            If processData = True And (aModule.LoadedCharge.ModuleState And chkEffect.Status) = aModule.LoadedCharge.ModuleState Then
                                fEffect = New FinalEffect
                                fEffect.AffectedAtt = chkEffect.AffectedAtt
                                fEffect.AffectedType = chkEffect.AffectedType
                                If chkEffect.AffectedType = EffectType.Slot Then
                                    fEffect.AffectedID.Add(aModule.SlotType & aModule.SlotNo)
                                Else
                                    fEffect.AffectedID = chkEffect.AffectedID
                                End If
                                fEffect.AffectedValue = CDbl(aModule.LoadedCharge.Attributes(att))
                                fEffect.StackNerf = chkEffect.StackNerf
                                fEffect.Cause = aModule.LoadedCharge.Name
                                fEffect.CalcType = chkEffect.CalcType
                                If ChargeEffectsTable.Contains(fEffect.AffectedAtt.ToString) = False Then
                                    fEffectList = New ArrayList
                                    ChargeEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                Else
                                    fEffectList = CType(ChargeEffectsTable(fEffect.AffectedAtt.ToString), Collections.ArrayList)
                                End If
                                fEffectList.Add(fEffect)
                            End If
                        Next
                    End If
                Next
            End If ' End of LoadedCharge checking
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub

    Public Shared Function ApplyFitting(ByVal baseShip As Ship, ByVal shipPilot As HQFPilot, Optional ByVal BuildMethod As Integer = 0) As Ship
        ' Setup performance info - just in case!
        Dim stages As Integer = 18
        Dim pStages(stages) As String
        Dim pStageTime(stages) As DateTime
        pStages(0) = "Start Timing: "
        pStages(1) = "Building Skill Effects: "
        pStages(2) = "Building Implant Effects: "
        pStages(3) = "Building Ship Effects: "
        pStages(4) = "Applying Skill Effects to Ship: "
        pStages(5) = "Aplpying Skill Effects to Modules: "
        pStages(6) = "Applying Skill Effects to Drones: "
        pStages(7) = "Build Charge Effects: "
        pStages(8) = "Applying Charge Effects to Modules: "
        pStages(9) = "Applying Charge Effects to Ship: "
        pStages(10) = "Building Module Effects: "
        pStages(11) = "Applying Stacking Penalties: "
        pStages(12) = "Applying Module Effects to Modules: "
        pStages(13) = "Rebuilding Module Effects: "
        pStages(14) = "Recalculating Stacking Penalties: "
        pStages(15) = "Applying Module Effects to Drones: "
        pStages(16) = "Applying Module Effects to Ship: "
        pStages(17) = "Calculating Damage Statistics: "
        pStages(18) = "Calculating Defence Statistics: "
        ' Apply the pilot skills to the ship
        Dim newShip As New Ship
        Select Case BuildMethod
            Case BuildType.BuildEverything
                pStageTime(0) = Now
                Engine.BuildSkillEffects(shipPilot)
                pStageTime(1) = Now
                Engine.BuildImplantEffects(shipPilot)
                pStageTime(2) = Now
                Engine.BuildShipEffects(shipPilot, baseShip)
                pStageTime(3) = Now
                newShip = Engine.CollectModules(CType(baseShip.Clone, Ship))
                Engine.ApplySkillEffectsToShip(newShip)
                pStageTime(4) = Now
                Engine.ApplySkillEffectsToModules(newShip)
                pStageTime(5) = Now
                Engine.ApplySkillEffectsToDrones(newShip)
                pStageTime(6) = Now
                Engine.BuildChargeEffects(newShip)
                pStageTime(10) = Now
                Engine.ApplyChargeEffectsToModules(newShip)
                pStageTime(11) = Now
                Engine.ApplyChargeEffectsToShip(newShip)
                pStageTime(12) = Now
                Engine.BuildModuleEffects(newShip)
                pStageTime(7) = Now
                Call Engine.ApplyStackingPenalties()
                pStageTime(8) = Now
                Engine.ApplyModuleEffectsToModules(newShip)
                pStageTime(9) = Now
                Engine.BuildModuleEffects(newShip)
                pStageTime(13) = Now
                Call Engine.ApplyStackingPenalties()
                pStageTime(14) = Now
                Engine.ApplyModuleEffectsToDrones(newShip)
                pStageTime(15) = Now
                Engine.ApplyModuleEffectsToShip(newShip)
                pStageTime(16) = Now
                Engine.CalculateDamageStatistics(newShip)
                pStageTime(17) = Now
                Ship.MapShipAttributes(newShip)
                Engine.CalculateDefenceStatistics(newShip)
                pStageTime(18) = Now
            Case BuildType.BuildEffectsMaps
                pStageTime(0) = Now
                Engine.BuildSkillEffects(shipPilot)
                pStageTime(1) = Now
                Engine.BuildImplantEffects(shipPilot)
                pStageTime(2) = Now
                Engine.BuildShipEffects(shipPilot, baseShip)
                pStageTime(3) = Now
                'newShip = Engine.CollectModules(CType(baseShip.Clone, Ship))
                'Engine.ApplySkillEffectsToShip(newShip)
                pStageTime(4) = Now
                'Engine.ApplySkillEffectsToModules(newShip)
                pStageTime(5) = Now
                'Engine.ApplySkillEffectsToDrones(newShip)
                pStageTime(6) = Now
                'Engine.BuildModuleEffects(newShip)
                pStageTime(7) = Now
                'Engine.ApplyStackingPenalties()
                pStageTime(8) = Now
                'Engine.ApplyModuleEffectsToModules(newShip)
                pStageTime(9) = Now
                'Engine.BuildChargeEffects(newShip)
                pStageTime(10) = Now
                'Engine.ApplyChargeEffectsToModules(newShip)
                pStageTime(11) = Now
                'Engine.ApplyChargeEffectsToShip(newShip)
                pStageTime(12) = Now
                'Engine.BuildModuleEffects(newShip)
                pStageTime(13) = Now
                'Call Engine.ApplyStackingPenalties()
                pStageTime(14) = Now
                'Engine.ApplyModuleEffectsToDrones(newShip)
                pStageTime(15) = Now
                'Engine.ApplyModuleEffectsToShip(newShip)
                pStageTime(16) = Now
                'Engine.CalculateDamageStatistics(newShip)
                pStageTime(17) = Now
                'Ship.MapShipAttributes(newShip)
                'Engine.CalculateDefenceStatistics(newShip)
                pStageTime(18) = Now
            Case BuildType.BuildFromEffectsMaps
                pStageTime(0) = Now
                'Engine.BuildSkillEffects(shipPilot)
                pStageTime(1) = Now
                'Engine.BuildImplantEffects(shipPilot)
                pStageTime(2) = Now
                'Engine.BuildShipEffects(shipPilot, baseShip)
                pStageTime(3) = Now
                newShip = Engine.CollectModules(CType(baseShip.Clone, Ship))
                Engine.ApplySkillEffectsToShip(newShip)
                pStageTime(4) = Now
                Engine.ApplySkillEffectsToModules(newShip)
                pStageTime(5) = Now
                Engine.ApplySkillEffectsToDrones(newShip)
                pStageTime(6) = Now
                Engine.BuildChargeEffects(newShip)
                pStageTime(10) = Now
                Engine.ApplyChargeEffectsToModules(newShip)
                pStageTime(11) = Now
                Engine.ApplyChargeEffectsToShip(newShip)
                pStageTime(12) = Now
                Engine.BuildModuleEffects(newShip)
                pStageTime(7) = Now
                Engine.ApplyStackingPenalties()
                pStageTime(8) = Now
                Engine.ApplyModuleEffectsToModules(newShip)
                pStageTime(9) = Now
                Engine.BuildModuleEffects(newShip)
                pStageTime(13) = Now
                Engine.ApplyStackingPenalties()
                pStageTime(14) = Now
                Engine.ApplyModuleEffectsToDrones(newShip)
                pStageTime(15) = Now
                Engine.ApplyModuleEffectsToShip(newShip)
                pStageTime(16) = Now
                Engine.CalculateDamageStatistics(newShip)
                pStageTime(17) = Now
                Ship.MapShipAttributes(newShip)
                Engine.CalculateDefenceStatistics(newShip)
                pStageTime(18) = Now
        End Select
        If Settings.HQFSettings.ShowPerformanceData = True Then
            Dim dTime As TimeSpan
            Dim perfMsg As String = ""
            For stage As Integer = 1 To stages
                perfMsg &= pStages(stage)
                dTime = pStageTime(stage) - pStageTime(stage - 1)
                perfMsg &= FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms" & ControlChars.CrLf
            Next
            dTime = pStageTime(stages) - pStageTime(0)
            perfMsg &= "Total Time: " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms" & ControlChars.CrLf
            MessageBox.Show(perfMsg, "Performance Data Results: Method " & BuildMethod, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Ship.MapShipAttributes(newShip)
        Return newShip
    End Function

    Private Shared Function CollectModules(ByVal newShip As Ship) As Ship
        newShip.SlotCollection.Clear()
        For Each RemoteObject As Object In newShip.RemoteSlotCollection
            If TypeOf RemoteObject Is ShipModule Then
                newShip.SlotCollection.Add(RemoteObject)
            Else
                Dim remoteDrones As DroneBayItem = CType(RemoteObject, DroneBayItem)
                For drone As Integer = 1 To remoteDrones.Quantity
                    newShip.SlotCollection.Add(remoteDrones.DroneType)
                Next
            End If
        Next
        For Each FleetObject As Object In newShip.FleetSlotCollection
            If TypeOf FleetObject Is ShipModule Then
                newShip.SlotCollection.Add(FleetObject)
            End If
        Next
        For slot As Integer = 1 To newShip.HiSlots
            If newShip.HiSlot(slot) IsNot Nothing Then
                newShip.SlotCollection.Add(newShip.HiSlot(slot))
            End If
        Next
        For slot As Integer = 1 To newShip.MidSlots
            If newShip.MidSlot(slot) IsNot Nothing Then
                newShip.SlotCollection.Add(newShip.MidSlot(slot))
            End If
        Next
        For slot As Integer = 1 To newShip.LowSlots
            If newShip.LowSlot(slot) IsNot Nothing Then
                newShip.SlotCollection.Add(newShip.LowSlot(slot))
            End If
        Next
        For slot As Integer = 1 To newShip.RigSlots
            If newShip.RigSlot(slot) IsNot Nothing Then
                newShip.SlotCollection.Add(newShip.RigSlot(slot))
            End If
        Next
        Return newShip
    End Function

    Private Shared Sub ApplyChargeEffectsToModules(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        Dim att As String = ""
        Dim oldAtt As String = ""
        Dim log As String = ""
        Dim processAtt As Boolean = False
        For Each aModule As ShipModule In newShip.SlotCollection
            If aModule.ModuleState < 16 Then
                For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                    att = CStr(aModule.Attributes.GetKey(attNo))
                    If ChargeEffectsTable.Contains(att) = True Then
                        For Each fEffect As FinalEffect In CType(ChargeEffectsTable(att), ArrayList)
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
                                    If fEffect.AffectedID.Contains(aModule.SlotType & aModule.SlotNo) Then
                                        processAtt = True
                                    End If
                                Case EffectType.Attribute
                                    If aModule.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
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
                                    Case EffectCalcType.AddPositive
                                        If fEffect.AffectedValue > 0 Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        End If
                                    Case EffectCalcType.AddNegative
                                        If fEffect.AffectedValue < 0 Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        End If
                                    Case EffectCalcType.Subtraction
                                        aModule.Attributes(att) = CDbl(aModule.Attributes(att)) - fEffect.AffectedValue
                                    Case EffectCalcType.CloakedVelocity
                                        aModule.Attributes(att) = -100 + ((100 + CDbl(aModule.Attributes(att))) * (fEffect.AffectedValue / 100))
                                End Select
                                log &= " --> " & aModule.Attributes(att).ToString
                                If oldAtt <> aModule.Attributes(att).ToString Then
                                    aModule.AuditLog.Add(log)
                                End If
                            End If
                        Next
                    End If
                Next
                ShipModule.MapModuleAttributes(aModule)
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub

    Private Shared Sub ApplyChargeEffectsToShip(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        Dim maxSlots As Integer = 0
        Dim att As String = ""
        Dim oldAtt As String = ""
        Dim log As String = ""
        Dim processAtt As Boolean = False
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = CStr(newShip.Attributes.GetKey(attNo))
            If ChargeEffectsTable.Contains(att) = True Then
                For Each fEffect As FinalEffect In CType(ChargeEffectsTable(att), ArrayList)
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
                        Case EffectType.Attribute
                            If newShip.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
                                processAtt = True
                            End If
                    End Select
                    If processAtt = True Then
                        log &= Attributes.AttributeQuickList(att).ToString & "# " & fEffect.Cause
                        If newShip.Name = fEffect.Cause Then
                            log &= " (Overloading)"
                        End If
                        oldAtt = newShip.Attributes(att).ToString()
                        log &= "# " & oldAtt
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
                            Case EffectCalcType.AddPositive
                                If fEffect.AffectedValue > 0 Then
                                    newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fEffect.AffectedValue
                                End If
                            Case EffectCalcType.AddNegative
                                If fEffect.AffectedValue < 0 Then
                                    newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fEffect.AffectedValue
                                End If
                            Case EffectCalcType.Subtraction
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) - fEffect.AffectedValue
                            Case EffectCalcType.CloakedVelocity
                                newShip.Attributes(att) = -100 + ((100 + CDbl(newShip.Attributes(att))) * (fEffect.AffectedValue / 100))
                        End Select
                        log &= "# " & newShip.Attributes(att).ToString
                        If oldAtt <> newShip.Attributes(att).ToString Then
                            newShip.AuditLog.Add(log)
                        End If
                    End If
                Next
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub

    Private Shared Sub ApplySkillEffectsToShip(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        Dim thisEffect As New FinalEffect
        Dim att As String = ""
        Dim processAtt As Boolean = False
        Dim log As String = ""
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
                        Case EffectType.Attribute
                            If newShip.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
                                processAtt = True
                            End If
                    End Select
                    If processAtt = True Then
                        log &= Attributes.AttributeQuickList(att).ToString & "# " & fEffect.Cause & "# " & newShip.Attributes(att).ToString
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
                            Case EffectCalcType.AddPositive
                                If fEffect.AffectedValue > 0 Then
                                    newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fEffect.AffectedValue
                                End If
                            Case EffectCalcType.AddNegative
                                If fEffect.AffectedValue < 0 Then
                                    newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fEffect.AffectedValue
                                End If
                            Case EffectCalcType.Subtraction
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) - fEffect.AffectedValue
                            Case EffectCalcType.CloakedVelocity
                                newShip.Attributes(att) = -100 + ((100 + CDbl(newShip.Attributes(att))) * (fEffect.AffectedValue / 100))
                        End Select
                        log &= "# " & newShip.Attributes(att).ToString
                        newShip.AuditLog.Add(log)
                    End If
                Next
            End If
        Next
        Ship.MapShipAttributes(newShip)
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub

    Private Shared Sub ApplySkillEffectsToModules(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        Dim att As String = ""
        Dim oldAtt As String = ""
        Dim processAtt As Boolean = False
        Dim log As String = ""
        For Each aModule As ShipModule In newShip.SlotCollection
            If aModule.ModuleState < 16 Then
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
                                    If fEffect.AffectedID.Contains(aModule.SlotType & aModule.SlotNo) Then
                                        processAtt = True
                                    End If
                                Case EffectType.Attribute
                                    If aModule.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
                                        processAtt = True
                                    End If
                            End Select
                            If processAtt = True Then
                                oldAtt = aModule.Attributes(att).ToString
                                log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause & ": " & oldAtt
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
                                    Case EffectCalcType.AddPositive
                                        If fEffect.AffectedValue > 0 Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        End If
                                    Case EffectCalcType.AddNegative
                                        If fEffect.AffectedValue < 0 Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        End If
                                    Case EffectCalcType.Subtraction
                                        aModule.Attributes(att) = CDbl(aModule.Attributes(att)) - fEffect.AffectedValue
                                    Case EffectCalcType.CloakedVelocity
                                        aModule.Attributes(att) = -100 + ((100 + CDbl(aModule.Attributes(att))) * (fEffect.AffectedValue / 100))
                                End Select
                                log &= " --> " & aModule.Attributes(att).ToString
                                If oldAtt <> aModule.Attributes(att).ToString Then
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
                                        If fEffect.AffectedID.Contains(aModule.LoadedCharge.SlotType & aModule.LoadedCharge.SlotNo) Then
                                            processAtt = True
                                        End If
                                    Case EffectType.Attribute
                                        If aModule.LoadedCharge.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
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
                                        Case EffectCalcType.AddPositive
                                            If fEffect.AffectedValue > 0 Then
                                                aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) + fEffect.AffectedValue
                                            End If
                                        Case EffectCalcType.AddNegative
                                            If fEffect.AffectedValue < 0 Then
                                                aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) + fEffect.AffectedValue
                                            End If
                                        Case EffectCalcType.Subtraction
                                            aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) - fEffect.AffectedValue
                                        Case EffectCalcType.CloakedVelocity
                                            aModule.LoadedCharge.Attributes(att) = -100 + ((100 + CDbl(aModule.LoadedCharge.Attributes(att))) * (fEffect.AffectedValue / 100))
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
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime

    End Sub

    Private Shared Sub ApplySkillEffectsToDrones(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        Dim aModule As New ShipModule
        Dim maxSlots As Integer = 0
        Dim att As String = ""
        Dim oldAtt As String = ""
        Dim processAtt As Boolean = False
        Dim log As String = ""
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            aModule = DBI.DroneType
            If aModule.ModuleState < 16 Then
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
                                    If fEffect.AffectedID.Contains(aModule.SlotType & aModule.SlotNo) Then
                                        processAtt = True
                                    End If
                                Case EffectType.Attribute
                                    If aModule.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
                                        processAtt = True
                                    End If
                            End Select
                            If processAtt = True Then
                                oldAtt = aModule.Attributes(att).ToString
                                log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause & ": " & oldAtt
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
                                    Case EffectCalcType.AddPositive
                                        If fEffect.AffectedValue > 0 Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        End If
                                    Case EffectCalcType.AddNegative
                                        If fEffect.AffectedValue < 0 Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                        End If
                                    Case EffectCalcType.Subtraction
                                        aModule.Attributes(att) = CDbl(aModule.Attributes(att)) - fEffect.AffectedValue
                                    Case EffectCalcType.CloakedVelocity
                                        aModule.Attributes(att) = -100 + ((100 + CDbl(aModule.Attributes(att))) * (fEffect.AffectedValue / 100))
                                End Select
                                log &= " --> " & aModule.Attributes(att).ToString
                                If oldAtt <> aModule.Attributes(att).ToString Then
                                    aModule.AuditLog.Add(log)
                                End If
                            End If
                        Next
                    End If
                Next
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime

    End Sub

    Private Shared Sub ApplyModuleEffectsToModules(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        Dim att As String = ""
        Dim oldAtt As String = ""
        Dim log As String = ""
        Dim processAtt As Boolean = False
        For Each aModule As ShipModule In newShip.SlotCollection
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
                                If fEffect.AffectedID.Contains(aModule.SlotType & aModule.SlotNo) Then
                                    processAtt = True
                                End If
                            Case EffectType.Attribute
                                If aModule.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
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
                                Case EffectCalcType.AddPositive
                                    If fEffect.AffectedValue > 0 Then
                                        aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                    End If
                                Case EffectCalcType.AddNegative
                                    If fEffect.AffectedValue < 0 Then
                                        aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                    End If
                                Case EffectCalcType.Subtraction
                                    aModule.Attributes(att) = CDbl(aModule.Attributes(att)) - fEffect.AffectedValue
                                Case EffectCalcType.CloakedVelocity
                                    aModule.Attributes(att) = -100 + ((100 + CDbl(aModule.Attributes(att))) * (fEffect.AffectedValue / 100))
                            End Select
                            log &= " --> " & aModule.Attributes(att).ToString
                            If oldAtt <> aModule.Attributes(att).ToString Then
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
                                    If fEffect.AffectedID.Contains(aModule.LoadedCharge.SlotType & aModule.LoadedCharge.SlotNo) Then
                                        processAtt = True
                                    End If
                                Case EffectType.Attribute
                                    If aModule.LoadedCharge.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
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
                                    Case EffectCalcType.AddPositive
                                        If fEffect.AffectedValue > 0 Then
                                            aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) + fEffect.AffectedValue
                                        End If
                                    Case EffectCalcType.AddNegative
                                        If fEffect.AffectedValue < 0 Then
                                            aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) + fEffect.AffectedValue
                                        End If
                                    Case EffectCalcType.Subtraction
                                        aModule.LoadedCharge.Attributes(att) = CDbl(aModule.LoadedCharge.Attributes(att)) - fEffect.AffectedValue
                                    Case EffectCalcType.CloakedVelocity
                                        aModule.LoadedCharge.Attributes(att) = -100 + ((100 + CDbl(aModule.LoadedCharge.Attributes(att))) * (fEffect.AffectedValue / 100))
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
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub

    Private Shared Sub ApplyModuleEffectsToDrones(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        Dim aModule As New ShipModule
        Dim maxSlots As Integer = 0
        Dim att As String = ""
        Dim oldAtt As String = ""
        Dim log As String = ""
        Dim processAtt As Boolean = False
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            aModule = DBI.DroneType
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
                                If fEffect.AffectedID.Contains(aModule.SlotType & aModule.SlotNo) Then
                                    processAtt = True
                                End If
                            Case EffectType.Attribute
                                If aModule.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
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
                                Case EffectCalcType.AddPositive
                                    If fEffect.AffectedValue > 0 Then
                                        aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                    End If
                                Case EffectCalcType.AddNegative
                                    If fEffect.AffectedValue < 0 Then
                                        aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fEffect.AffectedValue
                                    End If
                                Case EffectCalcType.Subtraction
                                    aModule.Attributes(att) = CDbl(aModule.Attributes(att)) - fEffect.AffectedValue
                                Case EffectCalcType.CloakedVelocity
                                    aModule.Attributes(att) = -100 + ((100 + CDbl(aModule.Attributes(att))) * (fEffect.AffectedValue / 100))
                            End Select
                            log &= " --> " & aModule.Attributes(att).ToString
                            If oldAtt <> aModule.Attributes(att).ToString Then
                                aModule.AuditLog.Add(log)
                            End If
                        End If
                    Next
                End If
            Next
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub

    Private Shared Sub ApplyModuleEffectsToShip(ByRef newShip As Ship)
        Dim sTime, eTime As Date
        sTime = Now
        Dim att As String = ""
        Dim log As String = ""
        Dim oldAtt As String = ""
        Dim processAtt As Boolean = False
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = CStr(newShip.Attributes.GetKey(attNo))
            If ModuleEffectsTable.Contains(att) = True Then
                For Each fEffect As FinalEffect In CType(ModuleEffectsTable(att), ArrayList)
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
                        Case EffectType.Attribute
                            If newShip.Attributes.Contains(CStr(fEffect.AffectedID(0))) Then
                                processAtt = True
                            End If
                    End Select
                    If processAtt = True Then
                        log &= Attributes.AttributeQuickList(att).ToString & "# " & fEffect.Cause
                        oldAtt = newShip.Attributes(att).ToString()
                        log &= "# " & oldAtt
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
                            Case EffectCalcType.AddPositive
                                If fEffect.AffectedValue > 0 Then
                                    newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fEffect.AffectedValue
                                End If
                            Case EffectCalcType.AddNegative
                                If fEffect.AffectedValue < 0 Then
                                    newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fEffect.AffectedValue
                                End If
                            Case EffectCalcType.Subtraction
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) - fEffect.AffectedValue
                            Case EffectCalcType.CloakedVelocity
                                newShip.Attributes(att) = -100 + ((100 + CDbl(newShip.Attributes(att))) * (fEffect.AffectedValue / 100))
                        End Select
                        log &= "# " & newShip.Attributes(att).ToString
                        newShip.AuditLog.Add(log)
                    End If
                Next
            End If
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub

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
    End Sub

    Private Shared Sub CalculateDamageStatistics(ByRef newShip As Ship)
        Dim cModule As New ShipModule
        Dim dgmMod As Double = 1
        Dim ROF As Double = 1
        newShip.Attributes("10006") = 0
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            If DBI.IsActive = True Then
                cModule = DBI.DroneType
                If CInt(cModule.DatabaseGroup) <> 101 Then
                    ' Not mining drone
                    cModule.Attributes("10017") = CDbl(cModule.Attributes("114")) + CDbl(cModule.Attributes("116")) + CDbl(cModule.Attributes("117")) + CDbl(cModule.Attributes("118"))
                    If cModule.Attributes.Contains("51") = True Then
                        ROF = CDbl(cModule.Attributes("51"))
                        dgmMod = CDbl(cModule.Attributes("64"))
                    Else
                        dgmMod = 0
                        ROF = 1
                    End If
                    cModule.Attributes("10051") = CDbl(cModule.Attributes("114")) * dgmMod
                    cModule.Attributes("10052") = CDbl(cModule.Attributes("116")) * dgmMod
                    cModule.Attributes("10053") = CDbl(cModule.Attributes("117")) * dgmMod
                    cModule.Attributes("10054") = CDbl(cModule.Attributes("118")) * dgmMod
                    newShip.Attributes("10006") = CInt(newShip.Attributes("10006")) + DBI.Quantity
                    cModule.Attributes("10018") = dgmMod * CDbl(cModule.Attributes("10017"))
                    cModule.Attributes("10019") = CDbl(cModule.Attributes("10018")) / ROF
                    newShip.Attributes("10023") = CDbl(newShip.Attributes("10023")) + CDbl(cModule.Attributes("10018")) * DBI.Quantity
                    newShip.Attributes("10027") = CDbl(newShip.Attributes("10027")) + CDbl(cModule.Attributes("10019")) * DBI.Quantity
                    newShip.Attributes("10028") = CDbl(newShip.Attributes("10028")) + CDbl(cModule.Attributes("10018")) * DBI.Quantity
                    newShip.Attributes("10029") = CDbl(newShip.Attributes("10029")) + CDbl(cModule.Attributes("10019")) * DBI.Quantity
                    newShip.Attributes("10055") = CDbl(newShip.Attributes("10055")) + CDbl(cModule.Attributes("10051")) * DBI.Quantity
                    newShip.Attributes("10056") = CDbl(newShip.Attributes("10056")) + CDbl(cModule.Attributes("10052")) * DBI.Quantity
                    newShip.Attributes("10057") = CDbl(newShip.Attributes("10057")) + CDbl(cModule.Attributes("10053")) * DBI.Quantity
                    newShip.Attributes("10058") = CDbl(newShip.Attributes("10058")) + CDbl(cModule.Attributes("10054")) * DBI.Quantity
                Else
                    ' Mining drone
                    newShip.Attributes("10006") = CInt(newShip.Attributes("10006")) + DBI.Quantity
                    cModule.Attributes("10040") = CDbl(cModule.Attributes("77")) / CDbl(cModule.Attributes("73"))
                    newShip.Attributes("10033") = CDbl(newShip.Attributes("10033")) + CDbl(cModule.Attributes("77")) * DBI.Quantity
                    newShip.Attributes("10035") = CDbl(newShip.Attributes("10035")) + CDbl(cModule.Attributes("77")) * DBI.Quantity
                    newShip.Attributes("10044") = CDbl(newShip.Attributes("10044")) + CDbl(cModule.Attributes("10040")) * DBI.Quantity
                    newShip.Attributes("10047") = CDbl(newShip.Attributes("10047")) + CDbl(cModule.Attributes("10040")) * DBI.Quantity
                End If
            End If
        Next
        For slot As Integer = 1 To newShip.HiSlots
            cModule = newShip.HiSlot(slot)
            If cModule IsNot Nothing Then
                If (cModule.ModuleState Or 12) = 12 Then
                    Select Case CInt(cModule.MarketGroup)
                        Case 1039, 1040 ' Ore Mining Turret
                            newShip.Attributes("10034") = CDbl(newShip.Attributes("10034")) + CDbl(cModule.Attributes("77"))
                            newShip.Attributes("10033") = CDbl(newShip.Attributes("10033")) + CDbl(cModule.Attributes("77"))
                            cModule.Attributes("10039") = CDbl(cModule.Attributes("77")) / CDbl(cModule.Attributes("73"))
                            newShip.Attributes("10043") = CDbl(newShip.Attributes("10043")) + CDbl(cModule.Attributes("10039"))
                            newShip.Attributes("10047") = CDbl(newShip.Attributes("10047")) + CDbl(cModule.Attributes("10039"))
                        Case 1038 ' Ice Mining Turret
                            newShip.Attributes("10037") = CDbl(newShip.Attributes("10037")) + CDbl(cModule.Attributes("77"))
                            newShip.Attributes("10036") = CDbl(newShip.Attributes("10036")) + CDbl(cModule.Attributes("77"))
                            cModule.Attributes("10041") = CDbl(cModule.Attributes("77")) / CDbl(cModule.Attributes("73"))
                            newShip.Attributes("10045") = CDbl(newShip.Attributes("10045")) + CDbl(cModule.Attributes("10041"))
                            newShip.Attributes("10048") = CDbl(newShip.Attributes("10048")) + CDbl(cModule.Attributes("10041"))
                        Case Else
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
                                        If cModule.LoadedCharge IsNot Nothing Then
                                            cModule.Attributes("54") = CDbl(cModule.LoadedCharge.Attributes("37")) * CDbl(cModule.LoadedCharge.Attributes("281")) * HQF.Settings.HQFSettings.MissileRangeConstant
                                        End If
                                    End If
                                    cModule.Attributes("10051") = CDbl(cModule.LoadedCharge.Attributes("114")) * dgmMod
                                    cModule.Attributes("10052") = CDbl(cModule.LoadedCharge.Attributes("116")) * dgmMod
                                    cModule.Attributes("10053") = CDbl(cModule.LoadedCharge.Attributes("117")) * dgmMod
                                    cModule.Attributes("10054") = CDbl(cModule.LoadedCharge.Attributes("118")) * dgmMod
                                    newShip.Attributes("10055") = CDbl(newShip.Attributes("10055")) + CDbl(cModule.Attributes("10051"))
                                    newShip.Attributes("10056") = CDbl(newShip.Attributes("10056")) + CDbl(cModule.Attributes("10052"))
                                    newShip.Attributes("10057") = CDbl(newShip.Attributes("10057")) + CDbl(cModule.Attributes("10053"))
                                    newShip.Attributes("10058") = CDbl(newShip.Attributes("10058")) + CDbl(cModule.Attributes("10054"))
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
                                    cModule.Attributes("10051") = CDbl(cModule.Attributes("114"))
                                    cModule.Attributes("10052") = CDbl(cModule.Attributes("116"))
                                    cModule.Attributes("10053") = CDbl(cModule.Attributes("117"))
                                    cModule.Attributes("10054") = CDbl(cModule.Attributes("118"))
                                    newShip.Attributes("10055") = CDbl(newShip.Attributes("10055")) + CDbl(cModule.Attributes("10051"))
                                    newShip.Attributes("10056") = CDbl(newShip.Attributes("10056")) + CDbl(cModule.Attributes("10052"))
                                    newShip.Attributes("10057") = CDbl(newShip.Attributes("10057")) + CDbl(cModule.Attributes("10053"))
                                    newShip.Attributes("10058") = CDbl(newShip.Attributes("10058")) + CDbl(cModule.Attributes("10054"))
                                End If
                            End If
                    End Select
                End If
            End If
        Next
    End Sub

    Private Shared Sub CalculateDefenceStatistics(ByRef newShip As Ship)
        Dim sR, aR, hR As Double
        For Each cModule As ShipModule In newShip.SlotCollection
            ' Calculate shield boosting
            If cModule.DatabaseGroup = "40" And (cModule.ModuleState And 12) = cModule.ModuleState Then
                sR = sR + CDbl(cModule.Attributes("68")) / CDbl(cModule.Attributes("73"))
            End If
            ' Calculate remote shield boosting
            If cModule.DatabaseGroup = "41" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                sR = sR + CDbl(cModule.Attributes("68")) / CDbl(cModule.Attributes("73"))
            End If
            ' Calculate shield maintenance drones
            If cModule.DatabaseGroup = "640" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                sR = sR + CDbl(cModule.Attributes("68")) / CDbl(cModule.Attributes("73"))
            End If
            ' Calculate armor repairing
            If cModule.DatabaseGroup = "62" And (cModule.ModuleState And 12) = cModule.ModuleState Then
                aR = aR + CDbl(cModule.Attributes("84")) / CDbl(cModule.Attributes("73"))
            End If
            ' Calculate remote armor repairing
            If cModule.DatabaseGroup = "325" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                aR = aR + CDbl(cModule.Attributes("84")) / CDbl(cModule.Attributes("73"))
            End If
            ' Calculate armor maintenance drones
            If cModule.DatabaseGroup = "640" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                aR = aR + CDbl(cModule.Attributes("84")) / CDbl(cModule.Attributes("73"))
            End If
            ' Calculate hull repairing
            If cModule.DatabaseGroup = "63" And (cModule.ModuleState And 12) = cModule.ModuleState Then
                hR = hR + CDbl(cModule.Attributes("83")) / CDbl(cModule.Attributes("73"))
            End If
            ' Calculate remote hull repairing
            If cModule.DatabaseGroup = "585" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                hR = hR + CDbl(cModule.Attributes("83")) / CDbl(cModule.Attributes("73"))
            End If
        Next
        sR = sR + (newShip.ShieldCapacity / newShip.ShieldRecharge * HQF.Settings.HQFSettings.ShieldRechargeConstant)
        ' Calculate the actual tanking ability
        Dim sT As Double = sR / ((newShip.DamageProfileEM * (1 - newShip.ShieldEMResist / 100)) + (newShip.DamageProfileEX * (1 - newShip.ShieldExResist / 100)) + (newShip.DamageProfileKI * (1 - newShip.ShieldKiResist / 100)) + (newShip.DamageProfileTH * (1 - newShip.ShieldThResist / 100)))
        Dim aT As Double = aR / ((newShip.DamageProfileEM * (1 - newShip.ArmorEMResist / 100)) + (newShip.DamageProfileEX * (1 - newShip.ArmorExResist / 100)) + (newShip.DamageProfileKI * (1 - newShip.ArmorKiResist / 100)) + (newShip.DamageProfileTH * (1 - newShip.ArmorThResist / 100)))
        Dim hT As Double = hR / ((newShip.DamageProfileEM * (1 - newShip.StructureEMResist / 100)) + (newShip.DamageProfileEX * (1 - newShip.StructureExResist / 100)) + (newShip.DamageProfileKI * (1 - newShip.StructureKiResist / 100)) + (newShip.DamageProfileTH * (1 - newShip.StructureThResist / 100)))
        newShip.Attributes("10059") = sT
        newShip.Attributes("10060") = aT
        newShip.Attributes("10061") = hT
        newShip.Attributes("10062") = Math.Max(sT, Math.Max(aT, hT))
    End Sub

#End Region

#Region "Cloning"
    Public Shared Function CloneSortedList(ByVal oldSortedList As SortedList) As SortedList
        Dim myMemoryStream As New MemoryStream()
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(myMemoryStream, oldSortedList)
        myMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newSortedList As SortedList = CType(objBinaryFormatter.Deserialize(myMemoryStream), SortedList)
        myMemoryStream.Close()
        Return newSortedList
    End Function
#End Region

#Region "Supplemental Routines"
    Public Shared Function ApplySkillEffectsToModule(ByVal baseModule As ShipModule) As ShipModule
        Dim sTime, eTime As Date
        sTime = Now
        'Dim maxSlots As Integer = 0
        Dim att As String = ""
        'Dim oldAtt As String = ""
        ' Define a new module
        Dim aModule As ShipModule = CType(baseModule.Clone, ShipModule)
        Dim processAtt As Boolean = False
        'Dim log As String = ""
        If aModule IsNot Nothing Then
            For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                att = CStr(aModule.Attributes.GetKey(attNo))
                If SkillEffectsTable.Contains(att) = True Then
                    For Each fEffect As FinalEffect In CType(SkillEffectsTable(att), ArrayList)
                        processAtt = False
                        'log = ""
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
                            'oldAtt = aModule.Attributes(att).ToString
                            'log &= Attributes.AttributeQuickList(att).ToString & ": " & fEffect.Cause & ": " & oldAtt
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
                            'log &= " --> " & aModule.Attributes(att).ToString
                            'If oldAtt <> aModule.Attributes(att).ToString Then
                            'aModule.AuditLog.Add(log)
                            'End If
                        End If
                    Next
                End If
            Next
            ShipModule.MapModuleAttributes(aModule)
        End If
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Skill Effects to Modules took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return aModule
    End Function
    Public Shared Function IsUsable(ByVal hPilot As HQFPilot, ByVal shipMod As ShipModule) As Boolean
        Dim usable As Boolean = True
        Dim rSkill As HQFSkill
        For Each reqSkill As ItemSkills In shipMod.RequiredSkills.Values
            If hPilot.SkillSet.Contains(reqSkill.Name) = True Then
                rSkill = CType(hPilot.SkillSet.Item(reqSkill.Name), HQFSkill)
                If rSkill.Level < reqSkill.Level Then
                    usable = False
                    Exit For
                End If
            Else
                usable = False
                Exit For
            End If
        Next
        Return usable
    End Function
    Public Shared Function IsFittable(ByVal cMod As ShipModule, ByVal cShip As Ship) As Boolean
        If cMod.CPU <= cShip.CPU - cShip.CPU_Used Then
            If cMod.PG <= cShip.PG - cShip.PG_Used Then
                If cMod.Calibration <= cShip.Calibration - cShip.Calibration_Used Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function
    Public Shared Function CalculateCapStatistics(ByVal baseShip As Ship) As Double
        Dim CapacitorCapacity As Double = baseShip.CapCapacity
        Dim Capacitor As Double = CapacitorCapacity
        Dim currentTime, nextTime As Double
        Dim RechargeRate As Double = baseShip.CapRecharge
        Dim capConstant As Double = (RechargeRate / 5.0)
        Dim maxTime As Double = 3600 ' an hour

        ' Populate the module list
        Dim modCount As Integer = 0
        Dim shipMods(baseShip.SlotCollection.Count, 2) As Double
        For Each slotMod As ShipModule In baseShip.SlotCollection
            If slotMod.CapUsage <> 0 And (slotMod.ModuleState Or 28) = 28 Then
                shipMods(modCount, 0) = slotMod.CapUsage
                shipMods(modCount, 1) = slotMod.ActivationTime + CDbl(slotMod.Attributes("10011")) + CDbl(slotMod.Attributes("10012"))
                modCount += 1
            End If
        Next

        ' Do the calculations
        While ((Capacitor > 0.0) And (nextTime < maxTime))
            Capacitor = (((1.0 + ((Math.Sqrt((Capacitor / CapacitorCapacity)) - 1.0) * Math.Exp(((currentTime - nextTime) / capConstant)))) ^ 2) * CapacitorCapacity)
            currentTime = nextTime
            nextTime = maxTime
            For sm As Integer = 0 To modCount - 1
                If (shipMods(sm, 2) = currentTime) Then
                    shipMods(sm, 2) += shipMods(sm, 1)
                    Capacitor -= shipMods(sm, 0)
                End If
                nextTime = Math.Min(nextTime, shipMods(sm, 2))
            Next
        End While

        ' Return the result
        If Capacitor > 0 Then
            Return Capacitor
        Else
            Return -currentTime
        End If
    End Function
#End Region

#Region "Fitting Routines"
    Public Shared Function UpdateShipDataFromFittingList(ByVal currentship As Ship, ByVal currentFit As ArrayList) As Ship
        Dim currentFitList As ArrayList = CType(currentFit.Clone, ArrayList)
        For Each shipMod As String In currentFitList
            If shipMod IsNot Nothing Then
                ' Check for installed charges
                Dim modData() As String = shipMod.Split(",".ToCharArray)
                ' Remove the activity flag
                If modData(0).Substring(modData(0).Length - 2, 1) = "_" Then
                    modData(0) = modData(0).Substring(0, modData(0).Length - 2)
                End If
                If ModuleLists.moduleListName.ContainsKey(modData(0)) = True Then
                    Dim modID As String = ModuleLists.moduleListName(modData(0).Trim).ToString
                    Dim sMod As ShipModule = CType(ModuleLists.moduleList(modID), ShipModule).Clone
                    If modData.GetUpperBound(0) > 0 Then
                        ' Check if a charge (will be a valid item)
                        If ModuleLists.moduleListName.Contains(modData(1).Trim) = True Then
                            Dim chgID As String = ModuleLists.moduleListName(modData(1).Trim).ToString
                            sMod.LoadedCharge = CType(ModuleLists.moduleList(chgID), ShipModule).Clone
                        End If
                    End If
                    ' Check if module is nothing
                    If sMod IsNot Nothing Then
                        ' Check if module is a drone
                        If sMod.IsDrone = True Then
                            Dim active As Boolean = False
                            If modData(1).EndsWith("a") = True Then
                                active = True
                            End If
                            Call Engine.AddDrone(currentship, sMod, CInt(modData(1).Substring(0, Len(modData(1)) - 1)), active)
                        Else
                            ' Check if module is a charge
                            If sMod.IsCharge = True Then
                                Call Engine.AddItem(currentship, sMod, CInt(modData(1)))
                            Else
                                ' Must be a proper module then!
                                Call Engine.AddModule(currentship, sMod)
                            End If
                        End If
                    Else
                        ' Unrecognised module
                        MessageBox.Show("Ship Module is unrecognised.", "Add Ship Module Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    currentFit.Remove(modData(0))
                End If
            End If
        Next
        Return currentship
    End Function
    Public Shared Sub AddModule(ByVal currentship As Ship, ByVal shipMod As ShipModule)
        ' Check slot availability
        If IsSlotAvailable(currentship, shipMod) = True Then
            ' Add Module to the next slot
            Dim slotNo As Integer = AddModuleInNextSlot(currentship, CType(shipMod.Clone, ShipModule))
        End If
    End Sub
    Public Shared Sub AddDrone(ByVal currentShip As Ship, ByVal Drone As ShipModule, ByVal Qty As Integer, ByVal Active As Boolean)
        ' Set grouping flag
        Dim grouped As Boolean = False
        ' See if there is sufficient space
        Dim vol As Double = Drone.Volume
        If currentShip.DroneBay - currentShip.DroneBay_Used >= vol Then
            ' Scan through existing items and see if we can group this new one
            For Each droneGroup As DroneBayItem In currentShip.DroneBayItems.Values
                If Drone.Name = droneGroup.DroneType.Name And Active = droneGroup.IsActive Then
                    ' Add to existing drone group
                    droneGroup.Quantity += Qty
                    grouped = True
                    Exit For
                End If
            Next
            ' Put the drone into the drone bay if not grouped
            If grouped = False Then
                Dim DBI As New DroneBayItem
                DBI.DroneType = Drone
                DBI.Quantity = Qty
                DBI.IsActive = Active
                currentShip.DroneBayItems.Add(currentShip.DroneBayItems.Count, DBI)
            End If
        Else
            MessageBox.Show("There is not enough space in the Drone Bay to hold 1 unit of " & Drone.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Public Shared Sub AddItem(ByVal currentShip As Ship, ByVal Item As ShipModule, ByVal Qty As Integer)
        If currentShip IsNot Nothing Then
            ' Set grouping flag
            Dim grouped As Boolean = False
            ' See if there is sufficient space
            Dim vol As Double = Item.Volume
            If currentShip.CargoBay - currentShip.CargoBay_Used >= vol Then
                ' Scan through existing items and see if we can group this new one
                For Each itemGroup As CargoBayItem In currentShip.CargoBayItems.Values
                    If Item.Name = itemGroup.ItemType.Name Then
                        ' Add to existing drone group
                        itemGroup.Quantity += Qty
                        grouped = True
                        Exit For
                    End If
                Next
                ' Put the item into the cargo bay if not grouped
                If grouped = False Then
                    Dim CBI As New CargoBayItem
                    CBI.ItemType = Item
                    CBI.Quantity = Qty
                    currentShip.CargoBayItems.Add(currentShip.CargoBayItems.Count, CBI)
                End If
            Else
                MessageBox.Show("There is not enough space in the Cargo Bay to hold 1 unit of " & Item.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Shared Function IsSlotAvailable(ByVal currentship As Ship, ByVal shipMod As ShipModule) As Boolean

        ' First, check slot layout
        Select Case shipMod.SlotType
            Case 1 ' Rig
                If currentship.RigSlots_Used = currentship.RigSlots Then
                    MessageBox.Show("There are no available rig slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 2 ' Low
                If currentship.LowSlots_Used = currentship.LowSlots Then
                    MessageBox.Show("There are no available low slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 4 ' Mid
                If currentship.MidSlots_Used = currentship.MidSlots Then
                    MessageBox.Show("There are no available mid slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 8 ' High
                If currentship.HiSlots_Used = currentship.HiSlots Then
                    MessageBox.Show("There are no available high slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
        End Select

        ' Now check launcher slots
        If shipMod.IsLauncher Then
            If currentship.LauncherSlots_Used = currentship.LauncherSlots Then
                MessageBox.Show("There are no available launcher slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        ' Now check turret slots
        If shipMod.IsTurret Then
            If currentship.TurretSlots_Used = currentship.TurretSlots Then
                MessageBox.Show("There are no available turret slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        Return True
    End Function
    Private Shared Function AddModuleInNextSlot(ByVal currentShip As Ship, ByVal shipMod As ShipModule) As Integer
        Select Case shipMod.SlotType
            Case 1 ' Rig
                For slotNo As Integer = 1 To 8
                    If currentShip.RigSlot(slotNo) Is Nothing Then
                        currentShip.RigSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available rig slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 2 ' Low
                For slotNo As Integer = 1 To 8
                    If currentShip.LowSlot(slotNo) Is Nothing Then
                        currentShip.LowSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available low slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 4 ' Mid
                For slotNo As Integer = 1 To 8
                    If currentShip.MidSlot(slotNo) Is Nothing Then
                        currentShip.MidSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available mid slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 8 ' High
                For slotNo As Integer = 1 To 8
                    If currentShip.HiSlot(slotNo) Is Nothing Then
                        currentShip.HiSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available high slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
        Return 0
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
    Attribute = 7
End Enum

Public Enum EffectCalcType
    Percentage = 0 ' Simply percentage variation (+/-)
    Addition = 1 ' For adding values
    Difference = 2 ' For resistances
    Velocity = 3 ' For AB/MWD calculations
    Absolute = 4 ' For setting values
    Multiplier = 5 ' Damage multiplier
    AddPositive = 6 ' Adding positive values only
    AddNegative = 7 ' Adding negative values only
    Subtraction = 8 ' Subtracting positive values
    CloakedVelocity = 9 ' Bonus for dealing with cloaked velocity
End Enum

Public Enum BuildType
    BuildEverything = 0
    BuildEffectsMaps = 1
    BuildFromEffectsMaps = 2
End Enum