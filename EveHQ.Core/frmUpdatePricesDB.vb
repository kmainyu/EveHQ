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
Imports System.Data.SqlServerCe
Imports System.Data.SqlClient
Imports System.Text
Imports System.Windows.Forms

Public Class frmUpdatePricesDB

    Dim WithEvents FormWorker As New System.ComponentModel.BackgroundWorker

    Private Sub frmShipComparisonWorker_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Refresh()
        FormWorker.WorkerReportsProgress = True
        FormWorker.RunWorkerAsync()
    End Sub

    Private Sub CompareWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles FormWorker.DoWork
        Call Me.CheckForPricesTables()
    End Sub

    Private Sub CompareWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles FormWorker.RunWorkerCompleted
        Me.Close()
    End Sub

    Private Sub FormWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles FormWorker.ProgressChanged
        Me.lblStatus.Text = e.UserState.ToString
        Me.Refresh()
    End Sub

#Region "Pricing Data Routines"

    Private Function CheckForPricesTables() As Boolean

        Dim CreateTables As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("priceLists") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTables = True
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
                CreateTables = True
            End If
        End If

        ' If we need to create the tables then call the required function and return the result
        If CreateTables = True Then
            Return CreateNewPricesTables()
        End If

    End Function

    Private Function CreateNewPricesTables() As Boolean
        ' We need to create new tables, but also need to see if we need to import from the old DB tables

        'Step 1: Create the priceLists table
        FormWorker.ReportProgress(100, "Stage 1/6: Creating Price List table...")
        Dim strSQL As New StringBuilder
        strSQL.AppendLine("CREATE TABLE priceLists")
        strSQL.AppendLine("(")
        strSQL.AppendLine("  priceListID    bigint IDENTITY(1,1),") ' Autonumber for this entry
        strSQL.AppendLine("  name           nvarchar(50),")
        strSQL.AppendLine("  description    nvarchar(250),")
        strSQL.AppendLine("")
        strSQL.AppendLine("  CONSTRAINT priceLists_PK PRIMARY KEY (priceListID)")
        strSQL.AppendLine(")")
        If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = -2 Then
            MessageBox.Show("There was an error creating the Price Lists database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        'Step 2: Create the prices table
        FormWorker.ReportProgress(100, "Stage 2/6: Creating Prices table...")
        strSQL = New StringBuilder
        strSQL.AppendLine("CREATE TABLE prices")
        strSQL.AppendLine("(")
        strSQL.AppendLine("  priceListID    bigint,")
        strSQL.AppendLine("  typeID         bigint,")
        strSQL.AppendLine("  price          float,")
        strSQL.AppendLine("  priceDate      datetime,")
        strSQL.AppendLine("")
        strSQL.AppendLine("  CONSTRAINT prices_PK PRIMARY KEY (priceListID, typeID)")
        strSQL.AppendLine(")")
        If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = -2 Then
            MessageBox.Show("There was an error creating the Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        ' Step 3: Create the old Market Prices price list
        FormWorker.ReportProgress(100, "Stage 3/6: Creating Market Price List...")
        Dim priceListSQL As String = "INSERT INTO priceLists (name, description) VALUES ('EveHQ Market Prices','Old EveHQ Market Prices data');"
        If EveHQ.Core.DataFunctions.SetData(priceListSQL) = -2 Then
            MessageBox.Show("There was an error writing the Market price list to the Price List database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Writing Market Price List", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        ' Step 4: Create the old Market Prices price list
        FormWorker.ReportProgress(100, "Stage 4/6: Creating Custom Price List...")
        priceListSQL = "INSERT INTO priceLists (name, description) VALUES ('EveHQ Custom Prices','EveHQ Custom Prices data');"
        If EveHQ.Core.DataFunctions.SetData(priceListSQL) = -2 Then
            MessageBox.Show("There was an error writing the Custom price list to the Price List database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Writing Custom Price List", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        ' These following steps will depend on the existence of the market and custom price database tables, so we'll need to check them

        ' Step 5: Import the old market price database table
        FormWorker.ReportProgress(100, "Stage 5/6: Importing Market Prices...")
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("marketPrices") = True Then
                Dim priceData As DataSet = EveHQ.Core.DataFunctions.GetCustomData("SELECT * FROM marketPrices ORDER BY typeID;")
                ' Check the data
                If priceData IsNot Nothing Then
                    If priceData.Tables(0).Rows.Count > 0 Then
                        ' Get the ID of the marketprice table in case it's not straightforward
                        Dim ID As Long = CLng(EveHQ.Core.DataFunctions.GetCustomData("SELECT * FROM priceLists WHERE name='EveHQ Market Prices';").Tables(0).Rows(0).Item("priceListID"))
                        If WriteOldPricesToNewTable(ID, priceData) = False Then
                            MessageBox.Show("There was an error writing the Market Prices to the new database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Writing New Market Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return False
                        End If
                    End If
                    ' Delete the old market prices table
                    'Dim deleteSQL As String = "DROP TABLE marketPrices;"
                    'If EveHQ.Core.DataFunctions.SetData(deleteSQL) = False Then
                    '    MessageBox.Show("There was an error deleting the Market Prices table from the database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Deleting Market Price Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    '    Return False
                    'End If
                End If
            End If
        Else
            ' We should have a big problem here because the database doesn't exist and our previous code is a cruel lying bastard!
            ' But return a message anyway and quit to the calling method
            MessageBox.Show("There was an error finding the custom database! The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Accessing Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        ' Step 6: Import the old custom price database table
        FormWorker.ReportProgress(100, "Stage 6/6: Importing Custom Prices...")
        tables = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("customPrices") = True Then
                Dim priceData As DataSet = EveHQ.Core.DataFunctions.GetCustomData("SELECT * FROM customPrices ORDER BY typeID;")
                ' Check the data
                If priceData IsNot Nothing Then
                    If priceData.Tables(0).Rows.Count > 0 Then
                        ' Get the ID of the marketprice table in case it's not straightforward
                        Dim ID As Long = CLng(EveHQ.Core.DataFunctions.GetCustomData("SELECT * FROM priceLists WHERE name='EveHQ Custom Prices';").Tables(0).Rows(0).Item("priceListID"))
                        If WriteOldPricesToNewTable(ID, priceData) = False Then
                            MessageBox.Show("There was an error writing the Custom Prices to the new database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Writing New Custom Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return False
                        End If
                    End If
                    ' Delete the old market prices table
                    Dim deleteSQL As String = "DROP TABLE customPrices;"
                    'If EveHQ.Core.DataFunctions.SetData(deleteSQL) = False Then
                    '    MessageBox.Show("There was an error deleting the Custom Prices table from the database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Deleting Custom Price Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    '    Return False
                    'End If
                End If
            End If
        Else
            ' We should have a big problem here because the database doesn't exist and our previous code is a cruel lying bastard!
            ' But return a message anyway and quit to the calling method
            MessageBox.Show("There was an error finding the custom database! The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Accessing Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

    End Function

    Private Function WriteOldPricesToNewTable(ByVal PriceListID As Long, ByVal PriceListData As DataSet) As Boolean

        Select Case EveHQ.Core.HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()

                    Dim ds As New DataSet
                    Dim da As New SqlCeDataAdapter("SELECT * FROM prices;", conn)
                    Dim cb As New SqlCeCommandBuilder(da)

                    da.Fill(ds, "prices")

                    For Each PriceRow As DataRow In PriceListData.Tables(0).Rows
                        Dim rec As DataRow = ds.Tables(0).NewRow
                        rec.Item("priceListID") = PriceListID
                        rec.Item("typeID") = CLng(PriceRow.Item("typeID"))
                        rec.Item("price") = CDbl(PriceRow.Item("price"))
                        rec.Item("priceDate") = CDate(PriceRow.Item("priceDate"))
                        ds.Tables("prices").Rows.Add(rec)
                    Next

                    cb.GetInsertCommand()
                    da.Update(ds, "prices")

                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()

                    Dim ds As New DataSet
                    Dim da As New SqlDataAdapter("SELECT * FROM prices;", conn)
                    Dim cb As New SqlCommandBuilder(da)

                    da.Fill(ds, "prices")

                    For Each PriceRow As DataRow In PriceListData.Tables(0).Rows
                        Dim rec As DataRow = ds.Tables(0).NewRow
                        rec.Item("priceListID") = PriceListID
                        rec.Item("typeID") = CLng(PriceRow.Item("typeID"))
                        rec.Item("price") = CDbl(PriceRow.Item("price"))
                        rec.Item("priceDate") = CDate(PriceRow.Item("priceDate"))
                        ds.Tables("prices").Rows.Add(rec)
                    Next

                    cb.GetInsertCommand()
                    da.Update(ds, "prices")

                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case Else
                EveHQ.Core.HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select

    End Function


#End Region

End Class

