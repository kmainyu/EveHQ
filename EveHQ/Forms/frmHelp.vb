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

Public Class frmHelp

    Private ReadOnly BlogItems As New ArrayList
    Private ReadOnly TwitterItems As New ArrayList
    Private ReadOnly feedIDs As New ArrayList
    Dim WithEvents BlogUpdater As New BackgroundWorker
    Dim WithEvents TwitterUpdater As New BackgroundWorker

    Private Sub frmHelp_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        tmrUpdate.Start()
    End Sub

    Private Sub UpdateBlogFeedDisplay()
        pnlBlogFeedItems.SuspendLayout()
        pnlBlogFeedItems.Controls.Clear()
        For Each item As EveHQ.Core.RSS.FeedItem In BlogItems
            Dim RSSItem As New RSSFeedItem
            RSSItem.lblFeedItemTitle.Text = item.Title
            RSSItem.lblFeedItemTitle.Tag = item.Link
            Dim itemDate As Date
            Date.TryParse(item.PubDate, itemDate)
            RSSItem.lblFeeItemDate.Text = itemDate.ToLongDateString & " " & itemDate.ToLongTimeString
            pnlBlogFeedItems.Controls.Add(RSSItem)
            RSSItem.Dock = DockStyle.Top
            RSSItem.BringToFront()
        Next
        pnlBlogFeedItems.ResumeLayout()
    End Sub

    Private Sub UpdateTwitterFeedDisplay()
        pnlTwitterFeedItems.SuspendLayout()
        pnlTwitterFeedItems.Controls.Clear()
        For Each item As EveHQ.Core.RSS.FeedItem In TwitterItems
            Dim RSSItem As New RSSTwitterItem
            RSSItem.lblFeedItemTitle.Text = item.Title.Replace("EveHQ", "<b><a href='" & item.Link & "'>EveHQ</a></b>")
            RSSItem.lblFeedItemTitle.Tag = item.Link
            Dim itemDate As Date
            Date.TryParse(item.PubDate, itemDate)
            RSSItem.lblFeeItemDate.Text = Format(itemDate, "d MMM")
            pnlTwitterFeedItems.Controls.Add(RSSItem)
            RSSItem.Dock = DockStyle.Top
            RSSItem.BringToFront()
        Next
        pnlTwitterFeedItems.ResumeLayout()
    End Sub

#Region "Feed Parsing Routines"

    Private Sub ParseFeed(ByVal URL As String, ByRef FeedItems As ArrayList)
        Try

            FeedItems.Clear()

            Dim parser As EveHQ.Core.RSS.IFeedParser = EveHQ.Core.RSS.ParserFactory.GetParser(URL)

            If parser Is Nothing Then
                Exit Sub
            End If

            Dim parse As List(Of EveHQ.Core.RSS.FeedItem) = parser.Parse(URL)

            If parse Is Nothing Then
                Exit Sub
            End If

            For Each item As EveHQ.Core.RSS.FeedItem In parse
                If feedIDs.Contains(item.GUID) Then
                    Continue For
                End If

                item.Title = Clean(item.Title).Replace(vbCr, "").Replace(vbLf, "")
                item.Description = Clean(item.Description)

                FeedItems.Add(item)
                'feedIDs.Add(item.GUID)

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

    Private Sub tmrUpdate_Tick(sender As System.Object, e As System.EventArgs) Handles tmrUpdate.Tick
        tmrUpdate.Stop()
        tmrUpdate.Enabled = False
        pbBlogUpdate.Visible = True
        BlogUpdater.RunWorkerAsync()
        pbTwitterUpdate.Visible = True
        TwitterUpdater.RunWorkerAsync()
        wbHelp.Navigate("http://evehq.net/wiki")
    End Sub

    Private Sub BlogUpdater_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BlogUpdater.DoWork
        Call Me.ParseFeed("http://evehq.net/feed/", BlogItems)
    End Sub

    Private Sub BlogUpdater_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BlogUpdater.RunWorkerCompleted
        Call Me.UpdateBlogFeedDisplay()
        pbBlogUpdate.Visible = False
    End Sub

    Private Sub TwitterUpdater_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles TwitterUpdater.DoWork
        Call Me.ParseFeed("http://api.twitter.com/1/statuses/user_timeline.rss?screen_name=EveHQ", TwitterItems)
    End Sub

    Private Sub TwitterUpdater_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles TwitterUpdater.RunWorkerCompleted
        Call Me.UpdateTwitterFeedDisplay()
        pbTwitterUpdate.Visible = False
    End Sub

    Private Sub lblBlogFeed_DoubleClick(sender As Object, e As System.EventArgs) Handles lblBlogFeed.DoubleClick
        pbBlogUpdate.Visible = True
        BlogUpdater.RunWorkerAsync()
    End Sub

    Private Sub lblTwitterFeed_DoubleClick(sender As Object, e As System.EventArgs) Handles lblTwitterFeed.DoubleClick
        pbTwitterUpdate.Visible = True
        TwitterUpdater.RunWorkerAsync()
    End Sub
End Class