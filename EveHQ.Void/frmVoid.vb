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


Imports System.Windows.Forms

Public Class FrmVoid

    Dim _whEffects As New SortedList(Of String, String)

    Private Sub frmVoid_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        Call LoadWormholeData()
        Call LoadWormholeSystemData()
        Call LoadWormholeEffects()
    End Sub

    Private Sub LoadWormholeData()
        cboWHType.BeginUpdate()
        cboWHType.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHType.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHType.Items.Clear()
        For Each wh As WormHole In VoidData.Wormholes.Values
            cboWHType.Items.Add(wh.Name)
            cboWHType.AutoCompleteCustomSource.Add(wh.Name)
        Next
        cboWHType.EndUpdate()
    End Sub

    Private Sub LoadWormholeSystemData()
        ' Load up pilot information
        cboWHSystem.BeginUpdate()
        cboWHSystem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHSystem.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHSystem.Items.Clear()
        For Each wh As WormholeSystem In VoidData.WormholeSystems.Values
            cboWHSystem.Items.Add(wh.Name)
            cboWHSystem.AutoCompleteCustomSource.Add(wh.Name)
        Next
        cboWHSystem.EndUpdate()
    End Sub

    Private Sub LoadWormholeEffects()
        ' Parse the WHEffects resource
        _whEffects = New SortedList(Of String, String)
        Dim effects() As String = My.Resources.WHEffects.Split((ControlChars.CrLf).ToCharArray)
        For Each effect As String In effects
            If effect <> "" Then
                Dim effectData() As String = effect.Split(",".ToCharArray)
                If _whEffects.ContainsKey(effectData(0)) = False Then
                    _whEffects.Add(effectData(0), effectData(10))
                End If
            End If
        Next
    End Sub

    Private Sub cboWHType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles cboWHType.SelectedIndexChanged
        ' Update the WH Details
        If VoidData.Wormholes.ContainsKey(cboWHType.SelectedItem.ToString) = True Then
            Dim wh As WormHole = VoidData.Wormholes(cboWHType.SelectedItem.ToString)
            If wh.Name <> "K162" Then
                lblTargetSystemClass.Text = wh.TargetClass
                Select Case CInt(wh.TargetClass)
                    Case 1 To 6
                        lblTargetSystemClass.Text &= " (Wormhole Class " & wh.TargetClass & ")"
                    Case 7
                        lblTargetSystemClass.Text &= " (High Security Space)"
                    Case 8
                        lblTargetSystemClass.Text &= " (Low Security Space)"
                    Case 9
                        lblTargetSystemClass.Text &= " (Null Security Space)"
                End Select
                lblMaxJumpableMass.Text = CLng(wh.MaxJumpableMass).ToString("N0") & " kg"
                lblMaxTotalMass.Text = CLng(wh.MaxMassCapacity).ToString("N0") & " kg"
                lblStabilityWindow.Text = (CDbl(wh.MaxStabilityWindow) / 60).ToString("N0") & " hours"
            Else
                lblTargetSystemClass.Text = "n/a (Return wormhole)"
                lblMaxJumpableMass.Text = "n/a"
                lblMaxTotalMass.Text = "n/a"
                lblStabilityWindow.Text = "n/a"
            End If
        End If
    End Sub

    Private Sub cboWHSystem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles cboWHSystem.SelectedIndexChanged
        ' Update the WH System Details
        If VoidData.WormholeSystems.ContainsKey(cboWHSystem.SelectedItem.ToString) = True Then
            Dim wh As WormholeSystem = VoidData.WormholeSystems(cboWHSystem.SelectedItem.ToString)
            If wh.WEffect <> "" Then
                Dim modName As String
                If wh.WEffect = "Red Giant" Then
                    modName = wh.WEffect & " Beacon Class " & wh.WClass
                Else
                    modName = wh.WEffect & " Effect Beacon Class " & wh.WClass
                End If
                'Dim SSun As EveHQ.Core.EveItem = EveHQ.StaticData.Types(EveHQ.StaticData.TypeNames(modName))
                lblAnomalyName.Text = wh.WEffect
                ' Establish the effects
                Dim effectList As New SortedList(Of String, Double)
                Dim sysEffects As WormholeEffect = VoidData.WormholeEffects(modName)
                For Each att As String In sysEffects.Attributes.Keys
                    If _whEffects.ContainsKey(att) = True Then
                        effectList.Add(_whEffects(att), sysEffects.Attributes(att))
                    End If
                Next
                lvwEffects.BeginUpdate()
                lvwEffects.Items.Clear()
                For Each effect As String In effectList.Keys
                    Dim newEffect As New ListViewItem
                    newEffect.Text = effect
                    Dim value As Double = CDbl(effectList(effect))
                    If value < 5 And value > -5 Then
                        If value < 1 Or effect.EndsWith("Penalty") Then
                            newEffect.ForeColor = Drawing.Color.Red
                        Else
                            newEffect.ForeColor = Drawing.Color.LimeGreen
                        End If
                        newEffect.SubItems.Add(effectList(effect).ToString("N2") & " x")
                    Else
                        If value < 0 Or effect.EndsWith("Penalty") Then
                            newEffect.ForeColor = Drawing.Color.Red
                        Else
                            newEffect.ForeColor = Drawing.Color.LimeGreen
                        End If
                        newEffect.SubItems.Add(effectList(effect).ToString("N2") & " %")
                    End If
                    lvwEffects.Items.Add(newEffect)
                Next
                lvwEffects.EndUpdate()
            Else
                lblAnomalyName.Text = "<None>"
                lvwEffects.Items.Clear()
            End If
            lblSystemClass.Text = wh.WClass
        End If
    End Sub
End Class