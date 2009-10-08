Public Class VoidData
    Public Shared Wormholes As New SortedList(Of String, WormHole)
    Public Shared WormholeSystems As New SortedList(Of String, WormholeSystem)
    Public Shared WormholeEffects As New SortedList(Of String, WormholeEffect)
End Class

Public Class WormHole
    Public ID As String
    Public Name As String
    Public TargetClass As String
    Public MaxStabilityWindow As String
    Public MaxMassCapacity As String
    Public MassRegeneration As String
    Public MaxJumpableMass As String
    Public TargetDistributionID As String
End Class

Public Class WormholeSystem
    Public ID As String
    Public Name As String
    Public Constellation As String
    Public Region As String
    Public WClass As String
    Public WEffect As String
End Class

Public Class WormholeEffect
    Public WormholeType As String
    Public Attributes As New SortedList(Of String, Double)
End Class
