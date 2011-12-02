' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports System.Data
Imports System.IO
Imports System.Text
Imports System.Xml

Public Class frmSQLQuery

    Dim currentQuery As String = ""
    Dim cQueryAmended As Boolean = False
    Dim QueryIsUpdating As Boolean = False

    Private Property QueryAmended() As Boolean
        Get
            Return cQueryAmended
        End Get
        Set(ByVal value As Boolean)
            cQueryAmended = value
            lblQueryAmended.Visible = value
        End Set
    End Property

    Private Sub frmSQLQuery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        QueryAmended = False
        Call Me.UpdateQueries()
    End Sub

#Region "Query Handling Routines"
    Private Sub ExecuteQuery(ByVal strSQL As String)
        Dim SQLData As New DataSet
        Dim RecordsAffected As Integer = 0
        Dim WriteQuery As Boolean = False
        If strSQL.ToLower.Contains("select") = False Then
            WriteQuery = True
        End If
        If WriteQuery = True Then
            RecordsAffected = EveHQ.Core.DataFunctions.SetData(strSQL)
            If RecordsAffected <> -1 Then
                lblRowCount.Text = "Records Affected: " & RecordsAffected.ToString("N0")
            Else
                lblRowCount.Text = "Write Query: Failed - Check SQL syntax and available data."
            End If
        Else
            If radStaticData.Checked = True Then
                SQLData = EveHQ.Core.DataFunctions.GetData(strSQL)
            Else
                SQLData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
            End If
            If SQLData IsNot Nothing Then
                If SQLData.Tables.Count > 0 Then
                    dgvQuery.DataSource = SQLData.Tables(0)
                    lblRowCount.Text = "Record Count: " & SQLData.Tables(0).Rows.Count.ToString("N0")
                Else
                    MessageBox.Show("Data contains no valid data tables! Please check you have the correct database selected and that your SQL Query is properly formatted.", "SQL Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("Unable to retrieve data! Please check you have the correct database selected and that your SQL Query is properly formatted.", "SQL Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub UpdateQueries()
        ' Update Saved SQL Queries 
        lvwQueries.BeginUpdate()
        lvwQueries.Items.Clear()
        For Each query As String In EveHQ.Core.HQ.EveHQSettings.SQLQueries.Keys
            lvwQueries.Items.Add(query)
        Next
        lvwQueries.EndUpdate()
        btnRename.Enabled = False
        btnDelete.Enabled = False
    End Sub

    Private Sub UpdateQuery(ByVal query As String)
        ' Set the SQL query text
        QueryIsUpdating = True
        currentQuery = query
        txtQuery.Text = EveHQ.Core.HQ.EveHQSettings.SQLQueries(query)
        lblQueryText.Text = "SQL Query String: " & query
        panelText.Refresh()
        QueryAmended = False
        QueryIsUpdating = False
    End Sub

    Private Sub SaveQuery()
        ' Check is this is a current query
        If currentQuery = "" Then
            ' We need a name so create a text name box
            Dim TextForm As New frmModifyText
            TextForm.Text = "Enter SQL Query Name"
            TextForm.lblDescription.Text = "Please enter a name for this SQL query"
            TextForm.ShowDialog()
            If TextForm.DialogResult = Windows.Forms.DialogResult.OK Then
                EveHQ.Core.HQ.EveHQSettings.SQLQueries.Add(TextForm.TextData, txtQuery.Text)
                Call Me.UpdateQueries()
                Call Me.UpdateQuery(TextForm.TextData)
                QueryAmended = False
            Else
                MessageBox.Show("The saving of the SQL query has been cancelled.", "SQL Query Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            TextForm.Dispose()
        Else
            ' Save the query into the list
            EveHQ.Core.HQ.EveHQSettings.SQLQueries(currentQuery) = txtQuery.Text
            QueryAmended = False
        End If
    End Sub

    Private Sub RenameQuery(ByVal OldQuery As String)
        ' We need a name so create a text name box
        Dim TextForm As New frmModifyText
        TextForm.Text = "Rename SQL Query"
        TextForm.lblDescription.Text = "Please enter a new name for this SQL query"
        TextForm.txtText.Text = OldQuery
        TextForm.ShowDialog()
        If TextForm.DialogResult = Windows.Forms.DialogResult.OK Then
            ' Remove the old data
            Dim SQL As String = EveHQ.Core.HQ.EveHQSettings.SQLQueries(OldQuery)
            EveHQ.Core.HQ.EveHQSettings.SQLQueries.Remove(OldQuery)
            EveHQ.Core.HQ.EveHQSettings.SQLQueries.Add(TextForm.TextData, SQL)
            Call Me.UpdateQueries()
            Call Me.UpdateQuery(TextForm.TextData)
            QueryAmended = False
        Else
            MessageBox.Show("The saving of the SQL query has been cancelled.", "SQL Query Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        TextForm.Dispose()
    End Sub

    Private Sub ResetCurrentQuery()
        currentQuery = ""
        txtQuery.Text = ""
        lblQueryText.Text = "SQL Query String: <new>"
        dgvQuery.DataSource = Nothing
        QueryAmended = False
    End Sub

#End Region

#Region "Ribbon Button Routines"
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If QueryAmended = True Then
            ' Ask if this query needs to be saved
            Dim reply As DialogResult = MessageBox.Show("This query has not been saved. Would you like to save it now?", "SQL Query Tool", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            Select Case reply
                Case Windows.Forms.DialogResult.Cancel
                    Exit Sub
                Case Windows.Forms.DialogResult.Yes
                    ' Save the query
                    Call Me.SaveQuery()
                Case Windows.Forms.DialogResult.No
                    ' Do nothing, just wipe the query as is
            End Select
        End If
        ' Reset the query data
        Call Me.ResetCurrentQuery()
    End Sub

    Private Sub btnRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRename.Click
        If lvwQueries.SelectedItems.Count > 0 Then
            Dim query As String = lvwQueries.SelectedItems(0).Text
            Call Me.RenameQuery(query)
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If lvwQueries.SelectedItems.Count > 0 Then
            Dim query As String = lvwQueries.SelectedItems(0).Text
            Dim reply As DialogResult = MessageBox.Show("Are you sure you want to delete this query?", "SQL Query Tool", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            Select Case reply
                Case Windows.Forms.DialogResult.Cancel
                    Exit Sub
                Case Windows.Forms.DialogResult.No
                    Exit Sub
                Case Windows.Forms.DialogResult.Yes
                    EveHQ.Core.HQ.EveHQSettings.SQLQueries.Remove(query)
                    Call Me.UpdateQueries()
                    ' Check if we need to reset the current query
                    If currentQuery = query Then
                        Call Me.ResetCurrentQuery()
                    End If
            End Select
        End If
    End Sub

    Private Sub btnExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecute.Click
        Call Me.ExecuteQuery(txtQuery.Text)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtQuery.Text = "" Then
            MessageBox.Show("There must be some text in the SQL Query String box before you can save it!", "SQL Query Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Call Me.SaveQuery()
        End If
    End Sub

    Private Sub btnExportData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportData.Click
        Dim sfd As New SaveFileDialog
        sfd.Title = "Export Data Grid"
        sfd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        Dim filterText As String = "Comma Separated Variable files (*.csv)|*.csv"
        filterText &= "|Tab Separated Variable files (*.txt)|*.txt"
        filterText &= "|XML files (*.xml)|*.xml"
        sfd.Filter = filterText
        sfd.FilterIndex = 0
        sfd.AddExtension = True
        sfd.ShowDialog()
        sfd.CheckPathExists = True
        If sfd.FileName <> "" Then
            Select Case sfd.FilterIndex
                Case 1
                    Call Me.ExportCSV(sfd.FileName)
                Case 2
                    Call Me.ExportTSV(sfd.FileName)
                Case 3
                    Call Me.ExportXML(sfd.FileName)
            End Select
        End If
        sfd.Dispose()
        MessageBox.Show("Export of data completed!", "SQL Query Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCopyData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyData.Click
        Dim sb As New StringBuilder
        Dim dt As DataTable = CType(dgvQuery.DataSource, DataTable)
        For Each col As DataColumn In dt.Columns
            sb.Append(ControlChars.Tab & ControlChars.Quote & col.ColumnName & ControlChars.Quote)
        Next
        sb.Remove(0, 1)
        sb.AppendLine("")
        For Each row As DataRow In dt.Rows
            For col As Integer = 0 To dt.Columns.Count - 1
                If col <> 0 Then
                    sb.Append(ControlChars.Tab)
                End If
                If IsNumeric(row.Item(col)) = True Then
                    sb.Append(row.Item(col).ToString)
                Else
                    sb.Append(ControlChars.Quote & row.Item(col).ToString.Replace(ControlChars.Quote, "'") & ControlChars.Quote)
                End If
            Next
            sb.AppendLine("")
        Next
        Clipboard.SetText(sb.ToString)
    End Sub

#End Region

#Region "Export Routines"

    Private Sub ExportCSV(ByVal Filename As String)
        Dim sw As New StreamWriter(Filename)
        Dim sb As New StringBuilder
        Dim dt As DataTable = CType(dgvQuery.DataSource, DataTable)
        For Each col As DataColumn In dt.Columns
            sb.Append("," & ControlChars.Quote & col.ColumnName & ControlChars.Quote)
        Next
        sb.Remove(0, 1)
        sw.WriteLine(sb.ToString)
        For Each row As DataRow In dt.Rows
            sb = New StringBuilder
            For col As Integer = 0 To dt.Columns.Count - 1
                If col <> 0 Then
                    sb.Append(EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                End If
                If IsNumeric(row.Item(col)) = True Then
                    sb.Append(row.Item(col).ToString)
                Else
                    sb.Append(ControlChars.Quote & row.Item(col).ToString.Replace(ControlChars.Quote, "'") & ControlChars.Quote)
                End If
            Next
            sw.WriteLine(sb.ToString)
        Next
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub

    Private Sub ExportTSV(ByVal Filename As String)
        Dim sw As New StreamWriter(Filename)
        Dim sb As New StringBuilder
        Dim dt As DataTable = CType(dgvQuery.DataSource, DataTable)
        For Each col As DataColumn In dt.Columns
            sb.Append(ControlChars.Tab & ControlChars.Quote & col.ColumnName & ControlChars.Quote)
        Next
        sb.Remove(0, 1)
        sw.WriteLine(sb.ToString)
        For Each row As DataRow In dt.Rows
            sb = New StringBuilder
            For col As Integer = 0 To dt.Columns.Count - 1
                If col <> 0 Then
                    sb.Append(ControlChars.Tab)
                End If
                If IsNumeric(row.Item(col)) = True Then
                    sb.Append(row.Item(col).ToString)
                Else
                    sb.Append(ControlChars.Quote & row.Item(col).ToString.Replace(ControlChars.Quote, "'") & ControlChars.Quote)
                End If
            Next
            sw.WriteLine(sb.ToString)
        Next
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub

    Private Sub ExportXML(ByVal Filename As String)
        Dim dt As DataTable = CType(dgvQuery.DataSource, DataTable)
        Dim xmlDoc As New XmlDocument
        Dim dec As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", Nothing, Nothing)
        xmlDoc.AppendChild(dec)

        ' Create XML root
        Dim xmlRoot As XmlElement = xmlDoc.CreateElement("eveData")
        xmlDoc.AppendChild(xmlRoot)

        ' Create main XML data
        For Each row As DataRow In dt.Rows
            Dim XMLRow As XmlElement = xmlDoc.CreateElement("row")
            xmlRoot.AppendChild(XMLRow)
            For Each col As DataColumn In dt.Columns
                Dim XMLCol As XmlNode = xmlDoc.CreateElement(col.ColumnName)
                XMLCol.InnerText = System.Web.HttpUtility.HtmlEncode(row.Item(col).ToString)
                XMLRow.AppendChild(XMLCol)
            Next
        Next

        ' Save the XML file
        xmlDoc.Save(Filename)

    End Sub

#End Region

#Region "Misc UI Routines"
    Private Sub txtQuery_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQuery.TextChanged
        If QueryIsUpdating = False Then
            QueryAmended = True
        End If
    End Sub

    Private Sub lvwQueries_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwQueries.DoubleClick
        If lvwQueries.SelectedItems.Count > 0 Then
            If cQueryAmended = True Then
                ' Ask if this query needs to be saved
                Dim reply As DialogResult = MessageBox.Show("This query has not been saved. Would you like to save it now?", "SQL Query Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Select Case reply
                    Case Windows.Forms.DialogResult.Yes
                        ' Save the query
                        Call Me.SaveQuery()
                    Case Windows.Forms.DialogResult.No
                        ' Do nothing, just wipe the query as is
                End Select
            End If
            Dim query As String = lvwQueries.SelectedItems(0).Text
            Call Me.UpdateQuery(query)
            Call Me.ExecuteQuery(txtQuery.Text)
        End If
    End Sub

    Private Sub lvwQueries_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwQueries.SelectedIndexChanged
        If lvwQueries.SelectedItems.Count > 0 Then
            btnRename.Enabled = True
            btnDelete.Enabled = True
        Else
            btnRename.Enabled = False
            btnDelete.Enabled = False
        End If
    End Sub
#End Region

End Class