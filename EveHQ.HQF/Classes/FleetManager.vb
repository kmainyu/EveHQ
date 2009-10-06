Public Class FleetManager
    Public Shared FleetCollection As New SortedList(Of String, Fleet)

    <Serializable()> Public Class Fleet
        Public Name As String
        Public Commander As String
        Public Booster As String
        Public Wings As New SortedList(Of String, Wing)
        Public FleetSetups As New SortedList(Of String, FleetSetup)
        Public RemoteReceiving As New SortedList(Of String, ArrayList) ' Key is receiving pilot
        Public RemoteGiving As New SortedList(Of String, ArrayList) ' Key is giving pilot
        Public WHEffect As String
        Public WHClass As String
        Public DamageProfile As String
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

    <Serializable()> Public Class RemoteAssignment
        Public FleetPilot As String
        Public RemotePilot As String
        Public RemoteModule As String
    End Class

    <Serializable()> Public Class FleetSetup
        Public PilotName As String
        Public FittingName As String
        <NonSerialized()> Public IsFlyable As Boolean
        <NonSerialized()> Public RequiredSkills As New SortedList
    End Class

    Public Shared Function IsFittingUsable(ByVal pilotName As String, ByVal shipFit As String) As SortedList
        Dim fittingSep As Integer = shipFit.IndexOf(", ")
        Dim shipName As String = shipFit.Substring(0, fittingSep)
        Dim fittingName As String = shipFit.Substring(fittingSep + 2)
        Dim aPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(pilotName), HQFPilot)
        Dim aShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
        aShip = Engine.UpdateShipDataFromFittingList(aShip, CType(Fittings.FittingList(shipFit), ArrayList))
        Return Engine.NeededSkillsForShip(pilotName, aShip)
    End Function

    Public Shared Sub CheckAllFittings()
        For Each cFleet As FleetManager.Fleet In FleetManager.FleetCollection.Values
            For Each cSetup As FleetManager.FleetSetup In cFleet.FleetSetups.Values
                cSetup.RequiredSkills = FleetManager.IsFittingUsable(cSetup.PilotName, cSetup.FittingName)
                If cSetup.RequiredSkills.Count = 0 Then
                    cSetup.IsFlyable = True
                Else
                    cSetup.IsFlyable = False
                End If
            Next
        Next
    End Sub

End Class

