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

