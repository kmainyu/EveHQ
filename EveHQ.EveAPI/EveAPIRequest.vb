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
    Private cAPILastRequestType As APITypes
    Private cAPILastResult As APIResults
    Private cAPILastError As Integer = -1
    Private cAPILastErrorText As String
    Private cAPILastFileName As String
    Private cAPIFileExtension As String = "aspx"
    Private cAPICacheLocation As String = Path.Combine(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData, "EveAPICache")
    Private cAPIServerAddress As String = "https://api.eveonline.com"
    Private cProxyServer As New RemoteProxyServer

    ''' <summary>
    ''' Gets or sets details to use as a web proxy server
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of the RemoteProxyServer class containing details of the proxy to use for web access</returns>
    ''' <remarks></remarks>
    Public Property ProxyServer() As RemoteProxyServer
        Get
            Return cProxyServer
        End Get
        Set(ByVal value As RemoteProxyServer)
            cProxyServer = value
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
            Return cAPIServerAddress
        End Get
        Set(ByVal value As String)
            If value = "" Then
                cAPIServerAddress = CCPServerAddress
            Else
                cAPIServerAddress = value
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
            Return cAPICacheLocation
        End Get
        Set(ByVal value As String)
            If value = "" Then
                cAPICacheLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveAPICache")
            Else
                cAPICacheLocation = value
            End If
            ' Try and create the cache location
            Try
                If My.Computer.FileSystem.DirectoryExists(cAPICacheLocation) = False Then
                    My.Computer.FileSystem.CreateDirectory(cAPICacheLocation)
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
            Return cAPIFileExtension
        End Get
        Set(ByVal value As String)
            If value = "" Then
                cAPIFileExtension = "aspx"
            Else
                cAPIFileExtension = value
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
            Return cAPILastResult
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
            Return cAPILastError
        End Get
    End Property

    Public ReadOnly Property LastAPIRequestType() As APITypes
        Get
            Return cAPILastRequestType
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
            Return cAPILastErrorText
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
            Return cAPILastFileName
        End Get
    End Property

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="APIType">The particular type of API to obtain</param>
    ''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal APIType As APITypes, ByVal APIReturnMethod As APIReturnMethods) As XmlDocument
        ' Accepts API features that do not have an explicit post request
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        cAPILastRequestType = APIType
        Select Case APIType
            Case APITypes.AllianceList
                remoteURL = "/eve/AllianceList.xml." & cAPIFileExtension
            Case APITypes.RefTypes
                remoteURL = "/eve/RefTypes.xml." & cAPIFileExtension
            Case APITypes.SkillTree
                remoteURL = "/eve/SkillTree.xml." & cAPIFileExtension
            Case APITypes.Sovereignty
                remoteURL = "/map/Sovereignty.xml." & cAPIFileExtension
            Case APITypes.SovereigntyStatus
                remoteURL = "/map/SovereigntyStatus.xml." & cAPIFileExtension
            Case APITypes.MapJumps
                remoteURL = "/map/Jumps.xml." & cAPIFileExtension
            Case APITypes.MapKills
                remoteURL = "/map/Kills.xml." & cAPIFileExtension
            Case APITypes.Conquerables
                remoteURL = "/eve/ConquerableStationList.xml." & cAPIFileExtension
            Case APITypes.ErrorList
                remoteURL = "/eve/ErrorList.xml." & cAPIFileExtension
            Case APITypes.FWStats
                remoteURL = "/eve/FacWarStats.xml." & cAPIFileExtension
            Case APITypes.FWTop100
                remoteURL = "/eve/FacWarTopStats.xml." & cAPIFileExtension
            Case APITypes.FWMap
                remoteURL = "/map/FacWarSystems.xml." & cAPIFileExtension
            Case APITypes.ServerStatus
                remoteURL = "/server/ServerStatus.xml." & cAPIFileExtension
            Case APITypes.CertificateTree
                remoteURL = "/eve/CertificateTree.xml." & cAPIFileExtension
            Case APITypes.CallList
                remoteURL = "/api/CallList.xml." & cAPIFileExtension
            Case Else
                cAPILastResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), APIType)
        Return GetXML(remoteURL, postdata, fileName, APIReturnMethod, APIType)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="APIType">The particular type of API to obtain</param>
    ''' <param name="Data">The information to be converted to an ID or Name</param>
    ''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal APIType As APITypes, ByVal Data As String, ByVal APIReturnMethod As APIReturnMethods) As XmlDocument
        ' Accepts API features that do not have an explicit post request
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        cAPILastRequestType = APIType
        Select Case APIType
            Case APITypes.NameToID
                postdata = "names=" & Data
                remoteURL = "/eve/CharacterID.xml." & cAPIFileExtension
            Case APITypes.IDToName
                postdata = "ids=" & Data
				remoteURL = "/eve/CharacterName.xml." & cAPIFileExtension
			Case APITypes.CharacterInfo
				postdata = "characterid=" & Data
				remoteURL = "/eve/CharacterInfo.xml." & cAPIFileExtension
			Case Else
				cAPILastResult = APIResults.InvalidFeature
				Return Nothing
				Exit Function
		End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), APIType) & "_" & Format(Now, "yyyyMMddhhmmssfffff")
        Return GetXML(remoteURL, postdata, fileName, APIReturnMethod, APIType)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="APIType">The particular type of API to obtain</param>
    ''' <param name="APIAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal APIType As APITypes, ByVal APIAccount As EveAPIAccount, ByVal APIReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        postdata = "keyID=" & APIAccount.userID & "&vCode=" & APIAccount.APIKey
        cAPILastRequestType = APIType
        Select Case APIType
            Case APITypes.Characters
                remoteURL = "/account/Characters.xml." & cAPIFileExtension
            Case APITypes.AccountStatus
                remoteURL = "/account/AccountStatus.xml." & cAPIFileExtension
            Case APITypes.APIKeyInfo
                remoteURL = "/account/APIKeyInfo.xml." & cAPIFileExtension
            Case Else
                cAPILastResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), APIType) & "_" & APIAccount.userID
        Return GetXML(remoteURL, postData, fileName, APIReturnMethod, APIType)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="APIType">The particular type of API to obtain</param>
    ''' <param name="APIAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal APIType As APITypes, ByVal APIAccount As EveAPIAccount, ByVal charID As String, ByVal APIReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        postdata = "keyID=" & APIAccount.userID & "&vCode=" & APIAccount.APIKey & "&characterID=" & charID
        cAPILastRequestType = APIType
        Select Case APIType
            Case APITypes.AccountBalancesChar
                remoteURL = "/char/AccountBalance.xml." & cAPIFileExtension
            Case APITypes.AccountBalancesCorp
                remoteURL = "/corp/AccountBalance.xml." & cAPIFileExtension
            Case APITypes.CharacterSheet
                remoteURL = "/char/CharacterSheet.xml." & cAPIFileExtension
            Case APITypes.CorpSheet
                remoteURL = "/corp/CorporationSheet.xml." & cAPIFileExtension
            Case APITypes.CorpMemberTracking
                remoteURL = "/corp/MemberTracking.xml." & cAPIFileExtension
            Case APITypes.SkillTraining
                remoteURL = "/char/SkillInTraining.xml." & cAPIFileExtension
            Case APITypes.SkillQueue
                remoteURL = "/char/SkillQueue.xml." & cAPIFileExtension
            Case APITypes.AssetsChar
                remoteURL = "/char/AssetList.xml." & cAPIFileExtension
            Case APITypes.AssetsCorp
                remoteURL = "/corp/AssetList.xml." & cAPIFileExtension
            Case APITypes.IndustryChar
                remoteURL = "/char/IndustryJobs.xml." & cAPIFileExtension
            Case APITypes.IndustryCorp
                remoteURL = "/corp/IndustryJobs.xml." & cAPIFileExtension
            Case APITypes.OrdersChar
                remoteURL = "/char/MarketOrders.xml." & cAPIFileExtension
            Case APITypes.OrdersCorp
                remoteURL = "/corp/MarketOrders.xml." & cAPIFileExtension
            Case APITypes.POSList
				remoteURL = "/corp/StarbaseList.xml." & cAPIFileExtension
			Case APITypes.OutpostList
				remoteURL = "/corp/OutpostList.xml." & cAPIFileExtension
            Case APITypes.StandingsChar
                remoteURL = "/char/Standings.xml." & cAPIFileExtension
            Case APITypes.StandingsCorp
                remoteURL = "/corp/Standings.xml." & cAPIFileExtension
            Case APITypes.CorpMemberSecurity
                remoteURL = "/corp/MemberSecurity.xml." & cAPIFileExtension
            Case APITypes.CorpMemberSecurityLog
                remoteURL = "/corp/MemberSecurityLog.xml." & cAPIFileExtension
            Case APITypes.CorpShareholders
                remoteURL = "/corp/Shareholders.xml." & cAPIFileExtension
            Case APITypes.CorpTitles
                remoteURL = "/corp/Titles.xml." & cAPIFileExtension
            Case APITypes.FWStatsChar
                remoteURL = "/char/FacWarStats.xml." & cAPIFileExtension
            Case APITypes.FWStatsCorp
                remoteURL = "/corp/FacWarStats.xml." & cAPIFileExtension
            Case APITypes.MedalsReceived
                remoteURL = "/char/Medals.xml." & cAPIFileExtension
            Case APITypes.MedalsAvailable
                remoteURL = "/corp/Medals.xml." & cAPIFileExtension
            Case APITypes.MemberMedals
                remoteURL = "/corp/MemberMedals.xml." & cAPIFileExtension
            Case APITypes.MailMessages
                remoteURL = "/char/MailMessages.xml." & cAPIFileExtension
            Case APITypes.Notifications
                remoteURL = "/char/Notifications.xml." & cAPIFileExtension
            Case APITypes.MailingLists
				remoteURL = "/char/MailingLists.xml." & cAPIFileExtension
			Case APITypes.Research
				remoteURL = "/char/Research.xml." & cAPIFileExtension
			Case APITypes.CharacterInfo
                remoteURL = "/eve/CharacterInfo.xml." & cAPIFileExtension
            Case APITypes.ContactListChar
                remoteURL = "/char/ContactList.xml." & cAPIFileExtension
            Case APITypes.ContactListCorp
                remoteURL = "/corp/ContactList.xml." & cAPIFileExtension
            Case APITypes.ContractsChar
                remoteURL = "/char/Contracts.xml." & cAPIFileExtension
            Case APITypes.ContractsCorp
                remoteURL = "/corp/Contracts.xml." & cAPIFileExtension
            Case APITypes.ContractBidsChar
                remoteURL = "/char/ContractBids.xml." & cAPIFileExtension
            Case APITypes.ContractBidsCorp
                remoteURL = "/corp/ContractBids.xml." & cAPIFileExtension
            Case APITypes.ContactNotifications
                remoteURL = "/char/ContactNotifications.xml." & cAPIFileExtension
            Case APITypes.UpcomingCalendarEvents
                remoteURL = "/char/UpcomingCalendarEvents.xml." & cAPIFileExtension
            Case Else
                cAPILastResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), APIType) & "_" & APIAccount.userID & "_" & charID
        Return GetXML(remoteURL, postData, fileName, APIReturnMethod, APIType)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="APIType">The particular type of API to obtain</param>
    ''' <param name="APIAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="itemID">The itemID of the starbase (POS) to query</param>
    ''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
	Public Overloads Function GetAPIXML(ByVal APIType As APITypes, ByVal APIAccount As EveAPIAccount, ByVal charID As String, ByVal itemID As Long, ByVal APIReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        postdata = "keyID=" & APIAccount.userID & "&vCode=" & APIAccount.APIKey & "&characterID=" & charID & "&itemID=" & itemID
		cAPILastRequestType = APIType
		Select Case APIType
			Case APITypes.POSDetails
				remoteURL = "/corp/StarbaseDetail.xml." & cAPIFileExtension
			Case APITypes.OutpostServiceDetail
                remoteURL = "/corp/OutpostServiceDetail.xml." & cAPIFileExtension
            Case APITypes.ContractItemsChar
                postdata = postdata.Replace("&itemID", "&contractID")
                remoteURL = "/char/ContractItems.xml." & cAPIFileExtension
            Case APITypes.ContractItemsCorp
                postdata = postdata.Replace("&itemID", "&contractID")
                remoteURL = "/corp/ContractItems.xml." & cAPIFileExtension
            Case Else
                cAPILastResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
		' Determine filename of cache
		Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), APIType) & "_" & APIAccount.userID & "_" & charID & "_" & itemID
		Return GetXML(remoteURL, postData, fileName, APIReturnMethod, APIType)
	End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="APIType">The particular type of API to obtain</param>
    ''' <param name="APIAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
	''' <param name="IDs">The additional Eve data with which to use with the request</param>
    ''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
	Public Overloads Function GetAPIXML(ByVal APIType As APITypes, ByVal APIAccount As EveAPIAccount, ByVal charID As String, ByVal IDs As String, ByVal APIReturnMethod As APIReturnMethods) As XmlDocument
		Dim remoteURL As String = ""
        Dim postdata As String = ""
        postdata = "keyID=" & APIAccount.userID & "&vCode=" & APIAccount.APIKey & "&characterID=" & charID
		cAPILastRequestType = APIType
		If IDs <> "" Then
			Select Case APIType
				Case APITypes.KillLogChar, APITypes.KillLogCorp
					postData &= "&beforeKillID=" & IDs
                Case APITypes.MailBodies, APITypes.NotificationTexts
                    postdata &= "&ids=" & IDs
                Case APITypes.CalendarEventAttendeesChar
                    postdata &= "eventIDs=" & IDs
            End Select
		End If
		Select Case APIType
			Case APITypes.KillLogChar
				remoteURL = "/char/Killlog.xml." & cAPIFileExtension
			Case APITypes.KillLogCorp
				remoteURL = "/corp/Killlog.xml." & cAPIFileExtension
			Case APITypes.MailBodies
                remoteURL = "/char/MailBodies.xml." & cAPIFileExtension
            Case APITypes.NotificationTexts
                remoteURL = "/char/NotificationTexts.xml." & cAPIFileExtension
            Case APITypes.CalendarEventAttendeesChar
                remoteURL = "/char/CalendarEventAttendees.xml." & cAPIFileExtension
            Case Else
                cAPILastResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
		' Determine filename of cache
		Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), APIType) & "_" & APIAccount.userID & "_" & charID
		If IDs <> "" Then
			Select Case APIType
				Case APITypes.KillLogChar, APITypes.KillLogCorp
					fileName &= "_" & IDs
                Case APITypes.MailBodies, APITypes.NotificationTexts
                    fileName &= "_" & Format(Now, "yyyyMMddhhmmssfffff")
            End Select
		End If
		Return GetXML(remoteURL, postData, fileName, APIReturnMethod, APIType)
	End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs
    ''' </summary>
    ''' <param name="APIType">The particular type of API to obtain</param>
    ''' <param name="APIAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="accountKey">The specific wallet accountID to query</param>
    ''' <param name="BeforeRefID">The Eve data reference from which to start the request</param>
    ''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns>An XMLDocument containing the request API data</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal APIType As APITypes, ByVal APIAccount As EveAPIAccount, ByVal charID As String, ByVal accountKey As Integer, ByVal BeforeRefID As String, ByVal APIReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        postdata = "keyID=" & APIAccount.userID & "&vCode=" & APIAccount.APIKey & "&characterID=" & charID & "&accountKey=" & accountKey
        cAPILastRequestType = APIType
        If BeforeRefID <> "" Then
            Select Case APIType
                Case APITypes.WalletTransChar, APITypes.WalletTransCorp
                    postData &= "&beforeTransID=" & BeforeRefID
            End Select
        End If
        Select Case APIType
            Case APITypes.WalletTransChar
                remoteURL = "/char/WalletTransactions.xml." & cAPIFileExtension
            Case APITypes.WalletTransCorp
                remoteURL = "/corp/WalletTransactions.xml." & cAPIFileExtension
            Case Else
                cAPILastResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), APIType) & "_" & APIAccount.userID & "_" & charID & "_" & accountKey
        If BeforeRefID <> "" Then
            fileName &= "_" & BeforeRefID
        End If
        Return GetXML(remoteURL, postData, fileName, APIReturnMethod, APIType)
    End Function

    ''' <summary>
    ''' Overloaded function to obtain an XML file from a particular API Type and using a particular return method
    ''' Different functions need to be used to obtain different APIs 
    ''' </summary>
     ''' <param name="APIType">The particular type of API to obtain</param>
    ''' <param name="APIAccount">An EveAPIAccount contained userID and APIKey to use in the request</param>
    ''' <param name="charID">The Eve characterID to use for the request</param>
    ''' <param name="accountKey">The specific wallet accountID to query</param>
    ''' <param name="fromID">The Eve data reference from which to start the request</param>
    ''' <param name="rowCount">The number of rows to return (max 256)</param>
    ''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetAPIXML(ByVal APIType As APITypes, ByVal APIAccount As EveAPIAccount, ByVal charID As String, ByVal accountKey As Integer, ByVal fromID As Long, ByVal rowCount As Integer, ByVal APIReturnMethod As APIReturnMethods) As XmlDocument
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        postdata = "keyID=" & APIAccount.userID & "&vCode=" & APIAccount.APIKey & "&characterID=" & charID & "&accountKey=" & accountKey
        cAPILastRequestType = APIType
        If fromID <> 0 Then
            Select Case APIType
                Case APITypes.WalletJournalChar, APITypes.WalletJournalCorp
                    postData &= "&fromID=" & fromID.ToString
            End Select
        End If
        postData &= "&rowCount=" & rowCount
        Select Case APIType
            Case APITypes.WalletJournalChar
                remoteURL = "/char/WalletJournal.xml." & cAPIFileExtension
            Case APITypes.WalletJournalCorp
                remoteURL = "/corp/WalletJournal.xml." & cAPIFileExtension
            Case Else
                cAPILastResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
		Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(APITypes), APIType) & "_" & APIAccount.userID & "_" & charID & "_" & accountKey & "_" & fromID
		Return GetXML(remoteURL, postData, fileName, APIReturnMethod, APIType)
    End Function

	''' <summary>
	''' Function to return an XML from an API Request
	''' </summary>
	''' <param name="remoteURL">The URL of the API page where the XML will be obtained</param>
	''' <param name="postData">Relevant data to be passed with the URL</param>
	''' <param name="fileName">The location in local storage of the cached XML to save and/or retrieve</param>
	''' <param name="APIReturnMethod">The particular method used to obtain the XML file</param>
	''' <param name="APIType">The particular type of API to obtain - required in case correction is required to the cache expiry time</param>
	''' <returns>An XMLDocument for the APIRequest, based upon the APIReturnMethod used</returns>
	''' <remarks></remarks>
    Private Function GetXML(ByVal remoteURL As String, ByVal postData As String, ByVal fileName As String, ByVal APIReturnMethod As APIReturnMethods, ByVal APIType As APITypes) As XmlDocument
        Dim fileDate As String = ""
        Dim APIXML As New XmlDocument
        Dim errlist As XmlNodeList
        Dim fileLoc As String = ""
        Try
            fileLoc = Path.Combine(cAPICacheLocation, fileName & fileDate & ".xml")
        Catch e As Exception
            Dim msg As String = "An error occured while trying to assemble the cache location string. The location being created should be in:" & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Cache Folder: " & cAPICacheLocation & ControlChars.CrLf
            msg &= "File Name: " & fileName & ControlChars.CrLf
            msg &= "File Date: " & fileDate & ControlChars.CrLf
            cAPILastResult = APIResults.InternalCodeError
            cAPILastErrorText = msg
            Return Nothing
        End Try
        Try
            ' See if we need to bypass the cache
            If APIReturnMethod = APIReturnMethods.BypassCache Then
                ' Perform this section if the cache is bypassed
                ' Fetch the XML from the EveAPI
                APIXML = FetchXMLFromWeb(remoteURL, postData)
                ' Check for null document (can happen if APIRS) isn't active and no backup is used
                If APIXML.InnerXml = "" Then
                    ' Do not save and return nothing
                    cAPILastResult = APIResults.APIServerDownReturnedNull
                    Return Nothing
                Else
                    ' Check for error codes
                    errlist = APIXML.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Try
                            Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                            Dim errMsg As String = errNode.InnerText
                            ' Return the current XML regardless
                            cAPILastResult = APIResults.ReturnedActual
                            cAPILastError = CInt(errCode)
                            cAPILastErrorText = errMsg
                            Return APIXML
                        Catch e As Exception
                            ' Return nothing and report a general error - usually as a result of a API Server error
                            cAPILastResult = APIResults.UnknownError
                            cAPILastError = 0
                            cAPILastErrorText = "A General Error occurred"
                            Return Nothing
                        End Try
                    Else
                        ' Update the incorrect cache data
                        Call UpdateAPICacheTime(APIXML, APIType)
                        cAPILastResult = APIResults.ReturnedActual
                        Return APIXML
                    End If
                End If
            Else
                ' Check if the file already exists
                cAPILastFileName = fileLoc
                If My.Computer.FileSystem.FileExists(fileLoc) = True Then
                    Dim tmpAPIXML As New XmlDocument
                    ' Check cache time of file
                    Dim failedCacheLoad As Boolean = False
                    Try
                        APIXML.Load(fileLoc)
                    Catch e As Exception
                        failedCacheLoad = True
                        ' Attempt to get a new XML and save
                        APIXML = FetchXMLFromWeb(remoteURL, postData)
                        ' Try to save the XML file
                        Call SaveAPIXML(APIXML, fileLoc)
                    End Try
                    ' Get Cache time details
                    Dim cacheDetails As XmlNodeList = APIXML.SelectNodes("/eveapi")
                    Dim cacheTime As DateTime = CDate(cacheDetails(0).ChildNodes(2).InnerText)
                    Dim localCacheTime As Date = ConvertEveTimeToLocal(cacheTime)
                    ' Has Cache expired?
                    If (localCacheTime > Now Or APIReturnMethod = APIReturnMethods.ReturnCacheOnly = True) And failedCacheLoad = False Then
                        '  Cache has not expired or a request to return cached version- return existing XML
                        cAPILastResult = APIResults.ReturnedCached
                        Return APIXML
                    Else
                        ' Cache has expired - get a new XML
                        tmpAPIXML = FetchXMLFromWeb(remoteURL, postData)
                        ' Check for null document (can happen if APIRS) isn't active and no backup is used
                        If tmpAPIXML.InnerXml = "" Then
                            ' Do not save and return the old API file
                            cAPILastResult = APIResults.APIServerDownReturnedCached
                            Return APIXML
                        End If
                        ' Check for error codes
                        errlist = tmpAPIXML.SelectNodes("/eveapi/error")
                        If errlist.Count <> 0 Then
                            Dim errNode As XmlNode = errlist(0)
                            If errlist(0).Attributes.GetNamedItem("code") IsNot Nothing Then
                                ' Get error code
                                Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                                Dim errMsg As String = errNode.InnerText
                                If APIReturnMethod = APIReturnMethods.ReturnStandard Then
                                    ' Return the old XML file but report the error
                                    cAPILastResult = APIResults.CCPError
                                    cAPILastError = CInt(errCode)
                                    cAPILastErrorText = errMsg
                                    Return APIXML
                                Else
                                    ' Return the current one regardless
                                    cAPILastResult = APIResults.ReturnedActual
                                    cAPILastError = CInt(errCode)
                                    cAPILastErrorText = errMsg
                                    ' Try to save the XML file
                                    Call SaveAPIXML(tmpAPIXML, fileLoc)
                                    Return tmpAPIXML
                                End If
                            Else
                                ' Return the old XML file but report a general error - usually as a result of a API Server error
                                cAPILastResult = APIResults.UnknownError
                                cAPILastError = 999
                                cAPILastErrorText = "A General Error occurred. " & errNode.InnerText
                                Return APIXML
                            End If
                        Else
                            ' No error codes so save, then return new XML file
                            cAPILastResult = APIResults.ReturnedNew
                            ' Update the incorrect cache data
                            Call UpdateAPICacheTime(tmpAPIXML, APIType)
                            ' Try to save the XML file
                            Call SaveAPIXML(tmpAPIXML, fileLoc)
                            Return tmpAPIXML
                        End If
                    End If
                Else
                    ' Do this if a file does not exist
                    If APIReturnMethod = APIReturnMethods.ReturnCacheOnly Then
                        ' If we demand that a cached fle be returned, return nothing as the file does not exist
                        Return Nothing
                    Else
                        ' Fetch the XML from the EveAPI
                        APIXML = FetchXMLFromWeb(remoteURL, postData)
                        ' Check for null document (can happen if APIRS) isn't active and no backup is used
                        If APIXML.InnerXml = "" Then
                            ' Do not save and return nothing
                            cAPILastResult = APIResults.APIServerDownReturnedNull
                            Return Nothing
                        Else
                            ' Check for error codes
                            errlist = APIXML.SelectNodes("/eveapi/error")
                            If errlist.Count <> 0 Then
                                Dim errNode As XmlNode = errlist(0)
                                ' Get error code
                                Try
                                    Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                                    Dim errMsg As String = errNode.InnerText
                                    If APIReturnMethod = APIReturnMethods.ReturnStandard Then
                                        ' Return the file but don't save it as we have an error
                                        cAPILastResult = APIResults.CCPError
                                        cAPILastError = CInt(errCode)
                                        cAPILastErrorText = errMsg
                                        Return APIXML
                                    Else
                                        ' Return the current one regardless
                                        cAPILastResult = APIResults.ReturnedActual
                                        cAPILastError = CInt(errCode)
                                        cAPILastErrorText = errMsg
                                        ' Try to save the XML file
                                        Call SaveAPIXML(APIXML, fileLoc)
                                        Return APIXML
                                    End If
                                Catch e As Exception
                                    ' Return nothing and report a general error - usually as a result of a API Server error
                                    cAPILastResult = APIResults.UnknownError
                                    cAPILastError = 0
                                    cAPILastErrorText = "A General Error occurred"
                                    Return Nothing
                                End Try
                            Else
                                ' Update the incorrect cache data
                                Call UpdateAPICacheTime(APIXML, APIType)
                                ' Save the XML to disk
                                ' Try to save the XML file
                                Call SaveAPIXML(APIXML, fileLoc)
                                cAPILastResult = APIResults.ReturnedNew
                                Return APIXML
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            ' Something happened so let's return nothing and let the calling routine handle it
            cAPILastResult = APIResults.InternalCodeError
            cAPILastErrorText = ex.Message
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
        Dim APIServer As String = cAPIServerAddress
        remoteURL = APIServer & remoteURL
        Dim webdata As String = ""
        Dim APIXML As New XmlDocument
        Try
            ' Create the requester
            ServicePointManager.DefaultConnectionLimit = 20
            ServicePointManager.Expect100Continue = False
            Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(remoteURL))
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
            Dim WebEncoding As New ASCIIEncoding()
            Dim byte1 As Byte() = WebEncoding.GetBytes(postData)
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
            APIXML.LoadXml(webdata)
            Dim errlist As XmlNodeList = APIXML.SelectNodes("/eveapi/error")
            If errlist.Count <> 0 Then
                Dim errNode As XmlNode = errlist(0)
                ' Get error code
                If errNode.Attributes.GetNamedItem("code") IsNot Nothing Then
                    Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                    Dim errMsg As String = errNode.InnerText
                    cAPILastResult = APIResults.CCPError
                    cAPILastError = CInt(errCode)
                    cAPILastErrorText = errMsg
                Else
                    cAPILastResult = APIResults.UnknownError
                    cAPILastError = 999
                    cAPILastErrorText = errNode.InnerText
                End If
            Else
                ' Result will be given in the calling sub
            End If
        Catch e As Exception
            If e.Message.Contains("timed out") = True Then
                cAPILastResult = APIResults.TimedOut
                cAPILastError = 0
                cAPILastErrorText = e.Message
            Else
                cAPILastResult = APIResults.UnknownError
                cAPILastError = 0
                cAPILastErrorText = e.Message
            End If
        End Try
        Return APIXML
    End Function

    ''' <summary>
    ''' Routine to correct the cache timers provided in certain APIs that are reported incorrectly
    ''' </summary>
    ''' <param name="APIXML">The XMLDocument to correct</param>
    ''' <param name="APIType">The type of API that is being passed</param>
    ''' <remarks></remarks>
    Private Sub UpdateAPICacheTime(ByRef APIXML As XmlDocument, ByVal APIType As APITypes)
        Dim cacheDetails As XmlNodeList = APIXML.SelectNodes("/eveapi")
        Dim APITime As DateTime = CDate(cacheDetails(0).ChildNodes(0).InnerText)
        ' Set the cache times to the correct values
        Select Case APIType
            'Case APITypes.WalletJournalChar, APITypes.WalletJournalCorp, APITypes.WalletTransChar, APITypes.WalletTransCorp
            'cacheDetails(0).ChildNodes(2).InnerText = Format(APITime.AddSeconds(3630), "yyyy-MM-dd HH:mm:ss")
            'Case APITypes.AccountBalancesChar, APITypes.AccountBalancesCorp, APITypes.OrdersChar, APITypes.OrdersCorp
            'cacheDetails(0).ChildNodes(2).InnerText = Format(APITime.AddSeconds(930), "yyyy-MM-dd HH:mm:ss")
        End Select
    End Sub

    ''' <summary>
    ''' Routine for saving an XMLDocument to the Cache
    ''' </summary>
    ''' <param name="APIXML">The XMLDocument to save</param>
    ''' <param name="fileLoc">The location in local storage to save the XMLDocument</param>
    ''' <remarks></remarks>
    Private Sub SaveAPIXML(ByRef APIXML As XmlDocument, ByVal fileLoc As String)
        Dim FileSaved As Boolean = False
        Dim CurrentAttempt As Integer = 0
        Do
            Try
                CurrentAttempt += 1
                APIXML.Save(fileLoc)
                FileSaved = True
            Catch e As Exception
                ' Failed save, presumably due to conflicting access
            End Try
        Loop Until FileSaved = True Or CurrentAttempt >= ErrorRetries
    End Sub

    ''' <summary>
    ''' Converts EveTime to the user's local time
    ''' </summary>
    ''' <param name="EveTime">The time of the Eve Server</param>
    ''' <returns>The local time equivalent of Eve Time</returns>
    ''' <remarks></remarks>
    Private Function ConvertEveTimeToLocal(ByVal EveTime As Date) As Date
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
        Me.APIServerAddress = ""
        Me.ProxyServer = Nothing
        Me.APIFileExtension = ""
        Me.APICacheLocation = ""
        cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server
    ''' </summary>
    ''' <param name="ServerInfo">The API server details used for access to the API</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ServerInfo As APIServerInfo)
        If ServerInfo.UseAPIRS = True Then
            Me.APIServerAddress = ServerInfo.APIRSServer
        Else
            Me.APIServerAddress = ServerInfo.CCPServer
        End If
        Me.ProxyServer = Nothing
        Me.APIFileExtension = ""
        Me.APICacheLocation = ""
        cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server together with a Proxy Server
    ''' </summary>
    ''' <param name="ServerInfo">The API server details used for access to the API</param>
    ''' <param name="ProxyDetails">An instance of the RemoteProxyServer class containing Proxy Server details</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ServerInfo As APIServerInfo, ByVal ProxyDetails As RemoteProxyServer)
        If ServerInfo.UseAPIRS = True Then
            Me.APIServerAddress = ServerInfo.APIRSServer
        Else
            Me.APIServerAddress = ServerInfo.CCPServer
        End If
        Me.ProxyServer = ProxyDetails
        Me.APIFileExtension = ""
        Me.APICacheLocation = ""
        cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server together with an alternative web page extension for non .Net servers
    ''' </summary>
    ''' <param name="ServerInfo">The API server details used for access to the API</param>
    ''' <param name="FileExtension">The web page extension to use for API Requests</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ServerInfo As APIServerInfo, ByVal FileExtension As String)
        If ServerInfo.UseAPIRS = True Then
            Me.APIServerAddress = ServerInfo.APIRSServer
        Else
            Me.APIServerAddress = ServerInfo.CCPServer
        End If
        Me.ProxyServer = Nothing
        Me.APIFileExtension = FileExtension
        Me.APICacheLocation = ""
        cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server and Proxy Server, together with an alternative web page extension for non .Net servers
    ''' </summary>
    ''' <param name="ServerInfo">The API server details used for access to the API</param>
    ''' <param name="ProxyDetails">An instance of the RemoteProxyServer class containing Proxy Server details</param>
    ''' <param name="FileExtension">The web page extension to use for API Requests</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ServerInfo As APIServerInfo, ByVal ProxyDetails As RemoteProxyServer, ByVal FileExtension As String)
        If ServerInfo.UseAPIRS = True Then
            Me.APIServerAddress = ServerInfo.APIRSServer
        Else
            Me.APIServerAddress = ServerInfo.CCPServer
        End If
        Me.ProxyServer = ProxyDetails
        Me.APIFileExtension = FileExtension
        Me.APICacheLocation = ""
        cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows the EveAPIRequest to use an alternative API Server and Cache Location, together with an alternative web page extension for non .Net servers
    ''' </summary>
    ''' <param name="ServerInfo">The API server details used for access to the API</param>
    ''' <param name="FileExtension">The web page extension to use for API Requests</param>
    ''' <param name="CacheLocation">The location in local storage of where the cached API files are maintained</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ServerInfo As APIServerInfo, ByVal FileExtension As String, ByVal CacheLocation As String)
        If ServerInfo.UseAPIRS = True Then
            Me.APIServerAddress = ServerInfo.APIRSServer
        Else
            Me.APIServerAddress = ServerInfo.CCPServer
        End If
        Me.ProxyServer = Nothing
        Me.APIFileExtension = FileExtension
        Me.APICacheLocation = CacheLocation
        cAPILastResult = APIResults.NotYetProcessed
    End Sub

    ''' <summary>
    ''' Initialises a new EveAPIRequest
    ''' Allows full configuration of the EveAPIRequest
    ''' </summary>
    ''' <param name="ServerInfo">The API server details used for access to the API</param>
    ''' <param name="ProxyDetails">An instance of the RemoteProxyServer class containing Proxy Server details</param>
    ''' <param name="FileExtension">The web page extension to use for API Requests</param>
    ''' <param name="CacheLocation">The location in local storage of where the cached API files are maintained</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ServerInfo As APIServerInfo, ByVal ProxyDetails As RemoteProxyServer, ByVal FileExtension As String, ByVal CacheLocation As String)
        If ServerInfo.UseAPIRS = True Then
            Me.APIServerAddress = ServerInfo.APIRSServer
        Else
            Me.APIServerAddress = ServerInfo.CCPServer
        End If
        Me.ProxyServer = ProxyDetails
        Me.APIFileExtension = FileExtension
        Me.APICacheLocation = CacheLocation
        cAPILastResult = APIResults.NotYetProcessed
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
    Public Property UseCCPAPIBackup As Boolean

    Public Sub New()
        Me.CCPServer = ""
        Me.APIRSServer = ""
        Me.UseAPIRS = False
        Me.UseCCPAPIBackup = False
    End Sub

    ''' <summary>
    ''' Creates a new instance of the APIServerInfo class to pass to an EveAPIRequest
    ''' </summary>
    ''' <param name="CCPServerAddress">URL of the CCP API Server</param>
    ''' <param name="APIRSAddress">URL of the custom API Relay Server</param>
    ''' <param name="UseAPIRelayServer">Set to true if using the API Relay Server</param>
    ''' <param name="UseCCPServerForBackup">Set to true if using the CCP API Server as a backup</param>
    ''' <remarks></remarks>
    Public Sub New(CCPServerAddress As String, APIRSAddress As String, UseAPIRelayServer As Boolean, UseCCPServerForBackup As Boolean)
        Me.CCPServer = CCPServerAddress
        Me.APIRSServer = APIRSAddress
        Me.UseAPIRS = UseAPIRelayServer
        Me.UseCCPAPIBackup = UseCCPServerForBackup
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
