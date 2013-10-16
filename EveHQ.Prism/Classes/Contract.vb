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
Imports System.Xml
Imports EveHQ.EveAPI
Imports EveHQ.Core
Imports System.Text

Public Class Contract

    Public ContractID As Long
    Public Owner As String
    Public IssuerID As Long
    Public IssuerCorpID As Long
    Public AssigneeID As Long
    Public AcceptorID As Long
    Public StartStationID As Integer
    Public EndStationID As Integer
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
    Public Items As New SortedList(Of Integer, Long)

End Class

Public Class Contracts
    Private Const IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Private Shared ReadOnly Culture As New CultureInfo("en-GB")
    Public Shared Contracts As SortedList(Of Long, Contract)

    Public Shared Function ParseContracts(orderOwner As String) As SortedList(Of Long, Contract)

        Dim owner As PrismOwner

        If PlugInData.PrismOwners.ContainsKey(OrderOwner) = True Then

            owner = PlugInData.PrismOwners(OrderOwner)
            Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(owner, CorpRepType.Contracts)
            Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(owner, CorpRepType.Contracts)
            Dim contractXML As XmlDocument
            Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)

            If ownerAccount IsNot Nothing Then

                If owner.IsCorp = True Then
                    contractXML = apiReq.GetAPIXML(APITypes.ContractsCorp, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                Else
                    contractXML = apiReq.GetAPIXML(APITypes.ContractsChar, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                End If

                If contractXML IsNot Nothing Then

                    ' Get the Node List
                    Dim contractNodes As XmlNodeList = contractXML.SelectNodes("/eveapi/result/rowset/row")

                    ' Parse the Node List
                    Dim contractList As New SortedList(Of Long, Contract)
                    For Each contract As XmlNode In contractNodes
                        Dim newContract As New Contract
                        newContract.ContractID = CLng(contract.Attributes.GetNamedItem("contractID").Value)
                        newContract.Owner = owner.Name
                        newContract.IssuerID = CLng(contract.Attributes.GetNamedItem("issuerID").Value)
                        newContract.IssuerCorpID = CLng(contract.Attributes.GetNamedItem("issuerCorpID").Value)
                        newContract.AssigneeID = CLng(contract.Attributes.GetNamedItem("assigneeID").Value)
                        newContract.AcceptorID = CLng(contract.Attributes.GetNamedItem("acceptorID").Value)
                        newContract.StartStationID = CInt(contract.Attributes.GetNamedItem("startStationID").Value)
                        newContract.EndStationID = CInt(contract.Attributes.GetNamedItem("endStationID").Value)
                        Select Case contract.Attributes.GetNamedItem("type").Value
                            Case "ItemExchange"
                                newContract.Type = ContractTypes.ItemExchange
                            Case "Auction"
                                newContract.Type = ContractTypes.Auction

                            Case "Courier"
                                newContract.Type = ContractTypes.Courier
                            Case Else
                                newContract.Type = ContractTypes.Other
                        End Select
                        Select Case contract.Attributes.GetNamedItem("status").Value
                            Case "Outstanding"
                                newContract.Status = ContractStatuses.Outstanding
                            Case "Completed"
                                newContract.Status = ContractStatuses.Completed
                            Case "CompletedByContractor"
                                newContract.Status = ContractStatuses.CompletedByContractor
                            Case Else
                                newContract.Status = ContractStatuses.Other
                        End Select
                        newContract.Title = contract.Attributes.GetNamedItem("title").Value
                        newContract.ForCorp = CBool(contract.Attributes.GetNamedItem("forCorp").Value)
                        If newContract.IssuerID = CLng(owner.ID) Or newContract.IssuerCorpID = CLng(owner.ID) Then
                            newContract.IsIssuer = True
                        End If
                        newContract.Availability = contract.Attributes.GetNamedItem("availability").Value
                        newContract.DateIssued = DateTime.ParseExact(contract.Attributes.GetNamedItem("dateIssued").Value, IndustryTimeFormat, Culture)
                        newContract.DateExpired = DateTime.ParseExact(contract.Attributes.GetNamedItem("dateExpired").Value, IndustryTimeFormat, Culture)
                        newContract.DateAccepted = contract.Attributes.GetNamedItem("dateAccepted").Value
                        newContract.NumDays = CInt(contract.Attributes.GetNamedItem("numDays").Value)
                        newContract.DateCompleted = contract.Attributes.GetNamedItem("dateCompleted").Value
                        newContract.Price = Double.Parse(contract.Attributes.GetNamedItem("price").Value, NumberStyles.Any, Culture)
                        newContract.Reward = Double.Parse(contract.Attributes.GetNamedItem("reward").Value, NumberStyles.Any, Culture)
                        newContract.Collateral = Double.Parse(contract.Attributes.GetNamedItem("collateral").Value, NumberStyles.Any, Culture)
                        newContract.Buyout = Double.Parse(contract.Attributes.GetNamedItem("buyout").Value, NumberStyles.Any, Culture)
                        newContract.Volume = Double.Parse(contract.Attributes.GetNamedItem("volume").Value, NumberStyles.Any, Culture)

                        ' Check for items
                        If owner.IsCorp = True Then
                            contractXML = apiReq.GetAPIXML(APITypes.ContractItemsCorp, ownerAccount.ToAPIAccount, ownerID, newContract.ContractID, APIReturnMethods.ReturnCacheOnly)
                        Else
                            contractXML = apiReq.GetAPIXML(APITypes.ContractItemsChar, ownerAccount.ToAPIAccount, ownerID, newContract.ContractID, APIReturnMethods.ReturnCacheOnly)
                        End If
                        If contractXML IsNot Nothing Then
                            newContract.Items = ParseContractItems(contractXML)
                        End If

                        'Check whether this is a corp contract and if the owner is a corp etc
                        If owner.IsCorp = newContract.ForCorp Then
                            contractList.Add(newContract.ContractID, newContract)
                        End If

                    Next
                    Return contractList
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

    Private Shared Function ParseContractItems(itemXML As XmlDocument) As SortedList(Of Integer, Long)
        Dim itemList As New SortedList(Of Integer, Long)
        If itemXML IsNot Nothing Then
            ' Get the Node List
            Dim contractItems As XmlNodeList = itemXML.SelectNodes("/eveapi/result/rowset/row")
            ' Parse the Node List
            For Each contractItem As XmlNode In contractItems
                If contractItem.Attributes.GetNamedItem("included").Value = "1" Then
                    Dim typeID As Integer = CInt(contractItem.Attributes.GetNamedItem("typeID").Value)
                    Dim qty As Long = CLng(contractItem.Attributes.GetNamedItem("quantity").Value)
                    If itemList.ContainsKey(typeID) = False Then
                        itemList.Add(typeID, qty)
                    Else
                        itemList(typeID) += qty
                    End If
                End If
            Next
        End If
        Return itemList
    End Function

    Public Shared Function GetContractIDList(ByVal contractList As SortedList(Of Long, Contract)) As SortedList(Of Long, String)
        Dim idList As New List(Of String)
        For Each c As Contract In contractList.Values
            If idList.Contains(c.AcceptorID.ToString) = False Then
                idList.Add(c.AcceptorID.ToString)
            End If
            If idList.Contains(c.AssigneeID.ToString) = False Then
                idList.Add(c.AssigneeID.ToString)
            End If
            If idList.Contains(c.IssuerID.ToString) = False Then
                idList.Add(c.IssuerID.ToString)
            End If
        Next
        Dim installerList As New SortedList(Of Long, String)
        Dim strID As New StringBuilder
        If idList.Count > 0 Then
            For Each id As String In idList
                If id <> "" Then
                    strID.Append("," & id)
                End If
            Next
            strID.Remove(0, 1)
            ' Get the name data from the DB
            Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
            Dim idData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
            If idData IsNot Nothing Then
                If idData.Tables(0).Rows.Count > 0 Then
                    For Each idRow As DataRow In idData.Tables(0).Rows
                        installerList.Add(CLng(idRow.Item("eveID")), CStr(idRow.Item("eveName")))
                    Next
                End If
            End If
        End If
        ' Add in any other IDs we don't have
        For Each id As String In idList
            If installerList.ContainsKey(CLng(id)) = False Then
                installerList.Add(CLng(id), id)
            End If
        Next
        Return installerList
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


