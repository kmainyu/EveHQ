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
Imports System.Data.SqlServerCe
Imports System.Data.SqlClient
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Data.OleDb

Public Class frmUpgradeMDB

	Dim WithEvents FormWorker As New System.ComponentModel.BackgroundWorker

	Private Sub frmUpgradeMDB_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Me.Refresh()
		FormWorker.WorkerReportsProgress = True
		FormWorker.RunWorkerAsync()
	End Sub

	Private Sub CompareWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles FormWorker.DoWork
		Call Me.UpgradeMDBtoSQLCE()
	End Sub

	Private Sub CompareWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles FormWorker.RunWorkerCompleted
		Me.Close()
	End Sub

	Private Sub FormWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles FormWorker.ProgressChanged
		Me.lblStatus.Text = e.UserState.ToString
		Me.Refresh()
	End Sub

#Region "Database Upgrade Routines"

	Private Sub UpgradeMDBtoSQLCE()

		' Six Tables to upgrade:
		' 1. assetItemNames
		' 2. customPrices
		' 3. eveIDToName
		' 4. eveMail
		' 5. eveNotifications
		' 6. marketPrices

		' Step 1 - Get the old .MDB file
		FormWorker.ReportProgress(100, "Stage 1/14: Getting Old Database details...")
		Dim OldMDB As String = EveHQ.Core.HQ.EveHQSettings.DBDataFilename

		' Step 2 - Create a new .SDF database in the same location as the existing MDB
		' IMPORTANT! Set the connection strings now so we can create our tables
		FormWorker.ReportProgress(100, "Stage 2/14: Creating New Database...")
        Dim outputfile As String = Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.sdf")
        'MessageBox.Show("Creating database using path: " & outputfile, "Custom Database Location", MessageBoxButtons.OK, MessageBoxIcon.Information)
		' Try to create a new SQL CE DB
        Dim strConnection As String = "Data Source = " & ControlChars.Quote & outputfile & ControlChars.Quote & "; Max Database Size = 512; Max Buffer Size = 2048;"
		Try
			Dim SQLCE As New SqlCeEngine(strConnection)
			SQLCE.CreateDatabase()
			EveHQ.Core.HQ.EveHQSettings.DBDataFilename = outputfile
			EveHQ.Core.HQ.EveHQDataConnectionString = strConnection
		Catch e As Exception
			EveHQ.Core.HQ.dataError = "Unable to create SQL CE database in " & outputfile & ControlChars.CrLf & ControlChars.CrLf & e.Message
		End Try

		' Step 3 - Create our tables
		Call Me.CreateAllDatabaseTables()

		' Step 4 - Convert the tables
		Call Me.ConvertAllDatabaseTables(OldMDB)

	End Sub

#End Region

