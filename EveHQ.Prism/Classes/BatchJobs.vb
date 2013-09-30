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
Imports Newtonsoft.Json

Public Class BatchJobs

    Public Shared Jobs As New SortedList(Of String, BatchJob)

    Private Const MainFileName As String = "BatchJobs.json"
    Private Shared ReadOnly LockObj As New Object

    Public Shared Sub SaveBatchJobs()

        SyncLock LockObj
            Dim newFile As String = Path.Combine(Settings.PrismFolder, MainFileName)
            Dim tempFile As String = Path.Combine(Settings.PrismFolder, MainFileName & ".temp")

            ' Create a JSON string for writing
            Dim json As String = JsonConvert.SerializeObject(Jobs, Newtonsoft.Json.Formatting.Indented)

            ' Write the JSON version of the settings
            Try
                Using s As New StreamWriter(tempFile, False)
                    s.Write(json)
                    s.Flush()
                End Using

                If File.Exists(newFile) Then
                    File.Delete(newFile)
                End If

                File.Move(tempFile, newFile)

            Catch e As Exception

            End Try

        End SyncLock

    End Sub

    Public Shared Function LoadBatchJobs() As Boolean
        SyncLock LockObj

            If My.Computer.FileSystem.FileExists(Path.Combine(Settings.PrismFolder, mainFileName)) = True Then
                Try
                    Using s As New StreamReader(Path.Combine(Settings.PrismFolder, mainFileName))
                        Dim json As String = s.ReadToEnd
                        Jobs = JsonConvert.DeserializeObject(Of SortedList(Of String, BatchJob))(json)
                        PrismEvents.StartUpdateBatchJobs()
                    End Using
                Catch ex As Exception
                    Dim msg As String = "There was an error trying to load the Batch Jobs and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Prism will delete this file and re-initialise the file." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Press OK to reset the batch Jobs file." & ControlChars.CrLf
                    MessageBox.Show(msg, "Invalid Batch Jobs file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End Try
            End If

            Return True

        End SyncLock
    End Function
End Class
