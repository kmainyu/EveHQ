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

    Public Class SkillQueueBlock

        ReadOnly _currentPilot As New EveHQPilot
        ReadOnly _currentPilotName As String = ""
        ReadOnly _currentQueuedSkill As New EveHQPilotQueuedSkill
        ReadOnly _currentSkill As New EveHQPilotSkill
        ReadOnly _trainedLevel As Integer = 0
        Dim _percent As Double = 0

        Private Sub DrawSkillLevelBlock()
            Dim g As Graphics = panelSQB.CreateGraphics
            Dim sx As Integer = panelSQB.Width - 60
            Const Sy As Integer = 7

            g.DrawRectangle(New Pen(Color.Silver, 1), New Rectangle(sx, Sy, 47, 9))
            If _trainedLevel > 0 Then
                For lvl As Integer = 1 To _trainedLevel
                    g.FillRectangle(Brushes.White, New RectangleF(sx + (lvl * 9) - 7, Sy + 2, 8, 6))
                Next
            End If
            For lvl As Integer = _trainedLevel + 1 To Math.Min(_currentQueuedSkill.Level, 5)
                g.FillRectangle(Brushes.DeepSkyBlue, New RectangleF(sx + (lvl * 9) - 7, Sy + 2, 8, 6))
            Next
        End Sub

        Private Sub DrawTimeBar()
            Dim g As Graphics = panelSQB.CreateGraphics
            Const Sx As Integer = 2
            Dim sy As Integer = panelSQB.Height - 5
            g.FillRectangle(Brushes.DimGray, New RectangleF(Sx, sy, panelSQB.Width - 4, 4))
            g.FillRectangle(Brushes.Silver, New RectangleF(Sx, sy + 3, panelSQB.Width - 4, 1))
            ' Calculate number of seconds from now till start
            Dim startSpan As TimeSpan = SkillFunctions.ConvertEveTimeToLocal(_currentQueuedSkill.StartTime) - Now
            Dim startSec As Double = Math.Min(startSpan.TotalSeconds, 86400)
            Dim endSpan As TimeSpan = SkillFunctions.ConvertEveTimeToLocal(_currentQueuedSkill.EndTime) - Now
            Dim endSec As Double = Math.Min(endSpan.TotalSeconds, 86400)
            Dim startMark As Integer = Math.Max(CInt(startSec / 86400 * (panelSQB.Width - 4)), 0)
            Dim endMark As Integer = CInt(endSec / 86400 * (panelSQB.Width - 4))
            If Math.IEEERemainder(_currentQueuedSkill.Position, 2) = 0 Then
                g.FillRectangle(Brushes.DeepSkyBlue, New RectangleF(Sx + startMark, sy, Math.Min(endMark - startMark, panelSQB.Width - 4), 3))
            Else
                g.FillRectangle(Brushes.SkyBlue, New RectangleF(Sx + startMark, sy, Math.Min(endMark - startMark, panelSQB.Width - 4), 3))
            End If
        End Sub

        Private Sub DrawPercentBar()
            Dim g As Graphics = panelSQB.CreateGraphics
            Dim sx As Integer = panelSQB.Width - 60
            Const Sy As Integer = 24
            g.DrawRectangle(New Pen(Color.Silver, 1), New Rectangle(sx, Sy, 47, 7))
            g.FillRectangle(Brushes.White, New RectangleF(sx, Sy + 2, 1, 4))
            g.FillRectangle(Brushes.White, New RectangleF(sx + 47, Sy + 2, 1, 4))
            Dim endMark As Integer = CInt(_percent * 0.44)
            g.FillRectangle(Brushes.Silver, New RectangleF(sx + 2, Sy + 2, endMark, 4))
        End Sub

        Private Sub panelSQB_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles panelSQB.Paint
            Call DrawSkillLevelBlock()
            Call DrawTimeBar()
            Call DrawPercentBar()
        End Sub

        ''' <summary>
        ''' Creates a new Skill Queue Block control using a pilot and a queued skill
        ''' </summary>
        ''' <param name="pilotName"></param>
        ''' <param name="queuedSkill"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pilotName As String, ByVal queuedSkill As EveHQPilotQueuedSkill)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Set the current pilot and queued skill
            _currentPilotName = pilotName
            _currentQueuedSkill = QueuedSkill

            ' Establish current skill & calculate level and percentage
            If HQ.Settings.Pilots.ContainsKey(_currentPilotName) = True Then
                _currentPilot = HQ.Settings.Pilots(_currentPilotName)
                If _currentPilot.PilotSkills.ContainsKey(SkillFunctions.SkillIDToName(_currentQueuedSkill.SkillID)) = True Then
                    _currentSkill = _currentPilot.PilotSkills(SkillFunctions.SkillIDToName(_currentQueuedSkill.SkillID))
                    lblSkillName.Text = _currentSkill.Name & " (" & _currentSkill.Rank.ToString & "x)"
                    lblSkillLevel.Text = "Level " & QueuedSkill.Level.ToString
                    _trainedLevel = _currentSkill.Level
                    ' Calculatate percentage
                    If _currentQueuedSkill.SkillID = _currentPilot.TrainingSkillID And _currentQueuedSkill.Level = _currentPilot.TrainingSkillLevel Then
                        _percent = (Math.Min(Math.Max(CDbl((_currentSkill.SP + _currentPilot.TrainingCurrentSP - HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel)) / (HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel + 1) - HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel)) * 100), 0), 100))
                    Else
                        _percent = (Math.Min(Math.Max(CDbl((_currentSkill.SP - HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel)) / (HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel + 1) - HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel)) * 100), 0), 100))
                    End If
                    If SkillFunctions.ConvertEveTimeToLocal(_currentQueuedSkill.StartTime) < Now And SkillFunctions.ConvertEveTimeToLocal(_currentQueuedSkill.EndTime) >= Now Then
                        lblTimeToTrain.Text = SkillFunctions.TimeToString((SkillFunctions.ConvertEveTimeToLocal(_currentQueuedSkill.EndTime) - Now).TotalSeconds)
                        PictureBox1.Image = My.Resources.SkillBook32
                    Else
                        lblTimeToTrain.Text = SkillFunctions.TimeToString((QueuedSkill.EndTime - QueuedSkill.StartTime).TotalSeconds)
                    End If
                Else
                    _trainedLevel = 0
                    _percent = 0
                End If
            Else
                _trainedLevel = 0
                _percent = 0
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
            _currentQueuedSkill = New EveHQPilotQueuedSkill
            _trainedLevel = 0
            _percent = 0
        End Sub

        Private Sub tmrUpdate_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrUpdate.Tick
            If SkillFunctions.ConvertEveTimeToLocal(_currentQueuedSkill.StartTime) < Now And SkillFunctions.ConvertEveTimeToLocal(_currentQueuedSkill.EndTime) >= Now Then
                lblTimeToTrain.Text = SkillFunctions.TimeToString((SkillFunctions.ConvertEveTimeToLocal(_currentQueuedSkill.EndTime) - Now).TotalSeconds)
                If _currentQueuedSkill.SkillID = CDbl(_currentPilot.TrainingSkillID) And _currentQueuedSkill.Level = _currentPilot.TrainingSkillLevel Then
                    _percent = (Math.Min(Math.Max(CDbl((_currentSkill.SP + _currentPilot.TrainingCurrentSP - HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel)) / (HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel + 1) - HQ.SkillListName(_currentSkill.Name).LevelUp(_trainedLevel)) * 100), 0), 100))
                End If
            End If
            Call DrawTimeBar()
            Call DrawPercentBar()
        End Sub
    End Class
End Namespace