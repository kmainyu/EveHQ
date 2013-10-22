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
Namespace Classes
    Public Class MarketOrdersCollection
        Public TotalOrders As Integer
        Public BuyOrders As Integer
        Public SellOrders As Integer
        Public BuyOrderValue As Double
        Public SellOrderValue As Double
        Public EscrowValue As Double
        Public MarketOrders As New ArrayList
    End Class

    Public Class MarketOrder
        Public OrderID As String
        Public CharID As String
        Public StationID As String
        Public VolEntered As Long
        Public VolRemaining As Long
        Public MinVolume As Long
        Public OrderState As Integer
        Public TypeID As Integer
        Public Range As Integer
        Public AccountKey As Integer
        Public Duration As Integer
        Public Escrow As Double
        Public Price As Double
        Public Bid As Integer
        Public Issued As Date
    End Class

    Public Enum MarketOrderState As Integer
        '0 = open/active, 1 = closed, 2 = expired (or fulfilled), 3 = cancelled, 4 = pending, 5 = character deleted
        Open = 0
        Closed = 1
        Completed = 2
        Cancelled = 3
        Pending = 4
        CharDeleted = 5
    End Enum
End Namespace