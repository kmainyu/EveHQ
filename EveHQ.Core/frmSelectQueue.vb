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