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

Public Class frmEditRequisition

    Dim newReq As Requisition
    Dim cRequisition As Requisition
    Dim newReqFlag As Boolean = True
    Dim CurrentMultiplier As Integer = 1

    Public ReadOnly Property Requisition() As Requisition
        Get
            Return cRequisition
        End Get
    End Property

    ''' <summary>
    ''' Form loading routine
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EditRequisition_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Load the pilots and corps
        Call Me.LoadRequestors()

        ' Update the Requistion orders
        Call Me.RefreshRequisition()

    End Sub

    ''' <summary>
    ''' Loads the active pilots and corps into the combo box on opening the form
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadRequestors()
        cboRequestor.BeginUpdate()
        cboRequestor.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHqSettings.Pilots
            If cPilot.Active = True Then
                cboRequestor.Items.Add(cPilot.Name)
                If cboRequestor.Items.Contains(cPilot.Corp) = False Then
                    cboRequestor.Items.Add(cPilot.Corp)
                End If
            End If
        Next
        cboRequestor.EndUpdate()
    End Sub

    ''' <summary>
    ''' Updates the list of items in the current Requisition
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshRequisition()
        txtReqName.Text = newReq.Name
        cboRequestor.SelectedItem = newReq.Requestor
        adtOrders.BeginUpdate()
        adtOrders.Nodes.Clear()
        For Each newOrder As RequisitionOrder In newReq.Orders.Values
            Dim tn As New DevComponents.AdvTree.Node
            tn.Text = newOrder.ItemName
            tn.Name = newOrder.ItemName
            Dim tc As New DevComponents.AdvTree.Cell
            Dim ii As New DevComponents.Editors.IntegerInput
            ii.Name = newOrder.ItemName
            ii.ShowUpDown = True
            ii.Value = newOrder.ItemQuantity
            AddHandler ii.ValueChanged, AddressOf Me.ItemQuantityChanged
            tc.HostedControl = ii
            tn.Cells.Add(tc)
            Dim tc2 As New DevComponents.AdvTree.Cell
            tc2.Text = newOrder.Source
            tn.Cells.Add(tc2)
            Dim tc3 As New DevComponents.AdvTree.Cell
            tc3.Text = newOrder.RequestDate.ToString
            tn.Cells.Add(tc3)
            adtOrders.Nodes.Add(tn)
        Next
        adtOrders.EndUpdate()
    End Sub

    ''' <summary>
    ''' Sets the new item quantity when an integer input control changes value
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemQuantityChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myII As DevComponents.Editors.IntegerInput = CType(sender, DevComponents.Editors.IntegerInput)
        newReq.Orders(myII.Name).ItemQuantity = myII.Value
    End Sub

    ''' <summary>
    ''' Adds a new item order to the requisition
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        ' Check for a valid Requistion Name first
        If txtReqName.Text = "" Then
            MessageBox.Show("You must enter a valid Requisition Name before adding any orders.", "Requisition Name Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Check for a selected requestor
        If cboRequestor.SelectedItem Is Nothing Then
            MessageBox.Show("You must enter a valid Requestor before adding any orders.", "Requestor Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim newOrder As New frmAddRequisitionItem("Requisitions")
        newOrder.ShowDialog()
        If newOrder.DialogResult = Windows.Forms.DialogResult.OK Then
            ' Check if this is an existing requisition
            If newReq.Orders.ContainsKey(newOrder.ItemName) = False Then
                ' Create a new order
                Dim newReqOrder As New RequisitionOrder
                newReqOrder.ID = "-1" ' Will be replaced by the database ID on reloading from the DB
                newReqOrder.ItemID = newOrder.ItemID
                newReqOrder.ItemName = newOrder.ItemName
                newReqOrder.ItemQuantity = newOrder.ItemQuantity
                newReqOrder.RequestDate = Now
                newReqOrder.Source = newOrder.Source
                newReq.Orders.Add(newReqOrder.ItemName, newReqOrder)
            Else
                ' Update the existing order
                newReq.Orders(newOrder.ItemName).ItemQuantity += newOrder.ItemQuantity
            End If
            Call Me.RefreshRequisition()
        End If
        newOrder.Dispose()
    End Sub

    Private Sub btnRemoveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveItem.Click
        If adtOrders.Nodes.Count > 0 Then
            If adtOrders.SelectedNodes.Count > 0 Then
                ' Remove all the selected items
                For Each selNode As DevComponents.AdvTree.Node In adtOrders.SelectedNodes
                    newReq.Orders.Remove(selNode.Text)
                Next
                ' Update the list
                Call Me.RefreshRequisition()
            Else
                MessageBox.Show("Please select items to remove before selecting this option.", "Selected Items Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("You have no items to remove - try adding some first!", "No Items Listed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        ' Check if the name has changed and see if the new name already exists
        Dim CurrentReqs As SortedList(Of String, EveHQ.Core.Requisition) = EveHQ.Core.RequisitionDataFunctions.PopulateRequisitions("", "", "", "")
        If cRequisition.Name <> newReq.Name And CurrentReqs.ContainsKey(newReq.Name) = True Then
            MessageBox.Show("The requisition name already exists. Please select a new name", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Remove the old requisition if it exists
        If CurrentReqs.ContainsKey(cRequisition.Name) = True Then
            CurrentReqs.Remove(cRequisition.Name)
        End If
        ' Add the new requisition
        CurrentReqs.Add(newReq.Name, newReq)
        ' See if this is a new requisition and we need to write it to the database
        If newReqFlag = True Then
            Call newReq.WriteToDatabase()
        Else
            ' Send across the old requisition for a comparison of the orders and requisition data
            Call newReq.UpdateDatabase(cRequisition)
        End If
        ' Set the property variable to the new requistion in case we want to access this after the form has closed
        cRequisition = CType(newReq.Clone, Core.Requisition)
        ' Set the form result and close
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnRevert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevert.Click
        ' Revert the requisition back to it's initial state
        newReq = CType(cRequisition.Clone, Core.Requisition)
        Call Me.RefreshRequisition()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Set the form result and close (retains the value of the default requisition)
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    ''' <summary>
    ''' Method for creating a new Requisition Edit form. Use this method for editing an existing Requisition
    ''' </summary>
    ''' <param name="DefaultRequisition">An instance of the Requisition class that needs to be edited. Adding a requisition should pass a new instance of the class.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal DefaultRequisition As Requisition)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the default requisition in case we need to revert to it later
        cRequisition = DefaultRequisition
        Me.Text = "Edit Requisition"
        newReqFlag = False

        ' Set up the temp requisition based on the default
        newReq = CType(cRequisition.Clone, Core.Requisition)

    End Sub

    ''' <summary>
    '''  Method for creating a new Requisition Edit form. Use this method for creating a new Requisition
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cRequisition = New Requisition
        Me.Text = "Add New Requisition"
        newReqFlag = True

        ' Set up the temp requisition based on the default
        newReq = CType(cRequisition.Clone, Core.Requisition)

    End Sub

    Private Sub txtReqName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReqName.TextChanged
        newReq.Name = txtReqName.Text
    End Sub

    Private Sub cboRequestor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRequestor.SelectedIndexChanged
        newReq.Requestor = cboRequestor.SelectedItem.ToString
    End Sub

    Private Sub nudMultiplier_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMultiplier.ValueChanged
        Dim OldValue As Integer = CurrentMultiplier
        Dim NewValue As Integer = nudMultiplier.Value
        For idx As Integer = 0 To adtOrders.Nodes.Count - 1
            Dim OldQ As Integer = CType(adtOrders.Nodes(idx).Cells(1).HostedControl, DevComponents.Editors.IntegerInput).Value
            CType(adtOrders.Nodes(idx).Cells(1).HostedControl, DevComponents.Editors.IntegerInput).Value = CInt(OldQ / OldValue * NewValue)
        Next
        CurrentMultiplier = nudMultiplier.Value
    End Sub
End Class