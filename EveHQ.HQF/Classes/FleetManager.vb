Public Class FleetManager
    Public Shared FleetCollection As New SortedList(Of String, Fleet)

    <Serializable()> Public Class Fleet
        Public Name As String
        Public Commander As String
        Public Booster As String
        Public Wings As New SortedList(Of String, Wing)
        Public FleetSetups As New SortedList(Of String, String)
    End Class

    <Serializable()> Public Class Wing
        Public Name As String
        Public Commander As String
        Public Booster As String
        Public Squads As New SortedList(Of String, Squad)
    End Class

    <Serializable()> Public Class Squad
        Public Name As String
        Public Commander As String
        Public Booster As String
        Public Members As New SortedList(Of String, String)
    End Class

    <Serializable()> Public Class FleetMember
        Public Name As String
        Public WingName As String
        Public SquadName As String
        Public IsFC As Boolean
        Public IsWC As Boolean
        Public IsSC As Boolean
        Public IsFB As Boolean
        Public IsWB As Boolean
        Public IsSB As Boolean
    End Class
End Class

