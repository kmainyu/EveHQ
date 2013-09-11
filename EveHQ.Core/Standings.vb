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
Imports System.Xml

Public Class Standings

    Public Shared Sub GetStandings(ByVal pilotName As String)

        If EveHQ.Core.HQ.Settings.Pilots.ContainsKey(pilotName) = True Then
            Dim pilot As EveHQ.Core.EveHQPilot = EveHQ.Core.HQ.Settings.Pilots(pilotName)

            ' Clear the existing standings
            pilot.Standings.Clear()

            ' Set culture info
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

            ' Get Account info for the API
            Dim accountName As String = pilot.Account
            If EveHQ.Core.HQ.Settings.Accounts.ContainsKey(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveHQAccount = EveHQ.Core.HQ.Settings.Accounts.Item(accountName)

                ' Stage 1 - Get the NPC Standings
                Try
                    Dim StandingsList As XmlNodeList
                    Dim StandingsNode As XmlNode
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.Settings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    Dim StandingsXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.StandingsChar, pilotAccount.ToAPIAccount, pilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
                    If StandingsXML IsNot Nothing Then

                        Dim errlist As XmlNodeList = StandingsXML.SelectNodes("/eveapi/error")
                        If errlist.Count = 0 Then
                            StandingsList = StandingsXML.SelectNodes("/eveapi/result/characterNPCStandings/rowset")
                            If StandingsList IsNot Nothing Then
                                If StandingsList.Count > 0 Then

                                    For Each StandingsNode In StandingsList

                                        Dim CurrentStandingsType As StandingType = StandingType.Unknown

                                        Select Case StandingsNode.Attributes.GetNamedItem("name").Value
                                            Case "agents"
                                                CurrentStandingsType = StandingType.Agent
                                            Case "NPCCorporations"
                                                CurrentStandingsType = StandingType.NPCCorporation
                                            Case "factions"
                                                CurrentStandingsType = StandingType.Faction
                                        End Select

                                        If StandingsNode.ChildNodes.Count > 0 Then
                                            For Each Entity As XmlNode In StandingsNode.ChildNodes
                                                Dim NewStanding As New PilotStanding
                                                NewStanding.ID = CLng(Entity.Attributes.GetNamedItem("fromID").Value)
                                                NewStanding.Name = Entity.Attributes.GetNamedItem("fromName").Value
                                                NewStanding.Type = CurrentStandingsType
                                                NewStanding.Standing = Double.Parse(Entity.Attributes.GetNamedItem("standing").Value, culture)
                                                If pilot.Standings.ContainsKey(NewStanding.ID) = False Then
                                                    pilot.Standings.Add(NewStanding.ID, NewStanding)
                                                End If
                                            Next
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
                    Dim StandingsList As XmlNodeList
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.Settings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    Dim StandingsXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.ContactListChar, pilotAccount.ToAPIAccount, pilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
                    If StandingsXML IsNot Nothing Then

                        Dim errlist As XmlNodeList = StandingsXML.SelectNodes("/eveapi/error")
                        If errlist.Count = 0 Then
                            StandingsList = StandingsXML.SelectNodes("/eveapi/result/rowset/row")
                            If StandingsList IsNot Nothing Then
                                If StandingsList.Count > 0 Then

                                    Dim CurrentStandingsType As StandingType = StandingType.PlayerCorp

                                    For Each Entity As XmlNode In StandingsList

                                        Dim NewStanding As New PilotStanding
                                        NewStanding.ID = CLng(Entity.Attributes.GetNamedItem("contactID").Value)
                                        NewStanding.Name = Entity.Attributes.GetNamedItem("contactName").Value
                                        NewStanding.Type = CurrentStandingsType
                                        NewStanding.Standing = Double.Parse(Entity.Attributes.GetNamedItem("standing").Value, culture)
                                        If pilot.Standings.ContainsKey(NewStanding.ID) = False Then
                                            pilot.Standings.Add(NewStanding.ID, NewStanding)
                                        End If

                                    Next

                                End If
                            End If
                        End If
                    End If
                Catch e As Exception
                    ' Just skip and exit
                End Try

            End If

        End If

    End Sub

    Public Shared Function GetStanding(ByVal pilotName As String, ByVal entityID As String, ByVal returnEffectiveStanding As Boolean) As Double
        ' Try and get the standings data
        If EveHQ.Core.HQ.Settings.Pilots.ContainsKey(pilotName) = True Then

            Dim sPilot As EveHQ.Core.EveHQPilot = EveHQ.Core.HQ.Settings.Pilots(pilotName)

            ' Get the Connections and Diplomacy skills
            Dim diplomacyLevel As Integer = sPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Diplomacy)
            Dim connectionsLevel As Integer = sPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Connections)

            If sPilot.Standings.ContainsKey(CLng(entityID)) = True Then
                If ReturnEffectiveStanding = True Then
                    Dim RawStanding As Double = sPilot.Standings(CLng(entityID)).Standing
                    If RawStanding < 0 Then
                        Return RawStanding + ((10 - RawStanding) * (diplomacyLevel * 4 / 100))
                    Else
                        Return RawStanding + ((10 - RawStanding) * (connectionsLevel * 4 / 100))
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
