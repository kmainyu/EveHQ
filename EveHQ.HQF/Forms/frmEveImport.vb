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

Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml
Imports DevComponents.AdvTree
Imports EveHQ.Core
Imports SearchOption = Microsoft.VisualBasic.FileIO.SearchOption

Namespace Forms

    Public Class FrmEveImport

        ReadOnly _eveFolder As String = Path.Combine(Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Eve"), "fittings")
        Dim _currentShip As Ship
        Dim _currentFit As New ArrayList
        Dim _currentFitName As String = ""
        Dim _currentFitting As Fitting

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add the current list of pilots to the combobox
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    cboPilots.Items.Add(cPilot.Name)
                End If
            Next
            cboPilots.EndUpdate()
            ' Look at the settings for default pilot
            If cboPilots.Items.Count > 0 Then
                If cboPilots.Items.Contains(PluginSettings.HQFSettings.DefaultPilot) = True Then
                    cboPilots.SelectedItem = PluginSettings.HQFSettings.DefaultPilot
                Else
                    cboPilots.SelectedIndex = 0
                End If
            End If

            ' Add the profiles
            cboProfiles.BeginUpdate()
            cboProfiles.Items.Clear()
            For Each newProfile As HQFDamageProfile In HQFDamageProfiles.ProfileList.Values
                cboProfiles.Items.Add(newProfile.Name)
            Next
            cboProfiles.EndUpdate()
            ' Select the default profile
            cboProfiles.SelectedItem = "<Omni-Damage>"

        End Sub

        Private Sub frmEveImport_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Call GetEveFittings()
        End Sub

#Region "Ship Fitting routines"

        Private Sub UpdateSlotColumns()
            ' Clear the columns
            lvwSlots.Columns.Clear()
            ' Add the module name column
            lvwSlots.Columns.Add("colName", "Module Name", 350, HorizontalAlignment.Left, "")
        End Sub
        Private Sub UpdateSlotLayout()
            If _currentShip IsNot Nothing Then
                lvwSlots.BeginUpdate()
                lvwSlots.Items.Clear()
                ' Produce high slots
                For slot As Integer = 1 To _currentShip.HiSlots
                    Dim newSlot As New ListViewItem
                    newSlot.Name = "8_" & slot
                    newSlot.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.HiSlotColour))
                    newSlot.ForeColor = Color.Black
                    newSlot.Group = lvwSlots.Groups.Item("lvwgHighSlots")
                    Call AddUserColumns(_currentShip.HiSlot(slot), newSlot)
                    lvwSlots.Items.Add(newSlot)
                Next
                For slot As Integer = 1 To _currentShip.MidSlots
                    Dim newSlot As New ListViewItem
                    newSlot.Name = "4_" & slot
                    newSlot.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.MidSlotColour))
                    newSlot.ForeColor = Color.Black
                    newSlot.Group = lvwSlots.Groups.Item("lvwgMidSlots")
                    Call AddUserColumns(_currentShip.MidSlot(slot), newSlot)
                    lvwSlots.Items.Add(newSlot)
                Next
                For slot As Integer = 1 To _currentShip.LowSlots
                    Dim newSlot As New ListViewItem
                    newSlot.Name = "2_" & slot
                    newSlot.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.LowSlotColour))
                    newSlot.ForeColor = Color.Black
                    newSlot.Group = lvwSlots.Groups.Item("lvwgLowSlots")
                    Call AddUserColumns(_currentShip.LowSlot(slot), newSlot)
                    lvwSlots.Items.Add(newSlot)
                Next
                For slot As Integer = 1 To _currentShip.RigSlots
                    Dim newSlot As New ListViewItem
                    newSlot.Name = "1_" & slot
                    newSlot.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.RigSlotColour))
                    newSlot.ForeColor = Color.Black
                    newSlot.Group = lvwSlots.Groups.Item("lvwgRigSlots")
                    Call AddUserColumns(_currentShip.RigSlot(slot), newSlot)
                    lvwSlots.Items.Add(newSlot)
                Next
                ' Produce sub slots
                For slot As Integer = 1 To _currentShip.SubSlots
                    Dim newSlot As New ListViewItem
                    newSlot.Name = "16_" & slot
                    newSlot.BackColor = Color.FromArgb(CInt(PluginSettings.HQFSettings.SubSlotColour))
                    newSlot.ForeColor = Color.Black
                    newSlot.Group = lvwSlots.Groups.Item("lvwgSubSlots")
                    Call AddUserColumns(_currentShip.SubSlot(slot), newSlot)
                    lvwSlots.Items.Add(newSlot)
                Next
                lvwSlots.EndUpdate()
            End If
        End Sub
        Private Sub AddUserColumns(ByVal shipMod As ShipModule, ByVal slotName As ListViewItem)
            ' Add subitems based on the user selected columns
            If shipMod IsNot Nothing Then
                ' Add in the module name
                slotName.Text = shipMod.Name
            Else
                slotName.Text = "<Empty>"
            End If
        End Sub
        Private Sub ClearShipSlots()
            If _currentShip IsNot Nothing Then
                For slot As Integer = 1 To _currentShip.HiSlots
                    _currentShip.HiSlot(slot) = Nothing
                Next
                For slot As Integer = 1 To _currentShip.MidSlots
                    _currentShip.MidSlot(slot) = Nothing
                Next
                For slot As Integer = 1 To _currentShip.LowSlots
                    _currentShip.LowSlot(slot) = Nothing
                Next
                For slot As Integer = 1 To _currentShip.RigSlots
                    _currentShip.RigSlot(slot) = Nothing
                Next
                For slot As Integer = 1 To _currentShip.SubSlots
                    _currentShip.SubSlot(slot) = Nothing
                Next
                _currentShip.DroneBayItems.Clear()
                _currentShip.DroneBayUsed = 0
                _currentShip.CargoBayItems.Clear()
                _currentShip.CargoBayUsed = 0
            End If
        End Sub
        Private Sub GenerateFittingData()
            ' Let's try and generate a fitting and get some damage info
            If _currentShip IsNot Nothing Then
                Dim loadoutPilot As FittingPilot = FittingPilots.HQFPilots(cboPilots.SelectedItem.ToString)
                Dim loadoutProfile As HQFDamageProfile = HQFDamageProfiles.ProfileList(cboProfiles.SelectedItem.ToString)

                _currentFitting.PilotName = loadoutPilot.PilotName
                _currentFitting.BaseShip.DamageProfile = loadoutProfile
                _currentFitting.ApplyFitting()
                Dim loadoutShip As Ship = _currentFitting.FittedShip

                lblEHP.Text = loadoutShip.EffectiveHP.ToString("N0")
                lblTank.Text = loadoutShip.Attributes(10062).ToString("N2") & " DPS"
                lblVolley.Text = loadoutShip.Attributes(10028).ToString("N2")
                lblDPS.Text = loadoutShip.Attributes(10029).ToString("N2")
                lblShieldResists.Text = loadoutShip.ShieldEMResist.ToString("N0") & "/" & loadoutShip.ShieldExResist.ToString("N0") & "/" & loadoutShip.ShieldKiResist.ToString("N0") & "/" & loadoutShip.ShieldThResist.ToString("N0")
                lblArmorResists.Text = loadoutShip.ArmorEMResist.ToString("N0") & "/" & loadoutShip.ArmorExResist.ToString("N0") & "/" & loadoutShip.ArmorKiResist.ToString("N0") & "/" & loadoutShip.ArmorThResist.ToString("N0")
                Dim csr As CapSimResults = Capacitor.CalculateCapStatistics(loadoutShip, False)
                If csr.CapIsDrained = False Then
                    lblCapacitor.Text = "Stable at " & (csr.MinimumCap / loadoutShip.CapCapacity * 100).ToString("N0") & "%"
                Else
                    lblCapacitor.Text = "Lasts " & SkillFunctions.TimeToString(csr.TimeToDrain, False)
                End If
                lblVelocity.Text = loadoutShip.MaxVelocity.ToString("N2") & " m/s"
                lblMaxRange.Text = loadoutShip.MaxTargetRange.ToString("N0") & "m"
                Dim cpu As Double = loadoutShip.CPUUsed / loadoutShip.CPU * 100
                lblCPU.Text = cpu.ToString("N2") & "%"
                If cpu > 100 Then
                    lblCPU.ForeColor = Color.Red
                Else
                    lblCPU.ForeColor = Color.Black
                End If
                Dim pg As Double = loadoutShip.PGUsed / loadoutShip.PG * 100
                lblPG.Text = pg.ToString("N2") & "%"
                If pg > 100 Then
                    lblPG.ForeColor = Color.Red
                Else
                    lblPG.ForeColor = Color.Black
                End If

                Dim maxOpt As Double = 0
                For slot As Integer = 1 To loadoutShip.HiSlots
                    Dim shipMod As ShipModule = loadoutShip.HiSlot(slot)
                    If shipMod IsNot Nothing Then
                        If shipMod.Attributes.ContainsKey(54) Then
                            maxOpt = Math.Max(maxOpt, CDbl(shipMod.Attributes(54)))
                        End If
                    End If
                Next
                lblOptimalRange.Text = maxOpt.ToString("N0") & "m"

            End If

        End Sub

        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilots.SelectedIndexChanged
            Call GenerateFittingData()
        End Sub

        Private Sub cboProfiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProfiles.SelectedIndexChanged
            Call GenerateFittingData()
        End Sub

        Private Sub btnImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImport.Click
            If _currentShip IsNot Nothing Then
                Dim shipName As String = _currentShip.Name
                Dim fittingName As String = _currentFitName
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
                Dim newFit As Fitting = Fittings.ConvertOldFitToNewFit(shipName & ", " & fittingName, _currentFit)
                Fittings.FittingList.Add(newFit.KeyName, newFit)
                HQFEvents.StartUpdateFittingList = True
            Else
                MessageBox.Show("Please select a fitting before importing.", "Fitting Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

#End Region

#Region "UI Routines"

        Private Sub mnuViewLoadout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewLoadout.Click
            Dim cLoadout As Node = adtLoadOuts.SelectedNodes(0)
            Call GetEveShipLoadout(cLoadout)
        End Sub

        Private Sub adtLoadOuts_NodeClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtLoadOuts.NodeClick
            If adtLoadOuts.SelectedNodes.Count > 0 Then
                If adtLoadOuts.SelectedNodes(0).Nodes.Count = 0 Then
                    Dim cLoadout As Node = adtLoadOuts.SelectedNodes(0)
                    Call GetEveShipLoadout(cLoadout)
                End If
            End If
        End Sub

        Private Sub adtLoadOuts_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles adtLoadOuts.SelectionChanged
            If adtLoadOuts.SelectedNodes.Count = 1 Then
                If adtLoadOuts.SelectedNodes(0).Tag IsNot Nothing Then
                    lblEveImportStatus.Text = adtLoadOuts.SelectedNodes(0).Text & ": " & adtLoadOuts.SelectedNodes(0).Tag.ToString
                Else
                    lblEveImportStatus.Text = ""
                End If
            End If
        End Sub

#End Region

#Region "Eve Import Routines"

        Private Sub GetEveFittings()

            ' Check for the fittings directory and create it
            If My.Computer.FileSystem.DirectoryExists(_eveFolder) = False Then
                MessageBox.Show("The Eve fittings folder is not present on your system and is required for this feature to work. You will need some fittings present in this folder either exported from Eve or EveHQ before proceeding.", "Fittings Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Close()
                Exit Sub
            End If

            Dim files As New ArrayList
            Dim ships As New SortedList
            Dim shipFittings As SortedList
            For Each fileName As String In My.Computer.FileSystem.GetFiles(_eveFolder, SearchOption.SearchTopLevelOnly, "*.xml")
                files.Add(fileName)
            Next
            Dim fitXML As New XmlDocument
            Dim fittingList As XmlNodeList
            Dim shipNode As XmlNode
            Dim shipItem As Node
            Dim fitItem As Node
            Dim shipName As String
            Dim shipFit As String
            adtLoadOuts.BeginUpdate()
            adtLoadOuts.Nodes.Clear()
            For Each filename As String In files
                Try
                    fitXML.Load(filename)
                    fittingList = fitXML.SelectNodes("/fittings/fitting")
                    For Each fitNode As XmlNode In fittingList
                        shipNode = fitNode.SelectSingleNode("shipType")
                        shipName = shipNode.Attributes("value").Value.ToString
                        shipFit = fitNode.Attributes("name").Value.ToString
                        If ships.ContainsKey(shipName) = False Then
                            shipFittings = New SortedList
                            shipFittings.Add(shipFit, filename)
                            ships.Add(shipName, shipFittings)
                        Else
                            shipFittings = CType(ships(shipName), SortedList)
                            If shipFittings.ContainsKey(shipFit) = False Then
                                shipFittings.Add(shipFit, filename)
                            End If
                        End If
                    Next
                Catch e As Exception
                End Try
            Next
            For Each ship As String In ships.Keys
                shipItem = New Node
                shipItem.Text = ship
                adtLoadOuts.Nodes.Add(shipItem)
                shipFittings = CType(ships(ship), SortedList)
                For Each fit As String In shipFittings.Keys
                    fitItem = New Node
                    fitItem.Text = fit
                    fitItem.Tag = CStr(shipFittings(fit))
                    shipItem.Nodes.Add(fitItem)
                Next
            Next
            adtLoadOuts.EndUpdate()
        End Sub

        Private Sub GetEveShipLoadout(ByVal cLoadout As Node)
            Dim fitName As String = cLoadout.Text
            Dim fileName As String = cLoadout.Tag.ToString
            Dim shipName As String = cLoadout.Parent.Text
            _currentShip = ShipLists.ShipList(shipName).Clone
            Dim moduleList As New SortedList
            Dim droneList As New SortedList
            Dim cargoList As New SortedList
            ' Open the file and load the XML
            Dim fitXML As New XmlDocument
            fitXML.Load(fileName)
            Dim fittingList As XmlNodeList = fitXML.SelectNodes("/fittings/fitting")
            Dim subCount As Integer = 0
            For Each fitNode As XmlNode In fittingList
                If fitNode.Attributes("name").Value = fitName And fitNode.SelectSingleNode("shipType").Attributes("value").Value = shipName Then
                    Dim modNodes As XmlNodeList = fitNode.SelectNodes("hardware")
                    For Each modNode As XmlNode In modNodes
                        If ModuleLists.ModuleListName.ContainsKey(modNode.Attributes("type").Value.Trim) Then
                            Dim fModule As ShipModule = ModuleLists.ModuleList(ModuleLists.ModuleListName(modNode.Attributes("type").Value.Trim))
                            If modNode.Attributes("slot").Value <> "subsystem slot 0" Then
                                If modNode.Attributes("slot").Value <> "cargo" Then
                                    If moduleList.ContainsKey(modNode.Attributes("slot").Value) = False Then
                                        ' Add the mod/ammo to the slot
                                        If fModule.IsCharge = True Then
                                            moduleList.Add(modNode.Attributes("slot").Value, ", " & fModule.Name)
                                        Else
                                            If fModule.IsDrone = True Then
                                                If droneList.ContainsKey(fModule.Name) = False Then
                                                    droneList.Add(fModule.Name, CInt(modNode.Attributes("qty").Value))
                                                Else
                                                    droneList(fModule.Name) = CInt(droneList(fModule.Name)) + CInt(modNode.Attributes("qty").Value)
                                                End If
                                            Else
                                                moduleList.Add(modNode.Attributes("slot").Value, fModule.Name)
                                            End If
                                        End If
                                    Else
                                        If fModule.IsCharge = True Then
                                            moduleList(modNode.Attributes("slot").Value) = CStr(moduleList(modNode.Attributes("slot").Value)) & ", " & fModule.Name
                                        Else
                                            If fModule.IsDrone = True Then
                                                If droneList.ContainsKey(fModule.Name) = False Then
                                                    droneList.Add(fModule.Name, CInt(modNode.Attributes("qty").Value))
                                                Else
                                                    droneList(fModule.Name) = CInt(droneList(fModule.Name)) + CInt(modNode.Attributes("qty").Value)
                                                End If
                                            Else
                                                moduleList(modNode.Attributes("slot").Value) = fModule.Name & ", " & CStr(moduleList(modNode.Attributes("slot").Value))
                                            End If
                                        End If
                                    End If
                                Else
                                    If cargoList.ContainsKey(fModule.Name) = False Then
                                        cargoList.Add(fModule.Name, CInt(modNode.Attributes("qty").Value))
                                    Else
                                        cargoList(fModule.Name) = CInt(cargoList(fModule.Name)) + CInt(modNode.Attributes("qty").Value)
                                    End If
                                End If
                            Else
                                moduleList.Add(modNode.Attributes("slot").Value.TrimEnd("0".ToCharArray) & subCount.ToString, fModule.Name)
                                subCount += 1
                            End If
                        End If
                    Next
                    _currentFit = New ArrayList
                    Call ClearShipSlots()
                    For Each fittedMod As String In moduleList.Values
                        _currentFit.Add(fittedMod)
                    Next
                    For Each fittedDrone As String In droneList.Keys
                        _currentFit.Add(fittedDrone & " x" & CStr(droneList(fittedDrone)))
                    Next
                    For Each cargoItem As String In cargoList.Keys
                        _currentFit.Add(cargoItem & " x" & CStr(cargoList(cargoItem)))
                    Next
                    _currentFitName = fitName
                    _currentFitting = Fittings.ConvertOldFitToNewFit(shipName & ", " & fitName, _currentFit)
                    _currentFitting.PilotName = cboPilots.SelectedItem.ToString
                    _currentFitting.UpdateBaseShipFromFitting()
                    _currentShip = _currentFitting.BaseShip
                    ' Generate fitting data
                    Call GenerateFittingData()
                    gpStatistics.Visible = True
                    btnImport.Enabled = True
                    Call UpdateSlotColumns()
                    Call UpdateSlotLayout()
                    Exit For
                End If
            Next
        End Sub

#End Region

    End Class
End NameSpace