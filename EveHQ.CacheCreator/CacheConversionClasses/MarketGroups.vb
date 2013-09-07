﻿' ========================================================================
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

'CREATE TABLE [dbo].[invMarketGroups](
'	[marketGroupID] [int] NOT NULL,
'	[parentGroupID] [int] NULL,
'	[marketGroupName] [nvarchar](100) NULL,
'	[description] [nvarchar](3000) NULL,
'	[iconID] [int] NULL,
'	[hasTypes] [bit] NULL,
' CONSTRAINT [invMarketGroups_PK] PRIMARY KEY CLUSTERED 
'(
'	[marketGroupID] ASC
')WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
') ON [PRIMARY]

Imports System.Reflection
Imports System.Data.SqlClient
Imports System.Data.SQLite

Namespace CacheConversionClasses

    Public Class MarketGroups

        Friend Shared dataCollection As New List(Of MarketGroup)
        Const tableName As String = "invMarketGroups"
        Const tableColumns As Integer = 6
        Const deleteSQL As String = "DELETE FROM " & tableName & ";"
        Const createSQL As String = "DROP TABLE IF EXISTS " & tableName & "; CREATE TABLE " & tableName & "(marketGroupID int NOT NULL, parentGroupID int NULL, marketGroupName text NULL, description text NULL, iconID int NULL, hasTypes boolean NULL);"
        Const insertSQL As String = "INSERT INTO " & tableName & "(marketGroupID, parentGroupID, marketGroupName, description, iconID, hasTypes) VALUES (?,?,?,?,?,?);"
        Const fetchSQL As String = "SELECT * FROM " & tableName & ";"

        Public Shared Sub UpdateMasterDB(SQLDBCS As String, SQLiteDBCS As String, result As Dictionary(Of Object, Object))

            ' First, convert the cache paarsing result to a collection
            Call ConvertCacheResultToClass(result)

            ' Now, update the SQLite DB
            Call UpdateSQLiteDB(SQLiteDBCS)

            ' Finally, update the main SQLDB
            Call UpdateSQLDB(SQLiteDBCS, SQLDBCS)

            ' Tidy up
            dataCollection.Clear()

        End Sub

        Private Shared Sub ConvertCacheResultToClass(result As Dictionary(Of Object, Object))

            ' Parse the result
            For Each row As Object In result.Values

                ' Get the list of key/values associated with the data
                For Each keyValuePair As Dictionary(Of Object, Object) In CType(row, List(Of Object))

                    ' Create a new instance
                    Dim data As New MarketGroup

                    ' Cycle through the keys
                    For Each key As String In keyValuePair.Keys
                        Dim type As Type = data.GetType()
                        Dim prop As PropertyInfo = type.GetProperty(key)
                        If prop IsNot Nothing Then
                            prop.SetValue(data, keyValuePair(key), Nothing)
                        End If
                    Next

                    ' Add the instance to the collection
                    dataCollection.Add(data)
                Next

            Next

        End Sub

        Private Shared Function UpdateSQLiteDB(DBCS As String) As Integer

            ' Procedure
            ' 1. Create new database table
            ' 2. Add new records to the table in bulk

            ' Step 1: Create new database table
            Dim delConn As New SQLiteConnection
            delConn.ConnectionString = DBCS
            delConn.Open()
            Try
                Dim keyCommand As New SQLiteCommand(createSQL, delConn)
                keyCommand.ExecuteNonQuery()
                If delConn.State = ConnectionState.Open Then
                    delConn.Close()
                End If
            Catch e As Exception
                ' Table already exists?
            End Try

            ' Step 2: Add new records to the data base
            Dim recordsAffected As Integer
            Using connection As New SQLiteConnection(DBCS)

                Try
                    connection.Open()

                    Using sqlCmd As SQLiteCommand = connection.CreateCommand
                        Using sqlTrans As SQLiteTransaction = connection.BeginTransaction

                            ' Create the DB Command text
                            sqlCmd.CommandText = insertSQL

                            ' Create the desired number of parameters
                            For col As Integer = 1 To tableColumns
                                sqlCmd.Parameters.Add(sqlCmd.CreateParameter)
                            Next

                            ' Run through the wallet journal collection and update where relevant
                            For Each data As MarketGroup In dataCollection

                                ' Add the database values and execute the query
                                sqlCmd.Parameters(0).Value = data.marketGroupID
                                sqlCmd.Parameters(1).Value = data.parentGroupID
                                sqlCmd.Parameters(2).Value = data.marketGroupName
                                sqlCmd.Parameters(3).Value = data.description
                                sqlCmd.Parameters(4).Value = data.iconID
                                sqlCmd.Parameters(5).Value = data.hasTypes
                                recordsAffected += sqlCmd.ExecuteNonQuery()

                            Next

                            ' Commit to the database
                            sqlTrans.Commit()

                        End Using
                    End Using

                    ' Close the connection and return the number of rows affected
                    connection.Close()
                    Return recordsAffected

                Catch e As Exception
                    Return -2
                Finally
                    If connection.State = ConnectionState.Open Then
                        connection.Close()
                    End If
                End Try

            End Using

        End Function

        Private Shared Sub UpdateSQLDB(SQLiteDBCS As String, SQLDBCS As String)

            ' Procedure
            ' 1. Read data from the SQLite DB
            ' 2. Delete all records from the SQL DB
            ' 3. BulkCopy data to the SQL DB

            ' Step 1: Read all data from the SQLite DB
            Using ds As New DataSet
                Using conn As New SQLiteConnection(SQLiteDBCS)
                    Try
                        conn.Open()
                        Dim da As New SQLiteDataAdapter(fetchSQL, conn)
                        da.Fill(ds, "Data")
                        conn.Close()
                    Catch e As Exception
                        ' Exit Sub
                    Finally
                        If conn.State = ConnectionState.Open Then
                            conn.Close()
                        End If
                    End Try
                End Using

                ' Step 2: Delete all existing records
                Using conn As New SqlConnection(SQLDBCS)
                    conn.Open()
                    Dim keyCommand As New SqlCommand(deleteSQL, conn)
                    keyCommand.ExecuteNonQuery()
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Using

                ' Step 3: BulkCopy data to the SQL DB
                Using conn As New SqlConnection(SQLDBCS)
                    Try
                        conn.Open()
                        Using copy As New SqlBulkCopy(SQLDBCS)
                            copy.DestinationTableName = tableName
                            For col As Integer = 0 To tableColumns - 1
                                copy.ColumnMappings.Add(col, col)
                            Next
                            copy.WriteToServer(ds.Tables(0))
                        End Using
                    Catch ex As Exception

                    End Try
                End Using
            End Using

        End Sub

    End Class

    ''' <summary>
    ''' Class to hold an instance of a MarketGroup
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MarketGroup
        Public Property marketGroupID As Integer
        Public Property parentGroupID As Integer
        Public Property marketGroupName As String
        Public Property description As String
        Public Property iconID As Integer
        Public Property hasTypes As Boolean
    End Class

End Namespace
