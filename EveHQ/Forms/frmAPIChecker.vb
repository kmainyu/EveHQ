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
        ' Load up account characters into the character combo
        cboCharacter.BeginUpdate()
        cboCharacter.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Account <> "" Then
                cboCharacter.Items.Add(cPilot.Name)
            End If
        Next
        cboCharacter.EndUpdate()
        ' Load up the Account combo
        cboAccount.BeginUpdate()
        cboAccount.Items.Clear()
        For account As Integer = 1000 To 1006
            cboAccount.Items.Add(CStr(account))
        Next
        cboAccount.EndUpdate()

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

    Private Sub cboAPIMethod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
                EveAPI.APITypes.CertificateTree
                lblCharacter.Enabled = False : cboCharacter.Enabled = False
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                APIStyle = 1

            Case EveAPI.APITypes.NameToID, EveAPI.APITypes.IDToName
                lblCharacter.Enabled = False : cboCharacter.Enabled = False
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                If CInt(APIMethods(cboAPIType.SelectedItem)) = EveAPI.APITypes.NameToID Then
                    lblOtherInfo.Text = "Item Name"
                Else
                    lblOtherInfo.Text = "Item ID:"
                End If
                APIStyle = 2

            Case EveAPI.APITypes.Characters, EveAPI.APITypes.AccountStatus
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = False : cboAccount.Enabled = False
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
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                APIStyle = 4

            Case EveAPI.APITypes.POSDetails, EveAPI.APITypes.OutpostServiceDetail
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "ItemID:"
                APIStyle = 5

            Case EveAPI.APITypes.WalletTransChar, EveAPI.APITypes.WalletTransCorp
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = True : cboAccount.Enabled = True
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "Before RefID:"
                APIStyle = 6

            Case EveAPI.APITypes.KillLogChar, EveAPI.APITypes.KillLogChar, EveAPI.APITypes.MailBodies, EveAPI.APITypes.CalendarEventAttendeesChar
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "IDs:"
                APIStyle = 7

            Case EveAPI.APITypes.WalletJournalChar, EveAPI.APITypes.WalletJournalCorp
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = True : cboAccount.Enabled = True
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "Before RefID:"
                APIStyle = 8

        End Select
    End Sub

    Private Sub btnFetchAPI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFetchAPI.Click
        Dim selpilot As New EveHQ.Core.Pilot
        Dim pilotAccount As New EveHQ.Core.EveAccount
        ' Check for the info
        If APIStyle > 2 Then
            If cboCharacter.SelectedItem Is Nothing Then
                MessageBox.Show("You must select a character to retrieve the requested API.", "Additional Info Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            selpilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboCharacter.SelectedItem.ToString), Core.Pilot)
            Dim accountName As String = selpilot.Account
            pilotAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
        End If
        Select Case APIStyle
            Case 2, 5
                If txtOtherInfo.Text = "" Then
                    MessageBox.Show("You must enter some data to retrieve the requested API.", "Additional Info Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            Case 6, 8
                If cboAccount.SelectedItem Is Nothing Then
                    MessageBox.Show("You must select an account key to retrieve the requested API.", "Additional Info Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
        End Select
        Dim testXML As New XmlDocument
        Dim returnMethod As EveAPI.APIReturnMethods = EveAPI.APIReturnMethods.ReturnStandard
        If chkReturnCachedXML.Checked = True Then
            returnMethod = EveAPI.APIReturnMethods.ReturnCacheOnly
        End If
        If chkReturnActualXML.Checked = True Then
            returnMethod = EveAPI.APIReturnMethods.ReturnActual
        End If
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
        Select Case APIStyle
            Case 1
                testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), returnMethod)
            Case 2
                testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), txtOtherInfo.Text.Trim, returnMethod)
            Case 3
                testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), pilotAccount.ToAPIAccount, returnMethod)
            Case 4
                testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), pilotAccount.ToAPIAccount, selpilot.ID, returnMethod)
            Case 5
                testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), pilotAccount.ToAPIAccount, selpilot.ID, CInt(txtOtherInfo.Text), returnMethod)
            Case 6
                testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), pilotAccount.ToAPIAccount, selpilot.ID, CInt(cboAccount.SelectedItem.ToString), txtOtherInfo.Text, returnMethod)
            Case 7
                testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), pilotAccount.ToAPIAccount, selpilot.ID, txtOtherInfo.Text, returnMethod)
            Case 8
                testXML = APIReq.GetAPIXML(CType(CInt(APIMethods.Item(cboAPIType.SelectedItem.ToString)), EveAPI.APITypes), pilotAccount.ToAPIAccount, selpilot.ID, CInt(cboAccount.SelectedItem.ToString), 0, 256, returnMethod)
        End Select
        Try
            wbAPI.Navigate(APIReq.LastAPIFileName)
            lblCurrentlyViewing.Text = "Currently Viewing: " & cboAPIType.SelectedItem.ToString
            lblFileLocation.Text = "Cache File Location: " & APIReq.LastAPIFileName
        Catch ex As Exception
            MessageBox.Show("There was an error trying to display the requested API. The error was: " & ControlChars.CrLf & ex.Message, "Error Requesting API", MessageBoxButtons.OK, MessageBoxIcon.Error)
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