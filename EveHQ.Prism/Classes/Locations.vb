'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports EveHQ.EveData

Namespace Classes

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
End Namespace