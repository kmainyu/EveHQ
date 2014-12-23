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

Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Controls
Imports EveHQ.Core
Imports EveHQ.EveData
Imports EveHQ.Prism.Classes

Namespace Forms

    Public Class FrmPrismSettings
        Dim _startUp As Boolean = True
        Dim _redrawColumns As Boolean = True
        ReadOnly _corpRepCombos As New List(Of ComboBoxEx)
        ReadOnly _corpRepButtons As New List(Of ButtonX)
        Dim _corpRepUpdate As Boolean = False
        Dim _selectedCorpReps As SortedList(Of CorpRepType, String)

#Region "Form Opening & Closing"

        Private Sub frmSettings_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing

            ' Save the settings
            Call PrismSettings.UserSettings.SavePrismSettings()

        End Sub

        Private Sub frmSettings_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            _startUp = True

            Call UpdateGeneralOptions()
            Call UpdateCostsOptions()
            Call UpdateSlotFormatOptions()
            Call UpdateCorpRepOptions()

            _startUp = False

            ' Switch to the right tab
            tvwSettings.Select()
            If Tag IsNot Nothing Then
                If Tag.ToString = "" Then
                    tvwSettings.SelectedNode = tvwSettings.Nodes("nodeGeneral")
                Else
                    If tvwSettings.Nodes.ContainsKey(Tag.ToString) = True Then
                        tvwSettings.SelectedNode = tvwSettings.Nodes(Tag.ToString)
                    Else
                        tvwSettings.SelectedNode = tvwSettings.Nodes("nodeGeneral")
                    End If
                End If
            Else
                tvwSettings.SelectedNode = tvwSettings.Nodes("nodeGeneral")
            End If

        End Sub

        Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
            Close()
        End Sub

#End Region

#Region "Treeview Routines"

        Private Sub tvwSettings_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles tvwSettings.AfterSelect
            Dim nodeName As String = e.Node.Name
            Dim gbName As String = nodeName.TrimStart("node".ToCharArray)
            gbName = "gp" & gbName
            For Each setControl As Control In pnlSettings.Controls
                If setControl.Name = "tvwSettings" Or setControl.Name = "btnClose" Or setControl.Name = gbName Then
                    pnlSettings.Controls(gbName).Top = 12
                    pnlSettings.Controls(gbName).Left = 195
                    pnlSettings.Controls(gbName).Width = 585
                    pnlSettings.Controls(gbName).Height = 500
                    pnlSettings.Controls(gbName).Visible = True
                Else
                    setControl.Visible = False
                End If
            Next
        End Sub

        Private Sub tvwSettings_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwSettings.NodeMouseClick
            tvwSettings.SelectedNode = e.Node
        End Sub

#End Region

