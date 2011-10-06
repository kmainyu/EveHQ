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
Public Class DBCPilotInfo
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Initialise configuration form name
        Me.ControlConfigForm = "EveHQ.DBCPilotInfoConfig"

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
            Return "Pilot Information"
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
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilot.SelectedItem.ToString) Then
            ' Update the info
            pbPilot.Image = EveHQ.Core.ImageHandler.GetPortraitImage(cPilot)
            lblCorp.Text = "Member of " & cPilot.Corp
            lblIsk.Text = "Balance: " & cPilot.Isk.ToString("N2")
            Call Me.UpdateTrainingInfo()
        Else
            ' Clear the info
            lblCorp.Text = ""
            lblIsk.Text = ""
            lblSP.Text = ""
            lblTraining.Text = ""
            lblTrainingEnd.Text = ""
            lblTrainingTime.Text = ""
            lblSkillQueueEnd.Text = ""
            lblSkillQueueTime.Text = ""
            pbPilot.Image = My.Resources.noitem
        End If
    End Sub

    Private Sub UpdateTrainingInfo()
        If cPilot IsNot Nothing Then
            If cPilot.Training = True Then
                ' Training
                lblSP.Text = "Skillpoints: " & (cPilot.SkillPoints + cPilot.TrainingCurrentSP).ToString("N0")
                Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cPilot.TrainingEndTime)
                lblTraining.Text = "Training: " & cPilot.TrainingSkillName & " " & EveHQ.Core.SkillFunctions.Roman(cPilot.TrainingSkillLevel)
                lblTrainingEnd.Text = "Training Ends: " & Format(localdate, "ddd") & " " & localdate
                lblTrainingTime.Text = "Time Remaining: " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.TrainingCurrentTime)
            Else
                ' Not training
                lblSP.Text = "Skillpoints: " & cPilot.SkillPoints.ToString("N0")
                lblTraining.Text = "Not currently training"
                lblTrainingEnd.Text = "Training Ends: n/a"
                lblTrainingTime.Text = "Time Remaining: n/a"
            End If
            If cPilot.QueuedSkills IsNot Nothing Then
                If cPilot.QueuedSkills.Count > 0 Then
                    If cPilot.QueuedSkills.Count = 1 Then
                        Dim skillDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cPilot.TrainingEndTime)
                        lblSkillQueueEnd.Text = "Skill Queue Ends: " & Format(skillDate, "ddd") & " " & skillDate
                        lblSkillQueueTime.Text = "Queue Time Remaining: " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.TrainingCurrentTime)
                    Else
                        Dim lastSkill As EveHQ.Core.PilotQueuedSkill = cPilot.QueuedSkills(cPilot.QueuedSkills.Keys(cPilot.QueuedSkills.Count - 1))
                        Dim skillDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(lastSkill.EndTime)
                        lblSkillQueueEnd.Text = "Skill Queue Ends: " & Format(skillDate, "ddd") & " " & skillDate
                        lblSkillQueueTime.Text = "Queue Time Remaining: " & EveHQ.Core.SkillFunctions.TimeToString(CType(skillDate - Now, TimeSpan).TotalSeconds)
                    End If
                Else
                    lblSkillQueueEnd.Text = "Skill Queue Ends: n/a"
                    lblSkillQueueTime.Text = "Queue Time Remaining: n/a"
                End If
            Else
                lblSkillQueueEnd.Text = "Skill Queue Ends: n/a"
                lblSkillQueueTime.Text = "Queue Time Remaining: n/a"
            End If
        End If
    End Sub

    Private Sub tmrSkill_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSkill.Tick
        Call Me.UpdateTrainingInfo()
    End Sub

    Private Sub lblTraining_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblTraining.LinkClicked
        frmTraining.DisplayPilotName = cPilot.Name
        frmEveHQ.OpenSkillTrainingForm()
    End Sub

    Private Sub lblPilot_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblPilot.LinkClicked
        frmPilot.DisplayPilotName = cPilot.Name
        frmEveHQ.OpenPilotInfoForm()
    End Sub

#End Region
End Class
