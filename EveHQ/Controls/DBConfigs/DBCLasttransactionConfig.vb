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
    Public Class DBCLasttransactionConfig
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            ' Load the combo box with the pilot info
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()

            For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
                If pilot.Active = True And pilot.Account <> "" Then
                    cboPilots.Items.Add(pilot.Name)
                End If
            Next

            cboPilots.EndUpdate()

        End Sub

#Region "Properties"

        Dim _dbWidget As New DBCLastTransactions
        Public Property DBWidget() As DBCLastTransactions
            Get
                Return _dbWidget
            End Get
            Set(ByVal value As DBCLastTransactions)
                _dbWidget = value
                Call SetControlInfo()
            End Set
        End Property

#End Region

        Private Sub SetControlInfo()
            If cboPilots.Items.Contains(_dbWidget.DBCDefaultPilotName) = True Then
                cboPilots.SelectedItem = _dbWidget.DBCDefaultPilotName
            Else
                If cboPilots.Items.Count > 0 Then
                    cboPilots.SelectedIndex = 0
                End If
            End If
            spinDefaultTransactions.Value = CInt(_dbWidget.DBCDefaultTransactionsCount)

        End Sub

        Private Sub btnCancelForm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub btnAcceptForm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Update the control properties
            If cboPilots.SelectedItem IsNot Nothing Then
                _dbWidget.DBCDefaultPilotName = cboPilots.SelectedItem.ToString
            Else
                MessageBox.Show("You must select a valid Pilot before adding this widget.", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            _dbWidget.DBCDefaultTransactionsCount = CInt(spinDefaultTransactions.Value)
            ' Now close the form
            DialogResult = DialogResult.OK
            Close()
        End Sub
    End Class
End NameSpace