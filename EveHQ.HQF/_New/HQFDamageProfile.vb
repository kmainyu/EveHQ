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

<ProtoContract()>
<Serializable()>
Public Class HQFDamageProfile
    <ProtoMember(1)> Public Property Name As String
    <ProtoMember(2)> Public Property Type As ProfileTypes
    <ProtoMember(3)> Public Property EM As Double
    <ProtoMember(4)> Public Property Explosive As Double
    <ProtoMember(5)> Public Property Kinetic As Double
    <ProtoMember(6)> Public Property Thermal As Double
    <ProtoMember(7)> Public Property DPS As Double
    <ProtoMember(8)> Public Property Fitting As String
    <ProtoMember(9)> Public Property Pilot As String
End Class
