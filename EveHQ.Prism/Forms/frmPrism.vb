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
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web
Imports System.Windows.Forms
Imports System.Xml
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.Core
Imports EveHQ.Core.Requisitions
Imports EveHQ.EveAPI
Imports EveHQ.EveData
Imports EveHQ.Prism.BPCalc
Imports EveHQ.Prism.Classes
Imports EveHQ.Prism.Controls
Imports SearchOption = Microsoft.VisualBasic.FileIO.SearchOption

Namespace Forms

    Public Class FrmPrism

#Region "Class Wide Variables"

        Private Const PrismTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        ReadOnly _culture As CultureInfo = New CultureInfo("en-GB")

        Dim _startup As Boolean = True
        Dim _selectedTab As TabItem
        ReadOnly _divisions As New SortedList
        Dim _prismThreadMax As Integer = 16
        Dim _prismThreadCurrent As Integer = 0
        Private Const MaxAPIRetries As Integer = 3
        Private Const MaxAPIJournals As Integer = 2000

        Dim _bpManagerUpdate As Boolean = False
        ReadOnly _bpLocations As List(Of String) = New List(Of String)()

        ' Rig Builder Variables
        Dim _rigBPData As New SortedList(Of String, SortedList(Of Integer, Long))
        Dim _rigBuildData As New SortedList(Of Integer, Long)
        ReadOnly _salvageList As New SortedList

        ' Recycling Variables
        Dim _recyclerAssetList As New SortedList(Of Integer, Long)
        Dim _recyclerAssetOwner As String = ""
        Dim _recyclerAssetLocation As Integer
        ReadOnly _itemList As New SortedList(Of Integer, SortedList(Of String, Long))
        Dim _matList As New SortedList(Of String, Long)
        Dim _baseYield As Double = 0.5
        Dim _netYield As Double = 0
        Dim _stationYield As Double = 0
        Dim _stationTake As Double = 0
        Dim _stationStanding As Double = 0
        Dim _rBrokerFee As Double = 0
        Dim _rTransTax As Double = 0
        Dim _rTotalFees As Double = 0

        ' Node Element Styles
        Dim _styleRed As New ElementStyle
        Dim _styleRedRight As New ElementStyle
        Dim _styleGreen As New ElementStyle
        Dim _styleGreenRight As New ElementStyle
        Dim _styleRight As New ElementStyle

        ' BPManager Styles
        Dim _bpmStyleUnknown As ElementStyle
        Dim _bpmStyleBpo As ElementStyle
        Dim _bpmStyleBPC As ElementStyle
        Dim _bpmStyleUser As ElementStyle
        Dim _bpmStyleMissing As ElementStyle
        Dim _bpmStyleExhausted As ElementStyle

        Delegate Sub CheckXMLDelegate(ByVal apiXML As XmlDocument, ByVal xmlOwner As PrismOwner, ByVal apiType As CorpRepType)
        Private _xmlDelegate As CheckXMLDelegate

        Friend Shared LockObj As New Object()

#End Region

#Region "Form Initialisation Routines"

        Private Sub frmPrism_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            _xmlDelegate = New CheckXMLDelegate(AddressOf CheckXML)

            ' Add events
            AddHandler PrismEvents.UpdateProductionJobs, AddressOf UpdateProductionJobList
            AddHandler PrismEvents.UpdateInventionJobs, AddressOf UpdateInventionJobList
            AddHandler PrismEvents.UpdateBatchJobs, AddressOf UpdateBatchList
            AddHandler PrismEvents.RecyclingInfoAvailable, AddressOf RecycleInfoFromAssets

            ' Load the settings!
            Call PrismSettings.UserSettings.LoadPrismSettings()

            ' Load the Production Jobs
            Call Jobs.Load()
            ' Load the Batch Jobs
            Call BatchJobs.LoadBatchJobs()

            tabPrism.Dock = DockStyle.Fill

            _startup = True

            ' Create the styles
            _styleRed = adtJournal.Styles("ElementStyle1").Copy
            _styleRed.TextColor = Color.Red
            _styleRedRight = adtJournal.Styles("ElementStyle1").Copy
            _styleRedRight.TextColor = Color.Red
            _styleRedRight.TextAlignment = eStyleTextAlignment.Far
            _styleGreen = adtJournal.Styles("ElementStyle1").Copy
            _styleGreen.TextColor = Color.LimeGreen
            _styleGreenRight = adtJournal.Styles("ElementStyle1").Copy
            _styleGreenRight.TextColor = Color.LimeGreen
            _styleGreenRight.TextAlignment = eStyleTextAlignment.Far
            _styleRight = adtJournal.Styles("ElementStyle1").Copy
            _styleRight.TextAlignment = eStyleTextAlignment.Far

            ' Create BPM Styles
            _bpmStyleBPC = adtBlueprints.Styles("BP").Copy
            _bpmStyleBpo = adtBlueprints.Styles("BP").Copy
            _bpmStyleExhausted = adtBlueprints.Styles("BP").Copy
            _bpmStyleMissing = adtBlueprints.Styles("BP").Copy
            _bpmStyleUnknown = adtBlueprints.Styles("BP").Copy
            _bpmStyleUser = adtBlueprints.Styles("BP").Copy
            _bpmStyleBPC.BackColor2 = Color.LightSteelBlue
            _bpmStyleBPC.BackColor = Color.FromArgb(128, _bpmStyleBPC.BackColor2)
            _bpmStyleBpo.BackColor2 = Color.LightGreen
            _bpmStyleBpo.BackColor = Color.FromArgb(128, _bpmStyleBpo.BackColor2)
            _bpmStyleExhausted.BackColor2 = Color.Orange
            _bpmStyleExhausted.BackColor = Color.FromArgb(128, _bpmStyleExhausted.BackColor2)
            _bpmStyleMissing.BackColor2 = Color.LightCoral
            _bpmStyleMissing.BackColor = Color.FromArgb(128, _bpmStyleMissing.BackColor2)
            _bpmStyleUnknown.BackColor2 = Color.LightGray
            _bpmStyleUnknown.BackColor = Color.FromArgb(128, _bpmStyleUnknown.BackColor2)
            _bpmStyleUser.BackColor2 = Color.Yellow
            _bpmStyleUser.BackColor = Color.FromArgb(128, _bpmStyleUser.BackColor2)

            ' Build a corp list
            Call BuildOwnerList()

            ' Hide excess tabs
            For tabNo As Integer = 1 To tabPrism.Tabs.Count - 1
                tabPrism.Tabs(tabNo).Visible = False
            Next

            ' Initialise the Report data
            Call InitialiseReports()

            ' Initialise the Journal and Transaction Data
            Call InitialiseJournal()
            Call InitialiseTransactions()
            Call InitialiseInventionResults()

            ' Build the BP Manager category lists
            cboCategoryFilter.BeginUpdate()
            cboCategoryFilter.Items.Clear()
            cboCategoryFilter.Items.Add("All")
            For Each cat As String In PlugInData.CategoryNames.Keys
                cboCategoryFilter.Items.Add(cat)
            Next
            cboCategoryFilter.EndUpdate()

            ' Build the Blueprint Activity List
            cboActivityFilter.BeginUpdate()
            cboActivityFilter.Items.Clear()
            cboActivityFilter.Items.Add("<All>")
            For Each activity As String In [Enum].GetNames(GetType(BlueprintActivity))
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

            Call ScanForExistingXMLFiles()

            ' Initialise default Prism character
            If PrismSettings.UserSettings.DefaultCharacter <> "" And PlugInData.PrismOwners.ContainsKey(PrismSettings.UserSettings.DefaultCharacter) Then
                ' Wallet Journal
                CType(cboJournalOwners.DropDownControl, PrismSelectionControl).UpdateList()
                ' Wallet Transactions
                CType(cboTransactionOwner.DropDownControl, PrismSelectionControl).UpdateList()
                ' Assets
                CType(PAC.PSCAssetOwners.cboHost.DropDownControl, PrismSelectionControl).UpdateList()
                ' Market Orders
                cboOrdersOwner.SelectedItem = PrismSettings.UserSettings.DefaultCharacter
                ' Research Jobs
                cboJobOwner.SelectedItem = PrismSettings.UserSettings.DefaultCharacter
                ' BP Manager
                cboBPOwner.SelectedItem = PrismSettings.UserSettings.DefaultCharacter
                ' Contracts
                cboContractOwner.SelectedItem = PrismSettings.UserSettings.DefaultCharacter
            End If

            ' Set the refining info
            ' Set the pilot to the recycling one
            If cboRecyclePilots.Items.Contains(_recyclerAssetOwner) Then
                cboRecyclePilots.SelectedItem = _recyclerAssetOwner
            Else
                If cboRecyclePilots.Items.Count > 0 Then
                    cboRecyclePilots.SelectedIndex = 0
                End If
            End If

            ' Set the recycling mode
            cboRefineMode.SelectedIndex = 0
            _startup = False

            ' Start the update timer
            tmrUpdateInfo.Enabled = True
            tmrUpdateInfo.Start()

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

            For Each selAccount As EveHQAccount In HQ.Settings.Accounts.Values
                Select Case selAccount.ApiKeySystem
                    Case APIKeySystems.Version2
                        ' Check the type of the key
                        Select Case selAccount.APIKeyType
                            Case APIKeyTypes.Corporation
                                ' A corporate API key
                                For Each xmlOwner As String In selAccount.Characters
                                    If HQ.Settings.Corporations.ContainsKey(xmlOwner) Then
                                        Dim selCorp As Corporation = HQ.Settings.Corporations(xmlOwner)
                                        If StaticData.NpcCorps.ContainsKey(CInt(selCorp.ID)) = False Then
                                            If PlugInData.PrismOwners.ContainsKey(xmlOwner) = False Then
                                                Dim newOwner As New PrismOwner
                                                newOwner.Account = selAccount
                                                newOwner.Name = selCorp.Name
                                                newOwner.ID = CStr(selCorp.ID)
                                                newOwner.IsCorp = True
                                                newOwner.APIVersion = APIKeySystems.Version2
                                                PlugInData.PrismOwners.Add(newOwner.Name, newOwner)
                                            End If
                                            ' Add the corp to the CorpList
                                            PlugInData.CorpList.Add(selCorp.Name, CInt(selCorp.ID))
                                        End If
                                    End If
                                Next
                            Case APIKeyTypes.Account, APIKeyTypes.Character
                                ' A character related API key
                                For Each xmlOwner As String In selAccount.Characters
                                    If HQ.Settings.Pilots.ContainsKey(xmlOwner) Then
                                        Dim selPilot As EveHQPilot = HQ.Settings.Pilots(xmlOwner)
                                        If PlugInData.PrismOwners.ContainsKey(xmlOwner) = False Then
                                            Dim newOwner As New PrismOwner
                                            newOwner.Account = selAccount
                                            newOwner.Name = selPilot.Name
                                            newOwner.ID = selPilot.ID
                                            newOwner.IsCorp = False
                                            newOwner.APIVersion = APIKeySystems.Version2
                                            PlugInData.PrismOwners.Add(newOwner.Name, newOwner)
                                        End If
                                    End If
                                Next
                        End Select
                End Select
            Next
        End Sub

        ''' <summary>
        ''' Looks at existing XML files to determine the cache status
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ScanForExistingXMLFiles()

            lvwCurrentAPIs.BeginUpdate()
            lvwCurrentAPIs.Items.Clear()

            ' Cycle through our list of Prism owners and set up the API status matrix
            ' We have already checked the account active status and the pilot active status so no need to do this again
            For Each xmlOwner As PrismOwner In PlugInData.PrismOwners.Values
                If lvwCurrentAPIs.Items.ContainsKey(xmlOwner.ID) = False Then
                    Dim newOwner As New ListViewItem
                    newOwner.UseItemStyleForSubItems = False
                    newOwner.Name = xmlOwner.ID
                    newOwner.Text = xmlOwner.Name
                    newOwner.ToolTipText = ""
                    ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                    For si As Integer = 1 To 9
                        newOwner.SubItems.Add("")
                    Next
                    Select Case xmlOwner.IsCorp
                        Case True
                            newOwner.SubItems(1).Text = "Corporation"
                        Case False
                            newOwner.SubItems(1).Text = "Character"
                            newOwner.SubItems(9).Text = "n/a"
                            cboRecyclePilots.Items.Add(xmlOwner.Name)
                    End Select
                    lvwCurrentAPIs.Items.Add(newOwner)
                End If
            Next

            For Each xmlOwner As PrismOwner In PlugInData.PrismOwners.Values
                Call CheckXMLFiles(xmlOwner)
            Next

            lvwCurrentAPIs.EndUpdate()
            Call CompleteAPIUpdate()
        End Sub

        ''' <summary>
        ''' Checks existing XML files for display on Prism startup
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CheckXMLFiles(ByVal pOwner As PrismOwner)
            Select Case pOwner.IsCorp
                Case True
                    Call CheckCorpXMLFiles(pOwner)
                Case False
                    Call CheckCharXMLFiles(pOwner)
            End Select
        End Sub

        Private Sub CheckCharXMLFiles(ByVal pOwner As PrismOwner)

            If pOwner.IsCorp = False Then
                If HQ.Settings.Pilots.ContainsKey(pOwner.Name) = True Then
                    Dim selPilot As EveHQPilot = HQ.Settings.Pilots(pOwner.Name)
                    Dim pilotAccount As EveHQAccount = pOwner.Account

                    Dim apixml As XmlDocument
                    Const ReturnMethod As APIReturnMethods = APIReturnMethods.ReturnCacheOnly
                    Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                    ' Check for char assets
                    apixml = apireq.GetAPIXML(APITypes.AssetsChar, pilotAccount.ToAPIAccount, selPilot.ID, ReturnMethod)
                    Call CheckXML(apixml, pOwner, CorpRepType.Assets)

                    ' Check for char balances
                    apixml = apireq.GetAPIXML(APITypes.AccountBalancesChar, pilotAccount.ToAPIAccount, selPilot.ID, ReturnMethod)
                    Call CheckXML(apixml, pOwner, CorpRepType.Balances)

                    ' Check for char jobs
                    apixml = apireq.GetAPIXML(APITypes.IndustryChar, pilotAccount.ToAPIAccount, selPilot.ID, ReturnMethod)
                    Call CheckXML(apixml, pOwner, CorpRepType.Jobs)

                    ' Check for char journal
                    apixml = apireq.GetAPIXML(APITypes.WalletJournalChar, pilotAccount.ToAPIAccount, selPilot.ID, 1000, 0, 256, ReturnMethod)
                    Call CheckXML(apixml, pOwner, CorpRepType.WalletJournal)

                    ' Check for char orders
                    apixml = apireq.GetAPIXML(APITypes.OrdersChar, pilotAccount.ToAPIAccount, selPilot.ID, ReturnMethod)
                    Call CheckXML(apixml, pOwner, CorpRepType.Orders)

                    ' Check for char transactions
                    apixml = apireq.GetAPIXML(APITypes.WalletTransChar, pilotAccount.ToAPIAccount, selPilot.ID, 1000, "", ReturnMethod)
                    Call CheckXML(apixml, pOwner, CorpRepType.WalletTransactions)

                    ' Check for char contracts
                    apixml = apireq.GetAPIXML(APITypes.ContractsChar, pilotAccount.ToAPIAccount, selPilot.ID, ReturnMethod)
                    Call CheckXML(apixml, pOwner, CorpRepType.Contracts)

                    ' Check for corp sheets
                    If PrismSettings.UserSettings.CorpReps.ContainsKey(selPilot.Corp) Then
                        If PrismSettings.UserSettings.CorpReps(selPilot.Corp).ContainsKey(CorpRepType.CorpSheet) Then
                            If PrismSettings.UserSettings.CorpReps(selPilot.Corp).Item(CorpRepType.CorpSheet) = selPilot.Name Then
                                apixml = apireq.GetAPIXML(APITypes.CorpSheet, pilotAccount.ToAPIAccount, selPilot.ID, ReturnMethod)
                                Call CheckXML(apixml, pOwner, CorpRepType.CorpSheet)
                            Else

                            End If
                        Else

                        End If
                    Else

                    End If

                End If
            End If
        End Sub

        Private Sub CheckCorpXMLFiles(ByVal pOwner As PrismOwner)

            If pOwner.IsCorp = True Then

                Dim corpAccount As EveHQAccount = pOwner.Account

                Dim apixml As XmlDocument
                Const ReturnMethod As APIReturnMethods = APIReturnMethods.ReturnCacheOnly
                Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
                Dim ownerID As String

                ' Check for corp assets
                ownerID = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Assets)
                apixml = apireq.GetAPIXML(APITypes.AssetsCorp, corpAccount.ToAPIAccount, ownerID, ReturnMethod)
                Call CheckXML(apixml, pOwner, CorpRepType.Assets)

                ' Check for corp balances
                ownerID = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Balances)
                apixml = apireq.GetAPIXML(APITypes.AccountBalancesCorp, corpAccount.ToAPIAccount, ownerID, ReturnMethod)
                Call CheckXML(apixml, pOwner, CorpRepType.Balances)

                ' Check for corp jobs
                ownerID = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Jobs)
                apixml = apireq.GetAPIXML(APITypes.IndustryCorp, corpAccount.ToAPIAccount, ownerID, ReturnMethod)
                Call CheckXML(apixml, pOwner, CorpRepType.Jobs)

                ' Check for corp journal
                ownerID = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.WalletJournal)
                apixml = apireq.GetAPIXML(APITypes.WalletJournalCorp, corpAccount.ToAPIAccount, ownerID, 1000, 0, 256, ReturnMethod)
                Call CheckXML(apixml, pOwner, CorpRepType.WalletJournal)

                ' Check for corp orders
                ownerID = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Orders)
                apixml = apireq.GetAPIXML(APITypes.OrdersCorp, corpAccount.ToAPIAccount, ownerID, ReturnMethod)
                Call CheckXML(apixml, pOwner, CorpRepType.Orders)

                ' Check for corp transactions
                ownerID = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.WalletTransactions)
                apixml = apireq.GetAPIXML(APITypes.WalletTransCorp, corpAccount.ToAPIAccount, ownerID, 1000, "", ReturnMethod)
                Call CheckXML(apixml, pOwner, CorpRepType.WalletTransactions)

                ' Check for corp contracts
                ownerID = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Contracts)
                apixml = apireq.GetAPIXML(APITypes.ContractsCorp, corpAccount.ToAPIAccount, ownerID, ReturnMethod)
                Call CheckXML(apixml, pOwner, CorpRepType.Contracts)

                ' Check for corp sheets
                ownerID = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.CorpSheet)
                apixml = apireq.GetAPIXML(APITypes.CorpSheet, corpAccount.ToAPIAccount, ownerID, ReturnMethod)
                Call CheckXML(apixml, pOwner, CorpRepType.CorpSheet)

            End If

        End Sub

        Private Sub btnRefreshAPI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshAPI.Click
            _startup = True
            Call ScanForExistingXMLFiles()
            _startup = False
        End Sub

#End Region

#Region "Form Closing Routines"

        Private Sub frmPrism_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
            ' Save data and settings
            Call SaveAll()

            ' Remove events
            RemoveHandler PrismEvents.UpdateProductionJobs, AddressOf UpdateProductionJobList
            RemoveHandler PrismEvents.UpdateBatchJobs, AddressOf UpdateBatchList
        End Sub

        Public Sub SaveAll()

            ' Save the Owner Blueprints
            Call PlugInData.SaveOwnerBlueprints()

            ' Save the Production Jobs
            Call Jobs.Save()

            ' Save the Batch Jobs
            Call BatchJobs.SaveBatchJobs()

            ' Save the settings
            Call PrismSettings.UserSettings.SavePrismSettings()

        End Sub

#End Region

#Region "XML Retrieval and Parsing"

        Private Sub StartGetXMLDataThread()
            ' Perform this so that the API download process doesn't block the main UI thread
            GetXMLData()
        End Sub

        Private Sub GetXMLData()

            _prismThreadMax = 16
            _prismThreadCurrent = 0


            ' Setup separate threads for getting each type of API
            ThreadPool.QueueUserWorkItem(AddressOf GetCharAssets2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCorpAssets2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCharBalances2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCorpBalances2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCharJobs2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCorpJobs2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCharJournal2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCorpJournal2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCharOrders2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCorpOrders2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCharTransactions2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCorpTransactions2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCharContracts2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCorpContracts2)
            ThreadPool.QueueUserWorkItem(AddressOf GetCorpSheet2)
            ThreadPool.QueueUserWorkItem(AddressOf UpdateNullCorpSheet2)

        End Sub

        Private Sub CheckXML(ByVal apiXML As XmlDocument, ByVal pOwner As PrismOwner, ByVal apiType As CorpRepType)

            ' Get the listviewitem of the relevant Owner
            Dim apiOwner As ListViewItem = lvwCurrentAPIs.Items(pOwner.ID)
            ' Get the position of the cell in the listviewitem
            Dim pos As Integer = apiType + 2

            Try

                Select Case pOwner.APIVersion

                    Case APIKeySystems.Version2

                        ' Checking XML of APIv2 keys
                        If apiXML IsNot Nothing Then
                            If CanUseApiV2(pOwner, apiType) Then
                                Call DisplayAPIDetails(apiXML, apiOwner, pos)
                            Else
                                apiOwner.SubItems(pos).ForeColor = Color.Red
                                apiOwner.SubItems(pos).Text = "No Access"
                            End If
                        Else
                            If pOwner.IsCorp = False And apiType = CorpRepType.CorpSheet Then
                                apiOwner.SubItems(pos).ForeColor = Color.Black
                                apiOwner.SubItems(pos).Text = "n/a"
                            Else
                                If CanUseApiV2(pOwner, apiType) Then
                                    ' ...but we can use it (it's just missing for now)
                                    apiOwner.SubItems(pos).ForeColor = Color.Red
                                    apiOwner.SubItems(pos).Text = "Missing"
                                Else
                                    ' Put generic "No Access" notice here, but we could expand on this later
                                    apiOwner.SubItems(pos).ForeColor = Color.Red
                                    apiOwner.SubItems(pos).Text = "No Access"
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

        Private Sub DisplayAPIDetails(ByVal apiXML As XmlDocument, ByVal apiOwner As ListViewItem, ByVal pos As Integer)
            ' Check response string for any error codes?
            Dim errlist As XmlNodeList = apiXML.SelectNodes("/eveapi/error")
            If errlist.Count <> 0 Then
                Dim errNode As XmlNode = errlist(0)
                ' Get error code
                Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                apiOwner.SubItems(pos).ForeColor = Color.Red
                apiOwner.SubItems(pos).Text = errCode
            Else
                Dim cache As Date = CacheDate(apiXML)
                If cache <= Now Then
                    apiOwner.SubItems(pos).ForeColor = Color.Blue
                    apiOwner.SubItems(pos).Text = "Cache Expired!"
                Else
                    apiOwner.SubItems(pos).ForeColor = Color.Green
                    apiOwner.SubItems(pos).Text = Format(cache, "dd MMM HH:mm")
                End If
            End If
        End Sub

        Private Function CanUseAPI(ByVal pOwner As PrismOwner, ByVal apiType As CorpRepType) As Boolean
            Select Case pOwner.APIVersion
                Case APIKeySystems.Version2
                    Return CanUseApiV2(pOwner, apiType)
                Case Else
                    Return False
            End Select
        End Function

        Private Function CanUseApiV2(ByVal pOwner As PrismOwner, ByVal apiType As CorpRepType) As Boolean
            Dim account As EveHQAccount = pOwner.Account
            If account.ApiKeySystem = APIKeySystems.Version2 Then
                Select Case account.APIKeyType
                    Case APIKeyTypes.Corporation
                        Select Case apiType
                            Case CorpRepType.Assets
                                Return account.CanUseCorporateAPI(CorporateAccessMasks.AssetList)
                            Case CorpRepType.Balances
                                Return account.CanUseCorporateAPI(CorporateAccessMasks.AccountBalances)
                            Case CorpRepType.Contracts
                                Return account.CanUseCorporateAPI(CorporateAccessMasks.Contracts)
                            Case CorpRepType.CorpSheet
                                Return account.CanUseCorporateAPI(CorporateAccessMasks.CorporationSheet)
                            Case CorpRepType.Jobs
                                Return account.CanUseCorporateAPI(CorporateAccessMasks.IndustryJobs)
                            Case CorpRepType.Orders
                                Return account.CanUseCorporateAPI(CorporateAccessMasks.MarketOrders)
                            Case CorpRepType.WalletJournal
                                Return account.CanUseCorporateAPI(CorporateAccessMasks.WalletJournal)
                            Case CorpRepType.WalletTransactions
                                Return account.CanUseCorporateAPI(CorporateAccessMasks.WalletTransactions)
                        End Select
                    Case Else
                        Select Case apiType
                            Case CorpRepType.Assets
                                Return account.CanUseCharacterAPI(CharacterAccessMasks.AssetList)
                            Case CorpRepType.Balances
                                Return account.CanUseCharacterAPI(CharacterAccessMasks.AccountBalances)
                            Case CorpRepType.Contracts
                                Return account.CanUseCharacterAPI(CharacterAccessMasks.Contracts)
                            Case CorpRepType.CorpSheet
                                Return False
                            Case CorpRepType.Jobs
                                Return account.CanUseCharacterAPI(CharacterAccessMasks.IndustryJobs)
                            Case CorpRepType.Orders
                                Return account.CanUseCharacterAPI(CharacterAccessMasks.MarketOrders)
                            Case CorpRepType.WalletJournal
                                Return account.CanUseCharacterAPI(CharacterAccessMasks.WalletJournal)
                            Case CorpRepType.WalletTransactions
                                Return account.CanUseCharacterAPI(CharacterAccessMasks.WalletTransactions)
                        End Select
                End Select
            Else
                Return False
            End If
        End Function

        Private Sub CompleteAPIUpdate()
            ' Populate the various Owner boxes
            Call UpdatePrismOwners()
            ' Set the label, enable the button and inform the user
            lblCurrentAPI.Text = "Cached APIs Loaded:"
            btnDownloadAPIData.Enabled = True
            If _startup = False And PrismSettings.UserSettings.HideAPIDownloadDialog = False Then
                DisplayAPICompleteDialog()
            End If
        End Sub

        Private Sub DisplayAPICompleteDialog()
            TaskDialog.AntiAlias = True
            TaskDialog.EnableGlass = False
            Dim tdi As New TaskDialogInfo
            tdi.TaskDialogIcon = eTaskDialogIcon.CheckMark2
            tdi.DialogButtons = eTaskDialogButton.Ok
            tdi.DefaultButton = eTaskDialogButton.Ok
            tdi.Title = "API Download complete!"
            tdi.Header = "API Download complete!"
            tdi.Text = "Prism has completed the download of the API data. You may need to refresh your views to get updated information."
            tdi.DialogColor = eTaskDialogBackgroundColor.DarkBlue
            tdi.CheckBoxCommand = APIDownloadDialogCheckBox
            TaskDialog.Show(Me, tdi)
        End Sub

        Private Sub APIDownloadDialogCheckBox_Executed(ByVal sender As Object, ByVal e As EventArgs) Handles APIDownloadDialogCheckBox.Executed
            PrismSettings.UserSettings.HideAPIDownloadDialog = APIDownloadDialogCheckBox.Checked
        End Sub

        Private Sub UpdatePrismOwners()

            ' Check for old items
            Dim oldBPOwner As String = ""
            Dim oldJobOwner As String = ""
            Dim oldOrdersOwner As String = ""
            Dim oldContractOwner As String = ""

            If cboBPOwner.SelectedItem IsNot Nothing Then
                oldBPOwner = cboBPOwner.SelectedItem.ToString
            End If
            If cboJobOwner.SelectedItem IsNot Nothing Then
                oldJobOwner = cboJobOwner.SelectedItem.ToString
            End If
            If cboOrdersOwner.SelectedItem IsNot Nothing Then
                oldOrdersOwner = cboOrdersOwner.SelectedItem.ToString
            End If
            If cboContractOwner.SelectedItem IsNot Nothing Then
                oldContractOwner = cboContractOwner.SelectedItem.ToString
            End If

            ' Prepare each of the owner lists for loading
            cboBPOwner.BeginUpdate() : cboBPOwner.Items.Clear()
            cboJobOwner.BeginUpdate() : cboJobOwner.Items.Clear()
            cboOrdersOwner.BeginUpdate() : cboOrdersOwner.Items.Clear()
            cboContractOwner.BeginUpdate() : cboContractOwner.Items.Clear()

            ' Populate the lists
            For Each pOwner As String In PlugInData.PrismOwners.Keys
                cboBPOwner.Items.Add(pOwner)
                cboJobOwner.Items.Add(pOwner)
                cboOrdersOwner.Items.Add(pOwner)
                cboContractOwner.Items.Add(pOwner)
            Next

            ' Finalise the loading
            cboBPOwner.Sorted = True : cboBPOwner.EndUpdate()
            cboJobOwner.Sorted = True : cboJobOwner.EndUpdate()
            cboOrdersOwner.Sorted = True : cboOrdersOwner.EndUpdate()
            cboContractOwner.Sorted = True : cboContractOwner.EndUpdate()

            ' Set the old values if applicable
            If oldBPOwner <> "" And cboBPOwner.Items.Contains(oldBPOwner) Then
                cboBPOwner.SelectedItem = oldBPOwner
            End If
            If oldJobOwner <> "" And cboJobOwner.Items.Contains(oldJobOwner) Then
                cboJobOwner.SelectedItem = oldJobOwner
            End If
            If oldOrdersOwner <> "" And cboOrdersOwner.Items.Contains(oldOrdersOwner) Then
                cboOrdersOwner.SelectedItem = oldOrdersOwner
            End If
            If oldContractOwner <> "" And cboContractOwner.Items.Contains(oldContractOwner) Then
                cboContractOwner.SelectedItem = oldContractOwner
            End If

        End Sub

