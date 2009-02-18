' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports System.Windows.Forms
Imports System.Xml
Imports DotNetLib.Windows.Forms
Imports System
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Resources
Imports System.IO
Imports System.Diagnostics
Imports System.Text
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmPrism

#Region "Class Wide Variables"

    Dim filters As New ArrayList
    Dim catFilters As New ArrayList
    Dim groupFilters As New ArrayList
    Dim loadedOwners As New SortedList
    Dim loadedAssets As New SortedList
    Dim assetList As New SortedList
    Dim tempAssetList As New ArrayList
    Dim divisions As New SortedList
    Dim walletDivisions As New SortedList
    Dim charWallets As New SortedList
    Dim corpWallets As New SortedList
    Dim corpWalletDivisions As New SortedList
    Dim searchText As String = ""
    Dim totalAssetValue As Double = 0
    Dim totalAssetCount As Long = 0
    Dim totalAssetBatch As Long = 0
    Dim HQFShip As New ArrayList
    Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss.fff"

    ' Rig Builder Variables
    Dim RigBPData As New SortedList
    Dim RigBuildData As New SortedList
    Dim SalvageList As New SortedList

    ' Corp API Representatives & Lists
    Dim CorpList As New SortedList
    Dim CorpReps As New ArrayList

    Delegate Sub CheckXMLDelegate(ByVal apiXML As XmlDocument, ByVal Owner As String, ByVal Primary As String, ByVal Pos As Integer)
    Private XMLDelegate As CheckXMLDelegate

#End Region

#Region "Form Initialisation Routines"
    Private Sub frmAssets_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        XMLDelegate = New CheckXMLDelegate(AddressOf CheckXML)

        ' Build a corp list
        Call Me.BuildCorpList()

        ' Set the Corp Reps to Default
        CorpReps.Clear()
        For API As Integer = 0 To 6
            CorpReps.Add(New SortedList)
        Next

        Call Me.LoadFilterGroups()
        Call Me.ScanForExistingXMLs()
        Call Portfolio.SetupTypes()
        Call Me.LoadInvestments()

        cboPilots.SelectedItem = EveHQ.Core.HQ.myPilot.Name

    End Sub
    Private Sub BuildCorpList()
        CorpList.Clear()
        For Each selpilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If CorpList.ContainsKey(selpilot.Corp) = False Then
                CorpList.Add(selpilot.Corp, selpilot.CorpID)
            End If
        Next
    End Sub
    Private Sub LoadFilterGroups()
        Dim newNode As TreeNode
        tvwFilter.BeginUpdate()
        tvwFilter.Nodes.Clear()
        ' Load up the filter with categories
        For Each cat As String In EveHQ.Core.HQ.catList.GetKeyList
            newNode = New TreeNode
            newNode.Text = cat
            newNode.Name = CStr(EveHQ.Core.HQ.catList(cat))
            tvwFilter.Nodes.Add(newNode)
        Next
        ' Load up the filter with groups
        For Each group As String In EveHQ.Core.HQ.groupList.GetKeyList
            newNode = New TreeNode
            newNode.Text = group
            newNode.Name = CStr(EveHQ.Core.HQ.groupList(group))
            tvwFilter.Nodes(EveHQ.Core.HQ.groupCats(newNode.Name).ToString).Nodes.Add(newNode)
        Next
        ' Update the filter
        tvwFilter.Sorted = True
        tvwFilter.EndUpdate()
    End Sub
    Private Sub ScanForExistingXMLs()
        lvwCurrentAPIs.BeginUpdate()
        lvwCharFilter.BeginUpdate()
        lvwCurrentAPIs.Items.Clear()
        loadedOwners.Clear()
        cboPilots.Items.Clear()
        lvwCharFilter.Items.Clear()
        Dim fileName As String = ""
        Dim apiXML As New XmlDocument

        ' Get a list of Pilot and Corps
        For Each selpilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selpilot.Account <> "" Then
                If lvwCurrentAPIs.Items.ContainsKey(selpilot.ID) = False Then
                    Dim newOwner As New ListViewItem
                    newOwner.UseItemStyleForSubItems = False
                    newOwner.Name = selpilot.ID
                    newOwner.Text = selpilot.Name
                    newOwner.SubItems.Add("Character")
                    For si As Integer = 2 To 8
                        newOwner.SubItems.Add("")
                    Next
                    newOwner.SubItems(8).Text = "n/a"
                    lvwCurrentAPIs.Items.Add(newOwner)
                    loadedOwners.Add(selpilot.Name, selpilot)
                    cboPilots.Items.Add(selpilot.Name)
                    Dim newChar As New ListViewItem(selpilot.Name, lvwCharFilter.Groups.Item("grpPersonal"))
                    lvwCharFilter.Items.Add(newChar)
                End If
                If lvwCurrentAPIs.Items.ContainsKey(selpilot.CorpID) = False Then
                    If PlugInData.NPCCorps.Contains(selpilot.CorpID) = False Then
                        Dim newOwner As New ListViewItem
                        newOwner.UseItemStyleForSubItems = False
                        newOwner.Name = selpilot.CorpID
                        newOwner.Text = selpilot.Corp
                        newOwner.SubItems.Add("Corporation")
                        For si As Integer = 2 To 8
                            newOwner.SubItems.Add("")
                        Next
                        lvwCurrentAPIs.Items.Add(newOwner)
                        Dim newChar As New ListViewItem(selpilot.Corp, lvwCharFilter.Groups.Item("grpCorporation"))
                        If loadedOwners.Contains(selpilot.Corp) = False Then
                            loadedOwners.Add(selpilot.Corp, selpilot)
                            cboPilots.Items.Add(selpilot.Corp)
                            lvwCharFilter.Items.Add(newChar)
                        End If
                    End If
                End If
            End If
        Next

        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Check for char assets
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 2)

                ' Check for corp assets
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 2)

                ' Check for char balances
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 3)

                ' Check for corp balances
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 3)

                ' Check for char jobs
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryChar, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 4)

                ' Check for corp jobs
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryCorp, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 4)

                ' Check for char journal
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, pilotAccount, selPilot.ID, 1000, "", True)
                Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 5)

                ' Check for corp journal
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp, pilotAccount, selPilot.ID, 1000, "", True)
                Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 5)

                ' Check for char orders
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersChar, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 6)

                ' Check for corp orders
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersCorp, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 6)

                ' Check for char transactions
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransChar, pilotAccount, selPilot.ID, "", True)
                Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 7)

                ' Check for corp transactions
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransCorp, pilotAccount, selPilot.ID, "", True)
                Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 7)

                ' Check for corp sheets
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpSheet, pilotAccount, selPilot.ID, True)
                Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 8)

            End If
        Next
        lvwCurrentAPIs.EndUpdate()
        lvwCharFilter.EndUpdate()
    End Sub

    Private Sub CheckXML(ByVal apiXML As XmlDocument, ByVal Owner As String, ByVal Primary As String, ByVal Pos As Integer)
        Dim APIOwner As ListViewItem = lvwCurrentAPIs.Items(Owner)
        If APIOwner IsNot Nothing Then
            If apiXML IsNot Nothing Then
                ' Get the corp reps
                Dim CorpRep As SortedList = CType(CorpReps(Pos - 2), Collections.SortedList)
                ' If we already have a corp rep, no sense in getting a new one!
                If CorpRep.ContainsKey(Owner) = False Then
                    ' Check response string for any error codes?
                    Dim errlist As XmlNodeList = apiXML.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                        Dim errMsg As String = errNode.InnerText
                        APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                        APIOwner.SubItems(Pos).Text = errCode
                    Else
                        Dim cache As Date = CacheDate(apiXML)
                        If cache <= Now Then
                            APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Blue
                        Else
                            APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Green
                        End If
                        APIOwner.SubItems(Pos).Text = Format(cache, "dd/MM/yyyy HH:mm:ss")
                        APIOwner.ToolTipText = "Representative: " & Primary
                        CorpRep.Add(Owner, Primary)
                    End If
                End If
            Else
                APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                APIOwner.SubItems(Pos).Text = "missing"
            End If
        End If
    End Sub

#End Region

#Region "XML Retrieval and Parsing"
    Private Sub GetXMLData()
        ' Start separate threads for getting each collection of assets
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCharAssets, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpAssets, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCharBalances, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpBalances, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCharJobs, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpJobs, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCharJournal, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpJournal, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCharOrders, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpOrders, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCharTransactions, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpTranactions, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpSheet, Nothing)
    End Sub
    Private Sub GetCharAssets(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 2})

            End If
        Next
    End Sub
    Private Sub GetCorpAssets(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 2})

            End If
        Next
    End Sub
    Private Sub GetCharBalances(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 3})

            End If
        Next
    End Sub
    Private Sub GetCorpBalances(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 3})

            End If
        Next
    End Sub
    Private Sub GetCharJobs(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryChar, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 4})

            End If
        Next
    End Sub
    Private Sub GetCorpJobs(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryCorp, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 4})

            End If
        Next
    End Sub
    Private Sub GetCharJournal(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, pilotAccount, selPilot.ID, 1000, "", False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 5})

            End If
        Next
    End Sub
    Private Sub GetCorpJournal(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp, pilotAccount, selPilot.ID, 1000, "", False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 5})

            End If
        Next
    End Sub
    Private Sub GetCharOrders(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersChar, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 6})

            End If
        Next
    End Sub
    Private Sub GetCorpOrders(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersCorp, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 6})

            End If
        Next
    End Sub
    Private Sub GetCharTransactions(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransChar, pilotAccount, selPilot.ID, "", False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 7})

            End If
        Next
    End Sub
    Private Sub GetCorpTranactions(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransCorp, pilotAccount, selPilot.ID, "", False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 7})

            End If
        Next
    End Sub
    Private Sub GetCorpSheet(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim apiXML As New XmlDocument
                apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpSheet, pilotAccount, selPilot.ID, False)

                ' Update the dipsplay
                Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 8})

            End If
        Next
    End Sub

    Private Sub GetAssets()
        lvwCurrentAPIs.Items.Clear()
        lvwCharFilter.BeginUpdate()
        lvwCharFilter.Items.Clear()
        loadedOwners.Clear()
        cboPilots.Items.Clear()
        Call Me.GetCharacterAssets()
        Call Me.GetCorporateAssets()
        Call Me.GetCorporateSheet()
        Call Me.GetCharIsk()
        Call Me.GetCorpIsk()
        lvwCharFilter.EndUpdate()
        MessageBox.Show("Download of Assets complete. Please check the API Status for any error messages.", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub GetCharacterAssets()
        ' Get Individual Assets
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim assetXML As New XmlDocument
                assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, False)

                ' Setup the Assets table for a response
                Dim newAsset As New ListViewItem
                newAsset.Text = selPilot.Name
                newAsset.Name = "CharAssets_" & selPilot.Name
                newAsset.SubItems.Add("Char Assets")
                newAsset.Group = lvwCharFilter.Groups("grpPersonal")

                ' Check response string for any error codes?
                If assetXML IsNot Nothing Then
                    Dim errlist As XmlNodeList = assetXML.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                        Dim errMsg As String = errNode.InnerText
                        newAsset.ForeColor = Drawing.Color.Red
                        newAsset.SubItems.Add(errMsg)
                        newAsset.SubItems.Add(Format(CacheDate(assetXML), "dd/MM/yyyy HH:MM:ss"))
                    Else
                        newAsset.ForeColor = Drawing.Color.Green
                        newAsset.SubItems.Add("Loaded")
                        newAsset.SubItems.Add(Format(CacheDate(assetXML), "dd/MM/yyyy HH:MM:ss"))
                        loadedOwners.Add(selPilot.Name, selPilot)
                        cboPilots.Items.Add(selPilot.Name)
                        Dim newChar As New ListViewItem(selPilot.Name, lvwCharFilter.Groups.Item("grpPersonal"))
                        lvwCharFilter.Items.Add(newChar)
                    End If
                Else
                    newAsset.ForeColor = Drawing.Color.Red
                    newAsset.SubItems.Add("Could not reach API Server (no cached file)")
                    newAsset.SubItems.Add("n/a")
                End If
                lvwCurrentAPIs.Items.Add(newAsset)
                lvwCurrentAPIs.Refresh()
            End If
        Next
    End Sub
    Private Sub GetCorporateAssets()
        ' Get Corp Assets
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the assets
                Dim assetXML As New XmlDocument
                assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, False)

                ' Setup the Assets table for a response
                Dim newAsset As New ListViewItem
                newAsset.Text = selPilot.Corp & " (" & selPilot.Name & ")"
                newAsset.Name = "CorpAssets_" & selPilot.Corp
                newAsset.SubItems.Add("Corp Assets")
                newAsset.Group = lvwCharFilter.Groups("grpCorporation")

                ' Check response string for any error codes?
                If assetXML IsNot Nothing Then
                    Dim errlist As XmlNodeList = assetXML.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                        Dim errMsg As String = errNode.InnerText
                        newAsset.ForeColor = Drawing.Color.Red
                        newAsset.SubItems.Add(errMsg)
                        newAsset.SubItems.Add(Format(CacheDate(assetXML), "dd/MM/yyyy HH:MM:ss"))
                    Else
                        newAsset.ForeColor = Drawing.Color.Green
                        newAsset.SubItems.Add("Loaded")
                        newAsset.SubItems.Add(Format(CacheDate(assetXML), "dd/MM/yyyy HH:MM:ss"))
                        Dim newChar As New ListViewItem(selPilot.Corp, lvwCharFilter.Groups.Item("grpCorporation"))
                        If loadedOwners.Contains(selPilot.Corp) = False Then
                            loadedOwners.Add(selPilot.Corp, selPilot)
                            cboPilots.Items.Add(selPilot.Corp)
                            lvwCharFilter.Items.Add(newChar)
                        End If
                    End If
                Else
                    newAsset.ForeColor = Drawing.Color.Red
                    newAsset.SubItems.Add("Could not reach API Server (no cached file)")
                    newAsset.SubItems.Add("n/a")
                End If
                lvwCurrentAPIs.Items.Add(newAsset)
                lvwCurrentAPIs.Refresh()
            End If
        Next
    End Sub
    Private Sub GetCorporateSheet()
        ' Get Corp Sheet For Account/Hangar Divisions
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the corp sheet
                Dim corpXML As New XmlDocument
                corpXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpSheet, pilotAccount, selPilot.ID, False)

                ' Setup the Assets table for a response
                Dim newAPI As New ListViewItem
                newAPI.Text = selPilot.Corp & " (" & selPilot.Name & ")"
                newAPI.Name = "CorpSheet_" & selPilot.Corp
                newAPI.SubItems.Add("Corp Sheet")
                newAPI.Group = lvwCharFilter.Groups("grpCorporation")

                ' Check response string for any error codes?
                If corpXML IsNot Nothing Then
                    Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                        Dim errMsg As String = errNode.InnerText
                        newAPI.ForeColor = Drawing.Color.Red
                        newAPI.SubItems.Add(errMsg)
                        newAPI.SubItems.Add(Format(CacheDate(corpXML), "dd/MM/yyyy HH:MM:ss"))
                    Else
                        newAPI.ForeColor = Drawing.Color.Green
                        newAPI.SubItems.Add("Loaded")
                        newAPI.SubItems.Add(Format(CacheDate(corpXML), "dd/MM/yyyy HH:MM:ss"))
                    End If
                Else
                    newAPI.ForeColor = Drawing.Color.Red
                    newAPI.SubItems.Add("Could not reach API Server (no cached file)")
                    newAPI.SubItems.Add("n/a")
                End If
                lvwCurrentAPIs.Items.Add(newAPI)
                lvwCurrentAPIs.Refresh()
            End If
        Next
    End Sub
    Private Sub GetCharIsk()
        ' Get Corp Sheet For Account/Hangar Divisions
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the corp sheet
                Dim charXML As New XmlDocument
                charXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, pilotAccount, selPilot.ID, False)

                ' Setup the Assets table for a response
                Dim newAPI As New ListViewItem
                newAPI.Text = selPilot.Name
                newAPI.Name = "CharBalances_" & selPilot.Name
                newAPI.SubItems.Add("Char Balances")
                newAPI.Group = lvwCharFilter.Groups("grpCorporation")

                ' Check response string for any error codes?
                If charXML IsNot Nothing Then
                    Dim errlist As XmlNodeList = charXML.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                        Dim errMsg As String = errNode.InnerText
                        newAPI.ForeColor = Drawing.Color.Red
                        newAPI.SubItems.Add(errMsg)
                        newAPI.SubItems.Add(Format(CacheDate(charXML), "dd/MM/yyyy HH:MM:ss"))
                    Else
                        newAPI.ForeColor = Drawing.Color.Green
                        newAPI.SubItems.Add("Loaded")
                        newAPI.SubItems.Add(Format(CacheDate(charXML), "dd/MM/yyyy HH:MM:ss"))
                    End If
                Else
                    newAPI.ForeColor = Drawing.Color.Red
                    newAPI.SubItems.Add("Could not reach API Server (no cached file)")
                    newAPI.SubItems.Add("n/a")
                End If
                lvwCurrentAPIs.Items.Add(newAPI)
                lvwCurrentAPIs.Refresh()
            End If
        Next
    End Sub
    Private Sub GetCorpIsk()
        ' Get Corp Sheet For Account/Hangar Divisions
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Make a call to the EveHQ.Core.API to fetch the corp sheet
                Dim corpXML As New XmlDocument
                corpXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, pilotAccount, selPilot.ID, False)

                ' Setup the Assets table for a response
                Dim newAPI As New ListViewItem
                newAPI.Text = selPilot.Corp & " (" & selPilot.Name & ")"
                newAPI.Name = "CorpBalances_" & selPilot.Corp
                newAPI.SubItems.Add("Corp Balances")
                newAPI.Group = lvwCharFilter.Groups("grpCorporation")

                ' Check response string for any error codes?
                If corpXML IsNot Nothing Then
                    Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                        Dim errMsg As String = errNode.InnerText
                        newAPI.ForeColor = Drawing.Color.Red
                        newAPI.SubItems.Add(errMsg)
                        newAPI.SubItems.Add(Format(CacheDate(corpXML), "dd/MM/yyyy HH:MM:ss"))
                    Else
                        newAPI.ForeColor = Drawing.Color.Green
                        newAPI.SubItems.Add("Loaded")
                        newAPI.SubItems.Add(Format(CacheDate(corpXML), "dd/MM/yyyy HH:MM:ss"))
                    End If
                Else
                    newAPI.ForeColor = Drawing.Color.Red
                    newAPI.SubItems.Add("Could not reach API Server (no cached file)")
                    newAPI.SubItems.Add("n/a")
                End If
                lvwCurrentAPIs.Items.Add(newAPI)
                lvwCurrentAPIs.Refresh()
            End If
        Next
    End Sub
    Private Function CacheDate(ByVal APIXML As XmlDocument) As DateTime
        ' Get Cache time details
        Dim cacheDetails As XmlNodeList = APIXML.SelectNodes("/eveapi")
        Dim cacheTime As DateTime = CDate(cacheDetails(0).ChildNodes(2).InnerText)
        Dim localCacheTime As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cacheTime)
        Return localCacheTime
    End Function

