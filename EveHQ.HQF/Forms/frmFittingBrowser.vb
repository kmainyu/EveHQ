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

Public Class frmFittingBrowser

    Dim currentShip As Ship
    Dim currentFit As New ArrayList
    Dim currentFitting As Fitting
    Dim cDNAFit As DNAFitting
    Dim SourceURL As String = ""
    Dim LoadoutName As String = ""
    Dim LoadoutID As String = ""

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add the current list of pilots to the combobox
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each cPilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
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
        For Each newProfile As HQFDamageProfile In HQFDamageProfiles.ProfileList.Values
            cboProfiles.Items.Add(newProfile.Name)
        Next
        cboProfiles.EndUpdate()
        ' Select the default profile
        cboProfiles.SelectedItem = "<Omni-Damage>"

    End Sub

    Public Property DNAFit() As DNAFitting
        Get
            Return cDNAFit
        End Get
        Set(ByVal value As DNAFitting)
            cDNAFit = value
            currentShip = CType(ShipLists.shipList(ShipLists.shipListKeyID(value.ShipID)), Ship).Clone
            pbShip.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(currentShip.ID)
            lblShipType.Text = currentShip.Name
            Call Me.UseDNAFitting()
            lblLoadoutName.Text = LoadoutName
        End Set
    End Property

