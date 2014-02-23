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

Imports EveHQ.Core
Imports EveHQ.EveAPI

Namespace Forms

    Public Class FrmAPIChecker

        ReadOnly _apiMethods As New SortedList
        Dim _apiStyle As Integer = 0

        Private Sub frmAPIChecker_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

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

        Private Sub cboAPICategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAPICategory.SelectedIndexChanged
            ' Update the API Type combo box with relevant APIs
            _apiMethods.Clear()
            cboAPIType.BeginUpdate()
            cboAPIType.Items.Clear()
            Dim apiName As String
            Select Case cboAPICategory.SelectedItem.ToString
                Case "Character"
                    ' Update the APIs
                    For Each apiMethod As Integer In [Enum].GetValues(GetType(CharacterAPI))
                        apiName = [Enum].GetName(GetType(APITypes), apiMethod)
                        If _apiMethods.ContainsKey(apiName) = False Then
                            _apiMethods.Add(apiName, apiMethod)
                            cboAPIType.Items.Add(apiName)
                        End If
                    Next
                    ' Update the character list
                    cboAPIOwner.BeginUpdate()
                    cboAPIOwner.Items.Clear()
                    For Each apiPilot As EveHQPilot In HQ.Settings.Pilots.Values
                        If apiPilot.Account <> "" Then
                            cboAPIOwner.Items.Add(apiPilot.Name)
                        End If
                    Next
                    cboAPIOwner.EndUpdate()
                    cboAPIOwner.Enabled = True
                Case "Corporation"
                    ' Update the APIs
                    For Each apiMethod As Integer In [Enum].GetValues(GetType(CorporateAPI))
                        apiName = [Enum].GetName(GetType(APITypes), apiMethod)
                        If _apiMethods.ContainsKey(apiName) = False Then
                            _apiMethods.Add(apiName, apiMethod)
                            cboAPIType.Items.Add(apiName)
                        End If
                    Next
                    ' Update the corporation list
                    cboAPIOwner.BeginUpdate()
                    cboAPIOwner.Items.Clear()
                    For Each apiCorp As Corporation In HQ.Settings.Corporations.Values
                        If apiCorp.Accounts(0) <> "" Then
                            cboAPIOwner.Items.Add(apiCorp.Name)
                        End If
                    Next
                    cboAPIOwner.EndUpdate()
                    cboAPIOwner.Enabled = True
                Case "Account"
                    ' Update the APIs
                    For Each apiMethod As Integer In [Enum].GetValues(GetType(AccountAPI))
                        apiName = [Enum].GetName(GetType(APITypes), apiMethod)
                        If _apiMethods.ContainsKey(apiName) = False Then
                            _apiMethods.Add(apiName, apiMethod)
                            cboAPIType.Items.Add(apiName)
                        End If
                    Next
                    ' Update the account list
                    cboAPIOwner.BeginUpdate()
                    cboAPIOwner.Items.Clear()
                    For Each apiAccount As EveHQAccount In HQ.Settings.Accounts.Values
                        cboAPIOwner.Items.Add(apiAccount.FriendlyName)
                    Next
                    cboAPIOwner.EndUpdate()
                    cboAPIOwner.Enabled = True
                Case "Static"
                    ' Update the APIs
                    For Each apiMethod As Integer In [Enum].GetValues(GetType(StaticAPI))
                        apiName = [Enum].GetName(GetType(APITypes), apiMethod)
                        If _apiMethods.ContainsKey(apiName) = False Then
                            _apiMethods.Add(apiName, apiMethod)
                            cboAPIType.Items.Add(apiName)
                        End If
                    Next
                    ' Remove the list
                    cboAPIOwner.Items.Clear()
                    cboAPIOwner.Enabled = False
            End Select
            cboAPIType.EndUpdate()
        End Sub

        Private Sub cboAPIType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAPIType.SelectedIndexChanged
            ' Find out the selected APIMethod and determine what information we need
            Select Case CInt(_apiMethods(cboAPIType.SelectedItem))

                Case APITypes.AllianceList, _
                    APITypes.RefTypes, _
                    APITypes.SkillTree, _
                    APITypes.Sovereignty, _
                    APITypes.SovereigntyStatus, _
                    APITypes.MapJumps, _
                    APITypes.MapKills, _
                    APITypes.Conquerables, _
                    APITypes.ErrorList, _
                    APITypes.FWStats, _
                    APITypes.FWTop100, _
                    APITypes.FWMap, _
                    APITypes.ServerStatus, _
                    APITypes.CertificateTree, _
                    APITypes.CallList
                    lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                    lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                    _apiStyle = 1

                Case APITypes.NameToID, APITypes.IDToName
                    lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                    lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                    If CInt(_apiMethods(cboAPIType.SelectedItem)) = APITypes.NameToID Then
                        lblOtherInfo.Text = "Item Name"
                    Else
                        lblOtherInfo.Text = "Item ID:"
                    End If
                    _apiStyle = 2

                Case APITypes.Characters, APITypes.AccountStatus
                    lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                    lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                    _apiStyle = 3

                Case APITypes.AccountBalancesChar, _
                    APITypes.AccountBalancesCorp, _
                    APITypes.CharacterSheet, _
                    APITypes.CorpSheet, _
                    APITypes.CorpMemberTracking, _
                    APITypes.SkillTraining, _
                    APITypes.SkillQueue, _
                    APITypes.AssetsChar, _
                    APITypes.AssetsCorp, _
                    APITypes.IndustryChar, _
                    APITypes.IndustryCorp, _
                    APITypes.OrdersChar, _
                    APITypes.OrdersCorp, _
                    APITypes.POSList, _
                    APITypes.OutpostList, _
                    APITypes.StandingsChar, _
                    APITypes.StandingsCorp, _
                    APITypes.CorpMemberSecurity, _
                    APITypes.CorpMemberSecurityLog, _
                    APITypes.CorpShareholders, _
                    APITypes.CorpTitles, _
                    APITypes.FWStatsChar, _
                    APITypes.FWStatsCorp, _
                    APITypes.MedalsReceived, _
                    APITypes.MedalsAvailable, _
                    APITypes.MailMessages, _
                    APITypes.Notifications, _
                    APITypes.MailingLists, _
                    APITypes.Research, _
                    APITypes.CharacterInfo, _
                    APITypes.ContactListChar, _
                    APITypes.ContactListCorp, _
                    APITypes.ContactNotifications, _
                    APITypes.UpcomingCalendarEvents, _
                    APITypes.MemberMedals
                    lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                    lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                    _apiStyle = 4

                Case APITypes.POSDetails, APITypes.OutpostServiceDetail
                    lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                    lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                    lblOtherInfo.Text = "ItemID:"
                    _apiStyle = 5

                Case APITypes.WalletTransChar, APITypes.WalletTransCorp
                    lblWalletAccount.Enabled = True : cboWalletAccount.Enabled = True
                    lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                    lblOtherInfo.Text = "Before RefID:"
                    _apiStyle = 6

                Case APITypes.KillLogChar, APITypes.KillLogChar, APITypes.MailBodies, APITypes.CalendarEventAttendeesChar
                    lblWalletAccount.Enabled = False : cboWalletAccount.Enabled = False
                    lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                    lblOtherInfo.Text = "IDs:"
                    _apiStyle = 7

                Case APITypes.WalletJournalChar, APITypes.WalletJournalCorp
                    lblWalletAccount.Enabled = True : cboWalletAccount.Enabled = True
                    lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                    lblOtherInfo.Text = "Before RefID:"
                    _apiStyle = 8

            End Select
        End Sub

        Private Sub btnFetchAPI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFetchAPI.Click

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
            Dim apiAccount As New EveHQAccount
            Dim ownerID As String = ""

            Select Case cboAPICategory.SelectedItem.ToString
                Case "Character"
                    apiAccount = HQ.Settings.Accounts(HQ.Settings.Pilots(cboAPIOwner.SelectedItem.ToString).Account)
                    ownerID = HQ.Settings.Pilots(cboAPIOwner.SelectedItem.ToString).ID
                Case "Corporation"
                    apiAccount = HQ.Settings.Accounts(HQ.Settings.Corporations(cboAPIOwner.SelectedItem.ToString).Accounts(0))
                    ownerID = CStr(HQ.Settings.Corporations(cboAPIOwner.SelectedItem.ToString).ID)
                Case "Account"
                    For Each checkAccount As EveHQAccount In HQ.Settings.Accounts.Values
                        If checkAccount.FriendlyName = cboAPIOwner.SelectedItem.ToString Then
                            apiAccount = checkAccount
                            ownerID = checkAccount.UserID
                            Exit For
                        End If
                    Next
                Case "Static"
                    ' Don't need anything here
            End Select

            Select Case _apiStyle
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
            Dim returnMethod As APIReturnMethods = APIReturnMethods.ReturnStandard
            If chkReturnStandardXML.Checked = True Then
                returnMethod = APIReturnMethods.ReturnStandard
            Else
                If chkReturnCachedXML.Checked = True Then
                    returnMethod = APIReturnMethods.ReturnCacheOnly
                Else
                    If chkReturnActualXML.Checked = True Then
                        returnMethod = APIReturnMethods.ReturnActual
                    End If
                End If
            End If
            Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
            Try
                Select Case _apiStyle
                    Case 1
                        apiReq.GetAPIXML(CType(CInt(_apiMethods.Item(cboAPIType.SelectedItem.ToString)), APITypes), returnMethod)
                    Case 2
                        apiReq.GetAPIXML(CType(CInt(_apiMethods.Item(cboAPIType.SelectedItem.ToString)), APITypes), txtOtherInfo.Text.Trim, returnMethod)
                    Case 3
                        apiReq.GetAPIXML(CType(CInt(_apiMethods.Item(cboAPIType.SelectedItem.ToString)), APITypes), apiAccount.ToAPIAccount, returnMethod)
                    Case 4
                        apiReq.GetAPIXML(CType(CInt(_apiMethods.Item(cboAPIType.SelectedItem.ToString)), APITypes), apiAccount.ToAPIAccount, ownerID, returnMethod)
                    Case 5
                        apiReq.GetAPIXML(CType(CInt(_apiMethods.Item(cboAPIType.SelectedItem.ToString)), APITypes), apiAccount.ToAPIAccount, ownerID, CLng(txtOtherInfo.Text), returnMethod)
                    Case 6
                        apiReq.GetAPIXML(CType(CInt(_apiMethods.Item(cboAPIType.SelectedItem.ToString)), APITypes), apiAccount.ToAPIAccount, ownerID, CInt(cboWalletAccount.SelectedItem.ToString), txtOtherInfo.Text, returnMethod)
                    Case 7
                        apiReq.GetAPIXML(CType(CInt(_apiMethods.Item(cboAPIType.SelectedItem.ToString)), APITypes), apiAccount.ToAPIAccount, ownerID, txtOtherInfo.Text, returnMethod)
                    Case 8
                        apiReq.GetAPIXML(CType(CInt(_apiMethods.Item(cboAPIType.SelectedItem.ToString)), APITypes), apiAccount.ToAPIAccount, ownerID, CInt(cboWalletAccount.SelectedItem.ToString), 0, 256, returnMethod)
                End Select
            Catch ex As Exception
                MessageBox.Show("There was an error trying to retrieve the requested API. The error was: " & ControlChars.CrLf & ex.Message, "Error Requesting API", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Try
                wbAPI.Navigate(apiReq.LastAPIFileName)
                lblCurrentlyViewing.Text = "Currently Viewing: " & cboAPIType.SelectedItem.ToString
                lblFileLocation.Text = "Cache File Location: " & apiReq.LastAPIFileName
            Catch ex As Exception
                MessageBox.Show("There was an error trying to display the requested API. The error was: " & ControlChars.CrLf & ex.Message, "Error Displaying API", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

#Region "Enumerations for API Mappings"

        ''' <summary>
        ''' A list of all available APIs for characters together with official access masks represented as the log of the actual mask
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum CharacterAPI As Integer
            AccountBalances = APITypes.AccountBalancesChar
            AssetList = APITypes.AssetsChar
            CalendarEventAttendees = APITypes.CalendarEventAttendeesChar
            CharacterSheet = APITypes.CharacterSheet
            ContactList = APITypes.ContactListChar
            ContactNotifications = APITypes.ContactNotifications
            FacWarStats = APITypes.FWStatsChar
            IndustryJobs = APITypes.IndustryChar
            KillLog = APITypes.KillLogChar
            MailBodies = APITypes.MailBodies
            MailingLists = APITypes.MailingLists
            MailMessages = APITypes.MailMessages
            MarketOrders = APITypes.OrdersChar
            Medals = APITypes.MedalsReceived
            Notifications = APITypes.Notifications
            NotificationText = APITypes.NotificationTexts
            Research = APITypes.Research
            SkillInTraining = APITypes.SkillTraining
            SkillQueue = APITypes.SkillQueue
            Standings = APITypes.StandingsChar
            UpcomingCalendarEvents = APITypes.UpcomingCalendarEvents
            WalletJournal = APITypes.WalletJournalChar
            WalletTransactions = APITypes.WalletTransChar
            CharacterInfoPrivate = APITypes.CharacterInfo
            CharacterInfoPublic = APITypes.CharacterInfo
            AccountStatus = APITypes.AccountStatus
            Contracts = APITypes.ContractsChar
            ContractItems = APITypes.ContractItemsChar
            ContractBids = APITypes.ContractBidsChar
        End Enum

        ''' <summary>
        ''' A list of all available APIs for corporations together with official access masks represented as the log of the actual mask 
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum CorporateAPI As Integer
            AccountBalances = APITypes.AccountBalancesCorp
            AssetList = APITypes.AssetsCorp
            MemberMedals = APITypes.MemberMedals
            CorporationSheet = APITypes.CorpSheet
            ContactList = APITypes.ContactListCorp
            ContainerLog = APITypes.CorpContainerLog
            FacWarStats = APITypes.FWStatsCorp
            IndustryJobs = APITypes.IndustryCorp
            KillLog = APITypes.KillLogCorp
            MemberSecurity = APITypes.CorpMemberSecurity
            MemberSecurityLog = APITypes.CorpMemberSecurityLog
            MemberTracking = APITypes.CorpMemberTracking
            MarketOrders = APITypes.OrdersCorp
            Medals = APITypes.MedalsAvailable
            OutpostList = APITypes.OutpostList
            OutpostServiceList = APITypes.OutpostServiceDetail
            Shareholders = APITypes.CorpShareholders
            StarbaseDetail = APITypes.POSDetails
            Standings = APITypes.StandingsCorp
            StarbaseList = APITypes.POSList
            WalletJournal = APITypes.WalletJournalCorp
            WalletTransactions = APITypes.WalletTransCorp
            Titles = APITypes.CorpTitles
            Contracts = APITypes.ContractsCorp
            ContractItems = APITypes.ContractItemsCorp
            ContractBids = APITypes.ContractBidsCorp
        End Enum

        ''' <summary>
        ''' A list of all available APIs for an account (that doesn't require an access mask or additional info)
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum AccountAPI As Integer
            APIKeyInfo = APITypes.APIKeyInfo
            Characters = APITypes.Characters
        End Enum

        ''' <summary>
        ''' A list of the "static" APIs available
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum StaticAPI As Integer
            AllianceList = APITypes.AllianceList
            CertificateTree = APITypes.CertificateTree
            CharacterID = APITypes.NameToID
            CharacterInfo = APITypes.CharacterInfo
            CharacterName = APITypes.IDToName
            ConquerableStationList = APITypes.Conquerables
            ErrorList = APITypes.ErrorList
            FacWarStats = APITypes.FWStats
            FacWarTopStats = APITypes.FWTop100
            RefTypes = APITypes.RefTypes
            SkillTree = APITypes.SkillTree
            FacWarSystems = APITypes.FWMap
            Jumps = APITypes.MapJumps
            Kills = APITypes.MapKills
            Sovereignty = APITypes.Sovereignty
            SovereigntyStatus = APITypes.SovereigntyStatus
            ServerStatus = APITypes.ServerStatus
            CallList = APITypes.CallList
        End Enum

#End Region


    End Class
End Namespace