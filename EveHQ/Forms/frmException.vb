Imports System.Text

Public Class frmException

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
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
End Class