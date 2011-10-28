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
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Reflection
Imports System.Net.Sockets
Imports System.Threading
Imports System.Data
Imports System.IO.Compression
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports System.ComponentModel

Public Class frmUpdater

    Dim filesComplete As New SortedList
    Dim filesTried As Integer = 0
    Dim updateFolder As String = ""
    Dim updateRequired As Boolean = False
    Public startupTest As Boolean = False
    Dim BaseLocation As String = ""
    Dim PatcherLocation As String = ""
    Dim UpdateRequiredStyle As ElementStyle
    Dim UpdateNotRequiredStyle As ElementStyle
    Dim WithEvents MainUpdateWorker As New BackgroundWorker
    Dim UpdateWorkerList As New SortedList(Of String, BackgroundWorker)

    ' New variables
    Dim EveHQComponents As New SortedList(Of String, EveHQComponent) ' Stores list of current EveHQ files (not available!)
    Dim PBControls As New SortedList(Of String, ProgressBarItem) ' Progress bars for main files
    Dim PDBControls As New SortedList(Of String, ProgressBarItem) ' Progress bars for PDB files
    Dim NumberOfFilesRequired As Integer = 0
    Dim FilesRequired As New Queue(Of String)
    Dim UpdateQueue As New List(Of String)
    Dim UpdateAborted As Boolean = False

