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

Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Imports System.IO
Imports System.Xml

''' <summary>
''' Class for creating a web server used to handle Eve API requests to third party applications
''' </summary>
''' <remarks></remarks>
Public Class EveAPIProxy

#Region "Class Variables"
    Dim cPort As Integer = 26002
    Dim cAPICacheLocation As String = Path.Combine(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData, "EveAPICache")
    Dim cMessages As New EveAPIProxyMessageQueue(500) ' Limit the message queue size if we don't intend to process them
    Dim listener As System.Net.HttpListener
    Dim serverThread As Thread
#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets or sets the port number for the Eve API Proxy server to use
    ''' </summary>
    ''' <value></value>
    ''' <returns>An integer containing the port number of the Eve API Proxy server</returns>
    ''' <remarks></remarks>
    Public Property Port() As Integer
        Get
            Return cPort
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then
                cPort = 26002
            Else
                cPort = value
            End If
            ' Create an event
            cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.ServerPortNumberSet, Thread.CurrentThread.GetHashCode, "Eve API Proxy Server Port set to " & cPort.ToString))
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
                ' Create an event
                cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.ServerCacheLocationSet, Thread.CurrentThread.GetHashCode, "Eve API Proxy Server Cache Location set to " & cAPICacheLocation))
            Catch ex As Exception
                Throw New DirectoryNotFoundException
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Gets the current status of the Eve API Proxy Server
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsActive() As Boolean
        Get
            If serverThread IsNot Nothing Then
                Return serverThread.IsAlive
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property Messages() As EveAPIProxyMessageQueue
        Get
            Return cMessages
        End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Starts the Eve API Proxy Server
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StartServer()
        Try
            Dim prefixes(0) As String

            prefixes(0) = "http://*:" & cPort.ToString & "/"
            ' URI prefixes are required
            If prefixes Is Nothing OrElse prefixes.Length = 0 Then
                Throw New ArgumentException("prefixes")
            End If

            ' Create a listener and add the prefixes.
            listener = New System.Net.HttpListener

            For Each s As String In prefixes
                listener.Prefixes.Add(s)
            Next

            listener.Start()

            ' Create an event
            cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.ServerStarted, Thread.CurrentThread.GetHashCode, "Eve API Proxy Server Started"))

            Dim httpSession As New HTTPSession(listener, cMessages)

            serverThread = New Thread(New ThreadStart(AddressOf httpSession.ServerProcessThread))

            serverThread.Start()

        Catch ex As Exception
            Console.WriteLine(ex.StackTrace.ToString())
        End Try
    End Sub

    ''' <summary>
    ''' Stops the Eve API Proxy Server
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopServer()
        Do
            If serverThread.IsAlive Then
                listener.Abort()
                serverThread.Abort()
            End If
        Loop Until serverThread.IsAlive = False
        ' Create an event
        cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.ServerStopped, Thread.CurrentThread.GetHashCode, "Eve API Proxy Server Stopped"))
    End Sub

#End Region

