' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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

    Private Sub tmrDownload_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDownload.Tick
        tmrDownload.Enabled = False
        Me.Refresh()
        Call KillEveHQ()
        Call UpdateEveHQ()
        Call StartEveHQ()
        End
    End Sub

    Private Function KillEveHQ() As Boolean

        lblStatus.Text = "Stopping EveHQ Process..."
        lblStatus.Refresh()
        Dim local As Process() = Process.GetProcesses
        Dim processFound As Boolean = False
        Dim i As Integer
        For i = 0 To local.Length - 1
            If Strings.UCase(local(i).ProcessName) = Strings.UCase("EveHQ") Then
                local(i).Kill()
                MessageBox.Show(local(i).ProcessName)
                processFound = True
            End If
        Next
        ' Cycle until all the EveHQ processes are gone
        Dim allClosed As Boolean = True
        Do
            allClosed = True
            Dim pros As Process() = Process.GetProcesses
            For i = 0 To pros.Length - 1
                If Strings.UCase(local(i).ProcessName) = Strings.UCase("EveHQ") Then
                    allClosed = False
                End If
            Next
        Loop Until allClosed = True
        MessageBox.Show("All Closed")
        Return True
    End Function

    'Private Function DownloadFiles() As Boolean

    '    ' Set a default policy level for the "http:" and "https" schemes.
    '    Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.RequestCacheLevel.NoCacheNoStore)

    '    Dim count As Integer = 0
    '    For Each fileNeeded As String In fileList
    '        count += 1
    '        Dim httpURI As String = "http://www.evehq.net/downloads/" & fileNeeded
    '        Dim localFile As String = My.Application.Info.DirectoryPath & "\" & fileNeeded
    '        lblDownload.Text = count & " of " & fileList.Count & " - " & httpURI

    '        ' Create the request to access the server and set credentials
    '        Dim request As HttpWebRequest = WebRequest.Create(httpURI)
    '        request.CachePolicy = policy
    '        request.Method = WebRequestMethods.File.DownloadFile
    '        Try

    '            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
    '                Dim filesize As Long = CLng(response.ContentLength)
    '                lblTotalBytes.Text = filesize
    '                'Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
    '                Using responseStream As IO.Stream = response.GetResponseStream
    '                    'loop to read & write to file
    '                    Using fs As New IO.FileStream(localFile, IO.FileMode.Create)
    '                        Dim buffer(2047) As Byte
    '                        Dim read As Integer = 0
    '                        Dim totalBytes As Long = 0
    '                        Dim percent As Integer = 0
    '                        Do
    '                            read = responseStream.Read(buffer, 0, buffer.Length)
    '                            fs.Write(buffer, 0, read)
    '                            totalBytes += read
    '                            percent = totalBytes / filesize * 100
    '                            lblBytes.Text = totalBytes & " (" & percent & "%)"
    '                            pb1.Value = percent
    '                            Application.DoEvents()
    '                        Loop Until read = 0 'see Note(1)
    '                        responseStream.Close()
    '                        fs.Flush()
    '                        fs.Close()
    '                    End Using
    '                    responseStream.Close()
    '                End Using
    '                response.Close()
    '            End Using
    '        Catch e As WebException
    '            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
    '            errMsg &= "Status: " & e.Status & ControlChars.CrLf
    '            errMsg &= "Message: " & e.Message & ControlChars.CrLf
    '            MessageBox.Show(errMsg, "Error Uploading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Return False
    '        End Try
    '    Next
    '    Return True

    'End Function

    Private Function UpdateEveHQ() As Boolean

        lblStatus.Text = "Updating Files..."
        lblStatus.Refresh()
        Try
            Dim oldFile As String = ""
            For Each newFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.upd")
                oldFile = newFile.TrimEnd(".upd".ToCharArray)
                ' Check if old file exists and back it up!
                Dim ofi As New IO.FileInfo(oldFile)
                If My.Computer.FileSystem.FileExists(ofi.FullName) Then
                    ' Delete the old backup if it exists
                    If My.Computer.FileSystem.FileExists(ofi.FullName & ".bak") = True Then
                        My.Computer.FileSystem.DeleteFile(ofi.FullName & ".bak")
                    End If
                    My.Computer.FileSystem.RenameFile(ofi.FullName, ofi.Name & ".bak")
                End If
                Dim nfi As New IO.FileInfo(newFile)
                ' Copy the new file as the old one
                My.Computer.FileSystem.CopyFile(nfi.FullName, ofi.FullName, True)
            Next
        Catch e As Exception
            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
            errMsg &= "Message: " & e.Message & ControlChars.CrLf
            MessageBox.Show(errMsg, "Error Updating EveHQ Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End
        End Try
    End Function

    'Private Function UpdateEveHQ() As Boolean
    '    Try
    '        My.Computer.FileSystem.CopyFile(My.Application.Info.DirectoryPath & "\EveHQ.upd", My.Application.Info.DirectoryPath & "\EveHQ.exe", True)
    '        My.Computer.FileSystem.DeleteFile(My.Application.Info.DirectoryPath & "\EveHQ.upd")
    '        Return True
    '    Catch e As Exception
    '        Return False
    '    End Try
    'End Function

    Private Function StartEveHQ() As Boolean
        Try
            Process.Start(My.Application.Info.DirectoryPath & "\EveHQ.exe")
            Return True
        Catch e As Exception
            Return False
        End Try
    End Function

    'Private Function CompareVersions(ByVal localVer As String, ByVal remoteVer As String) As Boolean
    '    Dim localVers() As String = localVer.Split(".")
    '    Dim remoteVers() As String = remoteVer.Split(".")
    '    Dim requiresUpdate As Boolean = False
    '    For ver As Integer = 0 To 3
    '        If CInt(remoteVers(ver)) <> CInt(localVers(ver)) Then
    '            If CInt(remoteVers(ver)) > CInt(localVers(ver)) Then
    '                requiresUpdate = True
    '                Exit For
    '            Else
    '                requiresUpdate = False
    '                Exit For
    '            End If
    '        End If
    '    Next
    '    Return requiresUpdate
    'End Function
End Class
