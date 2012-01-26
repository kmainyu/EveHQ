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

Public Class ProductionJobs
    Public Shared Jobs As New SortedList(Of String, ProductionJob)

    Public Shared Sub SaveProductionJobs()

        ' Write a serial version of the classes
        Dim s As New FileStream(Path.Combine(Settings.PrismFolder, "ProductionJobs.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, ProductionJobs.Jobs)
        s.Flush()
        s.Close()

    End Sub

    Public Shared Function LoadProductionJobs() As Boolean

        If My.Computer.FileSystem.FileExists(Path.Combine(Settings.PrismFolder, "ProductionJobs.bin")) = True Then
            Dim s As New FileStream(Path.Combine(Settings.PrismFolder, "ProductionJobs.bin"), FileMode.Open)
            Try
                Dim f As BinaryFormatter = New BinaryFormatter
                ProductionJobs.Jobs = CType(f.Deserialize(s), SortedList(Of String, ProductionJob))
				s.Close()
				' Run a check on job names to ensure not blank
				For Each JobName As String In ProductionJobs.Jobs.Keys
					If ProductionJobs.Jobs(JobName).JobName <> JobName Then
						ProductionJobs.Jobs(JobName).JobName = JobName
					End If
				Next
                PrismEvents.StartUpdateProductionJobs()
            Catch ex As Exception
                Dim msg As String = "There was an error trying to load the Production Jobs and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Prism will delete this file and re-initialise the settings." & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Press OK to reset the Production Jobs file." & ControlChars.CrLf
                MessageBox.Show(msg, "Invalid Production Jobs file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Try
                    s.Close()
                    My.Computer.FileSystem.DeleteFile(Path.Combine(Settings.PrismFolder, "ProductionJobs.bin"))
                Catch e As Exception
                    MessageBox.Show("Unable to delete the ProductionJobs.bin file. Please delete this manually before proceeding", "Delete File Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End Try
            End Try
        End If

        Return True

    End Function
End Class
