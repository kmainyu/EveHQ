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
Imports DevComponents.DotNetBar
Imports System.Drawing
Imports DevComponents.AdvTree
Imports System.Windows.Forms

Public Class ShipWidgetModules

#Region "Control Constructor(s)"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(HostWidget As ShipWidget, ShipFitting As Fitting)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        HostControl = HostWidget
        Call Me.LoadRemoteGroups()
        cParentFitting = ShipFitting
        ParentFitting.UpdateBaseShipFromFitting()
        ParentFitting.ApplyFitting(BuildType.BuildEverything)
        Call Me.UpdateDetails()

    End Sub


#End Region

#Region "Property Variables"

    Private cUpdateAllSlots As Boolean
    Private cParentFitting As Fitting ' Stores the fitting to which this control is attached to
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

#End Region

#Region "Class Variables"

    Dim HostControl As ShipWidget
    Dim RemoteGroups As New List(Of Integer)

#End Region

    Private Sub UpdateDetails()

        Call Me.UpdateShipSlotLayout()
        Call Me.UpdateAllSlotLocations()

        Select Case Math.Round(ParentFitting.FittedShip.CPU_Used, 4) / ParentFitting.FittedShip.CPU
            Case Is > 1
                HostControl.pbCPUStability.Image = My.Resources.Mod02
                Dim STI As New SuperTooltipInfo("", "CPU Stability Information", "This fitting has overloaded the CPU requirement. Deactivate modules or find more CPU capacity.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                HostControl.STT.SetSuperTooltip(HostControl.pbCPUStability, STI)
            Case Else
                HostControl.pbCPUStability.Image = My.Resources.Mod04
                Dim STI As New SuperTooltipInfo("", "CPU Stability Information", "This fitting has adequate CPU requirement.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                HostControl.STT.SetSuperTooltip(HostControl.pbCPUStability, STI)
        End Select

        Select Case Math.Round(ParentFitting.FittedShip.PG_Used, 4) / ParentFitting.FittedShip.PG
            Case Is > 1
                HostControl.pbPGStability.Image = My.Resources.Mod02
                Dim STI As New SuperTooltipInfo("", "PG Stability Information", "This fitting has overloaded the PG requirement. Deactivate modules or find more PG capacity.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                HostControl.STT.SetSuperTooltip(HostControl.pbPGStability, STI)
            Case Else
                HostControl.pbPGStability.Image = My.Resources.Mod04
                Dim STI As New SuperTooltipInfo("", "PG Stability Information", "This fitting has adequate PG requirement.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
                HostControl.STT.SetSuperTooltip(HostControl.pbPGStability, STI)
        End Select

        Dim csr As CapSimResults = Capacitor.CalculateCapStatistics(ParentFitting.FittedShip, False)
        If csr.CapIsDrained = False Then
            HostControl.pbCapStability.Image = My.Resources.Mod04
            Dim STI As New SuperTooltipInfo("", "Capacitor Stability Information", "This fitting is cap stable. The minimum capacitor value is approximately " & (csr.MinimumCap / ParentFitting.FittedShip.CapCapacity * 100).ToString("N2") & "% of capacity.", Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
            HostControl.STT.SetSuperTooltip(HostControl.pbCapStability, STI)
        Else
            HostControl.pbCapStability.Image = My.Resources.Mod02
            Dim STI As New SuperTooltipInfo("", "Capacitor Stability Information", "This fitting is not cap stable. Capacitor will run out in " & EveHQ.Core.SkillFunctions.TimeToString(csr.TimeToDrain, False), Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow)
            HostControl.STT.SetSuperTooltip(HostControl.pbCapStability, STI)
        End If

    End Sub

    Private Sub LoadRemoteGroups()
        RemoteGroups.Add(41)
        RemoteGroups.Add(325)
        RemoteGroups.Add(585)
        RemoteGroups.Add(67)
        RemoteGroups.Add(65)
        RemoteGroups.Add(68)
        RemoteGroups.Add(71)
        RemoteGroups.Add(291)
        RemoteGroups.Add(209)
        RemoteGroups.Add(289)
        RemoteGroups.Add(290)
        RemoteGroups.Add(208)
        RemoteGroups.Add(379)
        RemoteGroups.Add(544)
        RemoteGroups.Add(641)
        RemoteGroups.Add(640)
        RemoteGroups.Add(639)
    End Sub

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
            For slot As Integer = 1 To ParentFitting.BaseShip.HiSlots
                If RemoteGroups.Contains(CInt(ParentFitting.BaseShip.HiSlot(slot).DatabaseGroup)) Then
                    Dim SlotNode As New Node("", HiSlotStyle)
                    SlotNode.Name = "8_" & slot
                    SlotNode.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                    SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                    SlotNode.StyleSelected = SelSlotStyle
                    Call Me.AddUserColumnData(ParentFitting.BaseShip.HiSlot(slot), SlotNode)
                    adtSlots.Nodes.Add(SlotNode)
                End If
            Next
        End If

        ' Create mid slots
        If ParentFitting.BaseShip.MidSlots > 0 Then
            For slot As Integer = 1 To ParentFitting.BaseShip.MidSlots
                If RemoteGroups.Contains(CInt(ParentFitting.BaseShip.MidSlot(slot).DatabaseGroup)) Then
                    Dim SlotNode As New Node("", MidSlotStyle)
                    SlotNode.Name = "4_" & slot
                    SlotNode.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                    SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                    SlotNode.StyleSelected = SelSlotStyle
                    Call Me.AddUserColumnData(ParentFitting.BaseShip.MidSlot(slot), SlotNode)
                    adtSlots.Nodes.Add(SlotNode)
                End If
            Next
        End If

        ' Create low slots
        If ParentFitting.BaseShip.LowSlots > 0 Then
            For slot As Integer = 1 To ParentFitting.BaseShip.LowSlots
                If RemoteGroups.Contains(CInt(ParentFitting.BaseShip.LowSlot(slot).DatabaseGroup)) Then
                    Dim SlotNode As New Node("", LowSlotStyle)
                    SlotNode.Name = "2_" & slot
                    SlotNode.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                    SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                    SlotNode.StyleSelected = SelSlotStyle
                    Call Me.AddUserColumnData(ParentFitting.BaseShip.LowSlot(slot), SlotNode)
                    adtSlots.Nodes.Add(SlotNode)
                End If
            Next
        End If

        ' Create rig slots
        If ParentFitting.BaseShip.RigSlots > 0 Then
            For slot As Integer = 1 To ParentFitting.BaseShip.RigSlots
                If RemoteGroups.Contains(CInt(ParentFitting.BaseShip.RigSlot(slot).DatabaseGroup)) Then
                    Dim SlotNode As New Node("", RigSlotStyle)
                    SlotNode.Name = "1_" & slot
                    SlotNode.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                    SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                    SlotNode.StyleSelected = SelSlotStyle
                    Call Me.AddUserColumnData(ParentFitting.BaseShip.RigSlot(slot), SlotNode)
                    adtSlots.Nodes.Add(SlotNode)
                End If
            Next
        End If

        ' Create sub slots
        If ParentFitting.BaseShip.SubSlots > 0 Then
            For slot As Integer = 1 To ParentFitting.BaseShip.SubSlots
                If RemoteGroups.Contains(CInt(ParentFitting.BaseShip.SubSlot(slot).DatabaseGroup)) Then
                    Dim SlotNode As New Node("", SubSlotStyle)
                    SlotNode.Name = "16_" & slot
                    SlotNode.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.SubSlotColour))
                    SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.SubSlotColour))
                    SlotNode.StyleSelected = SelSlotStyle
                    Call Me.AddUserColumnData(ParentFitting.BaseShip.SubSlot(slot), SlotNode)
                    adtSlots.Nodes.Add(SlotNode)
                End If
            Next
        End If

        ' Create Drone Slots
        For Each remoteDrone As DroneBayItem In ParentFitting.BaseShip.DroneBayItems.Values
            If RemoteGroups.Contains(CInt(remoteDrone.DroneType.DatabaseGroup)) = True Then
                If remoteDrone.IsActive = True Then
                    remoteDrone.DroneType.ModuleState = ModuleStates.Gang
                    Dim SlotNode As New Node("", SubSlotStyle)
                    SlotNode.Name = remoteDrone.DroneType.Name & " (x" & remoteDrone.Quantity & ")"
                    SlotNode.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.SubSlotColour))
                    SlotNode.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.SubSlotColour))
                    SlotNode.StyleSelected = SelSlotStyle
                    SlotNode.Text = remoteDrone.DroneType.Name & " (x" & remoteDrone.Quantity & ")"
                    Dim Desc As String = ""
                    Desc &= remoteDrone.DroneType.Description
                    SlotTip.SetSuperTooltip(SlotNode, New SuperTooltipInfo(remoteDrone.DroneType.Name, "Ship Module Information", Desc, EveHQ.Core.ImageHandler.GetImage(remoteDrone.DroneType.ID, 64), My.Resources.imgInfo1, eTooltipColor.Yellow))
                    adtSlots.Nodes.Add(SlotNode)
                End If
            End If
        Next

        adtSlots.EndUpdate()

        HostControl.pnlModules.TitleText = adtSlots.Nodes.Count & " remote modules available..."

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
        Else
            slotNode.Text = "<Empty>"
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
        SlotTip.SetSuperTooltip(slotNode, New SuperTooltipInfo(shipMod.Name, "Ship Module Information", Desc, EveHQ.Core.ImageHandler.GetImage(shipMod.ID, 64), My.Resources.imgInfo1, eTooltipColor.Yellow))
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
                If slotNode IsNot Nothing Then
                    slotNode.Image = New Bitmap(CType(My.Resources.ResourceManager.GetObject("Mod0" & CInt(shipMod.ModuleState).ToString), Image), 14, 14)
                    Call Me.UpdateUserColumnData(shipMod, slotNode)
                End If
            End If
        End If
    End Sub

    Private Sub adtSlots_NodeMouseDown(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtSlots.NodeMouseDown

        If e.Node IsNot Nothing Then

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
                Call Me.UpdateDetails()

            End If
        End If
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
            End If
        End If
    End Sub

End Class
