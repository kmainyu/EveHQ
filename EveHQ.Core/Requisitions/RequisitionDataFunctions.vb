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
Imports System.Text

''' <summary>
''' Class for holding shared data functions not specific to any particular requisition or order
''' </summary>
''' <remarks></remarks>
Public Class RequisitionDataFunctions

    ''' <summary>
    ''' Function to check for the existence of the Requisitions table. The table will be created if it is missing from the custom database.
    ''' Returns a boolean based on whether the table exists.
    ''' </summary>
    ''' <returns>A boolean value indicating whether the table exists.</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckForRequisitionsTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("requisitions") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTable = True
            Else
                ' We have the DB and table so we can return a good result
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE [requisitions](")
            strSQL.AppendLine("[orderID] [int] IDENTITY(1,1) NOT NULL,")
            strSQL.AppendLine("[itemID] [int] NOT NULL,")
            strSQL.AppendLine("[itemName] [nvarchar](200) NOT NULL,")
            strSQL.AppendLine("[itemQuantity] [int] NOT NULL,")
            strSQL.AppendLine("[source] [nvarchar](50) NOT NULL,")
            strSQL.AppendLine("[requestor] [nvarchar](50) NOT NULL,")
            strSQL.AppendLine("[requestDate] [datetime] NOT NULL,")
            strSQL.AppendLine("[requisition] [nvarchar](50) NOT NULL,")
            strSQL.AppendLine("CONSTRAINT [PK_requisitions] PRIMARY KEY ([orderID]) );")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) <> -2 Then
                ' Create the indexes
                strSQL = New StringBuilder
                EveHQ.Core.DataFunctions.SetData("CREATE INDEX idxReqSource ON requisitions (source);")
                EveHQ.Core.DataFunctions.SetData("CREATE INDEX idxReqRequestor ON requisitions (requestor);")
                EveHQ.Core.DataFunctions.SetData("CREATE INDEX idxReqName ON requisitions (requisition);")
            Else
                MessageBox.Show("There was an error creating the Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Requisitions Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
    End Function

    Public Shared Function PopulateRequisitions(ByVal SearchString As String, ByVal Requisition As String, ByVal Source As String, ByVal Requestor As String) As SortedList(Of String, EveHQ.Core.Requisition)
        ' Build the SQL string
        Dim strSQL As New StringBuilder
        strSQL.Append("SELECT * FROM requisitions")
        strSQL.Append(" WHERE (itemName LIKE '%" & SearchString.Replace("'", "''") & "%'")
        If Requisition <> "" Then
            strSQL.Append(" AND requisition='" & Requisition.Replace("'", "''") & "'")
        End If
        If Source <> "" Then
            strSQL.Append(" AND source='" & Source.Replace("'", "''") & "'")
        End If
        If Requestor <> "" Then
            strSQL.Append(" AND requestor='" & Requestor.Replace("'", "''") & "'")
        End If
        strSQL.Append(");")
        Dim reqData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL.ToString)
        Dim Reqs As New SortedList(Of String, EveHQ.Core.Requisition)
        If reqData IsNot Nothing Then
            If reqData.Tables(0).Rows.Count > 0 Then
                ' Populate the requisitions
                For Each reqRow As DataRow In reqData.Tables(0).Rows
                    Dim reqName As String = reqRow.Item("requisition").ToString
                    ' Check if the requisition exists
                    Dim newReq As New EveHQ.Core.Requisition
                    If Reqs.ContainsKey(reqName) Then
                        newReq = Reqs(reqName)
                    Else
                        newReq.Name = reqRow.Item("requisition").ToString
                        newReq.Requestor = reqRow.Item("requestor").ToString
                        newReq.Source = reqRow.Item("source").ToString
                        Reqs.Add(newReq.Name, newReq)
                    End If
                    ' Add the order
                    Dim newOrder As New EveHQ.Core.RequisitionOrder
                    newOrder.ID = reqRow.Item("orderID").ToString
                    newOrder.ItemID = reqRow.Item("itemID").ToString
                    newOrder.ItemName = reqRow.Item("itemName").ToString
                    newOrder.ItemQuantity = CInt(reqRow.Item("itemQuantity"))
                    newOrder.Source = reqRow.Item("source").ToString
                    newOrder.RequestDate = CDate(reqRow.Item("requestDate"))
                    If newReq.Orders.ContainsKey(newOrder.ItemName) = False Then
                        newReq.Orders.Add(newOrder.ItemName, newOrder)
                    End If
                Next
                Return Reqs
            Else
                Return Reqs
            End If
        Else
            Return Reqs
        End If
    End Function

    Public Shared Function CountRequisitions() As Long

        ' Check the table exists first
        CheckForRequisitionsTable()

        ' Return the number of available requisitions
        Dim strSQL As String = "SELECT * FROM requisitions;"
        Dim reqData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL.ToString)
        If reqData IsNot Nothing Then
            Return reqData.Tables(0).Rows.Count
        Else
            Return 0
        End If

    End Function

End Class
