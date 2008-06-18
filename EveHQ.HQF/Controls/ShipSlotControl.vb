﻿' ========================================================================
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
Imports System.Drawing
Imports System.Text

Public Class ShipSlotControl
    Dim UpdateAll As Boolean = False
    Dim rigGroups As New ArrayList

#Region "Property Variables"

    Private cShipFit As String
    Private currentShip As Ship
    Private fittedShip As Ship
    Private currentFit As ArrayList
    Private currentInfo As ShipInfoControl
    Private cUpdateAllSlots As Boolean

#End Region

#Region "Properties"

    Public Property ShipFit() As String
        Get
            Return cShipFit
        End Get
        Set(ByVal value As String)
            cShipFit = value
            Dim fittingSep As Integer = cShipFit.IndexOf(", ")
            Dim shipName As String = cShipFit.Substring(0, fittingSep)
            Dim FittingName As String = cShipFit.Substring(fittingSep + 2)
            currentShip = CType(ShipLists.fittedShipList(cShipFit), Ship)
            currentFit = CType(Fittings.FittingList.Item(cShipFit), ArrayList)
        End Set
    End Property

    Public Property ShipInfo() As ShipInfoControl
        Get
            Return currentInfo
        End Get
        Set(ByVal value As ShipInfoControl)
            currentInfo = value
        End Set
    End Property

    Public Property ShipType() As Ship
        Get
            Return currentShip
        End Get
        Set(ByVal value As Ship)
            currentShip = value
        End Set
    End Property

    Public Property ShipFitted() As Ship
        Get
            Return fittedShip
        End Get
        Set(ByVal value As Ship)
            fittedShip = value
            If UpdateAllSlots = True Then
                Call Me.UpdateAllSlotLocations()
            End If
        End Set
    End Property

    Public Property UpdateAllSlots() As Boolean
        Get
            Return cUpdateAllSlots
        End Get
        Set(ByVal value As Boolean)
            cUpdateAllSlots = value
        End Set
    End Property

#End Region