#End Region

#Region "Assets XML Parsing"
    Private Sub PopulateAssets()
        assetList.Clear()
        tlvAssets.BeginUpdate()
        tlvAssets.Items.Clear()
        totalAssetValue = 0
        totalAssetCount = 0
        ' Get the details of corp accounts
        Call Me.ParseCorpSheets()
        If chkExcludeItems.Checked = False Then
            Call Me.PopulateAssetTree()
            If filters.Count > 0 Or searchText <> "" Then
                Call Me.FilterTree()
            End If
            ' Check for minimum system value
            If chkMinSystemValue.Checked = True Then
                Call Me.FilterSystemValue()
            End If
        End If
        If chkExcludeCash.Checked = False Then
            Call Me.DisplayISKAssets()
        End If
        If chkExcludeInvestments.Checked = False Then
            Call Me.DisplayInvestments()
        End If
        tssLabelTotalAssets.Text = FormatNumber(totalAssetValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ISK  (" & FormatNumber(totalAssetCount, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " total quantity)"
        tlvAssets.Sort(0, System.Windows.Forms.SortOrder.Ascending, True)
        tlvAssets.EndUpdate()
    End Sub
    Private Sub ParseCorpSheets()
        Dim fileName As String = ""
        Dim corpXML As New XmlDocument
        ' Reset the lists of divisions and wallets
        divisions.Clear()
        walletDivisions.Clear()
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            Dim selPilot As EveHQ.Core.Pilot = CType(loadedOwners(cPilot.Text), Core.Pilot)
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Check for corp sheets
                fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_19_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                If My.Computer.FileSystem.FileExists(fileName) = True Then
                    corpXML.Load(fileName)
                    ' Check response string for any error codes?
                    Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                    If errlist.Count = 0 Then
                        ' No errors so parse the files
                        Dim divList As XmlNodeList
                        Dim div As XmlNode
                        divList = corpXML.SelectNodes("/eveapi/result/rowset")
                        For Each div In divList
                            Select Case div.Attributes.GetNamedItem("name").Value
                                Case "divisions"
                                    For Each divName As XmlNode In div.ChildNodes
                                        If divisions.ContainsKey(selPilot.CorpID & "_" & divName.Attributes.GetNamedItem("accountKey").Value) = False Then
                                            divisions.Add(selPilot.CorpID & "_" & divName.Attributes.GetNamedItem("accountKey").Value, StrConv(divName.Attributes.GetNamedItem("description").Value, VbStrConv.ProperCase))
                                        End If
                                    Next
                                Case "walletDivisions"
                                    For Each divName As XmlNode In div.ChildNodes
                                        If walletDivisions.ContainsKey(selPilot.CorpID & "_" & divName.Attributes.GetNamedItem("accountKey").Value) = False Then
                                            walletDivisions.Add(selPilot.CorpID & "_" & divName.Attributes.GetNamedItem("accountKey").Value, divName.Attributes.GetNamedItem("description").Value)
                                        End If
                                    Next
                            End Select
                        Next
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub PopulateAssetTree()

        Dim assetOwner As String = ""
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            ' Check in the cache folder for a valid file
            'Dim selPilot As EveHQ.Core.Pilot = CType(loadedOwners(cboPilots.SelectedItem), Core.Pilot)
            Dim selPilot As EveHQ.Core.Pilot = CType(loadedOwners(cPilot.Text), Core.Pilot)
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                Dim fileName As String = ""
                Dim processFile As Boolean = True
                If cPilot.Text = selPilot.Corp Then
                    fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_15_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                    assetOwner = selPilot.Corp
                    If PlugInData.NPCCorps.Contains(selPilot.CorpID) = True Then processFile = False
                Else
                    fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_14_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                    assetOwner = selPilot.Name
                End If
                If processFile = True Then
                    If My.Computer.FileSystem.FileExists(fileName) = False Then
                        MessageBox.Show("Unable to load assets file for " & selPilot.Name & ".", "Error Loading Assets", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                    ' File found so lets see what we have!
                    Dim assetXML As New XmlDocument
                    assetXML.Load(fileName)
                    Dim locList As XmlNodeList
                    Dim loc As XmlNode
                    locList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                    If locList.Count > 0 Then
                        Dim linePrice As Double = 0
                        Dim containerPrice As Double = 0
                        For Each loc In locList
                            ' Check if the location is already listed
                            Dim locNode As New ContainerListViewItem
                            Dim addLocation As Boolean = True
                            For Each testNode As ContainerListViewItem In tlvAssets.Items
                                If testNode.Tag.ToString = (loc.Attributes.GetNamedItem("locationID").Value) Then
                                    locNode = testNode
                                    addLocation = False
                                    Exit For
                                End If
                            Next
                            If addLocation = True Then
                                Dim locID As String = loc.Attributes.GetNamedItem("locationID").Value
                                If CDbl(locID) >= 66000000 Then
                                    If CDbl(locID) < 66014933 Then
                                        locID = (CDbl(locID) - 6000001).ToString
                                    Else
                                        locID = (CDbl(locID) - 6000000).ToString
                                    End If
                                End If
                                Dim newLocation As Prism.Station
                                If CDbl(locID) >= 61000000 And CDbl(locID) <= 61999999 Then
                                    If PlugInData.stations.Contains(locID) = True Then
                                        ' Known Outpost
                                        newLocation = CType(PlugInData.stations(locID), Prism.Station)
                                        locNode.Text = newLocation.stationName
                                        locNode.Tag = newLocation.stationID
                                    Else
                                        ' Unknown outpost!
                                        newLocation = New Prism.Station
                                        newLocation.stationID = CLng(locID)
                                        newLocation.stationName = "Unknown Outpost"
                                        newLocation.systemID = 0
                                        newLocation.constID = 0
                                        newLocation.regionID = 0
                                        locNode.Text = newLocation.stationName
                                        locNode.Tag = newLocation.stationID
                                    End If
                                Else
                                    If CDbl(locID) < 60000000 Then
                                        Dim newSystem As SolarSystem = CType(PlugInData.stations(locID), SolarSystem)
                                        locNode.Text = newSystem.Name
                                        locNode.Tag = newSystem.ID
                                    Else
                                        newLocation = CType(PlugInData.stations(locID), Prism.Station)
                                        If newLocation IsNot Nothing Then
                                            locNode.Text = newLocation.stationName
                                            locNode.Tag = newLocation.stationID
                                        Else
                                            ' Unknown system/station!
                                            newLocation = New Prism.Station
                                            newLocation.stationID = CLng(locID)
                                            newLocation.stationName = "Unknown Location"
                                            newLocation.systemID = 0
                                            newLocation.constID = 0
                                            newLocation.regionID = 0
                                            locNode.Text = newLocation.stationName
                                            locNode.Tag = newLocation.stationID
                                        End If
                                    End If
                                End If
                                tlvAssets.Items.Add(locNode)
                            End If

                            Dim itemID As String = loc.Attributes.GetNamedItem("typeID").Value
                            Dim itemIDX As Integer = EveHQ.Core.HQ.itemList.IndexOfValue(itemID)
                            Dim itemName As String = ""
                            Dim groupID As String = ""
                            Dim catID As String = ""
                            Dim groupIDX As Integer = 0
                            Dim groupName As String = ""
                            Dim catIDX As Integer = 0
                            Dim catName As String = ""
                            Dim metaLevel As String = ""
                            Dim volume As String = ""
                            If itemIDX <> -1 Then
                                itemName = CStr(EveHQ.Core.HQ.itemList.GetKey(itemIDX))
                                groupID = EveHQ.Core.HQ.typeGroups(itemID).ToString
                                catID = EveHQ.Core.HQ.groupCats(groupID).ToString
                                groupIDX = EveHQ.Core.HQ.groupList.IndexOfValue(groupID)
                                groupName = EveHQ.Core.HQ.groupList.GetKey(groupIDX).ToString
                                catIDX = EveHQ.Core.HQ.catList.IndexOfValue(catID)
                                catName = EveHQ.Core.HQ.catList.GetKey(catIDX).ToString
                                metaLevel = CType(PlugInData.Items(itemID), ItemData).MetaLevel.ToString
                                If PlugInData.PackedVolumes.Contains(groupID) = True Then
                                    If loc.Attributes.GetNamedItem("singleton").Value = "0" Then
                                        volume = FormatNumber(PlugInData.PackedVolumes(groupID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    Else
                                        volume = FormatNumber(CType(PlugInData.Items(itemID), ItemData).Volume * CDbl(loc.Attributes.GetNamedItem("quantity").Value), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    End If
                                Else
                                    volume = FormatNumber(CType(PlugInData.Items(itemID), ItemData).Volume * CDbl(loc.Attributes.GetNamedItem("quantity").Value), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                End If
                            Else
                                ' Can't find the item in the database
                                itemName = "ItemID: " & itemID.ToString
                                groupID = "unknown"
                                catID = "unknown"
                                groupIDX = -1
                                groupName = "Unknown"
                                catIDX = -1
                                catName = "Unknown"
                                metaLevel = "0"
                                volume = "0"
                            End If

                            Dim newAsset As New ContainerListViewItem
                            newAsset.Tag = loc.Attributes.GetNamedItem("itemID").Value
                            newAsset.Text = itemName
                            ' Add the asset to the treelistview
                            locNode.Items.Add(newAsset)

                            newAsset.SubItems(1).Text = assetOwner
                            newAsset.SubItems(2).Text = groupName
                            newAsset.SubItems(3).Text = catName
                            Dim flagID As Integer = CInt(loc.Attributes.GetNamedItem("flag").Value)
                            Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                            If assetOwner = selPilot.Corp And newAsset.SubItems(2).Text <> "Station Services" Then
                                Dim accountID As Integer = flagID + 885
                                If accountID = 889 Then accountID = 1000
                                If divisions.ContainsKey(selPilot.CorpID & "_" & accountID.ToString) = True Then
                                    flagName = CStr(divisions.Item(selPilot.CorpID & "_" & accountID.ToString))
                                End If
                            End If
                            newAsset.SubItems(4).Text = flagName
                            newAsset.SubItems(5).Text = metaLevel
                            newAsset.SubItems(6).Text = volume
                            newAsset.SubItems(7).Text = FormatNumber(loc.Attributes.GetNamedItem("quantity").Value, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            If newAsset.Text.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
                                newAsset.SubItems(8).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                linePrice = 0
                            Else
                                newAsset.SubItems(8).Text = FormatNumber(Math.Round(EveHQ.Core.DataFunctions.GetPrice(itemID), 2), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                If IsNumeric(newAsset.SubItems(8).Text) = True Then
                                    linePrice = CDbl(newAsset.SubItems(7).Text) * CDbl(newAsset.SubItems(8).Text)
                                Else
                                    linePrice = 0
                                End If
                            End If
                            newAsset.SubItems(9).Text = FormatNumber(linePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

                            ' Add the asset to the list of assets
                            Dim newAssetList As New AssetItem
                            newAssetList.itemID = newAsset.Tag.ToString
                            newAssetList.system = locNode.Text
                            newAssetList.typeID = itemID
                            newAssetList.typeName = itemName
                            newAssetList.owner = assetOwner
                            newAssetList.group = groupName
                            newAssetList.category = catName
                            newAssetList.location = flagName
                            newAssetList.quantity = CLng(loc.Attributes.GetNamedItem("quantity").Value)
                            newAssetList.price = CDbl(newAsset.SubItems(8).Text)
                            totalAssetCount += newAssetList.quantity
                            assetList.Add(newAssetList.itemID, newAssetList)

                            ' Check if this row has child nodes and repeat
                            If loc.HasChildNodes = True Then
                                Call Me.PopulateAssetNode(newAsset, loc, assetOwner, locNode.Text, selPilot)
                            End If
                        Next
                    End If
                End If
            End If
        Next
        ' Get the locations and total prices
        Dim locationPrice As Double = 0
        Dim cLoc As ContainerListViewItem
        Dim cL As Integer = 0
        If tlvAssets.Items.Count > 0 Then
            Do
                cLoc = tlvAssets.Items(cL)
                locationPrice = 0
                For Each cLine As ContainerListViewItem In cLoc.Items
                    locationPrice += CDbl(cLine.SubItems(9).Text)
                Next
                totalAssetValue += locationPrice
                cLoc.SubItems(9).Text = FormatNumber(locationPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ' Delete if no child nodes at the locations
                If cLoc.Items.Count = 0 Then
                    tlvAssets.Items.Remove(cLoc)
                    cL -= 1
                End If
                cL += 1
            Loop Until cL = tlvAssets.Items.Count
        End If
    End Sub
    Private Function PopulateAssetNode(ByVal parentAsset As ContainerListViewItem, ByVal loc As XmlNode, ByVal assetOwner As String, ByVal location As String, ByVal selPilot As EveHQ.Core.Pilot) As Double
        Dim subLocList As XmlNodeList
        Dim subLoc As XmlNode
        Dim containerPrice As Double = 0
        Dim linePrice As Double = 0
        subLocList = loc.ChildNodes(0).ChildNodes
        If IsNumeric(parentAsset.SubItems(8).Text) = True Then
            containerPrice = CDbl(parentAsset.SubItems(8).Text)
        Else
            containerPrice = 0
            parentAsset.SubItems(8).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        End If
        For Each subLoc In subLocList
            Try
                Dim ItemID As String = subLoc.Attributes.GetNamedItem("typeID").Value
                Dim ItemIDX As Integer = EveHQ.Core.HQ.itemList.IndexOfValue(ItemID)
                Dim itemName As String = ""
                Dim groupID As String = ""
                Dim catID As String = ""
                Dim groupIDX As Integer = 0
                Dim groupName As String = ""
                Dim catIDX As Integer = 0
                Dim catName As String = ""
                Dim metaLevel As String = ""
                Dim volume As String = ""
                If ItemIDX <> -1 Then
                    itemName = CStr(EveHQ.Core.HQ.itemList.GetKey(ItemIDX))
                    groupID = EveHQ.Core.HQ.typeGroups(ItemID).ToString
                    catID = EveHQ.Core.HQ.groupCats(groupID).ToString
                    groupIDX = EveHQ.Core.HQ.groupList.IndexOfValue(groupID)
                    groupName = EveHQ.Core.HQ.groupList.GetKey(groupIDX).ToString
                    catIDX = EveHQ.Core.HQ.catList.IndexOfValue(catID)
                    catName = EveHQ.Core.HQ.catList.GetKey(catIDX).ToString
                    metaLevel = CType(PlugInData.Items(ItemID), ItemData).MetaLevel.ToString
                    If PlugInData.PackedVolumes.Contains(groupID) = True Then
                        If loc.Attributes.GetNamedItem("singleton").Value = "0" Then
                            volume = FormatNumber(PlugInData.PackedVolumes(groupID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        Else
                            volume = FormatNumber(CType(PlugInData.Items(ItemID), ItemData).Volume * CDbl(subLoc.Attributes.GetNamedItem("quantity").Value), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        End If
                    Else
                        volume = FormatNumber(CType(PlugInData.Items(ItemID), ItemData).Volume * CDbl(subLoc.Attributes.GetNamedItem("quantity").Value), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    End If
                Else
                    ' Can't find the item in the database
                    itemName = "ItemID: " & ItemID.ToString
                    groupID = "unknown"
                    catID = "unknown"
                    groupIDX = -1
                    groupName = "Unknown"
                    catIDX = -1
                    catName = "Unknown"
                    metaLevel = "0"
                    volume = "0"
                End If

                Dim subAsset As New ContainerListViewItem
                subAsset.Tag = subLoc.Attributes.GetNamedItem("itemID").Value
                parentAsset.Items.Add(subAsset)
                subAsset.Text = itemName
                subAsset.SubItems(1).Text = assetOwner
                subAsset.SubItems(2).Text = groupName
                subAsset.SubItems(3).Text = catName
                Dim subFlagID As Integer = CInt(subLoc.Attributes.GetNamedItem("flag").Value)
                Dim subFlagName As String = PlugInData.itemFlags(subFlagID).ToString
                If assetOwner = selPilot.Corp And subAsset.SubItems(2).Text <> "Station Services" Then
                    Dim accountID As Integer = subFlagID + 885
                    If accountID = 889 Then accountID = 1000
                    If divisions.ContainsKey(selPilot.CorpID & "_" & accountID.ToString) = True Then
                        subFlagName = CStr(divisions.Item(selPilot.CorpID & "_" & accountID.ToString))
                    End If
                End If
                subAsset.SubItems(4).Text = subFlagName
                subAsset.SubItems(5).Text = metaLevel
                subAsset.SubItems(6).Text = volume
                subAsset.SubItems(7).Text = FormatNumber(subLoc.Attributes.GetNamedItem("quantity").Value, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                If subAsset.Text.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
                    subAsset.SubItems(8).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    linePrice = 0
                Else
                    subAsset.SubItems(8).Text = FormatNumber(Math.Round(EveHQ.Core.DataFunctions.GetPrice(ItemID), 2), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    If IsNumeric(subAsset.SubItems(8).Text) = True Then
                        linePrice = CDbl(subAsset.SubItems(7).Text) * CDbl(subAsset.SubItems(8).Text)
                    Else
                        linePrice = 0
                        subAsset.SubItems(8).Text = FormatNumber(linePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    End If
                End If
                containerPrice += linePrice
                subAsset.SubItems(9).Text = FormatNumber(linePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

                ' Add the asset to the list of assets
                Dim newAssetList As New AssetItem
                newAssetList.itemID = subAsset.Tag.ToString
                newAssetList.system = location
                newAssetList.typeID = ItemID
                newAssetList.typeName = itemName
                newAssetList.owner = assetOwner
                newAssetList.group = groupName
                newAssetList.category = catName
                newAssetList.location = parentAsset.Text & ": " & subFlagName
                newAssetList.quantity = CLng(subLoc.Attributes.GetNamedItem("quantity").Value)
                newAssetList.price = CDbl(subAsset.SubItems(8).Text)
                assetList.Add(newAssetList.itemID, newAssetList)
                totalAssetCount += newAssetList.quantity
                If subLoc.HasChildNodes = True Then
                    containerPrice -= linePrice
                    containerPrice += PopulateAssetNode(subAsset, subLoc, assetOwner, location, selPilot)
                End If
            Catch ex As Exception
                Dim msg As String = "Unable to parse Asset:" & ControlChars.CrLf
                msg &= "InnerXML: " & subLoc.InnerXml & ControlChars.CrLf
                msg &= "InnerText: " & subLoc.InnerText & ControlChars.CrLf
                msg &= "TypeID: " & subLoc.Attributes.GetNamedItem("typeID").Value
                MessageBox.Show(msg, "Error Parsing Assets File For " & assetOwner, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        Next
        parentAsset.SubItems(9).Text = FormatNumber(containerPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Return containerPrice
    End Function
    Private Sub DisplayISKAssets()
        Dim fileName As String = ""
        Dim corpXML As New XmlDocument
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

        ' Reset and parse the character wallets
        charWallets.Clear()
        corpWallets.Clear()
        corpWalletDivisions.Clear()
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            Dim selPilot As EveHQ.Core.Pilot = CType(loadedOwners(cPilot.Text), Core.Pilot)
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Check for corp wallets
                If cPilot.Text = selPilot.Corp Then
                    fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_12_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                    If My.Computer.FileSystem.FileExists(fileName) = True Then
                        corpXML.Load(fileName)
                        ' Check response string for any error codes?
                        Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                        If errlist.Count = 0 Then
                            ' No errors so parse the files
                            Dim accountList As XmlNodeList
                            Dim account As XmlNode
                            If corpWallets.Contains(selPilot.Corp) = False Then
                                corpWallets.Add(selPilot.Corp, selPilot.CorpID)
                                accountList = corpXML.SelectNodes("/eveapi/result/rowset/row")
                                For Each account In accountList
                                    Dim isk As Double = Double.Parse(account.Attributes.GetNamedItem("balance").Value, Globalization.NumberStyles.Number, culture)
                                    Dim accountKey As String = account.Attributes.GetNamedItem("accountKey").Value
                                    If corpWalletDivisions.ContainsKey(selPilot.CorpID & "_" & accountKey) = False Then
                                        corpWalletDivisions.Add(selPilot.CorpID & "_" & accountKey, isk)
                                    End If
                                Next
                            End If
                        End If
                    End If
                Else
                    ' Check for char wallets
                    fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_11_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                    If My.Computer.FileSystem.FileExists(fileName) = True Then
                        corpXML.Load(fileName)
                        ' Check response string for any error codes?
                        Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                        If errlist.Count = 0 Then
                            ' No errors so parse the files
                            Dim accountList As XmlNodeList
                            Dim account As XmlNode
                            accountList = corpXML.SelectNodes("/eveapi/result/rowset/row")
                            For Each account In accountList
                                Dim isk As Double = Double.Parse(account.Attributes.GetNamedItem("balance").Value, Globalization.NumberStyles.Number, culture)
                                If charWallets.Contains(selPilot.Name) = False Then
                                    charWallets.Add(selPilot.Name, isk)
                                End If
                            Next
                        End If
                    End If
                End If
            End If
        Next

        ' Add the balances to the assets schedule
        Dim node As New ContainerListViewItem
        Dim totalCash As Double = 0
        node.Tag = "ISK"
        node.Text = "Cash Balances"
        tlvAssets.Items.Add(node)
        ' Add the personal balances
        If charWallets.Count > 0 Then
            Dim personalNode As New ContainerListViewItem
            personalNode.Tag = "Personal"
            personalNode.Text = "Personal"
            node.Items.Add(personalNode)
            Dim personalCash As Double = 0
            For Each pilot As String In charWallets.Keys
                Dim iskNode As New ContainerListViewItem
                iskNode.Tag = pilot
                iskNode.Text = pilot
                personalNode.Items.Add(iskNode)
                iskNode.SubItems(9).Text = FormatNumber(charWallets(pilot), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                personalCash += CDbl(charWallets(pilot))
            Next
            personalNode.SubItems(9).Text = FormatNumber(personalCash, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            totalCash += personalCash
        End If
        ' Add the corporate balances
        If corpWallets.Count > 0 Then
            Dim corporateNode As New ContainerListViewItem
            corporateNode.Tag = "Corporate"
            corporateNode.Text = "Corporate"
            node.Items.Add(corporateNode)
            Dim corporateCash As Double = 0
            For Each corpName As String In corpWallets.Keys
                Dim corpID As String = CStr(corpWallets(corpName))
                Dim corpNode As New ContainerListViewItem
                corpNode.Tag = corpName
                corpNode.Text = corpName
                corporateNode.Items.Add(corpNode)
                Dim divisionCash As Double = 0
                For key As Integer = 1000 To 1006
                    Dim iskNode As New ContainerListViewItem
                    Dim idx As String = corpID & "_" & key.ToString
                    iskNode.Tag = walletDivisions(idx).ToString
                    iskNode.Text = CStr(walletDivisions(idx))
                    corpNode.Items.Add(iskNode)
                    iskNode.SubItems(9).Text = FormatNumber(corpWalletDivisions(idx), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    divisionCash += CDbl(corpWalletDivisions(idx))
                Next
                corporateCash += divisionCash
                corpNode.SubItems(9).Text = FormatNumber(divisionCash, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Next
            corporateNode.SubItems(9).Text = FormatNumber(corporateCash, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            totalCash += corporateCash
        End If
        node.SubItems(9).Text = FormatNumber(totalCash, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        totalAssetValue += totalCash
    End Sub
    Private Sub DisplayInvestments()
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

        ' Check for owners with investments
        Dim invOwners As New ArrayList
        For Each inv As Investment In Portfolio.Investments.Values
            If invOwners.Contains(inv.Owner) = False Then
                invOwners.Add(inv.Owner)
            End If
        Next

        Dim investNode As New ContainerListViewItem
        Dim totalValue As Double = 0
        investNode.Tag = "Investments"
        investNode.Text = "Investments"
        tlvAssets.Items.Add(investNode)

        ' Check and list investments
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            Dim ownerValue As Double = 0
            If invOwners.Contains(cPilot.Text) = True Then
                Dim ownerNode As New ContainerListViewItem
                ownerNode.Tag = cPilot.Text
                ownerNode.Text = cPilot.Text
                investNode.Items.Add(ownerNode)
                For Each inv As Investment In Portfolio.Investments.Values
                    If inv.Owner = cPilot.Text Then
                        Dim invNode As New ContainerListViewItem
                        invNode.Tag = inv.ID
                        invNode.Text = inv.Name
                        ownerNode.Items.Add(invNode)
                        Select Case inv.Type
                            Case InvestmentType.Cash
                                invNode.SubItems(3).Text = "Cash"
                                If inv.ValueIsCost = True Then
                                    invNode.SubItems(9).Text = FormatNumber(inv.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    ownerValue += inv.CurrentCost
                                Else
                                    invNode.SubItems(9).Text = FormatNumber(inv.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    ownerValue += inv.CurrentValue
                                End If
                            Case InvestmentType.Shares
                                invNode.SubItems(3).Text = "Shares"
                                invNode.SubItems(7).Text = FormatNumber(inv.CurrentQuantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                If inv.ValueIsCost = True Then
                                    invNode.SubItems(8).Text = FormatNumber(inv.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    invNode.SubItems(9).Text = FormatNumber(inv.CurrentQuantity * inv.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    ownerValue += (inv.CurrentQuantity * inv.CurrentCost)
                                Else
                                    invNode.SubItems(8).Text = FormatNumber(inv.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    invNode.SubItems(9).Text = FormatNumber(inv.CurrentQuantity * inv.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    ownerValue += (inv.CurrentQuantity * inv.CurrentValue)
                                End If
                        End Select
                    End If
                Next
                ownerNode.SubItems(9).Text = FormatNumber(ownerValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                totalValue += ownerValue
            End If
        Next
        investNode.SubItems(9).Text = FormatNumber(totalValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        totalAssetValue += totalValue
    End Sub
#End Region

#Region "Outpost XML Retrieval and Parsing"

    Private Sub GetOutposts()

        ' Make a call to the EveHQ.Core.API to fetch the assets
        Dim stationXML As New XmlDocument
        stationXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Conquerables, False)

        ' Check response string for any error codes?
        Dim errlist As XmlNodeList = stationXML.SelectNodes("/eveapi/error")
        If errlist.Count <> 0 Then
            Dim errNode As XmlNode = errlist(0)
            ' Get error code
            Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
            Dim errMsg As String = errNode.InnerText
            Dim msg As String = "The EveAPI returned an error while trying to download the Conquerable Station XML." & ControlChars.CrLf
            msg &= "The error was: '" & errMsg & "' (code: " & errCode & ")"
            MessageBox.Show(msg, "EveAPI Error - Code " & errCode, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ' XML file looks OK so let's parse it
            Call PlugInData.ParseConquerableXML(stationXML)
            MessageBox.Show("Conquerable Station XML successfully loaded!", "Conquerable Station XML Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

#End Region

#Region "Filter, Owner and Search Routines"
    Private Sub FilterTree()
        Dim cL As Integer = 0
        Dim cLoc As ContainerListViewItem
        Do
            cLoc = tlvAssets.Items(cL)
            If cLoc.Items.Count = 0 Then
                If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(3).Text) = False And groupFilters.Contains(cLoc.SubItems(2).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                    tlvAssets.Items.Remove(cLoc)
                    assetList.Remove(cLoc.Tag)
                    totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                    cL -= 1
                End If
            Else
                Call FilterNode(cLoc)
                If cLoc.Items.Count = 0 Then
                    If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(3).Text) = False And groupFilters.Contains(cLoc.SubItems(2).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        tlvAssets.Items.Remove(cLoc)
                        If IsNumeric(cLoc.SubItems(5).Text) = True Then
                            totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                        End If
                        'assetList.Remove(cLoc.Tag)
                        cL -= 1
                    End If
                Else
                    If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(3).Text) = False And groupFilters.Contains(cLoc.SubItems(2).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        If IsNumeric(cLoc.SubItems(5).Text) = True Then
                            totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                        End If
                        ' Remove quantity and price information
                        cLoc.SubItems(5).Text = ""
                        cLoc.SubItems(6).Text = ""
                        'assetList.Remove(cLoc.Tag)
                    End If
                End If
            End If
            cL += 1
        Loop Until (cL = tlvAssets.Items.Count)
        Call Me.CalcFilteredPrices()
    End Sub
    Private Sub FilterNode(ByVal pLoc As ContainerListViewItem)
        Dim cL As Integer = 0
        Dim cLoc As ContainerListViewItem
        Do
            cLoc = pLoc.Items(cL)
            If cLoc.Items.Count = 0 Then
                If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(3).Text) = False And groupFilters.Contains(cLoc.SubItems(2).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                    pLoc.Items.Remove(cLoc)
                    assetList.Remove(cLoc.Tag)
                    totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                    cL -= 1
                End If
            Else
                Call FilterNode(cLoc)
                If cLoc.Items.Count = 0 Then
                    If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(3).Text) = False And groupFilters.Contains(cLoc.SubItems(2).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        pLoc.Items.Remove(cLoc)
                        assetList.Remove(cLoc.Tag)
                        totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                        cL -= 1
                    End If
                Else
                    If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(3).Text) = False And groupFilters.Contains(cLoc.SubItems(2).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        If IsNumeric(cLoc.SubItems(5).Text) = True Then
                            totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                        End If
                        ' Remove quantity and price information
                        cLoc.SubItems(5).Text = ""
                        cLoc.SubItems(6).Text = ""
                        assetList.Remove(cLoc.Tag)
                    End If
                End If
            End If
            cL += 1
        Loop Until (cL = pLoc.Items.Count)
    End Sub
    Private Sub CalcFilteredPrices()
        totalAssetValue = 0
        Dim locPrice As Double = 0
        For Each cLoc As ContainerListViewItem In tlvAssets.Items
            ' Calculate cost of all the sub nodes
            If cLoc.Items.Count > 0 Then
                locPrice = Me.CalcNodePrice(cLoc)
                cLoc.SubItems(7).Text = FormatNumber(locPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                totalAssetValue += locPrice
            End If
        Next
    End Sub
    Private Function CalcNodePrice(ByVal pLoc As ContainerListViewItem) As Double
        Dim lineValue As Double = 0
        Dim contValue As Double = 0
        For Each cLoc As ContainerListViewItem In pLoc.Items
            If cLoc.Items.Count > 0 Then
                Call Me.CalcNodePrice(cLoc)
                lineValue = CDbl(cLoc.SubItems(7).Text)
                contValue += lineValue
            Else
                If IsNumeric(cLoc.SubItems(6).Text) = True Then
                    lineValue = CDbl(cLoc.SubItems(5).Text) * CDbl(cLoc.SubItems(6).Text)
                Else
                    lineValue = 0
                End If
                cLoc.SubItems(7).Text = FormatNumber(lineValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                contValue += lineValue
            End If
        Next
        pLoc.SubItems(7).Text = FormatNumber(contValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Return contValue
    End Function
    Private Sub AddFilter()
        Dim cNode As TreeNode = tvwFilter.SelectedNode
        If filters.Contains(cNode.FullPath) = False Then
            ' Add the filter to the list and display it
            filters.Add(cNode.FullPath)
            lstFilters.Items.Add(cNode.FullPath)
            ' Add to the category filter if a category
            If cNode.Nodes.Count > 0 Then
                catFilters.Add(cNode.Text)
            Else
                groupFilters.Add(cNode.Text)
            End If
        End If
        Call Me.SetGroupFilterLabel()
    End Sub
    Private Sub RemoveFilter()
        ' Check which we are removing
        If lstFilters.SelectedItem.ToString.Contains("\") = True Then
            ' Remove the group
            Dim sID As Integer = InStr(lstFilters.SelectedItem.ToString, "\")
            Dim groupName As String = lstFilters.SelectedItem.ToString.Substring(sID)
            groupFilters.Remove(groupName)
        Else
            ' Remove the category
            catFilters.Remove(lstFilters.SelectedItem.ToString)
        End If
        filters.Remove(lstFilters.SelectedItem.ToString)
        lstFilters.Items.Remove(lstFilters.SelectedItem.ToString)
        Call Me.SetGroupFilterLabel()
    End Sub
    Private Sub tvwFilter_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwFilter.NodeMouseClick
        tvwFilter.SelectedNode = e.Node
    End Sub
    Private Sub RemoveFilterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFilterToolStripMenuItem.Click
        Call Me.AddFilter()
    End Sub
    Private Sub lstFilters_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstFilters.MouseDoubleClick
        Call Me.RemoveFilter()
    End Sub
    Private Sub tvwFilter_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwFilter.NodeMouseDoubleClick
        Call Me.AddFilter()
    End Sub
    Private Sub SetGroupFilterLabel()
        Dim filter As String = "Group Filter: "
        If lstFilters.Items.Count > 0 Then
            For Each group As String In lstFilters.Items
                filter &= group & ", "
            Next
            filter = filter.TrimEnd(", ".ToCharArray)
        Else
            filter &= "None"
        End If
        lblGroupFilters.Text = filter
    End Sub
    Private Sub btnClearGroupFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearGroupFilters.Click
        ' Remove all items from the list of filters
        lstFilters.Items.Clear()
        ' Remove all items from the group and cat filters
        catFilters.Clear()
        groupFilters.Clear()
        filters.Clear()
        Call Me.SetGroupFilterLabel()
    End Sub
    Private Sub btnAddAllOwners_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAllOwners.Click
        For Each Owner As ListViewItem In lvwCharFilter.Items
            Owner.Checked = True
        Next
    End Sub
    Private Sub btnClearAllOwners_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAllOwners.Click
        For Each Owner As ListViewItem In lvwCharFilter.Items
            Owner.Checked = False
        Next
    End Sub
    Private Sub btnSelectCorp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectCorp.Click
        For Each Owner As ListViewItem In lvwCharFilter.Items
            If Owner.Group.Name = "grpCorporation" Then
                Owner.Checked = True
            Else
                Owner.Checked = False
            End If
        Next
    End Sub
    Private Sub btnSelectPersonal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectPersonal.Click
        For Each Owner As ListViewItem In lvwCharFilter.Items
            If Owner.Group.Name = "grpPersonal" Then
                Owner.Checked = True
            Else
                Owner.Checked = False
            End If
        Next
    End Sub
    Private Sub lvwCharFilter_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwCharFilter.ItemChecked
        ' Write the owner filter to the main Assets tab
        Dim filter As String = "Owner Filter: "
        Dim itemCount As Integer = 0
        Try
            itemCount = lvwCharFilter.CheckedItems.Count
        Catch ex As Exception
            itemCount = 0
        End Try
        If itemCount = 0 Then
            filter &= "None"
        Else
            Try
                For Each Owner As ListViewItem In lvwCharFilter.Items
                    If Owner.Checked = True Then
                        filter &= Owner.Text & ", "
                    End If
                Next
            Catch ex As Exception
            End Try
        End If
        filter = filter.TrimEnd(", ".ToCharArray)
        lblOwnerFilters.Text = filter
    End Sub
    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        ' Automatically set the filters to just agree to this pilot
        For Each Owner As ListViewItem In lvwCharFilter.Items
            If Owner.Text = cboPilots.SelectedItem.ToString Then
                Owner.Checked = True
            Else
                Owner.Checked = False
            End If
        Next
        lblOwnerFilters.Text = "Owner Filter: " & cboPilots.SelectedItem.ToString
        Call Me.RefreshAssets()
        Call Me.ParseOrders()
    End Sub
    Private Sub FilterSystemValue()
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
        Dim minValue As Double
        If Double.TryParse(txtMinSystemValue.Text, minValue) = False Then
            minValue = 0
        End If
        Dim cL As Integer = 0
        Dim cLoc As ContainerListViewItem
        Do
            cLoc = tlvAssets.Items(cL)
            If CDbl(cLoc.SubItems(7).Text) < minValue Then
                Call FilterSystemNode(cLoc)
                If cLoc.Items.Count = 0 Then
                    tlvAssets.Items.Remove(cLoc)
                    cL -= 1
                End If
            End If
            cL += 1
        Loop Until (cL = tlvAssets.Items.Count)
        Call Me.RecalcAllPrices()

    End Sub
    Private Sub FilterSystemNode(ByVal pLoc As ContainerListViewItem)
        Dim cL As Integer = 0
        Dim cLoc As ContainerListViewItem
        Do
            cLoc = pLoc.Items(cL)
            If cLoc.Items.Count = 0 Then
                pLoc.Items.Remove(cLoc)
                assetList.Remove(cLoc.Tag)
                totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                cL -= 1
            Else
                Call FilterSystemNode(cLoc)
                If cLoc.Items.Count = 0 Then
                    pLoc.Items.Remove(cLoc)
                    assetList.Remove(cLoc.Tag)
                    totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                    cL -= 1
                Else
                    If IsNumeric(cLoc.SubItems(5).Text) = True Then
                        totalAssetCount -= CLng(cLoc.SubItems(5).Text)
                    End If
                    ' Remove quantity and price information
                    cLoc.SubItems(5).Text = ""
                    cLoc.SubItems(6).Text = ""
                    assetList.Remove(cLoc.Tag)
                End If
            End If
            cL += 1
        Loop Until (cL = pLoc.Items.Count)
    End Sub
    Private Sub RecalcAllPrices()
        totalAssetValue = 0
        Dim locPrice As Double = 0
        For Each cLoc As ContainerListViewItem In tlvAssets.Items
            ' Calculate cost of all the sub nodes
            If cLoc.Items.Count > 0 Then
                locPrice = Me.RecalcNodePrice(cLoc)
                cLoc.SubItems(7).Text = FormatNumber(locPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                totalAssetValue += locPrice
            End If
        Next
    End Sub
    Private Function RecalcNodePrice(ByVal pLoc As ContainerListViewItem) As Double
        Dim lineValue As Double = 0
        Dim contValue As Double = 0
        For Each cLoc As ContainerListViewItem In pLoc.Items
            If cLoc.Items.Count > 0 Then
                Call Me.RecalcNodePrice(cLoc)
                lineValue = CDbl(cLoc.SubItems(7).Text)
                contValue += lineValue
            Else
                If IsNumeric(cLoc.SubItems(6).Text) = True Then
                    lineValue = CDbl(cLoc.SubItems(5).Text) * CDbl(cLoc.SubItems(6).Text)
                Else
                    lineValue = 0
                End If
                cLoc.SubItems(7).Text = FormatNumber(lineValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                contValue += lineValue
            End If
        Next
        If IsNumeric(pLoc.SubItems(6).Text) = True Then
            contValue += CDbl(pLoc.SubItems(6).Text)
        End If
        pLoc.SubItems(7).Text = FormatNumber(contValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Return contValue
    End Function
    Private Sub lblOwnerFilters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblOwnerFilters.TextChanged
        lblRigOwnerFilter.Text = lblOwnerFilters.Text
    End Sub
#End Region

#Region "Reports"

#Region "Common Routines"
    Private Function HTMLHeader(ByVal browserHeader As String, ByVal forIGB As Boolean) As String

        Dim strHTML As String = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""http://www.w3.org/TR/html4/strict.dtd"">"
        strHTML &= "<html lang=""" & System.Globalization.CultureInfo.CurrentCulture.ToString & """>"
        If forIGB = False Then
            strHTML &= "<head>"
            strHTML &= "<META http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">"
            strHTML &= "<title>" & browserHeader & "</title>" & CharacterCSS() & "</head>"
        Else
            strHTML &= "<head><title>" & browserHeader & "</title></head>"
        End If
        strHTML &= "<body>"
        strHTML &= "<p align='center'><img src='http://www.evehq.net/images/evehq_igb.png' alt='EveHQ Logo'></p>"
        Return strHTML

    End Function
    Private Function HTMLTitle(ByVal Title As String, ByVal forIGB As Boolean) As String
        Dim strHTML As String = ""

        If forIGB = False Then
            strHTML &= "<table width=800px border=0 align=center>"
            strHTML &= "<tr height=30px><td><p class=title>" & Title & "</p></td></tr>"
            strHTML &= "</table>"
        Else
            strHTML &= "<h1>" & Title & "</h1>"
        End If
        strHTML &= "<p></p>"
        Return strHTML
    End Function
    Private Function HTMLFooter(ByVal forIGB As Boolean) As String
        Dim strHTML As String = ""
        If forIGB = False Then
            strHTML &= "<table width=800px align=center border=0><hr>"
        Else
            strHTML &= "<table width=800px border=0><hr>"
        End If

        strHTML &= "<tr><td><p align=center class=footer>Generated on " & Format(Now, "dd/MM/yyyy HH:mm:ss") & " by <a href='http://www.evehq.net'>" & My.Application.Info.ProductName & "</a> v" & My.Application.Info.Version.ToString & "</p></td></tr>"
        strHTML &= "</table>"
        strHTML &= "</body></html>"

        Return strHTML
    End Function
    Private Function CharacterCSS() As String
        Dim strCSS As String = ""
        strCSS &= "<STYLE><!--"
        strCSS &= "BODY { font-family: Tahoma, Arial; font-size: 12px; bgcolor: #000000; background: #000000 }"
        strCSS &= "TD, P { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff }"
        strCSS &= ".thead { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff; font-variant: small-caps; background-color: #444444 }"
        strCSS &= ".footer { font-family: Tahoma, Arial; font-size: 9px; color: #ffffff; font-variant: small-caps }"
        strCSS &= ".title { font-family: Tahoma, Arial; font-size: 20px; color: #ffffff; font-variant: small-caps }"
        strCSS &= "--></STYLE>"
        Return strCSS
    End Function
#End Region

#Region "Comparers for Multi-dimensional arrays"

    Class RectangularComparer
        Implements IComparer
        ' maintain a reference to the 2-dimensional array being sorted

        Private sortArray(,) As Long

        ' constructor initializes the sortArray reference
        Public Sub New(ByVal theArray(,) As Long)
            sortArray = theArray
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            ' x and y are integer row numbers into the sortArray
            Dim i1 As Long = DirectCast(x, Integer)
            Dim i2 As Long = DirectCast(y, Integer)

            ' compare the items in the sortArray
            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
        End Function
    End Class

    Class ArrayComparerString
        Implements IComparer
        ' maintain a reference to the 2-dimensional array being sorted

        Private sortArray(,) As String

        ' constructor initializes the sortArray reference
        Public Sub New(ByVal theArray(,) As String)
            sortArray = theArray
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            ' x and y are integer row numbers into the sortArray
            Dim i1 As String = CStr(DirectCast(x, Integer))
            Dim i2 As String = CStr(DirectCast(y, Integer))

            ' compare the items in the sortArray
            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
        End Function
    End Class

    Class ArrayComparerDouble
        Implements IComparer
        ' maintain a reference to the 2-dimensional array being sorted

        Private sortArray(,) As Double

        ' constructor initializes the sortArray reference
        Public Sub New(ByVal theArray(,) As Double)
            sortArray = theArray
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            ' x and y are integer row numbers into the sortArray
            Dim i1 As Double = CDbl(DirectCast(x, Integer))
            Dim i2 As Double = CDbl(DirectCast(y, Integer))

            ' compare the items in the sortArray
            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
        End Function
    End Class

#End Region

#Region "Asset Location Report"

    Private Function AssetLocationReport() As String
        Dim strHTML As New StringBuilder
        Dim TotalValue As Double = 0
        Dim LocationValue As Double = 0
        Dim GroupValue As Double = 0
        strHTML.Append("<table width=800px align=center>")
        For Each Loc As ContainerListViewItem In tlvAssets.Items
            LocationValue = 0
            strHTML.Append("<tr bgcolor=444488><td colspan=6>" & Loc.Text & "</td></tr>")
            Dim assets As New SortedList
            Dim assetsList As New SortedList
            Dim newAsset As New AssetItem
            For Each item As ContainerListViewItem In Loc.Items
                If item.SubItems(2).Text <> "" Then
                    If (filters.Count > 0 And catFilters.Contains(item.SubItems(3).Text) = False And groupFilters.Contains(item.SubItems(2).Text) = False) Or (searchText <> "" And item.Text.ToLower.Contains(searchText.ToLower) = False) Then
                    Else
                        If assets.ContainsKey(item.SubItems(3).Text & "_" & item.SubItems(2).Text) = True Then
                            assetsList = CType(assets.Item(item.SubItems(3).Text & "_" & item.SubItems(2).Text), Collections.SortedList)
                            assetsList.Add(item.Tag.ToString, item.Tag.ToString)
                        Else
                            Dim assetList As New SortedList
                            assetsList = New SortedList
                            assetsList.Add(item.Tag.ToString, item.Tag.ToString)
                            assets.Add(item.SubItems(3).Text & "_" & item.SubItems(2).Text, assetsList)
                        End If
                    End If
                    If item.Items.Count > 0 Then
                        Call GetGroupsInNodes(item, assets, assetsList)
                    End If
                End If
            Next
            For Each assetGroup As String In assets.Keys
                GroupValue = 0
                Dim groupings() As String = assetGroup.Split("_".ToCharArray)
                strHTML.Append("<tr bgcolor=222244>")
                strHTML.Append("<td>" & groupings(0) & " / " & groupings(1) & "</td>")
                strHTML.Append("<td align=center>Owner</td>")
                strHTML.Append("<td align=center>Location</td>")
                strHTML.Append("<td align=center>Quantity</td>")
                strHTML.Append("<td align=center>Unit Price</td>")
                strHTML.Append("<td align=center>Total Price</td>")
                strHTML.Append("</tr>")
                assetsList = CType(assets(assetGroup), Collections.SortedList)
                For Each asset As String In assetsList.Keys
                    newAsset = CType(assetList.Item(asset), AssetItem)
                    strHTML.Append("<tr bgcolor=448844>")
                    strHTML.Append("<td>" & newAsset.typeName & "</td>")
                    strHTML.Append("<td align=center>" & newAsset.owner & "</td>")
                    strHTML.Append("<td align=center>" & newAsset.location & "</td>")
                    strHTML.Append("<td align=center>" & FormatNumber(newAsset.quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
                    strHTML.Append("<td align=right>" & FormatNumber(Math.Round(newAsset.price, 2), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
                    strHTML.Append("<td align=right>" & FormatNumber(CDbl(newAsset.quantity) * CDbl(Math.Round(newAsset.price, 2)), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
                    strHTML.Append("</tr>")
                    GroupValue += CDbl(newAsset.quantity) * CDbl(Math.Round(newAsset.price, 2))
                Next
                strHTML.Append("<tr bgcolor=000000>")
                strHTML.Append("<td></td>")
                strHTML.Append("<td colspan=4>TOTAL GROUP VALUE</td>")
                strHTML.Append("<td align=right>" & FormatNumber(GroupValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
                LocationValue += GroupValue
            Next
            strHTML.Append("<tr>")
            strHTML.Append("<td bgcolor=000000></td>")
            strHTML.Append("<td bgcolor=aa00aa colspan=4>TOTAL LOCATION VALUE</td>")
            strHTML.Append("<td bgcolor=aa00aa align=right>" & FormatNumber(LocationValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
            TotalValue += LocationValue
            strHTML.Append("<tr bgcolor=000000><td colspan=6>&nbsp;</td></tr>")
        Next
        strHTML.Append("<tr>")
        strHTML.Append("<td></td>")
        strHTML.Append("<td  bgcolor=aa6600 colspan=4>TOTAL VALUE</td>")
        strHTML.Append("<td  bgcolor=aa6600 align=right>" & FormatNumber(TotalValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
        strHTML.Append("</table>")
        Return strHTML.ToString
    End Function
    Private Sub GetGroupsInNodes(ByVal parent As ContainerListViewItem, ByVal assets As SortedList, ByVal assetslist As SortedList)
        For Each item As ContainerListViewItem In parent.Items
            If item.SubItems(2).Text <> "" Then
                If (filters.Count > 0 And catFilters.Contains(item.SubItems(3).Text) = False And groupFilters.Contains(item.SubItems(2).Text) = False) Or (searchText <> "" And item.Text.ToLower.Contains(searchText.ToLower) = False) Then
                Else
                    If assets.ContainsKey(item.SubItems(3).Text & "_" & item.SubItems(2).Text) = True Then
                        assetslist = CType(assets.Item(item.SubItems(3).Text & "_" & item.SubItems(2).Text), Collections.SortedList)
                        assetslist.Add(item.Tag.ToString, item.Tag.ToString)
                    Else
                        assetslist = New SortedList
                        assetslist.Add(item.Tag.ToString, item.Tag.ToString)
                        assets.Add(item.SubItems(3).Text & "_" & item.SubItems(2).Text, assetslist)
                    End If
                End If
                If item.Items.Count > 0 Then
                    Call GetGroupsInNodes(item, assets, assetslist)
                End If
            End If
        Next
    End Sub
#End Region

#Region "Asset List Reports"
    Private Function AssetListReportByName() As String
        Dim strHTML As New StringBuilder
        Dim TotalValue As Double = 0
        ' Create a new sortedlist with all the stuff in it
        Dim assets As New SortedList
        Dim newAsset As New AssetItem
        Dim currentValue As Double = 0
        For Each asset As String In assetList.Keys
            newAsset = CType(assetList(asset), AssetItem)
            If assets.ContainsKey(newAsset.typeName) = False Then
                assets.Add(newAsset.typeName, 0)
            End If
            currentValue = CDbl(assets(newAsset.typeName))
            currentValue += newAsset.quantity
            assets(newAsset.typeName) = currentValue
        Next
        strHTML.Append("<table width=800px align=center>")
        strHTML.Append("<tr bgcolor=222244>")
        strHTML.Append("<td>Asset Type</td>")
        strHTML.Append("<td align=center>Quantity</td>")
        strHTML.Append("<td align=center>Unit Price</td>")
        strHTML.Append("<td align=center>Total Price</td>")
        strHTML.Append("</tr>")
        Dim price As Double = 0
        Dim quantity As Double = 0
        For Each asset As String In assets.Keys
            ' Get price
            If asset.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
                price = 0
            Else
                If EveHQ.Core.HQ.itemList.Contains(asset) Then
                    price = EveHQ.Core.DataFunctions.GetPrice(CStr(EveHQ.Core.HQ.itemList(asset)))
                Else
                    price = 0
                End If
            End If
            price = Math.Round(price, 2)
            ' Get quantity
            quantity = CDbl(assets(asset))
            strHTML.Append("<tr bgcolor=448844>")
            strHTML.Append("<td>" & asset & "</td>")
            strHTML.Append("<td align=center>" & FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
            strHTML.Append("<td align=right>" & FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
            strHTML.Append("<td align=right>" & FormatNumber(quantity * price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
            strHTML.Append("</tr>")
            TotalValue += quantity * price
        Next
        strHTML.Append("<tr bgcolor=000000>")
        strHTML.Append("<td></td>")
        strHTML.Append("<td colspan=2>TOTAL ASSET VALUE</td>")
        strHTML.Append("<td align=right>" & FormatNumber(TotalValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
        strHTML.Append("</table>")
        Return strHTML.ToString
    End Function
    Private Function AssetListReportByNumeric(ByVal field As Integer, Optional ByVal reverse As Boolean = False) As String
        Dim strHTML As New StringBuilder
        Dim TotalValue As Double = 0
        ' Create a new sortedlist with all the stuff in it
        Dim assets As New SortedList
        Dim newAsset As New AssetItem
        Dim currentValue As Double = 0
        For Each asset As String In assetList.Keys
            newAsset = CType(assetList(asset), AssetItem)
            If assets.ContainsKey(newAsset.typeName) = False Then
                assets.Add(newAsset.typeName, 0)
            End If
            currentValue = CDbl(assets(newAsset.typeName))
            currentValue += newAsset.quantity
            assets(newAsset.typeName) = currentValue
        Next

        Dim sortSkill(assets.Count, 1) As Double
        Dim count As Integer = 0
        For Each asset As String In assets.Keys
            sortSkill(count, 0) = count
            Dim cPrice As Double = 0
            If asset.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
                cPrice = 0
            Else
                If EveHQ.Core.HQ.itemList.Contains(asset) Then
                    cPrice = EveHQ.Core.DataFunctions.GetPrice(CStr(EveHQ.Core.HQ.itemList(asset)))
                Else
                    cPrice = 0
                End If
            End If
            cPrice = Math.Round(cPrice, 2)
            Select Case field
                Case 1 ' Quantity
                    sortSkill(count, 1) = CDbl(assets(asset))
                Case 2 ' Unit Price
                    ' Get price
                    sortSkill(count, 1) = cPrice
                Case 3 ' Total Price
                    sortSkill(count, 1) = CDbl(assets(asset)) * cPrice
            End Select
            count += 1
        Next
        ' Create a tag array ready to sort the skill times
        Dim tagArray(assets.Count - 1) As Integer
        For a As Integer = 0 To assets.Count - 1
            tagArray(a) = a
        Next
        ' Initialize the comparer and sort
        Dim myComparer As New ArrayComparerDouble(sortSkill)
        Array.Sort(tagArray, myComparer)
        If reverse = True Then
            Array.Reverse(tagArray)
        End If

        strHTML.Append("<table width=800px align=center>")
        strHTML.Append("<tr bgcolor=222244>")
        strHTML.Append("<td>Asset Type</td>")
        strHTML.Append("<td align=center>Quantity</td>")
        strHTML.Append("<td align=center>Unit Price</td>")
        strHTML.Append("<td align=center>Total Price</td>")
        strHTML.Append("</tr>")
        Dim idx As Integer = 0
        Dim price As Double = 0
        Dim quantity As Double = 0
        Dim cAsset As String = ""
        For i As Integer = 0 To tagArray.Length - 1
            idx = CInt(sortSkill(tagArray(i), 0))
            cAsset = CStr(assets.GetKey(idx))
            'For Each asset As String In assets.Keys
            ' Get price
            If cAsset.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
                price = 0
            Else
                If EveHQ.Core.HQ.itemList.Contains(cAsset) Then
                    price = EveHQ.Core.DataFunctions.GetPrice(CStr(EveHQ.Core.HQ.itemList(cAsset)))
                Else
                    price = 0
                End If
            End If
            price = Math.Round(price, 2)
            ' Get quantity
            quantity = CDbl(assets(cAsset))
            strHTML.Append("<tr bgcolor=448844>")
            strHTML.Append("<td>" & cAsset & "</td>")
            strHTML.Append("<td align=center>" & FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
            strHTML.Append("<td align=right>" & FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
            strHTML.Append("<td align=right>" & FormatNumber(quantity * price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
            strHTML.Append("</tr>")
            TotalValue += quantity * price
        Next
        strHTML.Append("<tr bgcolor=000000>")
        strHTML.Append("<td></td>")
        strHTML.Append("<td colspan=2>TOTAL ASSET VALUE</td>")
        strHTML.Append("<td align=right>" & FormatNumber(TotalValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
        strHTML.Append("</table>")
        Return strHTML.ToString
    End Function

#End Region

#End Region

#Region "Investment Routines"
    Private Sub btnAddInvestment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddInvestment.Click
        Dim oldCount As Long = Portfolio.Investments.Count
        Dim NewInvestment As New frmAddInvestment
        NewInvestment.txtInvestmentID.Text = (Portfolio.Investments.Count + 1).ToString
        'NewInvestment.txtDateCreated.Text = Format(Now, "dd/MM/yyyy HH:mm:ss")
        NewInvestment.txtDateCreated.Text = FormatDateTime(Now, DateFormat.GeneralDate)
        NewInvestment.ShowDialog()
        If Portfolio.Investments.Count <> oldCount Then
            Call Me.ListInvestments()
            Call Me.SaveInvestments()
        End If
    End Sub
    Private Sub ListInvestments()
        lvwInvestments.BeginUpdate()
        lvwInvestments.Items.Clear()
        Dim newItem As New ListViewItem
        Try
            For Each myInvestment As Investment In Portfolio.Investments.Values
                newItem = New ListViewItem
                If myInvestment.DateClosed.Year = 1 Or (myInvestment.DateClosed.Year > 1 And chkViewClosedInvestments.Checked = True) Then
                    If myInvestment.DateClosed.Year > 1 Then
                        newItem.ForeColor = Drawing.Color.Red
                    End If
                    newItem.Name = myInvestment.ID.ToString
                    newItem.Text = myInvestment.ID.ToString
                    newItem.SubItems.Add(myInvestment.Name)
                    newItem.SubItems.Add(myInvestment.Owner)
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentQuantity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    If myInvestment.ValueIsCost = True Then
                        newItem.SubItems.Add(FormatNumber(myInvestment.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    Else
                        newItem.SubItems.Add(FormatNumber(myInvestment.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    End If
                    newItem.SubItems.Add(FormatNumber((myInvestment.CurrentValue * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.SubItems.Add(FormatNumber((myInvestment.CurrentValue * myInvestment.CurrentQuantity) - (myInvestment.CurrentCost * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentProfits, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentIncome, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentIncome / myInvestment.TotalCostsForYield * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%")
                    lvwInvestments.Items.Add(newItem)
                End If
            Next
            lvwInvestments.EndUpdate()
        Catch ice As InvalidCastException
            ' Catch an exception moving over to the new Prism plug-in
            Portfolio.Investments.Clear()
        End Try
    End Sub

    Private Sub btnClearInvestments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearInvestments.Click
        Dim msg As String = "This will clear all your investments and transactions. Are you sure you wish to proceed with this?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Clear Investments?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            Portfolio.Investments.Clear()
            Portfolio.Transactions.Clear()
            Call Me.ListInvestments()
            lvwTransactions.Items.Clear()
        End If
    End Sub

    Private Sub SaveInvestments()
        Dim s As New FileStream(EveHQ.Core.HQ.dataFolder & "\investments.txt", FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, Portfolio.Investments)
        s.Close()
        s = New FileStream(EveHQ.Core.HQ.dataFolder & "\investmentTransactions.txt", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Portfolio.Transactions)
        s.Close()
    End Sub
    Private Sub LoadInvestments()
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.dataFolder & "\investments.txt") = True Then
            Dim s As New FileStream(EveHQ.Core.HQ.dataFolder & "\investments.txt", FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            Portfolio.Investments = CType(f.Deserialize(s), SortedList)
            s.Close()
        End If
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.dataFolder & "\investmentTransactions.txt") = True Then
            Dim s As New FileStream(EveHQ.Core.HQ.dataFolder & "\investmentTransactions.txt", FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            Portfolio.Transactions = CType(f.Deserialize(s), SortedList)
            s.Close()
        End If
        Call Me.ListInvestments()
    End Sub

    Private Sub btnAddTransaction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTransaction.Click
        ' Check if there is an investment highlighted
        If lvwInvestments.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select an investment before entering a transaction", "Transaction Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            Dim myInv As Investment = CType(Portfolio.Investments(CLng(lvwInvestments.SelectedItems(0).Text)), Investment)
            Dim oldCount As Long = Portfolio.Transactions.Count
            Dim NewTransaction As New frmAddTransaction
            NewTransaction.txtTransactionID.Text = (Portfolio.Transactions.Count + 1).ToString
            NewTransaction.txtInvestmentID.Text = CStr(myInv.ID)
            NewTransaction.txtInvestmentName.Text = myInv.Name
            ' Disable certain items if a particular investment type
            Select Case myInv.Type
                Case 0 ' Cash
                    NewTransaction.cboType.Items.Remove("Purchase")
                    NewTransaction.cboType.Items.Remove("Sale")
                    NewTransaction.txtQuantity.Visible = False
                    NewTransaction.lblQuantity.Visible = False
                Case 1 ' Shares
                    NewTransaction.cboType.Items.Remove("Income (Retained)")
                    NewTransaction.cboType.Items.Remove("Cost (Retained)")
                    NewTransaction.cboType.Items.Remove("Transfer To Investment")
                    NewTransaction.cboType.Items.Remove("Transfer From Investment")
                    NewTransaction.txtCurrentQuantity.Text = FormatNumber(myInv.CurrentQuantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            End Select
            NewTransaction.ShowDialog()
            If Portfolio.Transactions.Count <> oldCount Then
                Call Me.UpdateTransactions()
                Call Me.UpdateInvestment()
                Call Me.SaveInvestments()
            End If
        End If
    End Sub

    Private Sub lvwInvestments_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwInvestments.SelectedIndexChanged
        Call Me.UpdateTransactions()
    End Sub

    Private Sub UpdateTransactions()
        ' Get investment
        If lvwInvestments.SelectedItems.Count = 1 Then
            Dim myInvestment As Investment = CType(Portfolio.Investments(CLng(lvwInvestments.SelectedItems(0).Text)), Investment)
            lvwTransactions.BeginUpdate()
            lvwTransactions.Items.Clear()
            For Each myTransaction As InvestmentTransaction In myInvestment.Transactions.Values
                Dim newTrans As New ListViewItem
                newTrans.Name = CStr(myTransaction.ID)
                newTrans.Text = CStr(myTransaction.ID)
                newTrans.SubItems.Add(Format(myTransaction.TransDate, "dd/MM/yyyy HH:mm:ss"))
                newTrans.SubItems.Add(myTransaction.Type.ToString)
                newTrans.SubItems.Add(myTransaction.Quantity.ToString)
                newTrans.SubItems.Add(myTransaction.UnitValue.ToString)
                lvwTransactions.Items.Add(newTrans)
            Next
            lvwTransactions.EndUpdate()
            If myInvestment.DateClosed.Year > 1 Then
                btnAddTransaction.Enabled = False
                btnEditTransaction.Enabled = False
                btnReOpenInvestment.Visible = True
            Else
                btnAddTransaction.Enabled = True
                btnEditTransaction.Enabled = True
                btnReOpenInvestment.Visible = False
            End If
        End If
    End Sub

    Private Sub UpdateInvestment()
        Dim newItem As ListViewItem = lvwInvestments.SelectedItems(0)
        Dim myInvestment As Investment = CType(Portfolio.Investments(CLng(lvwInvestments.SelectedItems(0).Text)), Investment)
        newItem.SubItems.Add(myInvestment.Name)
        newItem.SubItems.Add(myInvestment.Owner)
        newItem.SubItems(3).Text = (FormatNumber(myInvestment.CurrentQuantity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newItem.SubItems(4).Text = (FormatNumber(myInvestment.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        If myInvestment.ValueIsCost = True Then
            newItem.SubItems(5).Text = (FormatNumber(myInvestment.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        Else
            newItem.SubItems(5).Text = (FormatNumber(myInvestment.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        End If
        newItem.SubItems(6).Text = (FormatNumber((myInvestment.CurrentValue * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newItem.SubItems(7).Text = (FormatNumber((myInvestment.CurrentValue * myInvestment.CurrentQuantity) - (myInvestment.CurrentCost * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newItem.SubItems(8).Text = (FormatNumber(myInvestment.CurrentProfits, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newItem.SubItems(9).Text = (FormatNumber(myInvestment.CurrentIncome, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newItem.SubItems(10).Text = (FormatNumber(myInvestment.CurrentIncome / myInvestment.TotalCostsForYield * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%")
    End Sub

    Private Sub btnClearTransactions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearTransactions.Click
        Dim reply As Integer = MessageBox.Show("This will delete the ENTIRE transaction history for EVERY investment. Are you sure you wish to proceed?", "Confirm Clear Transactions", MessageBoxButtons.OK, MessageBoxIcon.Information)
        If reply = Windows.Forms.DialogResult.Yes Then
            ' Clear the entire transaction history
            Portfolio.Transactions.Clear()
            ' Now clear it from all the investments
            For Each inv As Investment In Portfolio.Investments.Values
                inv.Transactions.Clear()
            Next
            Call Me.SaveInvestments()
        End If
    End Sub

    Private Sub RecalculateInvestment(ByVal inv As Investment)
        ' Recalculates all the investment figures by scanning all the transactions
        ' Reset the totals
        inv.CurrentCost = 0
        inv.CurrentCosts = 0
        inv.CurrentIncome = 0
        inv.CurrentProfits = 0
        inv.CurrentQuantity = 0
        inv.CurrentValue = 0
        inv.LastValuation = New Date
        inv.TotalCostsForYield = 0
        ' Go through each transaction and update the investment
        For Each trans As InvestmentTransaction In inv.Transactions.Values
            Select Case trans.Type
                Case 0 ' Purchase
                    Dim totalCost As Double = inv.CurrentQuantity * inv.CurrentCost
                    totalCost += trans.Quantity * trans.UnitValue
                    inv.CurrentQuantity += trans.Quantity
                    inv.CurrentCost = totalCost / inv.CurrentQuantity
                Case 1 ' Sale
                    inv.CurrentQuantity -= trans.Quantity
                    Dim profits As Double = (trans.UnitValue - inv.CurrentCost) * trans.Quantity
                    inv.CurrentProfits += profits
                Case 2 ' Valuation
                    ' Disregard the quantity
                    inv.CurrentValue = trans.UnitValue
                    inv.LastValuation = trans.TransDate
                Case 3 ' Income
                    inv.CurrentIncome += trans.UnitValue
                    If inv.Type = InvestmentType.Cash Then
                        inv.TotalCostsForYield += inv.CurrentCost
                    Else
                        inv.TotalCostsForYield += (inv.CurrentCost * inv.CurrentQuantity)
                    End If
                Case 4 ' Cost
                    inv.CurrentCosts += trans.UnitValue
                Case 5 ' Income (Retained)
                    inv.CurrentIncome += trans.UnitValue
                    If inv.Type = InvestmentType.Cash Then
                        inv.TotalCostsForYield += inv.CurrentCost
                    Else
                        inv.TotalCostsForYield += (inv.CurrentCost * inv.CurrentQuantity)
                    End If
                    inv.CurrentCost += trans.UnitValue
                Case 6 ' Cost (Retained)
                    inv.CurrentCosts += trans.UnitValue
                    inv.CurrentCost -= trans.UnitValue
                Case 7 ' Tfr to investment
                    inv.CurrentCost += trans.UnitValue
                Case 8 ' Tfr from investment
                    inv.CurrentCost -= trans.UnitValue
            End Select
        Next
        Call Me.SaveInvestments()
        Call Me.UpdateInvestment()
        MessageBox.Show("Recalculation of Investment Complete!", "Recalculate Investment Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Function AuditInvestment(ByVal inv As Investment, Optional ByVal silent As Boolean = False) As Boolean
        Dim passedAudit As Boolean = True
        ' Checks whether the totals of the investment are consistent with the transaction history
        Dim chkInv As New Investment
        ' Go through each transaction and update the investment
        For Each trans As InvestmentTransaction In inv.Transactions.Values
            Select Case trans.Type
                Case 0 ' Purchase
                    Dim totalCost As Double = chkInv.CurrentQuantity * chkInv.CurrentCost
                    totalCost += trans.Quantity * trans.UnitValue
                    chkInv.CurrentQuantity += trans.Quantity
                    chkInv.CurrentCost = totalCost / chkInv.CurrentQuantity
                Case 1 ' Sale
                    chkInv.CurrentQuantity -= trans.Quantity
                    Dim profits As Double = (trans.UnitValue - chkInv.CurrentCost) * trans.Quantity
                    chkInv.CurrentProfits += profits
                Case 2 ' Valuation
                    ' Disregard the quantity
                    chkInv.CurrentValue = trans.UnitValue
                    chkInv.LastValuation = trans.TransDate
                Case 3 ' Income
                    chkInv.CurrentIncome += trans.UnitValue
                    If chkInv.Type = InvestmentType.Cash Then
                        chkInv.TotalCostsForYield += chkInv.CurrentCost
                    Else
                        chkInv.TotalCostsForYield += (chkInv.CurrentCost * chkInv.CurrentQuantity)
                    End If
                Case 4 ' Cost
                    chkInv.CurrentCosts += trans.UnitValue
                Case 5 ' Income (Retained)
                    chkInv.CurrentIncome += trans.UnitValue
                    If chkInv.Type = InvestmentType.Cash Then
                        chkInv.TotalCostsForYield += chkInv.CurrentCost
                    Else
                        chkInv.TotalCostsForYield += (chkInv.CurrentCost * chkInv.CurrentQuantity)
                    End If
                    chkInv.CurrentCost += trans.UnitValue
                Case 6 ' Cost (Retained)
                    chkInv.CurrentCosts += trans.UnitValue
                    chkInv.CurrentCost -= trans.UnitValue
                Case 7 ' Tfr to investment
                    chkInv.CurrentCost += trans.UnitValue
                Case 8 ' Tfr from investment
                    chkInv.CurrentCost -= trans.UnitValue
            End Select
        Next
        ' Now report any differences
        Dim audit As New StringBuilder
        If inv.CurrentCost <> chkInv.CurrentCost Then
            audit.AppendLine("Current Cost - Per Investment: " & FormatNumber(inv.CurrentCost, 2) & ", Per Transactions: " & FormatNumber(chkInv.CurrentCost, 2))
            passedAudit = False
        End If
        If inv.CurrentCosts <> chkInv.CurrentCosts Then
            audit.AppendLine("Current Costs - Per Investment: " & FormatNumber(inv.CurrentCosts, 2) & ", Per Transactions: " & FormatNumber(chkInv.CurrentCosts, 2))
            passedAudit = False
        End If
        If inv.CurrentIncome <> chkInv.CurrentIncome Then
            audit.AppendLine("Current Income - Per Investment: " & FormatNumber(inv.CurrentIncome, 2) & ", Per Transactions: " & FormatNumber(chkInv.CurrentIncome, 2))
            passedAudit = False
        End If
        If inv.CurrentProfits <> chkInv.CurrentProfits Then
            audit.AppendLine("Current Profits - Per Investment: " & FormatNumber(inv.CurrentProfits, 2) & ", Per Transactions: " & FormatNumber(chkInv.CurrentProfits, 2))
            passedAudit = False
        End If
        If inv.CurrentQuantity <> chkInv.CurrentQuantity Then
            audit.AppendLine("Current Quantity - Per Investment: " & FormatNumber(inv.CurrentQuantity, 2) & ", Per Transactions: " & FormatNumber(chkInv.CurrentQuantity, 2))
            passedAudit = False
        End If
        If inv.CurrentValue <> chkInv.CurrentValue Then
            audit.AppendLine("Current Value - Per Investment: " & FormatNumber(inv.CurrentValue, 2) & ", Per Transactions: " & FormatNumber(chkInv.CurrentValue, 2))
            passedAudit = False
        End If
        If inv.LastValuation <> chkInv.LastValuation Then
            audit.AppendLine("Last Valuation - Per Investment: " & FormatDateTime(inv.LastValuation, DateFormat.GeneralDate) & ", Per Transactions: " & FormatDateTime(chkInv.LastValuation, DateFormat.GeneralDate))
            passedAudit = False
        End If
        If inv.TotalCostsForYield <> chkInv.TotalCostsForYield Then
            audit.AppendLine("Total Costs For Yield - Per Investment: " & FormatNumber(inv.TotalCostsForYield, 2) & ", Per Transactions: " & FormatNumber(chkInv.TotalCostsForYield, 2))
            passedAudit = False
        End If
        If silent = False Then
            If passedAudit = False Then
                audit.AppendLine("")
                audit.AppendLine("Do you want to correct these inconsistencies?")
                Dim reply As Integer = MessageBox.Show(audit.ToString, "Transaction Inconsistencies Idenitfied", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = Windows.Forms.DialogResult.Yes Then
                    Call Me.RecalculateInvestment(inv)
                End If
            Else
                audit.AppendLine("Audit complete - No errors found")
                MessageBox.Show(audit.ToString, "Transaction Audit Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
        Return passedAudit
    End Function

    Private Sub btnAuditInvestment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuditInvestment.Click
        ' Check if there is an investment highlighted
        If lvwInvestments.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select an investment before starting the audit", "Audit Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            Dim auditInv As Investment = CType(Portfolio.Investments(CLng(lvwInvestments.SelectedItems(0).Text)), Investment)
            Call Me.AuditInvestment(auditInv)
        End If
    End Sub

    Private Sub btnEditTransaction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditTransaction.Click
        ' Check if there is a transaction highlighted
        If lvwTransactions.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select a Transaction in order to Edit it!", "Edit Transaction Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Put up the warning about recalculation
            Dim msg As New StringBuilder
            msg.AppendLine("Warning: Editing a transaction will automatically force the whole of the transaction history to be recalculated, most likely altering the current figures of the selected investment.")
            msg.AppendLine("")
            msg.AppendLine("Are you sure you wish to proceed?")
            Dim reply As Integer = MessageBox.Show(msg.ToString, "Confirm Transaction Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.No Then
                Exit Sub
            Else
                Dim NewTransaction As New frmAddTransaction
                Dim editTrans As InvestmentTransaction = CType(Portfolio.Transactions(CLng(lvwTransactions.SelectedItems(0).Text)), InvestmentTransaction)
                Dim editInv As Investment = CType(Portfolio.Investments(CLng(lvwInvestments.SelectedItems(0).Text)), Investment)
                NewTransaction.txtTransactionID.Text = CStr(editTrans.ID)
                NewTransaction.txtInvestmentID.Text = CStr(editInv.ID)
                NewTransaction.txtInvestmentName.Text = editInv.Name
                NewTransaction.cboType.SelectedIndex = editTrans.Type
                NewTransaction.txtQuantity.Text = CStr(editTrans.Quantity)
                NewTransaction.txtUnitValue.Text = CStr(editTrans.UnitValue)
                NewTransaction.ShowDialog()
                Me.RecalculateInvestment(editInv)
                Me.UpdateInvestment()
                Me.UpdateTransactions()
            End If
        End If
    End Sub

    Private Sub btnEditInvestment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditInvestment.Click
        If lvwInvestments.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select an Investment in order to Edit it!", "Edit Investment Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            Dim inv As Investment = CType(Portfolio.Investments(CLng(lvwInvestments.SelectedItems(0).Text)), Investment)
            Dim EditInvestment As New frmAddInvestment
            EditInvestment.txtInvestmentID.Text = inv.ID.ToString
            EditInvestment.txtInvestmentName.Text = inv.Name
            EditInvestment.cboOwner.SelectedItem = inv.Owner
            EditInvestment.cboType.SelectedIndex = inv.Type
            EditInvestment.chkValueIsCost.Checked = inv.ValueIsCost
            EditInvestment.txtDescription.Text = inv.Description
            EditInvestment.txtDateCreated.Text = FormatDateTime(inv.DateCreated, DateFormat.GeneralDate)
            If inv.DateClosed.Year > 1 Then
                EditInvestment.txtDateClosed.Text = FormatDateTime(inv.DateClosed, DateFormat.GeneralDate)
            Else
                EditInvestment.lblDateClosed.Visible = False
                EditInvestment.txtDateClosed.Visible = False
            End If
            ' Disable some items that cannot be changed
            EditInvestment.txtInvestmentID.Enabled = False
            EditInvestment.cboType.Enabled = False
            EditInvestment.txtDateCreated.Enabled = False
            EditInvestment.ShowDialog()
            Call Me.ListInvestments()
            Call Me.SaveInvestments()
        End If
    End Sub

    Private Sub btnCloseInvestment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseInvestment.Click
        If lvwInvestments.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select an Investment before closing it!", "Close Investment Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Put up the warning about deletion
            Dim msg As New StringBuilder
            msg.AppendLine("Warning: Closing an investment will cause it to become inactive.")
            msg.AppendLine("")
            msg.AppendLine("Are you sure you wish to proceed?")
            Dim reply As Integer = MessageBox.Show(msg.ToString, "Confirm Investment Close?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.No Then
                Exit Sub
            Else
                Dim editInv As Investment = CType(Portfolio.Investments(CLng(lvwInvestments.SelectedItems(0).Text)), Investment)
                editInv.DateClosed = Now
                Call Me.ListInvestments()
                Call Me.SaveInvestments()
                lvwTransactions.Items.Clear()
            End If
        End If
    End Sub
    Private Sub btnReOpenInvestment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReOpenInvestment.Click
        If lvwInvestments.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select an Investment before closing it!", "Reopen Investment Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Put up the warning about reopening
            Dim msg As New StringBuilder
            msg.AppendLine("Are you sure you wish to re-open this investment?")
            Dim reply As Integer = MessageBox.Show(msg.ToString, "Confirm Re-open Investment?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.No Then
                Exit Sub
            Else
                Dim editInv As Investment = CType(Portfolio.Investments(CLng(lvwInvestments.SelectedItems(0).Text)), Investment)
                editInv.DateClosed = New Date(1, 1, 1)
                Call Me.ListInvestments()
                Call Me.SaveInvestments()
                btnReOpenInvestment.Visible = False
            End If
        End If
    End Sub
    Private Sub chkViewClosedInvestments_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkViewClosedInvestments.CheckedChanged
        Me.ListInvestments()
    End Sub
#End Region

#Region "Asset Context Menu & UI Routines"
    Private Sub ctxAssets_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxAssets.Opening
        If tlvAssets.SelectedItems.Count > 0 Then
            If tlvAssets.SelectedItems.Count = 1 Then
                Dim itemName As String = tlvAssets.SelectedItems(0).Text
                If itemName <> "Cash Balances" And itemName <> "Investments" Then
                    If EveHQ.Core.HQ.itemList.Contains(itemName) = True And itemName <> "Office" And tlvAssets.SelectedItems(0).SubItems(7).Text <> "" Then
                        mnuItemName.Text = itemName
                        mnuItemName.Tag = EveHQ.Core.HQ.itemList(itemName)
                        mnuViewInIB.Visible = True
                        mnuModifyPrice.Visible = True
                        mnuToolSep.Visible = True
                        mnuRecycleItem.Enabled = True
                        If tlvAssets.SelectedItems(0).SubItems(3).Text = "Ship" Then
                            mnuViewInHQF.Visible = True
                        Else
                            mnuViewInHQF.Visible = False
                        End If
                        If tlvAssets.SelectedItems(0).Items.Count > 0 Then
                            mnuRecycleContained.Enabled = True
                            mnuRecycleAll.Enabled = True
                        Else
                            mnuRecycleContained.Enabled = False
                            mnuRecycleAll.Enabled = False
                        End If
                    Else
                        mnuItemName.Text = itemName
                        mnuViewInIB.Visible = False
                        mnuViewInHQF.Visible = False
                        mnuModifyPrice.Visible = False
                        mnuToolSep.Visible = False
                        If tlvAssets.SelectedItems(0).Items.Count > 0 Then
                            mnuRecycleItem.Enabled = False
                            mnuRecycleContained.Enabled = True
                            mnuRecycleAll.Enabled = False
                        Else
                            e.Cancel = True
                        End If
                    End If
                Else
                    e.Cancel = True
                End If
            Else
                mnuItemName.Text = "Multiple Items"
                mnuItemName.Tag = ""
                mnuViewInIB.Visible = False
                mnuViewInHQF.Visible = False
                mnuModifyPrice.Visible = False
                mnuToolSep.Visible = False
                Dim containerItems As Boolean = False
                For Each item As ContainerListViewItem In tlvAssets.SelectedItems
                    If item.Items.Count > 0 Then
                        containerItems = True
                        Exit For
                    End If
                Next
                If containerItems = True Then
                    mnuRecycleContained.Enabled = True
                    mnuRecycleAll.Enabled = True
                Else
                    mnuRecycleContained.Enabled = False
                    mnuRecycleAll.Enabled = False
                End If
                mnuRecycleItem.Enabled = True
            End If
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuViewInIB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewInIB.Click
        ' This routine is shit hot!!
        Dim PluginName As String = "EveHQ Item Browser"
        Dim itemID As String = mnuItemName.Tag.ToString
        Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PluginName), Core.PlugIn)
        If myPlugIn.Status = EveHQ.Core.PlugIn.PlugInStatus.Active Then
            Dim mainTab As TabControl = CType(EveHQ.Core.HQ.MainForm.Controls("tabMDI"), TabControl)
            If mainTab.TabPages.ContainsKey(PluginName) = True Then
                mainTab.SelectTab(PluginName)
            Else
                Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
                plugInForm.MdiParent = EveHQ.Core.HQ.MainForm
                plugInForm.Show()
            End If
            myPlugIn.Instance.GetPlugInData(itemID)
        Else
            ' Plug-in is not loaded so best not try to access it!
            Dim msg As String = ""
            msg &= "The " & myPlugIn.MainMenuText & " Plug-in is not currently active." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Please load the plug-in before proceeding."
            MessageBox.Show(msg, "Error Starting " & myPlugIn.MainMenuText & " Plug-in!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub
    Private Sub mnuModifyPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuModifyPrice.Click
        Dim newPrice As New frmModifyPrice
        Dim itemID As String = mnuItemName.Tag.ToString
        newPrice.lblBasePrice.Text = FormatNumber(EveHQ.Core.HQ.BasePriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.lblMarketPrice.Text = FormatNumber(EveHQ.Core.HQ.MarketPriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.lblCustomPrice.Text = FormatNumber(EveHQ.Core.HQ.CustomPriceList(itemID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        newPrice.txtNewPrice.Tag = itemID
        newPrice.txtNewPrice.Text = ""
        newPrice.Text = "Modify Price - " & mnuItemName.Text
        newPrice.ShowDialog()
    End Sub
    Private Sub mnuViewInHQF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewInHQF.Click
        If tlvAssets.SelectedItems.Count > 0 Then
            Dim assetID As String = tlvAssets.SelectedItems(0).Tag.ToString
            Dim shipName As String = tlvAssets.SelectedItems(0).Text
            Dim owner As String = tlvAssets.SelectedItems(0).SubItems(1).Text
            HQFShip = New ArrayList
            Call Me.SearchForShip(assetID, owner)
            ' Should have got the ship by now
            HQFShip.Sort()
            Dim FittedMods As New SortedList
            Dim Drones As New SortedList
            Dim ChargedMod(1) As String
            For Each fittedMod As String In HQFShip
                Dim modData() As String = fittedMod.Split(",".ToCharArray)
                If FittedMods.Contains(modData(0)) = False Then
                    ReDim ChargedMod(1)
                    If modData(3) = "8" Then ' Is Charge
                        ChargedMod(1) = modData(1)
                    Else
                        ChargedMod(0) = modData(1)
                    End If
                    FittedMods.Add(modData(0), ChargedMod)
                Else
                    ChargedMod = CType(FittedMods(modData(0)), String())
                    If modData(3) = "8" Then ' Is Charge
                        ChargedMod(1) = modData(1)
                    Else
                        ChargedMod(0) = modData(1)
                    End If
                    FittedMods(modData(0)) = ChargedMod
                End If
                If modData(3) = "18" Then ' Is Drone
                    If Drones.Contains(modData(1)) = True Then
                        Dim CDQ As Integer = CInt(modData(2))
                        Dim TDQ As Integer = CInt(Drones(modData(1)))
                        Drones(modData(1)) = (TDQ + CDQ).ToString
                    Else
                        Drones.Add(modData(1), modData(2))
                    End If
                End If
            Next
            Dim list As New StringBuilder
            list.AppendLine("[" & shipName & "," & owner & "'s " & shipName & "]")
            For Each fittedMod As String In HQFShip
                Dim modData() As String = fittedMod.Split(",".ToCharArray)
                Select Case modData(0)
                    Case "Drone Bay"
                        ' Ignore as we will be adding them later
                        '    list.AppendLine(modData(1) & ", " & modData(2) & "i")
                    Case "Cargo Bay"
                        If modData(3) = "8" Then
                            list.AppendLine(modData(1) & ", " & modData(2))
                        End If
                    Case Else
                        If FittedMods.Contains(modData(0)) = True Then
                            ChargedMod = CType(FittedMods(modData(0)), String())
                            If ChargedMod(1) = "" Then
                                list.AppendLine(ChargedMod(0))
                            Else
                                list.AppendLine(ChargedMod(0) & ", " & ChargedMod(1))
                            End If
                            FittedMods.Remove(modData(0))
                        End If
                End Select
            Next
            ' Add Drones
            For Each drone As String In Drones.Keys
                list.AppendLine(drone & ", " & Drones(drone).ToString & "i")
            Next
            Clipboard.SetText(list.ToString)
            MessageBox.Show(list.ToString, "Copy To Clipboard Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub SearchForShip(ByVal assetID As String, ByVal owner As String)

        Dim assetOwner As String = ""
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            ' Check in the cache folder for a valid file
            Dim selPilot As EveHQ.Core.Pilot = CType(loadedOwners(owner), Core.Pilot)
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                Dim fileName As String = ""
                If cPilot.Text = selPilot.Corp Then
                    fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_15_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                    assetOwner = selPilot.Corp
                Else
                    fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_14_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                    assetOwner = selPilot.Name
                End If
                If My.Computer.FileSystem.FileExists(fileName) = False Then
                    MessageBox.Show("Unable to load assets file for " & assetOwner & ".", "Error Loading Assets", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                ' File found so lets see what we have!
                Dim assetXML As New XmlDocument
                assetXML.Load(fileName)
                Dim locList As XmlNodeList
                Dim loc As XmlNode
                locList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                If locList.Count > 0 Then
                    For Each loc In locList
                        ' Let's search for our asset!
                        If loc.Attributes.GetNamedItem("itemID").Value = assetID Then
                            ' We found our ship so extract the subitem data
                            Dim groupID As String = ""
                            Dim catID As String = ""
                            Dim modList As XmlNodeList
                            Dim mods As XmlNode
                            If loc.ChildNodes.Count > 0 Then
                                modList = loc.ChildNodes(0).ChildNodes
                                For Each mods In modList
                                    Dim itemID As String = mods.Attributes.GetNamedItem("typeID").Value
                                    Dim itemIDX As Integer = EveHQ.Core.HQ.itemList.IndexOfValue(itemID)
                                    groupID = EveHQ.Core.HQ.typeGroups(itemID).ToString
                                    catID = EveHQ.Core.HQ.groupCats(groupID).ToString
                                    Dim itemName As String = ""
                                    If itemIDX <> -1 Then
                                        itemName = CStr(EveHQ.Core.HQ.itemList.GetKey(itemIDX))
                                    Else
                                        ' Can't find the item in the database
                                        itemName = "ItemID: " & itemID.ToString
                                    End If
                                    Dim flagID As Integer = CInt(mods.Attributes.GetNamedItem("flag").Value)
                                    Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                                    Dim quantity As String = mods.Attributes.GetNamedItem("quantity").Value
                                    HQFShip.Add(flagName & "," & itemName & "," & quantity & "," & catID)
                                Next
                            End If
                            Exit Sub
                        Else
                            ' Check if this row has child nodes and repeat
                            If loc.HasChildNodes = True Then
                                Call Me.SearchForShipNode(loc, assetID)
                                If HQFShip.Count > 0 Then Exit Sub
                            End If
                        End If
                    Next
                End If
            End If
        Next
    End Sub
    Private Sub SearchForShipNode(ByVal loc As XmlNode, ByVal assetID As String)
        Dim subLocList As XmlNodeList
        Dim subLoc As XmlNode
        subLocList = loc.ChildNodes(0).ChildNodes
        For Each subLoc In subLocList
            ' Let's search for our asset!
            Try
                If subLoc.Attributes.GetNamedItem("itemID").Value = assetID Then
                    ' We found our ship so extract the subitem data
                    Dim groupID As String = ""
                    Dim catID As String = ""
                    Dim modList As XmlNodeList
                    Dim mods As XmlNode
                    modList = subLoc.ChildNodes(0).ChildNodes
                    For Each mods In modList
                        Dim itemID As String = mods.Attributes.GetNamedItem("typeID").Value
                        Dim itemIDX As Integer = EveHQ.Core.HQ.itemList.IndexOfValue(itemID)
                        groupID = EveHQ.Core.HQ.typeGroups(itemID).ToString
                        catID = EveHQ.Core.HQ.groupCats(groupID).ToString
                        Dim itemName As String = ""
                        If itemIDX <> -1 Then
                            itemName = CStr(EveHQ.Core.HQ.itemList.GetKey(itemIDX))
                        Else
                            ' Can't find the item in the database
                            itemName = "ItemID: " & itemID.ToString
                        End If
                        Dim flagID As Integer = CInt(mods.Attributes.GetNamedItem("flag").Value)
                        Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                        Dim quantity As String = mods.Attributes.GetNamedItem("quantity").Value
                        HQFShip.Add(flagName & "," & itemName & "," & quantity & "," & catID)
                    Next
                    Exit Sub
                Else
                    ' Check if this row has child nodes and repeat
                    If subLoc.HasChildNodes = True Then
                        Call Me.SearchForShipNode(subLoc, assetID)
                    End If
                End If
            Catch ex As Exception
                Dim msg As String = "Unable to parse Asset:" & ControlChars.CrLf
                msg &= "InnerXML: " & subLoc.InnerXml & ControlChars.CrLf
                msg &= "InnerText: " & subLoc.InnerText & ControlChars.CrLf
                msg &= "TypeID: " & subLoc.Attributes.GetNamedItem("typeID").Value
                MessageBox.Show(msg, "Error Getting Ship Information!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        Next
    End Sub
    Private Sub tlvAssets_SelectedItemsChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tlvAssets.SelectedItemsChanged
        If tlvAssets.SelectedItems.Count > 0 Then
            Dim volume, value, quantity As Double
            For Each asset As ContainerListViewItem In tlvAssets.SelectedItems
                If asset.SubItems(7).Text <> "" Then
                    volume += CDbl(asset.SubItems(6).Text)
                    quantity += CDbl(asset.SubItems(7).Text)
                    value += CDbl(asset.SubItems(9).Text)
                End If
            Next
            tssLabelSelectedAssets.Text = "Volume = " & FormatNumber(volume, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m³ : Value = " & FormatNumber(value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ISK"
        Else
            tssLabelSelectedAssets.Text = ""
        End If
    End Sub

#End Region

#Region "Toolbar Menu Routines"
    Private Sub tsbDownloadAssets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDownloadData.Click
        ' Set the Corp Reps to Default
        CorpReps.Clear()
        For API As Integer = 0 To 6
            CorpReps.Add(New SortedList)
        Next
        ' Flick to the API Status tab
        tabPrism.SelectTab(tabAssetsAPI)
        ' Delete the current API Status data
        For Each Owner As ListViewItem In lvwCurrentAPIs.Items
            For si As Integer = 2 To 8
                Owner.SubItems(si).Text = ""
            Next
        Next
        'Call Me.GetAssets()
        Call Me.GetXMLData()
        cboPilots.SelectedItem = EveHQ.Core.HQ.myPilot.Name
    End Sub
    Private Sub tsbDownloadOutposts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDownloadOutposts.Click
        Call Me.GetOutposts()
    End Sub
    Private Sub tsbRefreshAssets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRefreshAssets.Click
        Dim minValue As Double
        If Double.TryParse(txtMinSystemValue.Text, minValue) = True Then
            Call Me.RefreshAssets()
        Else
            MessageBox.Show("Minimum System Value is not a valid number. Please try again!", "Error in Minimum Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub RefreshAssets()
        If lvwCharFilter.CheckedItems.Count > 0 Then
            ' Set search variables
            If txtSearch.Text <> "" Then
                searchText = txtSearch.Text
            Else
                searchText = ""
            End If
            ' Populate the assets list
            Call Me.PopulateAssets()
            ' Flip to the assets tab to see it!
            tabPrism.SelectedTab = tabAssets
        Else
            MessageBox.Show("Please select an Asset Owner before continuing!", "Please Select Asset Owner", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Private Sub mnuLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLocation.Click
        Dim strHTML As New StringBuilder
        strHTML.Append(HTMLHeader("Assets By Location", False))
        strHTML.Append(HTMLTitle("Assets By Location", False))
        strHTML.Append(AssetLocationReport())
        strHTML.Append(HTMLFooter(False))
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\AssetLocations.html")

        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.reportFolder & "\AssetLocations.html") = True Then
            Try
                Process.Start(EveHQ.Core.HQ.reportFolder & "\AssetLocations.html")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        Else
            MessageBox.Show("Unable to locate the Asset Location file, please try again!", "Error Finding File")
        End If

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Private Sub mnuAssetListName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAssetListName.Click
        Dim strHTML As New StringBuilder
        strHTML.Append(HTMLHeader("Asset List", False))
        strHTML.Append(HTMLTitle("Asset List", False))
        strHTML.Append(AssetListReportByName())
        strHTML.Append(HTMLFooter(False))
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\AssetList.html")

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.reportFolder & "\AssetList.html") = True Then
            Try
                Process.Start(EveHQ.Core.HQ.reportFolder & "\AssetList.html")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

        Else
            MessageBox.Show("Unable to locate the Asset List file, please try again!", "Error Finding File")
        End If

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Private Sub mnuAssetListQuantityA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAssetListQuantityA.Click
        Dim strHTML As New StringBuilder
        strHTML.Append(HTMLHeader("Asset List (By Ascending Quantity)", False))
        strHTML.Append(HTMLTitle("Asset List (By Ascending Quantity)", False))
        strHTML.Append(AssetListReportByNumeric(1, False))
        strHTML.Append(HTMLFooter(False))
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\AssetListQuantityA.html")

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.reportFolder & "\AssetListQuantityA.html") = True Then
            Try
                Process.Start(EveHQ.Core.HQ.reportFolder & "\AssetListQuantityA.html")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

        Else
            MessageBox.Show("Unable to locate the Asset Quantity file, please try again!", "Error Finding File")
        End If

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Private Sub mnuAssetListQuantityD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAssetListQuantityD.Click
        Dim strHTML As New StringBuilder
        strHTML.Append(HTMLHeader("Asset List (By Descending Quantity)", False))
        strHTML.Append(HTMLTitle("Asset List (By Descending Quantity)", False))
        strHTML.Append(AssetListReportByNumeric(1, True))
        strHTML.Append(HTMLFooter(False))
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\AssetListQuantityD.html")

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.reportFolder & "\AssetListQuantityD.html") = True Then
            Try
                Process.Start(EveHQ.Core.HQ.reportFolder & "\AssetListQuantityD.html")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

        Else
            MessageBox.Show("Unable to locate the Asset Quantity file, please try again!", "Error Finding File")
        End If

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Private Sub mnuAssetListPriceA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAssetListPriceA.Click
        Dim strHTML As New StringBuilder
        strHTML.Append(HTMLHeader("Asset List (By Ascending Unit Price)", False))
        strHTML.Append(HTMLTitle("Asset List (By Ascending Unit Price)", False))
        strHTML.Append(AssetListReportByNumeric(2, False))
        strHTML.Append(HTMLFooter(False))
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\AssetListPriceA.html")

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.reportFolder & "\AssetListPriceA.html") = True Then
            Try
                Process.Start(EveHQ.Core.HQ.reportFolder & "\AssetListPriceA.html")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

        Else
            MessageBox.Show("Unable to locate the Asset Price file, please try again!", "Error Finding File")
        End If

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Private Sub mnuAssetListPriceD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAssetListPriceD.Click
        Dim strHTML As New StringBuilder
        strHTML.Append(HTMLHeader("Asset List (By Descending Unit Price)", False))
        strHTML.Append(HTMLTitle("Asset List (By Descending Unit Price)", False))
        strHTML.Append(AssetListReportByNumeric(2, True))
        strHTML.Append(HTMLFooter(False))
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\AssetListPriceD.html")

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.reportFolder & "\AssetListPriceD.html") = True Then
            Try
                Process.Start(EveHQ.Core.HQ.reportFolder & "\AssetListPriceD.html")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

        Else
            MessageBox.Show("Unable to locate the Asset Price file, please try again!", "Error Finding File")
        End If

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Private Sub mnuAssetListValueA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAssetListValueA.Click
        Dim strHTML As New StringBuilder
        strHTML.Append(HTMLHeader("Asset List (By Ascending Value)", False))
        strHTML.Append(HTMLTitle("Asset List (By Ascending Value)", False))
        strHTML.Append(AssetListReportByNumeric(3, False))
        strHTML.Append(HTMLFooter(False))
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\AssetListValueA.html")

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.reportFolder & "\AssetListValueA.html") = True Then
            Try
                Process.Start(EveHQ.Core.HQ.reportFolder & "\AssetListValueA.html")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

        Else
            MessageBox.Show("Unable to locate the Asset Value file, please try again!", "Error Finding File")
        End If

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Private Sub mnuAssetListValueD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAssetListValueD.Click
        Dim strHTML As New StringBuilder
        strHTML.Append(HTMLHeader("Asset List (By Descending Value)", False))
        strHTML.Append(HTMLTitle("Asset List (By Descending Value)", False))
        strHTML.Append(AssetListReportByNumeric(3, True))
        strHTML.Append(HTMLFooter(False))
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\AssetListValueD.html")

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.reportFolder & "\AssetListValueD.html") = True Then
            Try
                Process.Start(EveHQ.Core.HQ.reportFolder & "\AssetListValueD.html")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        Else
            MessageBox.Show("Unable to locate the Asset Value file, please try again!", "Error Finding File")
        End If

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call Me.RefreshAssets()
        End If
    End Sub
    Private Sub txtMinSystemValue_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMinSystemValue.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim minValue As Double
            If Double.TryParse(txtMinSystemValue.Text, minValue) = True Then
                Call Me.RefreshAssets()
            Else
                MessageBox.Show("Minimum System Value is not a valid number. Please try again!", "Error in Minimum Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

#End Region

#Region "Rig Builder Routines"
    Private Sub btnGetSalvage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call Me.GetSalvage()
    End Sub
    Private Sub GetSalvage()

        SalvageList = New SortedList

        Dim assetOwner As String = ""
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            ' Check in the cache folder for a valid file
            Dim selPilot As EveHQ.Core.Pilot = CType(loadedOwners(cPilot.Text), Core.Pilot)
            Dim accountName As String = selPilot.Account
            Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            Dim fileName As String = ""
            If cPilot.Text = selPilot.Corp Then
                fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_15_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                assetOwner = selPilot.Corp
            Else
                fileName = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_14_" & pilotAccount.userID & "_" & selPilot.ID & ".xml"
                assetOwner = selPilot.Name
            End If
            If My.Computer.FileSystem.FileExists(fileName) = False Then
                MessageBox.Show("Unable to load assets file for " & selPilot.Name & ".", "Error Loading Assets", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            ' File found so lets see what we have!
            Dim assetXML As New XmlDocument
            assetXML.Load(fileName)
            Dim locList As XmlNodeList
            Dim loc As XmlNode
            locList = assetXML.SelectNodes("/eveapi/result/rowset/row")
            If locList.Count > 0 Then
                For Each loc In locList
                    Dim itemID As String = loc.Attributes.GetNamedItem("typeID").Value
                    If EveHQ.Core.HQ.typeGroups.Contains(itemID) = True Then
                        Dim groupID As String = EveHQ.Core.HQ.typeGroups(itemID).ToString
                        If CLng(groupID) = 754 Then

                            Dim quantity As Long = CLng(loc.Attributes.GetNamedItem("quantity").Value)
                            Dim itemIDX As Integer = EveHQ.Core.HQ.itemList.IndexOfValue(itemID)
                            Dim itemName As String = CStr(EveHQ.Core.HQ.itemList.GetKey(itemIDX))
                            If SalvageList.Contains(itemName) = False Then
                                SalvageList.Add(itemName, quantity)
                            Else
                                SalvageList.Item(itemName) = CLng(SalvageList.Item(itemName)) + quantity
                            End If
                        End If
                    End If

                    ' Check if this row has child nodes and repeat
                    If loc.HasChildNodes = True Then
                        Call Me.GetSalvageNode(SalvageList, loc, assetOwner, selPilot)
                    End If
                Next
            End If
        Next

    End Sub
    Private Sub GetSalvageNode(ByVal SalvageList As SortedList, ByVal loc As XmlNode, ByVal assetOwner As String, ByVal selPilot As EveHQ.Core.Pilot)
        Dim subLocList As XmlNodeList
        Dim subLoc As XmlNode
        subLocList = loc.ChildNodes(0).ChildNodes
        For Each subLoc In subLocList
            Try
                Dim itemID As String = subLoc.Attributes.GetNamedItem("typeID").Value
                If EveHQ.Core.HQ.typeGroups.Contains(itemID) = True Then
                    Dim groupID As String = EveHQ.Core.HQ.typeGroups(itemID).ToString
                    If CLng(groupID) = 754 Then
                        Dim quantity As Long = CLng(subLoc.Attributes.GetNamedItem("quantity").Value)
                        Dim itemIDX As Integer = EveHQ.Core.HQ.itemList.IndexOfValue(itemID)
                        Dim itemName As String = CStr(EveHQ.Core.HQ.itemList.GetKey(itemIDX))
                        If SalvageList.Contains(itemName) = False Then
                            SalvageList.Add(itemName, quantity)
                        Else
                            SalvageList.Item(itemName) = CLng(SalvageList.Item(itemName)) + quantity
                        End If
                    End If
                End If

                If subLoc.HasChildNodes = True Then
                    Call Me.GetSalvageNode(SalvageList, subLoc, assetOwner, selPilot)
                End If

            Catch ex As Exception
                Dim msg As String = "Unable to parse Asset:" & ControlChars.CrLf
                msg &= "InnerXML: " & subLoc.InnerXml & ControlChars.CrLf
                msg &= "InnerText: " & subLoc.InnerText & ControlChars.CrLf
                msg &= "TypeID: " & subLoc.Attributes.GetNamedItem("typeID").Value
                MessageBox.Show(msg, "Error Parsing Assets File For " & assetOwner, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        Next
    End Sub
    Private Sub btnBuildRigs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuildRigs.Click

        ' Get the rig and salvage info
        Call Me.PrepareRigData()

        ' Get the list of available rigs
        Call Me.GetBuildList()

    End Sub
    Private Sub PrepareRigData()
        ' Clear the build list
        lvwRigBuildList.Items.Clear()
        lvwRigs.Sort(0, SortOrder.Ascending, True)

        ' Build a Salvage List
        Call Me.GetSalvage()

        ' Calculate the true Waste Factor
        Dim BPWF As Double = 10
        If nudRigMELevel.Value >= 0 Then
            BPWF = 1 + ((1 / BPWF) / (1 + nudRigMELevel.Value))
        Else
            BPWF = 1 + ((1 / BPWF) * (1 - nudRigMELevel.Value))
        End If

        RigBPData = New SortedList
        RigBuildData = New SortedList

        ' Get the BP Details and build requirements
        Dim strSQL As String = "SELECT typeActivityMaterials.typeID AS typeActivityMaterials_typeID, typeActivityMaterials.activityID, typeActivityMaterials.requiredTypeID, typeActivityMaterials.quantity, typeActivityMaterials.damagePerJob, invTypes.typeID AS invTypes_typeID, invTypes.groupID, invTypes.published"
        strSQL &= " FROM invTypes INNER JOIN typeActivityMaterials ON invTypes.typeID = typeActivityMaterials.typeID"
        strSQL &= " WHERE (((typeActivityMaterials.activityID)=1) AND ((invTypes.groupID)=787) AND ((invTypes.published)=1));"
        Dim rigData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Dim BPID As String = ""
        Dim BPIDX As Integer = 0
        Dim BPName As String = ""
        Dim SalvageID As String = ""
        Dim SalvageIDX As Integer = 0
        Dim SalvageName As String = ""
        Dim SalvageQ As Double = 0
        Dim groupID As String = ""
        For Each rigRow As DataRow In rigData.Tables(0).Rows
            BPID = rigRow.Item("invTypes_typeID").ToString
            BPIDX = EveHQ.Core.HQ.itemList.IndexOfValue(BPID)
            BPName = CStr(EveHQ.Core.HQ.itemList.GetKey(BPIDX)).TrimEnd(" Blueprint".ToCharArray)
            ' Add it to the BPList if not already in
            If RigBPData.Contains(BPName) = False Then
                RigBPData.Add(BPName, New SortedList)
            End If
            ' Read the required type and see if it is salvage (read groupID = 754)
            SalvageID = rigRow.Item("requiredTypeID").ToString
            groupID = EveHQ.Core.HQ.typeGroups(SalvageID).ToString
            If groupID = "754" Then
                SalvageIDX = EveHQ.Core.HQ.itemList.IndexOfValue(SalvageID)
                SalvageName = CStr(EveHQ.Core.HQ.itemList.GetKey(SalvageIDX))
                SalvageQ = Math.Round(CDbl(rigRow.Item("quantity")) * BPWF, 0)
                RigBuildData = CType(RigBPData.Item(BPName), Collections.SortedList)
                RigBuildData.Add(SalvageName, SalvageQ)
            End If
        Next

    End Sub
    Private Sub GetBuildList()
        Dim buildableBP As Boolean = False
        Dim material As String = ""
        Dim minQuantity As Double = 1.0E+99
        Dim buildCost As Double = 0
        Dim rigCost As Double = 0
        lvwRigs.BeginUpdate()
        lvwRigs.Items.Clear()
        For Each BP As String In RigBPData.Keys
            buildableBP = True
            minQuantity = 1.0E+99
            buildCost = 0
            ' Fetch the build requirements
            RigBuildData = CType(RigBPData(BP), Collections.SortedList)
            ' Go through the requirements and see if have sufficient materials
            For Each material In RigBuildData.Keys
                If SalvageList.Contains(material) = True Then
                    ' Check quantity
                    If CDbl(SalvageList(material)) > CDbl(RigBuildData(material)) Then
                        ' We have enough so let's calculate the quantity we can use
                        minQuantity = Math.Min(minQuantity, (CDbl(SalvageList(material)) / CDbl(RigBuildData(material))))
                    Else
                        ' We are lacking
                        buildableBP = False
                        Exit For
                    End If
                Else
                    buildableBP = False
                    Exit For
                End If
            Next
            ' Find the results
            If buildableBP = True Then
                ' Caluclate the build cost
                For Each material In RigBuildData.Keys
                    ' Get price
                    buildCost += CInt(RigBuildData(material)) * EveHQ.Core.DataFunctions.GetPrice(CStr(EveHQ.Core.HQ.itemList(material)))
                Next
                rigCost = EveHQ.Core.DataFunctions.GetPrice(CStr(EveHQ.Core.HQ.itemList(BP)))
                Dim lviBP2 As New ContainerListViewItem
                lviBP2.Text = BP
                lvwRigs.Items.Add(lviBP2)
                lviBP2.SubItems(1).Text = (FormatNumber((Int(minQuantity)), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lviBP2.SubItems(2).Text = (FormatNumber(rigCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lviBP2.SubItems(3).Text = (FormatNumber(buildCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lviBP2.SubItems(4).Text = (FormatNumber(rigCost - buildCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lviBP2.SubItems(5).Text = (FormatNumber(Int(minQuantity) * rigCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lviBP2.SubItems(6).Text = (FormatNumber(Int(minQuantity) * buildCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lviBP2.SubItems(7).Text = (FormatNumber(Int(minQuantity) * (rigCost - buildCost), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lviBP2.SubItems(8).Text = (FormatNumber((Int(minQuantity) * (rigCost - buildCost)) / (Int(minQuantity) * rigCost) * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            End If
        Next
        lvwRigs.Sort(False)
        lvwRigs.EndUpdate()
    End Sub
    Private Sub lvwRigs_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRigs.DoubleClick
        If lvwRigs.SelectedItems.Count > 0 Then
            Call AddRigToBuildList(lvwRigs.SelectedItems(0))
            Call Me.GetBuildList()
            Call Me.CalculateRigBuildInfo()
        End If
    End Sub
    Private Sub lvwRigBuildList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRigBuildList.DoubleClick
        If lvwRigBuildList.SelectedItems.Count > 0 Then
            Call RemoveRigFromBuildList(lvwRigBuildList.SelectedItems(0))
            ' Recalculate the salvage available
            Call Me.GetBuildList()
            Call Me.CalculateRigBuildInfo()
        End If
    End Sub
    Private Sub AddRigToBuildList(ByVal currentRig As ContainerListViewItem)
        Dim newRig As New ContainerListViewItem(currentRig.Text)
        ' Add the selected rig to the build list
        lvwRigBuildList.Items.Add(newRig)
        ' Copy details (the inbuilt clone fails!)
        For subI As Integer = 1 To currentRig.SubItems.Count - 1
            newRig.SubItems(subI).Text = currentRig.SubItems(subI).Text
        Next
        'Get the salvage used by the rig and reduce the main list
        Dim RigSalvageList As SortedList = CType(RigBPData(currentRig.Text), Collections.SortedList)
        For Each salvage As String In RigSalvageList.Keys
            SalvageList(salvage) = CInt(SalvageList(salvage)) - (CInt(RigSalvageList(salvage)) * CInt(currentRig.SubItems(1).Text))
        Next
    End Sub
    Private Sub RemoveRigFromBuildList(ByVal currentRig As ContainerListViewItem)
        ' Remove the selected rig to the build list
        lvwRigBuildList.Items.Remove(currentRig)
        ' Get the salvage used by the rig and reduce the main list
        Dim RigSalvageList As SortedList = CType(RigBPData(currentRig.Text), Collections.SortedList)
        For Each salvage As String In RigSalvageList.Keys
            SalvageList(salvage) = CInt(SalvageList(salvage)) + (CInt(RigSalvageList(salvage)) * CInt(currentRig.SubItems(1).Text))
        Next
    End Sub
    Private Sub btnAutoRig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoRig.Click
        ' Get the rig and salvage info
        Call Me.PrepareRigData()
        ' Get the list of available rigs
        Call Me.GetBuildList()
        Do While lvwRigs.Items.Count > 0
            lvwRigs.Sort(CInt(btnAutoRig.Tag), SortOrder.Descending, True)
            AddRigToBuildList(lvwRigs.Items(0))
            Call Me.GetBuildList()
        Loop
        lvwRigBuildList.Sort(CInt(btnAutoRig.Tag), SortOrder.Descending, True)
        Call Me.CalculateRigBuildInfo()
    End Sub
    Private Sub radRigSaleprice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radRigSaleprice.CheckedChanged
        btnAutoRig.Tag = 2
    End Sub
    Private Sub radRigProfit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radRigProfit.CheckedChanged
        btnAutoRig.Tag = 4
    End Sub
    Private Sub radRigMargin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radRigMargin.CheckedChanged
        btnAutoRig.Tag = 8
    End Sub
    Private Sub radTotalSalePrice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radTotalSalePrice.CheckedChanged
        btnAutoRig.Tag = 5
    End Sub
    Private Sub radTotalProfit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radTotalProfit.CheckedChanged
        btnAutoRig.Tag = 7
    End Sub
    Private Sub CalculateRigBuildInfo()
        Dim totalRSP, totalRP As Double
        For Each rigItem As ContainerListViewItem In lvwRigBuildList.Items
            totalRSP += CDbl(rigItem.SubItems(5).Text)
            totalRP += CDbl(rigItem.SubItems(7).Text)
        Next
        lblTotalRigSalePrice.Text = "Total Rig Sale Price: " & FormatNumber(totalRSP, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblTotalRigProfit.Text = "Total Rig Profit: " & FormatNumber(totalRP, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblTotalRigMargin.Text = "Margin: " & FormatNumber(totalRP / totalRSP * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
    End Sub

#End Region

#Region "Item Recycler Routines"

    Private Sub mnuRecycleItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRecycleItem.Click
        Dim recycleList As New SortedList
        tempAssetList.Clear()
        For Each asset As ContainerListViewItem In tlvAssets.SelectedItems
            If recycleList.ContainsKey(EveHQ.Core.HQ.itemList(asset.Text)) = True Then
                If asset.SubItems(7).Text <> "" Then
                    If tempAssetList.Contains(asset.Tag.ToString) = False Then
                        recycleList(EveHQ.Core.HQ.itemList(asset.Text)) = CLng(recycleList(EveHQ.Core.HQ.itemList(asset.Text))) + CLng(asset.SubItems(7).Text)
                        tempAssetList.Add(asset.Tag.ToString)
                    End If
                End If
            Else
                If asset.SubItems(7).Text <> "" Then
                    If tempAssetList.Contains(asset.Tag.ToString) = False Then
                        recycleList.Add(EveHQ.Core.HQ.itemList(asset.Text), CLng(asset.SubItems(7).Text))
                        tempAssetList.Add(asset.Tag.ToString)
                    End If
                End If
            End If
        Next
        tempAssetList.Clear()
        Dim myRecycler As New frmRecycleAssets
        myRecycler.AssetList = recycleList
        myRecycler.AssetOwner = cboPilots.SelectedItem.ToString
        myRecycler.AssetLocation = GetLocationID(tlvAssets.SelectedItems(0))
        myRecycler.ShowDialog()
        myRecycler.Dispose()
    End Sub

    Private Sub mnuRecycleContained_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRecycleContained.Click
        Dim recycleList As New SortedList
        tempAssetList.Clear()
        For Each asset As ContainerListViewItem In tlvAssets.SelectedItems
            Call Me.AddItemsToRecycleList(asset, recycleList)
        Next
        tempAssetList.Clear()
        Dim myRecycler As New frmRecycleAssets
        myRecycler.AssetList = recycleList
        myRecycler.AssetOwner = cboPilots.SelectedItem.ToString
        myRecycler.AssetLocation = GetLocationID(tlvAssets.SelectedItems(0))
        myRecycler.ShowDialog()
        myRecycler.Dispose()
    End Sub

    Private Sub mnuRecycleAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRecycleAll.Click
        Dim recycleList As New SortedList
        tempAssetList.Clear()
        For Each asset As ContainerListViewItem In tlvAssets.SelectedItems
            If EveHQ.Core.HQ.itemList.Contains(asset.Text) = True Then
                If recycleList.ContainsKey(EveHQ.Core.HQ.itemList(asset.Text)) = True Then
                    If asset.SubItems(7).Text <> "" Then
                        If tempAssetList.Contains(asset.Tag.ToString) = False Then
                            recycleList(EveHQ.Core.HQ.itemList(asset.Text)) = CLng(recycleList(EveHQ.Core.HQ.itemList(asset.Text))) + CLng(asset.SubItems(7).Text)
                            tempAssetList.Add(asset.Tag.ToString)
                        End If
                    End If
                Else
                    If asset.SubItems(7).Text <> "" Then
                        If tempAssetList.Contains(asset.Tag.ToString) = False Then
                            recycleList.Add(EveHQ.Core.HQ.itemList(asset.Text), CLng(asset.SubItems(7).Text))
                            tempAssetList.Add(asset.Tag.ToString)
                        End If
                    End If
                End If
            End If
            Call Me.AddItemsToRecycleList(asset, recycleList)
        Next
        tempAssetList.Clear()
        Dim myRecycler As New frmRecycleAssets
        myRecycler.AssetList = recycleList
        myRecycler.AssetOwner = cboPilots.SelectedItem.ToString
        myRecycler.AssetLocation = GetLocationID(tlvAssets.SelectedItems(0))
        myRecycler.ShowDialog()
        myRecycler.Dispose()
    End Sub

    Private Sub AddItemsToRecycleList(ByVal item As ContainerListViewItem, ByRef assetList As SortedList)
        For Each childItem As ContainerListViewItem In item.Items
            If EveHQ.Core.HQ.itemList.Contains(childItem.Text) = True Then
                If assetList.ContainsKey(EveHQ.Core.HQ.itemList(childItem.Text)) = True Then
                    If childItem.SubItems(7).Text <> "" Then
                        If tempAssetList.Contains(childItem.Tag.ToString) = False Then
                            assetList(EveHQ.Core.HQ.itemList(childItem.Text)) = CLng(assetList(EveHQ.Core.HQ.itemList(childItem.Text))) + CLng(childItem.SubItems(7).Text)
                            tempAssetList.Add(childItem.Tag.ToString)
                        End If
                    End If
                Else
                    If childItem.SubItems(7).Text <> "" Then
                        If tempAssetList.Contains(childItem.Tag.ToString) = False Then
                            assetList.Add(EveHQ.Core.HQ.itemList(childItem.Text), CLng(childItem.SubItems(7).Text))
                            tempAssetList.Add(childItem.Tag.ToString)
                        End If
                    End If
                End If
            End If
            If childItem.Items.Count > 0 Then
                Call Me.AddItemsToRecycleList(childItem, assetList)
            End If
        Next
    End Sub

    Private Function GetLocationID(ByVal item As ContainerListViewItem) As String
        Do While item.ParentItem.ParentItem IsNot Nothing
            item = item.ParentItem
        Loop
        Return item.Tag.ToString
    End Function

#End Region

#Region "Market Orders Routines"

    Private Sub ParseOrders()
        Dim IsCorp As Boolean = False
        ' Get the owner we will use
        Dim owner As String = cboPilots.SelectedItem.ToString
        ' See if this owner is a corp
        If CorpList.ContainsKey(owner) = True Then
            IsCorp = True
            ' See if we have a representative
            Dim CorpRep As SortedList = CType(CorpReps(4), Collections.SortedList)
            If CorpRep.ContainsKey(CStr(CorpList(owner))) = True Then
                owner = CStr(CorpRep(CStr(CorpList(owner))))
            Else
                owner = ""
            End If
        End If

        If owner <> "" Then
            Dim sellTotal, buyTotal, TotalEscrow As Double
            Dim TotalOrders As Integer = 0
            Dim OrderXML As New XmlDocument
            Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
            Dim accountName As String = selPilot.Account
            Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            If IsCorp = True Then
                OrderXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersCorp, pilotAccount, selPilot.ID, True)
            Else
                OrderXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersChar, pilotAccount, selPilot.ID, True)
            End If
            Dim Orders As XmlNodeList = OrderXML.SelectNodes("/eveapi/result/rowset/row")
            If Orders IsNot Nothing Then
                For Each Order As XmlNode In Orders

                    If Order.Attributes.GetNamedItem("bid").Value = "0" = False Then
                        If Order.Attributes.GetNamedItem("orderState").Value = "0" Then
                            Dim bOrder As New ListViewItem
                            Dim itemID As String = Order.Attributes.GetNamedItem("typeID").Value
                            Dim itemName As String = CType(PlugInData.Items(itemID), Prism.ItemData).Name
                            bOrder.Text = itemName
                            Dim quantity As Double = CDbl(Order.Attributes.GetNamedItem("volRemaining").Value)
                            bOrder.SubItems.Add(FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            bOrder.SubItems.Add(FormatNumber(Order.Attributes.GetNamedItem("price").Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim loc As String = CStr(PlugInData.stations(Order.Attributes.GetNamedItem("stationID").Value))
                            bOrder.SubItems.Add(loc)
                            Dim issueDate As Date = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, Nothing, Globalization.DateTimeStyles.None)
                            Dim orderExpires As TimeSpan = issueDate - Now
                            orderExpires = orderExpires.Add(New TimeSpan(CInt(Order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                            If orderExpires.TotalSeconds <= 0 Then
                                bOrder.SubItems.Add("Expired!")
                            Else
                                bOrder.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False))
                            End If
                            sellTotal = sellTotal + quantity * CDbl(Order.Attributes.GetNamedItem("price").Value)
                            TotalOrders = TotalOrders + 1
                            lvwSellOrders.Items.Add(bOrder)
                        End If
                    Else
                        If Order.Attributes.GetNamedItem("orderState").Value = "0" Then
                            Dim sOrder As New ListViewItem
                            Dim itemID As String = Order.Attributes.GetNamedItem("typeID").Value
                            Dim itemName As String = CType(PlugInData.Items(itemID), Prism.ItemData).Name
                            sOrder.Text = itemName
                            Dim quantity As Double = CDbl(Order.Attributes.GetNamedItem("volRemaining").Value)
                            sOrder.SubItems.Add(FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            sOrder.SubItems.Add(FormatNumber(Order.Attributes.GetNamedItem("price").Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim loc As String = CStr(PlugInData.stations(Order.Attributes.GetNamedItem("stationID").Value))
                            sOrder.SubItems.Add(loc)
                            Select Case CInt(Order.Attributes.GetNamedItem("range").Value)
                                Case -1
                                    sOrder.SubItems.Add("Station")
                                Case 0
                                    sOrder.SubItems.Add("System")
                                Case 32767
                                    sOrder.SubItems.Add("Region")
                                Case Is > 0, Is < 32767
                                    sOrder.SubItems.Add(Order.Attributes.GetNamedItem("range").Value & " Jumps")
                            End Select
                            sOrder.SubItems.Add(FormatNumber(CDbl(Order.Attributes.GetNamedItem("minVolume").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Dim issueDate As Date = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, Nothing, Globalization.DateTimeStyles.None)
                            Dim orderExpires As TimeSpan = issueDate - Now
                            orderExpires = orderExpires.Add(New TimeSpan(CInt(Order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                            If orderExpires.TotalSeconds <= 0 Then
                                sOrder.SubItems.Add("Expired!")
                            Else
                                sOrder.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False))
                            End If
                            buyTotal = buyTotal + quantity * CDbl(Order.Attributes.GetNamedItem("price").Value)
                            TotalEscrow = TotalEscrow + CDbl(Order.Attributes.GetNamedItem("escrow").Value)
                            TotalOrders = TotalOrders + 1
                            lvwBuyOrders.Items.Add(sOrder)
                        End If
                    End If
                Next
            End If

            Dim maxorders As Integer = 5 + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Trade)) * 4) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Tycoon)) * 32) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Retail)) * 8) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Wholesale)) * 16)
            Dim cover As Double = buyTotal - TotalEscrow
            Dim TransTax As Double = 1 * (1 - 0.1 * (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Accounting))))
            Dim BrokerFee As Double = 1 * (1 - 0.05 * (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BrokerRelations))))
            lblorders.Text = Str(maxorders - TotalOrders) + "/" + Str(maxorders)
            lblselltotal.Text = FormatNumber(Str(sellTotal), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " isk"
            lblbuytotal.Text = FormatNumber(Str(buyTotal), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " isk"
            lblescrow.Text = FormatNumber(Str(TotalEscrow), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " isk (additional " + FormatNumber(Str(cover), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + " isk to cover)"
            lblask.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Procurement)))
            lblbid.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Marketing)))
            lblmod.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Daytrading)))
            lblremote.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Visibility)))
            lblbroker.Text = FormatNumber(BrokerFee, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "%"
            lbltax.Text = FormatNumber(TransTax, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "%"

        End If

    End Sub
    Private Function GetOrderRange(ByVal lvl As Integer) As String
        Select Case lvl
            Case 0
                Return "limited to stations"
            Case 1
                Return "limited to system"
            Case 2
                Return "limited to 5 Jumps"
            Case 3
                Return "limited to 10 Jumps"
            Case 4
                Return "limited to 20 Jumps"
            Case Else
                Return "limited to Region"
        End Select
    End Function
#End Region



End Class

