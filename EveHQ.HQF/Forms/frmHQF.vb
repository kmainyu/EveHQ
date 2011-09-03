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
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Xml
Imports Microsoft.Win32
Imports DevComponents.AdvTree

Public Class frmHQF

    Dim cActiveFitting As Fitting
    Dim LastSlotFitting As New ArrayList
    Dim LastModuleResults As New SortedList
    Dim ModuleListItems As New ArrayList
    Dim myPilotManager As New frmPilotManager
    Dim myBCBrowser As New frmBCBrowser
    Dim myEveImport As New frmEveImport
    Dim shutdownComplete As Boolean = False
    Dim shipPanelTemp As Boolean = False
    Dim modPanelTemp As Boolean = False

#Region "Class Wide Variables"

    Dim startUp As Boolean = False

    Dim cModuleDisplay As String = ""
    Public Property ModuleDisplay() As String
        Get
            Return cModuleDisplay
        End Get
        Set(ByVal value As String)
            cModuleDisplay = value
        End Set
    End Property

    Private Property ActiveFitting() As Fitting
        Get
            Return cActiveFitting
        End Get
        Set(ByVal value As Fitting)
            cActiveFitting = value
            If cActiveFitting IsNot Nothing Then
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

    Private Sub frmHQF_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' Remove events
        RemoveHandler HQFEvents.ShowModuleMarketGroup, AddressOf Me.UpdateMarketGroup
        RemoveHandler HQFEvents.FindModule, AddressOf Me.UpdateModulesThatWillFit
        RemoveHandler HQFEvents.UpdateFitting, AddressOf Me.UpdateFittings
        RemoveHandler HQFEvents.UpdateFittingList, AddressOf Me.UpdateShipFittings
        RemoveHandler HQFEvents.UpdateModuleList, AddressOf Me.UpdateModuleList
        RemoveHandler HQFEvents.UpdateShipInfo, AddressOf Me.UpdateShipInfo
        RemoveHandler HQFEvents.UpdateAllImplantLists, AddressOf Me.UpdateAllImplantLists

        If shutdownComplete = False Then
            ' Close any open windows
            If myPilotManager.IsHandleCreated Then myPilotManager.Close()
            If myBCBrowser.IsHandleCreated Then myBCBrowser.Close()

            ' Save the panel widths
            Settings.HQFSettings.ShipPanelWidth = panelShips.Width
            Settings.HQFSettings.ModPanelWidth = panelModules.Width
            Settings.HQFSettings.ShipSplitterWidth = panelFittings.Height
            Settings.HQFSettings.ModSplitterWidth = panelModFilters.Height

            ' Save fittings
            'MessageBox.Show("HQF is about to enter the routine to save the fittings file. There are " & Fittings.FittingList.Count & " fittings detected.", "Save Fittings Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Call Me.SaveFittings()

            ' Save pilots
            Call HQFPilotCollection.SaveHQFPilotData()

            ' Save the Settings
            Call Settings.HQFSettings.SaveHQFSettings()

            shutdownComplete = True
        End If

    End Sub

    Private Sub frmHQF_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        startUp = True
        ModuleDisplay = ""
        LastModuleResults.Clear()

        Me.SuspendLayout()

        ' Load the settings!
        Call Settings.HQFSettings.LoadHQFSettings()

        ' Clear tabs and fitted ship lists, results list
        ShipLists.fittedShipList.Clear()
        LastModuleResults.Clear()
        tabHQF.Dock = DockStyle.Fill ' Maximize the tab control after allowing for the ribbon merge container
        tabHQF.Tabs.Clear()
        Me.Show()
        Me.Refresh()

        AddHandler HQFEvents.ShowModuleMarketGroup, AddressOf Me.UpdateMarketGroup
        AddHandler HQFEvents.FindModule, AddressOf Me.UpdateModulesThatWillFit
        AddHandler HQFEvents.UpdateFitting, AddressOf Me.UpdateFittings
        AddHandler HQFEvents.UpdateFittingList, AddressOf Me.UpdateShipFittings
        AddHandler HQFEvents.UpdateModuleList, AddressOf Me.UpdateModuleList
        AddHandler HQFEvents.UpdateShipInfo, AddressOf Me.UpdateShipInfo
        AddHandler HQFEvents.UpdateAllImplantLists, AddressOf Me.UpdateAllImplantLists

        ' Load the Profiles - stored separately from settings for distribution!
        Call DamageProfiles.LoadProfiles()
        Call DefenceProfiles.LoadProfiles()

        ' Load up a collection of pilots from the EveHQ Core
        Call Me.LoadPilots()

        ' Load saved setups into the fitting array
        Call Me.LoadFittings()

        ' Set the MetaType Filter
        Call Me.SetMetaTypeFilters()

        ' Create Image Cache
        ImageHandler.BaseIcons.Clear()
        Dim IconList As New List(Of String)
        For Each sMod As ShipModule In ModuleLists.moduleList.Values
            If sMod.Icon <> "" Then
                If ImageHandler.BaseIcons.ContainsKey(sMod.Icon) = False Then
                    Dim OI As Bitmap = CType(My.Resources.ResourceManager.GetObject("_" & sMod.Icon), Bitmap)
                    If OI IsNot Nothing Then
                        ImageHandler.BaseIcons.Add(sMod.Icon, OI)
                    End If
                End If
            End If
        Next
        ImageHandler.MetaIcons.Clear()
        For idx As Integer = 0 To 32
            Dim OI As Bitmap = CType(My.Resources.ResourceManager.GetObject("Meta" & (2 ^ idx).ToString), Bitmap)
            If OI IsNot Nothing Then
                ImageHandler.MetaIcons.Add((2 ^ idx).ToString, OI)
            End If
        Next
        ' Combine the images
        Call ImageHandler.CombineIcons24()
        Call ImageHandler.CombineIcons48()

        ' Show the groups
        Call Me.ShowShipGroups()
        Call Me.ShowMarketGroups()

        startUp = False
        ' Temporarily disable the performance setting
        Dim performanceSetting As Boolean = HQF.Settings.HQFSettings.ShowPerformanceData
        HQF.Settings.HQFSettings.ShowPerformanceData = False

        ' Check if we need to restore tabs from the previous setup
        If HQF.Settings.HQFSettings.RestoreLastSession = True Then
            For Each FitKey As String In HQF.Settings.HQFSettings.OpenFittingList
                If Fittings.FittingList.ContainsKey(FitKey) = True Then
                    ' Create the tab and display
                    Dim newfit As Fitting = Fittings.FittingList(FitKey)
                    Call Me.CreateNewFittingTab(newfit)
                    newfit.ShipSlotCtrl.UpdateEverything()
                End If
            Next
            tabHQF.SelectedTabIndex = 0
        End If

        ' Set default widths of module list
        Dim ModuleListColumns As Integer = 5
        If HQF.Settings.HQFSettings.ModuleListColWidths.Count <> ModuleListColumns Then
            HQF.Settings.HQFSettings.ModuleListColWidths.Clear()
            HQF.Settings.HQFSettings.ModuleListColWidths.Add(0, 150)
            HQF.Settings.HQFSettings.ModuleListColWidths.Add(1, 40)
            HQF.Settings.HQFSettings.ModuleListColWidths.Add(2, 40)
            HQF.Settings.HQFSettings.ModuleListColWidths.Add(3, 40)
            HQF.Settings.HQFSettings.ModuleListColWidths.Add(4, 80)
        End If
        For col As Integer = 0 To ModuleListColumns - 1
            tvwModules.Columns(col).Width.Absolute = HQF.Settings.HQFSettings.ModuleListColWidths(CLng(col))
        Next

        ' Set the panel widths
        panelShips.Width = Settings.HQFSettings.ShipPanelWidth
        panelModules.Width = Settings.HQFSettings.ModPanelWidth
        panelFittings.Height = Settings.HQFSettings.ShipSplitterWidth
        panelModFilters.Height = Settings.HQFSettings.ModSplitterWidth

        HQF.Settings.HQFSettings.ShowPerformanceData = performanceSetting

        Me.ResumeLayout()

    End Sub
    Private Sub LoadFittings()
        Call SavedFittings.LoadFittings()
        'Fittings.FittingList.Clear()
        'If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin")) = True Then
        '    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin"), FileMode.Open)
        '    Dim f As BinaryFormatter = New BinaryFormatter
        '    Fittings.FittingList = CType(f.Deserialize(s), SortedList)
        '    s.Close()
        'End If
        '' Update Fitting Names
        'Dim updateList As New SortedList(Of String, String)
        'For Each fitting As String In Fittings.FittingList.Keys
        '    If fitting.Contains("Amarr Navy Slicer") Then
        '        Dim newFit As String = fitting.Replace("Amarr Navy Slicer", "Imperial Navy Slicer")
        '        updateList.Add(fitting, newFit)
        '    End If
        '    If fitting.Contains("Gallente Navy Comet") Then
        '        Dim newFit As String = fitting.Replace("Gallente Navy Comet", "Federation Navy Comet")
        '        updateList.Add(fitting, newFit)
        '    End If
        'Next
        'For Each fitting As String In updateList.Keys
        '    If Fittings.FittingList.ContainsKey(updateList(fitting)) = False Then
        '        Fittings.FittingList.Add(updateList(fitting), Fittings.FittingList(fitting))
        '    End If
        '    If Fittings.FittingList.ContainsKey(fitting) = False Then
        '        Fittings.FittingList.Remove(fitting)
        '    End If
        'Next
        '' Update Modules
        'For Each fitting As ArrayList In Fittings.FittingList.Values
        '    For idx As Integer = 0 To fitting.Count - 1
        '        fitting(idx) = fitting(idx).ToString.Replace("Amarr Navy", "Imperial Navy")
        '        fitting(idx) = fitting(idx).ToString.Replace("Gallente Navy", "Federation Navy")
        '    Next
        'Next
        Call Me.UpdateFittingsTree(True)
    End Sub
    Private Sub SaveFittings()
        Call SavedFittings.SaveFittings()
        'Try
        '    ' Save ships
        '    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin"), FileMode.Create)
        '    Dim f As New BinaryFormatter
        '    f.Serialize(s, Fittings.FittingList)
        '    s.Flush()
        '    s.Close()
        'Catch ex As Exception
        '    MessageBox.Show("There was an error saving the fittings file. The error was: " & ex.Message, "Save Fittings Failed :(", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End Try
    End Sub
    Private Sub ShowShipGroups()
        Dim sr As New StreamReader(Path.Combine(HQF.Settings.HQFCacheFolder, "ShipGroups.bin"))
        Dim ShipGroups As String = sr.ReadToEnd
        Dim PathLines() As String = ShipGroups.Split(ControlChars.CrLf.ToCharArray)
        Dim nodes() As String
        Dim cNode As New DevComponents.AdvTree.Node
        Dim isFlyable As Boolean = True
        sr.Close()
        tvwShips.BeginUpdate()
        tvwShips.Nodes.Clear()
        For Each pathline As String In PathLines
            If pathline.Trim <> "" Then
                If pathline.Contains("\") = False Then
                    Dim nNode As New DevComponents.AdvTree.Node
                    nNode.Name = pathline
                    nNode.Text = pathline
                    tvwShips.Nodes.Add(nNode)
                Else
                    nodes = pathline.Split("\".ToCharArray)
                    If ShipLists.shipListKeyName.ContainsKey(nodes(nodes.Length - 1)) And cboFlyable.SelectedIndex > 0 Then
                        'If nodes.Length = 5 And cboFlyable.SelectedIndex > 0 Then
                        isFlyable = IsShipFlyable(nodes(nodes.Length - 1), cboFlyable.SelectedItem.ToString)
                    Else
                        isFlyable = True
                    End If
                    If isFlyable = True Then
                        Dim parent As String = nodes(0)
                        If nodes.Length > 2 Then
                            For node As Integer = 1 To nodes.GetUpperBound(0) - 1
                                parent &= "\" & nodes(node)
                            Next
                        End If
                        cNode = tvwShips.FindNodeByName(parent)
                        Dim nNode As New DevComponents.AdvTree.Node
                        nNode.Text = nodes(nodes.GetUpperBound(0))
                        nNode.Name = pathline
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
        For Each ShipNode As Node In tvwShips.Nodes
            If Market.MarketShipList.Contains(ShipNode.Text) = False Then
                Market.MarketShipList.Add(ShipNode.Text)
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
                If ShipLists.shipListKeyName.ContainsKey(cNode.Text) = False Then
                    ' Remove the node
                    pNode.Nodes.RemoveAt(cNodeIdx)
                    cNodeIdx -= 1
                End If
            End If
            cNodeIdx += 1
        Loop Until cNodeIdx = pNode.Nodes.Count
    End Sub
    Private Sub ShowMarketGroups()
        Dim sr As New StreamReader(Path.Combine(HQF.Settings.HQFCacheFolder, "ItemGroups.bin"))
        Dim ShipGroups As String = sr.ReadToEnd
        Dim PathLines() As String = ShipGroups.Split(ControlChars.CrLf.ToCharArray)
        Dim nodes() As String
        Dim nodeData() As String
        Dim cNode As New Node
        Dim newNode As New Node
        sr.Close()
        tvwItems.BeginUpdate()
        tvwItems.Nodes.Clear()
        Market.MarketGroupList.Clear()
        Market.MarketGroupPath.Clear()
        For Each pathline As String In PathLines
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
                    Dim parent As String = nodes(0)
                    If nodes.Length > 2 Then
                        For node As Integer = 1 To nodes.GetUpperBound(0) - 1
                            parent &= "\" & nodes(node)
                        Next
                    End If
                    cNode = tvwItems.FindNodeByName(parent)
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
        Dim cNode As New Node
        Dim nodes() As String = path.Split("\".ToCharArray)
        Dim parent As String = nodes(0)
        If nodes.Length >= 2 Then
            For node As Integer = 1 To nodes.GetUpperBound(0)
                parent &= "\" & nodes(node)
            Next
        End If
        cNode = tvwItems.FindNodeByName(parent)
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
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFPilotSettings.bin")) = True Then
            Call HQFPilotCollection.LoadHQFPilotData()
            ' Check we have all the available pilots!
            Dim morePilots As Boolean = False
            For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
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
            For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
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

        ' Remove old HQF Pilots
        Dim removePilotList As New ArrayList
        For Each hPilot As String In HQFPilotCollection.HQFPilots.Keys
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(hPilot) = False Then
                removePilotList.Add(hPilot)
            End If
        Next
        For Each hpilot As String In removePilotList
            HQFPilotCollection.HQFPilots.Remove(hpilot)
        Next
        removePilotList.Clear()

        If HQFPilotCollection.HQFPilots.Count > 0 Then
            btnPilotManager.Enabled = True
            btnImplantManager.Enabled = True
        End If

        ' Update the ship filter
        Call Me.UpdateShipFilter()

    End Sub
    Private Sub UpdateShipFilter()
        cboFlyable.BeginUpdate()
        cboFlyable.Items.Clear()
        cboFlyable.Items.Add("<All Ships>")
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
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
            Dim chkBox As CheckBox = CType(Me.panelModFilters.Controls.Item("chkFilter" & filter.ToString), CheckBox)
            If (HQF.Settings.HQFSettings.ModuleFilter And filter) = filter Then
                chkBox.Checked = True
            Else
                chkBox.Checked = False
            End If
        Next
    End Sub
#End Region

#Region "Ship Browser Routines"
    Private Sub tvwShips_NodeMouseClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles tvwShips.NodeClick
        tvwShips.SelectedNode = e.Node
    End Sub
    Private Sub tvwShips_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles tvwShips.NodeDoubleClick
        tvwShips.SelectedNode = e.Node
        Dim curNode As DevComponents.AdvTree.Node = tvwShips.SelectedNode
        If curNode IsNot Nothing Then
            If curNode.Nodes.Count = 0 Then ' If has no child nodes, therefore a ship not group
                Dim shipName As String = curNode.Text
                mnuShipBrowserShipName.Text = shipName
                Call Me.CreateNewFitting(shipName)
            End If
        End If
    End Sub
    Private Sub ctxShipBrowser_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxShipBrowser.Opening
        Dim curNode As DevComponents.AdvTree.Node = tvwShips.SelectedNode
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
    Private Sub mnuCreateNewFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCreateNewFitting.Click
        ' Get the ship details
        Dim shipName As String = mnuShipBrowserShipName.Text
        Call Me.CreateNewFitting(shipName)
    End Sub
    Private Sub mnuPreviewShip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPreviewShip.Click
        Dim shipName As String = mnuShipBrowserShipName.Text
        Dim selShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        If ActiveFitting IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), Core.Pilot)
        Else
            If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot)
            Else
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot)
            End If
        End If
        showInfo.ShowItemDetails(selShip, hPilot)
    End Sub
    Private Sub mnuBattleClinicBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBattleClinicBrowser.Click
        Dim shipName As String = mnuShipBrowserShipName.Text
        Dim bShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
        If myBCBrowser.IsHandleCreated = True Then
            myBCBrowser.ShipType = bShip
            myBCBrowser.BringToFront()
        Else
            myBCBrowser = New frmBCBrowser
            myBCBrowser.ShipType = bShip
            myBCBrowser.Show()
        End If
    End Sub
    Private Sub DisplayBCBrowser(ByVal bShip As Ship)
        Dim URI As String = "http://eve.battleclinic.com/ship_loadout_feed.php?typeID=" & bShip.ID

    End Sub
    Private Sub CreateNewFitting(ByVal shipName As String)
        ' Check we have some valid characters
        If HQF.HQFPilotCollection.HQFPilots.Count > 0 Then
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
            If myNewFitting.DialogResult = Windows.Forms.DialogResult.Cancel Then
                MessageBox.Show("Create New Fitting has been cancelled!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If fittingName <> "" Then
                    Dim NewFit As New Fitting(shipName, fittingName, HQF.Settings.HQFSettings.DefaultPilot)
                    Fittings.FittingList.Add(NewFit.KeyName, NewFit)
                    If Me.CreateNewFittingTab(NewFit) = True Then
                        Call Me.UpdateFilteredShips()
                        tabHQF.SelectedTab = tabHQF.Tabs(NewFit.KeyName)
                        If tabHQF.Tabs.Count = 1 Then
                            Call Me.UpdateSelectedTab()   ' Called when tabpage count=0 as SelectedIndexChanged does not fire!
                        End If
                        ActiveFitting.ShipSlotCtrl.UpdateEverything()
                    End If
                Else
                    MessageBox.Show("Unable to create new fitting due to insufficient data!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
            myNewFitting = Nothing
        Else
            Dim msg As String = "There appears to be no pilots or accounts created in EveHQ." & ControlChars.CrLf
            msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
            msg &= ControlChars.CrLf & ControlChars.CrLf
            msg &= "If you have just added accounts or pilots with HQF open, please close the HQF plug-in and re-open it."
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
            Dim isFlyable As Boolean = True
            For Each sShip As String In ShipLists.shipList.Keys
                If sShip.ToLower.Contains(strSearch) Then
                    If cboFlyable.SelectedIndex > 0 Then
                        isFlyable = IsShipFlyable(sShip, cboFlyable.SelectedItem.ToString)
                    Else
                        isFlyable = True
                    End If
                    If isFlyable = True Then
                        shipResults.Add(sShip, sShip)
                    End If
                End If
            Next
            tvwShips.BeginUpdate()
            tvwShips.Nodes.Clear()
            For Each item As String In shipResults.Values
                Dim shipNode As New DevComponents.AdvTree.Node
                shipNode.Text = item
                shipNode.Name = item
                tvwShips.Nodes.Add(shipNode)
            Next
            tvwShips.EndUpdate()

            ' Redraw the fitting tree

            Dim shipName As String = ""
            Dim fittingName As String = ""
            Dim fittingSep As Integer = 0
            Dim fitResults As New SortedList(Of String, String)
            For Each sFit As String In Fittings.FittingList.Keys
                If sFit.ToLower.Contains(strSearch) Then
                    fittingSep = sFit.IndexOf(", ")
                    shipName = sFit.Substring(0, fittingSep)
                    If cboFlyable.SelectedIndex > 0 Then
                        isFlyable = IsShipFlyable(shipName, cboFlyable.SelectedItem.ToString)
                    Else
                        isFlyable = True
                    End If
                    If isFlyable = True Then
                        fitResults.Add(sFit, sFit)
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
            For Each item As String In fitResults.Values
                fittingSep = item.IndexOf(", ")
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
            Call Me.ShowShipGroups()
            Call Me.UpdateFittingsTree(False)
        End If
    End Sub
    Private Sub UpdateShipFittings()
        If Len(txtShipSearch.Text) > 0 Then
            Dim strSearch As String = txtShipSearch.Text.Trim.ToLower

            ' Redraw the fitting tree
            Dim fitResults As New SortedList(Of String, String)
            For Each sFit As String In Fittings.FittingList.Keys
                If sFit.ToLower.Contains(strSearch) Then
                    fitResults.Add(sFit, sFit)
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
            Dim shipName As String = ""
            Dim fittingName As String = ""
            Dim fittingSep As Integer = 0
            For Each item As String In fitResults.Values
                fittingSep = item.IndexOf(", ")
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
            Call Me.UpdateFittingsTree(False)
        End If
    End Sub
    Private Sub btnResetShips_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetShips.Click
        txtShipSearch.Text = ""
        Call Me.UpdateShipFilter()
    End Sub
    Private Sub cboFlyable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFlyable.SelectedIndexChanged
        If startUp = False Then
            Call ShowShipGroups()
            Call Me.UpdateFittingsTree(False)
        End If
    End Sub
    Private Function IsShipFlyable(ByVal shipName As String, ByVal pilotName As String) As Boolean
        Dim testShip As Ship = CType(ShipLists.shipList(shipName), Ship)
        Dim testPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(pilotName), HQFPilot)
        If testShip IsNot Nothing And testPilot IsNot Nothing Then
            Return Engine.IsFlyable(testPilot, testShip)
        Else
            Return False
        End If
    End Function
    Private Sub mnuAddToShipBay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToShipBay.Click
        If ActiveFitting.ShipSlotCtrl IsNot Nothing Then
            Dim shipName As String = mnuShipBrowserShipName.Text
            ' Add the ship to the maintenance bay
            Dim shipType As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
            ActiveFitting.AddShip(shipType, 1, False)
        End If
    End Sub

#End Region

#Region "Module Display, Filter and Search Options"

    Private Sub tvwItems_AfterNodeSelect(ByVal sender As Object, ByVal e As DevComponents.AdvTree.AdvTreeNodeEventArgs) Handles tvwItems.AfterNodeSelect
        Call Me.CalculateFilteredModules(e.Node)
    End Sub
    Private Sub tvwItems_NodeClick1(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles tvwItems.NodeClick
        tvwItems.SelectedNode = e.Node
    End Sub
    Private Sub MetaFilterChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilter1.CheckedChanged, chkFilter2.CheckedChanged, chkFilter4.CheckedChanged, chkFilter8.CheckedChanged, chkFilter16.CheckedChanged, chkFilter32.CheckedChanged, chkFilter8192.CheckedChanged
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
    Private Sub CalculateFilteredModules(ByVal groupNode As Node)
        Me.Cursor = Cursors.WaitCursor
        Dim sMod As New ShipModule
        Dim groupID As String
        Dim results As New SortedList
        If groupNode.Name = "Favourites" Then
            ModuleDisplay = "Favourites"
            For Each modName As String In Settings.HQFSettings.Favourites
                If HQF.ModuleLists.moduleListName.Contains(modName) = True Then
					sMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(modName)), ShipModule)
                    ' Add results in by name, module
                    If ActiveFitting IsNot Nothing Then
						If chkApplySkills.Checked = True Then
							sMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(modName)), ShipModule).Clone
							ActiveFitting.ApplySkillEffectsToModule(sMod, True)
						End If
                    End If
                    If ActiveFitting IsNot Nothing Then
                        If chkOnlyShowUsable.Checked = True Then
                            If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                If chkOnlyShowFittable.Checked = True Then
                                    If Engine.IsFittable(sMod, ActiveFitting.FittedShip) Then
                                        results.Add(sMod.Name, sMod)
                                    End If
                                Else
                                    results.Add(sMod.Name, sMod)
                                End If
                            End If
                        Else
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, ActiveFitting.FittedShip) Then
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
            lblModuleDisplayType.Tag = "Displaying: Favourites"
            LastModuleResults = results
        ElseIf groupNode.Name = "Recently Used" Then
            ModuleDisplay = "Recently Used"
            For Each modName As String In Settings.HQFSettings.MRUModules
                If HQF.ModuleLists.moduleListName.Contains(modName) = True Then
					sMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(modName)), ShipModule)
                    ' Add results in by name, module
                    If ActiveFitting IsNot Nothing Then
						If chkApplySkills.Checked = True Then
							sMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(modName)), ShipModule).Clone
							ActiveFitting.ApplySkillEffectsToModule(sMod, True)
						End If
                    End If
                    If ActiveFitting IsNot Nothing Then
                        If chkOnlyShowUsable.Checked = True Then
                            If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                If chkOnlyShowFittable.Checked = True Then
                                    If Engine.IsFittable(sMod, ActiveFitting.FittedShip) Then
                                        results.Add(sMod.Name, sMod)
                                    End If
                                Else
                                    results.Add(sMod.Name, sMod)
                                End If
                            End If
                        Else
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, ActiveFitting.FittedShip) Then
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
            lblModuleDisplayType.Tag = "Displaying: Recently Used"
            LastModuleResults = results
        Else
            If groupNode.Nodes.Count = 0 Then
                groupID = groupNode.Tag.ToString
                ModuleDisplay = groupID
                Call Me.AddGroupResults(HQF.ModuleLists.moduleList, groupID, results)
                lblModuleDisplayType.Tag = Market.MarketGroupList(groupID).ToString
                lblModuleDisplayType.Tag = "Displaying: " & lblModuleDisplayType.Tag.ToString
                LastModuleResults = results
            Else
                ' Check on the last results
                If LastModuleResults.Count > 0 Then
                    groupID = CType(LastModuleResults.GetByIndex(0), ShipModule).MarketGroup
                Else
                    groupID = "0"
                End If
                Call Me.AddGroupResults(LastModuleResults, groupID, results)
                LastModuleResults = results
            End If
        End If
        Call Me.ShowFilteredModules()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub AddGroupResults(ByVal ModuleList As SortedList, ByVal groupID As String, ByRef Results As SortedList)
        Dim sMod As New ShipModule
        For Each shipMod As ShipModule In ModuleList.Values
            If shipMod.MarketGroup = groupID Then
                ' Add results in by name, module
				sMod = CType(HQF.ModuleLists.moduleList(shipMod.ID), ShipModule)
                If ActiveFitting IsNot Nothing Then
					If chkApplySkills.Checked = True Then
						sMod = CType(HQF.ModuleLists.moduleList(shipMod.ID), ShipModule).Clone
						ActiveFitting.ApplySkillEffectsToModule(sMod, True)
					End If
                End If
                If ActiveFitting IsNot Nothing Then
                    If chkOnlyShowUsable.Checked = True Then
                        If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, ActiveFitting.FittedShip) Then
                                    Results.Add(sMod.Name, sMod)
                                End If
                            Else
                                Results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Else
                        If chkOnlyShowFittable.Checked = True Then
                            If ActiveFitting IsNot Nothing Then
                                If Engine.IsFittable(sMod, ActiveFitting.FittedShip) Then
                                    Results.Add(sMod.Name, sMod)
                                End If
                            Else
                                Results.Add(sMod.Name, sMod)
                            End If
                        Else
                            Results.Add(sMod.Name, sMod)
                        End If
                    End If
                Else
                    Results.Add(sMod.Name, sMod)
                End If
            End If
        Next
    End Sub
    Private Sub ShowFilteredModules()
        Call DisplaySelectedModules()
        ModuleDisplay = "Filter"
    End Sub
    Private Sub CalculateSearchedModules()
		Me.Cursor = Cursors.WaitCursor
		Dim sMod As New ShipModule
		If Len(txtSearchModules.Text) > 2 Then
			Dim strSearch As String = txtSearchModules.Text.Trim.ToLower
			Dim results As New SortedList
			For Each item As String In HQF.ModuleLists.moduleListName.Keys
				If item.ToLower.Contains(strSearch) Then
					' Add results in by name, module
					sMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(item)), ShipModule)
					If ActiveFitting IsNot Nothing Then
						If chkApplySkills.Checked = True Then
							sMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(item)), ShipModule).Clone
							ActiveFitting.ApplySkillEffectsToModule(sMod, True)
						End If
					End If
					If ActiveFitting IsNot Nothing Then
						If ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem IsNot Nothing Then
							If chkOnlyShowUsable.Checked = True Then
								If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
									If chkOnlyShowFittable.Checked = True Then
										If Engine.IsFittable(sMod, ActiveFitting.FittedShip) Then
											results.Add(sMod.Name, sMod)
										End If
									Else
										results.Add(sMod.Name, sMod)
									End If
								End If
							Else
								If chkOnlyShowFittable.Checked = True Then
									If Engine.IsFittable(sMod, ActiveFitting.FittedShip) Then
										results.Add(sMod.Name, sMod)
									End If
								Else
									results.Add(sMod.Name, sMod)
								End If
							End If
						Else
							results.Add(sMod.Name, sMod)
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
        Call DisplaySelectedModules()
        ModuleDisplay = "Search"
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
			sMod = cMod
            If ActiveFitting IsNot Nothing Then
				If chkApplySkills.Checked = True Then
					sMod = cMod.Clone
					ActiveFitting.ApplySkillEffectsToModule(sMod, True)
				End If
            End If
            If ActiveFitting.ShipSlotCtrl IsNot Nothing Then
                If sMod.SlotType = slotType Then
                    Select Case slotType
                        Case 16 ' Subsystem Slot
                            If CStr(Int(sMod.Attributes("1380"))) = CStr(ActiveFitting.BaseShip.ID) Then
                                If chkOnlyShowUsable.Checked = True Then
                                    If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                        results.Add(sMod.Name, sMod)
                                    End If
                                Else
                                    results.Add(sMod.Name, sMod)
                                End If
                            End If
                        Case 1 ' Rig Slot
                            If ActiveFitting.BaseShip.Attributes("1547") = sMod.Attributes("1547") Then
                                If sMod.Calibration <= Calibration Then
                                    If chkOnlyShowUsable.Checked = True Then
                                        If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                            results.Add(sMod.Name, sMod)
                                        End If
                                    Else
                                        If ActiveFitting.BaseShip.Attributes("1547") = sMod.Attributes("1547") Then
                                            results.Add(sMod.Name, sMod)
                                        End If
                                    End If
                                End If
                            End If
                        Case 2, 4 ' Low & Mid Slot
                            If sMod.CPU <= CPU Then
                                If sMod.PG <= PG Then
                                    If chkOnlyShowUsable.Checked = True Then
                                        If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
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
                                                If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
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
            End If
        Next
        LastModuleResults = results
        lblModuleDisplayType.Tag = "Displaying: Modules That Fit"
        Call Me.ShowModulesThatWillFit()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ShowModulesThatWillFit()
        Call DisplaySelectedModules()
        ModuleDisplay = "Fitted"
    End Sub
    Private Sub txtSearchModules_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchModules.TextChanged
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
        For Each shipmod As ShipModule In LastModuleResults.Values
            If shipmod.SlotType <> 0 Or (shipmod.SlotType = 0 And (shipmod.IsBooster Or shipmod.IsCharge Or shipmod.IsDrone)) Then
                If (shipmod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipmod.MetaType Then
                    Dim newModule As New Node
                    newModule.Name = shipmod.ID
                    newModule.Text = shipmod.Name
                    newModule.Cells.Add(New Cell(shipmod.MetaLevel.ToString))
                    newModule.Cells.Add(New Cell(shipmod.CPU.ToString))
                    newModule.Cells.Add(New Cell(shipmod.PG.ToString))
                    newModule.Cells.Add(New Cell(CStr(EveHQ.Core.DataFunctions.GetPrice(shipmod.ID))))
                    newModule.Cells(4).TextDisplayFormat = "N2"
                    newModule.Style = New DevComponents.DotNetBar.ElementStyle
                    newModule.Style.Font = Me.Font
                    newModule.Image = ImageHandler.IconImage24(shipmod.Icon, shipmod.MetaType)
                    Dim stt As New DevComponents.DotNetBar.SuperTooltipInfo
                    stt.HeaderText = shipmod.Name
                    stt.FooterText = "Module Info"
                    stt.BodyText = ""
                    If shipmod.SlotType = SlotTypes.Subsystem Then
                        stt.BodyText &= "Slot Modifiers - High: " & shipmod.Attributes("1374") & ", Mid: " & shipmod.Attributes("1375") & ", Low: " & shipmod.Attributes("1376") & ControlChars.CrLf & ControlChars.CrLf
                    End If
                    stt.BodyText &= shipmod.Description
                    stt.Color = DevComponents.DotNetBar.eTooltipColor.Yellow
                    'stt.FooterImage = CType(My.Resources.imgInfo1, Image)
                    stt.BodyImage = ImageHandler.IconImage48(shipmod.Icon, shipmod.MetaType)
                    SuperTooltip1.SetSuperTooltip(newModule, stt)
                    Select Case shipmod.SlotType
                        Case SlotTypes.Subsystem  ' Subsystem
                            newModule.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.SubSlotColour))
                        Case SlotTypes.High  ' High
                            newModule.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                        Case SlotTypes.Mid  ' Mid
                            newModule.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                        Case SlotTypes.Low  ' Low
                            newModule.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                        Case SlotTypes.Rig  ' Rig
                            newModule.Style.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                    End Select
                    Dim chkFilter As CheckBox = CType(Me.panelModFilters.Controls("chkFilter" & shipmod.MetaType), CheckBox)
                    If chkFilter IsNot Nothing Then
                        chkFilter.ForeColor = Color.Black
                    End If
                    tvwModules.Nodes.Add(newModule)
                Else
                    Dim chkFilter As CheckBox = CType(Me.panelModFilters.Controls("chkFilter" & shipmod.MetaType), CheckBox)
                    If chkFilter IsNot Nothing Then
                        chkFilter.ForeColor = Color.LimeGreen
                    End If
                End If
            End If
        Next
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
        tvwModules.EndUpdate()
        'endTime = Now
        'MessageBox.Show((endTime - startTime).TotalMilliseconds.ToString & "ms", "DisplayModules Time", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

