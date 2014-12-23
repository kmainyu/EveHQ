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

Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.Common.Extensions
Imports EveHQ.Core
Imports EveHQ.Core.Requisitions
Imports EveHQ.HQF.Controls

Namespace Forms

    Public Class FrmHQF

        Dim _activeFitting As Fitting
        Dim _lastSlotFitting As New ArrayList
        Dim _lastModuleResults As New SortedList(Of Integer, ShipModule)
        Dim _myPilotManager As New FrmPilotManager
        Dim _myBcBrowser As New FrmBcBrowser
        Dim _myEveImport As New FrmEveImport
        Dim _shutdownComplete As Boolean = False

#Region "Class Wide Variables"

        Dim _startUp As Boolean = False

        Public Property ModuleDisplay() As String

        Private Property ActiveFitting() As Fitting
            Get
                Return _activeFitting
            End Get
            Set(ByVal value As Fitting)
                _activeFitting = value
                If _activeFitting IsNot Nothing Then
                    btnExportEve.Enabled = True
                    btnExportFitting.Enabled = True
                    btnExportDetails.Enabled = True
                    btnScreenGrab.Enabled = True
                    btnExportReq.Enabled = True
                Else
                    btnExportEve.Enabled = False
                    btnExportFitting.Enabled = False
                    btnExportDetails.Enabled = False
                    btnScreenGrab.Enabled = False
                    btnExportReq.Enabled = False
                End If
            End Set
        End Property

#End Region

