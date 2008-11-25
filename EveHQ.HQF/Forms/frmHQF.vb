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
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Xml
Imports DotNetLib.Windows.Forms

Public Class frmHQF

    Dim dataCheckList As New SortedList
    Dim currentShipSlot As ShipSlotControl
    Dim currentShipInfo As ShipInfoControl
    Shared LastSlotFitting As New ArrayList
    Shared LastModuleResults As New SortedList

#Region "Class Wide Variables"

    Dim itemCount As Integer = 0
    Dim startUp As Boolean = False

    Shared cModuleDisplay As String = ""
    Public Shared Property ModuleDisplay() As String
        Get
            Return cModuleDisplay
        End Get
        Set(ByVal value As String)
            cModuleDisplay = value
        End Set
    End Property

#End Region

#Region "Form Initialisation & Closing Routines"

    Private Sub frmHQF_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Save the panel widths
        Settings.HQFSettings.ShipPanelWidth = SplitContainerShip.Width
        Settings.HQFSettings.ModPanelWidth = SplitContainerMod.Width
        Settings.HQFSettings.ShipSplitterWidth = SplitContainerShip.SplitterDistance
        Settings.HQFSettings.ModSplitterWidth = SplitContainerMod.SplitterDistance
        ' Save fittings
        Call Me.SaveFittings()
        ' Save pilots
        Call HQFPilotCollection.SaveHQFPilotData()
        ' Save the Settings
        Call Settings.HQFSettings.SaveHQFSettings()
        ' Destroy the tab settings
        Me.tabHQF.Dispose()
        ' Destroy the panels
        Me.SplitContainerShip.Dispose()
        Me.SplitContainerMod.Dispose()
        Me.lvwItems.Dispose()
        Me.tvwItems.Dispose()
        LastModuleResults.Clear()
        LastSlotFitting.Clear()
        ModuleDisplay = ""
    End Sub
    Private Sub frmHQF_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        startUp = True

        ' Close the EveHQ InfoPanel if opted to
        If Settings.HQFSettings.CloseInfoPanel = True Then
            EveHQ.Core.HQ.StartCloseInfoPanel = True
            Me.WindowState = FormWindowState.Maximized
        End If
        ModuleDisplay = ""
        LastModuleResults.Clear()

        ' Clear tabs and fitted ship lists, results list
        ShipLists.fittedShipList.Clear()
        Fittings.FittingTabList.Clear()
        LastModuleResults.Clear()
        tabHQF.TabPages.Clear()
        Me.Show()
        Me.Refresh()

        RemoveHandler ShipModule.ShowModuleMarketGroup, AddressOf Me.UpdateMarketGroup
        RemoveHandler HQFEvents.FindModule, AddressOf Me.UpdateModulesThatWillFit
        RemoveHandler HQFEvents.UpdateFitting, AddressOf Me.UpdateFittings
        RemoveHandler HQFEvents.UpdateModuleList, AddressOf Me.UpdateModuleList

        AddHandler ShipModule.ShowModuleMarketGroup, AddressOf Me.UpdateMarketGroup
        AddHandler HQFEvents.FindModule, AddressOf Me.UpdateModulesThatWillFit
        AddHandler HQFEvents.UpdateFitting, AddressOf Me.UpdateFittings
        AddHandler HQFEvents.UpdateModuleList, AddressOf Me.UpdateModuleList

        ' Load the settings!
        Call Settings.HQFSettings.LoadHQFSettings()

        ' Load the Profiles - stored separately from settings for distibution!
        Call Settings.HQFSettings.LoadProfiles()

        ' Load up a collection of pilots from the EveHQ Core
        Call Me.LoadPilots()

        ' Load saved setups into the fitting array
        Call Me.LoadFittings()

        ' Set the MetaType Filter
        Call Me.SetMetaTypeFilters()

        ' Show the groups
        Call Me.ShowShipGroups()
        Call Me.ShowMarketGroups()

        startUp = False
        ' Temporarily disable the performance setting
        Dim performanceSetting As Boolean = HQF.Settings.HQFSettings.ShowPerformanceData
        HQF.Settings.HQFSettings.ShowPerformanceData = False

        ' Check if we need to restore tabs from the previous setup
        If HQF.Settings.HQFSettings.RestoreLastSession = True Then
            For Each shipFit As String In HQF.Settings.HQFSettings.OpenFittingList
                If Fittings.FittingList.ContainsKey(shipFit) = True Then
                    ' Create the tab and display
                    If Fittings.FittingTabList.Contains(shipFit) = False Then
                        Call Me.CreateFittingTabPage(shipFit)
                    End If
                    tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
                    Call UpdateSelectedTab()
                    currentShipSlot.UpdateEverything()
                End If
            Next
        End If

        ' Set the panel widths
        SplitContainerShip.Width = Settings.HQFSettings.ShipPanelWidth
        SplitContainerMod.Width = Settings.HQFSettings.ModPanelWidth
        SplitContainerShip.SplitterDistance = Settings.HQFSettings.ShipSplitterWidth
        SplitContainerMod.SplitterDistance = Settings.HQFSettings.ModSplitterWidth

        HQF.Settings.HQFSettings.ShowPerformanceData = performanceSetting

    End Sub
    Private Sub LoadFittings()
        Fittings.FittingList.Clear()
        If My.Computer.FileSystem.FileExists(HQF.Settings.HQFFolder & "\HQFFittings.bin") = True Then
            Dim s As New FileStream(HQF.Settings.HQFFolder & "\HQFFittings.bin", FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            Fittings.FittingList = CType(f.Deserialize(s), SortedList)
            s.Close()
        End If
        Call Me.UpdateFittingsTree()
    End Sub
    Private Sub SaveFittings()
        ' Save ships
        Dim s As New FileStream(HQF.Settings.HQFFolder & "\HQFFittings.bin", FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, Fittings.FittingList)
        s.Flush()
        s.Close()
    End Sub
   
    Private Sub ShowShipGroups()
        Dim sr As New StreamReader(HQF.Settings.HQFCacheFolder & "\ShipGroups.bin")
        Dim ShipGroups As String = sr.ReadToEnd
        Dim PathLines() As String = ShipGroups.Split(ControlChars.CrLf.ToCharArray)
        Dim nodes() As String
        Dim cNode As New TreeNode
        sr.Close()
        tvwShips.BeginUpdate()
        tvwShips.Nodes.Clear()
        For Each pathline As String In PathLines
            If pathline <> "" Then
                If pathline.Contains("\") = False Then
                    tvwShips.Nodes.Add(pathline, pathline)
                Else
                    nodes = pathline.Split("\".ToCharArray)
                    cNode = tvwShips.Nodes(nodes(0))
                    For node As Integer = 1 To nodes.GetUpperBound(0) - 1
                        cNode = cNode.Nodes(nodes(node))
                    Next
                    cNode.Nodes.Add(nodes(nodes.GetUpperBound(0)), nodes(nodes.GetUpperBound(0)))
                End If
            End If
        Next
        tvwShips.Sorted = True
        tvwShips.EndUpdate()
    End Sub
    Private Sub ShowMarketGroups()
        Dim sr As New StreamReader(HQF.Settings.HQFCacheFolder & "\ItemGroups.bin")
        Dim ShipGroups As String = sr.ReadToEnd
        Dim PathLines() As String = ShipGroups.Split(ControlChars.CrLf.ToCharArray)
        Dim nodes() As String
        Dim nodeData() As String
        Dim cNode As New TreeNode
        Dim newNode As New TreeNode
        sr.Close()
        tvwItems.BeginUpdate()
        tvwItems.Nodes.Clear()
        Market.MarketGroupList.Clear()
        Market.MarketGroupPath.Clear()
        For Each pathline As String In PathLines
            If pathline <> "" Then
                If pathline.Contains("\") = False Then
                    nodeData = pathline.Split(",".ToCharArray)
                    tvwItems.Nodes.Add(nodeData(1), nodeData(1))
                Else
                    nodeData = pathline.Split(",".ToCharArray)
                    nodes = nodeData(1).Split("\".ToCharArray)
                    cNode = tvwItems.Nodes(nodes(0))
                    For node As Integer = 1 To nodes.GetUpperBound(0) - 1
                        cNode = cNode.Nodes(nodes(node))
                    Next
                    newNode = New TreeNode()
                    newNode.Name = nodes(nodes.GetUpperBound(0))
                    newNode.Text = nodes(nodes.GetUpperBound(0))
                    newNode.Tag = nodeData(0)
                    cNode.Nodes.Add(newNode)
                    If newNode.Tag.ToString <> "0" Then
                        Market.MarketGroupList.Add(newNode.Tag.ToString, newNode.Name)
                        Market.MarketGroupPath.Add(newNode.Tag.ToString, nodeData(1))
                    End If
                End If
            End If
        Next
        tvwItems.EndUpdate()
    End Sub
    Private Sub UpdateMarketGroup(ByVal path As String)
        Dim nodes() As String = path.Split("\".ToCharArray)
        Dim cNode As New TreeNode
        cNode = tvwItems.Nodes(nodes(0))
        For node As Integer = 1 To nodes.GetUpperBound(0)
            cNode = cNode.Nodes(nodes(node))
        Next
        tvwItems.SelectedNode = cNode
        tvwItems.Select()
    End Sub
    Private Sub SplitContainerShip_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerShip.Resize
        SplitContainerShip.SplitterDistance = Settings.HQFSettings.ShipSplitterWidth
    End Sub
    Private Sub SplitContainerMod_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerMod.Resize
        SplitContainerMod.SplitterDistance = Settings.HQFSettings.ModSplitterWidth
    End Sub
    Private Sub LoadPilots()
        ' Loads the skills for the selected pilots
        ' Check for a valid HQFPilotSettings.xml file
        If My.Computer.FileSystem.FileExists(HQF.Settings.HQFFolder & "\HQFPilotSettings.bin") = True Then
            Call HQFPilotCollection.LoadHQFPilotData()
            ' Check we have all the available pilots!
            Dim morePilots As Boolean = False
            For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
                If HQFPilotCollection.HQFPilots.Contains(pilot.Name) = False Then
                    ' We don't have it, so lets create one!
                    Dim newHQFPilot As New HQFPilot
                    newHQFPilot.PilotName = pilot.Name
                    newHQFPilot.SkillSet = New Collection
                    HQFPilotCollection.ResetSkillsToDefault(newHQFPilot)
                    For imp As Integer = 0 To 10
                        newHQFPilot.ImplantName(imp) = ""
                    Next
                    HQFPilotCollection.HQFPilots.Add(newHQFPilot.PilotName, newHQFPilot)
                    morePilots = True
                End If
            Next
            ' Check for missing skills in the pilots info
            For Each hPilot As HQFPilot In HQFPilotCollection.HQFPilots.Values
                Call HQFPilotCollection.CheckForMissingSkills(hPilot)
            Next
            ' Check if we need to update the HQFPilot skills to actuals
            If HQF.Settings.HQFSettings.AutoUpdateHQFSkills = True Then
                morePilots = True
                For Each hPilot As HQFPilot In HQFPilotCollection.HQFPilots.Values
                    Call HQFPilotCollection.UpdateHQFSkillsToActual(hPilot)
                Next
            End If
            ' Save the data if we need to
            If morePilots = True Then
                Call HQFPilotCollection.SaveHQFPilotData()
            End If
        Else
            HQFPilotCollection.HQFPilots.Clear()
            For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
                Dim newHQFPilot As New HQFPilot
                newHQFPilot.PilotName = pilot.Name
                newHQFPilot.SkillSet = New Collection
                HQFPilotCollection.ResetSkillsToDefault(newHQFPilot)
                For imp As Integer = 0 To 10
                    newHQFPilot.ImplantName(imp) = ""
                Next
                HQFPilotCollection.HQFPilots.Add(newHQFPilot.PilotName, newHQFPilot)
            Next
            ' Save the data
            Call HQFPilotCollection.SaveHQFPilotData()
        End If

        If HQFPilotCollection.HQFPilots.Count > 0 Then
            btnPilotManager.Enabled = True
        End If

    End Sub
    Private Sub SetMetaTypeFilters()
        Dim filters() As Integer = {1, 2, 4, 8, 16, 32}
        For Each filter As Integer In filters
            Dim chkBox As CheckBox = CType(Me.SplitContainerMod.Panel1.Controls.Item("chkFilter" & filter.ToString), CheckBox)
            If (HQF.Settings.HQFSettings.ModuleFilter And filter) = filter Then
                chkBox.Checked = True
            Else
                chkBox.Checked = False
            End If
        Next
    End Sub
#End Region

#Region "Ship Browser Routines"
    Private Sub tvwShips_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwShips.NodeMouseClick
        tvwShips.SelectedNode = e.Node
    End Sub
    Private Sub tvwShips_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwShips.NodeMouseDoubleClick
        tvwShips.SelectedNode = e.Node
        Dim curNode As TreeNode = tvwShips.SelectedNode
        If curNode IsNot Nothing Then
            If curNode.Nodes.Count = 0 Then ' If has no child nodes, therefore a ship not group
                Dim shipName As String = curNode.Text
                mnuShipBrowserShipName.Text = shipName
                Call Me.CreateNewFitting(shipName)
            End If
        End If
    End Sub
    Private Sub ctxShipBrowser_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxShipBrowser.Opening
        Dim curNode As TreeNode = tvwShips.SelectedNode
        If curNode IsNot Nothing Then
            If curNode.Nodes.Count = 0 Then ' If has no child nodes, therefore a ship not group
                Dim shipName As String = curNode.Text
                mnuShipBrowserShipName.Text = shipName
            Else
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuCreateNewFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCreateNewFitting.Click
        ' Get the ship details
        Dim shipName As String = mnuShipBrowserShipName.Text
        Call Me.CreateNewFitting(shipName)
    End Sub
    Private Sub mnuPreviewShip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPreviewShip.Click
        Dim shipName As String = mnuShipBrowserShipName.Text
        Dim selShip As Ship = CType(ShipLists.shipList(shipName), Ship)
        Call DisplayShipPreview(selShip)
    End Sub
    Private Sub DisplayShipPreview(ByVal selShip As Ship)
        pbShip.ImageLocation = "http://www.eve-online.com/bitmaps/icons/itemdb/shiptypes/128_128/" & selShip.ID & ".png"
        lblShipType.Text = selShip.Name
        txtShipDescription.Text = selShip.Description
        lblShieldHP.Text = FormatNumber(selShip.ShieldCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP"
        lblShieldEM.Text = FormatNumber(selShip.ShieldEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblShieldEx.Text = FormatNumber(selShip.ShieldExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblShieldKi.Text = FormatNumber(selShip.ShieldKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblShieldTh.Text = FormatNumber(selShip.ShieldThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblShieldRecharge.Text = "Recharge Rate: " & FormatNumber(selShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        lblArmorHP.Text = FormatNumber(selShip.ArmorCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP"
        lblArmorEM.Text = FormatNumber(selShip.ArmorEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblArmorEx.Text = FormatNumber(selShip.ArmorExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblArmorKi.Text = FormatNumber(selShip.ArmorKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblArmorTh.Text = FormatNumber(selShip.ArmorThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblStructureHP.Text = FormatNumber(selShip.StructureCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP"
        lblStructureEM.Text = FormatNumber(selShip.StructureEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblStructureEx.Text = FormatNumber(selShip.StructureExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblStructureKi.Text = FormatNumber(selShip.StructureKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblStructureTh.Text = FormatNumber(selShip.StructureThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblCapacitor.Text = "Capacitor: " & FormatNumber(selShip.CapCapacity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCapRecharge.Text = "Recharge Rate: " & FormatNumber(selShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        lblSpeed.Text = "Max Velocity: " & FormatNumber(selShip.MaxVelocity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m/s"
        lblInertia.Text = "Inertia: " & FormatNumber(selShip.Inertia, 6, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCargohold.Text = "Cargo: " & FormatNumber(selShip.CargoBay, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"
        lblDroneBay.Text = "Drone Bay: " & FormatNumber(selShip.DroneBay, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"

        lblCPU.Text = "CPU: " & FormatNumber(selShip.CPU, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblPG.Text = "Powergrid: " & FormatNumber(selShip.PG, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCalibration.Text = "Calibration: " & FormatNumber(selShip.Calibration, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        lblLowSlots.Text = "Low Slots: " & FormatNumber(selShip.LowSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblMedSlots.Text = "Med Slots: " & FormatNumber(selShip.MidSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblHiSlots.Text = "High Slots: " & FormatNumber(selShip.HiSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblRigSlots.Text = "Rig Slots: " & FormatNumber(selShip.RigSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblTurretSlots.Text = "Turret Slots: " & FormatNumber(selShip.TurretSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblLauncherSlots.Text = "Launcher Slots: " & FormatNumber(selShip.LauncherSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        lblWarpSpeed.Text = "Warp Speed: " & FormatNumber(selShip.WarpSpeed, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " au/s"

        ' Add Required Skill into the Description
        txtShipDescription.Text &= ControlChars.CrLf & ControlChars.CrLf & "Required Skills" & ControlChars.CrLf
        For Each cSkill As ItemSkills In selShip.RequiredSkills.Values
            txtShipDescription.Text &= cSkill.Name & " (Lvl " & EveHQ.Core.SkillFunctions.Roman(cSkill.Level) & ")" & ControlChars.CrLf
        Next

        ' Add the tab if it's not available
        If tabHQF.TabPages.Contains(tabShipPreview) = False Then
            tabHQF.TabPages.Add(tabShipPreview)
        End If
        ' Bring the Preview tab to the front
        tabHQF.SelectedTab = tabShipPreview

    End Sub
    Private Sub CreateNewFitting(ByVal shipName As String)
        ' Check we have some valid characters
        If EveHQ.Core.HQ.Pilots.Count > 0 Then
            ' Clear the text boxes
            Dim myNewFitting As New frmModifyFittingName
            Dim fittingName As String = ""
            With myNewFitting
                .txtFittingName.Text = "" : .txtFittingName.Enabled = True
                .btnAccept.Text = "Add" : .Tag = "Add"
                .btnAccept.Tag = shipName
                .Text = "Create New Fitting for " & shipName
                .ShowDialog()
                fittingName = .txtFittingName.Text
            End With
            myNewFitting = Nothing

            If fittingName <> "" Then
                Dim fittingKeyName As String = shipName & ", " & fittingName
                Fittings.FittingList.Add(fittingKeyName, New ArrayList)
                Call Me.CreateFittingTabPage(fittingKeyName)
                Call Me.UpdateFilteredShips()
                tabHQF.SelectedTab = tabHQF.TabPages(fittingKeyName)
                If tabHQF.TabPages.Count = 1 Then
                    Call Me.UpdateSelectedTab()   ' Called when tabpage count=0 as SelectedIndexChanged does not fire!
                End If
                currentShipSlot.UpdateEverything()
            Else
                MessageBox.Show("Unable to Create New Fitting!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            Dim msg As String = "There are no pilots or accounts created in EveHQ." & ControlChars.CrLf
            msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
            MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub txtShipSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtShipSearch.TextChanged
        Call Me.UpdateFilteredShips()
    End Sub
    Private Sub UpdateFilteredShips()
        If Len(txtShipSearch.Text) > 0 Then
            Dim strSearch As String = txtShipSearch.Text.Trim.ToLower

            ' Redraw the ships tree
            Dim shipResults As New SortedList(Of String, String)
            For Each sShip As String In ShipLists.shipList.Keys
                If sShip.ToLower.Contains(strSearch) Then
                    shipResults.Add(sShip, sShip)
                End If
            Next
            tvwShips.BeginUpdate()
            tvwShips.Nodes.Clear()
            For Each item As String In shipResults.Values
                Dim shipNode As TreeNode = New TreeNode
                shipNode.Text = item
                shipNode.Name = item
                tvwShips.Nodes.Add(shipNode)
            Next
            tvwShips.EndUpdate()

            ' Redraw the fitting tree
            Dim fitResults As New SortedList(Of String, String)
            For Each sFit As String In Fittings.FittingList.Keys
                If sFit.ToLower.Contains(strSearch) Then
                    fitResults.Add(sFit, sFit)
                End If
            Next
            clvFittings.BeginUpdate()
            clvFittings.SelectedItems.Clear()
            clvFittings.Items.Clear()
            Dim shipName As String = ""
            Dim fittingName As String = ""
            Dim fittingSep As Integer = 0
            For Each item As String In fitResults.Values
                fittingSep = item.IndexOf(", ")
                shipName = item.Substring(0, fittingSep)
                fittingName = item.Substring(fittingSep + 2)
                ' Create the ship node if it's not already present
                Dim containsShip As New ContainerListViewItem
                For Each ship As ContainerListViewItem In clvFittings.Items
                    If ship.Text = shipName Then
                        containsShip = ship
                    End If
                Next
                If containsShip.Text = "" Then
                    containsShip.Text = shipName
                    clvFittings.Items.Add(containsShip)
                End If
                ' Add the details to the Node, checking for duplicates
                containsShip.Items.Add(New ContainerListViewItem(fittingName))
            Next
            clvFittings.EndUpdate()
            Call Me.UpdateFittingsCombo()
        Else
            txtShipSearch.Text = ""
            Call Me.ShowShipGroups()
            Call Me.UpdateFittingsTree()
        End If
    End Sub
    Private Sub btnResetShips_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetShips.Click
        txtShipSearch.Text = ""
        Call Me.ShowShipGroups()
        Call Me.UpdateFittingsTree()
    End Sub
#End Region

#Region "Module Display, Filter and Search Options"
    Private Sub tvwItems_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwItems.NodeMouseClick
        tvwItems.SelectedNode = e.Node
    End Sub
    Private Sub tvwItems_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwItems.AfterSelect
        If e.Node.Nodes.Count = 0 Then
            Call Me.CalculateFilteredModules(e.Node)
        End If
    End Sub
    Private Sub MetaFilterChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilter1.CheckedChanged, chkFilter2.CheckedChanged, chkFilter4.CheckedChanged, chkFilter8.CheckedChanged, chkFilter16.CheckedChanged, chkFilter32.CheckedChanged
        If startUp = False Then
            Dim chkBox As CheckBox = CType(sender, CheckBox)
            Dim changedFilter As Integer = CInt(chkBox.Tag)
            HQF.Settings.HQFSettings.ModuleFilter = HQF.Settings.HQFSettings.ModuleFilter Xor changedFilter
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
    Private Sub chkApplySkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplySkills.CheckedChanged
        Call Me.UpdateModuleList()
    End Sub
    Private Sub chkOnlyShowUsable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOnlyShowUsable.CheckedChanged
        Call Me.UpdateModuleList()
    End Sub
    Private Sub chkOnlyShowFittable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOnlyShowFittable.CheckedChanged
        Call Me.UpdateModuleList()
    End Sub
    Private Sub UpdateModuleList()
        If startUp = False Then
            If ModuleDisplay <> "" Then
                Select Case ModuleDisplay
                    Case "Search"
                        Call CalculateSearchedModules()
                    Case "Fitted"
                        If LastSlotFitting.Count > 0 Then
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
    Private Sub CalculateFilteredModules(ByVal groupNode As TreeNode)
        Me.Cursor = Cursors.WaitCursor
        Dim cMod, sMod As New ShipModule
        Dim groupID As String
        Dim results As New SortedList
        If groupNode.Name = "Favourites" Then
            ModuleDisplay = "Favourites"
            For Each modName As String In Settings.HQFSettings.Favourites
                cMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(modName)), ShipModule)
                ' Add results in by name, module
                If chkApplySkills.Checked = True Then
                    sMod = Engine.ApplySkillEffectsToModule(cMod)
                Else
                    sMod = cMod.Clone
                End If
                If currentShipInfo IsNot Nothing Then
                    If chkOnlyShowUsable.Checked = True Then
                        If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Else
                        If chkOnlyShowFittable.Checked = True Then
                            If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                results.Add(sMod.Name, sMod)
                            End If
                        Else
                            results.Add(sMod.Name, sMod)
                        End If
                    End If
                Else
                    results.Add(sMod.Name, sMod)
                End If
            Next
            lblModuleDisplayType.Tag = "Displaying: Favourites"
        ElseIf groupNode.Name = "Recently Used" Then
            ModuleDisplay = "Recently Used"
            For Each modName As String In Settings.HQFSettings.MRUModules
                cMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(modName)), ShipModule)
                ' Add results in by name, module
                If chkApplySkills.Checked = True Then
                    sMod = Engine.ApplySkillEffectsToModule(cMod)
                Else
                    sMod = cMod.Clone
                End If
                If chkOnlyShowUsable.Checked = True Then
                    If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                        If chkOnlyShowFittable.Checked = True Then
                            If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                results.Add(sMod.Name, sMod)
                            End If
                        Else
                            results.Add(sMod.Name, sMod)
                        End If
                    End If
                Else
                    If chkOnlyShowFittable.Checked = True Then
                        If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                            results.Add(sMod.Name, sMod)
                        End If
                    Else
                        results.Add(sMod.Name, sMod)
                    End If
                End If
            Next
            lblModuleDisplayType.Tag = "Displaying: Recently Used"
        Else
            If groupNode.Nodes.Count = 0 Then
                groupID = groupNode.Tag.ToString
            Else
                groupID = ModuleDisplay
            End If
            ModuleDisplay = groupID
            lblModuleDisplayType.Tag = Market.MarketGroupList(groupID).ToString
            For Each shipMod As ShipModule In HQF.ModuleLists.moduleList.Values
                If shipMod.MarketGroup = groupID Then
                    ' Add results in by name, module
                    If chkApplySkills.Checked = True Then
                        sMod = Engine.ApplySkillEffectsToModule(shipMod)
                    Else
                        sMod = shipMod.Clone
                    End If
                    If chkOnlyShowUsable.Checked = True And currentShipInfo IsNot Nothing Then
                        If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Else
                        If chkOnlyShowFittable.Checked = True Then
                            If currentShipSlot IsNot Nothing Then
                                If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        Else
                            results.Add(sMod.Name, sMod)
                        End If
                    End If
                End If
            Next
            lblModuleDisplayType.Tag = "Displaying: " & lblModuleDisplayType.Tag.ToString
        End If
        LastModuleResults = results
        Call Me.ShowFilteredModules()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ShowFilteredModules()

        Dim groupID As String = ""
        ' Reset checkbox colours
        chkFilter1.ForeColor = Color.Red
        chkFilter2.ForeColor = Color.Red
        chkFilter4.ForeColor = Color.Red
        chkFilter8.ForeColor = Color.Red
        chkFilter16.ForeColor = Color.Red
        chkFilter32.ForeColor = Color.Red

        lvwItems.BeginUpdate()
        lvwItems.Items.Clear()
        For Each shipMod As ShipModule In LastModuleResults.Values
            If (shipMod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipMod.MetaType Then
                Dim newModule As New ListViewItem
                newModule.Name = shipMod.ID
                groupID = shipMod.MarketGroup
                newModule.Text = shipMod.Name
                newModule.ToolTipText = shipMod.Name
                newModule.SubItems.Add(shipMod.MetaLevel.ToString)
                newModule.SubItems.Add(shipMod.CPU.ToString)
                newModule.SubItems.Add(shipMod.PG.ToString)
                Select Case shipMod.SlotType
                    Case 8 ' High
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                        'newModule.ImageKey = "hiSlot"
                    Case 4 ' Mid
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                        'newModule.ImageKey = "midSlot"
                    Case 2 ' Low
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                        'newModule.ImageKey = "lowSlot"
                    Case 1 ' Rig
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                        'newModule.ImageKey = "rigSlot"
                End Select
                Dim chkFilter As CheckBox = CType(Me.SplitContainerMod.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                If chkFilter IsNot Nothing Then
                    chkFilter.ForeColor = Color.Black
                End If
                lvwItems.Items.Add(newModule)
            Else
                Dim chkFilter As CheckBox = CType(Me.SplitContainerMod.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                If chkFilter IsNot Nothing Then
                    chkFilter.ForeColor = Color.LimeGreen
                End If
            End If
        Next
        If lvwItems.Items.Count = 0 Then
            lvwItems.Items.Add("<Empty - Please check filters>")
            lvwItems.Enabled = False
            lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (0 items)"
        Else
            lvwItems.Enabled = True
            lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (" & lvwItems.Items.Count & " items)"
        End If
        lvwItems.EndUpdate()

    End Sub
    Private Sub CalculateSearchedModules()
        Me.Cursor = Cursors.WaitCursor
        Dim cMod, sMod As New ShipModule
        If Len(txtSearchModules.Text) > 2 Then
            Dim strSearch As String = txtSearchModules.Text.Trim.ToLower
            Dim results As New SortedList
            For Each item As String In HQF.ModuleLists.moduleListName.Keys
                If item.ToLower.Contains(strSearch) Then
                    ' Add results in by name, module
                    cMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(item)), ShipModule)
                    If chkApplySkills.Checked = True Then
                        sMod = Engine.ApplySkillEffectsToModule(cMod)
                    Else
                        sMod = cMod.Clone
                    End If
                    If chkOnlyShowUsable.Checked = True And currentShipInfo IsNot Nothing Then
                        If currentShipInfo.cboPilots.SelectedItem IsNot Nothing Then
                            If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                If chkOnlyShowFittable.Checked = True Then
                                    If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                        results.Add(sMod.Name, sMod)
                                    End If
                                Else
                                    results.Add(sMod.Name, sMod)
                                End If
                            End If
                        Else
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Else
                        results.Add(sMod.Name, sMod)
                    End If
                End If
            Next
            LastModuleResults = results
            lblModuleDisplayType.Tag = "Displaying: Matching *" & txtSearchModules.Text & "*"
            Call Me.ShowSearchedModules()
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ShowSearchedModules()

        ' Reset checkbox colours
        chkFilter1.ForeColor = Color.Red
        chkFilter2.ForeColor = Color.Red
        chkFilter4.ForeColor = Color.Red
        chkFilter8.ForeColor = Color.Red
        chkFilter16.ForeColor = Color.Red
        chkFilter32.ForeColor = Color.Red

        lvwItems.BeginUpdate()
        lvwItems.Items.Clear()
        For Each shipMod As ShipModule In LastModuleResults.Values
            If ModuleLists.moduleList.Contains(shipMod.ID) = True And Implants.implantList.ContainsKey(shipMod.ID) = False Then
                If (shipMod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipMod.MetaType Then
                    Dim newModule As New ListViewItem
                    newModule.Name = shipMod.ID
                    newModule.Text = shipMod.Name
                    newModule.ToolTipText = shipMod.Name
                    newModule.SubItems.Add(shipMod.MetaLevel.ToString)
                    newModule.SubItems.Add(shipMod.CPU.ToString)
                    newModule.SubItems.Add(shipMod.PG.ToString)
                    Select Case shipMod.SlotType
                        Case 8 ' High
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                            'newModule.ImageKey = "hiSlot"
                        Case 4 ' Mid
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                            'newModule.ImageKey = "midSlot"
                        Case 2 ' Low
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                            'newModule.ImageKey = "lowSlot"
                        Case 1 ' Rig
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                            'newModule.ImageKey = "rigSlot"
                    End Select
                    Dim chkFilter As CheckBox = CType(Me.SplitContainerMod.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                    chkFilter.ForeColor = Color.Black
                    lvwItems.Items.Add(newModule)
                Else
                    Dim chkFilter As CheckBox = CType(Me.SplitContainerMod.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                    chkFilter.ForeColor = Color.LimeGreen
                End If
            End If
        Next
        lvwItems.EndUpdate()
        ModuleDisplay = "Search"
        lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (" & lvwItems.Items.Count & " items)"
    End Sub
    Private Sub UpdateModulesThatWillFit(ByVal modData As ArrayList)
        LastSlotFitting = modData
        If LastSlotFitting.Count > 0 Then
            Call CalculateModulesThatWillFit()
        End If
    End Sub
    Private Sub CalculateModulesThatWillFit()
        Me.Cursor = Cursors.WaitCursor
        Dim slotType As Integer = CInt(LastSlotFitting(0))
        Dim CPU As Double = CDbl(LastSlotFitting(1))
        Dim PG As Double = CDbl(LastSlotFitting(2))
        Dim Calibration As Double = CDbl(LastSlotFitting(3))
        Dim LauncherSlots As Integer = CInt(LastSlotFitting(4))
        Dim TurretSlots As Integer = CInt(LastSlotFitting(5))
        Dim strSearch As String = txtSearchModules.Text.Trim.ToLower
        Dim results As New SortedList
        Dim sMod As New ShipModule
        For Each cMod As ShipModule In HQF.ModuleLists.moduleList.Values
            If chkApplySkills.Checked = True Then
                sMod = Engine.ApplySkillEffectsToModule(cMod)
            Else
                sMod = cMod.Clone
            End If
            If sMod.SlotType = slotType Then
                Select Case slotType
                    Case 1 ' Rig Slot
                        If sMod.Calibration <= Calibration Then
                            If chkOnlyShowUsable.Checked = True Then
                                If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Case 2, 4 ' Low & Mid Slot
                        If sMod.CPU <= CPU Then
                            If sMod.PG <= PG Then
                                If chkOnlyShowUsable.Checked = True Then
                                    If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                        results.Add(sMod.Name, sMod)
                                    End If
                                Else
                                    results.Add(sMod.Name, sMod)
                                End If
                            End If
                        End If
                    Case 8 ' Hi Slot
                        If sMod.CPU <= CPU Then
                            If sMod.PG <= PG Then
                                If LauncherSlots >= Math.Abs(CInt(sMod.IsLauncher)) Then
                                    If TurretSlots >= Math.Abs(CInt(sMod.IsTurret)) Then
                                        If chkOnlyShowUsable.Checked = True Then
                                            If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                                results.Add(sMod.Name, sMod)
                                            End If
                                        Else
                                            results.Add(sMod.Name, sMod)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                End Select
            End If
        Next
        LastModuleResults = results
        lblModuleDisplayType.Tag = "Displaying: Modules That Fit"
        Call Me.ShowModulesThatWillFit()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ShowModulesThatWillFit()

        ' Reset checkbox colours
        chkFilter1.ForeColor = Color.Red
        chkFilter2.ForeColor = Color.Red
        chkFilter4.ForeColor = Color.Red
        chkFilter8.ForeColor = Color.Red
        chkFilter16.ForeColor = Color.Red
        chkFilter32.ForeColor = Color.Red

        lvwItems.BeginUpdate()
        lvwItems.Items.Clear()
        For Each shipMod As ShipModule In LastModuleResults.Values
            If (shipMod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipMod.MetaType Then
                Dim newModule As New ListViewItem
                newModule.Name = shipMod.ID
                newModule.Text = shipMod.Name
                newModule.ToolTipText = shipMod.Name
                newModule.SubItems.Add(shipMod.MetaLevel.ToString)
                newModule.SubItems.Add(shipMod.CPU.ToString)
                newModule.SubItems.Add(shipMod.PG.ToString)
                Select Case shipMod.SlotType
                    Case 8 ' High
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                        'newModule.ImageKey = "hiSlot"
                    Case 4 ' Mid
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                        'newModule.ImageKey = "midSlot"
                    Case 2 ' Low
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                        'newModule.ImageKey = "lowSlot"
                    Case 1 ' Rig
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                        'newModule.ImageKey = "rigSlot"
                End Select
                Dim chkFilter As CheckBox = CType(Me.SplitContainerMod.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                If chkFilter IsNot Nothing Then
                    chkFilter.ForeColor = Color.Black
                End If
                lvwItems.Items.Add(newModule)
            Else
                Dim chkFilter As CheckBox = CType(Me.SplitContainerMod.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                chkFilter.ForeColor = Color.LimeGreen
            End If
        Next
        lvwItems.EndUpdate()
        ModuleDisplay = "Fitted"
        lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (" & lvwItems.Items.Count & " items)"
    End Sub
    Private Sub txtSearchModules_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchModules.GotFocus
        Call CalculateSearchedModules()
    End Sub
    Private Sub txtSearchModules_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchModules.TextChanged
        Call CalculateSearchedModules()
    End Sub

#End Region

#Region "Module List Routines"
    Private Sub lvwItems_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwItems.ColumnClick
        If CInt(lvwItems.Tag) = e.Column Then
            lvwItems.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwItems.Tag = -1
        Else
            lvwItems.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwItems.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwItems.Sort()
    End Sub
    Private Sub lvwItems_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwItems.DoubleClick
        If currentShipSlot IsNot Nothing Then
            If lvwItems.SelectedItems.Count > 0 Then
                Dim moduleID As String = lvwItems.SelectedItems(0).Name
                Dim shipMod As ShipModule = CType(ModuleLists.moduleList(moduleID), ShipModule).Clone
                If shipMod.IsDrone = True Then
                    Dim active As Boolean = False
                    Call currentShipSlot.AddDrone(shipMod, 1, False)
                Else
                    ' Check if module is a charge
                    If shipMod.IsCharge = True Then
                        currentShipSlot.AddItem(shipMod, 1)
                    Else
                        ' Must be a proper module then!
                        Call currentShipSlot.AddModule(shipMod, 0, True, Nothing)
                        ' Add it to the MRU
                        Call Me.UpdateMRUModules(shipMod.Name)
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
#End Region

    Private Sub tsbOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbOptions.Click
        Call Me.OpenSettingsForm()
    End Sub

    Public Sub UpdateFittings()
        Me.Cursor = Cursors.WaitCursor
        ' Updates all the open fittings
        For Each openTab As String In Fittings.FittingTabList
            Dim thisTab As TabPage = tabHQF.TabPages(openTab)
            If thisTab IsNot Nothing Then
                If thisTab.Controls.Count > 0 Then
                    Dim thisShipSlotControl As ShipSlotControl = CType(thisTab.Controls("panelShipSlot").Controls("shipSlot"), ShipSlotControl)
                    Dim thisShipInfoControl As ShipInfoControl = CType(thisTab.Controls("panelShipInfo").Controls("shipInfo"), ShipInfoControl)
                    thisShipSlotControl.UpdateEverything()
                End If
            End If
        Next
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        ' Count number of items
        Dim items As Integer = ModuleLists.moduleList.Count
        ' Check MarketGroups
        Dim marketError As Integer = 0
        For Each item As ShipModule In ModuleLists.moduleList.Values
            If Market.MarketGroupList.ContainsKey(item.MarketGroup) = False Then
                marketError += 1
                'MessageBox.Show(item.Name)
            End If
        Next
        ' Check MetaGroups
        Dim metaError As Integer = 0
        For Each item As ShipModule In ModuleLists.moduleList.Values
            If ModuleLists.moduleMetaGroups.ContainsKey(item.ID) = False Then
                metaError += 1
                'MessageBox.Show(item.Name)
            End If
        Next

        Dim msg As String = ""
        msg &= "Total items: " & items & ControlChars.CrLf
        msg &= "Orphaned market items: " & marketError & ControlChars.CrLf
        msg &= "Orphaned meta items: " & metaError & ControlChars.CrLf
        MessageBox.Show(msg)

        ' Traverse the tree, looking for goodies!
        itemCount = 0
        dataCheckList.Clear()
        For Each rootNode As TreeNode In tvwItems.Nodes
            SearchChildNodes(rootNode)
        Next

        ' Write missing items to a file
        Dim sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\missingItems.csv")
        For Each shipMod As ShipModule In ModuleLists.moduleList.Values
            If dataCheckList.Contains(shipMod.ID) = False Then
                sw.WriteLine(shipMod.ID & "," & shipMod.Name)
                dataCheckList.Add(shipMod.ID, shipMod.Name)
            End If
        Next
        sw.Flush()
        sw.Close()

        MessageBox.Show("Total traversed items: " & itemCount, "Tree Walk Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub SearchChildNodes(ByRef parentNode As TreeNode)

        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                SearchChildNodes(childNode)
            Else
                Dim groupID As String = childNode.Tag.ToString
                For Each shipMod As ShipModule In ModuleLists.moduleList.Values
                    If shipMod.MarketGroup = groupID Then
                        itemCount += 1
                        dataCheckList.Add(shipMod.ID, shipMod.Name)
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub SearchChildNodes1(ByRef parentNode As TreeNode, ByVal sw As IO.StreamWriter)
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                SearchChildNodes1(childNode, sw)
            Else
                sw.Write(childNode.FullPath & ControlChars.CrLf)
            End If
        Next
    End Sub

#Region "TabHQF Selection and Context Menu Routines"

    Private Sub tabHQF_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabHQF.SelectedIndexChanged
        Call Me.UpdateSelectedTab()
    End Sub
    Private Sub UpdateSelectedTab()
        If tabHQF.SelectedTab IsNot Nothing Then
            If Fittings.FittingTabList.Contains(tabHQF.SelectedTab.Text) Then
                ' Get the controls on the existing tab
                Dim thisShipSlotControl As ShipSlotControl = CType(tabHQF.SelectedTab.Controls("panelShipSlot").Controls("shipSlot"), ShipSlotControl)
                Dim thisShipInfoControl As ShipInfoControl = CType(tabHQF.SelectedTab.Controls("panelShipInfo").Controls("shipInfo"), ShipInfoControl)
                currentShipSlot = thisShipSlotControl
                currentShipInfo = thisShipInfoControl
                currentShipSlot.ShipFit = tabHQF.SelectedTab.Text
                currentShipSlot.ShipInfo = currentShipInfo
                currentShipInfo.BuildMethod = BuildType.BuildEffectsMaps
            End If
            tabHQF.Tag = tabHQF.SelectedIndex
            btnCopy.Enabled = True
            btnScreenshot.Enabled = True
        Else
            btnCopy.Enabled = False
            btnScreenshot.Enabled = False
        End If
    End Sub
    Private Function TabControlHitTest(ByVal TabCtrl As TabControl, ByVal pt As Point) As Integer
        ' Test each tabs rectangle to see if our point is contained within it.
        For x As Integer = 0 To TabCtrl.TabPages.Count - 1
            ' If tab contians our rectangle return it's index.
            If TabCtrl.GetTabRect(x).Contains(pt) Then Return x
        Next
        ' A tab was not located at specified point.
        Return -1
    End Function
    Private Sub TabHQF_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tabHQF.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim TabIndex As Integer
            ' Get index of tab clicked
            TabIndex = TabControlHitTest(tabHQF, e.Location)
            ' If a tab was clicked display it's index
            If TabIndex >= 0 Then
                tabHQF.Tag = TabIndex
                Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
                mnuCloseHQFTab.Text = "Close " & tp.Text
            Else
                mnuCloseHQFTab.Text = "Not Valid"
            End If
        End If
    End Sub

    Private Sub ctxTabHQF_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles ctxTabHQF.Closed
        mnuCloseHQFTab.Text = "Not Valid"
    End Sub
    Private Sub ctxTabHQF_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxTabHQF.Opening
        If mnuCloseHQFTab.Text = "Not Valid" Then
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuCloseMDITab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCloseHQFTab.Click
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Fittings.FittingTabList.Remove(tp.Text)
        ShipLists.fittedShipList.Remove(tp.Text)
        tabHQF.TabPages.Remove(tp)
        If Fittings.FittingTabList.Count = 0 Then
            currentShipInfo = Nothing
            currentShipSlot = Nothing
        End If
    End Sub
#End Region

#Region "Clipboard Paste Routines (incl Timer)"
    Private Sub tmrClipboard_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrClipboard.Tick
        ' Checks the clipboard for any compatible fixes!
        If Clipboard.GetDataObject IsNot Nothing Then
            Try
                Dim fileText As String = CStr(Clipboard.GetDataObject().GetData(DataFormats.Text))
                If fileText IsNot Nothing Then
                    Dim fittingMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(fileText, "\[(?<ShipName>[^,]*)\]|\[(?<ShipName>.*),\s?(?<FittingName>.*)\]")
                    If fittingMatch.Success = True Then
                        ' Appears to be a match so lets check the ship type
                        If ShipLists.shipList.Contains(fittingMatch.Groups.Item("ShipName").Value) = True Then
                            btnClipboardPaste.Enabled = True
                        Else
                            btnClipboardPaste.Enabled = False
                        End If
                    Else
                        btnClipboardPaste.Enabled = False
                    End If
                Else
                    btnClipboardPaste.Enabled = False
                End If
            Catch ex As Exception
                btnClipboardPaste.Enabled = False
            End Try
        Else
            btnClipboardPaste.Enabled = False
        End If
    End Sub
    Private Sub btnClipboardPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClipboardPaste.Click
        ' Pick the text up from the clipboard
        Dim fileText As String = CStr(Clipboard.GetDataObject().GetData(DataFormats.Text))
        ' Use Regex to get the data - No checking as this is done in the tmrClipboard_Tick sub
        Dim fittingMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(fileText, "\[(?<ShipName>[^,]*)\]|\[(?<ShipName>.*),\s?(?<FittingName>.*)\]")
        Dim shipName As String = fittingMatch.Groups.Item("ShipName").Value
        Dim fittingName As String = ""
        If fittingMatch.Groups.Item("FittingName").Value <> "" Then
            fittingName = fittingMatch.Groups.Item("FittingName").Value
        Else
            fittingName = "Imported Fit"
        End If
        ' If the fitting exists, add a number onto the end
        If Fittings.FittingList.ContainsKey(shipName & ", " & fittingName) = True Then
            Dim response As Integer = MessageBox.Show("Fitting name already exists. Are you sure you wish to import the fitting?", "Confirm Import for " & shipName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If response = Windows.Forms.DialogResult.Yes Then
                Dim newFittingName As String = ""
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
        For Each ShipMod As String In mods
            If ShipMod.StartsWith("[") = False And ShipMod <> "" Then
                newFit.Add(ShipMod)
            End If
        Next
        Fittings.FittingList.Add(shipName & ", " & fittingName, newFit)
        Call Me.UpdateFittingsTree()
    End Sub
#End Region

#Region "Fitting Panel Routines"

    Private Sub UpdateFittingsTree()
        clvFittings.BeginUpdate()
        ' Get Current List of "open" nodes
        Dim openNodes As New ArrayList
        For Each shipNode As ContainerListViewItem In clvFittings.Items
            If shipNode.Expanded = True Then
                openNodes.Add(shipNode.Text)
            End If
        Next
        ' Redraw the tree
        clvFittings.Items.Clear()
        Dim shipName As String = ""
        Dim fittingName As String = ""
        Dim fittingSep As Integer = 0
        For Each fitting As String In Fittings.FittingList.Keys
            fittingSep = fitting.IndexOf(", ")
            shipName = fitting.Substring(0, fittingSep)
            fittingName = fitting.Substring(fittingSep + 2)

            ' Create the ship node if it's not already present
            Dim containsShip As New ContainerListViewItem
            For Each ship As ContainerListViewItem In clvFittings.Items
                If ship.Text = shipName Then
                    containsShip = ship
                End If
            Next
            If containsShip.Text = "" Then
                containsShip.Text = shipName
                clvFittings.Items.Add(containsShip)
            End If

            ' Add the details to the Node, checking for duplicates
            Dim containsFitting As New ContainerListViewItem
            For Each fit As ContainerListViewItem In containsShip.Items
                If fit.Text = fittingName Then
                    MessageBox.Show("Duplicate fitting found for " & shipName & ", and omitted", "Duplicate Fitting Found!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    containsFitting = Nothing
                    Exit For
                End If
            Next
            If containsFitting IsNot Nothing Then
                containsFitting.Text = fittingName
                containsShip.Items.Add(containsFitting)
            End If
        Next
        For Each shipNode As ContainerListViewItem In clvFittings.Items
            If openNodes.Contains(shipNode.Text) Then
                shipNode.Expand()
            End If
        Next
        clvFittings.EndUpdate()
        Call Me.UpdateFittingsCombo()
    End Sub
    Private Sub UpdateFittingsCombo()
        cboFittings.BeginUpdate()
        cboFittings.Items.Clear()
        For Each fitting As String In Fittings.FittingList.Keys
            cboFittings.Items.Add(fitting)
        Next
        cboFittings.EndUpdate()
    End Sub

    Private Sub CreateFittingTabPage(ByVal shipFit As String)
        Dim fittingSep As Integer = shipFit.IndexOf(", ")
        Dim shipName As String = shipFit.Substring(0, fittingSep)
        Dim fittingName As String = shipFit.Substring(fittingSep + 2)
        Dim curShip As Ship = CType(CType(ShipLists.shipList(shipName), Ship).Clone, Ship)
        curShip.DamageProfile = CType(DamageProfiles.ProfileList.Item("<Omni-Damage>"), DamageProfile)
        ShipLists.fittedShipList.Add(shipFit, curShip)

        Dim tp As New TabPage(shipFit)
        tp.Tag = shipFit
        tp.Name = shipFit

        tabHQF.TabPages.Add(tp)
        tp.Parent = Me.tabHQF

        Dim pSS As New Panel
        pSS.BorderStyle = BorderStyle.Fixed3D
        pSS.Dock = System.Windows.Forms.DockStyle.Fill
        pSS.Location = New System.Drawing.Point(0, 0)
        pSS.Name = "panelShipSlot"
        pSS.Size = New System.Drawing.Size(414, 600)
        pSS.TabIndex = 1

        Dim pSI As New Panel
        pSI.Dock = System.Windows.Forms.DockStyle.Left
        pSI.Location = New System.Drawing.Point(0, 384)
        pSI.Name = "panelShipInfo"
        pSI.Size = New System.Drawing.Size(270, 600)
        pSI.TabIndex = 0

        tp.Controls.Add(pSS)
        tp.Controls.Add(pSI)
        tp.Location = New System.Drawing.Point(4, 22)
        tp.Size = New System.Drawing.Size(414, 666)
        tp.UseVisualStyleBackColor = True

        Dim shipSlot As New ShipSlotControl
        shipSlot.Name = "shipSlot"
        shipSlot.Location = New Point(0, 0)
        shipSlot.Dock = DockStyle.Fill
        pSS.Controls.Add(shipSlot)

        Dim shipInfo As New ShipInfoControl
        shipInfo.Name = "shipInfo"
        shipInfo.Location = New Point(0, 0)
        shipInfo.Dock = DockStyle.Fill
        pSI.Controls.Add(shipInfo)

        shipInfo.ShipSlot = shipSlot
        shipSlot.ShipInfo = shipInfo
        shipSlot.ShipFit = shipFit

        Fittings.FittingTabList.Add(shipFit)
    End Sub
    Private Sub ctxFittings_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxFittings.Opening
        If clvFittings.SelectedItems.Count < 2 Then
            Dim curNode As ContainerListViewItem = clvFittings.SelectedItems(0)
            If curNode IsNot Nothing Then
                If curNode.Items.Count = 0 Then
                    Dim parentNode As ContainerListViewItem = curNode.ParentItem
                    mnuFittingsFittingName.Text = parentNode.Text & ", " & curNode.Text
                    mnuFittingsFittingName.Tag = parentNode.Text
                    mnuFittingsCreateFitting.Text = "Create New " & parentNode.Text & " Fitting"
                    mnuFittingsCreateFitting.Enabled = True
                    mnuFittingsCopyFitting.Enabled = True
                    mnuFittingsDeleteFitting.Enabled = True
                    mnuFittingsRenameFitting.Enabled = True
                    mnuFittingsShowFitting.Enabled = True
                    mnuPreviewShip2.Enabled = True
                Else
                    mnuFittingsFittingName.Text = curNode.Text
                    mnuFittingsFittingName.Tag = curNode.Text
                    mnuFittingsCreateFitting.Text = "Create New " & curNode.Text & " Fitting"
                    mnuFittingsCreateFitting.Enabled = True
                    mnuFittingsCopyFitting.Enabled = False
                    mnuFittingsDeleteFitting.Enabled = False
                    mnuFittingsRenameFitting.Enabled = False
                    mnuFittingsShowFitting.Enabled = False
                    mnuPreviewShip2.Enabled = True
                End If
            Else
                e.Cancel = True
            End If
        Else
            mnuFittingsFittingName.Text = "[Multiple Selection]"
            mnuFittingsFittingName.Tag = "[Multiple Selection]"
            mnuFittingsCreateFitting.Text = "[Multiple Selection]"
            mnuFittingsCreateFitting.Enabled = False
            mnuFittingsCopyFitting.Enabled = False
            mnuFittingsDeleteFitting.Enabled = False
            mnuFittingsRenameFitting.Enabled = False
            mnuFittingsShowFitting.Enabled = False
            mnuPreviewShip2.Enabled = False
        End If
    End Sub
    Private Sub mnuFittingsShowFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsShowFitting.Click
        ' Get the node details
        Dim curNode As ContainerListViewItem = clvFittings.SelectedItems(0)
        Call Me.ShowFitting(curNode)
    End Sub
    Private Sub mnuFittingsRenameFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsRenameFitting.Click
        ' Get the node details
        Dim curNode As ContainerListViewItem = clvFittings.SelectedItems(0)
        Dim parentnode As ContainerListViewItem = curNode.ParentItem
        Dim shipName As String = parentnode.Text
        Dim fitName As String = curNode.Text
        Dim oldKeyName As String = shipName & ", " & fitName
        Dim FitToCopy As ArrayList = CType(Fittings.FittingList(oldKeyName), ArrayList)

        ' Clear the text boxes
        Dim myNewFitting As New frmModifyFittingName
        Dim fittingName As String = ""
        With myNewFitting
            .txtFittingName.Text = fitName : .txtFittingName.Enabled = True
            .btnAccept.Text = "Edit" : .Tag = "Edit"
            .btnAccept.Tag = shipName
            .Text = "Edit Fitting Name for " & shipName
            .ShowDialog()
            fittingName = .txtFittingName.Text
        End With
        myNewFitting = Nothing

        ' Add and Remove the Fittings
        If fittingName <> "" Then
            Dim fittingKeyName As String = shipName & ", " & fittingName
            Fittings.FittingList.Remove(oldKeyName)
            Fittings.FittingList.Add(fittingKeyName, FitToCopy.Clone)
            ' Amend it in the tabs if it's there!
            Dim tp As TabPage = tabHQF.TabPages(oldKeyName)
            If tp IsNot Nothing Then
                Fittings.FittingTabList.Remove(oldKeyName)
                Fittings.FittingTabList.Add(fittingKeyName)
                Dim copyShip As Ship = CType(ShipLists.fittedShipList(oldKeyName), Ship).Clone
                ShipLists.fittedShipList.Remove(oldKeyName)
                ShipLists.fittedShipList.Add(fittingKeyName, copyShip)
                tp.Name = fittingKeyName
                tp.Tag = fittingKeyName
                tp.Text = fittingKeyName
            End If
            Call Me.UpdateFilteredShips()
        Else
            MessageBox.Show("Unable to Copy Fitting!", "Copy Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub mnuFittingsCopyFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsCopyFitting.Click
        ' Get the node details
        Dim curNode As ContainerListViewItem = clvFittings.SelectedItems(0)
        Dim parentnode As ContainerListViewItem = curNode.ParentItem
        Dim shipName As String = parentnode.Text
        Dim fitName As String = curNode.Text
        Dim fitKeyName As String = shipName & ", " & fitName
        Dim FitToCopy As ArrayList = CType(Fittings.FittingList(fitKeyName), ArrayList)

        ' Clear the text boxes
        Dim myNewFitting As New frmModifyFittingName
        Dim fittingName As String = ""
        With myNewFitting
            .txtFittingName.Text = "" : .txtFittingName.Enabled = True
            .btnAccept.Text = "Copy" : .Tag = "Copy"
            .btnAccept.Tag = shipName
            .Text = "Copy '" & fitName & "' Fitting for " & shipName
            .ShowDialog()
            fittingName = .txtFittingName.Text
        End With
        myNewFitting = Nothing

        ' Add and Copy the Fitting
        If fittingName <> "" Then
            Dim fittingKeyName As String = shipName & ", " & fittingName
            Fittings.FittingList.Add(fittingKeyName, FitToCopy.Clone)
            Call Me.UpdateFilteredShips()
        Else
            MessageBox.Show("Unable to Copy Fitting!", "Copy Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub mnuFittingsDeleteFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsDeleteFitting.Click
        ' Get the node details
        Dim curNode As ContainerListViewItem = clvFittings.SelectedItems(0)
        Dim parentnode As ContainerListViewItem = curNode.ParentItem
        Dim shipName As String = parentnode.Text
        Dim fitName As String = curNode.Text

        ' Get confirmation of deletion
        Dim response As Integer = MessageBox.Show("Are you sure you wish to delete the '" & fitName & "' Fitting for the " & shipName & "?", "Confirm Fitting Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If response = Windows.Forms.DialogResult.Yes Then
            ' Remove the fit from the list
            Dim fittingKeyName As String = shipName & ", " & fitName
            Fittings.FittingList.Remove(fittingKeyName)
            ' Delete the file if it's there
            If My.Computer.FileSystem.FileExists(Settings.HQFFolder & "\" & fittingKeyName & ".hqf") = True Then
                My.Computer.FileSystem.DeleteFile(Settings.HQFFolder & "\" & fittingKeyName & ".hqf")
            End If
            ' Delete it from the tabs if it's there!
            Dim tp As TabPage = tabHQF.TabPages(fittingKeyName)
            If tp IsNot Nothing Then
                Fittings.FittingTabList.Remove(tp.Text)
                tabHQF.TabPages.Remove(tp)
                ShipLists.fittedShipList.Remove(tp.Text)
            End If
            ' Update the list
            Call Me.UpdateFilteredShips()
        End If
    End Sub
    Private Sub mnuFittingsCreateFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsCreateFitting.Click
        ' Get the node details
        Dim curNode As ContainerListViewItem = clvFittings.SelectedItems(0)
        Dim shipName As String = mnuFittingsFittingName.Tag.ToString

        ' Clear the text boxes
        Dim myNewFitting As New frmModifyFittingName
        Dim fittingName As String = ""
        With myNewFitting
            .txtFittingName.Text = "" : .txtFittingName.Enabled = True
            .btnAccept.Text = "Add" : .Tag = "Add"
            .btnAccept.Tag = shipName
            .Text = "Create New Fitting for " & shipName
            .ShowDialog()
            fittingName = .txtFittingName.Text
        End With
        myNewFitting = Nothing

        ' Add the Fitting
        If fittingName <> "" Then
            Dim fittingKeyName As String = shipName & ", " & fittingName
            Fittings.FittingList.Add(fittingKeyName, New ArrayList)
            Call Me.CreateFittingTabPage(fittingKeyName)
            Call Me.UpdateFilteredShips()
            tabHQF.SelectedTab = tabHQF.TabPages(fittingKeyName)
            If tabHQF.SelectedIndex = 0 Then Call Me.UpdateSelectedTab()
            currentShipSlot.UpdateEverything()
        Else
            MessageBox.Show("Unable to Create New Fitting!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub mnuPreviewShip2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPreviewShip2.Click
        Dim curNode As ContainerListViewItem = clvFittings.SelectedItems(0)
        Dim shipName As String = mnuFittingsFittingName.Tag.ToString
        Dim selShip As Ship = CType(ShipLists.shipList(shipName), Ship)
        Call DisplayShipPreview(selShip)
    End Sub
    Private Sub ShowFitting(ByVal fittingNode As ContainerListViewItem)
        ' Check we have some valid characters
        If EveHQ.Core.HQ.Pilots.Count > 0 Then
            ' Get the ship details
            If fittingNode.ParentItem IsNot Nothing Then
                Dim shipName As String = fittingNode.ParentItem.Text
                Dim shipFit As String = fittingNode.ParentItem.Text & ", " & fittingNode.Text
                ' Create the tab and display
                If Fittings.FittingTabList.Contains(shipFit) = False Then
                    Call Me.CreateFittingTabPage(shipFit)
                    tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
                    If tabHQF.SelectedIndex = 0 Then Call Me.UpdateSelectedTab()
                    currentShipSlot.UpdateEverything()
                Else
                    tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
                End If
            End If
        Else
            Dim msg As String = "There are no pilots or accounts created in EveHQ." & ControlChars.CrLf
            msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
            MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
#End Region

#Region "Fittings Combo Routines"
    Private Sub cboFittings_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFittings.SelectedIndexChanged
        Dim shipFit As String = cboFittings.SelectedItem.ToString
        ' Create the tab and display
        If Fittings.FittingTabList.Contains(shipFit) = False Then
            Call Me.CreateFittingTabPage(shipFit)
        End If
        tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
        If tabHQF.SelectedIndex = 0 Then Call Me.UpdateSelectedTab()
        currentShipSlot.UpdateEverything()
    End Sub
#End Region

    Private Sub btnShipPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShipPanel.Click
        If btnShipPanel.Checked = True Then
            ' If the panel is open
            'btnShipPanel.Image = My.Resources.panel_close
            SplitContainerShip.Visible = True
            cboFittings.Visible = False
        Else
            ' If the panel is closed
            'btnShipPanel.Image = My.Resources.panel_open
            SplitContainerShip.Visible = False
            cboFittings.Visible = True
        End If
    End Sub

    Private Sub btnItemPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemPanel.Click
        If btnItemPanel.Checked = True Then
            ' If the panel is open
            'btnShipPanel.Image = My.Resources.panel_close
            SplitContainerMod.Visible = True
        Else
            ' If the panel is closed
            'btnShipPanel.Image = My.Resources.panel_open
            SplitContainerMod.Visible = False
        End If
    End Sub



#Region "Module List Context Menu Routines"

    Private Sub ctxModuleList_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxModuleList.Opening
        If lvwItems.SelectedItems.Count > 0 Then
            Dim moduleID As String = lvwItems.SelectedItems(0).Name
            Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
            If ModuleDisplay = "Favourites" Then
                mnuAddToFavourites_List.Visible = False
                mnuRemoveFromFavourites.Visible = True
            Else
                mnuAddToFavourites_List.Visible = True
                mnuRemoveFromFavourites.Visible = False
                If Settings.HQFSettings.Favourites.Contains(cModule.Name) = True Then
                    mnuAddToFavourites_List.Enabled = False
                Else
                    mnuAddToFavourites_List.Enabled = True
                End If
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

    Private Sub mnuShowModuleInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowModuleInfo.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        If currentShipInfo IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.Pilots(currentShipInfo.cboPilots.SelectedItem), Core.Pilot)
        Else
            hPilot = EveHQ.Core.HQ.myPilot
        End If
        showInfo.ShowItemDetails(cModule, hPilot)
        showInfo = Nothing
    End Sub

    Private Sub mnuAddToFavourites_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToFavourites_List.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        If Settings.HQFSettings.Favourites.Contains(cModule.Name) = False Then
            Settings.HQFSettings.Favourites.Add(cModule.Name)
        End If
    End Sub

    Private Sub mnuRemoveFromFavourites_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveFromFavourites.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        If Settings.HQFSettings.Favourites.Contains(cModule.Name) = True Then
            Settings.HQFSettings.Favourites.Remove(cModule.Name)
        End If
        Call CalculateFilteredModules(tvwItems.SelectedNode)
    End Sub

    Private Sub mnuShowModuleMarketGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowModuleMarketGroup.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim pathLine As String = CStr(Market.MarketGroupPath(cModule.MarketGroup))
        ShipModule.DisplayedMarketGroup = pathLine
    End Sub

#End Region

#Region "Menu & Button Routines"

    Private Sub btnScreenshot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScreenshot.Click
        ' Determine co-ords of current main panel
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim xy As Point = tp.PointToScreen(New Point(0, 0))
        Dim sx As Integer = xy.X
        Dim sy As Integer = xy.Y
        Dim fittingImage As Bitmap = ScreenGrab.GrabScreen(New Rectangle(sx, sy, tp.Width, tp.Height))
        Clipboard.SetDataObject(fittingImage)
        Dim filename As String = "HQF_" & tp.Text & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".png"
        fittingImage.Save(EveHQ.Core.HQ.reportFolder & "\" & filename, System.Drawing.Imaging.ImageFormat.Png)
    End Sub

    Private Sub mnuCopyForHQF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopyForHQF.Click
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim currentShip As Ship = currentShipSlot.ShipCurrent
        Dim fittedShip As Ship = currentShipSlot.ShipFitted
        Dim cModule As New ShipModule
        Dim state As Integer
        Dim fitting As New System.Text.StringBuilder
        fitting.AppendLine("[" & tp.Text & "]")
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

    Private Sub mnuCopyForEFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopyForEFT.Click
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim currentship As Ship = currentShipSlot.ShipFitted
        Dim cModule As New ShipModule
        Dim fitting As New System.Text.StringBuilder
        fitting.AppendLine("[" & tp.Text & "]")
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

    Private Sub mnuCopyForForums_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopyForForums.Click
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim currentship As Ship = currentShipSlot.ShipFitted
        Dim slots As Dictionary(Of String, Integer)
        Dim slotList As New ArrayList
        Dim slotCount As Integer = 0
        Dim cModule As New ShipModule
        Dim state As Integer
        Dim fitting As New System.Text.StringBuilder

        slots = New Dictionary(Of String, Integer)
        For slot As Integer = 1 To currentship.HiSlots
            If currentship.HiSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.HiSlot(slot).ModuleState) / Math.Log(2))
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
                state = CInt(Math.Log(currentship.MidSlot(slot).ModuleState) / Math.Log(2))
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
                state = CInt(Math.Log(currentship.LowSlot(slot).ModuleState) / Math.Log(2))
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
                state = CInt(Math.Log(currentship.RigSlot(slot).ModuleState) / Math.Log(2))
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
                fitting.AppendLine(cargo.Quantity & "x " & cargo.ItemType.Name & " (cargo)")
            Next
        End If
        Try
            Clipboard.SetText(fitting.ToString)
        Catch ex As Exception
            MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy For Forums Error")
        End Try
    End Sub

    Private Sub mnuShipStats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShipStats.Click
        Dim currentship As Ship = currentShipSlot.ShipFitted
        Dim stats As New System.Text.StringBuilder
        stats.AppendLine("[Statistics]")
        stats.AppendLine("")
        stats.AppendLine(currentShipInfo.lblEffectiveHP.Text)
        stats.AppendLine(currentShipInfo.lblTankAbility.Text)
        stats.AppendLine("Damage Profile - " & currentship.DamageProfile.Name & " (EM: " & FormatNumber(currentship.DamageProfileEM * 100, 2) & "%, Ex: " & FormatNumber(currentship.DamageProfileEX * 100, 2) & "%, Ki: " & FormatNumber(currentship.DamageProfileKI * 100, 2) & "%, Th: " & FormatNumber(currentship.DamageProfileTH * 100, 2) & "%)")
        stats.AppendLine("Shield Resists - EM: " & currentShipInfo.lblShieldEM.Text & ", Ex: " & currentShipInfo.lblShieldExplosive.Text & ", Ki: " & currentShipInfo.lblShieldKinetic.Text & ", Th: " & currentShipInfo.lblShieldThermal.Text)
        stats.AppendLine("Armor Resists - EM: " & currentShipInfo.lblArmorEM.Text & ", Ex: " & currentShipInfo.lblArmorExplosive.Text & ", Ki: " & currentShipInfo.lblArmorKinetic.Text & ", Th: " & currentShipInfo.lblArmorThermal.Text)
        stats.AppendLine("")
        stats.AppendLine(currentShipInfo.gbCapacitor.Text)
        stats.AppendLine("")
        stats.AppendLine("Volley Damage: " & FormatNumber(currentship.TotalVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        stats.AppendLine("DPS: " & FormatNumber(currentship.TotalDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        Try
            Clipboard.SetText(stats.ToString)
        Catch ex As Exception
            MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy Stats Error")
        End Try
    End Sub

#End Region

    Private Sub btnPilotManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPilotManager.Click
        Dim myPilotManager As New frmPilotManager
        If currentShipInfo IsNot Nothing Then
            myPilotManager.pilotName = currentShipInfo.cboPilots.SelectedItem.ToString
        Else
            myPilotManager.pilotName = EveHQ.Core.HQ.myPilot.Name
        End If
        myPilotManager.ShowDialog()
        myPilotManager = Nothing
    End Sub

    Private Sub OpenSettingsForm()
        ' Open options form
        Dim mySettings As New frmHQFSettings
        mySettings.ShowDialog()
        mySettings = Nothing
        Call Me.UpdateFittingsTree()
        Call Me.CheckOpenTabs()
    End Sub
    Private Sub CheckOpenTabs()
        ' Checks whether the open tabs are still valid fittings
        For Each tp As TabPage In tabHQF.TabPages
            If Fittings.FittingTabList.Contains(tp.Text) = False Then
                ShipLists.fittedShipList.Remove(tp.Text)
                tabHQF.TabPages.Remove(tp)
            End If
        Next
    End Sub

    Private Sub clvFittings_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles clvFittings.MouseDoubleClick
        If clvFittings.SelectedItems.Count > 0 Then
            If clvFittings.SelectedItems(0).Items.Count = 0 Then
                Call Me.ShowFitting(clvFittings.SelectedItems(0))
            End If
        End If
    End Sub

    Private Sub clvFittings_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles clvFittings.Resize
        clvFittings.Columns(0).Width = clvFittings.Width - 30
    End Sub

    Private Sub clvFittings_SelectedItemsChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clvFittings.SelectedItemsChanged
        If clvFittings.SelectedItems.Count > 1 Then
            mnuCompareFittings.Enabled = True
        Else
            If clvFittings.SelectedItems.Count = 1 Then
                If clvFittings.SelectedItems(0).Items.Count > 1 Then
                    mnuCompareFittings.Enabled = True
                Else
                    mnuCompareFittings.Enabled = False
                End If
            Else
                mnuCompareFittings.Enabled = False
            End If
        End If
    End Sub

    Private Sub mnuCompareShips_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call Me.CompareShips()
    End Sub

    Private Sub mnuCompareFittings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCompareFittings.Click
        Call Me.CompareShips()
    End Sub

    Private Sub CompareShips()
        ' Establish which fittings we will be comparing
        Dim Fittings As New SortedList
        For Each fitting As ContainerListViewItem In clvFittings.SelectedItems
            If fitting.Items.Count = 0 Then
                ' If we have highlighted an item
                If Fittings.Contains(fitting.ParentItem.Text & ", " & fitting.Text) = False Then
                    Fittings.Add(fitting.ParentItem.Text & ", " & fitting.Text, "")
                End If
            Else
                ' If we have highlighted a group
                For Each subFit As ContainerListViewItem In fitting.Items
                    If Fittings.Contains(subFit.ParentItem.Text & ", " & subFit.Text) = False Then
                        Fittings.Add(subFit.ParentItem.Text & ", " & subFit.Text, "")
                    End If
                Next
            End If
        Next
        Dim CompareShips As New frmShipComparison
        CompareShips.ShipList = Fittings
        CompareShips.ShowDialog()
        CompareShips.Dispose()
    End Sub

    Private Sub btnImportEFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportEFT.Click
        Dim myEFTImport As New frmEFTImport
        myEFTImport.ShowDialog()
        myEFTImport = Nothing
        Call Me.UpdateFittingsTree()
    End Sub
End Class