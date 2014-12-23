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

Imports EveHQ.EveData
Imports EveHQ.Prism.Classes

Namespace Forms

    Public Class FrmQuickInventionChance

        Dim _formStartup As Boolean = True
        Dim _inventionChance As Double = 0

        Private Sub frmQuickInventionChance_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Set the startup flag
            _formStartup = True

            ' Load up the base chance combo box from static data
            Dim probabilityValues As New List(Of Double)
            For Each bp In StaticData.Blueprints.Values
                If probabilityValues.Contains(bp.InventionProbability) = False Then
                    If bp.InventionProbability > 0 Then
                        probabilityValues.Add(bp.InventionProbability)
                    End If
                End If
            Next
            probabilityValues.Sort()
            cboBaseChance.BeginUpdate()
            cboBaseChance.Items.Clear()
            For Each value As Double In probabilityValues
                cboBaseChance.Items.Add(value)
            Next
            cboBaseChance.EndUpdate()

            ' Load up decryptors from the static data
            cboDecryptor.BeginUpdate()
            cboDecryptor.Items.Clear()
            cboDecryptor.Items.Add("<None>")
            For Each decrypt As BPCalc.Decryptor In PlugInData.Decryptors.Values
                cboDecryptor.Items.Add(decrypt.Name & " (" & decrypt.ProbMod.ToString & "x, " & decrypt.MEMod.ToString & "ME, " & decrypt.PEMod.ToString & "TE, " & decrypt.RunMod.ToString & "r)")
            Next
            cboDecryptor.SelectedIndex = 0
            cboDecryptor.EndUpdate()

            ' Set the comboboxes to the first item in the list
            cboBaseChance.SelectedIndex = 0
            cboSkill1.SelectedIndex = 0
            cboSkill2.SelectedIndex = 0
            cboSkill3.SelectedIndex = 0
            cboDecryptor.SelectedIndex = 0

            _formStartup = False

            ' Do an initial calculation
            Call RecalculateInventionChance()

        End Sub

        Private Sub cboBaseChance_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboBaseChance.SelectedIndexChanged
            If _formStartup = False Then
                Call RecalculateInventionChance()
            End If
        End Sub

        Private Sub cboSkill1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboSkill1.SelectedIndexChanged
            If _formStartup = False Then
                Call RecalculateInventionChance()
            End If
        End Sub

        Private Sub cboSkill2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboSkill2.SelectedIndexChanged
            If _formStartup = False Then
                Call RecalculateInventionChance()
            End If
        End Sub

        Private Sub cboSkill3_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboSkill3.SelectedIndexChanged
            If _formStartup = False Then
                Call RecalculateInventionChance()
            End If
        End Sub

        Private Sub cboDecryptor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDecryptor.SelectedIndexChanged
            If _formStartup = False Then
                Call RecalculateInventionChance()
            End If
        End Sub

        Private Sub RecalculateInventionChance()

            ' Set up the base chance
            Dim baseChance As Double = CDbl(cboBaseChance.SelectedItem.ToString)

            Dim encSkillLevel As Integer = cboSkill1.SelectedIndex
            Dim dc1SkillLevel As Integer = cboSkill2.SelectedIndex
            Dim dc2SkillLevel As Integer = cboSkill3.SelectedIndex

            ' Determine the decryptor probability modifier
            Dim decryptorModifier As Double = 1
            Dim didx As Integer = cboDecryptor.SelectedItem.ToString.IndexOf("(", StringComparison.Ordinal)
            If didx > 0 Then
                Dim decryptorName As String = cboDecryptor.SelectedItem.ToString.Substring(0, didx - 1).Trim
                If PlugInData.Decryptors.ContainsKey(decryptorName) Then
                   decryptorModifier = CDbl(PlugInData.Decryptors(decryptorName).ProbMod)
              End If
            End If

            _inventionChance = Invention.CalculateInventionChance(baseChance, encSkillLevel, dc1SkillLevel, dc2SkillLevel, decryptorModifier)

            lblInventionChance.Text = "Invention Chance: " & _inventionChance.ToString("N2") & "%"

            Call RecalculateProbability()

        End Sub

        Private Sub RecalculateProbability()
            ' Calculate the probability of the successful vs total attempts
            Dim ic As Double = Math.Min(_inventionChance, 100) / 100.0
            Dim attempts As Integer = nudAttempts.Value
            Dim success As Integer = nudSuccess.Value

           'Calculate cumulative probability to get at least the specified number of successes
            Dim cp As Double = 0
            If success = 0 Or ic = 1 Then
                cp = 1
            ElseIf success >= attempts Then
                cp = Math.Pow(ic, attempts)
            Else
                BinomialDistribution(cp, attempts, success - 1, ic)
                cp = 1 - cp
            End If

            lblProbability.Text = "Probability: " & (cp * 100).ToString("N4") & "%"
        End Sub

        Private Function BinomialDistribution(ByRef cp As Double, ByVal n As Integer, ByVal k As Integer, ByVal p As Double) As Double
            Dim value As Double
            If k > 0 Then
                value = (n - k + 1) / k * p / (1 - p) * BinomialDistribution(cp, n, k - 1, p)
                cp += value
                Return value
            Else
                value = Math.Pow(1 - p, n)
                cp += value
                Return value
            End If
        End Function

        Private Sub nudAttempts_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudAttempts.ValueChanged
            nudSuccess.MaxValue = nudAttempts.Value
            Call RecalculateProbability()
        End Sub

        Private Sub nudSuccess_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudSuccess.ValueChanged
            Call RecalculateProbability()
        End Sub
    End Class
End NameSpace