#Region "Module List Routines"

    Private Sub tvwModules_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvwModules.ColumnHeaderMouseUp
        CType(sender, DevComponents.AdvTree.ColumnHeader).Sort()
    End Sub
    Private Sub tvwModules_ColumnResized(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvwModules.ColumnResized
        Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        Dim idx As Integer = ch.DisplayIndex
        HQF.Settings.HQFSettings.ModuleListColWidths(CLng(idx)) = ch.Width.Absolute
    End Sub
	Private Sub tvwModules_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles tvwModules.NodeDoubleClick
		If ActiveFitting IsNot Nothing Then
			If ActiveFitting.ShipSlotCtrl IsNot Nothing Then
				If tvwModules.SelectedNodes.Count > 0 Then
					Dim moduleID As String = e.Node.Name
					Dim shipMod As ShipModule = CType(ModuleLists.moduleList(moduleID), ShipModule).Clone
					If shipMod.IsDrone = True Then
						Dim active As Boolean = False
						Call ActiveFitting.AddDrone(shipMod, 1, False, False)
					Else
						' Check if module is a charge
						If shipMod.IsCharge = True Or shipMod.IsContainer Then
							ActiveFitting.AddItem(shipMod, 1, False)
						Else
							' Must be a proper module then!
                            ActiveFitting.AddModule(shipMod, 0, True, False, Nothing, False, False)
							' Add it to the MRU
							Call Me.UpdateMRUModules(shipMod.Name)
						End If
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

    Public Sub UpdateFittings()
        Me.Cursor = Cursors.WaitCursor
        ' Updates all the open fittings
        For Each thisTab As DevComponents.DotNetBar.TabItem In tabHQF.Tabs
            Dim thisFit As Fitting = Fittings.FittingList(thisTab.Text)
            thisFit.ShipSlotCtrl.UpdateEverything()
        Next
        Me.Cursor = Cursors.Default
    End Sub

    Public Sub UpdateAllImplantLists()
        Me.Cursor = Cursors.WaitCursor
        ' Updates all implant lists in the open fittings
        For Each thisTab As DevComponents.DotNetBar.TabItem In tabHQF.Tabs
            Dim thisFit As Fitting = Fittings.FittingList(thisTab.Text)
            thisFit.ShipInfoCtrl.UpdateImplantList()
        Next
        Me.Cursor = Cursors.Default
    End Sub

    Public Sub UpdateShipInfo(ByVal pilotName As String)
        Me.Cursor = Cursors.WaitCursor
        ' Updates all the open fittings
        For Each thisTab As DevComponents.DotNetBar.TabItem In tabHQF.Tabs
            If thisTab IsNot Nothing Then
                If thisTab.AttachedControl IsNot Nothing Then
                    Dim shipSlots As ShipSlotControl = CType(thisTab.AttachedControl.Controls("panelShipSlot").Controls("shipSlot"), ShipSlotControl)
                    Dim shipInfo As ShipInfoControl = CType(thisTab.AttachedControl.Controls("panelShipInfo").Controls("shipInfo"), ShipInfoControl)
                    If pilotName = shipInfo.cboPilots.SelectedItem.ToString Then
                        ' Build the Affections data for this pilot
                        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(shipInfo.cboPilots.SelectedItem), HQFPilot)
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
        Me.Cursor = Cursors.Default
        HQFEvents.StartUpdateModuleList = True
    End Sub


