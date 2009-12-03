Imports System.Net
Imports System.Drawing
Imports System.IO

Public Class ImageHandler

    Public Shared Function GetRawImageLocation(ByVal imageName As String, ByVal imageType As Integer) As String
        Dim remoteURL As String = ""
        Select Case imageType
            Case ImageHandler.ImageType.Blueprints
                remoteURL = "http://www.evehq.net/eve/images/blueprints/64_64/" & imageName & ".png"
            Case ImageHandler.ImageType.Icons
                remoteURL = "http://www.evehq.net/eve/images/icons/64_64/icon" & imageName & ".png"
            Case ImageHandler.ImageType.Types
                remoteURL = "http://www.evehq.net/eve/images/types/128_128/" & imageName & ".png"
        End Select
        ' Return the image location
        Return remoteURL

    End Function

    Public Shared Function GetImageLocation(ByVal imageName As String, ByVal imageType As Integer) As String
        ' Check EveHQ image cache folder for the image
        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, imageName & ".png")) = True Then
            ' Return the cached image location
            Return Path.Combine(EveHQ.Core.HQ.imageCacheFolder, imageName & ".png")
        Else
            ' Calculate the remote URL
            Dim remoteURL As String = ""
            Select Case imageType
                Case ImageHandler.ImageType.Blueprints
                    remoteURL = "http://www.evehq.net/eve/images/blueprints/64_64/" & imageName & ".png"
                Case ImageHandler.ImageType.Icons
                    remoteURL = "http://www.evehq.net/eve/images/icons/64_64/icon" & imageName & ".png"
                Case ImageHandler.ImageType.Types
                    remoteURL = "http://www.evehq.net/eve/images/types/128_128/" & imageName & ".png"
            End Select
            ' Download the image
            Threading.ThreadPool.QueueUserWorkItem(AddressOf DownloadImage, New Object() {remoteURL, imageName})
            ' Return the image location
            Return remoteURL
        End If

    End Function

    Public Shared Function GetImage(ByVal imageName As String, ByVal imageType As Integer) As Image
        ' Check EveHQ image cache folder for the image
        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, imageName & ".png")) = True Then
            ' Return the cached image location
            Return Image.FromFile(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, imageName & ".png"))
        Else
            ' Calculate the remote URL
            Dim remoteURL As String = ""
            Select Case imageType
                Case ImageHandler.ImageType.Blueprints
                    remoteURL = "http://www.evehq.net/eve/images/blueprints/64_64/" & imageName & ".png"
                Case ImageHandler.ImageType.Icons
                    remoteURL = "http://www.evehq.net/eve/images/icons/64_64/icon" & imageName & ".png"
                Case ImageHandler.ImageType.Types
                    remoteURL = "http://www.evehq.net/eve/images/types/128_128/" & imageName & ".png"
            End Select
            ' Download the image
            Return DownloadImage(New Object() {remoteURL, imageName})
        End If

    End Function

    Private Shared Function DownloadImage(ByVal DownloadInfoObject As Object) As Image
        Try
            Dim DownloadInfo() As Object = CType(DownloadInfoObject, Object())
            Dim remoteURL As String = CStr(DownloadInfo(0))
            Dim imageName As String = CStr(DownloadInfo(1))
            Dim RequestPic As WebRequest = WebRequest.Create(remoteURL)
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
                Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
                If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                    EveHQProxy.UseDefaultCredentials = True
                Else
                    EveHQProxy.UseDefaultCredentials = False
                    EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
                End If
                RequestPic.Proxy = EveHQProxy
            End If
            Dim ResponsePic As WebResponse = RequestPic.GetResponse
            Dim WebImage As Image = Image.FromStream(ResponsePic.GetResponseStream())
            ' Save the image into the cache
            WebImage.Save(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, imageName & ".png"), System.Drawing.Imaging.ImageFormat.Png)
            Return WebImage
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
            Return Nothing
        End Try
    End Function

    Public Enum ImageType As Integer
        Blueprints = 0
        Types = 1
        Icons = 2
    End Enum
End Class

