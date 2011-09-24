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

Public Class Invention

    Public Shared Function LoadInventionData() As Boolean

        ' Load all the decryptor data
        Dim strSQL As String = "SELECT invTypes.groupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueFloat, invTypes.typeName, invTypes.typeID"
        strSQL &= " FROM dgmTypeAttributes INNER JOIN"
        strSQL &= " invTypes ON dgmTypeAttributes.typeID = invTypes.typeID"
        strSQL &= " WHERE (invTypes.groupID IN (728, 729, 730, 731))"
        Dim InvData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        If InvData IsNot Nothing Then
            If InvData.Tables(0).Rows.Count > 0 Then
                PlugInData.Decryptors.Clear()
                For Each InvRow As DataRow In InvData.Tables(0).Rows
                    If PlugInData.Decryptors.ContainsKey(InvRow.Item("typeName").ToString) = False Then
                        Dim NewDecryptor As New Decryptor
                        NewDecryptor.ID = InvRow.Item("typeID").ToString
                        NewDecryptor.Name = InvRow.Item("typeName").ToString
                        NewDecryptor.GroupID = InvRow.Item("groupID").ToString
                        PlugInData.Decryptors.Add(NewDecryptor.Name, NewDecryptor)
                    End If
                    Dim CurrentDecryptor As Decryptor = PlugInData.Decryptors(InvRow.Item("typeName").ToString)
                    Select Case InvRow.Item("attributeID").ToString
                        Case "1112"
                            CurrentDecryptor.ProbMod = CDbl(InvRow.Item("valueFloat"))
                        Case "1113"
                            CurrentDecryptor.MEMod = CInt(InvRow.Item("valueFloat"))
                        Case "1114"
                            CurrentDecryptor.PEMod = CInt(InvRow.Item("valueFloat"))
                        Case "1124"
                            CurrentDecryptor.RunMod = CInt(InvRow.Item("valueFloat"))
                    End Select
                Next
            Else
                MessageBox.Show("Decryptor Data returned no valid rows.", "Prism Invention Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Else
            MessageBox.Show("Decryptor Data returned a null dataset.", "Prism Invention Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' Load all the meta level data for invention
        strSQL = "SELECT invBlueprintTypes.blueprintTypeID, invMetaTypes.typeID, invMetaTypes.parentTypeID FROM invBlueprintTypes INNER JOIN"
        strSQL &= " invMetaTypes ON invBlueprintTypes.productTypeID = invMetaTypes.parentTypeID"
        strSQL &= " WHERE (techLevel = 1)"
        strSQL &= " ORDER BY parentTypeID ;"
        InvData = EveHQ.Core.DataFunctions.GetData(strSQL)
        If InvData IsNot Nothing Then
            If InvData.Tables(0).Rows.Count > 0 Then
                For Each InvRow As DataRow In InvData.Tables(0).Rows
                    Dim CurrentBP As Blueprint = PlugInData.Blueprints(InvRow.Item("blueprintTypeID").ToString)
                    If CurrentBP.InventionMetaItems.ContainsKey(InvRow.Item("parentTypeID").ToString) = False Then
                        CurrentBP.InventionMetaItems.Add(InvRow.Item("parentTypeID").ToString, InvRow.Item("parentTypeID").ToString)
                    End If
                    If EveHQ.Core.HQ.itemData(InvRow.Item("typeID").ToString).MetaLevel < 5 Then
                        CurrentBP.InventionMetaItems.Add(InvRow.Item("typeID").ToString, InvRow.Item("typeID").ToString)
                    End If
                Next
            Else
                MessageBox.Show("Invention Meta Data returned no valid rows.", "Prism Invention Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Else
            MessageBox.Show("Invention Meta Data returned a null dataset.", "Prism Invention Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Return True

    End Function

    Public Shared Function CalculateInventionChance(ByVal BaseChance As Double, ByVal EncSkillLevel As Integer, ByVal DC1SkillLevel As Integer, ByVal DC2SkillLevel As Integer, ByVal MetaLevel As Integer, ByVal DecryptorModifier As Double) As Double
        Return BaseChance * (1 + (0.01 * EncSkillLevel)) * (1 + ((DC1SkillLevel + DC2SkillLevel) * (0.1 / (5 - MetaLevel)))) * DecryptorModifier
    End Function
End Class

