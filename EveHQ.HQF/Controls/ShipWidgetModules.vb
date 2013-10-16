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
Imports System.Drawing
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.Core

Namespace Controls

    Public Class ShipWidgetModules

#Region "Control Constructor(s)"

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

        End Sub

        Public Sub New(hostWidget As ShipWidget, shipFitting As Fitting)

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _hostControl = hostWidget
            Call LoadRemoteGroups()
            _parentFitting = shipFitting
            ParentFitting.UpdateBaseShipFromFitting()
            ParentFitting.ApplyFitting(BuildType.BuildEverything)
            Call UpdateDetails()

        End Sub


#End Region

#Region "Property Variables"

        Private _updateAllSlots As Boolean
        Private ReadOnly _parentFitting As Fitting ' Stores the fitting to which this control is attached to
#End Region

#Region "Properties"

        Public ReadOnly Property ParentFitting() As Fitting
            Get
                Return _parentFitting
            End Get
        End Property

        Public Property UpdateAllSlots() As Boolean
            Get
                Return _updateAllSlots
            End Get
            Set(ByVal value As Boolean)
                _updateAllSlots = value
            End Set
        End Property

#End Region

#Region "Class Variables"

        ReadOnly _hostControl As ShipWidget
        ReadOnly _remoteGroups As New List(Of Integer)

