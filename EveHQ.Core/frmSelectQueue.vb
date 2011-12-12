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
Imports System.Windows.Forms

Public Class frmSelectQueue

    Dim skillsNeeded As New List(Of String)
    Dim displayPilot As New EveHQ.Core.Pilot

    Public Sub New(ByVal PilotName As String, ByVal QueuedSkills As List(Of String))

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Setup the pilot for this form
        displayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(PilotName), Core.Pilot)
        skillsNeeded = QueuedSkills
        Me.Text = "Add to Skill Queue - " & PilotName
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        Dim qName As String = ""
        Dim qQueue As New EveHQ.Core.SkillQueue

        If radNewQueue.Checked = True Then
            If txtQueueName.Text = "" Then
                MessageBox.Show("A valid Skill Queue must be selected for this pilot!", "Error Creating Queue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            qName = txtQueueName.Text
            If displayPilot.TrainingQueues.Contains(txtQueueName.Text) Then
                Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Creating Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = Windows.Forms.DialogResult.Retry Then
                    Exit Sub
                Else
                    Me.Close()
                    Exit Sub
                End If
            End If
            qQueue = New EveHQ.Core.SkillQueue
            qQueue.Name = txtQueueName.Text
            qQueue.IncCurrentTraining = True
            qQueue.Primary = False
            qQueue.Queue = New Collection
            displayPilot.TrainingQueues.Add(qQueue.Name, qQueue)
        Else
            If cboQueueName.SelectedItem Is Nothing Then
                MessageBox.Show("A valid Skill Queue must be selected for this pilot!", "Error Creating Queue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            qName = cboQueueName.SelectedItem.ToString
            qQueue = CType(displayPilot.TrainingQueues(qName), Core.SkillQueue)
        End If
        If displayPilot.Name <> "" Then
            If skillsNeeded.Count <> 0 Then
                For Each skill As String In skillsNeeded
                    Dim skillName As String = skill.Substring(0, skill.Length - 1)
                    Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                    qQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, qQueue.Queue.Count + 1, qQueue, skillLvl, False, True)
                Next
            Else
                MessageBox.Show(displayPilot.Name & " has already trained all necessary skills to use this item.", "Already Trained!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("There is no pilot selected to add the skills to.", "Cannot Add Skills to Training Queue", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmModifyQueues_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Load up existing queues
        cboQueueName.Items.Clear()
        For Each qName As String In displayPilot.TrainingQueues.GetKeyList
            cboQueueName.Items.Add(qName)
        Next

        ' If no items, then disable the existing queue option
        If cboQueueName.Items.Count = 0 Then
            radNewQueue.Checked = True
            radExistingQueue.Enabled = False
            cboQueueName.Visible = False
            txtQueueName.Visible = True
        Else
            radExistingQueue.Checked = True
            radExistingQueue.Enabled = True
            radNewQueue.Enabled = True
            cboQueueName.Visible = True
            txtQueueName.Visible = False
            cboQueueName.BringToFront()
        End If
    End Sub

    Private Sub radExistingQueue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radExistingQueue.CheckedChanged
        If radExistingQueue.Checked = True Then
            cboQueueName.Visible = True
            txtQueueName.Visible = False
            cboQueueName.Select()
        End If
    End Sub

    Private Sub radNewQueue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radNewQueue.CheckedChanged
        If radNewQueue.Checked = True Then
            cboQueueName.Visible = False
            txtQueueName.Visible = True
            txtQueueName.Select()
        End If
    End Sub

End Class