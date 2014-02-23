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


Public Class VoidData
    Public Shared Wormholes As New SortedList(Of String, WormHole)
    Public Shared WormholeSystems As New SortedList(Of String, WormholeSystem)
    Public Shared WormholeEffects As New SortedList(Of String, WormholeEffect)
End Class

Public Class WormHole
    Public Property ID As String
    Public Property Name As String
    Public Property TargetClass As String
    Public Property MaxStabilityWindow As String
    Public Property MaxMassCapacity As String
    Public Property MassRegeneration As String
    Public Property MaxJumpableMass As String
    Public Property TargetDistributionID As String
End Class

Public Class WormholeSystem
    Public Property ID As String
    Public Property Name As String
    Public Property Constellation As String
    Public Property Region As String
    Public Property WClass As String
    Public Property WEffect As String
End Class

Public Class WormholeEffect
    Public Property WormholeType As String
    Public Property Attributes As New SortedList(Of String, Double)
End Class

Public Class TypeAttributeQuery
    Public Property TypeID As Long
    Public Property TypeName As String
    Public Property AttributeID As Integer
    Public Property UnitID As Integer
    Public Property Value As Double
End Class
