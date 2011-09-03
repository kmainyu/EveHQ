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

Public Class KMItem
    Public typeID As String
    Public flag As Integer
    Public qtyDropped As Integer
    Public qtyDestroyed As Integer
End Class

Public Class KMVictim
    Public charID As String
    Public charName As String
    Public corpID As String
    Public corpName As String
    Public allianceID As String
    Public allianceName As String
    Public factionID As String
    Public factionName As String
    Public damageTaken As Double
    Public shipTypeID As String
End Class

Public Class KMAttacker
    Public charID As String
    Public charName As String
    Public corpID As String
    Public corpName As String
    Public allianceID As String
    Public allianceName As String
    Public factionID As String
    Public factionName As String
    Public secStatus As Double
    Public damageDone As Double
    Public finalBlow As Boolean
    Public weaponTypeID As String
    Public shipTypeID As String
End Class

Public Class KillMail
    Public killID As String
    Public systemID As String
    Public killTime As Date
    Public moonID As String
    Public Victim As KMVictim
    Public Attackers As New SortedList
    Public Items As New List(Of KMItem)
End Class

Public Class SolarSystem
    Public ID As Integer
    Public Name As String
    Public Security As Double
    Public Region As String
    Public Constellation As String
End Class


