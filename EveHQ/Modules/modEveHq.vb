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
Imports System
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Xml
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Data

Module modEveHq
    Public eveData As New DataSet

    Public Sub CheckForUpdates(ByVal showNoUpdate As Boolean)

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        ' Create the request to access the server and set credentials
        Dim URL As String = "http://www.evehq.net/downloads/_latest.txt"
        Dim localfile As String = EveHQ.Core.HQ.appDataFolder & "\_latest.txt"
        Dim request As HttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)
        request.CachePolicy = policy
        request.Method = WebRequestMethods.File.DownloadFile
        Try
            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
                Dim filesize As Long = CLng(response.ContentLength)
                'Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(localfile, IO.FileMode.Create)
                        Dim buffer(2047) As Byte
                        Dim read As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
            Dim strData As String = ""
            Dim sr As StreamReader = New StreamReader(localfile)
            strData = sr.ReadToEnd
            sr.Close()

            Dim updatesRequired As New ArrayList
            Dim filesRequired As New ArrayList
            Dim files() As String = strData.Split(ControlChars.Cr)
            Dim file As String = ""
            For Each file In files
                file = file.Trim(ControlChars.Lf)
                If file.Trim <> "" Then
                    Dim fileDetails() As String = file.Split(CChar(","))
                    Dim fileName As String = fileDetails(0)
                    Dim fileVersion As String = fileDetails(1)

                    ' Check the plugins first
                    For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.PlugIns.Values
                        If PlugInInfo.Available = True Then
                            If PlugInInfo.ShortFileName = fileName Then
                                If CompareVersions(PlugInInfo.Version, fileVersion) = True Then
                                    updatesRequired.Add(fileName & " (Current: " & PlugInInfo.Version & ", Available: " & fileVersion & ")")
                                    filesRequired.Add(fileName)
                                End If
                                Exit For
                            End If
                        End If
                    Next
                    ' Check for the main EveHQ App
                    If fileName = "EveHQ.exe" Then
                        If CompareVersions(My.Application.Info.Version.ToString, fileVersion) = True Then
                            updatesRequired.Add(fileName & " (Current: " & My.Application.Info.Version.ToString & ", Available: " & fileVersion & ")")
                            filesRequired.Add(fileName)
                        End If
                    End If
                    ' Check for the EveHQ.Core.dll
                    If fileName = "EveHQ.Core.dll" Then
                        Dim coreVersion As String = ""
                        For Each asm As System.Reflection.AssemblyName In System.Reflection.Assembly.GetExecutingAssembly.GetReferencedAssemblies
                            If asm.Name = "EveHQ.Core" Then
                                coreVersion = asm.Version.ToString
                            End If
                        Next
                        If CompareVersions(coreVersion, fileVersion) = True Then
                            updatesRequired.Add(fileName & " (Current: " & coreVersion & ", Available: " & fileVersion & ")")
                            filesRequired.Add(fileName)
                        End If
                    End If
                End If
            Next

            Dim msg As String = ""
            If updatesRequired.Count > 0 Then
                msg &= "The following updates are available for download:" & ControlChars.CrLf & ControlChars.CrLf
                For Each update As String In updatesRequired
                    msg &= update & ControlChars.CrLf
                Next
                msg &= ControlChars.CrLf & "Would you like to download and install these updates now?"
                If MessageBox.Show(msg, "EveHQ Update Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                Else
                    Dim sw As New StreamWriter(EveHQ.Core.HQ.appDataFolder & "/EveHQ.upd")
                    For Each fileRequired As String In filesRequired
                        sw.WriteLine(fileRequired)
                    Next
                    sw.Flush()
                    sw.Close()
                    Dim startInfo As ProcessStartInfo = New ProcessStartInfo()
                    startInfo.UseShellExecute = True
                    startInfo.WorkingDirectory = Environment.CurrentDirectory
                    startInfo.FileName = EveHQ.Core.HQ.appFolder & "\EveHQPatcher.exe"
                    startInfo.Verb = "runas"
                    Process.Start(startInfo)
                End If
            Else
                If showNoUpdate = False Then
                    msg = "Your EveHQ installation is up-to-date."
                    MessageBox.Show(msg, "EveHQ Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

        Catch e As Exception
            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
            errMsg &= "Message: " & e.Message & ControlChars.CrLf
            MessageBox.Show(errMsg, "Error Accessing Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Function CompareVersions(ByVal localVer As String, ByVal remoteVer As String) As Boolean
        Dim localVers() As String = localVer.Split(CChar("."))
        Dim remoteVers() As String = remoteVer.Split(CChar("."))
        Dim requiresUpdate As Boolean = False
        For ver As Integer = 0 To 3
            If CInt(remoteVers(ver)) <> CInt(localVers(ver)) Then
                If CInt(remoteVers(ver)) > CInt(localVers(ver)) Then
                    requiresUpdate = True
                    Exit For
                Else
                    requiresUpdate = False
                    Exit For
                End If
            End If
        Next
        Return requiresUpdate
    End Function
    
End Module
