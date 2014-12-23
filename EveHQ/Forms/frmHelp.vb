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
Imports EveHQ.Core
Imports EveHQ.Controls
Imports System.Text.RegularExpressions

Namespace Forms

    Public Class FrmHelp

        Private ReadOnly _blogItems As New ArrayList
        Private ReadOnly _forumItems As New ArrayList
        Private ReadOnly _feedIDs As New ArrayList
        Dim WithEvents _blogUpdateWorker As New BackgroundWorker
        Dim WithEvents _forumUpdateWorker As New BackgroundWorker

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

        Private Sub UpdateForumFeedDisplay()
            pnlForumFeedItems.SuspendLayout()
            pnlForumFeedItems.Controls.Clear()
            For Each item As FeedItem In _forumItems
                Dim rssItem As New RSSFeedItem
                rssItem.lblFeedItemTitle.Text = item.Title
                rssItem.lblFeedItemTitle.Tag = item.Link
                Dim itemDate As Date
                Date.TryParse(item.PubDate, itemDate)
                rssItem.lblFeeItemDate.Text = itemDate.ToLongDateString & " " & itemDate.ToLongTimeString
                pnlForumFeedItems.Controls.Add(rssItem)
                rssItem.Dock = DockStyle.Top
                rssItem.BringToFront()
            Next
            pnlForumFeedItems.ResumeLayout()
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

        Private Shared Function RemoveHtmlTags(ByVal text As String) As String
            Const RegularExpressionString As String = "<.+?>"

            Dim r As New Regex(RegularExpressionString, RegexOptions.Singleline)
            Return r.Replace(text, "")
        End Function

#End Region

        Private Sub tmrUpdate_Tick(sender As Object, e As EventArgs) Handles tmrUpdate.Tick
            tmrUpdate.Stop()
            tmrUpdate.Enabled = False
            pbBlogUpdate.Visible = True
            _blogUpdateWorker.RunWorkerAsync()
            pbForumUpdate.Visible = True
            _forumUpdateWorker.RunWorkerAsync()
            wbHelp.Navigate("http://newedentech.com/wiki")
        End Sub

        Private Sub BlogUpdater_DoWork(sender As Object, e As DoWorkEventArgs) Handles _blogUpdateWorker.DoWork
            Call ParseFeed("http://newedentech.com/feed/", _blogItems)
        End Sub

        Private Sub BlogUpdater_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _blogUpdateWorker.RunWorkerCompleted
            Call UpdateBlogFeedDisplay()
            pbBlogUpdate.Visible = False
        End Sub

        Private Sub ForumUpdater_DoWork(sender As Object, e As DoWorkEventArgs) Handles _forumUpdateWorker.DoWork
            Call ParseFeed("http://newedentech.com/forum/index.php?action=.xml;type=rss;sa=recent;limit=10", _forumItems)
        End Sub

        Private Sub ForumUpdater_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _forumUpdateWorker.RunWorkerCompleted
            Call UpdateForumFeedDisplay()
            pbForumUpdate.Visible = False
        End Sub

        Private Sub lblBlogFeed_DoubleClick(sender As Object, e As EventArgs) Handles lblBlogFeed.DoubleClick
            pbBlogUpdate.Visible = True
            _blogUpdateWorker.RunWorkerAsync()
        End Sub

        Private Sub lblForumFeed_DoubleClick(sender As Object, e As EventArgs) Handles lblForumFeed.DoubleClick
            pbForumUpdate.Visible = True
            _forumUpdateWorker.RunWorkerAsync()
        End Sub
    End Class
End NameSpace