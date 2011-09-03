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
Public Class frmModifyQueues

    Dim cDisplayPilotName As String
    Dim displayPilot As New EveHQ.Core.Pilot
    Public Property DisplayPilotName() As String
        Get
            Return cDisplayPilotName
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
            DisplayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(value), Core.Pilot)
        End Set
    End Property

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check if the input is valid i.e. not blank
        If txtQueueName.Text = "" Then
            Dim reply As Integer = MessageBox.Show("Queue name cannot be blank! Would you like to try again?", "Error Creating Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Retry Then
                Exit Sub
            Else
                Me.Close()
                Exit Sub
            End If
        End If
        ' Decide which course of action to take depending on whether adding or editing an account
        Select Case Me.Tag.ToString
            Case "Add"
                ' Add the account to the accounts collection
                ' First check if the account already exists
                If displayPilot.TrainingQueues.Contains(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Creating Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        Me.Close()
                        Exit Sub
                    End If
                End If
                Dim newQueue As New EveHQ.Core.SkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = True
                If displayPilot.TrainingQueues.Count = 0 Then
                    newQueue.Primary = True
                Else
                    newQueue.Primary = False
                End If
                newQueue.Queue = New Collection
                displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Edit"
                If displayPilot.TrainingQueues.Contains(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Editing Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        Me.Close()
                        Exit Sub
                    End If
                End If
                ' Fetch the account from the collection
                Dim oldQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(txtQueueName.Tag), EveHQ.Core.SkillQueue)
                Dim newQueue As New EveHQ.Core.SkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = oldQueue.IncCurrentTraining
                newQueue.Primary = oldQueue.Primary
                newQueue.Queue = oldQueue.Queue
                ' Remove the old queue
                displayPilot.TrainingQueues.Remove(txtQueueName.Tag)
                ' Add the new queue
                displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Copy"
                If displayPilot.TrainingQueues.Contains(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Copying Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        Me.Close()
                        Exit Sub
                    End If
                End If
                Dim oldQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(txtQueueName.Tag), EveHQ.Core.SkillQueue)
                Dim newQueue As New EveHQ.Core.SkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = oldQueue.IncCurrentTraining
                newQueue.Primary = False
                Dim newQ As New Collection
                For Each qItem As EveHQ.Core.SkillQueueItem In oldQueue.Queue
                    Dim nItem As New EveHQ.Core.SkillQueueItem
                    nItem.ToLevel = qItem.ToLevel
                    nItem.FromLevel = qItem.FromLevel
                    nItem.Name = qItem.Name
                    nItem.Key = nItem.Name & nItem.FromLevel & nItem.ToLevel
					nItem.Pos = qItem.Pos
					nItem.Priority = qItem.Priority
					nItem.Notes = qItem.Notes
                    newQ.Add(nItem, nItem.Key)
                Next
                newQueue.Queue = newQ
                ' Add the new queue
                displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Merge"
                If displayPilot.TrainingQueues.Contains(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Copying Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        Me.Close()
                        Exit Sub
                    End If
                End If
                Dim mergeQueues As ListView.SelectedListViewItemCollection = CType(txtQueueName.Tag, ListView.SelectedListViewItemCollection)
                Dim newQueue As New EveHQ.Core.SkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = True
                newQueue.Primary = False
                newQueue.Queue = New Collection
                For Each item As ListViewItem In mergeQueues
                    Dim queueName As String = item.Name
                    Dim oldQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(queueName), EveHQ.Core.SkillQueue)
                    If oldQueue.Primary = True Then newQueue.Primary = True
                    For Each queueItem As EveHQ.Core.SkillQueueItem In oldQueue.Queue
                        Dim keyName As String = queueItem.Name & queueItem.FromLevel & queueItem.ToLevel
                        If newQueue.Queue.Contains(keyName) = False Then
                            newQueue.Queue.Add(queueItem, keyName)
                        End If
                    Next
                Next
				displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
			Case "Split"
				If displayPilot.TrainingQueues.Contains(txtQueueName.Text) Then
					Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Copying Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
					If reply = Windows.Forms.DialogResult.Retry Then
						Exit Sub
					Else
						Me.Close()
						Exit Sub
					End If
				End If
				Dim newQueue As New EveHQ.Core.SkillQueue
				newQueue.Name = txtQueueName.Text
				newQueue.IncCurrentTraining = True
				newQueue.Primary = False
				newQueue.Queue = CType(txtQueueName.Tag, Collection)
				displayPilot.TrainingQueues.Add(newQueue.Name, newQueue)
		End Select
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class