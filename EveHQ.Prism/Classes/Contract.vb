
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
Imports System.Text
Imports EveHQ.EveAPI
Imports EveHQ.Common.Extensions
Imports EveHQ.Core

Namespace Classes
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
        Public Shared Contracts As SortedList(Of Long, Contract)

        Public Shared Function ParseContracts(orderOwner As String) As SortedList(Of Long, Contract)

            Dim owner As PrismOwner

            If PlugInData.PrismOwners.ContainsKey(OrderOwner) = True Then

                owner = PlugInData.PrismOwners(OrderOwner)
                Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(owner, CorpRepType.Contracts)
                Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(owner, CorpRepType.Contracts)

                Dim contractResponse As EveServiceResponse(Of IEnumerable(Of EveApi.Contract))
                Dim contractItemsResponse As EveServiceResponse(Of IEnumerable(Of ContractItem))
             
                If ownerAccount IsNot Nothing Then

                    If owner.IsCorp = True Then
                        contractResponse = HQ.ApiProvider.Corporation.Contracts(ownerAccount.UserID, ownerAccount.APIKey,
                                                                                ownerID.ToInt32())

                    Else
                        contractResponse = HQ.ApiProvider.Character.Contracts(ownerAccount.UserID, ownerAccount.APIKey,
                                                                              ownerID.ToInt32())

                    End If

                    If contractResponse IsNot Nothing Then

                        ' Get the Node List

                        ' Parse the Node List
                        ' TODO: if both the API entity and prism class are identical, they should use the same type
                        Dim contractList As New SortedList(Of Long, Contract)
                        For Each contract As EveApi.Contract In contractResponse.ResultData
                            Dim newContract As New Contract
                            newContract.ContractID = contract.ContractId
                            newContract.Owner = owner.Name
                            newContract.IssuerID = contract.IssuerId
                            newContract.IssuerCorpID = contract.IssuserCorpId
                            newContract.AssigneeID = contract.AssigneeId
                            newContract.AcceptorID = contract.AcceptorId
                            newContract.StartStationID = contract.StartStationId
                            newContract.EndStationID = contract.EndStationId
                            Select Case contract.Type
                                Case ContractType.ItemExchange
                                    newContract.Type = ContractTypes.ItemExchange
                                Case ContractType.Auction
                                    newContract.Type = ContractTypes.Auction

                                Case ContractType.Courier
                                    newContract.Type = ContractTypes.Courier
                                Case Else
                                    newContract.Type = ContractTypes.Other
                            End Select
                            Select Case contract.Status
                                Case ContractStatus.Outstanding
                                    newContract.Status = ContractStatuses.Outstanding
                                Case ContractStatus.Completed
                                    newContract.Status = ContractStatuses.Completed
                                Case ContractStatus.CompletedByContractor
                                    newContract.Status = ContractStatuses.CompletedByContractor
                                Case Else
                                    newContract.Status = ContractStatuses.Other
                            End Select
                            newContract.Title = contract.Title
                            newContract.ForCorp = contract.ForCorp
                            If newContract.IssuerID = CLng(owner.ID) Or newContract.IssuerCorpID = CLng(owner.ID) Then
                                newContract.IsIssuer = True
                            End If
                            newContract.Availability = contract.Availability.ToString()
                            newContract.DateIssued = contract.DateIssued.DateTime
                            newContract.DateExpired = contract.DateExpired.DateTime
                            newContract.DateAccepted = contract.DateAccepted.DateTime.ToInvariantString()
                            newContract.NumDays = contract.NumberOfDays
                            newContract.DateCompleted = contract.DateCompleted.DateTime.ToInvariantString()
                            newContract.Price = contract.Price
                            newContract.Reward = contract.Reward
                            newContract.Collateral = contract.Collateral
                            newContract.Buyout = contract.Buyout
                            newContract.Volume = contract.Volume

                            ' Check for items
                            If owner.IsCorp = True Then
                                contractItemsResponse = HQ.ApiProvider.Corporation.ContractItems(ownerAccount.UserID,
                                                                                                 ownerAccount.UserID,
                                                                                                 ownerID.ToInt32(),
                                                                                                 newContract.ContractID)
                            Else
                                contractItemsResponse = HQ.ApiProvider.Character.ContractItems(ownerAccount.UserID,
                                                                                               ownerAccount.UserID,
                                                                                               ownerID.ToInt32(),
                                                                                               newContract.ContractID)
                            End If
                            If contractItemsResponse.IsSuccess Then
                                newContract.Items = ParseContractItems(contractItemsResponse.ResultData)
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

        Private Shared Function ParseContractItems(items As IEnumerable(Of ContractItem)) _
            As SortedList(Of Integer, Long)
            Dim itemList As New SortedList(Of Integer, Long)
            If items IsNot Nothing Then
                ' Parse the Node List
                For Each contractItem As ContractItem In items
                    If contractItem.IsIncluded Then
                        Dim typeID As Integer = contractItem.TypeId
                        Dim qty As Long = contractItem.Quantity

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

        Public Shared Function GetContractIDList(ByVal contractList As SortedList(Of Long, Contract)) _
            As SortedList(Of Long, String)
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
                Dim strSql As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                Dim idData As DataSet = CustomDataFunctions.GetCustomData(strSql)
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
End Namespace