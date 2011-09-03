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
<Serializable()> Public Class NPCs
    Public Shared NPCList As New SortedList
End Class

<Serializable()> Public Class NPC
    Public Name As String
    Public GroupName As String
    Public EM As Double
    Public Explosive As Double
    Public Kinetic As Double
    Public Thermal As Double
    Public ROF As Double
    Public DamageMod As Double
    Public MissileType As String
    Public MissileROF As Double
    Public DPS As Double
End Class
