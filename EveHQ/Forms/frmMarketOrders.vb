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
Imports System.IO
Imports ZedGraph
Imports DevComponents.AdvTree

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
            Call Me.ProcessMLPrices(cOrdersFile, True)
        End Set
    End Property

    Private Function ProcessMLPrices(ByVal orderFile As String, WriteToDB As Boolean) As Boolean
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

        If header <> "price,volRemaining,typeID,range,orderID,volEntered,minVolume,bid,issued,duration,stationID,regionID,solarSystemID,jumps," And header <> "price,volRemaining,typeID,range,orderID,volEntered,minVolume,bid,issueDate,duration,stationID,regionID,solarSystemID,jumps," Then
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
                Call CalculateMLStats(CType(items(item), ArrayList), orderdate, WriteToDB)
            Next

            endTime = Now
            timeTaken = endTime - startTime
            items.Clear() : items = Nothing
            itemOrders.Clear() : itemOrders = Nothing
            GC.Collect()
            'MessageBox.Show("Time taken to parse data for " & items.Count.ToString & " items = " & timeTaken.TotalSeconds.ToString, "Price Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Function

    Private Sub CalculateMLStats(ByVal orderList As ArrayList, ByVal orderDate As Date, WriteToDB As Boolean)
        Dim orderDetails(), oDate As String
        Dim issueDate As Date
        Dim TimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        Dim OldTimeFormat As String = "yyyy-MM-dd"
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

        adtSellers.BeginUpdate() : adtBuyers.BeginUpdate()
        adtSellers.Nodes.Clear() : adtBuyers.Nodes.Clear()
        For Each order As String In orderList
            order = order.Replace(Chr(0), "")
            orderDetails = order.Split(",".ToCharArray)
            oPrice = Double.Parse(orderDetails(0).Trim, Globalization.NumberStyles.Any, culture)
            oVol = Long.Parse(orderDetails(1).Trim, Globalization.NumberStyles.Any, culture)
            oTypeID = Long.Parse(orderDetails(2).Trim, Globalization.NumberStyles.Any, culture)
            oRange = Long.Parse(orderDetails(3).Trim, Globalization.NumberStyles.Any, culture)
            oID = Long.Parse(orderDetails(4).Trim, Globalization.NumberStyles.Any, culture)
            oVolEntered = Long.Parse(orderDetails(5).Trim, Globalization.NumberStyles.Any, culture)
            oMinVol = Long.Parse(orderDetails(6).Trim, Globalization.NumberStyles.Any, culture)
            oType = Math.Abs(CLng(CBool(orderDetails(7).Trim)))
            oDate = CStr(orderDetails(8))
            oDuration = Integer.Parse(orderDetails(9).Trim, Globalization.NumberStyles.Any, culture)
            oStation = Long.Parse(orderDetails(10).Trim, Globalization.NumberStyles.Any, culture)
            oReg = Long.Parse(orderDetails(11).Trim, Globalization.NumberStyles.Any, culture)
            oSys = Long.Parse(orderDetails(12).Trim, Globalization.NumberStyles.Any, culture)
            oJumps = Integer.Parse(orderDetails(13).Trim, Globalization.NumberStyles.Any, culture)

            ' Display the order (irrespective of whether we process it)
            If DateTime.TryParseExact(oDate, TimeFormat, Nothing, Globalization.DateTimeStyles.None, issueDate) = False Then
                issueDate = DateTime.ParseExact(oDate, OldTimeFormat, Nothing, Globalization.DateTimeStyles.None)
            End If
            orderExpires = issueDate - Now
            orderExpires = orderExpires.Add(New TimeSpan(oDuration, 0, 0, 0))
            If orderExpires.TotalSeconds <= 0 Then
                orderExpiry = "Expired!"
            Else
                orderExpiry = EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False)
            End If
            Dim newOrder As New Node
            If IndustryPluginAvailable = True Then
                newOrder.Text = CStr(Industry.Instance.GetPlugInData(oStation, 0))
            Else
                newOrder.Text = oStation.ToString
            End If
            newOrder.Cells.Add(New Cell(oVol.ToString("N0")))
            newOrder.Cells.Add(New Cell(oPrice.ToString("N2")))
            newOrder.Cells.Add(New Cell(orderExpiry))
            newOrder.Cells(3).Tag = orderExpires.TotalSeconds
            If oType = 0 Then
                ' Sell Order
                adtSellers.Nodes.Add(newOrder)
            Else
                ' Buy Order
                adtBuyers.Nodes.Add(newOrder)
            End If

            ' Check if we process this
            ProcessOrder = True
            If oType = 0 Then ' Sell Order
                If EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrders = True And oPrice > (EveHQ.Core.HQ.EveHQSettings.IgnoreSellOrderLimit * CDbl(EveHQ.Core.HQ.itemData(oTypeID.ToString).BasePrice)) Then
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
        EveHQ.Core.AdvTreeSorter.Sort(adtSellers, 1, True, True)
        EveHQ.Core.AdvTreeSorter.Sort(adtBuyers, 1, True, True)
        adtSellers.EndUpdate() : adtBuyers.EndUpdate()

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

        ' Show the order data
        LblSellOrderVol.Text = volSell.ToString("N0")
        lblSellOrderMin.Text = minSell.ToString("N2")
        lblSellOrderMax.Text = maxSell.ToString("N2")
        lblSellOrderMean.Text = avgSell.ToString("N2")
        lblSellOrderMedian.Text = medSell.ToString("N2")
        lblSellOrderStd.Text = stdSell.ToString("N2")
        lblBuyOrderVol.Text = volBuy.ToString("N0")
        lblBuyOrderMin.Text = minBuy.ToString("N2")
        lblBuyOrderMax.Text = maxBuy.ToString("N2")
        lblBuyOrderMean.Text = avgBuy.ToString("N2")
        lblBuyOrderMedian.Text = medBuy.ToString("N2")
        lblBuyOrderStd.Text = stdBuy.ToString("N2")
        lblAllOrderVol.Text = volAll.ToString("N0")
        lblAllOrderMin.Text = minAll.ToString("N2")
        lblAllOrderMax.Text = maxAll.ToString("N2")
        lblAllOrderMean.Text = avgAll.ToString("N2")
        lblAllOrderMedian.Text = medAll.ToString("N2")
        lblAllOrderStd.Text = stdAll.ToString("N2")

        'Calculate the user price
        Dim priceArray As New ArrayList
        priceArray.Add(avgBuy) : priceArray.Add(medBuy) : priceArray.Add(minBuy) : priceArray.Add(maxBuy)
        priceArray.Add(avgSell) : priceArray.Add(medSell) : priceArray.Add(minSell) : priceArray.Add(maxSell)
        priceArray.Add(avgAll) : priceArray.Add(medAll) : priceArray.Add(minAll) : priceArray.Add(maxAll)
        typeID = oTypeID

        ' Get the price
        UserPrice = EveHQ.Core.MarketFunctions.CalculateUserPriceFromPriceArray(priceArray, oReg.ToString, oTypeID.ToString, WriteToDB)

        lblYourPrice.Text = UserPrice.ToString("N2")
        lblCurrentPrice.Text = EveHQ.Core.DataFunctions.GetPrice(oTypeID.ToString).ToString("N2")

        ' Draw the graph
        If avgAll <> 0 And stdAll <> 0 Then
            Call Me.DrawGraph(avgAll, stdAll)
        End If

    End Sub

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
        lblCurrentPrice.Text = UserPrice.ToString("N2")
    End Sub

    Private Sub btnSetCustomPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetCustomPrice.Click
        ' Store the user's price in the database
        Call EveHQ.Core.DataFunctions.SetCustomPrice(typeID, UserPrice, False)
        lblCurrentPrice.Text = UserPrice.ToString("N2")
    End Sub

    Private Sub adtBuyers_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtBuyers.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub adtSellers_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtSellers.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

End Class