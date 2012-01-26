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
<Serializable()> Public Class Corporation
    Public Name As String
    Public ID As String
    Public CharacterIDs As New List(Of String) ' List of all chars supporting this corp (can be multiple)
    Public CharacterNames As New List(Of String) ' List of all char names supporting this corp (can be multiple)
    Public Accounts As New List(Of String) ' IDs of all corp accounts which support this corporation (can be multiple)
End Class
