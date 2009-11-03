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
        Try
            ' Create the zip folder
            If My.Computer.FileSystem.DirectoryExists(zipFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(zipFolder)
            End If

            Dim zipSettings As FastZip = New FastZip()
            Me.Cursor = Cursors.WaitCursor
            zipSettings.CreateZip(zipFileName, EveHQ.Core.HQ.appDataFolder, True, "", "^(?:(?!cache).)*$")
            Me.Cursor = Cursors.Default

            EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast = backupTime
            EveHQ.Core.HQ.EveHQSettings.EveHQBackupLastResult = -1
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
        chkAuto.Checked = EveHQ.Core.HQ.EveHQSettings.EveHQBackupAuto
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

    Private Sub chkAuto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAuto.CheckedChanged
        Me.lblBackupDays.Enabled = chkAuto.Checked
        Me.lblBackupFreq.Enabled = chkAuto.Checked
        Me.lblBackupStart.Enabled = chkAuto.Checked
        Me.lblLastBackup.Enabled = chkAuto.Checked
        Me.lblLastBackupLbl.Enabled = chkAuto.Checked
        Me.lblNextBackup.Enabled = chkAuto.Checked
        Me.lblNextBackupLbl.Enabled = chkAuto.Checked
        Me.lblStartFormat.Enabled = chkAuto.Checked
        Me.nudDays.Enabled = chkAuto.Checked
        Me.dtpStart.Enabled = chkAuto.Checked
        EveHQ.Core.HQ.EveHQSettings.EveHQBackupAuto = chkAuto.Checked
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
End Class