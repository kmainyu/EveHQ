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

Imports System.IO
Imports System.Windows.Forms
Imports DevComponents.DotNetBar
Imports Microsoft.VisualBasic.FileIO

Imports System.Text
Imports Ionic.Zip


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
            Using zip As New ZipFile(zipFileName, UTF8Encoding.UTF8)
                zip.AddFile(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json"), "")
                zip.AddFile(Path.Combine(HQ.AppDataFolder, "EveHQData.db3"), "")

                'TODO: Plugin backup should be a delegate method that can be called to get files for backup
                Dim fittingsFile = Path.Combine(HQ.AppDataFolder, "HQF", "Fittings.json")
                If (File.Exists(fittingsFile)) Then
                    zip.AddFile(fittingsFile, "HQF")
                End If

                Dim damageProfiles = Path.Combine(HQ.AppDataFolder, "HQF", "HQFDamageProfiles.json")
                If (File.Exists(damageProfiles)) Then
                    zip.AddFile(damageProfiles, "HQF")
                End If

                Dim pilotSettings = Path.Combine(HQ.AppDataFolder, "HQF", "HQFPilotSettings.json")
                If (File.Exists(pilotSettings)) Then
                    zip.AddFile(pilotSettings, "HQF")
                End If

                Dim defenceProfiles = Path.Combine(HQ.AppDataFolder, "HQF", "HQFDefenceProfiles.json")
                If (File.Exists(defenceProfiles)) Then
                    zip.AddFile(defenceProfiles, "HQF")
                End If

                Dim hqSettings = Path.Combine(HQ.AppDataFolder, "HQF", "HQFSettings.json")
                If (File.Exists(hqSettings)) Then
                    zip.AddFile(hqSettings, "HQF")
                End If

                'Prism data files
                Dim batchJobs = Path.Combine(HQ.AppDataFolder, "Prism", "BatchJobs.json")
                If (File.Exists(batchJobs)) Then
                    zip.AddFile(batchJobs, "Prism")
                End If

                Dim blueprints = Path.Combine(HQ.AppDataFolder, "Prism", "OwnerBlueprints.json")
                If (File.Exists(blueprints)) Then
                    zip.AddFile(blueprints, "Prism")
                End If

                Dim prismSettings = Path.Combine(HQ.AppDataFolder, "Prism", "PrismSettings.json")
                If (File.Exists(prismSettings)) Then
                    zip.AddFile(prismSettings, "Prism")
                End If

                Dim productionJobs = Path.Combine(HQ.AppDataFolder, "Prism", "ProductionJobs.json")
                If (File.Exists(productionJobs)) Then
                    zip.AddFile(productionJobs, "Prism")
                End If

                zip.Save()
                ' Update backup details
                HQ.WriteLogEvent("Backup: Store EveHQ backup results")
                HQ.Settings.EveHqBackupLast = backupTime
                HQ.Settings.EveHqBackupLastResult = -1
            End Using


            Return True
        Catch e As Exception
            ' Report the error
            Dim msg As String = "Error Performing EveHQ Backup:"
            msg &= ControlChars.CrLf & e.Message & ControlChars.CrLf
            If e.InnerException IsNot Nothing Then
                msg &= "Inner Exception: " & e.InnerException.Message & ControlChars.CrLf
            End If
            MessageBox.Show(msg, "EveHQ Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            HQ.Settings.EveHqBackupLastResult = 0
            HQ.Settings.EveHqBackupLast = oldTime
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

            Using zip As ZipFile = ZipFile.Read(backupFile)

                zip.ExtractAll(HQ.AppDataFolder, ExtractExistingFileAction.OverwriteSilently)
             
                ' Report success
                MessageBox.Show("Restore successful! EveHQ needs to be restarted for the new settings to apply - Click OK to close EveHQ.", "Restore Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' If all is good, set the exit flag
                HQ.RestoredSettings = True
            End Using

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
