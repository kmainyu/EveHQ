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
Imports System.IO
Imports System.Windows.Forms
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Xml
Imports System.Data.SqlServerCe

Public Class frmDataConvert
    Private Const DBVersion As String = "2.7.0.0"

#Region "SQLCE Version Table Routines"

    Private Sub btnBrowseDB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowseDB.Click
        With ofd1
            .Title = "Select the source SQLCE database to add the version table..."
            .FileName = ""
            .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            .Filter = "SQL CE files (*.sdf)|*.sdf|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Call AddSQLCEVersionTable(ofd1.FileName)
            End If
        End With
    End Sub

    Private Sub AddSQLCEVersionTable(ByVal SourceDB As String)
        Dim strConn As String = "Data Source = " & ControlChars.Quote & SourceDB & ControlChars.Quote & "; Max Database Size = 512; Max Buffer Size = 2048;"
        Dim MySQLCEConnection As New SqlCeConnection(strConn)
        MySQLCEConnection.Open()
        ' Create the table
        Dim strSQL As String = "CREATE TABLE EveHQVersion (Version  nvarchar(10)  NOT NULL);"
        Dim keyCommand As New SqlCeCommand(strSQL, MySQLCEConnection)
        keyCommand.ExecuteNonQuery()
        ' Add the version
        strSQL = "INSERT INTO EveHQVersion (Version) VALUES('" & DBVersion & "');"
        keyCommand = New SqlCeCommand(strSQL, MySQLCEConnection)
        keyCommand.ExecuteNonQuery()
        MySQLCEConnection.Close()
        MessageBox.Show("Table Addition Completed", "AddVersion Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

#Region "Wormhole Routines"

    Private Sub btnGenerateWHClassLocations_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerateWHClassLocations.Click
        With ofd1
            .Title = "Select WH Locations XML file..."
            .FileName = ""
            .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            .Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim WHXML As New XmlDocument
                WHXML.Load(.FileName)
                Dim ClassList As XmlNodeList = WHXML.SelectNodes("data/locationwormholeclasses")
                Dim sw As New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\EveHQ\WHClasses.txt")
                For Each ClassItem As XmlNode In ClassList
                    If CInt(ClassItem.ChildNodes(1).InnerText) >= 1 And CInt(ClassItem.ChildNodes(1).InnerText) <= 6 Then
                        sw.WriteLine(ClassItem.ChildNodes(0).InnerText & "," & ClassItem.ChildNodes(1).InnerText)
                    End If
                Next
                sw.Flush()
                sw.Close()
                MessageBox.Show("Export of WH Location Classes complete!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End With
    End Sub

    Private Sub btnGenerateWHAttribs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerateWHAttribs.Click
        With ofd1
            .Title = "Select dgmTypeAttributes XML file..."
            .FileName = ""
            .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            .Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim WHXML As New XmlDocument
                WHXML.Load(.FileName)
                MessageBox.Show("Starting WH Attribute parsing...")
                Dim ClassList As XmlNodeList = WHXML.SelectNodes("data/dgmtypeattribs")
                Dim sw As New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\EveHQ\WHattribs.txt")
                For Each ClassItem As XmlNode In ClassList
                    If Core.HQ.itemData.ContainsKey(ClassItem.ChildNodes(0).InnerText) = True Then
                        Dim item As Core.EveItem = Core.HQ.itemData(ClassItem.ChildNodes(0).InnerText)
                        If item.Group = 988 Then
                            sw.WriteLine(ClassItem.ChildNodes(0).InnerText & "," & ClassItem.ChildNodes(1).InnerText & "," & CLng(ClassItem.ChildNodes(2).InnerText).ToString)
                        End If
                    End If
                Next
                sw.Flush()
                sw.Close()
                MessageBox.Show("Export of WH Attribs complete!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End With
    End Sub

#End Region

#Region "DB Changes"

    Private Sub btnCompare_Click(sender As Object, e As EventArgs) Handles btnCompare.Click

        Dim ICS As String = "Server=" & Core.HQ.EveHQSettings.DBServer & "; Database = " & txtInitialDB.Text & "; Integrated Security = SSPI;"
        Dim RCS As String = "Server=" & Core.HQ.EveHQSettings.DBServer & "; Database = " & txtRevisedDB.Text & "; Integrated Security = SSPI;"

        Const strSQL As String = "SELECT typeID, typeName FROM invTypes;"

        Dim IDS As DataSet = GetData(ICS, strSQL)
        Dim RDS As DataSet = GetData(RCS, strSQL)

        Dim IL As New SortedList(Of String, String)
        Dim RL As New SortedList(Of String, String)

        If IDS IsNot Nothing And RDS IsNot Nothing Then
            If IDS.Tables(0).Rows.Count > 0 And RDS.Tables(0).Rows.Count > 0 Then
                For Each DR As DataRow In IDS.Tables(0).Rows
                    IL.Add(DR.Item("typeID").ToString, DR.Item("typeName").ToString)
                Next
                For Each DR As DataRow In RDS.Tables(0).Rows
                    RL.Add(DR.Item("typeID").ToString, DR.Item("typeName").ToString)
                Next
            End If
        End If

        Dim Changes As New SortedList(Of String, String)

        For Each ID As String In IL.Keys
            If RL.ContainsKey(ID) = True Then
                If IL(ID) <> RL(ID) Then
                    Changes.Add(ID, IL(ID) & ControlChars.Tab & RL(ID))
                End If
            End If
        Next

        Dim str As New StringBuilder
        str.AppendLine("Type ID" & ControlChars.Tab & "CategoryID" & ControlChars.Tab & "GroupID" & ControlChars.Tab & "Old Name" & ControlChars.Tab & "New Name")
        For Each Item As String In Changes.Keys
            str.Append(Item & ControlChars.Tab)
            str.Append(EveHQ.Core.HQ.itemData(Item).Category & ControlChars.Tab)
            str.Append(EveHQ.Core.HQ.itemData(Item).Group & ControlChars.Tab)
            str.AppendLine(Changes(Item))
        Next
        Clipboard.SetText(str.ToString)

        MessageBox.Show("Comparison Complete - data posted to clipboard")

    End Sub

    Private Function GetData(ByVal CS As String, ByVal strSQL As String) As DataSet
        Dim EveHQData As New DataSet
        Dim conn As New SqlConnection
        conn.ConnectionString = CS
        Try
            conn.Open()
            Dim da As New SqlDataAdapter(strSQL, conn)
            da.SelectCommand.CommandTimeout = Core.HQ.EveHQSettings.DBTimeout
            da.Fill(EveHQData, "EveHQData")
            conn.Close()
            Return EveHQData
        Catch e As Exception
            Core.HQ.dataError = e.Message
            Return Nothing
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Function


#End Region


End Class