#Region "Update Routines"

    Public Sub UpdateEverything()
        ' Update the slot layout
        currentShip = CType(ShipLists.fittedShipList(cShipFit), Ship)
        UpdateAll = True
        lvwCargoBay.BeginUpdate()
        lvwDroneBay.BeginUpdate()
        lvwSlots.BeginUpdate()
        Me.ClearShipSlots()
        Me.ClearDroneBay()
        Me.ClearCargoBay()
        Me.UpdateSlotLayout()
        Me.UpdateShipDataFromFittingList()
        lvwCargoBay.EndUpdate()
        lvwDroneBay.EndUpdate()
        lvwSlots.EndUpdate()
        UpdateAll = False
        currentInfo.ShipType = currentShip
        Me.UpdateAllSlotLocations()
        Me.UpdateShipDetails()
        Me.RedrawDroneBay()
        Me.RedrawCargoBay()
    End Sub
    Private Sub UpdateShipDetails()
        If UpdateAll = False Then
            Call UpdateSlotNumbers()
            Call UpdatePrices()
            Call UpdateFittingListFromShipData()
        End If
    End Sub
    Private Sub UpdateSlotLayout()
        lvwSlots.BeginUpdate()
        lvwSlots.Items.Clear()
        ' Produce high slots
        For slot As Integer = 1 To currentShip.HiSlots
            Dim newSlot As New ListViewItem
            newSlot.Name = "8_" & slot
            newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
            newSlot.ForeColor = Color.Black
            newSlot.Group = lvwSlots.Groups.Item("lvwgHighSlots")
            If currentShip.HiSlot(slot) IsNot Nothing Then
                newSlot.Text = currentShip.HiSlot(slot).Name
                If currentShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    newSlot.SubItems.Add(currentShip.HiSlot(slot).LoadedCharge.Name)
                Else
                    newSlot.SubItems.Add("")
                End If
                newSlot.SubItems.Add(currentShip.HiSlot(slot).CPU.ToString)
                newSlot.SubItems.Add(currentShip.HiSlot(slot).PG.ToString)
                newSlot.SubItems.Add(currentShip.HiSlot(slot).CapUsage.ToString)
                newSlot.SubItems.Add(currentShip.HiSlot(slot).ActivationTime.ToString)
                If currentShip.HiSlot(slot).MarketPrice > 0 Then
                    newSlot.SubItems.Add(FormatNumber(currentShip.HiSlot(slot).MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                Else
                    newSlot.SubItems.Add(FormatNumber(currentShip.HiSlot(slot).BasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " *")
                End If
            Else
                newSlot.Text = "<Empty>"
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
            End If
            lvwSlots.Items.Add(newSlot)
        Next
        For slot As Integer = 1 To currentShip.MidSlots
            Dim newSlot As New ListViewItem
            newSlot.Name = "4_" & slot
            newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
            newSlot.ForeColor = Color.Black
            newSlot.Group = lvwSlots.Groups.Item("lvwgMidSlots")
            If currentShip.MidSlot(slot) IsNot Nothing Then
                newSlot.Text = currentShip.MidSlot(slot).Name
                If currentShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    newSlot.SubItems.Add(currentShip.MidSlot(slot).LoadedCharge.Name)
                Else
                    newSlot.SubItems.Add("")
                End If
                newSlot.SubItems.Add(currentShip.MidSlot(slot).CPU.ToString)
                newSlot.SubItems.Add(currentShip.MidSlot(slot).PG.ToString)
                newSlot.SubItems.Add(currentShip.MidSlot(slot).CapUsage.ToString)
                newSlot.SubItems.Add(currentShip.MidSlot(slot).ActivationTime.ToString)
                If currentShip.MidSlot(slot).MarketPrice > 0 Then
                    newSlot.SubItems.Add(FormatNumber(currentShip.MidSlot(slot).MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                Else
                    newSlot.SubItems.Add(FormatNumber(currentShip.MidSlot(slot).BasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " *")
                End If

            Else
                newSlot.Text = "<Empty>"
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
            End If
            lvwSlots.Items.Add(newSlot)
        Next
        For slot As Integer = 1 To currentShip.LowSlots
            Dim newSlot As New ListViewItem
            newSlot.Name = "2_" & slot
            newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
            newSlot.ForeColor = Color.Black
            newSlot.Group = lvwSlots.Groups.Item("lvwgLowSlots")
            If currentShip.LowSlot(slot) IsNot Nothing Then
                newSlot.Text = currentShip.LowSlot(slot).Name
                If currentShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    newSlot.SubItems.Add(currentShip.LowSlot(slot).LoadedCharge.Name)
                Else
                    newSlot.SubItems.Add("")
                End If
                newSlot.SubItems.Add(currentShip.LowSlot(slot).CPU.ToString)
                newSlot.SubItems.Add(currentShip.LowSlot(slot).PG.ToString)
                newSlot.SubItems.Add(currentShip.LowSlot(slot).CapUsage.ToString)
                newSlot.SubItems.Add(currentShip.LowSlot(slot).ActivationTime.ToString)
                If currentShip.LowSlot(slot).MarketPrice > 0 Then
                    newSlot.SubItems.Add(FormatNumber(currentShip.LowSlot(slot).MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                Else
                    newSlot.SubItems.Add(FormatNumber(currentShip.LowSlot(slot).BasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " *")
                End If
            Else
                newSlot.Text = "<Empty>"
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
            End If
            lvwSlots.Items.Add(newSlot)
        Next
        For slot As Integer = 1 To currentShip.RigSlots
            Dim newSlot As New ListViewItem
            newSlot.Name = "1_" & slot
            newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
            newSlot.ForeColor = Color.Black
            newSlot.Group = lvwSlots.Groups.Item("lvwgRigSlots")
            If currentShip.RigSlot(slot) IsNot Nothing Then
                newSlot.Text = currentShip.RigSlot(slot).Name
                If currentShip.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    newSlot.SubItems.Add(currentShip.RigSlot(slot).LoadedCharge.Name)
                Else
                    newSlot.SubItems.Add("")
                End If
                newSlot.SubItems.Add(currentShip.RigSlot(slot).CPU.ToString)
                newSlot.SubItems.Add(currentShip.RigSlot(slot).PG.ToString)
                newSlot.SubItems.Add(currentShip.RigSlot(slot).CapUsage.ToString)
                newSlot.SubItems.Add(currentShip.RigSlot(slot).ActivationTime.ToString)
                If currentShip.RigSlot(slot).MarketPrice > 0 Then
                    newSlot.SubItems.Add(FormatNumber(currentShip.RigSlot(slot).MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                Else
                    newSlot.SubItems.Add(FormatNumber(currentShip.RigSlot(slot).BasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " *")
                End If
            Else
                newSlot.Text = "<Empty>"
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
                newSlot.SubItems.Add("")
            End If
            lvwSlots.Items.Add(newSlot)
        Next
        lvwSlots.EndUpdate()
        Call UpdateSlotNumbers()
        Call UpdatePrices()
    End Sub
    Private Sub UpdateSlotNumbers()
        lblHighSlots.Text = "Hi Slots: " & currentShip.HiSlots_Used & "/" & currentShip.HiSlots
        lblMidSlots.Text = "Mid Slots: " & currentShip.MidSlots_Used & "/" & currentShip.MidSlots
        lblLowSlots.Text = "Low Slots: " & currentShip.LowSlots_Used & "/" & currentShip.LowSlots
        lblRigSlots.Text = "Rig Slots: " & currentShip.RigSlots_Used & "/" & currentShip.RigSlots
        lblLauncherSlots.Text = "Launcher Slots: " & currentShip.LauncherSlots_Used & "/" & currentShip.LauncherSlots
        lblTurretSlots.Text = "Turret Slots: " & currentShip.TurretSlots_Used & "/" & currentShip.TurretSlots
    End Sub
    Private Sub UpdatePrices()
        currentShip.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(currentShip.ID)
        lblShipBasePrice.Text = "Ship Base Price: " & FormatNumber(currentShip.BasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblShipMarketPrice.Text = "Ship Market Price: " & FormatNumber(currentShip.MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblFittingBasePrice.Text = "Fitting Base Price: " & FormatNumber(currentShip.BasePrice + currentShip.FittingBasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblFittingMarketPrice.Text = "Fitting Market Price: " & FormatNumber(currentShip.MarketPrice + currentShip.FittingMarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
    End Sub
    Private Sub UpdateAllSlotLocations()
        For slot As Integer = 1 To fittedShip.HiSlots
            If fittedShip.HiSlot(slot) IsNot Nothing Then
                UpdateSlotLocation(fittedShip.HiSlot(slot), slot)
            End If
        Next
        For slot As Integer = 1 To fittedShip.MidSlots
            If fittedShip.MidSlot(slot) IsNot Nothing Then
                UpdateSlotLocation(fittedShip.MidSlot(slot), slot)
            End If
        Next
        For slot As Integer = 1 To fittedShip.LowSlots
            If fittedShip.LowSlot(slot) IsNot Nothing Then
                UpdateSlotLocation(fittedShip.LowSlot(slot), slot)
            End If
        Next
        For slot As Integer = 1 To fittedShip.RigSlots
            If fittedShip.RigSlot(slot) IsNot Nothing Then
                UpdateSlotLocation(fittedShip.RigSlot(slot), slot)
            End If
        Next
    End Sub
    Private Sub UpdateSlotLocation(ByVal oldMod As ShipModule, ByVal slotNo As Integer)
        Dim shipMod As New ShipModule
        Select Case oldMod.Slot
            Case 1 ' Rig
                shipMod = fittedShip.RigSlot(slotNo)
            Case 2 ' Low
                shipMod = fittedShip.LowSlot(slotNo)
            Case 4 ' Mid
                shipMod = fittedShip.MidSlot(slotNo)
            Case 8 ' High
                shipMod = fittedShip.HiSlot(slotNo)
        End Select
        shipMod.ModuleState = oldMod.ModuleState
        Dim slotName As ListViewItem = lvwSlots.Items(shipMod.Slot & "_" & slotNo)
        slotName.ImageIndex = shipMod.ModuleState
        slotName.Text = shipMod.Name
        If shipMod.LoadedCharge IsNot Nothing Then
            slotName.SubItems(1).Text = shipMod.LoadedCharge.Name
        Else
            slotName.SubItems(1).Text = ""
        End If
        slotName.SubItems(2).Text = shipMod.CPU.ToString
        slotName.SubItems(3).Text = shipMod.PG.ToString
        If shipMod.ModuleState = ModuleStates.Active Then
            slotName.SubItems(4).Text = shipMod.CapUsage.ToString
        Else
            slotName.SubItems(4).Text = "0"
        End If
        slotName.SubItems(5).Text = shipMod.ActivationTime.ToString
        shipMod.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(shipMod.ID)
        slotName.SubItems(6).Text = FormatNumber(shipMod.MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Call UpdateShipDetails()
    End Sub
    Private Sub UpdateShipDataFromFittingList()
        Dim currentFitList As ArrayList = CType(currentFit.Clone, ArrayList)
        For Each shipMod As String In currentFitList
            If shipMod IsNot Nothing Then
                ' Check for installed charges
                Dim modData() As String = shipMod.Split(",".ToCharArray)
                If ModuleLists.moduleListName.ContainsKey(modData(0)) = True Then
                    Dim modID As String = ModuleLists.moduleListName(modData(0).Trim).ToString
                    Dim sMod As ShipModule = CType(ModuleLists.moduleList(modID), ShipModule).Clone
                    If modData.GetUpperBound(0) > 0 Then
                        ' Check if a charge (will be a valid item)
                        If ModuleLists.moduleListName.Contains(modData(1).Trim) = True Then
                            Dim chgID As String = ModuleLists.moduleListName(modData(1).Trim).ToString
                            sMod.LoadedCharge = CType(ModuleLists.moduleList(chgID), ShipModule).Clone
                        End If
                    End If
                    ' Check if module is nothing
                    If sMod IsNot Nothing Then
                        ' Check if module is a drone
                        If sMod.IsDrone = True Then
                            Dim active As Boolean = False
                            If modData(1).EndsWith("a") = True Then
                                active = True
                            End If
                            Call Me.AddDrone(sMod, CInt(modData(1).Substring(0, Len(modData(1)) - 1)), active)
                        Else
                            ' Check if module is a charge
                            If sMod.IsCharge = True Then
                                Call Me.AddItem(sMod, CInt(modData(1)))
                            Else
                                ' Must be a proper module then!
                                Call AddModule(sMod)
                            End If
                        End If
                    Else
                        ' Unrecognised module
                        MessageBox.Show("Ship Module is unrecognised.", "Add Ship Module Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    currentFit.Remove(modData(0))
                End If
            End If
        Next
    End Sub
    Private Sub UpdateFittingListFromShipData()
        currentFit.Clear()
        For Each shipMod As ListViewItem In lvwSlots.Items
            If shipMod.Text <> "<Empty>" Then
                If shipMod.SubItems(1).Text <> "" Then
                    currentFit.Add(shipMod.Text & ", " & shipMod.SubItems(1).Text)
                Else
                    currentFit.Add(shipMod.Text)
                End If
            End If
        Next
        For Each DBI As DroneBayItem In currentShip.DroneBayItems.Values
            If DBI.IsActive = True Then
                currentFit.Add(DBI.DroneType.Name & ", " & DBI.Quantity & "a")
            Else
                currentFit.Add(DBI.DroneType.Name & ", " & DBI.Quantity & "i")
            End If
        Next
        For Each CBI As CargoBayItem In currentShip.CargoBayItems.Values
            currentFit.Add(CBI.ItemType.Name & ", " & CBI.Quantity)
        Next
    End Sub

#End Region

#Region "Clearing routines"
    Private Sub ClearShipSlots()
        For slot As Integer = 1 To currentShip.HiSlots
            currentShip.HiSlot(slot) = Nothing
        Next
        For slot As Integer = 1 To currentShip.MidSlots
            currentShip.MidSlot(slot) = Nothing
        Next
        For slot As Integer = 1 To currentShip.LowSlots
            currentShip.LowSlot(slot) = Nothing
        Next
        For slot As Integer = 1 To currentShip.RigSlots
            currentShip.RigSlot(slot) = Nothing
        Next
        'currentShip.HiSlots_Used = 0
        'currentShip.MidSlots_Used = 0
        'currentShip.LowSlots_Used = 0
        'currentShip.RigSlots_Used = 0
        'currentShip.LauncherSlots_Used = 0
        'currentShip.TurretSlots_Used = 0
        'currentShip.FittingBasePrice = 0
        'currentShip.FittingMarketPrice = 0
    End Sub
    Private Sub ClearDroneBay()
        currentShip.DroneBayItems.Clear()
        currentShip.DroneBay_Used = 0
        'Me.RedrawDroneBay()
    End Sub
    Private Sub ClearCargoBay()
        currentShip.CargoBayItems.Clear()
        currentShip.CargoBay_Used = 0
        'Me.RedrawCargoBay()
    End Sub
#End Region

#Region "Adding Mods/Drones/Items"
    Public Sub AddModule(ByVal shipMod As ShipModule)
        ' Check slot availability
        If IsSlotAvailable(shipMod) = True Then
            ' Add Module to the next slot
            Dim slotNo As Integer = AddModuleInNextSlot(CType(shipMod.Clone, ShipModule))
            If UpdateAll = False Then
                currentInfo.ShipType = currentShip
                Call UpdateSlotLocation(shipMod, slotNo)
            End If
        End If
    End Sub
    Public Sub AddDrone(ByVal Drone As ShipModule, ByVal Qty As Integer, ByVal Active As Boolean)
        ' Set grouping flag
        Dim grouped As Boolean = False
        ' See if there is sufficient space
        Dim vol As Double = Drone.Volume
        If currentShip.DroneBay - currentShip.DroneBay_Used >= vol Then
            ' Scan through existing items and see if we can group this new one
            For Each droneGroup As DroneBayItem In currentShip.DroneBayItems.Values
                If Drone.Name = droneGroup.DroneType.Name And Active = droneGroup.IsActive Then
                    ' Add to existing drone group
                    droneGroup.Quantity += Qty
                    grouped = True
                    Exit For
                End If
            Next
            ' Put the drone into the drone bay if not grouped
            If grouped = False Then
                Dim DBI As New DroneBayItem
                DBI.DroneType = Drone
                DBI.Quantity = Qty
                DBI.IsActive = Active
                currentShip.DroneBayItems.Add(currentShip.DroneBayItems.Count, DBI)
            End If
            ' Update stuff
            If UpdateAll = False Then
                Call Me.RedrawDroneBay()
                Call UpdatePrices()
                Call UpdateFittingListFromShipData()
            End If
        Else
            MessageBox.Show("There is not enough space in the Drone Bay to hold 1 unit of " & Drone.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Public Sub AddItem(ByVal Item As ShipModule, ByVal Qty As Integer)
        ' Set grouping flag
        Dim grouped As Boolean = False
        ' See if there is sufficient space
        Dim vol As Double = Item.Volume
        If currentShip.CargoBay - currentShip.CargoBay_Used >= vol Then
            ' Scan through existing items and see if we can group this new one
            For Each itemGroup As CargoBayItem In currentShip.CargoBayItems.Values
                If Item.Name = itemGroup.ItemType.Name Then
                    ' Add to existing drone group
                    itemGroup.Quantity += Qty
                    grouped = True
                    Exit For
                End If
            Next
            ' Put the drone into the cargo bay if not grouped
            If grouped = False Then
                Dim CBI As New CargoBayItem
                CBI.ItemType = Item
                CBI.Quantity = Qty
                currentShip.CargoBayItems.Add(currentShip.CargoBayItems.Count, CBI)
            End If
            ' Update stuff
            If UpdateAll = False Then
                Call Me.RedrawCargoBay()
                Call UpdatePrices()
                Call UpdateFittingListFromShipData()
            End If
        Else
            MessageBox.Show("There is not enough space in the Cargo Bay to hold 1 unit of " & Item.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Function IsSlotAvailable(ByVal shipMod As ShipModule) As Boolean

        ' First, check slot layout
        Select Case shipMod.Slot
            Case 1 ' Rig
                If currentShip.RigSlots_Used = currentShip.RigSlots Then
                    MessageBox.Show("There are no available rig slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 2 ' Low
                If currentShip.LowSlots_Used = currentShip.LowSlots Then
                    MessageBox.Show("There are no available low slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 4 ' Mid
                If currentShip.MidSlots_Used = currentShip.MidSlots Then
                    MessageBox.Show("There are no available mid slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 8 ' High
                If currentShip.HiSlots_Used = currentShip.HiSlots Then
                    MessageBox.Show("There are no available high slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
        End Select

        ' Now check launcher slots
        If shipMod.IsLauncher Then
            If currentShip.LauncherSlots_Used = currentShip.LauncherSlots Then
                MessageBox.Show("There are no available launcher slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        ' Now check turret slots
        If shipMod.IsTurret Then
            If currentShip.TurretSlots_Used = currentShip.TurretSlots Then
                MessageBox.Show("There are no available turret slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        Return True
    End Function
    Private Function AddModuleInNextSlot(ByVal shipMod As ShipModule) As Integer
        Select Case shipMod.Slot
            Case 1 ' Rig
                For slotNo As Integer = 1 To 8
                    If currentShip.RigSlot(slotNo) Is Nothing Then
                        currentShip.RigSlot(slotNo) = shipMod
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available rig slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 2 ' Low
                For slotNo As Integer = 1 To 8
                    If currentShip.LowSlot(slotNo) Is Nothing Then
                        currentShip.LowSlot(slotNo) = shipMod
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available low slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 4 ' Mid
                For slotNo As Integer = 1 To 8
                    If currentShip.MidSlot(slotNo) Is Nothing Then
                        currentShip.MidSlot(slotNo) = shipMod
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available mid slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 8 ' High
                For slotNo As Integer = 1 To 8
                    If currentShip.HiSlot(slotNo) Is Nothing Then
                        currentShip.HiSlot(slotNo) = shipMod
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available high slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
        Return 0
    End Function
#End Region

#Region "Removing Mods/Drones/Items"
    Private Sub lvwSlots_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSlots.DoubleClick
        If lvwSlots.SelectedItems.Count > 0 Then
            ' Check if the "slot" is not empty
            If lvwSlots.SelectedItems(0).Text <> "<Empty>" Then
                Call RemoveModule(lvwSlots.SelectedItems(0))
            End If
        End If
    End Sub
    Private Sub RemoveModule(ByVal slot As ListViewItem)
        ' Get name of the "slot" which has slot type and number
        Dim slotType As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(0, 1))
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        Select Case slotType
            Case 1 ' Rig
                currentShip.RigSlot(slotNo) = Nothing
            Case 2 ' Low
                currentShip.LowSlot(slotNo) = Nothing
            Case 4 ' Mid
                currentShip.MidSlot(slotNo) = Nothing
            Case 8 ' High
                currentShip.HiSlot(slotNo) = Nothing
        End Select
        lvwSlots.BeginUpdate()
        slot.Text = "<Empty>"
        slot.SubItems(1).Text = ""
        slot.SubItems(2).Text = ""
        slot.SubItems(3).Text = ""
        slot.SubItems(4).Text = ""
        slot.SubItems(5).Text = ""
        slot.SubItems(6).Text = ""
        slot.ImageIndex = -1
        lvwSlots.EndUpdate()
        currentInfo.ShipType = currentShip
        Call UpdateShipDetails()
    End Sub
#End Region

#Region "UI Routines"
    Private Sub btnToggleStorage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnToggleStorage.Click
        If SplitContainer1.Panel2Collapsed = True Then
            SplitContainer1.Panel2Collapsed = False
        Else
            SplitContainer1.Panel2Collapsed = True
        End If
    End Sub
    Private Sub ctxSlots_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxSlots.Opening
        If lvwSlots.SelectedItems.Count > 0 Then
            ' Get the module details
            Dim modID As String = CStr(ModuleLists.moduleListName.Item(lvwSlots.SelectedItems(0).Text))
            Dim currentMod As New ShipModule
            If modID Is Nothing Then
                e.Cancel = True
            Else
                currentMod = CType(ModuleLists.moduleList(modID), ShipModule)
                Dim chargeName As String = lvwSlots.SelectedItems(0).SubItems(1).Text
                ' Clear the context menu
                ctxSlots.Items.Clear()
                ' Add the Show Info menu item
                If lvwSlots.SelectedItems.Count = 1 Then
                    Dim showInfoMenuItem As New ToolStripMenuItem
                    showInfoMenuItem.Name = currentMod.Name
                    showInfoMenuItem.Text = "Show Info"
                    AddHandler showInfoMenuItem.Click, AddressOf Me.ShowInfo
                    ctxSlots.Items.Add(showInfoMenuItem)
                    ' Add the Show Info menu item
                    Dim showMarketGroupMenuItem As New ToolStripMenuItem
                    showMarketGroupMenuItem.Name = currentMod.Name
                    showMarketGroupMenuItem.Text = "Show Module Market Group"
                    AddHandler showMarketGroupMenuItem.Click, AddressOf Me.ShowModuleMarketGroup
                    ctxSlots.Items.Add(showMarketGroupMenuItem)
                    ' Add the Status menu item
                    If rigGroups.Contains(currentMod.DatabaseGroup) = False Then
                        Dim canDeactivate As Boolean = False
                        Dim canOverload As Boolean = False
                        ctxSlots.Items.Add("-")
                        Dim statusMenuItem As New ToolStripMenuItem
                        statusMenuItem.Name = lvwSlots.SelectedItems(0).Name
                        statusMenuItem.Text = "Set Module Status"
                        ' Check for activation cost
                        If currentMod.Attributes.Contains("6") = True Then
                            canDeactivate = True
                        End If
                        If currentMod.Attributes.Contains("1211") = True Then
                            canOverload = True
                        End If
                        Dim offlineStatusMenu As New ToolStripMenuItem
                        offlineStatusMenu.Name = lvwSlots.SelectedItems(0).Name
                        offlineStatusMenu.Text = "Offline"
                        offlineStatusMenu.Tag = currentMod
                        AddHandler offlineStatusMenu.Click, AddressOf Me.SetModuleOffline
                        statusMenuItem.DropDownItems.Add(offlineStatusMenu)
                        Dim inactiveStatusMenu As New ToolStripMenuItem
                        If canDeactivate = True Then
                            inactiveStatusMenu.Name = lvwSlots.SelectedItems(0).Name
                            inactiveStatusMenu.Text = "Inactive"
                            inactiveStatusMenu.Tag = currentMod
                            AddHandler inactiveStatusMenu.Click, AddressOf Me.SetModuleInactive
                            statusMenuItem.DropDownItems.Add(inactiveStatusMenu)
                        End If
                        Dim activeStatusMenu As New ToolStripMenuItem
                        activeStatusMenu.Name = lvwSlots.SelectedItems(0).Name
                        activeStatusMenu.Text = "Active"
                        activeStatusMenu.Tag = currentMod
                        AddHandler activeStatusMenu.Click, AddressOf Me.SetModuleActive
                        statusMenuItem.DropDownItems.Add(activeStatusMenu)
                        Dim OverloadStatusMenu As New ToolStripMenuItem
                        If canOverload = True Then
                            OverloadStatusMenu.Name = lvwSlots.SelectedItems(0).Name
                            OverloadStatusMenu.Text = "Overload"
                            OverloadStatusMenu.Tag = currentMod
                            AddHandler OverloadStatusMenu.Click, AddressOf Me.SetModuleOverload
                            statusMenuItem.DropDownItems.Add(OverloadStatusMenu)
                        End If
                        Select Case currentMod.ModuleState
                            Case ModuleStates.Offline
                                offlineStatusMenu.Enabled = False
                            Case ModuleStates.Inactive
                                inactiveStatusMenu.Enabled = False
                            Case ModuleStates.Active
                                activeStatusMenu.Enabled = False
                            Case ModuleStates.Overloaded
                                OverloadStatusMenu.Enabled = False
                        End Select
                        ctxSlots.Items.Add(statusMenuItem)
                    End If
                End If

                ' Check if we have the same collection of modules and therefore can accept the same charges
                Dim ShowCharges As Boolean = True
                Dim cMod As New ShipModule
                If lvwSlots.SelectedItems.Count > 1 Then
                    Dim marketGroup As String = currentMod.MarketGroup
                    For Each selItems As ListViewItem In lvwSlots.SelectedItems
                        cMod = CType(ModuleLists.moduleList(CStr(ModuleLists.moduleListName.Item(selItems.Text))), ShipModule)
                        If cMod.MarketGroup <> marketGroup Then
                            ShowCharges = False
                            Exit For
                        End If
                    Next
                End If

                If ShowCharges = True Then
                    ' Get the charge group and item data
                    Dim chargeGroups As New ArrayList
                    Dim chargeGroupData() As String
                    Dim chargeItems As New SortedList
                    Dim groupName As String = ""
                    For Each chargeGroup As String In Charges.ChargeGroups
                        chargeGroupData = chargeGroup.Split("_".ToCharArray)
                        If currentMod.Charges.Contains(chargeGroupData(1)) = True Then
                            Select Case Market.MarketGroupList.Item(chargeGroupData(0)).ToString
                                Case "Small", "Medium", "Large", "Extra Large"
                                    Dim pathLine As String = CStr(Market.MarketGroupPath(chargeGroupData(0)))
                                    Dim paths() As String = pathLine.Split("\".ToCharArray)
                                    groupName = paths(paths.GetUpperBound(0) - 1)
                                Case Else
                                    groupName = Market.MarketGroupList.Item(chargeGroupData(0)).ToString
                            End Select
                            If chargeGroups.Contains(groupName) = False Then
                                chargeGroups.Add(groupName)
                            End If
                            If currentMod.IsTurret Then
                                If currentMod.ChargeSize = CInt(chargeGroupData(3)) Then
                                    chargeItems.Add(chargeGroupData(2), groupName)
                                End If
                            Else
                                chargeItems.Add(chargeGroupData(2), groupName)
                            End If
                        End If
                    Next

                    ' Create the menu items if appropriate
                    If chargeGroups.Count > 0 Then
                        If ctxSlots.Items.Count > 0 Then
                            ctxSlots.Items.Add("-")
                        End If
                        ' Add the Remove Charge option and Show Charge Info options
                        If chargeName <> "" Then
                            Dim ShowChargeInfo As New ToolStripMenuItem
                            ShowChargeInfo.Name = chargeName
                            ShowChargeInfo.Text = "Show Charge Info"
                            AddHandler ShowChargeInfo.Click, AddressOf Me.ShowChargeInfo
                            ctxSlots.Items.Add(ShowChargeInfo)
                            Dim RemoveCharge As New ToolStripMenuItem
                            RemoveCharge.Name = currentMod.Name
                            If lvwSlots.SelectedItems.Count > 1 Then
                                RemoveCharge.Text = "Remove Charges"
                            Else
                                RemoveCharge.Text = "Remove " & chargeName
                            End If
                            AddHandler RemoveCharge.Click, AddressOf Me.RemoveChargeFromModule
                            ctxSlots.Items.Add(RemoveCharge)
                            ctxSlots.Items.Add("-")
                        End If
                        ' Add the Groups
                        For Each group As String In chargeGroups
                            Dim newGroup As New ToolStripMenuItem()
                            newGroup.Name = group
                            newGroup.Text = group
                            For Each charge As String In chargeItems.Keys
                                If chargeItems(charge).ToString = group Then
                                    Dim newCharge As New ToolStripMenuItem
                                    newCharge.Name = CStr(ModuleLists.moduleListName(charge))
                                    newCharge.Text = charge
                                    AddHandler newCharge.Click, AddressOf Me.LoadChargeIntoModule
                                    newGroup.DropDownItems.Add(newCharge)
                                End If
                            Next
                            ctxSlots.Items.Add(newGroup)
                        Next
                    End If
                End If
            End If
            ' Cancel the menu if there is nothing to display
            If ctxSlots.Items.Count = 0 Then
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If

    End Sub
    Private Sub ShowInfo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectedSlot As ListViewItem = lvwSlots.SelectedItems(0)
        Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
        Dim sModule As New ShipModule
        Select Case CInt(slotInfo(0))
            Case 1 ' Rig
                sModule = fittedShip.RigSlot(CInt(slotInfo(1)))
            Case 2 ' Low
                sModule = fittedShip.LowSlot(CInt(slotInfo(1)))
            Case 4 ' Mid
                sModule = fittedShip.MidSlot(CInt(slotInfo(1)))
            Case 8 ' High
                sModule = fittedShip.HiSlot(CInt(slotInfo(1)))
        End Select
        Dim showInfo As New frmShowInfo
        showInfo.ShowItemDetails(sModule)
        showInfo = Nothing
    End Sub
    Private Sub ShowChargeInfo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectedSlot As ListViewItem = lvwSlots.SelectedItems(0)
        Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
        Dim sModule As New ShipModule
        Select Case CInt(slotInfo(0))
            Case 1 ' Rig
                sModule = fittedShip.RigSlot(CInt(slotInfo(1))).LoadedCharge
            Case 2 ' Low
                sModule = fittedShip.LowSlot(CInt(slotInfo(1))).LoadedCharge
            Case 4 ' Mid
                sModule = fittedShip.MidSlot(CInt(slotInfo(1))).LoadedCharge
            Case 8 ' High
                sModule = fittedShip.HiSlot(CInt(slotInfo(1))).LoadedCharge
        End Select
        Dim showInfo As New frmShowInfo
        showInfo.ShowItemDetails(sModule)
        showInfo = Nothing
    End Sub
    Private Sub ShowModuleMarketGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ShowMarketMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim moduleName As String = ShowMarketMenu.Name
        Dim moduleID As String = CStr(ModuleLists.moduleListName(moduleName))
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim pathLine As String = CStr(Market.MarketGroupPath(cModule.MarketGroup))
        ShipModule.DisplayedMarketGroup = pathLine
    End Sub
#End Region

#Region "Set Module Status"
    Private Sub SetModuleOffline(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        sModule.ModuleState = ModuleStates.Offline
        currentInfo.ShipType = currentShip
        Call Me.UpdateSlotLocation(sModule, slotNo)
    End Sub
    Private Sub SetModuleInactive(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        sModule.ModuleState = ModuleStates.Inactive
        currentInfo.ShipType = currentShip
        Call Me.UpdateSlotLocation(sModule, slotNo)
    End Sub
    Private Sub SetModuleActive(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        sModule.ModuleState = ModuleStates.Active
        currentInfo.ShipType = currentShip
        Call Me.UpdateSlotLocation(sModule, slotNo)
    End Sub
    Private Sub SetModuleOverload(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        sModule.ModuleState = ModuleStates.Overloaded
        currentInfo.ShipType = currentShip
        Call Me.UpdateSlotLocation(sModule, slotNo)
    End Sub
#End Region

#Region "Load/Remove Charges"
    Private Sub RemoveChargeFromModule(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each selItem As ListViewItem In lvwSlots.SelectedItems
            Dim slotType As Integer = CInt(selItem.Name.Substring(0, 1))
            Dim slotNo As Integer = CInt(selItem.Name.Substring(2, 1))
            Dim LoadedModule As New ShipModule
            Select Case slotType
                Case 1 ' Rig
                    LoadedModule = currentShip.RigSlot(slotNo)
                Case 2 ' Low
                    LoadedModule = currentShip.LowSlot(slotNo)
                Case 4 ' Mid
                    LoadedModule = currentShip.MidSlot(slotNo)
                Case 8 ' High
                    LoadedModule = currentShip.HiSlot(slotNo)
            End Select
            LoadedModule.LoadedCharge = Nothing
        Next
        currentInfo.ShipType = currentShip
        Call UpdateAllSlotLocations()
    End Sub
    Private Sub LoadChargeIntoModule(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ChargeMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim moduleID As String = ChargeMenu.Name
        Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule).Clone
        ' Get name of the "slot" which has slot type and number
        For Each selItem As ListViewItem In lvwSlots.SelectedItems
            Dim slotType As Integer = CInt(selItem.Name.Substring(0, 1))
            Dim slotNo As Integer = CInt(selItem.Name.Substring(2, 1))
            Dim LoadedModule As New ShipModule
            Select Case slotType
                Case 1 ' Rig
                    LoadedModule = currentShip.RigSlot(slotNo)
                Case 2 ' Low
                    LoadedModule = currentShip.LowSlot(slotNo)
                Case 4 ' Mid
                    LoadedModule = currentShip.MidSlot(slotNo)
                Case 8 ' High
                    LoadedModule = currentShip.HiSlot(slotNo)
            End Select
            LoadedModule.LoadedCharge = Charge
        Next
        currentInfo.ShipType = currentShip
        Call UpdateAllSlotLocations()
    End Sub
#End Region

#Region "Cargo & Drone Bay Routines"
    Private Sub RedrawCargoBay()
        lvwCargoBay.BeginUpdate()
        lvwCargoBay.Items.Clear()
        currentShip.CargoBay_Used = 0
        Dim CBI As CargoBayItem
        Dim HoldingBay As New SortedList
        For Each CBI In currentShip.CargoBayItems.Values
            HoldingBay.Add(HoldingBay.Count, CBI)
        Next
        currentShip.CargoBayItems.Clear()
        For Each CBI In HoldingBay.Values
            Dim newCargoItem As New ListViewItem(CBI.ItemType.Name)
            newCargoItem.Name = CStr(lvwCargoBay.Items.Count)
            newCargoItem.SubItems.Add(CStr(CBI.Quantity))
            currentShip.CargoBayItems.Add(lvwCargoBay.Items.Count, CBI)
            currentShip.CargoBay_Used += CBI.ItemType.Volume * CBI.Quantity
            lvwCargoBay.Items.Add(newCargoItem)
        Next
        lvwCargoBay.EndUpdate()
        lblCargoBay.Text = FormatNumber(currentShip.CargoBay_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(currentShip.CargoBay, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m³"
        pbCargoBay.Maximum = CInt(currentShip.CargoBay)
        pbCargoBay.Value = CInt(currentShip.CargoBay_Used)
    End Sub

    Private Sub RedrawDroneBay()
        lvwDroneBay.BeginUpdate()
        lvwDroneBay.Items.Clear()
        currentShip.DroneBay_Used = 0
        Dim DBI As DroneBayItem
        Dim HoldingBay As New SortedList
        For Each DBI In currentShip.DroneBayItems.Values
            HoldingBay.Add(HoldingBay.Count, DBI)
        Next
        currentShip.DroneBayItems.Clear()
        For Each DBI In HoldingBay.Values
            Dim newDroneItem As New ListViewItem(DBI.DroneType.Name)
            newDroneItem.Name = CStr(lvwDroneBay.Items.Count)
            newDroneItem.SubItems.Add(CStr(DBI.Quantity))
            If DBI.IsActive = True Then
                newDroneItem.Checked = True
            End If
            currentShip.DroneBayItems.Add(lvwDroneBay.Items.Count, DBI)
            currentShip.DroneBay_Used += DBI.DroneType.Volume * DBI.Quantity
            lvwDroneBay.Items.Add(newDroneItem)
        Next
        lvwDroneBay.EndUpdate()
        lblDroneBay.Text = FormatNumber(currentShip.DroneBay_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(currentShip.DroneBay, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m³"
        pbDroneBay.Maximum = CInt(currentShip.DroneBay)
        pbDroneBay.Value = CInt(currentShip.DroneBay_Used)
    End Sub

    Private Sub lvwDroneBay_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwDroneBay.ItemChecked
        Dim idx As Integer = e.Item.Index
        Dim DBI As DroneBayItem = CType(currentShip.DroneBayItems.Item(idx), DroneBayItem)
        DBI.IsActive = e.Item.Checked
        Call Me.UpdateFittingListFromShipData()
    End Sub

    Private Sub ctxBays_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxBays.Opening
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        If lvwBay.SelectedIndices.Count = 0 Then
            e.Cancel = True
        Else
            If lvwBay.SelectedIndices.Count > 1 Then
                ctxAlterQuantity.Enabled = False
                ctxSplitBatch.Enabled = False
            Else
                ctxAlterQuantity.Enabled = True
                ctxSplitBatch.Enabled = True
            End If
        End If
    End Sub

    Private Sub ctxRemoveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxRemoveItem.Click
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        Select Case lvwBay.Name
            Case "lvwCargoBay"
                ' Removes the item from the cargo bay
                For Each remItem As ListViewItem In lvwCargoBay.SelectedItems
                    currentShip.CargoBayItems.Remove(CInt(remItem.Name))
                    lvwCargoBay.Items.RemoveByKey(remItem.Name)
                Next
                Call Me.UpdateFittingListFromShipData()
                Call RedrawCargoBay()
            Case "lvwDroneBay"
                ' Removes the item from the drone bay
                For Each remItem As ListViewItem In lvwDroneBay.SelectedItems
                    currentShip.DroneBayItems.Remove(CInt(remItem.Name))
                    lvwDroneBay.Items.RemoveByKey(remItem.Name)
                Next
                Call Me.UpdateFittingListFromShipData()
                Call RedrawDroneBay()
        End Select
    End Sub
#End Region

#Region "Clipboard Copy Routines"
    Private Sub btnClipboardCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClipboardCopy.Click
        Call Me.ClipboardCopy(True)
    End Sub
    Private Function ClipboardCopy(ByVal EFTCompatible As Boolean) As String
        Dim cModule As New ShipModule
        Dim fitting As New StringBuilder
        fitting.AppendLine("[" & cShipFit & "]")
        For slot As Integer = 1 To currentShip.LowSlots
            If currentShip.LowSlot(slot) IsNot Nothing Then
                If currentShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentShip.LowSlot(slot).Name & ", " & currentShip.LowSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentShip.LowSlot(slot).Name)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentShip.MidSlots
            If currentShip.MidSlot(slot) IsNot Nothing Then
                If currentShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentShip.MidSlot(slot).Name & ", " & currentShip.MidSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentShip.MidSlot(slot).Name)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentShip.HiSlots
            If currentShip.HiSlot(slot) IsNot Nothing Then
                If currentShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentShip.HiSlot(slot).Name & ", " & currentShip.HiSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentShip.HiSlot(slot).Name)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentShip.RigSlots
            If currentShip.RigSlot(slot) IsNot Nothing Then
                If currentShip.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentShip.RigSlot(slot).Name & ", " & currentShip.RigSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentShip.RigSlot(slot).Name)
                End If
            End If
        Next
        fitting.AppendLine("")
        For Each drone As DroneBayItem In currentShip.DroneBayItems.Values
            If EFTCompatible = True Then
                fitting.AppendLine(drone.DroneType.Name & " x" & drone.Quantity)
            Else
                fitting.AppendLine(drone.DroneType.Name & " x" & drone.Quantity & ", " & drone.IsActive)
            End If
        Next
        fitting.AppendLine("")
        For Each cargo As CargoBayItem In currentShip.CargoBayItems.Values
            fitting.AppendLine(cargo.ItemType.Name & " x" & cargo.Quantity)
        Next
        MessageBox.Show(fitting.ToString)
        Clipboard.SetText(fitting.ToString)
        Return fitting.ToString
    End Function
#End Region

End Class
