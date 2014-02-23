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

Imports EveHQ.EveAPI
Imports EveHQ.Core.Requisitions
Imports System.Data.SQLite
Imports System.Windows.Forms
Imports System.Text
Imports System.Xml
Imports EveHQ.Common.Extensions

''' <summary>
''' Class for handling the custom database - based on SQLite format
''' </summary>
''' <remarks></remarks>
Public Class CustomDataFunctions

#Region "Core Database Access Routines"

    ''' <summary>
    ''' Function to create the custom DB connection string
    ''' </summary>
    ''' <returns>A boolean value indicating if the routine was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function SetEveHQDataConnectionString() As Boolean

        HQ.EveHQDataConnectionString = "Data Source=" & ControlChars.Quote & HQ.Settings.CustomDBFilename & ControlChars.Quote & ";Version=3;"
        Return True
    End Function

    ''' <summary>
    ''' Function to check if a connection can be made to the custom database
    ''' </summary>
    ''' <param name="silentResponse">Set to "True" if the routine is to show an error message on connection failure</param>
    ''' <returns>A boolean value indicating if the connection was succesful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckCustomDatabaseConnection(ByVal silentResponse As Boolean) As Boolean
        Dim strConnection As String
        strConnection = "Data Source=" & ControlChars.Quote & HQ.Settings.CustomDBFileName & ControlChars.Quote & ";Version=3;"
        Dim connection As New SQLiteConnection(strConnection)
        Try
            connection.Open()
            connection.Close()
            If silentResponse = False Then
                MessageBox.Show("Connected successfully to SQLite database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return True
        Catch ex As Exception
            If silentResponse = False Then
                MessageBox.Show(ex.Message, "Error Opening SQLite Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Function to return a list of database tables in the custom database
    ''' </summary>
    ''' <returns>A List(Of String) containing the names of the tables in the custom database</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDatabaseTables() As List(Of String)

        Dim dbTables As New List(Of String)
        Using schemaTable As DataSet = GetCustomData("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;")
            For Each tableRow As DataRow In schemaTable.Tables(0).Rows
                dbTables.Add(tableRow.Item("name").ToString)
            Next
        End Using
        Return dbTables
    End Function

    ''' <summary>
    ''' Function to return data from a database
    ''' </summary>
    ''' <param name="sql">A string containing the SQL query to execute</param>
    ''' <returns>A Dataset containing the requrested data</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomData(ByVal sql As String) As DataSet

        Dim evehqData As New DataSet
        Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)
        Try
            conn.Open()
            Dim da As New SQLiteDataAdapter(sql, conn)
            da.Fill(evehqData, "EveHQData")
            conn.Close()
            Return evehqData
        Catch e As Exception
            HQ.dataError = e.Message
            Return Nothing
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    ''' <summary>
    ''' Function to write data to the custom database via an SQL string
    ''' </summary>
    ''' <param name="strSQL">A string containing the SQL command to execute</param>
    ''' <returns>The number of records affected by the command</returns>
    ''' <remarks>Returns -2 in the event of an error</remarks>
    Public Shared Function SetCustomData(ByVal strSQL As String) As Integer

        Dim recordsAffected As Integer
        Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)
        Try
            conn.Open()
            Dim keyCommand As New SQLiteCommand(strSQL, conn)
            recordsAffected = keyCommand.ExecuteNonQuery()
            Return recordsAffected
        Catch e As Exception
            HQ.dataError = e.Message
            HQ.WriteLogEvent("Database Error: " & e.Message)
            HQ.WriteLogEvent("SQL: " & strSQL)
            Return -2
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

#End Region

