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

Public Class frmGunnery

    Dim mCurrentSlot As String
    Dim mCurrentModule As ShipModule
    Dim mCurrentFit As Fitting
    Dim mCurrentPilot As FittingPilot
    Dim AmmoList As New ArrayList

    Public Property CurrentSlot() As String
        Get
            Return mCurrentSlot
        End Get
        Set(ByVal value As String)
            mCurrentSlot = value
            SetAmmoList()
            GenerateAmmoData()
        End Set
    End Property

    Public Property CurrentFit() As Fitting
        Get
            Return mCurrentFit
        End Get
        Set(ByVal value As Fitting)
            mCurrentFit = value.Clone
            mCurrentFit.BaseShip = mCurrentFit.BuildSubSystemEffects(mCurrentFit.BaseShip)
            mCurrentFit.UpdateBaseShipFromFitting()
        End Set
    End Property

    Public Property CurrentPilot() As FittingPilot
        Get
            Return mCurrentPilot
        End Get
        Set(ByVal value As FittingPilot)
            mCurrentPilot = value
        End Set
    End Property

    Private Sub SetAmmoList()
        Dim slotType As Integer = CInt(mCurrentSlot.Substring(0, 1))
        Dim slotNo As Integer = CInt(mCurrentSlot.Substring(2, 1))
        Select Case slotType
            Case SlotTypes.Rig
                mCurrentModule = mCurrentFit.BaseShip.RigSlot(slotNo)
            Case SlotTypes.Low
                mCurrentModule = mCurrentFit.BaseShip.LowSlot(slotNo)
            Case SlotTypes.Mid
                mCurrentModule = mCurrentFit.BaseShip.MidSlot(slotNo)
            Case SlotTypes.High
                mCurrentModule = mCurrentFit.BaseShip.HiSlot(slotNo)
        End Select
        ' Set the GroupBox for the weapon type
        gpStandardInfo.Text = mCurrentModule.Name
        ' Get the charge group and item data
        Dim chargeGroupData() As String
        AmmoList.Clear()
        For Each chargeGroup As String In Charges.ChargeGroups
            chargeGroupData = chargeGroup.Split("_".ToCharArray)
            If mCurrentModule.Charges.Contains(chargeGroupData(1)) = True Then
                If mCurrentModule.IsTurret Then
                    If mCurrentModule.ChargeSize = CInt(chargeGroupData(3)) Then
                        AmmoList.Add(chargeGroupData(2))
                    End If
                Else
                    AmmoList.Add(chargeGroupData(2))
                End If
            End If
        Next
        AmmoList.Sort()
    End Sub

    Private Sub GenerateAmmoData()
        Dim ammoShip As New Ship
        Dim ammoMod As New ShipModule
        Dim newMod As New ShipModule
        Dim newAmmo As ListViewItem
        ' Override the fitting rules
        CurrentFit.BaseShip.OverrideFittingRules = True
        ' Set the new fitting limits
        CurrentFit.BaseShip.Attributes(Attributes.Ship_HighSlots) = AmmoList.Count
        CurrentFit.BaseShip.HiSlots = AmmoList.Count
        CurrentFit.BaseShip.Attributes(Attributes.Ship_TurretHardpoints) = AmmoList.Count
        CurrentFit.BaseShip.TurretSlots = AmmoList.Count
        Dim slotCount As Integer = 0
        ' Load up the modules and charges
        For Each ammo As String In AmmoList
            slotCount += 1
            CurrentFit.BaseShip.HiSlot(slotCount) = mCurrentModule.Clone
            ' Convert ammo name into a ShipModule
            ammoMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(ammo)), ShipModule).Clone
            CurrentFit.BaseShip.HiSlot(slotCount).LoadedCharge = ammoMod
            CurrentFit.BaseShip.HiSlot(slotCount).SlotNo = slotCount
        Next
        ' Generate a fitting and get some info
        CurrentFit.ApplyFitting()
        ammoShip = CurrentFit.FittedShip
        ' Display the results
        lvGuns.BeginUpdate()
        lvGuns.Items.Clear()
        For ammo As Integer = 1 To AmmoList.Count
            newMod = ammoShip.HiSlot(ammo)
            newAmmo = New ListViewItem
            newAmmo.Text = newMod.LoadedCharge.Name
            newAmmo.SubItems.Add(newMod.CapUsage.ToString("N2"))
            newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_OptimalRange).ToString("N2"))
            If newMod.Attributes.ContainsKey(Attributes.Module_Falloff) Then
                newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_Falloff).ToString("N2"))
            Else
                newAmmo.SubItems.Add("0")
            End If
            If newMod.Attributes.ContainsKey(Attributes.Module_TrackingSpeed) Then
                newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_TrackingSpeed).ToString("N2"))
            Else
                newAmmo.SubItems.Add("0.00000")
            End If
            newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_EMDamage).ToString("N2"))
            newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_ExpDamage).ToString("N2"))
            newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_KinDamage).ToString("N2"))
            newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_ThermDamage).ToString("N2"))
            newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_VolleyDamage).ToString("N2"))
            newAmmo.SubItems.Add(newMod.Attributes(Attributes.Module_DPS).ToString("N2"))
            lvGuns.Items.Add(newAmmo)
        Next
        lvGuns.EndUpdate()
        ' Update the weapon standard information
        lblCPU.Text = "CPU: " & ammoShip.HiSlot(1).CPU.ToString("N2")
        lblPG.Text = "PG: " & ammoShip.HiSlot(1).PG.ToString("N2")
        Dim Dmg As Double = 0
        Dim ROF As Double = 0
        Select Case ammoShip.HiSlot(1).DatabaseGroup
            Case ShipModule.Group_EnergyTurrets
                Dmg = ammoShip.HiSlot(1).Attributes(Attributes.Module_EnergyDmgMod)
                ROF = ammoShip.HiSlot(1).Attributes(Attributes.Module_EnergyROF)
            Case ShipModule.Group_HybridTurrets
                Dmg = ammoShip.HiSlot(1).Attributes(Attributes.Module_HybridDmgMod)
                ROF = ammoShip.HiSlot(1).Attributes(Attributes.Module_HybridROF)
            Case ShipModule.Group_ProjectileTurrets
                Dmg = ammoShip.HiSlot(1).Attributes(Attributes.Module_ProjectileDmgMod)
                ROF = ammoShip.HiSlot(1).Attributes(Attributes.Module_ProjectileROF)
        End Select
        lblDmgMod.Text = "Damage Mod: " & Dmg.ToString("N2") & "x"
        lblROF.Text = "ROF: " & ROF.ToString("N2") & "s"
    End Sub

    Private Sub lvGuns_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvGuns.ColumnClick
        If lvGuns.Tag IsNot Nothing Then
            If CDbl(lvGuns.Tag.ToString) = e.Column Then
                Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
                lvGuns.Tag = -1
            Else
                Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
                lvGuns.Tag = e.Column
            End If
        Else
            Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvGuns.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvGuns.Sort()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        Dim buffer As New System.Text.StringBuilder
        buffer.AppendLine(gpStandardInfo.Text)
        For i As Integer = 0 To lvGuns.Columns.Count - 1
            buffer.Append(lvGuns.Columns(i).Text)
            buffer.Append(ControlChars.Tab)
        Next
        buffer.Append(ControlChars.CrLf)
        For i As Integer = 0 To lvGuns.Items.Count - 1
            For j As Integer = 0 To lvGuns.Items(0).SubItems.Count - 1
                If lvGuns.Items(i).SubItems(j) IsNot Nothing Then
                    buffer.Append(lvGuns.Items(i).SubItems(j).Text)
                    buffer.Append(ControlChars.Tab)
                End If
            Next
            buffer.Append(ControlChars.CrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub
End Class