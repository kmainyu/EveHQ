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
Imports System.Data
Imports DevComponents.AdvTree

Public Class frmModifyPriceGroupItem

	Dim ItemMarketGroups As New SortedList(Of String, String)
    Dim ParentMarketGroup As New SortedList(Of String, String) ' marketGroupID, parentGroupID
    Dim AdtPreparedItems As New AdvTree
    Dim cSelectedItems As New SortedList(Of String, String) ' typeName, typeID

    Public ReadOnly Property SelectedItems As SortedList(Of String, String)
        Get
            Return cSelectedItems
        End Get
    End Property

#Region "Form Constructors"
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ItemTree As AdvTree, ByVal ItemList As SortedList(Of String, String))
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AdtPreparedItems = ItemTree
        ItemMarketGroups = ItemList

    End Sub

#End Region

#Region "Form Loading Routines"

    Private Sub frmModifyPriceGroupItem_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.PopulateTree()
    End Sub

    Private Sub PopulateTree()
        adtItems.BeginUpdate()
        adtItems.Nodes.Clear()
        For Each MGNode As Node In AdtPreparedItems.Nodes
            adtItems.Nodes.Add(MGNode.DeepCopy)
        Next
        adtItems.EndUpdate()
    End Sub

#End Region

#Region "Form UI Methods"

    Private Sub adtItems_SelectionChanged(sender As Object, e As System.EventArgs) Handles adtItems.SelectionChanged
        Select Case adtItems.SelectedNodes.Count
            Case 0
                btnAddGroup.Text = "Selection Required"
                btnAddGroup.Enabled = False
            Case 1
                If adtItems.SelectedNodes(0).HasChildNodes = False Then
                    ' Item is selected
                    btnAddGroup.Text = "Add " & adtItems.SelectedNodes(0).Text
                Else
                    ' Group is selected
                    btnAddGroup.Text = "Add '" & adtItems.SelectedNodes(0).Text & "' Group"
                End If
                btnAddGroup.Enabled = True
            Case Else
                If adtItems.SelectedNodes(0).HasChildNodes = False Then
                    ' Item is selected
                    btnAddGroup.Text = "Add Mulitple Items"
                Else
                    ' Group is selected
                    btnAddGroup.Text = "Add Multiple Groups"
                End If
                btnAddGroup.Enabled = True
        End Select
    End Sub

    Private Sub adtSelection_SelectionChanged(sender As Object, e As System.EventArgs) Handles adtSelection.SelectionChanged
        Select Case adtSelection.SelectedNodes.Count
            Case 0
                btnDelete.Enabled = False
            Case 1
                btnDelete.Enabled = True
            Case Else
                btnDelete.Enabled = True
        End Select
    End Sub

    Private Sub btnAddGroup_Click(sender As System.Object, e As System.EventArgs) Handles btnAddGroup.Click
        ' Iterate through the entire group and add the items
        For Each ANode As Node In adtItems.SelectedNodes
            Call AddNodeToSelection(ANode)
        Next
        Call Me.UpdateSelectedItems()
    End Sub

    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        ' Delete all highlighted items
        For Each RNode As Node In adtSelection.SelectedNodes
            cSelectedItems.Remove(RNode.Text)
        Next
        Call Me.UpdateSelectedItems()
    End Sub

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        ' Confirm the clearing of the current items
        Dim reply As DialogResult = MessageBox.Show("Are you sure you want to clear all items?", "Confirm Clear Items", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            cSelectedItems.Clear()
            Call Me.UpdateSelectedItems()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        ' Set the result and close the form
        cSelectedItems = Nothing
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(sender As System.Object, e As System.EventArgs) Handles btnAccept.Click
        ' Set the result and close the form
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub UpdateSelectedItems()
        adtSelection.BeginUpdate()
        adtSelection.Nodes.Clear()
        For Each ItemID As String In cSelectedItems.Values
            Dim item As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(ItemID)
            Dim itemNode As New DevComponents.AdvTree.Node
            itemNode.Text = item.Name
            itemNode.Name = item.ID.ToString
            ' Check if the item has been added to another price group, and hence is a conflict
            itemNode.Image = My.Resources.Tick
            For Each PG As EveHQ.Core.PriceGroup In EveHQ.Core.HQ.Settings.PriceGroups.Values
                If PG.TypeIDs.Contains(item.ID.ToString) Then
                    itemNode.Image = My.Resources.Cross
                    Exit For
                End If
            Next
            'If item.Icon <> "" Then
            '    itemNode.Image = EveHQ.Core.ImageHandler.GetImage(item.Icon, EveHQ.Core.ImageHandler.ImageType.Icons, 24)
            'Else
            '    itemNode.Image = EveHQ.Core.ImageHandler.GetImage(item.ID.ToString, EveHQ.Core.ImageHandler.ImageType.Types, 24)
            'End If
            adtSelection.Nodes.Add(itemNode)
        Next
        adtSelection.EndUpdate()
    End Sub

    Private Sub AddNodeToSelection(ANode As Node)
        If ANode.HasChildNodes = True Then
            For Each SNode As Node In ANode.Nodes
                AddNodeToSelection(SNode)
            Next
        Else
            ' No child nodes therefore must be an item - but only add ones we don't have!
            If cSelectedItems.ContainsKey(ANode.Text) = False And ANode.Name.StartsWith("i") Then
                cSelectedItems.Add(ANode.Text, ANode.Name.Remove(0, 1))
            End If
        End If
    End Sub

#End Region


End Class
