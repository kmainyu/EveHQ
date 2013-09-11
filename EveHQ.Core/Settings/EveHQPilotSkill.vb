<Serializable()> Public Class EveHQPilotSkill
    Implements ICloneable

    Private ReadOnly _levelUp(5) As Integer

    Public Property ID As String

    Public Property Name As String

    Public Property GroupID As String

    Public Property Flag As Integer

    Public Property Rank As Integer

    Public Property SP As Integer

    Public Property Level As Integer

    Public Property LevelUp(index As Integer) As Integer
        Get
            Return _levelUp(index)
        End Get
        Set(value As Integer)
            _levelUp(index) = value
        End Set
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim r As PilotSkill = CType(MemberwiseClone(), PilotSkill)
        Return r
    End Function
End Class