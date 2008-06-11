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
<Serializable()> Public Class ItemSkills
    Public ID As String
    Public Name As String
    Public Level As Integer
End Class

<Serializable()> Public Class ReqSkill
    Public ID As String
    Public Name As String
    Public ReqLevel As Integer
    Public CurLevel As Integer
    Public NeededFor As String
End Class

<Serializable()> Public Class Skill
    Public ID As String
    Public Name As String
    Public GroupID As String
    Public Attributes As New SortedList
End Class

<Serializable()> Public Class SkillLists
    Public Shared SkillList As New SortedList
End Class
