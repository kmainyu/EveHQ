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
Imports EveHQ.EveData
Imports Newtonsoft.Json
Imports ProtoBuf

''' <summary>
''' Class for holding and handling the collection of custom classes for the HQF Editors
''' </summary>
''' <remarks></remarks>
Public Class CustomHQFClasses

#Region "Custom Ship Classes"
    ''' <summary>
    ''' A SortedList containing the collection of custom ship classes for the HQF Ship Editor
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared CustomShipClasses As New SortedList(Of String, CustomShipClass)

    ''' <summary>
    ''' Method for deserializing the custom ship classes into internal storage
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub LoadCustomShipClasses()

        If My.Computer.FileSystem.FileExists(Path.Combine(PluginSettings.HQFFolder, "CustomShipClasses.json")) = True Then
            Try
                Using s As New StreamReader(Path.Combine(PluginSettings.HQFFolder, "CustomShipClasses.json"))
                    Dim json As String = s.ReadToEnd
                    CustomShipClasses = JsonConvert.DeserializeObject(Of SortedList(Of String, CustomShipClass))(json)
                End Using
            Catch ex As Exception
                MessageBox.Show("There was an error loading the Custom Ship Classes file. The file appears corrupt, so it cannot be loaded at this time.")
            End Try
        End If

    End Sub

    ''' <summary>
    ''' Method for serializing the custom ship classes onto external storage
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SaveCustomShipClasses()

        ' Create a JSON string for writing
        Dim json As String = JsonConvert.SerializeObject(CustomShipClasses, Formatting.Indented)

        ' Write the JSON version of the settings
        Try
            Using s As New StreamWriter(Path.Combine(PluginSettings.HQFFolder, "CustomShipClasses.json"), False)
                s.Write(json)
                s.Flush()
            End Using
        Catch e As Exception
        End Try

    End Sub
#End Region

#Region "Custom Ships"

    ''' <summary>
    ''' A SortedList containing the collection of custom ships for the HQF Ship Editor
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared CustomShips As New SortedList(Of String, CustomShip)

    ''' <summary>
    ''' A SortedList(Of ShipID, ShipName) containing the relationship of custom ship ID to name
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared CustomShipIDs As New SortedList(Of Integer, String)

    ''' <summary>
    ''' Method for deserializing the custom ships into internal storage
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub LoadCustomShips()

        If My.Computer.FileSystem.FileExists(Path.Combine(PluginSettings.HQFFolder, "CustomShips.json")) = True Then
            Try
                Using s As New StreamReader(Path.Combine(PluginSettings.HQFFolder, "CustomShips.json"))
                    Dim json As String = s.ReadToEnd
                    CustomShips = JsonConvert.DeserializeObject(Of SortedList(Of String, CustomShip))(json)
                    CustomShipIDs.Clear()
                    For Each cShip As CustomShip In CustomShips.Values
                        CustomShipIDs.Add(cShip.ID, cShip.Name)
                    Next
                End Using
            Catch ex As Exception
                MessageBox.Show("There was an error loading the Custom Ships file. The file appears corrupt, so it cannot be loaded at this time.")
            End Try
        End If

    End Sub

    ''' <summary>
    ''' Method for serializing the custom ships onto external storage
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SaveCustomShips()

        ' Create a JSON string for writing
        Dim json As String = JsonConvert.SerializeObject(CustomShips, Formatting.Indented)

        ' Write the JSON version of the settings
        Try
            Using s As New StreamWriter(Path.Combine(PluginSettings.HQFFolder, "CustomShips.json"), False)
                s.Write(json)
                s.Flush()
            End Using
        Catch e As Exception
        End Try

    End Sub

    ''' <summary>
    ''' Method for incorporating the custom ship data into the main EveHQ routines
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub ImplementCustomShips()
        ' Recalculate the ship bonuses and add in all the data from the custom ships

        ' First rebuild the standard bonus data
        Call Engine.BuildShipBonusesMap()

        ' Rebuild the ship lists
        ShipLists.ShipListKeyID.Clear()
        ShipLists.ShipListKeyName.Clear()
        Dim s As FileStream
        If My.Computer.FileSystem.FileExists(Path.Combine(PluginSettings.HQFCacheFolder, "ships.dat")) = True Then
            s = New FileStream(Path.Combine(PluginSettings.HQFCacheFolder, "ships.dat"), FileMode.Open)
            Try
                ShipLists.ShipList = Serializer.Deserialize(Of Dictionary(Of String, Ship))(s)
                s.Close()
                For Each cShip As Ship In ShipLists.ShipList.Values
                    ShipLists.ShipListKeyID.Add(cShip.ID, cShip.Name)
                    ShipLists.ShipListKeyName.Add(cShip.Name, cShip.ID)
                Next
            Catch sex As Exception
                s.Close()
            End Try
            s.Dispose()
        End If

        ' Then add in all the data from the custom ships
        For Each cShip As CustomShip In CustomShips.Values
            ' Add the bonuses
            Engine.ShipBonusesMap.Add(cShip.ID, cShip.Bonuses)
            ' Add the ship data
            ShipLists.ShipList.Add(cShip.Name, cShip.ShipData)
            ShipLists.ShipListKeyID.Add(cShip.ID, cShip.Name)
            ShipLists.ShipListKeyName.Add(cShip.Name, cShip.ID)
        Next

        ' Rebuild the ship effects
        For Each cShip As CustomShip In CustomShips.Values
            cShip.ShipData.GlobalAffects.Clear()
            For Each neweffect As ShipEffect In cShip.Bonuses
                Dim affectingName As String
                affectingName = cShip.Name
                If neweffect.IsPerLevel = False Then
                    affectingName &= ";Ship Role;"
                Else
                    affectingName &= ";Ship Bonus;"
                End If
                affectingName &= Attributes.AttributeQuickList(neweffect.AffectedAtt).ToString
                If neweffect.IsPerLevel = False Then
                    affectingName &= ";"
                Else
                    affectingName &= ";" & StaticData.Types(neweffect.AffectingID).Name
                End If
                cShip.ShipData.GlobalAffects.Add(affectingName)
            Next
        Next
    End Sub

#End Region

End Class

