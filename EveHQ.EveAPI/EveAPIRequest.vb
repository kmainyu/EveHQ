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
Imports System
Imports System.Net
Imports System.Text
Imports System.Xml
Imports System.IO

''' <summary>
''' Class for handling requests to an Eve API Server
''' </summary>
''' <remarks></remarks>
Public Class EveAPIRequest

    Private Const ErrorRetries As Integer = 5
    Private Const CCPServerAddress As String = "https://api.eveonline.com"
    Private _cAPILastRequestType As APITypes
    Private _cAPILastResult As APIResults
    Private _cAPILastError As Integer = -1
    Private _cAPILastErrorText As String
    Private _cAPILastFileName As String
    Private _cAPIFileExtension As String = "aspx"
    Private _cAPICacheLocation As String = Path.Combine(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData, "EveAPICache")
    Private _cAPIServerAddress As String = "https://api.eveonline.com"
    Private _cProxyServer As New RemoteProxyServer

    ''' <summary>
    ''' Gets or sets details to use as a web proxy server
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of the RemoteProxyServer class containing details of the proxy to use for web access</returns>
    ''' <remarks></remarks>
    Public Property ProxyServer() As RemoteProxyServer
        Get
            Return _cProxyServer
        End Get
        Set(ByVal value As RemoteProxyServer)
            _cProxyServer = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the server host name or IP address to be used for API requests
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the host name or IP address of the server to be used for API requests</returns>
    ''' <remarks></remarks>
    Public Property APIServerAddress() As String
        Get
            Return _cAPIServerAddress
        End Get
        Set(ByVal value As String)
            If value = "" Then
                _cAPIServerAddress = CCPServerAddress
            Else
                _cAPIServerAddress = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the location of the cached XML files
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the location of the cached XML files</returns>
    ''' <remarks></remarks>
    Public Property APICacheLocation() As String
        Get
            Return _cAPICacheLocation
        End Get
        Set(ByVal value As String)
            If value = "" Then
                _cAPICacheLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveAPICache")
            Else
                _cAPICacheLocation = value
            End If
            ' Try and create the cache location
            Try
                If My.Computer.FileSystem.DirectoryExists(_cAPICacheLocation) = False Then
                    My.Computer.FileSystem.CreateDirectory(_cAPICacheLocation)
                End If
            Catch ex As Exception
                Throw New DirectoryNotFoundException
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the web page extension of API for use with non .Net API servers
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the extension of the web pages used for API requests</returns>
    ''' <remarks></remarks>
    Public Property APIFileExtension() As String
        Get
            Return _cAPIFileExtension
        End Get
        Set(ByVal value As String)
            If value = "" Then
                _cAPIFileExtension = "aspx"
            Else
                _cAPIFileExtension = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the result of the last API request operation
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value containing the result of the last API request operation (or -1 if no request has been made)</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastAPIResult() As APIResults
        Get
            Return _cAPILastResult
        End Get
    End Property

    ''' <summary>
    ''' Gets the error code associated with any error occuring during the API request
    ''' </summary>
    ''' <value></value>
    ''' <returns>An integer containing the error code of the the last API request</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastAPIError() As Integer
        Get
            Return _cAPILastError
        End Get
    End Property

    Public ReadOnly Property LastAPIRequestType() As APITypes
        Get
            Return _cAPILastRequestType
        End Get
    End Property

    ''' <summary>
    ''' Gets the error text associated with any error occuring during the API request
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the error details of the last API request</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastAPIErrorText() As String
        Get
            Return _cAPILastErrorText
        End Get
    End Property

    ''' <summary>
    ''' Gets the location in local storage of where the cached XML file would expect to be found
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the location in local storage of where the cached XML file would expect to be found</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastAPIFileName() As String
        Get
            Return _cAPILastFileName
        End Get
    End Property

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="apiType">The particular type of API to obtain</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal apiType As APITypes, ByVal apiReturnMethod As APIReturnMethods) As XmlDocument
        ' Accepts API features that do not have an explicit post request
        Dim remoteURL As String
        Const postdata As String = ""
        _cAPILastRequestType = apiType
        Select Case apiType
            Case APITypes.AllianceList
                remoteURL = "/eve/AllianceList.xml." & _cAPIFileExtension
            Case APITypes.RefTypes
                remoteURL = "/eve/RefTypes.xml." & _cAPIFileExtension
            Case APITypes.SkillTree
                remoteURL = "/eve/SkillTree.xml." & _cAPIFileExtension
            Case APITypes.Sovereignty
                remoteURL = "/map/Sovereignty.xml." & _cAPIFileExtension
            Case APITypes.SovereigntyStatus
                remoteURL = "/map/SovereigntyStatus.xml." & _cAPIFileExtension
            Case APITypes.MapJumps
                remoteURL = "/map/Jumps.xml." & _cAPIFileExtension
            Case APITypes.MapKills
                remoteURL = "/map/Kills.xml." & _cAPIFileExtension
            Case APITypes.Conquerables
                remoteURL = "/eve/ConquerableStationList.xml." & _cAPIFileExtension
            Case APITypes.ErrorList
                remoteURL = "/eve/ErrorList.xml." & _cAPIFileExtension
            Case APITypes.FWStats
                remoteURL = "/eve/FacWarStats.xml." & _cAPIFileExtension
            Case APITypes.FWTop100
                remoteURL = "/eve/FacWarTopStats.xml." & _cAPIFileExtension
            Case APITypes.FWMap
                remoteURL = "/map/FacWarSystems.xml." & _cAPIFileExtension
            Case APITypes.ServerStatus
                remoteURL = "/server/ServerStatus.xml." & _cAPIFileExtension
            Case APITypes.CertificateTree
                remoteURL = "/eve/CertificateTree.xml." & _cAPIFileExtension
            Case APITypes.CallList
                remoteURL = "/api/CallList.xml." & _cAPIFileExtension
            Case Else
                _cAPILastResult = APIResults.InvalidFeature
                Return Nothing
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), apiType)
        Return GetXML(remoteURL, postdata, fileName, apiReturnMethod)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="apiType">The particular type of API to obtain</param>
    ''' <param name="data">The information to be converted to an ID or Name</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal apiType As APITypes, ByVal data As String, ByVal apiReturnMethod As APIReturnMethods) As XmlDocument
        ' Accepts API features that do not have an explicit post request
        Dim remoteURL As String
        Dim postdata As String
        _cAPILastRequestType = apiType
        Select Case apiType
            Case APITypes.NameToID
                postdata = "names=" & data
                remoteURL = "/eve/CharacterID.xml." & _cAPIFileExtension
            Case APITypes.IDToName
                postdata = "ids=" & data
                remoteURL = "/eve/CharacterName.xml." & _cAPIFileExtension
            Case APITypes.CharacterInfo
                postdata = "characterid=" & data
                remoteURL = "/eve/CharacterInfo.xml." & _cAPIFileExtension
            Case Else
                _cAPILastResult = APIResults.InvalidFeature
                Return Nothing
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), apiType) & "_" & Format(Now, "yyyyMMddhhmmssfffff")
        Return GetXML(remoteURL, postdata, fileName, apiReturnMethod)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="apiType">The particular type of API to obtain</param>
    ''' <param name="apiAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    <Obsolete>
        Dim remoteURL As String
        Dim postdata As String
        postdata = "keyID=" & APIAccount.UserID & "&vCode=" & APIAccount.APIKey
        _cAPILastRequestType = apiType
        Select Case apiType
            Case APITypes.Characters
                remoteURL = "/account/Characters.xml." & _cAPIFileExtension
            Case APITypes.AccountStatus
                remoteURL = "/account/AccountStatus.xml." & _cAPIFileExtension
            Case APITypes.APIKeyInfo
                remoteURL = "/account/APIKeyInfo.xml." & _cAPIFileExtension
            Case Else
                _cAPILastResult = APIResults.InvalidFeature
                Return Nothing
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), apiType) & "_" & APIAccount.UserID
        Return GetXML(remoteURL, postdata, fileName, apiReturnMethod)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="apiType">The particular type of API to obtain</param>
    ''' <param name="apiAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal apiType As APITypes, ByVal apiAccount As EveAPIAccount, ByVal charID As String, ByVal apiReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String
        Dim postdata As String
        postdata = "keyID=" & apiAccount.UserID & "&vCode=" & apiAccount.APIKey & "&characterID=" & charID
        _cAPILastRequestType = apiType
        Select Case apiType
            Case APITypes.AccountBalancesChar
                remoteURL = "/char/AccountBalance.xml." & _cAPIFileExtension
            Case APITypes.AccountBalancesCorp
                remoteURL = "/corp/AccountBalance.xml." & _cAPIFileExtension
            Case APITypes.CharacterSheet
                remoteURL = "/char/CharacterSheet.xml." & _cAPIFileExtension
            Case APITypes.CorpSheet
                remoteURL = "/corp/CorporationSheet.xml." & _cAPIFileExtension
            Case APITypes.CorpMemberTracking
                remoteURL = "/corp/MemberTracking.xml." & _cAPIFileExtension
            Case APITypes.SkillTraining
                remoteURL = "/char/SkillInTraining.xml." & _cAPIFileExtension
            Case APITypes.SkillQueue
                remoteURL = "/char/SkillQueue.xml." & _cAPIFileExtension
            Case APITypes.AssetsChar
                remoteURL = "/char/AssetList.xml." & _cAPIFileExtension
            Case APITypes.AssetsCorp
                remoteURL = "/corp/AssetList.xml." & _cAPIFileExtension
            Case APITypes.IndustryChar
                remoteURL = "/char/IndustryJobs.xml." & _cAPIFileExtension
            Case APITypes.IndustryCorp
                remoteURL = "/corp/IndustryJobs.xml." & _cAPIFileExtension
            Case APITypes.OrdersChar
                remoteURL = "/char/MarketOrders.xml." & _cAPIFileExtension
            Case APITypes.OrdersCorp
                remoteURL = "/corp/MarketOrders.xml." & _cAPIFileExtension
            Case APITypes.POSList
                remoteURL = "/corp/StarbaseList.xml." & _cAPIFileExtension
            Case APITypes.OutpostList
                remoteURL = "/corp/OutpostList.xml." & _cAPIFileExtension
            Case APITypes.StandingsChar
                remoteURL = "/char/Standings.xml." & _cAPIFileExtension
            Case APITypes.StandingsCorp
                remoteURL = "/corp/Standings.xml." & _cAPIFileExtension
            Case APITypes.CorpMemberSecurity
                remoteURL = "/corp/MemberSecurity.xml." & _cAPIFileExtension
            Case APITypes.CorpMemberSecurityLog
                remoteURL = "/corp/MemberSecurityLog.xml." & _cAPIFileExtension
            Case APITypes.CorpShareholders
                remoteURL = "/corp/Shareholders.xml." & _cAPIFileExtension
            Case APITypes.CorpTitles
                remoteURL = "/corp/Titles.xml." & _cAPIFileExtension
            Case APITypes.FWStatsChar
                remoteURL = "/char/FacWarStats.xml." & _cAPIFileExtension
            Case APITypes.FWStatsCorp
                remoteURL = "/corp/FacWarStats.xml." & _cAPIFileExtension
            Case APITypes.MedalsReceived
                remoteURL = "/char/Medals.xml." & _cAPIFileExtension
            Case APITypes.MedalsAvailable
                remoteURL = "/corp/Medals.xml." & _cAPIFileExtension
            Case APITypes.MemberMedals
                remoteURL = "/corp/MemberMedals.xml." & _cAPIFileExtension
            Case APITypes.MailMessages
                remoteURL = "/char/MailMessages.xml." & _cAPIFileExtension
            Case APITypes.Notifications
                remoteURL = "/char/Notifications.xml." & _cAPIFileExtension
            Case APITypes.MailingLists
                remoteURL = "/char/MailingLists.xml." & _cAPIFileExtension
            Case APITypes.Research
                remoteURL = "/char/Research.xml." & _cAPIFileExtension
            Case APITypes.CharacterInfo
                remoteURL = "/eve/CharacterInfo.xml." & _cAPIFileExtension
            Case APITypes.ContactListChar
                remoteURL = "/char/ContactList.xml." & _cAPIFileExtension
            Case APITypes.ContactListCorp
                remoteURL = "/corp/ContactList.xml." & _cAPIFileExtension
            Case APITypes.ContractsChar
                remoteURL = "/char/Contracts.xml." & _cAPIFileExtension
            Case APITypes.ContractsCorp
                remoteURL = "/corp/Contracts.xml." & _cAPIFileExtension
            Case APITypes.ContractBidsChar
                remoteURL = "/char/ContractBids.xml." & _cAPIFileExtension
            Case APITypes.ContractBidsCorp
                remoteURL = "/corp/ContractBids.xml." & _cAPIFileExtension
            Case APITypes.ContactNotifications
                remoteURL = "/char/ContactNotifications.xml." & _cAPIFileExtension
            Case APITypes.UpcomingCalendarEvents
                remoteURL = "/char/UpcomingCalendarEvents.xml." & _cAPIFileExtension
            Case Else
                _cAPILastResult = APIResults.InvalidFeature
                Return Nothing
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), apiType) & "_" & apiAccount.UserID & "_" & charID
        Return GetXML(remoteURL, postdata, fileName, apiReturnMethod)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="apiType">The particular type of API to obtain</param>
    ''' <param name="apiAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="itemID">The itemID of the starbase (POS) to query</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal apiType As APITypes, ByVal apiAccount As EveAPIAccount, ByVal charID As String, ByVal itemID As Long, ByVal apiReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String
        Dim postdata As String
        postdata = "keyID=" & apiAccount.UserID & "&vCode=" & apiAccount.APIKey & "&characterID=" & charID & "&itemID=" & itemID
        _cAPILastRequestType = apiType
        Select Case apiType
            Case APITypes.POSDetails
                remoteURL = "/corp/StarbaseDetail.xml." & _cAPIFileExtension
            Case APITypes.OutpostServiceDetail
                remoteURL = "/corp/OutpostServiceDetail.xml." & _cAPIFileExtension
            Case APITypes.ContractItemsChar
                postdata = postdata.Replace("&itemID", "&contractID")
                remoteURL = "/char/ContractItems.xml." & _cAPIFileExtension
            Case APITypes.ContractItemsCorp
                postdata = postdata.Replace("&itemID", "&contractID")
                remoteURL = "/corp/ContractItems.xml." & _cAPIFileExtension
            Case Else
                _cAPILastResult = APIResults.InvalidFeature
                Return Nothing
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), apiType) & "_" & apiAccount.UserID & "_" & charID & "_" & itemID
        Return GetXML(remoteURL, postdata, fileName, apiReturnMethod)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="apiType">The particular type of API to obtain</param>
    ''' <param name="apiAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="IDs">The additional Eve data with which to use with the request</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal apiType As APITypes, ByVal apiAccount As EveAPIAccount, ByVal charID As String, ByVal ids As String, ByVal apiReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String
        Dim postdata As String
        postdata = "keyID=" & apiAccount.UserID & "&vCode=" & apiAccount.APIKey & "&characterID=" & charID
        _cAPILastRequestType = apiType
        If ids <> "" Then
            Select Case apiType
                Case APITypes.KillLogChar, APITypes.KillLogCorp
                    postdata &= "&beforeKillID=" & ids
                Case APITypes.MailBodies, APITypes.NotificationTexts
                    postdata &= "&ids=" & ids
                Case APITypes.CalendarEventAttendeesChar
                    postdata &= "eventIDs=" & ids
            End Select
        End If
        Select Case apiType
            Case APITypes.KillLogChar
                remoteURL = "/char/Killlog.xml." & _cAPIFileExtension
            Case APITypes.KillLogCorp
                remoteURL = "/corp/Killlog.xml." & _cAPIFileExtension
            Case APITypes.MailBodies
                remoteURL = "/char/MailBodies.xml." & _cAPIFileExtension
            Case APITypes.NotificationTexts
                remoteURL = "/char/NotificationTexts.xml." & _cAPIFileExtension
            Case APITypes.CalendarEventAttendeesChar
                remoteURL = "/char/CalendarEventAttendees.xml." & _cAPIFileExtension
            Case Else
                _cAPILastResult = APIResults.InvalidFeature
                Return Nothing
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), apiType) & "_" & apiAccount.UserID & "_" & charID
        If ids <> "" Then
            Select Case apiType
                Case APITypes.KillLogChar, APITypes.KillLogCorp
                    fileName &= "_" & ids
                Case APITypes.MailBodies, APITypes.NotificationTexts
                    fileName &= "_" & Format(Now, "yyyyMMddhhmmssfffff")
            End Select
        End If
        Return GetXML(remoteURL, postdata, fileName, apiReturnMethod)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="apiType">The particular type of API to obtain</param>
    ''' <param name="apiAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="accountKey">The specific wallet accountID to query</param>
    ''' <param name="beforeRefID">The Eve data reference from which to start the request</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal apiType As APITypes, ByVal apiAccount As EveAPIAccount, ByVal charID As String, ByVal accountKey As Integer, ByVal beforeRefID As String, ByVal apiReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String
        Dim postdata As String
        postdata = "keyID=" & apiAccount.UserID & "&vCode=" & apiAccount.APIKey & "&characterID=" & charID & "&accountKey=" & accountKey
        _cAPILastRequestType = apiType
        If beforeRefID <> "" Then
            Select Case apiType
                Case APITypes.WalletTransChar, APITypes.WalletTransCorp
                    postdata &= "&beforeTransID=" & beforeRefID
            End Select
        End If
        Select Case apiType
            Case APITypes.WalletTransChar
                remoteURL = "/char/WalletTransactions.xml." & _cAPIFileExtension
            Case APITypes.WalletTransCorp
                remoteURL = "/corp/WalletTransactions.xml." & _cAPIFileExtension
            Case Else
                _cAPILastResult = APIResults.InvalidFeature
                Return Nothing
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), apiType) & "_" & apiAccount.UserID & "_" & charID & "_" & accountKey
        If beforeRefID <> "" Then
            fileName &= "_" & beforeRefID
        End If
        Return GetXML(remoteURL, postdata, fileName, apiReturnMethod)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs 
    ''' </summary>
    ''' <param name="apiType">The particular type of API to obtain</param>
    ''' <param name="apiAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="accountKey">The specific wallet accountID to query</param>
    ''' <param name="fromID">The Eve data reference from which to start the request</param>
    ''' <param name="rowCount">The number of rows to return (max 256)</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal apiType As APITypes, ByVal apiAccount As EveAPIAccount, ByVal charID As String, ByVal accountKey As Integer, ByVal fromID As Long, ByVal rowCount As Integer, ByVal apiReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String
        Dim postdata As String
        postdata = "keyID=" & apiAccount.UserID & "&vCode=" & apiAccount.APIKey & "&characterID=" & charID & "&accountKey=" & accountKey
        _cAPILastRequestType = apiType
        If fromID <> 0 Then
            Select Case apiType
                Case APITypes.WalletJournalChar, APITypes.WalletJournalCorp
                    postdata &= "&fromID=" & fromID.ToString
            End Select
        End If
        postdata &= "&rowCount=" & rowCount
        Select Case apiType
            Case APITypes.WalletJournalChar
                remoteURL = "/char/WalletJournal.xml." & _cAPIFileExtension
            Case APITypes.WalletJournalCorp
                remoteURL = "/corp/WalletJournal.xml." & _cAPIFileExtension
            Case Else
                _cAPILastResult = APIResults.InvalidFeature
                Return Nothing
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), apiType) & "_" & apiAccount.UserID & "_" & charID & "_" & accountKey & "_" & fromID
        Return GetXML(remoteURL, postdata, fileName, apiReturnMethod)
    End Function

    ''' <summary>
    ''' Function to return an XML from an API Request
    ''' </summary>
    ''' <param name="remoteURL">The URL of the API page where the XML will be obtained</param>
    ''' <param name="postData">Relevant data to be passed with the URL</param>
    ''' <param name="fileName">The location in local storage of the cached XML to save and/or retrieve</param>
    ''' <param name="apiReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument for the APIRequest, based upon the APIReturnMethod used</returns>
    ''' <remarks></remarks>
    Private Function GetXML(ByVal remoteURL As String, ByVal postData As String, ByVal fileName As String, ByVal apiReturnMethod As APIReturnMethods) As XmlDocument
        Const fileDate As String = ""
        Dim apixml As New XmlDocument
        Dim errlist As XmlNodeList
        Dim fileLoc As String
        Try
            fileLoc = Path.Combine(_cAPICacheLocation, fileName & fileDate & ".xml")
        Catch e As Exception
            Dim msg As String = "An error occured while trying to assemble the cache location string. The location being created should be in:" & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Cache Folder: " & _cAPICacheLocation & ControlChars.CrLf
            msg &= "File Name: " & fileName & ControlChars.CrLf
            msg &= "File Date: " & fileDate & ControlChars.CrLf
            _cAPILastResult = APIResults.InternalCodeError
            _cAPILastErrorText = msg
            Return Nothing
        End Try
        Try
            ' See if we need to bypass the cache
            If apiReturnMethod = APIReturnMethods.BypassCache Then
                ' Perform this section if the cache is bypassed
                ' Fetch the XML from the EveAPI
                apixml = FetchXMLFromWeb(remoteURL, postData)
                ' Check for null document (can happen if APIRS) isn't active and no backup is used
                If apixml.InnerXml = "" Then
                    ' Do not save and return nothing
                    _cAPILastResult = APIResults.APIServerDownReturnedNull
                    Return Nothing
                Else
                    ' Check for error codes
                    errlist = apixml.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Try
                            Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                            Dim errMsg As String = errNode.InnerText
                            ' Return the current XML regardless
                            _cAPILastResult = APIResults.ReturnedActual
                            _cAPILastError = CInt(errCode)
                            _cAPILastErrorText = errMsg
                            Return apixml
                        Catch e As Exception
                            ' Return nothing and report a general error - usually as a result of a API Server error
                            _cAPILastResult = APIResults.UnknownError
                            _cAPILastError = 0
                            _cAPILastErrorText = "A General Error occurred"
                            Return Nothing
                        End Try
                    Else
                        _cAPILastResult = APIResults.ReturnedActual
                        Return apixml
                    End If
                End If
            Else
                ' Check if the file already exists
                _cAPILastFileName = fileLoc
                If My.Computer.FileSystem.FileExists(fileLoc) = True Then
                    Dim tmpApixml As XmlDocument
                    ' Check cache time of file
                    Dim failedCacheLoad As Boolean = False
                    Try
                        apixml.Load(fileLoc)
                    Catch e As Exception
                        failedCacheLoad = True
                        ' Attempt to get a new XML and save
                        apixml = FetchXMLFromWeb(remoteURL, postData)
                        ' Try to save the XML file
                        Call SaveAPIXML(apixml, fileLoc)
                    End Try
                    ' Get Cache time details
                    Dim cacheDetails As XmlNodeList = apixml.SelectNodes("/eveapi")
                    Dim cacheTime As DateTime = CDate(cacheDetails(0).ChildNodes(2).InnerText)
                    Dim localCacheTime As Date = ConvertEveTimeToLocal(cacheTime)
                    ' Has Cache expired?
                    If (localCacheTime > Now Or apiReturnMethod = APIReturnMethods.ReturnCacheOnly) And failedCacheLoad = False Then
                        '  Cache has not expired or a request to return cached version- return existing XML
                        _cAPILastResult = APIResults.ReturnedCached
                        Return apixml
                    Else
                        ' Cache has expired - get a new XML
                        tmpApixml = FetchXMLFromWeb(remoteURL, postData)
                        ' Check for null document (can happen if APIRS) isn't active and no backup is used
                        If tmpApixml.InnerXml = "" Then
                            ' Do not save and return the old API file
                            _cAPILastResult = APIResults.APIServerDownReturnedCached
                            Return apixml
                        End If
                        ' Check for error codes
                        errlist = tmpApixml.SelectNodes("/eveapi/error")
                        If errlist.Count <> 0 Then
                            Dim errNode As XmlNode = errlist(0)
                            If errlist(0).Attributes.GetNamedItem("code") IsNot Nothing Then
                                ' Get error code
                                Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                                Dim errMsg As String = errNode.InnerText
                                If apiReturnMethod = APIReturnMethods.ReturnStandard Then
                                    ' Return the old XML file but report the error
                                    _cAPILastResult = APIResults.CCPError
                                    _cAPILastError = CInt(errCode)
                                    _cAPILastErrorText = errMsg
                                    Return apixml
                                Else
                                    ' Return the current one regardless
                                    _cAPILastResult = APIResults.ReturnedActual
                                    _cAPILastError = CInt(errCode)
                                    _cAPILastErrorText = errMsg
                                    ' Try to save the XML file
                                    Call SaveAPIXML(tmpApixml, fileLoc)
                                    Return tmpApixml
                                End If
                            Else
                                ' Return the old XML file but report a general error - usually as a result of a API Server error
                                _cAPILastResult = APIResults.UnknownError
                                _cAPILastError = 999
                                _cAPILastErrorText = "A General Error occurred. " & errNode.InnerText
                                Return apixml
                            End If
                        Else
                            ' No error codes so save, then return new XML file
                            _cAPILastResult = APIResults.ReturnedNew
                            ' Try to save the XML file
                            Call SaveAPIXML(tmpApixml, fileLoc)
                            Return tmpApixml
                        End If
                    End If
                Else
                    ' Do this if a file does not exist
                    If apiReturnMethod = APIReturnMethods.ReturnCacheOnly Then
                        ' If we demand that a cached fle be returned, return nothing as the file does not exist
                        Return Nothing
                    Else
                        ' Fetch the XML from the EveAPI
                        apixml = FetchXMLFromWeb(remoteURL, postData)
                        ' Check for null document (can happen if APIRS) isn't active and no backup is used
                        If apixml.InnerXml = "" And (_cAPILastResult = APIResults.ReturnedNew Or _cAPILastResult = APIResults.ReturnedCached Or _cAPILastResult = APIResults.ReturnedActual) Then
                            ' Do not save and return nothing
                            _cAPILastResult = APIResults.APIServerDownReturnedNull
                            Return Nothing
                        Else
                            ' Check for error codes
                            errlist = apixml.SelectNodes("/eveapi/error")
                            If errlist.Count <> 0 Then
                                Dim errNode As XmlNode = errlist(0)
                                ' Get error code
                                Try
                                    Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                                    Dim errMsg As String = errNode.InnerText
                                    If apiReturnMethod = APIReturnMethods.ReturnStandard Then
                                        ' Return the file but don't save it as we have an error
                                        _cAPILastResult = APIResults.CCPError
                                        _cAPILastError = CInt(errCode)
                                        _cAPILastErrorText = errMsg
                                        Return apixml
                                    Else
                                        ' Return the current one regardless
                                        _cAPILastResult = APIResults.ReturnedActual
                                        _cAPILastError = CInt(errCode)
                                        _cAPILastErrorText = errMsg
                                        ' Try to save the XML file
                                        Call SaveAPIXML(apixml, fileLoc)
                                        Return apixml
                                    End If
                                Catch e As Exception
                                    ' Return nothing and report a general error - usually as a result of a API Server error
                                    _cAPILastResult = APIResults.UnknownError
                                    _cAPILastError = 0
                                    _cAPILastErrorText = "A General Error occurred"
                                    Return Nothing
                                End Try
                            Else
                                ' Try to save the XML file
                                Call SaveAPIXML(apixml, fileLoc)
                                _cAPILastResult = APIResults.ReturnedNew
                                Return apixml
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            ' Something happened so let's return nothing and let the calling routine handle it
            _cAPILastResult = APIResults.InternalCodeError
            _cAPILastErrorText = ex.Message
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Function to obtain an XML file from the remote API Server
    ''' </summary>
    ''' <param name="remoteURL">The URL of the API page where the XML will be obtained</param>
    ''' <param name="postData">Relevant data to be passed with the URL</param>
    ''' <returns>An XMLDocument containing the response from the API Server</returns>
    ''' <remarks></remarks>
    Private Function FetchXMLFromWeb(ByVal remoteURL As String, ByVal postData As String) As XmlDocument
        Dim apiServer As String = _cAPIServerAddress
        remoteURL = apiServer & remoteURL
        Dim webdata As String
        Dim apixml As New XmlDocument
        Try
            ' Create the requester
            ServicePointManager.DefaultConnectionLimit = 20
            ServicePointManager.Expect100Continue = False
            ServicePointManager.FindServicePoint(New Uri(remoteURL))
            Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
            ' Setup proxy server (if required)
            If ProxyServer IsNot Nothing Then
                If ProxyServer.ProxyRequired = True Then
                    request.Proxy = ProxyServer.SetupWebProxy
                End If
            End If
            ' Setup request parameters
            request.Method = "POST"
            request.ContentLength = postData.Length
            request.ContentType = "application/x-www-form-urlencoded"
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
            ' Setup a stream to write the HTTP "POST" data
            Dim webEncoding As New ASCIIEncoding()
            Dim byte1 As Byte() = webEncoding.GetBytes(postData)
            Dim newStream As Stream = request.GetRequestStream()
            newStream.Write(byte1, 0, byte1.Length)
            newStream.Close()
            newStream.Dispose()
            ' Prepare for a response from the server
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Get the stream associated with the response.
            Dim receiveStream As Stream = response.GetResponseStream()
            ' Pipes the stream to a higher level stream reader with the required encoding format. 
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            webdata = readStream.ReadToEnd()
            response.Close()
            receiveStream.Close()
            receiveStream.Dispose()
            readStream.Close()
            readStream.Dispose()
            ' Check response string for any error codes?
            apixml.LoadXml(webdata)
            Dim errlist As XmlNodeList = apixml.SelectNodes("/eveapi/error")
            If errlist.Count <> 0 Then
                Dim errNode As XmlNode = errlist(0)
                ' Get error code
                If errNode.Attributes.GetNamedItem("code") IsNot Nothing Then
                    Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                    Dim errMsg As String = errNode.InnerText
                    _cAPILastResult = APIResults.CCPError
                    _cAPILastError = CInt(errCode)
                    _cAPILastErrorText = errMsg
                Else
                    _cAPILastResult = APIResults.UnknownError
                    _cAPILastError = 999
                    _cAPILastErrorText = errNode.InnerText
                End If
            Else
                ' Result will be given in the calling sub
            End If
        Catch webEx As WebException
            'Quantix: The EveAPI now uses HTTP status messages for errors instead of 200 OK plus error details in xml.
            ' however there is no documentation at the moment about what error codes are used for which scenarios.
            _cAPILastResult = APIResults.CCPError
            Dim errorDetails As HttpWebResponse = CType(webEx.Response, HttpWebResponse)
            _cAPILastError = CInt(errorDetails.StatusCode)
            _cAPILastErrorText = CStr(errorDetails.StatusCode)
        Catch e As Exception
            If e.Message.Contains("timed out") = True Then
                _cAPILastResult = APIResults.TimedOut
                _cAPILastError = 0
                _cAPILastErrorText = e.Message
            Else
                _cAPILastResult = APIResults.UnknownError
                _cAPILastError = 0
                _cAPILastErrorText = e.Message
            End If
        End Try
        Return apixml
    End Function

    ''' <summary>
    ''' Routine for saving an XMLDocument to the Cache
    ''' </summary>
    ''' <param name="apixml">The XMLDocument to save</param>
    ''' <param name="fileLoc">The location in local storage to save the XMLDocument</param>
    ''' <remarks></remarks>
    Private Sub SaveAPIXML(ByRef apixml As XmlDocument, ByVal fileLoc As String)
        Dim fileSaved As Boolean = False
        Dim currentAttempt As Integer = 0
        Do
            Try
                currentAttempt += 1
                apixml.Save(fileLoc)
                fileSaved = True
            Catch e As Exception
                ' Failed save, presumably due to conflicting access
            End Try
        Loop Until fileSaved = True Or currentAttempt >= ErrorRetries
    End Sub

    ''' <summary>
    ''' Converts EveTime to the user's local time
    ''' </summary>
    ''' <param name="eveTime">The time of the Eve Server</param>
    ''' <returns>The local time equivalent of Eve Time</returns>
    ''' <remarks></remarks>
    Private Function ConvertEveTimeToLocal(ByVal eveTime As Date) As Date
        ' Calculate the local time and UTC offset.
        Return TimeZone.CurrentTimeZone.ToLocalTime(EveTime)
    End Function


