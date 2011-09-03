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
Public Class Market

End Class

Public Class MarketData
    Public ItemID As String
    Public RegionID As String
    Public HistoryDate As String
    Public MinAll As Double
    Public MaxAll As Double
    Public AvgAll As Double
    Public MedAll As Double
    Public StdAll As Double
    Public VarAll As Double
    Public VolAll As Double
    Public QtyAll As Double
    Public MinBuy As Double
    Public MaxBuy As Double
    Public AvgBuy As Double
    Public MedBuy As Double
    Public StdBuy As Double
    Public VarBuy As Double
    Public VolBuy As Double
    Public QtyBuy As Double
    Public MinSell As Double
    Public MaxSell As Double
    Public AvgSell As Double
    Public MedSell As Double
    Public StdSell As Double
    Public VarSell As Double
    Public VolSell As Double
    Public QtySell As Double
End Class

Public Enum MarketSite
    Battleclinic = 0
    EveMarketeer = 1
End Enum
