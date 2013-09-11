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
Public Class frmSelectQueuePilot

    Dim cDisplayPilotName As String
    Dim displayPilot As New EveHQ.Core.EveHQPilot
    Public Property DisplayPilotName() As String
        Get
            Return cDisplayPilotName
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
            displayPilot = EveHQ.Core.HQ.Settings.Pilots(value)
        End Set
    End Property

    Private Sub frmSelectQueuePilot_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Populate the combo box
        cboPilots.Items.Clear()
        For Each nPilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
            If nPilot.Active = True Then
                If nPilot.Name <> displayPilot.Name Then
                    cboPilots.Items.Add(nPilot.Name)
                End If
            End If
        Next
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        If cboPilots.SelectedItem IsNot Nothing Then
            If cboPilots.SelectedItem.ToString = "" Then
                MessageBox.Show("Please select a pilot to continue.", "Copy Queue to Pilot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Else
            MessageBox.Show("Please select a pilot to continue.", "Copy Queue to Pilot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim oldQueue As EveHQ.Core.EveHQSkillQueue = displayPilot.TrainingQueues(cboPilots.Tag.ToString)
        Dim newPilot As EveHQ.Core.EveHQPilot = EveHQ.Core.HQ.Settings.Pilots(cboPilots.SelectedItem.ToString)
        Dim newQueue As New EveHQ.Core.EveHQSkillQueue
        Dim reply As Integer = 0
        If newPilot.TrainingQueues.ContainsKey(cboPilots.Tag.ToString) Then
            reply = MessageBox.Show("Queue name '" & cboPilots.Tag.ToString & "' already exists for this pilot!" & ControlChars.CrLf & "Would you like to replace this Queue?", "Overwrite Existing Queue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.No Then
                Me.Close()
                Exit Sub
            Else
                newQueue = newPilot.TrainingQueues(cboPilots.Tag.ToString)
            End If
        End If
        newQueue.Name = cboPilots.Tag.ToString
        newQueue.IncCurrentTraining = CBool(oldQueue.IncCurrentTraining)

        If newPilot.TrainingQueues.Count <> 0 Then
            If newPilot.TrainingQueues.ContainsKey(newQueue.Name) = True Then
                If newPilot.PrimaryQueue = newQueue.Name Then
                    newQueue.Primary = True
                    newPilot.PrimaryQueue = newQueue.Name
                Else
                    newQueue.Primary = False
                End If
            Else
                newQueue.Primary = False
            End If
        Else
            newQueue.Primary = True
            newPilot.PrimaryQueue = newQueue.Name
        End If
        
        Dim newQ As New Dictionary(Of String, Core.EveHQSkillQueueItem)
        For Each qItem As EveHQ.Core.EveHQSkillQueueItem In oldQueue.Queue.Values
            Dim nItem As New EveHQ.Core.EveHQSkillQueueItem
            nItem.ToLevel = qItem.ToLevel
            nItem.FromLevel = qItem.FromLevel
            nItem.Name = qItem.Name
            nItem.Pos = qItem.Pos
            newQ.Add(nItem.Key, nItem)
        Next
        newQueue.Queue = newQ
        ' Add the new queue
        If reply <> Windows.Forms.DialogResult.Yes Then
            newPilot.TrainingQueues.Add(newQueue.Name, newQueue)
        End If
        Me.Close()
    End Sub
End Class