#Region "Class Initialisers"

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Uses the standard CCP server address and file format with no proxy server
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        APIServerAddress = ""
        ProxyServer = Nothing
        APIFileExtension = ""
        APICacheLocation = ""
        _cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server
    ''' </summary>
    ''' <param name="serverInfo">The API server details used for access to the API</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal serverInfo As APIServerInfo)
        If ServerInfo.UseAPIRS = True Then
            APIServerAddress = ServerInfo.APIRSServer
        Else
            APIServerAddress = ServerInfo.CCPServer
        End If
        ProxyServer = Nothing
        APIFileExtension = ""
        APICacheLocation = ""
        _cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server together with a Proxy Server
    ''' </summary>
    ''' <param name="serverInfo">The API server details used for access to the API</param>
    ''' <param name="proxyDetails">An instance of the RemoteProxyServer class containing Proxy Server details</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal serverInfo As APIServerInfo, ByVal proxyDetails As RemoteProxyServer)
        If serverInfo.UseAPIRS = True Then
            APIServerAddress = serverInfo.APIRSServer
        Else
            APIServerAddress = serverInfo.CCPServer
        End If
        ProxyServer = proxyDetails
        APIFileExtension = ""
        APICacheLocation = ""
        _cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server together with an alternative web page extension for non .Net servers
    ''' </summary>
    ''' <param name="serverInfo">The API server details used for access to the API</param>
    ''' <param name="fileExtension">The web page extension to use for API Requests</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal serverInfo As APIServerInfo, ByVal fileExtension As String)
        If serverInfo.UseAPIRS = True Then
            APIServerAddress = serverInfo.APIRSServer
        Else
            APIServerAddress = serverInfo.CCPServer
        End If
        ProxyServer = Nothing
        APIFileExtension = fileExtension
        APICacheLocation = ""
        _cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server and Proxy Server, together with an alternative web page extension for non .Net servers
    ''' </summary>
    ''' <param name="serverInfo">The API server details used for access to the API</param>
    ''' <param name="proxyDetails">An instance of the RemoteProxyServer class containing Proxy Server details</param>
    ''' <param name="fileExtension">The web page extension to use for API Requests</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal serverInfo As APIServerInfo, ByVal proxyDetails As RemoteProxyServer, ByVal fileExtension As String)
        If serverInfo.UseAPIRS = True Then
            APIServerAddress = serverInfo.APIRSServer
        Else
            APIServerAddress = serverInfo.CCPServer
        End If
        ProxyServer = proxyDetails
        APIFileExtension = FileExtension
        APICacheLocation = ""
        _cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server and Cache Location, together with an alternative web page extension for non .Net servers
    ''' </summary>
    ''' <param name="serverInfo">The API server details used for access to the API</param>
    ''' <param name="fileExtension">The web page extension to use for API Requests</param>
    ''' <param name="cacheLocation">The location in local storage of where the cached API files are maintained</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal serverInfo As APIServerInfo, ByVal fileExtension As String, ByVal cacheLocation As String)
        If serverInfo.UseAPIRS = True Then
            APIServerAddress = serverInfo.APIRSServer
        Else
            APIServerAddress = serverInfo.CCPServer
        End If
        ProxyServer = Nothing
        APIFileExtension = fileExtension
        APICacheLocation = CacheLocation
        _cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows full configuration of the EveAPIRequest
    ''' </summary>
    ''' <param name="serverInfo">The API server details used for access to the API</param>
    ''' <param name="proxyDetails">An instance of the RemoteProxyServer class containing Proxy Server details</param>
    ''' <param name="fileExtension">The web page extension to use for API Requests</param>
    ''' <param name="cacheLocation">The location in local storage of where the cached API files are maintained</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal serverInfo As APIServerInfo, ByVal proxyDetails As RemoteProxyServer, ByVal fileExtension As String, ByVal cacheLocation As String)
        If serverInfo.UseAPIRS = True Then
            APIServerAddress = serverInfo.APIRSServer
        Else
            APIServerAddress = serverInfo.CCPServer
        End If
        ProxyServer = proxyDetails
        APIFileExtension = fileExtension
        APICacheLocation = CacheLocation
        _cAPILastResult = APIResults.NotYetProcessed
    End Sub

#End Region

End Class

''' <summary>
''' Class for holding server information to pass to an EveAPIRequest
''' </summary>
''' <remarks></remarks>
Public Class APIServerInfo

    ''' <summary>
    ''' Gets or sets the location of the CCP API Server to use
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the URL of the CCP API server</returns>
    ''' <remarks></remarks>
    Public Property CCPServer As String

    ''' <summary>
    ''' Gets or sets the location of the custom API Relay Server
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the URL of the custom API Relay Server</returns>
    ''' <remarks></remarks>
    Public Property APIRSServer As String

    ''' <summary>
    ''' Gets of sets whether a custom API Relay Server should be used in place of the CCP API Server
    ''' </summary>
    ''' <value></value>
    ''' <returns>A boolean value indicating whether a custom API Relay Server is being used</returns>
    ''' <remarks></remarks>
    Public Property UseAPIRS As Boolean

    ''' <summary>
    ''' Gets or sets whether the CCP API should be used as a backup
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UseCcpapiBackup As Boolean

    Public Sub New()
        CCPServer = ""
        APIRSServer = ""
        UseAPIRS = False
        UseCCPAPIBackup = False
    End Sub

    ''' <summary>
    ''' Creates a new instance of the APIServerInfo class to pass to an EveAPIRequest
    ''' </summary>
    ''' <param name="ccpServerAddress">URL of the CCP API Server</param>
    ''' <param name="apirsAddress">URL of the custom API Relay Server</param>
    ''' <param name="useAPIRelayServer">Set to true if using the API Relay Server</param>
    ''' <param name="useCCPServerForBackup">Set to true if using the CCP API Server as a backup</param>
    ''' <remarks></remarks>
    Public Sub New(ccpServerAddress As String, apirsAddress As String, useAPIRelayServer As Boolean, useCCPServerForBackup As Boolean)
        CCPServer = ccpServerAddress
        APIRSServer = apirsAddress
        UseAPIRS = useAPIRelayServer
        UseCCPAPIBackup = UseCCPServerForBackup
    End Sub

End Class

''' <summary>
''' A list of available API types that can be obtained from API Servers
''' </summary>
''' <remarks></remarks>
Public Enum APITypes As Integer
    SkillTree = 0
    RefTypes = 1
    AllianceList = 2
    Sovereignty = 3
    Characters = 4
    CharacterSheet = 5
    SkillTraining = 6
    WalletJournalChar = 7
    WalletJournalCorp = 8
    WalletTransChar = 9
    WalletTransCorp = 10
    AccountBalancesChar = 11
    AccountBalancesCorp = 12
    CorpMemberTracking = 13
    AssetsChar = 14
    AssetsCorp = 15
    KillLogChar = 16
    KillLogCorp = 17
    Conquerables = 18
    CorpSheet = 19
    ErrorList = 20
    IndustryChar = 21
    IndustryCorp = 22
    OrdersChar = 23
    OrdersCorp = 24
    MapJumps = 25
    MapKills = 26
    NameToID = 27
    IDToName = 28
    POSList = 29
    POSDetails = 30
    StandingsChar = 31
    StandingsCorp = 32
    'StandingsAlliance = 33
    CorpContainerLog = 34
    CorpTitles = 35
    CorpMemberSecurity = 36
    CorpMemberSecurityLog = 37
    CorpShareholders = 38
    FWStatsChar = 39
    FWStatsCorp = 40
    FWTop100 = 41
    FWMap = 42
    ServerStatus = 43
    CertificateTree = 44
    MedalsReceived = 45
    MedalsAvailable = 46
    MemberMedals = 47
    SkillQueue = 48
    SovereigntyStatus = 49
    MailMessages = 50
    Notifications = 51
    MailingLists = 52
    MailBodies = 53
    Research = 54
    AccountStatus = 55
    CalendarEventAttendeesChar = 56
    'CalendarEventAttendeesCorp = 57
    UpcomingCalendarEvents = 58
    CharacterInfo = 59
    OutpostList = 60
    OutpostServiceDetail = 61
    ContactListChar = 62
    ContactListCorp = 63
    APIKeyInfo = 64
    ContractsChar = 65
    ContractsCorp = 66
    ContractItemsChar = 67
    ContractItemsCorp = 68
    ContractBidsChar = 69
    ContractBidsCorp = 70
    NotificationTexts = 71
    ContactNotifications = 72
    FWStats = 73
    CallList = 74
End Enum

''' <summary>
''' A list of all available APIs for characters together with official access masks represented as the log of the actual mask
''' </summary>
''' <remarks></remarks>
Public Enum CharacterAccessMasks As Long
    AccountBalances = 0
    AssetList = 1
    CalendarEventAttendees = 2
    CharacterSheet = 3
    ContactList = 4
    ContactNotifications = 5
    FacWarStats = 6
    IndustryJobs = 7
    KillLog = 8
    MailBodies = 9
    MailingLists = 10
    MailMessages = 11
    MarketOrders = 12
    Medals = 13
    Notifications = 14
    NotificationText = 15
    Research = 16
    SkillInTraining = 17
    SkillQueue = 18
    Standings = 19
    UpcomingCalendarEvents = 20
    WalletJournal = 21
    WalletTransactions = 22
    CharacterInfoPrivate = 23
    CharacterInfoPublic = 24
    AccountStatus = 25
    Contracts = 26
End Enum

''' <summary>
''' A list of all available APIs for corporations together with official access masks represented as the log of the actual mask 
''' </summary>
''' <remarks></remarks>
Public Enum CorporateAccessMasks As Long
    AccountBalances = 0
    AssetList = 1
    MemberMedals = 2
    CorporationSheet = 3
    ContactList = 4
    ContainerLog = 5
    FacWarStats = 6
    IndustryJobs = 7
    KillLog = 8
    MemberSecurity = 9
    MemberSecurityLog = 10
    MemberTracking = 11
    MarketOrders = 12
    Medals = 13
    OutpostList = 14
    OutpostServiceList = 15
    Shareholders = 16
    StarbaseDetail = 17
    Standings = 18
    StarbaseList = 19
    WalletJournal = 20
    WalletTransactions = 21
    Titles = 22
    Contracts = 23
End Enum

''' <summary>
''' A list of status codes as a result of processing an API Request
''' Defaults to NotYetProcessed on initialising a new APIRequest
''' </summary>
''' <remarks></remarks>
Public Enum APIResults As Integer
    ''' <summary>
    ''' The API Request has not yet been made
    ''' </summary>
    ''' <remarks></remarks>
    NotYetProcessed = -1
    ''' <summary>
    ''' A new XML file has been returned
    ''' </summary>
    ''' <remarks></remarks>
    ReturnedNew = 0
    ''' <summary>
    ''' A cached XML file has been returned
    ''' </summary>
    ''' <remarks></remarks>
    ReturnedCached = 1
    ''' <summary>
    ''' The specific page requested could not be found on the API Server
    ''' </summary>
    ''' <remarks></remarks>
    PageNotFound = 2
    ''' <summary>
    ''' A CCP Error code was returned
    ''' Read the APILastError and APILastErrorText to get specific details
    ''' </summary>
    ''' <remarks></remarks>
    CCPError = 3
    ''' <summary>
    ''' The API Server does not support the requested API Type
    ''' </summary>
    ''' <remarks></remarks>
    InvalidFeature = 4
    ''' <summary>
    ''' The API Server could not be contacted and a null XML was returned
    ''' </summary>
    ''' <remarks></remarks>
    APIServerDownReturnedNull = 5
    ''' <summary>
    ''' The API Server could not be contacted so a cached XML file was returned
    ''' </summary>
    ''' <remarks></remarks>
    APIServerDownReturnedCached = 6
    ''' <summary>
    ''' The actual response from the API Server has been returned
    ''' </summary>
    ''' <remarks></remarks>
    ReturnedActual = 7
    ''' <summary>
    ''' There was no response from the API Server within a timely period
    ''' </summary>
    ''' <remarks></remarks>
    TimedOut = 8
    ''' <summary>
    ''' An error occured with the API Request but the cause is not known
    ''' </summary>
    ''' <remarks></remarks>
    UnknownError = 9
    ''' <summary>
    ''' An error occured within the EveAPIRequest code
    ''' </summary>
    ''' <remarks></remarks>
    InternalCodeError = 10
End Enum

''' <summary>
''' A list of possible methods of returning the requested API
''' </summary>
''' <remarks></remarks>
Public Enum APIReturnMethods As Integer
    ''' <summary>
    ''' Returns a new XML file if the cache timer has expired, otherwise a cached XML file is returned
    ''' </summary>
    ''' <remarks></remarks>
    ReturnStandard = 0
    ''' <summary>
    ''' Returns the XML file from the API cache
    ''' Returns Nothing (null) if no cache file exists
    ''' </summary>
    ''' <remarks></remarks>
    ReturnCacheOnly = 1
    ''' <summary>
    ''' Returns the actual API Request as received from the API Server, only after checking the cache
    ''' </summary>
    ''' <remarks></remarks>
    ReturnActual = 2
    ''' <summary>
    ''' Bypass the cache and retrieve the actual repsonse from the
    ''' </summary>
    ''' <remarks></remarks>
    BypassCache = 3
End Enum
