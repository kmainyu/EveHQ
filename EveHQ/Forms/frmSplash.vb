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
Imports System.Reflection
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports ICSharpCode.SharpZipLib.Zip
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Data.OleDb

Public Class frmSplash

	Dim isLocal As Boolean = False
	Dim showSplash As Boolean = True
	Dim showSettings As Boolean = False
	Dim PlugInLoading As New SortedList(Of String, String)
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Dim PluginsLoaded As Boolean = False
    Dim WidgetsLoaded As Boolean = False
    Dim ItemsLoaded As Boolean = False
    Dim MessageLoaded As Boolean = False

    Private is64BitProcess As Boolean = (IntPtr.Size = 8)
    Private is64BitOperatingSystem As Boolean = (is64BitProcess Or InternalCheckIsWow64())

    <DllImport("kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)> _
    Private Shared Function IsWow64Process(<[In]()> hProcess As IntPtr, <Out()> ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Public Shared Function InternalCheckIsWow64() As Boolean
        If (Environment.OSVersion.Version.Major = 5 AndAlso Environment.OSVersion.Version.Minor >= 1) OrElse Environment.OSVersion.Version.Major >= 6 Then
            Using p As Process = Process.GetCurrentProcess()
                Dim retVal As Boolean
                If Not IsWow64Process(p.Handle, retVal) Then
                    Return False
                End If
                Return retVal
            End Using
        Else
            Return False
        End If
    End Function

    Private Sub frmSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Create a custom font collection from resources
        Dim MyFonts As New Drawing.Text.PrivateFontCollection
        Dim FontBuffer As IntPtr
        Dim MyFont As Byte() = My.Resources.Square721BT
        FontBuffer = Marshal.AllocCoTaskMem(MyFont.Length)
        Marshal.Copy(MyFont, 0, FontBuffer, MyFont.Length)
        MyFonts.AddMemoryFont(FontBuffer, MyFont.Length)
        ' Create the fonts and add them to the labels
        Dim SplashFontStatus As Font = New Font(MyFonts.Families(0), 13, GraphicsUnit.Pixel)
        Dim SplashFontVersion As Font = New Font(MyFonts.Families(0), 11, GraphicsUnit.Pixel)
        Dim SplashFontOther As Font = New Font(MyFonts.Families(0), 9, GraphicsUnit.Pixel)
        lblStatus.Font = SplashFontStatus
        lblVersion.Font = SplashFontVersion
        lblCopyright.Font = SplashFontOther
        lblDate.Font = SplashFontOther

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

        ' Set the application folder
        lblStatus.Text = "> Setting application directory..."
        lblStatus.Refresh()
        EveHQ.Core.HQ.appFolder = Application.StartupPath

        ' Check for existence of an application folder in the application directory
        If isLocal = False Then
            lblStatus.Text = "> Checking app data directory..."
            lblStatus.Refresh()
            EveHQ.Core.HQ.appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveHQ")
            If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.appDataFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.appDataFolder)
            End If
        Else
            EveHQ.Core.HQ.appDataFolder = EveHQ.Core.HQ.appFolder
        End If

        ' Create log file
        EveHQ.Core.HQ.EveHQLogFile = New IO.StreamWriter(Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQLog.log"), False)
        EveHQ.Core.HQ.EveHQLogTimer.Start()
        EveHQ.Core.HQ.WriteLogEvent("Start of EveHQ Event Log: " & Now.ToString)

        If Stopwatch.IsHighResolution Then
            EveHQ.Core.HQ.WriteLogEvent("Operations timed using the system's high-resolution performance counter.")
        Else
            EveHQ.Core.HQ.WriteLogEvent("Operations timed using the DateTime class.")
        End If

        EveHQ.Core.HQ.WriteLogEvent("***** Start: EveHQ Startup Routine via Splash Screen *****")

        ' Insert the version number to the splash screen
        EveHQ.Core.HQ.WriteLogEvent("Start: Insert version info into splash screen")
        lblVersion.Text = "Version " & My.Application.Info.Version.ToString
        lblDate.Text = "Released: " & My.Application.Info.Trademark
        lblCopyright.Text = My.Application.Info.Copyright
        EveHQ.Core.HQ.WriteLogEvent("End: Insert version info into splash screen")

        EveHQ.Core.HQ.WriteLogEvent("Start: Show Splash Screen")
        If showSplash = True And showSettings = False Then
            Me.Show()
            Me.Refresh()
        End If
        EveHQ.Core.HQ.WriteLogEvent("End: Show Splash Screen")

        ' Delete any .old files left over from the last update
        EveHQ.Core.HQ.WriteLogEvent("Start: Old update file check")
        Try
            For Each newFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.old")
                Dim nfi As New IO.FileInfo(newFile)
                My.Computer.FileSystem.DeleteFile(nfi.FullName)
            Next
        Catch ex As Exception
            MessageBox.Show("Unable to delete update files, please delete any .old files manually that exist in the installation folder.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        EveHQ.Core.HQ.WriteLogEvent("End: Old update file check")

        ' Check for existence of a cache folder in the application directory
        EveHQ.Core.HQ.WriteLogEvent("Start: Set core cache directory")
        lblStatus.Text = "> Checking core cache directory..."
        lblStatus.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.coreCacheFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "CoreCache")
        Else
            EveHQ.Core.HQ.coreCacheFolder = Path.Combine(Application.StartupPath, "CoreCache")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.coreCacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.coreCacheFolder)
        End If
        EveHQ.Core.HQ.WriteLogEvent("End: Set core cache directory")

        ' Check for existence of a cache folder in the application directory
        EveHQ.Core.HQ.WriteLogEvent("Start: Set XML cache directory")
        lblStatus.Text = "> Checking cache directory..."
        lblStatus.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.cacheFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "Cache")
        Else
            EveHQ.Core.HQ.cacheFolder = Path.Combine(Application.StartupPath, "Cache")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.cacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.cacheFolder)
        End If
        EveHQ.Core.HQ.WriteLogEvent("End: Set XML cache directory")

        ' Check for existence of a cache folder in the application directory
        EveHQ.Core.HQ.WriteLogEvent("Start: Set image cache directory")
        lblStatus.Text = "> Checking image cache directory..."
        lblStatus.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.imageCacheFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "ImageCache")
        Else
            EveHQ.Core.HQ.imageCacheFolder = Path.Combine(Application.StartupPath, "ImageCache")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.imageCacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.imageCacheFolder)
        End If
        EveHQ.Core.HQ.WriteLogEvent("End: Set image cache directory")

        ' Check for existence of a report folder in the application directory
        EveHQ.Core.HQ.WriteLogEvent("Start: Set report directory")
        lblStatus.Text = "> Checking report folder..."
        lblStatus.Refresh()
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
        EveHQ.Core.HQ.WriteLogEvent("End: Set report directory")

        ' Check for existence of a data folder in the application directory
        EveHQ.Core.HQ.WriteLogEvent("Start: Set data directory")
        lblStatus.Text = "> Checking data directory..."
        lblStatus.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.dataFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "Data")
        Else
            EveHQ.Core.HQ.dataFolder = Path.Combine(Application.StartupPath, "Data")
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.dataFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.dataFolder)
        End If
        EveHQ.Core.HQ.WriteLogEvent("End: Set data directory")

        ' Check for existence of a backup folder
        EveHQ.Core.HQ.WriteLogEvent("Start: Set Eve backup directory")
        lblStatus.Text = "> Checking backup directory..."
        lblStatus.Refresh()
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
        EveHQ.Core.HQ.WriteLogEvent("End: Set Eve backup directory")

        ' Check for existence of an EveHQ backup folder
        EveHQ.Core.HQ.WriteLogEvent("Start: Set EveHQ backup directory")
        lblStatus.Text = "> Checking EveHQ backup directory..."
        lblStatus.Refresh()
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
        EveHQ.Core.HQ.WriteLogEvent("End: Set EveHQ backup directory")

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
        EveHQ.Core.HQ.WriteLogEvent("Start: Loading settings")
        lblStatus.Text = "> Loading settings..."
        lblStatus.Refresh()
        Do While EveHQ.Core.EveHQSettingsFunctions.LoadSettings() = False
            ' Ask if we want to check for a database
            Dim msg As String = "EveHQ was unable to load data from a Database." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "If you do not select a valid Database, EveHQ will exit." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Would you like to select a Database now?" & ControlChars.CrLf
            Dim reply As Integer = MessageBox.Show(msg, "Database Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.No Then
                End
            End If
            Dim EveHQSettings As New frmSettings
            EveHQSettings.DoNotRecalculatePilots = True
            EveHQSettings.Tag = "nodeDatabaseFormat"
            EveHQSettings.ShowDialog()
            EveHQSettings.Dispose()
        Loop
        EveHQ.Core.HQ.WriteLogEvent("End: Loading settings")

        ' Check for Widgets
        EveHQ.Core.HQ.WriteLogEvent("Start: Enumerate widgets")
        lblStatus.Text = "> Enumerating Widgets..."
        lblStatus.Refresh()
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.EnumerateWidgets)
        EveHQ.Core.HQ.WriteLogEvent("End: Enumerate widgets")

        ' Check for server messages (only if auto-web connections enabled)
        If EveHQ.Core.HQ.EveHQSettings.DisableAutoWebConnections = False Then
            EveHQ.Core.HQ.WriteLogEvent("Start: Server Message Thread")
            lblStatus.Text = "> Fetching messages..."
            lblStatus.Refresh()
            ' Store a message ready for when the main form comes up
            Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.GetServerMessage)
            EveHQ.Core.HQ.WriteLogEvent("End: Server Message Thread")
        Else
            EveHQ.Core.HQ.WriteLogEvent(" *** Message Finished Loading - AutoWebConnections disabled")
            MessageLoaded = True
        End If

        ' Determine the visual style
        EveHQ.Core.HQ.WriteLogEvent("Start: Process Visual Styles")
        If EveHQ.Core.HQ.EveHQSettings.DisableVisualStyles = True Then
            Application.VisualStyleState = VisualStyles.VisualStyleState.NoneEnabled
        Else
            Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled
        End If
        EveHQ.Core.HQ.WriteLogEvent("End: Process Visual Styles")

        ' Start the G15 if applicable
        EveHQ.Core.HQ.WriteLogEvent("Start: Activate G15")
        If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True Then
            'Init the LCD
            Try
                Core.G15LCDv2.InitLCD()
            Catch ex As Exception
            End Try
            If EveHQ.Core.HQ.IsG15LCDActive = False Then
                MessageBox.Show("Unable to start G15 Display. Please ensure you have the keyboard and drivers correctly installed.", "Error Starting G15", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ' Check if the LCD will cycle chars
                If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
                    Core.G15LCDv2.tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.EveHQSettings.CycleG15Time)
                    Core.G15LCDv2.tmrLCDChar.Enabled = True
                End If
            End If
        End If
        ' Activate the lcdPilot
        EveHQ.Core.HQ.lcdPilot = EveHQ.Core.HQ.EveHQSettings.StartupPilot
        EveHQ.Core.HQ.WriteLogEvent("End: Activate G15")

        ' Force DBDataDirectory location if using SQL CE
        EveHQ.Core.HQ.WriteLogEvent("Start: Set data directory")
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = 0 And EveHQ.Core.HQ.EveHQSettings.DBDataFilename <> Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.sdf") Then

            If EveHQ.Core.HQ.EveHQSettings.DBDataFilename = "" Then
                EveHQ.Core.HQ.EveHQSettings.DBDataFilename = Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.sdf")
            End If

            ' ***** Check if we need to upgrade the database from v1 (MDB) to v2 (SQLCE)

            Dim UpgradeDB As Boolean = False
            If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.DBDataFilename) Then
                Dim DBFI As New FileInfo(EveHQ.Core.HQ.EveHQSettings.DBDataFilename)
                If DBFI.Extension = ".mdb" Then
                    ' Just check this is a valid MDB that requires a conversion
                    ' ***** Doesn't work on 64-bit windows with 64-bit processes, need to check for this and run the custom upgrade app
                    Dim Is64bit As Boolean = is64BitOperatingSystem
                    If Is64bit Then
                        ' Run some upgrade app?
                        Dim msg As String = "EveHQ has detected that you are using a 64-bit operating system and as EveHQ can run as a 64-bit process, this causes issues with the upgrade of the old Custom Database." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "To prevent this, a separate application has been written to convert this data for you. Press OK to start the conversion application."
                        MessageBox.Show(msg, "External Upgrade Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Try
                            UpgradeDB = True
                            Process.Start(Path.Combine(EveHQ.Core.HQ.appFolder, "EveHQ.DataUpgrader.exe"), EveHQ.Core.HQ.appDataFolder)
                        Catch ex As Exception
                            ' Upgrade app not there? Meh, user will have to start again but won't be losing much anyway
                        End Try
                        MessageBox.Show("Press OK once the upgrade process has been completed.", "Awiting Upgrade Completion", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        ' Check for MDB file on Win32 OS
                        Dim ValidMDB As Boolean = True
                        Dim MDBConnection As OleDbConnection = New OleDbConnection
                        MDBConnection.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source = " & EveHQ.Core.HQ.EveHQSettings.DBDataFilename
                        Try
                            MDBConnection.Open()
                        Catch ex As Exception
                            MessageBox.Show("Could not open MDB for conversion: " & ex.Message)
                            ValidMDB = False
                        End Try
                        If ValidMDB = True Then
                            ' Convert the database
                            Dim UpgradeMDB As New EveHQ.Core.frmUpgradeMDB
                            UpgradeMDB.ShowDialog()
                            UpgradeDB = True
                        End If
                    End If
                End If
            End If

            ' We don't need to relocate the DB if we've upgraded

            Dim oldLocation As String = EveHQ.Core.HQ.EveHQSettings.DBDataFilename
            Dim newLocation As String = Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.sdf")
            If UpgradeDB = False Then
                If My.Computer.FileSystem.FileExists(oldLocation) = True And oldLocation <> newLocation Then
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
            Else
                Try
                    My.Computer.FileSystem.DeleteFile(oldLocation)
                Catch ex As Exception
                    'Dim msg As String = "Unable to delete the old custom database from " & ControlChars.CrLf & oldLocation & ". This will need to be deleted manually if required."
                    'MessageBox.Show(msg, "Unable to Delete Custom Database", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End If
        End If
        EveHQ.Core.HQ.EveHQSettings.DBDataFilename = Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQData.sdf")
        Call EveHQ.Core.DataFunctions.SetEveHQDataConnectionString()
        EveHQ.Core.HQ.WriteLogEvent("End: Set data directory")

        ' Check for new database
        EveHQ.Core.HQ.WriteLogEvent("Start: Check custom database")
        lblStatus.Text = "> Checking custom data..."
        lblStatus.Refresh()
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case EveHQ.Core.DatabaseFormat.SQLCE
                If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.DBDataFilename) = False Then
                    ' Looks like it hasn't been set so let's create it - but inform the user
                    Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
                    msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
                    msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
                    'MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                        MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        'MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    ' Looks like the file exists, but let's test a connection to make sure 
                    If EveHQ.Core.DataFunctions.CheckDataDatabaseConnection(True) = False Then
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
            Case EveHQ.Core.DatabaseFormat.SQL
                If EveHQ.Core.HQ.EveHQSettings.DBDataName <> "EveHQData" Then
                    ' Setting is blank but let's just check for the DB anyway in case it's already there.
                    EveHQ.Core.HQ.EveHQSettings.DBDataName = "EveHQData"
                    If EveHQ.Core.DataFunctions.CheckDataDatabaseConnection(True) = False Then
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
                    End If
                Else
                    ' Looks like the file exists, but let's test a connection to make sure 
                    If EveHQ.Core.DataFunctions.CheckDataDatabaseConnection(True) = False Then
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
        EveHQ.Core.HQ.WriteLogEvent("End: Check custom database")

        ' Load skill data and item data
        EveHQ.Core.HQ.WriteLogEvent("Start: Load skills and item data")
        lblStatus.Text = "> Loading skills and items..."
        lblStatus.Refresh()
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.LoadItemData)
        EveHQ.Core.HQ.WriteLogEvent("End: Load skills and item data")

        ' If we get this far we have loaded a DB so check for SQL format and check the custom data
        EveHQ.Core.HQ.WriteLogEvent("Start: Customise database")
        lblStatus.Text = "> Checking database..."
        lblStatus.Refresh()
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = 1 Or EveHQ.Core.HQ.EveHQSettings.DBFormat = 2 Then
            Dim strSQL As String = "SELECT attributeGroup FROM dgmAttributeTypes"
            Dim testData As Data.DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If testData Is Nothing Then
                ' We seem to be missing the data so lets add it in!
                lblStatus.Text = "> Customising MSSQL database..."
                lblStatus.Refresh()
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
        EveHQ.Core.HQ.WriteLogEvent("End: Customise database")

        ' Check for modules
        EveHQ.Core.HQ.WriteLogEvent("Start: Load Plug-ins")
        lblStatus.Text = "> Loading modules..."
        lblStatus.Refresh()
        Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.LoadModules)
        EveHQ.Core.HQ.WriteLogEvent("End: Load Plug-ins")

        'Set the servers to their server details
        EveHQ.Core.HQ.WriteLogEvent("Start: Set Eve Server details")
        lblStatus.Text = "> Setting Eve Server details..."
        lblStatus.Refresh()
        EveHQ.Core.HQ.myTQServer.Server = 0
        EveHQ.Core.HQ.WriteLogEvent("End: Set Eve Server details")

        ' Update the pilot account info
        EveHQ.Core.HQ.WriteLogEvent("Start: Check key skill information")
        If EveHQ.Core.PilotParseFunctions.LoadKeySkills() = False Then
            Dim msg As String = "There was an error parsing your character skill data. This will be reset. Please connect to the API to download the latest data."
            MessageBox.Show(msg, "Error Parsing Pilot Skills", MessageBoxButtons.OK, MessageBoxIcon.Information)
            For Each rPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                rPilot.PilotSkills = New Collection
                rPilot.SkillPoints = 0
            Next
        End If
        Call EveHQ.Core.PilotParseFunctions.CheckMissingTrainingSkills()
        'Call frmSettings.UpdateAccounts()
        'Call frmEveHQ.UpdatePilotInfo(True)
        EveHQ.Core.HQ.WriteLogEvent("End: Check key skill information")

        ' Load the API Errors
        EveHQ.Core.HQ.WriteLogEvent("Start: Load ErrorAPI")
        Dim ErrorXML As New Xml.XmlDocument
        ErrorXML.LoadXml(My.Resources.Errors)
        Dim ErrList As Xml.XmlNodeList = ErrorXML.SelectNodes("/eveapi/result/rowset/row")
        If ErrList.Count <> 0 Then
            EveHQ.Core.HQ.APIErrors.Clear()
            For Each ErrNode As Xml.XmlNode In ErrList
                EveHQ.Core.HQ.APIErrors.Add(ErrNode.Attributes.GetNamedItem("errorCode").Value, ErrNode.Attributes.GetNamedItem("errorText").Value)
            Next
        End If
        EveHQ.Core.HQ.WriteLogEvent("End: Load ErrorAPI")

        ' Check for additional database tables in the custom database
        EveHQ.Core.HQ.WriteLogEvent("Start: Check Custom Database")
        lblStatus.Text = "> Checking Custom Database..."
        lblStatus.Refresh()
        Call EveHQ.Core.DataFunctions.CheckForEveMailTable()
        'Call EveHQ.Core.DataFunctions.CheckForEveNotificationTable()
        Call EveHQ.Core.DataFunctions.CheckForIDNameTable()
        EveHQ.Core.HQ.WriteLogEvent("End: Check Custom Database")

        ' Check if we need to start the market watcher
        EveHQ.Core.HQ.WriteLogEvent("Start: Enable Market Watcher")
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
        EveHQ.Core.HQ.WriteLogEvent("End: Enable Market Watcher")

        ' Show the main form
        EveHQ.Core.HQ.WriteLogEvent("Start: Initialise main form")
        lblStatus.Text = "> Initialising EveHQ..."
        lblStatus.Refresh()
        EveHQ.Core.G15LCDv2.SplashFlag = False
        EveHQ.Core.HQ.MainForm = frmEveHQ
        EveHQ.Core.HQ.WriteLogEvent("End: Initialise main form")

        ' Await all loading
        EveHQ.Core.HQ.WriteLogEvent("Start: Awaiting final data loading")
        Do
        Loop Until PluginsLoaded = True And WidgetsLoaded = True And ItemsLoaded = True And MessageLoaded = True
        EveHQ.Core.HQ.WriteLogEvent("End: Awaiting final data loading")

        EveHQ.Core.HQ.WriteLogEvent("***** End: EveHQ Startup Routine via Splash Screen *****")
        frmEveHQ.Show()

    End Sub

    Private Sub LoadItemData(state As Object)
        If EveHQ.Core.DataFunctions.LoadCoreCache = False Then
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
            Call EveHQ.Core.DataFunctions.LoadSolarSystems()
            Call EveHQ.Core.DataFunctions.LoadStations()
        End If
        EveHQ.Core.HQ.WriteLogEvent(" *** Items Finished Loading")
        ItemsLoaded = True
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
        EveHQ.Core.HQ.WriteLogEvent(" *** Plug-ins Finished Loading")
        PluginsLoaded = True
    End Sub

    Private Sub EnumerateWidgets(state As Object)
        Dim myAssembly As Assembly = Assembly.GetExecutingAssembly()
        For Each myType As Type In myAssembly.GetTypes
            If myType.BaseType.Name = "Widget" Then
                ' Get the control name based on the property of an instance of the control
                Dim myInstance As Control = CType(Activator.CreateInstance(myType), Control)
                Dim pi As System.Reflection.PropertyInfo = myType.GetProperty("ControlName")
                Dim myControlName As String = pi.GetValue(myInstance, Nothing).ToString
                EveHQ.Core.HQ.Widgets.Add(myControlName, myType.FullName)
                myInstance.Dispose()
            End If
        Next
        EveHQ.Core.HQ.WriteLogEvent(" *** Widgets Finished Loading")
        WidgetsLoaded = True
    End Sub

    Private Sub GetServerMessage(state As Object)
        ' Download the message from the server
        Dim MsgXML As XmlDocument = FetchMessageXML()
        Try
            If MsgXML IsNot Nothing Then
                Dim NewMessage As New EveHQ.Core.EveHQMessage
                Dim data As XmlNodeList = MsgXML.SelectNodes("/eveHQMessage")
                NewMessage.MessageDate = DateTime.Parse(data(0).ChildNodes(0).InnerText, culture)
                NewMessage.MessageTitle = data(0).ChildNodes(1).InnerText
                NewMessage.AllowIgnore = CBool(data(0).ChildNodes(2).InnerText)
                NewMessage.Message = data(0).ChildNodes(3).InnerText
                If data(0).ChildNodes(4).ChildNodes.Count > 0 Then
                    NewMessage.DisabledPlugins.Clear()
                    For Each DisabledPlugin As XmlNode In data(0).ChildNodes(4).ChildNodes
                        NewMessage.DisabledPlugins.Add(DisabledPlugin.Attributes.GetNamedItem("name").Value, DisabledPlugin.Attributes.GetNamedItem("version").Value)
                    Next
                End If
                EveHQ.Core.HQ.EveHQServerMessage = NewMessage
            Else
                EveHQ.Core.HQ.EveHQServerMessage = Nothing
            End If
        Catch e As Exception
            EveHQ.Core.HQ.EveHQServerMessage = Nothing
        End Try
        EveHQ.Core.HQ.WriteLogEvent(" *** Message Finished Loading")
        MessageLoaded = True
    End Sub

	Private Function FetchMessageXML() As XmlDocument
		' Set a default policy level for the "http:" and "https" schemes.
		Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)
		Dim UpdateServer As String = EveHQ.Core.HQ.EveHQSettings.UpdateURL
        Dim remoteURL As String = UpdateServer & "_message.xml"
		Dim webdata As String = ""
		Dim UpdateXML As New XmlDocument
		Try
			' Create the requester
			ServicePointManager.DefaultConnectionLimit = 10
			ServicePointManager.Expect100Continue = False
			Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(remoteURL))
			Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
			request.UserAgent = "EveHQ " & My.Application.Info.Version.ToString
            request.CachePolicy = policy
            request.Timeout = 10000 ' timeout set to 10s
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
			Return Nothing
		End Try
	End Function

    Private Function CompareVersions(ByVal thisVersion As String, ByVal requiredVersion As String) As Boolean
        Dim localVers() As String = thisVersion.Split(CChar("."))
        Dim remoteVers() As String = requiredVersion.Split(CChar("."))
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

End Class
