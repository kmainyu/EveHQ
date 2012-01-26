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
Public Class DBCEveSkillQueue
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Initialise configuration form name
        Me.ControlConfigForm = "EveHQ.DBCEveSkillQueueInfoConfig"

        ' Load the combo box with the pilot info
        cboPilot.BeginUpdate()
        cboPilot.Items.Clear()
        For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If pilot.Active = True Then
                cboPilot.Items.Add(pilot.Name)
            End If
        Next
        cboPilot.EndUpdate()

    End Sub

#Region "Public Overriding Propeties"

    Public Overrides ReadOnly Property ControlName() As String
        Get
            Return "Eve Skill Queue Information"
        End Get
    End Property

#End Region

#Region "Custom Control Variables"
    Dim cDefaultPilotName As String = ""
#End Region

#Region "Custom Control Properties"
    Public Property DefaultPilotName() As String
        Get
            Return cDefaultPilotName
        End Get
        Set(ByVal value As String)
            cDefaultPilotName = value
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(DefaultPilotName) Then
                cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(DefaultPilotName), Core.Pilot)
            End If
            If cboPilot.Items.Contains(DefaultPilotName) = True Then cboPilot.SelectedItem = DefaultPilotName
            If ReadConfig = False Then
                Me.SetConfig("DefaultPilotName", value)
                Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DefaultPilotName)
            End If
        End Set
    End Property

#End Region

#Region "Class Variables"
    Dim cPilot As EveHQ.Core.Pilot
#End Region

#Region "Private Methods"
    Private Sub cboPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilot.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilot.SelectedItem.ToString) Then
            cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilot.SelectedItem.ToString), Core.Pilot)
            Call Me.UpdatePilotInfo()
            ' Start the skill timer
            tmrSkill.Enabled = True
            tmrSkill.Start()
        Else
            tmrSkill.Stop()
            tmrSkill.Enabled = False
        End If
    End Sub

    Private Sub UpdatePilotInfo()
        sqcEveQueue.PilotName = cboPilot.SelectedItem.ToString
    End Sub

    Private Sub lblPilot_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblPilot.LinkClicked
        frmPilot.DisplayPilotName = cPilot.Name
        frmEveHQ.OpenPilotInfoForm()
    End Sub

#End Region
End Class