#Region "APIv2"

        Private Sub GetCharAssets2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = False Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = pOwner.Account
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(pOwner, CorpRepType.Assets) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim retries As Integer = 0
                            Do
                                retries += 1
                                apixml = apireq.GetAPIXML(APITypes.AssetsChar, pilotAccount.ToAPIAccount, pOwner.ID, APIReturnMethods.ReturnStandard)
                            Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Assets})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Character Assets data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCharBalances2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = False Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = pOwner.Account
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(pOwner, CorpRepType.Balances) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim retries As Integer = 0
                            Do
                                retries += 1
                                apixml = apireq.GetAPIXML(APITypes.AccountBalancesChar, pilotAccount.ToAPIAccount, pOwner.ID, APIReturnMethods.ReturnStandard)
                            Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Balances})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Character Balances data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCharJobs2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = False Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = pOwner.Account
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(pOwner, CorpRepType.Jobs) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim retries As Integer = 0
                            Do
                                retries += 1
                                apixml = apireq.GetAPIXML(APITypes.IndustryChar, pilotAccount.ToAPIAccount, pOwner.ID, APIReturnMethods.ReturnStandard)
                            Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                            ' Write the installerIDs to the database
                            If apixml IsNot Nothing Then
                                Call PrismDataFunctions.WriteInstallerIdsToDB(apixml)
                                Call PrismDataFunctions.WriteInventionResultsToDB(apixml)
                            End If

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Jobs})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Character Jobs data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCharJournal2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = False Then

                        Dim apixml As XmlDocument
                        Dim pilotAccount As EveHQAccount = pOwner.Account
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(pOwner, CorpRepType.WalletJournal) = True Then

                            ' Get the last referenceID for the wallet
                            Dim lastTrans As Long = PrismDataFunctions.GetLastWalletID(WalletTypes.Journal, CInt(pOwner.ID), 1000)

                            ' Start a loop to collect multiple APIs
                            Dim walletJournals As New SortedList(Of String, WalletJournalItem)
                            Dim lastRefID As Long = 0
                            Dim walletExhausted As Boolean
                            apireq = New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                            Do
                                ' Make a call to the EveHQ.Core.API to fetch the journal
                                Dim retries As Integer = 0
                                Do
                                    retries += 1
                                    apixml = apireq.GetAPIXML(APITypes.WalletJournalChar, pilotAccount.ToAPIAccount, pOwner.ID, 1000, lastRefID, MaxAPIJournals, APIReturnMethods.ReturnStandard)
                                Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                                ' Parse the Journal XML to get the data
                                If apixml IsNot Nothing Then
                                    walletExhausted = PrismDataFunctions.ParseWalletJournalXML(apixml, walletJournals, pOwner.ID)
                                Else
                                    walletExhausted = True
                                End If

                                If walletJournals.Count <> 0 Then
                                    lastRefID = walletJournals(walletJournals.Keys(0)).RefID
                                Else
                                    walletExhausted = True
                                End If

                            Loop Until walletExhausted = True Or lastTrans > lastRefID

                            ' Write the journal to the database!
                            If walletJournals.Count > 0 Then
                                Call PrismDataFunctions.WriteWalletJournalsToDB(walletJournals, CInt(pOwner.ID), pOwner.Name, 1000, lastTrans)
                            End If

                        End If

                        ' Update the display
                        Dim oldXML As XmlDocument = apireq.GetAPIXML(APITypes.WalletJournalChar, pilotAccount.ToAPIAccount, pOwner.ID, 1000, 0, MaxAPIJournals, APIReturnMethods.ReturnCacheOnly)
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {oldXML, pOwner, CorpRepType.WalletJournal})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Character Journal data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCharOrders2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = False Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = pOwner.Account
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(pOwner, CorpRepType.Orders) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the relevant API
                            Dim retries As Integer = 0
                            Do
                                retries += 1
                                apixml = apireq.GetAPIXML(APITypes.OrdersChar, pilotAccount.ToAPIAccount, pOwner.ID, APIReturnMethods.ReturnStandard)
                            Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Orders})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Character Orders data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCharTransactions2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = False Then

                        ' Setup the array of transactions
                        Const TransID As String = ""

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = pOwner.Account
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(pOwner, CorpRepType.WalletTransactions) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the transactions
                            Dim retries As Integer = 0
                            Do
                                retries += 1
                                apixml = apireq.GetAPIXML(APITypes.WalletTransChar, pilotAccount.ToAPIAccount, pOwner.ID, 1000, TransID, APIReturnMethods.ReturnStandard)
                            Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                            ' Write the journal to the database!
                            Call PrismDataFunctions.WriteWalletTransactionsToDB(apixml, False, CInt(pOwner.ID), pOwner.Name, 1000)

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.WalletTransactions})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Character Transactions data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCharContracts2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = False Then

                        Dim apixml As XmlDocument
                        Dim pilotAccount As EveHQAccount = pOwner.Account
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                        ' Check for valid API Usage
                        If CanUseAPI(pOwner, CorpRepType.Contracts) = True Then

                            ' Make a call to the EveHQ.Core.API to fetch the contracts
                            Dim retries As Integer = 0
                            Do
                                retries += 1
                                apixml = apireq.GetAPIXML(APITypes.ContractsChar, pilotAccount.ToAPIAccount, pOwner.ID, APIReturnMethods.ReturnStandard)
                            Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                            ' Write the contractIDs to the database
                            If apixml IsNot Nothing Then
                                Call PrismDataFunctions.WriteContractIdsToDB(apixml)
                            End If

                            If apixml IsNot Nothing Then
                                ' Get the Node List
                                Dim contracts As XmlNodeList = apixml.SelectNodes("/eveapi/result/rowset/row")
                                ' Parse the Node List
                                For Each contractItem As XmlNode In contracts
                                    Dim contractID As Long = CLng(contractItem.Attributes.GetNamedItem("contractID").Value)
                                    retries = 0
                                    Do
                                        retries += 1
                                        apireq.GetAPIXML(APITypes.ContractItemsChar, pilotAccount.ToAPIAccount, pOwner.ID, contractID, APIReturnMethods.ReturnStandard)
                                    Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0
                                Next
                            End If

                        End If

                        ' Update the display
                        apixml = apireq.GetAPIXML(APITypes.ContractsChar, pilotAccount.ToAPIAccount, pOwner.ID, APIReturnMethods.ReturnStandard)
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Contracts})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Character Contracts data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub UpdateNullCorpSheet2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = False Then

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {Nothing, pOwner, CorpRepType.CorpSheet})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Null Corp Sheet data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If
        End Sub

        Private Sub GetCorpAssets2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = True Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.Assets)
                        Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Assets)
                        If pilotAccount IsNot Nothing And ownerID <> "" Then
                            Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                            ' Check for valid API Usage
                            If CanUseAPI(pOwner, CorpRepType.Assets) = True Then

                                ' Make a call to the EveHQ.Core.API to fetch the relevant API
                                Dim retries As Integer = 0
                                Do
                                    retries += 1
                                    apixml = apireq.GetAPIXML(APITypes.AssetsCorp, pilotAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnStandard)
                                Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                            End If
                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Assets})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Corporate Assets data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCorpBalances2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = True Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.Balances)
                        Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Balances)
                        If pilotAccount IsNot Nothing And ownerID <> "" Then
                            Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                            ' Check for valid API Usage
                            If CanUseAPI(pOwner, CorpRepType.Balances) = True Then

                                ' Make a call to the EveHQ.Core.API to fetch the relevant API
                                Dim retries As Integer = 0
                                Do
                                    retries += 1
                                    apixml = apireq.GetAPIXML(APITypes.AccountBalancesCorp, pilotAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnStandard)
                                Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                            End If
                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Balances})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Corporate Balances data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCorpJobs2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = True Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.Jobs)
                        Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Jobs)
                        If pilotAccount IsNot Nothing And ownerID <> "" Then
                            Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                            ' Check for valid API Usage
                            If CanUseAPI(pOwner, CorpRepType.Jobs) = True Then

                                ' Make a call to the EveHQ.Core.API to fetch the relevant API
                                Dim retries As Integer = 0
                                Do
                                    retries += 1
                                    apixml = apireq.GetAPIXML(APITypes.IndustryCorp, pilotAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnStandard)
                                Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                                ' Write the installerIDs to the database
                                If apixml IsNot Nothing Then
                                    Call PrismDataFunctions.WriteInstallerIdsToDB(apixml)
                                    Call PrismDataFunctions.WriteInventionResultsToDB(apixml)
                                End If

                            End If

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Jobs})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Corporate Jobs data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCorpJournal2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = True Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.WalletJournal)
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
                        Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.WalletJournal)
                        If pilotAccount IsNot Nothing And ownerID <> "" Then

                            ' Check for valid API Usage
                            If CanUseAPI(pOwner, CorpRepType.WalletJournal) = True Then

                                For divID As Integer = 1006 To 1000 Step -1

                                    ' Get the last referenceID for the wallet
                                    Dim lastTrans As Long = PrismDataFunctions.GetLastWalletID(WalletTypes.Journal, CInt(pOwner.ID), divID)

                                    ' Start a loop to collect multiple APIs
                                    Dim walletJournals As New SortedList(Of String, WalletJournalItem)
                                    Dim lastRefID As Long = 0
                                    Dim walletExhausted As Boolean
                                    apireq = New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                                    Do
                                        ' Make a call to the EveHQ.Core.API to fetch the journal
                                        Dim retries As Integer = 0
                                        Do
                                            retries += 1
                                            apixml = apireq.GetAPIXML(APITypes.WalletJournalCorp, pilotAccount.ToAPIAccount, ownerID, divID, lastRefID, MaxAPIJournals, APIReturnMethods.ReturnStandard)
                                        Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                                        ' Parse the Journal XML to get the data
                                        If apixml IsNot Nothing Then
                                            walletExhausted = PrismDataFunctions.ParseWalletJournalXML(apixml, walletJournals, pOwner.ID)
                                        Else
                                            walletExhausted = True
                                        End If

                                        If walletJournals.Count <> 0 Then
                                            lastRefID = walletJournals(walletJournals.Keys(0)).RefID
                                        Else
                                            walletExhausted = True
                                        End If

                                    Loop Until walletExhausted = True Or lastTrans > lastRefID

                                    ' Write the journal to the database!
                                    If walletJournals.Count > 0 Then
                                        Call PrismDataFunctions.WriteWalletJournalsToDB(walletJournals, CInt(pOwner.ID), pOwner.Name, divID, lastTrans)
                                    End If

                                Next

                                apixml = apireq.GetAPIXML(APITypes.WalletJournalCorp, pilotAccount.ToAPIAccount, ownerID, 1000, 0, MaxAPIJournals, APIReturnMethods.ReturnCacheOnly)

                            End If

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.WalletJournal})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Corporate Journal data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCorpOrders2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = True Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.Orders)
                        Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Orders)
                        If pilotAccount IsNot Nothing And ownerID <> "" Then
                            Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                            ' Check for valid API Usage
                            If CanUseAPI(pOwner, CorpRepType.Orders) = True Then

                                ' Make a call to the EveHQ.Core.API to fetch the relevant API
                                Dim retries As Integer = 0
                                Do
                                    retries += 1
                                    apixml = apireq.GetAPIXML(APITypes.OrdersCorp, pilotAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnStandard)
                                Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                            End If

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Orders})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Corporate Orders data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCorpTransactions2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = True Then

                        ' Setup the array of transactions
                        Const TransID As String = ""

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.WalletTransactions)
                        Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.WalletTransactions)
                        If pilotAccount IsNot Nothing And ownerID <> "" Then
                            Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                            ' Check for valid API Usage
                            If CanUseAPI(pOwner, CorpRepType.WalletTransactions) = True Then

                                For divID As Integer = 1006 To 1000 Step -1

                                    ' Make a call to the EveHQ.Core.API to fetch the transactions
                                    Dim retries As Integer = 0
                                    Do
                                        retries += 1
                                        apixml = apireq.GetAPIXML(APITypes.WalletTransCorp, pilotAccount.ToAPIAccount, ownerID, divID, TransID, APIReturnMethods.ReturnStandard)
                                    Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                                    ' Write the journal to the database!
                                    Call PrismDataFunctions.WriteWalletTransactionsToDB(apixml, False, CInt(pOwner.ID), pOwner.Name, divID)

                                Next
                            End If

                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.WalletTransactions})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Corporate Transactions data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCorpContracts2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = True Then

                        Dim apixml As New XmlDocument
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
                        Dim pilotAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.Contracts)
                        Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Contracts)
                        If pilotAccount IsNot Nothing And ownerID <> "" Then

                            ' Check for valid API Usage
                            If CanUseAPI(pOwner, CorpRepType.Contracts) = True Then

                                ' Make a call to the EveHQ.Core.API to fetch the contracts
                                Dim retries As Integer = 0
                                Do
                                    retries += 1
                                    apixml = apireq.GetAPIXML(APITypes.ContractsCorp, pilotAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnStandard)
                                Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                                ' Write the contractIDs to the database
                                If apixml IsNot Nothing Then
                                    Call PrismDataFunctions.WriteContractIdsToDB(apixml)
                                End If

                                If apixml IsNot Nothing Then
                                    ' Get the Node List
                                    Dim contracts As XmlNodeList = apixml.SelectNodes("/eveapi/result/rowset/row")
                                    ' Parse the Node List
                                    For Each contractItem As XmlNode In contracts
                                        Dim contractID As Long = CLng(contractItem.Attributes.GetNamedItem("contractID").Value)
                                        retries = 0
                                        Do
                                            retries += 1
                                            apireq.GetAPIXML(APITypes.ContractItemsCorp, pilotAccount.ToAPIAccount, ownerID, contractID, APIReturnMethods.ReturnStandard)
                                        Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0
                                    Next
                                End If

                                apixml = apireq.GetAPIXML(APITypes.ContractsCorp, pilotAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnStandard)

                            End If
                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.Contracts})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Corporate Contracts data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub
        Private Sub GetCorpSheet2(ByVal state As Object)

            For Each pOwner As PrismOwner In PlugInData.PrismOwners.Values
                Try
                    If pOwner.IsCorp = True Then

                        Dim apixml As New XmlDocument
                        Dim pilotAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.CorpSheet)

                        Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.CorpSheet)
                        If pilotAccount IsNot Nothing And ownerID <> "" Then

                            Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                            ' Check for valid API Usage
                            If CanUseAPI(pOwner, CorpRepType.CorpSheet) = True Then

                                ' Make a call to the EveHQ.Core.API to fetch the relevant API
                                Dim retries As Integer = 0
                                Do
                                    retries += 1
                                    apixml = apireq.GetAPIXML(APITypes.CorpSheet, pilotAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnStandard)
                                Loop Until retries >= MaxAPIRetries Or apireq.LastAPIError <> 0

                            End If
                        End If

                        ' Update the display
                        If IsHandleCreated = True Then
                            Invoke(_xmlDelegate, New Object() {apixml, pOwner, CorpRepType.CorpSheet})
                        End If

                    End If
                Catch e As Exception
                    Dim msg As String = "An error occured while processing the Corporation Sheet data for " & pOwner.Name & ControlChars.CrLf
                    msg &= "The specific error was: " & e.Message & ControlChars.CrLf
                    msg &= "The stacktrace was: " & e.StackTrace & ControlChars.CrLf
                    MessageBox.Show(msg, "GetCharAssets Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            _prismThreadCurrent += 1
            If _prismThreadCurrent = _prismThreadMax Then
                Call CompleteAPIUpdate()
            End If

        End Sub

#End Region

        Private Function CacheDate(ByVal apixml As XmlDocument) As DateTime
            ' Get Cache time details
            Dim cacheDetails As XmlNodeList = apixml.SelectNodes("/eveapi")
            Dim cacheTime As DateTime = CDate(cacheDetails(0).ChildNodes(2).InnerText)
            Dim localCacheTime As Date = SkillFunctions.ConvertEveTimeToLocal(cacheTime)
            Return localCacheTime
        End Function

#End Region

#Region "Market Orders Routines"

        Private Sub cboOrdersOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboOrdersOwner.SelectedIndexChanged
            Call ParseOrders()
        End Sub

        Private Sub ParseOrders()
            ' Get the owner we will use
            Dim pOwner As PrismOwner
            If cboOrdersOwner.SelectedItem IsNot Nothing Then
                If PlugInData.PrismOwners.ContainsKey(cboOrdersOwner.SelectedItem.ToString) Then
                    pOwner = PlugInData.PrismOwners(cboOrdersOwner.SelectedItem.ToString)
                    Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.Orders)
                    Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Orders)
                    Dim orderXML As XmlDocument
                    Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                    If ownerAccount IsNot Nothing Then

                        If pOwner.IsCorp = True Then
                            orderXML = apireq.GetAPIXML(APITypes.OrdersCorp, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                        Else
                            orderXML = apireq.GetAPIXML(APITypes.OrdersChar, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                        End If
                        Dim sellTotal, buyTotal, totalEscrow As Double
                        Dim totalOrders As Integer = 0
                        If orderXML IsNot Nothing Then

                            Dim orders As XmlNodeList = orderXML.SelectNodes("/eveapi/result/rowset/row")
                            adtBuyOrders.BeginUpdate()
                            adtSellOrders.BeginUpdate()
                            adtBuyOrders.Nodes.Clear()
                            adtSellOrders.Nodes.Clear()
                            For Each order As XmlNode In orders
                                If order.Attributes.GetNamedItem("bid").Value = "0" Then
                                    If order.Attributes.GetNamedItem("orderState").Value = "0" Then
                                        Dim sOrder As New Node
                                        adtSellOrders.Nodes.Add(sOrder)
                                        sOrder.CreateCells()
                                        Dim itemID As Integer = CInt(order.Attributes.GetNamedItem("typeID").Value)
                                        Dim itemName As String
                                        If StaticData.Types.ContainsKey(itemID) = True Then
                                            itemName = StaticData.Types(itemID).Name
                                        Else
                                            itemName = "Unknown Item ID:" & itemID
                                        End If
                                        sOrder.Text = itemName
                                        Dim quantity As Double = Double.Parse(order.Attributes.GetNamedItem("volRemaining").Value, _culture)
                                        sOrder.Cells(1).Text = quantity.ToString("N0") & " / " & CDbl(order.Attributes.GetNamedItem("volEntered").Value).ToString("N0")
                                        Dim price As Double = Double.Parse(order.Attributes.GetNamedItem("price").Value, NumberStyles.Any, _culture)
                                        sOrder.Cells(2).Text = price.ToString("N2")
                                        Dim loc As String
                                        If StaticData.Stations.ContainsKey(CInt(order.Attributes.GetNamedItem("stationID").Value)) = True Then
                                            loc = StaticData.Stations(CInt(order.Attributes.GetNamedItem("stationID").Value)).StationName
                                        Else
                                            loc = "StationID: " & order.Attributes.GetNamedItem("stationID").Value
                                        End If
                                        sOrder.Cells(3).Text = loc
                                        Dim issueDate As Date = DateTime.ParseExact(order.Attributes.GetNamedItem("issued").Value, PrismTimeFormat, _culture, DateTimeStyles.None)
                                        Dim orderExpires As TimeSpan = issueDate - Now
                                        orderExpires = orderExpires.Add(New TimeSpan(CInt(order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                                        sOrder.Cells(4).Tag = orderExpires
                                        If orderExpires.TotalSeconds <= 0 Then
                                            sOrder.Cells(4).Text = "Expired!"
                                        Else
                                            sOrder.Cells(4).Text = SkillFunctions.TimeToString(orderExpires.TotalSeconds, False)
                                        End If
                                        sellTotal = sellTotal + quantity * price
                                        totalOrders = totalOrders + 1
                                    End If
                                Else
                                    If order.Attributes.GetNamedItem("orderState").Value = "0" Then
                                        Dim bOrder As New Node
                                        adtBuyOrders.Nodes.Add(bOrder)
                                        bOrder.CreateCells()
                                        Dim itemID As Integer = CInt(order.Attributes.GetNamedItem("typeID").Value)
                                        Dim itemName As String
                                        If StaticData.Types.ContainsKey(itemID) = True Then
                                            itemName = StaticData.Types(itemID).Name
                                        Else
                                            itemName = "Unknown Item ID:" & itemID
                                        End If
                                        bOrder.Text = itemName
                                        Dim quantity As Double = Double.Parse(order.Attributes.GetNamedItem("volRemaining").Value, _culture)
                                        bOrder.Cells(1).Text = quantity.ToString("N0") & " / " & CDbl(order.Attributes.GetNamedItem("volEntered").Value).ToString("N0")
                                        Dim price As Double = Double.Parse(order.Attributes.GetNamedItem("price").Value, NumberStyles.Any, _culture)
                                        bOrder.Cells(2).Text = price.ToString("N2")
                                        Dim loc As String
                                        If StaticData.Stations.ContainsKey(CInt(order.Attributes.GetNamedItem("stationID").Value)) = True Then
                                            loc = StaticData.Stations(CInt(order.Attributes.GetNamedItem("stationID").Value)).StationName
                                        Else
                                            loc = "StationID: " & order.Attributes.GetNamedItem("stationID").Value
                                        End If
                                        bOrder.Cells(3).Text = loc
                                        bOrder.Cells(4).Tag = CInt(order.Attributes.GetNamedItem("range").Value)
                                        Select Case CInt(order.Attributes.GetNamedItem("range").Value)
                                            Case -1
                                                bOrder.Cells(4).Text = "Station"
                                            Case 0
                                                bOrder.Cells(4).Text = "System"
                                            Case 32767
                                                bOrder.Cells(4).Text = "EveGalaticRegion"
                                            Case Is > 0, Is < 32767
                                                bOrder.Cells(4).Text = order.Attributes.GetNamedItem("range").Value & " Jumps"
                                        End Select
                                        bOrder.Cells(5).Text = Double.Parse(order.Attributes.GetNamedItem("minVolume").Value, _culture).ToString("N0")
                                        Dim issueDate As Date = DateTime.ParseExact(order.Attributes.GetNamedItem("issued").Value, PrismTimeFormat, _culture, DateTimeStyles.None)
                                        Dim orderExpires As TimeSpan = issueDate - Now
                                        orderExpires = orderExpires.Add(New TimeSpan(CInt(order.Attributes.GetNamedItem("duration").Value), 0, 0, 0))
                                        bOrder.Cells(6).Tag = orderExpires
                                        If orderExpires.TotalSeconds <= 0 Then
                                            bOrder.Cells(6).Text = "Expired!"
                                        Else
                                            bOrder.Cells(6).Text = SkillFunctions.TimeToString(orderExpires.TotalSeconds, False)
                                        End If
                                        buyTotal = buyTotal + quantity * price
                                        totalEscrow = totalEscrow + Double.Parse(order.Attributes.GetNamedItem("escrow").Value, _culture)
                                        totalOrders = totalOrders + 1
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
                            AdvTreeSorter.Sort(adtBuyOrders, 1, True, False)
                            adtBuyOrders.EndUpdate()
                            AdvTreeSorter.Sort(adtSellOrders, 1, True, False)
                            adtSellOrders.EndUpdate()
                        End If

                        If pOwner.IsCorp = False Then
                            If HQ.Settings.Pilots.ContainsKey(pOwner.Name) = True Then
                                Dim selPilot As EveHQPilot = HQ.Settings.Pilots(pOwner.Name)
                                Dim maxorders As Integer = 5 + (CInt(selPilot.KeySkills(KeySkill.Trade)) * 4) + (CInt(selPilot.KeySkills(KeySkill.Tycoon)) * 32) + (CInt(selPilot.KeySkills(KeySkill.Retail)) * 8) + (CInt(selPilot.KeySkills(KeySkill.Wholesale)) * 16)
                                Dim transTax As Double = 1 * (1.5 - 0.15 * (CInt(selPilot.KeySkills(KeySkill.Accounting))))
                                Dim brokerFee As Double = 1 * (1 - 0.05 * (CInt(selPilot.KeySkills(KeySkill.BrokerRelations))))
                                lblAskRange.Text = GetOrderRange(CInt(selPilot.KeySkills(KeySkill.Procurement)))
                                lblBidRange.Text = GetOrderRange(CInt(selPilot.KeySkills(KeySkill.Marketing)))
                                lblModRange.Text = GetOrderRange(CInt(selPilot.KeySkills(KeySkill.Daytrading)))
                                lblRemoteRange.Text = GetOrderRange(CInt(selPilot.KeySkills(KeySkill.Visibility)))
                                lblOrders.Text = (maxorders - totalOrders).ToString + " / " + maxorders.ToString
                                lblBrokerFee.Text = brokerFee.ToString("N2") & "%"
                                lblTransTax.Text = transTax.ToString("N2") & "%"
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
                        Dim cover As Double = buyTotal - totalEscrow
                        lblSellTotal.Text = sellTotal.ToString("N2") & " isk"
                        lblBuyTotal.Text = buyTotal.ToString("N2") & " isk"
                        lblEscrow.Text = totalEscrow.ToString("N2") & " isk (additional " & cover.ToString("N2") & " isk to cover)"
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
                    Return "limited to EveGalaticRegion"
            End Select
        End Function

        Private Sub adtBuyOrders_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtBuyOrders.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

        Private Sub adtSellOrders_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtSellOrders.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

#End Region

#Region "Wallet Journal Routines"

        Private Sub InitialiseJournal()
            ' Prepare info
            dtiJournalEndDate.Value = SkillFunctions.ConvertLocalTimeToEve(Now)
            dtiJournalStartDate.Value = SkillFunctions.ConvertLocalTimeToEve(Now.AddMonths(-1))
            cboJournalOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, False, cboJournalOwners)
            AddHandler CType(cboJournalOwners.DropDownControl, PrismSelectionControl).SelectionChanged, AddressOf JournalOwnersChanged
            cboJournalRefTypes.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalRefTypes, True, cboJournalRefTypes)
        End Sub

        Private Sub JournalOwnersChanged()
            ' Update the wallet based on the owner (should be single owner!)
            Call UpdateWalletJournalDivisions()
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
                    Dim itemName As String
                    If StaticData.Types.ContainsKey(CInt(argName1)) = True Then
                        itemName = StaticData.Types(CInt(argName1)).Name
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
                Dim pOwner As PrismOwner

                If PlugInData.PrismOwners.ContainsKey(cboJournalOwners.Text) = True Then

                    cboWalletJournalDivision.BeginUpdate()
                    cboWalletJournalDivision.Items.Clear()

                    pOwner = PlugInData.PrismOwners(cboJournalOwners.Text)
                    Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.CorpSheet)
                    Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.CorpSheet)

                    If ownerAccount IsNot Nothing Then

                        If pOwner.IsCorp = True Then
                            Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
                            Dim corpXML As XmlDocument = apireq.GetAPIXML(APITypes.CorpSheet, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
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
                            Else
                                For div As Integer = 1000 To 1006
                                    cboWalletJournalDivision.Items.Add(div.ToString.Trim)
                                Next
                            End If
                        Else
                            cboWalletJournalDivision.Items.Add("1000")
                        End If
                    Else
                        If pOwner.IsCorp Then
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
                    cboWalletJournalDivision.Items.Add("1000")
                    cboWalletJournalDivision.EndUpdate()
                    cboWalletJournalDivision.SelectedIndex = 0
                    cboWalletJournalDivision.Enabled = False
                End If

            End If

        End Sub

        Private Sub DisplayWalletJournalEntries()

            ' Fetch the appropriate data
            Dim walletData As DataSet = FetchWalletJournalData()

            adtJournal.BeginUpdate()
            adtJournal.Nodes.Clear()
            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then
                    Dim transItem As Node
                    Dim transDate As Date
                    Dim transA, transB As Double
                    Dim refType As String
                    Dim runningBalance As Double = 0
                    For Each je As DataRow In walletData.Tables(0).Rows
                        transItem = New Node
                        transDate = DateTime.Parse(je.Item("transDate").ToString)
                        transItem.Text = transDate.ToString
                        refType = je.Item("refTypeID").ToString
                        transItem.Cells.Add(New Cell(PlugInData.RefTypes(refType)))
                        transA = Double.Parse(je.Item("amount").ToString)
                        transB = Double.Parse(je.Item("balance").ToString)
                        runningBalance += transA

                        If transA < 0 Then
                            transItem.Cells.Add(New Cell(transA.ToString("N2"), _styleRedRight))
                        Else
                            transItem.Cells.Add(New Cell(transA.ToString("N2"), _styleGreenRight))

                        End If

                        If sbShowEveBalance.Value = True Then
                            transItem.Cells.Add(New Cell(transB.ToString("N2"), _styleRight))
                        Else
                            transItem.Cells.Add(New Cell(runningBalance.ToString("N2"), _styleRight))
                        End If

                        If je.Item("reason").ToString <> "" Then
                            transItem.Cells.Add(New Cell("[r] " & BuildJournalDescription(CInt(refType), je.Item("ownerName1").ToString, je.Item("ownerName2").ToString, je.Item("argID1").ToString, je.Item("argName1").ToString)))
                            Dim transReason As New Node
                            transReason.Cells.Add(New Cell) : transReason.Cells.Add(New Cell) : transReason.Cells.Add(New Cell)
                            transReason.Cells.Add(New Cell(je.Item("reason").ToString))
                            transItem.Nodes.Add(transReason)
                        Else
                            If IsDBNull(je.Item("typeID")) = False Then
                                ' Put some market data here
                                Dim typeId As Integer = CInt(je.Item("typeID"))
                                If (StaticData.Types.ContainsKey(typeId)) Then
                                    Dim item As EveType = StaticData.Types(typeId)
                                    transItem.Cells.Add(New Cell("[t] " & BuildJournalDescription(CInt(refType), je.Item("ownerName1").ToString, je.Item("ownerName2").ToString, je.Item("argID1").ToString, je.Item("argName1").ToString)))
                                    Dim transReason As New Node
                                    transReason.Cells.Add(New Cell) : transReason.Cells.Add(New Cell) : transReason.Cells.Add(New Cell)
                                    transReason.Cells.Add(New Cell(item.Name & " (" & CDbl(je.Item("quantity")).ToString("N0") & " @ " & CDbl(je.Item("price")).ToString("N2") & " isk p/u)"))
                                    transItem.Nodes.Add(transReason)
                                Else
                                    Trace.TraceWarning("Display Wallet Journal tried to find information on an unknown item type id: " & typeId)
                                End If

                            Else
                                transItem.Cells.Add(New Cell(BuildJournalDescription(CInt(refType), je.Item("ownerName1").ToString, je.Item("ownerName2").ToString, je.Item("argID1").ToString, je.Item("argName1").ToString)))
                            End If
                        End If

                        adtJournal.Nodes.Add(transItem)
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
            strSQL &= " LEFT JOIN walletTransactions ON walletJournal.argName1 = walletTransactions.transRef"
            strSQL &= " WHERE (walletJournal.walletID = " & walletID & ")"
            strSQL &= " AND walletJournal.transDate >= '" & dtiJournalStartDate.Value.ToString(PrismTimeFormat, _culture) & "' AND walletJournal.transDate <= '" & dtiJournalEndDate.Value.ToString(PrismTimeFormat, _culture) & "'"

            ' Build the Owners List
            If cboJournalOwners.Text <> "<All>" Then
                Dim ownerList As New StringBuilder
                For Each lvi As ListViewItem In CType(cboJournalOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                    ownerList.Append(", '" & lvi.Name.Replace("'", "''") & "'")
                Next
                If ownerList.Length > 2 Then
                    ownerList.Remove(0, 2)
                End If
                ' Default to None
                strSQL &= " AND walletJournal.charName IN (" & ownerList.ToString & ")"
            End If

            ' Build the refTypes List
            If cboJournalRefTypes.Text <> "All" Then
                ' Build a ref type list
                Dim refTypeList As New StringBuilder
                For Each lvi As ListViewItem In CType(cboJournalRefTypes.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                    refTypeList.Append(", " & lvi.Name)
                Next
                If refTypeList.Length > 2 Then
                    refTypeList.Remove(0, 2)
                    ' Default to All
                    strSQL &= " AND walletJournal.refTypeID IN (" & refTypeList.ToString & ")"
                End If
            End If

            ' Order the data
            strSQL &= " ORDER BY walletJournal.transKey DESC;"

            ' Fetch the data
            Dim walletData As DataSet = CustomDataFunctions.GetCustomData(strSQL)

            Return walletData

        End Function

        Private Function FetchWalletJournalDataForExport() As DataSet
            Dim walletID As String = (1000 + cboWalletJournalDivision.SelectedIndex).ToString
            Dim strSQL As String = "SELECT * FROM walletJournal"
            strSQL &= " WHERE (walletJournal.walletID = " & walletID & ")"
            strSQL &= " AND walletJournal.transDate >= '" & dtiJournalStartDate.Value.ToString(PrismTimeFormat, _culture) & "' AND walletJournal.transDate <= '" & dtiJournalEndDate.Value.ToString(PrismTimeFormat, _culture) & "'"

            ' Build the Owners List
            If cboJournalOwners.Text <> "<All>" Then
                Dim ownerList As New StringBuilder
                For Each lvi As ListViewItem In CType(cboJournalOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                    ownerList.Append(", '" & lvi.Name.Replace("'", "''") & "'")
                Next
                If ownerList.Length > 2 Then
                    ownerList.Remove(0, 2)
                End If
                ' Default to None
                strSQL &= " AND walletJournal.charName IN (" & ownerList.ToString & ")"
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
            Dim walletData As DataSet = CustomDataFunctions.GetCustomData(strSQL)

            Return walletData

        End Function

        Private Sub btnJournalQuery_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnJournalQuery.Click
            Call DisplayWalletJournalEntries()
        End Sub

        Private Sub sbShowEveBalance_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles sbShowEveBalance.ValueChanged
            If cboWalletJournalDivision.SelectedItem IsNot Nothing Then
                Call DisplayWalletJournalEntries()
            End If
        End Sub

        Private Sub dtiJournalStartDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As EventArgs) Handles dtiJournalStartDate.ButtonCustom2Click
            dtiJournalStartDate.Value = New Date(dtiJournalStartDate.Value.Year, dtiJournalStartDate.Value.Month, dtiJournalStartDate.Value.Day)
        End Sub

        Private Sub dtiJournalStartDate_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles dtiJournalStartDate.ButtonCustomClick
            dtiJournalStartDate.Value = SkillFunctions.ConvertLocalTimeToEve(Now)
        End Sub

        Private Sub dtiJournalEndDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As EventArgs) Handles dtiJournalEndDate.ButtonCustom2Click
            dtiJournalEndDate.Value = New Date(dtiJournalEndDate.Value.Year, dtiJournalEndDate.Value.Month, dtiJournalEndDate.Value.Day)
        End Sub

        Private Sub dtiJournalEndDate_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles dtiJournalEndDate.ButtonCustomClick
            dtiJournalEndDate.Value = SkillFunctions.ConvertLocalTimeToEve(Now)
        End Sub

        Private Sub btnResetJournal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetJournal.Click
            Dim reply As DialogResult = MessageBox.Show("Are you really sure you want to delete all the journal entries from the database?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.Yes Then
                Dim strSQL As String = "DELETE * FROM walletJournal;"
                If CustomDataFunctions.SetCustomData(strSQL) <> -2 Then
                    MessageBox.Show("Reset Complete")
                End If
                strSQL = "DROP TABLE walletJournal;"
                If CustomDataFunctions.SetCustomData(strSQL) <> -2 Then
                    MessageBox.Show("Table Deletion Complete")
                End If
                Call PrismDataFunctions.CheckDatabaseTables()
            End If
        End Sub

        Private Sub btnCheckJournalOmissions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCheckJournalOmissions.Click
            Using checkJournals As New FrmJournalCheck
                checkJournals.ShowDialog()
            End Using
        End Sub

        Private Sub btnExportEntries_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportEntries.Click

            ' Check for multiple owners - can't do this!

            If CType(cboJournalOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems.Count <> 1 Then
                MessageBox.Show("You can only export data for a single owner at a time. Please adjust the Journal Owners accordingly.", "Wallet Journal Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else

                Dim sfd As New SaveFileDialog
                sfd.Title = "Export Wallet Journal Entries"
                sfd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                Const FilterText As String = "XML files (*.xml)|*.xml"
                sfd.Filter = FilterText
                sfd.FilterIndex = 0
                sfd.AddExtension = True
                sfd.ShowDialog()
                sfd.CheckPathExists = True
                If sfd.FileName <> "" Then

                    ' Fetch the appropriate data
                    Dim walletData As DataSet = FetchWalletJournalDataForExport()

                    Dim xmlDoc As New XmlDocument
                    Dim dec As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", Nothing, Nothing)
                    xmlDoc.AppendChild(dec)

                    ' Create XML root
                    Dim xmlRoot As XmlElement = xmlDoc.CreateElement("EveHQWalletJournalExport")
                    xmlDoc.AppendChild(xmlRoot)

                    ' Create heading for future reference & import
                    Dim headerRow As XmlElement = xmlDoc.CreateElement("exportedData")
                    Dim headerAtt As XmlAttribute = xmlDoc.CreateAttribute("ownerName")
                    headerAtt.Value = HttpUtility.HtmlEncode(cboJournalOwners.Text)
                    headerRow.Attributes.Append(headerAtt)
                    headerAtt = xmlDoc.CreateAttribute("walletID")
                    headerAtt.Value = HttpUtility.HtmlEncode((1000 + cboWalletJournalDivision.SelectedIndex).ToString)
                    headerRow.Attributes.Append(headerAtt)
                    headerAtt = xmlDoc.CreateAttribute("startDate")
                    headerAtt.Value = HttpUtility.HtmlEncode(dtiJournalStartDate.Value.ToString(PrismTimeFormat, _culture))
                    headerRow.Attributes.Append(headerAtt)
                    headerAtt = xmlDoc.CreateAttribute("endDate")
                    headerAtt.Value = HttpUtility.HtmlEncode(dtiJournalEndDate.Value.ToString(PrismTimeFormat, _culture))
                    headerRow.Attributes.Append(headerAtt)
                    ' Add the header row
                    xmlRoot.AppendChild(headerRow)

                    ' Create main XML data
                    For Each row As DataRow In walletData.Tables(0).Rows
                        Dim xmlRow As XmlElement = xmlDoc.CreateElement("row")
                        For Each col As DataColumn In walletData.Tables(0).Columns
                            Dim xmlAtt As XmlAttribute = xmlDoc.CreateAttribute(col.ColumnName)
                            If col.DataType() Is GetType(Double) Then
                                xmlAtt.Value = HttpUtility.HtmlEncode(CDbl(row.Item(col)).ToString(_culture))
                            Else
                                If col.DataType() Is GetType(DateTime) Then
                                    xmlAtt.Value = HttpUtility.HtmlEncode(CDate(row.Item(col)).ToString(PrismTimeFormat, _culture))
                                Else
                                    xmlAtt.Value = HttpUtility.HtmlEncode(row.Item(col).ToString)
                                End If
                            End If
                            xmlRow.Attributes.Append(xmlAtt)
                        Next
                        xmlRoot.AppendChild(xmlRow)
                    Next

                    ' Save the XML file
                    xmlDoc.Save(sfd.FileName)

                End If

                sfd.Dispose()

                MessageBox.Show("Export of Wallet Journal Data completed!", "Wallet Journal Export", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        End Sub

        Private Sub btnImportEntries_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImportEntries.Click

            ' Step 1: Ask for a filename
            Dim ofd As New OpenFileDialog
            ofd.Title = "Import Wallet Journal Entries"
            ofd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Const FilterText As String = "XML files (*.xml)|*.xml"
            ofd.Filter = FilterText
            ofd.FilterIndex = 0
            ofd.AddExtension = True
            ofd.CheckPathExists = True
            ofd.ShowDialog()

            ' Step 2: Check the file exists and is of the right format
            Dim xmlDoc As New XmlDocument
            Dim ownerID As String
            Dim ownerName As String
            Dim walletID As Integer
            Dim startDate As Date
            Dim endDate As Date

            If ofd.FileName <> "" Then
                ' Check existence
                If My.Computer.FileSystem.FileExists(ofd.FileName) = True Then
                    ' Load the file into an XML document
                    Try
                        xmlDoc.Load(ofd.FileName)
                        ' Assume in XML format so check for some key information, starting with the root node
                        Dim rootNodes As XmlNodeList = xmlDoc.GetElementsByTagName("EveHQWalletJournalExport")
                        If rootNodes.Count = 1 Then
                            ' Check for the exportedData node
                            Dim configNodes As XmlNodeList = xmlDoc.GetElementsByTagName("exportedData")
                            If configNodes.Count = 1 Then
                                ' Check the node attributes
                                Dim configNode As XmlNode = configNodes(0)
                                If configNode.Attributes.Count = 5 Then
                                    Try
                                        ownerID = configNode.Attributes.GetNamedItem("ownerID").Value
                                        ownerName = configNode.Attributes.GetNamedItem("ownerName").Value
                                        walletID = CInt(configNode.Attributes.GetNamedItem("walletID").Value)
                                        startDate = DateTime.ParseExact(configNode.Attributes.GetNamedItem("startDate").Value, PrismTimeFormat, _culture)
                                        endDate = DateTime.ParseExact(configNode.Attributes.GetNamedItem("endDate").Value, PrismTimeFormat, _culture)
                                        If ownerID = "" Then
                                            MessageBox.Show("The import configuration data for OwnerID cannot be blank. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            Exit Sub
                                        End If
                                        If ownerName = "" Then
                                            MessageBox.Show("The import configuration data for OwnerName cannot be blank. Please check the file is in the correct XML format.", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            Exit Sub
                                        End If
                                        If walletID < 1000 Or walletID > 1006 Then
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
            msg.AppendLine("This procedure will first delete all wallet transactions in WalletID " & walletID.ToString & " for " & ownerName & " between the dates of " & startDate.ToString(PrismTimeFormat) & " and " & endDate.ToString(PrismTimeFormat) & ".")
            msg.AppendLine("")
            msg.AppendLine("Are you sure you wish to proceed?")
            Dim reply As DialogResult = MessageBox.Show(msg.ToString, "Confirm Wallet Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            End If

            ' Step 4: Delete existing transactions
            Dim strSQL As String = "DELETE FROM walletJournal"
            strSQL &= " WHERE (walletJournal.walletID = " & walletID & ")"
            strSQL &= " AND walletJournal.transDate >= '" & startDate.ToString(PrismTimeFormat, _culture) & "' AND walletJournal.transDate < '" & endDate.ToString(PrismTimeFormat, _culture) & "'"
            strSQL &= " AND walletJournal.charName IN ('" & ownerName.Replace("'", "''") & "')"
            Try
                CustomDataFunctions.SetCustomData(strSQL)
            Catch ex As Exception
                MessageBox.Show("There was an error removing existing transactions. The error was: " & ex.Message, "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End Try

            ' Step 5: Import new transactions
            Dim walletJournals As New SortedList(Of String, WalletJournalItem)
            PrismDataFunctions.ParseWalletJournalExportXML(xmlDoc, walletJournals, ownerID)
            PrismDataFunctions.WriteWalletJournalsToDB(walletJournals, CInt(ownerID), ownerName, walletID, 0)

            ' Step 6: Tidy up
            ofd.Dispose()

            MessageBox.Show("Import of Wallet Journal Data completed!", "Import Wallet Journal", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Sub

        Private Sub adtJournal_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtJournal.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

#End Region

#Region "Wallet Transaction Routines"

        Private Sub InitialiseTransactions()
            ' Prepare info
            dtiTransEndDate.Value = SkillFunctions.ConvertLocalTimeToEve(Now)
            dtiTransStartDate.Value = SkillFunctions.ConvertLocalTimeToEve(Now.AddMonths(-1))
            cboTransactionOwner.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionOwnersAll, False, cboTransactionOwner)
            AddHandler CType(cboTransactionOwner.DropDownControl, PrismSelectionControl).SelectionChanged, AddressOf TransactionOwnersChanged
            cboWalletTransItem.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionItems, True, cboWalletTransItem)
        End Sub

        Private Sub TransactionOwnersChanged()
            ' Update the wallet based on the owner (should be single owner!)
            Call UpdateWalletTransactionDivisions()
            cboWalletTransItem.DropDownControl = New PrismSelectionControl(PrismSelectionType.TransactionItems, True, cboWalletTransItem, cboTransactionOwner.Text)
        End Sub

        Private Sub DisplayWalletTransactions()

            If cboWalletTransType.SelectedIndex = -1 Then cboWalletTransType.SelectedIndex = 0

            Dim walletID As String = (1000 + cboWalletTransDivision.SelectedIndex).ToString
            Dim strSQL As String = "SELECT * FROM walletTransactions"
            strSQL &= " WHERE (walletTransactions.walletID = " & walletID & ")"
            strSQL &= " AND walletTransactions.transDate >= '" & dtiTransStartDate.Value.ToString(PrismTimeFormat, _culture) & "' AND walletTransactions.transDate <= '" & dtiTransEndDate.Value.ToString(PrismTimeFormat, _culture) & "'"

            ' Build the Owners List
            If cboJournalOwners.Text <> "<All>" Then
                Dim ownerList As New StringBuilder
                For Each lvi As ListViewItem In CType(cboTransactionOwner.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                    ownerList.Append(", '" & lvi.Name.Replace("'", "''") & "'")
                Next
                If ownerList.Length > 2 Then
                    ownerList.Remove(0, 2)
                End If
                ' Default to None
                strSQL &= " AND walletTransactions.charName IN (" & ownerList.ToString & ")"
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
                Dim itemTypeList As New StringBuilder
                For Each lvi As ListViewItem In CType(cboWalletTransItem.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                    itemTypeList.Append(", '" & lvi.Name.Replace("'", "''") & "'")
                Next
                If itemTypeList.Length > 2 Then
                    itemTypeList.Remove(0, 2)
                    strSQL &= " AND walletTransactions.typeName IN (" & itemTypeList.ToString & ")"
                End If
            End If

            ' Order the data
            strSQL &= " ORDER BY walletTransactions.transKey DESC;"

            ' Fetch the data
            Dim walletData As DataSet = CustomDataFunctions.GetCustomData(strSQL)

            ' Determine if this is personal, or corp, or unknown if an old owner
            Dim isPersonal As Boolean = False
            Dim isCorp As Boolean = False
            ' Check for personal
            If HQ.Settings.Pilots.ContainsKey(cboTransactionOwner.Text) = True Then
                isPersonal = True
            Else
                ' Check for corp
                If PlugInData.CorpList.ContainsKey(cboTransactionOwner.Text) = True Then
                    isCorp = True
                End If
            End If

            Dim buyValue As Double = 0
            Dim sellValue As Double = 0

            adtTransactions.BeginUpdate()
            adtTransactions.Nodes.Clear()
            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    Dim transItem As Node
                    Dim transDate As Date
                    Dim price, qty, value As Double
                    For Each je As DataRow In walletData.Tables(0).Rows
                        If (je.Item("transFor").ToString = "personal" And isPersonal = True) Or isCorp = True Or (isPersonal = False And isCorp = False) Then
                            transItem = New Node
                            transDate = DateTime.Parse(je.Item("transDate").ToString)
                            transItem.Text = transDate.ToString
                            transItem.Cells.Add(New Cell(je.Item("typeName").ToString))

                            price = Double.Parse(je.Item("price").ToString)
                            qty = Double.Parse(je.Item("quantity").ToString)

                            transItem.Cells.Add(New Cell(qty.ToString("N0"), _styleRight))
                            transItem.Cells.Add(New Cell(price.ToString("N2"), _styleRight))
                            If je.Item("transType").ToString = "sell" Then
                                value = price * qty
                                transItem.Cells.Add(New Cell(value.ToString("N2"), _styleGreenRight))
                                sellValue += value
                            Else
                                value = -price * qty
                                transItem.Cells.Add(New Cell(value.ToString("N2"), _styleRedRight))
                                buyValue += -value
                            End If

                            transItem.Cells.Add(New Cell(je.Item("stationName").ToString))
                            transItem.Cells.Add(New Cell(je.Item("clientName").ToString))

                            adtTransactions.Nodes.Add(transItem)
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
            lblTransBuyValue.Text = "Buy Value: " & buyValue.ToString("N2")
            lblTransSellValue.Text = "Sell Value: " & sellValue.ToString("N2")
            lblTransProfitValue.Text = "Profit Value: " & (sellValue - buyValue).ToString("N2")
            Dim gp As Double = 0
            Dim mu As Double = 0
            If sellValue <> 0 Then
                gp = (sellValue - buyValue) / sellValue * 100
            End If
            If buyValue <> 0 Then
                mu = (sellValue - buyValue) / buyValue * 100
            End If
            lblTransProfitRatio.Text = "Profit Ratios: GP%: " & gp.ToString("N2") & "%, MU: " & mu.ToString("N2") & "%"

        End Sub

        Private Sub UpdateWalletTransactionDivisions()

            If cboTransactionOwner.Text <> "" Then
                Dim pOwner As PrismOwner

                If PlugInData.PrismOwners.ContainsKey(cboTransactionOwner.Text) = True Then

                    cboWalletTransDivision.BeginUpdate()
                    cboWalletTransDivision.Items.Clear()

                    pOwner = PlugInData.PrismOwners(cboTransactionOwner.Text)
                    Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.CorpSheet)
                    Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.CorpSheet)

                    If ownerAccount IsNot Nothing Then

                        If pOwner.IsCorp = True Then
                            Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
                            Dim corpXML As XmlDocument = apireq.GetAPIXML(APITypes.CorpSheet, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
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
                            For div As Integer = 1000 To 1006
                                cboWalletTransDivision.Items.Add(div.ToString.Trim)
                            Next
                        End If
                    Else
                        If pOwner.IsCorp Then
                            For div As Integer = 1000 To 1006
                                cboWalletTransDivision.Items.Add(div.ToString.Trim)
                            Next
                        Else
                            cboWalletTransDivision.Items.Add("1000")
                        End If
                    End If
                    cboWalletTransDivision.Enabled = True
                    cboWalletTransDivision.EndUpdate()
                    If cboWalletTransDivision.Items.Count > 0 Then
                        cboWalletTransDivision.SelectedIndex = 0
                    End If
                Else
                    cboWalletTransDivision.Enabled = False
                End If

            End If
        End Sub

        Private Sub btnGetTransactions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetTransactions.Click
            Call DisplayWalletTransactions()
        End Sub

        Private Sub dtiTransStartDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As EventArgs) Handles dtiTransStartDate.ButtonCustom2Click
            dtiTransStartDate.Value = New Date(dtiTransStartDate.Value.Year, dtiTransStartDate.Value.Month, dtiTransStartDate.Value.Day)
        End Sub

        Private Sub dtiTransStartDate_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles dtiTransStartDate.ButtonCustomClick
            dtiTransStartDate.Value = SkillFunctions.ConvertLocalTimeToEve(Now)
        End Sub

        Private Sub dtiTransEndDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As EventArgs) Handles dtiTransEndDate.ButtonCustom2Click
            dtiTransEndDate.Value = New Date(dtiTransEndDate.Value.Year, dtiTransEndDate.Value.Month, dtiTransEndDate.Value.Day)
        End Sub

        Private Sub dtiTransEndDate_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles dtiTransEndDate.ButtonCustomClick
            dtiTransEndDate.Value = SkillFunctions.ConvertLocalTimeToEve(Now)
        End Sub

        Private Sub adtTransactions_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtTransactions.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

#End Region

#Region "Industry Jobs Routines"

        Private Sub cboJobOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboJobOwner.SelectedIndexChanged
            Call DisplayIndustryJobs()
        End Sub

        Private Sub DisplayIndustryJobs()

            ' Get the owner we will use
            If cboJobOwner.SelectedItem IsNot Nothing Then
                Dim pOwner As String = cboJobOwner.SelectedItem.ToString

                adtJobs.BeginUpdate()
                adtJobs.Nodes.Clear()

                Dim jobList As List(Of IndustryJob) = IndustryJob.ParseIndustryJobs(pOwner)

                If jobList IsNot Nothing Then

                    ' Get InstallerIDs from the database and return list
                    Dim installerList As SortedList(Of Long, String) = IndustryJob.GetInstallerList(jobList)

                    ' Initialise the installer filter
                    cboInstallerFilter.Tag = True
                    Dim oldFilter As String = ""
                    If cboInstallerFilter.SelectedItem IsNot Nothing Then
                        oldFilter = cboInstallerFilter.SelectedItem.ToString
                    End If
                    cboInstallerFilter.Items.Clear()
                    cboInstallerFilter.BeginUpdate()
                    cboInstallerFilter.Items.Add("<All Installers>")
                    For Each installerName As String In installerList.Values
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
                    Dim transItem As Node
                    Dim transTypeID As Integer
                    Dim locationID As Integer
                    Dim displayJob As Boolean

                    For Each job As IndustryJob In jobList

                        ' Check filters to see if the job is allowed
                        displayJob = False
                        ' Check installer filter
                        If cboInstallerFilter.SelectedIndex = 0 Or (cboInstallerFilter.SelectedIndex > 0 And installerList(job.InstallerID) = cboInstallerFilter.SelectedItem.ToString) Then
                            ' Check activity filter
                            If cboActivityFilter.SelectedIndex = 0 Or job.ActivityID.ToString = cboActivityFilter.SelectedItem.ToString Then
                                ' Check status filter
                                If cboStatusFilter.SelectedIndex = 0 Then
                                    displayJob = True
                                Else
                                    Select Case job.Completed
                                        Case 0
                                            If job.EndProductionTime < DateTime.Now.ToUniversalTime Then
                                                ' Job finished but not delivered
                                                If cboStatusFilter.SelectedItem.ToString = PlugInData.Statuses("B") Then
                                                    displayJob = True
                                                End If
                                            Else
                                                ' Job in progress
                                                If cboStatusFilter.SelectedItem.ToString = PlugInData.Statuses("A") Then
                                                    displayJob = True
                                                End If
                                            End If
                                        Case Else
                                            If cboStatusFilter.SelectedItem.ToString = PlugInData.Statuses(job.CompletedStatus.ToString) Then
                                                displayJob = True
                                            End If
                                    End Select
                                End If
                            End If
                        End If

                        ' Display the job if applicable
                        If displayJob = True Then
                            transItem = New Node
                            adtJobs.Nodes.Add(transItem)
                            transItem.CreateCells()
                            transTypeID = job.InstalledItemTypeID
                            If StaticData.Types.ContainsKey(transTypeID) = True Then
                                transItem.Text = StaticData.Types(transTypeID).Name
                            Else
                                transItem.Text = "Unknown Item ID:" & transTypeID
                            End If

                            transItem.Cells(1).Text = job.ActivityID.ToString
                            transItem.Cells(2).Text = job.Runs.ToString
                            transItem.Cells(3).Text = installerList(job.InstallerID)
                            locationID = job.InstalledItemLocationID
                            If StaticData.Stations.ContainsKey(locationID) = True Then
                                transItem.Cells(4).Text = StaticData.Stations(locationID).StationName
                            Else
                                If StaticData.Stations.ContainsKey(job.OutputLocationID) = True Then
                                    transItem.Cells(4).Text = StaticData.Stations(job.OutputLocationID).StationName
                                Else
                                    transItem.Cells(4).Text = "POS in " & StaticData.SolarSystems(job.InstalledInSolarSystemID).Name
                                End If
                            End If
                            transItem.Cells(5).Text = job.EndProductionTime.ToString
                            transItem.Cells(5).Tag = job.EndProductionTime
                            transItem.Cells(6).Text = SkillFunctions.TimeToString(Int((CDate(transItem.Cells(5).Tag) - Now).TotalMinutes) * 60, False, "Complete")
                            If job.Completed = 0 Then
                                If job.EndProductionTime < DateTime.Now.ToUniversalTime Then
                                    transItem.Cells(7).Text = PlugInData.Statuses("B")
                                Else
                                    transItem.Cells(7).Text = PlugInData.Statuses("A")
                                End If
                            Else
                                transItem.Cells(7).Text = PlugInData.Statuses(job.CompletedStatus.ToString)
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

        Private Sub UpdateIndustryJobTimes()
            For Each transItem As Node In adtJobs.Nodes
                transItem.Cells(6).Text = SkillFunctions.TimeToString(Int((CDate(transItem.Cells(5).Tag) - Now).TotalMinutes) * 60, False, "Complete")
            Next
        End Sub

        Private Sub cboInstallerFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboInstallerFilter.SelectedIndexChanged
            If CBool(cboInstallerFilter.Tag) = False Then
                ' We are not triggering a change in the selected item from the main drawing routine
                Call DisplayIndustryJobs()
            End If
        End Sub

        Private Sub cboActivityFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboActivityFilter.SelectedIndexChanged
            If _startup = False Then
                ' We are not triggering a change in the selected item from the main drawing routine
                Call DisplayIndustryJobs()
            End If
        End Sub

        Private Sub cboStatusFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboStatusFilter.SelectedIndexChanged
            If _startup = False Then
                ' We are not triggering a change in the selected item from the main drawing routine
                Call DisplayIndustryJobs()
            End If
        End Sub

#End Region

#Region "Contracts Routines"

        Private Sub cboContractOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboContractOwner.SelectedIndexChanged
            Call DisplayContracts()
        End Sub

        Private Sub DisplayContracts()

            adtContracts.BeginUpdate()
            adtContracts.Nodes.Clear()

            ' Get the owner we will use
            If cboContractOwner.SelectedItem IsNot Nothing Then
                Dim pOwner As String = cboContractOwner.SelectedItem.ToString
                Dim contractList As SortedList(Of Long, Contract) = Contracts.ParseContracts(pOwner)

                If contractList IsNot Nothing Then

                    ' Get InstallerIDs from the database and return list
                    Dim idList As SortedList(Of Long, String) = Contracts.GetContractIDList(contractList)

                    For Each c As Contract In contractList.Values
                        ' Setup filter result
                        Const DisplayContract As Boolean = True
                        ' Apply filtering...

                        ' Display the result if allowed by filters
                        If DisplayContract = True Then
                            Dim newContract As New Node
                            adtContracts.Nodes.Add(newContract)
                            newContract.CreateCells()
                            newContract.Name = c.ContractID.ToString
                            If c.Title <> "" Then
                                newContract.Text = c.Title
                            Else
                                If c.Items.Count = 1 Then
                                    newContract.Text = StaticData.Types(c.Items.Keys(0)).Name
                                Else
                                    newContract.Text = "Contract ID: " & c.ContractID.ToString
                                End If
                            End If
                            If StaticData.Stations.ContainsKey(c.StartStationID) Then
                                newContract.Cells(1).Text = StaticData.Stations(c.StartStationID).StationName
                            Else
                                newContract.Cells(1).Text = "Station ID: " & c.StartStationID.ToString
                            End If
                            If c.IsIssuer = True Then
                                newContract.Cells(2).Text = "Issued"
                            Else
                                newContract.Cells(2).Text = "Accepted"
                            End If
                            newContract.Cells(3).Text = c.Type.ToString
                            newContract.Cells(4).Text = c.Status.ToString
                            If idList.ContainsKey(c.IssuerID) Then
                                newContract.Cells(5).Text = idList(c.IssuerID)
                            Else
                                newContract.Cells(5).Text = c.IssuerID.ToString
                            End If
                            If idList.ContainsKey(c.AcceptorID) Then
                                newContract.Cells(6).Text = idList(c.AcceptorID)
                            Else
                                newContract.Cells(6).Text = c.AcceptorID.ToString
                            End If
                            newContract.Cells(7).Text = c.DateIssued.ToString
                            newContract.Cells(8).Text = c.DateExpired.ToString
                            newContract.Cells(9).Text = c.Price.ToString("N2")
                            newContract.Cells(10).Text = c.Volume.ToString("N2")

                            ' Add items
                            If c.Items.Count > 0 Then
                                Dim itemCh As New DevComponents.AdvTree.ColumnHeader("Item Name")
                                itemCh.SortingEnabled = False
                                itemCh.Width.Absolute = 300
                                itemCh.DisplayIndex = 1
                                newContract.NodesColumns.Add(itemCh)
                                Dim qtyCh As New DevComponents.AdvTree.ColumnHeader("Quantity")
                                qtyCh.SortingEnabled = False
                                qtyCh.Width.Absolute = 100
                                qtyCh.DisplayIndex = 2
                                newContract.NodesColumns.Add(qtyCh)
                                For Each typeID As Integer In c.Items.Keys
                                    Dim itemNode As New Node
                                    itemNode.Name = CStr(typeID)
                                    itemNode.Text = StaticData.Types(typeID).Name
                                    itemNode.Cells.Add(New Cell(c.Items(typeID).ToString))
                                    newContract.Nodes.Add(itemNode)
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

            AdvTreeSorter.Sort(adtContracts, 1, True, False)
            adtContracts.EndUpdate()

        End Sub

        Private Sub adtContracts_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtContracts.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

#End Region

#Region "Recycler Routines"

        Public Sub RecycleInfoFromAssets()
            ' Fetch the recycling info from the assets control
            _recyclerAssetList = PAC.RecyclingAssetList
            _recyclerAssetOwner = cboRecyclePilots.SelectedItem.ToString
            _recyclerAssetLocation = GetLocationID(PAC.RecyclingAssetLocation)
            Call LoadRecyclingInfo()
            tabPrism.SelectedTab = tiRecycler
            tiRecycler.Visible = True
        End Sub
        Private Function GetLocationID(ByVal item As Node) As Integer
            Do While item.Level > 0
                item = item.Parent
            Loop
            If item.Tag IsNot Nothing Then
                Return CInt(item.Tag)
            Else
                Return 0
            End If
        End Function
        Private Sub LoadRecyclingInfo()

            _itemList.Clear()
            If _recyclerAssetList.Count > 0 Then

                ' Cycle through the items and get the materials
                For Each itemID As Integer In _recyclerAssetList.Keys
                    Dim bpID As Integer = StaticData.GetBPTypeId(CInt(itemID))
                    _itemList.Add(itemID, New SortedList(Of String, Long))
                    If StaticData.Blueprints(bpID).Resources.ContainsKey(6) Then
                        For Each br As EveData.BlueprintResource In StaticData.Blueprints(bpID).Resources(6).Values
                            _itemList(itemID).Add(StaticData.Types(br.TypeId).Name, br.Quantity)
                        Next
                    End If
                Next

            End If

            ' Load the characters into the combobox
            cboRecyclePilots.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    cboRecyclePilots.Items.Add(cPilot.Name)
                End If
            Next

            ' Get the location details
            If StaticData.Stations.ContainsKey(_recyclerAssetLocation) = True Then
                If CDbl(_recyclerAssetLocation) >= 60000000 Then ' Is a station
                    Dim aLocation As Station = StaticData.Stations(_recyclerAssetLocation)
                    lblStation.Text = aLocation.StationName
                    lblCorp.Text = aLocation.CorpId.ToString
                    If StaticData.NpcCorps.ContainsKey(aLocation.CorpId) = True Then
                        lblCorp.Text = CStr(StaticData.NpcCorps(aLocation.CorpId))
                        lblCorp.Tag = aLocation.CorpId.ToString
                        _stationYield = aLocation.RefiningEfficiency
                        lblBaseYield.Text = (_stationYield * 100).ToString("N2")
                    Else
                        If PlugInData.Corps.ContainsKey(aLocation.CorpId.ToString) = True Then
                            lblCorp.Text = CStr(PlugInData.Corps(aLocation.CorpId.ToString))
                            lblBaseYield.Text = (aLocation.RefiningEfficiency * 100).ToString("N2")
                        Else
                            lblCorp.Text = "Unknown"
                            lblCorp.Tag = Nothing
                            lblBaseYield.Text = CDbl(50).ToString("N2")
                        End If
                    End If
                Else ' Is a system
                    lblStation.Text = "n/a"
                    lblCorp.Text = "n/a"
                    lblCorp.Tag = Nothing
                    lblBaseYield.Text = CDbl(50).ToString("N2")
                End If
            Else
                lblStation.Text = "n/a"
                lblCorp.Text = "n/a"
                lblCorp.Tag = Nothing
                lblBaseYield.Text = CDbl(50).ToString("N2")
            End If

            ' Set the pilot to the recycling one
            If cboRecyclePilots.Items.Contains(_recyclerAssetOwner) Then
                cboRecyclePilots.SelectedItem = _recyclerAssetOwner
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
            Dim price As Double
            Dim perfect As Long
            Dim quantity As Long
            Dim quant As Long
            Dim wastage As Long
            Dim taken As Long
            Dim value As Double
            Dim fees As Double
            Dim sale As Double
            Dim recycleTotal As Double
            Dim newClvItem As Node
            Dim newClvSubItem As Node
            Dim itemInfo As EveType
            Dim batches As Integer
            Dim items As Long = 0
            Dim volume As Double = 0
            Dim benefit As Double
            Dim tempNetYield As Double = 0
            Dim bestPriceTotal As Double = 0
            Dim salePriceTotal As Double = 0
            Dim refinePriceTotal As Double = 0
            Dim recycleResults As New SortedList
            Dim recycleWaste As New SortedList
            Dim recycleTake As New SortedList
            Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
            Dim priceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From a In _recyclerAssetList.Keys Select a)
            priceTask.Wait()
            Dim prices As Dictionary(Of Integer, Double) = priceTask.Result
            For Each asset As Integer In _recyclerAssetList.Keys
                itemInfo = StaticData.Types(asset)
                If itemInfo.Category = 25 Then
                    Select Case itemInfo.Group
                        Case 465 ' Ice
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.IceProc)))) + _baseYield
                        Case 450 ' Arkonor
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.ArkonorProc)))) + _baseYield
                        Case 451 ' Bistot
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.BistotProc)))) + _baseYield
                        Case 452 ' Crokite
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.CrokiteProc)))) + _baseYield
                        Case 453 ' Dark Ochre
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.DarkOchreProc)))) + _baseYield
                        Case 467 ' Gneiss
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.GneissProc)))) + _baseYield
                        Case 454 ' Hedbergite
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.HedbergiteProc)))) + _baseYield
                        Case 455 ' Hemorphite
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.HemorphiteProc)))) + _baseYield
                        Case 456 ' Jaspet
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.JaspetProc)))) + _baseYield
                        Case 457 ' Kernite
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.KerniteProc)))) + _baseYield
                        Case 468 ' Mercoxit
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.MercoxitProc)))) + _baseYield
                        Case 469 ' Omber
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.OmberProc)))) + _baseYield
                        Case 458 ' Plagioclase
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.PlagioclaseProc)))) + _baseYield
                        Case 459 ' Pyroxeres
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.PyroxeresProc)))) + _baseYield
                        Case 460 ' Scordite
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.ScorditeProc)))) + _baseYield
                        Case 461 ' Spodumain
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.SpodumainProc)))) + _baseYield
                        Case 462 ' Veldspar
                            tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.VeldsparProc)))) + _baseYield
                    End Select
                Else
                    tempNetYield = (_netYield - _baseYield) * (1 + (0.05 * CDbl(rPilot.KeySkills(KeySkill.ScrapMetalProc)))) + _baseYield
                End If
                tempNetYield = Math.Min(tempNetYield, 1)
                _matList = _itemList(asset)
                newClvItem = New Node
                adtRecycle.Nodes.Add(newClvItem)
                newClvItem.CreateCells()
                newClvItem.Text = itemInfo.Name
                newClvItem.Tag = itemInfo.Id
                price = Math.Round(prices(asset), 2, MidpointRounding.AwayFromZero)
                batches = CInt(Int(CLng(_recyclerAssetList(itemInfo.Id)) / itemInfo.PortionSize))
                quantity = CLng(_recyclerAssetList(asset))
                volume += itemInfo.Volume * quantity
                items += CLng(quantity)
                value = price * quantity
                fees = Math.Round(value * (_rTotalFees / 100), 2, MidpointRounding.AwayFromZero)
                sale = value - fees
                newClvItem.Cells(1).Text = itemInfo.MetaLevel.ToString("N0")
                newClvItem.Cells(2).Text = quantity.ToString("N0")
                newClvItem.Cells(3).Text = batches.ToString("N0")
                newClvItem.Cells(4).Text = price.ToString("N2")
                newClvItem.Cells(5).Text = value.ToString("N2")
                If chkFeesOnItems.Checked = True Then
                    newClvItem.Cells(6).Text = fees.ToString("N2")
                    newClvItem.Cells(7).Text = sale.ToString("N2")
                Else
                    newClvItem.Cells(7).Text = value.ToString("N2")
                End If
                recycleTotal = 0
                If _matList IsNot Nothing Then ' i.e. it can be refined
                    Dim matPriceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From m In _matList.Keys Select StaticData.TypeNames(CStr(m)))
                    matPriceTask.Wait()
                    Dim matPrices As Dictionary(Of Integer, Double) = matPriceTask.Result
                    For Each mat As String In _matList.Keys
                        price = Math.Round(matPrices(StaticData.TypeNames(mat)), 2, MidpointRounding.AwayFromZero)
                        perfect = CLng(_matList(mat)) * batches
                        wastage = CLng(perfect * (1 - tempNetYield))
                        quant = CLng(perfect * tempNetYield)
                        taken = CLng(quant * (_stationTake / 100))
                        quant = quant - taken
                        value = price * quant
                        fees = Math.Round(value * (_rTotalFees / 100), 2, MidpointRounding.AwayFromZero)
                        sale = value - fees
                        newClvSubItem = New Node
                        newClvItem.Nodes.Add(newClvSubItem)
                        newClvSubItem.CreateCells()
                        newClvSubItem.Text = mat
                        newClvSubItem.Cells(2).Text = quant.ToString("N0")
                        newClvSubItem.Cells(3).Text = quant.ToString("N0")
                        newClvSubItem.Cells(4).Text = price.ToString("N2")
                        newClvSubItem.Cells(5).Text = value.ToString("N2")
                        If chkFeesOnRefine.Checked = True Then
                            newClvSubItem.Cells(6).Text = fees.ToString("N2")
                            newClvSubItem.Cells(8).Text = sale.ToString("N2")
                            recycleTotal += sale
                        Else
                            newClvSubItem.Cells(8).Text = newClvSubItem.Cells(5).Text
                            recycleTotal += value
                        End If
                        ' Save the perfect refining quantity
                        If recycleResults.Contains(mat) = False Then
                            recycleResults.Add(mat, quant)
                        Else
                            recycleResults(mat) = CDbl(recycleResults(mat)) + quant
                        End If
                        ' Save the wasted amounts
                        If recycleWaste.Contains(mat) = False Then
                            recycleWaste.Add(mat, wastage)
                        Else
                            recycleWaste(mat) = CDbl(recycleWaste(mat)) + wastage
                        End If
                        ' Save the take amounts
                        If recycleTake.Contains(mat) = False Then
                            recycleTake.Add(mat, taken)
                        Else
                            recycleTake(mat) = CDbl(recycleTake(mat)) + taken
                        End If
                    Next
                End If
                newClvItem.Cells(8).Text = recycleTotal.ToString("N2")
                If CDbl(newClvItem.Cells(8).Text) > CDbl(newClvItem.Cells(7).Text) Then
                    newClvItem.Style = adtRecycle.Styles("ItemGood")
                    newClvItem.Cells(9).Text = newClvItem.Cells(8).Text
                Else
                    newClvItem.Cells(9).Text = newClvItem.Cells(7).Text
                End If
                benefit = CDbl(newClvItem.Cells(8).Text) - CDbl(newClvItem.Cells(7).Text)
                newClvItem.Cells(10).Tag = benefit
                newClvItem.Cells(10).Text = benefit.ToString("N2")
                newClvItem.Cells(11).Tag = (benefit / quantity)
                newClvItem.Cells(11).Text = (benefit / quantity).ToString("N2")
                salePriceTotal += CDbl(newClvItem.Cells(7).Text)
                refinePriceTotal += CDbl(newClvItem.Cells(8).Text)
                bestPriceTotal += CDbl(newClvItem.Cells(9).Text)
            Next
            lblPriceTotals.Text = "Sale / Refine / Best Totals: " & salePriceTotal.ToString("N2") & " / " & refinePriceTotal.ToString("N2") & " / " & bestPriceTotal.ToString("N2")
            AdvTreeSorter.Sort(adtRecycle, 1, True, True)
            adtRecycle.EndUpdate()
            lblVolume.Text = volume.ToString("N2") & " m³"
            lblItems.Text = adtRecycle.Nodes.Count.ToString("N0")
            lblItems.Text &= " (" & items.ToString("N0") & ")"
            ' Create the totals list
            adtTotals.BeginUpdate()
            adtTotals.Nodes.Clear()
            If recycleResults IsNot Nothing Then
                Dim matPriceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From m In recycleResults.Keys Select StaticData.TypeNames(CStr(m)))
                matPriceTask.Wait()
                Dim matPrices As Dictionary(Of Integer, Double) = matPriceTask.Result
                For Each mat As String In recycleResults.Keys
                    price = Math.Round(matPrices(StaticData.TypeNames(mat)), 2, MidpointRounding.AwayFromZero)
                    wastage = CLng(recycleWaste(mat))
                    taken = CLng(recycleTake(mat))
                    quant = CLng(recycleResults(mat))
                    newClvItem = New Node
                    adtTotals.Nodes.Add(newClvItem)
                    newClvItem.CreateCells()
                    newClvItem.Text = mat
                    newClvItem.Cells(1).Text = taken.ToString("N0")
                    newClvItem.Cells(2).Text = wastage.ToString("N0")
                    newClvItem.Cells(3).Text = quant.ToString("N0")
                    newClvItem.Cells(4).Text = price.ToString("N2")
                    newClvItem.Cells(5).Text = (price * quant).ToString("N2")
                Next
            End If
            AdvTreeSorter.Sort(adtTotals, 1, True, True)
            adtTotals.EndUpdate()
        End Sub

        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboRecyclePilots.SelectedIndexChanged
            Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
            If chkPerfectRefine.Checked = True Then
                _netYield = 1
            Else
                _netYield = (_baseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(KeySkill.RefiningEfficiency)) * 0.04)))
            End If
            lblBaseYield.Text = (_baseYield * 100).ToString("N2") & "%"
            lblNetYield.Text = (_netYield * 100).ToString("N2") & "%"
            If lblCorp.Tag IsNot Nothing Then
                _stationStanding = Standings.GetStanding(rPilot.Name, lblCorp.Tag.ToString, True)
            Else
                _stationStanding = 0
            End If
            ' Update Standings
            If chkOverrideStandings.Checked = True Then
                lblStandings.Text = nudStandings.Value.ToString("N2")
            Else
                If lblCorp.Tag Is Nothing Then
                    lblStandings.Text = CDbl(0).ToString("N2")
                Else
                    lblStandings.Text = _stationStanding.ToString("N2")
                End If
            End If
            ' Update Broker Fee
            If chkOverrideBrokerFee.Checked = False Then
                _rBrokerFee = 1 * (1 - 0.05 * (CInt(rPilot.KeySkills(KeySkill.BrokerRelations))))
            Else
                _rBrokerFee = nudBrokerFee.Value
            End If
            ' Update Trans Tax
            If chkOverrideTax.Checked = False Then
                _rTransTax = 1 * (1.5 - 0.15 * (CInt(rPilot.KeySkills(KeySkill.Accounting))))
            Else
                _rTransTax = nudTax.Value
            End If
            _rTotalFees = _rBrokerFee + _rTransTax
            lblTotalFees.Text = _rTotalFees.ToString("N2") & "%"
            Call RecalcRecycling()
        End Sub

        Private Sub chkFeesOnRefine_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkFeesOnRefine.CheckedChanged
            Call RecalcRecycling()
        End Sub

        Private Sub chkFeesOnItems_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkFeesOnItems.CheckedChanged
            Call RecalcRecycling()
        End Sub

        Private Sub adtRecycle_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtRecycle.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub

        Private Sub adtRecycle_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles adtRecycle.KeyDown
            If e.Control = True And e.KeyCode = Keys.A Then
                adtRecycle.SelectedNodes.Clear()
                For Each rNode As Node In adtRecycle.Nodes
                    adtRecycle.SelectedNodes.Add(rNode)
                Next
            End If
        End Sub

        Private Sub adtTotals_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtTotals.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub

