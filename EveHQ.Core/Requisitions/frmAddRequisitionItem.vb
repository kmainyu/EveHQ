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
Imports System.Windows.Forms

Public Class frmAddRequisitionItem

#Region "Property Variables"
    Dim cSource As String = ""
    Dim cItemID As String = ""
    Dim cItemName As String = ""
    Dim cItemQuantity As Integer = 0
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
            Return cSource
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
            Return cItemID
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
            Return cItemName
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
            Return cItemQuantity
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Initialises a new Requsition Form
    ''' </summary>
    ''' <param name="SourceFeature">A string containing the name of the EveHQ feature or plug-in that will be used to identify where the requisition originated from</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal SourceFeature As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call Me.LoadItems()

        ' Set the current source feature and quantity
        cSource = SourceFeature
        lblSource.Text = SourceFeature
        cItemQuantity = 1

    End Sub

    ''' <summary>
    ''' Loads the names of the Eve items into the combobox
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadItems()
        ' Load the items
        cboItems.BeginUpdate()
        cboItems.Items.Clear()
        For Each newItem As EveHQ.Core.EveItem In EveHQ.Core.HQ.itemData.Values
            If newItem.Published = True Then
                cboItems.Items.Add(newItem.Name)
            End If
        Next
        cboItems.Sorted = True
        cboItems.EndUpdate()
    End Sub

    Private Sub nudQuantity_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudQuantity.ValueChanged
        cItemQuantity = nudQuantity.Value
    End Sub

    Private Sub cboItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboItems.SelectedIndexChanged
        cItemName = cboItems.SelectedItem.ToString
        cItemID = EveHQ.Core.HQ.itemList(cboItems.SelectedItem.ToString)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Set the dialog result, then close
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check for a valid item
        If cboItems.SelectedItem Is Nothing Then
            MessageBox.Show("You must select an item before continuing. Please try again.", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Set the dialog result, then close
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class