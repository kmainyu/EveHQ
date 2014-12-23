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
            For Each tm As Integer In StaticData.TypeMaterials.Keys
                If StaticData.Types.ContainsKey(tm) Then
                    cboItems.AutoCompleteCustomSource.Add(StaticData.Types(tm).Name)
                    cboItems.Items.Add(StaticData.Types(tm).Name)
                End If
            Next
            cboItems.Sorted = True
            cboItems.EndUpdate()

        End Sub

        Private Sub FrmSelectItem_Shown(sender As Object, e As EventArgs) Handles Me.Shown
            cboItems.Focus()
        End Sub

    End Class
End NameSpace