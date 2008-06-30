' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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

Public Class frmDoomsday

    Private cShipType As Ship
    Private DDLevel As Integer = 1

    Public Property ShipType() As Ship
        Get
            Return cShipType
        End Get
        Set(ByVal value As Ship)
            cShipType = value
            ' Trigger listview update, then display form
            Call Me.DisplayDoomsdayData()
        End Set
    End Property

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub DisplayDoomsdayData()
        ' Set variables
        Dim newDD As ListViewItem
        Dim damage As Double = 0
        Dim effShield As Double = 0
        Dim effArmor As Double = 0
        Dim effStructure As Double = 0
        Dim effTotal As Double = 0
        Dim residual As Double = 0

        ' Clear the list
        lvwDoomsday.BeginUpdate()
        lvwDoomsday.Items.Clear()

        ' Set damage
        damage = 46875 * (1 + (0.1 * DDLevel))

        ' EM Resists
        effShield = cShipType.ShieldCapacity * (100 / (100 - cShipType.ShieldEMResist))
        effArmor = cShipType.ShieldCapacity * (100 / (100 - cShipType.ArmorEMResist))
        effStructure = cShipType.ShieldCapacity * (100 / (100 - cShipType.StructureEMResist))
        effTotal = effShield + effArmor + effStructure
        residual = effTotal - damage
        newDD = New ListViewItem
        newDD.Text = "Amarr"
        newDD.SubItems.Add("EM")
        newDD.SubItems.Add(FormatNumber(damage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effShield, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effArmor, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effStructure, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(residual, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        If residual < 0 Then
            newDD.BackColor = Drawing.Color.LightSalmon
        Else
            newDD.BackColor = Drawing.Color.LightGreen
        End If
        lvwDoomsday.Items.Add(newDD)

        ' Explosive Resists
        effShield = cShipType.ShieldCapacity * (100 / (100 - cShipType.ShieldExResist))
        effArmor = cShipType.ShieldCapacity * (100 / (100 - cShipType.ArmorExResist))
        effStructure = cShipType.ShieldCapacity * (100 / (100 - cShipType.StructureExResist))
        effTotal = effShield + effArmor + effStructure
        residual = effTotal - damage
        newDD = New ListViewItem
        newDD.Text = "Minmatar"
        newDD.SubItems.Add("Explosive")
        newDD.SubItems.Add(FormatNumber(damage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effShield, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effArmor, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effStructure, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(residual, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        If residual < 0 Then
            newDD.BackColor = Drawing.Color.LightSalmon
        Else
            newDD.BackColor = Drawing.Color.LightGreen
        End If
        lvwDoomsday.Items.Add(newDD)

        ' Kinetic Resists
        effShield = cShipType.ShieldCapacity * (100 / (100 - cShipType.ShieldKiResist))
        effArmor = cShipType.ShieldCapacity * (100 / (100 - cShipType.ArmorKiResist))
        effStructure = cShipType.ShieldCapacity * (100 / (100 - cShipType.StructureKiResist))
        effTotal = effShield + effArmor + effStructure
        residual = effTotal - damage
        newDD = New ListViewItem
        newDD.Text = "Caldari"
        newDD.SubItems.Add("Kinetic")
        newDD.SubItems.Add(FormatNumber(damage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effShield, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effArmor, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effStructure, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(residual, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        If residual < 0 Then
            newDD.BackColor = Drawing.Color.LightSalmon
        Else
            newDD.BackColor = Drawing.Color.LightGreen
        End If
        lvwDoomsday.Items.Add(newDD)

        ' Thermal Resists
        effShield = cShipType.ShieldCapacity * (100 / (100 - cShipType.ShieldThResist))
        effArmor = cShipType.ShieldCapacity * (100 / (100 - cShipType.ArmorThResist))
        effStructure = cShipType.ShieldCapacity * (100 / (100 - cShipType.StructureThResist))
        effTotal = effShield + effArmor + effStructure
        residual = effTotal - damage
        newDD = New ListViewItem
        newDD.Text = "Gallente"
        newDD.SubItems.Add("Thermal")
        newDD.SubItems.Add(FormatNumber(damage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effShield, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effArmor, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effStructure, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(effTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        newDD.SubItems.Add(FormatNumber(residual, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        If residual < 0 Then
            newDD.BackColor = Drawing.Color.LightSalmon
        Else
            newDD.BackColor = Drawing.Color.LightGreen
        End If
        lvwDoomsday.Items.Add(newDD)
        lvwDoomsday.EndUpdate()

    End Sub

    Private Sub nudDoomsdayLevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDoomsdayLevel.ValueChanged
        DDLevel = CInt(nudDoomsdayLevel.Value)
        If cShipType IsNot Nothing Then
            Call Me.DisplayDoomsdayData()
        End If
    End Sub
End Class