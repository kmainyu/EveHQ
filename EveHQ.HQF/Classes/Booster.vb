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
Imports ProtoBuf

<ProtoContract()> <Serializable()> Public Class Boosters

    <ProtoMember(1)> Public Shared BoosterList As New SortedList(Of String, ShipModule)   ' Key = Name
    <ProtoMember(2)> Public Shared BoosterEffects As New SortedList(Of String, SortedList(Of String, BoosterEffect)) ' Key = Name, Value = SortedList (of String, BoosterEffect)

End Class

<ProtoContract()> <Serializable()> Public Class BoosterEffect
    <ProtoMember(1)> Public AttributeID As String
    <ProtoMember(2)> Public AttributeEffect As String
End Class


