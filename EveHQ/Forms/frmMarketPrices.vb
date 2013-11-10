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
Imports System.ComponentModel
Imports EveHQ.Core
Imports DevComponents.AdvTree
Imports EveHQ.EveData
Imports EveHQ.Market
Imports System.IO
Imports EveHQ.Common.Extensions
Imports System.Threading.Tasks

Namespace Forms

    Public Class FrmMarketPrices
        Dim _marketCacheFolder As String = ""
        Private Const MarketCacheFolderName As String = "MarketCache"
        Private Const Expired As String = "Expired!"

#Region "Form Opening and Closing Routines"

        Private Sub frmMarketPrices_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Initialize Price check item list
            If (ComboBox1.Items.Count = 0) Then
                ComboBox1.Items.AddRange((From item In StaticData.Types.Values Where item.MarketGroupId <> 0 Select item.Name).ToArray())
            End If

            ' Check for the market cache folder
            If My.Computer.FileSystem.DirectoryExists(Path.Combine(HQ.AppDataFolder, MarketCacheFolderName)) = False Then
                Try
                    _marketCacheFolder = Path.Combine(HQ.AppDataFolder, MarketCacheFolderName)
                    My.Computer.FileSystem.CreateDirectory(_marketCacheFolder)
                Catch ex As Exception
                    MessageBox.Show("An error occured while attempting to create the Market Cache folder: " & ex.Message, "Error Creating Market Cache Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End Try
            Else
                _marketCacheFolder = Path.Combine(HQ.AppDataFolder, MarketCacheFolderName)
            End If

            ' Update the Custom Price Grid
            Call UpdatePriceMatrix()
        End Sub

#End Region

#Region "Custom Prices Functions"
        Private Sub txtSearchPrices_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearchPrices.TextChanged
            If Len(txtSearchPrices.Text) > 2 Then
                Dim strSearch As String = txtSearchPrices.Text.Trim.ToLower
                Call UpdatePriceMatrix(strSearch)
            End If
        End Sub
        Private Sub btnResetGrid_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetGrid.Click
            txtSearchPrices.Text = ""
            Call UpdatePriceMatrix("")
        End Sub
        Private Sub UpdatePriceMatrix(Optional ByVal search As String = "")
            ' Loads prices into the listview
            adtPrices.BeginUpdate()
            adtPrices.Nodes.Clear()
            Dim lvItem As Node
            Dim itemID As Integer
            Dim itemData As New EveType
            Dim price As Double
            If chkShowOnlyCustom.Checked = True Then
                For Each itemID In HQ.CustomPriceList.Keys ' ID
                    itemData = StaticData.Types(itemID)
                    If itemData.Name.ToLower.Contains(search) = True Then
                        If itemData.Published = True Then
                            lvItem = New Node
                            lvItem.Text = itemData.Name
                            lvItem.Name = CStr(itemID)
                            lvItem.Cells.Add(New Cell(StaticData.Types(itemID).BasePrice.ToString("N2")))

                            price = DataFunctions.GetPrice(itemID)
                            lvItem.Cells.Add(New Cell(price.ToInvariantString("N2")))

                            If HQ.CustomPriceList.ContainsKey(itemID) Then
                                price = CDbl(HQ.CustomPriceList(itemID))
                                lvItem.Cells.Add(New Cell(price.ToInvariantString("N2")))
                            Else
                                lvItem.Cells.Add(New Cell(""))
                            End If
                            adtPrices.Nodes.Add(lvItem)
                        End If
                    End If
                Next
            Else
                Dim itemCells As New Dictionary(Of String, Cell)
                For Each item As String In StaticData.TypeNames.Keys
                    If item.ToLower.Contains(search) = True Then
                        itemID = StaticData.TypeNames(item)
                        If StaticData.Types.TryGetValue(itemID, itemData) = True Then
                            If itemData.Published = True Then
                                lvItem = New Node
                                lvItem.Text = itemData.Name
                                lvItem.Name = CStr(itemData.Id)
                                lvItem.Cells.Add(New Cell(StaticData.Types(itemID).BasePrice.ToString("N2")))

                                itemCells.Add(CStr(itemID), New Cell())
                                lvItem.Cells.Add(itemCells(CStr(itemID)))

                                If HQ.CustomPriceList.ContainsKey(itemID) Then
                                    price = CDbl(HQ.CustomPriceList(itemID))
                                    lvItem.Cells.Add(New Cell(price.ToString("N2")))
                                Else
                                    lvItem.Cells.Add(New Cell(""))
                                End If
                                adtPrices.Nodes.Add(lvItem)
                            End If
                        End If
                    End If
                Next

                '' get the prices for the items
                'Task.Factory.StartNew(Sub()
                '                          GetItemPrices(itemCells)
                '                      End Sub)


            End If
            AdvTreeSorter.Sort(adtPrices, 1, False, True)
            adtPrices.EndUpdate()
        End Sub

        Private Sub ctxPrices_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxPrices.Opening
            Select Case adtPrices.SelectedNodes.Count
                Case 0
                    e.Cancel = True
                Case 1
                    Dim selItem As Node = adtPrices.SelectedNodes(0)
                    mnuPriceItemName.Text = selItem.Text
                    Dim selItemID As String = selItem.Name
                    ' Check if it exists and we can edit/delete it
                    If HQ.CustomPriceList.ContainsKey(CInt(selItemID)) = True Then
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
                Case Is > 1
                    ' Add option to remove multiple prices
                    mnuPriceItemName.Text = "[Multiple Prices]"
                    mnuPriceAdd.Enabled = False
                    mnuPriceDelete.Enabled = True
                    mnuPriceEdit.Enabled = False
            End Select
        End Sub
        Private Sub mnuPriceDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuPriceDelete.Click
            For Each selitem As Node In adtPrices.SelectedNodes
                Dim selItemID As Integer = CInt(selitem.Name)
                If HQ.CustomPriceList.ContainsKey(selItemID) = True Then
                    ' Double check it exists and delete it
                    Call CustomDataFunctions.DeleteCustomPrice(selItemID)
                    ' refresh that asset rather than the whole list
                End If
                selitem.Cells(3).Text = ""
            Next
        End Sub
        Private Sub mnuPriceAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuPriceAdd.Click
            Dim selItem As Node = adtPrices.SelectedNodes(0)
            Dim itemID As Integer = CInt(selItem.Name)
            Using newPrice As New FrmModifyPrice(itemID, 0)
                newPrice.ShowDialog()
                If HQ.CustomPriceList.ContainsKey(CInt(itemID)) Then
                    selItem.Cells(3).Text = HQ.CustomPriceList(CInt(itemID)).ToString("N2")
                End If
            End Using
        End Sub
        Private Sub mnuPriceEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuPriceEdit.Click
            Dim selItem As Node = adtPrices.SelectedNodes(0)
            Dim itemID As Integer = CInt(selItem.Name)
            Using newPrice As New FrmModifyPrice(itemID, 0)
                newPrice.ShowDialog()
                If HQ.CustomPriceList.ContainsKey(CInt(itemID)) Then
                    selItem.Cells(3).Text = HQ.CustomPriceList(CInt(itemID)).ToString("N2")
                End If
            End Using
        End Sub
        Private Sub chkShowOnlyCustom_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkShowOnlyCustom.CheckedChanged
            If Len(txtSearchPrices.Text) > 2 Then
                Dim strSearch As String = txtSearchPrices.Text.Trim.ToLower
                Call UpdatePriceMatrix(strSearch)
            Else
                Call UpdatePriceMatrix()
            End If
        End Sub
        Private Sub adtPrices_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtPrices.ColumnHeaderMouseDown
            Dim ch As ColumnHeader = CType(sender, ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub
#End Region


        Private Sub OnGetMarketOrdersClick(sender As Object, e As EventArgs) Handles _getMarketOrders.Click
            ' check that an item is selected in the drop down
            If ComboBox1.SelectedItem Is Nothing Then
                MessageBox.Show("You must first select an item from the drop down list before retrieving market information.")
                Return
            End If

            'get the itemid needed for retrieval
            Dim itemId As Integer = (From items In StaticData.Types.Values Where items.Name = CStr(ComboBox1.SelectedItem) Select items.Id).FirstOrDefault()

            Dim orderTask As Task(Of ItemMarketOrders)
            Dim statstask As Task(Of IEnumerable(Of ItemOrderStats))
            Dim statItems As New List(Of Integer)
            statItems.Add(itemId)
            If (HQ.Settings.MarketUseRegionMarket) = True Then
                orderTask = HQ.MarketOrderDataProvider.GetMarketOrdersForItemType(itemId, HQ.Settings.MarketRegions, Nothing, 1)
                statstask = HQ.MarketStatDataProvider.GetOrderStats(statItems, HQ.Settings.MarketRegions, Nothing, 1)
            Else
                orderTask = HQ.MarketOrderDataProvider.GetMarketOrdersForItemType(itemId, Nothing, HQ.Settings.MarketSystem, 1)
                statstask = HQ.MarketStatDataProvider.GetOrderStats(statItems, Nothing, HQ.Settings.MarketSystem, 1)
            End If

            Dim orderContinuation As Action(Of Task(Of ItemMarketOrders)) = Sub(dataTask As Task(Of ItemMarketOrders))
                                                                                'Bug EVEHQ-169 : this is called even after the window is destroyed but not GC'd. check the handle boolean first.
                                                                                If IsHandleCreated Then
                                                                                    If dataTask.IsCanceled = False And dataTask.IsFaulted = False And dataTask.Exception Is Nothing And dataTask.Result IsNot Nothing Then
                                                                                        Invoke(Sub() UpdateMarketDisplayWithNewData(dataTask.Result))
                                                                                        'TODO: this is where display of an error message should go.
                                                                                    End If
                                                                                    'Return result
                                                                                End If
                                                                            End Sub

            Dim statsContinuation As Action(Of Task(Of IEnumerable(Of ItemOrderStats))) = Sub(dataTask As Task(Of IEnumerable(Of ItemOrderStats)))
                                                                                              'Bug EVEHQ-169 : this is called even after the window is destroyed but not GC'd. check the handle boolean first.
                                                                                              If IsHandleCreated Then
                                                                                                  If dataTask.IsCanceled = False And dataTask.IsFaulted = False And dataTask.Exception Is Nothing And dataTask.Result IsNot Nothing And dataTask.Result.Any() Then
                                                                                                      Invoke(Sub() UpdateItemOrderMetrics(dataTask.Result))
                                                                                                      'TODO: this is where display of an error message should go.
                                                                                                  End If
                                                                                              End If
                                                                                          End Sub


            orderTask.ContinueWith(orderContinuation)

            statstask.ContinueWith(statsContinuation)
        End Sub

        ' updates the market tables with new data
        Private Sub UpdateMarketDisplayWithNewData(ByVal data As ItemMarketOrders)
            _sellOrders.Nodes.Clear()
            _buyOrders.Nodes.Clear()

            For Each order As MarketOrder In data.SellOrders
                Dim row As New Node
                row.Text = order.StationName

                'quantity
                Dim quantityCell As New Cell()
                quantityCell.Text = order.QuantityRemaining.ToInvariantString("F0")
                quantityCell.TextDisplayFormat = "N0"
                row.Cells.Add(quantityCell)


                'Price 
                Dim priceCell As New Cell()
                priceCell.Text = order.Price.ToInvariantString("F2")
                priceCell.TextDisplayFormat = "N2"
                row.Cells.Add(priceCell)

                'format expiry to countdown timespan
                Dim diff As TimeSpan = order.Expires - DateTimeOffset.UtcNow
                Dim expiresLabel As String

                If (diff.TotalSeconds <= 0) Then
                    expiresLabel = Expired
                Else
                    expiresLabel = SkillFunctions.TimeToString(diff.TotalSeconds, False)
                End If

                row.Cells.Add(New Cell(expiresLabel))
                _sellOrders.Nodes.Add(row)
            Next
            For Each order As MarketOrder In data.BuyOrders
                Dim row As New Node

                row.Text = order.StationName

                'quantity
                Dim quantityCell As New Cell()
                quantityCell.Text = order.QuantityRemaining.ToInvariantString("F0")
                quantityCell.TextDisplayFormat = "N0"
                row.Cells.Add(quantityCell)


                'Price 
                Dim priceCell As New Cell()

                priceCell.Text = order.Price.ToInvariantString("F2")
                priceCell.TextDisplayFormat = "N2"
                row.Cells.Add(priceCell)

                'format expiry to countdown timespace

                Dim diff As TimeSpan = order.Expires - DateTimeOffset.UtcNow
                Dim expiresLabel As String

                If (diff.TotalSeconds <= 0) Then
                    expiresLabel = Expired
                Else
                    expiresLabel = SkillFunctions.TimeToString(diff.TotalSeconds, False)
                End If


                row.Cells.Add(New Cell(expiresLabel))
                _buyOrders.Nodes.Add(row)
            Next

        End Sub

        Private Sub UpdateItemOrderMetrics(stats As IEnumerable(Of ItemOrderStats))

            ' there should just be one item to get see stats for
            Dim itemStat As ItemOrderStats = stats.First()

            ' update the UI controls

            ' Sell Orders
            _sellOrderMetrics.Maximum = itemStat.Sell.Maximum.ToInvariantString("N2")
            _sellOrderMetrics.Minimum = itemStat.Sell.Minimum.ToInvariantString("N2")
            _sellOrderMetrics.Average = itemStat.Sell.Average.ToInvariantString("N2")
            _sellOrderMetrics.Median = itemStat.Sell.Median.ToInvariantString("N2")
            _sellOrderMetrics.StdDeviation = itemStat.Sell.StdDeviation.ToInvariantString("N2")
            _sellOrderMetrics.Percentile = itemStat.Sell.Percentile.ToInvariantString("N2")
            _sellOrderMetrics.Volume = itemStat.Sell.Volume.ToInvariantString()


            ' Buy Orders
            _buyOrderMetrics.Maximum = itemStat.Buy.Maximum.ToInvariantString("N2")
            _buyOrderMetrics.Minimum = itemStat.Buy.Minimum.ToInvariantString("N2")
            _buyOrderMetrics.Average = itemStat.Buy.Average.ToInvariantString("N2")
            _buyOrderMetrics.Median = itemStat.Buy.Median.ToInvariantString("N2")
            _buyOrderMetrics.StdDeviation = itemStat.Buy.StdDeviation.ToInvariantString("N2")
            _buyOrderMetrics.Percentile = itemStat.Buy.Percentile.ToInvariantString("N2")
            _buyOrderMetrics.Volume = itemStat.Buy.Volume.ToInvariantString()

        End Sub
        
        Private Sub OnMarketSettingsClick(sender As Object, e As EventArgs) Handles Button2.Click
            ShowMarketSettings()
        End Sub

        Private Sub ShowMarketSettings()
            Using eveHQSettings As New FrmSettings
                eveHQSettings.Tag = "nodeMarket"
                eveHQSettings.ShowDialog()
            End Using
        End Sub
    End Class
End NameSpace