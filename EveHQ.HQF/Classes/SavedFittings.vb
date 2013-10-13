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
       
       ' Check for the profiles file so we can load it
        If My.Computer.FileSystem.FileExists(Path.Combine(Settings.HQFFolder, "Fittings.json")) = True Then
            Try
                Using s As New StreamReader(Path.Combine(Settings.HQFFolder, "Fittings.json"))
                    Dim json As String = s.ReadToEnd
                    SavedFittingList = JsonConvert.DeserializeObject(Of SortedList(Of String, SavedFitting))(json)
                End Using
            Catch ex As Exception
                MessageBox.Show("There was an error loading the Fittings file. The file appears corrupt, so it cannot be loaded at this time.")
            End Try
        End If

        ' Copy the saved fittings ready for use
        Call CopySavedFittings()

    End Sub

    ''' <summary>
    ''' Saves fittings into storage.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SaveFittings()

        ' Prepare the list of fittings to save
        Call PrepareFittingsForSaving()

        ' Create a JSON string for writing
        Dim json As String = JsonConvert.SerializeObject(SavedFittingList, Newtonsoft.Json.Formatting.Indented)

        ' Write the JSON version of the fittings
        Try
            Using s As New StreamWriter(Path.Combine(Settings.HQFFolder, "Fittings.json"), False)
                s.Write(json)
                s.Flush()
            End Using
        Catch e As Exception
            Windows.Forms.MessageBox.Show("There was an error saving the fittings file. The error was: " & e.Message, "Save Fittings Failed :(", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
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
            Dim loadedFitting As Fitting = CreateFittingFromSavedFitting(fit)
            If loadedFitting IsNot Nothing Then
                Fittings.FittingList.Add(fit.KeyName, loadedFitting)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Copies an instance of a Fitting to a SavedFitting
    ''' </summary>
    ''' <param name="fit">The instance of a Fitting class to convert</param>
    ''' <returns>An instance of the SavedFitting class</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateSavedFittingFromFitting(ByVal fit As Fitting) As SavedFitting
        Dim savedFit As New SavedFitting
        savedFit.ShipName = Fit.ShipName
        savedFit.FittingName = Fit.FittingName
        savedFit.PilotName = Fit.PilotName
        savedFit.DamageProfileName = Fit.DamageProfileName
        savedFit.Modules = Fit.Modules
        savedFit.Drones = Fit.Drones
        savedFit.Items = Fit.Items
        savedFit.Ships = Fit.Ships
        savedFit.ImplantGroup = Fit.ImplantGroup
        savedFit.Implants = Fit.Implants
        savedFit.Boosters = Fit.Boosters
        savedFit.WHEffect = Fit.WHEffect
        savedFit.WHLevel = Fit.WHLevel
        savedFit.FleetEffects = Fit.FleetEffects
        savedFit.RemoteEffects = fit.RemoteEffects
        savedFit.Notes = fit.Notes
        savedFit.Tags = fit.Tags
        savedFit.Rating = fit.rating
        Return savedFit
    End Function

    ''' <summary>
    ''' Copies an instance of a SavedFitting to a Fitting
    ''' </summary>
    ''' <param name="fit">The instance of the SavedFitting class to convert</param>
    ''' <returns>An instance of the Fitting class</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateFittingFromSavedFitting(ByVal fit As SavedFitting) As Fitting
        Select Case Fit.ShipName
            Case "Badger Mark II"
                Fit.ShipName = "Tayra"
            Case "Iteron"
                Fit.ShipName = "Nereus"
            Case "Iteron Mark II"
                Fit.ShipName = "Kryos"
            Case "Iteron Mark III"
                Fit.ShipName = "Epithal"
            Case "Iteron Mark IV"
                Fit.ShipName = "Miasmos"
        End Select
        If ShipLists.shipList.ContainsKey(Fit.ShipName) Then
            Dim newFit As New Fitting(Fit.ShipName, Fit.FittingName, Fit.PilotName)
            newFit.DamageProfileName = Fit.DamageProfileName
            newFit.Modules = Fit.Modules
            newFit.Drones = Fit.Drones
            newFit.Items = Fit.Items
            newFit.Ships = Fit.Ships
            newFit.ImplantGroup = Fit.ImplantGroup
            newFit.Implants = Fit.Implants
            newFit.Boosters = Fit.Boosters
            newFit.WHEffect = Fit.WHEffect
            newFit.WHLevel = Fit.WHLevel
            newFit.FleetEffects = Fit.FleetEffects
            newFit.RemoteEffects = fit.RemoteEffects
            newFit.Notes = fit.Notes
            newFit.Tags = fit.Tags
            newFit.rating = fit.Rating
            Return newFit
        Else
            Return Nothing
        End If
    End Function

End Class

