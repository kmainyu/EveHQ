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

<Serializable()> Public Class ImplantCollection

#Region "Property Variables"

    ''' <summary>
    ''' Constructor for a new ImplantCollection
    ''' </summary>
    ''' <param name="autoPopulate">Set to true if declaring from code to add in the required 11 elements</param>
    ''' <remarks></remarks>
    Public Sub New(autoPopulate As Boolean)
        ImplantName.Clear()
        If autoPopulate = True Then
            ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
            For slot As Integer = 0 To 10
                ImplantName.Add("")
            Next
        End If
    End Sub

#End Region

#Region "Properties"

    Public Property GroupName() As String
       
    Public Property ImplantName As New List(Of String)
       
#End Region

End Class

