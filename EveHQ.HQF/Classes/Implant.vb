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

<ProtoContract()> <Serializable()> Public Class Implants

    <ProtoMember(1)> Public Shared ImplantList As New SortedList(Of String, ShipModule)    ' Key = Name

End Class

<ProtoContract()> <Serializable()> Public Class ImplantGroup

#Region "Property Variables"

    Private _groupName As String
    Private ReadOnly _implantName(10) As String

#End Region

#Region "Properties"

    <ProtoMember(1)> Public Property GroupName() As String
        Get
            Return _groupName
        End Get
        Set(ByVal value As String)
            _groupName = value
        End Set
    End Property
    <ProtoMember(2)> Public Property ImplantName(ByVal index As Integer) As String
        Get
            Return _implantName(index)
        End Get
        Set(ByVal value As String)
            _implantName(index) = value
        End Set
    End Property

#End Region

End Class
