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

Public Class frmAddRequisition

    Dim CurrentReqs As New SortedList(Of String, EveHQ.Core.Requisition)
    Dim CurrentMultiplier As Integer = 1

#Region "Property Variables"
    Dim cRequisition As Requisition
    Dim cSource As String = ""
    Dim cOrders As New SortedList(Of String, Integer)
#End Region

#Region "Public Properties"

    Public ReadOnly Property Requisition() As Requisition
        Get
            Return cRequisition
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Initialises a new Requsition Form with no Requisition Orders
    ''' </summary>
    ''' <param name="SourceFeature">A string containing the name of the EveHQ feature or plug-in that will be used to identify where the requisition originated from</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal SourceFeature As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Get requisitions
        Call Me.LoadRequisitions()
        Call Me.LoadRequestors()

        ' Set the current source feature and quantity
        cSource = SourceFeature
        lblSource.Text = SourceFeature

    End Sub

    ''' <summary>
    ''' Initialises a new Requsition Form with Requisition Orders
    ''' </summary>
    ''' <param name="SourceFeature">A string containing the name of the EveHQ feature or plug-in that will be used to identify where the requisition originated from</param>
    ''' <param name="Orders">A list of item names and their quantities to enter into the requisition</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal SourceFeature As String, ByVal Orders As SortedList(Of String, Integer))

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Get requisitions
        Call Me.LoadRequisitions()
        Call Me.LoadRequestors()

        ' Set the current source feature and quantity
        cSource = SourceFeature
        cOrders = Orders
        lblSource.Text = SourceFeature

        ' Add the orders to the list
        Call Me.ShowOrders()

	End Sub

	''' <summary>
	''' Initialises a new Requsition Form with Requisition Orders
	''' </summary>
	''' <param name="DefaultName">A default name to use for the requisition</param>
	''' <param name="SourceFeature">A string containing the name of the EveHQ feature or plug-in that will be used to identify where the requisition originated from</param>
	''' <param name="Orders">A list of item names and their quantities to enter into the requisition</param>
	''' <remarks></remarks>
	Public Sub New(ByVal DefaultName As String, ByVal SourceFeature As String, ByVal Orders As SortedList(Of String, Integer))

		' This call is required by the Windows Form Designer.
		InitializeComponent()

		' Get requisitions
		Call Me.LoadRequisitions()
		Call Me.LoadRequestors()

		' Set the current source feature and quantity
		cSource = SourceFeature
		cOrders = Orders
		lblSource.Text = SourceFeature
		cboReqName.Text = DefaultName

		' Add the orders to the list
		Call Me.ShowOrders()
	End Sub

	''' <summary>
	''' Loads the requistions and populates the requisitions combo
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadRequisitions()
		' Get requisitions
		CurrentReqs = EveHQ.Core.RequisitionDataFunctions.PopulateRequisitions("", "", "", "")
		cboReqName.BeginUpdate()
		cboReqName.Items.Clear()
		For Each cReq As String In CurrentReqs.Keys
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
        For Each cPilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
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
		For Each itemName As String In cOrders.Keys
			Dim newOrder As New ListViewItem
			newOrder.Name = itemName
			newOrder.Text = itemName
			newOrder.SubItems.Add(cOrders(itemName).ToString("N0"))
			lvwOrders.Items.Add(newOrder)
		Next
		lvwOrders.EndUpdate()
	End Sub

	Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
		' Set the dialog result, then close
		cRequisition = Nothing
		Me.DialogResult = Windows.Forms.DialogResult.Cancel
		Me.Close()
	End Sub

	Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
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
		If CurrentReqs.ContainsKey(cboReqName.Text) = True Then
			Dim reply As DialogResult = MessageBox.Show("There is already a Requisition with this name. Would you like to add this to the existing Requisition?", "Edit Existing Requisition?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			Select Case reply
				Case Windows.Forms.DialogResult.Cancel
					Exit Sub
				Case Windows.Forms.DialogResult.No
					Exit Sub
				Case Windows.Forms.DialogResult.Yes
					' Get the existing requisition
					cRequisition = CurrentReqs(cboReqName.Text)
					' Clone ready for update
					Dim nRequisition As EveHQ.Core.Requisition = CType(cRequisition.Clone, Core.Requisition)
					' Add the new orders to the requisition
					nRequisition.AddOrders(cSource, cOrders)
					' Update the database with the changes
					nRequisition.UpdateDatabase(cRequisition)
			End Select
		Else
			' Create the new requisition
			cRequisition = New Requisition(cboReqName.Text, cboRequestor.SelectedItem.ToString, cSource)
			For Each newItem As String In cOrders.Keys
				Dim newOrder As New RequisitionOrder()
				newOrder.ItemID = EveHQ.Core.HQ.itemList(newItem)
				newOrder.ItemName = newItem
				newOrder.ItemQuantity = cOrders(newItem)
				newOrder.Source = cSource
				newOrder.RequestDate = Now
				cRequisition.Orders.Add(newOrder.ItemName, newOrder)
			Next
			' Add the new requisition
			CurrentReqs.Add(cRequisition.Name, cRequisition)
			' Write it to the database
			Call cRequisition.WriteToDatabase()
		End If

		' Set the form result and close
		Me.DialogResult = Windows.Forms.DialogResult.OK
		Me.Close()

	End Sub

    Private Sub nudMultiplier_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMultiplier.ValueChanged
        Dim OldValue As Integer = CurrentMultiplier
        Dim NewValue As Integer = nudMultiplier.Value
        For cItem As Integer = 0 To cOrders.Count - 1
            cOrders(cOrders.Keys(cItem)) = CInt(cOrders(cOrders.Keys(cItem)) / OldValue * NewValue)
        Next
        CurrentMultiplier = nudMultiplier.Value
        Call ShowOrders()
    End Sub

End Class