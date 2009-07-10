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

Public Class frmPatcher

    Dim isLocal As Boolean = False
    Dim DBFileName As String = ""
    Dim EveHQFolder As String = ""

    Private Sub tmrDownload_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDownload.Tick
        lblCurrentStatus.Text = "Waiting for EveHQ Shutdown..."
        Me.Refresh()
        tmrDownload.Enabled = False
        Me.Refresh()
        lblCurrentStatus.Text = "Confirming EveHQ Shutdown..."
        Me.Refresh()
        Call KillEveHQ()
        Call UpdateEveHQ()
        Call StartEveHQ()
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
        Dim updateFolder As String = ""

        If isLocal = False Then
            updateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveHQ")
            updateFolder = Path.Combine(updateFolder, "Updates")
        Else
            updateFolder = Path.Combine(EveHQFolder, "Updates")
        End If

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

    End Function

    Private Function StartEveHQ() As Boolean
        Try
            Process.Start(Path.Combine(My.Application.Info.DirectoryPath, "EveHQ.exe"))
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
                EveHQFolder = paramData(1)
                lblEveHQLocation.Text = "EveHQ Location: " & EveHQFolder
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
                    DBFileName = paramData(1)
                End If
                lblDatabaseLocation.Text = "Database Location: " & paramData(1)
            End If
        Next
    End Sub
End Class
