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
Imports System.Drawing

Public Class SkillQueueTimeControl

    ''' <summary>
    ''' Creates a new Skill Queue Time Control for the required pilot
    ''' </summary>
    ''' <param name="PilotName"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal PilotName As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.DoubleBuffered = True

        ' Add any initialization after the InitializeComponent() call.
        CurrentPilotName = PilotName
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(currentPilotName) = True Then
            QueuedSkills = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentPilotName), EveHQ.Core.Pilot).QueuedSkills
        End If

    End Sub

    Dim QueuedSkills As New SortedList(Of Long, EveHQ.Core.PilotQueuedSkill)
    Dim currentPilotName As String = ""
    Dim lastTime As Date

    Private Sub DrawTimeBar()
        Dim g As Graphics = panelSQT.CreateGraphics
        Dim sx As Integer = 2
        Dim sy As Integer = 16
        ' Calculate number of seconds from now till start
        For Each QueuedSkill As EveHQ.Core.PilotQueuedSkill In QueuedSkills.Values
            If EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.EndTime) >= Now Then
                Dim startSpan As TimeSpan = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.StartTime) - Now
                Dim startSec As Double = Math.Min(startSpan.TotalSeconds, 86400)
                Dim endSpan As TimeSpan = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.EndTime) - Now
                Dim endSec As Double = Math.Min(endSpan.TotalSeconds, 86400)
                Dim startMark As Integer = Math.Max(CInt(startSec / 86400 * (panelSQT.Width - 4)), 0)
                Dim endMark As Integer = CInt(endSec / 86400 * (panelSQT.Width - 4))
                If Math.IEEERemainder(QueuedSkill.Position, 2) = 0 Then
                    g.FillRectangle(Brushes.DeepSkyBlue, New RectangleF(sx + startMark, sy, Math.Min(endMark - startMark, panelSQT.Width - 4), 15))
                Else
                    g.FillRectangle(Brushes.SkyBlue, New RectangleF(sx + startMark, sy, Math.Min(endMark - startMark, panelSQT.Width - 4), 15))
                End If
            End If
            lastTime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.EndTime)
        Next
        lblQueueEnds.Text = "Finishes: " & FormatDateTime(lastTime, DateFormat.GeneralDate)
        lblQueueRemaining.Text = EveHQ.Core.SkillFunctions.TimeToString((lastTime - Now).TotalSeconds)
        Call Me.DrawMarks()
    End Sub

    Private Sub DrawMarks()
        Dim g As Graphics = panelSQT.CreateGraphics
        Dim sx As Integer = 2
        Dim sy As Integer = 34
        Dim cx As Integer = 0
        Dim cy As Integer = 3
        g.DrawLine(Pens.Silver, sx, sy, sx, sy + 6)
        g.DrawLine(Pens.Silver, panelSQT.Width - 4, sy, panelSQT.Width - 4, sy + 6)
        For timeMark As Integer = 1 To 23
            cx = (panelSQT.Width - 4)
            If Math.IEEERemainder(timeMark, 6) = 0 Then
                g.DrawLine(Pens.Silver, CInt(cx / 24 * timeMark), sy, CInt(cx / 24 * timeMark), sy + 6)
            Else
                g.DrawLine(Pens.Silver, CInt(cx / 24 * timeMark), sy, CInt(cx / 24 * timeMark), sy + 3)
            End If
        Next
        Dim gFont As New Font("Tahoma", 6)
        g.DrawString("0", gFont, Brushes.Silver, sx + 2, 35)
        g.DrawString("24", gFont, Brushes.Silver, panelSQT.Width - 15, 35)
    End Sub

    Private Sub panelSQT_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles panelSQT.Paint
        Call Me.DrawTimeBar()
    End Sub

    Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        Call Me.UpdateTime()
    End Sub

    Private Sub UpdateTime()
        lblQueueRemaining.Text = EveHQ.Core.SkillFunctions.TimeToString((lastTime - Now).TotalSeconds)
    End Sub
End Class
