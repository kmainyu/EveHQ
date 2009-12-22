Public Class frmMail

    Private Sub btnCreateDBTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateDBTables.Click
        Call EveHQ.Core.DataFunctions.CheckForEveMailTable()
        Call EveHQ.Core.DataFunctions.CheckForIDNameTable()
    End Sub
End Class