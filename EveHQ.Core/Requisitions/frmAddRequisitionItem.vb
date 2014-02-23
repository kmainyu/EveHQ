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
Imports System.Windows.Forms

Namespace Requisitions

    Public Class FrmAddRequisitionItem

#Region "Property Variables"
        ReadOnly _cSource As String = ""
        Dim _cItemID As String = ""
        Dim _cItemName As String = ""
        Dim _cItemQuantity As Integer = 0
#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Gets the source of the requisition
        ''' </summary>
        ''' <value></value>
        ''' <returns>The source of the requisition</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Source() As String
            Get
                Return _cSource
            End Get
        End Property

        ''' <summary>
        ''' Gets the current selected Eve typeID of the item in the requisition form
        ''' </summary>
        ''' <value></value>
        ''' <returns>The ID of the current selected item</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ItemID() As String
            Get
                Return _cItemID
            End Get
        End Property

        ''' <summary>
        ''' Gets the current selected Eve typeName of the item in the requisition form
        ''' </summary>
        ''' <value></value>
        ''' <returns>The Name of the current selected item</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ItemName() As String
            Get
                Return _cItemName
            End Get
        End Property

        ''' <summary>
        ''' Gets the quantity of the selected item
        ''' </summary>
        ''' <value></value>
        ''' <returns>The current item quantity</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ItemQuantity() As Integer
            Get
                Return _cItemQuantity
            End Get
        End Property

#End Region

        ''' <summary>
        ''' Initialises a new Requsition Form
        ''' </summary>
        ''' <param name="sourceFeature">A string containing the name of the EveHQ feature or plug-in that will be used to identify where the requisition originated from</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sourceFeature As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Call LoadItems()

            ' Set the current source feature and quantity
            _cSource = sourceFeature
            lblSource.Text = sourceFeature
            _cItemQuantity = 1

        End Sub

        ''' <summary>
        ''' Loads the names of the Eve items into the combobox
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadItems()
            ' Load the items
            cboItems.BeginUpdate()
            cboItems.Items.Clear()
            For Each newItem As EveType In StaticData.Types.Values
                If newItem.Published = True Then
                    cboItems.Items.Add(newItem.Name)
                End If
            Next
            cboItems.Sorted = True
            cboItems.EndUpdate()
        End Sub

        Private Sub nudQuantity_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudQuantity.ValueChanged
            _cItemQuantity = nudQuantity.Value
        End Sub

        Private Sub cboItems_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboItems.SelectedIndexChanged
            _cItemName = cboItems.SelectedItem.ToString
            _cItemID = CStr(StaticData.TypeNames(cboItems.SelectedItem.ToString))
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ' Set the dialog result, then close
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Check for a valid item
            If cboItems.SelectedItem Is Nothing Then
                MessageBox.Show("You must select an item before continuing. Please try again.", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            ' Set the dialog result, then close
            DialogResult = DialogResult.OK
            Close()
        End Sub
    End Class
End Namespace