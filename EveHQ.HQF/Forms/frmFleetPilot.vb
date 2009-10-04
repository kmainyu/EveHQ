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
Imports System.Windows.Forms


Public Class frmFleetPilot

    Dim cPilotNames As New ArrayList
    Public Property PilotNames() As ArrayList
        Get
            Return cPilotNames
        End Get
        Set(ByVal value As ArrayList)
            cPilotNames = value
            If cPilotNames.Count > 0 Then
                If cPilotNames.Count = 1 And HQF.HQFPilotCollection.HQFPilots.ContainsKey(cPilotNames(0)) = True Then
                    cboPilot.SelectedItem = cPilotNames(0)
                    cboPilot.Enabled = False
                Else
                    cboPilot.Text = "<Mulitple Pilots>"
                    cboPilot.Enabled = False
                End If
            End If
        End Set
    End Property

    Dim cFleetName As String = ""
    Public Property FleetName() As String
        Get
            Return cFleetName
        End Get
        Set(ByVal value As String)
            cFleetName = value
            Call Me.LoadPilotData()
        End Set
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call Me.LoadFittingData()

    End Sub

    Private Sub LoadFittingData()
        ' Load up fitting information
        cboFitting.BeginUpdate()
        cboFitting.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboFitting.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboFitting.Items.Clear()
        For Each fitting As String In Fittings.FittingList.Keys
            cboFitting.Items.Add(fitting)
            cboFitting.AutoCompleteCustomSource.Add(fitting)
        Next
        cboFitting.EndUpdate()
    End Sub

    Private Sub LoadPilotData()
        ' Load up pilot information
        cboPilot.BeginUpdate()
        cboPilot.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboPilot.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboPilot.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilot.Items.Add(cPilot.Name)
                cboPilot.AutoCompleteCustomSource.Add(cPilot.Name)
            End If
        Next
        cboPilot.EndUpdate()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check for fleet!
        If cFleetName = "" Then
            MessageBox.Show("A Fleet hasn't been set in the Fleet Manager. You will need to set a Fleet up before adding members to it.", "Fleet Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Check complete data
        If cboFitting.SelectedItem Is Nothing Then
            MessageBox.Show("You must select a fitting before proceeding.", "Fitting Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Check for a current existing pilot (only if we are adding)
        If cPilotNames.Count = 0 Then
            If cboPilot.SelectedItem Is Nothing Then
                MessageBox.Show("You must select a pilot before proceeding.", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            If FleetManager.FleetCollection(FleetName).FleetSetups.ContainsKey(cboPilot.SelectedItem.ToString) = True Then
                MessageBox.Show("This pilot is already setup as a fleet member", "Duplicate Pilot Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                ' Add the pilot and fitting to the setups
                FleetManager.FleetCollection(FleetName).FleetSetups.Add(cboPilot.SelectedItem.ToString, cboFitting.SelectedItem.ToString)
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Else
            ' We should change the pilot fitting
            For Each pilotName As String In cPilotNames
                FleetManager.FleetCollection(FleetName).FleetSetups(pilotName) = cboFitting.SelectedItem.ToString
            Next
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class