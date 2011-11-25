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
Imports System.Drawing
Imports System.Text
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar

Public Class ShipSlotControl
    Dim UpdateAll As Boolean = False
    Dim UpdateDrones As Boolean = False
    Dim UpdateBoosters As Boolean = False
    Dim cancelDroneActivation As Boolean = False
    Dim rigGroups As New ArrayList
    Dim remoteGroups As New ArrayList
    Dim fleetGroups As New ArrayList
    Dim fleetSkills As New ArrayList
    Dim cancelSlotMenu As Boolean = False
    Dim DefaultModuleTooltipInfo As String = "<b><i>Double-click to remove this module<br />Right-click to bring up the module menu<br />Middle-click to toggle module state (extra uses with Ctrl/Shift)</i></b>"

#Region "Property Variables"

    Private currentInfo As ShipInfoControl
    Private cUpdateAllSlots As Boolean
    Private cParentFitting As Fitting ' Stores the fitting to which this control is attached to
    Private cUndoStack As Stack(Of UndoInfo)
    Private cRedoStack As Stack(Of UndoInfo)
#End Region

#Region "Properties"

    Public ReadOnly Property ParentFitting() As Fitting
        Get
            Return cParentFitting
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

    Public Property UndoStack() As Stack(Of UndoInfo)
        Get
            Return cUndoStack
        End Get
        Set(ByVal value As Stack(Of UndoInfo))
            cUndoStack = value
        End Set
    End Property

    Public Property RedoStack() As Stack(Of UndoInfo)
        Get
            Return cRedoStack
        End Get
        Set(ByVal value As Stack(Of UndoInfo))
            cRedoStack = value
        End Set
    End Property

#End Region

#Region "Constructor and Related Routines"

    Public Sub New(ByVal ShipFit As Fitting)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the parent fitting
        cParentFitting = ShipFit

        ' Set the associated ShipInfoControl
        currentInfo = ParentFitting.ShipInfoCtrl

        ' Add any initialization after the InitializeComponent() call.
		tcStorage.Height = HQF.Settings.HQFSettings.StorageBayHeight
        rigGroups.Add(773)
        rigGroups.Add(782)
        rigGroups.Add(778)
        rigGroups.Add(780)
        rigGroups.Add(786)
        rigGroups.Add(781)
        rigGroups.Add(775)
        rigGroups.Add(776)
        rigGroups.Add(779)
        rigGroups.Add(904)
        rigGroups.Add(777)
        rigGroups.Add(896)
        rigGroups.Add(774)
        remoteGroups.Add(41)
        remoteGroups.Add(325)
        remoteGroups.Add(585)
        remoteGroups.Add(67)
        remoteGroups.Add(65)
        remoteGroups.Add(68)
        remoteGroups.Add(71)
        remoteGroups.Add(291)
        remoteGroups.Add(209)
        remoteGroups.Add(289)
        remoteGroups.Add(290)
        remoteGroups.Add(208)
        remoteGroups.Add(379)
        remoteGroups.Add(544)
        remoteGroups.Add(641)
        remoteGroups.Add(640)
        remoteGroups.Add(639)
        fleetGroups.Add(316)
        fleetSkills.Add("Armored Warfare")
        fleetSkills.Add("Information Warfare")
        fleetSkills.Add("Leadership")
        fleetSkills.Add("Mining Foreman")
        fleetSkills.Add("Siege Warfare")
        fleetSkills.Add("Skirmish Warfare")

        ' Load the remote and fleet info
        Call Me.LoadRemoteFleetInfo()

        ' Load the Booster info
        Call Me.LoadBoosterInfo()

        ' Load WH Info
        Call Me.LoadWHInfo()

        cUndoStack = New Stack(Of UndoInfo)
        cRedoStack = New Stack(Of UndoInfo)

    End Sub

#End Region

