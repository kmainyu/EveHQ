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
Imports System.Windows.Forms
Imports System.Xml
Imports System
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Resources
Imports System.IO
Imports System.Diagnostics
Imports System.Text
Imports System.Runtime.Serialization.Formatters.Binary
Imports DevComponents.DotNetBar
Imports DevComponents.AdvTree
Imports System.Drawing
Imports System.Threading
Imports System.Data

Public Class frmPrism

#Region "Class Wide Variables"

    Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    Dim startup As Boolean = True
    Dim SelectedTab As TabItem
    Dim divisions As New SortedList
    Dim loadedAssets As New SortedList
    Dim PrismThreadMax As Integer = 16
    Dim PrismThreadCurrent As Integer = 0
    Dim MaxAPIRetries As Integer = 3
    Dim MaxAPIJournals As Integer = 500

    Dim BPManagerUpdate As Boolean = False

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

    ' Node Element Styles
    Dim StyleRed As New ElementStyle
    Dim StyleRedRight As New ElementStyle
    Dim StyleGreen As New ElementStyle
    Dim StyleGreenRight As New ElementStyle
    Dim StyleRight As New ElementStyle

    ' BPManager Styles
    Dim BPMStyleUnknown As ElementStyle
    Dim BPMStyleBPO As ElementStyle
    Dim BPMStyleBPC As ElementStyle
    Dim BPMStyleUser As ElementStyle
    Dim BPMStyleMissing As ElementStyle
    Dim BPMStyleExhausted As ElementStyle

    Delegate Sub CheckXMLDelegate(ByVal apiXML As XmlDocument, ByVal Owner As PrismOwner, ByVal APIType As CorpRepType)
    Private XMLDelegate As CheckXMLDelegate

#End Region

