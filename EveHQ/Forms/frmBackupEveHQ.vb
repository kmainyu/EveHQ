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
Imports System.IO

Namespace Forms

    Public Class frmBackupEveHQ

        Private Sub btnBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackup.Click
            If EveHQ.Core.EveHQBackup.BackupEveHQSettings() = True Then
                lblLastBackup.Text = EveHQ.Core.HQ.Settings.EveHQBackupLast.ToString
            End If
            Call CalcNextBackup()
            Call ScanBackups()
        End Sub

        Private Sub CalcNextBackup()
            lblNextBackup.Text = EveHQ.Core.EveHQBackup.CalcNextBackup().ToString
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
            Select Case EveHQ.Core.HQ.Settings.EveHQBackupMode
                Case 0
                    radManualBackup.Checked = True
                Case 1
                    radPromptBackup.Checked = True
                Case 2
                    RadAutoBackup.Checked = True
            End Select
            If EveHQ.Core.HQ.Settings.EveHQBackupWarnFreq < 1 Then
                EveHQ.Core.HQ.Settings.EveHQBackupWarnFreq = 1
            End If
            nudBackupWarning.Value = EveHQ.Core.HQ.Settings.EveHQBackupWarnFreq
            If EveHQ.Core.HQ.Settings.EveHQBackupFreq < 1 Then
                EveHQ.Core.HQ.Settings.EveHQBackupFreq = 1
            End If
            nudDays.Value = EveHQ.Core.HQ.Settings.EveHQBackupFreq
            If EveHQ.Core.HQ.Settings.EveHQBackupStart < dtpStart.MinDate Then
                EveHQ.Core.HQ.Settings.EveHQBackupStart = Now
            End If
            dtpStart.Value = EveHQ.Core.HQ.Settings.EveHQBackupStart
            nudDays.Tag = 0 : dtpStart.Tag = 0
            Call CalcNextBackup()
            If EveHQ.Core.HQ.Settings.EveHQBackupLast.Year < 2000 Then
                lblLastBackup.Text = "<not backed up>"
            Else
                lblLastBackup.Text = EveHQ.Core.HQ.Settings.EveHQBackupLast.ToString
            End If
            chkBackupBeforeUpdate.Checked = EveHQ.Core.HQ.Settings.BackupBeforeUpdate
            Call ScanBackups()
        End Sub

        Private Sub btnResetBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetBackup.Click
            If MessageBox.Show("Are you sure you wish to reset the last backup time?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
            EveHQ.Core.HQ.Settings.EveHQBackupLast = CDate("01/01/1999")
            lblLastBackup.Text = "<not backed up>"
            Call CalcNextBackup()
        End Sub

        Private Sub nudDays_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDays.ValueChanged
            If nudDays.Tag IsNot Nothing Then
                If nudDays.Tag.ToString = "0" Then
                    EveHQ.Core.HQ.Settings.EveHQBackupFreq = CInt(nudDays.Value)
                End If
            End If
            Call CalcNextBackup()
        End Sub

        Private Sub dtpStart_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpStart.ValueChanged
            If dtpStart.Tag IsNot Nothing Then
                If dtpStart.Tag.ToString = "0" Then
                    EveHQ.Core.HQ.Settings.EveHQBackupStart = dtpStart.Value
                End If
            End If
            Call CalcNextBackup()
        End Sub

        Private Sub radManualBackup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radManualBackup.CheckedChanged
            If radManualBackup.Checked = True Then
                EveHQ.Core.HQ.Settings.EveHQBackupMode = 0
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
                EveHQ.Core.HQ.Settings.EveHQBackupMode = 1
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
                EveHQ.Core.HQ.Settings.EveHQBackupMode = 2
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

        Private Sub chkBackupBeforeUpdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBackupBeforeUpdate.CheckedChanged
            EveHQ.Core.HQ.Settings.BackupBeforeUpdate = chkBackupBeforeUpdate.Checked
        End Sub

        Private Sub btnRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestore.Click
            If lvwBackups.SelectedItems.Count = 1 Then
                ' Get the backup directory
                If My.Computer.FileSystem.DirectoryExists(lvwBackups.SelectedItems(0).Text) = True Then
                    Dim BackupDir As DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(lvwBackups.SelectedItems(0).Text)
                    ' Calculate the file
                    Dim FileName As String = BackupDir.Name & ".zip"
                    Dim BackupFile As String = Path.Combine(BackupDir.FullName, FileName)
                    ' Check the file exists
                    If My.Computer.FileSystem.FileExists(BackupFile) = True Then
                        Call EveHQ.Core.EveHQBackup.RestoreEveHQSettings(BackupFile)
                    Else
                        MessageBox.Show("Unable to locate backup file. Please verify that the file exists.", "Missing Backup File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                End If
            End If
        End Sub

        Private Sub lvwBackups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwBackups.SelectedIndexChanged
            If lvwBackups.SelectedItems.Count > 0 Then
                btnRestore.Enabled = True
            Else
                btnRestore.Enabled = False
            End If
        End Sub
    End Class
End NameSpace