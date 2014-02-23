'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports EveHQ.Core
Imports System.Text
Imports System.Threading

Namespace Forms

    Public Class FrmModifyEveAccounts
        Dim _testV2APIKeyType As String = "Unknown"
        Dim _testV2APIAccessMask As Long = 0
        Private Const V2URL As String = "https://support.eveonline.com/api"

        Private Sub frmModifyEveAccounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            lblGetAPIKeyV2.Text = V2URL
        End Sub

        Private Sub btnAcceptV2_Click(sender As Object, e As EventArgs) Handles btnAcceptV2.Click
            ' Check if the input is valid i.e. not blank
            If txtUserIDV2.Text.Trim = "" Or txtAPIKeyV2.Text.Trim = "" Or txtAccountNameV2.Text.Trim = "" Then
                Dim reply As Integer = MessageBox.Show("Account details cannot be blank! Would you like to try again?", "Error Creating Account", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = DialogResult.Retry Then
                    Exit Sub
                Else
                    Close()
                    Exit Sub
                End If
            End If
            ' Decide which course of action to take depending on whether adding or editing an account
            If Tag.ToString = "Add" Then
                ' Add the account to the accounts collection
                ' First check if the account already exists
                If HQ.Settings.Accounts.ContainsKey(txtUserIDV2.Text.Trim) Then
                    Dim reply As Integer = MessageBox.Show("Key ID '" & txtUserIDV2.Text & "' already exists in EveHQ! Would you like to try another Key ID?", "Error Creating Account", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                    If reply = DialogResult.Retry Then
                        Exit Sub
                    Else
                        Close()
                        Exit Sub
                    End If
                End If
                Dim newAccount As New EveHQAccount
                newAccount.UserID = txtUserIDV2.Text.Trim
                newAccount.APIKey = txtAPIKeyV2.Text.Trim
                newAccount.FriendlyName = txtAccountNameV2.Text.Trim
                newAccount.ApiKeySystem = APIKeySystems.Version2
                newAccount.CheckAPIKey()
                If ExistingCharactersOnAccount(newAccount) = False Then
                    HQ.Settings.Accounts.Add(newAccount.UserID, newAccount)
                    Close()
                End If
            Else
                ' Fetch the account from the collection
                Dim newAccount As EveHQAccount = HQ.Settings.Accounts(txtUserIDV2.Text.Trim)
                ' Change the password on the account
                newAccount.APIKey = txtAPIKeyV2.Text.Trim
                newAccount.FriendlyName = txtAccountNameV2.Text.Trim
                newAccount.CheckAPIKey()
                Close()
            End If
        End Sub

        Private Function ExistingCharactersOnAccount(newAccount As EveHQAccount) As Boolean

            Dim characterList As List(Of String) = newAccount.GetCharactersOnAccount

            ' Check each pilot for any existing characters
            For Each account As EveHQAccount In HQ.Settings.Accounts.Values
                For Each character As String In account.Characters
                    Select Case account.ApiKeySystem
                        Case APIKeySystems.Version2
                            ' Only check "characters"
                            If characterList.Contains(character) = True Then
                                ' We have a character already
                                Dim msg As New StringBuilder
                                msg.AppendLine("The new account contains an entity (" & character & ") already in use by EveHQ under a new API key. Try deleting the old account first.")
                                MessageBox.Show(msg.ToString, "Duplicate Entity Identified", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return True
                            End If
                    End Select
                Next

            Next

            Return False
        End Function

        Private Sub btnCheckV2_Click(sender As Object, e As EventArgs) Handles btnCheckV2.Click
            Call CheckV2Key()
        End Sub

        Private Sub CheckV2Key()
            If txtAPIKeyV2.Text = "" Then
                lblAPIKeyTypeV2.Text = "Unknown"
            Else
                lblAPIKeyTypeV2.Text = "Checking..."
                lblAPIAccessMask.Text = "Checking..."
                Dim nt As New Thread(AddressOf CheckAPIV2Key)
                nt.IsBackground = True
                nt.Start()
            End If
        End Sub

        Private Sub CheckApiv2Key(ByVal state As Object)
            Dim testAccount As New EveHQAccount
            testAccount.UserID = txtUserIDV2.Text.Trim
            testAccount.APIKey = txtAPIKeyV2.Text.Trim
            testAccount.FriendlyName = txtAccountNameV2.Text.Trim
            testAccount.ApiKeySystem = APIKeySystems.Version2
            testAccount.CheckAPIKey()
            _testV2APIKeyType = testAccount.APIKeyType.ToString
            _testV2APIAccessMask = testAccount.AccessMask
            Invoke(New MethodInvoker(AddressOf UpdateAPIV2KeyType))
        End Sub

        Private Sub UpdateApiv2KeyType()
            lblAPIKeyTypeV2.Text = _testV2APIKeyType
            lblAPIAccessMask.Text = _testV2APIAccessMask.ToString
        End Sub

        Private Sub lblGetAPIKeyV2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblGetAPIKeyV2.LinkClicked
            Try
                Process.Start(lblGetAPIKeyV2.Text)
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Sub

        Private Sub btnCancelV2_Click(sender As Object, e As EventArgs) Handles btnCancelV2.Click
            Close()
        End Sub
    End Class
End NameSpace