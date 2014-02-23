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

Imports System.Globalization
Imports DevComponents.AdvTree
Imports EveHQ.EveAPI
Imports EveHQ.Core
Imports DevComponents.DotNetBar
Imports System.Xml
Imports EveHQ.Common.Extensions

Namespace Controls.DBControls

    Public Class DBCLastJournals
        Private Const IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        ReadOnly _culture As CultureInfo = New CultureInfo("en-GB")
        ReadOnly _refTypes As New SortedList(Of String, String)
        ReadOnly _styleRed As New ElementStyle
        ReadOnly _styleRedRight As New ElementStyle
        ReadOnly _styleGreen As New ElementStyle
        ReadOnly _styleGreenRight As New ElementStyle

        Public Sub New()

            ' This call is required by the Windows Form Designer.

            InitializeComponent()

            ControlConfigForm = "EveHQ.Controls.DBConfigs.DBCLastJournalsConfig"
            ControlConfigInfo = "<Not yet configured>"

            ' Load RefTypes
            Call LoadRefTypes()

            'Populate Pilot ComboBox
            cboPilotList.BeginUpdate()
            cboPilotList.Items.Clear()
            For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
                If pilot.Active = True And pilot.Account <> "" Then
                    cboPilotList.Items.Add(pilot.Name)
                End If
            Next
            cboPilotList.EndUpdate()

            ' Create the styles
            _styleRed = adtLastTransactions.Styles("ElementStyle1").Copy
            _styleRed.TextColor = Color.Red
            _styleRedRight = adtLastTransactions.Styles("ElementStyle1").Copy
            _styleRedRight.TextColor = Color.Red
            _styleRedRight.TextAlignment = eStyleTextAlignment.Far
            _styleGreen = adtLastTransactions.Styles("ElementStyle1").Copy
            _styleGreen.TextColor = Color.DarkGreen
            _styleGreenRight = adtLastTransactions.Styles("ElementStyle1").Copy
            _styleGreenRight.TextColor = Color.DarkGreen
            _styleGreenRight.TextAlignment = eStyleTextAlignment.Far

        End Sub

        Public Overrides ReadOnly Property ControlName() As String
            Get
                Return "Last Journals"
            End Get
        End Property

        Dim _dbcDefaultPilotName As String = ""
        Public Property DBCDefaultPilotName() As String
            Get
                Return _dbcDefaultPilotName
            End Get
            Set(ByVal value As String)
                _dbcDefaultPilotName = value
                cboPilotList.SelectedItem = value
                If ReadConfig = False Then
                    SetConfig("DBCDefaultPilotName", value)
                    SetConfig("ControlConfigInfo", "Default Pilot: " & DBCDefaultPilotName.ToString & ", Transactions: " & DBCDefaultTransactionsCount.ToString)
                End If
            End Set
        End Property

        Dim _dbcDefaultTransactionsCount As Integer = 10
        Public Property DBCDefaultTransactionsCount() As Integer
            Get
                Return _dbcDefaultTransactionsCount
            End Get
            Set(ByVal value As Integer)
                _dbcDefaultTransactionsCount = value
                If ReadConfig = False Then
                    SetConfig("DBCDefaultTransactionsCount", value)
                    SetConfig("ControlConfigInfo", "Default Pilot: " & DBCDefaultPilotName.ToString & ", Transactions: " & DBCDefaultTransactionsCount.ToString)
                End If
                ' This will update the transactions
                nudEntries.Value = value
            End Set
        End Property

        Private Sub UpdateTransactions()
            If cboPilotList.SelectedItem IsNot Nothing Then
                'Get transactions XML
                Dim numTransactionsDisplay As Integer = nudEntries.Value ' how much transactions to display in listview

                Dim cPilot As EveHQPilot = HQ.Settings.Pilots(cboPilotList.SelectedItem.ToString)
                Dim cAccount As EveHQAccount = HQ.Settings.Accounts(cPilot.Account)
                Dim cCharID As String = cPilot.ID
                Const AccountKey As Integer = 1000
                Dim transA As Double
                Dim transB As Double

                Dim journalData = HQ.ApiProvider.Character.WalletJournal(cAccount.UserID, cAccount.APIKey, CInt(cCharID), 1000)
                'Parse the XML document
                If journalData.IsSuccess Then
                    ' Get transactions
                    Dim transactionList = journalData.ResultData

                    
                    adtLastTransactions.BeginUpdate()
                    adtLastTransactions.Nodes.Clear()
                    Dim maxCount = Math.Min(numTransactionsDisplay - 1, transactionList.Count - 1)
                    For currentTransactionCounter As Integer = 0 To maxCount
                        Dim newTransaction As New Node
                        Dim transaction = transactionList(currentTransactionCounter)
                        If transaction IsNot Nothing Then
                            Dim transDate As Date = transaction.Date.DateTime
                            newTransaction.Text = transDate.ToString
                            newTransaction.Cells.Add(New Cell(_refTypes(transaction.ReferenceType.ToInvariantString)))
                            transA = transaction.Amount
                            transB = transaction.Balance
                            If transA >= 0 Then
                                newTransaction.Style = _styleGreen
                                newTransaction.Cells.Add(New Cell(transA.ToString("N2"), _styleGreenRight))
                                newTransaction.Cells.Add(New Cell(transB.ToString("N2"), _styleGreenRight))
                            Else
                                newTransaction.Style = _styleRed
                                newTransaction.Cells.Add(New Cell(transA.ToString("N2"), _styleRedRight))
                                newTransaction.Cells.Add(New Cell(transB.ToString("N2"), _styleRedRight))
                            End If
                            adtLastTransactions.Nodes.Add(newTransaction)
                        End If
                    Next
                    adtLastTransactions.EndUpdate()
                End If
            End If
        End Sub

        Private Sub cboPilotList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilotList.SelectedIndexChanged
            UpdateTransactions()
        End Sub

        Private Sub nudEntries_LostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles nudEntries.LostFocus
            DBCDefaultTransactionsCount = nudEntries.Value
        End Sub

        Private Sub nudEntries_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudEntries.ValueChanged
            If cboPilotList.SelectedItem IsNot Nothing Then
                DBCDefaultTransactionsCount = nudEntries.Value
                Call UpdateTransactions()
            End If
        End Sub

        Public Function LoadRefTypes() As Boolean
            Try
                ' Dimension variables
                Dim refData = HQ.ApiProvider.Eve.RefTypes()
                If refData.EveErrorCode = 0 Then

                    If refData.IsSuccess Then
                        _refTypes.Clear()
                        For Each refNode In refData.ResultData
                            _refTypes.Add(refNode.Id.ToInvariantString(), refNode.Name)
                        Next
                    End If
                Else
                    ' Get error code
                    Dim errCode As String = refData.EveErrorCode.ToInvariantString()
                    Dim errMsg As String = refData.EveErrorText
                    MessageBox.Show("The RefTypes API returned error " & errCode & ": " & errMsg, "RefTypes Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
                Return True
            Catch e As Exception
                MessageBox.Show("There was an error loading the RefTypes API. The error was: " & e.Message, "Last Journal Widget Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Trace.TraceError(e.FormatException())
                Return False
            End Try
        End Function

        Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefresh.Click
            Call UpdateTransactions()
        End Sub
    End Class
End NameSpace