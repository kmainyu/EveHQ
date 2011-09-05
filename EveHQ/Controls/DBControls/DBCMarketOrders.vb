' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2011  EveHQ Development Team
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

Public Class DBCMarketOrders
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.ControlConfigForm = "EveHQ.DBCMarketOrdersConfig"

        ' Load the combo box with the owner info
        cboOwner.BeginUpdate()
        cboOwner.Items.Clear()
        For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If pilot.Active = True Then
                cboOwner.Items.Add(pilot.Name)
            End If
        Next
        cboOwner.EndUpdate()

    End Sub

    Public Overrides ReadOnly Property ControlName() As String
        Get
            Return "Market Orders"
        End Get
    End Property

#Region "Custom Control Variables"
    Dim cDefaultPilotName As String = ""
#End Region

#Region "Custom Control Properties"
    Public Property DefaultPilotName() As String
        Get
            Return cDefaultPilotName
        End Get
        Set(ByVal value As String)
            cDefaultPilotName = value
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(DefaultPilotName) Then
                cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(DefaultPilotName), Core.Pilot)
            End If
            If cboOwner.Items.Contains(DefaultPilotName) = True Then
                cboOwner.SelectedItem = DefaultPilotName
                lblHeader.Text = "Market Orders - " & value
            End If
            If ReadConfig = False Then
                Me.SetConfig("DefaultPilotName", value)
                Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DefaultPilotName)
            End If
        End Set
    End Property
#End Region

#Region "Class Variables"
    Dim cPilot As EveHQ.Core.Pilot
    Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

#End Region

