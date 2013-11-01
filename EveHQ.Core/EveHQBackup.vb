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
Imports System.Windows.Forms
Imports DevComponents.DotNetBar
Imports Microsoft.VisualBasic.FileIO
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
        Dim nextBackup As Date = HQ.Settings.EveHQBackupStart
        If HQ.Settings.EveHQBackupLast > nextBackup Then
            nextBackup = HQ.Settings.EveHQBackupLast
        End If
        nextBackup = DateAdd(DateInterval.Day, HQ.Settings.EveHQBackupFreq, nextBackup)
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
        Dim zipFolder As String = Path.Combine(HQ.EveHQBackupFolder, "EveHQBackup " & timeStamp)
        Dim zipFileName As String = Path.Combine(zipFolder, "EveHQBackup " & timeStamp & ".zip")
        Dim oldTime As Date = HQ.Settings.EveHqBackupLast
        Try
            ' Save the settings file
            HQ.WriteLogEvent("Backup: Request to save EveHQ Settings before backup")
            Call HQ.Settings.Save()

           ' Create the zip folder
            If My.Computer.FileSystem.DirectoryExists(zipFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(zipFolder)
            End If

            ' Backup the data
            ' TODO: Replace ZIP logic here from Fast zip to Ionic like the Market providers use.
            HQ.WriteLogEvent("Backup: Backup EveHQ settings")
            Const fileFilter As String = "-\.config$;-\.dll$;-\.exe$;-\.manifest$;-\.log$;-\.sdf$;-\.mdb$;-\.pdb;-\.sdf$;-\.zip$"
            Dim zipSettings As FastZip = New FastZip()
            zipSettings.CreateZip(zipFileName, HQ.AppDataFolder, True, fileFilter, "^(?:(?!cache).)*$")

            ' Update backup details
            HQ.WriteLogEvent("Backup: Store EveHQ backup results")
            HQ.Settings.EveHQBackupLast = backupTime
            HQ.Settings.EveHQBackupLastResult = -1
            Return True
        Catch e As Exception
            ' Report the error
            Dim msg As String = "Error Performing EveHQ Backup:"
            msg &= ControlChars.CrLf & e.Message & ControlChars.CrLf
            If e.InnerException IsNot Nothing Then
                msg &= "Inner Exception: " & e.InnerException.Message & ControlChars.CrLf
            End If
            MessageBox.Show(msg, "EveHQ Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            HQ.Settings.EveHQBackupLastResult = 0
            HQ.Settings.EveHQBackupLast = oldTime
            ' Try and delete the zip folder
            Try
                If My.Computer.FileSystem.DirectoryExists(zipFolder) = True Then
                    My.Computer.FileSystem.DeleteDirectory(zipFolder, DeleteDirectoryOption.DeleteAllContents)
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
    Public Shared Function RestoreEveHQSettings(ByVal backupFile As String) As Boolean

        ' Close all the open tabs first
        Dim mainTab As TabStrip = CType(HQ.MainForm.Controls("tabEveHQMDI"), TabStrip)
        If mainTab.Tabs.Count > 0 Then
            For tab As Integer = mainTab.Tabs.Count - 1 To 0 Step -1
                CType(mainTab.Tabs(tab).AttachedControl, Form).Close()
            Next
        End If

        ' Try and unzip the backup file
        Try
            Dim zipSettings As FastZip = New FastZip()
            zipSettings.ExtractZip(backupFile, HQ.AppDataFolder, "")

            ' Report success
            MessageBox.Show("Restore successful! EveHQ needs to be restarted for the new settings to apply - Click OK to close EveHQ.", "Restore Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' If all is good, set the exit flag
            HQ.RestoredSettings = True

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

End Class
