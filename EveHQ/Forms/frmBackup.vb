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
Imports EveHQ.Core
Imports System.Collections.ObjectModel
Imports System.IO
Imports Microsoft.VisualBasic.FileIO

Namespace Forms

    Public Class FrmBackup

        Private Sub chkAuto_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAuto.CheckedChanged
            If chkAuto.Checked = False Then
                lblBackupDays.Enabled = False
                lblBackupFreq.Enabled = False
                lblBackupStart.Enabled = False
                lblLastBackup.Enabled = False
                lblLastBackupLbl.Enabled = False
                lblNextBackup.Enabled = False
                lblNextBackupLbl.Enabled = False
                lblStartFormat.Enabled = False
                nudDays.Enabled = False
                dtpStart.Enabled = False
                HQ.Settings.BackupAuto = False
            Else
                lblBackupDays.Enabled = True
                lblBackupFreq.Enabled = True
                lblBackupStart.Enabled = True
                lblLastBackup.Enabled = True
                lblLastBackupLbl.Enabled = True
                lblNextBackup.Enabled = True
                lblNextBackupLbl.Enabled = True
                lblStartFormat.Enabled = True
                nudDays.Enabled = True
                dtpStart.Enabled = True
                HQ.Settings.BackupAuto = True
            End If
        End Sub

        Private Sub frmBackup_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            nudDays.Tag = CInt(1) : dtpStart.Tag = CInt(1)
            chkAuto.Checked = HQ.Settings.BackupAuto
            nudDays.Value = HQ.Settings.BackupFreq
            dtpStart.Value = HQ.Settings.BackupStart
            nudDays.Tag = 0 : dtpStart.Tag = 0
            Call CalcNextBackup()
            If HQ.Settings.BackupLast.Year < 2000 Then
                lblLastBackup.Text = "<not backed up>"
            Else
                lblLastBackup.Text = HQ.Settings.BackupLast.ToString
            End If
            Call ScanBackups()
        End Sub

        Private Sub nudDays_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudDays.ValueChanged
            If nudDays.Tag IsNot Nothing Then
                If nudDays.Tag.ToString = "0" Then
                    HQ.Settings.BackupFreq = CInt(nudDays.Value)
                End If
            End If
            Call CalcNextBackup()
        End Sub

        Private Sub dtpStart_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtpStart.ValueChanged
            If dtpStart.Tag IsNot Nothing Then
                If dtpStart.Tag.ToString = "0" Then
                    HQ.Settings.BackupStart = dtpStart.Value
                End If
            End If
            Call CalcNextBackup()
        End Sub

        Private Sub btnBackup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackup.Click

            ' Check if we have anything to back up!
            Dim noLocations As Boolean = True
            For eveLocation As Integer = 1 To 4
                If HQ.Settings.EveFolder(eveLocation) <> "" Then
                    noLocations = False
                End If
            Next
            Do
                If noLocations = True Then
                    Dim msg As String = ""
                    msg &= "Before trying to backup your Eve-Online settings, you must set the" & ControlChars.CrLf
                    msg &= "path to your Eve installation(s) in the Eve Folders section in EveHQ Settings." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Would you like to do this now?"
                    If MessageBox.Show(msg, "Backup Location Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                        Exit Sub
                    Else
                        Using eveHQSettings As New FrmSettings
                            eveHQSettings.Tag = "nodeEveFolders"
                            eveHQSettings.ShowDialog()
                        End Using
                    End If
                End If
            Loop Until noLocations = False

            If BackupEveSettings() = True Then
                lblLastBackup.Text = HQ.Settings.BackupLast.ToString
            End If
            Call CalcNextBackup()
            Call ScanBackups()
        End Sub

        Private Sub btnScan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnScan.Click
            Call ScanBackups()
        End Sub

        Public Sub ScanBackups()
            lvwBackups.BeginUpdate()
            lvwBackups.Items.Clear()
            Dim backupDirs As ReadOnlyCollection(Of String)
            backupDirs = My.Computer.FileSystem.GetDirectories(HQ.backupFolder)
            Dim backupDir As String
            For Each backupDir In backupDirs
                Dim backupFile As String = IO.Path.Combine(backupDir, "backup.txt")
                If My.Computer.FileSystem.FileExists(backupFile) = True Then
                    Dim sr As StreamReader = New StreamReader(IO.Path.Combine(backupDir, "backup.txt"))
                    Dim newLine As ListViewItem = New ListViewItem
                    newLine.Tag = backupDir
                    newLine.Text = sr.ReadLine
                    Dim bDate As DateTime
                    If DateTime.TryParse(newLine.Text, bDate) = True Then
                        newLine.Name = bDate.ToString("yyyyMMddHHmmss")
                    Else
                        newLine.Name = newLine.Text
                    End If
                    newLine.SubItems.Add(sr.ReadLine)
                    newLine.SubItems.Add(sr.ReadLine)
                    lvwBackups.Items.Add(newLine)
                    sr.Close()
                End If
            Next

            ' Do an initial sort of the first column
            lvwBackups.ListViewItemSorter = New ListViewItemComparerName(0, SortOrder.Ascending)
            lvwBackups.Tag = -1
            lvwBackups.Sort()
            lvwBackups.EndUpdate()

        End Sub

        Private Sub btnRestore_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRestore.Click
            If lvwBackups.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select a backup to restore before proceeding.", "Backup Set Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                Call RestoreEveSettings(lvwBackups.SelectedItems(0))
                Call ScanBackups()
            End If
        End Sub

        Private Sub btnResetBackup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetBackup.Click
            If MessageBox.Show("Are you sure you wish to reset the last backup time?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Exit Sub
            End If
            HQ.Settings.BackupLast = CDate("01/01/1999")
            lblLastBackup.Text = "<not backed up>"
            Call CalcNextBackup()
        End Sub

        Private Sub lvwBackups_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) Handles lvwBackups.ColumnClick
            If CInt(lvwBackups.Tag) = e.Column Then
                lvwBackups.ListViewItemSorter = New ListViewItemComparerName(e.Column, SortOrder.Ascending)
                lvwBackups.Tag = -1
            Else
                lvwBackups.ListViewItemSorter = New ListViewItemComparerName(e.Column, SortOrder.Descending)
                lvwBackups.Tag = e.Column
            End If
            ' Call the sort method to manually sort.
            lvwBackups.Sort()
        End Sub

        Public Sub CalcNextBackup()
            Dim nextBackup As Date = HQ.Settings.BackupStart
            If HQ.Settings.BackupLast > nextBackup Then
                nextBackup = HQ.Settings.BackupLast
            End If
            nextBackup = DateAdd(DateInterval.Day, HQ.Settings.BackupFreq, nextBackup)
            lblNextBackup.Text = nextBackup.ToString
        End Sub

        Public Function BackupEveSettings() As Boolean
            Dim backupTime As Date = Now
            Dim timeStamp As String = Format(backupTime, "dd-MM-yyyy HH-mm")
            Dim noFolders As Boolean = True

            Try
                For eveLocation As Integer = 1 To 4
                    If HQ.Settings.EveFolder(eveLocation) <> "" Then
                        noFolders = False
                        ' We need to check 3 thing before backup
                        ' 1. There is a cache directory in our location directory
                        ' 2. The cache directory contains a prefs.ini file
                        ' 3. The cache directory contains a settings folder
                        ' 4. The cache directory contains a browser folder
                        Dim passed As Boolean = False

                        ' Check for correct cache locations
                        Dim cacheDir As String
                        Dim prefsFile As String
                        Dim settingsDir As String
                        Dim browserDir As String
                        Dim eveFolder As String
                        If HQ.Settings.EveFolderLUA(eveLocation) = True Then
                            cacheDir = IO.Path.Combine(HQ.Settings.EveFolder(eveLocation), "cache")
                            settingsDir = IO.Path.Combine(HQ.Settings.EveFolder(eveLocation), "settings")
                            prefsFile = IO.Path.Combine(cacheDir, "prefs.ini")
                            browserDir = IO.Path.Combine(cacheDir, "browser")
                        Else
                            ' Trinity 1.1 introduced (yet) another location :( Try to recreate this from the "location"
                            Dim eveSettingsFolder As String = HQ.Settings.EveFolder(eveLocation)
                            eveSettingsFolder = eveSettingsFolder.Replace("\", "_").Replace(":", "").Replace(" ", "_").ToLower
                            eveSettingsFolder &= "_tranquility"
                            eveFolder = IO.Path.Combine(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP"), "Eve")
                            eveFolder = IO.Path.Combine(eveFolder, eveSettingsFolder)
                            cacheDir = IO.Path.Combine(eveFolder, "cache")
                            settingsDir = IO.Path.Combine(eveFolder, "settings")
                            prefsFile = IO.Path.Combine(settingsDir, "prefs.ini")
                            browserDir = IO.Path.Combine(cacheDir, "browser")
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
                            Dim destDir As String = IO.Path.Combine(HQ.backupFolder, "Location " & eveLocation & " (" & timeStamp & ")")
                            Dim destPrefs As String = IO.Path.Combine(destDir, "prefs.ini")
                            Dim destText As String = IO.Path.Combine(destDir, "backup.txt")
                            Dim destSettings As String = IO.Path.Combine(destDir, "Settings")
                            Dim destBrowser As String = IO.Path.Combine(destDir, "Browser")

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
                            sw.WriteLine("Location " & eveLocation)
                            sw.WriteLine(My.Computer.FileSystem.GetParentPath(settingsDir))
                            sw.Flush()
                            sw.Close()
                        Else
                        End If
                    End If
                Next
                HQ.Settings.BackupLast = backupTime
                If noFolders = True Then
                    HQ.Settings.BackupLastResult = 1
                Else
                    HQ.Settings.BackupLastResult = -1
                End If
                Return True
            Catch e As Exception
                ' Try and tidy up
                For eveLocation As Integer = 1 To 4
                    Dim chkDir As String = HQ.backupFolder & "Location " & eveLocation & timeStamp
                    If My.Computer.FileSystem.DirectoryExists(chkDir) = True Then
                        My.Computer.FileSystem.DeleteDirectory(chkDir, CType(DeleteDirectoryOption.DeleteAllContents, UIOption), RecycleOption.DeletePermanently)
                    End If
                Next
                Dim msg As String = "Error Performing Backup"
                msg &= ControlChars.CrLf & e.Message & ControlChars.CrLf
                MessageBox.Show(msg, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                HQ.Settings.BackupLastResult = 0
                Return False
            End Try
        End Function

        Public Function RestoreEveSettings(ByVal backupItem As ListViewItem) As Boolean
            If MessageBox.Show("Are you sure you wish to restore this backup?", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Return False
            End If
            Try
                Dim strLoc As String = backupItem.SubItems(1).Text
                Dim eveLocation As String = strLoc.Substring(strLoc.Length - 1, 1)
                Dim sourceDir As String = backupItem.Tag.ToString
                Dim destDir As String = backupItem.SubItems(2).Text

                ' Start the restore procedure
                'Dim cacheDir As String = EveHQ.Core.HQ.Settings.EveFolder(location) & "\cache"
                Dim prefsFile As String = IO.Path.Combine(sourceDir, "prefs.ini")
                Dim settingsDir As String = IO.Path.Combine(sourceDir, "settings")
                Dim browserDir As String = IO.Path.Combine(sourceDir, "browser")
                Dim destCache As String
                Dim destPrefs As String
                Dim destSettings As String
                Dim destBrowser As String
                If HQ.Settings.EveFolderLUA(CInt(eveLocation)) = True Then
                    destCache = IO.Path.Combine(destDir, "cache")
                    destSettings = IO.Path.Combine(destDir, "settings")
                    destPrefs = IO.Path.Combine(destCache, "prefs.ini")
                    destBrowser = IO.Path.Combine(destCache, "Browser")
                Else
                    destCache = IO.Path.Combine(destDir, "cache")
                    destSettings = IO.Path.Combine(destDir, "settings")
                    destPrefs = IO.Path.Combine(destSettings, "prefs.ini")
                    destBrowser = IO.Path.Combine(destCache, "Browser")
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
            End Try
        End Function
        
    End Class
End NameSpace