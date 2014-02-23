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

Imports DevComponents.AdvTree
Imports DevComponents.Editors
Imports System.Windows.Forms

Namespace Requisitions

    Public Class FrmEditRequisition

        Dim _newReq As Requisition
        Dim _cRequisition As Requisition
        ReadOnly _newReqFlag As Boolean = True
        Dim _currentMultiplier As Integer = 1

        Public ReadOnly Property Requisition() As Requisition
            Get
                Return _cRequisition
            End Get
        End Property

        ''' <summary>
        ''' Form loading routine
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub EditRequisition_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Load the pilots and corps
            Call LoadRequestors()

            ' Update the Requistion orders
            Call RefreshRequisition()

        End Sub

        ''' <summary>
        ''' Loads the active pilots and corps into the combo box on opening the form
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadRequestors()
            cboRequestor.BeginUpdate()
            cboRequestor.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
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
            txtReqName.Text = _newReq.Name
            cboRequestor.SelectedItem = _newReq.Requestor
            adtOrders.BeginUpdate()
            adtOrders.Nodes.Clear()
            For Each newOrder As RequisitionOrder In _newReq.Orders.Values
                Dim tn As New Node
                tn.Text = newOrder.ItemName
                tn.Name = newOrder.ItemName
                Dim tc As New Cell
                Dim ii As New IntegerInput
                ii.Name = newOrder.ItemName
                ii.ShowUpDown = True
                ii.Value = newOrder.ItemQuantity
                AddHandler ii.ValueChanged, AddressOf ItemQuantityChanged
                tc.HostedControl = ii
                tn.Cells.Add(tc)
                Dim tc2 As New Cell
                tc2.Text = newOrder.Source
                tn.Cells.Add(tc2)
                Dim tc3 As New Cell
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
        Private Sub ItemQuantityChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim intInput As IntegerInput = CType(sender, IntegerInput)
            _newReq.Orders(intInput.Name).ItemQuantity = intInput.Value
        End Sub

        ''' <summary>
        ''' Adds a new item order to the requisition
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnAddItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddItem.Click
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
            Using newOrder As New FrmAddRequisitionItem("Requisitions")
                newOrder.ShowDialog()
                If newOrder.DialogResult = DialogResult.OK Then
                    ' Check if this is an existing requisition
                    If _newReq.Orders.ContainsKey(newOrder.ItemName) = False Then
                        ' Create a new order
                        Dim newReqOrder As New RequisitionOrder
                        newReqOrder.ID = "-1" ' Will be replaced by the database ID on reloading from the DB
                        newReqOrder.ItemID = newOrder.ItemID
                        newReqOrder.ItemName = newOrder.ItemName
                        newReqOrder.ItemQuantity = newOrder.ItemQuantity
                        newReqOrder.RequestDate = Now
                        newReqOrder.Source = newOrder.Source
                        _newReq.Orders.Add(newReqOrder.ItemName, newReqOrder)
                    Else
                        ' Update the existing order
                        _newReq.Orders(newOrder.ItemName).ItemQuantity += newOrder.ItemQuantity
                    End If
                    Call RefreshRequisition()
                End If
            End Using
        End Sub

        Private Sub btnRemoveItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemoveItem.Click
            If adtOrders.Nodes.Count > 0 Then
                If adtOrders.SelectedNodes.Count > 0 Then
                    ' Remove all the selected items
                    For Each selNode As Node In adtOrders.SelectedNodes
                        _newReq.Orders.Remove(selNode.Text)
                    Next
                    ' Update the list
                    Call RefreshRequisition()
                Else
                    MessageBox.Show("Please select items to remove before selecting this option.", "Selected Items Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("You have no items to remove - try adding some first!", "No Items Listed", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

        Private Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
            ' Check if the name has changed and see if the new name already exists
            Dim currentReqs As SortedList(Of String, Requisition) = CustomDataFunctions.PopulateRequisitions("", "", "", "")
            If _cRequisition.Name <> _newReq.Name And currentReqs.ContainsKey(_newReq.Name) = True Then
                MessageBox.Show("The requisition name already exists. Please select a new name", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            ' Remove the old requisition if it exists
            If currentReqs.ContainsKey(_cRequisition.Name) = True Then
                currentReqs.Remove(_cRequisition.Name)
            End If
            ' Add the new requisition
            currentReqs.Add(_newReq.Name, _newReq)
            ' See if this is a new requisition and we need to write it to the database
            If _newReqFlag = True Then
                Call _newReq.WriteToDatabase()
            Else
                ' Send across the old requisition for a comparison of the orders and requisition data
                Call _newReq.UpdateDatabase(_cRequisition)
            End If
            ' Set the property variable to the new requistion in case we want to access this after the form has closed
            _cRequisition = CType(_newReq.Clone, Requisition)
            ' Set the form result and close
            DialogResult = DialogResult.OK
            Close()
        End Sub

        Private Sub btnRevert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRevert.Click
            ' Revert the requisition back to it's initial state
            _newReq = CType(_cRequisition.Clone, Requisition)
            Call RefreshRequisition()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ' Set the form result and close (retains the value of the default requisition)
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        ''' <summary>
        ''' Method for creating a new Requisition Edit form. Use this method for editing an existing Requisition
        ''' </summary>
        ''' <param name="defaultRequisition">An instance of the Requisition class that needs to be edited. Adding a requisition should pass a new instance of the class.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal defaultRequisition As Requisition)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Set the default requisition in case we need to revert to it later
            _cRequisition = DefaultRequisition
            Text = "Edit Requisition"
            _newReqFlag = False

            ' Set up the temp requisition based on the default
            _newReq = CType(_cRequisition.Clone, Requisition)

        End Sub

        ''' <summary>
        '''  Method for creating a new Requisition Edit form. Use this method for creating a new Requisition
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _cRequisition = New Requisition
            Text = "Add New Requisition"
            _newReqFlag = True

            ' Set up the temp requisition based on the default
            _newReq = CType(_cRequisition.Clone, Requisition)

        End Sub

        Private Sub txtReqName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtReqName.TextChanged
            _newReq.Name = txtReqName.Text
        End Sub

        Private Sub cboRequestor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboRequestor.SelectedIndexChanged
            _newReq.Requestor = cboRequestor.SelectedItem.ToString
        End Sub

        Private Sub nudMultiplier_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudMultiplier.ValueChanged
            Dim oldValue As Integer = _currentMultiplier
            Dim newValue As Integer = nudMultiplier.Value
            For idx As Integer = 0 To adtOrders.Nodes.Count - 1
                Dim oldQ As Integer = CType(adtOrders.Nodes(idx).Cells(1).HostedControl, IntegerInput).Value
                CType(adtOrders.Nodes(idx).Cells(1).HostedControl, IntegerInput).Value = CInt(oldQ / oldValue * newValue)
            Next
            _currentMultiplier = nudMultiplier.Value
        End Sub
    End Class
End Namespace