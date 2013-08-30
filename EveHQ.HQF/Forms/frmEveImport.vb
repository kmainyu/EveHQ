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
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.IO
Imports DevComponents.AdvTree

Public Class frmEveImport

	Dim EveFolder As String = Path.Combine(Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Eve"), "fittings")
	Dim currentShip As Ship
	Dim currentFit As New ArrayList
	Dim currentFitName As String = ""
	Dim currentFitting As Fitting

	Public Sub New()

		' This call is required by the Windows Form Designer.
		InitializeComponent()

		' Add the current list of pilots to the combobox
		cboPilots.BeginUpdate()
		cboPilots.Items.Clear()
		For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHqSettings.Pilots
			If cPilot.Active = True Then
				cboPilots.Items.Add(cPilot.Name)
			End If
		Next
		cboPilots.EndUpdate()
		' Look at the settings for default pilot
		If cboPilots.Items.Count > 0 Then
			If cboPilots.Items.Contains(HQF.Settings.HQFSettings.DefaultPilot) = True Then
				cboPilots.SelectedItem = HQF.Settings.HQFSettings.DefaultPilot
			Else
				cboPilots.SelectedIndex = 0
			End If
		End If

		' Add the profiles
		cboProfiles.BeginUpdate()
		cboProfiles.Items.Clear()
		For Each newProfile As DamageProfile In DamageProfiles.ProfileList.Values
			cboProfiles.Items.Add(newProfile.Name)
		Next
		cboProfiles.EndUpdate()
		' Select the default profile
		cboProfiles.SelectedItem = "<Omni-Damage>"

	End Sub

	Private Sub frmEveImport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
		If currentShip IsNot Nothing Then
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
			' Produce sub slots
			For slot As Integer = 1 To currentShip.SubSlots
				Dim newSlot As New ListViewItem
				newSlot.Name = "16_" & slot
				newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.SubSlotColour))
				newSlot.ForeColor = Color.Black
				newSlot.Group = lvwSlots.Groups.Item("lvwgSubSlots")
				Call Me.AddUserColumns(currentShip.SubSlot(slot), newSlot)
				lvwSlots.Items.Add(newSlot)
			Next
			lvwSlots.EndUpdate()
		End If
	End Sub
	Private Sub AddUserColumns(ByVal shipMod As ShipModule, ByVal slotName As ListViewItem)
		' Add subitems based on the user selected columns
		If shipMod IsNot Nothing Then
			Dim colName As String = ""
			' Add in the module name
			slotName.Text = shipMod.Name
		Else
			slotName.Text = "<Empty>"
		End If
	End Sub
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
			For slot As Integer = 1 To currentShip.SubSlots
				currentShip.SubSlot(slot) = Nothing
			Next
			currentShip.DroneBayItems.Clear()
			currentShip.DroneBay_Used = 0
			currentShip.CargoBayItems.Clear()
			currentShip.CargoBay_Used = 0
		End If
	End Sub
	Private Sub GenerateFittingData()
		' Let's try and generate a fitting and get some damage info
		If currentShip IsNot Nothing Then
			Dim loadoutPilot As HQF.HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem.ToString), HQFPilot)
			Dim loadoutProfile As HQF.DamageProfile = CType(HQF.DamageProfiles.ProfileList(cboProfiles.SelectedItem.ToString), DamageProfile)

			currentFitting.PilotName = loadoutPilot.PilotName
			currentFitting.BaseShip.DamageProfile = loadoutProfile
			currentFitting.ApplyFitting()
			Dim loadoutShip As Ship = currentFitting.FittedShip

            lblEHP.Text = loadoutShip.EffectiveHP.ToString("N0")
            lblTank.Text = loadoutShip.Attributes("10062").ToString("N2") & " DPS"
            lblVolley.Text = loadoutShip.Attributes("10028").ToString("N2")
            lblDPS.Text = loadoutShip.Attributes("10029").ToString("N2")
            lblShieldResists.Text = loadoutShip.ShieldEMResist.ToString("N0") & "/" & loadoutShip.ShieldExResist.ToString("N0") & "/" & loadoutShip.ShieldKiResist.ToString("N0") & "/" & loadoutShip.ShieldThResist.ToString("N0")
            lblArmorResists.Text = loadoutShip.ArmorEMResist.ToString("N0") & "/" & loadoutShip.ArmorExResist.ToString("N0") & "/" & loadoutShip.ArmorKiResist.ToString("N0") & "/" & loadoutShip.ArmorThResist.ToString("N0")
			Dim csr As CapSimResults = Capacitor.CalculateCapStatistics(loadoutShip, False)
			If csr.CapIsDrained = False Then
                lblCapacitor.Text = "Stable at " & (csr.MinimumCap / loadoutShip.CapCapacity * 100).ToString("N0") & "%"
			Else
				lblCapacitor.Text = "Lasts " & EveHQ.Core.SkillFunctions.TimeToString(csr.TimeToDrain, False)
			End If
            lblVelocity.Text = loadoutShip.MaxVelocity.ToString("N2") & " m/s"
            lblMaxRange.Text = loadoutShip.MaxTargetRange.ToString("N0") & "m"
			Dim CPU As Double = loadoutShip.CPU_Used / loadoutShip.CPU * 100
            lblCPU.Text = CPU.ToString("N2") & "%"
			If CPU > 100 Then
				lblCPU.ForeColor = Color.Red
			Else
				lblCPU.ForeColor = Color.Black
			End If
			Dim PG As Double = loadoutShip.PG_Used / loadoutShip.PG * 100
            lblPG.Text = PG.ToString("N2") & "%"
			If PG > 100 Then
				lblPG.ForeColor = Color.Red
			Else
				lblPG.ForeColor = Color.Black
			End If

			Dim maxOpt As Double = 0
			For slot As Integer = 1 To loadoutShip.HiSlots
				Dim shipMod As ShipModule = loadoutShip.HiSlot(slot)
				If shipMod IsNot Nothing Then
					If shipMod.Attributes.ContainsKey("54") Then
						maxOpt = Math.Max(maxOpt, CDbl(shipMod.Attributes("54")))
					End If
				End If
			Next
            lblOptimalRange.Text = maxOpt.ToString("N0") & "m"

		End If

	End Sub

	Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
		Call GenerateFittingData()
	End Sub

	Private Sub cboProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProfiles.SelectedIndexChanged
		Call GenerateFittingData()
	End Sub

	Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
		If currentShip IsNot Nothing Then
			Dim shipName As String = currentShip.Name
			Dim fittingName As String = currentFitName
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
			Dim NewFit As Fitting = Fittings.ConvertOldFitToNewFit(shipName & ", " & fittingName, currentFit)
			Fittings.FittingList.Add(NewFit.KeyName, NewFit)
			HQFEvents.StartUpdateFittingList = True
		Else
			MessageBox.Show("Please select a fitting before importing.", "Fitting Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

#End Region

#Region "UI Routines"

	Private Sub mnuViewLoadout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewLoadout.Click
		Dim cLoadout As Node = adtLoadOuts.SelectedNodes(0)
		Call GetEveShipLoadout(cLoadout)
	End Sub

	Private Sub adtLoadOuts_NodeClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtLoadOuts.NodeClick
		If adtLoadOuts.SelectedNodes.Count > 0 Then
			If adtLoadOuts.SelectedNodes(0).Nodes.Count = 0 Then
				Dim cLoadout As Node = adtLoadOuts.SelectedNodes(0)
				Call GetEveShipLoadout(cLoadout)
			End If
		End If
	End Sub

	Private Sub adtLoadOuts_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtLoadOuts.SelectionChanged
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
		If My.Computer.FileSystem.DirectoryExists(EveFolder) = False Then
			MessageBox.Show("The Eve fittings folder is not present on your system and is required for this feature to work. You will need some fittings present in this folder either exported from Eve or EveHQ before proceeding.", "Fittings Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Me.Close()
			Exit Sub
		End If

		Dim files As New ArrayList
		Dim Ships As New SortedList
		Dim ShipFittings As New SortedList
		For Each fileName As String In My.Computer.FileSystem.GetFiles(EveFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.xml")
			files.Add(fileName)
		Next
		Dim fitXML As New XmlDocument
		Dim fittingList As XmlNodeList
		Dim shipNode As XmlNode
		Dim shipItem As New Node
		Dim fitItem As New Node
		Dim shipName As String = ""
		Dim shipFit As String = ""
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
					If Ships.ContainsKey(shipName) = False Then
						ShipFittings = New SortedList
						ShipFittings.Add(shipFit, filename)
						Ships.Add(shipName, ShipFittings)
					Else
						ShipFittings = CType(Ships(shipName), SortedList)
						If ShipFittings.ContainsKey(shipFit) = False Then
							ShipFittings.Add(shipFit, filename)
						End If
					End If
				Next
			Catch e As Exception
			End Try
		Next
		For Each Ship As String In Ships.Keys
			shipItem = New Node
			shipItem.Text = Ship
			adtLoadOuts.Nodes.Add(shipItem)
			ShipFittings = CType(Ships(Ship), Collections.SortedList)
			For Each fit As String In ShipFittings.Keys
				fitItem = New Node
				fitItem.Text = fit
				fitItem.Tag = CStr(ShipFittings(fit))
				shipItem.Nodes.Add(fitItem)
			Next
		Next
		adtLoadOuts.EndUpdate()
	End Sub

	Private Sub GetEveShipLoadout(ByVal cLoadout As Node)
		Dim fitName As String = cLoadout.Text
		Dim fileName As String = cLoadout.Tag.ToString
		Dim shipName As String = cLoadout.Parent.Text
		currentShip = CType(ShipLists.shipList(shipName), Ship).Clone
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
                    If ModuleLists.moduleListName.ContainsKey(modNode.Attributes("type").Value.Trim) Then
                        Dim fModule As ShipModule = CType(ModuleLists.moduleList(ModuleLists.moduleListName(modNode.Attributes("type").Value.Trim)), ShipModule)
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
				currentFit = New ArrayList
				Call ClearShipSlots()
				For Each fittedMod As String In moduleList.Values
					currentFit.Add(fittedMod)
				Next
				For Each fittedDrone As String In droneList.Keys
					currentFit.Add(fittedDrone & " x" & CStr(droneList(fittedDrone)))
                Next
                For Each cargoItem As String In cargoList.Keys
                    currentFit.Add(cargoItem & " x" & CStr(cargoList(cargoItem)))
                Next
				currentFitName = fitName
				currentFitting = Fittings.ConvertOldFitToNewFit(shipName & ", " & fitName, currentFit)
				currentFitting.PilotName = cboPilots.SelectedItem.ToString
				currentFitting.UpdateBaseShipFromFitting()
				currentShip = currentFitting.BaseShip
				' Generate fitting data
				Call Me.GenerateFittingData()
				gpStatistics.Visible = True
				btnImport.Enabled = True
				Call UpdateSlotColumns()
				Call UpdateSlotLayout()
				Exit For
			End If
		Next
    End Sub

	Private Sub ReorderModules()
		Dim subs, mods As New ArrayList
		For Each cMod As String In currentFit
			If ModuleLists.moduleListName.ContainsKey(cMod) = True Then
				If CType(ModuleLists.moduleList(ModuleLists.moduleListName(cMod)), ShipModule).SlotType = 16 Then
					subs.Add(cMod)
				Else
					mods.Add(cMod)
				End If
			Else
				mods.Add(cMod)
			End If
		Next
		' Recreate the current fit
		currentFit.Clear()
		For Each cmod As String In subs
			currentFit.Add(cmod)
		Next
		For Each cmod As String In mods
			currentFit.Add(cmod)
		Next
		subs.Clear()
		mods.Clear()
	End Sub

#End Region

End Class