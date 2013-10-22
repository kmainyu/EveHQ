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

' ReSharper disable once CheckNamespace - For binary serialization compatability
<Serializable()> Public Class Blueprint
    Public ID As Integer
    Public AssetID As Long
    Public Name As String = ""
    Public GroupID As Integer
    Public ProductID As Integer
    Public ProdTime As Double
    Public TechLevel As Integer
    Public ResearchProdTime As Double
    Public ResearchMatTime As Double
    Public ResearchCopyTime As Double
    Public ResearchTechTime As Double
    Public ProdMod As Integer
    Public MatMod As Integer
    Public WasteFactor As Double
    Public MaxProdLimit As Integer
    Public Resources As New SortedList(Of String, BlueprintResource)
    Public Inventions As New List(Of String)
    Public InventionMetaItems As New SortedList(Of String, String)
    Public InventFrom As New List(Of String)
End Class

<Serializable()> Public Class BlueprintResource
    Public TypeID As Integer
    Public TypeName As String
    Public TypeGroup As Integer
    Public TypeCategory As Integer
    Public Activity As Integer
    Public Quantity As Integer
    Public DamagePerJob As Double
    Public BaseMaterial As Integer
End Class

<Serializable()> Public Class RequiredResource
    Public TypeID As Integer
    Public TypeName As String
    Public TypeGroup As Integer
    Public TypeCategory As Integer
    Public PerfectUnits As Double
    Public BaseUnits As Double
    Public WasteUnits As Double
End Class

<Serializable()> Public Class BlueprintSelection
    Inherits Blueprint
    Public MELevel As Integer
    Public PELevel As Integer
    Public Runs As Integer

End Class


