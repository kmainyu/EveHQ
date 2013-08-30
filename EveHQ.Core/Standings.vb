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

    Public Shared Sub GetStandings(ByVal PilotName As String)

        If EveHQ.Core.HQ.EveHqSettings.Pilots.Contains(PilotName) = True Then
			Dim Pilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHqSettings.Pilots(PilotName), Core.Pilot)

			' Clear the existing standings
			Pilot.Standings.Clear()

            ' Set culture info
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

            ' Get Account info for the API
            Dim accountName As String = Pilot.Account
            If EveHQ.Core.HQ.EveHqSettings.Accounts.Contains(accountName) = True Then
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHqSettings.Accounts.Item(accountName), Core.EveAccount)

                ' Stage 1 - Get the NPC Standings
                Try
                    Dim StandingsList As XmlNodeList
                    Dim StandingsNode As XmlNode
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHqSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    Dim StandingsXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.StandingsChar, pilotAccount.ToAPIAccount, Pilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
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
                                                If Pilot.Standings.ContainsKey(NewStanding.ID) = False Then
                                                    Pilot.Standings.Add(NewStanding.ID, NewStanding)
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
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHqSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    Dim StandingsXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.ContactListChar, pilotAccount.ToAPIAccount, Pilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
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
                                        If Pilot.Standings.ContainsKey(NewStanding.ID) = False Then
                                            Pilot.Standings.Add(NewStanding.ID, NewStanding)
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

    Public Shared Function GetStanding(ByVal PilotName As String, ByVal EntityID As String, ByVal ReturnEffectiveStanding As Boolean) As Double
        ' Try and get the standings data
        If EveHQ.Core.HQ.EveHqSettings.Pilots.Contains(PilotName) = True Then

            Dim SPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHqSettings.Pilots(PilotName), EveHQ.Core.Pilot)

            ' Get the Connections and Diplomacy skills
            Dim DiplomacyLevel As Integer = CInt(SPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Diplomacy))
            Dim ConnectionsLevel As Integer = CInt(SPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Connections))

            If SPilot.Standings.ContainsKey(CLng(EntityID)) = True Then
                If ReturnEffectiveStanding = True Then
                    Dim RawStanding As Double = SPilot.Standings(CLng(EntityID)).Standing
                    If RawStanding < 0 Then
                        Return RawStanding + ((10 - RawStanding) * (DiplomacyLevel * 4 / 100))
                    Else
                        Return RawStanding + ((10 - RawStanding) * (ConnectionsLevel * 4 / 100))
                    End If
                Else
                    Return SPilot.Standings(CLng(EntityID)).Standing
                End If
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function
End Class
