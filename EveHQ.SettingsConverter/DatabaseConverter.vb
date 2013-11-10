' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports System.ComponentModel
Imports EveHQ.Prism.Classes
Imports EveHQ.Core
Imports System.Data.SqlServerCe
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.IO

''' <summary>
''' Converts the custom database in version 2.12.3 to an equivalent in SQLite.
''' </summary>
''' <remarks></remarks>
Public Class DatabaseConverter

    Dim _sqlConn As String = ""
    Dim _sqLiteConn As String = ""
    ReadOnly _settingsFolder As String

    ReadOnly _sqLiteFile As String

    ReadOnly _dbFormat As DBFormat
    ReadOnly _dbFileName As String = ""
    ReadOnly _dbServerName As String = ""
    ReadOnly _dbDatabase As String = ""
    ReadOnly _dbSQLSec As Boolean = False
    ReadOnly _dbUsername As String = ""
    ReadOnly _dbPassword As String = ""

    ReadOnly _worker As BackgroundWorker

    ''' <summary>
    ''' Create a new instance of the Database convert
    ''' </summary>
    ''' <param name="worker">The BackgroundWorker used to process the conversion.</param>
    ''' <param name="settingsFolder">The location of the EveHQSettings.bin file (HQ.AppData folder).</param>
    ''' <param name="dbFormat">The format of the database to convert.</param>
    ''' <param name="dbFileName">The SQL Compact filename used.</param>
    ''' <param name="dbServerName">The server instance</param>
    ''' <param name="dbDatabase"></param>
    ''' <param name="dbSQLSec"></param>
    ''' <param name="dbUsername"></param>
    ''' <param name="dbPassword"></param>
    ''' <remarks></remarks>
    Public Sub New(worker As backgroundworker, settingsFolder As String, dbFormat As DBFormat, dbFileName As String, dbServerName As String, dbDatabase As String, dbSQLSec As Boolean, dbUsername As String, dbPassword As String)

        _worker = worker
        _settingsFolder = settingsFolder
        _dbFormat = dbFormat
        _dbFileName = dbFileName
        _dbServerName = dbServerName
        _dbDatabase = dbDatabase
        _dbSQLSec = dbSQLSec
        _dbUsername = dbUsername
        _dbPassword = dbPassword

        _sqLiteFile = Path.Combine(_settingsFolder, "EveHQData.db3")

    End Sub

    Public Sub Convert()

        ' Create some new settings
        HQ.Settings = New EveHQSettings

        ' Step 1 - Set the connection strings
        _worker.ReportProgress(0, "Database Conversion Step 1/15: Setting database connections...")
        SetOldConnectionString()
        SetNewConnectionString()

        ' Step 2 - Check the database connection
        _worker.ReportProgress(0, "Database Conversion Step 2/15: Checking database connections...")
        If CheckOldDatabaseConnection() = False Then
            _worker.ReportProgress(0, "Unable to connect to the v2 database!")
            MessageBox.Show("Unable to connect to the v2 database!", "Database connection error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Step 3 - Create the SQLite database
        _worker.ReportProgress(0, "Database Conversion Step 3/15: Creating SQLite database...")
        Try
            CustomDataFunctions.CreateCustomDB()
        Catch e As Exception
            _worker.ReportProgress(0, "Error creating SQLite database: " & e.Message)
        End Try
        
        ' Step 4 - Create the core database tables
        _worker.ReportProgress(0, "Database Conversion Step 4/15: Creating Core database tables...")
        Try
            CustomDataFunctions.CheckCoreDBTables()
        Catch e As Exception
            _worker.ReportProgress(0, "Error creating Core database tables: " & e.Message)
        End Try

        ' Step 5 - Convert the customPrices table
        _worker.ReportProgress(0, "Database Conversion Step 5/15: Converting Custom Prices database table...")
        ConvertCustomPricesTable()

        ' Step 6 - Convert the marketPrices table
        _worker.ReportProgress(0, "Database Conversion Step 6/15: Converting Market Prices database table...")
        ConvertMarketPricesTable()

        ' Step 7 - Convert the eveIDToName table
        _worker.ReportProgress(0, "Database Conversion Step 7/15: Converting ID To Name database table...")
        ConvertEveIDToNameTable()

        ' Step 8 - Convert the eveMail table
        _worker.ReportProgress(0, "Database Conversion Step 8/15: Converting Eve Mail database table ...")
        ConvertEveMailTable()

        ' Step 9 - Convert the eveNotifications table
        _worker.ReportProgress(0, "Database Conversion Step 9/15: Converting Eve Notifications database table...")
        ConvertEveNotificationsTable()

        ' Step 10 - Convert the requisitions table
        _worker.ReportProgress(0, "Database Conversion Step 10/15: Converting Requisitions database table...")
        ConvertRequisitionsTable()

        ' Step 11 - Create the Prism database tables
        _worker.ReportProgress(0, "Database Conversion Step 11/15: Creating Prism database tables...")
        Try
            PrismDataFunctions.CheckDatabaseTables()
        Catch e As Exception
            _worker.ReportProgress(0, "Error creating Prism database tables: " & e.Message)
        End Try

        ' Step 12 - Convert the assetItemNames tables
        _worker.ReportProgress(0, "Database Conversion Step 12/15: Converting Asset Item Names database table...")
        ConvertAssetItemNamesTable()

        ' Step 13 - Convert the inventionResults tables
        _worker.ReportProgress(0, "Database Conversion Step 13/15: Converting Invention Results database table...")
        ConvertInventionResultsTable()

        ' Step 14 - Convert the walletJournal tables
        _worker.ReportProgress(0, "Database Conversion Step 14/15: Converting Wallet Journal database table...")
        ConvertWalletJournalTable()

        ' Step 15 - Convert the walletTransaction tables
        _worker.ReportProgress(0, "Database Conversion Step 15/15: Converting Wallet Transactions database table...")
        ConvertWalletTransactionsTable()

        ' Report finished
        _worker.ReportProgress(0, "Database Conversion complete!")

    End Sub

    Private Sub SetOldConnectionString()
        Select Case _dbFormat
            Case DBFormat.Sqlce  ' SQL CE
                _sqlConn = "Data Source = " & ControlChars.Quote & _dbFileName & ControlChars.Quote & ";" & "Max Database Size = 512; ; Max Buffer Size = 2048;"
            Case DBFormat.Sql  ' SQL
                _sqlConn = "Server=localhost\" & _dbServerName & "; Database = " & _dbDatabase
                If _dbSQLSec = True Then
                    _sqlConn &= "; User ID=" & _dbUsername & "; Password=" & _dbPassword & ";"
                Else
                    _sqlConn &= "; Integrated Security = SSPI;"
                End If
        End Select
    End Sub
    Private Sub SetNewConnectionString()
        _sqLiteConn = "Data Source=" & ControlChars.Quote & _sqLiteFile & ControlChars.Quote & ";Version=3;"
        HQ.Settings.CustomDBFileName = _sqLiteFile
        CustomDataFunctions.SetEveHQDataConnectionString()
    End Sub
    Private Function CheckOldDatabaseConnection() As Boolean
        Select Case _dbFormat
            Case DBFormat.Sqlce
                Dim connection As New SqlCeConnection(_sqlConn)
                Try
                    connection.Open()
                    connection.Close()
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            Case DBFormat.Sql
                Dim connection As New SqlConnection(_sqlConn)
                Try
                    connection.Open()
                    connection.Close()
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            Case Else
                Return False
        End Select
    End Function
    Public Function GetData(ByVal strSQL As String, format As DBFormat, connectionString As String) As DataSet

        Dim evehqData As New DataSet

        Select Case format
            Case DBFormat.Sqlce  ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = connectionString
                Try
                    conn.Open()
                    Dim da As New SqlCeDataAdapter(strSQL, conn)
                    da.Fill(evehqData, "EveHQData")
                    conn.Close()
                    Return evehqData
                Catch e As Exception
                    'MessageBox.Show(e.Message, "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _worker.ReportProgress(0, "Error in SQLCE GetData: " & e.Message)
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case DBFormat.Sql  ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = connectionString
                Try
                    conn.Open()
                    Dim da As New SqlDataAdapter(strSQL, conn)
                    da.Fill(evehqData, "EveHQData")
                    conn.Close()
                    Return evehqData
                Catch e As Exception
                    'MessageBox.Show(e.Message, "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _worker.ReportProgress(0, "Error in SQL GetData: " & e.Message)
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case Else
                'MessageBox.Show("Invalid database format!", "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                _worker.ReportProgress(0, "Error in Database format!")
                Return Nothing
        End Select
    End Function

    Private Sub ConvertCustomPricesTable()
        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from customPrices;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO customPrices ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting custom prices table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertMarketPricesTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from marketPrices;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO marketPrices ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting market prices table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertEveIDToNameTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from eveIDToName;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO eveIDToName ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting EveID to Name table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertEveMailTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from eveMail;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO eveMail ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?,?,?,?,?,?,?,?,?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting Evemail table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertEveNotificationsTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from eveNotifications;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO eveNotifications ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?,?,?,?,?,?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting notifications table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertRequisitionsTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from requisitions;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO requisitions ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?,?,?,?,?,?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting requisitions table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertAssetItemNamesTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from assetItemNames;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO assetItemNames (itemID, itemName"
                            cmd.CommandText &= ") VALUES(?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For col As Integer = 1 To 2
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                cmd.Parameters(0).Value = row.Item("itemID")
                                cmd.Parameters(1).Value = row.Item("itemName")
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting asset item names table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertInventionResultsTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from inventionResults;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO inventionResults ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?,?,?,?,?,?,?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting invention results table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertWalletJournalTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from walletJournal;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO walletJournal ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting wallet journal table: " & e.Message)
        End Try

    End Sub

    Private Sub ConvertWalletTransactionsTable()

        Try
            ' Step 1 - Load all the v2 data
            Using eveData As DataSet = GetData("SELECT * from walletTransactions;", _dbFormat, _sqlConn)
                If eveData IsNot Nothing Then
                    ' Step 2 - Put all the v2 data into the v3 table
                    Dim conn As New SQLiteConnection(_sqLiteConn)
                    conn.Open()
                    Using dbTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As SQLiteCommand = conn.CreateCommand
                            ' Create the DB Command
                            cmd.CommandText = "INSERT INTO walletTransactions ("
                            ' Add the columns
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                cmd.CommandText &= col.ColumnName & ", "
                            Next
                            cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2, 2)
                            cmd.CommandText &= ") VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect R# warning
                            For Each col As DataColumn In eveData.Tables(0).Columns
                                Dim field As SQLiteParameter = cmd.CreateParameter()
                                cmd.Parameters.Add(field)
                            Next
                            ' Add the values
                            For Each row As DataRow In eveData.Tables(0).Rows
                                For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                                    cmd.Parameters(col).Value = row.Item(col)
                                Next
                                cmd.ExecuteNonQuery()
                            Next
                            dbTrans.Commit()
                        End Using
                    End Using
                    conn.Close()
                End If
            End Using
        Catch e As Exception
            _worker.ReportProgress(0, "Error converting wallet transactions table: " & e.Message)
        End Try

    End Sub

End Class
