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

#Region "New Routines"
    Public Shared Sub BuildAffectionsMap()
        ' Fetch the affections list
        Dim AffectionFile As String = My.Resources.Affections.ToString
        ' Break the affections down into separate lines
        Dim AffectionLines() As String = AffectionFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim AffectionData() As String
        ' Build the map
        AffectionsMap.Clear()
        Dim AffectionClassList As New ArrayList
        Dim newAffection As New Affection
        For Each AffectionLine As String In AffectionLines
            If AffectionLine.Trim <> "" And AffectionLine.StartsWith("#") = False Then
                AffectionData = AffectionLine.Split(",".ToCharArray)
                newAffection = New Affection
                If AffectionsMap.Contains((AffectionData(0))) = True Then
                    AffectionClassList = CType(AffectionsMap(AffectionData(0)), ArrayList)
                Else
                    AffectionClassList = New ArrayList
                    AffectionsMap.Add(AffectionData(0), AffectionClassList)
                End If
                newAffection.AffectingAtt = CInt(AffectionData(0))
                newAffection.AffectingType = CInt(AffectionData(1))
                newAffection.AffectingID = CInt(AffectionData(2))
                newAffection.AffectedAtt = CInt(AffectionData(3))
                newAffection.AffectedType = CInt(AffectionData(4))
                newAffection.AffectedID = CInt(AffectionData(5))
                newAffection.StackNerf = CBool(AffectionData(6))
                newAffection.IsPerLevel = CBool(AffectionData(7))
                AffectionClassList.Add(newAffection)
            End If
        Next
    End Sub

    Public Shared Sub BuildSkillAffections(ByVal hPilot As HQFPilot)
        Dim sTime, eTime As Date
        sTime = Now
        ' Clear the Affections Table
        SkillAffectionsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim aSkill As New Skill
        Dim fAffection As New FinalAffection
        Dim fAffectionList As New ArrayList
        For Each hSkill As HQFSkill In hPilot.SkillSet
            If hSkill.Level <> 0 Then
                ' Go through the attributes
                aSkill = CType(SkillLists.SkillList(hSkill.ID), Skill)
                For Each att As String In aSkill.Attributes.Keys
                    If AffectionsMap.Contains(att) = True Then
                        For Each chkAffection As Affection In CType(AffectionsMap(att), ArrayList)
                            If chkAffection.AffectingType = AffectionType.Item And chkAffection.AffectingID = CInt(aSkill.ID) Then
                                fAffection = New FinalAffection
                                fAffection.AffectedAtt = chkAffection.AffectedAtt
                                fAffection.AffectedType = chkAffection.AffectedType
                                fAffection.AffectedID = chkAffection.AffectedID
                                fAffection.AffectedValue = CDbl(aSkill.Attributes(CInt(att))) * hSkill.Level
                                fAffection.StackNerf = chkAffection.StackNerf
                                If SkillAffectionsTable.Contains(fAffection.AffectedAtt.ToString) = False Then
                                    fAffectionList = New ArrayList
                                    SkillAffectionsTable.Add(fAffection.AffectedAtt.ToString, fAffectionList)
                                Else
                                    fAffectionList = CType(SkillAffectionsTable(fAffection.AffectedAtt.ToString), Collections.ArrayList)
                                End If
                                fAffectionList.Add(fAffection)
                            End If
                        Next
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
        Dim maxSlots As Integer = 0
        Dim aModule As New ShipModule
        Dim attData As New ArrayList
        Dim fAffection As New FinalAffection
        Dim fAffectionList As New ArrayList
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
                        If AffectionsMap.Contains(att) = True Then
                            For Each chkAffection As Affection In CType(AffectionsMap(att), ArrayList)
                                processData = False
                                Select Case chkAffection.AffectingType
                                    Case AffectionType.All
                                        processData = True
                                    Case AffectionType.Item
                                        If chkAffection.AffectingID.ToString = aModule.ID Then
                                            processData = True
                                        End If
                                    Case AffectionType.Group
                                        If chkAffection.AffectingID.ToString = aModule.DatabaseGroup Then
                                            processData = True
                                        End If
                                    Case AffectionType.Category
                                        If chkAffection.AffectingID.ToString = aModule.DatabaseCategory Then
                                            processData = True
                                        End If
                                    Case AffectionType.MarketGroup
                                        If chkAffection.AffectingID.ToString = aModule.MarketGroup Then
                                            processData = True
                                        End If
                                    Case AffectionType.Skill
                                        If aModule.RequiredSkills.Contains(chkAffection.AffectingID.ToString) Then
                                            processData = True
                                        End If
                                End Select
                                If processData = True Then
                                    fAffection = New FinalAffection
                                    fAffection.AffectedAtt = chkAffection.AffectedAtt
                                    fAffection.AffectedType = chkAffection.AffectedType
                                    fAffection.AffectedID = chkAffection.AffectedID
                                    fAffection.AffectedValue = CDbl(aModule.Attributes(att))
                                    fAffection.StackNerf = chkAffection.StackNerf
                                    If ModuleAffectionsTable.Contains(fAffection.AffectedAtt.ToString) = False Then
                                        fAffectionList = New ArrayList
                                        ModuleAffectionsTable.Add(fAffection.AffectedAtt.ToString, fAffectionList)
                                    Else
                                        fAffectionList = CType(ModuleAffectionsTable(fAffection.AffectedAtt.ToString), Collections.ArrayList)
                                    End If
                                    fAffectionList.Add(fAffection)
                                End If
                            Next
                        End If
                    Next
                End If
            Next
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Building Module Affections took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip
    End Function

    Public Shared Function ApplyFitting(ByVal baseShip As Ship, ByVal shipPilot As HQFPilot) As Ship
        ' Apply the pilot skills to the ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim newShip As Ship
        newShip = ApplySkillAffectionsToShip(CType(baseShip.Clone, Ship))
        newShip = ApplySkillAffectionsToModules(newShip)
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
        Dim thisAffection As New FinalAffection
        Dim att As String = ""
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = CStr(newShip.Attributes.GetKey(attNo))
            If SkillAffectionsTable.Contains(att) = True Then
                For Each fAffection As FinalAffection In CType(SkillAffectionsTable(att), ArrayList)
                    Select Case fAffection.AffectedType
                        Case AffectionType.All
                            newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                        Case AffectionType.Item
                            If newShip.ID = fAffection.AffectedID.ToString Then
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                            End If
                        Case AffectionType.Group
                            If newShip.DatabaseGroup = fAffection.AffectedID.ToString Then
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                            End If
                        Case AffectionType.Category
                            If newShip.DatabaseCategory = fAffection.AffectedID.ToString Then
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                            End If
                        Case AffectionType.MarketGroup
                            If newShip.MarketGroup = fAffection.AffectedID.ToString Then
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                            End If
                        Case AffectionType.Skill
                            If newShip.RequiredSkills.Contains(fAffection.AffectedID.ToString) Then
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                            End If
                    End Select
                Next
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
        Dim aModule As New ShipModule
        Dim maxSlots As Integer = 0
        Dim att As String
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
                        If SkillAffectionsTable.Contains(att) = True Then
                            For Each fAffection As FinalAffection In CType(SkillAffectionsTable(att), ArrayList)
                                Select Case fAffection.AffectedType
                                    Case AffectionType.All
                                        aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                                    Case AffectionType.Item
                                        If aModule.ID = fAffection.AffectedID.ToString Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                                        End If
                                    Case AffectionType.Group
                                        If aModule.DatabaseGroup = fAffection.AffectedID.ToString Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                                        End If
                                    Case AffectionType.Category
                                        If aModule.DatabaseCategory = fAffection.AffectedID.ToString Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                                        End If
                                    Case AffectionType.MarketGroup
                                        If aModule.MarketGroup = fAffection.AffectedID.ToString Then
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                                        End If
                                    Case AffectionType.Skill
                                        If aModule.RequiredSkills.Contains(fAffection.AffectedID.ToString) Then
                                            newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                                        End If
                                End Select
                            Next
                        End If
                    Next
                    ShipModule.MapModuleAttributes(aModule)
                End If
            Next
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
        'MessageBox.Show("Applying Skill Affections to Modules took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip

    End Function

    Private Shared Function ApplyModuleAffectionsToShip(ByVal baseShip As Ship) As Ship
        Dim sTime, eTime As Date
        sTime = Now
        Dim att As String = ""
        Dim processAtt As Boolean = False
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = CStr(newShip.Attributes.GetKey(attNo))
            If ModuleAffectionsTable.Contains(att) = True Then
                For Each fAffection As FinalAffection In CType(ModuleAffectionsTable(att), ArrayList)
                    processAtt = False
                    Select Case fAffection.AffectedType
                        Case AffectionType.All
                            processAtt = True
                        Case AffectionType.Item
                            If newShip.ID = fAffection.AffectedID.ToString Then
                                processAtt = True
                            End If
                        Case AffectionType.Group
                            If newShip.DatabaseGroup = fAffection.AffectedID.ToString Then
                                processAtt = True
                            End If
                        Case AffectionType.Category
                            If newShip.DatabaseCategory = fAffection.AffectedID.ToString Then
                                processAtt = True
                            End If
                        Case AffectionType.MarketGroup
                            If newShip.MarketGroup = fAffection.AffectedID.ToString Then
                                processAtt = True
                            End If
                        Case AffectionType.Skill
                            If newShip.RequiredSkills.Contains(fAffection.AffectedID.ToString) Then
                                processAtt = True
                            End If
                    End Select
                    If processAtt = True Then
                        Select Case CInt(att)
                            Case 49, 15, 1154
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fAffection.AffectedValue
                            Case Else
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
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

Public Class Affection
    Public AffectingAtt As Integer
    Public AffectingType As Integer
    Public AffectingID As Integer
    Public AffectedAtt As Integer
    Public AffectedType As Integer
    Public AffectedID As Integer
    Public StackNerf As Boolean
    Public IsPerLevel As Boolean
End Class

Public Class FinalAffection
    Public AffectedAtt As Integer
    Public AffectedType As Integer
    Public AffectedID As Integer
    Public AffectedValue As Double
    Public StackNerf As Boolean
End Class

Public Enum AffectionType
    All = 0
    Item = 1
    Group = 2
    Category = 3
    MarketGroup = 4
    Skill = 5
End Enum


