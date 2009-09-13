Public Class FleetManager
    Public Shared FleetCollection As New SortedList(Of String, Fleet)

    <Serializable()> Public Class Fleet
        Public Name As String
        Public Commander As String
        Public Wings As New SortedList(Of String, Wing)
        Public FleetSetups As New SortedList(Of String, String)
    End Class

    <Serializable()> Public Class Wing
        Public Name As String
        Public Commander As String
        Public Squads As New SortedList(Of String, Squad)
    End Class

    <Serializable()> Public Class Squad
        Public Name As String
        Public Commander As String
        Public Members As New SortedList(Of String, String)
    End Class
End Class

