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

''' <summary>
''' User control to house a QueueTimeDisplay control and mulitple instances
''' of the SkillQueueBlock control.
''' </summary>
''' <remarks></remarks>
Public Class SkillQueueControl

    ''' <summary>
    ''' Gets or sets the name of the pilot that is being used in the skill queue
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the pilot represented in the skill queue</returns>
    ''' <remarks></remarks>
    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
            If Me.CheckUpdateRequired = False Then
                Call Me.RedrawSkillQueue()
            End If
        End Set
    End Property

    Dim cPilotName As String = ""
    Dim currentSkill As Integer = 0
    Dim currentLevel As Integer = 0

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call Me.RedrawSkillQueue()

    End Sub

    Private Sub RedrawSkillQueue()
        If cPilotName = "" Then
            Exit Sub
        Else
            If EveHQ.Core.HQ.EveHqSettings.Pilots.Contains(cPilotName) = False Then
                Exit Sub
            Else
                Me.panelSkillQueue.Controls.Clear()
                Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHqSettings.Pilots(cPilotName), Pilot)
                Dim newSQT As New EveHQ.Core.SkillQueueTimeControl(cPilot.Name)
                panelSkillQueue.Controls.Add(newSQT)
                newSQT.Dock = Windows.Forms.DockStyle.Top
                newSQT.BringToFront()
                For Each QueuedSkill As EveHQ.Core.PilotQueuedSkill In cPilot.QueuedSkills.Values
                    If EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.EndTime) >= Now Then
                        Dim newSQB As New EveHQ.Core.SkillQueueBlock(cPilot.Name, QueuedSkill)
                        panelSkillQueue.Controls.Add(newSQB)
                        newSQB.Dock = Windows.Forms.DockStyle.Top
                        newSQB.BringToFront()
                    End If
                Next
            End If
        End If
        Me.Refresh()
    End Sub

    Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        Call CheckUpdateRequired()
    End Sub

    Private Function CheckUpdateRequired() As Boolean
        If cPilotName <> "" Then
            If EveHQ.Core.HQ.EveHqSettings.Pilots.Contains(cPilotName) = True Then
                Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHqSettings.Pilots(cPilotName), Pilot)
                For Each QueuedSkillNo As Long In cPilot.QueuedSkills.Keys
                    Dim QueuedSkill As EveHQ.Core.PilotQueuedSkill = cPilot.QueuedSkills(QueuedSkillNo)
                    If EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.EndTime) >= Now Then
                        ' This should be our first valid skill
                        If Not (QueuedSkill.SkillID = currentSkill And QueuedSkill.Level = currentLevel) Then
                            currentSkill = QueuedSkill.SkillID
                            currentLevel = QueuedSkill.Level
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
