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
Imports System.Xml
Imports System.Text
Imports DevComponents.AdvTree

Public Class frmBPCalculator

#Region "Class Variables"
    Dim BPPilot As EveHQ.Core.Pilot
    Dim UpdateBPInfo As Boolean = False
    Dim StartUp As Boolean = False
    Dim InventionStartUp As Boolean = True
	Dim CurrentBP As New BlueprintSelection
	Dim CurrentInventionBP As New BlueprintSelection
	Dim OwnedBP As BPAssetComboboxItem
    Dim currentJob As New ProductionJob
    Dim ProductionArray As AssemblyArray
    Dim CopyTimeMod As Double = 1.0

    Dim StartMode As BPCalcStartMode = BPCalcStartMode.None
    Dim InitialJob As ProductionJob = Nothing

    ' Invention Specific Variables
    Dim InventionBPID As Integer = 0
    Dim InventionBaseChance As Double = 20
    Dim InventionSkill1 As Integer = 0
    Dim InventionSkill2 As Integer = 0
    Dim InventionSkill3 As Integer = 0
    Dim InventionMetaLevel As Integer = 0
    Dim InventionMetaItemID As Integer = 0
    Dim InventionDecryptorID As Integer = 0
    Dim InventionDecryptorMod As Double = 1
    Dim InventionDecryptorName As String = ""
    Dim InventionChance As Double = 20
    Dim InventedBP As New BlueprintSelection
    Dim InventionAttempts As Double = 0
    Dim InventionSuccessCost As Double = 0

#End Region

#Region "Old Property Variables"
    Dim cBPName As String = ""
    Dim cBPOwnerName As String = ""
    Dim cUsingOwnedBPs As Boolean = False
    Dim cOwnedBPID As String = ""
#End Region

#Region "Internal Properties"
    Dim cProductionChanged As Boolean = False
    Private Property ProductionChanged() As Boolean
        Get
            Return cProductionChanged
        End Get
        Set(ByVal value As Boolean)
            cProductionChanged = value
            If Me.InitialJob IsNot Nothing Then
                btnSaveProductionJob.Enabled = value
            End If
        End Set
    End Property
#End Region

