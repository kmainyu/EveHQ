' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports EveHQ.EveData

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
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Check for a valid item
            If cboItems.SelectedItem Is Nothing Then
                MessageBox.Show("You must select an item before continuing. Please try again.", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            ' Set the dialog result, then close
            DialogResult = Windows.Forms.DialogResult.OK
            Close()
        End Sub
    End Class
End Namespace