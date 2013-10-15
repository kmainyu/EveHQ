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