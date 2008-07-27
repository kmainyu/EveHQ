' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
<Serializable()> Public Class SolarSystem
    Public ID As Integer
    Public Name As String
    Public x As Double
    Public y As Double
    Public z As Double
    Public Flag As Boolean
    Public Security As Double
    Public EveSec As Double
    Public Region As String
    Public Constellation As String
    Public Gates As ArrayList = New ArrayList
    Public Jumps As ArrayList = New ArrayList
    Public Planets As SortedList = New SortedList
    Public Moons As SortedList = New SortedList
    Public Stations As SortedList = New SortedList
    Public ABelts As SortedList = New SortedList
    Public IBelts As SortedList = New SortedList
    Public SovereigntyName As String
    Public SovereigntyID As String
    Public RegionId As String
    Public SecClass As String
    Public constellationSovereignty As Integer
    Public csID As String
    Public sovereigntyLevel As String
    Public SysStations As Collection = New Collection
    Public CacheFileTime As Date
    Public CacheExpiration As Long
    Public CacheExpirationTime As Date
    Public Updated As Boolean = False
    Public LastUpdate As String = ""
    Public Active As Boolean = True

    Public Function GetName() As String
        Dim text1 As String = (Me.Name & " (" & Me.Security)
        If ((Me.Security = 1) OrElse (Me.Security = 0)) Then
            text1 = (text1 & ".0")
        End If
        Return (text1 & ")")
    End Function

End Class
