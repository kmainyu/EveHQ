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

Public Class frmModifyFittingName

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check if the input is valid i.e. not blank
        If txtFittingName.Text = "" Then
            Dim reply As Integer = MessageBox.Show("Fitting Name cannot be blank! Would you like to try again?", "Error Creating Fitting", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Retry Then
                Exit Sub
            Else
                Me.Close()
                Exit Sub
            End If
        End If
        ' Decide which course of action to take depending on whether adding or editing an account
        Select Case Me.Tag.ToString
            Case "Add"
                ' Add the account to the accounts collection
                ' First check if the fitting already exists
                Dim fittingKeyName As String = btnAccept.Tag.ToString & ", " & txtFittingName.Text
                If Fittings.FittingList.Contains(fittingKeyName) Then
                    Dim reply As Integer = MessageBox.Show("Fitting Name '" & txtFittingName.Text & "' already exists for this ship!" & ControlChars.CrLf & "Would you like to try another name?", "Error Creating Fitting", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        txtFittingName.Text = ""
                        Me.Close()
                        Exit Sub
                    End If
                End If
            Case "Copy"
                ' Add the account to the accounts collection
                ' First check if the fitting already exists
                Dim fittingKeyName As String = btnAccept.Tag.ToString & ", " & txtFittingName.Text
                If Fittings.FittingList.Contains(fittingKeyName) Then
                    Dim reply As Integer = MessageBox.Show("Fitting Name '" & txtFittingName.Text & "' already exists for this ship!" & ControlChars.CrLf & "Would you like to try another name?", "Error Copying Fitting", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        txtFittingName.Text = ""
                        Me.Close()
                        Exit Sub
                    End If
                End If
            Case "Edit"
                ' Add the account to the accounts collection
                ' First check if the fitting already exists
                Dim fittingKeyName As String = btnAccept.Tag.ToString & ", " & txtFittingName.Text
                If Fittings.FittingList.Contains(fittingKeyName) Then
                    Dim reply As Integer = MessageBox.Show("Fitting Name '" & txtFittingName.Text & "' already exists for this ship!" & ControlChars.CrLf & "Would you like to try another name?", "Error Editing Fitting Name", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.Retry Then
                        Exit Sub
                    Else
                        txtFittingName.Text = ""
                        Me.Close()
                        Exit Sub
                    End If
                End If
        End Select
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        txtFittingName.Text = ""
        Me.Close()
    End Sub

    Private Sub txtFittingName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFittingName.KeyPress
        Dim myMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(e.KeyChar, "^[0-9a-zA-Z\x20\x08]*$")
        If myMatch.Success = False Then
            e.KeyChar = CChar("")
        End If
    End Sub
End Class