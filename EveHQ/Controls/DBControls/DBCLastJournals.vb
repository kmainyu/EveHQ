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
Imports System.Xml
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar

Public Class DBCLastJournals
    Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Dim RefTypes As New SortedList(Of String, String)
    Dim StyleRed As New ElementStyle
    Dim StyleRedRight As New ElementStyle
    Dim StyleGreen As New ElementStyle
    Dim StyleGreenRight As New ElementStyle

    Public Sub New()

        ' This call is required by the Windows Form Designer.

        InitializeComponent()

        Me.ControlConfigForm = "EveHQ.DBCLastJournalsConfig"
        Me.ControlConfigInfo = "<Not yet configured>"

        ' Load RefTypes
        Call Me.LoadRefTypes()

        'Populate Pilot ComboBox
        cboPilotList.BeginUpdate()
        cboPilotList.Items.Clear()
        For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
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
            Return "Last Journals"
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
            Dim numTransactionsDisplay As Integer = CType(nudEntries.Value, Integer) ' how much transactions to display in listview

            Dim transactionsXML As XmlDocument
            Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilotList.SelectedItem.ToString), Core.Pilot)
            Dim cAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(cPilot.Account), Core.EveAccount)
            Dim cCharID As String = cPilot.ID
            Dim accountKey As Integer = 1000
            Dim beforeRefID As String = ""
            Dim transA As Double = 0
            Dim transB As Double = 0

            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            transactionsXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalChar, cAccount.ToAPIAccount, cCharID, accountKey, 0, 256, EveAPI.APIReturnMethods.ReturnStandard)

            'Parse the XML document
            If transactionsXML IsNot Nothing Then
                ' Get transactions
                Dim transactionList As XmlNodeList

                transactionList = transactionsXML.SelectNodes("/eveapi/result/rowset/row")

                adtLastTransactions.BeginUpdate()
                adtLastTransactions.Nodes.Clear()
                For currentTransactionCounter As Integer = 0 To Math.Min(numTransactionsDisplay - 1, transactionList.Count - 1)
                    Dim newTransaction As New DevComponents.AdvTree.Node
                    Dim transaction As XmlNode = transactionList(currentTransactionCounter)
                    If transaction IsNot Nothing Then
                        Dim transDate As Date = DateTime.ParseExact(transaction.Attributes.GetNamedItem("date").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                        newTransaction.Text = FormatDateTime(transDate, DateFormat.GeneralDate)
                        newTransaction.Cells.Add(New Cell(RefTypes(transaction.Attributes.GetNamedItem("refTypeID").Value)))
                        transA = Double.Parse(transaction.Attributes.GetNamedItem("amount").Value, culture)
                        transB = Double.Parse(transaction.Attributes.GetNamedItem("balance").Value, culture)
                        If transA >= 0 Then
                            newTransaction.Style = StyleGreen
                            newTransaction.Cells.Add(New Cell(transA.ToString("N2"), StyleGreenRight))
                            newTransaction.Cells.Add(New Cell(transB.ToString("N2"), StyleGreenRight))
                        Else
                            newTransaction.Style = StyleRed
                            newTransaction.Cells.Add(New Cell(transA.ToString("N2"), StyleRedRight))
                            newTransaction.Cells.Add(New Cell(transB.ToString("N2"), StyleRedRight))
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

    Public Function LoadRefTypes() As Boolean
        Try
            ' Dimension variables
            Dim x As Integer = 0
            Dim refDetails As XmlNodeList
            Dim refNode As XmlNode
            Dim fileName As String = ""
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            Dim refXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.RefTypes, EveAPI.APIReturnMethods.ReturnStandard)
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
                    RefTypes.Clear()
                    For Each refNode In refDetails
                        RefTypes.Add(refNode.Attributes.GetNamedItem("refTypeID").Value, refNode.Attributes.GetNamedItem("refTypeName").Value)
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
            Exit Function
        End Try
    End Function

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Call Me.UpdateTransactions()
    End Sub
End Class
