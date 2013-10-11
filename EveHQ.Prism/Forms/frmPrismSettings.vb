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
Imports System.Xml
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar.Controls
Imports DevComponents.DotNetBar
Imports EveHQ.EveData

Public Class frmPrismSettings
    Dim startUp As Boolean = True
    Dim forceUpdate As Boolean = False
    Dim redrawColumns As Boolean = True
    Dim CorpRepCombos As New List(Of ComboBoxEx)
    Dim CorpRepButtons As New List(Of ButtonX)
    Dim CorpRepUpdate As Boolean = False
    Dim SelectedCorpReps As SortedList(Of CorpRepType, String)

#Region "Form Opening & Closing"

    Private Sub frmSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' Save the settings
        Call Settings.PrismSettings.SavePrismSettings()

    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        startUp = True

        Call Me.UpdateGeneralOptions()
        Call Me.UpdateCostsOptions()
        Call Me.UpdateSlotFormatOptions()
        Call Me.UpdateCorpRepOptions()

        startUp = False

        ' Switch to the right tab
        Me.tvwSettings.Select()
        If Me.Tag IsNot Nothing Then
            If Me.Tag.ToString = "" Then
                Me.tvwSettings.SelectedNode = Me.tvwSettings.Nodes("nodeGeneral")
            Else
                If Me.tvwSettings.Nodes.ContainsKey(Me.Tag.ToString) = True Then
                    Me.tvwSettings.SelectedNode = Me.tvwSettings.Nodes(Me.Tag.ToString)
                Else
                    Me.tvwSettings.SelectedNode = Me.tvwSettings.Nodes("nodeGeneral")
                End If
            End If
        Else
            Me.tvwSettings.SelectedNode = Me.tvwSettings.Nodes("nodeGeneral")
        End If

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

#End Region

