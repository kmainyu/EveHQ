'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports EveHQ.Core
Imports System.Collections.ObjectModel
Imports System.IO

Namespace Forms

    Public Class FrmBackupEveHQ

        Private Sub btnBackup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackup.Click
            If EveHQBackup.BackupEveHQSettings() = True Then
                lblLastBackup.Text = HQ.Settings.EveHQBackupLast.ToString
            End If
            Call CalcNextBackup()
            Call ScanBackups()
        End Sub

        Private Sub CalcNextBackup()
            lblNextBackup.Text = EveHQBackup.CalcNextBackup().ToString
        End Sub

        Public Sub ScanBackups()
            lvwBackups.BeginUpdate()
            lvwBackups.Items.Clear()
            Dim backupDirs As ReadOnlyCollection(Of String)
            backupDirs = My.Computer.FileSystem.GetDirectories(HQ.EveHQBackupFolder)
            Dim backupDir As String
            For Each backupDir In backupDirs
                Dim newLine As ListViewItem = New ListViewItem
                newLine.Name = backupDir
                newLine.Text = backupDir
                lvwBackups.Items.Add(newLine)
            Next
            lvwBackups.EndUpdate()
        End Sub

        Private Sub btnScan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnScan.Click
            Call ScanBackups()
        End Sub

        Private Sub frmBackupEveHQ_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            nudDays.Tag = CInt(1) : dtpStart.Tag = CInt(1)
            Select Case HQ.Settings.EveHQBackupMode
                Case 0
                    radManualBackup.Checked = True
                Case 1
                    radPromptBackup.Checked = True
                Case 2
                    RadAutoBackup.Checked = True
            End Select
            If HQ.Settings.EveHQBackupWarnFreq < 1 Then
                HQ.Settings.EveHQBackupWarnFreq = 1
            End If
            nudBackupWarning.Value = HQ.Settings.EveHQBackupWarnFreq
            If HQ.Settings.EveHQBackupFreq < 1 Then
                HQ.Settings.EveHQBackupFreq = 1
            End If
            nudDays.Value = HQ.Settings.EveHQBackupFreq
            If HQ.Settings.EveHQBackupStart < dtpStart.MinDate Then
                HQ.Settings.EveHQBackupStart = Now
            End If
            dtpStart.Value = HQ.Settings.EveHQBackupStart
            nudDays.Tag = 0 : dtpStart.Tag = 0
            Call CalcNextBackup()
            If HQ.Settings.EveHQBackupLast.Year < 2000 Then
                lblLastBackup.Text = "<not backed up>"
            Else
                lblLastBackup.Text = HQ.Settings.EveHQBackupLast.ToString
            End If
            chkBackupBeforeUpdate.Checked = HQ.Settings.BackupBeforeUpdate
            Call ScanBackups()
        End Sub

        Private Sub btnResetBackup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetBackup.Click
            If MessageBox.Show("Are you sure you wish to reset the last backup time?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Exit Sub
            End If
            HQ.Settings.EveHQBackupLast = CDate("01/01/1999")
            lblLastBackup.Text = "<not backed up>"
            Call CalcNextBackup()
        End Sub

        Private Sub nudDays_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudDays.ValueChanged
            If nudDays.Tag IsNot Nothing Then
                If nudDays.Tag.ToString = "0" Then
                    HQ.Settings.EveHQBackupFreq = CInt(nudDays.Value)
                End If
            End If
            Call CalcNextBackup()
        End Sub

        Private Sub dtpStart_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtpStart.ValueChanged
            If dtpStart.Tag IsNot Nothing Then
                If dtpStart.Tag.ToString = "0" Then
                    HQ.Settings.EveHQBackupStart = dtpStart.Value
                End If
            End If
            Call CalcNextBackup()
        End Sub

        Private Sub radManualBackup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radManualBackup.CheckedChanged
            If radManualBackup.Checked = True Then
                HQ.Settings.EveHQBackupMode = 0
                lblBackupWarning.Enabled = False
                lblBackupWarningDays.Enabled = False
                nudBackupWarning.Enabled = False
                lblBackupDays.Enabled = False
                lblBackupFreq.Enabled = False
                lblBackupStart.Enabled = False
                lblNextBackup.Enabled = False
                lblNextBackupLbl.Enabled = False
                lblStartFormat.Enabled = False
                nudDays.Enabled = False
                dtpStart.Enabled = False
            End If
        End Sub

        Private Sub radPromptBackup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radPromptBackup.CheckedChanged
            If radPromptBackup.Checked = True Then
                HQ.Settings.EveHQBackupMode = 1
                lblBackupWarning.Enabled = True
                lblBackupWarningDays.Enabled = True
                nudBackupWarning.Enabled = True
                lblBackupDays.Enabled = False
                lblBackupFreq.Enabled = False
                lblBackupStart.Enabled = False
                lblNextBackup.Enabled = False
                lblNextBackupLbl.Enabled = False
                lblStartFormat.Enabled = False
                nudDays.Enabled = False
                dtpStart.Enabled = False
            End If
        End Sub

        Private Sub RadAutoBackup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadAutoBackup.CheckedChanged
            If RadAutoBackup.Checked = True Then
                HQ.Settings.EveHQBackupMode = 2
                lblBackupWarning.Enabled = False
                lblBackupWarningDays.Enabled = False
                nudBackupWarning.Enabled = False
                lblBackupDays.Enabled = True
                lblBackupFreq.Enabled = True
                lblBackupStart.Enabled = True
                lblNextBackup.Enabled = True
                lblNextBackupLbl.Enabled = True
                lblStartFormat.Enabled = True
                nudDays.Enabled = True
                dtpStart.Enabled = True
            End If
        End Sub

        Private Sub chkBackupBeforeUpdate_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkBackupBeforeUpdate.CheckedChanged
            HQ.Settings.BackupBeforeUpdate = chkBackupBeforeUpdate.Checked
        End Sub

        Private Sub btnRestore_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRestore.Click
            If lvwBackups.SelectedItems.Count = 1 Then
                ' Get the backup directory
                If My.Computer.FileSystem.DirectoryExists(lvwBackups.SelectedItems(0).Text) = True Then
                    Dim backupDir As DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(lvwBackups.SelectedItems(0).Text)
                    ' Calculate the file
                    Dim fileName As String = backupDir.Name & ".zip"
                    Dim backupFile As String = Path.Combine(backupDir.FullName, fileName)
                    ' Check the file exists
                    If My.Computer.FileSystem.FileExists(backupFile) = True Then
                        Call EveHQBackup.RestoreEveHQSettings(backupFile)
                    Else
                        MessageBox.Show("Unable to locate backup file. Please verify that the file exists.", "Missing Backup File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                End If
            End If
        End Sub

        Private Sub lvwBackups_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lvwBackups.SelectedIndexChanged
            If lvwBackups.SelectedItems.Count > 0 Then
                btnRestore.Enabled = True
            Else
                btnRestore.Enabled = False
            End If
        End Sub
    End Class
End NameSpace