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
Public Class frmEditImplants

    Dim cDisplayPilotName As String
    Dim DisplayPilot As EveHQ.Core.Pilot

    Public Property DisplayPilotName() As String
        Get
            Return cDisplayPilotName
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
            DisplayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(value), Core.Pilot)
        End Set
    End Property

    Private Sub nudC_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudC.ValueChanged
        DisplayPilot.CImplantM = CInt(nudC.Value)
    End Sub

    Private Sub nudI_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudI.ValueChanged
        DisplayPilot.IImplantM = CInt(nudI.Value)
    End Sub

    Private Sub nudM_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudM.ValueChanged
        DisplayPilot.MImplantM = CInt(nudM.Value)
    End Sub

    Private Sub nudP_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudP.ValueChanged
        DisplayPilot.PImplantM = CInt(nudP.Value)
    End Sub

    Private Sub nudW_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudW.ValueChanged
        DisplayPilot.WImplantM = CInt(nudW.Value)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmEditImplants_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        nudC.Value = DisplayPilot.CImplantM
        nudI.Value = DisplayPilot.IImplantM
        nudM.Value = DisplayPilot.MImplantM
        nudP.Value = DisplayPilot.PImplantM
        nudW.Value = DisplayPilot.WImplantM
    End Sub
End Class