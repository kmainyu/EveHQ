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
Imports System.Reflection
Imports System.Windows.Forms
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Threading
Imports System.Xml

Public Class frmInEveUploader
    Dim queueXML As String = ""

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmInEveSkillsUploader_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Temporarily remove the skill queue uploader
        tabInEve.TabPages.Remove(Me.tabInEveQueues)

        lvwPilots.Items.Clear()
        cboPilots.Items.Clear()
        For Each uPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If uPilot.Active = True Then
                Dim newLI As New ListViewItem
                newLI.Name = uPilot.Name
                newLI.Text = uPilot.Name
                newLI.Checked = False
                newLI.SubItems.Add("")
                lvwPilots.Items.Add(newLI)
                cboPilots.Items.Add(uPilot.Name)
            End If
        Next
        If lvwPilots.Items.Count > 0 Then
            btnUploadSkills.Enabled = True
        Else
            lvwPilots.Items.Add("No pilots to select")
            btnUploadSkills.Enabled = False
        End If
    End Sub

    Private Sub btnUploadSkills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUploadSkills.Click
        ' Clear all the status fields
        For pilot As Integer = 0 To lvwPilots.Items.Count - 1
            lvwPilots.Items(pilot).SubItems(1).Text = ""
        Next

        For pilot As Integer = 0 To lvwPilots.Items.Count - 1
            If lvwPilots.Items(pilot).Checked = True Then
                Dim uPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(lvwPilots.Items(pilot).Name), Core.Pilot)
                lvwPilots.Items(pilot).SubItems(1).Text = "Working..."
                ThreadPool.QueueUserWorkItem(AddressOf Me.CreateReport, uPilot)
            Else
                lvwPilots.Items(pilot).SubItems(1).Text = "n/a"
            End If
        Next
    End Sub
    Private Sub CreateReport(ByVal uPilot As Object)

        Dim rPilot As EveHQ.Core.Pilot = CType(uPilot, Core.Pilot)
        ' Stage 1 - Create the latest report

        Dim strXML As String = ""
        strXML &= EveHQ.Core.Reports.CurrentPilotXML_New(rPilot)
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.reportFolder & "\CurrentXML - New (" & rPilot.Name & ").xml")
        sw.Write(strXML)
        sw.Flush()
        sw.Close()

        ' Stage 2 - Upload the XML data
        Call Me.UploadSkillsFile(rPilot, EveHQ.Core.HQ.reportFolder & "\CurrentXML - New (" & rPilot.Name & ").xml")
        strXML = Nothing
    End Sub

    Private Function GetHTTPResponseStream(ByVal wResponse As HttpWebResponse) As String
        ' Get the stream associated with the response.
        Dim wReceiveStream As Stream = wResponse.GetResponseStream()
        Dim wReadStream As New StreamReader(wReceiveStream, Encoding.UTF8)
        Dim wWebdata As String = wReadStream.ReadToEnd()
        Return wWebdata
    End Function

    Private Sub UploadSkillsFile(ByVal uPilot As EveHQ.Core.Pilot, ByVal strUploadFile As String)
        Dim RemoteURL As String = "http://ineve.net/skills/evehq_upload.php"
        Dim contentType As String = "multipart/form-data"
        Dim fileFormName As String = "file"

        Dim boundary As String
        Dim strTemp As String
        strTemp = DateTime.Now.Ticks.ToString("x")
        boundary = StrDup(39 - strTemp.Length, "-") & strTemp
        Dim HttpRequest As HttpWebRequest = CType(HttpWebRequest.Create(RemoteURL), HttpWebRequest)
        HttpRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*"
        HttpRequest.Referer = "http://ineve.net"
        HttpRequest.Headers.Add("Accept-Language", "en-gb")
        HttpRequest.ContentType = "multipart/form-data; boundary=" + boundary
        HttpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705; .NET CLR 1.1.4322)"
        HttpRequest.KeepAlive = True
        HttpRequest.Method = "POST"
        HttpRequest.ProtocolVersion = HttpVersion.Version10
        HttpRequest.AllowAutoRedirect = False
        Dim sb As New StringBuilder

        sb.Append("--")
        sb.Append(boundary)
        sb.Append(vbCrLf)
        sb.Append("Content-Disposition: form-data; name=""appName""")
        sb.Append(vbCrLf)
        sb.Append(vbCrLf)
        sb.Append("EveHQ")
        sb.Append(vbCrLf)

        sb.Append("--")
        sb.Append(boundary)
        sb.Append(vbCrLf)
        sb.Append("Content-Disposition: form-data; name=""appVersion""")
        sb.Append(vbCrLf)
        sb.Append(vbCrLf)
        sb.Append(My.Application.Info.Version.ToString)
        sb.Append(vbCrLf)

        sb.Append("--")
        sb.Append(boundary)
        sb.Append(vbCrLf)
        sb.Append("content-disposition: form-data; name=""")
        sb.Append(fileFormName)
        sb.Append("""; filename=""")
        sb.Append(strUploadFile)
        sb.Append("""")
        sb.Append(vbCrLf)
        sb.Append("Content-Type: ")
        sb.Append(contentType)
        sb.Append(vbCrLf)
        sb.Append("Content-Transfer-Encoding: binary")
        sb.Append(vbCrLf)
        sb.Append(vbCrLf)
        Dim postHeader As String = sb.ToString()
        Dim postHeaderBytes As Byte() = Encoding.ASCII.GetBytes(postHeader)
        ' Build the trailing boundary string as a byte array
        ' ensuring the boundary appears on a line by itself
        Dim boundaryBytes As Byte() = Encoding.ASCII.GetBytes(vbCrLf & "--" & boundary & "--" & vbCrLf)
        'open file to upload
        Dim fileStream As New FileStream(strUploadFile, FileMode.Open, FileAccess.Read)
        'calculate length of POST
        Dim length As Long = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length
        HttpRequest.ContentLength = length
        'contact server with POST 1.0
        Dim requestStream As Stream = HttpRequest.GetRequestStream()
        ' Write out header to server
        requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length)
        ' Write out the file contents to server in chunks of 4K
        Dim buffer() As Byte
        ReDim buffer(CInt(Math.Min(4095, fileStream.Length)))
        Dim bytesRead As Integer = 1 'initialize to any number except 0
        While bytesRead <> 0
            bytesRead = fileStream.Read(buffer, 0, buffer.Length)
            If bytesRead <> 0 Then 'make sure EOF is not reached
                requestStream.Write(buffer, 0, bytesRead)
            End If
        End While
        ' Write out the trailing boundary
        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length)
        requestStream.Close()
        fileStream.Close()

        Dim importSuccess As Boolean = False
        Try
            Dim HTTPResponse As HttpWebResponse = CType(HttpRequest.GetResponse(), HttpWebResponse)
            Dim strResponse As String = GetHTTPResponseStream(HTTPResponse)
            If strResponse.Contains("Failed") = True Then
                importSuccess = False
            Else
                importSuccess = True
            End If
        Catch e As Exception
            lvwPilots.Items(uPilot.Name).SubItems(1).Text = "Error retrieving response"
            Exit Sub
        End Try

        If importSuccess = True Then
            lvwPilots.Items(uPilot.Name).SubItems(1).Text = "Upload completed successfully"
        Else
            lvwPilots.Items(uPilot.Name).SubItems(1).Text = "Upload failed"
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Try
            Process.Start("http://ineve.net")
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        ' Load skill queues into listview
        lvwQueues.Items.Clear()
        Dim uPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
        For Each skillQueue As EveHQ.Core.SkillQueue In uPilot.TrainingQueues.Values
            Dim newLI As New ListViewItem
            newLI.Name = skillQueue.Name
            newLI.Text = skillQueue.Name
            newLI.Checked = False
            newLI.SubItems.Add("")
            lvwQueues.Items.Add(newLI)
        Next
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Try
            Process.Start("http://ineve.net")
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub btnUploadQueues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUploadQueues.Click
        ' Clear all the status fields
        For queue As Integer = 0 To lvwQueues.Items.Count - 1
            lvwQueues.Items(queue).SubItems(1).Text = ""
        Next

        Dim uPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
        Me.queueXML = ""
        Me.queueXML = ("<?xml version=""1.0"" encoding=""iso-8859-1"" ?>") & vbCrLf
        Me.queueXML &= "<training>" & vbCrLf
        For queue As Integer = 0 To lvwQueues.Items.Count - 1
            If lvwQueues.Items(queue).Checked = True Then
                lvwQueues.Items(queue).SubItems(1).Text = "Working..."
                Dim oQueue As EveHQ.Core.SkillQueue = CType(uPilot.TrainingQueues(lvwQueues.Items(queue).Name), Core.SkillQueue)
                Dim uQueue As EveHQ.Core.SkillQueue = CType(oQueue.Clone, Core.SkillQueue)
                EveHQ.Core.SkillQueueFunctions.RemoveTrainedSkills(uPilot, uQueue)
                uQueue = EveHQ.Core.SkillQueueFunctions.SortQueueByPos(uQueue)
                Me.queueXML &= Me.GenerateQueueXML(uPilot, uQueue)
            Else
                lvwQueues.Items(queue).SubItems(1).Text = "n/a"
            End If
        Next
        Me.queueXML &= "</training>" & vbCrLf

        ' Create and save the XML document
        Dim XMLdoc As XmlDocument = New XmlDocument
        Try
            XMLdoc.LoadXml(Me.queueXML)
            XMLdoc.Save(EveHQ.Core.HQ.reportFolder & "\InEveQ_" & uPilot.Name & ".xml")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Saving Training Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

        ' Upload the file to the website
        Call Me.UploadQueueFile(uPilot, EveHQ.Core.HQ.reportFolder & "\InEveQ_" & uPilot.Name & ".xml")

    End Sub

    Private Function GenerateQueueXML(ByVal cPilot As EveHQ.Core.Pilot, ByVal cQueue As EveHQ.Core.SkillQueue) As String
        Dim XMLS As String = ""

        XMLS &= Chr(9) & "<queue name=""" & cQueue.Name & """ ICT=""" & cQueue.IncCurrentTraining & """ primary=""" & cQueue.Primary & """ >"
        Dim mySkillQueue As EveHQ.Core.SkillQueueItem
        For Each mySkillQueue In cQueue.Queue
            XMLS &= Chr(9) & "<skill>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & "<skillID>" & mySkillQueue.Name & "</skillID>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & "<fromLevel>" & mySkillQueue.FromLevel & "</fromLevel>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & "<toLevel>" & mySkillQueue.ToLevel & "</toLevel>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & "<position>" & mySkillQueue.Pos & "</position>" & vbCrLf
            XMLS &= Chr(9) & "</skill>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</queue>"
        Return XMLS

    End Function

    Private Sub UploadQueueFile(ByVal uPilot As EveHQ.Core.Pilot, ByVal strUploadFile As String)
        Dim RemoteURL As String = "http://ineve.net/skills/evehq_upload.php"
        Dim contentType As String = "multipart/form-data"
        Dim fileFormName As String = "file"

        Dim boundary As String
        Dim strTemp As String
        strTemp = DateTime.Now.Ticks.ToString("x")
        boundary = StrDup(39 - strTemp.Length, "-") & strTemp
        Dim HttpRequest As HttpWebRequest = CType(HttpWebRequest.Create(RemoteURL), HttpWebRequest)
        HttpRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*"
        HttpRequest.Referer = "http://ineve.net"
        HttpRequest.Headers.Add("Accept-Language", "en-gb")
        HttpRequest.ContentType = "multipart/form-data; boundary=" + boundary
        HttpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705; .NET CLR 1.1.4322)"
        HttpRequest.KeepAlive = True
        HttpRequest.Method = "POST"
        HttpRequest.ProtocolVersion = HttpVersion.Version10
        HttpRequest.AllowAutoRedirect = False
        Dim sb As New StringBuilder

        sb.Append("--")
        sb.Append(boundary)
        sb.Append(vbCrLf)
        sb.Append("Content-Disposition: form-data; name=""charID""")
        sb.Append(vbCrLf)
        sb.Append(vbCrLf)
        sb.Append(uPilot.ID)
        sb.Append(vbCrLf)

        sb.Append("--")
        sb.Append(boundary)
        sb.Append(vbCrLf)
        sb.Append("Content-Disposition: form-data; name=""appName""")
        sb.Append(vbCrLf)
        sb.Append(vbCrLf)
        sb.Append("EveHQ")
        sb.Append(vbCrLf)

        sb.Append("--")
        sb.Append(boundary)
        sb.Append(vbCrLf)
        sb.Append("Content-Disposition: form-data; name=""appVersion""")
        sb.Append(vbCrLf)
        sb.Append(vbCrLf)
        sb.Append(My.Application.Info.Version.ToString)
        sb.Append(vbCrLf)

        sb.Append("--")
        sb.Append(boundary)
        sb.Append(vbCrLf)
        sb.Append("content-disposition: form-data; name=""")
        sb.Append(fileFormName)
        sb.Append("""; filename=""")
        sb.Append(strUploadFile)
        sb.Append("""")
        sb.Append(vbCrLf)
        sb.Append("Content-Type: ")
        sb.Append(contentType)
        sb.Append(vbCrLf)
        sb.Append("Content-Transfer-Encoding: binary")
        sb.Append(vbCrLf)
        sb.Append(vbCrLf)
        Dim postHeader As String = sb.ToString()
        Dim postHeaderBytes As Byte() = Encoding.ASCII.GetBytes(postHeader)
        ' Build the trailing boundary string as a byte array
        ' ensuring the boundary appears on a line by itself
        Dim boundaryBytes As Byte() = Encoding.ASCII.GetBytes(vbCrLf & "--" & boundary & "--" & vbCrLf)
        'open file to upload
        Dim fileStream As New FileStream(strUploadFile, FileMode.Open, FileAccess.Read)
        'calculate length of POST
        Dim length As Long = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length
        HttpRequest.ContentLength = length
        'contact server with POST 1.0
        Dim requestStream As Stream = HttpRequest.GetRequestStream()
        ' Write out header to server
        requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length)
        ' Write out the file contents to server in chunks of 4K
        Dim buffer() As Byte
        ReDim buffer(CInt(Math.Min(4095, fileStream.Length)))
        Dim bytesRead As Integer = 1 'initialize to any number except 0
        While bytesRead <> 0
            bytesRead = fileStream.Read(buffer, 0, buffer.Length)
            If bytesRead <> 0 Then 'make sure EOF is not reached
                requestStream.Write(buffer, 0, bytesRead)
            End If
        End While
        ' Write out the trailing boundary
        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length)
        requestStream.Close()
        fileStream.Close()

        Dim importSuccess As Boolean = False
        Try
            Dim HTTPResponse As HttpWebResponse = CType(HttpRequest.GetResponse(), HttpWebResponse)
            Dim strResponse As String = GetHTTPResponseStream(HTTPResponse)
            If strResponse.Contains("Failed") = True Then
                importSuccess = False
            Else
                importSuccess = True
            End If
        Catch e As Exception
            For queue As Integer = 0 To lvwQueues.Items.Count - 1
                If lvwQueues.Items(queue).Checked = True Then
                    lvwQueues.Items(queue).SubItems(1).Text = "Error retrieving response"
                End If
            Next
            Exit Sub
        End Try

        For queue As Integer = 0 To lvwQueues.Items.Count - 1
            If lvwQueues.Items(queue).Checked = True Then
                If importSuccess = True Then
                    lvwQueues.Items(queue).SubItems(1).Text = "Upload completed successfully"
                Else
                    lvwQueues.Items(queue).SubItems(1).Text = "Upload failed"
                End If
            End If
        Next

    End Sub
End Class