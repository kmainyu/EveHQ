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
Imports DevComponents.AdvTree
Imports System.Windows.Forms

Public Class frmJournalCheck

    Dim WithEvents _journalDiffWorker As New BackgroundWorker
    ReadOnly _owners As New List(Of String)
    ReadOnly _journalDiffs As New SortedList(Of String, WalletJournalDiff) ' Key = CurrKey
    Dim _rc As Integer = 0
    Dim _pc As Integer = 0

    Private Sub frmJournalCheck_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown
        _journalDiffWorker.WorkerReportsProgress = True
        _journalDiffWorker.RunWorkerAsync()
    End Sub

    Private Sub _journalDiffWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles _journalDiffWorker.DoWork
        Call CheckJournals()
    End Sub

    Private Sub _journalDiffWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles _journalDiffWorker.ProgressChanged
        If IsNumeric(e.UserState) = False Then
            ' Update the status label
            lblInfo.Text = e.UserState.ToString
        Else
            ' Update the progress bar
            Dim progress As Integer = CInt(e.UserState)
            pbProgress.Value = progress
        End If
    End Sub

    Private Sub _journalDiffWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles _journalDiffWorker.RunWorkerCompleted
        pbProgress.Visible = False
        picProgress.Image = My.Resources.Info32
        lblInfo.Text = "Updating journal difference list..."
        ' Display a list of the diffs if we have any
        adtJournals.BeginUpdate()
        adtJournals.Nodes.Clear()
        If _journalDiffs.Count > 0 Then
            For Each journalDiff As WalletJournalDiff In _journalDiffs.Values
                Dim newDiff As New Node
                newDiff.Name = journalDiff.CurrKey
                newDiff.Text = journalDiff.OwnerName
                newDiff.Cells.Add(New Cell(journalDiff.WalletID.ToString))
                newDiff.Cells.Add(New Cell(journalDiff.PrevDate.ToString))
                newDiff.Cells.Add(New Cell(journalDiff.CurrDate.ToString))
                newDiff.Cells.Add(New Cell(journalDiff.PrevKey.ToString))
                newDiff.Cells.Add(New Cell(journalDiff.CurrKey.ToString))
                newDiff.Cells.Add(New Cell(journalDiff.PrevBal.ToString("N2")))
                newDiff.Cells.Add(New Cell(journalDiff.Amount.ToString("N2")))
                newDiff.Cells.Add(New Cell(journalDiff.TaxAmount.ToString("N2")))
                newDiff.Cells.Add(New Cell(journalDiff.CurrBal.ToString("N2")))
                newDiff.Cells.Add(New Cell(journalDiff.Difference.ToString("N2")))
                adtJournals.Nodes.Add(newDiff)
            Next
            btnFixJournal.Enabled = True
        Else
            adtJournals.Nodes.Add(New Node("No Differences"))
            btnFixJournal.Enabled = False
        End If
        adtJournals.EndUpdate()
        ' Finish!
        lblInfo.Text = "Journal Checker has finished checking " & _pc.ToString("N0") & " records (" & _rc.ToString("N0") & " expected) - " & _journalDiffs.Count.ToString("N0") & " differences identifed."
    End Sub

    Private Sub CheckJournals()
        ' Check the number of records we have (no point doing this if no records, and we need the count)
        Dim strSQL As String = "SELECT COUNT(*) AS TR FROM walletJournal;"
        Dim sqlData As DataSet = Core.CustomDataFunctions.GetCustomData(strSQL)
        If sqlData IsNot Nothing Then
            If sqlData.Tables(0).Rows.Count > 0 Then
                _rc = CInt(sqlData.Tables(0).Rows(0).Item("TR"))
                If _rc > 0 Then
                    ' Set the progress bar values
                    pbProgress.Visible = True
                    pbProgress.Minimum = 0
                    pbProgress.Maximum = _rc
                    ' Check for a list of owners which we need to check specific journals for
                    _owners.Clear()
                    strSQL = "SELECT DISTINCT charName FROM walletJournal;"
                    sqlData = Core.CustomDataFunctions.GetCustomData(strSQL)
                    If sqlData IsNot Nothing Then
                        If sqlData.Tables(0).Rows.Count > 0 Then
                            For Each sqlRow As DataRow In sqlData.Tables(0).Rows
                                _owners.Add(sqlRow.Item("charName").ToString)
                            Next
                        End If
                    End If
                    ' Only proceed if there are some owners
                    If _owners.Count > 0 Then
                        Call CheckOwners()
                    End If
                End If
            End If
        End If
    End Sub

   Private Sub CheckOwners()
        ' Set the progress counter and the diff counter
        _pc = 0
        _journalDiffs.Clear()
        ' Check each wallet for each owner
        For Each prismOwner As String In _owners
            For walletID As Integer = 1000 To 1006
                ' Get the wallet data for this owner and wallet
                _journalDiffWorker.ReportProgress(100, "Checking Wallet Journal for " & prismOwner & " (WalletID: " & walletID.ToString & ")...")
                Dim strSQL As String = "SELECT * FROM walletJournal WHERE charName='" & prismOwner.Replace("'", "''") & "' AND walletID=" & walletID.ToString & " ORDER BY transKey;"
                Dim sqlData As DataSet = Core.CustomDataFunctions.GetCustomData(strSQL)
                If sqlData IsNot Nothing Then
                    If sqlData.Tables(0).Rows.Count > 0 Then
                        ' Set up variables for the check
                        Dim journalAmount As Double
                        Dim taxAmount As Double
                        Dim expBalance As Double
                        Dim actualBalance As Double
                        Dim lastBalance As Double = 0
                        Dim lastRefKey As String = ""
                        Dim lastDate As Date
                        Dim lastRef As Long = 0
                        For Each sqlRow As DataRow In sqlData.Tables(0).Rows
                            If lastRefKey <> "" Then
                                ' Get relevant figures
                                journalAmount = CDbl(sqlRow.Item("amount"))
                                taxAmount = CDbl(sqlRow.Item("taxAmount"))
                                actualBalance = CDbl(sqlRow.Item("balance"))
                                ' Check if this is a tax entry only
                                If journalAmount <> -taxAmount Then
                                    ' Calculate the expected balance
                                    expBalance = lastBalance + journalAmount - taxAmount
                                    ' Check if the expected balance is different to the actual
                                    If Math.Abs(Math.Round(expBalance - actualBalance, 2, MidpointRounding.AwayFromZero)) > 0.01 Then
                                        ' We have a difference so store it for review
                                        Dim newDiff As New WalletJournalDiff
                                        newDiff.OwnerID = CInt(sqlRow.Item("charID"))
                                        newDiff.OwnerName = prismOwner
                                        newDiff.WalletID = walletID
                                        newDiff.Amount = journalAmount
                                        newDiff.TaxAmount = taxAmount
                                        newDiff.PrevBal = lastBalance
                                        newDiff.PrevDate = lastDate
                                        newDiff.PrevKey = lastRefKey
                                        newDiff.PrevRef = lastRef
                                        newDiff.CurrBal = actualBalance
                                        newDiff.CurrDate = CDate(sqlRow.Item("transDate"))
                                        newDiff.CurrKey = sqlRow.Item("transKey").ToString
                                        newDiff.CurrRef = CLng(sqlRow.Item("transRef"))
                                        newDiff.Difference = newDiff.CurrBal - newDiff.PrevBal - (newDiff.Amount - newDiff.TaxAmount)
                                        _journalDiffs.Add(newDiff.CurrKey, newDiff)
                                    End If
                                    ' Update the Last items
                                    lastBalance = actualBalance
                                    lastRefKey = sqlRow.Item("transKey").ToString
                                    lastDate = CDate(sqlRow.Item("transDate"))
                                    lastRef = CLng(sqlRow.Item("transRef"))
                                End If
                            Else
                                ' Set the Last items
                                lastBalance = CDbl(sqlRow.Item("balance"))
                                lastRefKey = sqlRow.Item("transKey").ToString
                                lastDate = CDate(sqlRow.Item("transDate"))
                                lastRef = CLng(sqlRow.Item("transRef"))
                            End If
                            ' Update the progress bar status
                            _pc += 1
                            _journalDiffWorker.ReportProgress(100, _pc)
                        Next
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub btnFixJournal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFixJournal.Click
        ' Get confirmation of wanting to write dummy entries into the database
        Dim msg As String = "Are you sure you want to popualate the wallet journal database with dummy entries?" & ControlChars.CrLf & ControlChars.CrLf
        msg &= "(These will be marked to deal with later if required)"
        Dim reply As DialogResult = MessageBox.Show(msg, "Confirm Update Wallet Journal", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then

            ' Cycle through our list of entries and create a wallet journal list from them

            For Each wjd As WalletJournalDiff In _journalDiffs.Values

                Dim walletJournals As New SortedList(Of String, WalletJournalItem)

                Dim wji As New WalletJournalItem

                ' Parse Journal
                wji.JournalDate = wjd.CurrDate.AddSeconds(-1) ' Set to 1s before the current transaction
                wji.RefID = wjd.CurrRef - 1 ' If we're missing stuff, it needs to be between PrevRef and CurrRef
                wji.RefTypeID = 0 ' "Undefined" RefTypeID
                wji.OwnerName1 = ""
                wji.OwnerID1 = "0"
                wji.OwnerName2 = ""
                wji.OwnerID2 = "0"
                wji.ArgName1 = ""
                wji.ArgID1 = "0"
                wji.Amount = wjd.Difference
                wji.Balance = wjd.PrevBal + wjd.Difference
                wji.Reason = "Dummy Entry From EveHQ Prism"
                wji.TaxReceiverID = "0"
                wji.TaxAmount = 0

                ' Add the new journal to the collection
                If walletJournals.ContainsKey(wji.RefID.ToString) = False Then
                    walletJournals.Add(wji.RefID.ToString, wji)
                End If

                ' Write the new journals to the DB
                PrismDataFunctions.WriteWalletJournalsToDB(walletJournals, wjd.OwnerID, wjd.OwnerName, wjd.WalletID, 0)

            Next

            MessageBox.Show("Wallet Journal Database has been successfully updated. A further check will now be run to ensure data continutity - Press OK to continue with the check.", "Journal Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            picProgress.Image = My.Resources.Spinner
            _journalDiffWorker.WorkerReportsProgress = True
            _journalDiffWorker.RunWorkerAsync()

        End If
    End Sub
End Class
