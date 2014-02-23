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
Imports EveHQ.Controls.DBControls

Namespace Controls.DBConfigs
    Public Class DBCSkillQueueInfoConfig

#Region "Properties"

        Dim _dbWidget As New DBCSkillQueueInfo
        Public Property DBWidget() As DBCSkillQueueInfo
            Get
                Return _dbWidget
            End Get
            Set(ByVal value As DBCSkillQueueInfo)
                _dbWidget = value
                Call SetControlInfo()
            End Set
        End Property

#End Region

        Private Sub SetControlInfo()
            If cboPilots.Items.Contains(_dbWidget.DefaultPilotName) = True Then
                cboPilots.SelectedItem = _dbWidget.DefaultPilotName
            Else
                If cboPilots.Items.Count > 0 Then
                    cboPilots.SelectedIndex = 0
                End If
            End If
            If _dbWidget.EveQueue = True Then
                radEve.Checked = True
            Else
                radEveHQ.Checked = True
            End If
            If DBWidget.DefaultQueueName <> "" Then
                If cboSkillQueue.Items.Contains(DBWidget.DefaultQueueName) = True Then
                    cboSkillQueue.SelectedItem = DBWidget.DefaultQueueName
                End If
            End If
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ' Just close the form and do nothing
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Update the control properties
            If cboPilots.SelectedItem IsNot Nothing Then
                _dbWidget.DefaultPilotName = cboPilots.SelectedItem.ToString
            Else
                MessageBox.Show("You must select a valid Pilot before adding this widget.", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            If cboSkillQueue.SelectedItem IsNot Nothing Then
                _dbWidget.DefaultQueueName = cboSkillQueue.SelectedItem.ToString
            Else
                _dbWidget.DefaultQueueName = ""
            End If
            _dbWidget.EveQueue = radEve.Checked
            ' Now close the form
            DialogResult = DialogResult.OK
            Close()
        End Sub

        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilots.SelectedIndexChanged
            ' Update the queue information
            Call UpdateQueueList()
        End Sub

        Private Sub UpdateQueueList()
            cboSkillQueue.BeginUpdate()
            cboSkillQueue.Items.Clear()
            If HQ.Settings.Pilots.ContainsKey(cboPilots.SelectedItem.ToString) = True Then
                Dim cPilot As EveHQPilot = HQ.Settings.Pilots(cboPilots.SelectedItem.ToString)
                For Each sq As EveHQSkillQueue In cPilot.TrainingQueues.Values
                    cboSkillQueue.Items.Add(sq.Name)
                Next
                cboSkillQueue.EndUpdate()
            End If
        End Sub

        Private Sub radEveHQ_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radEveHQ.CheckedChanged
            cboSkillQueue.Enabled = radEveHQ.Checked
        End Sub

        Private Sub radEve_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radEve.CheckedChanged
            cboSkillQueue.Enabled = Not (radEve.Checked)
        End Sub
    End Class
End NameSpace