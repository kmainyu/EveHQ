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

Public Class frmGunnery

    Dim mCurrentSlot As String
    Dim mCurrentModule As ShipModule
    Dim mCurrentFit As Fitting
    Dim mCurrentPilot As HQFPilot
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

    Public Property CurrentPilot() As HQFPilot
        Get
            Return mCurrentPilot
        End Get
        Set(ByVal value As HQFPilot)
            mCurrentpilot = value
        End Set
    End Property

    Private Sub SetAmmoList()
        Dim slotType As Integer = CInt(mCurrentSlot.Substring(0, 1))
        Dim slotNo As Integer = CInt(mCurrentSlot.Substring(2, 1))
        Select Case slotType
            Case 1 ' Rig
                mCurrentModule = mCurrentFit.BaseShip.RigSlot(slotNo)
            Case 2 ' Low
                mCurrentModule = mCurrentFit.BaseShip.LowSlot(slotNo)
            Case 4 ' Mid
                mCurrentModule = mCurrentFit.BaseShip.MidSlot(slotNo)
            Case 8 ' High
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
        CurrentFit.BaseShip.Attributes("14") = AmmoList.Count
        CurrentFit.BaseShip.HiSlots = AmmoList.Count
        CurrentFit.BaseShip.Attributes("102") = AmmoList.Count
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
            newAmmo.SubItems.Add(FormatNumber(newMod.CapUsage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("54"), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            If newMod.Attributes.ContainsKey("158") Then
                newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("158"), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            Else
                newAmmo.SubItems.Add("0")
            End If
            If newMod.Attributes.ContainsKey("160") Then
                newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("160"), 5, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            Else
                newAmmo.SubItems.Add("0.00000")
            End If
            newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("10051"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("10052"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("10053"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("10054"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("10018"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newAmmo.SubItems.Add(FormatNumber(newMod.Attributes("10019"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            lvGuns.Items.Add(newAmmo)
        Next
        lvGuns.EndUpdate()
        ' Update the weapon standard information
        lblCPU.Text = "CPU: " & FormatNumber(ammoShip.HiSlot(1).CPU, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblPG.Text = "PG: " & FormatNumber(ammoShip.HiSlot(1).PG, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Dim Dmg As Double = 0
        Dim ROF As Double = 0
        Select Case ammoShip.HiSlot(1).DatabaseGroup
            Case "53" ' Energy Turret 
                dmg = ammoShip.HiSlot(1).Attributes("10014")
                ROF = ammoShip.HiSlot(1).Attributes("10011")
            Case "74" ' Hybrid Turret
                dmg = ammoShip.HiSlot(1).Attributes("10015")
                ROF = ammoShip.HiSlot(1).Attributes("10012")
            Case "55" ' Projectile Turret
                dmg = ammoShip.HiSlot(1).Attributes("10016")
                ROF = ammoShip.HiSlot(1).Attributes("10013")
        End Select
        lblDmgMod.Text = "Damage Mod: " & FormatNumber(Dmg, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "x"
        lblROF.Text = "ROF: " & FormatNumber(ROF, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "s"
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