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
Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports System.IO

Public Class DataFunctions

    Shared eveData As Data.DataSet

    Public Shared Function CreateEveHQDataDB() As Boolean
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0 ' Access
                ' Get the directory of the existing Access database to write the new one there
                Dim FI As New FileInfo(EveHQ.Core.HQ.EveHQSettings.DBFilename)
                Dim outputFile As String = FI.DirectoryName & "\EveHQData.mdb"

                ' Try to create a new access db from resources
                Dim fs As New FileStream(outputFile, FileMode.Create)
                Dim bw As New BinaryWriter(fs)
                Try
                    bw.Write(My.Resources.EveHQDataDB)
                    bw.Close()
                    fs.Close()
                    EveHQ.Core.HQ.EveHQSettings.DBDataFilename = outputFile
                    Return True
                Catch e As Exception
                    EveHQ.Core.HQ.dataError = "Unable to create Access database in " & outputFile & ControlChars.CrLf & ControlChars.CrLf & e.Message
                    Return False
                Finally
                    fs.Dispose()
                End Try
            Case 1, 2, 3 ' MSSQL, MSSQL Express, MySQL
                Dim strSQL As String = "CREATE DATABASE EveHQData;"
                If EveHQ.Core.DataFunctions.SetData(strSQL) = True Then
                    EveHQ.Core.HQ.EveHQSettings.DBDataName = "EveHQData"
                    Return True
                Else
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
            Case 3 ' MySQL
                Dim conn As New MySqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim da As New MySqlDataAdapter("SHOW TABLES;", conn)
                    'da.SelectCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
                    Dim EveHQData As New DataSet
                    da.Fill(EveHQData, "EveHQData")
                    For Each table As DataRow In EveHQData.Tables(0).Rows
                        DBTables.Add(table.Item(0).ToString)
                    Next
                    conn.Close()
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
    Public Shared Sub SetEveHQConnectionString()

        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0
                If EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = False Then
                    EveHQ.Core.HQ.itemDBConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & EveHQ.Core.HQ.EveHQSettings.DBFilename
                Else
                    Dim FI As New IO.FileInfo(EveHQ.Core.HQ.EveHQSettings.DBFilename)
                    EveHQ.Core.HQ.itemDBConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & EveHQ.Core.HQ.appFolder & "\" & FI.Name
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

    End Sub
    Public Shared Sub SetEveHQDataConnectionString()

        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0
                If EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = False Then
                    EveHQ.Core.HQ.EveHQDataConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & EveHQ.Core.HQ.EveHQSettings.DBFilename
                Else
                    Dim FI As New IO.FileInfo(EveHQ.Core.HQ.EveHQSettings.DBFilename)
                    EveHQ.Core.HQ.EveHQDataConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & EveHQ.Core.HQ.appFolder & "\" & FI.Name
                End If
            Case 1
                EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; Integrated Security = SSPI;"
                End If
            Case 2
                EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & "\SQLEXPRESS"
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    EveHQ.Core.HQ.EveHQDataConnectionString += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & "; Integrated Security = SSPI;"
                End If
            Case 3
                EveHQ.Core.HQ.EveHQDataConnectionString = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & ";Database=" & EveHQ.Core.HQ.EveHQSettings.DBName.ToLower & ";Uid=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & ";Pwd=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
        End Select

    End Sub
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
            Case 3 ' MySQL
                Dim conn As New MySqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
                Try
                    conn.Open()
                    Dim keyCommand As New MySqlCommand(strSQL, conn)
                    'keyCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
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
            Case 3 ' MySQL
                Dim conn As New MySqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                Try
                    conn.Open()
                    Dim da As New MySqlDataAdapter(strSQL.ToLower, conn)
                    'da.SelectCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
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
        eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invBlueprintTypes WHERE productTypeID=" & typeID & ";")
        If eveData.Tables(0).Rows.Count = 0 Then
            Return typeID
        Else
            Return eveData.Tables(0).Rows(0).Item("blueprintTypeID").ToString
        End If
    End Function
    Public Shared Function GetTypeID(ByVal bpTypeID As String) As String
        eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invBlueprintTypes WHERE blueprintTypeID=" & bpTypeID & ";")
        If eveData.Tables(0).Rows.Count = 0 Then
            Return bpTypeID
        Else
            Return eveData.Tables(0).Rows(0).Item("productTypeID").ToString
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
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        For col As Integer = 3 To eveData.Tables(0).Columns.Count - 1
            ' Check for BPWF
            If eveData.Tables(0).Columns(col).Caption = "wasteFactor" Then
                BPWF = Math.Round(CDbl(eveData.Tables(0).Rows(0).Item(col).ToString))
                Exit For
            End If
        Next
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
    Public Shared Function LoadItems() As Boolean
        EveHQ.Core.HQ.itemList.Clear()
        EveHQ.Core.HQ.groupList.Clear()
        EveHQ.Core.HQ.catList.Clear()
        EveHQ.Core.HQ.groupCats.Clear()
        EveHQ.Core.HQ.typeGroups.Clear()
        EveHQ.Core.HQ.attributeList.Clear()
        EveHQ.Core.HQ.itemPublishedList.Clear()
        Try

            Dim iKey As String = ""
            Dim iValue As String = ""
            Dim iParent As String = ""
            Dim iPublished As Boolean = False
            Dim iBasePrice As String = ""
            ' Load categories
            eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invCategories ORDER BY categoryName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iKey = eveData.Tables(0).Rows(item).Item("categoryName").ToString.Trim
                iValue = eveData.Tables(0).Rows(item).Item("categoryID").ToString.Trim
                EveHQ.Core.HQ.catList.Add(iKey, iValue)
            Next
            ' Load groups
            eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invGroups ORDER BY groupName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iKey = eveData.Tables(0).Rows(item).Item("groupName").ToString.Trim
                iValue = eveData.Tables(0).Rows(item).Item("groupID").ToString.Trim
                iParent = eveData.Tables(0).Rows(item).Item("categoryID").ToString.Trim
                EveHQ.Core.HQ.groupList.Add(iKey, iValue)
                EveHQ.Core.HQ.groupCats.Add(iValue, iParent)
            Next
            ' Load items
            eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invTypes ORDER BY typeName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iKey = eveData.Tables(0).Rows(item).Item("typeName").ToString.Trim
                iValue = eveData.Tables(0).Rows(item).Item("typeID").ToString.Trim
                iParent = eveData.Tables(0).Rows(item).Item("groupID").ToString.Trim
                iBasePrice = eveData.Tables(0).Rows(item).Item("basePrice").ToString.Trim
                iPublished = CBool(eveData.Tables(0).Rows(item).Item("published"))
                If EveHQ.Core.HQ.itemList.Contains(iKey) = False Then
                    EveHQ.Core.HQ.itemList.Add(iKey, iValue)
                End If
                EveHQ.Core.HQ.typeGroups.Add(iValue, iParent)
                If EveHQ.Core.HQ.itemPublishedList.Contains(iKey) = False Then
                    EveHQ.Core.HQ.itemPublishedList.Add(iKey, iPublished)
                End If
                If EveHQ.Core.HQ.BasePriceList.Contains(iValue) = False Then
                    EveHQ.Core.HQ.BasePriceList.Add(iValue, iBasePrice)
                End If
            Next
            ' Load attribute names
            eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM dgmAttributeTypes ORDER BY attributeName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                iKey = eveData.Tables(0).Rows(item).Item("attributeName").ToString.Trim
                iValue = eveData.Tables(0).Rows(item).Item("attributeID").ToString.Trim
                EveHQ.Core.HQ.attributeList.Add(iKey, iValue)
            Next
            If EveHQ.Core.DataFunctions.LoadUnlocks = False Then
                Return False
                Exit Function
            End If
            EveHQ.Core.DataFunctions.LoadMarketPrices()
            EveHQ.Core.DataFunctions.LoadCustomPrices()
            Return True
        Catch e As Exception
            Return False
            Exit Function
        End Try
    End Function
    Public Shared Function LoadUnlocks() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invTypes.typeID AS invTypeID, invTypes.groupID, invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, invTypes.published"
            strSQL &= " FROM invTypes INNER JOIN dgmTypeAttributes ON invTypes.typeID = dgmTypeAttributes.typeID"
            strSQL &= " WHERE (((dgmTypeAttributes.attributeID) IN (182,183,184,277,278,279)) AND ((invTypes.published)=true))"
            strSQL &= " ORDER BY invTypes.typeID, dgmTypeAttributes.attributeID;"
            Dim attData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            Dim lastAtt As String = "0"
            Dim skillIDLevel As String = ""
            Dim atts As Double = 0
            Dim itemList As New ArrayList
            For row As Integer = 0 To attData.Tables(0).Rows.Count - 1
                If attData.Tables(0).Rows(row).Item("invTypeID").ToString <> lastAtt Then
                    Dim attRows() As DataRow = attData.Tables(0).Select("invTypeID=" & attData.Tables(0).Rows(row).Item("invtypeID").ToString)
                    atts = (attRows.GetUpperBound(0) + 1) / 2
                    If atts = Int(atts) Then
                        For attributes As Integer = 0 To CInt(atts - 1)
                            skillIDLevel = attRows(attributes).Item("valueInt").ToString & "." & attRows(CInt(attributes + atts)).Item("valueInt").ToString
                            itemList.Add(skillIDLevel & "_" & attData.Tables(0).Rows(row).Item("invtypeID").ToString & "_" & attData.Tables(0).Rows(row).Item("groupID").ToString)
                        Next
                    End If
                    lastAtt = CStr(attData.Tables(0).Rows(row).Item("invtypeID"))
                End If
            Next

            ' Place the items into the Shared arrays
            Dim items(2) As String
            Dim itemUnlocked As New ArrayList
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
                    itemUnlocked = CType(EveHQ.Core.HQ.SkillUnlocks(items(0)), ArrayList)
                    itemUnlocked.Add(items(1) & "_" & items(2))
                End If
                If EveHQ.Core.HQ.ItemUnlocks.ContainsKey(items(1)) = False Then
                    ' Create an arraylist and add the item
                    itemUnlocked = New ArrayList
                    itemUnlocked.Add(items(0))
                    EveHQ.Core.HQ.ItemUnlocks.Add(items(1), itemUnlocked)
                Else
                    ' Fetch the item and add the new one
                    itemUnlocked = CType(EveHQ.Core.HQ.ItemUnlocks(items(1)), ArrayList)
                    itemUnlocked.Add(items(0))
                End If
            Next
            Return True
        Catch e As Exception
            Return False
            Exit Function
        End Try
    End Function
    Public Shared Function LoadMarketPrices() As Boolean
        Try
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            EveHQ.Core.HQ.MarketPriceList.Clear()
            If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\MarketPrices.txt") = True Then
                Dim sr As New IO.StreamReader(EveHQ.Core.HQ.cacheFolder & "\MarketPrices.txt")
                Dim marketLine As String = ""
                Dim price As Double
                Do
                    marketLine = sr.ReadLine()
                    Dim marketData() As String = marketLine.Split(",".ToCharArray)
                    price = Double.Parse(CStr(marketData(1)), Globalization.NumberStyles.Number, culture)
                    EveHQ.Core.HQ.MarketPriceList.Add(marketData(0), price)
                Loop Until sr.EndOfStream
            End If
        Catch ex As Exception
        End Try
        Return True
    End Function
    Public Shared Function LoadCustomPrices() As Boolean
        Try
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            EveHQ.Core.HQ.CustomPriceList.Clear()
            If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\CustomPrices.txt") = True Then
                Dim sr As New IO.StreamReader(EveHQ.Core.HQ.cacheFolder & "\CustomPrices.txt")
                Dim marketLine As String = ""
                Dim price As Double
                Do
                    marketLine = sr.ReadLine()
                    Dim marketData() As String = marketLine.Split(",".ToCharArray)
                    price = Double.Parse(CStr(marketData(1)), Globalization.NumberStyles.Number, culture)
                    EveHQ.Core.HQ.CustomPriceList.Add(marketData(0), price)
                Loop Until sr.EndOfStream
            End If
        Catch ex As Exception
        End Try
        Return True
    End Function
    Public Shared Function GetPrice(ByVal itemID As String) As Double
        Try
            If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) = True Then
                Return CDbl(EveHQ.Core.HQ.CustomPriceList(itemID))
            Else
                If EveHQ.Core.HQ.MarketPriceList.ContainsKey(itemID) Then
                    Return CDbl(EveHQ.Core.HQ.MarketPriceList(itemID))
                Else
                    Return CDbl(EveHQ.Core.HQ.BasePriceList(itemID))
                End If
            End If
        Catch e As Exception
            Return CDbl(EveHQ.Core.HQ.BasePriceList(itemID))
        End Try
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


End Class
