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


Public Class frmFleetName

    Dim cFleetType As Integer = 1
    Dim cFleetName As String = ""
    Dim cWingName As String = ""
    Dim cSquadName As String = ""
    Public Property FleetType() As Integer
        Get
            Return cFleetType
        End Get
        Set(ByVal value As Integer)
            cFleetType = value
            Select Case cFleetType
                Case 1 ' Fleet
                    Me.Text = "Create New Fleet"
                    Me.lblDescription.Text = "Please select a name for the new Fleet..."
                Case 2 ' Wing
                    Me.Text = "Create New Wing"
                    Me.lblDescription.Text = "Please select a name for the new Wing..."
                Case 3 ' Squad
                    Me.Text = "Create New Squad"
                    Me.lblDescription.Text = "Please select a name for the new Squad..."
            End Select
        End Set
    End Property
    Public Property FleetName() As String
        Get
            Return cFleetName
        End Get
        Set(ByVal value As String)
            cFleetName = value
        End Set
    End Property
    Public Property WingName() As String
        Get
            Return cWingName
        End Get
        Set(ByVal value As String)
            cWingName = value
        End Set
    End Property
    Public Property SquadName() As String
        Get
            Return cSquadName
        End Get
        Set(ByVal value As String)
            cSquadName = value
        End Set
    End Property

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        Dim sFleetName As String = txtFleetName.Text.Trim

        ' Check not blank
        If sFleetName = "" Then
            MessageBox.Show("Please enter a valid name before continuing.", "Name Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Check doesn't already exist
        Select Case cFleetType
            Case 1 ' Fleet
                If FleetManager.FleetCollection.ContainsKey(sFleetName) = True Then
                    MessageBox.Show("Fleet Name already exists, please enter a unique name before continuing.", "Duplicate Fleet Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                cFleetName = sFleetName
            Case 2 ' Wing
                If FleetManager.FleetCollection(cFleetName).Wings.ContainsKey(sFleetName) = True Then
                    MessageBox.Show("Wing Name already exists, please enter a unique name before continuing.", "Duplicate Wing Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                cWingName = sFleetName
            Case 3 ' Squad
                If FleetManager.FleetCollection(cFleetName).Wings(cWingName).Squads.ContainsKey(sFleetName) = True Then
                    MessageBox.Show("Squad Name already exists, please enter a unique name before continuing.", "Duplicate Squad Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                cSquadName = sFleetName
        End Select

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class