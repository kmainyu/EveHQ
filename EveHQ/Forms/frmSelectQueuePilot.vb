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

Namespace Forms
    Public Class FrmSelectQueuePilot

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

        Private Sub frmSelectQueuePilot_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ' Populate the combo box
            cboPilots.Items.Clear()
            For Each nPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If nPilot.Active = True Then
                    If nPilot.Name <> _displayPilot.Name Then
                        cboPilots.Items.Add(nPilot.Name)
                    End If
                End If
            Next
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            If cboPilots.SelectedItem IsNot Nothing Then
                If cboPilots.SelectedItem.ToString = "" Then
                    MessageBox.Show("Please select a pilot to continue.", "Copy Queue to Pilot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            Else
                MessageBox.Show("Please select a pilot to continue.", "Copy Queue to Pilot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            Dim oldQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(cboPilots.Tag.ToString)
            Dim newPilot As EveHQPilot = HQ.Settings.Pilots(cboPilots.SelectedItem.ToString)
            Dim newQueue As New EveHQSkillQueue
            Dim reply As Integer = 0
            If newPilot.TrainingQueues.ContainsKey(cboPilots.Tag.ToString) Then
                reply = MessageBox.Show("Queue name '" & cboPilots.Tag.ToString & "' already exists for this pilot!" & ControlChars.CrLf & "Would you like to replace this Queue?", "Overwrite Existing Queue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.No Then
                    Close()
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

            Dim newQ As New Dictionary(Of String, EveHQSkillQueueItem)
            For Each qItem As EveHQSkillQueueItem In oldQueue.Queue.Values
                Dim nItem As New EveHQSkillQueueItem
                nItem.ToLevel = qItem.ToLevel
                nItem.FromLevel = qItem.FromLevel
                nItem.Name = qItem.Name
                nItem.Pos = qItem.Pos
                newQ.Add(nItem.Key, nItem)
            Next
            newQueue.Queue = newQ
            ' Add the new queue
            If reply <> DialogResult.Yes Then
                newPilot.TrainingQueues.Add(newQueue.Name, newQueue)
            End If
            Close()
        End Sub
    End Class
End NameSpace