#Region "Override Base Yield functions"
        Private Sub chkOverrideBaseYield_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOverrideBaseYield.CheckedChanged
            If chkOverrideBaseYield.Checked = True Then
                _baseYield = CDbl(nudBaseYield.Value) / 100
            Else
                _baseYield = _stationYield
            End If
            If cboRecyclePilots.SelectedItem IsNot Nothing Then
                Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
                lblBaseYield.Text = (_baseYield * 100).ToString("N2") & "%"
                _netYield = (_baseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(KeySkill.RefiningEfficiency)) * 0.04)))
                lblNetYield.Text = (_netYield * 100).ToString("N2") & "%"
                Call RecalcRecycling()
            End If
        End Sub

        Private Sub nudBaseYield_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudBaseYield.ValueChanged
            If chkOverrideBaseYield.Checked = True Then
                _baseYield = CDbl(nudBaseYield.Value) / 100
                lblBaseYield.Text = (_baseYield * 100).ToString("N2") & "%"
                Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
                _netYield = (_baseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(KeySkill.RefiningEfficiency)) * 0.04)))
                lblNetYield.Text = (_netYield * 100).ToString("N2") & "%"
                Call RecalcRecycling()
            End If
        End Sub
#End Region

#Region "Override Standings functions"
        Private Sub chkOverrideStandings_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOverrideStandings.CheckedChanged
            If chkOverrideStandings.Checked = True Then
                lblStandings.Text = nudStandings.Value.ToString("N2")
            Else
                If lblCorp.Tag Is Nothing Then
                    lblStandings.Text = CDbl(0).ToString("N2")
                Else
                    lblStandings.Text = _stationStanding.ToString("N2")
                End If
            End If
            Call RecalcRecycling()
        End Sub

        Private Sub lblStandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lblStandings.TextChanged
            _stationTake = Math.Max(5 - (0.75 * CDbl(lblStandings.Text)), 0)
            lblStationTake.Text = _stationTake.ToString("N2") & "%"
        End Sub

        Private Sub nudStandings_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudStandings.ValueChanged
            If chkOverrideStandings.Checked = True Then
                lblStandings.Text = nudStandings.Value.ToString("N2")
                Call RecalcRecycling()
            End If
        End Sub

