Imports System.Windows.Forms

Public Class frmExtraStandings

    Dim cPilot As String = ""
    Dim cParty As String = ""
    Dim cStanding As Double = 0

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

    Private Sub CalculateMissions()
        Dim curStanding As Double = cStanding
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
                    If CDbl(gain) > 0 And CDbl(gain) < 100 Then
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
            lblGainAverage.Text = "Average: " & FormatNumber(missionGain, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        End If
        Dim missionCount As Integer = 0
        lvwStandings.BeginUpdate()
        lvwStandings.Items.Clear()
        Dim newStand As New ListViewItem
        If missionGain <> 0 Then
            Do While curStanding < reqStanding
                missionCount += 1
                newStand = New ListViewItem
                newStand.Text = missionCount.ToString
                newStand.SubItems.Add(FormatNumber(curStanding, 10, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                newStand.SubItems.Add(FormatNumber(missionGain, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                newStanding = curStanding + (missionGain * (10 - curStanding) / 100)
                newStand.SubItems.Add(FormatNumber(newStanding, 10, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                If Int(curStanding) <> Int(newStanding) Then
                    newStand.BackColor = Drawing.Color.LightSteelBlue
                End If
                lvwStandings.Items.Add(newStand)
                curStanding = newStanding
            Loop

            lblMissionsRequired.Text = FormatNumber(missionCount, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Else
            lblMissionsRequired.Text = "Infinite!"
        End If
        lvwStandings.EndUpdate()

    End Sub

    Private Sub frmExtraStandings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Standings Extrapolation - " & Party
        Me.lblCurrentStanding.Text = cStanding.ToString
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
End Class