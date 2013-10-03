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
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Imports EveHQ.Market
Imports EveHQ.Common.Extensions
Imports System.Threading.Tasks
Imports EveHQ.EveData

Public Class DataFunctions
    Shared ReadOnly Culture As CultureInfo = New CultureInfo("en-GB")
    
#Region "Pricing Functions"

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

#End Region
    
End Class
