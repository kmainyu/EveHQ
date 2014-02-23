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

