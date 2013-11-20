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
Imports System.Drawing
Imports System.Windows.Forms
Imports EveHQ.EveAPI
Imports System.Text

Public Class EveAPIStatusForm

    Dim _results() As String
    ReadOnly _errors As New SortedList

    Private Sub EveAPIStatusForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        If HQ.Settings.UseApirs = True Then
            Text = "Eve API Status - " & HQ.Settings.ApirsAddress
        Else
            Text = "Eve API Status - " & HQ.Settings.CcpapiServerAddress
        End If

        Dim accountName As String
        _errors.Clear()
        If HQ.APIResults.Count > 0 Then
            For Each result As String In HQ.APIResults.Keys
                Dim itemResult As New ListViewItem
                itemResult.UseItemStyleForSubItems = False
                _results = result.Split("_".ToCharArray)
                If _results.Length = 3 Then
                    If lvwStatus.Items.ContainsKey(_results(0) & "_" & _results(1)) Then
                        ' Use this listview
                        itemResult = lvwStatus.Items(_results(0) & "_" & _results(1))
                    Else
                        ' Create a new one
                        itemResult.Name = _results(0) & "_" & _results(1)
                        accountName = HQ.Settings.Accounts(_results(0)).FriendlyName
                        If accountName <> "" Then
                            itemResult.Text = accountName
                        Else
                            itemResult.Text = _results(0)
                        End If
                        ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                        For si As Integer = 1 To 4
                            itemResult.SubItems.Add("n/a")
                        Next
                    End If
                Else
                    itemResult.Name = _results(0)
                    accountName = HQ.Settings.Accounts(_results(0)).FriendlyName
                    If accountName <> "" Then
                        itemResult.Text = accountName
                    Else
                        itemResult.Text = _results(0)
                    End If
                    ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                    For si As Integer = 1 To 4
                        itemResult.SubItems.Add("n/a")
                    Next
                End If

                If _results.Length = 1 Then
                    ' Add Result for account XML here
                    Call DisplayAPIResult(2, CInt(HQ.APIResults(result)), itemResult)
                Else
                    ' Set pilot
                    itemResult.SubItems(1).Text = _results(1)
                    ' Add Result for character XML
                    If CInt(_results(2)) = APITypes.CharacterSheet Then
                        Call DisplayAPIResult(3, CInt(HQ.APIResults(result)), itemResult)
                    End If
                    ' Add Result for training XML
                    If CInt(_results(2)) = APITypes.SkillQueue Then
                        Call DisplayAPIResult(4, CInt(HQ.APIResults(result)), itemResult)
                    End If
                End If
                If lvwStatus.Items.ContainsKey(itemResult.Name) = False Then
                    lvwStatus.Items.Add(itemResult)
                End If
            Next

            ' Display any error codes
            If _errors.Count > 0 Then
                Dim strError As New StringBuilder
                For Each errCode As String In _errors.Keys
                    strError.AppendLine("Code: " & errCode & " - " & CStr(_errors(errCode)))
                Next
                lblErrorDetails.Text = strError.ToString
            End If
        End If
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

    Private Sub DisplayAPIResult(ByVal idx As Integer, ByVal result As Integer, ByRef lvItem As ListViewItem)
        LVItem.SubItems(idx).Tag = result
        Select Case result
            Case APIResults.ReturnedNew
                LVItem.SubItems(idx).ForeColor = Color.Green
                LVItem.SubItems(idx).Text = "New"
            Case APIResults.ReturnedCached
                LVItem.SubItems(idx).ForeColor = Color.Blue
                LVItem.SubItems(idx).Text = "Cached"
            Case APIResults.ReturnedActual
                LVItem.SubItems(idx).ForeColor = Color.Green
                LVItem.SubItems(idx).Text = "Actual"
            Case APIResults.APIServerDownReturnedCached
                LVItem.SubItems(idx).ForeColor = Color.Blue
                LVItem.SubItems(idx).Text = "Down - Cached"
            Case APIResults.APIServerDownReturnedNull
                LVItem.SubItems(idx).ForeColor = Color.Red
                LVItem.SubItems(idx).Text = "Server Down"
            Case APIResults.PageNotFound
                LVItem.SubItems(idx).ForeColor = Color.Red
                LVItem.SubItems(idx).Text = "Page Not Found"
            Case APIResults.TimedOut
                LVItem.SubItems(idx).ForeColor = Color.Red
                LVItem.SubItems(idx).Text = "Timed Out"
            Case APIResults.CCPError
                LVItem.SubItems(idx).ForeColor = Color.Red
                LVItem.SubItems(idx).Text = "API Error"
            Case APIResults.UnknownError
                LVItem.SubItems(idx).ForeColor = Color.Red
                LVItem.SubItems(idx).Text = "Unknown"
            Case Is < 0
                LVItem.SubItems(idx).ForeColor = Color.Red
                LVItem.SubItems(idx).Text = "API Error " & Math.Abs(result).ToString
                Dim errorCode As String = Math.Abs(result).ToString
                If _errors.Contains(errorCode) = False Then
                    _errors.Add(errorCode, CStr(HQ.APIErrors(CStr(errorCode))))
                End If
        End Select
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Close()
    End Sub
End Class