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
Imports System.Threading
Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports ICSharpCode.SharpZipLib.Zip
Imports SearchOption = Microsoft.VisualBasic.FileIO.SearchOption

Public Class FrmPatcher

    Dim _isLocal As Boolean = False
    Dim _dbFileName As String = ""
    Dim _eveHQFolder As String = ""
    Dim _baseLocation As String = ""
    Dim _updateFolder As String = ""

    Private Sub tmrDownload_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrDownload.Tick
        tmrDownload.Enabled = False
        lblCurrentStatus.Text = "Waiting for EveHQ Shutdown..."
        Refresh()
        lblCurrentStatus.Text = "Confirming EveHQ Shutdown..."
        Refresh()
        If KillEveHQ() = True Then
            Call UpdateEveHQ()
            If _isLocal = False Then
                Const msg As String = "The EveHQ update is complete. Would you like to start EveHQ now?"
                If MessageBox.Show(msg, "Start EveHQ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Call StartEveHQ()
                End If
            Else
                Const msg As String = "The EveHQ update is complete. Due to using the /local switch, please restart EveHQ manually."
                MessageBox.Show(msg, "Manual Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
        End
    End Sub

    Private Function KillEveHQ() As Boolean

        Try
            For Each proc As Process In Process.GetProcessesByName("EveHQ")
                'proc.CloseMainWindow()
                'proc.WaitForExit()
                Const maxAttempts As Integer = 10
                Dim attempts As Integer = 0
                proc.CloseMainWindow()
                Do
                    attempts += 1
                    lblCurrentStatus.Text = "Checking for EveHQ Shutdown...Attempt " & attempts.ToString & " of " & maxAttempts.ToString
                    Refresh()
                    Application.DoEvents()
                    Thread.Sleep(2000)
                Loop Until proc.HasExited = True Or attempts >= maxAttempts
                If proc.HasExited = False Then
                    lblCurrentStatus.Text = "Killing EveHQ..?" & attempts.ToString
                    Refresh()
                    Dim reply As DialogResult = MessageBox.Show("EveHQ has failed to exit in a timely manner. Would you like to force EveHQ closed to continue the update?", "Confirm Kill Process", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If reply = DialogResult.Yes Then
                        proc.Kill()
                        proc.WaitForExit()
                    Else
                        Return False
                    End If
                End If
            Next
        Catch ex As Exception
            ' Process ended...move on!
        End Try
        'MessageBox.Show("All EveHQ Processes have been closed", "Confirmed Shutdown", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return True

    End Function

    Private Sub UpdateEveHQ()

        lblCurrentStatus.Text = "Updating Files..."
        lblCurrentStatus.Refresh()

        _updateFolder = Path.Combine(_baseLocation, "Updates")

        For Each updateFile As String In My.Computer.FileSystem.GetFiles(_updateFolder, SearchOption.SearchTopLevelOnly)
            Try
                Dim ufi As New FileInfo(updateFile)
                lblCurrentStatus.Text = "Updating File..." & ufi.Name
                lblCurrentStatus.Refresh()
                Dim newFile As String = Path.Combine(_eveHQFolder, ufi.Name)
                Dim nfi As New FileInfo(newFile)
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
        Refresh()
        ' See if we have the EveHQ.sdf.zip file
        Dim dbZipLocation As String = Path.Combine(_updateFolder, "EveHQ.sdf.zip")
        If My.Computer.FileSystem.FileExists(dbZipLocation) = True Then
            ' We have the file, let's try extracting it
            lblCurrentStatus.Text = "Extracting new database..."
            Refresh()
            Try
                Dim unzip As FastZip = New FastZip()
                unzip.ExtractZip(dbZipLocation, _updateFolder, "")
                ' See if we have the EveHQ.sdf file
                Dim dbLocation As String = Path.Combine(_updateFolder, "EveHQ.sdf")
                If My.Computer.FileSystem.FileExists(dbLocation) = True Then
                    ' Copy to existing DB Location
                    lblCurrentStatus.Text = "Copying new database..."
                    Refresh()
                    My.Computer.FileSystem.CopyFile(dbLocation, _dbFileName, True)
                End If
            Catch ex As Exception
                ' Failed extraction
                Return
            End Try
        End If

        lblCurrentStatus.Text = "Clearing cache files..."
        ' Clear any known cache files/folders ... except for image cache
        Dim dataFolder As String
        If _isLocal = True Then
            dataFolder = _eveHQFolder
        Else
            dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "evehq")
        End If

        Dim cacheFolder As String = Path.Combine(dataFolder, "cache")
        Dim coreCacheFolder As String = Path.Combine(dataFolder, "coreCache")
        Dim hqfFolder As String = Path.Combine(dataFolder, "hqf")
        Dim hqfCacheFolder As String = Path.Combine(hqfFolder, "cache")
        Dim marketCache As String = Path.Combine(dataFolder, "marketCache")
        Try

            If Directory.Exists(cacheFolder) = True Then
                For Each file As String In Directory.GetFiles(cacheFolder)
                    IO.File.Delete(file)
                Next
            End If

            If Directory.Exists(coreCacheFolder) = True Then
                For Each file As String In Directory.GetFiles(coreCacheFolder)
                    IO.File.Delete(file)
                Next
            End If

            If Directory.Exists(hqfFolder) = True And Directory.Exists(hqfCacheFolder) = True Then
                For Each file As String In Directory.GetFiles(hqfCacheFolder)
                    IO.File.Delete(file)
                Next
            End If

            If Directory.Exists(marketCache) = True Then
                For Each file As String In Directory.GetFiles(marketCache)
                    IO.File.Delete(file)
                Next
            End If

        Catch ex As Exception
            ' proposely supressing the errors.
        End Try

        ' Delete All Upgrades
        Try
            My.Computer.FileSystem.DeleteDirectory(_updateFolder, DeleteDirectoryOption.DeleteAllContents)
        Catch ex As Exception
            ' Failed delete - meh, cleanup can occur in the main app if need be
        End Try

        Return
    End Sub

    Private Sub StartEveHQ()
        Try
            Dim startInfo As ProcessStartInfo = New ProcessStartInfo()
            startInfo.UseShellExecute = False
            startInfo.WorkingDirectory = _eveHQFolder
            startInfo.FileName = Path.Combine(_eveHQFolder, "EveHQ.exe")
            If _isLocal = True Then
                startInfo.Arguments = " /local"
            End If
            Process.Start(startInfo)
            Return
        Catch e As Exception
            Return
        End Try
    End Sub

    Private Sub frmPatcher_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Check for any commandline parameters that we need to account for
        For Each param As String In Environment.GetCommandLineArgs
            If param = "/wait" Then
                Thread.Sleep(2000)
            End If
            If param.StartsWith("/App") Then
                Dim paramData As String() = param.Split(CChar(";"))
                _eveHQFolder = paramData(1).TrimStart(CChar(ControlChars.Quote)).TrimEnd(CChar(ControlChars.Quote))
                lblEveHQLocation.Text = "EveHQ Location: " & _eveHQFolder
            End If
            If param.StartsWith("/Base") Then
                Dim paramData As String() = param.Split(CChar(";"))
                _baseLocation = paramData(1).TrimStart(CChar(ControlChars.Quote)).TrimEnd(CChar(ControlChars.Quote))
                lblUpdateLocation.Text = _baseLocation
            End If
            If param.StartsWith("/Local") Then
                Dim paramData As String() = param.Split(CChar(";"))
                If paramData(1) = "True" Then
                    _isLocal = True
                Else
                    _isLocal = False
                End If
                lblLocalFolders.Text = "Using Local Folders?  " & paramData(1)
            End If
            If param.StartsWith("/DB") Then
                Dim paramData As String() = param.Split(CChar(";"))
                If paramData(1) <> "None" Then
                    _dbFileName = paramData(1).TrimStart(CChar(ControlChars.Quote)).TrimEnd(CChar(ControlChars.Quote))
                End If
                lblDatabaseLocation.Text = "Database Location: " & paramData(1)
            End If
        Next
    End Sub

End Class
