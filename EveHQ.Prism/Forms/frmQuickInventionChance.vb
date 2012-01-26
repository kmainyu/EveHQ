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

Public Class frmQuickInventionChance

    Dim FormStartup As Boolean = True
    Dim InventionChance As Double = 0

    Private Sub frmQuickInventionChance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the startup flag
        FormStartup = True

        ' Set the comboboxes to the first item in the list
        cboBaseChance.SelectedIndex = 0
        cboSkill1.SelectedIndex = 0
        cboSkill2.SelectedIndex = 0
        cboSkill3.SelectedIndex = 0
        cboItemMetaLevel.SelectedIndex = 0
        cboDecryptor.SelectedIndex = 0

        FormStartup = False

        ' Do an initial calculation
        Call Me.RecalculateInventionChance()

    End Sub

    Private Sub cboBaseChance_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBaseChance.SelectedIndexChanged
        If FormStartup = False Then
            Call Me.RecalculateInventionChance()
        End If
    End Sub

    Private Sub cboSkill1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSkill1.SelectedIndexChanged
        If FormStartup = False Then
            Call Me.RecalculateInventionChance()
        End If
    End Sub

    Private Sub cboSkill2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSkill2.SelectedIndexChanged
        If FormStartup = False Then
            Call Me.RecalculateInventionChance()
        End If
    End Sub

    Private Sub cboSkill3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSkill3.SelectedIndexChanged
        If FormStartup = False Then
            Call Me.RecalculateInventionChance()
        End If
    End Sub

    Private Sub cboItemMetaLevel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboItemMetaLevel.SelectedIndexChanged
        If FormStartup = False Then
            Call Me.RecalculateInventionChance()
        End If
    End Sub

    Private Sub cboDecryptor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDecryptor.SelectedIndexChanged
        If FormStartup = False Then
            Call Me.RecalculateInventionChance()
        End If
    End Sub

    Private Sub RecalculateInventionChance()

        ' Set up the variable
        Dim BaseChance As Double = 0

        ' Determine base chance
        Select Case cboBaseChance.SelectedIndex
            Case 0
                BaseChance = 20
            Case 1
                BaseChance = 25
            Case 2
                BaseChance = 30
            Case 3
                BaseChance = 40
        End Select

        Dim EncSkillLevel As Integer = cboSkill1.SelectedIndex
        Dim DC1SkillLevel As Integer = cboSkill2.SelectedIndex
        Dim DC2SkillLevel As Integer = cboSkill3.SelectedIndex
        Dim MetaLevel As Integer = Math.Max(cboItemMetaLevel.SelectedIndex - 1, 0)

        Dim DecryptorModifier As Double = 1
        Select Case cboDecryptor.SelectedIndex
            Case 0
                DecryptorModifier = 1
            Case 1
                DecryptorModifier = 0.6
            Case 2
                DecryptorModifier = 1
            Case 3
                DecryptorModifier = 1.1
            Case 4
                DecryptorModifier = 1.2
            Case 5
                DecryptorModifier = 1.8
        End Select

        InventionChance = Invention.CalculateInventionChance(BaseChance, EncSkillLevel, DC1SkillLevel, DC2SkillLevel, MetaLevel, DecryptorModifier)

        Me.lblInventionChance.Text = "Invention Chance: " & InventionChance.ToString("N2") & "%"

        Call Me.RecalculateProbability()

    End Sub

    Private Sub RecalculateProbability()
        ' Calculate the probability of the successful vs total attempts
        Dim IC As Double = Math.Min(InventionChance, 100)
        Dim Attempts As Integer = nudAttempts.Value
        Dim Success As Integer = nudSuccess.Value
        Dim PS As Double = Math.Pow((IC / 100), Success)
        Dim PF As Double = Math.Pow(1 - (IC / 100), Attempts - Success)
        Dim FP As Double = Factorial(Attempts) / Factorial(Success) / Factorial(Attempts - Success)
        Dim TP As Double = FP * PS * PF * 100
        Me.lblProbability.Text = "Probability: " & TP.ToString("N4") & "%"
    End Sub

    Private Function Factorial(ByVal n As Integer) As Double
        Dim sum As Double = 1
        If n >= 1 Then
            For i As Integer = 1 To n
                sum *= i
            Next
            Return sum
        Else
            Return 1
        End If
    End Function

    Private Sub nudAttempts_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudAttempts.ValueChanged
        nudSuccess.MaxValue = nudAttempts.Value
        Call Me.RecalculateProbability()
    End Sub

    Private Sub nudSuccess_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSuccess.ValueChanged
        Call Me.RecalculateProbability()
    End Sub
End Class