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

Public Class IGB
    Shared context As HttpListenerContext
    Dim listener As System.Net.HttpListener
    Dim response As HttpListenerResponse
    Dim eveData As New DataSet
    Shared timeStart, timeEnd As DateTime
    Shared timeTaken As TimeSpan
    Const maxActivities As Integer = 9
    Private EveIcons As Collection = New Collection
    Private cTQPlayers, cSisiPlayers As Long

    Public Property TQPlayers() As Long
        Get
            Return cTQPlayers
        End Get
        Set(ByVal value As Long)
            cTQPlayers = value
        End Set
    End Property
    Public Property SisiPlayers() As Long
        Get
            Return cSisiPlayers
        End Get
        Set(ByVal value As Long)
            cSisiPlayers = value
        End Set
    End Property
    Public Sub RunIGB(ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs)
        Dim prefixes(0) As String
        prefixes(0) = "http://*:" & EveHQ.Core.HQ.EveHQSettings.IGBPort & "/"

        ' URI prefixes are required,
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

            ' Set the number of requests this application will handle.
            'Dim numRequestsToBeHandled As Integer = 10

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
                        ' Start the page generation timer
                        timeStart = Now

                        Select Case context.Request.Url.AbsolutePath.ToUpper
                            Case "", "/"
                                responseString &= RedirectHome()
                            Case "/HOME", "/HOME/"
                                responseString &= CreateHome()
                            Case "/ITEMDB", "/ITEMDB/"
                                responseString &= CreateHTMLItemDB()
                            Case "/SEARCHRESULTS", "SEARCHRESULTS"
                                responseString &= CreateHTMLSearchResultsSQL()
                            Case "/HEADERS", "/HEADERS/"
                                responseString &= CreateHeaders()
                            Case "/REPORTS", "/REPORTS/"
                                responseString &= CreateReports()
                            Case "/REPORTS/ALLOY", "/REPORTS/ALLOY/"
                                responseString &= CreateAlloyReport()
                            Case "/REPORTS/ORE", "/REPORTS/ORE/"
                                responseString &= CreateOreReport()
                            Case "/REPORTS/ICE", "/REPORTS/ICE/"
                                responseString &= CreateIceReport()
                            Case "/REPORTS/SKILLLEVELS", "/REPORTS/SKILLLEVELS/"
                                responseString &= CreateSPReport()
                            Case "/REPORTS/CHARACTER", "/REPORTS/CHARACTER/"
                                responseString &= ShowCharReports()
                            Case "/REPORTS/CHARSUMM", "/REPORTS/CHARASUMM/"
                                responseString &= ShowCharSummary()
                            Case "/REPORTS/CHARREPORT", "/REPORTS/CHARREPORT/"
                                responseString &= CreateCharReport()
                            Case "/REPORTS/CHARREPORT/QUEUES", "/REPORTS/CHARREPORT/QUEUES/"
                                responseString &= CreateQueueReport()
                            Case "/LOGO.JPG"
                                context.Response.ContentType = "image/jpeg"
                                My.Resources.EveHQ_IGBLogo.Save(Path.Combine(EveHQ.Core.HQ.cacheFolder, "logo.jpg"))
                                responseString = GetImage(Path.Combine(EveHQ.Core.HQ.cacheFolder, "logo.jpg"))
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
                                ' Check if this is a plugin string
                                Dim IGBPlugin As Boolean = False
                                For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
                                    Dim testName As String = PlugInInfo.Name.Replace(" ", "")
                                    If context.Request.Url.AbsolutePath.ToUpper.StartsWith("/" & testName.ToUpper) Then
                                        IGBPlugin = True
                                        Dim PlugInResponse As String = ""
                                        Dim myAssembly As Assembly = Assembly.LoadFrom(PlugInInfo.FileName)
                                        Dim t As Type = myAssembly.GetType(PlugInInfo.FileType)
                                        PlugInInfo.Instance = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
                                        Dim runPlugIn As EveHQ.Core.IEveHQPlugIn = PlugInInfo.Instance
                                        PlugInResponse = runPlugIn.IGBService(context)
                                        If PlugInResponse Is Nothing Then
                                            PlugInResponse = "The module '" & PlugInInfo.Name & "' failed to return a valid response."
                                        End If
                                        responseString &= PlugInResponse
                                    End If
                                Next
                                If IGBPlugin = False Then
                                    responseString &= IGBHTMLHeader(context, "EveHQ IGB Site")
                                    responseString &= "Sorry, the page you are looking for cannot be found.<br><br>"
                                    responseString &= IGBHTMLFooter(context)
                                End If
                        End Select

                        Dim buffer() As Byte = System.Text.Encoding.Default.GetBytes(responseString)
                        response.ContentLength64 = buffer.Length
                        response.ContentType = "text/html;q=0.9;*/*;q=0.5"
                        Dim output As System.IO.Stream = response.OutputStream
                        output.Write(buffer, 0, buffer.Length)
                        output.Flush()
                        output.Close()
                        output.Dispose()
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
            MessageBox.Show("There was an error using the IGB server. The error was: " & ex.Message, "IGB Server Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            ' Stop listening for requests.
            listener.Close()
        End Try
    End Sub
    Public Shared Function GetImage(ByVal LocalFile As String) As String
        'Create a file stream from an existing file.
        Dim fi As New FileInfo(LocalFile)
        Dim fs As FileStream = fi.OpenRead()
        'Read bytes into an array from the specified file.
        Dim nBytes As Integer = CInt(fi.Length)
        Dim ByteArray(nBytes - 1) As Byte
        Dim nBytesRead As Integer = fs.Read(ByteArray, 0, nBytes)
        fs.Close()
        fs.Dispose()
        Return System.Text.Encoding.Default.GetString(ByteArray)
    End Function

#Region "IGB Procedures and Functions"
    Private Sub ShowData()
        Dim lvwData As New ListView
        lvwData.View = View.Details
        lvwData.Width = 500
        lvwData.Height = 500
        With lvwData
            .Clear()
            .Columns.Clear()
            ' Add column headers
            For col As Integer = 0 To eveData.Tables(0).Columns.Count - 1
                .Columns.Add(eveData.Tables(0).Columns(col).Caption)
            Next
            ' Add data
            For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                Dim item As New ListViewItem
                If IsDBNull(eveData.Tables(0).Rows(row).Item(0)) = False Then
                    item.Text = CStr(eveData.Tables(0).Rows(row).Item(0))
                Else
                    item.Text = ""
                End If
                For col As Integer = 1 To eveData.Tables(0).Columns.Count - 1
                    If IsDBNull(eveData.Tables(0).Rows(row).Item(col)) = False Then
                        item.SubItems.Add(eveData.Tables(0).Rows(row).Item(col).ToString)
                    Else
                        item.SubItems.Add("")
                    End If
                Next
                .Items.Add(item)
            Next
            .Refresh()
            Dim frmData As New Form
            frmData.Width = 600
            frmData.Height = 600
            lvwData.Parent = frmData
            lvwData.Dock = DockStyle.Fill
            frmData.ShowDialog()
        End With
    End Sub
    Public Shared Function IGBHTMLHeader(ByVal context As Net.HttpListenerContext, Optional ByVal strTitle As String = "") As String
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
        strHTML &= "--></STYLE>"
        strHTML &= "</HEAD>"
        If context.Request.Headers("EVE_CHARNAME") <> "" Then
            strHTML &= "<BODY link=#ff8888 alink=#ff8888 vlink=#ff8888>"
        Else
            strHTML &= "<BODY onLoad=""CCPEVE.requestTrust('http://" & context.Request.Headers("Host") & "')"" link=#ff8888 alink=#ff8888 vlink=#ff8888>"
        End If
        strHTML &= "<img src=""http://" & context.Request.Headers("Host") & "/logo.jpg"" alt=""EveHQ Logo"" />  IGB Server<br>"
        strHTML &= "<p>"
        If context.Request.Headers("EVE_CHARNAME") = "" Then
            strHTML &= "Greetings Pilot!<br>"
        Else
            strHTML &= "Greetings " & context.Request.Headers("EVE_CHARNAME") & "!<br>"
        End If
        strHTML &= "<hr><a href=/>Home</a>  |  <a href=/itemDB>Item Database</a>  |  <a href=/reports>Reports</a>  |  <a href=/headers>IGB Headers</a>"
        For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
            If PlugInInfo.RunInIGB = True Then
                strHTML &= "  |  <a href=/" & PlugInInfo.Name.Replace(" ", "") & ">" & PlugInInfo.MainMenuText & "</a>"
            End If
        Next
        strHTML &= "</p>"
        strHTML &= "<form method=""GET"" action=""/searchResults"">"
        strHTML &= "Search Item Database:  "
        strHTML &= "<input type=""text"" name=""str"">"
        strHTML &= "<input type=""submit"" value=""Search!""></form><hr><br>"
        Return strHTML
    End Function
    Public Shared Function IGBHTMLFooter(ByVal context As Net.HttpListenerContext) As String
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
        Dim strPilot As String = ""
        Dim strHost As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ IGB Home")
        strHTML &= "<p>Welcome to the EveHQ In-Game Browser (IGB) Server!</p>"
        strHTML &= "<p>This server will give you access to the wealth of information that is the Eve Online database and present it in tabular form for easy reading.</p>"
        If context.Request.UserAgent.StartsWith("EVE-IGB") Then
            strHTML &= "<p>If you have any questions or suggestions, please contact <a href='evemail:Vessper' SUBJECT='EveHQ IGB'>Vessper</a> via Eve-mail.</p>"
        Else
            strHTML &= "<p>If you have any questions or suggestions, please contact <a href='mailto:vessper@evehq.net'>Vessper</a> via E-mail.</p>"
        End If
        strHTML &= "<p></p>"
        strHTML &= "<p>Happy browsing and fly safe!</p>"
        strHTML &= "<p>  - Vessper</p>"
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateHTMLItemDB() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ IGB Item Database")
        strHTML &= CreateNavPaneSQL()
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateHeaders() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ IGB Header Info")
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
        strHTML &= IGBHTMLHeader(context, "EveHQ Reports")
        strHTML &= "<p>Please select a report from the list below:</p><br>"
        strHTML &= "<p><a href=/reports/alloy>Alloy Composition Report</a><br>"
        strHTML &= "<br><a href=/reports/ore>Ore Composition Report</a><br>"
        strHTML &= "<br><a href=/reports/ice>Ice Composition Report</a><br>"
        strHTML &= "<br><a href=/reports/skilllevels>Skill Level Table</a><br>"
        strHTML &= "<br><a href=/reports/charsumm>Character Summary</a><br>"
        strHTML &= "<br><a href=/reports/character>Individual Character Reports</a></p>"
        strHTML &= "<br><br><br>"
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateOreReport() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Ore Composition Report")
        strHTML &= EveHQ.Core.Reports.HTMLTitle("Ore Composition Report")
        strHTML &= EveHQ.Core.Reports.RockReport(True)
        strHTML &= EveHQ.Core.Reports.HTMLFooter
        Return strHTML
    End Function
    Private Function CreateIceReport() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Ice Composition Report")
        strHTML &= EveHQ.Core.Reports.HTMLTitle("Ice Composition Report")
        strHTML &= EveHQ.Core.Reports.IceReport(True)
        strHTML &= EveHQ.Core.Reports.HTMLFooter
        Return strHTML
    End Function
    Private Function CreateAlloyReport() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Alloy Composition Report")
        strHTML &= EveHQ.Core.Reports.HTMLTitle("Alloy Composition Report")
        strHTML &= EveHQ.Core.Reports.AlloyReport(True)
        strHTML &= EveHQ.Core.Reports.HTMLFooter
        Return strHTML
    End Function
    Private Function CreateSPReport() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Skill Level Table")
        strHTML &= EveHQ.Core.Reports.HTMLTitle("Skill Level Table")
        strHTML &= EveHQ.Core.Reports.SPSummary
        strHTML &= EveHQ.Core.Reports.HTMLFooter
        Return strHTML
    End Function
    Private Function ShowCharSummary() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Pilot Summary")
        strHTML &= EveHQ.Core.Reports.HTMLTitle("Pilot Summary")
        strHTML &= EveHQ.Core.Reports.CharSummary()
        strHTML &= EveHQ.Core.Reports.HTMLFooter
        Return strHTML
    End Function
    Private Function ShowCharReports() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "Generate Character Report")
        strHTML &= CreateCharReports(True)
        strHTML &= IGBHTMLFooter(context)
        Return strHTML
    End Function
    Private Function CreateHTMLSearchResultsSQL() As String
        Dim strHTML As String = ""
        strHTML &= IGBHTMLHeader(context, "EveHQ Search Results")
        If context.Request.QueryString.Count = 0 Or context.Request.QueryString.Item("str") = "" Then
            strHTML &= "<p>Please enter a valid search parameter</p>"
        Else
            Try
                Dim searchFor As String = System.Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("str"))
                eveData = EveHQ.Core.DataFunctions.GetData("SELECT * from invTypes WHERE invTypes.typeName LIKE '%" & searchFor & "%' ORDER BY invTypes.typeName;")
                strHTML &= "<p>Search results for """ & searchFor & """ (" & eveData.Tables(0).Rows.Count & " items):</p>"
                For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                    strHTML &= "<a href=/itemDB/?view=t&id=" & eveData.Tables(0).Rows(row).Item("typeID").ToString & ">" & eveData.Tables(0).Rows(row).Item("typeName").ToString & "</a><br>"
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
        Dim strSQL As String = ""
        Dim dbNavigator As String = "Database Location: "

        Try
            strHTML &= "<table width=800px class=tbl border=0><tr><td width=100%>"
            Select Case context.Request.QueryString.Item("view")
                Case "c", ""
                    dbNavigator &= "<a href=/itemDB/>Home</a>"
                    strHTML &= "<p>" & dbNavigator & "</p>"
                    strHTML &= "</td></tr></table>"
                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * from invCategories ORDER BY categoryName;")
                    For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                        strHTML &= "<a href=/itemDB/?view=g&id=" & eveData.Tables(0).Rows(row).Item(0).ToString & ">" & eveData.Tables(0).Rows(row).Item(1).ToString & "</a><br>"
                    Next
                Case "g"
                    Dim catID As String = context.Request.QueryString.Item("id")
                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * from invCategories WHERE categoryID=" & catID)
                    dbNavigator &= "<a href=/itemDB/>Home</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=g&id=" & catID & ">" & eveData.Tables(0).Rows(0).Item(1).ToString & "</a>"
                    strHTML &= "<p>" & dbNavigator & "</p>"
                    strHTML &= "</td></tr></table>"
                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invGroups WHERE invGroups.categoryID=" & catID.Trim & " ORDER BY groupName;")
                    For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                        strHTML &= "<a href=/itemDB/?view=i&id=" & eveData.Tables(0).Rows(row).Item("groupID").ToString & ">" & eveData.Tables(0).Rows(row).Item("groupName").ToString.Trim & "</a><br>"
                    Next
                Case "i"
                    Dim groupID As String = context.Request.QueryString.Item("id")
                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * from invGroups WHERE groupID=" & groupID)
                    Dim catID As String = eveData.Tables(0).Rows(0).Item("categoryID").ToString
                    Dim groupName As String = eveData.Tables(0).Rows(0).Item("groupName").ToString.Trim
                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * from invCategories WHERE categoryID=" & catID)
                    Dim catName As String = eveData.Tables(0).Rows(0).Item("categoryName").ToString.Trim
                    dbNavigator &= "<a href=/itemDB/>Home</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=g&id=" & catID & ">" & catName & "</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=i&id=" & groupID & ">" & groupName & "</a>"
                    strHTML &= "<p>" & dbNavigator & "</p>"
                    strHTML &= "</td></tr></table>"
                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invTypes WHERE invTypes.groupID=" & groupID.Trim & " ORDER BY typeName;")
                    For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                        strHTML &= "<a href=/itemDB/?view=t&id=" & eveData.Tables(0).Rows(row).Item("typeID").ToString & ">" & eveData.Tables(0).Rows(row).Item("typeName").ToString.Trim & "</a><br>"
                    Next
                Case "t"
                    Dim typeID As String = context.Request.QueryString.Item("id")
                    Dim pInfo(5) As String
                    pInfo = EveHQ.Core.DataFunctions.GetTypeParentInfo(typeID)
                    dbNavigator &= "<a href=/itemDB/>Home</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=g&id=" & pInfo(4) & ">" & pInfo(5) & "</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=i&id=" & pInfo(2) & ">" & pInfo(3) & "</a> -> "
                    dbNavigator &= "<a href=/itemDB/?view=t&id=" & pInfo(0) & ">" & pInfo(1) & "</a>"
                    strHTML &= "<p>" & dbNavigator & "</p>"
                    strHTML &= "</td></tr></table>"
                    strHTML &= "<p><a href=/itemDB/?view=t&id=" & typeID & "&s=a>ATTRIBUTES</a>"
                    strHTML &= "  |  "
                    Dim bpTypeID As String = EveHQ.Core.DataFunctions.GetBPTypeID(typeID)
                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * from ramTypeRequirements WHERE (typeID=" & bpTypeID & " OR typeID=" & typeID & ")")
                    If eveData.Tables(0).Rows.Count > 0 Then
                        strHTML &= "<a href=/itemDB/?view=t&id=" & typeID & "&s=m>MATERIALS</a>"
                        strHTML &= "  |  "
                    End If
                    ' See if we can get any variations

                    strSQL = "SELECT invMetaTypes.typeID, invMetaTypes.parentTypeID"
                    strSQL &= " FROM invMetaTypes"
                    strSQL &= " WHERE (((invMetaTypes.typeID)=" & typeID & ") OR ((invMetaTypes.parentTypeID)=" & typeID & "));"
                    eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
                    If eveData.Tables(0).Rows.Count > 0 Then
                        strHTML &= "<a href=/itemDB/?view=t&id=" & typeID & "&s=v>VARIATIONS</a>"
                        strHTML &= "  |  "
                    End If

                    ' Show additional information re blueprint or product
                    If CDbl(pInfo(4)) = 9 Then
                        Dim typeID2 As String = EveHQ.Core.DataFunctions.GetTypeID(bpTypeID)
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
                    strHTML &= "<td width=64px><img src='" & Me.GetExternalIcon(pInfo(0), pInfo(4)) & "'></td>"
                    'End If
                    strHTML &= "<td style='font-size:x-large;'>"
                    strHTML &= "<b>" & pInfo(1) & "</b>"
                    strHTML &= "</td></tr></table><br>"

                    Select Case context.Request.QueryString.Item("s")
                        Case "a", ""            ' If blank or attributes!
                            ' Define array for holding info ready for categorising
                            Dim attributes(150, 5) As String
                            Dim attNo As Integer = 0
                            ' Set "unused" flag
                            For a As Integer = 0 To 150 : attributes(a, 0) = "0" : Next

                            strSQL = "SELECT * from invTypes where typeID=" & typeID
                            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)

                            ' Insert attribute 1 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "A"
                            attributes(attNo, 2) = "Group ID"
                            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("groupID").ToString
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"
                            ' Insert attribute 2 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "B"
                            attributes(attNo, 2) = "Description"
                            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("description").ToString
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"
                            ' Insert attribute 3 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "C"
                            attributes(attNo, 2) = "Radius"
                            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("radius").ToString
                            attributes(attNo, 4) = " m"
                            attributes(attNo, 5) = "1"
                            ' Insert attribute 4 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "D"
                            attributes(attNo, 2) = "Mass"
                            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("mass").ToString
                            attributes(attNo, 4) = " kg"
                            attributes(attNo, 5) = "1"
                            ' Insert attribute 5 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "E"
                            attributes(attNo, 2) = "Volume"
                            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("volume").ToString
                            attributes(attNo, 4) = " m3"
                            attributes(attNo, 5) = "1"
                            ' Insert attribute 6 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "F"
                            attributes(attNo, 2) = "Cargo Capacity"
                            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("capacity").ToString
                            attributes(attNo, 4) = " m3"
                            attributes(attNo, 5) = "1"
                            ' Insert attribute 7 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "G"
                            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("portionSize").ToString
                            attributes(attNo, 2) = "Portion Size"
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"
                            ' Insert attribute 8 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "H"
                            attributes(attNo, 2) = "Race ID"
                            If IsDBNull(eveData.Tables(0).Rows(0).Item("raceID")) = False Then
                                attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("raceID").ToString
                            Else
                                attributes(attNo, 3) = "0"
                            End If
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"
                            ' Insert attribute 8 from tblTypes
                            attNo += 1
                            attributes(attNo, 1) = "I"
                            attributes(attNo, 2) = "Base Price"
                            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("basePrice").ToString
                            attributes(attNo, 4) = ""
                            attributes(attNo, 5) = "0"

                            If CDbl(pInfo(4)) = 9 Then            ' If in the blueprint category
                                strSQL = "SELECT *"
                                strSQL &= " FROM invBlueprintTypes"
                                strSQL &= " WHERE blueprintTypeID=" & typeID & ";"
                                eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
                                For col As Integer = 3 To eveData.Tables(0).Columns.Count - 1
                                    attNo += 1
                                    attributes(attNo, 1) = "Z" & col - 2
                                    attributes(attNo, 2) = eveData.Tables(0).Columns(col).Caption
                                    attributes(attNo, 3) = EveHQ.Core.DataFunctions.Round(CStr(eveData.Tables(0).Rows(0).Item(col)))
                                    attributes(attNo, 4) = ""
                                    attributes(attNo, 5) = "15"
                                Next
                                attributes(10, 4) = " ms"
                                attributes(12, 4) = " ms"
                                attributes(13, 4) = " ms"
                                attributes(14, 4) = " ms"
                                attributes(15, 4) = " ms"
                            Else                            ' If not in the blueprint category
                                strSQL = "SELECT dgmTypeAttributes.attributeID as attributeTypeID, dgmAttributeTypes.attributeGroup, eveUnits.unitID, eveUnits.displayName as unitDisplayName, eveUnits.unitName, dgmTypeAttributes.attributeID, dgmAttributeTypes.attributeID, dgmAttributeTypes.displayName as attributeDisplayName, dgmAttributeTypes.attributeName, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
                                strSQL &= " FROM (eveUnits INNER JOIN dgmAttributeTypes ON eveUnits.unitID=dgmAttributeTypes.unitID) INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID=dgmTypeAttributes.attributeID"
                                strSQL &= " WHERE typeID=" & typeID & " ORDER BY dgmTypeAttributes.attributeID;"
                                eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
                                For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                                    attNo += 1
                                    attributes(attNo, 1) = eveData.Tables(0).Rows(row).Item("attributeTypeID").ToString.Trim
                                    'If IsDBNull(eveData.Tables(0).Rows(row).Item("dgmAttributeTypes.displayName")) = True Then
                                    If eveData.Tables(0).Rows(row).Item("attributeDisplayName").ToString.Trim = "" Then
                                        attributes(attNo, 2) = eveData.Tables(0).Rows(row).Item("attributeName").ToString.Trim
                                    Else
                                        attributes(attNo, 2) = eveData.Tables(0).Rows(row).Item("attributeDisplayName").ToString.Trim
                                    End If
                                    If IsDBNull(eveData.Tables(0).Rows(row).Item("valueInt")) = True Then
                                        attributes(attNo, 3) = eveData.Tables(0).Rows(row).Item("valueFloat").ToString
                                    Else
                                        attributes(attNo, 3) = eveData.Tables(0).Rows(row).Item("valueInt").ToString
                                    End If
                                    attributes(attNo, 4) = " " & eveData.Tables(0).Rows(row).Item("unitDisplayName").ToString.Trim
                                    attributes(attNo, 5) = eveData.Tables(0).Rows(row).Item("attributeGroup").ToString.Trim
                                    ' Do modifier calculations here!
                                    Select Case eveData.Tables(0).Rows(row).Item("unitID").ToString
                                        Case "108"
                                            attributes(attNo, 3) = EveHQ.Core.DataFunctions.Round(CStr(100 - (Val(attributes(attNo, 3)) * 100)))
                                        Case "109"
                                            attributes(attNo, 3) = EveHQ.Core.DataFunctions.Round(CStr((Val(attributes(attNo, 3)) * 100) - 100))
                                        Case "111"
                                            attributes(attNo, 3) = EveHQ.Core.DataFunctions.Round(CStr((Val(attributes(attNo, 3)) - 1) * 100))
                                        Case "101"
                                            If Val(attributes(attNo, 3)) > 1000 Then
                                                attributes(attNo, 3) = EveHQ.Core.DataFunctions.Round(CStr(Val(attributes(attNo, 3)) / 1000))
                                                attributes(attNo, 4) = " s"
                                            End If
                                    End Select
                                Next
                            End If

                            ' Do character attribute adjustments here
                            Dim attName As String = ""
                            For att As Integer = 1 To attNo
                                If attributes(att, 4) = " attributeID" Then
                                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM dgmAttributeTypes WHERE attributeID=" & attributes(att, 3))
                                    attributes(att, 3) = eveData.Tables(0).Rows(0).Item("attributeName").ToString.Trim
                                    attributes(att, 4) = ""
                                End If
                            Next

                            ' Do skill requirements adjustment & "rounding" here
                            Dim skillLvl As String = ""
                            For att As Integer = 1 To attNo
                                If attributes(att, 4) = " typeID" Then
                                    eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invTypes WHERE typeID=" & attributes(att, 3))
                                    For att2 As Integer = att To attNo
                                        If Val(attributes(att, 1)) + 95 = Val(attributes(att2, 1)) Then
                                            skillLvl = attributes(att2, 3)
                                            attributes(att2, 1) = "0"
                                            Exit For
                                        End If
                                    Next
                                    attributes(att, 3) = "<a href=/itemDB/?view=t&id=" & attributes(att, 3) & ">" & eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim & "</a>"
                                    If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                                    attributes(att, 4) = ""
                                Else
                                    attributes(att, 3) = EveHQ.Core.DataFunctions.Round(attributes(att, 3))
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
                                    For item As Integer = itemloop To attNo
                                        If attributes(item, 5) = attGroup And attributes(item, 1) <> "0" Then
                                            strHTML &= "<tr align=top width=600px><td width=300px>"
                                            strHTML &= "(" & attributes(item, 1) & ")  " & attributes(item, 2)
                                            strHTML &= "</td><td>"
                                            strHTML &= attributes(item, 3) & attributes(item, 4)
                                            strHTML &= "</td></tr>"
                                            attributes(item, 0) = "1"
                                        End If
                                    Next
                                    strHTML &= "</table><br>"
                                End If
                            Next

                        Case "m"
                            bpTypeID = EveHQ.Core.DataFunctions.GetBPTypeID(typeID)
                            ' Select only the building activity (at the minute!)
                            strSQL = "SELECT ramTypeRequirements.requiredTypeID, invTypes.typeName, ramTypeRequirements.quantity, ramTypeRequirements.damagePerJob, invCategories.categoryID as categoryTypeID, invCategories.categoryName, invGroups.groupID as groupTypeID, invGroups.groupName, ramTypeRequirements.activityID"
                            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID = invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN ramTypeRequirements ON invTypes.typeID = ramTypeRequirements.requiredTypeID"
                            strSQL &= " WHERE (ramTypeRequirements.typeID=" & bpTypeID & " OR ramTypeRequirements.typeID=" & typeID & ")"
                            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)

                            ' Work out what activities we have in the list
                            Dim activities(maxActivities) As Boolean
                            Dim strActivity As String = ""
                            For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                                activities(CInt(Val(eveData.Tables(0).Rows(row).Item("activityID")))) = True
                            Next
                            ' Then create sub-headings :)
                            strHTML &= "|"
                            For activity As Integer = 1 To maxActivities
                                If activities(activity) = True Then
                                    ' Add to list!
                                    strHTML &= "  <a href=/itemDB/?view=t&id=" & typeID & "&s=m&a=" & activity & ">"
                                    Select Case activity
                                        Case 1
                                            strActivity = "Manufacturing"
                                        Case 2
                                            strActivity = "Research Tech"
                                        Case 3
                                            strActivity = "Research PE"
                                        Case 4
                                            strActivity = "Research ME"
                                        Case 5
                                            strActivity = "Copying"
                                        Case 6
                                            strActivity = "Duplicating"
                                        Case 7
                                            strActivity = "Reverse Engineering"
                                        Case 8
                                            strActivity = "Invention"
                                        Case 9
                                            strActivity = "Composition"
                                    End Select
                                    strHTML &= strActivity & "</a>  |"
                                End If
                            Next

                            Dim materials(eveData.Tables(0).Rows.Count, 9) As String
                            With eveData.Tables(0)
                                For row As Integer = 0 To .Rows.Count - 1
                                    If Val(.Rows(row).Item("quantity")) > 0 Then
                                        materials(row, 0) = "0"
                                        materials(row, 1) = .Rows(row).Item("requiredTypeID").ToString.Trim
                                        materials(row, 2) = .Rows(row).Item("typeName").ToString.Trim
                                        materials(row, 3) = .Rows(row).Item("quantity").ToString.Trim
                                        materials(row, 4) = .Rows(row).Item("damagePerJob").ToString.Trim
                                        materials(row, 5) = .Rows(row).Item("categoryTypeID").ToString.Trim
                                        materials(row, 6) = .Rows(row).Item("categoryName").ToString.Trim
                                        materials(row, 7) = .Rows(row).Item("groupTypeID").ToString.Trim
                                        materials(row, 8) = .Rows(row).Item("groupName").ToString.Trim
                                        materials(row, 9) = .Rows(row).Item("activityID").ToString.Trim
                                    End If
                                Next
                            End With

                            Dim itemcount As Integer = eveData.Tables(0).Rows.Count - 1
                            Dim matCatID, matCatName, matGroupID, matGroupName As String
                            Dim act As String = context.Request.QueryString.Item("a")
                            ' Decide which view to take if the activity is blank
                            If act = "" Then
                                For a As Integer = 1 To maxActivities
                                    If activities(a) = True Then
                                        act = CStr(a)
                                        Exit For
                                    End If
                                Next
                            End If

                            Select Case CInt(act)
                                Case 1
                                    strActivity = "Manufacturing"
                                Case 2
                                    strActivity = "Research Tech"
                                Case 3
                                    strActivity = "Research PE"
                                Case 4
                                    strActivity = "Research ME"
                                Case 5
                                    strActivity = "Copying"
                                Case 6
                                    strActivity = "Duplicating"
                                Case 7
                                    strActivity = "Reverse Engineering"
                                Case 8
                                    strActivity = "Invention"
                                Case 9
                                    strActivity = "Composition"
                            End Select
                            strHTML &= "<p style='font-size:14;'><b>Materials: " & strActivity & "</b></p>"

                            For itemloop As Integer = 0 To itemcount
                                If materials(itemloop, 0) = "0" And materials(itemloop, 9) = act Then
                                    matCatID = materials(itemloop, 5)
                                    matCatName = materials(itemloop, 6)
                                    matGroupID = materials(itemloop, 7)
                                    matGroupName = materials(itemloop, 8)
                                    strHTML &= "<table width=600px border=1 cellpadding=0><tr bgcolor=#661111><td colspan=3>" & matCatName & " / " & matGroupName & "</td></tr>"
                                    For item As Integer = itemloop To itemcount
                                        If materials(item, 9) = act And materials(item, 5) = matCatID And materials(item, 7) = matGroupID Then
                                            strHTML &= "<tr align=top width=600px>"
                                            'If context.Request.UserAgent.StartsWith("EVE-IGB") Then
                                            '    strHTML &= "<td width=32px><img src=typeicon:" & materials(item, 1) & " width=32 height=32></td>"
                                            'Else
                                            Dim iInfo() As String = EveHQ.Core.DataFunctions.GetTypeParentInfo(materials(item, 1))
                                            strHTML &= "<td width=32px><img src='" & Me.GetExternalIcon(iInfo(0), iInfo(4)) & "' width=32px height=32px></td>"
                                            'End If
                                            strHTML &= "<td width=300px><a href=/itemDB/?view=t&id=" & materials(item, 1) & ">" & materials(item, 2) & "</a>"
                                            strHTML &= "</td><td>"
                                            strHTML &= materials(item, 3)
                                            strHTML &= "</td></tr>"
                                            materials(item, 0) = "1"
                                        End If
                                    Next
                                    strHTML &= "</table><br>"
                                End If
                            Next
                        Case "v"
                            strSQL = "SELECT invMetaTypes.typeID, invMetaTypes.parentTypeID"
                            strSQL &= " FROM invMetaTypes"
                            strSQL &= " WHERE (((invMetaTypes.typeID)=" & typeID & ") OR ((invMetaTypes.parentTypeID)=" & typeID & "));"
                            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
                            Dim metaParentID As String = eveData.Tables(0).Rows(0).Item("parentTypeID").ToString
                            strSQL = ""
                            strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invTypes.typeName, invMetaTypes.typeID AS invMetaTypes_typeID, invMetaTypes.parentTypeID, invMetaTypes.metaGroupID AS invMetaTypes_metaGroupID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID, invMetaGroups.metaGroupName"
                            strSQL &= " FROM invMetaGroups INNER JOIN (invTypes INNER JOIN invMetaTypes ON invTypes.typeID = invMetaTypes.typeID) ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID"
                            strSQL &= " WHERE (((invMetaTypes.parentTypeID)=" & metaParentID & "));"
                            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
                            Dim metaItemCount As Integer = eveData.Tables(0).Rows.Count
                            Dim itemVariations(2, metaItemCount) As String
                            For item As Integer = 0 To metaItemCount - 1
                                itemVariations(0, item + 1) = eveData.Tables(0).Rows(item).Item("invTypes_typeID").ToString
                                itemVariations(1, item + 1) = eveData.Tables(0).Rows(item).Item("typeName").ToString.Trim
                                itemVariations(2, item + 1) = eveData.Tables(0).Rows(item).Item("metaGroupName").ToString.Trim
                            Next
                            strSQL = "SELECT invTypes.typeID, invTypes.typeName FROM invTypes WHERE invTypes.typeID=" & metaParentID & ";"
                            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
                            itemVariations(0, 0) = eveData.Tables(0).Rows(0).Item("typeID").ToString.Trim
                            itemVariations(1, 0) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                            itemVariations(2, 0) = "Tech I"

                            strHTML &= "<p style='font-size:14;'><b>Variations</b></p>"
                            strHTML &= "<table width=600px border=1 cellpadding=0><tr bgcolor=#661111><td width=400px colspan=2>Item Name</td><td width=200px>Meta Type</td></tr>"
                            For item As Integer = 0 To metaItemCount
                                strHTML &= "<tr>"
                                'If context.Request.UserAgent.StartsWith("EVE-IGB") Then
                                '    strHTML &= "<td width=32px><img src=typeicon:" & itemVariations(0, item) & " width=32 height=32></td>"
                                'Else
                                Dim iInfo() As String = EveHQ.Core.DataFunctions.GetTypeParentInfo(typeID)
                                strHTML &= "<td width=32px><img src='" & Me.GetExternalIcon(iInfo(0), iInfo(4)) & "' width=32px height=32px></td>"
                                'End If
                                strHTML &= "<td width=368px><a href=/itemDB/?view=t&id=" & itemVariations(0, item) & ">" & itemVariations(1, item) & "</a></td><td>" & itemVariations(2, item) & "</td></tr>"
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

    Private Function GetExternalIcon(ByVal typeID As String, ByVal catID As String) As String
        Select Case CInt(catID)
            Case 6, 18, 23
                Return EveHQ.Core.ImageHandler.GetRawImageLocation(typeID, ImageHandler.ImageType.Types)
            Case 9
                Return EveHQ.Core.ImageHandler.GetRawImageLocation(typeID, ImageHandler.ImageType.Blueprints)
            Case Else
                Dim iconData As System.Data.DataSet = EveHQ.Core.DataFunctions.GetData("SELECT invTypes.typeID, eveIcons.iconFile FROM eveIcons INNER JOIN invTypes ON eveIcons.iconID = invTypes.iconID WHERE typeID=" & typeID & ";")
                Return EveHQ.Core.ImageHandler.GetRawImageLocation(iconData.Tables(0).Rows(0).Item("iconFile").ToString, ImageHandler.ImageType.Icons)
        End Select
    End Function

    Private Function CreateCharReports(ByVal forIGB As Boolean) As String

        Dim pilotNames As ArrayList = New ArrayList
        Dim curPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        For Each curPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If curPilot.Updated = True Then
                pilotNames.Add(curPilot.Name)
            End If
        Next
        pilotNames.Sort()

        Dim strHTML As String = ""
        ' Step 1 - Draw the pilots drop down list
        strHTML &= "<p>Please select a Pilot and a report to view</p>"
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count <> pilotNames.Count Then
            strHTML &= "<p><b>EveHQ is indicating that you have pilots but they have not been updated, therefore not all pilots will be accessible here.<br><br>"
            strHTML &= "Please update your accounts and/or pilots in order to view reports on all pilots.</b></p>"
        End If
        If pilotNames.Count > 0 Then
            strHTML &= "<form method=""GET"" action=""/reports/charreport"">"
            strHTML &= "<table><tr><td width=100px>Pilot:</td><td width=250px><select name='Pilot' style='width: 200px;'>"
            Dim pilotName As String = ""
            For Each pilotName In pilotNames
                strHTML &= "<option "
                If context.Request.UserAgent.StartsWith("EVE-IGB") Then
                    If context.Request.Headers("EVE_CHARNAME") = pilotName Then
                        strHTML &= "selected='selected'"
                    End If
                Else
                    If pilotName = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), EveHQ.Core.Pilot).Name Then
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
            strHTML &= IGBHTMLHeader(context, "Character Report Error")
            strHTML &= "<p>There was an error generating your character report</p>"
            strHTML &= IGBHTMLFooter(context)
        Else
            Dim repString As String = context.Request.QueryString.Item("report")
            Dim pilotString As String = context.Request.QueryString.Item("Pilot")
            repString = repString.Replace("+", " ")
            pilotString = pilotString.Replace("+", " ")
            strHTML &= IGBHTMLHeader(context, repString & " Report For " & pilotString)
            Dim repPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(pilotString), Pilot)
            Select Case repString
                Case "Character Sheet"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Character Sheet - " & repPilot.Name)
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= EveHQ.Core.Reports.CharacterSheet(repPilot)
                Case "Skill Levels"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Skill Levels Sheet - " & repPilot.Name)
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= EveHQ.Core.Reports.SkillLevels(repPilot)
                Case "Training Queues"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Training Queues - " & repPilot.Name)
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= CreateQueueLists(repPilot)
                Case "Training Times"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Training Times - " & repPilot.Name)
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= EveHQ.Core.Reports.TrainingTime(repPilot)
                Case "Time To Level 5"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Time To Level 5 - " & repPilot.Name)
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= EveHQ.Core.Reports.TimeToLevel5(repPilot)
                Case "Skills Available To Train"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Skills Available To Train - " & repPilot.Name)
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= EveHQ.Core.Reports.SkillsAvailable(repPilot)
                Case "Skills Not Trained"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Skills Not Trained - " & repPilot.Name)
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= EveHQ.Core.Reports.SkillsNotTrained(repPilot)
                Case Else
                    strHTML &= "<p>There was an error generating your character report</p>"
            End Select
            strHTML &= EveHQ.Core.Reports.HTMLFooter
        End If
        Return strHTML
    End Function
    Private Function CreateQueueLists(ByVal repPilot As EveHQ.Core.Pilot) As String
        Dim strHTML As String = ""

        strHTML &= "<table width=800px cellspacing=0 cellpadding=0>"
        strHTML &= "<tr><td width=300px bgcolor=#44444488>Training Queues:</td><td width=100px></td><td width=300px bgcolor=#44444488>Shopping Lists:</td></tr>"
        strHTML &= "<tr><td width=300px></td><td width=100px></td><td width=300px></td></tr>"

        For Each cQueue As SkillQueue In repPilot.TrainingQueues.Values
            strHTML &= "<tr><td width=300px><a href=/REPORTS/CHARREPORT/QUEUES?Pilot=" & repPilot.Name.Replace(" ", "+") & "&Report=Training+Queue&Queue=" & cQueue.Name.Replace(" ", "+") & ">" & cQueue.Name & "</a></td><td width=100px></td><td width=300px><a href=/REPORTS/CHARREPORT/QUEUES?Pilot=" & repPilot.Name.Replace(" ", "+") & "&Report=Shopping+List&Queue=" & cQueue.Name.Replace(" ", "+") & ">" & cQueue.Name & "</a></td></tr>"
        Next

        strHTML &= "</table>"
        strHTML &= "<p></p>"

        Return strHTML
    End Function
    Private Function CreateQueueReport() As String
        Dim strHTML As String = ""
        If context.Request.QueryString.Count < 3 Then
            strHTML &= IGBHTMLHeader(context, "Character Queue Report Error")
            strHTML &= "<p>There was an error generating your character report</p>"
            strHTML &= IGBHTMLFooter(context)
        Else
            Dim repString As String = context.Request.QueryString.Item("Report")
            Dim pilotString As String = context.Request.QueryString.Item("Pilot")
            Dim queueString As String = context.Request.QueryString.Item("Queue")
            repString = repString.Replace("+", " ")
            pilotString = pilotString.Replace("+", " ")
            queueString = queueString.Replace("+", " ")
            strHTML &= IGBHTMLHeader(context, repString & " Report For " & pilotString)
            Dim repPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(pilotString), Pilot)
            Select Case repString
                Case "Training Queue"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Training Queue - " & repPilot.Name & " (" & queueString & ")")
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= EveHQ.Core.Reports.TrainQueue(repPilot, CType(repPilot.TrainingQueues(queueString), SkillQueue))
                Case "Shopping List"
                    strHTML &= EveHQ.Core.Reports.HTMLTitle("Shopping List - " & repPilot.Name & " (" & queueString & ")")
                    strHTML &= EveHQ.Core.Reports.HTMLCharacterDetails(repPilot)
                    strHTML &= EveHQ.Core.Reports.ShoppingList(repPilot, CType(repPilot.TrainingQueues(queueString), SkillQueue))
                Case Else
                    strHTML &= "<p>There was an error generating your character report</p>"
            End Select
            strHTML &= EveHQ.Core.Reports.HTMLFooter
        End If
        Return strHTML
    End Function
#End Region
End Class

