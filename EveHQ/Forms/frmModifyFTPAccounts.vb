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
Public Class frmModifyFTPAccounts

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        ' Check if the input is valid i.e. not blank
        If txtFTPName.Text = "" Then
            MessageBox.Show("The FTP Name cannot be blank. Please try again.", "Error Creating FTP Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If txtServer.Text = "" Then
            MessageBox.Show("The FTP Server cannot be blank. Please try again.", "Error Creating FTP Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If IsNumeric(txtPort.Text) = False Then
            MessageBox.Show("The FTP Port must be numeric. Please try again.", "Error Creating FTP Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Decide which course of action to take depending on whether adding or editing an account
        If btnAccept.Text = "Add" Then
            ' Add the account to the accounts collection
            ' First check if the account already exists
            If EveHQ.Core.HQ.FTPAccounts.Contains(txtFTPName.Text) Then
                Dim reply As Integer = MessageBox.Show("Account name '" & txtFTPName.Text & "' already exists! Would you like to try another Account name?", "Error Creating Account", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = Windows.Forms.DialogResult.Retry Then
                    Exit Sub
                Else
                    Me.Close()
                    Exit Sub
                End If
            End If
            Dim newAccount As EveHQ.Core.FTPAccount = New EveHQ.Core.FTPAccount
            newAccount.FTPName = txtFTPName.Text
            newAccount.Server = txtServer.Text
            newAccount.Port = CInt(txtPort.Value)
            newAccount.Path = txtPath.Text
            newAccount.Username = txtUsername.Text
            newAccount.Password = txtPassword.Text
            EveHQ.Core.HQ.FTPAccounts.Add(newAccount, newAccount.FTPName)
        Else
            ' Fetch the account from the collection
            Dim newAccount As EveHQ.Core.FTPAccount = CType(EveHQ.Core.HQ.FTPAccounts(txtFTPName.Text), Core.FTPAccount)
            ' Change the password on the account
            newAccount.Server = txtServer.Text
            newAccount.Port = CInt(txtPort.Value)
            newAccount.Path = txtPath.Text
            newAccount.Username = txtUsername.Text
            newAccount.Password = txtPassword.Text
        End If
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class