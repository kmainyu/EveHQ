' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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

Imports System.Text
Imports System.Windows.Forms

''' <summary>
''' Storage class for a single Requisition (shopping list)
''' </summary>
''' <remarks></remarks>
Public Class Requisition
    Implements ICloneable

    Dim cName As String = "" ' Key for Requisitions
    Dim cRequestor As String = "" ' Pilot/Corp
    Dim cSource As String = "" ' Original Source
    Dim cOrders As New SortedList(Of String, RequisitionOrder) ' Collection of items

    Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim SQLTimeFormat As String = "yyyyMMdd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    ''' <summary>
    ''' Holds the name of the Requisition
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the requisition</returns>
    ''' <remarks>This is the key to be used in the Requisitions class</remarks>
    Public Property Name() As String
        Get
            Return cName
        End Get
        Set(ByVal value As String)
            cName = value
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
            Return cRequestor
        End Get
        Set(ByVal value As String)
            cRequestor = value
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
            Return cSource
        End Get
        Set(ByVal value As String)
            cSource = value
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
            Return cOrders
        End Get
        Set(ByVal value As SortedList(Of String, RequisitionOrder))
            cOrders = value
        End Set
    End Property

    ''' <summary>
    ''' Method for cloning a requisition 
    ''' </summary>
    ''' <returns>A copy of the instance of EveHQ.Core.Requisition from where the function was called</returns>
    ''' <remarks></remarks>
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim ClonedItem As New Requisition(Me.Name, Me.Requestor, Me.Source)
        ' Update orders
        ClonedItem.Orders.Clear()
        For Each ReqOrder As EveHQ.Core.RequisitionOrder In Me.Orders.Values
            ClonedItem.Orders.Add(ReqOrder.ItemName, CType(ReqOrder.Clone, RequisitionOrder))
        Next
        Return ClonedItem
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
    Public Sub New(ByVal Name As String, ByVal Requestor As String, ByVal Source As String)
        cName = Name
        cRequestor = Requestor
        cSource = Source
    End Sub

    ''' <summary>
    ''' Adds a collection of orders to an existing Requisition
    ''' </summary>
    ''' <param name="Source">The source of the changes to the Requisition</param>
    ''' <param name="OrderList">A SortedList(itemName, Quantity) of orders to add</param>
    ''' <remarks></remarks>
    Public Sub AddOrders(ByVal Source As String, ByVal OrderList As SortedList(Of String, Integer))
        For Each ItemName As String In OrderList.Keys
            ' Check if this is an existing requisition
            If Me.Orders.ContainsKey(ItemName) = False Then
                ' Create a new order
                Dim newReqOrder As New RequisitionOrder
                newReqOrder.ID = "-1" ' Will be replaced by the database ID on reloading from the DB
                newReqOrder.ItemID = EveHQ.Core.HQ.itemList(ItemName)
                newReqOrder.ItemName = ItemName
                newReqOrder.ItemQuantity = OrderList(ItemName)
                newReqOrder.RequestDate = Now
                newReqOrder.Source = Source
                Me.Orders.Add(newReqOrder.ItemName, newReqOrder)
            Else
                ' Update the existing order
                Me.Orders(ItemName).ItemQuantity += OrderList(ItemName)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Adds a collection of orders to an existing Requisition
    ''' </summary>
    ''' <param name="OrderList">A SortedList(of String, RequisitionOrder) of orders to add</param>
    ''' <remarks></remarks>
    Public Sub AddOrdersFromRequisition(ByVal OrderList As SortedList(Of String, EveHQ.Core.RequisitionOrder))
        For Each OldOrder As EveHQ.Core.RequisitionOrder In OrderList.Values
            ' Check if this is an existing requisition
            If Me.Orders.ContainsKey(OldOrder.ItemName) = False Then
                ' Create a new order
                Dim newReqOrder As New RequisitionOrder
                newReqOrder.ID = "-1" ' Will be replaced by the database ID on reloading from the DB
                newReqOrder.ItemID = OldOrder.ItemID
                newReqOrder.ItemName = OldOrder.ItemName
                newReqOrder.ItemQuantity = OldOrder.ItemQuantity
                newReqOrder.RequestDate = OldOrder.RequestDate
                newReqOrder.Source = OldOrder.Source
                Me.Orders.Add(newReqOrder.ItemName, newReqOrder)
            Else
                ' Update the existing order
                Me.Orders(OldOrder.ItemName).ItemQuantity += OldOrder.ItemQuantity
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
        If Me.cOrders.Count > 0 Then
            For Each newOrder As EveHQ.Core.RequisitionOrder In Me.cOrders.Values
                Dim strSQL As String = BuildInsertSQLFromOrder(newOrder)
                ' Attempt to write the new requisition
                If EveHQ.Core.DataFunctions.SetData(strSQL) = False Then
                    MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
        Dim strSQL As String = "DELETE FROM requisitions WHERE requisition='" & Me.Name.Replace("'", "''") & "';"
        ' Attempt to delete the requisitions
        If EveHQ.Core.DataFunctions.SetData(strSQL) = False Then
            MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Updates the current Requisition in the database with any changed information
    ''' </summary>
    ''' <param name="OldRequisition">The old Requisition data used as a comparison for the update</param>
    ''' <returns>A boolean value stating whether the operation was successful</returns>
    ''' <remarks></remarks>
    Public Function UpdateDatabase(ByVal OldRequisition As EveHQ.Core.Requisition) As Boolean

        ' Step 1: Check for deleted items - this will be those in the original but not in the revised
        Dim OldOrderList As New StringBuilder
        For Each OrderID As String In OldRequisition.Orders.Keys
            If Me.Orders.ContainsKey(OrderID) = False Then
                OldOrderList.Append("," & OldRequisition.Orders(OrderID).ID)
            End If
        Next
        If OldOrderList.Length > 1 Then
            ' Trim the excess text
            OldOrderList = OldOrderList.Remove(0, 1)
            ' Build the SQL
            Dim strSQL As String = "DELETE FROM requisitions WHERE orderID IN (" & OldOrderList.ToString & ");"
            ' Attempt to delete the requisitions
            If EveHQ.Core.DataFunctions.SetData(strSQL) = False Then
                MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

        ' Step 2: Update what is left in the list - this will be any orderID which is <> -1
        For Each newOrder As EveHQ.Core.RequisitionOrder In Me.cOrders.Values
            If newOrder.ID <> "-1" Then
                Dim strSQL As String = BuildUpdateSQLFromOrder(Me, newOrder)
                ' Attempt to write the new requisition order
                If EveHQ.Core.DataFunctions.SetData(strSQL) = False Then
                    MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            End If
        Next

        ' Step 3: Check for new items added - this will be any orderID which is -1
        For Each newOrder As EveHQ.Core.RequisitionOrder In Me.cOrders.Values
            If newOrder.ID = "-1" Then
                Dim strSQL As String = BuildInsertSQLFromOrder(newOrder)
                ' Attempt to write the new requisition
                If EveHQ.Core.DataFunctions.SetData(strSQL) = False Then
                    MessageBox.Show("There was an error writing data to the EveHQ Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL.ToString, "Error Writing EveHQ Requisition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            End If
        Next

    End Function

    ''' <summary>
    ''' Builds a SQL string that can be used to write a new Requisition Order to the database
    ''' </summary>
    ''' <param name="newOrder">The Requisition to be written to the database</param>
    ''' <returns>A string containing the SQL to perform the database transaction</returns>
    ''' <remarks></remarks>
    Private Function BuildInsertSQLFromOrder(ByVal newOrder As EveHQ.Core.RequisitionOrder) As String
        Dim strSQL As New StringBuilder("INSERT INTO requisitions (itemID, itemName, itemQuantity, source, requestor, requestDate, requisition) VALUES (")
        strSQL.Append(newOrder.ItemID & ",")
        strSQL.Append("'" & newOrder.ItemName.Replace("'", "''") & "',")
        strSQL.Append(newOrder.ItemQuantity & ",")
        strSQL.Append("'" & newOrder.Source.Replace("'", "''") & "',")
        strSQL.Append("'" & Me.Requestor.Replace("'", "''") & "',")
        strSQL.Append("'" & newOrder.RequestDate.ToString(SQLTimeFormat, culture) & "', ")
        strSQL.Append("'" & Me.Name.Replace("'", "''") & "'")
        strSQL.Append(");")
        Return strSQL.ToString
    End Function

    ''' <summary>
    ''' Builds a SQL string that can be used to update a Requisition Order in the database
    ''' </summary>
    ''' <param name="UpdateReq">The new Requisition we are updating to</param>
    ''' <param name="UpdateOrder">The specific order we need to update</param>
    ''' <returns>A string containing the SQL to perform the database transaction</returns>
    ''' <remarks></remarks>
    Private Function BuildUpdateSQLFromOrder(ByVal UpdateReq As EveHQ.Core.Requisition, ByVal UpdateOrder As EveHQ.Core.RequisitionOrder) As String
        Dim strSQL As New StringBuilder("UPDATE requisitions SET ")
        strSQL.Append("itemQuantity=" & UpdateOrder.ItemQuantity & ", ")
        strSQL.Append("source='" & UpdateOrder.Source.Replace("'", "''") & "', ")
        strSQL.Append("requestor='" & UpdateReq.Requestor.Replace("'", "''") & "', ")
        strSQL.Append("requestDate='" & UpdateOrder.RequestDate.ToString(SQLTimeFormat, culture) & "', ")
        strSQL.Append("requisition='" & UpdateReq.Name.Replace("'", "''") & "' ")
        strSQL.Append("WHERE orderID=" & UpdateOrder.ID & ";")
        Return strSQL.ToString
    End Function

End Class
