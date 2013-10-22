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
Imports EveHQ.EveData

Namespace Forms

    Public Class FrmSelectItem

        Private _item As String

        Public ReadOnly Property Item() As String
            Get
                Return _item
            End Get
        End Property

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            If cboItems.SelectedItem IsNot Nothing Then
                _item = cboItems.SelectedItem.ToString
            End If
            Close()
        End Sub

        Private Sub frmSelectItem_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Load recyclable items
            cboItems.BeginUpdate()
            cboItems.Items.Clear()
            For Each bp As EveData.Blueprint In StaticData.Blueprints.Values
                If bp.Resources.ContainsKey(6) Then
                    cboItems.AutoCompleteCustomSource.Add(StaticData.Types(bp.ProductId).Name)
                    cboItems.Items.Add(StaticData.Types(bp.ProductId).Name)
                End If
            Next
            cboItems.EndUpdate()

        End Sub
    End Class
End NameSpace