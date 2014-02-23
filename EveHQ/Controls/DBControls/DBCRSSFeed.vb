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

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports EveHQ.Core

Namespace Controls.DBControls

    Public Class DBCRSSFeed
        Private ReadOnly _feedItems As New ArrayList
        Private ReadOnly _feedIDs As New ArrayList
        Public Feeds As New List(Of String)()
        Dim WithEvents _rssFeedWorker As New BackgroundWorker

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            ' Initialise configuration form name
            ControlConfigForm = "EveHQ.Controls.DBConfigs.DBCRSSFeedConfig"
            _rssFeedWorker = New BackgroundWorker

        End Sub

#Region "Public Overriding Propeties"

        Public Overrides ReadOnly Property ControlName() As String
            Get
                Return "RSS Feed"
            End Get
        End Property

#End Region

#Region "Custom Control Variables"
        Dim _rssSFeed As String = ""
#End Region

#Region "Custom Control Properties"
        Public Property RSSFeed() As String
            Get
                Return _rssSFeed
            End Get
            Set(ByVal value As String)
                _rssSFeed = value
                If ReadConfig = False Then
                    SetConfig("RSSFeed", value)
                    SetConfig("ControlConfigInfo", "RSS Feed: " & RSSFeed)
                End If
                cpFeed.IsRunning = True
                _rssFeedWorker.RunWorkerAsync()
            End Set
        End Property
#End Region

        Private Sub UpdateFeedDisplay()
            pnlFeedItems.SuspendLayout()
            pnlFeedItems.Controls.Clear()
            If _feedItems.Count > 0 Then
                pnlFeedItems.Text = ""
                For Each item As FeedItem In _feedItems
                    Dim rssItem As New RSSFeedItem
                    rssItem.lblFeedItemTitle.Text = item.Title
                    rssItem.lblFeedItemTitle.Tag = item.Link
                    Dim itemDate As Date
                    Date.TryParse(item.PubDate, itemDate)
                    rssItem.lblFeeItemDate.Text = itemDate.ToLongDateString & " " & itemDate.ToLongTimeString
                    pnlFeedItems.Controls.Add(rssItem)
                    rssItem.Dock = DockStyle.Top
                    rssItem.BringToFront()
                    lblHeader.Text = "RSS Feed: " & item.Source
                Next
            Else
                pnlFeedItems.Text = "Unable to obtain feed information."
            End If
            pnlFeedItems.ResumeLayout()
        End Sub

#Region "Background Worker Routines"

        Private Sub FeedWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles _rssFeedWorker.DoWork
            Call ParseFeed(_rssSFeed)
        End Sub

        Private Sub FeedWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _rssFeedWorker.RunWorkerCompleted
            Call UpdateFeedDisplay()
        End Sub

#End Region

#Region "Feed Parsing Routines"

        Private Sub ParseFeed(ByVal url As String)
            Try

                Dim parser As IFeedParser = ParserFactory.GetParser(URL)

                If parser Is Nothing Then
                    Exit Sub
                End If

                Dim parse As List(Of FeedItem) = parser.Parse(URL)

                If parse Is Nothing Then
                    Exit Sub
                End If

                For Each item As FeedItem In parse
                    If _feedIDs.Contains(item.GUID) Then
                        Continue For
                    End If

                    item.Title = Clean(item.Title).Replace(vbCr, "").Replace(vbLf, "")
                    item.Description = Clean(item.Description)

                    _feedItems.Add(item)
                    _feedIDs.Add(item.GUID)

                Next
            Catch
                'Suppress any errors 
            End Try
        End Sub

        Private Shared Function Clean(ByVal input As String) As String
            Dim output As String = input.Trim()

            output = output.Replace("&#39;", "'")
            output = output.Replace("&amp;", "&")
            output = output.Replace("&quot;", """")
            output = output.Replace("&nbsp;", " ")

            output = RemoveHTMLTags(output)

            Return output
        End Function

        Private Shared Function RemoveHTMLTags(ByVal text As String) As String
            Const RegularExpressionString As String = "<.+?>"

            Dim r As New Regex(RegularExpressionString, RegexOptions.Singleline)
            Return r.Replace(text, "")
        End Function

#End Region

    End Class
End NameSpace