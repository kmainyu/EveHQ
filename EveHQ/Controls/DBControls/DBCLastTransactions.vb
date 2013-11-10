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
Imports DevComponents.AdvTree
Imports EveHQ.EveAPI
Imports EveHQ.Core
Imports DevComponents.DotNetBar
Imports System.Globalization
Imports System.Xml

Namespace Controls.DBControls

    Public Class DBCLastTransactions

        ReadOnly _styleRed As New ElementStyle
        ReadOnly _styleRedRight As New ElementStyle
        ReadOnly _styleGreen As New ElementStyle
        ReadOnly _styleGreenRight As New ElementStyle
        ReadOnly _culture As CultureInfo = New CultureInfo("en-GB")

        Public Sub New()

            ' This call is required by the Windows Form Designer.

            InitializeComponent()

            ControlConfigForm = "EveHQ.Controls.DBConfigs.DBCLasttransactionConfig"
            ControlConfigInfo = "Last transaction description"

            'populate Pilot ComboBox
            cboPilotList.BeginUpdate()
            cboPilotList.Items.Clear()
            For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
                If pilot.Active = True Then
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
                Return "Last Transactions"
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
                Dim numTransactionsDisplay As Integer = nudEntries.Value ' how much transactions to display in listview/
                Dim transactionsXML As XmlDocument
                Dim cPilot As EveHQPilot = HQ.Settings.Pilots(cboPilotList.SelectedItem.ToString)
                Dim cAccount As EveHQAccount = HQ.Settings.Accounts(cPilot.Account)
                Dim cCharID As String = cPilot.ID
                Const accountKey As Integer = 1000
                Const beforeRefID As String = ""

                Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
                transactionsXML = apiReq.GetAPIXML(APITypes.WalletTransChar, cAccount.ToAPIAccount, cCharID, accountKey, beforeRefID, APIReturnMethods.ReturnStandard)

                'Parse the XML document
                If transactionsXML IsNot Nothing Then
                    ' Get transactions
                    Dim transactionList As XmlNodeList

                    transactionList = transactionsXML.SelectNodes("/eveapi/result/rowset/row")
                    Dim sortedTransactions As New List(Of XmlNode)
                    For Each transaction As XmlNode In transactionList
                        sortedTransactions.Add(transaction)
                    Next
                    sortedTransactions.Reverse()

                    adtLastTransactions.BeginUpdate()
                    adtLastTransactions.Nodes.Clear()
                    For currentTransactionCounter As Integer = 0 To Math.Min(numTransactionsDisplay - 1, sortedTransactions.Count - 1)
                        Dim newTransaction As New Node
                        Dim transaction As XmlNode = sortedTransactions(currentTransactionCounter)
                        If transaction IsNot Nothing Then
                            newTransaction.Text = transaction.Attributes.GetNamedItem("transactionDateTime").Value
                            newTransaction.Name = transaction.Attributes.GetNamedItem("typeName").Value & currentTransactionCounter.ToString
                            newTransaction.Cells.Add(New Cell(transaction.Attributes.GetNamedItem("typeName").Value))
                            newTransaction.Cells.Add(New Cell(CLng(transaction.Attributes.GetNamedItem("quantity").Value).ToString("N0")))
                            If transaction.Attributes.GetNamedItem("transactionType").Value = "sell" Then
                                newTransaction.Style = _styleGreen
                                newTransaction.Cells.Add(New Cell(Double.Parse(transaction.Attributes.GetNamedItem("price").Value, _culture).ToString("N2"), _styleGreenRight))
                            Else
                                newTransaction.Style = _styleRed
                                newTransaction.Cells.Add(New Cell(Double.Parse(transaction.Attributes.GetNamedItem("price").Value, _culture).ToString("N2"), _styleRedRight))
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

        Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefresh.Click
            Call UpdateTransactions()
        End Sub
    End Class
End NameSpace