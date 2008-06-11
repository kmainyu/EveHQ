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

    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        ' Check the URL to see if it is something we can use (via the importFlag!)
        If importFlag = False Then
            If e.Url.AbsoluteUri.StartsWith("http://myeve.eve-online.com/character/skilltree.asp?characterID=") Or e.Url.AbsoluteUri.StartsWith("http://myeve.eve-online.com/character/xml.asp?characterID=") Then
                Dim viewID() As String = txtUrl.Text.Split(CChar("="))
                Dim charID As String = viewID(1)
                Dim webPilot As EveHQ.Core.Pilot
                Dim pilotUsed As Boolean = False
                Dim msg As String = "It looks like you are viewing your Eve-Online character information!" & ControlChars.CrLf & ControlChars.CrLf
                For Each webPilot In EveHQ.Core.HQ.Pilots
                    If webPilot.ID = charID Then
                        pilotUsed = True
                        msg &= "You are using EveHQ to monitor this character. Would you like to import the XML data into EveHQ?" & ControlChars.CrLf & ControlChars.CrLf
                        Dim webReply As Integer = MessageBox.Show(msg, "Eve-Online Character Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If webReply = Windows.Forms.DialogResult.Yes Then
                            Call ImportPilot(charID)
                        End If
                        Exit Sub            ' Not needing to be here anymore therefore leave
                    End If
                Next
                msg &= "Would you like to import this character into EveHQ and monitor it from there?" & ControlChars.CrLf
                Dim reply As Integer = MessageBox.Show(msg, "Eve-Online Character Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = Windows.Forms.DialogResult.Yes Then
                    Call ImportPilot(charID)
                End If
            End If
        Else
            If WebBrowser1.Url.ToString.EndsWith("m=t") Then
                Call ImportPilotPart2()
            Else
                Call ImportPilotPart1()
            End If
        End If
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

    Private Sub ImportPilot(ByVal charID As String)
        Dim newPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        newPilot.Name = "___" & charID
        newPilot.ID = charID
        newPilot.Account = ""
        newPilot.AccountPosition = "0"
        importFlag = True
        EveHQ.Core.HQ.TPilots.Clear()
        EveHQ.Core.HQ.TPilots.Add(newPilot)
        WebBrowser1.Navigate("http://myeve.eve-online.com/character/xml.asp?characterID=" & newPilot.ID)
    End Sub

    Private Sub ImportPilotPart1()
        Dim cPilot, nPilot As New EveHQ.Core.Pilot
        For Each cPilot In EveHQ.Core.HQ.TPilots
            Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.cacheFolder & "\cimp" & cPilot.ID & ".html")
            sw.Write(WebBrowser1.DocumentText)
            sw.Flush()
            sw.Close()
        Next
        Dim XMLResult As String = ""
        XMLResult = HTML_XML.GetPathForCachedFile("http://myeve.eve-online.com/character/xml.asp?characterID=" & cPilot.ID)

        Try
            cPilot.PilotData.Load(XMLResult)
            cPilot.PilotData.Save(EveHQ.Core.HQ.cacheFolder & "\c" & cPilot.ID & ".xml")
        Catch e As XmlException
            MsgBox("Error: " & e.Message)
            Exit Sub
        End Try

        WebBrowser1.Navigate("http://myeve.eve-online.com/character/xml.asp?characterID=" & cPilot.ID & "&m=t")
    End Sub

    Private Sub ImportPilotPart2()
        Dim cPilot, nPilot As New EveHQ.Core.Pilot
        For Each cPilot In EveHQ.Core.HQ.TPilots
            Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.cacheFolder & "\cimp" & cPilot.ID & ".html")
            sw.Write(WebBrowser1.DocumentText)
            sw.Flush()
            sw.Close()
        Next
        Dim XMLResult As String = ""
        XMLResult = HTML_XML.GetPathForCachedFile("http://myeve.eve-online.com/character/xml.asp?characterID=" & cPilot.ID & "&m=t")

        Try
            cPilot.PilotTrainingData.Load(XMLResult)
            cPilot.PilotTrainingData.Save(EveHQ.Core.HQ.cacheFolder & "\t" & cPilot.ID & ".xml")
        Catch e As XmlException
            MsgBox("Error: " & e.Message)
            Exit Sub
        End Try

        Call EveHQ.Core.PilotParseFunctions.ImportWebPilot()
        Call frmEveHQ.UpdatePilotInfo()
        Call frmSettings.UpdatePilots()

        importFlag = False

        MessageBox.Show("XML Data successfully imported!", "Import Pilot Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
        WebBrowser1.Navigate("http://myeve.eve-online.com/character/skilltree.asp")

    End Sub

End Class