'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Globalization
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

Public Class Engine

    Public Shared EffectsMap As New SortedList(Of Integer, List(Of Effect))
    Public Shared ShipEffectsMap As New SortedList(Of Integer, List(Of Effect))
    Public Shared ShipBonusesMap As New SortedList(Of Integer, List(Of ShipEffect)) ' ShipID, List(Of ShipEffect)
    Public Shared SubSystemEffectsMap As New SortedList(Of Integer, List(Of ShipEffect)) ' TypeID, List(Of ShipEffect)
    Public Shared FleetEffectsMap As New SortedList
    Public Shared ImplantEffectsMap As New SortedList
    Public Shared PirateImplants As New Dictionary(Of String, String)
    Public Shared PirateImplantGroups As New Dictionary(Of String, Double)

    Shared ReadOnly Culture As CultureInfo = New CultureInfo("en-GB")

#Region "Fitting Routines"

    Public Shared Sub BuildPirateImplants()
        Dim pirateImplantComponents As New ArrayList
        PirateImplantGroups.Clear()
        PirateImplantGroups.Add("High-grade Ascendancy", 1)
        PirateImplantGroups.Add("Mid-grade Ascendancy", 1)
        PirateImplantGroups.Add("Mid-grade Centurion", 1)
        PirateImplantGroups.Add("Low-grade Centurion", 1)
        PirateImplantGroups.Add("High-grade Crystal", 1)
        PirateImplantGroups.Add("Mid-grade Crystal", 1)
        PirateImplantGroups.Add("Low-grade Crystal", 1)
        PirateImplantGroups.Add("Mid-grade Edge", 1)
        PirateImplantGroups.Add("Low-grade Edge", 1)
        PirateImplantGroups.Add("High-grade Grail", 1)
        PirateImplantGroups.Add("Low-grade Grail", 1)
        PirateImplantGroups.Add("High-grade Halo", 1)
        PirateImplantGroups.Add("Mid-grade Halo", 1)
        PirateImplantGroups.Add("Low-grade Halo", 1)
        PirateImplantGroups.Add("Mid-grade Harvest", 1)
        PirateImplantGroups.Add("Low-grade Harvest", 1)
        PirateImplantGroups.Add("High-grade Jackal", 1)
        PirateImplantGroups.Add("Low-grade Jackal", 1)
        PirateImplantGroups.Add("Mid-grade Nomad", 1)
        PirateImplantGroups.Add("Low-grade Nomad", 1)
        PirateImplantGroups.Add("High-grade Slave", 1)
        PirateImplantGroups.Add("Mid-grade Slave", 1)
        PirateImplantGroups.Add("Low-grade Slave", 1)
        PirateImplantGroups.Add("High-grade Snake", 1)
        PirateImplantGroups.Add("Mid-grade Snake", 1)
        PirateImplantGroups.Add("Low-grade Snake", 1)
        PirateImplantGroups.Add("High-grade Spur", 1)
        PirateImplantGroups.Add("Low-grade Spur", 1)
        PirateImplantGroups.Add("High-grade Talisman", 1)
        PirateImplantGroups.Add("Mid-grade Talisman", 1)
        PirateImplantGroups.Add("Low-grade Talisman", 1)
        PirateImplantGroups.Add("High-grade Talon", 1)
        PirateImplantGroups.Add("Low-grade Talon", 1)
        PirateImplantGroups.Add("Mid-grade Virtue", 1)
        PirateImplantGroups.Add("Low-grade Virtue", 1)
        pirateImplantComponents.Clear()
        pirateImplantComponents.Add(" Alpha")
        pirateImplantComponents.Add(" Beta")
        pirateImplantComponents.Add(" Delta")
        pirateImplantComponents.Add(" Epsilon")
        pirateImplantComponents.Add(" Gamma")
        pirateImplantComponents.Add(" Omega")
        PirateImplants.Clear()
        For Each group As String In PirateImplantGroups.Keys
            For Each component As String In pirateImplantComponents
                PirateImplants.Add(group & component, group)
            Next
        Next
        ' Add other random implants
        PirateImplantGroups.Add("Genolution Core Augmentation", 1)
        PirateImplants.Add("Genolution Core Augmentation CA-1", "Genolution Core Augmentation")
        PirateImplants.Add("Genolution Core Augmentation CA-2", "Genolution Core Augmentation")
        PirateImplants.Add("Genolution Core Augmentation CA-3", "Genolution Core Augmentation")
        PirateImplants.Add("Genolution Core Augmentation CA-4", "Genolution Core Augmentation")
    End Sub
    Public Shared Sub BuildBoosterPenaltyList()
        ' Fetch the Effects list
        Dim effectFile As String = My.Resources.BoosterEffects.ToString
        ' Break the Effects down into separate lines
        Dim effectLines() As String = effectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim effectData() As String
        ' Build the map
        Boosters.BoosterEffects.Clear()
        Dim effectList As SortedList(Of String, BoosterEffect)
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#") = False Then
                effectData = effectLine.Split(",".ToCharArray)
                If Boosters.BoosterEffects.ContainsKey(effectData(0)) = True Then
                    ' Get the current effects
                    If CInt(effectData(1)) = 1 Then
                        effectList = Boosters.BoosterEffects(effectData(0))
                        ' Add the penalty to the list
                        Dim newEffect As New BoosterEffect
                        newEffect.AttributeID = effectData(2)
                        newEffect.AttributeEffect = effectData(5)
                        effectList.Add(newEffect.AttributeID, newEffect)
                    End If
                Else
                    ' Start a new set of effects
                    If CInt(effectData(1)) = 1 Then
                        effectList = New SortedList(Of String, BoosterEffect)
                        ' Add the penalty to the list
                        Dim newEffect As New BoosterEffect
                        newEffect.AttributeID = effectData(2)
                        newEffect.AttributeEffect = effectData(5)
                        effectList.Add(newEffect.AttributeID, newEffect)
                        Boosters.BoosterEffects.Add(effectData(0), effectList)
                    End If
                End If
            End If
        Next
    End Sub
    Public Shared Sub BuildEffectsMap()
        ' Break the Effects down into separate lines
        Dim effectLines As List(Of String) = My.Resources.Effects.ToString.Split(ControlChars.CrLf.ToCharArray).ToList
        ' Go through lines and break each one down
        Dim effectData As New List(Of String)
        ' Build the map
        EffectsMap.Clear()
        Dim effectClassList As List(Of Effect)
        Dim newEffect As Effect
        Dim ids As List(Of String)
        Dim affectingIDs As List(Of String)
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#") = False Then
                effectData = effectLine.Split(",".ToCharArray).ToList
                affectingIDs = effectData(2).Split(";".ToCharArray).ToList()

                For Each affectingID As String In affectingIDs

                    newEffect = New Effect
                    If EffectsMap.ContainsKey(CInt((effectData(0)))) = True Then
                        effectClassList = EffectsMap(CInt(effectData(0)))
                    Else
                        effectClassList = New List(Of Effect)
                        EffectsMap.Add(CInt(effectData(0)), effectClassList)
                    End If
                    newEffect.AffectingAtt = CInt(effectData(0))
                    newEffect.AffectingType = CType(effectData(1), HQFEffectType)
                    newEffect.AffectingID = CInt(affectingID)
                    newEffect.AffectedAtt = CInt(effectData(3))
                    newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                    If effectData(5).Contains(";") = True Then
                        ids = effectData(5).Split(";".ToCharArray).ToList
                        For Each id As String In ids
                            newEffect.AffectedID.Add(CInt(id))
                        Next
                    Else
                        newEffect.AffectedID.Add(CInt(effectData(5)))
                    End If
                    newEffect.StackNerf = CType(effectData(6), EffectStackType)
                    newEffect.IsPerLevel = CBool(effectData(7))
                    newEffect.CalcType = CType(effectData(8), EffectCalcType)
                    newEffect.Status = CInt(effectData(9))
                    effectClassList.Add(newEffect)

                Next

            End If
        Next
        effectLines.Clear()
        effectData.Clear()
    End Sub
    Public Shared Sub BuildShipEffectsMap()
        ' Fetch the Effects list
        Dim effectFile As String = My.Resources.ShipEffects.ToString
        ' Break the Effects down into separate lines
        Dim effectLines() As String = effectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim effectData() As String
        ' Build the map
        ShipEffectsMap.Clear()
        Dim effectClassList As List(Of Effect)
        Dim newEffect As Effect
        Dim ids() As String
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#") = False Then
                effectData = effectLine.Split(",".ToCharArray)
                newEffect = New Effect
                If ShipEffectsMap.ContainsKey(CInt((effectData(0)))) = True Then
                    effectClassList = ShipEffectsMap(CInt(effectData(0)))
                Else
                    effectClassList = New List(Of Effect)
                    ShipEffectsMap.Add(CInt(effectData(0)), effectClassList)
                End If
                newEffect.AffectingAtt = CInt(effectData(0))
                newEffect.AffectingType = CType(effectData(1), HQFEffectType)
                newEffect.AffectingID = CInt(effectData(2))
                newEffect.AffectedAtt = CInt(effectData(3))
                newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                If effectData(5).Contains(";") = True Then
                    ids = effectData(5).Split(";".ToCharArray)
                    For Each id As String In ids
                        newEffect.AffectedID.Add(CInt(id))
                    Next
                Else
                    newEffect.AffectedID.Add(CInt(effectData(5)))
                End If
                newEffect.StackNerf = CType(effectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(effectData(7))
                newEffect.CalcType = CType(effectData(8), EffectCalcType)
                newEffect.Status = CInt(effectData(9))
                effectClassList.Add(newEffect)
            End If
        Next
    End Sub
    Public Shared Sub BuildShipBonusesMap()
        ' Fetch the Effects list
        Dim effectFile As String = My.Resources.ShipBonuses.ToString
        ' Break the Effects down into separate lines
        Dim effectLines() As String = effectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim effectData() As String
        ' Build the map
        ShipBonusesMap.Clear()
        Dim shipEffectClassList As List(Of ShipEffect)
        Dim newEffect As ShipEffect
        Dim ids() As String
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#") = False Then
                effectData = effectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                If ShipBonusesMap.ContainsKey(CInt(effectData(0))) = True Then
                    shipEffectClassList = ShipBonusesMap(CInt(effectData(0)))
                Else
                    shipEffectClassList = New List(Of ShipEffect)
                    ShipBonusesMap.Add(CInt(effectData(0)), shipEffectClassList)
                End If
                newEffect.ShipID = CInt(effectData(0))
                newEffect.AffectingType = CType(effectData(1), HQFEffectType)
                newEffect.AffectingID = CInt(effectData(2))
                newEffect.AffectedAtt = CInt(effectData(3))
                newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                If effectData(5).Contains(";") = True Then
                    ids = effectData(5).Split(";".ToCharArray)
                    For Each id As String In ids
                        newEffect.AffectedID.Add(CInt(id))
                    Next
                Else
                    newEffect.AffectedID.Add(CInt(effectData(5)))
                End If
                newEffect.StackNerf = CType(effectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(effectData(7))
                newEffect.CalcType = CType(effectData(8), EffectCalcType)
                newEffect.Value = Double.Parse(effectData(9), NumberStyles.Any, culture)
                newEffect.Status = CInt(effectData(10))
                shipEffectClassList.Add(newEffect)
            End If
        Next
    End Sub
    Public Shared Sub BuildSubSystemBonusMap()
        ' Fetch the Effects list
        Dim effectFile As String = My.Resources.Subsystems.ToString
        ' Break the Effects down into separate lines
        Dim effectLines() As String = effectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim effectData() As String
        ' Build the map
        SubSystemEffectsMap.Clear()
        Dim shipEffectClassList As List(Of ShipEffect)
        Dim newEffect As ShipEffect
        Dim ids() As String
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#") = False Then
                effectData = effectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                If SubSystemEffectsMap.ContainsKey(CInt(effectData(0))) = True Then
                    shipEffectClassList = SubSystemEffectsMap(CInt(effectData(0)))
                Else
                    shipEffectClassList = New List(Of ShipEffect)
                    SubSystemEffectsMap.Add(CInt(effectData(0)), shipEffectClassList)
                End If
                newEffect.ShipID = CInt(effectData(0))
                newEffect.AffectingType = CType(effectData(1), HQFEffectType)
                newEffect.AffectingID = CInt(effectData(2))
                newEffect.AffectedAtt = CInt(effectData(3))
                newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                If effectData(5).Contains(";") = True Then
                    ids = effectData(5).Split(";".ToCharArray)
                    For Each id As String In ids
                        newEffect.AffectedID.Add(CInt(id))
                    Next
                Else
                    newEffect.AffectedID.Add(CInt(effectData(5)))
                End If
                newEffect.StackNerf = CType(effectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(effectData(7))
                newEffect.CalcType = CType(effectData(8), EffectCalcType)
                newEffect.Value = Double.Parse(effectData(9), NumberStyles.Any, culture)
                newEffect.Status = CInt(effectData(10))
                shipEffectClassList.Add(newEffect)
            End If
        Next
    End Sub
    Public Shared Sub BuildImplantEffectsMap()
        ' Fetch the Effects list
        Dim effectFile As String = My.Resources.ImplantEffects.ToString
        ' Break the Effects down into separate lines
        Dim effectLines() As String = effectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim effectData() As String
        ' Build the map
        ImplantEffectsMap.Clear()
        Dim implantEffectClassList As ArrayList
        Dim newEffect As ImplantEffect
        Dim ids() As String
        Dim attIDs() As String
        Dim atts As New ArrayList
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#") = False Then
                effectData = effectLine.Split(",".ToCharArray)
                atts.Clear()
                If effectData(3).Contains(";") Then
                    attIDs = effectData(3).Split(";".ToCharArray)
                    For Each attID As String In attIDs
                        atts.Add(attID)
                    Next
                Else
                    atts.Add(effectData(3))
                End If
                For Each att As String In atts
                    newEffect = New ImplantEffect
                    If ImplantEffectsMap.Contains((effectData(10))) = True Then
                        implantEffectClassList = CType(ImplantEffectsMap(effectData(10)), ArrayList)
                    Else
                        implantEffectClassList = New ArrayList
                        ImplantEffectsMap.Add(effectData(10), implantEffectClassList)
                    End If
                    newEffect.ImplantName = CStr(effectData(10))
                    newEffect.AffectingAtt = CInt(effectData(0))
                    newEffect.AffectedAtt = CInt(att)
                    newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                    If effectData(5).Contains(";") = True Then
                        ids = effectData(5).Split(";".ToCharArray)
                        For Each id As String In ids
                            newEffect.AffectedID.Add(CInt(id))
                        Next
                    Else
                        newEffect.AffectedID.Add(CInt(effectData(5)))
                    End If
                    newEffect.CalcType = CType(effectData(6), EffectCalcType)
                    Dim cImplant As ShipModule
                    ' Fix for EVEHQ-520. Check to see if the implant exists still before trying to get its attributes.
                    ' Implants are known to be renamed in updates.
                    If Implants.ImplantList.TryGetValue(newEffect.ImplantName, cImplant) Then
                        newEffect.Value = CDbl(cImplant.Attributes(CInt(effectData(0))))
                        newEffect.IsGang = CBool(effectData(8))
                        If effectData(9).Contains(";") = True Then
                            ids = effectData(9).Split(";".ToCharArray)
                            For Each id As String In ids
                                newEffect.Groups.Add(id)
                            Next
                        Else
                            newEffect.Groups.Add(effectData(9))
                        End If
                        implantEffectClassList.Add(newEffect)
                    End If
                Next
            End If
        Next
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

    Public Shared Function IsUsable(ByVal hPilot As FittingPilot, ByVal shipMod As ShipModule) As Boolean
        Dim usable As Boolean = True
        Dim rSkill As FittingSkill
        For Each reqSkill As ItemSkills In shipMod.RequiredSkills.Values
            If hPilot.SkillSet.ContainsKey(reqSkill.Name) = True Then
                rSkill = hPilot.SkillSet.Item(reqSkill.Name)
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
    Public Shared Function IsFlyable(ByVal hPilot As FittingPilot, ByVal testShip As Ship) As Boolean
        Dim usable As Boolean = True
        Dim rSkill As FittingSkill
        For Each reqSkill As ItemSkills In testShip.RequiredSkills.Values
            If hPilot.SkillSet.ContainsKey(reqSkill.Name) = True Then
                rSkill = hPilot.SkillSet.Item(reqSkill.Name)
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
        If cMod.CPU <= cShip.CPU - cShip.CpuUsed Then
            If cMod.PG <= cShip.PG - cShip.PgUsed Then
                If cMod.Calibration <= cShip.Calibration - cShip.CalibrationUsed Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

#End Region

End Class

Public Enum BuildType As Integer
    BuildEverything = 0
    BuildEffectsMaps = 1
    BuildFromEffectsMaps = 2
End Enum



