' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "CustomShipClasses.bin")) = True Then
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "CustomShipClasses.bin"), FileMode.Open)
            Try
                Dim f As BinaryFormatter = New BinaryFormatter
                CustomShipClasses = CType(f.Deserialize(s), SortedList(Of String, CustomShipClass))
                s.Close()
            Catch sex As Exception
                s.Close()
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Method for serializing the custom ship classes onto external storage
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SaveCustomShipClasses()
        ' Save ships
        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "CustomShipClasses.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, CustomShipClasses)
        s.Flush()
        s.Close()
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
    Public Shared CustomShipIDs As New SortedList(Of String, String)

    ''' <summary>
    ''' Method for deserializing the custom ships into internal storage
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub LoadCustomShips()
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "CustomShips.bin")) = True Then
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "CustomShips.bin"), FileMode.Open)
            Try
                Dim f As BinaryFormatter = New BinaryFormatter
                CustomShips = CType(f.Deserialize(s), SortedList(Of String, CustomShip))
                CustomShipIDs.Clear()
                For Each cShip As CustomShip In CustomShips.Values
                    CustomShipIDs.Add(cShip.ID, cShip.Name)
                Next
                s.Close()
            Catch sex As Exception
                s.Close()
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Method for serializing the custom ships onto external storage
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SaveCustomShips()
        ' Save ships
        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "CustomShips.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, CustomShips)
        s.Flush()
        s.Close()
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
        ShipLists.shipListKeyID.Clear()
        ShipLists.shipListKeyName.Clear()
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "ships.bin")) = True Then
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "ships.bin"), FileMode.Open)
            Try
                Dim f As BinaryFormatter = New BinaryFormatter
                ShipLists.shipList = CType(f.Deserialize(s), SortedList)
                s.Close()
                For Each cShip As Ship In ShipLists.shipList.Values
                    ShipLists.shipListKeyID.Add(cShip.ID, cShip.Name)
                    ShipLists.shipListKeyName.Add(cShip.Name, cShip.ID)
                Next
            Catch sex As Exception
                s.Close()
                MessageBox.Show("Unable to rebuild ship data. The error was: " & sex.Message, "Error Rebuilding Ship Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try
        Else
            MessageBox.Show("Unable to rebuild ship data. The ship cache file could not be found.", "Error Rebuilding Ship Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Then add in all the data from the custom ships
        For Each cShip As CustomShip In CustomHQFClasses.CustomShips.Values
            ' Add the bonuses
            Engine.ShipBonusesMap.Add(cShip.ID, cShip.Bonuses)
            ' Add the ship data
            ShipLists.shipList.Add(cShip.Name, cShip.ShipData)
            ShipLists.shipListKeyID.Add(cShip.ID, cShip.Name)
            ShipLists.shipListKeyName.Add(cShip.Name, cShip.ID)
        Next

        ' Rebuild the ship effects
        For Each cShip As CustomShip In CustomHQFClasses.CustomShips.Values
            cShip.ShipData.GlobalAffects.Clear()
            For Each neweffect As ShipEffect In cShip.Bonuses
                Dim AffectingName As String = ""
                AffectingName = cShip.Name
                If neweffect.IsPerLevel = False Then
                    AffectingName &= ";Ship Role;"
                Else
                    AffectingName &= ";Ship Bonus;"
                End If
                AffectingName &= HQF.Attributes.AttributeQuickList(neweffect.AffectedAtt.ToString).ToString
                If neweffect.IsPerLevel = False Then
                    AffectingName &= ";"
                Else
                    AffectingName &= ";" & EveHQ.Core.HQ.itemData(neweffect.AffectingID.ToString).Name
                End If
                cShip.ShipData.GlobalAffects.Add(AffectingName)
            Next
        Next
    End Sub

#End Region



End Class

