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

Imports System.Data.SQLite
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports EveHQ.EveAPI
Imports EveHQ.Core
Imports EveHQ.EveData
Imports EveHQ.Common.Extensions

Namespace Classes

    Public Class PrismDataFunctions
        Private Const PrismTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        Private Shared ReadOnly Culture As CultureInfo = New CultureInfo("en-GB")

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
            Dim createTable As Boolean
            Dim tables As List(Of String) = CustomDataFunctions.GetDatabaseTables
            If tables IsNot Nothing Then
                If tables.Contains("inventionResults") = False Then
                    ' The DB exists but the table doesn't so we'll create this
                    createTable = True
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
                If CustomDataFunctions.CreateCustomDB = False Then
                    MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                Else
                    MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    createTable = True
                End If
            End If

            ' Create the database table 
            If createTable = True Then
                Dim strSQL As New StringBuilder
                strSQL.AppendLine("CREATE TABLE inventionResults")
                strSQL.AppendLine("(")
                strSQL.AppendLine("  resultID       INTEGER PRIMARY KEY,") ' Autonumber for this entry
                strSQL.AppendLine("  jobID          bigint,")
                strSQL.AppendLine("  resultDate     datetime,")
                strSQL.AppendLine("  BPID           int,")
                strSQL.AppendLine("  typeID         int,")
                strSQL.AppendLine("  typeName       nvarchar(100),")
                strSQL.AppendLine("  installerID    bigint,")
                strSQL.AppendLine("  installerName  nvarchar(100),")
                strSQL.AppendLine("  result         int )") ' 0=failed, 1=success
                If CustomDataFunctions.SetCustomData(strSQL.ToString) <> -2 Then
                    Return True
                Else
                    MessageBox.Show("There was an error creating the Invention Results database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            End If

        End Function

        Private Shared Function CheckAssetItemNameDBTable() As Boolean
            Dim createTable As Boolean
            Dim tables As List(Of String) = CustomDataFunctions.GetDatabaseTables
            If tables IsNot Nothing Then
                If tables.Contains("assetItemNames") = False Then
                    ' The DB exists but the table doesn't so we'll create this
                    createTable = True
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
                If CustomDataFunctions.CreateCustomDB = False Then
                    MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                Else
                    MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    createTable = True
                End If
            End If

            ' Create the database table 
            If createTable = True Then
                Dim strSQL As New StringBuilder
                strSQL.AppendLine("CREATE TABLE assetItemNames")
                strSQL.AppendLine("(")
                strSQL.AppendLine("  itemID         bigint,")
                strSQL.AppendLine("  itemName       nvarchar(100),")
                strSQL.AppendLine("")
                strSQL.AppendLine("  CONSTRAINT assetItemNames_PK PRIMARY KEY (itemID)")
                strSQL.AppendLine(")")
                If CustomDataFunctions.SetCustomData(strSQL.ToString) <> -2 Then
                    Return True
                Else
                    MessageBox.Show("There was an error creating the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            End If

        End Function

        Private Shared Function CheckWalletTransDBTable() As Boolean
            Dim createTable As Boolean
            Dim tables As List(Of String) = CustomDataFunctions.GetDatabaseTables
            If tables IsNot Nothing Then
                If tables.Contains("walletTransactions") = False Then
                    ' The DB exists but the table doesn't so we'll create this
                    createTable = True
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
                If CustomDataFunctions.CreateCustomDB = False Then
                    MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                Else
                    MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    createTable = True
                End If
            End If

            ' Create the database table 
            If createTable = True Then
                Dim strSQL As New StringBuilder
                strSQL.AppendLine("CREATE TABLE walletTransactions")
                strSQL.AppendLine("(")
                strSQL.AppendLine("  transID        INTEGER PRIMARY KEY,") ' Autonumber for this entry
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
                strSQL.AppendLine("  transType      nvarchar(5),")  ' 1 = Buy, 2=Sell
                strSQL.AppendLine("  transFor       nvarchar(15),")  ' 1 = Personal, 2 = Corporation
                strSQL.AppendLine("  systemID       bigint,")
                strSQL.AppendLine("  constID        bigint,")
                strSQL.AppendLine("  regionID       bigint,")
                strSQL.AppendLine("  charID         bigint,")
                strSQL.AppendLine("  charName       nvarchar(100),")
                strSQL.AppendLine("  walletID       int,")
                strSQL.AppendLine("  importDate     datetime)")
                If CustomDataFunctions.SetCustomData(strSQL.ToString) <> -2 Then
                    Return True
                Else
                    MessageBox.Show("There was an error creating the Wallet Transactions database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            End If

        End Function

        Public Shared Function WriteWalletTransactionsToDB(ByVal walletTransactions As IEnumerable(Of WalletTransaction), ByVal isCorp As Boolean, ByVal charID As Integer, ByVal charName As String, ByVal walletID As Integer) As Integer

            If walletTransactions IsNot Nothing Then

                ' Get the last referenceID for the wallet
                Dim lastTrans As Long = GetLastWalletID(WalletTypes.Transactions, charID, walletID)

                ' Create the database commands for updating
                Dim recordsAffected As Integer
                Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)

                Try
                    conn.Open()
                    Using sqlTrans As SQLiteTransaction = conn.BeginTransaction()
                        Using sqlCmd As SQLiteCommand = conn.CreateCommand

                            ' Create the DB Command text
                            sqlCmd.CommandText = "INSERT INTO walletTransactions (transDate, transRef, transKey, quantity, typeName, typeID, groupID, categoryID, marketGroupID, price, clientID, clientName, stationID, stationName, transType, transFor, systemID, constID, regionID, charID, charName, walletID, importDate) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);"

                            ' Create the desired number of parameters
                            ' ReSharper disable once RedundantAssignment - incorrect warning by R#
                            For col As Integer = 1 To 23
                                sqlCmd.Parameters.Add(sqlCmd.CreateParameter)
                            Next

                            ' Run through the transactions and update where relevant
                            For Each trans As WalletTransaction In walletTransactions

                                Dim currentTrans As Long = trans.TransactionId
                                ' Only write if it's something we haven't seen before i.e. is above our last transaction
                                If currentTrans > lastTrans Then

                                    ' Create key
                                    Dim transDate As String = trans.TransactionDateTime.DateTime.ToInvariantString("yyyyMMddHHmmss")
                                    Dim transRef As String = trans.TransactionId.ToInvariantString("D20")
                                    Dim transCharID As String = charID.ToInvariantString("D20")
                                    Dim transKey As String = transDate & transRef & transCharID
                                    Dim typeGroup, typeCategory, typeMarketGroup As Integer

                                    ' Get item ID
                                    Dim typeData As EveType
                                    If StaticData.Types.TryGetValue(trans.TypeId, typeData) = True Then
                                        typeGroup = typeData.Group
                                        typeCategory = typeData.Category
                                        typeMarketGroup = typeData.MarketGroupId
                                    Else
                                        typeGroup = 0
                                        typeCategory = 0
                                        typeMarketGroup = 0
                                    End If

                                    ' Add the database values and execute the query
                                    Dim recDate As DateTime = trans.TransactionDateTime.DateTime
                                    sqlCmd.Parameters(0).Value = recDate.ToInvariantString(PrismTimeFormat)
                                    sqlCmd.Parameters(1).Value = trans.TransactionId.ToInvariantString()
                                    sqlCmd.Parameters(2).Value = transKey
                                    sqlCmd.Parameters(3).Value = trans.Quantity.ToInvariantString()
                                    sqlCmd.Parameters(4).Value = trans.TypeName
                                    sqlCmd.Parameters(5).Value = trans.TypeId.ToInvariantString()
                                    sqlCmd.Parameters(6).Value = typeGroup
                                    sqlCmd.Parameters(7).Value = typeCategory
                                    sqlCmd.Parameters(8).Value = typeMarketGroup
                                    sqlCmd.Parameters(9).Value = trans.Price.ToInvariantString(2)
                                    sqlCmd.Parameters(10).Value = trans.ClientId.ToInvariantString()
                                    sqlCmd.Parameters(11).Value = trans.ClientName
                                    sqlCmd.Parameters(12).Value = trans.StationId.ToInvariantString()
                                    sqlCmd.Parameters(13).Value = trans.StationName
                                    sqlCmd.Parameters(14).Value = trans.TransactionType
                                    sqlCmd.Parameters(15).Value = trans.TransactionFor
                                    Dim station As Station
                                    Dim system As SolarSystem
                                    If StaticData.Stations.TryGetValue(trans.StationId, station) = True Then
                                        sqlCmd.Parameters(16).Value = station.SystemId
                                        system = StaticData.SolarSystems(station.SystemId)
                                        sqlCmd.Parameters(17).Value = system.ConstellationId
                                        sqlCmd.Parameters(18).Value = system.RegionId
                                    Else
                                        sqlCmd.Parameters(16).Value = 0
                                        sqlCmd.Parameters(17).Value = 0
                                        sqlCmd.Parameters(18).Value = 0
                                    End If
                                    sqlCmd.Parameters(19).Value = charID
                                    sqlCmd.Parameters(20).Value = charName
                                    sqlCmd.Parameters(21).Value = walletID.ToString
                                    sqlCmd.Parameters(22).Value = Now.ToString(PrismTimeFormat, Culture)
                                    recordsAffected += sqlCmd.ExecuteNonQuery()

                                End If

                            Next

                            ' Commit to the database
                            sqlTrans.Commit()

                        End Using
                    End Using

                    ' Close the connection and return the number of rows affected
                    conn.Close()
                    Return recordsAffected

                Catch e As Exception
                    HQ.DataError = e.Message
                    HQ.WriteLogEvent("Database Error: " & e.Message)
                    HQ.WriteLogEvent("SQL: " & "")
                    Return -2
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try

            End If

        End Function

        Private Shared Function CheckWalletJournalDBTable() As Boolean
            Dim createTable As Boolean
            Dim tables As List(Of String) = CustomDataFunctions.GetDatabaseTables
            If tables IsNot Nothing Then
                If tables.Contains("walletJournal") = False Then
                    ' The DB exists but the table doesn't so we'll create this
                    createTable = True
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
                If CustomDataFunctions.CreateCustomDB = False Then
                    MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                Else
                    MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    createTable = True
                End If
            End If

            ' Create the database table 
            If createTable = True Then
                Dim strSQL As New StringBuilder
                strSQL.AppendLine("CREATE TABLE walletJournal")
                strSQL.AppendLine("(")
                strSQL.AppendLine("  transID        INTEGER PRIMARY KEY,") ' Autonumber for this entry
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
                strSQL.AppendLine("  importDate     datetime)")
                If CustomDataFunctions.SetCustomData(strSQL.ToString) <> -2 Then
                    Return True
                Else
                    MessageBox.Show("There was an error creating the Wallet Journal database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError, "Error Creating Database Table", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            End If

        End Function

        Public Shared Function ParseWalletJournal(ByVal journal As IEnumerable(Of WalletJournalEntry), ByRef walletJournals As SortedList(Of String, WalletJournalItem), ownerID As String) As Boolean

            If journal IsNot Nothing Then

                Dim newJournals As Boolean = False


                If journal.Any() Then
                    For Each trans As WalletJournalEntry In journal

                        ' Start a New WalletJournalItem
                        Dim wji As New WalletJournalItem

                        ' Parse Journal
                        wji.JournalDate = trans.Date.DateTime
                        wji.RefID = trans.RefId
                        wji.RefTypeID = trans.ReferenceType
                        wji.OwnerName1 = trans.FirstPartyName
                        wji.OwnerID1 = trans.FirstPartyId.ToInvariantString()
                        wji.OwnerName2 = trans.SecondPartyName
                        wji.OwnerID2 = trans.SecondPartyId.ToInvariantString()
                        wji.ArgName1 = trans.ArgumentName
                        wji.ArgID1 = trans.ArgumentId.ToInvariantString()
                        wji.Amount = trans.Amount
                        wji.Balance = trans.Balance
                        wji.Reason = trans.Reason
                        If trans.TaxAmount > 0 Then

                            wji.TaxReceiverID = trans.TaxReceiverId.ToInvariantString()
                            wji.TaxAmount = trans.TaxAmount

                        Else
                            wji.TaxReceiverID = "0"
                            wji.TaxAmount = 0
                        End If

                        ' Create a key
                        CreateWalletJournalKey(wji, CInt(ownerID))

                        If walletJournals.ContainsKey(wji.Key) = False Then
                            walletJournals.Add(wji.Key, wji)
                            newJournals = True
                        End If

                    Next
                    If newJournals = True Then
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
        End Function

        Public Shared Function ParseWalletJournalExportXML(ByVal jxml As XmlDocument, ByRef walletJournals As SortedList(Of String, WalletJournalItem), ownerID As String) As Boolean

            If jxml IsNot Nothing Then

                Dim newJournals As Boolean = False

                ' Go through each journal entry and see if we should write it
                Dim transList As XmlNodeList = jxml.SelectNodes("/EveHQWalletJournalExport/row")

                If transList IsNot Nothing Then
                    If transList.Count > 0 Then
                        For Each trans As XmlNode In transList

                            ' Start a New WalletJournalItem
                            Dim wji As New WalletJournalItem

                            ' Parse Journal
                            wji.JournalDate = Date.ParseExact(trans.Attributes.GetNamedItem("transDate").Value, PrismTimeFormat, Culture)
                            wji.RefID = CLng(trans.Attributes.GetNamedItem("transRef").Value)
                            wji.RefTypeID = CInt(trans.Attributes.GetNamedItem("refTypeID").Value)
                            wji.OwnerName1 = trans.Attributes.GetNamedItem("ownerName1").Value
                            wji.OwnerID1 = trans.Attributes.GetNamedItem("ownerID1").Value
                            wji.OwnerName2 = trans.Attributes.GetNamedItem("ownerName2").Value
                            wji.OwnerID2 = trans.Attributes.GetNamedItem("ownerID2").Value
                            wji.ArgName1 = trans.Attributes.GetNamedItem("argName1").Value
                            wji.ArgID1 = trans.Attributes.GetNamedItem("argID1").Value
                            wji.Amount = Double.Parse(trans.Attributes.GetNamedItem("amount").Value, Culture)
                            wji.Balance = Double.Parse(trans.Attributes.GetNamedItem("balance").Value, Culture)
                            wji.Reason = trans.Attributes.GetNamedItem("reason").Value
                            If trans.Attributes.GetNamedItem("taxAmount") IsNot Nothing Then
                                If trans.Attributes.GetNamedItem("taxAmount").Value <> "" Then
                                    wji.TaxReceiverID = trans.Attributes.GetNamedItem("taxID").Value
                                    wji.TaxAmount = Double.Parse(trans.Attributes.GetNamedItem("taxAmount").Value, Culture)
                                Else
                                    wji.TaxReceiverID = "0"
                                    wji.TaxAmount = 0
                                End If
                            Else
                                wji.TaxReceiverID = "0"
                                wji.TaxAmount = 0
                            End If

                            ' Create a key
                            CreateWalletJournalKey(wji, CInt(ownerID))

                            If walletJournals.ContainsKey(wji.Key) = False Then
                                walletJournals.Add(wji.Key, wji)
                                newJournals = True
                            End If

                        Next
                        If newJournals = True Then
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
            Else
                Return True ' Wallet is exhausted
            End If
        End Function

        Public Shared Function WriteWalletJournalsToDB(ByVal walletJournals As SortedList(Of String, WalletJournalItem), ByVal charID As Integer, ByVal charName As String, ByVal walletID As Integer, ByVal lastTrans As Long) As Integer

            ' Create the database commands for updating
            Dim recordsAffected As Integer
            Dim conn As New SQLiteConnection(HQ.EveHQDataConnectionString)

            Try
                conn.Open()
                Using sqlTrans As SQLiteTransaction = conn.BeginTransaction()
                    Using sqlCmd As SQLiteCommand = conn.CreateCommand

                        ' Create the DB Command text
                        sqlCmd.CommandText = "INSERT INTO walletJournal (transDate, transRef, transKey, refTypeID, ownerName1, ownerID1, ownerName2, ownerID2, argName1, argID1, amount, balance, reason, taxID, taxAmount, charID, charName, walletID, importDate) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);"

                        ' Create the desired number of parameters
                        ' ReSharper disable once RedundantAssignment - incorrect warning by R#
                        For col As Integer = 1 To 19
                            sqlCmd.Parameters.Add(sqlCmd.CreateParameter)
                        Next

                        ' Run through the wallet journal collection and update where relevant
                        For Each journalItem As WalletJournalItem In walletJournals.Values

                            If journalItem.RefID > lastTrans Then

                                ' Create key for the wallet journal
                                CreateWalletJournalKey(journalItem, charID)

                                ' Get amounts
                                Dim amount As Double = journalItem.Amount
                                Dim tax As Double = journalItem.TaxAmount
                                amount += tax

                                ' Add the database values and execute the query
                                sqlCmd.Parameters(0).Value = journalItem.JournalDate.ToString(PrismTimeFormat, Culture)
                                sqlCmd.Parameters(1).Value = journalItem.RefID
                                sqlCmd.Parameters(2).Value = journalItem.Key
                                sqlCmd.Parameters(3).Value = journalItem.RefTypeID
                                sqlCmd.Parameters(4).Value = journalItem.OwnerName1
                                sqlCmd.Parameters(5).Value = journalItem.OwnerID1
                                sqlCmd.Parameters(6).Value = journalItem.OwnerName2
                                sqlCmd.Parameters(7).Value = journalItem.OwnerID2
                                sqlCmd.Parameters(8).Value = journalItem.ArgName1
                                sqlCmd.Parameters(9).Value = journalItem.ArgID1
                                sqlCmd.Parameters(10).Value = amount
                                sqlCmd.Parameters(11).Value = journalItem.Balance
                                sqlCmd.Parameters(12).Value = journalItem.Reason
                                sqlCmd.Parameters(13).Value = journalItem.TaxReceiverID
                                sqlCmd.Parameters(14).Value = journalItem.TaxAmount
                                sqlCmd.Parameters(15).Value = charID
                                sqlCmd.Parameters(16).Value = charName
                                sqlCmd.Parameters(17).Value = walletID
                                sqlCmd.Parameters(18).Value = Now.ToString(PrismTimeFormat, Culture)
                                recordsAffected += sqlCmd.ExecuteNonQuery()

                                ' Check for tax record and write
                                If tax <> 0 Then
                                    WriteTaxJournalToDB(sqlCmd, recordsAffected, journalItem, charID, charName, walletID)
                                End If

                            End If

                        Next

                        ' Commit to the database
                        sqlTrans.Commit()

                    End Using
                End Using

                ' Close the connection and return the number of rows affected
                conn.Close()
                Return recordsAffected

            Catch e As Exception
                HQ.DataError = e.Message
                HQ.WriteLogEvent("Database Error: " & e.Message)
                HQ.WriteLogEvent("SQL: " & "")
                Return -2
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try

        End Function

        Private Shared Sub WriteTaxJournalToDB(ByRef sqlCmd As SQLiteCommand, ByRef recordsAffected As Integer, ByVal journalItem As WalletJournalItem, ByVal charID As Integer, ByVal charName As String, ByVal walletID As Integer)

            ' Switch Transaction Ref ID
            Dim refTypeID As Integer = journalItem.RefTypeID
            Select Case refTypeID
                Case 85
                    refTypeID = 92
                Case 33
                    refTypeID = 93
                Case 34
                    refTypeID = 94
            End Select

            ' Create key
            Dim transDate As String = journalItem.JournalDate.ToString("yyyyMMddHHmmss")
            Dim transRef As String = journalItem.RefID.ToString("D20")
            Dim transTypeID As String = refTypeID.ToString("D4")
            Dim transCharID As String = charID.ToString("D20")
            Dim transAmountFlag As String
            If -journalItem.Amount < 0 Then
                transAmountFlag = "0"
            Else
                transAmountFlag = "1"
            End If
            Dim transKey As String = transDate & transRef & transTypeID & transCharID & transAmountFlag

            ' Get amounts
            Dim amount As Double = -journalItem.TaxAmount

            ' Add the database values and execute the query
            sqlCmd.Parameters(0).Value = journalItem.JournalDate.ToString(PrismTimeFormat, Culture)
            sqlCmd.Parameters(1).Value = journalItem.RefID
            sqlCmd.Parameters(2).Value = transKey
            sqlCmd.Parameters(3).Value = refTypeID ' Note - revised RefTypeID, not the original
            sqlCmd.Parameters(4).Value = journalItem.OwnerName1
            sqlCmd.Parameters(5).Value = journalItem.OwnerID1
            sqlCmd.Parameters(6).Value = journalItem.OwnerName2
            sqlCmd.Parameters(7).Value = journalItem.OwnerID2
            sqlCmd.Parameters(8).Value = journalItem.ArgName1
            sqlCmd.Parameters(9).Value = journalItem.ArgID1
            sqlCmd.Parameters(10).Value = amount
            sqlCmd.Parameters(11).Value = journalItem.Balance
            sqlCmd.Parameters(12).Value = journalItem.Reason
            sqlCmd.Parameters(13).Value = journalItem.TaxReceiverID
            sqlCmd.Parameters(14).Value = journalItem.TaxAmount
            sqlCmd.Parameters(15).Value = charID
            sqlCmd.Parameters(16).Value = charName
            sqlCmd.Parameters(17).Value = walletID
            sqlCmd.Parameters(18).Value = Now.ToString(PrismTimeFormat, Culture)
            recordsAffected += sqlCmd.ExecuteNonQuery()

        End Sub

        Private Shared Sub CreateWalletJournalKey(ByRef trans As WalletJournalItem, ByVal charID As Integer)

            Dim transDate As String = trans.JournalDate.ToString("yyyyMMddHHmmss")
            Dim transRef As String = trans.RefID.ToString("D20")
            Dim transTypeID As String = trans.RefTypeID.ToString("D4")
            Dim transCharID As String = charID.ToString("D20")
            Dim transAmountFlag As String
            If trans.Amount < 0 Then
                transAmountFlag = "0"
            Else
                transAmountFlag = "1"
            End If
            trans.Key = transDate & transRef & transTypeID & transCharID & transAmountFlag

        End Sub

        Public Shared Function GetLastWalletID(ByVal walletType As WalletTypes, ByVal charID As Integer, ByVal walletID As Integer) As Long
            Dim strSQL As String = ""
            Select Case walletType
                Case WalletTypes.Journal
                    strSQL = "SELECT transRef FROM walletJournal WHERE charID=" & charID & " AND walletID=" & walletID & " ORDER BY transRef DESC LIMIT 1;"
                Case WalletTypes.Transactions
                    strSQL = "SELECT transRef FROM walletTransactions WHERE charID=" & charID & " AND walletID=" & walletID & " ORDER BY transRef DESC LIMIT 1;"
            End Select
            Dim walletData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
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

        Public Shared Function WriteInstallerIdsToDB(ByVal jobs As IEnumerable(Of EveAPI.IndustryJob)) As Boolean

            Dim idList As New HashSet(Of String)

            ' Get the installerIDs from the JobXML
            For Each job In jobs
                Dim installer = job.InstallerId.ToInvariantString()
                If idList.Contains(installer) = False Then
                    idList.Add(installer)
                End If
            Next

            ' Write the IDs to the database
            Call CustomDataFunctions.WriteEveIDsToDatabase(idList.ToList)

        End Function

        Public Shared Function WriteContractIdsToDB(ByVal contracts As IEnumerable(Of EveAPI.Contract)) As Boolean

            Dim idList As New List(Of Integer)


            ' Get the character IDs from the ContractXML
            For Each contract In contracts
                idList.Add(contract.AcceptorId)
                idList.Add(contract.AssigneeId)
                idList.Add(contract.IssuerId)
            Next

            ' Write the IDs to the database
            Call CustomDataFunctions.WriteEveIDsToDatabase(idList.Select(Function(id) id.ToInvariantString()).Distinct().ToList())

        End Function

        Public Shared Function WriteInventionResultsToDB(ByVal jobs As IEnumerable(Of EveAPI.IndustryJob)) As Boolean

            ' Parse the list of jobs
            Dim inventionList As Dictionary(Of Long, InventionAPIJob) = InventionAPIJob.ParseInventionJobsFromAPI(jobs)

            ' Prepare a list of job IDs that could already be in the DB
            Dim dbList As New List(Of Long)
            Dim idList As New StringBuilder
            For Each id As Long In inventionList.Keys
                idList.Append("," & id.ToString)
            Next
            If idList.Length > 1 Then
                idList.Remove(0, 1)
                ' Get the list from the DB
                Dim strSQL As String = "SELECT * FROM inventionResults WHERE jobID IN (" & idList.ToString & ");"
                Dim idData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
                If idData IsNot Nothing Then
                    If idData.Tables(0).Rows.Count > 0 Then
                        For Each idRow As DataRow In idData.Tables(0).Rows
                            dbList.Add(CLng(idRow.Item("jobID")))
                        Next
                    End If
                End If
            End If

            ' Write new jobs to the database
            Const StrIDInsert As String = "INSERT INTO inventionResults (jobID, resultDate, BPID, typeID, typeName, installerID, installerName, result) VALUES ("
            For Each job As InventionAPIJob In inventionList.Values
                If dbList.Contains(job.JobID) = False Then
                    Dim uSQL As New StringBuilder
                    uSQL.Append(StrIDInsert)
                    uSQL.Append(job.JobID & ", ")
                    uSQL.Append("'" & job.ResultDate.ToString(PrismTimeFormat, Culture) & "', ")
                    uSQL.Append(job.BlueprintID & ", ")
                    uSQL.Append(job.TypeID & ", ")
                    uSQL.Append("'" & job.TypeName.Replace("'", "''") & "', ")
                    uSQL.Append(job.InstallerID & ", ")
                    uSQL.Append("'" & job.InstallerName.Replace("'", "''") & "', ")
                    uSQL.Append(job.Result & ");")
                    If CustomDataFunctions.SetCustomData(uSQL.ToString) = -2 Then
                        'MessageBox.Show("There was an error writing data to the Invention Results database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve IDs", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)  
                    End If
                End If
            Next

        End Function

    End Class

    Public Enum WalletTypes As Integer
        Journal = 0
        Transactions = 1
    End Enum
End Namespace