<Serializable()> Public Class EveHQPlugInStatus

    Public Name As String
    Public Disabled As Boolean

    Public Enum PlugInStatus
        Uninitialised = 0
        Loading = 1
        Failed = 2
        Active = 3
    End Enum

End Class
