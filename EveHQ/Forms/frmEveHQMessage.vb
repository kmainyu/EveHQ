﻿' ========================================================================
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
Public Class frmEveHQMessage

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        EveHQ.Core.HQ.Settings.IgnoreLastMessage = chkIgnore.Checked
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub LinkClicked(ByVal sender As Object, ByVal e As DevComponents.DotNetBar.MarkupLinkClickEventArgs) Handles lblMessage.MarkupLinkClick
        Try
            Process.Start(e.HRef)
        Catch ex As Exception
            ' Don't do anything
        End Try
    End Sub

End Class