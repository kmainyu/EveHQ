'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================


Imports EveHQ.EveData
Imports EveHQ.EveApi
Imports EveHQ.Market
Imports System.Threading.Tasks
Imports EveHQ.Common.Extensions

Public Class DataFunctions
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

                Dim itemIdNumbersToRequest As IEnumerable(Of Integer) = (From itemId In filteredIdNumbers Where StaticData.Types(itemId).MarketGroupId <> 0 Select itemId).ToList

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

        Dim distinctItems As List(Of Integer) = itemIDs.Distinct().ToList

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
End Class
