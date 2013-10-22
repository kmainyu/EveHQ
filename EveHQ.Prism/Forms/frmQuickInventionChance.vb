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
Imports EveHQ.Prism.Classes

Namespace Forms

    Public Class FrmQuickInventionChance

        Dim _formStartup As Boolean = True
        Dim _inventionChance As Double = 0

        Private Sub frmQuickInventionChance_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Set the startup flag
            _formStartup = True

            ' Set the comboboxes to the first item in the list
            cboBaseChance.SelectedIndex = 0
            cboSkill1.SelectedIndex = 0
            cboSkill2.SelectedIndex = 0
            cboSkill3.SelectedIndex = 0
            cboItemMetaLevel.SelectedIndex = 0
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

        Private Sub cboItemMetaLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboItemMetaLevel.SelectedIndexChanged
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

            ' Set up the variable
            Dim baseChance As Double = 0

            ' Determine base chance
            Select Case cboBaseChance.SelectedIndex
                Case 0
                    baseChance = 20
                Case 1
                    baseChance = 25
                Case 2
                    baseChance = 30
                Case 3
                    baseChance = 40
            End Select

            Dim encSkillLevel As Integer = cboSkill1.SelectedIndex
            Dim dc1SkillLevel As Integer = cboSkill2.SelectedIndex
            Dim dc2SkillLevel As Integer = cboSkill3.SelectedIndex
            Dim metaLevel As Integer = Math.Max(cboItemMetaLevel.SelectedIndex - 1, 0)

            Dim decryptorModifier As Double = 1
            Select Case cboDecryptor.SelectedIndex
                Case 0
                    decryptorModifier = 1
                Case 1
                    decryptorModifier = 0.6
                Case 2
                    decryptorModifier = 0.9
                Case 3
                    decryptorModifier = 1
                Case 4
                    decryptorModifier = 1.1
                Case 5
                    decryptorModifier = 1.2
                Case 6
                    decryptorModifier = 1.5
                Case 7
                    decryptorModifier = 1.8
                Case 8
                    decryptorModifier = 1.9
            End Select

            _inventionChance = Invention.CalculateInventionChance(baseChance, encSkillLevel, dc1SkillLevel, dc2SkillLevel, metaLevel, decryptorModifier)

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