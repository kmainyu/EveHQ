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
Public Class frmEveAPIProxy
    Dim myEveAPIProxy As New EveAPI.EveAPIProxy(26002, "")

    Private Sub tmrMessage_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMessage.Tick
        ' Check for proxy messages
        If myEveAPIProxy.Messages.Count > 0 Then
            For m As Integer = 0 To myEveAPIProxy.Messages.Count - 1
                Dim newEvent As EveAPI.EveAPIProxyEvent = CType(myEveAPIProxy.Messages.Dequeue, EveAPI.EveAPIProxyEvent)
                Dim newLI As New ListViewItem(newEvent.EventDate.ToString)
                newLI.SubItems.Add(newEvent.EventType.ToString)
                newLI.SubItems.Add(newEvent.EventRef.ToString)
                newLI.SubItems.Add(newEvent.Description)
                lvwEvents.BeginUpdate()
                lvwEvents.Items.Add(newLI)
                lvwEvents.EndUpdate()
            Next
        End If
    End Sub

    Private Sub frmEveAPIProxy_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        myEveAPIProxy.StopServer()
    End Sub

    Private Sub frmEveAPIProxy_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tmrMessage.Start()
        myEveAPIProxy.StartServer()
    End Sub
End Class
