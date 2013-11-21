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
Imports System.Drawing
Imports System.IO
Imports System.Threading
Imports System.Drawing.Imaging
Imports System.Net

Public Class ImageHandler

    Public Shared Function GetRawImageLocation(ByVal typeID As Integer) As String

        Dim remoteURL As String = "http://image.eveonline.com/Type/" & CStr(typeID) & "_64.png"
        ' Return the image location
        Return remoteURL

    End Function

    Public Shared Function GetImageLocation(ByVal typeID As Integer) As String
        ' Check EveHQ image cache folder for the image
        If My.Computer.FileSystem.FileExists(Path.Combine(HQ.imageCacheFolder, typeID & ".png")) = True Then
            ' Return the cached image location
            Return Path.Combine(HQ.imageCacheFolder, CStr(typeID) & ".png")
        Else
            ' Calculate the remote URL
            Dim remoteURL As String = GetRawImageLocation(typeID)
            ' Download the image
            ThreadPool.QueueUserWorkItem(AddressOf DownloadImage, New Object() {remoteURL, typeID})
            ' Return the image location
            Return remoteURL
        End If

    End Function

    Public Shared Function GetImage(ByVal typeID As Integer, Optional ByVal imageSize As Integer = 64, Optional ByVal fileName As String = "") As Image
        Try
            If FileName = "" Then
                fileName = CStr(typeID)
            End If
            ' Check EveHQ image cache folder for the image
            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.imageCacheFolder, FileName & ".png")) = True Then
                ' Return the cached image location
                Return CType(New Bitmap(Image.FromFile(Path.Combine(HQ.imageCacheFolder, FileName & ".png")), imageSize, imageSize), Image)
            Else
                ' Calculate the remote URL
                Dim remoteURL As String = GetRawImageLocation(typeID)
                ' Download the image
                Dim dlImage As Image = DownloadImage(New Object() {remoteURL, FileName})
                If dlImage IsNot Nothing Then
                    Return CType(New Bitmap(dlImage, imageSize, imageSize), Image)
                Else
                    Return Nothing
                End If
            End If
        Catch e As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetPortraitImage(ByVal pilotID As String) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim filename As String = Path.Combine(HQ.imageCacheFolder, pilotID & ".png")
            If My.Computer.FileSystem.FileExists(filename) = False Then
                Call DownloadPortrait(pilotID.ToString)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(filename) = True Then
                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, ImageFormat.Png)
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
            Return My.Resources.nochar
        End Try
    End Function

    Public Shared Function GetPortraitImage(ByVal pilotID As String, imageSize As Integer) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim filename As String = Path.Combine(HQ.imageCacheFolder, pilotID & ".png")
            If My.Computer.FileSystem.FileExists(filename) = False Then
                Call DownloadPortrait(pilotID.ToString)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(filename) = True Then
                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, ImageFormat.Png)
                Dim imgClone As Image = Image.FromStream(ms)
                img.Dispose()
                fs.Close()
                fs.Dispose()
                ms.Dispose()
                Return CType(New Bitmap(imgClone, imageSize, imageSize), Image)
            Else
                Return CType(New Bitmap(My.Resources.nochar, imageSize, imageSize), Image)
            End If
        Catch e As Exception
            Return CType(New Bitmap(My.Resources.nochar, imageSize, imageSize), Image)
        End Try
    End Function

    Public Shared Function GetCorpImage(corpID As String) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim filename As String = Path.Combine(HQ.imageCacheFolder, corpID & ".png")
            If My.Computer.FileSystem.FileExists(filename) = False Then
                Call DownloadCorpImage(corpID.ToString)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(filename) = True Then
                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, ImageFormat.Png)
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
            Return My.Resources.nochar
        End Try
    End Function

    Public Shared Function GetCorpImage(corpID As String, imageSize As Integer) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim filename As String = Path.Combine(HQ.imageCacheFolder, corpID & ".png")
            If My.Computer.FileSystem.FileExists(filename) = False Then
                Call DownloadCorpImage(corpID.ToString)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(filename) = True Then
                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, ImageFormat.Png)
                Dim imgClone As Image = Image.FromStream(ms)
                img.Dispose()
                fs.Close()
                fs.Dispose()
                ms.Dispose()
                Return CType(New Bitmap(imgClone, imageSize, imageSize), Image)
            Else
                Return CType(New Bitmap(My.Resources.nochar, imageSize, imageSize), Image)
            End If
        Catch e As Exception
            Return CType(New Bitmap(My.Resources.nochar, imageSize, imageSize), Image)
        End Try
    End Function

    Public Shared Function GetAllianceImage(allianceID As String) As Image
        ' Requires a special function due to Bitmap.FromFile not releasing the file handle on a timely basis
        Try
            ' Check for the file first and download it
            Dim filename As String = Path.Combine(HQ.imageCacheFolder, allianceID & ".png")
            If My.Computer.FileSystem.FileExists(filename) = False Then
                Call DownloadAllianceImage(allianceID)
            End If

            ' Get the file if it's there
            If My.Computer.FileSystem.FileExists(filename) = True Then
                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
                Dim img As Image = Image.FromStream(fs)
                Dim ms As New MemoryStream()
                img.Save(ms, ImageFormat.Png)
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
            Return My.Resources.nochar
        End Try
    End Function

    Public Shared Function DownloadImage(ByVal downloadInfoObject As Object) As Image
        Try
            Dim downloadInfo() As Object = CType(downloadInfoObject, Object())
            Dim remoteURL As String = CStr(downloadInfo(0))
            Dim typeID As String = CStr(downloadInfo(1))
            Dim requestPic As WebRequest = WebRequest.Create(remoteURL)
            ' Setup proxy server (if required)
            If HQ.RemoteProxy IsNot Nothing Then
                If HQ.RemoteProxy.ProxyRequired = True Then
                    requestPic.Proxy = HQ.RemoteProxy.SetupWebProxy
                End If
            End If
            Dim responsePic As WebResponse = requestPic.GetResponse
            Dim webImage As Image = Image.FromStream(responsePic.GetResponseStream())
            ' Save the image into the cache
            webImage.Save(Path.Combine(HQ.imageCacheFolder, typeID & ".png"), ImageFormat.Png)
            Return webImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function

    Public Shared Function DownloadPortrait(ByVal characterID As String) As Image
        Try
            Dim requestPic As WebRequest = WebRequest.Create("https://image.eveonline.com/Character/" & characterID & "_256.jpg")
            ' Setup proxy server (if required)
            If HQ.RemoteProxy IsNot Nothing Then
                If HQ.RemoteProxy.ProxyRequired = True Then
                    requestPic.Proxy = HQ.RemoteProxy.SetupWebProxy
                End If
            End If
            Dim responsePic As WebResponse = requestPic.GetResponse
            Dim webImage As Image = Image.FromStream(responsePic.GetResponseStream())
            ' Save the image into the cache
            webImage.Save(Path.Combine(HQ.imageCacheFolder, characterID & ".png"), ImageFormat.Png)
            Return webImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function

    Public Shared Function DownloadCorpImage(ByVal corporationID As String) As Image
        Try
            Dim requestPic As WebRequest = WebRequest.Create("https://image.eveonline.com/Corporation/" & corporationID & "_256.png")
            ' Setup proxy server (if required)
            If HQ.RemoteProxy IsNot Nothing Then
                If HQ.RemoteProxy.ProxyRequired = True Then
                    requestPic.Proxy = HQ.RemoteProxy.SetupWebProxy
                End If
            End If
            Dim responsePic As WebResponse = requestPic.GetResponse
            Dim webImage As Image = Image.FromStream(responsePic.GetResponseStream())
            ' Save the image into the cache
            webImage.Save(Path.Combine(HQ.imageCacheFolder, corporationID & ".png"), ImageFormat.Png)
            Return webImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function

    Public Shared Function DownloadAllianceImage(ByVal allianceID As String) As Image
        Try
            Dim requestPic As WebRequest = WebRequest.Create("https://image.eveonline.com/Alliance/" & allianceID & "_256.png")
            ' Setup proxy server (if required)
            If HQ.RemoteProxy IsNot Nothing Then
                If HQ.RemoteProxy.ProxyRequired = True Then
                    requestPic.Proxy = HQ.RemoteProxy.SetupWebProxy
                End If
            End If
            Dim responsePic As WebResponse = requestPic.GetResponse
            Dim webImage As Image = Image.FromStream(responsePic.GetResponseStream())
            ' Save the image into the cache
            webImage.Save(Path.Combine(HQ.imageCacheFolder, allianceID & ".png"), ImageFormat.Png)
            Return webImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function

End Class