#End Region

        Private Sub UpdateDetails()

            Call UpdateShipSlotLayout()
            Call UpdateAllSlotLocations()

            Select Case Math.Round(ParentFitting.FittedShip.CPUUsed, 4) / ParentFitting.FittedShip.CPU
                Case Is > 1
                    _hostControl.pbCPUStability.Image = My.Resources.Mod02
                    Dim sti As New SuperTooltipInfo("", "CPU Stability Information", "This fitting has overloaded the CPU requirement. Deactivate modules or find more CPU capacity.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                    _hostControl.STT.SetSuperTooltip(_hostControl.pbCPUStability, sti)
                Case Else
                    _hostControl.pbCPUStability.Image = My.Resources.Mod04
                    Dim sti As New SuperTooltipInfo("", "CPU Stability Information", "This fitting has adequate CPU requirement.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                    _hostControl.STT.SetSuperTooltip(_hostControl.pbCPUStability, sti)
            End Select

            Select Case Math.Round(ParentFitting.FittedShip.PGUsed, 4) / ParentFitting.FittedShip.PG
                Case Is > 1
                    _hostControl.pbPGStability.Image = My.Resources.Mod02
                    Dim sti As New SuperTooltipInfo("", "PG Stability Information", "This fitting has overloaded the PG requirement. Deactivate modules or find more PG capacity.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                    _hostControl.STT.SetSuperTooltip(_hostControl.pbPGStability, sti)
                Case Else
                    _hostControl.pbPGStability.Image = My.Resources.Mod04
                    Dim sti As New SuperTooltipInfo("", "PG Stability Information", "This fitting has adequate PG requirement.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                    _hostControl.STT.SetSuperTooltip(_hostControl.pbPGStability, sti)
            End Select

            Dim csr As CapSimResults = Capacitor.CalculateCapStatistics(ParentFitting.FittedShip, False)
            If csr.CapIsDrained = False Then
                _hostControl.pbCapStability.Image = My.Resources.Mod04
                Dim sti As New SuperTooltipInfo("", "Capacitor Stability Information", "This fitting is cap stable. The minimum capacitor value is approximately " & (csr.MinimumCap / ParentFitting.FittedShip.CapCapacity * 100).ToString("N2") & "% of capacity.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                _hostControl.STT.SetSuperTooltip(_hostControl.pbCapStability, sti)
            Else
                _hostControl.pbCapStability.Image = My.Resources.Mod02
                Dim sti As New SuperTooltipInfo("", "Capacitor Stability Information", "This fitting is not cap stable. Capacitor will run out in " & SkillFunctions.TimeToString(csr.TimeToDrain, False), Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                _hostControl.STT.SetSuperTooltip(_hostControl.pbCapStability, sti)
            End If

        End Sub

        Private Sub LoadRemoteGroups()
            _remoteGroups.Add(41)
            _remoteGroups.Add(325)
            _remoteGroups.Add(585)
            _remoteGroups.Add(67)
            _remoteGroups.Add(65)
            _remoteGroups.Add(68)
            _remoteGroups.Add(71)
            _remoteGroups.Add(291)
            _remoteGroups.Add(209)
            _remoteGroups.Add(289)
            _remoteGroups.Add(290)
            _remoteGroups.Add(208)
            _remoteGroups.Add(379)
            _remoteGroups.Add(544)
            _remoteGroups.Add(641)
            _remoteGroups.Add(640)
            _remoteGroups.Add(639)
        End Sub

        Public Sub UpdateShipSlotLayout()

            adtSlots.BeginUpdate()
            adtSlots.Nodes.Clear()

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
                For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                    If _remoteGroups.Contains(CInt(ParentFitting.BaseShip.HiSlot(slot).DatabaseGroup)) Then
                        Dim slotNode As New Node("", hiSlotStyle)
                        slotNode.Name = "8_" & slot
                        slotNode.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.HiSlotColour))
                        slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.HiSlotColour))
                        slotNode.StyleSelected = selSlotStyle
                        Call AddUserColumnData(ParentFitting.BaseShip.HiSlot(slot), slotNode)
                        adtSlots.Nodes.Add(slotNode)
                    End If
                Next
            End If

            ' Create mid slots
            If ParentFitting.BaseShip.MidSlots > 0 Then
                For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                    If _remoteGroups.Contains(CInt(ParentFitting.BaseShip.MidSlot(slot).DatabaseGroup)) Then
                        Dim slotNode As New Node("", midSlotStyle)
                        slotNode.Name = "4_" & slot
                        slotNode.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.MidSlotColour))
                        slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.MidSlotColour))
                        slotNode.StyleSelected = selSlotStyle
                        Call AddUserColumnData(ParentFitting.BaseShip.MidSlot(slot), slotNode)
                        adtSlots.Nodes.Add(slotNode)
                    End If
                Next
            End If

            ' Create low slots
            If ParentFitting.BaseShip.LowSlots > 0 Then
                For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                    If _remoteGroups.Contains(CInt(ParentFitting.BaseShip.LowSlot(slot).DatabaseGroup)) Then
                        Dim slotNode As New Node("", lowSlotStyle)
                        slotNode.Name = "2_" & slot
                        slotNode.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.LowSlotColour))
                        slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.LowSlotColour))
                        slotNode.StyleSelected = selSlotStyle
                        Call AddUserColumnData(ParentFitting.BaseShip.LowSlot(slot), slotNode)
                        adtSlots.Nodes.Add(slotNode)
                    End If
                Next
            End If

            ' Create rig slots
            If ParentFitting.BaseShip.RigSlots > 0 Then
                For slot As Integer = 1 To ParentFitting.BaseShip.RigSlots
                    If _remoteGroups.Contains(CInt(ParentFitting.BaseShip.RigSlot(slot).DatabaseGroup)) Then
                        Dim slotNode As New Node("", rigSlotStyle)
                        slotNode.Name = "1_" & slot
                        slotNode.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.RigSlotColour))
                        slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.RigSlotColour))
                        slotNode.StyleSelected = selSlotStyle
                        Call AddUserColumnData(ParentFitting.BaseShip.RigSlot(slot), slotNode)
                        adtSlots.Nodes.Add(slotNode)
                    End If
                Next
            End If

            ' Create sub slots
            If ParentFitting.BaseShip.SubSlots > 0 Then
                For slot As Integer = 1 To ParentFitting.BaseShip.SubSlots
                    If _remoteGroups.Contains(CInt(ParentFitting.BaseShip.SubSlot(slot).DatabaseGroup)) Then
                        Dim slotNode As New Node("", subSlotStyle)
                        slotNode.Name = "16_" & slot
                        slotNode.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.SubSlotColour))
                        slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.SubSlotColour))
                        slotNode.StyleSelected = selSlotStyle
                        Call AddUserColumnData(ParentFitting.BaseShip.SubSlot(slot), slotNode)
                        adtSlots.Nodes.Add(slotNode)
                    End If
                Next
            End If

            ' Create Drone Slots
            For Each remoteDrone As DroneBayItem In ParentFitting.BaseShip.DroneBayItems.Values
                If _remoteGroups.Contains(CInt(remoteDrone.DroneType.DatabaseGroup)) = True Then
                    If remoteDrone.IsActive = True Then
                        remoteDrone.DroneType.ModuleState = ModuleStates.Gang
                        Dim slotNode As New Node("", subSlotStyle)
                        slotNode.Name = remoteDrone.DroneType.Name & " (x" & remoteDrone.Quantity & ")"
                        slotNode.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.SubSlotColour))
                        slotNode.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.SubSlotColour))
                        slotNode.StyleSelected = selSlotStyle
                        slotNode.Text = remoteDrone.DroneType.Name & " (x" & remoteDrone.Quantity & ")"
                        Dim desc As String = ""
                        desc &= remoteDrone.DroneType.Description
                        SlotTip.SetSuperTooltip(slotNode, New SuperTooltipInfo(remoteDrone.DroneType.Name, "Ship Module Information", desc, Core.ImageHandler.GetImage(CInt(remoteDrone.DroneType.ID), 64), My.Resources.imgInfo1, eTooltipColor.Yellow))
                        adtSlots.Nodes.Add(slotNode)
                    End If
                End If
            Next

            adtSlots.EndUpdate()

            _hostControl.pnlModules.TitleText = adtSlots.Nodes.Count & " remote modules available..."

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
                ' Add in the module name
                slotNode.Text = shipMod.Name
                Dim desc As String = ""
                If shipMod.SlotType = SlotTypes.Subsystem Then
                    desc &= "Slot Modifiers - High: " & shipMod.Attributes(AttributeEnum.ModuleHighSlotModifier) & ", Mid: " & shipMod.Attributes(AttributeEnum.ModuleMidSlotModifier) & ", Low: " & shipMod.Attributes(AttributeEnum.ModuleLowSlotModifier) & ControlChars.CrLf & ControlChars.CrLf
                End If
                desc &= shipMod.Description
                SlotTip.SetSuperTooltip(slotNode, New SuperTooltipInfo(shipMod.Name, "Ship Module Information", desc, Core.ImageHandler.GetImage(CInt(shipMod.ID), 64), My.Resources.imgInfo1, eTooltipColor.Yellow))
            Else
                slotNode.Text = "<Empty>"
                SlotTip.SetSuperTooltip(slotNode, Nothing)
                For Each userCol As UserSlotColumn In PluginSettings.HQFSettings.UserSlotColumns
                    If userCol.Active = True Then
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
            ' Add in the module name
            slotNode.Text = shipMod.Name
            Dim desc As String = ""
            If shipMod.SlotType = SlotTypes.Subsystem Then
                desc &= "Slot Modifiers - High: " & shipMod.Attributes(AttributeEnum.ModuleHighSlotModifier) & ", Mid: " & shipMod.Attributes(AttributeEnum.ModuleMidSlotModifier) & ", Low: " & shipMod.Attributes(AttributeEnum.ModuleLowSlotModifier) & ControlChars.CrLf & ControlChars.CrLf
            End If
            desc &= shipMod.Description
            SlotTip.SetSuperTooltip(slotNode, New SuperTooltipInfo(shipMod.Name, "Ship Module Information", desc, Core.ImageHandler.GetImage(CInt(shipMod.ID), 64), My.Resources.imgInfo1, eTooltipColor.Yellow))
        End Sub

        Public Sub UpdateAllSlotLocations()
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
                    If slotNode IsNot Nothing Then
                        slotNode.Image = New Bitmap(CType(My.Resources.ResourceManager.GetObject("Mod0" & CInt(shipMod.ModuleState).ToString), Image), 14, 14)
                        Call UpdateUserColumnData(shipMod, slotNode)
                    End If
                End If
            End If
        End Sub

        Private Sub adtSlots_NodeMouseDown(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtSlots.NodeMouseDown

            If e.Node IsNot Nothing Then

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
                    Call UpdateDetails()

                End If
            End If
        End Sub

        ''' <summary>
        ''' Changes the module state of the module in the current node (slot)
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
                If currentMod.Attributes.ContainsKey(AttributeEnum.ModuleCapacitorNeed) = True Or currentMod.Attributes.ContainsKey(AttributeEnum.ModuleReactivationDelay) Or currentMod.IsTurret Or currentMod.IsLauncher Or currentMod.Attributes.ContainsKey(AttributeEnum.ModuleConsumptionType) Then
                    canDeactivate = True
                End If
                If currentMod.Attributes.ContainsKey(AttributeEnum.ModuleHeatDamage) = True Then
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
                    If currentMod.ID = ModuleEnum.ItemCommandProcessorI Then
                        If currentstate = ModuleStates.Offline Then
                            ParentFitting.BaseShip.Attributes(AttributeEnum.ShipMaxGangLinks) -= 1
                            ' Check if we need to deactivate a highslot ganglink
                            Dim activeGanglinks As New List(Of Integer)
                            For slot As Integer = 8 To 1 Step -1
                                If ParentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                                    If ParentFitting.BaseShip.HiSlot(slot).DatabaseGroup = ModuleEnum.GroupGangLinks And ParentFitting.BaseShip.HiSlot(slot).ModuleState = ModuleStates.Active Then
                                        activeGanglinks.Add(slot)
                                    End If
                                End If
                            Next
                            If activeGanglinks.Count > ParentFitting.BaseShip.Attributes(AttributeEnum.ShipMaxGangLinks) Then
                                ParentFitting.BaseShip.HiSlot(activeGanglinks(0)).ModuleState = ModuleStates.Inactive
                            End If
                        Else
                            ParentFitting.BaseShip.Attributes(AttributeEnum.ShipMaxGangLinks) += 1
                        End If
                    End If
                    Dim oldState As ModuleStates = currentMod.ModuleState
                    currentMod.ModuleState = CType(currentstate, ModuleStates)
                    ' Check for maxGroupActive flag
                    If (currentstate = ModuleStates.Active Or currentstate = ModuleStates.Overloaded) And currentMod.Attributes.ContainsKey(AttributeEnum.ModuleMaxGroupActive) = True Then
                        If currentMod.DatabaseGroup <> ModuleEnum.GroupGangLinks Then
                            If ParentFitting.IsModuleGroupLimitExceeded(fittedMod, False, AttributeEnum.ModuleMaxGroupActive) = True Then
                                ' Set the module offline
                                MessageBox.Show("You cannot activate the " & currentMod.Name & " due to a restriction on the maximum number permitted for this group.", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                currentMod.ModuleState = oldState
                                Exit Sub
                            End If
                        Else
                            If ParentFitting.IsModuleGroupLimitExceeded(fittedMod, False, AttributeEnum.ModuleMaxGroupActive) = True Then
                                ' Set the module offline
                                MessageBox.Show("You cannot activate the " & currentMod.Name & " due to a restriction on the maximum number permitted for this group.", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                currentMod.ModuleState = oldState
                                Exit Sub
                            Else
                                If ParentFitting.CountActiveTypeModules(fittedMod.ID) > CInt(fittedMod.Attributes(AttributeEnum.ModuleMaxGroupActive)) Then
                                    ' Set the module offline
                                    MessageBox.Show("You cannot activate the " & currentMod.Name & " due to a restriction on the maximum number permitted for this type.", "Module Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    currentMod.ModuleState = oldState
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

    End Class
End NameSpace