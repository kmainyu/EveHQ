' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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

Public Class FrmSelectQueue

    ReadOnly _skillsNeeded As New Dictionary(Of String, Integer)
    ReadOnly _displayPilot As New EveHQPilot
    ReadOnly _queueReason As String = ""

    Public Sub New(ByVal pilotName As String, ByVal queuedSkills As Dictionary(Of String, Integer), ByVal reason As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the queue reason
        _queueReason = reason

        ' Setup the pilot for this form
        _displayPilot = HQ.Settings.Pilots(pilotName)
        _skillsNeeded = queuedSkills
        Text = "Add to Skill Queue - " & pilotName
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
        Dim qName As String
        Dim qQueue As EveHQSkillQueue

        If radNewQueue.Checked = True Then
            If txtQueueName.Text = "" Then
                MessageBox.Show("A valid Skill Queue must be selected for this pilot!", "Error Creating Queue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            If _displayPilot.TrainingQueues.ContainsKey(txtQueueName.Text) Then
                Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Creating Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = DialogResult.Retry Then
                    Exit Sub
                Else
                    Close()
                    Exit Sub
                End If
            End If
            qQueue = New EveHQSkillQueue
            qQueue.Name = txtQueueName.Text
            qQueue.IncCurrentTraining = True
            qQueue.Primary = False
            qQueue.Queue = New Dictionary(Of String, EveHQSkillQueueItem)
            _displayPilot.TrainingQueues.Add(qQueue.Name, qQueue)
        Else
            If cboQueueName.SelectedItem Is Nothing Then
                MessageBox.Show("A valid Skill Queue must be selected for this pilot!", "Error Creating Queue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            qName = cboQueueName.SelectedItem.ToString
            qQueue = _displayPilot.TrainingQueues(qName)
        End If
        If _displayPilot.Name <> "" Then
            If _skillsNeeded.Count <> 0 Then
                For Each skillName As String In _skillsNeeded.Keys
                    qQueue = SkillQueueFunctions.AddSkillToQueue(_displayPilot, skillName, qQueue.Queue.Count + 1, qQueue, _skillsNeeded(skillName), False, True, _queueReason)
                Next
            Else
                MessageBox.Show(_displayPilot.Name & " has already trained all necessary skills to use this item.", "Already Trained!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("There is no pilot selected to add the skills to.", "Cannot Add Skills to Training Queue", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub frmModifyQueues_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Load up existing queues
        cboQueueName.Items.Clear()
        For Each qName As String In _displayPilot.TrainingQueues.Keys
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

    Private Sub radExistingQueue_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radExistingQueue.CheckedChanged
        If radExistingQueue.Checked = True Then
            cboQueueName.Visible = True
            txtQueueName.Visible = False
            cboQueueName.Select()
        End If
    End Sub

    Private Sub radNewQueue_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radNewQueue.CheckedChanged
        If radNewQueue.Checked = True Then
            cboQueueName.Visible = False
            txtQueueName.Visible = True
            txtQueueName.Select()
        End If
    End Sub

End Class