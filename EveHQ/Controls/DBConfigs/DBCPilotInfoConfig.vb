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
Public Class DBCPilotInfoConfig

#Region "Properties"

    Dim cDBWidget As New DBCPilotInfo
    Public Property DBWidget() As DBCPilotInfo
        Get
            Return cDBWidget
        End Get
        Set(ByVal value As DBCPilotInfo)
            cDBWidget = value
            Call SetControlInfo()
        End Set
    End Property

#End Region

    Private Sub SetControlInfo()
        If cboPilots.Items.Contains(cDBWidget.DefaultPilotName) = True Then
            cboPilots.SelectedItem = cDBWidget.DefaultPilotName
        Else
            If cboPilots.Items.Count > 0 Then
                cboPilots.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Just close the form and do nothing
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Update the control properties
        If cboPilots.SelectedItem Is Nothing Then
            MessageBox.Show("You must select a valid pilot before adding this widget.", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If cboPilots.SelectedItem IsNot Nothing Then
            cDBWidget.DefaultPilotName = cboPilots.SelectedItem.ToString
        End If
        ' Now close the form
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class