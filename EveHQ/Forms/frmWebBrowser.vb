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
Imports System.Web
Imports System.Net
Imports System.Xml
Imports System.IO
Imports System.Text

Public Class frmWebBrowser

    Dim importFlag As Boolean = False
    Dim strFavoriteFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Favorites)

    Private Sub frmWebBrowser_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Load Favourites
        Dim Favorites As String() = System.IO.Directory.GetFiles(strFavoriteFolder)
        ' Process the list of files found in the directory.
        Dim fileName As String
        For Each fileName In Favorites
            Dim fileReader As System.IO.StreamReader
            fileReader = My.Computer.FileSystem.OpenTextFileReader(fileName)
            Dim stringReader As String
            Do
                stringReader = fileReader.ReadLine
            Loop Until fileReader.EndOfStream Or stringReader.StartsWith("URL=")
            If stringReader.StartsWith("URL=") Then
                Dim file2 As String = fileName.Remove(0, Len(strFavoriteFolder) + 1)
                Me.cboFavourites.DropDownItems.Add(file2.Remove(file2.Length - 4))
            End If
        Next fileName
        If EveHQ.Core.HQ.IGBActive = False Then
            btnDB.Enabled = False
        Else
            btnDB.Enabled = True
        End If
        WebBrowser1.Navigate("http://www.evehq.net")
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        WebBrowser1.GoBack()
    End Sub

    Private Sub btnForward_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForward.Click
        WebBrowser1.GoForward()
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        WebBrowser1.Navigate(txtUrl.Text)
    End Sub

    Private Sub txtUrl_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUrl.KeyUp
        If e.KeyCode = Keys.Enter Then
            WebBrowser1.Navigate(txtUrl.Text)
        End If
    End Sub

    Private Sub cboFavourites_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cboFavourites.DropDownItemClicked
        Dim fileName, strURL As String
        fileName = strFavoriteFolder & "/" & e.ClickedItem.Text & ".url"
        Dim fileReader As System.IO.StreamReader
        fileReader = My.Computer.FileSystem.OpenTextFileReader(fileName)
        Dim stringReader As String
        Do
            stringReader = fileReader.ReadLine
        Loop Until fileReader.EndOfStream Or stringReader.StartsWith("URL=")
        If stringReader.StartsWith("URL=") Then
            strURL = stringReader.Remove(0, 4)
            WebBrowser1.Stop()
            WebBrowser1.Navigate(strURL)
        End If
    End Sub

    Private Sub btnDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDB.Click
        Dim strURL As String = "http://localhost:" & EveHQ.Core.HQ.EveHQSettings.IGBPort
        WebBrowser1.Navigate(strURL)
    End Sub

    Private Sub WebBrowser1_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        If WebBrowser1.CanGoBack = True Then
            btnBack.Enabled = True
        Else
            btnBack.Enabled = False
        End If
        If WebBrowser1.CanGoForward = True Then
            btnForward.Enabled = True
        Else
            btnForward.Enabled = False
        End If
        txtUrl.Text = WebBrowser1.Url.ToString
    End Sub
End Class