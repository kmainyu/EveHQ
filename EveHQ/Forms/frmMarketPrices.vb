﻿Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports System.IO.Compression

Public Class frmMarketPrices
    Dim saveLoc As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\MarketXMLs"
    Dim insertStat As String = "INSERT INTO marketStats (statDate, typeID, regionID, systemID, volAll, avgAll, maxAll, minAll, stdAll, medAll, volBuy, avgBuy, maxBuy, minBuy, stdBuy, medBuy, volSell, avgSell, maxSell, minSell, stdSell, medSell) "
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
            strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_type ON marketStats (typeID)")
            strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_region ON marketStats (regionID)")
            strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_system ON marketStats (systemID)")
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
        strSQL = insertStat & "VALUES (" & orderDate.ToOADate - 2 & ", " & oTypeID.ToString & ", " & oReg.ToString & ", " & oSys.ToString & ", " & volAll & ", " & avgAll & ", " & maxAll & ", " & minAll & ", " & stdAll & ", " & medAll & ", " & volBuy & ", " & avgBuy & ", " & maxBuy & ", " & minBuy & ", " & stdBuy & ", " & medBuy & ", " & volSell & ", " & avgSell & ", " & maxSell & ", " & minSell & ", " & stdSell & ", " & medSell & ");"
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
            For Each regionRow As DataRow In regionSet.Tables(0).Rows
                ' Create a batch of checkboxes for the regions
                Dim chk As New CheckBox
                chk.Text = CStr(regionRow.Item("regionName"))
                chk.Name = CStr(regionRow.Item("regionID"))
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
        lvwLogs.BeginUpdate()
        lvwLogs.Items.Clear()
        For Each file As String In My.Computer.FileSystem.GetFiles(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Eve\Logs\Marketlogs", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
            lvwLogs.Items.Add(file)
        Next
        lvwLogs.EndUpdate()

        ' Start the initial timing check (but do last so form is drawn)
        tmrStart.Enabled = True

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
            Dim dateSQL As String = "INSERT INTO marketDates (priceDate) VALUES (" & orderDate.ToOADate - 2 & ");"
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

    Private Sub AddRegions()
        EveHQ.Core.HQ.EveHQSettings.MarketRegionList.Clear()
        For Each chk As CheckBox In grpRegions.Controls
            EveHQ.Core.HQ.EveHQSettings.MarketRegionList.Add(chk.Name)
        Next
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

    Private Sub tmrStart_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrStart.Tick
        tmrStart.Stop()
        Dim lastDate As Date = GetLastMarketDate()
        If lastDate.ToOADate = 0 Then
            Dim msg As String = "EveHQ has detected that no market prices have been processed. Would you like to download and process these from Eve-Central now?"
            Dim reply As Integer = MessageBox.Show(msg, "Get Initial Prices?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                System.Threading.ThreadPool.QueueUserWorkItem(AddressOf GetFirstPrices, Nothing)
            End If
        End If
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


#Region "Market Log Watcher Routines"
    Private Sub chkEnableLogWatcher_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableLogWatcher.CheckedChanged
        If chkEnableLogWatcher.Checked = True Then
            If frmEveHQ.InitialiseWatchers() = True Then
                'EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcher = True
            Else
                MessageBox.Show("Unable to start Market Log Watcher. Please check your Eve folders in the EveHQ Settings.", "Error Starting Watcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
                chkEnableLogWatcher.Checked = False
            End If
        Else
            Call frmEveHQ.CancelWatchers()
            'EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcher = False
        End If
    End Sub

#End Region

    
End Class