#Region "Update Routines"

    Public Sub UpdateEverything()
        ' Update the slot layout
        currentInfo = ParentFitting.ShipInfoCtrl
        ' Build Subsystems
        ParentFitting.BaseShip = ParentFitting.BuildSubSystemEffects(ParentFitting.BaseShip)
        UpdateAll = True
        Call Me.UpdateShipSlotColumns()
        lvwCargoBay.BeginUpdate()
        lvwDroneBay.BeginUpdate()
        Me.ClearShipSlots()
        Me.ClearDroneBay()
        Me.ClearCargoBay()
        Me.ClearShipBay()
        Me.ParentFitting.UpdateBaseShipFromFitting()
        Me.UpdateShipSlotLayout()
        lvwCargoBay.EndUpdate()
        lvwDroneBay.EndUpdate()
        ParentFitting.ShipInfoCtrl.UpdateImplantList()
        ParentFitting.ApplyFitting(BuildType.BuildEverything)
        If ParentFitting.FittedShip IsNot Nothing Then
            Me.UpdateShipDetails()
            Me.RedrawDroneBay()
            Me.RedrawCargoBay()
            Me.RedrawShipBay()
            Me.UpdateBoosterSlots()
            Me.UpdateWHUI()
        Else
            MessageBox.Show("The fitting for " & Me.ParentFitting.KeyName & " failed to produce a calculated setup.", "Error Calculating Fitting", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        UpdateAll = False
    End Sub
    Private Sub UpdateShipDetails()
        If UpdateAll = False Then
            Call UpdateSlotNumbers()
            Call UpdatePrices()
            Call ParentFitting.UpdateFittingFromBaseShip()
        End If
    End Sub
    Private Sub UpdateSlotNumbers()
        Dim sc As Node = adtSlots.FindNodeByName("8")
        If sc IsNot Nothing Then
            adtSlots.FindNodeByName("8").Text = "High Slots (Launchers: " & ParentFitting.BaseShip.LauncherSlots_Used & "/" & ParentFitting.BaseShip.LauncherSlots & ", Turrets: " & ParentFitting.BaseShip.TurretSlots_Used & "/" & ParentFitting.BaseShip.TurretSlots & ")"
        End If
    End Sub
    Private Sub UpdatePrices()
        ParentFitting.BaseShip.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(ParentFitting.BaseShip.ID)
        lblShipMarketPrice.Text = "Ship Price: " & ParentFitting.BaseShip.MarketPrice.ToString("N2")
        lblFittingMarketPrice.Text = "Total Price: " & (ParentFitting.BaseShip.MarketPrice + ParentFitting.BaseShip.FittingMarketPrice).ToString("N2")
    End Sub
    Public Sub UpdateAllSlotLocations()
        Dim sTime, eTime As Date
        sTime = Now
        If ParentFitting.FittedShip IsNot Nothing Then
            adtSlots.BeginUpdate()
            For slot As Integer = 1 To ParentFitting.FittedShip.HiSlots
                If ParentFitting.FittedShip.HiSlot(slot) IsNot Nothing Then
                    UpdateSlotLocation(ParentFitting.FittedShip.HiSlot(slot), slot)
                End If
            Next
            For slot As Integer = 1 To ParentFitting.FittedShip.MidSlots
                If ParentFitting.FittedShip.MidSlot(slot) IsNot Nothing Then
                    UpdateSlotLocation(ParentFitting.FittedShip.MidSlot(slot), slot)
                End If
            Next
            For slot As Integer = 1 To ParentFitting.FittedShip.LowSlots
                If ParentFitting.FittedShip.LowSlot(slot) IsNot Nothing Then
                    UpdateSlotLocation(ParentFitting.FittedShip.LowSlot(slot), slot)
                End If
            Next
            For slot As Integer = 1 To ParentFitting.FittedShip.RigSlots
                If ParentFitting.FittedShip.RigSlot(slot) IsNot Nothing Then
                    UpdateSlotLocation(ParentFitting.FittedShip.RigSlot(slot), slot)
                End If
            Next
            For slot As Integer = 1 To ParentFitting.FittedShip.SubSlots
                If ParentFitting.FittedShip.SubSlot(slot) IsNot Nothing Then
                    UpdateSlotLocation(ParentFitting.FittedShip.SubSlot(slot), slot)
                End If
            Next
            adtSlots.EndUpdate()
            Call Me.RedrawCargoBayCapacity()
            Call Me.RedrawDroneBayCapacity()
            Call Me.RedrawShipBayCapacity()
        End If
        eTime = Now
        'MessageBox.Show((eTime - sTime).TotalMilliseconds.ToString & "ms", "Ship Slot Control Update")
        Call UpdateShipDetails()
    End Sub
    Private Sub UpdateSlotLocation(ByVal oldMod As ShipModule, ByVal slotNo As Integer)
        If oldMod IsNot Nothing Then
            Dim shipMod As New ShipModule
            Select Case oldMod.SlotType
                Case SlotTypes.Rig  ' Rig
                    shipMod = ParentFitting.FittedShip.RigSlot(slotNo)
                Case SlotTypes.Low  ' Low
                    shipMod = ParentFitting.FittedShip.LowSlot(slotNo)
                Case SlotTypes.Mid  ' Mid
                    shipMod = ParentFitting.FittedShip.MidSlot(slotNo)
                Case SlotTypes.High  ' High
                    shipMod = ParentFitting.FittedShip.HiSlot(slotNo)
                Case SlotTypes.Subsystem  ' Subsystem
                    shipMod = ParentFitting.FittedShip.SubSlot(slotNo)
            End Select
            If shipMod IsNot Nothing Then
                shipMod.ModuleState = oldMod.ModuleState
                Dim slotNode As Node = adtSlots.FindNodeByName(shipMod.SlotType & "_" & slotNo)
                slotNode.Image = CType(My.Resources.ResourceManager.GetObject("Mod0" & CInt(shipMod.ModuleState).ToString), Image)
                Call Me.UpdateUserColumnData(shipMod, slotNode)
            End If
        End If
    End Sub

#End Region

#Region "New Slot Update Routines"

    ''' <summary>
    ''' Updates the column headers of the ship slot control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateShipSlotColumns()
        ' Clear the columns
        adtSlots.BeginUpdate()
        adtSlots.Columns.Clear()
        ' Add the module name column
        Dim MainCol As New DevComponents.AdvTree.ColumnHeader("Module Name")
        MainCol.Name = "colName"
        MainCol.SortingEnabled = False
        MainCol.Width.Absolute = HQF.Settings.HQFSettings.SlotNameWidth
        MainCol.Width.AutoSizeMinHeader = True
        adtSlots.Columns.Add(MainCol)
        ' Iterate through the user selected columns and add them in
        For Each UserCol As UserSlotColumn In HQF.Settings.HQFSettings.UserSlotColumns
            If UserCol.Active = True Then
                Dim SubCol As New DevComponents.AdvTree.ColumnHeader(UserCol.Name)
                SubCol.SortingEnabled = False
                SubCol.Name = UserCol.Name
                SubCol.Width.Absolute = UserCol.Width
                SubCol.Width.AutoSizeMinHeader = True
                adtSlots.Columns.Add(SubCol)
            End If
        Next
        adtSlots.EndUpdate()
    End Sub

    ''' <summary>
    ''' Draws placeholders for the ship slots
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateShipSlotLayout()

        adtSlots.BeginUpdate()
        adtSlots.Nodes.Clear()

        Dim SHIZ As Integer = 24
        Dim HiSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
        Dim MidSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
        Dim LowSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
        Dim RigSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
        Dim SubSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
        Dim SelSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
        SelSlotStyle.BackColorGradientType = eGradientType.Linear
        SelSlotStyle.BackColor = Color.Orange
        SelSlotStyle.BackColor2 = Color.OrangeRed

        ' Create high slots
        If ParentFitting.BaseShip.HiSlots > 0 Then
            Dim ParentNode As New Node("High Slots", adtSlots.Styles("HeaderStyle"))
            ParentNode.Name = "8"
            ParentNode.FullRowBackground = True
            ParentNode.Image = New Bitmap(My.Resources.imgHiSlot, SHIZ, SHIZ)
            adtSlots.Nodes.Add(ParentNode)
            For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                Dim SlotNode As New Node("", HiSlotStyle)
                SlotNode.Name = "8_" & slot
                SlotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                SlotNode.StyleSelected = SelSlotStyle
                Call Me.AddUserColumnData(ParentFitting.BaseShip.HiSlot(slot), SlotNode)
                ParentNode.Nodes.Add(SlotNode)
            Next
            ParentNode.Expanded = True
        End If

        ' Create mid slots
        If ParentFitting.BaseShip.MidSlots > 0 Then
            Dim ParentNode As New Node("Mid Slots", adtSlots.Styles("HeaderStyle"))
            ParentNode.Name = "4"
            ParentNode.FullRowBackground = True
            ParentNode.Image = New Bitmap(My.Resources.imgMidSlot, SHIZ, SHIZ)
            adtSlots.Nodes.Add(ParentNode)
            For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                Dim SlotNode As New Node("", MidSlotStyle)
                SlotNode.Name = "4_" & slot
                SlotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                SlotNode.StyleSelected = SelSlotStyle
                Call Me.AddUserColumnData(ParentFitting.BaseShip.MidSlot(slot), SlotNode)
                ParentNode.Nodes.Add(SlotNode)
            Next
            ParentNode.Expanded = True
        End If

        ' Create low slots
        If ParentFitting.BaseShip.LowSlots > 0 Then
            Dim ParentNode As New Node("Low Slots", adtSlots.Styles("HeaderStyle"))
            ParentNode.Name = "2"
            ParentNode.FullRowBackground = True
            ParentNode.Image = New Bitmap(My.Resources.imgLowSlot, SHIZ, SHIZ)
            adtSlots.Nodes.Add(ParentNode)
            For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                Dim SlotNode As New Node("", LowSlotStyle)
                SlotNode.Name = "2_" & slot
                SlotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                SlotNode.StyleSelected = SelSlotStyle
                Call Me.AddUserColumnData(ParentFitting.BaseShip.LowSlot(slot), SlotNode)
                ParentNode.Nodes.Add(SlotNode)
            Next
            ParentNode.Expanded = True
        End If

        ' Create rig slots
        If ParentFitting.BaseShip.RigSlots > 0 Then
            Dim ParentNode As New Node("Rig Slots", adtSlots.Styles("HeaderStyle"))
            ParentNode.Name = "1"
            ParentNode.FullRowBackground = True
            ParentNode.Image = New Bitmap(My.Resources.imgRigSlot, SHIZ, SHIZ)
            adtSlots.Nodes.Add(ParentNode)
            For slot As Integer = 1 To ParentFitting.BaseShip.RigSlots
                Dim SlotNode As New Node("", RigSlotStyle)
                SlotNode.Name = "1_" & slot
                SlotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                SlotNode.StyleSelected = SelSlotStyle
                Call Me.AddUserColumnData(ParentFitting.BaseShip.RigSlot(slot), SlotNode)
                ParentNode.Nodes.Add(SlotNode)
            Next
            ParentNode.Expanded = True
        End If

        ' Create sub slots
        If ParentFitting.BaseShip.SubSlots > 0 Then
            Dim ParentNode As New Node("Subsystem Slots", adtSlots.Styles("HeaderStyle"))
            ParentNode.Name = "16"
            ParentNode.FullRowBackground = True
            ParentNode.Image = New Bitmap(My.Resources.imgSubSlot, SHIZ, SHIZ)
            adtSlots.Nodes.Add(ParentNode)
            For slot As Integer = 1 To ParentFitting.BaseShip.SubSlots
                Dim SlotNode As New Node("", SubSlotStyle)
                SlotNode.Name = "16_" & slot
                SlotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.SubSlotColour))
                SlotNode.StyleSelected = SelSlotStyle
                Call Me.AddUserColumnData(ParentFitting.BaseShip.SubSlot(slot), SlotNode)
                ParentNode.Nodes.Add(SlotNode)
            Next
            ParentNode.Expanded = True
        End If

        adtSlots.EndUpdate()

        ' Update details
        Call UpdateSlotNumbers()
        Call UpdatePrices()

    End Sub

    ''' <summary>
    ''' Creates the individual cells of a node based on the ship module and user columns required
    ''' </summary>
    ''' <param name="shipMod">The module to use the information from</param>
    ''' <param name="slotNode">The particular node to update</param>
    ''' <remarks></remarks>
    Private Sub AddUserColumnData(ByVal shipMod As ShipModule, ByVal slotNode As Node)
        ' Add subitems based on the user selected columns
        If shipMod IsNot Nothing Then
            Dim colName As String = ""
            ' Add in the module name
            slotNode.Text = shipMod.Name
            Dim Desc As String = ""
            If shipMod.SlotType = SlotTypes.Subsystem Then
                Desc &= "Slot Modifiers - High: " & shipMod.Attributes("1374") & ", Mid: " & shipMod.Attributes("1375") & ", Low: " & shipMod.Attributes("1376") & ControlChars.CrLf & ControlChars.CrLf
            End If
            Desc &= shipMod.Description
            SlotTip.SetSuperTooltip(slotNode, New SuperTooltipInfo(shipMod.Name, "Ship Module Information", Desc, EveHQ.Core.ImageHandler.GetImage(shipMod.ID, 64), My.Resources.imgInfo1, eTooltipColor.Yellow))
            ' Add the additional columns
            For Each UserCol As UserSlotColumn In Settings.HQFSettings.UserSlotColumns
                If UserCol.Active = True Then
                    Select Case UserCol.Name
                        Case "Charge"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                slotNode.Cells.Add(New Cell(shipMod.LoadedCharge.Name))
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "CPU"
                            slotNode.Cells.Add(New Cell(shipMod.CPU.ToString("N2")))
                        Case "PG"
                            slotNode.Cells.Add(New Cell(shipMod.PG.ToString("N2")))
                        Case "Calib"
                            slotNode.Cells.Add(New Cell(shipMod.Calibration.ToString("N2")))
                        Case "Price"
                            shipMod.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(shipMod.ID)
                            slotNode.Cells.Add(New Cell(shipMod.MarketPrice.ToString("N2")))
                        Case "ActCost"
                            If shipMod.Attributes.ContainsKey("6") Then
                                If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotNode.Cells.Add(New Cell(shipMod.CapUsage.ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "ActTime"
                            If shipMod.Attributes.ContainsKey("73") Then
                                If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotNode.Cells.Add(New Cell(shipMod.ActivationTime.ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "CapRate"
                            If shipMod.Attributes.ContainsKey("10032") Then
                                If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotNode.Cells.Add(New Cell((shipMod.CapUsageRate * -1).ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "OptRange"
                            If shipMod.Attributes.ContainsKey("54") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("54").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("87") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("87").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("91") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("91").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("98") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("98").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("99") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("99").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("103") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("103").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("142") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("142").ToString("N2")))
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "ROF"
                            If shipMod.Attributes.ContainsKey("51") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("51").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("10011") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("10011").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("10012") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("10012").ToString("N2")))
                            ElseIf shipMod.Attributes.ContainsKey("10013") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("10013").ToString("N2")))
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "Damage"
                            If shipMod.Attributes.ContainsKey("10018") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("10018").ToString("N2")))
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "DPS"
                            If shipMod.Attributes.ContainsKey("10019") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("10019").ToString("N2")))
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "Falloff"
                            If shipMod.Attributes.ContainsKey("158") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("158").ToString("N2")))
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "Tracking"
                            If shipMod.Attributes.ContainsKey("160") Then
                                slotNode.Cells.Add(New Cell(shipMod.Attributes("160").ToString("N4")))
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "ExpRad"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                If shipMod.LoadedCharge.Attributes.ContainsKey("654") Then
                                    slotNode.Cells.Add(New Cell(shipMod.LoadedCharge.Attributes("654").ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                        Case "ExpVel"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                If shipMod.LoadedCharge.Attributes.ContainsKey("653") Then
                                    slotNode.Cells.Add(New Cell(shipMod.LoadedCharge.Attributes("653").ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Else
                                slotNode.Cells.Add(New Cell(""))
                            End If
                    End Select
                End If
            Next
        Else
            slotNode.Text = "<Empty>"
            slotNode.Image = CType(My.Resources.ResourceManager.GetObject("Mod01"), Image)
            SlotTip.SetSuperTooltip(slotNode, Nothing)
            For Each UserCol As UserSlotColumn In Settings.HQFSettings.UserSlotColumns
                If UserCol.Active = True Then
                    slotNode.Cells.Add(New Cell(""))
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Updates the individual cells of a node based on the ship module and user columns
    ''' </summary>
    ''' <param name="shipMod">The module to use the information from</param>
    ''' <param name="slotNode">The particular node to update</param>
    ''' <remarks></remarks>
    Private Sub UpdateUserColumnData(ByVal shipMod As ShipModule, ByVal slotNode As Node)
        ' Add subitems based on the user selected columns
        Dim colName As String = ""
        Dim idx As Integer = 1
        ' Add in the module name
        slotNode.Text = shipMod.Name
        Dim Desc As String = ""
        If shipMod.SlotType = SlotTypes.Subsystem Then
            Desc &= "Slot Modifiers - High: " & shipMod.Attributes("1374") & ", Mid: " & shipMod.Attributes("1375") & ", Low: " & shipMod.Attributes("1376") & ControlChars.CrLf & ControlChars.CrLf
        End If
        Desc &= shipMod.Description
        SlotTip.SetSuperTooltip(slotNode, New SuperTooltipInfo(shipMod.Name, DefaultModuleTooltipInfo, Desc, EveHQ.Core.ImageHandler.GetImage(shipMod.ID, 64), My.Resources.imgInfo1, eTooltipColor.Yellow))
        ' Add the additional columns
        For Each UserCol As UserSlotColumn In Settings.HQFSettings.UserSlotColumns
            If UserCol.Active = True Then
                Select Case UserCol.Name
                    Case "Charge"
                        If shipMod.LoadedCharge IsNot Nothing Then
                            slotNode.Cells(idx).Text = shipMod.LoadedCharge.Name
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "CPU"
                        slotNode.Cells(idx).Text = shipMod.CPU.ToString("N2")
                        idx += 1
                    Case "PG"
                        slotNode.Cells(idx).Text = shipMod.PG.ToString("N2")
                        idx += 1
                    Case "Calib"
                        slotNode.Cells(idx).Text = shipMod.Calibration.ToString("N2")
                        idx += 1
                    Case "Price"
                        shipMod.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(shipMod.ID)
                        slotNode.Cells(idx).Text = shipMod.MarketPrice.ToString("N2")
                        idx += 1
                    Case "ActCost"
                        If shipMod.Attributes.ContainsKey("6") Then
                            If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                slotNode.Cells(idx).Text = shipMod.CapUsage.ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "ActTime"
                        If shipMod.Attributes.ContainsKey("73") Then
                            If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                slotNode.Cells(idx).Text = shipMod.ActivationTime.ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "CapRate"
                        If shipMod.Attributes.ContainsKey("10032") Then
                            If shipMod.ModuleState = ModuleStates.Active Or shipMod.ModuleState = ModuleStates.Overloaded Then
                                slotNode.Cells(idx).Text = (shipMod.CapUsageRate * -1).ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "OptRange"
                        If shipMod.Attributes.ContainsKey("54") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("54").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("87") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("87").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("91") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("91").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("98") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("98").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("99") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("99").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("103") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("103").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("142") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("142").ToString("N2")
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "ROF"
                        If shipMod.Attributes.ContainsKey("51") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("51").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("10011") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("10011").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("10012") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("10012").ToString("N2")
                        ElseIf shipMod.Attributes.ContainsKey("10013") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("10013").ToString("N2")
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "Damage"
                        If shipMod.Attributes.ContainsKey("10018") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("10018").ToString("N2")
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "DPS"
                        If shipMod.Attributes.ContainsKey("10019") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("10019").ToString("N2")
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "Falloff"
                        If shipMod.Attributes.ContainsKey("158") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("158").ToString("N2")
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "Tracking"
                        If shipMod.Attributes.ContainsKey("160") Then
                            slotNode.Cells(idx).Text = shipMod.Attributes("160").ToString("N4")
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "ExpRad"
                        If shipMod.LoadedCharge IsNot Nothing Then
                            If shipMod.LoadedCharge.Attributes.ContainsKey("654") Then
                                slotNode.Cells(idx).Text = shipMod.LoadedCharge.Attributes("654").ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                    Case "ExpVel"
                        If shipMod.LoadedCharge IsNot Nothing Then
                            If shipMod.LoadedCharge.Attributes.ContainsKey("653") Then
                                slotNode.Cells(idx).Text = shipMod.LoadedCharge.Attributes("653").ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                        Else
                            slotNode.Cells(idx).Text = ""
                        End If
                        idx += 1
                End Select
            End If
        Next
    End Sub

#End Region

#Region "New Slot UI Functions"

    Private Sub adtSlots_ColumnMoved(ByVal sender As Object, ByVal ea As DevComponents.AdvTree.ColumnMovedEventArgs) Handles adtSlots.ColumnMoved
        ' Get true locations
        Dim StartColName As String = ea.Column.Name
        Dim EndColName As String = adtSlots.Columns(ea.NewColumnDisplayIndex).Name
        Dim StartIdx As Integer = 0
        Dim EndIdx As Integer = 0
        For idx As Integer = 1 To HQF.Settings.HQFSettings.UserSlotColumns.Count - 1
            If HQF.Settings.HQFSettings.UserSlotColumns(idx).Name = StartColName Then
                StartIdx = idx
            End If
            If HQF.Settings.HQFSettings.UserSlotColumns(idx).Name = EndColName Then
                EndIdx = idx
            End If
        Next

        If StartIdx = 0 Then
            ' Ignore stuff
        Else
            Dim SCol As UserSlotColumn = HQF.Settings.HQFSettings.UserSlotColumns(StartIdx)
            Dim StartUserCol As New UserSlotColumn(SCol.Name, SCol.Description, SCol.Width, SCol.Active)
            If EndIdx > StartIdx Then
                For Idx As Integer = StartIdx To EndIdx - 1
                    Dim MCol As UserSlotColumn = HQF.Settings.HQFSettings.UserSlotColumns(Idx + 1)
                    HQF.Settings.HQFSettings.UserSlotColumns(Idx) = New UserSlotColumn(MCol.Name, MCol.Description, MCol.Width, MCol.Active)
                Next
                HQF.Settings.HQFSettings.UserSlotColumns(EndIdx) = StartUserCol
            Else
                For Idx As Integer = StartIdx - 1 To EndIdx Step -1
                    Dim MCol As UserSlotColumn = HQF.Settings.HQFSettings.UserSlotColumns(Idx)
                    HQF.Settings.HQFSettings.UserSlotColumns(Idx + 1) = New UserSlotColumn(MCol.Name, MCol.Description, MCol.Width, MCol.Active)
                Next
                HQF.Settings.HQFSettings.UserSlotColumns(EndIdx) = StartUserCol
            End If
        End If
    End Sub

    Private Sub adtSlots_ColumnResized(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtSlots.ColumnResized
        Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        If ch.Name <> "colName" Then
            For Each UserCol As UserSlotColumn In HQF.Settings.HQFSettings.UserSlotColumns
                If UserCol.Name = ch.Name Then
                    UserCol.Width = ch.Width.Absolute
                    Exit Sub
                End If
            Next
        Else
            HQF.Settings.HQFSettings.SlotNameWidth = ch.Width.Absolute
        End If
    End Sub

    Private Sub adtSlots_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles adtSlots.KeyDown
        Select Case e.KeyCode
            Case Keys.Delete
                Call Me.RemoveModules(sender, e)
            Case Keys.C
                If e.Control = True Then
                    Call Me.CopyModulesToClipboard()
                End If
            Case Keys.V
                If e.Control = True Then
                    Call Me.PasteModuleFromClipboard()
                End If
            Case Keys.Space
                ' Check for key status
                Dim keyMode As Integer = 0 ' 0=None, 1=Shift, 2=Ctrl, 4=Alt
                If e.Shift = True Then keyMode += 1
                If e.Control = True Then keyMode += 2
                If e.Alt = True Then keyMode += 4
                Call ChangeModuleState(keyMode)
        End Select
    End Sub

    Private Sub adtSlots_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtSlots.NodeDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Only remove if double-clicking the left button!
            If adtSlots.SelectedNodes.Count > 0 Then
                ' Check if the "slot" is not empty
                If cancelSlotMenu = False Then
                    If adtSlots.SelectedNodes(0).Text <> "<Empty>" Then
                        Call Me.UpdateMRUModules(adtSlots.SelectedNodes(0).Text)
                        Call RemoveModule(adtSlots.SelectedNodes(0), True, False)
                        HQFEvents.StartUpdateModuleList = True
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub UpdateMRUModules(ByVal modName As String)
        If HQF.Settings.HQFSettings.MRUModules.Count < HQF.Settings.HQFSettings.MRULimit Then
            ' If the MRU list isn't full
            If HQF.Settings.HQFSettings.MRUModules.Contains(modName) = False Then
                ' If the module isn't already in the list
                HQF.Settings.HQFSettings.MRUModules.Add(modName)
            Else
                ' If it is in the list, remove it and add it at the end
                HQF.Settings.HQFSettings.MRUModules.Remove(modName)
                HQF.Settings.HQFSettings.MRUModules.Add(modName)
            End If
        Else
            If HQF.Settings.HQFSettings.MRUModules.Contains(modName) = False Then
                For m As Integer = 0 To HQF.Settings.HQFSettings.MRULimit - 2
                    HQF.Settings.HQFSettings.MRUModules(m) = HQF.Settings.HQFSettings.MRUModules(m + 1)
                Next
                HQF.Settings.HQFSettings.MRUModules.RemoveAt(HQF.Settings.HQFSettings.MRULimit - 1)
                HQF.Settings.HQFSettings.MRUModules.Add(modName)
            Else
                ' If it is in the list, remove it and add it at the end
                HQF.Settings.HQFSettings.MRUModules.Remove(modName)
                HQF.Settings.HQFSettings.MRUModules.Add(modName)
            End If
        End If
    End Sub

    Private Sub adtSlots_NodeClick(sender As Object, e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtSlots.NodeClick
        If e.Node.Level = 0 Then
            adtSlots.SelectedNodes.Clear()
            For Each SubNode As Node In e.Node.Nodes
                adtSlots.SelectedNodes.Add(SubNode, eTreeAction.Keyboard)
            Next
        End If
    End Sub

    Private Sub adtSlots_NodeMouseDown(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtSlots.NodeMouseDown

        If e.Node IsNot Nothing Then

            If e.Node.Level > 0 Then

                If e.Button = Windows.Forms.MouseButtons.Middle Then

                    ' Check for key status
                    Dim keyMode As Integer = 0 ' 0=None, 1=Shift, 2=Ctrl, 4=Alt
                    If My.Computer.Keyboard.ShiftKeyDown Then keyMode += 1
                    If My.Computer.Keyboard.CtrlKeyDown Then keyMode += 2
                    If My.Computer.Keyboard.AltKeyDown Then keyMode += 4

                    ' Check which mode, single or multi
                    If adtSlots.SelectedNodes.Count > 1 Then
                        If e.Node.IsSelected = True Then
                            For Each SelNode As Node In adtSlots.SelectedNodes
                                Call Me.ChangeSingleModuleState(keyMode, SelNode)
                            Next
                        Else
                            Call Me.ChangeSingleModuleState(keyMode, e.Node)
                        End If
                    Else
                        Call Me.ChangeSingleModuleState(keyMode, e.Node)
                    End If

                    ' Update the ship data
                    ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)

                End If

            End If

        End If

    End Sub

    Private Sub ChangeModuleState(KeyMode As integer)
        If adtSlots.SelectedNodes.Count > 0 Then
            For Each SelNode As Node In adtSlots.SelectedNodes
                Call Me.ChangeSingleModuleState(KeyMode, SelNode)
            Next
        End If
        ' Update the ship data
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
    End Sub

    ''' <summary>
    ''' Changes the module state of the module in the current node (slot)
    ''' </summary>
    ''' <param name="KeyMode">The Shift/Ctrl/Aly key combination used in conjunction with the state change</param>
    ''' <param name="slotNode">The affected slot node</param>
    ''' <remarks></remarks>
    Private Sub ChangeSingleModuleState(ByVal KeyMode As Integer, ByVal slotNode As Node)
        ' Get the module details
        Dim currentMod As New ShipModule
        Dim fittedMod As New ShipModule
        Dim sep As Integer = slotNode.Name.LastIndexOf("_")
        Dim slotType As Integer = CInt(slotNode.Name.Substring(0, sep))
        Dim slotNo As Integer = CInt(slotNode.Name.Substring(sep + 1, 1))
        Dim canOffline As Boolean = True
        Select Case slotType
            Case 1 ' Rig
                currentMod = ParentFitting.BaseShip.RigSlot(slotNo)
                fittedMod = ParentFitting.FittedShip.RigSlot(slotNo)
                canOffline = False
            Case 2 ' Low
                currentMod = ParentFitting.BaseShip.LowSlot(slotNo)
                fittedMod = ParentFitting.FittedShip.LowSlot(slotNo)
            Case 4 ' Mid
                currentMod = ParentFitting.BaseShip.MidSlot(slotNo)
                fittedMod = ParentFitting.FittedShip.MidSlot(slotNo)
            Case 8 ' High
                currentMod = ParentFitting.BaseShip.HiSlot(slotNo)
                fittedMod = ParentFitting.FittedShip.HiSlot(slotNo)
            Case 16 ' Subsystem
                currentMod = ParentFitting.BaseShip.SubSlot(slotNo)
                fittedMod = ParentFitting.FittedShip.SubSlot(slotNo)
                canOffline = False
        End Select
        If currentMod IsNot Nothing Then
            Dim currentstate As Integer = currentMod.ModuleState
            ' Check for status
            Dim canDeactivate As Boolean = False
            Dim canOverload As Boolean = False
            ' Check for activation cost
            If currentMod.Attributes.ContainsKey("6") = True Or currentMod.Attributes.ContainsKey("669") Or currentMod.IsTurret Or currentMod.IsLauncher Or currentMod.Attributes.ContainsKey("713") Then
                canDeactivate = True
            End If
            If currentMod.Attributes.ContainsKey("1211") = True Then
                canOverload = True
            End If

            ' Do new routine for handling module state changes
            Select Case KeyMode
                Case 0 ' No additional keys
                    If currentstate = ModuleStates.Offline Or currentstate = ModuleStates.Inactive Or currentstate = ModuleStates.Overloaded Then
                        currentstate = ModuleStates.Active
                    ElseIf currentstate = ModuleStates.Active Then
                        If canDeactivate = True Then
                            currentstate = ModuleStates.Inactive
                        End If
                    End If
                Case 1 ' Shift
                    If currentstate = ModuleStates.Overloaded Then
                        currentstate = ModuleStates.Active
                    Else
                        If canOverload = True Then
                            currentstate = ModuleStates.Overloaded
                        End If
                    End If
                Case 2 ' Ctrl
                    If currentstate = ModuleStates.Offline Then
                        currentstate = ModuleStates.Active
                    Else
                        If canOffline = True Then
                            currentstate = ModuleStates.Offline
                        End If
                    End If
            End Select

            ' Update only if the module state has changed
            If currentstate <> currentMod.ModuleState Then
                ' Check for command processors as this affects the fitting!
                If currentMod.ID = "11014" Then
                    If currentstate = ModuleStates.Offline Then
                        ParentFitting.BaseShip.Attributes("10063") -= 1
                        ' Check if we need to deactivate a highslot ganglink
                        Dim ActiveGanglinks As New List(Of Integer)
                        For slot As Integer = 8 To 1 Step -1
                            If ParentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                                If ParentFitting.BaseShip.HiSlot(slot).DatabaseGroup = "316" And ParentFitting.BaseShip.HiSlot(slot).ModuleState = ModuleStates.Active Then
                                    ActiveGanglinks.Add(slot)
                                End If
                            End If
                        Next
                        If ActiveGanglinks.Count > ParentFitting.BaseShip.Attributes("10063") Then
                            ParentFitting.BaseShip.HiSlot(ActiveGanglinks(0)).ModuleState = ModuleStates.Inactive
                        End If
                    Else
                        ParentFitting.BaseShip.Attributes("10063") += 1
                    End If
                End If
                Dim oldState As ModuleStates = currentMod.ModuleState
                currentMod.ModuleState = CType(currentstate, ModuleStates)
                ' Check for maxGroupActive flag
                If (currentstate = ModuleStates.Active Or currentstate = ModuleStates.Overloaded) And currentMod.Attributes.ContainsKey("763") = True Then
                    If currentMod.DatabaseGroup <> "316" Then
                        If ParentFitting.IsModuleGroupLimitExceeded(fittedMod, False) = True Then
                            ' Set the module offline
                            MessageBox.Show("You cannot activate the " & currentMod.Name & " due to a restriction on the maximum number permitted for this group.", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            currentMod.ModuleState = oldState
                            Exit Sub
                        End If
                    Else
                        If ParentFitting.IsModuleGroupLimitExceeded(fittedMod, False) = True Then
                            ' Set the module offline
                            MessageBox.Show("You cannot activate the " & currentMod.Name & " due to a restriction on the maximum number permitted for this group.", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            currentMod.ModuleState = oldState
                            Exit Sub
                        Else
                            If ParentFitting.CountActiveTypeModules(fittedMod.ID) > CInt(fittedMod.Attributes("763")) Then
                                ' Set the module offline
                                MessageBox.Show("You cannot activate the " & currentMod.Name & " due to a restriction on the maximum number permitted for this type.", "Module Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                currentMod.ModuleState = oldState
                                Exit Sub
                            End If
                        End If
                    End If
                End If
                ' Check for activation of siege mode with remote effects
                If fittedMod.ID = "20280" Then
                    If ParentFitting.FittedShip.RemoteSlotCollection.Count > 0 Then
                        Dim msg As String = "You have active remote modules and activating Siege Mode will cancel these effects. Do you wish to continue activating Siege Mode?"
                        Dim reply As Integer = MessageBox.Show(msg, "Confirm Activate Siege Mode", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = DialogResult.No Then
                            fittedMod.ModuleState = oldState
                            Exit Sub
                        Else
                            ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                            Call Me.ResetRemoteEffects()
                        End If
                    End If
                End If
                ' Check for activation of triage mode with remote effects
                If fittedMod.ID = "27951" Then
                    If ParentFitting.FittedShip.RemoteSlotCollection.Count > 0 Then
                        Dim msg As String = "You have active remote modules and activating Traige Mode will cancel these effects. Do you wish to continue activating Triage Mode?"
                        Dim reply As Integer = MessageBox.Show(msg, "Confirm Activate Triage Mode", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = DialogResult.No Then
                            fittedMod.ModuleState = oldState
                            Exit Sub
                        Else
                            ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                            Call Me.ResetRemoteEffects()
                        End If
                    End If
                End If

            End If
        End If
    End Sub

    Private Sub CopyModulesToClipboard()
        If adtSlots.SelectedNodes.Count = 1 Then
            Dim selitem As Node = adtSlots.SelectedNodes(0)
            If selitem.Text <> "<Empty>" Then
                Dim sep As Integer = selitem.Name.LastIndexOf("_")
                Dim slotType As Integer = CInt(selitem.Name.Substring(0, sep))
                Dim slotNo As Integer = CInt(selitem.Name.Substring(sep + 1, 1))
                Dim LoadedModule As New ShipModule
                Select Case slotType
                    Case 1 ' Rig
                        LoadedModule = ParentFitting.BaseShip.RigSlot(slotNo)
                    Case 2 ' Low
                        LoadedModule = ParentFitting.BaseShip.LowSlot(slotNo)
                    Case 4 ' Mid
                        LoadedModule = ParentFitting.BaseShip.MidSlot(slotNo)
                    Case 8 ' High
                        LoadedModule = ParentFitting.BaseShip.HiSlot(slotNo)
                    Case 16 ' Subsystem
                        LoadedModule = ParentFitting.BaseShip.SubSlot(slotNo)
                End Select
                Clipboard.SetData("ShipModule", LoadedModule.Clone)
            End If
        End If
    End Sub

    Private Sub PasteModuleFromClipboard()
        If Clipboard.ContainsData("ShipModule") Then
            If adtSlots.SelectedNodes.Count > 0 Then
                Dim UpdatedCount As Integer = 0
                For Each selItem As Node In adtSlots.SelectedNodes
                    Dim NewModule As ShipModule = CType(Clipboard.GetData("ShipModule"), ShipModule).Clone
                    Dim sep As Integer = selItem.Name.LastIndexOf("_")
                    Dim slotType As Integer = CInt(selItem.Name.Substring(0, sep))
                    Dim slotNo As Integer = CInt(selItem.Name.Substring(sep + 1, 1))
                    UpdatedCount += 1
                    If UpdatedCount = adtSlots.SelectedNodes.Count Then
                        ParentFitting.AddModule(NewModule, slotNo, True, False, Nothing, False, False)
                    Else
                        ParentFitting.AddModule(NewModule, slotNo, False, False, Nothing, False, False)
                    End If
                Next
            End If
        End If
    End Sub

#End Region

#Region "Clearing routines"
    Private Sub ClearShipSlots()
        If ParentFitting.BaseShip IsNot Nothing Then
            For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                ParentFitting.BaseShip.HiSlot(slot) = Nothing
            Next
            For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                ParentFitting.BaseShip.MidSlot(slot) = Nothing
            Next
            For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                ParentFitting.BaseShip.LowSlot(slot) = Nothing
            Next
            For slot As Integer = 1 To ParentFitting.BaseShip.RigSlots
                ParentFitting.BaseShip.RigSlot(slot) = Nothing
            Next
            For slot As Integer = 1 To ParentFitting.BaseShip.SubSlots
                ParentFitting.BaseShip.SubSlot(slot) = Nothing
            Next
        End If
    End Sub
    Private Sub ClearDroneBay()
        If ParentFitting.BaseShip IsNot Nothing Then
            ParentFitting.BaseShip.DroneBayItems.Clear()
            ParentFitting.BaseShip.DroneBay_Used = 0
        End If
    End Sub
    Private Sub ClearCargoBay()
        If ParentFitting.BaseShip IsNot Nothing Then
            ParentFitting.BaseShip.CargoBayItems.Clear()
            ParentFitting.BaseShip.CargoBay_Used = 0
            ParentFitting.BaseShip.CargoBay_Additional = 0
        End If
    End Sub
    Private Sub ClearShipBay()
        If ParentFitting.BaseShip IsNot Nothing Then
            ParentFitting.BaseShip.ShipBayItems.Clear()
            ParentFitting.BaseShip.ShipBay_Used = 0
        End If
		' Remove the Ship Maintenance Bay tab if we don't need it (to avoid confusion)
		If ParentFitting.BaseShip.ShipBay = 0 Then
			tiShipBay.Visible = False
		Else
			tiShipBay.Visible = True
		End If
    End Sub
#End Region

#Region "Removing Mods/Drones/Items"

    Private Sub RemoveModules(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim removedSubsystems As Boolean = False
        adtSlots.BeginUpdate()
        For Each slot As Node In adtSlots.SelectedNodes
            If slot.Text <> "<Empty>" Then
                Dim sep As Integer = slot.Name.LastIndexOf("_")
                Dim slotType As Integer = CInt(slot.Name.Substring(0, sep))
                Dim slotNo As Integer = CInt(slot.Name.Substring(sep + 1, 1))
                Dim selMod As New ShipModule
                Select Case slotType
                    Case 1 ' Rig
                        selMod = ParentFitting.BaseShip.RigSlot(slotNo)
                    Case 2 ' Low
                        selMod = ParentFitting.BaseShip.LowSlot(slotNo)
                    Case 4 ' Mid
                        selMod = ParentFitting.BaseShip.MidSlot(slotNo)
                    Case 8 ' High
                        selMod = ParentFitting.BaseShip.HiSlot(slotNo)
                    Case 16 ' Subsystem
                        selMod = ParentFitting.BaseShip.SubSlot(slotNo)
                End Select
                If selMod.LoadedCharge IsNot Nothing Then
                    Me.UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveModule, slotType, slotNo, selMod.Name, selMod.LoadedCharge.Name, slotNo, "", ""))
                Else
                    Me.UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveModule, slotType, slotNo, selMod.Name, "", slotNo, "", ""))
                End If
                Select Case slotType
                    Case 1 ' Rig
                        ParentFitting.BaseShip.RigSlot(slotNo) = Nothing
                    Case 2 ' Low
                        ParentFitting.BaseShip.LowSlot(slotNo) = Nothing
                    Case 4 ' Mid
                        ParentFitting.BaseShip.MidSlot(slotNo) = Nothing
                    Case 8 ' High
                        ParentFitting.BaseShip.HiSlot(slotNo) = Nothing
                    Case 16 ' Subsystem
                        ParentFitting.BaseShip.SubSlot(slotNo) = Nothing
                        removedSubsystems = True
                End Select
                For Each SlotCell As Cell In slot.Cells
                    SlotCell.Text = ""
                Next
                slot.Text = "<Empty>"
                SlotTip.SetSuperTooltip(slot, Nothing)
                slot.Image = CType(My.Resources.ResourceManager.GetObject("Mod01"), Image)
            End If
        Next
        adtSlots.EndUpdate()
        Me.UpdateHistory()
        If removedSubsystems = True Then
            ParentFitting.BaseShip = ParentFitting.BuildSubSystemEffects(ParentFitting.BaseShip)
            Me.UpdateShipSlotLayout()
            ParentFitting.ApplyFitting(BuildType.BuildEverything)
            Call UpdateShipDetails()
        Else
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            Call UpdateShipDetails()
        End If
    End Sub
    Private Sub RemoveModule(ByVal slot As Node, ByVal updateShip As Boolean, ByVal SuppressUndo As Boolean)
        ' Get name of the "slot" which has slot type and number
        Dim sep As Integer = slot.Name.LastIndexOf("_")
        If sep > 0 Then ' Denotes that this is a parent node for the slots
            Dim slotType As Integer = CInt(slot.Name.Substring(0, sep))
            Dim slotNo As Integer = CInt(slot.Name.Substring(sep + 1, 1))
            Dim selMod As New ShipModule
            Select Case slotType
                Case 1 ' Rig
                    selMod = ParentFitting.BaseShip.RigSlot(slotNo)
                Case 2 ' Low
                    selMod = ParentFitting.BaseShip.LowSlot(slotNo)
                Case 4 ' Mid
                    selMod = ParentFitting.BaseShip.MidSlot(slotNo)
                Case 8 ' High
                    selMod = ParentFitting.BaseShip.HiSlot(slotNo)
                Case 16 ' Subsystem
                    selMod = ParentFitting.BaseShip.SubSlot(slotNo)
            End Select
            ' Check for command processor usage
            If selMod IsNot Nothing Then
                If selMod.ID = "11014" Then
                    ParentFitting.BaseShip.Attributes("10063") -= 1
                End If
                If SuppressUndo = False Then
                    If selMod.LoadedCharge IsNot Nothing Then
                        Me.UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveModule, slotType, slotNo, selMod.Name, selMod.LoadedCharge.Name, slotNo, "", ""))
                    Else
                        Me.UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveModule, slotType, slotNo, selMod.Name, "", slotNo, "", ""))
                    End If
                    Me.UpdateHistory()
                End If
                Select Case slotType
                    Case 1 ' Rig
                        ParentFitting.BaseShip.RigSlot(slotNo) = Nothing
                    Case 2 ' Low
                        ParentFitting.BaseShip.LowSlot(slotNo) = Nothing
                    Case 4 ' Mid
                        ParentFitting.BaseShip.MidSlot(slotNo) = Nothing
                    Case 8 ' High
                        ParentFitting.BaseShip.HiSlot(slotNo) = Nothing
                    Case 16 ' Subsystem
                        ParentFitting.BaseShip.SubSlot(slotNo) = Nothing
                End Select
                adtSlots.BeginUpdate()
                For Each SlotCell As Cell In slot.Cells
                    SlotCell.Text = ""
                Next
                slot.Text = "<Empty>"
                SlotTip.SetSuperTooltip(slot, Nothing)
                slot.Image = CType(My.Resources.ResourceManager.GetObject("Mod01"), Image)
                adtSlots.EndUpdate()
                If updateShip = True Then
                    If slotType = 16 Then
                        ParentFitting.BaseShip = ParentFitting.BuildSubSystemEffects(ParentFitting.BaseShip)
                        Me.UpdateShipSlotLayout()
                        ParentFitting.ApplyFitting(BuildType.BuildEverything)
                        Call UpdateShipDetails()
                    Else
                        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                        Call UpdateShipDetails()
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#Region "UI Routines"

    Private Sub ctxSlots_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxSlots.Opening
        If cancelSlotMenu = False Then
            ctxSlots.Items.Clear()
            If adtSlots.SelectedNodes.Count > 0 Then
                If adtSlots.SelectedNodes(0).Nodes.Count = 0 Then
                    Dim currentMod As New ShipModule
                    Dim chargeName As String = ""
                    If adtSlots.SelectedNodes.Count = 1 And adtSlots.SelectedNodes(0).Nodes.Count = 0 Then
                        ' Get the module details
                        Dim modID As String = CStr(ModuleLists.moduleListName.Item(adtSlots.SelectedNodes(0).Text))
                        Dim sep As Integer = adtSlots.SelectedNodes(0).Name.LastIndexOf("_")
                        Dim slotType As Integer = CInt(adtSlots.SelectedNodes(0).Name.Substring(0, sep))
                        Dim slotNo As Integer = CInt(adtSlots.SelectedNodes(0).Name.Substring(sep + 1, 1))
                        Select Case slotType
                            Case 1 ' Rig
                                currentMod = ParentFitting.BaseShip.RigSlot(slotNo)
                            Case 2 ' Low
                                currentMod = ParentFitting.BaseShip.LowSlot(slotNo)
                            Case 4 ' Mid
                                currentMod = ParentFitting.BaseShip.MidSlot(slotNo)
                            Case 8 ' High
                                currentMod = ParentFitting.BaseShip.HiSlot(slotNo)
                            Case 16 ' Subsystem
                                currentMod = ParentFitting.BaseShip.SubSlot(slotNo)
                        End Select
                        If currentMod Is Nothing Then
                            Dim FindModuleMenuItem As New ToolStripMenuItem
                            Dim sSep As Integer = adtSlots.SelectedNodes(0).Name.LastIndexOf("_")
                            FindModuleMenuItem.Name = adtSlots.SelectedNodes(0).Name.Substring(0, sSep)
                            FindModuleMenuItem.Text = "Find Module To Fit"
                            AddHandler FindModuleMenuItem.Click, AddressOf Me.FindModuleToFit
                            ctxSlots.Items.Add(FindModuleMenuItem)
                        Else
                            If currentMod.LoadedCharge IsNot Nothing Then
                                chargeName = currentMod.LoadedCharge.Name
                            End If
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
                            ' Add the Show Meta Variations menu item
                            Dim showMetaVariationsMenuItem As New ToolStripMenuItem
                            showMetaVariationsMenuItem.Name = currentMod.Name
                            showMetaVariationsMenuItem.Text = "Show Meta Variations"
                            AddHandler showMetaVariationsMenuItem.Click, AddressOf Me.ShowMetaVariations
                            ctxSlots.Items.Add(showMetaVariationsMenuItem)
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
                                    If Me.ParentFitting.ShipName = Affects(0) Then
                                        If RelModuleSkills.Contains(Affects(3)) = False Then
                                            RelModuleSkills.Add(Affects(3))
                                        End If
                                    End If
                                End If
                                If Affect.Contains(";Subsystem;") = True Then
                                    Affects = Affect.Split((";").ToCharArray)
                                    If RelModuleSkills.Contains(Affects(3)) = False Then
                                        RelModuleSkills.Add(Affects(3))
                                    End If
                                End If
                            Next
                            RelModuleSkills.Sort()
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
                                        If Me.ParentFitting.ShipName = Affects(0) Then
                                            If RelChargeSkills.Contains(Affects(3)) = False Then
                                                RelChargeSkills.Add(Affects(3))
                                            End If
                                        End If
                                    End If
                                    If Affect.Contains(";Subsystem;") = True Then
                                        Affects = Affect.Split((";").ToCharArray)
                                        If RelChargeSkills.Contains(Affects(3)) = False Then
                                            RelChargeSkills.Add(Affects(3))
                                        End If
                                    End If
                                Next
                            End If
                            RelChargeSkills.Sort()
                            If RelModuleSkills.Count > 0 Or RelChargeSkills.Count > 0 Then
                                ' Add the Main menu item
                                Dim AlterRelevantSkills As New ToolStripMenuItem
                                AlterRelevantSkills.Name = currentMod.Name
                                AlterRelevantSkills.Text = "Alter Relevant Skills"
                                For Each relSkill As String In RelModuleSkills
                                    Dim newRelSkill As New ToolStripMenuItem
                                    newRelSkill.Name = relSkill
                                    newRelSkill.Text = relSkill
                                    Dim pilotLevel As Integer = 0
                                    If CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet.Contains(relSkill) Then
                                        pilotLevel = CType(CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet(relSkill), HQFSkill).Level
                                    Else
                                        MessageBox.Show("There is a mis-match of roles for the " & ParentFitting.BaseShip.Name & ". Please report this to the EveHQ Developers.", "Ship Role Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    End If
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
                                    Dim defaultLevel As Integer = 0
                                    If CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills.Contains(relSkill) = True Then
                                        defaultLevel = CType(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(relSkill), EveHQ.Core.PilotSkill).Level
                                    Else
                                    End If
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
                                    Dim pilotLevel As Integer = 0
                                    If CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet.Contains(relSkill) Then
                                        pilotLevel = CType(CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet(relSkill), HQFSkill).Level
                                    Else
                                        MessageBox.Show("There is a mis-match of roles for the " & ParentFitting.BaseShip.Name & ". Please report this to the EveHQ Developers.", "Ship Role Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    End If
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
                                    Dim defaultLevel As Integer = 0
                                    If CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills.Contains(relSkill) = True Then
                                        defaultLevel = CType(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(relSkill), EveHQ.Core.PilotSkill).Level
                                    End If
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
                            If slotType <> 1 And slotType <> 16 Then
                                Dim canDeactivate As Boolean = False
                                Dim canOverload As Boolean = False
                                ctxSlots.Items.Add("-")
                                Dim statusMenuItem As New ToolStripMenuItem
                                statusMenuItem.Name = adtSlots.SelectedNodes(0).Name
                                statusMenuItem.Text = "Set Module Status"
                                ' Check for activation cost
                                If currentMod.Attributes.ContainsKey("6") = True Or currentMod.Attributes.ContainsKey("669") Or currentMod.IsTurret Or currentMod.IsLauncher Or currentMod.Attributes.ContainsKey("713") Then
                                    canDeactivate = True
                                End If
                                If currentMod.Attributes.ContainsKey("1211") = True Then
                                    canOverload = True
                                End If
                                Dim offlineStatusMenu As New ToolStripMenuItem
                                offlineStatusMenu.Name = adtSlots.SelectedNodes(0).Name
                                offlineStatusMenu.Text = "Offline"
                                offlineStatusMenu.Tag = currentMod
                                AddHandler offlineStatusMenu.Click, AddressOf Me.SetModuleOffline
                                statusMenuItem.DropDownItems.Add(offlineStatusMenu)
                                Dim inactiveStatusMenu As New ToolStripMenuItem
                                If canDeactivate = True Then
                                    inactiveStatusMenu.Name = adtSlots.SelectedNodes(0).Name
                                    inactiveStatusMenu.Text = "Inactive"
                                    inactiveStatusMenu.Tag = currentMod
                                    AddHandler inactiveStatusMenu.Click, AddressOf Me.SetModuleInactive
                                    statusMenuItem.DropDownItems.Add(inactiveStatusMenu)
                                End If
                                Dim activeStatusMenu As New ToolStripMenuItem
                                activeStatusMenu.Name = adtSlots.SelectedNodes(0).Name
                                activeStatusMenu.Text = "Active"
                                activeStatusMenu.Tag = currentMod
                                AddHandler activeStatusMenu.Click, AddressOf Me.SetModuleActive
                                statusMenuItem.DropDownItems.Add(activeStatusMenu)
                                Dim OverloadStatusMenu As New ToolStripMenuItem
                                If canOverload = True Then
                                    OverloadStatusMenu.Name = adtSlots.SelectedNodes(0).Name
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
						ctxSlots.Tag = currentMod
                    Else
                        Dim modulesPresent As Boolean = False
                        For Each slot As Node In adtSlots.SelectedNodes
                            If slot.Text <> "<Empty>" Then
                                modulesPresent = True
                                Dim modID As String = CStr(ModuleLists.moduleListName.Item(slot.Text))
                                Dim sep As Integer = slot.Name.LastIndexOf("_")
                                Dim slotType As Integer = CInt(slot.Name.Substring(0, sep))
                                Dim slotNo As Integer = CInt(slot.Name.Substring(sep + 1, 1))
                                Select Case slotType
                                    Case 1 ' Rig
                                        currentMod = ParentFitting.BaseShip.RigSlot(slotNo)
                                    Case 2 ' Low
                                        currentMod = ParentFitting.BaseShip.LowSlot(slotNo)
                                    Case 4 ' Mid
                                        currentMod = ParentFitting.BaseShip.MidSlot(slotNo)
                                    Case 8 ' High
                                        currentMod = ParentFitting.BaseShip.HiSlot(slotNo)
                                    Case 16 ' High
                                        currentMod = ParentFitting.BaseShip.SubSlot(slotNo)
                                End Select
                                If currentMod.LoadedCharge IsNot Nothing Then
                                    chargeName = currentMod.LoadedCharge.Name
                                End If
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
                    If adtSlots.SelectedNodes.Count > 1 Then
                        Dim marketGroup As String = currentMod.MarketGroup
                        For Each selNodes As Node In adtSlots.SelectedNodes
                            If selNodes.Text <> "<Empty>" Then
                                cMod = CType(ModuleLists.moduleList(CStr(ModuleLists.moduleListName.Item(selNodes.Text))), ShipModule)
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

                    If ShowCharges = True And currentMod IsNot Nothing Then

                        ' Get the charge group and item data
                        Dim chargeGroups As New ArrayList
                        Dim chargeGroupData() As String
                        Dim chargeItems As New SortedList
                        Dim groupName As String = ""
                        For Each chargeGroup As String In Charges.ChargeGroups
                            chargeGroupData = chargeGroup.Split("_".ToCharArray)
                            If currentMod.Charges.Contains(chargeGroupData(1)) = True Then
                                If Market.MarketGroupList.ContainsKey(chargeGroupData(0)) = True Then
                                    Select Case Market.MarketGroupList.Item(chargeGroupData(0)).ToString
                                        Case "Small", "Medium", "Large", "Extra Large"
                                            Dim pathLine As String = CStr(Market.MarketGroupPath(chargeGroupData(0)))
                                            Dim paths() As String = pathLine.Split("\".ToCharArray)
                                            groupName = paths(paths.GetUpperBound(0) - 1)
                                        Case Else
                                            groupName = Market.MarketGroupList.Item(chargeGroupData(0)).ToString
                                    End Select
                                End If
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
                                If adtSlots.SelectedNodes.Count > 1 Then
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
                    ' Add the configure display option
                    Dim SlotInfo As New ToolStripMenuItem
                    SlotInfo.Name = "SlotConfiguration"
                    SlotInfo.Text = "Configure Slot Display"
                    AddHandler SlotInfo.Click, AddressOf Me.ConfigureSlotColumns
                    ctxSlots.Items.Add("-")
					ctxSlots.Items.Add(SlotInfo)
					' Add the Requisition option
					Dim ReqInfo As New ToolStripMenuItem
					ReqInfo.Name = "AddToRequisition"
					ReqInfo.Text = "Add to Requisition"
					AddHandler ReqInfo.Click, AddressOf Me.AddModuleToRequisitions
					ctxSlots.Items.Add("-")
                    ctxSlots.Items.Add(ReqInfo)
                    ' Add the Copy and Paste options
                    ctxSlots.Items.Add("-")
                    Dim CopyOption As New ToolStripMenuItem
                    CopyOption.Name = "CopyModule"
                    If adtSlots.SelectedNodes.Count > 1 Then
                        CopyOption.Text = "<Copy Not Available>"
                        CopyOption.Enabled = False
                    Else
                        If adtSlots.SelectedNodes(0).Text <> "<Empty>" Then
                            CopyOption.Text = "Copy " & adtSlots.SelectedNodes(0).Text
                            CopyOption.Enabled = True
                        Else
                            CopyOption.Text = "<Copy Not Available>"
                            CopyOption.Enabled = False
                        End If
                    End If
                    AddHandler CopyOption.Click, AddressOf Me.CopyModule
                    ctxSlots.Items.Add(CopyOption)
                    Dim PasteOption As New ToolStripMenuItem
                    PasteOption.Name = "PasteModule"
                    If Clipboard.ContainsData("ShipModule") = True Then
                        PasteOption.Text = "Paste " & CType(Clipboard.GetData("ShipModule"), ShipModule).Name
                        PasteOption.Enabled = True
                    Else
                        PasteOption.Text = "<Paste Not Available>"
                        PasteOption.Enabled = False
                    End If
                    AddHandler PasteOption.Click, AddressOf Me.PasteModule
                    ctxSlots.Items.Add(PasteOption)
                Else
                    ' Activate the overload rack menu
                    Dim ActiveRack As Node = adtSlots.SelectedNodes(0)
                    Dim OverloadRackMenuItem As New ToolStripMenuItem
                    OverloadRackMenuItem.Name = ActiveRack.Name
                    If ActiveRack.Text.StartsWith("High Slots") Then
                        OverloadRackMenuItem.Text = "Toggle Overload: High Slots"
                        AddHandler OverloadRackMenuItem.Click, AddressOf Me.OverloadRack
                        ctxSlots.Items.Add(OverloadRackMenuItem)
                    Else
                        Select Case ActiveRack.Text
                            Case "Mid Slots", "Low Slots"
                                OverloadRackMenuItem.Text = "Toggle Overload: " & ActiveRack.Text
                                AddHandler OverloadRackMenuItem.Click, AddressOf Me.OverloadRack
                                ctxSlots.Items.Add(OverloadRackMenuItem)
                            Case Else
                                e.Cancel = True
                        End Select
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

    Private Sub OverloadRack(sender As Object, e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Dim SlotType As Integer = 0
        ' Determine slot type
        Select Case menuItem.Text.TrimStart("Toggle Overload: ".ToCharArray)
            Case "High Slots"
                SlotType = SlotTypes.High
            Case "Mid Slots"
                SlotType = SlotTypes.Mid
            Case "Low Slots"
                SlotType = SlotTypes.Low
            Case Else
                SlotType = 0
        End Select

        ' Check status of all slots - if none are overloaded, make all overloaded, if any are overloaded, make none overloaded
        Dim IsOverloaded As Boolean = False
        Select Case SlotType
            Case SlotTypes.High
                For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                    If ParentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                        If ParentFitting.BaseShip.HiSlot(slot).ModuleState = ModuleStates.Overloaded Then
                            IsOverloaded = True
                            Exit For
                        End If
                    End If
                Next
            Case SlotTypes.Mid
                For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                    If ParentFitting.BaseShip.MidSlot(slot) IsNot Nothing Then
                        If ParentFitting.BaseShip.MidSlot(slot).ModuleState = ModuleStates.Overloaded Then
                            IsOverloaded = True
                            Exit For
                        End If
                    End If
                Next
            Case SlotTypes.Low
                For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                    If ParentFitting.BaseShip.LowSlot(slot) IsNot Nothing Then
                        If ParentFitting.BaseShip.LowSlot(slot).ModuleState = ModuleStates.Overloaded Then
                            IsOverloaded = True
                            Exit For
                        End If
                    End If
                Next
        End Select

        If IsOverloaded = False Then
            ' Set all modules to overloaded
            Select Case SlotType
                Case SlotTypes.High
                    For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                        If ParentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                            If ParentFitting.BaseShip.HiSlot(slot).Attributes.ContainsKey("1211") = True Then
                                SetSingleModuleOverloaded(ParentFitting.BaseShip.HiSlot(slot))
                            End If
                        End If
                    Next
                Case SlotTypes.Mid
                    For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                        If ParentFitting.BaseShip.MidSlot(slot) IsNot Nothing Then
                            If ParentFitting.BaseShip.MidSlot(slot).Attributes.ContainsKey("1211") = True Then
                                SetSingleModuleOverloaded(ParentFitting.BaseShip.MidSlot(slot))
                            End If
                        End If
                    Next
                Case SlotTypes.Low
                    For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                        If ParentFitting.BaseShip.LowSlot(slot) IsNot Nothing Then
                            If ParentFitting.BaseShip.LowSlot(slot).Attributes.ContainsKey("1211") = True Then
                                SetSingleModuleOverloaded(ParentFitting.BaseShip.LowSlot(slot))
                            End If
                        End If
                    Next
            End Select
        Else
            ' Set all modules to active
            Select Case SlotType
                Case SlotTypes.High
                    For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                        If ParentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                            SetSingleModuleActive(ParentFitting.BaseShip.HiSlot(slot))
                        End If
                    Next
                Case SlotTypes.Mid
                    For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                        If ParentFitting.BaseShip.MidSlot(slot) IsNot Nothing Then
                            SetSingleModuleActive(ParentFitting.BaseShip.MidSlot(slot))
                        End If
                    Next
                Case SlotTypes.Low
                    For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                        If ParentFitting.BaseShip.LowSlot(slot) IsNot Nothing Then
                            SetSingleModuleActive(ParentFitting.BaseShip.LowSlot(slot))
                        End If
                    Next
            End Select
        End If
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
    End Sub

    Private Sub FindModuleToFit(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectedSlot As Node = adtSlots.SelectedNodes(0)
        Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
        Dim modData As New ArrayList
        modData.Add(slotInfo(0))
        modData.Add(ParentFitting.FittedShip.CPU - ParentFitting.FittedShip.CPU_Used)
        modData.Add(ParentFitting.FittedShip.PG - ParentFitting.FittedShip.PG_Used)
        modData.Add(ParentFitting.FittedShip.Calibration - ParentFitting.FittedShip.Calibration_Used)
        modData.Add(ParentFitting.FittedShip.LauncherSlots - ParentFitting.FittedShip.LauncherSlots_Used)
        modData.Add(ParentFitting.FittedShip.TurretSlots - ParentFitting.FittedShip.TurretSlots_Used)
        HQF.HQFEvents.StartFindModule = modData
	End Sub

    Private Sub ShowInfo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectedSlot As Node = adtSlots.SelectedNodes(0)
        Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
        Dim sModule As New ShipModule
        Select Case CInt(slotInfo(0))
            Case 1 ' Rig
                sModule = ParentFitting.FittedShip.RigSlot(CInt(slotInfo(1)))
            Case 2 ' Low
                sModule = ParentFitting.FittedShip.LowSlot(CInt(slotInfo(1)))
            Case 4 ' Mid
                sModule = ParentFitting.FittedShip.MidSlot(CInt(slotInfo(1)))
            Case 8 ' High
                sModule = ParentFitting.FittedShip.HiSlot(CInt(slotInfo(1)))
            Case 16 ' Subsystem
                sModule = ParentFitting.FittedShip.SubSlot(CInt(slotInfo(1)))
        End Select
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        If currentInfo IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem), Core.Pilot)
        Else
            If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot)
            Else
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot)
            End If
        End If
        showInfo.ShowItemDetails(sModule, hPilot)
	End Sub

    Private Sub ShowChargeInfo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectedSlot As Node = adtSlots.SelectedNodes(0)
        Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
        Dim sModule As New ShipModule
        Select Case CInt(slotInfo(0))
            Case 1 ' Rig
                sModule = ParentFitting.FittedShip.RigSlot(CInt(slotInfo(1))).LoadedCharge
            Case 2 ' Low
                sModule = ParentFitting.FittedShip.LowSlot(CInt(slotInfo(1))).LoadedCharge
            Case 4 ' Mid
                sModule = ParentFitting.FittedShip.MidSlot(CInt(slotInfo(1))).LoadedCharge
            Case 8 ' High
                sModule = ParentFitting.FittedShip.HiSlot(CInt(slotInfo(1))).LoadedCharge
            Case 16 ' Subsystem
                sModule = ParentFitting.FittedShip.SubSlot(CInt(slotInfo(1))).LoadedCharge
        End Select
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        If currentInfo IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem), Core.Pilot)
        Else
            If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot)
            Else
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot)
            End If
        End If

        showInfo.ShowItemDetails(sModule, hPilot)
	End Sub

    Private Sub AnalyseAmmo(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Display the ammo types available by this module
        Dim AmmoAnalysis As New frmGunnery
        AmmoAnalysis.CurrentFit = ParentFitting
        AmmoAnalysis.CurrentPilot = CType(HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQFPilot)
        AmmoAnalysis.CurrentSlot = adtSlots.SelectedNodes(0).Name
        AmmoAnalysis.ShowDialog()
        AmmoAnalysis.Dispose()
	End Sub

    Private Sub ConfigureSlotColumns(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Open options form
        Dim mySettings As New frmHQFSettings
        mySettings.Tag = "nodeSlotFormat"
        mySettings.ShowDialog()
        mySettings = Nothing
    End Sub

	Private Sub AddModuleToRequisitions(ByVal sender As Object, ByVal e As System.EventArgs)
		' Set up a new Sortedlist to store the required items
		Dim Orders As New SortedList(Of String, Integer)
		' Add the current ship
		For Each SelNode As Node In adtSlots.SelectedNodes
			If Orders.ContainsKey(SelNode.Text) = False Then
				Orders.Add(SelNode.Text, 1)
			Else
				Orders(SelNode.Text) += 1
			End If
		Next
		' Setup the Requisition form for HQF and open it
		Dim newReq As New EveHQ.Core.frmAddRequisition("HQF", Orders)
		newReq.ShowDialog()
	End Sub

    Private Sub ShowModuleMarketGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ShowMarketMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim moduleName As String = ShowMarketMenu.Name
        Dim moduleID As String = CStr(ModuleLists.moduleListName(moduleName))
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim pathLine As String = CStr(Market.MarketGroupPath(cModule.MarketGroup))
        HQFEvents.DisplayedMarketGroup = pathLine
	End Sub

    Private Sub ShowMetaVariations(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ShowMarketMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim moduleName As String = ShowMarketMenu.Name
		Dim moduleID As String = CStr(ModuleLists.moduleListName(moduleName))
		Dim cModule As ShipModule
		If TypeOf ctxSlots.Tag Is ShipModule Then
			cModule = CType(ctxSlots.Tag, ShipModule)
		Else
			cModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
		End If
        Dim newComparison As New frmMetaVariations(Me.ParentFitting, cModule)
        newComparison.Size = HQF.Settings.HQFSettings.MetaVariationsFormSize
		newComparison.ShowDialog()
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
        ToolTip1.SetToolTip(pbShipInfo, SquishText(ParentFitting.BaseShip.Description))
    End Sub

    Private Sub SetPilotSkillLevel(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mnuPilotLevel As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim hPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQFPilot)
        Dim pilotSkill As HQFSkill = CType(hPilot.SkillSet(mnuPilotLevel.Name.Substring(0, mnuPilotLevel.Name.Length - 1)), HQFSkill)
        Dim level As Integer = CInt(mnuPilotLevel.Name.Substring(mnuPilotLevel.Name.Length - 1))
        If level <> pilotSkill.Level Then
            pilotSkill.Level = level
            ParentFitting.ApplyFitting(BuildType.BuildEverything)
        End If
        ' Trigger an update of all open ship fittings!
        HQFEvents.StartUpdateShipInfo = hPilot.PilotName
    End Sub

    Private Function SquishText(ByVal text As String) As String
        Dim MaxLength As Integer = 80
        Dim words() As String = text.Split(" ".ToCharArray)
        Dim newText As New StringBuilder
        Dim charCount As Integer = 0
        For c As Integer = 0 To words.Length - 1
            If charCount + words(c).Length > MaxLength Then
                newText.AppendLine("")
                charCount = 0
            End If
            newText.Append(words(c) & " ")
            charCount += words(c).Length
            If words(c).Contains(ControlChars.CrLf) Then
                charCount = 0
            End If
        Next
        Return newText.ToString
    End Function

    Private Sub ExpandableSplitter1_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles ExpandableSplitter1.SplitterMoved
        HQF.Settings.HQFSettings.StorageBayHeight = tcStorage.Height
    End Sub

    Private Sub CopyModule(ByVal sender As Object, ByVal e As System.EventArgs)
        Call Me.CopyModulesToClipboard()
    End Sub

    Private Sub PasteModule(ByVal sender As Object, ByVal e As System.EventArgs)
        Call Me.PasteModuleFromClipboard()
    End Sub

    Private Sub btnAutoSize_Click(sender As System.Object, e As System.EventArgs) Handles btnAutoSize.Click
        Call Me.AutoSizeAllColumns()
    End Sub

    Private Sub AutoSizeAllColumns()
        adtSlots.SuspendLayout()
        For Each c As DevComponents.AdvTree.ColumnHeader In adtSlots.Columns
            c.AutoSize()
        Next
        adtSlots.ResumeLayout()
    End Sub

#End Region

#Region "Set Module Status"
    Private Sub SetModuleOffline(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        sModule.ModuleState = ModuleStates.Offline
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
    End Sub
    Private Sub SetModuleInactive(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        sModule.ModuleState = ModuleStates.Inactive
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
    End Sub
    Private Sub SetModuleActive(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Call SetSingleModuleActive(sModule)
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
    End Sub
    Private Sub SetModuleOverload(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
        Call SetSingleModuleOverloaded(sModule)
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
    End Sub

    Private Sub SetSingleModuleActive(sModule As ShipModule)

        Dim fModule As New ShipModule
        Select Case sModule.SlotType
            Case SlotTypes.Rig  ' Rig
                fModule = ParentFitting.FittedShip.RigSlot(sModule.SlotNo)
            Case SlotTypes.Low  ' Low
                fModule = ParentFitting.FittedShip.LowSlot(sModule.SlotNo)
            Case SlotTypes.Mid  ' Mid
                fModule = ParentFitting.FittedShip.MidSlot(sModule.SlotNo)
            Case SlotTypes.High  ' High
                fModule = ParentFitting.FittedShip.HiSlot(sModule.SlotNo)
            Case SlotTypes.Subsystem  ' Subsystem
                fModule = ParentFitting.FittedShip.SubSlot(sModule.SlotNo)
        End Select
        Dim oldState As ModuleStates = sModule.ModuleState
        sModule.ModuleState = ModuleStates.Active
        ' Check for maxGroupActive flag
        If sModule.Attributes.ContainsKey("763") = True Then
            If sModule.DatabaseGroup <> "316" Then
                If ParentFitting.IsModuleGroupLimitExceeded(sModule, False) = True Then
                    ' Set the module offline
                    MessageBox.Show("You cannot activate the " & sModule.Name & " due to a restriction on the maximum number permitted for this group.", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    sModule.ModuleState = oldState
                    Exit Sub
                End If
            Else
                If ParentFitting.IsModuleGroupLimitExceeded(sModule, False) = True Then
                    ' Set the module offline
                    MessageBox.Show("You cannot activate the " & sModule.Name & " due to a restriction on the maximum number permitted for this group.", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    sModule.ModuleState = oldState
                    Exit Sub
                Else
                    If ParentFitting.CountActiveTypeModules(sModule.ID) > CInt(fModule.Attributes("763")) Then
                        ' Set the module offline
                        MessageBox.Show("You cannot activate the " & sModule.Name & " due to a restriction on the maximum number permitted for this type.", "Module Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        sModule.ModuleState = oldState
                        Exit Sub
                    End If
                End If
            End If
        End If
        ' Check for activation of siege mode with remote effects
        If sModule.ID = "20280" Then
            If ParentFitting.FittedShip.RemoteSlotCollection.Count > 0 Then
                Dim msg As String = "You have active remote modules and activating Siege Mode will cancel these effects. Do you wish to continue activating Siege Mode?"
                Dim reply As Integer = MessageBox.Show(msg, "Confirm Activate Siege Mode", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.No Then
                    sModule.ModuleState = oldState
                    Exit Sub
                Else
                    ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                    Call Me.ResetRemoteEffects()
                End If
            End If
        End If
        ' Check for activation of triage mode with remote effects
        If sModule.ID = "27951" Then
            If ParentFitting.FittedShip.RemoteSlotCollection.Count > 0 Then
                Dim msg As String = "You have active remote modules and activating Traige Mode will cancel these effects. Do you wish to continue activating Triage Mode?"
                Dim reply As Integer = MessageBox.Show(msg, "Confirm Activate Triage Mode", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.No Then
                    sModule.ModuleState = oldState
                    Exit Sub
                Else
                    ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                    Call Me.ResetRemoteEffects()
                End If
            End If
        End If
    End Sub

    Private Sub SetSingleModuleOverloaded(sModule As ShipModule)
        Dim fModule As New ShipModule
        Select Case sModule.SlotType
            Case SlotTypes.Rig  ' Rig
                fModule = ParentFitting.FittedShip.RigSlot(sModule.SlotNo)
            Case SlotTypes.Low  ' Low
                fModule = ParentFitting.FittedShip.LowSlot(sModule.SlotNo)
            Case SlotTypes.Mid  ' Mid
                fModule = ParentFitting.FittedShip.MidSlot(sModule.SlotNo)
            Case SlotTypes.High  ' High
                fModule = ParentFitting.FittedShip.HiSlot(sModule.SlotNo)
            Case SlotTypes.Subsystem  ' Subsystem
                fModule = ParentFitting.FittedShip.SubSlot(sModule.SlotNo)
        End Select
        Dim oldState As ModuleStates = sModule.ModuleState
        sModule.ModuleState = ModuleStates.Overloaded
        ' Check for maxGroupActive flag
        If sModule.Attributes.ContainsKey("763") = True Then
            If sModule.DatabaseGroup <> "316" Then
                If ParentFitting.IsModuleGroupLimitExceeded(sModule, False) = True Then
                    ' Set the module offline
                    MessageBox.Show("You cannot activate the " & sModule.Name & " due to a restriction on the maximum number permitted for this group.", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    sModule.ModuleState = oldState
                    Exit Sub
                End If
            Else
                If ParentFitting.IsModuleGroupLimitExceeded(sModule, False) = True Then
                    ' Set the module offline
                    MessageBox.Show("You cannot activate the " & sModule.Name & " due to a restriction on the maximum number permitted for this group.", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    sModule.ModuleState = oldState
                    Exit Sub
                Else
                    If ParentFitting.CountActiveTypeModules(sModule.ID) > CInt(fModule.Attributes("763")) Then
                        ' Set the module offline
                        MessageBox.Show("You cannot activate the " & sModule.Name & " due to a restriction on the maximum number permitted for this type.", "Module Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        sModule.ModuleState = oldState
                        Exit Sub
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#Region "Load/Remove Charges"

    Private Sub RemoveChargeFromModule(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each selItem As Node In adtSlots.SelectedNodes
            Call Me.RemoveSingleChargeFromModule(selItem, False)
        Next
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        Call Me.UpdateHistory()
    End Sub

    Private Sub RemoveSingleChargeFromModule(ByVal selItem As Node, ByVal SuppressUndo As Boolean)
        Dim sep As Integer = selItem.Name.LastIndexOf("_")
        Dim slotType As Integer = CInt(selItem.Name.Substring(0, sep))
        Dim slotNo As Integer = CInt(selItem.Name.Substring(sep + 1, 1))
        Dim LoadedModule As New ShipModule
        Select Case slotType
            Case 1 ' Rig
                LoadedModule = ParentFitting.BaseShip.RigSlot(slotNo)
            Case 2 ' Low
                LoadedModule = ParentFitting.BaseShip.LowSlot(slotNo)
            Case 4 ' Mid
                LoadedModule = ParentFitting.BaseShip.MidSlot(slotNo)
            Case 8 ' High
                LoadedModule = ParentFitting.BaseShip.HiSlot(slotNo)
            Case 16 ' Subsystem
                LoadedModule = ParentFitting.BaseShip.SubSlot(slotNo)
        End Select
        If SuppressUndo = False Then
            UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveCharge, slotType, slotNo, LoadedModule.Name, LoadedModule.LoadedCharge.Name, slotNo, LoadedModule.Name, ""))
        End If
        LoadedModule.LoadedCharge = Nothing
    End Sub

    Private Sub LoadChargeIntoModule(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ChargeMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim moduleID As String = ChargeMenu.Name
        ' Get name of the "slot" which has slot type and number
        For Each selItem As Node In adtSlots.SelectedNodes
            Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule).Clone
            Call Me.LoadSingleChargeIntoModule(selItem, Charge, False)
        Next
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        Call Me.UpdateHistory()
    End Sub

    Private Sub LoadSingleChargeIntoModule(ByVal selItem As Node, ByVal Charge As ShipModule, ByVal SuppressUndo As Boolean)
        If selItem.Text <> "<Empty>" Then
            Dim sep As Integer = selItem.Name.LastIndexOf("_")
            Dim slotType As Integer = CInt(selItem.Name.Substring(0, sep))
            Dim slotNo As Integer = CInt(selItem.Name.Substring(sep + 1, 1))
            Dim LoadedModule As New ShipModule
            Select Case slotType
                Case 1 ' Rig
                    LoadedModule = ParentFitting.BaseShip.RigSlot(slotNo)
                Case 2 ' Low
                    LoadedModule = ParentFitting.BaseShip.LowSlot(slotNo)
                Case 4 ' Mid
                    LoadedModule = ParentFitting.BaseShip.MidSlot(slotNo)
                Case 8 ' High
                    LoadedModule = ParentFitting.BaseShip.HiSlot(slotNo)
                Case 16 ' Subsystem
                    LoadedModule = ParentFitting.BaseShip.SubSlot(slotNo)
            End Select
            Dim OldChargeName As String = ""
            If LoadedModule.LoadedCharge IsNot Nothing Then
                OldChargeName = LoadedModule.LoadedCharge.Name
            End If
            LoadedModule.LoadedCharge = Charge
            If SuppressUndo = False Then
                UndoStack.Push(New UndoInfo(UndoInfo.TransType.AddCharge, slotType, slotNo, LoadedModule.Name, OldChargeName, slotNo, LoadedModule.Name, LoadedModule.LoadedCharge.Name))
            End If
        End If
    End Sub
#End Region

#Region "Storage Bay Routines"
    Private Sub RedrawCargoBay()
        lvwCargoBay.BeginUpdate()
        lvwCargoBay.Items.Clear()
        ParentFitting.BaseShip.CargoBay_Used = 0
        ParentFitting.BaseShip.CargoBay_Additional = 0
        Dim CBI As CargoBayItem
        Dim HoldingBay As New SortedList
        For Each CBI In ParentFitting.BaseShip.CargoBayItems.Values
            HoldingBay.Add(HoldingBay.Count, CBI)
        Next
        ParentFitting.BaseShip.CargoBayItems.Clear()
        For Each CBI In HoldingBay.Values
            Dim newCargoItem As New ListViewItem(CBI.ItemType.Name)
            newCargoItem.Name = CStr(lvwCargoBay.Items.Count)
            newCargoItem.SubItems.Add(CStr(CBI.Quantity))
            ParentFitting.BaseShip.CargoBayItems.Add(lvwCargoBay.Items.Count, CBI)
            ParentFitting.BaseShip.CargoBay_Used += CBI.ItemType.Volume * CBI.Quantity
            If CBI.ItemType.IsContainer Then ParentFitting.BaseShip.CargoBay_Additional += (CBI.ItemType.Capacity - CBI.ItemType.Volume) * CBI.Quantity
            lvwCargoBay.Items.Add(newCargoItem)
        Next
        lvwCargoBay.EndUpdate()
        Call Me.RedrawCargoBayCapacity()
    End Sub

    Private Sub RedrawDroneBay()
        lvwDroneBay.BeginUpdate()
        lvwDroneBay.Items.Clear()
        ParentFitting.BaseShip.DroneBay_Used = 0
        Dim DBI As DroneBayItem
        Dim HoldingBay As New SortedList
        For Each DBI In ParentFitting.BaseShip.DroneBayItems.Values
            HoldingBay.Add(HoldingBay.Count, DBI)
        Next
        ParentFitting.BaseShip.DroneBayItems.Clear()
        For Each DBI In HoldingBay.Values
            Dim newDroneItem As New ListViewItem(DBI.DroneType.Name)
            newDroneItem.Name = CStr(lvwDroneBay.Items.Count)
            newDroneItem.SubItems.Add(CStr(DBI.Quantity))
            If DBI.IsActive = True Then
                newDroneItem.Checked = True
            End If
            ParentFitting.BaseShip.DroneBayItems.Add(lvwDroneBay.Items.Count, DBI)
            ParentFitting.BaseShip.DroneBay_Used += DBI.DroneType.Volume * DBI.Quantity
            lvwDroneBay.Items.Add(newDroneItem)
        Next
        lvwDroneBay.EndUpdate()
        Call Me.RedrawDroneBayCapacity()
    End Sub

    Private Sub RedrawShipBay()
        lvwShipBay.BeginUpdate()
        lvwShipBay.Items.Clear()
        ParentFitting.BaseShip.ShipBay_Used = 0
        Dim SBI As ShipBayItem
        Dim HoldingBay As New SortedList
        For Each SBI In ParentFitting.BaseShip.ShipBayItems.Values
            HoldingBay.Add(HoldingBay.Count, SBI)
        Next
        ParentFitting.BaseShip.ShipBayItems.Clear()
        For Each SBI In HoldingBay.Values
            Dim newCargoItem As New ListViewItem(SBI.ShipType.Name)
            newCargoItem.Name = CStr(lvwShipBay.Items.Count)
            newCargoItem.SubItems.Add(CStr(SBI.Quantity))
            newCargoItem.SubItems.Add(SBI.ShipType.Volume.ToString("N0"))
            newCargoItem.SubItems.Add((SBI.ShipType.Volume * SBI.Quantity).ToString("N0"))
            ParentFitting.BaseShip.ShipBayItems.Add(lvwShipBay.Items.Count, SBI)
            ParentFitting.BaseShip.ShipBay_Used += SBI.ShipType.Volume * SBI.Quantity
            lvwShipBay.Items.Add(newCargoItem)
        Next
        lvwShipBay.EndUpdate()
        Call Me.RedrawShipBayCapacity()
    End Sub

    Private Sub RedrawCargoBayCapacity()
        If ParentFitting.BaseShip.CargoBay_Additional > 0 Then
            lblCargoBay.Text = ParentFitting.BaseShip.CargoBay_Used.ToString("N2") & " / " & ParentFitting.FittedShip.CargoBay.ToString("N2") & " (" & (ParentFitting.FittedShip.CargoBay + ParentFitting.BaseShip.CargoBay_Additional).ToString("N2") & ") m³"
        Else
            lblCargoBay.Text = ParentFitting.BaseShip.CargoBay_Used.ToString("N2") & " / " & ParentFitting.FittedShip.CargoBay.ToString("N2") & " m³"
        End If
        If ParentFitting.FittedShip.CargoBay > 0 Then
            pbCargoBay.Maximum = CInt(ParentFitting.FittedShip.CargoBay)
        Else
            pbCargoBay.Maximum = 1
        End If
        If ParentFitting.BaseShip.CargoBay_Used > ParentFitting.FittedShip.CargoBay Then
            pbCargoBay.Value = CInt(ParentFitting.FittedShip.CargoBay)
        Else
            pbCargoBay.Value = CInt(ParentFitting.BaseShip.CargoBay_Used)
        End If
    End Sub

    Private Sub RedrawDroneBayCapacity()
        lvwDroneBay.EndUpdate()
        lblDroneBay.Text = ParentFitting.BaseShip.DroneBay_Used.ToString("N2") & " / " & ParentFitting.FittedShip.DroneBay.ToString("N2") & " m³"
        If ParentFitting.FittedShip.DroneBay > 0 Then
            pbDroneBay.Maximum = CInt(ParentFitting.FittedShip.DroneBay)
        Else
            pbDroneBay.Maximum = 1
        End If
        If ParentFitting.BaseShip.DroneBay_Used > ParentFitting.FittedShip.DroneBay Then
            pbDroneBay.Value = CInt(ParentFitting.FittedShip.DroneBay)
        Else
            pbDroneBay.Value = CInt(ParentFitting.BaseShip.DroneBay_Used)
        End If
    End Sub

    Private Sub RedrawShipBayCapacity()
        lblShipBay.Text = ParentFitting.BaseShip.ShipBay_Used.ToString("N2") & " / " & ParentFitting.FittedShip.ShipBay.ToString("N2") & " m³"
        If ParentFitting.FittedShip.ShipBay > 0 Then
            pbShipBay.Maximum = CInt(ParentFitting.FittedShip.ShipBay)
        Else
            pbShipBay.Maximum = 1
        End If
        If ParentFitting.BaseShip.ShipBay_Used > ParentFitting.FittedShip.ShipBay Then
            pbShipBay.Value = CInt(ParentFitting.FittedShip.ShipBay)
        Else
            pbShipBay.Value = CInt(ParentFitting.BaseShip.ShipBay_Used)
        End If
    End Sub

    Private Sub lvwDroneBay_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwDroneBay.ItemChecked
        If UpdateAll = False Or cancelDroneActivation = True Then
            Dim idx As Integer = CInt(e.Item.Name)
            Dim DBI As DroneBayItem = CType(ParentFitting.BaseShip.DroneBayItems.Item(idx), DroneBayItem)
            ' Check we have the bandwidth and/or control ability for this item
            If UpdateDrones = False Then
                Dim reqQ As Integer = DBI.Quantity
                If e.Item.Checked = True Then
                    If ParentFitting.FittedShip.UsedDrones + reqQ > ParentFitting.FittedShip.MaxDrones Then
                        ' Cannot do this because our drone control skill in insufficient
                        MessageBox.Show("You do not have the ability to control this many drones. Please split the group and try again.", "Drone Control Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        cancelDroneActivation = True
                        e.Item.Checked = False
                        Exit Sub
                    End If
                    If ParentFitting.FittedShip.DroneBandwidth_Used + (reqQ * CDbl(DBI.DroneType.Attributes("1272"))) > ParentFitting.FittedShip.DroneBandwidth Then
                        ' Cannot do this because we don't have enough bandwidth
                        MessageBox.Show("You do not have the spare bandwidth to control this many drones. Please split the group and try again.", "Drone Bandwidth Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        cancelDroneActivation = True
                        e.Item.Checked = False
                        Exit Sub
                    End If
                End If
                DBI.IsActive = e.Item.Checked
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                If DBI.IsActive = True Then
                    ParentFitting.BaseShip.Attributes("10006") = CDbl(ParentFitting.BaseShip.Attributes("10006")) + reqQ
                Else
                    ParentFitting.BaseShip.Attributes("10006") = Math.Max(CDbl(ParentFitting.BaseShip.Attributes("10006")) - reqQ, 0)
                End If
            End If
            Call ParentFitting.UpdateFittingFromBaseShip()
            Call currentInfo.UpdateDroneUsage()
        End If
        cancelDroneActivation = False
    End Sub

    Private Sub ctxBays_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxBays.Opening
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        Select Case lvwBay.Name
            Case "lvwCargoBay"
                If lvwBay.SelectedItems.Count > 0 Then
                    ctxShowBayInfoItem.Text = "Show Item Info"
                    If ctxBays.Items.ContainsKey("Drone Skills") = True Then
                        ctxBays.Items.RemoveByKey("Drone Skills")
                    End If
                    ctxSplitBatch.Enabled = False
                    ctxShowBayInfoItem.Enabled = True
                Else
                    e.Cancel = True
                End If
            Case "lvwShipBay"
                If lvwBay.SelectedItems.Count > 0 Then
                    ctxShowBayInfoItem.Text = "Show Ship Info"
                    If ctxBays.Items.ContainsKey("Drone Skills") = True Then
                        ctxBays.Items.RemoveByKey("Drone Skills")
                    End If
                    ctxSplitBatch.Enabled = False
                    ctxShowBayInfoItem.Enabled = False
                Else
                    e.Cancel = True
                End If
            Case "lvwDroneBay"
                If lvwBay.SelectedItems.Count > 0 Then
                    ctxShowBayInfoItem.Text = "Show Drone Info"
                    ctxShowBayInfoItem.Enabled = True
                    ctxSplitBatch.Enabled = True
                    Dim selItem As ListViewItem = lvwBay.SelectedItems(0)
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim DBI As DroneBayItem = CType(ParentFitting.BaseShip.DroneBayItems.Item(idx), DroneBayItem)
                    Dim currentMod As ShipModule = DBI.DroneType

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
                            If Me.ParentFitting.ShipName = Affects(0) Then
                                If RelModuleSkills.Contains(Affects(3)) = False Then
                                    RelModuleSkills.Add(Affects(3))
                                End If
                            End If
                        End If
                    Next
                    RelModuleSkills.Sort()
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
                                If Me.ParentFitting.ShipName = Affects(0) Then
                                    If RelChargeSkills.Contains(Affects(3)) = False Then
                                        RelChargeSkills.Add(Affects(3))
                                    End If
                                End If
                            End If
                        Next
                    End If
                    RelChargeSkills.Sort()
                    If RelModuleSkills.Count > 0 Or RelChargeSkills.Count > 0 Then
                        ' Add the Main menu item
                        Dim AlterRelevantSkills As New ToolStripMenuItem
                        AlterRelevantSkills.Name = "Drone Skills"
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
                            Dim defaultLevel As Integer = 0
                            If CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills.Contains(relSkill) = True Then
                                defaultLevel = CType(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(relSkill), EveHQ.Core.PilotSkill).Level
                            End If
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
                            Dim defaultLevel As Integer = 0
                            If CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills.Contains(relSkill) = True Then
                                defaultLevel = CType(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(relSkill), EveHQ.Core.PilotSkill).Level
                            End If
                            Dim newRelSkillDefault As New ToolStripMenuItem
                            newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                            newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                            AddHandler newRelSkillDefault.Click, AddressOf Me.SetPilotSkillLevel
                            newRelSkill.DropDownItems.Add(newRelSkillDefault)
                            AlterRelevantSkills.DropDownItems.Add(newRelSkill)
                        Next
                        If ctxBays.Items.ContainsKey("Drone Skills") = True Then
                            ctxBays.Items.RemoveByKey("Drone Skills")
                        End If
                        ctxBays.Items.Add(AlterRelevantSkills)
                    End If
                    If lvwBay.SelectedItems.Count > 1 Then
                        ctxAlterQuantity.Enabled = False
                        ctxSplitBatch.Enabled = False
                    Else
                        ctxAlterQuantity.Enabled = True
                        ctxSplitBatch.Enabled = True
                    End If
                Else
                    e.Cancel = True
                End If
        End Select
    End Sub

    Private Sub ctxRemoveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxRemoveItem.Click
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        Select Case lvwBay.Name
            Case "lvwCargoBay"
                ' Removes the item from the cargo bay
                For Each remItem As ListViewItem In lvwCargoBay.SelectedItems
                    ParentFitting.BaseShip.CargoBayItems.Remove(CInt(remItem.Name))
                    lvwCargoBay.Items.RemoveByKey(remItem.Name)
                Next
                Call ParentFitting.UpdateFittingFromBaseShip()
                Call RedrawCargoBay()
            Case "lvwShipBay"
                ' Removes the item from the cargo bay
                For Each remItem As ListViewItem In lvwShipBay.SelectedItems
                    ParentFitting.BaseShip.ShipBayItems.Remove(CInt(remItem.Name))
                    lvwShipBay.Items.RemoveByKey(remItem.Name)
                Next
                Call ParentFitting.UpdateFittingFromBaseShip()
                Call RedrawShipBay()
            Case "lvwDroneBay"
                ' Removes the item from the drone bay
                For Each remItem As ListViewItem In lvwDroneBay.SelectedItems
                    ParentFitting.BaseShip.DroneBayItems.Remove(CInt(remItem.Name))
                    lvwDroneBay.Items.RemoveByKey(remItem.Name)
                Next
                Call ParentFitting.UpdateFittingFromBaseShip()
                UpdateDrones = True
                Call RedrawDroneBay()
                UpdateDrones = False
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Select
    End Sub

    Private Sub ctxShowBayInfoItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxShowBayInfoItem.Click
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        Dim selItem As ListViewItem = lvwBay.SelectedItems(0)
        Dim sModule As New ShipModule
        Select Case lvwBay.Name
            Case "lvwCargoBay"
                Dim idx As Integer = CInt(selItem.Name)
                Dim DBI As CargoBayItem = CType(ParentFitting.FittedShip.CargoBayItems.Item(idx), CargoBayItem)
                sModule = DBI.ItemType
            Case "lvwDroneBay"
                Dim idx As Integer = CInt(selItem.Name)
                Dim DBI As DroneBayItem = CType(ParentFitting.FittedShip.DroneBayItems.Item(idx), DroneBayItem)
                sModule = DBI.DroneType
        End Select
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        If currentInfo IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem), Core.Pilot)
        Else
            If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot)
            Else
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot)
            End If
        End If
        showInfo.ShowItemDetails(sModule, hPilot)
    End Sub

    Private Sub ctxAlterQuantity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxAlterQuantity.Click
        Dim lvwBay As ListView = CType(ctxBays.SourceControl, ListView)
        Select Case lvwBay.Name
            Case "lvwCargoBay"
                Dim selItem As ListViewItem = lvwCargoBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim CBI As CargoBayItem = CType(ParentFitting.BaseShip.CargoBayItems.Item(idx), CargoBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = ParentFitting.FittedShip
                newSelectForm.CBI = CBI
                newSelectForm.BayType = frmSelectQuantity.BayTypes.CargoBay
                newSelectForm.IsSplit = False
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = CBI.Quantity + CInt(Int((ParentFitting.FittedShip.CargoBay - ParentFitting.BaseShip.CargoBay_Used) / CBI.ItemType.Volume))
                newSelectForm.nudQuantity.Value = CBI.Quantity
                newSelectForm.ShowDialog()
                newSelectForm.Dispose()
                Call ParentFitting.UpdateFittingFromBaseShip()
                Call RedrawCargoBay()
            Case "lvwShipBay"
                Dim selItem As ListViewItem = lvwShipBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim SBI As ShipBayItem = CType(ParentFitting.BaseShip.ShipBayItems.Item(idx), ShipBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = ParentFitting.FittedShip
                newSelectForm.SBI = SBI
                newSelectForm.BayType = frmSelectQuantity.BayTypes.ShipBay
                newSelectForm.IsSplit = False
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = SBI.Quantity + CInt(Int((ParentFitting.FittedShip.ShipBay - ParentFitting.BaseShip.ShipBay_Used) / SBI.ShipType.Volume))
                newSelectForm.nudQuantity.Value = SBI.Quantity
                newSelectForm.ShowDialog()
                newSelectForm.Dispose()
                Call ParentFitting.UpdateFittingFromBaseShip()
                Call RedrawShipBay()
            Case "lvwDroneBay"
                Dim selItem As ListViewItem = lvwDroneBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim DBI As DroneBayItem = CType(ParentFitting.BaseShip.DroneBayItems.Item(idx), DroneBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = ParentFitting.FittedShip
                newSelectForm.DBI = DBI
                newSelectForm.BayType = frmSelectQuantity.BayTypes.DroneBay
                newSelectForm.IsSplit = False
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = DBI.Quantity + CInt(Int((ParentFitting.FittedShip.DroneBay - ParentFitting.BaseShip.DroneBay_Used) / DBI.DroneType.Volume))
                newSelectForm.nudQuantity.Value = DBI.Quantity
                newSelectForm.ShowDialog()
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                Call ParentFitting.UpdateFittingFromBaseShip()
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
                Dim CBI As CargoBayItem = CType(ParentFitting.BaseShip.CargoBayItems.Item(idx), CargoBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = ParentFitting.FittedShip
                newSelectForm.currentShip = ParentFitting.BaseShip
                newSelectForm.CBI = CBI
                newSelectForm.BayType = frmSelectQuantity.BayTypes.CargoBay
                newSelectForm.IsSplit = True
                newSelectForm.nudQuantity.Value = 1
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = CBI.Quantity - 1
                newSelectForm.ShowDialog()
                newSelectForm.Dispose()
                Call ParentFitting.UpdateFittingFromBaseShip()
                Call RedrawCargoBay()
            Case "lvwShipBay"
                Dim selItem As ListViewItem = lvwShipBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim SBI As ShipBayItem = CType(ParentFitting.BaseShip.ShipBayItems.Item(idx), ShipBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = ParentFitting.FittedShip
                newSelectForm.currentShip = ParentFitting.BaseShip
                newSelectForm.SBI = SBI
                newSelectForm.BayType = frmSelectQuantity.BayTypes.ShipBay
                newSelectForm.IsSplit = True
                newSelectForm.nudQuantity.Value = 1
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = SBI.Quantity - 1
                newSelectForm.ShowDialog()
                newSelectForm.Dispose()
                Call ParentFitting.UpdateFittingFromBaseShip()
                Call RedrawShipBay()
            Case "lvwDroneBay"
                Dim selItem As ListViewItem = lvwDroneBay.SelectedItems(0)
                Dim idx As Integer = CInt(selItem.Name)
                Dim DBI As DroneBayItem = CType(ParentFitting.BaseShip.DroneBayItems.Item(idx), DroneBayItem)
                Dim newSelectForm As New frmSelectQuantity
                newSelectForm.fittedShip = ParentFitting.FittedShip
                newSelectForm.currentShip = ParentFitting.BaseShip
                newSelectForm.DBI = DBI
                newSelectForm.BayType = frmSelectQuantity.BayTypes.DroneBay
                newSelectForm.IsSplit = True
                newSelectForm.nudQuantity.Value = 1
                newSelectForm.nudQuantity.Minimum = 1
                newSelectForm.nudQuantity.Maximum = DBI.Quantity - 1
                newSelectForm.ShowDialog()
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                Call ParentFitting.UpdateFittingFromBaseShip()
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
        ParentFitting.BaseShip.DroneBay_Used = 0
        Dim DBI As DroneBayItem
        Dim HoldingBay As New SortedList
        Dim DroneQuantities As New SortedList
        For Each DBI In ParentFitting.BaseShip.DroneBayItems.Values
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
        ParentFitting.BaseShip.DroneBayItems.Clear()
        For Each drone As String In HoldingBay.Keys
            DBI = New DroneBayItem
            DBI.DroneType = CType(HoldingBay(drone), ShipModule)
            DBI.IsActive = False
            DBI.Quantity = CInt(DroneQuantities(drone))
            Dim newDroneItem As New ListViewItem(DBI.DroneType.Name)
            newDroneItem.Name = CStr(lvwDroneBay.Items.Count)
            newDroneItem.SubItems.Add(CStr(DBI.Quantity))
            ParentFitting.BaseShip.DroneBayItems.Add(lvwDroneBay.Items.Count, DBI)
            ParentFitting.BaseShip.DroneBay_Used += DBI.DroneType.Volume * DBI.Quantity
            lvwDroneBay.Items.Add(newDroneItem)
        Next
        lvwDroneBay.EndUpdate()
        Call Me.RedrawDroneBayCapacity()
        UpdateDrones = False
        ' Rebuild the ship to account for any disabled drones
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
    End Sub

    Private Sub btnMergeCargo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMergeCargo.Click
        lvwCargoBay.BeginUpdate()
        lvwCargoBay.Items.Clear()
        ParentFitting.BaseShip.CargoBay_Used = 0
        ParentFitting.BaseShip.CargoBay_Additional = 0
        Dim CBI As CargoBayItem
        Dim HoldingBay As New SortedList
        Dim CargoQuantities As New SortedList
        For Each CBI In ParentFitting.BaseShip.CargoBayItems.Values
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
        ParentFitting.BaseShip.CargoBayItems.Clear()
        For Each Cargo As String In HoldingBay.Keys
            CBI = New CargoBayItem
            CBI.ItemType = CType(HoldingBay(Cargo), ShipModule)
            CBI.Quantity = CInt(CargoQuantities(Cargo))
            Dim newCargoItem As New ListViewItem(CBI.ItemType.Name)
            newCargoItem.Name = CStr(lvwCargoBay.Items.Count)
            newCargoItem.SubItems.Add(CStr(CBI.Quantity))
            ParentFitting.BaseShip.CargoBayItems.Add(lvwCargoBay.Items.Count, CBI)
            ParentFitting.BaseShip.CargoBay_Used += CBI.ItemType.Volume * CBI.Quantity
            If CBI.ItemType.IsContainer Then ParentFitting.BaseShip.CargoBay_Additional += (CBI.ItemType.Capacity - CBI.ItemType.Volume) * CBI.Quantity
            lvwCargoBay.Items.Add(newCargoItem)
        Next
        lvwCargoBay.EndUpdate()
        Call Me.RedrawCargoBayCapacity()
    End Sub

#End Region

#Region "Slot Drag/Drop Routines"

    Private Sub adtSlots_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles adtSlots.DragOver
        adtSlots.DragDropEnabled = False
        Dim oLVI As Node = CType(e.Data.GetData(GetType(Node)), Node)
        If oLVI IsNot Nothing Then
            Dim oSep As Integer = oLVI.Name.LastIndexOf("_")
            Dim oSlotType As Integer = CInt(oLVI.Name.Substring(0, oSep))
            Dim oslotNo As Integer = CInt(oLVI.Name.Substring(oSep + 1, 1))

            Dim p As Point = adtSlots.PointToClient(New Point(e.X, e.Y))
            Dim nLVI As Node = adtSlots.GetNodeAt(p.X, p.Y)
            If nLVI IsNot Nothing Then
                Dim nModID As String = CStr(ModuleLists.moduleListName.Item(nLVI.Text))
                Dim nMod As New ShipModule
                Dim nSep As Integer = nLVI.Name.LastIndexOf("_")
                Dim nSlotType As Integer = CInt(nLVI.Name.Substring(0, nSep))
                Dim nslotNo As Integer = CInt(nLVI.Name.Substring(nSep + 1, 1))
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
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub adtSlots_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles adtSlots.DragDrop
        Dim oLVI As Node = CType(e.Data.GetData(GetType(Node)), Node)
        Dim oModID As String = CStr(ModuleLists.moduleListName.Item(oLVI.Text))

        Dim oSep As Integer = oLVI.Name.LastIndexOf("_")
        Dim oSlotType As Integer = CInt(oLVI.Name.Substring(0, oSep))
        Dim oslotNo As Integer = CInt(oLVI.Name.Substring(oSep + 1, 1))

        Dim p As Point = adtSlots.PointToClient(New Point(e.X, e.Y))
        Dim nLVI As Node = adtSlots.GetNodeAt(p.X, p.Y)
        Dim nModID As String = CStr(ModuleLists.moduleListName.Item(nLVI.Text))

        Dim nSep As Integer = nLVI.Name.LastIndexOf("_")
        Dim nSlotType As Integer = CInt(nLVI.Name.Substring(0, nSep))
        Dim nslotNo As Integer = CInt(nLVI.Name.Substring(nSep + 1, 1))

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
                            ocMod = ParentFitting.BaseShip.RigSlot(oslotNo).Clone
                            ofMod = ParentFitting.FittedShip.RigSlot(oslotNo).Clone
                        Case 2 ' Low
                            ocMod = ParentFitting.BaseShip.LowSlot(oslotNo).Clone
                            ofMod = ParentFitting.FittedShip.LowSlot(oslotNo).Clone
                        Case 4 ' Mid
                            ocMod = ParentFitting.BaseShip.MidSlot(oslotNo).Clone
                            ofMod = ParentFitting.FittedShip.MidSlot(oslotNo).Clone
                        Case 8 ' High
                            ocMod = ParentFitting.BaseShip.HiSlot(oslotNo).Clone
                            ofMod = ParentFitting.FittedShip.HiSlot(oslotNo).Clone
                        Case 16 ' Subsystem
                            ocMod = ParentFitting.BaseShip.SubSlot(oslotNo).Clone
                            ofMod = ParentFitting.FittedShip.SubSlot(oslotNo).Clone
                    End Select
                End If

                If nLVI.Text = "<Empty>" Then
                    ncMod = Nothing
                    nfMod = Nothing
                Else
                    Select Case nSlotType
                        Case 1 ' Rig
                            ncMod = ParentFitting.BaseShip.RigSlot(nslotNo).Clone
                            nfMod = ParentFitting.FittedShip.RigSlot(nslotNo).Clone
                        Case 2 ' Low
                            ncMod = ParentFitting.BaseShip.LowSlot(nslotNo).Clone
                            nfMod = ParentFitting.FittedShip.LowSlot(nslotNo).Clone
                        Case 4 ' Mid
                            ncMod = ParentFitting.BaseShip.MidSlot(nslotNo).Clone
                            nfMod = ParentFitting.FittedShip.MidSlot(nslotNo).Clone
                        Case 8 ' High
                            ncMod = ParentFitting.BaseShip.HiSlot(nslotNo).Clone
                            nfMod = ParentFitting.FittedShip.HiSlot(nslotNo).Clone
                        Case 16 ' Subsystem
                            ncMod = ParentFitting.BaseShip.SubSlot(nslotNo).Clone
                            nfMod = ParentFitting.FittedShip.SubSlot(nslotNo).Clone
                    End Select
                End If
                If e.Effect = DragDropEffects.Move Then ' Mouse button released?
                    'MessageBox.Show("Wanting to swap " & oLVI.Text & " for " & nLVI.Text & "?", "Confirm swap", MessageBoxButtons.OK, MessageBoxIcon.Question)
                    If ocMod Is Nothing Then
                        RemoveModule(nLVI, False, True)
                    Else
                        ocMod.SlotNo = nslotNo
                        Select Case nSlotType
                            Case 1 ' Rig
                                ParentFitting.BaseShip.RigSlot(nslotNo) = ocMod
                                ParentFitting.FittedShip.RigSlot(nslotNo) = ofMod
                            Case 2 ' Low
                                ParentFitting.BaseShip.LowSlot(nslotNo) = ocMod
                                ParentFitting.FittedShip.LowSlot(nslotNo) = ofMod
                            Case 4 ' Mid
                                ParentFitting.BaseShip.MidSlot(nslotNo) = ocMod
                                ParentFitting.FittedShip.MidSlot(nslotNo) = ofMod
                            Case 8 ' High
                                ParentFitting.BaseShip.HiSlot(nslotNo) = ocMod
                                ParentFitting.FittedShip.HiSlot(nslotNo) = ofMod
                            Case 16 ' subsystem
                                ParentFitting.BaseShip.SubSlot(nslotNo) = ocMod
                                ParentFitting.FittedShip.SubSlot(nslotNo) = ofMod
                        End Select
                    End If
                    If ncMod Is Nothing Then
                        RemoveModule(oLVI, False, True)
                    Else
                        ncMod.SlotNo = oslotNo
                        Select Case oSlotType
                            Case 1 ' Rig
                                ParentFitting.BaseShip.RigSlot(oslotNo) = ncMod
                                ParentFitting.FittedShip.RigSlot(oslotNo) = nfMod
                            Case 2 ' Low
                                ParentFitting.BaseShip.LowSlot(oslotNo) = ncMod
                                ParentFitting.FittedShip.LowSlot(oslotNo) = nfMod
                            Case 4 ' Mid
                                ParentFitting.BaseShip.MidSlot(oslotNo) = ncMod
                                ParentFitting.FittedShip.MidSlot(oslotNo) = nfMod
                            Case 8 ' High
                                ParentFitting.BaseShip.HiSlot(oslotNo) = ncMod
                                ParentFitting.FittedShip.HiSlot(oslotNo) = nfMod
                            Case 16 ' Subsystem
                                ParentFitting.BaseShip.SubSlot(oslotNo) = ncMod
                                ParentFitting.FittedShip.SubSlot(oslotNo) = nfMod
                        End Select
                    End If
                    Call Me.UpdateSlotLocation(ofMod, nslotNo)
                    Call Me.UpdateSlotLocation(nfMod, oslotNo)
                    ' Update Undo History
                    Dim oModName As String = ""
                    Dim oLoadedChargeName As String = ""
                    If ocMod IsNot Nothing Then
                        oModName = ocMod.Name
                        If ocMod.LoadedCharge IsNot Nothing Then
                            oLoadedChargeName = ocMod.LoadedCharge.Name
                        End If
                    Else
                        oModName = ""
                    End If
                    Dim nModName As String = ""
                    Dim nLoadedChargeName As String = ""
                    If ncMod IsNot Nothing Then
                        nModName = ncMod.Name
                        If ncMod.LoadedCharge IsNot Nothing Then
                            nLoadedChargeName = ncMod.LoadedCharge.Name
                        End If
                    Else
                        nModName = ""
                    End If
                    ' Only update if we actually have something to undo!
                    If oModName <> nModName Or oLoadedChargeName <> nLoadedChargeName Then
                        UndoStack.Push(New UndoInfo(UndoInfo.TransType.SwapModules, oSlotType, oslotNo, oModName, oLoadedChargeName, nslotNo, nModName, nLoadedChargeName))
                        Me.UpdateHistory()
                    End If
                Else
                    'MessageBox.Show("Wanting to copy " & oLVI.Text & " for " & nLVI.Text & "?", "Confirm copy", MessageBoxButtons.OK, MessageBoxIcon.Question)
                    Dim rMod As ShipModule = Nothing
                    If ncMod IsNot Nothing Then
                        rMod = ncMod.Clone()
                    End If
                    If ocMod IsNot Nothing Then
                        ncMod = ocMod.Clone
                        ParentFitting.AddModule(ncMod, nslotNo, False, False, rMod, False, False)
                    Else
                        RemoveModule(nLVI, False, False)
                    End If
                    ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                End If
            End If
        End If
        ' Update the ship details
        Call UpdateShipDetails()
        adtSlots.DragDropEnabled = True
    End Sub

    Private Sub adtSlots_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtSlots.DragLeave
        ' Activate auto drag and drop once the drag and drop operation ends
        adtSlots.DragDropEnabled = True
    End Sub

#End Region

#Region "Remote Effects"

    Private Sub btnUpdateRemoteEffects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateRemoteEffects.Click
        Call Me.UpdateRemoteEffects()
    End Sub
    Private Sub UpdateRemoteEffects()
        ' Check if we have any remote fittings and if so, generate the fitting
        If lvwRemoteFittings.Items.Count > 0 Then
            lvwRemoteEffects.Tag = "Refresh"
            lvwRemoteEffects.BeginUpdate()
            lvwRemoteEffects.Items.Clear()
            For Each remoteFitting As ListViewItem In lvwRemoteFittings.CheckedItems
                ' Let's try and generate a fitting and get some module info
                Dim shipFit As String = remoteFitting.Tag.ToString
                Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(remoteFitting.Name.Substring(shipFit.Length + 2)), HQFPilot)
                Dim NewFit As Fitting = Fittings.FittingList(shipFit).Clone
                NewFit.UpdateBaseShipFromFitting()
                NewFit.PilotName = pPilot.PilotName
                NewFit.ApplyFitting(BuildType.BuildEverything)
                Dim remoteShip As Ship = NewFit.FittedShip
                For Each remoteModule As ShipModule In remoteShip.SlotCollection
                    If remoteGroups.Contains(CInt(remoteModule.DatabaseGroup)) = True Then
                        remoteModule.ModuleState = ModuleStates.Gang
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
                            remoteDrone.DroneType.ModuleState = ModuleStates.Gang
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
        Else
            lvwRemoteEffects.BeginUpdate()
            lvwRemoteEffects.Items.Clear()
            lvwRemoteEffects.EndUpdate()
        End If
    End Sub

    Private Sub ResetRemoteEffects()
        lvwRemoteEffects.BeginUpdate()
        lvwRemoteEffects.Tag = "Refresh"
        For Each li As ListViewItem In lvwRemoteEffects.Items
            li.Checked = False
        Next
        lvwRemoteEffects.Tag = ""
        lvwRemoteEffects.EndUpdate()
    End Sub

    Private Sub lvwRemoteEffects_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwRemoteEffects.ItemChecked
        ' Check for an active siege module in the current ship
        If e.Item.Checked = True Then
            For Each cMod As ShipModule In ParentFitting.FittedShip.SlotCollection
                If cMod.ID = "20280" And cMod.ModuleState = ModuleStates.Active Then
                    MessageBox.Show("You cannot apply remote effects while the " & ParentFitting.BaseShip.Name & " is in Siege Mode!", "Remote Effect Not Permitted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    e.Item.Checked = False
                    Exit Sub
                ElseIf cMod.ID = "27951" And cMod.ModuleState = ModuleStates.Active Then
                    MessageBox.Show("You cannot apply remote effects while the " & ParentFitting.BaseShip.Name & " is in Triage Mode!", "Remote Effect Not Permitted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    e.Item.Checked = False
                    Exit Sub
                End If
            Next
        End If
        If lvwRemoteEffects.Tag.ToString <> "Refresh" Then
            ParentFitting.BaseShip.RemoteSlotCollection.Clear()
            For Each item As ListViewItem In lvwRemoteEffects.CheckedItems
                ParentFitting.BaseShip.RemoteSlotCollection.Add(item.Tag)
            Next
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End If
    End Sub

    Private Sub mnuShowRemoteModInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowRemoteModInfo.Click
        Dim sModule As Object = lvwRemoteEffects.SelectedItems(0).Tag
        If TypeOf sModule Is DroneBayItem Then
            sModule = CType(sModule, DroneBayItem).DroneType
        End If
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(lvwRemoteEffects.SelectedItems(0).Name), Core.Pilot)
        showInfo.ShowItemDetails(sModule, hPilot)
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
                Call Me.UpdateRemoteEffects()
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
        Call Me.UpdateRemoteEffects()
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
        cboSCShip.Items.Add("<None>")
        cboWCShip.Items.Add("<None>")
        cboFCShip.Items.Add("<None>")
        For Each fitting As String In Fittings.FittingList.Keys
            cboFitting.Items.Add(fitting)
            cboSCShip.Items.Add(fitting)
            cboWCShip.Items.Add(fitting)
            cboFCShip.Items.Add(fitting)
        Next
        ' Add the pilots
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            cboPilot.Items.Add(cPilot.Name)
            cboSCPilot.Items.Add(cPilot.Name)
            cboWCPilot.Items.Add(cPilot.Name)
            cboFCPilot.Items.Add(cPilot.Name)
        Next
        cboPilot.EndUpdate() : cboFitting.EndUpdate()
        cboSCPilot.EndUpdate() : cboSCShip.EndUpdate()
        cboWCPilot.EndUpdate() : cboWCShip.EndUpdate()
        cboFCPilot.EndUpdate() : cboFCShip.EndUpdate()
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
            If chkSCActive.Checked = True Then
                Commanders.Add(cboSCPilot.SelectedItem.ToString)
            End If
        End If
        If cboWCPilot.SelectedItem IsNot Nothing Then
            If chkWCActive.Checked = True Then
                Commanders.Add(cboWCPilot.SelectedItem.ToString)
            End If
        End If
        If cboFCPilot.SelectedItem IsNot Nothing Then
            If chkFCActive.Checked = True Then
                Commanders.Add(cboFCPilot.SelectedItem.ToString)
            End If
        End If

        If Commanders.Count > 0 Then

            ' Go through each commander and parse the skills
            Dim FleetSkill(Commanders.Count + 1, fleetSkills.Count - 1) As String
            Dim hPilot As New HQFPilot
            For Commander As Integer = 0 To Commanders.Count - 1
                hPilot = CType(HQFPilotCollection.HQFPilots(Commanders(Commander)), HQFPilot)
                For Skill As Integer = 0 To fleetSkills.Count - 1
                    If hPilot.ImplantName(10) = fleetSkills(Skill).ToString & " Mindlink" Then
                        FleetSkill(Commander + 1, Skill) = "6"
                        FleetSkill(0, Skill) = FleetSkill(Commander + 1, Skill)
                        FleetSkill(Commanders.Count + 1, Skill) = hPilot.PilotName
                    Else
                        If hPilot.SkillSet.Contains(fleetSkills(Skill).ToString) Then
                            FleetSkill(Commander + 1, Skill) = CType(hPilot.SkillSet(fleetSkills(Skill).ToString), HQFSkill).Level.ToString
                            If FleetSkill(Commander + 1, Skill) >= FleetSkill(0, Skill) Then
                                FleetSkill(0, Skill) = FleetSkill(Commander + 1, Skill)
                                FleetSkill(Commanders.Count + 1, Skill) = hPilot.PilotName
                            End If
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
                    fleetModule.ModuleState = ModuleStates.Fleet
                    Select Case fleetSkills(skill).ToString
                        Case "Armored Warfare"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Armored Warfare Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("335", 15)
                            Else
                                fleetModule.Attributes.Add("335", 2 * CInt(FleetSkill(0, skill)))
                            End If
                        Case "Information Warfare"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Information Warfare Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("309", 15)
                            Else
                                fleetModule.Attributes.Add("309", 2 * CInt(FleetSkill(0, skill)))
                            End If
                        Case "Leadership"
                            fleetModule.Attributes.Add("566", 2 * CInt(FleetSkill(0, skill)))
                        Case "Mining Foreman"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Mining Foreman Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("434", 15)
                            Else
                                fleetModule.Attributes.Add("434", 2 * CInt(FleetSkill(0, skill)))
                            End If
                        Case "Siege Warfare"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Siege Warfare Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("337", 15)
                            Else
                                fleetModule.Attributes.Add("337", 2 * CInt(FleetSkill(0, skill)))
                            End If
                        Case "Skirmish Warfare"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Skirmish Warfare Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("151", -15)
                            Else
                                fleetModule.Attributes.Add("151", -2 * CInt(FleetSkill(0, skill)))
                            End If
                    End Select
                    ParentFitting.BaseShip.FleetSlotCollection.Add(fleetModule)
                End If
            Next
        Else
            ParentFitting.BaseShip.FleetSlotCollection.Clear()
        End If
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)

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
    Private Sub chkSCActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSCActive.CheckedChanged
        Call UpdateSCShipEffects()
    End Sub
    Private Sub chkWCActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWCActive.CheckedChanged
        Call UpdateWCShipEffects()
    End Sub
    Private Sub chkFCActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFCActive.CheckedChanged
        Call UpdateFCShipEffects()
    End Sub
    Private Sub UpdateSCShipEffects()
        If cboSCShip.SelectedIndex <> -1 Then
            If cboSCShip.SelectedItem.ToString = "<None>" Then
                cboSCShip.Tag = Nothing
            Else
                If chkSCActive.Checked = False Then
                    cboSCShip.Tag = Nothing
                Else
                    ' Let's try and generate a fitting and get some module info
                    Dim shipFit As String = cboSCShip.SelectedItem.ToString
                    If cboSCPilot.SelectedItem IsNot Nothing Then
                        Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboSCPilot.SelectedItem.ToString), HQFPilot)
                        Dim NewFit As Fitting = Fittings.FittingList(shipFit).Clone
                        NewFit.UpdateBaseShipFromFitting()
                        NewFit.PilotName = pPilot.PilotName
                        NewFit.ApplyFitting(BuildType.BuildEverything)
                        Dim remoteShip As Ship = NewFit.FittedShip
                        Dim SCModules As New ArrayList
                        ' Check the ship bonuses for further effects (Titans use this!)
                        SCModules = GetShipGangBonusModules(remoteShip, pPilot)
                        ' Check the modules for fleet effects
                        For Each FleetModule As ShipModule In remoteShip.SlotCollection
                            If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                                FleetModule.ModuleState = ModuleStates.Gang
                                FleetModule.SlotNo = 0
                                SCModules.Add(FleetModule)
                            End If
                        Next
                        cboSCShip.Tag = SCModules
                    End If
                End If
            End If
            Call Me.CalculateFleetEffects()
        End If
    End Sub

    Private Sub UpdateWCShipEffects()
        If cboWCShip.SelectedIndex <> -1 Then
            If cboWCShip.SelectedItem.ToString = "<None>" Then
                cboWCShip.Tag = Nothing
            Else
                If chkWCActive.Checked = False Then
                    cboWCShip.Tag = Nothing
                Else
                    ' Let's try and generate a fitting and get some module info
                    Dim shipFit As String = cboWCShip.SelectedItem.ToString
                    If cboWCPilot.SelectedItem IsNot Nothing Then
                        Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboWCPilot.SelectedItem.ToString), HQFPilot)
                        Dim NewFit As Fitting = Fittings.FittingList(shipFit).Clone
                        NewFit.UpdateBaseShipFromFitting()
                        NewFit.PilotName = pPilot.PilotName
                        NewFit.ApplyFitting(BuildType.BuildEverything)
                        Dim remoteShip As Ship = NewFit.FittedShip
                        Dim WCModules As New ArrayList
                        For Each FleetModule As ShipModule In remoteShip.SlotCollection
                            If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                                FleetModule.ModuleState = ModuleStates.Gang
                                FleetModule.SlotNo = 0
                                WCModules.Add(FleetModule)
                            End If
                        Next
                        cboWCShip.Tag = WCModules
                    End If
                End If
            End If
            Call Me.CalculateFleetEffects()
        End If
    End Sub

    Private Sub UpdateFCShipEffects()
        If cboFCShip.SelectedIndex <> -1 Then
            If cboFCShip.SelectedItem.ToString = "<None>" Then
                cboFCShip.Tag = Nothing
            Else
                If chkFCActive.Checked = False Then
                    cboFCShip.Tag = Nothing
                Else
                    ' Let's try and generate a fitting and get some module info
                    Dim shipFit As String = cboFCShip.SelectedItem.ToString
                    If cboFCPilot.SelectedItem IsNot Nothing Then
                        Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboFCPilot.SelectedItem.ToString), HQFPilot)
                        Dim NewFit As Fitting = Fittings.FittingList(shipFit).Clone
                        NewFit.UpdateBaseShipFromFitting()
                        NewFit.PilotName = pPilot.PilotName
                        NewFit.ApplyFitting(BuildType.BuildEverything)
                        Dim remoteShip As Ship = NewFit.FittedShip
                        Dim FCModules As New ArrayList
                        For Each FleetModule As ShipModule In remoteShip.SlotCollection
                            If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                                FleetModule.ModuleState = ModuleStates.Gang
                                FleetModule.SlotNo = 0
                                FCModules.Add(FleetModule)
                            End If
                        Next
                        cboFCShip.Tag = FCModules
                    End If
                End If
            End If
            Call Me.CalculateFleetEffects()
        End If
    End Sub

    Private Sub CalculateFleetEffects()
        ParentFitting.BaseShip.FleetSlotCollection.Clear()
        Call Me.CalculateFleetSkillEffects()
        Call Me.CalculateFleetModuleEffects()
        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
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
            ParentFitting.BaseShip.FleetSlotCollection.Add(FleetModule)
        Next

    End Sub

    Private Function GetShipGangBonusModules(ByVal hShip As Ship, ByVal hPilot As HQFPilot) As ArrayList
        Dim FleetModules As New ArrayList
        If hShip IsNot Nothing Then
            Dim shipRoles As New List(Of ShipEffect)
            Dim hSkill As New HQFSkill
            'Dim fEffect As New FinalEffect
            'Dim fEffectList As New ArrayList
            shipRoles = Engine.ShipBonusesMap(hShip.ID)
            If shipRoles IsNot Nothing Then
                For Each chkEffect As ShipEffect In shipRoles
                    If chkEffect.Status = 16 Then
                        ' We have a gang bonus effect so create a dummy module for handling this

                        Dim gangModule As New ShipModule
                        gangModule.Name = hShip.Name & " Gang Bonus"
                        gangModule.ID = "-1"
                        gangModule.SlotNo = 0
                        gangModule.ModuleState = ModuleStates.Gang
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

#Region "Ship Skill Context Menu"
    Private Sub ctxShipSkills_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxShipSkills.Opening
        ' Check for Relevant Skills in Modules/Charges
        Dim RelGlobalSkills As New ArrayList
        Dim ShipSkills As New ArrayList
        Dim Affects(10) As String
        ctxShipSkills.Items.Clear()
        For Each Affect As String In ParentFitting.BaseShip.GlobalAffects
            If Affect.Contains(";Skill;") = True Then
                Affects = Affect.Split((";").ToCharArray)
                If RelGlobalSkills.Contains(Affects(0)) = False Then
                    RelGlobalSkills.Add(Affects(0))
                End If
            End If
            If Affect.Contains(";Ship Bonus;") = True Then
                Affects = Affect.Split((";").ToCharArray)
                If Me.ParentFitting.ShipName = Affects(0) Then
                    If RelGlobalSkills.Contains(Affects(3)) = False Then
                        RelGlobalSkills.Add(Affects(3))
                    End If
                End If
            End If
            If Affect.Contains(";Subsystem;") = True Then
                Affects = Affect.Split((";").ToCharArray)
                If RelGlobalSkills.Contains(Affects(3)) = False Then
                    RelGlobalSkills.Add(Affects(3))
                End If
            End If
        Next
        RelGlobalSkills.Sort()
        For Each Affect As String In ParentFitting.BaseShip.Affects
            If Affect.Contains(";Skill;") = True Then
                Affects = Affect.Split((";").ToCharArray)
                If ShipSkills.Contains(Affects(0)) = False Then
                    ShipSkills.Add(Affects(0))
                End If
            End If
            If Affect.Contains(";Ship Bonus;") = True Then
                Affects = Affect.Split((";").ToCharArray)
                If Me.ParentFitting.ShipName = Affects(0) Then
                    If ShipSkills.Contains(Affects(3)) = False Then
                        ShipSkills.Add(Affects(3))
                    End If
                End If
            End If
            If Affect.Contains(";Subsystem;") = True Then
                Affects = Affect.Split((";").ToCharArray)
                If ShipSkills.Contains(Affects(3)) = False Then
                    ShipSkills.Add(Affects(3))
                End If
            End If
        Next
        ShipSkills.Sort()

        ' Add the Main menu item
        Dim AlterRelevantSkills As New ToolStripMenuItem
        AlterRelevantSkills.Name = ParentFitting.BaseShip.Name
        AlterRelevantSkills.Text = "Alter Relevant Skills"

        ' Add the bonus skills
        If RelGlobalSkills.Count > 0 Then
            For Each relSkill As String In RelGlobalSkills
                Dim newRelSkill As New ToolStripMenuItem
                newRelSkill.Name = relSkill
                newRelSkill.Text = relSkill
                Dim pilotLevel As Integer = 0
                If CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet.Contains(relSkill) Then
                    pilotLevel = CType(CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet(relSkill), HQFSkill).Level
                Else
                    MessageBox.Show("There is a mis-match of roles for the " & ParentFitting.BaseShip.Name & ". Please report this to the EveHQ Developers.", "Ship Role Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
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
                Dim defaultLevel As Integer = 0
                If CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills.Contains(relSkill) = True Then
                    defaultLevel = CType(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(relSkill), EveHQ.Core.PilotSkill).Level
                End If
                Dim newRelSkillDefault As New ToolStripMenuItem
                newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                AddHandler newRelSkillDefault.Click, AddressOf Me.SetPilotSkillLevel
                newRelSkill.DropDownItems.Add(newRelSkillDefault)
                AlterRelevantSkills.DropDownItems.Add(newRelSkill)
            Next
        End If

        ' Add a divider if relevant
        If RelGlobalSkills.Count > 0 And ShipSkills.Count > 0 Then
            AlterRelevantSkills.DropDownItems.Add("-")
        End If

        ' Add the ship skills
        If ShipSkills.Count > 0 Then
            For Each shipskill As String In ShipSkills
                Dim newShipSkill As New ToolStripMenuItem
                newShipSkill.Name = shipskill
                newShipSkill.Text = shipskill
                Dim pilotLevel As Integer = 0
                If CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet.Contains(shipskill) Then
                    pilotLevel = CType(CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet(shipskill), HQFSkill).Level
                Else
                    MessageBox.Show("There is a mis-match of roles for the " & ParentFitting.BaseShip.Name & ". Please report this to the EveHQ Developers.", "Ship Role Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                newShipSkill.Image = CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString), Image)
                For skillLevel As Integer = 0 To 5
                    Dim newRelSkillLevel As New ToolStripMenuItem
                    newRelSkillLevel.Name = shipskill & skillLevel.ToString
                    newRelSkillLevel.Text = "Level " & skillLevel.ToString
                    If skillLevel = pilotLevel Then
                        newRelSkillLevel.Checked = True
                    End If
                    AddHandler newRelSkillLevel.Click, AddressOf Me.SetPilotSkillLevel
                    newShipSkill.DropDownItems.Add(newRelSkillLevel)
                Next
                newShipSkill.DropDownItems.Add("-")
                Dim defaultLevel As Integer = 0
                If CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills.Contains(shipskill) = True Then
                    defaultLevel = CType(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(shipskill), EveHQ.Core.PilotSkill).Level
                End If
                Dim newRelSkillDefault As New ToolStripMenuItem
                newRelSkillDefault.Name = shipskill & defaultLevel.ToString
                newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                AddHandler newRelSkillDefault.Click, AddressOf Me.SetPilotSkillLevel
                newShipSkill.DropDownItems.Add(newRelSkillDefault)
                AlterRelevantSkills.DropDownItems.Add(newShipSkill)
            Next
        End If

        ctxShipSkills.Items.Add(AlterRelevantSkills)

    End Sub
#End Region

#Region "Wormhole Effects"

    Private Sub LoadWHInfo()
        cboWHEffect.Items.Clear()
        cboWHEffect.Items.Add("<None>")
        cboWHEffect.Items.Add("Black Hole")
        cboWHEffect.Items.Add("Cataclysmic Variable")
        cboWHEffect.Items.Add("Magnetar")
        cboWHEffect.Items.Add("Pulsar")
        cboWHEffect.Items.Add("Red Giant")
        cboWHEffect.Items.Add("Wolf Rayet")
        cboWHClass.Items.Clear()
        cboWHClass.Items.Add("1")
        cboWHClass.Items.Add("2")
        cboWHClass.Items.Add("3")
        cboWHClass.Items.Add("4")
        cboWHClass.Items.Add("5")
        cboWHClass.Items.Add("6")
    End Sub

    Private Sub UpdateWHUI()
        cboWHEffect.SelectedItem = ParentFitting.WHEffect
        cboWHClass.SelectedItem = ParentFitting.WHLevel.ToString
    End Sub

    Private Sub cboWHEffect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHEffect.SelectedIndexChanged
        If UpdateAll = False Then
            ' Clear the current effect
            ParentFitting.BaseShip.EnviroSlotCollection.Clear()
            ' Set the WH Class combo if it's not activated
            If cboWHEffect.SelectedIndex > 0 Then
                ParentFitting.WHEffect = cboWHEffect.SelectedItem.ToString
                If cboWHClass.SelectedIndex = -1 Then
                    cboWHClass.SelectedIndex = 0
                    Exit Sub
                Else
                    Dim modName As String = ""
                    If cboWHEffect.SelectedItem.ToString = "Red Giant" Then
                        modName = cboWHEffect.SelectedItem.ToString & " Beacon Class " & cboWHClass.SelectedItem.ToString
                    Else
                        modName = cboWHEffect.SelectedItem.ToString & " Effect Beacon Class " & cboWHClass.SelectedItem.ToString
                    End If
                    Dim modID As String = CStr(ModuleLists.moduleListName(modName))
                    Dim eMod As ShipModule = CType(ModuleLists.moduleList(modID), ShipModule).Clone
                    ParentFitting.BaseShip.EnviroSlotCollection.Add(eMod)
                End If
            Else
                ParentFitting.WHEffect = ""
            End If
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End If
    End Sub

    Private Sub cboWHClass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHClass.SelectedIndexChanged
        If UpdateAll = False Then
            If cboWHEffect.SelectedIndex > 0 Then
                ' Clear the current effect
                ParentFitting.BaseShip.EnviroSlotCollection.Clear()
                Dim modName As String = ""
                If cboWHEffect.SelectedItem.ToString = "Red Giant" Then
                    modName = cboWHEffect.SelectedItem.ToString & " Beacon Class " & cboWHClass.SelectedItem.ToString
                Else
                    modName = cboWHEffect.SelectedItem.ToString & " Effect Beacon Class " & cboWHClass.SelectedItem.ToString
                End If
                Dim modID As String = CStr(ModuleLists.moduleListName(modName))
                Dim eMod As ShipModule = CType(ModuleLists.moduleList(modID), ShipModule).Clone
                ParentFitting.BaseShip.EnviroSlotCollection.Add(eMod)
                ParentFitting.WHLevel = CInt(cboWHClass.SelectedItem.ToString)
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            Else
                ParentFitting.WHLevel = 0
            End If
        End If
    End Sub

#End Region

#Region "Booster Effects"

    Private Sub LoadBoosterInfo()
        cboBoosterSlot1.BeginUpdate() : cboBoosterSlot1.Items.Clear()
        cboBoosterSlot2.BeginUpdate() : cboBoosterSlot2.Items.Clear()
        cboBoosterSlot3.BeginUpdate() : cboBoosterSlot3.Items.Clear()
        For Each Booster As ShipModule In Boosters.BoosterList.Values
            Select Case CInt(Booster.Attributes("1087"))
                Case 1
                    cboBoosterSlot1.Items.Add(Booster.Name)
                Case 2
                    cboBoosterSlot2.Items.Add(Booster.Name)
                Case 3
                    cboBoosterSlot3.Items.Add(Booster.Name)
            End Select
        Next
        cboBoosterSlot1.EndUpdate()
        cboBoosterSlot2.EndUpdate()
        cboBoosterSlot3.EndUpdate()
    End Sub

    Private Sub UpdateBoosterSlots()
        UpdateBoosters = True
        For Each Booster As ShipModule In ParentFitting.BaseShip.BoosterSlotCollection
            Dim slot As Integer = CInt(Booster.Attributes("1087"))
            Dim cb As ComboBox = CType(Me.tcStorage.Controls("tcpBoosters").Controls("cboBoosterSlot" & slot.ToString), ComboBox)
            If cb.Items.Contains(Booster.Name) = True Then
                cb.SelectedItem = Booster.Name
            End If
        Next
        UpdateBoosters = False
    End Sub

    Private Sub cboBoosterSlots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBoosterSlot1.SelectedIndexChanged, cboBoosterSlot2.SelectedIndexChanged, cboBoosterSlot3.SelectedIndexChanged
        Dim cb As ComboBox = CType(sender, ComboBox)
        Dim idx As Integer = CInt(cb.Name.Substring(cb.Name.Length - 1, 1))
        Dim bt As ButtonX = New ButtonX
        Select Case idx
            Case 1
                bt = btnBoosterSlot1
            Case 2
                bt = btnBoosterSlot2
            Case 3
                bt = btnBoosterSlot3
        End Select
        ' Try to get the penalties
        If cb.SelectedItem IsNot Nothing Then
            Dim boosterName As String = cb.SelectedItem.ToString
            Dim boosterID As String = CStr(ModuleLists.moduleListName(boosterName))
            Dim bModule As ShipModule = CType(ModuleLists.moduleList(boosterID), ShipModule).Clone
            cb.Tag = bModule
            ToolTip1.SetToolTip(cb, SquishText(bModule.Description))
            Dim effects As SortedList(Of String, BoosterEffect) = CType(Boosters.BoosterEffects(boosterID), SortedList(Of String, BoosterEffect))
            Dim effectList As String = "Penalties: "
            Dim count As Integer = 0
            If effects IsNot Nothing Then
                For Each bEffect As BoosterEffect In effects.Values
                    effectList &= bEffect.AttributeEffect & ", "
                Next
            End If
            tcpBoosters.Refresh()
            bModule.ImplantSlot = 15
            Call Me.ApplyBoosters(cb, bt, idx)
            bt.Enabled = True
        Else
            bt.Enabled = False
        End If
    End Sub

    Private Sub ApplyBoosters(ByVal cb As ComboBox, ByVal ParentButton As ButtonX, ByVal idx As Integer)
        If UpdateBoosters = False Then
            ParentFitting.BaseShip.BoosterSlotCollection.Clear()
            For slot As Integer = 1 To 3
                Dim cbo As ComboBox = CType(Me.tcStorage.Controls("tcpBoosters").Controls("cboBoosterSlot" & slot.ToString), ComboBox)
                If cbo.Tag IsNot Nothing Then
                    ParentFitting.BaseShip.BoosterSlotCollection.Add(cbo.Tag)
                End If
            Next
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End If
        BuildBoosterSkills(cb, ParentButton, idx)
    End Sub

#End Region

#Region "Public Update Routines"

    Public Sub UpdateDroneBay()
        UpdateDrones = True
        Call Me.RedrawDroneBay()
        UpdateDrones = False
        Call UpdatePrices()
        Call ParentFitting.UpdateFittingFromBaseShip()
    End Sub

    Public Sub UpdateItemBay()
        Call Me.RedrawCargoBay()
        Call UpdatePrices()
        Call ParentFitting.UpdateFittingFromBaseShip()
    End Sub

    Public Sub UpdateShipBay()
        Call Me.RedrawShipBay()
        Call ParentFitting.UpdateFittingFromBaseShip()
    End Sub

#End Region

#Region "Undo Stuff"

    Public Sub UpdateHistory()

        ' Update the history tab
        lvwHistory.BeginUpdate()
        lvwHistory.Items.Clear()
        For Each UI As UndoInfo In Me.UndoStack
            Dim NewUI As New ListViewItem
            NewUI.Text = UI.Transaction.ToString
            NewUI.SubItems.Add(UI.SlotType.ToString)
            NewUI.SubItems.Add(UI.OldSlotNo.ToString)
            NewUI.SubItems.Add(UI.OldModName.ToString)
            NewUI.SubItems.Add(UI.OldChargeName.ToString)
            NewUI.SubItems.Add(UI.NewSlotNo.ToString)
            NewUI.SubItems.Add(UI.NewModName.ToString)
            NewUI.SubItems.Add(UI.NewChargeName.ToString)
            NewUI.SubItems.Add(UI.ChargeOnly.ToString)
            lvwHistory.Items.Add(NewUI)
        Next
        lvwHistory.EndUpdate()

        ' Update the Undo button subitems
        For Each bi As ButtonItem In btnUndo.SubItems
            RemoveHandler bi.Click, AddressOf Me.UndoSubItems
        Next
        btnUndo.SubItems.Clear()
        Dim idx As Integer = 0
        For Each UI As UndoInfo In Me.UndoStack
            idx += 1
            Dim bi As New ButtonItem
            Select Case UI.Transaction
                Case UndoInfo.TransType.AddCharge
                    bi.Text = UI.Transaction.ToString & " - " & UI.NewChargeName.ToString
                Case UndoInfo.TransType.AddModule
                    bi.Text = UI.Transaction.ToString & " - " & UI.NewModName.ToString
                Case UndoInfo.TransType.RemoveCharge
                    bi.Text = UI.Transaction.ToString & " - " & UI.OldChargeName.ToString
                Case UndoInfo.TransType.RemoveModule
                    bi.Text = UI.Transaction.ToString & " - " & UI.OldModName.ToString
                Case UndoInfo.TransType.ReplacedModule
                    bi.Text = UI.Transaction.ToString & " - " & UI.OldModName.ToString & " -> " & UI.NewModName.ToString
                Case UndoInfo.TransType.SwapModules
                    bi.Text = UI.Transaction.ToString & " - " & UI.OldModName.ToString & " -> " & UI.NewModName.ToString
            End Select
            bi.ImageFixedSize = New Size(20, 20)
            bi.Image = My.Resources.imgShield
            bi.Name = idx.ToString
            AddHandler bi.Click, AddressOf Me.UndoSubItems
            btnUndo.SubItems.Add(bi)
            ' Exit if we have exceeded 10 items
            If idx = 10 Then Exit For
        Next
        btnUndo.RecalcLayout()

        ' Update the Redo button subitems
        For Each bi As ButtonItem In btnRedo.SubItems
            RemoveHandler bi.Click, AddressOf Me.RedoSubItems
        Next
        btnRedo.SubItems.Clear()
        idx = 0
        For Each UI As UndoInfo In Me.RedoStack
            idx += 1
            Dim bi As New ButtonItem
            Select Case UI.Transaction
                Case UndoInfo.TransType.AddCharge
                    bi.Text = UI.Transaction.ToString & " - " & UI.NewChargeName.ToString
                Case UndoInfo.TransType.AddModule
                    bi.Text = UI.Transaction.ToString & " - " & UI.NewModName.ToString
                Case UndoInfo.TransType.RemoveCharge
                    bi.Text = UI.Transaction.ToString & " - " & UI.OldChargeName.ToString
                Case UndoInfo.TransType.RemoveModule
                    bi.Text = UI.Transaction.ToString & " - " & UI.OldModName.ToString
                Case UndoInfo.TransType.ReplacedModule
                    bi.Text = UI.Transaction.ToString & " - " & UI.OldModName.ToString & " -> " & UI.NewModName.ToString
                Case UndoInfo.TransType.SwapModules
                    bi.Text = UI.Transaction.ToString & " - " & UI.OldModName.ToString & " -> " & UI.NewModName.ToString
            End Select
            bi.ImageFixedSize = New Size(20, 20)
            bi.Image = My.Resources.imgShield
            bi.Name = idx.ToString
            AddHandler bi.Click, AddressOf Me.RedoSubItems
            btnRedo.SubItems.Add(bi)
            ' Exit if we have exceeded 10 items
            If idx = 10 Then Exit For
        Next
        btnRedo.RecalcLayout()

    End Sub

    Public Sub PerformUndo(ByVal Levels As Integer)
        ' Take the first x levels off the stack and reapply them
        If UndoStack.Count > 0 Then
            For level As Integer = 1 To Math.Min(Levels, UndoStack.Count)
                Dim UI As UndoInfo = UndoStack.Pop
                Dim SlotNode As Node = adtSlots.FindNodeByName(UI.SlotType & "_" & UI.NewSlotNo)
                ' Reverse the transaction based on the type
                Select Case UI.Transaction
                    Case UndoInfo.TransType.AddCharge
                        ' Need to check if the old charge was blank prior to adding
                        If UI.OldChargeName = "" Then
                            Call Me.RemoveSingleChargeFromModule(SlotNode, True)
                        Else
                            Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.OldChargeName))
                            Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                            Call Me.LoadSingleChargeIntoModule(SlotNode, Charge, True)
                        End If
                    Case UndoInfo.TransType.RemoveCharge
                        ' Need to add the charge
                        Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.OldChargeName))
                        Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                        Call Me.LoadSingleChargeIntoModule(SlotNode, Charge, True)
                    Case UndoInfo.TransType.AddModule
                        ' Need to check if the slot was blank prior to adding
                        If UI.OldModName = "" Then
                            Call Me.RemoveModule(SlotNode, False, True)
                        Else
                            ' Need to add the module back
                            Dim OldModID As String = CStr(ModuleLists.moduleListName(UI.OldModName))
                            Dim OldMod As ShipModule = CType(ModuleLists.moduleList.Item(OldModID), ShipModule).Clone
                            If UI.OldChargeName <> "" Then
                                Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.OldChargeName))
                                Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                                OldMod.LoadedCharge = Charge
                            End If
                            Call ParentFitting.AddModule(OldMod, UI.OldSlotNo, False, False, Nothing, True, False)
                        End If
                    Case UndoInfo.TransType.RemoveModule
                        ' Need to add the module back
                        Dim OldModID As String = CStr(ModuleLists.moduleListName(UI.OldModName))
                        Dim OldMod As ShipModule = CType(ModuleLists.moduleList.Item(OldModID), ShipModule).Clone
                        If UI.OldChargeName <> "" Then
                            Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.OldChargeName))
                            Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                            OldMod.LoadedCharge = Charge
                        End If
                        Call ParentFitting.AddModule(OldMod, UI.OldSlotNo, False, False, Nothing, True, False)
                    Case UndoInfo.TransType.SwapModules, UndoInfo.TransType.ReplacedModule
                        ' Swap modules back to their original positions
                        Dim OldMod As ShipModule = Nothing
                        If UI.OldModName <> "" Then
                            Dim OldModID As String = CStr(ModuleLists.moduleListName(UI.OldModName))
                            OldMod = CType(ModuleLists.moduleList.Item(OldModID), ShipModule).Clone
                            If UI.OldChargeName <> "" Then
                                Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.OldChargeName))
                                Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                                OldMod.LoadedCharge = Charge
                            End If
                        End If
                        Dim newMod As ShipModule = Nothing
                        If UI.NewModName <> "" Then ' = not empty
                            Dim NewModID As String = CStr(ModuleLists.moduleListName(UI.NewModName))
                            newMod = CType(ModuleLists.moduleList.Item(NewModID), ShipModule).Clone
                            If UI.NewChargeName <> "" Then
                                Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.NewChargeName))
                                Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                                newMod.LoadedCharge = Charge
                            End If
                        End If
                        If OldMod IsNot Nothing Then
                            Call ParentFitting.AddModule(OldMod, UI.OldSlotNo, False, False, Nothing, True, True)
                        Else
                            Call RemoveModule(adtSlots.FindNodeByName(UI.SlotType & "_" & UI.OldSlotNo), False, True)
                        End If
                        If newMod IsNot Nothing Then
                            Call ParentFitting.AddModule(newMod, UI.NewSlotNo, False, False, Nothing, True, True)
                        Else
                            Call RemoveModule(adtSlots.FindNodeByName(UI.SlotType & "_" & UI.NewSlotNo), False, True)
                        End If
                        ' Switch the UndoInfo for Replaced Modules
                        If UI.Transaction = UndoInfo.TransType.ReplacedModule Then
                            Dim TempMod As String = UI.OldModName
                            Dim TempChg As String = UI.OldChargeName
                            UI.OldModName = UI.NewModName
                            UI.OldChargeName = UI.NewChargeName
                            UI.NewModName = TempMod
                            UI.NewChargeName = TempChg
                        End If
                End Select
                ' Put this onto the Redo stack
                RedoStack.Push(UI)
            Next
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            Call Me.UpdateHistory()
        End If
    End Sub

    Public Sub PerformRedo(ByVal Levels As Integer)
        ' Take the first x levels off the stack and reapply them
        If RedoStack.Count > 0 Then
            For level As Integer = 1 To Math.Min(Levels, RedoStack.Count)
                Dim UI As UndoInfo = RedoStack.Pop
                Dim SlotNode As Node = adtSlots.FindNodeByName(UI.SlotType & "_" & UI.NewSlotNo)
                ' Reverse the transaction based on the type
                Select Case UI.Transaction
                    Case UndoInfo.TransType.AddCharge
                        ' Reperform the charge loading
                        Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.NewChargeName))
                        Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                        Call Me.LoadSingleChargeIntoModule(SlotNode, Charge, True)
                    Case UndoInfo.TransType.RemoveCharge
                        ' Reperform the charge removal
                        Call Me.RemoveSingleChargeFromModule(SlotNode, True)
                    Case UndoInfo.TransType.AddModule
                        ' Need to add the module again
                        Dim NewModID As String = CStr(ModuleLists.moduleListName(UI.NewModName))
                        Dim NewMod As ShipModule = CType(ModuleLists.moduleList.Item(NewModID), ShipModule).Clone
                        If UI.NewChargeName <> "" Then
                            Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.NewChargeName))
                            Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                            NewMod.LoadedCharge = Charge
                        End If
                        Call ParentFitting.AddModule(NewMod, UI.NewSlotNo, False, False, Nothing, True, False)
                    Case UndoInfo.TransType.RemoveModule
                        ' Need to remove the module again
                        Call Me.RemoveModule(SlotNode, False, True)
                    Case UndoInfo.TransType.SwapModules, UndoInfo.TransType.ReplacedModule
                        ' Swap modules back to their original positions
                        Dim OldMod As ShipModule = Nothing
                        If UI.OldModName <> "" Then
                            Dim OldModID As String = CStr(ModuleLists.moduleListName(UI.OldModName))
                            OldMod = CType(ModuleLists.moduleList.Item(OldModID), ShipModule).Clone
                            If UI.OldChargeName <> "" Then
                                Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.OldChargeName))
                                Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                                OldMod.LoadedCharge = Charge
                            End If
                        End If
                        Dim newMod As ShipModule = Nothing
                        If UI.NewModName <> "" Then ' = not empty
                            Dim NewModID As String = CStr(ModuleLists.moduleListName(UI.NewModName))
                            newMod = CType(ModuleLists.moduleList.Item(NewModID), ShipModule).Clone
                            If UI.NewChargeName <> "" Then
                                Dim ChargeID As String = CStr(ModuleLists.moduleListName(UI.NewChargeName))
                                Dim Charge As ShipModule = CType(ModuleLists.moduleList.Item(ChargeID), ShipModule).Clone
                                newMod.LoadedCharge = Charge
                            End If
                        End If
                        If OldMod IsNot Nothing Then
                            Call ParentFitting.AddModule(OldMod, UI.NewSlotNo, False, False, Nothing, True, True)
                        Else
                            Call RemoveModule(adtSlots.FindNodeByName(UI.SlotType & "_" & UI.NewSlotNo), False, True)
                        End If
                        If newMod IsNot Nothing Then
                            Call ParentFitting.AddModule(newMod, UI.OldSlotNo, False, False, Nothing, True, True)
                        Else
                            Call RemoveModule(adtSlots.FindNodeByName(UI.SlotType & "_" & UI.OldSlotNo), False, True)
                        End If
                        ' Switch the UndoInfo for Replaced Modules
                        If UI.Transaction = UndoInfo.TransType.ReplacedModule Then
                            Dim TempMod As String = UI.OldModName
                            Dim TempChg As String = UI.OldChargeName
                            UI.OldModName = UI.NewModName
                            UI.OldChargeName = UI.NewChargeName
                            UI.NewModName = TempMod
                            UI.NewChargeName = TempChg
                        End If
                End Select
                ' Put this back onto the undo stack
                UndoStack.Push(UI)
            Next
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            Call Me.UpdateHistory()
        End If
    End Sub

    Private Sub btnUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUndo.Click
        ' Undo the last action
        Call Me.PerformUndo(1)
    End Sub

    Private Sub btnRedo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRedo.Click
        ' Redo the last action
        Call Me.PerformRedo(1)
    End Sub

    Private Sub UndoSubItems(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim bi As ButtonItem = CType(sender, ButtonItem)
        Call Me.PerformUndo(CInt(bi.Name))
    End Sub

    Private Sub RedoSubItems(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim bi As ButtonItem = CType(sender, ButtonItem)
        Call Me.PerformRedo(CInt(bi.Name))
    End Sub

#End Region

#Region "Booster UI Regions"

    Private Sub btnShowInfo1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowInfo1.Click
        Call ShowBoosterInfo(cboBoosterSlot1)
    End Sub

    Private Sub btnRemoveBooster1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveBooster1.Click
        Call RemoveBooster(cboBoosterSlot1, btnBoosterSlot1, 1)
    End Sub

    Private Sub btnShowInfo2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowInfo2.Click
        Call ShowBoosterInfo(cboBoosterSlot2)
    End Sub

    Private Sub btnRemoveBooster2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveBooster2.Click
        Call RemoveBooster(cboBoosterSlot2, btnBoosterSlot2, 2)
    End Sub

    Private Sub btnShowInfo3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowInfo3.Click
        Call ShowBoosterInfo(cboBoosterSlot3)
    End Sub

    Private Sub btnRemoveBooster3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveBooster3.Click
        Call RemoveBooster(cboBoosterSlot3, btnBoosterSlot3, 3)
    End Sub

    Private Sub BuildBoosterSkills(ByVal cb As ComboBox, ByVal ParentButton As ButtonX, ByVal idx As Integer)

        If cb.Tag IsNot Nothing Then
            Dim bModule As ShipModule = CType(cb.Tag, ShipModule)
            Dim boosterName As String = cb.SelectedItem.ToString
            Dim boosterID As String = CStr(ModuleLists.moduleListName(boosterName))
            ' Check for related skills
            Dim RelModuleSkills As New ArrayList
            Dim Affects(3) As String
            For Each Affect As String In bModule.Affects
                If Affect.Contains(";Skill;") = True Then
                    Affects = Affect.Split((";").ToCharArray)
                    If RelModuleSkills.Contains(Affects(0)) = False Then
                        RelModuleSkills.Add(Affects(0))
                    End If
                End If
                If Affect.Contains(";Ship Bonus;") = True Then
                    Affects = Affect.Split((";").ToCharArray)
                    If Me.ParentFitting.ShipName = Affects(0) Then
                        If RelModuleSkills.Contains(Affects(3)) = False Then
                            RelModuleSkills.Add(Affects(3))
                        End If
                    End If
                End If
                If Affect.Contains(";Subsystem;") = True Then
                    Affects = Affect.Split((";").ToCharArray)
                    If RelModuleSkills.Contains(Affects(3)) = False Then
                        RelModuleSkills.Add(Affects(3))
                    End If
                End If
            Next
            RelModuleSkills.Sort()
            If RelModuleSkills.Count > 0 Then
                ' Add the Main menu item
                ParentButton.SubItems("btnAlterSkills" & idx.ToString).Text = "Alter Relevant Skills"
                ParentButton.SubItems("btnAlterSkills" & idx.ToString).SubItems.Clear()
                For Each relSkill As String In RelModuleSkills
                    Dim newRelSkill As New ButtonItem
                    newRelSkill.Name = relSkill
                    newRelSkill.Text = relSkill
                    Dim pilotLevel As Integer = 0
                    If CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet.Contains(relSkill) Then
                        pilotLevel = CType(CType(HQF.HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQF.HQFPilot).SkillSet(relSkill), HQFSkill).Level
                    Else
                        MessageBox.Show("There is a mis-match of roles for the " & ParentFitting.BaseShip.Name & ". Please report this to the EveHQ Developers.", "Ship Role Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    newRelSkill.Image = CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString), Image)
                    For skillLevel As Integer = 0 To 5
                        Dim newRelSkillLevel As New ButtonItem
                        newRelSkillLevel.Name = relSkill & skillLevel.ToString
                        newRelSkillLevel.Text = "Level " & skillLevel.ToString
                        If skillLevel = pilotLevel Then
                            newRelSkillLevel.Checked = True
                        End If
                        AddHandler newRelSkillLevel.Click, AddressOf Me.SetPilotBoosterSkillLevel
                        newRelSkill.SubItems.Add(newRelSkillLevel)
                    Next
                    Dim defaultLevel As Integer = 0
                    If CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills.Contains(relSkill) = True Then
                        defaultLevel = CType(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem.ToString), EveHQ.Core.Pilot).PilotSkills(relSkill), EveHQ.Core.PilotSkill).Level
                    Else
                    End If
                    Dim newRelSkillDefault As New ButtonItem
                    newRelSkillDefault.BeginGroup = True
                    newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                    newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                    AddHandler newRelSkillDefault.Click, AddressOf Me.SetPilotBoosterSkillLevel
                    newRelSkill.SubItems.Add(newRelSkillDefault)
                    ParentButton.SubItems("btnAlterSkills" & idx.ToString).SubItems.Add(newRelSkill)
                Next
            End If
        End If

    End Sub

    Private Sub ShowBoosterInfo(ByVal cb As ComboBox)
        If cb IsNot Nothing Then
            Dim sModule As ShipModule = CType(cb.Tag, ShipModule)
            For Each bModule As ShipModule In ParentFitting.FittedShip.BoosterSlotCollection
                If sModule.Name = bModule.Name Then
                    Dim showInfo As New frmShowInfo
                    Dim hPilot As EveHQ.Core.Pilot
                    If currentInfo IsNot Nothing Then
                        hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentInfo.cboPilots.SelectedItem), Core.Pilot)
                    Else
                        If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                            hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot)
                        Else
                            hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot)
                        End If
                    End If
                    showInfo.ShowItemDetails(bModule, hPilot)
                End If
            Next
        End If
    End Sub

    Private Sub RemoveBooster(ByVal cb As ComboBox, ByVal ParentButton As ButtonX, ByVal ButtonIdx As Integer)
        If cb IsNot Nothing Then
            cb.SelectedIndex = -1
            cb.Tag = Nothing
            ToolTip1.SetToolTip(cb, "")
            Dim cbidx As Integer = CInt(cb.Name.Substring(cb.Name.Length - 1, 1))
            tcpBoosters.Refresh()
            Call Me.ApplyBoosters(cb, ParentButton, ButtonIdx)
        End If
    End Sub

    Private Sub SetPilotBoosterSkillLevel(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mnuPilotLevel As ButtonItem = CType(sender, ButtonItem)
        Dim hPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(currentInfo.cboPilots.SelectedItem.ToString), HQFPilot)
        Dim pilotSkill As HQFSkill = CType(hPilot.SkillSet(mnuPilotLevel.Name.Substring(0, mnuPilotLevel.Name.Length - 1)), HQFSkill)
        Dim level As Integer = CInt(mnuPilotLevel.Name.Substring(mnuPilotLevel.Name.Length - 1))
        If level <> pilotSkill.Level Then
            pilotSkill.Level = level
            ParentFitting.ApplyFitting(BuildType.BuildEverything)
        End If
        ' Trigger an update of all open ship fittings!
        HQFEvents.StartUpdateShipInfo = hPilot.PilotName
        ' Rebuild all the menus for the boosters
        Call Me.BuildBoosterSkills(cboBoosterSlot1, btnBoosterSlot1, 1)
        Call Me.BuildBoosterSkills(cboBoosterSlot2, btnBoosterSlot2, 2)
        Call Me.BuildBoosterSkills(cboBoosterSlot3, btnBoosterSlot3, 3)
    End Sub

