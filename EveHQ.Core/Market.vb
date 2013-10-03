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

   Public Shared Function CalculateUserPriceFromPriceArray(PriceArray As ArrayList, regionID As String, typeID As String, WriteToDB As Boolean) As Double

        ' Get user price
        Dim GlobalPriceData As New SortedList(Of String, SortedList(Of String, EveHQ.Core.MarketData))
        ' Create a new set of regional data
        Dim RegionData As New SortedList(Of String, EveHQ.Core.MarketData)
        GlobalPriceData.Add(regionID.ToString, RegionData)
        Dim NewPriceData As New EveHQ.Core.MarketData
        NewPriceData.ItemID = typeID.ToString
        NewPriceData.RegionID = regionID.ToString
        NewPriceData.HistoryDate = Now.ToString
        RegionData.Add(NewPriceData.ItemID, NewPriceData)
        NewPriceData.MinAll = CDbl(PriceArray(10))
        NewPriceData.MaxAll = CDbl(PriceArray(11))
        NewPriceData.AvgAll = CDbl(PriceArray(8))
        NewPriceData.MedAll = CDbl(PriceArray(9))
        NewPriceData.StdAll = 0
        NewPriceData.VarAll = 0
        NewPriceData.VolAll = 0
        NewPriceData.QtyAll = 0
        NewPriceData.MinBuy = CDbl(PriceArray(2))
        NewPriceData.MaxBuy = CDbl(PriceArray(3))
        NewPriceData.AvgBuy = CDbl(PriceArray(0))
        NewPriceData.MedBuy = CDbl(PriceArray(1))
        NewPriceData.StdBuy = 0
        NewPriceData.VarBuy = 0
        NewPriceData.VolBuy = 0
        NewPriceData.QtyBuy = 0
        NewPriceData.MinSell = CDbl(PriceArray(6))
        NewPriceData.MaxSell = CDbl(PriceArray(7))
        NewPriceData.AvgSell = CDbl(PriceArray(4))
        NewPriceData.MedSell = CDbl(PriceArray(5))
        NewPriceData.StdSell = 0
        NewPriceData.VarSell = 0
        NewPriceData.VolSell = 0
        NewPriceData.QtySell = 0

        ' Get the price
        Return EveHQ.Core.MarketFunctions.CalculateUserPrice(GlobalPriceData, NewPriceData.ItemID, WriteToDB)

    End Function

    Public Shared Function CalculateUserPrice(ByVal GlobalPriceData As SortedList(Of String, SortedList(Of String, MarketData)), typeID As String, WriteToDB As Boolean) As Double

        ' Make a note of which item we have used here so we can apply general prices to everything else
        Dim UsedPriceList As New SortedList(Of String, Double)

        For Each MPG As EveHQ.Core.PriceGroup In EveHQ.Core.HQ.Settings.PriceGroups.Values

            If MPG.Name <> "<Global>" Then

                Call ParseMarketPriceGroup(GlobalPriceData, UsedPriceList, MPG, WriteToDB)

            End If

        Next

        ' See which items are left and apply prices to them - temporarily add them to the <Global> group
        If EveHQ.Core.HQ.Settings.PriceGroups.ContainsKey("<Global>") = True Then

            Dim MPG As EveHQ.Core.PriceGroup = EveHQ.Core.HQ.Settings.PriceGroups("<Global>")
            For Each itemID As String In StaticData.ItemMarketGroups.Keys
                If UsedPriceList.ContainsKey(itemID) = False Then
                    MPG.TypeIDs.Add(itemID)
                End If
            Next

            Call ParseMarketPriceGroup(GlobalPriceData, UsedPriceList, MPG, WriteToDB)

            ' Clear the typeIDs
            MPG.TypeIDs.Clear()

        End If

        If UsedPriceList.ContainsKey(typeID) Then
            Return UsedPriceList(typeID)
        Else
            Return 0
        End If

    End Function

    Public Shared Sub ParseMarketPriceGroup(ByRef GlobalPriceData As SortedList(Of String, SortedList(Of String, MarketData)), ByRef UsedPriceList As SortedList(Of String, Double), MPG As EveHQ.Core.PriceGroup, WriteToDB As Boolean)

        Dim MarketPrice As Double = 0
        Dim PriceCount As Integer = 0
        Dim PriceFlagList As New List(Of EveHQ.Core.PriceGroupFlags)
        Dim RegionPrices As New SortedList(Of String, MarketData)
        Dim ItemPrices As New MarketData

        ' Establish which price criteria we should be using
        Dim PFS As Integer = MPG.PriceFlags
        PriceFlagList.Clear()
        For i As Integer = 0 To 30
            If (PFS Or CInt(2 ^ i)) = PFS Then
                PriceFlagList.Add(CType(CInt(2 ^ i), Core.PriceGroupFlags))
            End If
        Next

        ' Get each itemID
        For Each ItemID As String In MPG.TypeIDs
            ' Set the market price and price count to nil
            MarketPrice = 0
            PriceCount = 0

            ' Go through each EveGalaticRegion and apply the correct prices
            For Each RegionID As String In MPG.RegionIDs

                If GlobalPriceData.ContainsKey(RegionID) Then
                    RegionPrices = GlobalPriceData(RegionID)

                    If RegionPrices.ContainsKey(ItemID) = True Then

                        ItemPrices = RegionPrices(ItemID)

                        For Each PF As EveHQ.Core.PriceGroupFlags In PriceFlagList
                            ' Determine what we do here!
                            Select Case PF
                                Case Core.PriceGroupFlags.MinAll
                                    If ItemPrices.MinAll <> 0 Then
                                        MarketPrice += ItemPrices.MinAll
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.MinBuy
                                    If ItemPrices.MinBuy <> 0 Then
                                        MarketPrice += ItemPrices.MinBuy
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.MinSell
                                    If ItemPrices.MinSell <> 0 Then
                                        MarketPrice += ItemPrices.MinSell
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.MaxAll
                                    If ItemPrices.MaxAll <> 0 Then
                                        MarketPrice += ItemPrices.MaxAll
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.MaxBuy
                                    If ItemPrices.MaxBuy <> 0 Then
                                        MarketPrice += ItemPrices.MaxBuy
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.MaxSell
                                    If ItemPrices.MaxSell <> 0 Then
                                        MarketPrice += ItemPrices.MaxSell
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.AvgAll
                                    If ItemPrices.AvgAll <> 0 Then
                                        MarketPrice += ItemPrices.AvgAll
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.AvgBuy
                                    If ItemPrices.AvgBuy <> 0 Then
                                        MarketPrice += ItemPrices.AvgBuy
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.AvgSell
                                    If ItemPrices.AvgSell <> 0 Then
                                        MarketPrice += ItemPrices.AvgSell
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.MedAll
                                    If ItemPrices.MedAll <> 0 Then
                                        MarketPrice += ItemPrices.MedAll
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.MedBuy
                                    If ItemPrices.MedBuy <> 0 Then
                                        MarketPrice += ItemPrices.MedBuy
                                        PriceCount += 1
                                    End If
                                Case Core.PriceGroupFlags.MedSell
                                    If ItemPrices.MedSell <> 0 Then
                                        MarketPrice += ItemPrices.MedSell
                                        PriceCount += 1
                                    End If

                            End Select

                        Next

                    End If

                End If

            Next

            ' Calculate an average of the prices if we need to
            If PriceCount > 0 Then
                MarketPrice = Math.Round(MarketPrice / PriceCount, 2)

             
            End If

            ' Add the price to the used list
            UsedPriceList.Add(ItemID, MarketPrice)

        Next

    End Sub
End Class
