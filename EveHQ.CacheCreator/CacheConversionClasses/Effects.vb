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

'CREATE TABLE [dbo].[dgmEffects](
'	[effectID] [smallint] NOT NULL,
'	[effectName] [varchar](400) NULL,
'	[effectCategory] [smallint] NULL,
'	[preExpression] [int] NULL,
'	[postExpression] [int] NULL,
'	[description] [varchar](1000) NULL,
'	[guid] [varchar](60) NULL,
'	[iconID] [int] NULL,
'	[isOffensive] [bit] NULL,
'	[isAssistance] [bit] NULL,
'	[durationAttributeID] [smallint] NULL,
'	[trackingSpeedAttributeID] [smallint] NULL,
'	[dischargeAttributeID] [smallint] NULL,
'	[rangeAttributeID] [smallint] NULL,
'	[falloffAttributeID] [smallint] NULL,
'	[disallowAutoRepeat] [bit] NULL,
'	[published] [bit] NULL,
'	[displayName] [varchar](100) NULL,
'	[isWarpSafe] [bit] NULL,
'	[rangeChance] [bit] NULL,
'	[electronicChance] [bit] NULL,
'	[propulsionChance] [bit] NULL,
'	[distribution] [tinyint] NULL,
'	[sfxName] [varchar](20) NULL,
'	[npcUsageChanceAttributeID] [smallint] NULL,
'	[npcActivationChanceAttributeID] [smallint] NULL,
'	[fittingUsageChanceAttributeID] [smallint] NULL,
' CONSTRAINT [dgmEffects_PK] PRIMARY KEY CLUSTERED 
'(
'	[effectID] ASC
')WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
') ON [PRIMARY]

Imports System.Reflection
Imports System.Data.SqlClient
Imports System.Data.SQLite

Namespace CacheConversionClasses

    Public Class Effects

        Friend Shared dataCollection As New List(Of Effect)
        Const tableName As String = "dgmEffects"
        Const tableColumns As Integer = 27
        Const deleteSQL As String = "DELETE FROM " & tableName & ";"
        Const createSQL As String = "DROP TABLE IF EXISTS " & tableName & "; CREATE TABLE " & tableName & "(effectID int NOT NULL, effectName text NULL, effectCategory int NULL, preExpression int NULL, postExpression int NULL, description text NULL, guid text NULL, iconID int NULL, isOffensive boolean NULL, isAssistance boolean NULL, durationAttributeID int NULL, trackingSpeedAttributeID int NULL, dischargeAttributeID int NULL, rangeAttributeID int NULL, falloffAttributeID int NULL, disallowAutoRepeat boolean NULL, published boolean NULL, displayName text NULL, isWarpSafe boolean NULL, rangeChance boolean NULL, electronicChance boolean NULL, propulsionChance boolean NULL, distribution int NULL, sfxName text NULL, npcUsageChanceAttributeID int NULL, npcActivationChanceAttributeID int NULL, fittingUsageChanceAttributeID int NULL);"
        Const insertSQL As String = "INSERT INTO " & tableName & "(effectID, effectName, effectCategory, preExpression, postExpression, description, guid, iconID, isOffensive, isAssistance, durationAttributeID, trackingSpeedAttributeID, dischargeAttributeID, rangeAttributeID, falloffAttributeID, disallowAutoRepeat, published, displayName, isWarpSafe, rangeChance, electronicChance, propulsionChance, distribution, sfxName, npcUsageChanceAttributeID, npcActivationChanceAttributeID, fittingUsageChanceAttributeID) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);"
        Const fetchSQL As String = "SELECT * FROM " & tableName & ";"

        Public Shared Sub UpdateMasterDB(SQLDBCS As String, SQLiteDBCS As String, result As List(Of Object))

            ' First, convert the cache paarsing result to a collection
            Call ConvertCacheResultToClass(result)

            ' Now, update the SQLite DB
            Call UpdateSQLiteDB(SQLiteDBCS)

            ' Finally, update the main SQLDB
            Call UpdateSQLDB(SQLiteDBCS, SQLDBCS)

            ' Tidy up
            dataCollection.Clear()

        End Sub

        Private Shared Sub ConvertCacheResultToClass(result As List(Of Object))

            ' Parse the result
            For Each row As Object In result

                ' Get the list of key/values associated with the data
                For Each keyValuePair As Dictionary(Of Object, Object) In CType(CType(row, Tuple(Of Object)).Item1, List(Of Object))

                    ' Create a new instance
                    Dim data As New Effect

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
                            For Each data As Effect In dataCollection

                                ' Add the database values and execute the query
                                sqlCmd.Parameters(0).Value = data.effectID
                                sqlCmd.Parameters(1).Value = data.effectName
                                sqlCmd.Parameters(2).Value = data.effectCategory
                                sqlCmd.Parameters(3).Value = data.preExpression
                                sqlCmd.Parameters(4).Value = data.postExpression
                                sqlCmd.Parameters(5).Value = data.description
                                sqlCmd.Parameters(6).Value = DBNull.Value
                                sqlCmd.Parameters(7).Value = data.iconID
                                sqlCmd.Parameters(8).Value = data.isOffensive
                                sqlCmd.Parameters(9).Value = data.isAssistance
                                sqlCmd.Parameters(10).Value = data.durationAttributeID
                                sqlCmd.Parameters(11).Value = data.trackingSpeedAttributeID
                                sqlCmd.Parameters(12).Value = data.dischargeAttributeID
                                sqlCmd.Parameters(13).Value = data.rangeAttributeID
                                sqlCmd.Parameters(14).Value = data.falloffAttributeID
                                sqlCmd.Parameters(15).Value = data.disallowAutoRepeat
                                sqlCmd.Parameters(16).Value = data.published
                                sqlCmd.Parameters(17).Value = data.displayName
                                sqlCmd.Parameters(18).Value = data.isWarpSafe
                                sqlCmd.Parameters(19).Value = data.rangeChance
                                sqlCmd.Parameters(20).Value = data.electronicChance
                                sqlCmd.Parameters(21).Value = data.propulsionChance
                                sqlCmd.Parameters(22).Value = data.distribution
                                sqlCmd.Parameters(23).Value = data.sfxName
                                sqlCmd.Parameters(24).Value = data.npcUsageChanceAttributeID
                                sqlCmd.Parameters(25).Value = data.npcActivationChanceAttributeID
                                sqlCmd.Parameters(26).Value = data.fittingUsageChanceAttributeID
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
    ''' Class to hold an instance of a EffectType
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Effect
        Public Property effectID As Integer
        Public Property effectName As String
        Public Property effectCategory As Integer
        Public Property preExpression As Integer
        Public Property postExpression As Integer
        Public Property description As String
        Public Property guid As String
        Public Property iconID As Integer
        Public Property isOffensive As Boolean
        Public Property isAssistance As Boolean
        Public Property durationAttributeID As Integer
        Public Property trackingSpeedAttributeID As Integer
        Public Property dischargeAttributeID As Integer
        Public Property rangeAttributeID As Integer
        Public Property falloffAttributeID As Integer
        Public Property disallowAutoRepeat As Boolean
        Public Property published As Boolean
        Public Property displayName As String
        Public Property isWarpSafe As Boolean
        Public Property rangeChance As Boolean
        Public Property electronicChance As Boolean
        Public Property propulsionChance As Boolean
        Public Property distribution As Integer
        Public Property sfxName As String
        Public Property npcUsageChanceAttributeID As Integer
        Public Property npcActivationChanceAttributeID As Integer
        Public Property fittingUsageChanceAttributeID As Integer
    End Class

End Namespace