#Region "Class Initialisation Routines"

    ''' <summary>
    ''' Creates a new instance of the Eve API Proxy Server using the specified port number and cache file location
    ''' </summary>
    ''' <param name="PortNumber">The port number to use for the Eve API Proxy Server</param>
    ''' <param name="CacheLocation">The location of the cached XML files</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal PortNumber As Integer, ByVal CacheLocation As String)
        cMessages.Clear()
        ' Create an event
        cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.ServerInitialised, Thread.CurrentThread.GetHashCode, "Eve API Proxy Server Initialised"))
        If PortNumber < 1 Or PortNumber > 65535 Then
            Throw New ArgumentException("EveAPIProxy Port Number must be between 1 and 65535")
        Else
            Me.Port = PortNumber
        End If
        Me.APICacheLocation = CacheLocation
    End Sub

#End Region

    ''' <summary>
    ''' Class to handle the HTTP Session between the Eve API Proxy server and a HTTP client
    ''' </summary>
    ''' <remarks></remarks>
    Private Class HTTPSession
        Private hListener As HttpListener
        Dim cMessages As EveAPIProxyMessageQueue

        Public Sub New(ByVal hListener As HttpListener, ByRef messageQueue As EveAPIProxyMessageQueue)
            Me.hListener = hListener
            cMessages = messageQueue
        End Sub

        Public Sub ServerProcessThread()
            While (True)
                Try
                    Dim context As HttpListenerContext = hListener.GetContext()

                    ' Client information
                    Dim clientInfo As IPEndPoint = context.Request.RemoteEndPoint

                    ' Set Thread for each Web Browser Connection
                    Dim clientThread As New Thread(New ParameterizedThreadStart(AddressOf ClientProcessThread))

                    ' Create an event
                    cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.RequestReceived, clientThread.GetHashCode, "Client (" & clientInfo.Address.ToString() + ":" & clientInfo.Port.ToString & ") requested " & context.Request.Url.AbsolutePath))

                    clientThread.Start(context)

                Catch tex As ThreadAbortException
                    Thread.CurrentThread.Abort()
                Catch ex As Exception
                    Console.WriteLine(ex.Message & ex.StackTrace.ToString())
                    Threading.Thread.CurrentThread.Abort()
                End Try
            End While
        End Sub

        Protected Sub ClientProcessThread(ByVal contextObject As Object)
            Try
                Dim context As HttpListenerContext = CType(contextObject, HttpListenerContext)
                Dim response As HttpListenerResponse = context.Response
                Dim responseString As String = ""
                Dim APIRequest As New EveAPIRequest()
                Dim APIXML As New XmlDocument

                Select Case context.Request.Url.AbsolutePath.ToUpper

                    Case "/STATUS", "/STATUS/"
                        ' Check for the status - tests if the APIRS is online and if the API should refer to a backup
                        responseString = "ACTIVE"
                        ' Select the API Requested
                        ' Create an event
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(EveAPIProxyEventTypes.StatusCheck, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Status Check confirmed to client"))
                    Case "/EVE/ALLIANCELIST.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.AllianceList, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.AllianceList.ToString))
                    Case "/EVE/SKILLTREE.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.SkillTree, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.SkillTree.ToString))
                    Case "/EVE/REFTYPES.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.RefTypes, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.RefTypes.ToString))
                    Case "/EVE/CONQUERABLESTATIONLIST.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.Conquerables, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.Conquerables.ToString))
                    Case "/EVE/ERRORLIST.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.ErrorList, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.ErrorList.ToString))
                    Case "/MAP/SOVEREIGNTY.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.Sovereignty, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.Sovereignty.ToString))
                    Case "/MAP/SOVEREIGNTYSTATUS.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.SovereigntyStatus, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.SovereigntyStatus.ToString))
                    Case "/MAP/JUMPS.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.MapJumps, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.MapJumps.ToString))
                    Case "/MAP/KILLS.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.MapKills, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.MapKills.ToString))
                    Case "/EVE/FACWARTOPSTATS.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.FWTop100, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.FWTop100.ToString))
                    Case "/MAP/FACWARSYSTEMS.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.FWMap, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.FWMap.ToString))
                    Case "/SERVER/SERVERSTATUS.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.ServerStatus, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.ServerStatus.ToString))
                    Case "/EVE/CERTIFICATETREE.XML.ASPX"
                        APIXML = APIRequest.GetAPIXML(APITypes.CertificateTree, APIReturnMethods.ReturnStandard)
                        responseString = APIXML.InnerXml
                        ' Create an event
                        Dim eventID As Integer = APIRequest.LastAPIResult + 100
                        cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Static data returned to client: " & APITypes.CertificateTree.ToString))
                    Case "/EVE/CHARACTERID.XML.ASPX"
                        ' Get the POST data
                        Dim postData As SortedList = HTTPPOSTData(context)
                        If postData.Contains("names") = True Then
                            Dim names As String = postData("names").ToString
                            APIXML = APIRequest.GetAPIXML(APITypes.NameToID, names, APIReturnMethods.ReturnStandard)
                            responseString = APIXML.InnerXml
                            ' Create an event
                            Dim eventID As Integer = APIRequest.LastAPIResult + 100
                            cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Data returned to client for Name to ID conversion"))
                        Else
                            responseString = "The HTTP POST data contained incorrect data."
                        End If
                    Case "/EVE/CHARACTERNAME.XML.ASPX"
                        ' Get the POST data
                        Dim postData As SortedList = HTTPPOSTData(context)
                        If postData.Contains("ids") = True Then
                            Dim ids As String = postData("ids").ToString
                            APIXML = APIRequest.GetAPIXML(APITypes.IDToName, ids, APIReturnMethods.ReturnStandard)
                            responseString = APIXML.InnerXml
                            ' Create an event
                            Dim eventID As Integer = APIRequest.LastAPIResult + 100
                            cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, "Data returned to client for ID to Name conversion"))
                        Else
                            responseString = "The HTTP POST data contained incorrect data."
						End If
					Case "/ACCOUNT/CHARACTERS.XML.ASPX", "/ACCOUNT/ACCOUNTSTATUS.XML.ASPX"
						' Get the POST data
						Dim postData As SortedList = HTTPPOSTData(context)
						' Check for userID and APIKey headers
						Dim responseType As Integer = CheckForCorrectPOSTDataAccount(postData)
						Select Case responseType
							Case APIRSResponseTypes.ValidResponse
								Dim userID As String = postData("userid").ToString
								Dim APIKey As String = postData("apikey").ToString
                                Dim aAccount As New EveAPIAccount(userID, APIKey)
								Select Case context.Request.Url.AbsolutePath.ToUpper
									Case "/ACCOUNT/CHARACTERS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.Characters, aAccount, APIReturnMethods.ReturnStandard)
									Case "/ACCOUNT/ACCOUNTSTATUS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.AccountStatus, aAccount, APIReturnMethods.ReturnStandard)
								End Select
								responseString = APIXML.InnerXml
								' Create an event
								Dim eventID As Integer = APIRequest.LastAPIResult + 100
								cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, APIRequest.LastAPIRequestType.ToString & " returned to client for userID:" & userID))
							Case APIRSResponseTypes.IncorrectPOSTData
								responseString = "The HTTP POST data contained incorrect data."
							Case APIRSResponseTypes.InvalidUserID
								responseString = "The userID was not valid."
							Case APIRSResponseTypes.InvalidAPIKey
								responseString = "The APIKey was not valid."
						End Select
							Case "/CHAR/ACCOUNTBALANCE.XML.ASPX", _
							"/CORP/ACCOUNTBALANCE.XML.ASPX", _
							"/CHAR/CHARACTERSHEET.XML.ASPX", _
							"/CORP/CORPORATIONSHEET.XML.ASPX", _
							"/CORP/MEMBERTRACKING.XML.ASPX", _
							"/CHAR/SKILLINTRAINING.XML.ASPX", _
							"/CHAR/SKILLQUEUE.XML.ASPX", _
							"/CHAR/ASSETLIST.XML.ASPX", _
							"/CORP/ASSETLIST.XML.ASPX", _
							"/CHAR/KILLLOG.XML.ASPX", _
							"/CORP/KILLLOG.XML.ASPX", _
							"/CHAR/INDUSTRYJOBS.XML.ASPX", _
							"/CORP/INDUSTRYJOBS.XML.ASPX", _
							"/CHAR/MARKETORDERS.XML.ASPX", _
							"/CORP/MARKETORDERS.XML.ASPX", _
							"/CORP/STARBASELIST.XML.ASPX", _
							"/CHAR/STANDINGS.XML.ASPX", _
							"/CORP/STANDINGS.XML.ASPX", _
							"/CORP/MEMBERSECURITY.XML.ASPX", _
							"/CORP/MEMBERSECURITYLOG.XML.ASPX", _
							"/CORP/SHAREHOLDERS.XML.ASPX", _
							"/CORP/TITLES.XML.ASPX", _
							"/CHAR/FACWARSTATS.XML.ASPX", _
							"/CORP/FACWARSTATS.XML.ASPX", _
							"/CHAR/MEDALS.XML.ASPX", _
							"/CORP/MEDALS.XML.ASPX", _
							"/CORP/MEMBERMEDALS.XML.ASPX", _
							"/CHAR/MAILINGLISTS.XML.ASPX", _
							"/CHAR/MAILMESSAGES.XML.ASPX", _
							"/CHAR/NOTIFICATIONS.XML.ASPX", _
							 "/CHAR/RESEARCH.XML.ASPX", _
							 "/CORP/OUTPOSTLIST.XML.ASPX", _
							 "/EVE/CHARACTERINFO.XML.ASPX"

						' Get the POST data
						Dim postData As SortedList = HTTPPOSTData(context)
						' Check for userID and APIKey headers
						Dim responseType As Integer = CheckForCorrectPOSTDataChar(postData)
						Select Case responseType
							Case APIRSResponseTypes.ValidResponse
								Dim userID As String = postData("userid").ToString
								Dim APIKey As String = postData("apikey").ToString
								Dim charID As String = postData("characterid").ToString
								Dim beforeTransID As String = ""
								If postData.Contains("beforetransid") = True Then
									beforeTransID = postData("beforetransid").ToString
								End If
								Dim aAccount As New EveAPIAccount(userID, APIKey)
								Select Case context.Request.Url.AbsolutePath.ToUpper
									Case "/CHAR/ACCOUNTBALANCE.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.AccountBalancesChar, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/ACCOUNTBALANCE.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.AccountBalancesCorp, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/CHARACTERSHEET.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.CharacterSheet, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/CORPORATIONSHEET.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.CorpSheet, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/MEMBERTRACKING.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.CorpMemberTracking, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/SKILLINTRAINING.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.SkillTraining, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/SKILLQUEUE.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.SkillQueue, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/ASSETLIST.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.AssetsChar, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/ASSETLIST.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.AssetsCorp, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/KILLLOG.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.KillLogChar, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/KILLLOG.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.KillLogCorp, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/INDUSTRYJOBS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.IndustryChar, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/INDUSTRYJOBS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.IndustryCorp, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/MARKETORDERS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.OrdersChar, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/MARKETORDERS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.OrdersCorp, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/STARBASELIST.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.POSList, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/STANDINGS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.StandingsChar, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/STANDINGS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.StandingsCorp, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/MEMBERSECURITY.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.CorpMemberSecurity, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/MEMBERSECURITYLOG.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.CorpMemberSecurityLog, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/SHAREHOLDERS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.CorpShareholders, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/TITLES.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.CorpTitles, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/FACWARSTATS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.FWStatsChar, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/FACWARSTATS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.FWStatsCorp, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/MEDALS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.MedalsReceived, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/MEDALS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.MedalsAvailable, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/MEMBERMEDALS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.MemberMedals, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/MAILMESSAGES.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.MailMessages, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/NOTIFICATIONS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.Notifications, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/MAILINGLISTS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.MailingLists, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/RESEARCH.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.Research, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/CORP/OUTPOSTLIST.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.OutpostList, aAccount, charID, APIReturnMethods.ReturnStandard)
									Case "/EVE/CHARACTERINFO.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.CharacterInfo, aAccount, charID, APIReturnMethods.ReturnStandard)
								End Select
								responseString = APIXML.InnerXml
								' Create an event
								Dim eventID As Integer = APIRequest.LastAPIResult + 100
								cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, APIRequest.LastAPIRequestType.ToString & " returned to client for userID:" & userID))
							Case APIRSResponseTypes.IncorrectPOSTData
								responseString = "The HTTP POST data contained incorrect data."
							Case APIRSResponseTypes.InvalidUserID
								responseString = "The userID was not valid."
							Case APIRSResponseTypes.InvalidAPIKey
								responseString = "The APIKey was not valid."
							Case APIRSResponseTypes.InvalidCharID
								responseString = "The charID was not valid."
						End Select
					Case "/CHAR/MAILBODIES.XML.ASPX"
						' Get the POST data
						Dim postData As SortedList = HTTPPOSTData(context)
						' Check for userID and APIKey headers
						Dim responseType As Integer = CheckForCorrectPOSTDataChar(postData)
						Select Case responseType
							Case APIRSResponseTypes.ValidResponse
								Dim userID As String = postData("userid").ToString
								Dim APIKey As String = postData("apikey").ToString
								Dim charID As String = postData("characterid").ToString
								Dim messageIDs As Integer = CInt(postData("ids").ToString)
								Dim aAccount As New EveAPIAccount(userID, APIKey)
								Select Case context.Request.Url.AbsolutePath.ToUpper
									Case "/CHAR/MAILBODIES.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.POSDetails, aAccount, charID, messageIDs, APIReturnMethods.ReturnStandard)
								End Select
								responseString = APIXML.InnerXml
								' Create an event
								Dim eventID As Integer = APIRequest.LastAPIResult + 100
								cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, APIRequest.LastAPIRequestType.ToString & " returned to client for userID:" & userID))
							Case APIRSResponseTypes.IncorrectPOSTData
								responseString = "The HTTP POST data contained incorrect data."
							Case APIRSResponseTypes.InvalidUserID
								responseString = "The userID was not valid."
							Case APIRSResponseTypes.InvalidAPIKey
								responseString = "The APIKey was not valid."
							Case APIRSResponseTypes.InvalidCharID
								responseString = "The charID was not valid."
							Case APIRSResponseTypes.InvalidAccountKey
								responseString = "The Account Key was not valid."
						End Select
					Case "/CORP/STARBASEDETAIL.XML.ASPX", "/CORP/OUTPOSTSERVICEDETAIL.XML.ASPX"
						' Get the POST data
						Dim postData As SortedList = HTTPPOSTData(context)
						' Check for userID and APIKey headers
						Dim responseType As Integer = CheckForCorrectPOSTDataChar(postData)
						Select Case responseType
							Case APIRSResponseTypes.ValidResponse
								Dim userID As String = postData("userid").ToString
								Dim APIKey As String = postData("apikey").ToString
								Dim charID As String = postData("characterid").ToString
								Dim itemID As Integer = CInt(postData("itemID").ToString)
								Dim aAccount As New EveAPIAccount(userID, APIKey)
								Select Case context.Request.Url.AbsolutePath.ToUpper
									Case "/CORP/STARBASEDETAIL.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.POSDetails, aAccount, charID, itemID, APIReturnMethods.ReturnStandard)
									Case "/CORP/OUTPOSTSERVICEDETAIL.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.OutpostServiceDetail, aAccount, charID, itemID, APIReturnMethods.ReturnStandard)
								End Select
								responseString = APIXML.InnerXml
								' Create an event
								Dim eventID As Integer = APIRequest.LastAPIResult + 100
								cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, APIRequest.LastAPIRequestType.ToString & " returned to client for userID:" & userID))
							Case APIRSResponseTypes.IncorrectPOSTData
								responseString = "The HTTP POST data contained incorrect data."
							Case APIRSResponseTypes.InvalidUserID
								responseString = "The userID was not valid."
							Case APIRSResponseTypes.InvalidAPIKey
								responseString = "The APIKey was not valid."
							Case APIRSResponseTypes.InvalidCharID
								responseString = "The charID was not valid."
							Case APIRSResponseTypes.InvalidAccountKey
								responseString = "The Account Key was not valid."
						End Select
					Case "/CHAR/WALLETJOURNAL.XML.ASPX", _
					"/CORP/WALLETJOURNAL.XML.ASPX", _
					"/CHAR/WALLETTRANSACTIONS.XML.ASPX", _
					"/CORP/WALLETTRANSACTIONS.XML.ASPX"
						' Get the POST data
						Dim postData As SortedList = HTTPPOSTData(context)
						' Check for userID and APIKey headers
						Dim responseType As Integer = CheckForCorrectPOSTDataChar(postData)
						Select Case responseType
							Case APIRSResponseTypes.ValidResponse
								Dim userID As String = postData("userid").ToString
								Dim APIKey As String = postData("apikey").ToString
								Dim charID As String = postData("characterid").ToString
								Dim accountKey As Integer = CInt(postData("accountkey").ToString)
								Dim beforeRefID As String = ""
								If postData.Contains("beforerefid") = True Then
									beforeRefID = postData("beforerefid").ToString
								End If
								Dim aAccount As New EveAPIAccount(userID, APIKey)
								Select Case context.Request.Url.AbsolutePath.ToUpper
									Case "/CHAR/WALLETJOURNAL.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.WalletJournalChar, aAccount, charID, accountKey, beforeRefID, APIReturnMethods.ReturnStandard)
									Case "/CORP/WALLETJOURNAL.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.WalletJournalCorp, aAccount, charID, accountKey, beforeRefID, APIReturnMethods.ReturnStandard)
									Case "/CHAR/WALLETTRANSACTIONS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.WalletTransChar, aAccount, charID, accountKey, beforeRefID, APIReturnMethods.ReturnStandard)
									Case "/CORP/WALLETTRANSACTIONS.XML.ASPX"
										APIXML = APIRequest.GetAPIXML(APITypes.WalletTransCorp, aAccount, charID, accountKey, beforeRefID, APIReturnMethods.ReturnStandard)
								End Select
								responseString = APIXML.InnerXml
								' Create an event
								Dim eventID As Integer = APIRequest.LastAPIResult + 100
								cMessages.Enqueue(New EveAPIProxyEvent(CType(eventID, EveAPIProxyEventTypes), Thread.CurrentThread.GetHashCode, APIRequest.LastAPIRequestType.ToString & " returned to client for userID:" & userID))
							Case APIRSResponseTypes.IncorrectPOSTData
								responseString = "The HTTP POST data contained incorrect data."
								' Create an event
								cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.IncorrectPOSTData, Thread.CurrentThread.GetHashCode, "Incorrect HTTP POST data provided"))
							Case APIRSResponseTypes.InvalidUserID
								responseString = "The userID was not valid."
								' Create an event
								cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.InvalidUserID, Thread.CurrentThread.GetHashCode, "Invalid UserID provided"))
							Case APIRSResponseTypes.InvalidAPIKey
								responseString = "The APIKey was not valid."
								' Create an event
								cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.InvalidAPIKey, Thread.CurrentThread.GetHashCode, "Invalid API Key provided"))
							Case APIRSResponseTypes.InvalidCharID
								responseString = "The charID was not valid."
								' Create an event
								cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.InvalidCharID, Thread.CurrentThread.GetHashCode, "Invalid Character ID provided"))
							Case APIRSResponseTypes.InvalidAccountKey
								responseString = "The Account Key was not valid."
								' Create an event
								cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.InvalidAccountKey, Thread.CurrentThread.GetHashCode, "Invalid Account Key provided"))
						End Select
					Case Else
						responseString &= "Sorry, the API request cannot be handled."
						' Create an event
						cMessages.Enqueue(New EveAPIProxyEvent(EveAPIProxyEventTypes.UnknownRequest, Thread.CurrentThread.GetHashCode, "Unknown request returned to client"))
				End Select

				Dim buffer() As Byte = System.Text.Encoding.Default.GetBytes(responseString)
				response.ContentLength64 = buffer.Length
				Dim output As System.IO.Stream = response.OutputStream
				output.Write(buffer, 0, buffer.Length)
			Catch hEx As HttpListenerException
                ' Do nothing
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace.ToString())
                Thread.CurrentThread.Abort()
            End Try
        End Sub

#Region "APIRS Procedures and Functions"

        ''' <summary>
        ''' Converts the HTTP POST data into a SortedList for later processing
        ''' </summary>
        ''' <returns>A SortedList containing the HTTP POST data</returns>
        ''' <remarks></remarks>
        Private Function HTTPPOSTData(ByVal context As HttpListenerContext) As SortedList
            Dim PostHeaders As New SortedList
            ' See if we have a querystring
            If context.Request.QueryString.Count = 0 Then
                Dim stream As System.IO.Stream = context.Request.InputStream
                Dim encoding As System.Text.Encoding = context.Request.ContentEncoding
                Dim reader As System.IO.StreamReader = New System.IO.StreamReader(stream, encoding)
                Dim postdata As String = reader.ReadToEnd
                ' Break the postdata down into seperate items
                If postdata <> "" Then
                    Dim postDataLines() As String = postdata.Split("&".ToCharArray)
                    For Each postDataLine As String In postDataLines
                        If postDataLine <> "" Then
                            Dim postDataItems() As String = postDataLine.Split("=".ToCharArray)
                            PostHeaders.Add(postDataItems(0).ToLower, postDataItems(1))
                        End If
                    Next
                End If
                Dim userID As String = postdata
                Dim APIKey As String = context.Request.Headers("APIKey")
            Else
                For Each item As String In context.Request.QueryString.Keys
                    PostHeaders.Add(item.ToLower, context.Request.QueryString(item))
                Next
            End If
            Return PostHeaders
        End Function

        ''' <summary>
        ''' Function to check the HTTP POST data of a received API Request
        ''' </summary>
        ''' <param name="postData">The actual POST data as a SortedList of keys/values</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckForCorrectPOSTDataAccount(ByVal postData As SortedList) As Integer
            If postData.Contains("userid".ToLower) = True And postData.Contains("apikey".ToLower) = True Then
                Dim userID As String = postData("userid").ToString
                Dim APIKey As String = postData("apikey").ToString
                Return APIRSResponseTypes.ValidResponse
            Else
                Return APIRSResponseTypes.IncorrectPOSTData
            End If
        End Function

        ''' <summary>
        ''' Function to check the HTTP POST data of a received API Request
        ''' </summary>
        ''' <param name="postData">The actual POST data as a SortedList of keys/values</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckForCorrectPOSTDataChar(ByVal postData As SortedList) As Integer
            If postData.Contains("userid".ToLower) = True And postData.Contains("apikey".ToLower) = True And postData.Contains("characterID".ToLower) = True Then
                Dim userID As String = postData("userid").ToString
                Dim APIKey As String = postData("apikey").ToString
                Dim charID As String = postData("characterid").ToString
                Return APIRSResponseTypes.ValidResponse
            Else
                Return APIRSResponseTypes.IncorrectPOSTData
            End If
        End Function

        ''' <summary>
        ''' Function to check the HTTP POST data of a received API Request
        ''' </summary>
        ''' <param name="postData">The actual POST data as a SortedList of keys/values</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckForCorrectPOSTDataAccountKey(ByVal postData As SortedList) As Integer
            If postData.Contains("userid".ToLower) = True And postData.Contains("apikey".ToLower) = True And postData.Contains("characterID".ToLower) = True And postData.Contains("accountKey".ToLower) = True Then
                Dim userID As String = postData("userid").ToString
                Dim APIKey As String = postData("apikey").ToString
                Dim charID As String = postData("characterid").ToString
                Dim accountKey As Integer = CInt(postData("accountkey").ToString)
                If accountKey < 1000 Or accountKey > 1006 Then
                    Return APIRSResponseTypes.InvalidAccountKey
                Else
                    Return APIRSResponseTypes.ValidResponse
                End If
            Else
                Return APIRSResponseTypes.IncorrectPOSTData
            End If
        End Function

        ''' <summary>
        ''' Function to check the HTTP POST data of a received API Request
        ''' </summary>
        ''' <param name="postData">The actual POST data as a SortedList of keys/values</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckForCorrectPOSTDataPOSDetail(ByVal postData As SortedList) As Integer
            If postData.Contains("userid".ToLower) = True And postData.Contains("apikey".ToLower) = True And postData.Contains("characterID".ToLower) = True And postData.Contains("itemID".ToLower) = True Then
                Dim userID As String = postData("userid").ToString
                Dim APIKey As String = postData("apikey").ToString
                Dim charID As String = postData("characterid").ToString
                Dim itemID As Integer = CInt(postData("itemID").ToString)
                Return APIRSResponseTypes.ValidResponse
            Else
                Return APIRSResponseTypes.IncorrectPOSTData
            End If
        End Function

        ''' <summary>
        ''' A list of possible responses to checking the HTTP POST data
        ''' </summary>
        ''' <remarks></remarks>
        Private Enum APIRSResponseTypes
            ValidResponse = 0
            IncorrectPOSTData = 1
            InvalidUserID = 2
            InvalidAPIKey = 3
            InvalidCharID = 4
            InvalidAccountKey = 5
        End Enum
