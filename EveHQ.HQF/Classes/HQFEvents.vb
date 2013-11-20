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
Public Class HQFEvents
    Public Shared Event FindModule(ByVal modData As ArrayList)
    Public Shared Event UpdateFitting()
    Public Shared Event UpdateFittingList()
    Public Shared Event UpdateModuleList()
    Public Shared Event UpdateMruModuleList(ByVal modName As String)
    Public Shared Event UpdateShipInfo(ByVal pilotName As String)
    Public Shared Event UpdateAllImplantLists()
    Public Shared Event ShowModuleMarketGroup(ByVal path As String)
    Public Shared Event OpenFitting(fittingName As String)
    Public Shared Event CreateFitting(shipName As String)

    Shared WriteOnly Property StartCreateFitting As String
        Set(value As String)
            RaiseEvent CreateFitting(value)
        End Set
    End Property
    Shared WriteOnly Property StartOpenFitting As String
        Set(value As String)
            RaiseEvent OpenFitting(value)
        End Set
    End Property
    Shared WriteOnly Property DisplayedMarketGroup() As String
        Set(ByVal value As String)
            RaiseEvent ShowModuleMarketGroup(value)
        End Set
    End Property
    Shared WriteOnly Property StartFindModule() As ArrayList
        Set(ByVal value As ArrayList)
            If value IsNot Nothing Then
                RaiseEvent FindModule(value)
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateFitting() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateFitting()
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateFittingList() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateFittingList()
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateShipInfo() As String
        Set(ByVal value As String)
            If value <> "" Then
                RaiseEvent UpdateShipInfo(value)
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateModuleList() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateModuleList()
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateMruModuleList() As String
        Set(ByVal value As String)
            If value <> "" Then
                RaiseEvent UpdateMruModuleList(value)
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateImplantComboBox() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateAllImplantLists()
            End If
        End Set
    End Property

End Class
