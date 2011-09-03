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
Imports System.IO
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO.Compression
Imports System.Text
Imports System.Xml
Imports System.Runtime.Serialization.Formatters.Binary
Imports ICSharpCode.SharpZipLib.Core
Imports ICSharpCode.SharpZipLib.Zip
Imports System.Data.SqlServerCe

Public Class frmDataConvert

    Dim DBVersion As String = "2.0.1.1"

#Region "SQLCE Version Table Routines"

    Private Sub btnBrowseDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDB.Click
        With ofd1
            .Title = "Select the source SQLCE database to add the version table..."
            .FileName = ""
            .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            .Filter = "SQL CE files (*.sdf)|*.sdf|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Call Me.AddSQLCEVersionTable(ofd1.FileName)
            End If
        End With
    End Sub

    Private Sub AddSQLCEVersionTable(ByVal SourceDB As String)
        Dim strConn As String = "Data Source = '" & SourceDB & "';" & "Max Database Size = 512; Max Buffer Size = 2048;"
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

    Private Sub btnGenerateWHClassLocations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateWHClassLocations.Click
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

    Private Sub btnGenerateWHAttribs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateWHAttribs.Click
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
                    If EveHQ.Core.HQ.itemData.ContainsKey(ClassItem.ChildNodes(0).InnerText) = True Then
                        Dim item As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(ClassItem.ChildNodes(0).InnerText)
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


  
End Class
