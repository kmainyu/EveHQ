Imports EveHQ.EveData

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

Public Class Locations
    Public Shared Function GetLocationNameFromID(ByVal locID As Integer) As String
        If locID >= 66000000 Then
            If locID < 66014933 Then
                locID = locID - 6000001
            Else
                locID = locID - 6000000
            End If
        End If
        Dim newLocation As Station
        If locID >= 61000000 And locID <= 61999999 Then
            If StaticData.Stations.ContainsKey(locID) = True Then
                ' Known Outpost
                newLocation = StaticData.Stations(locID)
                Return newLocation.StationName
            Else
                ' Unknown outpost!
                newLocation = New Station
                newLocation.StationId = locID
                newLocation.StationName = "Unknown Outpost"
                newLocation.SystemId = 0
                Return newLocation.StationName
            End If
        Else
            If locID < 60000000 Then
                If StaticData.Stations.ContainsKey(locID) Then
                    Dim newSystem As SolarSystem = StaticData.SolarSystems(locID)
                    Return newSystem.Name
                Else
                    Return "Unknown Location"
                End If
            Else
                newLocation = StaticData.Stations(locID)
                If newLocation IsNot Nothing Then
                    Return newLocation.StationName
                Else
                    ' Unknown system/station!
                    newLocation = New Station
                    newLocation.StationId = locID
                    newLocation.StationName = "Unknown Location"
                    newLocation.SystemId = 0
                    Return newLocation.StationName
                End If
            End If
        End If
    End Function
End Class
