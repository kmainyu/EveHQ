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
Imports EveHQ.Controls
Imports EveHQ.Core.RSS
Imports System.Text.RegularExpressions

Namespace Forms

    Public Class FrmHelp

        Private ReadOnly _blogItems As New ArrayList
        Private ReadOnly _twitterItems As New ArrayList
        Private ReadOnly _feedIDs As New ArrayList
        Dim WithEvents _blogUpdateWorker As New BackgroundWorker
        Dim WithEvents _twitterUpdateWorker As New BackgroundWorker

        Private Sub frmHelp_Shown(sender As Object, e As EventArgs) Handles Me.Shown
            tmrUpdate.Start()
        End Sub

        Private Sub UpdateBlogFeedDisplay()
            pnlBlogFeedItems.SuspendLayout()
            pnlBlogFeedItems.Controls.Clear()
            For Each item As FeedItem In _blogItems
                Dim rssItem As New RSSFeedItem
                rssItem.lblFeedItemTitle.Text = item.Title
                rssItem.lblFeedItemTitle.Tag = item.Link
                Dim itemDate As Date
                Date.TryParse(item.PubDate, itemDate)
                rssItem.lblFeeItemDate.Text = itemDate.ToLongDateString & " " & itemDate.ToLongTimeString
                pnlBlogFeedItems.Controls.Add(rssItem)
                rssItem.Dock = DockStyle.Top
                rssItem.BringToFront()
            Next
            pnlBlogFeedItems.ResumeLayout()
        End Sub

        Private Sub UpdateTwitterFeedDisplay()
            pnlTwitterFeedItems.SuspendLayout()
            pnlTwitterFeedItems.Controls.Clear()
            For Each item As FeedItem In _twitterItems
                Dim rssItem As New RSSTwitterItem
                rssItem.lblFeedItemTitle.Text = item.Title.Replace("EveHQ", "<b><a href='" & item.Link & "'>EveHQ</a></b>")
                rssItem.lblFeedItemTitle.Tag = item.Link
                Dim itemDate As Date
                Date.TryParse(item.PubDate, itemDate)
                rssItem.lblFeeItemDate.Text = Format(itemDate, "d MMM")
                pnlTwitterFeedItems.Controls.Add(rssItem)
                rssItem.Dock = DockStyle.Top
                rssItem.BringToFront()
            Next
            pnlTwitterFeedItems.ResumeLayout()
        End Sub

#Region "Feed Parsing Routines"

        Private Sub ParseFeed(ByVal url As String, ByRef feedItems As ArrayList)
            Try

                FeedItems.Clear()

                Dim parser As IFeedParser = ParserFactory.GetParser(url)

                If parser Is Nothing Then
                    Exit Sub
                End If

                Dim parse As List(Of FeedItem) = parser.Parse(url)

                If parse Is Nothing Then
                    Exit Sub
                End If

                For Each item As FeedItem In parse
                    If _feedIDs.Contains(item.GUID) Then
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
            Const regularExpressionString As String = "<.+?>"

            Dim r As New Regex(regularExpressionString, RegexOptions.Singleline)
            Return r.Replace(text, "")
        End Function

#End Region

        Private Sub tmrUpdate_Tick(sender As Object, e As EventArgs) Handles tmrUpdate.Tick
            tmrUpdate.Stop()
            tmrUpdate.Enabled = False
            pbBlogUpdate.Visible = True
            _blogUpdateWorker.RunWorkerAsync()
            pbTwitterUpdate.Visible = True
            _twitterUpdateWorker.RunWorkerAsync()
            wbHelp.Navigate("http://evehq.net/wiki")
        End Sub

        Private Sub BlogUpdater_DoWork(sender As Object, e As DoWorkEventArgs) Handles _blogUpdateWorker.DoWork
            Call ParseFeed("http://evehq.net/feed/", _blogItems)
        End Sub

        Private Sub BlogUpdater_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _blogUpdateWorker.RunWorkerCompleted
            Call UpdateBlogFeedDisplay()
            pbBlogUpdate.Visible = False
        End Sub

        Private Sub TwitterUpdater_DoWork(sender As Object, e As DoWorkEventArgs) Handles _twitterUpdateWorker.DoWork
            Call ParseFeed("http://api.twitter.com/1/statuses/user_timeline.rss?screen_name=EveHQToolkit", _twitterItems)
        End Sub

        Private Sub TwitterUpdater_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _twitterUpdateWorker.RunWorkerCompleted
            Call UpdateTwitterFeedDisplay()
            pbTwitterUpdate.Visible = False
        End Sub

        Private Sub lblBlogFeed_DoubleClick(sender As Object, e As EventArgs) Handles lblBlogFeed.DoubleClick
            pbBlogUpdate.Visible = True
            _blogUpdateWorker.RunWorkerAsync()
        End Sub

        Private Sub lblTwitterFeed_DoubleClick(sender As Object, e As EventArgs) Handles lblTwitterFeed.DoubleClick
            pbTwitterUpdate.Visible = True
            _twitterUpdateWorker.RunWorkerAsync()
        End Sub
    End Class
End NameSpace