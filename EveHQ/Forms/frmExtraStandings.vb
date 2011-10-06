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
Imports System.Windows.Forms

Public Class frmExtraStandings

    Dim cPilot As String = ""
    Dim cParty As String = ""
    Dim cStanding As Double = 0
    Dim cBaseStanding As Double = 0

    Public Property Pilot() As String
        Get
            Return cPilot
        End Get
        Set(ByVal value As String)
            cPilot = value
        End Set
    End Property
    Public Property Party() As String
        Get
            Return cParty
        End Get
        Set(ByVal value As String)
            cParty = value
        End Set
    End Property
    Public Property Standing() As Double
        Get
            Return cStanding
        End Get
        Set(ByVal value As Double)
            cStanding = value
        End Set
    End Property
    Public Property BaseStanding() As Double
        Get
            Return cBaseStanding
        End Get
        Set(ByVal value As Double)
            cBaseStanding = value
        End Set
    End Property

    Private Sub CalculateMissions()
        Dim curStanding As Double = 0
        If chkUseBaseOnly.Checked = True Then
            curStanding = cBaseStanding
        Else
            curStanding = cStanding
        End If
        Dim newStanding As Double = 0
        Dim reqStanding As Double = CDbl(nudReqStanding.Value)
        Dim missionGain As Double = 0

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
        Dim newStand As New ListViewItem
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
                    newStand.BackColor = Drawing.Color.LightSteelBlue
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

    Private Sub frmExtraStandings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Standings Extrapolation - " & Party
        Me.lblCurrentStanding.Text = cStanding.ToString("N10")
        Me.lblCurrentBaseStanding.Text = cBaseStanding.ToString("N10")
        Me.CalculateMissions()
    End Sub

    Private Sub nudReqStanding_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudReqStanding.ValueChanged
        Call Me.CalculateMissions()
    End Sub

    Private Sub nudMissionGain_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMissionGain.ValueChanged
        Call Me.CalculateMissions()
    End Sub

    Private Sub txtGains_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGains.TextChanged
        Call CalculateMissions()
    End Sub

    Private Sub radDirect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radDirect.CheckedChanged
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

    Private Sub chkUseBaseOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseBaseOnly.CheckedChanged
        Call CalculateMissions()
    End Sub
End Class