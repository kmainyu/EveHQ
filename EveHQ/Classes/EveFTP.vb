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
Imports System.IO
Imports System.Text

Public Class EveFTP

#Region "Class Variables"

    Private m_strHost, m_strMess As String
    Private m_strUser, m_strPass As String
    Private m_intPort, m_intBytes As Integer
    Private m_strLocalPath, m_strRemotePath As String
    Private m_strLocalFile, m_strRemoteFile As String
    Private m_boolLoggedIn As Boolean

#End Region

#Region "Public Properties"

    Public Property RemoteHost() As String
        Get
            Return m_strHost
        End Get
        Set(ByVal value As String)
            m_strHost = value
        End Set
    End Property
    Public Property RemotePath() As String
        Get
            Return m_strRemotePath
        End Get
        Set(ByVal value As String)
            m_strRemotePath = value
        End Set
    End Property
    Public Property LocalFile() As String
        Get
            Return m_strLocalFile
        End Get
        Set(ByVal value As String)
            m_strLocalFile = value
        End Set
    End Property
    Public Property Username() As String
        Get
            Return m_strUser
        End Get
        Set(ByVal value As String)
            m_strUser = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return m_strPass
        End Get
        Set(ByVal value As String)
            m_strPass = value
        End Set
    End Property
    Public Property RemoteFile() As String
        Get
            Return m_strRemoteFile
        End Get
        Set(ByVal value As String)
            m_strRemoteFile = value
        End Set
    End Property
    Public Property Message() As String
        Get
            Return m_strMess
        End Get
        Set(ByVal value As String)
            m_strMess = value
        End Set
    End Property
    Public Property RemotePort() As Integer
        Get
            Return m_intPort
        End Get
        Set(ByVal value As Integer)
            m_intPort = value
        End Set
    End Property

#End Region

#Region "Class Constructors"

    Public Sub New()
        m_strHost = ""
        m_strRemotePath = ""
        m_strRemoteFile = ""
        m_strUser = ""
        m_strPass = ""
        m_strMess = ""
        m_intPort = 21
        m_boolLoggedIn = False
    End Sub

    Public Sub New(ByVal strHost As String, ByVal strPath As String, ByVal strUser As String, ByVal strPass As String, ByVal intPort As Integer)
        m_strHost = strHost
        m_strRemotePath = strPath
        m_strRemoteFile = ""
        m_strUser = strUser
        m_strPass = strPass
        m_strMess = ""
        m_intPort = intPort
        m_boolLoggedIn = False
    End Sub

#End Region

