' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
' 
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================
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
        <NonSerialized()> Public RequiredSkills As New ArrayList
    End Class

    Public Shared Function IsFittingUsable(ByVal pilotName As String, ByVal shipFit As String) As ArrayList
        Return Fittings.FittingList(shipFit).CalculateNeededSkills(pilotName).ShipPilotSkills
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

