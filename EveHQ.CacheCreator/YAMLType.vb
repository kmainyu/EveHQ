' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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

''' <summary>
''' Defines a partial copy of an Eve Type which contains data from the YAML tables
''' </summary>
''' <remarks></remarks>
Public Class YAMLType
    Public Property TypeID As Integer
    Public Property IconID As Integer
    Public Property IconName As String
    Public Property Masteries As IDictionary(Of Integer, List(Of Integer))
    Public Property Traits As Dictionary(Of Integer, List(Of String))
End Class
