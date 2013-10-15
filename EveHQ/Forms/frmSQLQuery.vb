' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports EveHQ.Core
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Web

Namespace Forms

    Public Class FrmSQLQuery

        Dim _currentQuery As String = ""
        Dim _cQueryAmended As Boolean = False
        Dim _queryIsUpdating As Boolean = False

        Private Property QueryAmended() As Boolean
            Get
                Return _cQueryAmended
            End Get
            Set(ByVal value As Boolean)
                _cQueryAmended = value
                lblQueryAmended.Visible = value
            End Set
        End Property

        Private Sub frmSQLQuery_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            QueryAmended = False
            Call UpdateQueries()
        End Sub

#Region "Query Handling Routines"
        Private Sub ExecuteQuery(ByVal strSQL As String)
            Dim sqlData As DataSet
            Dim recordsAffected As Integer
            Dim writeQuery As Boolean
            If strSQL.ToLower.Contains("select") = False Then
                writeQuery = True
            End If
            If writeQuery = True Then
                dgvQuery.DataSource = Nothing
                recordsAffected = CustomDataFunctions.SetCustomData(strSQL)
                If recordsAffected <> -2 Then
                    lblRowCount.Text = "Records Affected: " & recordsAffected.ToString("N0")
                Else
                    lblRowCount.Text = "Write Query: Failed - Check SQL syntax and available data."
                End If
            Else
                sqlData = CustomDataFunctions.GetCustomData(strSQL)
                If sqlData IsNot Nothing Then
                    If sqlData.Tables.Count > 0 Then
                        dgvQuery.DataSource = sqlData.Tables(0)
                        lblRowCount.Text = "Record Count: " & sqlData.Tables(0).Rows.Count.ToString("N0")
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
            For Each query As String In HQ.Settings.SQLQueries.Keys
                lvwQueries.Items.Add(query)
            Next
            lvwQueries.EndUpdate()
            btnRename.Enabled = False
            btnDelete.Enabled = False
        End Sub

        Private Sub UpdateQuery(ByVal query As String)
            ' Set the SQL query text
            _queryIsUpdating = True
            _currentQuery = query
            txtQuery.Text = HQ.Settings.SQLQueries(query)
            lblQueryText.Text = "SQL Query String: " & query
            panelText.Refresh()
            QueryAmended = False
            _queryIsUpdating = False
        End Sub

        Private Sub SaveQuery()
            ' Check is this is a current query
            If _currentQuery = "" Then
                ' We need a name so create a text name box
                Dim textForm As New FrmModifyText
                textForm.Text = "Enter SQL Query Name"
                textForm.lblDescription.Text = "Please enter a name for this SQL query"
                textForm.ShowDialog()
                If textForm.DialogResult = DialogResult.OK Then
                    HQ.Settings.SQLQueries.Add(textForm.TextData, txtQuery.Text)
                    Call UpdateQueries()
                    Call UpdateQuery(textForm.TextData)
                    QueryAmended = False
                Else
                    MessageBox.Show("The saving of the SQL query has been cancelled.", "SQL Query Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                textForm.Dispose()
            Else
                ' Save the query into the list
                HQ.Settings.SQLQueries(_currentQuery) = txtQuery.Text
                QueryAmended = False
            End If
        End Sub

        Private Sub RenameQuery(ByVal oldQuery As String)
            ' We need a name so create a text name box
            Dim textForm As New FrmModifyText
            textForm.Text = "Rename SQL Query"
            textForm.lblDescription.Text = "Please enter a new name for this SQL query"
            textForm.txtText.Text = oldQuery
            textForm.ShowDialog()
            If textForm.DialogResult = DialogResult.OK Then
                ' Remove the old data
                Dim sql As String = HQ.Settings.SQLQueries(oldQuery)
                HQ.Settings.SQLQueries.Remove(oldQuery)
                HQ.Settings.SQLQueries.Add(textForm.TextData, sql)
                Call UpdateQueries()
                Call UpdateQuery(textForm.TextData)
                QueryAmended = False
            Else
                MessageBox.Show("The saving of the SQL query has been cancelled.", "SQL Query Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            textForm.Dispose()
        End Sub

        Private Sub ResetCurrentQuery()
            _currentQuery = ""
            txtQuery.Text = ""
            lblQueryText.Text = "SQL Query String: <new>"
            dgvQuery.DataSource = Nothing
            QueryAmended = False
        End Sub

#End Region

#Region "Ribbon Button Routines"
        Private Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNew.Click
            If QueryAmended = True Then
                ' Ask if this query needs to be saved
                Dim reply As DialogResult = MessageBox.Show("This query has not been saved. Would you like to save it now?", "SQL Query Tool", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Select Case reply
                    Case DialogResult.Cancel
                        Exit Sub
                    Case DialogResult.Yes
                        ' Save the query
                        Call SaveQuery()
                    Case DialogResult.No
                        ' Do nothing, just wipe the query as is
                End Select
            End If
            ' Reset the query data
            Call ResetCurrentQuery()
        End Sub

        Private Sub btnRename_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRename.Click
            If lvwQueries.SelectedItems.Count > 0 Then
                Dim query As String = lvwQueries.SelectedItems(0).Text
                Call RenameQuery(query)
            End If
        End Sub

        Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
            If lvwQueries.SelectedItems.Count > 0 Then
                Dim query As String = lvwQueries.SelectedItems(0).Text
                Dim reply As DialogResult = MessageBox.Show("Are you sure you want to delete this query?", "SQL Query Tool", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Select Case reply
                    Case DialogResult.Cancel
                        Exit Sub
                    Case DialogResult.No
                        Exit Sub
                    Case DialogResult.Yes
                        HQ.Settings.SQLQueries.Remove(query)
                        Call UpdateQueries()
                        ' Check if we need to reset the current query
                        If _currentQuery = query Then
                            Call ResetCurrentQuery()
                        End If
                End Select
            End If
        End Sub

        Private Sub btnExecute_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExecute.Click
            Call ExecuteQuery(txtQuery.Text)
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
            If txtQuery.Text = "" Then
                MessageBox.Show("There must be some text in the SQL Query String box before you can save it!", "SQL Query Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Call SaveQuery()
            End If
        End Sub

        Private Sub btnExportData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportData.Click
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
                        Call ExportCSV(sfd.FileName)
                    Case 2
                        Call ExportTSV(sfd.FileName)
                    Case 3
                        Call ExportXML(sfd.FileName)
                End Select
            End If
            sfd.Dispose()
            MessageBox.Show("Export of data completed!", "SQL Query Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub btnCopyData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopyData.Click
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

        Private Sub ExportCsv(ByVal filename As String)
            Dim sw As New StreamWriter(filename)
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
                        sb.Append(HQ.Settings.CsvSeparatorChar)
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

        Private Sub ExportTsv(ByVal filename As String)
            Dim sw As New StreamWriter(filename)
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

        Private Sub ExportXML(ByVal filename As String)
            Dim dt As DataTable = CType(dgvQuery.DataSource, DataTable)
            Dim xmlDoc As New XmlDocument
            Dim dec As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", Nothing, Nothing)
            xmlDoc.AppendChild(dec)

            ' Create XML root
            Dim xmlRoot As XmlElement = xmlDoc.CreateElement("eveData")
            xmlDoc.AppendChild(xmlRoot)

            ' Create main XML data
            For Each row As DataRow In dt.Rows
                Dim xmlRow As XmlElement = xmlDoc.CreateElement("row")
                xmlRoot.AppendChild(xmlRow)
                For Each col As DataColumn In dt.Columns
                    Dim xmlCol As XmlNode = xmlDoc.CreateElement(col.ColumnName)
                    xmlCol.InnerText = HttpUtility.HtmlEncode(row.Item(col).ToString)
                    xmlRow.AppendChild(xmlCol)
                Next
            Next

            ' Save the XML file
            xmlDoc.Save(filename)

        End Sub

#End Region

#Region "Misc UI Routines"
        Private Sub txtQuery_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtQuery.TextChanged
            If _queryIsUpdating = False Then
                QueryAmended = True
            End If
        End Sub

        Private Sub lvwQueries_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles lvwQueries.DoubleClick
            If lvwQueries.SelectedItems.Count > 0 Then
                If _cQueryAmended = True Then
                    ' Ask if this query needs to be saved
                    Dim reply As DialogResult = MessageBox.Show("This query has not been saved. Would you like to save it now?", "SQL Query Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    Select Case reply
                        Case DialogResult.Yes
                            ' Save the query
                            Call SaveQuery()
                        Case DialogResult.No
                            ' Do nothing, just wipe the query as is
                    End Select
                End If
                Dim query As String = lvwQueries.SelectedItems(0).Text
                Call UpdateQuery(query)
                Call ExecuteQuery(txtQuery.Text)
            End If
        End Sub

        Private Sub lvwQueries_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lvwQueries.SelectedIndexChanged
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
End Namespace