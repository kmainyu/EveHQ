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

Imports System.Globalization
Imports EveHQ.EveData
Imports System.Windows.Forms
Imports System.Text

Namespace Requisitions

    ''' <summary>
    ''' Storage class for a single Requisition (shopping list)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Requisition
        Implements ICloneable

        Dim _cName As String = "" ' Key for Requisitions
        Dim _cRequestor As String = "" ' Pilot/Corp
        Dim _cSource As String = "" ' Original Source
        Dim _cOrders As New SortedList(Of String, RequisitionOrder) ' Collection of items

        Private Const SQLTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        ReadOnly _culture As CultureInfo = New CultureInfo("en-GB")

        ''' <summary>
        ''' Holds the name of the Requisition
        ''' </summary>
        ''' <value></value>
        ''' <returns>The name of the requisition</returns>
        ''' <remarks>This is the key to be used in the Requisitions class</remarks>
        Public Property Name() As String
            Get
                Return _cName
            End Get
            Set(ByVal value As String)
                _cName = value
            End Set
        End Property

        ''' <summary>
        ''' Holds the character or corporation name of the entity making the request
        ''' </summary>
        ''' <value></value>
        ''' <returns>The name of the entity making the requisition</returns>
        ''' <remarks></remarks>
        Public Property Requestor() As String
            Get
                Return _cRequestor
            End Get
            Set(ByVal value As String)
                _cRequestor = value
            End Set
        End Property

        ''' <summary>
        ''' Holds the name of the EveHQ feature or plug-in where the requisition was created
        ''' </summary>
        ''' <value></value>
        ''' <returns>The name of the EveHQ feature or plug-in where the requisition was created</returns>
        ''' <remarks></remarks>
        Public Property Source() As String
            Get
                Return _cSource
            End Get
            Set(ByVal value As String)
                _cSource = value
            End Set
        End Property

        ''' <summary>
        ''' Holds a collection of Requisition Orders (items) that make up this Requisition
        ''' </summary>
        ''' <value></value>
        ''' <returns>A collection of individual orders</returns>
        ''' <remarks></remarks>
        Public Property Orders() As SortedList(Of String, RequisitionOrder)
            Get
                Return _cOrders
            End Get
            Set(ByVal value As SortedList(Of String, RequisitionOrder))
                _cOrders = value
            End Set
        End Property

        ''' <summary>
        ''' Method for cloning a requisition 
        ''' </summary>
        ''' <returns>A copy of the instance of Requisition from where the function was called</returns>
        ''' <remarks></remarks>
        Public Function Clone() As Object Implements ICloneable.Clone
            Dim clonedItem As New Requisition(Name, Requestor, Source)
            ' Update orders
            clonedItem.Orders.Clear()
            For Each reqOrder As RequisitionOrder In Orders.Values
                clonedItem.Orders.Add(reqOrder.ItemName, CType(reqOrder.Clone, RequisitionOrder))
            Next
            Return clonedItem
        End Function

        ''' <summary>
        ''' Creates a new Requisition
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            ' This sub intentionally left blank!
        End Sub

        ''' <summary>
        ''' Creates a new Requisition
        ''' </summary>
        ''' <param name="Name">The name of the new Requisition</param>
        ''' <param name="Requestor">The Character or Corporation assigned to the Requisition</param>
        ''' <param name="Source">The EveHQ feature or plug-in creating the Requisition</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal name As String, ByVal requestor As String, ByVal source As String)
            _cName = name
            _cRequestor = requestor
            _cSource = source
        End Sub

        ''' <summary>
        ''' Adds a collection of orders to an existing Requisition
        ''' </summary>
        ''' <param name="eveHQSource">The source of the changes to the Requisition</param>
        ''' <param name="orderList">A SortedList(itemName, Quantity) of orders to add</param>
        ''' <remarks></remarks>
        Public Sub AddOrders(ByVal eveHQSource As String, ByVal orderList As SortedList(Of String, Integer))
            For Each itemName As String In orderList.Keys
                ' Check if this is an existing requisition
                If Orders.ContainsKey(itemName) = False Then
                    ' Create a new order
                    Dim newReqOrder As New RequisitionOrder
                    newReqOrder.ID = "-1" ' Will be replaced by the database ID on reloading from the DB
                    newReqOrder.ItemID = CStr(StaticData.TypeNames(itemName))
                    newReqOrder.ItemName = itemName
                    newReqOrder.ItemQuantity = orderList(itemName)
                    newReqOrder.RequestDate = Now
                    newReqOrder.Source = EveHQSource
                    Orders.Add(newReqOrder.ItemName, newReqOrder)
                Else
                    ' Update the existing order
                    Orders(itemName).ItemQuantity += orderList(itemName)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Adds a collection of orders to an existing Requisition
        ''' </summary>
        ''' <param name="orderList">A SortedList(of String, RequisitionOrder) of orders to add</param>
        ''' <remarks></remarks>
        Public Sub AddOrdersFromRequisition(ByVal orderList As SortedList(Of String, RequisitionOrder))
            For Each oldOrder As RequisitionOrder In orderList.Values
                ' Check if this is an existing requisition
                If Orders.ContainsKey(oldOrder.ItemName) = False Then
                    ' Create a new order
                    Dim newReqOrder As New RequisitionOrder
                    newReqOrder.ID = "-1" ' Will be replaced by the database ID on reloading from the DB
                    newReqOrder.ItemID = oldOrder.ItemID
                    newReqOrder.ItemName = oldOrder.ItemName
                    newReqOrder.ItemQuantity = oldOrder.ItemQuantity
                    newReqOrder.RequestDate = oldOrder.RequestDate
                    newReqOrder.Source = oldOrder.Source
                    Orders.Add(newReqOrder.ItemName, newReqOrder)
                Else
                    ' Update the existing order
                    Orders(oldOrder.ItemName).ItemQuantity += oldOrder.ItemQuantity
                End If
            Next
        End Sub

        ''' <summary>
        ''' Writes the current Requisition to the database.
        ''' This is the routine for writing a new requisition and should only be used once for every requisition.
        ''' </summary>
        ''' <returns>A boolean value stating whether the operation was successful</returns>
        ''' <remarks></remarks>
        Public Function WriteToDatabase() As Boolean
            ' Only update if the orders > 0
            If _cOrders.Count > 0 Then
                For Each newOrder As RequisitionOrder In _cOrders.Values
                    Dim strSQL As String = BuildInsertSQLFromOrder(newOrder)
                    ' Attempt to write the new requisition
                    If CustomDataFunctions.SetCustomData(strSQL) = -2 Then
                        MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End If
                Next
            End If
            Return True
        End Function

        ''' <summary>
        ''' Deletes the current Requisition from the database
        ''' </summary>
        ''' <returns>A boolean value stating whether the operation was successful</returns>
        ''' <remarks></remarks>
        Public Function DeleteFromDatabase() As Boolean
            ' Remove the entire requisition and order
            Dim strSQL As String = "DELETE FROM requisitions WHERE requisition='" & Name.Replace("'", "''") & "';"
            ' Attempt to delete the requisitions
            If CustomDataFunctions.SetCustomData(strSQL) = -2 Then
                MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQRequisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Updates the current Requisition in the database with any changed information
        ''' </summary>
        ''' <param name="oldRequisition">The old Requisition data used as a comparison for the update</param>
        ''' <returns>A boolean value stating whether the operation was successful</returns>
        ''' <remarks></remarks>
        Public Function UpdateDatabase(ByVal oldRequisition As Requisition) As Boolean

            ' Step 1: Check for deleted items - this will be those in the original but not in the revised
            Dim oldOrderList As New StringBuilder
            For Each orderID As String In oldRequisition.Orders.Keys
                If Orders.ContainsKey(orderID) = False Then
                    oldOrderList.Append("," & oldRequisition.Orders(orderID).ID)
                End If
            Next
            If oldOrderList.Length > 1 Then
                ' Trim the excess text
                oldOrderList = oldOrderList.Remove(0, 1)
                ' Build the SQL
                Dim strSQL As String = "DELETE FROM requisitions WHERE orderID IN (" & oldOrderList.ToString & ");"
                ' Attempt to delete the requisitions
                If CustomDataFunctions.SetCustomData(strSQL) = -2 Then
                    MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            End If

            ' Step 2: Update what is left in the list - this will be any orderID which is <> -1
            For Each newOrder As RequisitionOrder In _cOrders.Values
                If newOrder.ID <> "-1" Then
                    Dim strSQL As String = BuildUpdateSQLFromOrder(Me, newOrder)
                    ' Attempt to write the new requisition order
                    If CustomDataFunctions.SetCustomData(strSQL) = -2 Then
                        MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End If
                End If
            Next

            ' Step 3: Check for new items added - this will be any orderID which is -1
            For Each newOrder As RequisitionOrder In _cOrders.Values
                If newOrder.ID = "-1" Then
                    Dim strSQL As String = BuildInsertSQLFromOrder(newOrder)
                    ' Attempt to write the new requisition
                    If CustomDataFunctions.SetCustomData(strSQL) = -2 Then
                        MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End If
                End If
            Next

            Return True

        End Function

        ''' <summary>
        ''' Builds a SQL string that can be used to write a new Requisition Order to the database
        ''' </summary>
        ''' <param name="newOrder">The Requisition to be written to the database</param>
        ''' <returns>A string containing the SQL to perform the database transaction</returns>
        ''' <remarks></remarks>
        Private Function BuildInsertSQLFromOrder(ByVal newOrder As RequisitionOrder) As String
            Dim strSQL As New StringBuilder("INSERT INTO requisitions (itemID, itemName, itemQuantity, source, requestor, requestDate, requisition) VALUES (")
            strSQL.Append(newOrder.ItemID & ",")
            strSQL.Append("'" & newOrder.ItemName.Replace("'", "''") & "',")
            strSQL.Append(newOrder.ItemQuantity & ",")
            strSQL.Append("'" & newOrder.Source.Replace("'", "''") & "',")
            strSQL.Append("'" & Requestor.Replace("'", "''") & "',")
            strSQL.Append("'" & newOrder.RequestDate.ToString(SQLTimeFormat, _culture) & "', ")
            strSQL.Append("'" & Name.Replace("'", "''") & "'")
            strSQL.Append(");")
            Return strSQL.ToString
        End Function

        ''' <summary>
        ''' Builds a SQL string that can be used to update a Requisition Order in the database
        ''' </summary>
        ''' <param name="updateReq">The new Requisition we are updating to</param>
        ''' <param name="updateOrder">The specific order we need to update</param>
        ''' <returns>A string containing the SQL to perform the database transaction</returns>
        ''' <remarks></remarks>
        Private Function BuildUpdateSQLFromOrder(ByVal updateReq As Requisition, ByVal updateOrder As RequisitionOrder) As String
            Dim strSQL As New StringBuilder("UPDATE requisitions SET ")
            strSQL.Append("itemQuantity=" & updateOrder.ItemQuantity & ", ")
            strSQL.Append("source='" & updateOrder.Source.Replace("'", "''") & "', ")
            strSQL.Append("requestor='" & updateReq.Requestor.Replace("'", "''") & "', ")
            strSQL.Append("requestDate='" & updateOrder.RequestDate.ToString(SQLTimeFormat, _culture) & "', ")
            strSQL.Append("requisition='" & updateReq.Name.Replace("'", "''") & "' ")
            strSQL.Append("WHERE orderID=" & updateOrder.ID & ";")
            Return strSQL.ToString
        End Function

    End Class
End Namespace