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

Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

' ReSharper disable once CheckNamespace - For binary serialization compatability
<Serializable()>Public Class ProductionJob
    Public JobName As String
    Public CurrentBP As BlueprintSelection
    ' ReSharper disable once InconsistentNaming - For binary serialization compatability
    Public BPID As Integer
    Public TypeID As Integer
    Public TypeName As String
    Public PerfectUnits As Double
    Public WasteUnits As Double
    Public Runs As Integer
    Public Manufacturer As String
    Public BPOwner As String
    Public PESkill As Integer
    Public IndSkill As Integer
    Public ProdImplant As Integer
    Public OverridingME As String
    Public OverridingPE As String
    Public AssemblyArray As AssemblyArray
    Public StartTime As Date
    Public RunTime As Long
    Public Cost As Double
    Public RequiredResources As New SortedList(Of String, Object)
    Public HasInventionJob As Boolean
    Public InventionJob As New InventionJob
    Public SubJobMEs As New SortedList(Of String, Integer)
    Public ProduceSubJob As Boolean = False

    Public Function Clone() As ProductionJob
        Using cloneMemoryStream As New MemoryStream
            Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
            objBinaryFormatter.Serialize(cloneMemoryStream, Me)
            cloneMemoryStream.Seek(0, SeekOrigin.Begin)
            Return CType(objBinaryFormatter.Deserialize(cloneMemoryStream), ProductionJob)
        End Using
    End Function

End Class

