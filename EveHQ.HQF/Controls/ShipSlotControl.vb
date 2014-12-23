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
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports EveHQ.Core
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.EveData
Imports System.Threading.Tasks
Imports EveHQ.Common.Extensions
Imports EveHQ.HQF.Forms
Imports EveHQ.Core.Requisitions
Imports System.Text

Namespace Controls
    Public Class ShipSlotControl
        Dim _updateAll As Boolean = False
        Dim _updateDrones As Boolean = False
        Dim _updateBoosters As Boolean = False
        Dim _cancelDroneActivation As Boolean = False
        ReadOnly _rigGroups As New ArrayList
        ReadOnly _remoteGroups As New ArrayList
        ReadOnly _fleetGroups As New ArrayList
        ReadOnly _fleetSkills As New Dictionary(Of Integer, Integer)
        Private Const CancelSlotMenu As Boolean = False

        Dim _lvwBay As New ListView

        Private _
            Const DefaultModuleTooltipInfo As String =
            "<b><i>Double-click to remove this module<br />Right-click to bring up the module menu<br />Middle-click to toggle module state (extra uses with Ctrl/Shift)</i></b>"

        Private Const MaxTooltipWidth As Integer = 100

#Region "Property Variables"

        Private _currentInfo As ShipInfoControl
        Private _cUpdateAllSlots As Boolean
        Private ReadOnly _parentFitting As Fitting ' Stores the fitting to which this control is attached to
        Private _undoStack As Stack(Of UndoInfo)
        Private _redoStack As Stack(Of UndoInfo)

#End Region

#Region "Properties"

        Public ReadOnly Property ParentFitting() As Fitting
            Get
                Return _parentFitting
            End Get
        End Property

        Public Property UpdateAllSlots() As Boolean
            Get
                Return _cUpdateAllSlots
            End Get
            Set(ByVal value As Boolean)
                _cUpdateAllSlots = value
            End Set
        End Property

        Public Property UndoStack() As Stack(Of UndoInfo)
            Get
                Return _undoStack
            End Get
            Set(ByVal value As Stack(Of UndoInfo))
                _undoStack = value
            End Set
        End Property

        Public Property RedoStack() As Stack(Of UndoInfo)
            Get
                Return _redoStack
            End Get
            Set(ByVal value As Stack(Of UndoInfo))
                _redoStack = value
            End Set
        End Property

#End Region

#Region "Constructor and Related Routines"

        Public Sub New(ByVal shipFit As Fitting)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Set the parent fitting
            _parentFitting = shipFit

            ' Set the associated ShipInfoControl
            _currentInfo = ParentFitting.ShipInfoCtrl

            ' Add any initialization after the InitializeComponent() call.
            tcStorage.Height = PluginSettings.HQFSettings.StorageBayHeight
            _rigGroups.Add(773)
            _rigGroups.Add(782)
            _rigGroups.Add(778)
            _rigGroups.Add(780)
            _rigGroups.Add(786)
            _rigGroups.Add(781)
            _rigGroups.Add(775)
            _rigGroups.Add(776)
            _rigGroups.Add(779)
            _rigGroups.Add(904)
            _rigGroups.Add(777)
            _rigGroups.Add(896)
            _rigGroups.Add(774)
            _remoteGroups.Add(41) ' Shield transporters
            _remoteGroups.Add(325) ' Remote armor repairers
            _remoteGroups.Add(585) ' Remote hull repairers
            _remoteGroups.Add(67) ' Energy transfer arrays
            _remoteGroups.Add(65) ' Stasis webifiers
            _remoteGroups.Add(68) ' Energy vampires (nos)
            _remoteGroups.Add(71) ' Energy neutralisers
            _remoteGroups.Add(291) ' Tracking disruptors
            _remoteGroups.Add(209) ' Tracking links
            _remoteGroups.Add(289) ' Projected ECCM
            _remoteGroups.Add(290) ' Remote sensor boosters
            _remoteGroups.Add(208) ' Remote sensor dampener
            _remoteGroups.Add(379) ' Target painter
            _remoteGroups.Add(544) ' Cap drain drone
            _remoteGroups.Add(641) ' Stasis web drone
            _remoteGroups.Add(640) ' Shield/armor repair drone
            _remoteGroups.Add(639) ' EW Drone
            _fleetGroups.Add(316)
            _fleetSkills.Add(ModuleEnum.ItemSkillLeadership, AttributeEnum.SkillScanResBonus)
            _fleetSkills.Add(ModuleEnum.ItemSkillArmoredWarfare, AttributeEnum.SkillArmorHpBonus)
            _fleetSkills.Add(ModuleEnum.ItemSkillInformationWarfare, AttributeEnum.SkillTargetRangeBonus)
            _fleetSkills.Add(ModuleEnum.ItemSkillSiegeWarfare, AttributeEnum.SkillShieldHpBonus)
            _fleetSkills.Add(ModuleEnum.ItemSkillSkirmishWarfare, AttributeEnum.SkillAgilityBonus)
            _fleetSkills.Add(ModuleEnum.ItemSkillMiningForeman, AttributeEnum.SkillMiningAmountBonus)

            ' Load the remote and fleet info
            Call LoadRemoteFleetInfo()

            ' Load the Booster info
            Call LoadBoosterInfo()

            ' Load WH Info
            Call LoadWormholeInfo()

            _undoStack = New Stack(Of UndoInfo)
            _redoStack = New Stack(Of UndoInfo)
        End Sub

#End Region

