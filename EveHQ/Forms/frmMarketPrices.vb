Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports System.IO.Compression
Imports DotNetLib.Windows.Forms

Public Class frmMarketPrices
    Dim saveLoc As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\MarketXMLs"
    Dim insertStat As String = "INSERT INTO marketStats (statDate, typeID, regionID, systemID, volAll, avgAll, maxAll, minAll, stdAll, medAll, volBuy, avgBuy, maxBuy, minBuy, stdBuy, medBuy, volSell, avgSell, maxSell, minSell, stdSell, medSell) "
    Dim regions As New SortedList
    Dim strSQL As String = ""
    Dim orderDetails() As String
    Dim oReg, oSys, oStation As Long
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
    Dim currentProgress As Double = 0
    Dim currentProgressScroll As Boolean = True
    Dim ProcessOrder As Boolean = True
    Dim marketCacheFolder As String = ""
    Dim ECDumpURL As String = "http://eve-central.com/dumps/"
    Dim startUp As Boolean = True

    Private Function CheckMarketStatsDBTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("marketStats") = False Then
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
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE marketStats")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  statID         int IDENTITY(1,1),")
            strSQL.AppendLine("  statDate       datetime,")
            strSQL.AppendLine("  typeID         int,")
            strSQL.AppendLine("  regionID       int,")
            strSQL.AppendLine("  systemID       int,")
            strSQL.AppendLine("  volAll         float,")
            strSQL.AppendLine("  avgAll         float,")
            strSQL.AppendLine("  maxAll         float,")
            strSQL.AppendLine("  minAll         float,")
            strSQL.AppendLine("  stdAll         float,")
            strSQL.AppendLine("  medAll         float,")
            strSQL.AppendLine("  volBuy         float,")
            strSQL.AppendLine("  avgBuy         float,")
            strSQL.AppendLine("  maxBuy         float,")
            strSQL.AppendLine("  minBuy         float,")
            strSQL.AppendLine("  stdBuy         float,")
            strSQL.AppendLine("  medBuy         float,")
            strSQL.AppendLine("  volSell         float,")
            strSQL.AppendLine("  avgSell         float,")
            strSQL.AppendLine("  maxSell         float,")
            strSQL.AppendLine("  minSell         float,")
            strSQL.AppendLine("  stdSell         float,")
            strSQL.AppendLine("  medSell         float,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT marketStats_PK PRIMARY KEY CLUSTERED (statID)")
            strSQL.AppendLine(")")
            If EveHQ.Core.HQ.EveHQSettings.DBFormat > 0 Then
                strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_type ON marketStats (typeID)")
                strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_region ON marketStats (regionID)")
                strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_system ON marketStats (systemID)")
            End If
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
                Return True
            Else
                MessageBox.Show("There was an error creating the marketStats database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

    End Function

    Private Function CheckMarketDatesDBTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("marketDates") = False Then
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
            strSQL.AppendLine("CREATE TABLE marketDates")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  dateID         int IDENTITY(1,1),")
            strSQL.AppendLine("  priceDate      datetime,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT marketDates_PK PRIMARY KEY CLUSTERED (dateID)")
            strSQL.AppendLine(")")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
                Return True
            Else
                MessageBox.Show("There was an error creating the marketDates database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Market Dates Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        ofd1.ShowDialog()
        Call Me.ProcessECPrices(ofd1.FileName)

    End Sub

    Private Sub CalculateECStats(ByVal orderList As ArrayList, ByVal CalcType As Integer, ByVal orderDate As Date)

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
            oReg = CLng(orderDetails(1).Trim)
            oSys = CLng(orderDetails(2).Trim)
            oStation = CLng(orderDetails(3).Trim)
            oTypeID = CLng(orderDetails(4).Trim)
            oType = CLng(orderDetails(5).Trim)
            oMinVol = CLng(orderDetails(7).Trim)
            oVol = CLng(orderDetails(8).Trim)
            oPrice = CDbl(orderDetails(6).Trim)

            If CalcType = 0 Then
                ' Add this to a regional order processing list if required
                If regions.Contains(oReg.ToString) = False Then
                    regOrders = New ArrayList
                    regOrders.Add(order)
                    regions.Add(oReg.ToString, regOrders)
                Else
                    regOrders = CType(regions(oReg.ToString), ArrayList)
                    regOrders.Add(order)
                End If
            End If
            'If CalcType = 1 Then
            '    ' Add this to a system order processing list if required
            '    If systems.Contains(oSys.ToString) = False Then
            '        sysOrders = New ArrayList
            '        sysOrders.Add(order)
            '        systems.Add(oSys.ToString, sysOrders)
            '    Else
            '        sysOrders = CType(systems(oSys.ToString), ArrayList)
            '        sysOrders.Add(order)
            '    End If
            'End If

            ' Check if we process this
            ProcessOrder = True
            If oType = 0 Then ' Sell Order
                If chkIgnoreSellOrders.Checked = True And oPrice > nudIgnoreSellOrderLimit.Value Then
                    ProcessOrder = False
                End If
            Else ' Buy Order
                If chkIgnoreBuyOrders.Checked = True And oPrice < nudIgnoreBuyOrderLimit.Value Then
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

        Select Case CalcType
            Case 0
                oReg = 0
                oSys = 0
            Case 1
                oSys = 0
        End Select

        ' Write data to the database
        strSQL = insertStat & "VALUES ('" & Format(orderDate, "yyyy-MM-dd HH:mm:ss") & "', " & oTypeID.ToString & ", " & oReg.ToString & ", " & oSys.ToString & ", " & volAll & ", " & avgAll & ", " & maxAll & ", " & minAll & ", " & stdAll & ", " & medAll & ", " & volBuy & ", " & avgBuy & ", " & maxBuy & ", " & minBuy & ", " & stdBuy & ", " & medBuy & ", " & volSell & ", " & avgSell & ", " & maxSell & ", " & minSell & ", " & stdSell & ", " & medSell & ");"
        If EveHQ.Core.DataFunctions.SetData(strSQL) = False Then
            MessageBox.Show("There was an error writing data to the marketStats database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL, "Error Writing Market Stats", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        ' Calculate regional stuff if required
        If CalcType = 0 Then
            For Each Region As String In regions.Keys
                Call CalculateECStats(CType(regions(Region), ArrayList), 1, orderDate)
            Next
        End If
        'If CalcType = 1 Then
        '    For Each System As String In systems.Keys
        '        Call CalculateStats(CType(systems(System), ArrayList), 2)
        '    Next
        'End If

    End Sub

    Private Function GetLastMarketDate() As Date
        Dim strSQL As String = "SELECT max(priceDate) AS lastDate FROM marketDates"
        Dim dateData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        If dateData IsNot Nothing Then
            If dateData.Tables(0).Rows.Count > 0 Then
                If IsDBNull(dateData.Tables(0).Rows(0).Item("lastDate")) = False Then
                    Return CDate(dateData.Tables(0).Rows(0).Item("lastDate"))
                Else
                    Return Date.FromOADate(0)
                End If
            Else
                Return Date.FromOADate(0)
            End If
            Return Date.FromOADate(0)
        End If
    End Function

    Private Sub frmMarketPrices_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Remove the EC Dumps tab
        tabMarketPrices.TabPages.Remove(tabDumps)

        startUp = True

        ' Check for the market cache folder
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.appDataFolder & "\MarketCache") = False Then
            Try
                My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.appDataFolder & "\MarketCache")
                marketCacheFolder = EveHQ.Core.HQ.appDataFolder & "\MarketCache\"
            Catch ex As Exception
                MessageBox.Show("An error occured while attempting to create the Market Cache folder: " & ex.Message, "Error Creating Market Cache Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End Try
        Else
            marketCacheFolder = EveHQ.Core.HQ.appDataFolder & "\MarketCache\"
        End If

        ' Check we have the EveHQDB Database and MarketStats table available
        If Me.CheckMarketStatsDBTable = False Then
            MessageBox.Show("EveHQ cannot proceed with the market price processing until the EveHQData storage database is available", "Error In Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check we have the EveHQDB Database and MarketDates table available
        If Me.CheckMarketDatesDBTable = False Then
            MessageBox.Show("EveHQ cannot proceed with the market price processing until the EveHQData storage database is available", "Error In Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Get Regions
        Dim regionSet As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM mapRegions ORDER BY regionName;")
        If regionSet IsNot Nothing Then
            Dim x, y As Integer : x = 10 : y = 20
            regions.Clear()
            For Each regionRow As DataRow In regionSet.Tables(0).Rows
                ' Create a batch of checkboxes for the regions
                Dim chk As New CheckBox
                chk.Text = CStr(regionRow.Item("regionName"))
                chk.Name = CStr(regionRow.Item("regionID"))
                regions.Add(CStr(regionRow.Item("regionName")), CStr(regionRow.Item("regionID")))
                AddHandler chk.CheckedChanged, AddressOf Me.MarketRegionChanged
                If IsDBNull(regionRow.Item("factionID")) Then
                    chk.Tag = 0
                Else
                    If CInt(regionRow.Item("factionID")) > 500008 Then
                        chk.Tag = 0
                    Else
                        chk.Tag = CInt(regionRow.Item("factionID"))
                    End If
                End If
                If CInt(chk.Tag) <> 500005 Then ' Ignore the Jove systems
                    chk.Location = New Point(x, y) : chk.Width = 145 : chk.Height = 18
                    grpRegions.Controls.Add(chk)
                    x += 150
                    If x > 600 Then
                        x = 10 : y += 18
                    End If
                End If
            Next
        Else
            MessageBox.Show("EveHQ cannot proceed with the market price processing until the Map Regions have been correctly loaded.", "Error Loading Map Regions", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        ' Get the contents of the market log folder
        Call Me.ImportLogDetails()

        ' Update Market Price Settings
        Call Me.UpdatePriceSettings()

        ' Update the Custom Price Grid
        Call Me.UpdatePriceMatrix()

        ' Set the faction price snapshot data
        lblLastFactionPriceUpdate.Text = Format(EveHQ.Core.HQ.EveHQSettings.LastFactionPriceUpdate, "dd/MM/yyyy HH:mm:ss")
        If EveHQ.Core.HQ.EveHQSettings.LastFactionPriceUpdate.AddSeconds(86400) < Now Then
            btnUpdateFactionPrices.Enabled = True
            lblFactionPriceUpdateStatus.Text = "Status: Awaiting update."
        Else
            btnUpdateFactionPrices.Enabled = False
            lblFactionPriceUpdateStatus.Text = "Status: Inactive due to 24 hour feed restriction."
        End If

        ' Set the market price snapshot data
        lblLastMarketPriceUpdate.Text = Format(EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate, "dd/MM/yyyy HH:mm:ss")
        If EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate.AddSeconds(86400) < Now Then
            btnUpdateMarketPrices.Enabled = True
            lblMarketPriceUpdateStatus.Text = "Status: Awaiting update."
        Else
            btnUpdateMarketPrices.Enabled = False
            lblMarketPriceUpdateStatus.Text = "Status: Inactive due to 24 hour feed restriction."
        End If

        startUp = False

        ' Start the initial timing check (but do last so form is drawn)
        tmrStart.Enabled = True

    End Sub

    Private Sub LoadRegions()
        Dim regionSet As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM mapRegions ORDER BY regionName;")
        If regionSet IsNot Nothing Then
            regions.Clear()
            For Each regionRow As DataRow In regionSet.Tables(0).Rows
                regions.Add(CStr(regionRow.Item("regionName")), CStr(regionRow.Item("regionID")))
            Next
        Else
            MessageBox.Show("EveHQ cannot proceed with the market price processing until the Map Regions have been correctly loaded.", "Error Loading Map Regions", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Public Sub ImportLogDetails()
        ' Get the contents of the market log folder and display them
        clvLogs.BeginUpdate()
        clvLogs.Items.Clear()
        If My.Computer.FileSystem.DirectoryExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Eve\Logs\Marketlogs") = True Then
            For Each file As String In My.Computer.FileSystem.GetFiles(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Eve\Logs\Marketlogs", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
                Call Me.DisplayLogDetails(file)
                clvLogs.Enabled = True
            Next
        Else
            clvLogs.Items.Add("Log Directory Not Available")
            clvLogs.Enabled = False
        End If
        clvLogs.EndUpdate()
    End Sub

    Public Sub DisplayLogDetails(ByVal file As String)
        ' Split the filename into sections denoted by "-"
        Dim LogItem As New ContainerListViewItem
        Dim FI As FileInfo
        Dim info() As String
        Dim idx As Integer = 0
        Dim region As String = ""
        Dim item As String = ""
        Dim TimeFormat As String = "yyyy.MM.dd HHmmss"
        Dim logDate As Date = Now
        FI = New FileInfo(file)
        info = FI.Name.TrimEnd(".txt".ToCharArray).Split("-".ToCharArray)
        region = "" : item = ""
        idx = -1
        Do
            idx += 1
            region &= info(idx).Trim & "-"
            ' Repeat until we have a valid
        Loop Until regions.ContainsKey(region.TrimEnd("-".ToCharArray))
        region = region.TrimEnd("-".ToCharArray)
        For idx = idx + 1 To info.Length - 2
            item &= info(idx) & "-"
        Next
        item = item.TrimEnd("-".ToCharArray).Trim
        logDate = DateTime.ParseExact(info(info.Length - 1).Trim, TimeFormat, Nothing, Globalization.DateTimeStyles.None)
        LogItem = New ContainerListViewItem
        LogItem.Tag = file
        LogItem.Text = region
        clvLogs.Items.Add(LogItem)
        LogItem.SubItems(1).Text = item
        LogItem.SubItems(2).Text = FormatDateTime(logDate, DateFormat.GeneralDate)
    End Sub

    Public Sub ResortLogs()
        clvLogs.Sort(False)
    End Sub

    Private Sub GetFirstPrices(ByVal dataObject As Object)
        Dim tryDate As Date = Now
        Dim fileName As String = Format(tryDate, "yyyy-MM-dd") & ".dump.gz"
        Dim DLsuccess As Boolean = False
        Dim tryCount As Integer = 0
        Do
            fileName = Format(tryDate, "yyyy-MM-dd") & ".dump.gz"
            lblProcess.Text = "Current Process: Attempting download of " & ECDumpURL & fileName
            lblProcess.Refresh()
            DLsuccess = Me.DownloadFile(fileName, True)
            tryDate = tryDate.AddDays(-1)
            tryCount += 1
            If tryCount = 5 And DLsuccess = False Then
                MessageBox.Show("Unable to find any recent dump files. Please check your system date and connection.", "Cannot Locate Dump Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Loop Until DLsuccess = True
        If Me.DecompressFile(marketCacheFolder & fileName, marketCacheFolder & fileName & ".txt") = True Then
            Call Me.ProcessECPrices(marketCacheFolder & fileName & ".txt")
        End If
        ' Set the market prices to the figures just downloaded

    End Sub

    Private Sub ProcessECPrices(ByVal orderFile As String)
        Dim startTime, endTime As Date
        Dim timeTaken As TimeSpan
        Dim orderFI As New FileInfo(orderFile)
        Dim orderDate As Date = Date.Parse(orderFI.Name.Trim(".dump.gz.txt".ToCharArray))
        Dim sr As New StreamReader(orderFile)
        Dim header As String = sr.ReadLine()
        Dim items As New SortedList
        Dim itemOrders As New ArrayList

        If header <> "orderid, regionid, systemid, stationid, typeid, bid, price, minvolume, volremain, volenter, issued, duration, range, reportedby, reportedtime" Then
            MessageBox.Show("File is not a valid Eve-Central dump file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            lblProcess.Text = "Current Process: Reading order list from file..."
            lblProgress.Text = "Progress: Reading orders..."
            currentProgressScroll = True : Me.Invoke(New MethodInvoker(AddressOf Me.UpdateProgressBar))
            startTime = Now
            Dim order As String = ""
            Dim orders As Long = 0
            Dim orderDetails() As String
            Do
                order = sr.ReadLine
                orderDetails = order.Split(",".ToCharArray)
                ' Add to the relevant item list
                If items.Contains(orderDetails(4).Trim) = False Then
                    itemOrders = New ArrayList
                    itemOrders.Add(order)
                    items.Add(orderDetails(4).Trim, itemOrders)
                Else
                    itemOrders = CType(items(orderDetails(4).Trim), ArrayList)
                    itemOrders.Add(order)
                End If
                orders += 1
            Loop Until sr.EndOfStream
            sr.Close()

            endTime = Now
            timeTaken = endTime - startTime
            'MessageBox.Show("Time taken to read " & orders.ToString & " orders = " & timeTaken.TotalSeconds.ToString)

            ' Calculate global statistics
            lblProcess.Text = "Current Process: Calculating Order Statistics for " & FormatDateTime(orderDate, DateFormat.LongDate) & "..."
            currentProgressScroll = False : Me.Invoke(New MethodInvoker(AddressOf Me.UpdateProgressBar))
            pbProgress.Style = ProgressBarStyle.Continuous
            startTime = Now
            Dim itemCount As Integer = items.Count
            Dim count As Integer = 0
            For Each item As String In items.Keys
                count += 1
                Call CalculateECStats(CType(items(item), ArrayList), 0, orderDate)
                lblProgress.Text = "Progress: Processing " & EveHQ.Core.HQ.itemList.GetKey(EveHQ.Core.HQ.itemList.IndexOfValue(item)).ToString & "..."
                currentProgress = count / itemCount * 100
                Me.Invoke(New MethodInvoker(AddressOf Me.UpdateProgressBar))
            Next

            ' Insert the date into the database
            lblProcess.Text = "Current Process: Updating price date table..."
            Dim dateSQL As String = "INSERT INTO marketDates (priceDate) VALUES ('" & Format(orderDate, "yyyy-MM-dd HH:mm:ss") & "');"
            If EveHQ.Core.DataFunctions.SetData(dateSQL) = False Then
                MessageBox.Show("There was an error writing the date to the marketDates database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL, "Error Writing Price Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            endTime = Now
            timeTaken = endTime - startTime
            lblProcess.Text = "Current Process: Processing Complete!"
            lblProgress.Text = "Progress: Finished Processing " & items.Count.ToString & " items in " & timeTaken.TotalSeconds.ToString & "s"
            items.Clear() : items = Nothing
            itemOrders.Clear() : itemOrders = Nothing
            GC.Collect()
            'MessageBox.Show("Time taken to parse data for " & items.Count.ToString & " items = " & timeTaken.TotalSeconds.ToString, "Price Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub SetMarketPrices()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim lastDate As Date = GetLastMarketDate()

        If lastDate.ToOADate = 0 Then
            System.Threading.ThreadPool.QueueUserWorkItem(AddressOf GetFirstPrices, Nothing)
            'Call Me.GetFirstPrices()
        Else
            MessageBox.Show("Last date is " & FormatDateTime(lastDate, DateFormat.LongDate))
        End If
    End Sub

    Private Function DownloadFile(ByVal FileNeeded As String, ByVal SilentError As Boolean) As Boolean

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        'Dim count As Integer = 0
        'count += 1
        Dim httpURI As String = ECDumpURL & FileNeeded
        Dim localFile As String = marketCacheFolder & FileNeeded

        ' Create the request to access the server and set credentials
        ServicePointManager.DefaultConnectionLimit = 10
        ServicePointManager.Expect100Continue = False
        Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(httpURI))
        Dim request As HttpWebRequest = CType(HttpWebRequest.Create(httpURI), HttpWebRequest)
        request.CachePolicy = policy
        ' Setup proxy server (if required)
        If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
            Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
            If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                EveHQProxy.UseDefaultCredentials = True
            Else
                EveHQProxy.UseDefaultCredentials = False
                EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
            End If
            request.Proxy = EveHQProxy
        End If
        request.CachePolicy = policy
        request.Method = WebRequestMethods.File.DownloadFile
        request.Timeout = 900000
        Try
            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
                Dim filesize As Long = CLng(response.ContentLength)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(localFile, IO.FileMode.Create)
                        Dim buffer(4095) As Byte
                        Dim read As Integer = 0
                        Dim totalBytes As Long = 0
                        Dim percent As Integer = 0
                        Dim startTime As Date
                        Dim timeTaken As TimeSpan
                        lblProcess.Text = "Current Process: Downloading file: " & httpURI
                        currentProgressScroll = False : Me.Invoke(New MethodInvoker(AddressOf Me.UpdateProgressBar))
                        startTime = Now
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                            totalBytes += read
                            percent = CInt(totalBytes / filesize * 100)
                            timeTaken = Now - startTime
                            lblProgress.Text = "Progress: " & FormatNumber(totalBytes, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " bytes (" & FormatNumber(totalBytes / 1024 / timeTaken.TotalSeconds, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " kb/s) in " & timeTaken.TotalSeconds.ToString & "s"
                            currentProgress = totalBytes / filesize * 100
                            Me.Invoke(New MethodInvoker(AddressOf Me.UpdateProgressBar))
                            'worker.ReportProgress(percent, FileNeeded)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
            Return True
        Catch e As WebException
            If SilentError = False Then
                Dim errMsg As String = "An error has occurred during download of " & httpURI & ":" & ControlChars.CrLf
                errMsg &= "Status: " & e.Status & ControlChars.CrLf
                errMsg &= "Message: " & e.Message & ControlChars.CrLf
                MessageBox.Show(errMsg, "Error Downloading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'worker.CancelAsync()
            End If
            Return False
        End Try
    End Function

    Public Sub CompressFile(ByVal sourceFile As String, ByVal destinationFile As String)

        ' make sure the source file is there
        If File.Exists(sourceFile) = False Then
            Throw New FileNotFoundException
        End If

        ' Create the streams and byte arrays needed
        Dim buffer As Byte() = Nothing
        Dim sourceStream As FileStream = Nothing
        Dim destinationStream As FileStream = Nothing
        Dim compressedStream As GZipStream = Nothing

        Try
            ' Read the bytes from the source file into a byte array
            sourceStream = New FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read)

            ' Read the source stream values into the buffer
            buffer = New Byte(CInt(sourceStream.Length)) {}
            Dim checkCounter As Integer = sourceStream.Read(buffer, 0, buffer.Length)

            ' Open the FileStream to write to
            destinationStream = New FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write)
            ' Create a compression stream pointing to the destiantion stream
            compressedStream = New GZipStream(destinationStream, CompressionMode.Compress, True)

            'Now write the compressed data to the destination file
            compressedStream.Write(buffer, 0, buffer.Length)

        Catch ex As ApplicationException
            MessageBox.Show(ex.Message, "An Error occured during compression", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Make sure we allways close all streams
            If Not (sourceStream Is Nothing) Then
                sourceStream.Close()
            End If
            If Not (compressedStream Is Nothing) Then
                compressedStream.Close()
            End If
            If Not (destinationStream Is Nothing) Then
                destinationStream.Close()
            End If
        End Try

    End Sub

    Public Function DecompressFile(ByVal sourceFile As String, ByVal destinationFile As String) As Boolean

        Dim noError As Boolean = True

        ' make sure the source file is there
        If File.Exists(sourceFile) = False Then
            Throw New FileNotFoundException
        End If

        ' Create the streams and byte arrays needed
        Dim sourceStream As FileStream = Nothing
        Dim destinationStream As FileStream = Nothing
        Dim decompressedStream As GZipStream = Nothing
        Dim quartetBuffer As Byte() = Nothing

        Try
            ' Read in the compressed source stream
            sourceStream = New FileStream(sourceFile, FileMode.Open)

            ' Create a compression stream pointing to the destiantion stream
            decompressedStream = New GZipStream(sourceStream, CompressionMode.Decompress, True)

            ' Read the footer to determine the length of the destiantion file
            quartetBuffer = New Byte(4) {}
            Dim position As Integer = CType(sourceStream.Length, Integer) - 4
            sourceStream.Position = position
            sourceStream.Read(quartetBuffer, 0, 4)
            sourceStream.Position = 0
            Dim checkLength As Integer = BitConverter.ToInt32(quartetBuffer, 0)

            Dim buffer(checkLength + 65536) As Byte
            Dim offset As Integer = 0
            Dim total As Integer = 0
            Dim bytesRead As Integer = 0

            ' Read the compressed data into the buffer
            lblProcess.Text = "Current Process: Decompressing GZip file..."
            currentProgress = 0 : currentProgressScroll = True : Me.Invoke(New MethodInvoker(AddressOf Me.UpdateProgressBar))
            While True
                bytesRead = decompressedStream.Read(buffer, offset, 65536)
                If bytesRead = 0 Then
                    Exit While
                End If
                offset += bytesRead
                total += bytesRead
                lblProgress.Text = "Progress: " & FormatNumber(total, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " bytes processed"
                Application.DoEvents()
            End While

            ' Now write everything to the destination file
            lblProcess.Text = "Current Process: Writing Decompressed File..."
            currentProgressScroll = True : Me.Invoke(New MethodInvoker(AddressOf Me.UpdateProgressBar))
            destinationStream = New FileStream(destinationFile, FileMode.Create)
            destinationStream.Write(buffer, 0, total)

            ' and flush everyhting to clean out the buffer
            destinationStream.Flush()
            buffer = Nothing
        Catch ex As ApplicationException
            MessageBox.Show(ex.Message, "An Error occured during compression", MessageBoxButtons.OK, MessageBoxIcon.Error)
            noError = False
        Finally
            ' Make sure we allways close all streams
            If Not (sourceStream Is Nothing) Then
                sourceStream.Close()
            End If
            If Not (decompressedStream Is Nothing) Then
                decompressedStream.Close()
            End If
            If Not (destinationStream Is Nothing) Then
                destinationStream.Close()
            End If
            GC.Collect()
        End Try
        Return noError

    End Function


    Private Sub tmrStart_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrStart.Tick
        tmrStart.Stop()
        'Dim lastDate As Date = GetLastMarketDate()
        'If lastDate.ToOADate = 0 Then
        '    Dim msg As String = "EveHQ has detected that no market prices have been processed. Would you like to download and process these from Eve-Central now?"
        '    Dim reply As Integer = MessageBox.Show(msg, "Get Initial Prices?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        '    If reply = DialogResult.No Then
        '        Exit Sub
        '    Else
        '        System.Threading.ThreadPool.QueueUserWorkItem(AddressOf GetFirstPrices, Nothing)
        '    End If
        'End If
    End Sub

    Private Sub UpdateProgressBar()
        If currentProgressScroll = True Then
            pbProgress.Style = ProgressBarStyle.Marquee
        Else
            pbProgress.Style = ProgressBarStyle.Continuous
        End If
        pbProgress.Value = CInt(currentProgress)
        pbProgress.Refresh()
    End Sub

#Region "Market Price Settings"

    Private Sub UpdatePriceSettings()
        ' Update the regional list
        For Each chk As CheckBox In grpRegions.Controls
            If EveHQ.Core.HQ.EveHQSettings.MarketRegionList.Contains(chk.Name) = True Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        Next
        ' Update the criteria list
        Dim idx As Integer = 0
        For Each chk As CheckBox In grpCriteria.Controls
            idx = CInt(chk.Name.Substring(16))
            chk.Checked = EveHQ.Core.HQ.EveHQSettings.PriceCriteria(idx)
        Next
        ' Update the checkboxes
        chkEnableLogWatcher.Checked = EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcher
        chkEnableWatcherAtStartup.Checked = EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcherAtStartup
        chkAutoUpdateCurrentPrice.Checked = EveHQ.Core.HQ.EveHQSettings.MarketLogUpdatePrice
        chkAutoUpdatePriceData.Checked = EveHQ.Core.HQ.EveHQSettings.MarketLogUpdateData
        chkNotifyPopup.Checked = EveHQ.Core.HQ.EveHQSettings.MarketLogPopupConfirm
        chkNotifyTray.Checked = EveHQ.Core.HQ.EveHQSettings.MarketLogToolTipConfirm
        chkIgnoreBuyOrders.Checked = EveHQ.Core.HQ.EveHQSettings.IgnoreBuyOrders
        chkIgnoreSellOrders.Checked = EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrders
        ' Update the NUD controls
        nudIgnoreBuyOrderLimit.Value = CDec(EveHQ.Core.HQ.EveHQSettings.IgnoreBuyOrderLimit)
        nudIgnoreSellOrderLimit.Value = CDec(EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrderLimit)
    End Sub

    Private Sub AddRegions()
        EveHQ.Core.HQ.EveHQSettings.MarketRegionList.Clear()
        For Each chk As CheckBox In grpRegions.Controls
            If chk.Checked = True Then
                EveHQ.Core.HQ.EveHQSettings.MarketRegionList.Add(chk.Name)
            End If
        Next
    End Sub

    Private Sub MarketRegionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If startUp = False Then
            Call Me.AddRegions()
        End If
    End Sub

    Private Sub btnAllRegions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllRegions.Click
        For Each chk As CheckBox In grpRegions.Controls
            chk.Checked = True
        Next
        Call Me.AddRegions()
    End Sub

    Private Sub btnEmpireRegions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmpireRegions.Click
        For Each chk As CheckBox In grpRegions.Controls
            If CInt(chk.Tag) > 0 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        Next
        Call Me.AddRegions()
    End Sub

    Private Sub btnNullRegions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNullRegions.Click
        For Each chk As CheckBox In grpRegions.Controls
            If CInt(chk.Tag) = 0 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        Next
        Call Me.AddRegions()
    End Sub

    Private Sub btnNoRegions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoRegions.Click
        For Each chk As CheckBox In grpRegions.Controls
            chk.Checked = False
        Next
        Call Me.AddRegions()
    End Sub

    Private Sub btnAmarr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAmarr.Click
        For Each chk As CheckBox In grpRegions.Controls
            If CInt(chk.Tag) = 500003 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        Next
        Call Me.AddRegions()
    End Sub

    Private Sub btnCaldari_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCaldari.Click
        For Each chk As CheckBox In grpRegions.Controls
            If CInt(chk.Tag) = 500001 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        Next
        Call Me.AddRegions()
    End Sub

    Private Sub btnGallente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGallente.Click
        For Each chk As CheckBox In grpRegions.Controls
            If CInt(chk.Tag) = 500004 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        Next
        Call Me.AddRegions()
    End Sub

    Private Sub btnMinmatar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMinmatar.Click
        For Each chk As CheckBox In grpRegions.Controls
            If CInt(chk.Tag) = 500002 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        Next
        Call Me.AddRegions()
    End Sub

    Private Sub chkEnableLogWatcher_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableLogWatcher.CheckedChanged
        If chkEnableLogWatcher.Checked = True Then
            If frmEveHQ.InitialiseWatchers() = True Then
                EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcher = True
            Else
                MessageBox.Show("Unable to start Market Log Watcher. Please check Eve is installed and the market log export folder exists.", "Error Starting Watcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
                chkEnableLogWatcher.Checked = False
            End If
        Else
            Call frmEveHQ.CancelWatchers()
            EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcher = False
        End If
    End Sub

    Private Sub chkEnableWatcherAtStartup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableWatcherAtStartup.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcherAtStartup = chkEnableWatcherAtStartup.Checked
    End Sub

    Private Sub chkAutoUpdateCurrentPrice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoUpdateCurrentPrice.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.MarketLogUpdatePrice = chkAutoUpdateCurrentPrice.Checked
    End Sub

    Private Sub chkAutoUpdatePriceData_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoUpdatePriceData.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.MarketLogUpdateData = chkAutoUpdatePriceData.Checked
    End Sub

    Private Sub chkNotifyPopup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyPopup.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.MarketLogPopupConfirm = chkNotifyPopup.Checked
    End Sub

    Private Sub chkNotifyTray_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyTray.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.MarketLogToolTipConfirm = chkNotifyTray.Checked
    End Sub

    Private Sub chkIgnoreBuyOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIgnoreBuyOrders.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.IgnoreBuyOrders = chkIgnoreBuyOrders.Checked
    End Sub

    Private Sub chkIgnoreSellOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIgnoreSellOrders.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrders = chkIgnoreSellOrders.Checked
    End Sub

    Private Sub nudIgnoreBuyOrderLimit_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudIgnoreBuyOrderLimit.HandleDestroyed
        EveHQ.Core.HQ.EveHQSettings.IgnoreBuyOrderLimit = nudIgnoreBuyOrderLimit.Value
    End Sub

    Private Sub nudIgnoreBuyOrderLimit_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIgnoreBuyOrderLimit.ValueChanged
        If startUp = False Then
            EveHQ.Core.HQ.EveHQSettings.IgnoreBuyOrderLimit = nudIgnoreBuyOrderLimit.Value
        End If
    End Sub

    Private Sub nudIgnoreSellOrderLimit_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudIgnoreSellOrderLimit.HandleDestroyed
        EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrderLimit = nudIgnoreSellOrderLimit.Value
    End Sub

    Private Sub nudIgnoreSellOrderLimit_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIgnoreSellOrderLimit.ValueChanged
        If startUp = False Then
            EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrderLimit = nudIgnoreSellOrderLimit.Value
        End If
    End Sub

    Private Sub chkPriceCriteria_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPriceCriteria0.CheckedChanged, chkPriceCriteria1.CheckedChanged, chkPriceCriteria2.CheckedChanged, chkPriceCriteria3.CheckedChanged, chkPriceCriteria4.CheckedChanged, chkPriceCriteria5.CheckedChanged, chkPriceCriteria6.CheckedChanged, chkPriceCriteria7.CheckedChanged, chkPriceCriteria8.CheckedChanged, chkPriceCriteria9.CheckedChanged, chkPriceCriteria10.CheckedChanged, chkPriceCriteria11.CheckedChanged
        If startUp = False Then
            Dim idx As Integer = 0
            For Each chk As CheckBox In grpCriteria.Controls
                idx = CInt(chk.Name.Substring(16))
                EveHQ.Core.HQ.EveHQSettings.PriceCriteria(idx) = chk.Checked
            Next
        End If
    End Sub

#End Region

#Region "Custom Prices Functions"
    Private Sub txtSearchPrices_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchPrices.TextChanged
        If Len(txtSearchPrices.Text) > 2 Then
            Dim strSearch As String = txtSearchPrices.Text.Trim.ToLower
            Call Me.UpdatePriceMatrix(strSearch)
        End If
    End Sub
    Private Sub btnResetGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetGrid.Click
        txtSearchPrices.Text = ""
        Call Me.UpdatePriceMatrix("")
    End Sub
    Private Sub UpdatePriceMatrix(Optional ByVal search As String = "")
        ' Loads prices into the listview
        lvwPrices.BeginUpdate()
        lvwPrices.Items.Clear()
        Dim lvItem As New ListViewItem
        Dim itemID As String = ""
        Dim itemData As New EveHQ.Core.EveItem
        Dim price As Double = 0
        If chkShowOnlyCustom.Checked = True Then
            For Each itemID In EveHQ.Core.HQ.CustomPriceList.Keys ' ID
                itemData = CType(EveHQ.Core.HQ.itemData(itemID), Core.EveItem)
                If itemData.Name.ToLower.Contains(search) = True Then
                    If itemData.Published = True Then
                        lvItem = New ListViewItem
                        lvItem.Text = itemData.Name
                        lvItem.Name = itemID
                        lvItem.SubItems.Add(FormatNumber(EveHQ.Core.HQ.BasePriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        If EveHQ.Core.HQ.MarketPriceList.Contains(itemID) Then
                            price = CDbl(EveHQ.Core.HQ.MarketPriceList(itemID))
                            lvItem.SubItems.Add(FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        Else
                            lvItem.SubItems.Add("")
                        End If
                        If EveHQ.Core.HQ.CustomPriceList.Contains(itemID) Then
                            price = CDbl(EveHQ.Core.HQ.CustomPriceList(itemID))
                            lvItem.SubItems.Add(FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        Else
                            lvItem.SubItems.Add("")
                        End If
                        lvwPrices.Items.Add(lvItem)
                    End If
                End If
            Next
        Else
            For Each item As String In EveHQ.Core.HQ.itemList.Keys
                If item.ToLower.Contains(search) = True Then
                    itemID = CStr(EveHQ.Core.HQ.itemList(item))
                    itemData = CType(EveHQ.Core.HQ.itemData(itemID), Core.EveItem)
                    If itemData.Published = True Then
                        lvItem = New ListViewItem
                        lvItem.Text = itemData.Name
                        lvItem.Name = CStr(itemData.ID)
                        lvItem.SubItems.Add(FormatNumber(EveHQ.Core.HQ.BasePriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        If EveHQ.Core.HQ.MarketPriceList.Contains(itemID) Then
                            price = CDbl(EveHQ.Core.HQ.MarketPriceList(itemID))
                            lvItem.SubItems.Add(FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        Else
                            lvItem.SubItems.Add("")
                        End If
                        If EveHQ.Core.HQ.CustomPriceList.Contains(itemID) Then
                            price = CDbl(EveHQ.Core.HQ.CustomPriceList(itemID))
                            lvItem.SubItems.Add(FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        Else
                            lvItem.SubItems.Add("")
                        End If
                        lvwPrices.Items.Add(lvItem)
                    End If
                End If
            Next
        End If
        lvwPrices.EndUpdate()
    End Sub
    Private Sub ctxPrices_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxPrices.Opening
        If lvwPrices.SelectedItems.Count > 0 Then
            Dim selItem As ListViewItem = lvwPrices.SelectedItems(0)
            mnuPriceItemName.Text = selItem.Text
            Dim selItemID As String = selItem.Name
            ' Check if it exists and we can edit/delete it
            If EveHQ.Core.HQ.CustomPriceList.Contains(selItemID) = True Then
                ' Already in custom price list
                mnuPriceAdd.Enabled = False
                mnuPriceDelete.Enabled = True
                mnuPriceEdit.Enabled = True
            Else
                ' Not in price list
                mnuPriceAdd.Enabled = True
                mnuPriceDelete.Enabled = False
                mnuPriceEdit.Enabled = False
            End If
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuPriceDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPriceDelete.Click
        Dim selItem As ListViewItem = lvwPrices.SelectedItems(0)
        mnuPriceItemName.Text = selItem.Text
        Dim selItemID As String = selItem.Name
        ' Double check it exists and delete it
        Call EveHQ.Core.DataFunctions.DeleteCustomPrice(selItemID)
        ' refresh that asset rather than the whole list
        selItem.SubItems(3).Text = ""
    End Sub
    Private Sub mnuPriceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPriceAdd.Click
        Dim selItem As ListViewItem = lvwPrices.SelectedItems(0)
        Dim itemID As String = selItem.Name
        Dim newPrice As New frmModifyPrice
        newPrice.lblBasePrice.Text = FormatNumber(EveHQ.Core.HQ.BasePriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.lblMarketPrice.Text = FormatNumber(EveHQ.Core.HQ.MarketPriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.lblCustomPrice.Text = FormatNumber(EveHQ.Core.HQ.CustomPriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.txtNewPrice.Tag = itemID
        newPrice.txtNewPrice.Text = ""
        newPrice.Text = "Add Price - " & selItem.Text
        newPrice.Tag = "Add"
        newPrice.ShowDialog()
        selItem.SubItems(3).Text = FormatNumber(EveHQ.Core.HQ.CustomPriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
    End Sub
    Private Sub mnuPriceEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPriceEdit.Click
        Dim selItem As ListViewItem = lvwPrices.SelectedItems(0)
        Dim itemID As String = selItem.Name
        Dim newPrice As New frmModifyPrice
        newPrice.lblBasePrice.Text = FormatNumber(EveHQ.Core.HQ.BasePriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.lblMarketPrice.Text = FormatNumber(EveHQ.Core.HQ.MarketPriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.lblCustomPrice.Text = FormatNumber(EveHQ.Core.HQ.CustomPriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.txtNewPrice.Tag = itemID
        newPrice.txtNewPrice.Text = ""
        newPrice.Text = "Modify Price - " & selItem.Text
        newPrice.Tag = "Edit"
        newPrice.ShowDialog()
        selItem.SubItems(3).Text = FormatNumber(EveHQ.Core.HQ.CustomPriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
    End Sub
    Private Sub chkShowOnlyCustom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowOnlyCustom.CheckedChanged
        If Len(txtSearchPrices.Text) > 2 Then
            Dim strSearch As String = txtSearchPrices.Text.Trim.ToLower
            Call Me.UpdatePriceMatrix(strSearch)
        Else
            Call Me.UpdatePriceMatrix()
        End If
    End Sub
#End Region

    Private Sub mnuViewOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewOrders.Click
        Dim marketOrders As New frmMarketOrders
        marketOrders.OrdersFile = clvLogs.SelectedItems(0).Tag.ToString
        marketOrders.Text = "Market Orders - " & clvLogs.SelectedItems(0).SubItems(1).Text & " (" & clvLogs.SelectedItems(0).Text & ") - " & clvLogs.SelectedItems(0).SubItems(2).Text
        marketOrders.ShowDialog()
    End Sub

#Region "Market & Faction Price Feed Routines"

    Private Sub btnUpdateFactionPrices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateFactionPrices.Click
        If GetPriceFeed("FactionPrices", "http://www.eve-prices.net/xml/today.xml", lblFactionPriceUpdateStatus, False) = True Then
            ' Open a persistant DB connection
            If EveHQ.Core.DataFunctions.OpenCustomDatabase = True Then
                Call Me.ParseFactionPriceFeed("FactionPrices", lblFactionPriceUpdateStatus)
                ' Close the connection
                EveHQ.Core.DataFunctions.CloseCustomDatabase()
                EveHQ.Core.HQ.EveHQSettings.LastFactionPriceUpdate = Now
                lblLastFactionPriceUpdate.Text = Format(EveHQ.Core.HQ.EveHQSettings.LastFactionPriceUpdate, "dd/MM/yyyy HH:mm:ss")
                If EveHQ.Core.HQ.EveHQSettings.LastFactionPriceUpdate.AddSeconds(86400) < Now Then
                    btnUpdateFactionPrices.Enabled = True
                    lblFactionPriceUpdateStatus.Text = "Status: Awaiting update."
                Else
                    btnUpdateFactionPrices.Enabled = False
                    lblFactionPriceUpdateStatus.Text = "Status: Inactive due to 24 hour feed restriction."
                End If
                lblFactionPriceUpdateStatus.Text = "Faction Price Update Complete!" : lblFactionPriceUpdateStatus.Refresh()
            Else
                lblFactionPriceUpdateStatus.Text = "Faction Price Update Failed!" : lblFactionPriceUpdateStatus.Refresh()
            End If
        Else
            lblFactionPriceUpdateStatus.Text = "Faction Price Update Failed!" : lblFactionPriceUpdateStatus.Refresh()
        End If
    End Sub
    Private Sub lblFactionPricesBy_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblFactionPricesBy.LinkClicked
        Try
            Process.Start("http://www.eve-prices.net")
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub btnUpdateMarketPrices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateMarketPrices.Click

        ' Check for price criteria
        Dim PCSet As Boolean = False
        For pc As Integer = 0 To 11
            If EveHQ.Core.HQ.EveHQSettings.PriceCriteria(pc) = True Then
                PCSet = True
                Exit For
            End If
        Next
        If PCSet = False Then
            Dim msg As String = ""
            'For pc As Integer = 0 To 11
            '    msg &= "Price Criteria Settings (" & pc.ToString & "): " & EveHQ.Core.HQ.EveHQSettings.PriceCriteria(pc) & ControlChars.CrLf
            'Next
            'msg &= ControlChars.CrLf
            'For pc As Integer = 0 To 11
            '    msg &= "Price Criteria Controls (" & pc.ToString & "): " & CType(grpCriteria.Controls("chkPriceCriteria" & pc.ToString), CheckBox).Checked & ControlChars.CrLf
            'Next
            'msg &= ControlChars.CrLf
            msg &= "You cannot process market prices without first setting price criteria." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "This can be done from the 'Price Settings' tab."
            MessageBox.Show(msg, "Price Criteria Required!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Check for region
        If EveHQ.Core.HQ.EveHQSettings.MarketRegionList.Count = 0 Then
            Dim msg As String = "You cannot process market prices without first selecting valid regions." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "This can be done from the 'Price Settings' tab."
            MessageBox.Show(msg, "Market Regions Required!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Build the URL of the data from region limits
        Dim mpURL As String = "http://eve-central.com/api/marketstat?"
        For Each Region As String In regions.Values
            If EveHQ.Core.HQ.EveHQSettings.MarketRegionList.Contains(Region) Then
                mpURL &= "&regionlimit=" & Region
            End If
        Next

        ' Open a persistent DB connection
        If EveHQ.Core.DataFunctions.OpenCustomDatabase = True Then

            ' Get the stuff that can have market prices
            Dim priceData As Data.DataSet = EveHQ.Core.DataFunctions.GetData("SELECT typeID, typeName FROM invTypes WHERE marketGroupID IS NOT NULL;")
            If priceData IsNot Nothing Then
                If priceData.Tables(0).Rows.Count > 0 Then
                    Dim count As Integer = 0
                    Dim ecURL As String = ""
                    Dim itemRow As DataRow
                    For item As Integer = 0 To priceData.Tables(0).Rows.Count - 1 Step 20
                        count += 1
                        ecURL = mpURL
                        For row As Integer = item To item + 19
                            If row < priceData.Tables(0).Rows.Count Then
                                itemRow = priceData.Tables(0).Rows(row)
                                ecURL &= "&typeid=" & itemRow.Item("typeID").ToString
                            End If
                        Next
                        lblMarketPriceUpdateStatus.Text = "Parsing Batch: " & count.ToString & " of " & Int((priceData.Tables(0).Rows.Count - 1) / 20) + 1 & "..." : lblMarketPriceUpdateStatus.Refresh()
                        If GetPriceFeed("MarketPrices", ecURL, lblMarketPriceUpdateStatus, True) = True Then
                            Call Me.ParseMarketPriceFeed("MarketPrices", lblMarketPriceUpdateStatus)
                        End If
                    Next
                    EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate = Now
                    lblLastMarketPriceUpdate.Text = Format(EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate, "dd/MM/yyyy HH:mm:ss")
                    If EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate.AddSeconds(86400) < Now Then
                        btnUpdateMarketPrices.Enabled = True
                        lblMarketPriceUpdateStatus.Text = "Status: Awaiting update."
                    Else
                        btnUpdateMarketPrices.Enabled = False
                        lblMarketPriceUpdateStatus.Text = "Status: Inactive due to 24 hour feed restriction."
                    End If
                    lblMarketPriceUpdateStatus.Text = "Market Price Update Complete!" : lblMarketPriceUpdateStatus.Refresh()
                End If
            End If
            ' Close the DB
            EveHQ.Core.DataFunctions.CloseCustomDatabase()
        Else
            lblMarketPriceUpdateStatus.Text = "Market Price Update Complete!" : lblMarketPriceUpdateStatus.Refresh()
        End If
    End Sub
    Private Sub lblMarketPricesBy_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblMarketPricesBy.LinkClicked
        Try
            Process.Start("http://www.eve-central.com")
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Function GetPriceFeed(ByVal FeedName As String, ByVal URL As String, ByVal StatusLabel As Label, ByVal SupressProgress As Boolean) As Boolean
        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        ' Create the request to access the server and set credentials
        If SupressProgress = False Then
            StatusLabel.Text = "Setting '" & FeedName & "' Server Address..." : StatusLabel.Refresh()
        End If
        Dim localfile As String = EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml"
        ServicePointManager.DefaultConnectionLimit = 10
        ServicePointManager.Expect100Continue = False
        Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(URL))
        Dim request As HttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)
        request.CachePolicy = policy
        ' Setup proxy server (if required)
        If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
            Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
            If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                EveHQProxy.UseDefaultCredentials = True
            Else
                EveHQProxy.UseDefaultCredentials = False
                EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
            End If
            request.Proxy = EveHQProxy
        End If
        request.Method = WebRequestMethods.File.DownloadFile
        request.UserAgent = "EveHQ v" & My.Application.Info.Version.ToString
        Try
            If SupressProgress = False Then
                StatusLabel.Text = "Contacting '" & FeedName & "' Server..." : StatusLabel.Refresh()
            End If
            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
                Dim filesize As Long = CLng(response.ContentLength)
                'Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(localfile, IO.FileMode.Create)
                        Dim buffer(4095) As Byte
                        Dim read As Integer = 0
                        Dim totalBytes As Long = 0
                        Dim percent As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                            totalBytes += read
                            If filesize <> -1 Then
                                percent = CInt(totalBytes / filesize * 100)
                                If SupressProgress = False Then
                                    StatusLabel.Text = "Downloading '" & FeedName & "'... " & totalBytes & " of " & filesize & " (" & percent & "%)" : StatusLabel.Refresh()
                                End If
                            Else
                                If SupressProgress = False Then
                                    StatusLabel.Text = "Downloading '" & FeedName & "'... " & totalBytes & " of unknown size" : StatusLabel.Refresh()
                                End If
                            End If
                            Application.DoEvents()
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
            If SupressProgress = False Then
                StatusLabel.Text = "Download of '" & FeedName & "' Complete!" : StatusLabel.Refresh()
            End If
            Return True
        Catch ex As Exception
            MessageBox.Show("There was an error downloading the '" & FeedName & "' data: " & ex.Message, "Error in Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Sub ParseFactionPriceFeed(ByVal FeedName As String, ByVal StatusLabel As Label)
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
        Dim feedXML As New XmlDocument
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml") = True Then
            Try
                feedXML.Load(EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml")
                Dim Items As XmlNodeList
                Dim Item As XmlNode
                Items = feedXML.SelectNodes("/factionPriceData/items/item")
                StatusLabel.Text = "Parsing '" & FeedName & "' (" & Items.Count & " Items)..." : StatusLabel.Refresh()
                For Each Item In Items
                    EveHQ.Core.DataFunctions.SetMarketPrice(CLng(Item.ChildNodes(0).InnerText), Double.Parse(Item.ChildNodes(2).InnerText, Globalization.NumberStyles.Number, culture), True)
                Next
            Catch e As Exception
                MessageBox.Show("Unable to parse Faction Price feed:" & ControlChars.CrLf & e.Message, "Error in Price Feed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml") = True Then
                    My.Computer.FileSystem.DeleteFile(EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml")
                End If
            End Try
        End If
    End Sub
    Private Sub ParseMarketPriceFeed(ByVal FeedName As String, ByVal StatusLabel As Label)
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
        Dim feedXML As New XmlDocument
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml") = True Then
            Try
                feedXML.Load(EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml")
                Dim Items As XmlNodeList
                Items = feedXML.SelectNodes("/evec_api/marketstat/type")
                For Each Item As XmlNode In Items
                    Dim typeID As Long = CLng(Item.Attributes("id").Value)
                    Dim avgBuy, avgSell, avgAll As Double
                    Dim medBuy, medSell, medAll As Double
                    Dim minBuy, minSell, minAll As Double
                    Dim maxBuy, maxSell, maxAll As Double
                    avgBuy = Double.Parse(Item.ChildNodes(1).ChildNodes(1).InnerText, Globalization.NumberStyles.Number, culture)
                    medBuy = Double.Parse(Item.ChildNodes(1).ChildNodes(5).InnerText, Globalization.NumberStyles.Number, culture)
                    minBuy = Double.Parse(Item.ChildNodes(1).ChildNodes(3).InnerText, Globalization.NumberStyles.Number, culture)
                    maxBuy = Double.Parse(Item.ChildNodes(1).ChildNodes(2).InnerText, Globalization.NumberStyles.Number, culture)
                    avgSell = Double.Parse(Item.ChildNodes(2).ChildNodes(1).InnerText, Globalization.NumberStyles.Number, culture)
                    medSell = Double.Parse(Item.ChildNodes(2).ChildNodes(5).InnerText, Globalization.NumberStyles.Number, culture)
                    minSell = Double.Parse(Item.ChildNodes(2).ChildNodes(3).InnerText, Globalization.NumberStyles.Number, culture)
                    maxSell = Double.Parse(Item.ChildNodes(2).ChildNodes(2).InnerText, Globalization.NumberStyles.Number, culture)
                    avgAll = Double.Parse(Item.ChildNodes(0).ChildNodes(1).InnerText, Globalization.NumberStyles.Number, culture)
                    medAll = Double.Parse(Item.ChildNodes(0).ChildNodes(5).InnerText, Globalization.NumberStyles.Number, culture)
                    minAll = Double.Parse(Item.ChildNodes(0).ChildNodes(3).InnerText, Globalization.NumberStyles.Number, culture)
                    maxAll = Double.Parse(Item.ChildNodes(0).ChildNodes(2).InnerText, Globalization.NumberStyles.Number, culture)
                    Dim priceArray As New ArrayList
                    priceArray.Add(avgBuy) : priceArray.Add(medBuy) : priceArray.Add(minBuy) : priceArray.Add(maxBuy)
                    priceArray.Add(avgSell) : priceArray.Add(medSell) : priceArray.Add(minSell) : priceArray.Add(maxSell)
                    priceArray.Add(avgAll) : priceArray.Add(medAll) : priceArray.Add(minAll) : priceArray.Add(maxAll)
                    Dim userPrice As Double = EveHQ.Core.DataFunctions.CalculateUserPrice(priceArray)
                    EveHQ.Core.DataFunctions.SetMarketPrice(typeID, userPrice, True)
                Next
            Catch e As Exception
                MessageBox.Show("Unable to parse Market Price feed:" & ControlChars.CrLf & e.Message, "Error in Price Feed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml") = True Then
                    My.Computer.FileSystem.DeleteFile(EveHQ.Core.HQ.cacheFolder & "\" & FeedName & ".xml")
                End If
            End Try
        End If
    End Sub
#End Region
   
    Private Sub btnResetFactionPriceData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetFactionPriceData.Click
        EveHQ.Core.HQ.EveHQSettings.LastFactionPriceUpdate = Now.AddDays(-2)
        lblLastFactionPriceUpdate.Text = Format(EveHQ.Core.HQ.EveHQSettings.LastFactionPriceUpdate, "dd/MM/yyyy HH:mm:ss")
        If EveHQ.Core.HQ.EveHQSettings.LastFactionPriceUpdate.AddSeconds(86400) < Now Then
            btnUpdateFactionPrices.Enabled = True
            lblFactionPriceUpdateStatus.Text = "Status: Awaiting update."
        Else
            btnUpdateFactionPrices.Enabled = False
            lblFactionPriceUpdateStatus.Text = "Status: Inactive due to 24 hour feed restriction."
        End If
    End Sub

    Private Sub btnResetMarketPriceDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetMarketPriceDate.Click
        EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate = Now.AddDays(-2)
        lblLastMarketPriceUpdate.Text = Format(EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate, "dd/MM/yyyy HH:mm:ss")
        If EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate.AddSeconds(86400) < Now Then
            btnUpdateMarketPrices.Enabled = True
            lblMarketPriceUpdateStatus.Text = "Status: Awaiting update."
        Else
            btnUpdateMarketPrices.Enabled = False
            lblMarketPriceUpdateStatus.Text = "Status: Inactive due to 24 hour feed restriction."
        End If
    End Sub
End Class