#End Region

#Region "Override Refining Skills functions"
        Private Sub chkPerfectRefine_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkPerfectRefine.CheckedChanged
            If chkPerfectRefine.Checked = True Then
                _netYield = 1
            Else
                Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
                _netYield = (_baseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(KeySkill.RefiningEfficiency)) * 0.04)))
            End If
            lblNetYield.Text = (_netYield * 100).ToString("N2") & "%"
            Call RecalcRecycling()
        End Sub
#End Region

#Region "Override Fees functions"
        Private Sub chkOverrideBrokerFee_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOverrideBrokerFee.CheckedChanged
            Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
            If chkOverrideBrokerFee.Checked = False Then
                _rBrokerFee = 1 * (1 - 0.05 * (CInt(rPilot.KeySkills(KeySkill.BrokerRelations))))
            Else
                _rBrokerFee = nudBrokerFee.Value
            End If
            _rTotalFees = _rBrokerFee + _rTransTax
            lblTotalFees.Text = _rTotalFees.ToString("N2") & "%"
            Call RecalcRecycling()
        End Sub
        Private Sub chkOverrideTax_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOverrideTax.CheckedChanged
            Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
            If chkOverrideTax.Checked = False Then
                _rTransTax = 1 * (1.5 - 0.15 * (CInt(rPilot.KeySkills(KeySkill.Accounting))))
            Else
                _rTransTax = nudTax.Value
            End If
            _rTotalFees = _rBrokerFee + _rTransTax
            lblTotalFees.Text = _rTotalFees.ToString("N2") & "%"
            Call RecalcRecycling()
        End Sub
        Private Sub nudBrokerFee_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudBrokerFee.ValueChanged
            If chkOverrideBrokerFee.Checked = True Then
                _rBrokerFee = nudBrokerFee.Value
                _rTotalFees = _rBrokerFee + _rTransTax
                lblTotalFees.Text = _rTotalFees.ToString("N2") & "%"
                Call RecalcRecycling()
            End If
        End Sub
        Private Sub nudTax_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudTax.ValueChanged
            If chkOverrideTax.Checked = True Then
                _rTransTax = nudTax.Value
                _rTotalFees = _rBrokerFee + _rTransTax
                lblTotalFees.Text = _rTotalFees.ToString("N2") & "%"
                Call RecalcRecycling()
            End If
        End Sub
