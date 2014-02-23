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

