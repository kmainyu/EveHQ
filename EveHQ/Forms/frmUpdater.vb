Imports DotNetLib.Windows.Forms
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Reflection
Imports System.Net.Sockets
Imports System.Threading
Imports System.Data
Imports System.IO.Compression

Public Class frmUpdater

    Dim pbControls As New SortedList
    Dim CurrentComponents As New SortedList
    Dim filesRequired As New SortedList
    Dim filesComplete As New SortedList
    Dim filesTried As Integer = 0
    Dim updateFolder As String = ""
    Dim updateRequired As Boolean = False
    Public startupTest As Boolean = False
    Dim BaseLocation As String = ""
    Dim PatcherLocation As String = ""

    Private Sub frmUpdater_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tmrUpdate.Interval = 100
        tmrUpdate.Enabled = True
        tmrUpdate.Start()
    End Sub

    Private Function FetchUpdateXML() As XmlDocument

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        Dim UpdateServer As String = EveHQ.Core.HQ.EveHQSettings.UpdateURL
        Dim remoteURL As String = UpdateServer & "/_updates.xml"
        Dim webdata As String = ""
        Dim UpdateXML As New XmlDocument
        Try
            lblUpdateStatus.Text = "Status: Attempting to obtain update file..."
            ' Create the requester
            ServicePointManager.DefaultConnectionLimit = 10
            ServicePointManager.Expect100Continue = False
            Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(remoteURL))
            Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
            request.CachePolicy = policy
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
                Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
                If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                    EveHQProxy.UseDefaultCredentials = True
                Else
                    EveHQProxy.UseDefaultCredentials = False
                    EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
                End If
                request.Proxy = EveHQProxy
            End If
            ' Prepare for a response from the server
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Get the stream associated with the response.
            Dim receiveStream As Stream = response.GetResponseStream()
            ' Pipes the stream to a higher level stream reader with the required encoding format. 
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            webdata = readStream.ReadToEnd()
            ' Check response string for any error codes?
            UpdateXML.LoadXml(webdata)
            Return UpdateXML
        Catch e As Exception
            Dim msg As String = e.Message & ControlChars.CrLf & ControlChars.CrLf
            MessageBox.Show(msg, "Error Obtaining Update File", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function

    Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        Call Me.ShowUpdates()
    End Sub

    Private Sub ShowUpdates()
        tmrUpdate.Enabled = False
        Dim UpdateXML As XmlDocument = FetchUpdateXML()
        If UpdateXML Is Nothing Then
            lblUpdateStatus.Text = "Status: Unable to obtain update file."
            btnRecheckUpdates.Enabled = True
            Exit Sub
        Else
            updateRequired = False
            ' Get a current list of components
            lblUpdateStatus.Text = "Status: Checking current components..."
            clvUpdates.Items.Clear()
            CurrentComponents.Clear()
            pbControls.Clear()
            For Each myAssembly As AssemblyName In Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                If CurrentComponents.Contains(myAssembly.Name & ".dll") = False Then
                    If myAssembly.Name.StartsWith("EveHQ") Then
                        CurrentComponents.Add(myAssembly.Name & ".dll", myAssembly.Version.ToString)
                        CurrentComponents.Add(myAssembly.Name & ".pdb", myAssembly.Version.ToString)
                    End If
                End If
            Next
            ' Add to that a list of the plug-ins used
            For Each myPlugIn As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
                If myPlugIn.ShortFileName IsNot Nothing Then
                    If CurrentComponents.Contains(myPlugIn.ShortFileName) = False Then
                        CurrentComponents.Add(myPlugIn.ShortFileName, myPlugIn.Version)
                        CurrentComponents.Add(System.IO.Path.GetFileNameWithoutExtension(myPlugIn.FileName) & ".pdb", myPlugIn.Version)
                    End If
                End If
            Next
            ' Add the current executable!
            CurrentComponents.Add("EveHQ.exe", My.Application.Info.Version.ToString)
            CurrentComponents.Add("EveHQ.pdb", My.Application.Info.Version.ToString)
            ' Add the LgLcd.dll - unique as not a .Net assembly
            If My.Computer.FileSystem.FileExists(Path.Combine(Application.StartupPath, "LgLcd.dll")) = True Then
                CurrentComponents.Add("LgLcd.dll", "Any")
            Else
                CurrentComponents.Add("LgLcd.dll", "Not Present")
            End If
            ' Try and add the database version (if using Access)
            If EveHQ.Core.HQ.EveHQSettings.DBFormat = 0 Then
                Dim databaseData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM EveHQVersion;")
                If databaseData IsNot Nothing Then
                    If databaseData.Tables(0).Rows.Count > 0 Then
                        CurrentComponents.Add("EveHQ.mdb.zip", databaseData.Tables(0).Rows(0).Item("Version").ToString)
                    Else
                        CurrentComponents.Add("EveHQ.mdb.zip", "1.0.0.0")
                    End If
                Else
                    CurrentComponents.Add("EveHQ.mdb.zip", "1.0.0.0")
                End If
            End If

            ' Try parsing the update file 
            lblUpdateStatus.Text = "Status: Parsing update file..."
            Try
                Dim updateDetails As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/lastUpdated")
                Dim lastUpdate As String = updateDetails(0).InnerText
                Dim requiredFiles As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/files/file")
                For Each updateFile As XmlNode In requiredFiles
                    Call AddUpdateToList(updateFile)
                Next
                If UpdateRequired = True Then
                    lblUpdateStatus.Text = "Status: Awaiting User Action..."
                    btnStartUpdate.Enabled = True
                Else
                    lblUpdateStatus.Text = "Status: No Updates Required!"
                    If startupTest = True Then
                        Me.Close()
                    End If
                    btnRecheckUpdates.Enabled = True
                End If
            Catch ex As Exception
                Dim ErrMsg As String = ex.Message & ControlChars.CrLf & ControlChars.CrLf
                MessageBox.Show(ErrMsg, "Error Parsing Update File", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub AddUpdateToList(ByVal updateFile As XmlNode)
        Dim newFile As New ContainerListViewItem
        newFile.Text = updateFile.ChildNodes(0).InnerText
        clvUpdates.Items.Add(newFile)
        newFile.SubItems(1).Text = updateFile.ChildNodes(1).InnerText
        If CurrentComponents.Contains(updateFile.ChildNodes(0).InnerText) = True Then
            newFile.SubItems(2).Text = CStr(CurrentComponents.Item(updateFile.ChildNodes(0).InnerText))
        End If
        newFile.SubItems(3).Text = updateFile.ChildNodes(2).InnerText
        ' Check if the plug-in is available
        If CStr(CurrentComponents.Item(updateFile.ChildNodes(0).InnerText)) IsNot Nothing Then
            ' Check which is the later version
            If IsUpdateAvailable(CStr(CurrentComponents.Item(updateFile.ChildNodes(0).InnerText)), updateFile.ChildNodes(2).InnerText) = True Then
                newFile.ForeColor = Color.Red
                Dim chkDownload As New CheckBox
                chkDownload.Name = newFile.Text
                chkDownload.Size = New Size(14, 14)
                If newFile.SubItems(1).Text = "Required" Then
                    chkDownload.Enabled = False
                Else
                    chkDownload.Enabled = True
                End If
                chkDownload.Checked = True
                newFile.SubItems(4).ItemControl = chkDownload
                newFile.SubItems(4).ControlResizeBehavior = ControlResizeBehavior.None
                Dim pbProgress As New ProgressBar
                pbProgress.Name = newFile.Text
                pbProgress.Width = 150
                pbProgress.Height = 16
                newFile.SubItems(5).ItemControl = pbProgress
                'newFile.SubItems(5).Text = "0%"
                pbControls.Add(newFile.Text, pbProgress)
                UpdateRequired = True
            Else
                Dim chkDownload As New CheckBox
                chkDownload.Name = newFile.Text
                chkDownload.Size = New Size(14, 14)
                chkDownload.Enabled = False
                chkDownload.Checked = False
                newFile.SubItems(4).ItemControl = chkDownload
                newFile.SubItems(4).ControlResizeBehavior = ControlResizeBehavior.None
                newFile.ForeColor = Color.LimeGreen
                newFile.SubItems(5).Text = "Update Not Required"
            End If
        Else
            If newFile.Text <> "EveHQ.mdb.zip" Or (newFile.Text = "EveHQ.mdb.zip" And EveHQ.Core.HQ.EveHQSettings.DBFormat = 0) Then
                newFile.SubItems(2).Text = "New!!"
                newFile.ForeColor = Color.Red
                Dim chkDownload As New CheckBox
                chkDownload.Name = newFile.Text
                chkDownload.Size = New Size(14, 14)
                chkDownload.Enabled = True
                chkDownload.Checked = True
                newFile.SubItems(4).ItemControl = chkDownload
                newFile.SubItems(4).ControlResizeBehavior = ControlResizeBehavior.None
                Dim pbProgress As New ProgressBar
                pbProgress.Name = newFile.Text
                pbProgress.Width = 150
                pbProgress.Height = 16
                newFile.SubItems(5).ItemControl = pbProgress
                'newFile.SubItems(5).Text = "0%"
                pbControls.Add(newFile.Text, pbProgress)
                updateRequired = True
            Else
                newFile.SubItems(2).Text = "Using SQL"
                Dim chkDownload As New CheckBox
                chkDownload.Name = newFile.Text
                chkDownload.Size = New Size(14, 14)
                chkDownload.Enabled = False
                chkDownload.Checked = False
                newFile.SubItems(4).ItemControl = chkDownload
                newFile.SubItems(4).ControlResizeBehavior = ControlResizeBehavior.None
                newFile.ForeColor = Color.LimeGreen
                newFile.SubItems(5).Text = "Update Not Required"
            End If
        End If
        If updateFile.ChildNodes(3).InnerText = "True" Then
            ' Add a pdb file reference
            updateFile.ChildNodes(3).InnerText = "False"
            updateFile.ChildNodes(0).InnerText = newFile.Text.Remove(newFile.Text.Length - 4, 4) & ".pdb"
            Call Me.AddUpdateToList(updateFile)
        End If
    End Sub

    Private Function IsUpdateAvailable(ByVal localVer As String, ByVal remoteVer As String) As Boolean
        If localVer = "Not Used" Then
            Return False
        Else
            If localVer = remoteVer Then
                Return False
            Else
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
            End If
        End If
    End Function

    Private Sub btnStartUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartUpdate.Click

        btnStartUpdate.Enabled = False
        filesTried = 0

        ' Reset the progress bars
        For Each pb As ProgressBar In pbControls.Values
            pb.Minimum = 0
            pb.Maximum = 100
            pb.Value = 0
        Next

        ' Check which files we are downloading (those that are checked)
        filesRequired.Clear()
        filesComplete.Clear()
        For Each item As ContainerListViewItem In clvUpdates.Items
            Dim chkDownload As CheckBox = CType(item.SubItems(4).ItemControl, CheckBox)
            If chkDownload IsNot Nothing Then
                If chkDownload.Checked = True Then
                    filesRequired.Add(item.Text, item)
                Else
                    item.SubItems(5).ItemControl = Nothing
                    item.SubItems(5).Text = "Not selected for download"
                End If
            End If
        Next

        ' Set Locations
        If EveHQ.Core.HQ.IsUsingLocalFolders = False Then
            BaseLocation = EveHQ.Core.HQ.appDataFolder
            PatcherLocation = Path.Combine(BaseLocation, "Updater")
            updateFolder = Path.Combine(BaseLocation, "Updates")
        Else
            BaseLocation = Application.StartupPath
            PatcherLocation = Path.Combine(BaseLocation, "Updater")
            updateFolder = Path.Combine(BaseLocation, "Updates")
        End If

        ' Check for existence of PatcherLocation
        If My.Computer.FileSystem.DirectoryExists(PatcherLocation) = False Then
            My.Computer.FileSystem.CreateDirectory(PatcherLocation)
        End If
        ' Check if the CoreControls.dll file can be copied
        Dim oldCCfile As String = Path.Combine(EveHQ.Core.HQ.appFolder, "EveHQ.CoreControls.dll")
        Dim newCCfile As String = Path.Combine(PatcherLocation, "EveHQ.CoreControls.dll")
        Try
            My.Computer.FileSystem.CopyFile(oldCCfile, newCCfile, True)
        Catch ex As Exception
            ' Access restricted therefore use the an AppData folder
            BaseLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveHQ")
            PatcherLocation = Path.Combine(BaseLocation, "Updater")
            updateFolder = Path.Combine(BaseLocation, "Updates")
            newCCfile = Path.Combine(PatcherLocation, "EveHQ.CoreControls.dll")
            Try
                My.Computer.FileSystem.CopyFile(oldCCfile, newCCfile, True)
            Catch ex2 As Exception
                ' Double failure - report and exit
                Dim msg As String = "Unable to copy necessary files for the update process to complete. Please update manually."
                MessageBox.Show(msg, "Unable to Update", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Try

        If My.Computer.FileSystem.DirectoryExists(updateFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(updateFolder)
        Else
            ' Clear the existing contents and recreate
            My.Computer.FileSystem.DeleteDirectory(updateFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(updateFolder)
        End If

        lblUpdateStatus.Text = "Status: Downloading updates..."
        For Each reqFile As String In filesRequired.Keys
            Dim updateWorker As New System.ComponentModel.BackgroundWorker
            AddHandler updateWorker.DoWork, AddressOf UpdateWorker_DoWork
            AddHandler updateWorker.ProgressChanged, AddressOf UpdateWorker_ProgressChanged
            AddHandler updateWorker.RunWorkerCompleted, AddressOf UpdateWorker_RunWorkerCompleted
            updateWorker.WorkerReportsProgress = True
            updateWorker.WorkerSupportsCancellation = True
            updateWorker.RunWorkerAsync(reqFile)
        Next
    End Sub

    Private Function DownloadFile(ByVal worker As System.ComponentModel.BackgroundWorker, ByVal FileNeeded As String, ByVal DebugFile As Boolean) As Boolean

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        Dim httpURI As String = ""
        Dim pdbFile As String = ""
        Dim localFile As String = ""
        If DebugFile = True Then
            Dim FI As New FileInfo(FileNeeded)
            pdbFile = FI.Name.TrimEnd(FI.Extension.ToCharArray) & ".pdb"
            httpURI = EveHQ.Core.HQ.EveHQSettings.UpdateURL & "/" & pdbFile
            localFile = Path.Combine(updateFolder, pdbFile)
        Else
            httpURI = EveHQ.Core.HQ.EveHQSettings.UpdateURL & "/" & FileNeeded
            localFile = Path.Combine(updateFolder, FileNeeded)
        End If
        ' Work out the debug file name to get

        ' Create the request to access the server and set credentials
        ServicePointManager.DefaultConnectionLimit = 10
        ServicePointManager.Expect100Continue = False
        Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(httpURI))
        Dim request As HttpWebRequest = CType(HttpWebRequest.Create(httpURI), HttpWebRequest)
        request.CachePolicy = policy
        ' Setup proxy server (if required)
        If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
            Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
            If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                EveHQProxy.UseDefaultCredentials = True
            Else
                EveHQProxy.UseDefaultCredentials = False
                EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
            End If
            request.Proxy = EveHQProxy
        End If
        request.CachePolicy = policy
        request.Method = WebRequestMethods.File.DownloadFile
        request.Timeout = 900000
        Try
            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
                Dim filesize As Long = CLng(response.ContentLength)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(localFile, IO.FileMode.Create)
                        Dim buffer(16383) As Byte
                        Dim read As Integer = 0
                        Dim totalBytes As Long = 0
                        Dim percent As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                            totalBytes += read
                            percent = CInt(totalBytes / filesize * 100)
                            worker.ReportProgress(percent, FileNeeded)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
            If DebugFile = True Then
                Dim FI As New FileInfo(FileNeeded)
                FileNeeded = FI.Name.TrimEnd(FI.Extension.ToCharArray) & ".pdb"
            End If
            filesComplete.Add(FileNeeded, True)
            filesTried += 1
            Return True
        Catch e As WebException
            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
            errMsg &= "Status: " & e.Status & ControlChars.CrLf
            errMsg &= "Message: " & e.Message & ControlChars.CrLf
            MessageBox.Show(errMsg, "Error Downloading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            filesTried += 1
            worker.CancelAsync()
            Return False
        End Try

    End Function

    Private Sub UpdateWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Dim reqfile As String = CStr(e.Argument)
        Call DownloadFile(CType(sender, System.ComponentModel.BackgroundWorker), reqfile, False)
    End Sub

    Private Sub UpdateWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)
        Dim fileName As String = e.UserState.ToString
        Dim pb As ProgressBar = CType(pbControls(fileName), ProgressBar)
        pb.Value = e.ProgressPercentage
    End Sub

    Private Sub UpdateWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        For Each item As ContainerListViewItem In clvUpdates.Items
            If filesComplete.Contains(item.Text) = True Then
                item.SubItems(5).ItemControl = Nothing
                If CBool(filesComplete.Item(item.Text)) = True Then
                    item.ForeColor = Color.LimeGreen
                    item.SubItems(5).Text = "Download Complete!"
                Else
                    item.ForeColor = Color.Red
                    item.SubItems(5).Text = "Download Failed!"
                End If
            End If
        Next
        If filesTried = filesRequired.Count Then
            If filesComplete.Count = filesRequired.Count Then
                lblUpdateStatus.Text = "Status: Download complete!"
                ' Ask the user if they want to update now
                Dim msg As String = "The download process is complete. Would you like to close and update EveHQ now?"
                If MessageBox.Show(msg, "Update Now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    ' Activate the update menu
                    EveHQ.Core.HQ.AppUpdateAvailable = True
                    frmEveHQ.mnuUpdateNow.Enabled = True
                    Me.Close()
                Else
                    
                    ' Try and download patchfile
                    Dim patcherFile As String = Path.Combine(PatcherLocation, "EveHQPatcher.exe")
                    Try
                        lblUpdateStatus.Text = "Status: Fetching Patcher File!"
                        Call Me.DownloadPatcherFile("EveHQPatcher.exe")
                        'MessageBox.Show("Patcher Deployment Successful!", "Patcher Deployment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        tmrUpdate.Enabled = True
                    Catch Excep As System.Runtime.InteropServices.COMException
                        Dim errMsg As String = "Unable to copy Patcher to " & ControlChars.CrLf & ControlChars.CrLf & patcherFile & ControlChars.CrLf & ControlChars.CrLf
                        errMsg &= "Please make sure this file is in the EveHQ program directory before continuing."
                        MessageBox.Show(errMsg, "Error Copying Patcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End Try
                    Dim startInfo As ProcessStartInfo = New ProcessStartInfo()
                    startInfo.UseShellExecute = True
                    startInfo.WorkingDirectory = Environment.CurrentDirectory
                    startInfo.FileName = patcherFile
                    Dim args As String = " /App;" & ControlChars.Quote & EveHQ.Core.HQ.appFolder & ControlChars.Quote
                    args &= " /Base;" & ControlChars.Quote & BaseLocation & ControlChars.Quote
                    If EveHQ.Core.HQ.IsUsingLocalFolders = True Then
                        args &= " /Local;True"
                    Else
                        args &= " /Local;False"
                    End If
                    If EveHQ.Core.HQ.EveHQSettings.DBFormat = 0 Then
                        args &= " /DB;" & ControlChars.Quote & EveHQ.Core.HQ.EveHQSettings.DBFilename & ControlChars.Quote
                    Else
                        args &= " /DB;None"
                    End If
                    startInfo.Arguments = args
                    Dim osInfo As OperatingSystem = System.Environment.OSVersion
                    If osInfo.Version.Major > 5 Then
                        startInfo.Verb = "runas"
                    End If
                    Process.Start(startInfo)
                    EveHQ.Core.HQ.StartShutdownEveHQ = True
                    Me.Close()
                End If
            Else
                MessageBox.Show("There was an error downloading the update files so the process has been aborted. Please try again and if the update continues to fail, please post a bug report.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End If
        End If
    End Sub

    Private Sub btnRecheckUpdates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecheckUpdates.Click
        btnRecheckUpdates.Enabled = False
        Call Me.ShowUpdates()
    End Sub

    Private Function DownloadPatcherFile(ByVal FileNeeded As String) As Boolean

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        Dim httpURI As String = EveHQ.Core.HQ.EveHQSettings.UpdateURL & "/" & FileNeeded
        Dim localFile As String = Path.Combine(PatcherLocation, FileNeeded)

        ' Create the request to access the server and set credentials
        ServicePointManager.DefaultConnectionLimit = 10
        ServicePointManager.Expect100Continue = False
        Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(httpURI))
        Dim request As HttpWebRequest = CType(HttpWebRequest.Create(httpURI), HttpWebRequest)
        request.CachePolicy = policy
        ' Setup proxy server (if required)
        If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
            Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
            If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                EveHQProxy.UseDefaultCredentials = True
            Else
                EveHQProxy.UseDefaultCredentials = False
                EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
            End If
            request.Proxy = EveHQProxy
        End If
        request.CachePolicy = policy
        request.Method = WebRequestMethods.File.DownloadFile
        request.Timeout = 900000
        Try
            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
                Dim filesize As Long = CLng(response.ContentLength)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(localFile, IO.FileMode.Create)
                        Dim buffer(16383) As Byte
                        Dim read As Integer = 0
                        Dim totalBytes As Long = 0
                        Dim percent As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                            totalBytes += read
                            percent = CInt(totalBytes / filesize * 100)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
            Return True
        Catch e As WebException
            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
            errMsg &= "Status: " & e.Status & ControlChars.CrLf
            errMsg &= "Message: " & e.Message & ControlChars.CrLf
            MessageBox.Show(errMsg, "Error Downloading Patcher File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End Try

    End Function

    
End Class