#End Region

#Region "Refining Mode functions"
        Private Sub cboRefineMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboRefineMode.SelectedIndexChanged
            Select Case cboRefineMode.SelectedIndex
                Case 0 ' Standard
                    If chkOverrideBaseYield.Checked = True Then
                        _baseYield = CDbl(nudBaseYield.Value) / 100
                    Else
                        _baseYield = _stationYield
                    End If
                    lblBaseYield.Text = (_baseYield * 100).ToString("N2") & "%"
                    If chkPerfectRefine.Checked = True Then
                        _netYield = 1
                    Else
                        Dim rPilot As EveHQPilot
                        If cboRecyclePilots.SelectedItem IsNot Nothing Then
                            rPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
                            _netYield = (_baseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(KeySkill.RefiningEfficiency)) * 0.04)))
                        Else
                            If cboRecyclePilots.Items.Count > 0 Then
                                cboRecyclePilots.SelectedIndex = 0
                                rPilot = HQ.Settings.Pilots(cboRecyclePilots.SelectedItem.ToString)
                                _netYield = (_baseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(KeySkill.RefiningEfficiency)) * 0.04)))
                            Else
                                _netYield = 0
                            End If
                        End If
                    End If
                    lblNetYield.Text = (_netYield * 100).ToString("N2") & "%"
                    If chkOverrideStandings.Checked = True Then
                        lblStandings.Text = nudStandings.Value.ToString("N2")
                    Else
                        If lblCorp.Tag Is Nothing Then
                            lblStandings.Text = CDbl(0).ToString("N2")
                        Else
                            lblStandings.Text = _stationStanding.ToString("N2")
                        End If
                    End If
                    chkOverrideBaseYield.Enabled = True
                    chkOverrideStandings.Enabled = True
                    chkPerfectRefine.Enabled = True
                    nudBaseYield.Enabled = True
                    nudStandings.Enabled = True
                    cboRecyclePilots.Enabled = True
                Case 1 ' Refining Array
                    _baseYield = 0.35
                    _netYield = 0.35
                    lblBaseYield.Text = (_baseYield * 100).ToString("N2") & "%"
                    lblNetYield.Text = (_netYield * 100).ToString("N2") & "%"
                    lblStandings.Text = CDbl(10).ToString("N2")
                    chkOverrideBaseYield.Enabled = False
                    chkOverrideStandings.Enabled = False
                    chkPerfectRefine.Enabled = False
                    nudBaseYield.Enabled = False
                    nudStandings.Enabled = False
                    cboRecyclePilots.Enabled = False
                Case 2 ' Intensive Refining Array
                    _baseYield = 0.75
                    _netYield = 0.75
                    lblBaseYield.Text = (_baseYield * 100).ToString("N2") & "%"
                    lblNetYield.Text = (_netYield * 100).ToString("N2") & "%"
                    lblStandings.Text = CDbl(10).ToString("N2")
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
                Call RecalcRecycling()
            End If
        End Sub
