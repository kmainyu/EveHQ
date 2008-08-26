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
<Serializable()> Public Class Implants

    Public Shared implantList As New SortedList   ' Key = Name
    Public Shared implantGroups As New SortedList   ' Key = Name

End Class

<Serializable()> Public Class ImplantGroup

#Region "Property Variables"

    Private cGroupName As String
    Private cImplantName(10) As String

#End Region

#Region "Properties"

    Public Property GroupName() As String
        Get
            Return cGroupName
        End Get
        Set(ByVal value As String)
            cGroupName = value
        End Set
    End Property
    Public Property ImplantName(ByVal index As Integer) As String
        Get
            Return cImplantName(index)
        End Get
        Set(ByVal value As String)
            cImplantName(index) = value
        End Set
    End Property

#End Region

End Class
