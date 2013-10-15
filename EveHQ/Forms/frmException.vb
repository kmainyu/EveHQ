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
Imports System.Text
Imports EveHQ.Core
Imports System.Net.Mail

Namespace Forms

    Public Class FrmException

        Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub btnCopyText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopyText.Click
            Dim errReport As New StringBuilder
            errReport.AppendLine("*EveHQ Error Report*")
            errReport.AppendLine("")
            errReport.AppendLine("EveHQ Version: " & My.Application.Info.Version.ToString)
            errReport.AppendLine("Date: " & Now.ToString)
            errReport.AppendLine("Error: " & lblError.Text)
            errReport.AppendLine("----")
            errReport.AppendLine("\\{panel:title=StackTrace|borderStyle=dashed|bordercolor=#ccc|titleBGColor=#eee|bgColor=#eee}" & txtStackTrace.Text & "\\{panel}")
            Try
                Clipboard.SetText(errReport.ToString)
            Catch ex As Exception
                MessageBox.Show("There was an error copying the data to the clipboard. Please take a screenshot of the error or perform a manual copy of the stack trace.", "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub btnSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSend.Click

            Dim reportingUser As String = HQ.Settings.ErrorReportingName
            Dim reportingEmail As String = HQ.Settings.ErrorReportingEmail

            If (String.IsNullOrEmpty(reportingEmail) Or String.IsNullOrEmpty(reportingUser)) Then
                MessageBox.Show("Your bug report cannot be filed as you have not set an email address and/or name in the Error Reporting settings.", "Submission Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            Try
                Const bugEmailAddress As String = "bugs@evehq.net"
                Const bugSmtpServer As String = "mail.evehq.net"

                Dim subject As String = "EveHQ v" & My.Application.Info.Version.ToString & " Error! - " & lblError.Text
                Dim message As String = "Error Message : " & lblError.Text & "\r\n" & "StackTrace :\r\n" & txtStackTrace.Text
                Dim payload As New MailMessage(reportingEmail, bugEmailAddress, subject, message)
                Dim client As New SmtpClient(bugSmtpServer)
                client.Send(payload)

                MessageBox.Show("Your bug report was successfully submitted and will be reviewed shortly.", "Submission Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                Dim msg As String = "There was a problem submitting your bug report. The error was:" & ControlChars.CrLf & ex.Message
                msg &= "If this error persists, please post the bug report on http://issues.evehq.net manually and make note in the report that sending the report through email failed.."
                MessageBox.Show(msg, "Submission Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try
            
        End Sub

        Private Sub frmException_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            btnSend.Visible = False
            btnSend.Enabled = False
        End Sub

        Private Sub btnContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnContinue.Click
            DialogResult = DialogResult.Ignore
        End Sub
    End Class
End NameSpace