#End Region

    End Class

End Class

''' <summary>
''' Class for holding information about events within the Eve API Proxy class
''' </summary>
''' <remarks></remarks>
Public Class EveAPIProxyEvent
    Dim cEventType As EveAPIProxyEventTypes
    Dim cEventRef As Integer
    Dim cEventDate As Date
    Dim cDescription As String

    Public ReadOnly Property EventType() As EveAPIProxyEventTypes
        Get
            Return cEventType
        End Get
    End Property

    Public ReadOnly Property EventRef() As Integer
        Get
            Return cEventRef
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the date and time when an event occured
    ''' </summary>
    ''' <value></value>
    ''' <returns>A date indicating the timing of an event</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property EventDate() As Date
        Get
            Return cEventDate
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the description of an Eve API Proxy event
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing a description of the Eve API Proxy event</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Description() As String
        Get
            Return cDescription
        End Get
    End Property

    ''' <summary>
    ''' Initialises a new EveAPIProxy Event class using the specified type, reference and description
    ''' </summary>
    ''' <param name="Type">The Type of event</param>
    ''' <param name="EventReference">A reference for the event</param>
    ''' <param name="EventDescription">A description of the event</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Type As EveAPIProxyEventTypes, ByVal EventReference As Integer, ByVal EventDescription As String)
        cEventType = Type
        cEventRef = EventReference
        cEventDate = Now
        cDescription = EventDescription
    End Sub
End Class

Public Enum EveAPIProxyEventTypes As Integer
    ServerInitialised = 0
    ServerCacheLocationSet = 1
    ServerPortNumberSet = 2
    ServerStarted = 3
    ServerStopped = 4
    RequestReceived = 5
    UnknownRequest = 6
    StatusCheck = 7
    ValidResponse = 10
    IncorrectPOSTData = 11
    InvalidUserID = 12
    InvalidAPIKey = 13
    InvalidCharID = 14
    InvalidAccountKey = 15
    ReturnedNew = 100
    ReturnedCached = 101
    PageNotFound = 102
    CCPError = 103
    InvalidFeature = 104
    APIServerDownReturnedNull = 105
    APIServerDownReturnedCached = 106
    ReturnedActual = 107
    TimedOut = 108
    UnknownError = 109
    InternalCodeError = 110
End Enum


