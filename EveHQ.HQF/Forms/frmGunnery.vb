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
Imports System.Text
Imports System.Windows.Forms
Imports EveHQ.Core

Namespace Forms

    Public Class FrmGunnery

        Dim _currentSlot As String
        Dim _currentModule As ShipModule
        Dim _currentFit As Fitting
        Dim _currentPilot As FittingPilot
        ReadOnly _ammoList As New ArrayList

        Public Property CurrentSlot() As String
            Get
                Return _currentSlot
            End Get
            Set(ByVal value As String)
                _currentSlot = value
                SetAmmoList()
                GenerateAmmoData()
            End Set
        End Property

        Public Property CurrentFit() As Fitting
            Get
                Return _currentFit
            End Get
            Set(ByVal value As Fitting)
                _currentFit = value.Clone
                _currentFit.BaseShip = _currentFit.BuildSubSystemEffects(_currentFit.BaseShip)
                _currentFit.UpdateBaseShipFromFitting()
            End Set
        End Property

        Public Property CurrentPilot() As FittingPilot
            Get
                Return _currentPilot
            End Get
            Set(ByVal value As FittingPilot)
                _currentPilot = value
            End Set
        End Property

        Private Sub SetAmmoList()
            Dim slotType As Integer = CInt(_currentSlot.Substring(0, 1))
            Dim slotNo As Integer = CInt(_currentSlot.Substring(2, 1))
            Select Case slotType
                Case SlotTypes.Rig
                    _currentModule = _currentFit.BaseShip.RigSlot(slotNo)
                Case SlotTypes.Low
                    _currentModule = _currentFit.BaseShip.LowSlot(slotNo)
                Case SlotTypes.Mid
                    _currentModule = _currentFit.BaseShip.MidSlot(slotNo)
                Case SlotTypes.High
                    _currentModule = _currentFit.BaseShip.HiSlot(slotNo)
            End Select
            ' Set the GroupBox for the weapon type
            gpStandardInfo.Text = _currentModule.Name
            ' Get the charge group and item data
            Dim chargeGroupData() As String
            _ammoList.Clear()
            For Each chargeGroup As String In Charges.ChargeGroups.Values
                chargeGroupData = chargeGroup.Split("_".ToCharArray)
                If _currentModule.Charges.Contains(CInt(chargeGroupData(1))) = True Then
                    If _currentModule.IsTurret Then
                        If _currentModule.ChargeSize = CInt(chargeGroupData(3)) Then
                            _ammoList.Add(chargeGroupData(2))
                        End If
                    Else
                        _ammoList.Add(chargeGroupData(2))
                    End If
                End If
            Next
            _ammoList.Sort()
        End Sub

        Private Sub GenerateAmmoData()
            Dim ammoShip As Ship
            Dim ammoMod As ShipModule
            Dim newMod As ShipModule
            Dim newAmmo As ListViewItem
            ' Override the fitting rules
            CurrentFit.BaseShip.OverrideFittingRules = True
            ' Set the new fitting limits
            CurrentFit.BaseShip.Attributes(AttributeEnum.ShipHighSlots) = _ammoList.Count
            CurrentFit.BaseShip.HiSlots = _ammoList.Count
            CurrentFit.BaseShip.Attributes(AttributeEnum.ShipTurretHardpoints) = _ammoList.Count
            CurrentFit.BaseShip.TurretSlots = _ammoList.Count
            Dim slotCount As Integer = 0
            ' Load up the modules and charges
            For Each ammo As String In _ammoList
                slotCount += 1
                CurrentFit.BaseShip.HiSlot(slotCount) = _currentModule.Clone
                ' Convert ammo name into a ShipModule
                ammoMod = ModuleLists.ModuleList(ModuleLists.ModuleListName(ammo)).Clone
                CurrentFit.BaseShip.HiSlot(slotCount).LoadedCharge = ammoMod
                CurrentFit.BaseShip.HiSlot(slotCount).SlotNo = slotCount
            Next
            ' Generate a fitting and get some info
            CurrentFit.ApplyFitting()
            ammoShip = CurrentFit.FittedShip
            ' Display the results
            lvGuns.BeginUpdate()
            lvGuns.Items.Clear()
            For ammo As Integer = 1 To _ammoList.Count
                newMod = ammoShip.HiSlot(ammo)
                newAmmo = New ListViewItem
                newAmmo.Text = newMod.LoadedCharge.Name
                newAmmo.SubItems.Add(newMod.CapUsage.ToString("N2"))
                If newMod.Attributes.ContainsKey(AttributeEnum.ModuleFalloff) Then
                    newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleOptimalRange).ToString("N2"))
                Else
                    newAmmo.SubItems.Add("0")
                End If
                If newMod.Attributes.ContainsKey(AttributeEnum.ModuleFalloff) Then
                    newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleFalloff).ToString("N2"))
                Else
                    newAmmo.SubItems.Add("0")
                End If
                If newMod.Attributes.ContainsKey(AttributeEnum.ModuleTrackingSpeed) Then
                    newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleTrackingSpeed).ToString("N2"))
                Else
                    newAmmo.SubItems.Add("0.00000")
                End If
                newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleEMDamage).ToString("N2"))
                newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleExpDamage).ToString("N2"))
                newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleKinDamage).ToString("N2"))
                newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleThermDamage).ToString("N2"))
                newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleVolleyDamage).ToString("N2"))
                newAmmo.SubItems.Add(newMod.Attributes(AttributeEnum.ModuleDPS).ToString("N2"))
                lvGuns.Items.Add(newAmmo)
            Next
            lvGuns.EndUpdate()
            ' Update the weapon standard information
            lblCPU.Text = "CPU: " & ammoShip.HiSlot(1).CPU.ToString("N2")
            lblPG.Text = "PG: " & ammoShip.HiSlot(1).PG.ToString("N2")
            Dim dmg As Double = 0
            Dim rof As Double = 0
            Select Case ammoShip.HiSlot(1).DatabaseGroup
                Case ModuleEnum.GroupEnergyTurrets
                    dmg = ammoShip.HiSlot(1).Attributes(AttributeEnum.ModuleEnergyDmgMod)
                    rof = ammoShip.HiSlot(1).Attributes(AttributeEnum.ModuleEnergyROF)
                Case ModuleEnum.GroupHybridTurrets
                    dmg = ammoShip.HiSlot(1).Attributes(AttributeEnum.ModuleHybridDmgMod)
                    rof = ammoShip.HiSlot(1).Attributes(AttributeEnum.ModuleHybridROF)
                Case ModuleEnum.GroupProjectileTurrets
                    dmg = ammoShip.HiSlot(1).Attributes(AttributeEnum.ModuleProjectileDmgMod)
                    rof = ammoShip.HiSlot(1).Attributes(AttributeEnum.ModuleProjectileROF)
            End Select
            lblDmgMod.Text = "Damage Mod: " & dmg.ToString("N2") & "x"
            lblROF.Text = "ROF: " & rof.ToString("N2") & "s"
        End Sub

        Private Sub lvGuns_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) Handles lvGuns.ColumnClick
            If lvGuns.Tag IsNot Nothing Then
                If CDbl(lvGuns.Tag.ToString) = e.Column Then
                    lvGuns.ListViewItemSorter = New ListViewItemComparerText(e.Column, SortOrder.Ascending)
                    lvGuns.Tag = -1
                Else
                    lvGuns.ListViewItemSorter = New ListViewItemComparerText(e.Column, SortOrder.Descending)
                    lvGuns.Tag = e.Column
                End If
            Else
                lvGuns.ListViewItemSorter = New ListViewItemComparerText(e.Column, SortOrder.Descending)
                lvGuns.Tag = e.Column
            End If
            ' Call the sort method to manually sort.
            lvGuns.Sort()
        End Sub

        Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToolStripMenuItem.Click
            Dim buffer As New StringBuilder
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
End NameSpace