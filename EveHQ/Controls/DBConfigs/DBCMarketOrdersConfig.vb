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
Imports EveHQ.Controls.DBControls

Namespace Controls.DBConfigs
    Public Class DBCMarketOrdersConfig

#Region "Properties"

        Dim _dbWidget As New DBCMarketOrders
        Public Property DBWidget() As DBCMarketOrders
            Get
                Return _dbWidget
            End Get
            Set(ByVal value As DBCMarketOrders)
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
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ' Just close the form and do nothing
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Update the control properties
            If cboPilots.SelectedItem Is Nothing Then
                MessageBox.Show("You must select a valid pilot before adding this widget.", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            If cboPilots.SelectedItem IsNot Nothing Then
                _dbWidget.DefaultPilotName = cboPilots.SelectedItem.ToString
            End If
            ' Now close the form
            DialogResult = DialogResult.OK
            Close()
        End Sub
    End Class
End NameSpace