#Region "Database & Table Creation Routines"

    ''' <summary>
    ''' Creates the custom DB in SQLite format
    ''' </summary>
    ''' <returns>A boolean value indicating if the routine was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateCustomDB() As Boolean

        ' Define the file to connect to
        Dim outputFile As String = HQ.Settings.CustomDBFileName.Replace("\\", "\")

        Try

            ' Create the file
            SQLiteConnection.CreateFile(outputFile)
            Return True

        Catch e As Exception
            HQ.dataError = "Unable to create SQL CE database in " & outputFile & ControlChars.CrLf & ControlChars.CrLf & e.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Function to check all the core database tables
    ''' </summary>
    ''' <returns>A boolean value indicating if all table checks were successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckCoreDBTables() As Boolean

        Dim success As Boolean
        success = True
        If CheckMarketOrdersTable() = False Then success = False
        If CheckMarketHistoryTable() = False Then success = False
        If CheckCustomPricesTable() = False Then success = False
        If CheckMarketPricesTable() = False Then success = False
        If CheckEveIDNameTable() = False Then success = False
        If CheckEveMailTable() = False Then success = False
        If CheckEveNotificationTable() = False Then success = False
        If CheckRequisitionsTable() = False Then success = False

        Return success

    End Function

    ''' <summary>
    ''' Function to check for the existence of the marketOrders database table. The table will be created if it is missing from the custom database.
    ''' </summary>
    ''' <returns>A boolean value indicating if the table exists or creation was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckMarketOrdersTable() As Boolean
        Dim createTable As Boolean
        Dim tables As List(Of String) = GetDatabaseTables()
        If tables IsNot Nothing Then
            If tables.Contains("marketOrders") = False Then
                ' The DB exists but the table doesn't so we'll create this
                createTable = True
            Else
                ' We have the Db and table so we can return a good result
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateCustomDB() = False Then
                MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                createTable = True
            End If
        End If

        ' Create the database table 
        If createTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE marketOrders (")
            strSQL.AppendLine("orderID INTEGER PRIMARY KEY,")
            strSQL.AppendLine("typeID INTEGER NOT NULL,")
            strSQL.AppendLine("regionID INTEGER NOT NULL,")
            strSQL.AppendLine("solarSystemID INTEGER NOT NULL,")
            strSQL.AppendLine("stationID INTEGER NOT NULL,")
            strSQL.AppendLine("cacheDate DATETIME NOT NULL,")
            strSQL.AppendLine("issueDate DATETIME NOT NULL,")
            strSQL.AppendLine("price FLOAT NOT NULL,")
            strSQL.AppendLine("volEntered BIGINT NOT NULL,")
            strSQL.AppendLine("volRemaining BIGINT NOT NULL,")
            strSQL.AppendLine("minVolume BIGINT NOT NULL,")
            strSQL.AppendLine("jumps INTEGER NOT NULL,")
            strSQL.AppendLine("range INTEGER NOT NULL,")
            strSQL.AppendLine("duration INTEGER NOT NULL,")
            strSQL.AppendLine("isBuyOrder BOOLEAN NOT NULL)")
            If SetCustomData(strSQL.ToString) <> -2 Then
                ' Create the indexes
                SetCustomData("CREATE INDEX idxOrdersTypeID ON marketOrders (typeID);")
                SetCustomData("CREATE INDEX idxOrdersRegionID ON marketOrders (regionID);")
                SetCustomData("CREATE INDEX idxOrdersCacheDate ON marketOrders (cacheDate);")
                Return True
            Else
                MessageBox.Show("There was an error creating the Market Orders database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Market Orders Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Function to check for the existence of the marketHistory database table. The table will be created if it is missing from the custom database.
    ''' </summary>
    ''' <returns>A boolean value indicating if the table exists or creation was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckMarketHistoryTable() As Boolean
        Dim createTable As Boolean
        Dim tables As List(Of String) = GetDatabaseTables()
        If tables IsNot Nothing Then
            If tables.Contains("marketHistory") = False Then
                ' The DB exists but the table doesn't so we'll create this
                createTable = True
            Else
                ' We have the Db and table so we can return a good result
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateCustomDB() = False Then
                MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                createTable = True
            End If
        End If

        ' Create the database table 
        If createTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE marketHistory (")
            strSQL.AppendLine("typeID INTEGER NOT NULL,")
            strSQL.AppendLine("regionID INTEGER NOT NULL,")
            strSQL.AppendLine("historyDate DATETIME NOT NULL,")
            strSQL.AppendLine("lowPrice FLOAT NOT NULL,")
            strSQL.AppendLine("highPrice FLOAT NOT NULL,")
            strSQL.AppendLine("avgPrice FLOAT NOT NULL,")
            strSQL.AppendLine("volume BIGINT NOT NULL,")
            strSQL.AppendLine("orders INTEGER NOT NULL)")
            If SetCustomData(strSQL.ToString) <> -2 Then
                ' Create the indexes
                SetCustomData("CREATE INDEX idxHistoryTypeID ON marketHistory (typeID);")
                SetCustomData("CREATE INDEX idxHistoryRegionID ON marketHistory (regionID);")
                SetCustomData("CREATE INDEX idxHistoryDate ON marketHistory (historyDate);")
                Return True
            Else
                MessageBox.Show("There was an error creating the Market History database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Market History Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Function to check for the existence of the customPrices database table. The table will be created if it is missing from the custom database.
    ''' </summary>
    ''' <returns>A boolean value indicating if the table exists or creation was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckCustomPricesTable() As Boolean
        Dim createTable As Boolean
        Dim tables As List(Of String) = GetDatabaseTables()
        If tables IsNot Nothing Then
            If tables.Contains("customPrices") = False Then
                ' The DB exists but the table doesn't so we'll create this
                createTable = True
            Else
                ' We have the Db and table so we can return a good result
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateCustomDB() = False Then
                MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                createTable = True
            End If
        End If

        ' Create the database table if we need to
        If createTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE customPrices")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  typeID         int,")
            strSQL.AppendLine("  price          float,")
            strSQL.AppendLine("  priceDate      datetime,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT customPrices_PK PRIMARY KEY (typeID)")
            strSQL.AppendLine(");")
            If SetCustomData(strSQL.ToString) <> -2 Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Function to check for the existence of the marketPrice database table. The table will be created if it is missing from the custom database.
    ''' </summary>
    ''' <returns>A boolean value indicating if the table exists or creation was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckMarketPricesTable() As Boolean
        Dim createTable As Boolean
        Dim tables As List(Of String) = GetDatabaseTables()
        If tables IsNot Nothing Then
            If tables.Contains("marketPrices") = False Then
                ' The DB exists but the table doesn't so we'll create this
                createTable = True
            Else
                ' We have the Db and table so we can return a good result
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateCustomDB() = False Then
                MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                createTable = True
            End If
        End If

        ' Create the database table 
        If createTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE marketPrices")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  typeID         int,")
            strSQL.AppendLine("  price          float,")
            strSQL.AppendLine("  priceDate      datetime,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT marketPrices_PK PRIMARY KEY (typeID)")
            strSQL.AppendLine(")")
            If SetCustomData(strSQL.ToString) <> -2 Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Function to check for the existence of the eveIDToName database table. The table will be created if it is missing from the custom database.
    ''' </summary>
    ''' <returns>A boolean value indicating if the table exists or creation was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckEveIDNameTable() As Boolean
        Dim createTable As Boolean
        Dim tables As List(Of String) = GetDatabaseTables()
        If tables IsNot Nothing Then
            If tables.Contains("eveIDToName") = False Then
                ' The DB exists but the table doesn't so we'll create this
                createTable = True
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
            If CreateCustomDB() = False Then
                MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                createTable = True
            End If
        End If

        ' Create the database table 
        If createTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE eveIDToName")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  eveID      bigint NOT NULL,")
            strSQL.AppendLine("  eveName    nvarchar(255) NOT NULL,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT eveIDToName_PK PRIMARY KEY (eveID)")
            strSQL.AppendLine(")")
            If SetCustomData(strSQL.ToString) <> -2 Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Eve ID-To-Name database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Eve ID-To-Name Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Function to check for the existence of the eveMail database table. The table will be created if it is missing from the custom database.
    ''' </summary>
    ''' <returns>A boolean value indicating if the table exists or creation was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckEveMailTable() As Boolean
        Dim createTable As Boolean
        Dim tables As List(Of String) = GetDatabaseTables()
        If tables IsNot Nothing Then
            If tables.Contains("eveMail") = False Then
                ' The DB exists but the table doesn't so we'll create this
                createTable = True
            Else
                ' We have the DB and table so we can check the existence of the messagebody field
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateCustomDB() = False Then
                MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                createTable = True
            End If
        End If

        ' Create the database table 
        If createTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE eveMail")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  messageKey           nvarchar(30) NOT NULL,")
            strSQL.AppendLine("  messageID            bigint NOT NULL,")
            strSQL.AppendLine("  originatorID         bigint NOT NULL,")
            strSQL.AppendLine("  senderID             bigint NOT NULL,")
            strSQL.AppendLine("  sentDate             datetime NOT NULL,")
            strSQL.AppendLine("  title                nvarchar(1000) NOT NULL,")
            strSQL.AppendLine("  toCorpOrAllianceID   nvarchar(1000) NULL,")
            strSQL.AppendLine("  toCharacterIDs       nvarchar(1000) NULL,")
            strSQL.AppendLine("  toListIDs            nvarchar(1000) NULL,")
            strSQL.AppendLine("  readMail             bit NOT NULL,")
            strSQL.AppendLine("  messageBody          ntext NULL,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT eveMail_PK PRIMARY KEY (messageKey)")
            strSQL.AppendLine(")")
            If SetCustomData(strSQL.ToString) <> -2 Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Eve Mail database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Eve Mail Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Function to check for the existence of the eveNotifications database table. The table will be created if it is missing from the custom database.
    ''' </summary>
    ''' <returns>A boolean value indicating if the table exists or creation was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckEveNotificationTable() As Boolean
        Dim createTable As Boolean
        Dim tables As List(Of String) = GetDatabaseTables()
        If tables IsNot Nothing Then
            If tables.Contains("eveNotifications") = False Then
                ' The DB exists but the table doesn't so we'll create this
                createTable = True
            Else
                ' We have the DB and table so we can check the existence of the messagebody field
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateCustomDB() = False Then
                MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                createTable = True
            End If
        End If

        ' Create the database table 
        If createTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE eveNotifications")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  messageKey           nvarchar(30) NOT NULL,")
            strSQL.AppendLine("  messageID            bigint NOT NULL,")
            strSQL.AppendLine("  typeID               bigint NOT NULL,")
            strSQL.AppendLine("  originatorID         bigint NOT NULL,")
            strSQL.AppendLine("  senderID             bigint NOT NULL,")
            strSQL.AppendLine("  sentDate             datetime NOT NULL,")
            strSQL.AppendLine("  readMail             bit NOT NULL,")
            strSQL.AppendLine("  messageBody          ntext NULL,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT eveNotifications_PK PRIMARY KEY (messageKey)")
            strSQL.AppendLine(")")
            If SetCustomData(strSQL.ToString) <> -2 Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Eve Notification database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Eve Notification Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Function to check for the existence of the requisitions database table. The table will be created if it is missing from the custom database.
    ''' </summary>
    ''' <returns>A boolean value indicating if the table exists or creation was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckRequisitionsTable() As Boolean
        Dim createTable As Boolean
        Dim tables As List(Of String) = GetDatabaseTables()
        If tables IsNot Nothing Then
            If tables.Contains("requisitions") = False Then
                ' The DB exists but the table doesn't so we'll create this
                createTable = True
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
            If CreateCustomDB() = False Then
                MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                createTable = True
            End If
        End If

        ' Create the database table 
        If createTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE [requisitions](")
            strSQL.AppendLine("[orderID] INTEGER PRIMARY KEY,")
            strSQL.AppendLine("[itemID] INTEGER NOT NULL,")
            strSQL.AppendLine("[itemName] [nvarchar](200) NOT NULL,")
            strSQL.AppendLine("[itemQuantity] [int] NOT NULL,")
            strSQL.AppendLine("[source] [nvarchar](50) NOT NULL,")
            strSQL.AppendLine("[requestor] [nvarchar](50) NOT NULL,")
            strSQL.AppendLine("[requestDate] [datetime] NOT NULL,")
            strSQL.AppendLine("[requisition] [nvarchar](50) NOT NULL )")
            If SetCustomData(strSQL.ToString) <> -2 Then
                ' Create the indexes
                SetCustomData("CREATE INDEX idxReqSource ON requisitions (source);")
                SetCustomData("CREATE INDEX idxReqRequestor ON requisitions (requestor);")
                SetCustomData("CREATE INDEX idxReqName ON requisitions (requisition);")
                Return True
            Else
                MessageBox.Show("There was an error creating the Requisitions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Requisitions Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Else
            Return True
        End If
    End Function

#End Region

#Region "Custom Price DB Routines"

    ''' <summary>
    ''' Loads the custom prices from the database
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub LoadCustomPricesFromDB()
        Dim eveData As New DataSet
        Try
            eveData = GetCustomData("SELECT * FROM customPrices ORDER BY typeID;")
            If eveData IsNot Nothing Then
                HQ.CustomPriceList.Clear()
                For Each priceRow As DataRow In eveData.Tables(0).Rows
                    HQ.CustomPriceList.Add(CInt(priceRow.Item("typeID")), CDbl(priceRow.Item("price")))
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("There was an error fetching the Custom Price data. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            If eveData IsNot Nothing Then
                eveData.Dispose()
                GC.Collect()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Function to write a single price to the custom price list
    ''' </summary>
    ''' <param name="typeID">The typeID of the item</param>
    ''' <param name="price">The price of the item</param>
    ''' <returns>An integer representing the number of database records affected</returns>
    ''' <remarks></remarks>
    Public Shared Function SetCustomPrice(typeID As Integer, price As Double) As Integer

        ' Create a new price list and pass to the multiple price routine
        Dim priceList As New Dictionary(Of Integer, Double)
        priceList.Add(typeID, price)
        Return SetCustomPrices(priceList)
    End Function

    ''' <summary>
    ''' Function to write single/multiple prices to the custom price list
    ''' </summary>
    ''' <param name="priceList">A list of typeIDs with prices</param>
    ''' <returns>An integer representing the number of database records affected</returns>
    ''' <remarks></remarks>
    Public Shared Function SetCustomPrices(priceList As Dictionary(Of Integer, Double)) As Integer

        ' Create the database commands for updating
        Dim recordsAffected As Integer
        Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)

        Try
            conn.Open()
            Using sqlTrans As SQLiteTransaction = conn.BeginTransaction()
                Using sqlCmd As SQLiteCommand = conn.CreateCommand

                    ' Create the DB Command - note use of the INSERT OR REPLACE syntax!
                    sqlCmd.CommandText = "INSERT OR REPLACE INTO customPrices (typeID, price, priceDate) VALUES (?,?,?)"

                    ' Create the desired number of parameters
                    ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                    For col As Integer = 1 To 3
                        sqlCmd.Parameters.Add(sqlCmd.CreateParameter)
                    Next

                    ' Run through the price list
                    For Each typeID As Integer In priceList.Keys

                        ' Update the local copy
                        If HQ.CustomPriceList.ContainsKey(typeID) = False Then
                            HQ.CustomPriceList.Add(typeID, priceList(typeID))
                        Else
                            HQ.CustomPriceList(typeID) = priceList(typeID)
                        End If

                        ' Add the database values and execute the query
                        sqlCmd.Parameters(0).Value = typeID
                        sqlCmd.Parameters(1).Value = priceList(typeID)
                        sqlCmd.Parameters(2).Value = Now
                        recordsAffected += sqlCmd.ExecuteNonQuery()

                    Next

                    ' Commit to the database
                    sqlTrans.Commit()

                End Using
            End Using

            ' Close the connection and return the number of rows affected
            conn.Close()
            Return recordsAffected

        Catch e As Exception
            HQ.dataError = e.Message
            HQ.WriteLogEvent("Database Error: " & e.Message)
            HQ.WriteLogEvent("SQL: " & "")
            Return -2
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    ''' <summary>
    ''' Removes a custom price from the database
    ''' </summary>
    ''' <param name="typeID">The typeID of the item</param>
    ''' <returns>A boolean indicating if the price was successfully removed from the DB</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteCustomPrice(ByVal typeID As Integer) As Boolean
        ' Double check it exists and delete it
        If HQ.CustomPriceList.ContainsKey(typeID) = True Then
            HQ.CustomPriceList.Remove(typeID)
        End If
        Dim priceSQL As String = "DELETE FROM customPrices WHERE typeID=" & typeID & ";"
        If SetCustomData(priceSQL) = -2 Then
            MessageBox.Show("There was an error deleting data from the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            Return True
        End If
    End Function

#End Region

#Region "Market Price DB Routines"

    ' ''' <summary>
    ' ''' Loads the market prices from the database
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Shared Sub LoadMarketPricesFromDB()
    '    Dim eveData As New DataSet
    '    Try
    '        eveData = GetCustomData("SELECT * FROM marketPrices ORDER BY typeID;")
    '        If eveData IsNot Nothing Then
    '            HQ.MarketPriceList.Clear()
    '            For Each priceRow As DataRow In eveData.Tables(0).Rows
    '                HQ.MarketPriceList.Add(CInt(priceRow.Item("typeID")), CDbl(priceRow.Item("price")))
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show("There was an error fetching the Market Price data. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    Finally
    '        If eveData IsNot Nothing Then
    '            eveData.Dispose()
    '            GC.Collect()
    '        End If
    '    End Try
    'End Sub

    ' ''' <summary>
    ' ''' Function to write a single price to the market price list
    ' ''' </summary>
    ' ''' <param name="typeID">The typeID of the item</param>
    ' ''' <param name="price">The price of the item</param>
    ' ''' <returns>An integer representing the number of database records affected</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function SetMarketPrice(typeID As Integer, price As Double) As Integer

    '    ' Create a new price list and pass to the multiple price routine
    '    Dim priceList As New Dictionary(Of Integer, Double)
    '    priceList.Add(typeID, price)
    '    Return SetMarketPrices(priceList)
    'End Function

    ' ''' <summary>
    ' ''' Function to write single/multiple prices to the market price list
    ' ''' </summary>
    ' ''' <param name="priceList">A list of typeIDs with prices</param>
    ' ''' <returns>An integer representing the number of database records affected</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function SetMarketPrices(priceList As Dictionary(Of Integer, Double)) As Integer

    '    ' Create the database commands for updating
    '    Dim recordsAffected As Integer
    '    Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)

    '    Try
    '        conn.Open()
    '        Using sqlTrans As SQLiteTransaction = conn.BeginTransaction()
    '            Using sqlCmd As SQLiteCommand = conn.CreateCommand

    '                ' Create the DB Command - note use of the INSERT OR REPLACE syntax!
    '                sqlCmd.CommandText = "INSERT OR REPLACE INTO marketPrices (typeID, price, priceDate) VALUES (?,?,?)"

    '                ' Create the desired number of parameters
    '                For col As Integer = 1 To 3
    '                    sqlCmd.Parameters.Add(sqlCmd.CreateParameter)
    '                Next

    '                ' Run through the price list
    '                For Each typeID As Integer In priceList.Keys

    '                    ' Update the local copy
    '                    If HQ.MarketPriceList.ContainsKey(typeID) = False Then
    '                        HQ.MarketPriceList.Add(typeID, priceList(typeID))
    '                    Else
    '                        HQ.MarketPriceList(typeID) = priceList(typeID)
    '                    End If

    '                    ' Add the database values and execute the query
    '                    sqlCmd.Parameters(0).Value = typeID
    '                    sqlCmd.Parameters(1).Value = priceList(typeID)
    '                    sqlCmd.Parameters(2).Value = Now
    '                    recordsAffected += sqlCmd.ExecuteNonQuery()

    '                Next

    '                ' Commit to the database
    '                sqlTrans.Commit()

    '            End Using
    '        End Using

    '        ' Close the connection and return the number of rows affected
    '        conn.Close()
    '        Return recordsAffected

    '    Catch e As Exception
    '        HQ.dataError = e.Message
    '        HQ.WriteLogEvent("Database Error: " & e.Message)
    '        HQ.WriteLogEvent("SQL: " & "")
    '        Return -2
    '    Finally
    '        If conn.State = ConnectionState.Open Then
    '            conn.Close()
    '        End If
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' Removes a market price from the database
    ' ''' </summary>
    ' ''' <param name="typeID">The typeID of the item</param>
    ' ''' <returns>A boolean indicating if the price was successfully removed from the DB</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function DeleteMarketPrice(ByVal typeID As Integer) As Boolean
    '    ' Double check it exists and delete it
    '    If HQ.MarketPriceList.ContainsKey(typeID) = True Then
    '        HQ.MarketPriceList.Remove(typeID)
    '    End If
    '    Dim priceSQL As String = "DELETE FROM marketPrices WHERE typeID=" & typeID & ";"
    '    If SetCustomData(priceSQL) = -2 Then
    '        MessageBox.Show("There was an error deleting data from the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '        Return False
    '    Else
    '        Return True
    '    End If
    'End Function

#End Region

#Region "EveIDToName DB Routines"

    ''' <summary>
    ''' Parses a string of comma-separated IDs into a list
    ''' </summary>
    ''' <param name="IDs">An existing list to add the parsed IDs into</param>
    ''' <param name="strID">A string of comma-separated IDs</param>
    ''' <remarks></remarks>
    Public Shared Sub ParseIDs(ByRef ids As List(Of String), ByVal strID As String)
        Dim strIDs() As String = strID.Split(",".ToCharArray)
        For Each id As String In strIDs
            If id.Trim <> "" Then
                If ids.Contains(id) = False Then
                    ids.Add(id)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Writes a list of IDs to the EveIDToName database table
    ''' </summary>
    ''' <param name="IDs">A List(Of String) containing the IDs to add to the database table</param>
    ''' <remarks></remarks>
    Public Shared Sub WriteEveIDsToDatabase(ByVal ids As List(Of String))

        Dim mainIDList As New List(Of String)
        For Each id As String In ids
            mainIDList.Add(id)
        Next

        If mainIDList.Count > 0 Then
            Do

                ' Break the ID list into groups of 200
                Const MaxIDs As Integer = 200
                Dim reqIDList As New List(Of String)
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                For idx As Integer = 0 To Math.Min(mainIDList.Count, MaxIDs) - 1
                    reqIDList.Add(mainIDList.Item(0))
                    mainIDList.RemoveAt(0)
                Next

                ' Write to log file about ID request
                HQ.WriteLogEvent("***** Start: Request Eve IDs From API *****")

                HQ.WriteLogEvent("Building Required ID List")
                Dim strID As New StringBuilder
                For Each id As String In reqIDList
                    strID.Append("," & id)
                Next
                strID.Remove(0, 1)
                HQ.WriteLogEvent("Finishing building list of " & reqIDList.Count.ToString & " IDs")

                ' Get a list of everything we already have
                HQ.WriteLogEvent("Querying existing IDToName data")
                Dim existingIDs As New List(Of String)
                Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                Dim idData As DataSet = GetCustomData(strSQL)
                If idData IsNot Nothing Then
                    If idData.Tables(0).Rows.Count > 0 Then
                        For Each idRow As DataRow In idData.Tables(0).Rows
                            existingIDs.Add(CStr(idRow.Item("eveID")))
                        Next
                    End If
                End If

                HQ.WriteLogEvent("Removing existing IDs from the list")
                ' Remove existing IDs from the parsed list
                Dim removeCount As Integer = 0
                For Each existingID As String In existingIDs
                    If reqIDList.Contains(existingID) Then
                        reqIDList.Remove(existingID)
                        removeCount += 1
                    End If
                Next
                HQ.WriteLogEvent("Finished Removing " & removeCount.ToString & " existing IDs from the list")

                If reqIDList.Count > 0 Then

                    HQ.WriteLogEvent("Rebuilding Required ID List for sending to the API")
                    Dim idsToQuery = reqIDList.Select(Function(s) s.ToInt64())

                    HQ.WriteLogEvent("Finishing building list of " & reqIDList.Count.ToString & " IDs for the API")

                    ' Send this to the API if we have something!
                    HQ.WriteLogEvent("Requesting ID List From the API: " & strID.ToString)
                    'Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
                    'Dim idxml As XmlDocument = apiReq.GetAPIXML(APITypes.IDToName, strID.ToString, APIReturnMethods.ReturnActual)
                    Dim nameResponse = HQ.ApiProvider.Eve.CharacterName(idsToQuery)
                    ' Parse this XML
                    Dim finalIDs As New SortedList(Of Long, String)

                    Dim eveID As Long
                    Dim eveName As String

                    Dim idList = nameResponse.ResultData
                    If nameResponse.IsSuccess Then
                        HQ.WriteLogEvent("Parsing " & idList.Count.ToString & " IDs in the XML file")
                        For Each idNode In idList
                            eveID = idNode.Id
                            eveName = idNode.Name
                            If finalIDs.ContainsKey(eveID) = False Then
                                finalIDs.Add(eveID, eveName)
                            End If
                        Next
                    Else
                        If nameResponse.EveErrorCode > 0 Then
                            HQ.WriteLogEvent("Error " & nameResponse.EveErrorText & " returned by the API!")
                        End If
                    End If

                    ' Add all the data to the database
                    HQ.WriteLogEvent("Committing data to the eveIDToName database table")
                    Call CommitEveIDListToDB(finalIDs)


                    HQ.WriteLogEvent("All IDs present in the database, no request required to the API")
                End If

                ' Write to log file about ID request
                HQ.WriteLogEvent("***** End: Request Eve IDs From API *****")

            Loop Until mainIDList.Count = 0
        End If
    End Sub

    ''' <summary>
    ''' Obtains a mailing list API and writes the IDs to the EveIDToName DB Table
    ''' </summary>
    ''' <param name="mPilot"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function WriteMailingListIDsToDatabase(ByVal mPilot As EveHQPilot) As SortedList(Of Long, String)
        Dim finalIDs As New SortedList(Of Long, String)
        Dim accountName As String = mPilot.Account
        If accountName <> "" Then
            Dim mAccount As EveHQAccount = HQ.Settings.Accounts.Item(accountName)
            ' Send this to the API
            Dim mailingListResponse As EveServiceResponse(Of IEnumerable(Of MailingList)) = HQ.ApiProvider.Character.MailingLists(mAccount.UserID, mAccount.APIKey, Int32.Parse(mPilot.ID))
            
            ' Parse this XML
            If mailingListResponse.IsSuccess Then
                Dim mailingLists As IEnumerable(Of MailingList) = mailingListResponse.ResultData

                If mailingLists.Any() Then
                    For Each list As MailingList In From list1 In mailingLists Where finalIDs.ContainsKey(list1.ListId) = False
                        finalIDs.Add(list.ListId, list.DisplayName)
                    Next
                End If
            End If

            ' Add all the data to the database
            Call CommitEveIDListToDB(finalIDs)
        End If
        Return finalIDs
    End Function

    ''' <summary>
    ''' Function to commit Eve IDs and names to the EveIDToName database table
    ''' </summary>
    ''' <param name="idList">A list of IDs with their associated name</param>
    ''' <returns>An integer representing the number of database records affected</returns>
    ''' <remarks></remarks>
    Public Shared Function CommitEveIDListToDB(idList As SortedList(Of Long, String)) As Integer

        ' Create the database commands for updating
        Dim recordsAffected As Integer
        Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)

        Try
            conn.Open()
            Using sqlTrans As SQLiteTransaction = conn.BeginTransaction()
                Using sqlCmd As SQLiteCommand = conn.CreateCommand

                    ' Create the DB Command - note use of the INSERT OR REPLACE syntax!
                    sqlCmd.CommandText = "INSERT OR REPLACE INTO eveIDToName (eveID, eveName) VALUES (?,?)"

                    ' Create the desired number of parameters
                    ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                    For col As Integer = 1 To 2
                        sqlCmd.Parameters.Add(sqlCmd.CreateParameter)
                    Next

                    ' Run through the price list
                    For Each id As Long In idList.Keys

                        ' Add the database values and execute the query
                        sqlCmd.Parameters(0).Value = id
                        sqlCmd.Parameters(1).Value = idList(id)
                        recordsAffected += sqlCmd.ExecuteNonQuery()

                    Next

                    ' Commit to the database
                    sqlTrans.Commit()

                End Using
            End Using

            ' Close the connection and return the number of rows affected
            conn.Close()
            Return recordsAffected

        Catch e As Exception
            HQ.dataError = e.Message
            HQ.WriteLogEvent("Database Error: " & e.Message)
            HQ.WriteLogEvent("SQL: " & "")
            Return -2
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

#End Region

#Region "EveMail & EveNotification DB Routines"


#End Region

#Region "Requisition DB Routines"

    ''' <summary>
    ''' Function to populate the internal requisition classes from the database
    ''' </summary>
    ''' <param name="searchString">A string with which to search the items in the requisition</param>
    ''' <param name="requisition">The name of the requisition</param>
    ''' <param name="source">The source name of the requisition</param>
    ''' <param name="requestor">The requestor of the requisition</param>
    ''' <returns>A SortedList of requisitions</returns>
    ''' <remarks></remarks>
    Public Shared Function PopulateRequisitions(ByVal searchString As String, ByVal requisition As String, ByVal source As String, ByVal requestor As String) As SortedList(Of String, Requisition)
        ' Build the SQL string
        Dim strSQL As New StringBuilder
        strSQL.Append("SELECT * FROM requisitions")
        strSQL.Append(" WHERE (itemName LIKE '%" & searchString.Replace("'", "''") & "%'")
        If requisition <> "" Then
            strSQL.Append(" AND requisition='" & requisition.Replace("'", "''") & "'")
        End If
        If source <> "" Then
            strSQL.Append(" AND source='" & source.Replace("'", "''") & "'")
        End If
        If requestor <> "" Then
            strSQL.Append(" AND requestor='" & requestor.Replace("'", "''") & "'")
        End If
        strSQL.Append(");")
        Dim reqData As DataSet = GetCustomData(strSQL.ToString)
        Dim reqs As New SortedList(Of String, Requisition)
        If reqData IsNot Nothing Then
            If reqData.Tables(0).Rows.Count > 0 Then
                ' Populate the requisitions
                For Each reqRow As DataRow In reqData.Tables(0).Rows
                    Dim reqName As String = reqRow.Item("requisition").ToString
                    ' Check if the requisition exists
                    Dim newReq As New Requisition
                    If reqs.ContainsKey(reqName) Then
                        newReq = reqs(reqName)
                    Else
                        newReq.Name = reqRow.Item("requisition").ToString
                        newReq.Requestor = reqRow.Item("requestor").ToString
                        newReq.Source = reqRow.Item("source").ToString
                        reqs.Add(newReq.Name, newReq)
                    End If
                    ' Add the order
                    Dim newOrder As New RequisitionOrder
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
                Return reqs
            Else
                Return reqs
            End If
        Else
            Return reqs
        End If
    End Function

    ''' <summary>
    ''' Function to count the total number of requisitions in the database
    ''' </summary>
    ''' <returns>The number of records in the requisitions database table</returns>
    ''' <remarks></remarks>
    Public Shared Function CountRequisitions() As Long

        ' Return the number of available requisitions
        Const StrSQL As String = "SELECT * FROM requisitions;"
        Using reqData As DataSet = GetCustomData(StrSQL.ToString)
            If reqData IsNot Nothing Then
                Return reqData.Tables(0).Rows.Count
            Else
                Return 0
            End If
        End Using
    End Function

#End Region

#Region "Market History DB Routines"

    ' ''' <summary>
    ' ''' Updates the database with the market history from the cache files
    ' ''' </summary>
    ' ''' <param name="result">A MarketCacheResult containing the data</param>
    ' ''' <returns>An integer containing the number of records written to the database</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function UpdateMarketHistory(result As Market.MarketCacheResult) As Integer

    '    ' Check if we need to update anything at all (no point in wasting time on it otherwise)
    '    If result.ResultType = Market.MarketCacheResultType.GetOldPriceHistory Then

    '        ' Get the last date in the current market history table
    '        Dim lastDate As DateTime = GetLastHistoryDate(result.TypeID, result.RegionID)

    '        ' Latest date will be the last value so check this...
    '        If lastDate < result.PriceHistory.Values(result.PriceHistory.Count - 1).HistoryDate Then

    '            ' Create the database commands for updating
    '            Dim recordsAffected As Integer
    '            Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)

    '            Try
    '                conn.Open()
    '                Using sqlTrans As SQLiteTransaction = conn.BeginTransaction()
    '                    Using sqlCmd As SQLiteCommand = conn.CreateCommand

    '                        ' Create the DB Command - note use of the INSERT OR REPLACE syntax!
    '                        sqlCmd.CommandText = "INSERT OR REPLACE INTO marketHistory (typeID, regionID, historyDate, lowPrice, highPrice, avgPrice, volume, orders) VALUES (?,?,?,?,?,?,?,?)"

    '                        ' Create the desired number of parameters
    '                        For col As Integer = 1 To 8
    '                            sqlCmd.Parameters.Add(sqlCmd.CreateParameter)
    '                        Next

    '                        ' ...but only add the items we actually need
    '                        Dim idx As Integer = result.PriceHistory.Count - 1
    '                        Do While idx >= 0

    '                            If result.PriceHistory.Values(idx).HistoryDate > lastDate Then
    '                                ' Add the database values and execute the query
    '                                sqlCmd.Parameters(0).Value = result.TypeID
    '                                sqlCmd.Parameters(1).Value = result.RegionID
    '                                sqlCmd.Parameters(2).Value = result.PriceHistory.Values(idx).HistoryDate
    '                                sqlCmd.Parameters(3).Value = result.PriceHistory.Values(idx).LowPrice
    '                                sqlCmd.Parameters(4).Value = result.PriceHistory.Values(idx).HighPrice
    '                                sqlCmd.Parameters(5).Value = result.PriceHistory.Values(idx).AvgPrice
    '                                sqlCmd.Parameters(6).Value = result.PriceHistory.Values(idx).Volume
    '                                sqlCmd.Parameters(7).Value = result.PriceHistory.Values(idx).Orders

    '                                recordsAffected += sqlCmd.ExecuteNonQuery()
    '                            End If

    '                            ' Decrease the loop counter
    '                            idx -= 1

    '                        Loop

    '                        ' Commit to the database now we've finished looping relevant history prices
    '                        sqlTrans.Commit()

    '                    End Using

    '                End Using

    '                ' Close the connection and return the number of rows affected
    '                conn.Close()
    '                Return recordsAffected

    '            Catch e As Exception
    '                HQ.DataError = e.Message
    '                HQ.WriteLogEvent("Database Error: " & e.Message)
    '                HQ.WriteLogEvent("SQL: " & "")
    '                Return -2
    '            Finally
    '                If conn.State = ConnectionState.Open Then
    '                    conn.Close()
    '                End If
    '            End Try

    '        Else
    '            Return -2
    '        End If
    '    Else
    '        Return -2
    '    End If

    'End Function

    ' ''' <summary>
    ' ''' Gets the last date provided for a particular item in a specified region
    ' ''' </summary>
    ' ''' <param name="typeID">The typeID of the item to query</param>
    ' ''' <param name="regionID">The regionID of the item to query</param>
    ' ''' <returns>The date of the last entry in the history table for the particular item in a specified region</returns>
    ' ''' <remarks>Used to determine whether to add any more price data for the item</remarks>
    'Public Shared Function GetLastHistoryDate(typeID As Integer, regionID As Integer) As DateTime

    '    Dim strSQL As String = "SELECT * FROM marketHistory WHERE typeID=" & typeID.ToString & " & regionID=" & regionID.ToString & " ORDER BY historyDate DESC LIMIT 1;"
    '    Dim priceData As DataSet = GetCustomData(strSQL)
    '    If priceData IsNot Nothing Then
    '        If priceData.Tables(0).Rows.Count > 0 Then
    '            Return CDate(priceData.Tables(0).Rows(0).Item("historyDate"))
    '        Else
    '            ' Return a old date before Eve
    '            Return New Date(2000, 1, 1)
    '        End If
    '    Else
    '        ' Return an old date before Eve
    '        Return New Date(2000, 1, 1)
    '    End If

    'End Function

    ' ''' <summary>
    ' ''' Routine to delete all rows in the market history table
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Shared Sub DeleteAllMarketHistory()

    '    Const priceSQL As String = "DELETE FROM marketHistory;"
    '    Dim recordsAffected As Integer = SetCustomData(priceSQL)
    '    If recordsAffected = -2 Then
    '        MessageBox.Show("There was an error deleting data from the Market History database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Deleting History Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    Else
    '        'MessageBox.Show("Successfully deleted " & recordsAffected.ToString("N0") & " market history records.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End If

    'End Sub

#End Region

#Region "Market Orders DB Routines"

    ' ''' <summary>
    ' ''' Updates the database with the market history from the cache files
    ' ''' </summary>
    ' ''' <param name="result">A MarketCacheResult containing the data</param>
    ' ''' <returns>An integer containing the number of records written to the database</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function UpdateMarketOrders(result As Market.MarketCacheResult) As Integer

    '    ' Check if we need to update anything at all (no point in wasting time on it otherwise)
    '    If result.ResultType = Market.MarketCacheResultType.GetOrders Then

    '        Dim cacheDate As DateTime = Now

    '        ' Rmove all existing orders for this particular type
    '        DeleteMarketOrders(result.TypeID, result.RegionID)

    '        ' Create the database commands for updating
    '        Dim recordsAffected As Integer
    '        Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)

    '        Try
    '            conn.Open()
    '            Using sqlTrans As SQLiteTransaction = conn.BeginTransaction()
    '                Using sqlCmd As SQLiteCommand = conn.CreateCommand

    '                    ' Create the DB Command - note use of the INSERT OR REPLACE syntax!
    '                    sqlCmd.CommandText = "INSERT OR REPLACE INTO marketOrders (orderID, typeID, regionID, solarSystemID, stationID, cacheDate, issueDate, price, volEntered, volRemaining, minVolume, jumps, range, duration, isBuyOrder) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

    '                    ' Create the desired number of parameters
    '                    For col As Integer = 1 To 15
    '                        sqlCmd.Parameters.Add(sqlCmd.CreateParameter)
    '                    Next

    '                    ' ...but only add the items we actually need
    '                    Dim idx As Integer = result.Orders.Count - 1
    '                    Do While idx >= 0

    '                        ' Add the database values and execute the query
    '                        sqlCmd.Parameters(0).Value = result.Orders.Values(idx).OrderID
    '                        sqlCmd.Parameters(1).Value = result.Orders.Values(idx).TypeID
    '                        sqlCmd.Parameters(2).Value = result.Orders.Values(idx).RegionID
    '                        sqlCmd.Parameters(3).Value = result.Orders.Values(idx).SolarSystemID
    '                        sqlCmd.Parameters(4).Value = result.Orders.Values(idx).StationID
    '                        sqlCmd.Parameters(5).Value = cacheDate
    '                        sqlCmd.Parameters(6).Value = result.Orders.Values(idx).IssueDate
    '                        sqlCmd.Parameters(7).Value = result.Orders.Values(idx).Price
    '                        sqlCmd.Parameters(8).Value = result.Orders.Values(idx).VolEntered
    '                        sqlCmd.Parameters(9).Value = result.Orders.Values(idx).VolRemaining
    '                        sqlCmd.Parameters(10).Value = result.Orders.Values(idx).MinVolume
    '                        sqlCmd.Parameters(11).Value = result.Orders.Values(idx).Jumps
    '                        sqlCmd.Parameters(12).Value = result.Orders.Values(idx).Range
    '                        sqlCmd.Parameters(13).Value = result.Orders.Values(idx).Duration
    '                        sqlCmd.Parameters(14).Value = result.Orders.Values(idx).IsBuyOrder

    '                        recordsAffected += sqlCmd.ExecuteNonQuery()

    '                        ' Decrease the loop counter
    '                        idx -= 1

    '                    Loop

    '                    ' Commit to the database now we've finished looping relevant history prices
    '                    sqlTrans.Commit()

    '                End Using

    '            End Using

    '            ' Close the connection and return the number of rows affected
    '            conn.Close()
    '            Return recordsAffected

    '        Catch e As Exception
    '            HQ.DataError = e.Message
    '            HQ.WriteLogEvent("Database Error: " & e.Message)
    '            HQ.WriteLogEvent("SQL: " & "")
    '            Return -2
    '        Finally
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End Try

    '    Else
    '        Return -2
    '    End If

    'End Function

    ' ''' <summary>
    ' ''' Routine to delete specific rows in the market orders table
    ' ''' </summary>
    ' ''' <param name="typeID">The typeID of rows to remove</param>
    ' ''' <param name="regionID">The regionID of rows to remove</param>
    ' ''' <remarks>Requires both typeID and regionID</remarks>
    'Public Shared Sub DeleteMarketOrders(typeID As Integer, regionID As Integer)

    '    Dim priceSQL As String = "DELETE FROM marketOrders WHERE typeID=" & typeID.ToString & " AND regionID=" & regionID.ToString & ";"
    '    Dim recordsAffected As Integer = SetCustomData(priceSQL)
    '    If recordsAffected = -2 Then
    '        MessageBox.Show("There was an error deleting data from the Market Orders database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Deleting Market Orders Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    Else
    '        'MessageBox.Show("Successfully deleted " & recordsAffected.ToString("N0") & " market orders records.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' Routine to delete all rows in the market orders table
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Shared Sub DeleteAllMarketOrders()

    '    Const priceSQL As String = "DELETE FROM marketOrders;"
    '    Dim recordsAffected As Integer = SetCustomData(priceSQL)
    '    If recordsAffected = -2 Then
    '        MessageBox.Show("There was an error deleting data from the Market Orders database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Deleting Market Orders Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    Else
    '        'MessageBox.Show("Successfully deleted " & recordsAffected.ToString("N0") & " market orders records.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End If

    'End Sub

#End Region

End Class
