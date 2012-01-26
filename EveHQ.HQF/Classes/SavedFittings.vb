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

''' <summary>
''' Class used to serialize fittings onto storage
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class SavedFittings

    Private Shared SavedFittingList As New SortedList(Of String, SavedFitting)  ' Key = FittingKey

    ''' <summary>
    ''' Loads the fittings from storage and copies them ready for use
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub LoadFittings()
        ' Load the fittings from the binary file
        Fittings.FittingList.Clear()
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "Fittings.bin")) = True Then
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "Fittings.bin"), FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            SavedFittingList = CType(f.Deserialize(s), SortedList(Of String, SavedFitting))
            s.Close()
        End If
        ' Copy the saved fittings ready for use
        Call CopySavedFittings()
    End Sub

    ''' <summary>
    ''' Saves fittings into storage
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SaveFittings()
        Try
            ' Prepare the list of fittings to save
            Call PrepareFittingsForSaving()
            ' Save fittings as a binary file
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "Fittings.bin"), FileMode.Create)
            Dim f As New BinaryFormatter
            f.Serialize(s, SavedFittingList)
            s.Flush()
            s.Close()
        Catch ex As Exception
            Windows.Forms.MessageBox.Show("There was an error saving the fittings file. The error was: " & ex.Message, "Save Fittings Failed :(", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
        End Try
    End Sub

    ''' <summary>
    ''' Copies all instances of Fitting into the SavedFittings collection ready for saving
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub PrepareFittingsForSaving()
        SavedFittingList.Clear()
        For Each fit As Fitting In Fittings.FittingList.Values
            SavedFittingList.Add(fit.KeyName, CreateSavedFittingFromFitting(fit))
        Next
    End Sub

    ''' <summary>
    ''' Copies all instances of SavedFitting into the Fittings collection ready for processing
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub CopySavedFittings()
        Fittings.FittingList.Clear()
        For Each fit As SavedFitting In SavedFittingList.Values
            Fittings.FittingList.Add(fit.KeyName, CreateFittingFromSavedFitting(fit))
        Next
    End Sub

    ''' <summary>
    ''' Copies an instance of a Fitting to a SavedFitting
    ''' </summary>
    ''' <param name="Fit">The instance of a Fitting class to convert</param>
    ''' <returns>An instance of the SavedFitting class</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateSavedFittingFromFitting(ByVal Fit As Fitting) As SavedFitting
        Dim SavedFit As New SavedFitting
        SavedFit.ShipName = Fit.ShipName
        SavedFit.FittingName = Fit.FittingName
        SavedFit.PilotName = Fit.PilotName
        SavedFit.DamageProfileName = Fit.DamageProfileName
        SavedFit.Modules = Fit.Modules
        SavedFit.Drones = Fit.Drones
        SavedFit.Items = Fit.Items
        SavedFit.Ships = Fit.Ships
        SavedFit.ImplantGroup = Fit.ImplantGroup
        SavedFit.Implants = Fit.Implants
        SavedFit.Boosters = Fit.Boosters
        SavedFit.WHEffect = Fit.WHEffect
        SavedFit.WHLevel = Fit.WHLevel
        SavedFit.FleetEffects = Fit.FleetEffects
        SavedFit.RemoteEffects = Fit.RemoteEffects
        Return SavedFit
    End Function

    ''' <summary>
    ''' Copies an instance of a SavedFitting to a Fitting
    ''' </summary>
    ''' <param name="Fit">The instance of the SavedFitting class to convert</param>
    ''' <returns>An instance of the Fitting class</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateFittingFromSavedFitting(ByVal Fit As SavedFitting) As Fitting
        Dim NewFit As New Fitting(Fit.ShipName, Fit.FittingName, Fit.PilotName)
        NewFit.ShipName = Fit.ShipName
        NewFit.FittingName = Fit.FittingName
        NewFit.PilotName = Fit.PilotName
        NewFit.DamageProfileName = Fit.DamageProfileName
        NewFit.Modules = Fit.Modules
        NewFit.Drones = Fit.Drones
        NewFit.Items = Fit.Items
        NewFit.Ships = Fit.Ships
        NewFit.ImplantGroup = Fit.ImplantGroup
        NewFit.Implants = Fit.Implants
        NewFit.Boosters = Fit.Boosters
        NewFit.WHEffect = Fit.WHEffect
        NewFit.WHLevel = Fit.WHLevel
        NewFit.FleetEffects = Fit.FleetEffects
        NewFit.RemoteEffects = Fit.RemoteEffects
        Return NewFit
    End Function

    ''' <summary>
    ''' Converts the old fittings file to the new version
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub ConvertOldFittingsFile()
        ' Step 1: Load fittings file
        ' Step 2: Cycle through fittings, creating a new one for each
        ' Step 3: Save new fittings file
        ' Step 4: Rename old fittings file

        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin")) = True Then

            ' Step 1
            Dim TempFittings As New SortedList
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin"), FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            TempFittings = CType(f.Deserialize(s), SortedList)
            s.Close()
            s.Dispose()

            ' Step 2
            Dim TempNewFittings As New SortedList(Of String, SavedFitting)
            For Each TempFittingName As String In TempFittings.Keys
                Dim TempFitting As ArrayList = CType(TempFittings(TempFittingName), ArrayList)
                Dim SavedFit As Fitting = Fittings.ConvertOldFitToNewFit(TempFittingName, TempFitting)
                TempNewFittings.Add(SavedFit.KeyName, SavedFittings.CreateSavedFittingFromFitting(SavedFit))
            Next

            ' Step 3
            Dim ss As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "Fittings.bin"), FileMode.Create)
            Dim ff As New BinaryFormatter
            ff.Serialize(ss, TempNewFittings)
            ss.Flush()
            ss.Close()
            ss.Dispose()

            ' Step 4
            Try
                My.Computer.FileSystem.RenameFile(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin"), "HQFFittings.old")
            Catch e As Exception
                ' File exists - try one more rename
                Try
                    My.Computer.FileSystem.RenameFile(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin"), "HQFFittings.bin.old")
                Catch ex As Exception
                    ' Just exit
                End Try
            End Try

        End If

    End Sub

End Class

