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
            Const regularExpressionString As String = "<.+?>"

            Dim r As New Regex(regularExpressionString, RegexOptions.Singleline)
            Return r.Replace(text, "")
        End Function

#End Region

    End Class
End NameSpace