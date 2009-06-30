Public Class Portfolio
    Public Shared Investments As New SortedList
    Public Shared Transactions As New SortedList
    Public Shared InvestmentTypes As New SortedList
    Public Shared TransactionTypes As New SortedList
    Public Shared Sub SetupTypes()
        InvestmentTypes.Clear()
        InvestmentTypes.Add("Cash", 0)
        InvestmentTypes.Add("Shares", 1)
        TransactionTypes.Clear()
        TransactionTypes.Add("Purchase", 0)
        TransactionTypes.Add("Sale", 1)
        TransactionTypes.Add("Valuation", 2)
        TransactionTypes.Add("Income", 3)
        TransactionTypes.Add("Cost", 4)
        TransactionTypes.Add("Income (Retained)", 5)
        TransactionTypes.Add("Cost (Retained)", 6)
        TransactionTypes.Add("Transfer To Investment", 7)
        TransactionTypes.Add("Transfer From Investment", 8)
    End Sub
End Class

<Serializable()> Public Class Investment
    Public ID As Long
    Public Name As String
    Public Owner As String
    Public Type As Integer
    Public Description As String
    Public DateCreated As Date
    Public DateClosed As Date
    Public CurrentQuantity As Double
    Public CurrentCost As Double
    Public CurrentValue As Double
    Public LastValuation As Date
    Public CurrentProfits As Double
    Public CurrentIncome As Double
    Public CurrentCosts As Double
    Public TotalCostsForYield As Double
    Public ValueIsCost As Boolean
    Public Transactions As New SortedList
End Class

<Serializable()> Public Class InvestmentTransaction
    Public ID As Long
    Public InvestmentID As Long
    Public Type As Integer
    Public TransDate As Date
    Public Quantity As Double
    Public UnitValue As Double
End Class

Public Enum InvestmentType
    Cash = 0
    Shares = 1
End Enum

Public Enum InvestmentTransactionType
    Purchase = 0
    Sale = 1
    Valuation = 2
    Income = 3
    Cost = 4
    IncomeRetained = 5
    CostRetained = 6
    TransferToInv = 7
    TransferFromInv = 8
End Enum