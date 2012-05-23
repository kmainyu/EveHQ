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
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports System.Data.SqlServerCe
Imports System.Runtime.Serialization.Formatters.Binary

Public Class DataFunctions

    Shared customSQLCEConnection As New SqlCeConnection
    Shared customSQLConnection As New SqlConnection
    Shared IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Shared SQLTimeFormat As String = "yyyyMMdd HH:mm:ss"
    Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Shared LastCacheRefresh As String = "2.8.0.3862"

    Public Shared Function CreateEveHQDataDB() As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                ' Get the directory of the existing SQL CE database to write the new one there
                Dim outputFile As String = ""
                outputFile = EveHQ.Core.HQ.EveHQSettings.DBDataFilename.Replace("\\", "\")
                'MessageBox.Show("Creating database using path: " & outputFile, "Custom Database Location", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Try to create a new SQL CE DB
                Dim strConnection As String = "Data Source = " & ControlChars.Quote & outputFile & ControlChars.Quote & ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Try
                    Dim SQLCE As New SqlCeEngine(strConnection)
                    SQLCE.CreateDatabase()
                    EveHQ.Core.HQ.EveHQSettings.DBDataFilename = outputFile
                    EveHQ.Core.HQ.EveHQDataConnectionString = strConnection
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = "Unable to create SQL CE database in " & outputFile & ControlChars.CrLf & ControlChars.CrLf & e.Message
                    Return False
                End Try
            Case 1 ' MSSQL
                Dim strSQL As String = "CREATE DATABASE EveHQData;"
                Dim oldStrConn As String = EveHQ.Core.HQ.EveHQDataConnectionString
                ' Set new database connection string
                EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Integrated Security = SSPI;"
                End If
                If EveHQ.Core.DataFunctions.SetData(strSQL) <> -1 Then
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
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    'Dim SchemaTable As DataTable = conn.GetSchema()
                    Dim schemaTable As DataSet = EveHQ.Core.DataFunctions.GetCustomData("SELECT * FROM INFORMATION_SCHEMA.TABLES")
                    For table As Integer = 0 To schemaTable.Tables(0).Rows.Count - 1
                        DBTables.Add(schemaTable.Tables(0).Rows(table).Item("TABLE_NAME").ToString)
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
            Case 1 ' MSSQL
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
            Case 0 ' SQL CE
                If EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = False Then
                    EveHQ.Core.HQ.itemDBConnectionString = "Data Source = " & ControlChars.Quote & EveHQ.Core.HQ.EveHQSettings.DBFilename & ControlChars.Quote & ";" & "Max Database Size = 512; ; Max Buffer Size = 2048;"
                Else
                    Try
                        Dim FI As New IO.FileInfo(EveHQ.Core.HQ.EveHQSettings.DBFilename)
                        EveHQ.Core.HQ.itemDBConnectionString = "Data Source = " & ControlChars.Quote & Path.Combine(EveHQ.Core.HQ.appFolder, FI.Name) & ControlChars.Quote & ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                    Catch e As Exception
                        MessageBox.Show("There was an error setting the EveHQ connection string: " & e.Message, "Error Forming DB Connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
            Case 1 ' SQL
                EveHQ.Core.HQ.itemDBConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.itemDBConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.itemDBConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; Integrated Security = SSPI;"
                End If
        End Select
        Return True
    End Function
    Public Shared Function SetEveHQDataConnectionString() As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                If EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = False Then
                    EveHQ.Core.HQ.EveHQDataConnectionString = "Data Source = " & ControlChars.Quote & EveHQ.Core.HQ.EveHQSettings.DBDataFilename & ControlChars.Quote & ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Else
                    Try
                        Dim FI As New IO.FileInfo(EveHQ.Core.HQ.EveHQSettings.DBDataFilename)
                        EveHQ.Core.HQ.EveHQDataConnectionString = "Data Source = " & ControlChars.Quote & Path.Combine(EveHQ.Core.HQ.appFolder, FI.Name) & ControlChars.Quote & ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                    Catch e As Exception
                        MessageBox.Show("There was an error setting the EveHQData connection string: " & e.Message, "Error Forming DB Connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
            Case 1 ' SQL
                EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName.ToLower & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName.ToLower & "; Integrated Security = SSPI;"
                End If
        End Select
        Return True
    End Function
    Public Shared Function OpenCustomDatabase() As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                customSQLCEConnection = New SqlCeConnection
                customSQLCEConnection.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    customSQLCEConnection.Open()
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                End Try
            Case 1 ' MSSQL
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
            Case 0 ' SQL CE
                Try
                    If customSQLCEConnection.State = ConnectionState.Open Then
                        customSQLCEConnection.Close()
                    End If
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    Return False
                End Try
            Case 1 ' MSSQL
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
        If strSQL.Contains(" LIKE ") = False Then
            strSQL = strSQL.Replace("'", "''")
            strSQL = strSQL.Replace(ControlChars.Quote, "'")
            strSQL = strSQL.Replace("=true", "=1")
        End If
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New SqlCeCommand(strSQL, conn)
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
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
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
    Public Shared Function SetData(ByVal strSQL As String) As Integer
        Dim RecordsAffected As Integer = 0
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New SqlCeCommand(strSQL, conn)
                    RecordsAffected = keyCommand.ExecuteNonQuery()
                    Return RecordsAffected
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    EveHQ.Core.HQ.WriteLogEvent("Database Error: " & e.Message)
                    EveHQ.Core.HQ.WriteLogEvent("SQL: " & strSQL)
                    Return -1
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
                    Dim keyCommand As New SqlCommand(strSQL, conn)
                    keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    RecordsAffected = keyCommand.ExecuteNonQuery()
                    Return RecordsAffected
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    EveHQ.Core.HQ.WriteLogEvent("Database Error: " & e.Message)
                    EveHQ.Core.HQ.WriteLogEvent("SQL: " & strSQL)
                    Return -1
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case Else
                EveHQ.Core.HQ.dataError = "Cannot Enumerate Database Format"
                Return -1
        End Select
    End Function
    Public Shared Function SetDataOnly(ByVal strSQL As String) As Integer
        Dim RecordsAffected As Integer = 0
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                Try
                    Dim keyCommand As New SqlCeCommand(strSQL, customSQLCEConnection)
                    RecordsAffected = keyCommand.ExecuteNonQuery()
                    Return RecordsAffected
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    EveHQ.Core.HQ.WriteLogEvent("Database Error: " & e.Message)
                    EveHQ.Core.HQ.WriteLogEvent("SQL: " & strSQL)
                    Return -1
                End Try
            Case 1 ' MSSQL
                Try
                    Dim keyCommand As New SqlCommand(strSQL, customSQLConnection)
                    keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    RecordsAffected = keyCommand.ExecuteNonQuery()
                    Return RecordsAffected
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = e.Message
                    EveHQ.Core.HQ.WriteLogEvent("Database Error: " & e.Message)
                    EveHQ.Core.HQ.WriteLogEvent("SQL: " & strSQL)
                    Return -1
                End Try
            Case Else
                EveHQ.Core.HQ.dataError = "Cannot Enumerate Database Format"
                Return -1
        End Select
    End Function
    Public Shared Function GetData(ByVal strSQL As String) As DataSet

        Dim EveHQData As New DataSet
        EveHQData.Clear()
        EveHQData.Tables.Clear()

        If strSQL.Contains(" LIKE ") = False And strSQL.Contains(" IN ") = False Then
            strSQL = strSQL.Replace("'", "''")
            strSQL = strSQL.Replace(ControlChars.Quote, "'")
            strSQL = strSQL.Replace("=true", "=1")
        End If

        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim da As New SqlCeDataAdapter(strSQL, conn)
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    Dim msg As New StringBuilder
                    msg.AppendLine("Database1: " & EveHQ.Core.HQ.EveHQSettings.DBFilename)
                    msg.AppendLine("Database2: " & EveHQ.Core.HQ.EveHQSettings.DBDataFilename)
                    msg.AppendLine("Using App: " & EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB.ToString)
                    msg.AppendLine("Connection String: " & conn.ConnectionString)
                    msg.AppendLine("SQL: " & strSQL)
                    msg.AppendLine("Message: " & e.Message)
                    If e.InnerException IsNot Nothing Then
                        msg.AppendLine("Inner Ex: " & e.InnerException.Message)
                    End If
                    EveHQ.Core.HQ.dataError = msg.ToString
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                Try
                    conn.Open()
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
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim da As New SqlCeDataAdapter(strSQL, conn)
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
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
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
    Public Shared Function GetBPWF(ByVal typeID As String) As Double
        Dim BPWF As Double = 0
        Dim strSQL As String = "SELECT *"
        strSQL &= " FROM invBlueprintTypes"
        strSQL &= " WHERE blueprintTypeID=" & typeID & ";"
        Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        If eveData IsNot Nothing Then
            If eveData.Tables(0).Rows.Count > 0 Then
                For col As Integer = 3 To eveData.Tables(0).Columns.Count - 1
                    ' Check for BPWF
                    If eveData.Tables(0).Columns(col).Caption = "wasteFactor" Then
                        BPWF = Math.Round(CDbl(eveData.Tables(0).Rows(0).Item(col).ToString))
                        Exit For
                    End If
                Next
            End If
            eveData.Dispose()
        End If
        Return BPWF
    End Function
    Public Shared Function LoadItemData() As Boolean
        Dim itemData As New DataSet
        Try
            EveHQ.Core.HQ.itemData.Clear()
            ' Get type data
            Dim strSQL As String = "SELECT invGroups.categoryID, invTypes.typeID, invTypes.groupID, invTypes.typeName, invTypes.volume, invTypes.portionSize, invTypes.basePrice, invTypes.published, invTypes.marketGroupID FROM invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID;"
            itemData = EveHQ.Core.DataFunctions.GetData(strSQL)
            ' Get meta data
            strSQL = ""
            Dim newItem As New EveItem
            If itemData IsNot Nothing Then
                If itemData.Tables(0).Rows.Count > 0 Then
                    For Each itemRow As DataRow In itemData.Tables(0).Rows
                        newItem = New EveItem
                        newItem.ID = CLng(itemRow.Item("typeID"))
                        newItem.Name = CStr(itemRow.Item("typeName"))
                        newItem.Group = CInt(itemRow.Item("groupID"))
                        newItem.Published = CBool(itemRow.Item("published"))
                        newItem.Category = CInt(itemRow.Item("categoryID"))
                        If IsDBNull(itemRow.Item("marketGroupID")) = False Then
                            newItem.MarketGroup = CInt(itemRow.Item("marketGroupID"))
                        Else
                            newItem.MarketGroup = 0
                        End If
                        newItem.Volume = CDbl(itemRow.Item("volume"))
                        newItem.PortionSize = CInt(itemRow.Item("portionSize"))
                        newItem.BasePrice = CDbl(itemRow.Item("basePrice"))
                        EveHQ.Core.HQ.itemData.Add(newItem.ID.ToString, newItem)
                    Next
                    ' Get the MetaLevel data
                    strSQL = "SELECT * FROM dgmTypeAttributes WHERE attributeID=633;"
                    itemData = EveHQ.Core.DataFunctions.GetData(strSQL)
                    If itemData.Tables(0).Rows.Count > 0 Then
                        For Each itemRow As DataRow In itemData.Tables(0).Rows
                            If EveHQ.Core.HQ.itemData.ContainsKey(CStr(itemRow.Item("typeID"))) Then
                                newItem = EveHQ.Core.HQ.itemData(CStr(itemRow.Item("typeID")))
                                If IsDBNull(itemRow.Item("valueInt")) = False Then
                                    newItem.MetaLevel = CInt(itemRow.Item("valueInt"))
                                Else
                                    newItem.MetaLevel = CInt(itemRow.Item("valueFloat"))
                                End If
                            End If
                        Next
                        ' Get the icon data
                        strSQL = "SELECT invTypes.typeID, eveIcons.iconFile FROM eveIcons INNER JOIN invTypes ON eveIcons.iconID = invTypes.iconID;"
                        itemData = EveHQ.Core.DataFunctions.GetData(strSQL)
                        If itemData.Tables(0).Rows.Count > 0 Then
                            For Each itemRow As DataRow In itemData.Tables(0).Rows
                                If EveHQ.Core.HQ.itemData.ContainsKey(CStr(itemRow.Item("typeID"))) Then
                                    newItem = EveHQ.Core.HQ.itemData(CStr(itemRow.Item("typeID")))
                                    If IsDBNull(itemRow.Item("iconFile")) = False Then
                                        newItem.Icon = CStr(itemRow.Item("iconFile"))
                                    End If
                                End If
                            Next
                        End If
                        itemData.Dispose()
                        GC.Collect()
                        Return True
                    Else
                        itemData.Dispose()
                        Return False
                    End If
                Else
                    itemData.Dispose()
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            If itemData IsNot Nothing Then
                itemData.Dispose()
            End If
            MessageBox.Show("Error Loading Item Data:" & ControlChars.CrLf & ex.Message, "Load Items Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Shared Function LoadItems() As Boolean

        ' Initally load the new item data routine
        Call LoadItemData()
        Call EveHQ.Core.MarketFunctions.LoadItemMarketGroups()

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
            Next
            ' Load Certificate data
            Call EveHQ.Core.DataFunctions.LoadCertCategories()
            Call EveHQ.Core.DataFunctions.LoadCertClasses()
            Call EveHQ.Core.DataFunctions.LoadCerts()
            Call EveHQ.Core.DataFunctions.LoadCertReqs()
            ' Load the "unlock" (dependancy) data
            If EveHQ.Core.DataFunctions.LoadUnlocks = False Then
                If eveData IsNot Nothing Then
                    eveData.Dispose()
                End If
                Return False
            End If
            ' Load price data
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
            strSQL &= " WHERE (((dgmTypeAttributes.attributeID) IN (182,183,184,277,278,279,1285,1286,1287,1288,1289,1290)) AND (invTypes.published=1))"
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
    Public Shared Function GetPrice(ByVal itemID As String) As Double
        If itemID IsNot Nothing Then
            Try
                If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) = True Then
                    Return CDbl(EveHQ.Core.HQ.CustomPriceList(itemID))
                Else
                    If EveHQ.Core.HQ.MarketPriceList.ContainsKey(itemID) Then
                        Return CDbl(EveHQ.Core.HQ.MarketPriceList(itemID))
                    Else
                        If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                            Return EveHQ.Core.HQ.itemData(itemID).BasePrice
                        Else
                            Return 0
                        End If
                    End If
                End If
            Catch e As Exception
                If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                    Return EveHQ.Core.HQ.itemData(itemID).BasePrice
                Else
                    Return 0
                End If
            End Try
        Else
            Return 0
        End If
    End Function

#Region "MSSQL Data Conversion Routines"
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
    Public Shared Sub DoSQLQuery(ByVal connection As SqlConnection)
        Dim strSQL As String = My.Resources.SQLQueries.ToString
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
    Public Shared Function CreateCustomPricesTable() As Boolean
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
            strSQL.AppendLine("  CONSTRAINT customPrices_PK PRIMARY KEY (typeID)")
            strSQL.AppendLine(")")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) <> -1 Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

    End Function
    Public Shared Function CreateMarketPricesTable() As Boolean
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
            strSQL.AppendLine("  CONSTRAINT marketPrices_PK PRIMARY KEY (typeID)")
            strSQL.AppendLine(")")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) <> -1 Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

    End Function
    Public Shared Function SetCustomPrice(ByVal typeID As Long, ByVal UserPrice As Double, ByVal DBOpen As Boolean) As Boolean
        ' Store the user's price in the database
        If EveHQ.Core.HQ.CustomPriceList.ContainsKey(typeID.ToString) = False Then
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
        Dim priceSQL As String = "INSERT INTO customPrices (typeID, price, priceDate) VALUES (" & itemID & ", " & price.ToString(culture) & ", '" & Now.ToString(SQLTimeFormat, culture) & "');"
        If DBOpen = False Then
            If EveHQ.Core.DataFunctions.SetData(priceSQL) = -1 Then
                MessageBox.Show("There was an error writing data to the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If EveHQ.Core.DataFunctions.SetDataOnly(priceSQL) = -1 Then
                MessageBox.Show("There was an error writing data to the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Shared Function EditCustomPrice(ByVal itemID As String, ByVal price As Double, ByVal DBOpen As Boolean) As Boolean
        EveHQ.Core.HQ.CustomPriceList(itemID) = price
        Dim priceSQL As String = "UPDATE customPrices SET price=" & price.ToString(culture) & ", priceDate='" & Now.ToString(SQLTimeFormat, culture) & "' WHERE typeID=" & itemID & ";"
        If DBOpen = False Then
            If EveHQ.Core.DataFunctions.SetData(priceSQL) = -1 Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If EveHQ.Core.DataFunctions.SetDataOnly(priceSQL) = -1 Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Shared Function DeleteCustomPrice(ByVal itemID As String) As Boolean
        ' Double check it exists and delete it
        If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) = True Then
            EveHQ.Core.HQ.CustomPriceList.Remove(itemID)
        End If
        Dim priceSQL As String = "DELETE FROM customPrices WHERE typeID=" & itemID & ";"
        If EveHQ.Core.DataFunctions.SetData(priceSQL) = -1 Then
            MessageBox.Show("There was an error deleting data from the Custom Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function SetMarketPrice(ByVal typeID As Long, ByVal UserPrice As Double, ByVal DBOpen As Boolean) As Boolean
        ' Store the user's price in the database
        If EveHQ.Core.HQ.MarketPriceList.ContainsKey(typeID.ToString) = False Then
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
        Dim priceSQL As String = "INSERT INTO marketPrices (typeID, price, priceDate) VALUES (" & itemID & ", " & price.ToString(culture) & ", '" & Now.ToString(SQLTimeFormat, culture) & "');"
        If DBOpen = False Then
            If EveHQ.Core.DataFunctions.SetData(priceSQL) = -1 Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If EveHQ.Core.DataFunctions.SetDataOnly(priceSQL) = -1 Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Shared Function EditMarketPrice(ByVal itemID As String, ByVal price As Double, ByVal DBOpen As Boolean) As Boolean
        EveHQ.Core.HQ.MarketPriceList(itemID) = price
        Dim priceSQL As String = "UPDATE marketPrices SET price=" & price.ToString(culture) & ", priceDate = '" & Now.ToString(SQLTimeFormat, culture) & "' WHERE typeID=" & itemID & ";"
        If DBOpen = False Then
            If EveHQ.Core.DataFunctions.SetData(priceSQL) = -1 Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If EveHQ.Core.DataFunctions.SetDataOnly(priceSQL) = -1 Then
                MessageBox.Show("There was an error writing data to the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Shared Function DeleteMarketPrice(ByVal itemID As String) As Boolean
        ' Double check it exists and delete it
        If EveHQ.Core.HQ.MarketPriceList.ContainsKey(itemID) = True Then
            EveHQ.Core.HQ.MarketPriceList.Remove(itemID)
        End If
        Dim priceSQL As String = "DELETE FROM marketPrices WHERE typeID=" & itemID & ";"
        If EveHQ.Core.DataFunctions.SetData(priceSQL) = -1 Then
            MessageBox.Show("There was an error deleting data from the Market Prices database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & priceSQL, "Error Writing Price Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function ProcessMarketExportFile(ByVal orderFile As String, WriteToDB As Boolean) As ArrayList

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

        If header <> "price,volRemaining,typeID,range,orderID,volEntered,minVolume,bid,issueDate,duration,stationID,regionID,solarSystemID,jumps," Then
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
                PriceData = CalculateMarketExportStats(CType(items(item), ArrayList), orderdate, WriteToDB)
            Next

            items.Clear() : items = Nothing
            itemOrders.Clear() : itemOrders = Nothing
            GC.Collect()

            Return PriceData

        End If
    End Function
    Private Shared Function CalculateMarketExportStats(ByVal orderList As ArrayList, ByVal orderDate As Date, WriteToDB As Boolean) As ArrayList
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
            order = order.Replace(Chr(0), "")
            orderDetails = order.Split(",".ToCharArray)
            oPrice = Double.Parse(orderDetails(0).Trim, Globalization.NumberStyles.Any, culture)
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
                If EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrders = True And oPrice > (EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrderLimit * EveHQ.Core.HQ.itemData(oTypeID.ToString).BasePrice) Then
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

        priceArray.Add(EveHQ.Core.MarketFunctions.CalculateUserPriceFromPriceArray(priceArray, oReg.ToString, oTypeID.ToString, WriteToDB))

        priceArray.Add(oTypeID)
        priceArray.Add(volBuy) : priceArray.Add(volSell) : priceArray.Add(volAll)

        Return priceArray
    End Function

    Public Shared Function CheckForIDNameTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("eveIDToName") = False Then
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
            strSQL.AppendLine("CREATE TABLE eveIDToName")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  eveID      bigint NOT NULL,")
            strSQL.AppendLine("  eveName    nvarchar(255) NOT NULL,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT eveIDToName_PK PRIMARY KEY (eveID)")
            strSQL.AppendLine(")")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) <> -1 Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Eve ID-To-Name database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Eve ID-To-Name Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
    End Function
    Public Shared Function CheckForEveMailTable() As Integer
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("eveMail") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTable = True
            Else
                ' We have the DB and table so we can check the existence of the messagebody field
                Return CheckForEveMailBodyColumn()
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return -1
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
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
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) <> -1 Then
                Return 0
            Else
                MessageBox.Show("There was an error creating the Eve Mail database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Eve Mail Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return -1
            End If
        End If
    End Function
    Public Shared Function CheckForEveNotificationTable() As Integer
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("eveNotifications") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTable = True
            Else
                ' We have the DB and table so we can check the existence of the messagebody field
                Return CheckForEveNotificationBodyColumn()
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return -1
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
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
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) <> -1 Then
                Return 0
            Else
                MessageBox.Show("There was an error creating the Eve Notification database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Eve Notification Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return -1
            End If
        End If
    End Function

    Public Shared Function CheckForEveMailBodyColumn() As Integer
        Dim strSQL As String = "SELECT * FROM eveMail;"
        Dim ColData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)

        ' Get the index of the field name
        Dim i As Integer = ColData.Tables(0).Columns.IndexOf("messageBody")

        If i = -1 Then
            'Field is missing
            Dim MailSQL As String = "ALTER TABLE eveMail ADD messageBody ntext NULL;"
            Return (EveHQ.Core.DataFunctions.SetData(MailSQL))
        Else
            Return 0
        End If

    End Function

    Public Shared Function CheckForEveNotificationBodyColumn() As Integer
        Dim strSQL As String = "SELECT * FROM eveNotifications;"
        Dim ColData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)

        ' Get the index of the field name
        Dim i As Integer = ColData.Tables(0).Columns.IndexOf("messageBody")

        If i = -1 Then
            'Field is missing
            Dim MailSQL As String = "ALTER TABLE eveNotifications ADD messageBody ntext NULL;"
            Return (EveHQ.Core.DataFunctions.SetData(MailSQL))
        Else
            Return 0
        End If

    End Function

    Public Shared Sub ParseIDs(ByRef IDs As List(Of String), ByVal strID As String)
        Dim strIDs() As String = strID.Split(",".ToCharArray)
        For Each ID As String In strIDs
            If ID.Trim <> "" Then
                If IDs.Contains(ID) = False Then
                    IDs.Add(ID)
                End If
            End If
        Next
    End Sub

    Public Shared Sub WriteEveIDsToDatabase(ByVal IDs As List(Of String))

        Dim MainIDList As New List(Of String)
        For Each ID As String In IDs
            MainIDList.Add(ID)
        Next

        If MainIDList.Count > 0 Then
            Do

                ' Break the ID list into groups of 200
                Dim MaxIDs As Integer = 200
                Dim ReqIDList As New List(Of String)
                For idx As Integer = 0 To Math.Min(MainIDList.Count, MaxIDs) - 1
                    ReqIDList.Add(MainIDList.Item(0))
                    MainIDList.RemoveAt(0)
                Next

                ' Write to log file about ID request
                EveHQ.Core.HQ.WriteLogEvent("***** Start: Request Eve IDs From API *****")

                EveHQ.Core.HQ.WriteLogEvent("Building Required ID List")
                Dim strID As New StringBuilder
                For Each ID As String In ReqIDList
                    strID.Append("," & ID)
                Next
                strID.Remove(0, 1)
                EveHQ.Core.HQ.WriteLogEvent("Finishing building list of " & ReqIDList.Count.ToString & " IDs")

                ' Get a list of everything we already have
                EveHQ.Core.HQ.WriteLogEvent("Querying existing IDToName data")
                Dim ExistingIDs As New List(Of String)
                Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                Dim IDData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If IDData IsNot Nothing Then
                    If IDData.Tables(0).Rows.Count > 0 Then
                        For Each IDRow As DataRow In IDData.Tables(0).Rows
                            ExistingIDs.Add(CStr(IDRow.Item("eveID")))
                        Next
                    End If
                End If

                EveHQ.Core.HQ.WriteLogEvent("Removing existing IDs from the list")
                ' Remove existing IDs from the parsed list
                Dim RemoveCount As Integer = 0
                For Each existingID As String In ExistingIDs
                    If ReqIDList.Contains(existingID) Then
                        ReqIDList.Remove(existingID)
                        RemoveCount += 1
                    End If
                Next
                EveHQ.Core.HQ.WriteLogEvent("Finished Removing " & RemoveCount.ToString & " existing IDs from the list")

                If ReqIDList.Count > 0 Then

                    EveHQ.Core.HQ.WriteLogEvent("Rebuilding Required ID List for sending to the API")
                    strID = New StringBuilder
                    For Each ID As String In ReqIDList
                        strID.Append("," & ID)
                    Next
                    If strID.Length > 1 Then
                        strID.Remove(0, 1)
                    End If
                    EveHQ.Core.HQ.WriteLogEvent("Finishing building list of " & ReqIDList.Count.ToString & " IDs for the API")

                    ' Send this to the API if we have something!
                    EveHQ.Core.HQ.WriteLogEvent("Requesting ID List From the API: " & strID.ToString)
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    Dim IDXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.IDToName, strID.ToString, EveAPI.APIReturnMethods.ReturnActual)
                    ' Parse this XML
                    Dim FinalIDs As New SortedList(Of Long, String)
                    Dim IDList As XmlNodeList
                    Dim IDNode As XmlNode
                    Dim eveID As Long = 0
                    Dim eveName As String = ""
                    If IDXML IsNot Nothing Then
                        IDList = IDXML.SelectNodes("/eveapi/result/rowset/row")
                        If IDList.Count > 0 Then
                            EveHQ.Core.HQ.WriteLogEvent("Parsing " & IDList.Count.ToString & " IDs in the XML file")
                            For Each IDNode In IDList
                                eveID = CLng(IDNode.Attributes.GetNamedItem("characterID").Value)
                                eveName = IDNode.Attributes.GetNamedItem("name").Value
                                If FinalIDs.ContainsKey(eveID) = False Then
                                    FinalIDs.Add(eveID, eveName)
                                End If
                            Next
                        Else
                            If APIReq.LastAPIError > 0 Then
                                EveHQ.Core.HQ.WriteLogEvent("Error " & APIReq.LastAPIError.ToString & " returned by the API!")
                            End If
                        End If

                        ' Add all the data to the database
                        EveHQ.Core.HQ.WriteLogEvent("Creating SQL Query for IDToName data")
                        Dim strIDInsert As String = "INSERT INTO eveIDToName (eveID, eveName) VALUES "
                        For Each eveID In FinalIDs.Keys
                            If ExistingIDs.Contains(eveID.ToString) = False Then
                                eveName = FinalIDs(eveID)
                                Dim uSQL As New StringBuilder
                                uSQL.Append(strIDInsert)
                                uSQL.Append("(" & eveID & ", ")
                                uSQL.Append("'" & eveName.Replace("'", "''") & "');")
                                EveHQ.Core.HQ.WriteLogEvent("Writing IDToName data to database")
                                If EveHQ.Core.DataFunctions.SetData(uSQL.ToString) = -1 Then
                                    'MessageBox.Show("There was an error writing data to the Eve ID database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve IDs", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)  
                                End If
                            End If
                        Next
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("ID XML returned nothing from the API")
                    End If
                Else
                    EveHQ.Core.HQ.WriteLogEvent("All IDs present in the database, no request required to the API")
                End If

                ' Write to log file about ID request
                EveHQ.Core.HQ.WriteLogEvent("***** End: Request Eve IDs From API *****")

            Loop Until MainIDList.Count = 0
        End If
    End Sub

    Public Shared Function WriteMailingListIDsToDatabase(ByVal mPilot As EveHQ.Core.Pilot) As SortedList(Of Long, String)
        Dim FinalIDs As New SortedList(Of Long, String)
        Dim accountName As String = mPilot.Account
        If accountName <> "" Then
            Dim mAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            ' Send this to the API
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            Dim IDXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.MailingLists, mAccount.ToAPIAccount, mPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
            ' Parse this XML
            Dim IDList As XmlNodeList
            Dim IDNode As XmlNode
            Dim eveID As Long = 0
            Dim eveName As String = ""
            IDList = IDXML.SelectNodes("/eveapi/result/rowset/row")
            If IDList.Count > 0 Then
                For Each IDNode In IDList
                    eveID = CLng(IDNode.Attributes.GetNamedItem("listID").Value)
                    eveName = IDNode.Attributes.GetNamedItem("displayName").Value
                    If FinalIDs.ContainsKey(eveID) = False Then
                        FinalIDs.Add(eveID, eveName)
                    End If
                Next
            End If
            ' Add all the data to the database
            Dim strIDInsert As String = "INSERT INTO eveIDToName (eveID, eveName) VALUES "
            For Each eveID In FinalIDs.Keys
                eveName = FinalIDs(eveID)
                Dim uSQL As New StringBuilder
                uSQL.Append(strIDInsert)
                uSQL.Append("(" & eveID & ", ")
                uSQL.Append("'" & eveName & "');")
                If EveHQ.Core.DataFunctions.SetData(uSQL.ToString) = -1 Then
                    'MessageBox.Show("There was an error writing data to the Eve ID database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve IDs", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            Next
        End If
        Return FinalIDs
    End Function

    Public Shared Function CheckDatabaseConnection(ByVal silentResponse As Boolean) As Boolean
        Dim strConnection As String = ""
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                strConnection = "Data Source = " & ControlChars.Quote & EveHQ.Core.HQ.EveHQSettings.DBFilename & ControlChars.Quote & ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Dim connection As New SqlCeConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    If silentResponse = False Then
                        MessageBox.Show("Connected successfully to SQL CE database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    If silentResponse = False Then
                        MessageBox.Show(ex.Message, "Error Opening SQL CE Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    Return False
                End Try
            Case 1 ' SQL
                strConnection = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName & "; Integrated Security = SSPI;"
                End If
                Dim connection As New SqlConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    If silentResponse = False Then
                        MessageBox.Show("Connected successfully to MS SQL database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    If silentResponse = False Then
                        MessageBox.Show(ex.Message, "Error Opening MS SQL Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    Return False
                End Try
        End Select
    End Function

    Public Shared Function CheckDataDatabaseConnection(ByVal silentResponse As Boolean) As Boolean
        Dim strConnection As String = ""
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' SQL CE
                strConnection = "Data Source = " & ControlChars.Quote & EveHQ.Core.HQ.EveHQSettings.DBDataFilename & ControlChars.Quote & ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Dim connection As New SqlCeConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    If silentResponse = False Then
                        MessageBox.Show("Connected successfully to SQL CE database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    If silentResponse = False Then
                        MessageBox.Show(ex.Message, "Error Opening SQL CE Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    Return False
                End Try
            Case 1 ' SQL
                strConnection = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName & "; Integrated Security = SSPI;"
                End If
                Dim connection As New SqlConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    If silentResponse = False Then
                        MessageBox.Show("Connected successfully to MS SQL database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    If silentResponse = False Then
                        MessageBox.Show(ex.Message, "Error Opening MS SQL Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    Return False
                End Try
        End Select
    End Function

#Region "Solar System & Station Data Routines"

    Public Shared Function LoadSolarSystems() As Boolean
        Dim strSQL As String = "SELECT mapSolarSystems.regionID AS mapSolarSystems_regionID, mapSolarSystems.constellationID AS mapSolarSystems_constellationID, mapSolarSystems.solarSystemID, mapSolarSystems.solarSystemName, mapSolarSystems.x, mapSolarSystems.y, mapSolarSystems.z, mapSolarSystems.xMin, mapSolarSystems.xMax, mapSolarSystems.yMin, mapSolarSystems.yMax, mapSolarSystems.zMin, mapSolarSystems.zMax, mapSolarSystems.luminosity, mapSolarSystems.border, mapSolarSystems.fringe, mapSolarSystems.corridor, mapSolarSystems.hub, mapSolarSystems.international, mapSolarSystems.regional, mapSolarSystems.constellation, mapSolarSystems.security, mapSolarSystems.factionID, mapSolarSystems.radius, mapSolarSystems.sunTypeID, mapSolarSystems.securityClass, mapRegions.regionID AS mapRegions_regionID, mapRegions.regionName, mapConstellations.constellationID AS mapConstellations_constellationID, mapConstellations.constellationName"
        strSQL &= " FROM (mapRegions INNER JOIN mapConstellations ON mapRegions.regionID = mapConstellations.regionID) INNER JOIN mapSolarSystems ON mapConstellations.constellationID = mapSolarSystems.constellationID;"
        Dim systemData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If systemData IsNot Nothing Then
                If systemData.Tables(0).Rows.Count > 0 Then
                    Dim cSystem As SolarSystem = New SolarSystem
                    EveHQ.Core.HQ.SolarSystems.Clear()
                    For solar As Integer = 0 To systemData.Tables(0).Rows.Count - 1
                        cSystem = New SolarSystem
                        cSystem.ID = CInt(systemData.Tables(0).Rows(solar).Item("solarSystemID"))
                        cSystem.Name = CStr(systemData.Tables(0).Rows(solar).Item("solarSystemName"))
                        cSystem.Region = CStr(systemData.Tables(0).Rows(solar).Item("regionName"))
                        cSystem.Constellation = CStr(systemData.Tables(0).Rows(solar).Item("constellationName"))
                        cSystem.Security = CDbl(systemData.Tables(0).Rows(solar).Item("security"))
                        EveHQ.Core.HQ.SolarSystems.Add(CStr(cSystem.ID), cSystem)
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading System Data for Prism Plugin" & ControlChars.CrLf & ex.Message, "Prism Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Public Shared Function LoadStations() As Boolean
        ' Load the Station Data
        Try
            Dim strSQL As String = "SELECT * FROM staStations;"
            Dim locationData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If locationData IsNot Nothing Then
                If locationData.Tables(0).Rows.Count > 0 Then
                    EveHQ.Core.HQ.Stations.Clear()
                    For Each locationRow As DataRow In locationData.Tables(0).Rows
                        Dim newStation As New Station
                        newStation.stationID = CLng(locationRow.Item("stationID"))
                        newStation.stationName = CStr(locationRow.Item("stationName"))
                        newStation.systemID = CLng(locationRow.Item("solarSystemID"))
                        newStation.constID = CLng(locationRow.Item("constellationID"))
                        newStation.regionID = CLng(locationRow.Item("regionID"))
                        newStation.corpID = CLng(locationRow.Item("corporationID"))
                        newStation.refiningEff = CDbl(locationRow.Item("reprocessingEfficiency"))
                        EveHQ.Core.HQ.Stations.Add(newStation.stationID.ToString, newStation)
                    Next
                    Return CheckForConqXMLFile()
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading Station Data for Prism Plugin" & ControlChars.CrLf & ex.Message, "Prism Plug-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Shared Function CheckForConqXMLFile() As Boolean
        ' Check for the Conquerable XML file in the cache
        Dim stationXML As New XmlDocument
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
        stationXML = APIReq.GetAPIXML(EveAPI.APITypes.Conquerables, EveAPI.APIReturnMethods.ReturnStandard)
        If stationXML IsNot Nothing Then
            Return ParseConquerableXML(stationXML)
        Else
            Return False
        End If
    End Function

    Private Shared Function ParseConquerableXML(ByVal stationXML As XmlDocument) As Boolean
        Dim locList As XmlNodeList
        Dim loc As XmlNode
        Dim stationID As String = ""
        locList = stationXML.SelectNodes("/eveapi/result/rowset/row")
        If locList.Count > 0 Then
            For Each loc In locList
                stationID = (loc.Attributes.GetNamedItem("stationID").Value)
                ' This is an outpost so needs adding to the station list if it's not there
                If EveHQ.Core.HQ.Stations.ContainsKey(stationID) = False Then
                    Dim cStation As New EveHQ.Core.Station
                    cStation.stationID = CLng(stationID)
                    cStation.stationName = (loc.Attributes.GetNamedItem("stationName").Value)
                    cStation.systemID = CLng(loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As EveHQ.Core.SolarSystem = EveHQ.Core.HQ.SolarSystems(cStation.systemID.ToString)
                    cStation.stationName &= " (" & system.Name & ", " & system.Region & ")"
                    cStation.corpID = CLng(loc.Attributes.GetNamedItem("corporationID").Value)
                    EveHQ.Core.HQ.Stations.Add(cStation.stationID.ToString, cStation)
                Else
                    Dim cStation As Station = CType(EveHQ.Core.HQ.Stations(stationID), EveHQ.Core.Station)
                    cStation.systemID = CLng(loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As EveHQ.Core.SolarSystem = EveHQ.Core.HQ.SolarSystems(cStation.systemID.ToString)
                    cStation.stationName &= " (" & system.Name & ", " & system.Region & ")"
                    cStation.corpID = CLng(loc.Attributes.GetNamedItem("corporationID").Value)
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function GetLocationName(ByVal LocationID As String) As String
        If CDbl(LocationID) >= 66000000 Then
            If CDbl(LocationID) < 66014933 Then
                LocationID = (CDbl(LocationID) - 6000001).ToString
            Else
                LocationID = (CDbl(LocationID) - 6000000).ToString
            End If
        End If
        If CDbl(LocationID) >= 61000000 And CDbl(LocationID) <= 61999999 Then
            If EveHQ.Core.HQ.Stations.ContainsKey(LocationID) = True Then
                ' Known Outpost
                Return EveHQ.Core.HQ.Stations(LocationID).stationName
            Else
                ' Unknown outpost!
                Return "Unknown Outpost"
            End If
        Else
            If CDbl(LocationID) < 60000000 Then
                If EveHQ.Core.HQ.SolarSystems.ContainsKey(LocationID) Then
                    ' Known solar system
                    Return EveHQ.Core.HQ.SolarSystems(LocationID).Name
                Else
                    ' Unknown solar system
                    Return "Unknown System"
                End If
            Else
                If EveHQ.Core.HQ.Stations.ContainsKey(LocationID) Then
                    ' Known station
                    Return EveHQ.Core.HQ.Stations(LocationID).stationName
                Else
                    ' Unknown station
                    Return "Unknown Station"
                End If
            End If
        End If
    End Function
#End Region

#Region "Certificate Load Routines"
    Public Shared Function LoadCertCategories() As Boolean
        Dim strSQL As String = "SELECT * FROM crtCategories;"
        Dim CertData As DataSet = GetData(strSQL)
        If CertData IsNot Nothing Then
            If CertData.Tables(0).Rows.Count > 0 Then
                EveHQ.Core.HQ.CertificateCategories.Clear()
                For Each CertRow As DataRow In CertData.Tables(0).Rows
                    Dim NewCat As New EveHQ.Core.CertificateCategory
                    NewCat.ID = CInt(CertRow.Item("categoryID"))
                    NewCat.Name = CertRow.Item("categoryName").ToString
                    EveHQ.Core.HQ.CertificateCategories.Add(NewCat.ID.ToString, NewCat)
                Next
                CertData.Dispose()
                Return True
            Else
                CertData.Dispose()
                Return False
            End If
        Else
            CertData.Dispose()
            Return False
        End If
    End Function

    Public Shared Function LoadCertClasses() As Boolean
        Dim strSQL As String = "SELECT * FROM crtClasses;"
        Dim CertData As DataSet = GetData(strSQL)
        If CertData IsNot Nothing Then
            If CertData.Tables(0).Rows.Count > 0 Then
                EveHQ.Core.HQ.CertificateClasses.Clear()
                For Each CertRow As DataRow In CertData.Tables(0).Rows
                    Dim NewClass As New EveHQ.Core.CertificateClass
                    NewClass.ID = CInt(CertRow.Item("classID"))
                    NewClass.Name = CertRow.Item("className").ToString
                    EveHQ.Core.HQ.CertificateClasses.Add(NewClass.ID.ToString, NewClass)
                Next
                CertData.Dispose()
                Return True
            Else
                CertData.Dispose()
                Return False
            End If
        Else
            CertData.Dispose()
            Return False
        End If
    End Function

    Public Shared Function LoadCerts() As Boolean
        Dim strSQL As String = "SELECT * FROM crtCertificates;"
        Dim CertData As DataSet = GetData(strSQL)
        If CertData IsNot Nothing Then
            If CertData.Tables(0).Rows.Count > 0 Then
                EveHQ.Core.HQ.Certificates.Clear()
                For Each CertRow As DataRow In CertData.Tables(0).Rows
                    Dim NewCert As New EveHQ.Core.Certificate
                    NewCert.ID = CInt(CertRow.Item("certificateID"))
                    NewCert.CategoryID = CInt(CertRow.Item("categoryID"))
                    NewCert.ClassID = CInt(CertRow.Item("classID"))
                    NewCert.Description = CStr(CertRow.Item("description"))
                    NewCert.Grade = CInt(CertRow.Item("grade"))
                    NewCert.CorpID = CInt(CertRow.Item("corpID"))
                    EveHQ.Core.HQ.Certificates.Add(NewCert.ID.ToString, NewCert)
                Next
                CertData.Dispose()
                Return True
            Else
                CertData.Dispose()
                Return False
            End If
        Else
            CertData.Dispose()
            Return False
        End If
    End Function

    Public Shared Function LoadCertReqs() As Boolean
        Dim strSQL As String = "SELECT * FROM crtRelationships;"
        Dim CertData As DataSet = GetData(strSQL)
        If CertData IsNot Nothing Then
            If CertData.Tables(0).Rows.Count > 0 Then
                For Each CertRow As DataRow In CertData.Tables(0).Rows
                    Dim certID As String = CertRow.Item("childID").ToString
                    If EveHQ.Core.HQ.Certificates.ContainsKey(certID) = True Then
                        Dim NewCert As EveHQ.Core.Certificate = EveHQ.Core.HQ.Certificates(certID)
                        If IsDBNull(CertRow.Item("parentID")) Then
                            ' This is a skill ID
                            NewCert.RequiredSkills.Add(CertRow.Item("parentTypeID").ToString, CInt(CertRow.Item("parentLevel")))
                        Else
                            ' This is a certID
                            NewCert.RequiredCerts.Add(CertRow.Item("parentID").ToString, 1)
                        End If
                    End If
                Next
                CertData.Dispose()
                Return True
            Else
                CertData.Dispose()
                Return False
            End If
        Else
            CertData.Dispose()
            Return False
        End If
    End Function
#End Region

#Region "Core Cache Routines"

    Public Shared Function CreateCoreCache() As Boolean

        ' Check for existence of a core cache folder in the application directory
        Dim CoreCacheFolder As String = ""
        If EveHQ.Core.HQ.IsUsingLocalFolders = False Then
            CoreCacheFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "CoreCache")
        Else
            CoreCacheFolder = Path.Combine(Application.StartupPath, "CoreCache")
        End If
        If My.Computer.FileSystem.DirectoryExists(CoreCacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(CoreCacheFolder)
        End If

        ' Dump core data to folder
        Dim s As FileStream
        Dim f As BinaryFormatter

        ' Item Data
        s = New FileStream(Path.Combine(CoreCacheFolder, "Items.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.itemData)
        s.Flush()
        s.Close()

        ' Item Market Groups
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemMarketGroups.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.ItemMarketGroups)
        s.Flush()
        s.Close()

        ' Item List
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemList.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.itemList)
        s.Flush()
        s.Close()

        ' Item Groups
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemGroups.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.itemGroups)
        s.Flush()
        s.Close()

        ' Items Cats
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemCats.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.itemCats)
        s.Flush()
        s.Close()

        ' Group Cats
        s = New FileStream(Path.Combine(CoreCacheFolder, "GroupCats.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.groupCats)
        s.Flush()
        s.Close()

        ' Cert Categories
        s = New FileStream(Path.Combine(CoreCacheFolder, "CertCats.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.CertificateCategories)
        s.Flush()
        s.Close()

        ' Cert Classes
        s = New FileStream(Path.Combine(CoreCacheFolder, "CertClasses.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.CertificateClasses)
        s.Flush()
        s.Close()

        ' Certs
        s = New FileStream(Path.Combine(CoreCacheFolder, "Certs.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.Certificates)
        s.Flush()
        s.Close()

        ' Unlocks
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemUnlocks.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.ItemUnlocks)
        s.Flush()
        s.Close()

        ' SkillUnlocks
        s = New FileStream(Path.Combine(CoreCacheFolder, "SkillUnlocks.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.SkillUnlocks)
        s.Flush()
        s.Close()

        ' CertCerts
        s = New FileStream(Path.Combine(CoreCacheFolder, "CertCerts.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.CertUnlockCerts)
        s.Flush()
        s.Close()

        ' CertSkills
        s = New FileStream(Path.Combine(CoreCacheFolder, "CertSkills.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.CertUnlockSkills)
        s.Flush()
        s.Close()

        ' Solar Systems
        s = New FileStream(Path.Combine(CoreCacheFolder, "Systems.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.SolarSystems)
        s.Flush()
        s.Close()

        ' Stations
        s = New FileStream(Path.Combine(CoreCacheFolder, "Stations.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.Stations)
        s.Flush()
        s.Close()

        ' Write the current version
        Dim sw As New StreamWriter(Path.Combine(CoreCacheFolder, "version.txt"))
        sw.Write(LastCacheRefresh)
        sw.Flush()
        sw.Close()

        Return True

    End Function

    Public Shared Function LoadCoreCache() As Boolean

        Try

            ' Check for existence of a core cache folder in the application directory
            Dim CoreCacheFolder As String = ""
            If EveHQ.Core.HQ.IsUsingLocalFolders = False Then
                CoreCacheFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "CoreCache")
            Else
                CoreCacheFolder = Path.Combine(Application.StartupPath, "CoreCache")
            End If
            If My.Computer.FileSystem.DirectoryExists(CoreCacheFolder) = True Then

                ' Check for last cache version file
                Dim UseCoreCache As Boolean = False
                If My.Computer.FileSystem.FileExists(Path.Combine(CoreCacheFolder, "version.txt")) = True Then
                    Dim sr As New StreamReader(Path.Combine(CoreCacheFolder, "version.txt"))
                    Dim cacheVersion As String = sr.ReadToEnd
                    sr.Close()
                    If IsUpdateAvailable(cacheVersion, LastCacheRefresh) = True Then
                        ' Delete the existing cache folder and force a rebuild
                        My.Computer.FileSystem.DeleteDirectory(CoreCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                        EveHQ.Core.HQ.WriteLogEvent("Core Cache outdated - rebuild of cache data required")
                        UseCoreCache = False
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("Core Cache still relevant - using existing cache data")
                        UseCoreCache = True
                    End If
                Else
                    ' Delete the existing cache folder and force a rebuild
                    My.Computer.FileSystem.DeleteDirectory(CoreCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    EveHQ.Core.HQ.WriteLogEvent("Core Cache version file not found - rebuild of cache data required")
                    UseCoreCache = False
                End If

                If UseCoreCache = True Then

                    ' Get files from dump
                    Dim s As FileStream
                    Dim f As BinaryFormatter

                    ' Item Data
                    s = New FileStream(Path.Combine(CoreCacheFolder, "Items.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.itemData = CType(f.Deserialize(s), SortedList(Of String, EveItem))
                    s.Close()

                    ' Item Market Groups
                    s = New FileStream(Path.Combine(CoreCacheFolder, "ItemMarketGroups.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.ItemMarketGroups = CType(f.Deserialize(s), SortedList(Of String, String))
                    s.Close()

                    ' Item List
                    s = New FileStream(Path.Combine(CoreCacheFolder, "ItemList.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.itemList = CType(f.Deserialize(s), SortedList(Of String, String))
                    s.Close()

                    ' Item Groups
                    s = New FileStream(Path.Combine(CoreCacheFolder, "ItemGroups.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.itemGroups = CType(f.Deserialize(s), SortedList(Of String, String))
                    s.Close()

                    ' Items Cats
                    s = New FileStream(Path.Combine(CoreCacheFolder, "ItemCats.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.itemCats = CType(f.Deserialize(s), SortedList(Of String, String))
                    s.Close()

                    ' Group Cats
                    s = New FileStream(Path.Combine(CoreCacheFolder, "GroupCats.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.groupCats = CType(f.Deserialize(s), SortedList(Of String, String))
                    s.Close()

                    ' Cert Categories
                    s = New FileStream(Path.Combine(CoreCacheFolder, "CertCats.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.CertificateCategories = CType(f.Deserialize(s), SortedList(Of String, CertificateCategory))
                    s.Close()

                    ' Cert Classes
                    s = New FileStream(Path.Combine(CoreCacheFolder, "CertClasses.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.CertificateClasses = CType(f.Deserialize(s), SortedList(Of String, CertificateClass))
                    s.Close()

                    ' Certs
                    s = New FileStream(Path.Combine(CoreCacheFolder, "Certs.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.Certificates = CType(f.Deserialize(s), SortedList(Of String, Certificate))
                    s.Close()

                    ' Unlocks
                    s = New FileStream(Path.Combine(CoreCacheFolder, "ItemUnlocks.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.ItemUnlocks = CType(f.Deserialize(s), SortedList(Of String, ArrayList))
                    s.Close()

                    ' SkillUnlocks
                    s = New FileStream(Path.Combine(CoreCacheFolder, "SkillUnlocks.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.SkillUnlocks = CType(f.Deserialize(s), SortedList(Of String, ArrayList))
                    s.Close()

                    ' CertCerts
                    s = New FileStream(Path.Combine(CoreCacheFolder, "CertCerts.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.CertUnlockCerts = CType(f.Deserialize(s), SortedList(Of String, ArrayList))
                    s.Close()

                    ' CertSkills
                    s = New FileStream(Path.Combine(CoreCacheFolder, "CertSkills.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.CertUnlockSkills = CType(f.Deserialize(s), SortedList(Of String, ArrayList))
                    s.Close()

                    ' SolarSystems
                    s = New FileStream(Path.Combine(CoreCacheFolder, "Systems.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.SolarSystems = CType(f.Deserialize(s), SortedList(Of String, SolarSystem))
                    s.Close()

                    ' Stations
                    s = New FileStream(Path.Combine(CoreCacheFolder, "Stations.bin"), FileMode.Open)
                    f = New BinaryFormatter
                    EveHQ.Core.HQ.Stations = CType(f.Deserialize(s), SortedList(Of String, Station))
                    s.Close()

                    ' Load price data
                    EveHQ.Core.DataFunctions.LoadMarketPricesFromDB()
                    EveHQ.Core.DataFunctions.LoadCustomPricesFromDB()

                    Return True

                Else
                    Return False
                End If

            Else
                Return False
            End If

        Catch e As Exception
            ' Load Core Cache failed
            Return False
        End Try

    End Function

    Private Shared Function IsUpdateAvailable(ByVal localVer As String, ByVal remoteVer As String) As Boolean
        If localVer = remoteVer Then
            Return False
        Else
            Dim localVers() As String = localVer.Split(CChar("."))
            Dim remoteVers() As String = remoteVer.Split(CChar("."))
            Dim requiresUpdate As Boolean = False
            For ver As Integer = 0 To 3
                If CInt(remoteVers(ver)) <> CInt(localVers(ver)) Then
                    If CInt(remoteVers(ver)) > CInt(localVers(ver)) Then
                        requiresUpdate = True
                        Exit For
                    Else
                        requiresUpdate = False
                        Exit For
                    End If
                End If
            Next
            Return requiresUpdate
        End If
    End Function

#End Region

End Class

Public Enum DatabaseFormat
    SQLCE = 0
    SQL = 1
End Enum
