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

Public Class frmSQLUpgrade

    Private Sub btnConvert_Click(sender As System.Object, e As System.EventArgs) Handles btnConvert.Click

        ' Open a file dialog to search for the EveHQ file
        Dim SQLCEFile As String = ""
        Dim ofd1 As New OpenFileDialog
        With ofd1
            .Title = "Select EveHQ SQLCE Data file"
            .FileName = ""
            .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            .Filter = "EveHQ SQLCE Data files (EveHQData.sdf)|EveHQData.sdf|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                SQLCEFile = .FileName
            End If
        End With

        If SQLCEFile = "" Then
            MessageBox.Show("Upgrade aborted by the user.", "Upgrade Aborted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        Else

            ' Test the file exists
            If My.Computer.FileSystem.FileExists(SQLCEFile) = False Then
                MessageBox.Show("The file '" & SQLCEFile & "' does not exist - upgrade cannot continue.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Copy the file - just in case
            Try
                My.Computer.FileSystem.CopyFile(SQLCEFile, Path.Combine(Path.GetDirectoryName(SQLCEFile), "EveHQDataV35.sdf"))
                MessageBox.Show("Backup of Database file completed. Click 'OK' to proceed with the conversion...", "Backup Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                Dim reply As DialogResult = MessageBox.Show("Backup of Database file failed. Would you still like to continue?", "Backup Failed", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End Try

            ' Try and upgrade the database
            Try
                Dim strConnection As String = "Data Source = '" & SQLCEFile & "';" & "Max Database Size = 512; Max Buffer Size = 2048;"
                Dim SQLCEE As New SqlServerCe.SqlCeEngine(strConnection)
                SQLCEE.Upgrade()
                MessageBox.Show("The upgrade of EveHQData.sdf from v3.5 to v4.0 is complete!", "Upgrade Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("There was an error during hte upgrade process. The error was: " & ex.Message, "Database Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If

    End Sub
End Class
