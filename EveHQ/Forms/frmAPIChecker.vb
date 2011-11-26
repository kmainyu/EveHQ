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

Public Class frmAPIChecker
    Dim APIMethods As New SortedList
    Dim APIStyle As Integer = 0

    Private Sub frmAPIChecker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Load up the Account combo
        cboWalletAccount.BeginUpdate()
        cboWalletAccount.Items.Clear()
        For account As Integer = 1000 To 1006
            cboWalletAccount.Items.Add(account.ToString)
        Next
        cboWalletAccount.EndUpdate()

        ' Set default to characters
        cboAPICategory.SelectedIndex = 0

    End Sub

    Private Sub cboAPICategory_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboAPICategory.SelectedIndexChanged
        ' Update the API Type combo box with relevant APIs
        APIMethods.Clear()
        cboAPIType.BeginUpdate()
        cboAPIType.Items.Clear()
        Dim APIName As String = ""
        Select Case cboAPICategory.SelectedItem.ToString
            Case "Character"
                ' Update the APIs
                For Each APIMethod As Integer In [Enum].GetValues(GetType(CharacterAPIs))
                    APIName = [Enum].GetName(GetType(EveAPI.APITypes), APIMethod)
                    If APIMethods.ContainsKey(APIName) = False Then
                        APIMethods.Add(APIName, APIMethod)
                        cboAPIType.Items.Add(APIName)
                    End If
                Next
                ' Update the character list
                cboAPIOwner.BeginUpdate()
                cboAPIOwner.Items.Clear()
                For Each APIPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                    If APIPilot.Account <> "" Then
                        cboAPIOwner.Items.Add(APIPilot.Name)
                    End If
                Next
                cboAPIOwner.EndUpdate()
                cboAPIOwner.Enabled = True
            Case "Corporation"
                ' Update the APIs
                For Each APIMethod As Integer In [Enum].GetValues(GetType(CorporateAPIs))
                    APIName = [Enum].GetName(GetType(EveAPI.APITypes), APIMethod)
                    If APIMethods.ContainsKey(APIName) = False Then
                        APIMethods.Add(APIName, APIMethod)
                        cboAPIType.Items.Add(APIName)
                    End If
                Next
                ' Update the corporation list
                cboAPIOwner.BeginUpdate()
                cboAPIOwner.Items.Clear()
                For Each APICorp As EveHQ.Core.Corporation In EveHQ.Core.HQ.EveHQSettings.Corporations.Values
                    If APICorp.Accounts(0) <> "" Then
                        cboAPIOwner.Items.Add(APICorp.Name)
                    End If
                Next
                cboAPIOwner.EndUpdate()
                cboAPIOwner.Enabled = True
            Case "Account"
                ' Update the APIs
                For Each APIMethod As Integer In [Enum].GetValues(GetType(AccountAPIs))
                    APIName = [Enum].GetName(GetType(EveAPI.APITypes), APIMethod)
                    If APIMethods.ContainsKey(APIName) = False Then
                        APIMethods.Add(APIName, APIMethod)
                        cboAPIType.Items.Add(APIName)
                    End If
                Next
                ' Update the account list
                cboAPIOwner.BeginUpdate()
                cboAPIOwner.Items.Clear()
                For Each APIAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
                    cboAPIOwner.Items.Add(APIAccount.FriendlyName)
                Next
                cboAPIOwner.EndUpdate()
                cboAPIOwner.Enabled = True
            Case "Static"
                ' Update the APIs
                For Each APIMethod As Integer In [Enum].GetValues(GetType(StaticAPIs))
                    APIName = [Enum].GetName(GetType(EveAPI.APITypes), APIMethod)
                    If APIMethods.ContainsKey(APIName) = False Then
                        APIMethods.Add(APIName, APIMethod)
                        cboAPIType.Items.Add(APIName)
                    End If
                Next
                ' Remove the list
                cboAPIOwner.Items.Clear()
                cboAPIOwner.Enabled = False
        End Select
        cboAPIType.EndUpdate()
    End Sub

    Private Sub cboAPIType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboAPIType.SelectedIndexChanged
        ' Find out the selected APIMethod and determine what information we need
        Select Case CInt(APIMethods(cboAPIType.SelectedItem))

            Case EveAPI.APITypes.AllianceList, _
                EveAPI.APITypes.RefTypes, _
                EveAPI.APITypes.SkillTree, _
                EveAPI.APITypes.Sovereignty, _
                EveAPI.APITypes.SovereigntyStatus, _
                EveAPI.APITypes.MapJumps, _
                EveAPI.APITypes.MapKills, _
                EveAPI.APITypes.Conquerables, _
                EveAPI.APITypes.ErrorList, _
                EveAPI.APITypes.FWStats, _
                EveAPI.APITypes.FWTop100, _
                EveAPI.APITypes.FWMap, _
                EveAPI.APITypes.ServerStatus, _
                EveAPI.APITypes.CertificateTree, _
                EveAPI.APITypes.CallList
                lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                APIStyle = 1

            Case EveAPI.APITypes.NameToID, EveAPI.APITypes.IDToName
                lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                If CInt(APIMethods(cboAPIType.SelectedItem)) = EveAPI.APITypes.NameToID Then
                    lblOtherInfo.Text = "Item Name"
                Else
                    lblOtherInfo.Text = "Item ID:"
                End If
                APIStyle = 2

            Case EveAPI.APITypes.Characters, EveAPI.APITypes.AccountStatus
                lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                APIStyle = 3

            Case EveAPI.APITypes.AccountBalancesChar, _
              EveAPI.APITypes.AccountBalancesCorp, _
              EveAPI.APITypes.CharacterSheet, _
              EveAPI.APITypes.CorpSheet, _
              EveAPI.APITypes.CorpMemberTracking, _
              EveAPI.APITypes.SkillTraining, _
              EveAPI.APITypes.SkillQueue, _
              EveAPI.APITypes.AssetsChar, _
              EveAPI.APITypes.AssetsCorp, _
              EveAPI.APITypes.IndustryChar, _
              EveAPI.APITypes.IndustryCorp, _
              EveAPI.APITypes.OrdersChar, _
              EveAPI.APITypes.OrdersCorp, _
              EveAPI.APITypes.POSList, _
              EveAPI.APITypes.OutpostList, _
              EveAPI.APITypes.StandingsChar, _
              EveAPI.APITypes.StandingsCorp, _
              EveAPI.APITypes.CorpMemberSecurity, _
              EveAPI.APITypes.CorpMemberSecurityLog, _
              EveAPI.APITypes.CorpShareholders, _
              EveAPI.APITypes.CorpTitles, _
              EveAPI.APITypes.FWStatsChar, _
              EveAPI.APITypes.FWStatsCorp, _
              EveAPI.APITypes.MedalsReceived, _
              EveAPI.APITypes.MedalsAvailable, _
              EveAPI.APITypes.MailMessages, _
              EveAPI.APITypes.Notifications, _
              EveAPI.APITypes.MailingLists, _
              EveAPI.APITypes.Research, _
              EveAPI.APITypes.CharacterInfo, _
              EveAPI.APITypes.ContactListChar, _
              EveAPI.APITypes.ContactListCorp, _
              EveAPI.APITypes.ContactNotifications, _
              EveAPI.APITypes.UpcomingCalendarEvents, _
              EveAPI.APITypes.MemberMedals
                lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                APIStyle = 4

            Case EveAPI.APITypes.POSDetails, EveAPI.APITypes.OutpostServiceDetail
                lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "ItemID:"
                APIStyle = 5

            Case EveAPI.APITypes.WalletTransChar, EveAPI.APITypes.WalletTransCorp
                lblWalletAccount.Enabled = True : cboWalletAccount.Enabled = True
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "Before RefID:"
                APIStyle = 6

            Case EveAPI.APITypes.KillLogChar, EveAPI.APITypes.KillLogChar, EveAPI.APITypes.MailBodies, EveAPI.APITypes.CalendarEventAttendeesChar
                lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "IDs:"
                APIStyle = 7

            Case EveAPI.APITypes.WalletJournalChar, EveAPI.APITypes.WalletJournalCorp
                lblWalletAccount.Enabled = True : cboWalletAccount.Enabled = True
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "Before RefID:"
                APIStyle = 8

        End Select
    End Sub

    Private Sub btnFetchAPI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFetchAPI.Click

        ' Check we have an API Selected
        If cboAPIType.SelectedItem Is Nothing Then
            MessageBox.Show("You must select an API Type before trying to fetch one!!", "API Type Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Check we have an owner selected (if one is required)
        If cboAPIOwner.SelectedItem Is Nothing And cboAPICategory.SelectedItem.ToString <> "Static" Then
            MessageBox.Show("You must select an owner to retrieve the requested API.", "API Owner Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Establish which API Account we need to use - if any
        Dim APIAccount As New EveHQ.Core.EveAccount
        Dim OwnerID As String = ""

        Select Case cboAPICategory.SelectedItem.ToString
            Case "Character"
                APIAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboAPIOwner.SelectedItem.ToString), EveHQ.Core.Pilot).Account), EveHQ.Core.EveAccount)
                OwnerID = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboAPIOwner.SelectedItem.ToString), EveHQ.Core.Pilot).ID
            Case "Corporation"
                APIAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(EveHQ.Core.HQ.EveHQSettings.Corporations(cboAPIOwner.SelectedItem.ToString).Accounts(0)), EveHQ.Core.EveAccount)
                OwnerID = EveHQ.Core.HQ.EveHQSettings.Corporations(cboAPIOwner.SelectedItem.ToString).ID
            Case "Account"
                For Each CheckAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
                    If CheckAccount.FriendlyName = cboAPIOwner.SelectedItem.ToString Then
                        APIAccount = CheckAccount
                        OwnerID = CheckAccount.userID
                        Exit For
                    End If
                Next
            Case "Static"
                ' Don't need anything here
        End Select

        Select Case APIStyle
            Case 2, 5
                If txtOtherInfo.Text = "" Then
                    MessageBox.Show("You must enter some data to retrieve the requested API.", "Additional Info Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            Case 6, 8
                If cboWalletAccount.SelectedItem Is Nothing Then
                    MessageBox.Show("You must select a wallet account key to retrieve the requested API.", "Additional Info Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
        End Select
        Dim testXML As New XmlDocument
        Dim ReturnMethod As EveAPI.APIReturnMethods = EveAPI.APIReturnMethods.ReturnStandard
        If chkReturnStandardXML.Checked = True Then
            ReturnMethod = EveAPI.APIReturnMethods.ReturnStandard
        Else
            If chkReturnCachedXML.Checked = True Then
                ReturnMethod = EveAPI.APIReturnMethods.ReturnCacheOnly
            Else
                If chkReturnActualXML.Checked = True Then
                    ReturnMethod = EveAPI.APIReturnMethods.ReturnActual
                End If
            End If
        End If
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
        Try
            Select Case APIStyle
                Case 1
                    testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), ReturnMethod)
                Case 2
                    testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), txtOtherInfo.Text.Trim, ReturnMethod)
                Case 3
                    testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), APIAccount.ToAPIAccount, ReturnMethod)
                Case 4
                    testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), APIAccount.ToAPIAccount, OwnerID, ReturnMethod)
                Case 5
                    testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), APIAccount.ToAPIAccount, OwnerID, CLng(txtOtherInfo.Text), ReturnMethod)
                Case 6
                    testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), APIAccount.ToAPIAccount, OwnerID, CInt(cboWalletAccount.SelectedItem.ToString), txtOtherInfo.Text, ReturnMethod)
                Case 7
                    testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), APIAccount.ToAPIAccount, OwnerID, txtOtherInfo.Text, ReturnMethod)
                Case 8
                    testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), APIAccount.ToAPIAccount, OwnerID, CInt(cboWalletAccount.SelectedItem.ToString), 0, 256, ReturnMethod)
            End Select
        Catch ex As Exception
            MessageBox.Show("There was an error trying to retrieve the requested API. The error was: " & ControlChars.CrLf & ex.Message, "Error Requesting API", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Try
            wbAPI.Navigate(APIReq.LastAPIFileName)
            lblCurrentlyViewing.Text = "Currently Viewing: " & cboAPIType.SelectedItem.ToString
            lblFileLocation.Text = "Cache File Location: " & APIReq.LastAPIFileName
        Catch ex As Exception
            MessageBox.Show("There was an error trying to display the requested API. The error was: " & ControlChars.CrLf & ex.Message, "Error Displaying API", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#Region "Enumerations for API Mappings"

    ''' <summary>
    ''' A list of all available APIs for characters together with official access masks represented as the log of the actual mask
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CharacterAPIs As Integer
        AccountBalances = EveHQ.EveAPI.APITypes.AccountBalancesChar
        AssetList = EveHQ.EveAPI.APITypes.AssetsChar
        CalendarEventAttendees = EveHQ.EveAPI.APITypes.CalendarEventAttendeesChar
        CharacterSheet = EveHQ.EveAPI.APITypes.CharacterSheet
        ContactList = EveHQ.EveAPI.APITypes.ContactListChar
        ContactNotifications = EveHQ.EveAPI.APITypes.ContactNotifications
        FacWarStats = EveHQ.EveAPI.APITypes.FWStatsChar
        IndustryJobs = EveHQ.EveAPI.APITypes.IndustryChar
        KillLog = EveHQ.EveAPI.APITypes.KillLogChar
        MailBodies = EveHQ.EveAPI.APITypes.MailBodies
        MailingLists = EveHQ.EveAPI.APITypes.MailingLists
        MailMessages = EveHQ.EveAPI.APITypes.MailMessages
        MarketOrders = EveHQ.EveAPI.APITypes.OrdersChar
        Medals = EveHQ.EveAPI.APITypes.MedalsReceived
        Notifications = EveHQ.EveAPI.APITypes.Notifications
        NotificationText = EveHQ.EveAPI.APITypes.NotificationTexts
        Research = EveHQ.EveAPI.APITypes.Research
        SkillInTraining = EveHQ.EveAPI.APITypes.SkillTraining
        SkillQueue = EveHQ.EveAPI.APITypes.SkillQueue
        Standings = EveHQ.EveAPI.APITypes.StandingsChar
        UpcomingCalendarEvents = EveHQ.EveAPI.APITypes.UpcomingCalendarEvents
        WalletJournal = EveHQ.EveAPI.APITypes.WalletJournalChar
        WalletTransactions = EveHQ.EveAPI.APITypes.WalletTransChar
        CharacterInfoPrivate = EveHQ.EveAPI.APITypes.CharacterInfo
        CharacterInfoPublic = EveHQ.EveAPI.APITypes.CharacterInfo
        AccountStatus = EveHQ.EveAPI.APITypes.AccountStatus
        Contracts = EveHQ.EveAPI.APITypes.ContractsChar
        ContractItems = EveHQ.EveAPI.APITypes.ContractItemsChar
        ContractBids = EveHQ.EveAPI.APITypes.ContractBidsChar
    End Enum

    ''' <summary>
    ''' A list of all available APIs for corporations together with official access masks represented as the log of the actual mask 
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CorporateAPIs As Integer
        AccountBalances = EveHQ.EveAPI.APITypes.AccountBalancesCorp
        AssetList = EveHQ.EveAPI.APITypes.AssetsCorp
        MemberMedals = EveHQ.EveAPI.APITypes.MemberMedals
        CorporationSheet = EveHQ.EveAPI.APITypes.CorpSheet
        ContactList = EveHQ.EveAPI.APITypes.ContactListCorp
        ContainerLog = EveHQ.EveAPI.APITypes.CorpContainerLog
        FacWarStats = EveHQ.EveAPI.APITypes.FWStatsCorp
        IndustryJobs = EveHQ.EveAPI.APITypes.IndustryCorp
        KillLog = EveHQ.EveAPI.APITypes.KillLogCorp
        MemberSecurity = EveHQ.EveAPI.APITypes.CorpMemberSecurity
        MemberSecurityLog = EveHQ.EveAPI.APITypes.CorpMemberSecurityLog
        MemberTracking = EveHQ.EveAPI.APITypes.CorpMemberTracking
        MarketOrders = EveHQ.EveAPI.APITypes.OrdersCorp
        Medals = EveHQ.EveAPI.APITypes.MedalsAvailable
        OutpostList = EveHQ.EveAPI.APITypes.OutpostList
        OutpostServiceList = EveHQ.EveAPI.APITypes.OutpostServiceDetail
        Shareholders = EveHQ.EveAPI.APITypes.CorpShareholders
        StarbaseDetail = EveHQ.EveAPI.APITypes.POSDetails
        Standings = EveHQ.EveAPI.APITypes.StandingsCorp
        StarbaseList = EveHQ.EveAPI.APITypes.POSList
        WalletJournal = EveHQ.EveAPI.APITypes.WalletJournalCorp
        WalletTransactions = EveHQ.EveAPI.APITypes.WalletTransCorp
        Titles = EveHQ.EveAPI.APITypes.CorpTitles
        Contracts = EveHQ.EveAPI.APITypes.ContractsCorp
        ContractItems = EveHQ.EveAPI.APITypes.ContractItemsCorp
        ContractBids = EveHQ.EveAPI.APITypes.ContractBidsCorp
    End Enum

    ''' <summary>
    ''' A list of all available APIs for an account (that doesn't require an access mask or additional info)
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AccountAPIs As Integer
        APIKeyInfo = EveHQ.EveAPI.APITypes.APIKeyInfo
        Characters = EveHQ.EveAPI.APITypes.Characters
    End Enum

    ''' <summary>
    ''' A list of the "static" APIs available
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum StaticAPIs As Integer
        AllianceList = EveHQ.EveAPI.APITypes.AllianceList
        CertificateTree = EveHQ.EveAPI.APITypes.CertificateTree
        CharacterID = EveHQ.EveAPI.APITypes.NameToID
        CharacterInfo = EveHQ.EveAPI.APITypes.CharacterInfo
        CharacterName = EveHQ.EveAPI.APITypes.IDToName
        ConquerableStationList = EveHQ.EveAPI.APITypes.Conquerables
        ErrorList = EveHQ.EveAPI.APITypes.ErrorList
        FacWarStats = EveHQ.EveAPI.APITypes.FWStats
        FacWarTopStats = EveHQ.EveAPI.APITypes.FWTop100
        RefTypes = EveHQ.EveAPI.APITypes.RefTypes
        SkillTree = EveHQ.EveAPI.APITypes.SkillTree
        FacWarSystems = EveHQ.EveAPI.APITypes.FWMap
        Jumps = EveHQ.EveAPI.APITypes.MapJumps
        Kills = EveHQ.EveAPI.APITypes.MapKills
        Sovereignty = EveHQ.EveAPI.APITypes.Sovereignty
        SovereigntyStatus = EveHQ.EveAPI.APITypes.SovereigntyStatus
        ServerStatus = EveHQ.EveAPI.APITypes.ServerStatus
        CallList = EveHQ.EveAPI.APITypes.CallList
    End Enum

#End Region

    
End Class