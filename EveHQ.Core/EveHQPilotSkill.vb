<Serializable()> Public Class EveHQPilotSkill
    Implements ICloneable

    Public Property ID As Integer

    Public Property Name As String

    Public Property GroupID As Integer

    Public Property Rank As Integer

    Public Property SP As Integer

    Public Property Level As Integer

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CType(MemberwiseClone(), PilotSkill)
    End Function
End Class