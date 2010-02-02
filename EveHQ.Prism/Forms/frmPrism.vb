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

    Dim startup As Boolean = True
    Dim filters As New ArrayList
    Dim catFilters As New ArrayList
    Dim groupFilters As New ArrayList
    Dim loadedOwners As New SortedList
    Dim ProcessXMLCount As Integer = 0
    Dim ProcessXMLMax As Integer = 0
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
    Dim assetCorpMode As Boolean = False
    Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    ' Investment variables
    Dim InvestmentID As String

    ' Rig Builder Variables
    Dim RigBPData As New SortedList
    Dim RigBuildData As New SortedList
    Dim SalvageList As New SortedList

    ' Recycling Variables
    Dim RecyclerAssetList As New SortedList
    Dim RecyclerAssetOwner As String = ""
    Dim RecyclerAssetLocation As String = ""
    Dim itemList As New SortedList
    Dim matList As New SortedList
    Dim BaseYield As Double = 0.5
    Dim NetYield As Double = 0
    Dim StationYield As Double = 0
    Dim StationTake As Double = 0
    Dim StationStanding As Double = 0
    Dim RBrokerFee As Double = 0
    Dim RTransTax As Double = 0
    Dim RTotalFees As Double = 0

    Delegate Sub CheckXMLDelegate(ByVal apiXML As XmlDocument, ByVal Owner As String, ByVal Primary As String, ByVal Pos As Integer)
    Private XMLDelegate As CheckXMLDelegate

#End Region

#Region "Form Initialisation Routines"

    Private Sub frmPrism_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        XMLDelegate = New CheckXMLDelegate(AddressOf CheckXML)

        startup = True

        ' Build a corp list
        Call Me.BuildCorpList()

        ' Set the Corp Reps to Default
        PlugInData.CorpReps.Clear()
        For API As Integer = 0 To 6
            PlugInData.CorpReps.Add(New SortedList)
        Next

        ' Remove excess tabs
        tabPrism.TabPages.Remove(tabAssets)
        tabPrism.TabPages.Remove(tabAssetFilters)
        tabPrism.TabPages.Remove(tabInvestments)
        tabPrism.TabPages.Remove(tabBPManager)
        tabPrism.TabPages.Remove(tabRigBuilder)
        tabPrism.TabPages.Remove(tabOrders)
        tabPrism.TabPages.Remove(tabTransactions)
        tabPrism.TabPages.Remove(tabJournal)
        tabPrism.TabPages.Remove(tabJobs)
        tabPrism.TabPages.Remove(tabRecycle)

        Call Me.LoadInvestments()
        Call Me.LoadFilterGroups()
        Call Portfolio.SetupTypes()
        Call Me.ScanForExistingXMLs()

        ' Set the refining info
        ' Set the pilot to the recycling one
        If cboRecyclePilots.Items.Contains(RecyclerAssetOwner) Then
            cboRecyclePilots.SelectedItem = RecyclerAssetOwner
        Else
            If cboOwner.SelectedItem IsNot Nothing Then
                If cboRecyclePilots.Items.Contains(cboOwner.SelectedItem.ToString) Then
                    cboRecyclePilots.SelectedItem = cboOwner.SelectedItem.ToString
                Else
                    cboRecyclePilots.SelectedIndex = 0
                End If
            Else
                If cboRecyclePilots.Items.Count > 0 Then
                    cboRecyclePilots.SelectedIndex = 0
                End If
            End If
        End If
        ' Set the recycling mode
        cboRefineMode.SelectedIndex = 0
        startup = False

        ' Set the value of the min system value text box
        txtMinSystemValue.Text = FormatNumber(0, 2)

    End Sub
    Private Sub BuildCorpList()
        PlugInData.CorpList.Clear()
        For Each selpilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If PlugInData.CorpList.ContainsKey(selpilot.Corp) = False Then
                PlugInData.CorpList.Add(selpilot.Corp, selpilot.CorpID)
            End If
        Next
    End Sub
    Private Sub LoadFilterGroups()
        Dim newNode As TreeNode
        tvwFilter.BeginUpdate()
        tvwFilter.Nodes.Clear()
        ' Load up the filter with categories
        For Each cat As String In EveHQ.Core.HQ.itemCats.Keys
            newNode = New TreeNode
            newNode.Name = cat
            newNode.Text = EveHQ.Core.HQ.itemCats(cat)
            tvwFilter.Nodes.Add(newNode)
        Next
        ' Load up the filter with groups
        For Each group As String In EveHQ.Core.HQ.itemGroups.Keys
            newNode = New TreeNode
            newNode.Name = group
            newNode.Text = CStr(EveHQ.Core.HQ.itemGroups(group))
            tvwFilter.Nodes(EveHQ.Core.HQ.groupCats(newNode.Name)).Nodes.Add(newNode)
        Next
        ' Update the filter
        tvwFilter.Sorted = True
        tvwFilter.EndUpdate()
    End Sub
    Private Sub ScanForExistingXMLs()

        ' Set the loaded counter to 0
        ProcessXMLCount = 0
        ProcessXMLMax = 0

        lvwCurrentAPIs.BeginUpdate()
        lvwCharFilter.BeginUpdate()
        lvwCurrentAPIs.Items.Clear()
        loadedOwners.Clear()
        cboOwner.Items.Clear()
        lvwCharFilter.Items.Clear()
        Dim fileName As String = ""
        Dim apiXML As New XmlDocument

        ' Get a list of Pilot and Corps
        For Each selpilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selpilot.Active = True Then
                If selpilot.Account <> "" Then
                    If lvwCurrentAPIs.Items.ContainsKey(selpilot.ID) = False Then
                        Dim newOwner As New ListViewItem
                        newOwner.UseItemStyleForSubItems = False
                        newOwner.Name = selpilot.ID
                        newOwner.Text = selpilot.Name
                        newOwner.ToolTipText = ""
                        newOwner.SubItems.Add("Character")
                        For si As Integer = 2 To 8
                            newOwner.SubItems.Add("")
                        Next
                        newOwner.SubItems(8).Text = "n/a"
                        lvwCurrentAPIs.Items.Add(newOwner)
                        loadedOwners.Add(selpilot.Name, selpilot)
                        ProcessXMLMax += 13
                        cboOwner.Items.Add(selpilot.Name)
                        cboRecyclePilots.Items.Add(selpilot.Name)
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
                                cboOwner.Items.Add(selpilot.Corp)
                                lvwCharFilter.Items.Add(newChar)
                            End If
                        End If
                    End If
                End If
            End If
        Next
        lvwCharFilter.EndUpdate()

        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Check for char assets
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 2)

                    ' Check for corp assets
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 2)

                    ' Check for char balances
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 3)

                    ' Check for corp balances
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 3)

                    ' Check for char jobs
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryChar, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 4)

                    ' Check for corp jobs
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryCorp, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 4)

                    ' Check for char journal
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, pilotAccount, selPilot.ID, 1000, "", 1)
                    Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 5)

                    ' Check for corp journal
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp, pilotAccount, selPilot.ID, 1000, "", 1)
                    Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 5)

                    ' Check for char orders
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersChar, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 6)

                    ' Check for corp orders
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersCorp, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 6)

                    ' Check for char transactions
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransChar, pilotAccount, selPilot.ID, 1000, "", 1)
                    Call CheckXML(apiXML, selPilot.ID, selPilot.Name, 7)

                    ' Check for corp transactions
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransCorp, pilotAccount, selPilot.ID, 1000, "", 1)
                    Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 7)

                    ' Check for corp sheets
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpSheet, pilotAccount, selPilot.ID, 1)
                    Call CheckXML(apiXML, selPilot.CorpID, selPilot.Name, 8)

                End If
            End If
        Next
        lvwCurrentAPIs.EndUpdate()

    End Sub

#End Region

