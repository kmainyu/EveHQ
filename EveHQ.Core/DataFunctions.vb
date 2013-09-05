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
Imports System.Globalization
Imports System.Xml
Imports System.IO
Imports System.Windows.Forms
Imports System.Text
Imports System.Runtime.Serialization.Formatters.Binary
Imports EveHQ.EveAPI
Imports EveHQ.Market
Imports EveHQ.Common.Extensions
Imports Microsoft.VisualBasic.FileIO
Imports System.Threading.Tasks


Public Class DataFunctions
    Shared customSQLCEConnection As New SqlCeConnection
    Shared customSQLConnection As New SqlConnection
    Shared IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Shared SQLTimeFormat As String = "yyyyMMdd HH:mm:ss"
    Shared culture As CultureInfo = New CultureInfo("en-GB")
    Shared LastCacheRefresh As String = "2.12.0.0"

    Public Shared Function CreateEveHQDataDB() As Boolean
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                ' Get the directory of the existing SQL CE database to write the new one there
                Dim outputFile As String = ""
                outputFile = HQ.EveHqSettings.DBDataFilename.Replace("\\", "\")
                'MessageBox.Show("Creating database using path: " & outputFile, "Custom Database Location", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Try to create a new SQL CE DB
                Dim strConnection As String = "Data Source = " & ControlChars.Quote & outputFile & ControlChars.Quote &
                                              ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Try
                    Dim SQLCE As New SqlCeEngine(strConnection)
                    SQLCE.CreateDatabase()
                    HQ.EveHqSettings.DBDataFilename = outputFile
                    HQ.EveHQDataConnectionString = strConnection
                    Return True
                Catch e As Exception
                    HQ.dataError = "Unable to create SQL CE database in " & outputFile & ControlChars.CrLf &
                                   ControlChars.CrLf & e.Message
                    Return False
                End Try
            Case 1 ' MSSQL
                Dim strSQL As String = "CREATE DATABASE EveHQData;"
                Dim oldStrConn As String = HQ.EveHQDataConnectionString
                ' Set new database connection string
                HQ.EveHQDataConnectionString = "Server=" & HQ.EveHqSettings.DBServer
                If HQ.EveHqSettings.DBSQLSecurity = True Then
                    HQ.EveHQDataConnectionString += "; User ID=" & HQ.EveHqSettings.DBUsername & "; Password=" &
                                                    HQ.EveHqSettings.DBPassword & ";"
                Else
                    HQ.EveHQDataConnectionString += "; Integrated Security = SSPI;"
                End If
                If SetData(strSQL) <> -2 Then
                    HQ.EveHqSettings.DBDataName = "EveHQData"
                    HQ.EveHQDataConnectionString = oldStrConn
                    Return True
                Else
                    HQ.EveHQDataConnectionString = oldStrConn
                    Return False
                End If
        End Select
    End Function

    Public Shared Function GetDatabaseTables() As ArrayList
        Dim DBTables As New ArrayList
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    'Dim SchemaTable As DataTable = conn.GetSchema()
                    Dim schemaTable As DataSet = GetCustomData("SELECT * FROM INFORMATION_SCHEMA.TABLES")
                    For table As Integer = 0 To schemaTable.Tables(0).Rows.Count - 1
                        DBTables.Add(schemaTable.Tables(0).Rows(table).Item("TABLE_NAME").ToString)
                    Next
                    Return DBTables
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim SchemaTable As DataTable = conn.GetSchema("Tables")
                    For table As Integer = 0 To SchemaTable.Rows.Count - 1
                        DBTables.Add(SchemaTable.Rows(table)!TABLE_NAME.ToString())
                    Next
                    Return DBTables
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case Else
                HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function

    Public Shared Function SetEveHQConnectionString() As Boolean
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                If HQ.EveHqSettings.UseAppDirectoryForDB = False Then
                    HQ.itemDBConnectionString = "Data Source = " & ControlChars.Quote & HQ.EveHqSettings.DBFilename &
                                                ControlChars.Quote & ";" &
                                                "Max Database Size = 512; ; Max Buffer Size = 2048;"
                Else
                    Try
                        Dim FI As New FileInfo(HQ.EveHqSettings.DBFilename)
                        HQ.itemDBConnectionString = "Data Source = " & ControlChars.Quote &
                                                    Path.Combine(HQ.appFolder, FI.Name) & ControlChars.Quote & ";" &
                                                    "Max Database Size = 512; Max Buffer Size = 2048;"
                    Catch e As Exception
                        MessageBox.Show("There was an error setting the EveHQ connection string: " & e.Message,
                                        "Error Forming DB Connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
            Case 1 ' SQL
                HQ.itemDBConnectionString = "Server=" & HQ.EveHqSettings.DBServer
                If HQ.EveHqSettings.DBSQLSecurity = True Then
                    HQ.itemDBConnectionString += "; Database = " & HQ.EveHqSettings.DBName.ToLower & "; User ID=" &
                                                 HQ.EveHqSettings.DBUsername & "; Password=" &
                                                 HQ.EveHqSettings.DBPassword & ";"
                Else
                    HQ.itemDBConnectionString += "; Database = " & HQ.EveHqSettings.DBName.ToLower &
                                                 "; Integrated Security = SSPI;"
                End If
        End Select
        Return True
    End Function

    Public Shared Function SetEveHQDataConnectionString() As Boolean
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                If HQ.EveHqSettings.UseAppDirectoryForDB = False Then
                    HQ.EveHQDataConnectionString = "Data Source = " & ControlChars.Quote &
                                                   HQ.EveHqSettings.DBDataFilename & ControlChars.Quote & ";" &
                                                   "Max Database Size = 512; Max Buffer Size = 2048;"
                Else
                    Try
                        Dim FI As New FileInfo(HQ.EveHqSettings.DBDataFilename)
                        HQ.EveHQDataConnectionString = "Data Source = " & ControlChars.Quote &
                                                       Path.Combine(HQ.appFolder, FI.Name) & ControlChars.Quote & ";" &
                                                       "Max Database Size = 512; Max Buffer Size = 2048;"
                    Catch e As Exception
                        MessageBox.Show("There was an error setting the EveHQData connection string: " & e.Message,
                                        "Error Forming DB Connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try
                End If
            Case 1 ' SQL
                HQ.EveHQDataConnectionString = "Server=" & HQ.EveHqSettings.DBServer
                If HQ.EveHqSettings.DBSQLSecurity = True Then
                    HQ.EveHQDataConnectionString += "; Database = " & HQ.EveHqSettings.DBDataName.ToLower & "; User ID=" &
                                                    HQ.EveHqSettings.DBUsername & "; Password=" &
                                                    HQ.EveHqSettings.DBPassword & ";"
                Else
                    HQ.EveHQDataConnectionString += "; Database = " & HQ.EveHqSettings.DBDataName.ToLower &
                                                    "; Integrated Security = SSPI;"
                End If
        End Select
        Return True
    End Function

    Public Shared Function OpenCustomDatabase() As Boolean
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                customSQLCEConnection = New SqlCeConnection
                customSQLCEConnection.ConnectionString = HQ.EveHQDataConnectionString
                Try
                    customSQLCEConnection.Open()
                    Return True
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return False
                End Try
            Case 1 ' MSSQL
                customSQLConnection = New SqlConnection
                customSQLConnection.ConnectionString = HQ.EveHQDataConnectionString
                Try
                    customSQLConnection.Open()
                    Return True
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return False
                End Try
            Case Else
                HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function

    Public Shared Function CloseCustomDatabase() As Boolean
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                Try
                    If customSQLCEConnection.State = ConnectionState.Open Then
                        customSQLCEConnection.Close()
                    End If
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return False
                End Try
            Case 1 ' MSSQL
                Try
                    If customSQLConnection.State = ConnectionState.Open Then
                        customSQLConnection.Close()
                    End If
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return False
                End Try
            Case Else
                HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function

    Public Shared Function SetStaticData(ByVal strSQL As String) As Boolean
        If strSQL.Contains(" LIKE ") = False Then
            strSQL = strSQL.Replace("'", "''")
            strSQL = strSQL.Replace(ControlChars.Quote, "'")
            strSQL = strSQL.Replace("=true", "=1")
        End If
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New SqlCeCommand(strSQL, conn)
                    keyCommand.ExecuteNonQuery()
                    Return True
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return False
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New SqlCommand(strSQL, conn)
                    keyCommand.CommandTimeout = HQ.EveHqSettings.DBTimeout
                    keyCommand.ExecuteNonQuery()
                    Return True
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return False
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case Else
                HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function

    Public Shared Function SetData(ByVal strSQL As String) As Integer
        Dim RecordsAffected As Integer = 0
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New SqlCeCommand(strSQL, conn)
                    RecordsAffected = keyCommand.ExecuteNonQuery()
                    Return RecordsAffected
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
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New SqlCommand(strSQL, conn)
                    keyCommand.CommandTimeout = HQ.EveHqSettings.DBTimeout
                    RecordsAffected = keyCommand.ExecuteNonQuery()
                    Return RecordsAffected
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
            Case Else
                HQ.dataError = "Cannot Enumerate Database Format"
                Return -2
        End Select
    End Function

    Public Shared Function SetDataOnly(ByVal strSQL As String) As Integer
        Dim RecordsAffected As Integer = 0
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                Try
                    Dim keyCommand As New SqlCeCommand(strSQL, customSQLCEConnection)
                    RecordsAffected = keyCommand.ExecuteNonQuery()
                    Return RecordsAffected
                Catch e As Exception
                    HQ.dataError = e.Message
                    HQ.WriteLogEvent("Database Error: " & e.Message)
                    HQ.WriteLogEvent("SQL: " & strSQL)
                    Return -2
                End Try
            Case 1 ' MSSQL
                Try
                    Dim keyCommand As New SqlCommand(strSQL, customSQLConnection)
                    keyCommand.CommandTimeout = HQ.EveHqSettings.DBTimeout
                    RecordsAffected = keyCommand.ExecuteNonQuery()
                    Return RecordsAffected
                Catch e As Exception
                    HQ.dataError = e.Message
                    HQ.WriteLogEvent("Database Error: " & e.Message)
                    HQ.WriteLogEvent("SQL: " & strSQL)
                    Return -2
                End Try
            Case Else
                HQ.dataError = "Cannot Enumerate Database Format"
                Return -2
        End Select
    End Function

    Public Shared Function GetData(ByVal strSQL As String, Optional ByVal replacementExempt As Boolean = False) As DataSet

        Dim EveHQData As New DataSet
        EveHQData.Clear()
        EveHQData.Tables.Clear()

        If strSQL.Contains(" LIKE ") = False And strSQL.Contains(" IN ") = False And replacementExempt = False Then
            strSQL = strSQL.Replace("'", "''")
            strSQL = strSQL.Replace(ControlChars.Quote, "'")
            strSQL = strSQL.Replace("=true", "=1")
        End If

        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim da As New SqlCeDataAdapter(strSQL, conn)
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    Dim msg As New StringBuilder
                    msg.AppendLine("Database1: " & HQ.EveHqSettings.DBFilename)
                    msg.AppendLine("Database2: " & HQ.EveHqSettings.DBDataFilename)
                    msg.AppendLine("Using App: " & HQ.EveHqSettings.UseAppDirectoryForDB.ToString)
                    msg.AppendLine("Connection String: " & conn.ConnectionString)
                    msg.AppendLine("SQL: " & strSQL)
                    msg.AppendLine("Message: " & e.Message)
                    If e.InnerException IsNot Nothing Then
                        msg.AppendLine("Inner Ex: " & e.InnerException.Message)
                    End If
                    HQ.dataError = msg.ToString
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim da As New SqlDataAdapter(strSQL, conn)
                    da.SelectCommand.CommandTimeout = HQ.EveHqSettings.DBTimeout
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case Else
                HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function

    Public Shared Function GetCustomData(ByVal strSQL As String) As DataSet

        Dim EveHQData As New DataSet
        EveHQData.Clear()
        EveHQData.Tables.Clear()

        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim da As New SqlCeDataAdapter(strSQL, conn)
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case 1 ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim da As New SqlDataAdapter(strSQL, conn)
                    da.SelectCommand.CommandTimeout = HQ.EveHqSettings.DBTimeout
                    da.Fill(EveHQData, "EveHQData")
                    conn.Close()
                    Return EveHQData
                Catch e As Exception
                    HQ.dataError = e.Message
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case Else
                HQ.dataError = "Cannot Enumerate Database Format"
                Return Nothing
        End Select
    End Function

    Public Shared Function GetBPTypeID(ByVal typeID As String) As String
        Dim eveData As DataSet = GetData("SELECT * FROM invBlueprintTypes WHERE productTypeID=" & typeID & ";")
        If eveData.Tables(0).Rows.Count = 0 Then
            Return typeID
        Else
            typeID = eveData.Tables(0).Rows(0).Item("blueprintTypeID").ToString
            Return typeID
        End If
    End Function

    Public Shared Function GetTypeID(ByVal bpTypeID As String) As String
        Dim eveData As DataSet = GetData("SELECT * FROM invBlueprintTypes WHERE blueprintTypeID=" & bpTypeID & ";")
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
        Dim eveData As DataSet = GetData(strSQL)
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
            HQ.itemData.Clear()
            ' Get type data
            ' Retribution 1.0 DB: introduced null typenames 
            Dim strSQL As String =
                    "SELECT invGroups.categoryID, invTypes.typeID, invTypes.groupID, invTypes.typeName, invTypes.volume, invTypes.portionSize, invTypes.basePrice, invTypes.published, invTypes.marketGroupID FROM invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID where typeName is not null;"
            itemData = GetData(strSQL)
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
                        HQ.itemData.Add(CStr(newItem.ID), newItem)
                    Next
                    ' Get the MetaLevel data
                    strSQL = "SELECT * FROM dgmTypeAttributes WHERE attributeID=633;"
                    itemData = GetData(strSQL)
                    If itemData.Tables(0).Rows.Count > 0 Then
                        For Each itemRow As DataRow In itemData.Tables(0).Rows
                            If HQ.itemData.ContainsKey(CStr(itemRow.Item("typeID"))) Then
                                newItem = HQ.itemData(CStr(itemRow.Item("typeID")))
                                If IsDBNull(itemRow.Item("valueInt")) = False Then
                                    newItem.MetaLevel = CInt(itemRow.Item("valueInt"))
                                Else
                                    newItem.MetaLevel = CInt(itemRow.Item("valueFloat"))
                                End If
                            End If
                        Next
                        ' Get the icon data
                        strSQL =
                            "SELECT invTypes.typeID, eveIcons.iconFile FROM eveIcons INNER JOIN invTypes ON eveIcons.iconID = invTypes.iconID;"
                        itemData = GetData(strSQL)
                        If itemData.Tables(0).Rows.Count > 0 Then
                            For Each itemRow As DataRow In itemData.Tables(0).Rows
                                If HQ.itemData.ContainsKey(CStr(itemRow.Item("typeID"))) Then
                                    newItem = HQ.itemData(CStr(itemRow.Item("typeID")))
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
            MessageBox.Show("Error Loading Item Data:" & ControlChars.CrLf & ex.Message, "Load Items Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Public Shared Function LoadItems() As Boolean

        ' Initally load the new item data routine
        Call LoadItemData()
        Call MarketFunctions.LoadItemMarketGroups()

        HQ.itemList.Clear()
        HQ.itemGroups.Clear()
        HQ.itemCats.Clear()
        HQ.groupCats.Clear()
        Dim eveData As New DataSet
        Try

            Dim iKey As String = ""
            Dim iValue As String = ""
            Dim iParent As String = ""
            Dim iPublished As Boolean = False
            Dim iBasePrice As String = ""
            ' Load categories
            eveData = GetData("SELECT * FROM invCategories ORDER BY categoryName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iValue = eveData.Tables(0).Rows(item).Item("categoryName").ToString.Trim
                iKey = eveData.Tables(0).Rows(item).Item("categoryID").ToString.Trim
                HQ.itemCats.Add(iKey, iValue)
            Next
            ' Load groups
            eveData = GetData("SELECT * FROM invGroups ORDER BY groupName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iValue = eveData.Tables(0).Rows(item).Item("groupName").ToString.Trim
                iKey = eveData.Tables(0).Rows(item).Item("groupID").ToString.Trim
                iParent = eveData.Tables(0).Rows(item).Item("categoryID").ToString.Trim
                HQ.itemGroups.Add(iKey, iValue)
                HQ.groupCats.Add(iKey, iParent)
            Next
            ' Load items
            eveData = GetData("SELECT * FROM invTypes ORDER BY typeName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iKey = eveData.Tables(0).Rows(item).Item("typeName").ToString.Trim
                iValue = eveData.Tables(0).Rows(item).Item("typeID").ToString.Trim
                iParent = eveData.Tables(0).Rows(item).Item("groupID").ToString.Trim
                iBasePrice = eveData.Tables(0).Rows(item).Item("basePrice").ToString.Trim
                iPublished = CBool(eveData.Tables(0).Rows(item).Item("published"))
                If HQ.itemList.ContainsKey(iKey) = False Then
                    HQ.itemList.Add(iKey, iValue)
                End If
            Next
            ' Load Certificate data
            Call LoadCertCategories()
            Call LoadCertClasses()
            Call LoadCerts()
            Call LoadCertReqs()
            ' Load the "unlock" (dependancy) data
            If LoadUnlocks() = False Then
                If eveData IsNot Nothing Then
                    eveData.Dispose()
                End If
                Return False
            End If
            ' Load price data
            LoadCustomPricesFromDB()

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
            strSQL &=
                "SELECT invTypes.typeID AS invTypeID, invTypes.groupID, invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, invTypes.published"
            strSQL &= " FROM invTypes INNER JOIN dgmTypeAttributes ON invTypes.typeID = dgmTypeAttributes.typeID"
            strSQL &=
                " WHERE (((dgmTypeAttributes.attributeID) IN (182,183,184,277,278,279,1285,1286,1287,1288,1289,1290)) AND (invTypes.published=1))"
            strSQL &= " ORDER BY invTypes.typeID, dgmTypeAttributes.attributeID;"
            Dim attData As DataSet = GetData(strSQL)
            Dim lastAtt As String = "0"
            Dim skillIDLevel As String = ""
            Dim atts As Double = 0
            Dim itemList As New ArrayList
            Dim attValue As Double
            For row As Integer = 0 To attData.Tables(0).Rows.Count - 1
                If attData.Tables(0).Rows(row).Item("invTypeID").ToString <> lastAtt Then
                    Dim attRows() As DataRow =
                            attData.Tables(0).Select(
                                "invTypeID=" & attData.Tables(0).Rows(row).Item("invtypeID").ToString)
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
                            itemList.Add(
                                skillIDLevel & "_" & attData.Tables(0).Rows(row).Item("invtypeID").ToString & "_" &
                                attData.Tables(0).Rows(row).Item("groupID").ToString)
                        End If
                    Next
                    lastAtt = CStr(attData.Tables(0).Rows(row).Item("invtypeID"))
                End If
            Next

            ' Place the items into the Shared arrays
            Dim items(2) As String
            Dim itemUnlocked As New ArrayList
            Dim certUnlocked As New ArrayList
            HQ.SkillUnlocks.Clear()
            HQ.ItemUnlocks.Clear()
            For Each item As String In itemList
                items = item.Split(CChar("_"))
                If HQ.SkillUnlocks.ContainsKey(items(0)) = False Then
                    ' Create an arraylist and add the item
                    itemUnlocked = New ArrayList
                    itemUnlocked.Add(items(1) & "_" & items(2))
                    HQ.SkillUnlocks.Add(items(0), itemUnlocked)
                Else
                    ' Fetch the item and add the new one
                    itemUnlocked = HQ.SkillUnlocks(items(0))
                    itemUnlocked.Add(items(1) & "_" & items(2))
                End If
                If HQ.ItemUnlocks.ContainsKey(items(1)) = False Then
                    ' Create an arraylist and add the item
                    itemUnlocked = New ArrayList
                    itemUnlocked.Add(items(0))
                    HQ.ItemUnlocks.Add(items(1), itemUnlocked)
                Else
                    ' Fetch the item and add the new one
                    itemUnlocked = HQ.ItemUnlocks(items(1))
                    itemUnlocked.Add(items(0))
                End If
            Next
            ' Add certificates into the skill unlocks?
            For Each cert As Certificate In HQ.Certificates.Values
                For Each skill As String In cert.RequiredSkills.Keys
                    Dim skillID As String = skill & "." & cert.RequiredSkills(skill).ToString
                    If HQ.CertUnlockSkills.ContainsKey(skillID) = False Then
                        ' Create an arraylist and add the item
                        certUnlocked = New ArrayList
                        certUnlocked.Add(cert.ID)
                        HQ.CertUnlockSkills.Add(skillID, certUnlocked)
                    Else
                        ' Fetch the item and add the new one
                        certUnlocked = HQ.CertUnlockSkills(skillID)
                        certUnlocked.Add(cert.ID)
                    End If
                Next
                For Each certID As String In cert.RequiredCerts.Keys
                    If HQ.CertUnlockCerts.ContainsKey(certID) = False Then
                        ' Create an arraylist and add the item
                        certUnlocked = New ArrayList
                        certUnlocked.Add(cert.ID)
                        HQ.CertUnlockCerts.Add(certID, certUnlocked)
                    Else
                        ' Fetch the item and add the new one
                        certUnlocked = HQ.CertUnlockCerts(certID)
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
        Return GetPrice(itemID, MarketMetric.Default, MarketTransactionKind.Default)
    End Function
    Public Shared Function GetPrice(ByVal itemID As String, ByVal metric As MarketMetric, ByVal transType As MarketTransactionKind) As Double
        Dim task As Task(Of Dictionary(Of String, Double)) = GetMarketPrices(New String() {itemID}, metric, transType)
        task.Wait()
        Return task.Result.Where(Function(pair) pair.Key = itemID).Select(Function(pair) pair.Value).FirstOrDefault()
    End Function


    Public Shared Function GetPriceAsync(ByVal itemID As String) As Task(Of Double)
        Return GetPriceAsync(itemID, MarketMetric.Default, MarketTransactionKind.Default)
    End Function

    Public Shared Function GetPriceAsync(ByVal itemID As String, ByVal metric As MarketMetric, ByVal transType As MarketTransactionKind) As Task(Of Double)
        Dim task As Task(Of Dictionary(Of String, Double)) = GetMarketPrices(New String() {itemID}, metric, transType)

        Dim task2 As Task(Of Double) = task.ContinueWith(Function(priceTask As Task(Of Dictionary(Of String, Double))) As Double
                                                             If priceTask.IsCompleted And priceTask.IsFaulted = False Then
                                                                 Return priceTask.Result(itemID)
                                                             End If
                                                             Return 0

                                                         End Function)

        Return task2
    End Function

    Public Shared Function GetMarketPrices(ByVal itemIDs As IEnumerable(Of String)) As Task(Of Dictionary(Of String, Double))
        Return GetMarketPrices(itemIDs, MarketMetric.Default, MarketTransactionKind.Default)
    End Function

    Public Shared Function GetMarketPrices(ByVal itemIDs As IEnumerable(Of String), ByVal metric As MarketMetric, ByVal transType As MarketTransactionKind) As Task(Of Dictionary(Of String, Double))
        If metric = MarketMetric.Default Then
            metric = HQ.EveHqSettings.MarketDefaultMetric
        End If
        If transType = MarketTransactionKind.Default Then
            transType = HQ.EveHqSettings.MarketDefaultTransactionType
        End If

        Dim dataTask As Task(Of IEnumerable(Of ItemOrderStats))
        Dim resultTask As Task(Of Dictionary(Of String, Double))

        If itemIDs IsNot Nothing Then
            If itemIDs.Any() Then
                ' Go through the list of id's provided and only get the items that have a valid market group.
                Dim filteredIdNumbers As IEnumerable(Of String) = (From itemId In itemIDs Where HQ.itemData.ContainsKey(itemId))

                Dim itemIdNumbersToRequest As IEnumerable(Of Integer) = (From itemId In filteredIdNumbers Where HQ.itemData(itemId).MarketGroup <> 0 Select itemId.ToInt())

                If itemIdNumbersToRequest Is Nothing Then
                    itemIdNumbersToRequest = New List(Of Integer)
                End If

                If (itemIdNumbersToRequest.Any()) Then
                    'Fetch all the item prices in a single request
                    If HQ.EveHqSettings.MarketUseRegionMarket Then
                        dataTask = HQ.MarketStatDataProvider.GetOrderStats(itemIdNumbersToRequest, HQ.EveHqSettings.MarketRegions, Nothing, 1)
                    Else
                        dataTask = HQ.MarketStatDataProvider.GetOrderStats(itemIdNumbersToRequest, Nothing, HQ.EveHqSettings.MarketSystem, 1)
                    End If

                    ' Still need to do this in a synchronous fashion...unfortunately
                    resultTask = dataTask.ContinueWith(Function(markettask As Task(Of IEnumerable(Of ItemOrderStats))) As Dictionary(Of String, Double)


                                                           Return ProcessPriceTaskData(markettask, itemIDs, metric, transType)


                                                       End Function)
                Else
                    resultTask = Task(Of Dictionary(Of String, Double)).Factory.TryRun(Function() As Dictionary(Of String, Double)
                                                                                           'Empty Result
                                                                                           Return itemIDs.ToDictionary(Of String, Double)(Function(id) id, Function(id) 0)
                                                                                       End Function)
                End If
            Else
                resultTask = Task(Of Dictionary(Of String, Double)).Factory.TryRun(Function() As Dictionary(Of String, Double)
                                                                                       'Empty Result
                                                                                       Return itemIDs.ToDictionary(Of String, Double)(Function(id) id, Function(id) 0)
                                                                                   End Function)

            End If
        Else
            resultTask = Task(Of Dictionary(Of String, Double)).Factory.TryRun(Function() As Dictionary(Of String, Double)
                                                                                   'Empty Result
                                                                                   Return itemIDs.ToDictionary(Of String, Double)(Function(id) id, Function(id) 0)
                                                                               End Function)
        End If

        Return resultTask
    End Function

    Private Shared Function ProcessPriceTaskData(markettask As Task(Of IEnumerable(Of ItemOrderStats)), itemIDs As IEnumerable(Of String), ByVal metric As MarketMetric, ByVal transType As MarketTransactionKind) As Dictionary(Of String, Double)

        ' TODO: Web exceptions and otheres can be thrown here... need to protect upstream code.

        ' TODO: ItemIds are integers but through out the existing code they are inconsistently treated as strings (or longs...)... must fix that.
        Dim itemPrices As New Dictionary(Of String, Double)

        Dim distinctItems As IEnumerable(Of String) = itemIDs.Distinct()

        ' Initialize all items to have a default price of 0 (provides a safe default for items being requested that do not have a valid marketgroup)
        itemPrices = distinctItems.ToDictionary(Of String, Double)(Function(item) item, Function(item) 0)

        If markettask.Exception Is Nothing Then

            Try
                Dim result As IEnumerable(Of ItemOrderStats) = Nothing
                Dim itemResult As ItemOrderStats = Nothing
                If markettask.IsCompleted And markettask.IsFaulted = False And markettask.Result IsNot Nothing Then
                    If markettask.Result.Any() Then
                        result = markettask.Result
                    End If
                End If

                For Each itemId As String In distinctItems 'We only need to process the unique id results.
                    Try
                        If result IsNot Nothing Then
                            itemResult = (From item In result Where item.ItemTypeId.ToString() = itemId Select item).FirstOrDefault()
                        End If

                        ' If there is a custom price set, use that if not get it from the provider.
                        If HQ.CustomPriceList.ContainsKey(itemId) = True Then
                            itemPrices(itemId) = CDbl(HQ.CustomPriceList(itemId))
                        ElseIf itemResult IsNot Nothing Then
                            ' if there's a market provider result use that

                            Dim itemMetric As MarketMetric = metric
                            Dim itemTransKind As MarketTransactionKind = transType
                            ' check to see if the item has a configured overrided for metric and trans type
                            Dim override As New ItemMarketOverride
                            If (HQ.EveHqSettings.MarketStatOverrides.TryGetValue(itemResult.ItemTypeId, override)) Then
                                itemMetric = override.MarketStat
                                itemTransKind = override.TransactionType
                            End If


                            Dim orderStat As OrderStats
                            ' get the right transaction type
                            Select Case itemTransKind
                                Case MarketTransactionKind.All
                                    orderStat = itemResult.All
                                Case MarketTransactionKind.Buy
                                    orderStat = itemResult.Buy
                                Case Else
                                    orderStat = itemResult.Sell
                            End Select

                            Select Case itemMetric
                                Case MarketMetric.Average
                                    itemPrices(itemId) = orderStat.Average
                                Case MarketMetric.Maximum
                                    itemPrices(itemId) = orderStat.Maximum
                                Case MarketMetric.Median
                                    itemPrices(itemId) = orderStat.Median
                                Case MarketMetric.Percentile
                                    itemPrices(itemId) = orderStat.Percentile
                                Case Else
                                    itemPrices(itemId) = orderStat.Minimum
                            End Select
                        Else
                            ' failing all that, fallback onto the base price.
                            If HQ.itemData.ContainsKey(itemId) Then
                                itemPrices(itemId) = HQ.itemData(itemId).BasePrice
                            Else
                                itemPrices(itemId) = 0
                            End If
                        End If
                    Catch e As Exception
                        If HQ.itemData.ContainsKey(itemId) Then
                            itemPrices(itemId) = HQ.itemData(itemId).BasePrice
                        Else
                            itemPrices(itemId) = 0
                        End If
                    End Try
                Next
            Catch ex As Exception
                Trace.TraceError(ex.FormatException())
            End Try
        Else
            Trace.TraceError(markettask.Exception.FormatException())
        End If

        Return itemPrices
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
                Dim strSQL2 As String = "UPDATE dgmAttributeTypes SET attributeGroup=" & fields(1) &
                                        " WHERE attributeID=" & fields(0) & ";"
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



    Public Shared Function LoadCustomPricesFromDB() As Boolean
        Dim eveData As New DataSet
        Try
            eveData = GetCustomData("SELECT * FROM customPrices ORDER BY typeID;")
            If eveData IsNot Nothing Then
                HQ.CustomPriceList.Clear()
                For Each priceRow As DataRow In eveData.Tables(0).Rows
                    HQ.CustomPriceList.Add(CStr(priceRow.Item("typeID")), CDbl(priceRow.Item("price")))
                Next
            Else
                ' Doesn't look like the table is there so try creating it
                Call CreateCustomPricesTable()
            End If
        Catch ex As Exception
            MessageBox.Show(
                "There was an error fetching the Custom Price data. The error was: " & ControlChars.CrLf &
                ControlChars.CrLf & HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation)
        Finally
            If eveData IsNot Nothing Then
                eveData.Dispose()
            End If
        End Try
    End Function

    Public Shared Function CreateCustomPricesTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = GetDatabaseTables()
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
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." &
                                ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." &
                   ControlChars.CrLf
            msg &=
                "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateEveHQDataDB() = False Then
                MessageBox.Show(
                    "There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
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
            If SetData(strSQL.ToString) <> -2 Then
                Return True
            Else
                MessageBox.Show(
                    "There was an error creating the Custom Prices database table. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
    End Function

    Public Shared Function CreateMarketPricesTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = GetDatabaseTables()
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
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." &
                                ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." &
                   ControlChars.CrLf
            msg &=
                "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateEveHQDataDB() = False Then
                MessageBox.Show(
                    "There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Market Stats Database", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
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
            If SetData(strSQL.ToString) <> -2 Then
                Return True
            Else
                MessageBox.Show(
                    "There was an error creating the Market Prices database table. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
    End Function

    Public Shared Function SetCustomPrice(ByVal typeID As Long, ByVal UserPrice As Double, ByVal DBOpen As Boolean) _
        As Boolean
        ' Store the user's price in the database
        If HQ.CustomPriceList.ContainsKey(typeID.ToString) = False Then
            ' Add the data
            If AddCustomPrice(typeID.ToString, UserPrice, DBOpen) = True Then
                Return True
            Else
                Return False
            End If
        Else
            ' Edit the data
            If EditCustomPrice(typeID.ToString, UserPrice, DBOpen) = True Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Shared Function AddCustomPrice(ByVal itemID As String, ByVal price As Double, ByVal DBOpen As Boolean) _
        As Boolean
        HQ.CustomPriceList(itemID) = price
        Dim priceSQL As String = "INSERT INTO customPrices (typeID, price, priceDate) VALUES (" & itemID & ", " &
                                 price.ToString(culture) & ", '" & Now.ToString(SQLTimeFormat, culture) & "');"
        If DBOpen = False Then
            If SetData(priceSQL) = -2 Then
                MessageBox.Show(
                    "There was an error writing data to the Custom Prices database table. The error was: " &
                    ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf &
                    "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If SetDataOnly(priceSQL) = -2 Then
                MessageBox.Show(
                    "There was an error writing data to the Custom Prices database table. The error was: " &
                    ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf &
                    "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Public Shared Function EditCustomPrice(ByVal itemID As String, ByVal price As Double, ByVal DBOpen As Boolean) _
        As Boolean
        HQ.CustomPriceList(itemID) = price
        Dim priceSQL As String = "UPDATE customPrices SET price=" & price.ToString(culture) & ", priceDate='" &
                                 Now.ToString(SQLTimeFormat, culture) & "' WHERE typeID=" & itemID & ";"
        If DBOpen = False Then
            If SetData(priceSQL) = -2 Then
                MessageBox.Show(
                    "There was an error writing data to the Market Prices database table. The error was: " &
                    ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf &
                    "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Else
            If SetDataOnly(priceSQL) = -2 Then
                MessageBox.Show(
                    "There was an error writing data to the Market Prices database table. The error was: " &
                    ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf &
                    "Data: " & priceSQL, "Error Writing Price Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Public Shared Function DeleteCustomPrice(ByVal itemID As String) As Boolean
        ' Double check it exists and delete it
        If HQ.CustomPriceList.ContainsKey(itemID) = True Then
            HQ.CustomPriceList.Remove(itemID)
        End If
        Dim priceSQL As String = "DELETE FROM customPrices WHERE typeID=" & itemID & ";"
        If SetData(priceSQL) = -2 Then
            MessageBox.Show(
                "There was an error deleting data from the Custom Prices database table. The error was: " &
                ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " &
                priceSQL, "Error Writing Price Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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

        If _
            header <>
            "price,volRemaining,typeID,range,orderID,volEntered,minVolume,bid,issueDate,duration,stationID,regionID,solarSystemID,jumps," _
            Then
            MessageBox.Show("File is not a valid Eve Market Export file", "File Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
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

            items.Clear()
            items = Nothing
            itemOrders.Clear()
            itemOrders = Nothing
            GC.Collect()

            Return PriceData

        End If
    End Function

    Private Shared Function CalculateMarketExportStats(ByVal orderList As ArrayList, ByVal orderDate As Date,
                                                       WriteToDB As Boolean) As ArrayList
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
        sorBuy.Clear()
        sorSell.Clear()
        sorAll.Clear()

        countBuy = 0
        countSell = 0
        countAll = 0
        volBuy = 0
        volSell = 0
        volAll = 0
        minBuy = 0
        minSell = 0
        minAll = 0
        maxBuy = 0
        maxSell = 0
        maxAll = 0
        valBuy = 0
        valSell = 0
        valAll = 0
        devBuy = 0
        devSell = 0
        devAll = 0

        For Each order As String In orderList
            order = order.Replace(Chr(0), "")
            orderDetails = order.Split(",".ToCharArray)
            oPrice = Double.Parse(orderDetails(0).Trim, NumberStyles.Any, culture)
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
                If _
                    HQ.EveHqSettings.IgnoreSellOrders = True And
                    oPrice > (HQ.EveHqSettings.IgnoreSellOrderLimit * HQ.itemData(oTypeID.ToString).BasePrice) Then
                    ProcessOrder = False
                End If
            Else ' Buy Order
                If HQ.EveHqSettings.IgnoreBuyOrders = True And oPrice < HQ.EveHqSettings.IgnoreBuyOrderLimit Then
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
            avgAll = 0
            stdAll = 0
            medAll = 0
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
            avgSell = 0
            stdSell = 0
            medSell = 0
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
            avgBuy = 0
            stdBuy = 0
            medBuy = 0
        End If

        'Calculate the user price
        Dim priceArray As New ArrayList
        priceArray.Add(avgBuy)
        priceArray.Add(medBuy)
        priceArray.Add(minBuy)
        priceArray.Add(maxBuy)
        priceArray.Add(avgSell)
        priceArray.Add(medSell)
        priceArray.Add(minSell)
        priceArray.Add(maxSell)
        priceArray.Add(avgAll)
        priceArray.Add(medAll)
        priceArray.Add(minAll)
        priceArray.Add(maxAll)

        priceArray.Add(MarketFunctions.CalculateUserPriceFromPriceArray(priceArray, oReg.ToString, oTypeID.ToString,
                                                                        WriteToDB))

        priceArray.Add(oTypeID)
        priceArray.Add(volBuy)
        priceArray.Add(volSell)
        priceArray.Add(volAll)

        Return priceArray
    End Function

    Public Shared Function CheckForIDNameTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = GetDatabaseTables()
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
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." &
                                ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." &
                   ControlChars.CrLf
            msg &=
                "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateEveHQDataDB() = False Then
                MessageBox.Show(
                    "There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Custom Database", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
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
            If SetData(strSQL.ToString) <> -2 Then
                Return True
            Else
                MessageBox.Show(
                    "There was an error creating the Eve ID-To-Name database table. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Eve ID-To-Name Table", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
    End Function

    Public Shared Function CheckForEveMailTable() As Integer
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = GetDatabaseTables()
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
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." &
                                ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." &
                   ControlChars.CrLf
            msg &=
                "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateEveHQDataDB() = False Then
                MessageBox.Show(
                    "There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Custom Database", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return -1
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
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
            If SetData(strSQL.ToString) <> -2 Then
                Return 0
            Else
                MessageBox.Show(
                    "There was an error creating the Eve Mail database table. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Eve Mail Table", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return -1
            End If
        End If
    End Function

    Public Shared Function CheckForEveNotificationTable() As Integer
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = GetDatabaseTables()
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
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." &
                                ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." &
                   ControlChars.CrLf
            msg &=
                "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If CreateEveHQDataDB() = False Then
                MessageBox.Show(
                    "There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf &
                    ControlChars.CrLf & HQ.dataError, "Error Creating Custom Database", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation)
                Return -1
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
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
            If SetData(strSQL.ToString) <> -2 Then
                Return 0
            Else
                MessageBox.Show(
                    "There was an error creating the Eve Notification database table. The error was: " &
                    ControlChars.CrLf & ControlChars.CrLf & HQ.dataError, "Error Creating Eve Notification Table",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return -1
            End If
        End If
    End Function

    Public Shared Function CheckForEveMailBodyColumn() As Integer
        Dim strSQL As String = "SELECT * FROM eveMail;"
        Dim ColData As DataSet = GetCustomData(strSQL)

        ' Get the index of the field name
        Dim i As Integer = ColData.Tables(0).Columns.IndexOf("messageBody")

        If i = -1 Then
            'Field is missing
            Dim MailSQL As String = "ALTER TABLE eveMail ADD messageBody ntext NULL;"
            Return (SetData(MailSQL))
        Else
            Return 0
        End If
    End Function

    Public Shared Function CheckForEveNotificationBodyColumn() As Integer
        Dim strSQL As String = "SELECT * FROM eveNotifications;"
        Dim ColData As DataSet = GetCustomData(strSQL)

        ' Get the index of the field name
        Dim i As Integer = ColData.Tables(0).Columns.IndexOf("messageBody")

        If i = -1 Then
            'Field is missing
            Dim MailSQL As String = "ALTER TABLE eveNotifications ADD messageBody ntext NULL;"
            Return (SetData(MailSQL))
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
                HQ.WriteLogEvent("***** Start: Request Eve IDs From API *****")

                HQ.WriteLogEvent("Building Required ID List")
                Dim strID As New StringBuilder
                For Each ID As String In ReqIDList
                    strID.Append("," & ID)
                Next
                strID.Remove(0, 1)
                HQ.WriteLogEvent("Finishing building list of " & ReqIDList.Count.ToString & " IDs")

                ' Get a list of everything we already have
                HQ.WriteLogEvent("Querying existing IDToName data")
                Dim ExistingIDs As New List(Of String)
                Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                Dim IDData As DataSet = GetCustomData(strSQL)
                If IDData IsNot Nothing Then
                    If IDData.Tables(0).Rows.Count > 0 Then
                        For Each IDRow As DataRow In IDData.Tables(0).Rows
                            ExistingIDs.Add(CStr(IDRow.Item("eveID")))
                        Next
                    End If
                End If

                HQ.WriteLogEvent("Removing existing IDs from the list")
                ' Remove existing IDs from the parsed list
                Dim RemoveCount As Integer = 0
                For Each existingID As String In ExistingIDs
                    If ReqIDList.Contains(existingID) Then
                        ReqIDList.Remove(existingID)
                        RemoveCount += 1
                    End If
                Next
                HQ.WriteLogEvent("Finished Removing " & RemoveCount.ToString & " existing IDs from the list")

                If ReqIDList.Count > 0 Then

                    HQ.WriteLogEvent("Rebuilding Required ID List for sending to the API")
                    strID = New StringBuilder
                    For Each ID As String In ReqIDList
                        strID.Append("," & ID)
                    Next
                    If strID.Length > 1 Then
                        strID.Remove(0, 1)
                    End If
                    HQ.WriteLogEvent("Finishing building list of " & ReqIDList.Count.ToString & " IDs for the API")

                    ' Send this to the API if we have something!
                    HQ.WriteLogEvent("Requesting ID List From the API: " & strID.ToString)
                    Dim _
                        APIReq As _
                            New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.EveHqSettings.APIFileExtension,
                                              HQ.cacheFolder)
                    Dim IDXML As XmlDocument = APIReq.GetAPIXML(APITypes.IDToName, strID.ToString,
                                                                APIReturnMethods.ReturnActual)
                    ' Parse this XML
                    Dim FinalIDs As New SortedList(Of Long, String)
                    Dim IDList As XmlNodeList
                    Dim IDNode As XmlNode
                    Dim eveID As Long = 0
                    Dim eveName As String = ""
                    If IDXML IsNot Nothing Then
                        IDList = IDXML.SelectNodes("/eveapi/result/rowset/row")
                        If IDList.Count > 0 Then
                            HQ.WriteLogEvent("Parsing " & IDList.Count.ToString & " IDs in the XML file")
                            For Each IDNode In IDList
                                eveID = CLng(IDNode.Attributes.GetNamedItem("characterID").Value)
                                eveName = IDNode.Attributes.GetNamedItem("name").Value
                                If FinalIDs.ContainsKey(eveID) = False Then
                                    FinalIDs.Add(eveID, eveName)
                                End If
                            Next
                        Else
                            If APIReq.LastAPIError > 0 Then
                                HQ.WriteLogEvent("Error " & APIReq.LastAPIError.ToString & " returned by the API!")
                            End If
                        End If

                        ' Add all the data to the database
                        HQ.WriteLogEvent("Creating SQL Query for IDToName data")
                        Dim strIDInsert As String = "INSERT INTO eveIDToName (eveID, eveName) VALUES "
                        For Each eveID In FinalIDs.Keys
                            If ExistingIDs.Contains(eveID.ToString) = False Then
                                eveName = FinalIDs(eveID)
                                Dim uSQL As New StringBuilder
                                uSQL.Append(strIDInsert)
                                uSQL.Append("(" & eveID & ", ")
                                uSQL.Append("'" & eveName.Replace("'", "''") & "');")
                                HQ.WriteLogEvent("Writing IDToName data to database")
                                If SetData(uSQL.ToString) = -2 Then
                                    'MessageBox.Show("There was an error writing data to the Eve ID database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve IDs", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)  
                                End If
                            End If
                        Next
                    Else
                        HQ.WriteLogEvent("ID XML returned nothing from the API")
                    End If
                Else
                    HQ.WriteLogEvent("All IDs present in the database, no request required to the API")
                End If

                ' Write to log file about ID request
                HQ.WriteLogEvent("***** End: Request Eve IDs From API *****")

            Loop Until MainIDList.Count = 0
        End If
    End Sub

    Public Shared Function WriteMailingListIDsToDatabase(ByVal mPilot As Pilot) As SortedList(Of Long, String)
        Dim FinalIDs As New SortedList(Of Long, String)
        Dim accountName As String = mPilot.Account
        If accountName <> "" Then
            Dim mAccount As EveAccount = CType(HQ.EveHqSettings.Accounts.Item(accountName), EveAccount)
            ' Send this to the API
            Dim _
                APIReq As _
                    New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.EveHqSettings.APIFileExtension,
                                      HQ.cacheFolder)
            Dim IDXML As XmlDocument = APIReq.GetAPIXML(APITypes.MailingLists, mAccount.ToAPIAccount, mPilot.ID,
                                                        APIReturnMethods.ReturnStandard)
            ' Parse this XML
            Dim IDList As XmlNodeList
            Dim eveID As Long = 0
            Dim eveName As String = ""
            If IDXML IsNot Nothing Then
                IDList = IDXML.SelectNodes("/eveapi/result/rowset/row")
                For Each IDNode As XmlNode In IDList
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
                If SetData(uSQL.ToString) = -2 Then
                    'MessageBox.Show("There was an error writing data to the Eve ID database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve IDs", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            Next
        End If
        Return FinalIDs
    End Function

    Public Shared Function CheckDatabaseConnection(ByVal silentResponse As Boolean) As Boolean
        Dim strConnection As String = ""
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                strConnection = "Data Source = " & ControlChars.Quote & HQ.EveHqSettings.DBFilename & ControlChars.Quote &
                                ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Dim connection As New SqlCeConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    If silentResponse = False Then
                        MessageBox.Show("Connected successfully to SQL CE database", "Connection Successful",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    If silentResponse = False Then
                        MessageBox.Show(ex.Message, "Error Opening SQL CE Database", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
                    End If
                    Return False
                End Try
            Case 1 ' SQL
                strConnection = "Server=" & HQ.EveHqSettings.DBServer
                If HQ.EveHqSettings.DBSQLSecurity = True Then
                    strConnection += "; Database = " & HQ.EveHqSettings.DBName & "; User ID=" &
                                     HQ.EveHqSettings.DBUsername & "; Password=" & HQ.EveHqSettings.DBPassword & ";"
                Else
                    strConnection += "; Database = " & HQ.EveHqSettings.DBName & "; Integrated Security = SSPI;"
                End If
                Dim connection As New SqlConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    If silentResponse = False Then
                        MessageBox.Show("Connected successfully to MS SQL database", "Connection Successful",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    If silentResponse = False Then
                        MessageBox.Show(ex.Message, "Error Opening MS SQL Database", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
                    End If
                    Return False
                End Try
        End Select
    End Function

    Public Shared Function CheckDataDatabaseConnection(ByVal silentResponse As Boolean) As Boolean
        Dim strConnection As String = ""
        Select Case HQ.EveHqSettings.DBFormat
            Case 0 ' SQL CE
                strConnection = "Data Source = " & ControlChars.Quote & HQ.EveHqSettings.DBDataFilename &
                                ControlChars.Quote & ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Dim connection As New SqlCeConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    If silentResponse = False Then
                        MessageBox.Show("Connected successfully to SQL CE database", "Connection Successful",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    If silentResponse = False Then
                        MessageBox.Show(ex.Message, "Error Opening SQL CE Database", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
                    End If
                    Return False
                End Try
            Case 1 ' SQL
                strConnection = "Server=" & HQ.EveHqSettings.DBServer
                If HQ.EveHqSettings.DBSQLSecurity = True Then
                    strConnection += "; Database = " & HQ.EveHqSettings.DBDataName & "; User ID=" &
                                     HQ.EveHqSettings.DBUsername & "; Password=" & HQ.EveHqSettings.DBPassword & ";"
                Else
                    strConnection += "; Database = " & HQ.EveHqSettings.DBDataName & "; Integrated Security = SSPI;"
                End If
                Dim connection As New SqlConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    If silentResponse = False Then
                        MessageBox.Show("Connected successfully to MS SQL database", "Connection Successful",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return True
                Catch ex As Exception
                    If silentResponse = False Then
                        MessageBox.Show(ex.Message, "Error Opening MS SQL Database", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
                    End If
                    Return False
                End Try
        End Select
    End Function

#Region "Solar System & Station Data Routines"

    Public Shared Function LoadSolarSystems() As Boolean
        Dim strSQL As String =
                "SELECT mapSolarSystems.regionID AS mapSolarSystems_regionID, mapSolarSystems.constellationID AS mapSolarSystems_constellationID, mapSolarSystems.solarSystemID, mapSolarSystems.solarSystemName, mapSolarSystems.x, mapSolarSystems.y, mapSolarSystems.z, mapSolarSystems.xMin, mapSolarSystems.xMax, mapSolarSystems.yMin, mapSolarSystems.yMax, mapSolarSystems.zMin, mapSolarSystems.zMax, mapSolarSystems.luminosity, mapSolarSystems.border, mapSolarSystems.fringe, mapSolarSystems.corridor, mapSolarSystems.hub, mapSolarSystems.international, mapSolarSystems.regional, mapSolarSystems.constellation, mapSolarSystems.security, mapSolarSystems.factionID, mapSolarSystems.radius, mapSolarSystems.sunTypeID, mapSolarSystems.securityClass, mapRegions.regionID AS mapRegions_regionID, mapRegions.regionName, mapConstellations.constellationID AS mapConstellations_constellationID, mapConstellations.constellationName"
        strSQL &=
            " FROM (mapRegions INNER JOIN mapConstellations ON mapRegions.regionID = mapConstellations.regionID) INNER JOIN mapSolarSystems ON mapConstellations.constellationID = mapSolarSystems.constellationID;"
        Dim systemData As DataSet = GetData(strSQL)
        Try
            If systemData IsNot Nothing Then
                If systemData.Tables(0).Rows.Count > 0 Then
                    Dim cSystem As SolarSystem = New SolarSystem
                    HQ.SolarSystemsById.Clear()
                    For solar As Integer = 0 To systemData.Tables(0).Rows.Count - 1
                        cSystem = New SolarSystem
                        cSystem.Id = CInt(systemData.Tables(0).Rows(solar).Item("solarSystemID"))
                        cSystem.Name = CStr(systemData.Tables(0).Rows(solar).Item("solarSystemName"))
                        cSystem.Region = CStr(systemData.Tables(0).Rows(solar).Item("regionName"))
                        cSystem.Constellation = CStr(systemData.Tables(0).Rows(solar).Item("constellationName"))
                        cSystem.Security = CDbl(systemData.Tables(0).Rows(solar).Item("security"))
                        HQ.SolarSystemsById.Add(CStr(cSystem.Id), cSystem)
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading System Data for Prism Plugin" & ControlChars.CrLf & ex.Message,
                            "Prism Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Public Shared Function LoadStations() As Boolean
        ' Load the Station Data
        Try
            Dim strSQL As String = "SELECT * FROM staStations;"
            Dim locationData As DataSet = GetData(strSQL)
            If locationData IsNot Nothing Then
                If locationData.Tables(0).Rows.Count > 0 Then
                    HQ.Stations.Clear()
                    For Each locationRow As DataRow In locationData.Tables(0).Rows
                        Dim newStation As New Station
                        newStation.stationID = CLng(locationRow.Item("stationID"))
                        newStation.stationName = CStr(locationRow.Item("stationName"))
                        newStation.systemID = CLng(locationRow.Item("solarSystemID"))
                        newStation.constID = CLng(locationRow.Item("constellationID"))
                        newStation.regionID = CLng(locationRow.Item("regionID"))
                        newStation.corpID = CLng(locationRow.Item("corporationID"))
                        newStation.refiningEff = CDbl(locationRow.Item("reprocessingEfficiency"))
                        HQ.Stations.Add(newStation.stationID.ToString, newStation)
                    Next
                    Return CheckForConqXMLFile()
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading Station Data for Prism Plugin" & ControlChars.CrLf & ex.Message,
                            "Prism Plug-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Shared Function CheckForConqXMLFile() As Boolean
        ' Check for the Conquerable XML file in the cache
        Dim stationXML As New XmlDocument
        Dim _
            APIReq As _
                New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.EveHqSettings.APIFileExtension,
                                  HQ.cacheFolder)
        stationXML = APIReq.GetAPIXML(APITypes.Conquerables, APIReturnMethods.ReturnStandard)
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
                If HQ.Stations.ContainsKey(stationID) = False Then
                    Dim cStation As New Station
                    cStation.stationID = CLng(stationID)
                    cStation.stationName = (loc.Attributes.GetNamedItem("stationName").Value)
                    cStation.systemID = CLng(loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As SolarSystem = HQ.SolarSystemsById(cStation.systemID.ToString)
                    cStation.stationName &= " (" & system.Name & ", " & system.Region & ")"
                    cStation.corpID = CLng(loc.Attributes.GetNamedItem("corporationID").Value)
                    HQ.Stations.Add(cStation.stationID.ToString, cStation)
                Else
                    Dim cStation As Station = CType(HQ.Stations(stationID), Station)
                    cStation.systemID = CLng(loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As SolarSystem = HQ.SolarSystemsById(cStation.systemID.ToString)
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
            If HQ.Stations.ContainsKey(LocationID) = True Then
                ' Known Outpost
                Return HQ.Stations(LocationID).stationName
            Else
                ' Unknown outpost!
                Return "Unknown Outpost"
            End If
        Else
            If CDbl(LocationID) < 60000000 Then
                If HQ.SolarSystemsById.ContainsKey(LocationID) Then
                    ' Known solar system
                    Return HQ.SolarSystemsById(LocationID).Name
                Else
                    ' Unknown solar system
                    Return "Unknown System"
                End If
            Else
                If HQ.Stations.ContainsKey(LocationID) Then
                    ' Known station
                    Return HQ.Stations(LocationID).stationName
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
                HQ.CertificateCategories.Clear()
                For Each CertRow As DataRow In CertData.Tables(0).Rows
                    Dim NewCat As New CertificateCategory
                    NewCat.ID = CInt(CertRow.Item("categoryID"))
                    NewCat.Name = CertRow.Item("categoryName").ToString
                    HQ.CertificateCategories.Add(NewCat.ID.ToString, NewCat)
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
                HQ.CertificateClasses.Clear()
                For Each CertRow As DataRow In CertData.Tables(0).Rows
                    Dim NewClass As New CertificateClass
                    NewClass.ID = CInt(CertRow.Item("classID"))
                    NewClass.Name = CertRow.Item("className").ToString
                    HQ.CertificateClasses.Add(NewClass.ID.ToString, NewClass)
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
                HQ.Certificates.Clear()
                For Each CertRow As DataRow In CertData.Tables(0).Rows
                    Dim NewCert As New Certificate
                    NewCert.ID = CInt(CertRow.Item("certificateID"))
                    NewCert.CategoryID = CInt(CertRow.Item("categoryID"))
                    NewCert.ClassID = CInt(CertRow.Item("classID"))
                    NewCert.Description = CStr(CertRow.Item("description"))
                    NewCert.Grade = CInt(CertRow.Item("grade"))
                    NewCert.CorpID = CInt(CertRow.Item("corpID"))
                    HQ.Certificates.Add(NewCert.ID.ToString, NewCert)
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
                    If HQ.Certificates.ContainsKey(certID) = True Then
                        Dim NewCert As Certificate = HQ.Certificates(certID)
                        If IsDBNull(CertRow.Item("parentID")) Then
                            ' This is a skill ID
                            NewCert.RequiredSkills.Add(CertRow.Item("parentTypeID").ToString,
                                                       CInt(CertRow.Item("parentLevel")))
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
        If HQ.IsUsingLocalFolders = False Then
            CoreCacheFolder = Path.Combine(HQ.AppDataFolder, "CoreCache")
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
        f.Serialize(s, HQ.itemData)
        s.Flush()
        s.Close()

        ' Item Market Groups
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemMarketGroups.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.ItemMarketGroups)
        s.Flush()
        s.Close()

        ' Item List
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemList.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.itemList)
        s.Flush()
        s.Close()

        ' Item Groups
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemGroups.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.itemGroups)
        s.Flush()
        s.Close()

        ' Items Cats
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemCats.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.itemCats)
        s.Flush()
        s.Close()

        ' Group Cats
        s = New FileStream(Path.Combine(CoreCacheFolder, "GroupCats.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.groupCats)
        s.Flush()
        s.Close()

        ' Cert Categories
        s = New FileStream(Path.Combine(CoreCacheFolder, "CertCats.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.CertificateCategories)
        s.Flush()
        s.Close()

        ' Cert Classes
        s = New FileStream(Path.Combine(CoreCacheFolder, "CertClasses.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.CertificateClasses)
        s.Flush()
        s.Close()

        ' Certs
        s = New FileStream(Path.Combine(CoreCacheFolder, "Certs.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.Certificates)
        s.Flush()
        s.Close()

        ' Unlocks
        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemUnlocks.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.ItemUnlocks)
        s.Flush()
        s.Close()

        ' SkillUnlocks
        s = New FileStream(Path.Combine(CoreCacheFolder, "SkillUnlocks.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.SkillUnlocks)
        s.Flush()
        s.Close()

        ' CertCerts
        s = New FileStream(Path.Combine(CoreCacheFolder, "CertCerts.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.CertUnlockCerts)
        s.Flush()
        s.Close()

        ' CertSkills
        s = New FileStream(Path.Combine(CoreCacheFolder, "CertSkills.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.CertUnlockSkills)
        s.Flush()
        s.Close()

        ' Solar Systems
        s = New FileStream(Path.Combine(CoreCacheFolder, "Systems.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.SolarSystemsById)
        s.Flush()
        s.Close()

        ' Stations
        s = New FileStream(Path.Combine(CoreCacheFolder, "Stations.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, HQ.Stations)
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
            If HQ.IsUsingLocalFolders = False Then
                CoreCacheFolder = Path.Combine(HQ.AppDataFolder, "CoreCache")
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
                        My.Computer.FileSystem.DeleteDirectory(CoreCacheFolder, DeleteDirectoryOption.DeleteAllContents)
                        HQ.WriteLogEvent("Core Cache outdated - rebuild of cache data required")
                        UseCoreCache = False
                    Else
                        HQ.WriteLogEvent("Core Cache still relevant - using existing cache data")
                        UseCoreCache = True
                    End If
                Else
                    ' Delete the existing cache folder and force a rebuild
                    My.Computer.FileSystem.DeleteDirectory(CoreCacheFolder, DeleteDirectoryOption.DeleteAllContents)
                    HQ.WriteLogEvent("Core Cache version file not found - rebuild of cache data required")
                    UseCoreCache = False
                End If

                If UseCoreCache = True Then

                    Try
                        ' Try to load from cache... in the event of a corrupt cache file, drop & rebuild

                        ' Get files from dump
                        Dim s As FileStream
                        Dim f As BinaryFormatter

                        ' Item Data
                        s = New FileStream(Path.Combine(CoreCacheFolder, "Items.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.itemData = CType(f.Deserialize(s), SortedList(Of String, EveItem))
                        s.Close()

                        ' Item Market Groups
                        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemMarketGroups.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.ItemMarketGroups = CType(f.Deserialize(s), SortedList(Of String, String))
                        s.Close()

                        ' Item List
                        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemList.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.itemList = CType(f.Deserialize(s), SortedList(Of String, String))
                        s.Close()

                        ' Item Groups
                        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemGroups.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.itemGroups = CType(f.Deserialize(s), SortedList(Of String, String))
                        s.Close()

                        ' Items Cats
                        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemCats.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.itemCats = CType(f.Deserialize(s), SortedList(Of String, String))
                        s.Close()

                        ' Group Cats
                        s = New FileStream(Path.Combine(CoreCacheFolder, "GroupCats.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.groupCats = CType(f.Deserialize(s), SortedList(Of String, String))
                        s.Close()

                        ' Cert Categories
                        s = New FileStream(Path.Combine(CoreCacheFolder, "CertCats.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.CertificateCategories = CType(f.Deserialize(s), SortedList(Of String, CertificateCategory))
                        s.Close()

                        ' Cert Classes
                        s = New FileStream(Path.Combine(CoreCacheFolder, "CertClasses.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.CertificateClasses = CType(f.Deserialize(s), SortedList(Of String, CertificateClass))
                        s.Close()

                        ' Certs
                        s = New FileStream(Path.Combine(CoreCacheFolder, "Certs.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.Certificates = CType(f.Deserialize(s), SortedList(Of String, Certificate))
                        s.Close()

                        ' Unlocks
                        s = New FileStream(Path.Combine(CoreCacheFolder, "ItemUnlocks.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.ItemUnlocks = CType(f.Deserialize(s), SortedList(Of String, ArrayList))
                        s.Close()

                        ' SkillUnlocks
                        s = New FileStream(Path.Combine(CoreCacheFolder, "SkillUnlocks.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.SkillUnlocks = CType(f.Deserialize(s), SortedList(Of String, ArrayList))
                        s.Close()

                        ' CertCerts
                        s = New FileStream(Path.Combine(CoreCacheFolder, "CertCerts.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.CertUnlockCerts = CType(f.Deserialize(s), SortedList(Of String, ArrayList))
                        s.Close()

                        ' CertSkills
                        s = New FileStream(Path.Combine(CoreCacheFolder, "CertSkills.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.CertUnlockSkills = CType(f.Deserialize(s), SortedList(Of String, ArrayList))
                        s.Close()

                        ' SolarSystemsById
                        s = New FileStream(Path.Combine(CoreCacheFolder, "Systems.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.SolarSystemsById = CType(f.Deserialize(s), SortedList(Of String, SolarSystem))
                        s.Close()

                        ' Stations
                        s = New FileStream(Path.Combine(CoreCacheFolder, "Stations.bin"), FileMode.Open)
                        f = New BinaryFormatter
                        HQ.Stations = CType(f.Deserialize(s), SortedList(Of String, Station))
                        s.Close()

                        ' Load price data
                        LoadCustomPricesFromDB()

                        Return True
                    Catch ex As Exception
                        ' todo log
                        Return False

                    End Try
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
