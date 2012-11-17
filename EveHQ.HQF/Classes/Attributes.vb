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
<Serializable()> Public Class Attributes

    Public Shared AttributeList As New SortedList
    Public Shared AttributeQuickList As New SortedList

#Region "Constants"
    ' attributeIDs (see dbo.dgmAttributeTypes)
    ' ship attributes
    Public Const Ship_PowergridOutput As String = "11"
    Public Const Ship_CpuOutput As String = "48"
    ' module attributes
    Public Const Module_PowergridUsage As String = "30"
    Public Const Module_CpuUsage As String = "50"
    Public Const Module_MaxGroupActive As String = "763"
    Public Const Module_DroneBandwidthNeeded As String = "1272"

    ' attributeIDs (see HQF/Resources/Attributes.csv)
    Public Const Ship_MaxGangLinks As String = "10063"

#End Region

End Class

<Serializable()> Public Class Attribute
    Public ID As String
    Public Name As String
    Public DisplayName As String
    Public UnitName As String
    Public AttributeGroup As String
End Class
