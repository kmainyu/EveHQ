﻿' ========================================================================
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
Imports System.Data
Imports EveHQ.Core
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports System.IO
Imports System.Xml
Imports System.Net
Imports System.Text
Imports EveHQ.Market
Imports System.Threading.Tasks

Public Class frmMarketPrices
    Dim Regions As New SortedList(Of String, Long) ' RegionName, RegionID
    Dim RegionNames As New SortedList(Of Long, String) ' RegionID, RegionName
    Dim marketCacheFolder As String = ""
    Dim startUp As Boolean = True
    Dim UpdatePriceGroup As Boolean = False
    Dim PriceGroupFlagBoxes As New List(Of CheckBox)
    Dim CurrentPriceGroup As New EveHQ.Core.PriceGroup
    Dim AdtItems As New AdvTree
    Dim WithEvents MarketGroupItemsWorker As New BackgroundWorker
    Dim WithEvents MarketDownloadWorker As New BackgroundWorker
    Dim WithEvents MarketUpdateWorker As New BackgroundWorker

    ' Set Styles
    Dim NormalLogStyle As ElementStyle
    Dim ExpiredLogStyle As ElementStyle

#Region "Form Opening and Closing Routines"

    Private Sub frmMarketPrices_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Initialize Price check item list
        If (_itemsList.Items.Count = 0) Then
            _itemsList.Items.AddRange((From item In Core.HQ.itemData.Values Where item.MarketGroup <> 0 Select item.Name).ToArray())
        End If


        ' Set Styles
        NormalLogStyle = adtLogs.Styles("Log").Copy
        ExpiredLogStyle = adtLogs.Styles("Log").Copy
        NormalLogStyle.BackColor = Color.LightGreen
        NormalLogStyle.BackColor2 = Color.FromArgb(128, NormalLogStyle.BackColor2)
        NormalLogStyle.TextColor = Color.Black
        ExpiredLogStyle.BackColor2 = Color.Salmon
        ExpiredLogStyle.BackColor = Color.FromArgb(128, ExpiredLogStyle.BackColor2)
        ExpiredLogStyle.TextColor = Color.Black

        startUp = True

        ' Check for the market cache folder
        If My.Computer.FileSystem.DirectoryExists(Path.Combine(EveHQ.Core.HQ.AppDataFolder, "MarketCache")) = False Then
            Try
                marketCacheFolder = Path.Combine(EveHQ.Core.HQ.AppDataFolder, "MarketCache")
                My.Computer.FileSystem.CreateDirectory(marketCacheFolder)
            Catch ex As Exception
                MessageBox.Show("An error occured while attempting to create the Market Cache folder: " & ex.Message, "Error Creating Market Cache Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End Try
        Else
            marketCacheFolder = Path.Combine(EveHQ.Core.HQ.AppDataFolder, "MarketCache")
        End If

        ' Get Regions
        Call Me.LoadRegionNames()

        ' Get the contents of the market log folder
        Call Me.ImportLogDetails()

        ' Update Market Price Settings
        'Call Me.UpdatePriceSettings()

        ' Update the Custom Price Grid
        Call Me.UpdatePriceMatrix()

        ' Update the Cache File List
        'Call Me.UpdateCacheFileList()

        ' Initialise the price group data
        Call Me.InitialisePriceGroupData()

        ' Set the market data source
        'Select Case EveHQ.Core.HQ.EveHqSettings.MarketDataSource
        '    Case MarketSite.Battleclinic
        '        radBattleclinic.Checked = True
        '    Case MarketSite.EveMarketeer
        '        radEveMarketeer.Checked = True
        'End Select

        startUp = False

    End Sub

    Private Sub LoadRegionNames()
        Dim regionSet As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM mapRegions ORDER BY regionName;")
        If regionSet IsNot Nothing Then
            Regions.Clear()
            RegionNames.Clear()
            For Each regionRow As DataRow In regionSet.Tables(0).Rows
                If CStr(regionRow.Item("regionName")) <> "Unknown" Then
                    Regions.Add(CStr(regionRow.Item("regionName")), CLng(regionRow.Item("regionID")))
                    RegionNames.Add(CLng(regionRow.Item("regionID")), CStr(regionRow.Item("regionName")))
                End If
            Next
        Else
            MessageBox.Show("EveHQ cannot proceed with the market price processing until the Map Regions have been correctly loaded.", "Error Loading Map Regions", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Public Sub ImportLogDetails()
        ' Get the contents of the market log folder and display them
        adtLogs.BeginUpdate()
        adtLogs.Nodes.Clear()
        Dim EveMLFolder As String = Path.Combine(Path.Combine(Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Eve"), "Logs"), "Marketlogs")
        If My.Computer.FileSystem.DirectoryExists(EveMLFolder) = True Then
            For Each file As String In My.Computer.FileSystem.GetFiles(EveMLFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
                Call Me.DisplayLogDetails(file)
                adtLogs.Enabled = True
            Next
        Else
            adtLogs.Nodes.Add(New Node("Log Directory Not Available"))
            adtLogs.Enabled = False
        End If
        adtLogs.EndUpdate()
    End Sub

    Private Sub frmMarketPrices_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        ' Disable the Add Item button until the thread has completed
        btnAddItem.Enabled = False
        MarketGroupItemsWorker.RunWorkerAsync()
    End Sub

    Private Sub MarketGroupItemsWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MarketGroupItemsWorker.DoWork
        ' Load the market group data
        Call Me.LoadMarketGroupData()
    End Sub

    Private Sub MarketGroupItemsWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles MarketGroupItemsWorker.RunWorkerCompleted
        ' Enable the Add Item button
        btnAddItem.Enabled = True
    End Sub

#End Region

    Public Sub DisplayLogDetails(ByVal file As String)
        ' Split the filename into sections denoted by "-"
        Dim LogItem As New Node
        Dim FI As FileInfo
        Dim info() As String
        Dim idx As Integer = 0
        Dim region As String = ""
        Dim item As String = ""
        Dim TimeFormat As String = "yyyy.MM.dd HHmmss"
        Dim logDate As Date = Now
        Dim logAge As Long
        ' Check for the regions
        If Regions.Count = 0 Then
            Call Me.LoadRegionNames()
        End If
        Try
            FI = New FileInfo(file)
            info = FI.Name.TrimEnd(".txt".ToCharArray).Split("-".ToCharArray)
            If info.Length > 2 Then
                region = "" : item = ""
                idx = -1
                Do
                    idx += 1
                    region &= info(idx).Trim & "-"
                    ' Repeat until we have a valid
                Loop Until Regions.ContainsKey(region.TrimEnd("-".ToCharArray))
                region = region.TrimEnd("-".ToCharArray)
                For idx = idx + 1 To info.Length - 2
                    item &= info(idx) & "-"
                Next
                item = item.TrimEnd("-".ToCharArray).Trim
                logDate = FI.CreationTime
                LogItem = New Node
                LogItem.Tag = file
                LogItem.Text = region
                adtLogs.Nodes.Add(LogItem)
                LogItem.Cells.Add(New Cell(item))
                LogItem.Cells.Add(New Cell(logDate.ToString))
                logAge = DateDiff("h", logDate, Date.Now)
                LogItem.Cells.Add(New Cell(CStr(logAge)))
                If logAge > CLng(nudAge.Value) Then
                    LogItem.Style = ExpiredLogStyle
                Else
                    LogItem.Style = NormalLogStyle
                End If
            End If
        Catch e As Exception
            MessageBox.Show("There was an error processing the log details for " & file & ". The error was: " & e.Message, "Display Log Details Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Public Sub ResortLogs()
        EveHQ.Core.AdvTreeSorter.Sort(adtLogs, 1, True, True)
    End Sub

    '#Region "Market Price Settings"

    '    Private Sub UpdatePriceSettings()
    '        ' Update the checkboxes
    '        chkEnableLogWatcher.Checked = EveHQ.Core.HQ.EveHqSettings.EnableMarketLogWatcher
    '        chkEnableWatcherAtStartup.Checked = EveHQ.Core.HQ.EveHqSettings.EnableMarketLogWatcherAtStartup
    '        chkAutoUpdateCurrentPrice.Checked = EveHQ.Core.HQ.EveHqSettings.MarketLogUpdatePrice
    '        chkAutoUpdatePriceData.Checked = EveHQ.Core.HQ.EveHqSettings.MarketLogUpdateData
    '        chkNotifyPopup.Checked = EveHQ.Core.HQ.EveHqSettings.MarketLogPopupConfirm
    '        chkNotifyTray.Checked = EveHQ.Core.HQ.EveHqSettings.MarketLogToolTipConfirm
    '        chkIgnoreBuyOrders.Checked = EveHQ.Core.HQ.EveHqSettings.IgnoreBuyOrders
    '        chkIgnoreSellOrders.Checked = EveHQ.Core.HQ.EveHqSettings.IgnoreSellOrders
    '        ' Update the NUD controls
    '        nudIgnoreBuyOrderLimit.Value = CDec(EveHQ.Core.HQ.EveHqSettings.IgnoreBuyOrderLimit)
    '        nudIgnoreSellOrderLimit.Value = CDec(EveHQ.Core.HQ.EveHqSettings.IgnoreSellOrderLimit)
    '    End Sub

    '    Private Sub chkEnableLogWatcher_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        If chkEnableLogWatcher.Checked = True Then
    '            If frmEveHQ.InitialiseWatchers() = True Then
    '                EveHQ.Core.HQ.EveHqSettings.EnableMarketLogWatcher = True
    '            Else
    '                MessageBox.Show("Unable to start Market Log Watcher. Please check Eve is installed and the market log export folder exists.", "Error Starting Watcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                chkEnableLogWatcher.Checked = False
    '            End If
    '        Else
    '            Call frmEveHQ.CancelWatchers()
    '            EveHQ.Core.HQ.EveHqSettings.EnableMarketLogWatcher = False
    '        End If
    '    End Sub

    '    Private Sub chkEnableWatcherAtStartup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.EnableMarketLogWatcherAtStartup = chkEnableWatcherAtStartup.Checked
    '    End Sub

    '    Private Sub chkAutoUpdateCurrentPrice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.MarketLogUpdatePrice = chkAutoUpdateCurrentPrice.Checked
    '    End Sub

    '    Private Sub chkAutoUpdatePriceData_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.MarketLogUpdateData = chkAutoUpdatePriceData.Checked
    '    End Sub

    '    Private Sub chkNotifyPopup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.MarketLogPopupConfirm = chkNotifyPopup.Checked
    '    End Sub

    '    Private Sub chkNotifyTray_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.MarketLogToolTipConfirm = chkNotifyTray.Checked
    '    End Sub

    '    Private Sub chkIgnoreBuyOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.IgnoreBuyOrders = chkIgnoreBuyOrders.Checked
    '    End Sub

    '    Private Sub chkIgnoreSellOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.IgnoreSellOrders = chkIgnoreSellOrders.Checked
    '    End Sub

    '    Private Sub nudIgnoreBuyOrderLimit_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.IgnoreBuyOrderLimit = nudIgnoreBuyOrderLimit.Value
    '    End Sub

    '    Private Sub nudIgnoreBuyOrderLimit_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        If startUp = False Then
    '            EveHQ.Core.HQ.EveHqSettings.IgnoreBuyOrderLimit = nudIgnoreBuyOrderLimit.Value
    '        End If
    '    End Sub

    '    Private Sub nudIgnoreSellOrderLimit_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs)
    '        EveHQ.Core.HQ.EveHqSettings.IgnoreSellOrderLimit = nudIgnoreSellOrderLimit.Value
    '    End Sub

    '    Private Sub nudIgnoreSellOrderLimit_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        If startUp = False Then
    '            EveHQ.Core.HQ.EveHqSettings.IgnoreSellOrderLimit = nudIgnoreSellOrderLimit.Value
    '        End If
    '    End Sub

    '#End Region

#Region "Custom Prices Functions"
    Private Sub txtSearchPrices_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchPrices.TextChanged
        If Len(txtSearchPrices.Text) > 2 Then
            Dim strSearch As String = txtSearchPrices.Text.Trim.ToLower
            Call Me.UpdatePriceMatrix(strSearch)
        End If
    End Sub
    Private Sub btnResetGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetGrid.Click
        txtSearchPrices.Text = ""
        Call Me.UpdatePriceMatrix("")
    End Sub
    Private Sub UpdatePriceMatrix(Optional ByVal search As String = "")
        ' Loads prices into the listview
        adtPrices.BeginUpdate()
        adtPrices.Nodes.Clear()
        Dim lvItem As New Node
        Dim itemID As String = ""
        Dim itemData As New EveHQ.Core.EveItem
        Dim price As Double = 0
        If chkShowOnlyCustom.Checked = True Then
            For Each itemID In EveHQ.Core.HQ.CustomPriceList.Keys ' ID
                itemData = EveHQ.Core.HQ.itemData(itemID)
                If itemData.Name.ToLower.Contains(search) = True Then
                    If itemData.Published = True Then
                        lvItem = New Node
                        lvItem.Text = itemData.Name
                        lvItem.Name = itemID
                        lvItem.Cells.Add(New Cell(EveHQ.Core.HQ.itemData(itemID).BasePrice.ToString("N2")))
                        If EveHQ.Core.HQ.MarketPriceList.ContainsKey(itemID) Then
                            price = CDbl(EveHQ.Core.HQ.MarketPriceList(itemID))
                            lvItem.Cells.Add(New Cell(price.ToString("N2")))
                        Else
                            lvItem.Cells.Add(New Cell(""))
                        End If
                        If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) Then
                            price = CDbl(EveHQ.Core.HQ.CustomPriceList(itemID))
                            lvItem.Cells.Add(New Cell(price.ToString("N2")))
                        Else
                            lvItem.Cells.Add(New Cell(""))
                        End If
                        adtPrices.Nodes.Add(lvItem)
                    End If
                End If
            Next
        Else
            For Each item As String In EveHQ.Core.HQ.itemList.Keys
                If item.ToLower.Contains(search) = True Then
                    itemID = EveHQ.Core.HQ.itemList(item)
                    If HQ.itemData.TryGetValue(itemID, itemData) = True Then
                        If itemData.Published = True Then
                            lvItem = New Node
                            lvItem.Text = itemData.Name
                            lvItem.Name = CStr(itemData.ID)
                            lvItem.Cells.Add(New Cell(EveHQ.Core.HQ.itemData(itemID).BasePrice.ToString("N2")))
                            If EveHQ.Core.HQ.MarketPriceList.ContainsKey(itemID) Then
                                price = CDbl(EveHQ.Core.HQ.MarketPriceList(itemID))
                                lvItem.Cells.Add(New Cell(price.ToString("N2")))
                            Else
                                lvItem.Cells.Add(New Cell(""))
                            End If
                            If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) Then
                                price = CDbl(EveHQ.Core.HQ.CustomPriceList(itemID))
                                lvItem.Cells.Add(New Cell(price.ToString("N2")))
                            Else
                                lvItem.Cells.Add(New Cell(""))
                            End If
                            adtPrices.Nodes.Add(lvItem)
                        End If
                    End If
                End If
            Next
        End If
        EveHQ.Core.AdvTreeSorter.Sort(adtPrices, 1, False, True)
        adtPrices.EndUpdate()
    End Sub
    Private Sub ctxPrices_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxPrices.Opening
        Select Case adtPrices.SelectedNodes.Count
            Case 0
                e.Cancel = True
            Case 1
                Dim selItem As Node = adtPrices.SelectedNodes(0)
                mnuPriceItemName.Text = selItem.Text
                Dim selItemID As String = selItem.Name
                ' Check if it exists and we can edit/delete it
                If EveHQ.Core.HQ.CustomPriceList.ContainsKey(selItemID) = True Then
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
    Private Sub mnuPriceDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPriceDelete.Click
        For Each selitem As Node In adtPrices.SelectedNodes
            Dim selItemID As String = selitem.Name
            If EveHQ.Core.HQ.CustomPriceList.ContainsKey(selItemID) = True Then
                ' Double check it exists and delete it
                Call EveHQ.Core.DataFunctions.DeleteCustomPrice(selItemID)
                ' refresh that asset rather than the whole list
            End If
            selitem.Cells(3).Text = ""
        Next
    End Sub
    Private Sub mnuPriceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPriceAdd.Click
        Dim selItem As Node = adtPrices.SelectedNodes(0)
        Dim itemID As String = selItem.Name
        Dim newPrice As New EveHQ.Core.frmModifyPrice(itemID, 0)
        newPrice.ShowDialog()
        If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) Then
            selItem.Cells(3).Text = EveHQ.Core.HQ.CustomPriceList(itemID).ToString("N2")
        End If
    End Sub
    Private Sub mnuPriceEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPriceEdit.Click
        Dim selItem As Node = adtPrices.SelectedNodes(0)
        Dim itemID As String = selItem.Name
        Dim newPrice As New EveHQ.Core.frmModifyPrice(itemID, 0)
        newPrice.ShowDialog()
        If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) Then
            selItem.Cells(3).Text = EveHQ.Core.HQ.CustomPriceList(itemID).ToString("N2")
        End If
    End Sub
    Private Sub chkShowOnlyCustom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowOnlyCustom.CheckedChanged
        If Len(txtSearchPrices.Text) > 2 Then
            Dim strSearch As String = txtSearchPrices.Text.Trim.ToLower
            Call Me.UpdatePriceMatrix(strSearch)
        Else
            Call Me.UpdatePriceMatrix()
        End If
    End Sub
    Private Sub adtPrices_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtPrices.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub
#End Region

    Private Sub mnuViewOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewOrders.Click
        Dim marketOrders As New frmMarketOrders
        marketOrders.OrdersFile = adtLogs.SelectedNodes(0).Tag.ToString
        marketOrders.Text = "Market Orders - " & adtLogs.SelectedNodes(0).Cells(1).Text & " (" & adtLogs.SelectedNodes(0).Text & ") - " & adtLogs.SelectedNodes(0).Cells(2).Text
        marketOrders.ShowDialog()
    End Sub

    Private Sub adtLogs_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub adtLogs_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs)
        Dim marketOrders As New frmMarketOrders
        If adtLogs.SelectedNodes.Count > 0 Then
            marketOrders.OrdersFile = adtLogs.SelectedNodes(0).Tag.ToString
            marketOrders.Text = "Market Orders - " & adtLogs.SelectedNodes(0).Cells(1).Text & " (" & adtLogs.SelectedNodes(0).Text & ") - " & adtLogs.SelectedNodes(0).Cells(2).Text
            marketOrders.ShowDialog()
        End If
    End Sub

    Private Sub DeleteLogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteLog.Click
        Dim msg As String = "Are you sure you wish to send the selected files to the recycle bin?"
        Dim reply As DialogResult = MessageBox.Show(msg, "Confirm Log File Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            For Each logFile As Node In adtLogs.SelectedNodes
                Try
                    My.Computer.FileSystem.DeleteFile(logFile.Tag.ToString, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)
                Catch ex As Exception
                    ' File is locked or doesn't exist - jog on
                End Try
            Next
            Me.ImportLogDetails()
        End If
    End Sub

    Private Sub HighlightAllLogs()
        Dim requestedAge As Integer = CInt(nudAge.Value)
        For Each logItem As Node In adtLogs.Nodes
            If logItem.Cells.Count >= 4 Then
                If IsNumeric(logItem.Cells(3).Text) Then
                    Dim logAge As Integer = CInt(logItem.Cells(3).Text)
                    If logAge > requestedAge Then
                        logItem.Style = ExpiredLogStyle
                    Else
                        logItem.Style = NormalLogStyle
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub nudAge_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.HighlightAllLogs()
    End Sub

    Private Sub nudAge_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.HighlightAllLogs()
    End Sub

    Private Sub ctxMarketExport_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxMarketExport.Opening
        Select Case adtLogs.SelectedNodes.Count
            Case 0
                e.Cancel = True
            Case 1
                mnuViewOrders.Enabled = True
                mnuDeleteLog.Text = "Delete Log"
            Case Is > 1
                mnuViewOrders.Enabled = False
                mnuDeleteLog.Text = "Delete Logs"
        End Select
    End Sub

    Private Sub btnRefreshLogs_Click(sender As System.Object, e As System.EventArgs)
        Call Me.ImportLogDetails()
    End Sub

#Region "Price Group Routines"

    Private Sub InitialisePriceGroupData()
        Call Me.UpdatePriceGroupList()
        Call Me.UpdatePriceGroupRegionList()
        Call Me.PopulateCheckBoxList()
        Dim GlobalNode As Node = adtPriceGroups.FindNodeByText("<Global>")
        If GlobalNode IsNot Nothing Then
            adtPriceGroups.SelectedNode = GlobalNode
        End If
    End Sub

    Private Function LoadMarketGroupData() As Boolean
        AdtItems.BeginUpdate()
        ' Create a default column
        AdtItems.Columns.Add(New ColumnHeader("Market Item/Group"))
        AdtItems.Nodes.Clear()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT * FROM invMarketGroups ORDER BY parentGroupID;"
            Dim MarketGroupData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If MarketGroupData IsNot Nothing Then
                If MarketGroupData.Tables(0).Rows.Count <> 0 Then
                    Dim TestNode As Node
                    Dim Orphans As New SortedList(Of String, DataRow)
                    For Each row As DataRow In MarketGroupData.Tables(0).Rows
                        If IsDBNull(row.Item("parentGroupID")) Then
                            ' This is a root item
                            Dim MGNode As New Node(row.Item("marketGroupName").ToString)
                            MGNode.Name = row.Item("marketGroupID").ToString
                            AdtItems.Nodes.Add(MGNode)
                        Else
                            TestNode = AdtItems.FindNodeByName(row.Item("parentGroupID").ToString)
                            If TestNode IsNot Nothing Then
                                ' Sub node that has an existing parent
                                Dim MGNode As New Node(row.Item("marketGroupName").ToString)
                                MGNode.Name = row.Item("marketGroupID").ToString
                                TestNode.Nodes.Add(MGNode)
                            Else
                                ' Sub node with no parent as yet
                                Orphans.Add(row.Item("marketGroupID").ToString, row)
                            End If
                        End If
                    Next
                    ' Check orphans
                    Dim lastOrphanCount As Int32
                    Do
                        lastOrphanCount = Orphans.Count
                        Dim RemoveOrphanList As New List(Of String)
                        For Each row As DataRow In Orphans.Values
                            TestNode = AdtItems.FindNodeByName(row.Item("parentGroupID").ToString)
                            If TestNode IsNot Nothing Then
                                ' Sub node that has an existing parent
                                Dim MGNode As New Node(row.Item("marketGroupName").ToString)
                                MGNode.Name = row.Item("marketGroupID").ToString
                                TestNode.Nodes.Add(MGNode)
                                RemoveOrphanList.Add(row.Item("marketGroupID").ToString)
                            Else
                                ' Sub node with no parent as yet
                            End If
                        Next
                        ' Remove orphans
                        For Each Orphan As String In RemoveOrphanList
                            Orphans.Remove(Orphan)
                        Next
                        ' Loop until we remove all the orphans, or we end up in an endless 
                    Loop Until Orphans.Count = 0 Or Orphans.Count = lastOrphanCount

                    ' Finally, add our items
                    For Each TypeID As String In EveHQ.Core.HQ.ItemMarketGroups.Keys
                        TestNode = AdtItems.FindNodeByName(EveHQ.Core.HQ.ItemMarketGroups(TypeID))
                        If TestNode IsNot Nothing Then
                            Dim MGNode As New Node(EveHQ.Core.HQ.itemData(TypeID).Name)
                            MGNode.Name = "i" & TypeID
                            TestNode.Nodes.Add(MGNode)
                        End If
                    Next
                End If
            End If
        Catch e As Exception
        End Try
        EveHQ.Core.AdvTreeSorter.Sort(AdtItems, 1, True, False)
        AdtItems.EndUpdate()
    End Function

    Private Sub UpdatePriceGroupList()
        adtPriceGroups.BeginUpdate()
        adtPriceGroups.Nodes.Clear()
        For Each PG As EveHQ.Core.PriceGroup In EveHQ.Core.HQ.EveHqSettings.PriceGroups.Values
            adtPriceGroups.Nodes.Add(New Node(PG.Name))
        Next
        adtPriceGroups.EndUpdate()
    End Sub

    Private Sub UpdatePriceGroupRegionList()
        adtPriceGroupRegions.BeginUpdate()
        adtPriceGroupRegions.Nodes.Clear()
        For Each R As String In Regions.Keys
            Dim NewRegion As New Node
            NewRegion.CheckBoxVisible = True
            NewRegion.Text = R
            NewRegion.Name = Regions(R).ToString
            adtPriceGroupRegions.Nodes.Add(NewRegion)
        Next
        adtPriceGroupRegions.EndUpdate()
    End Sub

    Private Sub PopulateCheckBoxList()
        PriceGroupFlagBoxes.Clear()
        PriceGroupFlagBoxes.Add(chkPGMinAll)
        PriceGroupFlagBoxes.Add(chkPGMinBuy)
        PriceGroupFlagBoxes.Add(chkPGMinSell)
        PriceGroupFlagBoxes.Add(chkPGMaxAll)
        PriceGroupFlagBoxes.Add(chkPGMaxBuy)
        PriceGroupFlagBoxes.Add(chkPGMaxSell)
        PriceGroupFlagBoxes.Add(chkPGAvgAll)
        PriceGroupFlagBoxes.Add(chkPGAvgBuy)
        PriceGroupFlagBoxes.Add(chkPGAvgSell)
        PriceGroupFlagBoxes.Add(chkPGMedAll)
        PriceGroupFlagBoxes.Add(chkPGMedBuy)
        PriceGroupFlagBoxes.Add(chkPGMedSell)
    End Sub

    Private Sub DisplayPriceGroupDetails(ByVal PriceGroupName As String)
        ' Check if the price group name actually exists
        If EveHQ.Core.HQ.EveHqSettings.PriceGroups.ContainsKey(PriceGroupName) = True Then

            ' Set the update flag
            Me.UpdatePriceGroup = True

            Me.CurrentPriceGroup = EveHQ.Core.HQ.EveHqSettings.PriceGroups(PriceGroupName)

            ' Display the name
            lblSelectedPriceGroup.Text = "Selected Price Group: " & Me.CurrentPriceGroup.Name

            ' Set the regions
            adtPriceGroupRegions.BeginUpdate()
            For Each RegionNode As Node In adtPriceGroupRegions.Nodes
                If Me.CurrentPriceGroup.RegionIDs.Contains(RegionNode.Name) = True Then
                    RegionNode.Checked = True
                Else
                    RegionNode.Checked = False
                End If
            Next
            adtPriceGroupRegions.EndUpdate()

            ' List the items
            Call Me.DisplayPriceGroupItems()

            ' Set the flags
            For Each PGF As CheckBox In Me.PriceGroupFlagBoxes
                If (Me.CurrentPriceGroup.PriceFlags Or CLng(PGF.Tag)) = Me.CurrentPriceGroup.PriceFlags Then
                    PGF.Checked = True
                Else
                    PGF.Checked = False
                End If
            Next

            ' Reset the update flag
            Me.UpdatePriceGroup = False

        End If
    End Sub

    Private Sub DisplayPriceGroupItems()
        ' List the items
        btnDeleteItem.Enabled = False
        adtPriceGroupItems.BeginUpdate()
        adtPriceGroupItems.Nodes.Clear()
        For Each itemID As String In Me.CurrentPriceGroup.TypeIDs
            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                Dim item As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(itemID)
                Dim itemNode As New DevComponents.AdvTree.Node
                itemNode.Text = item.Name
                itemNode.Name = item.ID.ToString
                'If item.Icon <> "" Then
                '    itemNode.Image = EveHQ.Core.ImageHandler.GetImage(item.Icon, EveHQ.Core.ImageHandler.ImageType.Icons, 24)
                'Else
                '    itemNode.Image = EveHQ.Core.ImageHandler.GetImage(item.ID.ToString, EveHQ.Core.ImageHandler.ImageType.Types, 24)
                'End If
                ' Add Node to the list
                adtPriceGroupItems.Nodes.Add(itemNode)
            End If
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtPriceGroupItems, 1, False, True)
        adtPriceGroupItems.EndUpdate()
    End Sub

    Private Sub btnAddPriceGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Create a new instance of the ModifyPriceGroup form
        Dim NewPriceGroup As New frmModifyPriceGroup
        NewPriceGroup.ShowDialog()
        If NewPriceGroup.DialogResult = Windows.Forms.DialogResult.OK Then
            Call Me.UpdatePriceGroupList()
        End If
        ' Dispose of the form
        NewPriceGroup.Dispose()
    End Sub

    Private Sub btnEditPriceGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If adtPriceGroups.SelectedNodes.Count = 1 Then
            If adtPriceGroups.SelectedNodes(0).Text <> "<Global>" Then
                Dim PriceGroupName As String = adtPriceGroups.SelectedNodes(0).Text
                Dim NewPriceGroup As New frmModifyPriceGroup(PriceGroupName)
                NewPriceGroup.ShowDialog()
                If NewPriceGroup.DialogResult = Windows.Forms.DialogResult.OK Then
                    Call Me.UpdatePriceGroupList()
                End If
                Dim NewPriceGroupName As String = NewPriceGroup.NewPriceGroupName
                Dim NewNode As Node = adtPriceGroups.FindNodeByText(NewPriceGroupName)
                If NewNode IsNot Nothing Then
                    adtPriceGroups.SelectedNode = NewNode
                End If
                ' Dispose of the form
                NewPriceGroup.Dispose()
            Else
                MessageBox.Show("You can't edit the Global Price Group", "Edit Denied", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("You must select a Price Group to edit before trying to edit it!", "Price Group Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub btnDeletePriceGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If adtPriceGroups.SelectedNodes.Count = 1 Then
            If adtPriceGroups.SelectedNodes(0).Text <> "<Global>" Then
                Dim reply As DialogResult = MessageBox.Show("Are you sure you want to delete the selected Price Group?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = Windows.Forms.DialogResult.No Then
                    Exit Sub
                Else
                    Dim PriceGroupName As String = adtPriceGroups.SelectedNodes(0).Text
                    If EveHQ.Core.HQ.EveHqSettings.PriceGroups.ContainsKey(PriceGroupName) Then
                        EveHQ.Core.HQ.EveHqSettings.PriceGroups.Remove(PriceGroupName)
                        Call Me.UpdatePriceGroupList()
                    End If
                End If
            Else
                MessageBox.Show("You can't delete the Global Price Group", "Delete Denied", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("You must select a Price Group to edit before trying to delete it!", "Price Group Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub adtPriceGroups_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If adtPriceGroups.SelectedNodes.Count = 0 Then
            ' Set the buttons
            btnAddPriceGroup.Enabled = True
            btnEditPriceGroup.Enabled = False
            btnDeletePriceGroup.Enabled = False
            gpSelectedPriceGroup.Enabled = False
        Else
            ' Set the buttons
            btnAddPriceGroup.Enabled = True
            btnEditPriceGroup.Enabled = True
            btnDeletePriceGroup.Enabled = True
            gpSelectedPriceGroup.Enabled = True
            ' Display the Price Group details
            Call Me.DisplayPriceGroupDetails(adtPriceGroups.SelectedNodes(0).Text)
        End If
    End Sub

    Private Sub PriceGroupFlagCheckBoxHandler(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
     chkPGMinAll.CheckedChanged,
     chkPGMinBuy.CheckedChanged,

        chkPGMaxAll.CheckedChanged,
        chkPGMaxBuy.CheckedChanged,
        chkPGMaxSell.CheckedChanged,
        chkPGAvgAll.CheckedChanged,
        chkPGAvgBuy.CheckedChanged,
        chkPGAvgSell.CheckedChanged,
        chkPGMedAll.CheckedChanged,
        chkPGMedBuy.CheckedChanged,
        chkPGMedSell.CheckedChanged

        If Me.UpdatePriceGroup = False Then

            ' Get the checkbox
            Dim selCB As CheckBox = CType(sender, CheckBox)

            ' Determine checkbox state and perform a logical OR/XOR operation on the CurrentPriceGroup.PriceFlag value
            If selCB.Checked = True Then
                CurrentPriceGroup.PriceFlags = CType((CurrentPriceGroup.PriceFlags Or CLng(selCB.Tag)), Core.PriceGroupFlags)
            Else
                CurrentPriceGroup.PriceFlags = CType((CurrentPriceGroup.PriceFlags Xor CLng(selCB.Tag)), Core.PriceGroupFlags)
            End If

        End If

    End Sub

    Private Sub adtPriceGroupItems_SelectionChanged(sender As Object, e As System.EventArgs)
        If adtPriceGroupItems.SelectedNodes.Count > 0 Then
            btnDeleteItem.Enabled = True
        Else
            btnDeleteItem.Enabled = False
        End If
    End Sub

    Private Sub btnAddItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Add a new item
        If CurrentPriceGroup.Name <> "<Global>" Then
            Dim NewPrice As New frmModifyPriceGroupItem(AdtItems, EveHQ.Core.HQ.ItemMarketGroups)
            Dim ItemUsed As Boolean = False
            NewPrice.ShowDialog()
            If NewPrice.SelectedItems IsNot Nothing Then
                If NewPrice.SelectedItems.Count > 0 Then
                    For Each ItemID As String In NewPrice.SelectedItems.Values
                        ItemUsed = False
                        For Each PG As EveHQ.Core.PriceGroup In EveHQ.Core.HQ.EveHqSettings.PriceGroups.Values
                            If PG.TypeIDs.Contains(ItemID) Then
                                ItemUsed = True
                                Exit For
                            End If
                        Next
                        If ItemUsed = False Then
                            CurrentPriceGroup.TypeIDs.Add(ItemID)
                        End If
                    Next
                    Call Me.DisplayPriceGroupItems()
                End If
            End If
            NewPrice.Dispose()
        Else
            MessageBox.Show("You cannot add prices to the Global Price Group. This group is reserved for items which aren't covered by any other Price Groups.", "Item Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnDeleteItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If adtPriceGroupItems.SelectedNodes.Count = 0 Then
            MessageBox.Show("You must select at least one item before trying to delete it!", "Price Group Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            For Each SNode As Node In adtPriceGroupItems.SelectedNodes
                CurrentPriceGroup.TypeIDs.Remove(SNode.Name)
            Next
            Call Me.DisplayPriceGroupItems()
        End If
    End Sub

    Private Sub btnClearItems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Confirm the clearing of the current items
        Dim reply As DialogResult = MessageBox.Show("Are you sure you want to clear all items in this Price Group?", "Confirm Clear Items", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            CurrentPriceGroup.TypeIDs.Clear()
            Call Me.DisplayPriceGroupItems()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub adtPriceGroupRegions_AfterCheck(ByVal sender As Object, ByVal e As DevComponents.AdvTree.AdvTreeCellEventArgs)

        ' Only perform this if not updating the Price Group
        If UpdatePriceGroup = False Then

            If e.Cell.Checked = True Then

                ' Add the EveGalaticRegion to the list
                If CurrentPriceGroup.RegionIDs.Contains(e.Cell.Parent.Name) = False Then
                    CurrentPriceGroup.RegionIDs.Add(e.Cell.Parent.Name)
                End If

            Else

                ' Remove the item
                If CurrentPriceGroup.RegionIDs.Contains(e.Cell.Parent.Name) = True Then
                    CurrentPriceGroup.RegionIDs.Remove(e.Cell.Parent.Name)
                End If

            End If

        End If

    End Sub

#End Region

    '#Region "Market UI Routines"

    '    Private Sub btnDownloadMarketPrices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        btnDownloadMarketPrices.Enabled = False
    '        btnUpdateMarketPrices.Enabled = False
    '        MarketDownloadWorker.WorkerReportsProgress = True
    '        MarketDownloadWorker.RunWorkerAsync()
    '    End Sub
    '    Private Sub btnUpdateMarketPrices_Click(sender As System.Object, e As System.EventArgs)
    '        btnDownloadMarketPrices.Enabled = False
    '        btnUpdateMarketPrices.Enabled = False
    '        MarketUpdateWorker.WorkerReportsProgress = True
    '        MarketUpdateWorker.RunWorkerAsync()
    '    End Sub
    '    Private Sub lblBattleclinicLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
    '        Try
    '            Process.Start("http://www.battleclinic.com")
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try
    '    End Sub
    '    Private Sub lblEveMarketeerLink_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
    '        Try
    '            Process.Start("http://www.evemarketeer.com")
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try
    '    End Sub
    '    Private Sub radBattleclinic_CheckedChanged(sender As System.Object, e As System.EventArgs)
    '        If startUp = False Then
    '            EveHQ.Core.HQ.EveHqSettings.MarketDataSource = MarketSite.Battleclinic
    '            Call Me.UpdateCacheFileList()
    '        End If
    '    End Sub

    '    Private Sub radEveMarketeer_CheckedChanged(sender As System.Object, e As System.EventArgs)
    '        If startUp = False Then
    '            EveHQ.Core.HQ.EveHqSettings.MarketDataSource = MarketSite.EveMarketeer
    '            Call Me.UpdateCacheFileList()
    '        End If
    '    End Sub

    '    Private Sub UpdateCacheFileList()
    '        Dim FeedName As String = ""
    '        Select Case EveHQ.Core.HQ.EveHqSettings.MarketDataSource
    '            Case MarketSite.Battleclinic
    '                FeedName = "MarketPrices"
    '            Case MarketSite.EveMarketeer
    '                FeedName = "EveMarketeerPrices"
    '        End Select
    '        ' Get the contents of the market log folder and display them
    '        adtMarketCache.BeginUpdate()
    '        adtMarketCache.Nodes.Clear()
    '        If My.Computer.FileSystem.DirectoryExists(marketCacheFolder) = True Then
    '            For Each file As String In My.Computer.FileSystem.GetFiles(marketCacheFolder, FileIO.SearchOption.SearchTopLevelOnly, FeedName & "*.xml")
    '                ' Get EveGalaticRegion details
    '                Dim FI As New FileInfo(file)
    '                ' Bug 44 (http://issues.evehq.net/thebuggenie/evehq/issues/44): hardening the parsing of cache files from manual modifications that would make them invalid to parse.
    '                ' Using TryParse instead of casting/converting.
    '                Dim regionID As Long
    '                If Long.TryParse(FI.Name.TrimStart(FeedName.ToCharArray).TrimEnd(".xml".ToCharArray), regionID) And RegionNames.ContainsKey(regionID) = True Then
    '                    Dim regionName As String = RegionNames(regionID)
    '                    Dim RegionNode As New Node(regionName)
    '                    RegionNode.Cells.Add(New Cell(FI.LastWriteTime.ToString))
    '                    ' Get the cache file time
    '                    Dim MarketFileCacheTime As DateTime = GetMarketFileCacheTime(file)
    '                    RegionNode.Cells.Add(New Cell(GetMarketFileCacheTime(file).ToString))
    '                    adtMarketCache.Nodes.Add(RegionNode)
    '                End If
    '            Next
    '            ' Check the results
    '            If adtMarketCache.Nodes.Count > 0 Then
    '                adtMarketCache.Enabled = True
    '            Else
    '                adtMarketCache.Nodes.Add(New Node("Cache Directory Empty"))
    '                adtMarketCache.Enabled = False
    '            End If
    '        Else
    '            adtMarketCache.Nodes.Add(New Node("Cache Directory Not Available"))
    '            adtMarketCache.Enabled = False
    '        End If
    '        adtMarketCache.EndUpdate()
    '    End Sub

    '    Private Function GetMarketFileCacheTime(Filename As String) As DateTime

    '        Dim PriceXML As New XmlDocument
    '        Try
    '            PriceXML.Load(Filename)
    '            Dim CacheNodeList As XmlNodeList = PriceXML.GetElementsByTagName("cacheExpires")
    '            If CacheNodeList.Count > 0 Then
    '                Dim CacheNode As XmlNode = CacheNodeList(0)
    '                Dim RawTime As String = CacheNode.InnerText
    '                Dim ActualTime As DateTime = DateTime.Parse(RawTime)
    '                Return ActualTime
    '            Else
    '                Return Now.AddDays(-1)
    '            End If
    '        Catch ex As Exception
    '            ' Error loading in XML file, probably due to corrupt format, allow a new XML file
    '            Return Now.AddDays(-1)
    '        End Try

    '    End Function

    '    'Private Sub ParseMarketPriceGroup(ByRef GlobalPriceData As SortedList(Of String, SortedList(Of String, MarketData)), ByRef UsedPriceList As List(Of String), MPG As EveHQ.Core.PriceGroup)

    '    '    Dim MarketPrice As Double = 0
    '    '    Dim PriceCount As Integer = 0
    '    '    Dim PriceFlagList As New List(Of EveHQ.Core.PriceGroupFlags)
    '    '    Dim RegionPrices As New SortedList(Of String, MarketData)
    '    '    Dim ItemPrices As New MarketData

    '    '    ' Establish which price criteria we should be using
    '    '    Dim PFS As Integer = MPG.PriceFlags
    '    '    PriceFlagList.Clear()
    '    '    For i As Integer = 0 To 30
    '    '        If (PFS Or CInt(2 ^ i)) = PFS Then
    '    '            PriceFlagList.Add(CType(CInt(2 ^ i), Core.PriceGroupFlags))
    '    '        End If
    '    '    Next

    '    '    ' Get each itemID
    '    '    For Each ItemID As String In MPG.TypeIDs
    '    '        ' Set the market price and price count to nil
    '    '        MarketPrice = 0
    '    '        PriceCount = 0

    '    '        ' Go through each EveGalaticRegion and apply the correct prices
    '    '        For Each RegionID As String In MPG.RegionIDs

    '    '            If GlobalPriceData.ContainsKey(RegionID) Then
    '    '                RegionPrices = GlobalPriceData(RegionID)

    '    '                If RegionPrices.ContainsKey(ItemID) = True Then

    '    '                    ItemPrices = RegionPrices(ItemID)

    '    '                    For Each PF As EveHQ.Core.PriceGroupFlags In PriceFlagList
    '    '                        ' Determine what we do here!
    '    '                        Select Case PF
    '    '                            Case Core.PriceGroupFlags.MinAll
    '    '                                If ItemPrices.MinAll <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MinAll
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.MinBuy
    '    '                                If ItemPrices.MinBuy <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MinBuy
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.MinSell
    '    '                                If ItemPrices.MinSell <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MinSell
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.MaxAll
    '    '                                If ItemPrices.MaxAll <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MaxAll
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.MaxBuy
    '    '                                If ItemPrices.MaxBuy <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MaxBuy
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.MaxSell
    '    '                                If ItemPrices.MaxSell <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MaxSell
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.AvgAll
    '    '                                If ItemPrices.AvgAll <> 0 Then
    '    '                                    MarketPrice += ItemPrices.AvgAll
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.AvgBuy
    '    '                                If ItemPrices.AvgBuy <> 0 Then
    '    '                                    MarketPrice += ItemPrices.AvgBuy
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.AvgSell
    '    '                                If ItemPrices.AvgSell <> 0 Then
    '    '                                    MarketPrice += ItemPrices.AvgSell
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.MedAll
    '    '                                If ItemPrices.MedAll <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MedAll
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.MedBuy
    '    '                                If ItemPrices.MedBuy <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MedBuy
    '    '                                    PriceCount += 1
    '    '                                End If
    '    '                            Case Core.PriceGroupFlags.MedSell
    '    '                                If ItemPrices.MedSell <> 0 Then
    '    '                                    MarketPrice += ItemPrices.MedSell
    '    '                                    PriceCount += 1
    '    '                                End If

    '    '                        End Select

    '    '                    Next

    '    '                End If

    '    '            End If

    '    '        Next

    '    '        ' Calculate an average of the prices if we need to
    '    '        If PriceCount > 0 Then
    '    '            MarketPrice = Math.Round(MarketPrice / PriceCount, 2)

    '    '            ' Set the price
    '    '            EveHQ.Core.DataFunctions.SetMarketPrice(CLng(ItemID), MarketPrice, True)
    '    '        End If

    '    '        ' Add the price to the used list
    '    '        UsedPriceList.Add(ItemID)

    '    '    Next

    '    'End Sub

    '#End Region

    '#Region "Market & Faction Price Feed Routines"

    '    Private Function GetBCPriceFeed(ByVal FeedName As String, ByVal URL As String, ByVal StatusLabel As Label, ByVal RegionID As String, ByVal SuppressProgress As Boolean, ByVal RegionTotal As Integer, ByVal RegionCount As Integer) As Boolean

    '        ' Set a default policy level for the "http:" and "https" schemes.
    '        Dim policy As System.Net.Cache.HttpRequestCachePolicy = New System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)

    '        ' Create the request to access the server and set credentials
    '        If SuppressProgress = False Then
    '            StatusLabel.Text = "Setting '" & FeedName & "' Server Address..." : StatusLabel.Refresh()
    '        End If

    '        ' Build URL
    '        URL &= RegionID & ".xml.gz"

    '        Dim localfile As String = Path.Combine(marketCacheFolder, FeedName & RegionID & ".xml.gz")
    '        ServicePointManager.DefaultConnectionLimit = 10
    '        ServicePointManager.Expect100Continue = False
    '        Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(URL))
    '        Dim request As HttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)
    '        request.CachePolicy = policy
    '        ' Setup proxy server (if required)
    '        Call EveHQ.Core.ProxyServerFunctions.SetupWebProxy(request)
    '        request.UserAgent = "EveHQ v" & My.Application.Info.Version.ToString

    '        ' New File Method
    '        request.Method = "GET"
    '        request.ContentType = "application/x-www-form-urlencoded"
    '        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")

    '        ' Old API Method
    '        'request.Method = "POST"
    '        'Dim postData As String = "applicationKey=" & EveHQ.Core.HQ.BCAppKey & "&regions=" & RegionID & "&types=0"
    '        'request.ContentLength = postData.Length
    '        'request.ContentType = "application/x-www-form-urlencoded"
    '        'request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
    '        '' Setup a stream to write the HTTP "POST" data
    '        'Dim WebEncoding As New ASCIIEncoding()
    '        'Dim byte1 As Byte() = WebEncoding.GetBytes(postData)
    '        'Dim newStream As Stream = request.GetRequestStream()
    '        'newStream.Write(byte1, 0, byte1.Length)
    '        'newStream.Close()
    '        'newStream.Dispose()

    '        Try
    '            If SuppressProgress = False Then
    '                StatusLabel.Text = "Contacting '" & FeedName & "' Server..." : StatusLabel.Refresh()
    '            End If
    '            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
    '                Dim filesize As Long = CLng(response.ContentLength)
    '                'Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
    '                Using responseStream As IO.Stream = response.GetResponseStream
    '                    'loop to read & write to file
    '                    Using fs As New IO.FileStream(localfile, IO.FileMode.Create)
    '                        Dim buffer(32767) As Byte
    '                        Dim read As Integer = 0
    '                        Dim totalBytes As Long = 0
    '                        Dim percent As Integer = 0
    '                        Do
    '                            read = responseStream.Read(buffer, 0, buffer.Length)
    '                            fs.Write(buffer, 0, read)
    '                            totalBytes += read
    '                            If filesize <> -1 Then
    '                                percent = CInt(totalBytes / filesize * 100)
    '                                If SuppressProgress = False Then
    '                                    StatusLabel.Text = "Downloading File " & RegionCount.ToString & " of " & RegionTotal.ToString & ": '" & RegionNames(CLng(RegionID)) & "'... " & totalBytes.ToString("N0") & " of " & filesize.ToString("N0") & " (" & percent & "%)" : StatusLabel.Refresh()
    '                                End If
    '                            Else
    '                                If SuppressProgress = False Then
    '                                    StatusLabel.Text = "Downloading File " & RegionCount.ToString & " of " & RegionTotal.ToString & ": '" & RegionNames(CLng(RegionID)) & "'... " & totalBytes.ToString("N0") & " of unknown size" : StatusLabel.Refresh()
    '                                End If
    '                            End If
    '                            Application.DoEvents()
    '                        Loop Until read = 0 'see Note(1)
    '                        responseStream.Close()
    '                        fs.Flush()
    '                        fs.Close()
    '                    End Using
    '                    responseStream.Close()
    '                End Using
    '                response.Close()
    '            End Using
    '            If SuppressProgress = False Then
    '                StatusLabel.Text = "Download of '" & FeedName & "' Complete!" : StatusLabel.Refresh()
    '            End If
    '            ' Unzip the file
    '            Dim ufs As FileStream = New FileStream(localfile, FileMode.Open, FileAccess.Read)
    '            Dim compstream As New System.IO.Compression.GZipStream(ufs, System.IO.Compression.CompressionMode.Decompress)
    '            Dim sr As New StreamReader(compstream)
    '            Dim unzipfile As String = Path.Combine(marketCacheFolder, FeedName & RegionID & ".xml")
    '            Dim sw As New StreamWriter(unzipfile)
    '            sw.Write(sr.ReadToEnd)
    '            sw.Flush()
    '            sw.Close()
    '            sw.Dispose()
    '            sr.Close()
    '            sr.Dispose()
    '            ufs.Close()
    '            ufs.Dispose()
    '            Return True
    '        Catch ex As Exception
    '            ' Suppress this message for now - just return
    '            'MessageBox.Show("There was an error downloading the '" & FeedName & "' data: " & ex.Message, "Error in Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return False
    '        End Try

    '    End Function

    '    Private Sub GetBCMarketPrices()

    '        ' Setup variables
    '        Dim PriceRegions As New List(Of String)
    '        Dim FeedName As String = "MarketPrices"

    '        ' Step 1: Determine which regions we need from the price groups
    '        For Each PG As EveHQ.Core.PriceGroup In EveHQ.Core.HQ.EveHqSettings.PriceGroups.Values
    '            For Each Region As String In PG.RegionIDs
    '                If PriceRegions.Contains(Region) = False Then
    '                    PriceRegions.Add(Region)
    '                End If
    '            Next
    '        Next

    '        ' Step 2: Cycle through each EveGalaticRegion and check the cache time to see if it needs an update
    '        Dim TotalRegions As Integer = PriceRegions.Count
    '        Dim RegionCount As Integer = 0
    '        For Each Region As String In PriceRegions

    '            RegionCount += 1

    '            ' Update the status
    '            MarketDownloadWorker.ReportProgress(100, "Status: Checking EveGalaticRegion: " & RegionNames(CLng(Region)) & " (" & RegionCount.ToString & " of " & TotalRegions.ToString & ")")

    '            ' Check to see if we have an existing XML file that should be cached
    '            Dim localfile As String = Path.Combine(marketCacheFolder, FeedName & Region & ".xml")
    '            Dim DownloadRequired As Boolean = True
    '            If My.Computer.FileSystem.FileExists(localfile) = True Then
    '                ' Load the file
    '                Dim PriceXML As New XmlDocument
    '                Try
    '                    PriceXML.Load(localfile)
    '                    ' Check the Cache details
    '                    Dim CacheNodes As XmlNodeList = PriceXML.SelectNodes("/GetCalculatedPrice/cacheExpires")
    '                    If CacheNodes.Count > 0 Then
    '                        Dim CacheDate As Date
    '                        If Date.TryParse(CacheNodes(0).InnerText, CacheDate) = True Then
    '                            If Date.Compare(Now.ToUniversalTime, CacheDate.ToUniversalTime) < 0 Then
    '                                DownloadRequired = False
    '                            End If
    '                        End If
    '                    End If
    '                Catch e As Exception
    '                    ' Catch cases of corrupt XML files and re-download
    '                    DownloadRequired = True
    '                End Try
    '            End If

    '            ' Download the correct data file
    '            If DownloadRequired = True Then
    '                Call GetBCPriceFeed("MarketPrices", "http://eve.battleclinic.com/xml/market_data/", lblMarketPriceUpdateStatus, Region, False, TotalRegions, RegionCount)
    '            End If

    '        Next

    '        ' Update the status
    '        MarketDownloadWorker.ReportProgress(100, "Download of Market Pricing Data complete!")

    '    End Sub

    '    Private Sub ParseBCPrices(WriteToDB As Boolean)
    '        ' New method of how we should roll!
    '        Dim EveCulture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    '        Dim EveDateFormat As String = "yyyy-MM-dd"

    '        Dim PriceRegions As New List(Of String)
    '        Dim FeedName As String = "MarketPrices"

    '        ' Step 1: Create a list of all market price items
    '        lblMarketPriceUpdateStatus.Text = "Establishing Market item list..." : lblMarketPriceUpdateStatus.Refresh()
    '        Dim MarketItems As New List(Of String)
    '        For Each Item As String In EveHQ.Core.HQ.ItemMarketGroups.Keys
    '            MarketItems.Add(Item)
    '        Next

    '        ' Step 2: Establish which regions we need to parse
    '        lblMarketPriceUpdateStatus.Text = "Determining Market regions..." : lblMarketPriceUpdateStatus.Refresh()
    '        For Each Region As String In EveHQ.Core.HQ.EveHqSettings.MarketRegionList
    '            PriceRegions.Add(Region)
    '        Next
    '        For Each PG As EveHQ.Core.PriceGroup In EveHQ.Core.HQ.EveHqSettings.PriceGroups.Values
    '            For Each Region As String In PG.RegionIDs
    '                If PriceRegions.Contains(Region) = False Then
    '                    PriceRegions.Add(Region)
    '                End If
    '            Next
    '        Next

    '        ' Step 3: Check we have all files
    '        lblMarketPriceUpdateStatus.Text = "Downloading missing EveGalaticRegion files..." : lblMarketPriceUpdateStatus.Refresh()
    '        For Each Region As String In PriceRegions
    '            ' Check if the file exists and download it if not
    '            ' We are not interested at this stage about out of date files
    '            Dim localfile As String = Path.Combine(marketCacheFolder, FeedName & Region & ".xml")
    '            If My.Computer.FileSystem.FileExists(localfile) = False Then
    '                ' Download the correct data file
    '                Call GetBCPriceFeed("MarketPrices", "http://api.battleclinic.com/xml/eve/market/GetCalculatedCurrentValue.php", lblMarketPriceUpdateStatus, Region, False, 1, 1)
    '            End If
    '        Next

    '        ' Step 4: Parse the XML files
    '        lblMarketPriceUpdateStatus.Text = "Parsing regional XML files..." : lblMarketPriceUpdateStatus.Refresh()
    '        Dim GlobalPriceData As New SortedList(Of String, SortedList(Of String, MarketData))
    '        For Each Region As String In PriceRegions

    '            ' Create a new set of regional data
    '            Dim RegionData As New SortedList(Of String, MarketData)
    '            GlobalPriceData.Add(Region, RegionData)

    '            ' Load the XML
    '            Dim localfile As String = Path.Combine(marketCacheFolder, FeedName & Region & ".xml")
    '            Dim PriceXML As New XmlDocument
    '            Try
    '                PriceXML.Load(localfile)

    '                ' Get the records
    '                Dim PriceNodes As XmlNodeList = PriceXML.SelectNodes("/GetCalculatedPrice/records/record")
    '                If PriceNodes.Count > 0 Then
    '                    For Each PriceNode As XmlNode In PriceNodes
    '                        Dim itemID As String = PriceNode.Attributes.GetNamedItem("itemID").Value
    '                        Dim NewPriceData As New MarketData
    '                        If RegionData.ContainsKey(itemID) = True Then
    '                            NewPriceData = RegionData(itemID)
    '                        Else
    '                            NewPriceData.ItemID = PriceNode.Attributes.GetNamedItem("itemID").Value
    '                            NewPriceData.RegionID = PriceNode.Attributes.GetNamedItem("regionID").Value
    '                            NewPriceData.HistoryDate = PriceNode.Attributes.GetNamedItem("historyDate").Value
    '                            RegionData.Add(itemID, NewPriceData)
    '                        End If
    '                        Select Case PriceNode.Attributes.GetNamedItem("type").Value
    '                            Case "all"
    '                                NewPriceData.MinAll = Double.Parse(PriceNode.Attributes.GetNamedItem("lowPrice").Value, EveCulture)
    '                                NewPriceData.MaxAll = Double.Parse(PriceNode.Attributes.GetNamedItem("highPrice").Value, EveCulture)
    '                                NewPriceData.AvgAll = Double.Parse(PriceNode.Attributes.GetNamedItem("avgPrice").Value, EveCulture)
    '                                NewPriceData.MedAll = Double.Parse(PriceNode.Attributes.GetNamedItem("medianPrice").Value, EveCulture)
    '                                NewPriceData.StdAll = Double.Parse(PriceNode.Attributes.GetNamedItem("standardDeviation").Value, EveCulture)
    '                                NewPriceData.VarAll = Double.Parse(PriceNode.Attributes.GetNamedItem("variance").Value, EveCulture)
    '                                NewPriceData.VolAll = Double.Parse(PriceNode.Attributes.GetNamedItem("volume").Value, EveCulture)
    '                                NewPriceData.QtyAll = Double.Parse(PriceNode.Attributes.GetNamedItem("orders").Value, EveCulture)
    '                            Case "buy"
    '                                NewPriceData.MinBuy = Double.Parse(PriceNode.Attributes.GetNamedItem("lowPrice").Value, EveCulture)
    '                                NewPriceData.MaxBuy = Double.Parse(PriceNode.Attributes.GetNamedItem("highPrice").Value, EveCulture)
    '                                NewPriceData.AvgBuy = Double.Parse(PriceNode.Attributes.GetNamedItem("avgPrice").Value, EveCulture)
    '                                NewPriceData.MedBuy = Double.Parse(PriceNode.Attributes.GetNamedItem("medianPrice").Value, EveCulture)
    '                                NewPriceData.StdBuy = Double.Parse(PriceNode.Attributes.GetNamedItem("standardDeviation").Value, EveCulture)
    '                                NewPriceData.VarBuy = Double.Parse(PriceNode.Attributes.GetNamedItem("variance").Value, EveCulture)
    '                                NewPriceData.VolBuy = Double.Parse(PriceNode.Attributes.GetNamedItem("volume").Value, EveCulture)
    '                                NewPriceData.QtyBuy = Double.Parse(PriceNode.Attributes.GetNamedItem("orders").Value, EveCulture)
    '                            Case "sell"
    '                                NewPriceData.MinSell = Double.Parse(PriceNode.Attributes.GetNamedItem("lowPrice").Value, EveCulture)
    '                                NewPriceData.MaxSell = Double.Parse(PriceNode.Attributes.GetNamedItem("highPrice").Value, EveCulture)
    '                                NewPriceData.AvgSell = Double.Parse(PriceNode.Attributes.GetNamedItem("avgPrice").Value, EveCulture)
    '                                NewPriceData.MedSell = Double.Parse(PriceNode.Attributes.GetNamedItem("medianPrice").Value, EveCulture)
    '                                NewPriceData.StdSell = Double.Parse(PriceNode.Attributes.GetNamedItem("standardDeviation").Value, EveCulture)
    '                                NewPriceData.VarSell = Double.Parse(PriceNode.Attributes.GetNamedItem("variance").Value, EveCulture)
    '                                NewPriceData.VolSell = Double.Parse(PriceNode.Attributes.GetNamedItem("volume").Value, EveCulture)
    '                                NewPriceData.QtySell = Double.Parse(PriceNode.Attributes.GetNamedItem("orders").Value, EveCulture)
    '                        End Select
    '                    Next
    '                End If
    '            Catch e As Exception
    '                ' Report a message - probably due to bad format or corruption
    '                Dim msg As New StringBuilder
    '                msg.AppendLine("There was an error parsing the market price file for " & Region & ".")
    '                msg.AppendLine("")
    '                msg.AppendLine("The error was: " & e.Message)
    '                msg.AppendLine("Stacktrace: " & e.StackTrace)
    '                msg.AppendLine("")
    '                msg.AppendLine("It is highly likely that the file is corrupt so re-downloading the file may resolve the issue.")
    '                MessageBox.Show(msg.ToString, "Error Parsing Price File", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            End Try
    '        Next

    '        ' Step 5: Open a connection to the database
    '        If EveHQ.Core.DataFunctions.OpenCustomDatabase = True Then

    '            ' Step 6: Go through the price groups and apply the prices to them
    '            ' Make a note of which item we have used here so we can apply general prices to everything else
    '            Dim UsedPriceList As New SortedList(Of String, Double)

    '            For Each MPG As EveHQ.Core.PriceGroup In EveHQ.Core.HQ.EveHqSettings.PriceGroups.Values

    '                If MPG.Name <> "<Global>" Then

    '                    lblMarketPriceUpdateStatus.Text = "Updating '" & MPG.Name & "' Price Group..." : lblMarketPriceUpdateStatus.Refresh()
    '                    Call EveHQ.Core.MarketFunctions.ParseMarketPriceGroup(GlobalPriceData, UsedPriceList, MPG, WriteToDB)

    '                End If

    '            Next

    '            ' Step 7: See which items are left and apply prices to them - temporarily add them to the <Global> group
    '            If EveHQ.Core.HQ.EveHqSettings.PriceGroups.ContainsKey("<Global>") = True Then

    '                Dim MPG As EveHQ.Core.PriceGroup = EveHQ.Core.HQ.EveHqSettings.PriceGroups("<Global>")
    '                For Each ItemID As String In EveHQ.Core.HQ.ItemMarketGroups.Keys
    '                    If UsedPriceList.ContainsKey(ItemID) = False Then
    '                        MPG.TypeIDs.Add(ItemID)
    '                    End If
    '                Next

    '                lblMarketPriceUpdateStatus.Text = "Updating '" & MPG.Name & "' Price Group..." : lblMarketPriceUpdateStatus.Refresh()
    '                Call EveHQ.Core.MarketFunctions.ParseMarketPriceGroup(GlobalPriceData, UsedPriceList, MPG, WriteToDB)

    '                ' Clear the typeIDs
    '                MPG.TypeIDs.Clear()

    '            End If

    '            ' Step 8: Close the database
    '            EveHQ.Core.DataFunctions.CloseCustomDatabase()

    '            ' Step 9: Update the price matrix
    '            lblMarketPriceUpdateStatus.Text = "Updating Price Matrix..." : lblMarketPriceUpdateStatus.Refresh()
    '            Call Me.UpdatePriceMatrix()

    '        End If

    '        ' Update the status
    '        lblMarketPriceUpdateStatus.Text = "Update of Market Pricing Data complete!" : lblMarketPriceUpdateStatus.Refresh()

    '    End Sub

    '#End Region


#Region "Market Worker Routines"

    'Private Sub MarketDownloadWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MarketDownloadWorker.DoWork
    '    Select Case EveHQ.Core.HQ.EveHqSettings.MarketDataSource
    '        Case MarketSite.Battleclinic
    '            Call Me.GetBCMarketPrices()
    '        Case MarketSite.EveMarketeer
    '            Call Me.GetEveMarketeerPrices()
    '    End Select
    'End Sub

    'Private Sub MarketDownloadWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles MarketDownloadWorker.ProgressChanged
    '    Dim StatusText As String = e.UserState.ToString
    '    lblMarketPriceUpdateStatus.Text = StatusText : lblMarketPriceUpdateStatus.Refresh()
    'End Sub

    'Private Sub MarketDownloadWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles MarketDownloadWorker.RunWorkerCompleted
    '    btnDownloadMarketPrices.Enabled = False
    '    btnUpdateMarketPrices.Enabled = False
    '    UpdateCacheFileList()
    '    MarketUpdateWorker.WorkerReportsProgress = True
    '    MarketUpdateWorker.RunWorkerAsync()
    'End Sub

    'Private Sub MarketUpdateWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MarketUpdateWorker.DoWork
    '    Select Case EveHQ.Core.HQ.EveHqSettings.MarketDataSource
    '        Case MarketSite.Battleclinic
    '            Call Me.ParseBCPrices(True)
    '        Case MarketSite.EveMarketeer
    '            Call Me.ParseEveMarketeerPrices(True)
    '    End Select
    'End Sub

    'Private Sub MarketUpdateWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles MarketUpdateWorker.ProgressChanged
    '    Dim StatusText As String = e.UserState.ToString
    '    lblMarketPriceUpdateStatus.Text = StatusText : lblMarketPriceUpdateStatus.Refresh()
    'End Sub

    'Private Sub MarketUpdateWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles MarketUpdateWorker.RunWorkerCompleted
    '    btnDownloadMarketPrices.Enabled = True
    '    btnUpdateMarketPrices.Enabled = True
    'End Sub

#End Region

    Private Sub OnGetMarketOrdersClick(sender As System.Object, e As System.EventArgs) Handles _getMarketOrders.Click
        ' check that an item is selected in the drop down
        If _itemsList.SelectedItem Is Nothing Then
            MessageBox.Show("You must first select an item from the drop down list before retrieving market information")
            Return
        End If

        'get the itemid needed for retrieval
        Dim itemId As Integer = (From items In Core.HQ.itemData.Values Where items.Name = CStr(_itemsList.SelectedItem) Select CInt(items.ID)).FirstOrDefault()

        Dim orderTask As Task(Of ItemMarketOrders)
        If (HQ.EveHqSettings.MarketUseRegionMarket) = True Then
            orderTask = HQ.MarketDataProvider.GetMarketOrdersForItemType(itemId, HQ.EveHqSettings.MarketRegions, Nothing, 1)
        Else
            orderTask = HQ.MarketDataProvider.GetMarketOrdersForItemType(itemId, Nothing, HQ.EveHqSettings.MarketSystem, 1)
        End If

        Dim cont As Action(Of Task(Of ItemMarketOrders)) = Sub(dataTask As Task(Of ItemMarketOrders))

                                                               If dataTask.IsCanceled = False And dataTask.IsFaulted = False And dataTask.Result IsNot Nothing Then
                                                                   Me.Invoke(Sub() UpdateMarketDisplayWithNewData(dataTask.Result))
                                                                   'TODO: this is where display of an error message should go.
                                                               End If
                                                               'Return result
                                                           End Sub

        orderTask.ContinueWith(cont)

    End Sub

    ' updates the market tables with new data
    Private Sub UpdateMarketDisplayWithNewData(ByVal data As ItemMarketOrders)
        _sellOrders.Nodes.Clear()
        _buyOrders.Nodes.Clear()
        For Each order As MarketOrder In data.SellOrders
            Dim row As New Node
            row.Text = order.StationName
            row.Cells.Add(New Cell(order.QuantityRemaining.ToInvariantString))
            row.Cells.Add(New Cell(order.Price.ToString("N2")))
            row.Cells.Add(New Cell(order.Expires.ToString()))
            _sellOrders.Nodes.Add(row)
        Next
        For Each order As MarketOrder In data.BuyOrders
            Dim row As New Node
            row.Text = order.StationName
            row.Cells.Add(New Cell(order.QuantityRemaining.ToInvariantString))
            row.Cells.Add(New Cell(order.Price.ToString("N2")))
            row.Cells.Add(New Cell(order.Expires.ToString()))
            _buyOrders.Nodes.Add(row)
        Next

    End Sub

End Class

