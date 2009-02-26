Imports System.IO
Imports DotNetLib.Windows.Forms
Imports ZedGraph

Public Class frmMarketOrders
    Dim cOrdersFile As String = ""
    Dim Industry As EveHQ.Core.PlugIn
    Dim IndustryPluginAvailable As Boolean = False
    Dim SellPricePoints, BuyPricePoints As New ArrayList
    Dim typeID As Long = 0
    Dim UserPrice As Double = 0

    Public Property OrdersFile() As String
        Get
            Return cOrdersFile
        End Get
        Set(ByVal value As String)
            cOrdersFile = value
            Call Me.CheckIndustry()
            Call Me.ProcessMLPrices(cOrdersFile)
        End Set
    End Property

    Private Function ProcessMLPrices(ByVal orderFile As String) As Boolean
        Dim startTime, endTime As Date
        Dim timeTaken As TimeSpan
        Dim orderFI As New FileInfo(orderFile)
        Dim orderdate As Date = Now
        Dim items As New SortedList
        Dim itemOrders As New ArrayList
        Dim FileInUse As Boolean = False
        Dim sr As StreamReader = Nothing
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
            Return False
        Else
            startTime = Now
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

            endTime = Now
            timeTaken = endTime - startTime
            'MessageBox.Show("Time taken to read " & orders.ToString & " orders = " & timeTaken.TotalSeconds.ToString)

            ' Calculate global statistics
            startTime = Now
            Dim itemCount As Integer = items.Count
            Dim count As Integer = 0
            For Each item As String In items.Keys
                count += 1
                Call CalculateMLStats(CType(items(item), ArrayList), orderdate)
            Next

            endTime = Now
            timeTaken = endTime - startTime
            items.Clear() : items = Nothing
            itemOrders.Clear() : itemOrders = Nothing
            GC.Collect()
            'MessageBox.Show("Time taken to parse data for " & items.Count.ToString & " items = " & timeTaken.TotalSeconds.ToString, "Price Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Function

    Private Sub CalculateMLStats(ByVal orderList As ArrayList, ByVal orderDate As Date)
        Dim orderDetails(), oDate As String
        Dim issueDate As Date
        Dim TimeFormat As String = "yyyy-MM-dd HH:mm:ss.fff"
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
        Dim orderExpires As TimeSpan
        Dim orderExpiry As String = ""
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
        BuyPricePoints.Clear() : SellPricePoints.Clear()

        countBuy = 0 : countSell = 0 : countAll = 0
        volBuy = 0 : volSell = 0 : volAll = 0
        minBuy = 0 : minSell = 0 : minAll = 0
        maxBuy = 0 : maxSell = 0 : maxAll = 0
        valBuy = 0 : valSell = 0 : valAll = 0
        devBuy = 0 : devSell = 0 : devAll = 0

        clvSellers.BeginUpdate()
        clvSellers.Items.Clear()
        For Each order As String In orderList
            orderDetails = order.Split(",".ToCharArray)

            oPrice = CDbl(orderDetails(0).Trim)
            oPrice = Double.Parse(orderDetails(0).Trim, Globalization.NumberStyles.Number, culture)
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

            ' Display the order (irrespective of whether we process it)
            issueDate = DateTime.ParseExact(oDate, TimeFormat, Nothing, Globalization.DateTimeStyles.None)
            orderExpires = issueDate - Now
            orderExpires = orderExpires.Add(New TimeSpan(oDuration, 0, 0, 0))
            If orderExpires.TotalSeconds <= 0 Then
                orderExpiry = "Expired!"
            Else
                orderExpiry = EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False)
            End If
            Dim newOrder As New ContainerListViewItem
            If IndustryPluginAvailable = True Then
                newOrder.Text = CStr(Industry.Instance.GetPlugInData(oStation))
            Else
                newOrder.Text = oStation.ToString
            End If
            If oType = 0 Then
                ' Sell Order
                clvSellers.Items.Add(newOrder)
            Else
                ' Buy Order
                clvBuyers.Items.Add(newOrder)
            End If
            newOrder.SubItems(1).Text = FormatNumber(oVol, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newOrder.SubItems(2).Text = FormatNumber(oPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newOrder.SubItems(3).Tag = orderExpires.TotalSeconds
            newOrder.SubItems(3).Text = orderExpiry

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
                        SellPricePoints.Add(oPrice)
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
                        BuyPricePoints.Add(oPrice)
                End Select
            End If
        Next
        clvSellers.EndUpdate()

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

        ' Write data to the database
        'strSQL = insertStat & "VALUES (" & orderDate.ToOADate - 2 & ", " & oTypeID.ToString & ", " & oReg.ToString & ", " & oSys.ToString & ", " & volAll & ", " & avgAll & ", " & maxAll & ", " & minAll & ", " & stdAll & ", " & medAll & ", " & volBuy & ", " & avgBuy & ", " & maxBuy & ", " & minBuy & ", " & stdBuy & ", " & medBuy & ", " & volSell & ", " & avgSell & ", " & maxSell & ", " & minSell & ", " & stdSell & ", " & medSell & ");"
        'If EveHQ.Core.DataFunctions.SetData(strSQL) = False Then
        '    MessageBox.Show("There was an error writing data to the marketStats database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL, "Error Writing Market Stats", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        'End If

        ' Show the order data
        LblSellOrderVol.Text = FormatNumber(volSell, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblSellOrderMin.Text = FormatNumber(minSell, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblSellOrderMax.Text = FormatNumber(maxSell, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblSellOrderMean.Text = FormatNumber(avgSell, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblSellOrderMedian.Text = FormatNumber(medSell, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblSellOrderStd.Text = FormatNumber(stdSell, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblBuyOrderVol.Text = FormatNumber(volBuy, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblBuyOrderMin.Text = FormatNumber(minBuy, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblBuyOrderMax.Text = FormatNumber(maxBuy, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblBuyOrderMean.Text = FormatNumber(avgBuy, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblBuyOrderMedian.Text = FormatNumber(medBuy, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblBuyOrderStd.Text = FormatNumber(stdBuy, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblAllOrderVol.Text = FormatNumber(volAll, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblAllOrderMin.Text = FormatNumber(minAll, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblAllOrderMax.Text = FormatNumber(maxAll, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblAllOrderMean.Text = FormatNumber(avgAll, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblAllOrderMedian.Text = FormatNumber(medAll, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblAllOrderStd.Text = FormatNumber(stdAll, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        'Calculate the user price
        Dim priceArray As New ArrayList
        priceArray.Add(avgBuy) : priceArray.Add(medBuy) : priceArray.Add(minBuy) : priceArray.Add(maxBuy)
        priceArray.Add(avgSell) : priceArray.Add(medSell) : priceArray.Add(minSell) : priceArray.Add(maxSell)
        priceArray.Add(avgAll) : priceArray.Add(medAll) : priceArray.Add(minAll) : priceArray.Add(maxAll)
        typeID = oTypeID : UserPrice = CalculateUserPrice(priceArray)
        lblYourPrice.Text = FormatNumber(UserPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCurrentPrice.Text = FormatNumber(EveHQ.Core.DataFunctions.GetPrice(oTypeID.ToString), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Draw the gay graph
        Call Me.DrawGraph(avgAll, stdAll)

    End Sub

    Private Function CalculateUserPrice(ByVal priceArray As ArrayList) As Double
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

    Private Sub CheckIndustry()
        ' Check for the existence of the CorpHQ plug-in and try and get the standing data
        Dim PluginName As String = "EveHQ Prism"
        Industry = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PluginName), Core.PlugIn)
        If Industry IsNot Nothing Then
            If Industry.Status = EveHQ.Core.PlugIn.PlugInStatus.Active Then
                IndustryPluginAvailable = True
            Else
                ' Plug-in is not loaded so best not try to access it!
                IndustryPluginAvailable = False
            End If
        Else
            IndustryPluginAvailable = False
        End If
    End Sub

    Private Sub DrawGraph(ByVal mean As Double, ByVal sd As Double)
        Dim myPane As GraphPane = zgcPrices.GraphPane

        ' Set the titles and axis labels
        myPane.Title.Text = "Market Price Analysis"
        myPane.XAxis.Title.Text = "Price (Isk)"
        myPane.XAxis.Scale.Min = mean - (3 * sd)
        myPane.XAxis.Scale.Max = mean + (3 * sd)
        myPane.XAxis.Scale.IsUseTenPower = False
        myPane.YAxis.Scale.IsVisible = False
        myPane.YAxis.Title.IsVisible = False
        myPane.YAxis.Scale.Min = 0
        myPane.YAxis.Scale.Max = NormalDist(mean, sd, mean) * 1.2
        'myPane.YAxis.Scale.MaxAuto = True

        ' Create some points from 0 to 5000
        Dim list, selllist, buylist As New PointPairList()
        Dim x As Double, y As Double
        Dim range As Double = mean + (3 * sd) - (mean - (3 * sd))
        Dim inc As Double = range / 2000
        For x = mean - (3 * sd) To mean + (3 * sd) Step inc
            y = NormalDist(x, sd, mean)
            list.Add(x, y)
        Next x
        ' Add the Sell Prices
        For Each price As Double In SellPricePoints
            selllist.Add(price, myPane.YAxis.Scale.Max / 10)
        Next
        ' Add the Buy Prices
        For Each price As Double In BuyPricePoints
            buylist.Add(price, myPane.YAxis.Scale.Max / 10)
        Next

        ' Generate a blue curve with circle symbols, and "My Curve 2" in the legend
        Dim myBCurve As LineItem = myPane.AddCurve("Buy Prices", buylist, Color.Green, SymbolType.XCross)
        Dim mySCurve As LineItem = myPane.AddCurve("Sell Prices", selllist, Color.Black, SymbolType.XCross)
        Dim myCurve As LineItem = myPane.AddCurve("Lock Time", list, Color.Blue, SymbolType.None)
        myCurve.Line.IsSmooth = True
        myCurve.Label.IsVisible = False
        myBCurve.Line.IsVisible = False
        mySCurve.Line.IsVisible = False

        ' Fill the area under the curve with a white-red gradient at 45 degrees
        myCurve.Line.Fill = New Fill(Color.White, Color.LightCoral, 45.0F)
        ' Make the symbols opaque by filling them with white
        myCurve.Symbol.Fill = New Fill(Color.White)

        ' Fill the axis background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
        myPane.Legend.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
        myPane.Legend.FontSpec.FontColor = Color.MidnightBlue

        ' Fill the pane background with a color gradient
        myPane.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
        ' Calculate the Axis Scale Ranges
        zgcPrices.AxisChange()
    End Sub

    Private Function NormalDist(ByVal x As Double, ByVal s As Double, ByVal m As Double) As Double
        Return (m / (s * Math.Sqrt(2 * Math.PI))) * Math.Exp(-1 * (Math.Pow(x - m, 2) / (2 * Math.Pow(s, 2))))
    End Function

    Private Sub btnSetMarketPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetMarketPrice.Click
        ' Store the user's price in the database
        Call EveHQ.Core.DataFunctions.SetMarketPrice(typeID, UserPrice, False)
        lblCurrentPrice.Text = FormatNumber(UserPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
    End Sub

    Private Sub btnSetCustomPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetCustomPrice.Click
        ' Store the user's price in the database
        Call EveHQ.Core.DataFunctions.SetCustomPrice(typeID, UserPrice, False)
        lblCurrentPrice.Text = FormatNumber(UserPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
    End Sub
End Class