#Region "Form Closing Routines"

    Private Sub frmPrism_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Save the current blueprints
        Dim s As New FileStream(Path.Combine(PrismSettings.PrismFolder, "OwnerBlueprints.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, PlugInData.BlueprintAssets)
        s.Flush()
        s.Close()
    End Sub
#End Region

#Region "Enumerators"
    Private Enum AssetColumn
        Name = 0
        Owner = 1
        Group = 2
        Category = 3
        Location = 4
        Meta = 5
        Volume = 6
        Quantity = 7
        Price = 8
        Value = 9
    End Enum
#End Region

#Region "XML Retrieval and Parsing"
    Private Sub GetXMLData()
        ' Count the maximum updates so we know when we're done
        ProcessXMLMax = 0
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    ProcessXMLMax += 14
                End If
            End If
        Next
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
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpTransactions, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetCorpSheet, Nothing)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.UpdateNullCorpSheet, Nothing)
    End Sub
    Private Sub CheckXML(ByVal apiXML As XmlDocument, ByVal Owner As String, ByVal Primary As String, ByVal Pos As Integer)
        Dim APIOwner As ListViewItem = lvwCurrentAPIs.Items(Owner)
        If APIOwner IsNot Nothing Then
            ' Get the corp reps
            Dim CorpRep As SortedList = CType(PlugInData.CorpReps(Pos - 2), Collections.SortedList)
            If apiXML IsNot Nothing Then
                ' If we already have a corp rep, no sense in getting a new one!
                If CorpRep.ContainsKey(Owner) = False Then
                    APIOwner.ToolTipText &= [Enum].GetName(GetType(PrismRepCodes), Pos - 2) & " Rep: " & Primary & ControlChars.CrLf
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
                        APIOwner.SubItems(Pos).Text = FormatDateTime(cache, DateFormat.GeneralDate)
                        CorpRep.Add(Owner, Primary)
                    End If
                End If
            Else
                If CorpRep.ContainsKey(Owner) = False Then
                    APIOwner.ToolTipText &= [Enum].GetName(GetType(PrismRepCodes), Pos - 2) & " Rep: " & Primary & ControlChars.CrLf
                    If Pos = 8 Then
                        APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Black
                        APIOwner.SubItems(Pos).Text = "n/a"
                    Else
                        APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                        APIOwner.SubItems(Pos).Text = "missing"
                    End If
                End If
            End If
        End If
        ProcessXMLCount += 1
        If ProcessXMLCount = ProcessXMLMax Then
            'cboOwner.SelectedItem = EveHQ.Core.HQ.myPilot.Name
            If cboOwner.SelectedItem IsNot Nothing Then
                If cboOwner.SelectedItem.ToString = EveHQ.Core.HQ.EveHQSettings.StartupPilot Then
                    ' Just refresh
                    Call Me.UpdatePrismInfo()
                    ' Set the label and enable the button
                    lblCurrentAPI.Text = "Cached APIs Loaded:"
                    tsbDownloadData.Enabled = True
                Else
                    ' Set the pilot
                    cboOwner.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
                End If
            Else
                ' Set the pilot
                cboOwner.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
            End If
        End If
    End Sub
    Private Sub GetCharAssets(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 2})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCorpAssets(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 2})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCharBalances(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 3})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCorpBalances(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 3})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCharJobs(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryChar, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 4})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCorpJobs(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryCorp, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 4})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCharJournal(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, pilotAccount, selPilot.ID, 1000, "", 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 5})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCorpJournal(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    For divID As Integer = 1006 To 1000 Step -1
                        apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp, pilotAccount, selPilot.ID, divID, "", 0)
                    Next

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 5})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCharOrders(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersChar, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 6})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCorpOrders(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersCorp, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 6})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCharTransactions(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Setup the array of transactions
                    Dim transNodes As New ArrayList
                    Dim transID As String = ""

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransChar, pilotAccount, selPilot.ID, 1000, transID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.ID, selPilot.Name, 7})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCorpTransactions(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim accountName As String = selPilot.Account
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                If selPilot.Active = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    For divID As Integer = 1006 To 1000 Step -1
                        apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransCorp, pilotAccount, selPilot.ID, divID, "", 0)
                    Next

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 7})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub GetCorpSheet(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim apiXML As New XmlDocument
                    apiXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpSheet, pilotAccount, selPilot.ID, 0)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {apiXML, selPilot.CorpID, selPilot.Name, 8})
                    End If

                End If
            End If
        Next
    End Sub
    Private Sub UpdateNullCorpSheet(ByVal State As Object)
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If selPilot.Active = True Then
                Dim accountName As String = selPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {Nothing, selPilot.ID, selPilot.Name, 8})
                    End If

                End If
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
        clvAssets.BeginUpdate()
        clvAssets.Items.Clear()
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
        If chkExcludeOrders.Checked = False Then
            Call Me.DisplayOrders()
        End If
        tssLabelTotalAssets.Text = FormatNumber(totalAssetValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ISK  (" & FormatNumber(totalAssetCount, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " total quantity)"
        clvAssets.Sort(0, System.Windows.Forms.SortOrder.Ascending, True)
        clvAssets.EndUpdate()
    End Sub
    Private Sub ParseCorpSheets()
        ' Reset the lists of divisions and wallets
        divisions.Clear()
        walletDivisions.Clear()
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            Dim owner As String = cPilot.Text
            Dim IsCorp As Boolean = False
            ' See if this owner is a corp
            If PlugInData.CorpList.ContainsKey(owner) = True Then
                IsCorp = True
                ' See if we have a representative
                Dim CorpRep As SortedList = CType(PlugInData.CorpReps(6), Collections.SortedList)
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
            End If
            If owner <> "" Then
                Dim corpXML As New XmlDocument
                Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
                Dim accountName As String = selPilot.Account
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                If IsCorp = True Then
                    corpXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpSheet, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                End If
                If corpXML IsNot Nothing Then
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

        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            Dim owner As String = cPilot.Text
            Dim rep As String = ""
            Dim IsCorp As Boolean = False
            assetCorpMode = False
            ' See if this owner is a corp
            If PlugInData.CorpList.ContainsKey(owner) = True Then
                IsCorp = True
                assetCorpMode = chkCorpHangarMode.Checked
                ' See if we have a representative
                Dim CorpRep As SortedList = CType(PlugInData.CorpReps(0), Collections.SortedList)
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    rep = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    rep = ""
                End If
            Else
                rep = owner
            End If

            If rep <> "" Then
                Dim assetXML As New XmlDocument
                Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(rep), Core.Pilot)
                Dim accountName As String = selPilot.Account
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                If IsCorp = True Then
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                Else
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                End If
                If assetXML IsNot Nothing Then
                    Dim locList As XmlNodeList
                    Dim loc As XmlNode
                    locList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                    If locList.Count > 0 Then
                        Dim linePrice As Double = 0
                        Dim containerPrice As Double = 0
                        Dim AssetIsInHanger As Boolean = False
                        Dim hangarPrice As Double = 0
                        For Each loc In locList
                            ' Check if the location is already listed
                            Dim locNode As New ContainerListViewItem
                            Dim addLocation As Boolean = True
                            For Each testNode As ContainerListViewItem In clvAssets.Items
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
                                locNode.SubItems(0).Tag = locNode.Text
                                clvAssets.Items.Add(locNode)
                            End If

                            Dim itemID As String = loc.Attributes.GetNamedItem("typeID").Value
                            Dim itemData As New EveHQ.Core.EveItem
                            Dim itemName As String = ""
                            Dim groupID As String = ""
                            Dim catID As String = ""
                            Dim groupName As String = ""
                            Dim catName As String = ""
                            Dim metaLevel As String = ""
                            Dim volume As String = ""
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                                itemData = EveHQ.Core.HQ.itemData(itemID)
                                itemName = itemData.Name
                                groupID = CStr(itemData.Group)
                                catID = CStr(itemData.Category)
                                groupName = EveHQ.Core.HQ.itemGroups(groupID)
                                catName = EveHQ.Core.HQ.itemCats(catID)
                                metaLevel = EveHQ.Core.HQ.itemData(itemID).MetaLevel.ToString
                                If PlugInData.PackedVolumes.Contains(groupID) = True Then
                                    If loc.Attributes.GetNamedItem("singleton").Value = "0" Then
                                        volume = FormatNumber(PlugInData.PackedVolumes(groupID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    Else
                                        volume = FormatNumber(EveHQ.Core.HQ.itemData(itemID).Volume * CDbl(loc.Attributes.GetNamedItem("quantity").Value), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    End If
                                Else
                                    volume = FormatNumber(EveHQ.Core.HQ.itemData(itemID).Volume * CDbl(loc.Attributes.GetNamedItem("quantity").Value), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                End If
                            Else
                                ' Can't find the item in the database
                                itemName = "ItemID: " & itemID.ToString
                                groupID = "unknown"
                                catID = "unknown"
                                groupName = "Unknown"
                                catName = "Unknown"
                                metaLevel = "0"
                                volume = "0"
                            End If

                            Dim newAsset As New ContainerListViewItem
                            newAsset.Tag = loc.Attributes.GetNamedItem("itemID").Value
                            Dim flagID As Integer = CInt(loc.Attributes.GetNamedItem("flag").Value)
                            Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                            Dim accountID As Integer = flagID + 885
                            If accountID = 889 Then accountID = 1000
                            If IsCorp = True And groupName <> "Station Services" Then
                                If divisions.ContainsKey(selPilot.CorpID & "_" & accountID.ToString) = True Then
                                    flagName = CStr(divisions.Item(selPilot.CorpID & "_" & accountID.ToString))
                                    If assetCorpMode = True And locNode.Items.Count < 7 Then
                                        ' Build the corp division nodes
                                        For div As Integer = 0 To 6
                                            Dim hangar As New ContainerListViewItem
                                            hangar.Text = CStr(divisions.Item(selPilot.CorpID & "_" & (1000 + div).ToString))
                                            locNode.Items.Add(hangar)
                                            hangar.SubItems(AssetColumn.Value).Text = FormatNumber(0, 2)
                                        Next
                                    End If
                                End If
                            End If

                            ' Add the asset to the treelistview
                            If assetCorpMode = True And (flagID = 4 Or (flagID >= 116 And flagID <= 121)) Then
                                If (accountID - 1000) >= 0 And (accountID - 1000) < locNode.Items.Count Then
                                    locNode.Items(accountID - 1000).Items.Add(newAsset)
                                    AssetIsInHanger = True
                                Else
                                    ' Catch case where errors were occuring
                                    Dim msg As String = "Unable to add corp asset into hangar:" & ControlChars.CrLf
                                    msg &= "AssetID: " & newAsset.Tag.ToString & ControlChars.CrLf
                                    msg &= "Item: " & itemName & ControlChars.CrLf
                                    msg &= "FlagID: " & flagID.ToString & " (" & flagName & ")" & ControlChars.CrLf
                                    msg &= "AccountID: " & accountID.ToString & ControlChars.CrLf
                                    msg &= "Parent: " & locNode.Text & ControlChars.CrLf & ControlChars.CrLf
                                    msg &= "Please post this as a bug report - the text of this message has been copied to the clipboard to assist you." & ControlChars.CrLf & ControlChars.CrLf
                                    msg &= "Corp Assets will be incomplete while this error appears. Corp Hangar Mode will be disabled."
                                    chkCorpHangarMode.Checked = False
                                    Try
                                        Clipboard.SetText(msg)
                                    Catch e As Exception
                                    End Try
                                    MessageBox.Show(msg, "Corp Hangar Mode Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit For
                                End If
                            Else
                                locNode.Items.Add(newAsset)
                                AssetIsInHanger = False
                            End If

                            newAsset.SubItems(0).Tag = itemName
                            If PlugInData.AssetItemNames.ContainsKey(newAsset.Tag.ToString) = True Then
                                newAsset.Text = PlugInData.AssetItemNames(newAsset.Tag.ToString)
                            Else
                                newAsset.Text = itemName
                            End If
                            newAsset.SubItems(AssetColumn.Owner).Text = owner
                            newAsset.SubItems(AssetColumn.Group).Text = groupName
                            newAsset.SubItems(AssetColumn.Category).Text = catName
                            newAsset.SubItems(AssetColumn.Location).Text = flagName
                            newAsset.SubItems(AssetColumn.Meta).Text = metaLevel
                            newAsset.SubItems(AssetColumn.Volume).Text = volume
                            newAsset.SubItems(AssetColumn.Quantity).Text = FormatNumber(loc.Attributes.GetNamedItem("quantity").Value, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            If newAsset.Text.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
                                newAsset.SubItems(AssetColumn.Price).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                linePrice = 0
                            Else
                                ' Check with BP Manager if this is a BPO
                                Dim IsBPO As Boolean = True
                                If PlugInData.BlueprintAssets.ContainsKey(owner) = True Then
                                    If PlugInData.BlueprintAssets(owner).ContainsKey(newAsset.Tag.ToString) = True Then
                                        Dim chkBPO As BlueprintAsset = PlugInData.BlueprintAssets(owner).Item(newAsset.Tag.ToString)
                                        If chkBPO.Runs > -1 Or chkBPO.BPType = BPType.Unknown Then
                                            IsBPO = False
                                            If chkBPO.BPType <> BPType.Unknown Then
                                                newAsset.Text = newAsset.Text.Replace("Blueprint", "BPC")
                                            End If
                                        Else
                                            newAsset.Text = newAsset.Text.Replace("Blueprint", "BPO")
                                        End If
                                    End If
                                End If
                                If IsBPO = True Then
                                    newAsset.SubItems(AssetColumn.Price).Text = FormatNumber(Math.Round(EveHQ.Core.DataFunctions.GetPrice(itemID), 2), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    If IsNumeric(newAsset.SubItems(AssetColumn.Price).Text) = True Then
                                        linePrice = CDbl(newAsset.SubItems(AssetColumn.Quantity).Text) * CDbl(newAsset.SubItems(AssetColumn.Price).Text)
                                    Else
                                        linePrice = 0
                                    End If
                                Else
                                    newAsset.SubItems(AssetColumn.Price).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    linePrice = 0
                                End If
                            End If
                            newAsset.SubItems(AssetColumn.Value).Text = FormatNumber(linePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

                            ' Add the asset to the list of assets
                            Dim newAssetList As New AssetItem
                            newAssetList.itemID = newAsset.Tag.ToString
                            newAssetList.system = locNode.Text
                            newAssetList.typeID = itemID
                            newAssetList.typeName = itemName
                            newAssetList.owner = owner
                            newAssetList.group = groupName
                            newAssetList.category = catName
                            newAssetList.location = flagName
                            newAssetList.quantity = CLng(loc.Attributes.GetNamedItem("quantity").Value)
                            newAssetList.price = CDbl(newAsset.SubItems(8).Text)
                            totalAssetCount += newAssetList.quantity
                            assetList.Add(newAssetList.itemID, newAssetList)

                            ' Check if this row has child nodes and repeat
                            If loc.HasChildNodes = True Then
                                Call Me.PopulateAssetNode(newAsset, loc, owner, locNode.Text, selPilot)
                            End If

                            ' Update hangar price if applicable
                            If AssetIsInHanger = True Then
                                hangarPrice = CDbl(newAsset.ParentItem.SubItems(AssetColumn.Value).Text)
                                newAsset.ParentItem.SubItems(AssetColumn.Value).Text = FormatNumber(hangarPrice + CDbl(newAsset.SubItems(AssetColumn.Value).Text), 2)
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
        If clvAssets.Items.Count > 0 Then
            Do
                cLoc = clvAssets.Items(cL)
                locationPrice = 0
                For Each cLine As ContainerListViewItem In cLoc.Items
                    locationPrice += CDbl(cLine.SubItems(AssetColumn.Value).Text)
                Next
                totalAssetValue += locationPrice
                cLoc.SubItems(AssetColumn.Value).Text = FormatNumber(locationPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                ' Delete if no child nodes at the locations
                If cLoc.Items.Count = 0 Then
                    clvAssets.Items.Remove(cLoc)
                    cL -= 1
                End If
                cL += 1
            Loop Until cL = clvAssets.Items.Count
        End If
    End Sub
    Private Function PopulateAssetNode(ByVal parentAsset As ContainerListViewItem, ByVal loc As XmlNode, ByVal assetOwner As String, ByVal location As String, ByVal selPilot As EveHQ.Core.Pilot) As Double
        Dim subLocList As XmlNodeList
        Dim subLoc As XmlNode
        Dim containerPrice As Double = 0
        Dim AssetIsInHanger As Boolean = False
        Dim hangarPrice As Double = 0
        Dim linePrice As Double = 0
        subLocList = loc.ChildNodes(0).ChildNodes
        If IsNumeric(parentAsset.SubItems(AssetColumn.Price).Text) = True Then
            containerPrice = CDbl(parentAsset.SubItems(AssetColumn.Price).Text)
        Else
            containerPrice = 0
            parentAsset.SubItems(AssetColumn.Price).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        End If
        For Each subLoc In subLocList
            Try
                Dim ItemID As String = subLoc.Attributes.GetNamedItem("typeID").Value
                Dim ItemData As New EveHQ.Core.EveItem
                Dim itemName As String = ""
                Dim groupID As String = ""
                Dim catID As String = ""
                Dim groupName As String = ""
                Dim catName As String = ""
                Dim metaLevel As String = ""
                Dim volume As String = ""
                If EveHQ.Core.HQ.itemData.ContainsKey(ItemID) Then
                    ItemData = EveHQ.Core.HQ.itemData(ItemID)
                    itemName = ItemData.Name
                    groupID = CStr(ItemData.Group)
                    catID = CStr(ItemData.Category)
                    groupName = EveHQ.Core.HQ.itemGroups(groupID)
                    catName = EveHQ.Core.HQ.itemCats(catID)
                    metaLevel = EveHQ.Core.HQ.itemData(ItemID).MetaLevel.ToString
                    If PlugInData.PackedVolumes.Contains(groupID) = True Then
                        If loc.Attributes.GetNamedItem("singleton").Value = "0" Then
                            volume = FormatNumber(PlugInData.PackedVolumes(groupID), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        Else
                            volume = FormatNumber(EveHQ.Core.HQ.itemData(ItemID).Volume * CDbl(subLoc.Attributes.GetNamedItem("quantity").Value), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        End If
                    Else
                        volume = FormatNumber(EveHQ.Core.HQ.itemData(ItemID).Volume * CDbl(subLoc.Attributes.GetNamedItem("quantity").Value), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    End If
                Else
                    ' Can't find the item in the database
                    itemName = "ItemID: " & ItemID.ToString
                    groupID = "unknown"
                    catID = "unknown"
                    groupName = "Unknown"
                    catName = "Unknown"
                    metaLevel = "0"
                    volume = "0"
                End If

                Dim subAsset As New ContainerListViewItem
                subAsset.Tag = subLoc.Attributes.GetNamedItem("itemID").Value
                Dim subFlagID As Integer = CInt(subLoc.Attributes.GetNamedItem("flag").Value)
                Dim subFlagName As String = PlugInData.itemFlags(subFlagID).ToString
                Dim accountID As Integer = subFlagID + 885
                If accountID = 889 Then accountID = 1000
                If assetOwner = selPilot.Corp And groupName <> "Station Services" Then
                    If divisions.ContainsKey(selPilot.CorpID & "_" & accountID.ToString) = True Then
                        subFlagName = CStr(divisions.Item(selPilot.CorpID & "_" & accountID.ToString))
                        If assetCorpMode = True And parentAsset.Items.Count < 7 Then
                            ' Build the corp division nodes
                            For div As Integer = 0 To 6
                                Dim hangar As New ContainerListViewItem
                                hangar.Text = CStr(divisions.Item(selPilot.CorpID & "_" & (1000 + div).ToString))
                                parentAsset.Items.Add(hangar)
                                hangar.SubItems(AssetColumn.Value).Text = FormatNumber(0, 2)
                            Next
                        End If
                    End If
                End If
                If assetCorpMode = True And (subFlagID = 4 Or (subFlagID >= 116 And subFlagID <= 121)) Then
                    parentAsset.Items(accountID - 1000).Items.Add(subAsset)
                    AssetIsInHanger = True
                Else
                    parentAsset.Items.Add(subAsset)
                    AssetIsInHanger = False
                End If
                subAsset.SubItems(0).Tag = itemName
                If PlugInData.AssetItemNames.ContainsKey(subAsset.Tag.ToString) = True Then
                    subAsset.Text = PlugInData.AssetItemNames(subAsset.Tag.ToString)
                Else
                    subAsset.Text = itemName
                End If
                subAsset.SubItems(AssetColumn.Owner).Text = assetOwner
                subAsset.SubItems(AssetColumn.Group).Text = groupName
                subAsset.SubItems(AssetColumn.Category).Text = catName
                subAsset.SubItems(AssetColumn.Location).Text = subFlagName
                subAsset.SubItems(AssetColumn.Meta).Text = metaLevel
                subAsset.SubItems(AssetColumn.Volume).Text = volume
                subAsset.SubItems(AssetColumn.Quantity).Text = FormatNumber(subLoc.Attributes.GetNamedItem("quantity").Value, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                If subAsset.Text.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
                    subAsset.SubItems(AssetColumn.Price).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    linePrice = 0
                Else
                    ' Check with BP Manager if this is a BPO
                    Dim IsBPO As Boolean = True
                    If PlugInData.BlueprintAssets.ContainsKey(assetOwner) = True Then
                        If PlugInData.BlueprintAssets(assetOwner).ContainsKey(subAsset.Tag.ToString) = True Then
                            Dim chkBPO As BlueprintAsset = PlugInData.BlueprintAssets(assetOwner).Item(subAsset.Tag.ToString)
                            If chkBPO.Runs > -1 Or chkBPO.BPType = BPType.Unknown Then
                                IsBPO = False
                                If chkBPO.BPType <> BPType.Unknown Then
                                    subAsset.Text = subAsset.Text.Replace("Blueprint", "BPC")
                                End If
                            Else
                                subAsset.Text = subAsset.Text.Replace("Blueprint", "BPO")
                            End If
                        End If
                    End If
                    If IsBPO = True Then
                        subAsset.SubItems(AssetColumn.Price).Text = FormatNumber(Math.Round(EveHQ.Core.DataFunctions.GetPrice(ItemID), 2), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        If IsNumeric(subAsset.SubItems(AssetColumn.Price).Text) = True Then
                            linePrice = CDbl(subAsset.SubItems(AssetColumn.Quantity).Text) * CDbl(subAsset.SubItems(AssetColumn.Price).Text)
                        Else
                            linePrice = 0
                            subAsset.SubItems(AssetColumn.Price).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        End If
                    Else
                        linePrice = 0
                        subAsset.SubItems(AssetColumn.Price).Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    End If
                End If
                containerPrice += linePrice
                subAsset.SubItems(AssetColumn.Value).Text = FormatNumber(linePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

                ' Update hangar price if applicable
                If AssetIsInHanger = True Then
                    hangarPrice = CDbl(subAsset.ParentItem.SubItems(AssetColumn.Value).Text)
                    subAsset.ParentItem.SubItems(AssetColumn.Value).Text = FormatNumber(hangarPrice + linePrice, 2)
                End If

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
                newAssetList.price = CDbl(subAsset.SubItems(AssetColumn.Price).Text)
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
        parentAsset.SubItems(AssetColumn.Value).Text = FormatNumber(containerPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Return containerPrice
    End Function
    Private Sub DisplayISKAssets()
        Dim corpXML As New XmlDocument

        ' Reset and parse the character wallets
        charWallets.Clear()
        corpWallets.Clear()
        corpWalletDivisions.Clear()
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            Dim IsCorp As Boolean = False
            ' Get the owner we will use
            Dim owner As String = cPilot.Text
            ' See if this owner is a corp
            If PlugInData.CorpList.ContainsKey(owner) = True Then
                IsCorp = True
                ' See if we have a representative
                Dim CorpRep As SortedList = CType(PlugInData.CorpReps(1), Collections.SortedList)
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
            End If

            If owner <> "" Then
                Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
                Dim accountName As String = selPilot.Account
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                ' Check for corp wallets
                If IsCorp = True Then
                    corpXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                    If corpXML IsNot Nothing Then
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
                    corpXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                    If corpXML IsNot Nothing Then
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
        clvAssets.Items.Add(node)
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
                iskNode.SubItems(AssetColumn.Value).Text = FormatNumber(charWallets(pilot), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                personalCash += CDbl(charWallets(pilot))
            Next
            personalNode.SubItems(AssetColumn.Value).Text = FormatNumber(personalCash, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
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
                    iskNode.SubItems(AssetColumn.Value).Text = FormatNumber(corpWalletDivisions(idx), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    divisionCash += CDbl(corpWalletDivisions(idx))
                Next
                corporateCash += divisionCash
                corpNode.SubItems(AssetColumn.Value).Text = FormatNumber(divisionCash, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Next
            corporateNode.SubItems(AssetColumn.Value).Text = FormatNumber(corporateCash, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            totalCash += corporateCash
        End If
        node.SubItems(AssetColumn.Value).Text = FormatNumber(totalCash, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        totalAssetValue += totalCash
    End Sub
    Private Sub DisplayInvestments()

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
        clvAssets.Items.Add(investNode)

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
                                invNode.SubItems(AssetColumn.Category).Text = "Cash"
                                If inv.ValueIsCost = True Then
                                    invNode.SubItems(AssetColumn.Value).Text = FormatNumber(inv.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    ownerValue += inv.CurrentCost
                                Else
                                    invNode.SubItems(AssetColumn.Value).Text = FormatNumber(inv.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    ownerValue += inv.CurrentValue
                                End If
                            Case InvestmentType.Shares
                                invNode.SubItems(AssetColumn.Category).Text = "Shares"
                                invNode.SubItems(AssetColumn.Price).Text = FormatNumber(inv.CurrentQuantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                If inv.ValueIsCost = True Then
                                    invNode.SubItems(AssetColumn.Price).Text = FormatNumber(inv.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    invNode.SubItems(AssetColumn.Value).Text = FormatNumber(inv.CurrentQuantity * inv.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    ownerValue += (inv.CurrentQuantity * inv.CurrentCost)
                                Else
                                    invNode.SubItems(AssetColumn.Price).Text = FormatNumber(inv.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    invNode.SubItems(AssetColumn.Value).Text = FormatNumber(inv.CurrentQuantity * inv.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    ownerValue += (inv.CurrentQuantity * inv.CurrentValue)
                                End If
                        End Select
                    End If
                Next
                ownerNode.SubItems(AssetColumn.Value).Text = FormatNumber(ownerValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                totalValue += ownerValue
            End If
        Next
        investNode.SubItems(AssetColumn.Value).Text = FormatNumber(totalValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        totalAssetValue += totalValue
    End Sub
    Private Sub DisplayOrders()

        ' Add the balances to the assets schedule
        Dim ordersNode As New ContainerListViewItem
        Dim buyOrders As New ContainerListViewItem
        Dim sellOrders As New ContainerListViewItem
        Dim buyValue, sellValue As Double
        ordersNode.Tag = "Orders"
        ordersNode.Text = "Market Orders"
        clvAssets.Items.Add(ordersNode)
        ' Add the Buy Orders node
        buyOrders.Text = "Buy Orders"
        buyOrders.Tag = "Buy Orders"
        ordersNode.Items.Add(buyOrders)
        ' Add the Sell Orders node
        sellOrders.Text = "Sell Orders"
        sellOrders.Tag = "Sell Orders"
        ordersNode.Items.Add(sellOrders)

        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            Dim IsCorp As Boolean = False
            ' Get the owner we will use
            Dim owner As String = cPilot.Text
            ' Get the orders
            Dim orderCollection As MarketOrdersCollection = ParseMarketOrders(owner)
            ' Add the orders (outstanding ones only)
            Dim category, group As String
            For Each ownerOrder As MarketOrder In orderCollection.MarketOrders
                If ownerOrder.OrderState = MarketOrderState.Open Then
                    Dim orderNode As New ContainerListViewItem
                    orderNode.Tag = ownerOrder.TypeID
                    Dim orderItem As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(ownerOrder.TypeID.ToString)
                    category = EveHQ.Core.HQ.itemCats(orderItem.Category.ToString)
                    group = EveHQ.Core.HQ.itemGroups(orderItem.Group.ToString)
                    ' Check for search criteria
                    If Not ((filters.Count > 0 And catFilters.Contains(category) = False And groupFilters.Contains(group) = False) Or (searchText <> "" And orderItem.Name.ToLower.Contains(searchText.ToLower) = False)) Then
                        orderNode.Text = orderItem.Name
                        If ownerOrder.Bid = 0 Then
                            sellOrders.Items.Add(orderNode)
                            sellValue += ownerOrder.Price * ownerOrder.VolRemaining
                        Else
                            buyOrders.Items.Add(orderNode)
                            buyValue += ownerOrder.Price * ownerOrder.VolRemaining
                        End If
                        orderNode.SubItems(AssetColumn.Owner).Text = owner
                        orderNode.SubItems(AssetColumn.Group).Text = group
                        orderNode.SubItems(AssetColumn.Category).Text = category
                        If PlugInData.stations.Contains(ownerOrder.StationID) = True Then
                            orderNode.SubItems(AssetColumn.Location).Text = CType(PlugInData.stations(ownerOrder.StationID), Station).stationName
                        Else
                            orderNode.SubItems(AssetColumn.Location).Text = "StationID: " & ownerOrder.StationID
                        End If
                        orderNode.SubItems(AssetColumn.Meta).Text = FormatNumber(orderItem.MetaLevel.ToString, 0)
                        orderNode.SubItems(AssetColumn.Volume).Text = FormatNumber(orderItem.Volume.ToString, 2)
                        orderNode.SubItems(AssetColumn.Quantity).Text = FormatNumber(ownerOrder.VolRemaining, 0)
                        orderNode.SubItems(AssetColumn.Price).Text = FormatNumber(ownerOrder.Price, 2)
                        orderNode.SubItems(AssetColumn.Value).Text = FormatNumber(ownerOrder.Price * ownerOrder.VolRemaining, 2)
                    End If
                End If
            Next
        Next
        buyOrders.SubItems(AssetColumn.Value).Text = FormatNumber(buyValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        sellOrders.SubItems(AssetColumn.Value).Text = FormatNumber(sellValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        ordersNode.SubItems(AssetColumn.Value).Text = FormatNumber(buyValue + sellValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        totalAssetValue += buyValue + sellValue
    End Sub
#End Region

#Region "Outpost XML Retrieval and Parsing"

    Private Sub GetOutposts()

        ' Make a call to the EveHQ.Core.API to fetch the assets
        Dim stationXML As New XmlDocument
        stationXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Conquerables, 0)

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
        If clvAssets.Items.Count > 0 Then
            Do
                cLoc = clvAssets.Items(cL)
                If cLoc.Items.Count = 0 Then
                    If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(AssetColumn.Category).Text) = False And groupFilters.Contains(cLoc.SubItems(AssetColumn.Group).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        clvAssets.Items.Remove(cLoc)
                        assetList.Remove(cLoc.Tag)
                        totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                        cL -= 1
                    End If
                Else
                    Call FilterNode(cLoc)
                    If cLoc.Items.Count = 0 Then
                        If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(AssetColumn.Category).Text) = False And groupFilters.Contains(cLoc.SubItems(AssetColumn.Group).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                            clvAssets.Items.Remove(cLoc)
                            If IsNumeric(cLoc.SubItems(AssetColumn.Quantity).Text) = True Then
                                totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                            End If
                            'assetList.Remove(cLoc.Tag)
                            cL -= 1
                        End If
                    Else
                        If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(AssetColumn.Category).Text) = False And groupFilters.Contains(cLoc.SubItems(AssetColumn.Group).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                            If IsNumeric(cLoc.SubItems(AssetColumn.Quantity).Text) = True Then
                                totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                            End If
                            ' Remove quantity and price information
                            cLoc.SubItems(AssetColumn.Quantity).Text = ""
                            cLoc.SubItems(AssetColumn.Price).Text = ""
                            'assetList.Remove(cLoc.Tag)
                        End If
                    End If
                End If
                cL += 1
            Loop Until (cL = clvAssets.Items.Count)
        End If
        Call Me.CalcFilteredPrices()
    End Sub
    Private Sub FilterNode(ByVal pLoc As ContainerListViewItem)
        Dim cL As Integer = 0
        Dim cLoc As ContainerListViewItem
        Do
            cLoc = pLoc.Items(cL)
            If cLoc.Items.Count = 0 Then
                If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(AssetColumn.Category).Text) = False And groupFilters.Contains(cLoc.SubItems(AssetColumn.Group).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                    pLoc.Items.Remove(cLoc)
                    If cLoc.Tag IsNot Nothing Then
                        assetList.Remove(cLoc.Tag)
                    End If
                    If IsNumeric(cLoc.SubItems(AssetColumn.Quantity).Text) = True Then
                        totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                    End If
                    cL -= 1
                End If
            Else
                Call FilterNode(cLoc)
                If cLoc.Items.Count = 0 Then
                    If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(AssetColumn.Category).Text) = False And groupFilters.Contains(cLoc.SubItems(AssetColumn.Group).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        pLoc.Items.Remove(cLoc)
                        If cLoc.Tag IsNot Nothing Then
                            assetList.Remove(cLoc.Tag)
                        End If
                        If IsNumeric(cLoc.SubItems(AssetColumn.Quantity).Text) = True Then
                            totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                        End If
                        cL -= 1
                    End If
                Else
                    If (filters.Count > 0 And catFilters.Contains(cLoc.SubItems(AssetColumn.Category).Text) = False And groupFilters.Contains(cLoc.SubItems(AssetColumn.Group).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        If IsNumeric(cLoc.SubItems(AssetColumn.Quantity).Text) = True Then
                            totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                        End If
                        ' Remove quantity and price information
                        cLoc.SubItems(AssetColumn.Quantity).Text = ""
                        cLoc.SubItems(AssetColumn.Price).Text = ""
                        If cLoc.Tag IsNot Nothing Then
                            assetList.Remove(cLoc.Tag)
                        End If
                    End If
                End If
            End If
            cL += 1
        Loop Until (cL = pLoc.Items.Count)
    End Sub
    Private Sub CalcFilteredPrices()
        totalAssetValue = 0
        Dim locPrice As Double = 0
        For Each cLoc As ContainerListViewItem In clvAssets.Items
            ' Calculate cost of all the sub nodes
            If cLoc.Items.Count > 0 Then
                locPrice = Me.CalcNodePrice(cLoc)
                cLoc.SubItems(AssetColumn.Value).Text = FormatNumber(locPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
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
                lineValue = CDbl(cLoc.SubItems(AssetColumn.Value).Text)
                contValue += lineValue
            Else
                If IsNumeric(cLoc.SubItems(AssetColumn.Price).Text) = True Then
                    lineValue = CDbl(cLoc.SubItems(AssetColumn.Quantity).Text) * CDbl(cLoc.SubItems(AssetColumn.Price).Text)
                Else
                    lineValue = 0
                End If
                cLoc.SubItems(AssetColumn.Value).Text = FormatNumber(lineValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                contValue += lineValue
            End If
        Next
        pLoc.SubItems(AssetColumn.Value).Text = FormatNumber(contValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
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
    Private Sub AddToFilterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddToFilterToolStripMenuItem.Click
        Call Me.AddFilter()
    End Sub
    Private Sub RemoveFilterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFilterToolStripMenuItem.Click
        Call Me.RemoveFilter()
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
    Private Sub ctxFilterList_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxFilterList.Opening
        If lstFilters.SelectedItems.Count = 0 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub cboOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboOwner.SelectedIndexChanged
        Call Me.UpdatePrismInfo()
    End Sub
    Private Sub UpdatePrismInfo()
        startup = True
        ' Automatically set the filters to just agree to this pilot
        For Each Owner As ListViewItem In lvwCharFilter.Items
            If Owner.Text = cboOwner.SelectedItem.ToString Then
                Owner.Checked = True
            Else
                Owner.Checked = False
            End If
        Next
        lblOwnerFilters.Text = "Owner Filter: " & cboOwner.SelectedItem.ToString
        Call Me.RefreshAssets()
        Call Me.ParseOrders()
        ' Update the Wallet Division Filters
        Call Me.UpdateWalletDivisions()
        ' Update the Wallet transactions
        If cboWalletTransDivision.SelectedIndex = 0 Then
            Call Me.ParseWalletTransactions()
        Else
            cboWalletTransDivision.SelectedIndex = 0
        End If
        If cboWalletJournalDivision.SelectedIndex = 0 Then
            Call Me.ParseWalletJournal()
        Else
            cboWalletJournalDivision.SelectedIndex = 0
        End If
        ' Update the Jobs
        Call Me.ParseIndustryJobs()
        ' Build BP Manager lists and set index
        cboCategoryFilter.BeginUpdate()
        cboCategoryFilter.Items.Clear()
        cboCategoryFilter.Items.Add("All")
        For Each cat As String In PlugInData.CategoryNames.Keys
            cboCategoryFilter.Items.Add(cat)
        Next
        cboCategoryFilter.EndUpdate()
        cboTechFilter.SelectedIndex = 0
        cboTypeFilter.SelectedIndex = 0
        cboCategoryFilter.SelectedIndex = 0
        ' Update the BP Manager List
        Call Me.UpdateBPList()
        startup = False
    End Sub
    Private Sub FilterSystemValue()
        Dim minValue As Double
        If Double.TryParse(txtMinSystemValue.Text, minValue) = False Then
            minValue = 0
        End If
        Dim cL As Integer = 0
        Dim cLoc As ContainerListViewItem
        If clvAssets.Items.Count > 0 Then
            Do
                cLoc = clvAssets.Items(cL)
                If CDbl(cLoc.SubItems(AssetColumn.Value).Text) < minValue Then
                    Call FilterSystemNode(cLoc)
                    If cLoc.Items.Count = 0 Then
                        clvAssets.Items.Remove(cLoc)
                        cL -= 1
                    End If
                End If
                cL += 1
            Loop Until (cL = clvAssets.Items.Count)
        End If
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
                totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                cL -= 1
            Else
                Call FilterSystemNode(cLoc)
                If cLoc.Items.Count = 0 Then
                    pLoc.Items.Remove(cLoc)
                    assetList.Remove(cLoc.Tag)
                    totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                    cL -= 1
                Else
                    If IsNumeric(cLoc.SubItems(AssetColumn.Quantity).Text) = True Then
                        totalAssetCount -= CLng(cLoc.SubItems(AssetColumn.Quantity).Text)
                    End If
                    ' Remove quantity and price information
                    cLoc.SubItems(AssetColumn.Quantity).Text = ""
                    cLoc.SubItems(AssetColumn.Price).Text = ""
                    assetList.Remove(cLoc.Tag)
                End If
            End If
            cL += 1
        Loop Until (cL = pLoc.Items.Count)
    End Sub
    Private Sub RecalcAllPrices()
        totalAssetValue = 0
        Dim locPrice As Double = 0
        For Each cLoc As ContainerListViewItem In clvAssets.Items
            ' Calculate cost of all the sub nodes
            If cLoc.Items.Count > 0 Then
                locPrice = Me.RecalcNodePrice(cLoc)
                cLoc.SubItems(AssetColumn.Value).Text = FormatNumber(locPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
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
                lineValue = CDbl(cLoc.SubItems(AssetColumn.Value).Text)
                contValue += lineValue
            Else
                If IsNumeric(cLoc.SubItems(AssetColumn.Price).Text) = True Then
                    lineValue = CDbl(cLoc.SubItems(AssetColumn.Quantity).Text) * CDbl(cLoc.SubItems(AssetColumn.Price).Text)
                Else
                    lineValue = 0
                End If
                cLoc.SubItems(AssetColumn.Value).Text = FormatNumber(lineValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                contValue += lineValue
            End If
        Next
        If IsNumeric(pLoc.SubItems(AssetColumn.Price).Text) = True Then
            contValue += CDbl(pLoc.SubItems(AssetColumn.Price).Text)
        End If
        pLoc.SubItems(AssetColumn.Value).Text = FormatNumber(contValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
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
        For Each Loc As ContainerListViewItem In clvAssets.Items
            LocationValue = 0
            strHTML.Append("<tr bgcolor=444488><td colspan=6>" & Loc.Text & "</td></tr>")
            Dim assets As New SortedList
            Dim assetsList As New SortedList
            Dim newAsset As New AssetItem
            For Each item As ContainerListViewItem In Loc.Items
                If item.SubItems(AssetColumn.Group).Text <> "" Then
                    If (filters.Count > 0 And catFilters.Contains(item.SubItems(AssetColumn.Category).Text) = False And groupFilters.Contains(item.SubItems(AssetColumn.Group).Text) = False) Or (searchText <> "" And item.Text.ToLower.Contains(searchText.ToLower) = False) Then
                    Else
                        If assets.ContainsKey(item.SubItems(AssetColumn.Category).Text & "_" & item.SubItems(AssetColumn.Group).Text) = True Then
                            assetsList = CType(assets.Item(item.SubItems(AssetColumn.Category).Text & "_" & item.SubItems(AssetColumn.Group).Text), Collections.SortedList)
                            assetsList.Add(item.Tag.ToString, item.Tag.ToString)
                        Else
                            Dim assetList As New SortedList
                            assetsList = New SortedList
                            assetsList.Add(item.Tag.ToString, item.Tag.ToString)
                            assets.Add(item.SubItems(AssetColumn.Category).Text & "_" & item.SubItems(AssetColumn.Group).Text, assetsList)
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
            If item.SubItems(AssetColumn.Group).Text <> "" Then
                If (filters.Count > 0 And catFilters.Contains(item.SubItems(AssetColumn.Category).Text) = False And groupFilters.Contains(item.SubItems(AssetColumn.Group).Text) = False) Or (searchText <> "" And item.Text.ToLower.Contains(searchText.ToLower) = False) Then
                Else
                    If assets.ContainsKey(item.SubItems(AssetColumn.Category).Text & "_" & item.SubItems(AssetColumn.Group).Text) = True Then
                        assetslist = CType(assets.Item(item.SubItems(AssetColumn.Category).Text & "_" & item.SubItems(AssetColumn.Group).Text), Collections.SortedList)
                        assetslist.Add(item.Tag.ToString, item.Tag.ToString)
                    Else
                        assetslist = New SortedList
                        assetslist.Add(item.Tag.ToString, item.Tag.ToString)
                        assets.Add(item.SubItems(AssetColumn.Category).Text & "_" & item.SubItems(AssetColumn.Group).Text, assetslist)
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
                If EveHQ.Core.HQ.itemList.ContainsKey(asset) Then
                    price = EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(asset))
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
                If EveHQ.Core.HQ.itemList.ContainsKey(asset) Then
                    cPrice = EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(asset))
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
                If EveHQ.Core.HQ.itemList.ContainsKey(cAsset) Then
                    price = EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(cAsset))
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
                    newItem.SubItems.Add(myInvestment.Name & " (" & [Enum].GetName(GetType(InvestmentType), myInvestment.Type) & ")")
                    newItem.SubItems.Add(myInvestment.Owner)
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentQuantity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    If myInvestment.ValueIsCost = True Then
                        newItem.SubItems.Add(FormatNumber(myInvestment.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    Else
                        newItem.SubItems.Add(FormatNumber(myInvestment.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    End If
                    If myInvestment.ValueIsCost = True Then
                        newItem.SubItems.Add(FormatNumber((myInvestment.CurrentCost * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    Else
                        newItem.SubItems.Add(FormatNumber((myInvestment.CurrentValue * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    End If
                    newItem.SubItems.Add(FormatNumber((myInvestment.CurrentValue * myInvestment.CurrentQuantity) - (myInvestment.CurrentCost * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentProfits, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.SubItems.Add(FormatNumber(myInvestment.CurrentIncome, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    If myInvestment.CurrentIncome > 0 Then
                        newItem.SubItems.Add(FormatNumber(myInvestment.CurrentIncome / myInvestment.TotalCostsForYield * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%")
                    Else
                        newItem.SubItems.Add(FormatNumber(0, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%")
                    End If
                    lvwInvestments.Items.Add(newItem)
                End If
            Next
            lvwInvestments.EndUpdate()
            If lvwInvestments.Items.ContainsKey(InvestmentID) = True Then
                lvwInvestments.Items(InvestmentID).Selected = True
            Else
                lvwTransactions.BeginUpdate()
                lvwTransactions.Items.Clear()
                lvwTransactions.EndUpdate()
            End If
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
        Dim s As New FileStream(Path.Combine(EveHQ.Core.HQ.dataFolder, "investments.txt"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, Portfolio.Investments)
        s.Close()
        s = New FileStream(Path.Combine(EveHQ.Core.HQ.dataFolder, "investmentTransactions.txt"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Portfolio.Transactions)
        s.Close()
    End Sub
    Private Sub LoadInvestments()
        Try
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.dataFolder, "investments.txt")) = True Then
                Dim s As New FileStream(Path.Combine(EveHQ.Core.HQ.dataFolder, "investments.txt"), FileMode.Open)
                Dim f As BinaryFormatter = New BinaryFormatter
                Portfolio.Investments = CType(f.Deserialize(s), SortedList)
                s.Close()
                Try
                    My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.dataFolder, "investments.txt"))
                Catch ex As Exception
                End Try
            End If
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.dataFolder, "investmentTransactions.txt")) = True Then
                Dim s As New FileStream(Path.Combine(EveHQ.Core.HQ.dataFolder, "investmentTransactions.txt"), FileMode.Open)
                Dim f As BinaryFormatter = New BinaryFormatter
                Portfolio.Transactions = CType(f.Deserialize(s), SortedList)
                s.Close()
                Try
                    My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.dataFolder, "investmentTransactions.txt"))
                Catch ex As Exception
                End Try
            End If
            Call Me.ListInvestments()
        Catch e As Exception
        End Try
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
                Call Me.ListInvestments()
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
            InvestmentID = lvwInvestments.SelectedItems(0).Text
            Dim myInvestment As Investment = CType(Portfolio.Investments(CLng(InvestmentID)), Investment)
            lvwTransactions.BeginUpdate()
            lvwTransactions.Items.Clear()
            For Each myTransaction As InvestmentTransaction In myInvestment.Transactions.Values
                Dim newTrans As New ListViewItem
                newTrans.Name = CStr(myTransaction.ID)
                newTrans.Text = CStr(myTransaction.ID)
                newTrans.SubItems.Add(Format(myTransaction.TransDate, "dd/MM/yyyy HH:mm:ss"))
                newTrans.SubItems.Add([Enum].GetName(GetType(InvestmentTransactionType), myTransaction.Type))
                newTrans.SubItems.Add(myTransaction.Quantity.ToString)
                newTrans.SubItems.Add(myTransaction.UnitValue.ToString)
                newTrans.SubItems.Add(myTransaction.Notes)
                lvwTransactions.Items.Add(newTrans)
            Next
            lvwTransactions.EndUpdate()
            lblTransactionView.Text = "Viewing Transactions: " & myInvestment.Name & " (" & myInvestment.Owner & ")"

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
        Dim myInvestment As Investment = CType(Portfolio.Investments(CLng(InvestmentID)), Investment)
        newItem.SubItems.Add(myInvestment.Name)
        newItem.SubItems.Add(myInvestment.Owner)
        newItem.SubItems(3).Text = (FormatNumber(myInvestment.CurrentQuantity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newItem.SubItems(4).Text = (FormatNumber(myInvestment.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        If myInvestment.ValueIsCost = True Then
            newItem.SubItems(5).Text = (FormatNumber(myInvestment.CurrentCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        Else
            newItem.SubItems(5).Text = (FormatNumber(myInvestment.CurrentValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        End If
        If myInvestment.ValueIsCost = True Then
            newItem.SubItems(6).Text = (FormatNumber((myInvestment.CurrentCost * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        Else
            newItem.SubItems(6).Text = (FormatNumber((myInvestment.CurrentValue * myInvestment.CurrentQuantity), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        End If
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
        Call Me.ListInvestments()
        MessageBox.Show("Recalculation of Investment Complete!", "Recalculate Investment Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Function AuditInvestment(ByVal inv As Investment, Optional ByVal silent As Boolean = False) As Boolean
        Dim passedAudit As Boolean = True
        ' Checks whether the totals of the investment are consistent with the transaction history
        Dim chkInv As New Investment
        chkInv.Type = inv.Type
        chkInv.ValueIsCost = inv.ValueIsCost
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
                NewTransaction.EditFlag = True
                NewTransaction.EditTrans = CType(Portfolio.Transactions(CLng(lvwTransactions.SelectedItems(0).Text)), InvestmentTransaction)
                Dim editInv As Investment = CType(Portfolio.Investments(CLng(InvestmentID)), Investment)
                NewTransaction.ShowDialog()
                If NewTransaction.DialogResult <> Windows.Forms.DialogResult.Cancel Then
                    Me.RecalculateInvestment(editInv)
                    Me.ListInvestments()
                    Me.UpdateTransactions()
                End If
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
        If chkViewClosedInvestments.Checked = False Then
            btnReOpenInvestment.Visible = False
        End If
        Me.ListInvestments()
    End Sub
#End Region

#Region "Asset Context Menu & UI Routines"
    Private Sub ctxAssets_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxAssets.Opening
        If clvAssets.SelectedItems.Count > 0 Then
            If clvAssets.SelectedItems.Count = 1 Then
                If clvAssets.SelectedItems(0).SubItems(0).Tag IsNot Nothing Then
                    Dim itemName As String = clvAssets.SelectedItems(0).SubItems(0).Tag.ToString
                    Dim itemText As String = clvAssets.SelectedItems(0).Text
                    Dim itemID As String = clvAssets.SelectedItems(0).Tag.ToString
                    If itemName <> "Cash Balances" And itemName <> "Investments" Then
                        If EveHQ.Core.HQ.itemList.ContainsKey(itemName) = True And itemName <> "Office" And clvAssets.SelectedItems(0).SubItems(AssetColumn.Quantity).Text <> "" Then
                            mnuItemName.Text = itemName
                            mnuItemName.Tag = EveHQ.Core.HQ.itemList(itemName)
                            mnuAddCustomName.Visible = True
                            mnuViewInIB.Visible = True
                            mnuModifyPrice.Visible = True
                            mnuToolSep.Visible = True
                            mnuRecycleItem.Enabled = True
                            If clvAssets.SelectedItems(0).SubItems(AssetColumn.Category).Text = "Ship" Then
                                mnuViewInHQF.Visible = True
                            Else
                                mnuViewInHQF.Visible = False
                            End If
                            If clvAssets.SelectedItems(0).Items.Count > 0 Then
                                mnuRecycleContained.Enabled = True
                                mnuRecycleAll.Enabled = True
                            Else
                                mnuRecycleContained.Enabled = False
                                mnuRecycleAll.Enabled = False
                            End If
                            If PlugInData.AssetItemNames.ContainsKey(itemID) = True Then
                                mnuAddCustomName.Text = "Edit Custom Name"
                                mnuRemoveCustomName.Visible = True
                            Else
                                mnuAddCustomName.Text = "Add Custom Name"
                                mnuRemoveCustomName.Visible = False
                            End If
                            mnuAddCustomName.Tag = itemID
                        Else
                            mnuItemName.Text = itemName
                            mnuAddCustomName.Visible = False
                            mnuRemoveCustomName.Visible = False
                            mnuViewInIB.Visible = False
                            mnuViewInHQF.Visible = False
                            mnuModifyPrice.Visible = False
                            mnuToolSep.Visible = False
                            If clvAssets.SelectedItems(0).Items.Count > 0 Then
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
                    e.Cancel = True
                End If
            Else
                mnuItemName.Text = "Multiple Items"
                mnuItemName.Tag = ""
                mnuAddCustomName.Visible = False
                mnuViewInIB.Visible = False
                mnuViewInHQF.Visible = False
                mnuModifyPrice.Visible = False
                mnuToolSep.Visible = False
                Dim containerItems As Boolean = False
                For Each item As ContainerListViewItem In clvAssets.SelectedItems
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
            myPlugIn.Instance.GetPlugInData(itemID, 0)
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
        If clvAssets.SelectedItems.Count > 0 Then
            Dim assetID As String = clvAssets.SelectedItems(0).Tag.ToString
            Dim shipName As String = clvAssets.SelectedItems(0).Text
            Dim owner As String = clvAssets.SelectedItems(0).SubItems(AssetColumn.Owner).Text
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
                    Case "Cargo"
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

        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems

            Dim IsCorp As Boolean = False
            ' See if this owner is a corp
            If PlugInData.CorpList.ContainsKey(owner) = True Then
                IsCorp = True
                ' See if we have a representative
                Dim CorpRep As SortedList = CType(PlugInData.CorpReps(0), Collections.SortedList)
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
            End If

            If owner <> "" Then
                Dim assetXML As New XmlDocument
                Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
                Dim accountName As String = selPilot.Account
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                If IsCorp = True Then
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                Else
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                End If
                If assetXML IsNot Nothing Then
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
                                        Dim itemName As String = ""
                                        If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                            Dim itemData As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(itemID)
                                            itemName = itemData.Name
                                            groupID = itemData.Group.ToString
                                            catID = itemData.Category.ToString
                                            Dim flagID As Integer = CInt(mods.Attributes.GetNamedItem("flag").Value)
                                            Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                                            Dim quantity As String = mods.Attributes.GetNamedItem("quantity").Value
                                            HQFShip.Add(flagName & "," & itemName & "," & quantity & "," & catID)
                                        Else
                                            ' Can't find the item in the database
                                            itemName = "ItemID: " & itemID.ToString
                                        End If
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
                    If subLoc.HasChildNodes = True Then
                        modList = subLoc.ChildNodes(0).ChildNodes
                        For Each mods In modList
                            Dim itemID As String = mods.Attributes.GetNamedItem("typeID").Value
                            Dim itemName As String = ""
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                Dim itemData As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(itemID)
                                itemName = itemData.Name
                                groupID = itemData.Group.ToString
                                catID = itemData.Category.ToString
                                Dim flagID As Integer = CInt(mods.Attributes.GetNamedItem("flag").Value)
                                Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                                Dim quantity As String = mods.Attributes.GetNamedItem("quantity").Value
                                HQFShip.Add(flagName & "," & itemName & "," & quantity & "," & catID)
                            Else
                                ' Can't find the item in the database
                                itemName = "ItemID: " & itemID.ToString
                            End If
                        Next
                    End If
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
    Private Sub tlvAssets_SelectedItemsChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clvAssets.SelectedItemsChanged
        If clvAssets.SelectedItems.Count > 0 Then
            Dim volume, lineValue, value As Double
            Dim chkParent As New ContainerListViewItem
            Dim parentFlag As Boolean = False
            For Each asset As ContainerListViewItem In clvAssets.SelectedItems
                parentFlag = False
                chkParent = asset.ParentItem
                Do While chkParent IsNot Nothing
                    If chkParent.Selected = True Then
                        parentFlag = True
                        Exit Do
                    End If
                    chkParent = chkParent.ParentItem
                Loop
                If parentFlag = False Then
                    If asset.SubItems(AssetColumn.Quantity).Text <> "" Then
                        volume += CDbl(asset.SubItems(AssetColumn.Volume).Text)
                        lineValue = CDbl(asset.SubItems(AssetColumn.Value).Text)
                        value += lineValue
                    Else
                        If asset.SubItems(AssetColumn.Value).Text <> "" Then
                            lineValue = CDbl(asset.SubItems(AssetColumn.Value).Text)
                            value += lineValue
                        End If
                    End If
                End If
            Next
            tssLabelSelectedAssets.Text = "Volume = " & FormatNumber(volume, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m³ : Value = " & FormatNumber(value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ISK"
        Else
            tssLabelSelectedAssets.Text = ""
        End If
    End Sub
    Private Sub mnuAddCustomName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddCustomName.Click
        Dim assetID As String = mnuAddCustomName.Tag.ToString
        Dim assetName As String = mnuItemName.Text
        Dim newCustomName As New frmAssetItemName
        If PlugInData.AssetItemNames.ContainsKey(assetID) = True Then
            newCustomName.Text = "Edit Custom Asset Name"
            newCustomName.EditMode = True
        Else
            newCustomName.Text = "Add Custom Asset Name"
            newCustomName.EditMode = False
        End If
        newCustomName.AssetID = assetID
        newCustomName.AssetName = assetName
        newCustomName.ShowDialog()
        If newCustomName.AssetItemName <> "" Then
            clvAssets.SelectedItems(0).Text = newCustomName.AssetItemName
        End If
        newCustomName.Dispose()
    End Sub
    Private Sub mnuRemoveCustomName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveCustomName.Click
        Dim assetID As String = mnuAddCustomName.Tag.ToString
        Dim itemName As String = mnuItemName.Text
        Dim assetSQL As String = "DELETE FROM assetItemNames WHERE itemID=" & assetID & ";"
        If EveHQ.Core.DataFunctions.SetData(assetSQL) = False Then
            MessageBox.Show("There was an error deleting the record from the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & assetSQL, "Error Writing Asset Name Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            PlugInData.AssetItemNames.Remove(assetID)
            clvAssets.SelectedItems(0).Text = itemName
        End If
    End Sub
#End Region

#Region "Toolbar Menu Routines"
    Private Sub tsbDownloadData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDownloadData.Click
        ' Set the loaded counter to 0
        ProcessXMLCount = 0

        ' Set the label and disable the button
        lblCurrentAPI.Text = "Downloading API Data..."
        tsbDownloadData.Enabled = False

        ' Set the Corp Reps to Default
        PlugInData.CorpReps.Clear()
        For API As Integer = 0 To 6
            PlugInData.CorpReps.Add(New SortedList)
        Next
        ' Flick to the API Status tab
        tabPrism.SelectTab(tabAPIStatus)
        ' Delete the current API Status data
        For Each Owner As ListViewItem In lvwCurrentAPIs.Items
            Owner.ToolTipText = ""
            For si As Integer = 2 To 8
                Owner.SubItems(si).Text = ""
            Next
        Next
        Call Me.GetXMLData()
    End Sub
    Private Sub btnRefreshAssets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshAssets.Click
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
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetLocations.html"))

        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetLocations.html")) = True Then
            Try
                Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetLocations.html"))
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
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetList.html"))

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetList.html")) = True Then
            Try
                Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetList.html"))
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
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityA.html"))

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityA.html")) = True Then
            Try
                Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityA.html"))
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
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityD.html"))

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityD.html")) = True Then
            Try
                Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityD.html"))
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
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceA.html"))

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceA.html")) = True Then
            Try
                Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceA.html"))
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
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceD.html"))

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceD.html")) = True Then
            Try
                Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceD.html"))
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
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueA.html"))

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueA.html")) = True Then
            Try
                Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueA.html"))
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
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueD.html"))

        sw.Write(strHTML.ToString)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueD.html")) = True Then
            Try
                Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueD.html"))
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
    Private Sub tsbAssets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAssets.Click
        If tabPrism.TabPages.Contains(tabAssets) = False Then
            tabPrism.TabPages.Add(tabAssets)
            tabPrism.SelectedTab = tabAssets
        Else
            tabPrism.SelectedTab = tabAssets
        End If
    End Sub
    Private Sub tsbInvestments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbInvestments.Click
        If tabPrism.TabPages.Contains(tabInvestments) = False Then
            tabPrism.TabPages.Add(tabInvestments)
            tabPrism.SelectedTab = tabInvestments
        Else
            tabPrism.SelectedTab = tabInvestments
        End If
    End Sub
    Private Sub tsbBPManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBPManager.Click
        If tabPrism.TabPages.Contains(tabBPManager) = False Then
            tabPrism.TabPages.Add(tabBPManager)
            tabPrism.SelectedTab = tabBPManager
        Else
            tabPrism.SelectedTab = tabBPManager
        End If
    End Sub
    Private Sub tsbRigBuilder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRigBuilder.Click
        If tabPrism.TabPages.Contains(tabRigBuilder) = False Then
            tabPrism.TabPages.Add(tabRigBuilder)
            tabPrism.SelectedTab = tabRigBuilder
        Else
            tabPrism.SelectedTab = tabRigBuilder
        End If
    End Sub
    Private Sub tsbOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbOrders.Click
        If tabPrism.TabPages.Contains(tabOrders) = False Then
            tabPrism.TabPages.Add(tabOrders)
            tabPrism.SelectedTab = tabOrders
        Else
            tabPrism.SelectedTab = tabOrders
        End If
    End Sub
    Private Sub tsbTransactions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbTransactions.Click
        If tabPrism.TabPages.Contains(tabTransactions) = False Then
            tabPrism.TabPages.Add(tabTransactions)
            tabPrism.SelectedTab = tabTransactions
        Else
            tabPrism.SelectedTab = tabTransactions
        End If
    End Sub
    Private Sub tsbJournal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbJournal.Click
        If tabPrism.TabPages.Contains(tabJournal) = False Then
            tabPrism.TabPages.Add(tabJournal)
            tabPrism.SelectedTab = tabJournal
        Else
            tabPrism.SelectedTab = tabJournal
        End If
    End Sub
    Private Sub tsbJobs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbJobs.Click
        If tabPrism.TabPages.Contains(tabJobs) = False Then
            tabPrism.TabPages.Add(tabJobs)
            tabPrism.SelectedTab = tabJobs
        Else
            tabPrism.SelectedTab = tabJobs
        End If
    End Sub
    Private Sub btnFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilters.Click
        If tabPrism.TabPages.Contains(tabAssetFilters) = False Then
            tabPrism.TabPages.Add(tabAssetFilters)
            tabPrism.SelectedTab = tabAssetFilters
        Else
            tabPrism.SelectedTab = tabAssetFilters
        End If
    End Sub
    Private Sub tsbRecycle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRecycle.Click
        If tabPrism.TabPages.Contains(tabRecycle) = False Then
            tabPrism.TabPages.Add(tabRecycle)
            tabPrism.SelectedTab = tabRecycle
        Else
            tabPrism.SelectedTab = tabRecycle
        End If
    End Sub
    Private Sub ctxTabPrism_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles ctxTabPrism.Closed
        mnuClosePrismTab.Text = "Not Valid"
    End Sub
    Private Sub ctxTabPrism_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxTabPrism.Opening
        If mnuClosePrismTab.Text = "Not Valid" Or tabPrism.TabPages(CInt(tabPrism.Tag)).Text = "API Status" Then
            e.Cancel = True
        End If
    End Sub
    Private Function TabControlHitTest(ByVal TabCtrl As TabControl, ByVal pt As Drawing.Point) As Integer
        ' Test each tabs rectangle to see if our point is contained within it.
        For x As Integer = 0 To TabCtrl.TabPages.Count - 1
            ' If tab contians our rectangle return it's index.
            If TabCtrl.GetTabRect(x).Contains(pt) Then Return x
        Next
        ' A tab was not located at specified point.
        Return -1
    End Function
    Private Sub TabPrism_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tabPrism.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Right
                Dim TabIndex As Integer
                ' Get index of tab clicked
                TabIndex = TabControlHitTest(tabPrism, e.Location)
                ' If a tab was clicked display it's index
                If TabIndex >= 0 Then
                    tabPrism.Tag = TabIndex
                    Dim tp As TabPage = tabPrism.TabPages(CInt(tabPrism.Tag))
                    mnuClosePrismTab.Text = "Close " & tp.Text
                Else
                    mnuClosePrismTab.Text = "Not Valid"
                End If
            Case Windows.Forms.MouseButtons.Middle
                Dim TabIndex As Integer
                ' Get index of tab clicked
                TabIndex = TabControlHitTest(tabPrism, e.Location)
                ' If a tab was clicked display it's index
                If TabIndex > 0 Then
                    tabPrism.Tag = TabIndex
                    Dim tp As TabPage = tabPrism.TabPages(CInt(tabPrism.Tag))
                    tabPrism.TabPages.Remove(tp)
                    tabPrism.Tag = 0
                End If
        End Select
    End Sub
    Private Sub mnuClosePrismTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClosePrismTab.Click
        Dim tp As TabPage = tabPrism.TabPages(CInt(tabPrism.Tag))
        tabPrism.TabPages.Remove(tp)
        tabPrism.Tag = 0
    End Sub

#End Region

#Region "Rig Builder Routines"
    Private Sub GetSalvage()
        SalvageList.Clear()
        For Each cPilot As ListViewItem In lvwCharFilter.CheckedItems
            Dim IsCorp As Boolean = False
            ' Get the owner we will use
            Dim owner As String = cboOwner.SelectedItem.ToString()
            ' See if this owner is a corp
            If PlugInData.CorpList.ContainsKey(owner) = True Then
                IsCorp = True
                ' See if we have a representative
                Dim CorpRep As SortedList = CType(PlugInData.CorpReps(0), Collections.SortedList)
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
            End If
            If owner <> "" Then
                Dim assetXML As New XmlDocument
                Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
                Dim accountName As String = selPilot.Account
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                If IsCorp = True Then
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                Else
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                End If
                If assetXML IsNot Nothing Then
                    Dim locList As XmlNodeList
                    Dim loc As XmlNode
                    locList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                    If locList.Count > 0 Then
                        For Each loc In locList
                            Dim itemID As String = loc.Attributes.GetNamedItem("typeID").Value
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                Dim groupID As String = EveHQ.Core.HQ.itemData(itemID).Group.ToString
                                If CLng(groupID) = 754 Then

                                    Dim quantity As Long = CLng(loc.Attributes.GetNamedItem("quantity").Value)
                                    Dim itemName As String = EveHQ.Core.HQ.itemData(itemID).Name
                                    If SalvageList.Contains(itemName) = False Then
                                        SalvageList.Add(itemName, quantity)
                                    Else
                                        SalvageList.Item(itemName) = CLng(SalvageList.Item(itemName)) + quantity
                                    End If
                                End If
                            End If

                            ' Check if this row has child nodes and repeat
                            If loc.HasChildNodes = True Then
                                Call Me.GetSalvageNode(SalvageList, loc, owner, selPilot)
                            End If
                        Next
                    End If
                End If
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
                If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                    Dim groupID As String = EveHQ.Core.HQ.itemData(itemID).Group.ToString
                    If CLng(groupID) = 754 Then
                        Dim quantity As Long = CLng(subLoc.Attributes.GetNamedItem("quantity").Value)
                        Dim itemName As String = EveHQ.Core.HQ.itemData(itemID).Name
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
        Dim strSQL As String = "SELECT invBuildMaterials.typeID AS invBuildMaterials_typeID, invBuildMaterials.activityID, invBuildMaterials.requiredTypeID, invBuildMaterials.quantity, invBuildMaterials.damagePerJob, invTypes.typeID AS invTypes_typeID, invTypes.groupID, invTypes.published"
        strSQL &= " FROM invTypes INNER JOIN invBuildMaterials ON invTypes.typeID = invBuildMaterials.typeID"
        strSQL &= " WHERE (((invBuildMaterials.activityID)=1) AND ((invTypes.groupID)=787) AND ((invTypes.published)=1));"
        Dim rigData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Dim BPID As String = ""
        Dim BPName As String = ""
        Dim SalvageID As String = ""
        Dim SalvageName As String = ""
        Dim SalvageQ As Double = 0
        Dim groupID As String = ""
        For Each rigRow As DataRow In rigData.Tables(0).Rows
            BPID = rigRow.Item("invTypes_typeID").ToString
            BPName = EveHQ.Core.HQ.itemData(BPID).Name.TrimEnd(" Blueprint".ToCharArray)
            If EveHQ.Core.HQ.itemList.ContainsKey(BPName) = True Then
                ' Add it to the BPList if not already in
                If RigBPData.Contains(BPName) = False Then
                    RigBPData.Add(BPName, New SortedList)
                End If
                ' Read the required type and see if it is salvage (read groupID = 754)
                SalvageID = rigRow.Item("requiredTypeID").ToString
                groupID = EveHQ.Core.HQ.itemData(SalvageID).Group.ToString
                If groupID = "754" Then
                    SalvageName = EveHQ.Core.HQ.itemData(SalvageID).Name
                    SalvageQ = Math.Round(CDbl(rigRow.Item("quantity")) * BPWF, 0)
                    RigBuildData = CType(RigBPData.Item(BPName), Collections.SortedList)
                    RigBuildData.Add(SalvageName, SalvageQ)
                End If
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
            If EveHQ.Core.HQ.itemList.ContainsKey(BP) = True Then
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
                        buildCost += CInt(RigBuildData(material)) * EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(material))
                    Next
                    rigCost = EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(BP))
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
        Dim assetName As String = ""
        tempAssetList.Clear()
        For Each asset As ContainerListViewItem In clvAssets.SelectedItems
            assetName = asset.SubItems(0).Tag.ToString
            If recycleList.ContainsKey(EveHQ.Core.HQ.itemList(assetName)) = True Then
                If asset.SubItems(AssetColumn.Quantity).Text <> "" Then
                    If tempAssetList.Contains(asset.Tag.ToString) = False Then
                        recycleList(EveHQ.Core.HQ.itemList(assetName)) = CLng(recycleList(EveHQ.Core.HQ.itemList(assetName))) + CLng(asset.SubItems(AssetColumn.Quantity).Text)
                        tempAssetList.Add(asset.Tag.ToString)
                    End If
                End If
            Else
                If asset.SubItems(AssetColumn.Quantity).Text <> "" Then
                    If tempAssetList.Contains(asset.Tag.ToString) = False Then
                        recycleList.Add(EveHQ.Core.HQ.itemList(assetName), CLng(asset.SubItems(AssetColumn.Quantity).Text))
                        tempAssetList.Add(asset.Tag.ToString)
                    End If
                End If
            End If
        Next
        tempAssetList.Clear()
        RecyclerAssetList = recycleList
        RecyclerAssetOwner = cboOwner.SelectedItem.ToString
        RecyclerAssetLocation = GetLocationID(clvAssets.SelectedItems(0))
        Call LoadRecyclingInfo()
        If tabPrism.TabPages.Contains(tabRecycle) = False Then
            tabPrism.TabPages.Add(tabRecycle)
            tabPrism.SelectedTab = tabRecycle
        Else
            tabPrism.SelectedTab = tabRecycle
        End If
    End Sub

    Private Sub mnuRecycleContained_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRecycleContained.Click
        Dim recycleList As New SortedList
        tempAssetList.Clear()
        For Each asset As ContainerListViewItem In clvAssets.SelectedItems
            Call Me.AddItemsToRecycleList(asset, recycleList)
        Next
        tempAssetList.Clear()
        RecyclerAssetList = recycleList
        RecyclerAssetOwner = cboOwner.SelectedItem.ToString
        RecyclerAssetLocation = GetLocationID(clvAssets.SelectedItems(0))
        Call Me.LoadRecyclingInfo()
        If tabPrism.TabPages.Contains(tabRecycle) = False Then
            tabPrism.TabPages.Add(tabRecycle)
            tabPrism.SelectedTab = tabRecycle
        Else
            tabPrism.SelectedTab = tabRecycle
        End If
    End Sub

    Private Sub mnuRecycleAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRecycleAll.Click
        Dim recycleList As New SortedList
        Dim assetName As String = ""
        tempAssetList.Clear()
        For Each asset As ContainerListViewItem In clvAssets.SelectedItems
            assetName = asset.SubItems(0).Tag.ToString
            If EveHQ.Core.HQ.itemList.ContainsKey(assetName) = True Then
                If recycleList.ContainsKey(EveHQ.Core.HQ.itemList(assetName)) = True Then
                    If asset.SubItems(AssetColumn.Quantity).Text <> "" Then
                        If tempAssetList.Contains(asset.Tag.ToString) = False Then
                            recycleList(EveHQ.Core.HQ.itemList(assetName)) = CLng(recycleList(EveHQ.Core.HQ.itemList(assetName))) + CLng(asset.SubItems(AssetColumn.Quantity).Text)
                            tempAssetList.Add(asset.Tag.ToString)
                        End If
                    End If
                Else
                    If asset.SubItems(AssetColumn.Quantity).Text <> "" Then
                        If tempAssetList.Contains(asset.Tag.ToString) = False Then
                            recycleList.Add(EveHQ.Core.HQ.itemList(assetName), CLng(asset.SubItems(AssetColumn.Quantity).Text))
                            tempAssetList.Add(asset.Tag.ToString)
                        End If
                    End If
                End If
            End If
            Call Me.AddItemsToRecycleList(asset, recycleList)
        Next
        tempAssetList.Clear()
        RecyclerAssetList = recycleList
        RecyclerAssetOwner = cboOwner.SelectedItem.ToString
        RecyclerAssetLocation = GetLocationID(clvAssets.SelectedItems(0))
        Call Me.LoadRecyclingInfo()
        If tabPrism.TabPages.Contains(tabRecycle) = False Then
            tabPrism.TabPages.Add(tabRecycle)
            tabPrism.SelectedTab = tabRecycle
        Else
            tabPrism.SelectedTab = tabRecycle
        End If
    End Sub

    Private Sub AddItemsToRecycleList(ByVal item As ContainerListViewItem, ByRef assetList As SortedList)
        For Each childItem As ContainerListViewItem In item.Items
            If EveHQ.Core.HQ.itemList.ContainsKey(childItem.Text) = True Then
                If assetList.ContainsKey(EveHQ.Core.HQ.itemList(childItem.Text)) = True Then
                    If childItem.SubItems(AssetColumn.Quantity).Text <> "" Then
                        If tempAssetList.Contains(childItem.Tag.ToString) = False Then
                            assetList(EveHQ.Core.HQ.itemList(childItem.Text)) = CLng(assetList(EveHQ.Core.HQ.itemList(childItem.Text))) + CLng(childItem.SubItems(AssetColumn.Quantity).Text)
                            tempAssetList.Add(childItem.Tag.ToString)
                        End If
                    End If
                Else
                    If childItem.SubItems(AssetColumn.Quantity).Text <> "" Then
                        If tempAssetList.Contains(childItem.Tag.ToString) = False Then
                            assetList.Add(EveHQ.Core.HQ.itemList(childItem.Text), CLng(childItem.SubItems(AssetColumn.Quantity).Text))
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

    Private Function ParseMarketOrders(ByVal owner As String) As MarketOrdersCollection

        Dim newOrderCollection As New MarketOrdersCollection

        Dim IsCorp As Boolean = False
        ' See if this owner is a corp
        If PlugInData.CorpList.ContainsKey(owner) = True Then
            IsCorp = True
            ' See if we have a representative
            Dim CorpRep As SortedList = CType(PlugInData.CorpReps(4), Collections.SortedList)
            If CorpRep IsNot Nothing Then
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
            Else
                owner = ""
            End If
        End If

        If owner <> "" Then
            Dim OrderXML As New XmlDocument
            Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
            Dim accountName As String = selPilot.Account
            Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            If IsCorp = True Then
                OrderXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            Else
                OrderXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            End If
            If OrderXML IsNot Nothing Then
                Dim Orders As XmlNodeList = OrderXML.SelectNodes("/eveapi/result/rowset/row")
                For Each Order As XmlNode In Orders
                    Dim newOrder As New MarketOrder
                    newOrder.OrderID = Order.Attributes.GetNamedItem("orderID").Value
                    newOrder.CharID = Order.Attributes.GetNamedItem("charID").Value
                    newOrder.StationID = Order.Attributes.GetNamedItem("stationID").Value
                    newOrder.VolEntered = CLng(Order.Attributes.GetNamedItem("volEntered").Value)
                    newOrder.VolRemaining = CLng(Order.Attributes.GetNamedItem("volRemaining").Value)
                    newOrder.MinVolume = CLng(Order.Attributes.GetNamedItem("minVolume").Value)
                    newOrder.OrderState = CInt(Order.Attributes.GetNamedItem("orderState").Value)
                    newOrder.TypeID = CInt(Order.Attributes.GetNamedItem("typeID").Value)
                    newOrder.Range = CInt(Order.Attributes.GetNamedItem("range").Value)
                    newOrder.AccountKey = CInt(Order.Attributes.GetNamedItem("accountKey").Value)
                    newOrder.Duration = CInt(Order.Attributes.GetNamedItem("duration").Value)
                    newOrder.Escrow = Double.Parse(Order.Attributes.GetNamedItem("escrow").Value, culture)
                    newOrder.Price = Double.Parse(Order.Attributes.GetNamedItem("price").Value, culture)
                    newOrder.Bid = CInt(Order.Attributes.GetNamedItem("bid").Value)
                    newOrder.Issued = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                    newOrderCollection.MarketOrders.Add(newOrder)
                    If newOrder.Bid = 0 Then ' Sell Order
                        newOrderCollection.SellOrders += 1
                        newOrderCollection.SellOrderValue += (newOrder.VolRemaining * newOrder.Price)
                    Else ' Buy Order (=1)
                        newOrderCollection.BuyOrders += 1
                        newOrderCollection.BuyOrderValue += (newOrder.VolRemaining * newOrder.Price)
                        newOrderCollection.EscrowValue += newOrder.Escrow
                    End If
                Next
                newOrderCollection.TotalOrders = newOrderCollection.BuyOrders + newOrderCollection.SellOrders
            End If
        End If

        Return newOrderCollection

    End Function

    Private Sub ParseOrders()
        ' Get the owner we will use
        Dim owner As String = cboOwner.SelectedItem.ToString
        Dim IsCorp As Boolean = False
        ' See if this owner is a corp
        If PlugInData.CorpList.ContainsKey(owner) = True Then
            IsCorp = True
            ' See if we have a representative
            Dim CorpRep As SortedList = CType(PlugInData.CorpReps(4), Collections.SortedList)
            If CorpRep IsNot Nothing Then
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
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
                OrderXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            Else
                OrderXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            End If
            If OrderXML IsNot Nothing Then
                Dim Orders As XmlNodeList = OrderXML.SelectNodes("/eveapi/result/rowset/row")
                clvBuyOrders.BeginUpdate()
                clvSellOrders.BeginUpdate()
                clvBuyOrders.Items.Clear()
                clvSellOrders.Items.Clear()
                For Each Order As XmlNode In Orders
                    If Order.Attributes.GetNamedItem("bid").Value = "0" Then
                        If Order.Attributes.GetNamedItem("orderState").Value = "0" Then
                            Dim sOrder As New ContainerListViewItem
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
                            sOrder.SubItems(1).Text = FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Dim price As Double = Double.Parse(Order.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Number, culture)
                            sOrder.SubItems(2).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Dim loc As String = ""
                            If PlugInData.stations.Contains(Order.Attributes.GetNamedItem("stationID").Value) = True Then
                                loc = CType(PlugInData.stations(Order.Attributes.GetNamedItem("stationID").Value), Station).stationName
                            Else
                                loc = "StationID: " & Order.Attributes.GetNamedItem("stationID").Value
                            End If
                            sOrder.SubItems(3).Text = loc
                            Dim issueDate As Date = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                            Dim orderExpires As TimeSpan = issueDate - Now
                            orderExpires = orderExpires.Add(New TimeSpan(CInt(Order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                            sOrder.SubItems(4).Tag = orderExpires
                            If orderExpires.TotalSeconds <= 0 Then
                                sOrder.SubItems(4).Text = "Expired!"
                            Else
                                sOrder.SubItems(4).Text = EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False)
                            End If
                            sellTotal = sellTotal + quantity * price
                            TotalOrders = TotalOrders + 1
                        End If
                    Else
                        If Order.Attributes.GetNamedItem("orderState").Value = "0" Then
                            Dim bOrder As New ContainerListViewItem
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
                            bOrder.SubItems(1).Text = FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Dim price As Double = Double.Parse(Order.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Number, culture)
                            bOrder.SubItems(2).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Dim loc As String = ""
                            If PlugInData.stations.Contains(Order.Attributes.GetNamedItem("stationID").Value) = True Then
                                loc = CType(PlugInData.stations(Order.Attributes.GetNamedItem("stationID").Value), Station).stationName
                            Else
                                loc = "StationID: " & Order.Attributes.GetNamedItem("stationID").Value
                            End If
                            bOrder.SubItems(3).Text = loc
                            bOrder.SubItems(4).Tag = CInt(Order.Attributes.GetNamedItem("range").Value)
                            Select Case CInt(Order.Attributes.GetNamedItem("range").Value)
                                Case -1
                                    bOrder.SubItems(4).Text = "Station"
                                Case 0
                                    bOrder.SubItems(4).Text = "System"
                                Case 32767
                                    bOrder.SubItems(4).Text = "Region"
                                Case Is > 0, Is < 32767
                                    bOrder.SubItems(4).Text = Order.Attributes.GetNamedItem("range").Value & " Jumps"
                            End Select
                            bOrder.SubItems(5).Text = FormatNumber(Double.Parse(Order.Attributes.GetNamedItem("minVolume").Value, culture), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Dim issueDate As Date = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                            Dim orderExpires As TimeSpan = issueDate - Now
                            orderExpires = orderExpires.Add(New TimeSpan(CInt(Order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                            bOrder.SubItems(6).Tag = orderExpires
                            If orderExpires.TotalSeconds <= 0 Then
                                bOrder.SubItems(6).Text = "Expired!"
                            Else
                                bOrder.SubItems(6).Text = EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False)
                            End If
                            buyTotal = buyTotal + quantity * price
                            TotalEscrow = TotalEscrow + Double.Parse(Order.Attributes.GetNamedItem("escrow").Value, culture)
                            TotalOrders = TotalOrders + 1
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
            lblOrders.Text = (maxorders - TotalOrders).ToString + " / " + maxorders.ToString
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

#Region "Wallet Transaction & Journal Routines"
    Private Sub ParseWalletTransactions()
        Dim IsCorp As Boolean = False
        ' Get the owner we will use
        Dim owner As String = cboOwner.SelectedItem.ToString()
        ' See if this owner is a corp
        If PlugInData.CorpList.ContainsKey(owner) = True Then
            IsCorp = True
            cboWalletTransDivision.Enabled = True
            ' See if we have a representative
            Dim CorpRep As SortedList = CType(PlugInData.CorpReps(5), Collections.SortedList)
            If CorpRep IsNot Nothing Then
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
            Else
                owner = ""
            End If
        Else
            cboWalletTransDivision.Enabled = False
        End If

        If cmbWalletTransType.SelectedIndex = -1 Then cmbWalletTransType.SelectedIndex = 0

        If owner <> "" Then
            Dim transXML As New XmlDocument
            Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
            Dim accountName As String = selPilot.Account
            Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            If IsCorp = True Then
                transXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransCorp, pilotAccount, selPilot.ID, 1000 + cboWalletTransDivision.SelectedIndex, "", EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            Else
                transXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransChar, pilotAccount, selPilot.ID, 1000 + cboWalletTransDivision.SelectedIndex, "", EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            End If
            If transXML IsNot Nothing Then
                Dim Trans As XmlNodeList = transXML.SelectNodes("/eveapi/result/rowset/row")
                Dim transItem As New ContainerListViewItem
                Dim transDate As Date
                Dim transP, transQ, transV As Double
                clvTransactions.BeginUpdate()
                clvTransactions.Items.Clear()
                For Each Tran As XmlNode In Trans

                    If Not cmbWalletTransType.SelectedIndex <= 0 Then
                        If CBool(String.Compare(Tran.Attributes.GetNamedItem("transactionType").Value, cmbWalletTransType.SelectedItem.ToString, True)) Then
                            Continue For
                        End If
                    End If

                    transItem = New ContainerListViewItem
                    If IsCorp = False And Tran.Attributes.GetNamedItem("transactionFor").Value = "corporation" Then
                        transItem.ForeColor = Drawing.Color.SlateBlue
                    Else
                        transItem.ForeColor = Drawing.Color.Black
                    End If
                    transDate = DateTime.ParseExact(Tran.Attributes.GetNamedItem("transactionDateTime").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                    transItem.Text = FormatDateTime(transDate, DateFormat.GeneralDate)
                    clvTransactions.Items.Add(transItem)
                    transItem.SubItems(1).Text = Tran.Attributes.GetNamedItem("typeName").Value
                    transP = Double.Parse(Tran.Attributes.GetNamedItem("price").Value, culture)
                    transQ = Double.Parse(Tran.Attributes.GetNamedItem("quantity").Value, culture)
                    transItem.SubItems(2).Text = FormatNumber(transQ, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    transItem.SubItems(3).Text = FormatNumber(transP, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    If Tran.Attributes.GetNamedItem("transactionType").Value = "buy" Then
                        transItem.SubItems(4).ForeColor = Drawing.Color.Red
                        transV = -transP * transQ
                    Else
                        transItem.SubItems(4).ForeColor = Drawing.Color.Green
                        transV = transP * transQ
                    End If
                    transItem.SubItems(4).Text = FormatNumber(transV, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    transItem.SubItems(5).Text = Tran.Attributes.GetNamedItem("stationName").Value
                    transItem.SubItems(6).Text = Tran.Attributes.GetNamedItem("clientName").Value
                Next
                If clvTransactions.Items.Count = 0 Then
                    clvTransactions.Items.Add("No Data Available...")
                    clvTransactions.Enabled = False
                Else
                    clvTransactions.Enabled = True
                End If
                clvTransactions.EndUpdate()
            End If
        Else
            clvTransactions.BeginUpdate()
            clvTransactions.Items.Clear()
            clvTransactions.Items.Add("Access Denied - check API Status")
            clvTransactions.EndUpdate()
            clvTransactions.Enabled = False
        End If

    End Sub

    Private Sub ParseWalletJournal()
        Dim IsCorp As Boolean = False
        ' Get the owner we will use
        Dim owner As String = cboOwner.SelectedItem.ToString()
        ' See if this owner is a corp
        If PlugInData.CorpList.ContainsKey(owner) = True Then
            IsCorp = True
            cboWalletJournalDivision.Enabled = True
            ' See if we have a representative
            Dim CorpRep As SortedList = CType(PlugInData.CorpReps(3), Collections.SortedList)
            If CorpRep IsNot Nothing Then
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
            Else
                owner = ""
            End If
        Else
            cboWalletJournalDivision.Enabled = False
        End If

        If owner <> "" Then
            Dim transXML As New XmlDocument
            Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
            Dim accountName As String = selPilot.Account
            Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            If IsCorp = True Then
                transXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp, pilotAccount, selPilot.ID, 1000 + cboWalletJournalDivision.SelectedIndex, "", EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            Else
                transXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, pilotAccount, selPilot.ID, 1000 + cboWalletJournalDivision.SelectedIndex, "", EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            End If
            If transXML IsNot Nothing Then
                Dim Trans As XmlNodeList = transXML.SelectNodes("/eveapi/result/rowset/row")
                Dim transItem As New ContainerListViewItem
                Dim transDate As Date
                Dim transA, transB As Double
                Dim refType As String = ""
                clvJournal.BeginUpdate()
                clvJournal.Items.Clear()
                For Each Tran As XmlNode In Trans
                    transItem = New ContainerListViewItem
                    transDate = DateTime.ParseExact(Tran.Attributes.GetNamedItem("date").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                    transItem.Text = FormatDateTime(transDate, DateFormat.GeneralDate)
                    clvJournal.Items.Add(transItem)
                    refType = Tran.Attributes.GetNamedItem("refTypeID").Value
                    transItem.SubItems(1).Text = PlugInData.RefTypes(refType)
                    transA = Double.Parse(Tran.Attributes.GetNamedItem("amount").Value, culture)
                    transB = Double.Parse(Tran.Attributes.GetNamedItem("balance").Value, culture)
                    transItem.SubItems(2).Text = FormatNumber(transA, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    transItem.SubItems(3).Text = FormatNumber(transB, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    If transA < 0 Then
                        transItem.SubItems(2).ForeColor = Drawing.Color.Red
                    Else
                        transItem.SubItems(2).ForeColor = Drawing.Color.Green
                    End If
                    If Tran.Attributes.GetNamedItem("reason").Value <> "" Then
                        transItem.SubItems(4).Text = "[r] " & BuildJournalDescription(CInt(refType), Tran.Attributes.GetNamedItem("ownerName1").Value, Tran.Attributes.GetNamedItem("ownerName2").Value, Tran.Attributes.GetNamedItem("argID1").Value, Tran.Attributes.GetNamedItem("argName1").Value)
                        Dim transReason As New ContainerListViewItem
                        transItem.Items.Add(transReason)
                        transReason.SubItems(4).Text = Tran.Attributes.GetNamedItem("reason").Value
                    Else
                        transItem.SubItems(4).Text = BuildJournalDescription(CInt(refType), Tran.Attributes.GetNamedItem("ownerName1").Value, Tran.Attributes.GetNamedItem("ownerName2").Value, Tran.Attributes.GetNamedItem("argID1").Value, Tran.Attributes.GetNamedItem("argName1").Value)
                    End If
                Next
                If clvJournal.Items.Count = 0 Then
                    clvJournal.Items.Add("No Data Available...")
                    clvJournal.Enabled = False
                Else
                    clvJournal.Enabled = True
                End If
                clvJournal.EndUpdate()
            End If
        Else
            clvJournal.BeginUpdate()
            clvJournal.Items.Clear()
            clvJournal.Items.Add("Access Denied - check API Status")
            clvJournal.EndUpdate()
            clvJournal.Enabled = False
        End If

    End Sub

    Function BuildJournalDescription(ByVal refType As Integer, ByVal owner1 As String, ByVal owner2 As String, ByVal argID1 As String, ByVal argName1 As String) As String
        Dim misc As String = ", refType=" & CStr(refType) & ", arg1=" & argName1 & ", own1 =" & owner1 & ", own2=" & owner2
        Dim desc As String = PlugInData.RefTypes(refType.ToString)
        Select Case refType ' Only use items which require a change

            Case 0 'undefined
                desc = "Undefined" & misc
            Case 1 'Player Trading
                desc = "Direct trade between " & owner1 & " and " & owner2
            Case 2 'Market Transaction
                desc = "Market: " & owner1 & " bought stuff from " & owner2
            Case 3 'GM cash transfer
                desc = "GM Cash Transfer" & misc
            Case 4 ' ATM Withdraw
                desc = "ATM Withdraw" & misc
            Case 5 ' ATM Deposit
                desc = "ATM Deposit" & misc
            Case 6 ' backward compatible
                desc = "Backward Compatible" & misc
            Case 7 ' Mission reward
                desc = "Mission Reward" & misc
            Case 8 ' Clone Activation
                desc = "Clone Activation"
            Case 9 ' Inheritance
                desc = "Inheritance" & misc
            Case 10 'Player Donation 
                desc = owner1 & " deposited cash in " & owner2 & "'s account"
            Case 11 'Corporation Payment
                desc = "Corporation Payment" & misc
            Case 12 'Docking Fee'
                desc = "Docking Fee" & misc
            Case 13 'Office Rental Fee
                desc = "Office Rental Fee payed to" & owner2 & " by " & owner2
            Case 14 'Factory Slot Rental Fee
                desc = "Factory Slot Rental Fee" & misc
            Case 15 'Repair Bill
                desc = "Repair Bill between " & owner1 & " and " & owner2
            Case 16 'Bounty
                desc = "Bounty" & misc
            Case 17 ' Bounty
                desc = owner1 & " recieved bounty prizes for killing pirates in" & argName1
            Case 18 ' Agents Temp
                desc = "Agents Temporary" & misc
            Case 19 ' Insurance
                If CInt(argName1) > 0 Then
                    Dim itemName As String = ""
                    If EveHQ.Core.HQ.itemData.ContainsKey(argName1) = True Then
                        itemName = EveHQ.Core.HQ.itemData(argName1).Name
                    Else
                        itemName = "ship"
                    End If
                    desc = "Insurance paid by EVE Central Bank to " & owner1 & " covering loss of a " & itemName
                End If
                If CInt(argName1) < 0 Then
                    Dim sid As Integer = -1 * CInt(argName1)
                    desc = "Insurance paid by " & owner1 & " to " & owner2 & "(Insurance RefID:" & CStr(sid) & ")"
                End If
            Case 20 'Mission Expiration
                desc = "Mission Expiration" & misc
            Case 21 'Mission Completion
                desc = "Mission Completion" & misc
            Case 22 'Shares
                desc = "Shares" & misc
            Case 23 'Courier Mission Escrow
                desc = "Courier Mission Escrow " & misc
            Case 24 'Mission Cost
                desc = "Mission Cost " & misc
            Case 25 'Agent Miscellaneous
                desc = "Agent Miscellaneous" & misc
            Case 26 'Miscellaneous Payment To Agent
                desc = "Miscellaneous Payment To Agent " & misc
            Case 27 'Agent Location Services
                desc = "Agent Location Service" & misc
            Case 28 'Agent Donation
                desc = "Agent Donation " & misc
            Case 29 'Agent Security Services
                desc = "Agent Security Services " & misc
            Case 30 'Agent Mission Collateral Paid
                desc = "Agent Mission Collateral Paid " & misc
            Case 31 'Agent Mission Collateral Refunded
                desc = "Agent Mission Collateral Refunded " & misc
            Case 32 'Agents_preward
                desc = "Agents Preward " & misc
            Case 33 'Agent Mission Reward
                'desc = "Agent Mission Reward" & misc
                desc = owner2 & " recieved mission reward from agent " & owner1
            Case 34 'Agent Mission Time Bonus Reward
                desc = owner2 & " received mission time bonus reward from agent " & owner1
            Case 35 ' CSPA Charges
                desc = "CSPA for communication with " & argName1
            Case 36 'CSPAOfflineRefund
                desc = "CSPA Offline Refund " & misc
            Case 37 'Corp Account Withdraw
                desc = argName1 & " transferred cash from " & owner1 & "'s corporate account to " & owner2 & "'s account"
            Case 38 'Corporation Dividend Payment
                If owner1 = "" Then
                    ' Receiving Dividend
                    desc = "Dividend received from " & argName1
                Else
                    ' Paying Dividend
                    desc = "Dividend payment made by " & owner1
                End If
            Case 39 'Corporation Registration Fee
                If owner1 = "CONCORD" Then
                    desc = owner1 & " refunded corporation registration fee"
                Else
                    desc = owner1 & " paid corporation registration fee"
                End If
            Case 40 'Corporation Logo Change Cost
                desc = misc
            Case 41 'Release Of Impounded Property
                desc = misc
            Case 42 'market escrow
                desc = "Market escrow authorized by " & owner1
            Case 43 'Agent Services Rendered
                desc = misc
            Case 44 'Market Fine Paid
                desc = misc
            Case 45 'Corporation Liquidation
                desc = misc
            Case 46 'Broker fee
                desc = "Brokers fee authorized by " & owner1
            Case 47 'Corporation Bulk Payment
                desc = misc
            Case 48 'Alliance Registration Fee
                desc = "Alliance registration fee paid to " & owner2
            Case 49 'War Fee
                desc = misc
            Case 50 'Alliance Maintainance Fee
                desc = "Alliance maintenance fee paid for membership in " & argName1
            Case 51 'Contraband Fine 
                desc = misc
            Case 52 'Clone Transfer
                desc = owner1 & " transfered clone to " & argName1
            Case 53 'Acceleration Gate Fee
                desc = misc
            Case 54 ' Transaction tax
                desc = "Sales tax paid to the SCC"
            Case 55 'Jump Clone Installation Fee
                desc = "Jump Clone Installation Fee " & misc
            Case 56 ' Manufacturing
                desc = "Manufacturing job fee between " & owner1 & " and " & owner2 & " (Job ID:" & argName1 & ")"
            Case 57 'Researching Technology
                desc = misc
            Case 58 'Researching Time Productivity
                desc = "Time productivity research job fee between " & owner1 & " and " & owner2 & " (Job ID:" & argName1 & ")"
            Case 59 'Researching Material Productivity
                desc = "Material productivity research job fee between " & owner1 & " and " & owner2 & " (Job ID:" & argName1 & ")"
            Case 60 'Copying
                desc = "Blueprint copying job fee between " & owner1 & " and " & owner2 & " (Job ID:" & argName1 & ")"
            Case 61 'Duplicating
                desc = misc
            Case 62 'Reverse Engineering
                desc = misc
            Case 63 'Contract
                desc = owner1 & " bid on an auction (ref:" & argName1 & ")"
            Case 64 'Contract Auction Bid Refund
                desc = owner2 & " received a refund on a contract aution bid (ref:" & argName1 & ")"
            Case 65 'Contract Collateral
                desc = misc
            Case 66 'Contract Reward Refund
                desc = misc
            Case 67 'Contract Auction Sold
                desc = "Price for Contract auction sold"
            Case 68 'Contract Reward
                desc = misc
            Case 69 'Contract Collateral Refund
                desc = misc
            Case 70 'Contract Collateral Payout
                desc = misc
            Case 71 'Contract Price
                desc = owner1 & " accepted a contract from " & owner2 & " (Contract ID: " & argName1 & ")"
            Case 72 'Contract Brokers Fee
                desc = "Contract Brokers Fee (Contract ID: " & argName1 & ")"
            Case 73 'Contract Sales Tax
                desc = "Contract Sales Tax (Contract ID: " & argName1 & ")"
            Case 74 'Contract Deposit
                desc = "Contract Deposit (Contract ID: " & argName1 & ")"
            Case 75 'Contract Deposit Sales Tax
                desc = misc
            Case 76 'Secure EVE Time Code Exchange
                desc = misc
            Case 77 'Contract Auction Bid (corp)
                desc = owner1 & " bid on a contract auction for the corp (Ref:" & argName1 & ")"
            Case 78 'Contract Collateral Deposited (corp)
                desc = misc
            Case 79 'Contract Price Payment (corp)
                desc = owner1 & " accepted a contract from " & owner2 & " (Contract ID: " & argName1 & ")"
            Case 80 'Contract Brokers Fee (corp)
                desc = "Corporation Contract Brokers Fee (Contract ID: " & argName1 & ")"
            Case 81 'Contract Deposit (corp)
                desc = "Corporation Contract Deposit (Contract ID: " & argName1 & ")"
            Case 82 'Contract Deposit Refund
                desc = "Contract Deposit Refund  (Contract ID: " & argName1 & ")"
            Case 83 'Contract Reward Deposited
                desc = misc
            Case 84 'Contract Reward Deposited (corp)
                desc = misc
            Case 85 ' Bounty owner1=concord
                desc = owner2 & " Got bounty prizes for killing pirates in " & argName1
            Case 86 'Advertisement Listing Fee
                desc = misc
        End Select
        Return desc
    End Function

    Private Sub cboWalletTransDivision_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWalletTransDivision.SelectedIndexChanged
        Call ParseWalletTransactions()
    End Sub

    Private Sub cboWalletJournalDivision_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWalletJournalDivision.SelectedIndexChanged
        Call ParseWalletJournal()
    End Sub

    Private Sub cmbWalletTransType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWalletTransType.SelectedIndexChanged
        Call Me.ParseWalletTransactions()
    End Sub

    Private Sub UpdateWalletDivisions()
        Dim owner As String = cboOwner.SelectedItem.ToString()
        Dim IsCorp As Boolean = False
        If PlugInData.CorpList.ContainsKey(owner) = True Then
            IsCorp = True
            cboWalletTransDivision.Enabled = True
            cboWalletJournalDivision.Enabled = True
            ' See if we have a representative
            Dim CorpRep As SortedList = CType(PlugInData.CorpReps(6), Collections.SortedList)
            If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
            Else
                owner = ""
            End If
        Else
            cboWalletTransDivision.Enabled = False
            cboWalletJournalDivision.Enabled = False
        End If

        If owner <> "" Then
            Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
            Dim accountName As String = selPilot.Account
            Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            cboWalletTransDivision.BeginUpdate() : cboWalletJournalDivision.BeginUpdate()
            cboWalletTransDivision.Items.Clear() : cboWalletJournalDivision.Items.Clear()
            If IsCorp = True Then
                Dim corpXML As New XmlDocument
                corpXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpSheet, pilotAccount, selPilot.ID, 1)
                Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                If errlist.Count = 0 Then
                    ' No errors so parse the files
                    Dim divList As XmlNodeList = corpXML.SelectNodes("/eveapi/result/rowset")
                    For Each div As XmlNode In divList
                        Select Case div.Attributes.GetNamedItem("name").Value
                            Case "walletDivisions"
                                For Each divName As XmlNode In div.ChildNodes
                                    cboWalletTransDivision.Items.Add(divName.Attributes.GetNamedItem("description").Value)
                                    cboWalletJournalDivision.Items.Add(divName.Attributes.GetNamedItem("description").Value)
                                Next
                        End Select
                    Next
                Else
                    For div As Integer = 1000 To 1006
                        cboWalletTransDivision.Items.Add(div.ToString.Trim)
                        cboWalletJournalDivision.Items.Add(div.ToString.Trim)
                    Next
                End If
            Else
                cboWalletTransDivision.Items.Add("Personal")
                cboWalletJournalDivision.Items.Add("Personal")
            End If
            cboWalletTransDivision.EndUpdate() : cboWalletJournalDivision.EndUpdate()
        End If
    End Sub
#End Region

#Region "Wallet Database Routines"
    Private Sub WriteTransactionsToDB(ByVal transList As ArrayList)
        Dim lastTrans As Integer = 0
        Dim transID As Integer = 0
        For Each transNode As XmlNode In transList
            transID = CInt(transNode.Attributes.GetNamedItem("transactionID").Value)
            If transID > lastTrans Then
                ' Write the transaction

            Else
                ' Leave the loop as there will be no more writeable transactions
                Exit For
            End If
        Next
    End Sub
#End Region

#Region "Industry Jobs Routines"
    Private Sub ParseIndustryJobs()
        Dim IsCorp As Boolean = False
        ' Get the owner we will use
        Dim owner As String = cboOwner.SelectedItem.ToString()
        ' See if this owner is a corp
        If PlugInData.CorpList.ContainsKey(owner) = True Then
            IsCorp = True
            ' See if we have a representative
            Dim CorpRep As SortedList = CType(PlugInData.CorpReps(2), Collections.SortedList)
            If CorpRep IsNot Nothing Then
                If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                    owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                Else
                    owner = ""
                End If
            Else
                owner = ""
            End If
        End If

        If owner <> "" Then
            Dim transXML As New XmlDocument
            Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
            Dim accountName As String = selPilot.Account
            Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
            If IsCorp = True Then
                transXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            Else
                transXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
            End If
            If transXML IsNot Nothing Then
                Dim pilotIDs As New SortedList
                ' Get a list of pilot IDs and names for our installer routines
                For Each iPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                    If pilotIDs.ContainsKey(iPilot.ID) = False Then
                        pilotIDs.Add(iPilot.ID, iPilot.Name)
                    End If
                Next

                ' Get the Node List
                Dim Trans As XmlNodeList = transXML.SelectNodes("/eveapi/result/rowset/row")

                ' Initialise the installer filter
                cboInstallerFilter.Tag = True
                Dim oldFilter As String = ""
                If cboInstallerFilter.SelectedItem IsNot Nothing Then
                    oldFilter = cboInstallerFilter.SelectedItem.ToString
                End If
                cboInstallerFilter.Items.Clear()
                cboInstallerFilter.BeginUpdate()
                cboInstallerFilter.Items.Add("<All Installers>")
                Dim installerID As String = ""
                Dim installerName As String = ""
                For Each Tran As XmlNode In Trans
                    installerID = Tran.Attributes.GetNamedItem("installerID").Value
                    If pilotIDs.Contains(installerID) = True Then
                        installerName = CStr(pilotIDs(installerID))
                    Else
                        installerName = installerID
                    End If
                    If cboInstallerFilter.Items.Contains(installerName) = False Then
                        cboInstallerFilter.Items.Add(installerName)
                    End If
                Next
                If oldFilter = "" Then
                    cboInstallerFilter.SelectedIndex = 0
                Else
                    If cboInstallerFilter.Items.Contains(oldFilter) = True Then
                        cboInstallerFilter.SelectedItem = oldFilter
                    Else
                        cboInstallerFilter.SelectedIndex = 0
                    End If
                End If
                cboInstallerFilter.EndUpdate()
                cboInstallerFilter.Tag = False

                ' Parse the XML
                Dim transItem As New ContainerListViewItem
                Dim transDate As Date
                Dim transTypeID As String = ""
                Dim locationID As String = ""
                Dim completed As String = ""
                clvJobs.BeginUpdate()
                clvJobs.Items.Clear()
                For Each Tran As XmlNode In Trans
                    installerID = Tran.Attributes.GetNamedItem("installerID").Value
                    If pilotIDs.Contains(installerID) = True Then
                        installerName = CStr(pilotIDs(installerID))
                    Else
                        installerName = installerID
                    End If
                    If cboInstallerFilter.SelectedIndex = 0 Or (cboInstallerFilter.SelectedIndex > 0 And installerName = cboInstallerFilter.SelectedItem.ToString) Then
                        transItem = New ContainerListViewItem
                        transTypeID = Tran.Attributes.GetNamedItem("installedItemTypeID").Value
                        If EveHQ.Core.HQ.itemData.ContainsKey(transTypeID) = True Then
                            transItem.Text = EveHQ.Core.HQ.itemData(transTypeID).Name
                        Else
                            transItem.Text = "Unknown Item ID:" & transTypeID
                        End If
                        clvJobs.Items.Add(transItem)
                        transItem.SubItems(1).Text = PlugInData.Activities(Tran.Attributes.GetNamedItem("activityID").Value)
                        transItem.SubItems(2).Text = Tran.Attributes.GetNamedItem("runs").Value

                        transItem.SubItems(3).Text = installerName
                        locationID = Tran.Attributes.GetNamedItem("installedItemLocationID").Value
                        If PlugInData.stations.ContainsKey(locationID) = True Then
                            transItem.SubItems(4).Text = CType(PlugInData.stations(locationID), Station).stationName
                        Else
                            If PlugInData.stations.ContainsKey(Tran.Attributes.GetNamedItem("outputLocationID").Value) = True Then
                                transItem.SubItems(4).Text = CType(PlugInData.stations(Tran.Attributes.GetNamedItem("outputLocationID").Value), Station).stationName
                            Else
                                transItem.SubItems(4).Text = "POS in " & CType(PlugInData.stations(Tran.Attributes.GetNamedItem("installedInSolarSystemID").Value), SolarSystem).Name
                            End If
                        End If
                        transDate = DateTime.ParseExact(Tran.Attributes.GetNamedItem("endProductionTime").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                        transItem.SubItems(5).Text = FormatDateTime(transDate, DateFormat.GeneralDate)
                        completed = Tran.Attributes.GetNamedItem("completed").Value
                        If completed = "0" Then
                            If transDate < DateTime.Now.ToUniversalTime Then
                                transItem.SubItems(6).Text = "Finished but not Delivered"
                            Else
                                transItem.SubItems(6).Text = "In Progress"
                            End If

                        Else
                            transItem.SubItems(6).Text = PlugInData.Statuses(Tran.Attributes.GetNamedItem("completedStatus").Value)
                        End If
                    End If
                Next
                If clvJobs.Items.Count = 0 Then
                    clvJobs.Items.Add("No Data Available...")
                    clvJobs.Enabled = False
                Else
                    clvJobs.Enabled = True
                End If
                clvJobs.EndUpdate()
            End If
        Else
            clvJobs.BeginUpdate()
            clvJobs.Items.Clear()
            clvJobs.Items.Add("Access Denied - check API Status")
            clvJobs.EndUpdate()
            clvJobs.Enabled = False
        End If
    End Sub

    Private Sub cboInstallerFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboInstallerFilter.SelectedIndexChanged
        If CBool(cboInstallerFilter.Tag) = False Then
            ' We are not triggering a change in the selected item from the main drawing routine
            Call ParseIndustryJobs()
        End If
    End Sub

#End Region

#Region "Recycler Routines"

    Private Sub LoadRecyclingInfo()
        ' Form a string of the asset IDs in the AssetList Property
        Dim strAssets As New StringBuilder
        For Each asset As String In RecyclerAssetList.Keys
            strAssets.Append(", " & asset)
        Next
        strAssets.Remove(0, 2)

        ' Fetch the data from the database
        Dim strSQL As String = "SELECT invTypeMaterials.typeID AS itemTypeID, invTypes.typeID AS materialTypeID, invTypes.typeName AS materialTypeName, invTypeMaterials.quantity AS materialQuantity"
        strSQL &= " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN invTypeMaterials ON invTypes.typeID = invTypeMaterials.materialTypeID) ON invCategories.categoryID = invGroups.categoryID"
        strSQL &= " WHERE (invTypeMaterials.typeID IN (" & strAssets.ToString & ") AND invTypes.groupID NOT IN (268,269,270,332)) ORDER BY invCategories.categoryName, invGroups.groupName"
        Dim mDataSet As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        ' Add the data into a collection for parsing

        itemList.Clear()
        With mDataSet.Tables(0)
            For row As Integer = 0 To .Rows.Count - 1
                If itemList.ContainsKey(.Rows(row).Item("itemTypeID").ToString.Trim) = True Then
                    matList = CType(itemList(.Rows(row).Item("itemTypeID").ToString.Trim), Collections.SortedList)
                Else
                    matList = New SortedList
                    itemList.Add(.Rows(row).Item("itemTypeID").ToString.Trim, matList)
                End If
                matList.Add(.Rows(row).Item("materialTypeName").ToString.Trim, CLng(.Rows(row).Item("materialQuantity").ToString.Trim))
            Next
        End With

        ' Load the characters into the combobox
        cboRecyclePilots.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboRecyclePilots.Items.Add(cPilot.Name)
            End If
        Next

        ' Get the location details
        If PlugInData.stations.ContainsKey(RecyclerAssetLocation) = True Then
            If CDbl(RecyclerAssetLocation) >= 60000000 Then ' Is a station
                Dim aLocation As Station = CType(PlugInData.stations(RecyclerAssetLocation), Station)
                lblStation.Text = aLocation.stationName
                lblCorp.Text = aLocation.corpID.ToString
                If PlugInData.NPCCorps.ContainsKey(aLocation.corpID.ToString) = True Then
                    lblCorp.Text = CStr(PlugInData.NPCCorps(aLocation.corpID.ToString))
                    lblCorp.Tag = aLocation.corpID.ToString
                    StationYield = aLocation.refiningEff
                    lblBaseYield.Text = FormatNumber(StationYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                Else
                    If PlugInData.NPCCorps.ContainsKey(aLocation.corpID.ToString) = True Then
                        lblCorp.Text = CStr(PlugInData.Corps(aLocation.corpID.ToString))
                        lblBaseYield.Text = FormatNumber(aLocation.refiningEff * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    Else
                        lblCorp.Text = "Unknown"
                        lblCorp.Tag = Nothing
                        lblBaseYield.Text = FormatNumber(50, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    End If
                End If
            Else ' Is a system
                lblStation.Text = "n/a"
                lblCorp.Text = "n/a"
                lblCorp.Tag = Nothing
                lblBaseYield.Text = FormatNumber(50, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            End If
        Else
            lblStation.Text = "n/a"
            lblCorp.Text = "n/a"
            lblCorp.Tag = Nothing
            lblBaseYield.Text = FormatNumber(50, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        End If

        ' Set the pilot to the recycling one
        If cboRecyclePilots.Items.Contains(RecyclerAssetOwner) Then
            cboRecyclePilots.SelectedItem = RecyclerAssetOwner
        Else
            If cboRecyclePilots.Items.Contains(cboOwner.SelectedItem.ToString) Then
                cboRecyclePilots.SelectedItem = cboOwner.SelectedItem.ToString
            Else
                cboRecyclePilots.SelectedIndex = 0
            End If
        End If

        ' Set the recycling mode
        cboRefineMode.SelectedIndex = 0
    End Sub
    Private Sub RecalcRecycling()
        ' Create the main list
        clvRecycle.BeginUpdate()
        clvRecycle.Items.Clear()
        Dim price As Double = 0
        Dim perfect As Long = 0
        Dim quantity As Long = 0
        Dim quant As Long = 0
        Dim wastage As Long = 0
        Dim taken As Long = 0
        Dim value As Double = 0
        Dim fees As Double = 0
        Dim sale As Double = 0
        Dim recycleTotal As Double = 0
        Dim newCLVItem As New ContainerListViewItem
        Dim newCLVSubItem As New ContainerListViewItem
        Dim itemInfo As New EveHQ.Core.EveItem
        Dim batches As Integer = 0
        Dim items As Long = 0
        Dim volume As Double = 0
        Dim benefit As Double = 0
        Dim tempNetYield As Double = 0
        Dim bestPriceTotal As Double = 0
        Dim salePriceTotal As Double = 0
        Dim refinePriceTotal As Double = 0
        Dim RecycleResults As New SortedList
        Dim RecycleWaste As New SortedList
        Dim RecycleTake As New SortedList
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
        For Each asset As String In RecyclerAssetList.Keys
            itemInfo = EveHQ.Core.HQ.itemData(asset)
            If itemInfo.Category = 25 Then
                Select Case itemInfo.Group
                    Case 465 ' Ice
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.IceProc)))) + BaseYield
                    Case 450 ' Arkonor
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ArkonorProc)))) + BaseYield
                    Case 451 ' Bistot
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BistotProc)))) + BaseYield
                    Case 452 ' Crokite
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.CrokiteProc)))) + BaseYield
                    Case 453 ' Dark Ochre
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.DarkOchreProc)))) + BaseYield
                    Case 467 ' Gneiss
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.GneissProc)))) + BaseYield
                    Case 454 ' Hedbergite
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.HedbergiteProc)))) + BaseYield
                    Case 455 ' Hemorphite
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.HemorphiteProc)))) + BaseYield
                    Case 456 ' Jaspet
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JaspetProc)))) + BaseYield
                    Case 457 ' Kernite
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.KerniteProc)))) + BaseYield
                    Case 468 ' Mercoxit
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MercoxitProc)))) + BaseYield
                    Case 469 ' Omber
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.OmberProc)))) + BaseYield
                    Case 458 ' Plagioclase
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.PlagioclaseProc)))) + BaseYield
                    Case 459 ' Pyroxeres
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.PyroxeresProc)))) + BaseYield
                    Case 460 ' Scordite
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ScorditeProc)))) + BaseYield
                    Case 461 ' Spodumain
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.SpodumainProc)))) + BaseYield
                    Case 462 ' Veldspar
                        tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.VeldsparProc)))) + BaseYield
                End Select
            Else
                tempNetYield = (NetYield - BaseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ScrapMetalProc)))) + BaseYield
            End If
            tempNetYield = Math.Min(tempNetYield, 1)
            matList = CType(itemList(asset), Collections.SortedList)
            newCLVItem = New ContainerListViewItem
            newCLVItem.Text = itemInfo.Name
            newCLVItem.Tag = itemInfo.ID
            clvRecycle.Items.Add(newCLVItem)
            price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(asset), 2)
            batches = CInt(Int(CLng(RecyclerAssetList(itemInfo.ID.ToString)) / itemInfo.PortionSize))
            quantity = CLng(RecyclerAssetList(asset))
            volume += itemInfo.Volume * quantity
            items += CLng(quantity)
            value = price * quantity
            fees = Math.Round(value * (RTotalFees / 100), 2)
            sale = value - fees
            newCLVItem.SubItems(1).Text = FormatNumber(itemInfo.MetaLevel, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(2).Text = FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(3).Text = FormatNumber(batches, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(4).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(5).Text = FormatNumber(value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            If chkFeesOnItems.Checked = True Then
                newCLVItem.SubItems(6).Text = FormatNumber(fees, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(7).Text = FormatNumber(sale, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Else
                newCLVItem.SubItems(7).Text = FormatNumber(value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            End If
            recycleTotal = 0
            If matList IsNot Nothing Then ' i.e. it can be refined
                For Each mat As String In matList.Keys
                    price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(mat)), 2)
                    perfect = CLng(matList(mat)) * batches
                    wastage = CLng(perfect * (1 - tempNetYield))
                    quant = CLng(perfect * tempNetYield)
                    taken = CLng(quant * (StationTake / 100))
                    quant = quant - taken
                    value = price * quant
                    fees = Math.Round(value * (RTotalFees / 100), 2)
                    sale = value - fees
                    newCLVSubItem = New ContainerListViewItem
                    newCLVSubItem.Text = mat
                    newCLVItem.Items.Add(newCLVSubItem)
                    newCLVSubItem.SubItems(2).Text = FormatNumber(quant, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(3).Text = FormatNumber(quant, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(4).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(5).Text = FormatNumber(value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    If chkFeesOnRefine.Checked = True Then
                        newCLVSubItem.SubItems(6).Text = FormatNumber(fees, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        newCLVSubItem.SubItems(8).Text = FormatNumber(sale, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        recycleTotal += sale
                    Else
                        newCLVSubItem.SubItems(8).Text = newCLVSubItem.SubItems(5).Text
                        recycleTotal += value
                    End If
                    ' Save the perfect refining quantity
                    If RecycleResults.Contains(mat) = False Then
                        RecycleResults.Add(mat, quant)
                    Else
                        RecycleResults(mat) = CDbl(RecycleResults(mat)) + quant
                    End If
                    ' Save the wasted amounts
                    If RecycleWaste.Contains(mat) = False Then
                        RecycleWaste.Add(mat, wastage)
                    Else
                        RecycleWaste(mat) = CDbl(RecycleWaste(mat)) + wastage
                    End If
                    ' Save the take amounts
                    If RecycleTake.Contains(mat) = False Then
                        RecycleTake.Add(mat, taken)
                    Else
                        RecycleTake(mat) = CDbl(RecycleTake(mat)) + taken
                    End If
                Next
            End If
            newCLVItem.SubItems(8).Text = FormatNumber(recycleTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            If CDbl(newCLVItem.SubItems(8).Text) > CDbl(newCLVItem.SubItems(7).Text) Then
                newCLVItem.BackColor = Drawing.Color.LightGreen
                newCLVItem.SubItems(9).Text = newCLVItem.SubItems(8).Text
            Else
                newCLVItem.SubItems(9).Text = newCLVItem.SubItems(7).Text
            End If
            benefit = CDbl(newCLVItem.SubItems(8).Text) - CDbl(newCLVItem.SubItems(7).Text)
            newCLVItem.SubItems(10).Tag = benefit
            newCLVItem.SubItems(10).Text = FormatNumber(benefit, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(11).Tag = (benefit / quantity)
            newCLVItem.SubItems(11).Text = FormatNumber(benefit / quantity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            salePriceTotal += CDbl(newCLVItem.SubItems(7).Text)
            refinePriceTotal += CDbl(newCLVItem.SubItems(8).Text)
            bestPriceTotal += CDbl(newCLVItem.SubItems(9).Text)
        Next
        lblPriceTotals.Text = "Sale / Refine / Best Totals: " & FormatNumber(salePriceTotal, 2) & " / " & FormatNumber(refinePriceTotal, 2) & " / " & FormatNumber(bestPriceTotal, 2)
        clvRecycle.Sort(0, SortOrder.Ascending, True)
        clvRecycle.EndUpdate()
        lblVolume.Text = FormatNumber(volume, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m³"
        lblItems.Text = FormatNumber(clvRecycle.Items.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblItems.Text &= " (" & FormatNumber(items, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")"
        ' Create the totals list
        clvTotals.BeginUpdate()
        clvTotals.Items.Clear()
        If RecycleResults IsNot Nothing Then
            For Each mat As String In RecycleResults.Keys
                price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(mat)), 2)
                wastage = CLng(RecycleWaste(mat))
                taken = CLng(RecycleTake(mat))
                quant = CLng(RecycleResults(mat))
                newCLVItem = New ContainerListViewItem
                newCLVItem.Text = mat
                clvTotals.Items.Add(newCLVItem)
                newCLVItem.SubItems(1).Text = FormatNumber(taken, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(2).Text = FormatNumber(wastage, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(3).Text = FormatNumber(quant, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(4).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(5).Text = FormatNumber(price * quant, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Next
        End If
        clvTotals.Sort(3, SortOrder.Descending, True)
        clvTotals.EndUpdate()
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRecyclePilots.SelectedIndexChanged
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
        If chkPerfectRefine.Checked = True Then
            NetYield = 1
        Else
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
        End If
        lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        If lblCorp.Tag IsNot Nothing Then
            StationStanding = GetStanding(rPilot.Name, lblCorp.Tag.ToString)
        Else
            StationStanding = 0
        End If
        ' Update Standings
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Else
            If lblCorp.Tag Is Nothing Then
                lblStandings.Text = FormatNumber(0, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Else
                lblStandings.Text = FormatNumber(StationStanding, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            End If
        End If
        ' Update Broker Fee
        If chkOverrideBrokerFee.Checked = False Then
            RBrokerFee = 1 * (1 - 0.05 * (CInt(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BrokerRelations))))
        Else
            RBrokerFee = nudBrokerFee.Value
        End If
        ' Update Trans Tax
        If chkOverrideTax.Checked = False Then
            RTransTax = 1 * (1 - 0.1 * (CInt(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Accounting))))
        Else
            RTransTax = nudTax.Value
        End If
        RTotalFees = RBrokerFee + RTransTax
        lblTotalFees.Text = FormatNumber(RTotalFees, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        Call Me.RecalcRecycling()
    End Sub

    Private Sub chkFeesOnRefine_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFeesOnRefine.CheckedChanged
        Call Me.RecalcRecycling()
    End Sub

    Private Sub chkFeesOnItems_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFeesOnItems.CheckedChanged
        Call Me.RecalcRecycling()
    End Sub

    Private Function GetStanding(ByVal pilotName As String, ByVal corpID As String) As Double
        ' Try and get the standings data
        Return EveHQ.Core.StandingsCacheDecoder.GetStanding(pilotName, corpID)
    End Function

#Region "Override Base Yield functions"
    Private Sub chkOverrideBaseYield_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideBaseYield.CheckedChanged
        If chkOverrideBaseYield.Checked = True Then
            BaseYield = CDbl(nudBaseYield.Value) / 100
        Else
            BaseYield = StationYield
        End If
        If cboRecyclePilots.SelectedItem IsNot Nothing Then
            Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
            lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
            lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            Call Me.RecalcRecycling()
        End If
    End Sub

    Private Sub nudBaseYield_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudBaseYield.ValueChanged
        If chkOverrideBaseYield.Checked = True Then
            BaseYield = CDbl(nudBaseYield.Value) / 100
            lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
            lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            Call Me.RecalcRecycling()
        End If
    End Sub
#End Region

#Region "Override Standings functions"
    Private Sub chkOverrideStandings_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideStandings.CheckedChanged
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Else
            If lblCorp.Tag Is Nothing Then
                lblStandings.Text = FormatNumber(0, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Else
                lblStandings.Text = FormatNumber(StationStanding, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            End If
        End If
        Call Me.RecalcRecycling()
    End Sub

    Private Sub lblStandings_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStandings.TextChanged
        StationTake = Math.Max(5 - (0.75 * CDbl(lblStandings.Text)), 0)
        lblStationTake.Text = FormatNumber(StationTake, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
    End Sub

    Private Sub nudStandings_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudStandings.ValueChanged
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Call Me.RecalcRecycling()
        End If
    End Sub

#End Region

#Region "Override Refining Skills functions"
    Private Sub chkPerfectRefine_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPerfectRefine.CheckedChanged
        If chkPerfectRefine.Checked = True Then
            NetYield = 1
        Else
            Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
        End If
        lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        Call Me.RecalcRecycling()
    End Sub
#End Region

#Region "Overrdie Fees functions"
    Private Sub chkOverrideBrokerFee_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideBrokerFee.CheckedChanged
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
        If chkOverrideBrokerFee.Checked = False Then
            RBrokerFee = 1 * (1 - 0.05 * (CInt(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BrokerRelations))))
        Else
            RBrokerFee = nudBrokerFee.Value
        End If
        RTotalFees = RBrokerFee + RTransTax
        lblTotalFees.Text = FormatNumber(RTotalFees, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        Call Me.RecalcRecycling()
    End Sub
    Private Sub chkOverrideTax_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideTax.CheckedChanged
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
        If chkOverrideTax.Checked = False Then
            RTransTax = 1 * (1 - 0.1 * (CInt(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Accounting))))
        Else
            RTransTax = nudTax.Value
        End If
        RTotalFees = RBrokerFee + RTransTax
        lblTotalFees.Text = FormatNumber(RTotalFees, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        Call Me.RecalcRecycling()
    End Sub
    Private Sub nudBrokerFee_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudBrokerFee.ValueChanged
        If chkOverrideBrokerFee.Checked = True Then
            RBrokerFee = nudBrokerFee.Value
            RTotalFees = RBrokerFee + RTransTax
            lblTotalFees.Text = FormatNumber(RTotalFees, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            Call Me.RecalcRecycling()
        End If
    End Sub
    Private Sub nudTax_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTax.ValueChanged
        If chkOverrideTax.Checked = True Then
            RTransTax = nudTax.Value
            RTotalFees = RBrokerFee + RTransTax
            lblTotalFees.Text = FormatNumber(RTotalFees, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            Call Me.RecalcRecycling()
        End If
    End Sub
#End Region

#Region "Refining Mode functions"
    Private Sub cboRefineMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRefineMode.SelectedIndexChanged
        Select Case cboRefineMode.SelectedIndex
            Case 0 ' Standard
                If chkOverrideBaseYield.Checked = True Then
                    BaseYield = CDbl(nudBaseYield.Value) / 100
                Else
                    BaseYield = StationYield
                End If
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                If chkPerfectRefine.Checked = True Then
                    NetYield = 1
                Else
                    Dim rPilot As New EveHQ.Core.Pilot
                    If cboRecyclePilots.SelectedItem IsNot Nothing Then
                        rPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
                        NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
                    Else
                        If cboRecyclePilots.Items.Count > 0 Then
                            cboRecyclePilots.SelectedIndex = 0
                            rPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
                            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
                        Else
                            NetYield = 0
                        End If
                    End If
                End If
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                If chkOverrideStandings.Checked = True Then
                    lblStandings.Text = FormatNumber(nudStandings.Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                Else
                    If lblCorp.Tag Is Nothing Then
                        lblStandings.Text = FormatNumber(0, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    Else
                        lblStandings.Text = FormatNumber(StationStanding, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    End If
                End If
                chkOverrideBaseYield.Enabled = True
                chkOverrideStandings.Enabled = True
                chkPerfectRefine.Enabled = True
                nudBaseYield.Enabled = True
                nudStandings.Enabled = True
                cboRecyclePilots.Enabled = True
            Case 1 ' Refining Array
                BaseYield = 0.35
                NetYield = 0.35
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lblStandings.Text = FormatNumber(10, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                chkOverrideBaseYield.Enabled = False
                chkOverrideStandings.Enabled = False
                chkPerfectRefine.Enabled = False
                nudBaseYield.Enabled = False
                nudStandings.Enabled = False
                cboRecyclePilots.Enabled = False
            Case 2 ' Intensive Refining Array
                BaseYield = 0.75
                NetYield = 0.75
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lblStandings.Text = FormatNumber(10, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                chkOverrideBaseYield.Enabled = False
                chkOverrideStandings.Enabled = False
                chkPerfectRefine.Enabled = False
                nudBaseYield.Enabled = False
                nudStandings.Enabled = False
                cboRecyclePilots.Enabled = False
        End Select
        ' Set the base yield if no station
        If lblStation.Text = "n/a" Then
            chkOverrideBaseYield.Checked = True
        Else
            Call Me.RecalcRecycling()
        End If
    End Sub
#End Region

    Private Sub mnuAlterRecycleQuantity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAlterRecycleQuantity.Click
        Dim newQ As New frmSelectQuantity
        newQ.Quantity = CLng(RecyclerAssetList(clvRecycle.SelectedItems(0).Tag.ToString))
        newQ.ShowDialog()
        RecyclerAssetList(clvRecycle.SelectedItems(0).Tag.ToString) = newQ.Quantity
        newQ.Dispose()
        Call Me.RecalcRecycling()
    End Sub

    Private Sub mnuRemoveRecycleItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveRecycleItem.Click
        RecyclerAssetList.Remove(clvRecycle.SelectedItems(0).Tag.ToString)
        Call Me.RecalcRecycling()
    End Sub

    Private Sub mnuAddRecycleItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddRecycleItem.Click
        Dim newI As New frmSelectItem
        newI.ShowDialog()
        Dim itemName As String = newI.Item
        If itemName IsNot Nothing Then
            Dim itemID As String = EveHQ.Core.HQ.itemList(itemName)
            If RecyclerAssetList.ContainsKey(itemID) = False Then
                RecyclerAssetList.Add(itemID, 1)
            End If
            newI.Dispose()
            Call Me.LoadRecyclingInfo()
        End If
    End Sub

#End Region

#Region "CSV Export Routines"

    Private Sub btnExportTransactions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportTransactions.Click
        Call Me.GenerateCSVFileFromCLV("Wallet Transactions", clvTransactions)
    End Sub

    Private Sub btnExportJournal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportJournal.Click
        Call Me.GenerateCSVFileFromCLV("Wallet Journal", clvJournal)
    End Sub

    Private Sub btnExportJobs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportJobs.Click
        Call Me.GenerateCSVFileFromCLV("Industry Jobs", clvJobs)
    End Sub

    Private Sub btnExportOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportOrders.Click
        Call Me.GenerateCSVFileFromCLV("Sell Orders", clvSellOrders)
        Call Me.GenerateCSVFileFromCLV("Buy Orders", clvBuyOrders)
    End Sub

    Private Sub btnExportRigList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportRigList.Click
        Call Me.GenerateCSVFileFromCLV("Rig List", lvwRigs)
    End Sub

    Private Sub btnExportRigBuildList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportRigBuildList.Click
        Call Me.GenerateCSVFileFromCLV("Rig Build List", lvwRigBuildList)
    End Sub

    Private Sub GenerateCSVFileFromCLV(ByVal Description As String, ByVal cListView As ContainerListView)
        If cboOwner.SelectedItem IsNot Nothing Then
            Try
                Dim csvFile As String = Path.Combine(EveHQ.Core.HQ.reportFolder, Description.Replace(" ", "") & " - " & cboOwner.SelectedItem.ToString & " (" & Format(Now, "yyyy-MM-dd HH-mm-ss") & ").csv")
                Dim csvText As New StringBuilder
                With cListView
                    ' Write the columns
                    For col As Integer = 0 To .Columns.Count - 1
                        csvText.Append(.Columns(col).Text)
                        If col <> .Columns.Count - 1 Then
                            csvText.Append(EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                        End If
                    Next
                    csvText.AppendLine("")
                    ' Write the data
                    For Each row As ContainerListViewItem In .Items
                        For col As Integer = 0 To .Columns.Count - 1
                            If IsNumeric(row.SubItems(col).Text) = True Then
                                csvText.Append(CDbl(row.SubItems(col).Text).ToString)
                            Else
                                csvText.Append("""" & row.SubItems(col).Text & """")
                            End If
                            If col <> .Columns.Count - 1 Then
                                csvText.Append(EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                            End If
                        Next
                        csvText.AppendLine("")
                    Next
                End With
                Dim sw As New StreamWriter(csvFile)
                sw.Write(csvText.ToString)
                sw.Flush()
                sw.Close()
                MessageBox.Show(Description & " successfully exported to " & csvFile, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("There was an error writing the " & Description & " File. The error was: " & ControlChars.CrLf & ControlChars.CrLf & ex.Message, "Error Writing File", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

#Region "BPManager Routines"

    Private Sub btnBPCalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBPCalc.Click
        Call Me.OpenBPCalculator()
    End Sub

    Private Sub chkShowOwnedBPs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowOwnedBPs.CheckedChanged
        Call Me.UpdateBPList()
    End Sub

    Private Sub UpdateBPList()
        ' Check if we are showing the full list or the owners list
        If chkShowOwnedBPs.Checked = False Then
            Dim search As String = txtBPSearch.Text
            ' Show the full BP list
            clvBlueprints.BeginUpdate()
            clvBlueprints.Items.Clear()
            Dim matchCat As Boolean = False
            For Each BP As Blueprint In PlugInData.Blueprints.Values
                If cboTechFilter.SelectedIndex = 0 Or (cboTechFilter.SelectedIndex = BP.TechLevel) Then
                    matchCat = False
                    If cboCategoryFilter.SelectedIndex = 0 Then
                        matchCat = True
                    Else
                        If PlugInData.CategoryNames.ContainsKey(cboCategoryFilter.SelectedItem.ToString) Then
                            If PlugInData.CategoryNames(cboCategoryFilter.SelectedItem.ToString) = CStr(EveHQ.Core.HQ.itemData(BP.ProductID.ToString).Category) Then
                                matchCat = True
                            End If
                        End If
                    End If
                    If matchCat = True Then
                        If search = "" Or BP.Name.ToLower.Contains(search.ToLower) Then
                            Dim newBPItem As New ContainerListViewItem
                            newBPItem.Text = BP.Name
                            clvBlueprints.Items.Add(newBPItem)
                            newBPItem.SubItems(1).Text = "n/a"
                            newBPItem.SubItems(2).Text = "n/a"
                            newBPItem.SubItems(3).Text = BP.TechLevel.ToString
                            newBPItem.SubItems(4).Text = "0"
                            newBPItem.SubItems(5).Text = "0"
                            newBPItem.SubItems(6).Text = "Infinite"
                            newBPItem.SubItems(7).Text = "n/a"
                        End If
                    End If
                End If
            Next
            clvBlueprints.Sort(0, False, False)
            clvBlueprints.EndUpdate()
        Else
            ' Show the owned BP list
            Call Me.UpdateOwnerBPList()
        End If
    End Sub

    Private Sub UpdateOwnerBPList()
        Dim search As String = txtBPSearch.Text
        ' Establish the owner
        If cboOwner.SelectedItem IsNot Nothing Then
            Dim owner As String = cboOwner.SelectedItem.ToString()

            clvBlueprints.BeginUpdate()
            clvBlueprints.Items.Clear()
            If owner <> "" Then
                ' Fetch the ownerBPs if it exists
                Dim ownerBPs As New SortedList(Of String, BlueprintAsset)
                If PlugInData.BlueprintAssets.ContainsKey(owner) = True Then
                    ownerBPs = PlugInData.BlueprintAssets(owner)
                End If
                Dim BPData As New Blueprint
                Dim LocationName As String = ""
                Dim matchCat As Boolean = False
                For Each BP As BlueprintAsset In ownerBPs.Values
                    If BP.LocationDetails Is Nothing Then BP.LocationDetails = "" ' Resets details
                    If BP.LocationID Is Nothing Then BP.LocationID = "0" ' Resets details
                    BPData = PlugInData.Blueprints(BP.TypeID)
                    LocationName = Me.GetLocationNameFromID(BP.LocationID)
                    If cboTechFilter.SelectedIndex = 0 Or (cboTechFilter.SelectedIndex = BPData.TechLevel) Then
                        If cboTypeFilter.SelectedIndex = 0 Or (cboTypeFilter.SelectedIndex = BP.BPType + 1) Then
                            matchCat = False
                            If cboCategoryFilter.SelectedIndex = 0 Then
                                matchCat = True
                            Else
                                If PlugInData.CategoryNames.ContainsKey(cboCategoryFilter.SelectedItem.ToString) Then
                                    If PlugInData.CategoryNames(cboCategoryFilter.SelectedItem.ToString) = CStr(EveHQ.Core.HQ.itemData(BPData.ProductID.ToString).Category) Then
                                        matchCat = True
                                    End If
                                End If
                            End If
                            If matchCat = True Then
                                If search = "" Or BPData.Name.ToLower.Contains(search.ToLower) Or BP.LocationDetails.ToLower.Contains(search.ToLower) Or LocationName.ToLower.Contains(search.ToLower) Then
                                    Dim newBPItem As New ContainerListViewItem
                                    newBPItem.Text = BPData.Name
                                    newBPItem.Tag = BP.AssetID
                                    clvBlueprints.Items.Add(newBPItem)
                                    newBPItem.SubItems(3).Text = BPData.TechLevel.ToString
                                    Call UpdateOwnerBPItem(owner, LocationName, BP, newBPItem)
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            clvBlueprints.Sort(0, False, False)
            clvBlueprints.EndUpdate()
        End If
    End Sub
    Private Sub UpdateOwnerBPItem(ByVal Owner As String, ByVal LocationName As String, ByVal BP As BlueprintAsset, ByVal newBPItem As ContainerListViewItem)
        newBPItem.SubItems(4).Text = FormatNumber(BP.MELevel, 0)
        newBPItem.SubItems(5).Text = FormatNumber(BP.PELevel, 0)
        Select Case BP.BPType
            Case BPType.Unknown  ' Undetermined
                newBPItem.SubItems(1).Text = LocationName
                newBPItem.SubItems(2).Text = BP.LocationDetails
                newBPItem.SubItems(6).Text = "Unknown"
                newBPItem.SubItems(6).Tag = BP.Runs
                newBPItem.BackColor = Drawing.Color.LightGray
            Case BPType.BPO  ' BPO
                newBPItem.SubItems(1).Text = LocationName
                newBPItem.SubItems(2).Text = BP.LocationDetails
                newBPItem.SubItems(6).Text = "BPO"
                newBPItem.SubItems(6).Tag = 1000000
                newBPItem.BackColor = Drawing.Color.LightGreen
            Case BPType.BPC  ' BPC
                newBPItem.SubItems(1).Text = LocationName
                newBPItem.SubItems(2).Text = BP.LocationDetails
                newBPItem.SubItems(6).Text = FormatNumber(BP.Runs, 0)
                newBPItem.SubItems(6).Tag = BP.Runs
                newBPItem.BackColor = Drawing.Color.LightSteelBlue
            Case BPType.User
                newBPItem.SubItems(1).Text = Owner & "'s Secret BP Stash"
                newBPItem.SubItems(2).Text = Owner & "'s Secret BP Stash"
                newBPItem.SubItems(6).Text = "BPO"
                newBPItem.SubItems(6).Tag = 1000000
                newBPItem.BackColor = Drawing.Color.Yellow
        End Select
        newBPItem.SubItems(7).Text = [Enum].GetName(GetType(BPStatus), BP.Status)
        newBPItem.SubItems(7).Tag = BP.Status
        Select Case BP.Status
            Case BPStatus.Missing
                newBPItem.BackColor = Drawing.Color.LightCoral
            Case BPStatus.Exhausted
                newBPItem.BackColor = Drawing.Color.Orange
        End Select
    End Sub

    Private Sub btnUpdateBPsFromAssets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateBPsFromAssets.Click
        ' Establish the owner
        Dim IsCorp As Boolean = False
        ' Get the owner we will use
        If cboOwner.SelectedItem IsNot Nothing Then
            Dim owner As String = cboOwner.SelectedItem.ToString()

            ' Fetch the ownerBPs if it exists
            Dim ownerBPs As New SortedList(Of String, BlueprintAsset)
            If PlugInData.BlueprintAssets.ContainsKey(owner) = True Then
                ownerBPs = PlugInData.BlueprintAssets(owner)
            Else
                PlugInData.BlueprintAssets.Add(owner, ownerBPs)
            End If

            ' See if this owner is a corp
            If PlugInData.CorpList.ContainsKey(owner) = True Then
                IsCorp = True
                ' See if we have a representative
                Dim CorpRep As SortedList = CType(PlugInData.CorpReps(0), Collections.SortedList)
                If CorpRep IsNot Nothing Then
                    If CorpRep.ContainsKey(CStr(PlugInData.CorpList(owner))) = True Then
                        owner = CStr(CorpRep(CStr(PlugInData.CorpList(owner))))
                    Else
                        owner = ""
                    End If
                Else
                    owner = ""
                End If
            End If

            If owner <> "" Then
                ' Parse the Assets XML
                Dim assetXML As New XmlDocument
                Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(owner), Core.Pilot)
                Dim accountName As String = selPilot.Account
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                If IsCorp = True Then
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                Else
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                End If
                If assetXML IsNot Nothing Then
                    Dim Assets As New SortedList(Of String, BlueprintAsset)
                    Dim locList As XmlNodeList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                    If locList.Count > 0 Then
                        ' Define what we want to obtain
                        Dim categories, groups, types As New ArrayList
                        categories.Add(9) ' Blueprints
                        For Each loc As XmlNode In locList
                            Dim locationID As String = loc.Attributes.GetNamedItem("locationID").Value
                            Dim flagID As Integer = CInt(loc.Attributes.GetNamedItem("flag").Value)
                            Dim locationDetails As String = PlugInData.itemFlags(flagID).ToString
                            ' Check the asset
                            Dim ItemData As New EveHQ.Core.EveItem
                            Dim AssetID As String = ""
                            Dim itemID As String = ""
                            AssetID = loc.Attributes.GetNamedItem("itemID").Value
                            itemID = loc.Attributes.GetNamedItem("typeID").Value
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                                ItemData = EveHQ.Core.HQ.itemData(itemID)
                                If flagID = 0 Then
                                    If PlugInData.AssetItemNames.ContainsKey(AssetID) = True Then
                                        locationDetails = PlugInData.AssetItemNames(AssetID)
                                    Else
                                        locationDetails = ItemData.Name
                                    End If
                                End If
                                If categories.Contains(ItemData.Category) Or groups.Contains(ItemData.Group) Or types.Contains(ItemData.ID) Then
                                    Dim newBP As New BlueprintAsset
                                    newBP.AssetID = AssetID
                                    newBP.LocationID = locationID
                                    If IsCorp = True Then
                                        Dim accountID As Integer = flagID + 885
                                        If accountID = 889 Then accountID = 1000
                                        If divisions.ContainsKey(selPilot.CorpID & "_" & accountID.ToString) = True Then
                                            locationDetails = CStr(divisions.Item(selPilot.CorpID & "_" & accountID.ToString))
                                        End If
                                    End If
                                    newBP.LocationDetails = locationDetails
                                    newBP.TypeID = itemID
                                    newBP.Status = BPStatus.Present
                                    newBP.MELevel = 0
                                    newBP.PELevel = 0
                                    newBP.Runs = -1
                                    newBP.Notes = ""
                                    Assets.Add(AssetID, newBP)
                                End If
                            End If

                            ' Get the location name
                            If loc.ChildNodes.Count > 0 Then
                                Call GetAssetFromNode(loc, categories, groups, types, Assets, locationID, locationDetails, selPilot, IsCorp)
                            End If
                        Next
                    End If
                    If Assets.Count > 0 Then
                        ' Mark all of our existing blueprints as missing
                        For Each ownerBP As BlueprintAsset In ownerBPs.Values
                            If ownerBP.BPType <> BPType.User Then
                                ownerBP.Status = BPStatus.Missing
                            Else
                                ownerBP.Status = BPStatus.Present
                            End If
                        Next
                        ' Should have our list of assets now so let's compare them
                        Dim item As New EveHQ.Core.EveItem
                        For Each assetID As String In Assets.Keys
                            ' See if the assetID already exists for the owner
                            If ownerBPs.ContainsKey(assetID) = True Then
                                ' We have it so set the status to present
                                ownerBPs(assetID).Status = BPStatus.Present
                                ' Update the location
                                ownerBPs(assetID).LocationID = Assets(assetID).LocationID
                                ownerBPs(assetID).LocationDetails = Assets(assetID).LocationDetails
                            Else
                                ' Not present in the existing list so let's add it in
                                ownerBPs.Add(assetID, Assets(assetID))
                            End If
                        Next
                    End If
                End If
                ' Update the owner list if the option requires it
                If chkShowOwnedBPs.Checked = True Then
                    Call Me.UpdateOwnerBPList()
                End If
            End If
        Else
            MessageBox.Show("Make sure you have entered your API details and selected the correct owner before proceeding.", "Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Function GetLocationNameFromID(ByVal locID As String) As String
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
                Return newLocation.stationName
            Else
                ' Unknown outpost!
                newLocation = New Prism.Station
                newLocation.stationID = CLng(locID)
                newLocation.stationName = "Unknown Outpost"
                newLocation.systemID = 0
                newLocation.constID = 0
                newLocation.regionID = 0
                Return newLocation.stationName
            End If
        Else
            If CDbl(locID) < 60000000 Then
                If PlugInData.stations.Contains(locID) Then
                    Dim newSystem As SolarSystem = CType(PlugInData.stations(locID), SolarSystem)
                    Return newSystem.Name
                Else
                    Return "Unknown Location"
                End If
            Else
                newLocation = CType(PlugInData.stations(locID), Prism.Station)
                If newLocation IsNot Nothing Then
                    Return newLocation.stationName
                Else
                    ' Unknown system/station!
                    newLocation = New Prism.Station
                    newLocation.stationID = CLng(locID)
                    newLocation.stationName = "Unknown Location"
                    newLocation.systemID = 0
                    newLocation.constID = 0
                    newLocation.regionID = 0
                    Return newLocation.stationName
                End If
            End If
        End If
    End Function
    Private Sub GetAssetFromNode(ByVal loc As XmlNode, ByVal categories As ArrayList, ByVal groups As ArrayList, ByVal types As ArrayList, ByRef Assets As SortedList(Of String, BlueprintAsset), ByVal locationID As String, ByVal locationDetails As String, ByVal selPilot As EveHQ.Core.Pilot, ByVal IsCorp As Boolean)
        Dim itemList As XmlNodeList = loc.ChildNodes(0).ChildNodes
        Dim ItemData As New EveHQ.Core.EveItem
        Dim AssetID As String = ""
        Dim itemID As String = ""
        Dim flagID As Integer = 0
        Dim flagName As String = ""
        Dim containerID As String = loc.Attributes.GetNamedItem("itemID").Value
        Dim containerType As String = loc.Attributes.GetNamedItem("typeID").Value
        For Each item As XmlNode In itemList
            AssetID = item.Attributes.GetNamedItem("itemID").Value
            itemID = item.Attributes.GetNamedItem("typeID").Value
            flagID = CInt(item.Attributes.GetNamedItem("flag").Value)
            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                ItemData = EveHQ.Core.HQ.itemData(itemID)
                If PlugInData.AssetItemNames.ContainsKey(containerID) = True Then
                    flagName = locationDetails & "/" & PlugInData.AssetItemNames(containerID)
                Else
                    flagName = locationDetails & "/" & EveHQ.Core.HQ.itemData(containerType).Name
                End If
                If categories.Contains(ItemData.Category) Or groups.Contains(ItemData.Group) Or types.Contains(ItemData.ID) Then
                    Dim newBP As New BlueprintAsset
                    newBP.AssetID = AssetID
                    newBP.LocationID = locationID
                    If IsCorp = True And EveHQ.Core.HQ.itemData(itemID).Group <> 16 Then
                        Dim accountID As Integer = flagID + 885
                        If accountID = 889 Then accountID = 1000
                        If divisions.ContainsKey(selPilot.CorpID & "_" & accountID.ToString) = True Then
                            flagName = locationDetails & "/" & CStr(divisions.Item(selPilot.CorpID & "_" & accountID.ToString))
                        End If
                    End If
                    newBP.LocationDetails = flagName
                    newBP.TypeID = itemID
                    newBP.Status = BPStatus.Present
                    newBP.MELevel = 0
                    newBP.PELevel = 0
                    newBP.Runs = -1
                    newBP.Notes = ""
                    Assets.Add(AssetID, newBP)
                End If
            End If
            ' Check child items if they exist
            If item.ChildNodes.Count > 0 Then
                Call GetAssetFromNode(item, categories, groups, types, Assets, locationID, flagName, selPilot, IsCorp)
            End If
        Next
    End Sub

    Private Sub btnGetBPJobInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetBPJobInfo.Click
        ' Get the owner BPs
        Dim ownerBPs As New SortedList(Of String, BlueprintAsset)
        If cboOwner.SelectedItem IsNot Nothing Then
            Dim owner As String = cboOwner.SelectedItem.ToString()
            ' Fetch the ownerBPs if it exists
            If PlugInData.BlueprintAssets.ContainsKey(owner) = True Then
                ownerBPs = PlugInData.BlueprintAssets(owner)
            End If
        Else
            MessageBox.Show("Make sure you have entered your API details and selected the correct owner before proceeding.", "Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' We are going to scan the whole of the Jobs API to try and find relevant IDs - no sense dicking around here, we need info!!
        If ownerBPs IsNot Nothing Then
            Dim cacheFolder As String = EveHQ.Core.HQ.cacheFolder
            For Each cacheFile As String In My.Computer.FileSystem.GetFiles(cacheFolder, FileIO.SearchOption.SearchTopLevelOnly, "EVEHQAPI_Industry*")
                ' Load up the XML
                Dim jobXML As New XmlDocument
                jobXML.Load(cacheFile)
                ' Get the Node List
                Dim jobs As XmlNodeList = jobXML.SelectNodes("/eveapi/result/rowset/row")
                For Each job As XmlNode In jobs
                    Dim assetID As String = job.Attributes.GetNamedItem("installedItemID").Value
                    If ownerBPs.ContainsKey(assetID) = True Then
                        ' Fetch the current BP Data
                        Dim cBPInfo As BlueprintAsset = ownerBPs(assetID)
                        Select Case CInt(job.Attributes.GetNamedItem("activityID").Value)
                            Case 1 ' Manufacturing
                                Dim Runs As Integer = CInt(job.Attributes.GetNamedItem("runs").Value)
                                ' Check if the MELevel is greater than what we have
                                If CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value) > cBPInfo.MELevel Then
                                    cBPInfo.MELevel = CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value)
                                End If
                                ' Check if the PELevel is greater than what we have
                                If CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value) > cBPInfo.PELevel Then
                                    cBPInfo.PELevel = CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value)
                                End If
                                ' Check if the Runs remaining are less than what we have
                                Dim initialRuns As Integer = CInt(job.Attributes.GetNamedItem("installedItemLicensedProductionRunsRemaining").Value)
                                If initialRuns <> -1 Then
                                    cBPInfo.BPType = BPType.BPC
                                    If initialRuns - Runs < cBPInfo.Runs Or cBPInfo.Runs = -1 Then
                                        cBPInfo.Runs = initialRuns - Runs
                                    End If
                                    If cBPInfo.Runs = 0 Then
                                        cBPInfo.Status = BPStatus.Exhausted
                                    End If
                                Else
                                    cBPInfo.BPType = BPType.BPO
                                End If
                            Case 3 ' PE Research
                                Dim Runs As Integer = CInt(job.Attributes.GetNamedItem("runs").Value)
                                ' Check if the MELevel is greater than what we have
                                If CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value) > cBPInfo.MELevel Then
                                    cBPInfo.MELevel = CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value)
                                End If
                                ' Check if the PELevel is greater than what we have
                                If CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value) + Runs > cBPInfo.PELevel Then
                                    cBPInfo.PELevel = CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value) + Runs
                                End If
                                ' Check if the Runs remaining are less than what we have
                                Dim initialRuns As Integer = CInt(job.Attributes.GetNamedItem("installedItemLicensedProductionRunsRemaining").Value)
                                If initialRuns <> -1 Then
                                    cBPInfo.BPType = BPType.BPC
                                    If initialRuns < cBPInfo.Runs Or cBPInfo.Runs = -1 Then
                                        cBPInfo.Runs = initialRuns
                                    End If
                                    If cBPInfo.Runs = 0 Then
                                        cBPInfo.Status = BPStatus.Exhausted
                                    End If
                                Else
                                    cBPInfo.BPType = BPType.BPO
                                End If
                            Case 4 ' ME Research
                                Dim Runs As Integer = CInt(job.Attributes.GetNamedItem("runs").Value)
                                ' Check if the MELevel is greater than what we have
                                If CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value) + Runs > cBPInfo.MELevel Then
                                    cBPInfo.MELevel = CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value) + Runs
                                End If
                                ' Check if the PELevel is greater than what we have
                                If CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value) > cBPInfo.PELevel Then
                                    cBPInfo.PELevel = CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value)
                                End If
                                ' Check if the Runs remaining are less than what we have
                                Dim initialRuns As Integer = CInt(job.Attributes.GetNamedItem("installedItemLicensedProductionRunsRemaining").Value)
                                If initialRuns <> -1 Then
                                    cBPInfo.BPType = BPType.BPC
                                    If initialRuns < cBPInfo.Runs Or cBPInfo.Runs = -1 Then
                                        cBPInfo.Runs = initialRuns
                                    End If
                                    If cBPInfo.Runs = 0 Then
                                        cBPInfo.Status = BPStatus.Exhausted
                                    End If
                                Else
                                    cBPInfo.BPType = BPType.BPO
                                End If
                            Case 5 ' Copying
                                Dim Runs As Integer = CInt(job.Attributes.GetNamedItem("runs").Value)
                                ' Check if the MELevel is greater than what we have
                                If CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value) > cBPInfo.MELevel Then
                                    cBPInfo.MELevel = CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value)
                                End If
                                ' Check if the PELevel is greater than what we have
                                If CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value) > cBPInfo.PELevel Then
                                    cBPInfo.PELevel = CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value)
                                End If
                                ' Check if the Runs remaining are less than what we have
                                Dim initialRuns As Integer = CInt(job.Attributes.GetNamedItem("installedItemLicensedProductionRunsRemaining").Value)
                                If initialRuns <> -1 Then
                                    cBPInfo.BPType = BPType.BPC
                                    If initialRuns < cBPInfo.Runs Or cBPInfo.Runs = -1 Then
                                        cBPInfo.Runs = initialRuns
                                    End If
                                    If cBPInfo.Runs = 0 Then
                                        cBPInfo.Status = BPStatus.Exhausted
                                    End If
                                Else
                                    cBPInfo.BPType = BPType.BPO
                                End If
                        End Select
                    End If
                Next
            Next
        Else
            MessageBox.Show("Make sure you have retrieved your Blueprint details before attempting to update them.", "Blueprints Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Update the owner list if the option requires it
        If chkShowOwnedBPs.Checked = True Then
            Call Me.UpdateOwnerBPList()
        End If
    End Sub

    Private Sub ctxBPManager_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxBPManager.Opening
        If clvBlueprints.SelectedItems.Count = 1 Then
            mnuSendToBPCalc.Enabled = True
            ' Get the blueprint info
            If chkShowOwnedBPs.Checked = True Then
                Dim assetID As String = CStr(clvBlueprints.SelectedItems(0).Tag)
                Dim BPOwner As String = cboOwner.SelectedItem.ToString
                Dim asset As BlueprintAsset = PlugInData.BlueprintAssets(BPOwner).Item(assetID)
                If asset.AssetID = asset.TypeID Then
                    ' Custom BP
                    mnuRemoveCustomBP.Text = "Remove Custom Blueprint"
                    mnuRemoveCustomBP.Enabled = True
                Else
                    ' Standard BP
                    mnuRemoveCustomBP.Text = "Remove Blueprint"
                    mnuRemoveCustomBP.Enabled = True
                End If
            Else
                mnuRemoveCustomBP.Text = "Remove Blueprint"
                mnuRemoveCustomBP.Enabled = False
            End If
        Else
            mnuSendToBPCalc.Enabled = False
            mnuRemoveCustomBP.Text = "Remove Blueprints (" & clvBlueprints.SelectedItems.Count.ToString & ")"
            mnuRemoveCustomBP.Enabled = True
        End If
        mnuAmendBPDetails.Enabled = chkShowOwnedBPs.Checked
    End Sub

    Private Sub mnuSendToBPCalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSendToBPCalc.Click
        Call Me.OpenBPCalculator()
    End Sub

    Private Sub OpenBPCalculator()
        Dim BPName As String = ""
        Dim BPID As String = ""
        If clvBlueprints.SelectedItems.Count = 1 Then
            BPName = clvBlueprints.SelectedItems(0).Text
            If clvBlueprints.SelectedItems(0).Tag IsNot Nothing Then
                BPID = clvBlueprints.SelectedItems(0).Tag.ToString
            End If
        End If
        Dim BPCalc As New frmBPCalculator
        ' Check for a selected BP on the BPManager form
        If BPName <> "" Then
            BPCalc.BPName = BPName
        End If
        If cboOwner.SelectedItem IsNot Nothing Then
            BPCalc.BPOwnerName = cboOwner.SelectedItem.ToString
        End If
        BPCalc.UsingOwnedBPs = chkShowOwnedBPs.Checked
        If chkShowOwnedBPs.Checked = True Then
            BPCalc.OwnedBPID = BPID
        End If
        BPCalc.Show()
    End Sub

    Private Sub mnuAmendBPDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAmendBPDetails.Click
        Call Me.EditBlueprintDetails()
    End Sub

    Private Sub EditBlueprintDetails()
        Dim BPForm As New frmEditBPDetails
        BPForm.OwnerName = cboOwner.SelectedItem.ToString
        Dim BPs As New ArrayList
        For Each selItem As ContainerListViewItem In clvBlueprints.SelectedItems
            BPs.Add(selItem.Tag.ToString)
        Next
        BPForm.AssetIDs = BPs
        BPForm.ShowDialog()
        ' Update the list using the details
        Dim BP As New BlueprintAsset
        Dim locationName As String = ""
        For Each selitem As ContainerListViewItem In clvBlueprints.SelectedItems
            BP = PlugInData.BlueprintAssets(BPForm.OwnerName).Item(selitem.Tag.ToString)
            LocationName = Me.GetLocationNameFromID(BP.LocationID)
            Call Me.UpdateOwnerBPItem(BPForm.OwnerName, locationName, BP, selitem)
        Next
    End Sub

    Private Sub txtBPSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBPSearch.TextChanged
        Call Me.UpdateBPList()
    End Sub

    Private Sub btnResetBPSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetBPSearch.Click
        txtBPSearch.Text = ""
    End Sub

    Private Sub btnAddCustomBP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCustomBP.Click
        Dim BPForm As New frmAddCustomBP
        BPForm.BPOwner = cboOwner.SelectedItem.ToString
        BPForm.ShowDialog()
        If BPForm.DialogResult = Windows.Forms.DialogResult.OK Then
            Call Me.UpdateBPList()
        End If
    End Sub

    Private Sub mnuRemoveCustomBP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveCustomBP.Click
        ' Remove the custom BP from the assets
        If clvBlueprints.SelectedItems.Count > 0 Then
            Dim rBP As New ContainerListViewItem
            Dim cIDX As Integer = clvBlueprints.SelectedItems.Count - 1
            Do
                rBP = clvBlueprints.SelectedItems(cIDX)
                Dim assetID As String = CStr(rBP.Tag)
                Dim BPOwner As String = cboOwner.SelectedItem.ToString
                If PlugInData.BlueprintAssets(BPOwner).ContainsKey(assetID) = True Then
                    PlugInData.BlueprintAssets(BPOwner).Remove(assetID)
                    clvBlueprints.Items.Remove(rBP)
                    cIDX -= 1
                End If
            Loop Until cIDX = -1
            'Call Me.UpdateBPList()
        End If
    End Sub

    Private Sub cboTechFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTechFilter.SelectedIndexChanged
        If startup = False Then
            Call UpdateBPList()
        End If
    End Sub

    Private Sub cboTypeFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTypeFilter.SelectedIndexChanged
        If startup = False Then
            Call UpdateBPList()
        End If
    End Sub

    Private Sub cboCategoryFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCategoryFilter.SelectedIndexChanged
        If startup = False Then
            Call UpdateBPList()
        End If
    End Sub

    Private Sub clvBlueprints_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles clvBlueprints.DoubleClick
        Call Me.OpenBPCalculator()
    End Sub

#End Region

#Region "Cache Clearing Routines"

    Private Sub ctxAPIStatus_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxAPIStatus.Opening
        If lvwCurrentAPIs.SelectedItems.Count > 0 Then
            Dim OwnerLVI As ListViewItem = lvwCurrentAPIs.SelectedItems(0)
            Dim RepData As String = OwnerLVI.ToolTipText.ToString
            Dim RepList() As String = RepData.Split(CChar(ControlChars.CrLf))
            Dim Reps As New ArrayList
            For Each RepInfo As String In RepList
                If RepInfo.Trim <> "" Then
                    Dim Rep As String = RepInfo.Substring(RepInfo.IndexOf(":") + 2).Trim
                    If Reps.Contains(Rep) = False Then
                        Reps.Add(Rep)
                    End If
                End If
            Next
            Reps.Sort()
            mnuClearXMLCache.DropDownItems.Clear()
            For Each Rep As String In Reps
                Dim newOwner As New ToolStripMenuItem
                newOwner.Text = Rep
                newOwner.Name = Rep
                newOwner.Tag = OwnerLVI.SubItems(1).Text
                AddHandler newOwner.Click, AddressOf Me.ClearPrismXMLCache
                mnuClearXMLCache.DropDownItems.Add(newOwner)
            Next
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub ClearPrismXMLCache(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selOwner As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sPilot As String = selOwner.Name
        Dim sType As String = selOwner.Tag.ToString
        MessageBox.Show("Removing " & sType & " data for " & sPilot)
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(sPilot), Core.Pilot)
        Dim prefix As String = "EVEHQAPI_"
        Dim suffix As String = "_" & rPilot.Account & "_" & rPilot.ID
        Dim ext As String = ".xml"
        If sType = "Character" Then
            Call Me.ClearCharacterCache(prefix, suffix, ext)
        Else
            Call Me.ClearCorporateCache(prefix, suffix, ext)
        End If
        ' Build a corp list
        Call Me.BuildCorpList()
        ' Set the Corp Reps to Default
        PlugInData.CorpReps.Clear()
        For API As Integer = 0 To 6
            PlugInData.CorpReps.Add(New SortedList)
        Next
        ' Rescan XML Data
        Call Me.ScanForExistingXMLs()
    End Sub

    Private Sub ClearCharacterCache(ByVal prefix As String, ByVal suffix As String, ByVal ext As String)
        ' Delete Account Balances
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "AccountBalancesChar" & suffix & ext))
        Catch ex As Exception
        End Try
        ' Delete Assets
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "AssetsChar" & suffix & ext))
        Catch ex As Exception
        End Try
        ' Delete Jobs
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "IndustryChar" & suffix & ext))
        Catch ex As Exception
        End Try
        ' Delete Orders
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "OrdersChar" & suffix & ext))
        Catch ex As Exception
        End Try
        ' Delete Journal
        For account As Integer = 1000 To 1000
            Try
                My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "WalletJournalChar" & suffix & "_" & account.ToString & ext))
            Catch ex As Exception
            End Try
        Next
        ' Delete Transactions
        For account As Integer = 1000 To 1000
            Try
                My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "WalletTransChar" & suffix & "_" & account.ToString & ext))
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub ClearCorporateCache(ByVal prefix As String, ByVal suffix As String, ByVal ext As String)
        ' Delete Account Balances
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "AccountBalancesCorp" & suffix & ext))
        Catch ex As Exception
        End Try
        ' Delete Assets
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "AssetsCorp" & suffix & ext))
        Catch ex As Exception
        End Try
        ' Delete Jobs
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "IndustryCorp" & suffix & ext))
        Catch ex As Exception
        End Try
        ' Delete Orders
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "OrdersCorp" & suffix & ext))
        Catch ex As Exception
        End Try
        ' Delete Journal
        For account As Integer = 1000 To 1006
            Try
                My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "WalletJournalCorp" & suffix & "_" & account.ToString & ext))
            Catch ex As Exception
            End Try
        Next
        ' Delete Transactions
        For account As Integer = 1000 To 1006
            Try
                My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "WalletTransCorp" & suffix & "_" & account.ToString & ext))
            Catch ex As Exception
            End Try
        Next
        ' Delete Corp Sheet
        Try
            My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.cacheFolder, prefix & "CorpSheet" & suffix & ext))
        Catch ex As Exception
        End Try
    End Sub

#End Region

    Private Sub mnuExportToCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportToCSV.Click
        Call Me.ExportToClipboard("PRISM Item Recycling Analysis", clvRecycle, EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
    End Sub

    Private Sub mnuExportToTSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportToTSV.Click
        Call Me.ExportToClipboard("PRISM Item Recycling Analysis", clvRecycle, ControlChars.Tab)
    End Sub

    Private Sub mnuExportTotalsToCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportTotalsToCSV.Click
        Call Me.ExportToClipboard("PRISM Item Recycling Totals", clvTotals, EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
    End Sub

    Private Sub mnuExportTotalsToTSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportTotalsToTSV.Click
        Call Me.ExportToClipboard("PRISM Item Recycling Totals", clvTotals, ControlChars.Tab)
    End Sub

    Private Sub ExportToClipboard(ByVal title As String, ByVal sourceList As ContainerListView, ByVal sepChar As String)
        Dim str As New StringBuilder
        ' Add a line for the current build job
        str.AppendLine(title)
        str.AppendLine("")
        ' Add some headings
        For c As Integer = 0 To sourceList.Columns.Count - 2
            str.Append(sourceList.Columns(c).Text & sepChar)
        Next
        str.AppendLine(sourceList.Columns(sourceList.Columns.Count - 1).Text)
        ' Add the details
        For Each req As ContainerListViewItem In sourceList.Items
            For c As Integer = 0 To sourceList.Columns.Count - 2
                str.Append(req.SubItems(c).Text & sepChar)
            Next
            str.AppendLine(req.SubItems(sourceList.Columns.Count - 1).Text)
        Next
        ' Copy to the clipboard
        Try
            Clipboard.SetText(str.ToString)
        Catch ex As Exception
            MessageBox.Show("Unable to copy Resource Data to the clipboard.", "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

End Class

Public Enum PrismRepCodes As Integer
    Assets = 0
    Balances = 1
    Jobs = 2
    Journals = 3
    Orders = 4
    Transactions = 5
    CorpSheet = 6
End Enum