#Region "Constructors"

    Public Sub New(ByVal UsingOwnedBPs As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' This is for a default blank BPCalc
        cUsingOwnedBPs = UsingOwnedBPs
        StartMode = BPCalcStartMode.None
        cBPOwnerName = Settings.PrismSettings.DefaultBPOwner

    End Sub

    Public Sub New(ByVal BPName As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' This is for a non-owned BP
        cBPName = BPName
        cUsingOwnedBPs = False
        cBPOwnerName = Settings.PrismSettings.DefaultBPOwner
        StartMode = BPCalcStartMode.StandardBP

    End Sub

    Public Sub New(ByVal BPOwner As String, ByVal BPAssetID As Long)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' This is for a non-owned BP
        cBPOwnerName = BPOwner
        cOwnedBPID = CStr(BPAssetID)
        cUsingOwnedBPs = True
        StartMode = BPCalcStartMode.OwnerBP

    End Sub

    Public Sub New(ByVal ExistingJob As ProductionJob, ForInvention As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        StartUp = True
        Me.InitialJob = ExistingJob
        currentJob = ExistingJob.Clone
        CurrentBP = currentJob.CurrentBP
        If ForInvention = False Then
            StartMode = BPCalcStartMode.ProductionJob
        Else
            StartMode = BPCalcStartMode.InventionJob
        End If
        cBPOwnerName = ExistingJob.BPOwner
        If CurrentBP IsNot Nothing Then
            cOwnedBPID = CStr(CurrentBP.AssetID)
        End If
        Me.Text = "BPCalc - Production Job: " & currentJob.JobName

    End Sub

#End Region

#Region "Form Loading Routines"

    Private Sub frmBPCalculator_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' Check for changed production values
        If Me.ProductionChanged = True Then
            Dim reply As DialogResult = MessageBox.Show("There are unsaved changes to this Production Job. Would you like to save these now?", "Save Job Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            Else
                If reply = Windows.Forms.DialogResult.Yes Then
                    ' Save the current job before exiting
                    Call Me.SaveCurrentProductionJob()
                End If

                ' Kill the event handlers from the PrismProductionResources controls
                RemoveHandler PPRInvention.ProductionResourcesChanged, AddressOf Me.InventionResourcesChanged
                RemoveHandler PPRProduction.ProductionResourcesChanged, AddressOf Me.ProductionResourcesChanged

				' Remove handlers for the price modification controls
				RemoveHandler PACUnitValue.PriceUpdated, AddressOf Me.CalculateInvention
                RemoveHandler PACDecryptor.PriceUpdated, AddressOf Me.CalculateInvention
                RemoveHandler PACMetaItem.PriceUpdated, AddressOf Me.CalculateInvention
				RemoveHandler PACSalesPrice.PriceUpdated, AddressOf Me.CalculateInvention

				RemoveHandler PACUnitValue.PriceUpdated, AddressOf Me.UpdateBlueprintInformation
				RemoveHandler PACSalesPrice.PriceUpdated, AddressOf Me.UpdateBlueprintInformation

            End If
        End If
    End Sub
    Private Sub frmBPCalculator_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set up the event handlers from the PrismProductionResources controls
        AddHandler PPRInvention.ProductionResourcesChanged, AddressOf Me.InventionResourcesChanged
        AddHandler PPRProduction.ProductionResourcesChanged, AddressOf Me.ProductionResourcesChanged

		' Set up handlers for the price modification controls
		AddHandler PACUnitValue.PriceUpdated, AddressOf Me.CalculateInvention
        AddHandler PACDecryptor.PriceUpdated, AddressOf Me.CalculateInvention
        AddHandler PACMetaItem.PriceUpdated, AddressOf Me.CalculateInvention
		AddHandler PACSalesPrice.PriceUpdated, AddressOf Me.CalculateInvention

		AddHandler PACUnitValue.PriceUpdated, AddressOf Me.UpdateBlueprintInformation
		AddHandler PACSalesPrice.PriceUpdated, AddressOf Me.UpdateBlueprintInformation

        ' Set the implants
        cboResearchImplant.SelectedIndex = 0
        cboMetallurgyImplant.SelectedIndex = 0
        cboScienceImplant.SelectedIndex = 0
        cboIndustryImplant.SelectedIndex = 0

        ' Add the skill levels
        cboResearchSkill.BeginUpdate()
        cboMetallurgySkill.BeginUpdate()
        cboScienceSkill.BeginUpdate()
        cboIndustrySkill.BeginUpdate()
        cboProdEffSkill.BeginUpdate()
        For idx As Integer = 0 To 5
            cboResearchSkill.Items.Add(idx.ToString)
            cboMetallurgySkill.Items.Add(idx.ToString)
            cboScienceSkill.Items.Add(idx.ToString)
            cboIndustrySkill.Items.Add(idx.ToString)
            cboProdEffSkill.Items.Add(idx.ToString)
        Next
        cboResearchSkill.EndUpdate()
        cboMetallurgySkill.EndUpdate()
        cboScienceSkill.EndUpdate()
        cboIndustrySkill.EndUpdate()
        cboProdEffSkill.EndUpdate()

        'Load the characters into the comboboxes
        cboPilot.BeginUpdate() : cboPilot.Items.Clear()
        cboOwner.BeginUpdate() : cboOwner.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilot.Items.Add(cPilot.Name)
                cboOwner.Items.Add(cPilot.Name)
                If cboOwner.Items.Contains(cPilot.Corp) = False Then
                    cboOwner.Items.Add(cPilot.Corp)
                End If
            End If
        Next
        cboPilot.Sorted = True
        cboOwner.Sorted = True
        cboPilot.EndUpdate()
        cboOwner.EndUpdate()

        If cBPOwnerName IsNot Nothing Then
            If cboOwner.Items.Contains(cBPOwnerName) = True Then
                cboOwner.SelectedItem = cBPOwnerName
            End If
        End If

        If CType(PPRProduction.cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.Items.ContainsKey(Settings.PrismSettings.DefaultBPCalcAssetOwner) = True Then
            CType(PPRProduction.cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.Items(Settings.PrismSettings.DefaultBPCalcAssetOwner).Checked = True
        End If
        If CType(PPRInvention.cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.Items.ContainsKey(Settings.PrismSettings.DefaultBPCalcAssetOwner) = True Then
            CType(PPRInvention.cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.Items(Settings.PrismSettings.DefaultBPCalcAssetOwner).Checked = True
        End If

        ' Select data depending on our startup routine
        Select Case StartMode

            Case BPCalcStartMode.None

                ' Set the Prism pilot as selected owner
                If cboPilot.Items.Contains(Settings.PrismSettings.DefaultBPCalcManufacturer) = True Then
                    cboPilot.SelectedItem = Settings.PrismSettings.DefaultBPCalcManufacturer
                Else
                    cboPilot.SelectedIndex = 0
                End If

                ' Update the list of Blueprints
                If cUsingOwnedBPs = True Then
                    chkOwnedBPOs.Checked = True
                Else
                    Call Me.DisplayAllBlueprints()
                End If

                ' Check if we have anything in the BPName Property
                If cBPName <> "" Then
                    If cboBPs.Items.Contains(cBPName) Then
                        cboBPs.SelectedItem = cBPName
                    End If
                End If

                If cUsingOwnedBPs = True Then
                    ' Check if we have anything in the OwnedBP Property
                    If OwnedBP IsNot Nothing Then
                        ' Set the details of the main Blueprint
                        cboBPs.SelectedItem = OwnedBP
                    End If
                End If


            Case BPCalcStartMode.StandardBP

                ' Set the Prism pilot as selected owner
                If cboPilot.Items.Contains(Settings.PrismSettings.DefaultBPCalcManufacturer) = True Then
                    cboPilot.SelectedItem = Settings.PrismSettings.DefaultBPCalcManufacturer
                Else
                    cboPilot.SelectedIndex = 0
                End If

                ' Update the list of Blueprints
                Call Me.DisplayAllBlueprints()

                ' Check if we have anything in the BPName Property
                If cBPName <> "" Then
                    If cboBPs.Items.Contains(cBPName) Then
                        cboBPs.SelectedItem = cBPName
                    End If
                End If

            Case BPCalcStartMode.OwnerBP

                ' Set the Prism pilot as selected owner
                If cboPilot.Items.Contains(Settings.PrismSettings.DefaultBPCalcManufacturer) = True Then
                    cboPilot.SelectedItem = Settings.PrismSettings.DefaultBPCalcManufacturer
                Else
                    cboPilot.SelectedIndex = 0
                End If

                ' Update the list of Blueprints
                chkOwnedBPOs.Checked = True

                If cUsingOwnedBPs = True Then
                    ' Check if we have anything in the OwnedBP Property
                    If OwnedBP IsNot Nothing Then
                        ' Set the details of the main Blueprint
                        cboBPs.SelectedItem = OwnedBP
                    End If
                End If

            Case BPCalcStartMode.ProductionJob

                Call Me.DisplayProductionJobDetails()
                tabBPCalcFunctions.SelectedTab = tiProduction

            Case BPCalcStartMode.InventionJob

                Call Me.DisplayProductionJobDetails()
                tabBPCalcFunctions.SelectedTab = tiInvention
        End Select

    End Sub

    Private Sub frmBPCalculator_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        StartUp = False
    End Sub

    Private Sub DisplayProductionJobDetails()
        ' Set Manufacturer
        If cboPilot.Items.Contains(currentJob.Manufacturer) Then
            cboPilot.SelectedItem = currentJob.Manufacturer
        Else
            If cboPilot.Items.Contains(Settings.PrismSettings.DefaultBPCalcManufacturer) = True Then
                cboPilot.SelectedItem = Settings.PrismSettings.DefaultBPCalcManufacturer
            Else
                cboPilot.SelectedIndex = 0
            End If
        End If

        ' Set BP values
        If currentJob.OverridingME <> "" Then
            nudMELevel.Value = CInt(currentJob.OverridingME)
        End If
        If currentJob.OverridingPE <> "" Then
            nudPELevel.Value = CInt(currentJob.OverridingPE)
        End If

        If CurrentBP IsNot Nothing Then
            If PlugInData.Blueprints.ContainsKey(CurrentBP.AssetID.ToString) Then
                ' This is a standard BP, not an owned one
                Call Me.DisplayAllBlueprints()
                cboBPs.SelectedItem = PlugInData.Blueprints(CurrentBP.AssetID.ToString).Name
            Else
                ' This is an owned BP
                chkOwnedBPOs.Checked = True
                cboBPs.SelectedItem = OwnedBP
            End If
        End If

        nudRuns.Value = currentJob.Runs

        PPRProduction.ProductionJob = currentJob
    End Sub

    Private Sub DisplayInventionDetails()

        If currentJob.InventionJob Is Nothing Then
            currentJob.InventionJob = New InventionJob
        End If

        ' Set InventionBP
        cboInventions.SelectedItem = EveHQ.Core.HQ.itemData(currentJob.InventionJob.InventedBPID.ToString).Name

        ' Set Decryptor
        If currentJob.InventionJob.DecryptorUsed IsNot Nothing Then
            cboDecryptor.SelectedItem = currentJob.InventionJob.DecryptorUsed.Name & " (" & currentJob.InventionJob.DecryptorUsed.ProbMod.ToString & "x, " & currentJob.InventionJob.DecryptorUsed.MEMod.ToString & "ME, " & currentJob.InventionJob.DecryptorUsed.PEMod.ToString & "PE, " & currentJob.InventionJob.DecryptorUsed.RunMod.ToString & "r)"
        End If

        ' Set MetaItem
        If currentJob.InventionJob.MetaItemID <> 0 Then
            cboMetaItem.Items.Add(EveHQ.Core.HQ.itemData(currentJob.InventionJob.MetaItemID.ToString).MetaLevel & ": " & EveHQ.Core.HQ.itemData(currentJob.InventionJob.MetaItemID.ToString).Name)
        End If

        ' Set Runs
        nudInventionBPCRuns.LockUpdateChecked = currentJob.InventionJob.OverrideBPCRuns
        If currentJob.InventionJob.OverrideBPCRuns = True Then
            nudInventionBPCRuns.Value = currentJob.InventionJob.BPCRuns
        End If

        ' Set Skills
        nudInventionSkill1.LockUpdateChecked = currentJob.InventionJob.OverrideEncSkill
        If currentJob.InventionJob.OverrideEncSkill = True Then
            nudInventionSkill1.Value = currentJob.InventionJob.EncryptionSkill
        End If
        nudInventionSkill2.LockUpdateChecked = currentJob.InventionJob.OverrideDCSkill1
        If currentJob.InventionJob.OverrideDCSkill1 = True Then
            nudInventionSkill2.Value = currentJob.InventionJob.DatacoreSkill1
        End If
        nudInventionSkill3.LockUpdateChecked = currentJob.InventionJob.OverrideDCSkill2
        If currentJob.InventionJob.OverrideDCSkill2 = True Then
            nudInventionSkill3.Value = currentJob.InventionJob.DatacoreSkill2
        End If

        ' Set InventionFlag
        chkInventionFlag.Checked = currentJob.HasInventionJob

        Call Me.CalculateInvention()


    End Sub

    Private Sub DisplayAllBlueprints()
        ' Load the Blueprints into the combo box
        cboBPs.BeginUpdate()
        cboBPs.Items.Clear()
        cboBPs.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboBPs.AutoCompleteSource = AutoCompleteSource.ListItems
        For Each newBP As Blueprint In PlugInData.Blueprints.Values
			If chkInventBPOs.Checked = True Then
				If btnToggleInvention.Value = True Then
					' Use T1 data
					If newBP.Inventions.Count > 0 Then
						cboBPs.Items.Add(newBP.Name)
					End If
				Else
					' Use T2 data
					If newBP.InventFrom.Count > 0 Then
						cboBPs.Items.Add(newBP.Name)
					End If
				End If
			Else
				cboBPs.Items.Add(newBP.Name)
			End If
		Next
        cboBPs.Sorted = True
        cboBPs.EndUpdate()
    End Sub

    Private Sub DisplayOwnedBlueprints()
        ' Load the Blueprints into the combo box
        cboBPs.BeginUpdate()
        cboBPs.Items.Clear()
        cboBPs.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboBPs.AutoCompleteSource = AutoCompleteSource.ListItems
        ' Fetch the ownerBPs if it exists
        Dim ownerBPs As New SortedList(Of String, BlueprintAsset)
        If PlugInData.BlueprintAssets.ContainsKey(cBPOwnerName) = True Then
            ownerBPs = PlugInData.BlueprintAssets(cBPOwnerName)
        End If
        Dim BPData As New Blueprint
        For Each BP As BlueprintAsset In ownerBPs.Values
            If BP.Runs <> 0 Then
                Dim BPACBI As New BPAssetComboboxItem(PlugInData.Blueprints(BP.TypeID).Name, BP.AssetID, BP.MELevel, BP.PELevel, BP.Runs)

                'Basic filter if inventable item filtering is on
                If chkInventBPOs.Checked = True Then
                    If btnToggleInvention.Value = True Then
                        ' Use T1 data
                        If PlugInData.Blueprints(BP.TypeID).Inventions.Count > 0 Then
                            cboBPs.Items.Add(BPACBI)
                        End If
                    Else
                        ' Use T2 data
                        If PlugInData.Blueprints(BP.TypeID).InventFrom.Count > 0 Then
                            cboBPs.Items.Add(BPACBI)
                        End If
                    End If
                Else
                    cboBPs.Items.Add(BPACBI)
                End If

                ' Check if this matches the ownedBPID
                If BPACBI.AssetID = cOwnedBPID Then
                    OwnedBP = BPACBI
                End If
            End If
        Next
		cboBPs.Sorted = True
        cboBPs.EndUpdate()
    End Sub

#End Region

#Region "Pilot & Owner Selection Routines"

    Private Sub cboOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboOwner.SelectedIndexChanged
        If cboOwner.SelectedItem IsNot Nothing Then
            cBPOwnerName = cboOwner.SelectedItem.ToString
            Call Me.DisplayOwnedBlueprints()
        End If
    End Sub

    Private Sub cboPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilot.SelectedIndexChanged
        ' Set the pilot
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilot.SelectedItem.ToString) Then
            BPPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilot.SelectedItem.ToString), Core.Pilot)
            ' Update skills and stuff
            currentJob.UpdateManufacturer(BPPilot.Name)
            Call Me.UpdatePilotSkills()
        End If
        If InventionStartUp = False Then
            Call Me.UpdateInventionUI()
        End If
    End Sub

    Private Sub chkOverrideSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideSkills.CheckedChanged

        ' Toggle the enabled status of the skill combo boxes
        cboResearchSkill.Enabled = chkOverrideSkills.Checked
        cboMetallurgySkill.Enabled = chkOverrideSkills.Checked
        cboIndustrySkill.Enabled = chkOverrideSkills.Checked
        cboProdEffSkill.Enabled = chkOverrideSkills.Checked
        cboScienceSkill.Enabled = chkOverrideSkills.Checked

        cboResearchImplant.Enabled = chkOverrideSkills.Checked
        cboMetallurgyImplant.Enabled = chkOverrideSkills.Checked
        cboIndustryImplant.Enabled = chkOverrideSkills.Checked
        cboScienceImplant.Enabled = chkOverrideSkills.Checked

        ' Determine whether to change the skills or leave the existing ones
        If chkOverrideSkills.Checked = False Then
            ' Use pilot skills
            Call Me.UpdatePilotSkills()
            ' Set change flag
            Me.ProductionChanged = True
        Else
            ' Don't do anything here at present as we shall just use the default values for the last selected pilot
        End If

    End Sub

    Private Sub UpdatePilotSkills()
        ' Delay updating the BP Info until we have completed the update to the pilot
        UpdateBPInfo = False
        ' Update Research Skill
        If BPPilot.PilotSkills.Contains("Research") = True Then
            cboResearchSkill.SelectedIndex = CType(BPPilot.PilotSkills("Research"), EveHQ.Core.PilotSkill).Level
        Else
            cboResearchSkill.SelectedIndex = 0
        End If
        ' Update Metallurgy Skill
        If BPPilot.PilotSkills.Contains("Metallurgy") = True Then
            cboMetallurgySkill.SelectedIndex = CType(BPPilot.PilotSkills("Metallurgy"), EveHQ.Core.PilotSkill).Level
        Else
            cboMetallurgySkill.SelectedIndex = 0
        End If
        ' Update Science Skill
        If BPPilot.PilotSkills.Contains("Science") = True Then
            cboScienceSkill.SelectedIndex = CType(BPPilot.PilotSkills("Science"), EveHQ.Core.PilotSkill).Level
        Else
            cboScienceSkill.SelectedIndex = 0
        End If
        ' Update Industry Skill
        If BPPilot.PilotSkills.Contains("Industry") = True Then
            cboIndustrySkill.SelectedIndex = CType(BPPilot.PilotSkills("Industry"), EveHQ.Core.PilotSkill).Level
        Else
            cboIndustrySkill.SelectedIndex = 0
        End If
        ' Update PE Skill
        If BPPilot.PilotSkills.Contains("Production Efficiency") = True Then
            cboProdEffSkill.SelectedIndex = CType(BPPilot.PilotSkills("Production Efficiency"), EveHQ.Core.PilotSkill).Level
        Else
            cboProdEffSkill.SelectedIndex = 0
        End If
        ' Allow updating again
        UpdateBPInfo = True
        If StartUp = False Then
            ' Set skills for job etc
            Dim ProdImplant As Integer = CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%")))
            ' Update the job skills
            currentJob.UpdateJobSkills(cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, ProdImplant)
            ' Update the Blueprint information
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub


#End Region

#Region "Blueprint Selection & Calculation Routines"

    Private Sub cboBPs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBPs.SelectedIndexChanged
        If cboBPs.SelectedItem IsNot Nothing Then
            If StartUp = False Then
                ' Enable the various parts
                gpPilotSkills.Enabled = True
                UpdateBPInfo = False
                If TypeOf (cboBPs.SelectedItem) Is BPAssetComboboxItem Then
                    ' This is an owner blueprint!
                    Dim selBP As BPAssetComboboxItem = CType(cboBPs.SelectedItem, BPAssetComboboxItem)
                    Dim bpID As String = EveHQ.Core.HQ.itemList(selBP.Name)
                    CurrentBP = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(bpID))
                    CurrentBP.MELevel = selBP.MELevel
                    CurrentBP.PELevel = selBP.PELevel
                    CurrentBP.Runs = selBP.Runs
                    CurrentBP.AssetID = CLng(selBP.AssetID)
                    ' Update the research boxes
                    nudMELevel.MinValue = CurrentBP.MELevel : nudMELevel.Value = CurrentBP.MELevel
                    nudPELevel.MinValue = CurrentBP.PELevel : nudPELevel.Value = CurrentBP.PELevel
                Else
                    ' This is a standard blueprint
                    Dim bpID As String = EveHQ.Core.HQ.itemList(cboBPs.SelectedItem.ToString.Trim)
                    CurrentBP = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(bpID))
                    CurrentBP.MELevel = 0
                    CurrentBP.PELevel = 0
                    CurrentBP.Runs = -1
                    CurrentBP.AssetID = CLng(bpID)
                    ' Update the research boxes
                    nudMELevel.MinValue = -10 : nudMELevel.Value = CurrentBP.MELevel
                    nudPELevel.MinValue = -10 : nudPELevel.Value = CurrentBP.PELevel
                End If
                ' Set change flag
                Me.ProductionChanged = True
            End If
            ' Check if all the invention data is present
            Call BlueprintSelection.CheckForInventionItems(CurrentBP)
            ' Disable the invention tab if we have no inventable items
            If CurrentBP.InventFrom.Count > 0 Then
                ' Populate invention data
                Call Me.UpdateInventionUI()
                tiInvention.Visible = True
            Else
                If CurrentBP.Inventions.Count > 0 Then
                    ' Populate invention data
                    Call Me.UpdateInventionUI()
                    tiInvention.Visible = True
                Else
                    tiInvention.Visible = False
                End If
            End If
            ' Update the form title
            If Me.StartMode <> BPCalcStartMode.ProductionJob Then
                Me.Text = "BPCalc - " & cboBPs.SelectedItem.ToString
            End If
            ' First get the image
            pbBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(CurrentBP.ID.ToString)
            ' Update the standard BP Info
            lblBPME.Text = CurrentBP.MELevel.ToString
            lblBPPE.Text = CurrentBP.PELevel.ToString
            lblBPRuns.Text = CurrentBP.Runs.ToString
            lblBPMaxRuns.Text = CurrentBP.MaxProdLimit.ToString("N0")
            ' Update the prices
            lblBPOMarketValue.Text = (CDbl(EveHQ.Core.HQ.BasePriceList(CurrentBP.ID.ToString)) * 0.9).ToString("N2") & " Isk"
            ' Update the limits on the Runs
            nudCopyRuns.MaxValue = CurrentBP.MaxProdLimit
            If CurrentBP.Runs = -1 Then
                nudRuns.MaxValue = 1000000
            Else
                nudRuns.MaxValue = Math.Min(CurrentBP.MaxProdLimit, CurrentBP.Runs)
            End If
            ToolTip1.SetToolTip(nudCopyRuns, "Limited to " & CurrentBP.MaxProdLimit.ToString & " runs by the Blueprint data")
            ToolTip1.SetToolTip(lblRunsPerCopy, "Limited to " & CurrentBP.MaxProdLimit.ToString & " runs by the Blueprint data")
            UpdateBPInfo = True
            ' Calculate what arrays we can use to manufacture this
            Call Me.CalculateAssemblyLocations()
            ' Calculate the remaining blueprint information
            Call Me.SetBlueprintInformation(StartUp)
        End If
    End Sub

    Private Sub UpdateInventionUI()

		InventionStartUp = True

		' Set the InventionBP based on selection
		If CurrentBP.InventFrom.Count > 0 Then
			CurrentInventionBP = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(CurrentBP.InventFrom(0)))
		Else
			CurrentInventionBP = BlueprintSelection.CopyFromBlueprint(CurrentBP)
		End If

		' Update the available inventions
		cboInventions.BeginUpdate()
		cboInventions.Items.Clear()
		For Each BPID As String In CurrentInventionBP.Inventions
			cboInventions.Items.Add(EveHQ.Core.HQ.itemData(BPID).Name)
		Next
		cboInventions.Sorted = True
		cboInventions.EndUpdate()

		Dim InventionSkills As New SortedList(Of String, Integer)

		' Update the decryptors and get skills by looking at the resources and determining the type of interface used
		Dim DecryptorGroupID As String = ""
		For Each Resource As BlueprintResource In CurrentInventionBP.Resources.Values
			If Resource.Activity = 8 = True Then
				' Add the resource to the list
				Dim IRPrice As Double = EveHQ.Core.DataFunctions.GetPrice(Resource.TypeID.ToString)
				If Resource.TypeName.EndsWith("Interface") = True Then
					Select Case Resource.TypeName.Substring(0, 1)
						Case "O"
							' Amarr
							DecryptorGroupID = "728"
							Dim SkillLevel As Integer = 0
							If BPPilot.PilotSkills.Contains("Amarr Encryption Methods") = True Then
								SkillLevel = CType(BPPilot.PilotSkills("Amarr Encryption Methods"), EveHQ.Core.PilotSkill).Level
							End If
							InventionSkills.Add("Amarr Encryption Methods", SkillLevel)
						Case "C"
							' Minmatar
							DecryptorGroupID = "729"
							Dim SkillLevel As Integer = 0
							If BPPilot.PilotSkills.Contains("Minmatar Encryption Methods") = True Then
								SkillLevel = CType(BPPilot.PilotSkills("Minmatar Encryption Methods"), EveHQ.Core.PilotSkill).Level
							End If
							InventionSkills.Add("Minmatar Encryption Methods", SkillLevel)
						Case "I"
							' Gallente
							DecryptorGroupID = "730"
							Dim SkillLevel As Integer = 0
							If BPPilot.PilotSkills.Contains("Gallente Encryption Methods") = True Then
								SkillLevel = CType(BPPilot.PilotSkills("Gallente Encryption Methods"), EveHQ.Core.PilotSkill).Level
							End If
							InventionSkills.Add("Gallente Encryption Methods", SkillLevel)
						Case "E"
							' Caldari
							DecryptorGroupID = "731"
							Dim SkillLevel As Integer = 0
							If BPPilot.PilotSkills.Contains("Caldari Encryption Methods") = True Then
								SkillLevel = CType(BPPilot.PilotSkills("Caldari Encryption Methods"), EveHQ.Core.PilotSkill).Level
							End If
							InventionSkills.Add("Caldari Encryption Methods", SkillLevel)
					End Select
					' Terminate early once we know
				ElseIf Resource.TypeName.StartsWith("Datacore") = True Then
					Dim SkillName As String = Resource.TypeName.TrimStart("Datacore - ".ToCharArray)
					Dim SkillLevel As Integer = 0
					If BPPilot.PilotSkills.Contains(SkillName) = True Then
						SkillLevel = CType(BPPilot.PilotSkills(SkillName), EveHQ.Core.PilotSkill).Level
					End If
					InventionSkills.Add(SkillName, SkillLevel)
				End If
			End If
		Next
		' Update the invention resources with this BP
		PPRInvention.InventionBP = CurrentInventionBP

		' Load the decryptors
		cboDecryptor.BeginUpdate()
		cboDecryptor.Items.Clear()
		cboDecryptor.Items.Add("<None>")
		For Each Decrypt As Decryptor In PlugInData.Decryptors.Values
			If Decrypt.GroupID = DecryptorGroupID Then
				cboDecryptor.Items.Add(Decrypt.Name & " (" & Decrypt.ProbMod.ToString & "x, " & Decrypt.MEMod.ToString & "ME, " & Decrypt.PEMod.ToString & "PE, " & Decrypt.RunMod.ToString & "r)")
			End If
		Next
		cboDecryptor.SelectedIndex = 0
		cboDecryptor.EndUpdate()

		' Display the skills - hopefully should be 3 :)
		If InventionSkills.Count = 3 Then
			lblInvSkill1.Text = "Skill 1: " & InventionSkills.Keys(0)
			lblInvSkill1.Tag = InventionSkills.Keys(0)
			nudInventionSkill1.Value = InventionSkills.Values(0)
			InventionSkill1 = InventionSkills.Values(0)
			lblInvSkill2.Text = "Skill 2: " & InventionSkills.Keys(1)
			lblInvSkill2.Tag = InventionSkills.Keys(1)
			nudInventionSkill2.Value = InventionSkills.Values(1)
			InventionSkill2 = InventionSkills.Values(1)
			lblInvSkill3.Text = "Skill 3: " & InventionSkills.Keys(2)
			lblInvSkill3.Tag = InventionSkills.Keys(2)
			nudInventionSkill3.Value = InventionSkills.Values(2)
			InventionSkill3 = InventionSkills.Values(2)
		Else
			MessageBox.Show("Ooops! Seems to be more invention skills here than what we can use in the calculation!", "Invention Skills Issue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		End If

		' Load the meta items
		cboMetaItem.BeginUpdate()
		cboMetaItem.Items.Clear()
		cboMetaItem.Items.Add("<None>")
		For Each MetaItem As String In CurrentInventionBP.InventionMetaItems.Keys
			cboMetaItem.Items.Add(EveHQ.Core.HQ.itemData(MetaItem).MetaLevel & ": " & EveHQ.Core.HQ.itemData(MetaItem).Name)
		Next
		cboMetaItem.SelectedIndex = 0
		cboMetaItem.EndUpdate()

		' Work out Base Chance
		Select Case EveHQ.Core.HQ.itemData(CurrentInventionBP.ProductID.ToString).Group
			Case 27, 419
				InventionBaseChance = 20
			Case 26, 28
				InventionBaseChance = 25
			Case 25, 420, 513
				InventionBaseChance = 30
			Case Else
				Select Case CurrentInventionBP.ID
                    Case 17477
                        InventionBaseChance = 20
                    Case 17479
                        InventionBaseChance = 25
                    Case 17481
                        InventionBaseChance = 30
					Case Else
						InventionBaseChance = 40
				End Select
		End Select
		lblBaseChance.Text = "Base Invention Chance: " & InventionBaseChance.ToString & "%"

		' Update the BPC Override Values
        nudInventionBPCRuns.MaxValue = CurrentInventionBP.MaxProdLimit

        Call Me.DisplayInventionDetails()

		If cboInventions.Items.Count > 0 Then
			If cboInventions.Items.Contains(CurrentBP.Name) Then
				cboInventions.SelectedItem = CurrentBP.Name
			Else
				cboInventions.SelectedIndex = 0
			End If
		End If

        InventionStartUp = False
        Call Me.CalculateInvention()

	End Sub

    Private Sub SetBlueprintInformation(ByVal RecalcOnly As Boolean)
        ' Recalculate the required resources
        If RecalcOnly = False Then
            Call Me.CalculateProductionResources()
        Else
            Call currentJob.RecalculateResourceRequirements()
            Call Me.DisplayProductionInformation()
        End If
    End Sub

    Private Sub ProductionResourcesChanged()
        If StartUp = False Then
            ' Set change flag
            Me.ProductionChanged = True
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub UpdateBlueprintInformation()
        If CurrentBP.Name <> "" Then
            ' Calculate and display the waste factor
            Call Me.CalculateWasteFactor()
            ' Display the research times
            Call Me.CalculateBlueprintTimes()
            ' Display production info
            Call Me.DisplayProductionInformation()
        End If
    End Sub

    Private Sub CalculateAssemblyLocations()
        Dim productID As String = CurrentBP.ProductID.ToString
        Dim product As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(productID)
        ' Load the Assembly Array Data
        cboPOSArrays.BeginUpdate()
        cboPOSArrays.Items.Clear()
        For Each newArray As AssemblyArray In PlugInData.AssemblyArrays.Values
            If newArray.AllowableCategories.Contains(product.Category) Or newArray.AllowableGroups.Contains(product.Group) Then
                If newArray.Name.EndsWith("Array") = True Then
                    cboPOSArrays.Items.Add(newArray.Name)
                End If
            End If
        Next
        cboPOSArrays.Sorted = True
        cboPOSArrays.EndUpdate()
        chkPOSProduction.Enabled = True
    End Sub

    Private Sub CalculateWasteFactor()
        Dim CurrentBPWF As Double = 0
        If CurrentBP.WasteFactor <> 0 Then
            If nudMELevel.Value < 0 Then
                ' This is for negative ME
                CurrentBPWF = ((CurrentBP.WasteFactor / 100) * (1 - nudMELevel.Value)) + (0.25 - (0.05 * cboProdEffSkill.SelectedIndex))
            Else
                ' This is for zero and positive ME
                CurrentBPWF = ((CurrentBP.WasteFactor / 100) / (1 + nudMELevel.Value)) + (0.25 - (0.05 * cboProdEffSkill.SelectedIndex))
            End If
        End If
        txtNewWasteFactor.Text = (CurrentBPWF * 100).ToString("N6") & "%"
    End Sub

    Private Sub CalculateBlueprintTimes()
        Dim MEImplant As Double = 1 - (CDbl(cboMetallurgyImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
        Dim PEImplant As Double = 1 - (CDbl(cboResearchImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
        Dim CopyImplant As Double = 1 - (CDbl(cboScienceImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
        Dim ProdImplant As Double = 1 - (CDbl(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
        Dim METime As Double = CurrentBP.ResearchMatTime * (1 - (0.05 * cboMetallurgySkill.SelectedIndex)) * MEImplant
        Dim PETime As Double = CurrentBP.ResearchProdTime * (1 - (0.05 * cboResearchSkill.SelectedIndex)) * PEImplant
        Dim CopyTime As Double = CurrentBP.ResearchCopyTime / CurrentBP.MaxProdLimit * 2 * (1 - (0.05 * cboScienceSkill.SelectedIndex)) * CopyImplant
        Dim prodTime As Double = CurrentBP.ProdTime * (1 - (0.04 * cboIndustrySkill.SelectedIndex)) * ProdImplant
        If chkResearchAtPOS.Checked = True Then
            METime *= 0.75
            PETime *= 0.75
            CopyTime *= CopyTimeMod
        End If
        ' Display the ME Time
        If nudMELevel.Value > 0 Then
            lblMETime.Text = EveHQ.Core.SkillFunctions.TimeToString(METime * (nudMELevel.Value - CurrentBP.MELevel), False)
        Else
            lblMETime.Text = EveHQ.Core.SkillFunctions.TimeToString(0, False)
        End If
        ' Display the PE Time
        If nudPELevel.Value > 0 Then
            lblPETime.Text = EveHQ.Core.SkillFunctions.TimeToString(PETime * (nudPELevel.Value - CurrentBP.PELevel), False)
        Else
            lblPETime.Text = EveHQ.Core.SkillFunctions.TimeToString(0, False)
        End If
        ' Display the Copy Time
        lblCopyTime.Text = EveHQ.Core.SkillFunctions.TimeToString(CopyTime * nudCopyRuns.Value, False)
    End Sub

    Private Sub CalculateProductionResources()

        ' Check for production array
        Dim arrayMod As Double = 1
        If chkPOSProduction.Checked = True Then
            If ProductionArray IsNot Nothing Then
                arrayMod = ProductionArray.MaterialMultiplier
            End If
        Else
            ProductionArray = Nothing
        End If

        ' Set blueprint runs
        Dim Runs As Integer = CInt(nudRuns.Value)

        ' Get resources
        Dim ProdImplant As Integer = CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%")))
        ' Save stuff for copying
        Dim OldJobName As String = ""
        If currentJob.JobName <> "" Then
            OldJobName = currentJob.JobName
        End If
        currentJob = CurrentBP.CreateProductionJob(cBPOwnerName, cboPilot.SelectedItem.ToString, cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, ProdImplant, nudMELevel.Value.ToString, nudPELevel.Value.ToString, Runs, ProductionArray, False)
        If OldJobName <> "" Then
            currentJob.JobName = OldJobName
        End If
        PPRProduction.ProductionJob = currentJob

    End Sub

#End Region

#Region "Production UI Routines"

    Private Sub chkOwnedBPOs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOwnedBPOs.CheckedChanged
        cboOwner.Enabled = chkOwnedBPOs.Checked
        If chkOwnedBPOs.Checked = True Then
            If cboOwner.SelectedItem IsNot Nothing Then
                cBPOwnerName = cboOwner.SelectedItem.ToString
            End If
            Call Me.DisplayOwnedBlueprints()
        Else
            cBPOwnerName = ""
            Call Me.DisplayAllBlueprints()
        End If
    End Sub

    Private Sub chkPOSProduction_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPOSProduction.CheckedChanged
        cboPOSArrays.Enabled = chkPOSProduction.Checked
        If UpdateBPInfo = True Then
            If chkPOSProduction.Checked = True Then
                If cboPOSArrays.SelectedItem IsNot Nothing Then
                    ProductionArray = PlugInData.AssemblyArrays(cboPOSArrays.SelectedItem.ToString)
                Else
                    ProductionArray = Nothing
                End If
            Else
                ProductionArray = Nothing
            End If
            currentJob.AssemblyArray = ProductionArray
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub cboPOSArrays_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPOSArrays.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ProductionArray = PlugInData.AssemblyArrays(cboPOSArrays.SelectedItem.ToString)
            currentJob.AssemblyArray = ProductionArray
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudMELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMELevel.ValueChanged
        If StartUp = False And UpdateBPInfo = True Then
            currentJob.OverridingME = CStr(nudMELevel.Value)
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudPELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPELevel.ValueChanged
        If StartUp = False And UpdateBPInfo = True Then
            currentJob.OverridingPE = CStr(nudPELevel.Value)
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudCopyRuns_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCopyRuns.ValueChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub chkResearchAtPOS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResearchAtPOS.CheckedChanged
        chkAdvancedLab.Enabled = chkResearchAtPOS.Checked
        If chkResearchAtPOS.Checked = True Then
            If chkAdvancedLab.Checked = True Then
                CopyTimeMod = 0.65
            Else
                CopyTimeMod = 0.75
            End If
        Else
            CopyTimeMod = 1.0
        End If
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub nudRuns_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRuns.ValueChanged
        If StartUp = False And UpdateBPInfo = True Then
            currentJob.Runs = CInt(nudRuns.Value)
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub cboResearchSkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboResearchSkill.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub cboMetallurgySkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMetallurgySkill.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub cboScienceSkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboScienceSkill.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub cboIndustrySkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboIndustrySkill.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            currentJob.UpdateJobIndSkill(cboIndustrySkill.SelectedIndex)
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub cboProdEffSkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProdEffSkill.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            currentJob.UpdateJobPESkill(cboProdEffSkill.SelectedIndex)
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub cboResearchImplant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboResearchImplant.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub cboMetallurgyImplant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMetallurgyImplant.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub cboScienceImplant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboScienceImplant.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub cboIndustryImplant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboIndustryImplant.SelectedIndexChanged
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Dim ProdImplant As Integer = CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%")))
            currentJob.UpdateJobProdImplant(ProdImplant)
            PPRProduction.ProductionJob = currentJob
            ' Set change flag
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub cboAssetSelection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If StartUp = False Then
            Call Me.CalculateProductionResources()
        End If
    End Sub

    Private Sub chkUseStandardBPCosting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If StartUp = False Then
            Call Me.CalculateProductionResources()
        End If
    End Sub

    Private Sub chkAdvancedLab_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAdvancedLab.CheckedChanged
        If chkAdvancedLab.Checked = True Then
            CopyTimeMod = 0.65
        Else
            CopyTimeMod = 0.75
        End If
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub DisplayProductionInformation()
        ' Calculate the batch size
        Dim productID As String = CurrentBP.ProductID.ToString
        Dim product As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(productID)
        lblBatchSize.Text = product.PortionSize.ToString("N0")
        lblProdQuantity.Text = (product.PortionSize * currentJob.Runs).ToString("N0")
        ' Calculate the factory costs
        Dim FactoryCosts As Double = Math.Round((Settings.PrismSettings.FactoryRunningCost / 3600 * currentJob.RunTime) + Settings.PrismSettings.FactoryInstallCost, 2)
        ' Display Build Time Information
        lblUnitBuildTime.Text = EveHQ.Core.SkillFunctions.TimeToString(currentJob.RunTime / currentJob.Runs, False)
        lblTotalBuildTime.Text = EveHQ.Core.SkillFunctions.TimeToString(currentJob.RunTime, False)
        ' Display Materials costs
        lblUnitBuildCost.Text = (currentJob.Cost / currentJob.Runs).ToString("N2") & " isk"
        lblTotalBuildCost.Text = currentJob.Cost.ToString("N2") & " isk"
        ' Display Factory costs
        lblFactoryCosts.Text = FactoryCosts.ToString("N2") & " isk"
        Dim totalCosts As Double = currentJob.Cost + FactoryCosts
        Dim unitcosts As Double = Math.Round(totalCosts / (currentJob.Runs * product.PortionSize), 2)
        Dim value As Double = EveHQ.Core.DataFunctions.GetPrice(productID)
        Dim profit As Double = value - unitcosts
        PACUnitValue.TypeID = CLng(productID)
        lblTotalCosts.Text = totalCosts.ToString("N2") & " isk"
        lblUnitCost.Text = unitcosts.ToString("N2") & " isk"
        lblUnitValue.Text = value.ToString("N2") & " isk"
        lblUnitProfit.Text = profit.ToString("N2") & " isk"
        lblProfitRate.Text = (profit / ((currentJob.RunTime / currentJob.Runs) / 3600)).ToString("N2") & " isk"
        lblProfitMargin.Text = CDbl(profit / value * 100).ToString("N2") & " %"
        lblProfitMarkup.Text = CDbl(profit / unitcosts * 100).ToString("N2") & " %"
        If profit > 0 Then
            lblUnitProfit.ForeColor = Drawing.Color.Green
            lblProfitRate.ForeColor = Drawing.Color.Green
        Else
            If profit < 0 Then
                lblUnitProfit.ForeColor = Drawing.Color.Red
                lblProfitRate.ForeColor = Drawing.Color.Red
            Else
                lblUnitProfit.ForeColor = Drawing.Color.Black
                lblProfitRate.ForeColor = Drawing.Color.Black
            End If
        End If
    End Sub

    Private Sub btnSaveProductionJob_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveProductionJob.Click
        Call Me.SaveCurrentProductionJob()
    End Sub

    Private Sub SaveCurrentProductionJob()
        If Me.InitialJob IsNot Nothing And currentJob IsNot Nothing Then
            ProductionJobs.Jobs(currentJob.JobName) = currentJob.Clone
            Me.ProductionChanged = False
            Me.InitialJob = currentJob.Clone
            PrismEvents.StartUpdateProductionJobs()
        Else
            Dim NewJobName As New frmAddProductionJob
            NewJobName.ShowDialog()
            If NewJobName.DialogResult = DialogResult.OK Then
                currentJob.JobName = NewJobName.JobName
                ProductionJobs.Jobs.Add(NewJobName.JobName, currentJob.Clone)
                Me.ProductionChanged = False
                Me.InitialJob = currentJob.Clone
                Me.Text = "BPCalc - Production Job: " & currentJob.JobName
            End If
            NewJobName.Dispose()
            PrismEvents.StartUpdateProductionJobs()
        End If
    End Sub

    Private Sub btnSaveProductionJobAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveProductionJobAs.Click
        Dim NewJobName As New frmAddProductionJob
        NewJobName.ShowDialog()
        If NewJobName.DialogResult = DialogResult.OK Then
            currentJob.JobName = NewJobName.JobName
            ProductionJobs.Jobs.Add(NewJobName.JobName, currentJob.Clone)
            Me.ProductionChanged = False
            Me.InitialJob = currentJob.Clone
            Me.Text = "BPCalc - Production Job: " & currentJob.JobName
        End If
        NewJobName.Dispose()
        PrismEvents.StartUpdateProductionJobs()
    End Sub

#End Region

#Region "Invention UI Routines"

	Private Sub chkInventBPOs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInventBPOs.CheckedChanged
		cboOwner.Enabled = chkOwnedBPOs.Checked
		btnToggleInvention.Enabled = chkInventBPOs.Checked
        If chkOwnedBPOs.Checked = True Then
            Call Me.DisplayOwnedBlueprints()
        Else
            Call Me.DisplayAllBlueprints()
        End If
	End Sub

	Private Sub btnToggleInvention_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToggleInvention.ValueChanged
		If chkOwnedBPOs.Checked = True Then
			Call Me.DisplayOwnedBlueprints()
		Else
			Call Me.DisplayAllBlueprints()
		End If
	End Sub
	
    Private Sub cboInventions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboInventions.SelectedIndexChanged
        InventionBPID = CInt(EveHQ.Core.HQ.itemList(Me.cboInventions.SelectedItem.ToString))
        If InventionStartUp = False Then
            Call Me.CalculateInvention()
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub cboDecryptor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDecryptor.SelectedIndexChanged

        If cboDecryptor.SelectedItem IsNot Nothing Then
            Dim Didx As Integer = cboDecryptor.SelectedItem.ToString.IndexOf("(")
            If Didx > 0 Then
                Dim DecryptorName As String = cboDecryptor.SelectedItem.ToString.Substring(0, Didx - 1).Trim
                If PlugInData.Decryptors.ContainsKey(DecryptorName) Then
                    InventionDecryptorName = DecryptorName
                    InventionDecryptorMod = PlugInData.Decryptors(DecryptorName).ProbMod
                    InventionDecryptorID = CInt(PlugInData.Decryptors(DecryptorName).ID)
                Else
                    InventionDecryptorName = ""
                    InventionDecryptorMod = 1
                    InventionDecryptorID = 0
                End If
            Else
                InventionDecryptorName = ""
                InventionDecryptorMod = 1
                InventionDecryptorID = 0
            End If
        End If
        PACDecryptor.TypeID = InventionDecryptorID
        If InventionStartUp = False Then
            Call Me.CalculateInvention()
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub cboMetaItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMetaItem.SelectedIndexChanged
        If cboMetaItem.SelectedItem IsNot Nothing Then
            If cboMetaItem.SelectedItem.ToString <> "<None>" Then
                InventionMetaLevel = CInt(cboMetaItem.SelectedItem.ToString.Substring(0, 1))
                InventionMetaItemID = CInt(EveHQ.Core.HQ.itemList(cboMetaItem.SelectedItem.ToString.Remove(0, 3)))
            Else
                InventionMetaLevel = 0
                InventionMetaItemID = 0
            End If
        End If
        PACMetaItem.TypeID = InventionMetaItemID
        If InventionStartUp = False Then
            Call Me.CalculateInvention()
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionBPCRuns_LockUpdateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudInventionBPCRuns.LockUpdateChanged
        If InventionStartUp = False Then
            Call Me.CalculateInvention()
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionBPCRuns_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudInventionBPCRuns.ValueChanged
        If InventionStartUp = False Then
            Call Me.CalculateInvention()
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionBPCRuns_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudInventionBPCRuns.ButtonCustomClick
        ' Set max runs
        nudInventionBPCRuns.Value = CurrentInventionBP.MaxProdLimit
    End Sub

    Private Sub nudInventionBPCRuns_ButtonCustom2Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudInventionBPCRuns.ButtonCustom2Click
        ' Set single run
        nudInventionBPCRuns.Value = 1
    End Sub

    Private Sub lblFactoryCostsLbl_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblFactoryCostsLbl.LinkClicked
        Dim NewSettingsForm As New frmPrismSettings
        NewSettingsForm.Tag = "nodeCosts"
        NewSettingsForm.ShowDialog()
        Call Me.UpdateBlueprintInformation()
        Call Me.CalculateInvention()
        NewSettingsForm.Dispose()
    End Sub

    Private Sub lblInventionLabCostsLbl_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblInventionLabCostsLbl.LinkClicked
        Dim NewSettingsForm As New frmPrismSettings
        NewSettingsForm.Tag = "nodeCosts"
        NewSettingsForm.ShowDialog()
        Call Me.UpdateBlueprintInformation()
        Call Me.CalculateInvention()
        NewSettingsForm.Dispose()
    End Sub

    Private Sub lblInventionBPCCostLbl_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblInventionBPCCostLbl.LinkClicked
        Dim NewSettingsForm As New frmPrismSettings
        NewSettingsForm.Tag = "nodeCosts"
        NewSettingsForm.ShowDialog()
        Call Me.UpdateBlueprintInformation()
        Call Me.CalculateInvention()
        NewSettingsForm.Dispose()
    End Sub

    Private Sub lblInventionBPCCost_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblInventionBPCCost.LinkClicked
        Dim PriceForm As New frmAddBPCPrice(CurrentBP.ID.ToString)
        PriceForm.ShowDialog()
        Call Me.UpdateBlueprintInformation()
        Call Me.CalculateInvention()
        PriceForm.Dispose()
    End Sub

    Private Sub nudInventionSkill1_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudInventionSkill1.ButtonCustomClick
        If BPPilot.PilotSkills.Contains(lblInvSkill1.Tag.ToString) = True Then
            nudInventionSkill1.Value = CType(BPPilot.PilotSkills(lblInvSkill1.Tag.ToString), EveHQ.Core.PilotSkill).Level
        Else
            nudInventionSkill1.Value = 0
        End If
        currentJob.InventionJob.EncryptionSkill = nudInventionSkill1.Value
        If InventionStartUp = False Then
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionSkill1_LockUpdateChanged(sender As Object, e As System.EventArgs) Handles nudInventionSkill1.LockUpdateChanged
        currentJob.InventionJob.OverrideEncSkill = nudInventionSkill1.LockUpdateChecked
        If InventionStartUp = False Then
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionSkill1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudInventionSkill1.ValueChanged
        InventionSkill1 = nudInventionSkill1.Value
        If InventionStartUp = False Then
            Call Me.CalculateInvention()
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionSkill2_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudInventionSkill2.ButtonCustomClick
        If BPPilot.PilotSkills.Contains(lblInvSkill2.Tag.ToString) = True Then
            nudInventionSkill2.Value = CType(BPPilot.PilotSkills(lblInvSkill2.Tag.ToString), EveHQ.Core.PilotSkill).Level
        Else
            nudInventionSkill2.Value = 0
        End If
        currentJob.InventionJob.DatacoreSkill1 = nudInventionSkill2.Value
        If InventionStartUp = False Then
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionSkill2_LockUpdateChanged(sender As Object, e As System.EventArgs) Handles nudInventionSkill2.LockUpdateChanged
        currentJob.InventionJob.OverrideDCSkill1 = nudInventionSkill2.LockUpdateChecked
        If InventionStartUp = False Then
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionSkill2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudInventionSkill2.ValueChanged
        InventionSkill2 = nudInventionSkill2.Value
        If InventionStartUp = False Then
            Call Me.CalculateInvention()
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionSkill3_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudInventionSkill3.ButtonCustomClick
        If BPPilot.PilotSkills.Contains(lblInvSkill3.Tag.ToString) = True Then
            nudInventionSkill3.Value = CType(BPPilot.PilotSkills(lblInvSkill3.Tag.ToString), EveHQ.Core.PilotSkill).Level
        Else
            nudInventionSkill3.Value = 0
        End If
        currentJob.InventionJob.DatacoreSkill2 = nudInventionSkill3.Value
        If InventionStartUp = False Then
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionSkill3_LockUpdateChanged(sender As Object, e As System.EventArgs) Handles nudInventionSkill3.LockUpdateChanged
        currentJob.InventionJob.OverrideDCSkill2 = nudInventionSkill3.LockUpdateChecked
        If InventionStartUp = False Then
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub nudInventionSkill3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudInventionSkill3.ValueChanged
        If InventionStartUp = False Then
            InventionSkill3 = nudInventionSkill3.Value
            Call Me.CalculateInvention()
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub CalculateInvention()

        If InventionBPID <> 0 Then

            ' Set the Invention Job Data
            Call Me.SetInventionJobData()

            ' Calculate Invention Chance
            InventionChance = currentJob.InventionJob.CalculateInventionChance

            ' Calculate Cost
            Dim InvCost As InventionCost = currentJob.InventionJob.CalculateInventionCost

            lblInventionChance.Text = "Total Invention Chance: " & InventionChance.ToString("N2") & "%"
            lblInventionBaseCost.Text = InvCost.DatacoreCost.ToString("N2") & " Isk"
            lblInventionDecryptorCost.Text = InvCost.DecryptorCost.ToString("N2") & " Isk"
            lblInventionMetaItemCost.Text = InvCost.MetaItemCost.ToString("N2") & " Isk"
            lblInventionLabCosts.Text = InvCost.LabCost.ToString("N2") & " Isk"
            lblInventionBPCCost.Text = InvCost.BPCCost.ToString("N2") & " Isk"
            lblInventionCost.Text = InvCost.TotalCost.ToString("N2") & " Isk"

            InventionAttempts = Math.Max(Math.Round(100 / InventionChance, 4), 1)
            InventionSuccessCost = InventionAttempts * InvCost.TotalCost

            lblInventedBP.Text = "ME:" & InventedBP.MELevel.ToString & "  PE:" & InventedBP.PELevel.ToString & "  Runs: " & InventedBP.Runs.ToString("N0")
            lblInventionTime.Text = EveHQ.Core.SkillFunctions.TimeToString(CurrentInventionBP.ResearchTechTime, False)
            lblAvgAttempts.Text = "Average Attempts Until Success: " & InventionAttempts.ToString("N4")
            lblSuccessCost.Text = InventionSuccessCost.ToString("N2") & " Isk"
            PACSalesPrice.TypeID = InventedBP.ProductID

            ' Calculate and show Resources
            PPRInvention.ProductionJob = currentJob.InventionJob.ProductionJob
            Call Me.DisplayInventionProfitInfo()

        End If

    End Sub

    Private Sub SetInventionJobData()
        ' Set the relevant parts of the current job
        Dim CurrentInventionJob As InventionJob = currentJob.InventionJob
        CurrentInventionJob.OverrideBPCRuns = nudInventionBPCRuns.LockUpdateChecked
        CurrentInventionJob.BPCRuns = nudInventionBPCRuns.Value
        If nudInventionBPCRuns.LockUpdateChecked = False Then
            ' Use current BP Runs, replacing max for unlimited
            If CurrentBP.Runs = -1 Then
                ' Use max runs
                CurrentInventionJob.BPCRuns = CurrentBP.MaxProdLimit
            End If
        End If
        If PlugInData.Decryptors.ContainsKey(InventionDecryptorName) Then
            CurrentInventionJob.DecryptorUsed = PlugInData.Decryptors(InventionDecryptorName)
        Else
            CurrentInventionJob.DecryptorUsed = Nothing
        End If
        CurrentInventionJob.InventedBPID = InventionBPID
        CurrentInventionJob.EncryptionSkill = InventionSkill1
        CurrentInventionJob.DatacoreSkill1 = InventionSkill2
        CurrentInventionJob.DatacoreSkill2 = InventionSkill3
        CurrentInventionJob.MetaItemID = InventionMetaItemID
        CurrentInventionJob.MetaItemLevel = InventionMetaLevel
        CurrentInventionJob.OverrideBPCRuns = nudInventionBPCRuns.LockUpdateChecked
        CurrentInventionJob.BaseChance = InventionBaseChance
        CurrentInventionJob.OverrideEncSkill = nudInventionSkill1.LockUpdateChecked
        CurrentInventionJob.OverrideDCSkill1 = nudInventionSkill2.LockUpdateChecked
        CurrentInventionJob.OverrideDCSkill2 = nudInventionSkill3.LockUpdateChecked
        CurrentInventionJob.EncryptionSkill = nudInventionSkill1.Value
        CurrentInventionJob.DatacoreSkill1 = nudInventionSkill2.Value
        CurrentInventionJob.DatacoreSkill2 = nudInventionSkill3.Value
        InventedBP = currentJob.InventionJob.CalculateInventedBPC
        CurrentInventionJob.ProductionJob = InventedBP.CreateProductionJob(cBPOwnerName, cboPilot.SelectedItem.ToString, cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%"))), "", "", 1, ProductionArray, False)
    End Sub

    Private Sub DisplayInventionProfitInfo()
        ' Show Production Cost

        Dim BatchQty As Integer = EveHQ.Core.HQ.itemData(InventedBP.ProductID.ToString).PortionSize
        Dim FactoryCost As Double = Math.Round((Settings.PrismSettings.FactoryRunningCost / 3600 * currentJob.InventionJob.ProductionJob.RunTime) + Settings.PrismSettings.FactoryInstallCost, 2)
        Dim AvgCost As Double = (Math.Round(InventionSuccessCost / InventedBP.Runs, 2) + PPRInvention.ProductionJob.Cost + FactoryCost) / BatchQty
        Dim SalesPrice As Double = EveHQ.Core.DataFunctions.GetPrice(InventedBP.ProductID.ToString)
        Dim UnitProfit As Double = SalesPrice - AvgCost
        Dim TotalProfit As Double = UnitProfit * InventedBP.Runs * BatchQty

        lblBatchProductionCost.Text = PPRInvention.ProductionJob.Cost.ToString("N2") & " Isk"
        lblBatchTotalCost.Text = (AvgCost * BatchQty).ToString("N2") & " Isk"
        lblAvgInventionCost.Text = AvgCost.ToString("N2") & " Isk"
        lblInventionSalesPrice.Text = SalesPrice.ToString("N2") & " Isk"
        lblUnitInventionProfit.Text = UnitProfit.ToString("N2") & " Isk"
        lblTotalInventionProfit.Text = TotalProfit.ToString("N2") & " Isk"

        If UnitProfit >= 0 Then
            lblUnitInventionProfitLbl.Text = "Profit per Unit:"
            lblUnitInventionProfit.ForeColor = Drawing.Color.Green
            lblTotalInventionProfitLbl.Text = "Total Profit:"
            lblTotalInventionProfit.ForeColor = Drawing.Color.Green
        Else
            lblUnitInventionProfitLbl.Text = "Loss per Unit:"
            lblUnitInventionProfit.ForeColor = Drawing.Color.Red
            lblTotalInventionProfitLbl.Text = "Total Loss:"
            lblTotalInventionProfit.ForeColor = Drawing.Color.Red
        End If

        Call Me.DisplayProfitTable()

    End Sub

    Private Sub DisplayProfitTable()

        Dim DecryptorID As Integer = 0
        Dim DecryptorMod As Double = 0

        adtInventionProfits.BeginUpdate()
        adtInventionProfits.Nodes.Clear()

        For Each Decryptor As String In cboDecryptor.Items
            Dim Didx As Integer = Decryptor.ToString.IndexOf("(")
            If Didx > 0 Then
                Dim DecryptorName As String = Decryptor.ToString.Substring(0, Didx - 1).Trim
                If PlugInData.Decryptors.ContainsKey(DecryptorName) Then
                    DecryptorMod = PlugInData.Decryptors(DecryptorName).ProbMod
                    DecryptorID = CInt(PlugInData.Decryptors(DecryptorName).ID)
                Else
                    DecryptorMod = 1
                    DecryptorID = 0
                End If
            Else
                DecryptorMod = 1
                DecryptorID = 0
            End If

            Dim BPCRuns As Integer = CurrentBP.Runs
            If nudInventionBPCRuns.LockUpdateChecked = False Then
                ' Use current BP Runs, replacing max for unlimited
                Select Case EveHQ.Core.HQ.itemData(CurrentBP.ProductID.ToString).Category
                    Case 6
                        BPCRuns = 1
                    Case Else
                        If CurrentInventionBP.Runs = -1 Then
                            ' Use max runs
                            BPCRuns = CurrentInventionBP.MaxProdLimit
                        End If
                End Select
            Else
                BPCRuns = nudInventionBPCRuns.Value
            End If

            Dim InvCost As InventionCost = currentJob.InventionJob.CalculateInventionCost

            Dim IC As Double = Invention.CalculateInventionChance(InventionBaseChance, InventionSkill1, InventionSkill2, InventionSkill3, InventionMetaLevel, DecryptorMod)

            Dim ICost As Double = CurrentInventionBP.CalculateInventionCost(InventionMetaItemID.ToString, DecryptorID.ToString, BPCRuns)
            Dim IBP As BlueprintSelection = CurrentInventionBP.CalculateInventedBPC(InventionBPID, DecryptorID, BPCRuns)
            Dim IA As Double = Math.Max(Math.Round(100 / IC, 4), 1)
            Dim ISC As Double = IA * ICost
            Dim IJ As ProductionJob = IBP.CreateProductionJob(cBPOwnerName, cboPilot.SelectedItem.ToString, cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%"))), "", "", 1, ProductionArray, PPRInvention.BuildResources)
            Dim BatchQty As Integer = EveHQ.Core.HQ.itemData(InventedBP.ProductID.ToString).PortionSize
            Dim FactoryCost As Double = Math.Round((Settings.PrismSettings.FactoryRunningCost / 3600 * IJ.RunTime) + Settings.PrismSettings.FactoryInstallCost, 2)
            Dim AvgCost As Double = (Math.Round(ISC / IBP.Runs, 2) + IJ.Cost + FactoryCost) / BatchQty
            Dim SalesPrice As Double = EveHQ.Core.DataFunctions.GetPrice(IBP.ProductID.ToString)
            Dim UnitProfit As Double = SalesPrice - AvgCost
            Dim TotalProfit As Double = (UnitProfit * IBP.Runs) * BatchQty

            Dim NewLine As New Node
            NewLine.Text = DecryptorMod.ToString("N1") & "x (" & IBP.Runs.ToString & " runs)"
            NewLine.Cells.Add(New Cell(UnitProfit.ToString("N2") & "<br />" & TotalProfit.ToString("N2")))
            adtInventionProfits.Nodes.Add(NewLine)

        Next
        adtInventionProfits.EndUpdate()
    End Sub

    Private Sub chkInventionFlag_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkInventionFlag.CheckedChanged
        currentJob.HasInventionJob = chkInventionFlag.Checked
        If InventionStartUp = False Then
            Me.ProductionChanged = True
        End If
    End Sub

    Private Sub InventionResourcesChanged()
        If StartUp = False Then
            ' Set change flag
            Me.ProductionChanged = True
            currentJob.InventionJob.ProductionJob = PPRInvention.ProductionJob
            Call Me.DisplayInventionProfitInfo()
        End If
    End Sub

#End Region

   
End Class

Public Enum BPCalcStartMode
    None = 0
    StandardBP = 1
    OwnerBP = 2
    ProductionJob = 3
    InventionJob = 4
End Enum
