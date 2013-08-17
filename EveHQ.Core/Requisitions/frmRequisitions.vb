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
Imports System.Windows.Forms
Imports System.Xml
Imports System.Text
Imports System.Threading.Tasks
Imports DevComponents.AdvTree
Imports EveHQ.Market
Imports EveHQ.Common.Extensions

Public Class frmRequisitions

    Dim SelectedReqName As String = ""
    Dim LastOwner As String = ""
    Dim ownedAssets As New SortedList(Of String, EveHQ.Core.RequisitionAsset)
    Dim groupResources As New SortedList(Of String, Long)
	Dim CurrentReqs As New SortedList(Of String, EveHQ.Core.Requisition)
	Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Dim ExclusionFlags As New List(Of Integer)

    Private Sub frmRequisitions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Check for the Requisitions table
        If EveHQ.Core.RequisitionDataFunctions.CheckForRequisitionsTable() = True Then
            Call Me.UpdateAssetOwners()
            Call Me.UpdateFilters()
			CurrentReqs = EveHQ.Core.RequisitionDataFunctions.PopulateRequisitions("", "", "", "")
			Call Me.ApplyFilter()
        End If
    End Sub

    Private Sub UpdateAssetOwners()
        cboAssetSelection.BeginUpdate()
        cboAssetSelection.Items.Clear()
        ' Add in pilots
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHqSettings.Pilots
            If cPilot.Active = True Then
                If cPilot.Account <> "" Then
                    cboAssetSelection.Items.Add(cPilot.Name)
                End If
            End If
        Next
        ' Add in corps
        For Each cCorp As EveHQ.Core.Corporation In EveHQ.Core.HQ.EveHqSettings.Corporations.Values
            If EveHQ.Core.HQ.EveHqSettings.Accounts.Contains(cCorp.Accounts(0)) Then
                Dim cAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHqSettings.Accounts(cCorp.Accounts(0)), EveAccount)
                If cAccount.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.AssetList) = True Then
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

        Dim filterData As New DataSet
        filterData = EveHQ.Core.DataFunctions.GetCustomData("SELECT DISTINCT requestor FROM requisitions;")
        If filterData IsNot Nothing Then
            For Each filterRow As DataRow In filterData.Tables(0).Rows
                cboRequestor.Items.Add(filterRow.Item("requestor").ToString)
            Next
        End If

        filterData = EveHQ.Core.DataFunctions.GetCustomData("SELECT DISTINCT requisition FROM requisitions;")
        If filterData IsNot Nothing Then
            For Each filterRow As DataRow In filterData.Tables(0).Rows
                cboRequisition.Items.Add(filterRow.Item("requisition").ToString)
            Next
        End If

        filterData = EveHQ.Core.DataFunctions.GetCustomData("SELECT DISTINCT source FROM requisitions;")
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

    Private Sub btnAddReq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddReq.Click
        ' Save the current selected Requisition
        Dim myReq As New EveHQ.Core.frmEditRequisition
        myReq.ShowDialog()
        If myReq.DialogResult = Windows.Forms.DialogResult.OK Then
            Call Me.ApplyFilter()
            Call Me.UpdateFilters()
        End If
        myReq.Dispose()
    End Sub

    Private Sub btnEditReq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditReq.Click
        Call Me.EditRequisition()
    End Sub

    Private Sub EditRequisition()
        If adtReqs.SelectedNodes.Count > 1 Then
            MessageBox.Show("You cannot edit multiple Requisitions at the same time. Please ensure only one is selected before using this option.", "Single Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            Dim selNode As DevComponents.AdvTree.Node = adtReqs.SelectedNode
            If selNode.Level = 1 Then
                selNode = selNode.Parent
            End If
            Dim reqName As String = selNode.Name
            Dim Req As Requisition = CurrentReqs(reqName)
            Dim myReq As New EveHQ.Core.frmEditRequisition(Req)
            myReq.ShowDialog()
            If myReq.DialogResult = Windows.Forms.DialogResult.OK Then
                Call Me.ApplyFilter()
                Call Me.UpdateFilters()
            End If
            myReq.Dispose()
        End If
    End Sub

    Private Sub btnApplyFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplyFilter.Click
        Call Me.ApplyFilter()
    End Sub

    Private Sub ApplyFilter()
        Dim Requisition As String = ""
        Dim Requestor As String = ""
        Dim Source As String = ""
        Dim Search As String = txtSearch.Text
        If cboRequisition.SelectedItem IsNot Nothing Then
            Requisition = cboRequisition.SelectedItem.ToString
        End If
        If cboRequestor.SelectedItem IsNot Nothing Then
            Requestor = cboRequestor.SelectedItem.ToString
        End If
        If cboSource.SelectedItem IsNot Nothing Then
            Source = cboSource.SelectedItem.ToString
        End If
        CurrentReqs = EveHQ.Core.RequisitionDataFunctions.PopulateRequisitions(Search, Requisition, Source, Requestor)
        Call Me.UpdateRequisitions()
    End Sub

    Private Sub btnResetFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetFilter.Click
        Call Me.ResetFilter()
    End Sub

    Private Sub ResetFilter()
        txtSearch.Text = ""
        cboRequestor.SelectedIndex = -1
        cboRequisition.SelectedIndex = -1
        cboSource.SelectedIndex = -1
        Call Me.ApplyFilter()
    End Sub

    Private Sub UpdateRequisitions()
        adtOrders.BeginUpdate()
        adtOrders.Nodes.Clear()
        adtOrders.EndUpdate()
        adtReqs.BeginUpdate()
        adtReqs.Nodes.Clear()
        For Each newReq As EveHQ.Core.Requisition In CurrentReqs.Values
            Dim reqNode As New DevComponents.AdvTree.Node
            reqNode.Text = newReq.Name & "<font color=""#BBBBBB""> (" & newReq.Requestor & " - " & newReq.Orders.Count.ToString("N0") & IIf(newReq.Orders.Count = 1, " item)", " items)").ToString & "</font>"
            reqNode.Name = newReq.Name
            For Each newOrder As EveHQ.Core.RequisitionOrder In newReq.Orders.Values
                Dim item As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(newOrder.ItemID)
                Dim orderNode As New DevComponents.AdvTree.Node
                orderNode.Text = newOrder.ItemName
                orderNode.Name = newOrder.ItemName
                orderNode.Image = EveHQ.Core.ImageHandler.GetImage(item.ID.ToString, 20)
                reqNode.Nodes.Add(orderNode)
            Next
            adtReqs.Nodes.Add(reqNode)
            If SelectedReqName = newReq.Name Then
                adtReqs.SelectedNode = reqNode
            End If
        Next
        adtReqs.EndUpdate()
    End Sub

    Private Sub adtReqs_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtReqs.NodeDoubleClick
        If e.Node.Level > 0 Then
            Call Me.EditRequisition()
        End If
    End Sub

    Private Sub adtReqs_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtReqs.SelectionChanged
        Call Me.UpdateOrderList()
    End Sub

    Private Sub UpdateOrderList()
        If adtReqs.SelectedNodes.Count > 0 Then
            Dim selNode As DevComponents.AdvTree.Node = adtReqs.SelectedNodes(0)
            If selNode IsNot Nothing Then
                If selNode.Level = 0 Then
                    If adtReqs.SelectedNodes.Count = 1 Then
                        SelectedReqName = selNode.Name
                        Dim Req As Requisition = CurrentReqs(selNode.Name)
                        Call Me.UpdateOrders(Req)
                        ' Enable buttons
                        btnEditReq.Enabled = True
                        btnDeleteReq.Enabled = True
                        btnCopyReq.Enabled = True
                        btnMerge.Enabled = False
                        btnExportReq.Enabled = True
                    Else
                        Call Me.ConsolidateRequisitionOrders()
                        ' Enable buttons
                        btnEditReq.Enabled = False
                        btnDeleteReq.Enabled = True
                        btnCopyReq.Enabled = False
                        btnMerge.Enabled = True
                        btnExportReq.Enabled = True
                    End If
                Else
                    If selNode.Level = 1 Then
                        SelectedReqName = selNode.Parent.Name
                        Dim Req As Requisition = CurrentReqs(selNode.Parent.Name)
                        Call Me.UpdateOrders(Req)
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
        Dim NewReq As New Requisition
        NewReq.Name = "<Mulitple>"
        For Each selNode As DevComponents.AdvTree.Node In adtReqs.SelectedNodes
            Dim Req As Requisition = CurrentReqs(selNode.Name)
            For Each order As RequisitionOrder In Req.Orders.Values
                If NewReq.Orders.ContainsKey(order.ItemName) = False Then
                    NewReq.Orders.Add(order.ItemName, CType(order.Clone, RequisitionOrder))
                Else
                    NewReq.Orders(order.ItemName).ItemQuantity += order.ItemQuantity
                End If
            Next
        Next
        Call Me.UpdateOrders(NewReq)
    End Sub

    Private Sub UpdateOrders(ByVal Req As Requisition)
        If cboAssetSelection.SelectedItem IsNot Nothing Then
            Call Me.GetOwnedResources()
        End If
        Dim TotalItems As Long = 0 : Dim TotalItemsReqd As Long = 0
        Dim TotalVolume As Double = 0 : Dim TotalVolumeReqd As Double = 0
        Dim TotalCost As Double = 0 : Dim TotalCostReqd As Double = 0
        ' Set style
        Dim NumberStyle As New DevComponents.DotNetBar.ElementStyle
        NumberStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far
        adtOrders.BeginUpdate()
        adtOrders.Nodes.Clear()
        Dim priceTask As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(From o In Req.Orders.Values Select o.ItemID)
        For Each order As RequisitionOrder In Req.Orders.Values
            Dim OrderOwned As Long = 0
            Dim item As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(order.ItemID)
            Dim UnitCost As Double = 0
            Dim orderNode As New DevComponents.AdvTree.Node
            orderNode.Text = order.ItemName
            orderNode.Name = order.ItemID
            orderNode.Image = EveHQ.Core.ImageHandler.GetImage(item.ID.ToString, 20)
            ' Add Quantity cell
            Dim qCell As New DevComponents.AdvTree.Cell(order.ItemQuantity.ToString("N0"))
            qCell.StyleNormal = NumberStyle
            orderNode.Cells.Add(qCell)
            ' Add MetaLevel cell
            Dim mlCell As New DevComponents.AdvTree.Cell(item.MetaLevel.ToString("N0"))
            mlCell.StyleNormal = NumberStyle
            orderNode.Cells.Add(mlCell)
            ' Add Volume cell
            Dim vCell As New DevComponents.AdvTree.Cell((item.Volume * order.ItemQuantity).ToString("N2"))
            vCell.StyleNormal = NumberStyle
            orderNode.Cells.Add(vCell)
            ' Add Unit Cost cell
            ' UnitCost = prices(order.ItemID)
            Dim ucCell As New DevComponents.AdvTree.Cell("Processing...")
            ucCell.TextDisplayFormat = "N2"
            ucCell.StyleNormal = NumberStyle
            orderNode.Cells.Add(ucCell)
            ' Add Total Cost cell
            Dim tcCell As New DevComponents.AdvTree.Cell("Processing...")
            tcCell.TextDisplayFormat = "N2"
            tcCell.StyleNormal = NumberStyle
            orderNode.Cells.Add(tcCell)
            ' Add Owned cell
            Dim oCell As New DevComponents.AdvTree.Cell
            oCell.StyleNormal = NumberStyle
            If ownedAssets.ContainsKey(order.ItemName) Then
                OrderOwned = ownedAssets(order.ItemName).TotalQuantity
                For Each locID As String In ownedAssets(order.ItemName).Locations.Keys
                    Dim locNode As New DevComponents.AdvTree.Node
                    locNode.Text = EveHQ.Core.DataFunctions.GetLocationName(locID)
                    locNode.Cells.Add(New DevComponents.AdvTree.Cell(ownedAssets(order.ItemName).Locations(locID).ToString("N0")))
                    locNode.Cells(1).StyleNormal = NumberStyle
                    orderNode.Nodes.Add(locNode)
                Next
            Else
                OrderOwned = 0
            End If
            oCell.Text = OrderOwned.ToString("N0")
            orderNode.Cells.Add(oCell)
            ' Add Surplus cell
            Dim sCell As New DevComponents.AdvTree.Cell((OrderOwned - order.ItemQuantity).ToString("N0"))
            sCell.StyleNormal = NumberStyle
            orderNode.Cells.Add(sCell)
            ' Add Surplus Cost cell
            Dim scCell As New DevComponents.AdvTree.Cell((UnitCost * (Math.Max(order.ItemQuantity - OrderOwned, 0))).ToString("N2"))
            scCell.StyleNormal = NumberStyle
            orderNode.Cells.Add(scCell)
            ' Add Node to the list
            adtOrders.Nodes.Add(orderNode)
            TotalItems += order.ItemQuantity
            TotalItemsReqd += Math.Max(order.ItemQuantity - OrderOwned, 0)
            TotalVolume += item.Volume * order.ItemQuantity
            TotalVolumeReqd += (item.Volume * (Math.Max(order.ItemQuantity - OrderOwned, 0)))
          
        Next
        adtOrders.EndUpdate()
        ' Update summary information
        lblSummary.Text = "Summary - " & Req.Name
        lblUniqueItems.Text = Req.Orders.Count.ToString("N0")

        ' Update Pricing from task
        priceTask.ContinueWith(Sub(currentTask As Task(Of Dictionary(Of String, Double)))
                                   'Bug EVEHQ-169 : this is called even after the window is destroyed but not GC'd. check the handle boolean first.
                                   If IsHandleCreated Then
                                       Dim priceData As Dictionary(Of String, Double) = currentTask.Result
                                       ' cut over to main thread
                                       Invoke(Sub()
                                                  For Each row As Node In adtOrders.Nodes
                                                      Dim price As Double
                                                      Dim quantity As Long
                                                      If (priceData.TryGetValue(row.Name, price)) Then
                                                          row.Cells(3).Text = price.ToInvariantString("F2")
                                                          Long.TryParse(row.Cells(0).Text, quantity)
                                                          row.Cells(4).Text = (price * quantity).ToInvariantString("F2")

                                                      End If
                                                      Dim asset As RequisitionAsset
                                                      Dim owned As Long = 0
                                                      If (ownedAssets.TryGetValue(row.Text, asset)) Then
                                                          owned = asset.TotalQuantity
                                                      End If

                                                      TotalCost += (price * quantity)
                                                      TotalCostReqd += (price * (Math.Max(quantity - owned, 0)))
                                                  Next

                                                  lblTotalItems.Text = TotalItems.ToString("N0") & " (" & TotalItemsReqd.ToString("N0") & ")"
                                                  lblTotalCost.Text = "Total: " & TotalCost.ToString("N2") & " ISK" & ControlChars.CrLf & "Reqd: " & TotalCostReqd.ToString("N2") & " ISK"
                                                  lblTotalVolume.Text = "Total: " & TotalVolume.ToString("N2") & " m³" & ControlChars.CrLf & "Reqd: " & TotalVolumeReqd.ToString("N2") & " m³"

                                              End Sub)
                                   End If


                               End Sub)


        
    End Sub

    Private Sub btnDeleteReq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteReq.Click
        Call Me.DeleteRequisitions()
    End Sub

    Private Sub DeleteRequisitions()
        ' Check parent nodes
        Dim parentNodeCount As Integer = 0
        For Each selNode As DevComponents.AdvTree.Node In adtReqs.SelectedNodes
            If selNode.Level = 0 Then
                parentNodeCount += 1
            End If
        Next
        If parentNodeCount = 0 Then
            MessageBox.Show("At least one Requisition heading must be selected before deleting. Please make a selection before using this option.", "Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to delete the selected Requisitions?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.No Then
                Exit Sub
            Else
                For Each selNode As DevComponents.AdvTree.Node In adtReqs.SelectedNodes
                    If selNode.Level = 0 Then
                        Dim reqName As String = selNode.Name
                        Dim Req As Requisition = CurrentReqs(reqName)
                        If Req.DeleteFromDatabase = True Then
                        End If
                    End If
                Next
                Call Me.ApplyFilter()
                Call Me.UpdateFilters()
            End If
        End If
    End Sub

    Private Sub btnCopyReq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyReq.Click
        ' Check parent nodes
        Dim parentNodeCount As Integer = 0
        For Each selNode As DevComponents.AdvTree.Node In adtReqs.SelectedNodes
            If selNode.Level = 0 Then
                parentNodeCount += 1
            End If
        Next
        If parentNodeCount = 0 Then
            MessageBox.Show("At least one Requisition heading must be selected before copying. Please make a selection before using this option.", "Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Clone the selected requisition
            Dim selNode As DevComponents.AdvTree.Node = adtReqs.SelectedNodes(0)
            Dim oldReq As EveHQ.Core.Requisition = CurrentReqs(selNode.Name)
            Dim newReq As EveHQ.Core.Requisition = CType(oldReq.Clone, Requisition)
            Dim copy As Integer = 0
            Do
                copy += 1
            Loop Until CurrentReqs.ContainsKey(newReq.Name & " (Copy " & copy.ToString & ")") = False
            newReq.Name &= " (Copy " & copy.ToString & ")"
            ' Save the current selected Requisition
            newReq.WriteToDatabase()
            Call Me.ApplyFilter()
            Call Me.UpdateFilters()
        End If
    End Sub

    Private Sub btnMerge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMerge.Click
        ' Check parent nodes
        Dim parentNodeCount As Integer = 0
        For Each selNode As DevComponents.AdvTree.Node In adtReqs.SelectedNodes
            If selNode.Level = 0 Then
                parentNodeCount += 1
            End If
        Next
        If parentNodeCount < 2 Then
            MessageBox.Show("At least two Requisition headings must be selected before merging. Please make the appropriate selection before using this option.", "Mulitple Requisitions Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            Dim ReqList As New SortedList(Of String, EveHQ.Core.Requisition)
            For Each reqNode As DevComponents.AdvTree.Node In adtReqs.SelectedNodes
                ReqList.Add(reqNode.Name, CurrentReqs(reqNode.Name))
            Next

            ' Create a new merge form and wait for a result
            Dim NewMergeForm As New frmMergeRequisitions(ReqList)
            NewMergeForm.ShowDialog()

            ' DB handling is done in the merge form so just update the filters
            Call Me.ApplyFilter()
            Call Me.UpdateFilters()
        End If
    End Sub

    Private Sub adtOrders_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtOrders.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub adtOrders_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtOrders.NodeDoubleClick
        Call Me.EditRequisition()
    End Sub

    Private Sub GetOwnedResources()

        ' Get the primary requestor
        If cboAssetSelection.SelectedItem IsNot Nothing Then

            Dim AssetOwner As String = cboAssetSelection.SelectedItem.ToString
            Dim AssetAccount As New EveHQ.Core.EveAccount
            Dim OwnerID As String = ""
            Dim IsCorp As Boolean = False

            ' Check whether a pilot or a corp
            If EveHQ.Core.HQ.EveHqSettings.Pilots.Contains(AssetOwner) = True Then
                AssetAccount = CType(EveHQ.Core.HQ.EveHqSettings.Accounts(CType(EveHQ.Core.HQ.EveHqSettings.Pilots(AssetOwner), EveHQ.Core.Pilot).Account), EveAccount)
                OwnerID = CType(EveHQ.Core.HQ.EveHqSettings.Pilots(AssetOwner), EveHQ.Core.Pilot).ID
                IsCorp = False
            Else
                AssetAccount = CType(EveHQ.Core.HQ.EveHqSettings.Accounts(EveHQ.Core.HQ.EveHqSettings.Corporations(AssetOwner).Accounts(0)), EveAccount)
                OwnerID = EveHQ.Core.HQ.EveHqSettings.Corporations(AssetOwner).ID
                IsCorp = True
            End If

            ' Clear the current owned resources list
            ownedAssets.Clear()

            ' Fetch the resources owned
            ' Parse the Assets XML
            Dim assetXML As New XmlDocument
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHqSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            If IsCorp = True Then
                assetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, AssetAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
            Else
                assetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, AssetAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
            End If
            If assetXML IsNot Nothing Then
                Dim locList As XmlNodeList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                If locList.Count > 0 Then
                    ' Define what we want to obtain
                    Dim categories, groups As New ArrayList
                    For Each loc As XmlNode In locList
                        Call GetAssetQuantitesFromNode(loc, "", categories, groups, ownedAssets)
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub GetAssetQuantitesFromNode(ByVal item As XmlNode, ByVal locationID As String, ByVal categories As ArrayList, ByVal groups As ArrayList, ByRef Assets As SortedList(Of String, EveHQ.Core.RequisitionAsset))
        Dim ItemData As New EveHQ.Core.EveItem
        Dim AssetID As String = ""
        Dim itemID As String = ""
        AssetID = item.Attributes.GetNamedItem("itemID").Value
        itemID = item.Attributes.GetNamedItem("typeID").Value
        ' Check if the flag is excluded
        If ExclusionFlags.Contains(CInt(item.Attributes.GetNamedItem("flag").Value)) = False Then
            If item.Attributes.GetNamedItem("locationID") IsNot Nothing Then
                locationID = item.Attributes.GetNamedItem("locationID").Value
            End If
            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                ItemData = EveHQ.Core.HQ.itemData(itemID)
                If categories.Count > 0 Or groups.Count > 0 Then
                    ' Check if this is an excluded ship first
                    If chkAssembledShips.Checked = False Or Not (chkAssembledShips.Checked = True And ItemData.Category = 6 And item.Attributes.GetNamedItem("singleton").Value = "1") Then
                        If categories.Contains(ItemData.Category) Or groups.Contains(ItemData.Group) Or groupResources.ContainsKey(CStr(ItemData.ID)) Then
                            ' Check if the item is in the list
                            If Assets.ContainsKey(CStr(ItemData.Name)) = False Then
                                Assets.Add(CStr(ItemData.Name), New EveHQ.Core.RequisitionAsset(locationID, CLng(item.Attributes.GetNamedItem("quantity").Value)))
                            Else
                                Assets(CStr(ItemData.Name)).AddAsset(locationID, CLng(item.Attributes.GetNamedItem("quantity").Value))
                            End If
                        End If
                    End If
                Else
                    ' Check if this is an excluded ship first
                    If chkAssembledShips.Checked = False Or Not (chkAssembledShips.Checked = True And ItemData.Category = 6 And item.Attributes.GetNamedItem("singleton").Value = "1") Then
                        ' Check if the item is in the list
                        If Assets.ContainsKey(CStr(ItemData.Name)) = False Then
                            Assets.Add(CStr(ItemData.Name), New EveHQ.Core.RequisitionAsset(locationID, CLng(item.Attributes.GetNamedItem("quantity").Value)))
                        Else
                            Assets(CStr(ItemData.Name)).AddAsset(locationID, CLng(item.Attributes.GetNamedItem("quantity").Value))
                        End If
                    End If
                End If
            End If
            ' Check child items if they exist
            If item.ChildNodes.Count > 0 Then
                For Each subitem As XmlNode In item.ChildNodes(0).ChildNodes
                    Call GetAssetQuantitesFromNode(subitem, locationID, categories, groups, Assets)
                Next
            End If
        End If
    End Sub

    Private Sub cboAssetSelection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAssetSelection.SelectedIndexChanged
        Call Me.UpdateOrderList()
    End Sub

    Private Sub btnExportTSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportTSV.Click
        Call Me.ExportReq(ControlChars.Tab)
    End Sub

    Private Sub btnExportCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportCSV.Click
        Call Me.ExportReq(EveHQ.Core.HQ.EveHqSettings.CSVSeparatorChar)
    End Sub

    Private Sub ExportReq(ByVal sepChar As String)
        ' Check parent nodes
        Dim parentNodeCount As Integer = 0
        For Each selNode As DevComponents.AdvTree.Node In adtReqs.SelectedNodes
            If selNode.Level = 0 Then
                parentNodeCount += 1
            End If
        Next
        If parentNodeCount = 0 Then
            MessageBox.Show("At least one Requisition heading must be selected before exporting. Please make a selection before using this option.", "Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Export the selected requisition
            Dim selNode As DevComponents.AdvTree.Node = adtReqs.SelectedNodes(0)
            Dim req As EveHQ.Core.Requisition = CurrentReqs(selNode.Name)
            Dim str As New StringBuilder
            str.AppendLine("EveHQ Requisition - " & req.Name)
            str.AppendLine("Requestor:" & sepChar & req.Requestor)
            str.AppendLine("Source: " & sepChar & req.Source)
            str.AppendLine()
            str.AppendLine("ItemName" & sepChar & "Date" & sepChar & "Quantity")
            For Each order As EveHQ.Core.RequisitionOrder In req.Orders.Values
                str.Append(order.ItemName & sepChar)
                str.Append(order.RequestDate.ToString & sepChar)
                str.AppendLine(order.ItemQuantity.ToString("N2"))
            Next
            Try
                Clipboard.SetText(str.ToString)
            Catch ex As Exception
                MessageBox.Show("There was an error exporting the data to the clipboard", "Export Requisition Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

	Private Sub btnExportEveHQ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportEveHQ.Click
		' Routine to export a requisition to a format that can be imported into EveHQ
		' Check parent nodes
		Dim parentNodeCount As Integer = 0
		For Each selNode As DevComponents.AdvTree.Node In adtReqs.SelectedNodes
			If selNode.Level = 0 Then
				parentNodeCount += 1
			End If
		Next
		If parentNodeCount = 0 Then
			MessageBox.Show("At least one Requisition heading must be selected before exporting. Please make a selection before using this option.", "Requisition Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Exit Sub
		Else
			' Create XML Document
			Dim ReqXML As New XmlDocument
			' Create XML Declaration
			Dim dec As XmlDeclaration = ReqXML.CreateXmlDeclaration("1.0", Nothing, Nothing)
			ReqXML.AppendChild(dec)
			' Create root
			Dim ReqRoot As XmlElement = ReqXML.CreateElement("EveHQRequisitions")
			ReqXML.AppendChild(ReqRoot)
			' Get the requisition
			Dim selNode As DevComponents.AdvTree.Node = adtReqs.SelectedNodes(0)
			Dim req As EveHQ.Core.Requisition = CurrentReqs(selNode.Name)
			' Generate the specific XML
			Call GenerateXMLForRequisition(req, ReqXML, ReqRoot)
			' Get a file name
			Dim sfd1 As New SaveFileDialog
			With sfd1
				.Title = "Save as EveHQ Requisition Export File..."
				.FileName = ""
				.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
				.Filter = "EveHQ Requisition Export Files (*.xml)|*.xml|All Files (*.*)|*.*"
				.FilterIndex = 1
				.RestoreDirectory = True
				If .ShowDialog() = Windows.Forms.DialogResult.OK Then
					If .FileName <> "" Then
						ReqXML.Save(.FileName)
					End If
				End If
			End With
		End If
	End Sub

	Private Sub GenerateXMLForRequisition(ByVal Req As Requisition, ByRef ReqXML As XmlDocument, ByRef ReqRoot As XmlElement)
		Dim ReqAtt As XmlAttribute

		' Create various elements
		Dim ReqReq As XmlElement = ReqXML.CreateElement("requisition")
		ReqRoot.AppendChild(ReqReq)

		ReqAtt = ReqXML.CreateAttribute("name")
		ReqAtt.Value = Req.Name
		ReqReq.Attributes.Append(ReqAtt)

		ReqAtt = ReqXML.CreateAttribute("requestor")
		ReqAtt.Value = Req.Requestor
		ReqReq.Attributes.Append(ReqAtt)

		ReqAtt = ReqXML.CreateAttribute("source")
		ReqAtt.Value = Req.Source
		ReqReq.Attributes.Append(ReqAtt)

		Dim ReqOrders As XmlElement = ReqXML.CreateElement("orders")
		ReqReq.AppendChild(ReqOrders)

		' Create Orders
		For Each Order As RequisitionOrder In Req.Orders.Values

			Dim ReqOrder As XmlNode = ReqXML.CreateElement("order")

			ReqAtt = ReqXML.CreateAttribute("itemID")
			ReqAtt.Value = Order.ItemID
			ReqOrder.Attributes.Append(ReqAtt)

			ReqAtt = ReqXML.CreateAttribute("itemName")
			ReqAtt.Value = Order.ItemName
			ReqOrder.Attributes.Append(ReqAtt)

			ReqAtt = ReqXML.CreateAttribute("itemQuantity")
			ReqAtt.Value = Order.ItemQuantity.ToString
			ReqOrder.Attributes.Append(ReqAtt)

			ReqAtt = ReqXML.CreateAttribute("requestDate")
			ReqAtt.Value = Order.RequestDate.ToString(IndustryTimeFormat)
			ReqOrder.Attributes.Append(ReqAtt)

			ReqAtt = ReqXML.CreateAttribute("source")
			ReqAtt.Value = Order.Source
			ReqOrder.Attributes.Append(ReqAtt)

			ReqOrders.AppendChild(ReqOrder)
		Next
	End Sub

	Private Sub btnImportEveHQ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportEveHQ.Click
		' Create a new file dialog
		Dim ofd1 As New OpenFileDialog
		With ofd1
			.Title = "Select EveHQ Requisition Export File..."
			.FileName = ""
			.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
			.Filter = "EveHQ Requisition Export Files (*.xml)|*.xml|All Files (*.*)|*.*"
			.FilterIndex = 1
			.RestoreDirectory = True
			If .ShowDialog() = Windows.Forms.DialogResult.OK Then
				If My.Computer.FileSystem.FileExists(.FileName) = False Then
					MessageBox.Show("Specified file does not exist. Please try again.", "Error Finding File", MessageBoxButtons.OK, MessageBoxIcon.Information)
					Exit Sub
				Else
					' Open the file for reading
					Dim ReqXML As New XmlDocument
					Try
						ReqXML.Load(.FileName)
					Catch ex As Exception
						MessageBox.Show("Unable to read file data. Please check the file is not corrupted and you have permission to access this file", "File Access Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
					End Try
					Dim ReqList As XmlNodeList = ReqXML.SelectNodes("/EveHQRequisitions/requisition")
					If ReqList.Count > 0 Then
						For Each Req As XmlNode In ReqList
							Dim Orders As SortedList(Of String, Integer) = GetOrdersFromRequisitionXML(Req)
							Dim newReq As New EveHQ.Core.frmAddRequisition(Req.Attributes.GetNamedItem("name").Value, Req.Attributes.GetNamedItem("source").Value, Orders)
							newReq.ShowDialog()
						Next
					Else
						MessageBox.Show("This file does not contain any valid EveHQ Requisitions. Import Process aborted.", "No Requisitions Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
					End If
				End If
			End If
		End With
	End Sub

	Private Function GetOrdersFromRequisitionXML(ByVal Req As XmlNode) As SortedList(Of String, Integer)
		Dim Orders As New SortedList(Of String, Integer)
		Dim OrderList As XmlNodeList = Req.SelectNodes("orders/order")
		For Each Order As XmlNode In OrderList
			Orders.Add(Order.Attributes.GetNamedItem("itemName").Value, CInt(Order.Attributes.GetNamedItem("itemQuantity").Value))
		Next
		Return Orders
	End Function

    Private Sub chkFittedModules_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFittedModules.CheckedChanged
        If chkFittedModules.Checked = True Then
            If ExclusionFlags.Contains(11) = False Then
                For flag As Integer = 11 To 34
                    ExclusionFlags.Add(flag)
                Next
                For flag As Integer = 92 To 99
                    ExclusionFlags.Add(flag)
                Next
                For flag As Integer = 125 To 132
                    ExclusionFlags.Add(flag)
                Next
            End If
        Else
            If ExclusionFlags.Contains(11) = True Then
                For flag As Integer = 11 To 34
                    ExclusionFlags.Remove(flag)
                Next
                For flag As Integer = 92 To 99
                    ExclusionFlags.Remove(flag)
                Next
                For flag As Integer = 125 To 132
                    ExclusionFlags.Remove(flag)
                Next
            End If
        End If
        Call Me.UpdateOrderList()
    End Sub

    Private Sub chkCargoBay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCargoBay.CheckedChanged
        If chkCargoBay.Checked = True Then
            If ExclusionFlags.Contains(5) = False Then
                ExclusionFlags.Add(5)
            End If
        Else
            If ExclusionFlags.Contains(5) = True Then
                ExclusionFlags.Remove(5)
            End If
        End If
        Call Me.UpdateOrderList()
    End Sub

    Private Sub chkDroneBay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDroneBay.CheckedChanged
        If chkDroneBay.Checked = True Then
            If ExclusionFlags.Contains(87) = False Then
                ExclusionFlags.Add(87)
            End If
        Else
            If ExclusionFlags.Contains(87) = True Then
                ExclusionFlags.Remove(87)
            End If
        End If
        Call Me.UpdateOrderList()
    End Sub

    Private Sub chkAssembledShips_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAssembledShips.CheckedChanged
        Call Me.UpdateOrderList()
    End Sub

End Class