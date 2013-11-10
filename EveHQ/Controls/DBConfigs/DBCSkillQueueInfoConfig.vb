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