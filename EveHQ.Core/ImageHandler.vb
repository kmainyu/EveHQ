Imports System.Net
Imports System.Drawing
Imports System.IO

Public Class ImageHandler

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

    Private Shared Sub DownloadImage(ByVal DownloadInfoObject As Object)
        Try
            Dim DownloadInfo() As Object = CType(DownloadInfoObject, Object())
            Dim remoteURL As String = CStr(DownloadInfo(0))
            Dim imageName As String = CStr(DownloadInfo(1))
            Dim RequestPic As WebRequest = WebRequest.Create(remoteURL)
            Dim ResponsePic As WebResponse = RequestPic.GetResponse
            Dim WebImage As Image = Image.FromStream(ResponsePic.GetResponseStream())
            ' Save the image into the cache
            WebImage.Save(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, imageName & ".png"), System.Drawing.Imaging.ImageFormat.Png)
        Catch ex As Exception
            ' Assume this is because the image was not found - which happens for a lot of the database items
            ' No need to do anything with the error, just carry on
        End Try
    End Sub

    Public Enum ImageType As Integer
        Blueprints = 0
        Types = 1
        Icons = 2
    End Enum
End Class

