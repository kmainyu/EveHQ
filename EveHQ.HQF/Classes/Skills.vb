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

<ProtoContract()> <Serializable()> Public Class ItemSkills
    <ProtoMember(1)> Public Property ID As Integer
    <ProtoMember(2)> Public Property Name As String
    <ProtoMember(3)> Public Property Level As Integer
End Class

<ProtoContract()> <Serializable()> Public Class NeededSkillsCollection
    <ProtoMember(1)> Public Property ShipPilotSkills As New ArrayList
    <ProtoMember(2)> Public Property TruePilotSkills As New ArrayList
End Class

<ProtoContract()> <Serializable()> Public Class ReqSkill
    <ProtoMember(1)> Public Property ID As Integer
    <ProtoMember(2)> Public Property Name As String
    <ProtoMember(3)> Public Property ReqLevel As Integer
    <ProtoMember(4)> Public Property CurLevel As Integer
    <ProtoMember(5)> Public Property NeededFor As String
End Class

<ProtoContract()> <Serializable()> Public Class Skill
    <ProtoMember(1)> Public Property ID As Integer
    <ProtoMember(2)> Public Property Name As String
    <ProtoMember(3)> Public Property GroupID As String
    <ProtoMember(4)> Public Property Attributes As New SortedList(Of Integer, Double)
End Class

Public Class SkillLists
    Public Shared SkillList As New SortedList(Of Integer, Skill) ' SkillID, Skill
End Class
