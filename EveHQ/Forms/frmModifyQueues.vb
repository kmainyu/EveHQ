' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
                If EveHQ.Core.HQ.myPilot.TrainingQueues.Contains(txtQueueName.Text) Then
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
                If EveHQ.Core.HQ.myPilot.TrainingQueues.Count = 0 Then
                    newQueue.Primary = True
                Else
                    newQueue.Primary = False
                End If
                newQueue.Queue = New Collection
                EveHQ.Core.HQ.myPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Edit"
                If EveHQ.Core.HQ.myPilot.TrainingQueues.Contains(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Editing Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        Me.Close()
                        Exit Sub
                    End If
                End If
                ' Fetch the account from the collection
                Dim oldQueue As EveHQ.Core.SkillQueue = CType(EveHQ.Core.HQ.myPilot.TrainingQueues(txtQueueName.Tag), EveHQ.Core.SkillQueue)
                Dim newQueue As New EveHQ.Core.SkillQueue
                newQueue.Name = txtQueueName.Text
                newQueue.IncCurrentTraining = oldQueue.IncCurrentTraining
                newQueue.Primary = oldQueue.Primary
                newQueue.Queue = oldQueue.Queue
                ' Remove the old queue
                EveHQ.Core.HQ.myPilot.TrainingQueues.Remove(txtQueueName.Tag)
                ' Add the new queue
                EveHQ.Core.HQ.myPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Copy"
                If EveHQ.Core.HQ.myPilot.TrainingQueues.Contains(txtQueueName.Text) Then
                    Dim reply As Integer = MessageBox.Show("Queue name " & txtQueueName.Text & " already exists for this pilot!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Copying Queue", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        Me.Close()
                        Exit Sub
                    End If
                End If
                Dim oldQueue As EveHQ.Core.SkillQueue = CType(EveHQ.Core.HQ.myPilot.TrainingQueues(txtQueueName.Tag), EveHQ.Core.SkillQueue)
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
                    newQ.Add(nItem, nItem.Key)
                Next
                newQueue.Queue = newQ
                ' Add the new queue
                EveHQ.Core.HQ.myPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            Case "Merge"
                If EveHQ.Core.HQ.myPilot.TrainingQueues.Contains(txtQueueName.Text) Then
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
                    Dim oldQueue As EveHQ.Core.SkillQueue = CType(EveHQ.Core.HQ.myPilot.TrainingQueues(queueName), EveHQ.Core.SkillQueue)
                    If oldQueue.Primary = True Then newQueue.Primary = True
                    For Each queueItem As EveHQ.Core.SkillQueueItem In oldQueue.Queue
                        Dim keyName As String = queueItem.Name & queueItem.FromLevel & queueItem.ToLevel
                        If newQueue.Queue.Contains(keyName) = False Then
                            newQueue.Queue.Add(queueItem, keyName)
                        End If
                    Next
                Next
                EveHQ.Core.HQ.myPilot.TrainingQueues.Add(newQueue.Name, newQueue)
        End Select
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtQueueName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtQueueName.KeyPress
        Dim myMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(e.KeyChar, "^[0-9a-zA-Z\x20\x08]*$")
        If myMatch.Success = False Then
            e.KeyChar = CChar("")
        End If
    End Sub
End Class