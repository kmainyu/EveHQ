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
        Dim IDs() As String
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
                If AffectionData(5).Contains(";") = True Then
                    IDs = AffectionData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newAffection.AffectedID.Add(ID)
                    Next
                Else
                    newAffection.AffectedID.Add(AffectionData(5))
                End If
                newAffection.StackNerf = CBool(AffectionData(6))
                newAffection.IsPerLevel = CBool(AffectionData(7))
                newAffection.CalcType = CInt(AffectionData(8))
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
                                fAffection.Cause = hSkill.Name & " (Level " & hSkill.Level & ")"
                                fAffection.CalcType = chkAffection.CalcType
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
                                    fAffection.Cause = aModule.Name
                                    fAffection.CalcType = chkAffection.CalcType
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
        Call Engine.ApplyStackingPenalties()
        newShip = Engine.ApplyModuleAffectionsToModules(newShip)
        newShip = Engine.ApplyModuleAffectionsToShip(newShip)
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
        Dim processAtt As Boolean = False
        Dim log As String = ""
        ' Define a new ship
        Dim newShip As Ship = CType(baseShip.Clone, Ship)
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = CStr(newShip.Attributes.GetKey(attNo))
            If SkillAffectionsTable.Contains(att) = True Then
                For Each fAffection As FinalAffection In CType(SkillAffectionsTable(att), ArrayList)
                    processAtt = False
                    log = ""
                    Select Case fAffection.AffectedType
                        Case AffectionType.All
                            processAtt = True
                        Case AffectionType.Item
                            If fAffection.AffectedID.Contains(newShip.ID) Then
                                processAtt = True
                            End If
                        Case AffectionType.Group
                            If fAffection.AffectedID.Contains(newShip.DatabaseGroup) Then
                                processAtt = True
                            End If
                        Case AffectionType.Category
                            If fAffection.AffectedID.Contains(newShip.DatabaseCategory) Then
                                processAtt = True
                            End If
                        Case AffectionType.MarketGroup
                            If fAffection.AffectedID.Contains(newShip.MarketGroup) Then
                                processAtt = True
                            End If
                        Case AffectionType.Skill
                            If newShip.RequiredSkills.Contains(fAffection.AffectedID(0)) Then
                                processAtt = True
                            End If
                    End Select
                    If processAtt = True Then
                        log &= Attributes.AttributeQuickList(att).ToString & ": " & fAffection.Cause & ": " & newShip.Attributes(att).ToString
                        newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                        log &= " --> " & newShip.Attributes(att).ToString
                        newShip.AuditLog.Add(log)
                    End If
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
                        If SkillAffectionsTable.Contains(att) = True Then
                            For Each fAffection As FinalAffection In CType(SkillAffectionsTable(att), ArrayList)
                                processAtt = False
                                log = ""
                                Select Case fAffection.AffectedType
                                    Case AffectionType.All
                                        processAtt = True
                                    Case AffectionType.Item
                                        If fAffection.AffectedID.Contains(aModule.ID) Then
                                            processAtt = True
                                        End If
                                    Case AffectionType.Group
                                        If fAffection.AffectedID.Contains(aModule.DatabaseGroup) Then
                                            processAtt = True
                                        End If
                                    Case AffectionType.Category
                                        If fAffection.AffectedID.Contains(aModule.DatabaseCategory) Then
                                            processAtt = True
                                        End If
                                    Case AffectionType.MarketGroup
                                        If fAffection.AffectedID.Contains(aModule.MarketGroup) Then
                                            processAtt = True
                                        End If
                                    Case AffectionType.Skill
                                        If aModule.RequiredSkills.Contains(fAffection.AffectedID(0)) Then
                                            processAtt = True
                                        End If
                                End Select
                                If processAtt = True Then
                                    log &= Attributes.AttributeQuickList(att).ToString & ": " & fAffection.Cause & ": " & aModule.Attributes(att).ToString
                                    aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
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
        'MessageBox.Show("Applying Skill Affections to Modules took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
        Return newShip

    End Function

    Private Shared Function ApplyModuleAffectionsToModules(ByVal baseShip As Ship) As Ship
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
                        If ModuleAffectionsTable.Contains(att) = True Then
                            For Each fAffection As FinalAffection In CType(ModuleAffectionsTable(att), ArrayList)
                                processAtt = False
                                log = ""
                                Select Case fAffection.AffectedType
                                    Case AffectionType.All
                                        processAtt = True
                                    Case AffectionType.Item
                                        If fAffection.AffectedID.Contains(aModule.ID) Then
                                            processAtt = True
                                        End If
                                    Case AffectionType.Group
                                        If fAffection.AffectedID.Contains(aModule.DatabaseGroup) Then
                                            processAtt = True
                                        End If
                                    Case AffectionType.Category
                                        If fAffection.AffectedID.Contains(aModule.DatabaseCategory) Then
                                            processAtt = True
                                        End If
                                    Case AffectionType.MarketGroup
                                        If fAffection.AffectedID.Contains(aModule.MarketGroup) Then
                                            processAtt = True
                                        End If
                                    Case AffectionType.Skill
                                        If aModule.RequiredSkills.Contains(fAffection.AffectedID(0)) Then
                                            processAtt = True
                                        End If
                                End Select
                                If processAtt = True Then
                                    log &= Attributes.AttributeQuickList(att).ToString & ": " & fAffection.Cause & ": " & aModule.Attributes(att).ToString
                                    Select Case fAffection.CalcType
                                        Case AffectionCalcType.Percentage
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                                        Case AffectionCalcType.Addition
                                            aModule.Attributes(att) = CDbl(aModule.Attributes(att)) + fAffection.AffectedValue
                                        Case AffectionCalcType.Difference ' Used for resistances
                                            aModule.Attributes(att) = ((100 - CDbl(aModule.Attributes(att))) * fAffection.AffectedValue) + CDbl(aModule.Attributes(att))
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
        'MessageBox.Show("Applying Module Affections to Ship took " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms")
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
                            If fAffection.AffectedID.Contains(newShip.ID) Then
                                processAtt = True
                            End If
                        Case AffectionType.Group
                            If fAffection.AffectedID.Contains(newShip.DatabaseGroup) Then
                                processAtt = True
                            End If
                        Case AffectionType.Category
                            If fAffection.AffectedID.Contains(newShip.DatabaseCategory) Then
                                processAtt = True
                            End If
                        Case AffectionType.MarketGroup
                            If fAffection.AffectedID.Contains(newShip.MarketGroup) Then
                                processAtt = True
                            End If
                        Case AffectionType.Skill
                            If newShip.RequiredSkills.Contains(fAffection.AffectedID(0)) Then
                                processAtt = True
                            End If
                    End Select
                    If processAtt = True Then
                        Select Case fAffection.CalcType
                            Case AffectionCalcType.Percentage
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) * (1 + (fAffection.AffectedValue / 100))
                            Case AffectionCalcType.Addition
                                newShip.Attributes(att) = CDbl(newShip.Attributes(att)) + fAffection.AffectedValue
                            Case AffectionCalcType.Difference ' Used for resistances
                                newShip.Attributes(att) = ((100 - CDbl(newShip.Attributes(att))) * fAffection.AffectedValue) + CDbl(newShip.Attributes(att))
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

    Private Shared Sub ApplyStackingPenalties()
        Dim baseAffectionList As New ArrayList
        Dim finalAffectionList As New ArrayList
        Dim tempPAffectionList As New SortedList
        Dim tempNAffectionList As New SortedList
        Dim attOrder(,) As Double
        Dim att As String
        For attNumber As Integer = 0 To ModuleAffectionsTable.Keys.Count - 1
            att = CStr(ModuleAffectionsTable.GetKey(attNumber))
            baseAffectionList = CType(ModuleAffectionsTable(att), ArrayList)
            tempPAffectionList.Clear()
            tempNAffectionList.Clear()
            finalAffectionList = New ArrayList
            For Each fAffection As FinalAffection In baseAffectionList
                If fAffection.StackNerf = True Then
                    If fAffection.AffectedValue >= 0 Then
                        tempPAffectionList.Add(tempPAffectionList.Count.ToString, fAffection)
                    Else
                        tempNAffectionList.Add(tempNAffectionList.Count.ToString, fAffection)
                    End If
                Else
                    finalAffectionList.Add(fAffection)
                End If
            Next
            If tempPAffectionList.Count > 0 Then
                ReDim attOrder(tempPAffectionList.Count - 1, 1)
                Dim sAffection As FinalAffection
                For Each attNo As String In tempPAffectionList.Keys
                    sAffection = CType(tempPAffectionList(attNo), FinalAffection)
                    attOrder(CInt(attNo), 0) = CDbl(attNo)
                    attOrder(CInt(attNo), 1) = sAffection.AffectedValue
                Next
                ' Create a tag array ready to sort the skill times
                Dim tagArray(tempPAffectionList.Count - 1) As Integer
                For a As Integer = 0 To tempPAffectionList.Count - 1
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
                    sAffection = CType(tempPAffectionList(idx.ToString), FinalAffection)
                    penalty = Math.Exp(-(i ^ 2 / 7.1289))
                    sAffection.AffectedValue = sAffection.AffectedValue * penalty
                    sAffection.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalAffectionList.Add(sAffection)
                Next
            End If
            If tempNAffectionList.Count > 0 Then
                ReDim attOrder(tempNAffectionList.Count - 1, 1)
                Dim sAffection As FinalAffection
                For Each attNo As String In tempNAffectionList.Keys
                    sAffection = CType(tempNAffectionList(attNo), FinalAffection)
                    attOrder(CInt(attNo), 0) = CDbl(attNo)
                    attOrder(CInt(attNo), 1) = sAffection.AffectedValue
                Next
                ' Create a tag array ready to sort the skill times
                Dim tagArray(tempNAffectionList.Count - 1) As Integer
                For a As Integer = 0 To tempNAffectionList.Count - 1
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
                    sAffection = CType(tempNAffectionList(idx.ToString), FinalAffection)
                    penalty = Math.Exp(-(i ^ 2 / 7.1289))
                    sAffection.AffectedValue = sAffection.AffectedValue * penalty
                    sAffection.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalAffectionList.Add(sAffection)
                Next
            End If
            ModuleAffectionsTable(att) = finalAffectionList
        Next
    End Sub

#End Region

End Class

Public Class Affection
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

Public Class FinalAffection
    Public AffectedAtt As Integer
    Public AffectedType As Integer
    Public AffectedID As New ArrayList
    Public AffectedValue As Double
    Public StackNerf As Boolean
    Public Cause As String
    Public CalcType As Integer
End Class

Public Enum AffectionType
    All = 0
    Item = 1
    Group = 2
    Category = 3
    MarketGroup = 4
    Skill = 5
End Enum

Public Enum AffectionCalcType
    Percentage = 0
    Addition = 1
    Difference = 2
End Enum


