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

Public Class frmSplash

    Dim isLocal As Boolean = False
    Dim showSplash As Boolean = True

    Private Sub frmSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Insert the version number to the splash screen
        lblVersion.Text = "Version " & My.Application.Info.Version.ToString

        ' Set the image for the splash screen
        Panel1.BackgroundImage = My.Resources.Splashv5

        ' Check for any commandline parameters that we need to account for
        For Each param As String In System.Environment.GetCommandLineArgs
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
        Next

        If showSplash = True Then
            Me.Show()
        End If

        ' Delete any .old files left over from the last update
        For Each newFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.old")
            Dim nfi As New IO.FileInfo(newFile)
            My.Computer.FileSystem.DeleteFile(nfi.FullName)
        Next

        ' Set the application folder
        lblStatus.Text = "Setting application directory..."
        Me.Refresh()
        EveHQ.Core.HQ.appFolder = Application.StartupPath

        ' Check for existence of an application folder in the application directory
        If isLocal = False Then
            lblStatus.Text = "Checking app data directory..."
            Me.Refresh()
            EveHQ.Core.HQ.appDataFolder = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\EveHQ").Replace("\\", "\")
            If EveHQ.Core.HQ.appDataFolder.StartsWith("\") = True Then
                EveHQ.Core.HQ.appDataFolder = "\" & EveHQ.Core.HQ.appDataFolder
            End If
            If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.appDataFolder) = False Then
                ' Create the cache folder if it doesn't exist
                My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.appDataFolder)
            End If
        Else
            EveHQ.Core.HQ.appDataFolder = EveHQ.Core.HQ.appFolder
        End If

        ' Check for existence of a cache folder in the application directory
        lblStatus.Text = "Checking cache directory..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.cacheFolder = (EveHQ.Core.HQ.appDataFolder & "\Cache").Replace("\\", "\")
        Else
            EveHQ.Core.HQ.cacheFolder = (Application.StartupPath & "\Cache").Replace("\\", "\")
        End If
        If EveHQ.Core.HQ.cacheFolder.StartsWith("\") = True Then
            EveHQ.Core.HQ.cacheFolder = "\" & EveHQ.Core.HQ.cacheFolder
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.cacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.cacheFolder)
        End If

        ' Check for existence of a report folder in the application directory
        lblStatus.Text = "Checking report folder..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.reportFolder = (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\EveHQ\Reports").Replace("\\", "\")
        Else
            EveHQ.Core.HQ.reportFolder = (Application.StartupPath & "\Reports").Replace("\\", "\")
        End If
        If EveHQ.Core.HQ.reportFolder.StartsWith("\") = True Then
            EveHQ.Core.HQ.reportFolder = "\" & EveHQ.Core.HQ.reportFolder
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.reportFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.reportFolder)
        End If

        ' Check for existence of a data folder in the application directory
        lblStatus.Text = "Checking data directory..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.dataFolder = (EveHQ.Core.HQ.appDataFolder & "\Data").Replace("\\", "\")
        Else
            EveHQ.Core.HQ.dataFolder = (Application.StartupPath & "\Data").Replace("\\", "\")
        End If
        If EveHQ.Core.HQ.dataFolder.StartsWith("\") = True Then
            EveHQ.Core.HQ.dataFolder = "\" & EveHQ.Core.HQ.dataFolder
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.dataFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.dataFolder)
        End If

        ' Check for existence of a backup folder in the application directory
        lblStatus.Text = "Checking backup directory..."
        Me.Refresh()
        If isLocal = False Then
            EveHQ.Core.HQ.backupFolder = (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\EveHQ\Backups").Replace("\\", "\")
        Else
            EveHQ.Core.HQ.backupFolder = (Application.StartupPath & "\Backups").Replace("\\", "\")
        End If
        If EveHQ.Core.HQ.backupFolder.StartsWith("\") = True Then
            EveHQ.Core.HQ.backupFolder = "\" & EveHQ.Core.HQ.backupFolder
        End If
        If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.backupFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.backupFolder)
        End If

        ' Load user settings - this is needed to work out data connection type & update requirements
        lblStatus.Text = "Loading settings..."
        Me.Refresh()
        Do While EveHQ.Core.HQ.EveHQSettings.LoadSettings() = False
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

        ' Start the G15 if applicable
        If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True Then
            'Init the LCD
            EveHQ.Core.HQ.EveHQLCD.InitLCD("EveHQ LCD Display")
            'With the LCD initialised, draw the opening screen
            EveHQ.Core.HQ.EveHQLCD.DrawIntroScreen()
            ' Check if the LCD will cycle chars
            If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
                EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Enabled = True
            End If
            ' Set the cycle timer
            EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.EveHQSettings.CycleG15Time)
        End If

        ' Load skill data and item data
        lblStatus.Text = "Loading skills && items..."
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
        lblStatus.Text = "Checking database..."
        Me.Refresh()
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = 1 Or EveHQ.Core.HQ.EveHQSettings.DBFormat = 2 Then
            Dim strSQL As String = "SELECT attributeGroup FROM dgmAttributeTypes"
            Dim testData As Data.DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If testData Is Nothing Then
                ' We seem to be missing the data so lets add it in!
                lblStatus.Text = "Customising MSSQL database..."
                Me.Refresh()
                Dim conn As New Data.SqlClient.SqlConnection
                conn.ConnectionString = EveHQ.Core.HQ.dataConnectionString
                conn.Open()
                Call EveHQ.Core.DataFunctions.AddSQLRefiningData(conn)
                Call EveHQ.Core.DataFunctions.AddSQLAttributeGroupColumn(conn)
                Call EveHQ.Core.DataFunctions.CorrectSQLEveUnits(conn)
                If conn.State = Data.ConnectionState.Open Then
                    conn.Close()
                End If
            End If
        End If

        ' Check for modules
        lblStatus.Text = "Loading modules..."
        Me.Refresh()
        Call LoadModules()

        'Set the servers to their server details
        lblStatus.Text = "Setting Eve Server details..."
        Me.Refresh()
        EveHQ.Core.HQ.myTQServer.Server = 0
        EveHQ.Core.HQ.mySiSiServer.Server = 1

        ' Update the pilot account info
        Call frmSettings.UpdateAccounts()
        Call frmEveHQ.UpdatePilotInfo(True)

        ' Show the main form
        lblStatus.Text = "Initialising EveHQ..."
        Me.Refresh()
        EveHQ.Core.HQ.MainForm = frmEveHQ
        frmEveHQ.Show()

        ' Activate the lcdPilot
        EveHQ.Core.HQ.lcdPilot = EveHQ.Core.HQ.myPilot.Name

        ' Check for updates if required
        If EveHQ.Core.HQ.EveHQSettings.AutoCheck = True Then
            lblStatus.Text = "Checking for updates..."
            Me.Refresh()
            Dim myUpdater As New frmUpdater
            myUpdater.ShowDialog()
        End If
    End Sub

    Private Sub LoadModules()
        For Each filename As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.dll")
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
                            If EveHQ.Core.HQ.PlugIns.Contains(EveHQPlugIn.Name) = True Then
                                Dim oldPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.PlugIns(EveHQPlugIn.Name), Core.PlugIn)
                                EveHQPlugIn.Disabled = oldPlugIn.Disabled
                                EveHQPlugIn.Available = True
                                EveHQ.Core.HQ.PlugIns.Remove(EveHQPlugIn.Name)
                            Else
                                ' If not listed, it must be new
                                EveHQPlugIn.Disabled = False
                                EveHQPlugIn.Available = True
                            End If
                            EveHQPlugIn.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                            EveHQ.Core.HQ.PlugIns.Add(EveHQPlugIn.Name, EveHQPlugIn)
                        End If
                    End If
                Next
                types = Nothing
                myAssembly = Nothing
            Catch bife As BadImageFormatException
                'Ignore non .Net dlls (ones without manifests i.e. the G15 lglcd.dll) i.e. don't error
            Catch e As Exception
                MessageBox.Show("Error loading module: " & filename & ControlChars.CrLf & e.Message.ToString, "Module Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try
        Next
    End Sub

End Class
