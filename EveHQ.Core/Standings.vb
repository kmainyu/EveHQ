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
Imports System.Xml
Imports EveHQ.EveAPI

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
                    Dim standingsList As XmlNodeList
                    Dim standingsNode As XmlNode
                    Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
                    Dim standingsXML As XmlDocument = apiReq.GetAPIXML(APITypes.StandingsChar, pilotAccount.ToAPIAccount, pilot.ID, APIReturnMethods.ReturnStandard)
                    If standingsXML IsNot Nothing Then

                        Dim errlist As XmlNodeList = standingsXML.SelectNodes("/eveapi/error")
                        If errlist.Count = 0 Then
                            standingsList = standingsXML.SelectNodes("/eveapi/result/characterNPCStandings/rowset")
                            If standingsList IsNot Nothing Then
                                If standingsList.Count > 0 Then

                                    For Each standingsNode In standingsList

                                        Dim currentStandingsType As StandingType = StandingType.Unknown

                                        Select Case standingsNode.Attributes.GetNamedItem("name").Value
                                            Case "agents"
                                                currentStandingsType = StandingType.Agent
                                            Case "NPCCorporations"
                                                currentStandingsType = StandingType.NPCCorporation
                                            Case "factions"
                                                currentStandingsType = StandingType.Faction
                                        End Select

                                        If standingsNode.ChildNodes.Count > 0 Then
                                            For Each entity As XmlNode In standingsNode.ChildNodes
                                                Dim newStanding As New PilotStanding
                                                newStanding.ID = CLng(entity.Attributes.GetNamedItem("fromID").Value)
                                                newStanding.Name = entity.Attributes.GetNamedItem("fromName").Value
                                                newStanding.Type = currentStandingsType
                                                newStanding.Standing = Double.Parse(entity.Attributes.GetNamedItem("standing").Value, culture)
                                                If pilot.Standings.ContainsKey(newStanding.ID) = False Then
                                                    pilot.Standings.Add(newStanding.ID, newStanding)
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
                    Dim standingsList As XmlNodeList
                    Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
                    Dim standingsXML As XmlDocument = apiReq.GetAPIXML(APITypes.ContactListChar, pilotAccount.ToAPIAccount, pilot.ID, APIReturnMethods.ReturnStandard)
                    If standingsXML IsNot Nothing Then

                        Dim errlist As XmlNodeList = standingsXML.SelectNodes("/eveapi/error")
                        If errlist.Count = 0 Then
                            standingsList = standingsXML.SelectNodes("/eveapi/result/rowset/row")
                            If standingsList IsNot Nothing Then
                                If standingsList.Count > 0 Then

                                    Const CurrentStandingsType As StandingType = StandingType.PlayerCorp

                                    For Each entity As XmlNode In standingsList

                                        Dim newStanding As New PilotStanding
                                        newStanding.ID = CLng(entity.Attributes.GetNamedItem("contactID").Value)
                                        newStanding.Name = entity.Attributes.GetNamedItem("contactName").Value
                                        newStanding.Type = CurrentStandingsType
                                        newStanding.Standing = Double.Parse(entity.Attributes.GetNamedItem("standing").Value, culture)
                                        If pilot.Standings.ContainsKey(newStanding.ID) = False Then
                                            pilot.Standings.Add(newStanding.ID, newStanding)
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
        If HQ.Settings.Pilots.ContainsKey(pilotName) = True Then

            Dim sPilot As EveHQPilot = HQ.Settings.Pilots(pilotName)

            ' Get the Connections and Diplomacy skills
            Dim diplomacyLevel As Integer = sPilot.KeySkills(KeySkill.Diplomacy)
            Dim connectionsLevel As Integer = sPilot.KeySkills(KeySkill.Connections)

            If sPilot.Standings.ContainsKey(CLng(entityID)) = True Then
                If ReturnEffectiveStanding = True Then
                    Dim rawStanding As Double = sPilot.Standings(CLng(entityID)).Standing
                    If rawStanding < 0 Then
                        Return rawStanding + ((10 - rawStanding) * (diplomacyLevel * 4 / 100))
                    Else
                        Return rawStanding + ((10 - rawStanding) * (connectionsLevel * 4 / 100))
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
