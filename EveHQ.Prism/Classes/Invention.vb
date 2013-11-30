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
Imports EveHQ.Prism.BPCalc
Imports EveHQ.EveData

Namespace Classes

    Public Class Invention

        Public Shared Function LoadInventionData() As Boolean

            ' Load all the decryptor data
            Dim groupIDs As List(Of Integer) = {728, 729, 730, 731}.ToList
            PlugInData.Decryptors.Clear()
            For Each groupID As Integer In groupIDs
                ' Get the items in each group
                Dim items As IEnumerable(Of EveType) = StaticData.GetItemsInGroup(groupID)
                For Each item As EveType In items
                    ' Set data
                    Dim newDecryptor As New BPCalc.Decryptor
                    newDecryptor.ID = item.Id
                    newDecryptor.Name = item.Name
                    newDecryptor.GroupID = item.Group.ToString
                    PlugInData.Decryptors.Add(newDecryptor.Name, newDecryptor)
                    ' Get attributes of each item
                    Dim atts As SortedList(Of Integer, ItemAttribData) = StaticData.GetAttributeDataForItem(item.Id)
                    For Each att As Integer In atts.Keys
                        Select Case att
                            Case 1112
                                newDecryptor.ProbMod = atts(att).Value
                            Case 1113
                                newDecryptor.MEMod = CInt(atts(att).Value)
                            Case 1114
                                newDecryptor.PEMod = CInt(atts(att).Value)
                            Case 1124
                                newDecryptor.RunMod = CInt(atts(att).Value)
                        End Select
                    Next
                Next
            Next

            Return True

        End Function

        Public Shared Function CalculateInventionChance(ByVal baseChance As Double, ByVal encSkillLevel As Integer, ByVal dc1SkillLevel As Integer, ByVal dc2SkillLevel As Integer, ByVal metaLevel As Integer, ByVal decryptorModifier As Double) As Double
            Return baseChance * (1 + (0.01 * encSkillLevel)) * (1 + ((dc1SkillLevel + dc2SkillLevel) * (0.1 / (5 - metaLevel)))) * DecryptorModifier
        End Function

    End Class
End Namespace