#Region "Table Creation Routines"

	Private Sub CreateAllDatabaseTables()

		' Create assetItemNames table
		FormWorker.ReportProgress(100, "Stage 3/14: Creating AssetItemNames Table...")
		Call Me.CreateAssetItemNamesTable()

		' Create customPrices table
		FormWorker.ReportProgress(100, "Stage 4/14: Creating CustomPrices Table...")
		Call EveHQ.Core.DataFunctions.CreateCustomPricesTable()

		' Create eveIDToName table
		FormWorker.ReportProgress(100, "Stage 5/14: Creating EveIDToName Table...")
		Call EveHQ.Core.DataFunctions.CheckForIDNameTable()

		' Create eveMail table
		FormWorker.ReportProgress(100, "Stage 6/14: Creating EveMail Table...")
		Call EveHQ.Core.DataFunctions.CheckForEveMailTable()

		' Create eveNotifications table
		FormWorker.ReportProgress(100, "Stage 7/14: Creating EveNotifications Table...")
		Call EveHQ.Core.DataFunctions.CheckForEveNotificationTable()

		' Create marketPrices table
		FormWorker.ReportProgress(100, "Stage 8/14: Creating MarketPrices Table...")
		Call EveHQ.Core.DataFunctions.CreateMarketPricesTable()

	End Sub

	Private Function CreateAssetItemNamesTable() As Boolean
		' Create the database table 

		Dim strSQL As New StringBuilder
		strSQL.AppendLine("CREATE TABLE assetItemNames")
		strSQL.AppendLine("(")
		strSQL.AppendLine("  itemID         bigint,")
		strSQL.AppendLine("  itemName       nvarchar(100),")
		strSQL.AppendLine("")
		strSQL.AppendLine("  CONSTRAINT assetItemNames_PK PRIMARY KEY (itemID)")
		strSQL.AppendLine(")")
		If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
			Return True
		Else
			MessageBox.Show("There was an error creating the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Return False
		End If

	End Function

#End Region

#Region "Table Conversion Routines"

	Private Sub ConvertAllDatabaseTables(ByVal OldMDB As String)

		' Convert assetItemNames table
		FormWorker.ReportProgress(100, "Stage 9/14: Converting AssetItemNames Table...")
		Call Me.ConvertAssetItemNamesTable(OldMDB)

		' Convert customPrices table
		FormWorker.ReportProgress(100, "Stage 10/14: Converting CustomPrices Table...")
		Call Me.ConvertCustomPricesTable(OldMDB)

		' Convert eveIDToName table
		FormWorker.ReportProgress(100, "Stage 11/14: Converting EveIDToName Table...")
		Call Me.ConvertEveIDToNameTable(OldMDB)

		' Convert eveMail table
		FormWorker.ReportProgress(100, "Stage 12/14: Converting EveMail Table...")
		Call Me.ConvertEveMailTable(OldMDB)

		' Convert eveNotifications table
		FormWorker.ReportProgress(100, "Stage 13/14: Converting EveNotifications Table...")
		Call Me.ConvertEveNotificationsTable(OldMDB)

		' Convert marketPrices table
		FormWorker.ReportProgress(100, "Stage 14/14: Converting MarketPrices Table...")
		Call Me.ConvertMarketPricesTable(OldMDB)

	End Sub

	Private Function GetMDBData(ByVal OldMDB As String, ByVal strSQL As String) As DataSet

		Dim EveHQData As New DataSet
		EveHQData.Clear()
		EveHQData.Tables.Clear()

		Dim conn As New OleDbConnection
		conn.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & OldMDB
		Try
			conn.Open()
			Dim da As New OleDbDataAdapter(strSQL, conn)
			da.SelectCommand.CommandTimeout = EveHQ.Core.HQ.EveHQSettings.DBTimeout
			da.Fill(EveHQData, "EveHQData")
			conn.Close()
			Return EveHQData
		Catch e As Exception
			Dim msg As New StringBuilder
			msg.AppendLine("Database1: " & EveHQ.Core.HQ.EveHQSettings.DBFilename)
			msg.AppendLine("Database2: " & EveHQ.Core.HQ.EveHQSettings.DBDataFilename)
			msg.AppendLine("Using App: " & EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB.ToString)
			msg.AppendLine("Connection String: " & conn.ConnectionString)
			msg.AppendLine("SQL: " & strSQL)
			msg.AppendLine("Message: " & e.Message)
			If e.InnerException IsNot Nothing Then
				msg.AppendLine("Inner Ex: " & e.InnerException.Message)
			End If
			EveHQ.Core.HQ.dataError = msg.ToString
			Return Nothing
		Finally
			If conn.State = ConnectionState.Open Then
				conn.Close()
			End If
		End Try

	End Function

	Private Function ConvertTable(ByVal EveData As DataSet, ByVal strSQL As String, ByVal FieldList As List(Of String)) As Boolean

		Dim conn As New SqlCeConnection
		conn.ConnectionString = EveHQ.Core.HQ.EveHQDataConnectionString
		Try
			conn.Open()

			Dim ds As New DataSet
			Dim da As New SqlCeDataAdapter(strSQL, conn)
			Dim cb As New SqlCeCommandBuilder(da)

			da.Fill(ds, "EveData")

			For Each ConvertRow As DataRow In EveData.Tables(0).Rows
				Dim rec As DataRow = ds.Tables(0).NewRow
				For Each Field As String In FieldList
					' Check if field exists in the original DB
					If EveData.Tables(0).Columns.IndexOf(Field) <> -1 Then
						rec.Item(Field) = ConvertRow.Item(Field)
					End If
				Next
				ds.Tables("EveData").Rows.Add(rec)
			Next

			cb.GetInsertCommand()
			da.Update(ds, "EveData")

			Return True
		Catch e As Exception
			EveHQ.Core.HQ.dataError = e.Message
			Return False
		Finally
			If conn.State = ConnectionState.Open Then
				conn.Close()
			End If
		End Try

	End Function

	Private Function ConvertAssetItemNamesTable(ByVal OldMDB As String) As Boolean

		' Get all the data from the old table
		Dim strSQL As String = "SELECT * FROM assetItemNames;"
		Dim EveData As DataSet = GetMDBData(OldMDB, strSQL)
		If EveData IsNot Nothing Then
			If EveData.Tables(0).Rows.Count > 0 Then

				' Set up a list of convertable fields
				Dim FieldList As New List(Of String)

				' Add the field names to the list
				FieldList.Add("itemID")
				FieldList.Add("itemName")

				' Convert the table with the field list
				Call ConvertTable(EveData, strSQL, FieldList)

			End If
		End If

	End Function

	Private Function ConvertCustomPricesTable(ByVal OldMDB As String) As Boolean

		' Get all the data from the old table
		Dim strSQL As String = "SELECT * FROM customPrices;"
		Dim EveData As DataSet = GetMDBData(OldMDB, strSQL)
		If EveData IsNot Nothing Then
			If EveData.Tables(0).Rows.Count > 0 Then

				' Set up a list of convertable fields
				Dim FieldList As New List(Of String)

				' Add the field names to the list
				FieldList.Add("typeID")
				FieldList.Add("price")
				FieldList.Add("priceDate")

				' Convert the table with the field list
				Call ConvertTable(EveData, strSQL, FieldList)

			End If
		End If

	End Function

	Private Function ConvertEveIDToNameTable(ByVal OldMDB As String) As Boolean

		' Get all the data from the old table
		Dim strSQL As String = "SELECT * FROM eveIDToName;"
		Dim EveData As DataSet = GetMDBData(OldMDB, "SELECT * FROM eveIDToName;")
		If EveData IsNot Nothing Then
			If EveData.Tables(0).Rows.Count > 0 Then

				' Set up a list of convertable fields
				Dim FieldList As New List(Of String)

				' Add the field names to the list
				FieldList.Add("eveID")
				FieldList.Add("eveName")

				' Convert the table with the field list
				Call ConvertTable(EveData, strSQL, FieldList)

			End If
		End If

	End Function

	Private Function ConvertEveMailTable(ByVal OldMDB As String) As Boolean

		' Get all the data from the old table
		Dim strSQL As String = "SELECT * FROM eveMail;"
		Dim EveData As DataSet = GetMDBData(OldMDB, "SELECT * FROM eveMail;")
		If EveData IsNot Nothing Then
			If EveData.Tables(0).Rows.Count > 0 Then
				
				' Set up a list of convertable fields
				Dim FieldList As New List(Of String)

				' Add the field names to the list
				FieldList.Add("messageKey")
				FieldList.Add("messageID")
				FieldList.Add("originatorID")
				FieldList.Add("senderID")
				FieldList.Add("sentDate")
				FieldList.Add("title")
				FieldList.Add("toCorpOrAllianceID")
				FieldList.Add("toCharacterIDs")
				FieldList.Add("toListIDs")
				FieldList.Add("readMail")
				FieldList.Add("messageBody")

				' Convert the table with the field list
				Call ConvertTable(EveData, strSQL, FieldList)

			End If
		End If

	End Function

	Private Function ConvertEveNotificationsTable(ByVal OldMDB As String) As Boolean

		' Get all the data from the old table
		Dim strSQL As String = "SELECT * FROM eveNotifications;"
		Dim EveData As DataSet = GetMDBData(OldMDB, "SELECT * FROM eveNotifications;")
		If EveData IsNot Nothing Then
			If EveData.Tables(0).Rows.Count > 0 Then

				' Set up a list of convertable fields
				Dim FieldList As New List(Of String)

				' Add the field names to the list
				FieldList.Add("messageKey")
				FieldList.Add("messageID")
				FieldList.Add("typeID")
				FieldList.Add("originatorID")
				FieldList.Add("senderID")
				FieldList.Add("sentDate")
				FieldList.Add("readMail")

				' Convert the table with the field list
				Call ConvertTable(EveData, strSQL, FieldList)

			End If
		End If

	End Function

	Private Function ConvertMarketPricesTable(ByVal OldMDB As String) As Boolean

		' Get all the data from the old table
		Dim strSQL As String = "SELECT * FROM marketPrices;"
		Dim EveData As DataSet = GetMDBData(OldMDB, "SELECT * FROM marketPrices;")
		If EveData IsNot Nothing Then
			If EveData.Tables(0).Rows.Count > 0 Then

				' Set up a list of convertable fields
				Dim FieldList As New List(Of String)

				' Add the field names to the list
				FieldList.Add("typeID")
				FieldList.Add("price")
				FieldList.Add("priceDate")

				' Convert the table with the field list
				Call ConvertTable(EveData, strSQL, FieldList)

			End If
		End If

	End Function

#End Region

End Class

