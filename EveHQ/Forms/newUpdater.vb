Imports EveHQ.Common.Extensions
Imports EveHQ.Common
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.IO


Public Class newUpdater

    Private _updateLocation As String
    Private _proxyServer As String
    Private _useDefaultCred As Boolean
    Private _proxUsername As String
    Private _proxyPassword As String
    Private _useBasicAuth As Boolean
    Private _storageFodler As String


    Const RequestTemplate As String = "Requesting: {0}"
    Const DownloadingTemplate As String = "Downloading: {0}"


    Public Sub New(updateLocation As String, storageFolder As String, proxyServer As String, useDefaultCredentials As Boolean, proxyUserName As String, proxyPassword As String, useBasicAuth As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _updateLocation = updateLocation
        _proxyServer = proxyServer
        _useDefaultCred = useDefaultCredentials
        _proxUsername = proxyUserName
        _proxyPassword = proxyPassword
        _useBasicAuth = useBasicAuth
        _storageFodler = storageFolder
    End Sub

    Private Sub newUpdater_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        _statusLabel.Text = RequestTemplate.FormatInvariant(_updateLocation)
    End Sub

    Private Sub DownloadUpdate()
        Try
            Dim proxyUri As Uri = Nothing
            If _proxyServer.IsNullOrWhiteSpace() = False Then
                proxyUri = New Uri(_proxyServer)
            End If

            Dim localStorage As String = _storageFodler

            Dim task As Task(Of HttpResponseMessage) = WebRequestHelper.GetAsync(New Uri(_updateLocation), proxyUri, _useDefaultCred, _proxUsername, _proxyPassword, _useBasicAuth, Nothing, HttpCompletionOption.ResponseHeadersRead)

            task.ContinueWith(Sub(dlTask As Task(Of HttpResponseMessage))
                                  If (dlTask.Exception IsNot Nothing) Then
                                      Trace.TraceError(dlTask.Exception.FormatException())
                                  End If

                                  If (dlTask.Result.IsSuccessStatusCode = False) Then
                                      Trace.TraceWarning("Update download returned unexpected status {0} {1}".FormatInvariant(dlTask.Result.StatusCode, dlTask.Result.ReasonPhrase))
                                  End If

                                  If (dlTask.IsCanceled Or dlTask.Result Is Nothing) Then
                                      Exit Sub
                                  End If

                                  Dim total As Long
                                  Dim current As Double

                                  If (dlTask.Result.Content.Headers.ContentLength.HasValue) Then
                                      total = dlTask.Result.Content.Headers.ContentLength.Value
                                  Else
                                      total = 25000000
                                  End If

                                  Invoke(Sub()
                                             _statusLabel.Text = DownloadingTemplate.FormatInvariant(_updateLocation)
                                         End Sub)

                                  Try


                                      Dim streamTask As Task(Of Stream) = dlTask.Result.Content.ReadAsStreamAsync()
                                      streamTask.Wait()

                                      If (streamTask.Result IsNot Nothing) Then
                                          Dim stream As Stream = streamTask.Result
                                          Dim result As List(Of Byte) = New List(Of Byte)
                                          Dim readBuffer(500) As Byte
                                          Dim readBytes As Int32
                                          Do
                                              Array.Clear(readBuffer, 0, 500)
                                              readBytes = stream.Read(readBuffer, 0, readBuffer.Length)
                                              If (readBytes > 0) Then
                                                  result.AddRange(readBuffer.Take(readBytes))
                                                  current += readBytes
                                                  Dim progress As Double = (current / total) * 100
                                                  Invoke(Sub()
                                                             UpdateProgress(progress)
                                                         End Sub)
                                              End If
                                          Loop While readBytes <> 0
                                          streamTask.Result.Close()
                                          streamTask.Result.Dispose()

                                          Invoke(Sub()
                                                     _statusLabel.Text = "Saving update file to disk."
                                                     _statusLabel.TextAlign = ContentAlignment.MiddleCenter
                                                 End Sub)


                                          Dim savedFilePath As String = Path.Combine(localStorage, GetFileNameFromUrl(_updateLocation))
                                          Using fs As FileStream = New FileStream(savedFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
                                              fs.Write(result.ToArray(), 0, result.Count)
                                          End Using

                                          Invoke(Sub()
                                                     _statusLabel.Text = "Download Complete. Click Continue to install update."
                                                     _statusLabel.TextAlign = ContentAlignment.MiddleCenter
                                                     _continueButton.Visible = True
                                                 End Sub)

                                      End If
                                  Catch ex As Exception
                                      Trace.TraceError(ex.FormatException)
                                      Throw ex


                                  End Try
                              End Sub)


        Catch ex As Exception
            Trace.TraceError(ex.FormatException())
            MessageBox.Show("There was an unexpected error downloading the update. Please look at the log file located at {0}\EveHQLog.log for details, and try your attempt again later".FormatInvariant(Environment.SpecialFolder.ApplicationData))
        End Try
    End Sub

    Private Sub UpdateProgress(progress As Double)

        _downloadProgress.Text = "{0:N1} %".FormatInvariant(progress)
        _downloadProgress.TextVisible = True
        _downloadProgress.Value = CInt(progress)

    End Sub


    Private Shared Function GetFileNameFromUrl(url As String) As String

        Dim lastIndex As Int32 = url.LastIndexOf("/")

        Return url.Substring(lastIndex + 1)

    End Function


    Private Sub newUpdater_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        DownloadUpdate()
    End Sub

    Private Sub RunUpdateInstaller(sender As System.Object, e As System.EventArgs) Handles _continueButton.Click
        Dim exeFile As String = GetFileNameFromUrl(_updateLocation)





        'exit evehq so the installer will run
        Dim formsToClose As New List(Of String)
        For Each openForm As Form In Application.OpenForms

            formsToClose.Add(openForm.Name)
        Next

        For Each formName As String In formsToClose
            Dim openForm As Form = Application.OpenForms(formName)
            If (openForm IsNot Nothing) Then
                openForm.Close()
            End If
        Next
        Dim updateProcess As Process = Process.Start(Path.Combine(_storageFodler, exeFile))
    End Sub
End Class