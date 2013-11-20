' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2012  EveHQ Development Team
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
Imports EveHQ.Prism.Classes
Imports Newtonsoft.Json

Namespace BPCalc

    Public Class Jobs
        Public Shared JobList As New SortedList(Of String, Job)

        Private Const MainFileName As String = "ProductionJobs.json"
        Private Shared ReadOnly LockObj As New Object

        Public Shared Sub Save()

            SyncLock LockObj
                Dim newFile As String = Path.Combine(PrismSettings.PrismFolder, MainFileName)
                Dim tempFile As String = Path.Combine(PrismSettings.PrismFolder, MainFileName & ".temp")

                ' Create a JSON string for writing
                Dim json As String = JsonConvert.SerializeObject(JobList, Formatting.Indented)

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

        Public Shared Function Load() As Boolean

            Dim jobsUpdated As Boolean = False

            SyncLock LockObj

                If My.Computer.FileSystem.FileExists(Path.Combine(PrismSettings.PrismFolder, MainFileName)) = True Then

                    Try
                        Using s As New StreamReader(Path.Combine(PrismSettings.PrismFolder, MainFileName))
                            Dim json As String = s.ReadToEnd
                            JobList = JsonConvert.DeserializeObject(Of SortedList(Of String, Job))(json)
                        End Using

                        ' Run a check on job names to ensure not blank
                        For Each jobName As String In JobList.Keys
                            If JobList(jobName).JobName <> jobName Then
                                JobList(jobName).JobName = jobName
                            End If
                        Next

                        ' Check resources of the blueprints to see if anything has changed in the static data
                        For Each checkJob As Job In JobList.Values
                            Dim jobChanged As Boolean = False
                            If checkJob.CurrentBlueprint IsNot Nothing Then
                                Dim bpID As Integer = checkJob.CurrentBlueprint.Id
                                Dim bp As EveData.Blueprint = EveData.StaticData.Blueprints(bpID)
                                For Each typeID As Integer In checkJob.Resources.Keys
                                    If bp.Resources.ContainsKey(1) Then
                                        If bp.Resources(1).ContainsKey(typeID) Then
                                            If checkJob.Resources(typeID).BaseUnits = bp.Resources(1).Item(typeID).BaseMaterial Then
                                                If checkJob.Resources(typeID).PerfectUnits = bp.Resources(1).Item(typeID).Quantity Then
                                                    Continue For
                                                Else
                                                    jobChanged = True
                                                    Exit For
                                                End If
                                            Else
                                                jobChanged = True
                                                Exit For
                                            End If
                                        Else
                                            jobChanged = True
                                            Exit For
                                        End If
                                    Else
                                        jobChanged = True
                                        Exit For
                                    End If
                                Next
                                If jobChanged = True Then
                                    ' Update the job
                                    checkJob.CurrentBlueprint = OwnedBlueprint.CopyFromBlueprint(EveData.StaticData.Blueprints(bpID))
                                    checkJob.CalculateResourceRequirements(True, checkJob.BlueprintOwner)
                                    checkJob.Cost = checkJob.CalculateCost
                                    jobsUpdated = True
                                End If
                            Else
                                ' Try and update the job using the type id
                                If EveData.StaticData.Blueprints.ContainsKey(checkJob.TypeID) Then
                                    checkJob.CurrentBlueprint = OwnedBlueprint.CopyFromBlueprint(EveData.StaticData.Blueprints(checkJob.TypeID))
                                    checkJob.CalculateResourceRequirements(True, checkJob.BlueprintOwner)
                                    checkJob.Cost = checkJob.CalculateCost
                                    checkJob.CurrentBlueprint.AssetId = checkJob.CurrentBlueprint.Id
                                    jobsUpdated = True
                                End If
                            End If
                        Next

                        PrismEvents.StartUpdateProductionJobs()
                    Catch ex As Exception
                        Dim msg As String = "There was an error trying to load the Production Jobs and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "Prism will rename this file (and add a .bad suffix) and re-initialise the settings." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "Press OK to reset the Production Jobs file." & ControlChars.CrLf
                        MessageBox.Show(msg, "Invalid Production Jobs file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Try
                            File.Move(Path.Combine(PrismSettings.PrismFolder, MainFileName), Path.Combine(PrismSettings.PrismFolder, MainFileName & ".bad"))
                        Catch e As Exception
                            MessageBox.Show("Unable to delete the ProductionJobs.json file. Please delete this manually before proceeding", "Delete File Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return jobsUpdated
                        End Try
                    End Try
                End If

                Return jobsUpdated

            End SyncLock

        End Function

    End Class

End Namespace