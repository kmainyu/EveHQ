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
Imports EveHQ.EveData
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class EffectFunctions

    ''' <summary>
    ''' Function to convert a List of Ship Bonuses to a text format for display
    ''' </summary>
    ''' <param name="shipBonuses">A List(Of ShipEffect) containing the ShipEffects</param>
    ''' <returns>A string containing a description of the collection of bonuses</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertShipBonusesToDescription(ByVal shipBonuses As List(Of ShipEffect)) As String
        Return ConvertShipBonuses(ShipBonuses)
    End Function

    ''' <summary>
    ''' Function to convert a ShipEffect to a text format for display
    ''' </summary>
    ''' <param name="shipBonus">The ShipEffect to convert</param>
    ''' <returns>A string containing a description of the bonus</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertShipBonusesToDescription(ByVal shipBonus As ShipEffect) As String
        Dim shipBonuses As New List(Of ShipEffect)
        shipBonuses.Add(shipBonus)
        Return ConvertShipBonuses(shipBonuses)
    End Function

    ''' <summary>
    ''' Converts a list of ShipBonuses to a text format for display
    ''' </summary>
    ''' <param name="shipBonuses">A List(Of ShipEffect) containing the ShipEffects</param>
    ''' <returns>A string containing a description of the collection of bonuses</returns>
    ''' <remarks></remarks>
    Private Shared Function ConvertShipBonuses(ByVal shipBonuses As List(Of ShipEffect)) As String
        Dim bonusDescription As New StringBuilder

        ' Create a SortedList to use to group skills etc
        ' Use key of "0" to identify roles
        Dim bonuses As New SortedList(Of Integer, List(Of ShipEffect))
        Dim bonusGroup As List(Of ShipEffect)
        For Each bonus As ShipEffect In shipBonuses
            ' Check if the bonus group already exists - create it if it doesn't
            If bonuses.ContainsKey(bonus.AffectingID) = False Then
                bonusGroup = New List(Of ShipEffect)
                bonuses.Add(bonus.AffectingID, bonusGroup)
            Else
                bonusGroup = bonuses(bonus.AffectingID)
            End If
            ' Add the bonus to the bonus group
            bonusGroup.Add(bonus)
        Next

        ' Go through the bonus groups and parse the data as a string description
        For Each bonusSkill As Integer In bonuses.Keys
            bonusGroup = bonuses(bonusSkill)
            ' Write the skill if not zero, or "role bonus" if so
            If bonusSkill = 0 Then
                If bonusGroup.Count = 1 Then
                    bonusDescription.AppendLine("Role Bonus:")
                Else
                    bonusDescription.AppendLine("Role Bonuses:")
                End If
            Else
                ' Get the skill name
                Dim skill As EveType = StaticData.Types(bonusSkill)
                If bonusGroup.Count = 1 Then
                    bonusDescription.AppendLine(skill.Name & " Skill Bonus:")
                Else
                    bonusDescription.AppendLine(skill.Name & " Skill Bonuses:")
                End If
            End If

            ' Now write the bonuses - use increase/reduction for wording to avoid bonus vagueness
            For Each bonus As ShipEffect In bonusGroup
                Dim desc As New StringBuilder

                ' Display value
                Select Case bonus.CalcType
                    Case EffectCalcType.Percentage
                        desc.Append(Math.Abs(bonus.Value).ToString & "% ")
                        If bonus.Value >= 0 Then
                            desc.Append("increase to ")
                        Else
                            desc.Append("reduction to ")
                        End If
                    Case EffectCalcType.Addition
                        If bonus.Value > 0 Then
                            desc.Append("+")
                        Else
                            desc.Append("-")
                        End If
                        desc.Append(Math.Abs(bonus.Value).ToString("N0") & " to ")
                    Case EffectCalcType.Difference
                        desc.Append(Math.Abs(bonus.Value).ToString & "% ")
                        If bonus.Value <= 0 Then
                            desc.Append("increase to ")
                        Else
                            desc.Append("reduction to ")
                        End If
                End Select

                ' Display attribute
                Dim culture As New CultureInfo("en-GB")
                Dim textInfo As TextInfo = culture.TextInfo
                If Attributes.AttributeList.ContainsKey(bonus.AffectedAtt) = True Then
                    desc.Append(textInfo.ToTitleCase(Attributes.AttributeList(bonus.AffectedAtt).DisplayName))
                Else
                    desc.Append("an unselected attribute")
                End If

                ' Display IDs
                Select Case bonus.AffectedType
                    Case HQFEffectType.All
                        desc.Append(" of everything")
                    Case HQFEffectType.Item
                        Dim ids As List(Of Integer) = bonus.AffectedID
                        If ids.Count > 0 Then
                            ' Check for the ship type
                            If ids.Count = 1 And CInt(ids(0)) = bonus.ShipID Then
                                ' Do we want any reference to the ship here?? Probably not
                            Else
                                Dim itemList As New List(Of String)
                                For Each id As Integer In ids
                                    itemList.Add(StaticData.Types(id).Name.Trim)
                                Next
                                desc.Append(" of " & ParseStringListToProperText(itemList, True))
                            End If
                        Else
                            ' No IDs?
                            desc.Append(" of nothing")
                        End If
                    Case HQFEffectType.Group
                        Dim ids As List(Of Integer) = bonus.AffectedID
                        If ids.Count > 0 Then
                            Dim itemList As New List(Of String)
                            For Each id As Integer In ids
                                itemList.Add(StaticData.TypeGroups(id).Trim)
                            Next
                            desc.Append(" of " & ParseStringListToProperText(itemList, True))
                        Else
                            ' No IDs?
                            desc.Append(" of nothing")
                        End If
                    Case HQFEffectType.Category
                        Dim ids As List(Of Integer) = bonus.AffectedID
                        If ids.Count > 0 Then
                            Dim itemList As New List(Of String)
                            For Each id As Integer In ids
                                itemList.Add(StaticData.TypeCats(id).Trim)
                            Next
                            desc.Append(" of " & ParseStringListToProperText(itemList, True))
                        Else
                            ' No IDs?
                            desc.Append(" of nothing")
                        End If
                    Case HQFEffectType.MarketGroup
                        Dim ids As List(Of Integer) = bonus.AffectedID
                        If ids.Count > 0 Then
                            Dim itemList As New List(Of String)
                            For Each id As Integer In ids
                                Dim mgPath As String = Market.MarketGroupPath(CStr(id))
                                Dim mgPaths() As String = mgPath.Split("\".ToCharArray)
                                Select Case mgPaths(mgPaths.Length - 1)
                                    Case "Small", "Medium", "Large", "Extra Large"
                                        itemList.Add(mgPaths(mgPaths.Length - 1) & " " & mgPaths(mgPaths.Length - 2))
                                    Case Else
                                        itemList.Add(Market.MarketGroupList(CStr(id)))
                                End Select
                            Next
                            desc.Append(" of " & ParseStringListToProperText(itemList, True))
                        Else
                            ' No IDs?
                            desc.Append(" of nothing")
                        End If
                    Case HQFEffectType.Skill
                        Dim ids As List(Of Integer) = bonus.AffectedID
                        If ids.Count > 0 Then
                            Dim itemList As New List(Of String)
                            For Each id As Integer In ids
                                itemList.Add(StaticData.Types(id).Name.Trim)
                            Next
                            desc.Append(" of items requiring the " & ParseStringListToProperText(itemList, False) & " skill")
                            If ids.Count > 1 Then desc.Append("s")
                        Else
                            ' No IDs?
                            desc.Append(" of nothing")
                        End If
                    Case HQFEffectType.Slot
                        desc.Append(" of the occupied slot")
                    Case HQFEffectType.Attribute
                        Dim ids As List(Of Integer) = bonus.AffectedID
                        If ids.Count > 0 Then
                            Dim itemList As New List(Of String)
                            For Each id As Integer In ids
                                itemList.Add(Attributes.AttributeQuickList(id).ToString.Trim)
                            Next
                            desc.Append(" of items containing the " & ParseStringListToProperText(itemList, False) & " attribute")
                            If ids.Count > 1 Then desc.Append("s")
                        Else
                            ' No IDs?
                            desc.Append(" of nothing")
                        End If
                    Case Else
                        desc.Append(" of something that EveHQ has yet to add code for")
                End Select

                ' Check if this is per level, ignore for a role bonus
                If bonusSkill <> 0 Then
                    desc.Append(" per level")
                End If

                bonusDescription.AppendLine(desc.ToString)
            Next
            bonusDescription.AppendLine("")
        Next

        Return bonusDescription.ToString
    End Function

    ''' <summary>
    ''' Function to parse a list of string items into a proper readable text format
    ''' </summary>
    ''' <param name="itemList">A List(Of String) containing the items to be parsed</param>
    ''' <returns>A string containing a readable list of items in proper text format</returns>
    ''' <remarks></remarks>
    Public Shared Function ParseStringListToProperText(ByVal itemList As List(Of String), ByVal usePlural As Boolean) As String
        Dim items As New StringBuilder
        Select Case itemList.Count
            Case 0
                Return ""
            Case 1
                If usePlural = True Then
                    If itemList(0).EndsWith("s") Then
                        Return itemList(0)
                    Else
                        If itemList(0).EndsWith("o") Then
                            Return itemList(0) & "es"
                        Else
                            Return itemList(0) & "s"
                        End If
                    End If
                Else
                    Return itemList(0)
                End If
            Case Is > 1
                ' Extract the last item
                Dim lastItem As String
                If usePlural = True Then
                    If itemList(itemList.Count - 1).EndsWith("s") Then
                        lastItem = itemList(itemList.Count - 1)
                    Else
                        If itemList(itemList.Count - 1).EndsWith("o") Then
                            lastItem = itemList(itemList.Count - 1) & "es"
                        Else
                            lastItem = itemList(itemList.Count - 1) & "s"
                        End If
                    End If
                Else
                    lastItem = itemList(itemList.Count - 1)
                End If
                lastItem = " and " & lastItem
                ' Make a list of the others with commas
                For idx As Integer = 0 To itemList.Count - 2
                    If usePlural = True Then
                        If itemList(idx).EndsWith("s") Then
                            items.Append(", " & itemList(idx))
                        Else
                            If itemList(idx).EndsWith("o") Then
                                items.Append(", " & itemList(idx) & "es")
                            Else
                                items.Append(", " & itemList(idx) & "s")
                            End If
                        End If
                    Else
                        items.Append(", " & itemList(idx))
                    End If
                Next
                ' Add the last item
                items.Append(lastItem)
                ' Remove the initial text
                items.Remove(0, 2)
                Return items.ToString
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' Function to return the ID of an attribute from the combined name/ID in the Effect Attribute lists
    ''' </summary>
    ''' <param name="attData">The string data to parse</param>
    ''' <returns>An integer representing the ID of the attribute</returns>
    ''' <remarks></remarks>
    Public Shared Function ExtractIDFromAttributeDetails(ByVal attData As String) As Integer
        Dim newRegex As New Regex("\((?'Number'\d+)\)")
        Dim newMatch As Match = newRegex.Match(AttData)
        If newMatch.Success = False Then
            Return 0
        Else
            Return CInt(newMatch.Groups("Number").Captures(0).Value)
        End If
    End Function

End Class