#Region "Form Initialisation Routines"

    Private Sub frmPrism_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        XMLDelegate = New CheckXMLDelegate(AddressOf CheckXML)

        ' Add events
        AddHandler PrismEvents.UpdateProductionJobs, AddressOf UpdateProductionJobList
        AddHandler PrismEvents.UpdateInventionJobs, AddressOf UpdateInventionJobList
        AddHandler PrismEvents.UpdateBatchJobs, AddressOf UpdateBatchList
        AddHandler PrismEvents.RecyclingInfoAvailable, AddressOf RecycleInfoFromAssets

        ' Load the settings!
        Call Settings.PrismSettings.LoadPrismSettings()

        ' Load the Production Jobs
        Call ProductionJobs.LoadProductionJobs()
        ' Load the Batch Jobs
        Call BatchJobs.LoadBatchJobs()

        tabPrism.Dock = DockStyle.Fill

        startup = True

        ' Create the styles
        StyleRed = adtJournal.Styles("ElementStyle1").Copy
        StyleRed.TextColor = Color.Red
        StyleRedRight = adtJournal.Styles("ElementStyle1").Copy
        StyleRedRight.TextColor = Color.Red
        StyleRedRight.TextAlignment = eStyleTextAlignment.Far
        StyleGreen = adtJournal.Styles("ElementStyle1").Copy
        StyleGreen.TextColor = Color.LimeGreen
        StyleGreenRight = adtJournal.Styles("ElementStyle1").Copy
        StyleGreenRight.TextColor = Color.LimeGreen
        StyleGreenRight.TextAlignment = eStyleTextAlignment.Far
        StyleRight = adtJournal.Styles("ElementStyle1").Copy
        StyleRight.TextAlignment = eStyleTextAlignment.Far

        ' Create BPM Styles
        BPMStyleBPC = adtBlueprints.Styles("BP").Copy
        BPMStyleBPO = adtBlueprints.Styles("BP").Copy
        BPMStyleExhausted = adtBlueprints.Styles("BP").Copy
        BPMStyleMissing = adtBlueprints.Styles("BP").Copy
        BPMStyleUnknown = adtBlueprints.Styles("BP").Copy
        BPMStyleUser = adtBlueprints.Styles("BP").Copy
        BPMStyleBPC.BackColor2 = Color.LightSteelBlue
        BPMStyleBPC.BackColor = Color.FromArgb(128, BPMStyleBPC.BackColor2)
        BPMStyleBPO.BackColor2 = Color.LightGreen
        BPMStyleBPO.BackColor = Color.FromArgb(128, BPMStyleBPO.BackColor2)
        BPMStyleExhausted.BackColor2 = Color.Orange
        BPMStyleExhausted.BackColor = Color.FromArgb(128, BPMStyleExhausted.BackColor2)
        BPMStyleMissing.BackColor2 = Color.LightCoral
        BPMStyleMissing.BackColor = Color.FromArgb(128, BPMStyleMissing.BackColor2)
        BPMStyleUnknown.BackColor2 = Color.LightGray
        BPMStyleUnknown.BackColor = Color.FromArgb(128, BPMStyleUnknown.BackColor2)
        BPMStyleUser.BackColor2 = Color.Yellow
        BPMStyleUser.BackColor = Color.FromArgb(128, BPMStyleUser.BackColor2)

        ' Build a corp list
        Call Me.BuildOwnerList()

        ' Hide excess tabs
        For TabNo As Integer = 1 To tabPrism.Tabs.Count - 1
            tabPrism.Tabs(TabNo).Visible = False
        Next

        ' Initialise the Report data
        Call Me.InitialiseReports()

        ' Initialise the Chart data
        Call Me.InitialiseCharts()

        ' Initialise the Journal and Transaction Data
        Call Me.InitialiseJournal()
        Call Me.InitialiseTransactions()
        Call Me.InitialiseInventionResults()

        ' Build the BP Manager category lists
        cboCategoryFilter.BeginUpdate()
        cboCategoryFilter.Items.Clear()
        cboCategoryFilter.Items.Add("All")
        For Each cat As String In PlugInData.CategoryNames.Keys
            cboCategoryFilter.Items.Add(cat)
        Next
        cboCategoryFilter.EndUpdate()

        ' Build the Job Activity List
        cboActivityFilter.BeginUpdate()
        cboActivityFilter.Items.Clear()
        cboActivityFilter.Items.Add("<All>")
        For Each activity As String In [Enum].GetNames(GetType(JobActivity))
            cboActivityFilter.Items.Add(activity)
        Next
        cboActivityFilter.EndUpdate()
        cboActivityFilter.SelectedIndex = 0

        ' Build the Job Status List
        cboStatusFilter.BeginUpdate()
        cboStatusFilter.Items.Clear()
        cboStatusFilter.Items.Add("<All>")
        For Each status As String In PlugInData.Statuses.Values
            cboStatusFilter.Items.Add(status)
        Next
        cboStatusFilter.EndUpdate()
        cboStatusFilter.SelectedIndex = 0

        Call Me.ScanForExistingXMLs()

        ' Initialise default Prism character
        If Settings.PrismSettings.DefaultCharacter <> "" And PlugInData.PrismOwners.ContainsKey(Settings.PrismSettings.DefaultCharacter) Then
            ' Wallet Journal
            CType(cboJournalOwners.DropDownControl, PrismSelectionControl).UpdateList()
            ' Wallet Transactions
            CType(cboTransactionOwner.DropDownControl, PrismSelectionControl).UpdateList()
            ' Assets
            CType(PAC.PSCAssetOwners.cboHost.DropDownControl, PrismSelectionControl).UpdateList()
            ' Market Orders
            cboOrdersOwner.SelectedItem = Settings.PrismSettings.DefaultCharacter
            ' Research Jobs
            cboJobOwner.SelectedItem = Settings.PrismSettings.DefaultCharacter
            ' BP Manager
            cboBPOwner.SelectedItem = Settings.PrismSettings.DefaultCharacter
            ' Contracts
            cboContractOwner.SelectedItem = Settings.PrismSettings.DefaultCharacter
        End If

        ' Set the refining info
        ' Set the pilot to the recycling one
        If cboRecyclePilots.Items.Contains(RecyclerAssetOwner) Then
            cboRecyclePilots.SelectedItem = RecyclerAssetOwner
        Else
            If cboRecyclePilots.Items.Count > 0 Then
                cboRecyclePilots.SelectedIndex = 0
            End If
        End If
        ' Set the recycling mode
        cboRefineMode.SelectedIndex = 0
        startup = False

    End Sub

    ''' <summary>
    ''' Builds a list of all owners to be used by Prism, and also builds the corp list
    ''' Could replace the LoadedOwners list
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildOwnerList()

        ' Clear the lists
        PlugInData.PrismOwners.Clear()
        PlugInData.CorpList.Clear()

        For Each SelAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            If SelAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then
                Select Case SelAccount.APIKeySystem
                    Case Core.APIKeySystems.Version1
                        For Each Owner As String In SelAccount.Characters
                            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(Owner) Then
                                ' This is a pilot under an APIv1 account
                                Dim SelPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Owner), Core.Pilot)
                                If SelPilot.Active = True Then
                                    ' Add this to the owners list
                                    Dim NewOwner As New PrismOwner
                                    NewOwner.Name = SelPilot.Name
                                    NewOwner.ID = SelPilot.ID
                                    NewOwner.Account = SelAccount
                                    NewOwner.IsCorp = False
                                    NewOwner.APIVersion = Core.APIKeySystems.Version1
                                    PlugInData.PrismOwners.Add(NewOwner.Name, NewOwner)
                                    ' Check if this is a APIv1 key, and see if we need to add the corp
                                    ' We don't add the corp if there is a APIv2 key at all for the corp
                                    ' It will be either APIv1 or APIv2, not BOTH - it's enough fucking work as it is
                                    If EveHQ.Core.HQ.EveHQSettings.Corporations.ContainsKey(SelPilot.Corp) = False Then
                                        If PlugInData.NPCCorps.Contains(SelPilot.CorpID) = False Then
                                            If PlugInData.PrismOwners.ContainsKey(SelPilot.Corp) = False Then
                                                ' Let's add the corp
                                                NewOwner = New PrismOwner
                                                NewOwner.Name = SelPilot.Corp
                                                NewOwner.ID = SelPilot.CorpID
                                                NewOwner.Account = SelAccount
                                                NewOwner.IsCorp = True
                                                NewOwner.APIVersion = Core.APIKeySystems.Version1
                                                PlugInData.PrismOwners.Add(NewOwner.Name, NewOwner)
                                                ' Add the corp to the CorpList
                                                PlugInData.CorpList.Add(SelPilot.Corp, SelPilot.CorpID)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    Case Core.APIKeySystems.Version2
                        ' Check the type of the key
                        Select Case SelAccount.APIKeyType
                            Case Core.APIKeyTypes.Corporation
                                ' A corporate API key
                                For Each Owner As String In SelAccount.Characters
                                    If EveHQ.Core.HQ.EveHQSettings.Corporations.ContainsKey(Owner) Then
                                        Dim SelCorp As EveHQ.Core.Corporation = EveHQ.Core.HQ.EveHQSettings.Corporations(Owner)
                                        If PlugInData.NPCCorps.Contains(SelCorp.ID) = False Then
                                            If PlugInData.PrismOwners.ContainsKey(Owner) = False Then
                                                Dim NewOwner As New PrismOwner
                                                NewOwner.Account = SelAccount
                                                NewOwner.Name = SelCorp.Name
                                                NewOwner.ID = SelCorp.ID
                                                NewOwner.IsCorp = True
                                                NewOwner.APIVersion = Core.APIKeySystems.Version2
                                                PlugInData.PrismOwners.Add(NewOwner.Name, NewOwner)
                                            End If
                                            ' Add the corp to the CorpList
                                            PlugInData.CorpList.Add(SelCorp.Name, SelCorp.ID)
                                        End If
                                    End If
                                Next
                            Case Core.APIKeyTypes.Account, Core.APIKeyTypes.Character
                                ' A character related API key
                                For Each Owner As String In SelAccount.Characters
                                    If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(Owner) Then
                                        Dim SelPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Owner), Core.Pilot)
                                        If PlugInData.PrismOwners.ContainsKey(Owner) = False Then
                                            Dim NewOwner As New PrismOwner
                                            NewOwner.Account = SelAccount
                                            NewOwner.Name = SelPilot.Name
                                            NewOwner.ID = SelPilot.ID
                                            NewOwner.IsCorp = False
                                            NewOwner.APIVersion = Core.APIKeySystems.Version2
                                            PlugInData.PrismOwners.Add(NewOwner.Name, NewOwner)
                                        End If
                                    End If
                                Next
                            Case Else
                                ' Do nothing
                        End Select
                End Select
            End If
        Next

    End Sub

    ''' <summary>
    ''' Looks at existing XML files to determine the cache status
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ScanForExistingXMLs()

        lvwCurrentAPIs.BeginUpdate()
        lvwCurrentAPIs.Items.Clear()
        Dim fileName As String = ""
        Dim apiXML As New XmlDocument

        ' Cycle through our list of Prism owners and set up the API status matrix
        ' We have already checked the account active status and the pilot active status so no need to do this again
        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            If lvwCurrentAPIs.Items.ContainsKey(Owner.ID) = False Then
                Dim newOwner As New ListViewItem
                newOwner.UseItemStyleForSubItems = False
                newOwner.Name = Owner.ID
                newOwner.Text = Owner.Name
                newOwner.ToolTipText = ""
                For si As Integer = 1 To 9
                    newOwner.SubItems.Add("")
                Next
                Select Case Owner.IsCorp
                    Case True
                        newOwner.SubItems(1).Text = "Corporation"
                    Case False
                        newOwner.SubItems(1).Text = "Character"
                        newOwner.SubItems(9).Text = "n/a"
                        cboRecyclePilots.Items.Add(Owner.Name)
                End Select
                lvwCurrentAPIs.Items.Add(newOwner)
            End If
        Next

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Call CheckXMLs(Owner)
        Next

        lvwCurrentAPIs.EndUpdate()
        Call Me.CompleteAPIUpdate()
    End Sub

    ''' <summary>
    ''' Checks existing XML files for display on Prism startup
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckXMLs(Owner As PrismOwner)
        Select Case Owner.IsCorp
            Case True
                Call CheckCorpXMLs(Owner)
            Case False
                Call CheckCharXMLs(Owner)
        End Select
    End Sub

    Private Sub CheckCharXMLs(Owner As PrismOwner)

        If Owner.IsCorp = False Then
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(Owner.Name) = True Then
                Dim SelPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Owner.Name), Core.Pilot)
                Dim PilotAccount As EveHQ.Core.EveAccount = Owner.Account

                Dim apiXML As New XmlDocument
                Dim ReturnMethod As EveAPI.APIReturnMethods = EveAPI.APIReturnMethods.ReturnCacheOnly
                Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                ' Check for char assets
                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, PilotAccount.ToAPIAccount, SelPilot.ID, ReturnMethod)
                Call CheckXML(apiXML, Owner, CorpRepType.Assets)

                ' Check for char balances
                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.AccountBalancesChar, PilotAccount.ToAPIAccount, SelPilot.ID, ReturnMethod)
                Call CheckXML(apiXML, Owner, CorpRepType.Balances)

                ' Check for char jobs
                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.IndustryChar, PilotAccount.ToAPIAccount, SelPilot.ID, ReturnMethod)
                Call CheckXML(apiXML, Owner, CorpRepType.Jobs)

                ' Check for char journal
                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalChar, PilotAccount.ToAPIAccount, SelPilot.ID, 1000, 0, 256, ReturnMethod)
                Call CheckXML(apiXML, Owner, CorpRepType.WalletJournal)

                ' Check for char orders
                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersChar, PilotAccount.ToAPIAccount, SelPilot.ID, ReturnMethod)
                Call CheckXML(apiXML, Owner, CorpRepType.Orders)

                ' Check for char transactions
                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletTransChar, PilotAccount.ToAPIAccount, SelPilot.ID, 1000, "", ReturnMethod)
                Call CheckXML(apiXML, Owner, CorpRepType.WalletTransactions)

                ' Check for char contracts
                apiXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsChar, PilotAccount.ToAPIAccount, SelPilot.ID, ReturnMethod)
                Call CheckXML(apiXML, Owner, CorpRepType.Contracts)

                ' Check for corp sheets
                If Settings.PrismSettings.CorpReps.ContainsKey(SelPilot.Corp) Then
                    If Settings.PrismSettings.CorpReps(SelPilot.Corp).ContainsKey(CorpRepType.CorpSheet) Then
                        If Settings.PrismSettings.CorpReps(SelPilot.Corp).Item(CorpRepType.CorpSheet) = SelPilot.Name Then
                            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.CorpSheet, PilotAccount.ToAPIAccount, SelPilot.ID, ReturnMethod)
                            Call CheckXML(apiXML, Owner, CorpRepType.CorpSheet)
                        Else
                            apiXML = Nothing
                        End If
                    Else
                        apiXML = Nothing
                    End If
                Else
                    apiXML = Nothing
                End If

            End If
        End If
    End Sub

    Private Sub CheckCorpXMLs(Owner As PrismOwner)

        If Owner.IsCorp = True Then

            Dim CorpAccount As EveHQ.Core.EveAccount = Owner.Account

            Dim apiXML As New XmlDocument
            Dim ReturnMethod As EveAPI.APIReturnMethods = EveAPI.APIReturnMethods.ReturnCacheOnly
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            Dim OwnerID As String = ""

            ' Check for corp assets
            OwnerID = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Assets)
            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, CorpAccount.ToAPIAccount, OwnerID, ReturnMethod)
            Call CheckXML(apiXML, Owner, CorpRepType.Assets)

            ' Check for corp balances
            OwnerID = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Balances)
            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.AccountBalancesCorp, CorpAccount.ToAPIAccount, OwnerID, ReturnMethod)
            Call CheckXML(apiXML, Owner, CorpRepType.Balances)

            ' Check for corp jobs
            OwnerID = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Jobs)
            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.IndustryCorp, CorpAccount.ToAPIAccount, OwnerID, ReturnMethod)
            Call CheckXML(apiXML, Owner, CorpRepType.Jobs)

            ' Check for corp journal
            OwnerID = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.WalletJournal)
            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalCorp, CorpAccount.ToAPIAccount, OwnerID, 1000, 0, 256, ReturnMethod)
            Call CheckXML(apiXML, Owner, CorpRepType.WalletJournal)

            ' Check for corp orders
            OwnerID = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Orders)
            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersCorp, CorpAccount.ToAPIAccount, OwnerID, ReturnMethod)
            Call CheckXML(apiXML, Owner, CorpRepType.Orders)

            ' Check for corp transactions
            OwnerID = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.WalletTransactions)
            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletTransCorp, CorpAccount.ToAPIAccount, OwnerID, 1000, "", ReturnMethod)
            Call CheckXML(apiXML, Owner, CorpRepType.WalletTransactions)

            ' Check for corp contracts
            OwnerID = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Contracts)
            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsCorp, CorpAccount.ToAPIAccount, OwnerID, ReturnMethod)
            Call CheckXML(apiXML, Owner, CorpRepType.Contracts)

            ' Check for corp sheets
            OwnerID = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.CorpSheet)
            apiXML = APIReq.GetAPIXML(EveAPI.APITypes.CorpSheet, CorpAccount.ToAPIAccount, OwnerID, ReturnMethod)
            Call CheckXML(apiXML, Owner, CorpRepType.CorpSheet)

        End If

    End Sub

    Private Sub btnRefreshAPI_Click(sender As System.Object, e As System.EventArgs) Handles btnRefreshAPI.Click
        startup = True
        Call Me.ScanForExistingXMLs()
        startup = False
    End Sub

#End Region

#Region "Form Closing Routines"

    Private Sub frmPrism_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        ' Save the current blueprints
        Dim s As New FileStream(Path.Combine(Settings.PrismFolder, "OwnerBlueprints.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, PlugInData.BlueprintAssets)
        s.Flush()
        s.Close()

        ' Save the Production Jobs
        Call ProductionJobs.SaveProductionJobs()
        ' Save the Batch Jobs
        Call BatchJobs.SaveBatchJobs()

        ' Remove events
        RemoveHandler PrismEvents.UpdateProductionJobs, AddressOf UpdateProductionJobList
        RemoveHandler PrismEvents.UpdateBatchJobs, AddressOf UpdateBatchList

        ' Save the settings
        Call Settings.PrismSettings.SavePrismSettings()

    End Sub

#End Region

#Region "XML Retrieval and Parsing"

    Private Sub StartGetXMLDataThread()
        ' Perform this so that the API download process doesn't block the main UI thread
        Threading.ThreadPool.QueueUserWorkItem(AddressOf GetXMLData, Nothing)
    End Sub

    Private Sub GetXMLData(ByVal State As Object)

        PrismThreadMax = 16
        PrismThreadCurrent = 0

        ' Start collection of threads
        Dim PrismThreads As New List(Of Thread)

        ' Setup separate threads for getting each type of API
        PrismThreads.Add(New Thread(AddressOf GetCharAssets2))
        PrismThreads.Add(New Thread(AddressOf GetCorpAssets2))
        PrismThreads.Add(New Thread(AddressOf GetCharBalances2))
        PrismThreads.Add(New Thread(AddressOf GetCorpBalances2))
        PrismThreads.Add(New Thread(AddressOf GetCharJobs2))
        PrismThreads.Add(New Thread(AddressOf GetCorpJobs2))
        PrismThreads.Add(New Thread(AddressOf GetCharJournal2))
        PrismThreads.Add(New Thread(AddressOf GetCorpJournal2))
        PrismThreads.Add(New Thread(AddressOf GetCharOrders2))
        PrismThreads.Add(New Thread(AddressOf GetCorpOrders2))
        PrismThreads.Add(New Thread(AddressOf GetCharTransactions2))
        PrismThreads.Add(New Thread(AddressOf GetCorpTransactions2))
        PrismThreads.Add(New Thread(AddressOf GetCharContracts2))
        PrismThreads.Add(New Thread(AddressOf GetCorpContracts2))
        PrismThreads.Add(New Thread(AddressOf GetCorpSheet2))
        PrismThreads.Add(New Thread(AddressOf UpdateNullCorpSheet2))

        ' Start the threads
        For Each PT As Thread In PrismThreads
            PT.Start()
        Next

    End Sub

    Private Sub CheckXML(apiXML As XmlDocument, Owner As PrismOwner, APIType As CorpRepType)

        ' Get the listviewitem of the relevant Owner
        Dim APIOwner As ListViewItem = lvwCurrentAPIs.Items(Owner.ID)
        ' Get the position of the cell in the listviewitem
        Dim Pos As Integer = APIType + 2

        Try

            'Dim cAccount As EveHQ.Core.EveAccount = Owner.Account
            'Dim IsCorp As Boolean = Owner.IsCorp

            Select Case Owner.APIVersion
                Case Core.APIKeySystems.Version1

                    ' Checking XML of APIv1 keys
                    If apiXML IsNot Nothing Then
                        If Owner.IsCorp = False And APIType = CorpRepType.CorpSheet Then
                            APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Black
                            APIOwner.SubItems(Pos).Text = "n/a"
                        Else
                            If CanUseAPIv1(Owner, APIType) Then
                                ' We have an XML file and we can use the APIv1
                                Call DisplayAPIDetails(apiXML, APIOwner, Pos)
                            Else
                                ' Put generic "No Access" notice here, but we could expand on this later
                                APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                                APIOwner.SubItems(Pos).Text = "No Access"
                            End If
                        End If
                    Else
                        ' We don't have an XML file...
                        If Owner.IsCorp = False And APIType = CorpRepType.CorpSheet Then
                            APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Black
                            APIOwner.SubItems(Pos).Text = "n/a"
                        Else
                            If CanUseAPIv1(Owner, APIType) Then
                                ' ...but we can use it (it's just missing for now)
                                APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                                APIOwner.SubItems(Pos).Text = "Missing"
                            Else
                                ' Put generic "No Access" notice here, but we could expand on this later
                                APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                                APIOwner.SubItems(Pos).Text = "No Access"
                            End If
                        End If
                    End If

                Case Core.APIKeySystems.Version2

                    ' Checking XML of APIv2 keys
                    If apiXML IsNot Nothing Then
                        If CanUseAPIv2(Owner, APIType) Then
                            Call DisplayAPIDetails(apiXML, APIOwner, Pos)
                        Else
                            APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                            APIOwner.SubItems(Pos).Text = "No Access"
                        End If
                    Else
                        If Owner.IsCorp = False And APIType = CorpRepType.CorpSheet Then
                            APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Black
                            APIOwner.SubItems(Pos).Text = "n/a"
                        Else
                            If CanUseAPIv2(Owner, APIType) Then
                                ' ...but we can use it (it's just missing for now)
                                APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                                APIOwner.SubItems(Pos).Text = "Missing"
                            Else
                                ' Put generic "No Access" notice here, but we could expand on this later
                                APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Red
                                APIOwner.SubItems(Pos).Text = "No Access"
                            End If
                        End If
                    End If

            End Select

        Catch ex As Exception
            Dim msg As String = "An error occured while processing the XML data." & ControlChars.CrLf
            msg &= "The specific error was: " & ex.Message & ControlChars.CrLf
            msg &= "The stacktrace was: " & ex.StackTrace & ControlChars.CrLf
            MessageBox.Show(msg, "CheckXML Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub DisplayAPIDetails(ByVal apiXML As XmlDocument, ByVal APIOwner As ListViewItem, ByVal Pos As Integer)
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
                APIOwner.SubItems(Pos).Text = "Cache Expired!"
            Else
                APIOwner.SubItems(Pos).ForeColor = Drawing.Color.Green
                APIOwner.SubItems(Pos).Text = Format(cache, "dd MMM HH:mm")
            End If
        End If
    End Sub
    Private Function CanUseAPI(ByVal Owner As PrismOwner, ByVal APIType As CorpRepType) As Boolean
        Select Case Owner.APIVersion
            Case Core.APIKeySystems.Version1
                Return CanUseAPIv1(Owner, APIType)
            Case Core.APIKeySystems.Version2
                Return CanUseAPIv2(Owner, APIType)
            Case Else
                Return False
        End Select
    End Function
    Private Function CanUseAPIv1(ByVal Owner As PrismOwner, ByVal APIType As CorpRepType) As Boolean
        ' Check if this is a corp owner as we have a lot to do if it is!
        If Owner.IsCorp = False Then

            ' This is a character so we can use the account
            Dim Account As EveHQ.Core.EveAccount = Owner.Account
            ' Only need to confirm the account is active and the API key is a full one
            If Account.APIAccountStatus = Core.APIAccountStatuses.Active And Account.APIKeyType = Core.APIKeyTypes.Full Then
                Return True
            Else
                Return False
            End If

        Else

            ' This is a corporation so we need to check determine the corp rep, key type and roles
            If Settings.PrismSettings.CorpReps.ContainsKey(Owner.Name) Then
                If Settings.PrismSettings.CorpReps(Owner.Name).ContainsKey(APIType) Then
                    ' Get the corp rep responsible for this corp and API type
                    Dim SelPilotName As String = Settings.PrismSettings.CorpReps(Owner.Name).Item(APIType)
                    If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(SelPilotName) Then
                        Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(SelPilotName), Core.Pilot)
                        ' Corp rep established, now get the account
                        If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(selPilot.Account) = True Then
                            Dim Account As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(selPilot.Account), Core.EveAccount)
                            ' Check the account is active and the API key is a full one
                            If Account.APIAccountStatus = Core.APIAccountStatuses.Active And Account.APIKeyType = Core.APIKeyTypes.Full Then
                                ' Check if the pilot has the required roles to access this particular API
                                Return HasCorpRoles(selPilot, APIType)
                            Else
                                Return False
                            End If
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End If

    End Function
    Private Function CanUseAPIv2(ByVal Owner As PrismOwner, ByVal APIType As CorpRepType) As Boolean
        Dim Account As EveHQ.Core.EveAccount = Owner.Account
        If Account.APIKeySystem = Core.APIKeySystems.Version2 Then
            Select Case Account.APIKeyType
                Case Core.APIKeyTypes.Corporation
                    Select Case APIType
                        Case CorpRepType.Assets
                            Return Account.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.AssetList)
                        Case CorpRepType.Balances
                            Return Account.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.AccountBalances)
                        Case CorpRepType.Contracts
                            Return Account.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.Contracts)
                        Case CorpRepType.CorpSheet
                            Return Account.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.CorporationSheet)
                        Case CorpRepType.Jobs
                            Return Account.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.IndustryJobs)
                        Case CorpRepType.Orders
                            Return Account.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.MarketOrders)
                        Case CorpRepType.WalletJournal
                            Return Account.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.WalletJournal)
                        Case CorpRepType.WalletTransactions
                            Return Account.CanUseCorporateAPI(EveAPI.CorporateAccessMasks.WalletTransactions)
                    End Select
                Case Else
                    Select Case APIType
                        Case CorpRepType.Assets
                            Return Account.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.AssetList)
                        Case CorpRepType.Balances
                            Return Account.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.AccountBalances)
                        Case CorpRepType.Contracts
                            Return Account.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.Contracts)
                        Case CorpRepType.CorpSheet
                            Return False
                        Case CorpRepType.Jobs
                            Return Account.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.IndustryJobs)
                        Case CorpRepType.Orders
                            Return Account.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.MarketOrders)
                        Case CorpRepType.WalletJournal
                            Return Account.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.WalletJournal)
                        Case CorpRepType.WalletTransactions
                            Return Account.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.WalletTransactions)
                    End Select
            End Select
        Else
            Return False
        End If
    End Function
    'Private Function HasFullKey(ByVal Primary As String) As Boolean
    '    If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(Primary) = True Then
    '        If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Primary), EveHQ.Core.Pilot).Account) Then
    '            Dim CheckAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Primary), EveHQ.Core.Pilot).Account), EveHQ.Core.EveAccount)
    '            If CheckAccount.APIAccountStatus = Core.APIAccountStatuses.Active And CheckAccount.APIKeyType = Core.APIKeyTypes.Full Then
    '                Return True
    '            End If
    '        End If
    '    End If
    '    Return False
    'End Function
    Private Function HasCorpRoles(cPilot As EveHQ.Core.Pilot, ByVal APIType As CorpRepType) As Boolean
        ' Check if CorpRoles have been initialised
        If cPilot.CorpRoles Is Nothing Then
            cPilot.CorpRoles = New List(Of EveHQ.Core.CorporationRoles)
        End If
        ' Check for roles on rep type
        Select Case APIType
            Case CorpRepType.Assets
                If cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Director) Then
                    Return True
                End If
            Case CorpRepType.Balances
                If cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Director) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Accountant) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.JuniorAccountant) Then
                    Return True
                End If
            Case CorpRepType.CorpSheet
                Return True
            Case CorpRepType.Jobs
                If cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Director) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.FactoryManager) Then
                    Return True
                End If
            Case CorpRepType.Orders
                If cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Director) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Accountant) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.JuniorAccountant) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Trader) Then
                    Return True
                End If
            Case CorpRepType.WalletJournal
                If cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Director) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Accountant) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.JuniorAccountant) Then
                    Return True
                End If
            Case CorpRepType.WalletTransactions
                If cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Director) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Accountant) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.JuniorAccountant) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Trader) Then
                    Return True
                End If
            Case CorpRepType.Contracts
                If cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Director) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Accountant) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.JuniorAccountant) Or cPilot.CorpRoles.Contains(EveHQ.Core.CorporationRoles.Trader) Then
                    Return True
                End If
        End Select
        Return False
    End Function
    Private Sub CompleteAPIUpdate()
        ' Populate the various Owner boxes
        Call Me.UpdatePrismOwners()
        'Call Me.UpdatePrismInfo()
        ' Set the label, enable the button and inform the user
        lblCurrentAPI.Text = "Cached APIs Loaded:"
        btnDownloadAPIData.Enabled = True
        If startup = False Then
            MessageBox.Show("Prism has completed the download of the API data. You may need to refresh your views to get updated information.", "API Download complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub UpdatePrismOwners()

        ' Check for old items
        Dim OldBPOwner As String = ""
        Dim OldJobOwner As String = ""
        Dim OldOrdersOwner As String = ""
        Dim OldContractOwner As String = ""

        Dim OldTransactionOwner As String = ""
        If cboBPOwner.SelectedItem IsNot Nothing Then
            OldBPOwner = cboBPOwner.SelectedItem.ToString
        End If
        If cboJobOwner.SelectedItem IsNot Nothing Then
            OldJobOwner = cboJobOwner.SelectedItem.ToString
        End If
        If cboOrdersOwner.SelectedItem IsNot Nothing Then
            OldOrdersOwner = cboOrdersOwner.SelectedItem.ToString
        End If
        If cboContractOwner.SelectedItem IsNot Nothing Then
            OldContractOwner = cboContractOwner.SelectedItem.ToString
        End If

        ' Prepare each of the owner lists for loading
        cboBPOwner.BeginUpdate() : cboBPOwner.Items.Clear()
        cboJobOwner.BeginUpdate() : cboJobOwner.Items.Clear()
        cboOrdersOwner.BeginUpdate() : cboOrdersOwner.Items.Clear()
        cboContractOwner.BeginUpdate() : cboContractOwner.Items.Clear()

        ' Populate the lists
        For Each Owner As String In PlugInData.PrismOwners.Keys
            cboBPOwner.Items.Add(Owner)
            cboJobOwner.Items.Add(Owner)
            cboOrdersOwner.Items.Add(Owner)
            cboContractOwner.Items.Add(Owner)
        Next

        ' Finalise the loading
        cboBPOwner.Sorted = True : cboBPOwner.EndUpdate()
        cboJobOwner.Sorted = True : cboJobOwner.EndUpdate()
        cboOrdersOwner.Sorted = True : cboOrdersOwner.EndUpdate()
        cboContractOwner.Sorted = True : cboContractOwner.EndUpdate()

        ' Set the old values if applicable
        If OldBPOwner <> "" And cboBPOwner.Items.Contains(OldBPOwner) Then
            cboBPOwner.SelectedItem = OldBPOwner
        End If
        If OldJobOwner <> "" And cboJobOwner.Items.Contains(OldJobOwner) Then
            cboJobOwner.SelectedItem = OldJobOwner
        End If
        If OldOrdersOwner <> "" And cboOrdersOwner.Items.Contains(OldOrdersOwner) Then
            cboOrdersOwner.SelectedItem = OldOrdersOwner
        End If
        If OldContractOwner <> "" And cboContractOwner.Items.Contains(OldContractOwner) Then
            cboContractOwner.SelectedItem = OldContractOwner
        End If

    End Sub

#Region "APIv1"

    'Private Sub GetCharAssets(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then

    '                    Dim APIXML As New XmlDocument
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If pilotAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then
    '                        ' Check for full API
    '                        If CanUseAPI(pilotAccount, CorpRepType.Assets, False, selPilot.Name, selPilot) = True Then

    '                            ' Make a call to the EveHQ.Core.API to fetch the assets

    '                            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                            Dim Retries As Integer = 0
    '                            Do
    '                                Retries += 1
    '                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                        End If
    '                    End If

    '                    ' Update the display
    '                    If Me.IsHandleCreated = True Then
    '                        Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.ID, selPilot.Name, CorpRepType.Assets})
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Character Assets data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCharBalances(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim APIXML As New XmlDocument
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If pilotAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then
    '                        ' Check for full API
    '                        If CanUseAPI(pilotAccount, CorpRepType.Balances, False, selPilot.Name, selPilot) = True Then

    '                            ' Make a call to the EveHQ.Core.API to fetch the balances
    '                            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                            Dim Retries As Integer = 0
    '                            Do
    '                                Retries += 1
    '                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.AccountBalancesChar, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                        End If
    '                    End If

    '                    ' Update the display
    '                    If Me.IsHandleCreated = True Then
    '                        Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.ID, selPilot.Name, CorpRepType.Balances})
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Character Balances data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCharBalances Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCharJobs(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim APIXML As New XmlDocument
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If pilotAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then

    '                        ' Check for full API
    '                        If CanUseAPI(pilotAccount, CorpRepType.Jobs, False, selPilot.Name, selPilot) = True Then

    '                            ' Make a call to the EveHQ.Core.API to fetch the jobs
    '                            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                            Dim Retries As Integer = 0
    '                            Do
    '                                Retries += 1
    '                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.IndustryChar, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                            ' Write the installerIDs to the database
    '                            If APIXML IsNot Nothing Then
    '                                Call Prism.DataFunctions.WriteInstallerIDsToDB(APIXML)
    '                            End If
    '                        End If
    '                    End If

    '                    ' Update the display
    '                    If Me.IsHandleCreated = True Then
    '                        Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.ID, selPilot.Name, CorpRepType.Jobs})
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Character Job data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCharJobs Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCharJournal(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim APIReq As New EveAPI.EveAPIRequest

    '                    Dim APIXML As New XmlDocument
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If pilotAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then

    '                        ' Check for full API
    '                        If CanUseAPI(pilotAccount, CorpRepType.WalletJournal, False, selPilot.Name, selPilot) = True Then

    '                            ' Get the last referenceID for the wallet
    '                            Dim LastTrans As Long = DataFunctions.GetLastWalletID(WalletTypes.Journal, CInt(selPilot.ID), 1000)

    '                            ' Start a loop to collect multiple APIs
    '                            Dim WalletJournals As New SortedList(Of Long, WalletJournalItem)
    '                            Dim LastRefID As Long = 0
    '                            Dim WalletExhausted As Boolean = False
    '                            APIReq = New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

    '                            Do
    '                                ' Make a call to the EveHQ.Core.API to fetch the journal
    '                                Dim Retries As Integer = 0
    '                                Do
    '                                    Retries += 1
    '                                    APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalChar, pilotAccount.ToAPIAccount, selPilot.ID, 1000, LastRefID, MaxAPIJournals, EveAPI.APIReturnMethods.ReturnStandard)
    '                                Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                ' Parse the Journal XML to get the data
    '                                If APIXML IsNot Nothing Then
    '                                    WalletExhausted = Prism.DataFunctions.ParseWalletJournalXML(APIXML, WalletJournals)
    '                                Else
    '                                    WalletExhausted = True
    '                                End If

    '                                If WalletJournals.Count <> 0 Then
    '                                    LastRefID = WalletJournals.Keys(0)
    '                                Else
    '                                    WalletExhausted = True
    '                                End If

    '                            Loop Until WalletExhausted = True Or LastTrans > LastRefID

    '                            ' Write the journal to the database!
    '                            If WalletJournals.Count > 0 Then
    '                                Call Prism.DataFunctions.WriteWalletJournalToDB(WalletJournals, CInt(selPilot.ID), selPilot.Name, 1000, LastTrans)
    '                            End If
    '                        End If
    '                    End If

    '                    ' Update the display
    '                    Dim oldXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalChar, pilotAccount.ToAPIAccount, selPilot.ID, 1000, 0, MaxAPIJournals, EveAPI.APIReturnMethods.ReturnCacheOnly)
    '                    If Me.IsHandleCreated = True Then
    '                        Me.Invoke(XMLDelegate, New Object() {oldXML, selPilot.ID, selPilot.Name, CorpRepType.WalletJournal})
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Character Journal data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCharJournal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCharOrders(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim APIXML As New XmlDocument
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If pilotAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then

    '                        ' Check for full API
    '                        If CanUseAPI(pilotAccount, CorpRepType.Orders, False, selPilot.Name, selPilot) = True Then

    '                            ' Make a call to the EveHQ.Core.API to fetch the orders
    '                            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                            Dim Retries As Integer = 0
    '                            Do
    '                                Retries += 1
    '                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersChar, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                        End If

    '                        ' Update the display
    '                        If Me.IsHandleCreated = True Then
    '                            Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.ID, selPilot.Name, CorpRepType.Orders})
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Character Order data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCharOrders Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCharTransactions(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then

    '                    ' Setup the array of transactions
    '                    Dim transNodes As New ArrayList
    '                    Dim transID As String = ""

    '                    Dim APIXML As New XmlDocument
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If pilotAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then

    '                        ' Check for full API
    '                        If CanUseAPI(pilotAccount, CorpRepType.WalletTransactions, False, selPilot.Name, selPilot) = True Then

    '                            ' Make a call to the EveHQ.Core.API to fetch the transactions
    '                            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                            Dim Retries As Integer = 0
    '                            Do
    '                                Retries += 1
    '                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletTransChar, pilotAccount.ToAPIAccount, selPilot.ID, 1000, transID, EveAPI.APIReturnMethods.ReturnStandard)
    '                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                            ' Write the journal to the database!
    '                            Call Prism.DataFunctions.WriteWalletTransactionsToDB(APIXML, False, CInt(selPilot.ID), selPilot.Name, 1000)

    '                        End If

    '                        ' Update the display
    '                        If Me.IsHandleCreated = True Then
    '                            Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.ID, selPilot.Name, CorpRepType.WalletTransactions})
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Character Transaction data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCharTransactions Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCharContracts(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim APIXML As New XmlDocument
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If pilotAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then
    '                        ' Check for full API
    '                        If CanUseAPI(pilotAccount, CorpRepType.Contracts, False, selPilot.Name, selPilot) = True Then

    '                            ' Make a call to the EveHQ.Core.API to fetch the contracts
    '                            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                            Dim Retries As Integer = 0
    '                            Do
    '                                Retries += 1
    '                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsChar, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                            If APIXML IsNot Nothing Then
    '                                ' Get the Node List
    '                                Dim Contracts As XmlNodeList = APIXML.SelectNodes("/eveapi/result/rowset/row")
    '                                ' Parse the Node List
    '                                For Each ContractItem As XmlNode In Contracts
    '                                    Dim contractID As Long = CLng(ContractItem.Attributes.GetNamedItem("contractID").Value)
    '                                    Retries = 0
    '                                    Do
    '                                        Retries += 1
    '                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractItemsChar, pilotAccount.ToAPIAccount, selPilot.ID, contractID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0
    '                                Next
    '                            End If

    '                            Retries = 0
    '                            Do
    '                                Retries += 1
    '                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractBidsChar, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                        End If
    '                    End If

    '                    ' Update the display
    '                    If Me.IsHandleCreated = True Then
    '                        Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.ID, selPilot.Name, CorpRepType.Contracts})
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Character Order data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCharOrders Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub UpdateNullCorpSheet(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If pilotAccount.APIAccountStatus = Core.APIAccountStatuses.Active Then
    '                        ' Update the display
    '                        If Me.IsHandleCreated = True Then
    '                            Me.Invoke(XMLDelegate, New Object() {Nothing, selPilot.ID, selPilot.Name, CorpRepType.CorpSheet})
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Null Corp Sheet data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "UpdateNullCorpSheet Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub

    'Private Sub GetCorpAssets(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then

    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

    '                    If Settings.PrismSettings.CorpReps.ContainsKey(selPilot.Corp) Then
    '                        If Settings.PrismSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.Assets) Then
    '                            If selPilot.Name = Settings.PrismSettings.CorpReps(selPilot.Corp).Item(CorpRepType.Assets) Then

    '                                Dim APIXML As New XmlDocument

    '                                ' Check for full API
    '                                If CanUseAPI(pilotAccount, CorpRepType.Assets, True, selPilot.CorpID, selPilot) = True Then

    '                                    ' Make a call to the EveHQ.Core.API to fetch the assets
    '                                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                                    Dim Retries As Integer = 0
    '                                    Do
    '                                        Retries += 1
    '                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                End If

    '                                ' Update the display
    '                                If Me.IsHandleCreated = True Then
    '                                    Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.CorpID, selPilot.Name, CorpRepType.Assets})
    '                                End If

    '                            End If
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Corporate Assets data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCorpAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    ' Parse corp accounts

    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCorpBalances(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

    '                    If Settings.PrismSettings.CorpReps.ContainsKey(selPilot.Corp) Then
    '                        If Settings.PrismSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.Balances) Then
    '                            If selPilot.Name = Settings.PrismSettings.CorpReps(selPilot.Corp).Item(CorpRepType.Balances) Then

    '                                Dim APIXML As New XmlDocument

    '                                ' Check for full API
    '                                If CanUseAPI(pilotAccount, CorpRepType.Balances, True, selPilot.CorpID, selPilot) = True Then

    '                                    ' Make a call to the EveHQ.Core.API to fetch the balances
    '                                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                                    Dim Retries As Integer = 0
    '                                    Do
    '                                        Retries += 1
    '                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.AccountBalancesCorp, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                End If

    '                                ' Update the display
    '                                If Me.IsHandleCreated = True Then
    '                                    Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.CorpID, selPilot.Name, CorpRepType.Balances})
    '                                End If

    '                            End If
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Corporate Balances data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCorpBalances Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCorpJobs(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

    '                    If Settings.PrismSettings.CorpReps.ContainsKey(selPilot.Corp) Then
    '                        If Settings.PrismSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.Jobs) Then
    '                            If selPilot.Name = Settings.PrismSettings.CorpReps(selPilot.Corp).Item(CorpRepType.Jobs) Then

    '                                Dim APIXML As New XmlDocument

    '                                ' Check for full API
    '                                If CanUseAPI(pilotAccount, CorpRepType.Jobs, True, selPilot.CorpID, selPilot) = True Then

    '                                    ' Make a call to the EveHQ.Core.API to fetch the jobs
    '                                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                                    Dim Retries As Integer = 0
    '                                    Do
    '                                        Retries += 1
    '                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.IndustryCorp, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                    ' Write the installerIDs to the database
    '                                    If APIXML IsNot Nothing Then
    '                                        Call Prism.DataFunctions.WriteInstallerIDsToDB(APIXML)
    '                                    End If

    '                                End If

    '                                ' Update the display
    '                                If Me.IsHandleCreated = True Then
    '                                    Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.CorpID, selPilot.Name, CorpRepType.Jobs})
    '                                End If

    '                            End If
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Corporate Job data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCorpJobs Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCorpJournal(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

    '                    If Settings.PrismSettings.CorpReps.ContainsKey(selPilot.Corp) Then
    '                        If Settings.PrismSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.WalletJournal) Then
    '                            If selPilot.Name = Settings.PrismSettings.CorpReps(selPilot.Corp).Item(CorpRepType.WalletJournal) Then

    '                                Dim APIReq As New EveAPI.EveAPIRequest
    '                                Dim APIXML As New XmlDocument

    '                                ' Check for full API
    '                                If CanUseAPI(pilotAccount, CorpRepType.WalletJournal, True, selPilot.CorpID, selPilot) = True Then

    '                                    APIReq = New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

    '                                    For divID As Integer = 1006 To 1000 Step -1

    '                                        ' Get the last referenceID for the wallet
    '                                        Dim LastTrans As Long = DataFunctions.GetLastWalletID(WalletTypes.Journal, CInt(selPilot.CorpID), divID)

    '                                        ' Start a loop to collect multiple APIs
    '                                        Dim WalletJournals As New SortedList(Of Long, WalletJournalItem)
    '                                        Dim LastRefID As Long = 0
    '                                        Dim WalletExhausted As Boolean = False

    '                                        Do
    '                                            ' Make a call to the EveHQ.Core.API to fetch the journal
    '                                            Dim Retries As Integer = 0
    '                                            Do
    '                                                Retries += 1
    '                                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalCorp, pilotAccount.ToAPIAccount, selPilot.ID, divID, LastRefID, MaxAPIJournals, EveAPI.APIReturnMethods.ReturnStandard)
    '                                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                            ' Parse the Journal XML to get the data
    '                                            If APIXML IsNot Nothing Then
    '                                                WalletExhausted = Prism.DataFunctions.ParseWalletJournalXML(APIXML, WalletJournals)
    '                                            Else
    '                                                WalletExhausted = True
    '                                            End If

    '                                            If WalletJournals.Count <> 0 Then
    '                                                LastRefID = WalletJournals.Keys(0)
    '                                            Else
    '                                                WalletExhausted = True
    '                                            End If

    '                                        Loop Until WalletExhausted = True Or LastTrans > LastRefID

    '                                        ' Write the journal to the database!
    '                                        If WalletJournals.Count > 0 Then
    '                                            Call Prism.DataFunctions.WriteWalletJournalToDB(WalletJournals, CInt(selPilot.CorpID), selPilot.Corp, divID, LastTrans)
    '                                        End If

    '                                    Next

    '                                End If

    '                                ' Update the display
    '                                Dim oldXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalCorp, pilotAccount.ToAPIAccount, selPilot.ID, 1000, 0, MaxAPIJournals, EveAPI.APIReturnMethods.ReturnCacheOnly)
    '                                If Me.IsHandleCreated = True Then
    '                                    Me.Invoke(XMLDelegate, New Object() {oldXML, selPilot.CorpID, selPilot.Name, CorpRepType.WalletJournal})
    '                                End If

    '                            End If
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Corporate Journal data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCorpJournal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCorpOrders(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If Settings.PrismSettings.CorpReps.ContainsKey(selPilot.Corp) Then
    '                        If Settings.PrismSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.Orders) Then
    '                            If selPilot.Name = Settings.PrismSettings.CorpReps(selPilot.Corp).Item(CorpRepType.Orders) Then

    '                                Dim APIXML As New XmlDocument

    '                                ' Check for full API
    '                                If CanUseAPI(pilotAccount, CorpRepType.Orders, True, selPilot.CorpID, selPilot) = True Then

    '                                    ' Make a call to the EveHQ.Core.API to fetch the orders
    '                                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                                    Dim Retries As Integer = 0
    '                                    Do
    '                                        Retries += 1
    '                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersCorp, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                End If

    '                                ' Update the display
    '                                If Me.IsHandleCreated = True Then
    '                                    Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.CorpID, selPilot.Name, CorpRepType.Orders})
    '                                End If

    '                            End If
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Corporate Order data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCorpOrders Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCorpTransactions(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            Dim accountName As String = selPilot.Account
    '            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                If selPilot.Active = True Then
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

    '                    If Settings.PrismSettings.CorpReps.ContainsKey(selPilot.Corp) Then
    '                        If Settings.PrismSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.WalletTransactions) Then
    '                            If selPilot.Name = Settings.PrismSettings.CorpReps(selPilot.Corp).Item(CorpRepType.WalletTransactions) Then

    '                                Dim APIXML As New XmlDocument

    '                                ' Check for full API
    '                                If CanUseAPI(pilotAccount, CorpRepType.WalletTransactions, True, selPilot.CorpID, selPilot) = True Then

    '                                    ' Make a call to the EveHQ.Core.API to fetch the transactions
    '                                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                                    APIXML = New XmlDocument
    '                                    For divID As Integer = 1006 To 1000 Step -1
    '                                        Dim Retries As Integer = 0
    '                                        Do
    '                                            Retries += 1
    '                                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletTransCorp, pilotAccount.ToAPIAccount, selPilot.ID, divID, "", EveAPI.APIReturnMethods.ReturnStandard)
    '                                        Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                        ' Write the journal to the database!
    '                                        Call Prism.DataFunctions.WriteWalletTransactionsToDB(APIXML, True, CInt(selPilot.CorpID), selPilot.Corp, divID)

    '                                    Next

    '                                End If

    '                                ' Update the display
    '                                If Me.IsHandleCreated = True Then
    '                                    Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.CorpID, selPilot.Name, CorpRepType.WalletTransactions})
    '                                End If

    '                            End If
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Corporate Transaction data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCorpTransactions Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCorpContracts(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
    '                    If Settings.PrismSettings.CorpReps.ContainsKey(selPilot.Corp) Then
    '                        If Settings.PrismSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.Orders) Then
    '                            If selPilot.Name = Settings.PrismSettings.CorpReps(selPilot.Corp).Item(CorpRepType.Orders) Then

    '                                Dim APIXML As New XmlDocument

    '                                ' Check for full API
    '                                If CanUseAPI(pilotAccount, CorpRepType.Contracts, True, selPilot.CorpID, selPilot) = True Then

    '                                    ' Make a call to the EveHQ.Core.API to fetch the contracts
    '                                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                                    Dim Retries As Integer = 0
    '                                    Do
    '                                        Retries += 1
    '                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsCorp, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                    If APIXML IsNot Nothing Then
    '                                        ' Get the Node List
    '                                        Dim Contracts As XmlNodeList = APIXML.SelectNodes("/eveapi/result/rowset/row")
    '                                        ' Parse the Node List
    '                                        For Each ContractItem As XmlNode In Contracts
    '                                            Dim contractID As Long = CLng(ContractItem.Attributes.GetNamedItem("contractID").Value)
    '                                            Retries = 0
    '                                            Do
    '                                                Retries += 1
    '                                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractItemsCorp, pilotAccount.ToAPIAccount, selPilot.ID, contractID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0
    '                                        Next
    '                                    End If

    '                                    Retries = 0
    '                                    Do
    '                                        Retries += 1
    '                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractBidsCorp, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                End If

    '                                ' Update the display
    '                                If Me.IsHandleCreated = True Then
    '                                    Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.CorpID, selPilot.Name, CorpRepType.Contracts})
    '                                End If

    '                            End If
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Corporate Order data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCorpOrders Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub
    'Private Sub GetCorpSheet(ByVal State As Object)
    '    For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
    '        Try
    '            If selPilot.Active = True Then
    '                Dim accountName As String = selPilot.Account
    '                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
    '                    Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)

    '                    If Settings.PrismSettings.CorpReps.ContainsKey(selPilot.Corp) Then
    '                        If Settings.PrismSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.CorpSheet) Then
    '                            If selPilot.Name = Settings.PrismSettings.CorpReps(selPilot.Corp).Item(CorpRepType.CorpSheet) Then

    '                                ' Make a call to the EveHQ.Core.API to fetch the corp sheet
    '                                Dim APIXML As New XmlDocument
    '                                Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
    '                                Dim Retries As Integer = 0
    '                                Do
    '                                    Retries += 1
    '                                    APIXML = APIReq.GetAPIXML(EveAPI.APITypes.CorpSheet, pilotAccount.ToAPIAccount, selPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
    '                                Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

    '                                ' Update the display
    '                                If Me.IsHandleCreated = True Then
    '                                    Me.Invoke(XMLDelegate, New Object() {APIXML, selPilot.CorpID, selPilot.Name, CorpRepType.CorpSheet})
    '                                End If

    '                            End If
    '                        End If
    '                    End If

    '                End If
    '            End If
    '        Catch e As Exception
    '            Dim msg As String = "An error occured while processing the Corporation Sheet data for " & selPilot.Name & ControlChars.CrLf
    '            msg &= "The specific error was: " & e.Message & ControlChars.CrLf
    '            msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
    '            MessageBox.Show(msg, "GetCorpSheet Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Next
    '    PrismThreadCurrent += 1
    '    If PrismThreadCurrent = PrismThreadMax Then
    '        Call Me.CompleteAPIUpdate()
    '    End If
    'End Sub

#End Region

#Region "APIv2"

    Private Sub GetCharAssets2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = False Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = Owner.Account
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                    ' Check for valid API Usage
                    If CanUseAPI(Owner, CorpRepType.Assets) = True Then

                        ' Make a call to the EveHQ.Core.API to fetch the relevant API
                        Dim Retries As Integer = 0
                        Do
                            Retries += 1
                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, pilotAccount.ToAPIAccount, Owner.ID, EveAPI.APIReturnMethods.ReturnStandard)
                        Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Assets})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Character Assets data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCharBalances2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = False Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = Owner.Account
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                    ' Check for valid API Usage
                    If CanUseAPI(Owner, CorpRepType.Balances) = True Then

                        ' Make a call to the EveHQ.Core.API to fetch the relevant API
                        Dim Retries As Integer = 0
                        Do
                            Retries += 1
                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.AccountBalancesChar, pilotAccount.ToAPIAccount, Owner.ID, EveAPI.APIReturnMethods.ReturnStandard)
                        Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Balances})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Character Balances data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCharJobs2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = False Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = Owner.Account
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                    ' Check for valid API Usage
                    If CanUseAPI(Owner, CorpRepType.Jobs) = True Then

                        ' Make a call to the EveHQ.Core.API to fetch the relevant API
                        Dim Retries As Integer = 0
                        Do
                            Retries += 1
                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.IndustryChar, pilotAccount.ToAPIAccount, Owner.ID, EveAPI.APIReturnMethods.ReturnStandard)
                        Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                        ' Write the installerIDs to the database
                        If APIXML IsNot Nothing Then
                            Call Prism.DataFunctions.WriteInstallerIDsToDB(APIXML)
                            Call Prism.DataFunctions.WriteInventionResultsToDB(APIXML)
                        End If

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Jobs})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Character Jobs data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCharJournal2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = False Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = Owner.Account
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                    ' Check for valid API Usage
                    If CanUseAPI(Owner, CorpRepType.WalletJournal) = True Then

                        ' Get the last referenceID for the wallet
                        Dim LastTrans As Long = DataFunctions.GetLastWalletID(WalletTypes.Journal, CInt(Owner.ID), 1000)

                        ' Start a loop to collect multiple APIs
                        Dim WalletJournals As New SortedList(Of Long, WalletJournalItem)
                        Dim LastRefID As Long = 0
                        Dim WalletExhausted As Boolean = False
                        APIReq = New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                        Do
                            ' Make a call to the EveHQ.Core.API to fetch the journal
                            Dim Retries As Integer = 0
                            Do
                                Retries += 1
                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalChar, pilotAccount.ToAPIAccount, Owner.ID, 1000, LastRefID, MaxAPIJournals, EveAPI.APIReturnMethods.ReturnStandard)
                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                            ' Parse the Journal XML to get the data
                            If APIXML IsNot Nothing Then
                                WalletExhausted = Prism.DataFunctions.ParseWalletJournalXML(APIXML, WalletJournals)
                            Else
                                WalletExhausted = True
                            End If

                            If WalletJournals.Count <> 0 Then
                                LastRefID = WalletJournals.Keys(0)
                            Else
                                WalletExhausted = True
                            End If

                        Loop Until WalletExhausted = True Or LastTrans > LastRefID

                        ' Write the journal to the database!
                        If WalletJournals.Count > 0 Then
                            Call Prism.DataFunctions.WriteWalletJournalToDB(WalletJournals, CInt(Owner.ID), Owner.Name, 1000, LastTrans)
                        End If

                    End If

                    ' Update the display
                    Dim oldXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalChar, pilotAccount.ToAPIAccount, Owner.ID, 1000, 0, MaxAPIJournals, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {oldXML, Owner, CorpRepType.WalletJournal})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Character Journal data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCharOrders2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = False Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = Owner.Account
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                    ' Check for valid API Usage
                    If CanUseAPI(Owner, CorpRepType.Orders) = True Then

                        ' Make a call to the EveHQ.Core.API to fetch the relevant API
                        Dim Retries As Integer = 0
                        Do
                            Retries += 1
                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersChar, pilotAccount.ToAPIAccount, Owner.ID, EveAPI.APIReturnMethods.ReturnStandard)
                        Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Orders})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Character Orders data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCharTransactions2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = False Then

                    ' Setup the array of transactions
                    Dim transNodes As New ArrayList
                    Dim transID As String = ""

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = Owner.Account
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                    ' Check for valid API Usage
                    If CanUseAPI(Owner, CorpRepType.WalletTransactions) = True Then

                        ' Make a call to the EveHQ.Core.API to fetch the transactions
                        Dim Retries As Integer = 0
                        Do
                            Retries += 1
                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletTransChar, pilotAccount.ToAPIAccount, Owner.ID, 1000, transID, EveAPI.APIReturnMethods.ReturnStandard)
                        Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                        ' Write the journal to the database!
                        Call Prism.DataFunctions.WriteWalletTransactionsToDB(APIXML, False, CInt(Owner.ID), Owner.Name, 1000)

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.WalletTransactions})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Character Transactions data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCharContracts2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = False Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = Owner.Account
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                    ' Check for valid API Usage
                    If CanUseAPI(Owner, CorpRepType.Contracts) = True Then

                        ' Make a call to the EveHQ.Core.API to fetch the contracts
                        Dim Retries As Integer = 0
                        Do
                            Retries += 1
                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsChar, pilotAccount.ToAPIAccount, Owner.ID, EveAPI.APIReturnMethods.ReturnStandard)
                        Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                        If APIXML IsNot Nothing Then
                            ' Get the Node List
                            Dim Contracts As XmlNodeList = APIXML.SelectNodes("/eveapi/result/rowset/row")
                            ' Parse the Node List
                            For Each ContractItem As XmlNode In Contracts
                                Dim contractID As Long = CLng(ContractItem.Attributes.GetNamedItem("contractID").Value)
                                Retries = 0
                                Do
                                    Retries += 1
                                    APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractItemsChar, pilotAccount.ToAPIAccount, Owner.ID, contractID, EveAPI.APIReturnMethods.ReturnStandard)
                                Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0
                            Next
                        End If

                    End If

                    ' Update the display
                    APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsChar, pilotAccount.ToAPIAccount, Owner.ID, EveAPI.APIReturnMethods.ReturnStandard)
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Contracts})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Character Contracts data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub UpdateNullCorpSheet2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = False Then

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {Nothing, Owner, CorpRepType.CorpSheet})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Null Corp Sheet data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If
    End Sub

    Private Sub GetCorpAssets2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = True Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Assets)
                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Assets)
                    If pilotAccount IsNot Nothing And OwnerID <> "" Then
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(Owner, CorpRepType.Assets) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim Retries As Integer = 0
                            Do
                                Retries += 1
                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, pilotAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnStandard)
                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                        End If
                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Assets})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Corporate Assets data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCorpBalances2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = True Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Balances)
                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Balances)
                    If pilotAccount IsNot Nothing And OwnerID <> "" Then
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(Owner, CorpRepType.Balances) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim Retries As Integer = 0
                            Do
                                Retries += 1
                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.AccountBalancesCorp, pilotAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnStandard)
                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                        End If
                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Balances})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Corporate Balances data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCorpJobs2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = True Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Jobs)
                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Jobs)
                    If pilotAccount IsNot Nothing And OwnerID <> "" Then
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(Owner, CorpRepType.Jobs) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim Retries As Integer = 0
                            Do
                                Retries += 1
                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.IndustryCorp, pilotAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnStandard)
                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                            ' Write the installerIDs to the database
                            If APIXML IsNot Nothing Then
                                Call Prism.DataFunctions.WriteInstallerIDsToDB(APIXML)
                                Call Prism.DataFunctions.WriteInventionResultsToDB(APIXML)
                            End If

                        End If

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Jobs})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Corporate Jobs data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCorpJournal2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = True Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.WalletJournal)
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.WalletJournal)
                    If pilotAccount IsNot Nothing And OwnerID <> "" Then

                        ' Check for valid API Usage
                        If CanUseAPI(Owner, CorpRepType.WalletJournal) = True Then

                            For divID As Integer = 1006 To 1000 Step -1

                                ' Get the last referenceID for the wallet
                                Dim LastTrans As Long = DataFunctions.GetLastWalletID(WalletTypes.Journal, CInt(Owner.ID), divID)

                                ' Start a loop to collect multiple APIs
                                Dim WalletJournals As New SortedList(Of Long, WalletJournalItem)
                                Dim LastRefID As Long = 0
                                Dim WalletExhausted As Boolean = False
                                APIReq = New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                                Do
                                    ' Make a call to the EveHQ.Core.API to fetch the journal
                                    Dim Retries As Integer = 0
                                    Do
                                        Retries += 1
                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalCorp, pilotAccount.ToAPIAccount, OwnerID, divID, LastRefID, MaxAPIJournals, EveAPI.APIReturnMethods.ReturnStandard)
                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                                    ' Parse the Journal XML to get the data
                                    If APIXML IsNot Nothing Then
                                        WalletExhausted = Prism.DataFunctions.ParseWalletJournalXML(APIXML, WalletJournals)
                                    Else
                                        WalletExhausted = True
                                    End If

                                    If WalletJournals.Count <> 0 Then
                                        LastRefID = WalletJournals.Keys(0)
                                    Else
                                        WalletExhausted = True
                                    End If

                                Loop Until WalletExhausted = True Or LastTrans > LastRefID

                                ' Write the journal to the database!
                                If WalletJournals.Count > 0 Then
                                    Call Prism.DataFunctions.WriteWalletJournalToDB(WalletJournals, CInt(Owner.ID), Owner.Name, divID, LastTrans)
                                End If

                            Next

                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletJournalCorp, pilotAccount.ToAPIAccount, OwnerID, 1000, 0, MaxAPIJournals, EveAPI.APIReturnMethods.ReturnCacheOnly)

                        End If

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.WalletJournal})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Corporate Journal data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCorpOrders2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = True Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Orders)
                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Orders)
                    If pilotAccount IsNot Nothing And OwnerID <> "" Then
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(Owner, CorpRepType.Orders) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim Retries As Integer = 0
                            Do
                                Retries += 1
                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersCorp, pilotAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnStandard)
                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                        End If

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Orders})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Corporate Orders data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCorpTransactions2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = True Then

                    ' Setup the array of transactions
                    Dim transNodes As New ArrayList
                    Dim transID As String = ""

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.WalletTransactions)
                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.WalletTransactions)
                    If pilotAccount IsNot Nothing And OwnerID <> "" Then
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(Owner, CorpRepType.WalletTransactions) = True Then

                            For divID As Integer = 1006 To 1000 Step -1

                                ' Make a call to the EveHQ.Core.API to fetch the transactions
                                Dim Retries As Integer = 0
                                Do
                                    Retries += 1
                                    APIXML = APIReq.GetAPIXML(EveAPI.APITypes.WalletTransCorp, pilotAccount.ToAPIAccount, OwnerID, divID, transID, EveAPI.APIReturnMethods.ReturnStandard)
                                Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                                ' Write the journal to the database!
                                Call Prism.DataFunctions.WriteWalletTransactionsToDB(APIXML, False, CInt(Owner.ID), Owner.Name, divID)

                            Next
                        End If

                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.WalletTransactions})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Corporate Transactions data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCorpContracts2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = True Then

                    Dim APIXML As New XmlDocument
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    Dim pilotAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Contracts)
                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Contracts)
                    If pilotAccount IsNot Nothing And OwnerID <> "" Then

                        ' Check for valid API Usage
                        If CanUseAPI(Owner, CorpRepType.Contracts) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the contracts
                            Dim Retries As Integer = 0
                            Do
                                Retries += 1
                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsCorp, pilotAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnStandard)
                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                            If APIXML IsNot Nothing Then
                                ' Get the Node List
                                Dim Contracts As XmlNodeList = APIXML.SelectNodes("/eveapi/result/rowset/row")
                                ' Parse the Node List
                                For Each ContractItem As XmlNode In Contracts
                                    Dim contractID As Long = CLng(ContractItem.Attributes.GetNamedItem("contractID").Value)
                                    Retries = 0
                                    Do
                                        Retries += 1
                                        APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractItemsCorp, pilotAccount.ToAPIAccount, OwnerID, contractID, EveAPI.APIReturnMethods.ReturnStandard)
                                    Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0
                                Next
                            End If

                            APIXML = APIReq.GetAPIXML(EveAPI.APITypes.ContractsCorp, pilotAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnStandard)

                        End If
                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.Contracts})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Corporate Contracts data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub
    Private Sub GetCorpSheet2(ByVal State As Object)

        For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
            Try
                If Owner.IsCorp = True Then

                    Dim APIXML As New XmlDocument
                    Dim pilotAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.CorpSheet)

                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.CorpSheet)
                    If pilotAccount IsNot Nothing And OwnerID <> "" Then

                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(Owner, CorpRepType.CorpSheet) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim Retries As Integer = 0
                            Do
                                Retries += 1
                                APIXML = APIReq.GetAPIXML(EveAPI.APITypes.CorpSheet, pilotAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnStandard)
                            Loop Until Retries >= MaxAPIRetries Or APIReq.LastAPIError <> 0

                        End If
                    End If

                    ' Update the display
                    If Me.IsHandleCreated = True Then
                        Me.Invoke(XMLDelegate, New Object() {APIXML, Owner, CorpRepType.CorpSheet})
                    End If

                End If
            Catch e As Exception
                Dim msg As String = "An error occured while processing the Corporation Sheet data for " & Owner.Name & ControlChars.CrLf
                msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next
        PrismThreadCurrent += 1
        If PrismThreadCurrent = PrismThreadMax Then
            Call Me.CompleteAPIUpdate()
        End If

    End Sub

#End Region


    Private Function CacheDate(ByVal APIXML As XmlDocument) As DateTime
        ' Get Cache time details
        Dim cacheDetails As XmlNodeList = APIXML.SelectNodes("/eveapi")
        Dim cacheTime As DateTime = CDate(cacheDetails(0).ChildNodes(2).InnerText)
        Dim localCacheTime As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cacheTime)
        Return localCacheTime
    End Function

#End Region

#Region "Outpost XML Retrieval and Parsing"

    Private Sub GetOutposts()

        ' Make a call to the EveHQ.Core.API to fetch the assets
        Dim stationXML As New XmlDocument
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
        stationXML = APIReq.GetAPIXML(EveAPI.APITypes.Conquerables, EveAPI.APIReturnMethods.ReturnStandard)

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

#Region "Reports"

    '#Region "Common Routines"
    '    Private Function HTMLHeader(ByVal browserHeader As String, ByVal forIGB As Boolean) As String

    '        Dim strHTML As String = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""http://www.w3.org/TR/html4/strict.dtd"">"
    '        strHTML &= "<html lang=""" & System.Globalization.CultureInfo.CurrentCulture.ToString & """>"
    '        If forIGB = False Then
    '            strHTML &= "<head>"
    '            strHTML &= "<META http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">"
    '            strHTML &= "<title>" & browserHeader & "</title>" & CharacterCSS() & "</head>"
    '        Else
    '            strHTML &= "<head><title>" & browserHeader & "</title></head>"
    '        End If
    '        strHTML &= "<body>"
    '        strHTML &= "<p align='center'><img src='http://www.evehq.net/images/evehq_igb.png' alt='EveHQ Logo'></p>"
    '        Return strHTML

    '    End Function
    '    Private Function HTMLTitle(ByVal Title As String, ByVal forIGB As Boolean) As String
    '        Dim strHTML As String = ""

    '        If forIGB = False Then
    '            strHTML &= "<table width=800px border=0 align=center>"
    '            strHTML &= "<tr height=30px><td><p class=title>" & Title & "</p></td></tr>"
    '            strHTML &= "</table>"
    '        Else
    '            strHTML &= "<h1>" & Title & "</h1>"
    '        End If
    '        strHTML &= "<p></p>"
    '        Return strHTML
    '    End Function
    '    Private Function HTMLFooter(ByVal forIGB As Boolean) As String
    '        Dim strHTML As String = ""
    '        If forIGB = False Then
    '            strHTML &= "<table width=800px align=center border=0><hr>"
    '        Else
    '            strHTML &= "<table width=800px border=0><hr>"
    '        End If

    '        strHTML &= "<tr><td><p align=center class=footer>Generated on " & Format(Now, "dd/MM/yyyy HH:mm:ss") & " by <a href='http://www.evehq.net'>" & My.Application.Info.ProductName & "</a> v" & My.Application.Info.Version.ToString & "</p></td></tr>"
    '        strHTML &= "</table>"
    '        strHTML &= "</body></html>"

    '        Return strHTML
    '    End Function
    '    Private Function CharacterCSS() As String
    '        Dim strCSS As String = ""
    '        strCSS &= "<STYLE><!--"
    '        strCSS &= "BODY { font-family: Tahoma, Arial; font-size: 12px; bgcolor: #000000; background: #000000 }"
    '        strCSS &= "TD, P { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff }"
    '        strCSS &= ".thead { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff; font-variant: small-caps; background-color: #444444 }"
    '        strCSS &= ".footer { font-family: Tahoma, Arial; font-size: 9px; color: #ffffff; font-variant: small-caps }"
    '        strCSS &= ".title { font-family: Tahoma, Arial; font-size: 20px; color: #ffffff; font-variant: small-caps }"
    '        strCSS &= "--></STYLE>"
    '        Return strCSS
    '    End Function
    '#End Region

    '#Region "Comparers for Multi-dimensional arrays"

    '    Class RectangularComparer
    '        Implements IComparer
    '        ' maintain a reference to the 2-dimensional array being sorted

    '        Private sortArray(,) As Long

    '        ' constructor initializes the sortArray reference
    '        Public Sub New(ByVal theArray(,) As Long)
    '            sortArray = theArray
    '        End Sub

    '        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
    '            ' x and y are integer row numbers into the sortArray
    '            Dim i1 As Long = DirectCast(x, Integer)
    '            Dim i2 As Long = DirectCast(y, Integer)

    '            ' compare the items in the sortArray
    '            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
    '        End Function
    '    End Class

    '    Class ArrayComparerString
    '        Implements IComparer
    '        ' maintain a reference to the 2-dimensional array being sorted

    '        Private sortArray(,) As String

    '        ' constructor initializes the sortArray reference
    '        Public Sub New(ByVal theArray(,) As String)
    '            sortArray = theArray
    '        End Sub

    '        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
    '            ' x and y are integer row numbers into the sortArray
    '            Dim i1 As String = CStr(DirectCast(x, Integer))
    '            Dim i2 As String = CStr(DirectCast(y, Integer))

    '            ' compare the items in the sortArray
    '            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
    '        End Function
    '    End Class

    '    Class ArrayComparerDouble
    '        Implements IComparer
    '        ' maintain a reference to the 2-dimensional array being sorted

    '        Private sortArray(,) As Double

    '        ' constructor initializes the sortArray reference
    '        Public Sub New(ByVal theArray(,) As Double)
    '            sortArray = theArray
    '        End Sub

    '        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
    '            ' x and y are integer row numbers into the sortArray
    '            Dim i1 As Double = CDbl(DirectCast(x, Integer))
    '            Dim i2 As Double = CDbl(DirectCast(y, Integer))

    '            ' compare the items in the sortArray
    '            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
    '        End Function
    '    End Class

    '#End Region

    '#Region "Asset Location Report"

    '    Private Function AssetLocationReport() As String
    '        Dim strHTML As New StringBuilder
    '        Dim TotalValue As Double = 0
    '        Dim LocationValue As Double = 0
    '        Dim GroupValue As Double = 0
    '        strHTML.Append("<table width=800px align=center>")
    '        For Each Loc As Node In adtAssets.Nodes
    '            LocationValue = 0
    '            strHTML.Append("<tr bgcolor=444488><td colspan=6>" & Loc.Text & "</td></tr>")
    '            Dim assets As New SortedList
    '            Dim assetsList As New SortedList
    '            Dim newAsset As New AssetItem
    '            For Each item As Node In Loc.Nodes
    '                If item.Cells(AssetColumn.Group).Text <> "" Then
    '                    If (filters.Count > 0 And catFilters.Contains(item.Cells(AssetColumn.Category).Text) = False And groupFilters.Contains(item.Cells(AssetColumn.Group).Text) = False) Or (searchText <> "" And item.Text.ToLower.Contains(searchText.ToLower) = False) Then
    '                    Else
    '                        If assets.ContainsKey(item.Cells(AssetColumn.Category).Text & "_" & item.Cells(AssetColumn.Group).Text) = True Then
    '                            assetsList = CType(assets.Item(item.Cells(AssetColumn.Category).Text & "_" & item.Cells(AssetColumn.Group).Text), Collections.SortedList)
    '                            assetsList.Add(item.Tag.ToString, item.Tag.ToString)
    '                        Else
    '                            Dim assetList As New SortedList
    '                            assetsList = New SortedList
    '                            assetsList.Add(item.Tag.ToString, item.Tag.ToString)
    '                            assets.Add(item.Cells(AssetColumn.Category).Text & "_" & item.Cells(AssetColumn.Group).Text, assetsList)
    '                        End If
    '                    End If
    '                    If item.Nodes.Count > 0 Then
    '                        Call GetGroupsInNodes(item, assets, assetsList)
    '                    End If
    '                End If
    '            Next
    '            For Each assetGroup As String In assets.Keys
    '                GroupValue = 0
    '                Dim groupings() As String = assetGroup.Split("_".ToCharArray)
    '                strHTML.Append("<tr bgcolor=222244>")
    '                strHTML.Append("<td>" & groupings(0) & " / " & groupings(1) & "</td>")
    '                strHTML.Append("<td align=center>Owner</td>")
    '                strHTML.Append("<td align=center>Location</td>")
    '                strHTML.Append("<td align=center>Quantity</td>")
    '                strHTML.Append("<td align=center>Unit Price</td>")
    '                strHTML.Append("<td align=center>Total Price</td>")
    '                strHTML.Append("</tr>")
    '                assetsList = CType(assets(assetGroup), Collections.SortedList)
    '                For Each asset As String In assetsList.Keys
    '                    newAsset = CType(assetList.Item(asset), AssetItem)
    '                    strHTML.Append("<tr bgcolor=448844>")
    '                    strHTML.Append("<td>" & newAsset.typeName & "</td>")
    '                    strHTML.Append("<td align=center>" & newAsset.owner & "</td>")
    '                    strHTML.Append("<td align=center>" & newAsset.location & "</td>")
    '                    strHTML.Append("<td align=center>" & FormatNumber(newAsset.quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '                    strHTML.Append("<td align=right>" & FormatNumber(Math.Round(newAsset.price, 2), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '                    strHTML.Append("<td align=right>" & FormatNumber(CDbl(newAsset.quantity) * CDbl(Math.Round(newAsset.price, 2)), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '                    strHTML.Append("</tr>")
    '                    GroupValue += CDbl(newAsset.quantity) * CDbl(Math.Round(newAsset.price, 2))
    '                Next
    '                strHTML.Append("<tr bgcolor=000000>")
    '                strHTML.Append("<td></td>")
    '                strHTML.Append("<td colspan=4>TOTAL GROUP VALUE</td>")
    '                strHTML.Append("<td align=right>" & FormatNumber(GroupValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '                LocationValue += GroupValue
    '            Next
    '            strHTML.Append("<tr>")
    '            strHTML.Append("<td bgcolor=000000></td>")
    '            strHTML.Append("<td bgcolor=aa00aa colspan=4>TOTAL LOCATION VALUE</td>")
    '            strHTML.Append("<td bgcolor=aa00aa align=right>" & FormatNumber(LocationValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '            TotalValue += LocationValue
    '            strHTML.Append("<tr bgcolor=000000><td colspan=6>&nbsp;</td></tr>")
    '        Next
    '        strHTML.Append("<tr>")
    '        strHTML.Append("<td></td>")
    '        strHTML.Append("<td  bgcolor=aa6600 colspan=4>TOTAL VALUE</td>")
    '        strHTML.Append("<td  bgcolor=aa6600 align=right>" & FormatNumber(TotalValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '        strHTML.Append("</table>")
    '        Return strHTML.ToString
    '    End Function
    '    Private Sub GetGroupsInNodes(ByVal parent As Node, ByVal assets As SortedList, ByVal assetslist As SortedList)
    '        For Each item As Node In parent.Nodes
    '            If item.Cells(AssetColumn.Group).Text <> "" Then
    '                If (filters.Count > 0 And catFilters.Contains(item.Cells(AssetColumn.Category).Text) = False And groupFilters.Contains(item.Cells(AssetColumn.Group).Text) = False) Or (searchText <> "" And item.Text.ToLower.Contains(searchText.ToLower) = False) Then
    '                Else
    '                    If assets.ContainsKey(item.Cells(AssetColumn.Category).Text & "_" & item.Cells(AssetColumn.Group).Text) = True Then
    '                        assetslist = CType(assets.Item(item.Cells(AssetColumn.Category).Text & "_" & item.Cells(AssetColumn.Group).Text), Collections.SortedList)
    '                        assetslist.Add(item.Tag.ToString, item.Tag.ToString)
    '                    Else
    '                        assetslist = New SortedList
    '                        assetslist.Add(item.Tag.ToString, item.Tag.ToString)
    '                        assets.Add(item.Cells(AssetColumn.Category).Text & "_" & item.Cells(AssetColumn.Group).Text, assetslist)
    '                    End If
    '                End If
    '                If item.Nodes.Count > 0 Then
    '                    Call GetGroupsInNodes(item, assets, assetslist)
    '                End If
    '            End If
    '        Next
    '    End Sub
    '#End Region

    '#Region "Asset List Reports"
    '    Private Function AssetListReportByName() As String
    '        Dim strHTML As New StringBuilder
    '        Dim TotalValue As Double = 0
    '        ' Create a new sortedlist with all the stuff in it
    '        Dim assets As New SortedList
    '        Dim newAsset As New AssetItem
    '        Dim currentValue As Double = 0
    '        For Each asset As String In assetList.Keys
    '            newAsset = CType(assetList(asset), AssetItem)
    '            If assets.ContainsKey(newAsset.typeName) = False Then
    '                assets.Add(newAsset.typeName, 0)
    '            End If
    '            currentValue = CDbl(assets(newAsset.typeName))
    '            currentValue += newAsset.quantity
    '            assets(newAsset.typeName) = currentValue
    '        Next
    '        strHTML.Append("<table width=800px align=center>")
    '        strHTML.Append("<tr bgcolor=222244>")
    '        strHTML.Append("<td>Asset Type</td>")
    '        strHTML.Append("<td align=center>Quantity</td>")
    '        strHTML.Append("<td align=center>Unit Price</td>")
    '        strHTML.Append("<td align=center>Total Price</td>")
    '        strHTML.Append("</tr>")
    '        Dim price As Double = 0
    '        Dim quantity As Double = 0
    '        For Each asset As String In assets.Keys
    '            ' Get price
    '            If asset.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
    '                price = 0
    '            Else
    '                If EveHQ.Core.HQ.itemList.ContainsKey(asset) Then
    '                    price = EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(asset))
    '                Else
    '                    price = 0
    '                End If
    '            End If
    '            price = Math.Round(price, 2)
    '            ' Get quantity
    '            quantity = CDbl(assets(asset))
    '            strHTML.Append("<tr bgcolor=448844>")
    '            strHTML.Append("<td>" & asset & "</td>")
    '            strHTML.Append("<td align=center>" & FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '            strHTML.Append("<td align=right>" & FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '            strHTML.Append("<td align=right>" & FormatNumber(quantity * price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '            strHTML.Append("</tr>")
    '            TotalValue += quantity * price
    '        Next
    '        strHTML.Append("<tr bgcolor=000000>")
    '        strHTML.Append("<td></td>")
    '        strHTML.Append("<td colspan=2>TOTAL ASSET VALUE</td>")
    '        strHTML.Append("<td align=right>" & FormatNumber(TotalValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '        strHTML.Append("</table>")
    '        Return strHTML.ToString
    '    End Function
    '    Private Function AssetListReportByNumeric(ByVal field As Integer, Optional ByVal reverse As Boolean = False) As String
    '        Dim strHTML As New StringBuilder
    '        Dim TotalValue As Double = 0
    '        ' Create a new sortedlist with all the stuff in it
    '        Dim assets As New SortedList
    '        Dim newAsset As New AssetItem
    '        Dim currentValue As Double = 0
    '        For Each asset As String In assetList.Keys
    '            newAsset = CType(assetList(asset), AssetItem)
    '            If assets.ContainsKey(newAsset.typeName) = False Then
    '                assets.Add(newAsset.typeName, 0)
    '            End If
    '            currentValue = CDbl(assets(newAsset.typeName))
    '            currentValue += newAsset.quantity
    '            assets(newAsset.typeName) = currentValue
    '        Next

    '        Dim sortSkill(assets.Count, 1) As Double
    '        Dim count As Integer = 0
    '        For Each asset As String In assets.Keys
    '            sortSkill(count, 0) = count
    '            Dim cPrice As Double = 0
    '            If asset.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
    '                cPrice = 0
    '            Else
    '                If EveHQ.Core.HQ.itemList.ContainsKey(asset) Then
    '                    cPrice = EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(asset))
    '                Else
    '                    cPrice = 0
    '                End If
    '            End If
    '            cPrice = Math.Round(cPrice, 2)
    '            Select Case field
    '                Case 1 ' Quantity
    '                    sortSkill(count, 1) = CDbl(assets(asset))
    '                Case 2 ' Unit Price
    '                    ' Get price
    '                    sortSkill(count, 1) = cPrice
    '                Case 3 ' Total Price
    '                    sortSkill(count, 1) = CDbl(assets(asset)) * cPrice
    '            End Select
    '            count += 1
    '        Next
    '        ' Create a tag array ready to sort the skill times
    '        Dim tagArray(assets.Count - 1) As Integer
    '        For a As Integer = 0 To assets.Count - 1
    '            tagArray(a) = a
    '        Next
    '        ' Initialize the comparer and sort
    '        Dim myComparer As New ArrayComparerDouble(sortSkill)
    '        Array.Sort(tagArray, myComparer)
    '        If reverse = True Then
    '            Array.Reverse(tagArray)
    '        End If

    '        strHTML.Append("<table width=800px align=center>")
    '        strHTML.Append("<tr bgcolor=222244>")
    '        strHTML.Append("<td>Asset Type</td>")
    '        strHTML.Append("<td align=center>Quantity</td>")
    '        strHTML.Append("<td align=center>Unit Price</td>")
    '        strHTML.Append("<td align=center>Total Price</td>")
    '        strHTML.Append("</tr>")
    '        Dim idx As Integer = 0
    '        Dim price As Double = 0
    '        Dim quantity As Double = 0
    '        Dim cAsset As String = ""
    '        For i As Integer = 0 To tagArray.Length - 1
    '            idx = CInt(sortSkill(tagArray(i), 0))
    '            cAsset = CStr(assets.GetKey(idx))
    '            'For Each asset As String In assets.Keys
    '            ' Get price
    '            If cAsset.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
    '                price = 0
    '            Else
    '                If EveHQ.Core.HQ.itemList.ContainsKey(cAsset) Then
    '                    price = EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(cAsset))
    '                Else
    '                    price = 0
    '                End If
    '            End If
    '            price = Math.Round(price, 2)
    '            ' Get quantity
    '            quantity = CDbl(assets(cAsset))
    '            strHTML.Append("<tr bgcolor=448844>")
    '            strHTML.Append("<td>" & cAsset & "</td>")
    '            strHTML.Append("<td align=center>" & FormatNumber(quantity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '            strHTML.Append("<td align=right>" & FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '            strHTML.Append("<td align=right>" & FormatNumber(quantity * price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '            strHTML.Append("</tr>")
    '            TotalValue += quantity * price
    '        Next
    '        strHTML.Append("<tr bgcolor=000000>")
    '        strHTML.Append("<td></td>")
    '        strHTML.Append("<td colspan=2>TOTAL ASSET VALUE</td>")
    '        strHTML.Append("<td align=right>" & FormatNumber(TotalValue, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>")
    '        strHTML.Append("</table>")
    '        Return strHTML.ToString
    '    End Function

    '#End Region

#End Region

#Region "Toolbar Menu Routines"

    'Private Sub mnuLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim strHTML As New StringBuilder
    '    strHTML.Append(HTMLHeader("Assets By Location", False))
    '    strHTML.Append(HTMLTitle("Assets By Location", False))
    '    strHTML.Append(AssetLocationReport())
    '    strHTML.Append(HTMLFooter(False))
    '    Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetLocations.html"))

    '    sw.Write(strHTML)
    '    sw.Flush()
    '    sw.Close()
    '    strHTML = Nothing

    '    If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetLocations.html")) = True Then
    '        Try
    '            Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetLocations.html"))
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try
    '    Else
    '        MessageBox.Show("Unable to locate the Asset Location file, please try again!", "Error Finding File")
    '    End If

    '    ' Tidy up report variables
    '    GC.Collect()
    'End Sub
    'Private Sub mnuAssetListName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim strHTML As New StringBuilder
    '    strHTML.Append(HTMLHeader("Asset List", False))
    '    strHTML.Append(HTMLTitle("Asset List", False))
    '    strHTML.Append(AssetListReportByName())
    '    strHTML.Append(HTMLFooter(False))
    '    Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetList.html"))

    '    sw.Write(strHTML.ToString)
    '    sw.Flush()
    '    sw.Close()
    '    strHTML = Nothing

    '    If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetList.html")) = True Then
    '        Try
    '            Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetList.html"))
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try
    '    Else
    '        MessageBox.Show("Unable to locate the Asset List file, please try again!", "Error Finding File")
    '    End If

    '    ' Tidy up report variables
    '    GC.Collect()
    'End Sub
    'Private Sub mnuAssetListQuantityA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim strHTML As New StringBuilder
    '    strHTML.Append(HTMLHeader("Asset List (By Ascending Quantity)", False))
    '    strHTML.Append(HTMLTitle("Asset List (By Ascending Quantity)", False))
    '    strHTML.Append(AssetListReportByNumeric(1, False))
    '    strHTML.Append(HTMLFooter(False))
    '    Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityA.html"))

    '    sw.Write(strHTML.ToString)
    '    sw.Flush()
    '    sw.Close()
    '    strHTML = Nothing

    '    If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityA.html")) = True Then
    '        Try
    '            Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityA.html"))
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try

    '    Else
    '        MessageBox.Show("Unable to locate the Asset Quantity file, please try again!", "Error Finding File")
    '    End If

    '    ' Tidy up report variables
    '    GC.Collect()
    'End Sub
    'Private Sub mnuAssetListQuantityD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim strHTML As New StringBuilder
    '    strHTML.Append(HTMLHeader("Asset List (By Descending Quantity)", False))
    '    strHTML.Append(HTMLTitle("Asset List (By Descending Quantity)", False))
    '    strHTML.Append(AssetListReportByNumeric(1, True))
    '    strHTML.Append(HTMLFooter(False))
    '    Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityD.html"))

    '    sw.Write(strHTML.ToString)
    '    sw.Flush()
    '    sw.Close()
    '    strHTML = Nothing

    '    If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityD.html")) = True Then
    '        Try
    '            Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListQuantityD.html"))
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try

    '    Else
    '        MessageBox.Show("Unable to locate the Asset Quantity file, please try again!", "Error Finding File")
    '    End If

    '    ' Tidy up report variables
    '    GC.Collect()
    'End Sub
    'Private Sub mnuAssetListPriceA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim strHTML As New StringBuilder
    '    strHTML.Append(HTMLHeader("Asset List (By Ascending Unit Price)", False))
    '    strHTML.Append(HTMLTitle("Asset List (By Ascending Unit Price)", False))
    '    strHTML.Append(AssetListReportByNumeric(2, False))
    '    strHTML.Append(HTMLFooter(False))
    '    Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceA.html"))

    '    sw.Write(strHTML.ToString)
    '    sw.Flush()
    '    sw.Close()
    '    strHTML = Nothing

    '    If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceA.html")) = True Then
    '        Try
    '            Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceA.html"))
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try

    '    Else
    '        MessageBox.Show("Unable to locate the Asset Price file, please try again!", "Error Finding File")
    '    End If

    '    ' Tidy up report variables
    '    GC.Collect()
    'End Sub
    'Private Sub mnuAssetListPriceD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim strHTML As New StringBuilder
    '    strHTML.Append(HTMLHeader("Asset List (By Descending Unit Price)", False))
    '    strHTML.Append(HTMLTitle("Asset List (By Descending Unit Price)", False))
    '    strHTML.Append(AssetListReportByNumeric(2, True))
    '    strHTML.Append(HTMLFooter(False))
    '    Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceD.html"))

    '    sw.Write(strHTML.ToString)
    '    sw.Flush()
    '    sw.Close()
    '    strHTML = Nothing

    '    If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceD.html")) = True Then
    '        Try
    '            Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListPriceD.html"))
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try

    '    Else
    '        MessageBox.Show("Unable to locate the Asset Price file, please try again!", "Error Finding File")
    '    End If

    '    ' Tidy up report variables
    '    GC.Collect()
    'End Sub
    'Private Sub mnuAssetListValueA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim strHTML As New StringBuilder
    '    strHTML.Append(HTMLHeader("Asset List (By Ascending Value)", False))
    '    strHTML.Append(HTMLTitle("Asset List (By Ascending Value)", False))
    '    strHTML.Append(AssetListReportByNumeric(3, False))
    '    strHTML.Append(HTMLFooter(False))
    '    Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueA.html"))

    '    sw.Write(strHTML.ToString)
    '    sw.Flush()
    '    sw.Close()
    '    strHTML = Nothing

    '    If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueA.html")) = True Then
    '        Try
    '            Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueA.html"))
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try

    '    Else
    '        MessageBox.Show("Unable to locate the Asset Value file, please try again!", "Error Finding File")
    '    End If

    '    ' Tidy up report variables
    '    GC.Collect()
    'End Sub
    'Private Sub mnuAssetListValueD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim strHTML As New StringBuilder
    '    strHTML.Append(HTMLHeader("Asset List (By Descending Value)", False))
    '    strHTML.Append(HTMLTitle("Asset List (By Descending Value)", False))
    '    strHTML.Append(AssetListReportByNumeric(3, True))
    '    strHTML.Append(HTMLFooter(False))
    '    Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueD.html"))

    '    sw.Write(strHTML.ToString)
    '    sw.Flush()
    '    sw.Close()
    '    strHTML = Nothing

    '    If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueD.html")) = True Then
    '        Try
    '            Process.Start(Path.Combine(EveHQ.Core.HQ.reportFolder, "AssetListValueD.html"))
    '        Catch ex As Exception
    '            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the html filetype is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try
    '    Else
    '        MessageBox.Show("Unable to locate the Asset Value file, please try again!", "Error Finding File")
    '    End If

    '    ' Tidy up report variables
    '    GC.Collect()
    'End Sub


#End Region

#Region "Market Orders Routines"

    Private Sub cboOrdersOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboOrdersOwner.SelectedIndexChanged
        Call Me.ParseOrders()
    End Sub

    Private Sub ParseOrders()
        ' Get the owner we will use
        Dim Owner As New PrismOwner
        If cboOrdersOwner.SelectedItem IsNot Nothing Then
            If PlugInData.PrismOwners.ContainsKey(cboOrdersOwner.SelectedItem.ToString) Then
                Owner = PlugInData.PrismOwners(cboOrdersOwner.SelectedItem.ToString)
                Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Orders)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Orders)
                Dim OrderXML As New XmlDocument
                Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                If OwnerAccount IsNot Nothing Then

                    If Owner.IsCorp = True Then
                        OrderXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    Else
                        OrderXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    End If
                    Dim sellTotal, buyTotal, TotalEscrow As Double
                    Dim TotalOrders As Integer = 0
                    If OrderXML IsNot Nothing Then

                        Dim Orders As XmlNodeList = OrderXML.SelectNodes("/eveapi/result/rowset/row")
                        adtBuyOrders.BeginUpdate()
                        adtSellOrders.BeginUpdate()
                        adtBuyOrders.Nodes.Clear()
                        adtSellOrders.Nodes.Clear()
                        For Each Order As XmlNode In Orders
                            If Order.Attributes.GetNamedItem("bid").Value = "0" Then
                                If Order.Attributes.GetNamedItem("orderState").Value = "0" Then
                                    Dim sOrder As New Node
                                    For NewCell As Integer = 1 To adtSellOrders.Columns.Count : sOrder.Cells.Add(New Cell) : Next
                                    adtSellOrders.Nodes.Add(sOrder)
                                    Dim itemID As String = Order.Attributes.GetNamedItem("typeID").Value
                                    Dim itemName As String = ""
                                    If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                        itemName = EveHQ.Core.HQ.itemData(itemID).Name
                                    Else
                                        itemName = "Unknown Item ID:" & itemID
                                    End If
                                    sOrder.Text = itemName
                                    Dim quantity As Double = Double.Parse(Order.Attributes.GetNamedItem("volRemaining").Value, culture)
                                    sOrder.Cells(1).Text = FormatNumber(quantity, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                                    Dim price As Double = Double.Parse(Order.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Any, culture)
                                    sOrder.Cells(2).Text = FormatNumber(price, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                                    Dim loc As String = ""
                                    If PlugInData.stations.Contains(Order.Attributes.GetNamedItem("stationID").Value) = True Then
                                        loc = CType(PlugInData.stations(Order.Attributes.GetNamedItem("stationID").Value), Station).stationName
                                    Else
                                        loc = "StationID: " & Order.Attributes.GetNamedItem("stationID").Value
                                    End If
                                    sOrder.Cells(3).Text = loc
                                    Dim issueDate As Date = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                                    Dim orderExpires As TimeSpan = issueDate - Now
                                    orderExpires = orderExpires.Add(New TimeSpan(CInt(Order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                                    sOrder.Cells(4).Tag = orderExpires
                                    If orderExpires.TotalSeconds <= 0 Then
                                        sOrder.Cells(4).Text = "Expired!"
                                    Else
                                        sOrder.Cells(4).Text = EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False)
                                    End If
                                    sellTotal = sellTotal + quantity * price
                                    TotalOrders = TotalOrders + 1
                                End If
                            Else
                                If Order.Attributes.GetNamedItem("orderState").Value = "0" Then
                                    Dim bOrder As New Node
                                    For NewCell As Integer = 1 To adtBuyOrders.Columns.Count : bOrder.Cells.Add(New Cell) : Next
                                    adtBuyOrders.Nodes.Add(bOrder)
                                    Dim itemID As String = Order.Attributes.GetNamedItem("typeID").Value
                                    Dim itemName As String = ""
                                    If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                        itemName = EveHQ.Core.HQ.itemData(itemID).Name
                                    Else
                                        itemName = "Unknown Item ID:" & itemID
                                    End If
                                    bOrder.Text = itemName
                                    Dim quantity As Double = Double.Parse(Order.Attributes.GetNamedItem("volRemaining").Value, culture)
                                    bOrder.Cells(1).Text = FormatNumber(quantity, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & " / " & FormatNumber(CDbl(Order.Attributes.GetNamedItem("volEntered").Value), 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                                    Dim price As Double = Double.Parse(Order.Attributes.GetNamedItem("price").Value, Globalization.NumberStyles.Any, culture)
                                    bOrder.Cells(2).Text = FormatNumber(price, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                                    Dim loc As String = ""
                                    If PlugInData.stations.Contains(Order.Attributes.GetNamedItem("stationID").Value) = True Then
                                        loc = CType(PlugInData.stations(Order.Attributes.GetNamedItem("stationID").Value), Station).stationName
                                    Else
                                        loc = "StationID: " & Order.Attributes.GetNamedItem("stationID").Value
                                    End If
                                    bOrder.Cells(3).Text = loc
                                    bOrder.Cells(4).Tag = CInt(Order.Attributes.GetNamedItem("range").Value)
                                    Select Case CInt(Order.Attributes.GetNamedItem("range").Value)
                                        Case -1
                                            bOrder.Cells(4).Text = "Station"
                                        Case 0
                                            bOrder.Cells(4).Text = "System"
                                        Case 32767
                                            bOrder.Cells(4).Text = "Region"
                                        Case Is > 0, Is < 32767
                                            bOrder.Cells(4).Text = Order.Attributes.GetNamedItem("range").Value & " Jumps"
                                    End Select
                                    bOrder.Cells(5).Text = FormatNumber(Double.Parse(Order.Attributes.GetNamedItem("minVolume").Value, culture), 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                                    Dim issueDate As Date = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                                    Dim orderExpires As TimeSpan = issueDate - Now
                                    orderExpires = orderExpires.Add(New TimeSpan(CInt(Order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                                    bOrder.Cells(6).Tag = orderExpires
                                    If orderExpires.TotalSeconds <= 0 Then
                                        bOrder.Cells(6).Text = "Expired!"
                                    Else
                                        bOrder.Cells(6).Text = EveHQ.Core.SkillFunctions.TimeToString(orderExpires.TotalSeconds, False)
                                    End If
                                    buyTotal = buyTotal + quantity * price
                                    TotalEscrow = TotalEscrow + Double.Parse(Order.Attributes.GetNamedItem("escrow").Value, culture)
                                    TotalOrders = TotalOrders + 1
                                End If
                            End If
                        Next
                        If adtBuyOrders.Nodes.Count = 0 Then
                            adtBuyOrders.Nodes.Add(New Node("No Data Available..."))
                            adtBuyOrders.Enabled = False
                        Else
                            adtBuyOrders.Enabled = True
                        End If
                        If adtSellOrders.Nodes.Count = 0 Then
                            adtSellOrders.Nodes.Add(New Node("No Data Available..."))
                            adtSellOrders.Enabled = False
                        Else
                            adtSellOrders.Enabled = True
                        End If
                        EveHQ.Core.AdvTreeSorter.Sort(adtBuyOrders, 1, True, False)
                        adtBuyOrders.EndUpdate()
                        EveHQ.Core.AdvTreeSorter.Sort(adtSellOrders, 1, True, False)
                        adtSellOrders.EndUpdate()
                    End If

                    If Owner.IsCorp = False Then
                        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(Owner.Name) = True Then
                            Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Owner.Name), Core.Pilot)
                            Dim maxorders As Integer = 5 + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Trade)) * 4) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Tycoon)) * 32) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Retail)) * 8) + (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Wholesale)) * 16)
                            Dim TransTax As Double = 1 * (1 - 0.1 * (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Accounting))))
                            Dim BrokerFee As Double = 1 * (1 - 0.05 * (CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BrokerRelations))))
                            lblAskRange.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Procurement)))
                            lblBidRange.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Marketing)))
                            lblModRange.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Daytrading)))
                            lblRemoteRange.Text = GetOrderRange(CInt(selPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Visibility)))
                            lblOrders.Text = (maxorders - TotalOrders).ToString + " / " + maxorders.ToString
                            lblBrokerFee.Text = BrokerFee.ToString("N2") & "%"
                            lblTransTax.Text = TransTax.ToString("N2") & "%"
                        Else
                            lblAskRange.Text = "n/a"
                            lblBidRange.Text = "n/a"
                            lblModRange.Text = "n/a"
                            lblRemoteRange.Text = "n/a"
                            lblOrders.Text = "n/a"
                            lblBrokerFee.Text = "n/a"
                            lblTransTax.Text = "n/a"
                        End If
                    Else
                        lblAskRange.Text = "n/a"
                        lblBidRange.Text = "n/a"
                        lblModRange.Text = "n/a"
                        lblRemoteRange.Text = "n/a"
                        lblOrders.Text = "n/a"
                        lblBrokerFee.Text = "n/a"
                        lblTransTax.Text = "n/a"
                    End If
                    Dim cover As Double = buyTotal - TotalEscrow
                    lblSellTotal.Text = sellTotal.ToString("N2") & " isk"
                    lblBuyTotal.Text = buyTotal.ToString("N2") & " isk"
                    lblEscrow.Text = TotalEscrow.ToString("N2") & " isk (additional " & cover.ToString("N2") & " isk to cover)"
                Else
                    adtBuyOrders.BeginUpdate()
                    adtBuyOrders.Nodes.Clear()
                    adtBuyOrders.Nodes.Add(New Node("Access Denied - check API Status"))
                    adtBuyOrders.EndUpdate()
                    adtBuyOrders.Enabled = False
                    adtSellOrders.BeginUpdate()
                    adtSellOrders.Nodes.Clear()
                    adtSellOrders.Nodes.Add(New Node("Access Denied - check API Status"))
                    adtSellOrders.EndUpdate()
                    adtSellOrders.Enabled = False
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
            Else
                adtBuyOrders.BeginUpdate()
                adtBuyOrders.Nodes.Clear()
                adtBuyOrders.Nodes.Add(New Node("Access Denied - check API Status"))
                adtBuyOrders.EndUpdate()
                adtBuyOrders.Enabled = False
                adtSellOrders.BeginUpdate()
                adtSellOrders.Nodes.Clear()
                adtSellOrders.Nodes.Add(New Node("Access Denied - check API Status"))
                adtSellOrders.EndUpdate()
                adtSellOrders.Enabled = False
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
        Else
            adtBuyOrders.BeginUpdate()
            adtBuyOrders.Nodes.Clear()
            adtBuyOrders.Nodes.Add(New Node("Access Denied - No valid pilot selected"))
            adtBuyOrders.EndUpdate()
            adtBuyOrders.Enabled = False
            adtSellOrders.BeginUpdate()
            adtSellOrders.Nodes.Clear()
            adtSellOrders.Nodes.Add(New Node("Access Denied - No valid pilot selected"))
            adtSellOrders.EndUpdate()
            adtSellOrders.Enabled = False
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

    Private Sub adtBuyOrders_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, False, False)
    End Sub

    Private Sub adtSellOrders_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, False, False)
    End Sub

#End Region

#Region "Wallet Journal Routines"

    Private Sub InitialiseJournal()
        ' Prepare info
        dtiJournalEndDate.Value = Now
        dtiJournalStartDate.Value = Now.AddMonths(-1)
        cboJournalOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, False, cboJournalOwners)
        AddHandler CType(cboJournalOwners.DropDownControl, PrismSelectionControl).SelectionChanged, AddressOf JournalOwnersChanged
        cboJournalRefTypes.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalRefTypes, True, cboJournalRefTypes)
    End Sub

    Private Sub JournalOwnersChanged()
        ' Update the wallet based on the owner (should be single owner!)
        Call Me.UpdateWalletJournalDivisions()
    End Sub

    Private Sub UpdateJournal()
        cboJournalOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, False, cboJournalOwners)
        cboJournalRefTypes.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalRefTypes, True, cboJournalRefTypes)
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
                desc = "GM Cash Transfer"
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
                desc = "Inheritance paid to " & owner2 & " following the death of " & owner1
            Case 10 'Player Donation 
                desc = owner1 & " deposited cash in " & owner2 & "'s account"
            Case 11 'Corporation Payment
                desc = "Corporation Payment" & misc
            Case 12 'Docking Fee'
                desc = "Docking Fee" & misc
            Case 13 'Office Rental Fee
                desc = "Office Rental Fee paid to" & owner2 & " by " & owner1
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
                Dim itemName As String = ""
                If EveHQ.Core.HQ.itemData.ContainsKey(argName1) = True Then
                    itemName = EveHQ.Core.HQ.itemData(argName1).Name
                Else
                    itemName = "ship"
                End If
                desc = "Insurance paid by EVE Central Bank to " & owner2 & " covering loss of a " & itemName
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
                desc = "Loyalty Store Payment to " & owner2
            Case 27 'Agent Location Services
                desc = "Agent Location Service Fee paid to " & owner2
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
                desc = "Jump Clone Installation Fee"
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
                desc = owner1 & " bid on an auction (Contract ID:" & argName1 & ")"
            Case 64 'Contract Auction Bid Refund
                desc = owner2 & " received a refund on a contract aution bid (Contract ID:" & argName1 & ")"
            Case 65 'Contract Collateral
                desc = misc
            Case 66 'Contract Reward Refund
                desc = misc
            Case 67 'Contract Auction Sold
                desc = "Price for Contract auction sold"
            Case 68 'Contract Reward
                desc = "Contract Reward (Contract ID: " & argName1 & ")"
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
                desc = owner1 & " bid on a contract auction for the corp (Contract ID:" & argName1 & ")"
            Case 78 'Contract Collateral Deposited (corp)
                desc = misc
            Case 79 'Contract Price Payment (corp)
                desc = owner1 & " accepted a contract from " & owner2 & " (Contract ID: " & argName1 & ")"
            Case 80 'Contract Brokers Fee (corp)
                desc = "Corporation Contract Brokers Fee (Contract ID: " & argName1 & ")"
            Case 81 'Contract Deposit (corp)
                desc = "Corporation Contract Deposit (Contract ID: " & argName1 & ")"
            Case 82 'Contract Deposit Refund
                desc = "Contract Deposit Refund (Contract ID: " & argName1 & ")"
            Case 83 'Contract Reward Deposited
                desc = "Contract Reward Deposited (Contract ID: " & argName1 & ")"
            Case 84 'Contract Reward Deposited (corp)
                desc = "Contract Reward Deposited (Contract ID: " & argName1 & ")"
            Case 85 ' Bounty owner1=concord
                desc = owner2 & " Got bounty prizes for killing pirates in " & argName1
            Case 86 'Advertisement Listing Fee
                desc = "Corporate Advert Fee (authorised by " & argName1 & ")"
            Case 87 ' Medal Creation Fee
                desc = "Medal Creation Fee (authorised by " & argName1 & ")"
            Case 88 ' Medal Issue Fee
                desc = "Medal Issue Fee (authorised by " & argName1 & ")"
            Case 89 ' Betting Fee
                desc = misc
            Case 90 ' DNA Modification Fee
                desc = misc
            Case 91 ' Sovereignty Bill
                desc = misc
            Case 92 ' Bounty Prize Corporation Tax
                desc = "Bounty Prize Corporation Tax"
            Case 93 ' Agent Mission Reward Corporation Tax
                desc = "Agent Mission Reward Corporation Tax"
            Case 94 ' Agent Mission Time Bonus Reward Corporation Tax
                desc = "Agent Mission Time Bonus Reward Corporation Tax"
            Case 95 ' Upkeep Adjustment Fee
                desc = misc
            Case 96 ' Planetary Import Tax
                desc = "Planetary Import Tax at " & argName1
            Case 97 ' Planetray Export Tax
                desc = "Planetary Export Tax at " & argName1
            Case 98 ' Planetary Construction Fee
                desc = "Planetary Construction Fee at " & argName1
            Case 99 ' Corporate Reward Payout
                desc = misc
            Case 100 ' Minigame Betting Fee
                desc = misc
            Case 101 ' Bounty Surcharge
                desc = misc
            Case 102 ' Contract Reversal
                desc = misc
            Case 103 ' Corporate Reward Tax
                desc = misc
            Case 104 ' Minigame Buy-in Fee
                desc = misc
            Case 105 ' Office Upgrade Fee
                desc = misc
            Case 106 ' Store Purchase
                desc = misc
            Case 107 ' Store Purchase Refund
                desc = misc
            Case 108 ' PLEX sold for Aurum
                desc = misc
        End Select
        Return desc
    End Function

    Private Sub UpdateWalletJournalDivisions()

        If cboJournalOwners.Text <> "" Then
            Dim Owner As New PrismOwner

            If PlugInData.PrismOwners.ContainsKey(cboJournalOwners.Text) = True Then

                cboWalletJournalDivision.BeginUpdate()
                cboWalletJournalDivision.Items.Clear()

                Owner = PlugInData.PrismOwners(cboJournalOwners.Text)
                Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.CorpSheet)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.CorpSheet)

                If OwnerAccount IsNot Nothing Then

                    If Owner.IsCorp = True Then
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                        Dim corpXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.CorpSheet, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        If corpXML IsNot Nothing Then
                            Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                            If errlist.Count = 0 Then
                                ' No errors so parse the files
                                Dim divList As XmlNodeList = corpXML.SelectNodes("/eveapi/result/rowset")
                                For Each div As XmlNode In divList
                                    Select Case div.Attributes.GetNamedItem("name").Value
                                        Case "walletDivisions"
                                            For Each divName As XmlNode In div.ChildNodes
                                                cboWalletJournalDivision.Items.Add(divName.Attributes.GetNamedItem("description").Value)
                                            Next
                                    End Select
                                Next
                            Else
                                For div As Integer = 1000 To 1006
                                    cboWalletJournalDivision.Items.Add(div.ToString.Trim)
                                Next
                            End If
                        End If
                    Else
                        cboWalletJournalDivision.Items.Add("1000")
                    End If
                Else
                    If Owner.IsCorp Then
                        For div As Integer = 1000 To 1006
                            cboWalletJournalDivision.Items.Add(div.ToString.Trim)
                        Next
                    Else
                        cboWalletJournalDivision.Items.Add("1000")
                    End If
                End If
                cboWalletJournalDivision.Enabled = True
                cboWalletJournalDivision.EndUpdate()
                cboWalletJournalDivision.SelectedIndex = 0
            Else
                cboWalletJournalDivision.Enabled = False
            End If

        End If

    End Sub

    Private Sub DisplayWalletJournalEntries()

        ' Fetch the appropriate data
        Dim WalletData As DataSet = FetchWalletJournalData()

        adtJournal.BeginUpdate()
        adtJournal.Nodes.Clear()
        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then
                Dim TransItem As New Node
                Dim transDate As Date
                Dim transA, transB As Double
                Dim refType As String = ""
                Dim RunningBalance As Double = 0
                For Each JE As DataRow In WalletData.Tables(0).Rows
                    TransItem = New Node
                    transDate = DateTime.Parse(JE.Item("transDate").ToString)
                    TransItem.Text = FormatDateTime(transDate, DateFormat.GeneralDate)

                    refType = JE.Item("refTypeID").ToString
                    TransItem.Cells.Add(New Cell(PlugInData.RefTypes(refType)))
                    transA = Double.Parse(JE.Item("amount").ToString)
                    transB = Double.Parse(JE.Item("balance").ToString)
                    RunningBalance += transA

                    If transA < 0 Then
                        TransItem.Cells.Add(New Cell(transA.ToString("N2"), StyleRedRight))
                    Else
                        TransItem.Cells.Add(New Cell(transA.ToString("N2"), StyleGreenRight))

                    End If

                    If sbShowEveBalance.Value = True Then
                        TransItem.Cells.Add(New Cell(transB.ToString("N2"), StyleRight))
                    Else
                        TransItem.Cells.Add(New Cell(RunningBalance.ToString("N2"), StyleRight))
                    End If

                    If JE.Item("reason").ToString <> "" Then
                        TransItem.Cells.Add(New Cell("[r] " & BuildJournalDescription(CInt(refType), JE.Item("ownerName1").ToString, JE.Item("ownerName2").ToString, JE.Item("argID1").ToString, JE.Item("argName1").ToString)))
                        Dim transReason As New Node
                        transReason.Cells.Add(New Cell) : transReason.Cells.Add(New Cell) : transReason.Cells.Add(New Cell)
                        transReason.Cells.Add(New Cell(JE.Item("reason").ToString))
                        TransItem.Nodes.Add(transReason)
                    Else
                        If IsDBNull(JE.Item("typeID")) = False Then
                            ' Put some market data here
                            Dim Item As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(JE.Item("typeID").ToString)
                            TransItem.Cells.Add(New Cell("[t] " & BuildJournalDescription(CInt(refType), JE.Item("ownerName1").ToString, JE.Item("ownerName2").ToString, JE.Item("argID1").ToString, JE.Item("argName1").ToString)))
                            Dim transReason As New Node
                            transReason.Cells.Add(New Cell) : transReason.Cells.Add(New Cell) : transReason.Cells.Add(New Cell)
                            transReason.Cells.Add(New Cell(Item.Name))
                            TransItem.Nodes.Add(transReason)
                        Else
                            TransItem.Cells.Add(New Cell(BuildJournalDescription(CInt(refType), JE.Item("ownerName1").ToString, JE.Item("ownerName2").ToString, JE.Item("argID1").ToString, JE.Item("argName1").ToString)))
                        End If
                    End If

                    adtJournal.Nodes.Add(TransItem)
                Next
                adtJournal.Enabled = True
            Else
                adtJournal.Nodes.Add(New Node("No Data Available..."))
                adtJournal.Enabled = False
            End If
        Else
            adtJournal.Nodes.Add(New Node("No Data Available..."))
            adtJournal.Enabled = False
        End If
        'EveHQ.Core.AdvTreeSorter.Sort(adtJournal, New EveHQ.Core.AdvTreeSortResult(1, Core.AdvTreeSortOrder.Descending), False)
        adtJournal.EndUpdate()
    End Sub

    Private Function FetchWalletJournalData() As DataSet
        Dim walletID As String = (1000 + cboWalletJournalDivision.SelectedIndex).ToString
        Dim strSQL As String = "SELECT * FROM walletJournal"
        strSQL &= " LEFT JOIN walletTransactions ON walletJournal.argName1 = CONVERT(varchar(20), walletTransactions.transRef)"
        strSQL &= " WHERE (walletJournal.walletID = " & walletID & ")"
        strSQL &= " AND walletJournal.transDate >= '" & dtiJournalStartDate.Value.ToString(IndustryTimeFormat, culture) & "' AND walletJournal.transDate <= '" & dtiJournalEndDate.Value.ToString(IndustryTimeFormat, culture) & "'"

        ' Build the Owners List
        If cboJournalOwners.Text <> "<All>" Then
            Dim OwnerList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboJournalOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                OwnerList.Append(", '" & LVI.Name.Replace("'", "''") & "'")
            Next
            If OwnerList.Length > 2 Then
                OwnerList.Remove(0, 2)
            End If
            ' Default to None
            strSQL &= " AND walletJournal.charName IN (" & OwnerList.ToString & ")"
        End If

        ' Build the refTypes List
        If cboJournalRefTypes.Text <> "All" Then
            ' Build a ref type list
            Dim RefTypeList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboJournalRefTypes.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                RefTypeList.Append(", " & LVI.Name)
            Next
            If RefTypeList.Length > 2 Then
                RefTypeList.Remove(0, 2)
                ' Default to All
                strSQL &= " AND walletJournal.refTypeID IN (" & RefTypeList.ToString & ")"
            End If
        End If

        ' Order the data
        strSQL &= " ORDER BY walletJournal.transKey DESC;"

        ' Fetch the data
        Dim WalletData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)

        Return WalletData

    End Function

    Private Function FetchWalletJournalDataForExport() As DataSet
        Dim walletID As String = (1000 + cboWalletJournalDivision.SelectedIndex).ToString
        Dim strSQL As String = "SELECT * FROM walletJournal"
        strSQL &= " WHERE (walletJournal.walletID = " & walletID & ")"
        strSQL &= " AND walletJournal.transDate >= '" & dtiJournalStartDate.Value.ToString(IndustryTimeFormat, culture) & "' AND walletJournal.transDate <= '" & dtiJournalEndDate.Value.ToString(IndustryTimeFormat, culture) & "'"

        ' Build the Owners List
        If cboJournalOwners.Text <> "<All>" Then
            Dim OwnerList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboJournalOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                OwnerList.Append(", '" & LVI.Name.Replace("'", "''") & "'")
            Next
            If OwnerList.Length > 2 Then
                OwnerList.Remove(0, 2)
            End If
            ' Default to None
            strSQL &= " AND walletJournal.charName IN (" & OwnerList.ToString & ")"
        End If

        '' Build the refTypes List
        'If cboJournalRefTypes.Text <> "All" Then
        '    ' Build a ref type list
        '    Dim RefTypeList As New StringBuilder
        '    For Each LVI As ListViewItem In CType(cboJournalRefTypes.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
        '        RefTypeList.Append(", " & LVI.Name)
        '    Next
        '    If RefTypeList.Length > 2 Then
        '        RefTypeList.Remove(0, 2)
        '        ' Default to All
        '        strSQL &= " AND walletJournal.refTypeID IN (" & RefTypeList.ToString & ")"
        '    End If
        'End If

        ' Order the data
        strSQL &= " ORDER BY walletJournal.transKey DESC;"

        ' Fetch the data
        Dim WalletData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)

        Return WalletData

    End Function

    Private Sub btnJournalQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJournalQuery.Click
        Call Me.DisplayWalletJournalEntries()
    End Sub

    Private Sub sbShowEveBalance_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles sbShowEveBalance.ValueChanged
        If cboWalletJournalDivision.SelectedItem IsNot Nothing Then
            Call Me.DisplayWalletJournalEntries()
        End If
    End Sub

    Private Sub dtiJournalStartDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiJournalStartDate.ButtonCustom2Click
        dtiJournalStartDate.Value = New Date(dtiJournalStartDate.Value.Year, dtiJournalStartDate.Value.Month, dtiJournalStartDate.Value.Day)
    End Sub

    Private Sub dtiJournalStartDate_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiJournalStartDate.ButtonCustomClick
        dtiJournalStartDate.Value = Now
    End Sub

    Private Sub dtiJournalEndDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiJournalEndDate.ButtonCustom2Click
        dtiJournalEndDate.Value = New Date(dtiJournalEndDate.Value.Year, dtiJournalEndDate.Value.Month, dtiJournalEndDate.Value.Day)
    End Sub

    Private Sub dtiJournalEndDate_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiJournalEndDate.ButtonCustomClick
        dtiJournalEndDate.Value = Now
    End Sub

    Private Sub btnResetJournal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetJournal.Click
        Dim reply As DialogResult = MessageBox.Show("Are you really sure you want to delete all the journal entries from the database?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            Dim strSQL As String = "DELETE * FROM walletJournal;"
            If EveHQ.Core.DataFunctions.SetData(strSQL) = True Then
                MessageBox.Show("Reset Complete")
            End If
            strSQL = "DROP TABLE walletJournal;"
            If EveHQ.Core.DataFunctions.SetData(strSQL) = True Then
                MessageBox.Show("Table Deletion Complete")
            End If
            Call Prism.DataFunctions.CheckDatabaseTables()
        End If
    End Sub

    Private Sub btnCheckJournalOmissions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckJournalOmissions.Click
        Dim CheckJournals As New frmJournalCheck
        CheckJournals.ShowDialog()
        CheckJournals.Dispose()
    End Sub

    Private Sub btnExportEntries_Click(sender As System.Object, e As System.EventArgs) Handles btnExportEntries.Click

        ' Check for multiple owners - can't do this!

        If CType(cboJournalOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems.Count <> 1 Then
            MessageBox.Show("You can only export data for a single owner at a time. Please adjust the Journal Owners accordingly.", "Wallet Journal Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

            Dim sfd As New SaveFileDialog
            sfd.Title = "Export Wallet Journal Entries"
            sfd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Dim filterText As String = "XML files (*.xml)|*.xml"
            sfd.Filter = filterText
            sfd.FilterIndex = 0
            sfd.AddExtension = True
            sfd.ShowDialog()
            sfd.CheckPathExists = True
            If sfd.FileName <> "" Then

                ' Fetch the appropriate data
                Dim WalletData As DataSet = FetchWalletJournalDataForExport()

                Dim xmlDoc As New XmlDocument
                Dim dec As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", Nothing, Nothing)
                xmlDoc.AppendChild(dec)

                ' Create XML root
                Dim xmlRoot As XmlElement = xmlDoc.CreateElement("EveHQWalletJournalExport")
                xmlDoc.AppendChild(xmlRoot)

                ' Create heading for future reference & import
                Dim HeaderRow As XmlElement = xmlDoc.CreateElement("exportedData")
                Dim HeaderAtt As XmlAttribute = xmlDoc.CreateAttribute("ownerName")
                HeaderAtt.Value = System.Web.HttpUtility.HtmlEncode(cboJournalOwners.Text)
                HeaderRow.Attributes.Append(HeaderAtt)
                HeaderAtt = xmlDoc.CreateAttribute("walletID")
                HeaderAtt.Value = System.Web.HttpUtility.HtmlEncode((1000 + cboWalletJournalDivision.SelectedIndex).ToString)
                HeaderRow.Attributes.Append(HeaderAtt)
                HeaderAtt = xmlDoc.CreateAttribute("startDate")
                HeaderAtt.Value = System.Web.HttpUtility.HtmlEncode(dtiJournalStartDate.Value.ToString(IndustryTimeFormat, culture))
                HeaderRow.Attributes.Append(HeaderAtt)
                HeaderAtt = xmlDoc.CreateAttribute("endDate")
                HeaderAtt.Value = System.Web.HttpUtility.HtmlEncode(dtiJournalEndDate.Value.ToString(IndustryTimeFormat, culture))
                HeaderRow.Attributes.Append(HeaderAtt)
                ' Add the header row
                xmlRoot.AppendChild(HeaderRow)

                ' Create main XML data
                For Each row As DataRow In WalletData.Tables(0).Rows
                    Dim XMLRow As XmlElement = xmlDoc.CreateElement("row")
                    For Each col As DataColumn In WalletData.Tables(0).Columns
                        Dim XMLAtt As XmlAttribute = xmlDoc.CreateAttribute(col.ColumnName)
                        If col.DataType() Is GetType(System.Double) Then
                            XMLAtt.Value = System.Web.HttpUtility.HtmlEncode(CDbl(row.Item(col)).ToString(culture))
                        Else
                            If col.DataType() Is GetType(System.DateTime) Then
                                XMLAtt.Value = System.Web.HttpUtility.HtmlEncode(CDate(row.Item(col)).ToString(IndustryTimeFormat, culture))
                            Else
                                XMLAtt.Value = System.Web.HttpUtility.HtmlEncode(row.Item(col).ToString)
                            End If
                        End If
                        XMLRow.Attributes.Append(XMLAtt)
                    Next
                    xmlRoot.AppendChild(XMLRow)
                Next

                ' Save the XML file
                xmlDoc.Save(sfd.FileName)

            End If

            sfd.Dispose()

            MessageBox.Show("Export of Wallet Journal Data completed!", "Wallet Journal Export", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub btnImportEntries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportEntries.Click

        ' Step 1: Ask for a filename
        Dim ofd As New OpenFileDialog
        ofd.Title = "Import Wallet Journal Entries"
        ofd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        Dim filterText As String = "XML files (*.xml)|*.xml"
        ofd.Filter = filterText
        ofd.FilterIndex = 0
        ofd.AddExtension = True
        ofd.CheckPathExists = True
        ofd.ShowDialog()

        ' Step 2: Check the file exists and is of the right format
        Dim xmlDoc As New XmlDocument
        Dim OwnerID As String = ""
        Dim OwnerName As String = ""
        Dim WalletID As Integer = 0
        Dim StartDate As Date
        Dim EndDate As Date

        If ofd.FileName <> "" Then
            ' Check existence
            If My.Computer.FileSystem.FileExists(ofd.FileName) = True Then
                ' Load the file into an XML document
                Try
                    xmlDoc.Load(ofd.FileName)
                    ' Assume in XML format so check for some key information, starting with the root node
                    Dim RootNodes As XmlNodeList = xmlDoc.GetElementsByTagName("EveHQWalletJournalExport")
                    If RootNodes.Count = 1 Then
                        ' Check for the exportedData node
                        Dim ConfigNodes As XmlNodeList = xmlDoc.GetElementsByTagName("exportedData")
                        If ConfigNodes.Count = 1 Then
                            ' Check the node attributes
                            Dim ConfigNode As XmlNode = ConfigNodes(0)
                            If ConfigNode.Attributes.Count = 5 Then
                                Try
                                    OwnerID = ConfigNode.Attributes.GetNamedItem("ownerID").Value
                                    OwnerName = ConfigNode.Attributes.GetNamedItem("ownerName").Value
                                    WalletID = CInt(ConfigNode.Attributes.GetNamedItem("walletID").Value)
                                    StartDate = DateTime.ParseExact(ConfigNode.Attributes.GetNamedItem("startDate").Value, IndustryTimeFormat, culture)
                                    EndDate = DateTime.ParseExact(ConfigNode.Attributes.GetNamedItem("endDate").Value, IndustryTimeFormat, culture)
                                    If OwnerID = "" Then
                                        MessageBox.Show("The import configuration data for OwnerID cannot be blank. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        Exit Sub
                                    End If
                                    If OwnerName = "" Then
                                        MessageBox.Show("The import configuration data for OwnerName cannot be blank. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        Exit Sub
                                    End If
                                    If WalletID < 1000 Or WalletID > 1006 Then
                                        MessageBox.Show("The import configuration data for WalletID must be between 1000 and 1006 inclusive. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        Exit Sub
                                    End If
                                    ' Seems to be right at this point!
                                Catch ex As Exception
                                    MessageBox.Show("The import configuration data could not be imported correctly. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Exit Sub
                                End Try
                            Else
                                MessageBox.Show("The configuration node contains the incorrect number of attributes. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If
                        Else
                            MessageBox.Show("The XML file contains invalid configuration nodes. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                    Else
                        MessageBox.Show("The XML file contains invalid root nodes. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                Catch ex As Exception
                    MessageBox.Show("There was an error loading the XML file. Please check the file is in XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End Try
            Else
                MessageBox.Show("Cannot find the selected file. Please try again.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Else
            MessageBox.Show("Import cancelled by user.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Step 3: Ask for confirmation (because it potentially involves deleting stuff)
        Dim msg As New StringBuilder
        msg.AppendLine("This procedure will first delete all wallet transactions in WalletID " & WalletID.ToString & " for " & OwnerName & " between the dates of " & StartDate.ToString(IndustryTimeFormat) & " and " & EndDate.ToString(IndustryTimeFormat) & ".")
        msg.AppendLine("")
        msg.AppendLine("Are you sure you wish to proceed?")
        Dim reply As DialogResult = MessageBox.Show(msg.ToString, "Confirm Wallet Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        ' Step 4: Delete existing transactions
        Dim strSQL As String = "DELETE FROM walletJournal"
        strSQL &= " WHERE (walletJournal.walletID = " & WalletID & ")"
        strSQL &= " AND walletJournal.transDate >= '" & StartDate.ToString(IndustryTimeFormat, culture) & "' AND walletJournal.transDate < '" & EndDate.ToString(IndustryTimeFormat, culture) & "'"
        strSQL &= " AND walletJournal.charName IN ('" & OwnerName.Replace("'", "''") & "')"
        Try
            EveHQ.Core.DataFunctions.SetData(strSQL)
        Catch ex As Exception
            MessageBox.Show("There was an error removing existing transactions. The error was: " & ex.Message, "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End Try

        ' Step 5: Import new transactions
        Dim WalletJournals As New SortedList(Of Long, WalletJournalItem)
        Prism.DataFunctions.ParseWalletJournalExportXML(xmlDoc, WalletJournals)
        Prism.DataFunctions.WriteWalletJournalToDB(WalletJournals, CInt(OwnerID), OwnerName, WalletID, 0)

        ' Step 6: Tidy up
        ofd.Dispose()

        MessageBox.Show("Import of Wallet Journal Data completed!", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub adtJournal_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtJournal.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, False, False)
    End Sub

#End Region

#Region "Wallet Transaction Routines"

    Private Sub InitialiseTransactions()
        ' Prepare info
        dtiTransEndDate.Value = Now
        dtiTransStartDate.Value = Now.AddMonths(-1)
        cboTransactionOwner.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionOwnersAll, False, cboTransactionOwner)
        AddHandler CType(cboTransactionOwner.DropDownControl, PrismSelectionControl).SelectionChanged, AddressOf TransactionOwnersChanged
        cboWalletTransItem.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionItems, True, cboWalletTransItem)
    End Sub

    Private Sub TransactionOwnersChanged()
        ' Update the wallet based on the owner (should be single owner!)
        Call Me.UpdateWalletTransactionDivisions()
        cboWalletTransItem.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionItems, True, cboWalletTransItem, cboTransactionOwner.Text)
    End Sub

    Private Sub UpdateTransactions()
        cboTransactionOwner.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, False, cboTransactionOwner)
        cboWalletTransItem.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionItems, True, cboWalletTransItem)
    End Sub

    Private Sub DisplayWalletTransactions()

        If cboWalletTransType.SelectedIndex = -1 Then cboWalletTransType.SelectedIndex = 0

        Dim walletID As String = (1000 + cboWalletTransDivision.SelectedIndex).ToString
        Dim strSQL As String = "SELECT * FROM walletTransactions"
        strSQL &= " WHERE (walletTransactions.walletID = " & walletID & ")"
        strSQL &= " AND walletTransactions.transDate >= '" & dtiTransStartDate.Value.ToString(IndustryTimeFormat, culture) & "' AND walletTransactions.transDate <= '" & dtiTransEndDate.Value.ToString(IndustryTimeFormat, culture) & "'"

        ' Build the Owners List
        If cboJournalOwners.Text <> "<All>" Then
            Dim OwnerList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboTransactionOwner.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                OwnerList.Append(", '" & LVI.Name.Replace("'", "''") & "'")
            Next
            If OwnerList.Length > 2 Then
                OwnerList.Remove(0, 2)
            End If
            ' Default to None
            strSQL &= " AND walletTransactions.charName IN (" & OwnerList.ToString & ")"
        End If

        ' Filter transaction type
        Select Case cboWalletTransType.SelectedIndex
            Case 0
                ' Don't do anything as we'll pick up all transactions
            Case 1
                ' Buy transactions only
                strSQL &= " AND walletTransactions.transType = 'buy' "
            Case 2
                ' See transactions only
                strSQL &= " AND walletTransactions.transType = 'sell' "
        End Select

        ' Filter item type
        If cboWalletTransItem.Text <> "All" Then
            ' Build a ref type list
            Dim ItemTypeList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboWalletTransItem.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                ItemTypeList.Append(", '" & LVI.Name.Replace("'", "''") & "'")
            Next
            If ItemTypeList.Length > 2 Then
                ItemTypeList.Remove(0, 2)
                strSQL &= " AND walletTransactions.typeName IN (" & ItemTypeList.ToString & ")"
            End If
        End If

        ' Order the data
        strSQL &= " ORDER BY walletTransactions.transKey DESC;"

        ' Fetch the data
        Dim WalletData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)

        ' Determine if this is personal, or corp, or unknown if an old owner
        Dim IsPersonal As Boolean = False
        Dim IsCorp As Boolean = False
        ' Check for personal
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboTransactionOwner.Text) = True Then
            IsPersonal = True
        Else
            ' Check for corp
            If PlugInData.CorpList.ContainsKey(cboTransactionOwner.Text) = True Then
                IsCorp = True
            End If
        End If

        Dim BuyValue As Double = 0
        Dim SellValue As Double = 0
        Dim Profit As Double = 0

        adtTransactions.BeginUpdate()
        adtTransactions.Nodes.Clear()
        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then

                Dim TransItem As New Node
                Dim transDate As Date
                Dim price, qty, value As Double
                Dim RunningBalance As Double = 0
                For Each JE As DataRow In WalletData.Tables(0).Rows
                    If (JE.Item("transFor").ToString = "personal" And IsPersonal = True) Or IsCorp = True Or (IsPersonal = False And IsCorp = False) Then
                        TransItem = New Node
                        transDate = DateTime.Parse(JE.Item("transDate").ToString)
                        TransItem.Text = FormatDateTime(transDate, DateFormat.GeneralDate)
                        TransItem.Cells.Add(New Cell(JE.Item("typeName").ToString))

                        price = Double.Parse(JE.Item("price").ToString)
                        qty = Double.Parse(JE.Item("quantity").ToString)

                        TransItem.Cells.Add(New Cell(qty.ToString("N0"), StyleRight))
                        TransItem.Cells.Add(New Cell(price.ToString("N2"), StyleRight))
                        If JE.Item("transType").ToString = "sell" Then
                            value = price * qty
                            TransItem.Cells.Add(New Cell(value.ToString("N2"), StyleGreenRight))
                            SellValue += value
                        Else
                            value = -price * qty
                            TransItem.Cells.Add(New Cell(value.ToString("N2"), StyleRedRight))
                            BuyValue += -value
                        End If

                        TransItem.Cells.Add(New Cell(JE.Item("stationName").ToString))
                        TransItem.Cells.Add(New Cell(JE.Item("clientName").ToString))

                        adtTransactions.Nodes.Add(TransItem)
                    End If
                Next
                adtTransactions.Enabled = True
            Else
                adtTransactions.Nodes.Add(New Node("No Data Available..."))
                adtTransactions.Enabled = False
            End If
        Else
            adtTransactions.Nodes.Add(New Node("No Data Available..."))
            adtTransactions.Enabled = False
        End If
        'EveHQ.Core.AdvTreeSorter.Sort(adtTransactions, New EveHQ.Core.AdvTreeSortResult(1, Core.AdvTreeSortOrder.Descending), False)
        adtTransactions.EndUpdate()

        ' Display Figures:
        lblTransBuyValue.Text = "Buy Value: " & BuyValue.ToString("N2")
        lblTransSellValue.Text = "Sell Value: " & SellValue.ToString("N2")
        lblTransProfitValue.Text = "Profit Value: " & (SellValue - BuyValue).ToString("N2")
        Dim GP As Double = 0
        Dim MU As Double = 0
        If SellValue <> 0 Then
            GP = (SellValue - BuyValue) / SellValue * 100
        End If
        If BuyValue <> 0 Then
            MU = (SellValue - BuyValue) / BuyValue * 100
        End If
        lblTransProfitRatio.Text = "Profit Ratios: GP%: " & GP.ToString("N2") & "%, MU: " & MU.ToString("N2") & "%"

    End Sub

    Private Sub UpdateWalletTransactionDivisions()

        If cboTransactionOwner.Text <> "" Then
            Dim Owner As New PrismOwner

            If PlugInData.PrismOwners.ContainsKey(cboTransactionOwner.Text) = True Then

                cboWalletTransDivision.BeginUpdate()
                cboWalletTransDivision.Items.Clear()

                Owner = PlugInData.PrismOwners(cboTransactionOwner.Text)
                Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.CorpSheet)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.CorpSheet)

                If OwnerAccount IsNot Nothing Then

                    If Owner.IsCorp = True Then
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                        Dim corpXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.CorpSheet, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        If corpXML IsNot Nothing Then
                            Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                            If errlist.Count = 0 Then
                                ' No errors so parse the files
                                Dim divList As XmlNodeList = corpXML.SelectNodes("/eveapi/result/rowset")
                                For Each div As XmlNode In divList
                                    Select Case div.Attributes.GetNamedItem("name").Value
                                        Case "walletDivisions"
                                            For Each divName As XmlNode In div.ChildNodes
                                                cboWalletTransDivision.Items.Add(divName.Attributes.GetNamedItem("description").Value)
                                            Next
                                    End Select
                                Next
                            Else
                                For div As Integer = 1000 To 1006
                                    cboWalletTransDivision.Items.Add(div.ToString.Trim)
                                Next
                            End If
                        End If
                    Else
                        cboWalletTransDivision.Items.Add("1000")
                    End If
                Else
                    If Owner.IsCorp Then
                        For div As Integer = 1000 To 1006
                            cboWalletTransDivision.Items.Add(div.ToString.Trim)
                        Next
                    Else
                        cboWalletTransDivision.Items.Add("1000")
                    End If
                End If
                cboWalletTransDivision.Enabled = True
                cboWalletTransDivision.EndUpdate()
                cboWalletTransDivision.SelectedIndex = 0
            Else
                cboWalletTransDivision.Enabled = False
            End If

        End If
    End Sub

    Private Sub cboTransactionOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call Me.UpdateWalletTransactionDivisions()
    End Sub

    Private Sub btnGetTransactions_Click(sender As System.Object, e As System.EventArgs) Handles btnGetTransactions.Click
        Call Me.DisplayWalletTransactions()
    End Sub

    Private Sub dtiTransStartDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiTransStartDate.ButtonCustom2Click
        dtiTransStartDate.Value = New Date(dtiTransStartDate.Value.Year, dtiTransStartDate.Value.Month, dtiTransStartDate.Value.Day)
    End Sub

    Private Sub dtiTransStartDate_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiTransStartDate.ButtonCustomClick
        dtiTransStartDate.Value = Now
    End Sub

    Private Sub dtiTransEndDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiTransEndDate.ButtonCustom2Click
        dtiTransEndDate.Value = New Date(dtiTransEndDate.Value.Year, dtiTransEndDate.Value.Month, dtiTransEndDate.Value.Day)
    End Sub

    Private Sub dtiTransEndDate_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiTransEndDate.ButtonCustomClick
        dtiTransEndDate.Value = Now
    End Sub

    Private Sub adtTransactions_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtTransactions.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, False, False)
    End Sub

#End Region

#Region "Industry Jobs Routines"

    Private Sub cboJobOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboJobOwner.SelectedIndexChanged
        Call Me.DisplayIndustryJobs()
    End Sub

    Private Sub DisplayIndustryJobs()

        ' Get the owner we will use
        If cboJobOwner.SelectedItem IsNot Nothing Then
            Dim owner As String = cboJobOwner.SelectedItem.ToString

            adtJobs.BeginUpdate()
            adtJobs.Nodes.Clear()

            Dim JobList As List(Of IndustryJob) = IndustryJob.ParseIndustryJobs(owner)

            If JobList IsNot Nothing Then

                ' Get InstallerIDs from the database and return list
                Dim InstallerList As SortedList(Of Long, String) = IndustryJob.GetInstallerList(JobList)

                ' Initialise the installer filter
                cboInstallerFilter.Tag = True
                Dim oldFilter As String = ""
                If cboInstallerFilter.SelectedItem IsNot Nothing Then
                    oldFilter = cboInstallerFilter.SelectedItem.ToString
                End If
                cboInstallerFilter.Items.Clear()
                cboInstallerFilter.BeginUpdate()
                cboInstallerFilter.Items.Add("<All Installers>")
                For Each installerName As String In InstallerList.Values
                    cboInstallerFilter.Items.Add(installerName)
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
                Dim transItem As New Node
                Dim transTypeID As String = ""
                Dim locationID As String = ""
                Dim completed As String = ""
                Dim DisplayJob As Boolean = False

                For Each Job As IndustryJob In JobList

                    ' Check filters to see if the job is allowed
                    DisplayJob = False
                    ' Check installer filter
                    If cboInstallerFilter.SelectedIndex = 0 Or (cboInstallerFilter.SelectedIndex > 0 And InstallerList(Job.InstallerID) = cboInstallerFilter.SelectedItem.ToString) Then
                        ' Check activity filter
                        If cboActivityFilter.SelectedIndex = 0 Or Job.ActivityID.ToString = cboActivityFilter.SelectedItem.ToString Then
                            ' Check status filter
                            If cboStatusFilter.SelectedIndex = 0 Then
                                DisplayJob = True
                            Else
                                Select Case Job.Completed
                                    Case 0
                                        If Job.EndProductionTime < DateTime.Now.ToUniversalTime Then
                                            ' Job finished but not delivered
                                            If cboStatusFilter.SelectedItem.ToString = PlugInData.Statuses("B") Then
                                                DisplayJob = True
                                            End If
                                        Else
                                            ' Job in progress
                                            If cboStatusFilter.SelectedItem.ToString = PlugInData.Statuses("A") Then
                                                DisplayJob = True
                                            End If
                                        End If
                                    Case Else
                                        If cboStatusFilter.SelectedItem.ToString = PlugInData.Statuses(Job.CompletedStatus.ToString) Then
                                            DisplayJob = True
                                        End If
                                End Select
                            End If
                        End If
                    End If

                    ' Display the job if applicable
                    If DisplayJob = True Then
                        transItem = New Node
                        For NewCell As Integer = 1 To adtJobs.Columns.Count : transItem.Cells.Add(New Cell) : Next
                        transTypeID = CStr(Job.InstalledItemTypeID)
                        If EveHQ.Core.HQ.itemData.ContainsKey(transTypeID) = True Then
                            transItem.Text = EveHQ.Core.HQ.itemData(transTypeID).Name
                        Else
                            transItem.Text = "Unknown Item ID:" & transTypeID
                        End If
                        adtJobs.Nodes.Add(transItem)
                        transItem.Cells(1).Text = Job.ActivityID.ToString
                        transItem.Cells(2).Text = Job.Runs.ToString
                        transItem.Cells(3).Text = InstallerList(Job.InstallerID)
                        locationID = Job.InstalledItemLocationID.ToString
                        If PlugInData.stations.ContainsKey(locationID) = True Then
                            transItem.Cells(4).Text = CType(PlugInData.stations(locationID), Station).stationName
                        Else
                            If PlugInData.stations.ContainsKey(Job.OutputLocationID.ToString) = True Then
                                transItem.Cells(4).Text = CType(PlugInData.stations(Job.OutputLocationID.ToString), Station).stationName
                            Else
                                transItem.Cells(4).Text = "POS in " & CType(PlugInData.stations(Job.InstalledInSolarSystemID.ToString), SolarSystem).Name
                            End If
                        End If
                        transItem.Cells(5).Text = FormatDateTime(Job.EndProductionTime, DateFormat.GeneralDate)
                        If Job.Completed = 0 Then
                            If Job.EndProductionTime < DateTime.Now.ToUniversalTime Then
                                transItem.Cells(6).Text = PlugInData.Statuses("B")
                            Else
                                transItem.Cells(6).Text = PlugInData.Statuses("A")
                            End If
                        Else
                            transItem.Cells(6).Text = PlugInData.Statuses(Job.CompletedStatus.ToString)
                        End If
                    End If
                Next
            End If
            If adtJobs.Nodes.Count = 0 Then
                adtJobs.Nodes.Add(New Node("No Data Available..."))
                adtJobs.Enabled = False
            Else
                adtJobs.Enabled = True
            End If
            adtJobs.EndUpdate()
        Else
            If adtJobs.Nodes.Count = 0 Then
                adtJobs.Nodes.Add(New Node("No Data Available..."))
                adtJobs.Enabled = False
            Else
                adtJobs.Enabled = True
            End If
            adtJobs.EndUpdate()
        End If
    End Sub

    Private Sub cboInstallerFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboInstallerFilter.SelectedIndexChanged
        If CBool(cboInstallerFilter.Tag) = False Then
            ' We are not triggering a change in the selected item from the main drawing routine
            Call DisplayIndustryJobs()
        End If
    End Sub

    Private Sub cboActivityFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboActivityFilter.SelectedIndexChanged
        If startup = False Then
            ' We are not triggering a change in the selected item from the main drawing routine
            Call DisplayIndustryJobs()
        End If
    End Sub

    Private Sub cboStatusFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStatusFilter.SelectedIndexChanged
        If startup = False Then
            ' We are not triggering a change in the selected item from the main drawing routine
            Call DisplayIndustryJobs()
        End If
    End Sub

#End Region

#Region "Contracts Routines"

    Private Sub cboContractOwner_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboContractOwner.SelectedIndexChanged
        Call Me.DisplayContracts()
    End Sub

    Private Sub DisplayContracts()

        adtContracts.BeginUpdate()
        adtContracts.Nodes.Clear()

        ' Get the owner we will use
        If cboContractOwner.SelectedItem IsNot Nothing Then
            Dim owner As String = cboContractOwner.SelectedItem.ToString
            Dim ContractList As SortedList(Of Long, Contract) = Contracts.ParseContracts(owner)

            If ContractList IsNot Nothing Then
                For Each C As Contract In ContractList.Values
                    ' Setup filter result
                    Dim DisplayContract As Boolean = True
                    ' Apply filtering...

                    ' Display the result if allowed by filters
                    If DisplayContract = True Then
                        Dim NewContract As New Node
                        For NewCell As Integer = 1 To adtContracts.Columns.Count : NewContract.Cells.Add(New Cell) : Next
                        adtContracts.Nodes.Add(NewContract)
                        NewContract.Name = C.ContractID.ToString
                        If C.Title <> "" Then
                            NewContract.Text = C.Title
                        Else
                            If C.Items.Count = 1 Then
                                NewContract.Text = EveHQ.Core.HQ.itemData(C.Items.Keys(0)).Name
                            Else
                                NewContract.Text = "Contract ID: " & C.ContractID.ToString
                            End If
                        End If
                        NewContract.Cells(1).Text = EveHQ.Core.HQ.Stations(C.StartStationID.ToString).stationName
                        If C.IsIssuer = True Then
                            NewContract.Cells(2).Text = "Issued"
                        Else
                            NewContract.Cells(2).Text = "Accepted"
                        End If
                        NewContract.Cells(3).Text = C.Type.ToString
                        NewContract.Cells(4).Text = C.Status.ToString
                        NewContract.Cells(5).Text = C.DateIssued.ToString
                        NewContract.Cells(6).Text = C.DateExpired.ToString
                        NewContract.Cells(7).Text = C.Price.ToString("N2")
                        NewContract.Cells(8).Text = C.Volume.ToString("N2")

                        ' Add items
                        If C.Items.Count > 0 Then
                            Dim ItemCH As New DevComponents.AdvTree.ColumnHeader("Item Name")
                            ItemCH.Width.Absolute = 300
                            ItemCH.DisplayIndex = 1
                            NewContract.NodesColumns.Add(ItemCH)
                            Dim QtyCH As New DevComponents.AdvTree.ColumnHeader("Quantity")
                            QtyCH.Width.Absolute = 100
                            QtyCH.DisplayIndex = 2
                            NewContract.NodesColumns.Add(QtyCH)
                            For Each TypeID As String In C.Items.Keys
                                Dim ItemNode As New Node
                                ItemNode.Name = TypeID
                                ItemNode.Text = EveHQ.Core.HQ.itemData(TypeID).Name
                                ItemNode.Cells.Add(New Cell(C.Items(TypeID).ToString))
                                NewContract.Nodes.Add(ItemNode)
                            Next

                        End If
                    End If

                Next
            End If

            ' Check if there are any nodes
            If adtContracts.Nodes.Count = 0 Then
                adtContracts.Nodes.Add(New Node("No Contract Data Available..."))
                adtContracts.Enabled = False
            Else
                adtContracts.Enabled = True
            End If

        Else

            If adtContracts.Nodes.Count = 0 Then
                adtContracts.Nodes.Add(New Node("No Contract Data Available..."))
                adtContracts.Enabled = False
            Else
                adtContracts.Enabled = True
            End If

        End If

        EveHQ.Core.AdvTreeSorter.Sort(adtContracts, 1, True, False)
        adtContracts.EndUpdate()

    End Sub

    Private Sub adtContracts_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtContracts.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, False, False)
    End Sub

#End Region

#Region "Recycler Routines"

    Public Sub RecycleInfoFromAssets()
        ' Fetch the recycling info from the assets control
        RecyclerAssetList = PAC.RecyclingAssetList
        RecyclerAssetOwner = cboRecyclePilots.SelectedItem.ToString
        RecyclerAssetLocation = GetLocationID(PAC.RecyclingAssetLocation)
        Call LoadRecyclingInfo()
        tabPrism.SelectedTab = tiRecycler
        tiRecycler.Visible = True
    End Sub
    Private Function GetLocationID(ByVal item As Node) As String
        Do While item.Level > 0
            item = item.Parent
        Loop
        Return item.Tag.ToString
    End Function
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
                    lblBaseYield.Text = FormatNumber(StationYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                Else
                    If PlugInData.NPCCorps.ContainsKey(aLocation.corpID.ToString) = True Then
                        lblCorp.Text = CStr(PlugInData.Corps(aLocation.corpID.ToString))
                        lblBaseYield.Text = FormatNumber(aLocation.refiningEff * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                    Else
                        lblCorp.Text = "Unknown"
                        lblCorp.Tag = Nothing
                        lblBaseYield.Text = FormatNumber(50, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                    End If
                End If
            Else ' Is a system
                lblStation.Text = "n/a"
                lblCorp.Text = "n/a"
                lblCorp.Tag = Nothing
                lblBaseYield.Text = FormatNumber(50, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            End If
        Else
            lblStation.Text = "n/a"
            lblCorp.Text = "n/a"
            lblCorp.Tag = Nothing
            lblBaseYield.Text = FormatNumber(50, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
        End If

        ' Set the pilot to the recycling one
        If cboRecyclePilots.Items.Contains(RecyclerAssetOwner) Then
            cboRecyclePilots.SelectedItem = RecyclerAssetOwner
        Else

            If cboRecyclePilots.Items.Count > 0 Then
                cboRecyclePilots.SelectedIndex = 0
            End If
        End If

        ' Set the recycling mode
        cboRefineMode.SelectedIndex = 0
    End Sub
    Private Sub RecalcRecycling()
        ' Create the main list
        adtRecycle.BeginUpdate()
        adtRecycle.Nodes.Clear()
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
        Dim newCLVItem As New Node
        Dim newCLVSubItem As New Node
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
            newCLVItem = New Node
            For NewCell As Integer = 1 To adtRecycle.Columns.Count : newCLVItem.Cells.Add(New Cell) : Next
            newCLVItem.Text = itemInfo.Name
            newCLVItem.Tag = itemInfo.ID
            adtRecycle.Nodes.Add(newCLVItem)
            price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(asset), 2)
            batches = CInt(Int(CLng(RecyclerAssetList(itemInfo.ID.ToString)) / itemInfo.PortionSize))
            quantity = CLng(RecyclerAssetList(asset))
            volume += itemInfo.Volume * quantity
            items += CLng(quantity)
            value = price * quantity
            fees = Math.Round(value * (RTotalFees / 100), 2)
            sale = value - fees
            newCLVItem.Cells(1).Text = FormatNumber(itemInfo.MetaLevel, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            newCLVItem.Cells(2).Text = FormatNumber(quantity, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            newCLVItem.Cells(3).Text = FormatNumber(batches, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            newCLVItem.Cells(4).Text = FormatNumber(price, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            newCLVItem.Cells(5).Text = FormatNumber(value, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            If chkFeesOnItems.Checked = True Then
                newCLVItem.Cells(6).Text = FormatNumber(fees, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                newCLVItem.Cells(7).Text = FormatNumber(sale, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            Else
                newCLVItem.Cells(7).Text = FormatNumber(value, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
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
                    newCLVSubItem = New Node
                    For NewCell As Integer = 1 To adtRecycle.Columns.Count : newCLVSubItem.Cells.Add(New Cell) : Next
                    newCLVSubItem.Text = mat
                    newCLVItem.Nodes.Add(newCLVSubItem)
                    newCLVSubItem.Cells(2).Text = FormatNumber(quant, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                    newCLVSubItem.Cells(3).Text = FormatNumber(quant, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                    newCLVSubItem.Cells(4).Text = FormatNumber(price, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                    newCLVSubItem.Cells(5).Text = FormatNumber(value, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                    If chkFeesOnRefine.Checked = True Then
                        newCLVSubItem.Cells(6).Text = FormatNumber(fees, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                        newCLVSubItem.Cells(8).Text = FormatNumber(sale, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                        recycleTotal += sale
                    Else
                        newCLVSubItem.Cells(8).Text = newCLVSubItem.Cells(5).Text
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
            newCLVItem.Cells(8).Text = FormatNumber(recycleTotal, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            If CDbl(newCLVItem.Cells(8).Text) > CDbl(newCLVItem.Cells(7).Text) Then
                newCLVItem.Style = adtRecycle.Styles("ItemGood")
                newCLVItem.Cells(9).Text = newCLVItem.Cells(8).Text
            Else
                newCLVItem.Cells(9).Text = newCLVItem.Cells(7).Text
            End If
            benefit = CDbl(newCLVItem.Cells(8).Text) - CDbl(newCLVItem.Cells(7).Text)
            newCLVItem.Cells(10).Tag = benefit
            newCLVItem.Cells(10).Text = FormatNumber(benefit, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            newCLVItem.Cells(11).Tag = (benefit / quantity)
            newCLVItem.Cells(11).Text = FormatNumber(benefit / quantity, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            salePriceTotal += CDbl(newCLVItem.Cells(7).Text)
            refinePriceTotal += CDbl(newCLVItem.Cells(8).Text)
            bestPriceTotal += CDbl(newCLVItem.Cells(9).Text)
        Next
        lblPriceTotals.Text = "Sale / Refine / Best Totals: " & FormatNumber(salePriceTotal, 2) & " / " & FormatNumber(refinePriceTotal, 2) & " / " & FormatNumber(bestPriceTotal, 2)
        EveHQ.Core.AdvTreeSorter.Sort(adtRecycle, 1, True, True)
        adtRecycle.EndUpdate()
        lblVolume.Text = FormatNumber(volume, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & " m³"
        lblItems.Text = FormatNumber(adtRecycle.Nodes.Count, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
        lblItems.Text &= " (" & FormatNumber(items, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & ")"
        ' Create the totals list
        adtTotals.BeginUpdate()
        adtTotals.Nodes.Clear()
        If RecycleResults IsNot Nothing Then
            For Each mat As String In RecycleResults.Keys
                price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(mat)), 2)
                wastage = CLng(RecycleWaste(mat))
                taken = CLng(RecycleTake(mat))
                quant = CLng(RecycleResults(mat))
                newCLVItem = New Node
                For NewCell As Integer = 1 To adtTotals.Columns.Count : newCLVItem.Cells.Add(New Cell) : Next
                newCLVItem.Text = mat
                adtTotals.Nodes.Add(newCLVItem)
                newCLVItem.Cells(1).Text = FormatNumber(taken, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                newCLVItem.Cells(2).Text = FormatNumber(wastage, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                newCLVItem.Cells(3).Text = FormatNumber(quant, 0, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                newCLVItem.Cells(4).Text = FormatNumber(price, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                newCLVItem.Cells(5).Text = FormatNumber(price * quant, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            Next
        End If
        EveHQ.Core.AdvTreeSorter.Sort(adtTotals, 1, True, True)
        adtTotals.EndUpdate()
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRecyclePilots.SelectedIndexChanged
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
        If chkPerfectRefine.Checked = True Then
            NetYield = 1
        Else
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
        End If
        lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
        lblNetYield.Text = FormatNumber(NetYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
        If lblCorp.Tag IsNot Nothing Then
            StationStanding = EveHQ.Core.Standings.GetStanding(rPilot.Name, lblCorp.Tag.ToString, True)
        Else
            StationStanding = 0
        End If
        ' Update Standings
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
        Else
            If lblCorp.Tag Is Nothing Then
                lblStandings.Text = FormatNumber(0, 4, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            Else
                lblStandings.Text = FormatNumber(StationStanding, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
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
        lblTotalFees.Text = FormatNumber(RTotalFees, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
        Call Me.RecalcRecycling()
    End Sub

    Private Sub chkFeesOnRefine_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFeesOnRefine.CheckedChanged
        Call Me.RecalcRecycling()
    End Sub

    Private Sub chkFeesOnItems_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFeesOnItems.CheckedChanged
        Call Me.RecalcRecycling()
    End Sub

    Private Sub adtRecycle_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtRecycle.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub adtTotals_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtTotals.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

#Region "Override Base Yield functions"
    Private Sub chkOverrideBaseYield_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideBaseYield.CheckedChanged
        If chkOverrideBaseYield.Checked = True Then
            BaseYield = CDbl(nudBaseYield.Value) / 100
        Else
            BaseYield = StationYield
        End If
        If cboRecyclePilots.SelectedItem IsNot Nothing Then
            Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
            lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
            lblNetYield.Text = FormatNumber(NetYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
            Call Me.RecalcRecycling()
        End If
    End Sub

    Private Sub nudBaseYield_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudBaseYield.ValueChanged
        If chkOverrideBaseYield.Checked = True Then
            BaseYield = CDbl(nudBaseYield.Value) / 100
            lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
            Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboRecyclePilots.SelectedItem.ToString), Core.Pilot)
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
            lblNetYield.Text = FormatNumber(NetYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
            Call Me.RecalcRecycling()
        End If
    End Sub
#End Region

#Region "Override Standings functions"
    Private Sub chkOverrideStandings_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideStandings.CheckedChanged
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
        Else
            If lblCorp.Tag Is Nothing Then
                lblStandings.Text = FormatNumber(0, 4, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            Else
                lblStandings.Text = FormatNumber(StationStanding, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
            End If
        End If
        Call Me.RecalcRecycling()
    End Sub

    Private Sub lblStandings_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStandings.TextChanged
        StationTake = Math.Max(5 - (0.75 * CDbl(lblStandings.Text)), 0)
        lblStationTake.Text = FormatNumber(StationTake, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
    End Sub

    Private Sub nudStandings_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudStandings.ValueChanged
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
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
        lblNetYield.Text = FormatNumber(NetYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
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
        lblTotalFees.Text = FormatNumber(RTotalFees, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
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
        lblTotalFees.Text = FormatNumber(RTotalFees, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
        Call Me.RecalcRecycling()
    End Sub
    Private Sub nudBrokerFee_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudBrokerFee.ValueChanged
        If chkOverrideBrokerFee.Checked = True Then
            RBrokerFee = nudBrokerFee.Value
            RTotalFees = RBrokerFee + RTransTax
            lblTotalFees.Text = FormatNumber(RTotalFees, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
            Call Me.RecalcRecycling()
        End If
    End Sub
    Private Sub nudTax_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTax.ValueChanged
        If chkOverrideTax.Checked = True Then
            RTransTax = nudTax.Value
            RTotalFees = RBrokerFee + RTransTax
            lblTotalFees.Text = FormatNumber(RTotalFees, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
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
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
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
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
                If chkOverrideStandings.Checked = True Then
                    lblStandings.Text = FormatNumber(nudStandings.Value, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                Else
                    If lblCorp.Tag Is Nothing Then
                        lblStandings.Text = FormatNumber(0, 4, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                    Else
                        lblStandings.Text = FormatNumber(StationStanding, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
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
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
                lblStandings.Text = FormatNumber(10, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
                chkOverrideBaseYield.Enabled = False
                chkOverrideStandings.Enabled = False
                chkPerfectRefine.Enabled = False
                nudBaseYield.Enabled = False
                nudStandings.Enabled = False
                cboRecyclePilots.Enabled = False
            Case 2 ' Intensive Refining Array
                BaseYield = 0.75
                NetYield = 0.75
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault) & "%"
                lblStandings.Text = FormatNumber(10, 2, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.UseDefault)
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
        If adtRecycle.SelectedNodes.Count > 0 Then
            Dim newQ As New frmSelectQuantity(CInt(RecyclerAssetList(adtRecycle.SelectedNodes(0).Tag.ToString)))
            newQ.ShowDialog()
            RecyclerAssetList(adtRecycle.SelectedNodes(0).Tag.ToString) = newQ.Quantity
            newQ.Dispose()
            Call Me.RecalcRecycling()
        End If
    End Sub

    Private Sub mnuRemoveRecycleItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveRecycleItem.Click
        If adtRecycle.SelectedNodes.Count > 0 Then
            RecyclerAssetList.Remove(adtRecycle.SelectedNodes(0).Tag.ToString)
            Call Me.RecalcRecycling()
        End If
    End Sub

    Private Sub mnuExportToCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportToCSV.Click
        Call Me.ExportToClipboard("PRISM Item Recycling Analysis", adtRecycle, EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
    End Sub

    Private Sub mnuExportToTSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportToTSV.Click
        Call Me.ExportToClipboard("PRISM Item Recycling Analysis", adtRecycle, ControlChars.Tab)
    End Sub

    Private Sub mnuExportTotalsToCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportTotalsToCSV.Click
        Call Me.ExportToClipboard("PRISM Item Recycling Totals", adtTotals, EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
    End Sub

    Private Sub mnuExportTotalsToTSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportTotalsToTSV.Click
        Call Me.ExportToClipboard("PRISM Item Recycling Totals", adtTotals, ControlChars.Tab)
    End Sub

    Private Sub ExportToClipboard(ByVal title As String, ByVal sourceList As AdvTree, ByVal sepChar As String)
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
        For Each req As Node In sourceList.Nodes
            For c As Integer = 0 To sourceList.Columns.Count - 2
                str.Append(req.Cells(c).Text & sepChar)
            Next
            str.AppendLine(req.Cells(sourceList.Columns.Count - 1).Text)
        Next
        ' Copy to the clipboard
        Try
            Clipboard.SetText(str.ToString)
        Catch ex As Exception
            MessageBox.Show("Unable to copy Resource Data to the clipboard.", "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub ctxRecycleItem_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles ctxRecycleItem.Opening
        If adtRecycle.SelectedNodes.Count > 0 Then
            mnuAlterRecycleQuantity.Enabled = True
            mnuRemoveRecycleItem.Enabled = True
        Else
            mnuAlterRecycleQuantity.Enabled = False
            mnuRemoveRecycleItem.Enabled = False
        End If
    End Sub

    Private Sub mnuAddRecycleItem_Click_1(sender As System.Object, e As System.EventArgs) Handles mnuAddRecycleItem.Click
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
        Call Me.GenerateCSVFileFromCLV(cboTransactionOwner.Text, "Wallet Transactions", adtTransactions)
    End Sub

    Private Sub btnExportJournal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportJournal.Click
        'TODO: Update the called routine
        'Call Me.GenerateCSVFileFromCLV("Wallet Journal", clvJournal)
    End Sub

    Private Sub btnExportJobs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportJobs.Click
        Call Me.GenerateCSVFileFromCLV(cboJobOwner.SelectedItem.ToString, "Industry Jobs", adtJobs)
    End Sub

    Private Sub btnExportOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call Me.GenerateCSVFileFromCLV(cboOrdersOwner.SelectedItem.ToString, "Sell Orders", adtSellOrders)
        Call Me.GenerateCSVFileFromCLV(cboOrdersOwner.SelectedItem.ToString, "Buy Orders", adtBuyOrders)
    End Sub

    Private Sub GenerateCSVFileFromCLV(ByVal OwnerName As String, ByVal Description As String, ByVal cAdvTree As AdvTree)

        Try
            Dim csvFile As String = Path.Combine(EveHQ.Core.HQ.reportFolder, Description.Replace(" ", "") & " - " & OwnerName & " (" & Format(Now, "yyyy-MM-dd HH-mm-ss") & ").csv")
            Dim csvText As New StringBuilder
            With cAdvTree
                ' Write the columns
                For col As Integer = 0 To .Columns.Count - 1
                    csvText.Append(.Columns(col).Text)
                    If col <> .Columns.Count - 1 Then
                        csvText.Append(EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                    End If
                Next
                csvText.AppendLine("")
                ' Write the data
                For Each row As Node In .Nodes
                    For col As Integer = 0 To .Columns.Count - 1
                        If IsNumeric(row.Cells(col).Text) = True Then
                            csvText.Append(CDbl(row.Cells(col).Text).ToString)
                        Else
                            csvText.Append("""" & row.Cells(col).Text & """")
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
    End Sub

#End Region

#Region "BPManager Routines"

    Private Sub btnBPCalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBPCalc.Click
        If adtBlueprints.SelectedNodes.Count = 1 Then
            Dim BPName As String = adtBlueprints.SelectedNodes(0).Text
            If chkShowOwnedBPs.Checked = True Then
                ' Start an owned BPCalc
                If adtBlueprints.SelectedNodes(0).Tag IsNot Nothing Then
                    Dim BPID As Long = CLng(adtBlueprints.SelectedNodes(0).Tag)
                    Dim BPCalc As New frmBPCalculator(EveHQ.Prism.Settings.PrismSettings.DefaultBPOwner, BPID)
                    Call OpenBPCalculator(BPCalc)
                End If
            Else
                ' Start a standard BPCalc
                Dim BPCalc As New frmBPCalculator(BPName)
                Call OpenBPCalculator(BPCalc)
            End If
        ElseIf adtBlueprints.SelectedNodes.Count = 0 Then
            ' Start a blank BP Calc
            Dim BPCalc As New frmBPCalculator(chkShowOwnedBPs.Checked)
            Call OpenBPCalculator(BPCalc)
        End If
    End Sub

    Private Sub cboBPOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBPOwner.SelectedIndexChanged

        ' Check for filter changes, but set the flag to avoid invoking other changes at this point
        BPManagerUpdate = True

        If cboTechFilter.SelectedIndex = -1 Then cboTechFilter.SelectedIndex = 0
        If cboTypeFilter.SelectedIndex = -1 Then cboTypeFilter.SelectedIndex = 0
        If cboCategoryFilter.SelectedIndex = -1 Then cboCategoryFilter.SelectedIndex = 0

        BPManagerUpdate = False

        Call Me.UpdateBPList()
    End Sub

    Private Sub chkShowOwnedBPs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowOwnedBPs.CheckedChanged
        Call Me.UpdateBPList()
    End Sub

    Private Sub UpdateBPList()
        ' Check if we are showing the full list or the owners list
        If chkShowOwnedBPs.Checked = False Then
            Dim search As String = txtBPSearch.Text
            ' Show the full BP list
            adtBlueprints.BeginUpdate()
            adtBlueprints.Nodes.Clear()
            Dim matchCat As Boolean = False
            For Each BP As Blueprint In PlugInData.Blueprints.Values
                If cboTechFilter.SelectedIndex = 0 Or (cboTechFilter.SelectedIndex = BP.TechLevel) Then
                    matchCat = False
                    If cboCategoryFilter.SelectedIndex = 0 Then
                        matchCat = True
                    Else
                        If PlugInData.CategoryNames.ContainsKey(cboCategoryFilter.SelectedItem.ToString) Then
                            If EveHQ.Core.HQ.itemData.ContainsKey(BP.ProductID.ToString) Then
                                If PlugInData.CategoryNames(cboCategoryFilter.SelectedItem.ToString) = CStr(EveHQ.Core.HQ.itemData(BP.ProductID.ToString).Category) Then
                                    matchCat = True
                                End If
                            End If
                        End If
                    End If
                    If matchCat = True Then
                        If search = "" Or BP.Name.ToLower.Contains(search.ToLower) Then
                            Dim newBPItem As New Node
                            newBPItem.Text = BP.Name
                            adtBlueprints.Nodes.Add(newBPItem)
                            For NewCell As Integer = 1 To 7 : newBPItem.Cells.Add(New Cell) : Next
                            newBPItem.Cells(1).Text = "n/a"
                            newBPItem.Cells(2).Text = "n/a"
                            newBPItem.Cells(3).Text = BP.TechLevel.ToString
                            newBPItem.Cells(4).Text = "0"
                            newBPItem.Cells(5).Text = "0"
                            newBPItem.Cells(6).Text = "Infinite"
                            newBPItem.Cells(7).Text = "n/a"
                        End If
                    End If
                End If
            Next
            EveHQ.Core.AdvTreeSorter.Sort(adtBlueprints, 1, True, True)
            adtBlueprints.EndUpdate()
        Else
            ' Show the owned BP list
            Call Me.UpdateOwnerBPList()
        End If
    End Sub

    Private Sub UpdateOwnerBPList()
        Dim search As String = txtBPSearch.Text
        ' Establish the owner
        If cboBPOwner.SelectedItem IsNot Nothing Then
            Dim owner As String = cboBPOwner.SelectedItem.ToString()

            adtBlueprints.BeginUpdate()
            adtBlueprints.Nodes.Clear()
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
                                    Dim newBPItem As New Node
                                    For NewCell As Integer = 1 To 7 : newBPItem.Cells.Add(New Cell) : Next
                                    newBPItem.Text = BPData.Name
                                    newBPItem.Tag = BP.AssetID
                                    adtBlueprints.Nodes.Add(newBPItem)
                                    newBPItem.Cells(3).Text = BPData.TechLevel.ToString
                                    Call UpdateOwnerBPItem(owner, LocationName, BP, newBPItem)
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            EveHQ.Core.AdvTreeSorter.Sort(adtBlueprints, 1, True, True)
            adtBlueprints.EndUpdate()
        End If
    End Sub
    Private Sub UpdateOwnerBPItem(ByVal Owner As String, ByVal LocationName As String, ByVal BP As BlueprintAsset, ByVal newBPItem As Node)
        newBPItem.Cells(4).Text = FormatNumber(BP.MELevel, 0)
        newBPItem.Cells(5).Text = FormatNumber(BP.PELevel, 0)
        Select Case BP.BPType
            Case BPType.Unknown  ' Undetermined
                newBPItem.Cells(1).Text = LocationName
                newBPItem.Cells(2).Text = BP.LocationDetails
                newBPItem.Cells(6).Text = "Unknown"
                newBPItem.Cells(6).Tag = BP.Runs
                newBPItem.Style = BPMStyleUnknown
            Case BPType.BPO  ' BPO
                newBPItem.Cells(1).Text = LocationName
                newBPItem.Cells(2).Text = BP.LocationDetails
                newBPItem.Cells(6).Text = "BPO"
                newBPItem.Cells(6).Tag = 1000000
                newBPItem.Style = BPMStyleBPO
            Case BPType.BPC  ' BPC
                newBPItem.Cells(1).Text = LocationName
                newBPItem.Cells(2).Text = BP.LocationDetails
                newBPItem.Cells(6).Text = FormatNumber(BP.Runs, 0)
                newBPItem.Cells(6).Tag = BP.Runs
                newBPItem.Style = BPMStyleBPC
            Case BPType.User
                newBPItem.Cells(1).Text = Owner & "'s Secret BP Stash"
                newBPItem.Cells(2).Text = Owner & "'s Secret BP Stash"
                newBPItem.Cells(6).Text = "BPO"
                newBPItem.Cells(6).Tag = 1000000
                newBPItem.Style = BPMStyleUser
        End Select
        newBPItem.Cells(7).Text = [Enum].GetName(GetType(BPStatus), BP.Status)
        newBPItem.Cells(7).Tag = BP.Status
        Select Case BP.Status
            Case BPStatus.Missing
                newBPItem.Style = BPMStyleMissing
            Case BPStatus.Exhausted
                newBPItem.Style = BPMStyleExhausted
        End Select
    End Sub

    Private Sub btnUpdateBPsFromAssets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateBPsFromAssets.Click

        ' Get the owner we will use
        Dim Owner As New PrismOwner
        If cboBPOwner.SelectedItem IsNot Nothing Then
            If PlugInData.PrismOwners.ContainsKey(cboBPOwner.SelectedItem.ToString) Then
                Owner = PlugInData.PrismOwners(cboBPOwner.SelectedItem.ToString)

                ' Fetch the ownerBPs if it exists
                Dim ownerBPs As New SortedList(Of String, BlueprintAsset)
                If PlugInData.BlueprintAssets.ContainsKey(Owner.Name) = True Then
                    ownerBPs = PlugInData.BlueprintAssets(Owner.Name)
                Else
                    PlugInData.BlueprintAssets.Add(Owner.Name, ownerBPs)
                End If

                Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Assets)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Assets)
                Dim AssetXML As New XmlDocument
                Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

                If Owner.IsCorp = True Then
                    AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                Else
                    AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                End If

                If AssetXML IsNot Nothing Then
                    Dim Assets As New SortedList(Of String, BlueprintAsset)
                    Dim locList As XmlNodeList = AssetXML.SelectNodes("/eveapi/result/rowset/row")
                    If locList.Count > 0 Then
                        ' Define what we want to obtain
                        Dim categories, groups, types As New ArrayList
                        categories.Add(9) ' Blueprints
                        For Each loc As XmlNode In locList
                            Dim locationID As String = loc.Attributes.GetNamedItem("locationID").Value
                            Dim flagID As Integer = CInt(loc.Attributes.GetNamedItem("flag").Value)
                            Dim locationDetails As String = PlugInData.itemFlags(flagID).ToString
                            Dim BPCFlag As Boolean = False
                            ' Check the asset
                            Dim ItemData As New EveHQ.Core.EveItem
                            Dim AssetID As String = ""
                            Dim itemID As String = ""
                            AssetID = loc.Attributes.GetNamedItem("itemID").Value
                            itemID = loc.Attributes.GetNamedItem("typeID").Value
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                                ItemData = EveHQ.Core.HQ.itemData(itemID)
                                ' Check for BPO/BPC
                                If ItemData.Category = 9 Then
                                    If loc.Attributes.GetNamedItem("singleton").Value = "1" Then
                                        If loc.Attributes.GetNamedItem("rawQuantity") IsNot Nothing Then
                                            If loc.Attributes.GetNamedItem("rawQuantity").Value = "-2" Then
                                                BPCFlag = True
                                            End If
                                        End If
                                    End If
                                End If
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
                                    If Owner.IsCorp = True Then
                                        Dim accountID As Integer = flagID + 885
                                        If accountID = 889 Then accountID = 1000
                                        If divisions.ContainsKey(Owner.ID & "_" & accountID.ToString) = True Then
                                            locationDetails = CStr(divisions.Item(Owner.ID & "_" & accountID.ToString))
                                        End If
                                    End If
                                    If newBP.BPType = BPType.Unknown Then
                                        If BPCFlag = True Then
                                            newBP.BPType = BPType.BPC
                                            newBP.Runs = 1
                                        Else
                                            newBP.BPType = BPType.BPO
                                            newBP.Runs = -1
                                        End If
                                    End If
                                    newBP.LocationDetails = locationDetails
                                    newBP.TypeID = itemID
                                    newBP.Status = BPStatus.Present
                                    newBP.MELevel = 0
                                    newBP.PELevel = 0
                                    newBP.Notes = ""
                                    Assets.Add(AssetID, newBP)
                                End If
                            End If

                            ' Get the location name
                            If loc.ChildNodes.Count > 0 Then
                                Call GetAssetFromNode(loc, categories, groups, types, Assets, locationID, locationDetails, Owner)
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
                                ' Update the type
                                ownerBPs(assetID).BPType = Assets(assetID).BPType
                                ' Update the runs if we have found the asset is a BPC and the runs are still -1
                                If ownerBPs(assetID).BPType = BPType.BPC And ownerBPs(assetID).Runs = -1 Then
                                    ownerBPs(assetID).Runs = 0
                                End If
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
    Private Sub GetAssetFromNode(ByVal loc As XmlNode, ByVal categories As ArrayList, ByVal groups As ArrayList, ByVal types As ArrayList, ByRef Assets As SortedList(Of String, BlueprintAsset), ByVal locationID As String, ByVal locationDetails As String, ByVal Owner As PrismOwner)
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
            Dim BPCFlag As Boolean = False
            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                ItemData = EveHQ.Core.HQ.itemData(itemID)
                ' Check for BPO/BPC
                If ItemData.Category = 9 Then
                    If item.Attributes.GetNamedItem("singleton").Value = "1" Then
                        If item.Attributes.GetNamedItem("rawQuantity") IsNot Nothing Then
                            If item.Attributes.GetNamedItem("rawQuantity").Value = "-2" Then
                                BPCFlag = True
                            End If
                        End If
                    End If
                End If
                If PlugInData.AssetItemNames.ContainsKey(containerID) = True Then
                    flagName = locationDetails & "/" & PlugInData.AssetItemNames(containerID)
                Else
                    flagName = locationDetails & "/" & EveHQ.Core.HQ.itemData(containerType).Name
                End If
                If categories.Contains(ItemData.Category) Or groups.Contains(ItemData.Group) Or types.Contains(ItemData.ID) Then
                    Dim newBP As New BlueprintAsset
                    newBP.AssetID = AssetID
                    newBP.LocationID = locationID
                    If Owner.IsCorp = True And EveHQ.Core.HQ.itemData(itemID).Group <> 16 Then
                        Dim accountID As Integer = flagID + 885
                        If accountID = 889 Then accountID = 1000
                        If divisions.ContainsKey(Owner.ID & "_" & accountID.ToString) = True Then
                            flagName = locationDetails & "/" & CStr(divisions.Item(Owner.ID & "_" & accountID.ToString))
                        End If
                    End If
                    If newBP.BPType = BPType.Unknown Then
                        If BPCFlag = True Then
                            newBP.BPType = BPType.BPC
                            newBP.Runs = 1
                        Else
                            newBP.BPType = BPType.BPO
                            newBP.Runs = -1
                        End If
                    End If
                    newBP.LocationDetails = flagName
                    newBP.TypeID = itemID
                    newBP.Status = BPStatus.Present
                    newBP.MELevel = 0
                    newBP.PELevel = 0
                    newBP.Notes = ""
                    Assets.Add(AssetID, newBP)
                End If
            End If
            ' Check child items if they exist
            If item.ChildNodes.Count > 0 Then
                Call GetAssetFromNode(item, categories, groups, types, Assets, locationID, flagName, Owner)
            End If
        Next
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

    Private Sub btnGetBPJobInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetBPJobInfo.Click
        ' Get the owner BPs
        Dim ownerBPs As New SortedList(Of String, BlueprintAsset)
        If cboBPOwner.SelectedItem IsNot Nothing Then
            Dim owner As String = cboBPOwner.SelectedItem.ToString()
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
        If adtBlueprints.SelectedNodes.Count = 1 Then
            mnuSendToBPCalc.Enabled = True
            ' Get the blueprint info
            If chkShowOwnedBPs.Checked = True Then
                Dim assetID As String = CStr(adtBlueprints.SelectedNodes(0).Tag)
                Dim BPOwner As String = cboBPOwner.SelectedItem.ToString
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
            mnuRemoveCustomBP.Text = "Remove Blueprints (" & adtBlueprints.SelectedNodes.Count.ToString & ")"
            mnuRemoveCustomBP.Enabled = True
        End If
        mnuAmendBPDetails.Enabled = chkShowOwnedBPs.Checked
    End Sub

    Private Sub mnuSendToBPCalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSendToBPCalc.Click
        If adtBlueprints.SelectedNodes.Count = 1 Then
            Dim BPName As String = adtBlueprints.SelectedNodes(0).Text
            If chkShowOwnedBPs.Checked = True Then
                ' Start an owned BPCalc
                If adtBlueprints.SelectedNodes(0).Tag IsNot Nothing Then
                    Dim BPID As Long = CLng(adtBlueprints.SelectedNodes(0).Tag)
                    Dim BPCalc As New frmBPCalculator(cboBPOwner.SelectedItem.ToString, BPID)
                    Call OpenBPCalculator(BPCalc)
                End If
            Else
                ' Start a standard BPCalc
                Dim BPCalc As New frmBPCalculator(BPName)
                Call OpenBPCalculator(BPCalc)
            End If
        ElseIf adtBlueprints.SelectedNodes.Count = 0 Then
            ' Start a blank BP Calc
            Dim BPCalc As New frmBPCalculator(chkShowOwnedBPs.Checked)
            Call OpenBPCalculator(BPCalc)
        End If
    End Sub

    Private Sub OpenBPCalculator(ByVal BPCalc As frmBPCalculator)
        BPCalc.Location = New Point(CInt(Me.ParentForm.Left + ((Me.ParentForm.Width - BPCalc.Width) / 2)), CInt(Me.ParentForm.Top + ((Me.ParentForm.Height - BPCalc.Height) / 2)))
        BPCalc.Show()
    End Sub

    Private Sub mnuAmendBPDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAmendBPDetails.Click
        Call Me.EditBlueprintDetails()
    End Sub

    Private Sub EditBlueprintDetails()
        Dim BPForm As New frmEditBPDetails
        BPForm.OwnerName = cboBPOwner.SelectedItem.ToString
        Dim BPs As New ArrayList
        For Each selItem As Node In adtBlueprints.SelectedNodes
            BPs.Add(selItem.Tag.ToString)
        Next
        If BPs.Count > 0 Then
            BPForm.AssetIDs = BPs
            BPForm.ShowDialog()
            ' Update the list using the details
            Dim BP As New BlueprintAsset
            Dim locationName As String = ""
            For Each selitem As Node In adtBlueprints.SelectedNodes
                BP = PlugInData.BlueprintAssets(BPForm.OwnerName).Item(selitem.Tag.ToString)
                locationName = Me.GetLocationNameFromID(BP.LocationID)
                Call Me.UpdateOwnerBPItem(BPForm.OwnerName, locationName, BP, selitem)
            Next
        Else
            Dim msg As New StringBuilder
            msg.AppendLine("An attempt to start the BP Editor was made but it appears as if there is nothing to edit! Please take a screenshot of this message together with the Blueprint Manager list and submit it to the developers for investigation.")
            msg.AppendLine("")
            msg.AppendLine("ArrayList Count: " & BPs.Count.ToString)
            msg.AppendLine("Selected Node Count: " & adtBlueprints.SelectedNodes.Count.ToString)
            MessageBox.Show(msg.ToString, "No Blueprints Selected??", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub txtBPSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBPSearch.TextChanged
        Call Me.UpdateBPList()
    End Sub

    Private Sub btnResetBPSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetBPSearch.Click
        txtBPSearch.Text = ""
    End Sub

    Private Sub btnAddCustomBP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCustomBP.Click
        Dim BPForm As New frmAddCustomBP
        If cboBPOwner.SelectedItem IsNot Nothing Then
            BPForm.BPOwner = cboBPOwner.SelectedItem.ToString
            BPForm.ShowDialog()
            If BPForm.DialogResult = Windows.Forms.DialogResult.OK Then
                Call Me.UpdateBPList()
            End If
        Else
            MessageBox.Show("Please select an BP Owner before adding a custom blueprint.", "BP Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub mnuRemoveCustomBP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveCustomBP.Click
        ' Remove the custom BP from the assets
        If adtBlueprints.SelectedNodes.Count > 0 Then
            Dim cIDX As Integer = adtBlueprints.SelectedNodes.Count - 1
            ' Establish list for removal
            Dim RemovalList As New List(Of Node)
            For Each RN As Node In adtBlueprints.SelectedNodes
                RemovalList.Add(RN)
            Next
            ' Remove the nodes
            Dim BPOwner As String = cboBPOwner.SelectedItem.ToString
            For Each RN As Node In RemovalList
                Dim assetID As String = CStr(RN.Tag)
                If PlugInData.BlueprintAssets(BPOwner).ContainsKey(assetID) = True Then
                    PlugInData.BlueprintAssets(BPOwner).Remove(assetID)
                    adtBlueprints.Nodes.Remove(RN)
                    cIDX -= 1
                End If
            Next
            RemovalList.Clear()
        End If
    End Sub

    Private Sub cboTechFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTechFilter.SelectedIndexChanged
        If startup = False And BPManagerUpdate = False Then
            Call UpdateBPList()
        End If
    End Sub

    Private Sub cboTypeFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTypeFilter.SelectedIndexChanged
        If startup = False And BPManagerUpdate = False Then
            Call UpdateBPList()
        End If
    End Sub

    Private Sub cboCategoryFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCategoryFilter.SelectedIndexChanged
        If startup = False And BPManagerUpdate = False Then
            Call UpdateBPList()
        End If
    End Sub

    Private Sub adtBlueprints_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtBlueprints.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, False, False)
    End Sub

    Private Sub adtBlueprints_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtBlueprints.NodeDoubleClick
        If adtBlueprints.SelectedNodes.Count = 1 Then
            Dim BPName As String = adtBlueprints.SelectedNodes(0).Text
            If chkShowOwnedBPs.Checked = True Then
                ' Start an owned BPCalc
                If adtBlueprints.SelectedNodes(0).Tag IsNot Nothing Then
                    Dim BPID As Long = CLng(adtBlueprints.SelectedNodes(0).Tag)
                    Dim BPCalc As New frmBPCalculator(cboBPOwner.SelectedItem.ToString, BPID)
                    Call OpenBPCalculator(BPCalc)
                End If
            Else
                ' Start a standard BPCalc
                Dim BPCalc As New frmBPCalculator(BPName)
                Call OpenBPCalculator(BPCalc)
            End If
        ElseIf adtBlueprints.SelectedNodes.Count = 0 Then
            ' Start a blank BP Calc
            Dim BPCalc As New frmBPCalculator(chkShowOwnedBPs.Checked)
            Call OpenBPCalculator(BPCalc)
        End If
    End Sub

    Private Sub btnCopyListToClipboard_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyListToClipboard.Click
        ' Exports the list to Clipboard in TSV format for pasting to Excel etc
        If cboBPOwner.SelectedItem IsNot Nothing Then
            Call Me.ExportToClipboard("Blueprint List for " & cboBPOwner.SelectedItem.ToString, adtBlueprints, ControlChars.Tab)
        Else
            MessageBox.Show("A BP Owner is required before copying the data to the clipboard", "BP Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#End Region

#Region "Transaction List Menu Options"

    Private Sub ctxTransactions_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxTransactions.Opening
        If adtTransactions.SelectedNodes.Count > 0 Then
            Dim TransItem As Node = adtTransactions.SelectedNodes(0)
            Dim ItemName As String = TransItem.Cells(1).Text
            mnuTransactionModifyPrice.Text = "Modify Custom Price of " & ItemName
            mnuTransactionModifyPrice.Tag = EveHQ.Core.HQ.itemList(ItemName)
        End If
    End Sub

    Private Sub mnuTransactionModifyPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuTransactionModifyPrice.Click
        If mnuTransactionModifyPrice.Tag IsNot Nothing Then
            Dim ItemID As String = mnuTransactionModifyPrice.Tag.ToString
            Dim Price As Double = Double.Parse(adtTransactions.SelectedNodes(0).Cells(3).Text, Globalization.NumberStyles.Any, culture)
            Dim NewPrice As New EveHQ.Core.frmModifyPrice(ItemID, Price)
            NewPrice.ShowDialog()
        End If
    End Sub

#End Region

#Region "Ribbon and Tab UI Functions"

    Private Sub tabPrism_SelectedTabChanging(sender As Object, e As DevComponents.DotNetBar.TabStripTabChangingEventArgs) Handles tabPrism.SelectedTabChanging
        SelectedTab = e.NewTab
    End Sub

    Private Sub tabPrism_TabItemClose(ByVal sender As Object, ByVal e As DevComponents.DotNetBar.TabStripActionEventArgs) Handles tabPrism.TabItemClose
        e.Cancel = True
        If SelectedTab IsNot Nothing Then
            If SelectedTab.Name <> "tiPrismHome" Then
                SelectedTab.Visible = False
            End If
        End If
    End Sub

    Private Sub btnOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptions.Click
        Dim NewSettings As New frmPrismSettings
        NewSettings.ShowDialog()
    End Sub

    Private Sub btnDownloadAPIData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownloadAPIData.Click

        ' Set the label and disable the button
        lblCurrentAPI.Text = "Downloading API Data..."
        btnDownloadAPIData.Enabled = False

        ' Flick to the API Status tab
        tabPrism.SelectedTab = tiPrismHome
        ' Delete the current API Status data
        For Each Owner As ListViewItem In lvwCurrentAPIs.Items
            Owner.ToolTipText = ""
            Dim OwnerName As String = Owner.Text
            For si As Integer = 0 To 7
                'If Owner.SubItems(1).Text = "Corporation" = True Then
                '    If Settings.PrismSettings.CorpReps.ContainsKey(OwnerName) = True Then
                '        If Settings.PrismSettings.CorpReps(OwnerName).ContainsKey(CType(si, CorpRepType)) = True Then
                '            Owner.SubItems(si + 2).Text = ""
                '        Else
                '            Owner.SubItems(si + 2).Text = "No Corp Rep"
                '            Owner.SubItems(si + 2).ForeColor = Color.Red
                '        End If
                '    Else
                '        Owner.SubItems(si + 2).Text = "No Corp Rep"
                '        Owner.SubItems(si + 2).ForeColor = Color.Red
                '    End If
                'Else
                '    Owner.SubItems(si + 2).Text = ""
                'End If
                Owner.SubItems(si + 2).Text = ""
            Next
        Next

        ' Get XMLs
        Call Me.StartGetXMLDataThread()

    End Sub

    Private Sub btnWalletJournal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWalletJournal.Click
        tabPrism.SelectedTab = tiJournal
        tiJournal.Visible = True
    End Sub

    Private Sub btnWalletTransactions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWalletTransactions.Click
        tabPrism.SelectedTab = tiTransactions
        tiTransactions.Visible = True
    End Sub

    Private Sub btnAssets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssets.Click
        tabPrism.SelectedTab = tiAssets
        tiAssets.Visible = True
    End Sub

    Private Sub btnBPManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBPManager.Click
        tabPrism.SelectedTab = tiBPManager
        tiBPManager.Visible = True
    End Sub

    Private Sub btnRecycler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecycler.Click
        tabPrism.SelectedTab = tiRecycler
        tiRecycler.Visible = True
    End Sub

    Private Sub btnOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrders.Click
        tabPrism.SelectedTab = tiMarketOrders
        tiMarketOrders.Visible = True
    End Sub

    Private Sub btnJobs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJobs.Click
        tiJobs.Visible = True
        tabPrism.SelectedTab = tiJobs
    End Sub

    Private Sub btnContracts_Click(sender As System.Object, e As System.EventArgs) Handles btnContracts.Click
        tiContracts.Visible = True
        tabPrism.SelectedTab = tiContracts
    End Sub

    Private Sub btnReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReports.Click
        tabPrism.SelectedTab = tiReports
        tiReports.Visible = True
    End Sub

    Private Sub btnCharts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCharts.Click
        tabPrism.SelectedTab = tiCharts
        tiCharts.Visible = True
    End Sub

    Private Sub btnInventionChance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInventionChance.Click
        Dim InvCalc As New frmQuickInventionChance
        InvCalc.ShowDialog()
        InvCalc.Dispose()
    End Sub

    Private Sub btnBlueprintCalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlueprintCalc.Click
        ' Start a blank BP Calc
        Dim BPCalc As New frmBPCalculator(chkShowOwnedBPs.Checked)
        Call OpenBPCalculator(BPCalc)
    End Sub

    Private Sub btnProductionManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProductionManager.Click
        tabPrism.SelectedTab = tiProductionManager
        tiProductionManager.Visible = True
    End Sub

    Private Sub btnInventionManager_Click(sender As System.Object, e As System.EventArgs) Handles btnInventionManager.Click
        tabPrism.SelectedTab = tiInventionManager
        tiInventionManager.Visible = True
    End Sub

    Private Sub btnQuickProduction_Click(sender As System.Object, e As System.EventArgs) Handles btnQuickProduction.Click
        Dim QP As New frmQuickProduction
        QP.ShowDialog()
        QP.Dispose()
    End Sub

    Private Sub btnRigBuilder_Click(sender As System.Object, e As System.EventArgs) Handles btnRigBuilder.Click
        tabPrism.SelectedTab = tiRigBuilder
        tiRigBuilder.Visible = True
    End Sub

    Private Sub btnInventionResults_Click(sender As System.Object, e As System.EventArgs) Handles btnInventionResults.Click
        tabPrism.SelectedTab = tiInventionResults
        tiInventionResults.Visible = True
    End Sub

#End Region

#Region "Search and Search UI Functions"

    Private Sub txtItemSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemSearch.TextChanged
        If Len(txtItemSearch.Text) > 2 Then
            Dim strSearch As String = txtItemSearch.Text.Trim.ToLower
            adtSearch.BeginUpdate()
            adtSearch.Nodes.Clear()
            ' Check items
            For Each item As String In EveHQ.Core.HQ.itemList.Keys
                If item.ToLower.Contains(strSearch) Then
                    Dim NewNode As New Node(item)
                    NewNode.Name = item
                    NewNode.TagString = "Item"
                    adtSearch.Nodes.Add(NewNode)
                End If
            Next
            ' Check Batch Jobs
            For Each BJob As BatchJob In BatchJobs.Jobs.Values
                ' Check the Job Name
                If BJob.BatchName.ToLower.Contains(strSearch) Then
                    Dim NewNode As New Node(BJob.BatchName & " [Batch Job]")
                    NewNode.Name = BJob.BatchName
                    NewNode.TagString = "Batch"
                    adtSearch.Nodes.Add(NewNode)
                End If
            Next
            ' Check Production Jobs
            For Each PJob As ProductionJob In ProductionJobs.Jobs.Values
                ' Check the Job Name
                If PJob.JobName.ToLower.Contains(strSearch) Then
                    Dim NewNode As New Node(PJob.JobName & " [Production Job]")
                    NewNode.Name = PJob.JobName
                    NewNode.TagString = "Production"
                    adtSearch.Nodes.Add(NewNode)
                End If
                ' Check the Job Type
                If PJob.TypeName.ToLower.Contains(strSearch) Then
                    Dim NewNode As New Node(PJob.TypeName & " [in Production Job '" & PJob.JobName & "']")
                    NewNode.Name = PJob.TypeName
                    NewNode.TagString = "Item"
                    adtSearch.Nodes.Add(NewNode)
                End If
            Next
            adtSearch.EndUpdate()
        End If
    End Sub

    Private Sub btnLinkBPCalc_Click(sender As System.Object, e As System.EventArgs) Handles btnLinkBPCalc.Click
        Dim KeyName As String = adtSearch.SelectedNodes(0).Name
        Select Case adtSearch.SelectedNodes(0).TagString
            Case "Item"
                Dim BPName As String = lblSelectedBP.Tag.ToString
                ' Start a standard BP Calc
                Dim BPCalc As New frmBPCalculator(BPName)
                Call OpenBPCalculator(BPCalc)
            Case "Production"
                If Prism.ProductionJobs.Jobs.ContainsKey(KeyName) Then
                    Dim PJob As ProductionJob = Prism.ProductionJobs.Jobs(KeyName)
                    Dim BPCalc As New frmBPCalculator(PJob, False)
                    Call OpenBPCalculator(BPCalc)
                End If
        End Select
    End Sub

    Private Sub btnLinkRequisition_Click(sender As System.Object, e As System.EventArgs) Handles btnLinkRequisition.Click
        Dim KeyName As String = adtSearch.SelectedNodes(0).Name
        Select Case adtSearch.SelectedNodes(0).TagString
            Case "Item"
                ' Set up a new Sortedlist to store the required items
                Dim Orders As New SortedList(Of String, Integer)
                ' Add the current item
                Orders.Add(KeyName, 1)
                ' Setup the Requisition form for Prism and open it
                Dim newReq As New EveHQ.Core.frmAddRequisition("Prism", Orders)
                newReq.ShowDialog()
                newReq.Dispose()
            Case "Production"
                ' Set up a new Sortedlist to store the required items
                Dim Orders As New SortedList(Of String, Integer)
                If Prism.ProductionJobs.Jobs.ContainsKey(KeyName) Then
                    Dim PJob As ProductionJob = Prism.ProductionJobs.Jobs(KeyName)
                    Call Me.CreateRequisitionFromJob(Orders, PJob)
                End If
                ' Setup the Requisition form for Prism and open it
                Dim newReq As New EveHQ.Core.frmAddRequisition("Prism", Orders)
                newReq.ShowDialog()
                newReq.Dispose()
            Case "Batch"
                ' Set up a new Sortedlist to store the required items
                Dim Orders As New SortedList(Of String, Integer)
                If Prism.BatchJobs.Jobs.ContainsKey(KeyName) Then
                    For Each PJobName As String In Prism.BatchJobs.Jobs(KeyName).ProductionJobs
                        If Prism.ProductionJobs.Jobs.ContainsKey(PJobName) Then
                            Dim PJob As ProductionJob = Prism.ProductionJobs.Jobs(PJobName)
                            Call Me.CreateRequisitionFromJob(Orders, PJob)
                        End If
                    Next
                End If
                ' Setup the Requisition form for Prism and open it
                Dim newReq As New EveHQ.Core.frmAddRequisition("Prism", Orders)
                newReq.ShowDialog()
                newReq.Dispose()
        End Select
    End Sub

    Private Sub btnLinkProduction_Click(sender As System.Object, e As System.EventArgs) Handles btnLinkProduction.Click
        Dim QP As New frmQuickProduction(lblSelectedBP.Tag.ToString)
        QP.ShowDialog()
        QP.Dispose()
    End Sub

    Private Sub CreateRequisitionFromJob(Orders As SortedList(Of String, Integer), CurrentJob As ProductionJob)

        Dim maxProducableUnits As Long = -1
        Dim UnitMaterial As Double = 0
        Dim UnitWaste As Double = 0

        If CurrentJob IsNot Nothing Then
            For Each resource As Object In CurrentJob.RequiredResources.Values
                If TypeOf (resource) Is RequiredResource Then
                    ' This is a resource so add it
                    Dim rResource As RequiredResource = CType(resource, RequiredResource)
                    If rResource.TypeCategory <> 16 Then
                        Dim perfectRaw As Integer = CInt(rResource.PerfectUnits)
                        Dim waste As Integer = CInt(rResource.WasteUnits)
                        Dim total As Integer = perfectRaw + waste
                        Dim price As Double = EveHQ.Core.DataFunctions.GetPrice(CStr(rResource.TypeID))
                        Dim value As Double = total * price
                        If total > 0 Then
                            UnitMaterial += value
                            UnitWaste += waste * price
                            Dim PerfectTotal As Long = CLng(perfectRaw) * CLng(CurrentJob.Runs)
                            Dim WasteTotal As Long = CLng(waste) * CLng(CurrentJob.Runs)
                            Dim TotalTotal As Long = CLng(total) * CLng(CurrentJob.Runs)
                            If Orders.ContainsKey(rResource.TypeName) = False Then
                                Orders.Add(rResource.TypeName, CInt(TotalTotal))
                            Else
                                Orders(rResource.TypeName) += CInt(TotalTotal)
                            End If
                        End If
                    End If
                Else
                    ' This is another production job
                    Dim subJob As ProductionJob = CType(resource, ProductionJob)
                    Call CreateRequisitionFromJob(Orders, subJob)
                End If
            Next
        End If
    End Sub

    Private Sub adtSearch_NodeClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtSearch.NodeClick

        Select Case adtSearch.SelectedNodes(0).TagString
            Case "Item"
                ' Get the name and ID
                Dim itemName As String = adtSearch.SelectedNodes(0).Name
                If EveHQ.Core.HQ.itemList.ContainsKey(itemName) = True Then
                    Dim itemID As String = EveHQ.Core.HQ.itemList(itemName)

                    ' See if we have a blueprint
                    Dim BPName As String = ""
                    Dim BPID As String = ""
                    If itemName.EndsWith("Blueprint") = False Then
                        If EveHQ.Core.HQ.itemList.ContainsKey(itemName.Trim & " Blueprint") = True Then
                            BPName = itemName.Trim & " Blueprint"
                            BPID = EveHQ.Core.HQ.itemList(BPName)
                        End If
                    Else
                        BPID = itemID
                        BPName = itemName
                        itemID = PlugInData.Blueprints(BPID).ProductID.ToString
                        itemName = EveHQ.Core.HQ.itemData(itemID).Name
                    End If

                    lblSelectedItem.Text = "Item: " & itemName
                    lblSelectedItem.Tag = itemName
                    If BPName <> "" Then
                        lblSelectedBP.Text = "Blueprint: " & BPName
                        lblSelectedBP.Tag = BPName
                    Else
                        lblSelectedBP.Text = "Blueprint: <none available>"
                    End If

                    ' Check we can activate buttons
                    If BPID <> "" Then
                        btnLinkBPCalc.Enabled = True
                        btnLinkProduction.Enabled = True
                    Else
                        btnLinkBPCalc.Enabled = False
                        btnLinkProduction.Enabled = False
                    End If
                    btnLinkRequisition.Enabled = True
                End If
            Case "Production"
                Dim JobName As String = adtSearch.SelectedNodes(0).Name
                If Prism.ProductionJobs.Jobs.ContainsKey(JobName) = True Then
                    lblSelectedItem.Text = "Job: " & JobName
                    lblSelectedBP.Text = "Blueprint: <per job>"
                    btnLinkBPCalc.Enabled = True
                    btnLinkProduction.Enabled = False
                    btnLinkRequisition.Enabled = True
                End If
            Case "Batch"
                Dim BatchName As String = adtSearch.SelectedNodes(0).Name
                If Prism.BatchJobs.Jobs.ContainsKey(BatchName) = True Then
                    lblSelectedItem.Text = "Batch: " & BatchName
                    lblSelectedBP.Text = "Blueprint: <multiple>"
                    btnLinkBPCalc.Enabled = False
                    btnLinkProduction.Enabled = False
                    btnLinkRequisition.Enabled = True
                End If
        End Select

    End Sub

    Private Sub adtSearch_NodeDoubleClick(sender As Object, e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtSearch.NodeDoubleClick
        Dim KeyName As String = e.Node.Name
        Select Case e.Node.TagString
            Case "Item"
                Dim itemName As String = KeyName
                Dim itemID As String = EveHQ.Core.HQ.itemList(itemName)
                ' See if we have a blueprint
                Dim BPName As String = ""
                Dim BPID As String = ""
                If itemName.EndsWith("Blueprint") = False Then
                    If EveHQ.Core.HQ.itemList.ContainsKey(itemName.Trim & " Blueprint") = True Then
                        BPName = itemName.Trim & " Blueprint"
                        BPID = EveHQ.Core.HQ.itemList(BPName)
                    End If
                Else
                    BPID = itemID
                    BPName = itemName
                    itemID = PlugInData.Blueprints(BPID).ProductID.ToString
                    itemName = EveHQ.Core.HQ.itemData(itemID).Name
                End If
                If BPID <> "" Then
                    ' Start a standard BP Calc
                    Dim BPCalc As New frmBPCalculator(BPName)
                    Call OpenBPCalculator(BPCalc)
                End If
            Case "Production"
                If Prism.ProductionJobs.Jobs.ContainsKey(KeyName) Then
                    Dim PJob As ProductionJob = Prism.ProductionJobs.Jobs(KeyName)
                    Dim BPCalc As New frmBPCalculator(PJob, False)
                    Call OpenBPCalculator(BPCalc)
                End If
        End Select
    End Sub

#End Region

#Region "Production Manager Routines"

    Private Sub UpdateProductionJobList()
        adtProdJobs.BeginUpdate()
        adtProdJobs.Nodes.Clear()
        For Each cJob As ProductionJob In ProductionJobs.Jobs.Values
            Dim NewJob As New Node
            NewJob.Name = cJob.JobName
            NewJob.Text = cJob.JobName
            NewJob.Cells.Add(New Cell(cJob.TypeName))
            If cJob.CurrentBP IsNot Nothing Then
                Dim product As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(CStr(cJob.CurrentBP.ProductID))
                Dim totalcosts As Double = cJob.Cost + Math.Round((Settings.PrismSettings.FactoryRunningCost / 3600 * cJob.RunTime) + Settings.PrismSettings.FactoryInstallCost, 2)
                Dim unitcosts As Double = Math.Round(totalcosts / (cJob.Runs * product.PortionSize), 2)
                Dim value As Double = EveHQ.Core.DataFunctions.GetPrice(CStr(cJob.CurrentBP.ProductID))
                Dim profit As Double = value - unitcosts
                Dim rate As Double = profit / ((cJob.RunTime / cJob.Runs) / 3600)
                Dim margin As Double = (profit / value * 100)
                NewJob.Cells.Add(New Cell(profit.ToString("N2")))
                NewJob.Cells.Add(New Cell(rate.ToString("N2")))
                NewJob.Cells.Add(New Cell(margin.ToString("N2")))
            Else
                NewJob.Cells.Add(New Cell(0.ToString("N2")))
                NewJob.Cells.Add(New Cell(0.ToString("N2")))
                NewJob.Cells.Add(New Cell(0.ToString("N2")))
            End If
            adtProdJobs.Nodes.Add(NewJob)
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtProdJobs, 1, True, True)
        adtProdJobs.EndUpdate()
    End Sub

    Private Sub UpdateBatchList()
        adtBatches.BeginUpdate()
        adtBatches.Nodes.Clear()
        For Each cBatch As BatchJob In BatchJobs.Jobs.Values
            Dim NewBatch As New Node
            NewBatch.Name = cBatch.BatchName
            NewBatch.Text = cBatch.BatchName
            For Each JobName As String In cBatch.ProductionJobs
                Dim NewJob As New Node
                NewJob.Name = JobName
                NewJob.Text = JobName
                NewBatch.Nodes.Add(NewJob)
            Next
            adtBatches.Nodes.Add(NewBatch)
        Next
        adtBatches.EndUpdate()
    End Sub

    Private Sub adtProdJobs_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtProdJobs.SelectionChanged
        Select Case adtProdJobs.SelectedNodes.Count
            Case 0
                btnDeleteJob.Text = "Delete Job"
                btnDeleteJob.Enabled = False
                btnMakeBatch.Enabled = False
                PRPM.BatchJob = Nothing
                PRPM.ProductionJob = Nothing
            Case 1
                btnDeleteJob.Text = "Delete Job"
                btnDeleteJob.Enabled = True
                btnMakeBatch.Enabled = False
                ' Create a null batch job to pass to the PR control to negate batch display
                PRPM.BatchJob = Nothing
                Dim JobName As String = adtProdJobs.SelectedNodes(0).Name
                Dim ExistingJob As ProductionJob = ProductionJobs.Jobs(JobName)
                PRPM.ProductionJob = ExistingJob
            Case Else
                btnDeleteJob.Text = "Delete Jobs"
                btnDeleteJob.Enabled = True
                btnMakeBatch.Enabled = True
                ' Create a temporary batch job to pass to the PR control
                Dim TempBatch As New BatchJob
                TempBatch.BatchName = "Temporary Batch from Production Manager"
                For Each JobNode As Node In adtProdJobs.SelectedNodes
                    TempBatch.ProductionJobs.Add(JobNode.Name)
                Next
                PRPM.BatchJob = TempBatch
        End Select
    End Sub

    Private Sub adtProdJobs_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtProdJobs.NodeDoubleClick
        Dim JobName As String = e.Node.Name
        Dim ExistingJob As ProductionJob = ProductionJobs.Jobs(JobName)
        Dim BPCalc As New frmBPCalculator(ExistingJob, False)
        BPCalc.Location = New Point(CInt(Me.ParentForm.Left + ((Me.ParentForm.Width - BPCalc.Width) / 2)), CInt(Me.ParentForm.Top + ((Me.ParentForm.Height - BPCalc.Height) / 2)))
        BPCalc.Show()
    End Sub

    Private Sub adtProdJobs_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtProdJobs.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub btnDeleteJob_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteJob.Click
        Dim reply As DialogResult = MessageBox.Show("Are you sure you want to delete the selected jobs?", "Confirm Job Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            For Each DelNode As Node In adtProdJobs.SelectedNodes
                ProductionJobs.Jobs.Remove(DelNode.Name)
            Next
            Call Me.UpdateProductionJobList()
        End If
    End Sub

    Private Sub btnClearAllJobs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAllJobs.Click
        Dim reply As DialogResult = MessageBox.Show("This will remove all your jobs. Are you sure you want to delete all jobs?", "Confirm Job Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            ProductionJobs.Jobs.Clear()
            Call Me.UpdateProductionJobList()
        End If
    End Sub

    Private Sub btnRefreshJobs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshJobs.Click
        ' Cycle through all the jobs and update the job names
        For Each JobName As String In ProductionJobs.Jobs.Keys
            ProductionJobs.Jobs(JobName).JobName = JobName
        Next
        Call Me.UpdateProductionJobList()
    End Sub

    Private Sub btnMakeBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMakeBatch.Click
        Dim NewBatchName As New frmAddBatchJob
        NewBatchName.ShowDialog()
        If NewBatchName.DialogResult = DialogResult.OK Then
            Dim NewBatch As New BatchJob
            NewBatch.BatchName = NewBatchName.JobName
            For Each JobNode As Node In adtProdJobs.SelectedNodes
                NewBatch.ProductionJobs.Add(JobNode.Name)
            Next
            BatchJobs.Jobs.Add(NewBatch.BatchName, NewBatch)
        End If
        NewBatchName.Dispose()
        PrismEvents.StartUpdateBatchJobs()
    End Sub

#End Region

#Region "Batch Manager Routines"

    Private Sub adtBatches_NodeClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtBatches.NodeClick
        Dim selNode As Node = e.Node
        If e.Node.Nodes.Count > 0 Then
            ' This is a batch name
            Dim BatchName As String = e.Node.Name
            Dim ExistingBatch As BatchJob = BatchJobs.Jobs(BatchName)
            PRPM.BatchJob = ExistingBatch
        Else
            ' This is a job name
            Dim JobName As String = e.Node.Name
            Dim ExistingJob As ProductionJob = ProductionJobs.Jobs(JobName)
            PRPM.ProductionJob = ExistingJob
        End If
    End Sub

    Private Sub adtBatches_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtBatches.SelectionChanged
        Select Case adtProdJobs.SelectedNodes.Count
            Case 0
                ' Do nothing
            Case 1
                If PRPM.BatchJob IsNot Nothing Then
                    PRPM.BatchJob = Nothing
                End If
            Case Is > 1
                ' Create a temporary batch job to pass to the PR control
                Dim TempBatch As New BatchJob
                TempBatch.BatchName = "Temporary Batch from Batch Manager"
                For Each JobNode As Node In adtProdJobs.SelectedNodes
                    TempBatch.ProductionJobs.Add(JobNode.Name)
                Next
                PRPM.BatchJob = TempBatch
        End Select
    End Sub

    Private Sub btnClearBatches_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearBatches.Click
        Dim reply As DialogResult = MessageBox.Show("This will remove all your batches. Are you sure you want to delete all batches?", "Confirm Batch Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            BatchJobs.Jobs.Clear()
            Call Me.UpdateBatchList()
        End If
    End Sub

#End Region

#Region "Invention Manager Routines"

    Private Sub UpdateInventionJobList()
        adtInventionJobs.BeginUpdate()
        adtInventionJobs.Nodes.Clear()
        For Each cJob As ProductionJob In ProductionJobs.Jobs.Values
            ' Check for the Invention Manager Flag
            If cJob.HasInventionJob = True Then
                Dim NewJob As New Node
                NewJob.Name = cJob.JobName
                NewJob.Text = cJob.JobName
                If cJob.InventionJob.InventedBPID <> 0 Then

                    ' Calculate costs
                    Dim InvCost As InventionCost = cJob.InventionJob.CalculateInventionCost
                    Dim IBP As BlueprintSelection = cJob.InventionJob.CalculateInventedBPC
                    Dim BatchQty As Integer = EveHQ.Core.HQ.itemData(IBP.ProductID.ToString).PortionSize
                    Dim InventionChance As Double = cJob.InventionJob.CalculateInventionChance
                    Dim InventionAttempts As Double = Math.Max(Math.Round(100 / InventionChance, 4), 1)
                    Dim InventionSuccessCost As Double = InventionAttempts * InvCost.TotalCost

                    ' Calculate Production Cost of invented item
                    Dim FactoryCost As Double = Math.Round((Settings.PrismSettings.FactoryRunningCost / 3600 * cJob.InventionJob.ProductionJob.RunTime) + Settings.PrismSettings.FactoryInstallCost, 2)
                    Dim AvgCost As Double = (Math.Round(InventionSuccessCost / IBP.Runs, 2) + cJob.InventionJob.ProductionJob.Cost + FactoryCost) / BatchQty
                    Dim SalesPrice As Double = EveHQ.Core.DataFunctions.GetPrice(IBP.ProductID.ToString)
                    Dim UnitProfit As Double = SalesPrice - AvgCost
                    Dim TotalProfit As Double = UnitProfit * IBP.Runs * BatchQty
                    Dim Margin As Double = UnitProfit / SalesPrice * 100

                    NewJob.Cells.Add(New Cell(EveHQ.Core.HQ.itemData(CStr(cJob.InventionJob.InventedBPID)).Name))
                    NewJob.Cells.Add(New Cell(InventionChance.ToString("N2")))
                    NewJob.Cells.Add(New Cell(InventionSuccessCost.ToString("N2")))
                    NewJob.Cells.Add(New Cell(AvgCost.ToString("N2")))
                    NewJob.Cells.Add(New Cell(SalesPrice.ToString("N2")))
                    NewJob.Cells.Add(New Cell(UnitProfit.ToString("N2")))
                    NewJob.Cells.Add(New Cell(Margin.ToString("N2")))
                Else
                    NewJob.Cells.Add(New Cell("n/a"))
                    NewJob.Cells.Add(New Cell(0.ToString("N2")))
                    NewJob.Cells.Add(New Cell(0.ToString("N2")))
                    NewJob.Cells.Add(New Cell(0.ToString("N2")))
                    NewJob.Cells.Add(New Cell(0.ToString("N2")))
                    NewJob.Cells.Add(New Cell(0.ToString("N2")))
                    NewJob.Cells.Add(New Cell(0.ToString("N2")))
                End If
                adtInventionJobs.Nodes.Add(NewJob)
            End If
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtInventionJobs, 1, True, True)
        adtInventionJobs.EndUpdate()
    End Sub

    Private Sub adtInventionJobs_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtInventionJobs.NodeDoubleClick
        Dim JobName As String = e.Node.Name
        Dim ExistingJob As ProductionJob = ProductionJobs.Jobs(JobName)
        Dim BPCalc As New frmBPCalculator(ExistingJob, True)
        BPCalc.Location = New Point(CInt(Me.ParentForm.Left + ((Me.ParentForm.Width - BPCalc.Width) / 2)), CInt(Me.ParentForm.Top + ((Me.ParentForm.Height - BPCalc.Height) / 2)))
        BPCalc.Show()
    End Sub

    Private Sub adtInventionJobs_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtInventionJobs.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

#End Region

#Region "Report Routines"

    Private Sub InitialiseReports()

        ' Update the report combobox with the reports

        cboReport.BeginUpdate()
        cboReport.Items.Clear()

        ' Add the report types here in 
        cboReport.Items.Add("Income Report")
        cboReport.Items.Add("Expenditure Report")
        cboReport.Items.Add("Income & Expenditure Report")
        cboReport.Items.Add("Corporation Tax Report")
        cboReport.Items.Add("Transaction Sales Report")
        cboReport.Items.Add("Transaction Purchases Report")
        cboReport.Items.Add("Transaction Trading Report")

        ' Finalise the report combobox update
        cboReport.EndUpdate()

        ' Set the dates
        dtiReportEndDate.Value = Now
        dtiReportStartDate.Value = Now.AddMonths(-1)

        cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, True, cboReportOwners)

    End Sub

    Private Sub cboReport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboReport.SelectedIndexChanged

        ' Set the report name
        Dim ReportName As String = cboReport.SelectedItem.ToString

        Select Case ReportName

            Case "Income Report"
                If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.JournalOwnersAll Then
                    cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, True, cboReportOwners)
                    cboReportOwners.Text = ""
                End If

            Case "Expenditure Report"
                If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.JournalOwnersAll Then
                    cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, True, cboReportOwners)
                    cboReportOwners.Text = ""
                End If

            Case "Income & Expenditure Report"
                If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.JournalOwnersAll Then
                    cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, True, cboReportOwners)
                    cboReportOwners.Text = ""
                End If

            Case "Corporation Tax Report"
                If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.JournalOwnersCorps Then
                    cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersCorps, True, cboReportOwners)
                    cboReportOwners.Text = ""
                End If

            Case "Transaction Sales Report"
                If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.TransactionOwnersAll Then
                    cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionOwnersAll, True, cboReportOwners)
                    cboReportOwners.Text = ""
                End If

            Case "Transaction Purchases Report"
                If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.TransactionOwnersAll Then
                    cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionOwnersAll, True, cboReportOwners)
                    cboReportOwners.Text = ""
                End If

            Case "Transaction Trading Report"
                If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.TransactionOwnersAll Then
                    cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionOwnersAll, True, cboReportOwners)
                    cboReportOwners.Text = ""
                End If

        End Select

    End Sub

    Private Sub btnGenerateReport_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerateReport.Click

        If CType(cboReportOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems.Count > 0 Then
            Call Me.GenerateReport()
        Else
            MessageBox.Show("You must select at least one owner before generating a report!", "Report Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub GenerateReport()

        If cboReport.SelectedItem IsNot Nothing Then

            ' Set the start and end date
            Dim StartDate As Date = New Date(dtiReportStartDate.Value.Year, dtiReportStartDate.Value.Month, dtiReportStartDate.Value.Day, 0, 0, 0)
            Dim EndDate As Date = New Date(dtiReportEndDate.Value.Year, dtiReportEndDate.Value.Month, dtiReportEndDate.Value.Day, 0, 0, 0)
            EndDate = EndDate.AddDays(1) ' Add 1 to the date so we can check everything less than it

            ' Set the report name
            Dim ReportName As String = cboReport.SelectedItem.ToString

            ' Set the report title
            Dim ReportTitle As String = ReportName & "<br />from " & StartDate.ToLongDateString & " to " & EndDate.AddDays(-1).ToLongDateString

            ' Create the report title
            Dim strHTML As String = Reports.HTMLHeader("Prism Report - " & ReportName, ReportTitle)

            ' Build the Owners List
            Dim OwnerNames As New List(Of String)
            Dim OwnerList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboReportOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                OwnerNames.Add(LVI.Name)
            Next

            ' Choose what report is selected and get the information
            Select Case ReportName

                Case "Income Report"
                    Dim ReportData As DataSet = Reports.GetJournalReportData(StartDate, EndDate, OwnerNames)
                    Dim Result As ReportResult = Reports.GenerateIncomeReportBodyHTML(Reports.GenerateIncomeAnalysis(ReportData))
                    strHTML &= Result.HTML

                Case "Expenditure Report"
                    Dim ReportData As DataSet = Reports.GetJournalReportData(StartDate, EndDate, OwnerNames)
                    Dim Result As ReportResult = Reports.GenerateExpenseReportBodyHTML(Reports.GenerateExpenseAnalysis(ReportData))
                    strHTML &= Result.HTML

                Case "Income & Expenditure Report"
                    Dim ReportData As DataSet = Reports.GetJournalReportData(StartDate, EndDate, OwnerNames)
                    Dim IResult As ReportResult = Reports.GenerateIncomeReportBodyHTML(Reports.GenerateIncomeAnalysis(ReportData))
                    strHTML &= IResult.HTML
                    Dim IncomeTotal As Double = CDbl(IResult.Values("Total Income"))
                    Dim EResult As ReportResult = Reports.GenerateExpenseReportBodyHTML(Reports.GenerateExpenseAnalysis(ReportData))
                    strHTML &= EResult.HTML
                    Dim ExpenditureTotal As Double = CDbl(EResult.Values("Total Expenditure"))
                    Dim CResult As ReportResult = Reports.GenerateCashFlowReportBodyHTML(IncomeTotal, ExpenditureTotal)
                    strHTML &= CResult.HTML
                    Dim MResult As ReportResult = Reports.GenerateMovementReportBodyHTML(Reports.GenerateOwnerMovements(ReportData))
                    strHTML &= MResult.HTML

                Case "Corporation Tax Report"
                    Dim ReportData As DataSet = Reports.GetJournalReportData(StartDate, EndDate, OwnerNames)
                    Dim Result As ReportResult = Reports.GenerateCTReportBodyHTML(Reports.GenerateCTAnalysis(ReportData))
                    strHTML &= Result.HTML

                Case "Transaction Sales Report"
                    Dim ReportData As DataSet = Reports.GetTransactionReportData(StartDate, EndDate, OwnerNames)
                    Dim Result As ReportResult = Reports.GenerateSalesReportBodyHTML(Reports.GenerateTransactionSalesAnalysis(ReportData))
                    strHTML &= Result.HTML

                Case "Transaction Purchases Report"
                    Dim ReportData As DataSet = Reports.GetTransactionReportData(StartDate, EndDate, OwnerNames)
                    Dim Result As ReportResult = Reports.GeneratePurchasesReportBodyHTML(Reports.GenerateTransactionPurchasesAnalysis(ReportData))
                    strHTML &= Result.HTML

                Case "Transaction Trading Report"
                    Dim ReportData As DataSet = Reports.GetTransactionReportData(StartDate, EndDate, OwnerNames)
                    Dim Result As ReportResult = Reports.GenerateTradingProfitReportBodyHTML(Reports.GenerateTransactionProfitAnalysis(ReportData))
                    strHTML &= Result.HTML

            End Select

            ' Save and navigate to the report
            Dim ReportFileName As String = Path.Combine(EveHQ.Core.HQ.reportFolder, ReportName.Replace(" ", "") & ".html")
            Dim sw As StreamWriter = New StreamWriter(ReportFileName)
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()
            wbReport.Navigate(ReportFileName)

        Else
            MessageBox.Show("You must select a report type before generating a report!", "Report Type Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub


#End Region

#Region "Chart Routines"

    Private Sub InitialiseCharts()

        ' Update the report combobox with the reports

        cboChart.BeginUpdate()
        cboChart.Items.Clear()

        ' Add the report types here in 
        cboChart.Items.Add("Wallet Balance History")

        ' Finalise the report combobox update
        cboChart.EndUpdate()

        ' Set the dates
        dtiChartEndDate.Value = Now
        dtiChartStartDate.Value = Now.AddMonths(-1)

        cboChartOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, True, cboChartOwners)

    End Sub

    Private Sub btnGenerateChart_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerateChart.Click

        Call Me.GenerateChart()

    End Sub

    Private Sub GenerateChart()

        If cboChart.SelectedItem IsNot Nothing Then

            ' Set the start and end date
            Dim StartDate As Date = dtiChartStartDate.Value
            Dim EndDate As Date = dtiChartEndDate.Value.AddDays(1) ' Add 1 to the date so we can check everything less than it

            ' Build the Owners List
            Dim OwnerNames As New List(Of String)
            Dim OwnerList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboChartOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                OwnerNames.Add(LVI.Name)
            Next

            ' Get the chart name
            Dim ChartName As String = cboChart.SelectedItem.ToString

            ' Choose what report is selected and get the information
            Select Case ChartName

                Case "Wallet Balance History"
                    Dim ReportData As DataSet = Reports.GetJournalReportData(StartDate, EndDate, OwnerNames)
                    Reports.GenerateWalletBalanceHistoryAnalysisChart(zgcPrism, Reports.GenerateWalletBalanceHistoryAnalysis(ReportData))

            End Select

            ' Display the chart
            zgcPrism.Visible = True

        Else
            MessageBox.Show("You must select a chart type before generating a chart!", "Chart Type Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#End Region

#Region "Rig Builder Routines"
    Private Sub GetSalvage()

        Dim Owner As New PrismOwner
        SalvageList.Clear()

        For Each cOwner As ListViewItem In PSCRigOwners.ItemList.CheckedItems

            If PlugInData.PrismOwners.ContainsKey(cOwner.Text) = True Then
                Owner = PlugInData.PrismOwners(cOwner.Text)
                Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Assets)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Assets)

                If OwnerAccount IsNot Nothing Then

                    Dim AssetXML As New XmlDocument
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    If Owner.IsCorp = True Then
                        AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    Else
                        AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    End If

                    If AssetXML IsNot Nothing Then
                        Dim locList As XmlNodeList
                        Dim loc As XmlNode
                        locList = AssetXML.SelectNodes("/eveapi/result/rowset/row")
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
                                    Call Me.GetSalvageNode(SalvageList, loc)
                                End If
                            Next
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub GetSalvageNode(ByVal SalvageList As SortedList, ByVal loc As XmlNode)
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
                    Call Me.GetSalvageNode(SalvageList, subLoc)
                End If

            Catch ex As Exception
               
            End Try
        Next
    End Sub
    Private Sub PrepareRigData()
        ' Clear the build list
        adtRigBuildList.Nodes.Clear()

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
        adtRigs.BeginUpdate()
        adtRigs.Nodes.Clear()
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
                    Dim lviBP2 As New Node
                    lviBP2.Text = BP
                    Dim Qty As Integer = CInt(Int(minQuantity))
                    lviBP2.Cells.Add(New Cell(Qty.ToString("N0")))
                    lviBP2.Cells.Add(New Cell(rigCost.ToString("N2")))
                    lviBP2.Cells.Add(New Cell(buildCost.ToString("N2")))
                    lviBP2.Cells.Add(New Cell((rigCost - buildCost).ToString("N2")))
                    lviBP2.Cells.Add(New Cell((Qty * rigCost).ToString("N2")))
                    lviBP2.Cells.Add(New Cell((Qty * buildCost).ToString("N2")))
                    lviBP2.Cells.Add(New Cell((Qty * (rigCost - buildCost)).ToString("N2")))
                    lviBP2.Cells.Add(New Cell((Qty * (rigCost - buildCost) / (Qty * rigCost) * 100).ToString("N2")))

                    adtRigs.Nodes.Add(lviBP2)
                End If
            End If
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtRigs, 1, True, True)
        adtRigs.EndUpdate()
    End Sub
    Private Sub adtRigs_NodeDoubleClick(sender As Object, e As DevComponents.AdvTree.TreeNodeMouseEventArgs)
        Call AddRigToBuildList(e.Node)
        Call Me.GetBuildList()
        Call Me.CalculateRigBuildInfo()
    End Sub
    Private Sub adtRigBuildList_NodeDoubleClick(sender As Object, e As DevComponents.AdvTree.TreeNodeMouseEventArgs)
        Call RemoveRigFromBuildList(e.Node)
        Call Me.GetBuildList()
        Call Me.CalculateRigBuildInfo()
    End Sub
    Private Sub AddRigToBuildList(ByVal currentRig As Node)
        Dim newRig As New Node(currentRig.Text)
        ' Add the selected rig to the build list
        adtRigBuildList.Nodes.Add(newRig)
        ' Copy details
        For subI As Integer = 1 To currentRig.Cells.Count - 1
            newRig.Cells.Add(New Cell(currentRig.Cells(subI).Text))
        Next
        'Get the salvage used by the rig and reduce the main list
        Dim RigSalvageList As SortedList = CType(RigBPData(currentRig.Text), Collections.SortedList)
        For Each salvage As String In RigSalvageList.Keys
            SalvageList(salvage) = CInt(SalvageList(salvage)) - (CInt(RigSalvageList(salvage)) * CInt(currentRig.Cells(1).Text))
        Next
    End Sub
    Private Sub RemoveRigFromBuildList(ByVal currentRig As Node)
        ' Remove the selected rig to the build list
        adtRigBuildList.Nodes.Remove(currentRig)
        ' Get the salvage used by the rig and reduce the main list
        Dim RigSalvageList As SortedList = CType(RigBPData(currentRig.Text), Collections.SortedList)
        For Each salvage As String In RigSalvageList.Keys
            SalvageList(salvage) = CInt(SalvageList(salvage)) + (CInt(RigSalvageList(salvage)) * CInt(currentRig.Cells(1).Text))
        Next
    End Sub
    Private Sub chkRigSaleprice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRigSalePrice.CheckedChanged
        If chkRigSalePrice.Checked = True Then
            btnAutoRig.Tag = 3
        End If
    End Sub
    Private Sub chkRigProfit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRigProfit.CheckedChanged
        If chkRigProfit.Checked = True Then
            btnAutoRig.Tag = 5
        End If
    End Sub
    Private Sub chkRigMargin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRigMargin.CheckedChanged
        If chkRigMargin.Checked = True Then
            btnAutoRig.Tag = 9
        End If
    End Sub
    Private Sub chkTotalSalePrice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTotalSalePrice.CheckedChanged
        If chkTotalSalePrice.Checked = True Then
            btnAutoRig.Tag = 6
        End If
    End Sub
    Private Sub chkTotalProfit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTotalProfit.CheckedChanged
        If chkTotalProfit.Checked = True Then
            btnAutoRig.Tag = 8
        End If
    End Sub
    Private Sub CalculateRigBuildInfo()
        Dim totalRSP, totalRP As Double
        For Each rigItem As Node In adtRigBuildList.Nodes
            totalRSP += CDbl(rigItem.Cells(5).Text)
            totalRP += CDbl(rigItem.Cells(7).Text)
        Next
        lblTotalRigSalePrice.Text = "Total Rig Sale Price: " & FormatNumber(totalRSP, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblTotalRigProfit.Text = "Total Rig Profit: " & FormatNumber(totalRP, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblTotalRigMargin.Text = "Margin: " & FormatNumber(totalRP / totalRSP * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
    End Sub
    Private Sub btnAutoRig_Click(sender As System.Object, e As System.EventArgs) Handles btnAutoRig.Click
        ' Get the rig and salvage info
        Call Me.PrepareRigData()
        ' Get the list of available rigs
        Call Me.GetBuildList()
        Do While adtRigs.Nodes.Count > 0
            EveHQ.Core.AdvTreeSorter.Sort(adtRigs, New EveHQ.Core.AdvTreeSortResult(CInt(btnAutoRig.Tag), Core.AdvTreeSortOrder.Descending), False)
            AddRigToBuildList(adtRigs.Nodes(0))
            Call Me.GetBuildList()
        Loop
        EveHQ.Core.AdvTreeSorter.Sort(adtRigBuildList, New EveHQ.Core.AdvTreeSortResult(CInt(btnAutoRig.Tag), Core.AdvTreeSortOrder.Descending), False)
        Call Me.CalculateRigBuildInfo()
    End Sub
    Private Sub btnBuildRigs_Click(sender As System.Object, e As System.EventArgs) Handles btnBuildRigs.Click

        ' Get the rig and salvage info
        Call Me.PrepareRigData()

        ' Get the list of available rigs
        Call Me.GetBuildList()
    End Sub
    Private Sub btnExportRigList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportRigList.Click
        Call Me.GenerateCSVFileFromCLV(PSCRigOwners.cboHost.Text, "Rig List", adtRigs)
    End Sub
    Private Sub btnExportRigBuildList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportRigBuildList.Click
        Call Me.GenerateCSVFileFromCLV(PSCRigOwners.cboHost.Text, "Rig Build List", adtRigBuildList)
    End Sub
    Private Sub adtRigs_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtRigs.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub
    Private Sub adtRigBuildList_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtRigBuildList.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub
#End Region

#Region "Invention Results Routines"

    Private Sub InitialiseInventionResults()
        ' Prepare info
        dtiInventionEndDate.Value = Now
        dtiInventionStartDate.Value = Now.AddMonths(-1)
        cboInventionInstallers.DropDownControl = New PrismSelectionControl(PrismSelectionType.InventionInstallers, True, cboInventionInstallers)
        cboInventionItems.DropDownControl = New PrismSelectionControl(PrismSelectionType.InventionItems, True, cboInventionItems)
    End Sub

    Private Sub DisplayInventionResults()
        Dim strSQL As String = "SELECT * FROM inventionResults"
        strSQL &= " WHERE inventionResults.resultDate >= '" & dtiInventionStartDate.Value.ToString(IndustryTimeFormat, culture) & "' AND inventionResults.resultDate <= '" & dtiInventionEndDate.Value.ToString(IndustryTimeFormat, culture) & "'"

        ' Build the Owners List
        If cboInventionInstallers.Text <> "<All>" Then
            Dim OwnerList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboInventionInstallers.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                OwnerList.Append(", '" & LVI.Name.Replace("'", "''") & "'")
            Next
            If OwnerList.Length > 2 Then
                OwnerList.Remove(0, 2)
            End If
            ' Default to None
            strSQL &= " AND inventionResults.installerName IN (" & OwnerList.ToString & ")"
        End If

        ' Filter item type
        If cboInventionItems.Text <> "All" Then
            ' Build a ref type list
            Dim ItemTypeList As New StringBuilder
            For Each LVI As ListViewItem In CType(cboInventionItems.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                ItemTypeList.Append(", '" & LVI.Name.Replace("'", "''") & "'")
            Next
            If ItemTypeList.Length > 2 Then
                ItemTypeList.Remove(0, 2)
                strSQL &= " AND inventionResults.typeName IN (" & ItemTypeList.ToString & ")"
            End If
        End If

        ' Order the data
        strSQL &= " ORDER BY inventionResults.resultDate ASC;"

        ' Get the data
        Dim JobList As SortedList(Of Long, InventionAPIJob) = InventionAPIJob.ParseInventionJobsFromDB(strSQL)

        ' Populate the list
        adtInventionResults.BeginUpdate()
        adtInventionResults.Nodes.Clear()

        If JobList.Count > 0 Then

            For Each job As InventionAPIJob In JobList.Values
                Dim JobItem As New Node
                JobItem.Name = job.JobID.ToString
                JobItem.Text = job.ResultDate.ToString
                JobItem.Cells.Add(New Cell(job.TypeName))
                JobItem.Cells.Add(New Cell(job.InstallerName))
                Select Case job.result
                    Case 1
                        JobItem.Cells.Add(New Cell("Successful"))
                    Case Else
                        JobItem.Cells.Add(New Cell("Failed"))
                End Select
                adtInventionResults.Nodes.Add(JobItem)
            Next
            adtInventionResults.Enabled = True
        Else
            adtInventionResults.Nodes.Add(New Node("No Data Available..."))
            adtInventionResults.Enabled = False
        End If

        adtInventionResults.EndUpdate()

        ' Update the Stats
        Call Me.DisplayInventionStats(JobList)

    End Sub

    Private Sub DisplayInventionStats(JobList As SortedList(Of Long, InventionAPIJob))

        adtInventionStats.BeginUpdate()
        adtInventionStats.Nodes.Clear()

        ' Clear and add default column
        adtInventionStats.Columns.Clear()
        Dim TypeNameCol As New DevComponents.AdvTree.ColumnHeader
        TypeNameCol.Name = "TypeName"
        TypeNameCol.Text = "Item Type"
        TypeNameCol.Width.Absolute = 250
        TypeNameCol.DisplayIndex = 1
        adtInventionStats.Columns.Add(TypeNameCol)

        ' Get the Invention Stats
        Dim Stats As SortedList(Of String, SortedList(Of String, InventionResults)) = InventionAPIJob.CalculateInventionStats(JobList)

        If Stats.Count > 0 Then

            ' Add columns and rows based
            Dim ColIdx As Integer = 0
            For Each installerName As String In Stats.Keys
                Dim ICol As New DevComponents.AdvTree.ColumnHeader
                ColIdx += 1
                ICol.Name = installerName
                ICol.Text = installerName
                ICol.Width.Absolute = 150
                ICol.DisplayIndex = ColIdx + 1
                ICol.EditorType = eCellEditorType.Custom
                adtInventionStats.Columns.Add(ICol)

                ' Check for modules
                For Each TypeName As String In Stats(installerName).Keys
                    ' Check it doesn't already exist
                    Dim TypeNode As Node = adtInventionStats.FindNodeByName(TypeName)
                    If TypeNode Is Nothing Then
                        ' Node doesn't exist, so add it
                        TypeNode = New Node
                        TypeNode.Name = TypeName
                        TypeNode.Text = TypeName
                        adtInventionStats.Nodes.Add(TypeNode)
                    End If
                Next
            Next

            ' Add the "Average Column"
            Dim AvgCol As New DevComponents.AdvTree.ColumnHeader
            ColIdx += 1
            AvgCol.Name = "Item Average"
            AvgCol.Text = "Item Average"
            AvgCol.Width.Absolute = 150
            AvgCol.DisplayIndex = ColIdx + 1
            AvgCol.EditorType = eCellEditorType.Custom
            adtInventionStats.Columns.Add(AvgCol)

            ' Populate the grid with blank cells
            For n As Integer = 0 To adtInventionStats.Nodes.Count - 1
                For c As Integer = 1 To adtInventionStats.Columns.Count - 1
                    adtInventionStats.Nodes(n).Cells.Add(New Cell("n/a"))
                    adtInventionStats.Nodes(n).Cells(c).Tag = -1
                Next
            Next

            ' Populate the grid with proper data
            Dim TypeAvgs As New SortedList(Of String, InventionResults)
            ColIdx = 0
            For Each installerName As String In Stats.Keys
                ColIdx += 1

                ' Check for modules
                For Each TypeName As String In Stats(installerName).Keys
                    ' Check it doesn't already exist
                    Dim TypeNode As Node = adtInventionStats.FindNodeByName(TypeName)
                    If TypeNode IsNot Nothing Then
                        ' Get the results
                        Dim Results As InventionResults = Stats(installerName).Item(TypeName)
                        Dim Percent As Double = Results.Successes / (Results.Successes + Results.Failures) * 100
                        TypeNode.Cells(ColIdx).Text = Results.Successes.ToString & " / " & (Results.Successes + Results.Failures).ToString & " (" & Percent.ToString("N2") & "%)"
                        TypeNode.Cells(ColIdx).Tag = Percent
                        ' Add results to the averages
                        Dim TypeAvg As New InventionResults
                        If TypeAvgs.ContainsKey(TypeName) = True Then
                            TypeAvg = TypeAvgs(TypeName)
                        Else
                            TypeAvgs.Add(TypeName, TypeAvg)
                        End If
                        TypeAvg.Successes += Results.Successes
                        TypeAvg.Failures += Results.Failures
                    End If
                Next
            Next

            ' Display Averages
            ColIdx = adtInventionStats.Columns.Count - 1
            For Each TypeNode As Node In adtInventionStats.Nodes
                If TypeAvgs.ContainsKey(TypeNode.Name) = True Then
                    Dim Results As InventionResults = TypeAvgs(TypeNode.Name)
                    Dim Percent As Double = Results.Successes / (Results.Successes + Results.Failures) * 100
                    TypeNode.Cells(ColIdx).Text = Results.Successes.ToString & " / " & (Results.Successes + Results.Failures).ToString & " (" & Percent.ToString("N2") & "%)"
                    TypeNode.Cells(ColIdx).Tag = Percent
                End If
            Next

            EveHQ.Core.AdvTreeSorter.Sort(adtInventionStats, 1, False, False)

            adtInventionStats.Enabled = True
        Else
            adtInventionStats.Nodes.Add(New Node("No Data Available..."))
            adtInventionStats.Enabled = False
        End If

        adtInventionStats.EndUpdate()

    End Sub

    Private Sub btnGetResults_Click(sender As System.Object, e As System.EventArgs) Handles btnGetResults.Click
        Call Me.DisplayInventionResults()
    End Sub

    Private Sub dtiInventionStartDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiInventionStartDate.ButtonCustom2Click
        dtiInventionStartDate.Value = New Date(dtiInventionStartDate.Value.Year, dtiInventionStartDate.Value.Month, dtiInventionStartDate.Value.Day)
    End Sub

    Private Sub dtiInventionStartDate_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiInventionStartDate.ButtonCustomClick
        dtiInventionStartDate.Value = Now
    End Sub

    Private Sub dtiInventionEndDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiInventionEndDate.ButtonCustom2Click
        dtiInventionEndDate.Value = New Date(dtiInventionEndDate.Value.Year, dtiInventionEndDate.Value.Month, dtiInventionEndDate.Value.Day)
    End Sub

    Private Sub dtiInventionEndDate_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtiInventionEndDate.ButtonCustomClick
        dtiInventionEndDate.Value = Now
    End Sub

    Private Sub adtInventionResults_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtInventionResults.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, False, False)
    End Sub

    Private Sub adtInventionStats_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtInventionStats.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, False, False)
    End Sub

#End Region

    

   
End Class
