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
        Dim cloneMemoryStream As New MemoryStream
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(cloneMemoryStream, Me)
        cloneMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newJob As ProductionJob = CType(objBinaryFormatter.Deserialize(cloneMemoryStream), ProductionJob)
        cloneMemoryStream.Close()
        cloneMemoryStream.Dispose()
        Return newJob
    End Function

End Class

