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
Imports System.Text

Public Class Contract

    Public ContractID As Long
    Public Owner As String
    Public IssuerID As Long
    Public IssuerCorpID As Long
    Public AssigneeID As Long
    Public AcceptorID As Long
    Public StartStationID As Long
    Public EndStationID As Long
    Public Type As ContractTypes
    Public Status As ContractStatuses
    Public Title As String
    Public ForCorp As Boolean
    Public IsIssuer As Boolean = False
    Public Availability As String
    Public DateIssued As Date
    Public DateExpired As Date
    Public DateAccepted As String
    Public NumDays As Integer
    Public DateCompleted As String
    Public Price As Double
    Public Reward As Double
    Public Collateral As Double
    Public Buyout As Double
    Public Volume As Double
    Public Items As New SortedList(Of String, Long)

End Class

Public Class Contracts

    Private Shared IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Private Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Public Shared Contracts As SortedList(Of Long, Contract)

    Public Shared Function ParseContracts(OrderOwner As String) As SortedList(Of Long, Contract)

        Dim Owner As New PrismOwner
        Dim newOrderCollection As New MarketOrdersCollection

        If PlugInData.PrismOwners.ContainsKey(OrderOwner) = True Then

            Owner = PlugInData.PrismOwners(OrderOwner)
            Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Contracts)
            Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Contracts)
            Dim ContractXML As New XmlDocument
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

            If OwnerAccount IsNot Nothing Then

                If Owner.IsCorp = True Then
                    ContractXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                Else
                    ContractXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                End If

                If ContractXML IsNot Nothing Then

                    ' Get the Node List
                    Dim ContractNodes As XmlNodeList = ContractXML.SelectNodes("/eveapi/result/rowset/row")

                    ' Parse the Node List
                    Dim ContractList As New SortedList(Of Long, Contract)
                    For Each Contract As XmlNode In ContractNodes
                        Dim NewContract As New Contract
                        NewContract.ContractID = CLng(Contract.Attributes.GetNamedItem("contractID").Value)
                        NewContract.Owner = Owner.Name
                        NewContract.IssuerID = CLng(Contract.Attributes.GetNamedItem("issuerID").Value)
                        NewContract.IssuerCorpID = CLng(Contract.Attributes.GetNamedItem("issuerCorpID").Value)
                        NewContract.AssigneeID = CLng(Contract.Attributes.GetNamedItem("assigneeID").Value)
                        NewContract.AcceptorID = CLng(Contract.Attributes.GetNamedItem("acceptorID").Value)
                        NewContract.StartStationID = CInt(Contract.Attributes.GetNamedItem("startStationID").Value)
                        NewContract.EndStationID = CLng(Contract.Attributes.GetNamedItem("endStationID").Value)
                        Select Case Contract.Attributes.GetNamedItem("type").Value
                            Case "ItemExchange"
                                NewContract.Type = ContractTypes.ItemExchange
                            Case "Auction"
                                NewContract.Type = ContractTypes.Auction

                            Case "Courier"
                                NewContract.Type = ContractTypes.Courier
                            Case Else
                                NewContract.Type = ContractTypes.Other
                        End Select
                        Select Case Contract.Attributes.GetNamedItem("status").Value
                            Case "Outstanding"
                                NewContract.Status = ContractStatuses.Outstanding
                            Case "Completed"
                                NewContract.Status = ContractStatuses.Completed
                            Case "CompletedByContractor"
                                NewContract.Status = ContractStatuses.CompletedByContractor
                            Case Else
                                NewContract.Status = ContractStatuses.Other
                        End Select
                        NewContract.Title = Contract.Attributes.GetNamedItem("title").Value
                        NewContract.ForCorp = CBool(Contract.Attributes.GetNamedItem("forCorp").Value)
                        If NewContract.IssuerID = CLng(Owner.ID) Or NewContract.IssuerCorpID = CLng(Owner.ID) Then
                            NewContract.IsIssuer = True
                        End If
                        NewContract.Availability = Contract.Attributes.GetNamedItem("availability").Value
                        NewContract.DateIssued = DateTime.ParseExact(Contract.Attributes.GetNamedItem("dateIssued").Value, IndustryTimeFormat, culture)
                        NewContract.DateExpired = DateTime.ParseExact(Contract.Attributes.GetNamedItem("dateExpired").Value, IndustryTimeFormat, culture)
                        NewContract.DateAccepted = Contract.Attributes.GetNamedItem("dateAccepted").Value
                        NewContract.NumDays = CInt(Contract.Attributes.GetNamedItem("numDays").Value)
                        NewContract.DateCompleted = Contract.Attributes.GetNamedItem("dateCompleted").Value
                        NewContract.Price = Double.Parse(Contract.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Any, culture)
                        NewContract.Reward = Double.Parse(Contract.Attributes.GetNamedItem("reward").Value, Globalization.NumberStyles.Any, culture)
                        NewContract.Collateral = Double.Parse(Contract.Attributes.GetNamedItem("collateral").Value, Globalization.NumberStyles.Any, culture)
                        NewContract.Buyout = Double.Parse(Contract.Attributes.GetNamedItem("buyout").Value, Globalization.NumberStyles.Any, culture)
                        NewContract.Volume = Double.Parse(Contract.Attributes.GetNamedItem("volume").Value, Globalization.NumberStyles.Any, culture)

                        ' Check for items
                        If Owner.IsCorp = True Then
                            ContractXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractItemsCorp, OwnerAccount.ToAPIAccount, OwnerID, NewContract.ContractID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        Else
                            ContractXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractItemsChar, OwnerAccount.ToAPIAccount, OwnerID, NewContract.ContractID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        End If
                        If ContractXML IsNot Nothing Then
                            NewContract.Items = ParseContractItems(ContractXML)
                        End If

                        'Check whether this is a corp contract and if the owner is a corp etc
                        If Owner.IsCorp = NewContract.ForCorp Then
                            ContractList.Add(NewContract.ContractID, NewContract)
                        End If

                    Next
                    Return ContractList
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

    Private Shared Function ParseContractItems(ItemXML As XmlDocument) As SortedList(Of String, Long)
        Dim ItemList As New SortedList(Of String, Long)
        If ItemXML IsNot Nothing Then
            ' Get the Node List
            Dim ContractItems As XmlNodeList = ItemXML.SelectNodes("/eveapi/result/rowset/row")
            ' Parse the Node List
            For Each ContractItem As XmlNode In ContractItems
                Dim NewContract As New Contract
                If ContractItem.Attributes.GetNamedItem("included").Value = "1" Then
                    Dim typeID As String = ContractItem.Attributes.GetNamedItem("typeID").Value
                    Dim qty As Long = CLng(ContractItem.Attributes.GetNamedItem("quantity").Value)
                    If ItemList.ContainsKey(typeID) = False Then
                        ItemList.Add(typeID, qty)
                    Else
                        ItemList(typeID) += qty
                    End If
                End If
            Next
        End If
        Return ItemList
    End Function

    Public Shared Function GetContractIDList(ByVal ContractList As SortedList(Of Long, Contract)) As SortedList(Of Long, String)
        Dim IDList As New List(Of String)
        For Each C As Contract In ContractList.Values
            If IDList.Contains(C.AcceptorID.ToString) = False Then
                IDList.Add(C.AcceptorID.ToString)
            End If
            If IDList.Contains(C.AssigneeID.ToString) = False Then
                IDList.Add(C.AssigneeID.ToString)
            End If
            If IDList.Contains(C.IssuerID.ToString) = False Then
                IDList.Add(C.IssuerID.ToString)
            End If
        Next
        Dim InstallerList As New SortedList(Of Long, String)
        Dim strID As New StringBuilder
        If IDList.Count > 0 Then
            For Each ID As String In IDList
                If ID <> "" Then
                    strID.Append("," & ID)
                End If
            Next
            strID.Remove(0, 1)
            ' Get the name data from the DB
            Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
            Dim IDData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
            If IDData IsNot Nothing Then
                If IDData.Tables(0).Rows.Count > 0 Then
                    For Each IDRow As DataRow In IDData.Tables(0).Rows
                        InstallerList.Add(CLng(IDRow.Item("eveID")), CStr(IDRow.Item("eveName")))
                    Next
                End If
            End If
        End If
        ' Add in any other IDs we don't have
        For Each ID As String In IDList
            If InstallerList.ContainsKey(CLng(ID)) = False Then
                InstallerList.Add(CLng(ID), ID)
            End If
        Next
        Return InstallerList
    End Function

End Class

Public Enum ContractTypes
    ItemExchange = 0
    Auction = 1
    Courier = 2
    Other = 3
End Enum

Public Enum ContractStatuses
    Outstanding = 0
    Completed = 1
    CompletedByContractor = 2
    Other = 3
End Enum


