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
Public Class frmModifyText

    Dim cTextData As String
    Public Property TextData() As String
        Get
            Return cTextData
        End Get
        Set(ByVal value As String)
            cTextData = value
            txtText.Text = value
        End Set
    End Property

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check if the input is valid i.e. not blank
        If txtText.Text = "" Then
            Dim reply As Integer = MessageBox.Show("Text field cannot be blank! Would you like to try again?", "Text Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Retry Then
                Exit Sub
            Else
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
                Exit Sub
            End If
        End If
        cTextData = txtText.Text
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class