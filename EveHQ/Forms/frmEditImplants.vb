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
    Public Class FrmEditImplants

        Dim _displayPilotName As String
        Dim _displayPilot As EveHQPilot

        Public Property DisplayPilotName() As String
            Get
                Return _displayPilotName
            End Get
            Set(ByVal value As String)
                _displayPilotName = value
                _displayPilot = HQ.Settings.Pilots(value)
            End Set
        End Property

        Private Sub nudC_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudC.ValueChanged
            _displayPilot.CImplantM = CInt(nudC.Value)
        End Sub

        Private Sub nudI_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudI.ValueChanged
            _displayPilot.IntImplantM = CInt(nudI.Value)
        End Sub

        Private Sub nudM_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudM.ValueChanged
            _displayPilot.MImplantM = CInt(nudM.Value)
        End Sub

        Private Sub nudP_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudP.ValueChanged
            _displayPilot.PImplantM = CInt(nudP.Value)
        End Sub

        Private Sub nudW_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudW.ValueChanged
            _displayPilot.WImplantM = CInt(nudW.Value)
        End Sub

        Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
            Close()
        End Sub

        Private Sub frmEditImplants_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            nudC.Value = _displayPilot.CImplantM
            nudI.Value = _displayPilot.IntImplantM
            nudM.Value = _displayPilot.MImplantM
            nudP.Value = _displayPilot.PImplantM
            nudW.Value = _displayPilot.WImplantM
        End Sub
    End Class
End NameSpace