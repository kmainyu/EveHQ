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

Imports System.Windows.Forms

Public Class frmVoid

    Dim WHEffects As New SortedList(Of String, String)

    Private Sub frmVoid_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.LoadWHData()
        Call Me.LoadWHSystemData()
        Call Me.LoadWHEffects()
    End Sub

    Private Sub LoadWHData()
        cboWHType.BeginUpdate()
        cboWHType.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHType.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHType.Items.Clear()
        For Each WH As Void.WormHole In VoidData.Wormholes.Values
            cboWHType.Items.Add(WH.Name)
            cboWHType.AutoCompleteCustomSource.Add(WH.Name)
        Next
        cboWHType.EndUpdate()
    End Sub

    Private Sub LoadWHSystemData()
        ' Load up pilot information
        cboWHSystem.BeginUpdate()
        cboWHSystem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHSystem.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHSystem.Items.Clear()
        For Each WH As Void.WormholeSystem In VoidData.WormholeSystems.Values
            cboWHSystem.Items.Add(WH.Name)
            cboWHSystem.AutoCompleteCustomSource.Add(WH.Name)
        Next
        cboWHSystem.EndUpdate()
    End Sub

    Private Sub LoadWHEffects()
        ' Parse the WHEffects resource
        WHEffects = New SortedList(Of String, String)
        Dim Effects() As String = My.Resources.WHEffects.Split((ControlChars.CrLf).ToCharArray)
        For Each Effect As String In Effects
            If Effect <> "" Then
                Dim EffectData() As String = Effect.Split(",".ToCharArray)
                If WHEffects.ContainsKey(EffectData(0)) = False Then
                    WHEffects.Add(EffectData(0), EffectData(10))
                End If
            End If
        Next
    End Sub

    Private Sub cboWHType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHType.SelectedIndexChanged
        ' Update the WH Details
        If VoidData.Wormholes.ContainsKey(cboWHType.SelectedItem.ToString) = True Then
            Dim WH As Void.WormHole = VoidData.Wormholes(cboWHType.SelectedItem.ToString)
            If WH.Name <> "K162" Then
                lblTargetSystemClass.Text = WH.TargetClass
                Select Case CInt(WH.TargetClass)
                    Case 1 To 6
                        lblTargetSystemClass.Text &= " (Wormhole Class " & WH.TargetClass & ")"
                    Case 7
                        lblTargetSystemClass.Text &= " (High Security Space)"
                    Case 8
                        lblTargetSystemClass.Text &= " (Low Security Space)"
                    Case 9
                        lblTargetSystemClass.Text &= " (Null Security Space)"
                End Select
                lblMaxJumpableMass.Text = CLng(WH.MaxJumpableMass).ToString("N0") & " kg"
                lblMaxTotalMass.Text = CLng(WH.MaxMassCapacity).ToString("N0") & " kg"
                lblStabilityWindow.Text = (CDbl(WH.MaxStabilityWindow) / 60).ToString("N0") & " hours"
            Else
                lblTargetSystemClass.Text = "n/a (Return wormhole)"
                lblMaxJumpableMass.Text = "n/a"
                lblMaxTotalMass.Text = "n/a"
                lblStabilityWindow.Text = "n/a"
            End If
        End If
    End Sub

    Private Sub cboWHSystem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHSystem.SelectedIndexChanged
        ' Update the WH System Details
        If VoidData.WormholeSystems.ContainsKey(cboWHSystem.SelectedItem.ToString) = True Then
            Dim WH As Void.WormholeSystem = VoidData.WormholeSystems(cboWHSystem.SelectedItem.ToString)
            If WH.WEffect <> "" Then
                Dim modName As String = ""
                If WH.WEffect = "Red Giant" Then
                    modName = WH.WEffect & " Beacon Class " & WH.WClass
                Else
                    modName = WH.WEffect & " Effect Beacon Class " & WH.WClass
                End If
                'Dim SSun As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(EveHQ.Core.HQ.itemList(modName))
                lblAnomalyName.Text = WH.WEffect
                ' Establish the effects
                Dim EffectList As New SortedList(Of String, Double)
                Dim SysEffects As WormholeEffect = VoidData.WormholeEffects(modName)
                For Each att As String In SysEffects.Attributes.Keys
                    If WHEffects.ContainsKey(att) = True Then
                        EffectList.Add(WHEffects(att), SysEffects.Attributes(att))
                    End If
                Next
                lvwEffects.BeginUpdate()
                lvwEffects.Items.Clear()
                For Each Effect As String In EffectList.Keys
                    Dim newEffect As New ListViewItem
                    newEffect.Text = Effect
                    Dim value As Double = CDbl(EffectList(Effect))
                    If value < 5 And value > -5 Then
                        If value < 1 Or Effect.EndsWith("Penalty") Then
                            newEffect.ForeColor = Drawing.Color.Red
                        Else
                            newEffect.ForeColor = Drawing.Color.LimeGreen
                        End If
                        newEffect.SubItems.Add(EffectList(Effect).ToString("N2") & " x")
                    Else
                        If value < 0 Or Effect.EndsWith("Penalty") Then
                            newEffect.ForeColor = Drawing.Color.Red
                        Else
                            newEffect.ForeColor = Drawing.Color.LimeGreen
                        End If
                        newEffect.SubItems.Add(EffectList(Effect).ToString("N2") & " %")
                    End If
                    lvwEffects.Items.Add(newEffect)
                Next
                lvwEffects.EndUpdate()
            Else
                lblAnomalyName.Text = "<None>"
                lvwEffects.Items.Clear()
            End If
            lblSystemClass.Text = WH.WClass
        End If
    End Sub
End Class