' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2009  Lee Vessey
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
Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports ICSharpCode.SharpZipLib.Zip
Imports Microsoft.Win32
Imports System.Text
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Security.AccessControl
Imports System.Security.Cryptography

Public Class frmPatcher

    Dim isLocal As Boolean = False
    Dim DBFileName As String = ""
    Dim EveHQFolder As String = ""
    Dim BaseLocation As String = ""
    Dim updateFolder As String = ""

    Private Sub tmrDownload_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDownload.Tick
        tmrDownload.Enabled = False
        lblCurrentStatus.Text = "Waiting for EveHQ Shutdown..."
        Me.Refresh()
        Me.Refresh()
        lblCurrentStatus.Text = "Confirming EveHQ Shutdown..."
        Me.Refresh()
        Call KillEveHQ()
        Call UpdateEveHQ()
        If isLocal = False Then
            Dim msg As String = "The EveHQ update is complete. Would you like to start EveHQ now?"
            If MessageBox.Show(msg, "Start EveHQ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Call StartEveHQ()
            End If
        Else
            Dim msg As String = "The EveHQ update is complete. Due to using the /local switch, please restart EveHQ manually."
            MessageBox.Show(msg, "Manual Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        End
    End Sub

    Private Function KillEveHQ() As Boolean

        Try
            For Each proc As Process In System.Diagnostics.Process.GetProcessesByName("EveHQ")
                proc.Kill()
                proc.WaitForExit()
            Next
        Catch ex As Exception
            ' Process ended...move on!
        End Try
        'MessageBox.Show("All EveHQ Processes have been closed", "Confirmed Shutdown", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return True

    End Function

    Private Function UpdateEveHQ() As Boolean

        lblCurrentStatus.Text = "Updating Files..."
        lblCurrentStatus.Refresh()

        updateFolder = Path.Combine(BaseLocation, "Updates")

        Dim oldFile As String = ""
        For Each updateFile As String In My.Computer.FileSystem.GetFiles(updateFolder, FileIO.SearchOption.SearchTopLevelOnly)
            Try
                Dim ufi As New IO.FileInfo(updateFile)
                lblCurrentStatus.Text = "Updating File..." & ufi.Name
                lblCurrentStatus.Refresh()
                Dim newFile As String = Path.Combine(EveHQFolder, ufi.Name)
                Dim nfi As New IO.FileInfo(newFile)
                ' Copy the new file as the old one
                My.Computer.FileSystem.CopyFile(ufi.FullName, nfi.FullName, True)
            Catch e As Exception
                Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
                errMsg &= "Message: " & e.Message & ControlChars.CrLf
                MessageBox.Show(errMsg, "Error Updating EveHQ File", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End
            End Try
        Next

        ' Check for a database upgrade
        lblCurrentStatus.Text = "Checking for database upgrade..."
        Me.Refresh()
        ' See if we have the EveHQ.mdb.zip file
        Dim DBZipLocation As String = Path.Combine(updateFolder, "EveHQ.mdb.zip")
        If My.Computer.FileSystem.FileExists(DBZipLocation) = True Then
            ' We have the file, let's try extracting it
            lblCurrentStatus.Text = "Extracting new database..."
            Me.Refresh()
            Try
                Dim unzip As FastZip = New FastZip()
                unzip.ExtractZip(DBZipLocation, updateFolder, "")
                ' See if we have the EveHQ.mdb file
                Dim DBLocation As String = Path.Combine(updateFolder, "EveHQ.mdb")
                If My.Computer.FileSystem.FileExists(DBLocation) = True Then
                    ' Copy to existing DB Location
                    lblCurrentStatus.Text = "Copying new database..."
                    Me.Refresh()
                    My.Computer.FileSystem.CopyFile(DBLocation, DBFileName, True)
                End If
            Catch ex As Exception
                ' Failed extraction
                Exit Function
            End Try
        End If

        ' Delete All Upgrades
        Try
            My.Computer.FileSystem.DeleteDirectory(updateFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Catch ex As Exception
            ' Failed delete - meh, cleanup can occur in the main app if need be
        End Try

    End Function

    Private Function StartEveHQ() As Boolean
        Try
            Dim startInfo As ProcessStartInfo = New ProcessStartInfo()
            startInfo.UseShellExecute = False
            startInfo.WorkingDirectory = EveHQFolder
            startInfo.FileName = Path.Combine(EveHQFolder, "EveHQ.exe")
            If isLocal = True Then
                startInfo.Arguments = " /local"
            End If
            Process.Start(startInfo)
            Return True
        Catch e As Exception
            Return False
        End Try
    End Function

    Private Sub frmPatcher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Check for any commandline parameters that we need to account for
        For Each param As String In System.Environment.GetCommandLineArgs
            If param = "/wait" Then
                Threading.Thread.Sleep(2000)
            End If
            If param.StartsWith("/App") Then
                Dim paramData As String() = param.Split(CChar(";"))
                EveHQFolder = paramData(1).TrimStart(CChar(ControlChars.Quote)).TrimEnd(CChar(ControlChars.Quote))
                lblEveHQLocation.Text = "EveHQ Location: " & EveHQFolder
            End If
            If param.StartsWith("/Base") Then
                Dim paramData As String() = param.Split(CChar(";"))
                BaseLocation = paramData(1).TrimStart(CChar(ControlChars.Quote)).TrimEnd(CChar(ControlChars.Quote))
            End If
            If param.StartsWith("/Local") Then
                Dim paramData As String() = param.Split(CChar(";"))
                If paramData(1) = "True" Then
                    isLocal = True
                Else
                    isLocal = False
                End If
                lblLocalFolders.Text = "Using Local Folders?  " & paramData(1)
            End If
            If param.StartsWith("/DB") Then
                Dim paramData As String() = param.Split(CChar(";"))
                If paramData(1) <> "None" Then
                    DBFileName = paramData(1).TrimStart(CChar(ControlChars.Quote)).TrimEnd(CChar(ControlChars.Quote))
                End If
                lblDatabaseLocation.Text = "Database Location: " & paramData(1)
            End If
        Next
    End Sub

End Class
