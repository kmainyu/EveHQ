' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2009  Lee Vessey
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
Imports ICSharpCode.SharpZipLib.Zip
Imports System.IO

Public Class frmBackupEveHQ

    Private Sub btnBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackup.Click
        If BackupEveHQSettings() = True Then
            lblLastBackup.Text = Format(EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast, "dd/MM/yyyy HH:mm")
        End If
        Call CalcNextBackup()
        Call ScanBackups()
    End Sub

    Public Function BackupEveHQSettings() As Boolean
        Dim backupTime As Date = Now
        Dim timeStamp As String = Format(backupTime, "yyyy-MM-dd-HH-mm-ss")
        Dim zipFolder As String = System.IO.Path.Combine(EveHQ.Core.HQ.EveHQBackupFolder, "EveHQBackup " & timeStamp)
        Dim zipFileName As String = System.IO.Path.Combine(zipFolder, "EveHQBackup " & timeStamp & ".zip")
        Dim oldTime As Date = EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast
        Dim oldResult As Integer = EveHQ.Core.HQ.EveHQSettings.EveHQBackupLastResult
        Try
            ' Update backup details
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast = backupTime
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupLastResult = -1

            ' Save the settings file
            Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()

            ' Create the zip folder
            If My.Computer.FileSystem.DirectoryExists(zipFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(zipFolder)
            End If

            ' Backup the data
            Dim zipSettings As FastZip = New FastZip()
            Me.Cursor = Cursors.WaitCursor
            zipSettings.CreateZip(zipFileName, EveHQ.Core.HQ.appDataFolder, True, "", "^(?:(?!cache).)*$")
            Me.Cursor = Cursors.Default
            Return True
        Catch e As Exception
            ' Try and tidy up
            ' Delete the zip folder
            If My.Computer.FileSystem.DirectoryExists(zipFolder) = True Then
                My.Computer.FileSystem.DeleteDirectory(zipFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
            Dim msg As String = "Error Performing EveHQ Backup:"
            msg &= ControlChars.CrLf & e.Message & ControlChars.CrLf
            MessageBox.Show(msg, "EveHQ Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupLastResult = 0
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast = oldTime
            Me.Cursor = Cursors.Default
            Return False
        End Try
    End Function

    Public Sub CalcNextBackup()
        Dim nextBackup As Date = EveHQ.Core.HQ.EveHQSettings.EveHQBackupStart
        If EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast > nextBackup Then
            nextBackup = EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast
        End If
        nextBackup = DateAdd(DateInterval.Day, EveHQ.Core.HQ.EveHQSettings.EveHQBackupFreq, nextBackup)
        lblNextBackup.Text = Format(nextBackup, "dd/MM/yyyy HH:mm")
    End Sub

    Public Sub ScanBackups()
        lvwBackups.BeginUpdate()
        lvwBackups.Items.Clear()
        Dim backupDirs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        backupDirs = My.Computer.FileSystem.GetDirectories(EveHQ.Core.HQ.EveHQBackupFolder)
        Dim backupDir As String = ""
        For Each backupDir In backupDirs
            Dim newLine As ListViewItem = New ListViewItem
            newLine.Name = backupDir
            newLine.Text = backupDir
            lvwBackups.Items.Add(newLine)
        Next
        lvwBackups.EndUpdate()
    End Sub

    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        Call ScanBackups()
    End Sub

    Private Sub frmBackupEveHQ_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        nudDays.Tag = CInt(1) : dtpStart.Tag = CInt(1)
        Select Case EveHQ.Core.HQ.EveHQSettings.EveHQBackupMode
            Case 0
                radManualBackup.Checked = True
            Case 1
                radPromptBackup.Checked = True
            Case 2
                RadAutoBackup.Checked = True
        End Select
        If EveHQ.Core.HQ.EveHQSettings.EveHQBackupWarnFreq < 1 Then
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupWarnFreq = 1
        End If
        nudBackupWarning.Value = EveHQ.Core.HQ.EveHQSettings.EveHQBackupWarnFreq
        If EveHQ.Core.HQ.EveHQSettings.EveHQBackupFreq < 1 Then
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupFreq = 1
        End If
        nudDays.Value = EveHQ.Core.HQ.EveHQSettings.EveHQBackupFreq
        If EveHQ.Core.HQ.EveHQSettings.EveHQBackupStart < dtpStart.MinDate Then
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupStart = Now
        End If
        dtpStart.Value = EveHQ.Core.HQ.EveHQSettings.EveHQBackupStart
        nudDays.Tag = 0 : dtpStart.Tag = 0
        Call CalcNextBackup()
        If EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast.Year < 2000 Then
            lblLastBackup.Text = "<not backed up>"
        Else
            lblLastBackup.Text = Format(EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast, "dd/MM/yyyy HH:mm")
        End If
        Call ScanBackups()
    End Sub

    Private Sub btnResetBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetBackup.Click
        If MessageBox.Show("Are you sure you wish to reset the last backup time?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast = CDate("01/01/1999")
        lblLastBackup.Text = "<not backed up>"
        Call CalcNextBackup()
    End Sub

    Private Sub nudDays_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDays.ValueChanged
        If nudDays.Tag IsNot Nothing Then
            If nudDays.Tag.ToString = "0" Then
                EveHQ.Core.HQ.EveHQSettings.EveHQBackupFreq = CInt(nudDays.Value)
            End If
        End If
        Call CalcNextBackup()
    End Sub

    Private Sub dtpStart_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpStart.ValueChanged
        If dtpStart.Tag IsNot Nothing Then
            If dtpStart.Tag.ToString = "0" Then
                EveHQ.Core.HQ.EveHQSettings.EveHQBackupStart = dtpStart.Value
            End If
        End If
        Call CalcNextBackup()
    End Sub

    Private Sub radManualBackup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radManualBackup.CheckedChanged
        If radManualBackup.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupMode = 0
            Me.lblBackupWarning.Enabled = False
            Me.lblBackupWarningDays.Enabled = False
            Me.nudBackupWarning.Enabled = False
            Me.lblBackupDays.Enabled = False
            Me.lblBackupFreq.Enabled = False
            Me.lblBackupStart.Enabled = False
            Me.lblNextBackup.Enabled = False
            Me.lblNextBackupLbl.Enabled = False
            Me.lblStartFormat.Enabled = False
            Me.nudDays.Enabled = False
            Me.dtpStart.Enabled = False
        End If
    End Sub

    Private Sub radPromptBackup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radPromptBackup.CheckedChanged
        If radPromptBackup.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupMode = 1
            Me.lblBackupWarning.Enabled = True
            Me.lblBackupWarningDays.Enabled = True
            Me.nudBackupWarning.Enabled = True
            Me.lblBackupDays.Enabled = False
            Me.lblBackupFreq.Enabled = False
            Me.lblBackupStart.Enabled = False
            Me.lblNextBackup.Enabled = False
            Me.lblNextBackupLbl.Enabled = False
            Me.lblStartFormat.Enabled = False
            Me.nudDays.Enabled = False
            Me.dtpStart.Enabled = False
        End If
    End Sub

    Private Sub RadAutoBackup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadAutoBackup.CheckedChanged
        If RadAutoBackup.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupMode = 2
            Me.lblBackupWarning.Enabled = False
            Me.lblBackupWarningDays.Enabled = False
            Me.nudBackupWarning.Enabled = False
            Me.lblBackupDays.Enabled = True
            Me.lblBackupFreq.Enabled = True
            Me.lblBackupStart.Enabled = True
            Me.lblNextBackup.Enabled = True
            Me.lblNextBackupLbl.Enabled = True
            Me.lblStartFormat.Enabled = True
            Me.nudDays.Enabled = True
            Me.dtpStart.Enabled = True
        End If
    End Sub
End Class