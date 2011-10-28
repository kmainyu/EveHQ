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
Imports EveHQ.Core.RSS
Imports System.ComponentModel

Public Class DBCRSSFeed
    Private ReadOnly feedItems As New ArrayList
    Private ReadOnly feedIDs As New ArrayList
    Public Feeds As New List(Of String)()
    Dim WithEvents FeedWorker As New BackgroundWorker

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Initialise configuration form name
        Me.ControlConfigForm = "EveHQ.DBCRSSFeedConfig"
        FeedWorker = New BackgroundWorker

    End Sub

#Region "Public Overriding Propeties"

    Public Overrides ReadOnly Property ControlName() As String
        Get
            Return "RSS Feed"
        End Get
    End Property

#End Region

#Region "Custom Control Variables"
    Dim cRSSFeed As String = ""
#End Region

#Region "Custom Control Properties"
    Public Property RSSFeed() As String
        Get
            Return cRSSFeed
        End Get
        Set(ByVal value As String)
            cRSSFeed = value
            If ReadConfig = False Then
                Me.SetConfig("RSSFeed", value)
                Me.SetConfig("ControlConfigInfo", "RSS Feed: " & Me.RSSFeed)
            End If
            cpFeed.IsRunning = True
            FeedWorker.RunWorkerAsync()
        End Set
    End Property
#End Region

    Private Sub UpdateFeedDisplay()
        pnlFeedItems.SuspendLayout()
        pnlFeedItems.Controls.Clear()
        If feedItems.Count > 0 Then
            pnlFeedItems.Text = ""
            For Each item As FeedItem In feedItems
                Dim RSSItem As New RSSFeedItem
                RSSItem.lblFeedItemTitle.Text = item.Title
                RSSItem.lblFeedItemTitle.Tag = item.Link
                Dim itemDate As Date
                Date.TryParse(item.PubDate, itemDate)
                RSSItem.lblFeeItemDate.Text = itemDate.ToLongDateString & " " & itemDate.ToLongTimeString
                pnlFeedItems.Controls.Add(RSSItem)
                RSSItem.Dock = DockStyle.Top
                RSSItem.BringToFront()
                lblHeader.Text = "RSS Feed: " & item.Source
            Next
        Else
            pnlFeedItems.Text = "Unable to obtain feed information."
        End If
        pnlFeedItems.ResumeLayout()
    End Sub

#Region "Background Worker Routines"

    Private Sub FeedWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles FeedWorker.DoWork
        Call Me.ParseFeed(cRSSFeed)
    End Sub

    Private Sub FeedWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles FeedWorker.RunWorkerCompleted
        Call Me.UpdateFeedDisplay()
    End Sub

#End Region

#Region "Feed Parsing Routines"

    Private Sub ParseFeed(ByVal URL As String)
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
                If feedIDs.Contains(item.GUID) Then
                    Continue For
                End If

                item.Title = Clean(item.Title).Replace(vbCr, "").Replace(vbLf, "")
                item.Description = Clean(item.Description)

                feedItems.Add(item)
                feedIDs.Add(item.GUID)

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
        Dim regularExpressionString As String = "<.+?>"

        Dim r As New System.Text.RegularExpressions.Regex(regularExpressionString, System.Text.RegularExpressions.RegexOptions.Singleline)
        Return r.Replace(text, "")
    End Function

#End Region

End Class