#Region "Form Initialisation & Closing Routines"

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            ' Set the panel widths
            panelShips.Width = PluginSettings.HQFSettings.ShipPanelWidth
            panelModules.Width = PluginSettings.HQFSettings.ModPanelWidth
            panelFittings.Height = PluginSettings.HQFSettings.ShipSplitterWidth
            panelModFilters.Height = PluginSettings.HQFSettings.ModSplitterWidth

        End Sub

        Private Sub frmHQF_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing

            ' Remove events
            RemoveHandler HQFEvents.ShowModuleMarketGroup, AddressOf UpdateMarketGroup
            RemoveHandler HQFEvents.FindModule, AddressOf UpdateModulesThatWillFit
            RemoveHandler HQFEvents.UpdateFitting, AddressOf UpdateFittings
            RemoveHandler HQFEvents.UpdateFittingList, AddressOf UpdateShipFittings
            RemoveHandler HQFEvents.UpdateModuleList, AddressOf UpdateModuleList
            RemoveHandler HQFEvents.UpdateShipInfo, AddressOf UpdateShipInfo
            RemoveHandler HQFEvents.UpdateAllImplantLists, AddressOf UpdateAllImplantLists
            RemoveHandler HQFEvents.OpenFitting, AddressOf RemoteShowFitting
            RemoveHandler HQFEvents.CreateFitting, AddressOf CreateNewFitting

            If _shutdownComplete = False Then
                ' Close any open windows
                If _myPilotManager.IsHandleCreated Then _myPilotManager.Close()
                If _myBcBrowser.IsHandleCreated Then _myBcBrowser.Close()

                ' Save data and settings
                Call SaveAll()

                _shutdownComplete = True
            End If

        End Sub

        Private Sub frmHQF_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            _startUp = True
            ModuleDisplay = ""
            _lastModuleResults.Clear()

            SuspendLayout()

            ' Clear tabs and fitted ship lists, results list
            ShipLists.FittedShipList.Clear()
            _lastModuleResults.Clear()
            tabHQF.Tabs.Clear()

            AddHandler HQFEvents.ShowModuleMarketGroup, AddressOf UpdateMarketGroup
            AddHandler HQFEvents.FindModule, AddressOf UpdateModulesThatWillFit
            AddHandler HQFEvents.UpdateFitting, AddressOf UpdateFittings
            AddHandler HQFEvents.UpdateFittingList, AddressOf UpdateShipFittings
            AddHandler HQFEvents.UpdateModuleList, AddressOf UpdateModuleList
            AddHandler HQFEvents.UpdateShipInfo, AddressOf UpdateShipInfo
            AddHandler HQFEvents.UpdateAllImplantLists, AddressOf UpdateAllImplantLists
            AddHandler HQFEvents.OpenFitting, AddressOf RemoteShowFitting
            AddHandler HQFEvents.CreateFitting, AddressOf CreateNewFitting

            ' Load the Profiles - stored separately from settings for distribution!
            Call HQFDamageProfiles.Load()
            Call HQFDefenceProfiles.Load()

            ' Load up a collection of pilots from the EveHQ Core
            Call LoadPilots()

            ' Load saved setups into the fitting array
            Call LoadFittings()

            ' Set the MetaType Filter
            Call SetMetaTypeFilters()

            ' Show the groups
            Call ShowShipGroups()
            Call ShowMarketGroups()

            _startUp = False
            ' Temporarily disable the performance setting
            PluginSettings.HQFSettings.ShowPerformanceData = False

            ' Check if we need to restore tabs from the previous setup
            If PluginSettings.HQFSettings.RestoreLastSession = True Then
                For Each fitKey As String In PluginSettings.HQFSettings.OpenFittingList
                    If Fittings.FittingList.ContainsKey(fitKey) = True Then
                        ' Create the tab and display
                        Dim newfit As Fitting = Fittings.FittingList(fitKey)
                        Call CreateNewFittingTab(newfit)
                        newfit.ShipSlotCtrl.UpdateEverything()

                        If ActiveFitting Is Nothing Then
                            ActiveFitting = newfit
                            tabHQF.SelectedTabIndex = 0
                        End If
                    End If
                Next
            End If

            ' Set default widths of module list
            Const ModuleListColumns As Integer = 5
            If PluginSettings.HQFSettings.ModuleListColWidths.Count <> ModuleListColumns Then
                PluginSettings.HQFSettings.ModuleListColWidths.Clear()
                PluginSettings.HQFSettings.ModuleListColWidths.Add(0, 150)
                PluginSettings.HQFSettings.ModuleListColWidths.Add(1, 40)
                PluginSettings.HQFSettings.ModuleListColWidths.Add(2, 40)
                PluginSettings.HQFSettings.ModuleListColWidths.Add(3, 40)
                PluginSettings.HQFSettings.ModuleListColWidths.Add(4, 80)
            End If
            For col As Integer = 0 To ModuleListColumns - 1
                tvwModules.Columns(col).Width.Absolute = PluginSettings.HQFSettings.ModuleListColWidths(CLng(col))
            Next

            ResumeLayout()

        End Sub
        Private Sub LoadFittings()
            Call SavedFittings.LoadFittings()
            Call UpdateFittingsTree(True)
        End Sub
        Private Sub SaveFittings()
            Call SavedFittings.SaveFittings()
        End Sub

        Public Sub SaveAll()
            ' Save the panel widths
            PluginSettings.HQFSettings.ShipPanelWidth = panelShips.Width
            PluginSettings.HQFSettings.ModPanelWidth = panelModules.Width
            PluginSettings.HQFSettings.ShipSplitterWidth = panelFittings.Height
            PluginSettings.HQFSettings.ModSplitterWidth = panelModFilters.Height

            ' Save fittings
            'MessageBox.Show("HQF is about to enter the routine to save the fittings file. There are " & Fittings.FittingList.Count & " fittings detected.", "Save Fittings Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Call SaveFittings()

            ' Save pilots
            Call FittingPilots.SaveHQFPilotData()

            ' Save the open fittings
            PluginSettings.HQFSettings.OpenFittingList.Clear()
            For Each fitting As String In ShipLists.FittedShipList.Keys
                PluginSettings.HQFSettings.OpenFittingList.Add(fitting)
            Next

            ' Save the Settings
            Call PluginSettings.HQFSettings.SaveHQFSettings()
        End Sub

        Private Sub ShowShipGroups()
            Dim sr As New StreamReader(Path.Combine(PluginSettings.HQFCacheFolder, "ShipGroups.bin"))
            Dim shipGroups As String = sr.ReadToEnd
            Dim pathLines() As String = shipGroups.Split(ControlChars.CrLf.ToCharArray)
            Dim nodes() As String
            Dim cNode As Node
            Dim isFlyable As Boolean
            sr.Close()
            tvwShips.BeginUpdate()
            tvwShips.Nodes.Clear()
            For Each pathline As String In pathLines
                If pathline.Trim <> "" Then
                    If pathline.Contains("\") = False Then
                        Dim nNode As New Node
                        nNode.Name = pathline
                        nNode.Text = pathline
                        tvwShips.Nodes.Add(nNode)
                    Else
                        nodes = pathline.Split("\".ToCharArray)
                        If ShipLists.ShipListKeyName.ContainsKey(nodes(nodes.Length - 1)) And cboFlyable.SelectedIndex > 0 Then
                            'If nodes.Length = 5 And cboFlyable.SelectedIndex > 0 Then
                            isFlyable = IsShipFlyable(nodes(nodes.Length - 1), cboFlyable.SelectedItem.ToString)
                        Else
                            isFlyable = True
                        End If
                        If isFlyable = True Then
                            Dim parentNode As String = nodes(0)
                            If nodes.Length > 2 Then
                                For node As Integer = 1 To nodes.GetUpperBound(0) - 1
                                    parentNode &= "\" & nodes(node)
                                Next
                            End If
                            cNode = tvwShips.FindNodeByName(parentNode)
                            Dim nNode As New Node
                            nNode.Text = nodes(nodes.GetUpperBound(0))
                            nNode.Name = pathline
                            If ShipLists.ShipList.ContainsKey(nNode.Text) = True Then
                                Dim ship As Ship = ShipLists.ShipList(nNode.Text)
                                Dim stt As New SuperTooltipInfo
                                stt.HeaderText = ship.Name
                                stt.FooterText = "HQF Ship Information - " & ship.Name
                                stt.BodyText &= "Slots - Hi:" & ship.HiSlots.ToString & " Mid:" & ship.MidSlots.ToString & " Low:" & ship.LowSlots.ToString & " Rig:" & ship.RigSlots.ToString & " T:" & ship.TurretSlots.ToString & " ML:" & ship.LauncherSlots.ToString & "<br />"
                                stt.BodyText &= "CPU: " & ship.CPU.ToString("N0") & ", PG: " & ship.PG.ToString("N0") & ", Cal: " & ship.Calibration.ToString("N0") & ", Cap: " & ship.CapCapacity.ToString("N0") & " (" & ship.CapRecharge.ToString("N0") & "s)<br />"
                                stt.BodyText &= "Shield: " & ship.ShieldCapacity.ToString("N0") & " (" & ship.ShieldRecharge.ToString("N0") & "s) - EM:" & ship.ShieldEMResist.ToString("N0") & "% Ex:" & ship.ShieldExResist.ToString("N0") & "% Ki:" & ship.ShieldKiResist.ToString("N0") & "% Th:" & ship.ShieldThResist.ToString("N0") & "%<br />"
                                stt.BodyText &= "Armor: " & ship.ArmorCapacity.ToString("N0") & " - EM:" & ship.ArmorEMResist.ToString("N0") & "% Ex:" & ship.ArmorExResist.ToString("N0") & "% Ki:" & ship.ArmorKiResist.ToString("N0") & "% Th:" & ship.ArmorThResist.ToString("N0") & "%<br />"
                                stt.BodyText &= "Hull: " & ship.StructureCapacity.ToString("N0") & " - EM:" & ship.StructureEMResist.ToString("N0") & "% Ex:" & ship.StructureExResist.ToString("N0") & "% Ki:" & ship.StructureKiResist.ToString("N0") & "% Th:" & ship.StructureThResist.ToString("N0") & "%<br />"
                                stt.BodyText &= "Targeting: " & (ship.MaxTargetRange / 1000).ToString("N0") & "km, (" & ship.MaxLockedTargets.ToString("N0") & "T), Scan Res: " & ship.ScanResolution.ToString("N0") & "mm, Sensors: " & (ship.GravSensorStrenth + ship.LadarSensorStrenth + ship.MagSensorStrenth + ship.RadarSensorStrenth).ToString("N0") & "<br />"
                                stt.BodyText &= "Cargo: " & ship.CargoBay.ToString("N0") & "m³ Drone Bay: " & ship.DroneBay.ToString("N0") & "m³ (B/W: " & ship.DroneBandwidth.ToString("N0") & "Mb/s)<br />"
                                stt.BodyText &= "Max Velocity: " & ship.MaxVelocity.ToString("N2") & "m/s, Warp: " & ship.WarpSpeed.ToString("N0") & "au/s"
                                stt.Color = eTooltipColor.Yellow
                                'stt.BodyImage = EveHQ.Core.ImageHandler.GetImage(Ship.ID, 96)
                                stt.FooterImage = My.Resources.imgInfo1
                                STTShips.SetSuperTooltip(nNode, stt)
                            End If
                            cNode.Nodes.Add(nNode)
                        End If
                    End If
                End If
            Next
            ' Remove any groups that have no children
            Dim cNodeIdx As Integer = 0
            Dim childNode As Node
            Do
                childNode = tvwShips.Nodes(cNodeIdx)
                If childNode.HasChildNodes Then
                    Call CheckShipNodes(childNode)
                    If childNode.HasChildNodes = False Then
                        tvwShips.Nodes.RemoveAt(cNodeIdx)
                        cNodeIdx -= 1
                    End If
                Else
                    tvwShips.Nodes.RemoveAt(cNodeIdx)
                    cNodeIdx -= 1
                End If
                cNodeIdx += 1
            Loop Until cNodeIdx = tvwShips.Nodes.Count
            ' Finalise and update the list
            tvwShips.EndUpdate()
            ' Put the root nodes into a ship list for later reference
            Market.MarketShipList.Clear()
            For Each shipNode As Node In tvwShips.Nodes
                If Market.MarketShipList.Contains(shipNode.Text) = False Then
                    Market.MarketShipList.Add(shipNode.Text)
                End If
            Next
        End Sub
        Private Sub CheckShipNodes(ByVal pNode As Node)
            Dim cNodeIdx As Integer = 0
            Dim cNode As Node
            Do
                cNode = pNode.Nodes(cNodeIdx)
                If cNode.HasChildNodes Then
                    Call CheckShipNodes(cNode)
                    If cNode.HasChildNodes = False Then
                        pNode.Nodes.RemoveAt(cNodeIdx)
                        cNodeIdx -= 1
                    End If
                Else
                    If ShipLists.ShipListKeyName.ContainsKey(cNode.Text) = False Then
                        ' Remove the node
                        pNode.Nodes.RemoveAt(cNodeIdx)
                        cNodeIdx -= 1
                    End If
                End If
                cNodeIdx += 1
            Loop Until cNodeIdx = pNode.Nodes.Count
        End Sub
        Private Sub ShowMarketGroups()
            Dim sr As New StreamReader(Path.Combine(PluginSettings.HQFCacheFolder, "ItemGroups.bin"))
            Dim shipGroups As String = sr.ReadToEnd
            Dim pathLines() As String = shipGroups.Split(ControlChars.CrLf.ToCharArray)
            Dim nodes() As String
            Dim nodeData() As String
            Dim cNode As Node
            Dim newNode As Node
            sr.Close()
            tvwItems.BeginUpdate()
            tvwItems.Nodes.Clear()
            Market.MarketGroupList.Clear()
            Market.MarketGroupPath.Clear()
            For Each pathline As String In pathLines
                If pathline <> "" Then
                    If pathline.Contains("\") = False Then
                        nodeData = pathline.Split(",".ToCharArray)
                        Dim n As New Node(nodeData(1))
                        n.Name = nodeData(1)
                        n.Tag = nodeData(0)
                        tvwItems.Nodes.Add(n)
                    Else
                        nodeData = pathline.Split(",".ToCharArray)
                        nodes = nodeData(1).Split("\".ToCharArray)
                        Dim parentNode As String = nodes(0)
                        If nodes.Length > 2 Then
                            For node As Integer = 1 To nodes.GetUpperBound(0) - 1
                                parentNode &= "\" & nodes(node)
                            Next
                        End If
                        cNode = tvwItems.FindNodeByName(parentNode)
                        newNode = New Node
                        newNode.Name = nodeData(1)
                        newNode.Text = nodes(nodes.GetUpperBound(0))
                        newNode.Tag = nodeData(0)
                        cNode.Nodes.Add(newNode)
                        If newNode.Tag.ToString <> "0" Then
                            Market.MarketGroupList.Add(newNode.Tag.ToString, newNode.Text)
                            Market.MarketGroupPath.Add(newNode.Tag.ToString, nodeData(1))
                        End If
                    End If
                End If
            Next
            tvwItems.EndUpdate()
            ' Copy these to the market node list
            Market.MarketNodeList.Clear()
            For Each newNode In tvwItems.Nodes
                Market.MarketNodeList.Add(newNode)
            Next
        End Sub
        Private Sub UpdateMarketGroup(ByVal path As String)
            Dim cNode As Node
            Dim nodes() As String = path.Split("\".ToCharArray)
            Dim parentNode As String = nodes(0)
            If nodes.Length >= 2 Then
                For node As Integer = 1 To nodes.GetUpperBound(0)
                    parentNode &= "\" & nodes(node)
                Next
            End If
            cNode = tvwItems.FindNodeByName(parentNode)
            If cNode.IsSelected Then
                Call CalculateFilteredModules(cNode)
            Else
                tvwItems.SelectedNode = cNode
            End If
            tvwItems.Select()
        End Sub

        Private Sub LoadPilots()
            ' Loads the skills for the selected pilots
            ' Check for a valid HQFPilotSettings.xml file
            If My.Computer.FileSystem.FileExists(Path.Combine(PluginSettings.HQFFolder, "HQFPilotSettings.json")) = True Then
                Call FittingPilots.LoadHQFPilotData()
                ' Check we have all the available pilots!
                Dim morePilots As Boolean = False
                For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
                    If FittingPilots.HQFPilots.ContainsKey(pilot.Name) = False Then
                        ' We don't have it, so lets create one!
                        Dim newHQFPilot As New FittingPilot
                        newHQFPilot.PilotName = pilot.Name
                        newHQFPilot.SkillSet = New Dictionary(Of String, FittingSkill)
                        FittingPilots.ResetSkillsToDefault(newHQFPilot)
                        For imp As Integer = 0 To 10
                            newHQFPilot.ImplantName(imp) = ""
                        Next
                        FittingPilots.HQFPilots.Add(newHQFPilot.PilotName, newHQFPilot)
                        morePilots = True
                    End If
                Next
                For Each hPilot As FittingPilot In FittingPilots.HQFPilots.Values
                    ' Check to make sure the skills the HQF record has are infact existing skills
                    ' Bug #40: The Heavy Assault Missile Specialization skill was renamed to just Assault Missile Specialization,
                    ' however HQF pilot records which had the old skill were having it added to modules as well as the new skill.
                    ' If a pilot is found to have invalid/unknown skills in their HQF record, their skills will get reset to what
                    ' the EVE record states as valid.
                    Call FittingPilots.CheckForInvalidSkills(hPilot)

                    ' Check for missing skills in the pilots info

                    Call FittingPilots.CheckForMissingSkills(hPilot)
                Next

                ' Check if we need to update the HQFPilot skills to actuals
                If PluginSettings.HQFSettings.AutoUpdateHQFSkills = True Then
                    morePilots = True
                    For Each hPilot As FittingPilot In FittingPilots.HQFPilots.Values
                        Call FittingPilots.UpdateHQFSkillsToActual(hPilot)
                    Next
                End If
                ' Save the data if we need to
                If morePilots = True Then
                    Call FittingPilots.SaveHQFPilotData()
                End If
            Else
                FittingPilots.HQFPilots.Clear()
                For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
                    Dim newHQFPilot As New FittingPilot
                    newHQFPilot.PilotName = pilot.Name
                    newHQFPilot.SkillSet = New Dictionary(Of String, FittingSkill)
                    FittingPilots.ResetSkillsToDefault(newHQFPilot)
                    For imp As Integer = 0 To 10
                        newHQFPilot.ImplantName(imp) = ""
                    Next
                    FittingPilots.HQFPilots.Add(newHQFPilot.PilotName, newHQFPilot)
                Next
                ' Save the data
                Call FittingPilots.SaveHQFPilotData()
            End If

            ' Remove old HQF Pilots
            Dim removePilotList As New ArrayList
            For Each hPilot As String In FittingPilots.HQFPilots.Keys
                If HQ.Settings.Pilots.ContainsKey(hPilot) = False Then
                    removePilotList.Add(hPilot)
                End If
            Next
            For Each hpilot As String In removePilotList
                FittingPilots.HQFPilots.Remove(hpilot)
            Next
            removePilotList.Clear()

            If FittingPilots.HQFPilots.Count > 0 Then
                btnPilotManager.Enabled = True
                btnImplantManager.Enabled = True
            End If

            'Update the default pilot if it is null or set to a non-existing pilot
            If (String.IsNullOrEmpty(PluginSettings.HQFSettings.DefaultPilot) = True Or FittingPilots.HQFPilots.ContainsKey(PluginSettings.HQFSettings.DefaultPilot) = False) And FittingPilots.HQFPilots.Count > 0 Then
                ' default to first pilot in the collection
                PluginSettings.HQFSettings.DefaultPilot = FittingPilots.HQFPilots.Values(0).PilotName
            End If

            ' Update the ship filter
            Call UpdateShipFilter()

        End Sub
        Private Sub UpdateShipFilter()
            cboFlyable.BeginUpdate()
            cboFlyable.Items.Clear()
            cboFlyable.Items.Add("<All Ships>")
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    cboFlyable.Items.Add(cPilot.Name)
                End If
            Next
            cboFlyable.EndUpdate()
            cboFlyable.SelectedIndex = 0
        End Sub
        Private Sub SetMetaTypeFilters()
            Dim filters() As Integer = {1, 2, 4, 8, 16, 32, 8192}
            For Each filter As Integer In filters
                Dim chkBox As CheckBox = CType(panelModFilters.Controls.Item("chkFilter" & filter.ToString), CheckBox)
                If (PluginSettings.HQFSettings.ModuleFilter And filter) = filter Then
                    chkBox.Checked = True
                Else
                    chkBox.Checked = False
                End If
            Next
        End Sub
#End Region

#Region "Ship Browser Routines"
        Private Sub tvwShips_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles tvwShips.NodeClick
            tvwShips.SelectedNode = e.Node
        End Sub
        Private Sub tvwShips_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles tvwShips.NodeDoubleClick
            tvwShips.SelectedNode = e.Node
            Dim curNode As Node = tvwShips.SelectedNode
            If curNode IsNot Nothing Then
                If curNode.Nodes.Count = 0 Then ' If has no child nodes, therefore a ship not group
                    Dim shipName As String = curNode.Text
                    mnuShipBrowserShipName.Text = shipName
                    Call CreateNewFitting(shipName)
                End If
            End If
        End Sub
        Private Sub ctxShipBrowser_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxShipBrowser.Opening
            Dim curNode As Node = tvwShips.SelectedNode
            If curNode IsNot Nothing Then
                If curNode.Nodes.Count = 0 Then ' If has no child nodes, therefore a ship not group
                    Dim shipName As String = curNode.Text
                    mnuShipBrowserShipName.Text = shipName
                    ' Check for a current ship and whether there is a ship bay
                    If ActiveFitting IsNot Nothing Then
                        If ActiveFitting.ShipSlotCtrl IsNot Nothing Then
                            If ActiveFitting.FittedShip.ShipBay > 0 Then
                                mnuAddToShipBay.Enabled = True
                            Else
                                mnuAddToShipBay.Enabled = False
                            End If
                        Else
                            mnuAddToShipBay.Enabled = False
                        End If
                    Else
                        mnuAddToShipBay.Enabled = False
                    End If
                Else
                    e.Cancel = True
                End If
            Else
                e.Cancel = True
            End If
        End Sub
        Private Sub mnuCreateNewFitting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuCreateNewFitting.Click
            ' Get the ship details
            Dim shipName As String = mnuShipBrowserShipName.Text
            Call CreateNewFitting(shipName)
        End Sub
        Private Sub mnuPreviewShip_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuPreviewShip.Click
            Dim shipName As String = mnuShipBrowserShipName.Text
            Dim selShip As Ship = ShipLists.ShipList(shipName).Clone
            Dim showInfo As New FrmShowInfo
            Dim hPilot As EveHQPilot
            If ActiveFitting IsNot Nothing Then
                hPilot = HQ.Settings.Pilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString)
            Else
                If HQ.Settings.StartupPilot <> "" Then
                    hPilot = HQ.Settings.Pilots(HQ.Settings.StartupPilot)
                Else
                    hPilot = HQ.Settings.Pilots.Values(0)
                End If
            End If
            showInfo.ShowItemDetails(selShip, hPilot)
        End Sub
        Private Sub mnuBattleClinicBrowser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuBattleClinicBrowser.Click
            Dim shipName As String = mnuShipBrowserShipName.Text
            Dim bShip As Ship = ShipLists.ShipList(shipName).Clone
            If _myBcBrowser.IsHandleCreated = True Then
                _myBcBrowser.ShipType = bShip
                _myBcBrowser.BringToFront()
            Else
                _myBcBrowser = New FrmBcBrowser
                _myBcBrowser.ShipType = bShip
                _myBcBrowser.Show()
            End If
        End Sub
        Private Sub CreateNewFitting(ByVal shipName As String)
            ' Check we have some valid characters
            ' Bug 83: Adding a check of the core pilots collection as well, since it may end up in an unstable state due to other actors, and needs to contain pilots before loading the new fitting.
            If HQ.Settings.Pilots.Count > 0 And FittingPilots.HQFPilots.Count > 0 Then
                ' Clear the text boxes
                Using myNewFitting As New FrmModifyFittingName
                    Dim fittingName As String
                    With myNewFitting
                        .txtFittingName.Text = "" : .txtFittingName.Enabled = True
                        .btnAccept.Text = "Add" : .Tag = "Add"
                        .btnAccept.Tag = shipName
                        .Text = "Create New Fitting for " & shipName
                        .ShowDialog()
                        fittingName = .txtFittingName.Text
                    End With
                    If myNewFitting.DialogResult = DialogResult.Cancel Then
                        'MessageBox.Show("Create New Fitting has been cancelled!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        If fittingName <> "" Then
                            Dim newFit As New Fitting(shipName, fittingName, PluginSettings.HQFSettings.DefaultPilot)
                            Fittings.FittingList.Add(newFit.KeyName, newFit)
                            If CreateNewFittingTab(newFit) = True Then
                                Call UpdateFilteredShips()
                                tabHQF.SelectedTab = tabHQF.Tabs(newFit.KeyName)
                                If tabHQF.Tabs.Count = 1 Then
                                    Call UpdateSelectedTab()   ' Called when tabpage count=0 as SelectedIndexChanged does not fire!
                                End If
                                ActiveFitting.ShipSlotCtrl.UpdateEverything()
                            End If
                        Else
                            MessageBox.Show("Unable to create new fitting due to insufficient data!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End Using
            Else
                Dim msg As String = "There appears to be no pilots or accounts created in EveHQ." & ControlChars.CrLf
                msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
                msg &= ControlChars.CrLf & ControlChars.CrLf
                msg &= "If you have just added accounts or pilots with HQF open, please close the HQF plug-in and re-open it."
                MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub
        Private Sub txtShipSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtShipSearch.TextChanged
            Call UpdateFilteredShips()
        End Sub
        Private Sub UpdateFilteredShips()
            If Len(txtShipSearch.Text) > 0 Then
                Dim strSearch As String = txtShipSearch.Text.Trim.ToLower

                ' Redraw the ships tree
                Dim shipResults As New List(Of String)
                Dim isFlyable As Boolean
                For Each sShip As String In ShipLists.ShipList.Keys
                    If sShip.ToLower.Contains(strSearch) Then
                        If cboFlyable.SelectedIndex > 0 Then
                            isFlyable = IsShipFlyable(sShip, cboFlyable.SelectedItem.ToString)
                        Else
                            isFlyable = True
                        End If
                        If isFlyable = True Then
                            shipResults.Add(sShip)
                        End If
                    End If
                Next
                tvwShips.BeginUpdate()
                tvwShips.Nodes.Clear()
                For Each item As String In shipResults
                    Dim shipNode As New Node

                    Dim ship As Ship = ShipLists.ShipList(item)
                    Dim stt As New SuperTooltipInfo
                    stt.HeaderText = ship.Name
                    stt.FooterText = "HQF Ship Information - " & ship.Name
                    stt.BodyText &= "Slots - Hi:" & ship.HiSlots.ToString & " Mid:" & ship.MidSlots.ToString & " Low:" & ship.LowSlots.ToString & " Rig:" & ship.RigSlots.ToString & " T:" & ship.TurretSlots.ToString & " ML:" & ship.LauncherSlots.ToString & "<br />"
                    stt.BodyText &= "CPU: " & ship.CPU.ToString("N0") & ", PG: " & ship.PG.ToString("N0") & ", Cal: " & ship.Calibration.ToString("N0") & ", Cap: " & ship.CapCapacity.ToString("N0") & " (" & ship.CapRecharge.ToString("N0") & "s)<br />"
                    stt.BodyText &= "Shield: " & ship.ShieldCapacity.ToString("N0") & " (" & ship.ShieldRecharge.ToString("N0") & "s) - EM:" & ship.ShieldEMResist.ToString("N0") & "% Ex:" & ship.ShieldExResist.ToString("N0") & "% Ki:" & ship.ShieldKiResist.ToString("N0") & "% Th:" & ship.ShieldThResist.ToString("N0") & "%<br />"
                    stt.BodyText &= "Armor: " & ship.ArmorCapacity.ToString("N0") & " - EM:" & ship.ArmorEMResist.ToString("N0") & "% Ex:" & ship.ArmorExResist.ToString("N0") & "% Ki:" & ship.ArmorKiResist.ToString("N0") & "% Th:" & ship.ArmorThResist.ToString("N0") & "%<br />"
                    stt.BodyText &= "Hull: " & ship.StructureCapacity.ToString("N0") & " - EM:" & ship.StructureEMResist.ToString("N0") & "% Ex:" & ship.StructureExResist.ToString("N0") & "% Ki:" & ship.StructureKiResist.ToString("N0") & "% Th:" & ship.StructureThResist.ToString("N0") & "%<br />"
                    stt.BodyText &= "Targeting: " & (ship.MaxTargetRange / 1000).ToString("N0") & "km, (" & ship.MaxLockedTargets.ToString("N0") & "T), Scan Res: " & ship.ScanResolution.ToString("N0") & "mm, Sensors: " & (ship.GravSensorStrenth + ship.LadarSensorStrenth + ship.MagSensorStrenth + ship.RadarSensorStrenth).ToString("N0") & "<br />"
                    stt.BodyText &= "Cargo: " & ship.CargoBay.ToString("N0") & "m³ Drone Bay: " & ship.DroneBay.ToString("N0") & "m³ (B/W: " & ship.DroneBandwidth.ToString("N0") & "Mb/s)<br />"
                    stt.BodyText &= "Max Velocity: " & ship.MaxVelocity.ToString("N2") & "m/s, Warp: " & ship.WarpSpeed.ToString("N0") & "au/s"
                    stt.Color = eTooltipColor.Yellow
                    'stt.BodyImage = EveHQ.Core.ImageHandler.GetImage(Ship.ID, 96)
                    stt.FooterImage = My.Resources.imgInfo1
                    STTShips.SetSuperTooltip(shipNode, stt)

                    shipNode.Text = item
                    shipNode.Name = item
                    tvwShips.Nodes.Add(shipNode)
                Next
                tvwShips.EndUpdate()

                ' Redraw the fitting tree

                Dim shipName As String
                Dim fittingName As String
                Dim fittingSep As Integer
                Dim fitResults As New List(Of String)
                For Each sFit As String In Fittings.FittingList.Keys
                    If sFit.ToLower.Contains(strSearch) Then
                        fittingSep = sFit.IndexOf(", ", StringComparison.Ordinal)
                        shipName = sFit.Substring(0, fittingSep)
                        If cboFlyable.SelectedIndex > 0 Then
                            isFlyable = IsShipFlyable(shipName, cboFlyable.SelectedItem.ToString)
                        Else
                            isFlyable = True
                        End If
                        If isFlyable = True Then
                            fitResults.Add(sFit)
                        End If
                    End If
                Next

                ' Get Current List of "open" nodes
                Dim openNodes As New ArrayList
                For Each shipNode As Node In tvwFittings.Nodes
                    If shipNode.Expanded = True Then
                        openNodes.Add(shipNode.Text)
                    End If
                Next
                tvwFittings.BeginUpdate()
                tvwFittings.Nodes.Clear()
                For Each item As String In fitResults
                    fittingSep = item.IndexOf(", ", StringComparison.Ordinal)
                    shipName = item.Substring(0, fittingSep)
                    fittingName = item.Substring(fittingSep + 2)
                    ' Create the ship node if it's not already present
                    Dim containsShip As New Node
                    For Each ship As Node In tvwFittings.Nodes
                        If ship.Text = shipName Then
                            containsShip = ship
                        End If
                    Next
                    If containsShip.Text = "" Then
                        containsShip.Text = shipName
                        tvwFittings.Nodes.Add(containsShip)
                    End If
                    ' Add the details to the Node, checking for duplicates
                    containsShip.Nodes.Add(New Node(fittingName))
                Next
                ' Open the previously opened nodes
                For Each shipNode As Node In tvwFittings.Nodes
                    If openNodes.Contains(shipNode.Text) Then
                        shipNode.Expand()
                    End If
                Next
                tvwFittings.EndUpdate()
            Else
                txtShipSearch.Text = ""
                Call ShowShipGroups()
                Call UpdateFittingsTree(False)
            End If
        End Sub
        Private Sub UpdateShipFittings()
            If Len(txtShipSearch.Text) > 0 Then
                Dim strSearch As String = txtShipSearch.Text.Trim.ToLower

                ' Redraw the fitting tree
                Dim fitResults As New List(Of String)
                For Each sFit As String In Fittings.FittingList.Keys
                    If sFit.ToLower.Contains(strSearch) Then
                        fitResults.Add(sFit)
                    End If
                Next
                ' Get Current List of "open" nodes
                Dim openNodes As New ArrayList
                For Each shipNode As Node In tvwFittings.Nodes
                    If shipNode.Expanded = True Then
                        openNodes.Add(shipNode.Text)
                    End If
                Next
                tvwFittings.BeginUpdate()
                tvwFittings.Nodes.Clear()
                Dim shipName As String
                Dim fittingName As String
                Dim fittingSep As Integer
                For Each item As String In fitResults
                    fittingSep = item.IndexOf(", ", StringComparison.Ordinal)
                    shipName = item.Substring(0, fittingSep)
                    fittingName = item.Substring(fittingSep + 2)
                    ' Create the ship node if it's not already present
                    Dim containsShip As New Node
                    For Each ship As Node In tvwFittings.Nodes
                        If ship.Text = shipName Then
                            containsShip = ship
                        End If
                    Next
                    If containsShip.Text = "" Then
                        containsShip.Text = shipName
                        tvwFittings.Nodes.Add(containsShip)
                    End If
                    ' Add the details to the Node, checking for duplicates
                    containsShip.Nodes.Add(New Node(fittingName))
                Next
                ' Open the previously opened nodes
                For Each shipNode As Node In tvwFittings.Nodes
                    If openNodes.Contains(shipNode.Text) Then
                        shipNode.Expand()
                    End If
                Next
                tvwFittings.EndUpdate()
            Else
                Call UpdateFittingsTree(False)
            End If
        End Sub
        Private Sub btnResetShips_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetShips.Click
            txtShipSearch.Text = ""
            Call UpdateShipFilter()
        End Sub
        Private Sub cboFlyable_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFlyable.SelectedIndexChanged
            If _startUp = False Then
                Call ShowShipGroups()
                Call UpdateFittingsTree(False)
            End If
        End Sub
        Private Function IsShipFlyable(ByVal shipName As String, ByVal pilotName As String) As Boolean
            Dim testShip As Ship = ShipLists.ShipList(shipName)
            Dim testPilot As FittingPilot = FittingPilots.HQFPilots(pilotName)
            If testShip IsNot Nothing And testPilot IsNot Nothing Then
                Return Engine.IsFlyable(testPilot, testShip)
            Else
                Return False
            End If
        End Function
        Private Sub mnuAddToShipBay_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToShipBay.Click
            If ActiveFitting.ShipSlotCtrl IsNot Nothing Then
                Dim shipName As String = mnuShipBrowserShipName.Text
                ' Add the ship to the maintenance bay
                Dim shipType As Ship = ShipLists.ShipList(shipName).Clone
                ActiveFitting.AddShip(shipType, 1, False)
            End If
        End Sub

#End Region

#Region "Module Display, Filter and Search Options"

        Private Sub tvwItems_AfterNodeSelect(ByVal sender As Object, ByVal e As AdvTreeNodeEventArgs) Handles tvwItems.AfterNodeSelect
            Call CalculateFilteredModules(e.Node)
        End Sub
        Private Sub tvwItems_NodeClick1(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles tvwItems.NodeClick
            tvwItems.SelectedNode = e.Node
        End Sub
        Private Sub MetaFilterChange(ByVal sender As Object, ByVal e As EventArgs) Handles chkFilter1.CheckedChanged, chkFilter2.CheckedChanged, chkFilter4.CheckedChanged, chkFilter8.CheckedChanged, chkFilter16.CheckedChanged, chkFilter32.CheckedChanged, chkFilter8192.CheckedChanged
            If _startUp = False Then
                Dim chkBox As CheckBox = CType(sender, CheckBox)
                Dim changedFilter As Integer = CInt(chkBox.Tag)
                PluginSettings.HQFSettings.ModuleFilter = PluginSettings.HQFSettings.ModuleFilter Xor changedFilter
                If ModuleDisplay <> "" Then
                    Select Case ModuleDisplay
                        Case "Search"
                            Call ShowSearchedModules()
                        Case "Fitted"
                            Call ShowModulesThatWillFit()
                        Case Else
                            Call ShowFilteredModules()
                    End Select
                End If
            End If
        End Sub
        Private Sub chkApplySkills_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkApplySkills.CheckedChanged
            Call UpdateModuleList()
        End Sub
        Private Sub chkOnlyShowUsable_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOnlyShowUsable.CheckedChanged
            Call UpdateModuleList()
        End Sub
        Private Sub chkOnlyShowFittable_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOnlyShowFittable.CheckedChanged
            Call UpdateModuleList()
        End Sub
        Private Sub UpdateModuleList()
            If _startUp = False Then
                If ModuleDisplay <> "" Then
                    Select Case ModuleDisplay
                        Case "Search"
                            Call CalculateSearchedModules()
                        Case "Fitted"
                            If _lastSlotFitting.Count > 0 Then
                                Call CalculateModulesThatWillFit()
                            End If
                        Case "Favourites"
                            Call CalculateFilteredModules(tvwItems.SelectedNode)
                        Case "Recently Used"
                            Call CalculateFilteredModules(tvwItems.SelectedNode)
                        Case Else
                            Call CalculateFilteredModules(tvwItems.SelectedNode)
                    End Select
                End If
            End If
        End Sub
        Private Sub CalculateFilteredModules(ByVal groupNode As Node)
            Cursor = Cursors.WaitCursor
            Dim sMod As ShipModule
            Dim groupID As Integer
            Dim results As New SortedList(Of Integer, ShipModule)
            If groupNode.Name = "Favourites" Then
                ModuleDisplay = "Favourites"
                For Each modName As String In PluginSettings.HQFSettings.Favourites
                    If ModuleLists.ModuleListName.ContainsKey(modName) = True Then
                        sMod = ModuleLists.ModuleList(ModuleLists.ModuleListName(modName))
                        ' Add results in by name, module
                        If ActiveFitting IsNot Nothing Then
                            If chkApplySkills.Checked = True Then
                                sMod = ModuleLists.ModuleList(ModuleLists.ModuleListName(modName)).Clone
                                ActiveFitting.ApplySkillEffectsToModule(sMod, True)
                            End If
                        End If
                        If ActiveFitting IsNot Nothing Then
                            If chkOnlyShowUsable.Checked = True Then
                                If Engine.IsUsable(FittingPilots.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString), sMod) = True Then
                                    If chkOnlyShowFittable.Checked = True Then
                                        If Engine.IsFittable(sMod, ActiveFitting.FittedShip) = True And ActiveFitting.IsModulePermitted(sMod, True) = True Then
                                            results.Add(sMod.ID, sMod)
                                        End If
                                    Else
                                        results.Add(sMod.ID, sMod)
                                    End If
                                End If
                            Else
                                If chkOnlyShowFittable.Checked = True Then
                                    If Engine.IsFittable(sMod, ActiveFitting.FittedShip) = True And ActiveFitting.IsModulePermitted(sMod, True) = True Then
                                        results.Add(sMod.ID, sMod)
                                    End If
                                Else
                                    results.Add(sMod.ID, sMod)
                                End If
                            End If
                        Else
                            results.Add(sMod.ID, sMod)
                        End If
                    End If
                Next
                lblModuleDisplayType.Tag = "Displaying: Favourites"
                _lastModuleResults = results
            ElseIf groupNode.Name = "Recently Used" Then
                ModuleDisplay = "Recently Used"
                For Each modName As String In PluginSettings.HQFSettings.MRUModules
                    If ModuleLists.ModuleListName.ContainsKey(modName) = True Then
                        sMod = ModuleLists.ModuleList(ModuleLists.ModuleListName(modName))
                        ' Add results in by name, module
                        If ActiveFitting IsNot Nothing Then
                            If chkApplySkills.Checked = True Then
                                sMod = ModuleLists.ModuleList(ModuleLists.ModuleListName(modName)).Clone
                                ActiveFitting.ApplySkillEffectsToModule(sMod, True)
                            End If
                        End If
                        If ActiveFitting IsNot Nothing Then
                            If chkOnlyShowUsable.Checked = True Then
                                If Engine.IsUsable(FittingPilots.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString), sMod) = True Then
                                    If chkOnlyShowFittable.Checked = True Then
                                        If Engine.IsFittable(sMod, ActiveFitting.FittedShip) = True And ActiveFitting.IsModulePermitted(sMod, True) = True Then
                                            results.Add(sMod.ID, sMod)
                                        End If
                                    Else
                                        results.Add(sMod.ID, sMod)
                                    End If
                                End If
                            Else
                                If chkOnlyShowFittable.Checked = True Then
                                    If Engine.IsFittable(sMod, ActiveFitting.FittedShip) = True And ActiveFitting.IsModulePermitted(sMod, True) = True Then
                                        results.Add(sMod.ID, sMod)
                                    End If
                                Else
                                    results.Add(sMod.ID, sMod)
                                End If
                            End If
                        Else
                            results.Add(sMod.ID, sMod)
                        End If
                    End If
                Next
                lblModuleDisplayType.Tag = "Displaying: Recently Used"
                _lastModuleResults = results
            Else
                If groupNode.Nodes.Count = 0 Then
                    groupID = CInt(groupNode.Tag)
                    ModuleDisplay = CStr(groupID)
                    Call AddGroupResults(ModuleLists.ModuleList, groupID, results)
                    lblModuleDisplayType.Tag = Market.MarketGroupList(CStr(groupID))
                    lblModuleDisplayType.Tag = "Displaying: " & lblModuleDisplayType.Tag.ToString
                    _lastModuleResults = results
                Else
                    ' Check on the last results
                    If _lastModuleResults.Count > 0 Then
                        groupID = _lastModuleResults.Values(0).MarketGroup
                    Else
                        groupID = 0
                    End If
                    Call AddGroupResults(_lastModuleResults, groupID, results)
                    _lastModuleResults = results
                End If
            End If
            Call ShowFilteredModules()
            Cursor = Cursors.Default
        End Sub
        Private Sub AddGroupResults(ByVal moduleList As SortedList(Of Integer, ShipModule), ByVal groupID As Integer, ByRef results As SortedList(Of Integer, ShipModule))
            Dim sMod As ShipModule
            For Each shipMod As ShipModule In moduleList.Values
                If shipMod.MarketGroup = groupID Then
                    ' Add results in by name, module
                    sMod = ModuleLists.ModuleList(shipMod.ID)
                    If ActiveFitting IsNot Nothing Then
                        If chkApplySkills.Checked = True Then
                            sMod = ModuleLists.ModuleList(shipMod.ID).Clone
                            ActiveFitting.ApplySkillEffectsToModule(sMod, True)
                        End If
                    End If
                    If ActiveFitting IsNot Nothing Then
                        If chkOnlyShowUsable.Checked = True Then
                            If Engine.IsUsable(FittingPilots.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString), sMod) = True Then
                                If chkOnlyShowFittable.Checked = True Then
                                    If Engine.IsFittable(sMod, ActiveFitting.FittedShip) = True And ActiveFitting.IsModulePermitted(sMod, True) = True Then
                                        results.Add(sMod.ID, sMod)
                                    End If
                                Else
                                    results.Add(sMod.ID, sMod)
                                End If
                            End If
                        Else
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, ActiveFitting.FittedShip) = True And ActiveFitting.IsModulePermitted(sMod, True) = True Then
                                    results.Add(sMod.ID, sMod)
                                End If
                            Else
                                results.Add(sMod.ID, sMod)
                            End If
                        End If
                    Else
                        results.Add(sMod.ID, sMod)
                    End If
                End If
            Next
        End Sub
        Private Sub ShowFilteredModules()
            Call DisplaySelectedModules()
            ModuleDisplay = "Filter"
        End Sub
        Private Sub CalculateSearchedModules()
            Cursor = Cursors.WaitCursor
            Dim sMod As ShipModule
            If Len(txtSearchModules.Text) > 2 Then
                Dim strSearch As String = txtSearchModules.Text.Trim.ToLower
                Dim results As New SortedList(Of Integer, ShipModule)
                For Each item As KeyValuePair(Of String, Integer) In ModuleLists.ModuleListName
                    If item.Key.ToLower.Contains(strSearch) Then
                        ' Add results in by name, module
                        sMod = ModuleLists.ModuleList(item.Value)
                        If ActiveFitting IsNot Nothing Then
                            If chkApplySkills.Checked = True Then
                                sMod = ModuleLists.ModuleList(item.Value).Clone
                                ActiveFitting.ApplySkillEffectsToModule(sMod, True)
                            End If
                        End If
                        If ActiveFitting IsNot Nothing Then
                            If ActiveFitting.ShipInfoCtrl IsNot Nothing Then
                                If ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem IsNot Nothing Then
                                    If chkOnlyShowUsable.Checked = True Then
                                        If Engine.IsUsable(FittingPilots.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString), sMod) = True Then
                                            If chkOnlyShowFittable.Checked = True Then
                                                If Engine.IsFittable(sMod, ActiveFitting.FittedShip) = True And ActiveFitting.IsModulePermitted(sMod, True) = True Then
                                                    results.Add(sMod.ID, sMod)
                                                End If
                                            Else
                                                results.Add(sMod.ID, sMod)
                                            End If
                                        End If
                                    Else
                                        If chkOnlyShowFittable.Checked = True Then
                                            If Engine.IsFittable(sMod, ActiveFitting.FittedShip) = True And ActiveFitting.IsModulePermitted(sMod, True) = True Then
                                                results.Add(sMod.ID, sMod)
                                            End If
                                        Else
                                            results.Add(sMod.ID, sMod)
                                        End If
                                    End If
                                Else
                                    results.Add(sMod.ID, sMod)
                                End If
                            Else
                                results.Add(sMod.ID, sMod)
                            End If
                        Else
                            results.Add(sMod.ID, sMod)
                        End If
                    End If
                Next
                _lastModuleResults = results
                lblModuleDisplayType.Tag = "Displaying: Matching *" & txtSearchModules.Text & "*"
                Call ShowSearchedModules()
            End If
            Cursor = Cursors.Default
        End Sub
        Private Sub ShowSearchedModules()
            Call DisplaySelectedModules()
            ModuleDisplay = "Search"
        End Sub
        Private Sub UpdateModulesThatWillFit(ByVal modData As ArrayList)
            _lastSlotFitting = modData
            If _lastSlotFitting.Count > 0 Then
                Call CalculateModulesThatWillFit()
            End If
        End Sub
        Private Sub CalculateModulesThatWillFit()
            Cursor = Cursors.WaitCursor
            Dim slotType As Integer = CInt(_lastSlotFitting(0))
            Dim cpu As Double = CDbl(_lastSlotFitting(1))
            Dim pg As Double = CDbl(_lastSlotFitting(2))
            Dim calibration As Double = CDbl(_lastSlotFitting(3))
            Dim launcherSlots As Integer = CInt(_lastSlotFitting(4))
            Dim turretSlots As Integer = CInt(_lastSlotFitting(5))
            Dim results As New SortedList(Of Integer, ShipModule)
            Dim sMod As ShipModule
            For Each cMod As ShipModule In ModuleLists.ModuleList.Values
                sMod = cMod
                If ActiveFitting IsNot Nothing Then
                    If chkApplySkills.Checked = True Then
                        sMod = cMod.Clone
                        ActiveFitting.ApplySkillEffectsToModule(sMod, True)
                    End If
                    If ActiveFitting.IsModulePermitted(sMod, True) = False Then
                        Continue For
                    End If
                    If ActiveFitting.ShipSlotCtrl IsNot Nothing Then
                        If sMod.SlotType = slotType Then
                            Select Case slotType
                                Case SlotTypes.Subsystem
                                    If chkOnlyShowUsable.Checked = True Then
                                        If Engine.IsUsable(FittingPilots.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString), sMod) = True Then
                                            results.Add(sMod.ID, sMod)
                                        End If
                                    Else
                                        results.Add(sMod.ID, sMod)
                                    End If
                                Case SlotTypes.Rig
                                    If sMod.Calibration <= calibration Then
                                        If chkOnlyShowUsable.Checked = True Then
                                            If Engine.IsUsable(FittingPilots.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString), sMod) = True Then
                                                results.Add(sMod.ID, sMod)
                                            End If
                                        Else
                                            results.Add(sMod.ID, sMod)
                                        End If
                                    End If
                                Case SlotTypes.Low, SlotTypes.Mid
                                    If sMod.CPU <= cpu Then
                                        If sMod.PG <= pg Then
                                            If chkOnlyShowUsable.Checked = True Then
                                                If Engine.IsUsable(FittingPilots.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString), sMod) = True Then
                                                    results.Add(sMod.ID, sMod)
                                                End If
                                            Else
                                                results.Add(sMod.ID, sMod)
                                            End If
                                        End If
                                    End If
                                Case SlotTypes.High
                                    If sMod.CPU <= cpu Then
                                        If sMod.PG <= pg Then
                                            If launcherSlots >= Math.Abs(CInt(sMod.IsLauncher)) Then
                                                If turretSlots >= Math.Abs(CInt(sMod.IsTurret)) Then
                                                    If chkOnlyShowUsable.Checked = True Then
                                                        If Engine.IsUsable(FittingPilots.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString), sMod) = True Then
                                                            results.Add(sMod.ID, sMod)
                                                        End If
                                                    Else
                                                        results.Add(sMod.ID, sMod)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                            End Select
                        End If
                    End If
                End If
            Next
            _lastModuleResults = results
            lblModuleDisplayType.Tag = "Displaying: Modules That Fit"
            Call ShowModulesThatWillFit()
            Cursor = Cursors.Default
        End Sub
        Private Sub ShowModulesThatWillFit()
            Call DisplaySelectedModules()
            ModuleDisplay = "Fitted"
        End Sub
        Private Sub txtSearchModules_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearchModules.TextChanged
            Call CalculateSearchedModules()
        End Sub
        Private Sub DisplaySelectedModules()

            'Dim startTime, endTime As DateTime
            'startTime = Now
            ' Reset checkbox colours
            chkFilter1.ForeColor = Color.Red
            chkFilter2.ForeColor = Color.Red
            chkFilter4.ForeColor = Color.Red
            chkFilter8.ForeColor = Color.Red
            chkFilter16.ForeColor = Color.Red
            chkFilter32.ForeColor = Color.Red
            chkFilter8192.ForeColor = Color.Red

            tvwModules.BeginUpdate()
            tvwModules.Nodes.Clear()


            For Each shipmod As ShipModule In _lastModuleResults.Values
                If shipmod.SlotType <> 0 Or (shipmod.SlotType = 0 And (shipmod.IsBooster Or shipmod.IsCharge Or shipmod.IsDrone Or shipmod.DatabaseCategory = 22)) Then
                    If (shipmod.MetaType And PluginSettings.HQFSettings.ModuleFilter) = shipmod.MetaType Then
                        Dim newModule As New Node
                        newModule.Name = CStr(shipmod.ID)
                        newModule.Text = shipmod.Name
                        newModule.Cells.Add(New Cell(shipmod.MetaLevel.ToString))
                        newModule.Cells.Add(New Cell(shipmod.Cpu.ToString))
                        newModule.Cells.Add(New Cell(shipmod.Pg.ToString))
                        ' newModule.Cells.Add(New Cell(CStr(costTable(shipmod.ID))))
                        newModule.Cells.Add(New Cell("Processing...")) 'Place holder for the price data that will update afterward.
                        newModule.Cells(4).TextDisplayFormat = "N2"
                        newModule.Style = New ElementStyle
                        newModule.Style.Font = Font
                        ' Create drone icons individually because drones have no iconID
                        If shipmod.IsDrone = True Then
                            newModule.Image = ImageHandler.CreateIcon(CStr(shipmod.ID), shipmod.MetaType.ToString, 24, True)
                        Else
                            newModule.Image = ImageHandler.IconImage24(shipmod.Icon, shipmod.MetaType)
                        End If
                        Dim stt As New SuperTooltipInfo
                        stt.HeaderText = shipmod.Name
                        stt.BodyText = ""
                        If shipmod.SlotType = SlotTypes.Subsystem Then
                            stt.BodyText &= "Slot Modifiers - High: " & shipmod.Attributes(1374) & ", Mid: " & shipmod.Attributes(1375) & ", Low: " & shipmod.Attributes(1376) & ControlChars.CrLf & ControlChars.CrLf
                            stt.FooterText = "Subsystem Module Information"
                        Else
                            stt.FooterText = " Meta: " & shipmod.MetaLevel.ToString("N0") & ", CPU: " & shipmod.Cpu.ToString("N0") & ", PG: " & shipmod.Pg.ToString("N0")
                        End If
                        stt.BodyText &= shipmod.Description
                        stt.Color = eTooltipColor.Yellow
                        'stt.FooterImage = CType(My.Resources.imgInfo1, Image)
                        ' Create drone icons individually because drones have no iconID
                        If shipmod.IsDrone = True Then
                            stt.BodyImage = ImageHandler.CreateIcon(CStr(shipmod.ID), shipmod.MetaType.ToString, 48, True)
                        Else
                            stt.BodyImage = ImageHandler.IconImage48(shipmod.Icon, shipmod.MetaType)
                        End If
                        SuperTooltip1.SetSuperTooltip(newModule, stt)
                        Select Case shipmod.SlotType
                            Case SlotTypes.Subsystem
                                newModule.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.SubSlotColour))
                            Case SlotTypes.High
                                newModule.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.HiSlotColour))
                            Case SlotTypes.Mid
                                newModule.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.MidSlotColour))
                            Case SlotTypes.Low
                                newModule.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.LowSlotColour))
                            Case SlotTypes.Rig
                                newModule.Style.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.RigSlotColour))
                        End Select
                        Dim chkFilter As CheckBox = CType(panelModFilters.Controls("chkFilter" & shipmod.MetaType), CheckBox)
                        If chkFilter IsNot Nothing Then
                            chkFilter.ForeColor = Color.Black
                        End If
                        tvwModules.Nodes.Add(newModule)
                    Else
                        Dim chkFilter As CheckBox = CType(panelModFilters.Controls("chkFilter" & shipmod.MetaType), CheckBox)
                        If chkFilter IsNot Nothing Then
                            chkFilter.ForeColor = Color.LimeGreen
                        End If
                    End If
                End If
            Next

            'Update pricing data
            'query for the prices of each of the modules. This iterates the collection more than once, but it is more efficient than potientially making a dozen or more web requests
            Dim priceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From modules In _lastModuleResults.Values Let mods = modules Select CInt(mods.ID))

            'Update UI when pricing data is acquired
            priceTask.ContinueWith(Sub(task As Task(Of Dictionary(Of Integer, Double)))
                                       Dim prices As Dictionary(Of Integer, Double) = task.Result
                                       'Bug EVEHQ-169 : this is called even after the window is destroyed but not GC'd. check the handle boolean first.
                                       If IsHandleCreated Then
                                           ' Switch to UI thread
                                           Invoke(Sub()
                                                      For Each moduleNode As Node In tvwModules.Nodes
                                                          Dim price As Double
                                                          If IsNumeric(moduleNode.Name) Then
                                                              If prices.TryGetValue(CInt(moduleNode.Name), price) Then
                                                                  moduleNode.Cells(4).Text = price.ToInvariantString("N2")
                                                              End If
                                                          End If
                                                      Next

                                                  End Sub)
                                       End If
                                   End Sub)

            If tvwModules.Nodes.Count = 0 Then
                tvwModules.Nodes.Add(New Node("<Empty - Please check filters>"))
                tvwModules.Enabled = False
                If lblModuleDisplayType.Tag IsNot Nothing Then
                    lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (0 items)"
                End If
            Else
                tvwModules.Enabled = True
                lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (" & tvwModules.Nodes.Count & " items)"
            End If
            AdvTreeSorter.Sort(tvwModules, PluginSettings.HQFSettings.SortedModuleListInfo, False)
            tvwModules.EndUpdate()
            tvwModules.Nodes(0).EnsureVisible()
            'endTime = Now
            'MessageBox.Show((endTime - startTime).TotalMilliseconds.ToString & "ms", "DisplayModules Time", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

