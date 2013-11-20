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

Public Class KillmailItem
    Public Property TypeID As Integer
    Public Property Flag As Integer
    Public Property QtyDropped As Integer
    Public Property QtyDestroyed As Integer
End Class

Public Class KillmailVictim
    Public Property CharID As String
    Public Property CharName As String
    Public Property CorpID As String
    Public Property CorpName As String
    Public Property AllianceID As String
    Public Property AllianceName As String
    Public Property FactionID As String
    Public Property FactionName As String
    Public Property DamageTaken As Double
    Public Property ShipTypeID As Integer
End Class

Public Class KillmailAttacker
    Public Property CharID As String
    Public Property CharName As String
    Public Property CorpID As String
    Public Property CorpName As String
    Public Property AllianceID As String
    Public Property AllianceName As String
    Public Property FactionID As String
    Public Property FactionName As String
    Public Property SecStatus As Double
    Public Property DamageDone As Double
    Public Property FinalBlow As Boolean
    Public Property WeaponTypeID As Integer
    Public Property ShipTypeID As Integer
End Class

Public Class KillMail
    Public Property KillID As Integer
    Public Property SystemID As Integer
    Public Property KillTime As Date
    Public Property MoonID As String
    Public Property Victim As KillmailVictim
    Public Property Attackers As New SortedList
    Public Property Items As New List(Of KillmailItem)
End Class
