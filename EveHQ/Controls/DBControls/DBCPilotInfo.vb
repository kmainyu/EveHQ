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

Imports EveHQ.Core
Imports EveHQ.Common.Extensions

Namespace Controls.DBControls
    Public Class DBCPilotInfo
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            ' Initialise configuration form name
            ControlConfigForm = "EveHQ.Controls.DBConfigs.DBCPilotInfoConfig"

            ' Load the combo box with the pilot info
            cboPilot.BeginUpdate()
            cboPilot.Items.Clear()
            For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
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
        Dim _defaultPilotName As String = ""
#End Region

#Region "Custom Control Properties"
        Public Property DefaultPilotName() As String
            Get
                Return _defaultPilotName
            End Get
            Set(ByVal value As String)
                _defaultPilotName = value
                If HQ.Settings.Pilots.ContainsKey(DefaultPilotName) Then
                    _pilot = HQ.Settings.Pilots(DefaultPilotName)
                End If
                If cboPilot.Items.Contains(DefaultPilotName) = True Then cboPilot.SelectedItem = DefaultPilotName
                If ReadConfig = False Then
                    SetConfig("DefaultPilotName", value)
                    SetConfig("ControlConfigInfo", "Default Pilot: " & DefaultPilotName)
                End If
            End Set
        End Property

#End Region

#Region "Class Variables"
        Dim _pilot As EveHQPilot
#End Region

#Region "Private Methods"
        Private Sub cboPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilot.SelectedIndexChanged
            If HQ.Settings.Pilots.ContainsKey(cboPilot.SelectedItem.ToString) Then
                _pilot = HQ.Settings.Pilots(cboPilot.SelectedItem.ToString)
                Call UpdatePilotInfo()
                ' Start the skill timer
                tmrSkill.Enabled = True
                tmrSkill.Start()
            Else
                tmrSkill.Stop()
                tmrSkill.Enabled = False
            End If
        End Sub

        Private Sub UpdatePilotInfo()
            If HQ.Settings.Pilots.ContainsKey(cboPilot.SelectedItem.ToString) Then
                ' Update the info
                pbPilot.Image = ImageHandler.GetPortraitImage(_pilot.ID)
                lblCorp.Text = "Member of " & _pilot.Corp
                lblIsk.Text = "Balance: " & _pilot.Isk.ToInvariantString("N2")
                Call UpdateTrainingInfo()
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
            If _pilot IsNot Nothing Then
                If _pilot.Training = True Then
                    ' Training
                    lblSP.Text = "Skillpoints: " & (_pilot.SkillPoints + _pilot.TrainingCurrentSP).ToString("N0")
                    Dim localdate As Date = SkillFunctions.ConvertEveTimeToLocal(_pilot.TrainingEndTime)
                    lblTraining.Text = "Training: " & _pilot.TrainingSkillName & " " & SkillFunctions.Roman(_pilot.TrainingSkillLevel)
                    lblTrainingEnd.Text = "Training Ends: " & Format(localdate, "ddd") & " " & localdate
                    lblTrainingTime.Text = "Time Remaining: " & SkillFunctions.TimeToString(_pilot.TrainingCurrentTime)
                Else
                    ' Not training
                    lblSP.Text = "Skillpoints: " & _pilot.SkillPoints.ToString("N0")
                    lblTraining.Text = "Not currently training"
                    lblTrainingEnd.Text = "Training Ends: n/a"
                    lblTrainingTime.Text = "Time Remaining: n/a"
                End If
                If _pilot.QueuedSkills IsNot Nothing Then
                    If _pilot.QueuedSkills.Count > 0 Then
                        If _pilot.QueuedSkills.Count = 1 Then
                            Dim skillDate As Date = SkillFunctions.ConvertEveTimeToLocal(_pilot.TrainingEndTime)
                            lblSkillQueueEnd.Text = "Skill Queue Ends: " & Format(skillDate, "ddd") & " " & skillDate
                            lblSkillQueueTime.Text = "Queue Time Remaining: " & SkillFunctions.TimeToString(_pilot.TrainingCurrentTime)
                        Else
                            Dim lastSkill As EveHQPilotQueuedSkill = _pilot.QueuedSkills(_pilot.QueuedSkills.Keys(_pilot.QueuedSkills.Count - 1))
                            Dim skillDate As Date = SkillFunctions.ConvertEveTimeToLocal(lastSkill.EndTime)
                            lblSkillQueueEnd.Text = "Skill Queue Ends: " & Format(skillDate, "ddd") & " " & skillDate
                            lblSkillQueueTime.Text = "Queue Time Remaining: " & SkillFunctions.TimeToString((skillDate - Now).TotalSeconds)
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

        Private Sub tmrSkill_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrSkill.Tick
            Call UpdateTrainingInfo()
        End Sub

        Private Sub lblTraining_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblTraining.LinkClicked
            Forms.FrmTraining.DisplayPilotName = _pilot.Name
            Forms.FrmEveHQ.OpenSkillTrainingForm()
        End Sub

        Private Sub lblPilot_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblPilot.LinkClicked
            Forms.FrmPilot.DisplayPilotName = _pilot.Name
            Forms.FrmEveHQ.OpenPilotInfoForm()
        End Sub

#End Region
    End Class
End Namespace