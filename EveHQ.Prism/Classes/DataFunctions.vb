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

Imports System.Windows.Forms
Imports System.Text
Imports System.Xml

Public Class DataFunctions

    Private Shared IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Private Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    Public Shared Function CheckDatabaseTables() As Boolean
        If CheckAssetItemNameDBTable() = True Then
            If CheckWalletJournalDBTable() = True Then
                If CheckWalletTransDBTable() = True Then
                    If CheckInventionResultsDBTable() = True Then
                        Return True
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Private Shared Function CheckInventionResultsDBTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("inventionResults") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTable = True
            Else
                ' We have the Db and table so we can return a good result
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE inventionResults")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  resultID       bigint IDENTITY(1,1),") ' Autonumber for this entry
            strSQL.AppendLine("  jobID          bigint,")
            strSQL.AppendLine("  resultDate     datetime,")
            strSQL.AppendLine("  BPID           int,")
            strSQL.AppendLine("  typeID         int,")
            strSQL.AppendLine("  installerID    bigint,")
            strSQL.AppendLine("  result         int,") ' 0=failed, 1=success
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT inventionResults_PK PRIMARY KEY (resultID)")
            strSQL.AppendLine(")")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
                Return True
            Else
                MessageBox.Show("There was an error creating the Invention Results database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

    End Function

    Private Shared Function CheckAssetItemNameDBTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("assetItemNames") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTable = True
            Else
				' We have the Db and table so we can return a good result
				' Check for the upgrade from v1 to v2
				Call UpgradeAssetItemNameDBTable()
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
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
        End If

	End Function

	Private Shared Function UpgradeAssetItemNameDBTable() As Boolean
		Dim strSQL As String = "SELECT * FROM assetItemNames;"
		Dim ColData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)

		' Get the data type
		Dim dt As Type = ColData.Tables(0).Columns("itemID").DataType

		If dt Is GetType(Integer) Then
			' Requires an upgrade!

			' Stage 1 - create a new column
			strSQL = "ALTER TABLE assetItemNames ADD tempItemID bigint NOT NULL DEFAULT 0;"
			EveHQ.Core.DataFunctions.SetData(strSQL)

			' Stage 2 - copy the data into the new column
			strSQL = "UPDATE assetItemNames SET tempItemID = itemID;"
			EveHQ.Core.DataFunctions.SetData(strSQL)

			' Stage 3 - drop the PK
			strSQL = "ALTER TABLE assetItemNames DROP CONSTRAINT assetItemNames_PK"
			EveHQ.Core.DataFunctions.SetData(strSQL)

			' Stage 4 - drop the old column
			strSQL = "ALTER TABLE assetItemNames DROP COLUMN itemID;"
			EveHQ.Core.DataFunctions.SetData(strSQL)

			' Stage 5 - recreate the old column
			strSQL = "ALTER TABLE assetItemNames ADD itemID bigint NOT NULL DEFAULT 0;"
			EveHQ.Core.DataFunctions.SetData(strSQL)

			' Stage 6 - copy the data into the new column
			strSQL = "UPDATE assetItemNames SET itemID = tempItemID;"
			EveHQ.Core.DataFunctions.SetData(strSQL)

			' Stage 7 - recreate the PK
			strSQL = "ALTER TABLE assetItemNames ADD CONSTRAINT assetItemNames_PK PRIMARY KEY (itemID);"
			EveHQ.Core.DataFunctions.SetData(strSQL)

			' Stage 8 - drop the temp column
			strSQL = "ALTER TABLE assetItemNames DROP COLUMN tempItemID;"
			EveHQ.Core.DataFunctions.SetData(strSQL)

		End If

		Return True
	End Function

	Private Shared Function CheckWalletTransDBTable() As Boolean
		Dim CreateTable As Boolean = False
		Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
		If tables IsNot Nothing Then
			If tables.Contains("walletTransactions") = False Then
				' The DB exists but the table doesn't so we'll create this
				CreateTable = True
			Else
				' We have the Db and table so we can return a good result
				Return True
			End If
		Else
			' Database doesn't exist?
			Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
			msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
			msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
			MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
			If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
				MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return False
			Else
				MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
				CreateTable = True
			End If
		End If

		' Create the database table 
		If CreateTable = True Then
			Dim strSQL As New StringBuilder
			strSQL.AppendLine("CREATE TABLE walletTransactions")
			strSQL.AppendLine("(")
			strSQL.AppendLine("  transID        bigint IDENTITY(1,1),")	' Autonumber for this entry
			strSQL.AppendLine("  transDate      datetime,")
			strSQL.AppendLine("  transRef       bigint,") ' Eve Reference ID
			strSQL.AppendLine("  transKey       nvarchar(100),") ' Unique Key based on date and ref
			strSQL.AppendLine("  quantity       float,")
			strSQL.AppendLine("  typeName       nvarchar(100),")
			strSQL.AppendLine("  typeID         int,")
			strSQL.AppendLine("  groupID        int,")
			strSQL.AppendLine("  categoryID     int,")
			strSQL.AppendLine("  marketgroupID  int,")
			strSQL.AppendLine("  price          float,")
			strSQL.AppendLine("  clientID       bigint,")
			strSQL.AppendLine("  clientName     nvarchar(100),")
			strSQL.AppendLine("  stationID      bigint,")
			strSQL.AppendLine("  stationName    nvarchar(100),")
			strSQL.AppendLine("  transType      nvarchar(5),")	' 1 = Buy, 2=Sell
			strSQL.AppendLine("  transFor       nvarchar(15),")	 ' 1 = Personal, 2 = Corporation
			strSQL.AppendLine("  systemID       bigint,")
			strSQL.AppendLine("  constID        bigint,")
			strSQL.AppendLine("  regionID       bigint,")
			strSQL.AppendLine("  charID         bigint,")
			strSQL.AppendLine("  charName       nvarchar(100),")
			strSQL.AppendLine("  walletID       int,")
			strSQL.AppendLine("  importDate     datetime,")
			strSQL.AppendLine("")
			strSQL.AppendLine("  CONSTRAINT walletTransactions_PK PRIMARY KEY (transID)")
			strSQL.AppendLine(")")
			If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
				Return True
			Else
				MessageBox.Show("There was an error creating the Wallet Transactions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return False
			End If
		End If

	End Function

	Public Shared Sub WriteWalletTransactionsToDB(ByVal WalletXML As XmlDocument, ByVal IsCorp As Boolean, ByVal CharID As Integer, ByVal CharName As String, ByVal WalletID As Integer)

		If WalletXML IsNot Nothing Then
			' Get the last referenceID for the wallet
            Dim LastTrans As Long = GetLastWalletID(WalletTypes.Transactions, CharID, WalletID)

            ' Setup the default header
			Dim strInsert As String = "INSERT INTO walletTransactions (transDate, transRef, transKey, quantity, typeName, typeID, groupID, categoryID, marketGroupID, price, clientID, clientName, stationID, stationName, transType, transFor, systemID, constID, regionID, charID, charName, walletID, importDate) VALUES "

			' Go through each journal entry and see if we should write it
			Dim TransList As XmlNodeList = WalletXML.SelectNodes("/eveapi/result/rowset/row")

			For Each Trans As XmlNode In TransList
				Dim CurrentTrans As Long = CLng(Trans.Attributes.GetNamedItem("transactionID").Value)
				' Only write if it's something we haven't seen before i.e. is above our last transaction
				If CurrentTrans > LastTrans Then

					' Write the details
					If WriteSingleWalletTransactionToDB(Trans, strInsert, CharID, CharName, WalletID) = False Then
						MessageBox.Show("There was an error writing data to the Wallet Transactions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Writing Wallet Transactions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					End If

				End If
			Next
		End If

	End Sub

	Private Shared Function WriteSingleWalletTransactionToDB(ByVal Trans As XmlNode, ByVal Header As String, ByVal CharID As Integer, ByVal CharName As String, ByVal WalletID As Integer) As Boolean

		' Create key
		Dim TransDate As String = Date.ParseExact(Trans.Attributes.GetNamedItem("transactionDateTime").Value, IndustryTimeFormat, culture).ToString("yyyyMMddHHmmss")
		Dim TransRef As String = CLng(Trans.Attributes.GetNamedItem("transactionID").Value).ToString("D20")
		Dim TransCharID As String = CharID.ToString("D20")
		Dim TransKey As String = TransDate & TransRef & TransCharID
        Dim TypeGroup, TypeCategory, TypeMarketGroup As Integer

        ' Get item ID
        If EveHQ.Core.HQ.itemData.ContainsKey(Trans.Attributes.GetNamedItem("typeID").Value) = True Then
            Dim TypeData As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(Trans.Attributes.GetNamedItem("typeID").Value)
            TypeGroup = TypeData.Group
            TypeCategory = TypeData.Category
            TypeMarketGroup = TypeData.MarketGroup
        Else
            TypeGroup = 0
            TypeCategory = 0
            TypeMarketGroup = 0
        End If

        Dim strSQL As New StringBuilder
        strSQL.Append(Header)
        strSQL.Append("(")
        ' Start of record
        strSQL.Append("'" & Trans.Attributes.GetNamedItem("transactionDateTime").Value & "',")
        strSQL.Append(Trans.Attributes.GetNamedItem("transactionID").Value & ",")
        strSQL.Append("'" & TransKey & "',")
        strSQL.Append(Trans.Attributes.GetNamedItem("quantity").Value & ",")
        strSQL.Append("'" & Trans.Attributes.GetNamedItem("typeName").Value.Replace("'", "''") & "',")
        strSQL.Append(Trans.Attributes.GetNamedItem("typeID").Value & ",")
        strSQL.Append(TypeGroup & ",")
        strSQL.Append(TypeCategory & ",")
        strSQL.Append(TypeMarketGroup & ",")
        strSQL.Append(Trans.Attributes.GetNamedItem("price").Value & ",")
        strSQL.Append(Trans.Attributes.GetNamedItem("clientID").Value & ",")
        strSQL.Append("'" & Trans.Attributes.GetNamedItem("clientName").Value.Replace("'", "''") & "',")
        strSQL.Append(Trans.Attributes.GetNamedItem("stationID").Value & ",")
        strSQL.Append("'" & Trans.Attributes.GetNamedItem("stationName").Value.Replace("'", "''") & "',")
        strSQL.Append("'" & Trans.Attributes.GetNamedItem("transactionType").Value.Replace("'", "''") & "',")
        strSQL.Append("'" & Trans.Attributes.GetNamedItem("transactionFor").Value.Replace("'", "''") & "',")
        If EveHQ.Core.HQ.Stations.ContainsKey(Trans.Attributes.GetNamedItem("stationID").Value) = True Then
            strSQL.Append(EveHQ.Core.HQ.Stations(Trans.Attributes.GetNamedItem("stationID").Value).systemID.ToString & ",")
            strSQL.Append(EveHQ.Core.HQ.Stations(Trans.Attributes.GetNamedItem("stationID").Value).constID.ToString & ",")
            strSQL.Append(EveHQ.Core.HQ.Stations(Trans.Attributes.GetNamedItem("stationID").Value).regionID.ToString & ",")
        Else
            strSQL.Append("0,0,0,")
        End If
        strSQL.Append(CharID.ToString & ",")
        strSQL.Append("'" & CharName.Replace("'", "''") & "',")
        strSQL.Append(WalletID.ToString & ",")
        strSQL.Append("'" & Now.ToString(IndustryTimeFormat, culture) & "'")
        ' End of record
        strSQL.Append(");")
        ' Store the record and return the result
        Return EveHQ.Core.DataFunctions.SetData(strSQL.ToString)

    End Function

	Private Shared Function CheckWalletJournalDBTable() As Boolean
		Dim CreateTable As Boolean = False
		Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
		If tables IsNot Nothing Then
			If tables.Contains("walletJournal") = False Then
				' The DB exists but the table doesn't so we'll create this
				CreateTable = True
			Else
				' We have the Db and table so we can return a good result
				Return True
			End If
		Else
			' Database doesn't exist?
			Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
			msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
			msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
			MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
			If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
				MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return False
			Else
				MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
				CreateTable = True
			End If
		End If

		' Create the database table 
		If CreateTable = True Then
			Dim strSQL As New StringBuilder
			strSQL.AppendLine("CREATE TABLE walletJournal")
			strSQL.AppendLine("(")
			strSQL.AppendLine("  transID        bigint IDENTITY(1,1),")	' Autonumber for this entry
			strSQL.AppendLine("  transDate      datetime,")
			strSQL.AppendLine("  transRef       bigint,") ' Eve Reference ID
			strSQL.AppendLine("  transKey       nvarchar(100),") ' Unique Key based on date and ref
			strSQL.AppendLine("  refTypeID      int,")
			strSQL.AppendLine("  ownerName1     nvarchar(100),")
			strSQL.AppendLine("  ownerID1       bigint,")
			strSQL.AppendLine("  ownerName2     nvarchar(100),")
			strSQL.AppendLine("  ownerID2       bigint,")
			strSQL.AppendLine("  argName1       nvarchar(100),")
			strSQL.AppendLine("  argID1         bigint,")
			strSQL.AppendLine("  amount         float,")
			strSQL.AppendLine("  balance        float,")
			strSQL.AppendLine("  reason         nvarchar(255),")
			strSQL.AppendLine("  taxID          bigint,")
			strSQL.AppendLine("  taxAmount      float,")
			strSQL.AppendLine("  charID         bigint,") ' CharID or CorpID
			strSQL.AppendLine("  charName       nvarchar(100),") ' Char Name or Corp Name
			strSQL.AppendLine("  walletID       int,")
			strSQL.AppendLine("  importDate     datetime,")
			strSQL.AppendLine("")
			strSQL.AppendLine("  CONSTRAINT walletJournal_PK PRIMARY KEY (transID)")
			strSQL.AppendLine(")")
			If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
				Return True
			Else
				MessageBox.Show("There was an error creating the Wallet Journal database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return False
			End If
		End If

    End Function

    Public Shared Function ParseWalletJournalXML(ByVal JXML As XmlDocument, ByRef WalletJournals As SortedList(Of Long, WalletJournalItem)) As Boolean

        If JXML IsNot Nothing Then

            Dim NewJournals As Boolean = False

            ' Go through each journal entry and see if we should write it
            Dim TransList As XmlNodeList = JXML.SelectNodes("/eveapi/result/rowset/row")

            If TransList IsNot Nothing Then
                If TransList.Count > 0 Then
                    For Each Trans As XmlNode In TransList

                        ' Start a New WalletJournalItem
                        Dim WJI As New WalletJournalItem

                        ' Parse Journal
                        WJI.JournalDate = Date.ParseExact(Trans.Attributes.GetNamedItem("date").Value, IndustryTimeFormat, culture)
                        WJI.RefID = CLng(Trans.Attributes.GetNamedItem("refID").Value)
                        WJI.RefTypeID = CInt(Trans.Attributes.GetNamedItem("refTypeID").Value)
                        WJI.OwnerName1 = Trans.Attributes.GetNamedItem("ownerName1").Value
                        WJI.OwnerID1 = Trans.Attributes.GetNamedItem("ownerID1").Value
                        WJI.OwnerName2 = Trans.Attributes.GetNamedItem("ownerName2").Value
                        WJI.OwnerID2 = Trans.Attributes.GetNamedItem("ownerID2").Value
                        WJI.ArgName1 = Trans.Attributes.GetNamedItem("argName1").Value
                        WJI.ArgID1 = Trans.Attributes.GetNamedItem("argID1").Value
                        WJI.Amount = Double.Parse(Trans.Attributes.GetNamedItem("amount").Value, culture)
                        WJI.Balance = Double.Parse(Trans.Attributes.GetNamedItem("balance").Value, culture)
                        WJI.Reason = Trans.Attributes.GetNamedItem("reason").Value
                        If Trans.Attributes.GetNamedItem("taxAmount") IsNot Nothing Then
                            If Trans.Attributes.GetNamedItem("taxAmount").Value <> "" Then
                                WJI.TaxReceiverID = Trans.Attributes.GetNamedItem("taxReceiverID").Value
                                WJI.TaxAmount = Double.Parse(Trans.Attributes.GetNamedItem("taxAmount").Value, culture)
                            Else
                                WJI.TaxReceiverID = "0"
                                WJI.TaxAmount = 0
                            End If
                        Else
                            WJI.TaxReceiverID = "0"
                            WJI.TaxAmount = 0
                        End If

                        If WalletJournals.ContainsKey(WJI.RefID) = False Then
                            WalletJournals.Add(WJI.RefID, WJI)
                            NewJournals = True
                        End If

                    Next
                    If NewJournals = True Then
                        Return False ' WalletExhausted? Possibly not
                    Else
                        Return True ' Wallet has no new entries
                    End If
                Else
                    Return True ' Wallet is exhausted
                End If

            Else
                Return True ' Wallet is exhausted 
            End If

            Return False ' WalletExhausted? Possibly not
        Else
            Return True ' Wallet is exhausted
        End If
    End Function

    Public Shared Function ParseWalletJournalExportXML(ByVal JXML As XmlDocument, ByRef WalletJournals As SortedList(Of Long, WalletJournalItem)) As Boolean

        If JXML IsNot Nothing Then

            Dim NewJournals As Boolean = False

            ' Go through each journal entry and see if we should write it
            Dim TransList As XmlNodeList = JXML.SelectNodes("/EveHQWalletJournalExport/row")

            If TransList IsNot Nothing Then
                If TransList.Count > 0 Then
                    For Each Trans As XmlNode In TransList

                        ' Start a New WalletJournalItem
                        Dim WJI As New WalletJournalItem

                        ' Parse Journal
                        WJI.JournalDate = Date.ParseExact(Trans.Attributes.GetNamedItem("transDate").Value, IndustryTimeFormat, culture)
                        WJI.RefID = CLng(Trans.Attributes.GetNamedItem("transRef").Value)
                        WJI.RefTypeID = CInt(Trans.Attributes.GetNamedItem("refTypeID").Value)
                        WJI.OwnerName1 = Trans.Attributes.GetNamedItem("ownerName1").Value
                        WJI.OwnerID1 = Trans.Attributes.GetNamedItem("ownerID1").Value
                        WJI.OwnerName2 = Trans.Attributes.GetNamedItem("ownerName2").Value
                        WJI.OwnerID2 = Trans.Attributes.GetNamedItem("ownerID2").Value
                        WJI.ArgName1 = Trans.Attributes.GetNamedItem("argName1").Value
                        WJI.ArgID1 = Trans.Attributes.GetNamedItem("argID1").Value
                        WJI.Amount = Double.Parse(Trans.Attributes.GetNamedItem("amount").Value, culture)
                        WJI.Balance = Double.Parse(Trans.Attributes.GetNamedItem("balance").Value, culture)
                        WJI.Reason = Trans.Attributes.GetNamedItem("reason").Value
                        If Trans.Attributes.GetNamedItem("taxAmount") IsNot Nothing Then
                            If Trans.Attributes.GetNamedItem("taxAmount").Value <> "" Then
                                WJI.TaxReceiverID = Trans.Attributes.GetNamedItem("taxID").Value
                                WJI.TaxAmount = Double.Parse(Trans.Attributes.GetNamedItem("taxAmount").Value, culture)
                            Else
                                WJI.TaxReceiverID = "0"
                                WJI.TaxAmount = 0
                            End If
                        Else
                            WJI.TaxReceiverID = "0"
                            WJI.TaxAmount = 0
                        End If

                        If WalletJournals.ContainsKey(WJI.RefID) = False Then
                            WalletJournals.Add(WJI.RefID, WJI)
                            NewJournals = True
                        End If

                    Next
                    If NewJournals = True Then
                        Return False ' WalletExhausted? Possibly not
                    Else
                        Return True ' Wallet has no new entries
                    End If
                Else
                    Return True ' Wallet is exhausted
                End If

            Else
                Return True ' Wallet is exhausted 
            End If

            Return False ' WalletExhausted? Possibly not
        Else
            Return True ' Wallet is exhausted
        End If
    End Function

	Public Shared Sub WriteWalletJournalToDB(ByVal WalletJournals As SortedList(Of Long, WalletJournalItem), ByVal CharID As Integer, ByVal CharName As String, ByVal WalletID As Integer, ByVal LastTrans As Long)

		' Setup the default header
		Dim strInsert As String = "INSERT INTO walletJournal (transDate, transRef, transKey, refTypeID, ownerName1, ownerID1, ownerName2, ownerID2, argName1, argID1, amount, balance, reason, taxID, taxAmount, charID, charName, walletID, importDate) VALUES "

		If WalletJournals.Count > 0 Then

			For Each WalletJournal As WalletJournalItem In WalletJournals.Values

				If WalletJournal.RefID > LastTrans Then

					If WriteSingleWalletJournalToDB(WalletJournal, strInsert, CharID, CharName, WalletID) = False Then
						MessageBox.Show("There was an error writing data to the Wallet Journal database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Writing Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					End If

				End If

			Next

		End If
	End Sub

	Public Shared Function WriteSingleWalletJournalToDB(ByVal Trans As WalletJournalItem, ByVal Header As String, ByVal CharID As Integer, ByVal CharName As String, ByVal WalletID As Integer) As Boolean

		Dim IsTax As Boolean = False

		' Create key
		Dim TransDate As String = Trans.JournalDate.ToString("yyyyMMddHHmmss")
		Dim TransRef As String = Trans.RefID.ToString("D20")
		Dim TransTypeID As String = Trans.RefTypeID.ToString("D4")
		Dim TransCharID As String = CharID.ToString("D20")
		Dim TransKey As String = TransDate & TransRef & TransTypeID & TransCharID

		' Get amounts
		Dim amount As Double = Trans.Amount
		Dim tax As Double = Trans.TaxAmount
		amount += tax

		' Write the details
		Dim strSQL As New StringBuilder
		strSQL.Append(Header)
		strSQL.Append("(")
		' Start of record
        strSQL.Append("'" & Trans.JournalDate.ToString(IndustryTimeFormat, culture) & "',")
		strSQL.Append(Trans.RefID.ToString & ",")
		strSQL.Append("'" & TransKey & "',")
		strSQL.Append(Trans.RefTypeID.ToString & ",")
		strSQL.Append("'" & Trans.OwnerName1.Replace("'", "''") & "',")
		strSQL.Append(Trans.OwnerID1 & ",")
		strSQL.Append("'" & Trans.OwnerName2.Replace("'", "''") & "',")
		strSQL.Append(Trans.OwnerID2 & ",")
		strSQL.Append("'" & Trans.ArgName1.Replace("'", "''") & "',")
		strSQL.Append(Trans.ArgID1 & ",")
		strSQL.Append(amount.ToString(culture) & ",")
		strSQL.Append(Trans.Balance.ToString(culture) & ",")
		strSQL.Append("'" & Trans.Reason.Replace("'", "''") & "',")

		strSQL.Append(Trans.TaxReceiverID & ",")
		strSQL.Append(Trans.TaxAmount.ToString(culture) & ",")

		strSQL.Append(CharID.ToString & ",")
		strSQL.Append("'" & CharName.Replace("'", "''") & "',")
		strSQL.Append(WalletID.ToString & ",")
        strSQL.Append("'" & Now.ToString(IndustryTimeFormat, culture) & "'")
		' End of record
		strSQL.Append(");")

		' Check for tax record and write
		If tax <> 0 Then
			If WriteTaxJournalToDB(Trans, Header, CharID, CharName, WalletID) = False Then
				MessageBox.Show("Error writing the tax journal!")
			End If
		End If

		' Store the record
		Return EveHQ.Core.DataFunctions.SetData(strSQL.ToString)

	End Function

	Private Shared Function WriteTaxJournalToDB(ByVal Trans As WalletJournalItem, ByVal Header As String, ByVal CharID As Integer, ByVal CharName As String, ByVal WalletID As Integer) As Boolean

		' Switch Transaction Ref ID
		Dim RefID As Integer = Trans.RefTypeID
		Select Case RefID
			Case 85
				RefID = 92
			Case 33
				RefID = 93
			Case 34
				RefID = 94
		End Select

		' Create key
		Dim TransDate As String = Trans.JournalDate.ToString("yyyyMMddHHmmss")
		Dim TransRef As String = Trans.RefID.ToString("D20")
        Dim TransTypeID As String = RefID.ToString("D4")
		Dim TransCharID As String = CharID.ToString("D20")
		Dim TransKey As String = TransDate & TransRef & TransTypeID & TransCharID

		' Get amounts
		Dim amount As Double = -Trans.TaxAmount

		' Write the details
		Dim strSQL As New StringBuilder
		strSQL.Append(Header)
		strSQL.Append("(")
		' Start of record
        strSQL.Append("'" & Trans.JournalDate.ToString(IndustryTimeFormat, culture) & "',")
		strSQL.Append(Trans.RefID.ToString & ",")
		strSQL.Append("'" & TransKey & "',")
		strSQL.Append(RefID.ToString & ",")
		strSQL.Append("'" & Trans.OwnerName1.Replace("'", "''") & "',")
		strSQL.Append(Trans.OwnerID1 & ",")
		strSQL.Append("'" & Trans.OwnerName2.Replace("'", "''") & "',")
		strSQL.Append(Trans.OwnerID2 & ",")
		strSQL.Append("'" & Trans.ArgName1.Replace("'", "''") & "',")
		strSQL.Append(Trans.ArgID1 & ",")
		strSQL.Append(amount.ToString(culture) & ",")
		strSQL.Append(Trans.Balance.ToString(culture) & ",")
		strSQL.Append("'" & Trans.Reason.Replace("'", "''") & "',")

		strSQL.Append(Trans.TaxReceiverID & ",")
		strSQL.Append(Trans.TaxAmount.ToString(culture) & ",")

		strSQL.Append(CharID.ToString & ",")
		strSQL.Append("'" & CharName.Replace("'", "''") & "',")
		strSQL.Append(WalletID.ToString & ",")
        strSQL.Append("'" & Now.ToString(IndustryTimeFormat, culture) & "'")
		' End of record
		strSQL.Append(");")

		' Store the record
		Return EveHQ.Core.DataFunctions.SetData(strSQL.ToString)

	End Function

    Public Shared Function GetLastWalletID(ByVal WalletType As WalletTypes, ByVal CharID As Integer, ByVal WalletID As Integer) As Long
        Dim strSQL As String = ""
        Select Case WalletType
            Case WalletTypes.Journal
                strSQL = "SELECT TOP (1) transRef FROM walletJournal WHERE charID=" & CharID & " AND walletID=" & WalletID & " ORDER BY transRef DESC;"
            Case WalletTypes.Transactions
                strSQL = "SELECT TOP (1) transRef FROM walletTransactions WHERE charID=" & CharID & " AND walletID=" & WalletID & " ORDER BY transRef DESC;"
        End Select
        Dim walletData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        If walletData IsNot Nothing Then
            If walletData.Tables(0).Rows.Count > 0 Then
                ' Return the actual value
                Return CLng(walletData.Tables(0).Rows(0).Item(0))
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function

    Public Shared Function WriteInstallerIDsToDB(ByVal JobXML As XmlDocument) As Boolean

        Dim IDList As New List(Of String)
        Dim JobList As XmlNodeList = JobXML.SelectNodes("/eveapi/result/rowset/row")

        ' Get the installerIDs from the JobXML
        For Each Job As XmlNode In JobList
            If IDList.Contains(Job.Attributes.GetNamedItem("installerID").Value) = False Then
                IDList.Add(Job.Attributes.GetNamedItem("installerID").Value)
            End If
        Next

        ' Write the IDs to the database
        Call EveHQ.Core.DataFunctions.WriteEveIDsToDatabase(IDList)

    End Function

    Public Shared Function WriteInventionResultsToDB(ByVal JobXML As XmlDocument) As Boolean

        ' Parse the list of jobs
        Dim InventionList As SortedList(Of Long, InventionJob) = InventionJob.ParseInventionJobsFromAPI(JobXML)

        ' Prepare a list of job IDs that could already be in the DB
        Dim DBList As New List(Of Long)
        Dim IDList As New StringBuilder
        For Each ID As Long In InventionList.Keys
            IDList.Append("," & ID.ToString)
        Next
        If IDList.Length > 1 Then
            IDList.Remove(0, 1)
            ' Get the list from the DB
            Dim strSQL As String = "SELECT * FROM inventionResults WHERE jobID IN (" & IDList.ToString & ");"
            Dim IDData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
            If IDData IsNot Nothing Then
                If IDData.Tables(0).Rows.Count > 0 Then
                    For Each IDRow As DataRow In IDData.Tables(0).Rows
                        DBList.Add(CLng(IDRow.Item("jobID")))
                    Next
                End If
            End If
        End If

        ' Write new jobs to the database
        Dim strIDInsert As String = "INSERT INTO inventionResults (jobID, resultDate, BPID, typeID, installerID, result) VALUES ("
        For Each Job As InventionJob In InventionList.Values
            If DBList.Contains(Job.JobID) = False Then
                Dim uSQL As New StringBuilder
                uSQL.Append(strIDInsert)
                uSQL.Append(Job.JobID & ", ")
                uSQL.Append("'" & Job.ResultDate.ToString(IndustryTimeFormat, culture) & "', ")
                uSQL.Append(Job.BPID & ", ")
                uSQL.Append(Job.TypeID & ", ")
                uSQL.Append(Job.InstallerID & ", ")
                uSQL.Append(Job.result & ");")
                If EveHQ.Core.DataFunctions.SetData(uSQL.ToString) = False Then
                    'MessageBox.Show("There was an error writing data to the Invention Results database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve IDs", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)  
                End If
            End If
        Next

    End Function

End Class

Public Enum WalletTypes As Integer
    Journal = 0
    Transactions = 1
End Enum
