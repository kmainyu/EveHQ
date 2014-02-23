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

Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Namespace Forms

    Public Class FrmModifyImplantGroups

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Check if the input is valid i.e. not blank
            If txtGroupName.Text = "" Then
                Dim reply As Integer = MessageBox.Show("Group Name cannot be blank! Would you like to try again?", "Error Creating Implant Group", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                If reply = DialogResult.Retry Then
                    Exit Sub
                Else
                    Close()
                    Exit Sub
                End If
            End If
            ' Decide which course of action to take depending on whether adding or editing a group
            Select Case Tag.ToString
                Case "Add"
                    ' Add the group to the group collection
                    ' First check if the group already exists
                    If PluginSettings.HQFSettings.ImplantGroups.ContainsKey(txtGroupName.Text) Then
                        Dim reply As Integer = MessageBox.Show("Group Name '" & txtGroupName.Text & "' already exists!" & ControlChars.CrLf & "Would you like to try another Group Name?", "Error Creating Implant Group", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                        If reply = DialogResult.Retry Then
                            Exit Sub
                        Else
                            Close()
                            Exit Sub
                        End If
                    End If
                    Dim newGroup As New ImplantCollection(True)
                    newGroup.GroupName = txtGroupName.Text
                    For imp As Integer = 1 To 10
                        newGroup.ImplantName(imp) = ""
                    Next
                    PluginSettings.HQFSettings.ImplantGroups.Add(newGroup.GroupName, newGroup)
                    txtGroupName.Tag = newGroup.GroupName
                Case "Edit"
                    If PluginSettings.HQFSettings.ImplantGroups.ContainsKey(txtGroupName.Text) Then
                        Dim reply As Integer = MessageBox.Show("Group Name " & txtGroupName.Text & " already exists!" & ControlChars.CrLf & "Would you like to try another Queue name?", "Error Editing Implant Group", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
                        If reply = DialogResult.Retry Then
                            Exit Sub
                        Else
                            Close()
                            Exit Sub
                        End If
                    End If
                    ' Fetch the group from the collection
                    Dim oldGroup As ImplantCollection = PluginSettings.HQFSettings.ImplantGroups.Item(txtGroupName.Tag.ToString)
                    oldGroup.GroupName = txtGroupName.Text
                    ' Remove the old group
                    PluginSettings.HQFSettings.ImplantGroups.Remove(txtGroupName.Tag.ToString)
                    ' Add the new group
                    PluginSettings.HQFSettings.ImplantGroups.Add(oldGroup.GroupName, oldGroup)
                    txtGroupName.Tag = oldGroup.GroupName
            End Select
            Close()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub txtQueueName_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtGroupName.KeyPress
            Dim myMatch As Match = Regex.Match(e.KeyChar, "^[0-9a-zA-Z\x20\x08()-_+]*$")
            If myMatch.Success = False Then
                e.KeyChar = CChar("")
            End If
        End Sub
    End Class
End NameSpace