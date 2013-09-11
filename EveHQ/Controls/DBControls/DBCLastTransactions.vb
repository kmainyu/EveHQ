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
Imports DevComponents.DotNetBar
Imports DevComponents.AdvTree

Public Class DBCLastTransactions

    Dim StyleRed As New ElementStyle
    Dim StyleRedRight As New ElementStyle
    Dim StyleGreen As New ElementStyle
	Dim StyleGreenRight As New ElementStyle
	Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
	Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    Public Sub New()

        ' This call is required by the Windows Form Designer.

        InitializeComponent()

        Me.ControlConfigForm = "EveHQ.DBCLasttransactionConfig"
        Me.ControlConfigInfo = "Last transaction description"

        'populate Pilot ComboBox
        cboPilotList.BeginUpdate()
        cboPilotList.Items.Clear()
        For Each pilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
            If pilot.Active = True Then
                cboPilotList.Items.Add(pilot.Name)
            End If
        Next
        cboPilotList.EndUpdate()

        ' Create the styles
        StyleRed = adtLastTransactions.Styles("ElementStyle1").Copy
        StyleRed.TextColor = Color.Red
        StyleRedRight = adtLastTransactions.Styles("ElementStyle1").Copy
        StyleRedRight.TextColor = Color.Red
        StyleRedRight.TextAlignment = eStyleTextAlignment.Far
        StyleGreen = adtLastTransactions.Styles("ElementStyle1").Copy
        StyleGreen.TextColor = Color.DarkGreen
        StyleGreenRight = adtLastTransactions.Styles("ElementStyle1").Copy
        StyleGreenRight.TextColor = Color.DarkGreen
        StyleGreenRight.TextAlignment = eStyleTextAlignment.Far

    End Sub

    Public Overrides ReadOnly Property ControlName() As String
        Get
            Return "Last Transactions"
        End Get
    End Property

    Dim cDBCDefaultPilotName As String = ""
    Public Property DBCDefaultPilotName() As String
        Get
            Return cDBCDefaultPilotName
        End Get
        Set(ByVal value As String)
            cDBCDefaultPilotName = value
            cboPilotList.SelectedItem = value
            If ReadConfig = False Then
                Me.SetConfig("DBCDefaultPilotName", value)
                Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DBCDefaultPilotName.ToString & ", Transactions: " & Me.DBCDefaultTransactionsCount.ToString)
            End If
        End Set
    End Property

    Dim cDBCDefaultTransactionsCount As Integer = 10
    Public Property DBCDefaultTransactionsCount() As Integer
        Get
            Return cDBCDefaultTransactionsCount
        End Get
        Set(ByVal value As Integer)
            cDBCDefaultTransactionsCount = value
            If ReadConfig = False Then
                Me.SetConfig("DBCDefaultTransactionsCount", value)
                Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DBCDefaultPilotName.ToString & ", Transactions: " & Me.DBCDefaultTransactionsCount.ToString)
            End If
            ' This will update the transactions
            nudEntries.Value = value
        End Set
    End Property

    Private Sub UpdateTransactions()
        If cboPilotList.SelectedItem IsNot Nothing Then
            'Get transactions XML
            Dim numTransactionsDisplay As Integer = CType(nudEntries.Value, Integer) ' how much transactions to display in listview/
            Dim transactionsXML As XmlDocument
            Dim cPilot As EveHQ.Core.EveHQPilot = EveHQ.Core.HQ.Settings.Pilots(cboPilotList.SelectedItem.ToString)
            Dim cAccount As EveHQ.Core.EveHQAccount = EveHQ.Core.HQ.Settings.Accounts(cPilot.Account)
            Dim cCharID As String = cPilot.ID
            Dim accountKey As Integer = 1000
            Dim beforeRefID As String = ""

            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.Settings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            transactionsXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletTransChar, cAccount.ToAPIAccount, cCharID, accountKey, beforeRefID, EveAPI.APIReturnMethods.ReturnStandard)

            'Parse the XML document
            If transactionsXML IsNot Nothing Then
                ' Get transactions
                Dim transactionList As XmlNodeList

                transactionList = transactionsXML.SelectNodes("/eveapi/result/rowset/row")

                adtLastTransactions.BeginUpdate()
                adtLastTransactions.Nodes.Clear()
                For currentTransactionCounter As Integer = 0 To Math.Min(numTransactionsDisplay - 1, transactionList.Count - 1)
                    Dim newTransaction As New Node
                    Dim transaction As XmlNode = transactionList(currentTransactionCounter)
                    If transaction IsNot Nothing Then
                        newTransaction.Text = transaction.Attributes.GetNamedItem("transactionDateTime").Value
                        newTransaction.Name = transaction.Attributes.GetNamedItem("typeName").Value & currentTransactionCounter.ToString
                        newTransaction.Cells.Add(New Cell(transaction.Attributes.GetNamedItem("typeName").Value))
                        newTransaction.Cells.Add(New Cell(CLng(transaction.Attributes.GetNamedItem("quantity").Value).ToString("N0")))
                        If transaction.Attributes.GetNamedItem("transactionType").Value = "sell" Then
                            newTransaction.Style = StyleGreen
                            newTransaction.Cells.Add(New Cell(Double.Parse(transaction.Attributes.GetNamedItem("price").Value, culture).ToString("N2"), StyleGreenRight))
                        Else
                            newTransaction.Style = StyleRed
                            newTransaction.Cells.Add(New Cell(Double.Parse(transaction.Attributes.GetNamedItem("price").Value, culture).ToString("N2"), StyleRedRight))
                        End If
                        adtLastTransactions.Nodes.Add(newTransaction)
                    End If
                Next
                adtLastTransactions.EndUpdate()
            End If
        End If
    End Sub

    Private Sub cboPilotList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilotList.SelectedIndexChanged
        Me.UpdateTransactions()
    End Sub

    Private Sub nudEntries_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudEntries.LostFocus
        Me.DBCDefaultTransactionsCount = CType(nudEntries.Value, Integer)
    End Sub

    Private Sub nudEntries_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudEntries.ValueChanged
        If cboPilotList.SelectedItem IsNot Nothing Then
            Me.DBCDefaultTransactionsCount = CType(nudEntries.Value, Integer)
            Call Me.UpdateTransactions()
        End If
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Call Me.UpdateTransactions()
    End Sub
End Class
