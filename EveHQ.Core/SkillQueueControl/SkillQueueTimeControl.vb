'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

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
            Dim queueTimeLength As Integer = HQ.Settings.EveQueueDisplayLength * 86400
            ' Calculate number of seconds from now till start
            For Each queuedSkill As EveHQPilotQueuedSkill In _queuedSkills.Values
                If SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) >= Now Then
                    Dim startSpan As TimeSpan = SkillFunctions.ConvertEveTimeToLocal(queuedSkill.StartTime) - Now
                    Dim startSec As Double = Math.Min(startSpan.TotalSeconds, queueTimeLength)
                    Dim endSpan As TimeSpan = SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) - Now
                    Dim endSec As Double = Math.Min(endSpan.TotalSeconds, queueTimeLength)
                    Dim startMark As Integer = Math.Max(CInt(startSec / queueTimeLength * (panelSQT.Width - 4)), 0)
                    Dim endMark As Integer = CInt(endSec / queueTimeLength * (panelSQT.Width - 4))
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
            Dim gFont As New Font("Tahoma", 6)
            Dim g As Graphics = panelSQT.CreateGraphics
            Const Sx As Integer = 2
            Const Sy As Integer = 34
            Dim cx As Integer
            Dim queueTimeLength As Integer = HQ.Settings.EveQueueDisplayLength * 24
            Dim stepAmount As Integer
            Select Case HQ.Settings.EveQueueDisplayLength
                Case 1 To 2
                    stepAmount = 1
                Case 3 To 4
                    stepAmount = 2
                Case 5 To 8
                    stepAmount = 3
                Case Is > 8
                    stepAmount = 6
            End Select
            For timeMark As Integer = 0 To queueTimeLength Step stepAmount
                cx = (panelSQT.Width - 4)
                If Math.IEEERemainder(timeMark, 6) = 0 Then
                    g.DrawLine(Pens.Silver, CInt(cx / queueTimeLength * timeMark), Sy, CInt(cx / queueTimeLength * timeMark), Sy + 2)
                Else
                    g.DrawLine(Pens.Silver, CInt(cx / queueTimeLength * timeMark), Sy, CInt(cx / queueTimeLength * timeMark), Sy + 4)
                End If
                If Math.IEEERemainder(timeMark, 24) = 0 Then
                    If HQ.Settings.EveQueueDisplayLength > 7 Then
                        g.DrawString((timeMark / 24).ToString, gFont, Brushes.Silver, CInt(cx / queueTimeLength * timeMark) - g.MeasureString((timeMark / 24).ToString, gFont).Width / 2, 36)
                    Else
                        g.DrawString(timeMark.ToString, gFont, Brushes.Silver, CInt(cx / queueTimeLength * timeMark) - g.MeasureString(timeMark.ToString, gFont).Width / 2, 36)
                    End If
                End If
            Next
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