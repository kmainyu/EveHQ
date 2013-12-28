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

                Dim journalData = HQ.ApiProvider.Character.WalletJournal(cAccount.UserID, cAccount.APIKey, CInt(cCharID))
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
                Dim refDetails As XmlNodeList
                Dim refNode As XmlNode
                Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
                Dim refXML As XmlDocument = apiReq.GetAPIXML(APITypes.RefTypes, APIReturnMethods.ReturnStandard)
                If refXML Is Nothing Then
                    ' Problem with the API server so let's use our resources to populate it
                    Try
                        refXML = New XmlDocument
                        refXML.LoadXml(My.Resources.RefTypes.ToString)
                    Catch ex As Exception
                        MessageBox.Show("There was an error loading the RefTypes API and it would appear there is a problem with the local copy. Please try again later.", "Last Journal Widget Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End Try
                End If
                Dim errlist As XmlNodeList = refXML.SelectNodes("/eveapi/error")
                If errlist.Count = 0 Then
                    refDetails = refXML.SelectNodes("/eveapi/result/rowset/row")
                    If refDetails IsNot Nothing Then
                        _refTypes.Clear()
                        For Each refNode In refDetails
                            _refTypes.Add(refNode.Attributes.GetNamedItem("refTypeID").Value, refNode.Attributes.GetNamedItem("refTypeName").Value)
                        Next
                    End If
                Else
                    Dim errNode As XmlNode = errlist(0)
                    ' Get error code
                    Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                    Dim errMsg As String = errNode.InnerText
                    MessageBox.Show("The RefTypes API returned error " & errCode & ": " & errMsg, "RefTypes Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
                Return True
            Catch e As Exception
                MessageBox.Show("There was an error loading the RefTypes API. The error was: " & e.Message, "Last Journal Widget Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

        Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefresh.Click
            Call UpdateTransactions()
        End Sub
    End Class
End NameSpace