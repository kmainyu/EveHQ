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

Imports System.Windows.Forms

Namespace SkillQueueControl

    ''' <summary>
    ''' User control to house a QueueTimeDisplay control and mulitple instances
    ''' of the SkillQueueBlock control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SkillQueueControl

        Dim _cPilotName As String = ""
        Dim _currentSkill As Integer = 0
        Dim _currentLevel As Integer = 0

        ''' <summary>
        ''' Gets or sets the name of the pilot that is being used in the skill queue
        ''' </summary>
        ''' <value></value>
        ''' <returns>The name of the pilot represented in the skill queue</returns>
        ''' <remarks></remarks>
        Public Property PilotName() As String
            Get
                Return _cPilotName
            End Get
            Set(ByVal value As String)
                _cPilotName = value
                If CheckUpdateRequired() = False Then
                    Call RedrawSkillQueue()
                End If
            End Set
        End Property

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Call RedrawSkillQueue()

        End Sub

        Private Sub RedrawSkillQueue()
            If _cPilotName = "" Then
                Exit Sub
            Else
                If HQ.Settings.Pilots.ContainsKey(_cPilotName) = False Then
                    Exit Sub
                Else
                    panelSkillQueue.Controls.Clear()
                    Dim cPilot As EveHQPilot = HQ.Settings.Pilots(_cPilotName)
                    Dim newSqt As New SkillQueueTimeControl(cPilot.Name)
                    panelSkillQueue.Controls.Add(newSqt)
                    newSqt.Dock = DockStyle.Top
                    newSqt.BringToFront()
                    For Each queuedSkill As EveHQPilotQueuedSkill In cPilot.QueuedSkills.Values
                        If SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) >= Now Then
                            Dim newSqb As New SkillQueueBlock(cPilot.Name, queuedSkill)
                            panelSkillQueue.Controls.Add(newSqb)
                            newSqb.Dock = DockStyle.Top
                            newSqb.BringToFront()
                        End If
                    Next
                End If
            End If
            Refresh()
        End Sub

        Private Sub tmrUpdate_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrUpdate.Tick
            Call CheckUpdateRequired()
        End Sub

        Private Function CheckUpdateRequired() As Boolean
            If _cPilotName <> "" Then
                If HQ.Settings.Pilots.ContainsKey(_cPilotName) = True Then
                    Dim cPilot As EveHQPilot = HQ.Settings.Pilots(_cPilotName)
                    For Each queuedSkillNo As Integer In cPilot.QueuedSkills.Keys
                        Dim queuedSkill As EveHQPilotQueuedSkill = cPilot.QueuedSkills(queuedSkillNo)
                        If SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) >= Now Then
                            ' This should be our first valid skill
                            If Not (queuedSkill.SkillID = _currentSkill And queuedSkill.Level = _currentLevel) Then
                                _currentSkill = queuedSkill.SkillID
                                _currentLevel = queuedSkill.Level
                                Call RedrawSkillQueue()
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    Next
                End If
            End If
        End Function
    End Class
End NameSpace