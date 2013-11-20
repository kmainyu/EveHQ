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
Namespace Forms

    Public Class FrmSelectLocation

        Private _location As String
        Private _bpLocations As List(Of String)
        Private _includeBPOs As Boolean = False

        Public ReadOnly Property BPLocation() As String
            Get
                Return _location
            End Get
        End Property

        Public Property BPLocations() As List(Of String)
            Get
                Return _bpLocations
            End Get
            Set(ByVal value As List(Of String))
                _bpLocations = value
            End Set
        End Property

        Public ReadOnly Property IncludeBPOs() As Boolean
            Get
                Return _includeBPOs
            End Get
        End Property

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            If cboLocations.SelectedItem IsNot Nothing Then
                _location = cboLocations.SelectedItem.ToString
            End If
            Close()
        End Sub

        Private Sub chkIncludeBPOs_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkIncludeBPOs.CheckedChanged
            _includeBPOs = chkIncludeBPOs.Checked
        End Sub

        Private Sub frmSelectLocation_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ' List all locations of blueprints
            cboLocations.Items.Clear()
            BPLocations.Sort()
            cboLocations.BeginUpdate()
            For Each bpLoc As String In BPLocations
                cboLocations.Items.Add(bpLoc)
            Next
            cboLocations.EndUpdate()
        End Sub

    End Class
End NameSpace