#Region "Treeview Routines"

    Private Sub tvwSettings_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwSettings.AfterSelect
        Dim nodeName As String = e.Node.Name
        Dim gbName As String = nodeName.TrimStart("node".ToCharArray)
        gbName = "gp" & gbName
        For Each setControl As Control In Me.pnlSettings.Controls
            If setControl.Name = "tvwSettings" Or setControl.Name = "btnClose" Or setControl.Name = gbName Then
                Me.pnlSettings.Controls(gbName).Top = 12
                Me.pnlSettings.Controls(gbName).Left = 195
                Me.pnlSettings.Controls(gbName).Width = 585
                Me.pnlSettings.Controls(gbName).Height = 500
                Me.pnlSettings.Controls(gbName).Visible = True
            Else
                setControl.Visible = False
            End If
        Next
    End Sub

    Private Sub tvwSettings_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwSettings.NodeMouseClick
        Me.tvwSettings.SelectedNode = e.Node
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
        For Each selpilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
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

        If cboDefaultPrismCharacter.Items.Contains(Settings.PrismSettings.DefaultCharacter) = True Then
            cboDefaultPrismCharacter.SelectedItem = Settings.PrismSettings.DefaultCharacter
        End If
        If cboDefaultBPCalcBPOwner.Items.Contains(Settings.PrismSettings.DefaultBPOwner) = True Then
            cboDefaultBPCalcBPOwner.SelectedItem = Settings.PrismSettings.DefaultBPOwner
        End If
        If cboDefaultBPCalcManufacturer.Items.Contains(Settings.PrismSettings.DefaultBPCalcManufacturer) = True Then
            cboDefaultBPCalcManufacturer.SelectedItem = Settings.PrismSettings.DefaultBPCalcManufacturer
        End If
        If cboDefaultBPCalcAssetOwner.Items.Contains(Settings.PrismSettings.DefaultBPCalcAssetOwner) = True Then
            cboDefaultBPCalcAssetOwner.SelectedItem = Settings.PrismSettings.DefaultBPCalcAssetOwner
        End If

        chkHideAPIDialog.Checked = Settings.PrismSettings.HideAPIDownloadDialog

    End Sub

    Private Sub cboDefaultPrismCharacter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDefaultPrismCharacter.SelectedIndexChanged
        If cboDefaultPrismCharacter.SelectedItem IsNot Nothing Then
            Settings.PrismSettings.DefaultCharacter = cboDefaultPrismCharacter.SelectedItem.ToString
        End If
    End Sub

    Private Sub cboDefaultBPOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDefaultBPCalcBPOwner.SelectedIndexChanged
        If cboDefaultBPCalcBPOwner.SelectedItem IsNot Nothing Then
            Settings.PrismSettings.DefaultBPOwner = cboDefaultBPCalcBPOwner.SelectedItem.ToString
        End If
    End Sub

    Private Sub cboDefaultBPCalcManufacturer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDefaultBPCalcManufacturer.SelectedIndexChanged
        If cboDefaultBPCalcManufacturer.SelectedItem IsNot Nothing Then
            Settings.PrismSettings.DefaultBPCalcManufacturer = cboDefaultBPCalcManufacturer.SelectedItem.ToString
        End If
    End Sub

    Private Sub cboDefaultBPCalcAssetOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDefaultBPCalcAssetOwner.SelectedIndexChanged
        If cboDefaultBPCalcAssetOwner.SelectedItem IsNot Nothing Then
            Settings.PrismSettings.DefaultBPCalcAssetOwner = cboDefaultBPCalcAssetOwner.SelectedItem.ToString
        End If
    End Sub

    Private Sub chkHideAPIDialog_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkHideAPIDialog.CheckedChanged
        Settings.PrismSettings.HideAPIDownloadDialog = chkHideAPIDialog.Checked
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
        If Core.CustomDataFunctions.SetCustomData(strSQL) = -2 Then
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
        If Core.CustomDataFunctions.SetCustomData(strSQL) = -2 Then
            MessageBox.Show("Error deleting duplicate entries from the Wallet Transactions table!", "Delete Duplicates Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            MessageBox.Show("Successfully deleted duplicate entries from the Wallet Transactions table!", "Delete Duplicates Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
#End Region

#Region "Costs Options"

    Private Sub UpdateCostsOptions()
        nudFactoryInstallCost.Value = Settings.PrismSettings.FactoryInstallCost
        nudFactoryRunningCost.Value = Settings.PrismSettings.FactoryRunningCost
        nudLabInstallCost.Value = Settings.PrismSettings.LabInstallCost
        nudLabRunningCost.Value = Settings.PrismSettings.LabRunningCost
        Call Me.PopulateBPCCostGrid()
    End Sub

    Private Sub nudFactoryInstallCost_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudFactoryInstallCost.ValueChanged
        If startUp = False Then
            Settings.PrismSettings.FactoryInstallCost = nudFactoryInstallCost.Value
        End If
    End Sub

    Private Sub nudFactoryRunningCost_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudFactoryRunningCost.ValueChanged
        If startUp = False Then
            Settings.PrismSettings.FactoryRunningCost = nudFactoryRunningCost.Value
        End If
    End Sub

    Private Sub nudLabInstallCost_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudLabInstallCost.ValueChanged
        If startUp = False Then
            Settings.PrismSettings.LabInstallCost = nudLabInstallCost.Value
        End If
    End Sub

    Private Sub nudLabRunningCost_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudLabRunningCost.ValueChanged
        If startUp = False Then
            Settings.PrismSettings.LabRunningCost = nudLabRunningCost.Value
        End If
    End Sub

    Private Sub PopulateBPCCostGrid()
        lvwBPCCosts.BeginUpdate()
        lvwBPCCosts.Items.Clear()
        For Each bp As EveData.Blueprint In StaticData.Blueprints.Values
            Dim newBP As New ListViewItem
            newBP.Name = bp.Id.ToString
            newBP.Text = StaticData.Types(bp.Id).Name
            If Settings.PrismSettings.BPCCosts.ContainsKey(bp.Id) Then
                newBP.SubItems.Add(Settings.PrismSettings.BPCCosts(bp.Id).MinRunCost.ToString("N2"))
                newBP.SubItems.Add(Settings.PrismSettings.BPCCosts(bp.Id).MaxRunCost.ToString("N2"))
            Else
                newBP.SubItems.Add(0.ToString("N2"))
                newBP.SubItems.Add(0.ToString("N2"))
            End If
            lvwBPCCosts.Items.Add(newBP)
        Next
        lvwBPCCosts.Sorting = SortOrder.Ascending
        lvwBPCCosts.Sort()
        lvwBPCCosts.EndUpdate()
    End Sub

    Private Sub lvwBPCCosts_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwBPCCosts.DoubleClick
        If lvwBPCCosts.SelectedItems.Count = 1 Then
            Dim bpid As Integer = CInt(lvwBPCCosts.SelectedItems(0).Name)
            Dim priceForm As New frmAddBPCPrice(bpid)
            priceForm.ShowDialog()
            Call PopulateBPCCostGrid()
            priceForm.Dispose()
        End If
    End Sub

#End Region

#Region "Slot Format Options"

    Private Sub UpdateSlotFormatOptions()
        redrawColumns = True
        Call Me.RedrawSlotColumnList()
        redrawColumns = False
    End Sub

    Private Sub RedrawSlotColumnList()
        ' Setup the listview
        Dim newCol As New ListViewItem
        lvwColumns.BeginUpdate()
        lvwColumns.Items.Clear()
        For Each UserSlot As UserSlotColumn In Settings.PrismSettings.UserSlotColumns
            newCol = New ListViewItem(UserSlot.Description)
            newCol.Name = UserSlot.Name
            newCol.Checked = UserSlot.Active
            lvwColumns.Items.Add(newCol)
        Next
        lvwColumns.EndUpdate()
    End Sub

    Private Sub btnMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveUp.Click
        ' Check we have something selected
        If lvwColumns.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Find the index in the user column list
        Dim idx As Integer = lvwColumns.SelectedItems(0).Index
        If idx > 0 Then
            Dim ColToMove As UserSlotColumn = Settings.PrismSettings.UserSlotColumns(idx)
            Dim ColToSwitch As UserSlotColumn = Settings.PrismSettings.UserSlotColumns(idx - 1)
            Settings.PrismSettings.UserSlotColumns.Item(idx) = ColToSwitch
            Settings.PrismSettings.UserSlotColumns.Item(idx - 1) = ColToMove
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawSlotColumnList()
            redrawColumns = False
            lvwColumns.Items(idx - 1).Selected = True
            lvwColumns.Select()
            forceUpdate = True
        End If

    End Sub

    Private Sub btnMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveDown.Click
        ' Check we have something selected
        If lvwColumns.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Find the index in the user column list
        Dim idx As Integer = lvwColumns.SelectedItems(0).Index
        If idx < lvwColumns.Items.Count - 1 Then
            Dim ColToMove As UserSlotColumn = Settings.PrismSettings.UserSlotColumns(idx)
            Dim ColToSwitch As UserSlotColumn = Settings.PrismSettings.UserSlotColumns(idx + 1)
            Settings.PrismSettings.UserSlotColumns.Item(idx) = ColToSwitch
            Settings.PrismSettings.UserSlotColumns.Item(idx + 1) = ColToMove
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawSlotColumnList()
            redrawColumns = False
            lvwColumns.Items(idx + 1).Selected = True
            lvwColumns.Select()
            forceUpdate = True
        End If
    End Sub

    Private Sub lvwColumns_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwColumns.ItemChecked
        If redrawColumns = False Then
            ' Find the index in the user column list
            Dim idx As Integer = e.Item.Index
            Settings.PrismSettings.UserSlotColumns.Item(idx).Active = e.Item.Checked
            forceUpdate = True
        End If
    End Sub

#End Region

#Region "Corp Rep Options"

    Private Sub UpdateCorpRepOptions()

        adtCorpReps.BeginUpdate()
        adtCorpReps.Nodes.Clear()

        Dim PlayerCorps As New List(Of String)
        ' Get a list of Corps
        For Each selpilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
            If PlugInData.NPCCorps.ContainsKey(selpilot.CorpID) = False Then
                If PlayerCorps.Contains(selpilot.Corp) = False Then
                    PlayerCorps.Add(selpilot.Corp)
                End If
            End If
        Next
        For Each PlayerCorp As String In PlayerCorps
            adtCorpReps.Nodes.Add(New Node(PlayerCorp))
        Next

        adtCorpReps.Columns(0).Sort()
        adtCorpReps.EndUpdate()

        ' Create a list of the combo boxes
        CorpRepCombos.Clear()
        CorpRepCombos.Add(cboAssetsRep)
        CorpRepCombos.Add(cboBalancesRep)
        CorpRepCombos.Add(cboJobsRep)
        CorpRepCombos.Add(cboJournalRep)
        CorpRepCombos.Add(cboOrdersRep)
        CorpRepCombos.Add(cboTransactionsRep)
        CorpRepCombos.Add(cboContractsRep)
        CorpRepCombos.Add(cboCorpSheetRep)

        ' Create a list of the remove corp rep buttons
        CorpRepButtons.Clear()
        CorpRepButtons.Add(btnNoRepAssets)
        CorpRepButtons.Add(btnNoRepBalances)
        CorpRepButtons.Add(btnNoRepContracts)
        CorpRepButtons.Add(btnNoRepCorpSheet)
        CorpRepButtons.Add(btnNoRepJobs)
        CorpRepButtons.Add(btnNoRepJournal)
        CorpRepButtons.Add(btnNoRepOrders)
        CorpRepButtons.Add(btnNoRepTransactions)

    End Sub

    Private Sub adtCorpReps_SelectionChanged(sender As Object, e As System.EventArgs) Handles adtCorpReps.SelectionChanged
        CorpRepUpdate = True
        If adtCorpReps.SelectedNodes.Count = 1 Then
            Dim CorpName As String = adtCorpReps.SelectedNodes(0).Text
            lblSelectedCorp.Text = "Selected Corp: " & CorpName
            ' Get a list of the pilots in this corp and add them to the comboboxes
            For Each cbo As ComboBoxEx In CorpRepCombos
                cbo.Items.Clear()
            Next
            For Each selpilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
                If selpilot.Corp = CorpName Then
                    For Each cbo As ComboBoxEx In CorpRepCombos
                        cbo.Items.Add(selpilot.Name)
                    Next
                End If
            Next
            For Each cbo As ComboBoxEx In CorpRepCombos
                cbo.Sorted = True
            Next
            ' Set the comboboxes to the existing corp rep settings (or reset if not available)
            If Settings.PrismSettings.CorpReps.ContainsKey(CorpName) = False Then
                Settings.PrismSettings.CorpReps.Add(CorpName, New SortedList(Of CorpRepType, String))
            End If
            SelectedCorpReps = Settings.PrismSettings.CorpReps(CorpName)
            For idx As Integer = 0 To 7
                If SelectedCorpReps.ContainsKey(CType(idx, CorpRepType)) = True Then
                    If CorpRepCombos(idx).Items.Contains(SelectedCorpReps(CType(idx, CorpRepType))) = True Then
                        CorpRepCombos(idx).SelectedItem = SelectedCorpReps(CType(idx, CorpRepType))
                    Else
                        CorpRepCombos(idx).SelectedIndex = -1
                    End If
                Else
                    CorpRepCombos(idx).SelectedIndex = -1
                End If
            Next
            ' Set the status of the combos
            If chkUseSamePilot.Checked = True Then
                For Each cbo As ComboBoxEx In CorpRepCombos
                    cbo.Enabled = False
                Next
                cboAssetsRep.Enabled = True
                For Each btn As ButtonX In CorpRepButtons
                    btn.Enabled = False
                Next
                btnNoRepAssets.Enabled = True
            Else
                For Each cbo As ComboBoxEx In CorpRepCombos
                    cbo.Enabled = True
                Next
                For Each btn As ButtonX In CorpRepButtons
                    btn.Enabled = True
                Next
            End If
        Else
            ' Clear the selection
            lblSelectedCorp.Text = "Selected Corp: <None>"
            For Each cbo As ComboBoxEx In CorpRepCombos
                cbo.Items.Clear() : cbo.SelectedIndex = -1 : cbo.Enabled = False
            Next
        End If
        CorpRepUpdate = False
    End Sub

    Private Sub cboAssetsRep_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboAssetsRep.SelectedIndexChanged
        If CorpRepUpdate = False Then
            If cboAssetsRep.SelectedIndex > -1 Then
                If SelectedCorpReps.ContainsKey(CorpRepType.Assets) = False Then
                    SelectedCorpReps.Add(CorpRepType.Assets, cboAssetsRep.SelectedItem.ToString)
                Else
                    SelectedCorpReps(CorpRepType.Assets) = cboAssetsRep.SelectedItem.ToString
                End If
                btnNoRepAssets.Enabled = True
            End If
        End If
        ' Update all the other cboboxes if using the same pilot for all
        If chkUseSamePilot.Checked = True Then
            For Each cbo As ComboBoxEx In CorpRepCombos
                cbo.SelectedItem = cboAssetsRep.SelectedItem
            Next
        End If
    End Sub

    Private Sub cboBalancesRep_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboBalancesRep.SelectedIndexChanged
        If CorpRepUpdate = False Then
            If cboBalancesRep.SelectedIndex > -1 Then
                If SelectedCorpReps.ContainsKey(CorpRepType.Balances) = False Then
                    SelectedCorpReps.Add(CorpRepType.Balances, cboBalancesRep.SelectedItem.ToString)
                Else
                    SelectedCorpReps(CorpRepType.Balances) = cboBalancesRep.SelectedItem.ToString
                End If
                btnNoRepBalances.Enabled = True
            End If
        End If
    End Sub

    Private Sub cboJobsRep_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboJobsRep.SelectedIndexChanged
        If CorpRepUpdate = False Then
            If cboJobsRep.SelectedIndex > -1 Then
                If SelectedCorpReps.ContainsKey(CorpRepType.Jobs) = False Then
                    SelectedCorpReps.Add(CorpRepType.Jobs, cboJobsRep.SelectedItem.ToString)
                Else
                    SelectedCorpReps(CorpRepType.Jobs) = cboJobsRep.SelectedItem.ToString
                End If
                btnNoRepJobs.Enabled = True
            End If
        End If
    End Sub

    Private Sub cboJournalRep_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboJournalRep.SelectedIndexChanged
        If CorpRepUpdate = False Then
            If cboJournalRep.SelectedIndex > -1 Then
                If SelectedCorpReps.ContainsKey(CorpRepType.WalletJournal) = False Then
                    SelectedCorpReps.Add(CorpRepType.WalletJournal, cboJournalRep.SelectedItem.ToString)
                Else
                    SelectedCorpReps(CorpRepType.WalletJournal) = cboJournalRep.SelectedItem.ToString
                End If
                btnNoRepJournal.Enabled = True
            End If
        End If
    End Sub

    Private Sub cboOrdersRep_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboOrdersRep.SelectedIndexChanged
        If CorpRepUpdate = False Then
            If cboOrdersRep.SelectedIndex > -1 Then
                If SelectedCorpReps.ContainsKey(CorpRepType.Orders) = False Then
                    SelectedCorpReps.Add(CorpRepType.Orders, cboOrdersRep.SelectedItem.ToString)
                Else
                    SelectedCorpReps(CorpRepType.Orders) = cboOrdersRep.SelectedItem.ToString
                End If
                btnNoRepOrders.Enabled = True
            End If
        End If
    End Sub

    Private Sub cboTransactionsRep_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboTransactionsRep.SelectedIndexChanged
        If CorpRepUpdate = False Then
            If cboTransactionsRep.SelectedIndex > -1 Then
                If SelectedCorpReps.ContainsKey(CorpRepType.WalletTransactions) = False Then
                    SelectedCorpReps.Add(CorpRepType.WalletTransactions, cboTransactionsRep.SelectedItem.ToString)
                Else
                    SelectedCorpReps(CorpRepType.WalletTransactions) = cboTransactionsRep.SelectedItem.ToString
                End If
                btnNoRepTransactions.Enabled = True
            End If
        End If
    End Sub

    Private Sub cboContractsRep_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboContractsRep.SelectedIndexChanged
        If CorpRepUpdate = False Then
            If cboContractsRep.SelectedIndex > -1 Then
                If SelectedCorpReps.ContainsKey(CorpRepType.Contracts) = False Then
                    SelectedCorpReps.Add(CorpRepType.Contracts, cboContractsRep.SelectedItem.ToString)
                Else
                    SelectedCorpReps(CorpRepType.Contracts) = cboContractsRep.SelectedItem.ToString
                End If
                btnNoRepContracts.Enabled = True
            End If
        End If
    End Sub

    Private Sub cboCorpSheetRep_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboCorpSheetRep.SelectedIndexChanged
        If CorpRepUpdate = False Then
            If cboCorpSheetRep.SelectedIndex > -1 Then
                If SelectedCorpReps.ContainsKey(CorpRepType.CorpSheet) = False Then
                    SelectedCorpReps.Add(CorpRepType.CorpSheet, cboCorpSheetRep.SelectedItem.ToString)
                Else
                    SelectedCorpReps(CorpRepType.CorpSheet) = cboCorpSheetRep.SelectedItem.ToString
                End If
                btnNoRepCorpSheet.Enabled = True
            End If
        End If
    End Sub

    Private Sub chkUseSamePilot_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkUseSamePilot.CheckedChanged
        If chkUseSamePilot.Checked = True Then
            For Each cbo As ComboBoxEx In CorpRepCombos
                cbo.Enabled = False
            Next
            cboAssetsRep.Enabled = True
            For Each btn As ButtonX In CorpRepButtons
                btn.Enabled = False
            Next
            btnNoRepAssets.Enabled = True
            ' Update all the other cboboxes if using the same pilot for all
            For Each cbo As ComboBoxEx In CorpRepCombos
                cbo.SelectedItem = cboAssetsRep.SelectedItem
            Next
        Else
            For Each cbo As ComboBoxEx In CorpRepCombos
                cbo.Enabled = True
            Next
            For Each btn As ButtonX In CorpRepButtons
                btn.Enabled = True
            Next
        End If
    End Sub

    Private Sub btnNoRepAll_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepAll.Click
        Call btnNoRepAssets_Click(sender, e)
        Call btnNoRepCorpSheet_Click(sender, e)
        Call btnNoRepTransactions_Click(sender, e)
        Call btnNoRepOrders_Click(sender, e)
        Call btnNoRepJournal_Click(sender, e)
        Call btnNoRepJobs_Click(sender, e)
        Call btnNoRepBalances_Click(sender, e)
        Call btnNoRepContracts_Click(sender, e)
    End Sub

    Private Sub btnNoRepAssets_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepAssets.Click
        Call ResetCorpRep(CorpRepType.Assets)
        cboAssetsRep.SelectedIndex = -1
        btnNoRepAssets.Enabled = False
    End Sub

    Private Sub btnNoRepCorpSheet_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepCorpSheet.Click
        Call ResetCorpRep(CorpRepType.CorpSheet)
        cboCorpSheetRep.SelectedIndex = -1
        btnNoRepCorpSheet.Enabled = False
    End Sub

    Private Sub btnNoRepTransactions_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepTransactions.Click
        Call ResetCorpRep(CorpRepType.WalletTransactions)
        cboTransactionsRep.SelectedIndex = -1
        btnNoRepTransactions.Enabled = False
    End Sub

    Private Sub btnNoRepOrders_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepOrders.Click
        Call ResetCorpRep(CorpRepType.Orders)
        cboOrdersRep.SelectedIndex = -1
        btnNoRepOrders.Enabled = False
    End Sub

    Private Sub btnNoRepJournal_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepJournal.Click
        Call ResetCorpRep(CorpRepType.WalletJournal)
        cboJournalRep.SelectedIndex = -1
        btnNoRepJournal.Enabled = False
    End Sub

    Private Sub btnNoRepJobs_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepJobs.Click
        Call ResetCorpRep(CorpRepType.Jobs)
        cboJobsRep.SelectedIndex = -1
        btnNoRepJobs.Enabled = False
    End Sub

    Private Sub btnNoRepBalances_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepBalances.Click
        Call ResetCorpRep(CorpRepType.Balances)
        cboBalancesRep.SelectedIndex = -1
        btnNoRepBalances.Enabled = False
    End Sub

    Private Sub btnNoRepContracts_Click(sender As System.Object, e As System.EventArgs) Handles btnNoRepContracts.Click
        Call ResetCorpRep(CorpRepType.Contracts)
        cboContractsRep.SelectedIndex = -1
        btnNoRepContracts.Enabled = False
    End Sub

    Private Sub ResetCorpRep(RepType As CorpRepType)
        If SelectedCorpReps.ContainsKey(RepType) Then
            SelectedCorpReps.Remove(RepType)
        End If
    End Sub

#End Region

    Private Sub btnDeleteUndefinedJournals_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteUndefinedJournals.Click
        Const strSQL As String = "DELETE FROM walletJournal WHERE refTypeID = 0;"
        If Core.CustomDataFunctions.SetCustomData(strSQL) = -2 Then
            MessageBox.Show("Error deleting undefined entries from the Wallet Journal table!", "Delete Undefined Entries Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            MessageBox.Show("Successfully deleted undefined entries from the Wallet Journal table!", "Delete Undefined Entries Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

End Class
