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

Imports DevComponents.AdvTree
Imports System.Windows.Forms
Imports DevComponents.DotNetBar
Imports EveHQ.EveData
Imports EveHQ.EveAPI
Imports System.Xml
Imports System.Threading.Tasks
Imports EveHQ.Common.Extensions
Imports System.Text

Namespace Requisitions
    Public Class FrmRequisitions
        Dim _selectedReqName As String = ""
        Dim _ownedAssets As New SortedList(Of String, RequisitionAsset)
        ReadOnly _groupResources As New SortedList(Of String, Long)
        Dim _currentReqs As New SortedList(Of String, Requisition)
        Private Const IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        ReadOnly _exclusionFlags As New List(Of Integer)

        Private Sub frmRequisitions_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Call UpdateAssetOwners()
            Call UpdateFilters()
            _currentReqs = CustomDataFunctions.PopulateRequisitions("", "", "", "")
            Call ApplyFilter()
        End Sub

        Private Sub UpdateAssetOwners()
            cboAssetSelection.BeginUpdate()
            cboAssetSelection.Items.Clear()
            ' Add in pilots
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    If cPilot.Account <> "" Then
                        cboAssetSelection.Items.Add(cPilot.Name)
                    End If
                End If
            Next
            ' Add in corps
            For Each cCorp As Corporation In HQ.Settings.Corporations.Values
                If HQ.Settings.Accounts.ContainsKey(cCorp.Accounts(0)) Then
                    Dim cAccount As EveHQAccount = HQ.Settings.Accounts(cCorp.Accounts(0))
                    If cAccount.CanUseCorporateAPI(CorporateAccessMasks.AssetList) = True Then
                        cboAssetSelection.Items.Add(cCorp.Name)
                    End If
                End If
            Next
            cboAssetSelection.EndUpdate()
        End Sub

        Private Sub UpdateFilters()
            cboRequestor.BeginUpdate()
            cboRequisition.BeginUpdate()
            cboSource.BeginUpdate()

            cboRequestor.Items.Clear()
            cboRequisition.Items.Clear()
            cboSource.Items.Clear()

            Dim filterData As DataSet
            filterData = CustomDataFunctions.GetCustomData("SELECT DISTINCT requestor FROM requisitions;")
            If filterData IsNot Nothing Then
                For Each filterRow As DataRow In filterData.Tables(0).Rows
                    cboRequestor.Items.Add(filterRow.Item("requestor").ToString)
                Next
            End If

            filterData = CustomDataFunctions.GetCustomData("SELECT DISTINCT requisition FROM requisitions;")
            If filterData IsNot Nothing Then
                For Each filterRow As DataRow In filterData.Tables(0).Rows
                    cboRequisition.Items.Add(filterRow.Item("requisition").ToString)
                Next
            End If

            filterData = CustomDataFunctions.GetCustomData("SELECT DISTINCT source FROM requisitions;")
            If filterData IsNot Nothing Then
                For Each filterRow As DataRow In filterData.Tables(0).Rows
                    cboSource.Items.Add(filterRow.Item("source").ToString)
                Next
            End If

            cboRequestor.EndUpdate()
            cboRequisition.EndUpdate()
            cboSource.EndUpdate()

            filterData.Dispose()
        End Sub

        Private Sub btnAddReq_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddReq.Click
            ' Save the current selected Requisition
            Using myReq As New FrmEditRequisition
                myReq.ShowDialog()
                If myReq.DialogResult = DialogResult.OK Then
                    Call ApplyFilter()
                    Call UpdateFilters()
                End If
            End Using
        End Sub

        Private Sub btnEditReq_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditReq.Click
            Call EditRequisition()
        End Sub

        Private Sub EditRequisition()
            If adtReqs.SelectedNodes.Count > 1 Then
                MessageBox.Show(
                    "You cannot edit multiple Requisitions at the same time. Please ensure only one is selected before using this option.",
                    "Single Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                Dim selNode As Node = adtReqs.SelectedNode
                If selNode.Level = 1 Then
                    selNode = selNode.Parent
                End If
                Dim reqName As String = selNode.Name
                Dim req As Requisition = _currentReqs(reqName)
                Using myReq As New FrmEditRequisition(req)
                    myReq.ShowDialog()
                    If myReq.DialogResult = DialogResult.OK Then
                        Call ApplyFilter()
                        Call UpdateFilters()
                    End If
                End Using
            End If
        End Sub

        Private Sub btnApplyFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApplyFilter.Click
            Call ApplyFilter()
        End Sub

        Private Sub ApplyFilter()
            Dim requisition As String = ""
            Dim requestor As String = ""
            Dim source As String = ""
            Dim search As String = txtSearch.Text
            If cboRequisition.SelectedItem IsNot Nothing Then
                requisition = cboRequisition.SelectedItem.ToString
            End If
            If cboRequestor.SelectedItem IsNot Nothing Then
                requestor = cboRequestor.SelectedItem.ToString
            End If
            If cboSource.SelectedItem IsNot Nothing Then
                source = cboSource.SelectedItem.ToString
            End If
            _currentReqs = CustomDataFunctions.PopulateRequisitions(search, requisition, source, requestor)
            Call UpdateRequisitions()
        End Sub

        Private Sub btnResetFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetFilter.Click
            Call ResetFilter()
        End Sub

        Private Sub ResetFilter()
            txtSearch.Text = ""
            cboRequestor.SelectedIndex = -1
            cboRequisition.SelectedIndex = -1
            cboSource.SelectedIndex = -1
            Call ApplyFilter()
        End Sub

        Private Sub UpdateRequisitions()
            adtOrders.BeginUpdate()
            adtOrders.Nodes.Clear()
            adtOrders.EndUpdate()
            adtReqs.BeginUpdate()
            adtReqs.Nodes.Clear()
            For Each newReq As Requisition In _currentReqs.Values
                Dim reqNode As New Node
                reqNode.Text = newReq.Name & "<font color=""#BBBBBB""> (" & newReq.Requestor & " - " &
                               newReq.Orders.Count.ToString("N0") &
                               IIf(newReq.Orders.Count = 1, " item)", " items)").ToString & "</font>"
                reqNode.Name = newReq.Name
                For Each newOrder As RequisitionOrder In newReq.Orders.Values
                    If StaticData.Types.ContainsKey(CInt(newOrder.ItemID)) Then
                        Dim item As EveType = StaticData.Types(CInt(newOrder.ItemID))
                        Dim orderNode As New Node
                        orderNode.Text = newOrder.ItemName
                        orderNode.Name = newOrder.ItemName
                        orderNode.Image = ImageHandler.GetImage(item.Id, 20)
                        reqNode.Nodes.Add(orderNode)
                    End If
                Next
                adtReqs.Nodes.Add(reqNode)
                If _selectedReqName = newReq.Name Then
                    adtReqs.SelectedNode = reqNode
                End If
            Next
            adtReqs.EndUpdate()
        End Sub

        Private Sub adtReqs_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) _
            Handles adtReqs.NodeDoubleClick
            If e.Node.Level > 0 Then
                Call EditRequisition()
            End If
        End Sub

        Private Sub adtReqs_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles adtReqs.SelectionChanged
            Call UpdateOrderList()
        End Sub

        Private Sub UpdateOrderList()
            If adtReqs.SelectedNodes.Count > 0 Then
                Dim selNode As Node = adtReqs.SelectedNodes(0)
                If selNode IsNot Nothing Then
                    If selNode.Level = 0 Then
                        If adtReqs.SelectedNodes.Count = 1 Then
                            _selectedReqName = selNode.Name
                            Dim req As Requisition = _currentReqs(selNode.Name)
                            Call UpdateOrders(req)
                            ' Enable buttons
                            btnEditReq.Enabled = True
                            btnDeleteReq.Enabled = True
                            btnCopyReq.Enabled = True
                            btnMerge.Enabled = False
                            btnExportReq.Enabled = True
                        Else
                            Call ConsolidateRequisitionOrders()
                            ' Enable buttons
                            btnEditReq.Enabled = False
                            btnDeleteReq.Enabled = True
                            btnCopyReq.Enabled = False
                            btnMerge.Enabled = True
                            btnExportReq.Enabled = True
                        End If
                    Else
                        If selNode.Level = 1 Then
                            _selectedReqName = selNode.Parent.Name
                            Dim req As Requisition = _currentReqs(selNode.Parent.Name)
                            Call UpdateOrders(req)
                            ' Enable buttons
                            btnEditReq.Enabled = True
                            btnDeleteReq.Enabled = True
                            btnCopyReq.Enabled = True
                            btnMerge.Enabled = False
                            btnExportReq.Enabled = True
                        End If
                    End If
                End If
            Else
                adtOrders.BeginUpdate()
                adtOrders.Nodes.Clear()
                adtOrders.EndUpdate()
                ' Disable the buttons
                btnEditReq.Enabled = False
                btnDeleteReq.Enabled = False
                btnCopyReq.Enabled = False
                btnExportReq.Enabled = False
                btnMerge.Enabled = False
            End If
        End Sub

        Private Sub ConsolidateRequisitionOrders()
            ' Create a dummy requisition to handle mutliple selections
            Dim newReq As New Requisition
            newReq.Name = "<Mulitple>"
            For Each selNode As Node In adtReqs.SelectedNodes
                Dim req As Requisition = _currentReqs(selNode.Name)
                For Each order As RequisitionOrder In req.Orders.Values
                    If newReq.Orders.ContainsKey(order.ItemName) = False Then
                        newReq.Orders.Add(order.ItemName, CType(order.Clone, RequisitionOrder))
                    Else
                        newReq.Orders(order.ItemName).ItemQuantity += order.ItemQuantity
                    End If
                Next
            Next
            Call UpdateOrders(newReq)
        End Sub

        Private Sub UpdateOrders(ByVal req As Requisition)
            If cboAssetSelection.SelectedItem IsNot Nothing Then
                Call GetOwnedResources()
            End If
            Dim totalItems As Long = 0
            Dim totalItemsReqd As Long = 0
            Dim totalVolume As Double = 0
            Dim totalVolumeReqd As Double = 0
            Dim totalCost As Double = 0
            Dim totalCostReqd As Double = 0
            ' Set style
            Dim numberStyle As New ElementStyle
            numberStyle.TextAlignment = eStyleTextAlignment.Far
            adtOrders.BeginUpdate()
            adtOrders.Nodes.Clear()
            Dim priceTask As Task(Of Dictionary(Of Integer, Double)) =
                    DataFunctions.GetMarketPrices(From o In req.Orders.Values Select CInt(o.ItemID))
            For Each order As RequisitionOrder In req.Orders.Values
                Dim orderOwned As Long
                Dim item As EveType = StaticData.Types(CInt(order.ItemID))
                Dim unitCost As Double
                Dim orderNode As New Node
                orderNode.Text = order.ItemName
                orderNode.Name = order.ItemName
                orderNode.Image = ImageHandler.GetImage(item.Id, 20)
                ' Add Quantity cell
                Dim qCell As New Cell(order.ItemQuantity.ToString("N0"))
                qCell.StyleNormal = numberStyle
                orderNode.Cells.Add(qCell)
                ' Add MetaLevel cell
                Dim mlCell As New Cell(item.MetaLevel.ToString("N0"))
                mlCell.StyleNormal = numberStyle
                orderNode.Cells.Add(mlCell)
                ' Add Volume cell
                Dim vCell As New Cell((item.Volume * order.ItemQuantity).ToString("N2"))
                vCell.StyleNormal = numberStyle
                orderNode.Cells.Add(vCell)
                Dim ucCell As New Cell("Processing...")
                ucCell.TextDisplayFormat = "N2"
                ucCell.StyleNormal = numberStyle
                orderNode.Cells.Add(ucCell)
                ' Add Total Cost cell
                Dim tcCell As New Cell("Processing...")
                tcCell.TextDisplayFormat = "N2"
                tcCell.StyleNormal = numberStyle
                orderNode.Cells.Add(tcCell)
                ' Add Owned cell
                Dim oCell As New Cell
                oCell.StyleNormal = numberStyle
                If _ownedAssets.ContainsKey(order.ItemName) Then
                    orderOwned = _ownedAssets(order.ItemName).TotalQuantity
                    For Each locID As String In _ownedAssets(order.ItemName).Locations.Keys
                        Dim locNode As New Node
                        locNode.Text = StaticData.GetLocationName(CInt(locID))
                        locNode.Cells.Add(New Cell(_ownedAssets(order.ItemName).Locations(locID).ToString("N0")))
                        locNode.Cells(1).StyleNormal = numberStyle
                        orderNode.Nodes.Add(locNode)
                    Next
                Else
                    orderOwned = 0
                End If
                oCell.Text = orderOwned.ToString("N0")
                orderNode.Cells.Add(oCell)
                ' Add Surplus cell
                Dim sCell As New Cell((orderOwned - order.ItemQuantity).ToString("N0"))
                sCell.StyleNormal = numberStyle
                orderNode.Cells.Add(sCell)
                ' Add Surplus Cost cell
                Dim scCell As New Cell((unitCost * (Math.Max(order.ItemQuantity - orderOwned, 0))).ToString("N2"))
                scCell.StyleNormal = numberStyle
                orderNode.Cells.Add(scCell)
                ' Add Node to the list
                adtOrders.Nodes.Add(orderNode)
                totalItems += order.ItemQuantity
                totalItemsReqd += Math.Max(order.ItemQuantity - orderOwned, 0)
                totalVolume += item.Volume * order.ItemQuantity
                totalVolumeReqd += (item.Volume * (Math.Max(order.ItemQuantity - orderOwned, 0)))
                totalCost += (unitCost * order.ItemQuantity)
                totalCostReqd += (unitCost * (Math.Max(order.ItemQuantity - orderOwned, 0)))
            Next
            adtOrders.EndUpdate()
            ' Update summary information
            lblSummary.Text = "Summary - " & req.Name
            lblUniqueItems.Text = req.Orders.Count.ToString("N0")
            lblTotalVolume.Text = "Total: " & totalVolume.ToString("N2") & " m³" & ControlChars.CrLf & "Reqd: " &
                                  totalVolumeReqd.ToString("N2") & " m³"

            ' Update Pricing from task
            priceTask.ContinueWith(Sub(currentTask As Task(Of Dictionary(Of Integer, Double)))
                                       If IsHandleCreated Then
                                           Dim priceData As Dictionary(Of Integer, Double) = currentTask.Result
                                           ' cut over to main thread
                                           Invoke(Sub()
                                                      For Each row As Node In adtOrders.Nodes
                                                          Dim price As Double
                                                          Dim quantity As Long
                                                          If (priceData.TryGetValue(StaticData.TypeNames(row.Name), price)) Then
                                                              row.Cells(4).Text = price.ToInvariantString("F2")
                                                              Long.TryParse(row.Cells(1).Text, quantity)
                                                              row.Cells(5).Text = (price * quantity).ToInvariantString("F2")

                                                          End If
                                                          Dim asset As New RequisitionAsset
                                                          Dim owned As Long = 0
                                                          If (_ownedAssets.TryGetValue(row.Text, asset)) Then
                                                              owned = asset.TotalQuantity
                                                          End If
                                                          totalCost += (price * quantity)
                                                          totalCostReqd += (price * (Math.Max(quantity - owned, 0)))
                                                      Next
                                                      lblTotalItems.Text = totalItems.ToString("N0") & " (" & totalItemsReqd.ToString("N0") & ")"
                                                      lblTotalCost.Text = "Total: " & totalCost.ToString("N2") & " ISK" & ControlChars.CrLf & "Reqd: " &
                                                                          totalCostReqd.ToString("N2") & " ISK"
                                                      lblTotalVolume.Text = "Total: " & totalVolume.ToString("N2") & " m³" & ControlChars.CrLf &
                                                                            "Reqd: " & totalVolumeReqd.ToString("N2") & " m³"
                                                  End Sub)
                                       End If
                                   End Sub)
        End Sub

        Private Sub btnDeleteReq_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteReq.Click
            Call DeleteRequisitions()
        End Sub

        Private Sub DeleteRequisitions()
            ' Check parent nodes
            Dim parentNodeCount As Integer = 0
            For Each selNode As Node In adtReqs.SelectedNodes
                If selNode.Level = 0 Then
                    parentNodeCount += 1
                End If
            Next
            If parentNodeCount = 0 Then
                MessageBox.Show(
                    "At least one Requisition heading must be selected before deleting. Please make a selection before using this option.",
                    "Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to delete the selected Requisitions?",
                                                            "Confirm Delete", MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Question)
                If reply = DialogResult.No Then
                    Exit Sub
                Else
                    For Each selNode As Node In adtReqs.SelectedNodes
                        If selNode.Level = 0 Then
                            Dim reqName As String = selNode.Name
                            Dim req As Requisition = _currentReqs(reqName)
                            If req.DeleteFromDatabase = True Then
                            End If
                        End If
                    Next
                    Call ApplyFilter()
                    Call UpdateFilters()
                End If
            End If
        End Sub

        Private Sub btnCopyReq_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopyReq.Click
            ' Check parent nodes
            Dim parentNodeCount As Integer = 0
            For Each selNode As Node In adtReqs.SelectedNodes
                If selNode.Level = 0 Then
                    parentNodeCount += 1
                End If
            Next
            If parentNodeCount = 0 Then
                MessageBox.Show(
                    "At least one Requisition heading must be selected before copying. Please make a selection before using this option.",
                    "Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                ' Clone the selected requisition
                Dim selNode As Node = adtReqs.SelectedNodes(0)
                Dim oldReq As Requisition = _currentReqs(selNode.Name)
                Dim newReq As Requisition = CType(oldReq.Clone, Requisition)
                Dim copy As Integer = 0
                Do
                    copy += 1
                Loop Until _currentReqs.ContainsKey(newReq.Name & " (Copy " & copy.ToString & ")") = False
                newReq.Name &= " (Copy " & copy.ToString & ")"
                ' Save the current selected Requisition
                newReq.WriteToDatabase()
                Call ApplyFilter()
                Call UpdateFilters()
            End If
        End Sub

        Private Sub btnMerge_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMerge.Click
            ' Check parent nodes
            Dim parentNodeCount As Integer = 0
            For Each selNode As Node In adtReqs.SelectedNodes
                If selNode.Level = 0 Then
                    parentNodeCount += 1
                End If
            Next
            If parentNodeCount < 2 Then
                MessageBox.Show(
                    "At least two Requisition headings must be selected before merging. Please make the appropriate selection before using this option.",
                    "Mulitple Requisitions Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                Dim reqList As New SortedList(Of String, Requisition)
                For Each reqNode As Node In adtReqs.SelectedNodes
                    reqList.Add(reqNode.Name, _currentReqs(reqNode.Name))
                Next

                ' Create a new merge form and wait for a result
                Using newMergeForm As New FrmMergeRequisitions(reqList)
                    newMergeForm.ShowDialog()
                End Using

                ' DB handling is done in the merge form so just update the filters
                Call ApplyFilter()
                Call UpdateFilters()
            End If
        End Sub

        Private Sub adtOrders_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) _
            Handles adtOrders.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub

        Private Sub adtOrders_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) _
            Handles adtOrders.NodeDoubleClick
            Call EditRequisition()
        End Sub

        Private Sub GetOwnedResources()

            ' Get the primary requestor
            If cboAssetSelection.SelectedItem IsNot Nothing Then

                Dim assetOwner As String = cboAssetSelection.SelectedItem.ToString
                Dim assetAccount As EveHQAccount
                Dim ownerID As String
                Dim isCorp As Boolean

                ' Check whether a pilot or a corp
                If HQ.Settings.Pilots.ContainsKey(assetOwner) = True Then
                    assetAccount = HQ.Settings.Accounts(HQ.Settings.Pilots(assetOwner).Account)
                    ownerID = HQ.Settings.Pilots(assetOwner).ID
                    isCorp = False
                Else
                    assetAccount = HQ.Settings.Accounts(HQ.Settings.Corporations(assetOwner).Accounts(0))
                    ownerID = CStr(HQ.Settings.Corporations(assetOwner).ID)
                    isCorp = True
                End If

                ' Clear the current owned resources list
                _ownedAssets.Clear()

                ' Fetch the resources owned
                ' Parse the Assets XML
                Dim assests As EveServiceResponse(Of IEnumerable(Of AssetItem))
                If isCorp = True Then
                    assests = HQ.ApiProvider.Corporation.AssetList(assetAccount.UserID, assetAccount.APIKey,
                                                                   Integer.Parse(ownerID))
                Else
                    assests = HQ.ApiProvider.Character.AssetList(assetAccount.UserID, assetAccount.APIKey,
                                                                 Integer.Parse(ownerID))
                End If
                If assests.ResultData IsNot Nothing Then
                    If assests.ResultData.Any() Then
                        ' Define what we want to obtain
                        Dim categories, groups As New ArrayList
                        For Each item As AssetItem In assests.ResultData
                            Call GetAssetQuantitesFromNode(item, "", categories, groups, _ownedAssets)
                        Next
                    End If
                End If
            End If
        End Sub

        Private Sub GetAssetQuantitesFromNode(ByVal item As AssetItem, ByVal locationID As String,
                                              ByVal categories As ArrayList, ByVal groups As ArrayList,
                                              ByRef Assets As SortedList(Of String, RequisitionAsset))
            Dim itemData As EveType
            Dim itemID As String

            itemID = item.TypeId.ToInvariantString()
            ' Check if the flag is excluded
            If _exclusionFlags.Contains(item.Flag) = False Then
                If item.LocationId > 0 Then
                    locationID = item.LocationId.ToInvariantString
                End If
                If StaticData.Types.ContainsKey(CInt(itemID)) Then
                    itemData = StaticData.Types(CInt(itemID))
                    If categories.Count > 0 Or groups.Count > 0 Then
                        ' Check if this is an excluded ship first
                        If _
                            chkAssembledShips.Checked = False Or
                            Not (chkAssembledShips.Checked = True And itemData.Category = 6 And item.Singleton = True) _
                            Then
                            If _
                                categories.Contains(itemData.Category) Or groups.Contains(itemData.Group) Or
                                _groupResources.ContainsKey(CStr(itemData.Id)) Then
                                ' Check if the item is in the list
                                If Assets.ContainsKey(CStr(itemData.Name)) = False Then
                                    Assets.Add(CStr(itemData.Name), New RequisitionAsset(locationID, item.Quantity))
                                Else
                                    Assets(CStr(itemData.Name)).AddAsset(locationID, item.Quantity)
                                End If
                            End If
                        End If
                    Else
                        ' Check if this is an excluded ship first
                        If _
                            chkAssembledShips.Checked = False Or
                            Not (chkAssembledShips.Checked = True And itemData.Category = 6 And item.Singleton = True) _
                            Then
                            ' Check if the item is in the list
                            If Assets.ContainsKey(CStr(itemData.Name)) = False Then
                                Assets.Add(CStr(itemData.Name), New RequisitionAsset(locationID, item.Quantity))
                            Else
                                Assets(CStr(itemData.Name)).AddAsset(locationID, item.Quantity)
                            End If
                        End If
                    End If
                End If
                ' Check child items if they exist
                If item.Contents IsNot Nothing Then
                    If item.Contents.Count > 0 Then
                        For Each subitem As AssetItem In item.Contents
                            Call GetAssetQuantitesFromNode(subitem, locationID, categories, groups, Assets)
                        Next
                    End If
                End If
            End If
        End Sub

        Private Sub cboAssetSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboAssetSelection.SelectedIndexChanged
            Call UpdateOrderList()
        End Sub

        Private Sub btnExportTSV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportTSV.Click
            Call ExportReq(ControlChars.Tab)
        End Sub

        Private Sub btnExportCSV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportCSV.Click
            Call ExportReq(HQ.Settings.CsvSeparatorChar)
        End Sub

        Private Sub ExportReq(ByVal sepChar As String)
            ' Check parent nodes
            Dim parentNodeCount As Integer = 0
            For Each selNode As Node In adtReqs.SelectedNodes
                If selNode.Level = 0 Then
                    parentNodeCount += 1
                End If
            Next
            If parentNodeCount = 0 Then
                MessageBox.Show(
                    "At least one Requisition heading must be selected before exporting. Please make a selection before using this option.",
                    "Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                ' Export the selected requisition
                Dim selNode As Node = adtReqs.SelectedNodes(0)
                Dim req As Requisition = _currentReqs(selNode.Name)
                Dim str As New StringBuilder
                str.AppendLine("EveHQ Requisition - " & req.Name)
                str.AppendLine("Requestor:" & sepChar & req.Requestor)
                str.AppendLine("Source: " & sepChar & req.Source)
                str.AppendLine()
                str.AppendLine("ItemName" & sepChar & "Date" & sepChar & "Quantity")
                For Each order As RequisitionOrder In req.Orders.Values
                    str.Append(order.ItemName & sepChar)
                    str.Append(order.RequestDate.ToString & sepChar)
                    str.AppendLine(order.ItemQuantity.ToString("N2"))
                Next
                Try
                    Clipboard.SetText(str.ToString)
                Catch ex As Exception
                    MessageBox.Show("There was an error exporting the data to the clipboard", "Export Requisition Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub btnExportEveHQ_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportEveHQ.Click
            ' Routine to export a requisition to a format that can be imported into EveHQ
            ' Check parent nodes
            Dim parentNodeCount As Integer = 0
            For Each selNode As Node In adtReqs.SelectedNodes
                If selNode.Level = 0 Then
                    parentNodeCount += 1
                End If
            Next
            If parentNodeCount = 0 Then
                MessageBox.Show(
                    "At least one Requisition heading must be selected before exporting. Please make a selection before using this option.",
                    "Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                ' Create XML Document
                Dim reqXML As New XmlDocument
                ' Create XML Declaration
                Dim dec As XmlDeclaration = reqXML.CreateXmlDeclaration("1.0", Nothing, Nothing)
                reqXML.AppendChild(dec)
                ' Create root
                Dim reqRoot As XmlElement = reqXML.CreateElement("EveHQRequisitions")
                reqXML.AppendChild(reqRoot)
                ' Get the requisition
                Dim selNode As Node = adtReqs.SelectedNodes(0)
                Dim req As Requisition = _currentReqs(selNode.Name)
                ' Generate the specific XML
                Call GenerateXMLForRequisition(req, reqXML, reqRoot)
                ' Get a file name
                Dim sfd1 As New SaveFileDialog
                With sfd1
                    .Title = "Save as EveHQ Requisition Export File..."
                    .FileName = ""
                    .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                    .Filter = "EveHQ Requisition Export Files (*.xml)|*.xml|All Files (*.*)|*.*"
                    .FilterIndex = 1
                    .RestoreDirectory = True
                    If .ShowDialog() = DialogResult.OK Then
                        If .FileName <> "" Then
                            reqXML.Save(.FileName)
                        End If
                    End If
                End With
            End If
        End Sub

        Private Sub GenerateXMLForRequisition(ByVal req As Requisition, ByRef reqXML As XmlDocument,
                                              ByRef reqRoot As XmlElement)
            Dim reqAtt As XmlAttribute

            ' Create various elements
            Dim reqReq As XmlElement = reqXML.CreateElement("requisition")
            reqRoot.AppendChild(reqReq)

            reqAtt = reqXML.CreateAttribute("name")
            reqAtt.Value = req.Name
            reqReq.Attributes.Append(reqAtt)

            reqAtt = reqXML.CreateAttribute("requestor")
            reqAtt.Value = req.Requestor
            reqReq.Attributes.Append(reqAtt)

            reqAtt = reqXML.CreateAttribute("source")
            reqAtt.Value = req.Source
            reqReq.Attributes.Append(reqAtt)

            Dim reqOrders As XmlElement = reqXML.CreateElement("orders")
            reqReq.AppendChild(reqOrders)

            ' Create Orders
            For Each order As RequisitionOrder In req.Orders.Values

                Dim reqOrder As XmlNode = reqXML.CreateElement("order")

                reqAtt = reqXML.CreateAttribute("itemID")
                reqAtt.Value = order.ItemID
                reqOrder.Attributes.Append(reqAtt)

                reqAtt = reqXML.CreateAttribute("itemName")
                reqAtt.Value = order.ItemName
                reqOrder.Attributes.Append(reqAtt)

                reqAtt = reqXML.CreateAttribute("itemQuantity")
                reqAtt.Value = order.ItemQuantity.ToString
                reqOrder.Attributes.Append(reqAtt)

                reqAtt = reqXML.CreateAttribute("requestDate")
                reqAtt.Value = order.RequestDate.ToString(IndustryTimeFormat)
                reqOrder.Attributes.Append(reqAtt)

                reqAtt = reqXML.CreateAttribute("source")
                reqAtt.Value = order.Source
                reqOrder.Attributes.Append(reqAtt)

                reqOrders.AppendChild(reqOrder)
            Next
        End Sub

        Private Sub btnImportEveHQ_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImportEveHQ.Click
            ' Create a new file dialog
            Dim ofd1 As New OpenFileDialog
            With ofd1
                .Title = "Select EveHQ Requisition Export File..."
                .FileName = ""
                .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                .Filter = "EveHQ Requisition Export Files (*.xml)|*.xml|All Files (*.*)|*.*"
                .FilterIndex = 1
                .RestoreDirectory = True
                If .ShowDialog() = DialogResult.OK Then
                    If My.Computer.FileSystem.FileExists(.FileName) = False Then
                        MessageBox.Show("Specified file does not exist. Please try again.", "Error Finding File",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    Else
                        ' Open the file for reading
                        Dim reqXML As New XmlDocument
                        Try
                            reqXML.Load(.FileName)
                        Catch ex As Exception
                            MessageBox.Show(
                                "Unable to read file data. Please check the file is not corrupted and you have permission to access this file",
                                "File Access Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End Try
                        Dim reqList As XmlNodeList = reqXML.SelectNodes("/EveHQRequisitions/requisition")
                        If reqList.Count > 0 Then
                            For Each req As XmlNode In reqList
                                Dim orders As SortedList(Of String, Integer) = GetOrdersFromRequisitionXML(req)
                                Using _
                                    newReq As _
                                        New FrmAddRequisition(req.Attributes.GetNamedItem("name").Value,
                                                              req.Attributes.GetNamedItem("source").Value, orders)
                                    newReq.ShowDialog()
                                End Using
                            Next
                        Else
                            MessageBox.Show(
                                "This file does not contain any valid EveHQ Requisitions. Import Process aborted.",
                                "No Requisitions Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
            End With
        End Sub

        Private Function GetOrdersFromRequisitionXML(ByVal req As XmlNode) As SortedList(Of String, Integer)
            Dim orders As New SortedList(Of String, Integer)
            Dim orderList As XmlNodeList = req.SelectNodes("orders/order")
            For Each order As XmlNode In orderList
                orders.Add(order.Attributes.GetNamedItem("itemName").Value,
                           CInt(order.Attributes.GetNamedItem("itemQuantity").Value))
            Next
            Return orders
        End Function

        Private Sub chkFittedModules_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles chkFittedModules.CheckedChanged
            If chkFittedModules.Checked = True Then
                If _exclusionFlags.Contains(11) = False Then
                    For flag As Integer = 11 To 34
                        _exclusionFlags.Add(flag)
                    Next
                    For flag As Integer = 92 To 99
                        _exclusionFlags.Add(flag)
                    Next
                    For flag As Integer = 125 To 132
                        _exclusionFlags.Add(flag)
                    Next
                End If
            Else
                If _exclusionFlags.Contains(11) = True Then
                    For flag As Integer = 11 To 34
                        _exclusionFlags.Remove(flag)
                    Next
                    For flag As Integer = 92 To 99
                        _exclusionFlags.Remove(flag)
                    Next
                    For flag As Integer = 125 To 132
                        _exclusionFlags.Remove(flag)
                    Next
                End If
            End If
            Call UpdateOrderList()
        End Sub

        Private Sub chkCargoBay_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles chkCargoBay.CheckedChanged
            If chkCargoBay.Checked = True Then
                If _exclusionFlags.Contains(5) = False Then
                    _exclusionFlags.Add(5)
                End If
            Else
                If _exclusionFlags.Contains(5) = True Then
                    _exclusionFlags.Remove(5)
                End If
            End If
            Call UpdateOrderList()
        End Sub

        Private Sub chkDroneBay_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles chkDroneBay.CheckedChanged
            If chkDroneBay.Checked = True Then
                If _exclusionFlags.Contains(87) = False Then
                    _exclusionFlags.Add(87)
                End If
            Else
                If _exclusionFlags.Contains(87) = True Then
                    _exclusionFlags.Remove(87)
                End If
            End If
            Call UpdateOrderList()
        End Sub

        Private Sub chkAssembledShips_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles chkAssembledShips.CheckedChanged
            Call UpdateOrderList()
        End Sub
    End Class
End Namespace