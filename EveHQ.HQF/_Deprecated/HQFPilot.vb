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

' ReSharper disable once CheckNamespace - for MS serialization compatability
<Serializable()> Public Class HQFPilot

#Region "Property Variables"

    ' ReSharper disable InconsistentNaming - for MS serialization compatability
    Private cPilotName As String
    Private cSkillSet As New Collection
    ' ReSharper disable once FieldCanBeMadeReadOnly.Local - for MS serialization compatability
    Private cImplantName(10) As String

#End Region

#Region "Properties"

    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
        End Set
    End Property

    Public Property SkillSet() As Collection
        Get
            Return cSkillSet
        End Get
        Set(ByVal value As Collection)
            cSkillSet = value
        End Set
    End Property

    Public Property ImplantName(ByVal index As Integer) As String
        ' Use ImplantName(0) as the GroupName identifer
        Get
            Return cImplantName(index)
        End Get
        Set(ByVal value As String)
            cImplantName(index) = value
            ' Check if we can set the implants from the group listing
            If index = 0 Then
                If value <> "*Custom*" Then
                    If PluginSettings.HQFSettings.ImplantGroups.ContainsKey(value) Then
                        Dim implantSet As ImplantGroup = Settings.HQFSettings.ImplantGroups(value)
                        For slot As Integer = 1 To 10
                            cImplantName(slot) = implantSet.ImplantName(slot)
                        Next
                    End If
                End If
            End If
        End Set
    End Property

    ' ReSharper restore InconsistentNaming

#End Region

End Class

