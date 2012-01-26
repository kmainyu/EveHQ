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

Public Class Station
    Public stationID As Long
    Public stationName As String
    Public systemID As Long
    Public constID As Long
    Public regionID As Long
    Public corpID As Long
    Public stationTypeID As Long
    Public operationID As Long
    Public refiningEff As Double
End Class

Public Class Corporation
    Public CorpID As Long
    Public CorpName As String
End Class

Public Class Locations
    Public Shared Function GetLocationNameFromID(ByVal locID As String) As String
        If CDbl(locID) >= 66000000 Then
            If CDbl(locID) < 66014933 Then
                locID = (CDbl(locID) - 6000001).ToString
            Else
                locID = (CDbl(locID) - 6000000).ToString
            End If
        End If
        Dim newLocation As Prism.Station
        If CDbl(locID) >= 61000000 And CDbl(locID) <= 61999999 Then
            If PlugInData.stations.Contains(locID) = True Then
                ' Known Outpost
                newLocation = CType(PlugInData.stations(locID), Prism.Station)
                Return newLocation.stationName
            Else
                ' Unknown outpost!
                newLocation = New Prism.Station
                newLocation.stationID = CLng(locID)
                newLocation.stationName = "Unknown Outpost"
                newLocation.systemID = 0
                newLocation.constID = 0
                newLocation.regionID = 0
                Return newLocation.stationName
            End If
        Else
            If CDbl(locID) < 60000000 Then
                If PlugInData.stations.Contains(locID) Then
                    Dim newSystem As SolarSystem = CType(PlugInData.stations(locID), SolarSystem)
                    Return newSystem.Name
                Else
                    Return "Unknown Location"
                End If
            Else
                newLocation = CType(PlugInData.stations(locID), Prism.Station)
                If newLocation IsNot Nothing Then
                    Return newLocation.stationName
                Else
                    ' Unknown system/station!
                    newLocation = New Prism.Station
                    newLocation.stationID = CLng(locID)
                    newLocation.stationName = "Unknown Location"
                    newLocation.systemID = 0
                    newLocation.constID = 0
                    newLocation.regionID = 0
                    Return newLocation.stationName
                End If
            End If
        End If
    End Function
End Class