#Region "General Options"

        Private Sub UpdateGeneralOptions()

            ' Update owner lists
            cboDefaultPrismCharacter.BeginUpdate() : cboDefaultPrismCharacter.Items.Clear()
            cboDefaultBPCalcBPOwner.BeginUpdate() : cboDefaultBPCalcBPOwner.Items.Clear()
            cboDefaultBPCalcManufacturer.BeginUpdate() : cboDefaultBPCalcManufacturer.Items.Clear()
            cboDefaultBPCalcAssetOwner.BeginUpdate() : cboDefaultBPCalcAssetOwner.Items.Clear()
            For Each prismOwner As String In PlugInData.PrismOwners.Keys
                cboDefaultPrismCharacter.Items.Add(prismOwner)
            Next
            For Each selpilot As EveHQPilot In HQ.Settings.Pilots.Values
                If selpilot.Active = True Then
                    cboDefaultBPCalcBPOwner.Items.Add(selpilot.Name)
                    If cboDefaultBPCalcBPOwner.Items.Contains(selpilot.Corp) = False Then
                        cboDefaultBPCalcBPOwner.Items.Add(selpilot.Corp)
                    End If
                    cboDefaultBPCalcManufacturer.Items.Add(selpilot.Name)
                    cboDefaultBPCalcAssetOwner.Items.Add(selpilot.Name)
                    If cboDefaultBPCalcAssetOwner.Items.Contains(selpilot.Corp) = False Then
                        cboDefaultBPCalcAssetOwner.Items.Add(selpilot.Corp)
                    End If
                End If
            Next
            cboDefaultPrismCharacter.Sorted = True : cboDefaultPrismCharacter.EndUpdate()
            cboDefaultBPCalcBPOwner.Sorted = True : cboDefaultBPCalcBPOwner.EndUpdate()
            cboDefaultBPCalcManufacturer.Sorted = True : cboDefaultBPCalcManufacturer.EndUpdate()
            cboDefaultBPCalcAssetOwner.Sorted = True : cboDefaultBPCalcAssetOwner.EndUpdate()

            If cboDefaultPrismCharacter.Items.Contains(PrismSettings.UserSettings.DefaultCharacter) = True Then
                cboDefaultPrismCharacter.SelectedItem = PrismSettings.UserSettings.DefaultCharacter
            End If
            If cboDefaultBPCalcBPOwner.Items.Contains(PrismSettings.UserSettings.DefaultBPOwner) = True Then
                cboDefaultBPCalcBPOwner.SelectedItem = PrismSettings.UserSettings.DefaultBPOwner
            End If
            If cboDefaultBPCalcManufacturer.Items.Contains(PrismSettings.UserSettings.DefaultBPCalcManufacturer) = True Then
                cboDefaultBPCalcManufacturer.SelectedItem = PrismSettings.UserSettings.DefaultBPCalcManufacturer
            End If
            If cboDefaultBPCalcAssetOwner.Items.Contains(PrismSettings.UserSettings.DefaultBPCalcAssetOwner) = True Then
                cboDefaultBPCalcAssetOwner.SelectedItem = PrismSettings.UserSettings.DefaultBPCalcAssetOwner
            End If

            chkHideAPIDialog.Checked = PrismSettings.UserSettings.HideAPIDownloadDialog
            chkHideSaveJobDialog.Checked = PrismSettings.UserSettings.HideSaveJobDialog

        End Sub

        Private Sub cboDefaultPrismCharacter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDefaultPrismCharacter.SelectedIndexChanged
            If cboDefaultPrismCharacter.SelectedItem IsNot Nothing Then
                PrismSettings.UserSettings.DefaultCharacter = cboDefaultPrismCharacter.SelectedItem.ToString
            End If
        End Sub

        Private Sub cboDefaultBPOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDefaultBPCalcBPOwner.SelectedIndexChanged
            If cboDefaultBPCalcBPOwner.SelectedItem IsNot Nothing Then
                PrismSettings.UserSettings.DefaultBPOwner = cboDefaultBPCalcBPOwner.SelectedItem.ToString
            End If
        End Sub

        Private Sub cboDefaultBPCalcManufacturer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDefaultBPCalcManufacturer.SelectedIndexChanged
            If cboDefaultBPCalcManufacturer.SelectedItem IsNot Nothing Then
                PrismSettings.UserSettings.DefaultBPCalcManufacturer = cboDefaultBPCalcManufacturer.SelectedItem.ToString
            End If
        End Sub

        Private Sub cboDefaultBPCalcAssetOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDefaultBPCalcAssetOwner.SelectedIndexChanged
            If cboDefaultBPCalcAssetOwner.SelectedItem IsNot Nothing Then
                PrismSettings.UserSettings.DefaultBPCalcAssetOwner = cboDefaultBPCalcAssetOwner.SelectedItem.ToString
            End If
        End Sub

        Private Sub chkHideAPIDialog_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideAPIDialog.CheckedChanged
            PrismSettings.UserSettings.HideAPIDownloadDialog = chkHideAPIDialog.Checked
        End Sub

        Private Sub chkHideSaveJobDialog_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkHideSaveJobDialog.CheckedChanged
            PrismSettings.UserSettings.HideSaveJobDialog = chkHideSaveJobDialog.Checked
        End Sub

        Private Sub btnDeleteDuplicateJournals_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteDuplicateJournals.Click
            Dim strSQL As String
            strSQL = "DELETE FROM walletJournal WHERE transID IN ("
            strSQL &= " SELECT walletJournal.transID"
            strSQL &= " FROM walletJournal INNER JOIN"
            strSQL &= " (SELECT transKey, MIN(transID) AS MinTransID, MAX(transID) AS MaxTransID"
            strSQL &= " FROM walletJournal"
            strSQL &= " GROUP BY transKey"
            strSQL &= " HAVING COUNT(*) > 1) AS Dupes ON walletJournal.transKey = Dupes.transKey AND walletJournal.transID <> Dupes.MinTransID)"
            ' Old SQL code - may come in useful!
            'strSQL = "DELETE T1 FROM walletJournal T1, walletJournal T2 WHERE (T1.transKey = T2.transKey) AND T1.importDate > T2.importDate"
            If CustomDataFunctions.SetCustomData(strSQL) = -2 Then
                MessageBox.Show("Error deleting duplicate entries from the Wallet Journal table!", "Delete Duplicates Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Successfully deleted duplicate entries from the Wallet Journal table!", "Delete Duplicates Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

        Private Sub btnDeleteDuplicateTransactions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteDuplicateTransactions.Click
            Dim strSQL As String
            strSQL = "DELETE FROM walletTransactions WHERE transID IN ("
            strSQL &= " SELECT walletTransactions.transID"
            strSQL &= " FROM walletTransactions INNER JOIN"
            strSQL &= " (SELECT transKey, MIN(transID) AS MinTransID, MAX(transID) AS MaxTransID"
            strSQL &= " FROM walletTransactions"
            strSQL &= " GROUP BY transKey"
            strSQL &= " HAVING COUNT(*) > 1) AS Dupes ON walletTransactions.transKey = Dupes.transKey AND walletTransactions.transID <> Dupes.MinTransID)"
            ' Old SQL code - may come in useful!
            'strSQL = "DELETE T1 FROM walletTransactions T1, walletTransactions T2 WHERE (T1.transKey = T2.transKey) AND T1.importDate > T2.importDate"
            If CustomDataFunctions.SetCustomData(strSQL) = -2 Then
                MessageBox.Show("Error deleting duplicate entries from the Wallet Transactions table!", "Delete Duplicates Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Successfully deleted duplicate entries from the Wallet Transactions table!", "Delete Duplicates Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub
#End Region

#Region "Costs Options"

        Private Sub UpdateCostsOptions()
            nudFactoryInstallCost.Value = PrismSettings.UserSettings.FactoryInstallCost
            nudFactoryRunningCost.Value = PrismSettings.UserSettings.FactoryRunningCost
            nudLabInstallCost.Value = PrismSettings.UserSettings.LabInstallCost
            nudLabRunningCost.Value = PrismSettings.UserSettings.LabRunningCost
            Call PopulateBPCCostGrid()
        End Sub

        Private Sub nudFactoryInstallCost_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudFactoryInstallCost.ValueChanged
            If _startUp = False Then
                PrismSettings.UserSettings.FactoryInstallCost = nudFactoryInstallCost.Value
            End If
        End Sub

        Private Sub nudFactoryRunningCost_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudFactoryRunningCost.ValueChanged
            If _startUp = False Then
                PrismSettings.UserSettings.FactoryRunningCost = nudFactoryRunningCost.Value
            End If
        End Sub

        Private Sub nudLabInstallCost_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudLabInstallCost.ValueChanged
            If _startUp = False Then
                PrismSettings.UserSettings.LabInstallCost = nudLabInstallCost.Value
            End If
        End Sub

        Private Sub nudLabRunningCost_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudLabRunningCost.ValueChanged
            If _startUp = False Then
                PrismSettings.UserSettings.LabRunningCost = nudLabRunningCost.Value
            End If
        End Sub

        Private Sub PopulateBPCCostGrid()
            lvwBPCCosts.BeginUpdate()
            lvwBPCCosts.Items.Clear()
            For Each bp As EveData.Blueprint In StaticData.Blueprints.Values
                If StaticData.Types.ContainsKey(bp.Id) Then
                    Dim newBP As New ListViewItem
                    newBP.Name = bp.Id.ToString
                    newBP.Text = StaticData.Types(bp.Id).Name
                    If PrismSettings.UserSettings.BPCCosts.ContainsKey(bp.Id) Then
                        newBP.SubItems.Add(PrismSettings.UserSettings.BPCCosts(bp.Id).MinRunCost.ToString("N2"))
                        newBP.SubItems.Add(PrismSettings.UserSettings.BPCCosts(bp.Id).MaxRunCost.ToString("N2"))
                    Else
                        newBP.SubItems.Add(0.ToString("N2"))
                        newBP.SubItems.Add(0.ToString("N2"))
                    End If
                    lvwBPCCosts.Items.Add(newBP)
                End If
            Next
            lvwBPCCosts.Sorting = SortOrder.Ascending
            lvwBPCCosts.Sort()
            lvwBPCCosts.EndUpdate()
        End Sub

        Private Sub lvwBPCCosts_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles lvwBPCCosts.DoubleClick
            If lvwBPCCosts.SelectedItems.Count = 1 Then
                Dim bpid As Integer = CInt(lvwBPCCosts.SelectedItems(0).Name)
                Using priceForm As New FrmAddBPCPrice(bpid)
                    priceForm.ShowDialog()
                    Call PopulateBPCCostGrid()
                End Using
            End If
        End Sub

#End Region

#Region "Slot Format Options"

        Private Sub UpdateSlotFormatOptions()
            _redrawColumns = True
            Call RedrawSlotColumnList()
            _redrawColumns = False
        End Sub

        Private Sub RedrawSlotColumnList()
            ' Setup the listview
            Dim newCol As ListViewItem
            lvwColumns.BeginUpdate()
            lvwColumns.Items.Clear()
            For Each userSlot As UserSlotColumn In PrismSettings.UserSettings.UserSlotColumns
                newCol = New ListViewItem(userSlot.Description)
                newCol.Name = userSlot.Name
                newCol.Checked = userSlot.Active
                lvwColumns.Items.Add(newCol)
            Next
            lvwColumns.EndUpdate()
        End Sub

        Private Sub btnMoveUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMoveUp.Click
            ' Check we have something selected
            If lvwColumns.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            ' Find the index in the user column list
            Dim idx As Integer = lvwColumns.SelectedItems(0).Index
            If idx > 0 Then
                Dim colToMove As UserSlotColumn = PrismSettings.UserSettings.UserSlotColumns(idx)
                Dim colToSwitch As UserSlotColumn = PrismSettings.UserSettings.UserSlotColumns(idx - 1)
                PrismSettings.UserSettings.UserSlotColumns.Item(idx) = colToSwitch
                PrismSettings.UserSettings.UserSlotColumns.Item(idx - 1) = colToMove
                ' Redraw the list
                _redrawColumns = True
                Call RedrawSlotColumnList()
                _redrawColumns = False
                lvwColumns.Items(idx - 1).Selected = True
                lvwColumns.Select()
            End If

        End Sub

        Private Sub btnMoveDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMoveDown.Click
            ' Check we have something selected
            If lvwColumns.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            ' Find the index in the user column list
            Dim idx As Integer = lvwColumns.SelectedItems(0).Index
            If idx < lvwColumns.Items.Count - 1 Then
                Dim colToMove As UserSlotColumn = PrismSettings.UserSettings.UserSlotColumns(idx)
                Dim colToSwitch As UserSlotColumn = PrismSettings.UserSettings.UserSlotColumns(idx + 1)
                PrismSettings.UserSettings.UserSlotColumns.Item(idx) = colToSwitch
                PrismSettings.UserSettings.UserSlotColumns.Item(idx + 1) = colToMove
                ' Redraw the list
                _redrawColumns = True
                Call RedrawSlotColumnList()
                _redrawColumns = False
                lvwColumns.Items(idx + 1).Selected = True
                lvwColumns.Select()
            End If
        End Sub

        Private Sub lvwColumns_ItemChecked(ByVal sender As Object, ByVal e As ItemCheckedEventArgs) Handles lvwColumns.ItemChecked
            If _redrawColumns = False Then
                ' Find the index in the user column list
                Dim idx As Integer = e.Item.Index
                PrismSettings.UserSettings.UserSlotColumns.Item(idx).Active = e.Item.Checked
            End If
        End Sub

#End Region

#Region "Corp Rep Options"

        Private Sub UpdateCorpRepOptions()

            adtCorpReps.BeginUpdate()
            adtCorpReps.Nodes.Clear()

            Dim playerCorps As New List(Of String)
            ' Get a list of Corps
            For Each selpilot As EveHQPilot In HQ.Settings.Pilots.Values
                If selpilot.CorpID <> "" Then
                    If StaticData.NpcCorps.ContainsKey(CInt(selpilot.CorpID)) = False Then
                        If playerCorps.Contains(selpilot.Corp) = False Then
                            playerCorps.Add(selpilot.Corp)
                        End If
                    End If
                End If
            Next
            For Each playerCorp As String In playerCorps
                adtCorpReps.Nodes.Add(New Node(playerCorp))
            Next

            adtCorpReps.Columns(0).Sort()
            adtCorpReps.EndUpdate()

            ' Create a list of the combo boxes
            _corpRepCombos.Clear()
            _corpRepCombos.Add(cboAssetsRep)
            _corpRepCombos.Add(cboBalancesRep)
            _corpRepCombos.Add(cboJobsRep)
            _corpRepCombos.Add(cboJournalRep)
            _corpRepCombos.Add(cboOrdersRep)
            _corpRepCombos.Add(cboTransactionsRep)
            _corpRepCombos.Add(cboContractsRep)
            _corpRepCombos.Add(cboCorpSheetRep)

            ' Create a list of the remove corp rep buttons
            _corpRepButtons.Clear()
            _corpRepButtons.Add(btnNoRepAssets)
            _corpRepButtons.Add(btnNoRepBalances)
            _corpRepButtons.Add(btnNoRepContracts)
            _corpRepButtons.Add(btnNoRepCorpSheet)
            _corpRepButtons.Add(btnNoRepJobs)
            _corpRepButtons.Add(btnNoRepJournal)
            _corpRepButtons.Add(btnNoRepOrders)
            _corpRepButtons.Add(btnNoRepTransactions)

        End Sub

        Private Sub adtCorpReps_SelectionChanged(sender As Object, e As EventArgs) Handles adtCorpReps.SelectionChanged
            _corpRepUpdate = True
            If adtCorpReps.SelectedNodes.Count = 1 Then
                Dim corpName As String = adtCorpReps.SelectedNodes(0).Text
                lblSelectedCorp.Text = "Selected Corp: " & corpName
                ' Get a list of the pilots in this corp and add them to the comboboxes
                For Each cbo As ComboBoxEx In _corpRepCombos
                    cbo.Items.Clear()
                Next
                For Each selpilot As EveHQPilot In HQ.Settings.Pilots.Values
                    If selpilot.Corp = corpName Then
                        For Each cbo As ComboBoxEx In _corpRepCombos
                            cbo.Items.Add(selpilot.Name)
                        Next
                    End If
                Next
                For Each cbo As ComboBoxEx In _corpRepCombos
                    cbo.Sorted = True
                Next
                ' Set the comboboxes to the existing corp rep settings (or reset if not available)
                If PrismSettings.UserSettings.CorpReps.ContainsKey(corpName) = False Then
                    PrismSettings.UserSettings.CorpReps.Add(corpName, New SortedList(Of CorpRepType, String))
                End If
                _selectedCorpReps = PrismSettings.UserSettings.CorpReps(corpName)
                For idx As Integer = 0 To 7
                    If _selectedCorpReps.ContainsKey(CType(idx, CorpRepType)) = True Then
                        If _corpRepCombos(idx).Items.Contains(_selectedCorpReps(CType(idx, CorpRepType))) = True Then
                            _corpRepCombos(idx).SelectedItem = _selectedCorpReps(CType(idx, CorpRepType))
                        Else
                            _corpRepCombos(idx).SelectedIndex = -1
                        End If
                    Else
                        _corpRepCombos(idx).SelectedIndex = -1
                    End If
                Next
                ' Set the status of the combos
                If chkUseSamePilot.Checked = True Then
                    For Each cbo As ComboBoxEx In _corpRepCombos
                        cbo.Enabled = False
                    Next
                    cboAssetsRep.Enabled = True
                    For Each btn As ButtonX In _corpRepButtons
                        btn.Enabled = False
                    Next
                    btnNoRepAssets.Enabled = True
                Else
                    For Each cbo As ComboBoxEx In _corpRepCombos
                        cbo.Enabled = True
                    Next
                    For Each btn As ButtonX In _corpRepButtons
                        btn.Enabled = True
                    Next
                End If
            Else
                ' Clear the selection
                lblSelectedCorp.Text = "Selected Corp: <None>"
                For Each cbo As ComboBoxEx In _corpRepCombos
                    cbo.Items.Clear() : cbo.SelectedIndex = -1 : cbo.Enabled = False
                Next
            End If
            _corpRepUpdate = False
        End Sub

        Private Sub cboAssetsRep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAssetsRep.SelectedIndexChanged
            If _corpRepUpdate = False Then
                If cboAssetsRep.SelectedIndex > -1 Then
                    If _selectedCorpReps.ContainsKey(CorpRepType.Assets) = False Then
                        _selectedCorpReps.Add(CorpRepType.Assets, cboAssetsRep.SelectedItem.ToString)
                    Else
                        _selectedCorpReps(CorpRepType.Assets) = cboAssetsRep.SelectedItem.ToString
                    End If
                    btnNoRepAssets.Enabled = True
                End If
            End If
            ' Update all the other cboboxes if using the same pilot for all
            If chkUseSamePilot.Checked = True Then
                For Each cbo As ComboBoxEx In _corpRepCombos
                    cbo.SelectedItem = cboAssetsRep.SelectedItem
                Next
            End If
        End Sub

        Private Sub cboBalancesRep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBalancesRep.SelectedIndexChanged
            If _corpRepUpdate = False Then
                If cboBalancesRep.SelectedIndex > -1 Then
                    If _selectedCorpReps.ContainsKey(CorpRepType.Balances) = False Then
                        _selectedCorpReps.Add(CorpRepType.Balances, cboBalancesRep.SelectedItem.ToString)
                    Else
                        _selectedCorpReps(CorpRepType.Balances) = cboBalancesRep.SelectedItem.ToString
                    End If
                    btnNoRepBalances.Enabled = True
                End If
            End If
        End Sub

        Private Sub cboJobsRep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboJobsRep.SelectedIndexChanged
            If _corpRepUpdate = False Then
                If cboJobsRep.SelectedIndex > -1 Then
                    If _selectedCorpReps.ContainsKey(CorpRepType.Jobs) = False Then
                        _selectedCorpReps.Add(CorpRepType.Jobs, cboJobsRep.SelectedItem.ToString)
                    Else
                        _selectedCorpReps(CorpRepType.Jobs) = cboJobsRep.SelectedItem.ToString
                    End If
                    btnNoRepJobs.Enabled = True
                End If
            End If
        End Sub

        Private Sub cboJournalRep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboJournalRep.SelectedIndexChanged
            If _corpRepUpdate = False Then
                If cboJournalRep.SelectedIndex > -1 Then
                    If _selectedCorpReps.ContainsKey(CorpRepType.WalletJournal) = False Then
                        _selectedCorpReps.Add(CorpRepType.WalletJournal, cboJournalRep.SelectedItem.ToString)
                    Else
                        _selectedCorpReps(CorpRepType.WalletJournal) = cboJournalRep.SelectedItem.ToString
                    End If
                    btnNoRepJournal.Enabled = True
                End If
            End If
        End Sub

        Private Sub cboOrdersRep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrdersRep.SelectedIndexChanged
            If _corpRepUpdate = False Then
                If cboOrdersRep.SelectedIndex > -1 Then
                    If _selectedCorpReps.ContainsKey(CorpRepType.Orders) = False Then
                        _selectedCorpReps.Add(CorpRepType.Orders, cboOrdersRep.SelectedItem.ToString)
                    Else
                        _selectedCorpReps(CorpRepType.Orders) = cboOrdersRep.SelectedItem.ToString
                    End If
                    btnNoRepOrders.Enabled = True
                End If
            End If
        End Sub

        Private Sub cboTransactionsRep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTransactionsRep.SelectedIndexChanged
            If _corpRepUpdate = False Then
                If cboTransactionsRep.SelectedIndex > -1 Then
                    If _selectedCorpReps.ContainsKey(CorpRepType.WalletTransactions) = False Then
                        _selectedCorpReps.Add(CorpRepType.WalletTransactions, cboTransactionsRep.SelectedItem.ToString)
                    Else
                        _selectedCorpReps(CorpRepType.WalletTransactions) = cboTransactionsRep.SelectedItem.ToString
                    End If
                    btnNoRepTransactions.Enabled = True
                End If
            End If
        End Sub

        Private Sub cboContractsRep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboContractsRep.SelectedIndexChanged
            If _corpRepUpdate = False Then
                If cboContractsRep.SelectedIndex > -1 Then
                    If _selectedCorpReps.ContainsKey(CorpRepType.Contracts) = False Then
                        _selectedCorpReps.Add(CorpRepType.Contracts, cboContractsRep.SelectedItem.ToString)
                    Else
                        _selectedCorpReps(CorpRepType.Contracts) = cboContractsRep.SelectedItem.ToString
                    End If
                    btnNoRepContracts.Enabled = True
                End If
            End If
        End Sub

        Private Sub cboCorpSheetRep_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCorpSheetRep.SelectedIndexChanged
            If _corpRepUpdate = False Then
                If cboCorpSheetRep.SelectedIndex > -1 Then
                    If _selectedCorpReps.ContainsKey(CorpRepType.CorpSheet) = False Then
                        _selectedCorpReps.Add(CorpRepType.CorpSheet, cboCorpSheetRep.SelectedItem.ToString)
                    Else
                        _selectedCorpReps(CorpRepType.CorpSheet) = cboCorpSheetRep.SelectedItem.ToString
                    End If
                    btnNoRepCorpSheet.Enabled = True
                End If
            End If
        End Sub

        Private Sub chkUseSamePilot_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseSamePilot.CheckedChanged
            If chkUseSamePilot.Checked = True Then
                For Each cbo As ComboBoxEx In _corpRepCombos
                    cbo.Enabled = False
                Next
                cboAssetsRep.Enabled = True
                For Each btn As ButtonX In _corpRepButtons
                    btn.Enabled = False
                Next
                btnNoRepAssets.Enabled = True
                ' Update all the other cboboxes if using the same pilot for all
                For Each cbo As ComboBoxEx In _corpRepCombos
                    cbo.SelectedItem = cboAssetsRep.SelectedItem
                Next
            Else
                For Each cbo As ComboBoxEx In _corpRepCombos
                    cbo.Enabled = True
                Next
                For Each btn As ButtonX In _corpRepButtons
                    btn.Enabled = True
                Next
            End If
        End Sub

        Private Sub btnNoRepAll_Click(sender As Object, e As EventArgs) Handles btnNoRepAll.Click
            Call btnNoRepAssets_Click(sender, e)
            Call btnNoRepCorpSheet_Click(sender, e)
            Call btnNoRepTransactions_Click(sender, e)
            Call btnNoRepOrders_Click(sender, e)
            Call btnNoRepJournal_Click(sender, e)
            Call btnNoRepJobs_Click(sender, e)
            Call btnNoRepBalances_Click(sender, e)
            Call btnNoRepContracts_Click(sender, e)
        End Sub

        Private Sub btnNoRepAssets_Click(sender As Object, e As EventArgs) Handles btnNoRepAssets.Click
            Call ResetCorpRep(CorpRepType.Assets)
            cboAssetsRep.SelectedIndex = -1
            btnNoRepAssets.Enabled = False
        End Sub

        Private Sub btnNoRepCorpSheet_Click(sender As Object, e As EventArgs) Handles btnNoRepCorpSheet.Click
            Call ResetCorpRep(CorpRepType.CorpSheet)
            cboCorpSheetRep.SelectedIndex = -1
            btnNoRepCorpSheet.Enabled = False
        End Sub

        Private Sub btnNoRepTransactions_Click(sender As Object, e As EventArgs) Handles btnNoRepTransactions.Click
            Call ResetCorpRep(CorpRepType.WalletTransactions)
            cboTransactionsRep.SelectedIndex = -1
            btnNoRepTransactions.Enabled = False
        End Sub

        Private Sub btnNoRepOrders_Click(sender As Object, e As EventArgs) Handles btnNoRepOrders.Click
            Call ResetCorpRep(CorpRepType.Orders)
            cboOrdersRep.SelectedIndex = -1
            btnNoRepOrders.Enabled = False
        End Sub

        Private Sub btnNoRepJournal_Click(sender As Object, e As EventArgs) Handles btnNoRepJournal.Click
            Call ResetCorpRep(CorpRepType.WalletJournal)
            cboJournalRep.SelectedIndex = -1
            btnNoRepJournal.Enabled = False
        End Sub

        Private Sub btnNoRepJobs_Click(sender As Object, e As EventArgs) Handles btnNoRepJobs.Click
            Call ResetCorpRep(CorpRepType.Jobs)
            cboJobsRep.SelectedIndex = -1
            btnNoRepJobs.Enabled = False
        End Sub

        Private Sub btnNoRepBalances_Click(sender As Object, e As EventArgs) Handles btnNoRepBalances.Click
            Call ResetCorpRep(CorpRepType.Balances)
            cboBalancesRep.SelectedIndex = -1
            btnNoRepBalances.Enabled = False
        End Sub

        Private Sub btnNoRepContracts_Click(sender As Object, e As EventArgs) Handles btnNoRepContracts.Click
            Call ResetCorpRep(CorpRepType.Contracts)
            cboContractsRep.SelectedIndex = -1
            btnNoRepContracts.Enabled = False
        End Sub

        Private Sub ResetCorpRep(repType As CorpRepType)
            If _selectedCorpReps.ContainsKey(RepType) Then
                _selectedCorpReps.Remove(RepType)
            End If
        End Sub

#End Region

        Private Sub btnDeleteUndefinedJournals_Click(sender As Object, e As EventArgs) Handles btnDeleteUndefinedJournals.Click
            Const StrSQL As String = "DELETE FROM walletJournal WHERE refTypeID = 0;"
            If CustomDataFunctions.SetCustomData(StrSQL) = -2 Then
                MessageBox.Show("Error deleting undefined entries from the Wallet Journal table!", "Delete Undefined Entries Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Successfully deleted undefined entries from the Wallet Journal table!", "Delete Undefined Entries Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

     
    End Class
End NameSpace