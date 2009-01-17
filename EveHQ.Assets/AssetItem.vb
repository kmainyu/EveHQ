﻿' ========================================================================
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
Public Class AssetItem
    Public itemID As String
    Public system As String
    Public typeID As String
    Public typeName As String
    Public owner As String
    Public group As String
    Public category As String
    Public location As String
    Public quantity As Long
    Public price As Double
End Class

Public Class ItemData
    Public ID As Long
    Public Name As String
    Public Group As Integer
    Public Category As Integer
    Public MarketGroup As Integer
    Public Published As Integer
    Public Volume As Integer
    Public MetaLevel As Integer = 1
    Public PortionSize As Integer
End Class
