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

Imports EveHQ.EveData

Namespace Classes

    Public Class Invention

        Public Shared Function LoadInventionData() As Boolean

            ' Load all the decryptor data
            Dim groupIDs As List(Of Integer) = {1304}.ToList
            PlugInData.Decryptors.Clear()
            For Each groupId As Integer In groupIDs
                ' Get the items in each group
                Dim items As IEnumerable(Of EveType) = StaticData.GetItemsInGroup(groupId)
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

        Public Shared Function CalculateInventionChance(ByVal baseChance As Double, ByVal encryptionSkill As Integer, ByVal datacoreSkill1 As Integer, ByVal datacoreSkill2 As Integer, ByVal decryptorMod As Double) As Double
            'Return baseChance * (1 + (0.01 * encSkillLevel)) * (1 + ((dc1SkillLevel + dc2SkillLevel) * (0.1 / (5 - metaLevel)))) * decryptorModifier
            Return baseChance * 100 * (1 + (((datacoreSkill1 + datacoreSkill2) / 30) + (encryptionSkill / 40))) * decryptorMod
        End Function

    End Class

End Namespace