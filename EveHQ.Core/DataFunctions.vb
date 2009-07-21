' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Windows.Forms

Public Class DataFunctions

    Shared customMDBConnection As New OleDbConnection
    Shared customSQLConnection As New SqlConnection
    Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    Public Shared Function CreateEveHQDataDB() As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                ' Get the directory of the existing Access database to write the new one there
                Dim outputFile As String = ""
                If EveHQ.Core.HQ.EveHQSettings.DBDataFilename <> "" Then
                    outputFile = EveHQ.Core.HQ.EveHQSettings.DBDataFilename.Replace("\\", "\")
                    MessageBox.Show("Creating database using existing path: " & outputFile, "Custom Database Location", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    If EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = True Or EveHQ.Core.HQ.IsUsingLocalFolders = True Then
                        outputFile = (Path.Combine(EveHQ.Core.HQ.appFolder, "EveHQData.mdb"))
                        If EveHQ.Core.HQ.IsUsingLocalFolders = True Then
                            MessageBox.Show("/local switch active - Location: " & outputFile, "Custom Database Location", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            If EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = True Then
                                MessageBox.Show("Using application directory for database - Location: " & outputFile, "Custom Database Location", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End If
                    Else
                        outputFile = (Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.mdb"))
                        MessageBox.Show("Creating database in users EveHQ Applciation Data folder: " & outputFile, "Custom Database Location", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
                ' Try to create a new access db from resources
                Dim fs As New FileStream(outputFile, FileMode.Create)
                Dim bw As New BinaryWriter(fs)
                Try
                    bw.Write(My.Resources.EveHQDataDB)
                    bw.Close()
                    fs.Close()
                    EveHQ.Core.HQ.EveHQSettings.DBDataFilename = outputFile
                    EveHQ.Core.HQ.EveHQDataConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & outputFile
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = "Unable to create Access database in " & outputFile & ControlChars.CrLf & ControlChars.CrLf & e.Message
                    Return False
                Finally
                    fs.Dispose()
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                Dim strSQL As String = "CREATE DATABASE EveHQData;"
                Dim oldStrConn As String = EveHQ.Core.HQ.EveHQDataConnectionString
                ' Set new database connection string
                Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
                    Case 1
                        EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                        If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                            EveHQ.Core.HQ.EveHQDataConnectionString += "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                        Else
                            EveHQ.Core.HQ.EveHQDataConnectionString += "; Integrated Security = SSPI;"
                        End If
                    Case 2
                        EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & "\SQLEXPRESS"
                        If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                            EveHQ.Core.HQ.EveHQDataConnectionString += "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                        Else
                            EveHQ.Core.HQ.EveHQDataConnectionString += "; Integrated Security = SSPI;"
                        End If
                    Case 3
                        EveHQ.Core.HQ.EveHQDataConnectionString = "Data Source=" & EveHQ.Core.HQ.EveHQSettings.DBServer & ";User Id=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & ";Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                End Select
                If EveHQ.Core.DataFunctions.SetData(strSQL) = True Then
                    EveHQ.Core.HQ.EveHQSettings.DBDataName = "EveHQData"
                    EveHQ.Core.HQ.EveHQDataConnectionString = oldStrConn
                    Return True
                Else
                    EveHQ.Core.HQ.EveHQDataConnectionString = oldStrConn
                    Return False
                End If
        End Select
    End Function
    Public Shared Function GetDatabaseTables() As ArrayList
        Dim DBTables As New ArrayList
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                Dim conn As New OleDbConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim SchemaTable As DataTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, Nothing})
                    For table As Integer = 0 To SchemaTable.Rows.Count - 1
                        If SchemaTable.Rows(table)!TABLE_TYPE.ToString = "TABLE" Then
                            DBTables.Add(SchemaTable.Rows(table)!TABLE_NAME.ToString())
                        End If
                    Next
                    Return DBTables
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim SchemaTable As DataTable = conn.GetSchema("Tables")
                    For table As Integer = 0 To SchemaTable.Rows.Count - 1
                        DBTables.Add(SchemaTable.Rows(table)!TABLE_NAME.ToString())
                    Next
                    Return DBTables
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return Nothing
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
    Public Shared Function SetEveHQConnectionString() As Boolean

        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0
                If EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = False Then
                    EveHQ.Core.HQ.itemDBConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & EveHQ.Core.HQ.EveHQSettings.DBFilename
                Else
                    Try
                        Dim FI As New IO.FileInfo(EveHQ.Core.HQ.EveHQSettings.DBFilename)
                        EveHQ.Core.HQ.itemDBConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & Path.Combine(EveHQ.Core.HQ.appFolder, FI.Name)
                    Catch e As Exception
                        MessageBox.Show("There was an error setting the EveHQ connection string: " & e.Message, "Error Forming DB Connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
            Case 1
                EveHQ.Core.HQ.itemDBConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.itemDBConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.itemDBConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; Integrated Security = SSPI;"
                End If
            Case 2
                EveHQ.Core.HQ.itemDBConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & "\SQLEXPRESS"
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.itemDBConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.itemDBConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; Integrated Security = SSPI;"
                End If
            Case 3
                EveHQ.Core.HQ.itemDBConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & ";Database=" & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & ";Uid=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & ";Pwd=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
        End Select
        Return True

    End Function
    Public Shared Function SetEveHQDataConnectionString() As Boolean

        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0
                If EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = False Then
                    EveHQ.Core.HQ.EveHQDataConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & EveHQ.Core.HQ.EveHQSettings.DBDataFilename
                Else
                    Try
                        Dim FI As New IO.FileInfo(EveHQ.Core.HQ.EveHQSettings.DBDataFilename)
                        EveHQ.Core.HQ.EveHQDataConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & Path.Combine(EveHQ.Core.HQ.appFolder, FI.Name)
                    Catch e As Exception
                        MessageBox.Show("There was an error setting the EveHQData connection string: " & e.Message, "Error Forming DB Connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
            Case 1
                EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName.ToLower & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName.ToLower & "; Integrated Security = SSPI;"
                End If
            Case 2
                EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & "\SQLEXPRESS"
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName.ToLower & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName.ToLower & "; Integrated Security = SSPI;"
                End If
            Case 3
                EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & ";Database=" & EveHQ.Core.HQ.EveHQSettings.DBDataName.ToLower & ";Uid=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & ";Pwd=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
        End Select
        Return True

    End Function
    Public Shared Function OpenCustomDatabase() As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                customMDBConnection = New OleDbConnection
                customMDBConnection.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    customMDBConnection.Open()
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                customSQLConnection = New SqlConnection
                customSQLConnection.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    customSQLConnection.Open()
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                End Try
            Case Else
                EveHQ.Core.HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function
    Public Shared Function CloseCustomDatabase() As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                Try
                    If customMDBConnection.State = ConnectionState.Open Then
                        customMDBConnection.Close()
                    End If
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                Try
                    If customSQLConnection.State = ConnectionState.Open Then
                        customSQLConnection.Close()
                    End If
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                End Try
            Case Else
                EveHQ.Core.HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function
    Public Shared Function SetStaticData(ByVal strSQL As String) As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                Dim conn As New OleDbConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New OleDbCommand(strSQL, conn)
                    keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    keyCommand.ExecuteNonQuery()
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                Try
                    conn.Open()
                    If strSQL.Contains(" LIKE ") = False Then
                        strSQL = strSQL.Replace("'", "''")
                        strSQL = strSQL.Replace(ControlChars.Quote, "'")
                        strSQL = strSQL.Replace("=true", "=1")
                    End If
                    Dim keyCommand As New SqlCommand(strSQL, conn)
                    keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    keyCommand.ExecuteNonQuery()
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
    Public Shared Function SetData(ByVal strSQL As String) As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                Dim conn As New OleDbConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New OleDbCommand(strSQL, conn)
                    keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    keyCommand.ExecuteNonQuery()
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New SqlCommand(strSQL, conn)
                    keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    keyCommand.ExecuteNonQuery()
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
    Public Shared Function SetDataOnly(ByVal strSQL As String) As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                Try
                    Dim keyCommand As New OleDbCommand(strSQL, customMDBConnection)
                    keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    keyCommand.ExecuteNonQuery()
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                Try
                    Dim keyCommand As New SqlCommand(strSQL, customSQLConnection)
                    keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    keyCommand.ExecuteNonQuery()
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                End Try
            Case Else
                EveHQ.Core.HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function

    Public Shared Function GetData(ByVal strSQL As String) As DataSet

        Dim EveHQData As New DataSet
        EveHQData.Clear()
        EveHQData.Tables.Clear()

        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                Dim conn As New OleDbConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim da As New OleDbDataAdapter(strSQL, conn)
                    da.SelectCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                Try
                    conn.Open()
                    If strSQL.Contains(" LIKE ") = False Then
                        strSQL = strSQL.Replace("'", "''")
                        strSQL = strSQL.Replace(ControlChars.Quote, "'")
                        strSQL = strSQL.Replace("=true", "=1")
                    End If
                    Dim da As New SqlDataAdapter(strSQL, conn)
                    da.SelectCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return Nothing
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
    Public Shared Function GetCustomData(ByVal strSQL As String) As DataSet

        Dim EveHQData As New DataSet
        EveHQData.Clear()
        EveHQData.Tables.Clear()

        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                Dim conn As New OleDbConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim da As New OleDbDataAdapter(strSQL, conn)
                    da.SelectCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1, 2 ' MSSQL, MSSQL Express
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    If strSQL.Contains(" LIKE ") = False Then
                        strSQL = strSQL.Replace("'", "''")
                        strSQL = strSQL.Replace(ControlChars.Quote, "'")
                        strSQL = strSQL.Replace("=true", "=1")
                    End If
                    Dim da As New SqlDataAdapter(strSQL, conn)
                    da.SelectCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return Nothing
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
    Public Shared Function GetBPTypeID(ByVal typeID As String) As String
        Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invBlueprintTypes WHERE productTypeID=" & typeID & ";")
        If eveData.Tables(0).Rows.Count = 0 Then
            Return typeID
        Else
            typeID = eveData.Tables(0).Rows(0).Item("blueprintTypeID").ToString
            Return typeID
        End If
    End Function
    Public Shared Function GetTypeID(ByVal bpTypeID As String) As String
        Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invBlueprintTypes WHERE blueprintTypeID=" & bpTypeID & ";")
        If eveData.Tables(0).Rows.Count = 0 Then
            Return bpTypeID
        Else
            bpTypeID = eveData.Tables(0).Rows(0).Item("productTypeID").ToString
            Return bpTypeID
        End If
    End Function
    Public Shared Function GetTypeParentInfo(ByVal typeID As String) As String()
        Dim info(5) As String
        Dim strSQL As String = "SELECT invTypes.typeID, invTypes.typeName, invGroups.groupID, invGroups.groupName, invCategories.categoryID, invCategories.categoryName"
        strSQL &= " FROM (invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID"
        strSQL &= " WHERE invTypes.typeID=" & typeID & ";"
        Dim ds As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        info(0) = ds.Tables(0).Rows(0).Item("typeID").ToString.Trim
        info(1) = ds.Tables(0).Rows(0).Item("typeName").ToString.Trim
        info(2) = ds.Tables(0).Rows(0).Item("groupID").ToString.Trim
        info(4) = ds.Tables(0).Rows(0).Item("categoryID").ToString.Trim
        info(3) = ds.Tables(0).Rows(0).Item("groupName").ToString.Trim
        info(5) = ds.Tables(0).Rows(0).Item("categoryName").ToString.Trim
        Return info
    End Function
    Public Shared Function GetBPWF(ByVal typeID As String) As Double
        Dim BPWF As Double = 0
        Dim strSQL As String = "SELECT *"
        strSQL &= " FROM invBlueprintTypes"
        strSQL &= " WHERE blueprintTypeID=" & typeID & ";"
        Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        For col As Integer = 3 To eveData.Tables(0).Columns.Count - 1
            ' Check for BPWF
            If eveData.Tables(0).Columns(col).Caption = "wasteFactor" Then
                BPWF = Math.Round(CDbl(eveData.Tables(0).Rows(0).Item(col).ToString))
                Exit For
            End If
        Next
        If eveData IsNot Nothing Then
            eveData.Dispose()
        End If
        Return BPWF
    End Function
    Public Shared Function Round(ByVal data As String, Optional ByVal places As Integer = 6) As String
        If IsNumeric(data) = True Then
            Dim figure As Double = 0
            figure = Int((CDbl(data) * 10 ^ places) + 0.5) / 10 ^ places
            Return Format(figure, "#,###,##0.######")
        Else
            Return data
        End If
    End Function
    Public Shared Function LoadItemData() As Boolean
        Dim itemData As New DataSet
        Try
            EveHQ.Core.HQ.itemData.Clear()
            Dim strSQL As String = "SELECT invGroups.categoryID, invTypes.typeID, invTypes.groupID, invTypes.typeName, invTypes.volume, invTypes.portionSize, invTypes.published, invTypes.marketGroupID FROM invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID;"
            itemData = EveHQ.Core.DataFunctions.GetData(strSQL)
            Dim newItem As New EveItem
            If itemData IsNot Nothing Then
                If itemData.Tables(0).Rows.Count > 0 Then
                    For Each itemRow As DataRow In itemData.Tables(0).Rows
                        newItem = New EveItem
                        newItem.ID = CLng(itemRow.Item("typeID"))
                        newItem.Name = CStr(itemRow.Item("typeName"))
                        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
                            Case 0, 3 ' Access & MySQL
                                newItem.Group = CInt(itemRow.Item("groupID"))
                                newItem.Published = CBool(itemRow.Item("published"))
                            Case 1, 2 ' SQL
                                newItem.Group = CInt(itemRow.Item("groupID"))
                                newItem.Published = CBool(itemRow.Item("published"))
                        End Select
                        newItem.Category = CInt(itemRow.Item("categoryID"))
                        If IsDBNull(itemRow.Item("marketGroupID")) = False Then
                            newItem.MarketGroup = CInt(itemRow.Item("marketGroupID"))
                        Else
                            newItem.MarketGroup = 0
                        End If
                        newItem.Volume = CDbl(itemRow.Item("volume"))
                        newItem.PortionSize = CInt(itemRow.Item("portionSize"))
                        EveHQ.Core.HQ.itemData.Add(newItem.ID.ToString, newItem)
                    Next
                    ' Get the MetaLevel data
                    strSQL = "SELECT * FROM dgmTypeAttributes WHERE attributeID=633;"
                    itemData = EveHQ.Core.DataFunctions.GetData(strSQL)
                    If itemData.Tables(0).Rows.Count > 0 Then
                        For Each itemRow As DataRow In itemData.Tables(0).Rows
                            newItem = EveHQ.Core.HQ.itemData(CStr(itemRow.Item("typeID")))
                            If IsDBNull(itemRow.Item("valueInt")) = False Then
                                newItem.MetaLevel = CInt(itemRow.Item("valueInt"))
                            Else
                                newItem.MetaLevel = CInt(itemRow.Item("valueFloat"))
                            End If
                        Next
                        If itemData IsNot Nothing Then
                            itemData.Dispose()
                        End If
                        GC.Collect()
                        Return True
                    Else
                        If itemData IsNot Nothing Then
                            itemData.Dispose()
                        End If
                        Return False
                    End If
                Else
                    If itemData IsNot Nothing Then
                        itemData.Dispose()
                    End If
                    Return False
                End If
            Else
                If itemData IsNot Nothing Then
                    itemData.Dispose()
                End If
                Return False
            End If
        Catch ex As Exception
            If itemData IsNot Nothing Then
                itemData.Dispose()
            End If
            MessageBox.Show("Error Loading Item Data for Assets Plugin" & ControlChars.CrLf & ex.Message, "Assets Plug-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Shared Function LoadItems() As Boolean

        ' Initally load the new item data routine
        Call LoadItemData()

        EveHQ.Core.HQ.itemList.Clear()
        EveHQ.Core.HQ.itemGroups.Clear()
        EveHQ.Core.HQ.itemCats.Clear()
        EveHQ.Core.HQ.groupCats.Clear()
        Dim eveData As New DataSet
        Try
            Dim iKey As String = ""
            Dim iValue As String = ""
            Dim iParent As String = ""
            Dim iPublished As Boolean = False
            Dim iBasePrice As String = ""
            ' Load categories
            eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invCategories ORDER BY categoryName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iValue = eveData.Tables(0).Rows(item).Item("categoryName").ToString.Trim
                iKey = eveData.Tables(0).Rows(item).Item("categoryID").ToString.Trim
                EveHQ.Core.HQ.itemCats.Add(iKey, iValue)
            Next
            ' Load groups
            eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invGroups ORDER BY groupName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iValue = eveData.Tables(0).Rows(item).Item("groupName").ToString.Trim
                iKey = eveData.Tables(0).Rows(item).Item("groupID").ToString.Trim
                iParent = eveData.Tables(0).Rows(item).Item("categoryID").ToString.Trim
                EveHQ.Core.HQ.itemGroups.Add(iKey, iValue)
                EveHQ.Core.HQ.groupCats.Add(iKey, iParent)
            Next
            ' Load items
            eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invTypes ORDER BY typeName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iKey = eveData.Tables(0).Rows(item).Item("typeName").ToString.Trim
                iValue = eveData.Tables(0).Rows(item).Item("typeID").ToString.Trim
                iParent = eveData.Tables(0).Rows(item).Item("groupID").ToString.Trim
                iBasePrice = eveData.Tables(0).Rows(item).Item("basePrice").ToString.Trim
                iPublished = CBool(eveData.Tables(0).Rows(item).Item("published"))
                If EveHQ.Core.HQ.itemList.ContainsKey(iKey) = False Then
                    EveHQ.Core.HQ.itemList.Add(iKey, iValue)
                End If
                If EveHQ.Core.HQ.BasePriceList.Contains(iValue) = False Then
                    EveHQ.Core.HQ.BasePriceList.Add(iValue, iBasePrice)
                End If
            Next
            If EveHQ.Core.DataFunctions.LoadUnlocks = False Then
                If eveData IsNot Nothing Then
                    eveData.Dispose()
                End If
                Return False
            End If
            EveHQ.Core.DataFunctions.LoadMarketPricesFromDB()
            EveHQ.Core.DataFunctions.LoadCustomPricesFromDB()
            If eveData IsNot Nothing Then
                eveData.Dispose()
            End If
            Return True
        Catch e As Exception
            If eveData IsNot Nothing Then
                eveData.Dispose()
            End If
            Return False
            Exit Function
        End Try
    End Function
    Public Shared Function LoadUnlocks() As Boolean
        Dim skillIDs(), skillLevels() As String
        skillIDs = New String() {"182", "183", "184", "1285", "1289", "1290"}
        skillLevels = New String() {"277", "278", "279", "1286", "1287", "1288"}
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invTypes.typeID AS invTypeID, invTypes.groupID, invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, invTypes.published"
            strSQL &= " FROM invTypes INNER JOIN dgmTypeAttributes ON invTypes.typeID = dgmTypeAttributes.typeID"
            strSQL &= " WHERE (((dgmTypeAttributes.attributeID) IN (182,183,184,277,278,279,1285,1286,1287,1288,1289,1290)) AND ((invTypes.published)=true))"
            strSQL &= " ORDER BY invTypes.typeID, dgmTypeAttributes.attributeID;"
            Dim attData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            Dim lastAtt As String = "0"
            Dim skillIDLevel As String = ""
            Dim atts As Double = 0
            Dim itemList As New ArrayList
            Dim attValue As Double
            For row As Integer = 0 To attData.Tables(0).Rows.Count - 1
                If attData.Tables(0).Rows(row).Item("invTypeID").ToString <> lastAtt Then
                    Dim attRows() As DataRow = attData.Tables(0).Select("invTypeID=" & attData.Tables(0).Rows(row).Item("invtypeID").ToString)
                    Dim MaxPreReqs As Integer = 10
                    Dim PreReqSkills(MaxPreReqs) As String
                    Dim PreReqSkillLevels(MaxPreReqs) As Integer
                    For Each attRow As DataRow In attRows
                        If IsDBNull(attRow.Item("valueInt")) = False Then
                            attValue = CDbl(attRow.Item("valueInt"))
                        Else
                            attValue = CDbl(attRow.Item("valueFloat"))
                        End If
                        Select Case CInt(attRow.Item("attributeID"))
                            Case 182
                                PreReqSkills(1) = CStr(attValue)
                            Case 183
                                PreReqSkills(2) = CStr(attValue)
                            Case 184
                                PreReqSkills(3) = CStr(attValue)
                            Case 1285
                                PreReqSkills(4) = CStr(attValue)
                            Case 1289
                                PreReqSkills(5) = CStr(attValue)
                            Case 1290
                                PreReqSkills(6) = CStr(attValue)
                            Case 277
                                PreReqSkillLevels(1) = CInt(attValue)
                            Case 278
                                PreReqSkillLevels(2) = CInt(attValue)
                            Case 279
                                PreReqSkillLevels(3) = CInt(attValue)
                            Case 1286
                                PreReqSkillLevels(4) = CInt(attValue)
                            Case 1287
                                PreReqSkillLevels(5) = CInt(attValue)
                            Case 1288
                                PreReqSkillLevels(6) = CInt(attValue)
                        End Select
                    Next
                    For prereq As Integer = 1 To MaxPreReqs
                        If PreReqSkills(prereq) <> "" Then
                            skillIDLevel = PreReqSkills(prereq) & "." & PreReqSkillLevels(prereq).ToString
                            itemList.Add(skillIDLevel & "_" & attData.Tables(0).Rows(row).Item("invtypeID").ToString & "_" & attData.Tables(0).Rows(row).Item("groupID").ToString)
                        End If
                    Next
                    lastAtt = CStr(attData.Tables(0).Rows(row).Item("invtypeID"))
                End If
            Next

            ' Place the items into the Shared arrays
            Dim items(2) As String
            Dim itemUnlocked As New ArrayList
            Dim certUnlocked As New ArrayList
            EveHQ.Core.HQ.SkillUnlocks.Clear()
            EveHQ.Core.HQ.ItemUnlocks.Clear()
            For Each item As String In itemList
                items = item.Split(CChar("_"))
                If EveHQ.Core.HQ.SkillUnlocks.ContainsKey(items(0)) = False Then
                    ' Create an arraylist and add the item
                    itemUnlocked = New ArrayList
                    itemUnlocked.Add(items(1) & "_" & items(2))
                    EveHQ.Core.HQ.SkillUnlocks.Add(items(0), itemUnlocked)
                Else
                    ' Fetch the item and add the new one
                    itemUnlocked = EveHQ.Core.HQ.SkillUnlocks(items(0))
                    itemUnlocked.Add(items(1) & "_" & items(2))
                End If
                If EveHQ.Core.HQ.ItemUnlocks.ContainsKey(items(1)) = False Then
                    ' Create an arraylist and add the item
                    itemUnlocked = New ArrayList
                    itemUnlocked.Add(items(0))
                    EveHQ.Core.HQ.ItemUnlocks.Add(items(1), itemUnlocked)
                Else
                    ' Fetch the item and add the new one
                    itemUnlocked = EveHQ.Core.HQ.ItemUnlocks(items(1))
                    itemUnlocked.Add(items(0))
                End If
            Next
            ' Add certificates into the skill unlocks?
            For Each cert As EveHQ.Core.Certificate In EveHQ.Core.HQ.Certificates.Values
                For Each skill As String In cert.RequiredSkills.Keys
                    Dim skillID As String = skill & "." & cert.RequiredSkills(skill).ToString
                    If EveHQ.Core.HQ.CertUnlockSkills.ContainsKey(skillID) = False Then
                        ' Create an arraylist and add the item
                        certUnlocked = New ArrayList
                        certUnlocked.Add(cert.ID)
                        EveHQ.Core.HQ.CertUnlockSkills.Add(skillID, certUnlocked)
                    Else
                        ' Fetch the item and add the new one
                        certUnlocked = EveHQ.Core.HQ.CertUnlockSkills(skillID)
                        certUnlocked.Add(cert.ID)
                    End If
                Next
                For Each certID As String In cert.RequiredCerts.Keys
                    If EveHQ.Core.HQ.CertUnlockCerts.ContainsKey(certID) = False Then
                        ' Create an arraylist and add the item
                        certUnlocked = New ArrayList
                        certUnlocked.Add(cert.ID)
                        EveHQ.Core.HQ.CertUnlockCerts.Add(certID, certUnlocked)
                    Else
                        ' Fetch the item and add the new one
                        certUnlocked = EveHQ.Core.HQ.CertUnlockCerts(certID)
                        certUnlocked.Add(cert.ID)
                    End If
                Next
            Next
            Return True
        Catch e As Exception
            Return False
            Exit Function
        End Try
    End Function
    'Public Shared Function LoadMarketPrices() As Boolean
    '    Try
    '        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    '        EveHQ.Core.HQ.MarketPriceList.Clear()
    '        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\MarketPrices.txt") = True Then
    '            Dim sr As New IO.StreamReader(EveHQ.Core.HQ.cacheFolder & "\MarketPrices.txt")
    '            Dim marketLine As String = ""
    '            Dim price As Double
    '            Do
    '                marketLine = sr.ReadLine()
    '                Dim marketData() As String = marketLine.Split(",".ToCharArray)
    '                price = Double.Parse(CStr(marketData(1)), Globalization.NumberStyles.Number, culture)
    '                EveHQ.Core.HQ.MarketPriceList.Add(marketData(0), price)
    '            Loop Until sr.EndOfStream
    '        End If
    '    Catch ex As Exception
    '    End Try
    '    Return True
    'End Function
    'Public Shared Function LoadCustomPrices() As Boolean
    '    Try
    '        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    '        EveHQ.Core.HQ.CustomPriceList.Clear()
    '        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\CustomPrices.txt") = True Then
    '            Dim sr As New IO.StreamReader(EveHQ.Core.HQ.cacheFolder & "\CustomPrices.txt")
    '            Dim marketLine As String = ""
    '            Dim price As Double
    '            Do
    '                marketLine = sr.ReadLine()
    '                If marketLine IsNot Nothing Then
    '                    Dim marketData() As String = marketLine.Split(",".ToCharArray)
    '                    price = Double.Parse(CStr(marketData(1)), Globalization.NumberStyles.Number, culture)
    '                    EveHQ.Core.HQ.CustomPriceList.Add(marketData(0), price)
    '                End If
    '            Loop Until sr.EndOfStream
    '        End If
    '    Catch ex As Exception
    '    End Try
    '    Call LoadCustomPricesFromDB()
    '    Return True
    'End Function
    Public Shared Function GetPrice(ByVal itemID As String) As Double
        If itemID IsNot Nothing Then
            Try
                If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) = True Then
                    Return CDbl(EveHQ.Core.HQ.CustomPriceList(itemID))
                Else
                    If EveHQ.Core.HQ.MarketPriceList.ContainsKey(itemID) Then
                        Return CDbl(EveHQ.Core.HQ.MarketPriceList(itemID))
                    Else
                        If EveHQ.Core.HQ.BasePriceList.ContainsKey(itemID) Then
                            Return CDbl(EveHQ.Core.HQ.BasePriceList(itemID))
                        Else
                            Return 0
                        End If
                    End If
                End If
            Catch e As Exception
                Return CDbl(EveHQ.Core.HQ.BasePriceList(itemID))
            End Try
        Else
            Return 0
        End If
    End Function

#Region "MSSQL Data Conversion Routines"
    Public Shared Sub AddSQLRefiningData(ByVal connection As SqlConnection)
        Dim line As String = My.Resources.materialsForRefining.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("typeID") = False And line <> "" Then
                Dim strSQL As String = "INSERT INTO typeActivityMaterials (typeID,activityID,requiredTypeID,quantity,damagePerJob) VALUES(" & line & ");"
                Dim keyCommand As New SqlCommand(strSQL, connection)
                keyCommand.ExecuteNonQuery()
            End If
        Next
    End Sub
    Public Shared Sub AddSQLAttributeGroupColumn(ByVal connection As SqlConnection)
        Dim strSQL As String = "ALTER TABLE dgmAttributeTypes ADD attributeGroup INTEGER DEFAULT 0;"
        Dim keyCommand As New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        strSQL = "UPDATE dgmAttributeTypes SET attributeGroup=0;"
        keyCommand = New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        Dim line As String = My.Resources.attributeGroups.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("attributeID") = False And line <> "" Then
                Dim fields() As String = line.Split(",".ToCharArray)
                Dim strSQL2 As String = "UPDATE dgmAttributeTypes SET attributeGroup=" & fields(1) & " WHERE attributeID=" & fields(0) & ";"
                Dim keyCommand2 As New SqlCommand(strSQL2, connection)
                keyCommand2.ExecuteNonQuery()
            End If
        Next
    End Sub
    Public Shared Sub CorrectSQLEveUnits(ByVal connection As SqlConnection)
        Dim strSQL As String = "UPDATE dgmAttributeTypes SET unitID=122 WHERE unitID IS NULL;"
        Dim keyCommand As New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
    End Sub
#End Region ' Converts the Base CCP Data Export into something EveHQ can use

    Public Shared Function LoadMarketPricesFromDB() As Boolean
        Dim eveData As New DataSet
        Try
            eveData = EveHQ.Core.DataFunctions.GetCustomData("SELECT * FROM marketPrices ORDER BY typeID;")
            If eveData IsNot Nothing Then
                EveHQ.Core.HQ.MarketPriceList.Clear()
                For Each priceRow As DataRow In eveData.Tables(0).Rows
                    EveHQ.Core.HQ.MarketPriceList.Add(CStr(priceRow.Item("typeID")), CDbl(priceRow.Item("price")))
                Next
            Else
                ' Doesn't look like the table is there so try creating it
                Call CreateMarketPricesTable()
            End If
        Catch ex As Exception
            MessageBox.Show("There was an error fetching the Market Price data. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            If eveData IsNot Nothing Then
                eveData.Dispose()
            End If
        End Try
    End Function
    Public Shared Function LoadCustomPricesFromDB() As Boolean
        Dim eveData As New DataSet
        Try
            eveData = EveHQ.Core.DataFunctions.GetCustomData("SELECT * FROM customPrices ORDER BY typeID;")
            If eveData IsNot Nothing Then
                EveHQ.Core.HQ.CustomPriceList.Clear()
                For Each priceRow As DataRow In eveData.Tables(0).Rows
                    EveHQ.Core.HQ.CustomPriceList.Add(CStr(priceRow.Item("typeID")), CDbl(priceRow.Item("price")))
                Next
            Else
                ' Doesn't look like the table is there so try creating it
                Call CreateCustomPricesTable()
            End If
        Catch ex As Exception
            MessageBox.Show("There was an error fetching the Custom Price data. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            If eveData IsNot Nothing Then
                eveData.Dispose()
            End If
        End Try
    End Function
    Private Shared Function CreateCustomPricesTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("customPrices") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTable = True
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
            If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE customPrices")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  typeID         int,")
            strSQL.AppendLine("  price          float,")
            strSQL.AppendLine("  priceDate      datetime,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT customPrices_PK PRIMARY KEY CLUSTERED (typeID)")
            strSQL.AppendLine(")")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

    End Function
    Private Shared Function CreateMarketPricesTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("marketPrices") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTable = True
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
            If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE marketPrices")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  typeID         int,")
            strSQL.AppendLine("  price          float,")
            strSQL.AppendLine("  priceDate      datetime,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT marketPrices_PK PRIMARY KEY CLUSTERED (typeID)")
            strSQL.AppendLine(")")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

    End Function
    Public Shared Function SetCustomPrice(ByVal typeID As Long, ByVal UserPrice As Double, ByVal DBOpen As Boolean) As Boolean
        ' Store the user's price in the database
        If EveHQ.Core.HQ.CustomPriceList.Contains(typeID.ToString) = False Then
            ' Add the data
            If EveHQ.Core.DataFunctions.AddCustomPrice(typeID.ToString, UserPrice, DBOpen) = True Then
                Return True
            Else
                Return False
            End If
        Else
            ' Edit the data
            If EveHQ.Core.DataFunctions.EditCustomPrice(typeID.ToString, UserPrice, DBOpen) = True Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Shared Function AddCustomPrice(ByVal itemID As String, ByVal price As Double, ByVal DBOpen As Boolean) As Boolean
        EveHQ.Core.HQ.CustomPriceList(itemID) = price
        Dim priceSQL As String = "INSERT INTO customPrices (typeID, price, priceDate) VALUES (" & itemID & ", " & price.ToString(culture) & ", '" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "');"
        If DBOpen = False Then
            If EveHQ.Core.DataFunctions.SetData(priceSQL) = False Then
                MessageBox.Show("There was an error writing data to the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If EveHQ.Core.DataFunctions.SetDataOnly(priceSQL) = False Then
                MessageBox.Show("There was an error writing data to the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Shared Function EditCustomPrice(ByVal itemID As String, ByVal price As Double, ByVal DBOpen As Boolean) As Boolean
        EveHQ.Core.HQ.CustomPriceList(itemID) = price
        Dim priceSQL As String = "UPDATE customPrices SET price=" & price.ToString(culture) & ", priceDate='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' WHERE typeID=" & itemID & ";"
        If DBOpen = False Then
            If EveHQ.Core.DataFunctions.SetData(priceSQL) = False Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If EveHQ.Core.DataFunctions.SetDataOnly(priceSQL) = False Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Shared Function DeleteCustomPrice(ByVal itemID As String) As Boolean
        ' Double check it exists and delete it
        If EveHQ.Core.HQ.CustomPriceList.Contains(itemID) = True Then
            EveHQ.Core.HQ.CustomPriceList.Remove(itemID)
        End If
        Dim priceSQL As String = "DELETE FROM customPrices WHERE typeID=" & itemID & ";"
        If EveHQ.Core.DataFunctions.SetData(priceSQL) = False Then
            MessageBox.Show("There was an error deleting data from the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function SetMarketPrice(ByVal typeID As Long, ByVal UserPrice As Double, ByVal DBOpen As Boolean) As Boolean
        ' Store the user's price in the database
        If EveHQ.Core.HQ.MarketPriceList.Contains(typeID.ToString) = False Then
            ' Add the data
            If EveHQ.Core.DataFunctions.AddMarketPrice(typeID.ToString, UserPrice, DBOpen) = True Then
                Return True
            Else
                Return False
            End If
        Else
            ' Edit the data
            If EveHQ.Core.DataFunctions.EditMarketPrice(typeID.ToString, UserPrice, DBOpen) = True Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Shared Function AddMarketPrice(ByVal itemID As String, ByVal price As Double, ByVal DBOpen As Boolean) As Boolean
        EveHQ.Core.HQ.MarketPriceList(itemID) = price
        Dim priceSQL As String = "INSERT INTO marketPrices (typeID, price, priceDate) VALUES (" & itemID & ", " & price.ToString(culture) & ", '" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "');"
        If DBOpen = False Then
            If EveHQ.Core.DataFunctions.SetData(priceSQL) = False Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If EveHQ.Core.DataFunctions.SetDataOnly(priceSQL) = False Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Shared Function EditMarketPrice(ByVal itemID As String, ByVal price As Double, ByVal DBOpen As Boolean) As Boolean
        EveHQ.Core.HQ.MarketPriceList(itemID) = price
        Dim priceSQL As String = "UPDATE marketPrices SET price=" & price.ToString(culture) & ", priceDate = '" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "' WHERE typeID=" & itemID & ";"
        If DBOpen = False Then
            If EveHQ.Core.DataFunctions.SetData(priceSQL) = False Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If EveHQ.Core.DataFunctions.SetDataOnly(priceSQL) = False Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Shared Function DeleteMarketPrice(ByVal itemID As String) As Boolean
        ' Double check it exists and delete it
        If EveHQ.Core.HQ.MarketPriceList.Contains(itemID) = True Then
            EveHQ.Core.HQ.MarketPriceList.Remove(itemID)
        End If
        Dim priceSQL As String = "DELETE FROM marketPrices WHERE typeID=" & itemID & ";"
        If EveHQ.Core.DataFunctions.SetData(priceSQL) = False Then
            MessageBox.Show("There was an error deleting data from the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function ProcessMarketExportFile(ByVal orderFile As String) As ArrayList

        Dim orderFI As New FileInfo(orderFile)
        Dim orderdate As Date = Now
        Dim items As New SortedList
        Dim itemOrders As New ArrayList
        Dim FileInUse As Boolean = False
        Dim sr As StreamReader = Nothing
        Dim PriceData As New ArrayList
        Do
            Try
                sr = New StreamReader(orderFile)
                FileInUse = False
            Catch ex As Exception
                FileInUse = True
            End Try
        Loop Until FileInUse = False
        Dim header As String = sr.ReadLine()

        If header <> "price,volRemaining,typeID,range,orderID,volEntered,minVolume,bid,issued,duration,stationID,regionID,solarSystemID,jumps," Then
            MessageBox.Show("File is not a valid Eve Market Export file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Else
            Dim order As String = ""
            Dim orders As Long = 0
            Dim orderDetails() As String
            Do
                order = sr.ReadLine
                orderDetails = order.Split(",".ToCharArray)
                ' Add to the relevant item list
                If items.Contains(orderDetails(2).Trim) = False Then
                    itemOrders = New ArrayList
                    itemOrders.Add(order)
                    items.Add(orderDetails(2).Trim, itemOrders)
                Else
                    itemOrders = CType(items(orderDetails(2).Trim), ArrayList)
                    itemOrders.Add(order)
                End If
                orders += 1
            Loop Until sr.EndOfStream
            sr.Close()


            ' Calculate global statistics
            Dim itemCount As Integer = items.Count
            Dim count As Integer = 0
            For Each item As String In items.Keys
                count += 1
                PriceData = CalculateMarketExportStats(CType(items(item), ArrayList), orderdate)
            Next

            items.Clear() : items = Nothing
            itemOrders.Clear() : itemOrders = Nothing
            GC.Collect()

            Return PriceData

        End If
    End Function
    Private Shared Function CalculateMarketExportStats(ByVal orderList As ArrayList, ByVal orderDate As Date) As ArrayList
        Dim orderDetails(), oDate As String
        Dim oReg, oSys, oStation As Long
        Dim oID, oRange, oVolEntered, oJumps As Long
        Dim oDuration As Integer
        Dim oTypeID, oType, oMinVol, oVol As Long
        Dim oPrice As Double
        Dim avgBuy, avgSell, avgAll As Double
        Dim medBuy, medSell, medAll As Double
        Dim stdBuy, stdSell, stdAll As Double
        Dim sorBuy, sorSell, sorAll As New SortedList
        Dim countBuy, countSell, countAll As Integer
        Dim volBuy, volSell, volAll As Long
        Dim minBuy, minSell, minAll As Double
        Dim maxBuy, maxSell, maxAll As Double
        Dim valBuy, valSell, valAll As Double
        Dim devBuy, devSell, devAll As Double
        Dim cumVol As Long = 0
        Dim ProcessOrder As Boolean = True

        Dim regions, systems As New SortedList
        Dim regOrders, sysOrders As New ArrayList
        sorBuy.Clear() : sorSell.Clear() : sorAll.Clear()

        countBuy = 0 : countSell = 0 : countAll = 0
        volBuy = 0 : volSell = 0 : volAll = 0
        minBuy = 0 : minSell = 0 : minAll = 0
        maxBuy = 0 : maxSell = 0 : maxAll = 0
        valBuy = 0 : valSell = 0 : valAll = 0
        devBuy = 0 : devSell = 0 : devAll = 0

        For Each order As String In orderList
            orderDetails = order.Split(",".ToCharArray)

            oPrice = CDbl(orderDetails(0).Trim)
            oVol = CLng(orderDetails(1).Trim)
            oTypeID = CLng(orderDetails(2).Trim)
            oRange = CLng(orderDetails(3).Trim)
            oID = CLng(orderDetails(4).Trim)
            oVolEntered = CLng(orderDetails(5).Trim)
            oMinVol = CLng(orderDetails(6).Trim)
            oType = Math.Abs(CLng(CBool(orderDetails(7).Trim)))
            oDate = CStr(orderDetails(8).Trim)
            oDuration = CInt(orderDetails(9).Trim)
            oStation = CLng(orderDetails(10).Trim)
            oReg = CLng(orderDetails(11).Trim)
            oSys = CLng(orderDetails(12).Trim)
            oJumps = CInt(orderDetails(13).Trim)

            ' Check if we process this
            ProcessOrder = True
            If oType = 0 Then ' Sell Order
                If EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrders = True And oPrice > (EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrderLimit * CDbl(EveHQ.Core.HQ.BasePriceList(oTypeID.ToString))) Then
                    ProcessOrder = False
                End If
            Else ' Buy Order
                If EveHQ.Core.HQ.EveHQSettings.IgnoreBuyOrders = True And oPrice < EveHQ.Core.HQ.EveHQSettings.IgnoreBuyOrderLimit Then
                    ProcessOrder = False
                End If
            End If

            If ProcessOrder = True Then

                countAll += 1
                volAll += oVol
                If oPrice < minAll Or minAll = 0 Then
                    minAll = oPrice
                End If
                If oPrice > maxAll Then
                    maxAll = oPrice
                End If
                valAll += (oVol * oPrice)
                If sorAll.Contains(oPrice.ToString) = False Then
                    sorAll.Add(oPrice.ToString, oVol)
                Else
                    sorAll(oPrice.ToString) = CLng(sorAll(oPrice.ToString)) + oVol
                End If
                devAll += Math.Pow(oPrice, 2) * oVol

                Select Case oType
                    Case 0 ' Sell order
                        countBuy += 1
                        volSell += oVol
                        If oPrice < minSell Or minSell = 0 Then
                            minSell = oPrice
                        End If
                        If oPrice > maxSell Then
                            maxSell = oPrice
                        End If
                        valSell += (oVol * oPrice)
                        If sorSell.Contains(oPrice.ToString) = False Then
                            sorSell.Add(oPrice.ToString, oVol)
                        Else
                            sorSell(oPrice.ToString) = CLng(sorSell(oPrice.ToString)) + oVol
                        End If
                        devSell += Math.Pow(oPrice, 2) * oVol
                    Case 1 ' Buy order
                        countSell += 1
                        volBuy += oVol
                        If oPrice < minBuy Or minBuy = 0 Then
                            minBuy = oPrice
                        End If
                        If oPrice > maxBuy Then
                            maxBuy = oPrice
                        End If
                        valBuy += (oVol * oPrice)
                        If sorBuy.Contains(oPrice.ToString) = False Then
                            sorBuy.Add(oPrice.ToString, oVol)
                        Else
                            sorBuy(oPrice.ToString) = CLng(sorBuy(oPrice.ToString)) + oVol
                        End If
                        devBuy += Math.Pow(oPrice, 2) * oVol
                End Select
            End If
        Next

        ' Calculate Averages, Standard Deviations & Medians
        If volAll > 0 Then
            avgAll = valAll / volAll
            stdAll = Math.Sqrt(Math.Abs((devAll / volAll) - Math.Pow(avgAll, 2)))
            cumVol = 0
            For Each chkVol As String In sorAll.Keys
                cumVol += CLng(sorAll(chkVol))
                If cumVol >= (volAll / 2) Then
                    medAll = CDbl(chkVol)
                    Exit For
                End If
            Next
        Else
            avgAll = 0 : stdAll = 0 : medAll = 0
        End If
        If volSell > 0 Then
            avgSell = valSell / volSell
            stdSell = Math.Sqrt(Math.Abs((devSell / volSell) - Math.Pow(avgSell, 2)))
            cumVol = 0
            For Each chkVol As String In sorSell.Keys
                cumVol += CLng(sorSell(chkVol))
                If cumVol >= (volSell / 2) Then
                    medSell = CDbl(chkVol)
                    Exit For
                End If
            Next
        Else
            avgSell = 0 : stdSell = 0 : medSell = 0
        End If
        If volBuy > 0 Then
            avgBuy = valBuy / volBuy
            stdBuy = Math.Sqrt(Math.Abs((devBuy / volBuy) - Math.Pow(avgBuy, 2)))
            cumVol = 0
            For Each chkVol As String In sorBuy.Keys
                cumVol += CLng(sorBuy(chkVol))
                If cumVol >= (volBuy / 2) Then
                    medBuy = CDbl(chkVol)
                    Exit For
                End If
            Next
        Else
            avgBuy = 0 : stdBuy = 0 : medBuy = 0
        End If

        'Calculate the user price
        Dim priceArray As New ArrayList
        priceArray.Add(avgBuy) : priceArray.Add(medBuy) : priceArray.Add(minBuy) : priceArray.Add(maxBuy)
        priceArray.Add(avgSell) : priceArray.Add(medSell) : priceArray.Add(minSell) : priceArray.Add(maxSell)
        priceArray.Add(avgAll) : priceArray.Add(medAll) : priceArray.Add(minAll) : priceArray.Add(maxAll)
        priceArray.Add(CalculateUserPrice(priceArray)) : priceArray.Add(oTypeID)
        priceArray.Add(volBuy) : priceArray.Add(volSell) : priceArray.Add(volAll)

        Return priceArray
    End Function
    Public Shared Function CalculateUserPrice(ByVal priceArray As ArrayList) As Double
        'EveHQ.Core.HQ.EveHQSettings.PriceCriteria(idx) = chk.Checked
        Dim price As Double = 0
        Dim count As Double = 0
        For crit As Integer = 0 To 11
            If EveHQ.Core.HQ.EveHQSettings.PriceCriteria(crit) = True Then
                count += 1
                price += CDbl(priceArray(crit))
            End If
        Next
        Return CDbl(price / count)
    End Function

End Class
