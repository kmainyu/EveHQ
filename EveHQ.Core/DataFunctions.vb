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
Imports EveHQ.EveAPI
Imports EveHQ.Market
Imports EveHQ.Common.Extensions
Imports System.Threading.Tasks
Imports EveHQ.EveData


Public Class DataFunctions
    Shared customSQLCEConnection As New SqlCeConnection
    Shared customSQLConnection As New SqlConnection
    Shared IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Shared SQLTimeFormat As String = "yyyyMMdd HH:mm:ss"
    Shared culture As CultureInfo = New CultureInfo("en-GB")
    Shared LastCacheRefresh As String = "2.12.0.0"

    Public Shared Function CreateEveHQDataDB() As Boolean
        Select Case HQ.Settings.DBFormat
            Case 0 ' SQL CE
                ' Get the directory of the existing SQL CE database to write the new one there
                Dim outputFile As String = ""
                outputFile = HQ.Settings.DBDataFilename.Replace("\\", "\")
                'MessageBox.Show("Creating database using path: " & outputFile, "Custom Database Location", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Try to create a new SQL CE DB
                Dim strConnection As String = "Data Source = " & ControlChars.Quote & outputFile & ControlChars.Quote &
                                              ";" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Try
                    Dim SQLCE As New SqlCeEngine(strConnection)
                    SQLCE.CreateDatabase()
                    HQ.Settings.DBDataFilename = outputFile
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
                HQ.EveHQDataConnectionString = "Server=" & HQ.Settings.DBServer
                If HQ.Settings.DBSQLSecurity = True Then
                    HQ.EveHQDataConnectionString += "; User ID=" & HQ.Settings.DBUsername & "; Password=" &
                                                    HQ.Settings.DBPassword & ";"
                Else
                    HQ.EveHQDataConnectionString += "; Integrated Security = SSPI;"
                End If
                If SetData(strSQL) <> -2 Then
                    HQ.Settings.DBDataName = "EveHQData"
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
        Select Case HQ.Settings.DBFormat
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
        Select Case HQ.Settings.DBFormat
            Case 0 ' SQL CE
                If HQ.Settings.UseAppDirectoryForDB = False Then
                    HQ.itemDBConnectionString = "Data Source = " & ControlChars.Quote & HQ.Settings.DBFilename &
                                                ControlChars.Quote & ";" &
                                                "Max Database Size = 512; ; Max Buffer Size = 2048;"
                Else
                    Try
                        Dim FI As New FileInfo(HQ.Settings.DBFilename)
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
                HQ.itemDBConnectionString = "Server=" & HQ.Settings.DBServer
                If HQ.Settings.DBSQLSecurity = True Then
                    HQ.itemDBConnectionString += "; Database = " & HQ.Settings.DBName.ToLower & "; User ID=" &
                                                 HQ.Settings.DBUsername & "; Password=" &
                                                 HQ.Settings.DBPassword & ";"
                Else
                    HQ.itemDBConnectionString += "; Database = " & HQ.Settings.DBName.ToLower &
                                                 "; Integrated Security = SSPI;"
                End If
        End Select
        Return True
    End Function

    Public Shared Function SetEveHQDataConnectionString() As Boolean
        Select Case HQ.Settings.DBFormat
            Case 0 ' SQL CE
                If HQ.Settings.UseAppDirectoryForDB = False Then
                    HQ.EveHQDataConnectionString = "Data Source = " & ControlChars.Quote &
                                                   HQ.Settings.DBDataFilename & ControlChars.Quote & ";" &
                                                   "Max Database Size = 512; Max Buffer Size = 2048;"
                Else
                    Try
                        Dim FI As New FileInfo(HQ.Settings.DBDataFilename)
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
                HQ.EveHQDataConnectionString = "Server=" & HQ.Settings.DBServer
                If HQ.Settings.DBSQLSecurity = True Then
                    HQ.EveHQDataConnectionString += "; Database = " & HQ.Settings.DBDataName.ToLower & "; User ID=" &
                                                    HQ.Settings.DBUsername & "; Password=" &
                                                    HQ.Settings.DBPassword & ";"
                Else
                    HQ.EveHQDataConnectionString += "; Database = " & HQ.Settings.DBDataName.ToLower &
                                                    "; Integrated Security = SSPI;"
                End If
        End Select
        Return True
    End Function

    Public Shared Function OpenCustomDatabase() As Boolean
        Select Case HQ.Settings.DBFormat
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
        Select Case HQ.Settings.DBFormat
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
        Select Case HQ.Settings.DBFormat
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
                    keyCommand.CommandTimeout = HQ.Settings.DBTimeout
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
        Select Case HQ.Settings.DBFormat
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
                    keyCommand.CommandTimeout = HQ.Settings.DBTimeout
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
        Select Case HQ.Settings.DBFormat
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
                    keyCommand.CommandTimeout = HQ.Settings.DBTimeout
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

        Select Case HQ.Settings.DBFormat
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
                    msg.AppendLine("Database1: " & HQ.Settings.DBFilename)
                    msg.AppendLine("Database2: " & HQ.Settings.DBDataFilename)
                    msg.AppendLine("Using App: " & HQ.Settings.UseAppDirectoryForDB.ToString)
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
                    da.SelectCommand.CommandTimeout = HQ.Settings.DBTimeout
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

        Select Case HQ.Settings.DBFormat
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
                    da.SelectCommand.CommandTimeout = HQ.Settings.DBTimeout
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

    Public Shared Function GetPrice(ByVal itemID As Integer) As Double
        Return GetPrice(itemID, MarketMetric.Default, MarketTransactionKind.Default)
    End Function
    Public Shared Function GetPrice(ByVal itemID As Integer, ByVal metric As MarketMetric, ByVal transType As MarketTransactionKind) As Double
        Dim task As Task(Of Dictionary(Of Integer, Double)) = GetMarketPrices(New Integer() {itemID}, metric, transType)
        task.Wait()
        Return task.Result.Where(Function(pair) pair.Key = itemID).Select(Function(pair) pair.Value).FirstOrDefault()
    End Function


    Public Shared Function GetPriceAsync(ByVal itemID As Integer) As Task(Of Double)
        Return GetPriceAsync(itemID, MarketMetric.Default, MarketTransactionKind.Default)
    End Function

    Public Shared Function GetPriceAsync(ByVal itemID As Integer, ByVal metric As MarketMetric, ByVal transType As MarketTransactionKind) As Task(Of Double)
        Dim task As Task(Of Dictionary(Of Integer, Double)) = GetMarketPrices(New Integer() {itemID}, metric, transType)

        Dim task2 As Task(Of Double) = task.ContinueWith(Function(priceTask As Task(Of Dictionary(Of Integer, Double))) As Double
                                                             If priceTask.IsCompleted And priceTask.IsFaulted = False Then
                                                                 Return priceTask.Result(itemID)
                                                             End If
                                                             Return 0

                                                         End Function)

        Return task2
    End Function

    Public Shared Function GetMarketPrices(ByVal itemIDs As IEnumerable(Of Integer)) As Task(Of Dictionary(Of Integer, Double))
        Return GetMarketPrices(itemIDs, MarketMetric.Default, MarketTransactionKind.Default)
    End Function

    Public Shared Function GetMarketPrices(ByVal itemIDs As IEnumerable(Of Integer), ByVal metric As MarketMetric, ByVal transType As MarketTransactionKind) As Task(Of Dictionary(Of Integer, Double))
        If metric = MarketMetric.Default Then
            metric = HQ.Settings.MarketDefaultMetric
        End If
        If transType = MarketTransactionKind.Default Then
            transType = HQ.Settings.MarketDefaultTransactionType
        End If

        Dim dataTask As Task(Of IEnumerable(Of ItemOrderStats))
        Dim resultTask As Task(Of Dictionary(Of Integer, Double))

        If itemIDs IsNot Nothing Then
            If itemIDs.Any() Then
                ' Go through the list of id's provided and only get the items that have a valid market group.
                Dim filteredIdNumbers As IEnumerable(Of Integer) = (From itemId In itemIDs Where StaticData.Types.ContainsKey(itemId))

                Dim itemIdNumbersToRequest As IEnumerable(Of Integer) = (From itemId In filteredIdNumbers Where StaticData.Types(itemId).MarketGroupId <> 0 Select itemId)

                If itemIdNumbersToRequest Is Nothing Then
                    itemIdNumbersToRequest = New List(Of Integer)
                End If

                If (itemIdNumbersToRequest.Any()) Then
                    'Fetch all the item prices in a single request
                    If HQ.Settings.MarketUseRegionMarket Then
                        dataTask = HQ.MarketStatDataProvider.GetOrderStats(itemIdNumbersToRequest, HQ.Settings.MarketRegions, Nothing, 1)
                    Else
                        dataTask = HQ.MarketStatDataProvider.GetOrderStats(itemIdNumbersToRequest, Nothing, HQ.Settings.MarketSystem, 1)
                    End If

                    ' Still need to do this in a synchronous fashion...unfortunately
                    resultTask = dataTask.ContinueWith(Function(markettask As Task(Of IEnumerable(Of ItemOrderStats))) As Dictionary(Of Integer, Double)


                                                           Return ProcessPriceTaskData(markettask, itemIDs, metric, transType)


                                                       End Function)
                Else
                    resultTask = Task(Of Dictionary(Of Integer, Double)).Factory.TryRun(Function() As Dictionary(Of Integer, Double)
                                                                                            'Empty Result
                                                                                            Return itemIDs.ToDictionary(Of Integer, Double)(Function(id) id, Function(id) 0)
                                                                                        End Function)
                End If
            Else
                resultTask = Task(Of Dictionary(Of Integer, Double)).Factory.TryRun(Function() As Dictionary(Of Integer, Double)
                                                                                        'Empty Result
                                                                                        Return itemIDs.ToDictionary(Of Integer, Double)(Function(id) id, Function(id) 0)
                                                                                    End Function)

            End If
        Else
            resultTask = Task(Of Dictionary(Of Integer, Double)).Factory.TryRun(Function() As Dictionary(Of Integer, Double)
                                                                                    'Empty Result
                                                                                    Return itemIDs.ToDictionary(Of Integer, Double)(Function(id) id, Function(id) 0)
                                                                                End Function)
        End If

        Return resultTask
    End Function

    Private Shared Function ProcessPriceTaskData(markettask As Task(Of IEnumerable(Of ItemOrderStats)), itemIDs As IEnumerable(Of Integer), ByVal metric As MarketMetric, ByVal transType As MarketTransactionKind) As Dictionary(Of Integer, Double)

        ' TODO: Web exceptions and otheres can be thrown here... need to protect upstream code.

        ' TODO: ItemIds are integers but through out the existing code they are inconsistently treated as strings (or longs...)... must fix that.
        Dim itemPrices As Dictionary(Of Integer, Double)

        Dim distinctItems As IEnumerable(Of Integer) = itemIDs.Distinct()

        ' Initialize all items to have a default price of 0 (provides a safe default for items being requested that do not have a valid marketgroup)
        itemPrices = distinctItems.ToDictionary(Of Integer, Double)(Function(item) item, Function(item) 0)

        If markettask.Exception Is Nothing Then

            Try
                Dim result As IEnumerable(Of ItemOrderStats) = Nothing
                Dim itemResult As ItemOrderStats = Nothing
                If markettask.IsCompleted And markettask.IsFaulted = False And markettask.Result IsNot Nothing Then
                    If markettask.Result.Any() Then
                        result = markettask.Result
                    End If
                End If

                Dim testItem As Integer
                For Each itemId As Integer In distinctItems 'We only need to process the unique id results.
                    Try
                        testItem = itemId
                        If result IsNot Nothing Then
                            itemResult = (From item In result Where item.ItemTypeId = testItem Select item).FirstOrDefault()
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
                            If (HQ.Settings.MarketStatOverrides.TryGetValue(itemResult.ItemTypeId, override)) Then
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
                            If StaticData.Types.ContainsKey(itemId) Then
                                itemPrices(itemId) = StaticData.Types(itemId).BasePrice
                            Else
                                itemPrices(itemId) = 0
                            End If
                        End If
                    Catch e As Exception
                        If StaticData.Types.ContainsKey(itemId) Then
                            itemPrices(itemId) = StaticData.Types(itemId).BasePrice
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

    Public Shared Function LoadCustomPricesFromDB() As Boolean
        Dim eveData As New DataSet
        Try
            eveData = GetCustomData("SELECT * FROM customPrices ORDER BY typeID;")
            If eveData IsNot Nothing Then
                HQ.CustomPriceList.Clear()
                For Each priceRow As DataRow In eveData.Tables(0).Rows
                    HQ.CustomPriceList.Add(CInt(priceRow.Item("typeID")), CDbl(priceRow.Item("price")))
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

    Public Shared Function SetCustomPrice(ByVal typeID As Integer, ByVal userPrice As Double, ByVal dbOpen As Boolean) _
        As Boolean
        ' Store the user's price in the database
        If HQ.CustomPriceList.ContainsKey(typeID) = False Then
            ' Add the data
            If AddCustomPrice(typeID, userPrice, DBOpen) = True Then
                Return True
            Else
                Return False
            End If
        Else
            ' Edit the data
            If EditCustomPrice(typeID, userPrice, DBOpen) = True Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Shared Function AddCustomPrice(ByVal itemID As Integer, ByVal price As Double, ByVal dbOpen As Boolean) _
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

    Public Shared Function EditCustomPrice(ByVal itemID As Integer, ByVal price As Double, ByVal dbOpen As Boolean) _
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

    Public Shared Function DeleteCustomPrice(ByVal itemID As Integer) As Boolean
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
                    HQ.Settings.IgnoreSellOrders = True And
                    oPrice > (HQ.Settings.IgnoreSellOrderLimit * HQ.itemData(oTypeID.ToString).BasePrice) Then
                    ProcessOrder = False
                End If
            Else ' Buy Order
                If HQ.Settings.IgnoreBuyOrders = True And oPrice < HQ.Settings.IgnoreBuyOrderLimit Then
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
                            New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension,
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

    Public Shared Function WriteMailingListIDsToDatabase(ByVal mPilot As EveHQPilot) As SortedList(Of Long, String)
        Dim FinalIDs As New SortedList(Of Long, String)
        Dim accountName As String = mPilot.Account
        If accountName <> "" Then
            Dim mAccount As EveHQAccount = HQ.Settings.Accounts.Item(accountName)
            ' Send this to the API
            Dim _
                APIReq As _
                    New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension,
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
                    Dim cSystem As SolarSystem
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
                New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension,
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

End Class

Public Enum DatabaseFormat
    SQLCE = 0
    SQL = 1
End Enum
