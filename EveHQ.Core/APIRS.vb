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
Imports System.Net
Imports System.ComponentModel
Imports System.Xml
Imports System.Data
Imports System.IO
Imports System.Windows.Forms
Imports System.Reflection

Public Class APIRS
    Shared context As HttpListenerContext
    Dim listener As System.Net.HttpListener
    Dim response As HttpListenerResponse
    Dim eveData As New DataSet
    Private cAPIRSPort As Integer

    Public Sub RunAPIRS(ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs)
        Dim prefixes(0) As String

        prefixes(0) = "http://*:" & EveHQ.Core.HQ.EveHQSettings.APIRSPort & "/"
        ' URI prefixes are required
        If prefixes Is Nothing OrElse prefixes.Length = 0 Then
            Throw New ArgumentException("prefixes")
        End If

        ' Create a listener and add the prefixes.
        listener = New System.Net.HttpListener()
        For Each s As String In prefixes
            listener.Prefixes.Add(s)
        Next

        Try
            ' Start the listener to begin listening for requests.
            listener.Start()

            Do
                response = Nothing
                Try
                    ' Note: GetContext blocks while waiting for a request.
                    If worker.CancellationPending = True Then
                        e.Cancel = True
                    Else

                        context = listener.GetContext()

                        ' Create the response.
                        response = context.Response
                        Dim responseString As String = ""

                        Select Case context.Request.Url.AbsolutePath.ToUpper
                            ' Check for a "heartbeat" - tests if the APIRS is online and if the API should refer to a backup
                            Case "/HEARTBEAT", "/HEARTBEAT/"
                                responseString = "ACTIVE"
                                ' Select the API Requested
                            Case "/EVE/ALLIANCELIST.XML.ASPX"
                                Dim APIXML As New XmlDocument
                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AllianceList, False)
                                responseString = APIXML.InnerXml
                            Case "/EVE/SKILLTREE.XML.ASPX"
                                Dim APIXML As New XmlDocument
                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.SkillTree, False)
                                responseString = APIXML.InnerXml
                            Case "/EVE/REFTYPES.XML.ASPX"
                                Dim APIXML As New XmlDocument
                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.RefTypes, False)
                                responseString = APIXML.InnerXml
                            Case "/EVE/CONQUERABLESTATIONLIST.XML.ASPX"
                                Dim APIXML As New XmlDocument
                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Conquerables, False)
                                responseString = APIXML.InnerXml
                            Case "/EVE/ERRORLIST.XML.ASPX"
                                Dim APIXML As New XmlDocument
                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.ErrorList, False)
                                responseString = APIXML.InnerXml
                            Case "/MAP/SOVEREIGNTY.XML.ASPX"
                                Dim APIXML As New XmlDocument
                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Sovereignty, False)
                                responseString = APIXML.InnerXml
                            Case "/MAP/JUMPS.XML.ASPX"
                                Dim APIXML As New XmlDocument
                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.MapJumps, False)
                                responseString = APIXML.InnerXml
                            Case "/MAP/KILLS.XML.ASPX"
                                Dim APIXML As New XmlDocument
                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.MapKills, False)
                                responseString = APIXML.InnerXml
                            Case "EVE/CHARACTERID.XML.ASPX"
                                ' Get the POST data
                                Dim postData As SortedList = HTTPPOSTData()
                                If postData.Contains("names") = True Then
                                    Dim names As String = postData("names").ToString
                                    Dim APIXML As New XmlDocument
                                    APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.NameToID, names)
                                    responseString = APIXML.InnerXml
                                Else
                                    responseString = "The HTTP POST data contained incorrect data."
                                End If
                            Case "EVE/CHARACTERNAME.XML.ASPX"
                                ' Get the POST data
                                Dim postData As SortedList = HTTPPOSTData()
                                If postData.Contains("ids") = True Then
                                    Dim ids As String = postData("ids").ToString
                                    Dim APIXML As New XmlDocument
                                    APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IDToName, ids)
                                    responseString = APIXML.InnerXml
                                Else
                                    responseString = "The HTTP POST data contained incorrect data."
                                End If
                            Case "/ACCOUNT/CHARACTERS.XML.ASPX"
                                ' Get the POST data
                                Dim postData As SortedList = HTTPPOSTData()
                                ' Check for userID and APIKey headers
                                Dim response As Integer = CheckForCorrectPOSTDataAccount(postData)
                                Select Case response
                                    Case APIRSResponseTypes.ValidResponse
                                        Dim userID As String = postData("userid").ToString
                                        Dim APIKey As String = postData("apikey").ToString
                                        Dim aAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.Accounts(userID), EveAccount)
                                        Dim APIXML As New XmlDocument
                                        APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Characters, aAccount)
                                        responseString = APIXML.InnerXml
                                    Case APIRSResponseTypes.IncorrectPOSTData
                                        responseString = "The HTTP POST data contained incorrect data."
                                    Case APIRSResponseTypes.InvalidUserID
                                        responseString = "The userID was not valid."
                                    Case APIRSResponseTypes.InvalidAPIKey
                                        responseString = "The APIKey was not valid."
                                End Select
                            Case "CHAR/ACCOUNTBALANCE.XML.ASPX", "CORP/ACCOUNTBALANCE.XML.ASPX", "/CHAR/CHARACTERSHEET.XML.ASPX", "/CORP/CORPORATIONSHEET.XML.ASPX", "/CORP/MEMBERTRACKING.XML.ASPX", "/CHAR/SKILLINTRAINING.XML.ASPX", "/CHAR/ASSETLIST.XML.ASPX", "/CORP/ASSETLIST.XML.ASPX", "/CHAR/KILLLOG.XML.ASPX", "/CORP/KILLLOG.XML.ASPX", "/CHAR/INDUSTRYJOBS.XML.ASPX", "/CORP/INDUSTRYJOBS.XML.ASPX", "/CHAR/MARKETORDERS.XML.ASPX", "/CORP/MARKETORDERS.XML.ASPX", "/CORP/STARBASELIST.XML.ASPX", "/CORP/STARBASEDETAIL.XML.ASPX", "/CHAR/WALLETTRANSACTIONS.XML.ASPX", "/CORP/WALLETTRANSACTIONS.XML.ASPX"
                                ' Get the POST data
                                Dim postData As SortedList = HTTPPOSTData()
                                ' Check for userID and APIKey headers
                                Dim response As Integer = CheckForCorrectPOSTDataChar(postData)
                                Select Case response
                                    Case APIRSResponseTypes.ValidResponse
                                        Dim userID As String = postData("userid").ToString
                                        Dim APIKey As String = postData("apikey").ToString
                                        Dim charID As String = postData("characterid").ToString
                                        Dim beforeTransID As String = ""
                                        If postData.Contains("beforetransid") = True Then
                                            beforeTransID = postData("beforetransid").ToString
                                        End If
                                        Dim aAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.Accounts(userID), EveAccount)
                                        Dim APIXML As New XmlDocument
                                        Select Case context.Request.Url.AbsolutePath.ToUpper
                                            Case "CHAR/ACCOUNTBALANCE.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, aAccount, charID)
                                            Case "CORP/ACCOUNTBALANCE.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, aAccount, charID)
                                            Case "/CHAR/CHARACTERSHEET.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CharacterSheet, aAccount, charID)
                                            Case "/CORP/CORPORATIONSHEET.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpSheet, aAccount, charID)
                                            Case "/CORP/MEMBERTRACKING.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CorpMemberTracking, aAccount, charID)
                                            Case "/CHAR/SKILLINTRAINING.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.SkillTraining, aAccount, charID)
                                            Case "/CHAR/ASSETLIST.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, aAccount, charID)
                                            Case "/CORP/ASSETLIST.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, aAccount, charID)
                                            Case "/CHAR/KILLLOG.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.KillLogChar, aAccount, charID)
                                            Case "/CORP/KILLLOG.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.KillLogCorp, aAccount, charID)
                                            Case "/CHAR/INDUSTRYJOBS.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryChar, aAccount, charID)
                                            Case "/CORP/INDUSTRYJOBS.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IndustryCorp, aAccount, charID)
                                            Case "/CHAR/MARKETORDERS.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersChar, aAccount, charID)
                                            Case "/CORP/MARKETORDERS.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.OrdersCorp, aAccount, charID)
                                            Case "/CORP/STARBASELIST.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.POSList, aAccount, charID)
                                            Case "/CORP/STARBASEDETAIL.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.POSDetails, aAccount, charID)
                                            Case "/CHAR/WALLETTRANSACTIONS.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransChar, aAccount, charID, beforeTransID)
                                            Case "/CORP/WALLETTRANSACTIONS.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletTransCorp, aAccount, charID, beforeTransID)
                                        End Select
                                        responseString = APIXML.InnerXml
                                    Case APIRSResponseTypes.IncorrectPOSTData
                                        responseString = "The HTTP POST data contained incorrect data."
                                    Case APIRSResponseTypes.InvalidUserID
                                        responseString = "The userID was not valid."
                                    Case APIRSResponseTypes.InvalidAPIKey
                                        responseString = "The APIKey was not valid."
                                    Case APIRSResponseTypes.InvalidCharID
                                        responseString = "The charID was not valid."
                                End Select
                            Case "/CORP/STARBASEDETAIL.XML.ASPX"
                                ' Get the POST data
                                Dim postData As SortedList = HTTPPOSTData()
                                ' Check for userID and APIKey headers
                                Dim response As Integer = CheckForCorrectPOSTDataChar(postData)
                                Select Case response
                                    Case APIRSResponseTypes.ValidResponse
                                        Dim userID As String = postData("userid").ToString
                                        Dim APIKey As String = postData("apikey").ToString
                                        Dim charID As String = postData("charid").ToString
                                        Dim itemID As Integer = CInt(postData("itemID").ToString)
                                        Dim aAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.Accounts(userID), EveAccount)
                                        Dim APIXML As New XmlDocument
                                        APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.POSDetails, aAccount, charID, itemID)
                                        responseString = APIXML.InnerXml
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
                            Case "CHAR/WALLETJOURNAL.XML.ASPX", "CORP/WALLETJOURNAL.XML.ASPX"
                                ' Get the POST data
                                Dim postData As SortedList = HTTPPOSTData()
                                ' Check for userID and APIKey headers
                                Dim response As Integer = CheckForCorrectPOSTDataChar(postData)
                                Select Case response
                                    Case APIRSResponseTypes.ValidResponse
                                        Dim userID As String = postData("userid").ToString
                                        Dim APIKey As String = postData("apikey").ToString
                                        Dim charID As String = postData("charid").ToString
                                        Dim accountKey As Integer = CInt(postData("accountkey").ToString)
                                        Dim beforeRefID As String = ""
                                        If postData.Contains("beforerefid") = True Then
                                            beforeRefID = postData("beforerefid").ToString
                                        End If
                                        Dim aAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.Accounts(userID), EveAccount)
                                        Dim APIXML As New XmlDocument
                                        Select Case context.Request.Url.AbsolutePath.ToUpper
                                            Case "CHAR/WALLETJOURNAL.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, aAccount, charID, accountKey, beforeRefID)
                                            Case "CORP/WALLETJOURNAL.XML.ASPX"
                                                APIXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp, aAccount, charID, accountKey, beforeRefID)
                                        End Select
                                        responseString = APIXML.InnerXml
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
                            Case Else
                                responseString &= "Sorry, the API request cannot be handled."
                        End Select
                        Dim buffer() As Byte = System.Text.Encoding.Default.GetBytes(responseString)
                        response.ContentLength64 = buffer.Length
                        Dim output As System.IO.Stream = response.OutputStream
                        output.Write(buffer, 0, buffer.Length)
                    End If
                Catch ex As HttpListenerException
                    Console.WriteLine(ex.Message)
                Finally
                    If response IsNot Nothing Then
                        response.Close()
                    End If
                End Try
            Loop
        Catch ex As HttpListenerException
            Console.WriteLine(ex.Message)
        Finally
            ' Stop listening for requests.
            listener.Close()
        End Try
    End Sub

