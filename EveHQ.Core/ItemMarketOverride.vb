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
Imports EveHQ.Market

<Serializable()>
Public Class ItemMarketOverride
    Private _itemId As Integer
    Private _transactionType As MarketTransactionKind
    Private _marketStat As MarketMetric

    Public Property ItemId As Integer
        Get
            Return _itemId
        End Get
        Set(value As Integer)
            _itemId = value
        End Set
    End Property

    Public Property TransactionType As MarketTransactionKind
        Get
            Return _transactionType
        End Get
        Set(value As MarketTransactionKind)
            _transactionType = value
        End Set
    End Property

    Public Property MarketStat As MarketMetric
        Get
            Return _marketStat
        End Get
        Set(value As MarketMetric)
            _marketStat = value
        End Set
    End Property
End Class