#Region "Update Routines"

        Public Sub UpdateEverything()
            ' Update the slot layout
            _currentInfo = ParentFitting.ShipInfoCtrl
            ' Build Subsystems
            ParentFitting.BaseShip = ParentFitting.BuildSubSystemEffects(ParentFitting.BaseShip)
            _updateAll = True
            Call UpdateShipSlotColumns()
            lvwCargoBay.BeginUpdate()
            lvwDroneBay.BeginUpdate()
            ClearShipSlots()
            ClearDroneBay()
            ClearCargoBay()
            ClearShipBay()
            ParentFitting.UpdateBaseShipFromFitting()
            UpdateShipSlotLayout()
            lvwCargoBay.EndUpdate()
            lvwDroneBay.EndUpdate()
            ParentFitting.ShipInfoCtrl.UpdateImplantList()
            ParentFitting.ApplyFitting(BuildType.BuildEverything)
            If ParentFitting.FittedShip IsNot Nothing Then
                UpdateShipDetails()
                RedrawDroneBay()
                RedrawCargoBay()
                RedrawShipBay()
                UpdateBoosterSlots()
                UpdateWormholeUi()
                UpdateNotes()
                UpdateTags()
                Call UpdatePrices()
            Else
                MessageBox.Show("The fitting for " & ParentFitting.KeyName & " failed to produce a calculated setup.",
                                "Error Calculating Fitting", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            _updateAll = False
        End Sub

        Private Sub UpdateShipDetails()
            If _updateAll = False Then
                Call UpdateSlotNumbers()
                Call UpdatePrices()
                Call ParentFitting.UpdateFittingFromBaseShip()
            End If
        End Sub

        Private Sub UpdateSlotNumbers()
            Dim sc As Node = adtSlots.FindNodeByName("8")
            If sc IsNot Nothing Then
                adtSlots.FindNodeByName("8").Text = "High Slots (Launchers: " & ParentFitting.BaseShip.LauncherSlotsUsed &
                                                    "/" & ParentFitting.BaseShip.LauncherSlots & ", Turrets: " &
                                                    ParentFitting.BaseShip.TurretSlotsUsed & "/" &
                                                    ParentFitting.BaseShip.TurretSlots & ")"
            End If
        End Sub

        Private Sub UpdatePrices()
            ' get a collection of the item Ids used in the fitting
            Dim itemIds As New List(Of Integer)

            ' Get the base ship ID, for custom ships, this will need to be the hull on which the custom ship is based on
            ' Fixes EVEHQ-178
            Dim baseShipId As Integer
            If StaticData.Types.ContainsKey(CInt(ParentFitting.BaseShip.ID)) Then
                baseShipId = ParentFitting.BaseShip.ID
            Else
                If CustomHQFClasses.CustomShipIDs.ContainsKey(ParentFitting.BaseShip.ID) Then
                    If _
                        StaticData.TypeNames.ContainsKey(
                            CustomHQFClasses.CustomShips(CustomHQFClasses.CustomShipIDs(ParentFitting.BaseShip.ID)).
                                                            BaseShipName) Then
                        baseShipId =
                            StaticData.TypeNames(
                                CustomHQFClasses.CustomShips(CustomHQFClasses.CustomShipIDs(ParentFitting.BaseShip.ID)).
                                                    BaseShipName)
                    End If
                End If
            End If

            ' Add the baseShipID, but only if not zero
            If baseShipId <> 0 Then
                itemIds.Add(CInt(baseShipId))
            End If

            ' add in the HiSlot items
            For slot As Integer = 1 To ParentFitting.FittedShip.HiSlots
                If ParentFitting.FittedShip.HiSlot(slot) IsNot Nothing Then
                    itemIds.Add(CInt(ParentFitting.FittedShip.HiSlot(slot).ID))
                End If
            Next
            ' Mids
            For slot As Integer = 1 To ParentFitting.FittedShip.MidSlots
                If ParentFitting.FittedShip.MidSlot(slot) IsNot Nothing Then
                    itemIds.Add(CInt(ParentFitting.FittedShip.MidSlot(slot).ID))
                End If
            Next
            ' Lows
            For slot As Integer = 1 To ParentFitting.FittedShip.LowSlots
                If ParentFitting.FittedShip.LowSlot(slot) IsNot Nothing Then
                    itemIds.Add(CInt(ParentFitting.FittedShip.LowSlot(slot).ID))
                End If
            Next
            'Rigs
            For slot As Integer = 1 To ParentFitting.FittedShip.RigSlots
                If ParentFitting.FittedShip.RigSlot(slot) IsNot Nothing Then
                    itemIds.Add(CInt(ParentFitting.FittedShip.RigSlot(slot).ID))
                End If
            Next

            'Subsystems
            For slot As Integer = 1 To ParentFitting.FittedShip.SubSlots
                If ParentFitting.FittedShip.SubSlot(slot) IsNot Nothing Then
                    itemIds.Add(CInt(ParentFitting.FittedShip.SubSlot(slot).ID))
                End If
            Next

            'Drone bay
            itemIds.AddRange(
                From dbi As Object In ParentFitting.FittedShip.DroneBayItems.Values
                                Select CInt(CType(dbi, DroneBayItem).DroneType.ID))

            'Cargo bay
            For Each item As Object In ParentFitting.FittedShip.CargoBayItems.Values
                itemIds.Add(CInt(CType(item, CargoBayItem).ItemType.ID))
            Next

            ' Calculate the fitted prices
            Dim priceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(itemIds)


            priceTask.ContinueWith(Sub(currentTask As Task(Of Dictionary(Of Integer, Double)))

                                       'Bug EVEHQ-169 : this is called even after the window is destroyed but not GC'd. check the handle boolean first.
                                       If IsHandleCreated Then


                                           Dim prices As Dictionary(Of Integer, Double) = currentTask.Result

                                           ' call back to main thread to update UI
                                           Invoke(Sub()
                                                      ' update the values
                                                      ' base ship price
                                                      If baseShipId <> 0 Then
                                                          ParentFitting.BaseShip.MarketPrice = prices(CInt(baseShipId))
                                                      End If
                                                      ' the sum of all the modules except the ship item.
                                                      Dim total As Double =
                                                              prices.Sum(
                                                                  Function(itemPrice) _
                                                                            itemPrice.Value * (From id In itemIds Where id = itemPrice.Key).Count())

                                                      ParentFitting.BaseShip.FittingMarketPrice = total

                                                      lblShipMarketPrice.Text = "Ship Price: " &
                                                                                ParentFitting.BaseShip.MarketPrice.ToInvariantString("N2")
                                                      lblFittingMarketPrice.Text = "Total Price: " &
                                                                                   (ParentFitting.BaseShip.FittingMarketPrice).
                                                                                       ToInvariantString("N2")
                                                  End Sub)
                                       End If
                                   End Sub)
        End Sub

        Public Sub UpdateAllSlotLocations()
            'Dim sTime, eTime As Date
            'sTime = Now
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
                Call RedrawCargoBayCapacity()
                Call RedrawDroneBayCapacity()
                Call RedrawShipBayCapacity()
            End If
            'eTime = Now
            'MessageBox.Show((eTime - sTime).TotalMilliseconds.ToString & "ms", "Ship Slot Control Update")
            Call UpdateShipDetails()
            If PluginSettings.HQFSettings.AutoResizeColumns = True Then
                Call AutoSizeAllColumns()
            End If
        End Sub

        Private Sub UpdateSlotLocation(ByVal oldMod As ShipModule, ByVal slotNo As Integer)
            If oldMod IsNot Nothing Then
                Dim shipMod As New ShipModule
                Select Case oldMod.SlotType
                    Case SlotTypes.Rig
                        shipMod = ParentFitting.FittedShip.RigSlot(slotNo)
                    Case SlotTypes.Low
                        shipMod = ParentFitting.FittedShip.LowSlot(slotNo)
                    Case SlotTypes.Mid
                        shipMod = ParentFitting.FittedShip.MidSlot(slotNo)
                    Case SlotTypes.High
                        shipMod = ParentFitting.FittedShip.HiSlot(slotNo)
                    Case SlotTypes.Subsystem
                        shipMod = ParentFitting.FittedShip.SubSlot(slotNo)
                End Select
                If shipMod IsNot Nothing Then
                    shipMod.ModuleState = oldMod.ModuleState
                    Dim slotNode As Node = adtSlots.FindNodeByName(shipMod.SlotType & "_" & slotNo)
                    slotNode.Image =
                        CType(My.Resources.ResourceManager.GetObject("Mod0" & CInt(shipMod.ModuleState).ToString), Image)
                    Call UpdateUserColumnData(shipMod, slotNode)
                End If
            End If
        End Sub

#End Region

#Region "New Slot Update Routines"

        
        ''' <summary>
        '''     Updates the column headers of the ship slot control
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub UpdateShipSlotColumns()
            ' Clear the columns
            adtSlots.BeginUpdate()
            adtSlots.Columns.Clear()
            ' Add the module name column
            Dim mainCol As New DevComponents.AdvTree.ColumnHeader("Module Name")
            mainCol.Name = "colName"
            mainCol.SortingEnabled = False
            mainCol.Width.Absolute = PluginSettings.HQFSettings.SlotNameWidth
            mainCol.Width.AutoSizeMinHeader = True
            adtSlots.Columns.Add(mainCol)
            ' Iterate through the user selected columns and add them in
            For Each userCol As UserSlotColumn In PluginSettings.HQFSettings.UserSlotColumns
                If userCol.Active = True Then
                    Dim subCol As New DevComponents.AdvTree.ColumnHeader(userCol.Name)
                    subCol.SortingEnabled = False
                    subCol.Name = userCol.Name
                    subCol.Width.Absolute = userCol.Width
                    subCol.Width.AutoSizeMinHeader = True
                    adtSlots.Columns.Add(subCol)
                End If
            Next
            adtSlots.EndUpdate()
        End Sub

        
        ''' <summary>
        '''     Draws placeholders for the ship slots
        '''     Also update ship mode switch availability
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateShipSlotLayout()

            adtSlots.BeginUpdate()
            adtSlots.Nodes.Clear()

            Const ImageSize As Integer = 24
            Dim hiSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
            Dim midSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
            Dim lowSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
            Dim rigSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
            Dim subSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
            Dim selSlotStyle As ElementStyle = adtSlots.Styles("SlotStyle").Copy
            selSlotStyle.BackColorGradientType = eGradientType.Linear
            selSlotStyle.BackColor = Color.Orange
            selSlotStyle.BackColor2 = Color.OrangeRed


            ' Create high slots
            If ParentFitting.BaseShip.HiSlots > 0 Then
                Dim parentNode As New Node("High Slots", adtSlots.Styles("HeaderStyle"))
                parentNode.Name = "8"
                parentNode.FullRowBackground = True
                parentNode.Image = New Bitmap(My.Resources.imgHiSlot, ImageSize, ImageSize)
                adtSlots.Nodes.Add(parentNode)
                For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                    Dim slotNode As New Node("", hiSlotStyle)
                    slotNode.Name = "8_" & slot
                    slotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                    slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.HiSlotColour))
                    slotNode.StyleSelected = selSlotStyle
                    Call AddUserColumnData(ParentFitting.BaseShip.HiSlot(slot), slotNode)
                    parentNode.Nodes.Add(slotNode)
                Next
                parentNode.Expanded = True
            End If

            ' Create mid slots
            If ParentFitting.BaseShip.MidSlots > 0 Then
                Dim parentNode As New Node("Mid Slots", adtSlots.Styles("HeaderStyle"))
                parentNode.Name = "4"
                parentNode.FullRowBackground = True
                parentNode.Image = New Bitmap(My.Resources.imgMidSlot, ImageSize, ImageSize)
                adtSlots.Nodes.Add(parentNode)
                For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                    Dim slotNode As New Node("", midSlotStyle)
                    slotNode.Name = "4_" & slot
                    slotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                    slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.MidSlotColour))
                    slotNode.StyleSelected = selSlotStyle
                    Call AddUserColumnData(ParentFitting.BaseShip.MidSlot(slot), slotNode)
                    parentNode.Nodes.Add(slotNode)
                Next
                parentNode.Expanded = True
            End If

            ' Create low slots
            If ParentFitting.BaseShip.LowSlots > 0 Then
                Dim parentNode As New Node("Low Slots", adtSlots.Styles("HeaderStyle"))
                parentNode.Name = "2"
                parentNode.FullRowBackground = True
                parentNode.Image = New Bitmap(My.Resources.imgLowSlot, ImageSize, ImageSize)
                adtSlots.Nodes.Add(parentNode)
                For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                    Dim slotNode As New Node("", lowSlotStyle)
                    slotNode.Name = "2_" & slot
                    slotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                    slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.LowSlotColour))
                    slotNode.StyleSelected = selSlotStyle
                    Call AddUserColumnData(ParentFitting.BaseShip.LowSlot(slot), slotNode)
                    parentNode.Nodes.Add(slotNode)
                Next
                parentNode.Expanded = True
            End If

            ' Create rig slots
            If ParentFitting.BaseShip.RigSlots > 0 Then
                Dim parentNode As New Node("Rig Slots", adtSlots.Styles("HeaderStyle"))
                parentNode.Name = "1"
                parentNode.FullRowBackground = True
                parentNode.Image = New Bitmap(My.Resources.imgRigSlot, ImageSize, ImageSize)
                adtSlots.Nodes.Add(parentNode)
                For slot As Integer = 1 To ParentFitting.BaseShip.RigSlots
                    Dim slotNode As New Node("", rigSlotStyle)
                    slotNode.Name = "1_" & slot
                    slotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                    slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.RigSlotColour))
                    slotNode.StyleSelected = selSlotStyle
                    Call AddUserColumnData(ParentFitting.BaseShip.RigSlot(slot), slotNode)
                    parentNode.Nodes.Add(slotNode)
                Next
                parentNode.Expanded = True
            End If

            ' Create sub slots
            If ParentFitting.BaseShip.SubSlots > 0 Then
                Dim parentNode As New Node("Subsystem Slots", adtSlots.Styles("HeaderStyle"))
                parentNode.Name = "16"
                parentNode.FullRowBackground = True
                parentNode.Image = New Bitmap(My.Resources.imgSubSlot, ImageSize, ImageSize)
                adtSlots.Nodes.Add(parentNode)
                For slot As Integer = 1 To ParentFitting.BaseShip.SubSlots
                    Dim slotNode As New Node("", subSlotStyle)
                    slotNode.Name = "16_" & slot
                    slotNode.Style.BackColor = Color.FromArgb(255, 255, 255)
                    slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.SubSlotColour))
                    slotNode.StyleSelected = selSlotStyle
                    Call AddUserColumnData(ParentFitting.BaseShip.SubSlot(slot), slotNode)
                    parentNode.Nodes.Add(slotNode)
                Next
                parentNode.Expanded = True
            End If

            adtSlots.EndUpdate()

            ' Update ship mode switch
            Dim shipModeVisible = ParentFitting.BaseShip.Attributes.ContainsKey(1985)
            lblShipMode.Visible = shipModeVisible
            btnShipMode0.Visible = shipModeVisible
            btnShipMode1.Visible = shipModeVisible
            btnShipMode2.Visible = shipModeVisible
            btnShipMode3.Visible = shipModeVisible
            Select Case ParentFitting.ShipMode
                Case ShipModes.None
                    btnShipMode0.Checked = True
                Case ShipModes.Defence
                    btnShipMode1.Checked = True
                Case ShipModes.Sharpshooting
                    btnShipMode2.Checked = True
                Case ShipModes.Propulsion
                    btnShipMode3.Checked = True
            End Select
            
            ' Update details
            Call UpdateSlotNumbers()
            Call UpdatePrices()
        End Sub

        
        ''' <summary>
        '''     Creates the individual cells of a node based on the ship module and user columns required
        ''' </summary>
        ''' <param name="shipMod">The module to use the information from</param>
        ''' <param name="slotNode">The particular node to update</param>
        ''' <remarks></remarks>
        Private Sub AddUserColumnData(ByVal shipMod As ShipModule, ByVal slotNode As Node)
            ' Add subitems based on the user selected columns
            If shipMod IsNot Nothing Then
                ' Add in the module name
                slotNode.Text = shipMod.Name
                Dim desc As String = ""
                If shipMod.SlotType = SlotTypes.Subsystem Then
                    desc &= "Slot Modifiers - High: " & shipMod.Attributes(AttributeEnum.ModuleHighSlotModifier) &
                            ", Mid: " & shipMod.Attributes(AttributeEnum.ModuleMidSlotModifier) & ", Low: " &
                            shipMod.Attributes(AttributeEnum.ModuleLowSlotModifier) & ControlChars.CrLf &
                            ControlChars.CrLf

                End If
                desc &= shipMod.Description
                SlotTip.SetSuperTooltip(slotNode,
                                        New SuperTooltipInfo(shipMod.Name, "Ship Module Information", desc,
                                                             ImageHandler.IconImage48(shipMod.Icon, shipMod.MetaType),
                                                             My.Resources.imgInfo1, eTooltipColor.Yellow))
                ' Add the additional columns
                For Each userCol As UserSlotColumn In PluginSettings.HQFSettings.UserSlotColumns
                    If userCol.Active = True Then
                        Select Case userCol.Name
                            Case "Charge"
                                If shipMod.LoadedCharge IsNot Nothing Then
                                    slotNode.Cells.Add(New Cell(shipMod.LoadedCharge.Name))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "CPU"
                                slotNode.Cells.Add(New Cell(shipMod.Cpu.ToString("N2")))
                            Case "PG"
                                slotNode.Cells.Add(New Cell(shipMod.Pg.ToString("N2")))
                            Case "Calib"
                                slotNode.Cells.Add(New Cell(shipMod.Calibration.ToString("N2")))
                            Case "Price"

                                Dim priceCell As New Cell()
                                slotNode.Cells.Add(priceCell)
                            Case "ActCost"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleCapacitorNeed) Then
                                    If _
                                        shipMod.ModuleState = ModuleStates.Active Or
                                        shipMod.ModuleState = ModuleStates.Overloaded Then
                                        slotNode.Cells.Add(New Cell(shipMod.CapUsage.ToString("N2")))
                                    Else
                                        slotNode.Cells.Add(New Cell(""))
                                    End If
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "ActTime"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleActivationTime) Then
                                    If _
                                        shipMod.ModuleState = ModuleStates.Active Or
                                        shipMod.ModuleState = ModuleStates.Overloaded Then
                                        slotNode.Cells.Add(New Cell(shipMod.ActivationTime.ToString("N2")))
                                    Else
                                        slotNode.Cells.Add(New Cell(""))
                                    End If
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "CapRate"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleCapUsageRate) Then
                                    If _
                                        shipMod.ModuleState = ModuleStates.Active Or
                                        shipMod.ModuleState = ModuleStates.Overloaded Then
                                        slotNode.Cells.Add(New Cell((shipMod.CapUsageRate*- 1).ToString("N2")))
                                    Else
                                        slotNode.Cells.Add(New Cell(""))
                                    End If
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "OptRange"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleOptimalRange) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleOptimalRange).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleShieldTransferRange) Then
                                    slotNode.Cells.Add(
                                        New Cell(
                                            shipMod.Attributes(AttributeEnum.ModuleShieldTransferRange).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModulePowerTransferRange) Then
                                    slotNode.Cells.Add(
                                        New Cell(
                                            shipMod.Attributes(AttributeEnum.ModulePowerTransferRange).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleEnergyNeutRange) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleEnergyNeutRange).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleAoERadius) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleAoERadius).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleWarpScrambleRange) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleWarpScrambleRange).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleECMBurstRadius) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleECMBurstRadius).ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "ROF"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleRof) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleRof).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleEnergyRof) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleEnergyRof).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleHybridRof) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleHybridRof).ToString("N2")))
                                ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleProjectileRof) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleProjectileRof).ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "Damage"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleVolleyDamage) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleVolleyDamage).ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "DPS"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleDPS) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleDPS).ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "Falloff"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleFalloff) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleFalloff).ToString("N2")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "Tracking"
                                If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleTrackingSpeed) Then
                                    slotNode.Cells.Add(
                                        New Cell(shipMod.Attributes(AttributeEnum.ModuleTrackingSpeed).ToString("N4")))
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "ExpRad"
                                If shipMod.LoadedCharge IsNot Nothing Then
                                    If shipMod.LoadedCharge.Attributes.ContainsKey(AttributeEnum.ModuleExplosionRadius) _
                                        Then
                                        slotNode.Cells.Add(
                                            New Cell(
                                                shipMod.LoadedCharge.Attributes(AttributeEnum.ModuleExplosionRadius).
                                                        ToString("N2")))
                                    Else
                                        slotNode.Cells.Add(New Cell(""))
                                    End If
                                Else
                                    slotNode.Cells.Add(New Cell(""))
                                End If
                            Case "ExpVel"
                                If shipMod.LoadedCharge IsNot Nothing Then
                                    If _
                                        shipMod.LoadedCharge.Attributes.ContainsKey(
                                            AttributeEnum.ModuleExplosionVelocity) Then
                                        slotNode.Cells.Add(
                                            New Cell(
                                                shipMod.LoadedCharge.Attributes(AttributeEnum.ModuleExplosionVelocity).
                                                        ToString("N2")))
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
                For Each userCol As UserSlotColumn In PluginSettings.HQFSettings.UserSlotColumns
                    If userCol.Active = True Then
                        slotNode.Cells.Add(New Cell(""))
                    End If
                Next
            End If
        End Sub

        
        ''' <summary>
        '''     Updates the individual cells of a node based on the ship module and user columns
        ''' </summary>
        ''' <param name="shipMod">The module to use the information from</param>
        ''' <param name="slotNode">The particular node to update</param>
        ''' <remarks></remarks>
        Private Sub UpdateUserColumnData(ByVal shipMod As ShipModule, ByVal slotNode As Node)
            ' Add subitems based on the user selected columns
            Dim idx As Integer = 1
            ' Add in the module name
            slotNode.Text = shipMod.Name
            Dim desc As String = ""
            If shipMod.SlotType = SlotTypes.Subsystem Then
                desc &= "Slot Modifiers - High: " & shipMod.Attributes(AttributeEnum.ModuleHighSlotModifier) & ", Mid: " &
                        shipMod.Attributes(AttributeEnum.ModuleMidSlotModifier) & ", Low: " &
                        shipMod.Attributes(AttributeEnum.ModuleLowSlotModifier) & ControlChars.CrLf & ControlChars.CrLf


            End If
            desc &= shipMod.Description
            SlotTip.SetSuperTooltip(slotNode,
                                    New SuperTooltipInfo(shipMod.Name, DefaultModuleTooltipInfo, desc,
                                                         ImageHandler.IconImage48(shipMod.Icon, shipMod.MetaType),
                                                         My.Resources.imgInfo1, eTooltipColor.Yellow))
            ' Add the additional columns
            For Each userCol As UserSlotColumn In PluginSettings.HQFSettings.UserSlotColumns
                If userCol.Active = True Then
                    Select Case userCol.Name
                        Case "Charge"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                slotNode.Cells(idx).Text = shipMod.LoadedCharge.Name
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "CPU"
                            slotNode.Cells(idx).Text = shipMod.Cpu.ToString("N2")
                            idx += 1
                        Case "PG"
                            slotNode.Cells(idx).Text = shipMod.Pg.ToString("N2")
                            idx += 1
                        Case "Calib"
                            slotNode.Cells(idx).Text = shipMod.Calibration.ToString("N2")
                            idx += 1
                        Case "Price"
                            Dim currentIndex As Integer = idx
                            Dim task As Task(Of Double) = DataFunctions.GetPriceAsync(CInt(shipMod.ID))
                            task.ContinueWith(Sub(price As Task(Of Double))
                                If IsHandleCreated Then
                                    If (price.IsCompleted And price.IsFaulted = False) Then
                                        'Bug EVEHQ-169 : this is called even after the window is destroyed but not GC'd. check the handle boolean first.

                                        Invoke(Sub()
                                            shipMod.MarketPrice = price.Result
                                            slotNode.Cells(currentIndex).Text = shipMod.MarketPrice.ToString("N2")
                                                  End Sub)
                                                 End If
                                                 End If
                                                 End Sub)

                            idx += 1
                        Case "ActCost"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleCapacitorNeed) Then
                                If _
                                    shipMod.ModuleState = ModuleStates.Active Or
                                    shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotNode.Cells(idx).Text = shipMod.CapUsage.ToString("N2")
                                Else
                                    slotNode.Cells(idx).Text = ""
                                End If
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "ActTime"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleActivationTime) Then
                                If _
                                    shipMod.ModuleState = ModuleStates.Active Or
                                    shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotNode.Cells(idx).Text = shipMod.ActivationTime.ToString("N2")
                                Else
                                    slotNode.Cells(idx).Text = ""
                                End If
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "CapRate"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleCapUsageRate) Then
                                If _
                                    shipMod.ModuleState = ModuleStates.Active Or
                                    shipMod.ModuleState = ModuleStates.Overloaded Then
                                    slotNode.Cells(idx).Text = (shipMod.CapUsageRate*- 1).ToString("N2")
                                Else
                                    slotNode.Cells(idx).Text = ""
                                End If
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "OptRange"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleOptimalRange) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleOptimalRange).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleShieldTransferRange) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleShieldTransferRange).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModulePowerTransferRange) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModulePowerTransferRange).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleEnergyNeutRange) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleEnergyNeutRange).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleAoERadius) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleAoERadius).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleWarpScrambleRange) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleWarpScrambleRange).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleECMBurstRadius) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleECMBurstRadius).ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "ROF"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleRof) Then
                                slotNode.Cells(idx).Text = shipMod.Attributes(AttributeEnum.ModuleRof).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleEnergyRof) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleEnergyRof).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleHybridRof) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleHybridRof).ToString("N2")
                            ElseIf shipMod.Attributes.ContainsKey(AttributeEnum.ModuleProjectileRof) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleProjectileRof).ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "Damage"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleVolleyDamage) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleVolleyDamage).ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "DPS"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleDPS) Then
                                slotNode.Cells(idx).Text = shipMod.Attributes(AttributeEnum.ModuleDPS).ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "Falloff"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleFalloff) Then
                                slotNode.Cells(idx).Text = shipMod.Attributes(AttributeEnum.ModuleFalloff).ToString("N2")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "Tracking"
                            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleTrackingSpeed) Then
                                slotNode.Cells(idx).Text =
                                    shipMod.Attributes(AttributeEnum.ModuleTrackingSpeed).ToString("N4")
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "ExpRad"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                If shipMod.LoadedCharge.Attributes.ContainsKey(AttributeEnum.ModuleExplosionRadius) Then
                                    slotNode.Cells(idx).Text =
                                        shipMod.LoadedCharge.Attributes(AttributeEnum.ModuleExplosionRadius).ToString(
                                            "N2")
                                Else
                                    slotNode.Cells(idx).Text = ""
                                End If
                            Else
                                slotNode.Cells(idx).Text = ""
                            End If
                            idx += 1
                        Case "ExpVel"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                If shipMod.LoadedCharge.Attributes.ContainsKey(AttributeEnum.ModuleExplosionVelocity) _
                                    Then
                                    slotNode.Cells(idx).Text =
                                        shipMod.LoadedCharge.Attributes(AttributeEnum.ModuleExplosionVelocity).ToString(
                                            "N2")
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

        Private Sub adtSlots_ColumnMoved(ByVal sender As Object, ByVal ea As ColumnMovedEventArgs) _
            Handles adtSlots.ColumnMoved
            ' Get true locations
            Dim startColName As String = ea.Column.Name
            Dim endColName As String = adtSlots.Columns(ea.NewColumnDisplayIndex).Name
            Dim startIdx As Integer = 0
            Dim endIdx As Integer = 0
            For idx As Integer = 1 To PluginSettings.HQFSettings.UserSlotColumns.Count - 1
                If PluginSettings.HQFSettings.UserSlotColumns(idx).Name = startColName Then
                    startIdx = idx
                End If
                If PluginSettings.HQFSettings.UserSlotColumns(idx).Name = endColName Then
                    endIdx = idx
                End If
            Next

            If startIdx = 0 Then
                ' Ignore stuff
            Else
                Dim sCol As UserSlotColumn = PluginSettings.HQFSettings.UserSlotColumns(startIdx)
                Dim startUserCol As New UserSlotColumn(sCol.Name, sCol.Description, sCol.Width, sCol.Active)
                If endIdx > startIdx Then
                    For idx As Integer = startIdx To endIdx - 1
                        Dim mCol As UserSlotColumn = PluginSettings.HQFSettings.UserSlotColumns(idx + 1)
                        PluginSettings.HQFSettings.UserSlotColumns(idx) = New UserSlotColumn(mCol.Name, mCol.Description,
                                                                                             mCol.Width, mCol.Active)
                    Next
                    PluginSettings.HQFSettings.UserSlotColumns(endIdx) = startUserCol
                Else
                    For idx As Integer = startIdx - 1 To endIdx Step - 1
                        Dim mCol As UserSlotColumn = PluginSettings.HQFSettings.UserSlotColumns(idx)
                        PluginSettings.HQFSettings.UserSlotColumns(idx + 1) = New UserSlotColumn(mCol.Name,
                                                                                                 mCol.Description,
                                                                                                 mCol.Width, mCol.Active)
                    Next
                    PluginSettings.HQFSettings.UserSlotColumns(endIdx) = startUserCol
                End If
            End If

            Call UpdateShipSlotColumns()
            Call UpdateAllSlotLocations()
        End Sub

        Private Sub adtSlots_ColumnResized(ByVal sender As Object, ByVal e As EventArgs) Handles adtSlots.ColumnResized
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            If ch.Name <> "colName" Then
                For Each userCol As UserSlotColumn In PluginSettings.HQFSettings.UserSlotColumns
                    If userCol.Name = ch.Name Then
                        userCol.Width = ch.Width.Absolute
                        Exit Sub
                    End If
                Next
            Else
                PluginSettings.HQFSettings.SlotNameWidth = ch.Width.Absolute
            End If
        End Sub

        Private Sub adtSlots_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles adtSlots.KeyDown
            Select Case e.KeyCode
                Case Keys.Delete
                    Call RemoveModules(sender, e)
                Case Keys.C
                    If e.Control = True Then
                        Call CopyModulesToClipboard()
                    End If
                Case Keys.V
                    If e.Control = True Then
                        Call PasteModuleFromClipboard()
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

        Private Sub adtSlots_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) _
            Handles adtSlots.NodeDoubleClick
            If e.Button = MouseButtons.Left Then
                ' Only remove if double-clicking the left button!
                If adtSlots.SelectedNodes.Count > 0 Then
                    ' Check if the "slot" is not empty
                    If CancelSlotMenu = False Then
                        If adtSlots.SelectedNodes(0).Text <> "<Empty>" Then
                            Call UpdateMruModules(adtSlots.SelectedNodes(0).Text)
                            Call RemoveModule(adtSlots.SelectedNodes(0), True, False)
                            HQFEvents.StartUpdateModuleList = True
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub UpdateMruModules(ByVal modName As String)
            If PluginSettings.HQFSettings.MruModules.Count < PluginSettings.HQFSettings.MruLimit Then
                ' If the MRU list isn't full
                If PluginSettings.HQFSettings.MruModules.Contains(modName) = False Then
                    ' If the module isn't already in the list
                    PluginSettings.HQFSettings.MruModules.Add(modName)
                Else
                    ' If it is in the list, remove it and add it at the end
                    PluginSettings.HQFSettings.MruModules.Remove(modName)
                    PluginSettings.HQFSettings.MruModules.Add(modName)
                End If
            Else
                If PluginSettings.HQFSettings.MruModules.Contains(modName) = False Then
                    For m As Integer = 0 To PluginSettings.HQFSettings.MruLimit - 2
                        PluginSettings.HQFSettings.MruModules(m) = PluginSettings.HQFSettings.MruModules(m + 1)
                    Next
                    PluginSettings.HQFSettings.MruModules.RemoveAt(PluginSettings.HQFSettings.MruLimit - 1)
                    PluginSettings.HQFSettings.MruModules.Add(modName)
                Else
                    ' If it is in the list, remove it and add it at the end
                    PluginSettings.HQFSettings.MruModules.Remove(modName)
                    PluginSettings.HQFSettings.MruModules.Add(modName)
                End If
            End If
        End Sub

        Private Sub adtSlots_NodeClick(sender As Object, e As TreeNodeMouseEventArgs) Handles adtSlots.NodeClick
            If e.Node.Level = 0 Then
                adtSlots.SelectedNodes.Clear()
                For Each subNode As Node In e.Node.Nodes
                    adtSlots.SelectedNodes.Add(subNode, eTreeAction.Keyboard)
                Next
            End If
        End Sub

        Private Sub adtSlots_NodeMouseDown(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) _
            Handles adtSlots.NodeMouseDown

            If e.Node IsNot Nothing Then

                If e.Node.Level > 0 Then

                    If e.Button = MouseButtons.Middle Then

                        ' Check for key status
                        Dim keyMode As Integer = 0 ' 0=None, 1=Shift, 2=Ctrl, 4=Alt
                        If My.Computer.Keyboard.ShiftKeyDown Then keyMode += 1
                        If My.Computer.Keyboard.CtrlKeyDown Then keyMode += 2
                        If My.Computer.Keyboard.AltKeyDown Then keyMode += 4

                        ' Check which mode, single or multi
                        If adtSlots.SelectedNodes.Count > 1 Then
                            If e.Node.IsSelected = True Then
                                For Each selNode As Node In adtSlots.SelectedNodes
                                    Call ChangeSingleModuleState(keyMode, selNode)
                                Next
                            Else
                                Call ChangeSingleModuleState(keyMode, e.Node)
                            End If
                        Else
                            Call ChangeSingleModuleState(keyMode, e.Node)
                        End If

                        ' Update the ship data
                        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)

                    End If

                End If

            End If
        End Sub

        Private Sub ChangeModuleState(keyMode As Integer)
            If adtSlots.SelectedNodes.Count > 0 Then
                For Each selNode As Node In adtSlots.SelectedNodes
                    Call ChangeSingleModuleState(keyMode, selNode)
                Next
            End If
            ' Update the ship data
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Sub

        
        ''' <summary>
        '''     Changes the module state of the module in the current node (slot)
        ''' </summary>
        ''' <param name="keyMode">The Shift/Ctrl/Aly key combination used in conjunction with the state change</param>
        ''' <param name="slotNode">The affected slot node</param>
        ''' <remarks></remarks>
        Private Sub ChangeSingleModuleState(ByVal keyMode As Integer, ByVal slotNode As Node)
            ' Get the module details
            Dim currentMod As New ShipModule
            Dim fittedMod As New ShipModule
            Dim sep As Integer = slotNode.Name.LastIndexOf("_", StringComparison.Ordinal)
            Dim slotType As Integer = CInt(slotNode.Name.Substring(0, sep))
            Dim slotNo As Integer = CInt(slotNode.Name.Substring(sep + 1, 1))
            Dim canOffline As Boolean = True
            Select Case slotType
                Case SlotTypes.Rig
                    currentMod = ParentFitting.BaseShip.RigSlot(slotNo)
                    fittedMod = ParentFitting.FittedShip.RigSlot(slotNo)
                    canOffline = False
                Case SlotTypes.Low
                    currentMod = ParentFitting.BaseShip.LowSlot(slotNo)
                    fittedMod = ParentFitting.FittedShip.LowSlot(slotNo)
                Case SlotTypes.Mid
                    currentMod = ParentFitting.BaseShip.MidSlot(slotNo)
                    fittedMod = ParentFitting.FittedShip.MidSlot(slotNo)
                Case SlotTypes.High
                    currentMod = ParentFitting.BaseShip.HiSlot(slotNo)
                    fittedMod = ParentFitting.FittedShip.HiSlot(slotNo)
                Case SlotTypes.Subsystem
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
                If _
                    currentMod.Attributes.ContainsKey(AttributeEnum.ModuleCapacitorNeed) = True Or
                    currentMod.Attributes.ContainsKey(AttributeEnum.ModuleReactivationDelay) Or currentMod.IsTurret Or
                    currentMod.IsLauncher Or currentMod.Attributes.ContainsKey(AttributeEnum.ModuleActivationTime) Then

                    canDeactivate = True
                End If
                If currentMod.Attributes.ContainsKey(AttributeEnum.ModuleHeatDamage) = True Then
                    canOverload = True
                End If

                ' Do new routine for handling module state changes
                Select Case keyMode
                    Case 0 ' No additional keys
                        If _
                            currentstate = ModuleStates.Offline Or currentstate = ModuleStates.Inactive Or
                            currentstate = ModuleStates.Overloaded Then
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
                    If currentMod.ID = ModuleEnum.ItemCommandProcessorI Then
                        If currentstate = ModuleStates.Offline Then
                            ParentFitting.BaseShip.Attributes(AttributeEnum.ShipMaxGangLinks) -= 1
                            ' Check if we need to deactivate a highslot ganglink
                            Dim activeGanglinks As New List(Of Integer)
                            If ParentFitting.BaseShip.HiSlots > 0 Then
                                For slot As Integer = ParentFitting.BaseShip.HiSlots To 1 Step - 1
                                    If ParentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                                        If _
                                            ParentFitting.BaseShip.HiSlot(slot).DatabaseGroup =
                                            ModuleEnum.GroupGangLinks And
                                            ParentFitting.BaseShip.HiSlot(slot).ModuleState = ModuleStates.Active Then
                                            activeGanglinks.Add(slot)
                                        End If
                                    End If
                                Next
                            End If
                            If activeGanglinks.Count > ParentFitting.BaseShip.Attributes(AttributeEnum.ShipMaxGangLinks) _
                                Then
                                ParentFitting.BaseShip.HiSlot(activeGanglinks(0)).ModuleState = ModuleStates.Inactive
                            End If
                        Else
                            ParentFitting.BaseShip.Attributes(AttributeEnum.ShipMaxGangLinks) += 1
                        End If
                    End If
                    Dim oldState As ModuleStates = currentMod.ModuleState
                    currentMod.ModuleState = CType(currentstate, ModuleStates)
                    ' Check for maxGroupActive flag
                    If _
                        (currentstate = ModuleStates.Active Or currentstate = ModuleStates.Overloaded) And
                        currentMod.Attributes.ContainsKey(AttributeEnum.ModuleMaxGroupActive) = True Then
                        If currentMod.DatabaseGroup <> ModuleEnum.GroupGangLinks Then
                            If _
                                ParentFitting.IsModuleGroupLimitExceeded(fittedMod, False,
                                                                         AttributeEnum.ModuleMaxGroupActive) = True Then

                                ' Set the module offline
                                MessageBox.Show(
                                    "You cannot activate the " & currentMod.Name &
                                    " due to a restriction on the maximum number permitted for this group.",
                                    "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                currentMod.ModuleState = oldState
                                Exit Sub
                            End If
                        Else
                            If _
                                ParentFitting.IsModuleGroupLimitExceeded(fittedMod, False,
                                                                         AttributeEnum.ModuleMaxGroupActive) = True Then
                                ' Set the module offline
                                MessageBox.Show(
                                    "You cannot activate the " & currentMod.Name &
                                    " due to a restriction on the maximum number permitted for this group.",
                                    "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                currentMod.ModuleState = oldState
                                Exit Sub
                            Else
                                If _
                                    ParentFitting.CountActiveTypeModules(fittedMod.ID) >
                                    CInt(fittedMod.Attributes(AttributeEnum.ModuleMaxGroupActive)) Then


                                    ' Set the module offline
                                    MessageBox.Show(
                                        "You cannot activate the " & currentMod.Name &
                                        " due to a restriction on the maximum number permitted for this type.",
                                        "Module Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    currentMod.ModuleState = oldState
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                    ' Check for activation of siege mode with remote effects
                    If fittedMod.ID = ModuleEnum.ItemSiegeModuleI Or fittedMod.ID = ModuleEnum.ItemSiegeModuleT2 Then
                        If ParentFitting.FittedShip.RemoteSlotCollection.Count > 0 Then
                            Const Msg As String =
                                      "You have active remote modules and activating Siege Mode will cancel these effects. Do you wish to continue activating Siege Mode?"
                            Dim reply As Integer = MessageBox.Show(Msg, "Confirm Activate Siege Mode",
                                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If reply = DialogResult.No Then
                                fittedMod.ModuleState = oldState
                                Exit Sub
                            Else
                                ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                                Call ResetRemoteEffects()
                            End If
                        End If
                    End If
                    ' Check for activation of triage mode with remote effects
                    If fittedMod.ID = ModuleEnum.ItemTriageModuleI Or fittedMod.ID = ModuleEnum.ItemTriageModuleT2 Then
                        If ParentFitting.FittedShip.RemoteSlotCollection.Count > 0 Then
                            Const Msg As String =
                                      "You have active remote modules and activating Triage Mode will cancel these effects. Do you wish to continue activating Triage Mode?"
                            Dim reply As Integer = MessageBox.Show(Msg, "Confirm Activate Triage Mode",
                                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                            If reply = DialogResult.No Then
                                fittedMod.ModuleState = oldState
                                Exit Sub
                            Else
                                ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                                Call ResetRemoteEffects()
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
                    Dim sep As Integer = selitem.Name.LastIndexOf("_", StringComparison.Ordinal)
                    Dim slotType As Integer = CInt(selitem.Name.Substring(0, sep))
                    Dim slotNo As Integer = CInt(selitem.Name.Substring(sep + 1, 1))
                    Dim loadedModule As New ShipModule
                    Select Case slotType
                        Case SlotTypes.Rig
                            loadedModule = ParentFitting.BaseShip.RigSlot(slotNo)
                        Case SlotTypes.Low
                            loadedModule = ParentFitting.BaseShip.LowSlot(slotNo)
                        Case SlotTypes.Mid
                            loadedModule = ParentFitting.BaseShip.MidSlot(slotNo)
                        Case SlotTypes.High
                            loadedModule = ParentFitting.BaseShip.HiSlot(slotNo)
                        Case SlotTypes.Subsystem
                            loadedModule = ParentFitting.BaseShip.SubSlot(slotNo)
                    End Select
                    Clipboard.SetData("ShipModule", loadedModule.Clone)
                End If
            End If
        End Sub

        Private Sub PasteModuleFromClipboard()
            If Clipboard.ContainsData("ShipModule") Then
                If adtSlots.SelectedNodes.Count > 0 Then
                    Dim updatedCount As Integer = 0
                    Dim selectedNodeCount As Integer = adtSlots.SelectedNodes.Count
                    For index As Integer = 0 To selectedNodeCount - 1
                        Dim selItem As Node = adtSlots.SelectedNodes(index)
                        ' For Each selItem As Node In adtSlots.SelectedNodes
                        Dim newModule As ShipModule = CType(Clipboard.GetData("ShipModule"), ShipModule).Clone
                        Dim sep As Integer = selItem.Name.LastIndexOf("_", StringComparison.Ordinal)
                        'Dim slotType As Integer = CInt(selItem.Name.Substring(0, sep))
                        Dim slotNo As Integer = CInt(selItem.Name.Substring(sep + 1, 1))
                        updatedCount += 1
                        If updatedCount = adtSlots.SelectedNodes.Count Then
                            ParentFitting.AddModule(newModule, slotNo, True, False, Nothing, False, False)
                        Else
                            ParentFitting.AddModule(newModule, slotNo, False, False, Nothing, False, False)
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
                ParentFitting.BaseShip.DroneBayUsed = 0
            End If
        End Sub

        Private Sub ClearCargoBay()
            If ParentFitting.BaseShip IsNot Nothing Then
                ParentFitting.BaseShip.CargoBayItems.Clear()
                ParentFitting.BaseShip.CargoBayUsed = 0
                ParentFitting.BaseShip.CargoBayAdditional = 0
            End If
        End Sub

        Private Sub ClearShipBay()
            If ParentFitting.BaseShip IsNot Nothing Then
                ParentFitting.BaseShip.ShipBayItems.Clear()
                ParentFitting.BaseShip.ShipBayUsed = 0
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

        Private Sub RemoveModules(ByVal sender As Object, ByVal e As EventArgs)
            Dim removedSubsystems As Boolean = False
            adtSlots.BeginUpdate()
            For Each slot As Node In adtSlots.SelectedNodes
                If slot.Text <> "<Empty>" Then
                    Dim sep As Integer = slot.Name.LastIndexOf("_", StringComparison.Ordinal)
                    Dim slotType As Integer = CInt(slot.Name.Substring(0, sep))
                    Dim slotNo As Integer = CInt(slot.Name.Substring(sep + 1, 1))
                    Dim selMod As New ShipModule
                    Select Case slotType
                        Case SlotTypes.Rig
                            selMod = ParentFitting.BaseShip.RigSlot(slotNo)
                        Case SlotTypes.Low
                            selMod = ParentFitting.BaseShip.LowSlot(slotNo)
                        Case SlotTypes.Mid
                            selMod = ParentFitting.BaseShip.MidSlot(slotNo)
                        Case SlotTypes.High
                            selMod = ParentFitting.BaseShip.HiSlot(slotNo)
                        Case SlotTypes.Subsystem
                            selMod = ParentFitting.BaseShip.SubSlot(slotNo)
                    End Select
                    If selMod.LoadedCharge IsNot Nothing Then
                        UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveModule, slotType, slotNo, selMod.Name,
                                                    selMod.LoadedCharge.Name, slotNo, "", ""))
                    Else
                        UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveModule, slotType, slotNo, selMod.Name, "",
                                                    slotNo, "", ""))
                    End If
                    Select Case slotType
                        Case SlotTypes.Rig
                            ParentFitting.BaseShip.RigSlot(slotNo) = Nothing
                        Case SlotTypes.Low
                            ParentFitting.BaseShip.LowSlot(slotNo) = Nothing
                        Case SlotTypes.Mid
                            ParentFitting.BaseShip.MidSlot(slotNo) = Nothing
                        Case SlotTypes.High
                            ParentFitting.BaseShip.HiSlot(slotNo) = Nothing
                        Case SlotTypes.Subsystem
                            ParentFitting.BaseShip.SubSlot(slotNo) = Nothing
                            removedSubsystems = True
                    End Select
                    For Each slotCell As Cell In slot.Cells
                        slotCell.Text = ""
                    Next
                    slot.Text = "<Empty>"
                    SlotTip.SetSuperTooltip(slot, Nothing)
                    slot.Image = CType(My.Resources.ResourceManager.GetObject("Mod01"), Image)
                End If
            Next
            adtSlots.EndUpdate()
            UpdateHistory()
            If removedSubsystems = True Then
                ParentFitting.BaseShip = ParentFitting.BuildSubSystemEffects(ParentFitting.BaseShip)
                UpdateShipSlotLayout()
                ParentFitting.ApplyFitting(BuildType.BuildEverything)
                Call UpdateShipDetails()
            Else
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                Call UpdateShipDetails()
            End If
        End Sub

        Private Sub RemoveModule(ByVal slot As Node, ByVal updateShip As Boolean, ByVal suppressUndo As Boolean)
            ' Get name of the "slot" which has slot type and number
            Dim sep As Integer = slot.Name.LastIndexOf("_", StringComparison.Ordinal)
            If sep > 0 Then ' Denotes that this is a parent node for the slots
                Dim slotType As Integer = CInt(slot.Name.Substring(0, sep))
                Dim slotNo As Integer = CInt(slot.Name.Substring(sep + 1, 1))
                Dim selMod As New ShipModule
                Select Case slotType
                    Case SlotTypes.Rig
                        selMod = ParentFitting.BaseShip.RigSlot(slotNo)
                    Case SlotTypes.Low
                        selMod = ParentFitting.BaseShip.LowSlot(slotNo)
                    Case SlotTypes.Mid
                        selMod = ParentFitting.BaseShip.MidSlot(slotNo)
                    Case SlotTypes.High
                        selMod = ParentFitting.BaseShip.HiSlot(slotNo)
                    Case SlotTypes.Subsystem
                        selMod = ParentFitting.BaseShip.SubSlot(slotNo)
                End Select
                ' Check for command processor usage
                If selMod IsNot Nothing Then
                    If selMod.ID = ModuleEnum.ItemCommandProcessorI Then
                        ParentFitting.BaseShip.Attributes(AttributeEnum.ShipMaxGangLinks) -= 1

                        ' Check if we need to deactivate a highslot ganglink
                        Dim activeGanglinks As New List(Of Integer)
                        If ParentFitting.BaseShip.HiSlots > 0 Then
                            For hSlot As Integer = ParentFitting.BaseShip.HiSlots To 1 Step - 1
                                If ParentFitting.BaseShip.HiSlot(hSlot) IsNot Nothing Then
                                    If _
                                        ParentFitting.BaseShip.HiSlot(hSlot).DatabaseGroup = 316 And
                                        ParentFitting.BaseShip.HiSlot(hSlot).ModuleState = ModuleStates.Active Then
                                        activeGanglinks.Add(hSlot)
                                    End If
                                End If
                            Next
                        End If
                        If activeGanglinks.Count > ParentFitting.BaseShip.Attributes(10063) Then
                            ParentFitting.BaseShip.HiSlot(activeGanglinks(0)).ModuleState = ModuleStates.Inactive
                        End If

                    End If
                    If suppressUndo = False Then
                        If selMod.LoadedCharge IsNot Nothing Then
                            UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveModule, slotType, slotNo, selMod.Name,
                                                        selMod.LoadedCharge.Name, slotNo, "", ""))
                        Else
                            UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveModule, slotType, slotNo, selMod.Name,
                                                        "", slotNo, "", ""))
                        End If
                        UpdateHistory()
                    End If
                    Select Case slotType
                        Case SlotTypes.Rig
                            ParentFitting.BaseShip.RigSlot(slotNo) = Nothing
                        Case SlotTypes.Low
                            ParentFitting.BaseShip.LowSlot(slotNo) = Nothing
                        Case SlotTypes.Mid
                            ParentFitting.BaseShip.MidSlot(slotNo) = Nothing
                        Case SlotTypes.High
                            ParentFitting.BaseShip.HiSlot(slotNo) = Nothing
                        Case SlotTypes.Subsystem
                            ParentFitting.BaseShip.SubSlot(slotNo) = Nothing
                    End Select
                    adtSlots.BeginUpdate()
                    For Each slotCell As Cell In slot.Cells
                        slotCell.Text = ""
                    Next
                    slot.Text = "<Empty>"
                    SlotTip.SetSuperTooltip(slot, Nothing)
                    slot.Image = CType(My.Resources.ResourceManager.GetObject("Mod01"), Image)
                    adtSlots.EndUpdate()
                    If updateShip = True Then
                        If slotType = 16 Then
                            ParentFitting.BaseShip = ParentFitting.BuildSubSystemEffects(ParentFitting.BaseShip)
                            UpdateShipSlotLayout()
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

        Private Sub ctxSlots_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxSlots.Opening
            If CancelSlotMenu = False Then
                ctxSlots.Items.Clear()
                If adtSlots.SelectedNodes.Count > 0 Then
                    If adtSlots.SelectedNodes(0).Nodes.Count = 0 Then
                        Dim currentMod As New ShipModule
                        Dim chargeName As String = ""
                        If adtSlots.SelectedNodes.Count = 1 And adtSlots.SelectedNodes(0).Nodes.Count = 0 Then
                            ' Get the module details
                            'Dim modID As String = CStr(ModuleLists.ModuleListName.Item(adtSlots.SelectedNodes(0).Text))
                            Dim sep As Integer = adtSlots.SelectedNodes(0).Name.LastIndexOf("_",
                                                                                            StringComparison.Ordinal)
                            Dim slotType As Integer = CInt(adtSlots.SelectedNodes(0).Name.Substring(0, sep))
                            Dim slotNo As Integer = CInt(adtSlots.SelectedNodes(0).Name.Substring(sep + 1, 1))
                            Select Case slotType
                                Case SlotTypes.Rig
                                    currentMod = ParentFitting.BaseShip.RigSlot(slotNo)
                                Case SlotTypes.Low
                                    currentMod = ParentFitting.BaseShip.LowSlot(slotNo)
                                Case SlotTypes.Mid
                                    currentMod = ParentFitting.BaseShip.MidSlot(slotNo)
                                Case SlotTypes.High
                                    currentMod = ParentFitting.BaseShip.HiSlot(slotNo)
                                Case SlotTypes.Subsystem
                                    currentMod = ParentFitting.BaseShip.SubSlot(slotNo)
                            End Select
                            If currentMod Is Nothing Then
                                Dim findModuleMenuItem As New ToolStripMenuItem
                                Dim sSep As Integer = adtSlots.SelectedNodes(0).Name.LastIndexOf("_",
                                                                                                 StringComparison.
                                                                                                    Ordinal)
                                findModuleMenuItem.Name = adtSlots.SelectedNodes(0).Name.Substring(0, sSep)
                                findModuleMenuItem.Text = "Find Module To Fit"
                                AddHandler findModuleMenuItem.Click, AddressOf FindModuleToFit
                                ctxSlots.Items.Add(findModuleMenuItem)
                            Else
                                If currentMod.LoadedCharge IsNot Nothing Then
                                    chargeName = currentMod.LoadedCharge.Name
                                End If
                                ' Add the Show Info menu item
                                Dim showInfoMenuItem As New ToolStripMenuItem
                                showInfoMenuItem.Name = currentMod.Name
                                showInfoMenuItem.Text = "Show Info"
                                AddHandler showInfoMenuItem.Click, AddressOf ShowInfo
                                ctxSlots.Items.Add(showInfoMenuItem)
                                ' Add the Show Market Group menu item
                                Dim showMarketGroupMenuItem As New ToolStripMenuItem
                                showMarketGroupMenuItem.Name = currentMod.Name
                                showMarketGroupMenuItem.Text = "Show Module Market Group"
                                AddHandler showMarketGroupMenuItem.Click, AddressOf ShowModuleMarketGroup
                                ctxSlots.Items.Add(showMarketGroupMenuItem)
                                ' Add the Show Meta Variations menu item
                                Dim showMetaVariationsMenuItem As New ToolStripMenuItem
                                showMetaVariationsMenuItem.Name = currentMod.Name
                                showMetaVariationsMenuItem.Text = "Show Meta Variations"
                                AddHandler showMetaVariationsMenuItem.Click, AddressOf ShowMetaVariations
                                ctxSlots.Items.Add(showMetaVariationsMenuItem)
                                ' Add the Add to Favourites menu item
                                Dim addToFavourtiesMenuItem As New ToolStripMenuItem
                                addToFavourtiesMenuItem.Name = currentMod.Name
                                addToFavourtiesMenuItem.Text = "Add To Favourites"
                                If PluginSettings.HQFSettings.Favourites.Contains(currentMod.Name) = True Then
                                    addToFavourtiesMenuItem.Enabled = False
                                Else
                                    addToFavourtiesMenuItem.Enabled = True
                                End If
                                AddHandler addToFavourtiesMenuItem.Click, AddressOf AddModuleToFavourites
                                ctxSlots.Items.Add(addToFavourtiesMenuItem)
                                ' Check for Relevant Skills in Modules/Charges
                                Dim relModuleSkills, relChargeSkills As New ArrayList
                                Dim affects(3) As String
                                For Each affect As String In currentMod.Affects
                                    If affect.Contains(";Skill;") = True Then
                                        affects = affect.Split((";").ToCharArray)
                                        If relModuleSkills.Contains(affects(0)) = False Then
                                            relModuleSkills.Add(affects(0))
                                        End If
                                    End If
                                    If affect.Contains(";Ship Bonus;") = True Then
                                        affects = affect.Split((";").ToCharArray)
                                        If ParentFitting.ShipName = affects(0) Then
                                            If relModuleSkills.Contains(affects(3)) = False Then
                                                relModuleSkills.Add(affects(3))
                                            End If
                                        End If
                                    End If
                                    If affect.Contains(";Subsystem;") = True Then
                                        affects = affect.Split((";").ToCharArray)
                                        If relModuleSkills.Contains(affects(3)) = False Then
                                            relModuleSkills.Add(affects(3))
                                        End If
                                    End If
                                Next
                                relModuleSkills.Sort()
                                If currentMod.LoadedCharge IsNot Nothing Then
                                    For Each affect As String In currentMod.LoadedCharge.Affects
                                        If affect.Contains(";Skill;") = True Then
                                            affects = affect.Split((";").ToCharArray)
                                            If relChargeSkills.Contains(affects(0)) = False Then
                                                relChargeSkills.Add(affects(0))
                                            End If
                                        End If
                                        If affect.Contains(";Ship Bonus;") = True Then
                                            affects = affect.Split((";").ToCharArray)
                                            If ParentFitting.ShipName = affects(0) Then
                                                If relChargeSkills.Contains(affects(3)) = False Then
                                                    relChargeSkills.Add(affects(3))
                                                End If
                                            End If
                                        End If
                                        If affect.Contains(";Subsystem;") = True Then
                                            affects = affect.Split((";").ToCharArray)
                                            If relChargeSkills.Contains(affects(3)) = False Then
                                                relChargeSkills.Add(affects(3))
                                            End If
                                        End If
                                    Next
                                End If
                                relChargeSkills.Sort()
                                If relModuleSkills.Count > 0 Or relChargeSkills.Count > 0 Then
                                    ' Add the Main menu item
                                    Dim alterRelevantSkills As New ToolStripMenuItem
                                    alterRelevantSkills.Name = currentMod.Name
                                    alterRelevantSkills.Text = "Alter Relevant Skills"
                                    For Each relSkill As String In relModuleSkills
                                        Dim newRelSkill As New ToolStripMenuItem
                                        newRelSkill.Name = relSkill
                                        newRelSkill.Text = relSkill
                                        Dim pilotLevel As Integer = 0
                                        If _
                                            FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).
                                                SkillSet.ContainsKey(relSkill) Then
                                            pilotLevel =
                                                FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).
                                                    SkillSet(relSkill).Level
                                        Else
                                            MessageBox.Show(
                                                "There is a mis-match of roles for the " & ParentFitting.BaseShip.Name &
                                                ". Please report this to the EveHQ Developers.", "Ship Role Error",
                                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        End If
                                        newRelSkill.Image =
                                            CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString),
                                                  Image)
                                        For skillLevel As Integer = 0 To 5
                                            Dim newRelSkillLevel As New ToolStripMenuItem
                                            newRelSkillLevel.Name = relSkill & skillLevel.ToString
                                            newRelSkillLevel.Text = "Level " & skillLevel.ToString
                                            If skillLevel = pilotLevel Then
                                                newRelSkillLevel.Checked = True
                                            End If
                                            AddHandler newRelSkillLevel.Click, AddressOf SetPilotSkillLevel
                                            newRelSkill.DropDownItems.Add(newRelSkillLevel)
                                        Next
                                        newRelSkill.DropDownItems.Add("-")
                                        Dim defaultLevel As Integer = 0
                                        If _
                                            HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills _
                                                .ContainsKey(relSkill) = True Then
                                            defaultLevel =
                                                HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).
                                                    PilotSkills(relSkill).Level
                                        Else
                                        End If
                                        Dim newRelSkillDefault As New ToolStripMenuItem
                                        newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                                        newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                                        AddHandler newRelSkillDefault.Click, AddressOf SetPilotSkillLevel
                                        newRelSkill.DropDownItems.Add(newRelSkillDefault)
                                        alterRelevantSkills.DropDownItems.Add(newRelSkill)
                                    Next
                                    If alterRelevantSkills.DropDownItems.Count > 0 And relChargeSkills.Count > 0 Then
                                        alterRelevantSkills.DropDownItems.Add("-")
                                    End If
                                    For Each relSkill As String In relChargeSkills
                                        Dim newRelSkill As New ToolStripMenuItem
                                        newRelSkill.Name = relSkill
                                        newRelSkill.Text = relSkill
                                        Dim pilotLevel As Integer = 0
                                        If _
                                            FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).
                                                SkillSet.ContainsKey(relSkill) Then
                                            pilotLevel =
                                                FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).
                                                    SkillSet(relSkill).Level
                                        Else
                                            MessageBox.Show(
                                                "There is a mis-match of roles for the " & ParentFitting.BaseShip.Name &
                                                ". Please report this to the EveHQ Developers.", "Ship Role Error",
                                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        End If
                                        newRelSkill.Image =
                                            CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString),
                                                  Image)
                                        For skillLevel As Integer = 0 To 5
                                            Dim newRelSkillLevel As New ToolStripMenuItem
                                            newRelSkillLevel.Name = relSkill & skillLevel.ToString
                                            newRelSkillLevel.Text = "Level " & skillLevel.ToString
                                            If skillLevel = pilotLevel Then
                                                newRelSkillLevel.Checked = True
                                            End If
                                            AddHandler newRelSkillLevel.Click, AddressOf SetPilotSkillLevel
                                            newRelSkill.DropDownItems.Add(newRelSkillLevel)
                                        Next
                                        newRelSkill.DropDownItems.Add("-")
                                        Dim defaultLevel As Integer = 0
                                        If _
                                            HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills _
                                                .ContainsKey(relSkill) = True Then
                                            defaultLevel =
                                                HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).
                                                    PilotSkills(relSkill).Level
                                        End If
                                        Dim newRelSkillDefault As New ToolStripMenuItem
                                        newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                                        newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                                        AddHandler newRelSkillDefault.Click, AddressOf SetPilotSkillLevel
                                        newRelSkill.DropDownItems.Add(newRelSkillDefault)
                                        alterRelevantSkills.DropDownItems.Add(newRelSkill)
                                    Next
                                    ctxSlots.Items.Add(alterRelevantSkills)
                                End If
                                ' Add the Remove Module item
                                Dim removeModuleMenuItem As New ToolStripMenuItem
                                removeModuleMenuItem.Name = currentMod.Name
                                removeModuleMenuItem.Text = "Remove " & currentMod.Name
                                removeModuleMenuItem.ShortcutKeys = Keys.Delete
                                AddHandler removeModuleMenuItem.Click, AddressOf RemoveModules
                                ctxSlots.Items.Add(removeModuleMenuItem)
                                ' Add the Status menu item
                                If slotType <> SlotTypes.Rig And slotType <> SlotTypes.Subsystem Then
                                    Dim canDeactivate As Boolean = False
                                    Dim canOverload As Boolean = False
                                    ctxSlots.Items.Add("-")
                                    Dim statusMenuItem As New ToolStripMenuItem
                                    statusMenuItem.Name = adtSlots.SelectedNodes(0).Name
                                    statusMenuItem.Text = "Set Module Status"
                                    ' Check for activation cost
                                    If _
                                        currentMod.Attributes.ContainsKey(AttributeEnum.ModuleCapacitorNeed) = True Or
                                        currentMod.Attributes.ContainsKey(AttributeEnum.ModuleReactivationDelay) Or
                                        currentMod.IsTurret Or currentMod.IsLauncher Or
                                        currentMod.Attributes.ContainsKey(AttributeEnum.ModuleActivationTime) Then

                                        canDeactivate = True
                                    End If
                                    If currentMod.Attributes.ContainsKey(AttributeEnum.ModuleHeatDamage) = True Then
                                        canOverload = True
                                    End If
                                    Dim offlineStatusMenu As New ToolStripMenuItem
                                    offlineStatusMenu.Name = adtSlots.SelectedNodes(0).Name
                                    offlineStatusMenu.Text = "Offline"
                                    offlineStatusMenu.Tag = currentMod
                                    AddHandler offlineStatusMenu.Click, AddressOf SetModuleOffline
                                    statusMenuItem.DropDownItems.Add(offlineStatusMenu)
                                    Dim inactiveStatusMenu As New ToolStripMenuItem
                                    If canDeactivate = True Then
                                        inactiveStatusMenu.Name = adtSlots.SelectedNodes(0).Name
                                        inactiveStatusMenu.Text = "Inactive"
                                        inactiveStatusMenu.Tag = currentMod
                                        AddHandler inactiveStatusMenu.Click, AddressOf SetModuleInactive
                                        statusMenuItem.DropDownItems.Add(inactiveStatusMenu)
                                    End If
                                    Dim activeStatusMenu As New ToolStripMenuItem
                                    activeStatusMenu.Name = adtSlots.SelectedNodes(0).Name
                                    activeStatusMenu.Text = "Active"
                                    activeStatusMenu.Tag = currentMod
                                    AddHandler activeStatusMenu.Click, AddressOf SetModuleActive
                                    statusMenuItem.DropDownItems.Add(activeStatusMenu)
                                    Dim overloadStatusMenu As New ToolStripMenuItem
                                    If canOverload = True Then
                                        overloadStatusMenu.Name = adtSlots.SelectedNodes(0).Name
                                        overloadStatusMenu.Text = "Overload"
                                        overloadStatusMenu.Tag = currentMod
                                        AddHandler overloadStatusMenu.Click, AddressOf SetModuleOverload
                                        statusMenuItem.DropDownItems.Add(overloadStatusMenu)
                                    End If
                                    Select Case currentMod.ModuleState
                                        Case ModuleStates.Offline
                                            offlineStatusMenu.Enabled = False
                                        Case ModuleStates.Inactive
                                            inactiveStatusMenu.Enabled = False
                                        Case ModuleStates.Active
                                            activeStatusMenu.Enabled = False
                                        Case ModuleStates.Overloaded
                                            overloadStatusMenu.Enabled = False
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
                                    'Dim modID As String = CStr(ModuleLists.ModuleListName.Item(slot.Text))
                                    Dim sep As Integer = slot.Name.LastIndexOf("_", StringComparison.Ordinal)
                                    Dim slotType As Integer = CInt(slot.Name.Substring(0, sep))
                                    Dim slotNo As Integer = CInt(slot.Name.Substring(sep + 1, 1))
                                    Select Case slotType
                                        Case SlotTypes.Rig
                                            currentMod = ParentFitting.BaseShip.RigSlot(slotNo)
                                        Case SlotTypes.Low
                                            currentMod = ParentFitting.BaseShip.LowSlot(slotNo)
                                        Case SlotTypes.Mid
                                            currentMod = ParentFitting.BaseShip.MidSlot(slotNo)
                                        Case SlotTypes.High
                                            currentMod = ParentFitting.BaseShip.HiSlot(slotNo)
                                        Case SlotTypes.Subsystem
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
                                Dim removeModuleMenuItem As New ToolStripMenuItem
                                removeModuleMenuItem.Name = "RemoveMods"
                                removeModuleMenuItem.Text = "Remove Modules"
                                removeModuleMenuItem.ShortcutKeys = Keys.Delete
                                AddHandler removeModuleMenuItem.Click, AddressOf RemoveModules
                                ctxSlots.Items.Add(removeModuleMenuItem)
                            End If
                        End If

                        ' Calculate all the charge information
                        ' Check if we have the same collection of modules and therefore can accept the same charges
                        Dim showCharges As Boolean = True
                        Dim ammoAnalysis As ShipModule = currentMod
                        Dim cMod As ShipModule
                        If adtSlots.SelectedNodes.Count > 1 Then
                            Dim marketGroup As Integer = currentMod.MarketGroup
                            For Each selNodes As Node In adtSlots.SelectedNodes
                                If selNodes.Text <> "<Empty>" Then
                                    cMod = ModuleLists.ModuleList(ModuleLists.ModuleListName.Item(selNodes.Text))
                                    If cMod.MarketGroup <> marketGroup Then
                                        ammoAnalysis = Nothing
                                        showCharges = False
                                        Exit For
                                    Else
                                        ammoAnalysis = cMod
                                    End If
                                End If
                            Next
                        End If

                        If showCharges = True And currentMod IsNot Nothing Then

                            ' Get the charge group and item data
                            Dim chargeGroups As New ArrayList
                            Dim chargeGroupData() As String
                            Dim chargeItems As New SortedList
                            Dim groupName As String = "Charge"
                            For Each chargeGroup As String In Charges.ChargeGroups.Values
                                chargeGroupData = chargeGroup.Split("_".ToCharArray)
                                If currentMod.Charges.Contains(CInt(chargeGroupData(1))) = True Then
                                    If Market.MarketGroupList.ContainsKey(chargeGroupData(0)) = True Then
                                        Select Case Market.MarketGroupList.Item(chargeGroupData(0))
                                            Case "Small", "Medium", "Large", "Extra Large"
                                                Dim pathLine As String = Market.MarketGroupPath(chargeGroupData(0))
                                                Dim paths() As String = pathLine.Split("\".ToCharArray)
                                                groupName = paths(paths.GetUpperBound(0) - 1)
                                            Case Else
                                                groupName = Market.MarketGroupList.Item(chargeGroupData(0))
                                        End Select
                                    End If
                                    If chargeGroups.Contains(groupName) = False Then
                                        chargeGroups.Add(groupName)
                                    End If
                                    If _
                                        currentMod.IsTurret Or
                                        currentMod.DatabaseGroup = ModuleEnum.GroupFueledShieldBoosters Then
                                        If _
                                            currentMod.ChargeSize = CInt(chargeGroupData(3)) And
                                            chargeItems.ContainsKey(chargeGroupData(2)) = False Then
                                            chargeItems.Add(chargeGroupData(2), groupName)
                                        End If
                                    ElseIf chargeItems.ContainsKey(chargeGroupData(2)) = False Then
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
                                    Dim showChargeInfo As New ToolStripMenuItem
                                    showChargeInfo.Name = chargeName
                                    showChargeInfo.Text = "Show Charge Info"
                                    AddHandler showChargeInfo.Click, AddressOf Me.ShowChargeInfo
                                    ctxSlots.Items.Add(showChargeInfo)
                                    Dim removeCharge As New ToolStripMenuItem
                                    removeCharge.Name = currentMod.Name
                                    If adtSlots.SelectedNodes.Count > 1 Then
                                        removeCharge.Text = "Remove Charges"
                                    Else
                                        removeCharge.Text = "Remove " & chargeName
                                    End If
                                    AddHandler removeCharge.Click, AddressOf RemoveChargeFromModule
                                    ctxSlots.Items.Add(removeCharge)
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
                                            Dim chargeMod As ShipModule =
                                                    ModuleLists.ModuleList(ModuleLists.ModuleListName(charge))
                                            If chargeMod.Volume <= currentMod.Capacity Then
                                                newCharge.Name = CStr(ModuleLists.ModuleListName(charge))
                                                newCharge.Text = charge
                                                AddHandler newCharge.Click, AddressOf LoadChargeIntoModule
                                                newGroup.DropDownItems.Add(newCharge)
                                            End If
                                        End If
                                    Next
                                    ctxSlots.Items.Add(newGroup)
                                Next
                            End If
                            ' Add an "Ammo Analysis" option - the old Gunnery tool feature
                            If ammoAnalysis.IsTurret Or ammoAnalysis.IsLauncher Then
                                ctxSlots.Items.Add("-")
                                Dim ammoInfo As New ToolStripMenuItem
                                ammoInfo.Name = "AmmoInfo"
                                ammoInfo.Text = "Ammo Analysis"
                                AddHandler ammoInfo.Click, AddressOf AnalyseAmmo
                                ctxSlots.Items.Add(ammoInfo)
                            End If
                        End If
                        ' Add the configure display option
                        Dim slotInfo As New ToolStripMenuItem
                        slotInfo.Name = "SlotConfiguration"
                        slotInfo.Text = "Configure Slot Display"
                        AddHandler slotInfo.Click, AddressOf ConfigureSlotColumns
                        ctxSlots.Items.Add("-")
                        ctxSlots.Items.Add(slotInfo)
                        ' Add the Requisition option
                        Dim reqInfo As New ToolStripMenuItem
                        reqInfo.Name = "AddToRequisition"
                        reqInfo.Text = "Add to Requisition"
                        AddHandler reqInfo.Click, AddressOf AddModuleToRequisitions
                        ctxSlots.Items.Add("-")
                        ctxSlots.Items.Add(reqInfo)
                        ' Add the Copy and Paste options
                        ctxSlots.Items.Add("-")
                        Dim copyOption As New ToolStripMenuItem
                        copyOption.Name = "CopyModule"
                        If adtSlots.SelectedNodes.Count > 1 Then
                            copyOption.Text = "<Copy Not Available>"
                            copyOption.Enabled = False
                        Else
                            If adtSlots.SelectedNodes(0).Text <> "<Empty>" Then
                                copyOption.Text = "Copy " & adtSlots.SelectedNodes(0).Text
                                copyOption.Enabled = True
                            Else
                                copyOption.Text = "<Copy Not Available>"
                                copyOption.Enabled = False
                            End If
                        End If
                        AddHandler copyOption.Click, AddressOf CopyModule
                        ctxSlots.Items.Add(copyOption)
                        Dim pasteOption As New ToolStripMenuItem
                        pasteOption.Name = "PasteModule"
                        If Clipboard.ContainsData("ShipModule") = True Then
                            pasteOption.Text = "Paste " & CType(Clipboard.GetData("ShipModule"), ShipModule).Name
                            pasteOption.Enabled = True
                        Else
                            pasteOption.Text = "<Paste Not Available>"
                            pasteOption.Enabled = False
                        End If
                        AddHandler pasteOption.Click, AddressOf PasteModule
                        ctxSlots.Items.Add(pasteOption)
                    Else
                        ' Activate the overload rack menu
                        Dim activeRack As Node = adtSlots.SelectedNodes(0)
                        Dim overloadRackMenuItem As New ToolStripMenuItem
                        overloadRackMenuItem.Name = activeRack.Name
                        If activeRack.Text.StartsWith("High Slots") Then
                            overloadRackMenuItem.Text = "Toggle Overload: High Slots"
                            AddHandler overloadRackMenuItem.Click, AddressOf OverloadRack
                            ctxSlots.Items.Add(overloadRackMenuItem)
                        Else
                            Select Case activeRack.Text
                                Case "Mid Slots", "Low Slots"
                                    overloadRackMenuItem.Text = "Toggle Overload: " & activeRack.Text
                                    AddHandler overloadRackMenuItem.Click, AddressOf OverloadRack
                                    ctxSlots.Items.Add(overloadRackMenuItem)
                                Case Else
                                    e.Cancel = True
                            End Select
                        End If
                    End If
                Else
                    e.Cancel = True
                End If
            End If

            ' Cancel the menu if there is nothing to display
            If ctxSlots.Items.Count = 0 Then
                e.Cancel = True
            End If
        End Sub

        Private Sub OverloadRack(sender As Object, e As EventArgs)
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            'Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
            Dim slotType As Integer
            ' Determine slot type
            Select Case menuItem.Text.TrimStart("Toggle Overload: ".ToCharArray)
                Case "High Slots"
                    slotType = SlotTypes.High
                Case "Mid Slots"
                    slotType = SlotTypes.Mid
                Case "Low Slots"
                    slotType = SlotTypes.Low
                Case Else
                    slotType = 0
            End Select

            ' Check status of all slots - if none are overloaded, make all overloaded, if any are overloaded, make none overloaded
            Dim isOverloaded As Boolean = False
            Select Case slotType
                Case SlotTypes.High
                    For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                        If ParentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                            If ParentFitting.BaseShip.HiSlot(slot).ModuleState = ModuleStates.Overloaded Then
                                isOverloaded = True
                                Exit For
                            End If
                        End If
                    Next
                Case SlotTypes.Mid
                    For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                        If ParentFitting.BaseShip.MidSlot(slot) IsNot Nothing Then
                            If ParentFitting.BaseShip.MidSlot(slot).ModuleState = ModuleStates.Overloaded Then
                                isOverloaded = True
                                Exit For
                            End If
                        End If
                    Next
                Case SlotTypes.Low
                    For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                        If ParentFitting.BaseShip.LowSlot(slot) IsNot Nothing Then
                            If ParentFitting.BaseShip.LowSlot(slot).ModuleState = ModuleStates.Overloaded Then
                                isOverloaded = True
                                Exit For
                            End If
                        End If
                    Next
            End Select

            If isOverloaded = False Then
                ' Set all modules to overloaded
                Select Case slotType
                    Case SlotTypes.High
                        For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                            If ParentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                                If ParentFitting.BaseShip.HiSlot(slot).Attributes.ContainsKey(1211) = True Then
                                    SetSingleModuleOverloaded(ParentFitting.BaseShip.HiSlot(slot))
                                End If
                            End If
                        Next
                    Case SlotTypes.Mid
                        For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                            If ParentFitting.BaseShip.MidSlot(slot) IsNot Nothing Then
                                If ParentFitting.BaseShip.MidSlot(slot).Attributes.ContainsKey(1211) = True Then
                                    SetSingleModuleOverloaded(ParentFitting.BaseShip.MidSlot(slot))
                                End If
                            End If
                        Next
                    Case SlotTypes.Low
                        For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                            If ParentFitting.BaseShip.LowSlot(slot) IsNot Nothing Then
                                If ParentFitting.BaseShip.LowSlot(slot).Attributes.ContainsKey(1211) = True Then
                                    SetSingleModuleOverloaded(ParentFitting.BaseShip.LowSlot(slot))
                                End If
                            End If
                        Next
                End Select
            Else
                ' Set all modules to active
                Select Case slotType
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

        Private Sub FindModuleToFit(ByVal sender As Object, ByVal e As EventArgs)
            Dim selectedSlot As Node = adtSlots.SelectedNodes(0)
            Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
            Dim modData As New ArrayList
            modData.Add(slotInfo(0))
            modData.Add(ParentFitting.FittedShip.CPU - ParentFitting.FittedShip.CPUUsed)
            modData.Add(ParentFitting.FittedShip.PG - ParentFitting.FittedShip.PGUsed)
            modData.Add(ParentFitting.FittedShip.Calibration - ParentFitting.FittedShip.CalibrationUsed)
            modData.Add(ParentFitting.FittedShip.LauncherSlots - ParentFitting.FittedShip.LauncherSlotsUsed)
            modData.Add(ParentFitting.FittedShip.TurretSlots - ParentFitting.FittedShip.TurretSlotsUsed)
            HQFEvents.StartFindModule = modData
        End Sub

        Private Sub ShowInfo(ByVal sender As Object, ByVal e As EventArgs)
            Dim selectedSlot As Node = adtSlots.SelectedNodes(0)
            Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
            Dim sModule As New ShipModule
            Select Case CInt(slotInfo(0))
                Case SlotTypes.Rig
                    sModule = ParentFitting.FittedShip.RigSlot(CInt(slotInfo(1)))
                Case SlotTypes.Low
                    sModule = ParentFitting.FittedShip.LowSlot(CInt(slotInfo(1)))
                Case SlotTypes.Mid
                    sModule = ParentFitting.FittedShip.MidSlot(CInt(slotInfo(1)))
                Case SlotTypes.High
                    sModule = ParentFitting.FittedShip.HiSlot(CInt(slotInfo(1)))
                Case SlotTypes.Subsystem
                    sModule = ParentFitting.FittedShip.SubSlot(CInt(slotInfo(1)))
            End Select
            Dim showInfo As New FrmShowInfo
            Dim hPilot As EveHQPilot
            If _currentInfo IsNot Nothing Then
                hPilot = HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString)
            Else
                If HQ.Settings.StartupPilot <> "" Then
                    hPilot = HQ.Settings.Pilots(HQ.Settings.StartupPilot)
                Else
                    hPilot = HQ.Settings.Pilots.Values(0)
                End If
            End If
            showInfo.ShowItemDetails(sModule, hPilot)
        End Sub

        Private Sub ShowChargeInfo(ByVal sender As Object, ByVal e As EventArgs)
            Dim selectedSlot As Node = adtSlots.SelectedNodes(0)
            Dim slotInfo() As String = selectedSlot.Name.Split("_".ToCharArray)
            Dim sModule As New ShipModule
            Select Case CInt(slotInfo(0))
                Case SlotTypes.Rig
                    sModule = ParentFitting.FittedShip.RigSlot(CInt(slotInfo(1))).LoadedCharge
                Case SlotTypes.Low
                    sModule = ParentFitting.FittedShip.LowSlot(CInt(slotInfo(1))).LoadedCharge
                Case SlotTypes.Mid
                    sModule = ParentFitting.FittedShip.MidSlot(CInt(slotInfo(1))).LoadedCharge
                Case SlotTypes.High
                    sModule = ParentFitting.FittedShip.HiSlot(CInt(slotInfo(1))).LoadedCharge
                Case SlotTypes.Subsystem
                    sModule = ParentFitting.FittedShip.SubSlot(CInt(slotInfo(1))).LoadedCharge
            End Select
            Dim showInfo As New FrmShowInfo
            Dim hPilot As EveHQPilot
            If _currentInfo IsNot Nothing Then
                hPilot = HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString)
            Else
                If HQ.Settings.StartupPilot <> "" Then
                    hPilot = HQ.Settings.Pilots(HQ.Settings.StartupPilot)
                Else
                    hPilot = HQ.Settings.Pilots.Values(0)
                End If
            End If

            showInfo.ShowItemDetails(sModule, hPilot)
        End Sub

        Private Sub AnalyseAmmo(ByVal sender As Object, ByVal e As EventArgs)
            ' Display the ammo types available by this module
            Using ammoAnalysis As New FrmGunnery
                ammoAnalysis.CurrentFit = ParentFitting
                ammoAnalysis.CurrentPilot = FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString)
                ammoAnalysis.CurrentSlot = adtSlots.SelectedNodes(0).Name
                ammoAnalysis.ShowDialog()
            End Using
        End Sub

        Private Sub ConfigureSlotColumns(ByVal sender As Object, ByVal e As EventArgs)
            ' Open options form
            Using mySettings As New FrmHQFSettings
                mySettings.Tag = "nodeSlotFormat"
                mySettings.ShowDialog()
            End Using
        End Sub

        Private Sub AddModuleToRequisitions(ByVal sender As Object, ByVal e As EventArgs)
            ' Set up a new Sortedlist to store the required items
            Dim orders As New SortedList(Of String, Integer)
            ' Add the current ship
            For Each selNode As Node In adtSlots.SelectedNodes
                If orders.ContainsKey(selNode.Text) = False Then
                    orders.Add(selNode.Text, 1)
                Else
                    orders(selNode.Text) += 1
                End If
            Next
            ' Setup the Requisition form for HQF and open it
            Using newReq As New FrmAddRequisition("HQF", orders)
                newReq.ShowDialog()
            End Using
        End Sub

        Private Sub ShowModuleMarketGroup(ByVal sender As Object, ByVal e As EventArgs)
            Dim showMarketMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim moduleName As String = showMarketMenu.Name
            Dim moduleID As Integer = ModuleLists.ModuleListName(moduleName)
            Dim cModule As ShipModule = ModuleLists.ModuleList.Item(moduleID)
            Dim pathLine As String = Market.MarketGroupPath(CStr(cModule.MarketGroup))
            If pathLine IsNot Nothing Then
                HQFEvents.DisplayedMarketGroup = pathLine
            Else
                MessageBox.Show(
                    "The market group for this item could not be found in the database. Please check the data source to ensure it exists and has a path to it from the root.")
            End If
        End Sub

        Private Sub ShowMetaVariations(ByVal sender As Object, ByVal e As EventArgs)
            Dim showMarketMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim moduleName As String = showMarketMenu.Name
            Dim moduleID As Integer = ModuleLists.ModuleListName(moduleName)
            Dim cModule As ShipModule
            If TypeOf ctxSlots.Tag Is ShipModule Then
                cModule = CType(ctxSlots.Tag, ShipModule)
            Else
                cModule = ModuleLists.ModuleList.Item(moduleID)
            End If
            Using newComparison As New FrmMetaVariations(ParentFitting, cModule)
                newComparison.Size = PluginSettings.HQFSettings.MetaVariationsFormSize
                newComparison.ShowDialog()
            End Using
        End Sub

        Private Sub AddModuleToFavourites(ByVal sender As Object, ByVal e As EventArgs)
            Dim showMarketMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim moduleName As String = showMarketMenu.Name
            Dim moduleID As Integer = ModuleLists.ModuleListName(moduleName)
            Dim cModule As ShipModule = ModuleLists.ModuleList.Item(moduleID)
            If PluginSettings.HQFSettings.Favourites.Contains(cModule.Name) = False Then
                PluginSettings.HQFSettings.Favourites.Add(cModule.Name)
                HQFEvents.StartUpdateModuleList = True
            End If
        End Sub

        Private Sub pbShipInfo_MouseHover(ByVal sender As Object, ByVal e As EventArgs) Handles pbShipInfo.MouseHover
            Dim traits As String = FrmShowInfo.ComposeTraits(ParentFitting.BaseShip.ID)

            Dim charlist As List(Of Char) = New List(Of Char)
            Dim skip As Boolean
            ' remove any HTML markup/format tags
            For Each letter As Char In traits
                If letter = "<" Then
                    skip = True
                ElseIf letter = ">" Then
                    skip = False
                ElseIf skip = False Then
                    charlist.Add(letter)
                End If
            Next
            ToolTip1.SetToolTip(pbShipInfo, SquishText(charlist.ToArray()))
        End Sub

        Private Sub SetPilotSkillLevel(ByVal sender As Object, ByVal e As EventArgs)
            Dim mnuPilotLevel As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim hPilot As FittingPilot = FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString)
            Dim pilotSkill As FittingSkill = hPilot.SkillSet(mnuPilotLevel.Name.Substring(0,
                                                                                          mnuPilotLevel.Name.Length - 1))
            Dim level As Integer = CInt(mnuPilotLevel.Name.Substring(mnuPilotLevel.Name.Length - 1))
            If level <> pilotSkill.Level Then
                pilotSkill.Level = level
                ParentFitting.ApplyFitting(BuildType.BuildEverything)
            End If
            ' Trigger an update of all open ship fittings!
            HQFEvents.StartUpdateShipInfo = hPilot.PilotName
        End Sub

        Private Function SquishText(ByVal textToSquish As String) As String
            Dim words() As String = textToSquish.Split(" ".ToCharArray)
            Dim newText As New StringBuilder
            Dim charCount As Integer = 0
            For c As Integer = 0 To words.Length - 1
                If words(c).Contains(vbCrLf) Then
                    If charCount + words(c).IndexOf(vbCrLf, StringComparison.Ordinal) > MaxTooltipWidth Then
                        newText.AppendLine()
                    End If
                    charCount = 0
                ElseIf charCount + words(c).Length > MaxTooltipWidth Then
                    newText.AppendLine()
                    charCount = 0
                End If
                newText.Append(words(c) & " ")
                ' if no CRLF is found LastIndexOf(vbCrLf) returns -1 which makes charCount what it should be in that case
                charCount += words(c).Length - words(c).LastIndexOf(vbCrLf, StringComparison.Ordinal)
            Next

            Return newText.ToString
        End Function

        Private Sub ExpandableSplitter1_SplitterMoved(ByVal sender As Object, ByVal e As SplitterEventArgs) _
            Handles ExpandableSplitter1.SplitterMoved
            PluginSettings.HQFSettings.StorageBayHeight = tcStorage.Height
        End Sub

        Private Sub CopyModule(ByVal sender As Object, ByVal e As EventArgs)
            Call CopyModulesToClipboard()
        End Sub

        Private Sub PasteModule(ByVal sender As Object, ByVal e As EventArgs)
            Call PasteModuleFromClipboard()
        End Sub

        Private Sub btnAutoSize_Click(sender As Object, e As EventArgs) Handles btnAutoSize.Click
            Call AutoSizeAllColumns()
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

        Private Sub SetModuleOffline(ByVal sender As Object, ByVal e As EventArgs)
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
            sModule.ModuleState = ModuleStates.Offline
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Sub

        Private Sub SetModuleInactive(ByVal sender As Object, ByVal e As EventArgs)
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
            sModule.ModuleState = ModuleStates.Inactive
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Sub

        Private Sub SetModuleActive(ByVal sender As Object, ByVal e As EventArgs)
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
            Call SetSingleModuleActive(sModule)
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Sub

        Private Sub SetModuleOverload(ByVal sender As Object, ByVal e As EventArgs)
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim sModule As ShipModule = CType(menuItem.Tag, ShipModule)
            Call SetSingleModuleOverloaded(sModule)
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Sub

        Private Sub SetSingleModuleActive(sModule As ShipModule)

            Dim fModule As New ShipModule
            Select Case sModule.SlotType
                Case SlotTypes.Rig

                    fModule = ParentFitting.FittedShip.RigSlot(sModule.SlotNo)
                Case SlotTypes.Low

                    fModule = ParentFitting.FittedShip.LowSlot(sModule.SlotNo)
                Case SlotTypes.Mid

                    fModule = ParentFitting.FittedShip.MidSlot(sModule.SlotNo)
                Case SlotTypes.High
                    fModule = ParentFitting.FittedShip.HiSlot(sModule.SlotNo)
                Case SlotTypes.Subsystem
                    fModule = ParentFitting.FittedShip.SubSlot(sModule.SlotNo)
            End Select
            Dim oldState As ModuleStates = sModule.ModuleState
            sModule.ModuleState = ModuleStates.Active
            ' Check for maxGroupActive flag
            If sModule.Attributes.ContainsKey(AttributeEnum.ModuleMaxGroupActive) = True Then
                If sModule.DatabaseGroup <> ModuleEnum.GroupGangLinks Then
                    If _
                        ParentFitting.IsModuleGroupLimitExceeded(sModule, False, AttributeEnum.ModuleMaxGroupActive) =
                        True Then
                        ' Set the module offline
                        MessageBox.Show(
                            "You cannot activate the " & sModule.Name &
                            " due to a restriction on the maximum number permitted for this group.",
                            "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        sModule.ModuleState = oldState
                        Exit Sub
                    End If
                Else
                    If _
                        ParentFitting.IsModuleGroupLimitExceeded(sModule, False, AttributeEnum.ModuleMaxGroupActive) =
                        True Then
                        ' Set the module offline
                        MessageBox.Show(
                            "You cannot activate the " & sModule.Name &
                            " due to a restriction on the maximum number permitted for this group.",
                            "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        sModule.ModuleState = oldState
                        Exit Sub
                    Else
                        If _
                            ParentFitting.CountActiveTypeModules(sModule.ID) >
                            CInt(fModule.Attributes(AttributeEnum.ModuleMaxGroupActive)) Then
                            ' Set the module offline
                            MessageBox.Show(
                                "You cannot activate the " & sModule.Name &
                                " due to a restriction on the maximum number permitted for this type.",
                                "Module Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            sModule.ModuleState = oldState
                            Exit Sub
                        End If
                    End If
                End If
            End If
            ' Check for activation of siege mode with remote effects
            If sModule.ID = ModuleEnum.ItemSiegeModuleI Or sModule.ID = ModuleEnum.ItemSiegeModuleT2 Then
                If ParentFitting.FittedShip.RemoteSlotCollection.Count > 0 Then
                    Const Msg As String =
                              "You have active remote modules and activating Siege Mode will cancel these effects. Do you wish to continue activating Siege Mode?"
                    Dim reply As Integer = MessageBox.Show(Msg, "Confirm Activate Siege Mode", MessageBoxButtons.YesNo,
                                                           MessageBoxIcon.Question)
                    If reply = DialogResult.No Then
                        sModule.ModuleState = oldState
                        Exit Sub
                    Else
                        ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                        Call ResetRemoteEffects()
                    End If
                End If
            End If
            ' Check for activation of triage mode with remote effects
            If sModule.ID = ModuleEnum.ItemTriageModuleI Or sModule.ID = ModuleEnum.ItemTriageModuleT2 Then
                If ParentFitting.FittedShip.RemoteSlotCollection.Count > 0 Then
                    Const Msg As String =
                              "You have active remote modules and activating Triage Mode will cancel these effects. Do you wish to continue activating Triage Mode?"
                    Dim reply As Integer = MessageBox.Show(Msg, "Confirm Activate Triage Mode", MessageBoxButtons.YesNo,
                                                           MessageBoxIcon.Question)
                    If reply = DialogResult.No Then
                        sModule.ModuleState = oldState
                        Exit Sub
                    Else
                        ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                        Call ResetRemoteEffects()
                    End If
                End If
            End If
        End Sub

        Private Sub SetSingleModuleOverloaded(sModule As ShipModule)
            Dim fModule As New ShipModule
            Select Case sModule.SlotType
                Case SlotTypes.Rig

                    fModule = ParentFitting.FittedShip.RigSlot(sModule.SlotNo)
                Case SlotTypes.Low
                    fModule = ParentFitting.FittedShip.LowSlot(sModule.SlotNo)
                Case SlotTypes.Mid
                    fModule = ParentFitting.FittedShip.MidSlot(sModule.SlotNo)
                Case SlotTypes.High
                    fModule = ParentFitting.FittedShip.HiSlot(sModule.SlotNo)
                Case SlotTypes.Subsystem
                    fModule = ParentFitting.FittedShip.SubSlot(sModule.SlotNo)
            End Select
            Dim oldState As ModuleStates = sModule.ModuleState
            sModule.ModuleState = ModuleStates.Overloaded
            ' Check for maxGroupActive flag
            If sModule.Attributes.ContainsKey(AttributeEnum.ModuleMaxGroupActive) = True Then
                If sModule.DatabaseGroup <> ModuleEnum.GroupGangLinks Then
                    If _
                        ParentFitting.IsModuleGroupLimitExceeded(sModule, False, AttributeEnum.ModuleMaxGroupActive) =
                        True Then
                        ' Set the module offline
                        MessageBox.Show(
                            "You cannot activate the " & sModule.Name &
                            " due to a restriction on the maximum number permitted for this group.",
                            "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        sModule.ModuleState = oldState
                        Exit Sub
                    End If
                Else
                    If _
                        ParentFitting.IsModuleGroupLimitExceeded(sModule, False, AttributeEnum.ModuleMaxGroupActive) =
                        True Then
                        ' Set the module offline
                        MessageBox.Show(
                            "You cannot activate the " & sModule.Name &
                            " due to a restriction on the maximum number permitted for this group.",
                            "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        sModule.ModuleState = oldState
                        Exit Sub
                    Else
                        If _
                            ParentFitting.CountActiveTypeModules(sModule.ID) >
                            CInt(fModule.Attributes(AttributeEnum.ModuleMaxGroupActive)) Then
                            ' Set the module offline
                            MessageBox.Show(
                                "You cannot activate the " & sModule.Name &
                                " due to a restriction on the maximum number permitted for this type.",
                                "Module Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            sModule.ModuleState = oldState
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End Sub

#End Region

#Region "Load/Remove Charges"

        Private Sub RemoveChargeFromModule(ByVal sender As Object, ByVal e As EventArgs)
            For Each selItem As Node In adtSlots.SelectedNodes
                Call RemoveSingleChargeFromModule(selItem, False)
            Next
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            Call UpdateHistory()
        End Sub

        Private Sub RemoveSingleChargeFromModule(ByVal selItem As Node, ByVal suppressUndo As Boolean)
            Dim sep As Integer = selItem.Name.LastIndexOf("_", StringComparison.Ordinal)
            Dim slotType As Integer = CInt(selItem.Name.Substring(0, sep))
            Dim slotNo As Integer = CInt(selItem.Name.Substring(sep + 1, 1))
            Dim loadedModule As New ShipModule
            Select Case slotType
                Case SlotTypes.Rig
                    loadedModule = ParentFitting.BaseShip.RigSlot(slotNo)
                Case SlotTypes.Low
                    loadedModule = ParentFitting.BaseShip.LowSlot(slotNo)
                Case SlotTypes.Mid
                    loadedModule = ParentFitting.BaseShip.MidSlot(slotNo)
                Case SlotTypes.High
                    loadedModule = ParentFitting.BaseShip.HiSlot(slotNo)
                Case SlotTypes.Subsystem
                    loadedModule = ParentFitting.BaseShip.SubSlot(slotNo)
            End Select
            If suppressUndo = False Then
                UndoStack.Push(New UndoInfo(UndoInfo.TransType.RemoveCharge, slotType, slotNo, loadedModule.Name,
                                            loadedModule.LoadedCharge.Name, slotNo, loadedModule.Name, ""))
            End If
            loadedModule.LoadedCharge = Nothing
        End Sub

        Private Sub LoadChargeIntoModule(ByVal sender As Object, ByVal e As EventArgs)
            Dim chargeMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim moduleID As Integer = CInt(chargeMenu.Name)
            ' Get name of the "slot" which has slot type and number
            For Each selItem As Node In adtSlots.SelectedNodes
                Dim charge As ShipModule = ModuleLists.ModuleList.Item(moduleID).Clone
                Call LoadSingleChargeIntoModule(selItem, charge, False)
            Next
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            Call UpdateHistory()
        End Sub

        Private Sub LoadSingleChargeIntoModule(ByVal selItem As Node, ByVal charge As ShipModule,
                                               ByVal suppressUndo As Boolean)
            If selItem.Text <> "<Empty>" Then
                Dim sep As Integer = selItem.Name.LastIndexOf("_", StringComparison.Ordinal)
                Dim slotType As Integer = CInt(selItem.Name.Substring(0, sep))
                Dim slotNo As Integer = CInt(selItem.Name.Substring(sep + 1, 1))
                Dim loadedModule As New ShipModule
                Select Case slotType
                    Case SlotTypes.Rig
                        loadedModule = ParentFitting.BaseShip.RigSlot(slotNo)
                    Case SlotTypes.Low
                        loadedModule = ParentFitting.BaseShip.LowSlot(slotNo)
                    Case SlotTypes.Mid
                        loadedModule = ParentFitting.BaseShip.MidSlot(slotNo)
                    Case SlotTypes.High
                        loadedModule = ParentFitting.BaseShip.HiSlot(slotNo)
                    Case SlotTypes.Subsystem
                        loadedModule = ParentFitting.BaseShip.SubSlot(slotNo)
                End Select
                Dim oldChargeName As String = ""
                If loadedModule.LoadedCharge IsNot Nothing Then
                    oldChargeName = loadedModule.LoadedCharge.Name
                End If
                loadedModule.LoadedCharge = charge
                If suppressUndo = False Then
                    UndoStack.Push(New UndoInfo(UndoInfo.TransType.AddCharge, slotType, slotNo, loadedModule.Name,
                                                oldChargeName, slotNo, loadedModule.Name, loadedModule.LoadedCharge.Name))
                End If
            End If
        End Sub

#End Region

#Region "Storage Bay Routines"

        Private Sub RedrawCargoBay()
            lvwCargoBay.BeginUpdate()
            lvwCargoBay.Items.Clear()
            ParentFitting.BaseShip.CargoBayUsed = 0
            ParentFitting.BaseShip.CargoBayAdditional = 0
            Dim cbi As CargoBayItem
            Dim holdingBay As New SortedList
            For Each cbi In ParentFitting.BaseShip.CargoBayItems.Values
                holdingBay.Add(holdingBay.Count, cbi)
            Next
            ParentFitting.BaseShip.CargoBayItems.Clear()
            For Each cbi In holdingBay.Values
                Dim newCargoItem As New ListViewItem(cbi.ItemType.Name)
                newCargoItem.Name = CStr(lvwCargoBay.Items.Count)
                newCargoItem.SubItems.Add(CStr(cbi.Quantity))
                ParentFitting.BaseShip.CargoBayItems.Add(lvwCargoBay.Items.Count, cbi)
                ParentFitting.BaseShip.CargoBayUsed += cbi.ItemType.Volume*cbi.Quantity
                If cbi.ItemType.IsContainer Then _
                    ParentFitting.BaseShip.CargoBayAdditional += (cbi.ItemType.Capacity - cbi.ItemType.Volume)*
                                                                 cbi.Quantity
                lvwCargoBay.Items.Add(newCargoItem)
            Next
            lvwCargoBay.EndUpdate()
            Call RedrawCargoBayCapacity()
        End Sub

        Private Sub RedrawDroneBay()
            lvwDroneBay.BeginUpdate()
            lvwDroneBay.Items.Clear()
            ParentFitting.BaseShip.DroneBayUsed = 0
            Dim dbi As DroneBayItem
            Dim holdingBay As New SortedList
            For Each dbi In ParentFitting.BaseShip.DroneBayItems.Values
                holdingBay.Add(holdingBay.Count, dbi)
            Next
            ParentFitting.BaseShip.DroneBayItems.Clear()
            For Each dbi In holdingBay.Values
                Dim newDroneItem As New ListViewItem(dbi.DroneType.Name)
                newDroneItem.Name = CStr(lvwDroneBay.Items.Count)
                newDroneItem.SubItems.Add(CStr(dbi.Quantity))
                If dbi.IsActive = True Then
                    newDroneItem.Checked = True
                End If
                ParentFitting.BaseShip.DroneBayItems.Add(lvwDroneBay.Items.Count, dbi)
                ParentFitting.BaseShip.DroneBayUsed += dbi.DroneType.Volume*dbi.Quantity
                lvwDroneBay.Items.Add(newDroneItem)
            Next
            lvwDroneBay.EndUpdate()
            Call RedrawDroneBayCapacity()
        End Sub

        Private Sub RedrawShipBay()
            lvwShipBay.BeginUpdate()
            lvwShipBay.Items.Clear()
            ParentFitting.BaseShip.ShipBayUsed = 0
            Dim sbi As ShipBayItem
            Dim holdingBay As New SortedList
            For Each sbi In ParentFitting.BaseShip.ShipBayItems.Values
                holdingBay.Add(holdingBay.Count, sbi)
            Next
            ParentFitting.BaseShip.ShipBayItems.Clear()
            For Each sbi In holdingBay.Values
                Dim newCargoItem As New ListViewItem(sbi.ShipType.Name)
                newCargoItem.Name = CStr(lvwShipBay.Items.Count)
                newCargoItem.SubItems.Add(CStr(sbi.Quantity))
                newCargoItem.SubItems.Add(sbi.ShipType.Volume.ToString("N0"))
                newCargoItem.SubItems.Add((sbi.ShipType.Volume*sbi.Quantity).ToString("N0"))
                ParentFitting.BaseShip.ShipBayItems.Add(lvwShipBay.Items.Count, sbi)
                ParentFitting.BaseShip.ShipBayUsed += sbi.ShipType.Volume*sbi.Quantity
                lvwShipBay.Items.Add(newCargoItem)
            Next
            lvwShipBay.EndUpdate()
            Call RedrawShipBayCapacity()
        End Sub

        Private Sub RedrawCargoBayCapacity()
            If ParentFitting.BaseShip.CargoBayAdditional > 0 Then
                lblCargoBay.Text = ParentFitting.BaseShip.CargoBayUsed.ToString("N2") & " / " &
                                   ParentFitting.FittedShip.CargoBay.ToString("N2") & " (" &
                                   (ParentFitting.FittedShip.CargoBay + ParentFitting.BaseShip.CargoBayAdditional).
                                       ToString(
                                           "N2") & ") m³"
            Else
                lblCargoBay.Text = ParentFitting.BaseShip.CargoBayUsed.ToString("N2") & " / " &
                                   ParentFitting.FittedShip.CargoBay.ToString("N2") & " m³"
            End If
            If ParentFitting.FittedShip.CargoBay > 0 Then
                pbCargoBay.Maximum = CInt(ParentFitting.FittedShip.CargoBay)
            Else
                pbCargoBay.Maximum = 1
            End If
            If ParentFitting.BaseShip.CargoBayUsed > ParentFitting.FittedShip.CargoBay Then
                pbCargoBay.Value = CInt(ParentFitting.FittedShip.CargoBay)
            Else
                pbCargoBay.Value = CInt(ParentFitting.BaseShip.CargoBayUsed)
            End If
        End Sub

        Private Sub RedrawDroneBayCapacity()
            lvwDroneBay.EndUpdate()
            lblDroneBay.Text = ParentFitting.BaseShip.DroneBayUsed.ToString("N2") & " / " &
                               ParentFitting.FittedShip.DroneBay.ToString("N2") & " m³"
            If ParentFitting.FittedShip.DroneBay > 0 Then
                pbDroneBay.Maximum = CInt(ParentFitting.FittedShip.DroneBay)
            Else
                pbDroneBay.Maximum = 1
            End If
            If ParentFitting.BaseShip.DroneBayUsed > ParentFitting.FittedShip.DroneBay Then
                pbDroneBay.Value = CInt(ParentFitting.FittedShip.DroneBay)
            Else
                pbDroneBay.Value = CInt(ParentFitting.BaseShip.DroneBayUsed)
            End If
        End Sub

        Private Sub RedrawShipBayCapacity()
            lblShipBay.Text = ParentFitting.BaseShip.ShipBayUsed.ToString("N2") & " / " &
                              ParentFitting.FittedShip.ShipBay.ToString("N2") & " m³"
            If ParentFitting.FittedShip.ShipBay > 0 Then
                pbShipBay.Maximum = CInt(ParentFitting.FittedShip.ShipBay)
            Else
                pbShipBay.Maximum = 1
            End If
            If ParentFitting.BaseShip.ShipBayUsed > ParentFitting.FittedShip.ShipBay Then
                pbShipBay.Value = CInt(ParentFitting.FittedShip.ShipBay)
            Else
                pbShipBay.Value = CInt(ParentFitting.BaseShip.ShipBayUsed)
            End If
        End Sub

        Private Sub lvwDroneBay_ItemChecked(ByVal sender As Object, ByVal e As ItemCheckedEventArgs) _
            Handles lvwDroneBay.ItemChecked
            If _updateAll = False Or _cancelDroneActivation = True Then
                Dim idx As Integer = CInt(e.Item.Name)
                Dim dbi As DroneBayItem = ParentFitting.BaseShip.DroneBayItems.Item(idx)
                ' Check we have the bandwidth and/or control ability for this item
                If _updateDrones = False Then
                    Dim reqQ As Integer = dbi.Quantity
                    If e.Item.Checked = True Then
                        If ParentFitting.FittedShip.UsedDrones + reqQ > ParentFitting.FittedShip.MaxDrones Then
                            ' Cannot do this because our drone control skill in insufficient
                            MessageBox.Show(
                                "You do not have the ability to control this many drones. Please split the group and try again.",
                                "Drone Control Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            _cancelDroneActivation = True
                            e.Item.Checked = False
                            Exit Sub
                        End If
                        If _
                            ParentFitting.FittedShip.DroneBandwidthUsed + (reqQ*CDbl(dbi.DroneType.Attributes(1272))) >
                            ParentFitting.FittedShip.DroneBandwidth Then
                            ' Cannot do this because we don't have enough bandwidth
                            MessageBox.Show(
                                "You do not have the spare bandwidth to control this many drones. Please split the group and try again.",
                                "Drone Bandwidth Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            _cancelDroneActivation = True
                            e.Item.Checked = False
                            Exit Sub
                        End If
                    End If
                    dbi.IsActive = e.Item.Checked
                    ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                    If dbi.IsActive = True Then
                        ParentFitting.BaseShip.Attributes(10006) = CDbl(ParentFitting.BaseShip.Attributes(10006)) + reqQ
                    Else
                        ParentFitting.BaseShip.Attributes(10006) =
                            Math.Max(CDbl(ParentFitting.BaseShip.Attributes(10006)) - reqQ, 0)
                    End If
                End If
                Call ParentFitting.UpdateFittingFromBaseShip()
                Call _currentInfo.UpdateDroneUsage()
            End If
            _cancelDroneActivation = False
        End Sub

        Private Sub ctxBays_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxBays.Opening
            _lvwBay = CType(ctxBays.SourceControl, ListView)
            Select Case _lvwBay.Name
                Case "lvwCargoBay"
                    If _lvwBay.SelectedItems.Count > 0 Then
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
                    If _lvwBay.SelectedItems.Count > 0 Then
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
                    If _lvwBay.SelectedItems.Count > 0 Then
                        ctxShowBayInfoItem.Text = "Show Drone Info"
                        ctxShowBayInfoItem.Enabled = True
                        ctxSplitBatch.Enabled = True
                        Dim selItem As ListViewItem = _lvwBay.SelectedItems(0)
                        Dim idx As Integer = CInt(selItem.Name)
                        Dim dbi As DroneBayItem = ParentFitting.BaseShip.DroneBayItems.Item(idx)
                        Dim currentMod As ShipModule = dbi.DroneType

                        ' Check for Relevant Skills in Modules/Charges
                        Dim relModuleSkills, relChargeSkills As New ArrayList
                        Dim affects(3) As String
                        For Each affect As String In currentMod.Affects
                            If affect.Contains(";Skill;") = True Then
                                affects = affect.Split((";").ToCharArray)
                                If relModuleSkills.Contains(affects(0)) = False Then
                                    relModuleSkills.Add(affects(0))
                                End If
                            End If
                            If affect.Contains(";Ship Bonus;") = True Then
                                affects = affect.Split((";").ToCharArray)
                                If ParentFitting.ShipName = affects(0) Then
                                    If relModuleSkills.Contains(affects(3)) = False Then
                                        relModuleSkills.Add(affects(3))
                                    End If
                                End If
                            End If
                        Next
                        relModuleSkills.Sort()
                        If currentMod.LoadedCharge IsNot Nothing Then
                            For Each affect As String In currentMod.LoadedCharge.Affects
                                If affect.Contains(";Skill;") = True Then
                                    affects = affect.Split((";").ToCharArray)
                                    If relChargeSkills.Contains(affects(0)) = False Then
                                        relChargeSkills.Add(affects(0))
                                    End If
                                End If
                                If affect.Contains(";Ship Bonus;") = True Then
                                    affects = affect.Split((";").ToCharArray)
                                    If ParentFitting.ShipName = affects(0) Then
                                        If relChargeSkills.Contains(affects(3)) = False Then
                                            relChargeSkills.Add(affects(3))
                                        End If
                                    End If
                                End If
                            Next
                        End If
                        relChargeSkills.Sort()
                        If relModuleSkills.Count > 0 Or relChargeSkills.Count > 0 Then
                            ' Add the Main menu item
                            Dim alterRelevantSkills As New ToolStripMenuItem
                            alterRelevantSkills.Name = "Drone Skills"
                            alterRelevantSkills.Text = "Alter Relevant Skills"
                            For Each relSkill As String In relModuleSkills
                                Dim newRelSkill As New ToolStripMenuItem
                                newRelSkill.Name = relSkill
                                newRelSkill.Text = relSkill
                                Dim pilotLevel As Integer =
                                        FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).SkillSet(
                                            relSkill).Level
                                newRelSkill.Image =
                                    CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString), Image)
                                For skillLevel As Integer = 0 To 5
                                    Dim newRelSkillLevel As New ToolStripMenuItem
                                    newRelSkillLevel.Name = relSkill & skillLevel.ToString
                                    newRelSkillLevel.Text = "Level " & skillLevel.ToString
                                    If skillLevel = pilotLevel Then
                                        newRelSkillLevel.Checked = True
                                    End If
                                    AddHandler newRelSkillLevel.Click, AddressOf SetPilotSkillLevel
                                    newRelSkill.DropDownItems.Add(newRelSkillLevel)
                                Next
                                newRelSkill.DropDownItems.Add("-")
                                Dim defaultLevel As Integer = 0
                                If _
                                    HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills.
                                        ContainsKey(relSkill) = True Then
                                    defaultLevel =
                                        HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills(
                                            relSkill).Level
                                End If
                                Dim newRelSkillDefault As New ToolStripMenuItem
                                newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                                newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                                AddHandler newRelSkillDefault.Click, AddressOf SetPilotSkillLevel
                                newRelSkill.DropDownItems.Add(newRelSkillDefault)
                                alterRelevantSkills.DropDownItems.Add(newRelSkill)
                            Next
                            If alterRelevantSkills.DropDownItems.Count > 0 And relChargeSkills.Count > 0 Then
                                alterRelevantSkills.DropDownItems.Add("-")
                            End If
                            For Each relSkill As String In relChargeSkills
                                Dim newRelSkill As New ToolStripMenuItem
                                newRelSkill.Name = relSkill
                                newRelSkill.Text = relSkill
                                Dim pilotLevel As Integer =
                                        FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).SkillSet(
                                            relSkill).Level
                                newRelSkill.Image =
                                    CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString), Image)
                                For skillLevel As Integer = 0 To 5
                                    Dim newRelSkillLevel As New ToolStripMenuItem
                                    newRelSkillLevel.Name = relSkill & skillLevel.ToString
                                    newRelSkillLevel.Text = "Level " & skillLevel.ToString
                                    If skillLevel = pilotLevel Then
                                        newRelSkillLevel.Checked = True
                                    End If
                                    AddHandler newRelSkillLevel.Click, AddressOf SetPilotSkillLevel
                                    newRelSkill.DropDownItems.Add(newRelSkillLevel)
                                Next
                                newRelSkill.DropDownItems.Add("-")
                                Dim defaultLevel As Integer = 0
                                If _
                                    HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills.
                                        ContainsKey(relSkill) = True Then
                                    defaultLevel =
                                        HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills(
                                            relSkill).Level
                                End If
                                Dim newRelSkillDefault As New ToolStripMenuItem
                                newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                                newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                                AddHandler newRelSkillDefault.Click, AddressOf SetPilotSkillLevel
                                newRelSkill.DropDownItems.Add(newRelSkillDefault)
                                alterRelevantSkills.DropDownItems.Add(newRelSkill)
                            Next
                            If ctxBays.Items.ContainsKey("Drone Skills") = True Then
                                ctxBays.Items.RemoveByKey("Drone Skills")
                            End If
                            ctxBays.Items.Add(alterRelevantSkills)
                        End If
                        If _lvwBay.SelectedItems.Count > 1 Then
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

        Private Sub ctxRemoveItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxRemoveItem.Click
            Select Case _lvwBay.Name
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
                    _updateDrones = True
                    Call RedrawDroneBay()
                    _updateDrones = False
                    ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            End Select
        End Sub

        Private Sub ctxShowBayInfoItem_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles ctxShowBayInfoItem.Click
            Dim selItem As ListViewItem = _lvwBay.SelectedItems(0)
            Dim sModule As New ShipModule
            Select Case _lvwBay.Name
                Case "lvwCargoBay"
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim dbi As CargoBayItem = ParentFitting.FittedShip.CargoBayItems.Item(idx)
                    sModule = dbi.ItemType
                Case "lvwDroneBay"
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim dbi As DroneBayItem = ParentFitting.FittedShip.DroneBayItems.Item(idx)
                    sModule = dbi.DroneType
            End Select
            Dim showInfo As New FrmShowInfo
            Dim hPilot As EveHQPilot
            If _currentInfo IsNot Nothing Then
                hPilot = HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString)
            Else
                If HQ.Settings.StartupPilot <> "" Then
                    hPilot = HQ.Settings.Pilots(HQ.Settings.StartupPilot)
                Else
                    hPilot = HQ.Settings.Pilots.Values(0)
                End If
            End If
            showInfo.ShowItemDetails(sModule, hPilot)
        End Sub

        Private Sub ctxAlterQuantity_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxAlterQuantity.Click
            Select Case _lvwBay.Name
                Case "lvwCargoBay"
                    Dim selItem As ListViewItem = lvwCargoBay.SelectedItems(0)
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim cbi As CargoBayItem = ParentFitting.BaseShip.CargoBayItems.Item(idx)
                    Using newSelectForm As New FrmSelectQuantity
                        newSelectForm.FittedShip = ParentFitting.FittedShip
                        newSelectForm.Cbi = cbi
                        newSelectForm.BayType = FrmSelectQuantity.BayTypes.CargoBay
                        newSelectForm.IsSplit = False
                        newSelectForm.nudQuantity.Minimum = 1
                        newSelectForm.nudQuantity.Maximum = cbi.Quantity +
                                                            CInt(
                                                                Int(
                                                                    (ParentFitting.FittedShip.CargoBay -
                                                                     ParentFitting.BaseShip.CargoBayUsed)/
                                                                    cbi.ItemType.Volume))
                        newSelectForm.nudQuantity.Value = cbi.Quantity
                        newSelectForm.ShowDialog()
                    End Using
                    ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                    Call ParentFitting.UpdateFittingFromBaseShip()
                    Call RedrawCargoBay()
                Case "lvwShipBay"
                    Dim selItem As ListViewItem = lvwShipBay.SelectedItems(0)
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim sbi As ShipBayItem = ParentFitting.BaseShip.ShipBayItems.Item(idx)
                    Using newSelectForm As New FrmSelectQuantity
                        newSelectForm.FittedShip = ParentFitting.FittedShip
                        newSelectForm.Sbi = sbi
                        newSelectForm.BayType = FrmSelectQuantity.BayTypes.ShipBay
                        newSelectForm.IsSplit = False
                        newSelectForm.nudQuantity.Minimum = 1
                        newSelectForm.nudQuantity.Maximum = sbi.Quantity +
                                                            CInt(
                                                                Int(
                                                                    (ParentFitting.FittedShip.ShipBay -
                                                                     ParentFitting.BaseShip.ShipBayUsed)/
                                                                    sbi.ShipType.Volume))
                        newSelectForm.nudQuantity.Value = sbi.Quantity
                        newSelectForm.ShowDialog()
                    End Using
                    Call ParentFitting.UpdateFittingFromBaseShip()
                    Call RedrawShipBay()
                Case "lvwDroneBay"
                    Dim selItem As ListViewItem = lvwDroneBay.SelectedItems(0)
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim dbi As DroneBayItem = ParentFitting.BaseShip.DroneBayItems.Item(idx)
                    Using newSelectForm As New FrmSelectQuantity
                        newSelectForm.FittedShip = ParentFitting.FittedShip
                        newSelectForm.Dbi = dbi
                        newSelectForm.BayType = FrmSelectQuantity.BayTypes.DroneBay
                        newSelectForm.IsSplit = False
                        newSelectForm.nudQuantity.Minimum = 1
                        newSelectForm.nudQuantity.Maximum = dbi.Quantity +
                                                            CInt(
                                                                Int(
                                                                    (ParentFitting.FittedShip.DroneBay -
                                                                     ParentFitting.BaseShip.DroneBayUsed)/
                                                                    dbi.DroneType.Volume))
                        newSelectForm.nudQuantity.Value = dbi.Quantity
                        newSelectForm.ShowDialog()
                        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                        Call ParentFitting.UpdateFittingFromBaseShip()
                        _updateDrones = True
                        Call RedrawDroneBay()
                        _updateDrones = False
                    End Using
            End Select
        End Sub

        Private Sub ctxSplitBatch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxSplitBatch.Click
            Select Case _lvwBay.Name
                Case "lvwCargoBay"
                    Dim selItem As ListViewItem = lvwCargoBay.SelectedItems(0)
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim cbi As CargoBayItem = ParentFitting.BaseShip.CargoBayItems.Item(idx)
                    Using newSelectForm As New FrmSelectQuantity
                        newSelectForm.FittedShip = ParentFitting.FittedShip
                        newSelectForm.CurrentShip = ParentFitting.BaseShip
                        newSelectForm.Cbi = cbi
                        newSelectForm.BayType = FrmSelectQuantity.BayTypes.CargoBay
                        newSelectForm.IsSplit = True
                        newSelectForm.nudQuantity.Value = 1
                        newSelectForm.nudQuantity.Minimum = 1
                        newSelectForm.nudQuantity.Maximum = cbi.Quantity - 1
                        newSelectForm.ShowDialog()
                    End Using
                    Call ParentFitting.UpdateFittingFromBaseShip()
                    Call RedrawCargoBay()
                Case "lvwShipBay"
                    Dim selItem As ListViewItem = lvwShipBay.SelectedItems(0)
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim sbi As ShipBayItem = ParentFitting.BaseShip.ShipBayItems.Item(idx)
                    Using newSelectForm As New FrmSelectQuantity
                        newSelectForm.FittedShip = ParentFitting.FittedShip
                        newSelectForm.CurrentShip = ParentFitting.BaseShip
                        newSelectForm.Sbi = sbi
                        newSelectForm.BayType = FrmSelectQuantity.BayTypes.ShipBay
                        newSelectForm.IsSplit = True
                        newSelectForm.nudQuantity.Value = 1
                        newSelectForm.nudQuantity.Minimum = 1
                        newSelectForm.nudQuantity.Maximum = sbi.Quantity - 1
                        newSelectForm.ShowDialog()
                    End Using
                    Call ParentFitting.UpdateFittingFromBaseShip()
                    Call RedrawShipBay()
                Case "lvwDroneBay"
                    Dim selItem As ListViewItem = lvwDroneBay.SelectedItems(0)
                    Dim idx As Integer = CInt(selItem.Name)
                    Dim dbi As DroneBayItem = ParentFitting.BaseShip.DroneBayItems.Item(idx)
                    Using newSelectForm As New FrmSelectQuantity
                        newSelectForm.FittedShip = ParentFitting.FittedShip
                        newSelectForm.CurrentShip = ParentFitting.BaseShip
                        newSelectForm.Dbi = dbi
                        newSelectForm.BayType = FrmSelectQuantity.BayTypes.DroneBay
                        newSelectForm.IsSplit = True
                        newSelectForm.nudQuantity.Value = 1
                        newSelectForm.nudQuantity.Minimum = 1
                        newSelectForm.nudQuantity.Maximum = dbi.Quantity - 1
                        newSelectForm.ShowDialog()
                        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                        Call ParentFitting.UpdateFittingFromBaseShip()
                        _updateDrones = True
                        Call RedrawDroneBay()
                        _updateDrones = False
                    End Using
            End Select
        End Sub

        Private Sub btnMergeDrones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMergeDrones.Click
            _updateDrones = True
            lvwDroneBay.BeginUpdate()
            lvwDroneBay.Items.Clear()
            ParentFitting.BaseShip.DroneBayUsed = 0
            Dim dbi As DroneBayItem
            Dim holdingBay As New SortedList
            Dim droneQuantities As New SortedList
            For Each dbi In ParentFitting.BaseShip.DroneBayItems.Values
                If holdingBay.Contains(dbi.DroneType.Name) = False Then
                    holdingBay.Add(dbi.DroneType.Name, dbi.DroneType)
                End If
                If droneQuantities.Contains(dbi.DroneType.Name) = True Then
                    Dim cq As Integer = CInt(droneQuantities(dbi.DroneType.Name))
                    droneQuantities(dbi.DroneType.Name) = cq + dbi.Quantity
                Else
                    droneQuantities.Add(dbi.DroneType.Name, dbi.Quantity)
                End If
            Next
            ParentFitting.BaseShip.DroneBayItems.Clear()
            For Each drone As String In holdingBay.Keys
                dbi = New DroneBayItem
                dbi.DroneType = CType(holdingBay(drone), ShipModule)
                dbi.IsActive = False
                dbi.Quantity = CInt(droneQuantities(drone))
                Dim newDroneItem As New ListViewItem(dbi.DroneType.Name)
                newDroneItem.Name = CStr(lvwDroneBay.Items.Count)
                newDroneItem.SubItems.Add(CStr(dbi.Quantity))
                ParentFitting.BaseShip.DroneBayItems.Add(lvwDroneBay.Items.Count, dbi)
                ParentFitting.BaseShip.DroneBayUsed += dbi.DroneType.Volume*dbi.Quantity
                lvwDroneBay.Items.Add(newDroneItem)
            Next
            lvwDroneBay.EndUpdate()
            Call RedrawDroneBayCapacity()
            _updateDrones = False
            ' Rebuild the ship to account for any disabled drones
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Sub

        Private Sub btnMergeCargo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMergeCargo.Click
            lvwCargoBay.BeginUpdate()
            lvwCargoBay.Items.Clear()
            ParentFitting.BaseShip.CargoBayUsed = 0
            ParentFitting.BaseShip.CargoBayAdditional = 0
            Dim cbi As CargoBayItem
            Dim holdingBay As New SortedList
            Dim cargoQuantities As New SortedList
            For Each cbi In ParentFitting.BaseShip.CargoBayItems.Values
                If holdingBay.Contains(cbi.ItemType.Name) = False Then
                    holdingBay.Add(cbi.ItemType.Name, cbi.ItemType)
                End If
                If cargoQuantities.Contains(cbi.ItemType.Name) = True Then
                    Dim cq As Integer = CInt(cargoQuantities(cbi.ItemType.Name))
                    cargoQuantities(cbi.ItemType.Name) = cq + cbi.Quantity
                Else
                    cargoQuantities.Add(cbi.ItemType.Name, cbi.Quantity)
                End If
            Next
            ParentFitting.BaseShip.CargoBayItems.Clear()
            For Each cargo As String In holdingBay.Keys
                cbi = New CargoBayItem
                cbi.ItemType = CType(holdingBay(cargo), ShipModule)
                cbi.Quantity = CInt(cargoQuantities(cargo))
                Dim newCargoItem As New ListViewItem(cbi.ItemType.Name)
                newCargoItem.Name = CStr(lvwCargoBay.Items.Count)
                newCargoItem.SubItems.Add(CStr(cbi.Quantity))
                ParentFitting.BaseShip.CargoBayItems.Add(lvwCargoBay.Items.Count, cbi)
                ParentFitting.BaseShip.CargoBayUsed += cbi.ItemType.Volume*cbi.Quantity
                If cbi.ItemType.IsContainer Then _
                    ParentFitting.BaseShip.CargoBayAdditional += (cbi.ItemType.Capacity - cbi.ItemType.Volume)*
                                                                 cbi.Quantity
                lvwCargoBay.Items.Add(newCargoItem)
            Next
            lvwCargoBay.EndUpdate()
            Call RedrawCargoBayCapacity()
        End Sub

#End Region

#Region "Slot Drag/Drop Routines"

        Private Sub adtSlots_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles adtSlots.DragOver
            adtSlots.DragDropEnabled = False
            Dim oLvi As Node = CType(e.Data.GetData(GetType(Node)), Node)
            If oLvi IsNot Nothing Then
                Dim oSep As Integer = oLvi.Name.LastIndexOf("_", StringComparison.Ordinal)
                Dim oSlotType As Integer = CInt(oLvi.Name.Substring(0, oSep))
                Dim oslotNo As Integer = CInt(oLvi.Name.Substring(oSep + 1, 1))

                Dim p As Point = adtSlots.PointToClient(New Point(e.X, e.Y))
                Dim nLvi As Node = adtSlots.GetNodeAt(p.X, p.Y)
                If nLvi IsNot Nothing Then
                    Dim nSep As Integer = nLvi.Name.LastIndexOf("_", StringComparison.Ordinal)
                    Dim nSlotType As Integer = CInt(nLvi.Name.Substring(0, nSep))
                    Dim nslotNo As Integer = CInt(nLvi.Name.Substring(nSep + 1, 1))
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

        Private Sub adtSlots_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles adtSlots.DragDrop
            Dim oLvi As Node = CType(e.Data.GetData(GetType(Node)), Node)

            Dim oSep As Integer = oLvi.Name.LastIndexOf("_", StringComparison.Ordinal)
            Dim oSlotType As Integer = CInt(oLvi.Name.Substring(0, oSep))
            Dim oslotNo As Integer = CInt(oLvi.Name.Substring(oSep + 1, 1))

            Dim p As Point = adtSlots.PointToClient(New Point(e.X, e.Y))
            Dim nLvi As Node = adtSlots.GetNodeAt(p.X, p.Y)

            Dim nSep As Integer = nLvi.Name.LastIndexOf("_", StringComparison.Ordinal)
            Dim nSlotType As Integer = CInt(nLvi.Name.Substring(0, nSep))
            Dim nslotNo As Integer = CInt(nLvi.Name.Substring(nSep + 1, 1))

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
                    If oLvi.Text = "<Empty>" Then
                        ocMod = Nothing
                        ofMod = Nothing
                    Else
                        Select Case oSlotType
                            Case SlotTypes.Rig
                                ocMod = ParentFitting.BaseShip.RigSlot(oslotNo).Clone
                                ofMod = ParentFitting.FittedShip.RigSlot(oslotNo).Clone
                            Case SlotTypes.Low
                                ocMod = ParentFitting.BaseShip.LowSlot(oslotNo).Clone
                                ofMod = ParentFitting.FittedShip.LowSlot(oslotNo).Clone
                            Case SlotTypes.Mid
                                ocMod = ParentFitting.BaseShip.MidSlot(oslotNo).Clone
                                ofMod = ParentFitting.FittedShip.MidSlot(oslotNo).Clone
                            Case SlotTypes.High
                                ocMod = ParentFitting.BaseShip.HiSlot(oslotNo).Clone
                                ofMod = ParentFitting.FittedShip.HiSlot(oslotNo).Clone
                            Case SlotTypes.Subsystem
                                ocMod = ParentFitting.BaseShip.SubSlot(oslotNo).Clone
                                ofMod = ParentFitting.FittedShip.SubSlot(oslotNo).Clone
                        End Select
                    End If

                    If nLvi.Text = "<Empty>" Then
                        ncMod = Nothing
                        nfMod = Nothing
                    Else
                        Select Case nSlotType
                            Case SlotTypes.Rig
                                ncMod = ParentFitting.BaseShip.RigSlot(nslotNo).Clone
                                nfMod = ParentFitting.FittedShip.RigSlot(nslotNo).Clone
                            Case SlotTypes.Low
                                ncMod = ParentFitting.BaseShip.LowSlot(nslotNo).Clone
                                nfMod = ParentFitting.FittedShip.LowSlot(nslotNo).Clone
                            Case SlotTypes.Mid
                                ncMod = ParentFitting.BaseShip.MidSlot(nslotNo).Clone
                                nfMod = ParentFitting.FittedShip.MidSlot(nslotNo).Clone
                            Case SlotTypes.High
                                ncMod = ParentFitting.BaseShip.HiSlot(nslotNo).Clone
                                nfMod = ParentFitting.FittedShip.HiSlot(nslotNo).Clone
                            Case SlotTypes.Subsystem
                                ncMod = ParentFitting.BaseShip.SubSlot(nslotNo).Clone
                                nfMod = ParentFitting.FittedShip.SubSlot(nslotNo).Clone
                        End Select
                    End If
                    If e.Effect = DragDropEffects.Move Then ' Mouse button released?
                        'MessageBox.Show("Wanting to swap " & oLVI.Text & " for " & nLVI.Text & "?", "Confirm swap", MessageBoxButtons.OK, MessageBoxIcon.Question)
                        If ocMod Is Nothing Then
                            RemoveModule(nLvi, False, True)
                        Else
                            ocMod.SlotNo = nslotNo
                            Select Case nSlotType
                                Case SlotTypes.Rig
                                    ParentFitting.BaseShip.RigSlot(nslotNo) = ocMod
                                    ParentFitting.FittedShip.RigSlot(nslotNo) = ofMod
                                Case SlotTypes.Low
                                    ParentFitting.BaseShip.LowSlot(nslotNo) = ocMod
                                    ParentFitting.FittedShip.LowSlot(nslotNo) = ofMod
                                Case SlotTypes.Mid
                                    ParentFitting.BaseShip.MidSlot(nslotNo) = ocMod
                                    ParentFitting.FittedShip.MidSlot(nslotNo) = ofMod
                                Case SlotTypes.High
                                    ParentFitting.BaseShip.HiSlot(nslotNo) = ocMod
                                    ParentFitting.FittedShip.HiSlot(nslotNo) = ofMod
                                Case SlotTypes.Subsystem
                                    ParentFitting.BaseShip.SubSlot(nslotNo) = ocMod
                                    ParentFitting.FittedShip.SubSlot(nslotNo) = ofMod
                            End Select
                        End If
                        If ncMod Is Nothing Then
                            RemoveModule(oLvi, False, True)
                        Else
                            ncMod.SlotNo = oslotNo
                            Select Case oSlotType
                                Case SlotTypes.Rig
                                    ParentFitting.BaseShip.RigSlot(oslotNo) = ncMod
                                    ParentFitting.FittedShip.RigSlot(oslotNo) = nfMod
                                Case SlotTypes.Low
                                    ParentFitting.BaseShip.LowSlot(oslotNo) = ncMod
                                    ParentFitting.FittedShip.LowSlot(oslotNo) = nfMod
                                Case SlotTypes.Mid
                                    ParentFitting.BaseShip.MidSlot(oslotNo) = ncMod
                                    ParentFitting.FittedShip.MidSlot(oslotNo) = nfMod
                                Case SlotTypes.High
                                    ParentFitting.BaseShip.HiSlot(oslotNo) = ncMod
                                    ParentFitting.FittedShip.HiSlot(oslotNo) = nfMod
                                Case SlotTypes.Subsystem
                                    ParentFitting.BaseShip.SubSlot(oslotNo) = ncMod
                                    ParentFitting.FittedShip.SubSlot(oslotNo) = nfMod
                            End Select
                        End If
                        Call UpdateSlotLocation(ofMod, nslotNo)
                        Call UpdateSlotLocation(nfMod, oslotNo)
                        ' Update Undo History
                        Dim oModName As String
                        Dim oLoadedChargeName As String = ""
                        If ocMod IsNot Nothing Then
                            oModName = ocMod.Name
                            If ocMod.LoadedCharge IsNot Nothing Then
                                oLoadedChargeName = ocMod.LoadedCharge.Name
                            End If
                        Else
                            oModName = ""
                        End If
                        Dim nModName As String
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
                            UndoStack.Push(New UndoInfo(UndoInfo.TransType.SwapModules, oSlotType, oslotNo, oModName,
                                                        oLoadedChargeName, nslotNo, nModName, nLoadedChargeName))
                            UpdateHistory()
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
                            RemoveModule(nLvi, False, False)
                        End If
                        ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                    End If
                End If
            End If
            ' Update the ship details
            Call UpdateShipDetails()
            adtSlots.DragDropEnabled = True
        End Sub

        Private Sub adtSlots_DragLeave(ByVal sender As Object, ByVal e As EventArgs) Handles adtSlots.DragLeave
            ' Activate auto drag and drop once the drag and drop operation ends
            adtSlots.DragDropEnabled = True
        End Sub

#End Region

#Region "Remote Effects"

        Private Sub btnUpdateRemoteEffects_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles btnUpdateRemoteEffects.Click
            Call UpdateRemoteEffects()
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
                    Dim pPilot As FittingPilot = FittingPilots.HQFPilots(remoteFitting.Name.Substring(shipFit.Length + 2))
                    Dim newFit As Fitting = Fittings.FittingList(shipFit).Clone
                    newFit.UpdateBaseShipFromFitting()
                    newFit.PilotName = pPilot.PilotName
                    newFit.ApplyFitting(BuildType.BuildEverything)
                    Dim remoteShip As Ship = newFit.FittedShip
                    For Each remoteModule As ShipModule In remoteShip.SlotCollection
                        If _remoteGroups.Contains(CInt(remoteModule.DatabaseGroup)) = True Then
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
                        If _remoteGroups.Contains(CInt(remoteDrone.DroneType.DatabaseGroup)) = True Then
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

        Private Sub lvwRemoteEffects_ItemChecked(ByVal sender As Object, ByVal e As ItemCheckedEventArgs) _
            Handles lvwRemoteEffects.ItemChecked
            ' Check for an active siege module in the current ship
            If e.Item.Checked = True Then
                ' Get mod group ID
                Dim remoteMod As ShipModule
                If TypeOf e.Item.Tag Is ShipModule Then
                    remoteMod = CType(e.Item.Tag, ShipModule)
                Else
                    Dim remoteDrones As DroneBayItem = CType(e.Item.Tag, DroneBayItem)
                    remoteMod = remoteDrones.DroneType
                End If
                If _
                    remoteMod.DatabaseGroup <> ModuleEnum.GroupEnergyVampires And
                    remoteMod.DatabaseGroup <> ModuleEnum.GroupEnergyNeutralizers And
                    remoteMod.DatabaseGroup <> ModuleEnum.GroupEnergyNeutralizerDrones Then
                    For Each cMod As ShipModule In ParentFitting.FittedShip.SlotCollection
                        If _
                            (cMod.ID = ModuleEnum.ItemSiegeModuleI Or cMod.ID = ModuleEnum.ItemSiegeModuleT2) And
                            cMod.ModuleState = ModuleStates.Active Then
                            MessageBox.Show(
                                "You cannot apply remote effects from " & remoteMod.Name & " while the " &
                                ParentFitting.BaseShip.Name & " is in Siege Mode!", "Remote Effect Not Permitted",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                            e.Item.Checked = False
                            Exit Sub
                        ElseIf _
                            (cMod.ID = ModuleEnum.ItemTriageModuleI Or cMod.ID = ModuleEnum.ItemTriageModuleT2) And
                            cMod.ModuleState = ModuleStates.Active Then
                            MessageBox.Show(
                                "You cannot apply remote effects from " & remoteMod.Name & " while the " &
                                ParentFitting.BaseShip.Name & " is in Triage Mode!", "Remote Effect Not Permitted",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                            e.Item.Checked = False
                            Exit Sub
                        ElseIf cMod.ID = ModuleEnum.ItemBastionModuleI And cMod.ModuleState = ModuleStates.Active Then
                            MessageBox.Show(
                                "You cannot apply remote effects from " & remoteMod.Name & " while the " &
                                ParentFitting.BaseShip.Name & " is in Bastion Mode!", "Remote Effect Not Permitted",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                            e.Item.Checked = False
                            Exit Sub
                        End If
                    Next
                End If
            End If
            If lvwRemoteEffects.Tag.ToString <> "Refresh" Then
                ParentFitting.BaseShip.RemoteSlotCollection.Clear()
                For Each item As ListViewItem In lvwRemoteEffects.CheckedItems
                    If TypeOf item.Tag Is ShipModule Then
                        ParentFitting.BaseShip.RemoteSlotCollection.Add(CType(item.Tag, ShipModule))
                    Else
                        Dim remoteDrones As DroneBayItem = CType(item.Tag, DroneBayItem)
                        For drone = 1 To remoteDrones.Quantity
                            ParentFitting.BaseShip.RemoteSlotCollection.Add(remoteDrones.DroneType)
                        Next
                    End If
                Next
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            End If
        End Sub

        Private Sub mnuShowRemoteModInfo_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles mnuShowRemoteModInfo.Click
            Dim sModule As Object = lvwRemoteEffects.SelectedItems(0).Tag
            If TypeOf sModule Is DroneBayItem Then
                sModule = CType(sModule, DroneBayItem).DroneType
            End If
            Dim showInfo As New FrmShowInfo
            Dim hPilot As EveHQPilot
            hPilot = HQ.Settings.Pilots(lvwRemoteEffects.SelectedItems(0).Name)
            showInfo.ShowItemDetails(sModule, hPilot)
        End Sub

        Private Sub ctxRemoteModule_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) _
            Handles ctxRemoteModule.Opening
            If lvwRemoteEffects.SelectedItems.Count = 0 Then
                e.Cancel = True
            End If
        End Sub

        Private Sub btnAddRemoteFitting_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles btnAddRemoteFitting.Click
            ' Check if we have a fitting and a pilot and if so, generate the fitting
            If cboFitting.SelectedItem IsNot Nothing And cboPilot.SelectedItem IsNot Nothing Then
                If _
                    lvwRemoteFittings.Items.ContainsKey(
                        cboFitting.SelectedItem.ToString & ": " & cboPilot.SelectedItem.ToString) = False Then
                    Dim newFitting As New ListViewItem
                    newFitting.Name = cboFitting.SelectedItem.ToString & ": " & cboPilot.SelectedItem.ToString
                    newFitting.Text = cboFitting.SelectedItem.ToString & ": " & cboPilot.SelectedItem.ToString
                    newFitting.Tag = cboFitting.SelectedItem.ToString
                    newFitting.Checked = True
                    lvwRemoteFittings.Items.Add(newFitting)
                    Call UpdateRemoteEffects()
                Else
                    MessageBox.Show("Fitting and Pilot combination already exists!", "Duplicate Remote Setup Detected",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        End Sub

        Private Sub RemoveFittingToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles RemoveFittingToolStripMenuItem.Click
            lvwRemoteFittings.BeginUpdate()
            For Each fit As ListViewItem In lvwRemoteFittings.SelectedItems
                lvwRemoteFittings.Items.Remove(fit)
            Next
            lvwRemoteFittings.EndUpdate()
            Call UpdateRemoteEffects()
        End Sub

        Private Sub ctxRemoteFittings_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) _
            Handles ctxRemoteFittings.Opening
            If lvwRemoteFittings.SelectedItems.Count = 0 Then
                e.Cancel = True
            End If
        End Sub

#End Region

#Region "Fleet Effects"

        Private Sub LoadRemoteFleetInfo()
            ' Load details into the combo boxes
            cboPilot.BeginUpdate()
            cboFitting.BeginUpdate()
            cboSCPilot.BeginUpdate()
            cboSCShip.BeginUpdate()
            cboWCPilot.BeginUpdate()
            cboWCShip.BeginUpdate()
            cboFCPilot.BeginUpdate()
            cboFCShip.BeginUpdate()
            cboPilot.Items.Clear()
            cboFitting.Items.Clear()
            cboSCPilot.Items.Clear()
            cboSCShip.Items.Clear()
            cboWCPilot.Items.Clear()
            cboWCShip.Items.Clear()
            cboFCPilot.Items.Clear()
            cboFCShip.Items.Clear()
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
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                cboPilot.Items.Add(cPilot.Name)
                cboSCPilot.Items.Add(cPilot.Name)
                cboWCPilot.Items.Add(cPilot.Name)
                cboFCPilot.Items.Add(cPilot.Name)
            Next
            cboPilot.EndUpdate()
            cboFitting.EndUpdate()
            cboSCPilot.EndUpdate()
            cboSCShip.EndUpdate()
            cboWCPilot.EndUpdate()
            cboWCShip.EndUpdate()
            cboFCPilot.EndUpdate()
            cboFCShip.EndUpdate()
        End Sub

        Private Sub cboSCPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboSCPilot.SelectedIndexChanged
            ' Set the fleet status
            If cboSCPilot.SelectedIndex <> - 1 Then
                lblFleetStatus.Text = "Active"
                btnLeaveFleet.Enabled = True
                lblSCShip.Enabled = True
                cboSCShip.Enabled = True
                If cboSCShip.SelectedIndex = - 1 Then
                    Call CalculateFleetEffects()
                Else
                    Call UpdateScShipEffects()
                End If
            End If
        End Sub

        Private Sub cboWCPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboWCPilot.SelectedIndexChanged
            ' Set the fleet status
            If cboWCPilot.SelectedIndex <> - 1 Then
                lblFleetStatus.Text = "Active"
                btnLeaveFleet.Enabled = True
                lblWCShip.Enabled = True
                cboWCShip.Enabled = True
                If cboWCShip.SelectedIndex = - 1 Then
                    Call CalculateFleetEffects()
                Else
                    Call UpdateWcShipEffects()
                End If
            End If
        End Sub

        Private Sub cboFCPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboFCPilot.SelectedIndexChanged
            ' Set the fleet status
            If cboFCPilot.SelectedIndex <> - 1 Then
                lblFleetStatus.Text = "Active"
                btnLeaveFleet.Enabled = True
                lblFCShip.Enabled = True
                cboFCShip.Enabled = True
                If cboFCShip.SelectedIndex = - 1 Then
                    Call CalculateFleetEffects()
                Else
                    Call UpdateFcShipEffects()
                End If
            End If
        End Sub

        Private Sub btnLeaveFleet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLeaveFleet.Click
            ' Set the fleet status
            cboSCShip.SelectedIndex = - 1
            cboWCShip.SelectedIndex = - 1
            cboFCShip.SelectedIndex = - 1
            cboSCPilot.SelectedIndex = - 1
            cboWCPilot.SelectedIndex = - 1
            cboFCPilot.SelectedIndex = - 1
            cboSCShip.Enabled = False
            cboWCShip.Enabled = False
            cboFCShip.Enabled = False
            lblSCShip.Enabled = False
            lblWCShip.Enabled = False
            lblFCShip.Enabled = False
            cboSCShip.Tag = Nothing
            cboWCShip.Tag = Nothing
            cboFCShip.Tag = Nothing
            lblFleetStatus.Text = "Inactive"
            btnLeaveFleet.Enabled = False
            Call CalculateFleetEffects()
        End Sub

        Private Sub CalculateFleetSkillEffects()

            ' Add in the commander details
            Dim commanders As New List(Of String)
            If cboSCPilot.SelectedItem IsNot Nothing Then
                If chkSCActive.Checked = True Then
                    commanders.Add(cboSCPilot.SelectedItem.ToString)
                End If
            End If
            If cboWCPilot.SelectedItem IsNot Nothing Then
                If chkWCActive.Checked = True Then
                    commanders.Add(cboWCPilot.SelectedItem.ToString)
                End If
            End If
            If cboFCPilot.SelectedItem IsNot Nothing Then
                If chkFCActive.Checked = True Then
                    commanders.Add(cboFCPilot.SelectedItem.ToString)
                End If
            End If

            If commanders.Count > 0 Then
                ' Go through all fleet skills
                For Each currentSkillAttPair As KeyValuePair(Of Integer, Integer) In _fleetSkills
                    Dim skillLevel As Integer = 0
                    Dim bonusPilot As String = ""
                    Dim bonusImplant As String = ""

                    ' Armor mindlinks use a different attribute than the Armored Warfare skill
                    Dim att As Integer = currentSkillAttPair.Value
                    If att = AttributeEnum.SkillArmorHpBonus Then
                        att = AttributeEnum.SkillArmorHpBonus2
                    End If

                    ' Check each commander's skill level
                    For Each commander As String In commanders
                        Dim pilot As FittingPilot = FittingPilots.HQFPilots(commander)
                        Dim implant As ShipModule = pilot.Implant(10)

                        If implant IsNot Nothing AndAlso implant.Attributes.ContainsKey(att) Then
                            skillLevel = 6
                            bonusPilot = pilot.PilotName
                            bonusImplant = implant.Name
                        ElseIf pilot.SkillSet.ContainsKey(SkillFunctions.SkillIDToName(currentSkillAttPair.Key)) Then
                            If pilot.SkillSet(SkillFunctions.SkillIDToName(currentSkillAttPair.Key)).Level >= skillLevel _
                                Then
                                skillLevel = pilot.SkillSet(SkillFunctions.SkillIDToName(currentSkillAttPair.Key)).Level
                                bonusPilot = pilot.PilotName
                            End If
                        End If
                    Next

                    ' Add the fleet bonus item
                    If skillLevel > 0 Then
                        Dim fleetModule As New ShipModule
                        If skillLevel = 6 Then
                            ' Mindlink bonus
                            fleetModule.Name = bonusImplant & " (" & bonusPilot & ")"
                            fleetModule.Attributes.Add(currentSkillAttPair.Value,
                                                       ModuleLists.ModuleList(ModuleLists.ModuleListName(bonusImplant)).
                                                          Attributes(att))
                        Else
                            ' Skill bonus
                            fleetModule.Name = SkillFunctions.SkillIDToName(currentSkillAttPair.Key) & " (" & bonusPilot &
                                               " - Level " & skillLevel & ")"
                            fleetModule.Attributes.Add(currentSkillAttPair.Value,
                                                       SkillLists.SkillList(currentSkillAttPair.Key).Attributes(
                                                           currentSkillAttPair.Value)*skillLevel)
                        End If
                        fleetModule.ID = - 1*currentSkillAttPair.Key
                        fleetModule.ModuleState = ModuleStates.Fleet

                        ParentFitting.BaseShip.FleetSlotCollection.Add(fleetModule)
                    End If
                Next
            Else
                ParentFitting.BaseShip.FleetSlotCollection.Clear()
            End If
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Sub

        Private Sub cboSCShip_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboSCShip.SelectedIndexChanged
            Call UpdateScShipEffects()
        End Sub

        Private Sub cboWCShip_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboWCShip.SelectedIndexChanged
            Call UpdateWcShipEffects()
        End Sub

        Private Sub cboFCShip_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboFCShip.SelectedIndexChanged
            Call UpdateFcShipEffects()
        End Sub

        Private Sub chkSCActive_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles chkSCActive.CheckedChanged
            Call UpdateScShipEffects()
        End Sub

        Private Sub chkWCActive_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles chkWCActive.CheckedChanged
            Call UpdateWcShipEffects()
        End Sub

        Private Sub chkFCActive_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles chkFCActive.CheckedChanged
            Call UpdateFcShipEffects()
        End Sub

        Private Sub UpdateScShipEffects()
            If cboSCShip.SelectedIndex <> - 1 Then
                If cboSCShip.SelectedItem.ToString = "<None>" Then
                    cboSCShip.Tag = Nothing
                Else
                    If chkSCActive.Checked = False Then
                        cboSCShip.Tag = Nothing
                    Else
                        ' Let's try and generate a fitting and get some module info
                        Dim shipFit As String = cboSCShip.SelectedItem.ToString
                        If cboSCPilot.SelectedItem IsNot Nothing Then
                            If FittingPilots.HQFPilots.ContainsKey(cboSCPilot.SelectedItem.ToString) Then
                                If Fittings.FittingList.ContainsKey(shipFit) Then
                                    Dim pPilot As FittingPilot =
                                            FittingPilots.HQFPilots(cboSCPilot.SelectedItem.ToString)
                                    Dim newFit As Fitting = Fittings.FittingList(shipFit).Clone
                                    newFit.UpdateBaseShipFromFitting()
                                    newFit.PilotName = pPilot.PilotName
                                    newFit.ApplyFitting(BuildType.BuildEverything)
                                    Dim remoteShip As Ship = newFit.FittedShip
                                    Dim scModules As List(Of ShipModule)
                                    ' Check the ship bonuses for further effects (Titans use this!)
                                    scModules = GetShipGangBonusModules(remoteShip, pPilot)
                                    ' Check the modules for fleet effects
                                    For Each fleetModule As ShipModule In remoteShip.SlotCollection
                                        If _fleetGroups.Contains(CInt(fleetModule.DatabaseGroup)) = True Then
                                            fleetModule.ModuleState = ModuleStates.Gang
                                            fleetModule.SlotNo = 0
                                            scModules.Add(fleetModule)
                                        End If
                                    Next
                                    cboSCShip.Tag = scModules
                                Else
                                    cboSCShip.Tag = Nothing
                                    MessageBox.Show(
                                        "There was a mismatch of expected data. '" & shipFit &
                                        "' was not found in the collection of saved fittings.", "Data Retrieval Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            Else
                                MessageBox.Show(
                                    "There was a mismatch of expected data. '" & cboSCPilot.SelectedItem.ToString &
                                    "' was not found in the collection of pilots.", "Data Retrieval Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End If
                    End If
                End If
                Call CalculateFleetEffects()
            End If
        End Sub

        Private Sub UpdateWcShipEffects()
            If cboWCShip.SelectedIndex <> - 1 Then
                If cboWCShip.SelectedItem.ToString = "<None>" Then
                    cboWCShip.Tag = Nothing
                Else
                    If chkWCActive.Checked = False Then
                        cboWCShip.Tag = Nothing
                    Else
                        ' Let's try and generate a fitting and get some module info
                        Dim shipFit As String = cboWCShip.SelectedItem.ToString
                        If cboWCPilot.SelectedItem IsNot Nothing Then
                            If FittingPilots.HQFPilots.ContainsKey(cboWCPilot.SelectedItem.ToString) Then
                                If Fittings.FittingList.ContainsKey(shipFit) Then
                                    Dim pPilot As FittingPilot =
                                            FittingPilots.HQFPilots(cboWCPilot.SelectedItem.ToString)
                                    Dim newFit As Fitting = Fittings.FittingList(shipFit).Clone
                                    newFit.UpdateBaseShipFromFitting()
                                    newFit.PilotName = pPilot.PilotName
                                    newFit.ApplyFitting(BuildType.BuildEverything)
                                    Dim remoteShip As Ship = newFit.FittedShip
                                    Dim wcModules As New List(Of ShipModule)
                                    For Each fleetModule As ShipModule In remoteShip.SlotCollection
                                        If _fleetGroups.Contains(CInt(fleetModule.DatabaseGroup)) = True Then
                                            fleetModule.ModuleState = ModuleStates.Gang
                                            fleetModule.SlotNo = 0
                                            wcModules.Add(fleetModule)
                                        End If
                                    Next
                                    cboWCShip.Tag = wcModules
                                Else
                                    cboWCShip.Tag = Nothing
                                    MessageBox.Show(
                                        "There was a mismatch of expected data. '" & shipFit &
                                        "' was not found in the collection of saved fittings.", "Data Retrieval Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            Else
                                MessageBox.Show(
                                    "There was a mismatch of expected data. '" & cboWCPilot.SelectedItem.ToString &
                                    "' was not found in the collection of pilots.", "Data Retrieval Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End If
                    End If
                End If
                Call CalculateFleetEffects()
            End If
        End Sub

        Private Sub UpdateFcShipEffects()
            If cboFCShip.SelectedIndex <> - 1 Then
                If cboFCShip.SelectedItem.ToString = "<None>" Then
                    cboFCShip.Tag = Nothing
                Else
                    If chkFCActive.Checked = False Then
                        cboFCShip.Tag = Nothing
                    Else
                        ' Let's try and generate a fitting and get some module info
                        Dim shipFit As String = cboFCShip.SelectedItem.ToString
                        If cboFCPilot.SelectedItem IsNot Nothing Then
                            If FittingPilots.HQFPilots.ContainsKey(cboFCPilot.SelectedItem.ToString) Then
                                If Fittings.FittingList.ContainsKey(shipFit) Then
                                    Dim pPilot As FittingPilot =
                                            FittingPilots.HQFPilots(cboFCPilot.SelectedItem.ToString)
                                    Dim newFit As Fitting = Fittings.FittingList(shipFit).Clone
                                    newFit.UpdateBaseShipFromFitting()
                                    newFit.PilotName = pPilot.PilotName
                                    newFit.ApplyFitting(BuildType.BuildEverything)
                                    Dim remoteShip As Ship = newFit.FittedShip
                                    Dim fcModules As New List(Of ShipModule)
                                    For Each fleetModule As ShipModule In remoteShip.SlotCollection
                                        If _fleetGroups.Contains(CInt(fleetModule.DatabaseGroup)) = True Then
                                            fleetModule.ModuleState = ModuleStates.Gang
                                            fleetModule.SlotNo = 0
                                            fcModules.Add(fleetModule)
                                        End If
                                    Next
                                    cboFCShip.Tag = fcModules
                                Else
                                    cboFCShip.Tag = Nothing
                                    MessageBox.Show(
                                        "There was a mismatch of expected data. '" & shipFit &
                                        "' was not found in the collection of saved fittings.", "Data Retrieval Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            Else
                                MessageBox.Show(
                                    "There was a mismatch of expected data. '" & cboFCPilot.SelectedItem.ToString &
                                    "' was not found in the collection of pilots.", "Data Retrieval Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End If
                    End If
                End If
                Call CalculateFleetEffects()
            End If
        End Sub

        Private Sub CalculateFleetEffects()
            ParentFitting.BaseShip.FleetSlotCollection.Clear()
            Call CalculateFleetSkillEffects()
            Call CalculateFleetModuleEffects()
            ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
        End Sub

        Private Sub CalculateFleetModuleEffects()

            Dim fleetCollection As New SortedList(Of String, ShipModule)

            If cboSCShip.Tag IsNot Nothing Then
                Dim scModules As List(Of ShipModule) = CType(cboSCShip.Tag, List(Of ShipModule))
                For Each fleetModule As ShipModule In scModules
                    If fleetCollection.ContainsKey(fleetModule.Name) = False Then
                        ' Add it to the Fleet Collection
                        fleetCollection.Add(fleetModule.Name, fleetModule)
                    Else
                        ' See if this module improves the fleet capabilities
                        Dim compareModule As ShipModule = fleetCollection(fleetModule.Name)
                        If compareModule.Attributes.ContainsKey(AttributeEnum.ModuleCommandBonus) = True Then
                            ' Contains the Command Bonus attribute
                            If _
                                Math.Abs(CDbl(fleetModule.Attributes(AttributeEnum.ModuleCommandBonus))) >=
                                Math.Abs(CDbl(compareModule.Attributes(AttributeEnum.ModuleCommandBonus))) Then
                                fleetCollection(fleetModule.Name) = fleetModule
                            End If
                        Else
                            ' Contains the ECM Command Bonus attribute
                            If compareModule.Attributes.ContainsKey(AttributeEnum.ModuleCommandBonusECM) = True Then
                                If _
                                    Math.Abs(CDbl(fleetModule.Attributes(AttributeEnum.ModuleCommandBonusECM))) >=
                                    Math.Abs(CDbl(compareModule.Attributes(AttributeEnum.ModuleCommandBonusECM))) Then

                                    fleetCollection(fleetModule.Name) = fleetModule
                                End If
                            End If
                        End If
                    End If

                Next
            End If

            If cboWCShip.Tag IsNot Nothing Then
                Dim wcModules As List(Of ShipModule) = CType(cboWCShip.Tag, List(Of ShipModule))
                For Each fleetModule As ShipModule In wcModules
                    If fleetCollection.ContainsKey(fleetModule.Name) = False Then
                        ' Add it to the Fleet Collection
                        fleetCollection.Add(fleetModule.Name, fleetModule)
                    Else
                        ' See if this module improves the fleet capabilities
                        Dim compareModule As ShipModule = fleetCollection(fleetModule.Name)
                        If compareModule.Attributes.ContainsKey(AttributeEnum.ModuleCommandBonus) = True Then
                            ' Contains the Command Bonus attribute
                            If _
                                Math.Abs(CDbl(fleetModule.Attributes(AttributeEnum.ModuleCommandBonus))) >=
                                Math.Abs(CDbl(compareModule.Attributes(AttributeEnum.ModuleCommandBonus))) Then

                                fleetCollection(fleetModule.Name) = fleetModule
                            End If
                        Else
                            ' Contains the ECM Command Bonus attribute
                            If compareModule.Attributes.ContainsKey(AttributeEnum.ModuleCommandBonusECM) = True Then
                                If _
                                    Math.Abs(CDbl(fleetModule.Attributes(AttributeEnum.ModuleCommandBonusECM))) >=
                                    Math.Abs(CDbl(compareModule.Attributes(AttributeEnum.ModuleCommandBonusECM))) Then

                                    fleetCollection(fleetModule.Name) = fleetModule
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            If cboFCShip.Tag IsNot Nothing Then
                Dim fcModules As List(Of ShipModule) = CType(cboFCShip.Tag, List(Of ShipModule))
                For Each fleetModule As ShipModule In fcModules
                    If fleetCollection.ContainsKey(fleetModule.Name) = False Then
                        ' Add it to the Fleet Collection
                        fleetCollection.Add(fleetModule.Name, fleetModule)
                    Else
                        ' See if this module improves the fleet capabilities
                        Dim compareModule As ShipModule = fleetCollection(fleetModule.Name)
                        If compareModule.Attributes.ContainsKey(AttributeEnum.ModuleCommandBonus) = True Then
                            ' Contains the Command Bonus attribute
                            If _
                                Math.Abs(CDbl(fleetModule.Attributes(AttributeEnum.ModuleCommandBonus))) >=
                                Math.Abs(CDbl(compareModule.Attributes(AttributeEnum.ModuleCommandBonus))) Then

                                fleetCollection(fleetModule.Name) = fleetModule
                            End If
                        Else
                            ' Contains the ECM Command Bonus attribute
                            If compareModule.Attributes.ContainsKey(AttributeEnum.ModuleCommandBonusECM) = True Then
                                If _
                                    Math.Abs(CDbl(fleetModule.Attributes(AttributeEnum.ModuleCommandBonusECM))) >=
                                    Math.Abs(CDbl(compareModule.Attributes(AttributeEnum.ModuleCommandBonusECM))) Then

                                    fleetCollection(fleetModule.Name) = fleetModule
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            For Each fleetModule As ShipModule In fleetCollection.Values
                ParentFitting.BaseShip.FleetSlotCollection.Add(fleetModule)
            Next
        End Sub

        Private Function GetShipGangBonusModules(ByVal hShip As Ship, ByVal hPilot As FittingPilot) _
            As List(Of ShipModule)
            Dim fleetModules As New List(Of ShipModule)
            If hShip IsNot Nothing Then
                Dim shipRoles As List(Of ShipEffect)
                Dim hSkill As FittingSkill
                'Dim fEffect As New FinalEffect
                'Dim fEffectList As New ArrayList
                shipRoles = Engine.ShipBonusesMap(hShip.ID)
                If shipRoles IsNot Nothing Then
                    For Each chkEffect As ShipEffect In shipRoles
                        If chkEffect.Status = 16 Then
                            ' We have a gang bonus effect so create a dummy module for handling this

                            Dim gangModule As New ShipModule
                            gangModule.Name = hShip.Name & " Gang Bonus"
                            gangModule.ID = - 1
                            gangModule.SlotNo = 0
                            gangModule.ModuleState = ModuleStates.Gang
                            If hPilot.SkillSet.ContainsKey(SkillFunctions.SkillIDToName(chkEffect.AffectingID)) = True _
                                Then
                                hSkill = hPilot.SkillSet(SkillFunctions.SkillIDToName(chkEffect.AffectingID))
                                If chkEffect.IsPerLevel = True Then
                                    gangModule.Attributes.Add(chkEffect.AffectedAtt, chkEffect.Value*hSkill.Level)
                                Else
                                    gangModule.Attributes.Add(chkEffect.AffectedAtt, chkEffect.Value)
                                End If
                            Else
                                gangModule.Attributes.Add(chkEffect.AffectedAtt, chkEffect.Value)
                            End If
                            fleetModules.Add(gangModule)

                        End If
                    Next
                End If
            End If
            Return fleetModules
        End Function

#End Region

#Region "Ship Skill Context Menu"

        Private Sub ctxShipSkills_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) _
            Handles ctxShipSkills.Opening
            ' Check for Relevant Skills in Modules/Charges
            Dim relGlobalSkills As New ArrayList
            Dim shipSkills As New ArrayList
            Dim affects(10) As String
            ctxShipSkills.Items.Clear()
            If ParentFitting.BaseShip.GlobalAffects IsNot Nothing Then
                For Each affect As String In ParentFitting.BaseShip.GlobalAffects
                    If affect.Contains(";Skill;") = True Then
                        affects = affect.Split((";").ToCharArray)
                        If relGlobalSkills.Contains(affects(0)) = False Then
                            relGlobalSkills.Add(affects(0))
                        End If
                    End If
                    If affect.Contains(";Ship Bonus;") = True Then
                        affects = affect.Split((";").ToCharArray)
                        If ParentFitting.ShipName = affects(0) Then
                            If relGlobalSkills.Contains(affects(3)) = False Then
                                relGlobalSkills.Add(affects(3))
                            End If
                        End If
                    End If
                    If affect.Contains(";Subsystem;") = True Then
                        affects = affect.Split((";").ToCharArray)
                        If relGlobalSkills.Contains(affects(3)) = False Then
                            relGlobalSkills.Add(affects(3))
                        End If
                    End If
                Next
            End If
            relGlobalSkills.Sort()
            For Each affect As String In ParentFitting.BaseShip.Affects
                If affect.Contains(";Skill;") = True Then
                    affects = affect.Split((";").ToCharArray)
                    If shipSkills.Contains(affects(0)) = False Then
                        shipSkills.Add(affects(0))
                    End If
                End If
                If affect.Contains(";Ship Bonus;") = True Then
                    affects = affect.Split((";").ToCharArray)
                    If ParentFitting.ShipName = affects(0) Then
                        If shipSkills.Contains(affects(3)) = False Then
                            shipSkills.Add(affects(3))
                        End If
                    End If
                End If
                If affect.Contains(";Subsystem;") = True Then
                    affects = affect.Split((";").ToCharArray)
                    If shipSkills.Contains(affects(3)) = False Then
                        shipSkills.Add(affects(3))
                    End If
                End If
            Next
            shipSkills.Sort()

            ' Add the Main menu item
            Dim alterRelevantSkills As New ToolStripMenuItem
            alterRelevantSkills.Name = ParentFitting.BaseShip.Name
            alterRelevantSkills.Text = "Alter Relevant Skills"

            ' Add the bonus skills
            If relGlobalSkills.Count > 0 Then
                For Each relSkill As String In relGlobalSkills
                    Dim newRelSkill As New ToolStripMenuItem
                    newRelSkill.Name = relSkill
                    newRelSkill.Text = relSkill
                    Dim pilotLevel As Integer = 0
                    If _
                        FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).SkillSet.ContainsKey(
                            relSkill) Then
                        pilotLevel =
                            FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).SkillSet(relSkill).
                                Level
                    Else
                        MessageBox.Show(
                            "There is a mis-match of roles for the " & ParentFitting.BaseShip.Name &
                            ". Please report this to the EveHQ Developers.", "Ship Role Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
                    End If
                    newRelSkill.Image = CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString), 
                                              Image)
                    For skillLevel As Integer = 0 To 5
                        Dim newRelSkillLevel As New ToolStripMenuItem
                        newRelSkillLevel.Name = relSkill & skillLevel.ToString
                        newRelSkillLevel.Text = "Level " & skillLevel.ToString
                        If skillLevel = pilotLevel Then
                            newRelSkillLevel.Checked = True
                        End If
                        AddHandler newRelSkillLevel.Click, AddressOf SetPilotSkillLevel
                        newRelSkill.DropDownItems.Add(newRelSkillLevel)
                    Next
                    newRelSkill.DropDownItems.Add("-")
                    Dim defaultLevel As Integer = 0
                    If _
                        HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills.ContainsKey(
                            relSkill) = True Then
                        defaultLevel =
                            HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills(relSkill).Level
                    End If
                    Dim newRelSkillDefault As New ToolStripMenuItem
                    newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                    newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                    AddHandler newRelSkillDefault.Click, AddressOf SetPilotSkillLevel
                    newRelSkill.DropDownItems.Add(newRelSkillDefault)
                    alterRelevantSkills.DropDownItems.Add(newRelSkill)
                Next
            End If

            ' Add a divider if relevant
            If relGlobalSkills.Count > 0 And shipSkills.Count > 0 Then
                alterRelevantSkills.DropDownItems.Add("-")
            End If

            ' Add the ship skills
            If shipSkills.Count > 0 Then
                For Each shipskill As String In shipSkills
                    Dim newShipSkill As New ToolStripMenuItem
                    newShipSkill.Name = shipskill
                    newShipSkill.Text = shipskill
                    Dim pilotLevel As Integer = 0
                    If _
                        FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).SkillSet.ContainsKey(
                            shipskill) Then
                        pilotLevel =
                            FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).SkillSet(shipskill).
                                Level
                    Else
                        MessageBox.Show(
                            "There is a mis-match of roles for the " & ParentFitting.BaseShip.Name &
                            ". Please report this to the EveHQ Developers.", "Ship Role Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
                    End If
                    newShipSkill.Image = CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString), 
                                               Image)
                    For skillLevel As Integer = 0 To 5
                        Dim newRelSkillLevel As New ToolStripMenuItem
                        newRelSkillLevel.Name = shipskill & skillLevel.ToString
                        newRelSkillLevel.Text = "Level " & skillLevel.ToString
                        If skillLevel = pilotLevel Then
                            newRelSkillLevel.Checked = True
                        End If
                        AddHandler newRelSkillLevel.Click, AddressOf SetPilotSkillLevel
                        newShipSkill.DropDownItems.Add(newRelSkillLevel)
                    Next
                    newShipSkill.DropDownItems.Add("-")
                    Dim defaultLevel As Integer = 0
                    If _
                        HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills.ContainsKey(
                            shipskill) = True Then
                        defaultLevel =
                            HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills(shipskill).
                                Level
                    End If
                    Dim newRelSkillDefault As New ToolStripMenuItem
                    newRelSkillDefault.Name = shipskill & defaultLevel.ToString
                    newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                    AddHandler newRelSkillDefault.Click, AddressOf SetPilotSkillLevel
                    newShipSkill.DropDownItems.Add(newRelSkillDefault)
                    alterRelevantSkills.DropDownItems.Add(newShipSkill)
                Next
            End If

            ctxShipSkills.Items.Add(alterRelevantSkills)
        End Sub

#End Region

#Region "Wormhole Effects"

        Private Sub LoadWormholeInfo()
            cboWHEffect.Items.Clear()
            cboWHEffect.Items.Add("<None>")
            cboWHEffect.Items.Add("Black Hole")
            cboWHEffect.Items.Add("Cataclysmic Variable")
            cboWHEffect.Items.Add("Magnetar")
            cboWHEffect.Items.Add("Pulsar")
            cboWHEffect.Items.Add("Red Giant")
            cboWHEffect.Items.Add("Wolf Rayet")
            cboWHEffect.Items.Add("Incursion - Assault")
            cboWHEffect.Items.Add("Incursion - HQ")
            cboWHEffect.Items.Add("Incursion - Vanguard")
            cboWHClass.Items.Clear()
            cboWHClass.Items.Add("1")
            cboWHClass.Items.Add("2")
            cboWHClass.Items.Add("3")
            cboWHClass.Items.Add("4")
            cboWHClass.Items.Add("5")
            cboWHClass.Items.Add("6")
        End Sub

        Private Sub UpdateWormholeUi()
            cboWHEffect.SelectedItem = ParentFitting.WHEffect
            cboWHClass.SelectedItem = ParentFitting.WHLevel.ToString
        End Sub

        Private Sub cboWHEffect_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboWHEffect.SelectedIndexChanged
            If _updateAll = False Then
                ' Clear the current effect
                ParentFitting.BaseShip.EnviroSlotCollection.Clear()
                ' Set the WH Class combo if it's not activated
                If cboWHEffect.SelectedIndex > 0 Then
                    ParentFitting.WHEffect = cboWHEffect.SelectedItem.ToString
                    If cboWHClass.SelectedIndex = - 1 Then
                        cboWHClass.SelectedIndex = 0
                        Exit Sub
                    Else
                        Dim modName As String
                        If cboWHEffect.SelectedItem.ToString = "Red Giant" Then
                            modName = cboWHEffect.SelectedItem.ToString & " Beacon Class " &
                                      cboWHClass.SelectedItem.ToString
                        Else
                            If cboWHEffect.SelectedItem.ToString.StartsWith("Incursion") Then
                                modName = cboWHEffect.SelectedItem.ToString.Replace("-", "ship attributes effects")
                            Else
                                modName = cboWHEffect.SelectedItem.ToString & " Effect Beacon Class " &
                                          cboWHClass.SelectedItem.ToString
                            End If
                        End If
                        Dim modID As Integer = ModuleLists.ModuleListName(modName)
                        Dim eMod As ShipModule = ModuleLists.ModuleList(modID).Clone
                        ParentFitting.BaseShip.EnviroSlotCollection.Add(eMod)
                    End If
                Else
                    ParentFitting.WHEffect = ""
                End If
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            End If
        End Sub

        Private Sub cboWHClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboWHClass.SelectedIndexChanged
            If _updateAll = False Then
                If cboWHEffect.SelectedIndex > 0 Then
                    ' Clear the current effect
                    ParentFitting.BaseShip.EnviroSlotCollection.Clear()
                    Dim modName As String
                    If cboWHEffect.SelectedItem.ToString = "Red Giant" Then
                        modName = cboWHEffect.SelectedItem.ToString & " Beacon Class " &
                                  cboWHClass.SelectedItem.ToString
                    Else
                        If cboWHEffect.SelectedItem.ToString.StartsWith("Incursion") Then
                            modName = cboWHEffect.SelectedItem.ToString.Replace("-", "ship attributes effects")
                        Else
                            modName = cboWHEffect.SelectedItem.ToString & " Effect Beacon Class " &
                                      cboWHClass.SelectedItem.ToString
                        End If
                    End If
                    Dim modID As Integer = ModuleLists.ModuleListName(modName)
                    Dim eMod As ShipModule = ModuleLists.ModuleList(modID).Clone
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
            cboBoosterSlot1.BeginUpdate()
            cboBoosterSlot1.Items.Clear()
            cboBoosterSlot2.BeginUpdate()
            cboBoosterSlot2.Items.Clear()
            cboBoosterSlot3.BeginUpdate()
            cboBoosterSlot3.Items.Clear()
            For Each booster As ShipModule In Boosters.BoosterList.Values
                Select Case CInt(booster.Attributes(1087))
                    Case 1
                        cboBoosterSlot1.Items.Add(booster.Name)
                    Case 2
                        cboBoosterSlot2.Items.Add(booster.Name)
                    Case 3
                        cboBoosterSlot3.Items.Add(booster.Name)
                End Select
            Next
            cboBoosterSlot1.EndUpdate()
            cboBoosterSlot2.EndUpdate()
            cboBoosterSlot3.EndUpdate()
        End Sub

        Private Sub UpdateBoosterSlots()
            _updateBoosters = True
            For Each booster As ShipModule In ParentFitting.BaseShip.BoosterSlotCollection
                Dim slot As Integer = CInt(booster.Attributes(1087))
                Dim cb As ComboBox = CType(tcStorage.Controls("tcpBoosters").Controls("cboBoosterSlot" & slot.ToString),
                                           ComboBox)
                If cb.Items.Contains(booster.Name) = True Then
                    cb.SelectedItem = booster.Name
                End If
            Next
            _updateBoosters = False
        End Sub

        Private Sub cboBoosterSlots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboBoosterSlot1.SelectedIndexChanged, cboBoosterSlot2.SelectedIndexChanged,
                    cboBoosterSlot3.SelectedIndexChanged
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
                Dim boosterID As Integer = ModuleLists.ModuleListName(boosterName)
                Dim bModule As ShipModule = ModuleLists.ModuleList(boosterID).Clone
                cb.Tag = bModule
                ToolTip1.SetToolTip(cb, SquishText(bModule.Description))
                'Dim effects As SortedList(Of String, BoosterEffect) = Boosters.BoosterEffects(CStr(boosterID))
                'Dim effectList As String = "Penalties: "
                'Dim count As Integer = 0
                'If effects IsNot Nothing Then
                '    For Each bEffect As BoosterEffect In effects.Values
                '        effectList &= bEffect.AttributeEffect & ", "
                '    Next
                'End If
                tcpBoosters.Refresh()
                bModule.ImplantSlot = 15
                Call ApplyBoosters(cb, bt, idx)
                bt.Enabled = True
            Else
                bt.Enabled = False
            End If
        End Sub

        Private Sub ApplyBoosters(ByVal cb As ComboBox, ByVal parentButton As ButtonX, ByVal idx As Integer)
            If _updateBoosters = False Then
                ParentFitting.BaseShip.BoosterSlotCollection.Clear()
                For slot As Integer = 1 To 3
                    Dim cbo As ComboBox =
                            CType(tcStorage.Controls("tcpBoosters").Controls("cboBoosterSlot" & slot.ToString), ComboBox)
                    If cbo.Tag IsNot Nothing Then
                        ParentFitting.BaseShip.BoosterSlotCollection.Add(CType(cbo.Tag, ShipModule))
                    End If
                Next
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
            End If
            BuildBoosterSkills(cb, parentButton, idx)
        End Sub

#End Region

#Region "Public Update Routines"

        Public Sub UpdateDroneBay()
            _updateDrones = True
            Call RedrawDroneBay()
            _updateDrones = False
            Call UpdatePrices()
            Call ParentFitting.UpdateFittingFromBaseShip()
        End Sub

        Public Sub UpdateItemBay()
            Call RedrawCargoBay()
            Call UpdatePrices()
            Call ParentFitting.UpdateFittingFromBaseShip()
        End Sub

        Public Sub UpdateShipBay()
            Call RedrawShipBay()
            Call ParentFitting.UpdateFittingFromBaseShip()
        End Sub

#End Region

#Region "Undo Stuff"

        Public Sub UpdateHistory()

            ' Update the history tab
            lvwHistory.BeginUpdate()
            lvwHistory.Items.Clear()
            For Each ui As UndoInfo In UndoStack
                Dim newUi As New ListViewItem
                newUi.Text = ui.Transaction.ToString
                newUi.SubItems.Add(ui.SlotType.ToString)
                newUi.SubItems.Add(ui.OldSlotNo.ToString)
                newUi.SubItems.Add(ui.OldModName.ToString)
                newUi.SubItems.Add(ui.OldChargeName.ToString)
                newUi.SubItems.Add(ui.NewSlotNo.ToString)
                newUi.SubItems.Add(ui.NewModName.ToString)
                newUi.SubItems.Add(ui.NewChargeName.ToString)
                newUi.SubItems.Add(ui.ChargeOnly.ToString)
                lvwHistory.Items.Add(newUi)
            Next
            lvwHistory.EndUpdate()

            ' Update the Undo button subitems
            For Each bi As ButtonItem In btnUndo.SubItems
                RemoveHandler bi.Click, AddressOf UndoSubItems
            Next
            btnUndo.SubItems.Clear()
            Dim idx As Integer = 0
            For Each ui As UndoInfo In UndoStack
                idx += 1
                Dim bi As New ButtonItem
                Select Case ui.Transaction
                    Case UndoInfo.TransType.AddCharge
                        bi.Text = ui.Transaction.ToString & " - " & ui.NewChargeName.ToString
                    Case UndoInfo.TransType.AddModule
                        bi.Text = ui.Transaction.ToString & " - " & ui.NewModName.ToString
                    Case UndoInfo.TransType.RemoveCharge
                        bi.Text = ui.Transaction.ToString & " - " & ui.OldChargeName.ToString
                    Case UndoInfo.TransType.RemoveModule
                        bi.Text = ui.Transaction.ToString & " - " & ui.OldModName.ToString
                    Case UndoInfo.TransType.ReplacedModule
                        bi.Text = ui.Transaction.ToString & " - " & ui.OldModName.ToString & " -> " &
                                  ui.NewModName.ToString
                    Case UndoInfo.TransType.SwapModules
                        bi.Text = ui.Transaction.ToString & " - " & ui.OldModName.ToString & " -> " &
                                  ui.NewModName.ToString
                End Select
                bi.ImageFixedSize = New Size(20, 20)
                bi.Image = My.Resources.imgShield
                bi.Name = idx.ToString
                AddHandler bi.Click, AddressOf UndoSubItems
                btnUndo.SubItems.Add(bi)
                ' Exit if we have exceeded 10 items
                If idx = 10 Then Exit For
            Next
            btnUndo.RecalcLayout()

            ' Update the Redo button subitems
            For Each bi As ButtonItem In btnRedo.SubItems
                RemoveHandler bi.Click, AddressOf RedoSubItems
            Next
            btnRedo.SubItems.Clear()
            idx = 0
            For Each ui As UndoInfo In RedoStack
                idx += 1
                Dim bi As New ButtonItem
                Select Case ui.Transaction
                    Case UndoInfo.TransType.AddCharge
                        bi.Text = ui.Transaction.ToString & " - " & ui.NewChargeName.ToString
                    Case UndoInfo.TransType.AddModule
                        bi.Text = ui.Transaction.ToString & " - " & ui.NewModName.ToString
                    Case UndoInfo.TransType.RemoveCharge
                        bi.Text = ui.Transaction.ToString & " - " & ui.OldChargeName.ToString
                    Case UndoInfo.TransType.RemoveModule
                        bi.Text = ui.Transaction.ToString & " - " & ui.OldModName.ToString
                    Case UndoInfo.TransType.ReplacedModule
                        bi.Text = ui.Transaction.ToString & " - " & ui.OldModName.ToString & " -> " &
                                  ui.NewModName.ToString
                    Case UndoInfo.TransType.SwapModules
                        bi.Text = ui.Transaction.ToString & " - " & ui.OldModName.ToString & " -> " &
                                  ui.NewModName.ToString
                End Select
                bi.ImageFixedSize = New Size(20, 20)
                bi.Image = My.Resources.imgShield
                bi.Name = idx.ToString
                AddHandler bi.Click, AddressOf RedoSubItems
                btnRedo.SubItems.Add(bi)
                ' Exit if we have exceeded 10 items
                If idx = 10 Then Exit For
            Next
            btnRedo.RecalcLayout()
        End Sub

        Public Sub PerformUndo(ByVal levels As Integer)
            ' Take the first x levels off the stack and reapply them
            If UndoStack.Count > 0 Then
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                For level As Integer = 1 To Math.Min(levels, UndoStack.Count)
                    Dim ui As UndoInfo = UndoStack.Pop
                    Dim slotNode As Node = adtSlots.FindNodeByName(ui.SlotType & "_" & ui.NewSlotNo)
                    ' Reverse the transaction based on the type
                    Select Case ui.Transaction
                        Case UndoInfo.TransType.AddCharge
                            ' Need to check if the old charge was blank prior to adding
                            If ui.OldChargeName = "" Then
                                Call RemoveSingleChargeFromModule(slotNode, True)
                            Else
                                Dim chargeID As Integer = ModuleLists.ModuleListName(ui.OldChargeName)
                                Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                                Call LoadSingleChargeIntoModule(slotNode, charge, True)
                            End If
                        Case UndoInfo.TransType.RemoveCharge
                            ' Need to add the charge
                            Dim chargeID As Integer = ModuleLists.ModuleListName(ui.OldChargeName)
                            Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                            Call LoadSingleChargeIntoModule(slotNode, charge, True)
                        Case UndoInfo.TransType.AddModule
                            ' Need to check if the slot was blank prior to adding
                            If ui.OldModName = "" Then
                                Call RemoveModule(slotNode, True, True)
                            Else
                                ' Need to add the module back
                                Dim oldModID As Integer = ModuleLists.ModuleListName(ui.OldModName)
                                Dim oldMod As ShipModule = ModuleLists.ModuleList.Item(oldModID).Clone
                                If ui.OldChargeName <> "" Then
                                    Dim chargeID As Integer = ModuleLists.ModuleListName(ui.OldChargeName)
                                    Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                                    oldMod.LoadedCharge = charge
                                End If
                                Call ParentFitting.AddModule(oldMod, ui.OldSlotNo, True, False, Nothing, True, False)
                            End If
                        Case UndoInfo.TransType.RemoveModule
                            ' Need to add the module back
                            Dim oldModID As Integer = ModuleLists.ModuleListName(ui.OldModName)
                            Dim oldMod As ShipModule = ModuleLists.ModuleList.Item(oldModID).Clone
                            If ui.OldChargeName <> "" Then
                                Dim chargeID As Integer = ModuleLists.ModuleListName(ui.OldChargeName)
                                Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                                oldMod.LoadedCharge = charge
                            End If
                            Call ParentFitting.AddModule(oldMod, ui.OldSlotNo, True, False, Nothing, True, False)
                        Case UndoInfo.TransType.SwapModules, UndoInfo.TransType.ReplacedModule
                            ' Swap modules back to their original positions
                            Dim isSwapping As Boolean = ui.Transaction = UndoInfo.TransType.SwapModules
                            Dim oldMod As ShipModule = Nothing
                            If ui.OldModName <> "" Then
                                Dim oldModID As Integer = ModuleLists.ModuleListName(ui.OldModName)
                                oldMod = ModuleLists.ModuleList.Item(oldModID).Clone
                                If ui.OldChargeName <> "" Then
                                    Dim chargeID As Integer = ModuleLists.ModuleListName(ui.OldChargeName)
                                    Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                                    oldMod.LoadedCharge = charge
                                End If
                            End If
                            Dim newMod As ShipModule = Nothing
                            If ui.NewModName <> "" Then ' = not empty
                                Dim newModID As Integer = ModuleLists.ModuleListName(ui.NewModName)
                                newMod = ModuleLists.ModuleList.Item(newModID).Clone
                                If ui.NewChargeName <> "" Then
                                    Dim chargeID As Integer = ModuleLists.ModuleListName(ui.NewChargeName)
                                    Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                                    newMod.LoadedCharge = charge
                                End If
                            End If
                            If oldMod IsNot Nothing Then
                                Call _
                                    ParentFitting.AddModule(oldMod, ui.OldSlotNo, True, False, newMod, True, isSwapping)
                            Else
                                Call RemoveModule(adtSlots.FindNodeByName(ui.SlotType & "_" & ui.OldSlotNo), True, True)
                            End If
                            If isSwapping = True Then
                                If newMod IsNot Nothing Then
                                    Call ParentFitting.AddModule(newMod, ui.NewSlotNo, True, False, oldMod, True, True)
                                Else
                                    Call _
                                        RemoveModule(adtSlots.FindNodeByName(ui.SlotType & "_" & ui.NewSlotNo), True,
                                                     True)
                                End If
                            End If
                    End Select
                    ' Put this onto the Redo stack
                    RedoStack.Push(ui)
                Next
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                Call UpdateHistory()
            End If
        End Sub

        Public Sub PerformRedo(ByVal levels As Integer)
            ' Take the first x levels off the stack and reapply them
            If RedoStack.Count > 0 Then
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                For level As Integer = 1 To Math.Min(levels, RedoStack.Count)
                    Dim ui As UndoInfo = RedoStack.Pop
                    Dim slotNode As Node = adtSlots.FindNodeByName(ui.SlotType & "_" & ui.NewSlotNo)
                    ' Reverse the transaction based on the type
                    Select Case ui.Transaction
                        Case UndoInfo.TransType.AddCharge
                            ' Reperform the charge loading
                            Dim chargeID As Integer = ModuleLists.ModuleListName(ui.NewChargeName)
                            Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                            Call LoadSingleChargeIntoModule(slotNode, charge, True)
                        Case UndoInfo.TransType.RemoveCharge
                            ' Reperform the charge removal
                            Call RemoveSingleChargeFromModule(slotNode, True)
                        Case UndoInfo.TransType.AddModule
                            ' Need to add the module again
                            Dim newModID As Integer = ModuleLists.ModuleListName(ui.NewModName)
                            Dim newMod As ShipModule = ModuleLists.ModuleList.Item(newModID).Clone
                            If ui.NewChargeName <> "" Then
                                Dim chargeID As Integer = ModuleLists.ModuleListName(ui.NewChargeName)
                                Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                                newMod.LoadedCharge = charge
                            End If
                            Call ParentFitting.AddModule(newMod, ui.NewSlotNo, True, False, Nothing, True, False)
                        Case UndoInfo.TransType.RemoveModule
                            ' Need to remove the module again
                            Call RemoveModule(slotNode, True, True)
                        Case UndoInfo.TransType.SwapModules, UndoInfo.TransType.ReplacedModule
                            ' Swap modules back to their original positions
                            Dim isSwapping As Boolean = ui.Transaction = UndoInfo.TransType.SwapModules
                            Dim oldMod As ShipModule = Nothing
                            If ui.OldModName <> "" Then
                                Dim oldModID As Integer = ModuleLists.ModuleListName(ui.OldModName)
                                oldMod = ModuleLists.ModuleList.Item(oldModID).Clone
                                If ui.OldChargeName <> "" Then
                                    Dim chargeID As Integer = ModuleLists.ModuleListName(ui.OldChargeName)
                                    Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                                    oldMod.LoadedCharge = charge
                                End If
                            End If
                            Dim newMod As ShipModule = Nothing
                            If ui.NewModName <> "" Then ' = not empty
                                Dim newModID As Integer = ModuleLists.ModuleListName(ui.NewModName)
                                newMod = ModuleLists.ModuleList.Item(newModID).Clone
                                If ui.NewChargeName <> "" Then
                                    Dim chargeID As Integer = ModuleLists.ModuleListName(ui.NewChargeName)
                                    Dim charge As ShipModule = ModuleLists.ModuleList.Item(chargeID).Clone
                                    newMod.LoadedCharge = charge
                                End If
                            End If
                            If isSwapping = True Then
                                If oldMod IsNot Nothing Then
                                    Call ParentFitting.AddModule(oldMod, ui.NewSlotNo, True, False, newMod, True, True)
                                Else
                                    Call _
                                        RemoveModule(adtSlots.FindNodeByName(ui.SlotType & "_" & ui.NewSlotNo), True,
                                                     True)
                                End If
                            End If
                            If newMod IsNot Nothing Then
                                Call _
                                    ParentFitting.AddModule(newMod, ui.OldSlotNo, True, False, oldMod, True, isSwapping)
                            Else
                                Call RemoveModule(adtSlots.FindNodeByName(ui.SlotType & "_" & ui.OldSlotNo), True, True)
                            End If
                    End Select
                    ' Put this back onto the undo stack
                    UndoStack.Push(ui)
                Next
                ParentFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                Call UpdateHistory()
            End If
        End Sub

        Private Sub btnUndo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUndo.Click
            ' Undo the last action
            Call PerformUndo(1)
        End Sub

        Private Sub btnRedo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRedo.Click
            ' Redo the last action
            Call PerformRedo(1)
        End Sub

        Private Sub UndoSubItems(ByVal sender As Object, ByVal e As EventArgs)
            Dim bi As ButtonItem = CType(sender, ButtonItem)
            Call PerformUndo(CInt(bi.Name))
        End Sub

        Private Sub RedoSubItems(ByVal sender As Object, ByVal e As EventArgs)
            Dim bi As ButtonItem = CType(sender, ButtonItem)
            Call PerformRedo(CInt(bi.Name))
        End Sub

#End Region

#Region "Booster UI Regions"

        Private Sub btnShowInfo1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowInfo1.Click
            Call ShowBoosterInfo(cboBoosterSlot1)
        End Sub

        Private Sub btnRemoveBooster1_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles btnRemoveBooster1.Click
            Call RemoveBooster(cboBoosterSlot1, btnBoosterSlot1, 1)
        End Sub

        Private Sub btnShowInfo2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowInfo2.Click
            Call ShowBoosterInfo(cboBoosterSlot2)
        End Sub

        Private Sub btnRemoveBooster2_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles btnRemoveBooster2.Click
            Call RemoveBooster(cboBoosterSlot2, btnBoosterSlot2, 2)
        End Sub

        Private Sub btnShowInfo3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowInfo3.Click
            Call ShowBoosterInfo(cboBoosterSlot3)
        End Sub

        Private Sub btnRemoveBooster3_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles btnRemoveBooster3.Click
            Call RemoveBooster(cboBoosterSlot3, btnBoosterSlot3, 3)
        End Sub

        Private Sub BuildBoosterSkills(ByVal cb As ComboBox, ByVal parentButton As ButtonX, ByVal idx As Integer)

            If cb.Tag IsNot Nothing Then
                Dim bModule As ShipModule = CType(cb.Tag, ShipModule)
                'Dim boosterName As String = cb.SelectedItem.ToString
                'Dim boosterID As String = CStr(ModuleLists.ModuleListName(boosterName))
                ' Check for related skills
                Dim relModuleSkills As New List(Of String)
                Dim affects(3) As String
                For Each affect As String In bModule.Affects
                    If affect.Contains(";Skill;") = True Then
                        affects = affect.Split((";").ToCharArray)
                        If relModuleSkills.Contains(affects(0)) = False Then
                            relModuleSkills.Add(affects(0))
                        End If
                    End If
                    If affect.Contains(";Ship Bonus;") = True Then
                        affects = affect.Split((";").ToCharArray)
                        If ParentFitting.ShipName = affects(0) Then
                            If relModuleSkills.Contains(affects(3)) = False Then
                                relModuleSkills.Add(affects(3))
                            End If
                        End If
                    End If
                    If affect.Contains(";Subsystem;") = True Then
                        affects = affect.Split((";").ToCharArray)
                        If relModuleSkills.Contains(affects(3)) = False Then
                            relModuleSkills.Add(affects(3))
                        End If
                    End If
                Next
                relModuleSkills.Sort()
                If relModuleSkills.Count > 0 Then
                    ' Add the Main menu item
                    parentButton.SubItems("btnAlterSkills" & idx.ToString).Text = "Alter Relevant Skills"
                    parentButton.SubItems("btnAlterSkills" & idx.ToString).SubItems.Clear()
                    For Each relSkill As String In relModuleSkills
                        Dim newRelSkill As New ButtonItem
                        newRelSkill.Name = relSkill
                        newRelSkill.Text = relSkill
                        Dim pilotLevel As Integer = 0
                        If _
                            FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).SkillSet.ContainsKey(
                                relSkill) Then
                            pilotLevel =
                                FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString).SkillSet(relSkill) _
                                    .Level
                        Else
                            MessageBox.Show(
                                "There is a mis-match of roles for the " & ParentFitting.BaseShip.Name &
                                ". Please report this to the EveHQ Developers.", "Ship Role Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                        End If
                        newRelSkill.Image = CType(My.Resources.ResourceManager.GetObject("Level" & pilotLevel.ToString),
                                                  Image)
                        For skillLevel As Integer = 0 To 5
                            Dim newRelSkillLevel As New ButtonItem
                            newRelSkillLevel.Name = relSkill & skillLevel.ToString
                            newRelSkillLevel.Text = "Level " & skillLevel.ToString
                            If skillLevel = pilotLevel Then
                                newRelSkillLevel.Checked = True
                            End If
                            AddHandler newRelSkillLevel.Click, AddressOf SetPilotBoosterSkillLevel
                            newRelSkill.SubItems.Add(newRelSkillLevel)
                        Next
                        Dim defaultLevel As Integer = 0
                        If _
                            HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills.ContainsKey(
                                relSkill) = True Then
                            defaultLevel =
                                HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString).PilotSkills(relSkill).
                                    Level
                        Else
                        End If
                        Dim newRelSkillDefault As New ButtonItem
                        newRelSkillDefault.BeginGroup = True
                        newRelSkillDefault.Name = relSkill & defaultLevel.ToString
                        newRelSkillDefault.Text = "Actual (Level " & defaultLevel.ToString & ")"
                        AddHandler newRelSkillDefault.Click, AddressOf SetPilotBoosterSkillLevel
                        newRelSkill.SubItems.Add(newRelSkillDefault)
                        parentButton.SubItems("btnAlterSkills" & idx.ToString).SubItems.Add(newRelSkill)
                    Next
                End If
            End If
        End Sub

        Private Sub ShowBoosterInfo(ByVal cb As ComboBox)
            If cb IsNot Nothing Then
                Dim sModule As ShipModule = CType(cb.Tag, ShipModule)
                For Each bModule As ShipModule In ParentFitting.FittedShip.BoosterSlotCollection
                    If sModule.Name = bModule.Name Then
                        Dim showInfo As New FrmShowInfo
                        Dim hPilot As EveHQPilot
                        If _currentInfo IsNot Nothing Then
                            hPilot = HQ.Settings.Pilots(_currentInfo.cboPilots.SelectedItem.ToString)
                        Else
                            If HQ.Settings.StartupPilot <> "" Then
                                hPilot = HQ.Settings.Pilots(HQ.Settings.StartupPilot)
                            Else
                                hPilot = HQ.Settings.Pilots.Values(0)
                            End If
                        End If
                        showInfo.ShowItemDetails(bModule, hPilot)
                    End If
                Next
            End If
        End Sub

        Private Sub RemoveBooster(ByVal cb As ComboBox, ByVal parentButton As ButtonX, ByVal buttonIdx As Integer)
            If cb IsNot Nothing Then
                cb.SelectedIndex = - 1
                cb.Tag = Nothing
                ToolTip1.SetToolTip(cb, "")
                'Dim cbidx As Integer = CInt(cb.Name.Substring(cb.Name.Length - 1, 1))
                tcpBoosters.Refresh()
                Call ApplyBoosters(cb, parentButton, buttonIdx)
            End If
        End Sub

        Private Sub SetPilotBoosterSkillLevel(ByVal sender As Object, ByVal e As EventArgs)
            Dim mnuPilotLevel As ButtonItem = CType(sender, ButtonItem)
            Dim hPilot As FittingPilot = FittingPilots.HQFPilots(_currentInfo.cboPilots.SelectedItem.ToString)
            Dim pilotSkill As FittingSkill = hPilot.SkillSet(mnuPilotLevel.Name.Substring(0,
                                                                                          mnuPilotLevel.Name.Length - 1))
            Dim level As Integer = CInt(mnuPilotLevel.Name.Substring(mnuPilotLevel.Name.Length - 1))
            If level <> pilotSkill.Level Then
                pilotSkill.Level = level
                ParentFitting.ApplyFitting(BuildType.BuildEverything)
            End If
            ' Trigger an update of all open ship fittings!
            HQFEvents.StartUpdateShipInfo = hPilot.PilotName
            ' Rebuild all the menus for the boosters
            Call BuildBoosterSkills(cboBoosterSlot1, btnBoosterSlot1, 1)
            Call BuildBoosterSkills(cboBoosterSlot2, btnBoosterSlot2, 2)
            Call BuildBoosterSkills(cboBoosterSlot3, btnBoosterSlot3, 3)
        End Sub

#End Region

#Region "Notes & Tags Methods"

        Private Sub UpdateNotes()
            txtNotes.Text = ParentFitting.Notes
            rateFitting.Rating = ParentFitting.Rating
        End Sub

        Private Sub UpdateTags()
            If ParentFitting.Tags Is Nothing Then
                ParentFitting.Tags = New List(Of String)
            End If
            ParentFitting.Tags.Sort()
            lblTags.Text = ""
            For Each shipTag As String In ParentFitting.Tags
                lblTags.Text &= "<a href='" & shipTag & "'>" & shipTag & "</a>   "
            Next
        End Sub

        Private Sub txtNotes_TextChanged(sender As Object, e As EventArgs) Handles txtNotes.TextChanged
            If ParentFitting IsNot Nothing Then
                ParentFitting.Notes = txtNotes.Text
            End If
        End Sub

        Private Sub txtAddTag_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAddTag.KeyDown
            If e.KeyCode = Keys.Enter Then
                Dim tagList As List(Of String) = txtAddTag.Text.Trim.Split(" ".ToCharArray).ToList
                For Each shipTag As String In tagList
                    If String.IsNullOrWhiteSpace(shipTag.Trim) = False Then
                        If ParentFitting.Tags.Contains(shipTag.Trim) = False Then
                            ParentFitting.Tags.Add(shipTag.Trim)
                        End If
                    End If
                Next
                txtAddTag.Text = ""
                ParentFitting.Tags.Sort()
                UpdateTags()
            End If
        End Sub

        Private Sub LinkClicked(ByVal sender As Object, ByVal e As DevComponents.DotNetBar.MarkupLinkClickEventArgs) _
            Handles lblTags.MarkupLinkClick
            If ParentFitting.Tags.Contains(e.HRef) Then
                ParentFitting.Tags.Remove(e.HRef)
                ParentFitting.Tags.Sort()
                UpdateTags()
            End If
        End Sub

        Private Sub rateFitting_RatingChanged(sender As Object, e As EventArgs) Handles rateFitting.RatingChanged
            ParentFitting.Rating = rateFitting.Rating
        End Sub

#End Region

#Region "Ship Button Methods"

        Private Sub btnShipMode_Click(sender As Object, e As EventArgs) Handles btnShipMode0.Click, btnShipMode1.Click, btnShipMode2.Click, btnShipMode3.Click
            Call ShipModeSelection(sender)
        End Sub

        Private Sub ShipModeSelection(sender As Object)
            Dim btn As ButtonX = CType(sender, ButtonX)
            btnShipMode0.Checked = False
            btnShipMode1.Checked = False
            btnShipMode2.Checked = False
            btnShipMode3.Checked = False
            btn.Checked = True
            Dim mode As Integer = CInt(btn.Name.Substring(btn.Name.Length - 1, 1))
            ParentFitting.ShipMode = CType(mode, ShipModes)
            If _updateAll = False Then
                ParentFitting.ApplyFitting(BuildType.BuildEverything)
            End If
        End Sub

#End Region


    End Class

    Public Class UndoInfo
        Dim _transaction As TransType
        Dim _slotType As Integer = 0
        Dim _newSlotNo As Integer = 0
        Dim _newModName As String = ""
        Dim _newChargeName As String = ""
        Dim _oldSlotNo As Integer = 0
        Dim _oldModName As String = ""
        Dim _oldChargeName As String = ""
        Dim _chargeOnly As Boolean = False

        Public Property Transaction() As TransType
            Get
                Return _transaction
            End Get
            Set(ByVal value As TransType)
                _transaction = value
            End Set
        End Property

        Public Property SlotType() As Integer
            Get
                Return _slotType
            End Get
            Set(ByVal value As Integer)
                _slotType = value
            End Set
        End Property

        Public Property NewSlotNo() As Integer
            Get
                Return _newSlotNo
            End Get
            Set(ByVal value As Integer)
                _newSlotNo = value
            End Set
        End Property

        Public Property NewModName() As String
            Get
                Return _newModName
            End Get
            Set(ByVal value As String)
                _newModName = value
            End Set
        End Property

        Public Property NewChargeName() As String
            Get
                Return _newChargeName
            End Get
            Set(ByVal value As String)
                _newChargeName = value
            End Set
        End Property

        Public Property OldSlotNo() As Integer
            Get
                Return _oldSlotNo
            End Get
            Set(ByVal value As Integer)
                _oldSlotNo = value
            End Set
        End Property

        Public Property OldModName() As String
            Get
                Return _oldModName
            End Get
            Set(ByVal value As String)
                _oldModName = value
            End Set
        End Property

        Public Property OldChargeName() As String
            Get
                Return _oldChargeName
            End Get
            Set(ByVal value As String)
                _oldChargeName = value
            End Set
        End Property

        Public Property ChargeOnly() As Boolean
            Get
                Return _chargeOnly
            End Get
            Set(ByVal value As Boolean)
                _chargeOnly = value
            End Set
        End Property

        Public Sub New(ByVal transactionType As TransType, ByVal newSlotType As Integer, ByVal slotNo As Integer,
                       ByVal modName As String, ByVal chargeName As String, ByVal isChargeOnly As Boolean)
            ' Used to add a module or charge, or remove a module or charge
            _transaction = transactionType
            _slotType = newSlotType
            _newSlotNo = slotNo
            _newModName = modName
            _newChargeName = chargeName
            _chargeOnly = isChargeOnly
        End Sub

        Public Sub New(ByVal transactionType As TransType, ByVal newSlotType As Integer, ByVal slotNo1 As Integer,
                       ByVal slotNo2 As Integer)
            ' Used to swap modules
            _transaction = transactionType
            _slotType = newSlotType
            _oldSlotNo = slotNo1
            _newSlotNo = slotNo2
            _chargeOnly = False
        End Sub

        Public Sub New(ByVal transactionType As TransType, ByVal newSlotType As Integer, ByVal oldSlot As Integer,
                       ByVal oldModuleName As String, ByVal oldCharge As String, ByVal newSlot As Integer,
                       ByVal newModuleName As String, ByVal newCharge As String)
            ' Used to replace a module
            _transaction = transactionType
            _slotType = newSlotType
            _oldSlotNo = oldSlot
            _oldModName = oldModuleName
            _oldChargeName = oldCharge
            _newSlotNo = newSlot
            _newModName = newModuleName
            _newChargeName = newCharge
            _chargeOnly = False
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
End Namespace