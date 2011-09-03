' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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

	Dim WithEvents JournalWorker As New BackgroundWorker
	Dim Owners As New List(Of String)
	Dim JournalDiffs As New SortedList(Of String, WalletJournalDiff) ' Key = CurrKey
	Dim RC As Integer = 0
	Dim PC As Integer = 0

	Private Sub frmJournalCheck_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
		JournalWorker.WorkerReportsProgress = True
		JournalWorker.RunWorkerAsync()
	End Sub

	Private Sub JournalWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles JournalWorker.DoWork
		Call Me.CheckJournals()
	End Sub

	Private Sub JournalWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles JournalWorker.ProgressChanged
		If IsNumeric(e.UserState) = False Then
			' Update the status label
			lblInfo.Text = e.UserState.ToString
		Else
			' Update the progress bar
			Dim progress As Integer = CInt(e.UserState)
			pbProgress.Value = progress
		End If
	End Sub

	Private Sub JournalWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles JournalWorker.RunWorkerCompleted
		pbProgress.Visible = False
		picProgress.Image = My.Resources.Info32
		lblInfo.Text = "Updating journal difference list..."
		' Display a list of the diffs if we have any
		adtJournals.BeginUpdate()
		adtJournals.Nodes.Clear()
		If JournalDiffs.Count > 0 Then
			For Each JournalDiff As WalletJournalDiff In JournalDiffs.Values
				Dim NewDiff As New Node
				NewDiff.Name = JournalDiff.CurrKey
				NewDiff.Text = JournalDiff.OwnerName
				NewDiff.Cells.Add(New Cell(JournalDiff.WalletID.ToString))
				NewDiff.Cells.Add(New Cell(JournalDiff.PrevDate.ToString))
				NewDiff.Cells.Add(New Cell(JournalDiff.CurrDate.ToString))
				NewDiff.Cells.Add(New Cell(JournalDiff.PrevKey.ToString))
				NewDiff.Cells.Add(New Cell(JournalDiff.CurrKey.ToString))
				NewDiff.Cells.Add(New Cell(JournalDiff.PrevBal.ToString("N2")))
				NewDiff.Cells.Add(New Cell(JournalDiff.Amount.ToString("N2")))
				NewDiff.Cells.Add(New Cell(JournalDiff.TaxAmount.ToString("N2")))
				NewDiff.Cells.Add(New Cell(JournalDiff.CurrBal.ToString("N2")))
				NewDiff.Cells.Add(New Cell(JournalDiff.Difference.ToString("N2")))
				adtJournals.Nodes.Add(NewDiff)
			Next
			btnFixJournal.Enabled = True
		Else
			adtJournals.Nodes.Add(New Node("No Differences"))
			btnFixJournal.Enabled = False
		End If
		adtJournals.EndUpdate()
		' Finish!
		lblInfo.Text = "Journal Checker has finished checking " & PC.ToString("N0") & " records (" & RC.ToString("N0") & " expected) - " & JournalDiffs.Count.ToString("N0") & " differences identifed."
	End Sub

	Private Sub CheckJournals()
		' Check the number of records we have (no point doing this if no records, and we need the count)
		Dim strSQL As String = "SELECT COUNT(*) AS TR FROM walletJournal;"
		Dim SQLData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
		If SQLData IsNot Nothing Then
			If SQLData.Tables(0).Rows.Count > 0 Then
				RC = CInt(SQLData.Tables(0).Rows(0).Item("TR"))
				If RC > 0 Then
					' Set the progress bar values
					pbProgress.Visible = True
					pbProgress.Minimum = 0
					pbProgress.Maximum = RC
					' Check for a list of owners which we need to check specific journals for
					Owners.Clear()
					strSQL = "SELECT DISTINCT charName FROM walletJournal;"
					SQLData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
					If SQLData IsNot Nothing Then
						If SQLData.Tables(0).Rows.Count > 0 Then
							For Each SQLRow As DataRow In SQLData.Tables(0).Rows
								Owners.Add(SQLRow.Item("charName").ToString)
							Next
						End If
					End If
					' Only proceed if there are some owners
					If Owners.Count > 0 Then
						Call Me.CheckOwners()
					End If
				End If
			End If
		End If
	End Sub

	Private Sub CheckOwners()
		' Set the progress counter and the diff counter
		PC = 0
		JournalDiffs.Clear()
		' Check each wallet for each owner
		For Each Owner As String In Owners
			For walletID As Integer = 1000 To 1006
				' Get the wallet data for this owner and wallet
				JournalWorker.ReportProgress(100, "Checking Wallet Journal for " & Owner & " (WalletID: " & walletID.ToString & ")...")
				Dim strSQL As String = "SELECT * FROM walletJournal WHERE charName='" & Owner.Replace("'", "''") & "' AND walletID=" & walletID.ToString & " ORDER BY transKey;"
				Dim SQLData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
				If SQLData IsNot Nothing Then
					If SQLData.Tables(0).Rows.Count > 0 Then
						' Set up variables for the check
						Dim JournalAmount As Double = 0
						Dim TaxAmount As Double = 0
						Dim ExpBalance As Double = 0
						Dim ActualBalance As Double = 0
						Dim LastBalance As Double = 0
						Dim LastRefKey As String = ""
						Dim LastDate As Date
						Dim LastRef As Long = 0
						Dim BalDiff As Double = 0
						For Each SQLRow As DataRow In SQLData.Tables(0).Rows
							If LastRefKey <> "" Then
								' Get relevant figures
								JournalAmount = CDbl(SQLRow.Item("amount"))
								TaxAmount = CDbl(SQLRow.Item("taxAmount"))
								ActualBalance = CDbl(SQLRow.Item("balance"))
								' Check if this is a tax entry only
								If JournalAmount <> -TaxAmount Then
									' Calculate the expected balance
									ExpBalance = LastBalance + JournalAmount - TaxAmount
									' Check if the expected balance is different to the actual
									If Math.Abs(Math.Round(ExpBalance - ActualBalance, 2)) > 0.01 Then
										' We have a difference so store it for review
										BalDiff = ExpBalance - ActualBalance
										Dim NewDiff As New WalletJournalDiff
										NewDiff.OwnerID = CInt(SQLRow.Item("charID"))
										NewDiff.OwnerName = Owner
										NewDiff.WalletID = walletID
										NewDiff.Amount = JournalAmount
										NewDiff.TaxAmount = TaxAmount
										NewDiff.PrevBal = LastBalance
										NewDiff.PrevDate = LastDate
										NewDiff.PrevKey = LastRefKey
										NewDiff.PrevRef = LastRef
										NewDiff.CurrBal = ActualBalance
										NewDiff.CurrDate = CDate(SQLRow.Item("transDate"))
										NewDiff.CurrKey = SQLRow.Item("transKey").ToString
										NewDiff.CurrRef = CLng(SQLRow.Item("transRef"))
										NewDiff.Difference = NewDiff.CurrBal - NewDiff.PrevBal - (NewDiff.Amount - NewDiff.TaxAmount)
										JournalDiffs.Add(NewDiff.CurrKey, NewDiff)
									End If
									' Update the Last items
									LastBalance = ActualBalance
									LastRefKey = SQLRow.Item("transKey").ToString
									LastDate = CDate(SQLRow.Item("transDate"))
									LastRef = CLng(SQLRow.Item("transRef"))
								End If
							Else
								' Set the Last items
								LastBalance = CDbl(SQLRow.Item("balance"))
								LastRefKey = SQLRow.Item("transKey").ToString
								LastDate = CDate(SQLRow.Item("transDate"))
								LastRef = CLng(SQLRow.Item("transRef"))
							End If
							' Update the progress bar status
							PC += 1
							JournalWorker.ReportProgress(100, PC)
						Next
					End If
				End If
			Next
		Next
	End Sub

	Private Sub btnFixJournal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFixJournal.Click
		' Get confirmation of wanting to write dummy entries into the database
		Dim msg As String = "Are you sure you want to popualate the wallet journal database with dummy entries?" & ControlChars.CrLf & ControlChars.CrLf
		msg &= "(These will be marked to deal with later if required)"
		Dim reply As DialogResult = MessageBox.Show(msg, "Confirm Update Wallet Journal", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
		If reply = Windows.Forms.DialogResult.Yes Then
			' Cycle through our list of entries and create a wallet journal list from them
			Dim WalletJournals As New SortedList(Of Long, WalletJournalItem)

			' Setup the default header
			Dim strInsert As String = "INSERT INTO walletJournal (transDate, transRef, transKey, refTypeID, ownerName1, ownerID1, ownerName2, ownerID2, argName1, argID1, amount, balance, reason, taxID, taxAmount, charID, charName, walletID, importDate) VALUES "

			For Each WJD As WalletJournalDiff In JournalDiffs.Values

				Dim WJI As New WalletJournalItem

				' Parse Journal
				WJI.JournalDate = WJD.CurrDate.AddSeconds(-1) ' Set to 1s before the current transaction
				WJI.RefID = WJD.CurrRef - 1	' If we're missing stuff, it needs to be between PrevRef and CurrRef
				WJI.RefTypeID = 0 ' "Undefined" RefTypeID
				WJI.OwnerName1 = ""
				WJI.OwnerID1 = "0"
				WJI.OwnerName2 = ""
				WJI.OwnerID2 = "0"
				WJI.ArgName1 = ""
				WJI.ArgID1 = "0"
				WJI.Amount = WJD.Difference
				WJI.Balance = WJD.PrevBal + WJD.Difference
				WJI.Reason = "Dummy Entry From EveHQ Prism"
				WJI.TaxReceiverID = "0"
				WJI.TaxAmount = 0

				If WalletJournals.ContainsKey(WJI.RefID) = False Then
					WalletJournals.Add(WJI.RefID, WJI)
				End If

				' Write the journal to the database!
				Call Prism.DataFunctions.WriteSingleWalletJournalToDB(WJI, strInsert, WJD.OwnerID, WJD.OwnerName, WJD.WalletID)

			Next
			MessageBox.Show("Wallet Journal Database has been successfully updated. A further check will now be run to ensure data continutity - Press OK to continue with the check.", "Journal Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

			picProgress.Image = My.Resources.Spinner
			JournalWorker.WorkerReportsProgress = True
			JournalWorker.RunWorkerAsync()

		End If
	End Sub
End Class
