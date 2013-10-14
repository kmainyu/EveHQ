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
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports System.Xml
Imports EveHQ.Common.Logging

Namespace Forms

    Public Class frmSplash

        Dim _isLocal As Boolean = False
        Dim _showSplash As Boolean = True
        Dim _showSettings As Boolean = False
        ReadOnly _plugInLoading As New SortedList(Of String, String)
        ReadOnly _culture As CultureInfo = New CultureInfo("en-GB")
        Dim _pluginsLoaded As Boolean = False
        Dim _widgetsLoaded As Boolean = False
        Dim _itemsLoaded As Boolean = False
        Dim _messageLoaded As Boolean = False

        Private Sub frmSplash_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            ' Force primary thread into UK english
            Application.CurrentCulture = CultureInfo.GetCultureInfo("en-GB")
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-GB")
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-GB")

            ' Check for any commandline parameters that we need to account for
            For Each param As String In Environment.GetCommandLineArgs
                If param = "/wait" Then
                    Thread.Sleep(2000)
                End If
                If param = "/local" Then
                    _isLocal = True
                    Core.HQ.IsUsingLocalFolders = True
                End If
                If param = "/nosplash" Then
                    _showSplash = False
                    Core.HQ.IsSplashFormDisabled = True
                End If
                If param = "/settings" Then
                    _showSettings = True
                End If
                If param.StartsWith(Core.HQ.FittingProtocol) Then
                    _plugInLoading.Add("EveHQ Fitter", param)
                End If
            Next

            ' Set the application folder
            lblStatus.Text = "> Setting application directory..."
            lblStatus.Refresh()
            Core.HQ.appFolder = Application.StartupPath

            ' Check for existence of an application folder in the application directory
            If _isLocal = False Then
                lblStatus.Text = "> Checking app data directory..."
                lblStatus.Refresh()
                Core.HQ.AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveHQ")
                If My.Computer.FileSystem.DirectoryExists(Core.HQ.AppDataFolder) = False Then
                    ' Create the cache folder if it doesn't exist
                    My.Computer.FileSystem.CreateDirectory(Core.HQ.AppDataFolder)
                End If
            Else
                Core.HQ.AppDataFolder = Core.HQ.appFolder
            End If

            ' Configure trace listener
            Core.HQ.LoggingStream = New FileStream(Path.Combine(Core.HQ.AppDataFolder, "EveHQ.log"), FileMode.Create, FileAccess.Write, FileShare.Read)
            Core.HQ.EveHqTracer = New EveHQTraceLogger(Core.HQ.LoggingStream)
            Trace.Listeners.Add(Core.HQ.EveHqTracer)
            Core.HQ.EveHQLogTimer.Start()
            Core.HQ.WriteLogEvent("Start of EveHQ Event Log: " & Now.ToString)
            If Stopwatch.IsHighResolution Then
                Core.HQ.WriteLogEvent("Operations timed using the system's high-resolution performance counter.")
            Else
                Core.HQ.WriteLogEvent("Operations timed using the DateTime class.")
            End If
            Core.HQ.WriteLogEvent("***** Start: EveHQ Startup Routine via Splash Screen *****")

            ' Show the settings form only, then quit
            If _showSettings = True Then
                Dim failedShowSettings As Boolean = Core.EveHQSettings.Load(True)
                If failedShowSettings = True Then
                    frmSettings.ShowDialog()
                    ' Remove the icons
                    frmEveHQ.EveStatusIcon.Visible = False : frmEveHQ.iconEveHQMLW.Visible = False
                    frmEveHQ.EveStatusIcon.Icon = Nothing : frmEveHQ.iconEveHQMLW.Icon = Nothing
                    frmEveHQ.EveStatusIcon.Dispose() : frmEveHQ.iconEveHQMLW.Dispose()
                Else
                    MessageBox.Show("Unable to load and display settings. Check log file for errors.", "Error displaying settings.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                End
            End If

            ' Check for existence of a cache folder in the application directory
            Core.HQ.WriteLogEvent("Start: Set core cache directory")
            Core.HQ.coreCacheFolder = Path.Combine(Application.StartupPath, "CoreCache")
            If My.Computer.FileSystem.DirectoryExists(Core.HQ.coreCacheFolder) = False Then
                MessageBox.Show("Unable to find core cache folder. EveHQ will now quit", "Cache Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End
            End If
            Core.HQ.WriteLogEvent("End: Set core cache directory")

            ' Load static data
            ThreadPool.QueueUserWorkItem(AddressOf LoadItemData)

            ' Insert the version number to the splash screen
            Core.HQ.WriteLogEvent("Start: Insert version info into splash screen")
            lblVersion.Text = "Version " & FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion
            lblDate.Text = "Released: " & My.Application.Info.Trademark
            lblCopyright.Text = My.Application.Info.Copyright
            Core.HQ.WriteLogEvent("End: Insert version info into splash screen")
            Core.HQ.WriteLogEvent("Start: Show Splash Screen")
            If _showSplash = True And _showSettings = False Then
                Show()
                Refresh()
            End If
            Core.HQ.WriteLogEvent("End: Show Splash Screen")

            ' Delete any .old files left over from the last update
            Core.HQ.WriteLogEvent("Start: Old update file check")
            Try
                For Each newFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.old")
                    Dim nfi As New FileInfo(newFile)
                    My.Computer.FileSystem.DeleteFile(nfi.FullName)
                Next
            Catch ex As Exception
                MessageBox.Show("Unable to delete update files, please delete any .old files manually that exist in the installation folder.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
            Core.HQ.WriteLogEvent("End: Old update file check")

            ' Check for existence of the XML cache folder in the application directory
            Core.HQ.WriteLogEvent("Start: Set XML cache directory")
            lblStatus.Text = "> Checking cache directory..."
            lblStatus.Refresh()
            If _isLocal = False Then
                Core.HQ.cacheFolder = Path.Combine(Core.HQ.AppDataFolder, "Cache")
            Else
                Core.HQ.cacheFolder = Path.Combine(Application.StartupPath, "Cache")
            End If
            If My.Computer.FileSystem.DirectoryExists(Core.HQ.cacheFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(Core.HQ.cacheFolder)
            End If
            Core.HQ.WriteLogEvent("End: Set XML cache directory")

            ' Check for existence of the image cache folder in the application directory
            Core.HQ.WriteLogEvent("Start: Set image cache directory")
            lblStatus.Text = "> Checking image cache directory..."
            lblStatus.Refresh()
            If _isLocal = False Then
                Core.HQ.imageCacheFolder = Path.Combine(Core.HQ.AppDataFolder, "ImageCache")
            Else
                Core.HQ.imageCacheFolder = Path.Combine(Application.StartupPath, "ImageCache")
            End If
            If My.Computer.FileSystem.DirectoryExists(Core.HQ.imageCacheFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(Core.HQ.imageCacheFolder)
            End If
            Core.HQ.WriteLogEvent("End: Set image cache directory")

            ' Check for existence of a report folder in the application directory
            Core.HQ.WriteLogEvent("Start: Set report directory")
            lblStatus.Text = "> Checking report folder..."
            lblStatus.Refresh()
            If _isLocal = False Then
                Core.HQ.reportFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EveHQ")
                Core.HQ.reportFolder = Path.Combine(Core.HQ.reportFolder, "Reports")
            Else
                Core.HQ.reportFolder = Path.Combine(Application.StartupPath, "Reports")
            End If
            If My.Computer.FileSystem.DirectoryExists(Core.HQ.reportFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(Core.HQ.reportFolder)
            End If
            Core.HQ.WriteLogEvent("End: Set report directory")

            ' Check for existence of a data folder in the application directory
            Core.HQ.WriteLogEvent("Start: Set data directory")
            lblStatus.Text = "> Checking data directory..."
            lblStatus.Refresh()
            If _isLocal = False Then
                Core.HQ.dataFolder = Path.Combine(Core.HQ.AppDataFolder, "Data")
            Else
                Core.HQ.dataFolder = Path.Combine(Application.StartupPath, "Data")
            End If
            If My.Computer.FileSystem.DirectoryExists(Core.HQ.dataFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(Core.HQ.dataFolder)
            End If
            Core.HQ.WriteLogEvent("End: Set data directory")

            ' Check for existence of a backup folder
            Core.HQ.WriteLogEvent("Start: Set Eve backup directory")
            lblStatus.Text = "> Checking backup directory..."
            lblStatus.Refresh()
            If _isLocal = False Then
                Core.HQ.backupFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EveHQ")
                Core.HQ.backupFolder = Path.Combine(Core.HQ.backupFolder, "Backups")
            Else
                Core.HQ.backupFolder = Path.Combine(Application.StartupPath, "Backups")
            End If
            If My.Computer.FileSystem.DirectoryExists(Core.HQ.backupFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(Core.HQ.backupFolder)
            End If
            Core.HQ.WriteLogEvent("End: Set Eve backup directory")

            ' Check for existence of an EveHQ backup folder
            Core.HQ.WriteLogEvent("Start: Set EveHQ backup directory")
            lblStatus.Text = "> Checking EveHQ backup directory..."
            lblStatus.Refresh()
            If _isLocal = False Then
                Core.HQ.EveHQBackupFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EveHQ")
                Core.HQ.EveHQBackupFolder = Path.Combine(Core.HQ.EveHQBackupFolder, "EveHQBackups")
            Else
                Core.HQ.EveHQBackupFolder = Path.Combine(Application.StartupPath, "EveHQBackups")
            End If
            If My.Computer.FileSystem.DirectoryExists(Core.HQ.EveHQBackupFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(Core.HQ.EveHQBackupFolder)
            End If
            Core.HQ.WriteLogEvent("End: Set EveHQ backup directory")

            ' Convert the settings if relevant
            ' Relevance of settings conversion is based on existence of EveHQSettings.bin file - may want to add other conditions
            Core.HQ.WriteLogEvent("Start: Checking settings files")
            If My.Computer.FileSystem.FileExists(Path.Combine(Core.HQ.AppDataFolder, "EveHQSettings.bin")) = True Then
                Core.HQ.WriteLogEvent("Start: Converting settings files")
                lblStatus.Text = "> Converting EveHQ settings..."
                lblStatus.Refresh()
                ConvertSettings(_isLocal, Core.HQ.AppDataFolder)
                Core.HQ.WriteLogEvent("End: Converting settings files")
            End If

            Core.HQ.WriteLogEvent("End: Checking settings files")

            ' Load user settings - this is needed to work out data connection type & update requirements
            Core.HQ.WriteLogEvent("Start: Loading settings")
            lblStatus.Text = "> Loading settings..."
            lblStatus.Refresh()
            Core.EveHQSettings.Load(False)
            Core.HQ.WriteLogEvent("End: Loading settings")

            ' Check for Widgets
            Core.HQ.WriteLogEvent("Start: Enumerate widgets")
            lblStatus.Text = "> Enumerating Widgets..."
            lblStatus.Refresh()
            ThreadPool.QueueUserWorkItem(AddressOf EnumerateWidgets)
            Core.HQ.WriteLogEvent("End: Enumerate widgets")

            ' Determine the visual style
            Core.HQ.WriteLogEvent("Start: Process Visual Styles")
            If Core.HQ.Settings.DisableVisualStyles = True Then
                Application.VisualStyleState = VisualStyles.VisualStyleState.NoneEnabled
            Else
                Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled
            End If
            Core.HQ.WriteLogEvent("End: Process Visual Styles")

            ' Start the G15 if applicable
            Core.HQ.WriteLogEvent("Start: Activate G15")
            If Core.HQ.Settings.ActivateG15 = True Then
                'Init the LCD
                Try
                    Core.G15Lcd.InitLCD()
                Catch ex As Exception
                End Try
                If Core.HQ.IsG15LCDActive = False Then
                    MessageBox.Show("Unable to start G15 Display. Please ensure you have the keyboard and drivers correctly installed.", "Error Starting G15", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ' Check if the LCD will cycle chars
                    If Core.HQ.Settings.CycleG15Pilots = True Then
                        Core.G15Lcd.tmrLCDChar.Interval = (1000 * Core.HQ.Settings.CycleG15Time)
                        Core.G15Lcd.tmrLCDChar.Enabled = True
                    End If
                End If
            End If
            Core.HQ.lcdPilot = Core.HQ.Settings.StartupPilot
            Core.HQ.WriteLogEvent("End: Activate G15")

            ' Force DBDataDirectory location if using SQL CE
            Core.HQ.WriteLogEvent("Start: Set data directory")
            Core.HQ.Settings.CustomDBFileName = Path.Combine(Core.HQ.AppDataFolder, "EveHQData.db3")
            Call Core.CustomDataFunctions.SetEveHQDataConnectionString()
            Core.HQ.WriteLogEvent("End: Set data directory")

            ' Check for new database
            Core.HQ.WriteLogEvent("Start: Check custom database")
            lblStatus.Text = "> Checking custom data..."
            lblStatus.Refresh()
            If My.Computer.FileSystem.FileExists(Core.HQ.Settings.CustomDBFileName) = False Then
                ' Looks like it hasn't been set so let's create it
                If Core.CustomDataFunctions.CreateCustomDB = False Then
                    MessageBox.Show("There was an error creating the EveHQ database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    'MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                ' Looks like the file exists, but let's test a connection to make sure 
                If Core.CustomDataFunctions.CheckCustomDatabaseConnection(True) = False Then
                    ' Looks like it fails so let's recreate it - but inform the user
                    Dim msg As String = "EveHQ has detected that although the the new storage database is initialised, it cannot connect to it." & ControlChars.CrLf
                    msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
                    msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to create the new database."
                    MessageBox.Show(msg, "EveHQ Custom Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
            Core.HQ.WriteLogEvent("End: Check custom database")

            ' Check for modules
            Core.HQ.WriteLogEvent("Start: Load Plug-ins")
            lblStatus.Text = "> Loading modules..."
            lblStatus.Refresh()
            ThreadPool.QueueUserWorkItem(AddressOf LoadModules)
            Core.HQ.WriteLogEvent("End: Load Plug-ins")

            'Set the servers to their server details
            Core.HQ.WriteLogEvent("Start: Set Eve Server details")
            lblStatus.Text = "> Setting Eve Server details..."
            lblStatus.Refresh()
            Core.HQ.myTQServer.Server = 0
            Core.HQ.WriteLogEvent("End: Set Eve Server details")

            ' Update the pilot account info
            Core.HQ.WriteLogEvent("Start: Check key skill information")
            If Core.PilotParseFunctions.LoadKeySkills() = False Then
                Const msg As String = "There was an error parsing your character skill data. This will be reset. Please connect to the API to download the latest data."
                MessageBox.Show(msg, "Error Parsing Pilot Skills", MessageBoxButtons.OK, MessageBoxIcon.Information)
                For Each rPilot As Core.EveHQPilot In Core.HQ.Settings.Pilots.Values
                    rPilot.PilotSkills = New Dictionary(Of String, Core.EveHQPilotSkill)
                    rPilot.SkillPoints = 0
                Next
            End If
            'Call frmSettings.UpdateAccounts()
            'Call frmEveHQ.UpdatePilotInfo(True)
            Core.HQ.WriteLogEvent("End: Check key skill information")

            ' Load the API Errors
            Core.HQ.WriteLogEvent("Start: Load ErrorAPI")
            Dim errorXML As New XmlDocument
            errorXML.LoadXml(My.Resources.Errors)
            Dim errList As XmlNodeList = errorXML.SelectNodes("/eveapi/result/rowset/row")
            If errList.Count <> 0 Then
                Core.HQ.APIErrors.Clear()
                For Each errNode As XmlNode In errList
                    Core.HQ.APIErrors.Add(errNode.Attributes.GetNamedItem("errorCode").Value, errNode.Attributes.GetNamedItem("errorText").Value)
                Next
            End If
            Core.HQ.WriteLogEvent("End: Load ErrorAPI")

            ' Check for additional database tables in the custom database
            Core.HQ.WriteLogEvent("Start: Check custom database tables")
            lblStatus.Text = "> Checking custom database tables..."
            lblStatus.Refresh()
            Call Core.CustomDataFunctions.CheckCoreDBTables()
            Core.HQ.WriteLogEvent("End: Check custom database tables")

            ' Load price data
            'Core.HQ.WriteLogEvent("Start: Load market prices")
            'Core.CustomDataFunctions.LoadMarketPricesFromDB()
            'Core.HQ.WriteLogEvent("End: Load market prices")
            Core.HQ.WriteLogEvent("Start: Load custom prices")
            Core.CustomDataFunctions.LoadCustomPricesFromDB()
            Core.HQ.WriteLogEvent("End: Load custom prices")

            ' Check if we need to start the market watcher
            Core.HQ.WriteLogEvent("Start: Enable Market Data Uploader")
            If Core.HQ.Settings.MarketDataUploadEnabled = True Then
                Core.HQ.MarketCacheUploader.Start()
            Else
                Core.HQ.MarketCacheUploader.Stop() ' It should be stopped already, but never hurts to set it so again.
            End If
            Core.HQ.WriteLogEvent("End: Enable Market Data Uploader")

            ' Show the main form
            Core.HQ.WriteLogEvent("Start: Initialise main form")
            lblStatus.Text = "> Initialising EveHQ..."
            lblStatus.Refresh()
            Core.G15Lcd.SplashFlag = False
            Core.HQ.MainForm = frmEveHQ
            Core.HQ.WriteLogEvent("End: Initialise main form")

            ' Await all loading
            Core.HQ.WriteLogEvent("Start: Awaiting final data loading")
            Do
            Loop Until _pluginsLoaded = True And _widgetsLoaded = True And _itemsLoaded = True
            Core.HQ.WriteLogEvent("End: Awaiting final data loading")

            ' Perform additional skill loading
            Core.HQ.WriteLogEvent(" *** Loading Eve skill data...")
            Call Core.SkillFunctions.LoadEveSkillData()
            Core.HQ.WriteLogEvent(" *** Checking missing skills...")
            Call Core.PilotParseFunctions.CheckMissingTrainingSkills()

            Core.HQ.WriteLogEvent("***** End: EveHQ Startup Routine via Splash Screen *****")
            frmEveHQ.Show()

        End Sub

        Private Sub ConvertSettings(useLocalSwitch As Boolean, settingsFolder As String)

            Dim oldSettings As Core.EveSettings
            Dim arguments As New StringBuilder
            Dim externalApp As New ProcessStartInfo
            externalApp.FileName = Path.Combine(Application.StartupPath, "EveHQ.SettingsConverter.exe")

            ' Load up the old settings file to see if we can grab some info
            If My.Computer.FileSystem.FileExists(Path.Combine(settingsFolder, "EveHQSettings.bin")) = True Then
                Using s As New FileStream(Path.Combine(settingsFolder, "EveHQSettings.bin"), FileMode.Open)
                    Dim f As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                    oldSettings = CType(f.Deserialize(s), Core.EveSettings)
                End Using

                ' Build the command line arguments
                If useLocalSwitch = True Then
                    arguments.Append("/local")
                End If
                arguments.Append(" /dbformat;" & CStr(oldSettings.DBFormat))
                arguments.Append(" /dbserver;" & oldSettings.DBServer)
                arguments.Append(" /dbname;" & oldSettings.DBServer)
                If oldSettings.DBSQLSecurity = True Then
                    arguments.Append(" /dbsqlsec;1")
                Else
                    arguments.Append(" /dbsqlsec;0")
                End If
                arguments.Append(" /dbusername;" & oldSettings.DBUsername)
                arguments.Append(" /dbpassword;" & oldSettings.DBPassword)
            End If
            externalApp.Arguments = arguments.ToString

            ' Start the process and wait until it has finished
            Using newProcess As Process = Process.Start(externalApp)
                newProcess.WaitForExit()
            End Using
      
        End Sub

        Private Sub LoadItemData(state As Object)

            ' Load data from the core cache
            EveData.StaticData.LoadCoreCache(Core.HQ.coreCacheFolder)

            ' Finalise skill loading
            Core.HQ.WriteLogEvent(" *** Items Finished Loading")
            _itemsLoaded = True

        End Sub

        Private Sub LoadModules(state As Object)
            For Each filename As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.dll")
                If IsDotNetAssembly(filename) = True Then
                    Try
                        Dim myAssembly As Assembly = Assembly.LoadFrom(filename)
                        Dim types() As Type = myAssembly.GetTypes
                        For Each t As Type In types
                            If t.IsPublic = True Then
                                If t.GetInterface("EveHQ.Core.IEveHQPlugIn") IsNot Nothing Then
                                    Dim myPlugIn As Core.IEveHQPlugIn = CType(Activator.CreateInstance(t), Core.IEveHQPlugIn)
                                    Dim eveHQPlugIn As Core.EveHQPlugIn = myPlugIn.GetEveHQPlugInInfo
                                    eveHQPlugIn.FileName = filename
                                    Dim fi As New FileInfo(filename)
                                    eveHQPlugIn.ShortFileName = fi.Name
                                    eveHQPlugIn.FileType = t.FullName
                                    eveHQPlugIn.Version = myAssembly.GetName.Version.ToString
                                    eveHQPlugIn.Instance = myPlugIn
                                    ' Get status of plug-ins from settings (should already exist!)
                                    If Core.HQ.Settings.Plugins.ContainsKey(eveHQPlugIn.Name) = True Then
                                        Dim userPlugIn As Core.EveHQPlugInConfig = Core.HQ.Settings.Plugins(eveHQPlugIn.Name)
                                        eveHQPlugIn.Disabled = userPlugIn.Disabled
                                        eveHQPlugIn.Available = True
                                    Else
                                        ' If not listed, it must be new
                                        eveHQPlugIn.Disabled = False
                                        eveHQPlugIn.Available = True
                                    End If
                                    ' Check for opening parameters
                                    If _plugInLoading.ContainsKey(eveHQPlugIn.Name) = True Then
                                        eveHQPlugIn.PostStartupData = _plugInLoading(eveHQPlugIn.Name)
                                    End If
                                    eveHQPlugIn.Status = Core.EveHQPlugInStatus.Uninitialised
                                    Core.HQ.Plugins.Add(eveHQPlugIn.Name, eveHQPlugIn)
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
            Core.HQ.WriteLogEvent(" *** Plug-ins Finished Loading")
            _pluginsLoaded = True
        End Sub

        Private Sub EnumerateWidgets(state As Object)
            Dim myAssembly As Assembly = Assembly.GetExecutingAssembly()
            For Each myType As Type In myAssembly.GetTypes
                If myType.BaseType.Name = "Widget" Then
                    ' Get the control name based on the property of an instance of the control
                    Dim myInstance As Control = CType(Activator.CreateInstance(myType), Control)
                    Dim pi As PropertyInfo = myType.GetProperty("ControlName")
                    Dim myControlName As String = pi.GetValue(myInstance, Nothing).ToString
                    Core.HQ.Widgets.Add(myControlName, myType.FullName)
                    myInstance.Dispose()
                End If
            Next
            Core.HQ.WriteLogEvent(" *** Widgets Finished Loading")
            _widgetsLoaded = True
        End Sub

        Private Sub GetServerMessage(state As Object)
            ' Download the message from the server
            Dim msgXML As XmlDocument = FetchMessageXML()
            Try
                If msgXML IsNot Nothing Then
                    Dim newMessage As New Core.EveHQMessage
                    Dim data As XmlNodeList = msgXML.SelectNodes("/eveHQMessage")
                    newMessage.MessageDate = DateTime.Parse(data(0).ChildNodes(0).InnerText, _culture)
                    newMessage.MessageTitle = data(0).ChildNodes(1).InnerText
                    newMessage.AllowIgnore = CBool(data(0).ChildNodes(2).InnerText)
                    newMessage.Message = data(0).ChildNodes(3).InnerText
                    If data(0).ChildNodes(4).ChildNodes.Count > 0 Then
                        newMessage.DisabledPlugins.Clear()
                        For Each disabledPlugin As XmlNode In data(0).ChildNodes(4).ChildNodes
                            newMessage.DisabledPlugins.Add(disabledPlugin.Attributes.GetNamedItem("name").Value, disabledPlugin.Attributes.GetNamedItem("version").Value)
                        Next
                    End If
                    Core.HQ.EveHQServerMessage = newMessage
                Else
                    Core.HQ.EveHQServerMessage = Nothing
                End If
            Catch e As Exception
                Core.HQ.EveHQServerMessage = Nothing
            End Try
            Core.HQ.WriteLogEvent(" *** Message Finished Loading")
            _messageLoaded = True
        End Sub

        Private Function FetchMessageXML() As XmlDocument
            ' Set a default policy level for the "http:" and "https" schemes.
            Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)
            Dim updateServer As String = Core.HQ.Settings.UpdateUrl
            Dim remoteURL As String = updateServer & "_message.xml"
            Dim webdata As String
            Dim updateXML As New XmlDocument
            Try
                ' Create the requester
                ServicePointManager.DefaultConnectionLimit = 10
                ServicePointManager.Expect100Continue = False
                ServicePointManager.FindServicePoint(New Uri(remoteURL))
                Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
                request.UserAgent = "EveHQ " & My.Application.Info.Version.ToString
                request.CachePolicy = policy
                request.Timeout = 10000 ' timeout set to 10s
                ' Setup proxy server (if required)
                Call Core.ProxyServerFunctions.SetupWebProxy(request)
                ' Prepare for a response from the server
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                ' Get the stream associated with the response.
                Dim receiveStream As Stream = response.GetResponseStream()
                ' Pipes the stream to a higher level stream reader with the required encoding format. 
                Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
                webdata = readStream.ReadToEnd()
                ' Check response string for any error codes?
                updateXML.LoadXml(webdata)
                Return updateXML
            Catch e As Exception
                Return Nothing
            End Try
        End Function

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
                            Dim rva15Value As UInt32 = br.ReadUInt32()
                            Return rva15Value <> 0
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

    End Class
End NameSpace