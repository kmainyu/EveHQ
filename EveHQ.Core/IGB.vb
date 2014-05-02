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

Imports System.Net
Imports System.ComponentModel
Imports System.IO
Imports EveHQ.EveData
Imports EveHQ.Core.CoreReports
Imports EveHQ.Core.Requisitions
Imports System.Reflection
Imports System.Text
Imports System.Windows.Forms
Imports System.Web

Public Class IGB
    Shared context As HttpListenerContext
    Public Listener As HttpListener
    Dim _response As HttpListenerResponse
    Shared timeStart, timeEnd As DateTime
    Shared timeTaken As TimeSpan
    Private _tqPlayers, _sisiPlayers As Long

    Public Property TqPlayers() As Long
        Get
            Return _tqPlayers
        End Get
        Set(ByVal value As Long)
            _tqPlayers = value
        End Set
    End Property
    Public Property SisiPlayers() As Long
        Get
            Return _sisiPlayers
        End Get
        Set(ByVal value As Long)
            _sisiPlayers = value
        End Set
    End Property
    Public Sub RunIGB(ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs)
        Dim prefixes(0) As String
        prefixes(0) = "http://localhost:" & HQ.Settings.IgbPort & "/"

        ' URI prefixes are required,
        If prefixes Is Nothing OrElse prefixes.Length = 0 Then
            Throw New ArgumentException("prefixes")
        End If

        ' Create a listener and add the prefixes.
        Listener = New HttpListener()
        For Each s As String In prefixes
            Listener.Prefixes.Add(s)
        Next

        ' Check our IGB Access List is complete
        Call CheckAllIGBAccessRights()

        Try
            ' Start the listener to begin listening for requests.
            Listener.Start()

            ' Set the number of requests this application will handle.

            Do
                _response = Nothing
                Try
                    ' Note: GetContext blocks while waiting for a request.
                    If worker.CancellationPending = True Then
                        e.Cancel = True
                        Exit Do
                    Else

                        context = Listener.GetContext

                        ' Create the response.
                        _response = context.Response
                        Dim responseString As String = ""
                        ' Start the page generation timer
                        timeStart = Now

                        Select Case context.Request.Url.AbsolutePath.ToUpper
                            Case "", "/"
                                responseString &= RedirectHome()
                            Case "/HOME", "/HOME/"
                                responseString &= CreateHome()
                            Case "/ITEMDB", "/ITEMDB/"
                                If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Item Database") = True Then
                                    responseString &= CreateHTMLItemDB()
                                Else
                                    responseString &= CreateHome()
                                End If
                            Case "/MARKETTEST", "/MARKETTEST/"
                                responseString &= CreateMarketTest()
                            Case "/SEARCHRESULTS", "SEARCHRESULTS"
                                responseString &= CreateHTMLSearchResultsSQL()
                            Case "/HEADERS", "/HEADERS/"
                                responseString &= CreateHeaders()
                            Case "/REPORTS", "/REPORTS/"
                                If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Reports - Main Menu") = True Then
                                    responseString &= CreateReports()
                                Else
                                    responseString &= CreateHome()
                                End If
                            Case "/REPORTS/SKILLLEVELS", "/REPORTS/SKILLLEVELS/"
                                If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Report - Skill Level Table") = True Then
                                    responseString &= CreateSPReport()
                                Else
                                    responseString &= CreateHome()
                                End If
                            Case "/REPORTS/CHARACTER", "/REPORTS/CHARACTER/"
                                If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Report - Character Individual Reports") = True Then
                                    responseString &= ShowCharReports()
                                Else
                                    responseString &= CreateHome()
                                End If
                            Case "/REPORTS/CHARSUMM", "/REPORTS/CHARASUMM/"
                                If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Report - Character Summary") = True Then
                                    responseString &= ShowCharSummary()
                                Else
                                    responseString &= CreateHome()
                                End If
                            Case "/REPORTS/CHARREPORT", "/REPORTS/CHARREPORT/"
                                If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Report - Character Individual Reports") = True Then
                                    responseString &= CreateCharReport()
                                Else
                                    responseString &= CreateHome()
                                End If
                            Case "/REPORTS/CHARREPORT/QUEUES", "/REPORTS/CHARREPORT/QUEUES/"
                                If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Report - Character Individual Reports") = True Then
                                    responseString &= CreateQueueReport()
                                Else
                                    responseString &= CreateHome()
                                End If
                            Case "/LOGO.JPG"
                                context.Response.ContentType = "image/jpeg"
                                My.Resources.EveHQ_IGBLogo.Save(Path.Combine(HQ.ImageCacheFolder, "logo.jpg"))
                                responseString = GetImage(Path.Combine(HQ.ImageCacheFolder, "logo.jpg"))
                            Case "/TEST.GIF"
                                context.Response.ContentType = "image/gif"
                                responseString = GetImage("c:/test.gif")
                            Case "/TEST.PNG"
                                context.Response.ContentType = "image/jpeg"
                                responseString = GetImage("c:/test.png")
                                'NB Other examples!
                                'Case "/TIME", "/TIME/"
                                '    responseString &= IGBHTMLHeader(context, "EveHQ Time Check")
                                '    responseString &= "<p>The time is currently " & Format(Now, "dd/MM/yyyy HH:mm") & "</p>"
                                '    responseString &= IGBHTMLFooter(context)
                                'Case "/FORM", "/FORM/"
                                '    responseString &= IGBHTMLHeader(context, "Test Form")
                                '    responseString &= "<br><br><form method=""GET"" action=""/form2"">"
                                '    responseString &= "<input type=""text"" name=""wtf"">  "
                                '    responseString &= "<input type=""submit"" value=""Show Me!"">"
                                '    responseString &= "</form><br><br>"
                                '    responseString &= IGBHTMLFooter(context)
                                'Case "/FORM2", "/FORM2/"
                                '    responseString &= IGBHTMLHeader(context, "Test Form Results")
                                '    responseString &= "<HTML><BODY>Well done, you clicked the button!<br><br>"
                                '    If context.Request.QueryString.Count = 0 Then
                                '        responseString &= "Nothing to note!"
                                '    Else
                                '        responseString &= "You typed in: " & context.Request.QueryString(0) & "<br><br>"
                                '    End If
                                '    responseString &= IGBHTMLFooter(context)
                            Case Else
                                ' Check for requisitions
                                If context.Request.Url.AbsolutePath.ToUpper.StartsWith("/REQS") Or context.Request.Url.AbsolutePath.ToUpper.StartsWith("/REQS/") Then
                                    If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Requisitions") = True Then
                                        responseString = RequisitionIGB.Response(context)
                                    Else
                                        responseString = CreateHome()
                                    End If
                                Else
                                    ' Check if this is a plugin string
                                    Dim igbPlugin As Boolean = False
                                    For Each plugInInfo As EveHQPlugIn In HQ.Plugins.Values
                                        If plugInInfo.RunInIGB Then
                                            If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData(plugInInfo.Name) = True Then
                                                Dim testName As String = plugInInfo.Name.Replace(" ", "")
                                                If context.Request.Url.AbsolutePath.ToUpper.StartsWith("/" & testName.ToUpper) Then
                                                    igbPlugin = True
                                                    Dim plugInResponse As String
                                                    Dim myAssembly As Assembly = Assembly.LoadFrom(plugInInfo.FileName)
                                                    Dim t As Type = myAssembly.GetType(plugInInfo.FileType)
                                                    plugInInfo.Instance = CType(Activator.CreateInstance(t), IEveHQPlugIn)
                                                    Dim runPlugIn As IEveHQPlugIn = plugInInfo.Instance
                                                    plugInResponse = runPlugIn.IGBService(context)
                                                    If plugInResponse Is Nothing Then
                                                        plugInResponse = "The module '" & plugInInfo.Name & "' failed to return a valid response."
                                                    End If
                                                    responseString &= plugInResponse
                                                End If
                                            End If
                                        End If
                                    Next
                                    If igbPlugin = False Then
                                        responseString &= IGBHTMLHeader(context, "EveHQ IGB Site", 0)
                                        responseString &= "Sorry, the page you are looking for cannot be found.<br><br>"
                                        responseString &= IGBHTMLFooter(context)
                                    End If
                                End If
                        End Select

                        Dim buffer() As Byte = Encoding.Default.GetBytes(responseString)
                        _response.ContentLength64 = buffer.Length
                        _response.ContentType = "text/html;q=0.9;*/*;q=0.5"
                        Dim output As Stream = _response.OutputStream
                        output.Write(buffer, 0, buffer.Length)
                        output.Flush()
                        output.Close()
                        output.Dispose()
                    End If
                Catch ex As HttpListenerException
                    'Console.WriteLine(ex.Message)
                Finally
                    If _response IsNot Nothing Then
                        _response.Close()
                    End If
                End Try
            Loop
        Catch ex As HttpListenerException
            MessageBox.Show("There was an error using the IGB server. The error was: " & ex.Message, "IGB Server Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            ' Stop listening for requests.
            Listener.Close()
        End Try
    End Sub
    Public Shared Function GetImage(ByVal localFile As String) As String
        'Create a file stream from an existing file.
        Dim fi As New FileInfo(localFile)
        Dim fs As FileStream = fi.OpenRead()
        'Read bytes into an array from the specified file.
        Dim nBytes As Integer = CInt(fi.Length)
        Dim byteArray(nBytes - 1) As Byte
        fs.Read(byteArray, 0, nBytes)
        fs.Close()
        fs.Dispose()
        Return Encoding.Default.GetString(byteArray)
    End Function
    Public Shared Sub CheckAllIGBAccessRights()
        Dim igbAccessFile As String = My.Resources.IGBAccess.ToString
        Dim igbAccessList() As String = igbAccessFile.Split(ControlChars.CrLf.ToCharArray)
        For Each igbFeature As String In igbAccessList
            If igbFeature.Trim <> "" Then
                Dim igbFeatureData() As String = igbFeature.Split(",".ToCharArray)
                If HQ.Settings.IgbAllowedData.ContainsKey(igbFeatureData(0)) = False Then
                    HQ.Settings.IgbAllowedData.Add(igbFeatureData(0), CBool(igbFeatureData(1)))
                End If
            End If
        Next
    End Sub

#Region "IGB Procedures and Functions"

    Public Shared Function IGBHTMLHeader(ByVal igbContext As HttpListenerContext, ByVal strTitle As String, ByVal pageRefreshPeriod As Integer) As String
        Dim strHTML As String = ""
        strHTML &= "<HTML>"
        strHTML &= "<HEAD><TITLE>" & strTitle & "</TITLE>"
        strHTML &= "<STYLE><!--"
        strHTML &= "BODY { font-family: Arial, Tahoma; font-size: 10px; bgcolor: #000000; background: #000000; color: #ffffff; }"
        strHTML &= "TD, P, FORM { font-family: Arial, Tahoma; font-size: 10px;}"
        strHTML &= ".attbody { font-family: Arial, Tahoma; font-size: 10px; color: #ffffff; }"
        strHTML &= ".atthead { font-family: Tahoma, Arial; font-size: 8px; color: #ffffff; font-variant: small-caps; }"
        strHTML &= ".thead { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff; font-variant: small-caps; background-color: #444444; }"
        strHTML &= ".footer { font-family: Tahoma, Arial; font-size: 9px; color: #ffffff; font-variant: small-caps; }"
        strHTML &= ".title { font-family: Tahoma, Arial; font-size: 20px; color: #ffffff; font-variant: small-caps; }"
        strHTML &= ".tbl { width: 800px; color: #ffffff; }"
        strHTML &= "#wrapper {overflow: auto; height: 100%; width:820px; margin-left:auto; margin-right:auto;}"
        strHTML &= "--></STYLE>"
        If pageRefreshPeriod <> 0 Then
            strHTML &= "<META HTTP-EQUIV=""REFRESH"" CONTENT=""" & pageRefreshPeriod.ToString & """>"
        End If
        strHTML &= "</HEAD>"
        If pageRefreshPeriod <> 0 Then
            strHTML &= "<BODY onLoad=""CCPEVE.showMarketDetails(638)"" link=#ff8888 alink=#ff8888 vlink=#ff8888>"
        Else
            If igbContext.Request.Headers("EVE_CHARNAME") <> "" Then
                strHTML &= "<BODY link=#ff8888 alink=#ff8888 vlink=#ff8888>"
            Else
                strHTML &= "<BODY onLoad=""CCPEVE.requestTrust('http://" & igbContext.Request.Headers("Host") & "')"" link=#ff8888 alink=#ff8888 vlink=#ff8888>"
            End If
        End If

        ' Draw header table
        strHTML &= "<table border=0 width=100%>"

        ' Draw logo
        strHTML &= "<tr><td>"
        strHTML &= "<img src=""http://" & igbContext.Request.Headers("Host") & "/logo.jpg"" alt=""EveHQ Logo"" />"
        strHTML &= "</td>"

        ' Draw navigation area
        strHTML &= "<td><table width=100%>"

        ' Draw "logged in" line
        strHTML &= "<tr><td align=right>"
        If igbContext.Request.Headers("EVE_CHARNAME") = "" Then
            strHTML &= "Welcome Pilot!"
        Else
            strHTML &= "Greetings " & igbContext.Request.Headers("EVE_CHARNAME")
        End If
        strHTML &= "</td></tr>"

        ' Draw menu

        strHTML &= "<tr><td align=right>"

        strHTML &= "<hr><a href=/>Home</a>  | "

        If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Item Database") = True Then
            strHTML &= " <a href=/itemDB>Item Database</a>  | "
        End If

        If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Reports - Main Menu") = True Then
            strHTML &= " <a href=/reports>Reports</a>  | "
        End If

        If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Requisitions") = True Then
            If CustomDataFunctions.CountRequisitions > 0 Then
                strHTML &= " <a href=/reqs>Requisitions</a> | "
            End If
        End If

        strHTML &= " <a href=/headers>IGB Headers</a>"

        For Each plugInInfo As EveHQPlugIn In HQ.Plugins.Values
            If plugInInfo.RunInIGB = True Then
                ' Check if in the access list, and if not, default to allowable
                If HQ.Settings.IgbAllowedData.ContainsKey(plugInInfo.Name) = False Then
                    HQ.Settings.IgbAllowedData.Add(plugInInfo.Name, True)
                End If
                If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData(plugInInfo.Name) = True Then
                    strHTML &= "  |  <a href=/" & plugInInfo.Name.Replace(" ", "") & ">" & plugInInfo.MainMenuText & "</a>"
                End If
            End If
        Next
        strHTML &= "</td></tr>"

        ' Draw search area
        strHTML &= "<tr><td align=right>"
        strHTML &= "<form method=""GET"" action=""/searchResults"">"
        If HQ.Settings.IgbAllowedData("Item Database") = True Then
            strHTML &= "Search Item Database:  "
            strHTML &= "<input type=""text"" name=""str"">"
            strHTML &= "<input type=""submit"" value=""Search""></form>"
        End If
        strHTML &= "</td></tr></table>"

        strHTML &= "</td></tr></table>"

        strHTML &= "<hr>"

        Return strHTML
    End Function
    Public Shared Function IGBHTMLFooter(ByVal igbContext As HttpListenerContext) As String
        Dim strHTML As String = ""
        strHTML &= "<table width=100% border=0><tr><td width=100% align=center>"
        strHTML &= "<hr>"
        strHTML &= "<p align=""center"">Created by " & My.Application.Info.ProductName & " v" & My.Application.Info.Version.ToString
        timeEnd = Now
        timeTaken = timeEnd - timeStart
        strHTML &= " (Generated in " & timeTaken.TotalSeconds.ToString("0.00000") & "s)</p>"
        strHTML &= "</td></tr></table></BODY></HTML>"
        Return strHTML
    End Function
    Private Function RedirectHome() As String
        context.Response.Redirect("/home/")
        Return ""
    End Function
    Private Function CreateHome() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ IGB Home", 0)
        strHTML &= "<p>Welcome to the EveHQ In-Game Browser (IGB) Server!</p>"
        strHTML &= "<p>This server will give you access to the wealth of information that is the Eve Online database and present it in tabular form for easy reading.</p>"
        If context.Request.UserAgent.StartsWith("EVE-IGB") Then
            strHTML &= "<p>If you have any questions or suggestions, please contact <a href='evemail:Vessper' SUBJECT='EveHQ IGB'>Vessper</a> via Eve-mail.</p>"
        Else
            strHTML &= "<p>If you have any questions or suggestions, please contact <a href='mailto:vessper@EveHQ.net'>Vessper</a> via E-mail.</p>"
        End If
        strHTML &= "<p></p>"
        strHTML &= "<p>Happy browsing and fly safe!</p>"
        strHTML &= "<p>  - Vessper</p>"
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateHTMLItemDB() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ IGB Item Database", 0)
        strHTML &= CreateNavPaneSQL()
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateHeaders() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ IGB Header Info", 0)
        If context.Request.UserAgent.StartsWith("EVE-IGB") Then
            context.Response.Headers.Add("refresh:sessionchange;URL=/HEADERS")
        End If
        strHTML &= "<p>If viewed through the Eve IGB, this page will show a list of the Eve specific browser headers which can be used to identify certain Pilot information. If a site is trusted by "
        strHTML &= "the Eve IGB (shown by the EVE_TRUSTED header), this information is made available to the web server (the remote site).</p><p>"
        For a As Integer = 0 To context.Request.Headers.Count - 1
            strHTML &= context.Request.Headers.Keys(a) & " : " & context.Request.Headers.Item(a) & "<br>"
        Next
        strHTML &= "</p><br><br><br>"
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateReports() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ Reports", 0)
        strHTML &= "<p>Please select a report from the list below:</p><br>"
        If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Report - Skill Level Table") = True Then
            strHTML &= "<br><a href=/reports/skilllevels>Skill Level Table</a><br>"
        End If
        If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Report - Character Summary") = True Then
            strHTML &= "<br><a href=/reports/charsumm>Character Summary</a><br>"
        End If
        If HQ.Settings.IgbFullMode Or HQ.Settings.IgbAllowedData("Report - Character Individual Reports") = True Then
            strHTML &= "<br><a href=/reports/character>Individual Character Reports</a></p>"
        End If
        strHTML &= "<br><br><br>"
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateSPReport() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Skill Level Table", 0)
        strHTML &= Reports.HTMLTitle("Skill Level Table")
        strHTML &= Reports.SPSummary
        strHTML &= Reports.HTMLFooter
        Return strHTML
    End Function
    Private Function ShowCharSummary() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Pilot Summary", 0)
        strHTML &= Reports.HTMLTitle("Pilot Summary")
        strHTML &= Reports.CharSummary()
        strHTML &= Reports.HTMLFooter
        Return strHTML
    End Function
    Private Function ShowCharReports() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Generate Character Report", 0)
        strHTML &= CreateCharReports()
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateHTMLSearchResultsSQL() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ Search Results", 0)
        If context.Request.QueryString.Count = 0 Or context.Request.QueryString.Item("str") = "" Then
            strHTML &= "<p>Please enter a valid search parameter</p>"
        Else
            Try
                Dim searchFor As String = (HttpUtility.UrlDecode(context.Request.QueryString.Item("str"))).Trim
                Dim strSearch As String = searchFor.ToLower
                Dim results As New SortedList(Of String, Integer)
                For Each item As String In StaticData.TypeNames.Keys
                    If item.ToLower.Contains(strSearch) Then
                        results.Add(item, StaticData.TypeNames(item))
                    End If
                Next
                strHTML &= "<p>Search results for """ & searchFor & """ (" & results.Count.ToString & " items):</p>"
                For Each typeName As String In results.Keys
                    strHTML &= "<a href=/itemDB/?view=t&id=" & results(typeName) & ">" & typeName & "</a><br>"
                Next
                strHTML &= "<br>"
            Catch e As Exception
                strHTML &= "<p>Unable to access database...please check location and integrity.</p>"
            End Try
        End If
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateNavPaneSQL() As String
        Dim strHTML As String = ""
        Dim dbNavigator As String = "Database Location: "

        Try
            strHTML &= "<table width=800px class=tbl border=0><tr><td width=100%>"
            Select Case context.Request.QueryString.Item("view")
                Case "c", ""
                    dbNavigator &= "<a href=/itemDB/>Home</a>"
                    strHTML &= "<p>" & dbNavigator & "</p>"
                    strHTML &= "</td></tr></table>"
                    Dim categories As New SortedList(Of String, Integer)
                    For Each catID As Integer In StaticData.TypeCats.Keys
                        categories.Add(StaticData.TypeCats(catID), catID)
                    Next
                    For Each catName As String In categories.Keys
                        strHTML &= "<a href=/itemDB/?view=g&id=" & categories(catName) & ">" & catName & "</a><br>"
                    Next
                Case "g"
                    Dim catID As Integer = CInt(context.Request.QueryString.Item("id"))
                    dbNavigator &= "<a href=/itemDB/>Home</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=g&id=" & catID & ">" & StaticData.TypeCats(catID) & "</a>"
                    strHTML &= "<p>" & dbNavigator & "</p>"
                    strHTML &= "</td></tr></table>"
                    For Each groupID As Integer In StaticData.GetGroupsInCategory(catID)
                        strHTML &= "<a href=/itemDB/?view=i&id=" & groupID & ">" & StaticData.TypeGroups(groupID) & "</a><br>"
                    Next
                Case "i"
                    Dim groupID As Integer = CInt(context.Request.QueryString.Item("id"))
                    Dim catID As Integer = StaticData.GroupCats(groupID)
                    dbNavigator &= "<a href=/itemDB/>Home</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=g&id=" & catID & ">" & StaticData.TypeCats(catID) & "</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=i&id=" & groupID & ">" & StaticData.TypeGroups(groupID) & "</a>"
                    strHTML &= "<p>" & dbNavigator & "</p>"
                    strHTML &= "</td></tr></table>"
                    For Each typeName As String In StaticData.GetSortedItemListInGroup(CInt(groupID)).Keys
                        strHTML &= "<a href=/itemDB/?view=t&id=" & StaticData.TypeNames(typeName) & ">" & typeName & "</a><br>"
                    Next
                Case "t"
                    Dim typeID As Integer = CInt(context.Request.QueryString.Item("id"))
                    Dim item As EveType = StaticData.Types(typeID)
                    dbNavigator &= "<a href=/itemDB/>Home</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=g&id=" & item.Category.ToString & ">" & StaticData.TypeCats(item.Category) & "</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=i&id=" & item.Group.ToString & ">" & StaticData.TypeGroups(item.Group) & "</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=t&id=" & item.Id.ToString & ">" & item.Name & "</a>"
                    strHTML &= "<p>" & dbNavigator & "</p>"
                    strHTML &= "</td></tr></table>"
                    strHTML &= "<p><a href=/itemDB/?view=t&id=" & typeID & "&s=a>ATTRIBUTES</a>"
                    strHTML &= "  |  "
                    Dim bpTypeID As Integer = StaticData.GetBPTypeId(CInt(typeID))
                    Dim displayMaterialsLink As Boolean = False
                    If StaticData.Blueprints(bpTypeID).Resources.ContainsKey(1) Then
                        If StaticData.Blueprints(bpTypeID).Resources(1).Count > 0 Then
                            displayMaterialsLink = True
                        End If
                    End If
                    If StaticData.Blueprints(bpTypeID).Resources.ContainsKey(6) Then
                        If StaticData.Blueprints(bpTypeID).Resources(6).Count > 0 Then
                            displayMaterialsLink = True
                        End If
                    End If
                    If displayMaterialsLink = True Then
                        strHTML &= "<a href=/itemDB/?view=t&id=" & typeID & "&s=m>MATERIALS</a>"
                        strHTML &= "  |  "
                    End If

                    ' See if we can get any variations
                    If StaticData.GetVariationsForItem(typeID).Count > 1 Then
                        strHTML &= "<a href=/itemDB/?view=t&id=" & typeID & "&s=v>VARIATIONS</a>"
                        strHTML &= "  |  "
                    End If

                    ' Show additional information re blueprint or product
                    If item.Category = 9 Then
                        Dim typeID2 As Integer = StaticData.GetTypeId(bpTypeID)
                        If bpTypeID <> typeID2 Then
                            strHTML &= "<a href=/itemDB/?view=t&id=" & typeID2 & ">PRODUCT</a>"
                            strHTML &= "  |  "
                        End If
                    Else
                        If bpTypeID <> typeID Then
                            strHTML &= "<a href=/itemDB/?view=t&id=" & bpTypeID & ">BLUEPRINT</a>"
                            strHTML &= "  |  "
                        End If
                    End If
                    strHTML &= "<button type=""button"" onClick=""CCPEVE.showInfo(" & typeID & ")"">Show Info</button>"
                    strHTML &= "</p>"
                    strHTML &= "<table width=800px class=tbl border=1 cellpadding=0><tr width=100%>"
                    'If context.Request.UserAgent.StartsWith("EVE-IGB") Then
                    '    strHTML &= "<td width=64px><img src=""typeicon:" & typeID & """ width=64 height=64></td>"
                    'Else
                    strHTML &= "<td width=64px><img src='" & GetExternalIcon(item.Id) & "'></td>"
                    'End If
                    strHTML &= "<td style='font-size:x-large;'>"
                    strHTML &= "<b>" & item.Name & "</b>"
                    strHTML &= "</td></tr></table><br>"

                    Select Case context.Request.QueryString.Item("s")
                        Case "a", ""            ' If blank or attributes!
                            ' Define array for holding info ready for categorising
                            Dim attributes(150, 5) As String
                            Dim attNo As Integer = 0
                            ' Set "unused" flag
                            For a As Integer = 0 To 150 : attributes(a, 0) = "0" : Next

                            ' Insert attribute 1 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "A"
                            attributes(attNo, 2) = "Group ID"
                            attributes(attNo, 3) = item.Group.ToString
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"
                            ' Insert attribute 2 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "B"
                            attributes(attNo, 2) = "Description"
                            attributes(attNo, 3) = item.Description
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"
                            ' Insert attribute 3 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "C"
                            attributes(attNo, 2) = "Mass"
                            attributes(attNo, 3) = item.Mass.ToString("N0")
                            attributes(attNo, 4) = " kg"
                            attributes(attNo, 5) = "1"
                            ' Insert attribute 4 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "D"
                            attributes(attNo, 2) = "Volume"
                            attributes(attNo, 3) = item.Volume.ToString("N2")
                            attributes(attNo, 4) = " m3"
                            attributes(attNo, 5) = "1"
                            ' Insert attribute 5 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "E"
                            attributes(attNo, 2) = "Cargo Capacity"
                            attributes(attNo, 3) = item.Capacity.ToString("N2")
                            attributes(attNo, 4) = " m3"
                            attributes(attNo, 5) = "1"
                            ' Insert attribute 6 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "F"
                            attributes(attNo, 3) = item.PortionSize.ToString("N0")
                            attributes(attNo, 2) = "Portion Size"
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"
                            ' Insert attribute 7 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "G"
                            attributes(attNo, 2) = "Base Price"
                            attributes(attNo, 3) = item.BasePrice.ToString("N2")
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"

                            If item.Category = 9 Then
                                ' If in the blueprint category, fetch the relevant blueprint and display info
                                Dim bp As Blueprint = StaticData.Blueprints(typeID)

                                attNo += 1
                                attributes(attNo, 1) = "BP1"
                                attributes(attNo, 2) = "Tech Level"
                                attributes(attNo, 3) = bp.TechLevel.ToString("N0")
                                attributes(attNo, 4) = ""
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP2"
                                attributes(attNo, 2) = "Waste Factor"
                                attributes(attNo, 3) = bp.WasteFactor.ToString("N0")
                                attributes(attNo, 4) = ""
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP3"
                                attributes(attNo, 2) = "Material Modifier"
                                attributes(attNo, 3) = bp.MaterialModifier.ToString("N0")
                                attributes(attNo, 4) = ""
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP4"
                                attributes(attNo, 2) = "Productivity Modifier"
                                attributes(attNo, 3) = bp.ProductivityModifier.ToString("N0")
                                attributes(attNo, 4) = ""
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP5"
                                attributes(attNo, 2) = "Max Production Limit"
                                attributes(attNo, 3) = bp.MaxProductionLimit.ToString("N0")
                                attributes(attNo, 4) = ""
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP6"
                                attributes(attNo, 2) = "Production Time"
                                attributes(attNo, 3) = bp.ProductionTime.ToString("N0")
                                attributes(attNo, 4) = " s"
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP7"
                                attributes(attNo, 2) = "Research ML Time"
                                attributes(attNo, 3) = bp.ResearchMaterialLevelTime.ToString("N0")
                                attributes(attNo, 4) = " s"
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP8"
                                attributes(attNo, 2) = "Research PL Time"
                                attributes(attNo, 3) = bp.ResearchProductionLevelTime.ToString("N0")
                                attributes(attNo, 4) = " s"
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP9"
                                attributes(attNo, 2) = "Research Copy Time"
                                attributes(attNo, 3) = bp.ResearchCopyTime.ToString("N0")
                                attributes(attNo, 4) = " s"
                                attributes(attNo, 5) = "15"

                                attNo += 1
                                attributes(attNo, 1) = "BP10"
                                attributes(attNo, 2) = "Research Tech Time"
                                attributes(attNo, 3) = bp.ResearchTechTime.ToString("N0")
                                attributes(attNo, 4) = " s"
                                attributes(attNo, 5) = "15"

                            Else

                                ' If not in the blueprint category, fetch the attributes
                                Dim attributeList As SortedList(Of Integer, ItemAttribData) = StaticData.GetAttributeDataForItem(typeID)

                                For Each att As ItemAttribData In attributeList.Values
                                    attNo += 1
                                    attributes(attNo, 1) = att.Id.ToString
                                    attributes(attNo, 2) = att.DisplayName
                                    attributes(attNo, 3) = att.DisplayValue
                                    attributes(attNo, 4) = att.Unit
                                Next

                            End If

                            ' Do character attribute adjustments here
                            For att As Integer = 1 To attNo
                                If attributes(att, 4) = "attributeID" Then
                                    attributes(att, 3) = StaticData.AttributeTypes(CInt(attributes(att, 3))).DisplayName
                                    attributes(att, 4) = ""
                                End If
                            Next

                            ' Put the stuff into a nice table!
                            Dim attGroups(15) As String
                            attGroups(0) = "Miscellaneous" : attGroups(1) = "Structure" : attGroups(2) = "Armor" : attGroups(3) = "Shield"
                            attGroups(4) = "Capacitor" : attGroups(5) = "Targetting" : attGroups(6) = "Propulsion" : attGroups(7) = "Required Skills"
                            attGroups(8) = "Fitting Requirements" : attGroups(9) = "Damage" : attGroups(10) = "Entity Targetting" : attGroups(11) = "Entity Kill"
                            attGroups(12) = "Entity EWar" : attGroups(13) = "Usage" : attGroups(14) = "Skill Information" : attGroups(15) = "Blueprint Information"
                            For itemloop As Integer = 1 To attNo
                                If attributes(itemloop, 0) = "0" And attributes(itemloop, 1) <> "0" Then
                                    Dim attGroup As String = attributes(itemloop, 5)
                                    strHTML &= "<table width=800px border=1 cellpadding=0><tr bgcolor=#661111><td colspan=2>" & attGroups(CInt(attGroup)) & "</td></tr>"
                                    For itemNo As Integer = itemloop To attNo
                                        If attributes(itemNo, 5) = attGroup And attributes(itemNo, 1) <> "0" Then
                                            strHTML &= "<tr align=top width=600px><td width=300px>"
                                            strHTML &= "(" & attributes(itemNo, 1) & ")  " & attributes(itemNo, 2)
                                            strHTML &= "</td><td>"
                                            strHTML &= attributes(itemNo, 3) & attributes(itemNo, 4)
                                            strHTML &= "</td></tr>"
                                            attributes(itemNo, 0) = "1"
                                        End If
                                    Next
                                    strHTML &= "</table><br>"
                                End If
                            Next

                        Case "m"
                            Dim bp As Blueprint = StaticData.Blueprints(typeID)

                            ' Display our activities
                            strHTML &= "|"
                            For Each activity As Integer In [Enum].GetValues(GetType(BlueprintActivity))
                                If bp.Resources(activity).Count > 0 Then
                                    strHTML &= "  <a href=/itemDB/?view=t&id=" & typeID & "&s=m&a=" & activity & ">"
                                    strHTML &= [Enum].GetName(GetType(BlueprintActivity), activity) & "</a>  |"
                                End If
                            Next

                            ' If activity is blank then set to manufacturing
                            Dim act As String = context.Request.QueryString.Item("a")
                            If act = "" Then act = "1"
                            strHTML &= "<p style='font-size:14;'><b>Materials: " & [Enum].GetName(GetType(BlueprintActivity), CInt(act)) & "</b></p>"

                            strHTML &= "<table width=600px border=1 cellpadding=0><tr bgcolor=#661111><td colspan=3></td></tr>"
                            For Each br As BlueprintResource In bp.Resources(CInt(act)).Values
                                strHTML &= "<tr align=top width=600px>"
                                Dim matInfo As EveType = StaticData.Types(br.TypeId)
                                'If context.Request.UserAgent.StartsWith("EVE-IGB") Then
                                '    strHTML &= "<td width=32px><img src=typeicon:" & matInfo.ID.ToString & " width=32 height=32></td>"
                                'Else
                                strHTML &= "<td width=32px><img src='" & GetExternalIcon(matInfo.Id) & "' width=32px height=32px></td>"
                                'End If
                                strHTML &= "<td width=300px><a href=/itemDB/?view=t&id=" & matInfo.Id.ToString & ">" & matInfo.Name & "</a>"
                                strHTML &= "</td><td>"
                                strHTML &= br.Quantity.ToString("N2")
                                strHTML &= "</td></tr>"
                            Next
                            strHTML &= "</table><br>"
                        Case "v"
                            Dim metaItems As List(Of Integer) = StaticData.GetVariationsForItem(typeID)

                            strHTML &= "<p style='font-size:14;'><b>Variations</b></p>"
                            strHTML &= "<table width=600px border=1 cellpadding=0><tr bgcolor=#661111><td width=400px colspan=2>Item Name</td><td width=200px>Meta Type</td></tr>"
                            For Each metaItemID As Integer In metaItems
                                strHTML &= "<tr>"
                                'If context.Request.UserAgent.StartsWith("EVE-IGB") Then
                                '    strHTML &= "<td width=32px><img src=typeicon:" & itemVariations(0, item) & " width=32 height=32></td>"
                                'Else
                                Dim iInfo As EveType = StaticData.Types(metaItemID)
                                strHTML &= "<td width=32px><img src='" & GetExternalIcon(iInfo.Id) & "' width=32px height=32px></td>"
                                'End If
                                strHTML &= "<td width=368px><a href=/itemDB/?view=t&id=" & iInfo.Id.ToString & ">" & iInfo.Name & "</a></td><td>" & StaticData.MetaGroups(StaticData.MetaTypes(iInfo.Id).MetaGroupId) & "</td></tr>"
                            Next
                            strHTML &= "</table><br>"
                    End Select

                Case Else
                    strHTML &= "<p>Unknown view type!!!</p>"
            End Select
        Catch e As Exception
            strHTML &= "<p>Unable to access database...please check location and integrity.</p>"
            strHTML &= "<p>" & e.Message & "</p>"
        End Try
        Return strHTML
    End Function

    Private Function GetExternalIcon(ByVal typeID As Integer) As String
        Return ImageHandler.GetRawImageLocation(typeID)
    End Function

    Private Function CreateCharReports() As String

        Dim pilotNames As ArrayList = New ArrayList
        Dim curPilot As EveHQPilot
        For Each curPilot In HQ.Settings.Pilots.Values
            If curPilot.Updated = True Then
                pilotNames.Add(curPilot.Name)
            End If
        Next
        pilotNames.Sort()

        Dim strHTML As String = ""
        ' Step 1 - Draw the pilots drop down list
        strHTML &= "<p>Please select a Pilot and a report to view</p>"
        If HQ.Settings.Pilots.Count <> pilotNames.Count Then
            strHTML &= "<p><b>EveHQ is indicating that you have pilots but they have not been updated, therefore not all pilots will be accessible here.<br><br>"
            strHTML &= "Please update your accounts and/or pilots in order to view reports on all pilots.</b></p>"
        End If
        If pilotNames.Count > 0 Then
            strHTML &= "<form method=""GET"" action=""/reports/charreport"">"
            strHTML &= "<table><tr><td width=100px>Pilot:</td><td width=250px><select name='Pilot' style='width: 200px;'>"
            Dim pilotName As String
            For Each pilotName In pilotNames
                strHTML &= "<option "
                If context.Request.UserAgent.StartsWith("EVE-IGB") Then
                    If context.Request.Headers("EVE_CHARNAME") = pilotName Then
                        strHTML &= "selected='selected'"
                    End If
                Else
                    If pilotName = HQ.Settings.Pilots.Values(0).Name Then
                        strHTML &= "selected='selected'"
                    End If
                End If
                strHTML &= ">" & pilotName & "</option>"
            Next
            strHTML &= "</select></td></tr><br>"
            strHTML &= "<tr><td width=100px>Report:</td><td width=250px><select name='Report' style='width: 200px'>"
            strHTML &= "<option selected='selected'>Character Sheet</option>"
            strHTML &= "<option>Skill Levels</option>"
            strHTML &= "<option>Training Queues</option>"
            strHTML &= "<option>Training Times</option>"
            strHTML &= "<option>Time To Level 5</option>"
            strHTML &= "<option>Skills Available To Train</option>"
            strHTML &= "<option>Skills Not Trained</option>"
            strHTML &= "</select></td></tr>"
            strHTML &= "<br><tr><td><input type='submit' value='Get Report'></td></tr></table></form>"
        Else
            strHTML &= "<p>You currently have no valid pilots on which to generate a report.</p>"
        End If

        Return strHTML
    End Function
    Private Function CreateCharReport() As String
        Dim strHTML As String = ""
        If context.Request.QueryString.Count < 2 Then
            strHTML &= IGBHTMLHeader(context, "Character Report Error", 0)
            strHTML &= "<p>There was an error generating your character report</p>"
            strHTML &= IGBHTMLFooter(context)
        Else
            Dim repString As String = context.Request.QueryString.Item("report")
            Dim pilotString As String = context.Request.QueryString.Item("Pilot")
            repString = repString.Replace("+", " ")
            pilotString = pilotString.Replace("+", " ")
            strHTML &= IGBHTMLHeader(context, repString & " Report For " & pilotString, 0)
            Dim repPilot As EveHQPilot = HQ.Settings.Pilots(pilotString)
            Select Case repString
                Case "Character Sheet"
                    strHTML &= Reports.HTMLTitle("Character Sheet - " & repPilot.Name)
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= Reports.CharacterSheet(repPilot)
                Case "Skill Levels"
                    strHTML &= Reports.HTMLTitle("Skill Levels Sheet - " & repPilot.Name)
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= Reports.SkillLevels(repPilot)
                Case "Training Queues"
                    strHTML &= Reports.HTMLTitle("Training Queues - " & repPilot.Name)
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= CreateQueueLists(repPilot)
                Case "Training Times"
                    strHTML &= Reports.HTMLTitle("Training Times - " & repPilot.Name)
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= Reports.TrainingTime(repPilot)
                Case "Time To Level 5"
                    strHTML &= Reports.HTMLTitle("Time To Level 5 - " & repPilot.Name)
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= Reports.TimeToLevel5(repPilot)
                Case "Skills Available To Train"
                    strHTML &= Reports.HTMLTitle("Skills Available To Train - " & repPilot.Name)
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= Reports.SkillsAvailable(repPilot)
                Case "Skills Not Trained"
                    strHTML &= Reports.HTMLTitle("Skills Not Trained - " & repPilot.Name)
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= Reports.SkillsNotTrained(repPilot)
                Case Else
                    strHTML &= "<p>There was an error generating your character report</p>"
            End Select
            strHTML &= Reports.HTMLFooter
        End If
        Return strHTML
    End Function
    Private Function CreateQueueLists(ByVal repPilot As EveHQPilot) As String
        Dim strHTML As String = ""

        strHTML &= "<table width=800px cellspacing=0 cellpadding=0>"
        strHTML &= "<tr><td width=300px bgcolor=#44444488>Training Queues:</td><td width=100px></td><td width=300px bgcolor=#44444488>Shopping Lists:</td></tr>"
        strHTML &= "<tr><td width=300px></td><td width=100px></td><td width=300px></td></tr>"

        For Each cQueue As EveHQSkillQueue In repPilot.TrainingQueues.Values
            strHTML &= "<tr><td width=300px><a href=/REPORTS/CHARREPORT/QUEUES?Pilot=" & repPilot.Name.Replace(" ", "+") & "&Report=Training+Queue&Queue=" & cQueue.Name.Replace(" ", "+") & ">" & cQueue.Name & "</a></td><td width=100px></td><td width=300px><a href=/REPORTS/CHARREPORT/QUEUES?Pilot=" & repPilot.Name.Replace(" ", "+") & "&Report=Shopping+List&Queue=" & cQueue.Name.Replace(" ", "+") & ">" & cQueue.Name & "</a></td></tr>"
        Next

        strHTML &= "</table>"
        strHTML &= "<p></p>"

        Return strHTML
    End Function
    Private Function CreateQueueReport() As String
        Dim strHTML As String = ""
        If context.Request.QueryString.Count < 3 Then
            strHTML &= IGBHTMLHeader(context, "Character Queue Report Error", 0)
            strHTML &= "<p>There was an error generating your character report</p>"
            strHTML &= IGBHTMLFooter(context)
        Else
            Dim repString As String = context.Request.QueryString.Item("Report")
            Dim pilotString As String = context.Request.QueryString.Item("Pilot")
            Dim queueString As String = context.Request.QueryString.Item("Queue")
            repString = repString.Replace("+", " ")
            pilotString = pilotString.Replace("+", " ")
            queueString = queueString.Replace("+", " ")
            strHTML &= IGBHTMLHeader(context, repString & " Report For " & pilotString, 0)
            Dim repPilot As EveHQPilot = HQ.Settings.Pilots(pilotString)
            Select Case repString
                Case "Training Queue"
                    strHTML &= Reports.HTMLTitle("Training Queue - " & repPilot.Name & " (" & queueString & ")")
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= Reports.TrainQueue(repPilot, repPilot.TrainingQueues(queueString))
                Case "Shopping List"
                    strHTML &= Reports.HTMLTitle("Shopping List - " & repPilot.Name & " (" & queueString & ")")
                    strHTML &= Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= Reports.ShoppingList(repPilot, repPilot.TrainingQueues(queueString))
                Case Else
                    strHTML &= "<p>There was an error generating your character report</p>"
            End Select
            strHTML &= Reports.HTMLFooter
        End If
        Return strHTML
    End Function

    Private Function CreateMarketTest() As String
        Dim strHTML As New StringBuilder

        strHTML.Append(IGBHTMLHeader(context, "EveHQ IGB Market Crawl Test", 5))
        strHTML.Append("<p>Welcome to the EveHQ IGB Market Crawl Test</p>")
        strHTML.Append(Now.ToString)

        Return strHTML.ToString
    End Function
#End Region
End Class

