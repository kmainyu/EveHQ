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

' ReSharper disable once CheckNamespace - for binary serialization compatability
<Serializable()> Public Class BPCCostInfo

    Public ID As String
    Public MinRunCost As Double
    Public MaxRunCost As Double

    Public Sub New(ByVal bpcid As String, ByVal bpcMinRunCost As Double, ByVal bpcMaxRunCost As Double)
        ID = bpcid
        MinRunCost = bpcMinRunCost
        MaxRunCost = bpcMaxRunCost
    End Sub

End Class