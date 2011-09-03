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

Public Class VoidData
    Public Shared Wormholes As New SortedList(Of String, WormHole)
    Public Shared WormholeSystems As New SortedList(Of String, WormholeSystem)
    Public Shared WormholeEffects As New SortedList(Of String, WormholeEffect)
End Class

Public Class WormHole
    Public ID As String
    Public Name As String
    Public TargetClass As String
    Public MaxStabilityWindow As String
    Public MaxMassCapacity As String
    Public MassRegeneration As String
    Public MaxJumpableMass As String
    Public TargetDistributionID As String
End Class

Public Class WormholeSystem
    Public ID As String
    Public Name As String
    Public Constellation As String
    Public Region As String
    Public WClass As String
    Public WEffect As String
End Class

Public Class WormholeEffect
    Public WormholeType As String
    Public Attributes As New SortedList(Of String, Double)
End Class
