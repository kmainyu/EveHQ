' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
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
Imports System.Globalization
Imports EveHQ.EveApi
Imports EveHQ.Common.Extensions
Imports System.Xml

Public Class Standings
    Public Shared Sub GetStandings(ByVal pilotName As String)

        If HQ.Settings.Pilots.ContainsKey(pilotName) = True Then
            Dim pilot As EveHQPilot = HQ.Settings.Pilots(pilotName)

            ' Clear the existing standings
            pilot.Standings.Clear()

            ' Set culture info
            Dim culture As CultureInfo = New CultureInfo("en-GB")

            ' Get Account info for the API
            Dim accountName As String = pilot.Account
            If HQ.Settings.Accounts.ContainsKey(accountName) = True Then
                Dim pilotAccount As EveHQAccount = HQ.Settings.Accounts.Item(accountName)

                ' Stage 1 - Get the NPC Standings
                Try
                    Dim standingsList As IEnumerable(Of NpcStanding)
                    Dim standingsNode As NpcStanding
                    Dim standingsResponse As EveServiceResponse(Of IEnumerable(Of NpcStanding)) =
                            HQ.ApiProvider.Character.NPCStandings(pilotAccount.UserID, pilotAccount.APIKey,
                                                                  pilot.ID.ToInt32())
                    'Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
                    'Dim standingsXML As XmlDocument = apiReq.GetAPIXML(APITypes.StandingsChar, pilotAccount.ToAPIAccount, pilot.ID, APIReturnMethods.ReturnStandard)
                    If standingsResponse IsNot Nothing Then


                        If standingsResponse.IsSuccess Then
                            standingsList = standingsResponse.ResultData
                            If standingsList IsNot Nothing Then
                                If standingsList.Count > 0 Then

                                    For Each standingsNode In standingsList

                                        Dim currentStandingsType As StandingType = StandingType.Unknown

                                        Select Case standingsNode.Kind
                                            Case NpcType.Agents
                                                currentStandingsType = StandingType.Agent
                                            Case NpcType.NPCCorporations
                                                currentStandingsType = StandingType.NPCCorporation
                                            Case NpcType.Factions
                                                currentStandingsType = StandingType.Faction
                                        End Select


                                        Dim newStanding As New PilotStanding
                                        newStanding.ID = standingsNode.FromId
                                        newStanding.Name = standingsNode.FromName
                                        newStanding.Type = currentStandingsType
                                        newStanding.Standing = standingsNode.Standing
                                        If pilot.Standings.ContainsKey(newStanding.ID) = False Then
                                            pilot.Standings.Add(newStanding.ID, newStanding)
                                        End If


                                    Next

                                End If
                            End If
                        End If
                    End If
                Catch e As Exception
                    ' Just skip and try the next stage
                End Try

                ' Stage 2 - Get the player and corp standings
                Try
                    Dim standingsList As IEnumerable(Of Contact)
                    Dim contactResponse As EveServiceResponse(Of IEnumerable(Of Contact)) = HQ.ApiProvider.Character.ContactList(pilotAccount.UserID, pilotAccount.APIKey, pilot.ID.ToInt32())
                    'Dim _
                    '    apiReq As _
                    '        New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension,
                    '                          HQ.CacheFolder)
                    'Dim standingsXML As XmlDocument = apiReq.GetAPIXML(APITypes.ContactListChar,
                    '                                                   pilotAccount.ToAPIAccount, pilot.ID,
                    '                                                   APIReturnMethods.ReturnStandard)
                    If contactResponse IsNot Nothing Then


                        If contactResponse.IsSuccess Then
                            standingsList = contactResponse.ResultData
                            If standingsList IsNot Nothing Then


                                Const currentStandingsType As StandingType = StandingType.PlayerCorp

                                For Each entity As Contact In standingsList

                                    Dim newStanding As New PilotStanding
                                    newStanding.ID = entity.ContactId
                                    newStanding.Name = entity.ContactName
                                    newStanding.Type = currentStandingsType
                                    newStanding.Standing = entity.Standing
                                    If pilot.Standings.ContainsKey(newStanding.ID) = False Then
                                        pilot.Standings.Add(newStanding.ID, newStanding)
                                    End If

                                Next
                            End If
                        End If
                    End If
                Catch e As Exception
                ' Just skip and exit
            End Try

            End If

        End If
    End Sub

    Public Shared Function GetStanding(ByVal pilotName As String, ByVal entityID As String,
                                       ByVal returnEffectiveStanding As Boolean) As Double
        ' Try and get the standings data
        If HQ.Settings.Pilots.ContainsKey(pilotName) = True Then

            Dim sPilot As EveHQPilot = HQ.Settings.Pilots(pilotName)

            ' Get the Connections and Diplomacy skills
            Dim diplomacyLevel As Integer = sPilot.KeySkills(KeySkill.Diplomacy)
            Dim connectionsLevel As Integer = sPilot.KeySkills(KeySkill.Connections)

            If sPilot.Standings.ContainsKey(CLng(entityID)) = True Then
                If returnEffectiveStanding = True Then
                    Dim rawStanding As Double = sPilot.Standings(CLng(entityID)).Standing
                    If rawStanding < 0 Then
                        Return rawStanding + ((10 - rawStanding)*(diplomacyLevel*4/100))
                    Else
                        Return rawStanding + ((10 - rawStanding)*(connectionsLevel*4/100))
                    End If
                Else
                    Return sPilot.Standings(CLng(entityID)).Standing
                End If
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function
End Class
