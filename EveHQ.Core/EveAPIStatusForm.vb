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
Public Class EveAPIStatusForm

    Dim TimeToClose As Integer = 11
    Private cContainsError As Boolean = False
    Public Property ContainsError() As Boolean
        Get
            Return cContainsError
        End Get
        Set(ByVal value As Boolean)
            cContainsError = value
            If value = True Then
                cContainsError = value
                EveHQ.Core.HQ.LastAutoAPIResult = False
                EveHQ.Core.HQ.LastAutoAPITime = Now
            Else
                EveHQ.Core.HQ.LastAutoAPIResult = True
                EveHQ.Core.HQ.LastAutoAPITime = Now
            End If
        End Set
    End Property

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub

    Private Sub btnClose_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.EnabledChanged
        If btnClose.Enabled = True Then
            TimeToClose = 11
            tmrClose.Enabled = True
        End If
    End Sub

    Private Sub tmrClose_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrClose.Tick
        TimeToClose -= 1
        If TimeToClose <= 0 Then
            If Me.ContainsError = False Then
                Me.Hide()
            Else
                tmrClose.Enabled = False
                btnClose.Text = "Close (Errors Detected!)"
            End If
        Else
            btnClose.Text = "Close (Autoclose in " & TimeToClose & " s)"
        End If
    End Sub

    Private Sub EveAPIStatusForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Me.Hide()
    End Sub

    Private Sub EveAPIStatusForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        btnClose.Enabled = False
        btnClose.Text = "Close"
        TimeToClose = 11
        ContainsError = False
        Call EveHQ.Core.PilotParseFunctions.GetCharacterData()
    End Sub

    Private Sub lstStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstStatus.SelectedIndexChanged
        If lstStatus.SelectedItems.Count > 0 Then
            lblErrorReason.Text = CStr(lstStatus.SelectedItems(0).Tag)
        End If
    End Sub

    Private Sub EveAPIStatusForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If EveHQ.Core.HQ.EveHQSettings.UseAPIRS = True Then
            Me.Text = "Eve API Status - " & EveHQ.Core.HQ.EveHQSettings.APIRSAddress
        Else
            Me.Text = "Eve API Status - " & EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress
        End If
    End Sub
End Class