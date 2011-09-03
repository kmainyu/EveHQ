' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
<Serializable()> Public Class PriceGroup
    Public Name As String
    Public TypeIDs As New List(Of String)
    Public RegionIDs As New List(Of String)
    Public PriceFlags As PriceGroupFlags
    Public PriceListID As Integer
End Class

<Serializable()> Public Enum PriceGroupFlags
    MinAll = 1
    MinBuy = 2
    MinSell = 4
    MaxAll = 8
    MaxBuy = 16
    MaxSell = 32
    AvgAll = 64
    AvgBuy = 128
    AvgSell = 256
    MedAll = 512
    MedBuy = 1024
    MedSell = 2048
End Enum