#Region "APIRS Procedures and Functions"
    Private Function HTTPPOSTData() As SortedList
        Dim stream As System.IO.Stream = context.Request.InputStream
        Dim encoding As System.Text.Encoding = context.Request.ContentEncoding
        Dim reader As System.IO.StreamReader = New System.IO.StreamReader(stream, encoding)
        Dim postdata As String = reader.ReadToEnd
        ' Break the postdata down into seperate items
        Dim PostHeaders As New SortedList
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
        Return PostHeaders
    End Function
    Private Function CheckForCorrectPOSTDataAccount(ByVal postData As SortedList) As Integer
        If postData.Contains("userid".ToLower) = True And postData.Contains("apikey".ToLower) = True Then
            Dim userID As String = postData("userid").ToString
            Dim APIKey As String = postData("apikey").ToString
            ' Check for a matching account
            If EveHQ.Core.HQ.Accounts.Contains(userID) = True Then
                ' Check for a matching API Key
                Dim aAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.Accounts(userID), EveAccount)
                If APIKey = aAccount.APIKey Then
                    Return APIRSResponseTypes.ValidResponse
                Else
                    Return APIRSResponseTypes.InvalidAPIKey
                End If
            Else
                Return APIRSResponseTypes.InvalidUserID
            End If
        Else
            Return APIRSResponseTypes.IncorrectPOSTData
        End If
    End Function
    Private Function CheckForCorrectPOSTDataChar(ByVal postData As SortedList) As Integer
        If postData.Contains("userid".ToLower) = True And postData.Contains("apikey".ToLower) = True And postData.Contains("characterID".ToLower) = True Then
            Dim userID As String = postData("userid").ToString
            Dim APIKey As String = postData("apikey").ToString
            Dim charID As String = postData("characterid").ToString
            ' Check for a matching account
            If EveHQ.Core.HQ.Accounts.Contains(userID) = True Then
                ' Check for a matching API Key
                Dim aAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.Accounts(userID), EveAccount)
                If APIKey = aAccount.APIKey Then
                    ' Check for a character on this account
                    For Each aPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
                        If aPilot.ID = charID Then
                            If aPilot.Account = userID Then
                                Return APIRSResponseTypes.ValidResponse
                            Else
                                Return APIRSResponseTypes.InvalidCharID
                            End If
                        End If
                    Next
                Else
                    Return APIRSResponseTypes.InvalidAPIKey
                End If
            Else
                Return APIRSResponseTypes.InvalidUserID
            End If
        Else
            Return APIRSResponseTypes.IncorrectPOSTData
        End If
    End Function
    Private Function CheckForCorrectPOSTDataAccountKey(ByVal postData As SortedList) As Integer
        If postData.Contains("userid".ToLower) = True And postData.Contains("apikey".ToLower) = True And postData.Contains("characterID".ToLower) = True And postData.Contains("accountKey".ToLower) = True Then
            Dim userID As String = postData("userid").ToString
            Dim APIKey As String = postData("apikey").ToString
            Dim charID As String = postData("characterid").ToString
            Dim accountKey As Integer = CInt(postData("accountkey").ToString)
            ' Check for a matching account
            If EveHQ.Core.HQ.Accounts.Contains(userID) = True Then
                ' Check for a matching API Key
                Dim aAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.Accounts(userID), EveAccount)
                If APIKey = aAccount.APIKey Then
                    ' Check for a character on this account
                    For Each aPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
                        If aPilot.ID = charID Then
                            If aPilot.Account = userID Then
                                If accountKey < 1000 Or accountKey > 1006 Then
                                    Return APIRSResponseTypes.InvalidAccountKey
                                Else
                                    Return APIRSResponseTypes.ValidResponse
                                End If
                            Else
                                Return APIRSResponseTypes.InvalidCharID
                            End If
                        End If
                    Next
                Else
                    Return APIRSResponseTypes.InvalidAPIKey
                End If
            Else
                Return APIRSResponseTypes.InvalidUserID
            End If
        Else
            Return APIRSResponseTypes.IncorrectPOSTData
        End If
    End Function
    Private Function CheckForCorrectPOSTDataPOSDetail(ByVal postData As SortedList) As Integer
        If postData.Contains("userid".ToLower) = True And postData.Contains("apikey".ToLower) = True And postData.Contains("characterID".ToLower) = True And postData.Contains("itemID".ToLower) = True Then
            Dim userID As String = postData("userid").ToString
            Dim APIKey As String = postData("apikey").ToString
            Dim charID As String = postData("characterid").ToString
            Dim itemID As Integer = CInt(postData("itemID").ToString)
            ' Check for a matching account
            If EveHQ.Core.HQ.Accounts.Contains(userID) = True Then
                ' Check for a matching API Key
                Dim aAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.Accounts(userID), EveAccount)
                If APIKey = aAccount.APIKey Then
                    ' Check for a character on this account
                    For Each aPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
                        If aPilot.ID = charID Then
                            If aPilot.Account = userID Then
                                Return APIRSResponseTypes.ValidResponse
                            Else
                                Return APIRSResponseTypes.InvalidCharID
                            End If
                        End If
                    Next
                Else
                    Return APIRSResponseTypes.InvalidAPIKey
                End If
            Else
                Return APIRSResponseTypes.InvalidUserID
            End If
        Else
            Return APIRSResponseTypes.IncorrectPOSTData
        End If
    End Function
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

