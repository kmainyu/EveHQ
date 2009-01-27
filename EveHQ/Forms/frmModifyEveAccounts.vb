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
Public Class frmModifyEveAccounts

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check if the input is valid i.e. not blank
        If txtUserID.Text = "" Or txtAPIKey.Text = "" Or txtAccountName.Text = "" Then
            Dim reply As Integer = MessageBox.Show("Account details cannot be blank! Would you like to try again?", "Error Creating Account", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Retry Then
                Exit Sub
            Else
                Me.Close()
                Exit Sub
            End If
        End If
        ' Decide which course of action to take depending on whether adding or editing an account
        If Me.Tag.ToString = "Add" Then
            ' Add the account to the accounts collection
            ' First check if the account already exists
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(txtUserID.Text) Then
                Dim reply As Integer = MessageBox.Show("Account name " & txtUserID.Text & " already exists in EveHQ! Would you like to try another Account name?", "Error Creating Account", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = Windows.Forms.DialogResult.Retry Then
                    Exit Sub
                Else
                    Me.Close()
                    Exit Sub
                End If
            End If
            Dim newAccount As EveHQ.Core.EveAccount = New EveHQ.Core.EveAccount
            newAccount.userID = txtUserID.Text
            newAccount.APIKey = txtAPIKey.Text
            newAccount.FriendlyName = txtAccountName.Text
            EveHQ.Core.HQ.EveHQSettings.Accounts.Add(newAccount, newAccount.userID)
        Else
            ' Fetch the account from the collection
            Dim newAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(txtUserID.Text), Core.EveAccount)
            ' Change the password on the account
            newAccount.APIKey = txtAPIKey.Text
            newAccount.FriendlyName = txtAccountName.Text
        End If
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub lblGetAPIKey_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblGetAPIKey.LinkClicked
        Try
            Process.Start(lblGetAPIKey.Text)
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class