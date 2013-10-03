<Serializable()> Public Class BPCCostInfo

    Public ID As Integer
    Public MinRunCost As Double
    Public MaxRunCost As Double

    Public Sub New(ByVal bpcid As Integer, ByVal bpcMinRunCost As Double, ByVal bpcMaxRunCost As Double)
        ID = bpcid
        MinRunCost = bpcMinRunCost
        MaxRunCost = bpcMaxRunCost
    End Sub

End Class