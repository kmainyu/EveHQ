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
    Dim DatabaseUpgradeAvailable As Boolean = False
    Public startupTest As Boolean = False

    Private Sub frmUpdater_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tmrUpdate.Enabled = True
    End Sub

    Private Function FetchUpdateXML() As XmlDocument

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        Dim UpdateServer As String = EveHQ.Core.HQ.EveHQSettings.UpdateURL
        Dim remoteURL As String = UpdateServer & "/_latest.xml"
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
            Dim UpdateRequired As Boolean = False
            ' Get a current list of components
            lblUpdateStatus.Text = "Status: Checking current components..."
            clvUpdates.Items.Clear()
            CurrentComponents.Clear()
            pbControls.Clear()
            Dim msg As String = ""
            For Each myAssembly As AssemblyName In Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                If CurrentComponents.Contains(myAssembly.Name & ".dll") = False Then
                    CurrentComponents.Add(myAssembly.Name & ".dll", myAssembly.Version.ToString)
                    msg &= myAssembly.Name & ".dll (" & myAssembly.Version.ToString & ")" & ControlChars.CrLf
                End If
                If myAssembly.Name = "EveHQ.Core" Or myAssembly.Name = "EveHQ.CoreControls" Then
                    ' Check the references for these
                    Dim subAssembly As Assembly = Assembly.ReflectionOnlyLoad(myAssembly.Name)
                    For Each subAssemblyName As AssemblyName In subAssembly.GetReferencedAssemblies
                        If CurrentComponents.Contains(subAssemblyName.Name & ".dll") = False Then
                            CurrentComponents.Add(subAssemblyName.Name & ".dll", subAssemblyName.Version.ToString)
                            msg &= subAssemblyName.Name & ".dll (" & subAssemblyName.Version.ToString & ")" & ControlChars.CrLf
                        End If
                    Next
                End If
            Next
            ' Add to that a list of the plug-ins used
            For Each myPlugIn As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
                If myPlugIn.ShortFileName IsNot Nothing Then
                    If CurrentComponents.Contains(myPlugIn.ShortFileName) = False Then
                        CurrentComponents.Add(myPlugIn.ShortFileName, myPlugIn.Version)
                        msg &= myPlugIn.ShortFileName & " (" & myPlugIn.Version & ")" & ControlChars.CrLf
                    End If
                End If
            Next
            ' Add the current executable!
            CurrentComponents.Add("EveHQ.exe", My.Application.Info.Version.ToString)
            msg &= "EveHQ.exe (" & My.Application.Info.Version.ToString & ")" & ControlChars.CrLf
            ' Add the EveHQPatcher if available?
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\EveHQPatcher.exe") = True Then
                Dim myAssembly As Assembly = Assembly.ReflectionOnlyLoadFrom(Application.StartupPath & "\EveHQPatcher.exe")
                CurrentComponents.Add("EveHQPatcher.exe", myAssembly.GetName.Version.ToString)
                msg &= "EveHQPatcher.exe (" & myAssembly.GetName.Version.ToString & ")" & ControlChars.CrLf
            Else
                CurrentComponents.Add("EveHQPatcher.exe", "Not Present")
                msg &= "EveHQPatcher.exe (Not Present)" & ControlChars.CrLf
            End If
            ' Add the LgLcd.dll - unique as not a .Net assembly
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\LgLcd.dll") = True Then
                CurrentComponents.Add("LgLcd.dll", "Any")
                msg &= "LgLcd.dll (Any)" & ControlChars.CrLf
            Else
                CurrentComponents.Add("LgLcd.dll", "Not Present")
                msg &= "LgLcd.dll (Not Present)" & ControlChars.CrLf
            End If
            ' Try and add the database version (if using Access)
            Dim DBData As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/database")
            Dim localDBVersion As String = ""
            Dim remoteDBVersion As String = ""
            If DBData.Count > 0 Then
                remoteDBVersion = DBData(0).ChildNodes(0).InnerText
            End If
            If EveHQ.Core.HQ.EveHQSettings.DBFormat = 0 Then
                Dim databaseData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM EveHQVersion;")
                If databaseData IsNot Nothing Then
                    If databaseData.Tables(0).Rows.Count > 0 Then
                        localDBVersion = databaseData.Tables(0).Rows(0).Item("Version").ToString
                        If IsUpdateAvailable(localDBVersion, remoteDBVersion) = True Then
                            DatabaseUpgradeAvailable = True
                        Else
                            DatabaseUpgradeAvailable = False
                        End If
                    Else
                        If remoteDBVersion <> "" Then
                            DatabaseUpgradeAvailable = True
                        Else
                            DatabaseUpgradeAvailable = False
                        End If
                    End If
                Else
                    If remoteDBVersion <> "" Then
                        DatabaseUpgradeAvailable = True
                    Else
                        DatabaseUpgradeAvailable = False
                    End If
                End If
            Else
                DatabaseUpgradeAvailable = False
            End If
            ' Try parsing the update file 
            lblUpdateStatus.Text = "Status: Parsing update file..."
            Try
                Dim updateDetails As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/lastUpdated")
                Dim lastUpdate As String = updateDetails(0).InnerText
                Dim requiredFiles As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/files/file")
                For Each updateFile As XmlNode In requiredFiles
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
                        UpdateRequired = True
                    End If
                Next
                If UpdateRequired = True Then
                    lblUpdateStatus.Text = "Status: Awaiting User Action..."
                    btnStartUpdate.Enabled = True
                Else
                    If DatabaseUpgradeAvailable = True Then
                        lblUpdateStatus.Text = "Status: No Updates Required (Database Available)"
                    Else
                        lblUpdateStatus.Text = "Status: No Updates Required!"
                        If startupTest = True Then
                            Me.Close()
                        End If
                    End If
                    btnRecheckUpdates.Enabled = True
                End If
                ' Report if there is a database upgrade available
                If DatabaseUpgradeAvailable = True Then
                    Dim DBMsg As String = "There is a new version of the Access database available for download." & ControlChars.CrLf & ControlChars.CrLf
                    DBMsg &= "It is recommended that you upgrade your database as soon as possible so as to include all the latest changes from Eve patches and to maximise compatability with EveHQ." & ControlChars.CrLf & ControlChars.CrLf
                    DBMsg &= "Please visit the EveHQ website to obtain this newer version."
                    MessageBox.Show(DBMsg, "Database Upgrade Available", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                Dim ErrMsg As String = ex.Message & ControlChars.CrLf & ControlChars.CrLf
                MessageBox.Show(ErrMsg, "Error Parsing Update File", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
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

        'Dim count As Integer = 0
        'count += 1
        Dim httpURI As String = ""
        Dim pdbFile As String = ""
        Dim localFile As String = ""
        If DebugFile = True Then
            Dim FI As New FileInfo(FileNeeded)
            pdbFile = FI.Name.TrimEnd(FI.Extension.ToCharArray) & ".pdb"
            httpURI = EveHQ.Core.HQ.EveHQSettings.UpdateURL & "/" & pdbFile
            localFile = My.Application.Info.DirectoryPath & "\" & pdbFile & ".upd"
        Else
            httpURI = EveHQ.Core.HQ.EveHQSettings.UpdateURL & "/" & FileNeeded
            localFile = My.Application.Info.DirectoryPath & "\" & FileNeeded & ".upd"
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
                        Dim buffer(4095) As Byte
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
        Catch e As WebException
            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
            errMsg &= "Status: " & e.Status & ControlChars.CrLf
            errMsg &= "Message: " & e.Message & ControlChars.CrLf
            MessageBox.Show(errMsg, "Error Uploading File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            worker.CancelAsync()
            filesComplete.Add(FileNeeded, False)
            Return False
        End Try

        If DebugFile = True Then
            Dim FI As New FileInfo(FileNeeded)
            FileNeeded = FI.Name.TrimEnd(FI.Extension.ToCharArray) & ".pdb"
            '    If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\" & FileNeeded & ".upd") = True Then
            '        My.Computer.FileSystem.DeleteFile(My.Application.Info.DirectoryPath & "\" & FileNeeded & ".upd")
            '    End If
            '    My.Computer.FileSystem.RenameFile(My.Application.Info.DirectoryPath & "\" & pdbFile & ".upd", FileNeeded & ".upd")
        End If

        filesComplete.Add(FileNeeded, True)
        Return True

    End Function

    Private Function DownloadFile2(ByVal worker As System.ComponentModel.BackgroundWorker, ByVal FileNeeded As String) As Boolean

        Dim percent As Integer = 0
        For a As Integer = 1 To 100000
            percent = CInt(a / 1000)
            worker.ReportProgress(percent, FileNeeded)
        Next

    End Function

    Private Sub UpdateWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Dim reqfile As String = CStr(e.Argument)
        Call DownloadFile(CType(sender, System.ComponentModel.BackgroundWorker), reqfile, False)
        Call DownloadFile(CType(sender, System.ComponentModel.BackgroundWorker), reqfile, True)
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
        If filesComplete.Count = (filesRequired.Count * 2) Then
            lblUpdateStatus.Text = "Updating Files..."
            Call UpdateEveHQ()
            lblUpdateStatus.Text = "Status: Update Complete!"
            Dim msg As String = "All Updates Completed! EveHQ needs to be restarted to use the new updates." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Would you like to close EveHQ now?"
            Dim result As Integer = MessageBox.Show(msg, "Update Complete!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.No Then
                Me.Close()
            Else
                EveHQ.Core.HQ.StartShutdownEveHQ = True
                Me.Close()
            End If
        End If
    End Sub

    Private Function UpdateEveHQ() As Boolean
        Try
            Dim oldFile As String = ""
            For Each newFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.upd")
                oldFile = newFile.TrimEnd(".upd".ToCharArray)
                Dim ofi As New IO.FileInfo(oldFile)
                Dim nfi As New IO.FileInfo(newFile)
                ' Check if this is the database upgrade
                If nfi.Name = "EveHQ.mdb.upd" Then
                    ' Get the current database file location and rename it
                    If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.DBFilename) = True Then
                        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.DBFilename & ".old") = True Then
                            My.Computer.FileSystem.DeleteFile(EveHQ.Core.HQ.EveHQSettings.DBFilename & ".old")
                        End If
                        My.Computer.FileSystem.RenameFile(EveHQ.Core.HQ.EveHQSettings.DBFilename, EveHQ.Core.HQ.EveHQSettings.DBFilename & ".old")
                    End If
                    ' Uncompress the database file
                    DecompressFile(nfi.Name, EveHQ.Core.HQ.EveHQSettings.DBFilename)
                Else
                    ' Not the DB upgrade
                    If ofi.Exists = True Then
                        My.Computer.FileSystem.RenameFile(ofi.FullName, ofi.Name & ".old")
                    End If
                    ' Copy the new file as the old one
                    My.Computer.FileSystem.CopyFile(nfi.FullName, ofi.FullName, True)
                    ' If no errors, then delete the update file
                    My.Computer.FileSystem.DeleteFile(nfi.FullName)
                End If
            Next
        Catch e As Exception
            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
            errMsg &= "Message: " & e.Message & ControlChars.CrLf
            MessageBox.Show(errMsg, "Error Updating EveHQ Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Function

    Private Sub btnRecheckUpdates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecheckUpdates.Click
        btnRecheckUpdates.Enabled = False
        Call Me.ShowUpdates()
    End Sub

    Public Sub CompressFile(ByVal sourceFile As String, ByVal destinationFile As String)

        ' make sure the source file is there
        If File.Exists(sourceFile) = False Then
            Throw New FileNotFoundException
        End If

        ' Create the streams and byte arrays needed
        Dim buffer As Byte() = Nothing
        Dim sourceStream As FileStream = Nothing
        Dim destinationStream As FileStream = Nothing
        Dim compressedStream As GZipStream = Nothing

        Try
            ' Read the bytes from the source file into a byte array
            sourceStream = New FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read)

            ' Read the source stream values into the buffer
            buffer = New Byte(CInt(sourceStream.Length)) {}
            Dim checkCounter As Integer = sourceStream.Read(buffer, 0, buffer.Length)

            ' Open the FileStream to write to
            destinationStream = New FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write)

            ' Create a compression stream pointing to the destiantion stream
            compressedStream = New GZipStream(destinationStream, CompressionMode.Compress, True)

            'Now write the compressed data to the destination file
            compressedStream.Write(buffer, 0, buffer.Length)

        Catch ex As ApplicationException
            MessageBox.Show(ex.Message, "An Error occured during compression", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Make sure we allways close all streams
            If Not (sourceStream Is Nothing) Then
                sourceStream.Close()
            End If
            If Not (compressedStream Is Nothing) Then
                compressedStream.Close()
            End If
            If Not (destinationStream Is Nothing) Then
                destinationStream.Close()
            End If
        End Try

    End Sub

    Public Sub DecompressFile(ByVal sourceFile As String, ByVal destinationFile As String)

        ' make sure the source file is there
        If File.Exists(sourceFile) = False Then
            Throw New FileNotFoundException
        End If

        ' Create the streams and byte arrays needed
        Dim sourceStream As FileStream = Nothing
        Dim destinationStream As FileStream = Nothing
        Dim decompressedStream As GZipStream = Nothing
        Dim quartetBuffer As Byte() = Nothing

        Try
            ' Read in the compressed source stream
            sourceStream = New FileStream(sourceFile, FileMode.Open)

            ' Create a compression stream pointing to the destiantion stream
            decompressedStream = New GZipStream(sourceStream, CompressionMode.Decompress, True)

            ' Read the footer to determine the length of the destiantion file
            quartetBuffer = New Byte(4) {}
            Dim position As Integer = CType(sourceStream.Length, Integer) - 4
            sourceStream.Position = position
            sourceStream.Read(quartetBuffer, 0, 4)
            sourceStream.Position = 0
            Dim checkLength As Integer = BitConverter.ToInt32(quartetBuffer, 0)

            Dim buffer(checkLength + 100) As Byte
            Dim offset As Integer = 0
            Dim total As Integer = 0

            ' Read the compressed data into the buffer
            While True
                Dim bytesRead As Integer = decompressedStream.Read(buffer, offset, 100)
                If bytesRead = 0 Then
                    Exit While
                End If
                offset += bytesRead
                total += bytesRead
            End While

            ' Now write everything to the destination file
            destinationStream = New FileStream(destinationFile, FileMode.Create)
            destinationStream.Write(buffer, 0, total)

            ' and flush everyhting to clean out the buffer
            destinationStream.Flush()

        Catch ex As ApplicationException
            MessageBox.Show(ex.Message, "An Error occured during compression", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Make sure we allways close all streams
            If Not (sourceStream Is Nothing) Then
                sourceStream.Close()
            End If
            If Not (decompressedStream Is Nothing) Then
                decompressedStream.Close()
            End If
            If Not (destinationStream Is Nothing) Then
                destinationStream.Close()
            End If
        End Try

    End Sub

    'Public Sub CheckForUpdates(ByVal showNoUpdate As Boolean)

    '    ' Set a default policy level for the "http:" and "https" schemes.
    '    Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

    '    ' Create the request to access the server and set credentials
    '    Dim URL As String = "http://www.evehq.net/downloads/_latest.txt"
    '    Dim localfile As String = EveHQ.Core.HQ.appDataFolder & "\_latest.txt"
    '    Dim request As HttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)
    '    request.CachePolicy = policy
    '    request.Method = WebRequestMethods.File.DownloadFile
    '    Try
    '        Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
    '            Dim filesize As Long = CLng(response.ContentLength)
    '            'Using response As FtpWebResponse = CType(request.GetResponse, FtpWebResponse)
    '            Using responseStream As IO.Stream = response.GetResponseStream
    '                'loop to read & write to file
    '                Using fs As New IO.FileStream(localfile, IO.FileMode.Create)
    '                    Dim buffer(2047) As Byte
    '                    Dim read As Integer = 0
    '                    Do
    '                        read = responseStream.Read(buffer, 0, buffer.Length)
    '                        fs.Write(buffer, 0, read)
    '                    Loop Until read = 0 'see Note(1)
    '                    responseStream.Close()
    '                    fs.Flush()
    '                    fs.Close()
    '                End Using
    '                responseStream.Close()
    '            End Using
    '            response.Close()
    '        End Using
    '        Dim strData As String = ""
    '        Dim sr As StreamReader = New StreamReader(localfile)
    '        strData = sr.ReadToEnd
    '        sr.Close()

    '        Dim updatesRequired As New ArrayList
    '        Dim filesRequired As New ArrayList
    '        Dim files() As String = strData.Split(ControlChars.Cr)
    '        Dim file As String = ""
    '        For Each file In files
    '            file = file.Trim(ControlChars.Lf)
    '            If file.Trim <> "" Then
    '                Dim fileDetails() As String = file.Split(CChar(","))
    '                Dim fileName As String = fileDetails(0)
    '                Dim fileVersion As String = fileDetails(1)

    '                ' Check the plugins first
    '                For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.PlugIns.Values
    '                    If PlugInInfo.Available = True Then
    '                        If PlugInInfo.ShortFileName = fileName Then
    '                            If CompareVersions(PlugInInfo.Version, fileVersion) = True Then
    '                                updatesRequired.Add(fileName & " (Current: " & PlugInInfo.Version & ", Available: " & fileVersion & ")")
    '                                filesRequired.Add(fileName)
    '                            End If
    '                            Exit For
    '                        End If
    '                    End If
    '                Next
    '                ' Check for the main EveHQ App
    '                If fileName = "EveHQ.exe" Then
    '                    If CompareVersions(My.Application.Info.Version.ToString, fileVersion) = True Then
    '                        updatesRequired.Add(fileName & " (Current: " & My.Application.Info.Version.ToString & ", Available: " & fileVersion & ")")
    '                        filesRequired.Add(fileName)
    '                    End If
    '                End If
    '                ' Check for the EveHQ.Core.dll
    '                If fileName = "EveHQ.Core.dll" Then
    '                    Dim coreVersion As String = ""
    '                    For Each asm As System.Reflection.AssemblyName In System.Reflection.Assembly.GetExecutingAssembly.GetReferencedAssemblies
    '                        If asm.Name = "EveHQ.Core" Then
    '                            coreVersion = asm.Version.ToString
    '                        End If
    '                    Next
    '                    If CompareVersions(coreVersion, fileVersion) = True Then
    '                        updatesRequired.Add(fileName & " (Current: " & coreVersion & ", Available: " & fileVersion & ")")
    '                        filesRequired.Add(fileName)
    '                    End If
    '                End If
    '            End If
    '        Next

    '        Dim msg As String = ""
    '        If updatesRequired.Count > 0 Then
    '            msg &= "The following updates are available for download:" & ControlChars.CrLf & ControlChars.CrLf
    '            For Each update As String In updatesRequired
    '                msg &= update & ControlChars.CrLf
    '            Next
    '            msg &= ControlChars.CrLf & "Would you like to download and install these updates now?"
    '            If MessageBox.Show(msg, "EveHQ Update Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
    '                Exit Sub
    '            Else
    '                Dim sw As New StreamWriter(EveHQ.Core.HQ.appDataFolder & "/EveHQ.upd")
    '                For Each fileRequired As String In filesRequired
    '                    sw.WriteLine(fileRequired)
    '                Next
    '                sw.Flush()
    '                sw.Close()
    '                Dim startInfo As ProcessStartInfo = New ProcessStartInfo()
    '                startInfo.UseShellExecute = True
    '                startInfo.WorkingDirectory = Environment.CurrentDirectory
    '                startInfo.FileName = EveHQ.Core.HQ.appFolder & "\EveHQPatcher.exe"
    '                startInfo.Verb = "runas"
    '                Process.Start(startInfo)
    '            End If
    '        Else
    '            If showNoUpdate = False Then
    '                msg = "Your EveHQ installation is up-to-date."
    '                MessageBox.Show(msg, "EveHQ Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            End If
    '        End If

    '    Catch e As Exception
    '        Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
    '        errMsg &= "Message: " & e.Message & ControlChars.CrLf
    '        MessageBox.Show(errMsg, "Error Accessing Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Sub

    'Private Function CompareVersions(ByVal localVer As String, ByVal remoteVer As String) As Boolean
    '    Dim localVers() As String = localVer.Split(CChar("."))
    '    Dim remoteVers() As String = remoteVer.Split(CChar("."))
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

