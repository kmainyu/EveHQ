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
<Serializable()> Public Class CertificateCategory
    Public ID As Integer
    Public Name As String
End Class
<Serializable()> Public Class CertificateClass
    Public ID As Integer
    Public Name As String
End Class
<Serializable()> Public Class Certificate
    Public ID As Integer
    Public Grade As Integer
    Public ClassID As Integer
    Public CategoryID As Integer
    Public CorpID As Long
    Public Description As String
    Public RequiredSkills As New SortedList(Of Integer, Integer)
    Public RequiredCerts As New SortedList(Of String, Integer)
End Class
