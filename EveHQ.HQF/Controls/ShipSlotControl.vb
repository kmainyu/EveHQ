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
Imports System.Drawing
Imports System.Text

Public Class ShipSlotControl
    Dim UpdateAll As Boolean = False
    Dim UpdateDrones As Boolean = False
    Dim cancelDroneActivation As Boolean = False
    Dim rigGroups As New ArrayList
    Dim remoteGroups As New ArrayList
    Dim fleetGroups As New ArrayList
    Dim fleetSkills As New ArrayList
    Dim cancelSlotMenu As Boolean = False

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

    Public ReadOnly Property ShipCurrent() As Ship
        Get
            Return currentShip
        End Get
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
        Call Me.UpdateSlotColumns()
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
        currentInfo.UpdateImplantList()
        currentInfo.BuildMethod = BuildType.BuildEverything
        If fittedShip IsNot Nothing Then
            Me.UpdateAllSlotLocations()
            Me.UpdateShipDetails()
            Me.RedrawDroneBay()
            Me.RedrawCargoBay()
        Else
            MessageBox.Show("The fitting for " & cShipFit & " failed to produce a calculated setup.", "Error Calculating Fitting", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
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
            Call Me.AddUserColumns(currentShip.HiSlot(slot), newSlot)
            lvwSlots.Items.Add(newSlot)
        Next
        For slot As Integer = 1 To currentShip.MidSlots
            Dim newSlot As New ListViewItem
            newSlot.Name = "4_" & slot
            newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
            newSlot.ForeColor = Color.Black
            newSlot.Group = lvwSlots.Groups.Item("lvwgMidSlots")
            Call Me.AddUserColumns(currentShip.MidSlot(slot), newSlot)
            lvwSlots.Items.Add(newSlot)
        Next
        For slot As Integer = 1 To currentShip.LowSlots
            Dim newSlot As New ListViewItem
            newSlot.Name = "2_" & slot
            newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
            newSlot.ForeColor = Color.Black
            newSlot.Group = lvwSlots.Groups.Item("lvwgLowSlots")
            Call Me.AddUserColumns(currentShip.LowSlot(slot), newSlot)
            lvwSlots.Items.Add(newSlot)
        Next
        For slot As Integer = 1 To currentShip.RigSlots
            Dim newSlot As New ListViewItem
            newSlot.Name = "1_" & slot
            newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
            newSlot.ForeColor = Color.Black
            newSlot.Group = lvwSlots.Groups.Item("lvwgRigSlots")
            Call Me.AddUserColumns(currentShip.RigSlot(slot), newSlot)
            lvwSlots.Items.Add(newSlot)
        Next
        lvwSlots.EndUpdate()
        Call UpdateSlotNumbers()
        Call UpdatePrices()
    End Sub
    Private Sub UpdateSlotNumbers()
        lblHighSlots.Text = "Hi: " & currentShip.HiSlots_Used & "/" & currentShip.HiSlots
        lblMidSlots.Text = "Mid: " & currentShip.MidSlots_Used & "/" & currentShip.MidSlots
        lblLowSlots.Text = "Low: " & currentShip.LowSlots_Used & "/" & currentShip.LowSlots
        lblRigSlots.Text = "Rig: " & currentShip.RigSlots_Used & "/" & currentShip.RigSlots
        lblLauncherSlots.Text = "Launchers: " & currentShip.LauncherSlots_Used & "/" & currentShip.LauncherSlots
        lblTurretSlots.Text = "Turrets: " & currentShip.TurretSlots_Used & "/" & currentShip.TurretSlots
    End Sub
    Private Sub UpdatePrices()
        currentShip.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(currentShip.ID)
        lblShipMarketPrice.Text = "Ship Market Price: " & FormatNumber(currentShip.MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblFittingMarketPrice.Text = "Fitting Market Price: " & FormatNumber(currentShip.MarketPrice + currentShip.FittingMarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
    End Sub
    Private Sub UpdateSlotColumns()
        ' Clear the columns
        lvwSlots.Columns.Clear()
        ' Add the module name column
        lvwSlots.Columns.Add("colName", "Module Name", 175, HorizontalAlignment.Left, "")
        ' Iterate through the user selected columns and add them in
        Dim colName As String = ""
        For Each col As String In HQF.Settings.HQFSettings.UserSlotColumns
            If col.EndsWith("1") = True Then
                colName = col.Substring(0, col.Length - 1)
                lvwSlots.Columns.Add(colName, colName, 75, HorizontalAlignment.Left, "")
            End If
        Next
    End Sub
    Private Sub UpdateAllSlotLocations()
        If fittedShip IsNot Nothing Then
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
            Call Me.RedrawCargoBayCapacity()
            Call Me.RedrawDroneBayCapacity()
        End If
    End Sub
    Private Sub UpdateSlotLocation(ByVal oldMod As ShipModule, ByVal slotNo As Integer)
        If oldMod IsNot Nothing Then
            Dim shipMod As New ShipModule
            Select Case oldMod.SlotType
                Case 1 ' Rig
                    shipMod = fittedShip.RigSlot(slotNo)
                Case 2 ' Low
                    shipMod = fittedShip.LowSlot(slotNo)
                Case 4 ' Mid
                    shipMod = fittedShip.MidSlot(slotNo)
                Case 8 ' High
                    shipMod = fittedShip.HiSlot(slotNo)
            End Select
            If shipMod IsNot Nothing Then
                shipMod.ModuleState = oldMod.ModuleState
                Dim slotName As ListViewItem = lvwSlots.Items(shipMod.SlotType & "_" & slotNo)
                slotName.ImageIndex = CInt(Math.Log(shipMod.ModuleState) / Math.Log(2))
                slotName.UseItemStyleForSubItems = True
                Call Me.UpdateUserColumns(shipMod, slotName)
            End If
        End If
        Call UpdateShipDetails()
    End Sub
    Private Sub AddUserColumns(ByVal shipMod As ShipModule, ByVal slotName As ListViewItem)
        ' Add subitems based on the user selected columns
        If shipMod IsNot Nothing Then
            Dim colName As String = ""
            ' Add in the module name
            slotName.Text = shipMod.Name
            ' Add the additional columns
            For Each col As String In Settings.HQFSettings.UserSlotColumns
                If col.EndsWith("1") = True Then
                    colName = col.Substring(0, col.Length - 1)
                    Select Case colName
                        Case "Charge"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                slotName.SubItems.Add(shipMod.LoadedCharge.Name)
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "CPU"
                            slotName.SubItems.Add(FormatNumber(shipMod.CPU, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        Case "PG"
                            slotName.SubItems.Add(FormatNumber(shipMod.PG, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        Case "Calibration"
                            slotName.SubItems.Add(FormatNumber(shipMod.Calibration, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        Case "Price"
                            shipMod.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(shipMod.ID)
                            slotName.SubItems.Add(FormatNumber(shipMod.MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        Case "ActCost"
                            If shipMod.Attributes.Contains("6") Then
                                If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotName.SubItems.Add(FormatNumber(shipMod.CapUsage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                                Else
                                    slotName.SubItems.Add("")
                                End If
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "ActTime"
                            If shipMod.Attributes.Contains("73") Then
                                If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotName.SubItems.Add(FormatNumber(shipMod.ActivationTime, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                                Else
                                    slotName.SubItems.Add("")
                                End If
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "CapUsageRate"
                            If shipMod.Attributes.Contains("10032") Then
                                If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotName.SubItems.Add(FormatNumber(shipMod.CapUsageRate * -1, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                                Else
                                    slotName.SubItems.Add("")
                                End If
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "OptRange"
                            If shipMod.Attributes.Contains("54") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("54"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            ElseIf shipMod.Attributes.Contains("87") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("87"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            ElseIf shipMod.Attributes.Contains("91") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("91"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            ElseIf shipMod.Attributes.Contains("98") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("98"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            ElseIf shipMod.Attributes.Contains("99") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("99"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "ROF"
                            If shipMod.Attributes.Contains("51") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("51"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            ElseIf shipMod.Attributes.Contains("10011") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("10011"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            ElseIf shipMod.Attributes.Contains("10012") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("10012"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            ElseIf shipMod.Attributes.Contains("10013") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("10013"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "Damage"
                            If shipMod.Attributes.Contains("10018") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("10018"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "DPS"
                            If shipMod.Attributes.Contains("10019") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("10019"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "Falloff"
                            If shipMod.Attributes.Contains("158") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("158"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "Tracking"
                            If shipMod.Attributes.Contains("160") Then
                                slotName.SubItems.Add(FormatNumber(shipMod.Attributes("160"), 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "ExpRad"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                If shipMod.LoadedCharge.Attributes.Contains("654") Then
                                    slotName.SubItems.Add(FormatNumber(shipMod.LoadedCharge.Attributes("654"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                                Else
                                    slotName.SubItems.Add("")
                                End If
                            Else
                                slotName.SubItems.Add("")
                            End If
                        Case "ExpVel"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                If shipMod.LoadedCharge.Attributes.Contains("653") Then
                                    slotName.SubItems.Add(FormatNumber(shipMod.LoadedCharge.Attributes("653"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                                Else
                                    slotName.SubItems.Add("")
                                End If
                            Else
                                slotName.SubItems.Add("")
                            End If
                    End Select
                End If
            Next
        Else
            slotName.Text = "<Empty>"
            For Each col As String In Settings.HQFSettings.UserSlotColumns
                slotName.SubItems.Add("")
            Next
        End If
    End Sub
    Private Sub UpdateUserColumns(ByVal shipMod As ShipModule, ByVal slotName As ListViewItem)
        ' Add subitems based on the user selected columns
        Dim colName As String = ""
        Dim idx As Integer = 1
        ' Add in the module name
        slotName.Text = shipMod.Name
        ' Add the additional columns
        For Each col As String In Settings.HQFSettings.UserSlotColumns
            If col.EndsWith("1") = True Then
                colName = col.Substring(0, col.Length - 1)
                Select Case colName
                    Case "Charge"
                        If shipMod.LoadedCharge IsNot Nothing Then
                            slotName.SubItems(idx).Text = shipMod.LoadedCharge.Name
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "CPU"
                        slotName.SubItems(idx).Text = FormatNumber(shipMod.CPU, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        idx += 1
                    Case "PG"
                        slotName.SubItems(idx).Text = FormatNumber(shipMod.PG, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        idx += 1
                    Case "Calibration"
                        slotName.SubItems(idx).Text = FormatNumber(shipMod.Calibration, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        idx += 1
                    Case "Price"
                        shipMod.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(shipMod.ID)
                        slotName.SubItems(idx).Text = FormatNumber(shipMod.MarketPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        idx += 1
                    Case "ActCost"
                        If shipMod.Attributes.Contains("6") Then
                            If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                slotName.SubItems(idx).Text = FormatNumber(shipMod.CapUsage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Else
                                slotName.SubItems(idx).Text = ""
                            End If
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "ActTime"
                        If shipMod.Attributes.Contains("73") Then
                            If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                slotName.SubItems(idx).Text = FormatNumber(shipMod.ActivationTime, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Else
                                slotName.SubItems(idx).Text = ""
                            End If
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "CapUsageRate"
                        If shipMod.Attributes.Contains("10032") Then
                            If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                slotName.SubItems(idx).Text = FormatNumber(shipMod.CapUsageRate * -1, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Else
                                slotName.SubItems(idx).Text = ""
                            End If
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "OptRange"
                        If shipMod.Attributes.Contains("54") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("54"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        ElseIf shipMod.Attributes.Contains("87") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("87"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        ElseIf shipMod.Attributes.Contains("91") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("91"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        ElseIf shipMod.Attributes.Contains("98") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("98"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        ElseIf shipMod.Attributes.Contains("99") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("99"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "ROF"
                        If shipMod.Attributes.Contains("51") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("51"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        ElseIf shipMod.Attributes.Contains("10011") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("10011"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        ElseIf shipMod.Attributes.Contains("10012") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("10012"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        ElseIf shipMod.Attributes.Contains("10013") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("10013"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "Damage"
                        If shipMod.Attributes.Contains("10018") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("10018"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "DPS"
                        If shipMod.Attributes.Contains("10019") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("10019"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "Falloff"
                        If shipMod.Attributes.Contains("158") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("158"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "Tracking"
                        If shipMod.Attributes.Contains("160") Then
                            slotName.SubItems(idx).Text = FormatNumber(shipMod.Attributes("160"), 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "ExpRad"
                        If shipMod.LoadedCharge IsNot Nothing Then
                            If shipMod.LoadedCharge.Attributes.Contains("654") Then
                                slotName.SubItems(idx).Text = FormatNumber(shipMod.LoadedCharge.Attributes("654"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Else
                                slotName.SubItems(idx).Text = ""
                            End If
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                    Case "ExpVel"
                        If shipMod.LoadedCharge IsNot Nothing Then
                            If shipMod.LoadedCharge.Attributes.Contains("653") Then
                                slotName.SubItems(idx).Text = FormatNumber(shipMod.LoadedCharge.Attributes("653"), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            Else
                                slotName.SubItems(idx).Text = ""
                            End If
                        Else
                            slotName.SubItems(idx).Text = ""
                        End If
                        idx += 1
                End Select
            End If
        Next
    End Sub
    Private Sub UpdateShipDataFromFittingList()
        Dim currentFitList As ArrayList = CType(currentFit.Clone, ArrayList)
        For Each shipMod As String In currentFitList
            If shipMod IsNot Nothing Then
                ' Check for installed charges
                Dim modData() As String = shipMod.Split(",".ToCharArray)
                Dim state As Integer = 4
                Dim itemQuantity As Integer = 1
                If modData(0).Length > 2 Then
                    If modData(0).Substring(modData(0).Length - 2, 1) = "_" Then
                        state = CInt(modData(0).Substring(modData(0).Length - 1, 1))
                        modData(0) = modData(0).TrimEnd(("_" & state.ToString).ToCharArray)
                        state = CInt(Math.Pow(2, state))
                    End If
                End If
                ' Check for item quantity (EFT method)
                Dim qSep As Integer = InStrRev(modData(0), " ")
                If qSep > 0 Then
                    Dim qString As String = modData(0).Substring(qSep)
                    If qString.StartsWith("x") Then
                        qString = qString.TrimStart("x".ToCharArray)
                        If IsNumeric(qString) = True Then
                            itemQuantity = CInt(qString)
                            modData(0) = modData(0).TrimEnd((" x" & itemQuantity.ToString).ToCharArray)
                        End If
                    End If
                End If
                ' Check if the module exists
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
                            If modData.GetUpperBound(0) > 0 Then
                                If modData(1).EndsWith("a") = True Then
                                    active = True
                                    itemQuantity = CInt(modData(1).Substring(0, Len(modData(1)) - 1))
                                Else
                                    If modData(1).EndsWith("i") = True Then
                                        itemQuantity = CInt(modData(1).Substring(0, Len(modData(1)) - 1))
                                    Else
                                        itemQuantity = CInt(modData(1))
                                    End If
                                End If
                            End If
                            Call Me.AddDrone(sMod, itemQuantity, active)
                        Else
                            ' Check if module is a charge
                            If sMod.IsCharge = True Then
                                If modData.GetUpperBound(0) > 0 Then
                                    itemQuantity = CInt(modData(1))
                                End If
                                Call Me.AddItem(sMod, itemQuantity)
                            Else
                                ' Must be a proper module then!
                                sMod.ModuleState = state
                                Call AddModule(sMod, 0, True, Nothing)
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
        Dim state As Integer

        For slot As Integer = 1 To currentShip.HiSlots
            If currentShip.HiSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentShip.HiSlot(slot).ModuleState) / Math.Log(2))
                If currentShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    currentFit.Add(currentShip.HiSlot(slot).Name & "_" & state & ", " & currentShip.HiSlot(slot).LoadedCharge.Name)
                Else
                    currentFit.Add(currentShip.HiSlot(slot).Name & "_" & state)
                End If
            End If
        Next

        For slot As Integer = 1 To currentShip.MidSlots
            If currentShip.MidSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentShip.MidSlot(slot).ModuleState) / Math.Log(2))
                If currentShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    currentFit.Add(currentShip.MidSlot(slot).Name & "_" & state & ", " & currentShip.MidSlot(slot).LoadedCharge.Name)
                Else
                    currentFit.Add(currentShip.MidSlot(slot).Name & "_" & state)
                End If
            End If
        Next

        For slot As Integer = 1 To currentShip.LowSlots
            If currentShip.LowSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentShip.LowSlot(slot).ModuleState) / Math.Log(2))
                If currentShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    currentFit.Add(currentShip.LowSlot(slot).Name & "_" & state & ", " & currentShip.LowSlot(slot).LoadedCharge.Name)
                Else
                    currentFit.Add(currentShip.LowSlot(slot).Name & "_" & state)
                End If
            End If
        Next

        For slot As Integer = 1 To currentShip.RigSlots
            If currentShip.RigSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentShip.RigSlot(slot).ModuleState) / Math.Log(2))
                If currentShip.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    currentFit.Add(currentShip.RigSlot(slot).Name & "_" & state & ", " & currentShip.RigSlot(slot).LoadedCharge.Name)
                Else
                    currentFit.Add(currentShip.RigSlot(slot).Name & "_" & state)
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
        If currentShip IsNot Nothing Then
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
        End If
    End Sub
    Private Sub ClearDroneBay()
        If currentShip IsNot Nothing Then
            currentShip.DroneBayItems.Clear()
            currentShip.DroneBay_Used = 0
        End If
    End Sub
    Private Sub ClearCargoBay()
        If currentShip IsNot Nothing Then
            currentShip.CargoBayItems.Clear()
            currentShip.CargoBay_Used = 0
        End If
    End Sub
#End Region

#Region "Adding Mods/Drones/Items"
    Public Sub AddModule(ByVal shipMod As ShipModule, ByVal slotNo As Integer, ByVal updateShip As Boolean, ByVal repMod As ShipModule)
        ' Check slot availability
        If IsSlotAvailable(shipMod, repMod) = False Then
            Exit Sub
        End If
        ' Add Module to the next slot
        If slotNo = 0 Then
            slotNo = AddModuleInNextSlot(CType(shipMod.Clone, ShipModule))
        Else
            AddModuleInSpecifiedSlot(CType(shipMod.Clone, ShipModule), slotNo)
        End If
        If UpdateAll = False And updateShip = True Then
            currentInfo.ShipType = currentShip
            currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
            'Call UpdateSlotLocation(shipMod, slotNo)
            Call Me.UpdateAllSlotLocations()
        End If
    End Sub
    Public Sub AddDrone(ByVal Drone As ShipModule, ByVal Qty As Integer, ByVal Active As Boolean)
        ' Set grouping flag
        Dim grouped As Boolean = False
        ' See if there is sufficient space
        Dim vol As Double = Drone.Volume
        Dim myShip As New Ship
        If fittedShip IsNot Nothing Then
            myShip = fittedShip
        Else
            myShip = currentShip
        End If
        If myShip.DroneBay - currentShip.DroneBay_Used >= vol Then
            ' Scan through existing items and see if we can group this new one
            For Each droneGroup As DroneBayItem In currentShip.DroneBayItems.Values
                If Drone.Name = droneGroup.DroneType.Name And Active = droneGroup.IsActive And UpdateAll = False Then
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
                currentInfo.ShipType = currentShip
                currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
                UpdateDrones = True
                Call Me.RedrawDroneBay()
                UpdateDrones = False
                Call UpdatePrices()
                Call UpdateFittingListFromShipData()
            End If
        Else
            MessageBox.Show("There is not enough space in the Drone Bay to hold 1 unit of " & Drone.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Public Sub AddItem(ByVal Item As ShipModule, ByVal Qty As Integer)
        If currentShip IsNot Nothing Then
            ' Set grouping flag
            Dim grouped As Boolean = False
            ' See if there is sufficient space
            Dim vol As Double = Item.Volume
            Dim myShip As New Ship
            If fittedShip IsNot Nothing Then
                myShip = fittedShip
            Else
                myShip = currentShip
            End If
            If myShip.CargoBay - currentShip.CargoBay_Used >= vol Then
                ' Scan through existing items and see if we can group this new one
                For Each itemGroup As CargoBayItem In currentShip.CargoBayItems.Values
                    If Item.Name = itemGroup.ItemType.Name And UpdateAll = False Then
                        ' Add to existing drone group
                        itemGroup.Quantity += Qty
                        grouped = True
                        Exit For
                    End If
                Next
                ' Put the item into the cargo bay if not grouped
                If grouped = False Then
                    Dim CBI As New CargoBayItem
                    CBI.ItemType = Item
                    CBI.Quantity = Qty
                    currentShip.CargoBayItems.Add(currentShip.CargoBayItems.Count, CBI)
                End If
                ' Update stuff
                If UpdateAll = False Then
                    currentInfo.ShipType = currentShip
                    currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
                    Call Me.RedrawCargoBay()
                    Call UpdatePrices()
                    Call UpdateFittingListFromShipData()
                End If
            Else
                MessageBox.Show("There is not enough space in the Cargo Bay to hold 1 unit of " & Item.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Function IsSlotAvailable(ByVal shipMod As ShipModule, Optional ByVal repShipMod As ShipModule = Nothing) As Boolean
        Dim cRig, cLow, cMid, cHi, cTurret, cLauncher As Integer

        If repShipMod IsNot Nothing Then
            Select Case repShipMod.SlotType
                Case 1 ' Rig
                    cRig = currentShip.RigSlots_Used - 1
                Case 2 ' Low
                    cLow = currentShip.LowSlots_Used - 1
                Case 4 ' Mid
                    cMid = currentShip.MidSlots_Used - 1
                Case 8 ' High
                    cHi = currentShip.HiSlots_Used - 1
            End Select
            If repShipMod.IsTurret = True Then
                cTurret = currentShip.TurretSlots_Used - 1
            End If
            If repShipMod.IsLauncher = True Then
                cLauncher = currentShip.LauncherSlots_Used - 1
            End If
        Else
            cRig = currentShip.RigSlots_Used
            cLow = currentShip.LowSlots_Used
            cMid = currentShip.MidSlots_Used
            cHi = currentShip.HiSlots_Used
            cTurret = currentShip.TurretSlots_Used
            cLauncher = currentShip.LauncherSlots_Used
        End If
        ' First, check slot layout
        Select Case shipMod.SlotType
            Case 1 ' Rig
                If cRig = currentShip.RigSlots Then
                    MessageBox.Show("There are no available rig slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 2 ' Low
                If cLow = currentShip.LowSlots Then
                    MessageBox.Show("There are no available low slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 4 ' Mid
                If cMid = currentShip.MidSlots Then
                    MessageBox.Show("There are no available mid slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case 8 ' High
                If cHi = currentShip.HiSlots Then
                    MessageBox.Show("There are no available high slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
        End Select

        ' Now check launcher slots
        If shipMod.IsLauncher Then
            If cLauncher = currentShip.LauncherSlots Then
                MessageBox.Show("There are no available launcher slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        ' Now check turret slots
        If shipMod.IsTurret Then
            If cTurret = currentShip.TurretSlots Then
                MessageBox.Show("There are no available turret slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        Return True
    End Function
    Private Function AddModuleInNextSlot(ByVal shipMod As ShipModule) As Integer
        Select Case shipMod.SlotType
            Case 1 ' Rig
                For slotNo As Integer = 1 To 8
                    If currentShip.RigSlot(slotNo) Is Nothing Then
                        currentShip.RigSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available rig slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 2 ' Low
                For slotNo As Integer = 1 To 8
                    If currentShip.LowSlot(slotNo) Is Nothing Then
                        currentShip.LowSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available low slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 4 ' Mid
                For slotNo As Integer = 1 To 8
                    If currentShip.MidSlot(slotNo) Is Nothing Then
                        currentShip.MidSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available mid slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case 8 ' High
                For slotNo As Integer = 1 To 8
                    If currentShip.HiSlot(slotNo) Is Nothing Then
                        currentShip.HiSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available high slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
        Return 0
    End Function
    Private Function AddModuleInSpecifiedSlot(ByVal shipMod As ShipModule, ByVal slotNo As Integer) As Integer
        Select Case shipMod.SlotType
            Case 1 ' Rig
                currentShip.RigSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case 2 ' Low
                currentShip.LowSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case 4 ' Mid
                currentShip.MidSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case 8 ' High
                currentShip.HiSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
        End Select
        Return 0
    End Function
#End Region

#Region "Removing Mods/Drones/Items"

    Private Sub lvwSlots_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSlots.Click
        If cancelSlotMenu = True Then
            lvwSlots.SelectedItems.Clear()
        End If
    End Sub

    Private Sub lvwSlots_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwSlots.ColumnClick
        lvwSlots.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub
    Private Sub lvwSlots_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSlots.DoubleClick
        If lvwSlots.SelectedItems.Count > 0 Then
            ' Check if the "slot" is not empty
            If cancelSlotMenu = False Then
                If lvwSlots.SelectedItems(0).Text <> "<Empty>" Then
                    Call RemoveModule(lvwSlots.SelectedItems(0), True)
                End If
            End If
        End If
    End Sub
    Private Sub RemoveModules(ByVal sender As Object, ByVal e As System.EventArgs)
        lvwSlots.BeginUpdate()
        For Each slot As ListViewItem In lvwSlots.SelectedItems
            If slot.Text <> "<Empty>" Then
                Dim slotType As Integer = CInt(slot.Name.Substring(0, 1))
                Dim slotNo As Integer = CInt(slot.Name.Substring(2, 1))
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
                For Each si As ListViewItem.ListViewSubItem In slot.SubItems
                    si.Text = ""
                Next
                slot.Text = "<Empty>"
                slot.ImageIndex = -1
            End If
        Next
        lvwSlots.EndUpdate()
        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
        Call UpdateShipDetails()
        Call Me.UpdateAllSlotLocations()
    End Sub
    Private Sub RemoveModule(ByVal slot As ListViewItem, ByVal updateShip As Boolean)
        ' Get name of the "slot" which has slot type and number
        Dim slotType As Integer = CInt(slot.Name.Substring(0, 1))
        Dim slotNo As Integer = CInt(slot.Name.Substring(2, 1))
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
        For Each si As ListViewItem.ListViewSubItem In slot.SubItems
            si.Text = ""
        Next
        slot.Text = "<Empty>"
        slot.ImageIndex = -1
        lvwSlots.EndUpdate()
        If updateShip = True Then
            currentInfo.ShipType = currentShip
            currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
            Call UpdateShipDetails()
            Call Me.UpdateAllSlotLocations()
        End If
    End Sub
#End Region

#Region "UI Routines"
    Private Sub btnToggleStorage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnToggleStorage.Click
        If SplitContainer1.Panel2Collapsed = True Then
            UpdateDrones = True
            SplitContainer1.Panel2Collapsed = False
            SplitContainer1.SplitterDistance = SplitContainer1.Height - 180
            Me.RedrawDroneBay()
            Me.RedrawCargoBay()
            UpdateDrones = False
        Else
            SplitContainer1.Panel2Collapsed = True
        End If
    End Sub
    Private Sub ctxSlots_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxSlots.Opening
        If cancelSlotMenu = False Then
            ctxSlots.Items.Clear()
            If lvwSlots.SelectedItems.Count > 0 Then
                Dim currentMod As New ShipModule
                Dim chargeName As String = ""
                If lvwSlots.SelectedItems.Count = 1 Then
                    ' Get the module details
                    Dim modID As String = CStr(ModuleLists.moduleListName.Item(lvwSlots.SelectedItems(0).Text))
                    Dim slotType As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(0, 1))
                    Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
                    Select Case slotType
                        Case 1 ' Rig
                            currentMod = currentShip.RigSlot(slotNo)
                        Case 2 ' Low
                            currentMod = currentShip.LowSlot(slotNo)
                        Case 4 ' Mid
                            currentMod = currentShip.MidSlot(slotNo)
                        Case 8 ' High
                            currentMod = currentShip.HiSlot(slotNo)
                    End Select
                    If currentMod Is Nothing Then
                        Dim FindModuleMenuItem As New ToolStripMenuItem
                        FindModuleMenuItem.Name = lvwSlots.SelectedItems(0).Name.Substring(0, 1)
                        FindModuleMenuItem.Text = "Find Module To Fit"
                        AddHandler FindModuleMenuItem.Click, AddressOf Me.FindModuleToFit
                        ctxSlots.Items.Add(FindModuleMenuItem)
                    Else
                        chargeName = lvwSlots.SelectedItems(0).SubItems(1).Text
                        ' Add the Show Info menu item
                        Dim showInfoMenuItem As New ToolStripMenuItem
                        showInfoMenuItem.Name = currentMod.Name
                        showInfoMenuItem.Text = "Show Info"
                        AddHandler showInfoMenuItem.Click, AddressOf Me.ShowInfo
                        ctxSlots.Items.Add(showInfoMenuItem)
                        ' Add the Show Market Group menu item
                        Dim showMarketGroupMenuItem As New ToolStripMenuItem
                        showMarketGroupMenuItem.Name = currentMod.Name
                        showMarketGroupMenuItem.Text = "Show Module Market Group"
                        AddHandler showMarketGroupMenuItem.Click, AddressOf Me.ShowModuleMarketGroup
                        ctxSlots.Items.Add(showMarketGroupMenuItem)
                        ' Add the Add to Favourites menu item
                        Dim AddToFavourtiesMenuItem As New ToolStripMenuItem
                        AddToFavourtiesMenuItem.Name = currentMod.Name
                        AddToFavourtiesMenuItem.Text = "Add To Favourites"
                        If Settings.HQFSettings.Favourites.Contains(currentMod.Name) = True Then
                            AddToFavourtiesMenuItem.Enabled = False
                        Else
                            AddToFavourtiesMenuItem.Enabled = True
                        End If
                        AddHandler AddToFavourtiesMenuItem.Click, AddressOf Me.AddModuleToFavourites
                        ctxSlots.Items.Add(AddToFavourtiesMenuItem)
                        ' Check for Relevant Skills in Modules/Charges
                        Dim RelModuleSkills, RelChargeSkills As New ArrayList
                        Dim Affects(3) As String
                        For Each Affect As String In currentMod.Affects
                            If Affect.Contains(";Skill;") = True Then
                                Affects = Affect.Split((";").ToCharArray)
                                If RelModuleSkills.Contains(Affects(0)) = False Then
                                    RelModuleSkills.Add(Affects(0))
                                End If
                            End If
                            If Affect.Contains(";Ship Bonus;") = True Then
                                Affects = Affect.Split((";").ToCharArray)
                                If ShipCurrent.Name = Affects(0) Then
                                    If RelModuleSkills.Contains(Affects(3)) = False Then
                                        RelModuleSkills.Add(Affects(3))
                                    End If
                                End If
                            End If
                        Next
                        If currentMod.LoadedCharge IsNot Nothing Then
                            For Each Affect As String In currentMod.LoadedCharge.Affects
                                If Affect.Contains(";Skill;") = True Then
                                    Affects = Affect.Split((";").ToCharArray)
                                    If RelChargeSkills.Contains(Affects(0)) = False Then
                                        RelChargeSkills.Add(Affects(0))
                                    End If
                                End If
                                If Affect.Contains(";Ship Bonus;") = True Then
                                    Affects = Affect.Split((";").ToCharArray)
                                    If ShipCurrent.Name = Affects(0) Then
                                        If RelChargeSkills.Contains(Affects(3)) = False Then
                                            RelChargeSkills.Add(Affects(3))
                                        End If
                                    End If
                                End If
                            Next
                        End If
                        If RelModuleSkills.Count > 0 Or RelChargeSkills.Count > 0 Then
                            ' Add the Main menu item
                            Dim AlterRelevantSkills As New ToolStripMenuItem
                            AlterRelevantSkills.Name = currentMod.Name
                            AlterRelevantSkills.Text = "Alter Relevant Skills"
                            For Each relSkill As String In RelModuleSkills
                                Dim newRelSkill As New ToolStripMenuItem
                                newRelSkill.Name = relSkill
                                newRelSkill.Text = relSkill
                                Dim pilotLevel As Integer = CType(CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet(relSkill), HQFSkill).Level
                                newRelSkill.Image = CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString), Image)
                                For skillLevel As Integer = 0 To 5
                                    Dim newRelSkillLevel As New ToolStripMenuItem
                                    newRelSkillLevel.Name = relSkill & skillLevel.ToString
                                    newRelSkillLevel.Text = "Level " & skillLevel.ToString
                                    If skillLevel = pilotLevel Then
                                        newRelSkillLevel.Checked = True
                                    End If
                                    AddHandler newRelSkillLevel.Click, AddressOf Me.SetPilotSkillLevel
                                    newRelSkill.DropDownItems.Add(newRelSkillLevel)
                                Next
                                newRelSkill.DropDownItems.Add("-")
                                Dim defaultLevel As Integer = CType(CType(EveHQ.Core.HQ.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(relSkill), EveHQ.Core.Skills).Level
                                Dim newRelSkillDefault As New ToolStripMenuItem
                                newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                                newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                                AddHandler newRelSkillDefault.Click, AddressOf Me.SetPilotSkillLevel
                                newRelSkill.DropDownItems.Add(newRelSkillDefault)
                                AlterRelevantSkills.DropDownItems.Add(newRelSkill)
                            Next
                            If AlterRelevantSkills.DropDownItems.Count > 0 And RelChargeSkills.Count > 0 Then
                                AlterRelevantSkills.DropDownItems.Add("-")
                            End If
                            For Each relSkill As String In RelChargeSkills
                                Dim newRelSkill As New ToolStripMenuItem
                                newRelSkill.Name = relSkill
                                newRelSkill.Text = relSkill
                                Dim pilotLevel As Integer = CType(CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet(relSkill), HQFSkill).Level
                                newRelSkill.Image = CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString), Image)
                                For skillLevel As Integer = 0 To 5
                                    Dim newRelSkillLevel As New ToolStripMenuItem
                                    newRelSkillLevel.Name = relSkill & skillLevel.ToString
                                    newRelSkillLevel.Text = "Level " & skillLevel.ToString
                                    If skillLevel = pilotLevel Then
                                        newRelSkillLevel.Checked = True
                                    End If
                                    AddHandler newRelSkillLevel.Click, AddressOf Me.SetPilotSkillLevel
                                    newRelSkill.DropDownItems.Add(newRelSkillLevel)
                                Next
                                newRelSkill.DropDownItems.Add("-")
                                Dim defaultLevel As Integer = CType(CType(EveHQ.Core.HQ.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(relSkill), EveHQ.Core.Skills).Level
                                Dim newRelSkillDefault As New ToolStripMenuItem
                                newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                                newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                                AddHandler newRelSkillDefault.Click, AddressOf Me.SetPilotSkillLevel
                                newRelSkill.DropDownItems.Add(newRelSkillDefault)
                                AlterRelevantSkills.DropDownItems.Add(newRelSkill)
                            Next
                            ctxSlots.Items.Add(AlterRelevantSkills)
                        End If
                        ' Add the Remove Module item
                        Dim RemoveModuleMenuItem As New ToolStripMenuItem
                        RemoveModuleMenuItem.Name = currentMod.Name
                        RemoveModuleMenuItem.Text = "Remove " & currentMod.Name
                        RemoveModuleMenuItem.ShortcutKeys = Keys.Delete
                        AddHandler RemoveModuleMenuItem.Click, AddressOf Me.RemoveModules
                        ctxSlots.Items.Add(RemoveModuleMenuItem)
                        ' Add the Status menu item
                        If rigGroups.Contains(CInt(currentMod.DatabaseGroup)) = False Then
                            Dim canDeactivate As Boolean = False
                            Dim canOverload As Boolean = False
                            ctxSlots.Items.Add("-")
                            Dim statusMenuItem As New ToolStripMenuItem
                            statusMenuItem.Name = lvwSlots.SelectedItems(0).Name
                            statusMenuItem.Text = "Set Module Status"
                            ' Check for activation cost
                            If currentMod.Attributes.Contains("6") = True Or currentMod.Attributes.Contains("669") Or currentMod.IsTurret Or currentMod.IsLauncher Then
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
                Else
                    Dim modulesPresent As Boolean = False
                    For Each slot As ListViewItem In lvwSlots.SelectedItems
                        If slot.Text <> "<Empty>" Then
                            modulesPresent = True
                            Dim modID As String = CStr(ModuleLists.moduleListName.Item(slot.Text))
                            Dim slotType As Integer = CInt(slot.Name.Substring(0, 1))
                            Dim slotNo As Integer = CInt(slot.Name.Substring(2, 1))
                            Select Case slotType
                                Case 1 ' Rig
                                    currentMod = currentShip.RigSlot(slotNo)
                                Case 2 ' Low
                                    currentMod = currentShip.LowSlot(slotNo)
                                Case 4 ' Mid
                                    currentMod = currentShip.MidSlot(slotNo)
                                Case 8 ' High
                                    currentMod = currentShip.HiSlot(slotNo)
                            End Select
                            chargeName = slot.SubItems(1).Text
                            Exit For
                        End If
                    Next
                    If modulesPresent = True Then
                        ' Add the Remove Module item
                        Dim RemoveModuleMenuItem As New ToolStripMenuItem
                        RemoveModuleMenuItem.Name = "RemoveMods"
                        RemoveModuleMenuItem.Text = "Remove Modules"
                        RemoveModuleMenuItem.ShortcutKeys = Keys.Delete
                        AddHandler RemoveModuleMenuItem.Click, AddressOf Me.RemoveModules
                        ctxSlots.Items.Add(RemoveModuleMenuItem)
                    End If
                End If

                ' Calculate all the charge information
                ' Check if we have the same collection of modules and therefore can accept the same charges
                Dim ShowCharges As Boolean = True
                Dim AmmoAnalysis As ShipModule = currentMod
                Dim cMod As New ShipModule
                If lvwSlots.SelectedItems.Count > 1 Then
                    Dim marketGroup As String = currentMod.MarketGroup
                    For Each selItems As ListViewItem In lvwSlots.SelectedItems
                        If selItems.Text <> "<Empty>" Then
                            cMod = CType(ModuleLists.moduleList(CStr(ModuleLists.moduleListName.Item(selItems.Text))), ShipModule)
                            If cMod.MarketGroup <> marketGroup Then
                                AmmoAnalysis = Nothing
                                ShowCharges = False
                                Exit For
                            Else
                                AmmoAnalysis = cMod
                            End If
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
                                    Dim chargeMod As ShipModule = CType(ModuleLists.moduleList(ModuleLists.moduleListName(charge)), ShipModule)
                                    If chargeMod.Volume <= currentMod.Capacity Then
                                        newCharge.Name = CStr(ModuleLists.moduleListName(charge))
                                        newCharge.Text = charge
                                        AddHandler newCharge.Click, AddressOf Me.LoadChargeIntoModule
                                        newGroup.DropDownItems.Add(newCharge)
                                    End If
                                End If
                            Next
                            ctxSlots.Items.Add(newGroup)
                        Next
                    End If
                    ' Add an "Ammo Analysis" option - the old Gunnery tool feature
                    If AmmoAnalysis.IsTurret Or AmmoAnalysis.IsLauncher Then
                        ctxSlots.Items.Add("-")
                        Dim AmmoInfo As New ToolStripMenuItem
                        AmmoInfo.Name = "AmmoInfo"
                        AmmoInfo.Text = "Ammo Analysis"
                        AddHandler AmmoInfo.Click, AddressOf Me.AnalyseAmmo
                        ctxSlots.Items.Add(AmmoInfo)
                    End If
                End If

            Else
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If

        ' Cancel the menu if there is nothing to display
        If ctxSlots.Items.Count = 0 Then
            e.Cancel = True
        End If

    End Sub
    Private Sub FindModuleToFit(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectedSlot As ListViewItem = lvwSlots.SelectedItems(0)
        Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
        Dim modData As New ArrayList
        modData.Add(slotInfo(0))
        modData.Add(fittedShip.CPU - fittedShip.CPU_Used)
        modData.Add(fittedShip.PG - fittedShip.PG_Used)
        modData.Add(fittedShip.Calibration - fittedShip.Calibration_Used)
        modData.Add(fittedShip.LauncherSlots - fittedShip.LauncherSlots_Used)
        modData.Add(fittedShip.TurretSlots - fittedShip.TurretSlots_Used)
        HQF.HQFEvents.StartFindModule = modData
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
        Dim hPilot As EveHQ.Core.Pilot
        If currentInfo IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.Pilots(currentInfo.cboPilots.SelectedItem), Core.Pilot)
        Else
            hPilot = EveHQ.Core.HQ.myPilot
        End If
        showInfo.ShowItemDetails(sModule, hPilot)
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
        Dim hPilot As EveHQ.Core.Pilot
        If currentInfo IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.Pilots(currentInfo.cboPilots.SelectedItem), Core.Pilot)
        Else
            hPilot = EveHQ.Core.HQ.myPilot
        End If

        showInfo.ShowItemDetails(sModule, hPilot)
        showInfo = Nothing
    End Sub
    Private Sub AnalyseAmmo(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Display the ammo types available by this module
        Dim AmmoAnalysis As New frmGunnery
        AmmoAnalysis.CurrentShip = currentShip
        AmmoAnalysis.CurrentPilot = CType(HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQFPilot)
        AmmoAnalysis.CurrentSlot = lvwSlots.SelectedItems(0).Name
        AmmoAnalysis.ShowDialog()
        AmmoAnalysis.Dispose()
    End Sub
    Private Sub ShowModuleMarketGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ShowMarketMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim moduleName As String = ShowMarketMenu.Name
        Dim moduleID As String = CStr(ModuleLists.moduleListName(moduleName))
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim pathLine As String = CStr(Market.MarketGroupPath(cModule.MarketGroup))
        ShipModule.DisplayedMarketGroup = pathLine
    End Sub
    Private Sub AddModuleToFavourites(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ShowMarketMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim moduleName As String = ShowMarketMenu.Name
        Dim moduleID As String = CStr(ModuleLists.moduleListName(moduleName))
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        If Settings.HQFSettings.Favourites.Contains(cModule.Name) = False Then
            Settings.HQFSettings.Favourites.Add(cModule.Name)
            HQFEvents.StartUpdateModuleList = True
        End If
    End Sub
    Private Sub pbShipInfo_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbShipInfo.MouseHover
        ToolTip1.SetToolTip(pbShipInfo, currentShip.Description)
    End Sub
    Private Sub SetPilotSkillLevel(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mnuPilotLevel As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim hPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQFPilot)
        Dim pilotSkill As HQFSkill = CType(hPilot.SkillSet(mnuPilotLevel.Name.Substring(0, mnuPilotLevel.Name.Length - 1)), HQFSkill)
        Dim level As Integer = CInt(mnuPilotLevel.Name.Substring(mnuPilotLevel.Name.Length - 1))
        If level <> pilotSkill.Level Then
            pilotSkill.Level = level
            currentInfo.ShipType = currentShip
            currentInfo.BuildMethod = BuildType.BuildEverything
            Call Me.UpdateAllSlotLocations()
        End If
    End Sub
#End Region

#Region "Set Module Status"
    Private Sub SetModuleOffline(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        sModule.ModuleState = ModuleStates.Offline
        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
        Call Me.UpdateSlotLocation(sModule, slotNo)
    End Sub
    Private Sub SetModuleInactive(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        sModule.ModuleState = ModuleStates.Inactive
        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
        Call Me.UpdateSlotLocation(sModule, slotNo)
    End Sub
    Private Sub SetModuleActive(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        sModule.ModuleState = ModuleStates.Active
        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
        Call Me.UpdateSlotLocation(sModule, slotNo)
    End Sub
    Private Sub SetModuleOverload(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim slotNo As Integer = CInt(lvwSlots.SelectedItems(0).Name.Substring(2, 1))
        sModule.ModuleState = ModuleStates.Overloaded
        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
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
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
        Call UpdateAllSlotLocations()
    End Sub
    Private Sub LoadChargeIntoModule(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ChargeMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim moduleID As String = ChargeMenu.Name
        ' Get name of the "slot" which has slot type and number
        For Each selItem As ListViewItem In lvwSlots.SelectedItems
            If selItem.Text <> "<Empty>" Then
                Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule).Clone
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
            End If
        Next
        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
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
        Call Me.RedrawCargoBayCapacity()
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
        Call Me.RedrawDroneBayCapacity()
    End Sub

    Private Sub RedrawCargoBayCapacity()
        lblCargoBay.Text = FormatNumber(currentShip.CargoBay_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.CargoBay, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m³"
        If fittedShip.CargoBay > 0 Then
            pbCargoBay.MaxValue = CInt(fittedShip.CargoBay)
        Else
            pbCargoBay.MaxValue = 1
        End If
        If currentShip.CargoBay_Used > fittedShip.CargoBay Then
            pbCargoBay.Value = CInt(fittedShip.CargoBay)
            pbCargoBay.StartColor = Drawing.Color.Red
            pbCargoBay.EndColor = Drawing.Color.Red
            pbCargoBay.HighlightColor = Drawing.Color.White
            pbCargoBay.GlowColor = Drawing.Color.LightPink
        Else
            pbCargoBay.Value = CInt(currentShip.CargoBay_Used)
            pbCargoBay.StartColor = Drawing.Color.LimeGreen
            pbCargoBay.EndColor = Drawing.Color.LimeGreen
            pbCargoBay.HighlightColor = Drawing.Color.White
            pbCargoBay.GlowColor = Drawing.Color.LightGreen
        End If
    End Sub

    Private Sub RedrawDroneBayCapacity()
        lvwDroneBay.EndUpdate()
        lblDroneBay.Text = FormatNumber(currentShip.DroneBay_Used, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " / " & FormatNumber(fittedShip.DroneBay, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m³"
        If fittedShip.DroneBay > 0 Then
            pbDroneBay.MaxValue = CInt(fittedShip.DroneBay)
        Else
            pbDroneBay.MaxValue = 1
        End If
        If currentShip.DroneBay_Used > fittedShip.DroneBay Then
            pbDroneBay.Value = CInt(fittedShip.DroneBay)
            pbDroneBay.StartColor = Drawing.Color.Red
            pbDroneBay.EndColor = Drawing.Color.Red
            pbDroneBay.HighlightColor = Drawing.Color.White
            pbDroneBay.GlowColor = Drawing.Color.LightPink
        Else
            pbDroneBay.Value = CInt(currentShip.DroneBay_Used)
            pbDroneBay.StartColor = Drawing.Color.LimeGreen
            pbDroneBay.EndColor = Drawing.Color.LimeGreen
            pbDroneBay.HighlightColor = Drawing.Color.White
            pbDroneBay.GlowColor = Drawing.Color.LightGreen
        End If
    End Sub

    Private Sub lvwDroneBay_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwDroneBay.ItemChecked
        If cancelDroneActivation = False Then
            Dim idx As Integer = CInt(e.Item.Name)
            Dim DBI As DroneBayItem = CType(currentShip.DroneBayItems.Item(idx), DroneBayItem)
            ' Check we have the bandwidth and/or control ability for this item
            If UpdateDrones = False Then
                Dim reqQ As Integer = DBI.Quantity
                If e.Item.Checked = True Then
                    If fittedShip.UsedDrones + reqQ > fittedShip.MaxDrones Then
                        ' Cannot do this because our drone control skill in insufficient
                        MessageBox.Show("You do not have the ability to control this many drones. Please split the group and try again.", "Drone Control Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        cancelDroneActivation = True
                        e.Item.Checked = False
                        Exit Sub
                    End If
                    If fittedShip.DroneBandwidth_Used + (reqQ * CDbl(DBI.DroneType.Attributes("1272"))) > fittedShip.DroneBandwidth Then
                        ' Cannot do this because we don't have enough bandwidth
                        MessageBox.Show("You do not have the spare bandwidth to control this many drones. Please split the group and try again.", "Drone Bandwidth Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        cancelDroneActivation = True
                        e.Item.Checked = False
                        Exit Sub
                    End If
                End If
                DBI.IsActive = e.Item.Checked
                currentInfo.ShipType = currentShip
                currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
                If DBI.IsActive = True Then
                    currentShip.Attributes("10006") = CDbl(currentShip.Attributes("10006")) + reqQ
                Else
                    currentShip.Attributes("10006") = Math.Max(CDbl(currentShip.Attributes("10006")) - reqQ, 0)
                End If
            End If
            Call Me.UpdateFittingListFromShipData()
            Call currentInfo.UpdateDroneUsage()
        End If
        cancelDroneActivation = False
    End Sub

    Private Sub ctxBays_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxBays.Opening
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        If lvwBay Is lvwCargoBay Then
            ctxShowBayInfoItem.Text = "Show Item Info"
        Else
            ctxShowBayInfoItem.Text = "Show Drone Info"
        End If
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
                currentInfo.ShipType = currentShip
                currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
                Call Me.UpdateFittingListFromShipData()
                UpdateDrones = True
                Call RedrawDroneBay()
                UpdateDrones = False
        End Select
    End Sub

    Private Sub ctxShowBayInfoItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxShowBayInfoItem.Click
        Dim selItem As ListViewItem
        Dim sModule As ShipModule
        If ctxShowBayInfoItem.Text = "Show Item Info" Then
            selItem = lvwCargoBay.SelectedItems(0)
            Dim idx As Integer = CInt(selItem.Name)
            Dim DBI As CargoBayItem = CType(fittedShip.CargoBayItems.Item(idx), CargoBayItem)
            sModule = DBI.ItemType
        Else
            selItem = lvwDroneBay.SelectedItems(0)
            Dim idx As Integer = CInt(selItem.Name)
            Dim DBI As DroneBayItem = CType(fittedShip.DroneBayItems.Item(idx), DroneBayItem)
            sModule = DBI.DroneType
        End If  
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        If currentInfo IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.Pilots(currentInfo.cboPilots.SelectedItem), Core.Pilot)
        Else
            hPilot = EveHQ.Core.HQ.myPilot
        End If
        showInfo.ShowItemDetails(sModule, hPilot)
        showInfo = Nothing
    End Sub

    Private Sub ctxAlterQuantity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxAlterQuantity.Click
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        Select Case lvwBay.Name
            Case "lvwCargoBay"
                Dim selItem As ListViewItem = lvwCargoBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim CBI As CargoBayItem = CType(currentShip.CargoBayItems.Item(idx), CargoBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = fittedShip
                newSelectForm.CBI = CBI
                newSelectForm.IsDroneBay = False
                newSelectForm.IsSplit = False
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = CBI.Quantity + CInt(Int((fittedShip.CargoBay - currentShip.CargoBay_Used) / CBI.ItemType.Volume))
                newSelectForm.nudQuantity.Value = CBI.Quantity
                newSelectForm.ShowDialog()
                newSelectForm.Dispose()
                Call Me.UpdateFittingListFromShipData()
                Call RedrawCargoBay()
            Case "lvwDroneBay"
                Dim selItem As ListViewItem = lvwDroneBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim DBI As DroneBayItem = CType(currentShip.DroneBayItems.Item(idx), DroneBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = fittedShip
                newSelectForm.DBI = DBI
                newSelectForm.IsDroneBay = True
                newSelectForm.IsSplit = False
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = DBI.Quantity + CInt(Int((fittedShip.DroneBay - currentShip.DroneBay_Used) / DBI.DroneType.Volume))
                newSelectForm.nudQuantity.Value = DBI.Quantity
                newSelectForm.ShowDialog()
                currentInfo.ShipType = currentShip
                currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
                Call Me.UpdateFittingListFromShipData()
                UpdateDrones = True
                Call RedrawDroneBay()
                UpdateDrones = False
                newSelectForm.Dispose()
        End Select
    End Sub

    Private Sub ctxSplitBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxSplitBatch.Click
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        Select Case lvwBay.Name
            Case "lvwCargoBay"
                Dim selItem As ListViewItem = lvwCargoBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim CBI As CargoBayItem = CType(currentShip.CargoBayItems.Item(idx), CargoBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = fittedShip
                newSelectForm.currentShip = currentShip
                newSelectForm.CBI = CBI
                newSelectForm.IsDroneBay = False
                newSelectForm.IsSplit = True
                newSelectForm.nudQuantity.Value = 1
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = CBI.Quantity - 1
                newSelectForm.ShowDialog()
                newSelectForm.Dispose()
                Call Me.UpdateFittingListFromShipData()
                Call RedrawCargoBay()
            Case "lvwDroneBay"
                Dim selItem As ListViewItem = lvwDroneBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim DBI As DroneBayItem = CType(currentShip.DroneBayItems.Item(idx), DroneBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = fittedShip
                newSelectForm.currentShip = currentShip
                newSelectForm.DBI = DBI
                newSelectForm.IsDroneBay = True
                newSelectForm.IsSplit = True
                newSelectForm.nudQuantity.Value = 1
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = DBI.Quantity - 1
                newSelectForm.ShowDialog()
                currentInfo.ShipType = currentShip
                currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
                Call Me.UpdateFittingListFromShipData()
                UpdateDrones = True
                Call RedrawDroneBay()
                UpdateDrones = False
                newSelectForm.Dispose()
        End Select
    End Sub

    Private Sub btnMergeDrones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMergeDrones.Click
        UpdateDrones = True
        lvwDroneBay.BeginUpdate()
        lvwDroneBay.Items.Clear()
        currentShip.DroneBay_Used = 0
        Dim DBI As DroneBayItem
        Dim HoldingBay As New SortedList
        Dim DroneQuantities As New SortedList
        For Each DBI In currentShip.DroneBayItems.Values
            If HoldingBay.Contains(DBI.DroneType.Name) = False Then
                HoldingBay.Add(DBI.DroneType.Name, DBI.DroneType)
            End If
            If DroneQuantities.Contains(DBI.DroneType.Name) = True Then
                Dim CQ As Integer = CInt(DroneQuantities(DBI.DroneType.Name))
                DroneQuantities(DBI.DroneType.Name) = CQ + DBI.Quantity
            Else
                DroneQuantities.Add(DBI.DroneType.Name, DBI.Quantity)
            End If
        Next
        currentShip.DroneBayItems.Clear()
        For Each drone As String In HoldingBay.Keys
            DBI = New DroneBayItem
            DBI.DroneType = CType(HoldingBay(drone), ShipModule)
            DBI.IsActive = False
            DBI.Quantity = CInt(DroneQuantities(drone))
            Dim newDroneItem As New ListViewItem(DBI.DroneType.Name)
            newDroneItem.Name = CStr(lvwDroneBay.Items.Count)
            newDroneItem.SubItems.Add(CStr(DBI.Quantity))
            currentShip.DroneBayItems.Add(lvwDroneBay.Items.Count, DBI)
            currentShip.DroneBay_Used += DBI.DroneType.Volume * DBI.Quantity
            lvwDroneBay.Items.Add(newDroneItem)
        Next
        lvwDroneBay.EndUpdate()
        Call Me.RedrawDroneBayCapacity()
        UpdateDrones = False
        ' Rebuild the ship to account for any disabled drones
        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
    End Sub

    Private Sub btnMergeCargo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMergeCargo.Click
        lvwCargoBay.BeginUpdate()
        lvwCargoBay.Items.Clear()
        currentShip.CargoBay_Used = 0
        Dim CBI As CargoBayItem
        Dim HoldingBay As New SortedList
        Dim CargoQuantities As New SortedList
        For Each CBI In currentShip.CargoBayItems.Values
            If HoldingBay.Contains(CBI.ItemType.Name) = False Then
                HoldingBay.Add(CBI.ItemType.Name, CBI.ItemType)
            End If
            If CargoQuantities.Contains(CBI.ItemType.Name) = True Then
                Dim CQ As Integer = CInt(CargoQuantities(CBI.ItemType.Name))
                CargoQuantities(CBI.ItemType.Name) = CQ + CBI.Quantity
            Else
                CargoQuantities.Add(CBI.ItemType.Name, CBI.Quantity)
            End If
        Next
        currentShip.CargoBayItems.Clear()
        For Each Cargo As String In HoldingBay.Keys
            CBI = New CargoBayItem
            CBI.ItemType = CType(HoldingBay(Cargo), ShipModule)
            CBI.Quantity = CInt(CargoQuantities(Cargo))
            Dim newCargoItem As New ListViewItem(CBI.ItemType.Name)
            newCargoItem.Name = CStr(lvwCargoBay.Items.Count)
            newCargoItem.SubItems.Add(CStr(CBI.Quantity))
            currentShip.CargoBayItems.Add(lvwCargoBay.Items.Count, CBI)
            currentShip.CargoBay_Used += CBI.ItemType.Volume * CBI.Quantity
            lvwCargoBay.Items.Add(newCargoItem)
        Next
        lvwCargoBay.EndUpdate()
        Call Me.RedrawCargoBayCapacity()
    End Sub

#End Region

#Region "Slot Drag/Drop Routines"

    Private Sub lvwSlots_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvwSlots.MouseDown
        Dim hti As ListViewHitTestInfo = lvwSlots.HitTest(e.X, e.Y)
        If hti.Item IsNot Nothing Then
            If e.Button = Windows.Forms.MouseButtons.Right Then
                If hti.Location = ListViewHitTestLocations.Image Then
                    cancelSlotMenu = True ' Should cancel the menu and any double-click event?
                    ' Get the module details
                    Dim modID As String = CStr(ModuleLists.moduleListName.Item(hti.Item.Text))
                    Dim currentMod As New ShipModule
                    Dim slotType As Integer = CInt(hti.Item.Name.Substring(0, 1))
                    Dim slotNo As Integer = CInt(hti.Item.Name.Substring(2, 1))
                    Dim canOffline As Boolean = True
                    Select Case slotType
                        Case 1 ' Rig
                            currentMod = currentShip.RigSlot(slotNo)
                            canOffline = False
                        Case 2 ' Low
                            currentMod = currentShip.LowSlot(slotNo)
                        Case 4 ' Mid
                            currentMod = currentShip.MidSlot(slotNo)
                        Case 8 ' High
                            currentMod = currentShip.HiSlot(slotNo)
                    End Select

                    If currentMod IsNot Nothing Then
                        Dim currentstate As Integer = currentMod.ModuleState
                        ' Check for status
                        Dim canDeactivate As Boolean = False
                        Dim canOverload As Boolean = False
                        ' Check for activation cost
                        If currentMod.Attributes.Contains("6") = True Or currentMod.Attributes.Contains("669") Or currentMod.IsTurret Or currentMod.IsLauncher Then
                            canDeactivate = True
                        End If
                        If currentMod.Attributes.Contains("1211") = True Then
                            canOverload = True
                        End If
                        currentstate *= 2
                        Dim changedstate As Boolean = False
                        Do
                            changedstate = False
                            If currentstate > 8 Then
                                currentstate = 1
                                changedstate = True
                            End If
                            If currentstate = ModuleStates.Offline And canOffline = False Then
                                currentstate *= 2
                                changedstate = True
                            End If
                            If currentstate = ModuleStates.Inactive And canDeactivate = False Then
                                currentstate *= 2
                                changedstate = True
                            End If
                            If currentstate = ModuleStates.Overloaded And canOverload = False Then
                                currentstate *= 2
                                changedstate = True
                            End If
                        Loop Until changedstate = False

                        ' Update only if the module state has changed
                        If currentstate <> currentMod.ModuleState Then
                            currentMod.ModuleState = currentstate
                            currentInfo.ShipType = currentShip
                            currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
                            Call Me.UpdateAllSlotLocations()
                            'Call Me.UpdateSlotLocation(currentMod, slotNo)
                            'MessageBox.Show("Changing to State: " & currentMod.ModuleState)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub lvwSlots_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwSlots.DragOver
        Dim oLVI As ListViewItem = CType(e.Data.GetData(GetType(ListViewItem)), ListViewItem)
        Dim oModID As String = CStr(ModuleLists.moduleListName.Item(oLVI.Text))
        Dim oMod As New ShipModule
        Dim oSlotType As Integer = CInt(oLVI.Name.Substring(0, 1))
        Dim oslotNo As Integer = CInt(oLVI.Name.Substring(2, 1))

        Dim p As Point = lvwSlots.PointToClient(New Point(e.X, e.Y))
        Dim nLVI As ListViewItem = lvwSlots.GetItemAt(p.X, p.Y)
        If nLVI IsNot Nothing Then
            Dim nModID As String = CStr(ModuleLists.moduleListName.Item(nLVI.Text))
            Dim nMod As New ShipModule
            Dim nSlotType As Integer = CInt(nLVI.Name.Substring(0, 1))
            Dim nslotNo As Integer = CInt(nLVI.Name.Substring(2, 1))
            If oSlotType <> nSlotType Then
                e.Effect = DragDropEffects.None
            Else
                If oslotNo = nslotNo Then
                    e.Effect = DragDropEffects.None
                Else
                    If e.KeyState = 1 Then
                        e.Effect = DragDropEffects.Move
                    Else
                        e.Effect = DragDropEffects.Copy
                    End If
                End If
            End If
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub

    Private Sub lvwSlots_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwSlots.DragDrop
        Dim oLVI As ListViewItem = CType(e.Data.GetData(GetType(ListViewItem)), ListViewItem)
        Dim oModID As String = CStr(ModuleLists.moduleListName.Item(oLVI.Text))

        Dim oSlotType As Integer = CInt(oLVI.Name.Substring(0, 1))
        Dim oslotNo As Integer = CInt(oLVI.Name.Substring(2, 1))

        Dim p As Point = lvwSlots.PointToClient(New Point(e.X, e.Y))
        Dim nLVI As ListViewItem = lvwSlots.GetItemAt(p.X, p.Y)
        Dim nModID As String = CStr(ModuleLists.moduleListName.Item(nLVI.Text))

        Dim nSlotType As Integer = CInt(nLVI.Name.Substring(0, 1))
        Dim nslotNo As Integer = CInt(nLVI.Name.Substring(2, 1))

        Dim ocMod As New ShipModule
        Dim ncMod As New ShipModule
        Dim ofMod As New ShipModule
        Dim nfMod As New ShipModule

        If oSlotType <> nSlotType Then
            e.Effect = DragDropEffects.None
        Else
            If oslotNo = nslotNo Then
                e.Effect = DragDropEffects.None
            Else
                If oLVI.Text = "<Empty>" Then
                    ocMod = Nothing
                    ofMod = Nothing
                Else
                    Select Case oSlotType
                        Case 1 ' Rig
                            ocMod = currentShip.RigSlot(oslotNo).Clone
                            ofMod = fittedShip.RigSlot(oslotNo).Clone
                        Case 2 ' Low
                            ocMod = currentShip.LowSlot(oslotNo).Clone
                            ofMod = fittedShip.LowSlot(oslotNo).Clone
                        Case 4 ' Mid
                            ocMod = currentShip.MidSlot(oslotNo).Clone
                            ofMod = fittedShip.MidSlot(oslotNo).Clone
                        Case 8 ' High
                            ocMod = currentShip.HiSlot(oslotNo).Clone
                            ofMod = fittedShip.HiSlot(oslotNo).Clone
                    End Select
                End If

                If nLVI.Text = "<Empty>" Then
                    ncMod = Nothing
                    nfMod = Nothing
                Else
                    Select Case nSlotType
                        Case 1 ' Rig
                            ncMod = currentShip.RigSlot(nslotNo).Clone
                            nfMod = fittedShip.RigSlot(nslotNo).Clone
                        Case 2 ' Low
                            ncMod = currentShip.LowSlot(nslotNo).Clone
                            nfMod = fittedShip.LowSlot(nslotNo).Clone
                        Case 4 ' Mid
                            ncMod = currentShip.MidSlot(nslotNo).Clone
                            nfMod = fittedShip.MidSlot(nslotNo).Clone
                        Case 8 ' High
                            ncMod = currentShip.HiSlot(nslotNo).Clone
                            nfMod = fittedShip.HiSlot(nslotNo).Clone
                    End Select
                End If
                If e.Effect = DragDropEffects.Move Then ' Mouse button released?
                    'MessageBox.Show("Wanting to swap " & oLVI.Text & " for " & nLVI.Text & "?", "Confirm swap", MessageBoxButtons.OK, MessageBoxIcon.Question)
                    If ocMod Is Nothing Then
                        RemoveModule(nLVI, False)
                    Else
                        ocMod.SlotNo = nslotNo
                        Select Case nSlotType
                            Case 1 ' Rig
                                currentShip.RigSlot(nslotNo) = ocMod
                                fittedShip.RigSlot(nslotNo) = ofMod
                            Case 2 ' Low
                                currentShip.LowSlot(nslotNo) = ocMod
                                fittedShip.LowSlot(nslotNo) = ofMod
                            Case 4 ' Mid
                                currentShip.MidSlot(nslotNo) = ocMod
                                fittedShip.MidSlot(nslotNo) = ofMod
                            Case 8 ' High
                                currentShip.HiSlot(nslotNo) = ocMod
                                fittedShip.HiSlot(nslotNo) = ofMod
                        End Select
                    End If
                    If ncMod Is Nothing Then
                        RemoveModule(oLVI, False)
                    Else
                        ncMod.SlotNo = oslotNo
                        Select Case oSlotType
                            Case 1 ' Rig
                                currentShip.RigSlot(oslotNo) = ncMod
                                fittedShip.RigSlot(oslotNo) = nfMod
                            Case 2 ' Low
                                currentShip.LowSlot(oslotNo) = ncMod
                                fittedShip.LowSlot(oslotNo) = nfMod
                            Case 4 ' Mid
                                currentShip.MidSlot(oslotNo) = ncMod
                                fittedShip.MidSlot(oslotNo) = nfMod
                            Case 8 ' High
                                currentShip.HiSlot(oslotNo) = ncMod
                                fittedShip.HiSlot(oslotNo) = nfMod
                        End Select
                    End If
                    Call Me.UpdateSlotLocation(ofMod, nslotNo)
                    Call Me.UpdateSlotLocation(nfMod, oslotNo)
                Else
                    'MessageBox.Show("Wanting to copy " & oLVI.Text & " for " & nLVI.Text & "?", "Confirm copy", MessageBoxButtons.OK, MessageBoxIcon.Question)
                    Dim rMod As ShipModule = Nothing
                    If ncMod IsNot Nothing Then
                        rMod = ncMod.Clone()
                    End If
                    If ocMod IsNot Nothing Then
                        ncMod = ocMod.Clone
                        AddModule(ncMod, nslotNo, False, rMod)
                    Else
                        RemoveModule(nLVI, False)
                    End If
                    currentInfo.ShipType = currentShip
                    currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
                    Call Me.UpdateAllSlotLocations()
                End If
            End If
        End If
    End Sub

    Private Sub lvwSlots_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lvwSlots.ItemDrag
        lvwSlots.DoDragDrop(e.Item, DragDropEffects.Copy Or DragDropEffects.Move)
    End Sub
#End Region

#Region "Remote Effects"

    Private Sub btnUpdateRemoteEffects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateRemoteEffects.Click
        ' Check if we have any remote fittings and if so, generate the fitting
        If lvwRemoteFittings.Items.Count > 0 Then
            lvwRemoteEffects.Tag = "Refresh"
            lvwRemoteEffects.BeginUpdate()
            lvwRemoteEffects.Items.Clear()
            For Each remoteFitting As ListViewItem In lvwRemoteFittings.CheckedItems
                ' Let's try and generate a fitting and get some module info
                Dim shipFit As String = remoteFitting.Tag.ToString
                Dim fittingSep As Integer = shipFit.IndexOf(", ")
                Dim shipName As String = shipFit.Substring(0, fittingSep)
                Dim fittingName As String = shipFit.Substring(fittingSep + 2)
                Dim pShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
                pShip = Engine.UpdateShipDataFromFittingList(pShip, CType(Fittings.FittingList(shipFit), ArrayList))
                Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(remoteFitting.Name.Substring(shipFit.Length + 2)), HQFPilot)
                Dim remoteShip As Ship = Engine.ApplyFitting(pShip, pPilot)
                pShip = Nothing
                For Each remoteModule As ShipModule In remoteShip.SlotCollection
                    If remoteGroups.Contains(CInt(remoteModule.DatabaseGroup)) = True Then
                        remoteModule.ModuleState = 16
                        remoteModule.SlotNo = 0
                        Dim newRemoteItem As New ListViewItem
                        newRemoteItem.Tag = remoteModule
                        newRemoteItem.Name = pPilot.PilotName
                        If remoteModule.LoadedCharge IsNot Nothing Then
                            newRemoteItem.Text = remoteModule.Name & " (" & remoteModule.LoadedCharge.Name & ")"
                        Else
                            newRemoteItem.Text = remoteModule.Name
                        End If
                        lvwRemoteEffects.Items.Add(newRemoteItem)
                    End If
                Next
                For Each remoteDrone As DroneBayItem In remoteShip.DroneBayItems.Values
                    If remoteGroups.Contains(CInt(remoteDrone.DroneType.DatabaseGroup)) = True Then
                        If remoteDrone.IsActive = True Then
                            remoteDrone.DroneType.ModuleState = 16
                            Dim newRemoteItem As New ListViewItem
                            newRemoteItem.Tag = remoteDrone
                            newRemoteItem.Name = pPilot.PilotName
                            newRemoteItem.Text = remoteDrone.DroneType.Name & " (x" & remoteDrone.Quantity & ")"
                            lvwRemoteEffects.Items.Add(newRemoteItem)
                        End If
                    End If
                Next
            Next
            lvwRemoteEffects.EndUpdate()
            lvwRemoteEffects.Tag = ""
            ' Update the mapping effects back to the current pilot
            currentInfo.BuildMethod = BuildType.BuildEffectsMaps
        End If
    End Sub

    Private Sub lvwRemoteEffects_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwRemoteEffects.ItemChecked
        If lvwRemoteEffects.Tag.ToString <> "Refresh" Then
            currentShip.RemoteSlotCollection.Clear()
            For Each item As ListViewItem In lvwRemoteEffects.CheckedItems
                currentShip.RemoteSlotCollection.Add(item.Tag)
            Next
            currentInfo.ShipType = currentShip
            currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
            Call Me.UpdateAllSlotLocations()
        End If
    End Sub

    Private Sub mnuShowRemoteModInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowRemoteModInfo.Click
        Dim sModule As Object = lvwRemoteEffects.SelectedItems(0).Tag
        If TypeOf sModule Is DroneBayItem Then
            sModule = CType(sModule, DroneBayItem).DroneType
        End If
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        hPilot = CType(EveHQ.Core.HQ.Pilots(lvwRemoteEffects.SelectedItems(0).Name), Core.Pilot)
        showInfo.ShowItemDetails(sModule, hPilot)
        showInfo = Nothing
    End Sub

    Private Sub ctxRemoteModule_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxRemoteModule.Opening
        If lvwRemoteEffects.SelectedItems.Count = 0 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub btnAddRemoteFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRemoteFitting.Click
        ' Check if we have a fitting and a pilot and if so, generate the fitting
        If cboFitting.SelectedItem IsNot Nothing And cboPilot.SelectedItem IsNot Nothing Then
            If lvwRemoteFittings.Items.ContainsKey(cboFitting.SelectedItem.ToString & ": " & cboPilot.SelectedItem.ToString) = False Then
                Dim newFitting As New ListViewItem
                newFitting.Name = cboFitting.SelectedItem.ToString & ": " & cboPilot.SelectedItem.ToString
                newFitting.Text = cboFitting.SelectedItem.ToString & ": " & cboPilot.SelectedItem.ToString
                newFitting.Tag = cboFitting.SelectedItem.ToString
                newFitting.Checked = True
                lvwRemoteFittings.Items.Add(newFitting)
            Else
                MessageBox.Show("Fitting and Pilot combination already exists!", "Duplicate Remote Setup Detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub RemoveFittingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFittingToolStripMenuItem.Click
        lvwRemoteFittings.BeginUpdate()
        For Each fit As ListViewItem In lvwRemoteFittings.SelectedItems
            lvwRemoteFittings.Items.Remove(fit)
        Next
        lvwRemoteFittings.EndUpdate()
    End Sub

    Private Sub ctxRemoteFittings_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxRemoteFittings.Opening
        If lvwRemoteFittings.SelectedItems.Count = 0 Then
            e.Cancel = True
        End If
    End Sub
#End Region

#Region "Fleet Effects"

    Private Sub LoadRemoteFleetInfo()
        ' Load details into the combo boxes
        cboPilot.BeginUpdate() : cboFitting.BeginUpdate()
        cboSCPilot.BeginUpdate() : cboSCShip.BeginUpdate()
        cboWCPilot.BeginUpdate() : cboWCShip.BeginUpdate()
        cboFCPilot.BeginUpdate() : cboFCShip.BeginUpdate()
        cboPilot.Items.Clear() : cboFitting.Items.Clear()
        cboSCPilot.Items.Clear() : cboSCShip.Items.Clear()
        cboWCPilot.Items.Clear() : cboWCShip.Items.Clear()
        cboFCPilot.Items.Clear() : cboFCShip.Items.Clear()
        ' Add the fittings
        For Each fitting As String In Fittings.FittingList.Keys
            cboFitting.Items.Add(fitting)
            cboSCShip.Items.Add(fitting)
            cboWCShip.Items.Add(fitting)
            cboFCShip.Items.Add(fitting)
        Next
        ' Add the pilots
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
            cboPilot.Items.Add(cPilot.Name)
            cboSCPilot.Items.Add(cPilot.Name)
            cboWCPilot.Items.Add(cPilot.Name)
            cboFCPilot.Items.Add(cPilot.Name)
        Next
        cboPilot.EndUpdate() : cboFitting.BeginUpdate()
        cboSCPilot.BeginUpdate() : cboSCShip.BeginUpdate()
        cboWCPilot.BeginUpdate() : cboWCShip.BeginUpdate()
        cboFCPilot.BeginUpdate() : cboFCShip.BeginUpdate()
    End Sub

    Private Sub cboSCPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSCPilot.SelectedIndexChanged
        ' Set the fleet status
        If cboSCPilot.SelectedIndex <> -1 Then
            lblFleetStatus.Text = "Active"
            btnLeaveFleet.Enabled = True
            lblSCShip.Enabled = True
            cboSCShip.Enabled = True
            If cboSCShip.SelectedIndex = -1 Then
                Call Me.CalculateFleetEffects()
            Else
                Call Me.UpdateSCShipEffects()
            End If
        End If
    End Sub

    Private Sub cboWCPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWCPilot.SelectedIndexChanged
        ' Set the fleet status
        If cboWCPilot.SelectedIndex <> -1 Then
            lblFleetStatus.Text = "Active"
            btnLeaveFleet.Enabled = True
            lblWCShip.Enabled = True
            cboWCShip.Enabled = True
            If cboWCShip.SelectedIndex = -1 Then
                Call Me.CalculateFleetEffects()
            Else
                Call Me.UpdateWCShipEffects()
            End If
        End If
    End Sub

    Private Sub cboFCPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFCPilot.SelectedIndexChanged
        ' Set the fleet status
        If cboFCPilot.SelectedIndex <> -1 Then
            lblFleetStatus.Text = "Active"
            btnLeaveFleet.Enabled = True
            lblFCShip.Enabled = True
            cboFCShip.Enabled = True
            If cboFCShip.SelectedIndex = -1 Then
                Call Me.CalculateFleetEffects()
            Else
                Call Me.UpdateFCShipEffects()
            End If
        End If
    End Sub

    Private Sub btnLeaveFleet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeaveFleet.Click
        ' Set the fleet status
        cboSCShip.SelectedIndex = -1 : cboWCShip.SelectedIndex = -1 : cboFCShip.SelectedIndex = -1
        cboSCPilot.SelectedIndex = -1 : cboWCPilot.SelectedIndex = -1 : cboFCPilot.SelectedIndex = -1
        cboSCShip.Enabled = False : cboWCShip.Enabled = False : cboFCShip.Enabled = False
        lblSCShip.Enabled = False : lblWCShip.Enabled = False : lblFCShip.Enabled = False
        cboSCShip.Tag = Nothing : cboWCShip.Tag = Nothing : cboFCShip.Tag = Nothing
        lblFleetStatus.Text = "Inactive"
        btnLeaveFleet.Enabled = False
        Call Me.CalculateFleetEffects()
    End Sub

    Private Sub CalculateFleetSkillEffects()

        ' Add in the commander details
        Dim Commanders As New ArrayList
        If cboSCPilot.SelectedItem IsNot Nothing Then
            Commanders.Add(cboSCPilot.SelectedItem.ToString)
        End If
        If cboWCPilot.SelectedItem IsNot Nothing Then
            Commanders.Add(cboWCPilot.SelectedItem.ToString)
        End If
        If cboFCPilot.SelectedItem IsNot Nothing Then
            Commanders.Add(cboFCPilot.SelectedItem.ToString)
        End If

        If Commanders.Count > 0 Then

            ' Go through each commander and parse the skills
            Dim FleetSkill(Commanders.Count + 1, fleetSkills.Count - 1) As String
            Dim hPilot As New HQFPilot
            For Commander As Integer = 0 To Commanders.Count - 1
                hPilot = CType(HQFPilotCollection.HQFPilots(Commanders(Commander)), HQFPilot)
                For Skill As Integer = 0 To FleetSkills.Count - 1
                    If hPilot.SkillSet.Contains(FleetSkills(Skill).ToString) Then
                        FleetSkill(Commander + 1, Skill) = CType(hPilot.SkillSet(fleetSkills(Skill).ToString), HQFSkill).Level.ToString
                        If FleetSkill(Commander + 1, Skill) >= FleetSkill(0, Skill) Then
                            FleetSkill(0, Skill) = FleetSkill(Commander + 1, Skill)
                            FleetSkill(Commanders.Count + 1, Skill) = hPilot.PilotName
                        End If
                    End If
                Next
            Next

            ' Display the fleet skills data
            For skill As Integer = 0 To fleetSkills.Count - 1
                If CInt(FleetSkill(0, skill)) > 0 Then
                    Dim fleetModule As New ShipModule
                    fleetModule.Name = fleetSkills(skill).ToString & " (" & FleetSkill(Commanders.Count + 1, skill) & " - Level " & FleetSkill(0, skill) & ")"
                    fleetModule.ID = "-" & EveHQ.Core.SkillFunctions.SkillNameToID(fleetSkills(skill).ToString)
                    fleetModule.ModuleState = 32
                    Select Case fleetSkills(skill).ToString
                        Case "Armored Warfare"
                            fleetModule.Attributes.Add("335", 2 * CInt(FleetSkill(0, skill)))
                        Case "Information Warfare"
                            fleetModule.Attributes.Add("309", 2 * CInt(FleetSkill(0, skill)))
                        Case "Leadership"
                            fleetModule.Attributes.Add("566", 2 * CInt(FleetSkill(0, skill)))
                        Case "Mining Foreman"
                            fleetModule.Attributes.Add("434", 2 * CInt(FleetSkill(0, skill)))
                        Case "Siege Warfare"
                            fleetModule.Attributes.Add("337", 2 * CInt(FleetSkill(0, skill)))
                        Case "Skirmish Warfare"
                            fleetModule.Attributes.Add("151", 2 * CInt(FleetSkill(0, skill)))
                    End Select
                    currentShip.FleetSlotCollection.Add(fleetModule)
                End If
            Next
        Else
            currentShip.FleetSlotCollection.Clear()
        End If

        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
        Call Me.UpdateAllSlotLocations()

    End Sub

    Private Sub cboSCShip_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSCShip.SelectedIndexChanged
        Call UpdateSCShipEffects()
    End Sub
    Private Sub cboWCShip_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWCShip.SelectedIndexChanged
        Call UpdateWCShipEffects()
    End Sub
    Private Sub cboFCShip_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFCShip.SelectedIndexChanged
        Call UpdateFCShipEffects()
    End Sub
    Private Sub UpdateSCShipEffects()
        If cboSCShip.SelectedIndex <> -1 Then
            ' Let's try and generate a fitting and get some module info
            Dim shipFit As String = cboSCShip.SelectedItem.ToString
            Dim fittingSep As Integer = shipFit.IndexOf(", ")
            Dim shipName As String = shipFit.Substring(0, fittingSep)
            Dim fittingName As String = shipFit.Substring(fittingSep + 2)
            Dim pShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
            pShip = Engine.UpdateShipDataFromFittingList(pShip, CType(Fittings.FittingList(shipFit), ArrayList))
            Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboSCPilot.SelectedItem.ToString), HQFPilot)
            Dim remoteShip As Ship = Engine.ApplyFitting(pShip, pPilot)
            pShip = Nothing
            Dim SCModules As New ArrayList
            ' Check the ship bonuses for further effects (Titans use this!)
            SCModules = GetShipGangBonusModules(remoteShip, pPilot)
            ' Check the modules for fleet effects
            For Each FleetModule As ShipModule In remoteShip.SlotCollection
                If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                    FleetModule.ModuleState = 16
                    FleetModule.SlotNo = 0
                    SCModules.Add(FleetModule)
                End If
            Next
            cboSCShip.Tag = SCModules
            ' Update the mapping effects back to the current pilot
            currentInfo.BuildMethod = BuildType.BuildEffectsMaps
            Call Me.CalculateFleetEffects()
        End If
    End Sub

    Private Sub UpdateWCShipEffects()
        If cboWCShip.SelectedIndex <> -1 Then
            ' Let's try and generate a fitting and get some module info
            Dim shipFit As String = cboWCShip.SelectedItem.ToString
            Dim fittingSep As Integer = shipFit.IndexOf(", ")
            Dim shipName As String = shipFit.Substring(0, fittingSep)
            Dim fittingName As String = shipFit.Substring(fittingSep + 2)
            Dim pShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
            pShip = Engine.UpdateShipDataFromFittingList(pShip, CType(Fittings.FittingList(shipFit), ArrayList))
            Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboWCPilot.SelectedItem.ToString), HQFPilot)
            Dim remoteShip As Ship = Engine.ApplyFitting(pShip, pPilot)
            pShip = Nothing
            Dim WCModules As New ArrayList
            For Each FleetModule As ShipModule In remoteShip.SlotCollection
                If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                    FleetModule.ModuleState = 16
                    FleetModule.SlotNo = 0
                    WCModules.Add(FleetModule)
                End If
            Next
            cboWCShip.Tag = WCModules
            ' Update the mapping effects back to the current pilot
            currentInfo.BuildMethod = BuildType.BuildEffectsMaps
            Call Me.CalculateFleetEffects()
        End If
    End Sub

    Private Sub UpdateFCShipEffects()
        If cboFCShip.SelectedIndex <> -1 Then
            ' Let's try and generate a fitting and get some module info
            Dim shipFit As String = cboFCShip.SelectedItem.ToString
            Dim fittingSep As Integer = shipFit.IndexOf(", ")
            Dim shipName As String = shipFit.Substring(0, fittingSep)
            Dim fittingName As String = shipFit.Substring(fittingSep + 2)
            Dim pShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
            pShip = Engine.UpdateShipDataFromFittingList(pShip, CType(Fittings.FittingList(shipFit), ArrayList))
            Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboFCPilot.SelectedItem.ToString), HQFPilot)
            Dim remoteShip As Ship = Engine.ApplyFitting(pShip, pPilot)
            pShip = Nothing
            Dim FCModules As New ArrayList
            For Each FleetModule As ShipModule In remoteShip.SlotCollection
                If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                    FleetModule.ModuleState = 16
                    FleetModule.SlotNo = 0
                    FCModules.Add(FleetModule)
                End If
            Next
            cboFCShip.Tag = FCModules
            ' Update the mapping effects back to the current pilot
            currentInfo.BuildMethod = BuildType.BuildEffectsMaps
            Call Me.CalculateFleetEffects()
        End If
    End Sub

    Private Sub CalculateFleetEffects()
        currentShip.FleetSlotCollection.Clear()
        Call Me.CalculateFleetSkillEffects()
        Call Me.CalculateFleetModuleEffects()
        currentInfo.ShipType = currentShip
        currentInfo.BuildMethod = BuildType.BuildFromEffectsMaps
        Call Me.UpdateAllSlotLocations()
    End Sub

    Private Sub CalculateFleetModuleEffects()

        Dim FleetCollection As New SortedList

        If cboSCShip.Tag IsNot Nothing Then
            Dim SCModules As ArrayList = CType(cboSCShip.Tag, ArrayList)
            For Each fleetModule As ShipModule In SCModules
                If FleetCollection.ContainsKey(fleetModule.Name) = False Then
                    ' Add it to the Fleet Collection
                    FleetCollection.Add(fleetModule.Name, fleetModule)
                Else
                    ' See if this module improves the fleet capabilities
                    Dim compareModule As ShipModule = CType(FleetCollection(fleetModule.Name), ShipModule)
                    If compareModule.Attributes.ContainsKey("833") = True Then
                        ' Contains the Command Bonus attribute
                        If Math.Abs(CDbl(fleetModule.Attributes("833"))) >= Math.Abs(CDbl(compareModule.Attributes("833"))) Then
                            FleetCollection(fleetModule.Name) = fleetModule
                        End If
                    Else
                        ' Contains the ECM Command Bonus attribute
                        If compareModule.Attributes.ContainsKey("1320") = True Then
                            ' Contains the Command Bonus attribute
                            If Math.Abs(CDbl(fleetModule.Attributes("1320"))) >= Math.Abs(CDbl(compareModule.Attributes("1320"))) Then
                                FleetCollection(fleetModule.Name) = fleetModule
                            End If
                        End If
                    End If
                End If

            Next
        End If

        If cboWCShip.Tag IsNot Nothing Then
            Dim WCModules As ArrayList = CType(cboWCShip.Tag, ArrayList)
            For Each fleetModule As ShipModule In WCModules
                If FleetCollection.ContainsKey(fleetModule.Name) = False Then
                    ' Add it to the Fleet Collection
                    FleetCollection.Add(fleetModule.Name, fleetModule)
                Else
                    ' See if this module improves the fleet capabilities
                    Dim compareModule As ShipModule = CType(FleetCollection(fleetModule.Name), ShipModule)
                    If compareModule.Attributes.ContainsKey("833") = True Then
                        ' Contains the Command Bonus attribute
                        If Math.Abs(CDbl(fleetModule.Attributes("833"))) >= Math.Abs(CDbl(compareModule.Attributes("833"))) Then
                            FleetCollection(fleetModule.Name) = fleetModule
                        End If
                    Else
                        ' Contains the ECM Command Bonus attribute
                        If compareModule.Attributes.ContainsKey("1320") = True Then
                            ' Contains the Command Bonus attribute
                            If Math.Abs(CDbl(fleetModule.Attributes("1320"))) >= Math.Abs(CDbl(compareModule.Attributes("1320"))) Then
                                FleetCollection(fleetModule.Name) = fleetModule
                            End If
                        End If
                    End If
                End If
            Next
        End If

        If cboFCShip.Tag IsNot Nothing Then
            Dim FCModules As ArrayList = CType(cboFCShip.Tag, ArrayList)
            For Each fleetModule As ShipModule In FCModules
                If FleetCollection.ContainsKey(fleetModule.Name) = False Then
                    ' Add it to the Fleet Collection
                    FleetCollection.Add(fleetModule.Name, fleetModule)
                Else
                    ' See if this module improves the fleet capabilities
                    Dim compareModule As ShipModule = CType(FleetCollection(fleetModule.Name), ShipModule)
                    If compareModule.Attributes.ContainsKey("833") = True Then
                        ' Contains the Command Bonus attribute
                        If Math.Abs(CDbl(fleetModule.Attributes("833"))) >= Math.Abs(CDbl(compareModule.Attributes("833"))) Then
                            FleetCollection(fleetModule.Name) = fleetModule
                        End If
                    Else
                        ' Contains the ECM Command Bonus attribute
                        If compareModule.Attributes.ContainsKey("1320") = True Then
                            ' Contains the Command Bonus attribute
                            If Math.Abs(CDbl(fleetModule.Attributes("1320"))) >= Math.Abs(CDbl(compareModule.Attributes("1320"))) Then
                                FleetCollection(fleetModule.Name) = fleetModule
                            End If
                        End If
                    End If
                End If
            Next
        End If

        For Each FleetModule As ShipModule In FleetCollection.Values
            currentShip.FleetSlotCollection.Add(FleetModule)
        Next

    End Sub

    Private Function GetShipGangBonusModules(ByVal hShip As Ship, ByVal hPilot As HQFPilot) As ArrayList
        Dim FleetModules As New ArrayList
        If hShip IsNot Nothing Then
            Dim shipRoles As New ArrayList
            Dim hSkill As New HQFSkill
            'Dim fEffect As New FinalEffect
            'Dim fEffectList As New ArrayList
            shipRoles = CType(Engine.ShipEffectsMap(hShip.ID), ArrayList)
            If shipRoles IsNot Nothing Then
                For Each chkEffect As ShipEffect In shipRoles
                    If chkEffect.Status = 16 Then
                        ' We have a gang bonus effect so create a dummy module for handling this

                        Dim gangModule As New ShipModule
                        gangModule.Name = hShip.Name & " Gang Bonus"
                        gangModule.ID = "-1"
                        gangModule.SlotNo = 0
                        gangModule.ModuleState = 16
                        If hPilot.SkillSet.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))) = True Then
                            hSkill = CType(hPilot.SkillSet(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))), HQFSkill)
                            If chkEffect.IsPerLevel = True Then
                                gangModule.Attributes.Add(chkEffect.AffectedAtt.ToString, chkEffect.Value * hSkill.Level)
                            Else
                                gangModule.Attributes.Add(chkEffect.AffectedAtt.ToString, chkEffect.Value)
                            End If
                        Else
                            gangModule.Attributes.Add(chkEffect.AffectedAtt.ToString, chkEffect.Value)
                        End If
                        FleetModules.Add(gangModule)

                    End If
                Next
            End If
        End If
        Return FleetModules

    End Function

#End Region

   
End Class
