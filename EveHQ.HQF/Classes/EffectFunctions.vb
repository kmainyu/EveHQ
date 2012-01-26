' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
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

Imports System.Text
Imports System.Text.RegularExpressions

Public Class EffectFunctions

    ''' <summary>
    ''' Function to convert a List of Ship Bonuses to a text format for display
    ''' </summary>
    ''' <param name="ShipBonuses">A List(Of ShipEffect) containing the ShipEffects</param>
    ''' <returns>A string containing a description of the collection of bonuses</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertShipBonusesToDescription(ByVal ShipBonuses As List(Of ShipEffect)) As String
        Return ConvertShipBonuses(ShipBonuses)
    End Function

    ''' <summary>
    ''' Function to convert a ShipEffect to a text format for display
    ''' </summary>
    ''' <param name="ShipBonus">The ShipEffect to convert</param>
    ''' <returns>A string containing a description of the bonus</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertShipBonusesToDescription(ByVal ShipBonus As ShipEffect) As String
        Dim ShipBonuses As New List(Of ShipEffect)
        ShipBonuses.Add(ShipBonus)
        Return ConvertShipBonuses(ShipBonuses)
    End Function

    ''' <summary>
    ''' Converts a list of ShipBonuses to a text format for display
    ''' </summary>
    ''' <param name="ShipBonuses">A List(Of ShipEffect) containing the ShipEffects</param>
    ''' <returns>A string containing a description of the collection of bonuses</returns>
    ''' <remarks></remarks>
    Private Shared Function ConvertShipBonuses(ByVal ShipBonuses As List(Of ShipEffect)) As String
        Dim BonusDescription As New StringBuilder

        ' Create a SortedList to use to group skills etc
        ' Use key of "0" to identify roles
        Dim Bonuses As New SortedList(Of String, List(Of ShipEffect))
        Dim BonusGroup As New List(Of ShipEffect)
        For Each bonus As ShipEffect In ShipBonuses
            ' Check if the bonus group already exists - create it if it doesn't
            If Bonuses.ContainsKey(bonus.AffectingID.ToString) = False Then
                BonusGroup = New List(Of ShipEffect)
                Bonuses.Add(bonus.AffectingID.ToString, BonusGroup)
            Else
                BonusGroup = Bonuses(bonus.AffectingID.ToString)
            End If
            ' Add the bonus to the bonus group
            BonusGroup.Add(bonus)
        Next

        ' Go through the bonus groups and parse the data as a string description
        For Each BonusSkill As String In Bonuses.Keys
            BonusGroup = Bonuses(BonusSkill)
            ' Write the skill if not zero, or "role bonus" if so
            If BonusSkill = "0" Then
                If BonusGroup.Count = 1 Then
                    BonusDescription.AppendLine("Role Bonus:")
                Else
                    BonusDescription.AppendLine("Role Bonuses:")
                End If
            Else
                ' Get the skill name
                Dim Skill As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(BonusSkill)
                If BonusGroup.Count = 1 Then
                    BonusDescription.AppendLine(Skill.Name & " Skill Bonus:")
                Else
                    BonusDescription.AppendLine(Skill.Name & " Skill Bonuses:")
                End If
            End If

            ' Now write the bonuses - use increase/reduction for wording to avoid bonus vagueness
            For Each bonus As ShipEffect In BonusGroup
                Dim Desc As New StringBuilder

                ' Display value
                Select Case bonus.CalcType
                    Case EffectCalcType.Percentage
                        Desc.Append(Math.Abs(bonus.Value).ToString & "% ")
                        If bonus.Value >= 0 Then
                            Desc.Append("increase to ")
                        Else
                            Desc.Append("reduction to ")
                        End If
                    Case EffectCalcType.Addition
                        If bonus.Value > 0 Then
                            Desc.Append("+")
                        Else
                            Desc.Append("-")
                        End If
                        Desc.Append(Math.Abs(bonus.Value).ToString("N0") & " to ")
                    Case EffectCalcType.Difference
                        Desc.Append(Math.Abs(bonus.Value).ToString & "% ")
                        If bonus.Value <= 0 Then
                            Desc.Append("increase to ")
                        Else
                            Desc.Append("reduction to ")
                        End If
                End Select

                ' Display attribute
                Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
                Dim TextInfo As Globalization.TextInfo = culture.TextInfo
                If Attributes.AttributeList.ContainsKey(bonus.AffectedAtt.ToString) = True Then
                    Desc.Append(TextInfo.ToTitleCase(CType(Attributes.AttributeList(bonus.AffectedAtt.ToString), Attribute).DisplayName))
                Else
                    Desc.Append("an unselected attribute")
                End If

                ' Display IDs
                Select Case bonus.AffectedType
                    Case EffectType.All
                        Desc.Append(" of everything")
                    Case EffectType.Item
                        Dim IDs As List(Of String) = bonus.AffectedID
                        If IDs.Count > 0 Then
                            ' Check for the ship type
                            If IDs.Count = 1 And CInt(IDs(0)) = bonus.ShipID Then
                                ' Do we want any reference to the ship here?? Probably not
                            Else
                                Dim ItemList As New List(Of String)
                                For Each ID As String In IDs
                                    ItemList.Add(EveHQ.Core.HQ.itemData(ID).Name.Trim)
                                Next
                                Desc.Append(" of " & ParseStringListToProperText(ItemList, True))
                            End If
                        Else
                            ' No IDs?
                            Desc.Append(" of nothing")
                        End If
                    Case EffectType.Group
                        Dim IDs As List(Of String) = bonus.AffectedID
                        If IDs.Count > 0 Then
                            Dim ItemList As New List(Of String)
                            For Each ID As String In IDs
                                ItemList.Add(EveHQ.Core.HQ.itemGroups(ID).Trim)
                            Next
                            Desc.Append(" of " & ParseStringListToProperText(ItemList, True))
                        Else
                            ' No IDs?
                            Desc.Append(" of nothing")
                        End If
                    Case EffectType.Category
                        Dim IDs As List(Of String) = bonus.AffectedID
                        If IDs.Count > 0 Then
                            Dim ItemList As New List(Of String)
                            For Each ID As String In IDs
                                ItemList.Add(EveHQ.Core.HQ.itemCats(ID).Trim)
                            Next
                            Desc.Append(" of " & ParseStringListToProperText(ItemList, True))
                        Else
                            ' No IDs?
                            Desc.Append(" of nothing")
                        End If
                    Case EffectType.MarketGroup
                        Dim IDs As List(Of String) = bonus.AffectedID
                        If IDs.Count > 0 Then
                            Dim ItemList As New List(Of String)
                            For Each ID As String In IDs
                                Dim MGPath As String = HQF.Market.MarketGroupPath(ID).ToString
                                Dim MGPaths() As String = MGPath.Split("\".ToCharArray)
                                Select Case MGPaths(MGPaths.Length - 1)
                                    Case "Small", "Medium", "Large", "Extra Large"
                                        ItemList.Add(MGPaths(MGPaths.Length - 1) & " " & MGPaths(MGPaths.Length - 2))
                                    Case Else
                                        ItemList.Add(HQF.Market.MarketGroupList(ID).ToString)
                                End Select
                            Next
                            Desc.Append(" of " & ParseStringListToProperText(ItemList, True))
                        Else
                            ' No IDs?
                            Desc.Append(" of nothing")
                        End If
                    Case EffectType.Skill
                        Dim IDs As List(Of String) = bonus.AffectedID
                        If IDs.Count > 0 Then
                            Dim ItemList As New List(Of String)
                            For Each ID As String In IDs
                                ItemList.Add(EveHQ.Core.HQ.itemData(ID).Name.Trim)
                            Next
                            Desc.Append(" of items requiring the " & ParseStringListToProperText(ItemList, False) & " skill")
                            If IDs.Count > 1 Then Desc.Append("s")
                        Else
                            ' No IDs?
                            Desc.Append(" of nothing")
                        End If
                    Case EffectType.Slot
                        Desc.Append(" of the occupied slot")
                    Case EffectType.Attribute
                        Dim IDs As List(Of String) = bonus.AffectedID
                        If IDs.Count > 0 Then
                            Dim ItemList As New List(Of String)
                            For Each ID As String In IDs
                                ItemList.Add(Attributes.AttributeQuickList(ID).ToString.Trim)
                            Next
                            Desc.Append(" of items containing the " & ParseStringListToProperText(ItemList, False) & " attribute")
                            If IDs.Count > 1 Then Desc.Append("s")
                        Else
                            ' No IDs?
                            Desc.Append(" of nothing")
                        End If
                    Case Else
                        Desc.Append(" of something that Vessper has yet to add code for")
                End Select

                ' Check if this is per level, ignore for a role bonus
                If BonusSkill <> "0" Then
                    Desc.Append(" per level")
                End If

                BonusDescription.AppendLine(Desc.ToString)
            Next
            BonusDescription.AppendLine("")
        Next

        Return BonusDescription.ToString
    End Function

    ''' <summary>
    ''' Function to parse a list of string items into a proper readable text format
    ''' </summary>
    ''' <param name="ItemList">A List(Of String) containing the items to be parsed</param>
    ''' <returns>A string containing a readable list of items in proper text format</returns>
    ''' <remarks></remarks>
    Public Shared Function ParseStringListToProperText(ByVal ItemList As List(Of String), ByVal UsePlural As Boolean) As String
        Dim Items As New StringBuilder
        Select Case ItemList.Count
            Case 0
                Return ""
            Case 1
                If UsePlural = True Then
                    If ItemList(0).EndsWith("s") Then
                        Return ItemList(0)
                    Else
                        If ItemList(0).EndsWith("o") Then
                            Return ItemList(0) & "es"
                        Else
                            Return ItemList(0) & "s"
                        End If
                    End If
                Else
                    Return ItemList(0)
                End If
            Case Is > 1
                ' Extract the last item
                Dim LastItem As String = ""
                If UsePlural = True Then
                    If ItemList(ItemList.Count - 1).EndsWith("s") Then
                        LastItem = ItemList(ItemList.Count - 1)
                    Else
                        If ItemList(ItemList.Count - 1).EndsWith("o") Then
                            LastItem = ItemList(ItemList.Count - 1) & "es"
                        Else
                            LastItem = ItemList(ItemList.Count - 1) & "s"
                        End If
                    End If
                Else
                    LastItem = ItemList(ItemList.Count - 1)
                End If
                LastItem = " and " & LastItem
                ' Make a list of the others with commas
                For idx As Integer = 0 To ItemList.Count - 2
                    If UsePlural = True Then
                        If ItemList(idx).EndsWith("s") Then
                            Items.Append(", " & ItemList(idx))
                        Else
                            If ItemList(idx).EndsWith("o") Then
                                Items.Append(", " & ItemList(idx) & "es")
                            Else
                                Items.Append(", " & ItemList(idx) & "s")
                            End If
                        End If
                    Else
                        Items.Append(", " & ItemList(idx))
                    End If
                Next
                ' Add the last item
                Items.Append(LastItem)
                ' Remove the initial text
                Items.Remove(0, 2)
                Return Items.ToString
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' Function to return the ID of an attribute from the combined name/ID in the Effect Attribute lists
    ''' </summary>
    ''' <param name="AttData">The string data to parse</param>
    ''' <returns>An integer representing the ID of the attribute</returns>
    ''' <remarks></remarks>
    Public Shared Function ExtractIDFromAttributeDetails(ByVal AttData As String) As Integer
        Dim NewRegex As New Regex("\((?'Number'\d+)\)")
        Dim NewMatch As Match = NewRegex.Match(AttData)
        If NewMatch.Success = False Then
            Return 0
        Else
            Return CInt(NewMatch.Groups("Number").Captures(0).Value)
        End If
    End Function

End Class
