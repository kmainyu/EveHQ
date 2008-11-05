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
Public Class Dijkstra

    Dim found(50000) As Boolean
    Dim dist(50000) As Integer

    Dim prev As SortedList = New SortedList
    Public Const ly As Double = 9.4605284E+15

    Dim s As SortedList = New SortedList
    Dim q As SortedList = New SortedList
    Dim open As SortedList = New SortedList

    Public Function GetRoute() As SortedList
        Return prev
    End Function

    Private Function GetCost(ByVal u As SolarSystem, ByVal v As SolarSystem) As Double
        If u.ID = v.ID Then
            Return 0
            Exit Function
        End If
        ' Get value of the edge between u and v (will always be 1 if connected)
        For Each gatelink As String In u.Gates
            If CInt(gatelink) = v.ID Then
                Return CalcDistance(u, v)
                Exit Function
            End If
        Next
        Return 1000000
    End Function
    Private Function Choose(Optional ByVal minSec As Double = 0.0, Optional ByVal maxSec As Double = 1.0) As Integer
        Dim min As Double = 1000000
        Dim minPos As Integer = -1
        For check As Integer = 0 To q.Count - 1
            Dim sys As SolarSystem = CType(q.GetByIndex(check), SolarSystem)
            If dist(sys.ID) < min And found(sys.ID) = False Then
                min = dist(sys.ID)
                minPos = sys.ID
            End If
        Next
        Return minPos
    End Function
    Public Function GetPath(ByVal fromSys As SolarSystem, ByVal toSys As SolarSystem, ByVal Recalc As Boolean, Optional ByVal minSec As Double = 0.0, Optional ByVal maxSec As Double = 1.0) As ArrayList

        If Recalc = True Then
            Call Me.BuildPaths(fromSys, toSys, minSec, maxSec)
        End If

        Dim tracer As Integer = toSys.ID
        Dim route As New ArrayList
        Do While tracer <> fromSys.ID And tracer <> 0
            route.Add(tracer)
            tracer = CInt(prev.Item(tracer))
        Loop
        If tracer = 0 Then
            Return Nothing
        Else
            route.Add(fromSys.ID)
            Return route
        End If
    End Function
    Public Function GetJumpPath(ByVal fromSys As SolarSystem, ByVal toSys As SolarSystem, ByVal Recalc As Boolean) As ArrayList

        If Recalc = True Then
            Call Me.BuildJumpPaths(fromSys, toSys)
        End If

        Dim tracer As Integer = toSys.ID
        Dim route As New ArrayList
        Do While tracer <> fromSys.ID And tracer <> 0
            route.Add(tracer)
            tracer = CInt(prev.Item(tracer))
        Loop
        If tracer = 0 Then
            Return Nothing
        Else
            route.Add(fromSys.ID)
            Return route
        End If

    End Function

    Private Sub BuildPaths(ByVal fromSys As SolarSystem, ByVal toSys As SolarSystem, Optional ByVal minSec As Double = 0.0, Optional ByVal maxSec As Double = 1.0)
        q.Clear()
        s.Clear()
        prev.Clear()

        ' Prepare array
        For sys As Integer = 1 To 50000
            dist(sys) = 1000000
            found(sys) = False
        Next

        dist(fromSys.ID) = 0
        q.Add(fromSys.ID, fromSys)

        ' The REAL work!!!
        While q.Count > 0
            Dim c2 As Integer = Choose()
            Dim u As SolarSystem = CType(PlugInData.SystemsID(c2.ToString), SolarSystem)
            q.Remove(u.ID)
            s.Add(u.ID, u)
            found(u.ID) = True
            ' See if we can improve the distance
            For Each gatelink As SolarSystem In u.Gates
                If q.Contains(gatelink.ID) = False Then
                    If u.EveSec >= minSec And u.EveSec <= maxSec Then
                        If dist(gatelink.ID) > dist(u.ID) + 1 Then
                            dist(gatelink.ID) = dist(u.ID) + 1
                            q.Add(gatelink.ID, PlugInData.SystemsID(gatelink.ID.ToString))
                            prev.Add(gatelink.ID, u.ID)
                        End If
                    End If
                End If
            Next
        End While
    End Sub
    Public Function SystemsInGateRange(ByVal fromSys As SolarSystem, ByVal toSys As SolarSystem, ByVal Recalc As Boolean, ByVal Range As Integer) As ArrayList
        If Recalc = True Then
            Call Me.BuildPaths(fromSys, toSys)
        End If

        Dim systems As New ArrayList
        For distcheck As Integer = 1 To 50000
            If dist(distcheck) <= Range And dist(distcheck) <> 0 Then
                Dim system(0, 1) As Integer
                system(0, 0) = distcheck
                system(0, 1) = dist(distcheck)
                systems.Add(system)
            End If
        Next
        Return systems

    End Function
    Private Sub BuildJumpPaths(ByVal fromSys As SolarSystem, ByVal toSys As SolarSystem)
        q.Clear()
        s.Clear()
        prev.Clear()

        Dim maxdist As Double = 5.0

        ' Prepare array
        For sys As Integer = 1 To PlugInData.SystemsID.Count
            dist(sys) = 1000000
            found(sys) = False
        Next

        dist(fromSys.ID) = 0
        q.Add(fromSys.ID, fromSys)

        ' The REAL work!!!
        Dim u, v As SolarSystem
        While q.Count > 0
            Dim c2 As Integer = Choose()
            Dim cdist As Double = 0
            u = CType(PlugInData.SystemsID(CStr(c2)), SolarSystem)
            q.Remove(u.ID)
            s.Add(u.ID, u)
            found(u.ID) = True
            ' See if we can improve the distance
            For Each gatelink As SolarSystem In u.Jumps
                v = CType(PlugInData.SystemsID(CStr(gatelink.ID)), SolarSystem)
                If q.Contains(gatelink.ID) = False Then
                    cdist = CalcDistance(u, v)
                    If cdist < maxdist Then
                        If dist(gatelink.ID) > dist(u.ID) + cdist Then
                            dist(gatelink.ID) = CInt(dist(u.ID) + cdist)
                            q.Add(gatelink.ID, PlugInData.SystemsID(CStr(gatelink.ID)))
                            prev.Add(gatelink.ID, u.ID)
                        End If
                    End If
                End If
            Next
        End While
    End Sub
    Public Function CalcDistance(ByVal sSys As SolarSystem, ByVal tSys As SolarSystem) As Double
        Dim dist As Double = 0
        Dim x1, x2, y1, y2, z1, z2 As Double

        x1 = sSys.x
        y1 = sSys.y
        z1 = sSys.z
        x2 = tSys.x
        y2 = tSys.y
        z2 = tSys.z
        dist = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2)) / ly

        Return dist

    End Function

End Class
