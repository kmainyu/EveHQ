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
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Windows.Forms

Public Class BatchJobs
    Public Shared Jobs As New SortedList(Of String, BatchJob)

    Private Shared LockObj As New Object

    Private Const BatchJobsName As String = "BatchJobs.bin"

    Public Shared Sub SaveBatchJobs()
        SyncLock LockObj
            Dim batchFile As String = Path.Combine(Settings.PrismFolder, BatchJobsName)
            Dim batchTemp As String = Path.Combine(Settings.PrismFolder, BatchJobsName & ".temp")

            ' Write a serial version of the classes
            Using s As New FileStream(batchTemp, FileMode.Create)
                Dim f As New BinaryFormatter
                f.Serialize(s, BatchJobs.Jobs)
                s.Flush()
            End Using

            If (File.Exists(batchFile)) Then
                File.Delete(batchFile)
            End If

            File.Move(batchTemp, batchFile)

        End SyncLock
    End Sub

    Public Shared Function LoadBatchJobs() As Boolean
        SyncLock LockObj
            If My.Computer.FileSystem.FileExists(Path.Combine(Settings.PrismFolder, "BatchJobs.bin")) = True Then
                Dim s As New FileStream(Path.Combine(Settings.PrismFolder, "BatchJobs.bin"), FileMode.Open)
                Try
                    Dim f As BinaryFormatter = New BinaryFormatter
                    BatchJobs.Jobs = CType(f.Deserialize(s), SortedList(Of String, BatchJob))
                    s.Close()
                    PrismEvents.StartUpdateBatchJobs()
                Catch ex As Exception
                    Dim msg As String = "There was an error trying to load the Batch Jobs and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Prism will delete this file and re-initialise the file." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Press OK to reset the batch Jobs file." & ControlChars.CrLf
                    MessageBox.Show(msg, "Invalid Batch Jobs file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Try
                        s.Close()
                        My.Computer.FileSystem.DeleteFile(Path.Combine(Settings.PrismFolder, "BatchJobs.bin"))
                    Catch e As Exception
                        MessageBox.Show("Unable to delete the BatchJobs.bin file. Please delete this manually before proceeding", "Delete File Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    End Try
                End Try
            End If

            Return True
        End SyncLock
    End Function
End Class
