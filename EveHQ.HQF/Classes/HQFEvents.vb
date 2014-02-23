'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

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
