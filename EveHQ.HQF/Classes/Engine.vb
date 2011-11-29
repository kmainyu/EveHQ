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
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

Public Class Engine

    Public Shared EffectsMap As New SortedList
    Public Shared ShipEffectsMap As New SortedList
    Public Shared ShipBonusesMap As New SortedList(Of String, List(Of ShipEffect))
    Public Shared SubSystemEffectsMap As New SortedList(Of String, List(Of ShipEffect))
    Public Shared FleetEffectsMap As New SortedList
    Public Shared ImplantEffectsMap As New SortedList
    Public Shared PirateImplants As New SortedList(Of String, String)
    Public Shared PirateImplantGroups As New SortedList

    Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

#Region "Fitting Routines"

    Public Shared Sub BuildPirateImplants()
        Dim PirateImplantComponents As New ArrayList
        PirateImplantGroups.Clear()
        PirateImplantGroups.Add("Centurion", 1)
        PirateImplantGroups.Add("Crystal", 1)
        PirateImplantGroups.Add("Edge", 1)
        PirateImplantGroups.Add("Grail", 1)
        PirateImplantGroups.Add("Halo", 1)
        PirateImplantGroups.Add("Harvest", 1)
        PirateImplantGroups.Add("Jackal", 1)
        PirateImplantGroups.Add("Nomad", 1)
        PirateImplantGroups.Add("Slave", 1)
        PirateImplantGroups.Add("Snake", 1)
        PirateImplantGroups.Add("Spur", 1)
        PirateImplantGroups.Add("Talisman", 1)
        PirateImplantGroups.Add("Talon", 1)
        PirateImplantGroups.Add("Virtue", 1)
        PirateImplantGroups.Add("Low-grade Centurion", 1)
        PirateImplantGroups.Add("Low-grade Crystal", 1)
        PirateImplantGroups.Add("Low-grade Edge", 1)
        PirateImplantGroups.Add("Low-grade Grail", 1)
        PirateImplantGroups.Add("Low-grade Halo", 1)
        PirateImplantGroups.Add("Low-grade Harvest", 1)
        PirateImplantGroups.Add("Low-grade Jackal", 1)
        PirateImplantGroups.Add("Low-grade Nomad", 1)
        PirateImplantGroups.Add("Low-grade Slave", 1)
        PirateImplantGroups.Add("Low-grade Snake", 1)
        PirateImplantGroups.Add("Low-grade Spur", 1)
        PirateImplantGroups.Add("Low-grade Talisman", 1)
        PirateImplantGroups.Add("Low-grade Talon", 1)
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
    Public Shared Sub BuildBoosterPenaltyList()
        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.BoosterEffects.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String
        ' Build the map
        Boosters.BoosterEffects.Clear()
        Dim EffectList As New SortedList(Of String, BoosterEffect)
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                If Boosters.BoosterEffects.ContainsKey(EffectData(0)) = True Then
                    ' Get the current effects
                    If CInt(EffectData(1)) = 1 Then
                        EffectList = CType(Boosters.BoosterEffects(EffectData(0)), SortedList(Of String, EveHQ.HQF.BoosterEffect))
                        ' Add the penalty to the list
                        Dim newEffect As New BoosterEffect
                        newEffect.AttributeID = EffectData(2)
                        newEffect.AttributeEffect = EffectData(5)
                        EffectList.Add(newEffect.AttributeID, newEffect)
                    End If
                Else
                    ' Start a new set of effects
                    If CInt(EffectData(1)) = 1 Then
                        EffectList = New SortedList(Of String, BoosterEffect)
                        ' Add the penalty to the list
                        Dim newEffect As New BoosterEffect
                        newEffect.AttributeID = EffectData(2)
                        newEffect.AttributeEffect = EffectData(5)
                        EffectList.Add(newEffect.AttributeID, newEffect)
                        Boosters.BoosterEffects.Add(EffectData(0), EffectList)
                    End If
                End If
            End If
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
                newEffect.AffectingType = CType(EffectData(1), EffectType)
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CType(EffectData(4), EffectType)
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CType(EffectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CType(EffectData(8), EffectCalcType)
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
        Dim EffectClassList As New ArrayList
        Dim newEffect As New Effect
        Dim IDs() As String
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                newEffect = New Effect
                If ShipEffectsMap.Contains((EffectData(0))) = True Then
                    EffectClassList = CType(ShipEffectsMap(EffectData(0)), ArrayList)
                Else
                    EffectClassList = New ArrayList
                    ShipEffectsMap.Add(EffectData(0), EffectClassList)
                End If
                newEffect.AffectingAtt = CInt(EffectData(0))
                newEffect.AffectingType = CType(EffectData(1), EffectType)
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CType(EffectData(4), EffectType)
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CType(EffectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CType(EffectData(8), EffectCalcType)
                newEffect.Status = CInt(EffectData(9))
                EffectClassList.Add(newEffect)
            End If
        Next
    End Sub
    Public Shared Sub BuildShipBonusesMap()
        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.ShipBonuses.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String
        ' Build the map
        ShipBonusesMap.Clear()
        Dim shipEffectClassList As New List(Of ShipEffect)
        Dim newEffect As New ShipEffect
        Dim IDs() As String
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                If ShipBonusesMap.ContainsKey((EffectData(0))) = True Then
                    shipEffectClassList = ShipBonusesMap(EffectData(0))
                Else
                    shipEffectClassList = New List(Of ShipEffect)
                    ShipBonusesMap.Add(EffectData(0), shipEffectClassList)
                End If
                newEffect.ShipID = CInt(EffectData(0))
                newEffect.AffectingType = CType(EffectData(1), EffectType)
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CType(EffectData(4), EffectType)
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CType(EffectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CType(EffectData(8), EffectCalcType)
                newEffect.Value = Double.Parse(EffectData(9), Globalization.NumberStyles.Any, culture)
                newEffect.Status = CInt(EffectData(10))
                shipEffectClassList.Add(newEffect)
            End If
        Next
    End Sub
    Public Shared Sub BuildSubSystemBonusMap()
        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.Subsystems.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String
        ' Build the map
        SubSystemEffectsMap.Clear()
        Dim shipEffectClassList As New List(Of ShipEffect)
        Dim newEffect As New ShipEffect
        Dim IDs() As String
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                If SubSystemEffectsMap.ContainsKey((EffectData(0))) = True Then
                    shipEffectClassList = SubSystemEffectsMap(EffectData(0))
                Else
                    shipEffectClassList = New List(Of ShipEffect)
                    SubSystemEffectsMap.Add(EffectData(0), shipEffectClassList)
                End If
                newEffect.ShipID = CInt(EffectData(0))
                newEffect.AffectingType = CType(EffectData(1), EffectType)
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CType(EffectData(4), EffectType)
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CType(EffectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CType(EffectData(8), EffectCalcType)
                newEffect.Value = Double.Parse(EffectData(9), Globalization.NumberStyles.Any, culture)
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
                    newEffect.AffectedType = CType(EffectData(4), EffectType)
                    If EffectData(5).Contains(";") = True Then
                        IDs = EffectData(5).Split(";".ToCharArray)
                        For Each ID As String In IDs
                            newEffect.AffectedID.Add(ID)
                        Next
                    Else
                        newEffect.AffectedID.Add(EffectData(5))
                    End If
                    newEffect.CalcType = CType(EffectData(6), EffectCalcType)
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
    Public Shared Function IsFlyable(ByVal hPilot As HQFPilot, ByVal testShip As Ship) As Boolean
        Dim usable As Boolean = True
        Dim rSkill As HQFSkill
        For Each reqSkill As ItemSkills In testShip.RequiredSkills.Values
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

#End Region

End Class

Public Enum BuildType As Integer
    BuildEverything = 0
    BuildEffectsMaps = 1
    BuildFromEffectsMaps = 2
End Enum