#Region "TabHQF Selection and Context Menu Routines"

    Private Sub tabHQF_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabHQF.SelectedTabChanged
        Call Me.UpdateSelectedTab()
    End Sub
    Private Sub UpdateSelectedTab()
        If tabHQF.SelectedTab IsNot Nothing Then
            ActiveFitting = Fittings.FittingList(tabHQF.SelectedTab.Text)
        End If
    End Sub
#End Region

#Region "Clipboard Paste Routines (incl Timer)"
    Private Sub tmrClipboard_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrClipboard.Tick
        ' Checks the clipboard for any compatible items!
        Try
            If Clipboard.GetDataObject IsNot Nothing Then
                Dim fileText As String = CStr(Clipboard.GetDataObject().GetData(DataFormats.Text))
                If fileText IsNot Nothing Then
                    Dim fittingMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(fileText, "\[(?<ShipName>[^,]*)\]|\[(?<ShipName>.*),\s?(?<FittingName>.*)\]")
                    If fittingMatch.Success = True Then
                        ' Appears to be a match so lets check the ship type
                        If ShipLists.shipList.Contains(fittingMatch.Groups.Item("ShipName").Value) = True Then
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

    Private Sub UpdateFittingsTree(ByVal CollapseAllNodes As Boolean)
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
        Dim shipName As String = ""
        Dim fittingName As String = ""
        Dim isFlyable As Boolean = True

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

    Private Function CreateNewFittingTab(ByVal NewFit As Fitting) As Boolean
        If ShipLists.shipList.ContainsKey(NewFit.ShipName) = True Then
            NewFit.BaseShip.DamageProfile = CType(DamageProfiles.ProfileList.Item("<Omni-Damage>"), DamageProfile)
            ShipLists.fittedShipList.Add(NewFit.KeyName, NewFit.BaseShip)

            tabHQF.SuspendLayout()

            ' Create the tab page
            Dim tp As DevComponents.DotNetBar.TabItem = tabHQF.CreateTab(NewFit.KeyName)
            tp.Tag = NewFit.KeyName
            tp.Name = NewFit.KeyName

            ' Create the Ship Slot panel
            Dim pSS As New Panel
            pSS.BorderStyle = BorderStyle.Fixed3D
            pSS.Dock = System.Windows.Forms.DockStyle.Fill
            pSS.Location = New System.Drawing.Point(0, 0)
            pSS.Name = "panelShipSlot"
            pSS.Size = New System.Drawing.Size(414, 600)
            pSS.TabIndex = 1

            ' Create the Ship Info Panel
            Dim pSI As New Panel
            pSI.Dock = System.Windows.Forms.DockStyle.Left
            pSI.Location = New System.Drawing.Point(0, 384)
            pSI.Name = "panelShipInfo"
            pSI.Size = New System.Drawing.Size(270, 600)
            pSI.TabIndex = 0

            ' Attach the panels to the tab page
            tp.AttachedControl.Controls.Add(pSS)
            tp.AttachedControl.Controls.Add(pSI)

            ' Create a new Ship Slot Control
            Dim shipSlot As New ShipSlotControl(NewFit)
            shipSlot.Name = "shipSlot"
            shipSlot.Location = New Point(0, 0)
            shipSlot.Dock = DockStyle.Fill
            pSS.Controls.Add(shipSlot)
            ' TODO: Check if a custom ship - this should be done in the constructor of the SSC
            Dim baseID As String = ""
            If CustomHQFClasses.CustomShipIDs.ContainsKey(NewFit.BaseShip.ID) Then
                baseID = ShipLists.shipListKeyName(CustomHQFClasses.CustomShips(NewFit.BaseShip.Name).BaseShipName)
            Else
                baseID = NewFit.BaseShip.ID
            End If
            shipSlot.pbShip.Image = EveHQ.Core.ImageHandler.GetImage(baseID, 32)

            ' Create a new Ship Info Control
            Dim shipInfo As New ShipInfoControl(NewFit)
            shipInfo.Name = "shipInfo"
            shipInfo.Location = New Point(0, 0)
            shipInfo.Dock = DockStyle.Fill
            pSI.Controls.Add(shipInfo)

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

    Private Sub ctxFittings_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxFittings.Opening
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
    Private Sub mnuFittingsShowFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsShowFitting.Click
        ' Get the node details
        Dim fittingnode As Node = tvwFittings.SelectedNodes(0)
        Call Me.ShowFitting(fittingnode)
    End Sub
    Private Sub mnuFittingsRenameFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsRenameFitting.Click
        ' Get the node details
        Dim curNode As Node = tvwFittings.SelectedNodes(0)
        Dim parentnode As Node = curNode.Parent
        Dim shipName As String = parentnode.Text
        Dim fitName As String = curNode.Text
        Dim oldKeyName As String = shipName & ", " & fitName
        Dim FitToCopy As Fitting = Fittings.FittingList(oldKeyName)

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

        If myNewFitting.DialogResult = Windows.Forms.DialogResult.Cancel Then
            MessageBox.Show("Rename Fitting has been cancelled!", "Rename Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If fittingName <> "" Then
                Fittings.FittingList.Remove(oldKeyName)
                Dim NewFit As Fitting = FitToCopy.Clone
                NewFit.FittingName = fittingName
                Fittings.FittingList.Add(NewFit.KeyName, NewFit)
                ' Amend it in the tabs if it's there!
                Dim tp As DevComponents.DotNetBar.TabItem = tabHQF.Tabs(oldKeyName)
                If tp IsNot Nothing Then
                    Dim copyShip As Ship = CType(ShipLists.fittedShipList(oldKeyName), Ship).Clone
                    ShipLists.fittedShipList.Remove(oldKeyName)
                    ShipLists.fittedShipList.Add(NewFit.KeyName, copyShip)
                    tp.Name = NewFit.KeyName
                    tp.Tag = NewFit.KeyName
                    tp.Text = NewFit.KeyName
                End If
                Call Me.UpdateFilteredShips()
            Else
                MessageBox.Show("Unable to rename fitting due to insufficient data!", "Rename Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
        myNewFitting = Nothing
    End Sub
    Private Sub mnuFittingsCopyFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsCopyFitting.Click
        ' Get the node details
        Dim curNode As Node = tvwFittings.SelectedNodes(0)
        Dim parentnode As Node = curNode.Parent
        Dim shipName As String = parentnode.Text
        Dim fitName As String = curNode.Text
        Dim fitKeyName As String = shipName & ", " & fitName
        Dim FitToCopy As Fitting = Fittings.FittingList(fitKeyName)

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
        If myNewFitting.DialogResult = Windows.Forms.DialogResult.Cancel Then
            MessageBox.Show("Copy Fitting has been cancelled!", "Copy Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If fittingName <> "" Then
                Dim NewFit As Fitting = FitToCopy.Clone
                NewFit.FittingName = fittingName
                Fittings.FittingList.Add(NewFit.KeyName, NewFit)
                Call Me.UpdateFilteredShips()
            Else
                MessageBox.Show("Unable to copy fitting due to insufficient data!", "Copy Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
        myNewFitting = Nothing
    End Sub
    Private Sub mnuFittingsDeleteFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsDeleteFitting.Click
        ' Get the node details
        Dim response As Integer = 0
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
        If response = Windows.Forms.DialogResult.Yes Then
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
                            Dim ti As DevComponents.DotNetBar.TabItem = tabHQF.Tabs(fittingKeyName)
                            If ti IsNot Nothing Then
                                tabHQF.Tabs.Remove(ti)
                                ShipLists.fittedShipList.Remove(ti.Text)
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
                Dim tp As DevComponents.DotNetBar.TabItem = tabHQF.Tabs(fittingKeyName)
                If tp IsNot Nothing Then
                    tabHQF.Tabs.Remove(tp)
                    ShipLists.fittedShipList.Remove(tp.Text)
                End If
            Next
            ' Update the list
            Call Me.UpdateFilteredShips()
        End If
    End Sub
    Private Sub mnuFittingsCreateFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsCreateFitting.Click
        ' Get the node details
        Dim curNode As Node = tvwFittings.SelectedNodes(0)
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
            Dim NewFit As New Fitting(shipName, fittingName, HQF.Settings.HQFSettings.DefaultPilot)
            Fittings.FittingList.Add(NewFit.KeyName, NewFit)
            If Me.CreateNewFittingTab(NewFit) = True Then
                Call Me.UpdateFilteredShips()
                tabHQF.SelectedTab = tabHQF.Tabs(NewFit.KeyName)
                If tabHQF.SelectedTabIndex = 0 Then Call Me.UpdateSelectedTab()
                ActiveFitting.ShipSlotCtrl.UpdateEverything()
            End If
        Else
            MessageBox.Show("Unable to Create New Fitting!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub mnuPreviewShip2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPreviewShip2.Click
        Dim curNode As Node = tvwFittings.SelectedNodes(0)
        Dim shipName As String = mnuFittingsFittingName.Tag.ToString
        Dim selShip As Ship = CType(ShipLists.shipList(shipName), Ship)
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        If ActiveFitting IsNot Nothing Then
            hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), Core.Pilot)
        Else
            If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot)
            Else
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot)
            End If
        End If
        showInfo.ShowItemDetails(selShip, hPilot)
    End Sub
    Private Sub ShowFitting(ByVal fittingNode As Node)
        ' Check we have some valid characters
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 Then
            ' Get the ship details
            If fittingNode.Parent IsNot Nothing Then
                Dim ShipName As String = fittingNode.Parent.Text
                Dim FitName As String = fittingNode.Text
                Dim FitKey As String = fittingNode.Parent.Text & ", " & fittingNode.Text

                If Me.OpenFittingsContains(FitKey) = False Then
                    If Fittings.FittingList.ContainsKey(FitKey) = True Then
                        Dim newfit As Fitting = Fittings.FittingList(FitKey)
                        Call Me.CreateNewFittingTab(newfit)
                        newfit.ShipSlotCtrl.UpdateEverything()

                        ' Set the newly opened fitting
                        ' NB: Doesn't trigger the event if this is the first tab open
                        ActiveFitting = newfit
                        tabHQF.SelectedTab = tabHQF.Tabs(FitKey)
                    Else
                        MessageBox.Show("Can't load the '" & FitKey & "' fitting as it's not there!!", "Error locating fitting details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    tabHQF.SelectedTab = tabHQF.Tabs(FitKey)
                End If
            End If
            ' Check for user columns only being the default "Module Name"
            Dim colCount As Integer = 0
            For Each UserCol As UserSlotColumn In HQF.Settings.HQFSettings.UserSlotColumns
                If UserCol.Active = True Then
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
                    Dim mySettings As New frmHQFSettings
                    mySettings.Tag = "nodeSlotFormat"
                    mySettings.ShowDialog()
                    mySettings = Nothing
                    Call Me.UpdateFittingsTree(False)
                End If
            End If
        Else
            Dim msg As String = "There are no pilots or accounts created in EveHQ." & ControlChars.CrLf
            msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
            MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Public Sub RemoteShowFitting(ByVal FitKey As String)
        If Fittings.FittingList.ContainsKey(FitKey) = True Then
            ' Create the tab and display
            If Me.OpenFittingsContains(FitKey) = False Then
                Dim newfit As Fitting = Fittings.FittingList(FitKey)
                Call Me.CreateNewFittingTab(newfit)
                newfit.ShipSlotCtrl.UpdateEverything()
            End If
            tabHQF.SelectedTab = tabHQF.Tabs(FitKey)
        End If
    End Sub
    Private Sub mnuFittingsBCBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsBCBrowser.Click
        Dim curNode As Node = tvwFittings.SelectedNodes(0)
        Dim shipName As String = mnuFittingsFittingName.Tag.ToString
        Dim bShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
        If myBCBrowser.IsHandleCreated = True Then
            myBCBrowser.ShipType = bShip
            myBCBrowser.BringToFront()
        Else
            myBCBrowser = New frmBCBrowser
            myBCBrowser.ShipType = bShip
            myBCBrowser.Show()
        End If
    End Sub
    Private Sub mnuExportToEve_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportToEve.Click
        Call Me.ExportFittingsToEve()
	End Sub
	Private Sub mnuExportToRequisitions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportToRequisitions.Click
		Call Me.ExportRequisitions()
	End Sub
    Private Sub tvwfittings_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvwFittings.MouseDoubleClick
        If tvwFittings.SelectedNodes.Count > 0 Then
            If tvwFittings.SelectedNodes(0).Nodes.Count = 0 Then
                Call Me.ShowFitting(tvwFittings.SelectedNodes(0))
            End If
        End If
    End Sub

    Private Sub tvwFittings_SelecttionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tvwFittings.SelectionChanged
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

    Private Sub mnuCompareFittings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCompareFittings.Click
        Call Me.CompareShips()
    End Sub

    Private Sub CompareShips()
        ' Establish which fittings we will be comparing
        Dim Fittings As New SortedList
        For Each fitting As Node In tvwFittings.SelectedNodes
            If fitting.Nodes.Count = 0 Then
                ' If we have highlighted an item
                If Fittings.Contains(fitting.Parent.Text & ", " & fitting.Text) = False Then
                    Fittings.Add(fitting.Parent.Text & ", " & fitting.Text, "")
                End If
            Else
                ' If we have highlighted a group
                For Each subFit As Node In fitting.Nodes
                    If Fittings.Contains(subFit.Parent.Text & ", " & subFit.Text) = False Then
                        Fittings.Add(subFit.Parent.Text & ", " & subFit.Text, "")
                    End If
                Next
            End If
        Next
        Dim CompareShips As New frmShipComparison
        CompareShips.ShipList = Fittings
        CompareShips.ShowDialog()
        CompareShips.Dispose()
    End Sub
#End Region

#Region "Module List Context Menu Routines"

    Private Sub ctxModuleList_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxModuleList.Opening
        If tvwModules.SelectedNodes.Count > 0 Then
            Dim moduleID As String = tvwModules.SelectedNodes(0).Name
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
        Dim moduleID As String = tvwModules.SelectedNodes(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim showInfo As New frmShowInfo
        Dim hPilot As EveHQ.Core.Pilot
        If ActiveFitting IsNot Nothing Then
            If ActiveFitting.ShipInfoCtrl IsNot Nothing Then
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem), Core.Pilot)
            Else
                If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                    hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot)
                Else
                    hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot)
                End If
            End If
        Else
            If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot)
            Else
                hPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot)
            End If
        End If
        showInfo.ShowItemDetails(cModule, hPilot)
    End Sub

    Private Sub mnuAddToFavourites_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToFavourites_List.Click
        Dim moduleID As String = tvwModules.SelectedNodes(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        If Settings.HQFSettings.Favourites.Contains(cModule.Name) = False Then
            Settings.HQFSettings.Favourites.Add(cModule.Name)
        End If
    End Sub

    Private Sub mnuRemoveFromFavourites_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveFromFavourites.Click
        Dim moduleID As String = tvwModules.SelectedNodes(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        If Settings.HQFSettings.Favourites.Contains(cModule.Name) = True Then
            Settings.HQFSettings.Favourites.Remove(cModule.Name)
        End If
        Call CalculateFilteredModules(tvwItems.SelectedNode)
    End Sub

    Private Sub mnuShowModuleMarketGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowModuleMarketGroup.Click
        Dim moduleID As String = tvwModules.SelectedNodes(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim pathLine As String = CStr(Market.MarketGroupPath(cModule.MarketGroup))
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
        Dim mySettings As New frmHQFSettings
        mySettings.ShowDialog()
        mySettings = Nothing
        Call Me.UpdateFittingsTree(False)
    End Sub

#End Region

#Region "Export to Eve Routines"

    Private Sub ExportFittingsToEve()
        Dim Fittings As ArrayList = Me.GetExportFittingsCollection()
        Dim myEveExport As New frmEveExport
		myEveExport.FittingList = Fittings
		myEveExport.UpdateRequired = True
        myEveExport.ShowDialog()
        myEveExport.Dispose()
    End Sub

    Private Sub ExportMainFittingToEve()
        Dim Fittings As New ArrayList
        Fittings.Add(ActiveFitting.KeyName)
        Dim myEveExport As New frmEveExport
		myEveExport.FittingList = Fittings
        myEveExport.UpdateRequired = True
        myEveExport.ShowDialog()
        myEveExport.Dispose()
    End Sub

    Private Function GetExportFittingsCollection() As ArrayList
        Dim Fittings As New ArrayList
        For Each fitting As Node In tvwFittings.SelectedNodes
            If fitting.Nodes.Count = 0 Then
                ' If we have highlighted an item
                If Fittings.Contains(fitting.Parent.Text & ", " & fitting.Text) = False Then
                    Fittings.Add(fitting.Parent.Text & ", " & fitting.Text)
                End If
            Else
                ' If we have highlighted a group
                For Each subFit As Node In fitting.Nodes
                    If Fittings.Contains(subFit.Parent.Text & ", " & subFit.Text) = False Then
                        Fittings.Add(subFit.Parent.Text & ", " & subFit.Text)
                    End If
                Next
            End If
        Next
        Fittings.Sort()
        Return Fittings
    End Function

#End Region

#Region "Export To Requisitions Routines"

	Private Sub ExportRequisitions()
		' Establish which fittings we will be comparing
		Dim ShipFits As New SortedList
		For Each fitting As Node In tvwFittings.SelectedNodes
			If fitting.Nodes.Count = 0 Then
				' If we have highlighted an item
				If ShipFits.Contains(fitting.Parent.Text & ", " & fitting.Text) = False Then
					ShipFits.Add(fitting.Parent.Text & ", " & fitting.Text, "")
				End If
			Else
				' If we have highlighted a group
				For Each subFit As Node In fitting.Nodes
					If ShipFits.Contains(subFit.Parent.Text & ", " & subFit.Text) = False Then
						ShipFits.Add(subFit.Parent.Text & ", " & subFit.Text, "")
					End If
				Next
			End If
		Next

		If ShipFits.Count > 0 Then

			' Set up a new Sortedlist to store the required items
			Dim Orders As New SortedList(Of String, Integer)

			For Each ShipFit As String In ShipFits.Keys
				Dim currentFit As Fitting = Fittings.FittingList.Item(ShipFit)
				currentFit.UpdateBaseShipFromFitting()

				' Collect the orders
				CollectModulesForExport(Orders, currentFit)
				' Add the current ship
				If Orders.ContainsKey(currentFit.BaseShip.Name) = False Then
					Orders.Add(currentFit.BaseShip.Name, 1)
				Else
					Orders(currentFit.BaseShip.Name) += 1
				End If

			Next

			Dim newReq As New EveHQ.Core.frmAddRequisition("HQF", Orders)
			newReq.ShowDialog()

		End If
	End Sub

#End Region

#Region "Meta Variations Code"

    Private Sub mnuShowMetaVariations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowMetaVariations.Click
        Dim moduleID As String = tvwModules.SelectedNodes(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim newComparison As New frmMetaVariations(ActiveFitting, cModule)
        newComparison.Size = HQF.Settings.HQFSettings.MetaVariationsFormSize
		newComparison.ShowDialog()
    End Sub

#End Region

    Private Sub tabHQF_TabRemoved(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabHQF.TabRemoved

        ' Get name of closed tab - DNB refuses to expose this information to us
        Dim ClosedTabName As String = ""
        Dim tp As New DevComponents.DotNetBar.TabItem
        For Each fitting As String In ShipLists.fittedShipList.Keys
            tp = tabHQF.Tabs(fitting)
            If tp Is Nothing Then
                ClosedTabName = fitting
                Exit For
            End If
        Next
        If ClosedTabName <> "" Then
            ' Remove data
            ShipLists.fittedShipList.Remove(ClosedTabName)
            ActiveFitting.ShipInfoCtrl = Nothing
            ActiveFitting.ShipSlotCtrl = Nothing
        End If
        ' Check for last fitting closure to remove the Active Fitting
        If tabHQF.Tabs.Count = 0 Then
            ActiveFitting = Nothing
        End If
    End Sub

#Region "HQF Ribbon UI Functions"


#End Region

    Private Sub btnOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptions.Click
        Call Me.OpenSettingsForm()
    End Sub

    Private Sub btnPilotManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPilotManager.Click
        Call Me.OpenPilotManagerForm(0)
    End Sub

    Private Sub btnImplantManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImplantManager.Click
        Call Me.OpenPilotManagerForm(2)
    End Sub

    Private Sub OpenPilotManagerForm(ByVal TabIndex As Integer)
        If myPilotManager.IsHandleCreated = False Then
            myPilotManager = New frmPilotManager
            If ActiveFitting IsNot Nothing Then
                myPilotManager.pilotName = ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString
            Else
                If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                    myPilotManager.pilotName = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.EveHQSettings.StartupPilot), Core.Pilot).Name
                Else
                    myPilotManager.pilotName = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(1), Core.Pilot).Name
                End If
            End If
            myPilotManager.Show()
            myPilotManager.tabControlPM.SelectedTabIndex = TabIndex
        Else
            If myPilotManager.WindowState = FormWindowState.Minimized Then
                myPilotManager.WindowState = FormWindowState.Normal
            End If
            myPilotManager.Show()
            myPilotManager.BringToFront()
        End If
    End Sub

    Private Sub btnScreenGrab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScreenGrab.Click
        ' Determine co-ords of current main panel
        Try
            Dim xy As Point = tabHQF.PointToScreen(New Point(0, 0))
            Dim sx As Integer = xy.X
            Dim sy As Integer = xy.Y
            Dim fittingImage As Bitmap = ScreenGrab.GrabScreen(New Rectangle(sx, sy, tabHQF.Width, tabHQF.Height))
            Clipboard.SetDataObject(fittingImage)
            Dim rgPattern As String = "[\\\/:\*\?""'<>|]"
            Dim objRegEx As New System.Text.RegularExpressions.Regex(rgPattern)
            Dim fittingName As String = objRegEx.Replace(ActiveFitting.KeyName, "_")
            Dim filename As String = "HQF_" & fittingName & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".png"
            fittingImage.Save(Path.Combine(EveHQ.Core.HQ.reportFolder, filename), System.Drawing.Imaging.ImageFormat.Png)
        Catch ex As Exception
            MessageBox.Show("There was an error taking a screenshot of the current fitting. The error was: " & ControlChars.CrLf & ControlChars.CrLf & ex.Message, "Error Taking Screenshot", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub btnExportEve_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportEve.Click
        If ActiveFitting Is Nothing Then
            Dim msg As String = "Please make sure you have a fit open and active before exporting to Eve."
            MessageBox.Show(msg, "Open Fitting Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Call Me.ExportMainFittingToEve()
        End If
    End Sub

    Private Sub btnExportHQF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportHQF.Click
		Dim currentShip As Ship = ActiveFitting.BaseShip
        Dim fittedShip As Ship = ActiveFitting.FittedShip
        Dim cModule As New ShipModule
        Dim state As Integer
        Dim fitting As New System.Text.StringBuilder
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

    Private Sub btnExportEFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportEFT.Click
		Dim currentship As Ship = ActiveFitting.FittedShip
        Dim cModule As New ShipModule
        Dim fitting As New System.Text.StringBuilder
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

    Private Sub btnExportForums_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportForums.Click
		Dim currentship As Ship = ActiveFitting.FittedShip
        Dim slots As Dictionary(Of String, Integer)
        Dim slotList As New ArrayList
        Dim slotCount As Integer = 0
        Dim cModule As New ShipModule
        Dim state As Integer
        Dim fitting As New System.Text.StringBuilder
		fitting.AppendLine("[" & ActiveFitting.KeyName & "]")

        slots = New Dictionary(Of String, Integer)
        For slot As Integer = 1 To currentship.SubSlots
            If currentship.SubSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.SubSlot(slot).ModuleState) / Math.Log(2))
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
                fitting.AppendLine(cargo.Quantity & "x " & cargo.ItemType.Name)
            Next
        End If
        Try
            Clipboard.SetText(fitting.ToString)
        Catch ex As Exception
            MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy For Forums Error")
        End Try
    End Sub

    Private Sub btnExportStats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportStats.Click
        Dim currentship As Ship = ActiveFitting.FittedShip
        Dim stats As New System.Text.StringBuilder
        stats.AppendLine("[Statistics - " & ActiveFitting.ShipInfoCtrl.cboPilots.SelectedItem.ToString & "]")
        stats.AppendLine("")
        stats.AppendLine(ActiveFitting.ShipInfoCtrl.lblEffectiveHP.Text)
        stats.AppendLine(ActiveFitting.ShipInfoCtrl.lblTankAbility.Text)
        stats.AppendLine("Damage Profile - " & currentship.DamageProfile.Name & " (EM: " & FormatNumber(currentship.DamageProfileEM * 100, 2) & "%, Ex: " & FormatNumber(currentship.DamageProfileEX * 100, 2) & "%, Ki: " & FormatNumber(currentship.DamageProfileKI * 100, 2) & "%, Th: " & FormatNumber(currentship.DamageProfileTH * 100, 2) & "%)")
        stats.AppendLine("Shield Resists - EM: " & ActiveFitting.ShipInfoCtrl.lblShieldEM.Text & ", Ex: " & ActiveFitting.ShipInfoCtrl.lblShieldExplosive.Text & ", Ki: " & ActiveFitting.ShipInfoCtrl.lblShieldKinetic.Text & ", Th: " & ActiveFitting.ShipInfoCtrl.lblShieldThermal.Text)
        stats.AppendLine("Armor Resists - EM: " & ActiveFitting.ShipInfoCtrl.lblArmorEM.Text & ", Ex: " & ActiveFitting.ShipInfoCtrl.lblArmorExplosive.Text & ", Ki: " & ActiveFitting.ShipInfoCtrl.lblArmorKinetic.Text & ", Th: " & ActiveFitting.ShipInfoCtrl.lblArmorThermal.Text)
        stats.AppendLine("")
        stats.AppendLine(ActiveFitting.ShipInfoCtrl.epCapacitor.TitleText)
        stats.AppendLine("")
        stats.AppendLine("Volley Damage: " & FormatNumber(currentship.TotalVolley, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        stats.AppendLine("DPS: " & FormatNumber(currentship.TotalDPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        Try
            Clipboard.SetText(stats.ToString)
        Catch ex As Exception
            MessageBox.Show("There was an error writing data to the clipboard. Please wait a couple of seconds and try again.", "Copy Stats Error")
        End Try
    End Sub

    Private Sub btnExportImplants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportImplants.Click
        Dim implantSetName As String = ActiveFitting.ShipInfoCtrl.cboImplants.SelectedItem.ToString
        If HQF.Settings.HQFSettings.ImplantGroups.ContainsKey(implantSetName) = True Then
            Dim implantSet As ImplantGroup = CType(HQF.Settings.HQFSettings.ImplantGroups(implantSetName), ImplantGroup)
            Dim stats As New System.Text.StringBuilder
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

    Private Sub btnExportReq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportReq.Click
        ' Set up a new Sortedlist to store the required items
		Dim Orders As New SortedList(Of String, Integer)
		CollectModulesForExport(Orders, ActiveFitting)
        ' Add the current ship
        Orders.Add(ActiveFitting.BaseShip.Name, 1)
        ' Setup the Requisition form for HQF and open it
        Dim newReq As New EveHQ.Core.frmAddRequisition("HQF", Orders)
        newReq.ShowDialog()
    End Sub

	Private Function CollectModulesForExport(ByRef ModList As SortedList(Of String, Integer), ByVal ShipFitting As Fitting) As SortedList(Of String, Integer)

		Dim currentship As Ship = ShipFitting.BaseShip

		' Parse HiSlots
		For slot As Integer = 1 To currentship.HiSlots
			If currentship.HiSlot(slot) IsNot Nothing Then
				If ModList.ContainsKey(currentship.HiSlot(slot).Name) = True Then
					ModList(currentship.HiSlot(slot).Name) += 1
				Else
					ModList.Add(currentship.HiSlot(slot).Name, 1)
				End If
			End If
		Next

		' Parse MidSlots
		For slot As Integer = 1 To currentship.MidSlots
			If currentship.MidSlot(slot) IsNot Nothing Then
				If ModList.ContainsKey(currentship.MidSlot(slot).Name) = True Then
					ModList(currentship.MidSlot(slot).Name) += 1
				Else
					ModList.Add(currentship.MidSlot(slot).Name, 1)
				End If
			End If
		Next

		' Parse LowSlots
		For slot As Integer = 1 To currentship.LowSlots
			If currentship.LowSlot(slot) IsNot Nothing Then
				If ModList.ContainsKey(currentship.LowSlot(slot).Name) = True Then
					ModList(currentship.LowSlot(slot).Name) += 1
				Else
					ModList.Add(currentship.LowSlot(slot).Name, 1)
				End If
			End If
		Next

		' Parse RigSlots
		For slot As Integer = 1 To currentship.RigSlots
			If currentship.RigSlot(slot) IsNot Nothing Then
				If ModList.ContainsKey(currentship.RigSlot(slot).Name) = True Then
					ModList(currentship.RigSlot(slot).Name) += 1
				Else
					ModList.Add(currentship.RigSlot(slot).Name, 1)
				End If
			End If
		Next

		' Parse subslots
		For slot As Integer = 1 To currentship.SubSlots
			If currentship.SubSlot(slot) IsNot Nothing Then
				If ModList.ContainsKey(currentship.SubSlot(slot).Name) = True Then
					ModList(currentship.SubSlot(slot).Name) += 1
				Else
					ModList.Add(currentship.SubSlot(slot).Name, 1)
				End If
			End If
		Next

		' Parse drones
		If currentship.DroneBayItems.Count > 0 Then
			For Each drone As DroneBayItem In currentship.DroneBayItems.Values
				If ModList.ContainsKey(drone.DroneType.Name) = True Then
					ModList(drone.DroneType.Name) += drone.Quantity
				Else
					ModList.Add(drone.DroneType.Name, drone.Quantity)
				End If
			Next
		End If

		' Send list back to the caller
		Return ModList

	End Function

    Private Sub btnImportEve_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportEve.Click
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count = 0 Then
            Dim msg As String = "There are no pilots or accounts created in EveHQ." & ControlChars.CrLf
            msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
            MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If myEveImport.IsHandleCreated = True Then
                myEveImport.BringToFront()
            Else
                myEveImport = New frmEveImport
                myEveImport.Show()
            End If
        End If
    End Sub

    Private Sub btnImportEFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportEFT.Click
        Dim myEFTImport As New frmEFTImport
        myEFTImport.ShowDialog()
        myEFTImport = Nothing
        Call Me.UpdateFittingsTree(False)
    End Sub

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
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
            ShipMod = ShipMod.Trim
            If ShipMod.StartsWith("[") = False And ShipMod <> "" Then
                ' Check for "Drones_" label
                If ShipMod.StartsWith("Drones_") Then
                    ShipMod = ShipMod.TrimStart("Drones_Active=".ToCharArray)
                    ShipMod = ShipMod.TrimStart("Drones_Inactive=".ToCharArray)
                End If
                ' Check for forum format (to shut the fucking whiners up)
                Dim ModuleMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(ShipMod, "(?<quantity>\d)*x\s(?<module>.+)\s\((?<charge>.+)\)|(?<quantity>\d)*x\s(?<module>.+)")
                If ModuleMatch.Success = True Then
                    Dim ModName As String = ModuleMatch.Groups.Item("module").Value
                    Dim ChargeName As String = ModuleMatch.Groups.Item("charge").Value
                    Dim Quantity As String = ModuleMatch.Groups.Item("quantity").Value
                    If IsNumeric(Quantity) = True Then
                        If ModuleLists.moduleListName.ContainsKey(ModName) = True Then
                            Dim TestMod As ShipModule = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(ModName)), ShipModule).Clone
                            If TestMod.IsDrone = True Then
                                newFit.Add(ModName & ", " & Quantity)
                            Else
                                For ModCount As Integer = 1 To CInt(Quantity)
                                    If ChargeName <> "" Then
                                        newFit.Add(ModName & ", " & ChargeName)
                                    Else
                                        newFit.Add(ModName)
                                    End If
                                Next
                            End If
                        End If
                    End If
                Else
                    Dim DroneMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(ShipMod, "(?<module>.+)\sx(?<quantity>\d+)|(?<module>.+)")
                    If DroneMatch.Success = True Then
                        Dim ModName As String = DroneMatch.Groups.Item("module").Value
                        Dim Quantity As String = DroneMatch.Groups.Item("quantity").Value
                        If IsNumeric(Quantity) = True Then
                            If ModuleLists.moduleListName.ContainsKey(ModName) = True Then
                                Dim TestMod As ShipModule = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(ModName)), ShipModule).Clone
                                If TestMod.IsDrone = True Then
                                    newFit.Add(ModName & ", " & Quantity)
                                Else
                                    For ModCount As Integer = 1 To CInt(Quantity)
                                        newFit.Add(ModName)
                                    Next
                                End If
                            End If
                        Else
                            newFit.Add(ShipMod)
                        End If
                    Else
                        newFit.Add(ShipMod)
                    End If
                End If
            End If
        Next
        Dim NewFitting As Fitting = Fittings.ConvertOldFitToNewFit(shipName & ", " & fittingName, newFit)
        Fittings.FittingList.Add(NewFitting.KeyName, NewFitting)
        Call Me.UpdateFittingsTree(False)
    End Sub

    Private Sub btnEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditor.Click
        Dim newShipEditor As New frmShipEditor
        newShipEditor.ShowDialog()
    End Sub

    Private Function OpenFittingsContains(ByVal FitKey As String) As Boolean
        For Each FitTab As DevComponents.DotNetBar.TabItem In tabHQF.Tabs
            If FitTab.Text = FitKey Then
                Return True
            End If
        Next
        Return False
    End Function

#Region "Undo & Redo Code"

    Private Sub frmHQF_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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