#End Region

        Private Sub mnuAlterRecycleQuantity_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAlterRecycleQuantity.Click
            If adtRecycle.SelectedNodes.Count > 0 Then
                Using newQ As New FrmSelectQuantity(CInt(_recyclerAssetList(CInt(adtRecycle.SelectedNodes(0).Tag))))
                    newQ.ShowDialog()
                    For Each rNode As Node In adtRecycle.SelectedNodes
                        _recyclerAssetList(CInt(rNode.Tag)) = newQ.Quantity
                    Next
                End Using
                Call RecalcRecycling()
            End If
        End Sub

        Private Sub mnuRemoveRecycleItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRemoveRecycleItem.Click
            If adtRecycle.SelectedNodes.Count > 0 Then
                For Each rNode As Node In adtRecycle.SelectedNodes
                    _recyclerAssetList.Remove(CInt(rNode.Tag))
                Next
                Call RecalcRecycling()
            End If
        End Sub

        Private Sub mnuExportToCSV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExportToCSV.Click
            Call ExportToClipboard("PRISM Item Recycling Analysis", adtRecycle, HQ.Settings.CsvSeparatorChar)
        End Sub

        Private Sub mnuExportToTSV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExportToTSV.Click
            Call ExportToClipboard("PRISM Item Recycling Analysis", adtRecycle, ControlChars.Tab)
        End Sub

        Private Sub mnuExportTotalsToCSV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExportTotalsToCSV.Click
            Call ExportToClipboard("PRISM Item Recycling Totals", adtTotals, HQ.Settings.CsvSeparatorChar)
        End Sub

        Private Sub mnuExportTotalsToTSV_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExportTotalsToTSV.Click
            Call ExportToClipboard("PRISM Item Recycling Totals", adtTotals, ControlChars.Tab)
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

        Private Sub ctxRecycleItem_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxRecycleItem.Opening
            If adtRecycle.SelectedNodes.Count > 0 Then
                If adtRecycle.SelectedNodes(0).Level = 0 Then
                    mnuAlterRecycleQuantity.Enabled = True
                    mnuRemoveRecycleItem.Enabled = True
                Else
                    e.Cancel = True
                End If
            Else
                mnuAlterRecycleQuantity.Enabled = False
                mnuRemoveRecycleItem.Enabled = False
            End If
        End Sub

        Private Sub mnuAddRecycleItem_Click_1(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddRecycleItem.Click
            Using newI As New FrmSelectItem
                newI.ShowDialog()
                Dim itemName As String = newI.Item
                If itemName IsNot Nothing Then
                    Dim itemID As Integer = StaticData.TypeNames(itemName)
                    If _recyclerAssetList.ContainsKey(itemID) = False Then
                        _recyclerAssetList.Add(itemID, 1)
                    End If
                    Call LoadRecyclingInfo()
                End If
            End Using
        End Sub

#End Region

#Region "CSV Export Routines"

        Private Sub btnExportTransactions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportTransactions.Click
            Call GenerateCsvFileFromClv(cboTransactionOwner.Text, "Wallet Transactions", adtTransactions)
        End Sub

        Private Sub btnExportJournal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportJournal.Click
            'TODO: Update the called routine
            Call GenerateCsvFileFromClv(cboJournalOwners.Text, "Wallet Journal", adtJournal)
        End Sub

        Private Sub btnExportJobs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportJobs.Click
            Call GenerateCsvFileFromClv(cboJobOwner.SelectedItem.ToString, "Industry Jobs", adtJobs)
        End Sub

        'Private Sub btnExportOrders_Click(ByVal sender As Object, ByVal e As EventArgs)
        '    Call GenerateCSVFileFromCLV(cboOrdersOwner.SelectedItem.ToString, "Sell Orders", adtSellOrders)
        '    Call GenerateCSVFileFromCLV(cboOrdersOwner.SelectedItem.ToString, "Buy Orders", adtBuyOrders)
        'End Sub

        Private Sub GenerateCsvFileFromClv(ByVal ownerName As String, ByVal description As String, ByVal cAdvTree As AdvTree)

            Try
                Dim csvFile As String = Path.Combine(HQ.ReportFolder, description.Replace(" ", "") & " - " & ownerName & " (" & Format(Now, "yyyy-MM-dd HH-mm-ss") & ").csv")
                Dim csvText As New StringBuilder
                With cAdvTree
                    ' Write the columns
                    For col As Integer = 0 To .Columns.Count - 1
                        csvText.Append(.Columns(col).Text)
                        If col <> .Columns.Count - 1 Then
                            csvText.Append(HQ.Settings.CsvSeparatorChar)
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
                                csvText.Append(HQ.Settings.CsvSeparatorChar)
                            End If
                        Next
                        csvText.AppendLine("")
                    Next
                End With
                Dim sw As New StreamWriter(csvFile)
                sw.Write(csvText.ToString)
                sw.Flush()
                sw.Close()
                MessageBox.Show(description & " successfully exported to " & csvFile, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("There was an error writing the " & description & " File. The error was: " & ControlChars.CrLf & ControlChars.CrLf & ex.Message, "Error Writing File", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

#End Region

#Region "BPManager Routines"

        Private Sub btnBPCalc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBPCalc.Click
            If adtBlueprints.SelectedNodes.Count = 1 Then
                Dim bpName As String = adtBlueprints.SelectedNodes(0).Text
                If chkShowOwnedBPs.Checked = True Then
                    ' Start an owned BPCalc
                    If adtBlueprints.SelectedNodes(0).Tag IsNot Nothing Then
                        Dim bpid As Long = CLng(adtBlueprints.SelectedNodes(0).Tag)
                        Dim bpCalc As New frmBPCalculator(PrismSettings.UserSettings.DefaultBPOwner, bpid)
                        Call OpenBPCalculator(bpCalc)
                    End If
                Else
                    ' Start a standard BPCalc
                    Dim bpCalc As New frmBPCalculator(bpName)
                    Call OpenBPCalculator(bpCalc)
                End If
            ElseIf adtBlueprints.SelectedNodes.Count = 0 Then
                ' Start a blank BP Calc
                Dim bpCalc As New frmBPCalculator(chkShowOwnedBPs.Checked)
                Call OpenBPCalculator(bpCalc)
            End If
        End Sub

        Private Sub cboBPOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboBPOwner.SelectedIndexChanged

            ' Check for filter changes, but set the flag to avoid invoking other changes at this point
            _bpManagerUpdate = True

            If cboTechFilter.SelectedIndex = -1 Then cboTechFilter.SelectedIndex = 0
            If cboTypeFilter.SelectedIndex = -1 Then cboTypeFilter.SelectedIndex = 0
            If cboCategoryFilter.SelectedIndex = -1 Then cboCategoryFilter.SelectedIndex = 0

            _bpManagerUpdate = False

            Call UpdateBPList()
        End Sub

        Private Sub chkShowOwnedBPs_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkShowOwnedBPs.CheckedChanged
            Call UpdateBPList()
        End Sub

        Private Sub UpdateBPList()
            ' Check if we are showing the full list or the owners list
            If chkShowOwnedBPs.Checked = False Then
                Dim search As String = txtBPSearch.Text
                ' Show the full BP list
                adtBlueprints.BeginUpdate()
                adtBlueprints.Nodes.Clear()
                Dim matchCat As Boolean
                For Each blueprint As EveData.Blueprint In StaticData.Blueprints.Values
                    Dim bpName As String = StaticData.Types(blueprint.Id).Name
                    If cboTechFilter.SelectedIndex = 0 Or (cboTechFilter.SelectedIndex = blueprint.TechLevel) Then
                        matchCat = False
                        If cboCategoryFilter.SelectedIndex = 0 Then
                            matchCat = True
                        Else
                            If PlugInData.CategoryNames.ContainsKey(cboCategoryFilter.SelectedItem.ToString) Then
                                If StaticData.Types.ContainsKey(blueprint.ProductId) Then
                                    If CInt(PlugInData.CategoryNames(cboCategoryFilter.SelectedItem.ToString)) = StaticData.Types(blueprint.ProductId).Category Then
                                        matchCat = True
                                    End If
                                End If
                            End If
                        End If
                        If matchCat = True Then
                            If search = "" Or bpName.ToLower.Contains(search.ToLower) Then
                                Dim newBPItem As New Node
                                newBPItem.Text = bpName
                                adtBlueprints.Nodes.Add(newBPItem)
                                newBPItem.CreateCells()
                                newBPItem.Cells(1).Text = "n/a"
                                newBPItem.Cells(2).Text = "n/a"
                                newBPItem.Cells(3).Text = blueprint.TechLevel.ToString
                                newBPItem.Cells(4).Text = "0"
                                newBPItem.Cells(5).Text = "0"
                                newBPItem.Cells(6).Text = "Infinite"
                                newBPItem.Cells(7).Text = "n/a"
                            End If
                        End If
                    End If
                Next
                AdvTreeSorter.Sort(adtBlueprints, 1, True, True)
                adtBlueprints.EndUpdate()
            Else
                ' Show the owned BP list
                Call UpdateOwnerBPList()
            End If
        End Sub

        Private Sub UpdateOwnerBPList()
            Dim search As String = txtBPSearch.Text
            ' Establish the owner
            If cboBPOwner.SelectedItem IsNot Nothing Then
                Dim prismOwner As String = cboBPOwner.SelectedItem.ToString()

                adtBlueprints.BeginUpdate()
                adtBlueprints.Nodes.Clear()
                If prismOwner <> "" Then
                    ' Fetch the ownerBPs if it exists
                    Dim ownerBPs As New SortedList(Of Long, BlueprintAsset)
                    If PlugInData.BlueprintAssets.ContainsKey(prismOwner) = True Then
                        ownerBPs = PlugInData.BlueprintAssets(prismOwner)
                    End If
                    Dim bpData As EveData.Blueprint
                    Dim locationName As String
                    Dim matchCat As Boolean
                    For Each blueprint As BlueprintAsset In ownerBPs.Values
                        If blueprint.LocationDetails Is Nothing Then blueprint.LocationDetails = "" ' Resets details
                        If blueprint.LocationID Is Nothing Then blueprint.LocationID = "0" ' Resets details
                        If StaticData.Blueprints.ContainsKey(CInt(blueprint.TypeID)) Then
                            bpData = StaticData.Blueprints(CInt(blueprint.TypeID))
                            locationName = Locations.GetLocationNameFromID(CInt(blueprint.LocationID))
                            If cboTechFilter.SelectedIndex = 0 Or (cboTechFilter.SelectedIndex = bpData.TechLevel) Then
                                If cboTypeFilter.SelectedIndex = 0 Or (cboTypeFilter.SelectedIndex = blueprint.BPType + 1) Then
                                    matchCat = False
                                    If cboCategoryFilter.SelectedIndex = 0 Then
                                        matchCat = True
                                    Else
                                        If PlugInData.CategoryNames.ContainsKey(cboCategoryFilter.SelectedItem.ToString) Then
                                            If CInt(PlugInData.CategoryNames(cboCategoryFilter.SelectedItem.ToString)) = StaticData.Types(bpData.ProductId).Category Then
                                                matchCat = True
                                            End If
                                        End If
                                    End If
                                    If matchCat = True Then
                                        Dim bpName As String = StaticData.Types(CInt(blueprint.TypeID)).Name
                                        If search = "" Or bpName.ToLower.Contains(search.ToLower) Or blueprint.LocationDetails.ToLower.Contains(search.ToLower) Or locationName.ToLower.Contains(search.ToLower) Then
                                            Dim newBpItem As New Node
                                            adtBlueprints.Nodes.Add(newBpItem)
                                            newBpItem.CreateCells()
                                            newBpItem.Text = bpName
                                            newBpItem.Tag = blueprint.AssetID
                                            newBpItem.Cells(3).Text = bpData.TechLevel.ToString
                                            Call UpdateOwnerBPItem(prismOwner, locationName, blueprint, newBpItem)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                AdvTreeSorter.Sort(adtBlueprints, 1, True, True)
                adtBlueprints.EndUpdate()
            End If
        End Sub
        Private Sub UpdateOwnerBPItem(ByVal pOwner As String, ByVal locationName As String, ByVal bpAsset As BlueprintAsset, ByVal newBPItem As Node)
            newBPItem.Cells(4).Text = bpAsset.MELevel.ToString("N0")
            newBPItem.Cells(5).Text = bpAsset.PELevel.ToString("N0")
            Select Case bpAsset.BPType
                Case BPType.Unknown  ' Undetermined
                    newBPItem.Cells(1).Text = locationName
                    newBPItem.Cells(2).Text = bpAsset.LocationDetails
                    newBPItem.Cells(6).Text = "Unknown"
                    newBPItem.Cells(6).Tag = bpAsset.Runs
                    newBPItem.Style = _bpmStyleUnknown
                Case BPType.Original  ' BPO
                    newBPItem.Cells(1).Text = locationName
                    newBPItem.Cells(2).Text = bpAsset.LocationDetails
                    newBPItem.Cells(6).Text = "BPO"
                    newBPItem.Cells(6).Tag = 1000000
                    newBPItem.Style = _bpmStyleBpo
                Case BPType.Copy ' BPC
                    newBPItem.Cells(1).Text = locationName
                    newBPItem.Cells(2).Text = bpAsset.LocationDetails
                    newBPItem.Cells(6).Text = bpAsset.Runs.ToString("N0")
                    newBPItem.Cells(6).Tag = bpAsset.Runs
                    newBPItem.Style = _bpmStyleBPC
                Case BPType.User
                    newBPItem.Cells(1).Text = pOwner & "'s Secret BP Stash"
                    newBPItem.Cells(2).Text = pOwner & "'s Secret BP Stash"
                    newBPItem.Cells(6).Text = "BPO"
                    newBPItem.Cells(6).Tag = 1000000
                    newBPItem.Style = _bpmStyleUser
            End Select
            newBPItem.Cells(7).Text = [Enum].GetName(GetType(BPStatus), bpAsset.Status)
            newBPItem.Cells(7).Tag = bpAsset.Status
            Select Case bpAsset.Status
                Case BPStatus.Missing
                    newBPItem.Style = _bpmStyleMissing
                Case BPStatus.Exhausted
                    newBPItem.Style = _bpmStyleExhausted
            End Select
        End Sub

        Private Sub btnUpdateBPsFromAssets_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateBPsFromAssets.Click

            ' Get the owner we will use
            Dim pOwner As PrismOwner
            If cboBPOwner.SelectedItem IsNot Nothing Then
                If PlugInData.PrismOwners.ContainsKey(cboBPOwner.SelectedItem.ToString) Then
                    pOwner = PlugInData.PrismOwners(cboBPOwner.SelectedItem.ToString)

                    ' Fetch the ownerBPs if it exists
                    Dim ownerBPs As New SortedList(Of Long, BlueprintAsset)
                    If PlugInData.BlueprintAssets.ContainsKey(pOwner.Name) = True Then
                        ownerBPs = PlugInData.BlueprintAssets(pOwner.Name)
                    Else
                        PlugInData.BlueprintAssets.Add(pOwner.Name, ownerBPs)
                    End If

                    Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.Assets)
                    Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Assets)
                    Dim assetXML As XmlDocument
                    Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

                    If pOwner.IsCorp = True Then
                        assetXML = apireq.GetAPIXML(APITypes.AssetsCorp, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                    Else
                        assetXML = apireq.GetAPIXML(APITypes.AssetsChar, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                    End If

                    If assetXML IsNot Nothing Then
                        Dim assets As New SortedList(Of Long, BlueprintAsset)
                        Dim locList As XmlNodeList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                        If locList.Count > 0 Then
                            ' Define what we want to obtain
                            Dim categories, groups, types As New ArrayList
                            categories.Add(9) ' Blueprints
                            For Each loc As XmlNode In locList
                                Dim locationID As String = loc.Attributes.GetNamedItem("locationID").Value
                                Dim flagID As Integer = CInt(loc.Attributes.GetNamedItem("flag").Value)
                                Dim locationDetails As String = StaticData.ItemMarkers(flagID)
                                Dim bpcFlag As Boolean = False
                                ' Check the asset
                                Dim itemData As EveType
                                Dim assetID As Long
                                Dim itemID As Integer
                                assetID = CLng(loc.Attributes.GetNamedItem("itemID").Value)
                                itemID = CInt(loc.Attributes.GetNamedItem("typeID").Value)
                                If StaticData.Types.ContainsKey(itemID) Then
                                    itemData = StaticData.Types(itemID)
                                    ' Check for BPO/BPC
                                    If itemData.Category = 9 Then
                                        If loc.Attributes.GetNamedItem("singleton").Value = "1" Then
                                            If loc.Attributes.GetNamedItem("rawQuantity") IsNot Nothing Then
                                                If loc.Attributes.GetNamedItem("rawQuantity").Value = "-2" Then
                                                    bpcFlag = True
                                                End If
                                            End If
                                        End If
                                    End If
                                    If flagID = 0 Then
                                        If PlugInData.AssetItemNames.ContainsKey(assetID) = True Then
                                            locationDetails = PlugInData.AssetItemNames(assetID)
                                        Else
                                            locationDetails = itemData.Name
                                        End If
                                    End If
                                    If categories.Contains(itemData.Category) Or groups.Contains(itemData.Group) Or types.Contains(itemData.Id) Then
                                        Dim newBP As New BlueprintAsset
                                        newBP.AssetID = CStr(assetID)
                                        newBP.LocationID = locationID
                                        If pOwner.IsCorp = True Then
                                            Dim accountID As Integer = flagID + 885
                                            If accountID = 889 Then accountID = 1000
                                            If _divisions.ContainsKey(pOwner.ID & "_" & accountID.ToString) = True Then
                                                locationDetails = CStr(_divisions.Item(pOwner.ID & "_" & accountID.ToString))
                                            End If
                                        End If
                                        If newBP.BPType = BPType.Unknown Then
                                            If bpcFlag = True Then
                                                newBP.BPType = BPType.Copy
                                                newBP.Runs = 1
                                            Else
                                                newBP.BPType = BPType.Original
                                                newBP.Runs = -1
                                            End If
                                        End If
                                        newBP.LocationDetails = locationDetails
                                        newBP.TypeID = CStr(itemID)
                                        newBP.Status = BPStatus.Present
                                        newBP.MELevel = 0
                                        newBP.PELevel = 0
                                        newBP.Notes = ""
                                        assets.Add(assetID, newBP)
                                    End If
                                End If

                                ' Get the location name
                                If loc.ChildNodes.Count > 0 Then
                                    Call GetAssetFromNode(loc, categories, groups, types, assets, locationID, locationDetails, pOwner)
                                End If
                            Next
                        End If
                        If assets.Count > 0 Then
                            ' Mark all of our existing blueprints as missing
                            For Each ownerBP As BlueprintAsset In ownerBPs.Values
                                If ownerBP.BPType <> BPType.User Then
                                    ownerBP.Status = BPStatus.Missing
                                Else
                                    ownerBP.Status = BPStatus.Present
                                End If
                            Next
                            ' Should have our list of assets now so let's compare them
                            For Each assetID As Long In assets.Keys
                                ' See if the assetID already exists for the owner
                                If ownerBPs.ContainsKey(assetID) = True Then
                                    ' We have it so set the status to present
                                    ownerBPs(assetID).Status = BPStatus.Present
                                    ' Update the location
                                    ownerBPs(assetID).LocationID = assets(assetID).LocationID
                                    ownerBPs(assetID).LocationDetails = assets(assetID).LocationDetails
                                    ' Update the type
                                    ownerBPs(assetID).BPType = assets(assetID).BPType
                                    ' Update the runs if we have found the asset is a BPC and the runs are still -1
                                    If ownerBPs(assetID).BPType = BPType.Copy And ownerBPs(assetID).Runs = -1 Then
                                        ownerBPs(assetID).Runs = 0
                                    End If
                                Else
                                    ' Not present in the existing list so let's add it in
                                    ownerBPs.Add(assetID, assets(assetID))
                                End If
                            Next
                        End If
                    End If

                    ' Update the owner list if the option requires it
                    If chkShowOwnedBPs.Checked = True Then
                        Call UpdateOwnerBPList()
                    End If

                End If
            Else
                MessageBox.Show("Make sure you have entered your API details and selected the correct owner before proceeding.", "Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End Sub
        Private Sub GetAssetFromNode(ByVal loc As XmlNode, ByVal categories As ArrayList, ByVal groups As ArrayList, ByVal types As ArrayList, ByRef assets As SortedList(Of Long, BlueprintAsset), ByVal locationID As String, ByVal locationDetails As String, ByVal prismOwner As PrismOwner)
            Dim itemList As XmlNodeList = loc.ChildNodes(0).ChildNodes
            Dim itemData As EveType
            Dim assetID As Long
            Dim itemID As Integer
            Dim flagID As Integer
            Dim flagName As String = ""
            Dim containerID As Long = CLng(loc.Attributes.GetNamedItem("itemID").Value)
            Dim containerType As Integer = CInt(loc.Attributes.GetNamedItem("typeID").Value)
            For Each item As XmlNode In itemList
                assetID = CLng(item.Attributes.GetNamedItem("itemID").Value)
                itemID = CInt(item.Attributes.GetNamedItem("typeID").Value)
                flagID = CInt(item.Attributes.GetNamedItem("flag").Value)
                Dim bpcFlag As Boolean = False
                If StaticData.Types.ContainsKey(itemID) Then
                    itemData = StaticData.Types(itemID)
                    ' Check for BPO/BPC
                    If itemData.Category = 9 Then
                        If item.Attributes.GetNamedItem("singleton").Value = "1" Then
                            If item.Attributes.GetNamedItem("rawQuantity") IsNot Nothing Then
                                If item.Attributes.GetNamedItem("rawQuantity").Value = "-2" Then
                                    bpcFlag = True
                                End If
                            End If
                        End If
                    End If
                    If PlugInData.AssetItemNames.ContainsKey(containerID) = True Then
                        flagName = locationDetails & "/" & PlugInData.AssetItemNames(containerID)
                    Else
                        flagName = locationDetails & "/" & StaticData.Types(containerType).Name
                    End If
                    If categories.Contains(itemData.Category) Or groups.Contains(itemData.Group) Or types.Contains(itemData.Id) Then
                        Dim newBP As New BlueprintAsset
                        newBP.AssetID = CStr(assetID)
                        newBP.LocationID = locationID
                        If prismOwner.IsCorp = True And StaticData.Types(itemID).Group <> 16 Then
                            Dim accountID As Integer = flagID + 885
                            If accountID = 889 Then accountID = 1000
                            If _divisions.ContainsKey(prismOwner.ID & "_" & accountID.ToString) = True Then
                                flagName = locationDetails & "/" & CStr(_divisions.Item(prismOwner.ID & "_" & accountID.ToString))
                            End If
                        End If
                        If newBP.BPType = BPType.Unknown Then
                            If bpcFlag = True Then
                                newBP.BPType = BPType.Copy
                                newBP.Runs = 1
                            Else
                                newBP.BPType = BPType.Original
                                newBP.Runs = -1
                            End If
                        End If
                        newBP.LocationDetails = flagName
                        newBP.TypeID = CStr(itemID)
                        newBP.Status = BPStatus.Present
                        newBP.MELevel = 0
                        newBP.PELevel = 0
                        newBP.Notes = ""
                        assets.Add(assetID, newBP)
                    End If
                End If
                ' Check child items if they exist
                If item.ChildNodes.Count > 0 Then
                    Call GetAssetFromNode(item, categories, groups, types, assets, locationID, flagName, prismOwner)
                End If
            Next
        End Sub

        Private Sub btnGetBPJobInfo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetBPJobInfo.Click
            ' Get the owner BPs
            Dim ownerBPs As New SortedList(Of Long, BlueprintAsset)
            If cboBPOwner.SelectedItem IsNot Nothing Then
                Dim pOwner As String = cboBPOwner.SelectedItem.ToString()
                ' Fetch the ownerBPs if it exists
                If PlugInData.BlueprintAssets.ContainsKey(pOwner) = True Then
                    ownerBPs = PlugInData.BlueprintAssets(pOwner)
                End If
            Else
                MessageBox.Show("Make sure you have entered your API details and selected the correct owner before proceeding.", "Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' We are going to scan the whole of the Jobs API to try and find relevant IDs - no sense dicking around here, we need info!!
            If ownerBPs IsNot Nothing Then
                Dim cacheFolder As String = HQ.CacheFolder
                For Each cacheFile As String In My.Computer.FileSystem.GetFiles(cacheFolder, SearchOption.SearchTopLevelOnly, "EVEHQAPI_Industry*")
                    ' Load up the XML
                    Dim jobXML As New XmlDocument
                    jobXML.Load(cacheFile)
                    ' Get the Node List
                    Dim jobs As XmlNodeList = jobXML.SelectNodes("/eveapi/result/rowset/row")
                    For Each job As XmlNode In jobs
                        Dim assetID As Integer = CInt(job.Attributes.GetNamedItem("installedItemID").Value)
                        If ownerBPs.ContainsKey(assetID) = True Then
                            ' Fetch the current BP Data
                            Dim cBPInfo As BlueprintAsset = ownerBPs(assetID)
                            Select Case CInt(job.Attributes.GetNamedItem("activityID").Value)
                                Case BlueprintActivity.Manufacturing
                                    Dim runs As Integer = CInt(job.Attributes.GetNamedItem("runs").Value)
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
                                        cBPInfo.BPType = BPType.Copy
                                        If initialRuns - runs < cBPInfo.Runs Or cBPInfo.Runs = -1 Then
                                            cBPInfo.Runs = initialRuns - runs
                                        End If
                                        If cBPInfo.Runs = 0 Then
                                            cBPInfo.Status = BPStatus.Exhausted
                                        End If
                                    Else
                                        cBPInfo.BPType = BPType.Original
                                    End If
                                Case BlueprintActivity.ResearchProductionLevel
                                    Dim runs As Integer = CInt(job.Attributes.GetNamedItem("runs").Value)
                                    ' Check if the MELevel is greater than what we have
                                    If CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value) > cBPInfo.MELevel Then
                                        cBPInfo.MELevel = CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value)
                                    End If
                                    ' Check if the PELevel is greater than what we have
                                    If CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value) + runs > cBPInfo.PELevel Then
                                        cBPInfo.PELevel = CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value) + runs
                                    End If
                                    ' Check if the Runs remaining are less than what we have
                                    Dim initialRuns As Integer = CInt(job.Attributes.GetNamedItem("installedItemLicensedProductionRunsRemaining").Value)
                                    If initialRuns <> -1 Then
                                        cBPInfo.BPType = BPType.Copy
                                        If initialRuns < cBPInfo.Runs Or cBPInfo.Runs = -1 Then
                                            cBPInfo.Runs = initialRuns
                                        End If
                                        If cBPInfo.Runs = 0 Then
                                            cBPInfo.Status = BPStatus.Exhausted
                                        End If
                                    Else
                                        cBPInfo.BPType = BPType.Original
                                    End If
                                Case BlueprintActivity.ResearchMaterialLevel
                                    Dim runs As Integer = CInt(job.Attributes.GetNamedItem("runs").Value)
                                    ' Check if the MELevel is greater than what we have
                                    If CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value) + runs > cBPInfo.MELevel Then
                                        cBPInfo.MELevel = CInt(job.Attributes.GetNamedItem("installedItemMaterialLevel").Value) + runs
                                    End If
                                    ' Check if the PELevel is greater than what we have
                                    If CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value) > cBPInfo.PELevel Then
                                        cBPInfo.PELevel = CInt(job.Attributes.GetNamedItem("installedItemProductivityLevel").Value)
                                    End If
                                    ' Check if the Runs remaining are less than what we have
                                    Dim initialRuns As Integer = CInt(job.Attributes.GetNamedItem("installedItemLicensedProductionRunsRemaining").Value)
                                    If initialRuns <> -1 Then
                                        cBPInfo.BPType = BPType.Copy
                                        If initialRuns < cBPInfo.Runs Or cBPInfo.Runs = -1 Then
                                            cBPInfo.Runs = initialRuns
                                        End If
                                        If cBPInfo.Runs = 0 Then
                                            cBPInfo.Status = BPStatus.Exhausted
                                        End If
                                    Else
                                        cBPInfo.BPType = BPType.Original
                                    End If
                                Case BlueprintActivity.Copying
                                    'Dim runs As Integer = CInt(job.Attributes.GetNamedItem("runs").Value)
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
                                        cBPInfo.BPType = BPType.Copy
                                        If initialRuns < cBPInfo.Runs Or cBPInfo.Runs = -1 Then
                                            cBPInfo.Runs = initialRuns
                                        End If
                                        If cBPInfo.Runs = 0 Then
                                            cBPInfo.Status = BPStatus.Exhausted
                                        End If
                                    Else
                                        cBPInfo.BPType = BPType.Original
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
                Call UpdateOwnerBPList()
            End If
        End Sub

        Private Sub btnGetBPClipboardInfo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetBPClipboardInfo.Click
            ' Get BPs of selected owner
            Dim pOwner As String
            If cboBPOwner.SelectedItem IsNot Nothing Then
                pOwner = cboBPOwner.SelectedItem.ToString()
            Else
                MessageBox.Show("There is no blueprint owner selected.", "Empty Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            Dim ownerBPAssets As SortedList(Of Long, BlueprintAsset)
            If PlugInData.BlueprintAssets.ContainsKey(pOwner) Then
                ownerBPAssets = PlugInData.BlueprintAssets(pOwner)
            Else
                MessageBox.Show("There were no blueprint assets found for the selected owner.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Get data from clipboard and split it into separate blueprints
            Dim clipboardBPs() As String = Clipboard.GetText().Split(vbCrLf.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            If clipboardBPs.Length = 0 Then
                MessageBox.Show("There was no blueprint data found in the clipboard.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Check each imported blueprint for valid data
            Dim importedBpoAssets As List(Of BlueprintAsset) = New List(Of BlueprintAsset)()
            Dim importedBpcAssets As List(Of BlueprintAsset) = New List(Of BlueprintAsset)()
            Dim currentBPInfo() As String
            Dim tempMELevel As Integer
            Dim tempPELevel As Integer
            Dim tempRuns As Integer
            For Each clipboardBP As String In clipboardBPs
                currentBPInfo = clipboardBP.Split(vbTab.ToCharArray())
                If currentBPInfo.Length <> 8 Then
                    MessageBox.Show("There was a data row with invalid length.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                Dim currentBP As BlueprintAsset = New BlueprintAsset()

                If StaticData.TypeNames.ContainsKey(currentBPInfo(0)) = True Then
                    currentBP.TypeID = CStr(StaticData.TypeNames(currentBPInfo(0)))
                Else
                    MessageBox.Show("There was a data row with invalid 'Name' data.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                Select Case currentBPInfo(4)
                    Case "Yes"
                        currentBP.BPType = BPType.Copy
                    Case "No"
                        currentBP.BPType = BPType.Original
                    Case Else
                        MessageBox.Show("There was a data row with invalid 'Copy' data.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                End Select
                If Integer.TryParse(currentBPInfo(5).Replace(",", ""), tempMELevel) = True Then
                    currentBP.MELevel = tempMELevel
                Else
                    MessageBox.Show("There was a data row with invalid 'ME' data.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                If Integer.TryParse(currentBPInfo(6).Replace(",", ""), tempPELevel) = True Then
                    currentBP.PELevel = tempPELevel
                Else
                    MessageBox.Show("There was a data row with invalid 'PE' data.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                If Integer.TryParse(currentBPInfo(7).Replace(",", ""), tempRuns) = True Then
                    currentBP.Runs = tempRuns
                ElseIf currentBP.BPType <> BPType.Original Then
                    MessageBox.Show("There was a data row with invalid 'Runs' data.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                ' Store valid imported BPs separately by type
                If currentBP.BPType = BPType.Original Then
                    importedBpoAssets.Add(currentBP)
                ElseIf currentBP.BPType = BPType.Copy Then
                    importedBpcAssets.Add(currentBP)
                End If
            Next

            ' Get location of imported BPs from user
            Using selectLoc As New FrmSelectLocation
                selectLoc.BPLocations = _bpLocations
                selectLoc.ShowDialog()
                Dim bpLoc As String = selectLoc.BPLocation
                If bpLoc Is Nothing Then
                    Exit Sub
                End If
                Dim includeBPOs As Boolean = selectLoc.IncludeBPOs

                ' Match owned BPs at selected location to imported BPs and update their data
                Dim unknownBPs As List(Of BlueprintAsset) = New List(Of BlueprintAsset)()
                For Each ownedBP As BlueprintAsset In ownerBPAssets.Values
                    If Locations.GetLocationNameFromID(CInt(ownedBP.LocationID)) = bpLoc Then
                        Select Case ownedBP.BPType
                            Case BPType.Original
                                If includeBPOs = True Then
                                    For Each impBP As BlueprintAsset In importedBpoAssets
                                        If ownedBP.TypeID = impBP.TypeID Then
                                            ownedBP.MELevel = impBP.MELevel
                                            ownedBP.PELevel = impBP.PELevel
                                            importedBpoAssets.Remove(impBP)
                                            Exit For
                                        End If
                                    Next
                                End If
                            Case BPType.Copy
                                For Each impBP As BlueprintAsset In importedBpcAssets
                                    If ownedBP.TypeID = impBP.TypeID Then
                                        ownedBP.MELevel = impBP.MELevel
                                        ownedBP.PELevel = impBP.PELevel
                                        ownedBP.Runs = impBP.Runs
                                        importedBpcAssets.Remove(impBP)
                                        Exit For
                                    End If
                                Next
                            Case BPType.Unknown
                                unknownBPs.Add(ownedBP)
                        End Select
                    End If
                Next

                ' Match and update remaining BPs of unknown BP type
                For Each ownedBP As BlueprintAsset In unknownBPs
                    ' Check imported BPCs
                    For Each impBP As BlueprintAsset In importedBpcAssets
                        If ownedBP.TypeID = impBP.TypeID Then
                            ownedBP.BPType = impBP.BPType
                            ownedBP.MELevel = impBP.MELevel
                            ownedBP.PELevel = impBP.PELevel
                            ownedBP.Runs = impBP.Runs
                            importedBpcAssets.Remove(impBP)
                            Exit For
                        End If
                    Next
                    ' Check imported BPOs if no match was found in BPCs
                    If ownedBP.BPType = BPType.Unknown And includeBPOs = True Then
                        For Each impBP As BlueprintAsset In importedBpoAssets
                            If ownedBP.TypeID = impBP.TypeID Then
                                ownedBP.BPType = impBP.BPType
                                ownedBP.MELevel = impBP.MELevel
                                ownedBP.PELevel = impBP.PELevel
                                importedBpoAssets.Remove(impBP)
                                Exit For
                            End If
                        Next
                    End If
                Next
            End Using

            ' Update the owner list if the option requires it
            If chkShowOwnedBPs.Checked = True Then
                Call UpdateOwnerBPList()
            End If
        End Sub

        Private Sub ctxBPManager_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxBPManager.Opening
            If adtBlueprints.SelectedNodes.Count = 1 Then
                mnuSendToBPCalc.Enabled = True
                ' Get the blueprint info
                If chkShowOwnedBPs.Checked = True Then
                    Dim assetID As Long = CLng(adtBlueprints.SelectedNodes(0).Tag)
                    Dim bpOwner As String = cboBPOwner.SelectedItem.ToString
                    Dim asset As BlueprintAsset = PlugInData.BlueprintAssets(bpOwner).Item(assetID)
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

        Private Sub mnuSendToBPCalc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSendToBPCalc.Click
            If adtBlueprints.SelectedNodes.Count = 1 Then
                Dim bpName As String = adtBlueprints.SelectedNodes(0).Text
                If chkShowOwnedBPs.Checked = True Then
                    ' Start an owned BPCalc
                    If adtBlueprints.SelectedNodes(0).Tag IsNot Nothing Then
                        Dim bpid As Long = CLng(adtBlueprints.SelectedNodes(0).Tag)
                        Dim bpCalc As New frmBPCalculator(cboBPOwner.SelectedItem.ToString, bpid)
                        Call OpenBPCalculator(bpCalc)
                    End If
                Else
                    ' Start a standard BPCalc
                    Dim bpCalc As New frmBPCalculator(bpName)
                    Call OpenBPCalculator(bpCalc)
                End If
            ElseIf adtBlueprints.SelectedNodes.Count = 0 Then
                ' Start a blank BP Calc
                Dim bpCalc As New frmBPCalculator(chkShowOwnedBPs.Checked)
                Call OpenBPCalculator(bpCalc)
            End If
        End Sub

        Private Sub OpenBPCalculator(ByVal bpCalc As frmBPCalculator)
            bpCalc.Location = New Point(CInt(ParentForm.Left + ((ParentForm.Width - bpCalc.Width) / 2)), CInt(ParentForm.Top + ((ParentForm.Height - bpCalc.Height) / 2)))
            bpCalc.Show()
        End Sub

        Private Sub mnuAmendBPDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAmendBPDetails.Click
            Call EditBlueprintDetails()
        End Sub

        Private Sub EditBlueprintDetails()
            Using bpForm As New FrmEditBPDetails
                bpForm.OwnerName = cboBPOwner.SelectedItem.ToString
                Dim bps As New List(Of Long)
                For Each selItem As Node In adtBlueprints.SelectedNodes
                    bps.Add(CLng(selItem.Tag))
                Next
                If bps.Count > 0 Then
                    bpForm.AssetIDs = bps
                    bpForm.ShowDialog()
                    ' Update the list using the details
                    Dim bpAsset As BlueprintAsset
                    Dim locationName As String
                    For Each selitem As Node In adtBlueprints.SelectedNodes
                        bpAsset = PlugInData.BlueprintAssets(bpForm.OwnerName).Item(CLng(selitem.Tag))
                        locationName = Locations.GetLocationNameFromID(CInt(bpAsset.LocationID))
                        Call UpdateOwnerBPItem(bpForm.OwnerName, locationName, bpAsset, selitem)
                    Next
                Else
                    Dim msg As New StringBuilder
                    msg.AppendLine("An attempt to start the BP Editor was made but it appears as if there is nothing to edit! Please take a screenshot of this message together with the Blueprint Manager list and submit it to the developers for investigation.")
                    msg.AppendLine("")
                    msg.AppendLine("ArrayList Count: " & bps.Count.ToString)
                    msg.AppendLine("Selected Node Count: " & adtBlueprints.SelectedNodes.Count.ToString)
                    MessageBox.Show(msg.ToString, "No Blueprints Selected??", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        End Sub

        Private Sub txtBPSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBPSearch.TextChanged
            Call UpdateBPList()
        End Sub

        Private Sub btnResetBPSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetBPSearch.Click
            txtBPSearch.Text = ""
        End Sub

        Private Sub btnAddCustomBP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddCustomBP.Click
            Using bpForm As New FrmAddCustomBP
                If cboBPOwner.SelectedItem IsNot Nothing Then
                    bpForm.BPOwner = cboBPOwner.SelectedItem.ToString
                    bpForm.ShowDialog()
                    If bpForm.DialogResult = DialogResult.OK Then
                        Call UpdateBPList()
                    End If
                Else
                    MessageBox.Show("Please select an BP Owner before adding a custom blueprint.", "BP Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        End Sub

        Private Sub mnuRemoveCustomBP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRemoveCustomBP.Click
            ' Remove the custom BP from the assets
            If adtBlueprints.SelectedNodes.Count > 0 Then
                ' Establish list for removal
                Dim removalList As New List(Of Node)
                For Each rn As Node In adtBlueprints.SelectedNodes
                    removalList.Add(rn)
                Next
                ' Remove the nodes
                Dim bpOwner As String = cboBPOwner.SelectedItem.ToString
                For Each rn As Node In removalList
                    Dim assetId As Long = CLng(rn.Tag)
                    If PlugInData.BlueprintAssets(bpOwner).ContainsKey(assetId) = True Then
                        PlugInData.BlueprintAssets(bpOwner).Remove(assetId)
                        adtBlueprints.Nodes.Remove(rn)
                    End If
                Next
                removalList.Clear()
            End If
        End Sub

        Private Sub cboTechFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTechFilter.SelectedIndexChanged
            If _startup = False And _bpManagerUpdate = False Then
                Call UpdateBPList()
            End If
        End Sub

        Private Sub cboTypeFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTypeFilter.SelectedIndexChanged
            If _startup = False And _bpManagerUpdate = False Then
                Call UpdateBPList()
            End If
        End Sub

        Private Sub cboCategoryFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboCategoryFilter.SelectedIndexChanged
            If _startup = False And _bpManagerUpdate = False Then
                Call UpdateBPList()
            End If
        End Sub

        Private Sub adtBlueprints_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtBlueprints.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

        Private Sub adtBlueprints_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtBlueprints.NodeDoubleClick
            If adtBlueprints.SelectedNodes.Count = 1 Then
                Dim bpName As String = adtBlueprints.SelectedNodes(0).Text
                If chkShowOwnedBPs.Checked = True Then
                    ' Start an owned BPCalc
                    If adtBlueprints.SelectedNodes(0).Tag IsNot Nothing Then
                        Dim bpid As Long = CLng(adtBlueprints.SelectedNodes(0).Tag)
                        Dim bpCalc As New frmBPCalculator(cboBPOwner.SelectedItem.ToString, bpid)
                        Call OpenBPCalculator(bpCalc)
                    End If
                Else
                    ' Start a standard BPCalc
                    Dim bpCalc As New frmBPCalculator(bpName)
                    Call OpenBPCalculator(bpCalc)
                End If
            ElseIf adtBlueprints.SelectedNodes.Count = 0 Then
                ' Start a blank BP Calc
                Dim bpCalc As New frmBPCalculator(chkShowOwnedBPs.Checked)
                Call OpenBPCalculator(bpCalc)
            End If
        End Sub

        Private Sub btnCopyListToClipboard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopyListToClipboard.Click
            ' Exports the list to Clipboard in TSV format for pasting to Excel etc
            If cboBPOwner.SelectedItem IsNot Nothing Then
                Call ExportToClipboard("Blueprint List for " & cboBPOwner.SelectedItem.ToString, adtBlueprints, ControlChars.Tab)
            Else
                MessageBox.Show("A BP Owner is required before copying the data to the clipboard", "BP Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

#End Region

#Region "Transaction List Menu Options"

        Private Sub ctxTransactions_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxTransactions.Opening
            If adtTransactions.SelectedNodes.Count > 0 Then
                Dim transItem As Node = adtTransactions.SelectedNodes(0)
                Dim itemName As String = transItem.Cells(1).Text
                mnuTransactionModifyPrice.Text = "Modify Custom Price of " & itemName
                If StaticData.TypeNames.ContainsKey(itemName) Then
                    mnuTransactionModifyPrice.Tag = StaticData.TypeNames(itemName)
                Else
                    MessageBox.Show("There was a mismatch of expected data. '" & itemName & "' was not found in the collection of items.", "Data Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End Sub

        Private Sub mnuTransactionModifyPrice_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuTransactionModifyPrice.Click
            If mnuTransactionModifyPrice.Tag IsNot Nothing Then
                Dim itemID As Integer = CInt(mnuTransactionModifyPrice.Tag)
                Dim price As Double = Double.Parse(adtTransactions.SelectedNodes(0).Cells(3).Text, NumberStyles.Any, _culture)
                Using newPrice As New FrmModifyPrice(itemID, price)
                    newPrice.ShowDialog()
                End Using
            End If
        End Sub

#End Region

#Region "Ribbon and Tab UI Functions"

        Private Sub tabPrism_SelectedTabChanging(ByVal sender As Object, ByVal e As TabStripTabChangingEventArgs) Handles tabPrism.SelectedTabChanging
            _selectedTab = e.NewTab
        End Sub

        Private Sub tabPrism_TabItemClose(ByVal sender As Object, ByVal e As TabStripActionEventArgs) Handles tabPrism.TabItemClose
            e.Cancel = True
            If _selectedTab IsNot Nothing Then
                If _selectedTab.Name <> "tiPrismHome" Then
                    _selectedTab.Visible = False
                End If
            End If
        End Sub

        Private Sub btnOptions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOptions.Click
            Using newSettings As New FrmPrismSettings
                newSettings.ShowDialog()
            End Using
        End Sub

        Private Sub btnDownloadAPIData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDownloadAPIData.Click

            ' Set the label and disable the button
            lblCurrentAPI.Text = "Downloading API Data..."
            btnDownloadAPIData.Enabled = False

            ' Flick to the API Status tab
            tabPrism.SelectedTab = tiPrismHome
            ' Delete the current API Status data
            For Each pOwner As ListViewItem In lvwCurrentAPIs.Items
                pOwner.ToolTipText = ""
                'Dim OwnerName As String = pOwner.Text
                For si As Integer = 0 To 7
                    'If Owner.SubItems(1).Text = "Corporation" = True Then
                    '    If PrismSettings.UserSettings.CorpReps.ContainsKey(OwnerName) = True Then
                    '        If PrismSettings.UserSettings.CorpReps(OwnerName).ContainsKey(CType(si, CorpRepType)) = True Then
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
                    pOwner.SubItems(si + 2).Text = ""
                Next
            Next

            ' Get XMLs
            Call StartGetXMLDataThread()

        End Sub

        Private Sub btnWalletJournal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnWalletJournal.Click
            tabPrism.SelectedTab = tiJournal
            tiJournal.Visible = True
        End Sub

        Private Sub btnWalletTransactions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnWalletTransactions.Click
            tabPrism.SelectedTab = tiTransactions
            tiTransactions.Visible = True
        End Sub

        Private Sub btnAssets_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAssets.Click
            tabPrism.SelectedTab = tiAssets
            tiAssets.Visible = True
        End Sub

        Private Sub btnBPManager_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBPManager.Click
            tabPrism.SelectedTab = tiBPManager
            tiBPManager.Visible = True
        End Sub

        Private Sub btnRecycler_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRecycler.Click
            tabPrism.SelectedTab = tiRecycler
            tiRecycler.Visible = True
        End Sub

        Private Sub btnOrders_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOrders.Click
            tabPrism.SelectedTab = tiMarketOrders
            tiMarketOrders.Visible = True
        End Sub

        Private Sub btnJobs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnJobs.Click
            tiJobs.Visible = True
            tabPrism.SelectedTab = tiJobs
        End Sub

        Private Sub btnContracts_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnContracts.Click
            tiContracts.Visible = True
            tabPrism.SelectedTab = tiContracts
        End Sub

        Private Sub btnReports_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReports.Click
            tabPrism.SelectedTab = tiReports
            tiReports.Visible = True
        End Sub

        Private Sub btnInventionChance_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInventionChance.Click
            Using invCalc As New FrmQuickInventionChance
                invCalc.ShowDialog()
            End Using
        End Sub

        Private Sub btnBlueprintCalc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBlueprintCalc.Click
            ' Start a blank BP Calc
            Dim bpCalc As New frmBPCalculator(chkShowOwnedBPs.Checked)
            Call OpenBPCalculator(bpCalc)
        End Sub

        Private Sub btnProductionManager_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProductionManager.Click
            tabPrism.SelectedTab = tiProductionManager
            tiProductionManager.Visible = True
        End Sub

        Private Sub btnInventionManager_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInventionManager.Click
            tabPrism.SelectedTab = tiInventionManager
            tiInventionManager.Visible = True
        End Sub

        Private Sub btnQuickProduction_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnQuickProduction.Click
            Using qp As New FrmQuickProduction
                qp.ShowDialog()
            End Using
        End Sub

        Private Sub btnRigBuilder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRigBuilder.Click
            tabPrism.SelectedTab = tiRigBuilder
            tiRigBuilder.Visible = True
        End Sub

        Private Sub btnInventionResults_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInventionResults.Click
            tabPrism.SelectedTab = tiInventionResults
            tiInventionResults.Visible = True
        End Sub

#End Region

#Region "Search and Search UI Functions"

        Private Sub txtItemSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtItemSearch.TextChanged
            If Len(txtItemSearch.Text) > 2 Then
                Dim strSearch As String = txtItemSearch.Text.Trim.ToLower
                adtSearch.BeginUpdate()
                adtSearch.Nodes.Clear()
                ' Check items
                For Each item As String In StaticData.TypeNames.Keys
                    If item.ToLower.Contains(strSearch) Then
                        Dim newNode As New Node(item)
                        newNode.Name = item
                        newNode.TagString = "Item"
                        adtSearch.Nodes.Add(newNode)
                    End If
                Next
                ' Check Batch Jobs
                For Each bJob As BatchJob In BatchJobs.Jobs.Values
                    ' Check the Job Name
                    If bJob.BatchName.ToLower.Contains(strSearch) Then
                        Dim newNode As New Node(bJob.BatchName & " [Batch Job]")
                        newNode.Name = bJob.BatchName
                        newNode.TagString = "Batch"
                        adtSearch.Nodes.Add(newNode)
                    End If
                Next
                ' Check Production Jobs
                For Each pJob As Job In Jobs.JobList.Values
                    ' Check the Job Name
                    If pJob.JobName.ToLower.Contains(strSearch) Then
                        Dim newNode As New Node(pJob.JobName & " [Production Job]")
                        newNode.Name = pJob.JobName
                        newNode.TagString = "Production"
                        adtSearch.Nodes.Add(newNode)
                    End If
                    ' Check the Job Type
                    If pJob.TypeName.ToLower.Contains(strSearch) Then
                        Dim newNode As New Node(pJob.TypeName & " [in Production Job '" & pJob.JobName & "']")
                        newNode.Name = pJob.TypeName
                        newNode.TagString = "Item"
                        adtSearch.Nodes.Add(newNode)
                    End If
                Next
                adtSearch.EndUpdate()
            End If
        End Sub

        Private Sub btnLinkBPCalc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLinkBPCalc.Click
            Dim keyName As String = adtSearch.SelectedNodes(0).Name
            Select Case adtSearch.SelectedNodes(0).TagString
                Case "Item"
                    Dim bpName As String = lblSelectedBP.Tag.ToString
                    ' Start a standard BP Calc
                    Dim bpCalc As New frmBPCalculator(bpName)
                    Call OpenBPCalculator(bpCalc)
                Case "Production"
                    If Jobs.JobList.ContainsKey(keyName) Then
                        Dim pJob As Job = Jobs.JobList(keyName)
                        Dim bpCalc As New frmBPCalculator(pJob, False)
                        Call OpenBPCalculator(bpCalc)
                    End If
            End Select
        End Sub

        Private Sub btnLinkRequisition_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLinkRequisition.Click
            Dim keyName As String = adtSearch.SelectedNodes(0).Name
            Select Case adtSearch.SelectedNodes(0).TagString
                Case "Item"
                    ' Set up a new Sortedlist to store the required items
                    Dim orders As New SortedList(Of String, Integer)
                    ' Add the current item
                    orders.Add(keyName, 1)
                    ' Setup the Requisition form for Prism and open it
                    Using newReq As New FrmAddRequisition("Prism", orders)
                        newReq.ShowDialog()
                    End Using
                Case "Production"
                    ' Set up a new Sortedlist to store the required items
                    Dim orders As New SortedList(Of String, Integer)
                    If Jobs.JobList.ContainsKey(keyName) Then
                        Dim pJob As Job = Jobs.JobList(keyName)
                        Call CreateRequisitionFromJob(orders, pJob)
                    End If
                    ' Setup the Requisition form for Prism and open it
                    Using newReq As New FrmAddRequisition("Prism", orders)
                        newReq.ShowDialog()
                    End Using
                Case "Batch"
                    ' Set up a new Sortedlist to store the required items
                    Dim orders As New SortedList(Of String, Integer)
                    If BatchJobs.Jobs.ContainsKey(keyName) Then
                        For Each pJobName As String In BatchJobs.Jobs(keyName).ProductionJobs
                            If Jobs.JobList.ContainsKey(pJobName) Then
                                Dim pJob As Job = Jobs.JobList(pJobName)
                                Call CreateRequisitionFromJob(orders, pJob)
                            End If
                        Next
                    End If
                    ' Setup the Requisition form for Prism and open it
                    Using newReq As New FrmAddRequisition("Prism", orders)
                        newReq.ShowDialog()
                    End Using
            End Select
        End Sub

        Private Sub btnLinkProduction_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLinkProduction.Click
            Using qp As New FrmQuickProduction(lblSelectedBP.Tag.ToString)
                qp.ShowDialog()
            End Using
        End Sub

        Private Sub CreateRequisitionFromJob(ByVal orders As SortedList(Of String, Integer), ByVal currentJob As Job)
            If currentJob IsNot Nothing Then
                Dim priceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From r In currentJob.Resources.Values Where TypeOf (r) Is JobResource Select r.TypeID)
                priceTask.Wait()
                For Each resource As JobResource In currentJob.Resources.Values
                    ' This is a resource so add it
                    If resource.TypeCategory <> 16 Then
                        Dim perfectRaw As Integer = CInt(resource.PerfectUnits)
                        Dim waste As Integer = CInt(resource.WasteUnits)
                        Dim total As Integer = perfectRaw + waste
                        If total > 0 Then
                            Dim totalTotal As Long = CLng(total) * CLng(currentJob.Runs)
                            If orders.ContainsKey(resource.TypeName) = False Then
                                orders.Add(resource.TypeName, CInt(totalTotal))
                            Else
                                orders(resource.TypeName) += CInt(totalTotal)
                            End If
                        End If
                    End If
                Next
                For Each subJob As Job In currentJob.SubJobs.Values
                    Call CreateRequisitionFromJob(orders, subJob)
                Next
            End If
        End Sub

        Private Sub adtSearch_NodeClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtSearch.NodeClick

            Select Case adtSearch.SelectedNodes(0).TagString
                Case "Item"
                    ' Get the name and ID
                    Dim itemName As String = adtSearch.SelectedNodes(0).Name
                    If StaticData.TypeNames.ContainsKey(itemName) = True Then
                        Dim itemID As Integer = StaticData.TypeNames(itemName)

                        ' See if we have a blueprint
                        Dim bpName As String = ""
                        Dim bpid As Integer
                        If itemName.EndsWith("Blueprint", StringComparison.Ordinal) = False Then
                            If StaticData.TypeNames.ContainsKey(itemName.Trim & " Blueprint") = True Then
                                bpName = itemName.Trim & " Blueprint"
                                bpid = StaticData.TypeNames(bpName)
                            End If
                        Else
                            bpid = itemID
                            bpName = itemName
                            itemID = StaticData.Blueprints(CInt(bpid)).ProductId
                            itemName = StaticData.Types(itemID).Name
                        End If

                        lblSelectedItem.Text = "Item: " & itemName
                        lblSelectedItem.Tag = itemName
                        If bpName <> "" Then
                            lblSelectedBP.Text = "Blueprint: " & bpName
                            lblSelectedBP.Tag = bpName
                        Else
                            lblSelectedBP.Text = "Blueprint: <none available>"
                        End If

                        ' Check we can activate buttons
                        If bpid <> 0 Then
                            btnLinkBPCalc.Enabled = True
                            btnLinkProduction.Enabled = True
                        Else
                            btnLinkBPCalc.Enabled = False
                            btnLinkProduction.Enabled = False
                        End If
                        btnLinkRequisition.Enabled = True
                    End If
                Case "Production"
                    Dim jobName As String = adtSearch.SelectedNodes(0).Name
                    If Jobs.JobList.ContainsKey(jobName) = True Then
                        lblSelectedItem.Text = "Job: " & jobName
                        lblSelectedBP.Text = "Blueprint: <per job>"
                        btnLinkBPCalc.Enabled = True
                        btnLinkProduction.Enabled = False
                        btnLinkRequisition.Enabled = True
                    End If
                Case "Batch"
                    Dim batchName As String = adtSearch.SelectedNodes(0).Name
                    If BatchJobs.Jobs.ContainsKey(batchName) = True Then
                        lblSelectedItem.Text = "Batch: " & batchName
                        lblSelectedBP.Text = "Blueprint: <multiple>"
                        btnLinkBPCalc.Enabled = False
                        btnLinkProduction.Enabled = False
                        btnLinkRequisition.Enabled = True
                    End If
            End Select

        End Sub

        Private Sub adtSearch_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtSearch.NodeDoubleClick
            Dim keyName As String = e.Node.Name
            Select Case e.Node.TagString
                Case "Item"
                    Dim itemName As String = keyName
                    Dim itemID As Integer
                    ' See if we have a blueprint
                    Dim bpName As String = ""
                    Dim bpid As Integer
                    If itemName.EndsWith("Blueprint", StringComparison.Ordinal) = False Then
                        If StaticData.TypeNames.ContainsKey(itemName.Trim & " Blueprint") = True Then
                            bpName = itemName.Trim & " Blueprint"
                            bpid = StaticData.TypeNames(bpName)
                        End If
                    Else
                        itemID = StaticData.Blueprints(CInt(bpid)).ProductId
                        itemName = StaticData.Types(itemID).Name
                        bpid = itemID
                        bpName = itemName
                    End If
                    If bpid <> 0 Then
                        ' Start a standard BP Calc
                        Dim bpCalc As New frmBPCalculator(bpName)
                        Call OpenBPCalculator(bpCalc)
                    End If
                Case "Production"
                    If Jobs.JobList.ContainsKey(keyName) Then
                        Dim pJob As Job = Jobs.JobList(keyName)
                        Dim bpCalc As New frmBPCalculator(pJob, False)
                        Call OpenBPCalculator(bpCalc)
                    End If
            End Select
        End Sub

#End Region

#Region "Production Manager Routines"

        Private Sub UpdateProductionJobList()
            adtProdJobs.BeginUpdate()
            adtProdJobs.Nodes.Clear()
            Dim priceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From r In Jobs.JobList.Values Where r.CurrentBlueprint IsNot Nothing Select r.CurrentBlueprint.ProductId)
            priceTask.Wait()
            Dim prices As Dictionary(Of Integer, Double) = priceTask.Result
            For Each cJob As Job In Jobs.JobList.Values
                Dim newJob As New Node
                newJob.Name = cJob.JobName
                newJob.Text = cJob.JobName
                newJob.Cells.Add(New Cell(cJob.TypeName))
                If cJob.CurrentBlueprint IsNot Nothing Then
                    Dim product As EveType = StaticData.Types(cJob.CurrentBlueprint.ProductId)
                    Dim totalcosts As Double = cJob.Cost + Math.Round((PrismSettings.UserSettings.FactoryRunningCost / 3600 * cJob.RunTime) + PrismSettings.UserSettings.FactoryInstallCost, 2, MidpointRounding.AwayFromZero)
                    Dim unitcosts As Double = Math.Round(totalcosts / (cJob.Runs * product.PortionSize), 2, MidpointRounding.AwayFromZero)
                    Dim value As Double = prices(cJob.CurrentBlueprint.ProductId)
                    Dim profit As Double = value - unitcosts
                    Dim rate As Double = profit / ((cJob.RunTime / cJob.Runs) / 3600)
                    Dim profitMargin As Double = (profit / value * 100)
                    newJob.Cells.Add(New Cell(profit.ToString("N2")))
                    newJob.Cells.Add(New Cell(rate.ToString("N2")))
                    newJob.Cells.Add(New Cell(profitMargin.ToString("N2")))
                Else
                    newJob.Cells.Add(New Cell(0.ToString("N2")))
                    newJob.Cells.Add(New Cell(0.ToString("N2")))
                    newJob.Cells.Add(New Cell(0.ToString("N2")))
                End If
                adtProdJobs.Nodes.Add(newJob)
            Next
            AdvTreeSorter.Sort(adtProdJobs, 1, True, True)
            adtProdJobs.EndUpdate()
        End Sub

        Private Sub UpdateBatchList()
            adtBatches.BeginUpdate()
            adtBatches.Nodes.Clear()
            Dim obsoleteBatches As List(Of String) = New List(Of String)()
            For Each cBatch As BatchJob In BatchJobs.Jobs.Values
                Dim newBatch As New Node
                newBatch.Name = cBatch.BatchName
                newBatch.Text = cBatch.BatchName
                Dim obsoleteJobs As List(Of String) = New List(Of String)()
                For Each jobName As String In cBatch.ProductionJobs
                    If Jobs.JobList.ContainsKey(jobName) Then
                        Dim newJob As New Node
                        newJob.Name = jobName
                        newJob.Text = jobName
                        newBatch.Nodes.Add(newJob)
                    Else
                        obsoleteJobs.Add(jobName)
                    End If
                Next
                For Each jobName As String In obsoleteJobs
                    cBatch.ProductionJobs.Remove(jobName)
                Next
                If newBatch.Nodes.Count > 0 Then
                    adtBatches.Nodes.Add(newBatch)
                Else
                    obsoleteBatches.Add(cBatch.BatchName)
                End If
            Next
            For Each batchName As String In obsoleteBatches
                BatchJobs.Jobs.Remove(batchName)
            Next
            adtBatches.EndUpdate()
        End Sub

        Private Sub adtProdJobs_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles adtProdJobs.SelectionChanged
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
                    Dim jobName As String = adtProdJobs.SelectedNodes(0).Name
                    Dim existingJob As Job = Jobs.JobList(jobName)
                    PRPM.ProductionJob = existingJob
                Case Else
                    btnDeleteJob.Text = "Delete Jobs"
                    btnDeleteJob.Enabled = True
                    btnMakeBatch.Enabled = True
                    ' Create a temporary batch job to pass to the PR control
                    Dim tempBatch As New BatchJob
                    tempBatch.BatchName = "Temporary Batch from Production Manager"
                    For Each jobNode As Node In adtProdJobs.SelectedNodes
                        tempBatch.ProductionJobs.Add(jobNode.Name)
                    Next
                    PRPM.BatchJob = tempBatch
            End Select
        End Sub

        Private Sub adtProdJobs_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtProdJobs.NodeDoubleClick
            Dim jobName As String = e.Node.Name
            Dim existingJob As Job = Jobs.JobList(jobName)
            Dim bpCalc As New frmBPCalculator(existingJob, False)
            bpCalc.Location = New Point(CInt(ParentForm.Left + ((ParentForm.Width - bpCalc.Width) / 2)), CInt(ParentForm.Top + ((ParentForm.Height - bpCalc.Height) / 2)))
            bpCalc.Show()
        End Sub

        Private Sub adtProdJobs_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtProdJobs.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub

        Private Sub btnDeleteJob_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteJob.Click
            Dim reply As DialogResult = MessageBox.Show("Are you sure you want to delete the selected jobs?", "Confirm Job Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                For Each delNode As Node In adtProdJobs.SelectedNodes
                    Jobs.JobList.Remove(delNode.Name)
                Next
                Call UpdateProductionJobList()
                Call UpdateBatchList()
            End If
        End Sub

        Private Sub btnClearAllJobs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearAllJobs.Click
            Dim reply As DialogResult = MessageBox.Show("This will remove all your jobs. Are you sure you want to delete all jobs?", "Confirm Job Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                Jobs.JobList.Clear()
                Call UpdateProductionJobList()
                Call UpdateBatchList()
            End If
        End Sub

        Private Sub btnRefreshJobs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshJobs.Click
            ' Cycle through all the jobs and update the job names
            For Each jobName As String In Jobs.JobList.Keys
                Jobs.JobList(jobName).JobName = jobName
            Next
            Call UpdateProductionJobList()
        End Sub

        Private Sub btnMakeBatch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMakeBatch.Click
            Using newBatchName As New FrmAddBatchJob
                newBatchName.ShowDialog()
                If newBatchName.DialogResult = DialogResult.OK Then
                    Dim newBatch As New BatchJob
                    newBatch.BatchName = newBatchName.JobName
                    For Each jobNode As Node In adtProdJobs.SelectedNodes
                        newBatch.ProductionJobs.Add(jobNode.Name)
                    Next
                    BatchJobs.Jobs.Add(newBatch.BatchName, newBatch)
                End If
            End Using
            PrismEvents.StartUpdateBatchJobs()
        End Sub

#End Region

#Region "Batch Manager Routines"

        Private Sub adtBatches_NodeClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtBatches.NodeClick
            If e.Node.Nodes.Count > 0 Then
                ' This is a batch name
                Dim batchName As String = e.Node.Name
                Dim existingBatch As BatchJob = BatchJobs.Jobs(batchName)
                PRPM.BatchJob = existingBatch
                PRPM.tcResources.SelectedTab = PRPM.tiBatchResources
            Else
                ' This is a job name
                Dim jobName As String = e.Node.Name
                Dim existingJob As Job = Jobs.JobList(jobName)
                PRPM.ProductionJob = existingJob
                PRPM.tcResources.SelectedTab = PRPM.tiProductionResources
            End If
        End Sub

        Private Sub adtBatches_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles adtBatches.SelectionChanged
            Select Case adtProdJobs.SelectedNodes.Count
                Case 0
                    ' Do nothing
                Case 1
                    If PRPM.BatchJob IsNot Nothing Then
                        PRPM.BatchJob = Nothing
                    End If
                Case Is > 1
                    ' Create a temporary batch job to pass to the PR control
                    Dim tempBatch As New BatchJob
                    tempBatch.BatchName = "Temporary Batch from Batch Manager"
                    For Each jobNode As Node In adtProdJobs.SelectedNodes
                        tempBatch.ProductionJobs.Add(jobNode.Name)
                    Next
                    PRPM.BatchJob = tempBatch
            End Select
        End Sub

        Private Sub btnClearBatches_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearBatches.Click
            Dim reply As DialogResult = MessageBox.Show("This will remove all your batches. Are you sure you want to delete all batches?", "Confirm Batch Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                BatchJobs.Jobs.Clear()
                Call UpdateBatchList()
            End If
        End Sub

#End Region

#Region "Invention Manager Routines"

        Private Sub UpdateInventionJobList()
            adtInventionJobs.BeginUpdate()
            adtInventionJobs.Nodes.Clear()
            Dim priceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From r In Jobs.JobList.Values Where r.HasInventionJob = True Where r.InventionJob.InventedBpid <> 0 Select r.InventionJob.CalculateInventedBPC.ProductId)
            priceTask.Wait()
            Dim prices As Dictionary(Of Integer, Double) = priceTask.Result
            For Each cJob As Job In Jobs.JobList.Values
                ' Check for the Invention Manager Flag
                If cJob.HasInventionJob = True Then
                    Dim newJob As New Node
                    newJob.Name = cJob.JobName
                    newJob.Text = cJob.JobName
                    If cJob.InventionJob.InventedBpid <> 0 Then

                        ' Calculate costs
                        Dim invCost As InventionCost = cJob.InventionJob.CalculateInventionCost
                        Dim ibp As OwnedBlueprint = cJob.InventionJob.CalculateInventedBPC
                        Dim batchQty As Integer = StaticData.Types(ibp.ProductId).PortionSize
                        Dim inventionChance As Double = cJob.InventionJob.CalculateInventionChance
                        Dim inventionAttempts As Double = Math.Max(Math.Round(100 / inventionChance, 4, MidpointRounding.AwayFromZero), 1)
                        Dim inventionSuccessCost As Double = inventionAttempts * invCost.TotalCost

                        ' Calculate Production Cost of invented item
                        Dim factoryCost As Double = Math.Round((PrismSettings.UserSettings.FactoryRunningCost / 3600 * cJob.InventionJob.ProductionJob.RunTime) + PrismSettings.UserSettings.FactoryInstallCost, 2, MidpointRounding.AwayFromZero)
                        Dim avgCost As Double = (Math.Round(inventionSuccessCost / ibp.Runs, 2, MidpointRounding.AwayFromZero) + cJob.InventionJob.ProductionJob.Cost + factoryCost) / batchQty
                        Dim salesPrice As Double = prices(ibp.ProductId)
                        Dim unitProfit As Double = salesPrice - avgCost
                        Dim profitMargin As Double = unitProfit / salesPrice * 100

                        newJob.Cells.Add(New Cell(StaticData.Types(cJob.InventionJob.InventedBpid).Name))
                        newJob.Cells.Add(New Cell(inventionChance.ToString("N2")))
                        newJob.Cells.Add(New Cell(inventionSuccessCost.ToString("N2")))
                        newJob.Cells.Add(New Cell(avgCost.ToString("N2")))
                        newJob.Cells.Add(New Cell(salesPrice.ToString("N2")))
                        newJob.Cells.Add(New Cell(unitProfit.ToString("N2")))
                        newJob.Cells.Add(New Cell(profitMargin.ToString("N2")))
                    Else
                        newJob.Cells.Add(New Cell("n/a"))
                        newJob.Cells.Add(New Cell(0.ToString("N2")))
                        newJob.Cells.Add(New Cell(0.ToString("N2")))
                        newJob.Cells.Add(New Cell(0.ToString("N2")))
                        newJob.Cells.Add(New Cell(0.ToString("N2")))
                        newJob.Cells.Add(New Cell(0.ToString("N2")))
                        newJob.Cells.Add(New Cell(0.ToString("N2")))
                    End If
                    adtInventionJobs.Nodes.Add(newJob)
                End If
            Next
            AdvTreeSorter.Sort(adtInventionJobs, 1, True, True)
            adtInventionJobs.EndUpdate()
        End Sub

        Private Sub adtInventionJobs_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtInventionJobs.NodeDoubleClick
            Dim jobName As String = e.Node.Name
            If Jobs.JobList.ContainsKey(jobName) Then
                Dim existingJob As Job = Jobs.JobList(jobName)
                Dim bpCalc As New frmBPCalculator(existingJob, True)
                bpCalc.Location = New Point(CInt(ParentForm.Left + ((ParentForm.Width - bpCalc.Width) / 2)), CInt(ParentForm.Top + ((ParentForm.Height - bpCalc.Height) / 2)))
                bpCalc.Show()
            End If
        End Sub

        Private Sub adtInventionJobs_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtInventionJobs.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
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
            cboReport.Items.Add("Journal Income Type Analysis")
            cboReport.Items.Add("Journal Expenditure Type Analysis")

            ' Finalise the report combobox update
            cboReport.EndUpdate()

            ' Set the dates
            dtiReportEndDate.Value = Now
            dtiReportStartDate.Value = Now.AddMonths(-1)

            cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, True, cboReportOwners)

            ' Update the ref types box
            cboReportJournalType.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalRefTypes, False, cboReportJournalType)

        End Sub

        Private Sub cboReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboReport.SelectedIndexChanged

            ' Set the report name
            Dim reportName As String = cboReport.SelectedItem.ToString

            Select Case reportName

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

                Case "Journal Income Type Analysis"
                    If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.JournalOwnersAll Then
                        cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, True, cboReportOwners)
                        cboReportOwners.Text = ""
                    End If

                Case "Journal Expenditure Type Analysis"
                    If CType(cboReportOwners.DropDownControl, PrismSelectionControl).ListType <> PrismSelectionType.JournalOwnersAll Then
                        cboReportOwners.DropDownControl = New PrismSelectionControl(PrismSelectionType.JournalOwnersAll, True, cboReportOwners)
                        cboReportOwners.Text = ""
                    End If

            End Select

        End Sub

        Private Sub btnGenerateReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerateReport.Click

            ' Check for selected items
            If CType(cboReportOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems.Count = 0 Then
                MessageBox.Show("You must select at least one owner before generating a report!", "Report Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Check for selected journal type
            ' Set the report name
            Dim reportName As String = cboReport.SelectedItem.ToString
            Select Case reportName
                Case "Journal Income Type Analysis", "Journal Expenditure Type Analysis"
                    If CType(cboReportJournalType.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems.Count = 0 Then
                        MessageBox.Show("You must select a journal type before generating this type of report!", "Report Journal Type Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
            End Select

            Call GenerateReport()

        End Sub

        Private Sub GenerateReport()

            If cboReport.SelectedItem IsNot Nothing Then

                ' Set the start and end date
                Dim startDate As Date = New Date(dtiReportStartDate.Value.Year, dtiReportStartDate.Value.Month, dtiReportStartDate.Value.Day, 0, 0, 0)
                Dim endDate As Date = New Date(dtiReportEndDate.Value.Year, dtiReportEndDate.Value.Month, dtiReportEndDate.Value.Day, 0, 0, 0)
                endDate = endDate.AddDays(1) ' Add 1 to the date so we can check everything less than it

                ' Set the report name
                Dim reportName As String = cboReport.SelectedItem.ToString

                ' Set the report title
                Dim reportTitle As String = reportName & "<br />from " & startDate.ToLongDateString & " to " & endDate.AddDays(-1).ToLongDateString

                ' Create the report title
                Dim strHTML As String = PrismReports.HTMLHeader("Prism Report - " & reportName, reportTitle)

                ' Build the Owners List
                Dim ownerNames As New List(Of String)
                For Each lvi As ListViewItem In CType(cboReportOwners.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                    ownerNames.Add(lvi.Name)
                Next

                ' Choose what report is selected and get the information
                Select Case reportName

                    Case "Income Report"
                        Dim reportData As DataSet = PrismReports.GetJournalReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult = PrismReports.GenerateIncomeReportBodyHTML(PrismReports.GenerateIncomeAnalysis(reportData))
                        strHTML &= result.HTML

                    Case "Expenditure Report"
                        Dim reportData As DataSet = PrismReports.GetJournalReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult = PrismReports.GenerateExpenseReportBodyHTML(PrismReports.GenerateExpenseAnalysis(reportData))
                        strHTML &= result.HTML

                    Case "Income & Expenditure Report"
                        Dim reportData As DataSet = PrismReports.GetJournalReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult = PrismReports.GenerateIncomeReportBodyHTML(PrismReports.GenerateIncomeAnalysis(reportData))
                        strHTML &= result.HTML
                        Dim incomeTotal As Double = CDbl(result.Values("Total Income"))
                        Dim eResult As ReportResult = PrismReports.GenerateExpenseReportBodyHTML(PrismReports.GenerateExpenseAnalysis(reportData))
                        strHTML &= eResult.HTML
                        Dim expenditureTotal As Double = CDbl(eResult.Values("Total Expenditure"))
                        Dim cResult As ReportResult = PrismReports.GenerateCashFlowReportBodyHTML(incomeTotal, expenditureTotal)
                        strHTML &= cResult.HTML
                        Dim mResult As ReportResult = PrismReports.GenerateMovementReportBodyHTML(PrismReports.GenerateOwnerMovements(reportData))
                        strHTML &= mResult.HTML

                    Case "Corporation Tax Report"
                        Dim reportData As DataSet = PrismReports.GetJournalReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult = PrismReports.GenerateCorpTaxReportBodyHTML(PrismReports.GenerateCorpTaxAnalysis(reportData))
                        strHTML &= result.HTML

                    Case "Transaction Sales Report"
                        Dim reportData As DataSet = PrismReports.GetTransactionReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult = PrismReports.GenerateSalesReportBodyHTML(PrismReports.GenerateTransactionSalesAnalysis(reportData))
                        strHTML &= result.HTML

                    Case "Transaction Purchases Report"
                        Dim reportData As DataSet = PrismReports.GetTransactionReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult = PrismReports.GeneratePurchasesReportBodyHTML(PrismReports.GenerateTransactionPurchasesAnalysis(reportData))
                        strHTML &= result.HTML

                    Case "Transaction Trading Report"
                        Dim reportData As DataSet = PrismReports.GetTransactionReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult = PrismReports.GenerateTradingProfitReportBodyHTML(PrismReports.GenerateTransactionProfitAnalysis(reportData))
                        strHTML &= result.HTML

                    Case "Journal Income Type Analysis"
                        ' Get the RefTypeID
                        Dim refTypeID As String = CType(cboReportJournalType.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems(0).Name
                        Dim reportData As DataSet = PrismReports.GetJournalReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult
                        Select Case refTypeID
                            Case "37"
                                result = PrismReports.GenerateJournalTypeIncomeReportBodyHTML(refTypeID, PrismReports.GenerateJournalTypeIncomeAnalysis(reportData, refTypeID, JournalKeyTypes.OwnerName2))
                            Case Else
                                result = PrismReports.GenerateJournalTypeIncomeReportBodyHTML(refTypeID, PrismReports.GenerateJournalTypeIncomeAnalysis(reportData, refTypeID, JournalKeyTypes.OwnerName1))
                        End Select
                        strHTML &= result.HTML

                    Case "Journal Expenditure Type Analysis"
                        ' Get the RefTypeID
                        Dim refTypeID As String = CType(cboReportJournalType.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems(0).Name
                        Dim reportData As DataSet = PrismReports.GetJournalReportData(startDate, endDate, ownerNames)
                        Dim result As ReportResult
                        Select Case refTypeID
                            Case "37", "13"
                                result = PrismReports.GenerateJournalTypeExpenditureReportBodyHTML(refTypeID, PrismReports.GenerateJournalTypeExpenditureAnalysis(reportData, refTypeID, JournalKeyTypes.OwnerName2))
                            Case Else
                                result = PrismReports.GenerateJournalTypeExpenditureReportBodyHTML(refTypeID, PrismReports.GenerateJournalTypeExpenditureAnalysis(reportData, refTypeID, JournalKeyTypes.OwnerName1))
                        End Select
                        strHTML &= result.HTML

                End Select

                ' Save and navigate to the report
                Dim reportFileName As String = Path.Combine(HQ.ReportFolder, reportName.Replace(" ", "") & ".html")
                Dim sw As StreamWriter = New StreamWriter(reportFileName)
                sw.Write(strHTML)
                sw.Flush()
                sw.Close()
                wbReport.Navigate(reportFileName)

            Else
                MessageBox.Show("You must select a report type before generating a report!", "Report Type Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub


#End Region

#Region "Rig Builder Routines"
        Private Sub GetSalvage()

            Dim pOwner As PrismOwner
            _salvageList.Clear()

            For Each cOwner As ListViewItem In PSCRigOwners.ItemList.CheckedItems

                If PlugInData.PrismOwners.ContainsKey(cOwner.Text) = True Then
                    pOwner = PlugInData.PrismOwners(cOwner.Text)
                    Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(pOwner, CorpRepType.Assets)
                    Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(pOwner, CorpRepType.Assets)

                    If ownerAccount IsNot Nothing Then

                        Dim assetXML As XmlDocument
                        Dim apireq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
                        If pOwner.IsCorp = True Then
                            assetXML = apireq.GetAPIXML(APITypes.AssetsCorp, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                        Else
                            assetXML = apireq.GetAPIXML(APITypes.AssetsChar, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                        End If

                        If assetXML IsNot Nothing Then
                            Dim locList As XmlNodeList
                            Dim loc As XmlNode
                            locList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                            If locList.Count > 0 Then
                                For Each loc In locList
                                    Dim itemID As Integer = CInt(loc.Attributes.GetNamedItem("typeID").Value)
                                    If StaticData.Types.ContainsKey(itemID) = True Then
                                        Dim groupID As String = StaticData.Types(itemID).Group.ToString
                                        If CLng(groupID) = 754 Then

                                            Dim quantity As Long = CLng(loc.Attributes.GetNamedItem("quantity").Value)
                                            Dim itemName As String = StaticData.Types(itemID).Name
                                            If _salvageList.Contains(itemName) = False Then
                                                _salvageList.Add(itemName, quantity)
                                            Else
                                                _salvageList.Item(itemName) = CLng(_salvageList.Item(itemName)) + quantity
                                            End If
                                        End If
                                    End If

                                    ' Check if this row has child nodes and repeat
                                    If loc.HasChildNodes = True Then
                                        Call GetSalvageNode(_salvageList, loc)
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            Next
        End Sub
        Private Sub GetSalvageNode(ByVal salvageList As SortedList, ByVal loc As XmlNode)
            Dim subLocList As XmlNodeList
            Dim subLoc As XmlNode
            subLocList = loc.ChildNodes(0).ChildNodes
            For Each subLoc In subLocList
                Try
                    Dim itemID As Integer = CInt(subLoc.Attributes.GetNamedItem("typeID").Value)
                    If StaticData.Types.ContainsKey(itemID) = True Then
                        Dim groupID As String = StaticData.Types(itemID).Group.ToString
                        If CLng(groupID) = 754 Then
                            Dim quantity As Long = CLng(subLoc.Attributes.GetNamedItem("quantity").Value)
                            Dim itemName As String = StaticData.Types(itemID).Name
                            If salvageList.Contains(itemName) = False Then
                                salvageList.Add(itemName, quantity)
                            Else
                                salvageList.Item(itemName) = CLng(salvageList.Item(itemName)) + quantity
                            End If
                        End If
                    End If

                    If subLoc.HasChildNodes = True Then
                        Call GetSalvageNode(salvageList, subLoc)
                    End If

                Catch ex As Exception

                End Try
            Next
        End Sub
        Private Sub PrepareRigData()
            ' Clear the build list
            adtRigBuildList.Nodes.Clear()

            ' Build a Salvage List
            Call GetSalvage()

            Dim bpName As String
            _rigBPData = New SortedList(Of String, SortedList(Of Integer, Long))

            ' Get the items in the group and build the materials
            Dim items As IEnumerable(Of EveType) = StaticData.GetItemsInGroup(787)
            For Each item As EveType In items
                bpName = item.Name.TrimEnd(" Blueprint".ToCharArray)
                _rigBPData.Add(bpName, New SortedList(Of Integer, Long))
                Dim rigBP As EveData.Blueprint = StaticData.Blueprints(item.Id)
                For Each br As EveData.BlueprintResource In rigBP.Resources(1).Values
                    ' Check if the resource is salvage
                    If br.TypeGroup = 754 Then
                        _rigBPData(bpName).Add(br.TypeId, br.Quantity)
                    End If
                Next
            Next

        End Sub
        Private Sub GetBuildList()
            Dim buildableBP As Boolean
            Dim material As Integer
            Dim minQuantity As Double
            Dim buildCost As Double
            Dim rigCost As Double
            adtRigs.BeginUpdate()
            adtRigs.Nodes.Clear()
            Dim bpCostsTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From blueprint In _rigBPData.Keys Select StaticData.TypeNames(CStr(blueprint)))
            bpCostsTask.Wait()
            Dim bpCosts As Dictionary(Of Integer, Double) = bpCostsTask.Result
            For Each blueprint As String In _rigBPData.Keys
                If StaticData.TypeNames.ContainsKey(blueprint) = True Then
                    buildableBP = True
                    minQuantity = 1.0E+99
                    buildCost = 0
                    ' Fetch the build requirements
                    _rigBuildData = _rigBPData(blueprint)
                    ' Go through the requirements and see if have sufficient materials
                    For Each material In _rigBuildData.Keys
                        If _salvageList.Contains(material) = True Then
                            ' Check quantity
                            If CDbl(_salvageList(material)) > CDbl(_rigBuildData(material)) Then
                                ' We have enough so let's calculate the quantity we can use
                                minQuantity = Math.Min(minQuantity, (CDbl(_salvageList(material)) / CDbl(_rigBuildData(material))))
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
                        Dim costTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From mat In _rigBuildData.Keys Select StaticData.TypeNames(CStr(mat)))
                        costTask.Wait()
                        Dim costs As Dictionary(Of Integer, Double) = costTask.Result
                        For Each material In _rigBuildData.Keys
                            ' Get price
                            buildCost += CInt(_rigBuildData(material)) * costs(material)
                        Next
                        rigCost = bpCosts(StaticData.TypeNames(blueprint))
                        Dim lviBP2 As New Node
                        lviBP2.Text = blueprint
                        Dim qty As Integer = CInt(Int(minQuantity))
                        lviBP2.Cells.Add(New Cell(qty.ToString("N0")))
                        lviBP2.Cells.Add(New Cell(rigCost.ToString("N2")))
                        lviBP2.Cells.Add(New Cell(buildCost.ToString("N2")))
                        lviBP2.Cells.Add(New Cell((rigCost - buildCost).ToString("N2")))
                        lviBP2.Cells.Add(New Cell((qty * rigCost).ToString("N2")))
                        lviBP2.Cells.Add(New Cell((qty * buildCost).ToString("N2")))
                        lviBP2.Cells.Add(New Cell((qty * (rigCost - buildCost)).ToString("N2")))
                        If qty = 0 Or rigCost = 0 Then
                            lviBP2.Cells.Add(New Cell(CInt(0).ToString("N2")))
                        Else
                            lviBP2.Cells.Add(New Cell((qty * (rigCost - buildCost) / (qty * rigCost) * 100).ToString("N2")))
                        End If
                        adtRigs.Nodes.Add(lviBP2)
                    End If
                End If
            Next
            AdvTreeSorter.Sort(adtRigs, 1, True, True)
            adtRigs.EndUpdate()
        End Sub
        Private Sub adtRigs_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtRigs.NodeDoubleClick
            Call AddRigToBuildList(e.Node)
            Call GetBuildList()
            Call CalculateRigBuildInfo()
        End Sub
        Private Sub adtRigBuildList_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtRigBuildList.NodeDoubleClick
            Call RemoveRigFromBuildList(e.Node)
            Call GetBuildList()
            Call CalculateRigBuildInfo()
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
            Dim rigSalvageList As SortedList(Of Integer, Long) = _rigBPData(currentRig.Text)
            For Each salvage As Integer In rigSalvageList.Keys
                _salvageList(salvage) = CInt(_salvageList(salvage)) - (CInt(rigSalvageList(salvage)) * CInt(currentRig.Cells(1).Text))
            Next
        End Sub
        Private Sub RemoveRigFromBuildList(ByVal currentRig As Node)
            ' Remove the selected rig to the build list
            adtRigBuildList.Nodes.Remove(currentRig)
            ' Get the salvage used by the rig and reduce the main list
            Dim rigSalvageList As SortedList(Of Integer, Long) = _rigBPData(currentRig.Text)
            For Each salvage As Integer In rigSalvageList.Keys
                _salvageList(salvage) = CInt(_salvageList(salvage)) + (CInt(rigSalvageList(salvage)) * CInt(currentRig.Cells(1).Text))
            Next
        End Sub
        Private Sub chkRigSaleprice_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRigSalePrice.CheckedChanged
            If chkRigSalePrice.Checked = True Then
                btnAutoRig.Tag = 3
            End If
        End Sub
        Private Sub chkRigProfit_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRigProfit.CheckedChanged
            If chkRigProfit.Checked = True Then
                btnAutoRig.Tag = 5
            End If
        End Sub
        Private Sub chkRigMargin_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRigMargin.CheckedChanged
            If chkRigMargin.Checked = True Then
                btnAutoRig.Tag = 9
            End If
        End Sub
        Private Sub chkTotalSalePrice_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkTotalSalePrice.CheckedChanged
            If chkTotalSalePrice.Checked = True Then
                btnAutoRig.Tag = 6
            End If
        End Sub
        Private Sub chkTotalProfit_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkTotalProfit.CheckedChanged
            If chkTotalProfit.Checked = True Then
                btnAutoRig.Tag = 8
            End If
        End Sub
        Private Sub CalculateRigBuildInfo()
            Dim totalRsp, totalRp As Double
            For Each rigItem As Node In adtRigBuildList.Nodes
                totalRsp += CDbl(rigItem.Cells(5).Text)
                totalRp += CDbl(rigItem.Cells(7).Text)
            Next
            lblTotalRigSalePrice.Text = "Total Rig Sale Price: " & totalRsp.ToString("N2")
            lblTotalRigProfit.Text = "Total Rig Profit: " & totalRp.ToString("N2")
            lblTotalRigMargin.Text = "Margin: " & (totalRp / totalRsp * 100).ToString("N2") & "%"
        End Sub
        Private Sub btnAutoRig_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAutoRig.Click
            ' Get the rig and salvage info
            Call PrepareRigData()
            ' Get the list of available rigs
            Call GetBuildList()
            Do While adtRigs.Nodes.Count > 0
                AdvTreeSorter.Sort(adtRigs, New AdvTreeSortResult(CInt(btnAutoRig.Tag), AdvTreeSortOrder.Descending), False)
                AddRigToBuildList(adtRigs.Nodes(0))
                Call GetBuildList()
            Loop
            AdvTreeSorter.Sort(adtRigBuildList, New AdvTreeSortResult(CInt(btnAutoRig.Tag), AdvTreeSortOrder.Descending), False)
            Call CalculateRigBuildInfo()
        End Sub
        Private Sub btnBuildRigs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuildRigs.Click

            ' Get the rig and salvage info
            Call PrepareRigData()

            ' Get the list of available rigs
            Call GetBuildList()
        End Sub
        Private Sub btnExportRigList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportRigList.Click
            Call GenerateCsvFileFromClv(PSCRigOwners.cboHost.Text, "Rig List", adtRigs)
        End Sub
        Private Sub btnExportRigBuildList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportRigBuildList.Click
            Call GenerateCsvFileFromClv(PSCRigOwners.cboHost.Text, "Rig Build List", adtRigBuildList)
        End Sub
        Private Sub adtRigs_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtRigs.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub
        Private Sub adtRigBuildList_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtRigBuildList.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
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
            strSQL &= " WHERE inventionResults.resultDate >= '" & dtiInventionStartDate.Value.ToString(PrismTimeFormat, _culture) & "' AND inventionResults.resultDate <= '" & dtiInventionEndDate.Value.ToString(PrismTimeFormat, _culture) & "'"

            ' Build the Owners List
            If cboInventionInstallers.Text <> "<All>" Then
                Dim ownerList As New StringBuilder
                For Each lvi As ListViewItem In CType(cboInventionInstallers.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                    ownerList.Append(", '" & lvi.Name.Replace("'", "''") & "'")
                Next
                If ownerList.Length > 2 Then
                    ownerList.Remove(0, 2)
                End If
                ' Default to None
                strSQL &= " AND inventionResults.installerName IN (" & ownerList.ToString & ")"
            End If

            ' Filter item type
            If cboInventionItems.Text <> "All" Then
                ' Build a ref type list
                Dim itemTypeList As New StringBuilder
                For Each lvi As ListViewItem In CType(cboInventionItems.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems
                    itemTypeList.Append(", '" & lvi.Name.Replace("'", "''") & "'")
                Next
                If itemTypeList.Length > 2 Then
                    itemTypeList.Remove(0, 2)
                    strSQL &= " AND inventionResults.typeName IN (" & itemTypeList.ToString & ")"
                End If
            End If

            ' Order the data
            strSQL &= " ORDER BY inventionResults.resultDate ASC;"

            ' Get the data
            Dim jobList As SortedList(Of Long, InventionAPIJob) = InventionAPIJob.ParseInventionJobsFromDB(strSQL)

            ' Populate the list
            adtInventionResults.BeginUpdate()
            adtInventionResults.Nodes.Clear()

            If jobList.Count > 0 Then

                For Each job As InventionAPIJob In jobList.Values
                    Dim jobItem As New Node
                    jobItem.Name = job.JobID.ToString
                    jobItem.Text = job.ResultDate.ToString
                    jobItem.Cells.Add(New Cell(job.TypeName))
                    jobItem.Cells.Add(New Cell(job.InstallerName))
                    Select Case job.Result
                        Case 1
                            jobItem.Cells.Add(New Cell("Successful"))
                        Case Else
                            jobItem.Cells.Add(New Cell("Failed"))
                    End Select
                    adtInventionResults.Nodes.Add(jobItem)
                Next
                adtInventionResults.Enabled = True
            Else
                adtInventionResults.Nodes.Add(New Node("No Data Available..."))
                adtInventionResults.Enabled = False
            End If

            adtInventionResults.EndUpdate()

            ' Update the Stats
            Call DisplayInventionStats(jobList)

        End Sub

        Private Sub DisplayInventionStats(ByVal jobList As SortedList(Of Long, InventionAPIJob))

            adtInventionStats.BeginUpdate()
            adtInventionStats.Nodes.Clear()

            ' Clear and add default column
            adtInventionStats.Columns.Clear()
            Dim typeNameCol As New DevComponents.AdvTree.ColumnHeader
            typeNameCol.SortingEnabled = False
            typeNameCol.Name = "TypeName"
            typeNameCol.Text = "Item Type"
            typeNameCol.Width.Absolute = 250
            typeNameCol.DisplayIndex = 1
            adtInventionStats.Columns.Add(typeNameCol)

            ' Get the Invention Stats
            Dim stats As SortedList(Of String, SortedList(Of String, InventionResults)) = InventionAPIJob.CalculateInventionStats(jobList)

            If stats.Count > 0 Then

                ' Add columns and rows based
                Dim colIdx As Integer = 0
                For Each installerName As String In stats.Keys
                    Dim col As New DevComponents.AdvTree.ColumnHeader
                    col.SortingEnabled = False
                    colIdx += 1
                    col.Name = installerName
                    col.Text = installerName
                    col.Width.Absolute = 150
                    col.DisplayIndex = colIdx + 1
                    col.EditorType = eCellEditorType.Custom
                    adtInventionStats.Columns.Add(col)

                    ' Check for modules
                    For Each typeName As String In stats(installerName).Keys
                        ' Check it doesn't already exist
                        Dim typeNode As Node = adtInventionStats.FindNodeByName(typeName)
                        If typeNode Is Nothing Then
                            ' Node doesn't exist, so add it
                            typeNode = New Node
                            typeNode.Name = typeName
                            typeNode.Text = typeName
                            adtInventionStats.Nodes.Add(typeNode)
                        End If
                    Next
                Next

                ' Add the "Average Column"
                Dim avgCol As New DevComponents.AdvTree.ColumnHeader
                avgCol.SortingEnabled = False
                colIdx += 1
                avgCol.Name = "Item Average"
                avgCol.Text = "Item Average"
                avgCol.Width.Absolute = 150
                avgCol.DisplayIndex = colIdx + 1
                avgCol.EditorType = eCellEditorType.Custom
                adtInventionStats.Columns.Add(avgCol)

                ' Populate the grid with blank cells
                For n As Integer = 0 To adtInventionStats.Nodes.Count - 1
                    For c As Integer = 1 To adtInventionStats.Columns.Count - 1
                        adtInventionStats.Nodes(n).Cells.Add(New Cell("n/a"))
                        adtInventionStats.Nodes(n).Cells(c).Tag = -1
                    Next
                Next

                ' Populate the grid with proper data
                Dim typeAvgs As New SortedList(Of String, InventionResults)
                colIdx = 0
                For Each installerName As String In stats.Keys
                    colIdx += 1

                    ' Check for modules
                    For Each typeName As String In stats(installerName).Keys
                        ' Check it doesn't already exist
                        Dim typeNode As Node = adtInventionStats.FindNodeByName(typeName)
                        If typeNode IsNot Nothing Then
                            ' Get the results
                            Dim results As InventionResults = stats(installerName).Item(typeName)
                            Dim percent As Double = results.Successes / (results.Successes + results.Failures) * 100
                            typeNode.Cells(colIdx).Text = results.Successes.ToString & " / " & (results.Successes + results.Failures).ToString & " (" & percent.ToString("N2") & "%)"
                            typeNode.Cells(colIdx).Tag = percent
                            ' Add results to the averages
                            Dim typeAvg As New InventionResults
                            If typeAvgs.ContainsKey(typeName) = True Then
                                typeAvg = typeAvgs(typeName)
                            Else
                                typeAvgs.Add(typeName, typeAvg)
                            End If
                            typeAvg.Successes += results.Successes
                            typeAvg.Failures += results.Failures
                        End If
                    Next
                Next

                ' Display Averages
                colIdx = adtInventionStats.Columns.Count - 1
                For Each typeNode As Node In adtInventionStats.Nodes
                    If typeAvgs.ContainsKey(typeNode.Name) = True Then
                        Dim results As InventionResults = typeAvgs(typeNode.Name)
                        Dim percent As Double = results.Successes / (results.Successes + results.Failures) * 100
                        typeNode.Cells(colIdx).Text = results.Successes.ToString & " / " & (results.Successes + results.Failures).ToString & " (" & percent.ToString("N2") & "%)"
                        typeNode.Cells(colIdx).Tag = percent
                    End If
                Next

                AdvTreeSorter.Sort(adtInventionStats, 1, False, True)

                adtInventionStats.Enabled = True
            Else
                adtInventionStats.Nodes.Add(New Node("No Data Available..."))
                adtInventionStats.Enabled = False
            End If

            adtInventionStats.EndUpdate()

        End Sub

        Private Sub btnGetResults_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetResults.Click
            Call DisplayInventionResults()
        End Sub

        Private Sub dtiInventionStartDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As EventArgs) Handles dtiInventionStartDate.ButtonCustom2Click
            dtiInventionStartDate.Value = New Date(dtiInventionStartDate.Value.Year, dtiInventionStartDate.Value.Month, dtiInventionStartDate.Value.Day)
        End Sub

        Private Sub dtiInventionStartDate_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles dtiInventionStartDate.ButtonCustomClick
            dtiInventionStartDate.Value = Now
        End Sub

        Private Sub dtiInventionEndDate_ButtonCustom2Click(ByVal sender As Object, ByVal e As EventArgs) Handles dtiInventionEndDate.ButtonCustom2Click
            dtiInventionEndDate.Value = New Date(dtiInventionEndDate.Value.Year, dtiInventionEndDate.Value.Month, dtiInventionEndDate.Value.Day)
        End Sub

        Private Sub dtiInventionEndDate_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles dtiInventionEndDate.ButtonCustomClick
            dtiInventionEndDate.Value = Now
        End Sub

        Private Sub adtInventionResults_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtInventionResults.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

        Private Sub adtInventionStats_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtInventionStats.ColumnHeaderMouseUp
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

#End Region

#Region "Timer Update Methods"

        Private Sub tmrUpdateInfo_Tick(sender As Object, e As EventArgs) Handles tmrUpdateInfo.Tick
            ' Use this to update any information on the form with reasonable frequency

            ' Check if the jobs tab is visible
            If tiJobs.Visible = True Then
                ' Update the jobs screen as appropriate
                Call UpdateIndustryJobTimes()
            End If
        End Sub

#End Region

    End Class
End Namespace