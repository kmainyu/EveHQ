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
Imports System.Windows.Forms

Public Class EveAPIStatusForm

    Dim results() As String
    Dim errors As New SortedList
    Dim DefaultColor As Drawing.Color

    Private Sub EveAPIStatusForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If EveHQ.Core.HQ.EveHQSettings.UseAPIRS = True Then
            Me.Text = "Eve API Status - " & EveHQ.Core.HQ.EveHQSettings.APIRSAddress
        Else
            Me.Text = "Eve API Status - " & EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress
        End If

        Dim accountName As String = ""
        errors.Clear()
        If EveHQ.Core.HQ.APIResults.Count > 0 Then
            For Each result As String In EveHQ.Core.HQ.APIResults.Keys
                Dim itemResult As New ListViewItem
                itemResult.UseItemStyleForSubItems = False
                results = result.Split("_".ToCharArray)
                If results.Length = 3 Then
                    If lvwStatus.Items.ContainsKey(results(0) & "_" & results(1)) Then
                        ' Use this listview
                        itemResult = lvwStatus.Items(results(0) & "_" & results(1))
                    Else
                        ' Create a new one
                        itemResult.Name = results(0) & "_" & results(1)
                        accountName = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(results(0)), EveHQ.Core.EveAccount).FriendlyName
                        If accountName <> "" Then
                            itemResult.Text = accountName
                        Else
                            itemResult.Text = results(0)
                        End If
                        For si As Integer = 1 To 4
                            itemResult.SubItems.Add("n/a")
                        Next
                    End If
                Else
                    itemResult.Name = results(0)
                    accountName = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(results(0)), EveHQ.Core.EveAccount).FriendlyName
                    If accountName <> "" Then
                        itemResult.Text = accountName
                    Else
                        itemResult.Text = results(0)
                    End If
                    For si As Integer = 1 To 4
                        itemResult.SubItems.Add("n/a")
                    Next
                End If

                If results.Length = 1 Then
                    ' Add Result for account XML here
                    Call DisplayAPIResult(2, CInt(EveHQ.Core.HQ.APIResults(result)), itemResult)
                Else
                    ' Set pilot
                    itemResult.SubItems(1).Text = results(1)
                    ' Add Result for character XML
                    If CInt(results(2)) = EveAPI.APITypes.CharacterSheet Then
                        Call DisplayAPIResult(3, CInt(EveHQ.Core.HQ.APIResults(result)), itemResult)
                    End If
                    ' Add Result for training XML
                    If CInt(results(2)) = EveAPI.APITypes.SkillQueue Then
                        Call DisplayAPIResult(4, CInt(EveHQ.Core.HQ.APIResults(result)), itemResult)
                    End If
                End If
                If lvwStatus.Items.ContainsKey(itemResult.Name) = False Then
                    lvwStatus.Items.Add(itemResult)
                End If
            Next

            ' Display any error codes
            If errors.Count > 0 Then
                Dim strError As New System.Text.StringBuilder
                For Each errCode As String In errors.Keys
                    strError.AppendLine("Code: " & errCode & " - " & CStr(errors(errCode)))
                Next
                lblErrorDetails.Text = strError.ToString
            End If
        End If

        DefaultColor = lvwStatus.Items(0).BackColor

    End Sub

    'Private Sub lvwStatus_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvwStatus.MouseDown
    '    lvwStatus.BeginUpdate()
    '    Dim hti As ListViewHitTestInfo = lvwStatus.HitTest(e.Location)
    '    If hti.SubItem IsNot Nothing Then
    '        Dim sID As Integer = hti.Item.SubItems.IndexOf(hti.SubItem)
    '        Call ClearItemSelection()
    '        hti.Item.SubItems(sID).BackColor = Drawing.Color.LightSteelBlue
    '        lvwStatus.EndUpdate()
    '        If hti.Item.SubItems(sID).Tag IsNot Nothing Then
    '            Dim errorCode As Integer = CInt(hti.Item.SubItems(sID).Tag)
    '            If errorCode < 0 Then
    '                errorCode *= -1
    '            End If
    '            lblErrorDetails.Text = CStr(EveHQ.Core.HQ.APIErrors(CStr(errorCode)))
    '        End If
    '    End If
    'End Sub

    'Private Sub ClearItemSelection()
    '    For row As Integer = 0 To lvwStatus.Items.Count - 1
    '        lvwStatus.Items(row).BackColor = Control.DefaultBackColor
    '        For col As Integer = 0 To 4
    '            lvwStatus.Items(row).SubItems(col).BackColor = DefaultColor
    '        Next
    '    Next
    'End Sub

    Private Sub DisplayAPIResult(ByVal idx As Integer, ByVal result As Integer, ByRef LVItem As ListViewItem)
        LVItem.SubItems(idx).Tag = result
        Select Case result
            Case EveAPI.APIResults.ReturnedNew
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Green
                LVItem.SubItems(idx).Text = "New"
            Case EveAPI.APIResults.ReturnedCached
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Blue
                LVItem.SubItems(idx).Text = "Cached"
            Case EveAPI.APIResults.ReturnedActual
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Green
                LVItem.SubItems(idx).Text = "Actual"
            Case EveAPI.APIResults.APIServerDownReturnedCached
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Blue
                LVItem.SubItems(idx).Text = "Down - Cached"
            Case EveAPI.APIResults.APIServerDownReturnedNull
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Red
                LVItem.SubItems(idx).Text = "Server Down"
            Case EveAPI.APIResults.PageNotFound
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Red
                LVItem.SubItems(idx).Text = "Page Not Found"
            Case EveAPI.APIResults.TimedOut
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Red
                LVItem.SubItems(idx).Text = "Timed Out"
            Case EveAPI.APIResults.CCPError
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Red
                LVItem.SubItems(idx).Text = "API Error"
            Case EveAPI.APIResults.UnknownError
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Red
                LVItem.SubItems(idx).Text = "Unknown"
            Case Is < 0
                LVItem.SubItems(idx).ForeColor = Drawing.Color.Red
                LVItem.SubItems(idx).Text = "API Error " & Math.Abs(result).ToString
                Dim errorCode As String = Math.Abs(result).ToString
                If errors.Contains(errorCode) = False Then
                    errors.Add(errorCode, CStr(EveHQ.Core.HQ.APIErrors(CStr(errorCode))))
                End If
        End Select
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class