#End Region

#Region "Module List Routines"

        Private Sub tvwModules_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles tvwModules.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            PluginSettings.HQFSettings.SortedModuleListInfo = AdvTreeSorter.Sort(ch, True, False)
        End Sub
        Private Sub tvwModules_ColumnResized(ByVal sender As Object, ByVal e As EventArgs) Handles tvwModules.ColumnResized
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            Dim idx As Integer = ch.DisplayIndex - 1
            PluginSettings.HQFSettings.ModuleListColWidths(CLng(idx)) = ch.Width.Absolute
        End Sub
        Private Sub tvwModules_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles tvwModules.NodeDoubleClick
            If ActiveFitting IsNot Nothing Then
                If ActiveFitting.ShipSlotCtrl IsNot Nothing Then
                    If tvwModules.SelectedNodes.Count > 0 Then
                        Dim moduleID As Integer = CInt(e.Node.Name)
                        Dim shipMod As ShipModule = ModuleLists.ModuleList(moduleID).Clone
                        If shipMod.IsDrone = True Then
                            Call ActiveFitting.AddDrone(shipMod, 1, False, False)
                        Else
                            ' Check if module is a charge
                            If shipMod.IsCharge = True Or shipMod.IsContainer Or shipMod.DatabaseCategory = 22 Then
                                ActiveFitting.AddItem(shipMod, 1, False)
                            Else
                                ' Must be a proper module then!
                                ActiveFitting.AddModule(shipMod, 0, True, False, Nothing, False, False)
                                ' Add it to the MRU
                                Call UpdateMruModules(shipMod.Name)
                            End If
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
#End Region

        Public Sub UpdateFittings()
            Cursor = Cursors.WaitCursor
            ' Updates all the open fittings
            For Each thisTab As TabItem In tabHQF.Tabs
                Dim thisFit As Fitting = Fittings.FittingList(thisTab.Text)
                thisFit.ShipSlotCtrl.UpdateEverything()
            Next
            Cursor = Cursors.Default
        End Sub

        Public Sub UpdateAllImplantLists()
            Cursor = Cursors.WaitCursor
            ' Updates all implant lists in the open fittings
            For Each thisTab As TabItem In tabHQF.Tabs
                Dim thisFit As Fitting = Fittings.FittingList(thisTab.Text)
                Try
                    thisFit.ShipInfoCtrl.UpdateImplantList()
                Catch e As Exception
                    Dim msg As New StringBuilder
                    msg.AppendLine("Error: " & e.Message)
                    msg.AppendLine("Tab Text: " & thisTab.Text)
                    If thisFit IsNot Nothing Then
                        msg.AppendLine("Fitting: " & thisFit.KeyName)
                    Else
                        msg.AppendLine("Fitting: Nothing!!")
                    End If
                    If thisFit.ShipInfoCtrl IsNot Nothing Then
                        msg.AppendLine("ShipInfo: Present")
                    Else
                        msg.AppendLine("ShipInfo: Missing!!")
                    End If
                    If thisFit.ShipSlotCtrl IsNot Nothing Then
                        msg.AppendLine("ShipSlot: Present")
                    Else
                        msg.AppendLine("ShipSlot: Missing!!")
                    End If
                    MessageBox.Show(msg.ToString, "Error Updating Implants", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            Cursor = Cursors.Default
        End Sub

        Public Sub UpdateShipInfo(ByVal pilotName As String)
            ' Updates all the open fittings
            For Each thisTab As TabItem In tabHQF.Tabs
                If thisTab IsNot Nothing Then
                    If thisTab.AttachedControl IsNot Nothing Then
                        Dim shipSlots As ShipSlotControl = CType(thisTab.AttachedControl.Controls("panelShipSlot").Controls("shipSlot"), ShipSlotControl)
                        Dim shipInfo As ShipInfoControl = CType(thisTab.AttachedControl.Controls("panelShipInfo").Controls("shipInfo"), ShipInfoControl)
                        If pilotName = shipInfo.cboPilots.SelectedItem.ToString Then
                            ' Build the Affections data for this pilot
                            Dim shipPilot As FittingPilot = FittingPilots.HQFPilots(shipInfo.cboPilots.SelectedItem.ToString)
                            ' Call the property modifier again which will trigger the fitting routines and update all slots for the new pilot
                            If shipSlots IsNot Nothing Then
                                shipSlots.UpdateAllSlots = True
                            End If
                            shipInfo.cboImplants.Tag = "Updating"
                            If shipPilot.ImplantName(0) IsNot Nothing Then
                                shipInfo.cboImplants.SelectedItem = shipPilot.ImplantName(0)
                            End If
                            shipInfo.cboImplants.Tag = Nothing
                            shipInfo.ParentFitting.ApplyFitting(BuildType.BuildEverything)
                            If shipSlots IsNot Nothing Then
                                shipSlots.UpdateAllSlots = False
                            End If
                        End If
                    End If
                End If
            Next
            HQFEvents.StartUpdateModuleList = True
        End Sub


#Region "TabHQF Selection and Context Menu Routines"

        Private Sub tabHQF_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tabHQF.SelectedTabChanged
            Call UpdateSelectedTab()
        End Sub
        Private Sub UpdateSelectedTab()
            If tabHQF.SelectedTab IsNot Nothing Then
                ActiveFitting = Fittings.FittingList(tabHQF.SelectedTab.Text)
            End If
        End Sub
#End Region

#Region "Clipboard Paste Routines (incl Timer)"
        Private Sub tmrClipboard_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrClipboard.Tick
            ' Checks the clipboard for any compatible items!
            Try
                If Clipboard.GetDataObject IsNot Nothing Then
                    Dim fileText As String = CStr(Clipboard.GetDataObject().GetData(DataFormats.Text))
                    If fileText IsNot Nothing Then
                        Dim fittingMatch As Match = Regex.Match(fileText, "\[(?<ShipName>[^,]*)\]|\[(?<ShipName>.*),\s?(?<FittingName>.*)\]")
                        If fittingMatch.Success = True Then
                            ' Appears to be a match so lets check the ship type
                            If ShipLists.ShipList.ContainsKey(fittingMatch.Groups.Item("ShipName").Value) = True Then
                                btnImport.Enabled = True
                            Else
                                btnImport.Enabled = False
                            End If
                        Else
                            btnImport.Enabled = False
                        End If
                    Else
                        btnImport.Enabled = False
                    End If
                Else
                    btnImport.Enabled = False
                End If
            Catch ex As Exception
                btnImport.Enabled = False
            End Try
        End Sub

#End Region

#Region "Fitting Panel Routines"

        Private Sub UpdateFittingsTree(ByVal collapseAllNodes As Boolean)
            tvwFittings.BeginUpdate()
            ' Get current list of "open" nodes if we need the feature
            Dim openNodes As New ArrayList
            If CollapseAllNodes = False Then
                For Each shipNode As Node In tvwFittings.Nodes
                    If shipNode.Expanded = True Then
                        openNodes.Add(shipNode.Text)
                    End If
                Next
            End If
            ' Redraw the tree
            tvwFittings.Nodes.Clear()
            Dim shipName As String
            Dim fittingName As String
            Dim isFlyable As Boolean

            If Fittings.FittingList.Count > 0 Then
                For Each fitting As String In Fittings.FittingList.Keys
                    shipName = Fittings.FittingList(fitting).ShipName
                    fittingName = Fittings.FittingList(fitting).FittingName

                    ' Create the ship node if it's not already present
                    Dim containsShip As New Node
                    For Each ship As Node In tvwFittings.Nodes
                        If ship.Text = shipName Then
                            containsShip = ship
                        End If
                    Next
                    If containsShip.Text = "" Then
                        containsShip.Text = shipName
                        tvwFittings.Nodes.Add(containsShip)
                    End If

                    ' Add the details to the Node, checking for duplicates
                    If cboFlyable.SelectedIndex > 0 Then
                        isFlyable = IsShipFlyable(shipName, cboFlyable.SelectedItem.ToString)
                    Else
                        isFlyable = True
                    End If
                    If isFlyable = True Then
                        Dim containsFitting As New Node
                        For Each fit As Node In containsShip.Nodes
                            If fit.Text = fittingName Then
                                MessageBox.Show("Duplicate fitting found for " & shipName & ", and omitted", "Duplicate Fitting Found!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                containsFitting = Nothing
                                Exit For
                            End If
                        Next
                        If containsFitting IsNot Nothing Then
                            containsFitting.Text = fittingName
                            containsShip.Nodes.Add(containsFitting)
                        End If
                    End If
                Next
                ' Remove any parent nodes with no children
                Dim fNodeID As Integer = 0
                Do
                    If tvwFittings.Nodes(fNodeID).Nodes.Count = 0 Then
                        tvwFittings.Nodes.Remove(tvwFittings.Nodes(fNodeID))
                        fNodeID -= 1
                    End If
                    fNodeID += 1
                Loop Until fNodeID = tvwFittings.Nodes.Count
                ' Open the previously opened nodes
                If CollapseAllNodes = False Then
                    For Each shipNode As Node In tvwFittings.Nodes
                        If openNodes.Contains(shipNode.Text) Then
                            shipNode.Expand()
                        End If
                    Next
                End If
            End If
            tvwFittings.EndUpdate()
        End Sub

        Private Function CreateNewFittingTab(ByVal newFit As Fitting) As Boolean
            If ShipLists.ShipList.ContainsKey(NewFit.ShipName) = True Then
                NewFit.BaseShip.DamageProfile = HQFDamageProfiles.ProfileList.Item("<Omni-Damage>")
                ShipLists.FittedShipList.Add(NewFit.KeyName, NewFit.BaseShip)

                tabHQF.SuspendLayout()

                ' Create the tab page
                Dim tp As TabItem = tabHQF.CreateTab(NewFit.KeyName)
                tp.Tag = NewFit.KeyName
                tp.Name = NewFit.KeyName

                ' Create the Ship Slot panel
                Dim pss As New Panel
                pss.BorderStyle = BorderStyle.Fixed3D
                pss.Dock = DockStyle.Fill
                pss.Location = New Point(0, 0)
                pss.Name = "panelShipSlot"
                pss.Size = New Size(414, 600)
                pss.TabIndex = 1

                ' Create the Ship Info Panel
                Dim psi As New Panel
                psi.Dock = DockStyle.Left
                psi.Location = New Point(0, 384)
                psi.Name = "panelShipInfo"
                psi.Size = New Size(270, 600)
                psi.TabIndex = 0

                ' Attach the panels to the tab page
                tp.AttachedControl.Controls.Add(pss)
                tp.AttachedControl.Controls.Add(psi)

                ' Create a new Ship Slot Control
                Dim shipSlot As New ShipSlotControl(NewFit)
                shipSlot.Name = "shipSlot"
                shipSlot.Location = New Point(0, 0)
                shipSlot.Dock = DockStyle.Fill
                pss.Controls.Add(shipSlot)
                ' TODO: Check if a custom ship - this should be done in the constructor of the SSC
                Dim baseID As Integer
                If CustomHQFClasses.CustomShipIDs.ContainsKey(NewFit.BaseShip.ID) Then
                    baseID = ShipLists.ShipListKeyName(CustomHQFClasses.CustomShips(NewFit.BaseShip.Name).BaseShipName)
                Else
                    baseID = NewFit.BaseShip.ID
                End If
                shipSlot.pbShip.Image = Core.ImageHandler.GetImage(CInt(baseID), 32)

                ' Create a new Ship Info Control
                Dim shipInfo As New ShipInfoControl(NewFit)
                shipInfo.Name = "shipInfo"
                shipInfo.Location = New Point(0, 0)
                shipInfo.Dock = DockStyle.Fill
                psi.Controls.Add(shipInfo)

                ' Set the ship controls to the fitting
                NewFit.ShipInfoCtrl = shipInfo
                NewFit.ShipSlotCtrl = shipSlot

                tabHQF.ResumeLayout()
                Return True
            Else
                Dim msg As String = NewFit.ShipName & " is no longer a valid ship type."
                MessageBox.Show(msg, "Unknown Ship Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End Function

        Private Sub ctxFittings_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxFittings.Opening
            If tvwFittings.SelectedNodes.Count < 2 Then
                If tvwFittings.SelectedNodes.Count = 0 Then
                    e.Cancel = True
                Else
                    Dim curNode As Node = tvwFittings.SelectedNodes(0)
                    If curNode IsNot Nothing Then
                        If curNode.Nodes.Count = 0 Then
                            Dim parentNode As Node = curNode.Parent
                            mnuFittingsFittingName.Text = parentNode.Text & ", " & curNode.Text
                            mnuFittingsFittingName.Tag = parentNode.Text
                            mnuFittingsCreateFitting.Text = "Create New " & parentNode.Text & " Fitting"
                            mnuFittingsCreateFitting.Enabled = True
                            mnuFittingsBCBrowser.Enabled = True
                            mnuFittingsCopyFitting.Enabled = True
                            mnuFittingsDeleteFitting.Text = "Delete Fitting"
                            mnuFittingsDeleteFitting.Enabled = True
                            mnuFittingsRenameFitting.Enabled = True
                            mnuFittingsShowFitting.Enabled = True
                            mnuPreviewShip2.Enabled = True
                            mnuExportToEve.Enabled = True
                            mnuExportToRequisitions.Enabled = True
                        Else
                            mnuFittingsFittingName.Text = curNode.Text
                            mnuFittingsFittingName.Tag = curNode.Text
                            mnuFittingsCreateFitting.Text = "Create New " & curNode.Text & " Fitting"
                            mnuFittingsCreateFitting.Enabled = True
                            mnuFittingsBCBrowser.Enabled = True
                            mnuFittingsCopyFitting.Enabled = False
                            mnuFittingsDeleteFitting.Text = "Delete All Ship Fittings"
                            mnuFittingsDeleteFitting.Enabled = True
                            mnuFittingsRenameFitting.Enabled = False
                            mnuFittingsShowFitting.Enabled = False
                            mnuPreviewShip2.Enabled = True
                            mnuExportToEve.Enabled = True
                            mnuExportToRequisitions.Enabled = True
                        End If
                    Else
                        e.Cancel = True
                    End If
                End If
            Else
                mnuFittingsFittingName.Text = "[Multiple Selection]"
                mnuFittingsFittingName.Tag = "[Multiple Selection]"
                mnuFittingsCreateFitting.Text = "[Multiple Selection]"
                mnuFittingsCreateFitting.Enabled = False
                mnuFittingsBCBrowser.Enabled = False
                mnuFittingsCopyFitting.Enabled = False
                mnuFittingsDeleteFitting.Text = "Delete Mulitple Fittings"
                mnuFittingsDeleteFitting.Enabled = True
                mnuFittingsRenameFitting.Enabled = False
                mnuFittingsShowFitting.Enabled = False
                mnuPreviewShip2.Enabled = False
                mnuExportToEve.Enabled = True
                mnuExportToRequisitions.Enabled = True
            End If
        End Sub
        Private Sub mnuFittingsShowFitting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFittingsShowFitting.Click
            ' Get the node details
            Dim fittingnode As Node = tvwFittings.SelectedNodes(0)
            Call ShowFitting(fittingnode)
        End Sub
        Private Sub mnuFittingsRenameFitting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFittingsRenameFitting.Click
            ' Get the node details
            Dim curNode As Node = tvwFittings.SelectedNodes(0)
            Dim parentnode As Node = curNode.Parent
            Dim shipName As String = parentnode.Text
            Dim fitName As String = curNode.Text
            Dim oldKeyName As String = shipName & ", " & fitName
            Dim fitToCopy As Fitting = Fittings.FittingList(oldKeyName)

            ' Clear the text boxes
            Using myNewFitting As New FrmModifyFittingName
                Dim fittingName As String
                With myNewFitting
                    .txtFittingName.Text = fitName : .txtFittingName.Enabled = True
                    .btnAccept.Text = "Edit" : .Tag = "Edit"
                    .btnAccept.Tag = shipName
                    .Text = "Edit Fitting Name for " & shipName
                    .ShowDialog()
                    fittingName = .txtFittingName.Text
                End With

                If myNewFitting.DialogResult = DialogResult.Cancel Then
                    'MessageBox.Show("Rename Fitting has been cancelled!", "Rename Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    If fittingName <> "" Then
                        Fittings.FittingList.Remove(oldKeyName)
                        Dim newFit As Fitting = fitToCopy.Clone(fitToCopy.ShipSlotCtrl, fitToCopy.ShipInfoCtrl)
                        newFit.FittingName = fittingName
                        Fittings.FittingList.Add(newFit.KeyName, newFit)
                        ' Amend it in the tabs if it's there!
                        Dim tp As TabItem = tabHQF.Tabs(oldKeyName)
                        If tp IsNot Nothing Then
                            Dim copyShip As Ship = ShipLists.FittedShipList(oldKeyName).Clone
                            ShipLists.FittedShipList.Remove(oldKeyName)
                            ShipLists.FittedShipList.Add(newFit.KeyName, copyShip)
                            tp.Name = newFit.KeyName
                            tp.Tag = newFit.KeyName
                            tp.Text = newFit.KeyName
                        End If
                        Call UpdateFilteredShips()
                    Else
                        MessageBox.Show("Unable to rename fitting due to insufficient data!", "Rename Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End Using
        End Sub
        Private Sub mnuFittingsCopyFitting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFittingsCopyFitting.Click
            ' Get the node details
            Dim curNode As Node = tvwFittings.SelectedNodes(0)
            Dim parentnode As Node = curNode.Parent
            Dim shipName As String = parentnode.Text
            Dim fitName As String = curNode.Text
            Dim fitKeyName As String = shipName & ", " & fitName
            Dim fitToCopy As Fitting = Fittings.FittingList(fitKeyName)

            ' Clear the text boxes
            Using myNewFitting As New FrmModifyFittingName
                Dim fittingName As String
                With myNewFitting
                    .txtFittingName.Text = fitName : .txtFittingName.Enabled = True : .txtFittingName.SelectionStart = fitName.Length
                    .btnAccept.Text = "Copy" : .Tag = "Copy"
                    .btnAccept.Tag = shipName
                    .Text = "Copy '" & fitName & "' Fitting for " & shipName
                    .ShowDialog()
                    fittingName = .txtFittingName.Text
                End With
                If myNewFitting.DialogResult = DialogResult.Cancel Then
                    'MessageBox.Show("Copy Fitting has been cancelled!", "Copy Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    If fittingName <> "" Then
                        Dim newFit As Fitting = fitToCopy.Clone
                        newFit.FittingName = fittingName
                        Fittings.FittingList.Add(newFit.KeyName, newFit)
                        If CreateNewFittingTab(newFit) = True Then
                            Call UpdateFilteredShips()
                            tabHQF.SelectedTab = tabHQF.Tabs(newFit.KeyName)
                            If tabHQF.SelectedTabIndex = 0 Then Call UpdateSelectedTab()
                            ActiveFitting.ShipSlotCtrl.UpdateEverything()
                        End If
                        Call UpdateFilteredShips()
                    Else
                        MessageBox.Show("Unable to copy fitting due to insufficient data!", "Copy Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End Using
        End Sub
        Private Sub mnuFittingsDeleteFitting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFittingsDeleteFitting.Click
            ' Get the node details
            Dim response As Integer
            If tvwFittings.SelectedNodes.Count = 1 Then
                Dim curNode As Node = tvwFittings.SelectedNodes(0)
                If curNode.Level = 0 Then
                    ' Ship parent node
                    Dim shipName As String = curNode.Text
                    response = MessageBox.Show("Are you sure you wish to delete all the fittings for the " & shipName & "?", "Confirm Fitting Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Else
                    ' Fitting node
                    Dim parentnode As Node = curNode.Parent
                    Dim shipName As String = parentnode.Text
                    Dim fitName As String = curNode.Text
                    response = MessageBox.Show("Are you sure you wish to delete the '" & fitName & "' Fitting for the " & shipName & "?", "Confirm Fitting Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                End If
            Else
                response = MessageBox.Show("Are you sure you wish to delete these multiple fittings?", "Confirm Fitting Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If
            ' Get confirmation of deletion
            If response = DialogResult.Yes Then
                For Each curNode As Node In tvwFittings.SelectedNodes
                    Dim fittingKeyName As String = ""
                    Select Case curNode.Level
                        Case 0 ' Ship Level
                            For Each subNode As Node In curNode.Nodes
                                Dim parentnode As Node = subNode.Parent
                                Dim shipName As String = parentnode.Text
                                Dim fitName As String = subNode.Text
                                fittingKeyName = shipName & ", " & fitName
                                ' Remove the fit from the list
                                If Fittings.FittingList.ContainsKey(fittingKeyName) Then
                                    Fittings.FittingList.Remove(fittingKeyName)
                                End If
                                ' Delete it from the tabs if it's there!
                                Dim ti As TabItem = tabHQF.Tabs(fittingKeyName)
                                If ti IsNot Nothing Then
                                    tabHQF.Tabs.Remove(ti)
                                    ShipLists.FittedShipList.Remove(ti.Text)
                                End If
                            Next
                        Case 1 ' Fitting Level
                            Dim parentnode As Node = curNode.Parent
                            Dim shipName As String = parentnode.Text
                            Dim fitName As String = curNode.Text
                            fittingKeyName = shipName & ", " & fitName
                    End Select
                    ' Remove the fit from the list
                    If Fittings.FittingList.ContainsKey(fittingKeyName) Then
                        Fittings.FittingList.Remove(fittingKeyName)
                    End If
                    ' Delete it from the tabs if it's there!
                    Dim tp As TabItem = tabHQF.Tabs(fittingKeyName)
                    If tp IsNot Nothing Then
                        tabHQF.Tabs.Remove(tp)
                        ShipLists.FittedShipList.Remove(tp.Text)
                    End If
                Next
                ' Update the list
                Call UpdateFilteredShips()
            End If
        End Sub
        Private Sub mnuFittingsCreateFitting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFittingsCreateFitting.Click
            ' Get the node details
            Dim shipName As String = mnuFittingsFittingName.Tag.ToString

            ' Clear the text boxes
            Dim fittingName As String
            Using myNewFitting As New FrmModifyFittingName
                With myNewFitting
                    .txtFittingName.Text = "" : .txtFittingName.Enabled = True
                    .btnAccept.Text = "Add" : .Tag = "Add"
                    .btnAccept.Tag = shipName
                    .Text = "Create New Fitting for " & shipName
                    .ShowDialog()
                    fittingName = .txtFittingName.Text
                End With
            End Using

            ' Add the Fitting
            If fittingName <> "" Then
                Dim newFit As New Fitting(shipName, fittingName, PluginSettings.HQFSettings.DefaultPilot)
                Fittings.FittingList.Add(newFit.KeyName, newFit)
                If CreateNewFittingTab(newFit) = True Then
                    Call UpdateFilteredShips()
                    tabHQF.SelectedTab = tabHQF.Tabs(newFit.KeyName)
                    If tabHQF.SelectedTabIndex = 0 Then Call UpdateSelectedTab()
                    ActiveFitting.ShipSlotCtrl.UpdateEverything()
                End If
                Call UpdateFilteredShips()
            Else
                'MessageBox.Show("Unable to Create New Fitting!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub
        Private Sub mnuPreviewShip2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuPreviewShip2.Click
            Dim shipName As String = mnuFittingsFittingName.Tag.ToString
            Dim selShip As Ship = ShipLists.ShipList(shipName)
            Dim showInfo As New FrmShowInfo
            Dim hPilot As EveHQPilot
            If ActiveFitting IsNot Nothing Then
                hPilot = HQ.Settings.Pilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString)
            Else
                If HQ.Settings.StartupPilot <> "" Then
                    hPilot = HQ.Settings.Pilots(HQ.Settings.StartupPilot)
                Else
                    hPilot = HQ.Settings.Pilots.Values(0)
                End If
            End If
            showInfo.ShowItemDetails(selShip, hPilot)
        End Sub
        Private Sub ShowFitting(ByVal fittingNode As Node)

            ' Get the ship details
            If fittingNode.Parent IsNot Nothing Then
                Dim fitKey As String = fittingNode.Parent.Text & ", " & fittingNode.Text
                ShowFitting(fitKey)
            End If

        End Sub
        Private Sub ShowFitting(fitKey As String)

            ' Check we have some valid characters
            If HQ.Settings.Pilots.Count > 0 Then

                If OpenFittingsContains(fitKey) = False Then
                    If Fittings.FittingList.ContainsKey(fitKey) = True Then
                        Dim newfit As Fitting = Fittings.FittingList(fitKey)
                        Call CreateNewFittingTab(newfit)
                        newfit.ShipSlotCtrl.UpdateEverything()

                        ' Set the newly opened fitting
                        ' NB: Doesn't trigger the event if this is the first tab open
                        ActiveFitting = newfit
                        tabHQF.SelectedTab = tabHQF.Tabs(fitKey)
                    Else
                        MessageBox.Show("Can't load the '" & fitKey & "' fitting as it's not there!!", "Error locating fitting details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    tabHQF.SelectedTab = tabHQF.Tabs(fitKey)
                End If

                Dim colCount As Integer = 0
                For Each userCol As UserSlotColumn In PluginSettings.HQFSettings.UserSlotColumns
                    If userCol.Active = True Then
                        colCount += 1
                    End If
                Next
                If colCount = 0 Then
                    Dim msg As String = "HQF has detected you may be using the old default column settings which will limit the amount of information displayed by HQF." & ControlChars.CrLf
                    msg &= "HQF can display much more module data if the columns are configured prior to displaying a fitting." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Would you like to configure the displayed columns now?"
                    Dim reply As Integer = MessageBox.Show(msg, "Configure Slot Columns?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If reply = DialogResult.No Then
                        MessageBox.Show("You can always configure the columns later by going into the HQF settings and choosing the Slot Layout section", "For Future Reference...", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        ' Open options form
                        Using mySettings As New FrmHQFSettings
                            mySettings.Tag = "nodeSlotFormat"
                            mySettings.ShowDialog()
                        End Using
                        Call UpdateFittingsTree(False)
                    End If
                End If
            Else
                Dim msg As String = "There are no pilots or accounts created in EveHQ." & ControlChars.CrLf
                msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
                MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End Sub
        Private Sub RemoteShowFitting(ByVal fitKey As String)
            ShowFitting(fitKey)
        End Sub
        Private Sub mnuFittingsBCBrowser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFittingsBCBrowser.Click
            Dim shipName As String = mnuFittingsFittingName.Tag.ToString
            Dim bShip As Ship = ShipLists.ShipList(shipName).Clone
            If _myBcBrowser.IsHandleCreated = True Then
                _myBcBrowser.ShipType = bShip
                _myBcBrowser.BringToFront()
            Else
                _myBcBrowser = New FrmBcBrowser
                _myBcBrowser.ShipType = bShip
                _myBcBrowser.Show()
            End If
        End Sub
        Private Sub mnuExportToEve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExportToEve.Click
            Call ExportFittingsToEve()
        End Sub
        Private Sub mnuExportToRequisitions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExportToRequisitions.Click
            Call ExportRequisitions()
        End Sub
        Private Sub tvwfittings_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles tvwFittings.MouseDoubleClick
            If tvwFittings.SelectedNodes.Count > 0 Then
                If tvwFittings.SelectedNodes(0).Nodes.Count = 0 Then
                    Call ShowFitting(tvwFittings.SelectedNodes(0))
                End If
            End If
        End Sub

        Private Sub tvwFittings_SelecttionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tvwFittings.SelectionChanged
            If tvwFittings.SelectedNodes.Count > 1 Then
                mnuCompareFittings.Enabled = True
            Else
                If tvwFittings.SelectedNodes.Count = 1 Then
                    If tvwFittings.SelectedNodes(0).Nodes.Count > 1 Then
                        mnuCompareFittings.Enabled = True
                    Else
                        mnuCompareFittings.Enabled = False
                    End If
                Else
                    mnuCompareFittings.Enabled = False
                End If
            End If
        End Sub

        Private Sub mnuCompareFittings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuCompareFittings.Click
            Call CompareShips()
        End Sub

        Private Sub CompareShips()
            ' Establish which fittings we will be comparing
            Dim fittings As New SortedList
            For Each fitting As Node In tvwFittings.SelectedNodes
                If fitting.Nodes.Count = 0 Then
                    ' If we have highlighted an item
                    If fittings.Contains(fitting.Parent.Text & ", " & fitting.Text) = False Then
                        fittings.Add(fitting.Parent.Text & ", " & fitting.Text, "")
                    End If
                Else
                    ' If we have highlighted a group
                    For Each subFit As Node In fitting.Nodes
                        If fittings.Contains(subFit.Parent.Text & ", " & subFit.Text) = False Then
                            fittings.Add(subFit.Parent.Text & ", " & subFit.Text, "")
                        End If
                    Next
                End If
            Next
            Using compareShips As New FrmShipComparison
                compareShips.ShipList = fittings
                compareShips.ShowDialog()
            End Using
        End Sub
#End Region

#Region "Module List Context Menu Routines"

        Private Sub ctxModuleList_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxModuleList.Opening
            If tvwModules.SelectedNodes.Count > 0 Then
                Dim moduleID As Integer = CInt(tvwModules.SelectedNodes(0).Name)
                Dim cModule As ShipModule = ModuleLists.ModuleList.Item(moduleID)
                If PluginSettings.HQFSettings.Favourites.Contains(cModule.Name) = True Then
                    mnuAddToFavourites_List.Visible = False
                    mnuRemoveFromFavourites.Visible = True
                Else
                    mnuAddToFavourites_List.Visible = True
                    mnuRemoveFromFavourites.Visible = False
                End If
                If IsNumeric(ModuleDisplay) = True Then
                    mnuSep2.Visible = False
                    mnuShowModuleMarketGroup.Visible = False
                Else
                    mnuSep2.Visible = True
                    mnuShowModuleMarketGroup.Visible = True
                End If
            Else
                e.Cancel = True
            End If
        End Sub

        Private Sub mnuShowModuleInfo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuShowModuleInfo.Click
            Dim moduleID As Integer = CInt(tvwModules.SelectedNodes(0).Name)
            Dim cModule As ShipModule = ModuleLists.ModuleList.Item(moduleID)
            Dim showInfo As New FrmShowInfo
            Dim hPilot As EveHQPilot
            If ActiveFitting IsNot Nothing Then
                If ActiveFitting.ShipInfoCtrl IsNot Nothing Then
                    hPilot = HQ.Settings.Pilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString)
                Else
                    If HQ.Settings.StartupPilot <> "" Then
                        hPilot = HQ.Settings.Pilots(HQ.Settings.StartupPilot)
                    Else
                        hPilot = HQ.Settings.Pilots.Values(0)
                    End If
                End If
            Else
                If HQ.Settings.StartupPilot <> "" Then
                    hPilot = HQ.Settings.Pilots(HQ.Settings.StartupPilot)
                Else
                    hPilot = HQ.Settings.Pilots.Values(0)
                End If
            End If
            showInfo.ShowItemDetails(cModule, hPilot)
        End Sub

        Private Sub mnuAddToFavourites_List_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToFavourites_List.Click
            Dim moduleID As Integer = CInt(tvwModules.SelectedNodes(0).Name)
            Dim cModule As ShipModule = ModuleLists.ModuleList.Item(moduleID)
            If PluginSettings.HQFSettings.Favourites.Contains(cModule.Name) = False Then
                PluginSettings.HQFSettings.Favourites.Add(cModule.Name)
            End If
        End Sub

        Private Sub mnuRemoveFromFavourites_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRemoveFromFavourites.Click
            Dim moduleID As Integer = CInt(tvwModules.SelectedNodes(0).Name)
            Dim cModule As ShipModule = ModuleLists.ModuleList.Item(moduleID)
            If PluginSettings.HQFSettings.Favourites.Contains(cModule.Name) = True Then
                PluginSettings.HQFSettings.Favourites.Remove(cModule.Name)
            End If
            Call CalculateFilteredModules(tvwItems.SelectedNode)
        End Sub

        Private Sub mnuShowModuleMarketGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuShowModuleMarketGroup.Click
            Dim moduleID As Integer = CInt(tvwModules.SelectedNodes(0).Name)
            Dim cModule As ShipModule = ModuleLists.ModuleList.Item(moduleID)
            Dim pathLine As String = Market.MarketGroupPath(CStr(cModule.MarketGroup))
            If pathLine IsNot Nothing Then
                HQFEvents.DisplayedMarketGroup = pathLine
            Else
                MessageBox.Show("Unable to display Market Group due to absence of Market Group information in HQF.", "Unable to Show Market Group", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

#End Region

#Region "Menu & Button Routines"

        Private Sub OpenSettingsForm()
            ' Open options form
            Using mySettings As New FrmHQFSettings
                mySettings.ShowDialog()
            End Using
            Call UpdateFittingsTree(False)
        End Sub

#End Region

#Region "Export to Eve Routines"

        Private Sub ExportFittingsToEve()
            Dim fittings As ArrayList = GetExportFittingsCollection()
            Using myEveExport As New FrmEveExport
                myEveExport.FittingList = fittings
                myEveExport.UpdateRequired = True
                myEveExport.ShowDialog()
            End Using
        End Sub

        Private Sub ExportMainFittingToEve()
            Dim fittings As New ArrayList
            fittings.Add(ActiveFitting.KeyName)
            Using myEveExport As New FrmEveExport
                myEveExport.FittingList = fittings
                myEveExport.UpdateRequired = True
                myEveExport.ShowDialog()
            End Using
        End Sub

        Private Function GetExportFittingsCollection() As ArrayList
            Dim fittings As New ArrayList
            For Each fitting As Node In tvwFittings.SelectedNodes
                If fitting.Nodes.Count = 0 Then
                    ' If we have highlighted an item
                    If fittings.Contains(fitting.Parent.Text & ", " & fitting.Text) = False Then
                        fittings.Add(fitting.Parent.Text & ", " & fitting.Text)
                    End If
                Else
                    ' If we have highlighted a group
                    For Each subFit As Node In fitting.Nodes
                        If fittings.Contains(subFit.Parent.Text & ", " & subFit.Text) = False Then
                            fittings.Add(subFit.Parent.Text & ", " & subFit.Text)
                        End If
                    Next
                End If
            Next
            fittings.Sort()
            Return fittings
        End Function

#End Region

#Region "Export To Requisitions Routines"

        Private Sub ExportRequisitions()
            ' Establish which fittings we will be comparing
            Dim shipFits As New SortedList
            For Each fitting As Node In tvwFittings.SelectedNodes
                If fitting.Nodes.Count = 0 Then
                    ' If we have highlighted an item
                    If shipFits.Contains(fitting.Parent.Text & ", " & fitting.Text) = False Then
                        shipFits.Add(fitting.Parent.Text & ", " & fitting.Text, "")
                    End If
                Else
                    ' If we have highlighted a group
                    For Each subFit As Node In fitting.Nodes
                        If shipFits.Contains(subFit.Parent.Text & ", " & subFit.Text) = False Then
                            shipFits.Add(subFit.Parent.Text & ", " & subFit.Text, "")
                        End If
                    Next
                End If
            Next

            If shipFits.Count > 0 Then

                ' Set up a new Sortedlist to store the required items
                Dim orders As New SortedList(Of String, Integer)

                For Each shipFit As String In shipFits.Keys
                    Dim currentFit As Fitting = Fittings.FittingList.Item(shipFit).Clone
                    currentFit.UpdateBaseShipFromFitting()

                    ' Collect the orders
                    CollectModulesForExport(orders, currentFit)
                    ' Add the current ship
                    If orders.ContainsKey(currentFit.BaseShip.Name) = False Then
                        orders.Add(currentFit.BaseShip.Name, 1)
                    Else
                        orders(currentFit.BaseShip.Name) += 1
                    End If

                Next

                Using newReq As New FrmAddRequisition("HQF", orders)
                    newReq.ShowDialog()
                End Using

            End If
        End Sub

#End Region

#Region "Meta Variations Code"

        Private Sub mnuShowMetaVariations_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuShowMetaVariations.Click
            Dim moduleID As Integer = CInt(tvwModules.SelectedNodes(0).Name)
            Dim cModule As ShipModule = ModuleLists.ModuleList.Item(moduleID)
            Using newComparison As New FrmMetaVariations(ActiveFitting, cModule)
                newComparison.Size = PluginSettings.HQFSettings.MetaVariationsFormSize
                newComparison.ShowDialog()
            End Using
        End Sub

#End Region

        Private Sub tabHQF_TabRemoved(ByVal sender As Object, ByVal e As EventArgs) Handles tabHQF.TabRemoved

            ' Get name of closed tab - DNB refuses to expose this information to us
            Dim closedTabName As String = ""
            Dim tp As TabItem
            For Each fitting As String In ShipLists.FittedShipList.Keys
                tp = tabHQF.Tabs(fitting)
                If tp Is Nothing Then
                    closedTabName = fitting
                    Exit For
                End If
            Next
            If closedTabName <> "" Then
                ' Remove data
                ShipLists.FittedShipList.Remove(closedTabName)
                ActiveFitting.ShipInfoCtrl = Nothing
                ActiveFitting.ShipSlotCtrl = Nothing
            End If
            ' Check for last fitting closure to remove the Active Fitting
            If tabHQF.Tabs.Count = 0 Then
                ActiveFitting = Nothing
            End If
        End Sub

#Region "HQF Ribbon UI Functions"

        Private Sub btnOptions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOptions.Click
            Call OpenSettingsForm()
        End Sub

        Private Sub btnPilotManager_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPilotManager.Click
            Call OpenPilotManagerForm(0)
        End Sub

        Private Sub btnImplantManager_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImplantManager.Click
            Call OpenPilotManagerForm(2)
        End Sub

        Private Sub OpenPilotManagerForm(ByVal tabIdx As Integer)
            If _myPilotManager.IsHandleCreated = False Then
                _myPilotManager = New FrmPilotManager
                If ActiveFitting IsNot Nothing Then
                    _myPilotManager.PilotName = ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString
                Else
                    If HQ.Settings.StartupPilot <> "" Then
                        _myPilotManager.PilotName = HQ.Settings.Pilots(HQ.Settings.StartupPilot).Name
                    Else
                        _myPilotManager.PilotName = HQ.Settings.Pilots.Values(0).Name
                    End If
                End If
                _myPilotManager.Show()
                _myPilotManager.tabControlPM.SelectedTabIndex = tabIdx
            Else
                If _myPilotManager.WindowState = FormWindowState.Minimized Then
                    _myPilotManager.WindowState = FormWindowState.Normal
                End If
                _myPilotManager.Show()
                _myPilotManager.BringToFront()
            End If
        End Sub

        Private Sub btnScreenGrab_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnScreenGrab.Click
            ' Determine co-ords of current main panel
            Try
                Dim xy As Point = tabHQF.PointToScreen(New Point(0, 0))
                Dim sx As Integer = xy.X
                Dim sy As Integer = xy.Y
                Dim fittingImage As Bitmap = ScreenGrab.GrabScreen(New Rectangle(sx, sy, tabHQF.Width, tabHQF.Height))
                Clipboard.SetDataObject(fittingImage)
                Const RgPattern As String = "[\\\/:\*\?""'<>|]"
                Dim objRegEx As New Regex(RgPattern)
                Dim fittingName As String = objRegEx.Replace(ActiveFitting.KeyName, "_")
                Dim filename As String = "HQF_" & fittingName & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".png"
                fittingImage.Save(Path.Combine(HQ.ReportFolder, filename), ImageFormat.Png)
            Catch ex As Exception
                MessageBox.Show("There was an error taking a screenshot of the current fitting. The error was: " & ControlChars.CrLf & ControlChars.CrLf & ex.Message, "Error Taking Screenshot", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End Sub

        Private Sub btnExportEve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportEve.Click
            If ActiveFitting Is Nothing Then
                Const Msg As String = "Please make sure you have a fit open and active before exporting to Eve."
                MessageBox.Show(Msg, "Open Fitting Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Call ExportMainFittingToEve()
            End If
        End Sub

        Private Sub btnExportHQF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportHQF.Click
            Dim currentShip As Ship = ActiveFitting.BaseShip
            Dim state As Integer
            Dim fitting As New StringBuilder
            fitting.AppendLine("[" & ActiveFitting.KeyName & "]")
            For slot As Integer = 1 To currentShip.SubSlots
                If currentShip.SubSlot(slot) IsNot Nothing Then
                    state = CInt(Math.Log(currentShip.SubSlot(slot).ModuleState) / Math.Log(2))
                    If currentShip.SubSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentShip.SubSlot(slot).Name & "_" & state & ", " & currentShip.SubSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentShip.SubSlot(slot).Name & "_" & state)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For slot As Integer = 1 To currentShip.HiSlots
                If currentShip.HiSlot(slot) IsNot Nothing Then
                    state = CInt(Math.Log(currentShip.HiSlot(slot).ModuleState) / Math.Log(2))
                    If currentShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentShip.HiSlot(slot).Name & "_" & state & ", " & currentShip.HiSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentShip.HiSlot(slot).Name & "_" & state)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For slot As Integer = 1 To currentShip.MidSlots
                If currentShip.MidSlot(slot) IsNot Nothing Then
                    state = CInt(Math.Log(currentShip.MidSlot(slot).ModuleState) / Math.Log(2))
                    If currentShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentShip.MidSlot(slot).Name & "_" & state & ", " & currentShip.MidSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentShip.MidSlot(slot).Name & "_" & state)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For slot As Integer = 1 To currentShip.LowSlots
                If currentShip.LowSlot(slot) IsNot Nothing Then
                    state = CInt(Math.Log(currentShip.LowSlot(slot).ModuleState) / Math.Log(2))
                    If currentShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentShip.LowSlot(slot).Name & "_" & state & ", " & currentShip.LowSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentShip.LowSlot(slot).Name & "_" & state)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For slot As Integer = 1 To currentShip.RigSlots
                If currentShip.RigSlot(slot) IsNot Nothing Then
                    state = CInt(Math.Log(currentShip.RigSlot(slot).ModuleState) / Math.Log(2))
                    If currentShip.RigSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentShip.RigSlot(slot).Name & "_" & state & ", " & currentShip.RigSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentShip.RigSlot(slot).Name & "_" & state)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For Each drone As DroneBayItem In currentShip.DroneBayItems.Values
                If drone.IsActive = True Then
                    fitting.AppendLine(drone.DroneType.Name & ", " & drone.Quantity & "a")
                Else
                    fitting.AppendLine(drone.DroneType.Name & ", " & drone.Quantity & "i")
                End If
            Next
            fitting.AppendLine("")
            For Each cargo As CargoBayItem In currentShip.CargoBayItems.Values
                fitting.AppendLine(cargo.ItemType.Name & ", " & cargo.Quantity)
            Next
            Try
                Clipboard.SetText(fitting.ToString)
            Catch ex As Exception
                MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy For HQF Error")
            End Try
        End Sub

        Private Sub btnExportEFT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportEFT.Click
            ExportEFT()
        End Sub

        Private Sub btnExportFitting_Click(sender As Object, e As EventArgs) Handles btnExportFitting.Click
            ExportEFT()
        End Sub
        
        Private Sub ExportEFT()
            Dim currentship As Ship = ActiveFitting.FittedShip
            Dim fitting As New StringBuilder
            fitting.AppendLine("[" & ActiveFitting.KeyName & "]")
            For slot As Integer = 1 To currentship.SubSlots
                If currentship.SubSlot(slot) IsNot Nothing Then
                    If currentship.SubSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentship.SubSlot(slot).Name & ", " & currentship.SubSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentship.SubSlot(slot).Name)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For slot As Integer = 1 To currentship.LowSlots
                If currentship.LowSlot(slot) IsNot Nothing Then
                    If currentship.LowSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentship.LowSlot(slot).Name & ", " & currentship.LowSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentship.LowSlot(slot).Name)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For slot As Integer = 1 To currentship.MidSlots
                If currentship.MidSlot(slot) IsNot Nothing Then
                    If currentship.MidSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentship.MidSlot(slot).Name & ", " & currentship.MidSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentship.MidSlot(slot).Name)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For slot As Integer = 1 To currentship.HiSlots
                If currentship.HiSlot(slot) IsNot Nothing Then
                    If currentship.HiSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentship.HiSlot(slot).Name & ", " & currentship.HiSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentship.HiSlot(slot).Name)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For slot As Integer = 1 To currentship.RigSlots
                If currentship.RigSlot(slot) IsNot Nothing Then
                    If currentship.RigSlot(slot).LoadedCharge IsNot Nothing Then
                        fitting.AppendLine(currentship.RigSlot(slot).Name & ", " & currentship.RigSlot(slot).LoadedCharge.Name)
                    Else
                        fitting.AppendLine(currentship.RigSlot(slot).Name)
                    End If
                End If
            Next
            fitting.AppendLine("")
            For Each drone As DroneBayItem In currentship.DroneBayItems.Values
                fitting.AppendLine(drone.DroneType.Name & " x" & drone.Quantity)
            Next
            Try
                Clipboard.SetText(fitting.ToString)
            Catch ex As Exception
                MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy For EFT Error")
            End Try
        End Sub

        Private Sub btnExportForums_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportForums.Click
            Dim currentship As Ship = ActiveFitting.FittedShip
            Dim slots As Dictionary(Of String, Integer)
            Dim slotList As New ArrayList
            Dim slotCount As Integer
            Dim fitting As New StringBuilder
            fitting.AppendLine("[" & ActiveFitting.KeyName & "]")

            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentship.SubSlots
                If currentship.SubSlot(slot) IsNot Nothing Then
                    If currentship.SubSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentship.SubSlot(slot).Name & " (" & currentship.SubSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentship.SubSlot(slot).Name & " (" & currentship.SubSlot(slot).LoadedCharge.Name & ")")
                            slots(currentship.SubSlot(slot).Name & " (" & currentship.SubSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentship.SubSlot(slot).Name & " (" & currentship.SubSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentship.SubSlot(slot).Name & " (" & currentship.SubSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentship.SubSlot(slot).Name) = True Then
                            slotCount = slots(currentship.SubSlot(slot).Name)
                            slots(currentship.SubSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentship.SubSlot(slot).Name)
                            slots.Add(currentship.SubSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If

            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentship.HiSlots
                If currentship.HiSlot(slot) IsNot Nothing Then
                    If currentship.HiSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")")
                            slots(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentship.HiSlot(slot).Name) = True Then
                            slotCount = slots(currentship.HiSlot(slot).Name)
                            slots(currentship.HiSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentship.HiSlot(slot).Name)
                            slots.Add(currentship.HiSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                fitting.AppendLine("")
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If

            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentship.MidSlots
                If currentship.MidSlot(slot) IsNot Nothing Then
                    If currentship.MidSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")")
                            slots(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentship.MidSlot(slot).Name) = True Then
                            slotCount = slots(currentship.MidSlot(slot).Name)
                            slots(currentship.MidSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentship.MidSlot(slot).Name)
                            slots.Add(currentship.MidSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                fitting.AppendLine("")
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If

            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentship.LowSlots
                If currentship.LowSlot(slot) IsNot Nothing Then
                    If currentship.LowSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")")
                            slots(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentship.LowSlot(slot).Name) = True Then
                            slotCount = slots(currentship.LowSlot(slot).Name)
                            slots(currentship.LowSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentship.LowSlot(slot).Name)
                            slots.Add(currentship.LowSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                fitting.AppendLine("")
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If


            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentship.RigSlots
                If currentship.RigSlot(slot) IsNot Nothing Then
                    If currentship.RigSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")")
                            slots(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentship.RigSlot(slot).Name) = True Then
                            slotCount = slots(currentship.RigSlot(slot).Name)
                            slots(currentship.RigSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentship.RigSlot(slot).Name)
                            slots.Add(currentship.RigSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                fitting.AppendLine("")
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If

            If currentship.DroneBayItems.Count > 0 Then
                fitting.AppendLine("")
                For Each drone As DroneBayItem In currentship.DroneBayItems.Values
                    fitting.AppendLine(drone.Quantity & "x " & drone.DroneType.Name)
                Next
            End If

            If currentship.CargoBayItems.Count > 0 Then
                fitting.AppendLine("")
                For Each cargo As CargoBayItem In currentship.CargoBayItems.Values
                    fitting.AppendLine(cargo.Quantity & "x " & cargo.ItemType.Name)
                Next
            End If
            Try
                Clipboard.SetText(fitting.ToString)
            Catch ex As Exception
                MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy For Forums Error")
            End Try
        End Sub

        Private Sub btnExportStats_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportStats.Click
            Dim currentship As Ship = ActiveFitting.FittedShip
            Dim stats As New StringBuilder
            stats.AppendLine("[Statistics - " & ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString & "]")
            stats.AppendLine("")
            stats.AppendLine(ActiveFitting.ShipInfoCtrl.lblEffectiveHP.Text)
            stats.AppendLine(ActiveFitting.ShipInfoCtrl.lblTankAbility.Text)
            stats.AppendLine("Damage Profile - " & currentship.DamageProfile.Name & " (EM: " & (currentship.DamageProfileEM * 100).ToString("N2") & "%, Ex: " & (currentship.DamageProfileEx * 100).ToString("N2") & "%, Ki: " & (currentship.DamageProfileKi * 100).ToString("N2") & "%, Th: " & (currentship.DamageProfileTh * 100).ToString("N2") & "%)")
            stats.AppendLine("Shield Resists - EM: " & ActiveFitting.ShipInfoCtrl.lblShieldEM.Text & ", Ex: " & ActiveFitting.ShipInfoCtrl.lblShieldExplosive.Text & ", Ki: " & ActiveFitting.ShipInfoCtrl.lblShieldKinetic.Text & ", Th: " & ActiveFitting.ShipInfoCtrl.lblShieldThermal.Text)
            stats.AppendLine("Armor Resists - EM: " & ActiveFitting.ShipInfoCtrl.lblArmorEM.Text & ", Ex: " & ActiveFitting.ShipInfoCtrl.lblArmorExplosive.Text & ", Ki: " & ActiveFitting.ShipInfoCtrl.lblArmorKinetic.Text & ", Th: " & ActiveFitting.ShipInfoCtrl.lblArmorThermal.Text)
            stats.AppendLine("")
            stats.AppendLine(ActiveFitting.ShipInfoCtrl.epCapacitor.TitleText)
            stats.AppendLine("")
            stats.AppendLine("Volley Damage: " & currentship.TotalVolley.ToString("N2"))
            stats.AppendLine("DPS: " & currentship.TotalDPS.ToString("N2"))
            Try
                Clipboard.SetText(stats.ToString)
            Catch ex As Exception
                MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy Stats Error")
            End Try
        End Sub

        Private Sub btnExportImplants_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportImplants.Click
            Dim implantSetName As String = ActiveFitting.ShipInfoCtrl.cboImplants.SelectedItem.ToString
            If PluginSettings.HQFSettings.ImplantGroups.ContainsKey(implantSetName) = True Then
                Dim implantSet As ImplantCollection = PluginSettings.HQFSettings.ImplantGroups(implantSetName)
                Dim stats As New StringBuilder
                stats.AppendLine("[Implants - " & implantSet.GroupName & "]")
                stats.AppendLine("")
                For slotNo As Integer = 1 To 10
                    If implantSet.ImplantName(slotNo) <> "" Then
                        stats.AppendLine("Slot " & slotNo.ToString & ": " & implantSet.ImplantName(slotNo))
                    Else
                        stats.AppendLine("Slot " & slotNo.ToString & ": <Empty>")
                    End If
                Next
                Try
                    Clipboard.SetText(stats.ToString)
                Catch ex As Exception
                    MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy Stats Error")
                End Try
            End If
        End Sub

        Private Sub btnExportReq_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportReq.Click
            ' Set up a new Sortedlist to store the required items
            Dim orders As New SortedList(Of String, Integer)
            CollectModulesForExport(orders, ActiveFitting)
            ' Add the current ship
            orders.Add(ActiveFitting.BaseShip.Name, 1)
            ' Setup the Requisition form for HQF and open it
            Using newReq As New FrmAddRequisition("HQF", orders)
                newReq.ShowDialog()
            End Using
        End Sub

        Private Sub CollectModulesForExport(ByRef modList As SortedList(Of String, Integer), ByVal shipFitting As Fitting)

            Dim currentship As Ship = shipFitting.BaseShip

            ' Parse HiSlots
            For slot As Integer = 1 To currentship.HiSlots
                If currentship.HiSlot(slot) IsNot Nothing Then
                    If modList.ContainsKey(currentship.HiSlot(slot).Name) = True Then
                        modList(currentship.HiSlot(slot).Name) += 1
                    Else
                        modList.Add(currentship.HiSlot(slot).Name, 1)
                    End If
                End If
            Next

            ' Parse MidSlots
            For slot As Integer = 1 To currentship.MidSlots
                If currentship.MidSlot(slot) IsNot Nothing Then
                    If modList.ContainsKey(currentship.MidSlot(slot).Name) = True Then
                        modList(currentship.MidSlot(slot).Name) += 1
                    Else
                        modList.Add(currentship.MidSlot(slot).Name, 1)
                    End If
                End If
            Next

            ' Parse LowSlots
            For slot As Integer = 1 To currentship.LowSlots
                If currentship.LowSlot(slot) IsNot Nothing Then
                    If modList.ContainsKey(currentship.LowSlot(slot).Name) = True Then
                        modList(currentship.LowSlot(slot).Name) += 1
                    Else
                        modList.Add(currentship.LowSlot(slot).Name, 1)
                    End If
                End If
            Next

            ' Parse RigSlots
            For slot As Integer = 1 To currentship.RigSlots
                If currentship.RigSlot(slot) IsNot Nothing Then
                    If modList.ContainsKey(currentship.RigSlot(slot).Name) = True Then
                        modList(currentship.RigSlot(slot).Name) += 1
                    Else
                        modList.Add(currentship.RigSlot(slot).Name, 1)
                    End If
                End If
            Next

            ' Parse subslots
            For slot As Integer = 1 To currentship.SubSlots
                If currentship.SubSlot(slot) IsNot Nothing Then
                    If modList.ContainsKey(currentship.SubSlot(slot).Name) = True Then
                        modList(currentship.SubSlot(slot).Name) += 1
                    Else
                        modList.Add(currentship.SubSlot(slot).Name, 1)
                    End If
                End If
            Next

            ' Parse drones
            If currentship.DroneBayItems.Count > 0 Then
                For Each drone As DroneBayItem In currentship.DroneBayItems.Values
                    If modList.ContainsKey(drone.DroneType.Name) = True Then
                        modList(drone.DroneType.Name) += drone.Quantity
                    Else
                        modList.Add(drone.DroneType.Name, drone.Quantity)
                    End If
                Next
            End If

            ' Parse cargo bay
            If currentship.CargoBayItems.Count > 0 Then
                For Each cargoitem As CargoBayItem In currentship.CargoBayItems.Values
                    If modList.ContainsKey(cargoitem.ItemType.Name) = True Then
                        modList(cargoitem.ItemType.Name) += cargoitem.Quantity
                    Else
                        modList.Add(cargoitem.ItemType.Name, cargoitem.Quantity)
                    End If
                Next
            End If

        End Sub

        Private Sub btnImportEve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImportEve.Click
            If HQ.Settings.Pilots.Count = 0 Then
                Dim msg As String = "There are no pilots or accounts created in EveHQ." & ControlChars.CrLf
                msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
                MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If _myEveImport.IsHandleCreated = True Then
                    _myEveImport.BringToFront()
                Else
                    _myEveImport = New FrmEveImport
                    _myEveImport.Show()
                End If
            End If
        End Sub

        Private Sub btnImportEFT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImportEFT.Click
            Using myEftImport As New FrmEftImport
                myEftImport.ShowDialog()
            End Using
            Call UpdateFittingsTree(False)
        End Sub

        Private Sub btnImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImport.Click
            ' Pick the text up from the clipboard
            Dim fileText As String = CStr(Clipboard.GetDataObject().GetData(DataFormats.Text))
            fileText = fileText.Replace(Chr(150), "-")

            ' Use Regex to get the data - No checking as this is done in the tmrClipboard_Tick sub
            Dim fittingMatch As Match = Regex.Match(fileText, "\[(?<ShipName>[^,]*)\]|\[(?<ShipName>.*),\s?(?<FittingName>.*)\]")
            Dim shipName As String = fittingMatch.Groups.Item("ShipName").Value
            Dim fittingName As String
            If fittingMatch.Groups.Item("FittingName").Value <> "" Then
                fittingName = fittingMatch.Groups.Item("FittingName").Value
            Else
                fittingName = "Imported Fit"
            End If
            ' If the fitting exists, add a number onto the end
            If Fittings.FittingList.ContainsKey(shipName & ", " & fittingName) = True Then
                Dim response As Integer = MessageBox.Show("Fitting name already exists. Are you sure you wish to import the fitting?", "Confirm Import for " & shipName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If response = DialogResult.Yes Then
                    Dim newFittingName As String
                    Dim revision As Integer = 1
                    Do
                        revision += 1
                        newFittingName = fittingName & " " & revision.ToString
                    Loop Until Fittings.FittingList.ContainsKey(shipName & ", " & newFittingName) = False
                    fittingName = newFittingName
                    MessageBox.Show("New fitting name is '" & fittingName & "'.", "New Fitting Imported", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Exit Sub
                End If
            End If
            ' Lets create the fitting
            Dim mods() As String = fileText.Split(ControlChars.CrLf.ToCharArray)
            Dim newFit As New ArrayList
            For Each shipMod As String In mods
                shipMod = shipMod.Trim
                If shipMod.StartsWith("[") = False And shipMod <> "" Then
                    ' Check for "Drones_" label
                    If shipMod.StartsWith("Drones_") Then
                        shipMod = shipMod.TrimStart("Drones_Active=".ToCharArray)
                        shipMod = shipMod.TrimStart("Drones_Inactive=".ToCharArray)
                    End If
                    ' Check for forum format (to shut the fucking whiners up)
                    Dim moduleMatch As Match = Regex.Match(shipMod, "(?<quantity>\d)*x\s(?<module>.+)\s\((?<charge>.+)\)|(?<quantity>\d)*x\s(?<module>.+)")
                    If moduleMatch.Success = True Then
                        Dim modName As String = moduleMatch.Groups.Item("module").Value
                        ' Check for module replacement
                        If PlugInData.ModuleChanges.ContainsKey(modName) Then
                            modName = PlugInData.ModuleChanges(modName)
                        End If
                        Dim chargeName As String = moduleMatch.Groups.Item("charge").Value
                        Dim quantity As String = moduleMatch.Groups.Item("quantity").Value
                        If IsNumeric(quantity) = True Then
                            If ModuleLists.ModuleListName.ContainsKey(modName) = True Then
                                Dim testMod As ShipModule = ModuleLists.ModuleList(ModuleLists.ModuleListName(modName)).Clone
                                If testMod.IsDrone = True Then
                                    newFit.Add(modName & ", " & quantity)
                                Else
                                    If CInt(quantity) > 1 Then
                                        modName = modName & " x" & quantity
                                    End If
                                    If chargeName <> "" Then
                                        newFit.Add(modName & ", " & chargeName)
                                    Else
                                        newFit.Add(modName)
                                    End If
                                End If
                            End If
                        End If
                    Else
                        ' Check for module replacement
                        If PlugInData.ModuleChanges.ContainsKey(shipMod) Then
                            shipMod = PlugInData.ModuleChanges(shipMod)
                        End If
                        Dim droneMatch As Match = Regex.Match(shipMod, "(?<module>.+)\sx(?<quantity>\d+)|(?<module>.+)")
                        If droneMatch.Success = True Then
                            Dim modName As String = droneMatch.Groups.Item("module").Value
                            Dim quantity As String = droneMatch.Groups.Item("quantity").Value
                            If IsNumeric(quantity) = True Then
                                If ModuleLists.ModuleListName.ContainsKey(modName) = True Then
                                    Dim testMod As ShipModule = ModuleLists.ModuleList(ModuleLists.ModuleListName(modName)).Clone
                                    If testMod.IsDrone = True Then
                                        newFit.Add(modName & ", " & quantity)
                                    Else
                                        If CInt(quantity) > 1 Then
                                            modName = modName & " x" & quantity
                                        End If
                                        newFit.Add(modName)

                                    End If
                                End If
                            Else
                                newFit.Add(shipMod)
                            End If
                        Else
                            newFit.Add(shipMod)
                        End If
                    End If
                End If
            Next
            Dim newFitting As Fitting = Fittings.ConvertOldFitToNewFit(shipName & ", " & fittingName, newFit)
            Fittings.FittingList.Add(newFitting.KeyName, newFitting)
            Call UpdateFittingsTree(False)
        End Sub

        Private Sub btnEditor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditor.Click
            Using newShipEditor As New FrmShipEditor
                newShipEditor.ShowDialog()
            End Using
        End Sub

        Private Function OpenFittingsContains(ByVal fitKey As String) As Boolean
            For Each fitTab As TabItem In tabHQF.Tabs
                If fitTab.Text = fitKey Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Sub btnShipSelector_Click(sender As Object, e As EventArgs) Handles btnShipSelector.Click
            Using form As New FrmShipSelection
                form.ShowDialog()
            End Using
        End Sub

#End Region

#Region "Undo & Redo Code"

        Private Sub frmHQF_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown

            If e.KeyCode = Keys.Z And e.Control = True Then
                If ActiveFitting IsNot Nothing Then
                    Call ActiveFitting.ShipSlotCtrl.PerformUndo(1)
                End If
            End If

            If e.KeyCode = Keys.Y And e.Control = True Then
                If ActiveFitting IsNot Nothing Then
                    Call ActiveFitting.ShipSlotCtrl.PerformRedo(1)
                End If
            End If

        End Sub

#End Region

    End Class
End NameSpace