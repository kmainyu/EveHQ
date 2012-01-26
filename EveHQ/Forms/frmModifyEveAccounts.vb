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
Imports System.Text

Public Class frmModifyEveAccounts

    Dim TestV1APIKeyType As String = "Unknown"
    Dim TestV2APIKeyType As String = "Unknown"
    Dim TestV2APIAccessMask As Long = 0
    Dim V1URL As String = "http://myeve.eve-online.com/api/default.asp"
    Dim V2URL As String = "https://support.eveonline.com/api"

    Private Sub frmModifyEveAccounts_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lblGetAPIKeyV2.Text = V2URL
    End Sub

    Private Sub btnAcceptV2_Click(sender As System.Object, e As System.EventArgs) Handles btnAcceptV2.Click
        ' Check if the input is valid i.e. not blank
        If txtUserIDV2.Text.Trim = "" Or txtAPIKeyV2.Text.Trim = "" Or txtAccountNameV2.Text.Trim = "" Then
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
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(txtUserIDV2.Text.Trim) Then
                Dim reply As Integer = MessageBox.Show("Key ID '" & txtUserIDV2.Text & "' already exists in EveHQ! Would you like to try another Key ID?", "Error Creating Account", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = Windows.Forms.DialogResult.Retry Then
                    Exit Sub
                Else
                    Me.Close()
                    Exit Sub
                End If
            End If
            Dim newAccount As EveHQ.Core.EveAccount = New EveHQ.Core.EveAccount
            newAccount.userID = txtUserIDV2.Text.Trim
            newAccount.APIKey = txtAPIKeyV2.Text.Trim
            newAccount.FriendlyName = txtAccountNameV2.Text.Trim
            newAccount.APIKeySystem = Core.APIKeySystems.Version2
            newAccount.CheckAPIKey()
            If ExistingCharactersOnAccount(newAccount) = False Then
                EveHQ.Core.HQ.EveHQSettings.Accounts.Add(newAccount, newAccount.userID)
                Me.Close()
            End If
        Else
            ' Fetch the account from the collection
            Dim newAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(txtUserIDV2.Text.Trim), Core.EveAccount)
            ' Change the password on the account
            newAccount.APIKey = txtAPIKeyV2.Text.Trim
            newAccount.FriendlyName = txtAccountNameV2.Text.Trim
            newAccount.CheckAPIKey()
            Me.Close()
        End If
    End Sub

    Private Function ExistingCharactersOnAccount(NewAccount As EveHQ.Core.EveAccount) As Boolean

        Dim CharacterList As List(Of String) = NewAccount.GetCharactersOnAccount

        ' Check each pilot for any existing characters
        For Each Account As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            For Each Character As String In Account.Characters
                Select Case Account.APIKeySystem
                    Case Core.APIKeySystems.Version2
                        ' Only check "characters"
                        If CharacterList.Contains(Character) = True Then
                            ' We have a character already
                            Dim msg As New StringBuilder
                            msg.AppendLine("The new account contains an entity (" & Character & ") already in use by EveHQ under a new API key. Try deleting the old account first.")
                            MessageBox.Show(msg.ToString, "Duplicate Entity Identified", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return True
                        End If
                    Case Else
                        ' Ignore
                End Select
            Next

        Next

        Return False

    End Function

    Private Sub btnCheckV2_Click(sender As System.Object, e As System.EventArgs) Handles btnCheckV2.Click
        Call Me.CheckV2Key()
    End Sub

    Private Sub CheckV2Key()
        If txtAPIKeyV2.Text = "" Then
            lblAPIKeyTypeV2.Text = "Unknown"
        Else
            lblAPIKeyTypeV2.Text = "Checking..."
            lblAPIAccessMask.Text = "Checking..."
            Dim NT As New Threading.Thread(AddressOf Me.CheckAPIV2Key)
            NT.Start()
        End If
    End Sub

    Private Sub CheckAPIV2Key(ByVal State As Object)
        Dim TestAccount As EveHQ.Core.EveAccount = New EveHQ.Core.EveAccount
        TestAccount.userID = txtUserIDV2.Text.Trim
        TestAccount.APIKey = txtAPIKeyV2.Text.Trim
        TestAccount.FriendlyName = txtAccountNameV2.Text.Trim
        TestAccount.APIKeySystem = Core.APIKeySystems.Version2
        TestAccount.CheckAPIKey()
        Me.TestV2APIKeyType = TestAccount.APIKeyType.ToString
        Me.TestV2APIAccessMask = TestAccount.AccessMask
        Me.Invoke(New MethodInvoker(AddressOf UpdateAPIV2KeyType))
    End Sub

    Private Sub UpdateAPIV2KeyType()
        lblAPIKeyTypeV2.Text = Me.TestV2APIKeyType
        lblAPIAccessMask.Text = Me.TestV2APIAccessMask.ToString
    End Sub

    Private Sub lblGetAPIKeyV2_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblGetAPIKeyV2.LinkClicked
        Try
            Process.Start(lblGetAPIKeyV2.Text)
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub btnCancelV2_Click(sender As System.Object, e As System.EventArgs) Handles btnCancelV2.Click
        Me.Close()
    End Sub

End Class