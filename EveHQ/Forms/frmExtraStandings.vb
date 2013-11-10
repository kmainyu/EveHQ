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


Namespace Forms

    Public Class FrmExtraStandings

        Dim _pilot As String = ""
        Dim _party As String = ""
        Dim _standing As Double = 0
        Dim _baseStanding As Double = 0

        Public Property Pilot() As String
            Get
                Return _pilot
            End Get
            Set(ByVal value As String)
                _pilot = value
            End Set
        End Property
        Public Property Party() As String
            Get
                Return _party
            End Get
            Set(ByVal value As String)
                _party = value
            End Set
        End Property
        Public Property Standing() As Double
            Get
                Return _standing
            End Get
            Set(ByVal value As Double)
                _standing = value
            End Set
        End Property
        Public Property BaseStanding() As Double
            Get
                Return _baseStanding
            End Get
            Set(ByVal value As Double)
                _baseStanding = value
            End Set
        End Property

        Private Sub CalculateMissions()
            Dim curStanding As Double
            If chkUseBaseOnly.Checked = True Then
                curStanding = _baseStanding
            Else
                curStanding = _standing
            End If
            Dim newStanding As Double = 0
            Dim reqStanding As Double = CDbl(nudReqStanding.Value)
            Dim missionGain As Double

            ' Calculate Average
            If radDirect.Checked = True Then
                missionGain = CDbl(nudMissionGain.Value)
            Else
                Dim gainList As String = txtGains.Text
                Dim gains() As String = gainList.Split(ControlChars.CrLf.ToCharArray)
                Dim gainTotal As Double = 0
                Dim gainCount As Integer = 0
                For Each gain As String In gains
                    If IsNumeric(gain) = True Then
                        If CDbl(gain) > -100 And CDbl(gain) < 100 Then
                            gainTotal += CDbl(gain)
                            gainCount += 1
                        End If
                    End If
                Next
                If gainCount = 0 Then
                    missionGain = 0
                Else
                    missionGain = gainTotal / gainCount
                End If
                lblGainAverage.Text = "Average: " & missionGain.ToString("N4")
            End If
            Dim missionCount As Integer = 0
            lvwStandings.BeginUpdate()
            lvwStandings.Items.Clear()
            Dim newStand As ListViewItem
            If missionGain <> 0 Then
                Do While curStanding < reqStanding And curStanding > -10
                    missionCount += 1
                    newStand = New ListViewItem
                    newStand.Text = missionCount.ToString
                    newStand.SubItems.Add(curStanding.ToString("N10"))
                    newStand.SubItems.Add(missionGain.ToString("N4"))
                    newStanding = curStanding + (missionGain * (10 - curStanding) / 100)
                    If newStanding <= -10 Then newStanding = -10
                    newStand.SubItems.Add(newStanding.ToString("N10"))
                    If Int(curStanding) <> Int(newStanding) Then
                        newStand.BackColor = Color.LightSteelBlue
                    End If
                    lvwStandings.Items.Add(newStand)
                    curStanding = newStanding
                Loop
                If newStanding = -10 Then
                    lblMissionsRequired.Text = "Infinite Missions Required!"
                Else
                    lblMissionsRequired.Text = "Missions Required: " & missionCount.ToString("N0")
                End If
            Else
                lblMissionsRequired.Text = "Infinite Missions Required!"
            End If
            lvwStandings.EndUpdate()

        End Sub

        Private Sub frmExtraStandings_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Text = "Standings Extrapolation - " & Party
            lblCurrentStanding.Text = _standing.ToString("N10")
            lblCurrentBaseStanding.Text = _baseStanding.ToString("N10")
            CalculateMissions()
        End Sub

        Private Sub nudReqStanding_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudReqStanding.ValueChanged
            Call CalculateMissions()
        End Sub

        Private Sub nudMissionGain_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudMissionGain.ValueChanged
            Call CalculateMissions()
        End Sub

        Private Sub txtGains_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtGains.TextChanged
            Call CalculateMissions()
        End Sub

        Private Sub radDirect_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radDirect.CheckedChanged
            If radDirect.Checked = True Then
                lblAvgMission.Enabled = True
                nudMissionGain.Enabled = True
                txtGains.Enabled = False
                lblGainAverage.Enabled = False
            Else
                lblAvgMission.Enabled = False
                nudMissionGain.Enabled = False
                txtGains.Enabled = True
                lblGainAverage.Enabled = True
            End If
            CalculateMissions()
        End Sub

        Private Sub chkUseBaseOnly_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseBaseOnly.CheckedChanged
            Call CalculateMissions()
        End Sub
    End Class
End NameSpace