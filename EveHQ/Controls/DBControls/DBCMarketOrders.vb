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
Imports EveHQ.EveData
Imports EveHQ.EveApi
Imports EveHQ.Core
Imports System.Xml
Imports EveHQ.Common.Extensions

Namespace Controls.DBControls
    Public Class DBCMarketOrders
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            ControlConfigForm = "EveHQ.Controls.DBConfigs.DBCMarketOrdersConfig"

            ' Load the combo box with the owner info
            cboOwner.BeginUpdate()
            cboOwner.Items.Clear()
            For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
                If pilot.Active = True And pilot.Account <> "" Then
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

        Dim _defaultPilotName As String = ""

        Public Property DefaultPilotName() As String
            Get
                Return _defaultPilotName
            End Get
            Set(ByVal value As String)
                _defaultPilotName = value
                If HQ.Settings.Pilots.ContainsKey(DefaultPilotName) Then
                End If
                If cboOwner.Items.Contains(DefaultPilotName) = True Then
                    cboOwner.SelectedItem = DefaultPilotName
                    lblHeader.Text = "Market Orders - " & value
                End If
                If ReadConfig = False Then
                    SetConfig("DefaultPilotName", value)
                    SetConfig("ControlConfigInfo", "Default Pilot: " & DefaultPilotName)
                End If
            End Set
        End Property

        Private Const IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        ReadOnly _culture As CultureInfo = New CultureInfo("en-GB")

        Private Sub ParseOrders()
            ' Get the owner we will use
            Dim owner As String = cboOwner.SelectedItem.ToString

            If owner <> "" Then
                Dim sellTotal, buyTotal, totalEscrow As Double
                Dim totalOrders As Integer = 0
                Dim orderXML As XmlDocument
                Dim selPilot As EveHQPilot = HQ.Settings.Pilots(owner)
                Dim pilotAccount As EveHQAccount = HQ.Settings.Accounts.Item(selPilot.Account)

                Dim ordersResponse As EveServiceResponse(Of IEnumerable(Of MarketOrder)) =
                        HQ.ApiProvider.Character.MarketOrders(pilotAccount.UserID, pilotAccount.APIKey,
                                                              Integer.Parse(selPilot.ID))

                If ordersResponse.ResultData IsNot Nothing Then

                    clvBuyOrders.BeginUpdate()
                    clvSellOrders.BeginUpdate()
                    clvBuyOrders.Items.Clear()
                    clvSellOrders.Items.Clear()
                    For Each order As MarketOrder In ordersResponse.ResultData
                        If order.IsBuyOrder = False Then

                            If order.OrderState = MarketOrderState.Active Then
                                Dim sOrder As New ListViewItem
                                clvSellOrders.Items.Add(sOrder)

                                Dim itemName As String
                                Dim itemType As EveType
                                If StaticData.Types.TryGetValue(order.TypeId, itemType) = True Then
                                    itemName = itemType.Name
                                Else
                                    itemName = "Unknown Item ID:" & order.TypeId.ToInvariantString()
                                End If
                                sOrder.Text = itemName
                                Dim quantity As Double = order.QuantityRemaining
                                sOrder.SubItems.Add(
                                    quantity.ToString("N0") & " / " &
                                    order.QuantityEntered.ToString("N0"))
                                Dim price As Double = order.Price
                                sOrder.SubItems.Add(price.ToString("N2"))
                                Dim loc As String
                                loc = StaticData.GetLocationName(order.StationId)
                                sOrder.SubItems.Add(loc)
                                Dim issueDate As Date = order.DateIssued.DateTime
                                Dim orderExpires As TimeSpan = issueDate - Now
                                orderExpires =
                                    orderExpires.Add(order.Duration)
                                If orderExpires.TotalSeconds <= 0 Then
                                    sOrder.SubItems.Add("Expired!")
                                Else
                                    sOrder.SubItems.Add(SkillFunctions.TimeToString(orderExpires.TotalSeconds, False))
                                End If
                                sOrder.SubItems(4).Tag = orderExpires
                                sellTotal = sellTotal + quantity*price
                                totalOrders = totalOrders + 1
                            ElseIf order.OrderState = MarketOrderState.Expired Then
                                Dim sOrder As New ListViewItem
                                clvRecentlySold.Items.Add(sOrder)
                                Dim itemID As String = order.TypeId.ToInvariantString()
                                Dim itemName As String
                                Dim itemType As EveType
                                If StaticData.Types.TryGetValue(order.TypeId, itemType) = True Then
                                    itemName = itemType.Name
                                Else
                                    itemName = "Unknown Item ID:" & order.TypeId.ToInvariantString()
                                End If
                                sOrder.Text = itemName
                                Dim quantity As Double = order.QuantityRemaining
                                sOrder.SubItems.Add(
                                    quantity.ToString("N0") & " / " &
                                    order.QuantityEntered.ToString("N0"))
                                Dim price As Double = order.Price
                                sOrder.SubItems.Add(price.ToString("N2"))
                                Dim loc As String
                                loc = StaticData.GetLocationName(order.StationId)
                                sOrder.SubItems.Add(loc)
                            End If
                        Else
                            If order.OrderState = MarketOrderState.Active Then
                                Dim bOrder As New ListViewItem
                                clvBuyOrders.Items.Add(bOrder)
                                Dim itemID As String = order.TypeId.ToInvariantString
                                Dim itemName As String
                                Dim itemType As EveType
                                If StaticData.Types.TryGetValue(order.TypeId, itemType) = True Then
                                    itemName = itemType.Name
                                Else
                                    itemName = "Unknown Item ID:" & order.TypeId.ToInvariantString()
                                End If
                                bOrder.Text = itemName
                                Dim quantity As Double = order.QuantityRemaining

                                bOrder.SubItems.Add(
                                    quantity.ToString("N0") & " / " &
                                    order.QuantityEntered.ToString("N0"))
                                Dim price As Double = order.Price
                                bOrder.SubItems.Add(price.ToString("N2"))
                                Dim loc As String
                                loc = StaticData.GetLocationName(order.StationId)
                                bOrder.SubItems.Add(loc)
                                Dim issueDate As Date = order.DateIssued.DateTime
                                Dim orderExpires As TimeSpan = issueDate - Now
                                orderExpires =
                                    orderExpires.Add(order.Duration)
                                If orderExpires.TotalSeconds <= 0 Then
                                    bOrder.SubItems.Add("Expired!")
                                Else
                                    bOrder.SubItems.Add(SkillFunctions.TimeToString(orderExpires.TotalSeconds, False))
                                End If
                                bOrder.SubItems(4).Tag = orderExpires
                                buyTotal = buyTotal + quantity*price
                                totalEscrow = totalEscrow + order.Escrow

                                totalOrders = totalOrders + 1
                            ElseIf order.OrderState = MarketOrderState.Expired Then
                                Dim bOrder As New ListViewItem
                                clvRecentlyBought.Items.Add(bOrder)
                                Dim itemID As String = order.TypeId.ToInvariantString()
                                Dim itemName As String
                                Dim itemType As EveType
                                If StaticData.Types.TryGetValue(order.TypeId, itemType) = True Then
                                    itemName = itemType.Name
                                Else
                                    itemName = "Unknown Item ID:" & order.TypeId.ToInvariantString()
                                End If
                                bOrder.Text = itemName
                                Dim quantity As Double = order.QuantityRemaining
                                bOrder.SubItems.Add(
                                    quantity.ToString("N0") & " / " &
                                    order.QuantityEntered.ToString("N0"))
                                Dim price As Double = order.Price
                                bOrder.SubItems.Add(price.ToString("N2"))
                                Dim loc As String
                                loc = StaticData.GetLocationName(order.StationId)
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

                Dim maxorders As Integer = 5 + (selPilot.KeySkills(KeySkill.Trade)*4) +
                                           (selPilot.KeySkills(KeySkill.Tycoon)*32) +
                                           (selPilot.KeySkills(KeySkill.Retail)*8) +
                                           (selPilot.KeySkills(KeySkill.Wholesale)*16)
                Dim cover As Double = buyTotal - totalEscrow
                Dim transTax As Double = 1*(1.5 - 0.15*(selPilot.KeySkills(KeySkill.Accounting)))
                Dim brokerFee As Double = 1*(1 - 0.05*(selPilot.KeySkills(KeySkill.BrokerRelations)))
                lblTotalOrders.Text = maxorders.ToString
                lblOrders.Text = (maxorders - totalOrders).ToString
                lblSellTotal.Text = sellTotal.ToString("N2") & " isk"
                lblBuyTotal.Text = buyTotal.ToString("N2") & " isk"
                lblEscrow.Text = totalEscrow.ToString("N2") & " isk (additional " + cover.ToString("N2") &
                                 " isk to cover)"
                lblAskRange.Text = GetOrderRange(selPilot.KeySkills(KeySkill.Procurement))
                lblBidRange.Text = GetOrderRange(selPilot.KeySkills(KeySkill.Marketing))
                lblModRange.Text = GetOrderRange(selPilot.KeySkills(KeySkill.Daytrading))
                lblRemoteRange.Text = GetOrderRange(selPilot.KeySkills(KeySkill.Visibility))
                lblBrokerFee.Text = brokerFee.ToString("N2") & "%"
                lblTransTax.Text = transTax.ToString("N2") & "%"
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
                    Return "EveGalaticRegion"
            End Select
        End Function

        Private Sub cboOwner_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboOwner.SelectedValueChanged
            Call ParseOrders()
        End Sub

        Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefresh.Click
            Call ParseOrders()
        End Sub
    End Class
End NameSpace