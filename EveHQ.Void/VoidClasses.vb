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

Public Class VoidData
    Public Shared Wormholes As New SortedList(Of String, WormHole)
    Public Shared WormholeSystems As New SortedList(Of String, WormholeSystem)
    Public Shared WormholeEffects As New SortedList(Of String, WormholeEffect)
End Class

Public Class WormHole
    Public Property ID As String
    Public Property Name As String
    Public Property TargetClass As String
    Public Property MaxStabilityWindow As String
    Public Property MaxMassCapacity As String
    Public Property MassRegeneration As String
    Public Property MaxJumpableMass As String
    Public Property TargetDistributionID As String
End Class

Public Class WormholeSystem
    Public Property ID As String
    Public Property Name As String
    Public Property Constellation As String
    Public Property Region As String
    Public Property WClass As String
    Public Property WEffect As String
End Class

Public Class WormholeEffect
    Public Property WormholeType As String
    Public Property Attributes As New SortedList(Of String, Double)
End Class

Public Class TypeAttributeQuery
    Public Property TypeID As Long
    Public Property TypeName As String
    Public Property AttributeID As Integer
    Public Property UnitID As Integer
    Public Property Value As Double
End Class