#Region "Ship Fitting routines"

    Private Sub UpdateSlotColumns()
        ' Clear the columns
        lvwSlots.Columns.Clear()
        ' Add the module name column
        lvwSlots.Columns.Add("colName", "Module Name", 175, HorizontalAlignment.Left, "")
        lvwSlots.Columns.Add("Charge", "Charge Name", 175, HorizontalAlignment.Left, "")
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
            If shipMod.LoadedCharge IsNot Nothing Then
                slotName.SubItems.Add(shipMod.LoadedCharge.Name)
            Else
                slotName.SubItems.Add("")
            End If
        Else
            slotName.Text = "<Empty>"
            slotName.SubItems.Add("")
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
            currentShip.SlotCollection.Clear()
            currentShip.FleetSlotCollection.Clear()
            currentShip.DroneBayItems.Clear()
            currentShip.DroneBayUsed = 0
            currentShip.CargoBayItems.Clear()
            currentShip.CargoBayUsed = 0
        End If
    End Sub
    Private Sub GenerateFittingData()
        ' Let's try and generate a fitting and get some damage info
        If currentShip IsNot Nothing Then
            If cboPilots.SelectedItem IsNot Nothing And cboProfiles.SelectedItem IsNot Nothing Then
                If FittingPilots.HQFPilots.ContainsKey(cboPilots.SelectedItem.ToString) Then

                    Dim loadoutPilot As FittingPilot = FittingPilots.HQFPilots(cboPilots.SelectedItem.ToString)
                    Dim loadoutProfile As HQFDamageProfile = HQFDamageProfiles.ProfileList(cboProfiles.SelectedItem.ToString)

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
                        lblCapacitor.Text = "Lasts " & Core.SkillFunctions.TimeToString(csr.TimeToDrain, False)
                    End If
                    lblVelocity.Text = loadoutShip.MaxVelocity.ToString("N2") & " m/s"
                    lblMaxRange.Text = loadoutShip.MaxTargetRange.ToString("N0") & "m"
                    Dim CPU As Double = loadoutShip.CpuUsed / loadoutShip.CPU * 100
                    lblCPU.Text = CPU.ToString("N2") & "%"
                    If CPU > 100 Then
                        lblCPU.ForeColor = Color.Red
                    Else
                        lblCPU.ForeColor = Color.Black
                    End If
                    Dim PG As Double = loadoutShip.PgUsed / loadoutShip.PG * 100
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
            End If
        End If
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        Call GenerateFittingData()
    End Sub

    Private Sub cboProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProfiles.SelectedIndexChanged
        Call GenerateFittingData()
    End Sub

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Dim shipName As String = lblShipType.Text
        Dim fittingName As String = LoadoutName
        ' If the fitting exists, add a number onto the end
        If fittingName.Trim <> "" Then
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
            MessageBox.Show("Cannot import a fitting without a valid fitting name.", "Fitting Name Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
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

#Region "UI Routines"

    Private Sub lblLoadoutTopic_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblLoadoutTopic.LinkClicked
        Try
            Process.Start(SourceURL)
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please check your browser settings.", "Error Starting Web Browser", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub lblLoadoutTopic_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblLoadoutTopic.MouseEnter
        lblTopicAddress.Text = SourceURL
    End Sub

    Private Sub lblLoadoutTopic_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblLoadoutTopic.MouseLeave
        lblTopicAddress.Text = ""
    End Sub

#End Region

#Region "DNA Fitting Routines"
    Private Sub UseDNAFitting()
        Dim BaseFit As String = ""
        Dim RevisedFit As String = ""
        currentFit.Clear()
        For Each fittedMod As String In cDNAFit.Modules
            Dim fModule As ShipModule = CType(ModuleLists.moduleList(fittedMod), ShipModule)
            If fModule IsNot Nothing Then
                BaseFit = fModule.Name : RevisedFit = BaseFit
                If fModule.Charges.Count <> 0 Then
                    For Each ammo As String In cDNAFit.Charges
                        If ModuleLists.moduleList.ContainsKey(ammo) = True Then
                            If fModule.Charges.Contains(CType(ModuleLists.moduleList(ammo), ShipModule).DatabaseGroup) Then
                                RevisedFit = BaseFit & "," & CType(ModuleLists.moduleList(ammo), ShipModule).Name
                            End If
                        End If
                    Next
                    currentFit.Add(RevisedFit)
                Else
                    currentFit.Add(fModule.Name)
                End If
            End If
        Next
        Call Me.ReorderModules()
        lblLoadoutName.Visible = True
        lblLoadoutTopic.Text = "Source Link"
        btnImport.Enabled = True
        Dim shipName As String = lblShipType.Text
        Dim fittingName As String = lblLoadoutName.Text
        currentFitting = Fittings.ConvertOldFitToNewFit(shipName & ", " & fittingName, currentFit)
        currentFitting.PilotName = cboPilots.SelectedItem.ToString
        currentFitting.UpdateBaseShipFromFitting()
        currentShip = currentFitting.BaseShip
        ' Generate fitting data
        Call Me.GenerateFittingData()
        gpStatistics.Visible = True
        Call UpdateSlotColumns()
        Call UpdateSlotLayout()
        ' Get SourceURL if available
        If cDNAFit.Arguments.ContainsKey("sourceURL") = True Then
            SourceURL = cDNAFit.Arguments("sourceURL")
            ' Try to get fitting name
            If SourceURL.Contains("eve.battleclinic.com") = True Then
                LoadoutName = SourceURL.TrimStart("http://eve.battleclinic.com/loadout/".ToCharArray).TrimEnd(".html".ToCharArray)
                LoadoutID = LoadoutName.Substring(0, LoadoutName.IndexOf("-".ToCharArray))
                LoadoutName = LoadoutName.TrimStart((LoadoutID & "-").ToCharArray)
                LoadoutName = LoadoutName.Replace(" - ", "######")
                LoadoutName = LoadoutName.Replace("-", " ")
                LoadoutName = LoadoutName.Replace("######", " - ")
            Else
                LoadoutName = "Unknown Fitting"
            End If
            lblLoadoutTopic.Visible = True
            LblLoadoutTopicLbl.Visible = True
        Else
            If cDNAFit.Arguments.ContainsKey("LoadoutName") = True Then
                LoadoutName = cDNAFit.Arguments("LoadoutName")
            End If
            lblLoadoutTopic.Visible = False
            LblLoadoutTopicLbl.Visible = False
        End If
    End Sub

#End Region

End Class

