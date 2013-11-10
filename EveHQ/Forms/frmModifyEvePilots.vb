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

Namespace Forms
    Public Class FrmModifyEvePilots

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Check if the input is valid i.e. not blank
            If txtPilotName.Text = "" Then
                Dim reply As Integer = MessageBox.Show("Pilot name cannot be blank! Would you like to try another Pilot name?", "Error Creating Pilot", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = DialogResult.Retry Then
                    Exit Sub
                Else
                    Close()
                    Exit Sub
                End If
            End If
            ' Check the pilot ID
            If IsNumeric(txtPilotID.Text) = False Then
                Dim reply As Integer = MessageBox.Show("Pilot ID must be numeric! Would you like to try again?", "Error Creating Pilot", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = DialogResult.Retry Then
                    Exit Sub
                Else
                    Close()
                    Exit Sub
                End If
            End If
            ' Add the pilot to the pilot collection
            ' First check if the pilot already exists
            If HQ.Settings.Pilots.ContainsKey(txtPilotName.Text) Then
                Dim reply As Integer = MessageBox.Show("Pilot name " & txtPilotName.Text & " already exists in EveHQ! Would you like to try another Pilot name?", "Error Creating Pilot", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = DialogResult.Retry Then
                    Exit Sub
                Else
                    Close()
                    Exit Sub
                End If
            End If
            Dim newPilot As New EveHQPilot
            newPilot.Name = txtPilotName.Text
            newPilot.ID = txtPilotID.Text
            newPilot.Account = ""
            newPilot.AccountPosition = "0"
            HQ.Settings.Pilots.Add(newPilot.Name, newPilot)
            Close()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub
    End Class
End NameSpace