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
Imports EveHQ.EveData

Public Class Market

End Class

Public Class MarketData
    Public ItemID As String
    Public RegionID As String
    Public HistoryDate As String
    Public MinAll As Double
    Public MaxAll As Double
    Public AvgAll As Double
    Public MedAll As Double
    Public StdAll As Double
    Public VarAll As Double
    Public VolAll As Double
    Public QtyAll As Double
    Public MinBuy As Double
    Public MaxBuy As Double
    Public AvgBuy As Double
    Public MedBuy As Double
    Public StdBuy As Double
    Public VarBuy As Double
    Public VolBuy As Double
    Public QtyBuy As Double
    Public MinSell As Double
    Public MaxSell As Double
    Public AvgSell As Double
    Public MedSell As Double
    Public StdSell As Double
    Public VarSell As Double
    Public VolSell As Double
    Public QtySell As Double
End Class

Public Enum MarketSite
    Battleclinic = 0
    EveMarketeer = 1
End Enum

Public Class MarketFunctions

    Public Shared Function CalculateUserPriceFromPriceArray(priceArray As ArrayList, regionID As String, typeID As String, writeToDB As Boolean) As Double

        ' Get user price
        Dim globalPriceData As New SortedList(Of String, SortedList(Of String, MarketData))
        ' Create a new set of regional data
        Dim regionData As New SortedList(Of String, MarketData)
        globalPriceData.Add(regionID.ToString, regionData)
        Dim newPriceData As New MarketData
        newPriceData.ItemID = typeID.ToString
        newPriceData.RegionID = regionID.ToString
        newPriceData.HistoryDate = Now.ToString
        regionData.Add(newPriceData.ItemID, newPriceData)
        newPriceData.MinAll = CDbl(priceArray(10))
        newPriceData.MaxAll = CDbl(priceArray(11))
        newPriceData.AvgAll = CDbl(priceArray(8))
        newPriceData.MedAll = CDbl(priceArray(9))
        newPriceData.StdAll = 0
        newPriceData.VarAll = 0
        newPriceData.VolAll = 0
        newPriceData.QtyAll = 0
        newPriceData.MinBuy = CDbl(priceArray(2))
        newPriceData.MaxBuy = CDbl(priceArray(3))
        newPriceData.AvgBuy = CDbl(priceArray(0))
        newPriceData.MedBuy = CDbl(priceArray(1))
        newPriceData.StdBuy = 0
        newPriceData.VarBuy = 0
        newPriceData.VolBuy = 0
        newPriceData.QtyBuy = 0
        newPriceData.MinSell = CDbl(priceArray(6))
        newPriceData.MaxSell = CDbl(priceArray(7))
        newPriceData.AvgSell = CDbl(priceArray(4))
        newPriceData.MedSell = CDbl(priceArray(5))
        newPriceData.StdSell = 0
        newPriceData.VarSell = 0
        newPriceData.VolSell = 0
        newPriceData.QtySell = 0

        ' Get the price
        Return CalculateUserPrice(globalPriceData, newPriceData.ItemID, writeToDB)

    End Function

    Public Shared Function CalculateUserPrice(ByVal globalPriceData As SortedList(Of String, SortedList(Of String, MarketData)), typeID As String, writeToDB As Boolean) As Double

        ' Make a note of which item we have used here so we can apply general prices to everything else
        Dim usedPriceList As New SortedList(Of String, Double)

        For Each mpg As PriceGroup In HQ.Settings.PriceGroups.Values

            If mpg.Name <> "<Global>" Then

                Call ParseMarketPriceGroup(globalPriceData, usedPriceList, mpg, writeToDB)

            End If

        Next

        ' See which items are left and apply prices to them - temporarily add them to the <Global> group
        If HQ.Settings.PriceGroups.ContainsKey("<Global>") = True Then

            Dim mpg As PriceGroup = HQ.Settings.PriceGroups("<Global>")
            For Each itemID As String In StaticData.ItemMarketGroups.Keys
                If usedPriceList.ContainsKey(itemID) = False Then
                    mpg.TypeIDs.Add(itemID)
                End If
            Next

            Call ParseMarketPriceGroup(globalPriceData, usedPriceList, mpg, writeToDB)

            ' Clear the typeIDs
            mpg.TypeIDs.Clear()

        End If

        If usedPriceList.ContainsKey(typeID) Then
            Return usedPriceList(typeID)
        Else
            Return 0
        End If

    End Function

    Public Shared Sub ParseMarketPriceGroup(ByRef globalPriceData As SortedList(Of String, SortedList(Of String, MarketData)), ByRef usedPriceList As SortedList(Of String, Double), mpg As PriceGroup, writeToDB As Boolean)

        Dim marketPrice As Double
        Dim priceCount As Integer
        Dim priceFlagList As New List(Of PriceGroupFlags)
        Dim regionPrices As SortedList(Of String, MarketData)
        Dim itemPrices As MarketData

        ' Establish which price criteria we should be using
        Dim pfs As Integer = mpg.PriceFlags
        priceFlagList.Clear()
        For i As Integer = 0 To 30
            If (pfs Or CInt(2 ^ i)) = pfs Then
                priceFlagList.Add(CType(CInt(2 ^ i), PriceGroupFlags))
            End If
        Next

        ' Get each itemID
        For Each itemID As String In mpg.TypeIDs
            ' Set the market price and price count to nil
            marketPrice = 0
            priceCount = 0

            ' Go through each EveGalaticRegion and apply the correct prices
            For Each regionID As String In mpg.RegionIDs

                If globalPriceData.ContainsKey(regionID) Then
                    regionPrices = globalPriceData(regionID)

                    If regionPrices.ContainsKey(itemID) = True Then

                        itemPrices = regionPrices(itemID)

                        For Each pf As PriceGroupFlags In priceFlagList
                            ' Determine what we do here!
                            Select Case pf
                                Case PriceGroupFlags.MinAll
                                    If itemPrices.MinAll <> 0 Then
                                        marketPrice += itemPrices.MinAll
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.MinBuy
                                    If itemPrices.MinBuy <> 0 Then
                                        marketPrice += itemPrices.MinBuy
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.MinSell
                                    If itemPrices.MinSell <> 0 Then
                                        marketPrice += itemPrices.MinSell
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.MaxAll
                                    If itemPrices.MaxAll <> 0 Then
                                        marketPrice += itemPrices.MaxAll
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.MaxBuy
                                    If itemPrices.MaxBuy <> 0 Then
                                        marketPrice += itemPrices.MaxBuy
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.MaxSell
                                    If itemPrices.MaxSell <> 0 Then
                                        marketPrice += itemPrices.MaxSell
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.AvgAll
                                    If itemPrices.AvgAll <> 0 Then
                                        marketPrice += itemPrices.AvgAll
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.AvgBuy
                                    If itemPrices.AvgBuy <> 0 Then
                                        marketPrice += itemPrices.AvgBuy
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.AvgSell
                                    If itemPrices.AvgSell <> 0 Then
                                        marketPrice += itemPrices.AvgSell
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.MedAll
                                    If itemPrices.MedAll <> 0 Then
                                        marketPrice += itemPrices.MedAll
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.MedBuy
                                    If itemPrices.MedBuy <> 0 Then
                                        marketPrice += itemPrices.MedBuy
                                        priceCount += 1
                                    End If
                                Case PriceGroupFlags.MedSell
                                    If itemPrices.MedSell <> 0 Then
                                        marketPrice += itemPrices.MedSell
                                        priceCount += 1
                                    End If

                            End Select

                        Next

                    End If

                End If

            Next

            ' Calculate an average of the prices if we need to
            If priceCount > 0 Then
                marketPrice = Math.Round(marketPrice / priceCount, 2)


            End If

            ' Add the price to the used list
            usedPriceList.Add(itemID, marketPrice)

        Next

    End Sub
End Class
