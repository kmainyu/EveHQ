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
Imports System.Drawing

Public Class SkillQueueBlock

    Dim currentPilot As New EveHQ.Core.Pilot
    Dim CurrentPilotName As String = ""
    Dim CurrentQueuedSkill As New EveHQ.Core.PilotQueuedSkill
    Dim currentSkill As New EveHQ.Core.PilotSkill
    Dim TrainedLevel As Integer = 0
    Dim Percent As Double = 0

    Private Sub DrawSkillLevelBlock()
        Dim g As Graphics = panelSQB.CreateGraphics
        Dim sx As Integer = panelSQB.Width - 60
        Dim sy As Integer = 7

        g.DrawRectangle(New Pen(Color.Silver, 1), New Rectangle(sx, sy, 47, 9))
        If TrainedLevel > 0 Then
            For lvl As Integer = 1 To TrainedLevel
                g.FillRectangle(Brushes.White, New RectangleF(sx + (lvl * 9) - 7, sy + 2, 8, 6))
            Next
        End If
        For lvl As Integer = TrainedLevel + 1 To Math.Min(CurrentQueuedSkill.Level, 5)
            g.FillRectangle(Brushes.DeepSkyBlue, New RectangleF(sx + (lvl * 9) - 7, sy + 2, 8, 6))
        Next
    End Sub

    Private Sub DrawTimeBar()
        Dim g As Graphics = panelSQB.CreateGraphics
        Dim sx As Integer = 2
        Dim sy As Integer = panelSQB.Height - 5
        g.FillRectangle(Brushes.DimGray, New RectangleF(sx, sy, panelSQB.Width - 4, 4))
        g.FillRectangle(Brushes.Silver, New RectangleF(sx, sy + 3, panelSQB.Width - 4, 1))
        ' Calculate number of seconds from now till start
        Dim startSpan As TimeSpan = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(CurrentQueuedSkill.StartTime) - Now
        Dim startSec As Double = Math.Min(startSpan.TotalSeconds, 86400)
        Dim endSpan As TimeSpan = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(CurrentQueuedSkill.EndTime) - Now
        Dim endSec As Double = Math.Min(endSpan.TotalSeconds, 86400)
        Dim startMark As Integer = Math.Max(CInt(startSec / 86400 * (panelSQB.Width - 4)), 0)
        Dim endMark As Integer = CInt(endSec / 86400 * (panelSQB.Width - 4))
        If Math.IEEERemainder(CurrentQueuedSkill.Position, 2) = 0 Then
            g.FillRectangle(Brushes.DeepSkyBlue, New RectangleF(sx + startMark, sy, Math.Min(endMark - startMark, panelSQB.Width - 4), 3))
        Else
            g.FillRectangle(Brushes.SkyBlue, New RectangleF(sx + startMark, sy, Math.Min(endMark - startMark, panelSQB.Width - 4), 3))
        End If
    End Sub

    Private Sub DrawPercentBar()
        Dim g As Graphics = panelSQB.CreateGraphics
        Dim sx As Integer = panelSQB.Width - 60
        Dim sy As Integer = 24
        g.DrawRectangle(New Pen(Color.Silver, 1), New Rectangle(sx, sy, 47, 7))
        g.FillRectangle(Brushes.White, New RectangleF(sx, sy + 2, 1, 4))
        g.FillRectangle(Brushes.White, New RectangleF(sx + 47, sy + 2, 1, 4))
        Dim endMark As Integer = CInt(Percent * 0.44)
        g.FillRectangle(Brushes.Silver, New RectangleF(sx + 2, sy + 2, endMark, 4))
    End Sub

    Private Sub panelSQB_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles panelSQB.Paint
        Call DrawSkillLevelBlock()
        Call DrawTimeBar()
        Call DrawPercentBar()
    End Sub

    ''' <summary>
    ''' Creates a new Skill Queue Block control using a pilot and a queued skill
    ''' </summary>
    ''' <param name="PilotName"></param>
    ''' <param name="QueuedSkill"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal PilotName As String, ByVal QueuedSkill As EveHQ.Core.PilotQueuedSkill)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the current pilot and queued skill
        CurrentPilotName = PilotName
        CurrentQueuedSkill = QueuedSkill

        ' Establish current skill & calculate level and percentage
        If EveHQ.Core.HQ.EveHqSettings.Pilots.Contains(CurrentPilotName) = True Then
            currentPilot = CType(EveHQ.Core.HQ.EveHqSettings.Pilots(CurrentPilotName), EveHQ.Core.Pilot)
            If currentPilot.PilotSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(CurrentQueuedSkill.SkillID))) = True Then
                currentSkill = CType(currentPilot.PilotSkills(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(CurrentQueuedSkill.SkillID))), EveHQ.Core.PilotSkill)
                lblSkillName.Text = currentSkill.Name & " (" & currentSkill.Rank.ToString & "x)"
                lblSkillLevel.Text = "Level " & QueuedSkill.Level.ToString
                TrainedLevel = currentSkill.Level
                ' Calculatate percentage
                If CurrentQueuedSkill.SkillID = CDbl(currentPilot.TrainingSkillID) And CurrentQueuedSkill.Level = currentPilot.TrainingSkillLevel Then
                    Percent = (Math.Min(Math.Max(CDbl((currentSkill.SP + currentPilot.TrainingCurrentSP - currentSkill.LevelUp(TrainedLevel)) / (currentSkill.LevelUp(TrainedLevel + 1) - currentSkill.LevelUp(TrainedLevel)) * 100), 0), 100))
                Else
                    Percent = (Math.Min(Math.Max(CDbl((currentSkill.SP - currentSkill.LevelUp(TrainedLevel)) / (currentSkill.LevelUp(TrainedLevel + 1) - currentSkill.LevelUp(TrainedLevel)) * 100), 0), 100))
                End If
                If EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(CurrentQueuedSkill.StartTime) < Now And EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(CurrentQueuedSkill.EndTime) >= Now Then
                    lblTimeToTrain.Text = EveHQ.Core.SkillFunctions.TimeToString((EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(CurrentQueuedSkill.EndTime) - Now).TotalSeconds)
                    PictureBox1.Image = My.Resources.SkillBook32
                Else
                    lblTimeToTrain.Text = EveHQ.Core.SkillFunctions.TimeToString((QueuedSkill.EndTime - QueuedSkill.StartTime).TotalSeconds)
                End If
            Else
                TrainedLevel = 0
                Percent = 0
            End If
            Else
                TrainedLevel = 0
                Percent = 0
            End If

        ' Start the timer
        tmrUpdate.Enabled = True
        tmrUpdate.Start()

    End Sub

    ''' <summary>
    ''' Creates a blank Skill Queue Block control
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the current pilot and queued skill
        CurrentQueuedSkill = New EveHQ.Core.PilotQueuedSkill
        TrainedLevel = 0
        Percent = 0
    End Sub

    Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        If EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(CurrentQueuedSkill.StartTime) < Now And EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(CurrentQueuedSkill.EndTime) >= Now Then
            lblTimeToTrain.Text = EveHQ.Core.SkillFunctions.TimeToString((EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(CurrentQueuedSkill.EndTime) - Now).TotalSeconds)
            If CurrentQueuedSkill.SkillID = CDbl(currentPilot.TrainingSkillID) And CurrentQueuedSkill.Level = currentPilot.TrainingSkillLevel Then
                Percent = (Math.Min(Math.Max(CDbl((currentSkill.SP + currentPilot.TrainingCurrentSP - currentSkill.LevelUp(TrainedLevel)) / (currentSkill.LevelUp(TrainedLevel + 1) - currentSkill.LevelUp(TrainedLevel)) * 100), 0), 100))
            End If
        End If
        Call Me.DrawTimeBar()
        Call Me.DrawPercentBar()
    End Sub
End Class