#Region "Public Subs & Functions"

    Public Function DeleteFile(ByVal serverURI As Uri) As Boolean
        If serverURI.Scheme <> Uri.UriSchemeFtp Then
            Return False
            Exit Function
        Else
            Dim request As FtpWebRequest = CType(WebRequest.Create(serverURI), FtpWebRequest)
            'request.Credentials = New NetworkCredential(m_strUser, m_strPass)
            request.Method = WebRequestMethods.Ftp.DeleteFile
            Dim response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
            MessageBox.Show("Delete Status: " & response.StatusDescription)
            response.Close()
            Return True
        End If
    End Function

    Public Function UploadFile(ByVal localFile As String, ByVal serverURI As String, Optional ByVal serverPath As String = "", Optional ByVal serverFile As String = "") As Boolean

        ' Check if local file is valid (and exists) - exit if in error
        If My.Computer.FileSystem.FileExists(localFile) = False Then
            MessageBox.Show(localFile & " is not a valid file or does not exist.", "Error Uploading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If

        ' Build the remote server URI and check compatible with FTP
        Dim lfInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(localFile)
        Dim lfDir As String = lfInfo.Directory.ToString
        Dim lfFile As String = lfInfo.Name
        ' Prepare the serverURI string
        If serverURI.EndsWith("/") = False And serverURI.EndsWith("\") = False Then
            serverURI &= "/"
        End If
        ' Prepare the serverPath string
        If serverPath.StartsWith("/") = True Or serverPath.StartsWith("\") = True Then
            serverPath.Remove(0, 1)
        End If
        If serverPath.EndsWith("/") = False And serverPath.EndsWith("\") = False Then
            serverPath &= "/"
        End If
        ' Check if the server file needs to be called something else
        If serverFile = "" Then serverFile = lfFile
        ' Create the URI
        Dim ftpURI As Uri = New Uri(serverURI & serverPath & serverFile)
        MessageBox.Show(ftpURI.AbsoluteUri & vbCrLf & lfDir & vbCrLf & lfFile)
        If ftpURI.Scheme <> Uri.UriSchemeFtp Then
            MessageBox.Show(serverURI & " is not a valid FTP address.", "Error Uploading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If

        ' Create the request to access the server and set credentials
        Dim request As FtpWebRequest = CType(WebRequest.Create(ftpURI), FtpWebRequest)
        request.Credentials = New NetworkCredential(m_strUser, m_strPass)
        request.UseBinary = True
        request.Method = WebRequestMethods.Ftp.UploadFile

        'create byte array to store: ensure at least 1 byte!
        Const BufferSize As Integer = 2048
        Dim content(BufferSize - 1) As Byte, dataRead As Integer
        'open file for reading 
        Using fs As FileStream = lfInfo.OpenRead
            Try
                'open request to send
                Using rs As Stream = request.GetRequestStream
                    Do
                        dataRead = fs.Read(content, 0, BufferSize)
                        rs.Write(content, 0, dataRead)
                    Loop Until dataRead < BufferSize
                    rs.Close()
                End Using
            Catch e As WebException
                Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
                errMsg &= "Status: " & e.Status & ControlChars.CrLf
                errMsg &= "Message: " & e.Message & ControlChars.CrLf
                MessageBox.Show(errMsg, "Error Uploading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
                Exit Function
            Finally
                'ensure file closed
                fs.Close()
            End Try
        End Using

        ' Confirm upload response from the server
        Dim response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
        MessageBox.Show("Upload Status: " & response.StatusDescription, "Upload Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        response.Close()
        Return True

    End Function

    Public Function DownloadFile(ByVal serverURI As String, ByVal serverFile As String, ByVal localFile As String, Optional ByVal serverPath As String = "") As Boolean

        ' Build the remote server URI and check compatible with FTP
        Dim lfInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(localfile)
        Dim lfDir As String = lfInfo.Directory.ToString
        Dim lfFile As String = lfInfo.Name
        ' Prepare the serverURI string
        If serverURI.EndsWith("/") = False And serverURI.EndsWith("\") = False Then
            serverURI &= "/"
        End If
        ' Prepare the serverPath string
        If serverPath.StartsWith("/") = True Or serverPath.StartsWith("\") = True Then
            serverPath.Remove(0, 1)
        End If
        If serverPath.EndsWith("/") = False And serverPath.EndsWith("\") = False Then
            serverPath &= "/"
        End If
        ' Check if the server file needs to be called something else
        If serverFile = "" Then serverFile = lfFile
        ' Create the URI
        Dim ftpURI As Uri = New Uri(serverURI & serverPath & serverFile)
        MessageBox.Show(ftpURI.AbsoluteUri & vbCrLf & lfDir & vbCrLf & lfFile)
        If ftpURI.Scheme <> Uri.UriSchemeFtp Then
            MessageBox.Show(serverURI & " is not a valid FTP address.", "Error Uploading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End If

        ' Create the request to access the server and set credentials
        Dim request As FtpWebRequest = CType(WebRequest.Create(ftpURI), FtpWebRequest)
        request.Credentials = New NetworkCredential(m_strUser, m_strPass)
        request.UseBinary = True
        request.Method = WebRequestMethods.Ftp.DownloadFile

        ' Attempt to download the file from the server and catch errors
        Try
            Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(localFile, IO.FileMode.Create)
                        Dim buffer(2047) As Byte
                        Dim read As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                        Loop Until read = 0
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
        Catch e As WebException
            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
            errMsg &= "Status: " & e.Status & ControlChars.CrLf
            errMsg &= "Message: " & e.Message & ControlChars.CrLf
            MessageBox.Show(errMsg, "Error Uploading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End Try

        ' Confirm upload response from the server
        Dim finalResponse As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
        MessageBox.Show("Upload Status: " & finalResponse.StatusDescription, "Upload Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        finalResponse.Close()
        Return True

    End Function

#End Region

End Class