#End Region

End Class

Public Class UndoInfo
    Dim cTransaction As TransType
    Dim cSlotType As Integer = 0
    Dim cNewSlotNo As Integer = 0
    Dim cNewModName As String = ""
    Dim cNewChargeName As String = ""
    Dim cOldSlotNo As Integer = 0
    Dim cOldModName As String = ""
    Dim cOldChargeName As String = ""
    Dim cChargeOnly As Boolean = False

    Public Property Transaction() As TransType
        Get
            Return cTransaction
        End Get
        Set(ByVal value As TransType)
            cTransaction = value
        End Set
    End Property

    Public Property SlotType() As Integer
        Get
            Return cSlotType
        End Get
        Set(ByVal value As Integer)
            cSlotType = value
        End Set
    End Property

    Public Property NewSlotNo() As Integer
        Get
            Return cNewSlotNo
        End Get
        Set(ByVal value As Integer)
            cNewSlotNo = value
        End Set
    End Property

    Public Property NewModName() As String
        Get
            Return cNewModName
        End Get
        Set(ByVal value As String)
            cNewModName = value
        End Set
    End Property

    Public Property NewChargeName() As String
        Get
            Return cNewChargeName
        End Get
        Set(ByVal value As String)
            cNewChargeName = value
        End Set
    End Property

    Public Property OldSlotNo() As Integer
        Get
            Return cOldSlotNo
        End Get
        Set(ByVal value As Integer)
            cOldSlotNo = value
        End Set
    End Property

    Public Property OldModName() As String
        Get
            Return cOldModName
        End Get
        Set(ByVal value As String)
            cOldModName = value
        End Set
    End Property

    Public Property OldChargeName() As String
        Get
            Return cOldChargeName
        End Get
        Set(ByVal value As String)
            cOldChargeName = value
        End Set
    End Property

    Public Property ChargeOnly() As Boolean
        Get
            Return cChargeOnly
        End Get
        Set(ByVal value As Boolean)
            cChargeOnly = value
        End Set
    End Property

    Public Sub New(ByVal TransactionType As TransType, ByVal SlotType As Integer, ByVal SlotNo As Integer, ByVal ModName As String, ByVal ChargeName As String, ByVal ChargeOnly As Boolean)
        ' Used to add a module or charge, or remove a module or charge
        cTransaction = TransactionType
        cSlotType = SlotType
        cNewSlotNo = SlotNo
        cNewModName = ModName
        cNewChargeName = ChargeName
        cChargeOnly = ChargeOnly
    End Sub

    Public Sub New(ByVal TransactionType As TransType, ByVal SlotType As Integer, ByVal SlotNo1 As Integer, ByVal SlotNo2 As Integer)
        ' Used to swap modules
        cTransaction = TransactionType
        cSlotType = SlotType
        cOldSlotNo = SlotNo1
        cNewSlotNo = SlotNo2
        cChargeOnly = False
    End Sub

    Public Sub New(ByVal TransactionType As TransType, ByVal SlotType As Integer, ByVal OldSlotNo As Integer, ByVal OldModName As String, ByVal OldChargeName As String, ByVal NewSlotNo As Integer, ByVal NewModName As String, ByVal NewChargeName As String)
        ' Used to replace a module
        cTransaction = TransactionType
        cSlotType = SlotType
        cOldSlotNo = OldSlotNo
        cOldModName = OldModName
        cOldChargeName = OldChargeName
        cNewSlotNo = NewSlotNo
        cNewModName = NewModName
        cNewChargeName = NewChargeName
        cChargeOnly = False
    End Sub

    Public Enum TransType As Integer
        AddModule = 0
        RemoveModule = 1
        AddCharge = 2
        RemoveCharge = 3
        SwapModules = 4
        ReplacedModule = 5
    End Enum

End Class

