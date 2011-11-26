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
Imports System.Net
Imports System.Drawing
Imports System.IO

Public Class ImageHandler

    Public Shared Function GetRawImageLocation(ByVal TypeID As String) As String

        Dim remoteURL As String = "http://image.eveonline.com/Type/" & TypeID & "_64.png"
        ' Return the image location
        Return remoteURL

    End Function

    Public Shared Function GetImageLocation(ByVal TypeID As String) As String
        ' Check EveHQ image cache folder for the image
        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, TypeID & ".png")) = True Then
            ' Return the cached image location
            Return Path.Combine(EveHQ.Core.HQ.imageCacheFolder, TypeID & ".png")
        Else
            ' Calculate the remote URL
            Dim remoteURL As String = "http://image.eveonline.com/Type/" & TypeID & "_64.png"
            ' Download the image
            Threading.ThreadPool.QueueUserWorkItem(AddressOf DownloadImage, New Object() {remoteURL, TypeID})
            ' Return the image location
            Return remoteURL
        End If

    End Function

    Public Shared Function GetImage(ByVal TypeID As String, Optional ByVal ImageSize As Integer = 64) As Image
        Try
            ' Check EveHQ image cache folder for the image
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, TypeID & ".png")) = True Then
                ' Return the cached image location
                Return CType(New Bitmap(Image.FromFile(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, TypeID & ".png")), ImageSize, ImageSize), Image)
            Else
                ' Calculate the remote URL
                Dim remoteURL As String = "http://image.eveonline.com/Type/" & TypeID & "_64.png"
                ' Download the image
                Dim dlImage As Image = DownloadImage(New Object() {remoteURL, TypeID})
                If dlImage IsNot Nothing Then
                    Return CType(New Bitmap(dlImage, ImageSize, ImageSize), Image)
                Else
                    Return Nothing
                End If
            End If
        Catch e As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetPortraitImage(ByVal PilotID As String) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim Filename As String = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, PilotID & ".png")
            If My.Computer.FileSystem.FileExists(Filename) = False Then
                Call DownloadPortrait(PilotID.ToString)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(Filename) = True Then
                Dim fs As New FileStream(Filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, Imaging.ImageFormat.Png)
                Dim imgClone As Image = Image.FromStream(ms)
                img.Dispose()
                fs.Close()
                fs.Dispose()
                ms.Dispose()
                Return imgClone
            Else
                Return My.Resources.nochar
            End If
        Catch e As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetPortraitImage(ByVal PilotID As String, ImageSize As Integer) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim Filename As String = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, PilotID & ".png")
            If My.Computer.FileSystem.FileExists(Filename) = False Then
                Call DownloadPortrait(PilotID.ToString)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(Filename) = True Then
                Dim fs As New FileStream(Filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, Imaging.ImageFormat.Png)
                Dim imgClone As Image = Image.FromStream(ms)
                img.Dispose()
                fs.Close()
                fs.Dispose()
                ms.Dispose()
                Return CType(New Bitmap(imgClone, ImageSize, ImageSize), Image)
            Else
                Return CType(New Bitmap(My.Resources.nochar, ImageSize, ImageSize), Image)
            End If
        Catch e As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetCorpImage(CorpID As String) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim Filename As String = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, CorpID & ".png")
            If My.Computer.FileSystem.FileExists(Filename) = False Then
                Call DownloadCorpImage(CorpID.ToString)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(Filename) = True Then
                Dim fs As New FileStream(Filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, Imaging.ImageFormat.Png)
                Dim imgClone As Image = Image.FromStream(ms)
                img.Dispose()
                fs.Close()
                fs.Dispose()
                ms.Dispose()
                Return imgClone
            Else
                Return My.Resources.nochar
            End If
        Catch e As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetCorpImage(CorpID As String, ImageSize As Integer) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim Filename As String = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, CorpID & ".png")
            If My.Computer.FileSystem.FileExists(Filename) = False Then
                Call DownloadCorpImage(CorpID.ToString)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(Filename) = True Then
                Dim fs As New FileStream(Filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, Imaging.ImageFormat.Png)
                Dim imgClone As Image = Image.FromStream(ms)
                img.Dispose()
                fs.Close()
                fs.Dispose()
                ms.Dispose()
                Return CType(New Bitmap(imgClone, ImageSize, ImageSize), Image)
            Else
                Return CType(New Bitmap(My.Resources.nochar, ImageSize, ImageSize), Image)
            End If
        Catch e As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetAllianceImage(AllianceID As String) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim Filename As String = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, AllianceID & ".png")
            If My.Computer.FileSystem.FileExists(Filename) = False Then
                Call DownloadAllianceImage(AllianceID)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(Filename) = True Then
                Dim fs As New FileStream(Filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, Imaging.ImageFormat.Png)
                Dim imgClone As Image = Image.FromStream(ms)
                img.Dispose()
                fs.Close()
                fs.Dispose()
                ms.Dispose()
                Return imgClone
            Else
                Return My.Resources.nochar
            End If
        Catch e As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function DownloadImage(ByVal DownloadInfoObject As Object) As Image
        Try
            Dim DownloadInfo() As Object = CType(DownloadInfoObject, Object())
            Dim remoteURL As String = CStr(DownloadInfo(0))
            Dim TypeID As String = CStr(DownloadInfo(1))
            Dim RequestPic As WebRequest = WebRequest.Create(remoteURL)
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.RemoteProxy IsNot Nothing Then
                If EveHQ.Core.HQ.RemoteProxy.ProxyRequired = True Then
                    RequestPic.Proxy = EveHQ.Core.HQ.RemoteProxy.SetupWebProxy
                End If
            End If
            Dim ResponsePic As WebResponse = RequestPic.GetResponse
            Dim WebImage As Image = Image.FromStream(ResponsePic.GetResponseStream())
            ' Save the image into the cache
            WebImage.Save(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, TypeID & ".png"), System.Drawing.Imaging.ImageFormat.Png)
            Return WebImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function

    Public Shared Function DownloadPortrait(ByVal CharacterID As String) As Image
        Dim PortraitSize As Integer = 256
        Try
            Dim RequestPic As WebRequest = WebRequest.Create("https://image.eveonline.com/Character/" & CharacterID & "_256.jpg")
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.RemoteProxy IsNot Nothing Then
                If EveHQ.Core.HQ.RemoteProxy.ProxyRequired = True Then
                    RequestPic.Proxy = EveHQ.Core.HQ.RemoteProxy.SetupWebProxy
                End If
            End If
            Dim ResponsePic As WebResponse = RequestPic.GetResponse
            Dim WebImage As Image = Image.FromStream(ResponsePic.GetResponseStream())
            ' Save the image into the cache
            WebImage.Save(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, CharacterID & ".png"), System.Drawing.Imaging.ImageFormat.Png)
            Return WebImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function

    Public Shared Function DownloadCorpImage(ByVal CorporationID As String) As Image
        Dim PortraitSize As Integer = 256
        Try
            Dim RequestPic As WebRequest = WebRequest.Create("https://image.eveonline.com/Corporation/" & CorporationID & "_256.png")
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.RemoteProxy IsNot Nothing Then
                If EveHQ.Core.HQ.RemoteProxy.ProxyRequired = True Then
                    RequestPic.Proxy = EveHQ.Core.HQ.RemoteProxy.SetupWebProxy
                End If
            End If
            Dim ResponsePic As WebResponse = RequestPic.GetResponse
            Dim WebImage As Image = Image.FromStream(ResponsePic.GetResponseStream())
            ' Save the image into the cache
            WebImage.Save(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, CorporationID & ".png"), System.Drawing.Imaging.ImageFormat.Png)
            Return WebImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function

    Public Shared Function DownloadAllianceImage(ByVal AllianceID As String) As Image
        Dim PortraitSize As Integer = 256
        Try
            Dim RequestPic As WebRequest = WebRequest.Create("https://image.eveonline.com/Alliance/" & AllianceID & "_256.png")
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.RemoteProxy IsNot Nothing Then
                If EveHQ.Core.HQ.RemoteProxy.ProxyRequired = True Then
                    RequestPic.Proxy = EveHQ.Core.HQ.RemoteProxy.SetupWebProxy
                End If
            End If
            Dim ResponsePic As WebResponse = RequestPic.GetResponse
            Dim WebImage As Image = Image.FromStream(ResponsePic.GetResponseStream())
            ' Save the image into the cache
            WebImage.Save(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, AllianceID & ".png"), System.Drawing.Imaging.ImageFormat.Png)
            Return WebImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function



End Class

