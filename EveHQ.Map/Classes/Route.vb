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
Public Class Route
    Public RouteDist As Double
    Public Shared Dest As SolarSystem
    Public Jump As Boolean
    Public Length As Integer
    Public [Next] As Route
    Public RemainDist As Double
    Public Sys As SolarSystem
    Public Temp As Integer

    Public Sub New(ByVal s As SolarSystem, ByVal n As Route)
        Me.Sys = s
        Me.Jump = False
        Me.Next = n
        If (Not n Is Nothing) Then
            Me.Length = (n.Length + 1)
            Me.RouteDist = (n.RouteDist + frmMap.Distance(s, n.Sys))
        Else
            Me.Length = 1
            Me.RouteDist = 0
        End If
        If (Not Route.Dest Is Nothing) Then
            Me.RemainDist = frmMap.Distance(s, Route.Dest)
        Else
            Me.RemainDist = 0
        End If
        Me.Temp = 0
    End Sub

    Public Function CompareTo(ByVal o As Object) As Integer
        Dim route1 As Route = DirectCast(o, Route)
        If (Me.Length < route1.Length) Then
            Return -1
        End If
        If (Me.Length > route1.Length) Then
            Return 1
        End If
        If (Me.RemainDist < route1.RemainDist) Then
            Return -1
        End If
        If (Me.RemainDist > route1.RemainDist) Then
            Return 1
        End If
        Return 0
    End Function
    Public Function Concat(ByVal c As Route) As Route
        Dim route1 As Route = Me
        Do While (Not route1.Next Is Nothing)
            route1 = route1.Next
        Loop
        If (Not c Is Nothing) Then
            Dim num1 As Double = c.RouteDist
            frmMap.Distance(route1.Sys, c.Sys)
            route1.Next = c
            Dim route2 As Route = Me
            Do While (Not route2 Is c)
                route2.Length = (route2.Length + c.Length)
                route2.RouteDist = (route2.RouteDist + c.RouteDist)
                route2 = route2.Next
            Loop
        End If
        Return route1
    End Function

    Public Function Contains(ByVal s As SolarSystem) As Boolean
        Dim route1 As Route = Me
        Do While (Not route1 Is Nothing)
            If (route1.Sys Is s) Then
                Return True
            End If
            route1 = route1.Next
        Loop
        Return False
    End Function
    Public Function Invert(ByVal prev As Route) As Route
        Dim route1 As Route = New Route(Me.Sys, prev)
        If (Not Me.Next Is Nothing) Then
            Return Me.Next.Invert(route1)
        End If
        Return route1
    End Function

End Class
