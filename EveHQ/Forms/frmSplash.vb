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
Imports System.Reflection
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports ICSharpCode.SharpZipLib.Zip

Public Class frmSplash

    Dim isLocal As Boolean = False
    Dim showSplash As Boolean = True
    Dim showSettings As Boolean = False
    Dim PlugInLoading As New SortedList(Of String, String)

    Private Sub frmSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Insert the version number to the splash screen
        lblVersion.Text = "Version " & My.Application.Info.Version.ToString
        lblCopyright.Text = My.Application.Info.Copyright
        lblDate.Text = My.Application.Info.Trademark

        ' Set the image for the splash screen
        'Dim r As New Random
        'Dim img As Integer = r.Next(1, 6)
        'Panel1.BackgroundImage = CType(My.Resources.ResourceManager.GetObject("Splashv" & img.ToString), Image)

        ' Check for any commandline parameters that we need to account for
        For Each param As String In System.Environment.GetCommandLineArgs
            'MessageBox.Show(param)
            If param = "/wait" Then
                Threading.Thread.Sleep(2000)
            End If
            If param = "/local" Then
                isLocal = True
                EveHQ.Core.HQ.IsUsingLocalFolders = True
            End If
            If param = "/nosplash" Then
                showSplash = False
                EveHQ.Core.HQ.IsSplashFormDisabled = True
            End If
            If param = "/settings" Then
                showSettings = True
            End If
            If param.StartsWith(EveHQ.Core.HQ.FittingProtocol) Then
                PlugInLoading.Add("EveHQ Fitter", param)
            End If
        Next

        If showSplash = True And showSettings = False Then
            Me.Show()
        End If

        ' Delete any .old files left over from the last update
        Try
            For Each newFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.old")
                Dim nfi As New IO.FileInfo(newFile)
                My.Computer.FileSystem.DeleteFile(nfi.FullName)
            Next
        Catch ex As Exception
            MessageBox.Show("Unable to delete update files, please delete any .old files manually that exist in the installation folder.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

        ' Set the application folder
        lblStatus.Text = "> Setting application directory..."
        Me.Refresh()
        EveHQ.Core.HQ.appFolder = Application.StartupPath

        ' Check for existence of an application folder in the application directory
        If isLocal = False Then
            lblStatus.Text = "> Checking app data directory..."
            Me.Refresh()
            EveHQ.Core.HQ.appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveHQ")
            If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.appDataFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.appDataFolder)
            End If
        Else
            EveHQ.Core.HQ.appDataFolder = EveHQ.Core.HQ.appFolder
        End If

        ' Check for existence of a cache folder in the application directory
        lblStatus.Text = "> Checking cache directory..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.cacheFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "Cache")
        Else
            EveHQ.Core.HQ.cacheFolder = Path.Combine(Application.StartupPath, "Cache")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.cacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.cacheFolder)
        End If

        ' Check for existence of a cache folder in the application directory
        lblStatus.Text = "> Checking image cache directory..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.imageCacheFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "ImageCache")
        Else
            EveHQ.Core.HQ.imageCacheFolder = Path.Combine(Application.StartupPath, "ImageCache")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.imageCacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.imageCacheFolder)
        End If

        ' Check for existence of a report folder in the application directory
        lblStatus.Text = "> Checking report folder..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.reportFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EveHQ")
            EveHQ.Core.HQ.reportFolder = Path.Combine(EveHQ.Core.HQ.reportFolder, "Reports")
        Else
            EveHQ.Core.HQ.reportFolder = Path.Combine(Application.StartupPath, "Reports")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.reportFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.reportFolder)
        End If

        ' Check for existence of a data folder in the application directory
        lblStatus.Text = "> Checking data directory..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.dataFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "Data")
        Else
            EveHQ.Core.HQ.dataFolder = Path.Combine(Application.StartupPath, "Data")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.dataFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.dataFolder)
        End If

        ' Check for existence of a backup folder
        lblStatus.Text = "> Checking backup directory..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.backupFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EveHQ")
            EveHQ.Core.HQ.backupFolder = Path.Combine(EveHQ.Core.HQ.backupFolder, "Backups")
        Else
            EveHQ.Core.HQ.backupFolder = Path.Combine(Application.StartupPath, "Backups")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.backupFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.backupFolder)
        End If

        ' Check for existence of an EveHQ backup folder
        lblStatus.Text = "> Checking EveHQ backup directory..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.EveHQBackupFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EveHQ")
            EveHQ.Core.HQ.EveHQBackupFolder = Path.Combine(EveHQ.Core.HQ.EveHQBackupFolder, "EveHQBackups")
        Else
            EveHQ.Core.HQ.EveHQBackupFolder = Path.Combine(Application.StartupPath, "EveHQBackups")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.EveHQBackupFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.EveHQBackupFolder)
        End If

        If showSettings = True Then
            EveHQ.Core.EveHQSettingsFunctions.LoadSettings()
            frmSettings.ShowDialog()
            ' Remove the icons
            frmEveHQ.EveStatusIcon.Visible = False : frmEveHQ.iconEveHQMLW.Visible = False
            frmEveHQ.EveStatusIcon.Icon = Nothing : frmEveHQ.iconEveHQMLW.Icon = Nothing
            frmEveHQ.EveStatusIcon.Dispose() : frmEveHQ.iconEveHQMLW.Dispose()
            End
        End If

        ' Load user settings - this is needed to work out data connection type & update requirements
        lblStatus.Text = "> Loading settings..."
        Me.Refresh()
        Do While EveHQ.Core.EveHQSettingsFunctions.LoadSettings() = False
            ' Ask if we want to check for a database
            Dim msg As String = "EveHQ was unable to load data from a Database." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "If you do not select a valid Database, EveHQ will exit." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Would you like to select a Database now?" & ControlChars.CrLf
            Dim reply As Integer = MessageBox.Show(msg, "Database Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.No Then
                End
            End If
            frmSettings.ShowDialog()
        Loop

        ' Determine the visual style
        If EveHQ.Core.HQ.EveHQSettings.DisableVisualStyles = True Then
            Application.VisualStyleState = VisualStyles.VisualStyleState.NoneEnabled
        Else
            Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled
        End If

        ' Start the G15 if applicable
        If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True Then
            'Init the LCD
            EveHQ.Core.HQ.EveHQLCD.InitLCD("EveHQ LCD Display")
            If EveHQ.Core.HQ.IsG15LCDActive = False Then
                MessageBox.Show("Unable to start G15 Display. Please ensure you have the keyboard and drivers correctly installed.", "Error Starting G15", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                'With the LCD initialised, draw the opening screen
                EveHQ.Core.HQ.EveHQLCD.DrawIntroScreen()
                ' Check if the LCD will cycle chars
                If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
                    EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.EveHQSettings.CycleG15Time)
                    EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Enabled = True
                End If
            End If
        End If

        ' Force DBDataDirectory location if using Access
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = 0 And EveHQ.Core.HQ.EveHQSettings.DBDataFilename <> Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.mdb") Then
            Dim oldLocation As String = EveHQ.Core.HQ.EveHQSettings.DBDataFilename
            Dim newLocation As String = Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.mdb")
            If My.Computer.FileSystem.FileExists(oldLocation) = True Then
                ' Attempt to copy to the new location
                Try
                    Dim msg As String = "Attempting to copy the custom database from " & ControlChars.CrLf & oldLocation & " to " & ControlChars.CrLf & newLocation & "."
                    MessageBox.Show(msg, "Relocating Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    My.Computer.FileSystem.CopyFile(oldLocation, newLocation)
                Catch ex As Exception
                    Dim msg As String = "Unable to copy the custom database from " & ControlChars.CrLf & oldLocation & " to " & ControlChars.CrLf & newLocation & ". This will need to be copied manually."
                    MessageBox.Show(msg, "Unable to Copy Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
                MessageBox.Show("Copying successful. Attempting to delete the old file at " & ControlChars.CrLf & oldLocation, "Copy Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Try
                    My.Computer.FileSystem.DeleteFile(oldLocation)
                Catch ex As Exception
                    Dim msg As String = "Unable to delete the old custom database from " & ControlChars.CrLf & oldLocation & ". This will need to be deleted manually if required."
                    MessageBox.Show(msg, "Unable to Delete Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End If
        End If
        EveHQ.Core.HQ.EveHQSettings.DBDataFilename = Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.mdb")
        Call EveHQ.Core.DataFunctions.SetEveHQDataConnectionString()

        ' Check for new database
        lblStatus.Text = "> Checking custom data..."
        Me.Refresh()
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0
                If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.DBDataFilename) = False Then
                    ' Looks like it hasn't been set so let's create it - but inform the user
                    Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
                    msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
                    msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
                    MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                        MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    ' Looks like the file exists, but let's test a connection to make sure 
                    If TestDataDBConnection() = False Then
                        ' Looks like it fails so let's recreate it - but inform the user
                        Dim msg As String = "EveHQ has detected that although the the new storage database is initialised, it cannot be located." & ControlChars.CrLf
                        msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
                        msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to create the new database."
                        MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                            MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Else
                            MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
            Case Else
                If EveHQ.Core.HQ.EveHQSettings.DBDataName = "" Then
                    ' Looks like it hasn't been set so let's create it - but inform the user
                    Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
                    msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
                    msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
                    MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                        MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    ' Looks like the file exists, but let's test a connection to make sure 
                    If TestDataDBConnection() = False Then
                        ' Looks like it fails so let's recreate it - but inform the user
                        Dim msg As String = "EveHQ has detected that although the the new storage database is initialised, it cannot be located." & ControlChars.CrLf
                        msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
                        msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to create the new database."
                        MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                            MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Else
                            MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
        End Select

        ' Load Certificate data
        lblStatus.Text = "> Loading Certificate data..."
        Me.Refresh()
        Dim s As New MemoryStream(My.Resources.CertificateCategories)
        Dim f As BinaryFormatter = New BinaryFormatter
        EveHQ.Core.HQ.CertificateCategories = CType(f.Deserialize(s), SortedList)
        s.Close()
        s = New MemoryStream(My.Resources.CertificateClasses)
        f = New BinaryFormatter
        EveHQ.Core.HQ.CertificateClasses = CType(f.Deserialize(s), SortedList)
        s.Close()
        s = New MemoryStream(My.Resources.Certificates)
        f = New BinaryFormatter
        EveHQ.Core.HQ.Certificates = CType(f.Deserialize(s), SortedList)
        s.Close()

        ' Load skill data and item data
        lblStatus.Text = "> Loading skills && items..."
        Me.Refresh()
        Do While EveHQ.Core.DataFunctions.LoadItems = False
            Dim msg As String = "EveHQ was unable to load data from a Database." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "If you do not select a valid Database, EveHQ will exit." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Would you like to select a Database now?" & ControlChars.CrLf
            Dim reply As Integer = MessageBox.Show(msg, "Database Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.No Then
                End
            End If
            frmSettings.ShowDialog()
        Loop

        ' If we get this far we have loaded a DB so check for SQL format and check the custom data
        lblStatus.Text = "> Checking database..."
        Me.Refresh()
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = 1 Or EveHQ.Core.HQ.EveHQSettings.DBFormat = 2 Then
            Dim strSQL As String = "SELECT attributeGroup FROM dgmAttributeTypes"
            Dim testData As Data.DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If testData Is Nothing Then
                ' We seem to be missing the data so lets add it in!
                lblStatus.Text = "> Customising MSSQL database..."
                Me.Refresh()
                Dim conn As New Data.SqlClient.SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.itemDBConnectionString
                conn.Open()
                Call EveHQ.Core.DataFunctions.AddSQLAttributeGroupColumn(conn)
                Call EveHQ.Core.DataFunctions.CorrectSQLEveUnits(conn)
                Call EveHQ.Core.DataFunctions.DoSQLQuery(conn)
                If conn.State = Data.ConnectionState.Open Then
                    conn.Close()
                End If
            End If
        End If

        ' Check for modules
        lblStatus.Text = "> Loading modules..."
        Me.Refresh()
        Call LoadModules()

        'Set the servers to their server details
        lblStatus.Text = "> Setting Eve Server details..."
        Me.Refresh()
        EveHQ.Core.HQ.myTQServer.Server = 0

        ' Update the pilot account info
        If EveHQ.Core.PilotParseFunctions.LoadKeySkills() = False Then
            Dim msg As String = "There was an error parsing your character skill data. This will be reset. Please connect to the API to download the latest data."
            MessageBox.Show(msg, "Error Parsing Pilot Skills", MessageBoxButtons.OK, MessageBoxIcon.Information)
            For Each rPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                rPilot.PilotSkills = New Collection
                rPilot.SkillPoints = 0
            Next
        End If

        Call EveHQ.Core.PilotParseFunctions.CheckMissingTrainingSkills()
        Call frmSettings.UpdateAccounts()
        Call frmEveHQ.UpdatePilotInfo(True)

        ' Load the API Errors
        Dim ErrorXML As New Xml.XmlDocument
        ErrorXML.LoadXml(My.Resources.Errors)
        Dim ErrList As Xml.XmlNodeList = ErrorXML.SelectNodes("/eveapi/result/rowset/row")
        If ErrList.Count <> 0 Then
            EveHQ.Core.HQ.APIErrors.Clear()
            For Each ErrNode As Xml.XmlNode In ErrList
                EveHQ.Core.HQ.APIErrors.Add(ErrNode.Attributes.GetNamedItem("errorCode").Value, ErrNode.Attributes.GetNamedItem("errorText").Value)
            Next
        End If

        ' Check if we need to start the market watcher
        If EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcherAtStartup = True Then
            If frmEveHQ.InitialiseWatchers() = True Then
                EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcher = True
            Else
                MessageBox.Show("Unable to start Market Log Watcher. Please check Eve is installed and the market log export folder exists.", "Error Starting Watcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
                EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcher = False
            End If
        Else
            EveHQ.Core.HQ.EveHQSettings.EnableMarketLogWatcher = False
        End If

        ' Show the main form
        lblStatus.Text = "> Initialising EveHQ..."
        Me.Refresh()
        EveHQ.Core.HQ.MainForm = frmEveHQ
        frmEveHQ.Show()

        ' Activate the lcdPilot
        EveHQ.Core.HQ.lcdPilot = EveHQ.Core.HQ.EveHQSettings.StartupPilot

    End Sub

    Private Sub LoadModules()
        For Each filename As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.dll")
            If IsDotNetAssembly(filename) = True Then
                Try
                    Dim myAssembly As Assembly = Assembly.LoadFrom(filename)
                    Dim types() As Type = myAssembly.GetTypes
                    For Each t As Type In types
                        If t.IsPublic = True Then
                            If t.GetInterface("EveHQ.Core.IEveHQPlugIn") IsNot Nothing Then
                                Dim myPlugIn As EveHQ.Core.IEveHQPlugIn = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
                                Dim EveHQPlugIn As EveHQ.Core.PlugIn = myPlugIn.GetEveHQPlugInInfo
                                EveHQPlugIn.FileName = filename
                                Dim fi As New IO.FileInfo(filename)
                                EveHQPlugIn.ShortFileName = fi.Name
                                EveHQPlugIn.FileType = t.FullName
                                EveHQPlugIn.Version = myAssembly.GetName.Version.ToString
                                EveHQPlugIn.Instance = myPlugIn
                                ' Get status of plug-ins from settings (should already exist!)
                                If EveHQ.Core.HQ.EveHQSettings.Plugins.Contains(EveHQPlugIn.Name) = True Then
                                    Dim oldPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(EveHQPlugIn.Name), Core.PlugIn)
                                    EveHQPlugIn.Disabled = oldPlugIn.Disabled
                                    EveHQPlugIn.Available = True
                                    EveHQ.Core.HQ.EveHQSettings.Plugins.Remove(EveHQPlugIn.Name)
                                Else
                                    ' If not listed, it must be new
                                    EveHQPlugIn.Disabled = False
                                    EveHQPlugIn.Available = True
                                End If
                                ' Check for opening parameters
                                If PlugInLoading.ContainsKey(EveHQPlugIn.Name) = True Then
                                    EveHQPlugIn.PostStartupData = PlugInLoading(EveHQPlugIn.Name)
                                End If
                                EveHQPlugIn.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                                EveHQ.Core.HQ.EveHQSettings.Plugins.Add(EveHQPlugIn.Name, EveHQPlugIn)
                            End If
                        End If
                    Next
                    types = Nothing
                    myAssembly = Nothing
                Catch bife As BadImageFormatException
                    'Ignore non .Net dlls (ones without manifests i.e. the G15 lglcd.dll) i.e. don't error
                Catch rtle As ReflectionTypeLoadException
                    ' Assume it's a bad/old version and ignore it
                Catch e As Exception
                    MessageBox.Show("Error loading module: " & filename & ControlChars.CrLf & e.Message.ToString, "Module Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
            End If
        Next
    End Sub

    Private Function IsDotNetAssembly(ByVal fileName As String) As Boolean
        'private bool IsDotNetAssembly(string fileName)
        Using fs As New FileStream(fileName, FileMode.Open, FileAccess.Read)
            Try
                Using br As New BinaryReader(fs)
                    Try
                        fs.Position = &H3C ' PE Header start offset
                        Dim headerOffset As UInteger = br.ReadUInt32
                        fs.Position = headerOffset + &H18
                        Dim magicNumber As UInt16 = br.ReadUInt16()
                        Dim dictionaryOffset As Integer
                        Select Case magicNumber
                            Case &H10B ' 32 bit
                                dictionaryOffset = &H60
                            Case &H20B ' 64 bit
                                dictionaryOffset = &H70
                            Case Else
                                Throw New Exception("Invalid Image Format")
                        End Select
                        ' Position to RVA 15
                        fs.Position = headerOffset + &H18 + dictionaryOffset + &H70
                        ' Read the value
                        Dim rva15value As UInt32 = br.ReadUInt32()
                        Return rva15value <> 0
                    Catch ex As Exception
                    Finally
                        br.Close()
                    End Try
                End Using
            Catch ex As Exception
            Finally
                fs.Close()
            End Try
        End Using
    End Function

    Private Function TestDataDBConnection() As Boolean
        Dim strConnection As String = ""
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0
                strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & EveHQ.Core.HQ.EveHQSettings.DBDataFilename & ";"
                Dim connection As New Data.OleDb.OleDbConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            Case 1
                strConnection = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName & "; Integrated Security = SSPI;"
                End If
                Dim connection As New Data.SqlClient.SqlConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            Case 2
                strConnection = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & "\SQLEXPRESS"
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBDataName & "; Integrated Security = SSPI;"
                End If
                Dim connection As New Data.SqlClient.SqlConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    Return True
                Catch ex As Exception
                    Return False
                End Try
        End Select
    End Function

End Class
