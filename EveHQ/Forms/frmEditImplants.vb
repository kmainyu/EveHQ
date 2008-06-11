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
Public Class frmEditImplants

    Private Sub nudC_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudC.ValueChanged
        EveHQ.Core.HQ.myPilot.CImplantM = CInt(nudC.Value)
    End Sub

    Private Sub nudI_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudI.ValueChanged
        EveHQ.Core.HQ.myPilot.IImplantM = CInt(nudI.Value)
    End Sub

    Private Sub nudM_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudM.ValueChanged
        EveHQ.Core.HQ.myPilot.MImplantM = CInt(nudM.Value)
    End Sub

    Private Sub nudP_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudP.ValueChanged
        EveHQ.Core.HQ.myPilot.PImplantM = CInt(nudP.Value)
    End Sub

    Private Sub nudW_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudW.ValueChanged
        EveHQ.Core.HQ.myPilot.WImplantM = CInt(nudW.Value)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmEditImplants_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        nudC.Value = EveHQ.Core.HQ.myPilot.CImplantM
        nudI.Value = EveHQ.Core.HQ.myPilot.IImplantM
        nudM.Value = EveHQ.Core.HQ.myPilot.MImplantM
        nudP.Value = EveHQ.Core.HQ.myPilot.PImplantM
        nudW.Value = EveHQ.Core.HQ.myPilot.WImplantM
    End Sub
End Class