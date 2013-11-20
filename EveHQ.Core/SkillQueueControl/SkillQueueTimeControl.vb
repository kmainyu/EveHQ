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
Imports System.Windows.Forms

Namespace SkillQueueControl

    Public Class SkillQueueTimeControl

        ''' <summary>
        ''' Creates a new Skill Queue Time Control for the required pilot
        ''' </summary>
        ''' <param name="pilotName"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pilotName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            DoubleBuffered = True

            ' Add any initialization after the InitializeComponent() call.
            _currentPilotName = pilotName
            If HQ.Settings.Pilots.ContainsKey(_currentPilotName) = True Then
                _queuedSkills = HQ.Settings.Pilots(_currentPilotName).QueuedSkills
            End If

        End Sub

        ReadOnly _queuedSkills As New SortedList(Of Integer, EveHQPilotQueuedSkill)
        ReadOnly _currentPilotName As String = ""
        Dim _lastTime As Date

        Private Sub DrawTimeBar()
            Dim g As Graphics = panelSQT.CreateGraphics
            Const Sx As Integer = 2
            Const Sy As Integer = 16
            ' Calculate number of seconds from now till start
            For Each queuedSkill As EveHQPilotQueuedSkill In _queuedSkills.Values
                If SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) >= Now Then
                    Dim startSpan As TimeSpan = SkillFunctions.ConvertEveTimeToLocal(queuedSkill.StartTime) - Now
                    Dim startSec As Double = Math.Min(startSpan.TotalSeconds, 86400)
                    Dim endSpan As TimeSpan = SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) - Now
                    Dim endSec As Double = Math.Min(endSpan.TotalSeconds, 86400)
                    Dim startMark As Integer = Math.Max(CInt(startSec / 86400 * (panelSQT.Width - 4)), 0)
                    Dim endMark As Integer = CInt(endSec / 86400 * (panelSQT.Width - 4))
                    If Math.IEEERemainder(queuedSkill.Position, 2) = 0 Then
                        g.FillRectangle(Brushes.DeepSkyBlue, New RectangleF(Sx + startMark, Sy, Math.Min(endMark - startMark, panelSQT.Width - 4), 15))
                    Else
                        g.FillRectangle(Brushes.SkyBlue, New RectangleF(Sx + startMark, Sy, Math.Min(endMark - startMark, panelSQT.Width - 4), 15))
                    End If
                End If
                _lastTime = SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime)
            Next
            lblQueueEnds.Text = "Finishes: " & _lastTime.ToString
            lblQueueRemaining.Text = SkillFunctions.TimeToString((_lastTime - Now).TotalSeconds)
            Call DrawMarks()
        End Sub

        Private Sub DrawMarks()
            Dim g As Graphics = panelSQT.CreateGraphics
            Const Sx As Integer = 2
            Const Sy As Integer = 34
            Dim cx As Integer
            g.DrawLine(Pens.Silver, Sx, Sy, Sx, Sy + 6)
            g.DrawLine(Pens.Silver, panelSQT.Width - 4, Sy, panelSQT.Width - 4, Sy + 6)
            For timeMark As Integer = 1 To 23
                cx = (panelSQT.Width - 4)
                If Math.IEEERemainder(timeMark, 6) = 0 Then
                    g.DrawLine(Pens.Silver, CInt(cx / 24 * timeMark), Sy, CInt(cx / 24 * timeMark), Sy + 6)
                Else
                    g.DrawLine(Pens.Silver, CInt(cx / 24 * timeMark), Sy, CInt(cx / 24 * timeMark), Sy + 3)
                End If
            Next
            Dim gFont As New Font("Tahoma", 6)
            g.DrawString("0", gFont, Brushes.Silver, Sx + 2, 35)
            g.DrawString("24", gFont, Brushes.Silver, panelSQT.Width - 15, 35)
        End Sub

        Private Sub panelSQT_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles panelSQT.Paint
            Call DrawTimeBar()
        End Sub

        Private Sub tmrUpdate_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrUpdate.Tick
            Call UpdateTime()
        End Sub

        Private Sub UpdateTime()
            lblQueueRemaining.Text = SkillFunctions.TimeToString((_lastTime - Now).TotalSeconds)
        End Sub
    End Class
End NameSpace