#Region "Market Routines"

    Private Sub ParseOrders()
        ' Get the owner we will use
        Dim owner As String = cboOwner.SelectedItem.ToString

        If owner <> "" Then
            Dim sellTotal, buyTotal, TotalEscrow As Double
            Dim TotalOrders As Integer = 0
            Dim OrderXML As New XmlDocument
            Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
            Dim accountName As String = selPilot.Account
            Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            OrderXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersChar, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
            If OrderXML IsNot Nothing Then
                Dim Orders As XmlNodeList = OrderXML.SelectNodes("/eveapi/result/rowset/row")
                clvBuyOrders.BeginUpdate()
                clvSellOrders.BeginUpdate()
                clvBuyOrders.Items.Clear()
                clvSellOrders.Items.Clear()
                For Each Order As XmlNode In Orders
                    If Order.Attributes.GetNamedItem("bid").Value = "0" Then
                        If Order.Attributes.GetNamedItem("orderState").Value = "0" Then
                            Dim sOrder As New ListViewItem
                            clvSellOrders.Items.Add(sOrder)
                            Dim itemID As String = Order.Attributes.GetNamedItem("typeID").Value
                            Dim itemName As String = ""
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                itemName = EveHQ.Core.HQ.itemData(itemID).Name
                            Else
                                itemName = "Unknown Item ID:" & itemID
                            End If
                            sOrder.Text = itemName
                            Dim quantity As Double = Double.Parse(Order.Attributes.GetNamedItem("volRemaining").Value, culture)
                            sOrder.SubItems.Add(FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim price As Double = Double.Parse(Order.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Any, culture)
                            sOrder.SubItems.Add(FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim loc As String = ""
                            loc = EveHQ.Core.DataFunctions.GetLocationName(Order.Attributes.GetNamedItem("stationID").Value)
                            sOrder.SubItems.Add(loc)
                            Dim issueDate As Date = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                            Dim orderExpires As TimeSpan = issueDate - Now
                            orderExpires = orderExpires.Add(New TimeSpan(CInt(Order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                            If orderExpires.TotalSeconds <= 0 Then
                                sOrder.SubItems.Add("Expired!")
                            Else
                                sOrder.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False))
                            End If
                            sOrder.SubItems(4).Tag = orderExpires
                            sellTotal = sellTotal + quantity * price
                            TotalOrders = TotalOrders + 1
                        ElseIf Order.Attributes.GetNamedItem("orderState").Value = "2" Then
                            Dim sOrder As New ListViewItem
                            clvRecentlySold.Items.Add(sOrder)
                            Dim itemID As String = Order.Attributes.GetNamedItem("typeID").Value
                            Dim itemName As String = ""
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                itemName = EveHQ.Core.HQ.itemData(itemID).Name
                            Else
                                itemName = "Unknown Item ID:" & itemID
                            End If
                            sOrder.Text = itemName
                            Dim quantity As Double = Double.Parse(Order.Attributes.GetNamedItem("volRemaining").Value, culture)
                            sOrder.SubItems.Add(FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim price As Double = Double.Parse(Order.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Any, culture)
                            sOrder.SubItems.Add(FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim loc As String = ""
                            loc = EveHQ.Core.DataFunctions.GetLocationName(Order.Attributes.GetNamedItem("stationID").Value)
                            sOrder.SubItems.Add(loc)
                        End If
                    Else
                        If Order.Attributes.GetNamedItem("orderState").Value = "0" Then
                            Dim bOrder As New ListViewItem
                            clvBuyOrders.Items.Add(bOrder)
                            Dim itemID As String = Order.Attributes.GetNamedItem("typeID").Value
                            Dim itemName As String = ""
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                itemName = EveHQ.Core.HQ.itemData(itemID).Name
                            Else
                                itemName = "Unknown Item ID:" & itemID
                            End If
                            bOrder.Text = itemName
                            Dim quantity As Double = Double.Parse(Order.Attributes.GetNamedItem("volRemaining").Value, culture)
                            bOrder.SubItems.Add(FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim price As Double = Double.Parse(Order.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Any, culture)
                            bOrder.SubItems.Add(FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim loc As String = ""
                            loc = EveHQ.Core.DataFunctions.GetLocationName(Order.Attributes.GetNamedItem("stationID").Value)
                            bOrder.SubItems.Add(loc)
                            'Select Case CInt(Order.Attributes.GetNamedItem("range").Value)
                            '    Case -1
                            '        bOrder.SubItems.Add("Station")
                            '    Case 0
                            '        bOrder.SubItems.Add("System")
                            '    Case 32767
                            '        bOrder.SubItems.Add("Region")
                            '    Case Is > 0, Is < 32767
                            '        bOrder.SubItems.Add(Order.Attributes.GetNamedItem("range").Value & " Jumps")
                            'End Select
                            'bOrder.SubItems(4).Tag = CInt(Order.Attributes.GetNamedItem("range").Value)
                            'bOrder.SubItems.Add(FormatNumber(Double.Parse(Order.Attributes.GetNamedItem("minVolume").Value, culture), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim issueDate As Date = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                            Dim orderExpires As TimeSpan = issueDate - Now
                            orderExpires = orderExpires.Add(New TimeSpan(CInt(Order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                            If orderExpires.TotalSeconds <= 0 Then
                                bOrder.SubItems.Add("Expired!")
                            Else
                                bOrder.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False))
                            End If
                            bOrder.SubItems(4).Tag = orderExpires
                            buyTotal = buyTotal + quantity * price
                            TotalEscrow = TotalEscrow + Double.Parse(Order.Attributes.GetNamedItem("escrow").Value, culture)
                            TotalOrders = TotalOrders + 1
                        ElseIf Order.Attributes.GetNamedItem("orderState").Value = "2" Then
                            Dim bOrder As New ListViewItem
                            clvRecentlyBought.Items.Add(bOrder)
                            Dim itemID As String = Order.Attributes.GetNamedItem("typeID").Value
                            Dim itemName As String = ""
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                itemName = EveHQ.Core.HQ.itemData(itemID).Name
                            Else
                                itemName = "Unknown Item ID:" & itemID
                            End If
                            bOrder.Text = itemName
                            Dim quantity As Double = Double.Parse(Order.Attributes.GetNamedItem("volRemaining").Value, culture)
                            bOrder.SubItems.Add(FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim price As Double = Double.Parse(Order.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Any, culture)
                            bOrder.SubItems.Add(FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim loc As String = ""
                            loc = EveHQ.Core.DataFunctions.GetLocationName(Order.Attributes.GetNamedItem("stationID").Value)
                            bOrder.SubItems.Add(loc)
                        End If
                    End If
                Next
                If clvBuyOrders.Items.Count = 0 Then
                    clvBuyOrders.Items.Add("No Data Available...")
                    clvBuyOrders.Enabled = False
                Else
                    clvBuyOrders.Enabled = True
                End If
                If clvSellOrders.Items.Count = 0 Then
                    clvSellOrders.Items.Add("No Data Available...")
                    clvSellOrders.Enabled = False
                Else
                    clvSellOrders.Enabled = True
                End If
                clvBuyOrders.EndUpdate()
                clvSellOrders.EndUpdate()
            End If

            Dim maxorders As Integer = 5 + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Trade)) * 4) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Tycoon)) * 32) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Retail)) * 8) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Wholesale)) * 16)
            Dim cover As Double = buyTotal - TotalEscrow
            Dim TransTax As Double = 1 * (1 - 0.1 * (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Accounting))))
            Dim BrokerFee As Double = 1 * (1 - 0.05 * (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BrokerRelations))))
            lblTotalOrders.Text = maxorders.ToString
            lblOrders.Text = (maxorders - TotalOrders).ToString
            lblSellTotal.Text = FormatNumber(sellTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " isk"
            lblBuyTotal.Text = FormatNumber(buyTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " isk"
            lblEscrow.Text = FormatNumber(TotalEscrow, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " isk (additional " + FormatNumber(cover, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " isk to cover)"
            lblAskRange.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Procurement)))
            lblBidRange.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Marketing)))
            lblModRange.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Daytrading)))
            lblRemoteRange.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Visibility)))
            lblBrokerFee.Text = FormatNumber(BrokerFee, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "%"
            lblTransTax.Text = FormatNumber(TransTax, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "%"
        Else
            clvBuyOrders.BeginUpdate()
            clvBuyOrders.Items.Clear()
            clvBuyOrders.Items.Add("Access Denied - check API Status")
            clvBuyOrders.EndUpdate()
            clvBuyOrders.Enabled = False
            clvSellOrders.BeginUpdate()
            clvSellOrders.Items.Clear()
            clvSellOrders.Items.Add("Access Denied - check API Status")
            clvSellOrders.EndUpdate()
            clvSellOrders.Enabled = False
            lblOrders.Text = "n/a"
            lblSellTotal.Text = "n/a"
            lblBuyTotal.Text = "n/a"
            lblEscrow.Text = "n/a"
            lblAskRange.Text = "n/a"
            lblBidRange.Text = "n/a"
            lblModRange.Text = "n/a"
            lblRemoteRange.Text = "n/a"
            lblBrokerFee.Text = "n/a"
            lblTransTax.Text = "n/a"
        End If

    End Sub

    Private Function GetOrderRange(ByVal lvl As Integer) As String
        Select Case lvl
            Case 0
                Return "Station"
            Case 1
                Return "System"
            Case 2
                Return "5 Jumps"
            Case 3
                Return "10 Jumps"
            Case 4
                Return "20 Jumps"
            Case Else
                Return "Region"
        End Select
    End Function

#End Region

    Private Sub cboOwner_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboOwner.SelectedValueChanged
        Call ParseOrders()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Call Me.ParseOrders()
    End Sub
End Class