#Region " Form Opening and Closing Routines"

    Private Sub frmUpdater_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If EveHQ.Core.HQ.EveHQIsUpdating = True Then
            MessageBox.Show("Please wait while the download process is completed before closing the form. If you need to cancel the update, click the Cancel Update button first.", "Update in Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub frmUpdater_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tmrUpdate.Interval = 100
        tmrUpdate.Enabled = True
        tmrUpdate.Start()
        ' Set Styles
        UpdateRequiredStyle = adtUpdates.Styles("Log").Copy
        UpdateNotRequiredStyle = adtUpdates.Styles("Log").Copy
        UpdateRequiredStyle.TextColor = Color.Red
        UpdateNotRequiredStyle.TextColor = Color.LimeGreen
        ' Check height
        If Screen.PrimaryScreen.Bounds.Height <= 800 Then
            Me.Height = Screen.PrimaryScreen.Bounds.Height - 100
        End If
        ' Set Max Downloads
        nudDownloads.Value = EveHQ.Core.HQ.EveHQSettings.MaxUpdateThreads

        ' Temp
        EveHQ.Core.HQ.EveHQSettings.UpdateURL = "http://www.evehq.net/updatetest/"

    End Sub

#End Region

#Region "Update File Retrieval and Parsing Routines"

    Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        Call Me.ShowUpdates()
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
            ServicePointManager.DefaultConnectionLimit = 5
            ServicePointManager.Expect100Continue = False
            Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(remoteURL))
            Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
            request.UserAgent = "EveHQ Updater " & My.Application.Info.Version.ToString
            request.CachePolicy = policy
            ' Setup proxy server (if required)
            Call EveHQ.Core.ProxyServerFunctions.SetupWebProxy(request)
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

    Private Sub ShowUpdates()

        tmrUpdate.Enabled = False

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

        ' Check the updates location
        If My.Computer.FileSystem.DirectoryExists(updateFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(updateFolder)
        Else
            ' Clear the existing contents of .tmp files
            For Each File As String In My.Computer.FileSystem.GetFiles(updateFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.tmp")
                My.Computer.FileSystem.DeleteFile(File)
            Next
        End If

        Dim UpdateXML As XmlDocument = FetchUpdateXML()
        If UpdateXML Is Nothing Then
            lblUpdateStatus.Text = "Status: Unable to obtain update file."
            btnRecheckUpdates.Enabled = True
            Exit Sub
        Else
            updateRequired = False

            ' Get a current list of components
            lblUpdateStatus.Text = "Status: Checking current components..."
            adtUpdates.Nodes.Clear()
            EveHQComponents.Clear()
            PBControls.Clear()
            PDBControls.Clear()
            For Each myAssembly As AssemblyName In Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                If EveHQComponents.ContainsKey(myAssembly.Name) = False Then
                    If myAssembly.Name.StartsWith("EveHQ") Then
                        EveHQComponents.Add(myAssembly.Name & ".dll", New EveHQComponent(myAssembly.Name & ".dll", myAssembly.Version.ToString, True))
                    ElseIf myAssembly.Name.StartsWith("DevComponents") Then
                        EveHQComponents.Add(myAssembly.Name & ".dll", New EveHQComponent(myAssembly.Name & ".dll", myAssembly.Version.ToString, False))
                    End If
                End If
            Next

            ' Add to that a list of the plug-ins used
            For Each myPlugIn As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
                If myPlugIn.ShortFileName IsNot Nothing Then
                    If EveHQComponents.ContainsKey(myPlugIn.ShortFileName) = False Then
                        EveHQComponents.Add(myPlugIn.ShortFileName, New EveHQComponent(myPlugIn.ShortFileName, myPlugIn.Version, True))
                    End If
                End If
            Next

            ' Add the current executable!
            EveHQComponents.Add("EveHQ.exe", New EveHQComponent("EveHQ.exe", My.Application.Info.Version.ToString, True))

            ' Try and add the database version (if using SQL CE)
            If EveHQ.Core.HQ.EveHQSettings.DBFormat = 0 Then
                Dim databaseData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM EveHQVersion;")
                If databaseData IsNot Nothing Then
                    If databaseData.Tables(0).Rows.Count > 0 Then
                        EveHQComponents.Add("EveHQ.sdf.zip", New EveHQComponent("EveHQ.sdf.zip", databaseData.Tables(0).Rows(0).Item("Version").ToString, False))
                    Else
                        EveHQComponents.Add("EveHQ.sdf.zip", New EveHQComponent("EveHQ.sdf.zip", "1.0.0.0", False))
                    End If
                Else
                    EveHQComponents.Add("EveHQ.sdf.zip", New EveHQComponent("EveHQ.sdf.zip", "1.0.0.0", False))
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
                If updateRequired = True Then
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
        Dim newFile As New Node
        newFile.Text = updateFile.ChildNodes(0).InnerText
        newFile.Name = updateFile.ChildNodes(0).InnerText
        ' Add node with text
        adtUpdates.Nodes.Add(newFile)
        ' Add the function from the update file
        newFile.Cells.Add(New Cell(updateFile.ChildNodes(1).InnerText, adtUpdates.Styles("Centre")))
        ' Add the current version
        If EveHQComponents.ContainsKey(updateFile.ChildNodes(0).InnerText) = True Then
            newFile.Cells.Add(New Cell(EveHQComponents(updateFile.ChildNodes(0).InnerText).Version, adtUpdates.Styles("Centre")))
        Else
            newFile.Cells.Add(New Cell("Unknown", adtUpdates.Styles("Centre")))
        End If
        ' Add the version available
        newFile.Cells.Add(New Cell(updateFile.ChildNodes(2).InnerText, adtUpdates.Styles("Centre")))
        ' Check if the plug-in is available
        If EveHQComponents.ContainsKey(updateFile.ChildNodes(0).InnerText) = True Then
            ' Check which is the later version
            If IsUpdateAvailable(EveHQComponents(updateFile.ChildNodes(0).InnerText).Version, updateFile.ChildNodes(2).InnerText) = True Then
                newFile.Style = UpdateRequiredStyle
                Dim chkDownload As New DevComponents.DotNetBar.CheckBoxItem
                chkDownload.Name = newFile.Text
                If newFile.Cells(1).Text = "Required" Then
                    chkDownload.Enabled = False
                Else
                    chkDownload.Enabled = True
                End If
                chkDownload.Checked = True
                newFile.Cells.Add(New Cell(""))
                chkDownload.CheckBoxPosition = eCheckBoxPosition.Top
                newFile.Cells(4).HostedItem = chkDownload
                Dim pbProgress As New ProgressBarItem
                pbProgress.Name = newFile.Text
                pbProgress.TextVisible = True
                pbProgress.Text = "0%"
                pbProgress.Width = 120
                newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                newFile.Cells(5).HostedItem = pbProgress
                PBControls.Add(newFile.Text, pbProgress)
                ' Check for a PDB file
                If updateFile.ChildNodes(3).InnerText = "True" Then
                    Dim pdbProgress As New ProgressBarItem
                    pdbProgress.Name = newFile.Text
                    pdbProgress.TextVisible = True
                    pdbProgress.Text = "0%"
                    pdbProgress.Width = 120
                    newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                    newFile.Cells(6).HostedItem = pdbProgress
                    PDBControls.Add(newFile.Text.Remove(newFile.Text.Length - 4, 4) & ".pdb", pdbProgress)
                Else
                    newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                    newFile.Cells(6).Text = "PDB File Not Required"
                End If
                updateRequired = True
            Else
                Dim chkDownload As New DevComponents.DotNetBar.CheckBoxItem
                chkDownload.Name = newFile.Text
                chkDownload.Enabled = False
                chkDownload.Checked = False
                newFile.Cells.Add(New Cell(""))
                chkDownload.CheckBoxPosition = eCheckBoxPosition.Top
                newFile.Cells(4).HostedItem = chkDownload
                newFile.Style = UpdateNotRequiredStyle
                newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                newFile.Cells(5).Text = "Update Not Required"
                newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                newFile.Cells(6).Text = "Update Not Required"
            End If
        Else
            If newFile.Text <> "EveHQ.sdf.zip" Or (newFile.Text = "EveHQ.sdf.zip" And EveHQ.Core.HQ.EveHQSettings.DBFormat = 0) Then
                newFile.Cells(2).Text = "New!!"
                newFile.Style = UpdateRequiredStyle
                Dim chkDownload As New DevComponents.DotNetBar.CheckBoxItem
                chkDownload.Name = newFile.Text
                chkDownload.Enabled = True
                chkDownload.Checked = True
                newFile.Cells.Add(New Cell(""))
                chkDownload.CheckBoxPosition = eCheckBoxPosition.Top
                newFile.Cells(4).HostedItem = chkDownload
                Dim pbProgress As New ProgressBarItem
                pbProgress.Name = newFile.Text
                pbProgress.TextVisible = True
                pbProgress.Text = "0%"
                pbProgress.Width = 120
                newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                newFile.Cells(5).HostedItem = pbProgress
                PBControls.Add(newFile.Text, pbProgress)
                ' Check for a PDB file
                If updateFile.ChildNodes(3).InnerText = "True" Then
                    Dim pdbProgress As New ProgressBarItem
                    pdbProgress.Name = newFile.Text
                    pdbProgress.TextVisible = True
                    pdbProgress.Text = "0%"
                    pdbProgress.Width = 120
                    newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                    newFile.Cells(6).HostedItem = pdbProgress
                    PDBControls.Add(newFile.Text.Remove(newFile.Text.Length - 4, 4) & ".pdb", pdbProgress)
                Else
                    newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                    newFile.Cells(6).Text = "Update Not Required"
                End If
                updateRequired = True
            Else
                newFile.Cells(2).Text = "Using SQL"
                Dim chkDownload As New DevComponents.DotNetBar.CheckBoxItem
                chkDownload.Name = newFile.Text
                chkDownload.Enabled = False
                chkDownload.Checked = False
                newFile.Cells.Add(New Cell(""))
                chkDownload.CheckBoxPosition = eCheckBoxPosition.Top
                newFile.Cells(4).HostedItem = chkDownload
                newFile.Style = UpdateNotRequiredStyle
                newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                newFile.Cells(5).Text = "Update Not Required"
                newFile.Cells.Add(New Cell("", adtUpdates.Styles("Centre")))
                newFile.Cells(6).Text = "Update Not Required"
            End If
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

#End Region

    Private Sub btnStartUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartUpdate.Click

        btnStartUpdate.Enabled = False
        btnCancelUpdate.Visible = True
        EveHQ.Core.HQ.EveHQIsUpdating = True
        filesTried = 0

        ' Reset the progress bars
        For Each pb As ProgressBarItem In pbControls.Values
            pb.Minimum = 0
            pb.Maximum = 100
            pb.Value = 0
            pb.Text = pb.Value.ToString & "%"
        Next
        For Each pb As ProgressBarItem In PDBControls.Values
            pb.Minimum = 0
            pb.Maximum = 100
            pb.Value = 0
            pb.Text = pb.Value.ToString & "%"
        Next

        ' Check which files we are downloading (those that are checked)
        filesRequired.Clear()
        filesComplete.Clear()
        For Each item As Node In adtUpdates.Nodes
            If CType(item.Cells(4).HostedItem, CheckBoxItem).Checked = True Then
                ' Add the main file
                FilesRequired.Enqueue(item.Text)
                ' Check for a PDB file
                If item.Cells(6).HostedItem IsNot Nothing Then
                    FilesRequired.Enqueue(item.Text.Remove(item.Text.Length - 4, 4) & ".pdb")
                End If
            Else
                item.Cells(5).HostedItem = Nothing
                item.Cells(5).Text = "Not selected for download"
            End If
        Next

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
                EveHQ.Core.HQ.EveHQIsUpdating = False
                Exit Sub
            End Try
        End Try

        lblUpdateStatus.Text = "Status: Downloading updates..."
        MainUpdateWorker.WorkerReportsProgress = True
        MainUpdateWorker.WorkerSupportsCancellation = True
        MainUpdateWorker.RunWorkerAsync()

    End Sub

    Private Sub MainUpdateWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MainUpdateWorker.DoWork
        UpdateWorkerList.Clear()
        UpdateQueue.Clear()
        NumberOfFilesRequired = FilesRequired.Count
        Do
            ' Check if the queue is full
            If UpdateQueue.Count < EveHQ.Core.HQ.EveHQSettings.MaxUpdateThreads Then
                ' Get a file from the queue
                Dim reqfile As String = FilesRequired.Dequeue
                UpdateQueue.Add(reqfile)
                Dim updateWorker As New System.ComponentModel.BackgroundWorker
                AddHandler updateWorker.DoWork, AddressOf UpdateWorker_DoWork
                AddHandler updateWorker.ProgressChanged, AddressOf UpdateWorker_ProgressChanged
                AddHandler updateWorker.RunWorkerCompleted, AddressOf UpdateWorker_RunWorkerCompleted
                updateWorker.WorkerReportsProgress = True
                updateWorker.WorkerSupportsCancellation = True
                UpdateWorkerList.Add(reqfile, updateWorker)
                updateWorker.RunWorkerAsync(reqfile)
            End If
            If MainUpdateWorker.CancellationPending = True Then
                Exit Do
            End If
            Application.DoEvents()
        Loop Until FilesRequired.Count = 0
    End Sub

    Private Function DownloadFile(ByVal worker As System.ComponentModel.BackgroundWorker, ByVal FileNeeded As String) As Boolean

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        Dim httpURI As String = ""
        Dim pdbFile As String = ""
        Dim localFile As String = ""
        httpURI = EveHQ.Core.HQ.EveHQSettings.UpdateURL & "/" & FileNeeded
        localFile = Path.Combine(updateFolder, FileNeeded & ".tmp")

        ' Create the request to access the server and set credentials
        ServicePointManager.DefaultConnectionLimit = 10
        ServicePointManager.Expect100Continue = False
        Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(httpURI))
        Dim request As HttpWebRequest = CType(HttpWebRequest.Create(httpURI), HttpWebRequest)
        request.CachePolicy = policy
        ' Setup proxy server (if required)
        Call EveHQ.Core.ProxyServerFunctions.SetupWebProxy(request)
        request.UserAgent = "EveHQ Updater " & My.Application.Info.Version.ToString
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
                            If worker.CancellationPending = True Then
                                responseStream.Close()
                                fs.Flush()
                                fs.Close()
                                UpdateWorkerList.Remove(FileNeeded)
                                Exit Function
                            End If
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
            ' Rename the file now it's complete
            My.Computer.FileSystem.RenameFile(localFile, FileNeeded)
            filesComplete.Add(FileNeeded, True)
            UpdateQueue.Remove(FileNeeded)
            filesTried += 1
            UpdateWorkerList.Remove(FileNeeded)
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
        Call DownloadFile(CType(sender, System.ComponentModel.BackgroundWorker), reqfile)
    End Sub

    Private Sub UpdateWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)
        Dim fileName As String = e.UserState.ToString
        If fileName.EndsWith(".pdb") Then
            Dim pb As ProgressBarItem = CType(PDBControls(fileName), ProgressBarItem)
            pb.Value = e.ProgressPercentage
            pb.Text = pb.Value.ToString & "%"
            pb.Refresh()
        Else
            Dim pb As ProgressBarItem = CType(PBControls(fileName), ProgressBarItem)
            pb.Value = e.ProgressPercentage
            pb.Text = pb.Value.ToString & "%"
            pb.Refresh()
        End If
    End Sub

    Private Sub UpdateWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        For Each item As Node In adtUpdates.Nodes
            ' Check main file
            If filesComplete.Contains(item.Text) = True Then
                If CBool(filesComplete.Item(item.Text)) = True Then
                    item.Style = UpdateNotRequiredStyle
                    item.Cells(5).HostedItem.Text = "Download Complete!" : item.Cells(5).HostedItem.Refresh()
                    CType(item.Cells(5).HostedItem, ProgressBarItem).ColorTable = eProgressBarItemColor.Normal
                Else
                    item.Style = UpdateRequiredStyle
                    item.Cells(5).HostedItem.Text = "Download Failed!" : item.Cells(5).HostedItem.Refresh()
                    CType(item.Cells(5).HostedItem, ProgressBarItem).ColorTable = eProgressBarItemColor.Error
                End If
            End If
            ' Check PDB
            If filesComplete.Contains(item.Text.Remove(item.Text.Length - 4, 4) & ".pdb") = True Then
                If CBool(filesComplete.Item(item.Text.Remove(item.Text.Length - 4, 4) & ".pdb")) = True Then
                    item.Style = UpdateNotRequiredStyle
                    item.Cells(6).HostedItem.Text = "Download Complete!" : item.Cells(6).HostedItem.Refresh()
                    CType(item.Cells(6).HostedItem, ProgressBarItem).ColorTable = eProgressBarItemColor.Normal
                Else
                    item.Style = UpdateRequiredStyle
                    item.Cells(6).HostedItem.Text = "Download Failed!" : item.Cells(6).HostedItem.Refresh()
                    CType(item.Cells(6).HostedItem, ProgressBarItem).ColorTable = eProgressBarItemColor.Error
                End If
            End If
        Next
        ' Remove event handlers
        Dim updateWorker As System.ComponentModel.BackgroundWorker = CType(sender, System.ComponentModel.BackgroundWorker)
        RemoveHandler updateWorker.DoWork, AddressOf UpdateWorker_DoWork
        RemoveHandler updateWorker.ProgressChanged, AddressOf UpdateWorker_ProgressChanged
        RemoveHandler updateWorker.RunWorkerCompleted, AddressOf UpdateWorker_RunWorkerCompleted

        If filesTried = NumberOfFilesRequired Then
            If filesComplete.Count = NumberOfFilesRequired Then
                lblUpdateStatus.Text = "Status: Download complete!"
                ' Ask the user if they want to update now
                Dim msg As String = "The download process is complete. Would you like to close and update EveHQ now?"
                If MessageBox.Show(msg, "Update Now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    ' Activate the update menu
                    EveHQ.Core.HQ.AppUpdateAvailable = True
                    frmEveHQ.btnUpdateEveHQ.Enabled = True
                    EveHQ.Core.HQ.EveHQIsUpdating = False
                    Me.Close()
                Else
                    ' Try backup here?
                    If EveHQ.Core.HQ.EveHQSettings.BackupBeforeUpdate = True Then
                        EveHQ.Core.HQ.WriteLogEvent("Shutdown: Request to backup EveHQ Settings before update")
                        Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()
                        Call EveHQ.Core.EveHQBackup.BackupEveHQSettings()
                    End If
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
                    EveHQ.Core.HQ.EveHQIsUpdating = False
                    EveHQ.Core.HQ.UpdateShutDownRequest = True
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
        Call EveHQ.Core.ProxyServerFunctions.SetupWebProxy(request)
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

    Private Sub nudDownloads_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudDownloads.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.MaxUpdateThreads = nudDownloads.Value
    End Sub

    Private Sub btnCancelUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnCancelUpdate.Click
        UpdateAborted = True
        ' Cancel the main worker
        MainUpdateWorker.CancelAsync()
        ' Cancel all the other workers
        For Each worker As BackgroundWorker In UpdateWorkerList.Values
            worker.CancelAsync()
        Next
        ' Await cancellation
        Do
            Application.DoEvents()
        Loop Until UpdateWorkerList.Count = 0
        MessageBox.Show("The EveHQ Update has been cancelled.", "Update Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        EveHQ.Core.HQ.EveHQIsUpdating = False
        btnCancelUpdate.Visible = False
        Call Me.ShowUpdates()
    End Sub

End Class

Public Class EveHQComponent
    Public Name As String = ""
    Public Version As String = ""
    Public HasPDB As Boolean = False

    Public Sub New()
        Name = ""
        Version = ""
        HasPDB = False
    End Sub

    Public Sub New(cName As String, cVersion As String, cHasPDB As Boolean)
        Name = cName
        Version = cVersion
        HasPDB = cHasPDB
    End Sub

End Class