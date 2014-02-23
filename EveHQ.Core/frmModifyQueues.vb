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

Public Class FrmModifyQueues

    Dim _displayPilotName As String
    Dim _displayPilot As New EveHQPilot
    Public Property DisplayPilotName() As String
        Get
            Return _displayPilotName
        End Get
        Set(ByVal value As String)
            _displayPilotName = value
            _displayPilot = HQ.Settings.Pilots(value)
        End Set
    End Property

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
        ' Check if the input is valid i.e. not blank
        If txtQueueName.Text = "" Then
            Dim reply As Integer = MessageBox.Show("Queue name cannot be blank! Would you like to try again?", "Error Creating Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
            If reply = DialogResult.Retry Then
                Exit Sub
            Else
                Close()
                Exit Sub
            End If
        End If
        ' Decide which course of action to take depending on whether adding or editing an account
        Select Case Tag.ToString
            Case "Add"
                ' Add the account to the accounts collection
                ' First check if the account already exists
                If _displayPilot.TrainingQueues.ContainsKey(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Creating Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = DialogResult.Retry Then
                        Exit Sub
                    Else
                        Close()
                        Exit Sub
                    End If
                End If
                Dim newQueue As New EveHQSkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = True
                If _displayPilot.TrainingQueues.Count = 0 Then
                    newQueue.Primary = True
                Else
                    newQueue.Primary = False
                End If
                newQueue.Queue = New Dictionary(Of String, EveHQSkillQueueItem)
                _displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Edit"
                If _displayPilot.TrainingQueues.ContainsKey(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Editing Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = DialogResult.Retry Then
                        Exit Sub
                    Else
                        Close()
                        Exit Sub
                    End If
                End If
                ' Fetch the account from the collection
                Dim oldQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(txtQueueName.Tag.ToString)
                Dim newQueue As New EveHQSkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = oldQueue.IncCurrentTraining
                newQueue.Primary = oldQueue.Primary
                newQueue.Queue = oldQueue.Queue
                ' Remove the old queue
                _displayPilot.TrainingQueues.Remove(txtQueueName.Tag.ToString)
                ' Add the new queue
                _displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Copy"
                If _displayPilot.TrainingQueues.ContainsKey(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Copying Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = DialogResult.Retry Then
                        Exit Sub
                    Else
                        Close()
                        Exit Sub
                    End If
                End If
                Dim oldQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(txtQueueName.Tag.ToString)
                Dim newQueue As New EveHQSkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = oldQueue.IncCurrentTraining
                newQueue.Primary = False
                Dim newQ As New Dictionary(Of String, EveHQSkillQueueItem)
                For Each qItem As EveHQSkillQueueItem In oldQueue.Queue.Values
                    Dim nItem As New EveHQSkillQueueItem
                    nItem.ToLevel = qItem.ToLevel
                    nItem.FromLevel = qItem.FromLevel
                    nItem.Name = qItem.Name
                    nItem.Pos = qItem.Pos
                    nItem.Priority = qItem.Priority
                    nItem.Notes = qItem.Notes
                    newQ.Add(nItem.Key, nItem)
                Next
                newQueue.Queue = newQ
                ' Add the new queue
                _displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Merge"
                If _displayPilot.TrainingQueues.ContainsKey(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Copying Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = DialogResult.Retry Then
                        Exit Sub
                    Else
                        Close()
                        Exit Sub
                    End If
                End If
                Dim mergeQueues As ListView.SelectedListViewItemCollection = CType(txtQueueName.Tag, ListView.SelectedListViewItemCollection)
                Dim newQueue As New EveHQSkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = True
                newQueue.Primary = False
                newQueue.Queue = New Dictionary(Of String, EveHQSkillQueueItem)
                For Each item As ListViewItem In mergeQueues
                    Dim queueName As String = item.Name
                    Dim oldQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(queueName)
                    If oldQueue.Primary = True Then newQueue.Primary = True
                    For Each queueItem As EveHQSkillQueueItem In oldQueue.Queue.Values
                        Dim keyName As String = queueItem.Name & queueItem.FromLevel & queueItem.ToLevel
                        If newQueue.Queue.ContainsKey(keyName) = False Then
                            newQueue.Queue.Add(keyName, queueItem)
                        End If
                    Next
                Next
                _displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Split"
                If _displayPilot.TrainingQueues.ContainsKey(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Copying Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = DialogResult.Retry Then
                        Exit Sub
                    Else
                        Close()
                        Exit Sub
                    End If
                End If
                Dim newQueue As New EveHQSkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = True
                newQueue.Primary = False
                newQueue.Queue = CType(txtQueueName.Tag, Dictionary(Of String, EveHQSkillQueueItem))
                _displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
        End Select
        Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

End Class