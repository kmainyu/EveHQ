Imports System.Text
Imports System.Net
Imports System.IO

Public Class frmException

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnCopyText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyText.Click
        Dim errReport As New StringBuilder
        errReport.AppendLine("[size=12pt][b]EveHQ Error Report[/b][/size]")
        errReport.AppendLine("")
        errReport.AppendLine("EveHQ Version: " & My.Application.Info.Version.ToString)
        errReport.AppendLine("Date: " & FormatDateTime(Now, DateFormat.GeneralDate))
        errReport.AppendLine("Error: " & lblError.Text)
        errReport.AppendLine("Stack Trace:")
        errReport.AppendLine("[code]" & txtStackTrace.Text & "[/code]")
        Clipboard.SetText(errReport.ToString)
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        Dim strResponse As String = ""
        Dim remoteURL As String = "http://www.evehq.net/bugs/report.aspx"
        Dim postData As String = "reporter=" & System.Web.HttpUtility.UrlEncode(EveHQ.Core.HQ.EveHQSettings.ErrorReportingName)
        postData &= "&reporteremail=" & System.Web.HttpUtility.UrlEncode(EveHQ.Core.HQ.EveHQSettings.ErrorReportingEmail)
        postData &= "&version=" & My.Application.Info.Version.ToString
        postData &= "&message=" & System.Web.HttpUtility.UrlEncode(lblError.Text)
        postData &= "&trace=" & System.Web.HttpUtility.UrlEncode(txtStackTrace.Text)

        Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
        ' Setup proxy server (if required)
        If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
            Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
            If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                EveHQProxy.UseDefaultCredentials = True
            Else
                EveHQProxy.UseDefaultCredentials = False
                EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
            End If
            request.Proxy = EveHQProxy
        End If
        ' Setup request parameters
        request.Method = "POST"
        request.ContentLength = postData.Length
        request.ContentType = "application/x-www-form-urlencoded"
        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
        ' Setup a stream to write the HTTP "POST" data
        Dim WebEncoding As New ASCIIEncoding()
        Dim byte1 As Byte() = WebEncoding.GetBytes(postData)
        Dim newStream As Stream = request.GetRequestStream()
        newStream.Write(byte1, 0, byte1.Length)
        newStream.Close()
        ' Prepare for a response from the server
        Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
        ' Get the stream associated with the response.
        Dim receiveStream As Stream = response.GetResponseStream()
        ' Pipes the stream to a higher level stream reader with the required encoding format. 
        Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
        strResponse = readStream.ReadToEnd()

        ' Check response string for any error codes?
        If strResponse = "The bug report was successfully submitted." Then
            ' We have a successful write to the database
            btnSend.Enabled = False
            MessageBox.Show("Your bug report was successfully submitted and will be reviewed shortly.", "Submission Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim msg As String = "There was a problem submitting your bug report. The error was:" & ControlChars.CrLf & strResponse
            msg &= "If this error persists, please post the bug report on the forum and notify the administrator of the problems in directly submitting reports."
            MessageBox.Show(msg, "Submission Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub frmException_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnSend.Enabled = EveHQ.Core.HQ.EveHQSettings.ErrorReportingEnabled
        Try
            My.Computer.Audio.Play(My.Resources.doh3, AudioPlayMode.Background)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        Me.DialogResult = Windows.Forms.DialogResult.Ignore
    End Sub
End Class