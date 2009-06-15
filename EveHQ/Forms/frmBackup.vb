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
Imports System.IO

Public Class frmBackup

    Private Sub chkAuto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAuto.CheckedChanged
        If chkAuto.Checked = False Then
            Me.lblBackupDays.Enabled = False
            Me.lblBackupFreq.Enabled = False
            Me.lblBackupStart.Enabled = False
            Me.lblLastBackup.Enabled = False
            Me.lblLastBackupLbl.Enabled = False
            Me.lblNextBackup.Enabled = False
            Me.lblNextBackupLbl.Enabled = False
            Me.lblStartFormat.Enabled = False
            Me.nudDays.Enabled = False
            Me.dtpStart.Enabled = False
            EveHQ.Core.HQ.EveHQSettings.BackupAuto = False
        Else
            Me.lblBackupDays.Enabled = True
            Me.lblBackupFreq.Enabled = True
            Me.lblBackupStart.Enabled = True
            Me.lblLastBackup.Enabled = True
            Me.lblLastBackupLbl.Enabled = True
            Me.lblNextBackup.Enabled = True
            Me.lblNextBackupLbl.Enabled = True
            Me.lblStartFormat.Enabled = True
            Me.nudDays.Enabled = True
            Me.dtpStart.Enabled = True
            EveHQ.Core.HQ.EveHQSettings.BackupAuto = True
        End If
    End Sub

    Private Sub frmBackup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        nudDays.Tag = CInt(1) : dtpStart.Tag = CInt(1)
        chkAuto.Checked = EveHQ.Core.HQ.EveHQSettings.BackupAuto
        nudDays.Value = EveHQ.Core.HQ.EveHQSettings.BackupFreq
        dtpStart.Value = EveHQ.Core.HQ.EveHQSettings.BackupStart
        nudDays.Tag = 0 : dtpStart.Tag = 0
        Call CalcNextBackup()
        If EveHQ.Core.HQ.EveHQSettings.BackupLast.Year < 2000 Then
            lblLastBackup.Text = "<not backed up>"
        Else
            lblLastBackup.Text = Format(EveHQ.Core.HQ.EveHQSettings.BackupLast, "dd/MM/yyyy HH:mm")
        End If
        Call ScanBackups()
    End Sub

    Private Sub nudDays_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDays.ValueChanged
        If nudDays.Tag IsNot Nothing Then
            If nudDays.Tag.ToString = "0" Then
                EveHQ.Core.HQ.EveHQSettings.BackupFreq = CInt(nudDays.Value)
            End If
        End If
        Call CalcNextBackup()
    End Sub

    Private Sub dtpStart_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpStart.ValueChanged
        If dtpStart.Tag IsNot Nothing Then
            If dtpStart.Tag.ToString = "0" Then
                EveHQ.Core.HQ.EveHQSettings.BackupStart = dtpStart.Value
            End If
        End If
        Call CalcNextBackup()
    End Sub

    Private Sub btnBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackup.Click

        ' Check if we have anything to back up!
        Dim noLocations As Boolean = True
        For location As Integer = 1 To 4
            If EveHQ.Core.HQ.EveHQSettings.EveFolder(location) <> "" Then
                noLocations = False
            End If
        Next
        Do
            If noLocations = True Then
                Dim msg As String = ""
                msg &= "Before trying to backup your Eve-Online settings, you must set the" & ControlChars.CrLf
                msg &= "path to your Eve installation(s) in the Eve Folders section in EveHQ Settings." & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Would you like to do this now?"
                If MessageBox.Show(msg, "Backup Location Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                Else
                    Dim EveHQSettings As New frmSettings
                    EveHQSettings.Tag = "nodeEveFolders"
                    EveHQSettings.ShowDialog()
                    EveHQSettings.Dispose()
                End If
            End If
        Loop Until noLocations = False

        If BackupEveSettings() = True Then
            lblLastBackup.Text = Format(EveHQ.Core.HQ.EveHQSettings.BackupLast, "dd/MM/yyyy HH:mm")
        End If
        Call CalcNextBackup()
        Call ScanBackups()
    End Sub

    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        Call ScanBackups()
    End Sub

    Public Sub ScanBackups()
        lvwBackups.Items.Clear()
        Dim backupDirs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        backupDirs = My.Computer.FileSystem.GetDirectories(EveHQ.Core.HQ.backupFolder)
        Dim backupDir As String = ""
        For Each backupDir In backupDirs
            Dim backupFile As String = System.IO.Path.Combine(backupDir, "backup.txt")
            If My.Computer.FileSystem.FileExists(backupFile) = True Then
                Dim sr As StreamReader = New StreamReader(System.IO.Path.Combine(backupDir, "backup.txt"))
                Dim newLine As ListViewItem = New ListViewItem
                newLine.Name = backupDir
                newLine.Text = sr.ReadLine
                newLine.SubItems.Add(sr.ReadLine)
                newLine.SubItems.Add(sr.ReadLine)
                lvwBackups.Items.Add(newLine)
                sr.Close()
            End If
        Next
    End Sub

    Private Sub btnRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestore.Click
        If lvwBackups.SelectedItems.Count = 0 Then
            Dim msg As String = ""
            MessageBox.Show("Please select a backup to restore before proceeding.", "Backup Set Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            Call RestoreEveSettings(lvwBackups.SelectedItems(0))
            Call ScanBackups()
        End If
    End Sub

    Private Sub btnResetBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetBackup.Click
        If MessageBox.Show("Are you sure you wish to reset the last backup time?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        EveHQ.Core.HQ.EveHQSettings.BackupLast = CDate("01/01/1999")
        lblLastBackup.Text = "<not backed up>"
        Call CalcNextBackup()
    End Sub

    Public Sub CalcNextBackup()
        Dim nextBackup As Date = EveHQ.Core.HQ.EveHQSettings.BackupStart
        If EveHQ.Core.HQ.EveHQSettings.BackupLast > nextBackup Then
            nextBackup = EveHQ.Core.HQ.EveHQSettings.BackupLast
        End If
        nextBackup = DateAdd(DateInterval.Day, EveHQ.Core.HQ.EveHQSettings.BackupFreq, nextBackup)
        lblNextBackup.Text = Format(nextBackup, "dd/MM/yyyy HH:mm")
    End Sub

    Public Function BackupEveSettings() As Boolean
        Dim backupTime As Date = Now
        Dim timeStamp As String = Format(backupTime, "dd-MM-yyyy HH-mm")
        Dim noFolders As Boolean = True

        Try
            For location As Integer = 1 To 4
                If EveHQ.Core.HQ.EveHQSettings.EveFolder(location) <> "" Then
                    noFolders = False
                    ' We need to check 3 thing before backup
                    ' 1. There is a cache directory in our location directory
                    ' 2. The cache directory contains a prefs.ini file
                    ' 3. The cache directory contains a settings folder
                    ' 4. The cache directory contains a browser folder
                    Dim passed As Boolean = False

                    ' Check for correct cache locations
                    Dim cacheDir As String = ""
                    Dim prefsFile As String = ""
                    Dim settingsDir As String = ""
                    Dim browserDir As String = ""
                    Dim eveFolder As String = ""
                    If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(location) = True Then
                        cacheDir = System.IO.Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(location), "cache")
                        settingsDir = System.IO.Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(location), "settings")
                        prefsFile = System.IO.Path.Combine(cacheDir, "prefs.ini")
                        browserDir = System.IO.Path.Combine(cacheDir, "browser")
                    Else
                        ' Trinity 1.1 introduced (yet) another location :( Try to recreate this from the "location"
                        Dim eveSettingsFolder As String = EveHQ.Core.HQ.EveHQSettings.EveFolder(location)
                        eveSettingsFolder = eveSettingsFolder.Replace("\", "_").Replace(":", "").Replace(" ", "_").ToLower
                        eveSettingsFolder &= "_tranquility"
                        eveFolder = System.IO.Path.Combine(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP"), "Eve")
                        eveFolder = System.IO.Path.Combine(eveFolder, eveSettingsFolder)
                        cacheDir = System.IO.Path.Combine(eveFolder, "cache")
                        settingsDir = System.IO.Path.Combine(eveFolder, "settings")
                        prefsFile = System.IO.Path.Combine(settingsDir, "prefs.ini")
                        browserDir = System.IO.Path.Combine(cacheDir, "browser")
                    End If

                    ' Stage 1
                    If My.Computer.FileSystem.DirectoryExists(cacheDir) = True Then
                        ' Stage 2
                        If My.Computer.FileSystem.FileExists(prefsFile) = True Then
                            ' Stage 3
                            If My.Computer.FileSystem.DirectoryExists(settingsDir) = True Then
                                ' Stage 4
                                If My.Computer.FileSystem.DirectoryExists(browserDir) = True Then
                                    passed = True
                                End If
                            End If
                        End If
                    End If

                    If passed = True Then
                        ' Start the backup procedure
                        Dim destDir As String = System.IO.Path.Combine(EveHQ.Core.HQ.backupFolder, "Location " & location & " (" & timeStamp & ")")
                        Dim destPrefs As String = System.IO.Path.Combine(destDir, "prefs.ini")
                        Dim destText As String = System.IO.Path.Combine(destDir, "backup.txt")
                        Dim destSettings As String = System.IO.Path.Combine(destDir, "Settings")
                        Dim destBrowser As String = System.IO.Path.Combine(destDir, "Browser")

                        ' Copy the existing files
                        If My.Computer.FileSystem.DirectoryExists(destDir) = False Then
                            My.Computer.FileSystem.CreateDirectory(destDir)
                        End If
                        My.Computer.FileSystem.CopyDirectory(settingsDir, destSettings, True)
                        My.Computer.FileSystem.CopyFile(prefsFile, destPrefs, True)
                        My.Computer.FileSystem.CopyDirectory(browserDir, destBrowser, True)

                        ' Add a little text file!
                        Dim sw As StreamWriter = New StreamWriter(destText)
                        sw.WriteLine(backupTime)
                        sw.WriteLine("Location " & location)
                        sw.WriteLine(My.Computer.FileSystem.GetParentPath(settingsDir))
                        sw.Flush()
                        sw.Close()
                    Else
                    End If
                End If
            Next
            EveHQ.Core.HQ.EveHQSettings.BackupLast = backupTime
            If noFolders = True Then
                EveHQ.Core.HQ.EveHQSettings.BackupLastResult = 1
            Else
                EveHQ.Core.HQ.EveHQSettings.BackupLastResult = -1
            End If
            Return True
        Catch e As Exception
            ' Try and tidy up
            For location As Integer = 1 To 4
                Dim chkDir As String = EveHQ.Core.HQ.backupFolder & "Location " & location & timeStamp
                If My.Computer.FileSystem.DirectoryExists(chkDir) = True Then
                    My.Computer.FileSystem.DeleteDirectory(chkDir, CType(FileIO.DeleteDirectoryOption.DeleteAllContents, FileIO.UIOption), FileIO.RecycleOption.DeletePermanently)
                End If
            Next
            Dim msg As String = "Error Performing Backup"
            msg &= ControlChars.CrLf & e.Message & ControlChars.CrLf
            MessageBox.Show(msg, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            EveHQ.Core.HQ.EveHQSettings.BackupLastResult = 0
            Return False
        End Try
    End Function

    Public Function RestoreEveSettings(ByVal backupItem As ListViewItem) As Boolean
        If MessageBox.Show("Are you sure you wish to restore this backup?", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Return False
            Exit Function
        End If
        Try
            Dim strLoc As String = backupItem.SubItems(1).Text
            Dim location As String = strLoc.Substring(strLoc.Length - 1, 1)
            Dim sourceDir As String = backupItem.Name
            Dim destDir As String = backupItem.SubItems(2).Text

            ' Start the restore procedure
            'Dim cacheDir As String = EveHQ.Core.HQ.EveHQSettings.EveFolder(location) & "\cache"
            Dim prefsFile As String = System.IO.Path.Combine(sourceDir, "prefs.ini")
            Dim settingsDir As String = System.IO.Path.Combine(sourceDir, "settings")
            Dim browserDir As String = System.IO.Path.Combine(sourceDir, "browser")
            Dim destCache As String = ""
            Dim destPrefs As String = ""
            Dim destSettings As String = ""
            Dim destBrowser As String = ""
            If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(CInt(location)) = True Then
                destCache = System.IO.Path.Combine(destDir, "cache")
                destSettings = System.IO.Path.Combine(destDir, "settings")
                destPrefs = System.IO.Path.Combine(destCache, "prefs.ini")
                destBrowser = System.IO.Path.Combine(destCache, "Browser")
            Else
                destCache = System.IO.Path.Combine(destDir, "cache")
                destSettings = System.IO.Path.Combine(destDir, "settings")
                destPrefs = System.IO.Path.Combine(destSettings, "prefs.ini")
                destBrowser = System.IO.Path.Combine(destCache, "Browser")
            End If
            My.Computer.FileSystem.CopyDirectory(settingsDir, destSettings, True)
            My.Computer.FileSystem.CopyFile(prefsFile, destPrefs, True)
            My.Computer.FileSystem.CopyDirectory(browserDir, destBrowser, True)
            Return True
        Catch e As Exception
            Dim msg As String = "Error Performing Restore"
            msg &= ControlChars.CrLf & e.Message & ControlChars.CrLf
            MessageBox.Show(msg, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
            Exit Function
        End Try
    End Function


End Class