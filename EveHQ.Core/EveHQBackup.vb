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
Imports System.Windows.Forms
Imports ICSharpCode.SharpZipLib.Zip

''' <summary>
''' Contains shared routines for handling backup and restore of EveHQ
''' </summary>
''' <remarks></remarks>
Public Class EveHQBackup

    ''' <summary>
    ''' Calculates the date of the next EveHQ backup
    ''' </summary>
    ''' <returns>A date indicating the time of the next backup</returns>
    ''' <remarks></remarks>
    Public Shared Function CalcNextBackup() As Date
        Dim nextBackup As Date = EveHQ.Core.HQ.EveHqSettings.EveHQBackupStart
        If EveHQ.Core.HQ.EveHqSettings.EveHQBackupLast > nextBackup Then
            nextBackup = EveHQ.Core.HQ.EveHqSettings.EveHQBackupLast
        End If
        nextBackup = DateAdd(DateInterval.Day, EveHQ.Core.HQ.EveHqSettings.EveHQBackupFreq, nextBackup)
        Return nextBackup
    End Function

    ''' <summary>
    ''' Backs up the EveHQ settings and associated plug-in data
    ''' </summary>
    ''' <returns>A boolean value indicating if the backup procedure was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function BackupEveHQSettings() As Boolean
        Dim backupTime As Date = Now
        Dim timeStamp As String = Format(backupTime, "yyyy-MM-dd-HH-mm-ss")
        Dim zipFolder As String = System.IO.Path.Combine(EveHQ.Core.HQ.EveHQBackupFolder, "EveHQBackup " & timeStamp)
        Dim zipFileName As String = System.IO.Path.Combine(zipFolder, "EveHQBackup " & timeStamp & ".zip")
        Dim oldTime As Date = EveHQ.Core.HQ.EveHqSettings.EveHQBackupLast
        Dim oldResult As Integer = EveHQ.Core.HQ.EveHqSettings.EveHQBackupLastResult
        Try
            ' Save the settings file
            EveHQ.Core.HQ.WriteLogEvent("Backup: Request to save EveHQ Settings before backup")
            Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()

            ' Backup the SQL Data file if applicable
            If EveHQ.Core.HQ.EveHqSettings.DBFormat = 1 Then
                EveHQ.Core.HQ.WriteLogEvent("Backup: Backup SQL database")
                Call EveHQBackup.BackupSQLDB()
            End If

            ' Create the zip folder
            If My.Computer.FileSystem.DirectoryExists(zipFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(zipFolder)
            End If

            ' Backup the data
            EveHQ.Core.HQ.WriteLogEvent("Backup: Backup EveHQ settings")
            Dim fileFilter As String = "-\.config$;-\.dll$;-\.exe$;-\.manifest$;-\.log$;-\.sdf$;-\.mdb$;-\.pdb;-\.sdf$;-\.zip$"
            Dim zipSettings As FastZip = New FastZip()
            zipSettings.CreateZip(zipFileName, EveHQ.Core.HQ.AppDataFolder, True, fileFilter, "^(?:(?!cache).)*$")

            ' Update backup details
            EveHQ.Core.HQ.WriteLogEvent("Backup: Store EveHQ backup results")
            EveHQ.Core.HQ.EveHqSettings.EveHQBackupLast = backupTime
            EveHQ.Core.HQ.EveHqSettings.EveHQBackupLastResult = -1

            zipSettings = Nothing

            Return True
        Catch e As Exception
            ' Report the error
            Dim msg As String = "Error Performing EveHQ Backup:"
            msg &= ControlChars.CrLf & e.Message & ControlChars.CrLf
            If e.InnerException IsNot Nothing Then
                msg &= "Inner Exception: " & e.InnerException.Message & ControlChars.CrLf
            End If
            MessageBox.Show(msg, "EveHQ Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            EveHQ.Core.HQ.EveHqSettings.EveHQBackupLastResult = 0
            EveHQ.Core.HQ.EveHqSettings.EveHQBackupLast = oldTime
            ' Try and delete the zip folder
            Try
                If My.Computer.FileSystem.DirectoryExists(zipFolder) = True Then
                    My.Computer.FileSystem.DeleteDirectory(zipFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                End If
            Catch ex As Exception
                ' Delete failed - ignore!
            End Try
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Restores the EveHQ settings and associated plug-in data
    ''' </summary>
    ''' <returns>A boolean value indicating if the restore procedure was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function RestoreEveHQSettings(ByVal BackupFile As String) As Boolean

        ' Close all the open tabs first
        Dim mainTab As DevComponents.DotNetBar.TabStrip = CType(EveHQ.Core.HQ.MainForm.Controls("tabEveHQMDI"), DevComponents.DotNetBar.TabStrip)
        If mainTab.Tabs.Count > 0 Then
            For tab As Integer = mainTab.Tabs.Count - 1 To 0 Step -1
                CType(mainTab.Tabs(tab).AttachedControl, Form).Close()
            Next
        End If

        ' Try and unzip the backup file
        Try
            Dim ZipSettings As FastZip = New FastZip()
            ZipSettings.ExtractZip(BackupFile, EveHQ.Core.HQ.AppDataFolder, "")

            ' Restore the SQL Data file if applicable
            If EveHQ.Core.HQ.EveHqSettings.DBFormat = 1 Then
                Call EveHQBackup.RestoreSQLDB()
            End If

            ' Report success
            MessageBox.Show("Restore successful! EveHQ needs to be restarted for the new settings to apply - Click OK to close EveHQ.", "Restore Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' If all is good, set the exit flag
            EveHQ.Core.HQ.RestoredSettings = True

            ' Exit EveHQ
            Application.Exit()

        Catch e As Exception
            ' Report the error
            Dim msg As String = "Error Performing EveHQ Restore:"
            msg &= ControlChars.CrLf & e.Message & ControlChars.CrLf
            If e.InnerException IsNot Nothing Then
                msg &= "Inner Exception: " & e.InnerException.Message & ControlChars.CrLf
            End If
            MessageBox.Show(msg, "EveHQ Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Function

    ''' <summary>
    ''' Backs up the SQL database
    ''' </summary>
    ''' <returns>A boolean value indicating if the backup was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function BackupSQLDB() As Boolean
        Try
            Dim strSQL As String = "BACKUP DATABASE EveHQData TO DISK = '" & IO.Path.Combine(EveHQ.Core.HQ.AppDataFolder, "EveHQSQLBackup.bak") & "' WITH FORMAT;"
            EveHQ.Core.DataFunctions.SetData(strSQL)
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error backing up the SQL Database. You may need to back this up manually. The error was: " & e.Message, "SQL Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Restores the SQL database
    ''' </summary>
    ''' <returns>A boolean value indicating if the restore was successful</returns>
    ''' <remarks></remarks>
    Public Shared Function RestoreSQLDB() As Boolean
        Try
            ' Attempt a forced close of all connections to the database in order to perform a successful restore
            Dim closeSQL As String = "ALTER DATABASE EveHQData SET SINGLE_USER WITH ROLLBACK IMMEDIATE;"
            EveHQ.Core.DataFunctions.SetData(closeSQL)
            ' Now try the restore operation (NB needs to use the Master DB to avoid database in use onflicts)
            Dim strSQL As String = "USE [MASTER]; RESTORE DATABASE EveHQData FROM DISK = '" & IO.Path.Combine(EveHQ.Core.HQ.AppDataFolder, "EveHQSQLBackup.bak") & "' WITH RECOVERY;"
            EveHQ.Core.DataFunctions.SetData(strSQL)
            ' Set multi-user again, just in case the restore operation failed
            Dim openSQL As String = "ALTER DATABASE EveHQData SET MULTI_USER;"
            EveHQ.Core.DataFunctions.SetData(openSQL)
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error restoring the SQL Database. You may need to restore the database manually. The error was: " & e.Message, "SQL Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Class
