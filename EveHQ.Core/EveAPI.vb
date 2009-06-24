' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2008  Lee Vessey
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

Public Class EveAPI
    Private Shared cLastAPIResult As Integer
    Private Shared cLastAPIError As Integer
    Private Shared cLastAPIErrorText As String
    Private Shared cLastAPIFileName As String
    Public Shared ReadOnly Property LastAPIResult() As Integer
        Get
            Return cLastAPIResult
        End Get
    End Property
    Public Shared ReadOnly Property LastAPIError() As Integer
        Get
            Return cLastAPIError
        End Get
    End Property
    Public Shared ReadOnly Property LastAPIErrorText() As String
        Get
            Return cLastAPIErrorText
        End Get
    End Property
    Public Shared ReadOnly Property LastAPIFileName() As String
        Get
            Return cLastAPIFileName
        End Get
    End Property

    Overloads Shared Function GetAPIXML(ByVal Feature As Integer, ByVal ReturnMethod As Integer) As XmlDocument
        ' Accepts API features that do not have an explicit post request
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        Select Case Feature
            Case EveHQ.Core.EveAPI.APIRequest.AllianceList
                remoteURL = "/eve/AllianceList.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.RefTypes
                remoteURL = "/eve/RefTypes.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.SkillTree
                remoteURL = "/eve/SkillTree.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.Sovereignty
                remoteURL = "/map/Sovereignty.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.MapJumps
                remoteURL = "/map/Jumps.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.MapKills
                remoteURL = "/map/Kills.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.Conquerables
                remoteURL = "/eve/ConquerableStationList.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.ErrorList
                remoteURL = "/eve/ErrorList.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.FWTop100
                remoteURL = "/eve/FacWarTopStats.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.FWMap
                remoteURL = "/map/FacWarSystems.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.ServerStatus
                remoteURL = "/server/ServerStatus.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.CertificateTree
                remoteURL = "/eve/CertificateTree.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case Else
                cLastAPIResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(EveHQ.Core.EveAPI.APIRequest), Feature)
        Return EveHQ.Core.EveAPI.GetXML(remoteURL, postdata, fileName, ReturnMethod, Feature)
    End Function
    Overloads Shared Function GetAPIXML(ByVal Feature As Integer, ByVal charData As String, ByVal ReturnMethod As Integer) As XmlDocument
        ' Accepts API features that do not have an explicit post request
        Dim remoteURL As String = ""
        Dim postdata As String = ""
        Select Case Feature
            Case EveHQ.Core.EveAPI.APIRequest.NameToID
                postdata = "names=" & charData
                remoteURL = "/eve/CharacterID.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.IDToName
                postdata = "ids=" & charData
                remoteURL = "/eve/CharacterName.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case Else
                cLastAPIResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(EveHQ.Core.EveAPI.APIRequest), Feature) & "_" & charData
        Return EveHQ.Core.EveAPI.GetXML(remoteURL, postdata, fileName, ReturnMethod, Feature)
    End Function
    Overloads Shared Function GetAPIXML(ByVal Feature As Integer, ByVal cAccount As EveHQ.Core.EveAccount, ByVal ReturnMethod As Integer) As XmlDocument
        Dim remoteURL As String = ""
        Dim postData As String = "userID=" & cAccount.userID & "&apiKey=" & cAccount.APIKey
        Select Case Feature
            Case EveHQ.Core.EveAPI.APIRequest.Characters

                remoteURL = "/account/Characters.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case Else
                cLastAPIResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(EveHQ.Core.EveAPI.APIRequest), Feature) & "_" & cAccount.userID
        Return EveHQ.Core.EveAPI.GetXML(remoteURL, postData, fileName, ReturnMethod, Feature)
    End Function
    Overloads Shared Function GetAPIXML(ByVal Feature As Integer, ByVal cAccount As EveHQ.Core.EveAccount, ByVal charID As String, ByVal ReturnMethod As Integer) As XmlDocument
        Dim remoteURL As String = ""
        Dim postData As String = "userID=" & cAccount.userID & "&apiKey=" & cAccount.APIKey & "&characterID=" & charID
        Select Case Feature
            Case EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar
                remoteURL = "/char/AccountBalance.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp
                remoteURL = "/corp/AccountBalance.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.CharacterSheet
                remoteURL = "/char/CharacterSheet.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.CorpSheet
                remoteURL = "/corp/CorporationSheet.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.CorpMemberTracking
                remoteURL = "/corp/MemberTracking.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.SkillTraining
                remoteURL = "/char/SkillInTraining.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.SkillQueue
                remoteURL = "/char/SkillQueue.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.AssetsChar
                remoteURL = "/char/AssetList.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.AssetsCorp
                remoteURL = "/corp/AssetList.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.IndustryChar
                remoteURL = "/char/IndustryJobs.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.IndustryCorp
                remoteURL = "/corp/IndustryJobs.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.OrdersChar
                remoteURL = "/char/MarketOrders.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.OrdersCorp
                remoteURL = "/corp/MarketOrders.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.POSList
                remoteURL = "/corp/StarbaseList.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.StandingsChar
                remoteURL = "/char/Standings.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.StandingsCorp
                remoteURL = "/corp/Standings.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.CorpMemberSecurity
                remoteURL = "/corp/MemberSecurity.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.CorpMemberSecurityLog
                remoteURL = "/corp/MemberSecurityLog.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.CorpShareholders
                remoteURL = "/corp/Shareholders.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.CorpTitles
                remoteURL = "/corp/Titles.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.FWStatsChar
                remoteURL = "/char/FacWarStats.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.FWStatsCorp
                remoteURL = "/corp/FacWarStats.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.MedalsReceived
                remoteURL = "/char/Medals.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.MedalsAvailable
                remoteURL = "/corp/Medals.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.MemberMedals
                remoteURL = "/corp/MemberMedals.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case Else
                cLastAPIResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(EveHQ.Core.EveAPI.APIRequest), Feature) & "_" & cAccount.userID & "_" & charID
        Return EveHQ.Core.EveAPI.GetXML(remoteURL, postData, fileName, ReturnMethod, Feature)
    End Function
    Overloads Shared Function GetAPIXML(ByVal Feature As Integer, ByVal cAccount As EveHQ.Core.EveAccount, ByVal charID As String, ByVal itemID As Integer, ByVal ReturnMethod As Integer) As XmlDocument
        Dim remoteURL As String = ""
        Dim postData As String = "userID=" & cAccount.userID & "&apiKey=" & cAccount.APIKey & "&characterID=" & charID & "&itemID=" & itemID
        Select Case Feature
            Case EveHQ.Core.EveAPI.APIRequest.POSDetails
                remoteURL = "/corp/StarbaseDetail.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case Else
                cLastAPIResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(EveHQ.Core.EveAPI.APIRequest), Feature) & "_" & cAccount.userID & "_" & charID & "_" & itemID
        Return EveHQ.Core.EveAPI.GetXML(remoteURL, postData, fileName, ReturnMethod, Feature)
    End Function
    Overloads Shared Function GetAPIXML(ByVal Feature As Integer, ByVal cAccount As EveHQ.Core.EveAccount, ByVal charID As String, ByVal BeforeRefID As String, ByVal ReturnMethod As Integer) As XmlDocument
        Dim remoteURL As String = ""
        Dim postData As String = "userID=" & cAccount.userID & "&apiKey=" & cAccount.APIKey & "&characterID=" & charID
        If BeforeRefID <> "" Then
            Select Case Feature
                Case EveHQ.Core.EveAPI.APIRequest.KillLogChar, EveHQ.Core.EveAPI.APIRequest.KillLogCorp
                    postData &= "&beforeKillID=" & BeforeRefID
            End Select
        End If
        Select Case Feature
            Case EveHQ.Core.EveAPI.APIRequest.KillLogChar
                remoteURL = "/char/Killlog.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.KillLogCorp
                remoteURL = "/corp/Killlog.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case Else
                cLastAPIResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(EveHQ.Core.EveAPI.APIRequest), Feature) & "_" & cAccount.userID & "_" & charID
        If BeforeRefID <> "" Then
            fileName &= "_" & BeforeRefID
        End If
        Return EveHQ.Core.EveAPI.GetXML(remoteURL, postData, fileName, ReturnMethod, Feature)
    End Function
    Overloads Shared Function GetAPIXML(ByVal Feature As Integer, ByVal cAccount As EveHQ.Core.EveAccount, ByVal charID As String, ByVal accountKey As Integer, ByVal BeforeRefID As String, ByVal ReturnMethod As Integer) As XmlDocument
        Dim remoteURL As String = ""
        Dim postData As String = "userID=" & cAccount.userID & "&apiKey=" & cAccount.APIKey & "&characterID=" & charID & "&accountKey=" & accountKey
        If BeforeRefID <> "" Then
            Select Case Feature
                Case EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp
                    postData &= "&beforeRefID=" & BeforeRefID
                Case EveHQ.Core.EveAPI.APIRequest.WalletTransChar, EveHQ.Core.EveAPI.APIRequest.WalletTransCorp
                    postData &= "&beforeTransID=" & BeforeRefID
            End Select
        End If
        Select Case Feature
            Case EveHQ.Core.EveAPI.APIRequest.WalletJournalChar
                remoteURL = "/char/WalletJournal.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp
                remoteURL = "/corp/WalletJournal.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.WalletTransChar
                remoteURL = "/char/WalletTransactions.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case EveHQ.Core.EveAPI.APIRequest.WalletTransCorp
                remoteURL = "/corp/WalletTransactions.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            Case Else
                cLastAPIResult = APIResults.InvalidFeature
                Return Nothing
                Exit Function
        End Select
        ' Determine filename of cache
        Dim fileName As String = "EVEHQAPI_" & [Enum].GetName(GetType(EveHQ.Core.EveAPI.APIRequest), Feature) & "_" & cAccount.userID & "_" & charID & "_" & accountKey
        If BeforeRefID <> "" Then
            fileName &= "_" & BeforeRefID
        End If
        Return EveHQ.Core.EveAPI.GetXML(remoteURL, postData, fileName, ReturnMethod, Feature)
    End Function

    Private Shared Function GetXML(ByVal remoteURL As String, ByVal postData As String, ByVal fileName As String, ByVal ReturnMethod As Long, ByVal feature As Integer) As XmlDocument
        Dim fileDate As String = ""
        ' Check if the file already exists
        Dim fileLoc As String = Path.Combine(EveHQ.Core.HQ.cacheFolder, fileName & fileDate & ".xml")
        cLastAPIFileName = fileLoc
        Dim APIXML As New XmlDocument
        Dim errlist As XmlNodeList
        Try
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
                    APIXML.Save(fileLoc)
                End Try
                ' Get Cache time details
                Dim cacheDetails As XmlNodeList = APIXML.SelectNodes("/eveapi")
                Dim cacheTime As DateTime = CDate(cacheDetails(0).ChildNodes(2).InnerText)
                Dim localCacheTime As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cacheTime)
                ' Has Cache expired?
                If (localCacheTime > Now Or ReturnMethod = APIReturnMethod.ReturnCacheOnly = True) And failedCacheLoad = False Then
                    '  Cache has not expired or a request to return cached version- return existing XML
                    cLastAPIResult = APIResults.ReturnedCached
                    Return APIXML
                Else
                    ' Cache has expired - get a new XML
                    tmpAPIXML = FetchXMLFromWeb(remoteURL, postData)
                    ' Check for null document (can happen if APIRS) isn't active and no backup is used
                    If tmpAPIXML.InnerXml = "" Then
                        ' Do not save and return the old API file
                        cLastAPIResult = APIResults.APIServerDownReturnedCached
                        Return APIXML
                    End If
                    ' Check for error codes
                    errlist = tmpAPIXML.SelectNodes("/eveapi/error")
                    If errlist.Count <> 0 Then
                        Dim errNode As XmlNode = errlist(0)
                        ' Get error code
                        Try
                            Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                            Dim errMsg As String = errNode.InnerText
                            If ReturnMethod = APIReturnMethod.ReturnStandard Then
                                ' Return the old XML file but report the error
                                cLastAPIResult = APIResults.CCPError
                                cLastAPIError = CInt(errCode)
                                cLastAPIErrorText = errMsg
                                Return APIXML
                            Else
                                ' Return the current one regardless
                                cLastAPIResult = APIResults.ReturnedActual
                                tmpAPIXML.Save(fileLoc)
                                Return tmpAPIXML
                            End If
                        Catch e As Exception
                            ' Return the old XML file but report a general error - usually as a result of a API Server error
                            cLastAPIResult = APIResults.UnknownError
                            cLastAPIError = 999
                            cLastAPIErrorText = "A General Error occurred"
                            Return APIXML
                        End Try
                    Else
                        ' No error codes so save, then return new XML file
                        cLastAPIResult = APIResults.ReturnedNew
                        ' Update the incorrect cache data
                        Call UpdateAPICacheTime(tmpAPIXML, feature)
                        tmpAPIXML.Save(fileLoc)
                        Return tmpAPIXML
                    End If
                End If
            Else
                If ReturnMethod = APIReturnMethod.ReturnCacheOnly Then
                    ' If we demand that a cached fle be returned, return nothing as the file does not exist
                    Return Nothing
                Else
                    ' Fetch the XML from the EveAPI
                    APIXML = FetchXMLFromWeb(remoteURL, postData)
                    ' Check for null document (can happen if APIRS) isn't active and no backup is used
                    If APIXML.InnerXml = "" Then
                        ' Do not save and return nothing
                        cLastAPIResult = APIResults.APIServerDownReturnedNull
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
                                If ReturnMethod = APIReturnMethod.ReturnStandard Then
                                    ' Return the file but don't save it as we have an error
                                    cLastAPIResult = APIResults.CCPError
                                    cLastAPIError = CInt(errCode)
                                    cLastAPIErrorText = errMsg
                                    Return APIXML
                                Else
                                    ' Return the current one regardless
                                    cLastAPIResult = APIResults.ReturnedActual
                                    APIXML.Save(fileLoc)
                                    Return APIXML
                                End If
                            Catch e As Exception
                                ' Return nothing and report a general error - usually as a result of a API Server error
                                cLastAPIResult = APIResults.UnknownError
                                cLastAPIError = 0
                                cLastAPIErrorText = "A General Error occurred"
                                Return Nothing
                            End Try
                        Else
                            ' Update the incorrect cache data
                            Call UpdateAPICacheTime(APIXML, feature)
                            ' Save the XML to disk
                            APIXML.Save(fileLoc)
                            cLastAPIResult = APIResults.ReturnedNew
                            Return APIXML
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            ' Something happened so let's return nothing and let the calling routine handle it
            Return Nothing
        End Try
    End Function
    Private Shared Function FetchXMLFromWeb(ByVal remoteURL As String, ByVal postData As String) As XmlDocument
        ' Determine if we use the APIRS or CCP API Server

        Dim APIServer As String = ""
        If EveHQ.Core.HQ.EveHQSettings.UseAPIRS = True Then
            APIServer = EveHQ.Core.HQ.EveHQSettings.APIRSAddress
            ' Check for APIRS heartbeart
            If EveHQ.Core.EveAPI.APIRSHasHeartbeat() = False Then
                If EveHQ.Core.HQ.EveHQSettings.UseCCPAPIBackup = True Then
                    APIServer = EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress
                End If
            End If
        Else
            APIServer = EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress
        End If
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
            If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
                Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
                If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                    EveHQProxy.UseDefaultCredentials = True
                Else
                    EveHQProxy.UseDefaultCredentials = False
                    EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
                End If
                request.Proxy = EveHQProxy
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
            ' Prepare for a response from the server
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Get the stream associated with the response.
            Dim receiveStream As Stream = response.GetResponseStream()
            ' Pipes the stream to a higher level stream reader with the required encoding format. 
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            webdata = readStream.ReadToEnd()
            ' Check response string for any error codes?
            APIXML.LoadXml(webdata)
            Dim errlist As XmlNodeList = APIXML.SelectNodes("/eveapi/error")
            If errlist.Count <> 0 Then
                Dim errNode As XmlNode = errlist(0)
                ' Get error code
                Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                Dim errMsg As String = errNode.InnerText
                cLastAPIResult = APIResults.CCPError
                cLastAPIError = CInt(errCode)
                cLastAPIErrorText = errMsg
            Else
                ' Result will be given in the calling sub
            End If
        Catch e As Exception
            If e.Message.Contains("timed out") = True Then
                cLastAPIResult = APIResults.TimedOut
                cLastAPIError = 0
                cLastAPIErrorText = e.Message
            Else
                cLastAPIResult = APIResults.UnknownError
                cLastAPIError = 0
                cLastAPIErrorText = e.Message
            End If
        End Try
        Return APIXML
    End Function
    Private Shared Sub UpdateAPICacheTime(ByRef cXML As XmlDocument, ByVal feature As Integer)
        Dim cacheDetails As XmlNodeList = cXML.SelectNodes("/eveapi")
        Dim APITime As DateTime = CDate(cacheDetails(0).ChildNodes(0).InnerText)
        ' Set the cache times to the correct values (because CCP suck rhino balls)
        Select Case feature
            Case EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp, EveHQ.Core.EveAPI.APIRequest.WalletTransChar, EveHQ.Core.EveAPI.APIRequest.WalletTransCorp
                cacheDetails(0).ChildNodes(2).InnerText = Format(APITime.AddSeconds(3630), "yyyy-MM-dd HH:mm:ss")
            Case EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, EveHQ.Core.EveAPI.APIRequest.OrdersChar, EveHQ.Core.EveAPI.APIRequest.OrdersCorp
                cacheDetails(0).ChildNodes(2).InnerText = Format(APITime.AddSeconds(930), "yyyy-MM-dd HH:mm:ss")
        End Select
    End Sub

    Public Shared Function APIRSHasHeartbeat() As Boolean
        Dim webdata As String = ""
        Try
            ' Create the requester
            Dim request As HttpWebRequest = CType(WebRequest.Create(EveHQ.Core.HQ.EveHQSettings.APIRSAddress), HttpWebRequest)
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
                Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
                If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                    EveHQProxy.UseDefaultCredentials = True
                Else
                    EveHQProxy.UseDefaultCredentials = False
                    EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
                End If
                request.Proxy = EveHQProxy
            End If
            ' Setup request parameters
            request.Method = "POST"
            request.ContentType = "application/x-www-form-urlencoded"
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
            ' Prepare for a response from the server
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Get the stream associated with the response.
            Dim receiveStream As Stream = response.GetResponseStream()
            ' Pipes the stream to a higher level stream reader with the required encoding format. 
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            webdata = readStream.ReadToEnd()
            If webdata = "ACTIVE" Then
                Return True
            Else
                Return False
            End If
        Catch e As Exception
            Return False
        End Try
    End Function

    Public Enum APIRequest As Integer
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
        'CorpContainerLog = 34
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
    End Enum

    Public Enum APIResults As Integer
        ReturnedNew = 0
        ReturnedCached = 1
        PageNotFound = 2
        CCPError = 3
        InvalidFeature = 4
        APIServerDownReturnedNull = 5
        APIServerDownReturnedCached = 6
        ReturnedActual = 7
        TimedOut = 8
        UnknownError = 9
    End Enum

    Public Enum APIReturnMethod As Integer
        ReturnStandard = 0
        ReturnCacheOnly = 1
        ReturnActual = 2
    End Enum

End Class
