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

Imports System.Windows.Forms
Imports EveHQ.EveData

Namespace Requisitions

    Public Class FrmAddRequisition

        Dim _currentReqs As New SortedList(Of String, Requisition)
        Dim _currentMultiplier As Integer = 1

#Region "Property Variables"
        Dim _cRequisition As Requisition
        ReadOnly _cSource As String = ""
        ReadOnly _cOrders As New SortedList(Of String, Integer)
#End Region

#Region "Public Properties"

        Public ReadOnly Property Requisition() As Requisition
            Get
                Return _cRequisition
            End Get
        End Property

#End Region

        ''' <summary>
        ''' Initialises a new Requsition Form with no Requisition Orders
        ''' </summary>
        ''' <param name="sourceFeature">A string containing the name of the EveHQ feature or plug-in that will be used to identify where the requisition originated from</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sourceFeature As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Get requisitions
            Call LoadRequisitions()
            Call LoadRequestors()

            ' Set the current source feature and quantity
            _cSource = sourceFeature
            lblSource.Text = sourceFeature

        End Sub

        ''' <summary>
        ''' Initialises a new Requsition Form with Requisition Orders
        ''' </summary>
        ''' <param name="sourceFeature">A string containing the name of the EveHQ feature or plug-in that will be used to identify where the requisition originated from</param>
        ''' <param name="orders">A list of item names and their quantities to enter into the requisition</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sourceFeature As String, ByVal orders As SortedList(Of String, Integer))

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Get requisitions
            Call LoadRequisitions()
            Call LoadRequestors()

            ' Set the current source feature and quantity
            _cSource = sourceFeature
            _cOrders = Orders
            lblSource.Text = sourceFeature

            ' Add the orders to the list
            Call ShowOrders()

        End Sub

        ''' <summary>
        ''' Initialises a new Requsition Form with Requisition Orders
        ''' </summary>
        ''' <param name="defaultName">A default name to use for the requisition</param>
        ''' <param name="sourceFeature">A string containing the name of the EveHQ feature or plug-in that will be used to identify where the requisition originated from</param>
        ''' <param name="orders">A list of item names and their quantities to enter into the requisition</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal defaultName As String, ByVal sourceFeature As String, ByVal orders As SortedList(Of String, Integer))

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Get requisitions
            Call LoadRequisitions()
            Call LoadRequestors()

            ' Set the current source feature and quantity
            _cSource = sourceFeature
            _cOrders = orders
            lblSource.Text = sourceFeature
            cboReqName.Text = defaultName

            ' Add the orders to the list
            Call ShowOrders()
        End Sub

        ''' <summary>
        ''' Loads the requistions and populates the requisitions combo
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadRequisitions()
            ' Get requisitions
            _currentReqs = CustomDataFunctions.PopulateRequisitions("", "", "", "")
            cboReqName.BeginUpdate()
            cboReqName.Items.Clear()
            For Each cReq As String In _currentReqs.Keys
                cboReqName.Items.Add(cReq)
            Next
            cboReqName.EndUpdate()
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
        ''' Shows the contents of the orders in the listview
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ShowOrders()
            lvwOrders.BeginUpdate()
            lvwOrders.Items.Clear()
            For Each itemName As String In _cOrders.Keys
                Dim newOrder As New ListViewItem
                newOrder.Name = itemName
                newOrder.Text = itemName
                newOrder.SubItems.Add(_cOrders(itemName).ToString("N0"))
                lvwOrders.Items.Add(newOrder)
            Next
            lvwOrders.EndUpdate()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ' Set the dialog result, then close
            _cRequisition = Nothing
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Check for a valid name
            If cboReqName.Text = "" Then
                MessageBox.Show("You must select a valid requisition name before continuing. Please try again.", "Requisition Name Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Check for requestor
            If cboRequestor.SelectedItem Is Nothing Then
                MessageBox.Show("You must select a requestor before continuing. Please try again.", "Requestor Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Check for duplicate names
            If _currentReqs.ContainsKey(cboReqName.Text) = True Then
                Dim reply As DialogResult = MessageBox.Show("There is already a Requisition with this name. Would you like to add this to the existing Requisition?", "Edit Existing Requisition?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Select Case reply
                    Case DialogResult.Cancel
                        Exit Sub
                    Case DialogResult.No
                        Exit Sub
                    Case DialogResult.Yes
                        ' Get the existing requisition
                        _cRequisition = _currentReqs(cboReqName.Text)
                        ' Clone ready for update
                        Dim nRequisition As Requisition = CType(_cRequisition.Clone, Requisition)
                        ' Add the new orders to the requisition
                        nRequisition.AddOrders(_cSource, _cOrders)
                        ' Update the database with the changes
                        nRequisition.UpdateDatabase(_cRequisition)
                End Select
            Else
                ' Create the new requisition
                _cRequisition = New Requisition(cboReqName.Text, cboRequestor.SelectedItem.ToString, _cSource)
                For Each newItem As String In _cOrders.Keys
                    Dim newOrder As New RequisitionOrder()
                    newOrder.ItemID = CStr(StaticData.TypeNames(newItem))
                    newOrder.ItemName = newItem
                    newOrder.ItemQuantity = _cOrders(newItem)
                    newOrder.Source = _cSource
                    newOrder.RequestDate = Now
                    _cRequisition.Orders.Add(newOrder.ItemName, newOrder)
                Next
                ' Add the new requisition
                _currentReqs.Add(_cRequisition.Name, _cRequisition)
                ' Write it to the database
                Call _cRequisition.WriteToDatabase()
            End If

            ' Set the form result and close
            DialogResult = DialogResult.OK
            Close()

        End Sub

        Private Sub nudMultiplier_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudMultiplier.ValueChanged
            Dim oldValue As Integer = _currentMultiplier
            Dim newValue As Integer = nudMultiplier.Value
            For cItem As Integer = 0 To _cOrders.Count - 1
                _cOrders(_cOrders.Keys(cItem)) = CInt(_cOrders(_cOrders.Keys(cItem)) / oldValue * newValue)
            Next
            _currentMultiplier = nudMultiplier.Value
            Call ShowOrders()
        End Sub

    End Class
End Namespace