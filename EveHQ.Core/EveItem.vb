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
<Serializable()> Public Class EveItem
    Public ID As Long
    Public Name As String
    Public Group As Integer
    Public Category As Integer
    Public MarketGroup As Integer
    Public Published As Boolean
    Public Volume As Double = 0
    Public MetaLevel As Integer = 1
    Public PortionSize As Integer
    Public Icon As String = ""
    Public BasePrice As Double = 0
End Class
